// ===============================================================================
//
// StatisticsPgBiz.cs
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
 * Class Name: StatisticsPgBiz
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
    /// StatisticsPgBiz에 대한 요약 설명입니다.
    /// </summary>
    public class StatisticsPgBiz : BaseBiz
    {

        #region  생성자
        public StatisticsPgBiz()
            : base(FrameSystem.connSummaryDbString)
        {
            _log = FrameSystem.oLog;
        }
        #endregion

        #region 카테고리목록 조회
        /// <summary>
        /// 카테고리목록조회
        /// </summary>
        /// <param name="categoryModel"></param>
        public void GetCategoryList(HeaderModel header, StatisticsPgModel statisticsPgModel)
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
                _log.Debug("SearchMediaCode    :[" + statisticsPgModel.SearchMediaCode + "]");
                // __DEBUG__

                StringBuilder sbQuery = new StringBuilder();

                // 쿼리생성
                sbQuery.Append("\n"
                    + " SELECT A.MenuCode AS CategoryCode, A.MenuName AS CategoryName  \n"
                    + "   FROM AdTargetsHanaTV.dbo.Menu A with(NoLock)                \n"
                    + "  WHERE A.MediaCode = " + statisticsPgModel.SearchMediaCode + "   \n"
                    + "    AND A.MenuLevel = 1        \n"
                    + "  ORDER BY A.MenuCode          \n"
                    );

                // __DEBUG__
                _log.Debug(sbQuery.ToString());
                // __DEBUG__

                // 쿼리실행
                DataSet ds = new DataSet();
                _db.ExecuteQuery(ds, sbQuery.ToString());

                // 결과 DataSet의 카테고리모델에 복사
                statisticsPgModel.CategoryDataSet = ds.Copy();
                // 결과
                statisticsPgModel.ResultCnt = Utility.GetDatasetCount(statisticsPgModel.CategoryDataSet);
                // 결과코드 셋트
                statisticsPgModel.ResultCD = "0000";

                // __DEBUG__
                _log.Debug("<출력정보>");
                _log.Debug("ResultCnt:[" + statisticsPgModel.ResultCnt + "]");
                // __DEBUG__

                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "GetUsersList() End");
                _log.Debug("-----------------------------------------");
            }
            catch (Exception ex)
            {
                statisticsPgModel.ResultCD = "3000";
                statisticsPgModel.ResultDesc = "카테고리정보 조회중 오류가 발생하였습니다";
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
        public void GetGenreList(HeaderModel header, StatisticsPgModel statisticsPgModel)
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
                _log.Debug("SearchMediaCode    :[" + statisticsPgModel.SearchMediaCode + "]");
                // __DEBUG__

                StringBuilder sbQuery = new StringBuilder();

                // 쿼리생성
                sbQuery.Append("\n"
                    + " SELECT A.UpperMenuCode AS CategoryCode, A.MenuCode AS GenreCode, A.MenuName AS GenreName  \n"
                    + "   FROM AdTargetsHanaTV.dbo.Menu A  with(NoLock)                \n"
                    + "  WHERE A.MediaCode = " + statisticsPgModel.SearchMediaCode + "   \n"
                    + "    AND A.MenuLevel = 2        \n"
                    + "  ORDER BY A.UpperMenuCode, A.MenuCode \n"
                    );

                // __DEBUG__
                _log.Debug(sbQuery.ToString());
                // __DEBUG__

                // 쿼리실행
                DataSet ds = new DataSet();
                _db.ExecuteQuery(ds, sbQuery.ToString());

                // 결과 DataSet의 모델에 복사
                statisticsPgModel.GenreDataSet = ds.Copy();
                // 결과
                statisticsPgModel.ResultCnt = Utility.GetDatasetCount(statisticsPgModel.GenreDataSet);
                // 결과코드 셋트
                statisticsPgModel.ResultCD = "0000";

                // __DEBUG__
                _log.Debug("<출력정보>");
                _log.Debug("ResultCnt:[" + statisticsPgModel.ResultCnt + "]");
                // __DEBUG__

                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "GetGenreList() End");
                _log.Debug("-----------------------------------------");
            }
            catch (Exception ex)
            {
                statisticsPgModel.ResultCD = "3000";
                statisticsPgModel.ResultDesc = "장르정보 조회중 오류가 발생하였습니다";
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
        /// <param name="StatisticsPgModel"></param>
        public void GetStatisticsPgAge(HeaderModel header, StatisticsPgModel statisticsPgModel)
        {
            bool isNotTarget = false; // 타겟팅정보가 입력되지않아 존재하지 않을때.
            bool isNotReady = false; // 데이터가 집계가 되지않아 존재하지 않을때.
            try
            {
                StringBuilder sbQuery = null;

                // 데이터베이스를 OPEN한다
                _db.Open();


                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "GetStatisticsPgWeek() Start");
                _log.Debug("-----------------------------------------");

                // 일자가 6자리 이상(yyyymmdd)이면 6자리로 만든다.
                if (statisticsPgModel.SearchStartDay.Length > 6) statisticsPgModel.SearchStartDay = statisticsPgModel.SearchStartDay.Substring(2, 6);
                if (statisticsPgModel.SearchEndDay.Length > 6) statisticsPgModel.SearchEndDay = statisticsPgModel.SearchEndDay.Substring(2, 6);

                // __DEBUG__
                _log.Debug("<입력정보>");
                _log.Debug("SearchMediaCode	   :[" + statisticsPgModel.SearchMediaCode + "]");		// 검색 매체
                _log.Debug("SearchCategoryCode :[" + statisticsPgModel.SearchCategoryCode + "]");		// 검색 카테고리코드 
                _log.Debug("SearchGenreCode    :[" + statisticsPgModel.SearchGenreCode + "]");		// 검색 장르코드           
                _log.Debug("SearchKey          :[" + statisticsPgModel.SearchKey + "]");		// 검색 키(프로그램명)           
                _log.Debug("SearchStartDay     :[" + statisticsPgModel.SearchStartDay + "]");		// 검색 집계시작 일자          
                _log.Debug("SearchEndDay       :[" + statisticsPgModel.SearchEndDay + "]");		// 검색 집계종료 일자          
                // __DEBUG__

                string MediaCode = statisticsPgModel.SearchMediaCode;
                string CategoryCode = statisticsPgModel.SearchCategoryCode;
                string GenreCode = statisticsPgModel.SearchGenreCode;
                string ProgramName = statisticsPgModel.SearchKey;
                string StartDay = statisticsPgModel.SearchStartDay;
                string EndDay = statisticsPgModel.SearchEndDay;

                // 쿼리생성
                sbQuery = new StringBuilder();
                sbQuery.Append("\n"
                    + " /* 컨텐츠 연령별통계    */                          \n"
                    + " DECLARE @TotPgHit int;    -- 전체 컨텐츠노출수      \n"
                    + " SET @TotPgHit    = 0		                     \n"

                    + " -- 전체 광고Hit                              	     \n"
                    + " SELECT @TotPgHit = ISNULL(SUM(A.HitCnt),0)  	 \n"
                    + "   FROM dbo.SumPgDaily0 A   with(NoLock)          \n"
                    + "  WHERE A.LogDay BETWEEN " + StartDay + " AND " + EndDay + " 		\n"
                    + "    AND A.SummaryType = '3' 		        \n"

                    + " select  		                        \n"
                    + " 	SummaryName, sum(HitCnt) PgCnt		\n"
                    + "    ,CASE WHEN @TotPgHit > 0 THEN CONVERT(DECIMAL(9,2),(ISNULL(sum(HitCnt),0) / CONVERT(float,@TotPgHit)) * 100)     	\n"
                    + "                            ELSE 0 END AS PgRate                                                                      	\n"
                    + "    ,REPLICATE('■', CASE WHEN @TotPgHit  > 0 THEN ROUND((ISNULL(sum(HitCnt),0)/CONVERT(float,@TotPgHit) * 100),0)   		\n"
                    + "                                             ELSE 0 END) AS RateBar	\n"
                    + "  from (		                                                        \n"
                    + " 		select A.SummaryCode, SUM(A.HitCnt)HitCnt, S.SummaryName    \n"
                    + " 		   from dbo.SumPgDaily0 A with(nolock)		                \n"
                    + " 		   inner join summaryCode S with(nolock) on (S.SummaryCode = A.SummaryCode and S.SummaryType = '3')           	        \n"
                    + " 		where A.LogDay BETWEEN " + StartDay + " AND " + EndDay + " 		\n"
                    + " 		and A.SummaryType = '3'		                                    \n"
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

                sbQuery.Append("  "
                    + " 		group by A.SummaryCode, S.SummaryName		                        \n"
                    + " )AA		                                                                    \n"
                    + " group by SummaryName		                                                \n"
                    );

                // __DEBUG__
                _log.Debug(sbQuery.ToString());
                // __DEBUG__

                // 쿼리실행
                DataSet ds = new DataSet();
                _db.ExecuteQuery(ds, sbQuery.ToString());

                // 결과 DataSet의 데이터모델에 복사
                statisticsPgModel.ReportDataSet = ds.Copy();

                // 결과
                statisticsPgModel.ResultCnt = Utility.GetDatasetCount(statisticsPgModel.ReportDataSet);

                ds.Dispose();

                // 결과코드 셋트
                statisticsPgModel.ResultCD = "0000";

                // __DEBUG__
                _log.Debug("<출력정보>");
                _log.Debug("ResultCnt:[" + statisticsPgModel.ResultCnt + "]");
                // __DEBUG__

                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "GetStatisticsPgWeek() End");
                _log.Debug("-----------------------------------------");
            }
            catch (Exception ex)
            {
                statisticsPgModel.ResultCD = "3000";
                if (isNotTarget)
                {
                    statisticsPgModel.ResultDesc = "해당광고의 정보가 존재하지 않습니다.";
                }
                else if (isNotReady)
                {
                    statisticsPgModel.ResultDesc = "해당 기간은 데이터가 집계되지 않았습니다.";
                }
                else
                {
                    statisticsPgModel.ResultDesc = "요일별통계 조회중 오류가 발생하였습니다";
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
        /// <param name="StatisticsPgModel"></param>
        public void GetStatisticsPgAgeAVG(HeaderModel header, StatisticsPgModel statisticsPgModel)
        {
            bool isNotTarget = false; // 타겟팅정보가 입력되지않아 존재하지 않을때.
            bool isNotReady = false; // 데이터가 집계가 되지않아 존재하지 않을때.
            try
            {
                StringBuilder sbQuery = null;

                // 데이터베이스를 OPEN한다
                _db.Open();


                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "GetStatisticsPgWeekAVG() Start");
                _log.Debug("-----------------------------------------");

                // 일자가 6자리 이상(yyyymmdd)이면 6자리로 만든다.
                if (statisticsPgModel.SearchStartDay.Length > 6) statisticsPgModel.SearchStartDay = statisticsPgModel.SearchStartDay.Substring(2, 6);
                if (statisticsPgModel.SearchEndDay.Length > 6) statisticsPgModel.SearchEndDay = statisticsPgModel.SearchEndDay.Substring(2, 6);

                // __DEBUG__
                _log.Debug("<입력정보>");
                _log.Debug("SearchMediaCode	   :[" + statisticsPgModel.SearchMediaCode + "]");		// 검색 매체
                _log.Debug("SearchCategoryCode :[" + statisticsPgModel.SearchCategoryCode + "]");		// 검색 카테고리코드 
                _log.Debug("SearchGenreCode    :[" + statisticsPgModel.SearchGenreCode + "]");		// 검색 장르코드           
                _log.Debug("SearchKey          :[" + statisticsPgModel.SearchKey + "]");		// 검색 키(프로그램명)           
                _log.Debug("SearchStartDay     :[" + statisticsPgModel.SearchStartDay + "]");		// 검색 집계시작 일자          
                _log.Debug("SearchEndDay       :[" + statisticsPgModel.SearchEndDay + "]");		// 검색 집계종료 일자          
                // __DEBUG__

                string MediaCode = statisticsPgModel.SearchMediaCode;
                string CategoryCode = statisticsPgModel.SearchCategoryCode;
                string GenreCode = statisticsPgModel.SearchGenreCode;
                string ProgramName = statisticsPgModel.SearchKey;
                string StartDay = statisticsPgModel.SearchStartDay;
                string EndDay = statisticsPgModel.SearchEndDay;

                // 쿼리생성
                sbQuery = new StringBuilder();
                sbQuery.Append("\n"
                    + " /* 컨텐츠 연령별통계    */                          \n"
                    + " DECLARE @TotPgHit int;    -- 전체 컨텐츠노출수      \n"
                    + " SET @TotPgHit    = 0		                     \n"

                    + " -- 전체 광고Hit                              	     \n"
                    + "  SELECT @TotPgHit = ISNULL(SUM(C.avgData),0)         \n"      
                    + "   FROM (   \n"
                    + " 	select SummaryCode, avg(sumData)avgData, sum(sumData)sumData from ( \n"
                    + " 			SELECT A.LogDay, A.SummaryCode, ISNULL(SUM(A.HitCnt),0)sumData \n"
                    + " 			FROM dbo.SumPgDaily0 A   with(NoLock) \n"
                    + " 			WHERE A.LogDay BETWEEN " + StartDay + " AND " + EndDay + " 		\n"
                    + " 			AND A.SummaryType = '3' 		         \n"
                    + " 			group by A.LogDay, A.SummaryCode \n"
                    + " 	)B \n"
                    + " 	group by B.SummaryCode \n"
                    + " )C \n"

                    + " select  		                        \n"
                    + " 	SummaryName, AVG(HitCnt) PgCnt		\n"
                    + "    ,CASE WHEN @TotPgHit > 0 THEN CONVERT(DECIMAL(9,2),(ISNULL(AVG(HitCnt),0) / CONVERT(float,@TotPgHit)) * 100)     	\n"
                    + "                            ELSE 0 END AS PgRate                                                                      	\n"
                    + "    ,REPLICATE('■', CASE WHEN @TotPgHit  > 0 THEN ROUND((ISNULL(AVG(HitCnt),0)/CONVERT(float,@TotPgHit) * 100),0)   		\n"
                    + "                                             ELSE 0 END) AS RateBar		\n"
                    + "  from (		                                                            \n"
                    + " 		select A.SummaryCode, SUM(A.HitCnt)HitCnt, S.SummaryName        \n"
                    + " 		   from dbo.SumPgDaily0 A with(nolock)		                \n"
                    + " 		   inner join summaryCode S with(nolock) on (S.SummaryCode = A.SummaryCode and S.SummaryType = '3')           	\n"
                    + " 		where A.LogDay BETWEEN " + StartDay + " AND " + EndDay + " 		\n"
                    + " 		and A.SummaryType = '3'		                                        \n"
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

                sbQuery.Append("  "
                    + " 		group by A.LogDay, A.SummaryCode, S.SummaryName		                \n"
                    + " )AA		                                                                    \n"
                    + " group by SummaryCode, SummaryName		                                    \n"
                    );

                // __DEBUG__
                _log.Debug(sbQuery.ToString());
                // __DEBUG__

                // 쿼리실행
                DataSet ds = new DataSet();
                _db.ExecuteQuery(ds, sbQuery.ToString());

                // 결과 DataSet의 데이터모델에 복사
                statisticsPgModel.ReportDataSet = ds.Copy();

                // 결과
                statisticsPgModel.ResultCnt = Utility.GetDatasetCount(statisticsPgModel.ReportDataSet);

                ds.Dispose();

                // 결과코드 셋트
                statisticsPgModel.ResultCD = "0000";

                // __DEBUG__
                _log.Debug("<출력정보>");
                _log.Debug("ResultCnt:[" + statisticsPgModel.ResultCnt + "]");
                // __DEBUG__

                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "GetStatisticsPgWeekAVG() End");
                _log.Debug("-----------------------------------------");
            }
            catch (Exception ex)
            {
                statisticsPgModel.ResultCD = "3000";
                if (isNotTarget)
                {
                    statisticsPgModel.ResultDesc = "해당광고의 정보가 존재하지 않습니다.";
                }
                else if (isNotReady)
                {
                    statisticsPgModel.ResultDesc = "해당 기간은 데이터가 집계되지 않았습니다.";
                }
                else
                {
                    statisticsPgModel.ResultDesc = "요일별통계 조회중 오류가 발생하였습니다";
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