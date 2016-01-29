// ===============================================================================
//
// InventoryPresentConditionBiz.cs
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
 * Class Name: InventoryPresentConditionBiz
 * 주요기능  : 인벤토리예측 처리 로직
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
	public class InventoryPresentConditionBiz : BaseBiz
	{

		#region  생성자

		public InventoryPresentConditionBiz() : base(FrameSystem.connSummaryDbString)
		{
			_log = FrameSystem.oLog;
		}
		#endregion

		#region 레포팅 생성
		/// <summary>
		///  인벤토리현황 집계
		/// </summary>
		/// <param name="InventoryPresentConditionModel"></param>
		public void GetInventoryPresentCondition(HeaderModel header, InventoryPresentConditionModel inventoryPresentConditionModel)
		{
			bool isNotReady = false; // 데이터가 집계가 되지않아 존재하지 않을때.
			try
			{
				StringBuilder sbQuery = null;

				// 데이터베이스를 OPEN한다
				_db.Open();


				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetInventoryPresentCondition() Start");
				_log.Debug("-----------------------------------------");

				// 일자가 6자리 이상(yyyymmdd)이면 6자리로 만든다.
				if(inventoryPresentConditionModel.LogDay1.Length > 6) inventoryPresentConditionModel.LogDay1 = inventoryPresentConditionModel.LogDay1.Substring(2,6);
				if(inventoryPresentConditionModel.LogDay2.Length > 6) inventoryPresentConditionModel.LogDay2 = inventoryPresentConditionModel.LogDay2.Substring(2,6);			
				
				// __DEBUG__
				_log.Debug("<입력정보>");
				_log.Debug("LogDay1	  :[" + inventoryPresentConditionModel.LogDay1   + "]");		// 검색 매체
				_log.Debug("LogDay2	  :[" + inventoryPresentConditionModel.LogDay2   + "]");					
				_log.Debug("SearchType:[" + inventoryPresentConditionModel.SearchType   + "]");					
				// __DEBUG__

				string LogDay1   = inventoryPresentConditionModel.LogDay1;
				string LogDay2   = inventoryPresentConditionModel.LogDay2;
				int SearchType = inventoryPresentConditionModel.SearchType;
				
				SqlParameter[] sqlParams = new SqlParameter[1];
				sqlParams[0] = new SqlParameter("@LogDay1", SqlDbType.VarChar, 6);
				sqlParams[0].Value = inventoryPresentConditionModel.LogDay1;

				// 나오지 말아야 하는 카테고리 홈-2500, 쇼핑-2400, 기타-9999
				string ExceptionCategory = "9999";

				#region [ 조회대상 카테고리 쿼리 ]				
				// 쿼리생성
				// 조회대상 카테고리를 가져온다
				sbQuery = new StringBuilder();
				sbQuery.Append("	\n");
				sbQuery.Append(" SELECT	CategoryCode, CategoryName					\n");
                sbQuery.Append(" FROM	AdTargetsHanaTV.dbo.Category WITH (NOLOCK)						\n");
				sbQuery.Append(" WHERE	InventoryYn = 'Y'							\n");
				//sbQuery.Append(" AND	CategoryCode NOT IN ("+ ExceptionCategory +")	\n");
				sbQuery.Append(" ORDER BY SortNo										\n");

				_log.Debug(sbQuery.ToString());
				// 쿼리실행
				DataSet ds = new DataSet();
				_db.ExecuteQuery(ds,sbQuery.ToString());
				#endregion

				// 실제데이터 쿼리
				sbQuery.Length = 0;

				sbQuery.Append("	\n");
				sbQuery.Append("DECLARE @StartDay CHAR(6);	\n");
				sbQuery.Append("DECLARE @EndDay CHAR(6);	\n");
				sbQuery.Append("DECLARE @SearchType TINYINT;	\n");
				sbQuery.Append("DECLARE @Duration TINYINT;	\n");
				sbQuery.Append("DECLARE @CalcEndDate datetime;	\n");
				sbQuery.Append("	\n");
				sbQuery.Append("SELECT	\n");
				sbQuery.Append("	@CalcEndDate = MIN(Date)	\n");
				sbQuery.Append("FROM (	\n");
				sbQuery.Append("	SELECT CONVERT(DATETIME, @StartDay, 12) Date	\n");
				sbQuery.Append("	UNION ALL	\n");
				sbQuery.Append("	SELECT GETDATE()	\n");
				sbQuery.Append(") A	\n");
				sbQuery.Append("	\n");
				sbQuery.Append("SELECT	\n");
				sbQuery.Append("	@StartDay = '"+ LogDay1 +"'	\n");
				sbQuery.Append("	, @EndDay = '"+ LogDay2 +"'	\n");
				sbQuery.Append("	, @Duration = CONVERT(INT, CONVERT(DATETIME, @EndDay, 12) - CONVERT(DATETIME, @StartDay, 12)) + 1	\n");
				sbQuery.Append("	, @SearchType = "+ SearchType +";	\n");
				sbQuery.Append("WITH ItemData (ItemNo, ItemName, ExcuteStartDay, RealEndDay, RemaindCnt, AdState, FileState, ContractAmt, RemaindAmt)		\n");
				sbQuery.Append("AS (	\n");
				sbQuery.Append("	SELECT	\n");
                sbQuery.Append("		A.ItemNo, A.ItemNo + '-' + A.ItemName, A.ExcuteStartDay, A.RealEndDay, AdTargetsHanaTV.dbo.ufnRemaindAdCnt(A.ItemNo, @StartDay, @EndDay)	\n");
				sbQuery.Append("		, AdState, A.FileState, ContractAmt	\n");
                sbQuery.Append("		, AdTargetsHanaTV.dbo.ufnImpressionAccu(A.ItemNo, CONVERT(CHAR(6), @CalcEndDate-1, 12))	as RemaindAmt \n");
//				sbQuery.Append("		, (CASE WHEN ContractAmt IS NULL THEN 0 ELSE ContractAmt -	\n");
//				sbQuery.Append("			ISNULL((SELECT AdCntAccu + AdCnt FROM SummaryAdDaily0 B WITH (NOLOCK)	\n");
//				sbQuery.Append("				WHERE ItemNo = A.ItemNo AND SummaryType = 1 AND LogDay =	\n");
//				sbQuery.Append("					(SELECT MAX(LogDay) FROM SummaryAdDaily0	\n");
//				sbQuery.Append("						WHERE ItemNo = B.ItemNo AND SummaryType = 1	\n");
//				sbQuery.Append("						AND LogDay <= CONVERT(CHAR(6), @CalcEndDate-1, 12))	\n");
//				sbQuery.Append("				), 0)	\n");
//				sbQuery.Append("			END) RemaindAmt	\n");
                sbQuery.Append("	FROM AdTargetsHanaTV.dbo.ContractItem A WITH (NOLOCK)	\n");
                sbQuery.Append("	JOIN AdTargetsHanaTV.dbo.Targeting B WITH (NOLOCK) ON B.ItemNo = A.ItemNo		\n");
				sbQuery.Append("	WHERE A.ExcuteStartDay <= '20'+ @EndDay	\n");
				sbQuery.Append("		AND A.RealEndDay >= '20'+ @StartDay	\n");
				sbQuery.Append("		AND A.AdType = 10	\n");
				sbQuery.Append(")	\n");
				sbQuery.Append("	\n");

				sbQuery.Append("SELECT	\n");
				sbQuery.Append("	-1 Gubun	\n");
				sbQuery.Append("	, -1 ItemNo	\n");
				sbQuery.Append("	, 0 Rnum	\n");
				sbQuery.Append("	, '광고명' ItemName	\n");
				sbQuery.Append("	, '' StartDate	\n");
				sbQuery.Append("	, '' EndDAte	\n");
				sbQuery.Append("	, '' ContractAmt		\n");
				sbQuery.Append("	, '' RemaindAmt		\n");
				sbQuery.Append("	, '' AdState	\n");
				sbQuery.Append("	, '' FileState	\n");
				sbQuery.Append("	, '합계' RowTotal	\n");
				for(int i=0; i<ds.Tables[0].Rows.Count; i++)
				{
					sbQuery.Append(", '"+ ds.Tables[0].Rows[i][1] +"' c"+ i);
				}
				#region [ 예상 인벤토리 ] 
				sbQuery.Append("	\n");
				sbQuery.Append("UNION ALL	\n");
				sbQuery.Append("SELECT	\n");
				sbQuery.Append("	Gubun	\n");
				sbQuery.Append("	, -1 ItemNo	\n");
				sbQuery.Append("	, 0 Rnum	\n");
				sbQuery.Append("	, '예상인벤토리' ItemName	\n");
				sbQuery.Append("	, '' StartDate	\n");
				sbQuery.Append("	, '' EndDAte	\n");
				sbQuery.Append("	, '' ContractAmt		\n");
				sbQuery.Append("	, '' RemaindAmt		\n");
				sbQuery.Append("	, '' AdState	\n");
				sbQuery.Append("	, '' FileState	\n");
				sbQuery.Append("	, REPLACE(CONVERT(VARCHAR(20), CONVERT(MONEY, CONVERT(INT, SUM(DayPerHitCnt) * @Duration)), 1), '.00', '')	\n");
				for(int i=0; i<ds.Tables[0].Rows.Count; i++)
				{
					sbQuery.Append("	, REPLACE(CONVERT(VARCHAR(20), CONVERT(MONEY, CONVERT(INT, MAX(CASE WHEN CategoryCode = "+ ds.Tables[0].Rows[i][0] +" THEN DayPerHitCnt ELSE 0 END) * @Duration)), 1), '.00', '') c"+ i +"	\n");
				}
				sbQuery.Append("FROM (	\n");
				sbQuery.Append("	SELECT	\n");
				sbQuery.Append("		0 Gubun	\n");
				sbQuery.Append("		, CategoryCode	\n");
				sbQuery.Append("		, SUM(DayPerHitCnt) DayPerHitCnt	\n");
				sbQuery.Append("	FROM (	\n");
				sbQuery.Append("		SELECT	\n");
				sbQuery.Append("			B.UpperMenuCode CategoryCode	\n");
				sbQuery.Append("			, A.Genre	\n");
				sbQuery.Append("			, MAX(DayPerHitCnt) DayPerHitCnt	\n");
                sbQuery.Append("		FROM AdTargetsHanaTV.dbo.InventorySummury A	WITH (NOLOCK)	\n");
                sbQuery.Append("		JOIN AdTargetsHanaTV.dbo.Menu B WITH (NOLOCK) ON B.MenuCode = A.Genre AND MenuCode NOT IN (" + ExceptionCategory + ")	\n");
				sbQuery.Append("		WHERE Gubun = "+ SearchType +"	\n");
				sbQuery.Append("		GROUP BY B.UpperMenuCode, A.Genre	\n");
				sbQuery.Append("	) C	\n");
				sbQuery.Append("	GROUP BY CategoryCode	\n");
				sbQuery.Append(") D	\n");
				sbQuery.Append("GROUP BY Gubun	\n");
				#endregion

				sbQuery.Append("UNION ALL	\n");

				sbQuery.Append("SELECT	\n");
				sbQuery.Append("	  CASE WHEN RowNum = 1 THEN 1 ELSE 3 END Gubun	\n");
				sbQuery.Append("	, CASE WHEN RowNum = 1 THEN -1 ELSE ItemNo END ItemNo	\n");
				sbQuery.Append("	, ROW_NUMBER() OVER(ORDER BY RowNum) - 1 Rnum	\n");
				sbQuery.Append("	, CASE WHEN RowNum = 1 THEN '필요인벤토리' ELSE ItemName END ItemName	\n");
				sbQuery.Append("	, CASE WHEN RowNum = 1 THEN '' ELSE ExcuteStartDay END StartDate		\n");
				sbQuery.Append("	, CASE WHEN RowNum = 1 THEN '' ELSE RealEndDay END EndDate		\n");
				sbQuery.Append("	, CAST(SUM(ContractAmt)	AS INT)	AS	ContractAmt		\n");
				sbQuery.Append("	, CAST(SUM(RemaindAmt)	AS INT)	AS	RemaindAmt		\n");
				sbQuery.Append("	, CASE WHEN RowNum = 1 THEN '' ELSE AdState END AdState	\n");
				sbQuery.Append("	, CASE WHEN RowNum = 1 THEN '' ELSE FileState END FileState	\n");
				sbQuery.Append("	, REPLACE(CONVERT(VARCHAR(20), CONVERT(MONEY, CONVERT(INT, SUM(RowTotal))), 1), '.00', '') RowTotal	\n");
				for(int i=0; i<ds.Tables[0].Rows.Count; i++)
				{
					sbQuery.Append("	, REPLACE(CONVERT(VARCHAR(20), CONVERT(MONEY, CONVERT(INT, SUM(C"+ i +"))), 1), '.00', '') C"+ i +"	\n");
				}
				sbQuery.Append("FROM (	\n");
				sbQuery.Append("	SELECT	\n");
				sbQuery.Append("		E.ItemNo	\n");
				sbQuery.Append("		, E.ItemName	\n");
				sbQuery.Append("		, CONVERT(VARCHAR(10), CONVERT(DATETIME, F.ExcuteStartDay), 120) ExcuteStartDay		\n");
				sbQuery.Append("		, CONVERT(VARCHAR(10), CONVERT(DATETIME, F.RealEndDay), 120) RealEndDay		\n");
				sbQuery.Append("		, MAX(F.ContractAmt) ContractAmt	\n");
				sbQuery.Append("		, MAX(F.RemaindAmt) RemaindAmt	\n");
				sbQuery.Append("		, F.AdState	\n");
				sbQuery.Append("		, F.FileState	\n");
				sbQuery.Append("		, SUM(GenreRate / 100 * EntryRate * F.RemaindCnt) RowTotal	\n");
				for(int i=0; i<ds.Tables[0].Rows.Count; i++)
				{
					sbQuery.Append("		, SUM(CASE CategoryCode WHEN "+ ds.Tables[0].Rows[i][0] +" THEN GenreRate / 100 * EntryRate * F.RemaindCnt ELSE 0 END) C"+ i +"	\n");
				}
				sbQuery.Append("	FROM (	\n");
				sbQuery.Append("		--편성비율이 있는 경우 설정	\n");
				sbQuery.Append("		SELECT	\n");
				sbQuery.Append("			A.ItemNo	\n");
				sbQuery.Append("			, B.CategoryCode	\n");
				sbQuery.Append("			, B.GenreCode	\n");
				sbQuery.Append("			, B.EntrySeq	\n");
				sbQuery.Append("			, A.ItemName	\n");
				sbQuery.Append("			, C.EntryRate	\n");
				sbQuery.Append("			, CONVERT(DECIMAL(7, 4), D.DayPerHitCnt*1.00/SUM(D.DayPerHitCnt) OVER(PARTITION BY A.ItemNo, B.EntrySeq)) GenreRate	\n");
                sbQuery.Append("		FROM AdTargetsHanaTV.dbo.ContractItem A WITH (NOLOCK)	\n");
                sbQuery.Append("		INNER JOIN AdTargetsHanaTV.dbo.SchRateDetail B WITH (NOLOCK) ON B.ItemNo = A.ItemNo AND CategoryCode NOT IN (2500, 2400, 9999)	\n");
                sbQuery.Append("		INNER JOIN AdTargetsHanaTV.dbo.SchRate C WITH (NOLOCK) ON C.ItemNo = B.ItemNo AND C.MediaCode = B.MediaCode AND C.EntrySeq = B.EntrySeq	\n");
                sbQuery.Append("		INNER JOIN AdTargetsHanaTV.dbo.InventorySummury D WITH (NOLOCK) ON D.Genre = B.GenreCode AND Gubun = 1	\n");
				sbQuery.Append("		WHERE A.ExcuteStartDay <= '20'+ @EndDay	\n");
				sbQuery.Append("			AND A.RealEndDay >= '20'+ @StartDay	\n");
				sbQuery.Append("		GROUP BY A.ItemNo, B.CategoryCode, B.GenreCode, B.EntrySeq, A.ItemName, EntryRate, D.DayPerHitCnt	\n");
				sbQuery.Append("		UNION ALL	\n");
				sbQuery.Append("		--편성비율이 없고 장르 편성일 경우 설정	\n");
				sbQuery.Append("		--편성비율이 없는 경우는 해당하는 장르의 VOD 시청을 횟수 비율을 근거로 비율을 할당하도록 한다.	\n");
				sbQuery.Append("		SELECT	\n");
				sbQuery.Append("			A.ItemNo	\n");
				sbQuery.Append("			, C.UpperMenuCode CategoryCode	\n");
				sbQuery.Append("			, B.GenreCode	\n");
				sbQuery.Append("			, 0 EntrySeq	\n");
				sbQuery.Append("			, A.ItemName	\n");
				sbQuery.Append("			, 100 EntryRate	\n");
				sbQuery.Append("			, CONVERT(DECIMAL(7, 4), D.DayPerHitCnt*1.00/SUM(D.DayPerHitCnt) OVER(PARTITION BY A.ItemNo)) GenreRate	\n");
				sbQuery.Append("		FROM (	\n");
				sbQuery.Append("			SELECT	\n");
				sbQuery.Append("				A.ItemNo, A.ItemName	\n");
                sbQuery.Append("			FROM AdTargetsHanaTV.dbo.ContractItem A WITH (NOLOCK)	\n");
                sbQuery.Append("			WHERE NOT EXISTS (SELECT 1 FROM AdTargetsHanaTV.dbo.SchRateDetail WITH (NOLOCK) WHERE ItemNo = A.ItemNo)	\n");
				sbQuery.Append("				AND A.ExcuteStartDay <= '20'+ @EndDay	\n");
				sbQuery.Append("				AND A.RealEndDay >= '20'+ @StartDay	\n");
				sbQuery.Append("				AND A.AdType < 20	\n");
				sbQuery.Append("		) A	\n");
                sbQuery.Append("		JOIN AdTargetsHanaTV.dbo.SchChoiceMenuDetail B WITH (NOLOCK) ON B.ItemNo = A.ItemNo	\n");
                sbQuery.Append("		JOIN AdTargetsHanaTV.dbo.Menu C WITH (NOLOCK) ON C.MenuCode = B.GenreCode AND C.MenuCode NOT IN (2500, 2400, 9999)	\n");
                sbQuery.Append("		JOIN AdTargetsHanaTV.dbo.InventorySummury D WITH (NOLOCK) ON D.Genre = B.GenreCode AND Gubun = 1	\n");
				sbQuery.Append("		UNION ALL	\n");
				sbQuery.Append("		--채널별 구성인 데이터.	\n");
				sbQuery.Append("		--채널별 구성 데이터는 소속된 개수를 비교하여 구하도록 한다.	\n");
				sbQuery.Append("		SELECT	\n");
				sbQuery.Append("			ItemNo, CategoryCode, GenreCode	\n");
				sbQuery.Append("			, 0 EntrySeq, ItemName	\n");
				sbQuery.Append("			, 100 EntryRate, GenreRate	\n");
				sbQuery.Append("		FROM (	\n");
				sbQuery.Append("			SELECT	\n");
				sbQuery.Append("				ItemNo, ItemName, CategoryCode, GenreCode	\n");
                sbQuery.Append("				, AdTargetsHanaTV.dbo.ufnChannelSchRate(ItemNo, CategoryCode, GenreCode, @StartDay, @EndDay) GenreRate	\n");
				sbQuery.Append("			FROM (	\n");
				sbQuery.Append("				SELECT	\n");
				sbQuery.Append("					A.ItemNo, A.ItemName, C.CategoryCode, C.GenreCode, C.ChannelNo	\n");
                sbQuery.Append("				FROM AdTargetsHanaTV.dbo.ContractItem A WITH (NOLOCK)	\n");
                sbQuery.Append("				JOIN AdTargetsHanaTV.dbo.SchChoiceChannelDetail B WITH (NOLOCK) ON B.ItemNo = A.ItemNo	\n");
                sbQuery.Append("				JOIN AdTargetsHanaTV.dbo.ChannelSet C WITH (NOLOCK) ON C.ChannelNo = B.ChannelNo AND C.CategoryCode NOT IN (2500, 2400, 9999)	\n");
				sbQuery.Append("				WHERE	\n");
				sbQuery.Append("					A.ExcuteStartDay <= '20'+ @EndDay	\n");
				sbQuery.Append("					AND A.RealEndDay >= '20'+ @StartDay	\n");
				sbQuery.Append("					AND A.AdType in(10,13,14,15,16,17,18)	\n");
				sbQuery.Append("				GROUP BY A.ItemNo, A.ItemName, C.CategoryCode, C.GenreCode, C.ChannelNo	\n");
				sbQuery.Append("			)X	\n");
				sbQuery.Append("			GROUP BY ItemNo, ItemName, CategoryCode, GenreCode	\n");
				sbQuery.Append("		) A	\n");
				sbQuery.Append("		WHERE	\n");
				sbQuery.Append("			GenreRate > 0	\n");
				sbQuery.Append("	)E	\n");
				sbQuery.Append("	INNER JOIN ItemData F ON F.ItemNo = E.ItemNo	\n");
				sbQuery.Append("	GROUP BY E.ItemNo, E.ItemName, F.ExcuteStartDay, F.RealEndDay, F.RemaindCnt, F.AdState, F.FileState	\n");
				sbQuery.Append(") F	\n");
                sbQuery.Append("INNER JOIN AdTargetsHanaTV.dbo.COPY_T G WITH(NOLOCK) ON RowNum <= 2	\n");
				sbQuery.Append("GROUP BY RowNum, CASE WHEN RowNum = 1 THEN 0 ELSE 1 END	\n");
				sbQuery.Append("	, CASE WHEN RowNum = 1 THEN -1 ELSE ItemNo END	\n");
				sbQuery.Append("	, CASE WHEN RowNum = 1 THEN '필요인벤토리' ELSE ItemName END	\n");
				sbQuery.Append("	, CASE WHEN RowNum = 1 THEN '' ELSE ExcuteStartDay END		\n");
				sbQuery.Append("	, CASE WHEN RowNum = 1 THEN '' ELSE RealEndDay END		\n");
				sbQuery.Append("	, CASE WHEN RowNum = 1 THEN '' ELSE AdState END	\n");
				sbQuery.Append("	, CASE WHEN RowNum = 1 THEN '' ELSE FileState END	\n");
				sbQuery.Append("UNION ALL		\n");
				sbQuery.Append("SELECT		\n");
				sbQuery.Append("	2 Gubun		\n");
				sbQuery.Append("	, -1 ItemNo		\n");
				sbQuery.Append("	, 0 Rnum	\n");
				sbQuery.Append("	, '비율' ItemName		\n");
				sbQuery.Append("	, '' StartDate	\n");
				sbQuery.Append("	, '' EndDAte		\n");
				sbQuery.Append("	, '' ContractAmt		\n");
				sbQuery.Append("	, '' RemaindAmt		\n");
				sbQuery.Append("	, '' AdState	\n");
				sbQuery.Append("	, '' FileState	\n");
				sbQuery.Append("	, '' RowTotal		\n");
				for(int i=0; i<ds.Tables[0].Rows.Count; i++)
				{
					sbQuery.Append("		, '' C"+ i +"	\n");
				}
				sbQuery.Append("ORDER BY Gubun, Rnum	\n");

				// __DEBUG__
				_log.Debug(sbQuery.ToString());
				// __DEBUG__

				// 쿼리실행
				ds = new DataSet();
				_db.Timeout	= 300;
				_db.ExecuteQuery(ds,sbQuery.ToString());

				// 결과 DataSet의 데이터모델에 복사
				inventoryPresentConditionModel.ReportDataSet = ds.Copy();

				// 결과
				inventoryPresentConditionModel.ResultCnt = Utility.GetDatasetCount(inventoryPresentConditionModel.ReportDataSet);

				ds.Dispose();

				// 결과코드 셋트
				inventoryPresentConditionModel.ResultCD = "0000";

				// __DEBUG__
				_log.Debug("<출력정보>");
				_log.Debug("ResultCnt:[" + inventoryPresentConditionModel.ResultCnt + "]");
				// __DEBUG__

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetInventoryPresentCondition() End");
				_log.Debug("-----------------------------------------");
			}
			catch(Exception ex)
			{
				inventoryPresentConditionModel.ResultCD = "3000";
				if(isNotReady)
				{
					inventoryPresentConditionModel.ResultDesc = "해당 기간은 데이터가 집계되지 않았습니다.";
				}
				else
				{
					inventoryPresentConditionModel.ResultDesc = "인벤토리 현황 조회중 오류가 발생하였습니다";
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

		#region 기준데이터 생성
		/// <summary>
		///  기준데이터 생성
		/// </summary>
		/// <param name="InventoryPresentConditionModel"></param>
		public void SetInventorySummuryData(HeaderModel header, InventoryPresentConditionModel inventoryPresentConditionModel)
		{
			try
			{
				StringBuilder sbQuery = null;

				_db.Open();
				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "SetInventorySummuryData() Start");
				_log.Debug("-----------------------------------------");
				_log.Debug("<입력정보>");
				_log.Debug("SearchType:[" + inventoryPresentConditionModel.SearchType   + "]");					

				int SearchType = inventoryPresentConditionModel.SearchType;
				
				// 쿼리생성
				sbQuery = new StringBuilder();
				#region [ 3개월 평균 ]
				if(SearchType == 1)
				{
					/* CM 2슬롯의 경우 
					 * Category.InventoryYn='N'인 건들은 가상의 99999를 만들어서 통합한다
					 * */
                    sbQuery.Append("\n DELETE FROM AdTargetsHanaTV.dbo.InventorySummury WHERE Gubun = 1; ");

                    sbQuery.Append("\n INSERT INTO AdTargetsHanaTV.dbo.InventorySummury (Gubun, Category, Genre, DayPerHitCnt, ActiveDateCnt)");
					sbQuery.Append("\n SELECT 1	as Gubun");
					sbQuery.Append("\n 		, case when c.CategoryCode is null then 99999 else v1.category end	as	Category");
					sbQuery.Append("\n 		, case when c.CategoryCode is null then 99999 else v1.Genre		end	as	Genre");
					sbQuery.Append("\n 		, sum(v1.DayAvg * isnull(c.InventoryRate,1.0) )	as DayPerHitCnt");
					sbQuery.Append("\n 		, avg(v1.DayCnt)	as ActiveDateCnt");
					sbQuery.Append("\n FROM ( SELECT  Category");
					sbQuery.Append("\n 				, Genre");
					sbQuery.Append("\n 				, avg(HitCnt)	as DayAvg");
					sbQuery.Append("\n 				, count(*)		as DayCnt");
					sbQuery.Append("\n 		  FROM	SummaryPgDaily2 A WITH (NOLOCK)");
					sbQuery.Append("\n 		  WHERE	LogDay < CONVERT(CHAR(6), GETDATE(), 12)");
					sbQuery.Append("\n 		  AND	LogDay >= CONVERT(CHAR(6), DATEADD(DD, -90, GETDATE()), 12)");
					sbQuery.Append("\n 		  Group By Category, Genre ) V1");
                    sbQuery.Append("\n LEFT OUTER JOIN  AdTargetsHanaTV.dbo.Category C ON V1.CATEGORY = C.CATEGORYCODE AND C.InventoryYn = 'Y'");
					sbQuery.Append("\n GROUP By case when c.CategoryCode is null then 99999 else v1.category end");
					sbQuery.Append("\n 		  , case when c.CategoryCode is null then 99999 else v1.Genre		end ");
					sbQuery.Append("\n ORDER BY 1,2,3");

//					sbQuery.Append("SELECT																													\n");
//					sbQuery.Append("	1 Gubun, Category, Genre, CONVERT(DECIMAL(10, 2)																	\n");
//					sbQuery.Append("	, CONVERT(DECIMAL(10, 2), SUM(CONVERT(DECIMAL, DayPerHitCnt))/COUNT(DayPerHitCnt))) DayPerHitCnt					\n");
//					sbQuery.Append("	, COUNT(*) ActiveDateCnt																							\n");
//					sbQuery.Append("FROM (																													\n");
//					sbQuery.Append("	SELECT Category, Genre,																								\n");
//					sbQuery.Append("		(HitCnt) *																										\n");
//					sbQuery.Append("			--인벤토리는 프로그램 힛트 수 * CM채널 수 이므로 채널수에 따라 곱수를 만든다							.	\n");
//					sbQuery.Append("			--현재는 SlotAdTypeAssign 에 2중화 채널을 MenuLevel = 9 AND AssignSeq = 9 값으로 관리하고 있으나			\n");
//					sbQuery.Append("			--향후 변경이 된다면 이 부분도 같이 고쳐 주어야 할 것이다.													\n");
//					sbQuery.Append("			ISNULL((SELECT 2 FROM AdTargetsHanaTV.dbo.AdTargetsHanaTV.dbo.SlotAdTypeAssign WITH (NOLOCK)														\n");
//					sbQuery.Append("				WHERE MenuLevel = 9 AND AssignSeq = 9 AND A.Category = Menu1 AND (Menu2 = A.Genre OR Menu2 = 0)), 1)	\n");
//					sbQuery.Append("		AS DayPerHitCnt																									\n");
//					sbQuery.Append("	FROM SummaryPgDaily2 A WITH (NOLOCK)																				\n");
//					sbQuery.Append("	WHERE																												\n");
//					sbQuery.Append("		LogDay < CONVERT(CHAR(6), GETDATE(), 12)																		\n");
//					sbQuery.Append("		AND LogDay >= CONVERT(CHAR(6), DATEADD(DD, -90, GETDATE()), 12)													\n");
//					sbQuery.Append(") B																														\n");
//					sbQuery.Append("GROUP BY Category, Genre																								\n");
				}
				#endregion

				#region [ 1년 최저 ]
				else if(SearchType == 2)
				{
					sbQuery.Append("\n DECLARE @MaxMon CHAR(4);");
                    sbQuery.Append("\n DELETE FROM AdTargetsHanaTV.dbo.InventorySummury WHERE Gubun = 2;");
					// 현재월기준 가장 적은 Hit를 기록한 년월을 찾아온다.
                    sbQuery.Append("\n SELECT @MaxMon = AdTargetsHanaTV.dbo.ufnMinCntMonth();");

                    sbQuery.Append("\n INSERT INTO AdTargetsHanaTV.dbo.InventorySummury (Gubun, Category, Genre, DayPerHitCnt, ActiveDateCnt)");
					sbQuery.Append("\n SELECT 2	as Gubun");
					sbQuery.Append("\n		, case when c.CategoryCode is null then 99999 else v1.category end	as	Category");
					sbQuery.Append("\n 		, case when c.CategoryCode is null then 99999 else v1.Genre		end	as	Genre");
					sbQuery.Append("\n 		, sum(v1.DayAvg * isnull(c.InventoryRate,1.0) )	as DayPerHitCnt");
					sbQuery.Append("\n 		, avg(v1.DayCnt)	as ActiveDateCnt");
					sbQuery.Append("\n FROM (	SELECT	  Category");
					sbQuery.Append("\n 					, Genre");
					sbQuery.Append("\n 					, avg(HitCnt)	as DayAvg");
					sbQuery.Append("\n 					, count(*)		as DayCnt");
					sbQuery.Append("\n 			FROM	SummaryPgDaily2 A WITH (NOLOCK)");
					sbQuery.Append("\n 			WHERE	LogDay like @MaxMon + '%'");
					sbQuery.Append("\n 			group by Category, Genre ) V1");
                    sbQuery.Append("\n LEFT OUTER JOIN  AdTargetsHanaTV.dbo.Category C ON V1.CATEGORY = C.CATEGORYCODE AND C.InventoryYn = 'Y'");
					sbQuery.Append("\n GROUP By case when c.CategoryCode is null then 99999 else v1.category end");
					sbQuery.Append("\n 		   ,case when c.CategoryCode is null then 99999 else v1.Genre	 end");
				}
				#endregion

				#region [ 직전월 ]
				else if(SearchType == 3)
				{
					sbQuery.Append("\n DECLARE @MaxMon CHAR(4);");
					// 직전월 데이터를 기준으로 생성한다
					sbQuery.Append("\n SELECT @MaxMon = CONVERT(CHAR(4), DATEADD(MM, -1, GETDATE()), 12);");

                    sbQuery.Append("\n DELETE FROM AdTargetsHanaTV.dbo.InventorySummury WHERE Gubun = 3;");
					sbQuery.Append("\n");
                    sbQuery.Append("\n INSERT INTO AdTargetsHanaTV.dbo.InventorySummury (Gubun, Category, Genre, DayPerHitCnt, ActiveDateCnt)");
					sbQuery.Append("\n SELECT  3	as Gubun");
					sbQuery.Append("\n 		, case when c.CategoryCode is null then 99999 else v1.category end	as	Category");
					sbQuery.Append("\n 		, case when c.CategoryCode is null then 99999 else v1.Genre		end	as	Genre");
					sbQuery.Append("\n 		, sum(v1.DayAvg * isnull(c.InventoryRate,1.0) )	as DayPerHitCnt");
					sbQuery.Append("\n 		, avg(v1.DayCnt)	as ActiveDateCnt");
					sbQuery.Append("\n FROM (	SELECT	  Category");
					sbQuery.Append("\n 					, Genre");
					sbQuery.Append("\n 					, avg(HitCnt)	as DayAvg");
					sbQuery.Append("\n 					, count(*)		as DayCnt");
					sbQuery.Append("\n 			FROM	SummaryPgDaily2 A WITH (NOLOCK)");
					sbQuery.Append("\n 			WHERE	LogDay like @MaxMon + '%'");
					sbQuery.Append("\n 			Group by Category, Genre ) V1");
                    sbQuery.Append("\n LEFT OUTER JOIN  AdTargetsHanaTV.dbo.Category C ON V1.CATEGORY = C.CATEGORYCODE AND C.InventoryYn = 'Y'");
					sbQuery.Append("\n GROUP By case when c.CategoryCode is null then 99999 else v1.category end");
					sbQuery.Append("\n 			,case when c.CategoryCode is null then 99999 else v1.Genre		end");
				}
				#endregion

				// __DEBUG__
				_log.Debug(sbQuery.ToString());
				// __DEBUG__

				// 쿼리실행
				_db.ExecuteNonQuery(sbQuery.ToString());

				// 결과코드 셋트
				inventoryPresentConditionModel.ResultCD = "0000";

				// __DEBUG__
				_log.Debug("<출력정보>");
				_log.Debug("Complete!!!");
				// __DEBUG__

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "SetInventorySummuryData() End");
				_log.Debug("-----------------------------------------");
			}
			catch(Exception ex)
			{
				inventoryPresentConditionModel.ResultCD = "3000";
				inventoryPresentConditionModel.ResultDesc = "인벤토리 기준데이터 생성중 오류가 발생하였습니다";
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