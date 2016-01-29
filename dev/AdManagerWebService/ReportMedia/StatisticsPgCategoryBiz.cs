// ===============================================================================
//
// StatisticsPgCategoryBiz.cs
//
// 컨텐츠리포트 카테고리통계 서비스 
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
 * Class Name: StatisticsPgCategoryBiz
 * 주요기능  : 컨텐츠리포트 카테고리통계 처리 로직
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
    /// StatisticsPgCategoryBiz에 대한 요약 설명입니다.
    /// </summary>
    public class StatisticsPgCategoryBiz : BaseBiz
    {

        #region  생성자
        public StatisticsPgCategoryBiz()
            : base(FrameSystem.connSummaryDbString)
        {
            _log = FrameSystem.oLog;
        }
        #endregion

        #region 카테고리 통계
        /// <summary>
        ///  기간내 카테고리통계
        /// </summary>
        /// <param name="statisticsPgCategoryModel"></param>
        public void GetStatisticsPgCategory(HeaderModel header, StatisticsPgCategoryModel statisticsPgCategoryModel)
        {
            bool isNotTarget = false; // 타겟팅정보가 입력되지않아 존재하지 않을때.
            bool isNotReady = false; // 데이터가 집계가 되지않아 존재하지 않을때.
            try
            {
                StringBuilder sbQuery = null;

                // 데이터베이스를 OPEN한다
                _db.Open();


                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "GetStatisticsPgCategory() Start");
                _log.Debug("-----------------------------------------");

                // 일자가 6자리 이상(yyyymmdd)이면 6자리로 만든다.
                if (statisticsPgCategoryModel.SearchStartDay.Length > 6) statisticsPgCategoryModel.SearchStartDay = statisticsPgCategoryModel.SearchStartDay.Substring(2, 6);
                if (statisticsPgCategoryModel.SearchEndDay.Length > 6) statisticsPgCategoryModel.SearchEndDay = statisticsPgCategoryModel.SearchEndDay.Substring(2, 6);

                // __DEBUG__
                _log.Debug("<입력정보>");
                _log.Debug("SearchMediaCode	  :[" + statisticsPgCategoryModel.SearchMediaCode + "]");		// 검색 매체
                _log.Debug("SearchStartDay    :[" + statisticsPgCategoryModel.SearchStartDay + "]");		// 검색 집계시작 일자          
                _log.Debug("SearchEndDay      :[" + statisticsPgCategoryModel.SearchEndDay + "]");		// 검색 집계종료 일자          
                // __DEBUG__

                string MediaCode = statisticsPgCategoryModel.SearchMediaCode;
                string StartDay = statisticsPgCategoryModel.SearchStartDay;
                string EndDay = statisticsPgCategoryModel.SearchEndDay;

                // 쿼리생성
                sbQuery = new StringBuilder();
                sbQuery.Append("\n  /* 기간내 컨텐츠 카테고리 통계    */		    \n");
                sbQuery.Append("\n  DECLARE @TotPgHit int;    -- 전체 컨텐츠노출수	\n");
                sbQuery.Append("\n  SET @TotPgHit    = 0;                           \n");
                sbQuery.Append("\n -- 전체 컨텐츠Hit                                \n");
                sbQuery.Append("\n  SELECT @TotPgHit = ISNULL(SUM(A.HitCnt),0)      \n");
                sbQuery.Append("\n  FROM SummaryPgDaily1 A with(noLock)             \n");
                sbQuery.Append("\n  WHERE A.LogDay BETWEEN '" + StartDay + "'       \n");
                sbQuery.Append("\n                    AND '" + EndDay + "'        \n");

                sbQuery.Append("\n  -- 카테고리 집계                                \n");
                sbQuery.Append("\n  SELECT	AdTargetsHanaTV.dbo.ufnPadding('L',a.Category,5,'0') + ' ' +  b.MenuName AS Category     \n");
                sbQuery.Append("\n      ,B.MenuName AS CategoryName	        		\n");
                sbQuery.Append("\n      ,'1' AS ORDCode						        \n");
                sbQuery.Append("\n      ,'1 카테고리합계' AS ORD                    \n");
                sbQuery.Append("\n      ,'카테고리합계' AS ORDName                  \n");
                sbQuery.Append("\n      ,'' AS Title							    \n");
                sbQuery.Append("\n      ,SUM(A.HitCnt)				AS PgCnt	    \n");
                sbQuery.Append("\n      ,sum(isnull(a.PPx,0))		as PPxCnt	    \n");
                sbQuery.Append("\n      ,sum(isnull(a.Replay,0))	as RePlayCnt    \n");
                sbQuery.Append("\n		,CONVERT(DECIMAL(9,2),(sum(isnull(a.PPx,0)) / CONVERT(float,SUM(A.HitCnt))) * 100) as PPxRate                       \n");
                sbQuery.Append("\n      ,CASE WHEN @TotPgHit > 0 THEN CONVERT(DECIMAL(9,2),(ISNULL(SUM(A.HitCnt),0) / CONVERT(float,@TotPgHit)) * 100)      \n");
                sbQuery.Append("\n                               ELSE 0 END AS PgRate                                                                       \n");
                sbQuery.Append("\n      ,REPLICATE('■', CASE WHEN @TotPgHit  > 0 THEN ROUND((ISNULL(SUM(A.HitCnt),0)/CONVERT(float,@TotPgHit) * 50),0)     \n");
                sbQuery.Append("\n                                                ELSE 0 END) AS RateBar                                                    \n");
                sbQuery.Append("\n		,CONVERT(DECIMAL(9,2),(sum(isnull(a.Replay,0)) / CONVERT(float,SUM(A.HitCnt))) * 100) as RePlayRate                 \n");
                sbQuery.Append("\n  FROM  SummaryPgDaily3 A with(noLock)                                                                                    \n");
                sbQuery.Append("\n INNER JOIN AdTargetsHanaTV.dbo.Menu  B with(noLock) ON (A.Category   = B.MenuCode AND B.MenuLevel = 1 AND B.MediaCode = " + MediaCode + ")   \n");
                sbQuery.Append("\n   AND A.LogDay BETWEEN '" + StartDay + "'        \n");
                sbQuery.Append("\n                    AND '" + EndDay + "'          \n");
                sbQuery.Append("\n GROUP BY A.Category, B.MenuName                  \n");

                sbQuery.Append("\n  UNION ALL                                       \n");

                sbQuery.Append("\n  -- 장르 집계                                    \n");
                sbQuery.Append("\n  SELECT AdTargetsHanaTV.dbo.ufnPadding('L',a.Category,5,'0') + ' ' +  C.MenuName  AS Category \n");
                sbQuery.Append("\n      ,C.MenuName AS CatagoryName                 \n");
                sbQuery.Append("\n      ,'2' AS ORDCode                             \n");
                sbQuery.Append("\n      ,'2 장르' AS ORD                            \n");
                sbQuery.Append("\n      ,'장르' AS ORDName                          \n");
                sbQuery.Append("\n      ,B.MenuName AS Title                        \n");
                sbQuery.Append("\n      ,SUM(A.HitCnt)  AS PgCnt                    \n");
                sbQuery.Append("\n      ,sum(isnull(a.PPx,0))		as PPxCnt       \n");
                sbQuery.Append("\n      ,sum(isnull(a.Replay,0))	as RePlayCnt    \n");
                sbQuery.Append("\n		 ,CONVERT(DECIMAL(9,2),(sum(isnull(a.PPx,0)) / CONVERT(float,SUM(A.HitCnt))) * 100) as PPxRate                      \n");
                sbQuery.Append("\n      ,CASE WHEN @TotPgHit > 0 THEN CONVERT(DECIMAL(9,2),(ISNULL(SUM(A.HitCnt),0) / CONVERT(float,@TotPgHit)) * 100)      \n");
                sbQuery.Append("\n                               ELSE 0 END AS PgRate                                                                       \n");
                sbQuery.Append("\n     ,REPLICATE('■', CASE WHEN @TotPgHit  > 0 THEN ROUND((ISNULL(SUM(A.HitCnt),0)/CONVERT(float,@TotPgHit) * 50),0)      \n");
                sbQuery.Append("\n                                               ELSE 0 END) AS RateBar                                                     \n");
                sbQuery.Append("\n		,CONVERT(DECIMAL(9,2),(sum(isnull(a.Replay,0)) / CONVERT(float,SUM(A.HitCnt))) * 100) as RePlayRate                 \n");
                sbQuery.Append("\n  FROM SummaryPgDaily3 A with(noLock) INNER JOIN AdTargetsHanaTV.dbo.Menu  B with(noLock) ON (A.Genre    = B.MenuCode AND B.MenuLevel = 2 AND B.MediaCode = " + MediaCode + ")    \n");
                sbQuery.Append("\n                         INNER JOIN AdTargetsHanaTV.dbo.Menu  C with(noLock) ON (A.Category = C.MenuCode AND C.MenuLevel = 1 AND B.MediaCode = " + MediaCode + ")                 \n");
                sbQuery.Append("\n  WHERE A.LogDay BETWEEN '" + StartDay + "'       \n");
                sbQuery.Append("\n                   AND '" + EndDay + "'         \n");
                sbQuery.Append("\n GROUP BY A.Category, C.MenuName, B.MenuName      \n");

                sbQuery.Append("\nORDER BY Category, ORDCode, PgCnt DESC    \n");

                // __DEBUG__
                _log.Debug(sbQuery.ToString());
                // __DEBUG__

                // 쿼리실행
                DataSet ds = new DataSet();
                _db.ExecuteQuery(ds, sbQuery.ToString());

                // 결과 DataSet의 데이터모델에 복사
                statisticsPgCategoryModel.ReportDataSet = ds.Copy();

                // 결과
                statisticsPgCategoryModel.ResultCnt = Utility.GetDatasetCount(statisticsPgCategoryModel.ReportDataSet);

                ds.Dispose();

                // 결과코드 셋트
                statisticsPgCategoryModel.ResultCD = "0000";

                // __DEBUG__
                _log.Debug("<출력정보>");
                _log.Debug("ResultCnt:[" + statisticsPgCategoryModel.ResultCnt + "]");
                // __DEBUG__

                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "GetStatisticsPgCategory() End");
                _log.Debug("-----------------------------------------");
            }
            catch (Exception ex)
            {
                statisticsPgCategoryModel.ResultCD = "3000";
                if (isNotTarget)
                {
                    statisticsPgCategoryModel.ResultDesc = "해당기간의 정보가 존재하지 않습니다.";
                }
                else if (isNotReady)
                {
                    statisticsPgCategoryModel.ResultDesc = "해당 기간은 데이터가 집계되지 않았습니다.";
                }
                else
                {
                    statisticsPgCategoryModel.ResultDesc = "카테고리통계 조회중 오류가 발생하였습니다";
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