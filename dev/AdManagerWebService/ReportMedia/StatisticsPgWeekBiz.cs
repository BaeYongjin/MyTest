// ===============================================================================
//
// StatisticsPgWeekBiz.cs
//
// 컨텐츠리포트 요일별통계 서비스 
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
 * Class Name: StatisticsPgWeekBiz
 * 주요기능  : 컨텐츠리포트 요일별통계 처리 로직
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
 * -------------------------------------------------------
 * 수정코드  : [E_02]
 * 수정자    : Youngil.Yi
 * 수정일    : 2014.10.23
 * 수정부분  :
 *			  - GetStatisticsPgWeek()
 * 수정내용  : 
 * Exception : "이 쿼리에 정의된 힌트로 인해 쿼리 프로세서에서 쿼리 계획을 생성할 수 없습니다. 힌트를 지정하거나 SET FORCEPLAN을 사용하지 않고 쿼리를 다시 전송하십시오."
 * 처리를 위해서 merge를 없앰
 * --
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

namespace AdManagerWebService.ReportMedia
{
    /// <summary>
    /// StatisticsPgWeekBiz에 대한 요약 설명입니다.
    /// </summary>
    public class StatisticsPgWeekBiz : BaseBiz
    {

		#region  생성자
        public StatisticsPgWeekBiz() : base(FrameSystem.connSummaryDbString)
        {
            _log = FrameSystem.oLog;
        }
		#endregion

		#region 카테고리목록 조회
		/// <summary>
		/// 카테고리목록조회
		/// </summary>
		/// <param name="categoryModel"></param>
		public void GetCategoryList(HeaderModel header, StatisticsPgWeekModel statisticsPgWeekModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetCategoryList() Start");
				_log.Debug("-----------------------------------------");

				// 데이터베이스를 OPEN한다
				_db.Open();

				// __DEBUG__
				_log.Debug("<입력정보>");
				_log.Debug("SearchMediaCode    :[" + statisticsPgWeekModel.SearchMediaCode + "]");
				// __DEBUG__

				StringBuilder sbQuery = new StringBuilder();

				// 쿼리생성
				sbQuery.Append("\n"
					+ " SELECT A.MenuCode AS CategoryCode, A.MenuName AS CategoryName  \n"
                    + "   FROM AdTargetsHanaTV.dbo.Menu A with(NoLock)                \n"										
					+ "  WHERE A.MediaCode = " + statisticsPgWeekModel.SearchMediaCode +"   \n"			
					+ "    AND A.MenuLevel = 1        \n" 
					+ "  ORDER BY A.MenuCode          \n"
					);
				
				// __DEBUG__
				_log.Debug(sbQuery.ToString());
				// __DEBUG__
				
				// 쿼리실행
				DataSet ds = new DataSet();
				_db.ExecuteQuery(ds,sbQuery.ToString());

				// 결과 DataSet의 카테고리모델에 복사
				statisticsPgWeekModel.CategoryDataSet = ds.Copy();
				// 결과
				statisticsPgWeekModel.ResultCnt = Utility.GetDatasetCount(statisticsPgWeekModel.CategoryDataSet);
				// 결과코드 셋트
				statisticsPgWeekModel.ResultCD = "0000";

				// __DEBUG__
				_log.Debug("<출력정보>");
				_log.Debug("ResultCnt:[" + statisticsPgWeekModel.ResultCnt + "]");
				// __DEBUG__

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetUsersList() End");
				_log.Debug("-----------------------------------------");
			}
			catch(Exception ex)
			{
				statisticsPgWeekModel.ResultCD = "3000";
				statisticsPgWeekModel.ResultDesc = "카테고리정보 조회중 오류가 발생하였습니다";
				_log.Exception(ex);
			}
			finally
			{
				// 데이터베이스를  Close한다
				_db.Close();			
			}
		}

		#endregion

		#region 장르목록 조회
		/// <summary>
		/// 장르목록조회
		/// </summary>
		/// <param name="categoryModel"></param>
		public void GetGenreList(HeaderModel header, StatisticsPgWeekModel statisticsPgWeekModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetGenreList() Start");
				_log.Debug("-----------------------------------------");

				// 데이터베이스를 OPEN한다
				_db.Open();

				// __DEBUG__
				_log.Debug("<입력정보>");
				_log.Debug("SearchMediaCode    :[" + statisticsPgWeekModel.SearchMediaCode + "]");
				// __DEBUG__

				StringBuilder sbQuery = new StringBuilder();

				// 쿼리생성
				sbQuery.Append("\n"
					+ " SELECT A.UpperMenuCode AS CategoryCode, A.MenuCode AS GenreCode, A.MenuName AS GenreName  \n"
                    + "   FROM AdTargetsHanaTV.dbo.Menu A  with(NoLock)                \n"										
					+ "  WHERE A.MediaCode = " + statisticsPgWeekModel.SearchMediaCode +"   \n"			
					+ "    AND A.MenuLevel = 2        \n" 
					+ "  ORDER BY A.UpperMenuCode, A.MenuCode \n"
					);
				
				// __DEBUG__
				_log.Debug(sbQuery.ToString());
				// __DEBUG__
				
				// 쿼리실행
				DataSet ds = new DataSet();
				_db.ExecuteQuery(ds,sbQuery.ToString());

				// 결과 DataSet의 모델에 복사
				statisticsPgWeekModel.GenreDataSet = ds.Copy();
				// 결과
				statisticsPgWeekModel.ResultCnt = Utility.GetDatasetCount(statisticsPgWeekModel.GenreDataSet);
				// 결과코드 셋트
				statisticsPgWeekModel.ResultCD = "0000";

				// __DEBUG__
				_log.Debug("<출력정보>");
				_log.Debug("ResultCnt:[" + statisticsPgWeekModel.ResultCnt + "]");
				// __DEBUG__

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetGenreList() End");
				_log.Debug("-----------------------------------------");
			}
			catch(Exception ex)
			{
				statisticsPgWeekModel.ResultCD = "3000";
				statisticsPgWeekModel.ResultDesc = "장르정보 조회중 오류가 발생하였습니다";
				_log.Exception(ex);
			}
			finally
			{
				// 데이터베이스를  Close한다
				_db.Close();			
			}
		}

		#endregion

		#region 기간내 요일별통계
        /// <summary>
        ///  기간내 요일별통계
        /// </summary>
        /// <param name="statisticsPgWeekModel"></param>
        public void GetStatisticsPgWeek(HeaderModel header, StatisticsPgWeekModel statisticsPgWeekModel)
        {
			bool isNotTarget = false; // 타겟팅정보가 입력되지않아 존재하지 않을때.
			bool isNotReady  = false; // 데이터가 집계가 되지않아 존재하지 않을때.
            try
			{
				StringBuilder sbQuery = null;

				// 데이터베이스를 OPEN한다
				_db.Open();


                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "GetStatisticsPgWeek() Start");
                _log.Debug("-----------------------------------------");

				// 일자가 6자리 이상(yyyymmdd)이면 6자리로 만든다.
				if(statisticsPgWeekModel.SearchStartDay.Length > 6) statisticsPgWeekModel.SearchStartDay = statisticsPgWeekModel.SearchStartDay.Substring(2,6);
				if(statisticsPgWeekModel.SearchEndDay.Length   > 6) statisticsPgWeekModel.SearchEndDay   = statisticsPgWeekModel.SearchEndDay.Substring(2,6);
				
				// __DEBUG__
                _log.Debug("<입력정보>");
				_log.Debug("SearchMediaCode	   :[" + statisticsPgWeekModel.SearchMediaCode    + "]");		// 검색 매체
				_log.Debug("SearchCategoryCode :[" + statisticsPgWeekModel.SearchCategoryCode + "]");		// 검색 카테고리코드 
				_log.Debug("SearchGenreCode    :[" + statisticsPgWeekModel.SearchGenreCode    + "]");		// 검색 장르코드           
				_log.Debug("SearchKey          :[" + statisticsPgWeekModel.SearchKey          + "]");		// 검색 키(프로그램명)           
				_log.Debug("SearchStartDay     :[" + statisticsPgWeekModel.SearchStartDay     + "]");		// 검색 집계시작 일자          
				_log.Debug("SearchEndDay       :[" + statisticsPgWeekModel.SearchEndDay       + "]");		// 검색 집계종료 일자          
				// __DEBUG__

				string MediaCode    = statisticsPgWeekModel.SearchMediaCode;
				string CategoryCode = statisticsPgWeekModel.SearchCategoryCode;
				string GenreCode    = statisticsPgWeekModel.SearchGenreCode;
				string ProgramName  = statisticsPgWeekModel.SearchKey;
				string StartDay     = statisticsPgWeekModel.SearchStartDay;
				string EndDay       = statisticsPgWeekModel.SearchEndDay;

				// 쿼리생성
				sbQuery = new StringBuilder();
				sbQuery.Append("\n"
					+ "/* 컨텐츠 요일별통계    */                     \n"
                    + "DECLARE @TotPgHit int;    -- 전체 컨텐츠노출수 \n"
                    + "SET @TotPgHit    = 0;                       \n"
                    + "                                            \n"
                    + "-- 전체 광고Hit                               \n"
                    + "SELECT @TotPgHit = ISNULL(SUM(A.HitCnt),0)  \n"
                    + "  FROM dbo.SumPgDaily0 A   with(NoLock)     \n"
					+ " WHERE A.LogDay BETWEEN "+ StartDay  + "    \n"
					+ "                    AND "+ EndDay    + "    \n"
                    + " 	  AND A.SummaryType = 5		           \n"
                    + "                                            \n"
					+ "-- 요일별 집계                                \n"
                    + "SELECT TA.WeekOrder                         \n"
                    + "      ,TA.WeekName                          \n"
                    + "      ,ISNULL(SUM(TA.PgCnt),0) AS PgCnt     \n"
                    + "      ,CASE WHEN @TotPgHit > 0 THEN CONVERT(DECIMAL(9,2),(ISNULL(SUM(TA.PgCnt),0) / CONVERT(float,@TotPgHit)) * 100)     \n"
                    + "                               ELSE 0 END AS PgRate                                                                      \n"
                    + "      ,REPLICATE('■', CASE WHEN @TotPgHit  > 0 THEN ROUND((ISNULL(SUM(TA.PgCnt),0)/CONVERT(float,@TotPgHit) * 100),0)   \n"
                    + "                                                ELSE 0 END) AS RateBar                                                   \n"
                    + "  FROM (                                       \n"
					+ "        SELECT CASE D.week WHEN  1 THEN 7      \n"
					+ "                           WHEN  2 THEN 1      \n"
					+ "                           WHEN  3 THEN 2      \n"
					+ "                           WHEN  4 THEN 3      \n"
					+ "                           WHEN  5 THEN 4      \n"
					+ "                           WHEN  6 THEN 5      \n"
					+ "                           WHEN  7 THEN 6 END AS WeekOrder      \n"
					+ "              ,CASE D.week WHEN  1 THEN '일'      \n"
					+ "                           WHEN  2 THEN '월'      \n"
					+ "                           WHEN  3 THEN '화'      \n"
					+ "                           WHEN  4 THEN '수'      \n"
					+ "                           WHEN  5 THEN '목'      \n"
					+ "                           WHEN  6 THEN '금'      \n"
					+ "                           WHEN  7 THEN '토' END AS WeekName      \n"
					+ "              ,SUM(HitCnt) AS PgCnt      \n"
                    + "          FROM dbo.SumPgDaily0 A  with(NoLock)                  \n"

// -- 2014/10/23 - Youngil.Yi 수정 --  "
// Exception : 이 쿼리에 정의된 힌트로 인해 쿼리 프로세서에서 쿼리 계획을 생성할 수 없습니다. 힌트를 지정하거나 SET FORCEPLAN을 사용하지 않고 쿼리를 다시 전송하십시오.
// 처리를 위해서 merge를 없앰
//                   + "          INNER merge JOIN SummaryBase D with(NoLock) ON (A.LogDay      = D.LogDay)     \n"
// --

                    + "          INNER JOIN SummaryBase D with(NoLock) ON (A.LogDay      = D.LogDay)     \n"
                    + "         WHERE A.LogDay BETWEEN "+ StartDay  + " \n"
					+ "                            AND "+ EndDay    + " \n"
                    + " 		and A.SummaryType = 5		           \n"
					);

                if (!CategoryCode.Equals("00")) // 특정카테고리선택시
                {
                    sbQuery.Append(" AND A.Menu1 = " + CategoryCode + "  \n");

                    if (!GenreCode.Equals("00")) // 특정장르선택시
                    {
                        sbQuery.Append(" AND A.Menu2 = " + GenreCode + "  \n");
                    }
                }
                if (ProgramName.Trim().Length > 0)
                {
                    sbQuery.Append(" AND A.ProgKey IN (SELECT ProgramKey FROM AdTargetsHanaTV.dbo.Program with(NoLock) WHERE MediaCode   = " + MediaCode + " AND ProgramNm = '" + ProgramName + "') \n");
                }
				
				sbQuery.Append(""
					+ "         GROUP BY D.week                           \n"
					+ "       ) TA                                        \n"
					+ " GROUP BY WeekOrder, WeekName                      \n"
                    + "                                                   \n"
                    + " ORDER BY WeekOrder                                \n"
					);  

				// __DEBUG__
				_log.Debug(sbQuery.ToString());
				// __DEBUG__
				
				// 쿼리실행
				DataSet ds = new DataSet();
				_db.ExecuteQuery(ds,sbQuery.ToString());

				// 결과 DataSet의 데이터모델에 복사
				statisticsPgWeekModel.ReportDataSet = ds.Copy();

				// 결과
				statisticsPgWeekModel.ResultCnt = Utility.GetDatasetCount(statisticsPgWeekModel.ReportDataSet);

				ds.Dispose();

				// 결과코드 셋트
				statisticsPgWeekModel.ResultCD = "0000";

                // __DEBUG__
                _log.Debug("<출력정보>");
                _log.Debug("ResultCnt:[" + statisticsPgWeekModel.ResultCnt + "]");
                // __DEBUG__

                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "GetStatisticsPgWeek() End");
                _log.Debug("-----------------------------------------");
            }
            catch(Exception ex)
            {
                statisticsPgWeekModel.ResultCD = "3000";
				if(isNotTarget)
				{
					statisticsPgWeekModel.ResultDesc = "해당광고의 정보가 존재하지 않습니다.";
				}
				else if(isNotReady)
				{
					statisticsPgWeekModel.ResultDesc = "해당 기간은 데이터가 집계되지 않았습니다.";
				}
				else
				{
					statisticsPgWeekModel.ResultDesc = "요일별통계 조회중 오류가 발생하였습니다";
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

		#region 기간내 요일별평균 통계 
		/// <summary>
		///  기간내 요일별평균 통계
		/// </summary>
		/// <param name="statisticsPgWeekModel"></param>
		public void GetStatisticsPgWeekAVG(HeaderModel header, StatisticsPgWeekModel statisticsPgWeekModel)
		{
			bool isNotTarget = false; // 타겟팅정보가 입력되지않아 존재하지 않을때.
			bool isNotReady  = false; // 데이터가 집계가 되지않아 존재하지 않을때.
			try
			{
				StringBuilder sbQuery = null;

				// 데이터베이스를 OPEN한다
				_db.Open();


				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetStatisticsPgWeekAVG() Start");
				_log.Debug("-----------------------------------------");

				// 일자가 6자리 이상(yyyymmdd)이면 6자리로 만든다.
				if(statisticsPgWeekModel.SearchStartDay.Length > 6) statisticsPgWeekModel.SearchStartDay = statisticsPgWeekModel.SearchStartDay.Substring(2,6);
				if(statisticsPgWeekModel.SearchEndDay.Length   > 6) statisticsPgWeekModel.SearchEndDay   = statisticsPgWeekModel.SearchEndDay.Substring(2,6);
				
				// __DEBUG__
				_log.Debug("<입력정보>");
				_log.Debug("SearchMediaCode	   :[" + statisticsPgWeekModel.SearchMediaCode    + "]");		// 검색 매체
				_log.Debug("SearchCategoryCode :[" + statisticsPgWeekModel.SearchCategoryCode + "]");		// 검색 카테고리코드 
				_log.Debug("SearchGenreCode    :[" + statisticsPgWeekModel.SearchGenreCode    + "]");		// 검색 장르코드           
				_log.Debug("SearchKey          :[" + statisticsPgWeekModel.SearchKey          + "]");		// 검색 키(프로그램명)           
				_log.Debug("SearchStartDay     :[" + statisticsPgWeekModel.SearchStartDay     + "]");		// 검색 집계시작 일자          
				_log.Debug("SearchEndDay       :[" + statisticsPgWeekModel.SearchEndDay       + "]");		// 검색 집계종료 일자          
				// __DEBUG__

				string MediaCode    = statisticsPgWeekModel.SearchMediaCode;
				string CategoryCode = statisticsPgWeekModel.SearchCategoryCode;
				string GenreCode    = statisticsPgWeekModel.SearchGenreCode;
				string ProgramName  = statisticsPgWeekModel.SearchKey;
				string StartDay     = statisticsPgWeekModel.SearchStartDay;
				string EndDay       = statisticsPgWeekModel.SearchEndDay;

				// 쿼리생성
				sbQuery = new StringBuilder();
				sbQuery.Append("\n"
					+ "/* 컨텐츠 요일별통계    */                    \n"
					+ "DECLARE @TotPgHit int;    -- 전체 컨텐츠노출수\n"
					+ "SET @TotPgHit    = 0;                      \n"
					+ "                                           \n"
					+ "-- 전체 광고Hit                                           \n"
					+ "SELECT @TotPgHit = ISNULL(SUM(TA.PgCnt),0)              \n"
					+ "  FROM (                                                \n"
					+ "        SELECT TC.week, AVG(PgCnt) PgCnt                \n"
					+ "          FROM (                                        \n"
					+ "               SELECT LogDay, SUM(HitCnt) PgCnt         \n"
                    + "                 FROM dbo.SumPgDaily0 with(NoLock)      \n"
					+ "                WHERE LogDay BETWEEN "+ StartDay  + "   \n"
					+ "                                 AND "+ EndDay    + "   \n"
                    + " 	             AND SummaryType = 5		           \n"
					+ "                GROUP BY LogDay \n"
					+ "               ) TB \n"
					+ "               INNER JOIN SummaryBase TC with(NoLock) ON (TB.LogDay = TC.LogDay) \n"
					+ "        GROUP BY TC.week \n"
					+ "        ) TA \n"
					+ "                                                        \n"
					+ "-- 요일별 집계                               \n"
					+ "SELECT TA.WeekOrder           \n"
					+ "      ,TA.WeekName            \n"
					+ "      ,ISNULL(AVG(TA.PgCnt),0) AS PgCnt    \n"
					+ "      ,CASE WHEN @TotPgHit > 0 THEN CONVERT(DECIMAL(9,2),(ISNULL(AVG(TA.PgCnt),0) / CONVERT(float,@TotPgHit)) * 100)     \n"
					+ "                               ELSE 0 END AS PgRate                                                                      \n"
					+ "      ,REPLICATE('■', CASE WHEN @TotPgHit  > 0 THEN ROUND((ISNULL(AVG(TA.PgCnt),0)/CONVERT(float,@TotPgHit) * 100),0)   \n"
					+ "                                                ELSE 0 END) AS RateBar                                                   \n"
					+ "  FROM (                                                     \n"
					+ "        SELECT A.LogDAY                                      \n"
					+ "              ,CASE D.week WHEN  1 THEN 7                    \n"
					+ "                           WHEN  2 THEN 1                    \n"
					+ "                           WHEN  3 THEN 2                    \n"
					+ "                           WHEN  4 THEN 3                    \n"
					+ "                           WHEN  5 THEN 4                    \n"
					+ "                           WHEN  6 THEN 5                    \n"
					+ "                           WHEN  7 THEN 6 END AS WeekOrder   \n"
					+ "              ,CASE D.week WHEN  1 THEN '일'                 \n"
					+ "                           WHEN  2 THEN '월'                 \n"
					+ "                           WHEN  3 THEN '화'                 \n"
					+ "                           WHEN  4 THEN '수'                 \n"
					+ "                           WHEN  5 THEN '목'                 \n"
					+ "                           WHEN  6 THEN '금'                 \n"
					+ "                           WHEN  7 THEN '토' END AS WeekName \n"
					+ "              ,SUM(A.HitCnt) AS PgCnt \n"
                    + "          FROM dbo.SumPgDaily0 A  with(NoLock)                          \n"
                    + "               INNER merge JOIN SummaryBase D with(NoLock) ON (A.LogDay      = D.LogDay)     \n"
					+ "         WHERE A.LogDay BETWEEN "+ StartDay  + "   \n"
					+ "                            AND "+ EndDay    + "   \n"
                    + " 	      AND A.SummaryType = 5		              \n"
					);

                if (!CategoryCode.Equals("00")) // 특정카테고리선택시
                {
                    sbQuery.Append(" AND A.Menu1 = " + CategoryCode + "  \n");

                    if (!GenreCode.Equals("00")) // 특정장르선택시
                    {
                        sbQuery.Append(" AND A.Menu2 = " + GenreCode + "  \n");
                    }
                }

                if (ProgramName.Trim().Length > 0)
                {
                    sbQuery.Append(" AND A.ProgKey IN (SELECT ProgramKey FROM AdTargetsHanaTV.dbo.Program with(NoLock) WHERE MediaCode   = " + MediaCode + " AND ProgramNm = '" + ProgramName + "') \n");
                }
				
				sbQuery.Append(""
					+ "         GROUP BY A.LogDay, D.week \n"
					+ "       ) TA                                        \n"
					+ " GROUP BY WeekOrder, WeekName                      \n"
					+ "                                                   \n"
					+ " ORDER BY WeekOrder                                \n"
					);  

				// __DEBUG__
				_log.Debug(sbQuery.ToString());
				// __DEBUG__
				
				// 쿼리실행
				DataSet ds = new DataSet();
				_db.ExecuteQuery(ds,sbQuery.ToString());

				// 결과 DataSet의 데이터모델에 복사
				statisticsPgWeekModel.ReportDataSet = ds.Copy();

				// 결과
				statisticsPgWeekModel.ResultCnt = Utility.GetDatasetCount(statisticsPgWeekModel.ReportDataSet);

				ds.Dispose();

				// 결과코드 셋트
				statisticsPgWeekModel.ResultCD = "0000";

				// __DEBUG__
				_log.Debug("<출력정보>");
				_log.Debug("ResultCnt:[" + statisticsPgWeekModel.ResultCnt + "]");
				// __DEBUG__

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetStatisticsPgWeekAVG() End");
				_log.Debug("-----------------------------------------");
			}
			catch(Exception ex)
			{
				statisticsPgWeekModel.ResultCD = "3000";
				if(isNotTarget)
				{
					statisticsPgWeekModel.ResultDesc = "해당광고의 정보가 존재하지 않습니다.";
				}
				else if(isNotReady)
				{
					statisticsPgWeekModel.ResultDesc = "해당 기간은 데이터가 집계되지 않았습니다.";
				}
				else
				{
					statisticsPgWeekModel.ResultDesc = "요일별통계 조회중 오류가 발생하였습니다";
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