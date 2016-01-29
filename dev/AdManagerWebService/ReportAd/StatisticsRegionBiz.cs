// ===============================================================================
//
// StatisticsRegionBiz.cs
//
// 광고리포트 지역별통계 서비스 
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
 * Class Name: StatisticsRegionBiz
 * 주요기능  : 광고리포트 지역별통계 처리 로직
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
    /// StatisticsRegionBiz에 대한 요약 설명입니다.
    /// </summary>
    public class StatisticsRegionBiz : BaseBiz
    {

		#region  생성자
        public StatisticsRegionBiz() : base(FrameSystem.connSummaryDbString)
        {
            _log = FrameSystem.oLog;
        }
		#endregion

		#region 기간내 지역별통계
        /// <summary>
        ///  기간내 지역별통계=-원본 method
        /// </summary>
        /// <param name="statisticsRegionModel"></param>
        public void GetStatisticsRegion_Org(HeaderModel header, StatisticsRegionModel statisticsRegionModel)
        {
			bool isNotTarget = false; // 타겟팅정보가 입력되지않아 존재하지 않을때.
			bool isNotReady  = false; // 데이터가 집계가 되지않아 존재하지 않을때.
            try
			{
				StringBuilder sbQuery = null;

				// 데이터베이스를 OPEN한다
				_db.Open();


                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "GetStatisticsRegion() Start");
                _log.Debug("-----------------------------------------");

				// 일자가 6자리 이상(yyyymmdd)이면 6자리로 만든다.
				if(statisticsRegionModel.SearchStartDay.Length > 6) statisticsRegionModel.SearchStartDay = statisticsRegionModel.SearchStartDay.Substring(2,6);
				if(statisticsRegionModel.SearchEndDay.Length   > 6) statisticsRegionModel.SearchEndDay   = statisticsRegionModel.SearchEndDay.Substring(2,6);
				
				// __DEBUG__
                _log.Debug("<입력정보>");
				_log.Debug("SearchMediaCode	  :[" + statisticsRegionModel.SearchMediaCode   + "]");		// 검색 매체
				_log.Debug("SearchContractSeq :[" + statisticsRegionModel.SearchContractSeq      + "]");		// 검색 광고번호           
				_log.Debug("SearchItemNo      :[" + statisticsRegionModel.SearchItemNo      + "]");		// 검색 광고번호           
				_log.Debug("SearchStartDay    :[" + statisticsRegionModel.SearchStartDay    + "]");		// 검색 집계시작 일자          
				_log.Debug("SearchEndDay      :[" + statisticsRegionModel.SearchEndDay      + "]");		// 검색 집계종료 일자          
				// __DEBUG__

				string  MediaCode   = statisticsRegionModel.SearchMediaCode;
                int	    ContractSeq = Convert.ToInt32( statisticsRegionModel.SearchContractSeq );
                int	    CampaignCd	= Convert.ToInt32( statisticsRegionModel.CampaignCode );
                int	    ItemNo      = Convert.ToInt32( statisticsRegionModel.SearchItemNo );
				string  StartDay    = statisticsRegionModel.SearchStartDay;
				string  EndDay      = statisticsRegionModel.SearchEndDay;

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
                statisticsRegionModel.ContractAmt = ContractAmt;
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
                statisticsRegionModel.TotalAdCnt = TotalAdCnt;
                #endregion

                // 쿼리생성
				sbQuery = new StringBuilder();
				sbQuery.Append("\n"
					+ "/* 기간내 광고 지역별통계    */                 \n"
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
                    sbQuery.Append("    and a.ItemNo in( select ItemNo from AdTargetsHanaTV.dbo.CampaignItem where CampaignCode = " + CampaignCd + " )" + "\n");
                    // 계약조회인지 개별광고조회인지
				sbQuery.Append(""
                    + "   AND A.SummaryType  = 1                  \n"
                    + "                                           \n"
                    + "-- 지역명 순번을 위한 임시테이블                \n"
                    + "SELECT IDENTITY(INT, 1, 1) AS Rownum, T.*  \n"
                    + "INTO #TempRegion                           \n"
                    + "FROM (SELECT A.SummaryCode                 \n"
                    + "              ,A.SummaryName               \n"
                    + "          FROM SummaryCode A  with(NoLock) \n"
                    + "         WHERE A.SummaryType = 5           \n"
                    + "           AND A.Level = 1 ) T             \n"
					+ "                                           \n"
					+ "-- 지역별 집계                               \n"
                    + "SELECT '1' AS ORD                          \n"
					+ "      ,(SPACE(2 - LEN(CONVERT(VARCHAR(2),TT.Rownum))) \n"
					+ "               + CONVERT(VARCHAR(2),TT.Rownum) + ' ' + TA.SummaryName ) AS GrpName \n"
					+ "      ,TA.SummaryCode AS SumCode           \n"
                    + "      ,TA.SummaryName AS SumName           \n"
                    + "      ,(SELECT COUNT(*) FROM SummaryCode WHERE SummaryType = 5 AND Level = 2 AND UpperCode = TA.SummaryCode) AS SubCount \n"
                    + "      ,0 AS subSumCode                     \n"
                    + "      ,'합계' AS subSumName                 \n"
                    + "      ,ISNULL(SUM(TB.AdCnt),0) AS AdCnt    \n"
                    + "      ,CASE WHEN @TotAdHit > 0 THEN CONVERT(DECIMAL(9,2),(ISNULL(SUM(TB.AdCnt),0) / CONVERT(float,@TotAdHit)) * 100)     \n"
                    + "                               ELSE 0 END AS AdRate                                                                      \n"
                    + "      ,REPLICATE('■', CASE WHEN @TotAdHit  > 0 THEN ROUND((ISNULL(SUM(TB.AdCnt),0)/CONVERT(float,@TotAdHit) * 100),0)   \n"
                    + "                                                ELSE 0 END) AS RateBar                                                   \n"
                    + "  FROM (SELECT A.SummaryCode               \n"
                    + "              ,A.SummaryName               \n"
                    + "          FROM SummaryCode A  with(NoLock) \n"
                    + "         WHERE A.SummaryType = 5           \n"
                    + "           AND A.Level = 1                 \n"
                    + "       ) TA                                \n"
                    + "       LEFT JOIN                           \n"
                    + "       (                                   \n"
                    + "        SELECT CASE B.Level WHEN 1 THEN B.SummaryCode                      \n"
                    + "                                   ELSE B.UpperCode END AS SummaryCode     \n"
                    + "              ,A.AdCnt                                                     \n"
                    + "              ,B.Level                                                     \n"
                    + "          FROM SummaryAdDaily0        A with(NoLock)      \n"
					+ "               INNER JOIN SummaryCode B with(NoLock) ON (A.SummaryType = B.SummaryType AND A.SummaryCode = B.SummaryCode) \n"
					+ "         WHERE A.LogDay BETWEEN '"+ StartDay  + "' \n"
					+ "                            AND '"+ EndDay    + "' \n");
                if( ItemNo > 0 )                            // 캠페인이 전체이면서 광고내역이 개별이면 개별광고임
                    sbQuery.Append("    and a.ItemNo  = " + ItemNo + "\n");
                else if( CampaignCd == 0 && ItemNo == 0 )   // 캠페인이 전체이면서 광고내역이 전체이면 전체 계약임
                    sbQuery.Append("    and a.ItemNo in(select  ItemNo from AdTargetsHanaTV.dbo.ContractItem with(noLock) where MediaCode=1 and ContractSeq = " + ContractSeq + " and AdType < '90' )");
                else if( CampaignCd > 0 && ItemNo == 0 )    // 캠페인이 선택되고, 전체광고면 해당캠패인 전체임
                    sbQuery.Append("    and a.ItemNo in( select ItemNo from AdTargetsHanaTV.dbo.CampaignItem where CampaignCode = " + CampaignCd + " )" + "\n");
				sbQuery.Append(""
                    + "           AND A.SummaryType  = 5  -- 5:지역별      \n"
                    + "       ) TB                                        \n"
					+ "       ON (TA.SummaryCode = TB.SummaryCode)        \n"
					+ "       LEFT JOIN #TempRegion TT ON (TA.SummaryCode = TT.SummaryCode)   \n"
					+ " GROUP BY TT.Rownum, TA.SummaryCode, TA.SummaryName           \n"
                    + "                                                   \n"
                    + "UNION ALL                                          \n"
                    + "                                                   \n"
                    + "-- 상세지역별 집계                                    \n"
                    + "SELECT '2' AS ORD                                  \n"
					+ "      ,(SPACE(2 - LEN(CONVERT(VARCHAR(2),TT.Rownum)))  \n"
					+ "               + CONVERT(VARCHAR(2),TT.Rownum) + ' ' + TC.SummaryName ) AS GrpName \n"
                    + "      ,TA.UpperCode   AS SumCode                   \n"
                    + "      ,TC.SummaryName AS SumName                   \n"
                    + "      ,0 AS SubCount                               \n"
                    + "      ,TA.SummaryCode AS subSumCode                \n"
                    + "      ,TA.SummaryName AS subSumName                \n"
                    + "      ,ISNULL(SUM(TB.AdCnt),0) AS AdCnt            \n" 
                    + "      ,CASE WHEN @TotAdHit > 0 THEN CONVERT(DECIMAL(9,2),(ISNULL(SUM(TB.AdCnt),0) / CONVERT(float,@TotAdHit)) * 100)       \n"
                     + "                              ELSE 0 END AS AdRate                                                                        \n"
                    + "      ,REPLICATE('■', CASE WHEN @TotAdHit  > 0 THEN ROUND((ISNULL(SUM(TB.AdCnt),0)/CONVERT(float,@TotAdHit) * 100),0)     \n"
                    + "                                                ELSE 0 END) AS RateBar                                                     \n"
                    + "  FROM (SELECT SummaryCode, SummaryName, UpperCode FROM SummaryCode with(NoLock)  WHERE SummaryType = 5 AND Level = 2) TA                           \n"
                    + "       LEFT JOIN                                   \n"
                    + "       (                                           \n"
                    + "        SELECT A.SummaryCode                       \n"
                    + "              ,B.UpperCode                         \n"
                    + "              ,A.AdCnt                             \n"
                    + "              ,B.Level                             \n"
                    + "          FROM SummaryAdDaily0        A with(NoLock)      \n"
					+ "               INNER JOIN SummaryCode B with(NoLock) ON (A.SummaryType = B.SummaryType AND A.SummaryCode = B.SummaryCode)      \n"
	                + "         WHERE A.LogDay BETWEEN '"+ StartDay  + "' \n"
					+ "                            AND '"+ EndDay    + "' \n");
                if( ItemNo > 0 )                            // 캠페인이 전체이면서 광고내역이 개별이면 개별광고임
                    sbQuery.Append("    and a.ItemNo  = " + ItemNo + "\n");
                else if( CampaignCd == 0 && ItemNo == 0 )   // 캠페인이 전체이면서 광고내역이 전체이면 전체 계약임
                    sbQuery.Append("    and a.ItemNo in(select  ItemNo from AdTargetsHanaTV.dbo.ContractItem with(noLock) where MediaCode=1 and ContractSeq = " + ContractSeq + " and AdType < '90' )");
                else if( CampaignCd > 0 && ItemNo == 0 )    // 캠페인이 선택되고, 전체광고면 해당캠패인 전체임
                    sbQuery.Append("    and a.ItemNo in( select ItemNo from AdTargetsHanaTV.dbo.CampaignItem where CampaignCode = " + CampaignCd + " )" + "\n");
				sbQuery.Append(""
                    + "           AND A.SummaryType  = 5  -- 5:지역별      \n"
                    + "       ) TB                                        \n"
                    + "       ON (TA.SummaryCode = TB.SummaryCode)                                                                         \n"
					+ "       LEFT JOIN SummaryCode TC with(NoLock) ON (TA.UpperCode = TC.SummaryCode AND TC.SummaryType = 5 AND TC.Level = 1)          \n"
					+ "       LEFT JOIN #TempRegion TT ON (TA.UpperCode = TT.SummaryCode)                                                  \n"
					+ " GROUP BY TT.Rownum, TA.UpperCode, TC.SummaryName, TA.SummaryCode, TA.SummaryName                                              \n"
                    + "                                                   \n"
                    + " ORDER BY SumCode, AdCnt DESC                      \n"
					+ "                                                   \n"
					+ " DROP Table #TempRegion                            \n"
					);  

				// __DEBUG__
				_log.Debug(sbQuery.ToString());
				// __DEBUG__
				
				// 쿼리실행
				ds = new DataSet();
				_db.ExecuteQuery(ds,sbQuery.ToString());

				// 결과 DataSet의 데이터모델에 복사
				statisticsRegionModel.ReportDataSet = ds.Copy();

				// 결과
				statisticsRegionModel.ResultCnt = Utility.GetDatasetCount(statisticsRegionModel.ReportDataSet);

				ds.Dispose();

				// 결과코드 셋트
				statisticsRegionModel.ResultCD = "0000";

                // __DEBUG__
                _log.Debug("<출력정보>");
                _log.Debug("ResultCnt:[" + statisticsRegionModel.ResultCnt + "]");
                // __DEBUG__

                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "GetStatisticsRegion() End");
                _log.Debug("-----------------------------------------");
            }
            catch(Exception ex)
            {
                statisticsRegionModel.ResultCD = "3000";
				if(isNotTarget)
				{
					statisticsRegionModel.ResultDesc = "해당광고의 정보가 존재하지 않습니다.";
				}
				else if(isNotReady)
				{
					statisticsRegionModel.ResultDesc = "해당 기간은 데이터가 집계되지 않았습니다.";
				}
				else
				{
					statisticsRegionModel.ResultDesc = "지역별통계 조회중 오류가 발생하였습니다";
					_log.Exception(ex);
				}
            }
			finally
			{
				// 데이터베이스를  Close한다
				_db.Close();
			}

        }



		/// <summary>
		/// 기간내 지역별통계=확장본
		/// </summary>
		/// <param name="header"></param>
		/// <param name="statisticsRegionModel"></param>
		public void GetStatisticsRegion(HeaderModel header, StatisticsRegionModel statisticsRegionModel)
		{
			bool isNotTarget = false; // 타겟팅정보가 입력되지않아 존재하지 않을때.
			bool isNotReady  = false; // 데이터가 집계가 되지않아 존재하지 않을때.
			try
			{
				StringBuilder sbQuery = null;

				// 데이터베이스를 OPEN한다
				_db.Open();


				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetStatisticsRegion() Start");
				_log.Debug("-----------------------------------------");

				// 일자가 6자리 이상(yyyymmdd)이면 6자리로 만든다.
				if(statisticsRegionModel.SearchStartDay.Length > 6) statisticsRegionModel.SearchStartDay = statisticsRegionModel.SearchStartDay.Substring(2,6);
				if(statisticsRegionModel.SearchEndDay.Length   > 6) statisticsRegionModel.SearchEndDay   = statisticsRegionModel.SearchEndDay.Substring(2,6);
				
				// __DEBUG__
				_log.Debug("<입력정보>");
				_log.Debug("SearchMediaCode	  :[" + statisticsRegionModel.SearchMediaCode   + "]");		// 검색 매체
				_log.Debug("SearchContractSeq :[" + statisticsRegionModel.SearchContractSeq + "]");		// 검색 광고번호           
				_log.Debug("SearchItemNo      :[" + statisticsRegionModel.SearchItemNo      + "]");		// 검색 광고번호           
				_log.Debug("SearchStartDay    :[" + statisticsRegionModel.SearchStartDay    + "]");		// 검색 집계시작 일자          
				_log.Debug("SearchEndDay      :[" + statisticsRegionModel.SearchEndDay      + "]");		// 검색 집계종료 일자          
				// __DEBUG__

				string  MediaCode   = statisticsRegionModel.SearchMediaCode;
				int	    ContractSeq = Convert.ToInt32( statisticsRegionModel.SearchContractSeq );
				int	    CampaignCd	= Convert.ToInt32( statisticsRegionModel.CampaignCode );
				int	    ItemNo      = Convert.ToInt32( statisticsRegionModel.SearchItemNo );
				string  StartDay    = statisticsRegionModel.SearchStartDay;
				string  EndDay      = statisticsRegionModel.SearchEndDay;

				#region [ 보장노출건수 구하기 ]
				sbQuery = new StringBuilder();
				sbQuery.Append("\n");
				sbQuery.Append(" -- 해당광고의 노출기간조회" + "\n");
				sbQuery.Append(" -- 해당광고의 계약노출(보장노출)조회" + "\n");
				sbQuery.Append(" SELECT sum(ISNULL(b.ContractAmt,0)) AS ContractAmt" + "\n");
                sbQuery.Append(" FROM AdTargetsHanaTV.dbo.ContractItem a with(nolock)" + "\n");
                sbQuery.Append(" LEFT JOIN AdTargetsHanaTV.dbo.Targeting b with(nolock)" + "\n");
				sbQuery.Append("        ON (a.ItemNo = b.ItemNo)"    + "\n");
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
				statisticsRegionModel.ContractAmt = ContractAmt;
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
				statisticsRegionModel.TotalAdCnt = TotalAdCnt;
				#endregion

				// 쿼리생성
				sbQuery = new StringBuilder();
				
				sbQuery.Append("/* 기간내 광고 지역별통계    */              \n");
				sbQuery.Append("DECLARE @TotAdHit int;    -- 전체 광고노출수 \n");
				sbQuery.Append("SET @TotAdHit    = 0;                        \n");
				sbQuery.Append("                                             \n");

				#region [ 전체광고Hit ]
				sbQuery.Append("-- 전체 광고Hit                              \n");
				sbQuery.Append(" SELECT @TotAdHit = ISNULL(SUM(A.AdCnt),0)    \n");
				sbQuery.Append(" From   SummaryAdDaily0 a with(NoLock)			\n");
				sbQuery.Append(" Where  a.LogDay BETWEEN '"+ StartDay  + "' And '"+ EndDay + "'   \n");
				sbQuery.Append(" And    a.SummaryType = 1                     \n");

				if( ItemNo > 0 )                            // 캠페인이 전체이면서 광고내역이 개별이면 개별광고임
					sbQuery.Append("    and a.ItemNo  = " + ItemNo + "\n");
				else if( CampaignCd == 0 && ItemNo == 0 )   // 캠페인이 전체이면서 광고내역이 전체이면 전체 계약임
                    sbQuery.Append("    and a.ItemNo in(select  ItemNo from AdTargetsHanaTV.dbo.ContractItem with(noLock) where MediaCode=1 and ContractSeq = " + ContractSeq + " and AdType < '90' )");
				else if( CampaignCd > 0 && ItemNo == 0 )    // 캠페인이 선택되고, 전체광고면 해당캠패인 전체임
                    sbQuery.Append("    and a.ItemNo in( select ItemNo from AdTargetsHanaTV.dbo.CampaignItem where CampaignCode = " + CampaignCd + " )" + "\n");
				#endregion
									
				#region [지역명 순번을 위한 임시테이블 ]
				sbQuery.Append("-- 지역명 순번을 위한 임시테이블                    \n");
				sbQuery.Append("	Select IDENTITY(INT, 1, 1) AS Rownum, T.*       \n");
				sbQuery.Append("	Into #TempRegion                                \n");
				sbQuery.Append("	From (	select	 A.SummaryCode                  \n");
				sbQuery.Append("					,A.SummaryName                  \n");
				sbQuery.Append("			From   SummaryCode A  with(NoLock)      \n");
				sbQuery.Append("			Where  A.SummaryType = 5                \n"); 
				sbQuery.Append("			And    A.Level       = 1 ) T;           \n");
				#endregion

				#region [1단계- 광역시도 합]
				sbQuery.Append("-- 지역별 집계                             \n");
				sbQuery.Append("SELECT '1' AS ORD                          \n");
                sbQuery.Append("	  ,AdTargetsHanaTV.dbo.ufnPadding('L',tt.RowNum, 2,'0') + ' ' + TA.SummaryName AS GrpName \n");
				sbQuery.Append("      ,TA.SummaryCode AS SumCode           \n");
				sbQuery.Append("      ,TA.SummaryName AS SumName           \n");
                sbQuery.Append("      ,(SELECT COUNT(*) FROM AdTargetsHanaTV.dbo.TargetRegion WHERE Level = 2 AND UpperCode = TA.SummaryCode) AS SubCount \n");
				sbQuery.Append("      ,0 AS subSumCode                     \n");
				sbQuery.Append("      ,'[합계]' AS subSumName                \n");
				sbQuery.Append("      ,ISNULL(SUM(TB.AdCnt),0) AS AdCnt    \n");
				sbQuery.Append("      ,CASE WHEN @TotAdHit > 0 THEN CONVERT(DECIMAL(9,2),(ISNULL(SUM(TB.AdCnt),0) / CONVERT(float,@TotAdHit)) * 100)    \n");
				sbQuery.Append("                               ELSE 0 END AS AdRate                                                                     \n");
				sbQuery.Append("      ,''   AS RateBar						\n");
				sbQuery.Append("	  ,TA.Orders      As Orders				\n");
				sbQuery.Append("      ,TA.ParentCode  As ParentCode			\n");
				sbQuery.Append("      ,TA.SummaryDesc As SummaryDesc	\n");
				sbQuery.Append("  FROM (SELECT a.regionCode	as SummaryCode  \n");
				sbQuery.Append("              ,a.regionName as SummaryName  \n");
				sbQuery.Append("			  ,a.UpperCode					\n");
				sbQuery.Append("              ,a.Orders						\n");
				sbQuery.Append("              ,a.ParentCode					\n");
                sbQuery.Append("              ,a.regionName as SummaryDesc	\n");
                sbQuery.Append("        from	AdTargetsHanaTV.dbo.TargetRegion A  with(NoLock)\n");
				sbQuery.Append("        where	a.Level = 1 ) TA				\n");
				sbQuery.Append("  LEFT JOIN                           \n");
				sbQuery.Append("	  (	SELECT CASE B.Level WHEN 1 THEN B.regionCode                      \n");
				sbQuery.Append("                                   ELSE B.UpperCode END AS SummaryCode     \n");
				sbQuery.Append("              ,A.AdCnt                                                     \n");
				sbQuery.Append("              ,B.Level                                                     \n");
				sbQuery.Append("        FROM SummaryAdDaily0    A with(NoLock)									\n");
                sbQuery.Append("        INNER JOIN AdTargetsHanaTV.dbo.TargetRegion B with(NoLock) ON A.SummaryCode = B.regionCode	\n");
				sbQuery.Append("        WHERE A.LogDay BETWEEN '"+ StartDay  + "' AND '"+ EndDay + "' \n");
				sbQuery.Append("		And   a.SummaryType = 5		\n");

				if( ItemNo > 0 )                            // 캠페인이 전체이면서 광고내역이 개별이면 개별광고임
					sbQuery.Append("    and a.ItemNo  = " + ItemNo + "\n");
				else if( CampaignCd == 0 && ItemNo == 0 )   // 캠페인이 전체이면서 광고내역이 전체이면 전체 계약임
                    sbQuery.Append("    and a.ItemNo in(select  ItemNo from AdTargetsHanaTV.dbo.ContractItem with(noLock) where MediaCode=1 and ContractSeq = " + ContractSeq + " and AdType < '90' )");
				else if( CampaignCd > 0 && ItemNo == 0 )    // 캠페인이 선택되고, 전체광고면 해당캠패인 전체임
                    sbQuery.Append("    and a.ItemNo in( select ItemNo from AdTargetsHanaTV.dbo.CampaignItem where CampaignCode = " + CampaignCd + " )" + "\n");

				sbQuery.Append("        ) TB    		                  \n");

				sbQuery.Append("       ON (TA.SummaryCode = TB.SummaryCode)        \n");
				sbQuery.Append("       LEFT JOIN #TempRegion TT ON (TA.SummaryCode = TT.SummaryCode)   \n");
				sbQuery.Append(" GROUP BY TT.Rownum, TA.SummaryCode, TA.SummaryName, TA.Orders ,TA.ParentCode, TA.SummaryDesc           \n");
				sbQuery.Append("                                                   \n");
				#endregion
				#region[2단계-시군구]
				sbQuery.Append("UNION ALL                                          \n");
				sbQuery.Append("                                                   \n");
				sbQuery.Append("-- 상세지역별 집계                                    \n");
				sbQuery.Append("SELECT '2' AS ORD                                  \n");
                sbQuery.Append("	  ,AdTargetsHanaTV.dbo.ufnPadding('L',tt.RowNum, 2,'0') + ' ' + TC.RegionName AS GrpName \n");
				sbQuery.Append("      ,TA.UpperCode   AS SumCode                   \n");
				sbQuery.Append("      ,TC.RegionName AS SumName                   \n");
                sbQuery.Append("      ,(SELECT COUNT(*) FROM AdTargetsHanaTV.dbo.TargetRegion WHERE Level = 3 AND ParentCode = TA.SummaryCode) AS SubCount \n");
				sbQuery.Append("      ,TA.SummaryCode AS subSumCode                \n");
				sbQuery.Append("      ,'   ' + TA.SummaryName AS subSumName                \n");
				sbQuery.Append("      ,ISNULL(SUM(TB.AdCnt),0) AS AdCnt            \n"); 
				sbQuery.Append("      ,CASE WHEN @TotAdHit > 0 THEN CONVERT(DECIMAL(9,2),(ISNULL(SUM(TB.AdCnt),0) / CONVERT(float,@TotAdHit)) * 100)       \n");
				sbQuery.Append("                              ELSE 0 END AS AdRate                                                                         \n");
				sbQuery.Append("      ,'' AS RateBar                                                     \n");
				sbQuery.Append("	  ,TA.Orders      As Orders                    \n");
				sbQuery.Append("      ,TA.ParentCode  As ParentCode				   \n");
				sbQuery.Append("      ,TA.SummaryDesc As SummaryDesc			   \n");
				sbQuery.Append("  FROM (SELECT a.regionCode	as SummaryCode  \n");
				sbQuery.Append("              ,a.regionName as SummaryName  \n");
				sbQuery.Append("			  ,a.UpperCode					\n");
				sbQuery.Append("              ,a.Orders						\n");
				sbQuery.Append("              ,a.ParentCode					\n");
				sbQuery.Append("              ,a.regionName as SummaryDesc	\n");
                sbQuery.Append("        from	AdTargetsHanaTV.dbo.TargetRegion A  with(NoLock)\n");
				sbQuery.Append("        where	a.Level = 2 ) TA				\n");
				sbQuery.Append("       LEFT JOIN                                   \n");
				sbQuery.Append("       (                                           \n");
				sbQuery.Append("        SELECT A.SummaryCode                       \n");
				sbQuery.Append("              ,B.UpperCode                         \n");
				sbQuery.Append("              ,A.AdCnt                             \n");
				sbQuery.Append("              ,B.Level                             \n");
				sbQuery.Append("              ,B.ParentCode                        \n");
				sbQuery.Append("        FROM SummaryAdDaily0	A with(NoLock)      \n");
                sbQuery.Append("		INNER JOIN AdTargetsHanaTV.dbo.TargetRegion B with(NoLock) ON A.SummaryCode = B.RegionCode  \n");
				sbQuery.Append("        WHERE A.LogDay BETWEEN '"+ StartDay  + "' AND '"+ EndDay + "' \n");
				sbQuery.Append("		And   a.SummaryType = 5		\n");

				if( ItemNo > 0 )                            // 캠페인이 전체이면서 광고내역이 개별이면 개별광고임
					sbQuery.Append("    and a.ItemNo  = " + ItemNo + "\n");
				else if( CampaignCd == 0 && ItemNo == 0 )   // 캠페인이 전체이면서 광고내역이 전체이면 전체 계약임
                    sbQuery.Append("    and a.ItemNo in(select  ItemNo from AdTargetsHanaTV.dbo.ContractItem with(noLock) where MediaCode=1 and ContractSeq = " + ContractSeq + " and AdType < '90' )");
				else if( CampaignCd > 0 && ItemNo == 0 )    // 캠페인이 선택되고, 전체광고면 해당캠패인 전체임
                    sbQuery.Append("    and a.ItemNo in( select ItemNo from AdTargetsHanaTV.dbo.CampaignItem where CampaignCode = " + CampaignCd + " )" + "\n");
				
				sbQuery.Append("       ) TB                                         \n");
				sbQuery.Append("       ON (TA.SummaryCode = TB.ParentCode)          \n");
                sbQuery.Append("       LEFT JOIN AdTargetsHanaTV.dbo.TargetRegion TC with(NoLock) ON (TA.UpperCode = TC.RegionCode AND TC.Level = 1) \n");
				sbQuery.Append("       LEFT JOIN #TempRegion TT ON (TA.UpperCode = TT.SummaryCode)                                                      \n");
				sbQuery.Append(" GROUP BY TT.Rownum, TA.UpperCode, TC.RegionName, TA.SummaryCode, TA.SummaryName                                       \n");
                sbQuery.Append("         ,TA.Orders ,TA.ParentCode, TA.SummaryDesc \n");                        
				sbQuery.Append("                                                   \n");
				#endregion
//
				sbQuery.Append("UNION ALL                                          \n");                                          
                                                   
				sbQuery.Append("-- 상세지역별 집계                                 \n");    
				sbQuery.Append("SELECT                                             \n");
				sbQuery.Append("	'3' AS ORD                                     \n");
                sbQuery.Append("	,AdTargetsHanaTV.dbo.ufnPadding('L',tt.RowNum, 2,'0') + ' ' + TC.RegionName AS GrpName \n");
				sbQuery.Append("	,TA.UpperCode   AS SumCode                   \n");
				sbQuery.Append("	,TC.RegionName AS SumName                   \n");
				sbQuery.Append("	,0 AS SubCount                               \n");
				sbQuery.Append("	,TA.SummaryCode AS subSumCode                \n");
				sbQuery.Append("	,'   ' + TD.RegionName + '>>' + TA.SummaryName AS subSumName                \n");
				sbQuery.Append("	,ISNULL(SUM(TB.AdCnt),0) AS AdCnt            \n");
				sbQuery.Append("	,CASE WHEN @TotAdHit > 0 THEN CONVERT(DECIMAL(9,2),(ISNULL(SUM(TB.AdCnt),0) / CONVERT(float,@TotAdHit)) * 100)      \n");
				sbQuery.Append("							ELSE 0 END AS AdRate                                                                        \n");
				sbQuery.Append("	,'' AS RateBar       \n");
				sbQuery.Append("	,TA.Orders      As Orders		             \n");
				sbQuery.Append("	,TA.ParentCode  As ParentCode                \n");
				sbQuery.Append("	,TD.RegionName As SummaryDesc               \n");
				sbQuery.Append("  FROM (SELECT a.regionCode	as SummaryCode  \n");
				sbQuery.Append("              ,a.regionName as SummaryName  \n");
				sbQuery.Append("			  ,a.UpperCode					\n");
				sbQuery.Append("              ,a.Orders						\n");
				sbQuery.Append("              ,a.ParentCode					\n");
				sbQuery.Append("              ,a.regionName as SummaryDesc	\n");
                sbQuery.Append("        from	AdTargetsHanaTV.dbo.TargetRegion A  with(NoLock)\n");
				sbQuery.Append("        where	a.Level = 3 ) TA				\n");
				sbQuery.Append("	LEFT JOIN									 \n");	
				sbQuery.Append("	(                                            \n");
				sbQuery.Append("        SELECT A.SummaryCode                       \n");
				sbQuery.Append("              ,B.UpperCode                         \n");
				sbQuery.Append("              ,A.AdCnt                             \n");
				sbQuery.Append("              ,B.Level                             \n");
				sbQuery.Append("              ,B.ParentCode                        \n");
				sbQuery.Append("        FROM SummaryAdDaily0	A with(NoLock)      \n");
                sbQuery.Append("		INNER JOIN AdTargetsHanaTV.dbo.TargetRegion B with(NoLock) ON A.SummaryCode = B.RegionCode  \n");
				sbQuery.Append("        WHERE A.LogDay BETWEEN '"+ StartDay  + "' AND '"+ EndDay + "' \n");
				sbQuery.Append("		And   a.SummaryType = 5		\n");

				if( ItemNo > 0 )                            // 캠페인이 전체이면서 광고내역이 개별이면 개별광고임
					sbQuery.Append("    and a.ItemNo  = " + ItemNo + "\n");
				else if( CampaignCd == 0 && ItemNo == 0 )   // 캠페인이 전체이면서 광고내역이 전체이면 전체 계약임
                    sbQuery.Append("    and a.ItemNo in(select  ItemNo from AdTargetsHanaTV.dbo.ContractItem with(noLock) where MediaCode=1 and ContractSeq = " + ContractSeq + " and AdType < '90' )");
				else if( CampaignCd > 0 && ItemNo == 0 )    // 캠페인이 선택되고, 전체광고면 해당캠패인 전체임
                    sbQuery.Append("    and a.ItemNo in( select ItemNo from AdTargetsHanaTV.dbo.CampaignItem with(noLock) where CampaignCode = " + CampaignCd + " )" + "\n");

				
				sbQuery.Append("	) TB                                        \n");
				sbQuery.Append("	ON (TA.SummaryCode = TB.SummaryCode)         \n");
                sbQuery.Append("       LEFT JOIN AdTargetsHanaTV.dbo.TargetRegion TC with(NoLock) ON (TA.UpperCode = TC.RegionCode AND TC.Level = 1) \n");
                sbQuery.Append("       LEFT JOIN AdTargetsHanaTV.dbo.TargetRegion TD with(NoLock) ON (TA.ParentCode = TD.RegionCode AND TD.Level = 2) \n");
				sbQuery.Append("       LEFT JOIN #TempRegion TT ON (TA.UpperCode = TT.SummaryCode)                                                      \n");
				sbQuery.Append(" GROUP BY TT.Rownum, TA.UpperCode, TC.RegionName, TA.SummaryCode, TA.SummaryName                                       \n");
				sbQuery.Append("         ,TA.Orders ,TA.ParentCode, TD.RegionName \n");                        
				sbQuery.Append(" ORDER BY Orders, AdCnt DESC                       \n");
				sbQuery.Append("                                                   \n");
				sbQuery.Append(" DROP Table #TempRegion                            \n");
					

				// __DEBUG__
				_log.Debug(sbQuery.ToString());
				// __DEBUG__
				
				// 쿼리실행
				ds = new DataSet();
				_db.ExecuteQuery(ds,sbQuery.ToString());

				// 결과 DataSet의 데이터모델에 복사
				statisticsRegionModel.ReportDataSet = ds.Copy();

				// 결과
				statisticsRegionModel.ResultCnt = Utility.GetDatasetCount(statisticsRegionModel.ReportDataSet);

				ds.Dispose();

				// 결과코드 셋트
				statisticsRegionModel.ResultCD = "0000";

				// __DEBUG__
				_log.Debug("<출력정보>");
				_log.Debug("ResultCnt:[" + statisticsRegionModel.ResultCnt + "]");
				// __DEBUG__

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetStatisticsRegion() End");
				_log.Debug("-----------------------------------------");
			}
			catch(Exception ex)
			{
				statisticsRegionModel.ResultCD = "3000";
				if(isNotTarget)
				{
					statisticsRegionModel.ResultDesc = "해당광고의 정보가 존재하지 않습니다.";
				}
				else if(isNotReady)
				{
					statisticsRegionModel.ResultDesc = "해당 기간은 데이터가 집계되지 않았습니다.";
				}
				else
				{
					statisticsRegionModel.ResultDesc = "지역별통계 조회중 오류가 발생하였습니다";
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