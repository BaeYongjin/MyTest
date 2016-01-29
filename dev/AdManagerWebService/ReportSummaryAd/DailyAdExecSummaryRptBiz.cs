// ===============================================================================
//
// DailyAdExecSummaryRptBiz.cs
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
 * Class Name: DailyAdExecSummaryRptBiz
 * 주요기능  : 광고 총괄집계 로직
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

namespace AdManagerWebService.ReportSummaryAd
{
	/// <summary>
	/// DailyAdExecSummaryRptBiz에 대한 요약 설명입니다.
	/// </summary>
	public class DailyAdExecSummaryRptBiz : BaseBiz
	{

		#region  생성자
		public DailyAdExecSummaryRptBiz() : base(FrameSystem.connSummaryDbString)
		{
			_log = FrameSystem.oLog;
		}
		#endregion

		#region 일간 광고집행 종합 보고서
		/// <summary>
		///  총기간 광고집행 집계
		/// </summary>
		/// <param name="dailyAdExecSummaryRptModel"></param>
		public void GetDailyAdExecSummary(HeaderModel header, DailyAdExecSummaryRptModel dailyAdExecSummaryRptModel)
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
				if(dailyAdExecSummaryRptModel.LogDay1.Length > 6) dailyAdExecSummaryRptModel.LogDay1 = dailyAdExecSummaryRptModel.LogDay1.Substring(2,6);
//				if(dailyAdExecSummaryRptModel.LogDay2.Length > 6) dailyAdExecSummaryRptModel.LogDay2   = dailyAdExecSummaryRptModel.LogDay2.Substring(2,6);
//				if(dailyAdExecSummaryRptModel.WeekDay.Length > 6) dailyAdExecSummaryRptModel.WeekDay   = dailyAdExecSummaryRptModel.WeekDay.Substring(2,6);
//				if(dailyAdExecSummaryRptModel.MonthDay.Length > 6) dailyAdExecSummaryRptModel.MonthDay   = dailyAdExecSummaryRptModel.MonthDay.Substring(2,6);
				
				// __DEBUG__
				_log.Debug("<입력정보>");
				_log.Debug("LogDay1	  :[" + dailyAdExecSummaryRptModel.LogDay1   + "]");		// 검색 매체
//				_log.Debug("SearchContractSeq :[" + dailyAdExecSummaryRptModel.SearchContractSeq + "]");		// 검색 광고번호           
//				_log.Debug("SearchItemNo      :[" + dailyAdExecSummaryRptModel.SearchItemNo      + "]");		// 검색 광고번호           
//				_log.Debug("SearchStartDay    :[" + dailyAdExecSummaryRptModel.SearchStartDay    + "]");		// 검색 집계시작 일자          
//				_log.Debug("SearchEndDay      :[" + dailyAdExecSummaryRptModel.SearchEndDay      + "]");		// 검색 집계종료 일자          
				// __DEBUG__

				string logDay1   = dailyAdExecSummaryRptModel.LogDay1;
//				string logDay2   = dailyAdExecSummaryRptModel.LogDay2;
//				string weekDay   = dailyAdExecSummaryRptModel.WeekDay;
//				string monthDay  = dailyAdExecSummaryRptModel.MonthDay;

				SqlParameter[] sqlParams = new SqlParameter[1];
				sqlParams[0] = new SqlParameter("@LogDay", SqlDbType.VarChar, 6);
				sqlParams[0].Value = dailyAdExecSummaryRptModel.LogDay1;
								
				// 쿼리생성
				sbQuery = new StringBuilder();
				sbQuery.Append("\n"
					+ "/* 총기간 광고집행 집계																				\n"
					+ "   조회조건 : 매체코드, 계약번호 또는 광고번호 */														\n"
					+ "																									\n"
					+ "DECLARE @LogDay1 varchar(6);   -- 매체코드															\n"
					+ "DECLARE @LogDay2 varchar(6);  -- 집행일자															\n"
					+ "DECLARE @WeekDay varchar(6);      -- 광고번호										 				\n"
					+ "DECLARE @MonthDay varchar(6); -- 계약번호															\n"
					+ "DECLARE @totalAdCnt int;																			\n"
					+ "DECLARE @totalHouse int;																			\n"
					+ "DECLARE @actHouse   int;															 				\n"
										
					+ "SET @LogDay1   = '" + logDay1  + "'\n"
					+ "SET @LogDay2    = convert(varchar(6),dateadd( week,  -1, cast('" + logDay1  + "' as datetime)  ) ,12)		\n"
					+ "SET @WeekDay    = convert(varchar(6),dateadd( week,  -1, cast('" + logDay1  + "' as datetime)+1) ,12)      \n"
					+ "SET @MonthDay   = convert(varchar(6),dateadd( month, -1, cast('" + logDay1  + "' as datetime) + 1) ,12)    \n"
				
					+ "-- 당일																									\n"
					+ "select '1.당일' as [일자구분]                                                                    \n"
					+ "       ,HouseTotal as [총가구수]                                                                                                  \n"
					+ "       ,HouseHold  as [활성사용자]                                                               \n"
					+ "       ,cast(HouseHold as real) / HouseTotal * 100  as [사용률]                                 \n"
					+ "       ,ChannelHit as [채널Hit]                                                                 \n"
					+ "       ,ChannelAdHit as [광고Hit]                                                               \n"
					+ "       ,cast(ChannelAdHit as real) /  ChannelHit   as [광고빈도]                                 \n"
					+ "       from  dbo.SummaryBase a with(noLock)                                                    \n"
					+ "		  Where   LogDay = @LogDay1																  \n"
					+ "																	                              \n"
					+ "UNION ALL                                 \n"
					+ "                                          \n"
					+ "-- 전주	                                 \n"
					+ "select '2.전주'		                     \n"
					+ "       ,HouseTotal		                 \n"
					+ "       ,HouseHold	                     \n"
					+ "       ,cast(HouseHold as real) /HouseTotal * 100   as RateActiveHouse						  \n"
					+ "       ,ChannelHit																			  \n"
					+ "       ,ChannelAdHit																			  \n"
					+ "       ,cast(ChannelAdHit as real) /  ChannelHit     as HitAd								  \n"
					+ "       from  dbo.SummaryBase a with(noLock)													  \n"
					+ "       Where LogDay = @LogDay2																  \n"										
					+ "                                                                                               \n"
					+ "UNION ALL                                                                                      \n"
					+ "                                                                                               \n"
					+ "-- 주간평균																					  \n"
					+ "select '3.주간평균'																			  \n"
					+ "       ,avg(HouseTotal)																		  \n"
					+ "       ,avg(HouseHold)																		  \n"
					+ "       ,cast(avg(HouseHold) as real) /avg(HouseTotal) * 100   as RateActiveHouse				  \n"
					+ "       ,avg(ChannelHit)																		  \n"
					+ "       ,avg(ChannelAdHit)																	  \n"
					+ "       ,cast(avg(ChannelAdHit) as real) /  avg(ChannelHit)     as HitAd						  \n"					
					+ "  from  dbo.SummaryBase a with(noLock)														  \n"
					+ "  Where LogDay between @WeekDay and @LogDay1													  \n"					
					+ "																								  \n"
					+ "UNION ALL																					  \n"
					+ "																								  \n"
					+ "-- 초수별 집계																					  \n"
					+ "select '4.월간평균'																			  \n"
					+ "       ,avg(HouseTotal)	  \n"
					+ "       ,avg(HouseHold)   \n"
					+ "       ,cast(avg(HouseHold) as real) /avg(HouseTotal) * 100   as RateActiveHouse				  \n"
					+ "       ,avg(ChannelHit)																		  \n"
					+ "       ,avg(ChannelAdHit)																	  \n"
					+ "       ,cast(avg(ChannelAdHit) as real) /  avg(ChannelHit)     as HitAd						  \n"					
					+ "  from  dbo.SummaryBase a with(noLock)														  \n"
					+ "  Where LogDay between @MonthDay and @LogDay1												  \n"						
					);

				// __DEBUG__
				_log.Debug(sbQuery.ToString());
				// __DEBUG__
				
				// 쿼리실행
				DataSet ds = new DataSet();
				_db.ExecuteQuery(ds,sbQuery.ToString());

				// 결과 DataSet의 데이터모델에 복사
				dailyAdExecSummaryRptModel.ReportDataSet = ds.Copy();

				// 결과
				dailyAdExecSummaryRptModel.ResultCnt = Utility.GetDatasetCount(dailyAdExecSummaryRptModel.ReportDataSet);

				ds.Dispose();

				// 쿼리생성
				sbQuery = new StringBuilder();				
				sbQuery.Append(""					
					+ "DECLARE @LogDay1 varchar(6);																		\n"
					+ "DECLARE @totalAdCnt int;																			\n"
					+ "DECLARE @totalHouse int;																			\n"
					+ "DECLARE @actHouse   int;															 				\n"
					+ "																									\n"
					+ "SET @LogDay1   = '" + logDay1  + "'																\n"
					+ "-- 요일별 집계						                                                         \n"
					+ "SELECT  @totalAdCnt = ChannelAdHit                                                        \n"
					+ "       ,@totalHouse = HouseTotal                                                          \n"
					+ "       ,@actHouse = HouseHold                                                             \n"					
					+ "  FROM dbo.SummaryBase a with(noLock)	                                                 \n"
					+ "  Where   LogDay = @LogDay1															     \n"					
					+ "																							 \n"	
					+ "-- 연령별 집계                                                                                \n"
                    + "SELECT  '[' + cast(hou.TypeCode as varchar(2)) + '] ' + (select CodeName from AdTargetsHanaTV.dbo.SystemCode sy where sy.Section = hou.TypeSection and sy.Code = hou.TypeCode)      as [광고구분]                                                                        \n"
					+ "       , itm.AdCnt         as [노출물량]													   \n"
					+ "       , cast(itm.AdCnt as real) / @totalAdCnt * 100 as [점유률]							   \n"
					+ "       , hou.DayUsers  as [가구수]															   \n"
					+ "       , cast(hou.DayUsers as real) / @totalHouse * 100 as [도달률]						   \n"
					+ "       , cast(itm.AdCnt  as real) / hou.DayUsers  as [노출빈도]							   \n"
					+ "       , cast(hou.DayUsers as real) / @actHouse * 100 as [가구점유률]						   \n"
					+ " from (			                                                                           \n"
					+ "       SELECT  *																			   \n"
					+ "       FROM    SummaryHouseHoldType a with(noLock)                 \n"
					+ "       Where   LogDay = @LogDay1                                                            \n"
					+ "       and     TypeSection = 26 ) hou                                                       \n"
					+ "   inner join				                                                               \n"
					+ "     (  select  b.AdType																       \n"
					+ "				,sum(a.AdCnt) as AdCnt		                                                   \n"
					+ "       from  SummaryAdDaily0 a with(noLock)												   \n"
                    + " 	  inner join AdTargetsHanaTV.dbo.ContractItem b with(noLock) on a.ItemNo = b.ItemNo and b.AdType < '90'          \n"					
					+ " 	  Where a.LogDay = @LogDay1                                                                  \n"
					+ " 	    and   a.ItemNo  > 0                                                                      \n"
					+ " 	    and   a.ContractSeq = 0                                                                  \n"
					+ " 	    and   a.SummaryType = 1                                                                  \n"
					+ " 	    and   a.SummaryCode = 1                                                                  \n"
					+ " 		Group by b.AdType ) itm on hou.TypeCode = itm.AdType                                     \n"
					+ "union all                                                                                         \n"
					+ "select  '[99] 전체'																				 \n"
					+ "        ,@totalAdCnt																				 \n"
					+ "        ,0																					     \n"
					+ "        ,@actHouse																				 \n"
					+ "        ,0																						 \n"
					+ "        ,cast(@totalAdCnt  as real) / @actHouse													 \n"
					+ "        ,0																						 \n"					
					);
				
				// __DEBUG__
				_log.Debug(sbQuery.ToString());
				// __DEBUG__
				
				// 쿼리실행
				ds = new DataSet();
				_db.ExecuteQuery(ds,sbQuery.ToString());

				// 결과 DataSet의 데이터모델에 복사
				dailyAdExecSummaryRptModel.ItemDataSet = ds.Copy();

				// 결과
				dailyAdExecSummaryRptModel.ResultCnt = Utility.GetDatasetCount(dailyAdExecSummaryRptModel.ItemDataSet);

				ds.Dispose();

				// 결과코드 셋트
				dailyAdExecSummaryRptModel.ResultCD = "0000";

				// __DEBUG__
				_log.Debug("<출력정보>");
				_log.Debug("ResultCnt:[" + dailyAdExecSummaryRptModel.ResultCnt + "]");
				// __DEBUG__

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetDailyAdExecSummary() End");
				_log.Debug("-----------------------------------------");
			}
			catch(Exception ex)
			{
				dailyAdExecSummaryRptModel.ResultCD = "3000";
				if(isNotReady)
				{
					dailyAdExecSummaryRptModel.ResultDesc = "해당 기간은 데이터가 집계되지 않았습니다.";
				}
				else
				{
					dailyAdExecSummaryRptModel.ResultDesc = "총기간 광고집행 집계 조회중 오류가 발생하였습니다";
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