/*
 * -------------------------------------------------------
 * Class Name: ReportBiz
 * 주요기능  : 리포팅 처리 로직
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

namespace AdManagerWebService.Interface
{
	/// <summary>
	/// ReportBiz에 대한 요약 설명입니다.
	/// </summary>
	public class ReportBiz : BaseBiz
	{
        public ReportBiz() : base(FrameSystem.connSummaryDbString)
        {
            _log = FrameSystem.oLog;
        }

        #region 일간 광고별 시청 집계

        /// <summary>
        /// 일간 광고집행 광고별 리포트
        /// </summary>
        /// <param name="mediaRep"></param>
//        public void DailyViewCnt_EachAd( int mediaRep, string viewDay )
//        {
//            try
//            {
//                StringBuilder sb = new StringBuilder();
//                sb.Append("\n");
//                sb.Append(" select   ItemNo         -- 광고번호" + "\n");
//                sb.Append("         ,ItemName       -- 광고명(소재명)" + "\n");
//                sb.Append(" 		,AdCnt          -- 당일 노출수" + "\n");
//                sb.Append(" 		,TotAdCnt       -- 누적 노출수" + "\n");
//                sb.Append(" 		,CASE WHEN ExDays <> 0 THEN FLOOR((TotAdCnt/CONVERT(float,ExDays)) * TotDaysExcu) ELSE 0 END AS ExpCntExcu -- 계약종료일기준 예상노출치" + "\n");
//                sb.Append(" 		,CASE WHEN ExDays <> 0 THEN FLOOR((TotAdCnt/CONVERT(float,ExDays)) * TotDaysReal) ELSE 0 END AS ExpCntReal -- 실제종료일기준 예상노출치" + "\n");
//                sb.Append(" 		,CASE WHEN ExDays <> 0 THEN (AdCnt * ( TotDaysExcu - ExDays) + TotAdCnt ) ELSE 0 END AS ExpCntDay   -- 당일노출기준 예상노출치" + "\n");
//                sb.Append(" 		,ContractAmt     -- 계약노출수량" + "\n");
//                sb.Append(" 		,ExcuteStartDay  -- 집행시작일" + "\n");
//                sb.Append(" 		,ExcuteEndDay    -- 집행종료일(계약상)" + "\n");
//                sb.Append(" 		,RealEndDay      -- 집행종료일(실제)" + "\n");
//                sb.Append(" 		,ExDays          -- 당일까지의 집행일수" + "\n");
//                sb.Append(" 		,TotDaysExcu     -- 계약기준 총집행일수" + "\n");
//                sb.Append(" 		,TotDaysReal     -- 실제기준 총집행일수" + "\n");
//                sb.Append(" 		,ContractSeq     -- 광고계약번호" + "\n");
//                sb.Append(" 		,ContractName    -- 광고계약명" + "\n");
//                sb.Append(" 		,AgencyName      -- 대행사명" + "\n");
//                sb.Append(" 		,AdvertiserName  -- 광고주명" + "\n");
//                // 당일집행수 기준 예상치 계산 남은일수*당일노출수 + 누적노출수
//                sb.Append(" from (  select   itm.ItemNo, itm.ItemName" + "\n");
//                sb.Append(" 		        ,( select   ISNULL(SUM(AdCnt),0)" + "\n");
//                sb.Append(" 				    from    SummaryAdDaily0 with(NoLock)" + "\n");
//                sb.Append(" 				    where   LogDay = '" + viewDay +"'" + "\n");
//                sb.Append(" 					and     ItemNo = itm.ItemNo" + "\n");
//                sb.Append(" 					and     SummaryType = 1" + "\n");
//                sb.Append(" 				  ) AS AdCnt" + "\n");
//                sb.Append(" 				,ISNULL(tgr.ContractAmt,0) AS ContractAmt" + "\n");
//                sb.Append(" 				,IsNull( (  SELECT top 1 isnull(AdCnt,0) + Isnull(AdCntAccu,0)" + "\n");
//                sb.Append(" 					        FROM    SummaryAdDaily0 with(NoLock)" + "\n");
//                sb.Append(" 					        WHERE   LogDay BETWEEN SUBSTRING(itm.ExcuteStartDay,3,6)" + "\n");
//                sb.Append(" 					                           AND CASE WHEN SUBSTRING(itm.RealEndDay,3,6) < '" + viewDay +"'" + "\n");
//                sb.Append(" 																	THEN SUBSTRING(itm.RealEndDay,3,6)" + "\n");
//                sb.Append(" 					                                    ELSE '" + viewDay +"' END" + "\n");
//                sb.Append(" 					        AND ItemNo = itm.ItemNo" + "\n");
//                sb.Append(" 					        AND SummaryType = 1" + "\n");
//                sb.Append(" 					        ORDER BY LogDay desc" + "\n");
//                sb.Append(" 					 ),0) AS TotAdCnt" + "\n");
//                sb.Append("                 ,itm.ExcuteStartDay, itm.ExcuteEndDay, itm.RealEndDay" + "\n");
//                sb.Append(" 				,DATEDIFF(day,CONVERT(datetime, itm.ExcuteStartDay, 112),CONVERT(datetime, '20" + adHitStatusModel.SearchDay +"', 112)) + 1 ExDays" + "\n");
//                sb.Append(" 				,DATEDIFF(day,CONVERT(datetime, itm.ExcuteStartDay, 112),CONVERT(datetime, itm.ExcuteEndDay, 112)) + 1 TotDaysExcu" + "\n");
//                sb.Append(" 				,DATEDIFF(day,CONVERT(datetime, itm.ExcuteStartDay, 112),CONVERT(datetime, itm.RealEndDay  , 112)) + 1 TotDaysReal" + "\n");
//                sb.Append(" 				,ctr.ContractSeq, ctr.ContractName" + "\n");
//                sb.Append(" 				,agn.AgencyName ,adv.AdvertiserName" + "\n");
//                sb.Append(" 				,itm.RapCode, itm.AgencyCode" + "\n");
//                sb.Append("         from    ContractItem itm with(nolock)" + "\n");
//                sb.Append(" 		INNER JOIN Contract     ctr with(nolock) ON (itm.ContractSeq    = ctr.ContractSeq)" + "\n");
//                sb.Append(" 		INNER JOIN Agency       agn with(nolock) ON (itm.AgencyCode     = agn.AgencyCode)" + "\n");
//                sb.Append(" 		INNER JOIN Advertiser   adv with(nolock) ON (itm.AdvertiserCode = adv.AdvertiserCode)" + "\n");
//                sb.Append(" 		LEFT  JOIN Targeting    tgr with(nolock) ON (itm.ItemNo         = tgr.ItemNo)" + "\n");
//                sb.Append(" WHERE itm.ExcuteStartDay <= '20" + viewDay +"'" + "\n");
//                sb.Append(" AND   itm.RealEndDay     >= '20" + viewDay +"'" + "\n");
//                sb.Append(" AND   itm.AdType < '90'" + "\n");
//                sb.Append("  ) T" + "\n");
//                sb.Append(" Where   T.RapCode = " + mediaRep +" \n");
//                sb.Append(" ORDER BY AdCnt DESC" + "\n");
//
//                _log.Debug("-----------------------------------------");
//                _log.Debug(this.ToString() + ".DailyViewCntEachAd() Start");
//                _log.Debug("-----------------------------------------");
//                _log.Debug("<입력정보>");
//                _log.Debug("MediaRep    :[" + mediaRep  + "]");
//                _log.Debug("ViewDay     :[" + viewDay   + "]");
//                _log.Debug(sbQuery.ToString());
//				
//                // 쿼리실행
//                DataSet ds = new DataSet();
//                _db.ExecuteQuery(ds,sbQuery.ToString());
//
//                // 결과 DataSet의 데이터모델에 복사
//                adHitStatusModel.ReportDataSet = ds.Copy();
//
//                // 결과
//                adHitStatusModel.ResultCnt = Utility.GetDatasetCount(adHitStatusModel.ReportDataSet);
//
//                ds.Dispose();
//
//
//                // 결과코드 셋트
//                adHitStatusModel.ResultCD = "0000";
//
//                // __DEBUG__
//                _log.Debug("<출력정보>");
//                _log.Debug("ResultCnt:[" + adHitStatusModel.ResultCnt + "]");
//                // __DEBUG__
//
//                _log.Debug("-----------------------------------------");
//                _log.Debug(this.ToString() + "GetAdHitStatus() End");
//                _log.Debug("-----------------------------------------");
//            }
//            catch(Exception ex)
//            {
//                adHitStatusModel.ResultCD = "3000";
//                adHitStatusModel.ResultDesc = "광고별 시청현황 집계 조회중 오류가 발생하였습니다";
//                _log.Exception(ex);
//            }
//            finally
//            {
//                // 데이터베이스를  Close한다
//                _db.Close();
//            }
//
//        }
        #endregion

        /// <summary>
        /// 프로그램별 광고시청 집계
        /// </summary>
        public DataSet PeriodViewCntAdver_EachProgram( int ContractSeq, int CampaignCd, int ItemNo, string BgnDay, string EndDay)
        {
            DataSet ds = new DataSet();

            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "PeriodViewCntAdver_EachProgram() Start");
                _log.Debug("-----------------------------------------");
			
                #region [SQL]
                StringBuilder sb = new StringBuilder();
                sb.Append( " SELECT  ad.CategoryCode \n");
                sb.Append( "        ,ad.CategoryName \n");
                sb.Append( "        ,ad.GenreCode \n");
                sb.Append( "        ,ad.GenreName \n");
                sb.Append( "        ,ad.ChannelNo \n");
                sb.Append( "        ,ad.ProgramNm \n");
                sb.Append( "        ,ad.AdHitCnt \n");
                sb.Append( "        ,ad.PgHitCnt \n");
                sb.Append( " FROM ( SELECT   C.CategoryCode \n");
                sb.Append( "                ,C.CategoryName \n");
                sb.Append( "                ,D.GenreCode \n");
                sb.Append( "                ,D.GenreName \n");
                sb.Append( "                ,B.Channel AS ChannelNo \n");
                sb.Append( "                ,B.ProgramNm \n");
                sb.Append( "                ,A.ProgKey \n");
                sb.Append( "                ,SUM(A.AdCnt)  AS AdHitCnt \n");
                sb.Append( "                ,SUM(A.HitCnt) AS PgHitCnt \n");
                sb.Append( "        FROM SummaryAdDaily3 A with(NoLock) \n");
                sb.Append( "        INNER JOIN AdTargetsHanaTV.dbo.Program   B with(NoLock) ON (A.ProgKey  = B.ProgramKey) \n");
                sb.Append( "        INNER JOIN AdTargetsHanaTV.dbo.Category  C with(NoLock) ON (A.Category = C.CategoryCode) \n");
                sb.Append( "        INNER JOIN AdTargetsHanaTV.dbo.Genre     D with(NoLock) ON (A.Genre    = D.GenreCode) \n");
                sb.Append( "        WHERE   B.MediaCode = 1 \n");
                sb.Append( "        AND     A.LogDay BETWEEN '" + BgnDay + "' AND '" + EndDay + "' \n");
                #region 조회조건(계약,캠페인,내역)
                if( ItemNo > 0 )
                {
                    // 광고번호를 선택했으면, 캠페인선택여부와 무관하게 1개의 광고에 대한 쿼리임
                    // 캠페인이 전체이면서 광고내역이 개별이면 개별광고임
                    sb.Append("        AND		A.ItemNo  = " + ItemNo + "\n");
                
                }
                else if( CampaignCd == 0 && ItemNo == 0 )
                {
                    // 캠페인이 전체이면서 광고내역이 전체이면 전체 계약임
                    sb.Append("        AND		A.ItemNo in(select  ItemNo ");
                    sb.Append("                            from    AdTargetsHanaTV.dbo.ContractItem with(noLock)");
                    sb.Append("        		            where   MediaCode   = 1");
                    sb.Append("        					and		ContractSeq = " + ContractSeq + ")");
                }
                else if( CampaignCd > 0 && ItemNo == 0 )
                {
                    // 캠페인이 선택되고, 전체광고면 해당캠패인 전체임
                    sb.Append("        AND     A.ItemNo in(select ItemNo");
                    sb.Append("        				    from	AdTargetsHanaTV.dbo.CampaignDetail d with(noLock)");
                    sb.Append("        					inner join AdTargetsHanaTV.dbo.CampaignMaster m with(noLock) ");
                    sb.Append("        							on m.CampaignCode = d.CampaignCode");
                    sb.Append("        							and m.MediaCode = 1");
                    sb.Append("        							and	m.ContractSeq	= " + ContractSeq );
                    sb.Append("        							and m.CampaignCode	= " + CampaignCd + ")");
                }
                #endregion
                sb.Append( "        AND     A.ProgKey   > 0 \n");
                sb.Append( "        GROUP BY CategoryCode, CategoryName, GenreCode, GenreName,Channel,ProgramNm,ProgKey \n");
                sb.Append( "      ) as ad \n");
                sb.Append( " ORDER BY ad.CategoryCode, ad.GenreCode,ad.ProgramNm \n"); 
                #endregion

                #region [ 파라메터 DEBUG ]
                _log.Debug("ContractSeq :[" + ContractSeq.ToString() + "]");
                _log.Debug("CampaignCd  :[" + CampaignCd.ToString() + "]");
                _log.Debug("ItemNo      :[" + ItemNo.ToString() + "]");
                _log.Debug("BgnDay      :[" + BgnDay      + "]");
                _log.Debug("EndDay      :[" + EndDay      + "]");
                _log.Debug("-----------------------------------------");
                _log.Debug(sb.ToString());
                _log.Debug("-----------------------------------------");
                #endregion
				
                // 쿼리실행
                _db.Open();
				_db.ExecuteQuery(ds, sb.ToString());

                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "PeriodViewCntAdver_EachProgram() End");
                _log.Debug("-----------------------------------------");

                return ds.Copy();
            }
            catch(Exception ex)
            {
                _log.Exception(ex);
                return null;
            }
            finally
            {
                _db.Close();
                if( ds != null )
                {
                    ds.Dispose();
                    ds = null;
                }
            }
        }

	}
}
