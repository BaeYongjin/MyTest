// ===============================================================================
//
// StatisticsPgChannelBiz.cs
//
// 컨텐츠리포트 채널통계 서비스 
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
 * Class Name: StatisticsPgChannelBiz
 * 주요기능  : 컨텐츠리포트 채널통계 처리 로직
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
    /// StatisticsPgChannelBiz에 대한 요약 설명입니다.
    /// </summary>
    public class StatisticsPgChannelBiz : BaseBiz
    {

        #region  생성자
        public StatisticsPgChannelBiz()
            : base(FrameSystem.connSummaryDbString)
        {
            _log = FrameSystem.oLog;
        }
        #endregion

        #region 채널 통계
        /// <summary>
        ///  기간내 채널통계
        /// </summary>
        /// <param name="statisticsPgChannelModel"></param>
        public void GetStatisticsPgChannel(HeaderModel header, StatisticsPgChannelModel statisticsPgChannelModel)
        {
            bool isNotTarget = false; // 타겟팅정보가 입력되지않아 존재하지 않을때.
            bool isNotReady = false; // 데이터가 집계가 되지않아 존재하지 않을때.
            try
            {
                StringBuilder sbQuery = null;

                // 데이터베이스를 OPEN한다
                _db.Open();


                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "GetStatisticsPgChannel() Start");
                _log.Debug("-----------------------------------------");

                // 일자가 6자리 이상(yyyymmdd)이면 6자리로 만든다.
                if (statisticsPgChannelModel.SearchStartDay.Length > 6) statisticsPgChannelModel.SearchStartDay = statisticsPgChannelModel.SearchStartDay.Substring(2, 6);
                if (statisticsPgChannelModel.SearchEndDay.Length > 6) statisticsPgChannelModel.SearchEndDay = statisticsPgChannelModel.SearchEndDay.Substring(2, 6);

                // __DEBUG__
                _log.Debug("<입력정보>");
                _log.Debug("SearchMediaCode	  :[" + statisticsPgChannelModel.SearchMediaCode + "]");		// 검색 매체
                _log.Debug("SearchStartDay    :[" + statisticsPgChannelModel.SearchStartDay + "]");		// 검색 집계시작 일자          
                _log.Debug("SearchEndDay      :[" + statisticsPgChannelModel.SearchEndDay + "]");		// 검색 집계종료 일자          
                // __DEBUG__

                string MediaCode = statisticsPgChannelModel.SearchMediaCode;
                string StartDay = statisticsPgChannelModel.SearchStartDay;
                string EndDay = statisticsPgChannelModel.SearchEndDay;

                // 쿼리생성
                sbQuery = new StringBuilder();
                sbQuery.Append("\n"
                    + "/* 기간내 컨텐츠 채널 통계    */           \n"
                    + "DECLARE @TotPgHit int;    -- 전체 컨텐츠노출수  \n"
                    + "SET @TotPgHit    = 0;                      \n"
                    + "                                           \n"
                    + "-- 전체 컨텐츠Hit                          \n"
                    + "SELECT @TotPgHit = ISNULL(SUM(A.HitCnt),0) \n"
                    + "  FROM SummaryPgDaily3 A with(noLock)      \n"
                    + " WHERE A.LogDay BETWEEN '" + StartDay + "' \n"
                    + "                    AND '" + EndDay + "' \n"
                    + "                                           \n"
                    + "-- 채널 집계                               \n"
                    + "	SELECT   ROW_NUMBER() OVER(ORDER BY SUM(A.HitCnt) DESC ) AS Rank \n"
                    + "			,Category \n"
                    + "			,AdTargetsHanaTV.dbo.ufnGetCategoryName(1,Category) as CategoryNm \n"
                    + "			,Genre \n"
                    + "			,AdTargetsHanaTV.dbo.ufnGetGenreName(1,Genre) as GenreNm \n"
                    + "			,ProgKey \n"
                    + "			,( select top 1 b.ProgramNm from AdTargetsHanaTV.dbo.Program b with(noLock) where b.ProgramKey = a.ProgKey ) as ProgramNm \n"
                    + "			,Sum(isnull(HitCnt,0))	as PgCnt \n"
                    + "			,CONVERT(DECIMAL(9,2),(ISNULL(SUM(A.HitCnt),0) / CONVERT(float,@TotPgHit)) * 100)		AS PgRate  \n"
                    + "			,Sum(isnull(PPx,0))		as PPxCnt \n"
                    + "         ,CONVERT(DECIMAL(9,2),(SUM(isnull(PPx,0))) / CONVERT(float,sum(a.HitCnt)) * 100 )	    AS PPxRate  \n"
                    + "			,Sum(isnull(Replay,0))	as RePlayCnt \n"
                    + "         ,CONVERT(DECIMAL(9,2),(SUM(isnull(Replay,0))) / CONVERT(float,sum(a.HitCnt)) * 100 )	AS RePlayRate  \n"
                    + "	FROM	SummaryPgDaily3 A with(noLock) \n"
                    + "	WHERE A.LogDay BETWEEN '" + StartDay + "' AND '" + EndDay + "' \n"
                    + "	group by Category,Genre,ProgKey \n"
                    + "	ORDER BY 1  \n"

//                    + "SELECT TOP 100 ROW_NUMBER() OVER(ORDER BY SUM(A.HitCnt) DESC ) AS Rank   \n"            
                    //                    + "	     ,B.ProgramNm                         \n"
                    //					+ "      ,SUM(A.HitCnt)  AS PgCnt             \n"
                    //                    + "      ,CASE WHEN @TotPgHit > 0 THEN CONVERT(DECIMAL(9,2),(ISNULL(SUM(A.HitCnt),0) / CONVERT(float,@TotPgHit)) * 100)   \n"
                    //                    + "                               ELSE 0 END AS PgRate                                                                   \n"
                    //                    + "      ,REPLICATE('■', CASE WHEN @TotPgHit  > 0 THEN ROUND((ISNULL(SUM(A.HitCnt),0)/CONVERT(float,@TotPgHit) * 100),0) \n"
                    //                    + "                                                ELSE 0 END) AS RateBar                                                \n"
                    //                    + " FROM SummaryPgDaily3 A INNER JOIN AdTargetsHanaTV.dbo.Program  B with(NoLock) ON (A.ProgKey  = B.ProgramKey AND B.MediaCode = 1)                      \n"
                    //					+ "   AND A.LogDay BETWEEN '"+ StartDay  + "' AND '"+ EndDay    + "' \n"
                    //                    + " GROUP BY B.ProgramNm                      \n"
                    //                    + "                                           \n"
                    //                    + "ORDER BY Rank, PgCnt                    \n"
                    );

                // __DEBUG__
                _log.Debug(sbQuery.ToString());
                // __DEBUG__

                // 쿼리실행
                DataSet ds = new DataSet();
                _db.ExecuteQuery(ds, sbQuery.ToString());

                // 결과 DataSet의 데이터모델에 복사
                statisticsPgChannelModel.ReportDataSet = ds.Copy();

                // 결과
                statisticsPgChannelModel.ResultCnt = Utility.GetDatasetCount(statisticsPgChannelModel.ReportDataSet);

                ds.Dispose();

                // 결과코드 셋트
                statisticsPgChannelModel.ResultCD = "0000";

                // __DEBUG__
                _log.Debug("<출력정보>");
                _log.Debug("ResultCnt:[" + statisticsPgChannelModel.ResultCnt + "]");
                // __DEBUG__

                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "GetStatisticsPgChannel() End");
                _log.Debug("-----------------------------------------");
            }
            catch (Exception ex)
            {
                statisticsPgChannelModel.ResultCD = "3000";
                if (isNotTarget)
                {
                    statisticsPgChannelModel.ResultDesc = "해당 기간의 정보가 존재하지 않습니다.";
                }
                else if (isNotReady)
                {
                    statisticsPgChannelModel.ResultDesc = "해당 기간은 데이터가 집계되지 않았습니다.";
                }
                else
                {
                    statisticsPgChannelModel.ResultDesc = "채널통계 조회중 오류가 발생하였습니다";
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