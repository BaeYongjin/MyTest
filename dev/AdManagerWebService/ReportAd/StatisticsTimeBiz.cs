// ===============================================================================
//
// StatisticsTimeBiz.cs
//
// 광고리포트 시간대별통계 서비스 
//
// ===============================================================================
// Release history
//
// ===============================================================================
// Copyright (C) 2007 G-Inno Systems Inc.
// All rights reserved.
// ==============================================================================

/*
 * -------------------------------------------------------
 * Class Name: StatisticsTimeBiz
 * 주요기능  : 광고리포트 시간대별통계 처리 로직
 * 작성자    : 모름
 * 작성일    : 모름
 * 특이사항  : 없음
 * -------------------------------------------------------
 * [수정사항]
 * -------------------------------------------------------
 * 수정코드  : [E_01]
 * 수정자    : H.J.LEE
 * 수정일    : 2014.08.19
 * 수정부분  :
 *			  - 생성자
 *            - 모든 쿼리
 * 수정내용  : 
 *            - DB 이중화 작업으로 HanaTV , Summary로 분리됨
 *            - Summary가 아닌 HanaTV를 참조하는 모든 테이블,
 *              프로시저 등을 AdTargetsHanaTV.dbo.XX로 수정
 * --------------------------------------------------------
 */

using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;

using WinFramework.Data;
using WinFramework.Misc;
using WinFramework.Base;

using AdManagerModel;

namespace AdManagerWebService.ReportAd
{
    /// <summary>
    /// StatisticsTimeBiz에 대한 요약 설명입니다.
    /// </summary>
    public class StatisticsTimeBiz : BaseBiz
    {
		#region  생성자
        public StatisticsTimeBiz() : base(FrameSystem.connSummaryDbString)
        {
            _log = FrameSystem.oLog;
        }
		#endregion
		#region 기간내 시간대별통계
        /// <summary>
        ///  기간내 시간대별통계
        /// </summary>
        /// <param name="statisticsTimeModel"></param>
        public void GetStatisticsTime(HeaderModel header, StatisticsTimeModel statisticsTimeModel)
        {
			bool isNotTarget = false; // 타겟팅정보가 입력되지않아 존재하지 않을때.
			bool isNotReady  = false; // 데이터가 집계가 되지않아 존재하지 않을때.
            try
			{
				StringBuilder sbQuery = null;

				// 데이터베이스를 OPEN한다
				_db.Open();


                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "GetStatisticsTime() Start");
                _log.Debug("-----------------------------------------");

				// 일자가 6자리 이상(yyyymmdd)이면 6자리로 만든다.
				if(statisticsTimeModel.SearchStartDay.Length > 6) statisticsTimeModel.SearchStartDay = statisticsTimeModel.SearchStartDay.Substring(2,6);
				if(statisticsTimeModel.SearchEndDay.Length   > 6) statisticsTimeModel.SearchEndDay   = statisticsTimeModel.SearchEndDay.Substring(2,6);
				
				// __DEBUG__
                _log.Debug("<입력정보>");
				_log.Debug("SearchMediaCode	  :[" + statisticsTimeModel.SearchMediaCode   + "]");		// 검색 매체
				_log.Debug("SearchContractSeq      :[" + statisticsTimeModel.SearchContractSeq      + "]");		// 검색 광고번호           
				_log.Debug("SearchItemNo      :[" + statisticsTimeModel.SearchItemNo      + "]");		// 검색 광고번호           
				_log.Debug("SearchStartDay    :[" + statisticsTimeModel.SearchStartDay    + "]");		// 검색 집계시작 일자          
				_log.Debug("SearchEndDay      :[" + statisticsTimeModel.SearchEndDay      + "]");		// 검색 집계종료 일자          
				// __DEBUG__

				string MediaCode   = statisticsTimeModel.SearchMediaCode;
                int	    ContractSeq = Convert.ToInt32( statisticsTimeModel.SearchContractSeq );
                int	    CampaignCd	= Convert.ToInt32( statisticsTimeModel.CampaignCode );
                int	    ItemNo      = Convert.ToInt32( statisticsTimeModel.SearchItemNo );
				string StartDay    = statisticsTimeModel.SearchStartDay;
				string EndDay      = statisticsTimeModel.SearchEndDay;


				// 쿼리생성
                #region [ 보장노출건수 구하기 ]
                sbQuery = new StringBuilder();
                sbQuery.Append("\n");
                sbQuery.Append(" -- 해당광고의 노출기간조회" + "\n");
                sbQuery.Append(" -- 해당광고의 계약노출(보장노출)조회" + "\n");
                sbQuery.Append(" SELECT sum(ISNULL(b.ContractAmt,0)) AS ContractAmt" + "\n");
                sbQuery.Append(" FROM AdTargetsHanaTV.dbo.ContractItem a with(nolock)" + "\n");
                sbQuery.Append(" LEFT JOIN AdTargetsHanaTV.dbo.Targeting b with(nolock)" + "\n");
                sbQuery.Append("        ON (a.ItemNo = b.ItemNo)" + "\n");
                sbQuery.Append(" WHERE 1=1" + "\n");
                #region 조회조건(계약,캠페인,내역)
                if( ItemNo > 0 )                            // 캠페인이 전체이면서 광고내역이 개별이면 개별광고임
                    sbQuery.Append("    and a.ItemNo  = " + ItemNo + "\n");
                else if( CampaignCd == 0 && ItemNo == 0 )   // 캠페인이 전체이면서 광고내역이 전체이면 전체 계약임
                    sbQuery.Append("    and a.ItemNo in(select  ItemNo from AdTargetsHanaTV.dbo.ContractItem with(noLock) where MediaCode=1 and ContractSeq = " + ContractSeq + " and AdType < '90' )");
                else if( CampaignCd > 0 && ItemNo == 0 )    // 캠페인이 선택되고, 전체광고면 해당캠패인 전체임
                    sbQuery.Append("    and a.ItemNo in( select ItemNo from AdTargetsHanaTV.dbo.CampaignItem with(noLock) where CampaignCode = " + CampaignCd + " )" + "\n");
                #endregion

                // __DEBUG__
                _log.Debug(sbQuery.ToString());
                // __DEBUG__
				
                // 쿼리실행
                DataSet ds = new DataSet();
                _db.ExecuteQuery(ds,sbQuery.ToString());

                int    ContractAmt    = 0;

                if(ds.Tables[0].Rows.Count == 0)
                {
                    isNotTarget = true;
                    throw new Exception();
                }
                ContractAmt    = Convert.ToInt32(ds.Tables[0].Rows[0]["ContractAmt"].ToString());

                ds.Dispose();

                // 해당광고의 계약노출(보장노출)수
                statisticsTimeModel.ContractAmt = ContractAmt;
                #endregion

                #region [ 누적노출건수 구하기 ]
                sbQuery = new StringBuilder();
                sbQuery.Append("\n");
                sbQuery.Append(" -- 해당광고의 계약기간내 총 누적노출수" + "\n" );
                sbQuery.Append(" SELECT ISNULL(SUM(A.AdCnt),0) AS TotalAdCnt" + "\n" );
                sbQuery.Append(" FROM   SummaryAdDaily0 A with(NoLock)" + "\n" );
                sbQuery.Append(" WHERE  A.LogDay BETWEEN '"+ StartDay + "' AND '"+ EndDay + "'" + "\n" );
                sbQuery.Append(" AND		A.SummaryType = 1" + "\n" );
                if( ItemNo > 0 )                            // 캠페인이 전체이면서 광고내역이 개별이면 개별광고임
                    sbQuery.Append("    and a.ItemNo  = " + ItemNo + "\n");
                else if( CampaignCd == 0 && ItemNo == 0 )   // 캠페인이 전체이면서 광고내역이 전체이면 전체 계약임
                    sbQuery.Append("    and a.ItemNo in(select  ItemNo from AdTargetsHanaTV.dbo.ContractItem with(noLock) where MediaCode=1 and ContractSeq = " + ContractSeq + " and AdType < '90' )");
                else if( CampaignCd > 0 && ItemNo == 0 )    // 캠페인이 선택되고, 전체광고면 해당캠패인 전체임
                    sbQuery.Append("    and a.ItemNo in( select ItemNo from AdTargetsHanaTV.dbo.CampaignItem with(noLock) where CampaignCode = " + CampaignCd + " )" + "\n");

                // __DEBUG__
                _log.Debug(sbQuery.ToString());
                // __DEBUG__
				
                // 쿼리실행
                ds = new DataSet();
                _db.ExecuteQuery(ds,sbQuery.ToString());

                if(ds.Tables[0].Rows.Count == 0)
                {
                    isNotReady = true;
                    throw new Exception();
                }

                int TotalAdCnt = Convert.ToInt32(ds.Tables[0].Rows[0]["TotalAdCnt"].ToString());
                ds.Dispose();

                // 해당광고의 계약노출(보장노출)수
                statisticsTimeModel.TotalAdCnt = TotalAdCnt;
                #endregion


                // 쿼리생성
				sbQuery = new StringBuilder();
	
				sbQuery.Append("\n"
					+ "-- 기간내 시간대별 광고 통계                \n"
                    + "                                           \n"
                    + "DECLARE @TotAdHit int;    -- 전체 광고노출수  \n"
                    + "SET @TotAdHit    = 0;                      \n"
                    + "                                           \n"
                    + "-- 전체 광고Hit                              \n"
                    + "SELECT @TotAdHit = ISNULL(SUM(A.AdCnt),0)  \n"
                    + "  FROM SummaryAdDaily0 A with(NoLock)      \n"
                    + " WHERE A.LogDay BETWEEN '"+ StartDay  + "' \n"
                    + "                    AND '"+ EndDay    + "' \n");
                if( ItemNo > 0 )                            // 캠페인이 전체이면서 광고내역이 개별이면 개별광고임
                    sbQuery.Append("    and a.ItemNo  = " + ItemNo + "\n");
                else if( CampaignCd == 0 && ItemNo == 0 )   // 캠페인이 전체이면서 광고내역이 전체이면 전체 계약임
                    sbQuery.Append("    and a.ItemNo in(select  ItemNo from AdTargetsHanaTV.dbo.ContractItem with(noLock) where MediaCode=1 and ContractSeq = " + ContractSeq + " and AdType < '90' )");
                else if( CampaignCd > 0 && ItemNo == 0 )    // 캠페인이 선택되고, 전체광고면 해당캠패인 전체임
                    sbQuery.Append("    and a.ItemNo in( select ItemNo from AdTargetsHanaTV.dbo.CampaignItem with(noLock) where CampaignCode = " + CampaignCd + " )" + "\n");
				sbQuery.Append(""
                    + "   AND A.SummaryType  = 1                  \n"
					+ "                                           \n"
					+ "--시간대 순번을 위한 임시테이블  \n"
					+ "-- 0부터 1씩 증가 24개만  \n"
					+ "SELECT IDENTITY(INT, 0, 1) AS Rownum  \n"
					+ "INTO #TempTime  \n"
					+ "FROM (SELECT TOP 24 * FROM SummaryCode) T  \n"
					+ "                                           \n"
					+ "-- 시간대별 통계                           \n"
					+ "SELECT TA.TimeOrder                        \n"
					+ "      ,TA.TimeName                         \n"
					+ "      ,ISNULL(SUM(TA.AdCnt),0) AS AdCnt                                                                                 \n"
					+ "      ,CASE WHEN @TotAdHit > 0 THEN CONVERT(DECIMAL(9,2),(ISNULL(SUM(TA.AdCnt),0) / CONVERT(float,@TotAdHit)) * 100)    \n"
					+ "                               ELSE 0 END AS PgRate                                                                     \n"
					+ "      ,REPLICATE('■', CASE WHEN @TotAdHit  > 0 THEN ROUND((ISNULL(SUM(TA.AdCnt),0)/CONVERT(float,@TotAdHit) * 100),0)  \n"
					+ "                                                ELSE 0 END) AS RateBar                                                  \n"
					+ "  FROM (                                                 \n"
					+ "        SELECT TC.RowNum AS TimeOrder                    \n"
					+ "              ,CASE TC.RowNum WHEN  0 THEN '00시~01시'   \n"
					+ "                              WHEN  1 THEN '01시~02시'   \n"
					+ "                              WHEN  2 THEN '02시~03시'   \n"
					+ "                              WHEN  3 THEN '03시~04시'   \n"
					+ "                              WHEN  4 THEN '04시~05시'   \n"
					+ "                              WHEN  5 THEN '05시~06시'   \n"
					+ "                              WHEN  6 THEN '06시~07시'   \n"
					+ "                              WHEN  7 THEN '07시~08시'   \n"
					+ "                              WHEN  8 THEN '08시~09시'   \n"
					+ "                              WHEN  9 THEN '09시~10시'   \n"
					+ "                              WHEN 10 THEN '10시~11시'   \n"
					+ "                              WHEN 11 THEN '11시~12시'   \n"
					+ "                              WHEN 12 THEN '12시~13시'   \n"
					+ "                              WHEN 13 THEN '13시~14시'   \n"
					+ "                              WHEN 14 THEN '14시~15시'   \n"
					+ "                              WHEN 15 THEN '15시~16시'   \n"
					+ "                              WHEN 16 THEN '16시~17시'   \n"
					+ "                              WHEN 17 THEN '17시~18시'   \n"
					+ "                              WHEN 18 THEN '18시~19시'   \n"
					+ "                              WHEN 19 THEN '19시~20시'   \n"
					+ "                              WHEN 20 THEN '20시~21시'   \n"
					+ "                              WHEN 21 THEN '21시~22시'   \n"
					+ "                              WHEN 22 THEN '22시~23시'   \n"
					+ "                              WHEN 23 THEN '23시~24시' END AS TimeName \n"
					+ "              ,CASE TC.RowNum WHEN  0 THEN SUM(H00)      \n"
					+ "                              WHEN  1 THEN SUM(H01)      \n"
					+ "                              WHEN  2 THEN SUM(H02)      \n"
					+ "                              WHEN  3 THEN SUM(H03)      \n"
					+ "                              WHEN  4 THEN SUM(H04)      \n"
					+ "                              WHEN  5 THEN SUM(H05)      \n"
					+ "                              WHEN  6 THEN SUM(H06)      \n"
					+ "                              WHEN  7 THEN SUM(H07)      \n"
					+ "                              WHEN  8 THEN SUM(H08)      \n"
					+ "                              WHEN  9 THEN SUM(H09)      \n"
					+ "                              WHEN 10 THEN SUM(H10)      \n"
					+ "                              WHEN 11 THEN SUM(H11)      \n"
					+ "                              WHEN 12 THEN SUM(H12)      \n"
					+ "                              WHEN 13 THEN SUM(H13)      \n"
					+ "                              WHEN 14 THEN SUM(H14)      \n"
					+ "                              WHEN 15 THEN SUM(H15)      \n"
					+ "                              WHEN 16 THEN SUM(H16)      \n"
					+ "                              WHEN 17 THEN SUM(H17)      \n"
					+ "                              WHEN 18 THEN SUM(H18)      \n"
					+ "                              WHEN 19 THEN SUM(H19)      \n"
					+ "                              WHEN 20 THEN SUM(H20)      \n"
					+ "                              WHEN 21 THEN SUM(H21)      \n"
					+ "                              WHEN 22 THEN SUM(H22)      \n"
					+ "                              WHEN 23 THEN SUM(H23) END AdCnt \n"
					+ "          FROM SummaryAdDaily0 A with(NoLock)            \n"
					+ "              ,#TempTime TC                              \n"
					+ "         WHERE A.LogDay BETWEEN '"+ StartDay    + "'     \n"
                    + "                            AND '"+ EndDay      + "'     \n");
                if( ItemNo > 0 )                            // 캠페인이 전체이면서 광고내역이 개별이면 개별광고임
                    sbQuery.Append("    and a.ItemNo  = " + ItemNo + "\n");
                else if( CampaignCd == 0 && ItemNo == 0 )   // 캠페인이 전체이면서 광고내역이 전체이면 전체 계약임
                    sbQuery.Append("    and a.ItemNo in(select  ItemNo from AdTargetsHanaTV.dbo.ContractItem with(noLock) where MediaCode=1 and ContractSeq = " + ContractSeq + " and AdType < '90' )");
                else if( CampaignCd > 0 && ItemNo == 0 )    // 캠페인이 선택되고, 전체광고면 해당캠패인 전체임
                    sbQuery.Append("    and a.ItemNo in( select ItemNo from AdTargetsHanaTV.dbo.CampaignItem where CampaignCode = " + CampaignCd + " )" + "\n");
				sbQuery.Append(""
                    + "         AND SummaryType = 1                             \n"
					+ "         GROUP BY TC.RowNum                              \n"
					+ "       ) TA                                              \n"
					+ " GROUP BY TimeOrder, TimeName                            \n"
					+ "                                                         \n"
					+ " ORDER BY TimeOrder                                      \n"
					+ "                                                         \n"
					+ " DROP Table #TempTime                                    \n"
					);
					
				// __DEBUG__
				_log.Debug(sbQuery.ToString());
				// __DEBUG__
				
				// 쿼리실행
				ds = new DataSet();
				_db.ExecuteQuery(ds,sbQuery.ToString());

				// 결과 DataSet의 데이터모델에 복사
				statisticsTimeModel.ReportDataSet = ds.Copy();

				// 결과
				statisticsTimeModel.ResultCnt = Utility.GetDatasetCount(statisticsTimeModel.ReportDataSet);

				ds.Dispose();

				// 결과코드 셋트
				statisticsTimeModel.ResultCD = "0000";

                // __DEBUG__
                _log.Debug("<출력정보>");
                _log.Debug("ResultCnt:[" + statisticsTimeModel.ResultCnt + "]");
                // __DEBUG__

                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "GetStatisticsTime() End");
                _log.Debug("-----------------------------------------");
            }
            catch(Exception ex)
            {
                statisticsTimeModel.ResultCD = "3000";
				if(isNotTarget)
				{
					statisticsTimeModel.ResultDesc = "해당광고의 정보가 존재하지 않습니다.";
				}
				else if(isNotReady)
				{
					statisticsTimeModel.ResultDesc = "해당 기간은 데이터가 집계되지 않았습니다.";
				}
				else
				{
					statisticsTimeModel.ResultDesc = "시간대별통계 조회중 오류가 발생하였습니다";
					_log.Exception(ex);
				}
            }
			finally
			{
				// 데이터베이스를  Close한다
				_db.Close();
			}

        }
		#endregion
    }
}