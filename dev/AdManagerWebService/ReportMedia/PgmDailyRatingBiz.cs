// ===============================================================================
//
// PgmDailyRatingBiz.cs
//
// 프로그램별 시청률집계 서비스 
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
 * Class Name: PgmDailyRatingBiz
 * 주요기능  : 프로그램별 시청률집계 처리 로직
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
    public class PgmDailyRatingBiz : BaseBiz
    {

		#region  생성자
        public PgmDailyRatingBiz() : base(FrameSystem.connSummaryDbString)
        {
            _log = FrameSystem.oLog;
        }
		#endregion

		#region 채널별시청률일계 조회
        /// <summary>
        /// 채널별시청률일계 조회
        /// </summary>
        /// <param name="pgmDailyRatingModel"></param>
        public void GetPgmDailyRatingReport(HeaderModel header, PgmDailyRatingModel pgmDailyRatingModel)
        {

            try
			{
				// 데이터베이스를 OPEN한다
				_db.Open();


                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "GetDailyRatingList() Start");
                _log.Debug("-----------------------------------------");

                // __DEBUG__
                _log.Debug("<입력정보>");
				_log.Debug("SearchMediaCode	:[" + pgmDailyRatingModel.SearchMediaCode + "]");		// 검색 매체
				_log.Debug("SearchDateFrom  :[" + pgmDailyRatingModel.SearchDate      + "]");		// 검색 기간시작           

                // __DEBUG__

                StringBuilder sbQuery = new StringBuilder();

                // 쿼리생성
                sbQuery.Append("\n"
					+ " DECLARE @SumHit int;                            \n"
					+ "                                                 \n"
					+ " SELECT @SumHit = SUM(HitCnt)                    \n"
                    + "   FROM SummaryPg A INNER JOIN AdTargetsHanaTV.dbo.Program   B ON (A.ProgKey    = B.ProgramKey)  \n"
					+ "  WHERE B.MediaCode =  " + pgmDailyRatingModel.SearchMediaCode + " \n"
					+ "    AND A.ProgKey > 0                            \n"
   					+ "    AND A.LogDay    = '" + pgmDailyRatingModel.SearchDate + "' \n"
					+ "                                                 \n"
					+ " SELECT A.LogDay                                 \n" 
					+ "       ,C.CategoryCode                           \n"
					+ "       ,C.CategoryName                           \n"
					+ "       ,D.GenreCode                              \n"
					+ "       ,D.GenreName                              \n"  
					+ "       ,B.Channel AS ChannelNo                   \n"
					+ "       ,B.ProgramNm, A.HitCnt                    \n"
					+ "       ,CONVERT(DECIMAL(6,3),(CONVERT(FLOAT,A.HitCnt) / @SumHit) * 100.0) AS HitRate \n"
					+ "       ,A.Hit00 ,A.Hit01 ,A.Hit02 ,A.Hit03 ,A.Hit04 ,A.Hit05 ,A.Hit06 ,A.Hit07       \n"
					+ "       ,A.Hit08 ,A.Hit09 ,A.Hit10 ,A.Hit11 ,A.Hit12 ,A.Hit13 ,A.Hit14 ,A.Hit15       \n"
					+ "       ,A.Hit16 ,A.Hit17 ,A.Hit18 ,A.Hit19 ,A.Hit20 ,A.Hit21 ,A.Hit22 ,A.Hit23       \n"
                    + "   FROM SummaryPg A INNER JOIN AdTargetsHanaTV.dbo.Program   B ON (A.ProgKey    = B.ProgramKey)          \n"
                    + "                     LEFT JOIN AdTargetsHanaTV.dbo.Category  C ON (A.Category   = C.CategoryCode)        \n"
                    + "                     LEFT JOIN AdTargetsHanaTV.dbo.Genre     D ON (A.Genre      = D.GenreCode)           \n"
					+ "  WHERE B.MediaCode = " + pgmDailyRatingModel.SearchMediaCode + " \n"
					+ "    AND A.ProgKey   > 0                                           \n"
					+ "    AND A.LogDay = '" + pgmDailyRatingModel.SearchDate        + "'\n"
 					+ " ORDER BY LogDay, CategoryCode, GenreCode, ChannelNo              \n"
					);


				// __DEBUG__
				_log.Debug(sbQuery.ToString());
				// __DEBUG__
				
				// 쿼리실행
				DataSet ds = new DataSet();
				_db.ExecuteQuery(ds, sbQuery.ToString());

				// 결과 DataSet의 장르그룹모델에 복사
				pgmDailyRatingModel.ReportDataSet = ds.Copy();

				// 결과
				pgmDailyRatingModel.ResultCnt = Utility.GetDatasetCount(pgmDailyRatingModel.ReportDataSet);
				// 결과코드 셋트
				pgmDailyRatingModel.ResultCD = "0000";

                // __DEBUG__
                _log.Debug("<출력정보>");
                _log.Debug("ResultCnt:[" + pgmDailyRatingModel.ResultCnt + "]");
                // __DEBUG__

                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "GetDailyRatingList() End");
                _log.Debug("-----------------------------------------");
            }
            catch(Exception ex)
            {
                pgmDailyRatingModel.ResultCD = "3000";
                pgmDailyRatingModel.ResultDesc = "일별시청률 조회중 오류가 발생하였습니다";
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