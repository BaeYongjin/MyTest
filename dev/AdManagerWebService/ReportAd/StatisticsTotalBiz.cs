// ===============================================================================
//
// StatisticsTotalBiz.cs
//
// 전체통계 서비스 
//
// ===============================================================================
// Release history
// 2007.10.26 RH.Jung OAP도 조회 가능토톡 수정
// ===============================================================================
// Copyright (C) 2007 G-Inno Systems Inc.
// All rights reserved.
// ==============================================================================

/*
 * -------------------------------------------------------
 * Class Name: StatisticsTotalBiz
 * 주요기능  : 전체통계 처리 로직
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
    /// StatisticsTotalBiz에 대한 요약 설명입니다.
    /// </summary>
    public class StatisticsTotalBiz : BaseBiz
    {

		#region  생성자
        public StatisticsTotalBiz() : base(FrameSystem.connSummaryDbString)
        {
            _log = FrameSystem.oLog;
        }
		#endregion

		#region 전체통계
		/// <summary>
      /// 전체통계
      /// </summary>
      /// <param name="statisticsTotalModel"></param>
      public void GetStatisticsTotal(HeaderModel header, StatisticsTotalModel statisticsTotalModel)
      {
			try
			{
				StringBuilder sbQuery = null;

				// 데이터베이스를 OPEN한다
				_db.Open();
            _log.Debug("-----------------------------------------");
            _log.Debug(this.ToString() + "GetStatisticsTotal() Start");
            _log.Debug("-----------------------------------------");
				
				// __DEBUG__
            _log.Debug("<입력정보>");
				_log.Debug("SearchMediaCode	 :[" + statisticsTotalModel.SearchMediaCode  + "]");	// 검색 매체
				_log.Debug("SearchRapCode	 :[" + statisticsTotalModel.SearchRapCode    + "]");	// 검색 미디어렙
				_log.Debug("SearchAgencyCode :[" + statisticsTotalModel.SearchAgencyCode + "]");	// 검색 대행사
				_log.Debug("SearchKey        :[" + statisticsTotalModel.SearchKey        + "]");	// 검색 키워드           
            // __DEBUG__

            // 쿼리생성
				sbQuery = new StringBuilder();
				sbQuery.Append("\n"
					+ "-- 전체통계                                                     \n"
					+ "SELECT ItemNo          -- 광고번호                              \n"
					+ "      ,ItemName        -- 광고명(소재명)                        \n"
					+ "      ,ExcuteEndDay    -- 집행종료일(계약상)                    \n"
					+ "      ,ContractAmt     -- 계약(보장)노출수량                    \n"
					+ "      ,CASE WHEN TotDayCnt   > 0 THEN FLOOR((TotAdCnt/TotDayCnt) * (TotRealDays- ExDays + 1) + TotAdCnt )  \n"
                    + "            ELSE 0 END  AS AvgExpAmt -- 누적평균노출량(예상)     \n"
					+ "      ,TotAdCnt        -- 누적 노출수량                         \n"
					+ "      ,REPLICATE('■', CASE WHEN ContractAmt > 0 THEN ROUND((TotAdCnt/CONVERT(float,ContractAmt)*10),0)      \n"
                    + "                                                 ELSE 0 END) AS ExcRateBar                              \n"
					+ "      ,CASE WHEN ContractAmt > 0 THEN CONVERT(DECIMAL(9,2),(TotAdCnt/CONVERT(float,ContractAmt)) * 100) \n"
					+ "                                 ELSE 0 END AS ExcRate       \n"
					+ "      ,ExcuteStartDay  -- 집행시작일                            \n"
					+ "      ,RealEndDay      -- 실제종료일                            \n"
					+ "      ,TotExcDays      -- 계약기준 총집행일수                   \n"
					+ "      ,TotRealDays     -- 실제기준 총집행일수                   \n"
					+ "      ,TotDayCnt       -- 실제 실행일수                         \n"
					+ "      ,ContractName    -- 계약명                                \n"
					+ "      ,AgencyName      -- 대행사명 \n"
					+ "      ,AdvertiserName  -- 광고주명 \n"
					+ " FROM (                                                         \n"
					+ "      SELECT itm.ItemNo, itm.ItemName                           \n"
					+ "            ,ISNULL(tgr.ContractAmt,0) AS ContractAmt           \n"
					+ "            ,(SELECT ISNULL(SUM(AdCnt),0)                       \n"
					+ "                FROM SummaryAdDaily0 with(nolock)               \n"
					+ "               WHERE LogDay BETWEEN SUBSTRING(itm.ExcuteStartDay,3,6) \n"
					+ "                                AND SUBSTRING(itm.RealEndDay,3,6)     \n"
					+ "                 AND ItemNo = itm.ItemNo                        \n"
					+ "                 AND SummaryType = 1   -- 상품별                \n"
					+ "             ) AS TotAdCnt                                      \n"
					+ "            ,(SELECT COUNT(*)                                   \n"
					+ "               FROM (SELECT Distinct LogDay                     \n"
					+ "                       FROM SummaryAdDaily0 with(nolock)        \n"
					+ "                      WHERE LogDay BETWEEN SUBSTRING(itm.ExcuteStartDay,3,6) \n"
					+ "                                       AND SUBSTRING(itm.RealEndDay,3,6)     \n"
					+ "                        AND ItemNo = itm.ItemNo                 \n"
					+ "                        AND SummaryType = 1   -- 상품별         \n"
					+ "                    ) TL                                        \n"
					+ "             ) AS TotDayCnt                                     \n"
					+ "            ,itm.ExcuteStartDay, itm.ExcuteEndDay, itm.RealEndDay  \n"
					+ "            ,DATEDIFF(day,CONVERT(datetime, itm.ExcuteStartDay, 112),GetDate() - 1 ) + 1 as ExDays  \n"
					+ "            ,DATEDIFF(day,CONVERT(datetime, itm.ExcuteStartDay, 112),CONVERT(datetime, itm.ExcuteEndDay, 112)) TotExcDays  \n"
					+ "            ,DATEDIFF(day,CONVERT(datetime, itm.ExcuteStartDay, 112),CONVERT(datetime, itm.RealEndDay, 112))   TotRealDays \n"
					+ "            ,itm.RapCode                                        \n"
					+ "            ,ctr.ContractName                                   \n"
					+ "            ,agn.AgencyName                                     \n"
					+ "            ,adv.AdvertiserName                                 \n"
                    + "            ,agn.AgencyCode                                     \n"
                    + "        FROM AdTargetsHanaTV.dbo.ContractItem itm with(nolock)                      \n"
                    + "             INNER JOIN AdTargetsHanaTV.dbo.Contract     ctr with(nolock) ON (itm.ContractSeq    = ctr.ContractSeq) \n"
                    + "             LEFT  JOIN AdTargetsHanaTV.dbo.Targeting    tgr with(nolock) ON (itm.ItemNo         = tgr.ItemNo)      \n"
                    + "             INNER JOIN AdTargetsHanaTV.dbo.Agency       agn with(nolock) ON (itm.AgencyCode     = agn.AgencyCode)     \n"
                    + "             INNER JOIN AdTargetsHanaTV.dbo.Advertiser   adv with(nolock) ON (itm.AdvertiserCode = adv.AdvertiserCode) \n"
					+ "       WHERE itm.AdState IN ('20','30')       -- 편성 및 중지 광고만                            \n"
					+ "   --    AND	itm.AdType BETWEEN '10' AND '19' -- 필수광고만                                     \n"
 					+ "     ) T                                                        \n"
					+ " WHERE 1 = 1                                                    \n"
					);

				// 검색어가 있으면
				if (statisticsTotalModel.SearchKey.Trim().Length > 0)
				{
					// 여러컬럼에 대하여 LIKE 검색을 한다.
					sbQuery.Append(" AND ("
						+ "    T.ItemName       LIKE '%"+ statisticsTotalModel.SearchKey+"%' \n"													
						+ " OR T.ContractName   LIKE '%"+ statisticsTotalModel.SearchKey+"%' \n"													
						+ " OR T.AgencyName     LIKE '%"+ statisticsTotalModel.SearchKey+"%' \n"													
						+ " OR T.AdvertiserName LIKE '%"+ statisticsTotalModel.SearchKey+"%' \n"													
						+ " ) \n"
						);
				}

				if(!statisticsTotalModel.SearchRapCode.Equals("00"))
				{
					sbQuery.Append("  AND T.RapCode = '"+statisticsTotalModel.SearchRapCode+"'  \n");
				}        
				if(!statisticsTotalModel.SearchAgencyCode.Equals("00"))
				{
					sbQuery.Append("  AND T.AgencyCode = '"+statisticsTotalModel.SearchAgencyCode+"'  \n");
				}     


				sbQuery.Append(""
					+ " ORDER BY ExcuteEndDay, ItemNo      \n"
					);

				// __DEBUG__
				_log.Debug(sbQuery.ToString());
				// __DEBUG__
				
				// 쿼리실행
				DataSet ds = new DataSet();
				_db.ExecuteQuery(ds,sbQuery.ToString());

				// 결과 DataSet의 데이터모델에 복사
				statisticsTotalModel.ReportDataSet = ds.Copy();

				// 결과
				statisticsTotalModel.ResultCnt = Utility.GetDatasetCount(statisticsTotalModel.ReportDataSet);

				ds.Dispose();


				// 결과코드 셋트
				statisticsTotalModel.ResultCD = "0000";

                // __DEBUG__
                _log.Debug("<출력정보>");
                _log.Debug("ResultCnt:[" + statisticsTotalModel.ResultCnt + "]");
                // __DEBUG__

                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "GetStatisticsTotal() End");
                _log.Debug("-----------------------------------------");
            }
            catch(Exception ex)
            {
                statisticsTotalModel.ResultCD = "3000";
                statisticsTotalModel.ResultDesc = "전체통계 조회중 오류가 발생하였습니다";
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