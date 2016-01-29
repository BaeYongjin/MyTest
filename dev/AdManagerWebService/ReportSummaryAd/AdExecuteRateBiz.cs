// ===============================================================================
//
// AdExecuteRateBiz.cs
//
// 인벤토리예측
//
// ===============================================================================
// Release history
// 2010.02.26 BH.YOON
// ===============================================================================
// Copyright (C) 2007 G-Inno Systems Inc.
// All rights reserved.
// ==============================================================================

/*
 * -------------------------------------------------------
 * Class Name: AdExecuteRateBiz
 * 주요기능  :  처리 로직
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
using System.Collections;

using WinFramework.Data;
using WinFramework.Misc;
using WinFramework.Base;

using AdManagerModel;

namespace AdManagerWebService.ReportSummaryAd
{
	/// <summary>
	/// InventoryPresentConditionBiz에 대한 요약 설명입니다.
	/// </summary>
	public class AdExecuteRateBiz : BaseBiz
	{

		#region  생성자
		public AdExecuteRateBiz() : base(FrameSystem.connSummaryDbString)
		{
			_log = FrameSystem.oLog;
		}
		#endregion

		#region 레포팅 생성
		/// <summary>
		///  인벤토리현황 집계
		/// </summary>
		/// <param name="AdExecuteRateModel"></param>
		public void GetAdvertiseExecuteRate(HeaderModel header, AdExecuteRateModel adExecuteRateModel)
		{
			bool isNotReady = false; // 데이터가 집계가 되지않아 존재하지 않을때.
			try
			{
				StringBuilder sbQuery = null;

				// 데이터베이스를 OPEN한다
				_db.Open();


				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetAdvertiseExecuteRate() Start");
				_log.Debug("-----------------------------------------");
				
				// __DEBUG__
				_log.Debug("<입력정보>");
				_log.Debug("StartDay	  :[" + adExecuteRateModel.StartDay   + "]");		// 검색시작일자
				_log.Debug("EndDay	  :[" + adExecuteRateModel.EndDay   + "]");		// 검색종료일자
				// __DEBUG__

				string StartDay   = adExecuteRateModel.StartDay;
				string EndDay   = adExecuteRateModel.EndDay;
				
				// 쿼리실행
				sbQuery = new StringBuilder();
				sbQuery.Append("	DECLARE @StartDay CHAR(8);																										\n");
				sbQuery.Append("	DECLARE @EndDay CHAR(8);																										\n");
				sbQuery.Append("	SELECT @StartDay = '"+ StartDay +"', @EndDay = '"+ EndDay +"';																	\n");
				sbQuery.Append("	 	SELECT                    CONTSEQ																							\n");
				sbQuery.Append("	 				, ADVERNAME																										\n");
				sbQuery.Append("	 				, ITEMNAME																										\n");
				sbQuery.Append("	 				, BGNDAY																										\n");
				sbQuery.Append("	 				, ENDDAY																										\n");
				sbQuery.Append("	 				, CONTAMT       -- AS [계약노출수]																				\n");
				sbQuery.Append("	 				, TODAYCNT	-- AS [당일노출수]																					\n");
				sbQuery.Append("	 				, TOTCNT	-- AS [집행노출수]																					\n");
				sbQuery.Append("	 				,CASE CAST(CONTAMT AS REAL) WHEN 0 THEN 0 ELSE TOTCNT / CAST(CONTAMT AS REAL) * 100 END AS EXECUTE_RATE  	-- AS [집행진척율]	\n");
				sbQuery.Append("	 	            ,CASE (DATEDIFF( DAY, CAST( BGNDAY AS DATETIME), CAST( ENDDAY AS DATETIME)) + 1) * 100 WHEN 0 THEN 0 ELSE CAST( (DATEDIFF( DAY, CAST( BGNDAY AS DATETIME), CAST( @EndDay AS DATETIME)) + 1) AS REAL) / (DATEDIFF( DAY, CAST( BGNDAY AS DATETIME), CAST( ENDDAY AS DATETIME)) + 1) * 100 END NORMAL_EXECUTE_RATE  -- AS [정상진척율]	\n");
				sbQuery.Append("	               ,(CASE CAST(CONTAMT AS REAL) * 100 WHEN 0 THEN 0 ELSE  TOTCNT / CAST(CONTAMT AS REAL) * 100 END  ) - (CASE (DATEDIFF( DAY, CAST( BGNDAY AS DATETIME), CAST( ENDDAY AS DATETIME)) + 1) * 100 WHEN 0 THEN 0 ELSE CAST( (DATEDIFF( DAY, CAST( BGNDAY AS DATETIME), CAST( @EndDay AS DATETIME)) + 1) AS REAL) / (DATEDIFF( DAY, CAST( BGNDAY AS DATETIME), CAST( ENDDAY AS DATETIME)) + 1) * 100 END ) EXECUTE_RATE_COMPARE -- 진척률차이 	\n");
				sbQuery.Append("	 				, AMTRATE -- 노출물량제어비율																					\n");
				sbQuery.Append("	 	FROM (																														\n");
				sbQuery.Append("	 					SELECT   CASE WHEN GROUPING(V.CONTSEQ) = 1 THEN '9999' ELSE V.CONTSEQ END AS CONTSEQ						\n");
				sbQuery.Append("	 									,CASE WHEN GROUPING(V.CONTSEQ) = 1 THEN MAX('합계') ELSE  MAX(V.ADVERNAME) END AS ADVERNAME	\n");
				sbQuery.Append("	 									,CASE WHEN GROUPING(V.ITEMNAME) = 1 THEN '소계' ELSE V.ITEMNAME END AS ITEMNAME				\n");
				sbQuery.Append("	 									,MIN(V.BGNDAY) AS BGNDAY																	\n");
				sbQuery.Append("	 									,MAX(V.ENDDAY) AS ENDDAY																	\n");
				sbQuery.Append("	 									,SUM(V.CONTRACTAMT) AS CONTAMT													**			\n");
				sbQuery.Append("	 									,SUM( ISNULL(V.ACCUCNT,0) ) AS ACCUCNT														\n");
				sbQuery.Append("	 									,SUM( ISNULL(V.TODAYCNT,0) ) AS TODAYCNT													\n");
				sbQuery.Append("	 									,SUM( ISNULL(V.ACCUCNT,0) + ISNULL(V.TODAYCNT,0) )	AS TOTCNT								\n");
				sbQuery.Append("	 									,MAX( V.AMTRATE ) AS AMTRATE																\n");
				sbQuery.Append("	 					FROM	(																									\n");
                sbQuery.Append("	 									SELECT ( SELECT ADVERTISERNAME FROM AdTargetsHanaTV.dbo.ADVERTISER WITH (NOLOCK) WHERE ADVERTISERCODE = A.ADVERTISERCODE)	AS ADVERNAME	\n");
				sbQuery.Append("	 												,A.CONTRACTSEQ	AS CONTSEQ														\n");
				sbQuery.Append("	 												,A.ITEMNAME AS ITEMNAME															\n");
				sbQuery.Append("	 												,A.EXCUTESTARTDAY	AS BGNDAY													\n");
				sbQuery.Append("	 												,A.EXCUTEENDDAY		AS ENDDAY													\n");
				sbQuery.Append("	 												,C.CONTRACTAMT																	\n");
				sbQuery.Append("	 												,( SELECT TOP 1 ADCNTACCU + ADCNT												\n");
				sbQuery.Append("	 													 FROM	SUMMARYADDAILY0 WITH (NOLOCK)										\n");
				sbQuery.Append("	 													 WHERE	ITEMNO = A.ITEMNO													\n");
				sbQuery.Append("	 													 AND		SUMMARYTYPE = 1													\n");
				sbQuery.Append("	 													 AND		LOGDAY <= SUBSTRING(@StartDay, 3, 6)							\n");
				sbQuery.Append("	 													 ORDER BY LOGDAY DESC ) AS ACCUCNT											\n");
				sbQuery.Append("	 												,(SELECT SUM(HITSUM)															\n");
				sbQuery.Append("	 													FROM SUMMARYADGENRE WITH (NOLOCK)											\n");
				sbQuery.Append("	 													WHERE	LOGDAY = SUBSTRING(@EndDay, 3, 6)									\n");
				sbQuery.Append("	 													AND		ITEMNO	= A.ITEMNO													\n");
				sbQuery.Append("	 													GROUP BY ITEMNO) AS TODAYCNT												\n");
				sbQuery.Append("	 												,C.AMTCONTROLRATE		AS AMTRATE												\n");
                sbQuery.Append("	 									FROM	AdTargetsHanaTV.dbo.CONTRACTITEM A WITH(NOLOCK)															\n");
                sbQuery.Append("	 									INNER JOIN	AdTargetsHanaTV.dbo.CONTRACT B WITH(NOLOCK)	ON A.CONTRACTSEQ = B.CONTRACTSEQ						\n");
                sbQuery.Append("	 									INNER JOIN	AdTargetsHanaTV.dbo.TARGETING C WITH(NOLOCK)	ON A.ITEMNO = C.ITEMNO								\n");
				sbQuery.Append("	 									WHERE	A.RAPCODE = 3																		\n");
				sbQuery.Append("	 									AND		A.EXCUTESTARTDAY <= @EndDay															\n");
				sbQuery.Append("	 									AND		A.EXCUTEENDDAY >= @EndDay ) V														\n");
				sbQuery.Append("	 					GROUP BY V.CONTSEQ,V.ITEMNAME WITH ROLLUP ) V2																\n");


				// __DEBUG__
				_log.Debug(sbQuery.ToString());
				// __DEBUG__

				// 쿼리실행
				DataSet ds = new DataSet();
				_db.ExecuteQuery(ds,sbQuery.ToString());

				// 결과 DataSet의 데이터모델에 복사
				adExecuteRateModel.ReportDataSet = ds.Copy();

				// 결과
				adExecuteRateModel.ResultCnt = Utility.GetDatasetCount(adExecuteRateModel.ReportDataSet);

				ds.Dispose();

				// 결과코드 셋트
				adExecuteRateModel.ResultCD = "0000";

				// __DEBUG__
				_log.Debug("<출력정보>");
				_log.Debug("ResultCnt:[" + adExecuteRateModel.ResultCnt + "]");
				// __DEBUG__

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetAdvertiseExecuteRate() End");
				_log.Debug("-----------------------------------------");
			}
			catch(Exception ex)
			{
				adExecuteRateModel.ResultCD = "3000";
				if(isNotReady)
				{
					adExecuteRateModel.ResultDesc = "해당 기간은 데이터가 집계되지 않았습니다.";
				}
				else
				{
					adExecuteRateModel.ResultDesc = "광고집행율 조회중 오류가 발생하였습니다";
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

		#region 7일간 변동된 EPG 목록
		/// <summary>
		///  7일간 변동된 EPG 목록 - CUG코드는 없는 것을 찾는다.
		/// </summary>
		/// <param name="AdExecuteRateModel"></param>
		public void GetChangeEPGList(HeaderModel header, AdExecuteRateModel adExecuteRateModel)
		{
			bool isNotReady = false; // 데이터가 집계가 되지않아 존재하지 않을때.
			try
			{
				StringBuilder sbQuery = null;

				// 데이터베이스를 OPEN한다
				_db.Open();


				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetAdvertiseExecuteRate() Start");
				_log.Debug("-----------------------------------------");
				
				// __DEBUG__
				_log.Debug("<입력정보>");

				// __DEBUG__

				string StartDay   = adExecuteRateModel.StartDay;
				string EndDay   = adExecuteRateModel.EndDay;
				
				// 쿼리실행
				sbQuery = new StringBuilder();
				sbQuery.Append("DECLARE @DURATION INT;	\n");
				sbQuery.Append("SELECT @DURATION = 7;	\n");
				sbQuery.Append("SELECT	\n");
				sbQuery.Append("	1 Gubn, B.*	\n");
				sbQuery.Append("FROM (	\n");
				sbQuery.Append("	SELECT	\n");
				sbQuery.Append("		DISTINCT ServiceCode, MenuCode	\n");
                sbQuery.Append("	FROM AdTargetsHanaTV.dbo.MenuCode WITH (NOLOCK)	\n");
				sbQuery.Append("	WHERE	\n");
				sbQuery.Append("		IsNow = 'N'	\n");
				sbQuery.Append("		AND RegDate BETWEEN DATEADD(DD, -1*@DURATION, GETDATE()) AND GETDATE()	\n");
				sbQuery.Append("		AND ServiceCode = 0	\n");
				sbQuery.Append(") A	\n");
                sbQuery.Append("JOIN AdTargetsHanaTV.dbo.MenuCode B  WITH (NOLOCK) ON IsNow = 'Y' AND B.ServiceCode = A.ServiceCode AND B.MenuCode = A.MenuCode	\n");
				sbQuery.Append("UNION ALL	\n");
				sbQuery.Append("--7일동안 추가된 메뉴	\n");
				sbQuery.Append("SELECT	\n");
				sbQuery.Append("	2 Gubn, A.*	\n");
                sbQuery.Append("FROM AdTargetsHanaTV.dbo.MenuCode A WITH (NOLOCK)	\n");
				sbQuery.Append("WHERE	\n");
                sbQuery.Append("	NOT EXISTS (SELECT 1 FROM AdTargetsHanaTV.dbo.MenuCode WITH (NOLOCK)	\n");
				sbQuery.Append("		WHERE IsNow = 'N' AND ServiceCode = A.ServiceCode AND MenuCode = A.MenuCode)	\n");
				sbQuery.Append("	AND A.RegDate BETWEEN DATEADD(DD, -1*@DURATION, GETDATE()) AND GETDATE()	\n");
				sbQuery.Append("	AND ServiceCode = 0	\n");
				sbQuery.Append("--7일동안 삭제된 메뉴	\n");
				sbQuery.Append("UNION ALL	\n");
				sbQuery.Append("SELECT	\n");
				sbQuery.Append("	3 Gubn, A.*	\n");
                sbQuery.Append("FROM AdTargetsHanaTV.dbo.MenuCode A WITH (NOLOCK)	\n");
				sbQuery.Append("WHERE	\n");
				sbQuery.Append("	A.RegDate BETWEEN DATEADD(DD, -1*@DURATION, GETDATE()) AND GETDATE()	\n");
				sbQuery.Append("	AND IsNow = 'N'	\n");
				sbQuery.Append("	AND ServiceCode = 0	\n");
                sbQuery.Append("	AND NOT EXISTS (SELECT 1 FROM AdTargetsHanaTV.dbo.MenuCode WITH (NOLOCK)	\n");
				sbQuery.Append("		WHERE IsNow = 'Y' AND ServiceCode = A.ServiceCode AND MenuCode = A.MenuCode)	\n");

				// __DEBUG__
				_log.Debug(sbQuery.ToString());
				// __DEBUG__

				// 쿼리실행
				DataSet ds = new DataSet();
				_db.ExecuteQuery(ds,sbQuery.ToString());

				// 결과 DataSet의 데이터모델에 복사
				adExecuteRateModel.ReportDataSet = ds.Copy();

				// 결과
				adExecuteRateModel.ResultCnt = Utility.GetDatasetCount(adExecuteRateModel.ReportDataSet);

				ds.Dispose();

				// 결과코드 셋트
				adExecuteRateModel.ResultCD = "0000";

				// __DEBUG__
				_log.Debug("<출력정보>");
				_log.Debug("ResultCnt:[" + adExecuteRateModel.ResultCnt + "]");
				// __DEBUG__

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetAdvertiseExecuteRate() End");
				_log.Debug("-----------------------------------------");
			}
			catch(Exception ex)
			{
				adExecuteRateModel.ResultCD = "3000";
				if(isNotReady)
				{
					adExecuteRateModel.ResultDesc = "해당 기간은 데이터가 집계되지 않았습니다.";
				}
				else
				{
					adExecuteRateModel.ResultDesc = "광고집행율 조회중 오류가 발생하였습니다";
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