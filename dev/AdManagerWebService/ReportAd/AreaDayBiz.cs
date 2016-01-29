// ===============================================================================
//
// AreaDayBiz.cs
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
 * Class Name: AreaDayBiz
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
	/// AreaDayBiz에 대한 요약 설명입니다.
	/// </summary>
	public class AreaDayBiz : BaseBiz
	{
		#region  생성자
		public AreaDayBiz() : base(FrameSystem.connSummaryDbString)
		{
			_log = FrameSystem.oLog;
		}
		#endregion

		#region [함수] 지역-요일별 집계
		/// <summary>
		/// 지역-요일별 집계
		/// </summary>
		/// <param name="header"></param>
		/// <param name="data"></param>
		public void GetAreaDay(HeaderModel header, AreaDayModel data)
		{
            try
			{
				StringBuilder sbQuery = null;
				_db.Open();

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetAreaDay() Start");
				_log.Debug("-----------------------------------------");

				// 일자가 6자리 이상이면 6자리로 만든다.
				if(data.StartDay.Length > 6)    data.StartDay = data.StartDay.Substring(2,6);
				if(data.EndDay.Length > 6)      data.EndDay = data.EndDay.Substring(2,6);
				
				string sqlItems = "";
				if( data.AdList.Count > 0 )
				{
					for( int i = 0; i < data.AdList.Count;i++)
					{
						if( i == 0 )    sqlItems = data.AdList[i].ToString();
						else            sqlItems += "," + data.AdList[i].ToString();
					}
				}

				// __DEBUG__
				_log.Debug("<입력정보>");
				_log.Debug("StartDay :[" + data.StartDay    + "]");
				_log.Debug("EndDay   :[" + data.EndDay      + "]");
				_log.Debug("Items    :[" + sqlItems         + "]");
				// __DEBUG__
                
				// 쿼리생성
				sbQuery = new StringBuilder();
				sbQuery.Append("\n");
				sbQuery.Append(" SELECT v1.region         -- 광고번호 " + "\n");
				sbQuery.Append("       ,( select summaryName from SummaryCode nolock where SummaryType = 5 and SummaryCode = v1.region) as RegionName " + "\n");
				sbQuery.Append("       ,Sum( case v1.week when 2 then Cnt else 0 end)  as Mon " + "\n");
				sbQuery.Append("       ,sum( case v1.week when 3 then Cnt else 0 end)  as Tue " + "\n");
				sbQuery.Append("       ,sum( case v1.week when 4 then Cnt else 0 end)  as Wed " + "\n");
				sbQuery.Append("       ,sum( case v1.week when 5 then Cnt else 0 end)  as Thu " + "\n");
				sbQuery.Append("       ,sum( case v1.week when 6 then Cnt else 0 end)  as Fri " + "\n");
				sbQuery.Append("       ,sum( case v1.week when 7 then Cnt else 0 end)  as Sat " + "\n");
				sbQuery.Append("       ,sum( case v1.week when 1 then Cnt else 0 end)  as Sun " + "\n");
				sbQuery.Append(" from ( " + "\n");
				sbQuery.Append("       SELECT b.week " + "\n");
				sbQuery.Append(" 			 ,case c.Level	when 1 then c.RegionCode else c.UpperCode end  as region " + "\n");
				sbQuery.Append(" 			 ,sum(a.AdCnt) as Cnt " + "\n");
				sbQuery.Append("       FROM  dbo.SummaryAdDaily0 a with(NoLock) " + "\n");
				sbQuery.Append("       inner join dbo.SummaryBase b with(NoLock) on a.LogDay = b.LogDay " + "\n");
                sbQuery.Append("       inner join AdTargetsHanaTV.dbo.TargetRegion c with(NoLock) on a.SummaryCode = c.RegionCode " + "\n");
				sbQuery.Append(" 	   where a.LogDay between '" + data.StartDay + "' and '" + data.EndDay + "' " + "\n");
				sbQuery.Append("       and   a.ItemNo in(" + sqlItems + ") " + "\n");
				sbQuery.Append("       and   a.ContractSeq = 0 " + "\n");
				sbQuery.Append("       and   a.SummaryType = 5 " + "\n");
				sbQuery.Append("       group by b.week, case c.Level	when 1 then c.RegionCode else c.UpperCode end ) v1 " + "\n");
				sbQuery.Append(" group by v1.region " + "\n");
			
				// __DEBUG__
				_log.Debug(sbQuery.ToString());
				// __DEBUG__
				
				// 쿼리실행
				DataSet ds = new DataSet();
				_db.ExecuteQuery(ds, sbQuery.ToString());

				// 결과 DataSet의 데이터모델에 복사
				data.ReportDataSet = ds.Copy();

				// 결과
				data.ResultCnt = Utility.GetDatasetCount(data.ReportDataSet);

				ds.Dispose();


				// 결과코드 셋트
				data.ResultCD = "0000";

				// __DEBUG__
				_log.Debug("<출력정보>");
				_log.Debug("ResultCnt:[" + data.ResultCnt + "]");
				// __DEBUG__

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetAreaDay() End");
				_log.Debug("-----------------------------------------");
			}
			catch(Exception ex)
			{
				data.ResultCD = "3000";
				data.ResultDesc = "지역-요일별 집계 조회중 오류가 발생하였습니다";
				_log.Exception(ex);
			}
			finally
			{
				// 데이터베이스를  Close한다
				_db.Close();
			}

		}
		#endregion

		#region [함수] 지역-요일별 집계(자바용)
		/// <summary>
		/// 지역-요일별 집계
		/// </summary>
		/// <param name="header"></param>
		/// <param name="data"></param>
		public void GetAreaDayByJava(HeaderModel header, AreaDayModel data)
		{
			try
			{
				StringBuilder sbQuery = null;
				_db.Open();

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetAreaDay() Start");
				_log.Debug("-----------------------------------------");

				// 일자가 6자리 이상이면 6자리로 만든다.
				if(data.StartDay.Length > 6)    data.StartDay = data.StartDay.Substring(2,6);
				if(data.EndDay.Length > 6)      data.EndDay = data.EndDay.Substring(2,6);
				
				string sqlItems = "";
				if( data.AdListStr.Length > 0 )
				{
					for( int i = 0; i < data.AdListStr.Length;i++)
					{
						if( i == 0 )    sqlItems = data.AdListStr[i];
						else            sqlItems += "," + data.AdListStr[i];
					}
				}

				// __DEBUG__
				_log.Debug("<입력정보>");
				_log.Debug("StartDay :[" + data.StartDay    + "]");
				_log.Debug("EndDay   :[" + data.EndDay      + "]");
				_log.Debug("Items    :[" + sqlItems         + "]");
				// __DEBUG__
                
				// 쿼리생성
				sbQuery = new StringBuilder();
				sbQuery.Append("\n");
				sbQuery.Append(" SELECT v1.region         -- 광고번호 " + "\n");
				sbQuery.Append("       ,( select summaryName from SummaryCode nolock where SummaryType = 5 and SummaryCode = v1.region) as RegionName " + "\n");
				sbQuery.Append("       ,Sum( case v1.week when 2 then Cnt else 0 end)  as Mon " + "\n");
				sbQuery.Append("       ,sum( case v1.week when 3 then Cnt else 0 end)  as Tue " + "\n");
				sbQuery.Append("       ,sum( case v1.week when 4 then Cnt else 0 end)  as Wed " + "\n");
				sbQuery.Append("       ,sum( case v1.week when 5 then Cnt else 0 end)  as Thu " + "\n");
				sbQuery.Append("       ,sum( case v1.week when 6 then Cnt else 0 end)  as Fri " + "\n");
				sbQuery.Append("       ,sum( case v1.week when 7 then Cnt else 0 end)  as Sat " + "\n");
				sbQuery.Append("       ,sum( case v1.week when 1 then Cnt else 0 end)  as Sun " + "\n");
				sbQuery.Append(" from ( " + "\n");
				sbQuery.Append("       SELECT b.week " + "\n");
				sbQuery.Append(" 			 ,case c.UpperCode when 0 then c.SummaryCode else c.UpperCode end  as region " + "\n");
				sbQuery.Append(" 			 ,sum(a.AdCnt) as Cnt " + "\n");
				sbQuery.Append("       FROM  dbo.SummaryAdDaily0 a with(NoLock) " + "\n");
				sbQuery.Append("       inner join dbo.SummaryBase b with(NoLock) on a.LogDay = b.LogDay " + "\n");
				sbQuery.Append("       inner join dbo.SummaryCode c with(NoLock) on a.Summarytype = c.SummaryType and a.SummaryCode = c.SummaryCode " + "\n");
				sbQuery.Append(" 	   where a.LogDay between '" + data.StartDay + "' and '" + data.EndDay + "' " + "\n");
				sbQuery.Append("       and   a.ItemNo in(" + sqlItems + ") " + "\n");
				sbQuery.Append("       and   a.ContractSeq = 0 " + "\n");
				sbQuery.Append("       and   a.SummaryType = 5 " + "\n");
				sbQuery.Append("       group by b.week, case c.UpperCode when 0 then c.SummaryCode else c.UpperCode end ) v1 " + "\n");
				sbQuery.Append(" group by v1.region " + "\n");
			
				// __DEBUG__
				_log.Debug(sbQuery.ToString());
				// __DEBUG__
				
				// 쿼리실행
				DataSet ds = new DataSet();
				_db.ExecuteQuery(ds, sbQuery.ToString());

				// 결과 DataSet의 데이터모델에 복사
				data.ReportDataSet = ds.Copy();

				// 결과
				data.ResultCnt = Utility.GetDatasetCount(data.ReportDataSet);

				ds.Dispose();


				// 결과코드 셋트
				data.ResultCD = "0000";

				// __DEBUG__
				_log.Debug("<출력정보>");
				_log.Debug("ResultCnt:[" + data.ResultCnt + "]");
				// __DEBUG__

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetAreaDay() End");
				_log.Debug("-----------------------------------------");
			}
			catch(Exception ex)
			{
				data.ResultCD = "3000";
				data.ResultDesc = "지역-요일별 집계 조회중 오류가 발생하였습니다";
				_log.Exception(ex);
			}
			finally
			{
				// 데이터베이스를  Close한다
				_db.Close();
			}

		}
		#endregion


		#region 시간-요일별 집계(자바용)
		/// <summary>
		/// 광고별 시청현황 집계
		/// </summary>
		/// <param name="areaDayModel"></param>
		public void GetTimeDayByJava(HeaderModel header, AreaDayModel data)
		{

			try
			{
				StringBuilder sbQuery = null;

				// 데이터베이스를 OPEN한다
				_db.Open();


				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetTimeDay() Start");
				_log.Debug("-----------------------------------------");

				// 일자가 6자리 이상이면 6자리로 만든다.
				if(data.StartDay.Length > 6)    data.StartDay = data.StartDay.Substring(2,6);
				if(data.EndDay.Length > 6)      data.EndDay = data.EndDay.Substring(2,6);
				
				string sqlItems = "";
				if( data.AdListStr.Length > 0 )
				{
					for( int i = 0; i < data.AdListStr.Length;i++)
					{
						if( i == 0 )    sqlItems = data.AdListStr[i];
						else            sqlItems += "," + data.AdListStr[i];
					}
				}

				// __DEBUG__
				_log.Debug("<입력정보>");
				_log.Debug("StartDay :[" + data.StartDay    + "]");
				_log.Debug("EndDay   :[" + data.EndDay      + "]");
				_log.Debug("Items    :[" + sqlItems         + "]");
				// __DEBUG__


				// 쿼리생성
				sbQuery = new StringBuilder();
				sbQuery.Append("\n"
					+ "SELECT v2.Hour         -- 광고번호								      								\n"   
					+ "      ,Sum( case v2.week when 2 then Cnt else 0 end)  as Mon 									\n"
					+ "      ,sum( case v2.week when 3 then Cnt else 0 end)  as Tue										\n"		
					+ "      ,sum( case v2.week when 4 then Cnt else 0 end)  as Wed										\n" 					
					+ "      ,sum( case v2.week when 5 then Cnt else 0 end)  as Thu									    \n"
					+ "      ,sum( case v2.week when 6 then Cnt else 0 end)  as Fri										\n"
					+ "      ,sum( case v2.week when 7 then Cnt else 0 end)  as Sat										\n"
					+ "      ,sum( case v2.week when 1 then Cnt else 0 end)  as Sun										\n"					
					+ "from (																							\n"
					+ "      SELECT v1.week																				\n"
					+ "			,case t.RowNum																			\n"
					+ "			when 1  then 0    when 2  then 1    when 3    then 2    when 4  then 3						\n"
					+ "			when 5  then 4    when 6  then 5    when 7    then 6    when 8  then 7						\n"
					+ "			when 9  then 8    when 10 then 9    when 11   then 10   when 12 then 11						\n"
					+ "			when 13  then 12  when 14  then 13  when 15  then 14    when 16  then 15					\n"
					+ "			when 17  then 16  when 18  then 17  when 19  then 18    when 20  then 19					\n"
					+ "			when 21  then 20  when 22  then 21  when 23  then 22    when 24  then 23					\n"
					+ "			end  AS Hour																				\n" 
					+ "			,case t.RowNum																				\n"
					+ "			when 1   then H00  when 2   then H01 when 3  then H02 when 4  then H03 when 5  then H04 when 6  then H05  when 7  then H06 when 8  then H07       \n"
					+ "			when 9   then H08  when 10  then H09 when 11 then H10 when 12 then H11 when 13 then H12 when 14 then H13  when 15 then H14 when 16  then H15      \n"
					+ "			when 17  then H16  when 18  then H17 when 19 then H18 when 20 then H19 when 21 then H20 when 22 then H21  when 23 then H22 when 24  then H23      \n"
					+ "			end  As Cnt      \n"
					+ "   FROM (																					    \n"
					+ "      SELECT b.week																		        \n"
					+ "      ,SUM([H00]) AS  H00 ,SUM([H01]) AS  H01 ,SUM([H02]) AS  H02 ,SUM([H03]) AS  H03			\n"
					+ "      ,SUM([H04]) AS  H04 ,SUM([H05]) AS  H05 ,SUM([H06]) AS  H06 ,SUM([H07]) AS  H07			\n"
					+ "      ,SUM([H08]) AS  H08 ,SUM([H09]) AS  H09 ,SUM([H10]) AS  H10 ,SUM([H11]) AS  H11			\n"
					+ "      ,SUM([H12]) AS  H12 ,SUM([H13]) AS  H13 ,SUM([H14]) AS  H14 ,SUM([H15]) AS  H15			\n"
					+ "      ,SUM([H16]) AS  H16 ,SUM([H17]) AS  H17 ,SUM([H18]) AS  H18 ,SUM([H19]) AS  H19			\n"
					+ "      ,SUM([H20]) AS  H20 ,SUM([H21]) AS  H21 ,SUM([H22]) AS  H22 ,SUM([H23]) AS  H23			\n"
					+ "     FROM dbo.SummaryAdDaily0 a with(NoLock)													    \n"
					+ "      inner join dbo.SummaryBase b with(NoLock) on a.LogDay = b.LogDay					        \n"
					+ "      where a.LogDay between '" + data.StartDay + "' and '" + data.EndDay + "' " + "\n"
					+ "      and   a.ItemNo in(" + sqlItems + ") " + "\n"
					+ "      and   a.ContractSeq = 0																	\n" 
					+ "      and   a.SummaryType = 1																	\n"
                    + "      group by b.week ) V1 INNER JOIN AdTargetsHanaTV.dbo.COPY_T T ON T.ROWNUM <= 24 ) V2   \n"
					+ "GROUP BY v2.Hour																					\n"					
					);

			
				// __DEBUG__
				_log.Debug(sbQuery.ToString());
				// __DEBUG__
				
				// 쿼리실행
				DataSet ds = new DataSet();
				_db.ExecuteQuery(ds,sbQuery.ToString());

				// 결과 DataSet의 데이터모델에 복사
				data.ReportDataSet = ds.Copy();

				// 결과
				data.ResultCnt = Utility.GetDatasetCount(data.ReportDataSet);

				ds.Dispose();


				// 결과코드 셋트
				data.ResultCD = "0000";

				// __DEBUG__
				_log.Debug("<출력정보>");
				_log.Debug("ResultCnt:[" + data.ResultCnt + "]");
				// __DEBUG__

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetTimeDay() End");
				_log.Debug("-----------------------------------------");
			}
			catch(Exception ex)
			{
				data.ResultCD = "3000";
				data.ResultDesc = "시간-요일별 집계 조회중 오류가 발생하였습니다";
				_log.Exception(ex);
			}
			finally
			{
				// 데이터베이스를  Close한다
				_db.Close();
			}

		}
		#endregion

		#region 장르-요일별 집계
		/// <summary>
		/// 광고별 시청현황 집계
		/// </summary>
		/// <param name="areaDayModel"></param>
		public void GetGenreDayByJava(HeaderModel header, AreaDayModel data)
		{

			try
			{
				StringBuilder sbQuery = null;

				// 데이터베이스를 OPEN한다
				_db.Open();


				_log.Debug("-----------------------------------------");
				_log.Debug("-----------------------------------------");
				_log.Debug("-----------------------------------------");
				_log.Debug("-----------------------------------------");
				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetGenreDay() Start");
				_log.Debug("-----------------------------------------");

				// 일자가 6자리 이상이면 6자리로 만든다.
				if(data.StartDay.Length > 6)    data.StartDay = data.StartDay.Substring(2,6);
				if(data.EndDay.Length > 6)      data.EndDay = data.EndDay.Substring(2,6);
				
				string sqlItems = "";
				if( data.AdListStr.Length > 0 )
				{
					for( int i = 0; i < data.AdListStr.Length;i++)
					{
						if( i == 0 )    sqlItems = data.AdListStr[i];
						else            sqlItems += "," + data.AdListStr[i];
					}
				}

				// __DEBUG__
				_log.Debug("<입력정보>");
				_log.Debug("StartDay :[" + data.StartDay    + "]");
				_log.Debug("EndDay   :[" + data.EndDay      + "]");
				_log.Debug("Items    :[" + sqlItems         + "]");
				// __DEBUG__

				// 쿼리생성
				sbQuery = new StringBuilder();
				sbQuery.Append("\n"
                    + "SELECT v1.Category, (select CategoryName  from AdTargetsHanaTV.dbo.Category noLock where CategoryCode = v1.Category) as CategoryNm         -- 광고번호								      								\n"
                    + "      ,v1.Genre,    (select GenreName     from AdTargetsHanaTV.dbo.Genre    noLock where GenreCode    = v1.Genre)    as GenreNm 									\n"
					+ "      ,Sum( case v1.week when 2 then Cnt else 0 end)  as Mon										\n"		
					+ "      ,sum( case v1.week when 3 then Cnt else 0 end)  as Tue										\n" 					
					+ "      ,sum( case v1.week when 4 then Cnt else 0 end)  as Wed									    \n"
					+ "      ,sum( case v1.week when 5 then Cnt else 0 end)  as Thu										\n"
					+ "      ,sum( case v1.week when 6 then Cnt else 0 end)  as Fri										\n"
					+ "      ,sum( case v1.week when 7 then Cnt else 0 end)  as Sat										\n"					
					+ "      ,sum( case v1.week when 1 then Cnt else 0 end)  as Sun										\n"					
					+ "from (																							\n"
					+ "         select   b.week																				\n"
					+ "			        ,a.Category																				\n"
					+ "			        ,a.Genre																				\n"
					+ "			        ,sum(a.AdCnt) as Cnt																	\n"
					+ "			from    dbo.SummaryAdDaily3 a with(NoLock)													\n"
					+ "			inner   join dbo.SummaryBase b with(NoLock) on a.LogDay = b.LogDay						\n"
					+ "         where   a.LogDay between '" + data.StartDay + "' and '" + data.EndDay + "' " + "\n"
					+ "         and     a.ItemNo in(" + sqlItems + ") " + "\n"
					+ "			and     a.ContractSeq = 0																	\n" 
					+ "			and     a.progKey > 0																		\n"
					+ "			group by b.week,a.Category,a.Genre ) v1													\n"					
					+ "group by v1.Category, v1.Genre																	\n"					
					+ "order by v1.Category, v1.Genre																	\n"					
					);

			
				// __DEBUG__
				_log.Debug(sbQuery.ToString());
				// __DEBUG__
				
				// 쿼리실행
				DataSet ds = new DataSet();
				_db.ExecuteQuery(ds,sbQuery.ToString());

				// 결과 DataSet의 데이터모델에 복사
				data.ReportDataSet = ds.Copy();

				// 결과
				data.ResultCnt = Utility.GetDatasetCount(data.ReportDataSet);

				ds.Dispose();


				// 결과코드 셋트
				data.ResultCD = "0000";

				// __DEBUG__
				_log.Debug("<출력정보>");
				_log.Debug("ResultCnt:[" + data.ResultCnt + "]");
				// __DEBUG__

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetGenreDay() End");
				_log.Debug("-----------------------------------------");
			}
			catch(Exception ex)
			{
				data.ResultCD = "3000";
				data.ResultDesc = "장르별 요일집계 조회중 오류가 발생하였습니다";
				_log.Exception(ex);
			}
			finally
			{
				// 데이터베이스를  Close한다
				_db.Close();
			}

		}
		#endregion

		#region 채널별 요일집계
		/// <summary>
		/// 광고별 시청현황 집계
		/// </summary>
		/// <param name="areaDayModel"></param>
		public void GetChannelDayByJava(HeaderModel header, AreaDayModel data)
		{

			try
			{
				StringBuilder sbQuery = null;

				// 데이터베이스를 OPEN한다
				_db.Open();


				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetChannelDay() Start");
				_log.Debug("-----------------------------------------");

				// 일자가 6자리 이상이면 6자리로 만든다.
				if(data.StartDay.Length > 6)    data.StartDay = data.StartDay.Substring(2,6);
				if(data.EndDay.Length > 6)      data.EndDay = data.EndDay.Substring(2,6);
				
				string sqlItems = "";
				if( data.AdListStr.Length > 0 )
				{
					for( int i = 0; i < data.AdListStr.Length;i++)
					{
						if( i == 0 )    sqlItems = data.AdListStr[i];
						else            sqlItems += "," + data.AdListStr[i];
					}
				}

				// __DEBUG__
				_log.Debug("<입력정보>");
				_log.Debug("StartDay :[" + data.StartDay    + "]");
				_log.Debug("EndDay   :[" + data.EndDay      + "]");
				_log.Debug("Items    :[" + sqlItems         + "]");
				// __DEBUG__

				// 쿼리생성
				sbQuery = new StringBuilder();
				sbQuery.Append("\n"
					+ "-- 광고별 시청현황        \n"
                    + "select v1.Category, (select CategoryName  from AdTargetsHanaTV.dbo.Category where CategoryCode = v1.Category) as CategoryNm         -- 광고번호								      								\n"
                    + "      ,v1.Genre,    (select GenreName     from AdTargetsHanaTV.dbo.Genre    where GenreCode = v1.Genre)       as GenreNm 			\n"
                    + "      ,v1.progKey,  (select ProgramNm     from AdTargetsHanaTV.dbo.Program  where Programkey= v1.Progkey)       as ChannelNm			\n"		
					+ "      ,Sum( case v1.week when 2 then Cnt else 0 end)  as Mon			\n" 					
					+ "      ,sum( case v1.week when 3 then Cnt else 0 end)  as Tue		    \n"
					+ "      ,sum( case v1.week when 4 then Cnt else 0 end)  as Wed			\n"
					+ "      ,sum( case v1.week when 5 then Cnt else 0 end)  as Thu			\n"
					+ "      ,sum( case v1.week when 6 then Cnt else 0 end)  as Fri			\n"					
					+ "      ,sum( case v1.week when 7 then Cnt else 0 end)  as Sat			\n"					
					+ "      ,sum( case v1.week when 1 then Cnt else 0 end)  as Sun			\n"					
					+ "from (																							\n"
					+ "      SELECT b.week																				\n"
					+ "			,a.Category															\n"
					+ "			,a.Genre															\n"
					+ "			,a.Progkey															\n"
					+ "			,sum(a.AdCnt) as Cnt												\n"
					+ "			FROM dbo.SummaryAdDaily3 a with(NoLock)								\n"
					+ "			inner join dbo.SummaryBase b with(NoLock) on a.LogDay = b.LogDay												\n"
					+ "      where a.LogDay between '" + data.StartDay + "' and '" + data.EndDay + "' " + "\n"
					+ "      and   a.ItemNo in(" + sqlItems + ") " + "\n"
					+ "			and   a.ContractSeq = 0																	\n" 
					+ "			and   a.progKey > 0																		\n"
					+ "			group by b.week,a.Category,a.Genre,a.ProgKey ) v1										\n"										
					+ "group by v1.Category, v1.Genre,v1.ProgKey																	\n"					
					+ "order by v1.Category, v1.Genre,v1.ProgKey																	\n"					
					);

			
				// __DEBUG__
				_log.Debug(sbQuery.ToString());
				// __DEBUG__
				
				// 쿼리실행
				DataSet ds = new DataSet();
				_db.ExecuteQuery(ds,sbQuery.ToString());

				// 결과 DataSet의 데이터모델에 복사
				data.ReportDataSet = ds.Copy();

				// 결과
				data.ResultCnt = Utility.GetDatasetCount(data.ReportDataSet);

				ds.Dispose();


				// 결과코드 셋트
				data.ResultCD = "0000";

				// __DEBUG__
				_log.Debug("<출력정보>");
				_log.Debug("ResultCnt:[" + data.ResultCnt + "]");
				// __DEBUG__

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetChannelDay() End");
				_log.Debug("-----------------------------------------");
			}
			catch(Exception ex)
			{
				data.ResultCD = "3000";
				data.ResultDesc = "채널별 요일집계 조회중 오류가 발생하였습니다";
				_log.Exception(ex);
			}
			finally
			{
				// 데이터베이스를  Close한다
				_db.Close();
			}

		}
		#endregion

		#region 시간-요일별 집계
		/// <summary>
		/// 광고별 시청현황 집계
		/// </summary>
		/// <param name="areaDayModel"></param>
		public void GetTimeDay(HeaderModel header, AreaDayModel data)
		{

			try
			{
				StringBuilder sbQuery = null;

				// 데이터베이스를 OPEN한다
				_db.Open();


				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetTimeDay() Start");
				_log.Debug("-----------------------------------------");

				// 일자가 6자리 이상이면 6자리로 만든다.
				if(data.StartDay.Length > 6)    data.StartDay = data.StartDay.Substring(2,6);
				if(data.EndDay.Length > 6)      data.EndDay = data.EndDay.Substring(2,6);
				
				string sqlItems = "";
				if( data.AdList.Count > 0 )
				{
					for( int i = 0; i < data.AdList.Count;i++)
					{
						if( i == 0 )    sqlItems = data.AdList[i].ToString();
						else            sqlItems += "," + data.AdList[i].ToString();
					}
				}

				// __DEBUG__
				_log.Debug("<입력정보>");
				_log.Debug("StartDay :[" + data.StartDay    + "]");
				_log.Debug("EndDay   :[" + data.EndDay      + "]");
				_log.Debug("Items    :[" + sqlItems         + "]");
				// __DEBUG__


				// 쿼리생성
				sbQuery = new StringBuilder();
				sbQuery.Append("\n"
					+ "SELECT v2.Hour         -- 광고번호								      								\n"   
					+ "      ,Sum( case v2.week when 2 then Cnt else 0 end)  as Mon 									\n"
					+ "      ,sum( case v2.week when 3 then Cnt else 0 end)  as Tue										\n"		
					+ "      ,sum( case v2.week when 4 then Cnt else 0 end)  as Wed										\n" 					
					+ "      ,sum( case v2.week when 5 then Cnt else 0 end)  as Thu									    \n"
					+ "      ,sum( case v2.week when 6 then Cnt else 0 end)  as Fri										\n"
					+ "      ,sum( case v2.week when 7 then Cnt else 0 end)  as Sat										\n"
					+ "      ,sum( case v2.week when 1 then Cnt else 0 end)  as Sun										\n"					
					+ "from (																							\n"
					+ "      SELECT v1.week																				\n"
					+ "			,case t.RowNum																			\n"
					+ "			when 1  then 0    when 2  then 1    when 3    then 2    when 4  then 3						\n"
					+ "			when 5  then 4    when 6  then 5    when 7    then 6    when 8  then 7						\n"
					+ "			when 9  then 8    when 10 then 9    when 11   then 10   when 12 then 11						\n"
					+ "			when 13  then 12  when 14  then 13  when 15  then 14    when 16  then 15					\n"
					+ "			when 17  then 16  when 18  then 17  when 19  then 18    when 20  then 19					\n"
					+ "			when 21  then 20  when 22  then 21  when 23  then 22    when 24  then 23					\n"
					+ "			end  AS Hour																				\n" 
					+ "			,case t.RowNum																				\n"
					+ "			when 1   then H00  when 2   then H01 when 3  then H02 when 4  then H03 when 5  then H04 when 6  then H05  when 7  then H06 when 8  then H07       \n"
					+ "			when 9   then H08  when 10  then H09 when 11 then H10 when 12 then H11 when 13 then H12 when 14 then H13  when 15 then H14 when 16  then H15      \n"
					+ "			when 17  then H16  when 18  then H17 when 19 then H18 when 20 then H19 when 21 then H20 when 22 then H21  when 23 then H22 when 24  then H23      \n"
					+ "			end  As Cnt      \n"
					+ "   FROM (																					    \n"
					+ "      SELECT b.week																		        \n"
					+ "      ,SUM([H00]) AS  H00 ,SUM([H01]) AS  H01 ,SUM([H02]) AS  H02 ,SUM([H03]) AS  H03			\n"
					+ "      ,SUM([H04]) AS  H04 ,SUM([H05]) AS  H05 ,SUM([H06]) AS  H06 ,SUM([H07]) AS  H07			\n"
					+ "      ,SUM([H08]) AS  H08 ,SUM([H09]) AS  H09 ,SUM([H10]) AS  H10 ,SUM([H11]) AS  H11			\n"
					+ "      ,SUM([H12]) AS  H12 ,SUM([H13]) AS  H13 ,SUM([H14]) AS  H14 ,SUM([H15]) AS  H15			\n"
					+ "      ,SUM([H16]) AS  H16 ,SUM([H17]) AS  H17 ,SUM([H18]) AS  H18 ,SUM([H19]) AS  H19			\n"
					+ "      ,SUM([H20]) AS  H20 ,SUM([H21]) AS  H21 ,SUM([H22]) AS  H22 ,SUM([H23]) AS  H23			\n"
					+ "     FROM dbo.SummaryAdDaily0 a with(NoLock)													    \n"
					+ "      inner join dbo.SummaryBase b with(NoLock) on a.LogDay = b.LogDay					        \n"
					+ "      where a.LogDay between '" + data.StartDay + "' and '" + data.EndDay + "' " + "\n"
					+ "      and   a.ItemNo in(" + sqlItems + ") " + "\n"
					+ "      and   a.ContractSeq = 0																	\n" 
					+ "      and   a.SummaryType = 1																	\n"
                    + "      group by b.week ) V1 INNER JOIN AdTargetsHanaTV.dbo.COPY_T T ON T.ROWNUM <= 24 ) V2   \n"
					+ "GROUP BY v2.Hour																					\n"					
					);

			
				// __DEBUG__
				_log.Debug(sbQuery.ToString());
				// __DEBUG__
				
				// 쿼리실행
				DataSet ds = new DataSet();
				_db.ExecuteQuery(ds,sbQuery.ToString());

				// 결과 DataSet의 데이터모델에 복사
				data.ReportDataSet = ds.Copy();

				// 결과
				data.ResultCnt = Utility.GetDatasetCount(data.ReportDataSet);

				ds.Dispose();


				// 결과코드 셋트
				data.ResultCD = "0000";

				// __DEBUG__
				_log.Debug("<출력정보>");
				_log.Debug("ResultCnt:[" + data.ResultCnt + "]");
				// __DEBUG__

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetTimeDay() End");
				_log.Debug("-----------------------------------------");
			}
			catch(Exception ex)
			{
				data.ResultCD = "3000";
				data.ResultDesc = "시간-요일별 집계 조회중 오류가 발생하였습니다";
				_log.Exception(ex);
			}
			finally
			{
				// 데이터베이스를  Close한다
				_db.Close();
			}

		}
		#endregion

		#region 장르-요일별 집계
		/// <summary>
		/// 광고별 시청현황 집계
		/// </summary>
		/// <param name="areaDayModel"></param>
		public void GetGenreDay(HeaderModel header, AreaDayModel data)
		{

			try
			{
				StringBuilder sbQuery = null;

				// 데이터베이스를 OPEN한다
				_db.Open();


				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetGenreDay() Start");
				_log.Debug("-----------------------------------------");

				// 일자가 6자리 이상이면 6자리로 만든다.
				if(data.StartDay.Length > 6)    data.StartDay = data.StartDay.Substring(2,6);
				if(data.EndDay.Length > 6)      data.EndDay = data.EndDay.Substring(2,6);
				
				string sqlItems = "";
				if( data.AdList.Count > 0 )
				{
					for( int i = 0; i < data.AdList.Count;i++)
					{
						if( i == 0 )    sqlItems = data.AdList[i].ToString();
						else            sqlItems += "," + data.AdList[i].ToString();
					}
				}

				// __DEBUG__
				_log.Debug("<입력정보>");
				_log.Debug("StartDay :[" + data.StartDay    + "]");
				_log.Debug("EndDay   :[" + data.EndDay      + "]");
				_log.Debug("Items    :[" + sqlItems         + "]");
				// __DEBUG__

				// 쿼리생성
				sbQuery = new StringBuilder();
				sbQuery.Append("\n"
                    + "SELECT v1.Category, (select CategoryName  from AdTargetsHanaTV.dbo.Category noLock where CategoryCode = v1.Category) as CategoryNm         -- 광고번호								      								\n"
                    + "      ,v1.Genre,    (select GenreName     from AdTargetsHanaTV.dbo.Genre    noLock where GenreCode    = v1.Genre)    as GenreNm 									\n"
					+ "      ,Sum( case v1.week when 2 then Cnt else 0 end)  as Mon										\n"		
					+ "      ,sum( case v1.week when 3 then Cnt else 0 end)  as Tue										\n" 					
					+ "      ,sum( case v1.week when 4 then Cnt else 0 end)  as Wed									    \n"
					+ "      ,sum( case v1.week when 5 then Cnt else 0 end)  as Thu										\n"
					+ "      ,sum( case v1.week when 6 then Cnt else 0 end)  as Fri										\n"
					+ "      ,sum( case v1.week when 7 then Cnt else 0 end)  as Sat										\n"					
					+ "      ,sum( case v1.week when 1 then Cnt else 0 end)  as Sun										\n"					
					+ "from (																							\n"
					+ "         select   b.week																				\n"
					+ "			        ,a.Category																				\n"
					+ "			        ,a.Genre																				\n"
					+ "			        ,sum(a.AdCnt) as Cnt																	\n"
					+ "			from    dbo.SummaryAdDaily3 a with(NoLock)													\n"
					+ "			inner   join dbo.SummaryBase b with(NoLock) on a.LogDay = b.LogDay						\n"
					+ "         where   a.LogDay between '" + data.StartDay + "' and '" + data.EndDay + "' " + "\n"
					+ "         and     a.ItemNo in(" + sqlItems + ") " + "\n"
					+ "			and     a.ContractSeq = 0																	\n" 
					+ "			and     a.progKey > 0																		\n"
					+ "			group by b.week,a.Category,a.Genre ) v1													\n"					
					+ "group by v1.Category, v1.Genre																	\n"					
					+ "order by v1.Category, v1.Genre																	\n"					
					);

			
				// __DEBUG__
				_log.Debug(sbQuery.ToString());
				// __DEBUG__
				
				// 쿼리실행
				DataSet ds = new DataSet();
				_db.ExecuteQuery(ds,sbQuery.ToString());

				// 결과 DataSet의 데이터모델에 복사
				data.ReportDataSet = ds.Copy();

				// 결과
				data.ResultCnt = Utility.GetDatasetCount(data.ReportDataSet);

				ds.Dispose();


				// 결과코드 셋트
				data.ResultCD = "0000";

				// __DEBUG__
				_log.Debug("<출력정보>");
				_log.Debug("ResultCnt:[" + data.ResultCnt + "]");
				// __DEBUG__

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetGenreDay() End");
				_log.Debug("-----------------------------------------");
			}
			catch(Exception ex)
			{
				data.ResultCD = "3000";
				data.ResultDesc = "장르별 요일집계 조회중 오류가 발생하였습니다";
				_log.Exception(ex);
			}
			finally
			{
				// 데이터베이스를  Close한다
				_db.Close();
			}

		}
		#endregion

		#region 채널별 요일집계
		/// <summary>
		/// 광고별 시청현황 집계
		/// </summary>
		/// <param name="areaDayModel"></param>
		public void GetChannelDay(HeaderModel header, AreaDayModel data)
		{

			try
			{
				StringBuilder sbQuery = null;

				// 데이터베이스를 OPEN한다
				_db.Open();


				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetChannelDay() Start");
				_log.Debug("-----------------------------------------");

				// 일자가 6자리 이상이면 6자리로 만든다.
				if(data.StartDay.Length > 6)    data.StartDay = data.StartDay.Substring(2,6);
				if(data.EndDay.Length > 6)      data.EndDay = data.EndDay.Substring(2,6);
				
				string sqlItems = "";
				if( data.AdList.Count > 0 )
				{
					for( int i = 0; i < data.AdList.Count;i++)
					{
						if( i == 0 )    sqlItems = data.AdList[i].ToString();
						else            sqlItems += "," + data.AdList[i].ToString();
					}
				}

				// __DEBUG__
				_log.Debug("<입력정보>");
				_log.Debug("StartDay :[" + data.StartDay    + "]");
				_log.Debug("EndDay   :[" + data.EndDay      + "]");
				_log.Debug("Items    :[" + sqlItems         + "]");
				// __DEBUG__

				// 쿼리생성
				sbQuery = new StringBuilder();
				sbQuery.Append("\n"
					+ "-- 광고별 시청현황        \n"
                    + "select v1.Category, (select CategoryName  from AdTargetsHanaTV.dbo.Category where CategoryCode = v1.Category) as CategoryNm         -- 광고번호								      								\n"
                    + "      ,v1.Genre,    (select GenreName     from AdTargetsHanaTV.dbo.Genre    where GenreCode = v1.Genre)       as GenreNm 			\n"
                    + "      ,v1.progKey,  (select ProgramNm     from AdTargetsHanaTV.dbo.Program  where Programkey= v1.Progkey)       as ChannelNm			\n"		
					+ "      ,Sum( case v1.week when 2 then Cnt else 0 end)  as Mon			\n" 					
					+ "      ,sum( case v1.week when 3 then Cnt else 0 end)  as Tue		    \n"
					+ "      ,sum( case v1.week when 4 then Cnt else 0 end)  as Wed			\n"
					+ "      ,sum( case v1.week when 5 then Cnt else 0 end)  as Thu			\n"
					+ "      ,sum( case v1.week when 6 then Cnt else 0 end)  as Fri			\n"					
					+ "      ,sum( case v1.week when 7 then Cnt else 0 end)  as Sat			\n"					
					+ "      ,sum( case v1.week when 1 then Cnt else 0 end)  as Sun			\n"					
					+ "from (																							\n"
					+ "      SELECT b.week																				\n"
					+ "			,a.Category															\n"
					+ "			,a.Genre															\n"
					+ "			,a.Progkey															\n"
					+ "			,sum(a.AdCnt) as Cnt												\n"
					+ "			FROM dbo.SummaryAdDaily3 a with(NoLock)								\n"
					+ "			inner join dbo.SummaryBase b with(NoLock) on a.LogDay = b.LogDay												\n"
					+ "      where a.LogDay between '" + data.StartDay + "' and '" + data.EndDay + "' " + "\n"
					+ "      and   a.ItemNo in(" + sqlItems + ") " + "\n"
					+ "			and   a.ContractSeq = 0																	\n" 
					+ "			and   a.progKey > 0																		\n"
					+ "			group by b.week,a.Category,a.Genre,a.ProgKey ) v1										\n"										
					+ "group by v1.Category, v1.Genre,v1.ProgKey																	\n"					
					+ "order by v1.Category, v1.Genre,v1.ProgKey																	\n"					
					);

			
				// __DEBUG__
				_log.Debug(sbQuery.ToString());
				// __DEBUG__
				
				// 쿼리실행
				DataSet ds = new DataSet();
				_db.ExecuteQuery(ds,sbQuery.ToString());

				// 결과 DataSet의 데이터모델에 복사
				data.ReportDataSet = ds.Copy();

				// 결과
				data.ResultCnt = Utility.GetDatasetCount(data.ReportDataSet);

				ds.Dispose();


				// 결과코드 셋트
				data.ResultCD = "0000";

				// __DEBUG__
				_log.Debug("<출력정보>");
				_log.Debug("ResultCnt:[" + data.ResultCnt + "]");
				// __DEBUG__

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetChannelDay() End");
				_log.Debug("-----------------------------------------");
			}
			catch(Exception ex)
			{
				data.ResultCD = "3000";
				data.ResultDesc = "채널별 요일집계 조회중 오류가 발생하였습니다";
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