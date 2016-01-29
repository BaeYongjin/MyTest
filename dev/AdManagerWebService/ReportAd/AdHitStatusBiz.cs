// ===============================================================================
//
// AdHitStatusBiz.cs
//
// 광고시청현황 서비스 
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
 * Class Name: AdHitStatusBiz
 * 주요기능  : 광고시청현황 처리 로직
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
    /// AdHitStatusBiz에 대한 요약 설명입니다.
    /// </summary>
    public class AdHitStatusBiz : BaseBiz
    {
		#region  생성자
        public AdHitStatusBiz() : base(FrameSystem.connSummaryDbString)
        {
            _log = FrameSystem.oLog;
        }
		#endregion

        #region 광고별 시청현황 집계
        /// <summary>
        /// 광고별 시청현황 집계
        /// </summary>
		/// <param name="adHitStatusModel"></param>
        public void GetAdHitStatus(HeaderModel header, AdHitStatusModel adHitStatusModel)
        {
			try
			{
                StringBuilder sbQuery = null;

				// 데이터베이스를 OPEN한다
				_db.Open();
                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "GetAdHitStatus() Start");
                _log.Debug("-----------------------------------------");

				// 일자가 6자리 이상이면 6자리로 만든다.
				if(adHitStatusModel.SearchDay.Length > 6) adHitStatusModel.SearchDay = adHitStatusModel.SearchDay.Substring(2,6);
				
				// __DEBUG__
                _log.Debug("<입력정보>");
				_log.Debug("SearchMediaCode	 :[" + adHitStatusModel.SearchMediaCode + "]");		// 검색 매체
				_log.Debug("SearchRapCode	 :[" + adHitStatusModel.SearchRapCode + "]");		// 검색 미디어렙
				_log.Debug("SearchAgencyCode :[" + adHitStatusModel.SearchAgencyCode + "]");		// 검색 대행사
				_log.Debug("SearchDay        :[" + adHitStatusModel.SearchDay       + "]");		// 검색 집계일자           
				_log.Debug("SearchKey        :[" + adHitStatusModel.SearchKey       + "]");		// 검색 키워드           
                // __DEBUG__

                // 쿼리생성
				sbQuery = new StringBuilder();
				sbQuery.Append("\n-- 광고별 시청현황" + "\n");
				sbQuery.Append(" SELECT  ItemNo         -- 광고번호" + "\n");
				sbQuery.Append("        ,ItemName       -- 광고명(소재명)" + "\n");
				sbQuery.Append(" 		,AdCnt          -- 당일 노출수량" + "\n");
				sbQuery.Append(" 		,TotAdCnt       -- 당일까지의 누적 노출수량" + "\n");
				sbQuery.Append(" 		,CASE WHEN ExDays <> 0 THEN FLOOR((TotAdCnt/CONVERT(float,ExDays)) * TotDaysExcu) ELSE 0 END AS ExpCntExcu -- 계약종료일기준 예상노출치" + "\n");
				sbQuery.Append(" 		,CASE WHEN ExDays <> 0 THEN FLOOR((TotAdCnt/CONVERT(float,ExDays)) * TotDaysReal) ELSE 0 END AS ExpCntReal -- 실제종료일기준 예상노출치" + "\n");
				sbQuery.Append(" 		,ContractAmt     -- 계약노출수량" + "\n");
				sbQuery.Append(" 		,ExcuteStartDay  -- 집행시작일" + "\n");
				sbQuery.Append(" 		,ExcuteEndDay    -- 집행종료일(계약상)" + "\n");
				sbQuery.Append(" 		,RealEndDay      -- 집행종료일(실제)" + "\n");
				sbQuery.Append(" 		,ExDays          -- 당일까지의 집행일수" + "\n");
				sbQuery.Append(" 		,TotDaysExcu     -- 계약기준 총집행일수" + "\n");
				sbQuery.Append(" 		,TotDaysReal     -- 실제기준 총집행일수" + "\n");
				sbQuery.Append(" 		,ContractSeq     -- 광고계약번호" + "\n");
				sbQuery.Append(" 		,ContractName    -- 광고계약명" + "\n");
				sbQuery.Append(" 		,AgencyName      -- 대행사명" + "\n");
				sbQuery.Append(" 		,AdvertiserName  -- 광고주명" + "\n");
				// 당일집행수 기준 예상치 계산 남은일수*당일노출수 + 누적노출수
				sbQuery.Append(" 		,CASE WHEN ExDays <> 0 THEN (AdCnt * ( TotDaysExcu - ExDays) + TotAdCnt ) ELSE 0 END AS ExpCntDay   -- 당일노출기준 예상노출치" + "\n");
				sbQuery.Append(" FROM (	SELECT   itm.ItemNo, itm.ItemName" + "\n");
				sbQuery.Append(" 		        ,(SELECT ISNULL(SUM(AdCnt),0)" + "\n");
				sbQuery.Append(" 				  FROM SummaryAdDaily0 with(NoLock)" + "\n");
				sbQuery.Append(" 				  WHERE LogDay = '" + adHitStatusModel.SearchDay +"'" + "\n");
				sbQuery.Append(" 				  AND ItemNo = itm.ItemNo" + "\n");
				sbQuery.Append(" 				  AND SummaryType = 1" + "\n");
				sbQuery.Append(" 					 ) AS AdCnt" + "\n");
				sbQuery.Append(" 				,ISNULL(tgr.ContractAmt,0) AS ContractAmt" + "\n");
				sbQuery.Append(" 				,IsNull( (SELECT top 1 isnull(AdCnt,0) + Isnull(AdCntAccu,0)" + "\n");
				sbQuery.Append(" 					          FROM SummaryAdDaily0 with(NoLock)" + "\n");
				sbQuery.Append(" 					          WHERE LogDay BETWEEN SUBSTRING(itm.ExcuteStartDay,3,6)" + "\n");
				sbQuery.Append(" 					                           AND CASE WHEN SUBSTRING(itm.RealEndDay,3,6) < '" + adHitStatusModel.SearchDay +"'" + "\n");
				sbQuery.Append(" 																	THEN SUBSTRING(itm.RealEndDay,3,6)" + "\n");
				sbQuery.Append(" 					                                    ELSE '" + adHitStatusModel.SearchDay +"' END" + "\n");
				sbQuery.Append(" 					          AND ItemNo = itm.ItemNo" + "\n");
				sbQuery.Append(" 					          AND SummaryType = 1" + "\n");
				sbQuery.Append(" 					          ORDER BY LogDay desc" + "\n");
				sbQuery.Append(" 					 ),0) AS TotAdCnt" + "\n");
				sbQuery.Append(" 					,itm.ExcuteStartDay, itm.ExcuteEndDay, itm.RealEndDay" + "\n");
				sbQuery.Append(" 					,DATEDIFF(day,CONVERT(datetime, itm.ExcuteStartDay, 112),CONVERT(datetime, '20" + adHitStatusModel.SearchDay +"', 112)) + 1 ExDays" + "\n");
				sbQuery.Append(" 					,DATEDIFF(day,CONVERT(datetime, itm.ExcuteStartDay, 112),CONVERT(datetime, itm.ExcuteEndDay, 112)) + 1 TotDaysExcu" + "\n");
				sbQuery.Append(" 					,DATEDIFF(day,CONVERT(datetime, itm.ExcuteStartDay, 112),CONVERT(datetime, itm.RealEndDay  , 112)) + 1 TotDaysReal" + "\n");
				sbQuery.Append(" 					,ctr.ContractSeq, ctr.ContractName" + "\n");
				sbQuery.Append(" 					,agn.AgencyName ,adv.AdvertiserName" + "\n");
				sbQuery.Append(" 					,itm.RapCode, itm.AgencyCode, itm.AdType " + "\n");
                sbQuery.Append("			FROM AdTargetsHanaTV.dbo.ContractItem itm with(nolock)" + "\n");
                sbQuery.Append(" 			INNER JOIN AdTargetsHanaTV.dbo.Contract     ctr with(nolock) ON (itm.ContractSeq    = ctr.ContractSeq)" + "\n");
                sbQuery.Append(" 			INNER JOIN AdTargetsHanaTV.dbo.Agency       agn with(nolock) ON (itm.AgencyCode     = agn.AgencyCode)" + "\n");
                sbQuery.Append(" 			INNER JOIN AdTargetsHanaTV.dbo.Advertiser   adv with(nolock) ON (itm.AdvertiserCode = adv.AdvertiserCode)" + "\n");
                sbQuery.Append(" 			LEFT  JOIN AdTargetsHanaTV.dbo.Targeting    tgr with(nolock) ON (itm.ItemNo         = tgr.ItemNo)" + "\n");
				sbQuery.Append("		WHERE itm.ExcuteStartDay <= '20" + adHitStatusModel.SearchDay +"'" + "\n");
				sbQuery.Append("		AND   itm.RealEndDay     >= '20" + adHitStatusModel.SearchDay +"'" + "\n");
				sbQuery.Append("		AND   itm.AdType < '90'" + "\n");
				sbQuery.Append("  ) T" + "\n");
				sbQuery.Append(" WHERE 1 = 1" + "\n");
				// 2015-04-01 조건 추가함
				sbQuery.Append(" AND	AdCnt >5\n ");

				// 검색어가 있으면
				if (adHitStatusModel.SearchKey.Trim().Length > 0)
				{
					// 여러컬럼에 대하여 LIKE 검색을 한다.
					sbQuery.Append(" AND (    T.ItemName       LIKE '%"+ adHitStatusModel.SearchKey+"%'" + "\n");
					sbQuery.Append(" 		  OR T.ContractName   LIKE '%"+ adHitStatusModel.SearchKey+"%'" + "\n");
					sbQuery.Append(" 		  OR T.AgencyName     LIKE '%"+ adHitStatusModel.SearchKey+"%'" + "\n");
					sbQuery.Append(" 		  OR T.AdvertiserName LIKE '%"+ adHitStatusModel.SearchKey+"%'" + "\n");
					sbQuery.Append("      )" + "\n");
				}

				if(!adHitStatusModel.SearchRapCode.Equals("00"))
				{
					sbQuery.Append("  AND T.RapCode = '"+adHitStatusModel.SearchRapCode+"'  \n");
				}        
				if(!adHitStatusModel.SearchAgencyCode.Equals("00"))
				{
					sbQuery.Append("  AND T.AgencyCode = '"+adHitStatusModel.SearchAgencyCode+"'  \n");
				}

                if (!adHitStatusModel.SearchAdType.Equals("00"))
                {
                    sbQuery.Append("  AND T.AdType = '"+adHitStatusModel.SearchAdType+"'  \n");
                }

                if (!adHitStatusModel.SearchAgencyCode.Equals("00"))
                {
                    sbQuery.Append("  AND T.AgencyCode = '"+adHitStatusModel.SearchAgencyCode+"'  \n");
                }     

				sbQuery.Append(" ORDER BY AdCnt DESC" + "\n");

				// __DEBUG__
				_log.Debug(sbQuery.ToString());
				// __DEBUG__
				
				// 쿼리실행
				DataSet ds = new DataSet();
                _db.Timeout = 60 * 5;
				_db.ExecuteQuery(ds,sbQuery.ToString());

				// 결과 DataSet의 데이터모델에 복사
				adHitStatusModel.ReportDataSet = ds.Copy();

				// 결과
				adHitStatusModel.ResultCnt = Utility.GetDatasetCount(adHitStatusModel.ReportDataSet);

				ds.Dispose();

				// 결과코드 셋트
				adHitStatusModel.ResultCD = "0000";

                // __DEBUG__
                _log.Debug("<출력정보>");
                _log.Debug("ResultCnt:[" + adHitStatusModel.ResultCnt + "]");
                // __DEBUG__

                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "GetAdHitStatus() End");
                _log.Debug("-----------------------------------------");
            }
            catch(Exception ex)
            {
                adHitStatusModel.ResultCD = "3000";
                adHitStatusModel.ResultDesc = "광고별 시청현황 집계 조회중 오류가 발생하였습니다";
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