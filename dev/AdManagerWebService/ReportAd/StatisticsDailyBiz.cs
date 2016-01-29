// ===============================================================================
//
// StatisticsDailyBiz.cs
//
// 광고리포트 일별통계 서비스 
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
 * Class Name: StatisticsDailyBiz
 * 주요기능  : 광고리포트 처리 로직
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
    /// StatisticsDailyBiz에 대한 요약 설명입니다.
    /// </summary>
    public class StatisticsDailyBiz : BaseBiz
    {

	    #region  생성자
        public StatisticsDailyBiz() : base(FrameSystem.connSummaryDbString)
        {
            _log = FrameSystem.oLog;
        }
		#endregion

		#region 기간내 일별통계
        /// <summary>
        ///  기간내 일별통계
        /// </summary>
        /// <param name="statisticsDailyModel"></param>
		public void GetStatisticsDaily(HeaderModel header, StatisticsDailyModel statisticsDailyModel)
        {
			bool isNotTarget = false; // 타겟팅정보가 입력되지않아 존재하지 않을때.
			bool isNotReady  = false; // 데이터가 집계가 되지않아 존재하지 않을때.
         
			try
			{
				StringBuilder sbQuery = null;
				// 데이터베이스를 OPEN한다
				_db.Open();

                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "GetStatisticsDaily() Start");
                _log.Debug("-----------------------------------------");

				// 일자가 6자리 이상(yyyymmdd)이면 6자리로 만든다.
				if(statisticsDailyModel.SearchStartDay.Length > 6) statisticsDailyModel.SearchStartDay = statisticsDailyModel.SearchStartDay.Substring(2,6);
				if(statisticsDailyModel.SearchEndDay.Length   > 6) statisticsDailyModel.SearchEndDay   = statisticsDailyModel.SearchEndDay.Substring(2,6);
				
				// __DEBUG__
                _log.Debug("<입력정보>");
				_log.Debug("SearchMediaCode	:[" + statisticsDailyModel.SearchMediaCode   + "]");		// 검색 매체
				_log.Debug("SearchContractSeq :[" + statisticsDailyModel.SearchContractSeq + "]");		// 검색 광고번호           
				_log.Debug("SearchItemNo      :[" + statisticsDailyModel.SearchItemNo      + "]");		// 검색 광고번호           
				_log.Debug("SearchStartDay    :[" + statisticsDailyModel.SearchStartDay    + "]");		// 검색 집계시작 일자          
				_log.Debug("SearchEndDay      :[" + statisticsDailyModel.SearchEndDay      + "]");		// 검색 집계종료 일자          
				// __DEBUG__

				string  MediaCode   = statisticsDailyModel.SearchMediaCode;
				int	    ContractSeq = Convert.ToInt32( statisticsDailyModel.SearchContractSeq );
				int	    CampaignCd	= Convert.ToInt32( statisticsDailyModel.CampaignCode );
				int	    ItemNo      = Convert.ToInt32( statisticsDailyModel.SearchItemNo );
				string  StartDay    = statisticsDailyModel.SearchStartDay;
				string  EndDay      = statisticsDailyModel.SearchEndDay;

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
                    sbQuery.Append("    and a.ItemNo in( select ItemNo from AdTargetsHanaTV.dbo.CampaignItem where CampaignCode = " + CampaignCd + " )" + "\n");
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
				statisticsDailyModel.ContractAmt = ContractAmt;
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
                    sbQuery.Append("    and a.ItemNo in( select ItemNo from AdTargetsHanaTV.dbo.CampaignItem where CampaignCode = " + CampaignCd + " )" + "\n");

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
				statisticsDailyModel.TotalAdCnt = TotalAdCnt;
				#endregion

                // 쿼리생성
				sbQuery = new StringBuilder();
				sbQuery.Append("\n"
					+ "/* 기간내 일별 광고 통계    */                 \n"
                    + "DECLARE @TotAdHit int;    -- 전체 광고노출수  \n"
                    + "SET @TotAdHit    = 0;                      \n"
                    + "                                           \n"
                    + "-- 전체 광고Hit                              \n"
                    + "SELECT @TotAdHit = ISNULL(SUM(A.AdCnt),0)  \n"
                    + "  FROM SummaryAdDaily0 A with(NoLock)      \n"
					+ " WHERE A.LogDay BETWEEN '"+ StartDay  + "' \n"
					+ "                    AND '"+ EndDay    + "' \n");
				#region 조회조건(계약,캠페인,내역)
				if( ItemNo > 0 )                            // 캠페인이 전체이면서 광고내역이 개별이면 개별광고임
					sbQuery.Append("    and a.ItemNo  = " + ItemNo + "\n");
				else if( CampaignCd == 0 && ItemNo == 0 )   // 캠페인이 전체이면서 광고내역이 전체이면 전체 계약임
                    sbQuery.Append("    and a.ItemNo in(select  ItemNo from AdTargetsHanaTV.dbo.ContractItem with(noLock) where MediaCode=1 and ContractSeq = " + ContractSeq + " and AdType < '90' )");
				else if( CampaignCd > 0 && ItemNo == 0 )    // 캠페인이 선택되고, 전체광고면 해당캠패인 전체임
                    sbQuery.Append("    and a.ItemNo in( select ItemNo from AdTargetsHanaTV.dbo.CampaignItem where CampaignCode = " + CampaignCd + " )" + "\n");
				#endregion
				sbQuery.Append(""
                    + "   AND A.SummaryType  = 1                  \n"
                    + "                                           \n"
                    + "-- 일별 통계                                 \n"
                    + "SELECT CONVERT(CHAR(10), CONVERT(datetime, '20' + T.LogDay, 112),120) \n"
                    + "       + CASE T.week WHEN 1 THEN ' (일)'            \n"
                    + "                     WHEN 2 THEN ' (월)'            \n"
                    + "                     WHEN 3 THEN ' (화)'            \n"
                    + "                     WHEN 4 THEN ' (수)'            \n"
                    + "                     WHEN 5 THEN ' (목)'            \n"
                    + "                     WHEN 6 THEN ' (금)'            \n"
                    + "                     WHEN 7 THEN ' (토)'            \n"
                    + "                     ELSE '' END AS LogDay          \n"
                    + "      , T.week AS Week                              \n"
                    + "      ,ISNULL(cnt.AdCnt,0) AS AdCnt                 \n"
                    + "      ,CASE WHEN @TotAdHit > 0 THEN CONVERT(DECIMAL(9,2),(ISNULL(cnt.AdCnt,0) / CONVERT(float,@TotAdHit)) * 100) \n"
					+ "                               ELSE 0 END AS AdRate  \n"
					+ "      ,REPLICATE('■', CASE WHEN @TotAdHit  > 0 THEN ROUND((ISNULL(cnt.AdCnt,0)/CONVERT(float,@TotAdHit) * 100),0) \n"
                    + "                                                ELSE 0 END) AS RateBar   \n"
                    + " FROM                                                \n"
                    + "    (                                                \n"
                    + "      SELECT bs.LogDay                               \n"
					+ "            ,bs.week                                 \n"
                    + "        FROM SummaryBase bs  with(NoLock)            \n"
                    + "        WHERE bs.LogDay BETWEEN '"+ StartDay    + "' \n"
                    + "                            AND '"+ EndDay      + "' \n"
                    + "    ) T                                              \n"
                    + "    LEFT  JOIN                                       \n"
                    + "    ( SELECT A.LogDay, ISNULL(SUM(A.AdCnt),0) AS AdCnt \n"
                    + "        FROM SummaryAdDaily0 A  with(NoLock)           \n"
					+ "       WHERE A.LogDay BETWEEN '"+ StartDay        + "' \n"
					+ "                          AND '"+ EndDay          + "' \n");
				#region 조회조건(계약,캠페인,내역)
				if( ItemNo > 0 )                            // 캠페인이 전체이면서 광고내역이 개별이면 개별광고임
					sbQuery.Append("    and a.ItemNo  = " + ItemNo + "\n");
				else if( CampaignCd == 0 && ItemNo == 0 )   // 캠페인이 전체이면서 광고내역이 전체이면 전체 계약임
                    sbQuery.Append("    and a.ItemNo in(select  ItemNo from AdTargetsHanaTV.dbo.ContractItem with(noLock) where MediaCode=1 and ContractSeq = " + ContractSeq + " and AdType < '90' )");
				else if( CampaignCd > 0 && ItemNo == 0 )    // 캠페인이 선택되고, 전체광고면 해당캠패인 전체임
                    sbQuery.Append("    and a.ItemNo in( select ItemNo from AdTargetsHanaTV.dbo.CampaignItem where CampaignCode = " + CampaignCd + " )" + "\n");
				#endregion
				sbQuery.Append(""
                    + "         AND SummaryType = 1                           \n"
                    + "       GROUP BY A.LogDay                               \n"
                    + "    ) cnt                                              \n"
					+ "    ON (T.LogDay = cnt.LogDay)                         \n"
                    + " ORDER BY T.LogDay                                     \n"
					); 


				// __DEBUG__
				_log.Debug(sbQuery.ToString());
				// __DEBUG__
				
				// 쿼리실행
				ds = new DataSet();
				_db.ExecuteQuery(ds,sbQuery.ToString());

				// 결과 DataSet의 데이터모델에 복사
				statisticsDailyModel.ReportDataSet = ds.Copy();

				// 결과
				statisticsDailyModel.ResultCnt = Utility.GetDatasetCount(statisticsDailyModel.ReportDataSet);

				ds.Dispose();

				// 결과코드 셋트
				statisticsDailyModel.ResultCD = "0000";

                // __DEBUG__
                _log.Debug("<출력정보>");
                _log.Debug("ResultCnt:[" + statisticsDailyModel.ResultCnt + "]");
                // __DEBUG__

                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "GetStatisticsDaily() End");
                _log.Debug("-----------------------------------------");
            }
            catch(Exception ex)
            {
                statisticsDailyModel.ResultCD = "3000";
				if(isNotTarget)
				{
					statisticsDailyModel.ResultDesc = "해당광고의 정보가 존재하지 않습니다.";
				}
				else if(isNotReady)
				{
					statisticsDailyModel.ResultDesc = "해당 기간은 데이터가 집계되지 않았습니다.";
				}
				else
				{
					statisticsDailyModel.ResultDesc = "일별통계 조회중 오류가 발생하였습니다";
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