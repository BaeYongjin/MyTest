// ===============================================================================
//
// SummaryAdBiz.cs
//
// 광고 총괄집계 서비스 
//
// ===============================================================================
// Release history
// 2007.10.26 RH.Jung OAP도 집계가능토록 함 집계공통이용 메소드 => GetContractItemList()
// ===============================================================================
// Copyright (C) 2007 G-Inno Systems Inc.
// All rights reserved.
// ==============================================================================

/*
 * -------------------------------------------------------
 * Class Name: SummaryAdBiz
 * 주요기능  : 광고 총괄집계 처리 로직
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
    /// 총괄 광고 집계
    /// </summary>
    public class SummaryAdBiz : BaseBiz
    {

        #region  생성자
        public SummaryAdBiz()
            : base(FrameSystem.connSummaryDbString)
        {
            _log = FrameSystem.oLog;
        }
        #endregion

        #region 광고계약에 해당하는 광고내역조회
        /// <summary>
        /// 광고내역목록조회
        /// </summary>
        /// <param name="summaryAdModel"></param>
        public void GetContractItemList(HeaderModel header, SummaryAdModel summaryAdModel)
        {
            try
            {   // 데이터베이스를 OPEN한다
                _db.Open();

                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "GetContractItemList() Start");
                _log.Debug("-----------------------------------------");

                // __DEBUG__
                _log.Debug("<입력정보>");
                _log.Debug("_SearchContractSeq :[" + summaryAdModel.SearchContractSeq + "]");
                _log.Debug("_CampaignCode :[" + summaryAdModel.CampaignCode + "]");

                // __DEBUG__

                StringBuilder sbQuery = new StringBuilder();

                // 쿼리생성
                //				sbQuery.Append("\n"
                //					+ "SELECT a.ItemNo 	        \n"   
                //					+ "      ,a.ItemName	    \n"     
                //					+ "      ,a.ExcuteStartDay  \n"
                //					+ "      ,a.RealEndDay      \n"
                //					+ "  FROM AdTargetsHanaTV.dbo.ContractItem a with(NoLock)  \n"
                //					+ "  LEFT JOIN AdTargetsHanaTV.dbo.CampaignMaster     B with(NoLock) ON (A.ContractSeq    = B.ContractSeq) \n"
                //					+ "  LEFT JOIN AdTargetsHanaTV.dbo.CampaignDetail     C with(NoLock) ON (B.CampaignCode = C.CampaignCode AND A.Itemno    = C.Itemno) \n"
                //					+ " WHERE a.ContractSeq = " + summaryAdModel.SearchContractSeq + "\n"										
                //					+ "--   AND a.AdType BETWEEN '10'  AND '19'  \n"
                //					);
                //					if(!summaryAdModel.CampaignCode.Equals("00"))
                //					{
                //						sbQuery.Append("  AND C.CampaignCode = '"+summaryAdModel.CampaignCode+"' \n");
                //					}    
                //					sbQuery.Append(""
                //					+ " ORDER BY a.ItemName     \n"
                //					);
                sbQuery.Append("\n");
                sbQuery.Append(" SELECT a.ItemNo         \n");
                sbQuery.Append("       ,a.ItemName       \n");
                sbQuery.Append("       ,a.ExcuteStartDay \n");
                sbQuery.Append("       ,a.RealEndDay     \n");
                sbQuery.Append(" FROM   AdTargetsHanaTV.dbo.ContractItem a with(noLock)  \n");
                sbQuery.Append(" WHERE  a.ContractSeq = " + summaryAdModel.SearchContractSeq + "\n");

                if (!summaryAdModel.CampaignCode.Equals("00"))
                {
                    sbQuery.Append(" AND a.ItemNo in(select distinct ItemNo     \n");
                    sbQuery.Append("                 from   AdTargetsHanaTV.dbo.CampaignMaster m with(noLock)   \n");
                    sbQuery.Append("		         inner join AdTargetsHanaTV.dbo.CampaignDetail d with(noLock) on m.CampaignCode = d.CampaignCode    \n");
                    sbQuery.Append("				 where  m.ContractSeq = a.ContractSeq   \n");
                    sbQuery.Append("				 and    m.CampaignCode = '" + summaryAdModel.CampaignCode + "') \n");
                }
                sbQuery.Append(" ORDER BY a.ItemName");

                // __DEBUG__
                _log.Debug(sbQuery.ToString());
                // __DEBUG__

                // 쿼리실행
                DataSet ds = new DataSet();
                _db.ExecuteQuery(ds, sbQuery.ToString());

                // 결과 DataSet의 광고내역모델에 복사
                summaryAdModel.ItemDataSet = ds.Copy();
                // 결과
                summaryAdModel.ResultCnt = Utility.GetDatasetCount(summaryAdModel.ItemDataSet);
                // 결과코드 셋트
                summaryAdModel.ResultCD = "0000";

                // __DEBUG__
                _log.Debug("<출력정보>");
                _log.Debug("ResultCnt:[" + summaryAdModel.ResultCnt + "]");
                // __DEBUG__

                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "GetContractItemList() End");
                _log.Debug("-----------------------------------------");
            }
            catch (Exception ex)
            {
                summaryAdModel.ResultCD = "3000";
                summaryAdModel.ResultDesc = "광고내역정보 조회중 오류가 발생하였습니다";
                _log.Exception(ex);
            }
            finally
            {
                // 데이터베이스를  Close한다
                _db.Close();
            }
        }

        #endregion

        #region 총기간 광고집행 집계
        /// <summary>
        ///  총기간 광고집행 집계
        /// </summary>
        /// <param name="summaryAdModel"></param>
        public void GetSummaryAdTotality(HeaderModel header, SummaryAdModel summaryAdModel)
        {
            bool isNotReady = false; // 데이터가 집계가 되지않아 존재하지 않을때.
            try
            {
                StringBuilder sbQuery = null;

                // 데이터베이스를 OPEN한다
                _db.Open();


                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "GetSummaryAdDaily() Start");
                _log.Debug("-----------------------------------------");

                // 일자가 6자리 이상(yyyymmdd)이면 6자리로 만든다.
                if (summaryAdModel.SearchStartDay.Length > 6) summaryAdModel.SearchStartDay = summaryAdModel.SearchStartDay.Substring(2, 6);
                if (summaryAdModel.SearchEndDay.Length > 6) summaryAdModel.SearchEndDay = summaryAdModel.SearchEndDay.Substring(2, 6);

                // __DEBUG__
                _log.Debug("<입력정보>");
                _log.Debug("SearchMediaCode	  :[" + summaryAdModel.SearchMediaCode + "]");		// 검색 매체
                _log.Debug("SearchContractSeq :[" + summaryAdModel.SearchContractSeq + "]");		// 검색 광고번호           
                _log.Debug("SearchItemNo      :[" + summaryAdModel.SearchItemNo + "]");		// 검색 광고번호           
                _log.Debug("SearchStartDay    :[" + summaryAdModel.SearchStartDay + "]");		// 검색 집계시작 일자          
                _log.Debug("SearchEndDay      :[" + summaryAdModel.SearchEndDay + "]");		// 검색 집계종료 일자          
                // __DEBUG__

                string MediaCode = summaryAdModel.SearchMediaCode;
                string ContractSeq = summaryAdModel.SearchContractSeq;
                string ItemNo = summaryAdModel.SearchItemNo;
                string StartDay = summaryAdModel.SearchStartDay;
                string EndDay = summaryAdModel.SearchEndDay;

                string Type = "C"; // 계약건벌

                // 광고번호가 있으면 광고내역건별 조회이다.
                if (!ItemNo.Equals("") && !ItemNo.Equals("00"))
                {
                    Type = "I";
                }

                // 쿼리생성
                sbQuery = new StringBuilder();
                sbQuery.Append("\n"
                    + "-- 전체 이용가구수             \n"
                    + "SELECT ISNULL(HouseTotal,0) AS TotUsr \n"
                    + "  FROM SummaryBase  with(NoLock)   \n"
                    + " WHERE LogDay = '" + StartDay + "' \n"
                    );

                // __DEBUG__
                _log.Debug(sbQuery.ToString());
                // __DEBUG__

                // 쿼리실행
                DataSet ds = new DataSet();
                _db.ExecuteQuery(ds, sbQuery.ToString());

                if (ds.Tables[0].Rows.Count == 0)
                {
                    isNotReady = true;
                    throw new Exception();
                }

                int TotUsr = Convert.ToInt32(ds.Tables[0].Rows[0]["TotUsr"].ToString());
                ds.Dispose();

                // 종료일기준 이용자수
                summaryAdModel.TotalUser = TotUsr;

                // 쿼리생성
                sbQuery = new StringBuilder();
                sbQuery.Append("\n"
                    + "/* 총기간 광고집행 집계                                     \n"
                    + "   조회조건 : 매체코드, 계약번호 또는 광고번호 */        \n"
                    + "                                                    \n"
                    + "DECLARE @MediaCode int;   -- 매체코드                  \n"
                    + "DECLARE @LogDay CHAR(6);  -- 집행일자                  \n"
                    + "DECLARE @ItemNo int;      -- 광고번호                  \n"
                    + "DECLARE @ContractSeq int; -- 계약번호                    \n"
                    + "DECLARE @TotUsr int;      -- 전체 이용가구수              \n"
                    + "DECLARE @TotAdHit int;    -- 전체 광고노출수              \n"
                    + "DECLARE @TotPgHit int;    -- 전체 프로그램시청수           \n"
                    + "                                          \n"
                    + "SET @MediaCode   =  " + MediaCode + "  \n"
                    + "SET @LogDay      = '" + StartDay + "' \n"
                    + "SET @ItemNo      =  " + ItemNo + "  \n"
                    + "SET @ContractSeq =  " + ContractSeq + "  \n"
                    + "SET @TotUsr      =  " + TotUsr.ToString() + " \n"
                    + "SET @TotAdHit    = 0;                                                         \n"
                    + "SET @TotPgHit    = 0;                                                         \n"
                    + "                                                                              \n"
                    + "                                                                              \n"
                    + "-- 전체 광고Hit                                                               \n"
                    + "SELECT @TotAdHit = SUM(A.AdCnt)                                               \n"
                    + "      ,@TotPgHit = SUM(A.HitCnt)                                              \n"
                    + "  FROM SummaryAdTotality0 A  with(NoLock)                                      \n"
                    + " WHERE 1 = 1                                                                 \n"
                    );
                // 계약조회인지 개별광고조회인지
                if (Type.Equals("I"))
                {
                    sbQuery.Append("   AND A.ItemNo     = @ItemNo \n");
                }
                else
                {
                    sbQuery.Append("   AND A.ContractSeq  = @ContractSeq \n");
                }
                sbQuery.Append(""
                    + "   AND A.SummaryType  = 1                                                                                               \n"
                    + "                                                                                                                        \n"
                    + "-- 상품별 합계                                                                                                          \n"
                    + "SELECT  1 AS TypeCode                                                                                                   \n"
                    + "       ,'1 상품' AS TypeName                                                                                                   \n"
                    + "       ,'상품' AS SumType                                                                                               \n"
                    + "       ,B.SummaryCode AS SumCode                                                                                        \n"
                    + "       ,B.SummaryName AS SumName                                                                                        \n"
                    + "       ,SUM(A.AdCnt) AS AdCnt                                                                                           \n"
                    + "       ,CONVERT(DECIMAL(9,2),(SUM(A.AdCnt) / CONVERT(float,@TotAdHit)) * 100) AS AdRate                                 \n"
                    + "       ,MAX(A.AdHouseHold) AS HsCnt                                                                                     \n"
                    + "       ,CONVERT(DECIMAL(9,2),(SUM(A.AdHouseHold) / CONVERT(float,@TotUsr)) * 100) AS Reach                              \n"
                    + "       ,CONVERT(DECIMAL(9,2),(SUM(A.AdCnt) / CONVERT(float,SUM(A.AdHouseHold)))) AS Freq                                \n"
                    + "       ,CONVERT(DECIMAL(9,2),(SUM(A.AdCnt) / CONVERT(float,@TotUsr)) * 100) AS GRP                                      \n"
                    + "       ,SUM(A.PgCnt) AS PgCnt                                                                                           \n"
                    + "       ,SUM(A.HitCnt) AS HitCnt                                                                                         \n"
                    + "       ,CONVERT(DECIMAL(9,2),(SUM(A.HitCnt) / CONVERT(float,@TotPgHit)) * 100) AS PgRate                                \n"
                    + "  FROM SummaryAdTotality0     A with(NoLock)                                                                            \n"
                    + "       INNER JOIN SummaryCode B with(NoLock) ON (A.SummaryType = B.SummaryType AND A.SummaryCode = B.SummaryCode)      \n"
                    + " WHERE 1 = 1                                                                                                           \n"
                    );
                // 계약조회인지 개별광고조회인지
                if (Type.Equals("I"))
                {
                    sbQuery.Append("   AND A.ItemNo       = @ItemNo \n");
                }
                else
                {
                    sbQuery.Append("   AND A.ContractSeq  = @ContractSeq \n");
                }
                sbQuery.Append(""
                    + "   AND A.SummaryType  = 1 -- 1:상품별     \n"
                    + " GROUP BY B.SummaryCode, B.SummaryName    \n"
                    + "                                          \n"
                    + "UNION ALL                                 \n"
                    + "                                          \n"
                    + "-- 전체 합계                              \n"
                    + "SELECT  2 AS TypeCode                     \n"
                    + "       ,'2 합계' AS TypeName              \n"
                    + "       ,'합계' AS SumType                 \n"
                    + "       ,0 AS SumCode                      \n"
                    + "       ,'' AS SumName                     \n"
                    + "       ,SUM(A.AdCnt) AS AdCnt             \n"
                    + "       ,null AS AdRate                    \n"
                    + "       ,null AS HsCnt                     \n"
                    + "       ,null AS Reach                     \n"
                    + "       ,null AS Freq                      \n"
                    + "       ,null AS GRP                       \n"
                    + "       ,null AS PgCnt                     \n"
                    + "       ,null AS HitCnt                    \n"
                    + "       ,null AS PgRate                    \n"
                    + "  FROM SummaryAdTotality0     A with(NoLock)  \n"
                    + "       INNER JOIN SummaryCode B with(NoLock) ON (A.SummaryType = B.SummaryType AND A.SummaryCode = B.SummaryCode)  \n"
                    + " WHERE 1 = 1                                                                                          \n"
                    );
                // 계약조회인지 개별광고조회인지
                if (Type.Equals("I"))
                {
                    sbQuery.Append("   AND A.ItemNo     = @ItemNo \n");
                }
                else
                {
                    sbQuery.Append("   AND A.ContractSeq  = @ContractSeq \n");
                }
                sbQuery.Append(""
                    + "   AND A.SummaryType  = 1 -- 1:상품별                                                          \n"
                    + "                                                                                               \n"
                    + "UNION ALL                                                                                      \n"
                    + "                                                                                               \n"
                    + "-- 카테고리별 집계                                                                             \n"
                    + "SELECT  3 AS TypeCode                                                                          \n"
                    + "       ,'3 로딩광고요약' AS TypeName                                                           \n"
                    + "       ,'로딩광고요약' AS SumType                                                              \n"
                    + "       ,B.CategoryCode AS SumCode                                                              \n"
                    + "       ,B.CategoryName AS SumName                                                              \n"
                    + "       ,SUM(A.AdCnt) AS AdCnt                                                                  \n"
                    + "       ,CONVERT(DECIMAL(9,2),(SUM(A.AdCnt) / CONVERT(float,@TotAdHit)) * 100) AS AdRate        \n"
                    + "       ,MAX(A.AdHouseHold) AS HsCnt                                                            \n"
                    + "       ,CONVERT(DECIMAL(9,2),(SUM(A.AdHouseHold) / CONVERT(float,@TotUsr)) * 100) AS Reach     \n"
                    + "       ,CONVERT(DECIMAL(9,2),(SUM(A.AdCnt) / CONVERT(float,SUM(A.AdHouseHold)))) AS Freq       \n"
                    + "       ,CONVERT(DECIMAL(9,2),(SUM(A.AdCnt) / CONVERT(float,@TotUsr)) * 100) AS GRP             \n"
                    + "       ,SUM(A.PgCnt) AS PgCnt                                                                  \n"
                    + "       ,SUM(A.HitCnt) AS HitCnt                                                                \n"
                    + "       ,CONVERT(DECIMAL(9,2),(SUM(A.HitCnt) / CONVERT(float,@TotPgHit)) * 100) AS PgRate       \n"
                    + "  FROM SummaryAdTotality1  A with(NoLock)                                                      \n"
                    + "       INNER JOIN AdTargetsHanaTV.dbo.Category B with(NoLock) ON (A.Category = B.CategoryCode) \n"
                    + " WHERE 1 = 1                                                                                   \n"
                    );
                // 계약조회인지 개별광고조회인지
                if (Type.Equals("I"))
                {
                    sbQuery.Append("   AND A.ItemNo       = @ItemNo \n");
                }
                else
                {
                    sbQuery.Append("   AND A.ContractSeq  = @ContractSeq \n");
                }
                sbQuery.Append(""
                    + " GROUP BY B.CategoryCode, B.CategoryName                                                   \n"
                    + "                                                                                           \n"
                    + "UNION ALL                                                                                  \n"
                    + "                                                                                           \n"
                    + "-- 초수별 집계                                                                             \n"
                    + "SELECT  4 AS TypeCode                                                                      \n"
                    + "       ,'4 초수' AS TypeName                                                               \n"
                    + "       ,'초수' AS SumType                                                                  \n"
                    + "       ,B.AdTime AS SumCode                                                                \n"
                    + "       ,Convert(VARCHAR(5),B.AdTime) + '초' AS SumName                                     \n"
                    + "       ,SUM(A.AdCnt) AS AdCnt                                                              \n"
                    + "       ,CONVERT(DECIMAL(9,2),(SUM(A.AdCnt) / CONVERT(float,@TotAdHit)) * 100) AS AdRate    \n"
                    + "       ,null AS HsCnt                                                                      \n"
                    + "       ,null AS Reach                                                                      \n"
                    + "       ,null AS Freq                                                                       \n"
                    + "       ,null AS GRP                                                                        \n"
                    + "       ,SUM(A.PgCnt) AS PgCnt                                                              \n"
                    + "       ,SUM(A.HitCnt) AS HitCnt                                                            \n"
                    + "       ,CONVERT(DECIMAL(9,2),(SUM(A.HitCnt) / CONVERT(float,@TotPgHit)) * 100) AS PgRate   \n"
                    + "  FROM SummaryAdTotality0      A with(NoLock)                                              \n"
                    + "       INNER JOIN AdTargetsHanaTV.dbo.ContractItem B with(NoLock) ON (A.ItemNo = B.ItemNo) \n"
                    + " WHERE 1 = 1                                                                               \n"
                    );
                // 계약조회인지 개별광고조회인지
                if (Type.Equals("I"))
                {
                    sbQuery.Append("   AND A.ItemNo       = @ItemNo \n");
                }
                else
                {
                    sbQuery.Append("   AND A.ContractSeq  = @ContractSeq \n");
                }
                sbQuery.Append(""
                    + "   AND A.SummaryType  = 1 -- 1:상품별                                                     \n"
                    + " GROUP BY B.AdTime                                                                        \n"
                    + "                                                                                          \n"
                    + "UNION ALL                                                                                 \n"
                    + "                                                                                          \n"
                    + "-- 요일별 집계                                                                            \n"
                    + "SELECT  5 AS TypeCode                                                                     \n"
                    + "       ,'5 요일' AS TypeName                                                              \n"
                    + "       ,'요일' AS SumType                                                                 \n"
                    + "       ,B.SummaryCode AS SumCode                                                          \n"
                    + "       ,B.SummaryName AS SumName                                                          \n"
                    + "       ,SUM(A.AdCnt) AS AdCnt                                                             \n"
                    + "       ,CONVERT(DECIMAL(9,2),(SUM(A.AdCnt) / CONVERT(float,@TotAdHit)) * 100) AS AdRate   \n"
                    + "       ,null AS HsCnt                                                                     \n"
                    + "       ,null AS Reach                                                                     \n"
                    + "       ,null AS Freq                                                                      \n"
                    + "       ,null AS GRP                                                                       \n"
                    + "       ,SUM(A.PgCnt) AS PgCnt                                                             \n"
                    + "       ,SUM(A.HitCnt) AS HitCnt                                                           \n"
                    + "       ,CONVERT(DECIMAL(9,2),(SUM(A.HitCnt) / CONVERT(float,@TotPgHit)) * 100) AS PgRate  \n"
                    + "  FROM SummaryAdTotality0     A with(NoLock)                                              \n"
                    + "       INNER JOIN SummaryCode B with(NoLock) ON (A.SummaryType = B.SummaryType AND A.SummaryCode = B.SummaryCode)   \n"
                    + " WHERE 1 = 1                                                                              \n"
                    );
                // 계약조회인지 개별광고조회인지
                if (Type.Equals("I"))
                {
                    sbQuery.Append("   AND A.ItemNo     = @ItemNo \n");
                }
                else
                {
                    sbQuery.Append("   AND A.ContractSeq  = @ContractSeq \n");
                }
                sbQuery.Append(""
                    + "   AND A.SummaryType  = 6   -- 6:요일별                                                      \n"
                    + " GROUP BY B.SummaryCode, B.SummaryName                                                       \n"
                    + "                                                                                             \n"
                    + "UNION ALL                                                                                    \n"
                    + "                                                                                             \n"
                    + "-- 연령별 집계                                                                               \n"
                    + "SELECT  6 AS TypeCode                                                                        \n"
                    + "       ,'6 연령별' AS TypeName                                                               \n"
                    + "       ,'연령별' AS SumType                                                                  \n"
                    + "       ,B.SummaryCode AS SumCode                                                             \n"
                    + "       ,B.SummaryName AS SumName                                                             \n"
                    + "       ,SUM(A.AdCnt) AS AdCnt                                                                \n"
                    + "       ,CONVERT(DECIMAL(9,2),(SUM(A.AdCnt) / CONVERT(float,@TotAdHit)) * 100) AS AdRate      \n"
                    + "       ,null AS HsCnt                                                                        \n"
                    + "       ,null AS Reach                                                                        \n"
                    + "       ,null AS Freq                                                                         \n"
                    + "       ,null AS GRP                                                                          \n"
                    + "       ,SUM(A.PgCnt) AS PgCnt                                                                \n"
                    + "       ,SUM(A.HitCnt) AS HitCnt                                                              \n"
                    + "       ,CONVERT(DECIMAL(9,2),(SUM(A.HitCnt) / CONVERT(float,@TotPgHit)) * 100) AS PgRate     \n"
                    + "  FROM SummaryAdTotality0     A with(NoLock)                                                 \n"
                    + "       INNER JOIN SummaryCode B with(NoLock) ON (A.SummaryType = B.SummaryType AND A.SummaryCode = B.SummaryCode)  \n"
                    + " WHERE 1 = 1                                                                                 \n"
                    );
                // 계약조회인지 개별광고조회인지
                if (Type.Equals("I"))
                {
                    sbQuery.Append("   AND A.ItemNo     = @ItemNo \n");
                }
                else
                {
                    sbQuery.Append("   AND A.ContractSeq  = @ContractSeq \n");
                }
                sbQuery.Append(""
                    + "   AND A.SummaryType  = 3   -- 3:연령별                                                                                 \n"
                    + " GROUP BY B.SummaryCode, B.SummaryName                                                                                  \n"
                    + "                                                                                                                        \n"
                    + "UNION ALL                                                                                                              \n"
                    + "                                                                                                                        \n"
                    + "-- 지역별 집계                                                                                                          \n"
                    + "SELECT  7 AS TypeCode                                                                                                   \n"
                    + "       ,'7 지역별' AS TypeName                                                                                                   \n"
                    + "       ,'지역별' AS SumType                                                                                             \n"
                    + "       ,TA.SummaryCode AS SumCode                                                                                       \n"
                    + "       ,TA.SummaryName AS SumName                                                                                       \n"
                    + "       ,ISNULL(SUM(TB.AdCnt),0) AS AdCnt                                                                                \n"
                    + "       ,ISNULL(CONVERT(DECIMAL(9,2),(SUM(TB.AdCnt) / CONVERT(float,@TotAdHit)) * 100),0) AS AdRate                      \n"
                    + "       ,null AS HsCnt                                                                                                   \n"
                    + "       ,null AS Reach                                                                                                   \n"
                    + "       ,null AS Freq                                                                                                    \n"
                    + "       ,null AS GRP                                                                                                     \n"
                    + "       ,MAX(TB.PgCnt) AS PgCnt                                                                                           \n"
                    + "       ,SUM(TB.HitCnt) AS HitCnt                                                                                         \n"
                    + "       ,CONVERT(DECIMAL(9,2),(SUM(TB.HitCnt) / CONVERT(float,@TotPgHit)) * 100) AS PgRate                                \n"
                    + "  FROM (SELECT SummaryCode, SummaryName FROM SummaryCode with(NoLock)  WHERE SummaryType = 5 AND Level = 1) TA        \n"
                    + "       LEFT JOIN                                                                                                        \n"
                    + "       (                                                                                                                \n"
                    + "        SELECT CASE B.Level WHEN 1 THEN B.SummaryCode                                                                   \n"
                    + "                            ELSE B.UpperCode END AS SummaryCode                                                         \n"
                    + "              ,A.AdCnt                                                                                                  \n"
                    + "              ,A.HitCnt                                                                                                  \n"
                    + "              ,A.PgCnt                                                                                                  \n"
                    + "              ,B.Level                                                                                                  \n"
                    + "          FROM SummaryAdTotality0 A with(NoLock)                                                                        \n"
                    + "               INNER JOIN SummaryCode B with(NoLock) ON (A.SummaryType = B.SummaryType AND A.SummaryCode = B.SummaryCode) \n"
                    + "          WHERE 1 = 1                                                                                          \n"
                    );
                // 계약조회인지 개별광고조회인지
                if (Type.Equals("I"))
                {
                    sbQuery.Append("           AND A.ItemNo       = @ItemNo \n");
                }
                else
                {
                    sbQuery.Append("           AND A.ContractSeq  = @ContractSeq \n");
                }
                sbQuery.Append(""
                    + "           AND A.SummaryType  = 5  -- 5:지역별                                                                          \n"
                    + "       ) TB                                                                                                             \n"
                    + "       ON (TA.SummaryCode = TB.SummaryCode)                                                                             \n"
                    + " GROUP BY TA.SummaryCode, TA.SummaryName                                                                                \n"
                    + "                                                                                                                        \n"
                    + "ORDER BY TypeCode, SumCode                                                                                              \n"
                    );


                // __DEBUG__
                _log.Debug(sbQuery.ToString());
                // __DEBUG__

                // 쿼리실행
                ds = new DataSet();
                _db.Timeout = 600;
                _db.ExecuteQuery(ds, sbQuery.ToString());

                // 결과 DataSet의 데이터모델에 복사
                summaryAdModel.ReportDataSet = ds.Copy();

                // 결과
                summaryAdModel.ResultCnt = Utility.GetDatasetCount(summaryAdModel.ReportDataSet);

                ds.Dispose();

                // 결과코드 셋트
                summaryAdModel.ResultCD = "0000";

                // __DEBUG__
                _log.Debug("<출력정보>");
                _log.Debug("ResultCnt:[" + summaryAdModel.ResultCnt + "]");
                // __DEBUG__

                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "GetSummaryAdTotality() End");
                _log.Debug("-----------------------------------------");
            }
            catch (Exception ex)
            {
                summaryAdModel.ResultCD = "3000";
                if (isNotReady)
                {
                    summaryAdModel.ResultDesc = "해당 기간은 데이터가 집계되지 않았습니다.";
                }
                else
                {
                    summaryAdModel.ResultDesc = "총기간 광고집행 집계 조회중 오류가 발생하였습니다";
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

        #region 일간 광고집행 집계

        /// <summary>
        ///  일간 광고집행 집계
        /// </summary>
        /// <param name="summaryAdModel"></param>
        public void GetSummaryAdDaily(HeaderModel header, SummaryAdModel summaryAdModel)
        {
            bool isNotReady = false; // 데이터가 집계가 되지않아 존재하지 않을때.
            try
            {
                StringBuilder sb = new StringBuilder();
                DataSet ds = new DataSet();

                // 데이터베이스를 OPEN한다
                _db.Open();
                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "GetSummaryAdDaily() Start");
                _log.Debug("-----------------------------------------");

                // 일자가 6자리 이상(yyyymmdd)이면 6자리로 만든다.
                if (summaryAdModel.SearchStartDay.Length > 6) summaryAdModel.SearchStartDay = summaryAdModel.SearchStartDay.Substring(2, 6);
                if (summaryAdModel.SearchEndDay.Length > 6) summaryAdModel.SearchEndDay = summaryAdModel.SearchEndDay.Substring(2, 6);

                // __DEBUG__
                _log.Debug("<입력정보>");
                _log.Debug("SearchMediaCode	  :[" + summaryAdModel.SearchMediaCode + "]");		// 검색 매체
                _log.Debug("SearchContractSeq :[" + summaryAdModel.SearchContractSeq + "]");		// 검색 광고번호
                _log.Debug("SearchCampaign    :[" + summaryAdModel.CampaignCode + "]");		// 검색 광고번호
                _log.Debug("SearchItemNo      :[" + summaryAdModel.SearchItemNo + "]");		// 검색 광고번호           
                _log.Debug("SearchStartDay    :[" + summaryAdModel.SearchStartDay + "]");		// 검색 집계시작 일자          
                _log.Debug("SearchEndDay      :[" + summaryAdModel.SearchEndDay + "]");		// 검색 집계종료 일자          
                // __DEBUG__

                #region 프로파일 리포팅 포함여부 첵크
                bool IsProfile = false;
                if (summaryAdModel.TotalUser == 1)
                {
                    IsProfile = true;
                }
                #endregion

                #region 셋탑수 가져오기
                sb.Append("\n " + "select   top 1 isnull(HouseTotal,0) as TotUsr");
                sb.Append("\n " + "from     SummaryBase where logday <= '" + summaryAdModel.SearchEndDay + "'");
                sb.Append("\n " + "order by logday desc");

                _db.ExecuteQuery(ds, sb.ToString());

                if (ds.Tables[0].Rows.Count == 0)
                {
                    isNotReady = true;
                    throw new Exception();
                }

                int TotUsr = Convert.ToInt32(ds.Tables[0].Rows[0]["TotUsr"].ToString());
                // 종료일기준 이용자수
                summaryAdModel.TotalUser = TotUsr;

                ds.Dispose();
                #endregion

                // 쿼리실행
                ds = new DataSet();

                #region [ 파라메터 설정 ]
                SqlParameter[] sqlParam = new SqlParameter[8];
                sqlParam[0] = new SqlParameter("@MenuLevel", SqlDbType.Int, 4);
                sqlParam[1] = new SqlParameter("@MediaCode", SqlDbType.Int, 4);
                sqlParam[2] = new SqlParameter("@LogDay", SqlDbType.Char, 6);
                sqlParam[3] = new SqlParameter("@BeginDay", SqlDbType.Char, 6);
                sqlParam[4] = new SqlParameter("@EndDay", SqlDbType.Char, 6);
                sqlParam[5] = new SqlParameter("@ItemNo", SqlDbType.Int, 4);
                sqlParam[6] = new SqlParameter("@ContractSeq", SqlDbType.Int, 4);
                sqlParam[7] = new SqlParameter("@CampaignCd", SqlDbType.Int, 4);

                sqlParam[0].Value = summaryAdModel.MenuLevel;
                sqlParam[1].Value = summaryAdModel.SearchMediaCode;
                sqlParam[2].Value = summaryAdModel.SearchStartDay;
                sqlParam[3].Value = summaryAdModel.SearchStartDay;
                sqlParam[4].Value = summaryAdModel.SearchEndDay;
                sqlParam[5].Value = Convert.ToInt32(summaryAdModel.SearchItemNo);
                sqlParam[6].Value = Convert.ToInt32(summaryAdModel.SearchContractSeq);
                sqlParam[7].Value = Convert.ToInt32(summaryAdModel.CampaignCode);
                #endregion

                _db.Timeout = 60 * 30;

                if (IsProfile)
                {
                    _db.ExecuteProcedureParams(ds, "dbo.dao_SummaryAd_Pro", sqlParam);
                }
                else
                {
                    _db.ExecuteProcedureParams(ds, "dbo.dao_SummaryAd", sqlParam);
                }

                summaryAdModel.ReportDataSet = ds.Copy();
                summaryAdModel.ResultCnt = Utility.GetDatasetCount(summaryAdModel.ReportDataSet);

                ds.Dispose();

                summaryAdModel.ResultCD = "0000";

                // __DEBUG__
                _log.Debug("<출력정보>");
                _log.Debug("ResultCnt:[" + summaryAdModel.ResultCnt + "]");
                // __DEBUG__

                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "GetSummaryAdDaily() End");
                _log.Debug("-----------------------------------------");
            }
            catch (Exception ex)
            {
                summaryAdModel.ResultCD = "3000";
                if (isNotReady)
                {
                    summaryAdModel.ResultDesc = "해당 기간은 데이터가 집계되지 않았습니다.";
                }
                else
                {
                    summaryAdModel.ResultDesc = ex.Message;
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

        #region 주간 광고집행 집계
        /// <summary>
        ///  주간 광고집행 집계
        /// </summary>
        /// <param name="summaryAdModel"></param>
        public void GetSummaryAdWeekly(HeaderModel header, SummaryAdModel summaryAdModel)
        {
            bool isNotReady = false; // 데이터가 집계가 되지않아 존재하지 않을때.
            try
            {
                StringBuilder sbQuery = null;

                // 데이터베이스를 OPEN한다
                _db.Open();


                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "GetSummaryAdWeekly() Start");
                _log.Debug("-----------------------------------------");

                // 일자가 6자리 이상(yyyymmdd)이면 6자리로 만든다.
                if (summaryAdModel.SearchStartDay.Length > 6) summaryAdModel.SearchStartDay = summaryAdModel.SearchStartDay.Substring(2, 6);
                if (summaryAdModel.SearchEndDay.Length > 6) summaryAdModel.SearchEndDay = summaryAdModel.SearchEndDay.Substring(2, 6);

                // __DEBUG__
                _log.Debug("<입력정보>");
                _log.Debug("SearchMediaCode	  :[" + summaryAdModel.SearchMediaCode + "]");		// 검색 매체
                _log.Debug("SearchContractSeq :[" + summaryAdModel.SearchContractSeq + "]");		// 검색 광고번호           
                _log.Debug("SearchItemNo      :[" + summaryAdModel.SearchItemNo + "]");		// 검색 광고번호           
                _log.Debug("SearchStartDay    :[" + summaryAdModel.SearchStartDay + "]");		// 검색 집계시작 일자          
                _log.Debug("SearchEndDay      :[" + summaryAdModel.SearchEndDay + "]");		// 검색 집계종료 일자          
                // __DEBUG__

                string MediaCode = summaryAdModel.SearchMediaCode;
                string ContractSeq = summaryAdModel.SearchContractSeq;
                string ItemNo = summaryAdModel.SearchItemNo;
                string StartDay = summaryAdModel.SearchStartDay;
                string EndDay = summaryAdModel.SearchEndDay;

                string Type = "C"; // 계약건벌

                // 광고번호가 있으면 광고내역건별 조회이다.
                if (!ItemNo.Equals("") && !ItemNo.Equals("00"))
                {
                    Type = "I";
                }

                // 쿼리생성
                sbQuery = new StringBuilder();
                sbQuery.Append("\n"
                    + "-- 전체 이용가구수             \n"
                    + "SELECT ISNULL(HouseTotal,0) AS TotUsr \n"
                    + "  FROM SummaryBase  with(NoLock)  \n"
                    + " WHERE LogDay = '" + StartDay + "' \n"
                    );

                // __DEBUG__
                _log.Debug(sbQuery.ToString());
                // __DEBUG__

                // 쿼리실행
                DataSet ds = new DataSet();
                _db.ExecuteQuery(ds, sbQuery.ToString());

                if (ds.Tables[0].Rows.Count == 0)
                {
                    isNotReady = true;
                    throw new Exception();
                }

                int TotUsr = Convert.ToInt32(ds.Tables[0].Rows[0]["TotUsr"].ToString());
                ds.Dispose();

                // 종료일기준 이용자수
                summaryAdModel.TotalUser = TotUsr;

                // 쿼리생성
                sbQuery = new StringBuilder();
                sbQuery.Append("\n"
                    + "/* 주간 광고집행 집계                                     \n"
                    + "   조회조건 : 매체코드, 일자, 계약번호 또는 광고번호 */        \n"
                    + "                                                       \n"
                    + "DECLARE @MediaCode int;   -- 매체코드                    \n"
                    + "DECLARE @LogDay CHAR(6);  -- 집행일자                    \n"
                    + "DECLARE @ItemNo int;      -- 광고번호                    \n"
                    + "DECLARE @ContractSeq int; -- 계약번호                    \n"
                    + "DECLARE @TotUsr int;      -- 전체 이용가구수              \n"
                    + "DECLARE @TotAdHit int;    -- 전체 광고노출수              \n"
                    + "DECLARE @TotPgHit int;    -- 전체 프로그램시청수           \n"
                    + "                                          \n"
                    + "SET @MediaCode   =  " + MediaCode + "  \n"
                    + "SET @LogDay      = '" + StartDay + "' \n"
                    + "SET @ItemNo      =  " + ItemNo + "  \n"
                    + "SET @ContractSeq =  " + ContractSeq + "  \n"
                    + "SET @TotUsr      =  " + TotUsr.ToString() + " \n"
                    + "SET @TotAdHit    = 0;                                                         \n"
                    + "SET @TotPgHit    = 0;                                                         \n"
                    + "                                                                              \n"
                    + "                                                                              \n"
                    + "-- 전체 광고Hit                                                               \n"
                    + "SELECT @TotAdHit = SUM(A.AdCnt)                                               \n"
                    + "      ,@TotPgHit = SUM(A.HitCnt)                                              \n"
                    + "  FROM SummaryAdWeekly0 A with(NoLock)                              \n"
                    + " WHERE LogDay       = @LogDay                                                 \n"
                    );
                // 계약조회인지 개별광고조회인지
                if (Type.Equals("I"))
                {
                    sbQuery.Append("   AND A.ItemNo       = @ItemNo \n");
                }
                else
                {
                    sbQuery.Append("   AND A.ContractSeq  = @ContractSeq \n");
                }
                sbQuery.Append(""
                    + "   AND A.SummaryType  = 1                                                                                               \n"
                    + "                                                                                                                        \n"
                    + "-- 상품별 합계                                                                                                          \n"
                    + "SELECT  1 AS TypeCode                                                                                                   \n"
                    + "       ,'1 상품' AS TypeName                                                                                                   \n"
                    + "       ,'상품' AS SumType                                                                                               \n"
                    + "       ,B.SummaryCode AS SumCode                                                                                        \n"
                    + "       ,B.SummaryName AS SumName                                                                                        \n"
                    + "       ,SUM(A.AdCnt) AS AdCnt                                                                                           \n"
                    + "       ,CONVERT(DECIMAL(9,2),(SUM(A.AdCnt) / CONVERT(float,@TotAdHit)) * 100) AS AdRate                                 \n"
                    + "       ,MAX(A.AdHouseHold) AS HsCnt                                                                                     \n"
                    + "       ,CONVERT(DECIMAL(9,2),(SUM(A.AdHouseHold) / CONVERT(float,@TotUsr)) * 100) AS Reach                              \n"
                    + "       ,CONVERT(DECIMAL(9,2),(SUM(A.AdCnt) / CONVERT(float,SUM(A.AdHouseHold)))) AS Freq                                \n"
                    + "       ,CONVERT(DECIMAL(9,2),(SUM(A.AdCnt) / CONVERT(float,@TotUsr)) * 100) AS GRP                                      \n"
                    + "       ,SUM(A.PgCnt) AS PgCnt                                                                                           \n"
                    + "       ,SUM(A.HitCnt) AS HitCnt                                                                                         \n"
                    + "       ,CONVERT(DECIMAL(9,2),(SUM(A.HitCnt) / CONVERT(float,@TotPgHit)) * 100) AS PgRate                                \n"
                    + "  FROM SummaryAdWeekly0       A with(NoLock) \n"
                    + "       INNER JOIN SummaryCode B with(NoLock) ON (A.SummaryType = B.SummaryType AND A.SummaryCode = B.SummaryCode)  \n"
                    + " WHERE A.LogDay       = @LogDay                                                                                         \n"
                    );
                // 계약조회인지 개별광고조회인지
                if (Type.Equals("I"))
                {
                    sbQuery.Append("   AND A.ItemNo       = @ItemNo \n");
                }
                else
                {
                    sbQuery.Append("   AND A.ContractSeq  = @ContractSeq \n");
                }
                sbQuery.Append(""
                    + "   AND A.SummaryType  = 1 -- 1:상품별                                                                                   \n"
                    + " GROUP BY B.SummaryCode, B.SummaryName                                                                                  \n"
                    + "                                                                                                                        \n"
                    + "UNION                                                                                                                   \n"
                    + "                                                                                                                        \n"
                    + "-- 전체 합계                                                                                                            \n"
                    + "SELECT  2 AS TypeCode                                                                                                   \n"
                    + "       ,'2 합계' AS TypeName                                                                                                   \n"
                    + "       ,'합계' AS SumType                                                                                               \n"
                    + "       ,0 AS SumCode                                                                                                    \n"
                    + "       ,'' AS SumName                                                                                                   \n"
                    + "       ,SUM(A.AdCnt) AS AdCnt                                                                                           \n"
                    + "       ,null AS AdRate                                                                                                  \n"
                    + "       ,null AS HsCnt                                                                                                   \n"
                    + "       ,null AS Reach                                                                                                   \n"
                    + "       ,null AS Freq                                                                                                    \n"
                    + "       ,null AS GRP                                                                                                     \n"
                    + "       ,null AS PgCnt                                                                                                   \n"
                    + "       ,null AS HitCnt                                                                                                  \n"
                    + "       ,null AS PgRate                                                                                                  \n"
                    + "  FROM SummaryAdWeekly0       A with(NoLock)  \n"
                    + "       INNER JOIN SummaryCode B with(NoLock) ON (A.SummaryType = B.SummaryType AND A.SummaryCode = B.SummaryCode)  \n"
                    + " WHERE A.LogDay       = @LogDay                                                                                         \n"
                    );
                // 계약조회인지 개별광고조회인지
                if (Type.Equals("I"))
                {
                    sbQuery.Append("   AND A.ItemNo       = @ItemNo \n");
                }
                else
                {
                    sbQuery.Append("   AND A.ContractSeq  = @ContractSeq \n");
                }
                sbQuery.Append(""
                    + "   AND A.SummaryType  = 1 -- 1:상품별                                                                                   \n"
                    + "                                                                                                                        \n"
                    + "UNION                                                                                                                   \n"
                    + "                                                                                                                        \n"
                    + "-- 카테고리별 집계                                                                                                      \n"
                    + "SELECT  3 AS TypeCode                                                                                                   \n"
                    + "       ,'3 로딩광고요약' AS TypeName                                                                                                   \n"
                    + "       ,'로딩광고요약' AS SumType                                                                                       \n"
                    + "       ,B.CategoryCode AS SumCode                                                                                       \n"
                    + "       ,B.CategoryName AS SumName                                                                                       \n"
                    + "       ,SUM(A.AdCnt) AS AdCnt                                                                                           \n"
                    + "       ,CONVERT(DECIMAL(9,2),(SUM(A.AdCnt) / CONVERT(float,@TotAdHit)) * 100) AS AdRate                                 \n"
                    + "       ,MAX(A.AdHouseHold) AS HsCnt                                                                                     \n"
                    + "       ,CONVERT(DECIMAL(9,2),(SUM(A.AdHouseHold) / CONVERT(float,@TotUsr)) * 100) AS Reach                              \n"
                    + "       ,CONVERT(DECIMAL(9,2),(SUM(A.AdCnt) / CONVERT(float,SUM(A.AdHouseHold)))) AS Freq                                \n"
                    + "       ,CONVERT(DECIMAL(9,2),(SUM(A.AdCnt) / CONVERT(float,@TotUsr)) * 100) AS GRP                                      \n"
                    + "       ,SUM(A.PgCnt) AS PgCnt                                                                                           \n"
                    + "       ,SUM(A.HitCnt) AS HitCnt                                                                                         \n"
                    + "       ,CONVERT(DECIMAL(9,2),(SUM(A.HitCnt) / CONVERT(float,@TotPgHit)) * 100) AS PgRate                                \n"
                    + "  FROM SummaryAdWeekly1    A with(NoLock) \n"
                    + "       INNER JOIN AdTargetsHanaTV.dbo.Category B with(NoLock) ON (A.Category = B.CategoryCode)                                         \n"
                    + " WHERE A.LogDay       = @LogDay                                                                                         \n"
                    );
                // 계약조회인지 개별광고조회인지
                if (Type.Equals("I"))
                {
                    sbQuery.Append("   AND A.ItemNo       = @ItemNo \n");
                }
                else
                {
                    sbQuery.Append("   AND A.ContractSeq  = @ContractSeq \n");
                }
                sbQuery.Append(""
                    + " GROUP BY B.CategoryCode, B.CategoryName                                                                                \n"
                    + "                                                                                                                        \n"
                    + "UNION                                                                                                                   \n"
                    + "                                                                                                                        \n"
                    + "-- 초수별 집계                                                                                                          \n"
                    + "SELECT  4 AS TypeCode                                                                                                   \n"
                    + "       ,'4 초수' AS TypeName                                                                                                   \n"
                    + "       ,'초수' AS SumType                                                                                               \n"
                    + "       ,B.AdTime AS SumCode                                                                                             \n"
                    + "       ,Convert(VARCHAR(5),B.AdTime) + '초' AS SumName                                                                  \n"
                    + "       ,SUM(A.AdCnt) AS AdCnt                                                                                           \n"
                    + "       ,CONVERT(DECIMAL(9,2),(SUM(A.AdCnt) / CONVERT(float,@TotAdHit)) * 100) AS AdRate                                 \n"
                    + "       ,null AS HsCnt                                                                                                   \n"
                    + "       ,null AS Reach                                                                                                   \n"
                    + "       ,null AS Freq                                                                                                    \n"
                    + "       ,null AS GRP                                                                                                     \n"
                    + "       ,SUM(A.PgCnt) AS PgCnt                                                                                           \n"
                    + "       ,SUM(A.HitCnt) AS HitCnt                                                                                         \n"
                    + "       ,CONVERT(DECIMAL(9,2),(SUM(A.HitCnt) / CONVERT(float,@TotPgHit)) * 100) AS PgRate                                \n"
                    + "  FROM SummaryAdWeekly0        A with(NoLock)  \n"
                    + "       INNER JOIN AdTargetsHanaTV.dbo.ContractItem B with(NoLock)  ON (A.ItemNo = B.ItemNo)                                             \n"
                    + " WHERE A.LogDay       = @LogDay                                                                                         \n"
                    );
                // 계약조회인지 개별광고조회인지
                if (Type.Equals("I"))
                {
                    sbQuery.Append("   AND A.ItemNo       = @ItemNo \n");
                }
                else
                {
                    sbQuery.Append("   AND A.ContractSeq  = @ContractSeq \n");
                }
                sbQuery.Append(""
                    + "   AND A.SummaryType  = 1 -- 1:상품별                                                                                   \n"
                    + " GROUP BY B.AdTime                                                                                                      \n"
                    + "                                                                                                                        \n"
                    + "UNION                                                                                                                   \n"
                    + "                                                                                                                        \n"
                    + "-- 요일별 집계                                                                                                          \n"
                    + "SELECT  5 AS TypeCode                                                                                                   \n"
                    + "       ,'5 요일' AS TypeName                                                                                                   \n"
                    + "       ,'요일' AS SumType                                                                                               \n"
                    + "       ,B.SummaryCode AS SumCode                                                                                        \n"
                    + "       ,B.SummaryName AS SumName                                                                                        \n"
                    + "       ,SUM(A.AdCnt) AS AdCnt                                                                                           \n"
                    + "       ,CONVERT(DECIMAL(9,2),(SUM(A.AdCnt) / CONVERT(float,@TotAdHit)) * 100) AS AdRate                                 \n"
                    + "       ,null AS HsCnt                                                                                                   \n"
                    + "       ,null AS Reach                                                                                                   \n"
                    + "       ,null AS Freq                                                                                                    \n"
                    + "       ,null AS GRP                                                                                                     \n"
                    + "       ,SUM(A.PgCnt) AS PgCnt                                                                                           \n"
                    + "       ,SUM(A.HitCnt) AS HitCnt                                                                                         \n"
                    + "       ,CONVERT(DECIMAL(9,2),(SUM(A.HitCnt) / CONVERT(float,@TotPgHit)) * 100) AS PgRate                                \n"
                    + "  FROM SummaryAdWeekly0 A with(NoLock)  \n"
                    + "       INNER JOIN SummaryCode B with(NoLock) ON (A.SummaryType = B.SummaryType AND A.SummaryCode = B.SummaryCode)                   \n"
                    + " WHERE A.LogDay       = @LogDay                                                                                         \n"
                    );
                // 계약조회인지 개별광고조회인지
                if (Type.Equals("I"))
                {
                    sbQuery.Append("   AND A.ItemNo       = @ItemNo \n");
                }
                else
                {
                    sbQuery.Append("   AND A.ContractSeq  = @ContractSeq \n");
                }
                sbQuery.Append(""
                    + "   AND A.SummaryType  = 6   -- 6:요일별                                                                                 \n"
                    + " GROUP BY B.SummaryCode, B.SummaryName                                                                                  \n"
                    + "                                                                                                                        \n"
                    + "UNION                                                                                                                   \n"
                    + "                                                                                                                        \n"
                    + "-- 연령별 집계                                                                                                          \n"
                    + "SELECT  6 AS TypeCode                                                                                                   \n"
                    + "       ,'6 연령별' AS TypeName                                                                                                   \n"
                    + "       ,'연령별' AS SumType                                                                                             \n"
                    + "       ,B.SummaryCode AS SumCode                                                                                        \n"
                    + "       ,B.SummaryName AS SumName                                                                                        \n"
                    + "       ,SUM(A.AdCnt) AS AdCnt                                                                                           \n"
                    + "       ,CONVERT(DECIMAL(9,2),(SUM(A.AdCnt) / CONVERT(float,@TotAdHit)) * 100) AS AdRate                                 \n"
                    + "       ,null AS HsCnt                                                                                                   \n"
                    + "       ,null AS Reach                                                                                                   \n"
                    + "       ,null AS Freq                                                                                                    \n"
                    + "       ,null AS GRP                                                                                                     \n"
                    + "       ,SUM(A.PgCnt) AS PgCnt                                                                                           \n"
                    + "       ,SUM(A.HitCnt) AS HitCnt                                                                                         \n"
                    + "       ,CONVERT(DECIMAL(9,2),(SUM(A.HitCnt) / CONVERT(float,@TotPgHit)) * 100) AS PgRate                                \n"
                    + "  FROM SummaryAdWeekly0       A with(NoLock)  \n"
                    + "       INNER JOIN SummaryCode B with(NoLock) ON (A.SummaryType = B.SummaryType AND A.SummaryCode = B.SummaryCode)                   \n"
                    + " WHERE A.LogDay       = @LogDay                                                                                         \n"
                    );
                // 계약조회인지 개별광고조회인지
                if (Type.Equals("I"))
                {
                    sbQuery.Append("   AND A.ItemNo       = @ItemNo \n");
                }
                else
                {
                    sbQuery.Append("   AND A.ContractSeq  = @ContractSeq \n");
                }
                sbQuery.Append(""
                    + "   AND A.SummaryType  = 3   -- 3:연령별                                                                                 \n"
                    + " GROUP BY B.SummaryCode, B.SummaryName                                                                                  \n"
                    + "                                                                                                                        \n"
                    + "UNION                                                                                                                   \n"
                    + "                                                                                                                        \n"
                    + "-- 지역별 집계                                                                                                          \n"
                    + "SELECT  7 AS TypeCode                                                                                                   \n"
                    + "       ,'7 지역별' AS TypeName                                                                                                   \n"
                    + "       ,'지역별' AS SumType                                                                                             \n"
                    + "       ,TA.SummaryCode AS SumCode                                                                                       \n"
                    + "       ,TA.SummaryName AS SumName                                                                                       \n"
                    + "       ,ISNULL(SUM(TB.AdCnt),0) AS AdCnt                                                                                \n"
                    + "       ,ISNULL(CONVERT(DECIMAL(9,2),(SUM(TB.AdCnt) / CONVERT(float,@TotAdHit)) * 100),0) AS AdRate                      \n"
                    + "       ,null AS HsCnt                                                                                                   \n"
                    + "       ,null AS Reach                                                                                                   \n"
                    + "       ,null AS Freq                                                                                                    \n"
                    + "       ,null AS GRP                                                                                                     \n"
                    + "       ,MAX(TB.PgCnt) AS PgCnt                                                                                           \n"
                    + "       ,SUM(TB.HitCnt) AS HitCnt                                                                                         \n"
                    + "       ,CONVERT(DECIMAL(9,2),(SUM(TB.HitCnt) / CONVERT(float,@TotPgHit)) * 100) AS PgRate                                \n"
                    + "  FROM (SELECT SummaryCode, SummaryName FROM SummaryCode with(NoLock) WHERE SummaryType = 5 AND Level = 1) TA                        \n"
                    + "       LEFT JOIN                                                                                                        \n"
                    + "       (                                                                                                                \n"
                    + "        SELECT CASE B.Level WHEN 1 THEN B.SummaryCode                                                                   \n"
                    + "                            ELSE B.UpperCode END AS SummaryCode                                                         \n"
                    + "              ,A.AdCnt                                                                                                  \n"
                    + "              ,A.HitCnt                                                                                                  \n"
                    + "              ,A.PgCnt                                                                                                  \n"
                    + "              ,B.Level                                                                                                  \n"
                    + "          FROM SummaryAdWeekly0       A with(NoLock) \n"
                    + "               INNER JOIN SummaryCode B with(NoLock) ON (A.SummaryType = B.SummaryType AND A.SummaryCode = B.SummaryCode)           \n"
                    + "         WHERE A.LogDay       = @LogDay                                                                                 \n"
                    );
                // 계약조회인지 개별광고조회인지
                if (Type.Equals("I"))
                {
                    sbQuery.Append("           AND A.ItemNo       = @ItemNo \n");
                }
                else
                {
                    sbQuery.Append("           AND A.ContractSeq  = @ContractSeq \n");
                }
                sbQuery.Append(""
                    + "           AND A.SummaryType  = 5  -- 5:지역별                                                                          \n"
                    + "       ) TB                                                                                                             \n"
                    + "       ON (TA.SummaryCode = TB.SummaryCode)                                                                             \n"
                    + " GROUP BY TA.SummaryCode, TA.SummaryName                                                                                \n"
                    + "                                                                                                                        \n"
                    + "ORDER BY TypeCode, SumCode                                                                                              \n"
                    );


                // __DEBUG__
                _log.Debug(sbQuery.ToString());
                // __DEBUG__

                // 쿼리실행
                ds = new DataSet();
                _db.ExecuteQuery(ds, sbQuery.ToString());

                // 결과 DataSet의 데이터모델에 복사
                summaryAdModel.ReportDataSet = ds.Copy();

                // 결과
                summaryAdModel.ResultCnt = Utility.GetDatasetCount(summaryAdModel.ReportDataSet);

                ds.Dispose();

                // 결과코드 셋트
                summaryAdModel.ResultCD = "0000";

                // __DEBUG__
                _log.Debug("<출력정보>");
                _log.Debug("ResultCnt:[" + summaryAdModel.ResultCnt + "]");
                // __DEBUG__

                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "GetSummaryAdWeekly() End");
                _log.Debug("-----------------------------------------");
            }
            catch (Exception ex)
            {
                summaryAdModel.ResultCD = "3000";
                if (isNotReady)
                {
                    summaryAdModel.ResultDesc = "해당 기간은 데이터가 집계되지 않았습니다.";
                }
                else
                {
                    summaryAdModel.ResultDesc = "주간 광고집행 집계 조회중 오류가 발생하였습니다";
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

        #region 월간 광고집행 집계
        /// <summary>
        ///  월간 광고집행 집계
        /// </summary>
        /// <param name="summaryAdModel"></param>
        public void GetSummaryAdMonthly(HeaderModel header, SummaryAdModel summaryAdModel)
        {
            bool isNotReady = false; // 데이터가 집계가 되지않아 존재하지 않을때.
            try
            {
                StringBuilder sbQuery = null;

                // 데이터베이스를 OPEN한다
                _db.Open();


                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "GetSummaryAdMonthly() Start");
                _log.Debug("-----------------------------------------");

                // 일자가 6자리 이상(yyyymmdd)이면 6자리로 만든다.
                if (summaryAdModel.SearchStartDay.Length > 6) summaryAdModel.SearchStartDay = summaryAdModel.SearchStartDay.Substring(2, 6);
                if (summaryAdModel.SearchEndDay.Length > 6) summaryAdModel.SearchEndDay = summaryAdModel.SearchEndDay.Substring(2, 6);

                // __DEBUG__
                _log.Debug("<입력정보>");
                _log.Debug("SearchMediaCode	  :[" + summaryAdModel.SearchMediaCode + "]");		// 검색 매체
                _log.Debug("SearchContractSeq :[" + summaryAdModel.SearchContractSeq + "]");		// 검색 광고번호           
                _log.Debug("SearchItemNo      :[" + summaryAdModel.SearchItemNo + "]");		// 검색 광고번호           
                _log.Debug("SearchStartDay    :[" + summaryAdModel.SearchStartDay + "]");		// 검색 집계시작 일자          
                _log.Debug("SearchEndDay      :[" + summaryAdModel.SearchEndDay + "]");		// 검색 집계종료 일자          
                // __DEBUG__

                string MediaCode = summaryAdModel.SearchMediaCode;
                string ContractSeq = summaryAdModel.SearchContractSeq;
                string ItemNo = summaryAdModel.SearchItemNo;
                string StartDay = summaryAdModel.SearchStartDay;
                string EndDay = summaryAdModel.SearchEndDay;

                string Type = "C"; // 계약건벌

                // 광고번호가 있으면 광고내역건별 조회이다.
                if (!ItemNo.Equals("") && !ItemNo.Equals("00"))
                {
                    Type = "I";
                }

                // 쿼리생성
                sbQuery = new StringBuilder();
                sbQuery.Append("\n"
                    + "-- 전체 이용가구수             \n"
                    + "SELECT ISNULL(HouseTotal,0) AS TotUsr \n"
                    + "  FROM SummaryBase with(NoLock)  \n"
                    + " WHERE LogDay = '" + StartDay + "' \n"
                    );

                // __DEBUG__
                _log.Debug(sbQuery.ToString());
                // __DEBUG__

                // 쿼리실행
                DataSet ds = new DataSet();
                _db.ExecuteQuery(ds, sbQuery.ToString());

                if (ds.Tables[0].Rows.Count == 0)
                {
                    isNotReady = true;
                    throw new Exception();
                }

                int TotUsr = Convert.ToInt32(ds.Tables[0].Rows[0]["TotUsr"].ToString());
                ds.Dispose();

                // 종료일기준 이용자수
                summaryAdModel.TotalUser = TotUsr;

                // 쿼리생성
                sbQuery = new StringBuilder();
                sbQuery.Append("\n"
                    + "/* 주간 광고집행 집계                                     \n"
                    + "   조회조건 : 매체코드, 일자, 계약번호 또는 광고번호 */        \n"
                    + "                                                       \n"
                    + "DECLARE @MediaCode int;   -- 매체코드                    \n"
                    + "DECLARE @LogDay CHAR(6);  -- 집행일자                    \n"
                    + "DECLARE @ItemNo int;      -- 광고번호                    \n"
                    + "DECLARE @ContractSeq int; -- 계약번호                    \n"
                    + "DECLARE @TotUsr int;      -- 전체 이용가구수              \n"
                    + "DECLARE @TotAdHit int;    -- 전체 광고노출수              \n"
                    + "DECLARE @TotPgHit int;    -- 전체 프로그램시청수           \n"
                    + "                                          \n"
                    + "SET @MediaCode   =  " + MediaCode + "  \n"
                    + "SET @LogDay      = '" + StartDay + "' \n"
                    + "SET @ItemNo      =  " + ItemNo + "  \n"
                    + "SET @ContractSeq =  " + ContractSeq + "  \n"
                    + "SET @TotUsr      =  " + TotUsr.ToString() + " \n"
                    + "SET @TotAdHit    = 0;                                                         \n"
                    + "SET @TotPgHit    = 0;                                                         \n"
                    + "                                                                              \n"
                    + "                                                                              \n"
                    + "-- 전체 광고Hit                                                               \n"
                    + "SELECT @TotAdHit = SUM(A.AdCnt)                                               \n"
                    + "      ,@TotPgHit = SUM(A.HitCnt)                                              \n"
                    + "  FROM SummaryAdMonthly0 A with(NoLock)                                     \n"
                    + " WHERE LogDay       = @LogDay                                                 \n"
                    );
                // 계약조회인지 개별광고조회인지
                if (Type.Equals("I"))
                {
                    sbQuery.Append("   AND A.ItemNo       = @ItemNo \n");
                }
                else
                {
                    sbQuery.Append("   AND A.ContractSeq  = @ContractSeq \n");
                }
                sbQuery.Append(""
                    + "   AND A.SummaryType  = 1                                                                                               \n"
                    + "                                                                                                                        \n"
                    + "-- 상품별 합계                                                                                                          \n"
                    + "SELECT  1 AS TypeCode                                                                                                   \n"
                    + "       ,'1 상품' AS TypeName                                                                                                   \n"
                    + "       ,'상품' AS SumType                                                                                               \n"
                    + "       ,B.SummaryCode AS SumCode                                                                                        \n"
                    + "       ,B.SummaryName AS SumName                                                                                        \n"
                    + "       ,SUM(A.AdCnt) AS AdCnt                                                                                           \n"
                    + "       ,CONVERT(DECIMAL(9,2),(SUM(A.AdCnt) / CONVERT(float,@TotAdHit)) * 100) AS AdRate                                 \n"
                    + "       ,MAX(A.AdHouseHold) AS HsCnt                                                                                     \n"
                    + "       ,CONVERT(DECIMAL(9,2),(SUM(A.AdHouseHold) / CONVERT(float,@TotUsr)) * 100) AS Reach                              \n"
                    + "       ,CONVERT(DECIMAL(9,2),(SUM(A.AdCnt) / CONVERT(float,SUM(A.AdHouseHold)))) AS Freq                                \n"
                    + "       ,CONVERT(DECIMAL(9,2),(SUM(A.AdCnt) / CONVERT(float,@TotUsr)) * 100) AS GRP                                      \n"
                    + "       ,SUM(A.PgCnt) AS PgCnt                                                                                           \n"
                    + "       ,SUM(A.HitCnt) AS HitCnt                                                                                         \n"
                    + "       ,CONVERT(DECIMAL(9,2),(SUM(A.HitCnt) / CONVERT(float,@TotPgHit)) * 100) AS PgRate                                \n"
                    + "  FROM SummaryAdMonthly0      A with(NoLock) \n"
                    + "       INNER JOIN SummaryCode B with(NoLock) ON (A.SummaryType = B.SummaryType AND A.SummaryCode = B.SummaryCode)  \n"
                    + " WHERE A.LogDay       = @LogDay                                                                                         \n"
                    );
                // 계약조회인지 개별광고조회인지
                if (Type.Equals("I"))
                {
                    sbQuery.Append("   AND A.ItemNo       = @ItemNo \n");
                }
                else
                {
                    sbQuery.Append("   AND A.ContractSeq  = @ContractSeq \n");
                }
                sbQuery.Append(""
                    + "   AND A.SummaryType  = 1 -- 1:상품별                                                                                   \n"
                    + " GROUP BY B.SummaryCode, B.SummaryName                                                                                  \n"
                    + "                                                                                                                        \n"
                    + "UNION                                                                                                                   \n"
                    + "                                                                                                                        \n"
                    + "-- 전체 합계                                                                                                            \n"
                    + "SELECT  2 AS TypeCode                                                                                                   \n"
                    + "       ,'2 합계' AS TypeName                                                                                                   \n"
                    + "       ,'합계' AS SumType                                                                                               \n"
                    + "       ,0 AS SumCode                                                                                                    \n"
                    + "       ,'' AS SumName                                                                                                   \n"
                    + "       ,SUM(A.AdCnt) AS AdCnt                                                                                           \n"
                    + "       ,null AS AdRate                                                                                                  \n"
                    + "       ,null AS HsCnt                                                                                                   \n"
                    + "       ,null AS Reach                                                                                                   \n"
                    + "       ,null AS Freq                                                                                                    \n"
                    + "       ,null AS GRP                                                                                                     \n"
                    + "       ,null AS PgCnt                                                                                                   \n"
                    + "       ,null AS HitCnt                                                                                                  \n"
                    + "       ,null AS PgRate                                                                                                  \n"
                    + "  FROM SummaryAdMonthly0      A with(NoLock) \n"
                    + "       INNER JOIN SummaryCode B with(NoLock) ON (A.SummaryType = B.SummaryType AND A.SummaryCode = B.SummaryCode)  \n"
                    + " WHERE A.LogDay       = @LogDay                                                                                         \n"
                    );
                // 계약조회인지 개별광고조회인지
                if (Type.Equals("I"))
                {
                    sbQuery.Append("   AND A.ItemNo       = @ItemNo \n");
                }
                else
                {
                    sbQuery.Append("   AND A.ContractSeq  = @ContractSeq \n");
                }
                sbQuery.Append(""
                    + "   AND A.SummaryType  = 1 -- 1:상품별                                                                                   \n"
                    + "                                                                                                                        \n"
                    + "UNION                                                                                                                   \n"
                    + "                                                                                                                        \n"
                    + "-- 카테고리별 집계                                                                                                      \n"
                    + "SELECT  3 AS TypeCode                                                                                                   \n"
                    + "       ,'3 로딩광고요약' AS TypeName                                                                                                   \n"
                    + "       ,'로딩광고요약' AS SumType                                                                                       \n"
                    + "       ,B.CategoryCode AS SumCode                                                                                       \n"
                    + "       ,B.CategoryName AS SumName                                                                                       \n"
                    + "       ,SUM(A.AdCnt) AS AdCnt                                                                                           \n"
                    + "       ,CONVERT(DECIMAL(9,2),(SUM(A.AdCnt) / CONVERT(float,@TotAdHit)) * 100) AS AdRate                                 \n"
                    + "       ,MAX(A.AdHouseHold) AS HsCnt                                                                                     \n"
                    + "       ,CONVERT(DECIMAL(9,2),(SUM(A.AdHouseHold) / CONVERT(float,@TotUsr)) * 100) AS Reach                              \n"
                    + "       ,CONVERT(DECIMAL(9,2),(SUM(A.AdCnt) / CONVERT(float,SUM(A.AdHouseHold)))) AS Freq                                \n"
                    + "       ,CONVERT(DECIMAL(9,2),(SUM(A.AdCnt) / CONVERT(float,@TotUsr)) * 100) AS GRP                                      \n"
                    + "       ,SUM(A.PgCnt) AS PgCnt                                                                                           \n"
                    + "       ,SUM(A.HitCnt) AS HitCnt                                                                                         \n"
                    + "       ,CONVERT(DECIMAL(9,2),(SUM(A.HitCnt) / CONVERT(float,@TotPgHit)) * 100) AS PgRate                                \n"
                    + "  FROM SummaryAdMonthly1   A with(NoLock) \n"
                    + "       INNER JOIN AdTargetsHanaTV.dbo.Category B with(NoLock) ON (A.Category = B.CategoryCode)                                         \n"
                    + " WHERE A.LogDay       = @LogDay                                                                                         \n"
                    );
                // 계약조회인지 개별광고조회인지
                if (Type.Equals("I"))
                {
                    sbQuery.Append("   AND A.ItemNo       = @ItemNo \n");
                }
                else
                {
                    sbQuery.Append("   AND A.ContractSeq  = @ContractSeq \n");
                }
                sbQuery.Append(""
                    + " GROUP BY B.CategoryCode, B.CategoryName                                                                                \n"
                    + "                                                                                                                        \n"
                    + "UNION                                                                                                                   \n"
                    + "                                                                                                                        \n"
                    + "-- 초수별 집계                                                                                                          \n"
                    + "SELECT  4 AS TypeCode                                                                                                   \n"
                    + "       ,'4 초수' AS TypeName                                                                                                   \n"
                    + "       ,'초수' AS SumType                                                                                               \n"
                    + "       ,B.AdTime AS SumCode                                                                                             \n"
                    + "       ,Convert(VARCHAR(5),B.AdTime) + '초' AS SumName                                                                  \n"
                    + "       ,SUM(A.AdCnt) AS AdCnt                                                                                           \n"
                    + "       ,CONVERT(DECIMAL(9,2),(SUM(A.AdCnt) / CONVERT(float,@TotAdHit)) * 100) AS AdRate                                 \n"
                    + "       ,null AS HsCnt                                                                                                   \n"
                    + "       ,null AS Reach                                                                                                   \n"
                    + "       ,null AS Freq                                                                                                    \n"
                    + "       ,null AS GRP                                                                                                     \n"
                    + "       ,SUM(A.PgCnt) AS PgCnt                                                                                           \n"
                    + "       ,SUM(A.HitCnt) AS HitCnt                                                                                         \n"
                    + "       ,CONVERT(DECIMAL(9,2),(SUM(A.HitCnt) / CONVERT(float,@TotPgHit)) * 100) AS PgRate                                \n"
                    + "  FROM SummaryAdMonthly0       A with(NoLock)  \n"
                    + "       INNER JOIN AdTargetsHanaTV.dbo.ContractItem B with(NoLock) ON (A.ItemNo = B.ItemNo)                                             \n"
                    + " WHERE A.LogDay       = @LogDay                                                                                         \n"
                    );
                // 계약조회인지 개별광고조회인지
                if (Type.Equals("I"))
                {
                    sbQuery.Append("   AND A.ItemNo       = @ItemNo \n");
                }
                else
                {
                    sbQuery.Append("   AND A.ContractSeq  = @ContractSeq \n");
                }
                sbQuery.Append(""
                    + "   AND A.SummaryType  = 1 -- 1:상품별                                                                                   \n"
                    + " GROUP BY B.AdTime                                                                                                      \n"
                    + "                                                                                                                        \n"
                    + "UNION                                                                                                                   \n"
                    + "                                                                                                                        \n"
                    + "-- 요일별 집계                                                                                                          \n"
                    + "SELECT  5 AS TypeCode                                                                                                   \n"
                    + "       ,'5 요일' AS TypeName                                                                                                   \n"
                    + "       ,'요일' AS SumType                                                                                               \n"
                    + "       ,B.SummaryCode AS SumCode                                                                                        \n"
                    + "       ,B.SummaryName AS SumName                                                                                        \n"
                    + "       ,SUM(A.AdCnt) AS AdCnt                                                                                           \n"
                    + "       ,CONVERT(DECIMAL(9,2),(SUM(A.AdCnt) / CONVERT(float,@TotAdHit)) * 100) AS AdRate                                 \n"
                    + "       ,null AS HsCnt                                                                                                   \n"
                    + "       ,null AS Reach                                                                                                   \n"
                    + "       ,null AS Freq                                                                                                    \n"
                    + "       ,null AS GRP                                                                                                     \n"
                    + "       ,SUM(A.PgCnt) AS PgCnt                                                                                           \n"
                    + "       ,SUM(A.HitCnt) AS HitCnt                                                                                         \n"
                    + "       ,CONVERT(DECIMAL(9,2),(SUM(A.HitCnt) / CONVERT(float,@TotPgHit)) * 100) AS PgRate                                \n"
                    + "  FROM SummaryAdMonthly0      A with(NoLock) \n"
                    + "       INNER JOIN SummaryCode B with(NoLock) ON (A.SummaryType = B.SummaryType AND A.SummaryCode = B.SummaryCode)                   \n"
                    + " WHERE A.LogDay       = @LogDay                                                                                         \n"
                    );
                // 계약조회인지 개별광고조회인지
                if (Type.Equals("I"))
                {
                    sbQuery.Append("   AND A.ItemNo       = @ItemNo \n");
                }
                else
                {
                    sbQuery.Append("   AND A.ContractSeq  = @ContractSeq \n");
                }
                sbQuery.Append(""
                    + "   AND A.SummaryType  = 6   -- 6:요일별                                                                                 \n"
                    + " GROUP BY B.SummaryCode, B.SummaryName                                                                                  \n"
                    + "                                                                                                                        \n"
                    + "UNION                                                                                                                   \n"
                    + "                                                                                                                        \n"
                    + "-- 연령별 집계                                                                                                          \n"
                    + "SELECT  6 AS TypeCode                                                                                                   \n"
                    + "       ,'6 연령별' AS TypeName                                                                                                   \n"
                    + "       ,'연령별' AS SumType                                                                                             \n"
                    + "       ,B.SummaryCode AS SumCode                                                                                        \n"
                    + "       ,B.SummaryName AS SumName                                                                                        \n"
                    + "       ,SUM(A.AdCnt) AS AdCnt                                                                                           \n"
                    + "       ,CONVERT(DECIMAL(9,2),(SUM(A.AdCnt) / CONVERT(float,@TotAdHit)) * 100) AS AdRate                                 \n"
                    + "       ,null AS HsCnt                                                                                                   \n"
                    + "       ,null AS Reach                                                                                                   \n"
                    + "       ,null AS Freq                                                                                                    \n"
                    + "       ,null AS GRP                                                                                                     \n"
                    + "       ,SUM(A.PgCnt) AS PgCnt                                                                                           \n"
                    + "       ,SUM(A.HitCnt) AS HitCnt                                                                                         \n"
                    + "       ,CONVERT(DECIMAL(9,2),(SUM(A.HitCnt) / CONVERT(float,@TotPgHit)) * 100) AS PgRate                                \n"
                    + "  FROM SummaryAdMonthly0      A with(NoLock) \n "
                    + "       INNER JOIN SummaryCode B with(NoLock) ON (A.SummaryType = B.SummaryType AND A.SummaryCode = B.SummaryCode)                   \n"
                    + " WHERE A.LogDay       = @LogDay                                                                                         \n"
                    );
                // 계약조회인지 개별광고조회인지
                if (Type.Equals("I"))
                {
                    sbQuery.Append("   AND A.ItemNo       = @ItemNo \n");
                }
                else
                {
                    sbQuery.Append("   AND A.ContractSeq  = @ContractSeq \n");
                }
                sbQuery.Append(""
                    + "   AND A.SummaryType  = 3   -- 3:연령별                                                                                 \n"
                    + " GROUP BY B.SummaryCode, B.SummaryName                                                                                  \n"
                    + "                                                                                                                        \n"
                    + "UNION                                                                                                                   \n"
                    + "                                                                                                                        \n"
                    + "-- 지역별 집계                                                                                                          \n"
                    + "SELECT  7 AS TypeCode                                                                                                   \n"
                    + "       ,'7 지역별' AS TypeName                                                                                                   \n"
                    + "       ,'지역별' AS SumType                                                                                             \n"
                    + "       ,TA.SummaryCode AS SumCode                                                                                       \n"
                    + "       ,TA.SummaryName AS SumName                                                                                       \n"
                    + "       ,ISNULL(SUM(TB.AdCnt),0) AS AdCnt                                                                                \n"
                    + "       ,ISNULL(CONVERT(DECIMAL(9,2),(SUM(TB.AdCnt) / CONVERT(float,@TotAdHit)) * 100),0) AS AdRate                      \n"
                    + "       ,null AS HsCnt                                                                                                   \n"
                    + "       ,null AS Reach                                                                                                   \n"
                    + "       ,null AS Freq                                                                                                    \n"
                    + "       ,null AS GRP                                                                                                     \n"
                    + "       ,MAX(TB.PgCnt) AS PgCnt                                                                                           \n"
                    + "       ,SUM(TB.HitCnt) AS HitCnt                                                                                         \n"
                    + "       ,CONVERT(DECIMAL(9,2),(SUM(TB.HitCnt) / CONVERT(float,@TotPgHit)) * 100) AS PgRate                                \n"
                    + "  FROM (SELECT SummaryCode, SummaryName FROM SummaryCode with(NoLock) WHERE SummaryType = 5 AND Level = 1) TA                        \n"
                    + "       LEFT JOIN                                                                                                        \n"
                    + "       (                                                                                                                \n"
                    + "        SELECT CASE B.Level WHEN 1 THEN B.SummaryCode                                                                   \n"
                    + "                            ELSE B.UpperCode END AS SummaryCode                                                         \n"
                    + "              ,A.AdCnt                                                                                                  \n"
                    + "              ,A.HitCnt                                                                                                  \n"
                    + "              ,A.PgCnt                                                                                                  \n"
                    + "              ,B.Level                                                                                                  \n"
                    + "          FROM SummaryAdMonthly0      A with(NoLock) \n"
                    + "               INNER JOIN SummaryCode B with(NoLock) ON (A.SummaryType = B.SummaryType AND A.SummaryCode = B.SummaryCode)           \n"
                    + "         WHERE A.LogDay       = @LogDay                                                                                 \n"
                    );
                // 계약조회인지 개별광고조회인지
                if (Type.Equals("I"))
                {
                    sbQuery.Append("           AND A.ItemNo       = @ItemNo \n");
                }
                else
                {
                    sbQuery.Append("           AND A.ContractSeq  = @ContractSeq \n");
                }
                sbQuery.Append(""
                    + "           AND A.SummaryType  = 5  -- 5:지역별                                                                          \n"
                    + "       ) TB                                                                                                             \n"
                    + "       ON (TA.SummaryCode = TB.SummaryCode)                                                                             \n"
                    + " GROUP BY TA.SummaryCode, TA.SummaryName                                                                                \n"
                    + "                                                                                                                        \n"
                    + "ORDER BY TypeCode, SumCode                                                                                              \n"
                    );


                // __DEBUG__
                _log.Debug(sbQuery.ToString());
                // __DEBUG__

                // 쿼리실행
                ds = new DataSet();
                _db.ExecuteQuery(ds, sbQuery.ToString());

                // 결과 DataSet의 데이터모델에 복사
                summaryAdModel.ReportDataSet = ds.Copy();

                // 결과
                summaryAdModel.ResultCnt = Utility.GetDatasetCount(summaryAdModel.ReportDataSet);

                ds.Dispose();

                // 결과코드 셋트
                summaryAdModel.ResultCD = "0000";

                // __DEBUG__
                _log.Debug("<출력정보>");
                _log.Debug("ResultCnt:[" + summaryAdModel.ResultCnt + "]");
                // __DEBUG__

                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "GetSummaryAdMonthly() End");
                _log.Debug("-----------------------------------------");
            }
            catch (Exception ex)
            {
                summaryAdModel.ResultCD = "3000";
                if (isNotReady)
                {
                    summaryAdModel.ResultDesc = "해당 기간은 데이터가 집계되지 않았습니다.";
                }
                else
                {
                    summaryAdModel.ResultDesc = "월간 광고집행 집계 조회중 오류가 발생하였습니다";
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

        #region 일별 하우스홀드 변경추이 보고서
        /// <summary>
        ///  일별 하우스홀드 변경추이 보고서
        /// </summary>
        /// <param name="dateAdTypeSummaryRptModel"></param>
        public void GetDateAccountHouseHold(HeaderModel header, SummaryAdModel summaryAdModel)
        {
            bool isNotReady = false; // 데이터가 집계가 되지않아 존재하지 않을때.
            try
            {
                StringBuilder sbQuery = null;

                // 데이터베이스를 OPEN한다
                _db.Open();


                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "GetDateAccountHouseHold() Start");
                _log.Debug("-----------------------------------------");

                // 일자가 6자리 이상(yyyymmdd)이면 6자리로 만든다.
                if (summaryAdModel.SearchStartDay.Length > 6) summaryAdModel.SearchStartDay = summaryAdModel.SearchStartDay.Substring(2, 6);
                if (summaryAdModel.SearchEndDay.Length > 6) summaryAdModel.SearchEndDay = summaryAdModel.SearchEndDay.Substring(2, 6);


                _log.Debug("<입력정보>");
                _log.Debug("CampaignCode	  :[" + summaryAdModel.CampaignCode + "]");		// 캠페인 코드
                _log.Debug("SearchContractSeq :[" + summaryAdModel.SearchContractSeq + "]");		// 검색 광고번호           
                _log.Debug("SearchItemNo      :[" + summaryAdModel.SearchItemNo + "]");		// 검색 광고번호           
                _log.Debug("SearchStartDay    :[" + summaryAdModel.SearchStartDay + "]");		// 검색 집계시작 일자          
                _log.Debug("SearchEndDay      :[" + summaryAdModel.SearchEndDay + "]");		// 검색 집계종료 일자          
                // __DEBUG__

                string ContractSeq = summaryAdModel.SearchContractSeq;
                string CampaignCode = summaryAdModel.CampaignCode;
                string ItemNo = summaryAdModel.SearchItemNo;
                string StartDay = summaryAdModel.SearchStartDay;
                string EndDay = summaryAdModel.SearchEndDay;

                // 쿼리생성
                sbQuery = new StringBuilder();
                sbQuery.Append("																							\n");
                sbQuery.Append("DECLARE @LOGSTARTDAY CHAR(6);																\n");
                sbQuery.Append("DECLARE @LOGENDDAY CHAR(6);																	\n");
                sbQuery.Append("SELECT @LOGSTARTDAY = '" + StartDay + "';													\n");
                sbQuery.Append("SELECT @LOGENDDAY = '" + EndDay + "';														\n");
                sbQuery.Append("																							\n");
                sbQuery.Append("WITH ItemList (ItemNo)																		\n");
                sbQuery.Append("AS (																						\n");
                sbQuery.Append("		SELECT	a.ItemNo																	\n");
                sbQuery.Append("		FROM	AdTargetsHanaTV.dbo.ContractItem a WITH(NOLOCK)													\n");
                if (CampaignCode != null && !CampaignCode.Equals("00"))
                {
                    sbQuery.Append("		INNER JOIN AdTargetsHanaTV.dbo.CampaignDetail b ON (a.ItemNo = b.ItemNo AND b.CampaignCode = " + CampaignCode + ")	\n");
                }
                sbQuery.Append("		WHERE ContractSeq = " + ContractSeq + "												\n");
                if (ItemNo != null && !ItemNo.Equals("00"))
                {
                    sbQuery.Append("		AND		a.ItemNo = " + ItemNo + "													\n");
                }
                sbQuery.Append("), RowData(LogDay, Week, Rnum, PRnum, Cnt)														\n");
                sbQuery.Append("AS (																						\n");
                sbQuery.Append("		SELECT																				\n");
                sbQuery.Append("				LogDay, Week																\n");
                sbQuery.Append("				, ROW_NUMBER() OVER(ORDER BY LogDay) Rnum									\n");
                sbQuery.Append("				, ROW_NUMBER() OVER(ORDER BY LogDay) -1 PRnum								\n");
                sbQuery.Append("				, (SELECT COUNT(*) FROM HouseHoldAdDaily V1 WITH(NOLOCK)					\n");
                sbQuery.Append("						WHERE LogDay >= @LOGSTARTDAY AND LogDay <= A.LogDay AND ItemNo IN (	\n");
                sbQuery.Append("						SELECT ItemNo FROM ItemList)) Cnt									\n");
                sbQuery.Append("		FROM SummaryBase A WITH(NOLOCK)														\n");
                sbQuery.Append("		WHERE																				\n");
                sbQuery.Append("				LogDay >= @LOGSTARTDAY														\n");
                sbQuery.Append("				AND LogDay <= @LOGENDDAY													\n");
                sbQuery.Append(")																							\n");
                sbQuery.Append("SELECT																						\n");
                sbQuery.Append("		CONVERT(CHAR(10), CONVERT(datetime, '20' + A.LogDay, 112),120) +					\n");
                sbQuery.Append("				CASE A.week WHEN 1 THEN ' (일)'												\n");
                sbQuery.Append("						 WHEN 2 THEN ' (월)'												\n");
                sbQuery.Append("						 WHEN 3 THEN ' (화)'												\n");
                sbQuery.Append("						 WHEN 4 THEN ' (수)'												\n");
                sbQuery.Append("						 WHEN 5 THEN ' (목)'												\n");
                sbQuery.Append("						 WHEN 6 THEN ' (금)'												\n");
                sbQuery.Append("						 WHEN 7 THEN ' (토)'												\n");
                sbQuery.Append("				 ELSE '' END AS LogDay														\n");
                sbQuery.Append("		, A.Cnt, (A.Cnt - ISNULL(B.Cnt, 0)) Inc, A.Week										\n");
                sbQuery.Append("FROM RowData A																				\n");
                sbQuery.Append("		LEFT OUTER JOIN RowData B ON (B.Rnum = A.PRnum)										\n");
                sbQuery.Append("ORDER BY A.LogDay																			\n");
                // __DEBUG__
                _log.Debug(sbQuery.ToString());
                // __DEBUG__

                // 쿼리실행
                DataSet ds = new DataSet();
                _db.ExecuteQuery(ds, sbQuery.ToString());

                // 결과 DataSet의 데이터모델에 복사
                summaryAdModel.ReportDataSet = ds.Copy();

                // 결과
                summaryAdModel.ResultCnt = Utility.GetDatasetCount(summaryAdModel.ReportDataSet);

                ds.Dispose();

                // 결과코드 셋트
                summaryAdModel.ResultCD = "0000";

                // __DEBUG__
                _log.Debug("<출력정보>");
                _log.Debug("ResultCnt:[" + summaryAdModel.ResultCnt + "]");
                // __DEBUG__

                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "GetDateAccountHouseHold() End");
                _log.Debug("-----------------------------------------");
            }
            catch (Exception ex)
            {
                summaryAdModel.ResultCD = "3000";
                if (isNotReady)
                {
                    summaryAdModel.ResultDesc = "해당 기간은 데이터가 집계되지 않았습니다.";
                }
                else
                {
                    summaryAdModel.ResultDesc = "일별 하우스홀드 변경추이 조회중 오류가 발생하였습니다";
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