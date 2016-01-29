// ===============================================================================
//
// ProgramAdHitBiz.cs
//
// 프로그램 시청횟수집계 서비스 
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
 * Class Name: ProgramAdHitBiz
 * 주요기능  : 프로그램 시청횟수집계 처리 로직
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
    /// DailyRatingBiz에 대한 요약 설명입니다.
    /// </summary>
    public class ProgramAdHitBiz : BaseBiz
    {
		#region  생성자
        public ProgramAdHitBiz() : base(FrameSystem.connSummaryDbString)
        {
            _log = FrameSystem.oLog;
        }
		#endregion

		#region 프로그램별 시청횟수집계
        /// <summary>
        /// 프로그램별 시청횟수집계
        /// 2010/02/20일 변경 장용석
        /// </summary>
        /// <param name="programAdHitModel"></param>
        public void GetProgramAdHit(HeaderModel header, ProgramAdHitModel programAdHitModel)
        {
            try
			{
				StringBuilder sbQuery = null;

				// 데이터베이스를 OPEN한다
				_db.Open();
                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "GetProgramAdHit() Start");
                _log.Debug("-----------------------------------------");

				// 일자가 6자리 이상이면 6자리로 만든다.
				if(programAdHitModel.SearchBgnDay.Length > 6) programAdHitModel.SearchBgnDay = programAdHitModel.SearchBgnDay.Substring(2,6);
				if(programAdHitModel.SearchEndDay.Length > 6) programAdHitModel.SearchEndDay = programAdHitModel.SearchEndDay.Substring(2,6);
				
				// __DEBUG__
                _log.Debug("<입력정보>");
				_log.Debug("SearchMediaCode   :[" + programAdHitModel.SearchMediaCode   + "]");		// 검색 매체
				_log.Debug("SearchContractSeq :[" + programAdHitModel.SearchContractSeq + "]");		// 검색 계약번호           
				_log.Debug("SearchItemNo      :[" + programAdHitModel.SearchItemNo      + "]");		// 검색 구분           
				_log.Debug("SearchType        :[" + programAdHitModel.SearchType        + "]");		// 검색 구분 D:선택기간 C:계약기간(광고집행기간)        
				_log.Debug("SearchBgnDay      :[" + programAdHitModel.SearchBgnDay      + "]");		// 검색 집계시작일자           
				_log.Debug("SearchEndDay      :[" + programAdHitModel.SearchEndDay      + "]");		// 검색 집계종료일자           
                // __DEBUG__
				
				string MediaCode   = programAdHitModel.SearchMediaCode;
				string BgnDay      = programAdHitModel.SearchBgnDay;
				string EndDay      = programAdHitModel.SearchEndDay;

                int     ContractSeq = Convert.ToInt32(programAdHitModel.SearchContractSeq);
                int     CampaignCd  = Convert.ToInt32(programAdHitModel.CampaignCode);
                int     ItemNo      = Convert.ToInt32(programAdHitModel.SearchItemNo);

                // 쿼리생성
				sbQuery = new StringBuilder();

				#region 예전것
//				sbQuery.Append("\n"
//					+ "-- 프로그램별 시청집계                                                           \n"
//					+ "SELECT (SPACE(5 - LEN(CONVERT(VARCHAR(5),ad.CategoryCode)))                      \n"
//					+ "        + CONVERT(VARCHAR(10),ad.CategoryCode) + ' ' + ad.CategoryName)          \n"
//					+ "       AS CategoryCode     -- 카테고리코드                                       \n"
//					+ "      ,ad.CategoryName     -- 카테고리명                                         \n"
//					+ "      ,(SPACE(5 - LEN(CONVERT(VARCHAR(5),ad.GenreCode)))                         \n"
//					+ "        + CONVERT(VARCHAR(10),ad.GenreCode) + ' ' + ad.GenreName)                \n"
//					+ "       AS GenreCode        -- 장르코드                                           \n"
//					+ "      ,ad.GenreName        -- 장르명                                             \n"
//					+ "      ,ad.ChannelNo        -- 채널번호                                           \n"
//					+ "      ,ad.ProgramNm        -- 프로그램명                                         \n"
//					+ "      ,ad.AdHitCnt         -- 광고시청수                                         \n"
//					+ "      ,ad.PgHitCnt         -- 프로그램시청수                                     \n"
//					+ "  FROM                                                                           \n"
//					+ "  ( SELECT C.CategoryCode                                                        \n"
//					+ "          ,C.CategoryName                                                        \n"
//					+ "          ,D.GenreCode                                                           \n"
//					+ "          ,D.GenreName                                                           \n"
//					+ "          ,B.Channel AS ChannelNo                                                \n"
//					+ "          ,B.ProgramNm                                                           \n"
//					+ "          ,A.ProgKey                                                             \n"
//					//			초기엔 광고내역별,계약별로 집계를 했으나, 이후에 캠페인별 집계가 추가됨
//					//			캠페인은 다중광고내역임으로, 광고건수만큼 나눠줘야 정확한 값이 나옴, 혹은 min,max
//					+ "          ,sum(A.AdCnt)  AS AdHitCnt                                             \n"
//					+ "          ,min(A.HitCnt) AS PgHitCnt                                             \n"
//					+ "	     FROM SummaryAdDaily3		A with(NoLock)                                        \n"
//					+ "           INNER JOIN AdTargetsHanaTV.dbo.Program	B with(NoLock) ON (A.ProgKey  = B.ProgramKey)    \n"
//					+ "	          INNER JOIN AdTargetsHanaTV.dbo.Category	C with(NoLock) ON (A.Category = C.CategoryCode)  \n"
//					+ "           INNER JOIN AdTargetsHanaTV.dbo.Genre		D with(NoLock) ON (A.Genre    = D.GenreCode)     \n"
//					+ "	    WHERE B.MediaCode = " + MediaCode              + "                          \n"
//					+ "       AND A.LogDay BETWEEN '" + BgnDay + "' AND '" + EndDay + "'                \n"	);
				#endregion

                    sbQuery.Append(" SELECT (SPACE(5 - LEN(CONVERT(VARCHAR(5),ad.CategoryCode))) + CONVERT(VARCHAR(10),ad.CategoryCode) + ' ' + ad.CategoryName) AS CategoryCode \n");
                    sbQuery.Append("       ,ad.CategoryName \n");
                    sbQuery.Append("       ,(SPACE(5 - LEN(CONVERT(VARCHAR(5),ad.GenreCode))) + CONVERT(VARCHAR(10),ad.GenreCode) + ' ' + ad.GenreName) AS GenreCode \n");
                    sbQuery.Append("       ,ad.GenreName \n");
                    sbQuery.Append("       ,ad.ChannelNo \n");
                    sbQuery.Append("       ,ad.ProgramNm \n");
                    sbQuery.Append(" 			,sum(AdHitCnt)  AS AdHitCnt \n");
                    sbQuery.Append(" 			,sum(PgHitCnt) AS PgHitCnt \n");
                    sbQuery.Append(" FROM (	SELECT \n");
                    sbQuery.Append("             C.CategoryCode \n");
                    sbQuery.Append("            ,C.CategoryName \n");
                    sbQuery.Append(" 			,D.GenreCode \n");
                    sbQuery.Append(" 			,D.GenreName \n");
                    sbQuery.Append(" 			,B.Channel	AS ChannelNo \n");
                    sbQuery.Append(" 			,B.ProgramNm \n");
                    sbQuery.Append(" 			,A.ProgKey \n");
                    sbQuery.Append(" 			,A.LogDay \n");
                    sbQuery.Append(" 			,sum(A.AdCnt)  AS AdHitCnt \n");
                    sbQuery.Append("        	,min(A.HitCnt) AS PgHitCnt \n");
                    sbQuery.Append(" 		FROM SummaryAdDaily3	A with(NoLock) \n");
                    sbQuery.Append("        INNER JOIN AdTargetsHanaTV.dbo.Program	    B with(NoLock) ON (A.ProgKey  = B.ProgramKey) \n");
                    sbQuery.Append(" 	    INNER JOIN AdTargetsHanaTV.dbo.Category	    C with(NoLock) ON (A.Category = C.CategoryCode) \n");
                    sbQuery.Append("        INNER JOIN AdTargetsHanaTV.dbo.Genre        D with(NoLock) ON (A.Genre    = D.GenreCode) \n");
                    sbQuery.Append(" 		WHERE	B.MediaCode = " + MediaCode              + "                          \n");
                    sbQuery.Append(" 		AND		A.LogDay BETWEEN '" + BgnDay + "' AND '" + EndDay + "'                \n");

								#region 조회조건(계약,캠페인,내역)
								if( ItemNo > 0 )
								{
									// 광고번호를 선택했으면, 캠페인선택여부와 무관하게 1개의 광고에 대한 쿼리임
									// 캠페인이 전체이면서 광고내역이 개별이면 개별광고임
									sbQuery.Append("        AND		A.ItemNo  = " + ItemNo + "\n");
				                
								}
								else if( CampaignCd == 0 && ItemNo == 0 )
								{
									// 캠페인이 전체이면서 광고내역이 전체이면 전체 계약임
									sbQuery.Append("        AND		A.ItemNo in(select  ItemNo ");
                                    sbQuery.Append("                            from    AdTargetsHanaTV.dbo.ContractItem with(noLock)");
									sbQuery.Append("        		            where   MediaCode   = 1");
									sbQuery.Append("        					and		ContractSeq = " + ContractSeq + ")");
								}
								else if( CampaignCd > 0 && ItemNo == 0 )
								{
									// 캠페인이 선택되고, 전체광고면 해당캠패인 전체임
									sbQuery.Append("        AND     A.ItemNo in(select ItemNo");
                                    sbQuery.Append("        				    from	AdTargetsHanaTV.dbo.CampaignDetail d with(noLock)");
                                    sbQuery.Append("        					inner join AdTargetsHanaTV.dbo.CampaignMaster m with(noLock) ");
									sbQuery.Append("        							on m.CampaignCode = d.CampaignCode");
									sbQuery.Append("        							and m.MediaCode = 1");
									sbQuery.Append("        							and	m.ContractSeq	= " + ContractSeq );
									sbQuery.Append("        							and m.CampaignCode	= " + CampaignCd + ")");
								}
								#endregion

                    sbQuery.Append(" 				AND		A.ProgKey   > 0 \n");
                    sbQuery.Append(" 				GROUP BY CategoryCode, CategoryName, GenreCode, GenreName,Channel, ProgramNm ,ProgKey,LogDay ) AD \n");
                    sbQuery.Append(" GROUP BY CategoryCode, CategoryName, GenreCode, GenreName,ChannelNo, ProgramNm ,ProgKey \n");
                    sbQuery.Append(" ORDER BY ad.CategoryCode, ad.GenreCode \n");

				// __DEBUG__
				_log.Debug(sbQuery.ToString());
				// __DEBUG__
				
				// 쿼리실행
				DataSet ds = new DataSet();
				_db.ExecuteQuery(ds, sbQuery.ToString());

				// 결과 DataSet의 장르그룹모델에 복사
				programAdHitModel.ReportDataSet = ds.Copy();

				// 결과
				programAdHitModel.ResultCnt = Utility.GetDatasetCount(programAdHitModel.ReportDataSet);
				// 결과코드 셋트
				programAdHitModel.ResultCD = "0000";

                // __DEBUG__
                _log.Debug("<출력정보>");
                _log.Debug("ResultCnt:[" + programAdHitModel.ResultCnt + "]");
                // __DEBUG__

                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "GetProgramAdHit() End");
                _log.Debug("-----------------------------------------");
            }
            catch(Exception ex)
            {
                programAdHitModel.ResultCD = "3000";
                programAdHitModel.ResultDesc = "프로그램별 시청횟수집계 조회중 오류가 발생하였습니다";
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