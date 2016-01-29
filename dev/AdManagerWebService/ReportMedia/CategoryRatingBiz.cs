// ===============================================================================
//
// CategoryRatingBiz.cs
//
// 카테고리별 시청률집계 서비스 
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
 * Class Name: CategoryRatingBiz
 * 주요기능  : 카테고리별 시청률집계 처리 로직
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
    /// DailyRatingBiz에 대한 요약 설명입니다.
    /// </summary>
    public class CategoryRatingBiz : BaseBiz
    {

		#region  생성자
        public CategoryRatingBiz() : base(FrameSystem.connSummaryDbString)
        {
            _log = FrameSystem.oLog;
        }
		#endregion

		#region 카테고리별 시청률집계
        /// <summary>
        /// 카테고리별 시청률집계
        /// </summary>
        /// <param name="categoryRating"></param>
        public void GetCategoryRating(HeaderModel header, CategoryRatingModel categoryRating)
        {

            try
			{
				// 데이터베이스를 OPEN한다
				_db.Open();


                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "GetCategoryRating() Start");
                _log.Debug("-----------------------------------------");

				// 일자가 6자리 이상이면 6자리로 만든다.
				if(categoryRating.SearchBgnDay.Length > 6) categoryRating.SearchBgnDay = categoryRating.SearchBgnDay.Substring(2,6);
				if(categoryRating.SearchEndDay.Length > 6) categoryRating.SearchEndDay = categoryRating.SearchEndDay.Substring(2,6);
				
				// __DEBUG__
                _log.Debug("<입력정보>");
				_log.Debug("SearchMediaCode	:[" + categoryRating.SearchMediaCode + "]");		// 검색 매체
				_log.Debug("SearchType      :[" + categoryRating.SearchType      + "]");		// 검색 구분           
				_log.Debug("SearchBgnDay    :[" + categoryRating.SearchBgnDay    + "]");		// 검색 집계시작일자           
				_log.Debug("SearchEndDay    :[" + categoryRating.SearchEndDay    + "]");		// 검색 집계종료일자           
                // __DEBUG__

                StringBuilder sbQuery = new StringBuilder();

                // 쿼리생성
                sbQuery.Append("\n"
					+ " -- 카테고리별 시청률집계                                \n"
					+ "                                                       \n"
					+ "DECLARE @MediaCode int                                 \n"
					+ "DECLARE @SumHit int, @SumHouse int, @SumSetop int      \n"
					+ "DECLARE @BgnDay Char(6), @EndDay Char(6)               \n"
					+ "                                                       \n"
					+ "SET @MediaCode =  " + categoryRating.SearchMediaCode + "  \n"
					+ "SET @BgnDay    = '" + categoryRating.SearchBgnDay    + "' \n"
					+ "SET @EndDay    = '" + categoryRating.SearchEndDay    + "' \n"
					+ "                                                       \n"
					+ "SELECT @SumHit   = SUM(HitCnt)       -- 전체시청횟수   \n"
					+ "      ,@SumHouse = SUM(HitHouseHold) -- 전체이용가구수 \n"
                    + "  FROM SummaryPg A INNER JOIN AdTargetsHanaTV.dbo.Program   B ON (A.ProgKey    = B.ProgramKey) \n"
					+ " WHERE B.MediaCode =  @MediaCode                       \n"
					+ "   AND A.ProgKey > 0                                   \n"
					+ "   AND A.LogDay BETWEEN @BgnDay AND @EndDay            \n"
					+ "                                                       \n"
					+ "SELECT @SumSetop = ISNULL(HouseTotal,0)   -- 가입자수  \n"  
					+ "  FROM SummaryBase                                     \n"
					+ " WHERE LogDay = (SELECT MAX(LogDay) FROM SummaryBase WHERE LogDay < @BgnDay AND HouseTotal > 0) \n"
					+ "                                                       \n"
					+ "SELECT C.CategoryCode             -- 카테고리코드      \n"
					+ "      ,C.CategoryName            -- 카테고리명        \n"
					+ "      ,CASE WHEN @SumSetop > 0 THEN CONVERT(DECIMAL(6,3),(CONVERT(FLOAT,SUM(A.HitHouseHold)) / @SumSetop) * 100.0) ELSE 0 END AS UseRate   -- 이용률     = 이용가구수 / 가입자수 * 100        \n"
					+ "      ,CASE WHEN @SumHouse > 0 THEN CONVERT(DECIMAL(6,3),(CONVERT(FLOAT,SUM(A.HitHouseHold)) / @SumHouse) * 100.0) ELSE 0 END AS UseShare  -- 이용점유율 = 이용가구수 / 전체이용가구수 * 100  \n"
					+ "	     ,SUM(A.HitHouseHold) AS HitHouse                                                                                                        -- 이용가구수                                      \n"
					+ "      ,CASE WHEN @SumHit   > 0 THEN CONVERT(DECIMAL(6,3),(CONVERT(FLOAT,SUM(A.HitCnt)) / @SumHit) * 100.0)         ELSE 0 END AS HitShare  -- 시청점유률 = 시청횟수 / 전체시청횟수 * 100      \n"
					+ "      ,SUM(A.HitCnt) AS HitCnt                                                                     -- 시청횟수                                                                                \n"
					+ "      ,CASE WHEN SUM(A.HitHouseHold) > 0 THEN CONVERT(DECIMAL(6,3),(CONVERT(FLOAT,SUM(A.HitCnt)) / SUM(A.HitHouseHold)))  ELSE 0 END AS HitFreq       -- 시청빈도   = 시청횟수 / 이용가구수   \n"
                    + "  FROM SummaryPg A INNER JOIN AdTargetsHanaTV.dbo.Program   B ON (A.ProgKey    = B.ProgramKey)    \n"
                    + "                   INNER JOIN AdTargetsHanaTV.dbo.Category  C ON (A.Category   = C.CategoryCode)  \n"
					+ " WHERE B.MediaCode = @MediaCode                                               \n"
					+ "   AND A.ProgKey   > 0                                                        \n"
					+ "   AND A.LogDay BETWEEN @BgnDay AND @EndDay                                   \n"
					+ "GROUP BY CategoryCode, CategoryName                                           \n"
					+ "ORDER BY CategoryCode                                                         \n"
					);


				// __DEBUG__
				_log.Debug(sbQuery.ToString());
				// __DEBUG__
				
				// 쿼리실행
				DataSet ds = new DataSet();
				_db.ExecuteQuery(ds, sbQuery.ToString());

				// 결과 DataSet 복사
				categoryRating.ReportDataSet = ds.Copy();

				// 결과
				categoryRating.ResultCnt = Utility.GetDatasetCount(categoryRating.ReportDataSet);
				// 결과코드 셋트
				categoryRating.ResultCD = "0000";

                // __DEBUG__
                _log.Debug("<출력정보>");
                _log.Debug("ResultCnt:[" + categoryRating.ResultCnt + "]");
                // __DEBUG__

                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "GetCategoryRating() End");
                _log.Debug("-----------------------------------------");
            }
            catch(Exception ex)
            {
                categoryRating.ResultCD = "3000";
                categoryRating.ResultDesc = "카테고리별 시청률집계 조회중 오류가 발생하였습니다";
                _log.Exception(ex);
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