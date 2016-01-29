// ===============================================================================
//
// DailyProgramHitBiz.cs
//
// 일별 프로그램 시청횟수집계 서비스 
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
 * Class Name: DailyProgramHitBiz
 * 주요기능  : 일별 프로그램 시청횟수집계 처리 로직
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
    /// DailyProgramHitBiz에 대한 요약 설명입니다.
    /// </summary>
    public class DailyProgramHitBiz : BaseBiz
    {
		#region  생성자
        public DailyProgramHitBiz() : base(FrameSystem.connSummaryDbString)
        {
            _log = FrameSystem.oLog;
        }
		#endregion

		#region 프로그램별 시청횟수집계
        /// <summary>
        /// 프로그램별 시청횟수집계
        /// 20100419 윤병환 수정. 캠페인이 생겼으므로 로그가 중복이 되는 현상 발생. 따라서 이부분을 중복되는 수 만큼 나누어 출력.
        /// </summary>
        /// <param name="dailyProgramHitModel"></param>
        public void GetDailyProgramHit(HeaderModel header, DailyProgramHitModel dailyProgramHitModel)
        {

            try
			{
				StringBuilder sbQuery = null;

				// 데이터베이스를 OPEN한다
				_db.Open();


                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "GetDailyProgramHit() Start");
                _log.Debug("-----------------------------------------");

				// 일자가 6자리 이상이면 6자리로 만든다.
				if(dailyProgramHitModel.SearchBgnDay.Length > 6) dailyProgramHitModel.SearchBgnDay = dailyProgramHitModel.SearchBgnDay.Substring(2,6);
				if(dailyProgramHitModel.SearchEndDay.Length > 6) dailyProgramHitModel.SearchEndDay = dailyProgramHitModel.SearchEndDay.Substring(2,6);
				
				// __DEBUG__
                _log.Debug("<입력정보>");
				_log.Debug("SearchMediaCode   :[" + dailyProgramHitModel.SearchMediaCode   + "]");		// 검색 매체
				_log.Debug("SearchContractSeq :[" + dailyProgramHitModel.SearchContractSeq + "]");		// 검색 계약번호           
				_log.Debug("SearchItemNo      :[" + dailyProgramHitModel.SearchItemNo      + "]");		// 검색 광고번호           
				_log.Debug("SearchType        :[" + dailyProgramHitModel.SearchType        + "]");		// 검색 구분 D:선택기간 C:계약기간(광고집행기간)        
				_log.Debug("SearchBgnDay      :[" + dailyProgramHitModel.SearchBgnDay      + "]");		// 검색 집계시작일자           
				_log.Debug("SearchEndDay      :[" + dailyProgramHitModel.SearchEndDay      + "]");		// 검색 집계종료일자           
                // __DEBUG__

				string  MediaCode   = dailyProgramHitModel.SearchMediaCode;
                int     ContractSeq = Convert.ToInt32(dailyProgramHitModel.SearchContractSeq);
                int     CampaignCd  = Convert.ToInt32(dailyProgramHitModel.CampaignCode);
                int     ItemNo      = Convert.ToInt32(dailyProgramHitModel.SearchItemNo);

				string  BgnDay      = dailyProgramHitModel.SearchBgnDay;
				string  EndDay      = dailyProgramHitModel.SearchEndDay;

                // 쿼리생성
				sbQuery = new StringBuilder();

                sbQuery.Append ("\n"
                    + " SELECT   x.LogDay"  + "\n"
                    + "         ,x.Category" + "\n"
                    + "         ,ct.CategoryName" + "\n"
                    + "         ,x.Genre" + "\n"
                    + "         ,gr.GenreName" + "\n"
                    + "         ,ISNULL(y.AdCnt,0) as HitCnt" + "\n"
                    + " FROM (   /* 해당기간의 일별로 카테고리-장르 생성하기, 집행않된 일자도 표시하기 위해서 */" + "\n"
                    + "			SELECT	 bs.LogDay" + "\n"
                    + "				    ,ad.Category" + "\n"
                    + "					,ad.Genre" + "\n"
                    + "         FROM    SummaryBase bs with(noLock)" + "\n"
                    + "			      , (   /* 해당기간에 집행된 카테고리-장르 구하기 */" + "\n"
                    + "                     SELECT	Category, Genre" + "\n"
                    + "                     FROM    SummaryAdDaily3 a with(NoLock)" + "\n"
                    + "						WHERE   A.LogDay  BETWEEN '" + BgnDay + "' AND '" + EndDay + "'" + "\n");
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

                sbQuery.Append("            GROUP BY a.Category,a.Genre ) ad" + "\n"
                    + "			WHERE	bs.LogDay	BETWEEN '" + BgnDay + "' AND '" + EndDay + "'" + "\n"
                    + "     ) as x" + "\n"
                    + " LEFT OUTER JOIN" + "\n"
                    + "     (   /* 일별-카테고리장르단위로 노출물량 구하기 */" + "\n"
                    + "			SELECT	 LogDay" + "\n"
                    + "			        ,Category" + "\n"
                    + "					,Genre" + "\n"
                    + "					,sum(HitCnt)/COUNT(DISTINCT ItemNo)	as AdCnt" + "\n"	//중복되는 수 만큼 나누어 줄 수 있도록 수정.
                    + "			FROM SummaryAdDaily3 a with(NoLock) " + "\n"
                    + "			WHERE A.LogDay	BETWEEN '" + BgnDay + "' AND '" + EndDay + "'" + "\n");

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
                    sbQuery.Append("        					and		ContractSeq = " + ContractSeq  + ")");
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

                sbQuery.Append(""
                    + "			GROUP BY a.LogDay,a.Category,a.Genre" + "\n"
                    + "     ) as y	ON  x.LogDay    = y.LogDay" + "\n"
                    + "		        AND x.Category	= y.Category" + "\n"
                    + " 			AND x.Genre		= y.Genre" + "\n"
                    + " INNER JOIN AdTargetsHanaTV.dbo.Category	ct	with(noLock)    on x.Category   = ct.CategoryCode" + "\n"
                    + " INNER JOIN AdTargetsHanaTV.dbo.Genre		gr	with(noLock)	on x.Genre		= gr.GenreCode" + "\n"
                    + " ORDER BY x.LogDay, x.Category, x.Genre");

				// __DEBUG__
				_log.Debug(sbQuery.ToString());
				// __DEBUG__
				
				// 쿼리실행
				DataSet ds = new DataSet();
				_db.ExecuteQuery(ds,sbQuery.ToString());

				// 결과 DataSet의 데이터모델에 복사
				dailyProgramHitModel.ReportDataSet = ds.Copy();

				// 결과
				dailyProgramHitModel.ResultCnt = Utility.GetDatasetCount(dailyProgramHitModel.ReportDataSet);

				ds.Dispose();

				// 쿼리생성
				sbQuery = new StringBuilder();
                sbQuery.Append("\n"
                    + "-- 헤더용 장르조회                                                                          \n"
                    + " SELECT  x.Category                                                              \n"
                    + "		   ,ct.CategoryName                                                         \n"
                    + "		   ,x.Genre                                                                 \n"
                    + "		   ,gr.GenreName                                                            \n"
                    + " FROM  (                                                                         \n"
                    + "         SELECT	Category, Genre                                                 \n"
                    + "         FROM SummaryAdDaily3 a with(NoLock)                                     \n"
                    + "         WHERE A.LogDay	BETWEEN '" + BgnDay + "' AND '" + EndDay + "'           \n");
                
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
                    sbQuery.Append("        					and		ContractSeq = " + ContractSeq  + ")");
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

                sbQuery.Append(""
                    + "         GROUP BY a.Category,a.Genre ) as x                                      \n"
                    + "INNER JOIN AdTargetsHanaTV.dbo.Category	ct	with(noLock)	on x.Category   = ct.CategoryCode       \n"
                    + "INNER JOIN AdTargetsHanaTV.dbo.Genre		gr	with(noLock)	on x.Genre		= gr.GenreCode          \n"
                    + "ORDER BY x.Category, x.Genre \n");


				// __DEBUG__
				_log.Debug(sbQuery.ToString());
				// __DEBUG__
				
				// 쿼리실행
				ds = new DataSet();
				_db.ExecuteQuery(ds,sbQuery.ToString());

				// 결과 DataSet의 데이터모델에 복사
				dailyProgramHitModel.HeaderDataSet = ds.Copy();

				ds.Dispose();

				// 결과코드 셋트
				dailyProgramHitModel.ResultCD = "0000";

                // __DEBUG__
                _log.Debug("<출력정보>");
                _log.Debug("ResultCnt:[" + dailyProgramHitModel.ResultCnt + "]");
                // __DEBUG__

                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "GetDailyProgramHit() End");
                _log.Debug("-----------------------------------------");
            }
            catch(Exception ex)
            {
                dailyProgramHitModel.ResultCD = "3000";
                dailyProgramHitModel.ResultDesc = "프로그램별 시청횟수집계 조회중 오류가 발생하였습니다";
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