// ===============================================================================
//
// StatisticsPgTimeBiz.cs
//
// 컨텐츠리포트 시간대별통계 서비스 
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
 * Class Name: StatisticsPgTimeBiz
 * 주요기능  : 컨텐츠리포트 시간대별통계 처리 로직
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

namespace AdManagerWebService.ReportMedia
{
    /// <summary>
    /// StatisticsPgTimeBiz에 대한 요약 설명입니다.
    /// </summary>
    public class StatisticsPgTimeBiz : BaseBiz
    {

		#region  생성자
        public StatisticsPgTimeBiz() : base(FrameSystem.connSummaryDbString)
        {
            _log = FrameSystem.oLog;
        }
		#endregion

		#region 카테고리목록 조회
		/// <summary>
		/// 카테고리목록조회
		/// </summary>
		/// <param name="categoryModel"></param>
		public void GetCategoryList(HeaderModel header, StatisticsPgTimeModel statisticsPgTimeModel)
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
				_log.Debug("SearchMediaCode    :[" + statisticsPgTimeModel.SearchMediaCode + "]");
				// __DEBUG__

				StringBuilder sbQuery = new StringBuilder();

				// 쿼리생성
				sbQuery.Append("\n"
					+ " SELECT A.MenuCode AS CategoryCode, A.MenuName AS CategoryName  \n"
                    + "   FROM AdTargetsHanaTV.dbo.Menu A with(NoLock)                \n"										
					+ "  WHERE A.MediaCode = " + statisticsPgTimeModel.SearchMediaCode +"   \n"			
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
				statisticsPgTimeModel.CategoryDataSet = ds.Copy();
				// 결과
				statisticsPgTimeModel.ResultCnt = Utility.GetDatasetCount(statisticsPgTimeModel.CategoryDataSet);
				// 결과코드 셋트
				statisticsPgTimeModel.ResultCD = "0000";

				// __DEBUG__
				_log.Debug("<출력정보>");
				_log.Debug("ResultCnt:[" + statisticsPgTimeModel.ResultCnt + "]");
				// __DEBUG__

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetUsersList() End");
				_log.Debug("-----------------------------------------");
			}
			catch(Exception ex)
			{
				statisticsPgTimeModel.ResultCD = "3000";
				statisticsPgTimeModel.ResultDesc = "카테고리정보 조회중 오류가 발생하였습니다";
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
		public void GetGenreList(HeaderModel header, StatisticsPgTimeModel statisticsPgTimeModel)
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
				_log.Debug("SearchMediaCode    :[" + statisticsPgTimeModel.SearchMediaCode + "]");
				// __DEBUG__

				StringBuilder sbQuery = new StringBuilder();

				// 쿼리생성
				sbQuery.Append("\n"
					+ " SELECT A.UpperMenuCode AS CategoryCode, A.MenuCode AS GenreCode, A.MenuName AS GenreName  \n"
                    + "   FROM AdTargetsHanaTV.dbo.Menu A  with(NoLock)                \n"										
					+ "  WHERE A.MediaCode = " + statisticsPgTimeModel.SearchMediaCode +"   \n"			
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
				statisticsPgTimeModel.GenreDataSet = ds.Copy();
				// 결과
				statisticsPgTimeModel.ResultCnt = Utility.GetDatasetCount(statisticsPgTimeModel.GenreDataSet);
				// 결과코드 셋트
				statisticsPgTimeModel.ResultCD = "0000";

				// __DEBUG__
				_log.Debug("<출력정보>");
				_log.Debug("ResultCnt:[" + statisticsPgTimeModel.ResultCnt + "]");
				// __DEBUG__

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetGenreList() End");
				_log.Debug("-----------------------------------------");
			}
			catch(Exception ex)
			{
				statisticsPgTimeModel.ResultCD = "3000";
				statisticsPgTimeModel.ResultDesc = "장르정보 조회중 오류가 발생하였습니다";
				_log.Exception(ex);
			}
			finally
			{
				// 데이터베이스를  Close한다
				_db.Close();			
			}
		}

		#endregion

		#region 기간내 시간대별통계
        /// <summary>
        ///  기간내 시간대별통계
        /// </summary>
        /// <param name="statisticsPgTimeModel"></param>
        public void GetStatisticsPgTime(HeaderModel header, StatisticsPgTimeModel statisticsPgTimeModel)
        {
			bool isNotTarget = false; // 타겟팅정보가 입력되지않아 존재하지 않을때.
			bool isNotReady  = false; // 데이터가 집계가 되지않아 존재하지 않을때.
            try
			{
				StringBuilder sbQuery = null;

				// 데이터베이스를 OPEN한다
				_db.Open();


                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "GetStatisticsPgTime() Start");
                _log.Debug("-----------------------------------------");

				// 일자가 6자리 이상(yyyymmdd)이면 6자리로 만든다.
				if(statisticsPgTimeModel.SearchStartDay.Length > 6) statisticsPgTimeModel.SearchStartDay = statisticsPgTimeModel.SearchStartDay.Substring(2,6);
				if(statisticsPgTimeModel.SearchEndDay.Length   > 6) statisticsPgTimeModel.SearchEndDay   = statisticsPgTimeModel.SearchEndDay.Substring(2,6);
				
				// __DEBUG__
                _log.Debug("<입력정보>");
				_log.Debug("SearchMediaCode	   :[" + statisticsPgTimeModel.SearchMediaCode    + "]");		// 검색 매체
				_log.Debug("SearchCategoryCode :[" + statisticsPgTimeModel.SearchCategoryCode + "]");		// 검색 카테고리코드 
				_log.Debug("SearchGenreCode    :[" + statisticsPgTimeModel.SearchGenreCode    + "]");		// 검색 장르코드           
				_log.Debug("SearchKey          :[" + statisticsPgTimeModel.SearchKey          + "]");		// 검색 키(프로그램명)           
				_log.Debug("SearchStartDay     :[" + statisticsPgTimeModel.SearchStartDay     + "]");		// 검색 집계시작 일자          
				_log.Debug("SearchEndDay       :[" + statisticsPgTimeModel.SearchEndDay       + "]");		// 검색 집계종료 일자          
				// __DEBUG__

				string MediaCode    = statisticsPgTimeModel.SearchMediaCode;
				string CategoryCode = statisticsPgTimeModel.SearchCategoryCode;
				string GenreCode    = statisticsPgTimeModel.SearchGenreCode;
				string ProgramName  = statisticsPgTimeModel.SearchKey;
				string StartDay     = statisticsPgTimeModel.SearchStartDay;
				string EndDay       = statisticsPgTimeModel.SearchEndDay;

				// 쿼리생성
				sbQuery = new StringBuilder();
				sbQuery.Append("\n"
					+ "/* 컨텐츠 시간대별통계    */                 \n"
                    + "DECLARE @TotPgHit int;    -- 전체 컨텐츠노출수  \n"
                    + "SET @TotPgHit    = 0;                      \n"
                    + "                                           \n"
                    + "-- 전체 광고Hit                              \n"
                    + "SELECT @TotPgHit = ISNULL(SUM(A.HitCnt),0) \n"
                    + "  FROM dbo.SumPgDaily0 A   with(NoLock)    \n"
					+ " WHERE A.LogDay BETWEEN "+ StartDay  + "   \n"
					+ "                    AND "+ EndDay    + "   \n"
                    + "   AND A.SummaryType = 5 		          \n"
					+ "                                           \n"
					+ "-- 시간대별 집계                             \n"
                    + "SELECT TA.TimeOrder           \n"
                    + "      ,TA.TimeName            \n"
                    + "      ,ISNULL(SUM(TA.PgCnt),0) AS PgCnt    \n"
                    + "      ,CASE WHEN @TotPgHit > 0 THEN CONVERT(DECIMAL(9,2),(ISNULL(SUM(TA.PgCnt),0) / CONVERT(float,@TotPgHit)) * 100)     \n"
                    + "                               ELSE 0 END AS PgRate                                                                      \n"
                    + "      ,REPLICATE('■', CASE WHEN @TotPgHit  > 0 THEN ROUND((ISNULL(SUM(TA.PgCnt),0)/CONVERT(float,@TotPgHit) * 100),0)   \n"
                    + "                                                ELSE 0 END) AS RateBar                                                   \n"
                    + "  FROM (                                   \n"
					+ "        SELECT TC.RowNum AS TimeOrder     \n"
					+ "              ,CASE TC.RowNum WHEN  0 THEN '00시~01시'       \n"
					+ "                              WHEN  1 THEN '01시~02시'       \n"
					+ "                              WHEN  2 THEN '02시~03시'       \n"
					+ "                              WHEN  3 THEN '03시~04시'       \n"
					+ "                              WHEN  4 THEN '04시~05시'       \n"
					+ "                              WHEN  5 THEN '05시~06시'       \n"
					+ "                              WHEN  6 THEN '06시~07시'       \n"
					+ "                              WHEN  7 THEN '07시~08시'       \n"
					+ "                              WHEN  8 THEN '08시~09시'       \n"
					+ "                              WHEN  9 THEN '09시~10시'       \n"
					+ "                              WHEN 10 THEN '10시~11시'       \n"
					+ "                              WHEN 11 THEN '11시~12시'       \n"
					+ "                              WHEN 12 THEN '12시~13시'       \n"
					+ "                              WHEN 13 THEN '13시~14시'       \n"
					+ "                              WHEN 14 THEN '14시~15시'       \n"
					+ "                              WHEN 15 THEN '15시~16시'       \n"
					+ "                              WHEN 16 THEN '16시~17시'       \n"
					+ "                              WHEN 17 THEN '17시~18시'       \n"
					+ "                              WHEN 18 THEN '18시~19시'       \n"
					+ "                              WHEN 19 THEN '19시~20시'       \n"
					+ "                              WHEN 20 THEN '20시~21시'       \n"
					+ "                              WHEN 21 THEN '21시~22시'       \n"
					+ "                              WHEN 22 THEN '22시~23시'       \n"
					+ "                              WHEN 23 THEN '23시~24시' END AS TimeName  \n"
					+ "              ,CASE TC.RowNum WHEN  0 THEN SUM(H00)       \n"
					+ "                              WHEN  1 THEN SUM(H01)       \n"
					+ "                              WHEN  2 THEN SUM(H02)       \n"
					+ "                              WHEN  3 THEN SUM(H03)       \n"
					+ "                              WHEN  4 THEN SUM(H04)       \n"
					+ "                              WHEN  5 THEN SUM(H05)       \n"
					+ "                              WHEN  6 THEN SUM(H06)       \n"
					+ "                              WHEN  7 THEN SUM(H07)       \n"
					+ "                              WHEN  8 THEN SUM(H08)       \n"
					+ "                              WHEN  9 THEN SUM(H09)       \n"
					+ "                              WHEN 10 THEN SUM(H10)       \n"
					+ "                              WHEN 11 THEN SUM(H11)       \n"
					+ "                              WHEN 12 THEN SUM(H12)       \n"
					+ "                              WHEN 13 THEN SUM(H13)       \n"
					+ "                              WHEN 14 THEN SUM(H14)       \n"
					+ "                              WHEN 15 THEN SUM(H15)       \n"
					+ "                              WHEN 16 THEN SUM(H16)       \n"
					+ "                              WHEN 17 THEN SUM(H17)       \n"
					+ "                              WHEN 18 THEN SUM(H18)       \n"
					+ "                              WHEN 19 THEN SUM(H19)       \n"
					+ "                              WHEN 20 THEN SUM(H20)       \n"
					+ "                              WHEN 21 THEN SUM(H21)       \n"
					+ "                              WHEN 22 THEN SUM(H22)       \n"
					+ "                              WHEN 23 THEN SUM(H23) END PgCnt \n"
          	        + "     FROM ( \n"
			        + "		        select LogDay, SUM(H00) H00, SUM(H01) H01, SUM(H02) H02, SUM(H03) H03, SUM(H04) H04, SUM(H05) H05, SUM(H06) H06, SUM(H07) H07, SUM(H08) H08, SUM(H09) H09, SUM(H10) H10, SUM(H11) H11,  \n"
			        + "		        	SUM(H12) H12, SUM(H13) H13, SUM(H14) H14, SUM(H15) H15, SUM(H16) H16, SUM(H17) H17, SUM(H18) H18, SUM(H19) H19, SUM(H20) H20, SUM(H21) H21, SUM(H22) H22, SUM(H23) H23  \n"
			        + "		        from dbo.SumPgDaily0 AA with(nolock)  \n"
                    + "		        WHERE AA.LogDay BETWEEN " + StartDay + " AND " + EndDay + "  \n"
			        + "		        AND AA.SummaryType = 5 \n" );
                    if (!CategoryCode.Equals("00")) // 특정카테고리선택시
                    {
                        sbQuery.Append(" AND AA.Menu1 = " + CategoryCode + "  \n");

                        if (!GenreCode.Equals("00")) // 특정장르선택시
                        {
                            sbQuery.Append(" AND AA.Menu2 = " + GenreCode + "  \n");
                        }
                    }
                    if (ProgramName.Trim().Length > 0)
                    {
                        sbQuery.Append(" AND AA.ProgKey IN (SELECT ProgramKey FROM AdTargetsHanaTV.dbo.Program with(NoLock) WHERE MediaCode   = 1 AND ProgramNm = '" + ProgramName + "') \n");
                    }
				sbQuery.Append(""
                    + "		        group by AA.LogDay                    \n"
                    + "         ) A ,(select top 24 (RowNum-1)Rownum from AdTargetsHanaTV.dbo.copy_t) TC                         \n"
					+ "         GROUP BY TC.RowNum                        \n"
					+ "       ) TA                                        \n"
					+ " GROUP BY TimeOrder, TimeName                      \n"
                    + "                                                   \n"
                    + " ORDER BY TimeOrder                                \n"
					+ "                                                   \n"
					);  

				// __DEBUG__
				_log.Debug(sbQuery.ToString());
				// __DEBUG__
				
				// 쿼리실행
				DataSet ds = new DataSet();
				_db.ExecuteQuery(ds,sbQuery.ToString());

				// 결과 DataSet의 데이터모델에 복사
				statisticsPgTimeModel.ReportDataSet = ds.Copy();

				// 결과
				statisticsPgTimeModel.ResultCnt = Utility.GetDatasetCount(statisticsPgTimeModel.ReportDataSet);

				ds.Dispose();

				// 결과코드 셋트
				statisticsPgTimeModel.ResultCD = "0000";

                // __DEBUG__
                _log.Debug("<출력정보>");
                _log.Debug("ResultCnt:[" + statisticsPgTimeModel.ResultCnt + "]");
                // __DEBUG__

                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "GetStatisticsPgTime() End");
                _log.Debug("-----------------------------------------");
            }
            catch(Exception ex)
            {
                statisticsPgTimeModel.ResultCD = "3000";
				if(isNotTarget)
				{
					statisticsPgTimeModel.ResultDesc = "해당광고의 정보가 존재하지 않습니다.";
				}
				else if(isNotReady)
				{
					statisticsPgTimeModel.ResultDesc = "해당 기간은 데이터가 집계되지 않았습니다.";
				}
				else
				{
					statisticsPgTimeModel.ResultDesc = "시간대별통계 조회중 오류가 발생하였습니다";
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

		#region 기간내 시간대별평균 통계 
		/// <summary>
		///  기간내 시간대별평균 통계
		/// </summary>
		/// <param name="statisticsPgTimeModel"></param>
		public void GetStatisticsPgTimeAVG(HeaderModel header, StatisticsPgTimeModel statisticsPgTimeModel)
		{
			bool isNotTarget = false; // 타겟팅정보가 입력되지않아 존재하지 않을때.
			bool isNotReady  = false; // 데이터가 집계가 되지않아 존재하지 않을때.
			try
			{
				StringBuilder sbQuery = null;

				// 데이터베이스를 OPEN한다
				_db.Open();


				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetStatisticsPgTimeAVG() Start");
				_log.Debug("-----------------------------------------");

				// 일자가 6자리 이상(yyyymmdd)이면 6자리로 만든다.
				if(statisticsPgTimeModel.SearchStartDay.Length > 6) statisticsPgTimeModel.SearchStartDay = statisticsPgTimeModel.SearchStartDay.Substring(2,6);
				if(statisticsPgTimeModel.SearchEndDay.Length   > 6) statisticsPgTimeModel.SearchEndDay   = statisticsPgTimeModel.SearchEndDay.Substring(2,6);
				
				// __DEBUG__
				_log.Debug("<입력정보>");
				_log.Debug("SearchMediaCode	   :[" + statisticsPgTimeModel.SearchMediaCode    + "]");		// 검색 매체
				_log.Debug("SearchCategoryCode :[" + statisticsPgTimeModel.SearchCategoryCode + "]");		// 검색 카테고리코드 
				_log.Debug("SearchGenreCode    :[" + statisticsPgTimeModel.SearchGenreCode    + "]");		// 검색 장르코드           
				_log.Debug("SearchKey          :[" + statisticsPgTimeModel.SearchKey          + "]");		// 검색 키(프로그램명)           
				_log.Debug("SearchStartDay     :[" + statisticsPgTimeModel.SearchStartDay     + "]");		// 검색 집계시작 일자          
				_log.Debug("SearchEndDay       :[" + statisticsPgTimeModel.SearchEndDay       + "]");		// 검색 집계종료 일자          
				// __DEBUG__

				string MediaCode    = statisticsPgTimeModel.SearchMediaCode;
				string CategoryCode = statisticsPgTimeModel.SearchCategoryCode;
				string GenreCode    = statisticsPgTimeModel.SearchGenreCode;
				string ProgramName  = statisticsPgTimeModel.SearchKey;
				string StartDay     = statisticsPgTimeModel.SearchStartDay;
				string EndDay       = statisticsPgTimeModel.SearchEndDay;

				// 쿼리생성
				sbQuery = new StringBuilder();
				sbQuery.Append("\n"
					+ "/* 컨텐츠 시간대별통계    */                 \n"
					+ "DECLARE @TotPgHit int;    -- 전체 컨텐츠노출수  \n"
					+ "SET @TotPgHit    = 0;                      \n"
					+ "                                           \n"
					+ "                                           \n"
					+ "-- 전체 광고Hit                                 \n"
					+ "SELECT @TotPgHit = ISNULL(AVG(PgCnt),0)         \n"
					+ "  FROM (                                        \n"
					+ "       SELECT LogDay,                           \n"
					+ "              SUM(HitCnt) PgCnt                 \n"
                    + "         FROM dbo.SumPgDaily0  with(NoLock)     \n"
					+ "        WHERE LogDay BETWEEN "+ StartDay  + "   \n"
					+ "                         AND "+ EndDay    + "   \n"
                    + " 	     AND SummaryType = 5		           \n"
					+ "        GROUP BY LogDay                         \n"
					+ "        ) TA                                    \n"
					+ "                                                \n"
					+ "-- 시간대별 집계                                  \n"
					+ "SELECT TA.TimeOrder           \n"
					+ "      ,TA.TimeName            \n"
					+ "      ,ISNULL(AVG(TA.PgCnt),0) AS PgCnt    \n"
					+ "      ,CASE WHEN @TotPgHit > 0 THEN CONVERT(DECIMAL(9,2),(ISNULL(AVG(TA.PgCnt),0) / CONVERT(float,@TotPgHit)) * 100)     \n"
					+ "                               ELSE 0 END AS PgRate                                                                      \n"
					+ "      ,REPLICATE('■', CASE WHEN @TotPgHit  > 0 THEN ROUND((ISNULL(AVG(TA.PgCnt),0)/CONVERT(float,@TotPgHit) * 100),0)   \n"
					+ "                                                ELSE 0 END) AS RateBar                                                   \n"
					+ "  FROM (                                                     \n"
					+ "        SELECT A.LogDAY                                      \n"
					+ "              ,TC.RowNum AS TimeOrder                        \n"
					+ "              ,CASE TC.RowNum WHEN  0 THEN '00시~01시'       \n"
					+ "                              WHEN  1 THEN '01시~02시'       \n"
					+ "                              WHEN  2 THEN '02시~03시'       \n"
					+ "                              WHEN  3 THEN '03시~04시'       \n"
					+ "                              WHEN  4 THEN '04시~05시'       \n"
					+ "                              WHEN  5 THEN '05시~06시'       \n"
					+ "                              WHEN  6 THEN '06시~07시'       \n"
					+ "                              WHEN  7 THEN '07시~08시'       \n"
					+ "                              WHEN  8 THEN '08시~09시'       \n"
					+ "                              WHEN  9 THEN '09시~10시'       \n"
					+ "                              WHEN 10 THEN '10시~11시'       \n"
					+ "                              WHEN 11 THEN '11시~12시'       \n"
					+ "                              WHEN 12 THEN '12시~13시'       \n"
					+ "                              WHEN 13 THEN '13시~14시'       \n"
					+ "                              WHEN 14 THEN '14시~15시'       \n"
					+ "                              WHEN 15 THEN '15시~16시'       \n"
					+ "                              WHEN 16 THEN '16시~17시'       \n"
					+ "                              WHEN 17 THEN '17시~18시'       \n"
					+ "                              WHEN 18 THEN '18시~19시'       \n"
					+ "                              WHEN 19 THEN '19시~20시'       \n"
					+ "                              WHEN 20 THEN '20시~21시'       \n"
					+ "                              WHEN 21 THEN '21시~22시'       \n"
					+ "                              WHEN 22 THEN '22시~23시'       \n"
					+ "                              WHEN 23 THEN '23시~24시' END AS TimeName  \n"
					+ "              ,CASE TC.RowNum WHEN  0 THEN SUM(H00)       \n"
					+ "                              WHEN  1 THEN SUM(H01)       \n"
					+ "                              WHEN  2 THEN SUM(H02)       \n"
					+ "                              WHEN  3 THEN SUM(H03)       \n"
					+ "                              WHEN  4 THEN SUM(H04)       \n"
					+ "                              WHEN  5 THEN SUM(H05)       \n"
					+ "                              WHEN  6 THEN SUM(H06)       \n"
					+ "                              WHEN  7 THEN SUM(H07)       \n"
					+ "                              WHEN  8 THEN SUM(H08)       \n"
					+ "                              WHEN  9 THEN SUM(H09)       \n"
					+ "                              WHEN 10 THEN SUM(H10)       \n"
					+ "                              WHEN 11 THEN SUM(H11)       \n"
					+ "                              WHEN 12 THEN SUM(H12)       \n"
					+ "                              WHEN 13 THEN SUM(H13)       \n"
					+ "                              WHEN 14 THEN SUM(H14)       \n"
					+ "                              WHEN 15 THEN SUM(H15)       \n"
					+ "                              WHEN 16 THEN SUM(H16)       \n"
					+ "                              WHEN 17 THEN SUM(H17)       \n"
					+ "                              WHEN 18 THEN SUM(H18)       \n"
					+ "                              WHEN 19 THEN SUM(H19)       \n"
					+ "                              WHEN 20 THEN SUM(H20)       \n"
					+ "                              WHEN 21 THEN SUM(H21)       \n"
					+ "                              WHEN 22 THEN SUM(H22)       \n"
					+ "                              WHEN 23 THEN SUM(H23) END PgCnt \n"
          	        + "     FROM ( \n"
			        + "		        select LogDay, SUM(H00) H00, SUM(H01) H01, SUM(H02) H02, SUM(H03) H03, SUM(H04) H04, SUM(H05) H05, SUM(H06) H06, SUM(H07) H07, SUM(H08) H08, SUM(H09) H09, SUM(H10) H10, SUM(H11) H11,  \n"
			        + "		        	SUM(H12) H12, SUM(H13) H13, SUM(H14) H14, SUM(H15) H15, SUM(H16) H16, SUM(H17) H17, SUM(H18) H18, SUM(H19) H19, SUM(H20) H20, SUM(H21) H21, SUM(H22) H22, SUM(H23) H23  \n"
			        + "		        from dbo.SumPgDaily0 AA with(nolock)  \n"
                    + "		        WHERE AA.LogDay BETWEEN " + StartDay + " AND " + EndDay + "  \n"
			        + "		        AND AA.SummaryType = 5 \n" );
                    if (!CategoryCode.Equals("00")) // 특정카테고리선택시
                    {
                        sbQuery.Append(" AND AA.Menu1 = " + CategoryCode + "  \n");

                        if (!GenreCode.Equals("00")) // 특정장르선택시
                        {
                            sbQuery.Append(" AND AA.Menu2 = " + GenreCode + "  \n");
                        }
                    }
                    if (ProgramName.Trim().Length > 0)
                    {
                        sbQuery.Append(" AND AA.ProgKey IN (SELECT ProgramKey FROM AdTargetsHanaTV.dbo.Program with(NoLock) WHERE MediaCode   = 1 AND ProgramNm = '" + ProgramName + "') \n");
                    }
				
				sbQuery.Append(""
                    + "		        group by AA.LogDay                    \n"
                    + "         ) A ,(select top 24 (RowNum-1)Rownum from AdTargetsHanaTV.dbo.copy_t) TC                         \n"
					+ "         GROUP BY LogDay, TC.RowNum                \n"
					+ "       ) TA                                        \n"
					+ " GROUP BY TimeOrder, TimeName                      \n"
					+ "                                                   \n"
					+ " ORDER BY TimeOrder                                \n"
					+ "                                                   \n"
					);

				// __DEBUG__
				_log.Debug(sbQuery.ToString());
				// __DEBUG__
				
				// 쿼리실행
				DataSet ds = new DataSet();
				_db.ExecuteQuery(ds,sbQuery.ToString());

				// 결과 DataSet의 데이터모델에 복사
				statisticsPgTimeModel.ReportDataSet = ds.Copy();

				// 결과
				statisticsPgTimeModel.ResultCnt = Utility.GetDatasetCount(statisticsPgTimeModel.ReportDataSet);

				ds.Dispose();

				// 결과코드 셋트
				statisticsPgTimeModel.ResultCD = "0000";

				// __DEBUG__
				_log.Debug("<출력정보>");
				_log.Debug("ResultCnt:[" + statisticsPgTimeModel.ResultCnt + "]");
				// __DEBUG__

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetStatisticsPgTimeAVG() End");
				_log.Debug("-----------------------------------------");
			}
			catch(Exception ex)
			{
				statisticsPgTimeModel.ResultCD = "3000";
				if(isNotTarget)
				{
					statisticsPgTimeModel.ResultDesc = "해당광고의 정보가 존재하지 않습니다.";
				}
				else if(isNotReady)
				{
					statisticsPgTimeModel.ResultDesc = "해당 기간은 데이터가 집계되지 않았습니다.";
				}
				else
				{
					statisticsPgTimeModel.ResultDesc = "시간대별통계 조회중 오류가 발생하였습니다";
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