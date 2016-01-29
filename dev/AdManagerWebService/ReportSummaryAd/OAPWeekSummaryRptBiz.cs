// ===============================================================================
//
// OAPWeekSummaryRptBiz.cs
//
// 광고 총괄집계 서비스 
//
// ===============================================================================
// Release history
// 2007.10.26 BJ.PARK OAP도 집계가능토록 함 집계공통이용 메소드 => GetContractItemList()
// ===============================================================================
// Copyright (C) 2007 G-Inno Systems Inc.
// All rights reserved.
// ==============================================================================
/*
 * -------------------------------------------------------
 * [수정사항]
 * -------------------------------------------------------
 * 수정코드  : [E_01]
 * 수정자    : 김보배
 * 수정일    : 2013.10.01
 * 수정부분  :
 *            - GetOAPWeekChannelJump() 수정
 * 수정내용  : 
 *            - 광고가 삭제되었을 경우에 광고명 부분이 빈 공간으로 나오기 때문에
 *              광고가 삭제된 경우에는 안나오도록 수정
 * ---------------------------------------------------------
 * 수정코드  : [E_02]
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

namespace AdManagerWebService.ReportSummaryAd
{
	/// <summary>
	/// OAPWeekSummaryRptBiz에 대한 요약 설명입니다.
	/// </summary>
	public class OAPWeekSummaryRptBiz : BaseBiz
	{

		#region  생성자
		public OAPWeekSummaryRptBiz() : base(FrameSystem.connSummaryDbString)
		{
			_log = FrameSystem.oLog;
		}
		#endregion

		#region OAP주간홈광고 보고서
		/// <summary>
		///  OAP주간홈광고 집계
		/// </summary>
		/// <param name="oAPWeekSummaryRptModel"></param>
		public void GetOAPWeekHomeAd(HeaderModel header, OAPWeekSummaryRptModel oAPWeekSummaryRptModel)
		{
			bool isNotReady = false; // 데이터가 집계가 되지않아 존재하지 않을때.
			try
			{
				StringBuilder sbQuery = null;

				// 데이터베이스를 OPEN한다
				_db.Open();


				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetOAPWeekHomeAd() Start");
				_log.Debug("-----------------------------------------");

				// 일자가 6자리 이상(yyyymmdd)이면 6자리로 만든다.
				if(oAPWeekSummaryRptModel.LogDay1.Length > 6) oAPWeekSummaryRptModel.LogDay1 = oAPWeekSummaryRptModel.LogDay1.Substring(2,6);
				if(oAPWeekSummaryRptModel.LogDay2.Length > 6) oAPWeekSummaryRptModel.LogDay2 = oAPWeekSummaryRptModel.LogDay2.Substring(2,6);			
				
				// __DEBUG__
				_log.Debug("<입력정보>");
				_log.Debug("LogDay1	  :[" + oAPWeekSummaryRptModel.LogDay1   + "]");		// 검색 매체
				_log.Debug("LogDay2	  :[" + oAPWeekSummaryRptModel.LogDay2   + "]");
					
				// __DEBUG__

				string logDay1   = oAPWeekSummaryRptModel.LogDay1;
				string logDay2   = oAPWeekSummaryRptModel.LogDay2;

				SqlParameter[] sqlParams = new SqlParameter[1];
				sqlParams[0] = new SqlParameter("@LogDay1", SqlDbType.VarChar, 6);
				sqlParams[0].Value = oAPWeekSummaryRptModel.LogDay1;
								
				// 쿼리생성
				sbQuery = new StringBuilder();
				sbQuery.Append("\n"
					+ "/* 총기간 광고집행 집계																		\n"
					+ "   조회조건 : 매체코드, 계약번호 또는 광고번호 */											\n"
					+ "																								\n"
					+ "DECLARE @logDay1 varchar(6);   -- 매체코드													\n"
					+ "DECLARE @logDay2 varchar(6);  -- 집행일자														\n"
																				
					+ "SET @logDay1    = '" + logDay1  + "'\n"
					+ "SET @logDay2    = '" + logDay2  + "'\n"	
										
					+ " 	select v.ItemNo	  \n"
					+ " 				,x.ItemName	  \n"
					+ " 				,v.ScheduleOrder	  \n"
					+ " 				,sum(case when v.week = 2 then v.Hit else 0 end) as w1																  \n"
					+ " 				,sum(case when v.week = 3 then v.Hit else 0 end) as w2                                                                \n"
					+ " 				,sum(case when v.week = 4 then v.Hit else 0 end) as w3                                                                \n"
					+ " 				,sum(case when v.week = 5 then v.Hit else 0 end) as w4                                                                \n"
					+ " 				,sum(case when v.week = 6 then v.Hit else 0 end) as w5                                                                \n"
					+ " 				,sum(case when v.week = 7 then v.Hit else 0 end) as w6                                                                \n"
					+ " 				,sum(case when v.week = 1 then v.Hit else 0 end) as w7                                                                \n"					
					+ " 		from	(	select ad.LogDay	as	C9																						  \n"
					+ " 						  ,ad.ItemNo                                                                \n"
					+ " 						  ,ho.ScheduleOrder                                                                \n"
					+ " 						  ,sum(sb.Week)	as week                                                                \n"
					+ " 						  ,sum(ad.Hit00+ad.Hit01+ad.Hit02+ad.Hit03+ad.Hit04+ad.Hit05+ad.Hit06+ad.Hit07+ad.Hit08+ad.Hit09+ad.Hit10+ad.Hit11+ad.Hit12+ad.Hit13+ad.Hit14+ad.Hit15+ad.Hit16+ad.Hit17+ad.Hit18+ad.Hit19+ad.Hit20+ad.Hit21+ad.Hit22+ad.Hit23) as hit                                                                \n"
					+ " 					from SummaryAdHome ad with(NoLock)                                                                  \n"
					+ " 					inner join (select ItemNo                                                                 \n"
					+ " 									   ,min(SchSeq) ScheduleOrder                                                                 \n"
                    + " 								from	AdTargetsHanaTV.dbo.SchPublishHistory with(NoLock)                                                                \n"
					+ " 								where	AckNo in (select	AckNo                                                                \n"
                    + " 													from	AdTargetsHanaTV.dbo.SchPublish with(NoLock)                                                                 \n"
					+ " 													where	State = '30'                                                                 \n"
					+ " 													and		AdSchType = 0                                                                \n"
					+ " 													and		PublishDay between cast( '20' + @LogDay1  as datetime) and dateadd(ss,0,cast( '20' + @LogDay2 as datetime) ) )                                                                 \n"
					+ " 								and		SchType   = 1                                                                 \n"
					+ " 													  group by ItemNo) ho		on ( ad.ItemNo = ho.ItemNo )                                                                 \n"
					+ " 					inner join SummaryBase sb	with(nolock)	on(  ad.LogDay = sb.LogDay)                                                                 \n"
					+ " 					where	ad.LogDay between @LogDay1 and @LogDay2                                                                 \n"
					+ " 					Group by ad.LogDay,ad.ItemNo,ho.ScheduleOrder ) v                                                                 \n"
                    + " 				inner join AdTargetsHanaTV.dbo.ContractItem x with(noLock) on( x.ItemNo = v.ItemNo )                                                                 \n"
					+ " 	group by v.ItemNo, x.ItemName, v.ScheduleOrder                                                                 \n"
					+ " 	union all																									   \n"					
					+ "     select 999999 ItemNo																						   \n"
					+ "     			,'합계'	ItemName																				   \n"
					+ "					,999		ScheduleOrder																		   \n"
					+ "					,sum(case when v.week = 2 then v.Hit else 0 end) as Monday										   \n"
					+ "					,sum(case when v.week = 3 then v.Hit else 0 end) as Tuesday										   \n"
					+ "					,sum(case when v.week = 4 then v.Hit else 0 end) as Wednesday									   \n"
					+ "					,sum(case when v.week = 5 then v.Hit else 0 end) as Thursday									   \n"
					+ "					,sum(case when v.week = 6 then v.Hit else 0 end) as Friday										   \n"
					+ "					,sum(case when v.week = 7 then v.Hit else 0 end) as Saturday									   \n"
					+ "					,sum(case when v.week = 1 then v.Hit else 0 end) as Sunday										   \n"					
					+ "		from	(	select ad.LogDay							  \n"
					+ "		  				  ,ad.ItemNo							  \n"
					+ "		  				  ,ho.ScheduleOrder							  \n"
					+ "		  				  ,sum(sb.Week)	as week							  \n"
					+ "		  				  ,sum(ad.Hit00+ad.Hit01+ad.Hit02+ad.Hit03+ad.Hit04+ad.Hit05+ad.Hit06+ad.Hit07+ad.Hit08+ad.Hit09+ad.Hit10+ad.Hit11+ad.Hit12+ad.Hit13+ad.Hit14+ad.Hit15+ad.Hit16+ad.Hit17+ad.Hit18+ad.Hit19+ad.Hit20+ad.Hit21+ad.Hit22+ad.Hit23) as hit							  \n"
					+ "		  			from SummaryAdHome ad with(NoLock)					\n"
					+ "		  			inner join (select	 ItemNo							\n"
					+ "		  								,min(SchSeq) ScheduleOrder		\n"
                    + "								from	AdTargetsHanaTV.dbo.SchPublishHistory with(NoLock)	\n"
					+ "								where	AckNo in (	select	AckNo		\n"
                    + "													from	AdTargetsHanaTV.dbo.SchPublish with(NoLock)	\n"
					+ "		  										  	where	State = '30'			\n"
					+ "		  											and		AdSchType = 0			\n"
					+ "		  										  	and		PublishDay between cast( '20' + @LogDay1  as datetime) and dateadd(ss,-1,cast( '20' + @LogDay2 as datetime) ) )							   \n"
					+ "		  						and		SchType   = 1							 \n"
					+ "		  						group by ItemNo) ho		on ( ad.ItemNo = ho.ItemNo ) \n"
					+ "		  			inner join SummaryBase sb	with(nolock)	on(  ad.LogDay = sb.LogDay)							  \n"
					+ "				    where	ad.LogDay between @LogDay1 and @LogDay2						  \n"					
					+ "					Group by ad.LogDay,ad.ItemNo,ho.ScheduleOrder ) v						  \n"
                    + "			 inner join AdTargetsHanaTV.dbo.ContractItem x with(noLock) on( x.ItemNo = v.ItemNo )																  \n"
					+ "		Order by ScheduleOrder													  \n"														
					);

				// __DEBUG__
				_log.Debug(sbQuery.ToString());
				// __DEBUG__
				
				// 쿼리실행
				DataSet ds = new DataSet();
				_db.ExecuteQuery(ds,sbQuery.ToString());

				// 결과 DataSet의 데이터모델에 복사
				oAPWeekSummaryRptModel.ReportDataSet = ds.Copy();

				// 결과
				oAPWeekSummaryRptModel.ResultCnt = Utility.GetDatasetCount(oAPWeekSummaryRptModel.ReportDataSet);

				ds.Dispose();

				// 결과코드 셋트
				oAPWeekSummaryRptModel.ResultCD = "0000";

				// __DEBUG__
				_log.Debug("<출력정보>");
				_log.Debug("ResultCnt:[" + oAPWeekSummaryRptModel.ResultCnt + "]");
				// __DEBUG__

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetOAPWeekHomeAd() End");
				_log.Debug("-----------------------------------------");
			}
			catch(Exception ex)
			{
				oAPWeekSummaryRptModel.ResultCD = "3000";
				if(isNotReady)
				{
					oAPWeekSummaryRptModel.ResultDesc = "해당 기간은 데이터가 집계되지 않았습니다.";
				}
				else
				{
					oAPWeekSummaryRptModel.ResultDesc = "OAP주간홈광고 집계 조회중 오류가 발생하였습니다";
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

		#region OAP주간채널점핑 보고서
		/// <summary>
		///  OAP주간홈광고 집계
		/// </summary>
		/// <param name="oAPWeekSummaryRptModel"></param>
		public void GetOAPWeekChannelJump(HeaderModel header, OAPWeekSummaryRptModel oAPWeekSummaryRptModel)
		{
			bool isNotReady = false; // 데이터가 집계가 되지않아 존재하지 않을때.
			try
			{
				StringBuilder sbQuery = null;

				// 데이터베이스를 OPEN한다
				_db.Open();


				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetOAPWeekChannelJump() Start");
				_log.Debug("-----------------------------------------");

				// 일자가 6자리 이상(yyyymmdd)이면 6자리로 만든다.
				if(oAPWeekSummaryRptModel.LogDay1.Length > 6) oAPWeekSummaryRptModel.LogDay1 = oAPWeekSummaryRptModel.LogDay1.Substring(2,6);
				if(oAPWeekSummaryRptModel.LogDay2.Length > 6) oAPWeekSummaryRptModel.LogDay2 = oAPWeekSummaryRptModel.LogDay2.Substring(2,6);			
				
				// __DEBUG__
				_log.Debug("<입력정보>");
				_log.Debug("LogDay1	  :[" + oAPWeekSummaryRptModel.LogDay1   + "]");		// 검색 매체
				_log.Debug("LogDay2	  :[" + oAPWeekSummaryRptModel.LogDay2   + "]");
					
				// __DEBUG__

				string logDay1   = oAPWeekSummaryRptModel.LogDay1;
				string logDay2   = oAPWeekSummaryRptModel.LogDay2;

				SqlParameter[] sqlParams = new SqlParameter[1];
				sqlParams[0] = new SqlParameter("@LogDay1", SqlDbType.VarChar, 6);
				sqlParams[0].Value = oAPWeekSummaryRptModel.LogDay1;
								
				// 쿼리생성
				sbQuery = new StringBuilder();

				sbQuery.Append("/* 총기간 광고집행 집계																																				\r\n");
				sbQuery.Append("조회조건 : 매체코드, 계약번호 또는 광고번호 */																														\r\n");
				sbQuery.Append("																																									\r\n");
				sbQuery.Append("DECLARE @logDay1 varchar(6);   -- 매체코드																															\r\n");
				sbQuery.Append("DECLARE @logDay2 varchar(6);  -- 집행일자																															\r\n");
				sbQuery.Append("																																									\r\n");
				sbQuery.Append("SET @logDay1    = '" + logDay1 + "'																																	\r\n");
				sbQuery.Append("SET @logDay2    = '" + logDay2 + "'																																	\r\n");
				sbQuery.Append("\r																																									\r\n");
				sbQuery.Append("SELECT	v2.ItemNo																																					\r\n");
                sbQuery.Append("		,isnull((SELECT ItemName FROM AdTargetsHanaTV.dbo.ContractItem	NOLOCK WHERE ItemNo = v2.ItemNo), '') AS AdName																	\r\n");
                sbQuery.Append("		,isnull((SELECT min(ProgramNm) FROM AdTargetsHanaTV.dbo.Program	NOLOCK WHERE Channel= v2.Channel GROUP BY channel), '') AS ProgName												\r\n");
				sbQuery.Append("		, d0, t0, w0, CASE v2.ItemNo WHEN 999999 THEN CASE v2.d0 WHEN 0 THEN 0 ELSE CAST(v2.w0 / CAST( v2.d0 AS real) * 100 AS DECIMAL(10,2)) END ELSE r0 END	AS r0									\r\n");
				sbQuery.Append("		, d1, t1, w1, CASE v2.ItemNo WHEN 999999 THEN CASE v2.d1 WHEN 0 THEN 0 ELSE CAST(v2.w1 / CAST( v2.d1 AS real) * 100 AS DECIMAL(10,2)) END ELSE r1 END	AS r1									\r\n");
				sbQuery.Append("		, d2, t2, w2, CASE v2.ItemNo WHEN 999999 THEN CASE v2.d2 WHEN 0 THEN 0 ELSE CAST(v2.w2 / CAST( v2.d2 AS real) * 100 AS DECIMAL(10,2)) END ELSE r2 END	AS r2									\r\n");
				sbQuery.Append("		, d3, t3, w3, CASE v2.ItemNo WHEN 999999 THEN CASE v2.d3 WHEN 0 THEN 0 ELSE CAST(v2.w3 / CAST( v2.d3 AS real) * 100 AS DECIMAL(10,2)) END ELSE r3 END	AS r3									\r\n");
				sbQuery.Append("		, d4, t4, w4, CASE v2.ItemNo WHEN 999999 THEN CASE v2.d4 WHEN 0 THEN 0 ELSE CAST(v2.w4 / CAST( v2.d4 AS real) * 100 AS DECIMAL(10,2)) END ELSE r4 END	AS r4									\r\n");
				sbQuery.Append("		, d5, t5, w5, CASE v2.ItemNo WHEN 999999 THEN CASE v2.d5 WHEN 0 THEN 0 ELSE CAST(v2.w5 / CAST( v2.d5 AS real) * 100 AS DECIMAL(10,2)) END ELSE r5 END	AS r5									\r\n");
				sbQuery.Append("		, d6, t6, w6, CASE v2.ItemNo WHEN 999999 THEN CASE v2.d6 WHEN 0 THEN 0 ELSE CAST(v2.w6 / CAST( v2.d6 AS real) * 100 AS DECIMAL(10,2)) END ELSE r6 END	AS r6									\r\n");
				sbQuery.Append("		, d7, t7, w7, CASE v2.ItemNo WHEN 999999 THEN CASE v2.d7 WHEN 0 THEN 0 ELSE CAST(v2.w7 / CAST( v2.d7 AS real) * 100 AS DECIMAL(10,2)) END ELSE r7 END	AS r7									\r\n");
				sbQuery.Append("FROM																																								\r\n");
				sbQuery.Append("(		SELECT	 CASE t.rownum WHEN 1 THEN v.ItemNo  ELSE 999999  END	AS  itemNo																					\r\n");
				sbQuery.Append("			,CASE t.rownum WHEN 1 THEN v.Channel	ELSE 999999	END	AS	Channel																						\r\n");
				sbQuery.Append("			,SUM(v.Hit) AS d0, SUM(t0) AS t0, SUM(v.tot) AS w0, SUM(v.r0) AS r0																						\r\n");
				sbQuery.Append("			,SUM(v.d1)  AS d1, SUM(t1) AS t1, SUM(v.w1)	AS w1, SUM(v.r1) AS r1																						\r\n");
				sbQuery.Append("			,SUM(v.d2)  AS d2, SUM(t2) AS t2, SUM(v.w2)	AS w2, SUM(v.r2) AS r2																						\r\n");
				sbQuery.Append("			,SUM(v.d3)  AS d3, SUM(t3) AS t3, SUM(v.w3)	AS w3, SUM(v.r3) AS r3																						\r\n");
				sbQuery.Append("			,SUM(v.d4)  AS d4, SUM(t4) AS t4, SUM(v.w4)	AS w4, SUM(v.r4) AS r4																						\r\n");
				sbQuery.Append("			,SUM(v.d5)  AS d5, SUM(t5) AS t5, SUM(v.w5)	AS w5, SUM(v.r5) AS r5																						\r\n");
				sbQuery.Append("			,SUM(v.d6)  AS d6, SUM(t6) AS t6, SUM(v.w6)	AS w6, SUM(v.r6) AS r6																						\r\n");
				sbQuery.Append("			,SUM(v.d7)  AS d7, SUM(t7) AS t7, SUM(v.w7)	AS w7, SUM(v.r7) AS r7																						\r\n");
				sbQuery.Append("		FROM  																																						\r\n");
				sbQuery.Append("		(		SELECT	 jump.ItemNo																																\r\n");
				sbQuery.Append("						,jump.Channel																																\r\n");
				sbQuery.Append("						,hit.Hit, t0, jump.tot AS tot ,CASE WHEN hit.Hit > 0 THEN CAST(jump.tot / CAST(hit.Hit AS real) * 100 AS DECIMAL(10,2))  ELSE 0.00 END r0	\r\n");
				sbQuery.Append("						,hit.d1,  t1, jump.MON AS w1  ,CASE WHEN hit.d1  > 0 THEN CAST(jump.MON / CAST(hit.d1  AS real) * 100 AS DECIMAL(10,2))  ELSE 0.00 END r1	\r\n");
				sbQuery.Append("						,hit.d2,  t2, jump.Tue AS w2  ,CASE WHEN hit.d2  > 0 THEN CAST(jump.Tue / CAST(hit.d2  AS real) * 100 AS DECIMAL(10,2))  ELSE 0.00 END r2	\r\n");
				sbQuery.Append("						,hit.d3,  t3, jump.Wed AS w3  ,CASE WHEN hit.d3  > 0 THEN CAST(jump.Wed / CAST(hit.d3  AS real) * 100 AS DECIMAL(10,2))  ELSE 0.00 END r3	\r\n");
				sbQuery.Append("						,hit.d4,  t4, jump.Thu AS w4  ,CASE WHEN hit.d4  > 0 THEN CAST(jump.Thu / CAST(hit.d4  AS real) * 100 AS DECIMAL(10,2))  ELSE 0.00 END r4	\r\n");
				sbQuery.Append("						,hit.d5,  t5, jump.Fri AS w5  ,CASE WHEN hit.d5  > 0 THEN CAST(jump.Fri / CAST(hit.d5  AS real) * 100 AS DECIMAL(10,2))  ELSE 0.00 END r5	\r\n");
				sbQuery.Append("						,hit.d6,  t6, jump.Sat AS w6  ,CASE WHEN hit.d6  > 0 THEN CAST(jump.Sat / CAST(hit.d6  AS real) * 100 AS DECIMAL(10,2))  ELSE 0.00 END r6	\r\n");
				sbQuery.Append("						,hit.d7,  t7, jump.Sun AS w7  ,CASE WHEN hit.d7  > 0 THEN CAST(jump.Sun / CAST(hit.d7  AS real) * 100 AS DECIMAL(10,2))  ELSE 0.00 END r7	\r\n");
				sbQuery.Append("				FROM																							\r\n");
				sbQuery.Append("				(		SELECT	 Channel																		\r\n");
				sbQuery.Append("								,SUM(a.HitCnt) AS Hit															\r\n");
				sbQuery.Append("								,SUM(CASE WHEN sb.week = 2 THEN a.HitCnt ELSE 0 END) AS d1						\r\n");
				sbQuery.Append("								,SUM(CASE WHEN sb.week = 3 THEN a.HitCnt ELSE 0 END) AS d2						\r\n");
				sbQuery.Append("								,SUM(CASE WHEN sb.week = 4 THEN a.HitCnt ELSE 0 END) AS d3						\r\n");
				sbQuery.Append("								,SUM(CASE WHEN sb.week = 5 THEN a.HitCnt ELSE 0 END) AS d4						\r\n");
				sbQuery.Append("								,SUM(CASE WHEN sb.week = 6 THEN a.HitCnt ELSE 0 END) AS d5						\r\n");
				sbQuery.Append("								,SUM(CASE WHEN sb.week = 7 THEN a.HitCnt ELSE 0 END) AS d6						\r\n");
				sbQuery.Append("								,SUM(CASE WHEN sb.week = 1 THEN a.HitCnt ELSE 0 END) AS d7						\r\n");
				sbQuery.Append("						FROM		dbo.SummaryPgDaily3	a  WITH(NOLOCK)											\r\n");
				sbQuery.Append("						INNER JOIN	SummaryBase		sb WITH(NOLOCK) ON a.LogDay = sb.LogDay						\r\n");
                sbQuery.Append("						INNER JOIN	AdTargetsHanaTV.dbo.Program			pb WITH(NOLOCK)	ON a.ProgKey = pb.ProgramKey				\r\n");
				sbQuery.Append("						WHERE		a.LogDay between @LogDay1 AND @LogDay2										\r\n");
				sbQuery.Append("						AND			a.ProgKey > 0																\r\n");
				sbQuery.Append("						GROUP BY	pb.Channel ) hit															\r\n");
				sbQuery.Append("						INNER JOIN																				\r\n");
				sbQuery.Append("						(	SELECT	 ItemNo																		\r\n");
				sbQuery.Append("									,Channel																	\r\n");
				sbQuery.Append("									,SUM(tot)	AS	tot															\r\n");
				sbQuery.Append("									,SUM(HitCnt) AS t0															\r\n");
				sbQuery.Append("									,SUM(CASE WHEN week = 2 THEN tot ELSE 0 END) AS MON							\r\n");
				sbQuery.Append("									,SUM(CASE WHEN week = 3 THEN tot ELSE 0 END) AS Tue							\r\n");
				sbQuery.Append("									,SUM(CASE WHEN week = 4 THEN tot ELSE 0 END) AS Wed							\r\n");
				sbQuery.Append("									,SUM(CASE WHEN week = 5 THEN tot ELSE 0 END) AS Thu							\r\n");
				sbQuery.Append("									,SUM(CASE WHEN week = 6 THEN tot ELSE 0 END) AS Fri							\r\n");
				sbQuery.Append("									,SUM(CASE WHEN week = 7 THEN tot ELSE 0 END) AS Sat							\r\n");
				sbQuery.Append("									,SUM(CASE WHEN week = 1 THEN tot ELSE 0 END) AS Sun							\r\n");
				sbQuery.Append("									,SUM(CASE WHEN week = 2 THEN HitCnt ELSE 0 END) AS t1						\r\n");
				sbQuery.Append("									,SUM(CASE WHEN week = 3 THEN HitCnt ELSE 0 END) AS t2						\r\n");
				sbQuery.Append("									,SUM(CASE WHEN week = 4 THEN HitCnt ELSE 0 END) AS t3						\r\n");
				sbQuery.Append("									,SUM(CASE WHEN week = 5 THEN HitCnt ELSE 0 END) AS t4						\r\n");
				sbQuery.Append("									,SUM(CASE WHEN week = 6 THEN HitCnt ELSE 0 END) AS t5						\r\n");
				sbQuery.Append("									,SUM(CASE WHEN week = 7 THEN HitCnt ELSE 0 END) AS t6						\r\n");
				sbQuery.Append("									,SUM(CASE WHEN week = 1 THEN HitCnt ELSE 0 END) AS t7						\r\n");
				sbQuery.Append("							FROM																				\r\n");
				sbQuery.Append("							(	SELECT	 pb.Channel																\r\n");
				sbQuery.Append("										,a.ItemNo																\r\n");
				sbQuery.Append("										,sb.LogDay																\r\n");
				sbQuery.Append("										,sb.week																\r\n");
				sbQuery.Append("										,H00+H01+H02+H03+H04+H05+H06+H07+H08+H09+H10+H11+H12+H13+H14+H15+H16+H17+H18+H19+H20+H21+H22+H23 TOT	\r\n");
				sbQuery.Append("										,isnull(HitCnt, 0) HitCnt												\r\n");
				sbQuery.Append("								FROM																			\r\n");
				sbQuery.Append("								(	SELECT	 LogDay																\r\n");
				sbQuery.Append("											,ItemNo																\r\n");
				sbQuery.Append("											,ProgKey															\r\n");
				sbQuery.Append("											,SUM(Hit00) h00	,SUM(Hit01) h01	,SUM(Hit02) h02	,SUM(Hit03) h03		\r\n");
				sbQuery.Append("											,SUM(Hit04) h04	,SUM(Hit05) h05	,SUM(Hit06) h06	,SUM(hit07)	h07		\r\n");
				sbQuery.Append("											,SUM(hit08)	h08	,SUM(hit09)	h09	,SUM(hit10)	h10	,SUM(hit11)	h11		\r\n");
				sbQuery.Append("											,SUM(hit12)	h12	,SUM(hit13)	h13	,SUM(hit14)	h14	,SUM(hit15)	h15		\r\n");
				sbQuery.Append("											,SUM(hit16)	h16	,SUM(hit17)	h17	,SUM(hit18)	h18	,SUM(hit19)	h19		\r\n");
				sbQuery.Append("											,SUM(hit20)	h20	,SUM(hit21)	h21	,SUM(hit22)	h22	,SUM(hit23)	h23		\r\n");
				sbQuery.Append("									FROM	SummaryPgJump WITH(NOLOCK)											\r\n");
				sbQuery.Append("									WHERE	LogDay between @LogDay1 AND @LogDay2								\r\n");
				sbQuery.Append("									GROUP BY LogDay,ItemNo,ProgKey												\r\n");
				sbQuery.Append("								) a																				\r\n");
				sbQuery.Append("								INNER JOIN dbo.SummaryBase	sb WITH(NOLOCK) ON a.LogDay = sb.LogDay				\r\n");
                sbQuery.Append("								INNER JOIN AdTargetsHanaTV.dbo.Program		pb WITH(NOLOCK)	ON a.ProgKey = pb.ProgramKey		\r\n");
                sbQuery.Append("                                INNER JOIN AdTargetsHanaTV.dbo.ContractItem ci WITH(NOLOCK) ON a.ItemNo = ci.ItemNo		        \r\n"); // [E_01]
				sbQuery.Append("								LEFT JOIN																		\r\n");
				sbQuery.Append("								(	SELECT		 Channel														\r\n");
				sbQuery.Append("												,ItemNo															\r\n");
				sbQuery.Append("												,sb.LogDay														\r\n");
				sbQuery.Append("												,SUM(st.HitCnt) AS HitCnt										\r\n");
				sbQuery.Append("									FROM		SummaryTrigger st  WITH(NOLOCK)									\r\n");
				sbQuery.Append("									INNER JOIN	SummaryBase sb WITH(NOLOCK) ON st.LogDay = sb.LogDay			\r\n");
                sbQuery.Append("									INNER JOIN  AdTargetsHanaTV.dbo.Program		pb WITH(NOLOCK) ON st.ProgKey = pb.ProgramKey		\r\n");
				sbQuery.Append("									GROUP BY	Channel, ItemNo, sb.LogDay										\r\n");
				sbQuery.Append("								) st																			\r\n");
				sbQuery.Append("								ON	pb.Channel	= st.Channel													\r\n");
				sbQuery.Append("								AND	a.ItemNo	= st.ItemNo														\r\n");
				sbQuery.Append("								AND	a.LogDay	= st.LogDay														\r\n");
				sbQuery.Append("						) x																						\r\n");
				sbQuery.Append("						GROUP BY ItemNo,Channel																	\r\n");
				sbQuery.Append("				) jump																							\r\n");
				sbQuery.Append("				ON jump.Channel	= hit.Channel 																	\r\n");
                sbQuery.Append("		) v , AdTargetsHanaTV.dbo.copy_t t																							\r\n");
				sbQuery.Append("		WHERE t.rownum <= 2																						\r\n");
				sbQuery.Append("		GROUP BY CASE t.rownum WHEN 1 THEN v.ItemNo  ELSE 999999 END											\r\n");
				sbQuery.Append("				,CASE t.rownum WHEN 1 THEN v.Channel ELSE 999999 END											\r\n");
				sbQuery.Append(") v2																											\r\n");
				sbQuery.Append("ORDER BY 1,2																									\r\n");


				// __DEBUG__
				_log.Debug(sbQuery.ToString());
				// __DEBUG__
				
				// 쿼리실행
				DataSet ds = new DataSet();
				_db.ExecuteQuery(ds,sbQuery.ToString());

				// 결과 DataSet의 데이터모델에 복사
				oAPWeekSummaryRptModel.ReportDataSet = ds.Copy();

				// 결과
				oAPWeekSummaryRptModel.ResultCnt = Utility.GetDatasetCount(oAPWeekSummaryRptModel.ReportDataSet);

				ds.Dispose();

				// 결과코드 셋트
				oAPWeekSummaryRptModel.ResultCD = "0000";

				// __DEBUG__
				_log.Debug("<출력정보>");
				_log.Debug("ResultCnt:[" + oAPWeekSummaryRptModel.ResultCnt + "]");
				// __DEBUG__

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetOAPWeekChannelJump() End");
				_log.Debug("-----------------------------------------");
			}
			catch(Exception ex)
			{
				oAPWeekSummaryRptModel.ResultCD = "3000";
				if(isNotReady)
				{
					oAPWeekSummaryRptModel.ResultDesc = "해당 기간은 데이터가 집계되지 않았습니다.";
				}
				else
				{
					oAPWeekSummaryRptModel.ResultDesc = "OAP주간채널점핑 집계 조회중 오류가 발생하였습니다";
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

        #region Home 상업광고 레포팅
        /// <summary>
        /// 홈상업광고집계
        /// 귀챤아서 모델사용하지 않았슴
        /// </summary>
        /// <param name="beginDay"></param>
        /// <param name="endDay"></param>
        /// <returns></returns>
        public DataSet GetHomeCmReport(string beginDay,string endDay,string mediaRep)
        {
            DataSet rtnData = null;
            try
            {
                StringBuilder sbQuery = null;

                _db.Open();
                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "GetHomeCmReport() Start");
                _log.Debug("-----------------------------------------");

                SqlParameter[] sqlParams = new SqlParameter[3];
                sqlParams[0] = new SqlParameter("@LogDay1", SqlDbType.VarChar, 6);
                sqlParams[0].Value = beginDay;

                sqlParams[1] = new SqlParameter("@LogDay2", SqlDbType.VarChar, 6);
                sqlParams[1].Value = endDay;

                sqlParams[2] = new SqlParameter("@RepCode", SqlDbType.VarChar, 1);
                sqlParams[2].Value = mediaRep;

                sbQuery = new StringBuilder();
                // 헤더 타이틀 부분
                sbQuery.Append("\n select   00000000           as ItemNo ");
                sbQuery.Append("\n         ,'광고명'           as ItemName ");
                sbQuery.Append("\n         ,'소계'             as cTot        ");
                sbQuery.Append("\n         ,max(case v1.rowNum when 1 then substring(LogDay,3,2)+'-'+substring(LogDay,5,2) else '' end )   as c01 ");
                sbQuery.Append("\n         ,max(case v1.rowNum when 2 then substring(LogDay,3,2)+'-'+substring(LogDay,5,2) else '' end )   as c02 ");
                sbQuery.Append("\n         ,max(case v1.rowNum when 3 then substring(LogDay,3,2)+'-'+substring(LogDay,5,2) else '' end )   as c03 ");
                sbQuery.Append("\n         ,max(case v1.rowNum when 4 then substring(LogDay,3,2)+'-'+substring(LogDay,5,2) else '' end )   as c04 ");
                sbQuery.Append("\n         ,max(case v1.rowNum when 5 then substring(LogDay,3,2)+'-'+substring(LogDay,5,2) else '' end )   as c05 ");
                sbQuery.Append("\n         ,max(case v1.rowNum when 6 then substring(LogDay,3,2)+'-'+substring(LogDay,5,2) else '' end )   as c06 ");
                sbQuery.Append("\n         ,max(case v1.rowNum when 7 then substring(LogDay,3,2)+'-'+substring(LogDay,5,2) else '' end )   as c07 ");
                sbQuery.Append("\n         ,max(case v1.rowNum when 8 then substring(LogDay,3,2)+'-'+substring(LogDay,5,2) else '' end )   as c08 ");
                sbQuery.Append("\n         ,max(case v1.rowNum when 9 then substring(LogDay,3,2)+'-'+substring(LogDay,5,2) else '' end )   as c09 ");
                sbQuery.Append("\n         ,max(case v1.rowNum when 10 then substring(LogDay,3,2)+'-'+substring(LogDay,5,2) else '' end )   as c10 ");
                sbQuery.Append("\n         ,max(case v1.rowNum when 11 then substring(LogDay,3,2)+'-'+substring(LogDay,5,2) else '' end )   as c11 ");
                sbQuery.Append("\n         ,max(case v1.rowNum when 12 then substring(LogDay,3,2)+'-'+substring(LogDay,5,2) else '' end )   as c12 ");
                sbQuery.Append("\n         ,max(case v1.rowNum when 13 then substring(LogDay,3,2)+'-'+substring(LogDay,5,2) else '' end )   as c13 ");
                sbQuery.Append("\n         ,max(case v1.rowNum when 14 then substring(LogDay,3,2)+'-'+substring(LogDay,5,2) else '' end )   as c14 ");
                sbQuery.Append("\n         ,max(case v1.rowNum when 15 then substring(LogDay,3,2)+'-'+substring(LogDay,5,2) else '' end )   as c15 ");
                sbQuery.Append("\n         ,max(case v1.rowNum when 16 then substring(LogDay,3,2)+'-'+substring(LogDay,5,2) else '' end )   as c16 ");
                sbQuery.Append("\n         ,max(case v1.rowNum when 17 then substring(LogDay,3,2)+'-'+substring(LogDay,5,2) else '' end )   as c17 ");
                sbQuery.Append("\n         ,max(case v1.rowNum when 18 then substring(LogDay,3,2)+'-'+substring(LogDay,5,2) else '' end )   as c18 ");
                sbQuery.Append("\n         ,max(case v1.rowNum when 19 then substring(LogDay,3,2)+'-'+substring(LogDay,5,2) else '' end )   as c19 ");
                sbQuery.Append("\n         ,max(case v1.rowNum when 20 then substring(LogDay,3,2)+'-'+substring(LogDay,5,2) else '' end )   as c20 ");
                sbQuery.Append("\n         ,max(case v1.rowNum when 21 then substring(LogDay,3,2)+'-'+substring(LogDay,5,2) else '' end )   as c21 ");
                sbQuery.Append("\n         ,max(case v1.rowNum when 22 then substring(LogDay,3,2)+'-'+substring(LogDay,5,2) else '' end )   as c22 ");
                sbQuery.Append("\n         ,max(case v1.rowNum when 23 then substring(LogDay,3,2)+'-'+substring(LogDay,5,2) else '' end )   as c23 ");
                sbQuery.Append("\n         ,max(case v1.rowNum when 24 then substring(LogDay,3,2)+'-'+substring(LogDay,5,2) else '' end )   as c24 ");
                sbQuery.Append("\n         ,max(case v1.rowNum when 25 then substring(LogDay,3,2)+'-'+substring(LogDay,5,2) else '' end )   as c25 ");
                sbQuery.Append("\n         ,max(case v1.rowNum when 26 then substring(LogDay,3,2)+'-'+substring(LogDay,5,2) else '' end )   as c26 ");
                sbQuery.Append("\n         ,max(case v1.rowNum when 27 then substring(LogDay,3,2)+'-'+substring(LogDay,5,2) else '' end )   as c27 ");
                sbQuery.Append("\n         ,max(case v1.rowNum when 28 then substring(LogDay,3,2)+'-'+substring(LogDay,5,2) else '' end )   as c28 ");
                sbQuery.Append("\n         ,max(case v1.rowNum when 29 then substring(LogDay,3,2)+'-'+substring(LogDay,5,2) else '' end )   as c29 ");
                sbQuery.Append("\n         ,max(case v1.rowNum when 30 then substring(LogDay,3,2)+'-'+substring(LogDay,5,2) else '' end )   as c30 ");
                sbQuery.Append("\n         ,max(case v1.rowNum when 31 then substring(LogDay,3,2)+'-'+substring(LogDay,5,2) else '' end )   as c31 ");
                sbQuery.Append("\n from ( ");
                sbQuery.Append("\n          select  LogDay,cast(Week as char(1)) as week,row_number() over( order by LogDay) as rowNum ");
                sbQuery.Append("\n          from    SummaryBase with(noLock) ");
                sbQuery.Append("\n          where	LogDay between @LogDay1 and @LogDay2 ) v1 ");
                // 데이터 부분
                sbQuery.Append("\n union all ");
                sbQuery.Append("\n select ItemNo ");
                sbQuery.Append("\n       ,AdTargetsHanaTV.dbo.ufnGetItemName(ItemNo)       as ItemName ");
                sbQuery.Append("\n       ,AdTargetsHanaTV.dbo.ufnIntTypeString( sum(Imp))  as cTot ");
                sbQuery.Append("\n       ,AdTargetsHanaTV.dbo.ufnIntTypeString( sum(case rownum when 1 then Imp else 0 end )) ");
                sbQuery.Append("\n       ,AdTargetsHanaTV.dbo.ufnIntTypeString( sum(case rownum when 2 then Imp else 0 end )) ");
                sbQuery.Append("\n       ,AdTargetsHanaTV.dbo.ufnIntTypeString( sum(case rownum when 3 then Imp else 0 end )) ");
                sbQuery.Append("\n       ,AdTargetsHanaTV.dbo.ufnIntTypeString( sum(case rownum when 4 then Imp else 0 end )) ");
                sbQuery.Append("\n       ,AdTargetsHanaTV.dbo.ufnIntTypeString( sum(case rownum when 5 then Imp else 0 end )) ");
                sbQuery.Append("\n       ,AdTargetsHanaTV.dbo.ufnIntTypeString( sum(case rownum when 6 then Imp else 0 end )) ");
                sbQuery.Append("\n       ,AdTargetsHanaTV.dbo.ufnIntTypeString( sum(case rownum when 7 then Imp else 0 end )) ");
                sbQuery.Append("\n       ,AdTargetsHanaTV.dbo.ufnIntTypeString( sum(case rownum when 8 then Imp else 0 end )) ");
                sbQuery.Append("\n       ,AdTargetsHanaTV.dbo.ufnIntTypeString( sum(case rownum when 9 then Imp else 0 end )) ");
                sbQuery.Append("\n       ,AdTargetsHanaTV.dbo.ufnIntTypeString( sum(case rownum when 10 then Imp else 0 end )) ");
                sbQuery.Append("\n       ,AdTargetsHanaTV.dbo.ufnIntTypeString( sum(case rownum when 11 then Imp else 0 end )) ");
                sbQuery.Append("\n       ,AdTargetsHanaTV.dbo.ufnIntTypeString( sum(case rownum when 12 then Imp else 0 end )) ");
                sbQuery.Append("\n       ,AdTargetsHanaTV.dbo.ufnIntTypeString( sum(case rownum when 13 then Imp else 0 end )) ");
                sbQuery.Append("\n       ,AdTargetsHanaTV.dbo.ufnIntTypeString( sum(case rownum when 14 then Imp else 0 end )) ");
                sbQuery.Append("\n       ,AdTargetsHanaTV.dbo.ufnIntTypeString( sum(case rownum when 15 then Imp else 0 end )) ");
                sbQuery.Append("\n       ,AdTargetsHanaTV.dbo.ufnIntTypeString( sum(case rownum when 16 then Imp else 0 end )) ");
                sbQuery.Append("\n       ,AdTargetsHanaTV.dbo.ufnIntTypeString( sum(case rownum when 17 then Imp else 0 end )) ");
                sbQuery.Append("\n       ,AdTargetsHanaTV.dbo.ufnIntTypeString( sum(case rownum when 18 then Imp else 0 end )) ");
                sbQuery.Append("\n       ,AdTargetsHanaTV.dbo.ufnIntTypeString( sum(case rownum when 19 then Imp else 0 end )) ");
                sbQuery.Append("\n       ,AdTargetsHanaTV.dbo.ufnIntTypeString( sum(case rownum when 20 then Imp else 0 end )) ");
                sbQuery.Append("\n       ,AdTargetsHanaTV.dbo.ufnIntTypeString( sum(case rownum when 21 then Imp else 0 end )) ");
                sbQuery.Append("\n       ,AdTargetsHanaTV.dbo.ufnIntTypeString( sum(case rownum when 22 then Imp else 0 end )) ");
                sbQuery.Append("\n       ,AdTargetsHanaTV.dbo.ufnIntTypeString( sum(case rownum when 23 then Imp else 0 end )) ");
                sbQuery.Append("\n       ,AdTargetsHanaTV.dbo.ufnIntTypeString( sum(case rownum when 24 then Imp else 0 end )) ");
                sbQuery.Append("\n       ,AdTargetsHanaTV.dbo.ufnIntTypeString( sum(case rownum when 25 then Imp else 0 end )) ");
                sbQuery.Append("\n       ,AdTargetsHanaTV.dbo.ufnIntTypeString( sum(case rownum when 26 then Imp else 0 end )) ");
                sbQuery.Append("\n       ,AdTargetsHanaTV.dbo.ufnIntTypeString( sum(case rownum when 27 then Imp else 0 end )) ");
                sbQuery.Append("\n       ,AdTargetsHanaTV.dbo.ufnIntTypeString( sum(case rownum when 28 then Imp else 0 end )) ");
                sbQuery.Append("\n       ,AdTargetsHanaTV.dbo.ufnIntTypeString( sum(case rownum when 29 then Imp else 0 end )) ");
                sbQuery.Append("\n       ,AdTargetsHanaTV.dbo.ufnIntTypeString( sum(case rownum when 30 then Imp else 0 end )) ");
                sbQuery.Append("\n       ,AdTargetsHanaTV.dbo.ufnIntTypeString( sum(case rownum when 31 then Imp else 0 end )) ");
                sbQuery.Append("\n from ( ");
                sbQuery.Append("\n         select   j.ItemNo ");
                sbQuery.Append("\n                 ,j.LogDay ");
                sbQuery.Append("\n                 ,j.week ");
                sbQuery.Append("\n                 ,row_number() over( partition by j.ItemNo order by j.LogDay) as rownum ");
                sbQuery.Append("\n                 ,isnull( k.TotCnt,0 ) as Imp ");
                sbQuery.Append("\n         from ( ");
                sbQuery.Append("\n                 select ItemNo,LogDay,Week ");
                sbQuery.Append("\n                 from ( ");
                sbQuery.Append("\n                         select  LogDay,Week ");
                sbQuery.Append("\n                         from    SummaryBase ");
                sbQuery.Append("\n                         where	LogDay between @LogDay1 and @LogDay2 ) x ");
                sbQuery.Append("\n                 cross join ");
                sbQuery.Append("\n                      ( ");
                sbQuery.Append("\n                         select	distinct a.ItemNo ");
                sbQuery.Append("\n                         from	SummaryAdHome a with(noLock) ");
                sbQuery.Append("\n                         inner join AdTargetsHanaTV.dbo.SchHomeCmHistory b with(noLock) on b.ItemNo = a.ItemNo and b.LogDay=a.LogDay  ");
                if (!mediaRep.Equals("00"))
                {
                    sbQuery.Append("\n                         inner join AdTargetsHanaTV.dbo.ContractItem c with(noLock) on c.ItemNo = a.ItemNo and c.RapCode = @RepCode  ");
                }
                sbQuery.Append("\n                         where	a.LogDay between @LogDay1 and @LogDay2 ) y ) j ");
                sbQuery.Append("\n         left outer join ");
                sbQuery.Append("\n              ( select	 a.ItemNo ");
                sbQuery.Append("\n                         ,a.LogDay ");
                sbQuery.Append("\n                         ,(Hit00+Hit01+Hit02+Hit03+Hit04+Hit05+Hit06+Hit07+Hit08+Hit09+Hit10+Hit11+Hit12+Hit13+Hit14+Hit15+Hit16+Hit17+Hit18+Hit19+Hit20+Hit21+Hit22+Hit23) as TotCnt ");
                sbQuery.Append("\n                 from	SummaryAdHome a with(noLock) ");
                sbQuery.Append("\n                 inner join AdTargetsHanaTV.dbo.SchHomeCmHistory b with(noLock) on b.ItemNo = a.ItemNo and b.LogDay=a.LogDay  ");
                if (!mediaRep.Equals("00"))
                {
                    sbQuery.Append("\n                         inner join AdTargetsHanaTV.dbo.ContractItem c with(noLock) on c.ItemNo = a.ItemNo and c.RapCode = @RepCode  ");
                }
                sbQuery.Append("\n                 where	a.LogDay between @LogDay1 and @LogDay2 ) k on k.ItemNo = j.ItemNo and k.LogDay = j.LogDay ) z ");
                sbQuery.Append("\n group by ItemNo ");
                sbQuery.Append("\n union all ");
                // 합계부분
                sbQuery.Append("\n select 999999 ItemNo ");
                sbQuery.Append("\n         ,'합계'           as ItemName ");
                sbQuery.Append("\n       ,AdTargetsHanaTV.dbo.ufnIntTypeString( sum(Imp)) as cTot ");
                sbQuery.Append("\n       ,AdTargetsHanaTV.dbo.ufnIntTypeString( sum(case rownum when 1 then Imp else 0 end )) ");
                sbQuery.Append("\n       ,AdTargetsHanaTV.dbo.ufnIntTypeString( sum(case rownum when 2 then Imp else 0 end )) ");
                sbQuery.Append("\n       ,AdTargetsHanaTV.dbo.ufnIntTypeString( sum(case rownum when 3 then Imp else 0 end )) ");
                sbQuery.Append("\n       ,AdTargetsHanaTV.dbo.ufnIntTypeString( sum(case rownum when 4 then Imp else 0 end )) ");
                sbQuery.Append("\n       ,AdTargetsHanaTV.dbo.ufnIntTypeString( sum(case rownum when 5 then Imp else 0 end )) ");
                sbQuery.Append("\n       ,AdTargetsHanaTV.dbo.ufnIntTypeString( sum(case rownum when 6 then Imp else 0 end )) ");
                sbQuery.Append("\n       ,AdTargetsHanaTV.dbo.ufnIntTypeString( sum(case rownum when 7 then Imp else 0 end )) ");
                sbQuery.Append("\n       ,AdTargetsHanaTV.dbo.ufnIntTypeString( sum(case rownum when 8 then Imp else 0 end )) ");
                sbQuery.Append("\n       ,AdTargetsHanaTV.dbo.ufnIntTypeString( sum(case rownum when 9 then Imp else 0 end )) ");
                sbQuery.Append("\n       ,AdTargetsHanaTV.dbo.ufnIntTypeString( sum(case rownum when 10 then Imp else 0 end )) ");
                sbQuery.Append("\n       ,AdTargetsHanaTV.dbo.ufnIntTypeString( sum(case rownum when 11 then Imp else 0 end )) ");
                sbQuery.Append("\n       ,AdTargetsHanaTV.dbo.ufnIntTypeString( sum(case rownum when 12 then Imp else 0 end )) ");
                sbQuery.Append("\n       ,AdTargetsHanaTV.dbo.ufnIntTypeString( sum(case rownum when 13 then Imp else 0 end )) ");
                sbQuery.Append("\n       ,AdTargetsHanaTV.dbo.ufnIntTypeString( sum(case rownum when 14 then Imp else 0 end )) ");
                sbQuery.Append("\n       ,AdTargetsHanaTV.dbo.ufnIntTypeString( sum(case rownum when 15 then Imp else 0 end )) ");
                sbQuery.Append("\n       ,AdTargetsHanaTV.dbo.ufnIntTypeString( sum(case rownum when 16 then Imp else 0 end )) ");
                sbQuery.Append("\n       ,AdTargetsHanaTV.dbo.ufnIntTypeString( sum(case rownum when 17 then Imp else 0 end )) ");
                sbQuery.Append("\n       ,AdTargetsHanaTV.dbo.ufnIntTypeString( sum(case rownum when 18 then Imp else 0 end )) ");
                sbQuery.Append("\n       ,AdTargetsHanaTV.dbo.ufnIntTypeString( sum(case rownum when 19 then Imp else 0 end )) ");
                sbQuery.Append("\n       ,AdTargetsHanaTV.dbo.ufnIntTypeString( sum(case rownum when 20 then Imp else 0 end )) ");
                sbQuery.Append("\n       ,AdTargetsHanaTV.dbo.ufnIntTypeString( sum(case rownum when 21 then Imp else 0 end )) ");
                sbQuery.Append("\n       ,AdTargetsHanaTV.dbo.ufnIntTypeString( sum(case rownum when 22 then Imp else 0 end )) ");
                sbQuery.Append("\n       ,AdTargetsHanaTV.dbo.ufnIntTypeString( sum(case rownum when 23 then Imp else 0 end )) ");
                sbQuery.Append("\n       ,AdTargetsHanaTV.dbo.ufnIntTypeString( sum(case rownum when 24 then Imp else 0 end )) ");
                sbQuery.Append("\n       ,AdTargetsHanaTV.dbo.ufnIntTypeString( sum(case rownum when 25 then Imp else 0 end )) ");
                sbQuery.Append("\n       ,AdTargetsHanaTV.dbo.ufnIntTypeString( sum(case rownum when 26 then Imp else 0 end )) ");
                sbQuery.Append("\n       ,AdTargetsHanaTV.dbo.ufnIntTypeString( sum(case rownum when 27 then Imp else 0 end )) ");
                sbQuery.Append("\n       ,AdTargetsHanaTV.dbo.ufnIntTypeString( sum(case rownum when 28 then Imp else 0 end )) ");
                sbQuery.Append("\n       ,AdTargetsHanaTV.dbo.ufnIntTypeString( sum(case rownum when 29 then Imp else 0 end )) ");
                sbQuery.Append("\n       ,AdTargetsHanaTV.dbo.ufnIntTypeString( sum(case rownum when 30 then Imp else 0 end )) ");
                sbQuery.Append("\n       ,AdTargetsHanaTV.dbo.ufnIntTypeString( sum(case rownum when 31 then Imp else 0 end )) ");
                sbQuery.Append("\n from (   select   x.LogDay ");
                sbQuery.Append("\n                  ,row_number() over( order by x.LogDay) as rownum ");
                sbQuery.Append("\n                  ,isnull( y.TotCnt,0 ) as Imp ");
                sbQuery.Append("\n          from (  select  LogDay ");
                sbQuery.Append("\n                  from    SummaryBase with(noLock) ");
                sbQuery.Append("\n                  where	LogDay between @LogDay1 and @LogDay2 ) x ");
                sbQuery.Append("\n          left outer join ");
                sbQuery.Append("\n               (  select  a.LogDay ");
                sbQuery.Append("\n                         ,sum(Hit00+Hit01+Hit02+Hit03+Hit04+Hit05+Hit06+Hit07+Hit08+Hit09+Hit10+Hit11+Hit12+Hit13+Hit14+Hit15+Hit16+Hit17+Hit18+Hit19+Hit20+Hit21+Hit22+Hit23) as TotCnt ");
                sbQuery.Append("\n                  from	SummaryAdHome a with(noLock) ");
                sbQuery.Append("\n                  inner join AdTargetsHanaTV.dbo.SchHomeCmHistory b with(noLock) on b.ItemNo = a.ItemNo and b.LogDay=a.LogDay  ");
                if (!mediaRep.Equals("00"))
                {
                    sbQuery.Append("\n                         inner join AdTargetsHanaTV.dbo.ContractItem c with(noLock) on c.ItemNo = a.ItemNo and c.RapCode = @RepCode  ");
                }
                sbQuery.Append("\n                  where    a.LogDay between @LogDay1 and @LogDay2 ");
                sbQuery.Append("\n                  group by a.logday ) y on y.LogDay = x.LogDay ");
                sbQuery.Append("\n      ) v ");
                // __DEBUG__
                _log.Debug("<입력정보>");
                _log.Debug("LogDay1	  :[" + beginDay    + "]");
                _log.Debug("LogDay2	  :[" + endDay      + "]");
                _log.Debug("MediaRep  :[" + mediaRep    + "]");
                _log.Debug(sbQuery.ToString());
                // __DEBUG__
				
                // 쿼리실행
                rtnData = new DataSet();
                _db.ExecuteQueryParams(rtnData,sbQuery.ToString(),sqlParams);

                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "GetHomeCmReport() End");
                _log.Debug("-----------------------------------------");
            }
            catch(Exception ex)
            {
                rtnData = null;
                _log.Exception(ex);
            }
            finally
            {
                _db.Close();
            }
            return rtnData.Copy();

        }
        #endregion

	}
}