// ===============================================================================
//
// StatisticsChannelBiz.cs
//
// 광고리포트 채널통계 서비스 
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
 * Class Name: StatisticsChannelBiz
 * 주요기능  : 광고리포트 채널통계 처리 로직
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
    /// StatisticsChannelBiz에 대한 요약 설명입니다.
    /// </summary>
    public class StatisticsChannelBiz : BaseBiz
    {

		#region  생성자
        public StatisticsChannelBiz() : base(FrameSystem.connSummaryDbString)
        {
            _log = FrameSystem.oLog;
        }
		#endregion

		#region 기간내 채널통계
        /// <summary>
        ///  기간내 채널통계
        /// </summary>
        /// <param name="statisticsChannelModel"></param>
        public void GetStatisticsChannel(HeaderModel header, StatisticsChannelModel statisticsChannelModel)
        {
			bool isNotTarget = false; // 타겟팅정보가 입력되지않아 존재하지 않을때.
			bool isNotReady  = false; // 데이터가 집계가 되지않아 존재하지 않을때.
            try
			{
				StringBuilder sbQuery = null;

				// 데이터베이스를 OPEN한다
				_db.Open();

                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "GetStatisticsChannel() Start");
                _log.Debug("-----------------------------------------");

				// 일자가 6자리 이상(yyyymmdd)이면 6자리로 만든다.
				if(statisticsChannelModel.SearchStartDay.Length > 6) statisticsChannelModel.SearchStartDay = statisticsChannelModel.SearchStartDay.Substring(2,6);
				if(statisticsChannelModel.SearchEndDay.Length   > 6) statisticsChannelModel.SearchEndDay   = statisticsChannelModel.SearchEndDay.Substring(2,6);
				
				// __DEBUG__
                _log.Debug("<입력정보>");
				_log.Debug("SearchMediaCode	  :[" + statisticsChannelModel.SearchMediaCode   + "]");		// 검색 매체
				_log.Debug("SearchContractSeq :[" + statisticsChannelModel.SearchContractSeq + "]");		// 검색 광고번호           
				_log.Debug("SearchItemNo      :[" + statisticsChannelModel.SearchItemNo      + "]");		// 검색 광고번호           
				_log.Debug("SearchStartDay    :[" + statisticsChannelModel.SearchStartDay    + "]");		// 검색 집계시작 일자          
				_log.Debug("SearchEndDay      :[" + statisticsChannelModel.SearchEndDay      + "]");		// 검색 집계종료 일자          
				// __DEBUG__

				string  MediaCode   = statisticsChannelModel.SearchMediaCode;
                int	    ContractSeq = Convert.ToInt32( statisticsChannelModel.SearchContractSeq );
                int	    CampaignCd	= Convert.ToInt32( statisticsChannelModel.CampaignCode );
                int	    ItemNo      = Convert.ToInt32( statisticsChannelModel.SearchItemNo );
                string  StartDay    = statisticsChannelModel.SearchStartDay;
				string  EndDay      = statisticsChannelModel.SearchEndDay;

				// 쿼리생성
                #region [ 보장노출건수 구하기 ]
                sbQuery = new StringBuilder();
                sbQuery.Append("\n");
                sbQuery.Append(" -- 해당광고의 노출기간조회" + "\n");
                sbQuery.Append(" -- 해당광고의 계약노출(보장노출)조회" + "\n");
                sbQuery.Append(" SELECT sum(ISNULL(b.ContractAmt,0)) AS ContractAmt" + "\n");
                sbQuery.Append(" FROM AdTargetsHanaTV.dbo.ContractItem a with(nolock)" + "\n");
                sbQuery.Append(" LEFT JOIN AdTargetsHanaTV.dbo.Targeting b with(nolock)" + "\n");
                sbQuery.Append("        ON (a.ItemNo = b.ItemNo)" + "\n");
                sbQuery.Append(" WHERE 1=1" + "\n");
                #region 조회조건(계약,캠페인,내역)
                if( ItemNo > 0 )                            // 캠페인이 전체이면서 광고내역이 개별이면 개별광고임
                    sbQuery.Append("    and a.ItemNo  = " + ItemNo + "\n");
                else if( CampaignCd == 0 && ItemNo == 0 )   // 캠페인이 전체이면서 광고내역이 전체이면 전체 계약임
                    sbQuery.Append("    and a.ItemNo in(select  ItemNo from AdTargetsHanaTV.dbo.ContractItem with(noLock) where MediaCode=1 and ContractSeq = " + ContractSeq + " and AdType < '90' )");
                else if( CampaignCd > 0 && ItemNo == 0 )    // 캠페인이 선택되고, 전체광고면 해당캠패인 전체임
                    sbQuery.Append("    and a.ItemNo in( select ItemNo from AdTargetsHanaTV.dbo.CampaignItem where CampaignCode = " + CampaignCd + " )" + "\n");
                #endregion

                // __DEBUG__
                _log.Debug(sbQuery.ToString());
                // __DEBUG__
				
                // 쿼리실행
                DataSet ds = new DataSet();
                _db.ExecuteQuery(ds,sbQuery.ToString());

                int    ContractAmt    = 0;

                if(ds.Tables[0].Rows.Count == 0)
                {
                    isNotTarget = true;
                    throw new Exception();
                }
                ContractAmt    = Convert.ToInt32(ds.Tables[0].Rows[0]["ContractAmt"].ToString());

                ds.Dispose();

                // 해당광고의 계약노출(보장노출)수
                statisticsChannelModel.ContractAmt = ContractAmt;
                #endregion

                #region [ 누적노출건수 구하기 ]
                sbQuery = new StringBuilder();
                sbQuery.Append("\n");
                sbQuery.Append(" -- 해당광고의 계약기간내 총 누적노출수" + "\n" );
                sbQuery.Append(" SELECT ISNULL(SUM(A.AdCnt),0) AS TotalAdCnt" + "\n" );
                sbQuery.Append(" FROM   SummaryAdDaily0 A with(NoLock)" + "\n" );
                sbQuery.Append(" WHERE  A.LogDay BETWEEN '"+ StartDay + "' AND '"+ EndDay + "'" + "\n" );
                sbQuery.Append(" AND		A.SummaryType = 1" + "\n" );
                if( ItemNo > 0 )                            // 캠페인이 전체이면서 광고내역이 개별이면 개별광고임
                    sbQuery.Append("    and a.ItemNo  = " + ItemNo + "\n");
                else if( CampaignCd == 0 && ItemNo == 0 )   // 캠페인이 전체이면서 광고내역이 전체이면 전체 계약임
                    sbQuery.Append("    and a.ItemNo in(select  ItemNo from AdTargetsHanaTV.dbo.ContractItem with(noLock) where MediaCode=1 and ContractSeq = " + ContractSeq + " and AdType < '90' )");
                else if( CampaignCd > 0 && ItemNo == 0 )    // 캠페인이 선택되고, 전체광고면 해당캠패인 전체임
                    sbQuery.Append("    and a.ItemNo in( select ItemNo from AdTargetsHanaTV.dbo.CampaignItem where CampaignCode = " + CampaignCd + " )" + "\n");

                // __DEBUG__
                _log.Debug(sbQuery.ToString());
                // __DEBUG__
				
                // 쿼리실행
                ds = new DataSet();
                _db.ExecuteQuery(ds,sbQuery.ToString());

                if(ds.Tables[0].Rows.Count == 0)
                {
                    isNotReady = true;
                    throw new Exception();
                }

                int TotalAdCnt = Convert.ToInt32(ds.Tables[0].Rows[0]["TotalAdCnt"].ToString());
                ds.Dispose();

                // 해당광고의 계약노출(보장노출)수
                statisticsChannelModel.TotalAdCnt = TotalAdCnt;
                #endregion

                #region [ 실제 쿼리 ]
                sbQuery = new StringBuilder();
                sbQuery.Append("\n");
                sbQuery.Append(" /* 기간내 광고 채널통계*/  \n");
                sbQuery.Append(" declare @TotAdHit int;     \n");
                sbQuery.Append(" set     @TotAdHit    = " + TotalAdCnt + "; \n");
                sbQuery.Append(" with AdData( Category, Genre, Program, AdCnt )" + "\n");
                sbQuery.Append(" as ( SELECT a.Category" + "\n");
                sbQuery.Append(" 			,a.Genre" + "\n");
                sbQuery.Append(" 			,a.ProgKey" + "\n");
                sbQuery.Append(" 			,sum(A.AdCnt) as AdCnt" + "\n");
                sbQuery.Append(" 	    FROM SummaryAdDaily3 A with(NoLock)       " + "\n");
                sbQuery.Append(" 		LEFT JOIN AdTargetsHanaTV.dbo.Program	 B with(NoLock) ON A.ProgKey    = B.ProgramKey" + "\n");
                sbQuery.Append(" 		LEFT JOIN AdTargetsHanaTV.dbo.Category	 C with(NoLock) ON A.Category   = C.CategoryCode" + "\n");
                sbQuery.Append(" 		LEFT JOIN AdTargetsHanaTV.dbo.Genre		 D with(NoLock) ON A.Genre		= D.GenreCode" + "\n");
                sbQuery.Append(" 		WHERE	a.LogDay	between  '"+ StartDay + "' AND '"+ EndDay + "'" + "\n" );
                #region 조회조건(계약,캠페인,내역)
                if( ItemNo > 0 )                            // 캠페인이 전체이면서 광고내역이 개별이면 개별광고임
                    sbQuery.Append("    and a.ItemNo  = " + ItemNo + "\n");
                else if( CampaignCd == 0 && ItemNo == 0 )   // 캠페인이 전체이면서 광고내역이 전체이면 전체 계약임
                    sbQuery.Append("    and a.ItemNo in(select  ItemNo from AdTargetsHanaTV.dbo.ContractItem with(noLock) where MediaCode=1 and ContractSeq = " + ContractSeq + " and AdType < '90' )");
                else if( CampaignCd > 0 && ItemNo == 0 )    // 캠페인이 선택되고, 전체광고면 해당캠패인 전체임
                    sbQuery.Append("    and a.ItemNo in( select ItemNo from AdTargetsHanaTV.dbo.CampaignItem where CampaignCode = " + CampaignCd + " )" + "\n");
                #endregion
                sbQuery.Append(" 		AND		a.ProgKey   > 0                     " + "\n");
                sbQuery.Append(" 		GROUP BY a.Category,a.Genre,a.ProgKey" + "\n");
                sbQuery.Append(" )" + "\n");
                sbQuery.Append(" select	AdTargetsHanaTV.dbo.ufnPadding('L',v.Category,5,' ') + ' ' + v.CategoryName as Category" + "\n");
                sbQuery.Append(" 			, v.CategoryName" + "\n");
                sbQuery.Append(" 			, v.OrdCode" + "\n");
                sbQuery.Append(" 			, v.Ord" + "\n");
                sbQuery.Append(" 			, v.OrdName" + "\n");
                sbQuery.Append(" 			, v.Title" + "\n");
                sbQuery.Append(" 			, v.AdCnt" + "\n");
                sbQuery.Append(" 			, convert( decimal(5,2),( v.AdCnt / cast(@TotAdHit as float) * 100) )	as AdRate" + "\n");
                sbQuery.Append(" 			, Replicate('*', round( v.AdCnt / cast(@TotAdHit as float) * 100,0) ) as RateBar     " + "\n");
                sbQuery.Append(" from	(" + "\n");
                sbQuery.Append(" 				select	a.Category			as	Category" + "\n");
                sbQuery.Append(" 							, c.CategoryName	as CategoryName" + "\n");
                sbQuery.Append(" 							, '1'							as	OrdCode" + "\n");
                sbQuery.Append(" 							, '1 카테고리'			as	Ord" + "\n");
                sbQuery.Append(" 							, '카테고리'				as	OrdName" + "\n");
                sbQuery.Append(" 							, ''							as Title" + "\n");
                sbQuery.Append(" 							, sum(AdCnt)			as	AdCnt" + "\n");
                sbQuery.Append(" 				from	AdData a" + "\n");
                sbQuery.Append(" 				left join AdTargetsHanaTV.dbo.Category c with(NoLock) on A.Category = C.CategoryCode" + "\n");
                sbQuery.Append(" 				group by a.Category,c.CategoryName" + "\n");
                sbQuery.Append(" 				union all" + "\n");
                sbQuery.Append(" 				select	a.Category" + "\n");
                sbQuery.Append(" 							, c.CategoryName	as CategoryName" + "\n");
                sbQuery.Append(" 							, '2'						as	OrdCode" + "\n");
                sbQuery.Append(" 							, '2 장르	'			as	Ord" + "\n");
                sbQuery.Append(" 							, '장르'					as	OrdName" + "\n");
                sbQuery.Append(" 							, min(d.GenreName)   as Title" + "\n");
                sbQuery.Append(" 							, sum(AdCnt)" + "\n");
                sbQuery.Append(" 				from	AdData a" + "\n");
                sbQuery.Append(" 				left join AdTargetsHanaTV.dbo.Category c with(NoLock) on A.Category = C.CategoryCode" + "\n");
                sbQuery.Append(" 				left join	AdTargetsHanaTV.dbo.Genre d with(NoLock) on a.Genre = d.GenreCode" + "\n");
                sbQuery.Append(" 				group by a.Category,c.CategoryName, a.Genre" + "\n");
                sbQuery.Append(" 				union all" + "\n");
                sbQuery.Append(" 				select	a.Category" + "\n");
                sbQuery.Append(" 							, c.CategoryName	as CategoryName" + "\n");
                sbQuery.Append(" 							, '3'				as	OrdCode" + "\n");
                sbQuery.Append(" 							, '3 채널'		as	Ord" + "\n");
                sbQuery.Append(" 							, '채널'			as	OrdName" + "\n");
                sbQuery.Append(" 							, b.ProgramNm as Title" + "\n");
                sbQuery.Append(" 							, sum(AdCnt)" + "\n");
                sbQuery.Append(" 				from	AdData a" + "\n");
                sbQuery.Append(" 				left join AdTargetsHanaTV.dbo.Category c with(NoLock) on A.Category = C.CategoryCode" + "\n");
                sbQuery.Append(" 				left	join AdTargetsHanaTV.dbo.Program	b with(NoLock) on a.Program = b.ProgramKey" + "\n");
                sbQuery.Append(" 				group by a.Category, c.CategoryName, b.ProgramNm ) v" + "\n");
                sbQuery.Append(" order by Category,OrdCode, AdCnt desc" + "\n");
                #endregion

				// __DEBUG__
				_log.Debug(sbQuery.ToString());
				// __DEBUG__
				
				// 쿼리실행
				ds = new DataSet();
				_db.ExecuteQuery(ds,sbQuery.ToString());

				// 결과 DataSet의 데이터모델에 복사
				statisticsChannelModel.ReportDataSet = ds.Copy();

				// 결과
				statisticsChannelModel.ResultCnt = Utility.GetDatasetCount(statisticsChannelModel.ReportDataSet);

				ds.Dispose();

				// 결과코드 셋트
				statisticsChannelModel.ResultCD = "0000";

                // __DEBUG__
                _log.Debug("<출력정보>");
                _log.Debug("ResultCnt:[" + statisticsChannelModel.ResultCnt + "]");
                // __DEBUG__

                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "GetStatisticsChannel() End");
                _log.Debug("-----------------------------------------");
            }
            catch(Exception ex)
            {
                statisticsChannelModel.ResultCD = "3000";
				if(isNotTarget)
				{
					statisticsChannelModel.ResultDesc = "해당광고의 정보가 존재하지 않습니다.";
				}
				else if(isNotReady)
				{
					statisticsChannelModel.ResultDesc = "해당 기간은 데이터가 집계되지 않았습니다.";
				}
				else
				{
					statisticsChannelModel.ResultDesc = "채널통계 조회중 오류가 발생하였습니다";
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