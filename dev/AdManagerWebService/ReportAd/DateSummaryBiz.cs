// ===============================================================================
//
// DateSummaryBiz.cs
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
 * Class Name: DateSummaryBiz
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
	/// DateSummaryBiz에 대한 요약 설명입니다.
	/// </summary>
	public class DateSummaryBiz : BaseBiz
	{
		#region  생성자
		public DateSummaryBiz() : base(FrameSystem.connSummaryDbString)
		{
			_log = FrameSystem.oLog;
		}
		#endregion

		#region 기간별 장르집계
		/// <summary>
		/// 광고별 시청현황 집계
		/// </summary>
		/// <param name="dateSummaryModel"></param>
		public void GetDateGenre(HeaderModel header, DateSummaryModel data)
		{

			try
			{
				StringBuilder sbQuery = null;

				// 데이터베이스를 OPEN한다
				_db.Open();

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetDateGenre() Start");
				_log.Debug("-----------------------------------------");

                // 일자가 6자리 이상이면 6자리로 만든다.
                if(data.StartDay.Length > 6) data.StartDay = data.StartDay.Substring(2,6);
                if(data.EndDay.Length > 6) data.EndDay = data.EndDay.Substring(2,6);

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
                _log.Debug("StartDay :[" + data.StartDay    + "]");		// 검색 집계일자           
                _log.Debug("EndDay   :[" + data.EndDay      + "]");		// 검색 키워드   
                _log.Debug("Items    :[" + sqlItems                     + "]");
                // __DEBUG__

				// 쿼리생성
				sbQuery = new StringBuilder();
                sbQuery.Append("\n");
                sbQuery.Append(" SELECT 0           as category " + "\n");
                sbQuery.Append("       ,'카테고리'  as categoryNm " + "\n");
                sbQuery.Append("       ,0 as Genre " + "\n");
                sbQuery.Append("       ,'장르'  as GenreNm " + "\n");
                sbQuery.Append("       ,case d01 when '' then '' else substring(d01,3,2) + '/' + substring(d01,5,2) end	as d01 " + "\n");
                sbQuery.Append("       ,case d02 when '' then '' else substring(d02,3,2) + '/' + substring(d02,5,2) end	as d02 " + "\n");
                sbQuery.Append("       ,case d03 when '' then '' else substring(d03,3,2) + '/' + substring(d03,5,2) end	as d03 " + "\n");
                sbQuery.Append("       ,case d04 when '' then '' else substring(d04,3,2) + '/' + substring(d04,5,2) end	as d04 " + "\n");
                sbQuery.Append("       ,case d05 when '' then '' else substring(d05,3,2) + '/' + substring(d05,5,2) end	as d05 " + "\n");
                sbQuery.Append("       ,case d06 when '' then '' else substring(d06,3,2) + '/' + substring(d06,5,2) end	as d06 " + "\n");
                sbQuery.Append("       ,case d07 when '' then '' else substring(d07,3,2) + '/' + substring(d07,5,2) end	as d07 " + "\n");
                sbQuery.Append("       ,case d08 when '' then '' else substring(d08,3,2) + '/' + substring(d08,5,2) end	as d08 " + "\n");
                sbQuery.Append("       ,case d09 when '' then '' else substring(d09,3,2) + '/' + substring(d09,5,2) end	as d09 " + "\n");
                sbQuery.Append("       ,case d10 when '' then '' else substring(d10,3,2) + '/' + substring(d10,5,2) end	as d10 " + "\n");
                sbQuery.Append("       ,case d11 when '' then '' else substring(d11,3,2) + '/' + substring(d11,5,2) end	as d11 " + "\n");
                sbQuery.Append("       ,case d12 when '' then '' else substring(d12,3,2) + '/' + substring(d12,5,2) end	as d12 " + "\n");
                sbQuery.Append("       ,case d13 when '' then '' else substring(d13,3,2) + '/' + substring(d13,5,2) end	as d13 " + "\n");
                sbQuery.Append("       ,case d14 when '' then '' else substring(d14,3,2) + '/' + substring(d14,5,2) end	as d14 " + "\n");
                sbQuery.Append("       ,case d15 when '' then '' else substring(d15,3,2) + '/' + substring(d15,5,2) end	as d15 " + "\n");
                sbQuery.Append("       ,case d16 when '' then '' else substring(d16,3,2) + '/' + substring(d16,5,2) end	as d16 " + "\n");
                sbQuery.Append("       ,case d17 when '' then '' else substring(d17,3,2) + '/' + substring(d17,5,2) end	as d17 " + "\n");
                sbQuery.Append("       ,case d18 when '' then '' else substring(d18,3,2) + '/' + substring(d18,5,2) end	as d18 " + "\n");
                sbQuery.Append("       ,case d19 when '' then '' else substring(d19,3,2) + '/' + substring(d19,5,2) end	as d19 " + "\n");
                sbQuery.Append("       ,case d20 when '' then '' else substring(d20,3,2) + '/' + substring(d20,5,2) end	as d20 " + "\n");
                sbQuery.Append("       ,case d21 when '' then '' else substring(d21,3,2) + '/' + substring(d21,5,2) end	as d21 " + "\n");
                sbQuery.Append("       ,case d22 when '' then '' else substring(d22,3,2) + '/' + substring(d22,5,2) end	as d22 " + "\n");
                sbQuery.Append("       ,case d23 when '' then '' else substring(d23,3,2) + '/' + substring(d23,5,2) end	as d23 " + "\n");
                sbQuery.Append("       ,case d24 when '' then '' else substring(d24,3,2) + '/' + substring(d24,5,2) end	as d24 " + "\n");
                sbQuery.Append("       ,case d25 when '' then '' else substring(d25,3,2) + '/' + substring(d25,5,2) end	as d25 " + "\n");
                sbQuery.Append("       ,case d26 when '' then '' else substring(d26,3,2) + '/' + substring(d26,5,2) end	as d26 " + "\n");
                sbQuery.Append("       ,case d27 when '' then '' else substring(d27,3,2) + '/' + substring(d27,5,2) end	as d27 " + "\n");
                sbQuery.Append("       ,case d28 when '' then '' else substring(d28,3,2) + '/' + substring(d28,5,2) end	as d28 " + "\n");
                sbQuery.Append("       ,case d29 when '' then '' else substring(d29,3,2) + '/' + substring(d29,5,2) end	as d29 " + "\n");
                sbQuery.Append("       ,case d30 when '' then '' else substring(d30,3,2) + '/' + substring(d30,5,2) end	as d30 " + "\n");
                sbQuery.Append("       ,case d31 when '' then '' else substring(d31,3,2) + '/' + substring(d31,5,2) end	as d31 " + "\n");
                sbQuery.Append(" from   (   select    max(case v2.rowNum when 1 then v2.LogDay	else '' end)	as d01 " + "\n");
                sbQuery.Append("                    , max(case v2.rowNum when 2 then v2.LogDay	else '' end)	as d02 " + "\n");
                sbQuery.Append(" 	                , max(case v2.rowNum when 3 then v2.LogDay	else '' end)	as d03 " + "\n");
                sbQuery.Append(" 	                , max(case v2.rowNum when 4 then v2.LogDay	else '' end)	as d04 " + "\n");
                sbQuery.Append(" 	                , max(case v2.rowNum when 5 then v2.LogDay	else '' end)	as d05 " + "\n");
                sbQuery.Append(" 	                , max(case v2.rowNum when 6 then v2.LogDay	else '' end)	as d06 " + "\n");
                sbQuery.Append(" 					, max(case v2.rowNum when 7 then v2.LogDay	else '' end)	as d07 " + "\n");
                sbQuery.Append(" 					, max(case v2.rowNum when 8 then v2.LogDay	else '' end)	as d08 " + "\n");
                sbQuery.Append(" 					, max(case v2.rowNum when 9 then v2.LogDay	else '' end)	as d09 " + "\n");
                sbQuery.Append(" 					, max(case v2.rowNum when 10 then v2.LogDay else '' end)	as d10 " + "\n");
                sbQuery.Append(" 					, max(case v2.rowNum when 11 then v2.LogDay else '' end)	as d11 " + "\n");
                sbQuery.Append(" 					, max(case v2.rowNum when 12 then v2.LogDay else '' end)	as d12 " + "\n");
                sbQuery.Append("    				, max(case v2.rowNum when 13 then v2.LogDay else '' end)	as d13 " + "\n");
                sbQuery.Append(" 					, max(case v2.rowNum when 14 then v2.LogDay else '' end)	as d14 " + "\n");
                sbQuery.Append(" 					, max(case v2.rowNum when 15 then v2.LogDay else '' end)	as d15 " + "\n");
                sbQuery.Append(" 					, max(case v2.rowNum when 16 then v2.LogDay else '' end)	as d16 " + "\n");
                sbQuery.Append(" 					, max(case v2.rowNum when 17 then v2.LogDay else '' end)	as d17 " + "\n");
                sbQuery.Append(" 					, max(case v2.rowNum when 18 then v2.LogDay else '' end)	as d18 " + "\n");
                sbQuery.Append(" 					, max(case v2.rowNum when 19 then v2.LogDay else '' end)	as d19 " + "\n");
                sbQuery.Append(" 					, max(case v2.rowNum when 20 then v2.LogDay else '' end)	as d20 " + "\n");
                sbQuery.Append(" 					, max(case v2.rowNum when 21 then v2.LogDay else '' end)	as d21 " + "\n");
                sbQuery.Append(" 					, max(case v2.rowNum when 22 then v2.LogDay else '' end)	as d22 " + "\n");
                sbQuery.Append(" 					, max(case v2.rowNum when 23 then v2.LogDay else '' end)	as d23 " + "\n");
                sbQuery.Append(" 					, max(case v2.rowNum when 24 then v2.LogDay else '' end)	as d24 " + "\n");
                sbQuery.Append(" 					, max(case v2.rowNum when 25 then v2.LogDay else '' end)	as d25 " + "\n");
                sbQuery.Append(" 					, max(case v2.rowNum when 26 then v2.LogDay else '' end)	as d26 " + "\n");
                sbQuery.Append(" 					, max(case v2.rowNum when 27 then v2.LogDay else '' end)	as d27 " + "\n");
                sbQuery.Append(" 					, max(case v2.rowNum when 28 then v2.LogDay else '' end)	as d28 " + "\n");
                sbQuery.Append(" 					, max(case v2.rowNum when 29 then v2.LogDay else '' end)	as d29 " + "\n");
                sbQuery.Append(" 					, max(case v2.rowNum when 30 then v2.LogDay else '' end)	as d30 " + "\n");
                sbQuery.Append(" 					, max(case v2.rowNum when 31 then v2.LogDay else '' end)	as d31 " + "\n");
                sbQuery.Append(" 	        from (  select  LogDay,row_number() over(order by LogDay) as rowNum	 " + "\n");
                sbQuery.Append("                    from    SummaryBase base with(nolock) " + "\n");
                sbQuery.Append("                    where	base.LogDay between '" + data.StartDay + "' and '" + data.EndDay + "') v2 ) y1 " + "\n");
                sbQuery.Append("            union all " + "\n");
                sbQuery.Append("            select	  y2.Category " + "\n");
                sbQuery.Append("                    , (select CategoryName from AdTargetsHanaTV.dbo.Category where CategoryCode = y2.Category) " + "\n");
                sbQuery.Append(" 	          		, y2.Genre " + "\n");
                sbQuery.Append(" 	          		, (select GenreName from AdTargetsHanaTV.dbo.Genre where GenreCode = y2.Genre) " + "\n");
                sbQuery.Append(" 	          		, replace( Convert( varchar(12),Convert(money,y2.d01),1),'.00','')	as d01 " + "\n");
                sbQuery.Append(" 	          		, replace( Convert( varchar(12),Convert(money,y2.d02),1),'.00','')	as d02 " + "\n");
                sbQuery.Append(" 	        		, replace( Convert( varchar(12),Convert(money,y2.d03),1),'.00','')	as d03 " + "\n");
                sbQuery.Append(" 	          		, replace( Convert( varchar(12),Convert(money,y2.d04),1),'.00','')	as d04 " + "\n");
                sbQuery.Append(" 	          		, replace( Convert( varchar(12),Convert(money,y2.d05),1),'.00','')	as d05 " + "\n");
                sbQuery.Append(" 	          		, replace( Convert( varchar(12),Convert(money,y2.d06),1),'.00','')	as d06 " + "\n");
                sbQuery.Append(" 	          		, replace( Convert( varchar(12),Convert(money,y2.d07),1),'.00','')	as d07 " + "\n");
                sbQuery.Append(" 	          		, replace( Convert( varchar(12),Convert(money,y2.d08),1),'.00','')	as d08 " + "\n");
                sbQuery.Append(" 	          		, replace( Convert( varchar(12),Convert(money,y2.d09),1),'.00','')	as d09 " + "\n");
                sbQuery.Append(" 	          		, replace( Convert( varchar(12),Convert(money,y2.d10),1),'.00','')	as d10 " + "\n");
                sbQuery.Append(" 	          		, replace( Convert( varchar(12),Convert(money,y2.d11),1),'.00','')	as d11 " + "\n");
                sbQuery.Append(" 	          		, replace( Convert( varchar(12),Convert(money,y2.d12),1),'.00','')	as d12 " + "\n");
                sbQuery.Append(" 	          		, replace( Convert( varchar(12),Convert(money,y2.d13),1),'.00','')	as d13 " + "\n");
                sbQuery.Append(" 	          		, replace( Convert( varchar(12),Convert(money,y2.d14),1),'.00','')	as d14 " + "\n");
                sbQuery.Append(" 	          		, replace( Convert( varchar(12),Convert(money,y2.d15),1),'.00','')	as d15 " + "\n");
                sbQuery.Append(" 	          		, replace( Convert( varchar(12),Convert(money,y2.d16),1),'.00','')	as d16 " + "\n");
                sbQuery.Append(" 	          		, replace( Convert( varchar(12),Convert(money,y2.d17),1),'.00','')	as d17 " + "\n");
                sbQuery.Append(" 	          		, replace( Convert( varchar(12),Convert(money,y2.d18),1),'.00','')	as d18 " + "\n");
                sbQuery.Append(" 	          		, replace( Convert( varchar(12),Convert(money,y2.d19),1),'.00','')	as d19 " + "\n");
                sbQuery.Append(" 	          		, replace( Convert( varchar(12),Convert(money,y2.d20),1),'.00','')	as d20 " + "\n");
                sbQuery.Append(" 	          		, replace( Convert( varchar(12),Convert(money,y2.d21),1),'.00','')	as d21 " + "\n");
                sbQuery.Append(" 	          		, replace( Convert( varchar(12),Convert(money,y2.d22),1),'.00','')	as d22 " + "\n");
                sbQuery.Append(" 	          		, replace( Convert( varchar(12),Convert(money,y2.d23),1),'.00','')	as d23 " + "\n");
                sbQuery.Append(" 	          		, replace( Convert( varchar(12),Convert(money,y2.d24),1),'.00','')	as d24 " + "\n");
                sbQuery.Append(" 	          		, replace( Convert( varchar(12),Convert(money,y2.d25),1),'.00','')	as d25 " + "\n");
                sbQuery.Append(" 	          		, replace( Convert( varchar(12),Convert(money,y2.d26),1),'.00','')	as d26 " + "\n");
                sbQuery.Append(" 	          		, replace( Convert( varchar(12),Convert(money,y2.d27),1),'.00','')	as d27 " + "\n");
                sbQuery.Append(" 	          		, replace( Convert( varchar(12),Convert(money,y2.d28),1),'.00','')	as d28 " + "\n");
                sbQuery.Append(" 	          		, replace( Convert( varchar(12),Convert(money,y2.d29),1),'.00','')	as d29 " + "\n");
                sbQuery.Append(" 	          		, replace( Convert( varchar(12),Convert(money,y2.d30),1),'.00','')	as d30 " + "\n");
                sbQuery.Append(" 	          		, replace( Convert( varchar(12),Convert(money,y2.d31),1),'.00','')	as d31 " + "\n");
                sbQuery.Append(" 	        from (  select	 v1.Category " + "\n");
                sbQuery.Append(" 	                        ,v1.Genre " + "\n");
                sbQuery.Append(" 	      					, sum(case v2.rowNum when 1 then v1.adCnt else 0 end)	as d01 " + "\n");
                sbQuery.Append(" 	      					, sum(case v2.rowNum when 2 then v1.adCnt else 0 end)	as d02 " + "\n");
                sbQuery.Append(" 	      					, sum(case v2.rowNum when 3 then v1.adCnt else 0 end)	as d03 " + "\n");
                sbQuery.Append(" 	      					, sum(case v2.rowNum when 4 then v1.adCnt else 0 end)	as d04 " + "\n");
                sbQuery.Append(" 	      					, sum(case v2.rowNum when 5 then v1.adCnt else 0 end)	as d05 " + "\n");
                sbQuery.Append(" 	      					, sum(case v2.rowNum when 6 then v1.adCnt else 0 end)	as d06 " + "\n");
                sbQuery.Append(" 	      					, sum(case v2.rowNum when 7 then v1.adCnt else 0 end)	as d07 " + "\n");
                sbQuery.Append(" 	      					, sum(case v2.rowNum when 8 then v1.adCnt else 0 end)	as d08 " + "\n");
                sbQuery.Append(" 	      					, sum(case v2.rowNum when 9 then v1.adCnt else 0 end)	as d09 " + "\n");
                sbQuery.Append(" 	      					, sum(case v2.rowNum when 10 then v1.adCnt else 0 end)	as d10 " + "\n");
                sbQuery.Append(" 	      					, sum(case v2.rowNum when 11 then v1.adCnt else 0 end)	as d11 " + "\n");
                sbQuery.Append(" 	      					, sum(case v2.rowNum when 12 then v1.adCnt else 0 end)	as d12 " + "\n");
                sbQuery.Append(" 	      					, sum(case v2.rowNum when 13 then v1.adCnt else 0 end)	as d13 " + "\n");
                sbQuery.Append(" 	      					, sum(case v2.rowNum when 14 then v1.adCnt else 0 end)	as d14 " + "\n");
                sbQuery.Append(" 	      					, sum(case v2.rowNum when 15 then v1.adCnt else 0 end)	as d15 " + "\n");
                sbQuery.Append(" 	      					, sum(case v2.rowNum when 16 then v1.adCnt else 0 end)	as d16 " + "\n");
                sbQuery.Append(" 	      					, sum(case v2.rowNum when 17 then v1.adCnt else 0 end)	as d17 " + "\n");
                sbQuery.Append(" 	      					, sum(case v2.rowNum when 18 then v1.adCnt else 0 end)	as d18 " + "\n");
                sbQuery.Append(" 	      					, sum(case v2.rowNum when 19 then v1.adCnt else 0 end)	as d19 " + "\n");
                sbQuery.Append(" 	      					, sum(case v2.rowNum when 20 then v1.adCnt else 0 end)	as d20 " + "\n");
                sbQuery.Append(" 	      					, sum(case v2.rowNum when 21 then v1.adCnt else 0 end)	as d21 " + "\n");
                sbQuery.Append(" 	      					, sum(case v2.rowNum when 22 then v1.adCnt else 0 end)	as d22 " + "\n");
                sbQuery.Append(" 	      					, sum(case v2.rowNum when 23 then v1.adCnt else 0 end)	as d23 " + "\n");
                sbQuery.Append(" 	      					, sum(case v2.rowNum when 24 then v1.adCnt else 0 end)	as d24 " + "\n");
                sbQuery.Append(" 	      					, sum(case v2.rowNum when 25 then v1.adCnt else 0 end)	as d25 " + "\n");
                sbQuery.Append(" 	      					, sum(case v2.rowNum when 26 then v1.adCnt else 0 end)	as d26 " + "\n");
                sbQuery.Append(" 	      					, sum(case v2.rowNum when 27 then v1.adCnt else 0 end)	as d27 " + "\n");
                sbQuery.Append(" 	      					, sum(case v2.rowNum when 28 then v1.adCnt else 0 end)	as d28 " + "\n");
                sbQuery.Append(" 	      					, sum(case v2.rowNum when 29 then v1.adCnt else 0 end)	as d29 " + "\n");
                sbQuery.Append("         					, sum(case v2.rowNum when 30 then v1.adCnt else 0 end)	as d30 " + "\n");
                sbQuery.Append(" 	      					, sum(case v2.rowNum when 31 then v1.adCnt else 0 end)	as d31 " + "\n");
                sbQuery.Append(" 					from (	select   LogDay " + "\n");
                sbQuery.Append(" 								    ,Category " + "\n");
                sbQuery.Append(" 								    ,Genre " + "\n");
                sbQuery.Append(" 								    ,sum(AdCnt) as adCnt " + "\n");
                sbQuery.Append(" 					from	SummaryAdDaily3 a with(noLock) " + "\n");
                sbQuery.Append("                    where   a.LogDay between '" + data.StartDay + "' and '" + data.EndDay + "'  " + "\n");
                sbQuery.Append("                    and     a.ItemNo in(" + sqlItems + ")  " + "\n");
                sbQuery.Append(" 					and     a.ContractSeq = 0 " + "\n");
                sbQuery.Append(" 					and		a.progKey > 0 " + "\n");
                sbQuery.Append(" 					and		a.Category> 0 " + "\n");
                sbQuery.Append(" 					and		a.genre	  > 0 " + "\n");
                sbQuery.Append(" 					group by LogDay,Category,Genre ) v1 " + "\n");
                sbQuery.Append(" 			inner join " + "\n");
                sbQuery.Append(" 				(	select LogDay,row_number() over(order by LogDay) as rowNum " + "\n");
                sbQuery.Append(" 					from	SummaryBase base with(nolock) " + "\n");
                sbQuery.Append(" 					where	base.LogDay between '" + data.StartDay + "' and '" + data.EndDay + "' ) v2 on v1.LogDay = v2.LogDay " + "\n");
                sbQuery.Append(" 			group by v1.Category, v1.Genre ) y2 " + "\n");
                sbQuery.Append(" order by Category,Genre;" + "\n");
			
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
				_log.Debug(this.ToString() + "GetDateGenre() End");
				_log.Debug("-----------------------------------------");
			}
			catch(Exception ex)
			{
				data.ResultCD = "3000";
				data.ResultDesc = "기간별 장르집계 조회중 오류가 발생하였습니다";
				_log.Exception(ex);
			}
			finally
			{
				// 데이터베이스를  Close한다
				_db.Close();
			}

		}
		#endregion

		#region 기간별 채널집계
		/// <summary>
		/// 광고별 시청현황 집계
		/// </summary>
		/// <param name="dateSummaryModel"></param>
		public void GetDateChannel(HeaderModel header, DateSummaryModel data)
		{

			try
			{
				StringBuilder sbQuery = null;

				// 데이터베이스를 OPEN한다
				_db.Open();


				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetDateChannel() Start");
				_log.Debug("-----------------------------------------");

                // 일자가 6자리 이상이면 6자리로 만든다.
                if(data.StartDay.Length > 6) data.StartDay = data.StartDay.Substring(2,6);
                if(data.EndDay.Length > 6) data.EndDay = data.EndDay.Substring(2,6);

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
                _log.Debug("StartDay :[" + data.StartDay    + "]");		// 검색 집계일자           
                _log.Debug("EndDay   :[" + data.EndDay      + "]");		// 검색 키워드   
                _log.Debug("Items    :[" + sqlItems                     + "]");
                // __DEBUG__

				// 쿼리생성
				sbQuery = new StringBuilder();
				sbQuery.Append("\n"
					+ "-- 광고별 시청현황        \n"					
					+ "SELECT 0					as category         -- 광고번호				\n"   					
					+ "      ,'카테고리'  as categoryNm										\n"		
					+ "      ,0 as Genre													\n"		
					+ "      ,'장르'  as GenreNm												\n"		
					+ "      ,0 as Progkey													\n"		
					+ "      ,'채널'  as ChannelNm											\n"		
					+ "      ,case d01 when '' then '' else substring(d01,3,2) + '/' + substring(d01,5,2) end	as d01										\n" 					
					+ "      ,case d02 when '' then '' else substring(d02,3,2) + '/' + substring(d02,5,2) end	as d02									    \n"
					+ "      ,case d03 when '' then '' else substring(d03,3,2) + '/' + substring(d03,5,2) end	as d03										\n"
					+ "      ,case d04 when '' then '' else substring(d04,3,2) + '/' + substring(d04,5,2) end	as d04										\n"
					+ "      ,case d05 when '' then '' else substring(d05,3,2) + '/' + substring(d05,5,2) end	as d05										\n"
					+ "      ,case d06 when '' then '' else substring(d06,3,2) + '/' + substring(d06,5,2) end	as d06										\n"
					+ "      ,case d07 when '' then '' else substring(d07,3,2) + '/' + substring(d07,5,2) end	as d07										\n"		
					+ "      ,case d08 when '' then '' else substring(d08,3,2) + '/' + substring(d08,5,2) end	as d08										\n" 					
					+ "      ,case d09 when '' then '' else substring(d09,3,2) + '/' + substring(d09,5,2) end	as d09									    \n"
					+ "      ,case d10 when '' then '' else substring(d10,3,2) + '/' + substring(d10,5,2) end	as d10										\n"
					+ "      ,case d11 when '' then '' else substring(d11,3,2) + '/' + substring(d11,5,2) end	as d11										\n"
					+ "      ,case d12 when '' then '' else substring(d12,3,2) + '/' + substring(d12,5,2) end	as d12										\n"
					+ "      ,case d13 when '' then '' else substring(d13,3,2) + '/' + substring(d13,5,2) end	as d13										\n"
					+ "      ,case d14 when '' then '' else substring(d14,3,2) + '/' + substring(d14,5,2) end	as d14										\n"		
					+ "      ,case d15 when '' then '' else substring(d15,3,2) + '/' + substring(d15,5,2) end	as d15										\n" 					
					+ "      ,case d16 when '' then '' else substring(d16,3,2) + '/' + substring(d16,5,2) end	as d16									    \n"
					+ "      ,case d17 when '' then '' else substring(d17,3,2) + '/' + substring(d17,5,2) end	as d17										\n"
					+ "      ,case d18 when '' then '' else substring(d18,3,2) + '/' + substring(d18,5,2) end	as d18										\n"
					+ "      ,case d19 when '' then '' else substring(d19,3,2) + '/' + substring(d19,5,2) end	as d19										\n"
					+ "      ,case d20 when '' then '' else substring(d20,3,2) + '/' + substring(d20,5,2) end	as d20										\n"
					+ "      ,case d21 when '' then '' else substring(d21,3,2) + '/' + substring(d21,5,2) end	as d21										\n"		
					+ "      ,case d22 when '' then '' else substring(d22,3,2) + '/' + substring(d22,5,2) end	as d22										\n" 					
					+ "      ,case d23 when '' then '' else substring(d23,3,2) + '/' + substring(d23,5,2) end	as d23									    \n"
					+ "      ,case d24 when '' then '' else substring(d24,3,2) + '/' + substring(d24,5,2) end	as d24										\n"
					+ "      ,case d25 when '' then '' else substring(d25,3,2) + '/' + substring(d25,5,2) end	as d25										\n"
					+ "      ,case d26 when '' then '' else substring(d26,3,2) + '/' + substring(d26,5,2) end	as d26										\n"
					+ "      ,case d27 when '' then '' else substring(d27,3,2) + '/' + substring(d27,5,2) end	as d27										\n"
					+ "      ,case d28 when '' then '' else substring(d28,3,2) + '/' + substring(d28,5,2) end	as d28										\n"
					+ "      ,case d29 when '' then '' else substring(d29,3,2) + '/' + substring(d29,5,2) end	as d29										\n"
					+ "      ,case d30 when '' then '' else substring(d30,3,2) + '/' + substring(d30,5,2) end	as d30										\n"
					+ "      ,case d31 when '' then '' else substring(d31,3,2) + '/' + substring(d31,5,2) end	as d31										\n"
					+ "from (																							\n"
					+ "			  select  max(case v2.rowNum when 1 then v2.LogDay	else '' end)	as d01			\n"
					+ "					, max(case v2.rowNum when 2 then v2.LogDay	else '' end)	as d02			\n"
					+ "					, max(case v2.rowNum when 3 then v2.LogDay	else '' end)	as d03			\n"
					+ "					, max(case v2.rowNum when 4 then v2.LogDay	else '' end)	as d04			\n"
					+ "					, max(case v2.rowNum when 5 then v2.LogDay	else '' end)	as d05			\n"
					+ "					, max(case v2.rowNum when 6 then v2.LogDay	else '' end)	as d06			\n"
					+ "					, max(case v2.rowNum when 7 then v2.LogDay	else '' end)	as d07			\n"
					+ "					, max(case v2.rowNum when 8 then v2.LogDay	else '' end)	as d08			\n"
					+ "					, max(case v2.rowNum when 9 then v2.LogDay	else '' end)	as d09			\n"
					+ "					, max(case v2.rowNum when 10 then v2.LogDay else '' end)	as d10			\n"
					+ "					, max(case v2.rowNum when 11 then v2.LogDay else '' end)	as d11			\n"
					+ "					, max(case v2.rowNum when 12 then v2.LogDay else '' end)	as d12			\n"
					+ "					, max(case v2.rowNum when 13 then v2.LogDay else '' end)	as d13			\n"
					+ "					, max(case v2.rowNum when 14 then v2.LogDay else '' end)	as d14			\n"
					+ "					, max(case v2.rowNum when 15 then v2.LogDay else '' end)	as d15			\n"
					+ "					, max(case v2.rowNum when 16 then v2.LogDay else '' end)	as d16			\n"
					+ "					, max(case v2.rowNum when 17 then v2.LogDay else '' end)	as d17			\n"
					+ "					, max(case v2.rowNum when 18 then v2.LogDay else '' end)	as d18			\n"
					+ "					, max(case v2.rowNum when 19 then v2.LogDay else '' end)	as d19			\n"
					+ "					, max(case v2.rowNum when 20 then v2.LogDay else '' end)	as d20			\n"
					+ "					, max(case v2.rowNum when 21 then v2.LogDay else '' end)	as d21			\n"
					+ "					, max(case v2.rowNum when 22 then v2.LogDay else '' end)	as d22			\n"
					+ "					, max(case v2.rowNum when 23 then v2.LogDay else '' end)	as d23			\n"
					+ "					, max(case v2.rowNum when 24 then v2.LogDay else '' end)	as d24			\n"
					+ "					, max(case v2.rowNum when 25 then v2.LogDay else '' end)	as d25			\n"
					+ "					, max(case v2.rowNum when 26 then v2.LogDay else '' end)	as d26			\n"
					+ "					, max(case v2.rowNum when 27 then v2.LogDay else '' end)	as d27			\n"
					+ "					, max(case v2.rowNum when 28 then v2.LogDay else '' end)	as d28			\n"
					+ "					, max(case v2.rowNum when 29 then v2.LogDay else '' end)	as d29			\n"
					+ "					, max(case v2.rowNum when 30 then v2.LogDay else '' end)	as d30			\n"
					+ "					, max(case v2.rowNum when 31 then v2.LogDay else '' end)	as d31			\n"					
					+ "      from (	select LogDay,row_number() over(order by LogDay) as rowNum					    \n"
					+ "					from	SummaryBase base with(nolock)										\n"
					+ "					where	base.LogDay between '" + data.StartDay + "' and '" + data.EndDay + "' ) v2 ) y1					\n"			
					+ "		 union all																				\n"							
					+ "      select	y2.Category																		\n"
                    + "      		, (select CategoryName from AdTargetsHanaTV.dbo.Category where CategoryCode = y2.Category)																\n" 
					+ "      		, y2.Genre																															\n"
                    + "      		, (select GenreName    from AdTargetsHanaTV.dbo.Genre where GenreCode = y2.Genre)																			\n" 
					+ "      		, y2.Progkey																														\n"
                    + "      		, (select ProgramNm    from AdTargetsHanaTV.dbo.Program  where Programkey= y2.Progkey)       as ChannelNm												\n" 
					+ "      		, replace( Convert( varchar(12),Convert(money,y2.d01),1),'.00','')	as d01															\n"
					+ "      		, replace( Convert( varchar(12),Convert(money,y2.d02),1),'.00','')	as d02															\n" 
					+ "      		, replace( Convert( varchar(12),Convert(money,y2.d03),1),'.00','')	as d03															\n"
					+ "      		, replace( Convert( varchar(12),Convert(money,y2.d04),1),'.00','')	as d04															\n" 
					+ "      		, replace( Convert( varchar(12),Convert(money,y2.d05),1),'.00','')	as d05															\n"
					+ "      		, replace( Convert( varchar(12),Convert(money,y2.d06),1),'.00','')	as d06															\n" 
					+ "      		, replace( Convert( varchar(12),Convert(money,y2.d07),1),'.00','')	as d07															\n"
					+ "      		, replace( Convert( varchar(12),Convert(money,y2.d08),1),'.00','')	as d08															\n" 
					+ "      		, replace( Convert( varchar(12),Convert(money,y2.d09),1),'.00','')	as d09															\n"
					+ "      		, replace( Convert( varchar(12),Convert(money,y2.d10),1),'.00','')	as d10															\n" 
					+ "      		, replace( Convert( varchar(12),Convert(money,y2.d11),1),'.00','')	as d11															\n"
					+ "      		, replace( Convert( varchar(12),Convert(money,y2.d12),1),'.00','')	as d12															\n" 
					+ "      		, replace( Convert( varchar(12),Convert(money,y2.d13),1),'.00','')	as d13															\n"
					+ "      		, replace( Convert( varchar(12),Convert(money,y2.d14),1),'.00','')	as d14															\n" 
					+ "      		, replace( Convert( varchar(12),Convert(money,y2.d15),1),'.00','')	as d15															\n"
					+ "      		, replace( Convert( varchar(12),Convert(money,y2.d16),1),'.00','')	as d16															\n" 
					+ "      		, replace( Convert( varchar(12),Convert(money,y2.d17),1),'.00','')	as d17															\n"
					+ "      		, replace( Convert( varchar(12),Convert(money,y2.d18),1),'.00','')	as d18															\n" 
					+ "      		, replace( Convert( varchar(12),Convert(money,y2.d19),1),'.00','')	as d19															\n"
					+ "      		, replace( Convert( varchar(12),Convert(money,y2.d20),1),'.00','')	as d20															\n" 
					+ "      		, replace( Convert( varchar(12),Convert(money,y2.d21),1),'.00','')	as d21															\n"
					+ "      		, replace( Convert( varchar(12),Convert(money,y2.d22),1),'.00','')	as d22															\n" 
					+ "      		, replace( Convert( varchar(12),Convert(money,y2.d23),1),'.00','')	as d23															\n"
					+ "      		, replace( Convert( varchar(12),Convert(money,y2.d24),1),'.00','')	as d24															\n" 
					+ "      		, replace( Convert( varchar(12),Convert(money,y2.d25),1),'.00','')	as d25															\n"
					+ "      		, replace( Convert( varchar(12),Convert(money,y2.d26),1),'.00','')	as d26															\n" 
					+ "      		, replace( Convert( varchar(12),Convert(money,y2.d27),1),'.00','')	as d27															\n"
					+ "      		, replace( Convert( varchar(12),Convert(money,y2.d28),1),'.00','')	as d28															\n" 
					+ "      		, replace( Convert( varchar(12),Convert(money,y2.d29),1),'.00','')	as d29															\n"
					+ "      		, replace( Convert( varchar(12),Convert(money,y2.d30),1),'.00','')	as d30															\n" 
					+ "      		, replace( Convert( varchar(12),Convert(money,y2.d31),1),'.00','')	as d31															\n"
					+ "		from (																																		\n"
					+ "      		select		 v1.Category																													\n"
					+ "      					,v1.Genre																														\n" 
					+ "      					,v1.Progkey																														\n" 
					+ "      					, sum(case v2.rowNum when 1 then v1.adCnt else 0 end)		as d01																\n" 
					+ "      					, sum(case v2.rowNum when 2 then v1.adCnt else 0 end)		as d02																\n"
					+ "      					, sum(case v2.rowNum when 3 then v1.adCnt else 0 end)		as d03																\n" 
					+ "      					, sum(case v2.rowNum when 4 then v1.adCnt else 0 end)		as d04																\n"
					+ "      					, sum(case v2.rowNum when 5 then v1.adCnt else 0 end)		as d05																\n" 
					+ "      					, sum(case v2.rowNum when 6 then v1.adCnt else 0 end)		as d06																\n"
					+ "      					, sum(case v2.rowNum when 7 then v1.adCnt else 0 end)		as d07																\n" 
					+ "      					, sum(case v2.rowNum when 8 then v1.adCnt else 0 end)		as d08																\n"
					+ "      					, sum(case v2.rowNum when 9 then v1.adCnt else 0 end)		as d09																\n" 
					+ "      					, sum(case v2.rowNum when 10 then v1.adCnt else 0 end)	as d10																	\n"
					+ "      					, sum(case v2.rowNum when 11 then v1.adCnt else 0 end)	as d11																	\n" 
					+ "      					, sum(case v2.rowNum when 12 then v1.adCnt else 0 end)	as d12																	\n"
					+ "      					, sum(case v2.rowNum when 13 then v1.adCnt else 0 end)	as d13																	\n" 
					+ "      					, sum(case v2.rowNum when 14 then v1.adCnt else 0 end)	as d14																	\n"
					+ "      					, sum(case v2.rowNum when 15 then v1.adCnt else 0 end)	as d15																	\n" 
					+ "      					, sum(case v2.rowNum when 16 then v1.adCnt else 0 end)	as d16																	\n"
					+ "      					, sum(case v2.rowNum when 17 then v1.adCnt else 0 end)	as d17																	\n" 
					+ "      					, sum(case v2.rowNum when 18 then v1.adCnt else 0 end)	as d18																	\n"
					+ "      					, sum(case v2.rowNum when 19 then v1.adCnt else 0 end)	as d19																	\n" 
					+ "      					, sum(case v2.rowNum when 20 then v1.adCnt else 0 end)	as d20																	\n"
					+ "      					, sum(case v2.rowNum when 21 then v1.adCnt else 0 end)	as d21																	\n" 
					+ "      					, sum(case v2.rowNum when 22 then v1.adCnt else 0 end)	as d22																	\n"
					+ "      					, sum(case v2.rowNum when 23 then v1.adCnt else 0 end)	as d23																	\n" 
					+ "      					, sum(case v2.rowNum when 24 then v1.adCnt else 0 end)	as d24																	\n"
					+ "      					, sum(case v2.rowNum when 25 then v1.adCnt else 0 end)	as d25																	\n" 
					+ "      					, sum(case v2.rowNum when 26 then v1.adCnt else 0 end)	as d26																	\n"
					+ "      					, sum(case v2.rowNum when 27 then v1.adCnt else 0 end)	as d27																	\n" 
					+ "      					, sum(case v2.rowNum when 28 then v1.adCnt else 0 end)	as d28																	\n"
					+ "      					, sum(case v2.rowNum when 29 then v1.adCnt else 0 end)	as d29																	\n" 
					+ "      					, sum(case v2.rowNum when 30 then v1.adCnt else 0 end)	as d30																	\n"
					+ "      					, sum(case v2.rowNum when 31 then v1.adCnt else 0 end)	as d31																	\n" 
					+ "				from (	select   LogDay															    														\n"
					+ "							    ,Category															    														\n"
					+ "							    ,Genre															    														\n"
					+ "							    ,Progkey															    														\n"
					+ "							    ,sum(AdCnt) as adCnt															    													\n"
					+ "						from	SummaryAdDaily3 a with(noLock)															    										\n"
                    + "                     where   a.LogDay between '" + data.StartDay + "' and '" + data.EndDay + "' " + "\n"
                    + "                     and     a.ItemNo in(" + sqlItems + ") " + "\n"
					+ "						and     a.ContractSeq = 0															    														\n"
					+ "						and		a.progKey > 0															    														\n"
					+ "						and		a.Category> 0															    														\n"
					+ "						and		a.genre		> 0															    														\n"
					+ "						and		a.Progkey		> 0															    													\n"
					+ "						group by LogDay,Category,Genre,Progkey ) v1															    											\n"
					+ "				inner join 															    																			\n"
					+ "					(	select LogDay,row_number() over(order by LogDay) as rowNum 															    					\n"
					+ "						from	SummaryBase base with(nolock) 															    										\n"
					+ "						where	base.LogDay between '" + data.StartDay + "' and '" + data.EndDay + "') v2 on v1.LogDay = v2.LogDay 															\n"
					+ "						group by v1.Category, v1.Genre,v1.Progkey ) y2 															    										\n"										
                    + "         order by Category,Genre;" + "\n");

			
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
				_log.Debug(this.ToString() + "GetDateChannel() End");
				_log.Debug("-----------------------------------------");
			}
			catch(Exception ex)
			{
				data.ResultCD = "3000";
				data.ResultDesc = "기간별 채널집계 조회중 오류가 발생하였습니다";
				_log.Exception(ex);
			}
			finally
			{
				// 데이터베이스를  Close한다
				_db.Close();
			}

		}
		#endregion

		#region 기간별 장르집계(자바용)
		/// <summary>
		/// 광고별 시청현황 집계(자바용)
		/// </summary>
		/// <param name="dateSummaryModel"></param>
		public void GetDateGenreByJava(HeaderModel header, DateSummaryModel data)
		{

			try
			{
				StringBuilder sbQuery = null;

				// 데이터베이스를 OPEN한다
				_db.Open();

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetDateGenre() Start");
				_log.Debug("-----------------------------------------");

				// 일자가 6자리 이상이면 6자리로 만든다.
				if(data.StartDay.Length > 6) data.StartDay = data.StartDay.Substring(2,6);
				if(data.EndDay.Length > 6) data.EndDay = data.EndDay.Substring(2,6);

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
				_log.Debug("StartDay :[" + data.StartDay    + "]");		// 검색 집계일자           
				_log.Debug("EndDay   :[" + data.EndDay      + "]");		// 검색 키워드   
				_log.Debug("Items    :[" + sqlItems                     + "]");
				// __DEBUG__

				// 쿼리생성
				sbQuery = new StringBuilder();
				sbQuery.Append("\n");
				sbQuery.Append(" SELECT 0           as category " + "\n");
				sbQuery.Append("       ,'카테고리'  as categoryNm " + "\n");
				sbQuery.Append("       ,0 as Genre " + "\n");
				sbQuery.Append("       ,'장르'  as GenreNm " + "\n");
				sbQuery.Append("       ,case d01 when '' then '' else substring(d01,3,2) + '/' + substring(d01,5,2) end	as d01 " + "\n");
				sbQuery.Append("       ,case d02 when '' then '' else substring(d02,3,2) + '/' + substring(d02,5,2) end	as d02 " + "\n");
				sbQuery.Append("       ,case d03 when '' then '' else substring(d03,3,2) + '/' + substring(d03,5,2) end	as d03 " + "\n");
				sbQuery.Append("       ,case d04 when '' then '' else substring(d04,3,2) + '/' + substring(d04,5,2) end	as d04 " + "\n");
				sbQuery.Append("       ,case d05 when '' then '' else substring(d05,3,2) + '/' + substring(d05,5,2) end	as d05 " + "\n");
				sbQuery.Append("       ,case d06 when '' then '' else substring(d06,3,2) + '/' + substring(d06,5,2) end	as d06 " + "\n");
				sbQuery.Append("       ,case d07 when '' then '' else substring(d07,3,2) + '/' + substring(d07,5,2) end	as d07 " + "\n");
				sbQuery.Append("       ,case d08 when '' then '' else substring(d08,3,2) + '/' + substring(d08,5,2) end	as d08 " + "\n");
				sbQuery.Append("       ,case d09 when '' then '' else substring(d09,3,2) + '/' + substring(d09,5,2) end	as d09 " + "\n");
				sbQuery.Append("       ,case d10 when '' then '' else substring(d10,3,2) + '/' + substring(d10,5,2) end	as d10 " + "\n");
				sbQuery.Append("       ,case d11 when '' then '' else substring(d11,3,2) + '/' + substring(d11,5,2) end	as d11 " + "\n");
				sbQuery.Append("       ,case d12 when '' then '' else substring(d12,3,2) + '/' + substring(d12,5,2) end	as d12 " + "\n");
				sbQuery.Append("       ,case d13 when '' then '' else substring(d13,3,2) + '/' + substring(d13,5,2) end	as d13 " + "\n");
				sbQuery.Append("       ,case d14 when '' then '' else substring(d14,3,2) + '/' + substring(d14,5,2) end	as d14 " + "\n");
				sbQuery.Append("       ,case d15 when '' then '' else substring(d15,3,2) + '/' + substring(d15,5,2) end	as d15 " + "\n");
				sbQuery.Append("       ,case d16 when '' then '' else substring(d16,3,2) + '/' + substring(d16,5,2) end	as d16 " + "\n");
				sbQuery.Append("       ,case d17 when '' then '' else substring(d17,3,2) + '/' + substring(d17,5,2) end	as d17 " + "\n");
				sbQuery.Append("       ,case d18 when '' then '' else substring(d18,3,2) + '/' + substring(d18,5,2) end	as d18 " + "\n");
				sbQuery.Append("       ,case d19 when '' then '' else substring(d19,3,2) + '/' + substring(d19,5,2) end	as d19 " + "\n");
				sbQuery.Append("       ,case d20 when '' then '' else substring(d20,3,2) + '/' + substring(d20,5,2) end	as d20 " + "\n");
				sbQuery.Append("       ,case d21 when '' then '' else substring(d21,3,2) + '/' + substring(d21,5,2) end	as d21 " + "\n");
				sbQuery.Append("       ,case d22 when '' then '' else substring(d22,3,2) + '/' + substring(d22,5,2) end	as d22 " + "\n");
				sbQuery.Append("       ,case d23 when '' then '' else substring(d23,3,2) + '/' + substring(d23,5,2) end	as d23 " + "\n");
				sbQuery.Append("       ,case d24 when '' then '' else substring(d24,3,2) + '/' + substring(d24,5,2) end	as d24 " + "\n");
				sbQuery.Append("       ,case d25 when '' then '' else substring(d25,3,2) + '/' + substring(d25,5,2) end	as d25 " + "\n");
				sbQuery.Append("       ,case d26 when '' then '' else substring(d26,3,2) + '/' + substring(d26,5,2) end	as d26 " + "\n");
				sbQuery.Append("       ,case d27 when '' then '' else substring(d27,3,2) + '/' + substring(d27,5,2) end	as d27 " + "\n");
				sbQuery.Append("       ,case d28 when '' then '' else substring(d28,3,2) + '/' + substring(d28,5,2) end	as d28 " + "\n");
				sbQuery.Append("       ,case d29 when '' then '' else substring(d29,3,2) + '/' + substring(d29,5,2) end	as d29 " + "\n");
				sbQuery.Append("       ,case d30 when '' then '' else substring(d30,3,2) + '/' + substring(d30,5,2) end	as d30 " + "\n");
				sbQuery.Append("       ,case d31 when '' then '' else substring(d31,3,2) + '/' + substring(d31,5,2) end	as d31 " + "\n");
				sbQuery.Append(" from   (   select    max(case v2.rowNum when 1 then v2.LogDay	else '' end)	as d01 " + "\n");
				sbQuery.Append("                    , max(case v2.rowNum when 2 then v2.LogDay	else '' end)	as d02 " + "\n");
				sbQuery.Append(" 	                , max(case v2.rowNum when 3 then v2.LogDay	else '' end)	as d03 " + "\n");
				sbQuery.Append(" 	                , max(case v2.rowNum when 4 then v2.LogDay	else '' end)	as d04 " + "\n");
				sbQuery.Append(" 	                , max(case v2.rowNum when 5 then v2.LogDay	else '' end)	as d05 " + "\n");
				sbQuery.Append(" 	                , max(case v2.rowNum when 6 then v2.LogDay	else '' end)	as d06 " + "\n");
				sbQuery.Append(" 					, max(case v2.rowNum when 7 then v2.LogDay	else '' end)	as d07 " + "\n");
				sbQuery.Append(" 					, max(case v2.rowNum when 8 then v2.LogDay	else '' end)	as d08 " + "\n");
				sbQuery.Append(" 					, max(case v2.rowNum when 9 then v2.LogDay	else '' end)	as d09 " + "\n");
				sbQuery.Append(" 					, max(case v2.rowNum when 10 then v2.LogDay else '' end)	as d10 " + "\n");
				sbQuery.Append(" 					, max(case v2.rowNum when 11 then v2.LogDay else '' end)	as d11 " + "\n");
				sbQuery.Append(" 					, max(case v2.rowNum when 12 then v2.LogDay else '' end)	as d12 " + "\n");
				sbQuery.Append("    				, max(case v2.rowNum when 13 then v2.LogDay else '' end)	as d13 " + "\n");
				sbQuery.Append(" 					, max(case v2.rowNum when 14 then v2.LogDay else '' end)	as d14 " + "\n");
				sbQuery.Append(" 					, max(case v2.rowNum when 15 then v2.LogDay else '' end)	as d15 " + "\n");
				sbQuery.Append(" 					, max(case v2.rowNum when 16 then v2.LogDay else '' end)	as d16 " + "\n");
				sbQuery.Append(" 					, max(case v2.rowNum when 17 then v2.LogDay else '' end)	as d17 " + "\n");
				sbQuery.Append(" 					, max(case v2.rowNum when 18 then v2.LogDay else '' end)	as d18 " + "\n");
				sbQuery.Append(" 					, max(case v2.rowNum when 19 then v2.LogDay else '' end)	as d19 " + "\n");
				sbQuery.Append(" 					, max(case v2.rowNum when 20 then v2.LogDay else '' end)	as d20 " + "\n");
				sbQuery.Append(" 					, max(case v2.rowNum when 21 then v2.LogDay else '' end)	as d21 " + "\n");
				sbQuery.Append(" 					, max(case v2.rowNum when 22 then v2.LogDay else '' end)	as d22 " + "\n");
				sbQuery.Append(" 					, max(case v2.rowNum when 23 then v2.LogDay else '' end)	as d23 " + "\n");
				sbQuery.Append(" 					, max(case v2.rowNum when 24 then v2.LogDay else '' end)	as d24 " + "\n");
				sbQuery.Append(" 					, max(case v2.rowNum when 25 then v2.LogDay else '' end)	as d25 " + "\n");
				sbQuery.Append(" 					, max(case v2.rowNum when 26 then v2.LogDay else '' end)	as d26 " + "\n");
				sbQuery.Append(" 					, max(case v2.rowNum when 27 then v2.LogDay else '' end)	as d27 " + "\n");
				sbQuery.Append(" 					, max(case v2.rowNum when 28 then v2.LogDay else '' end)	as d28 " + "\n");
				sbQuery.Append(" 					, max(case v2.rowNum when 29 then v2.LogDay else '' end)	as d29 " + "\n");
				sbQuery.Append(" 					, max(case v2.rowNum when 30 then v2.LogDay else '' end)	as d30 " + "\n");
				sbQuery.Append(" 					, max(case v2.rowNum when 31 then v2.LogDay else '' end)	as d31 " + "\n");
				sbQuery.Append(" 	        from (  select  LogDay,row_number() over(order by LogDay) as rowNum	 " + "\n");
				sbQuery.Append("                    from    SummaryBase base with(nolock) " + "\n");
				sbQuery.Append("                    where	base.LogDay between '" + data.StartDay + "' and '" + data.EndDay + "') v2 ) y1 " + "\n");
				sbQuery.Append("            union all " + "\n");
				sbQuery.Append("            select	  y2.Category " + "\n");
                sbQuery.Append("                    , (select CategoryName from AdTargetsHanaTV.dbo.Category where CategoryCode = y2.Category) " + "\n");
				sbQuery.Append(" 	          		, y2.Genre " + "\n");
                sbQuery.Append(" 	          		, (select GenreName from AdTargetsHanaTV.dbo.Genre where GenreCode = y2.Genre) " + "\n");
				sbQuery.Append(" 	          		, replace( Convert( varchar(12),Convert(money,y2.d01),1),'.00','')	as d01 " + "\n");
				sbQuery.Append(" 	          		, replace( Convert( varchar(12),Convert(money,y2.d02),1),'.00','')	as d02 " + "\n");
				sbQuery.Append(" 	        		, replace( Convert( varchar(12),Convert(money,y2.d03),1),'.00','')	as d03 " + "\n");
				sbQuery.Append(" 	          		, replace( Convert( varchar(12),Convert(money,y2.d04),1),'.00','')	as d04 " + "\n");
				sbQuery.Append(" 	          		, replace( Convert( varchar(12),Convert(money,y2.d05),1),'.00','')	as d05 " + "\n");
				sbQuery.Append(" 	          		, replace( Convert( varchar(12),Convert(money,y2.d06),1),'.00','')	as d06 " + "\n");
				sbQuery.Append(" 	          		, replace( Convert( varchar(12),Convert(money,y2.d07),1),'.00','')	as d07 " + "\n");
				sbQuery.Append(" 	          		, replace( Convert( varchar(12),Convert(money,y2.d08),1),'.00','')	as d08 " + "\n");
				sbQuery.Append(" 	          		, replace( Convert( varchar(12),Convert(money,y2.d09),1),'.00','')	as d09 " + "\n");
				sbQuery.Append(" 	          		, replace( Convert( varchar(12),Convert(money,y2.d10),1),'.00','')	as d10 " + "\n");
				sbQuery.Append(" 	          		, replace( Convert( varchar(12),Convert(money,y2.d11),1),'.00','')	as d11 " + "\n");
				sbQuery.Append(" 	          		, replace( Convert( varchar(12),Convert(money,y2.d12),1),'.00','')	as d12 " + "\n");
				sbQuery.Append(" 	          		, replace( Convert( varchar(12),Convert(money,y2.d13),1),'.00','')	as d13 " + "\n");
				sbQuery.Append(" 	          		, replace( Convert( varchar(12),Convert(money,y2.d14),1),'.00','')	as d14 " + "\n");
				sbQuery.Append(" 	          		, replace( Convert( varchar(12),Convert(money,y2.d15),1),'.00','')	as d15 " + "\n");
				sbQuery.Append(" 	          		, replace( Convert( varchar(12),Convert(money,y2.d16),1),'.00','')	as d16 " + "\n");
				sbQuery.Append(" 	          		, replace( Convert( varchar(12),Convert(money,y2.d17),1),'.00','')	as d17 " + "\n");
				sbQuery.Append(" 	          		, replace( Convert( varchar(12),Convert(money,y2.d18),1),'.00','')	as d18 " + "\n");
				sbQuery.Append(" 	          		, replace( Convert( varchar(12),Convert(money,y2.d19),1),'.00','')	as d19 " + "\n");
				sbQuery.Append(" 	          		, replace( Convert( varchar(12),Convert(money,y2.d20),1),'.00','')	as d20 " + "\n");
				sbQuery.Append(" 	          		, replace( Convert( varchar(12),Convert(money,y2.d21),1),'.00','')	as d21 " + "\n");
				sbQuery.Append(" 	          		, replace( Convert( varchar(12),Convert(money,y2.d22),1),'.00','')	as d22 " + "\n");
				sbQuery.Append(" 	          		, replace( Convert( varchar(12),Convert(money,y2.d23),1),'.00','')	as d23 " + "\n");
				sbQuery.Append(" 	          		, replace( Convert( varchar(12),Convert(money,y2.d24),1),'.00','')	as d24 " + "\n");
				sbQuery.Append(" 	          		, replace( Convert( varchar(12),Convert(money,y2.d25),1),'.00','')	as d25 " + "\n");
				sbQuery.Append(" 	          		, replace( Convert( varchar(12),Convert(money,y2.d26),1),'.00','')	as d26 " + "\n");
				sbQuery.Append(" 	          		, replace( Convert( varchar(12),Convert(money,y2.d27),1),'.00','')	as d27 " + "\n");
				sbQuery.Append(" 	          		, replace( Convert( varchar(12),Convert(money,y2.d28),1),'.00','')	as d28 " + "\n");
				sbQuery.Append(" 	          		, replace( Convert( varchar(12),Convert(money,y2.d29),1),'.00','')	as d29 " + "\n");
				sbQuery.Append(" 	          		, replace( Convert( varchar(12),Convert(money,y2.d30),1),'.00','')	as d30 " + "\n");
				sbQuery.Append(" 	          		, replace( Convert( varchar(12),Convert(money,y2.d31),1),'.00','')	as d31 " + "\n");
				sbQuery.Append(" 	        from (  select	 v1.Category " + "\n");
				sbQuery.Append(" 	                        ,v1.Genre " + "\n");
				sbQuery.Append(" 	      					, sum(case v2.rowNum when 1 then v1.adCnt else 0 end)	as d01 " + "\n");
				sbQuery.Append(" 	      					, sum(case v2.rowNum when 2 then v1.adCnt else 0 end)	as d02 " + "\n");
				sbQuery.Append(" 	      					, sum(case v2.rowNum when 3 then v1.adCnt else 0 end)	as d03 " + "\n");
				sbQuery.Append(" 	      					, sum(case v2.rowNum when 4 then v1.adCnt else 0 end)	as d04 " + "\n");
				sbQuery.Append(" 	      					, sum(case v2.rowNum when 5 then v1.adCnt else 0 end)	as d05 " + "\n");
				sbQuery.Append(" 	      					, sum(case v2.rowNum when 6 then v1.adCnt else 0 end)	as d06 " + "\n");
				sbQuery.Append(" 	      					, sum(case v2.rowNum when 7 then v1.adCnt else 0 end)	as d07 " + "\n");
				sbQuery.Append(" 	      					, sum(case v2.rowNum when 8 then v1.adCnt else 0 end)	as d08 " + "\n");
				sbQuery.Append(" 	      					, sum(case v2.rowNum when 9 then v1.adCnt else 0 end)	as d09 " + "\n");
				sbQuery.Append(" 	      					, sum(case v2.rowNum when 10 then v1.adCnt else 0 end)	as d10 " + "\n");
				sbQuery.Append(" 	      					, sum(case v2.rowNum when 11 then v1.adCnt else 0 end)	as d11 " + "\n");
				sbQuery.Append(" 	      					, sum(case v2.rowNum when 12 then v1.adCnt else 0 end)	as d12 " + "\n");
				sbQuery.Append(" 	      					, sum(case v2.rowNum when 13 then v1.adCnt else 0 end)	as d13 " + "\n");
				sbQuery.Append(" 	      					, sum(case v2.rowNum when 14 then v1.adCnt else 0 end)	as d14 " + "\n");
				sbQuery.Append(" 	      					, sum(case v2.rowNum when 15 then v1.adCnt else 0 end)	as d15 " + "\n");
				sbQuery.Append(" 	      					, sum(case v2.rowNum when 16 then v1.adCnt else 0 end)	as d16 " + "\n");
				sbQuery.Append(" 	      					, sum(case v2.rowNum when 17 then v1.adCnt else 0 end)	as d17 " + "\n");
				sbQuery.Append(" 	      					, sum(case v2.rowNum when 18 then v1.adCnt else 0 end)	as d18 " + "\n");
				sbQuery.Append(" 	      					, sum(case v2.rowNum when 19 then v1.adCnt else 0 end)	as d19 " + "\n");
				sbQuery.Append(" 	      					, sum(case v2.rowNum when 20 then v1.adCnt else 0 end)	as d20 " + "\n");
				sbQuery.Append(" 	      					, sum(case v2.rowNum when 21 then v1.adCnt else 0 end)	as d21 " + "\n");
				sbQuery.Append(" 	      					, sum(case v2.rowNum when 22 then v1.adCnt else 0 end)	as d22 " + "\n");
				sbQuery.Append(" 	      					, sum(case v2.rowNum when 23 then v1.adCnt else 0 end)	as d23 " + "\n");
				sbQuery.Append(" 	      					, sum(case v2.rowNum when 24 then v1.adCnt else 0 end)	as d24 " + "\n");
				sbQuery.Append(" 	      					, sum(case v2.rowNum when 25 then v1.adCnt else 0 end)	as d25 " + "\n");
				sbQuery.Append(" 	      					, sum(case v2.rowNum when 26 then v1.adCnt else 0 end)	as d26 " + "\n");
				sbQuery.Append(" 	      					, sum(case v2.rowNum when 27 then v1.adCnt else 0 end)	as d27 " + "\n");
				sbQuery.Append(" 	      					, sum(case v2.rowNum when 28 then v1.adCnt else 0 end)	as d28 " + "\n");
				sbQuery.Append(" 	      					, sum(case v2.rowNum when 29 then v1.adCnt else 0 end)	as d29 " + "\n");
				sbQuery.Append("         					, sum(case v2.rowNum when 30 then v1.adCnt else 0 end)	as d30 " + "\n");
				sbQuery.Append(" 	      					, sum(case v2.rowNum when 31 then v1.adCnt else 0 end)	as d31 " + "\n");
				sbQuery.Append(" 					from (	select   LogDay " + "\n");
				sbQuery.Append(" 								    ,Category " + "\n");
				sbQuery.Append(" 								    ,Genre " + "\n");
				sbQuery.Append(" 								    ,sum(AdCnt) as adCnt " + "\n");
				sbQuery.Append(" 					from	SummaryAdDaily3 a with(noLock) " + "\n");
				sbQuery.Append("                    where   a.LogDay between '" + data.StartDay + "' and '" + data.EndDay + "'  " + "\n");
				sbQuery.Append("                    and     a.ItemNo in(" + sqlItems + ")  " + "\n");
				sbQuery.Append(" 					and     a.ContractSeq = 0 " + "\n");
				sbQuery.Append(" 					and		a.progKey > 0 " + "\n");
				sbQuery.Append(" 					and		a.Category> 0 " + "\n");
				sbQuery.Append(" 					and		a.genre	  > 0 " + "\n");
				sbQuery.Append(" 					group by LogDay,Category,Genre ) v1 " + "\n");
				sbQuery.Append(" 			inner join " + "\n");
				sbQuery.Append(" 				(	select LogDay,row_number() over(order by LogDay) as rowNum " + "\n");
				sbQuery.Append(" 					from	SummaryBase base with(nolock) " + "\n");
				sbQuery.Append(" 					where	base.LogDay between '" + data.StartDay + "' and '" + data.EndDay + "' ) v2 on v1.LogDay = v2.LogDay " + "\n");
				sbQuery.Append(" 			group by v1.Category, v1.Genre ) y2 " + "\n");
				sbQuery.Append(" order by Category,Genre;" + "\n");
			
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
				_log.Debug(this.ToString() + "GetDateGenre() End");
				_log.Debug("-----------------------------------------");
			}
			catch(Exception ex)
			{
				data.ResultCD = "3000";
				data.ResultDesc = "기간별 장르집계 조회중 오류가 발생하였습니다";
				_log.Exception(ex);
			}
			finally
			{
				// 데이터베이스를  Close한다
				_db.Close();
			}

		}
		#endregion

		#region 기간별 채널집계(자바용)
		/// <summary>
		/// 광고별 시청현황 집계(자바용)
		/// </summary>
		/// <param name="dateSummaryModel"></param>
		public void GetDateChannelByJava(HeaderModel header, DateSummaryModel data)
		{

			try
			{
				StringBuilder sbQuery = null;

				// 데이터베이스를 OPEN한다
				_db.Open();


				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetDateChannel() Start");
				_log.Debug("-----------------------------------------");

				// 일자가 6자리 이상이면 6자리로 만든다.
				if(data.StartDay.Length > 6) data.StartDay = data.StartDay.Substring(2,6);
				if(data.EndDay.Length > 6) data.EndDay = data.EndDay.Substring(2,6);

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
				_log.Debug("StartDay :[" + data.StartDay    + "]");		// 검색 집계일자           
				_log.Debug("EndDay   :[" + data.EndDay      + "]");		// 검색 키워드   
				_log.Debug("Items    :[" + sqlItems                     + "]");
				// __DEBUG__

				// 쿼리생성
				sbQuery = new StringBuilder();
				sbQuery.Append("\n"
					+ "-- 광고별 시청현황        \n"					
					+ "SELECT 0					as category         -- 광고번호				\n"   					
					+ "      ,'카테고리'  as categoryNm										\n"		
					+ "      ,0 as Genre													\n"		
					+ "      ,'장르'  as GenreNm												\n"		
					+ "      ,0 as Progkey													\n"		
					+ "      ,'채널'  as ChannelNm											\n"		
					+ "      ,case d01 when '' then '' else substring(d01,3,2) + '/' + substring(d01,5,2) end	as d01										\n" 					
					+ "      ,case d02 when '' then '' else substring(d02,3,2) + '/' + substring(d02,5,2) end	as d02									    \n"
					+ "      ,case d03 when '' then '' else substring(d03,3,2) + '/' + substring(d03,5,2) end	as d03										\n"
					+ "      ,case d04 when '' then '' else substring(d04,3,2) + '/' + substring(d04,5,2) end	as d04										\n"
					+ "      ,case d05 when '' then '' else substring(d05,3,2) + '/' + substring(d05,5,2) end	as d05										\n"
					+ "      ,case d06 when '' then '' else substring(d06,3,2) + '/' + substring(d06,5,2) end	as d06										\n"
					+ "      ,case d07 when '' then '' else substring(d07,3,2) + '/' + substring(d07,5,2) end	as d07										\n"		
					+ "      ,case d08 when '' then '' else substring(d08,3,2) + '/' + substring(d08,5,2) end	as d08										\n" 					
					+ "      ,case d09 when '' then '' else substring(d09,3,2) + '/' + substring(d09,5,2) end	as d09									    \n"
					+ "      ,case d10 when '' then '' else substring(d10,3,2) + '/' + substring(d10,5,2) end	as d10										\n"
					+ "      ,case d11 when '' then '' else substring(d11,3,2) + '/' + substring(d11,5,2) end	as d11										\n"
					+ "      ,case d12 when '' then '' else substring(d12,3,2) + '/' + substring(d12,5,2) end	as d12										\n"
					+ "      ,case d13 when '' then '' else substring(d13,3,2) + '/' + substring(d13,5,2) end	as d13										\n"
					+ "      ,case d14 when '' then '' else substring(d14,3,2) + '/' + substring(d14,5,2) end	as d14										\n"		
					+ "      ,case d15 when '' then '' else substring(d15,3,2) + '/' + substring(d15,5,2) end	as d15										\n" 					
					+ "      ,case d16 when '' then '' else substring(d16,3,2) + '/' + substring(d16,5,2) end	as d16									    \n"
					+ "      ,case d17 when '' then '' else substring(d17,3,2) + '/' + substring(d17,5,2) end	as d17										\n"
					+ "      ,case d18 when '' then '' else substring(d18,3,2) + '/' + substring(d18,5,2) end	as d18										\n"
					+ "      ,case d19 when '' then '' else substring(d19,3,2) + '/' + substring(d19,5,2) end	as d19										\n"
					+ "      ,case d20 when '' then '' else substring(d20,3,2) + '/' + substring(d20,5,2) end	as d20										\n"
					+ "      ,case d21 when '' then '' else substring(d21,3,2) + '/' + substring(d21,5,2) end	as d21										\n"		
					+ "      ,case d22 when '' then '' else substring(d22,3,2) + '/' + substring(d22,5,2) end	as d22										\n" 					
					+ "      ,case d23 when '' then '' else substring(d23,3,2) + '/' + substring(d23,5,2) end	as d23									    \n"
					+ "      ,case d24 when '' then '' else substring(d24,3,2) + '/' + substring(d24,5,2) end	as d24										\n"
					+ "      ,case d25 when '' then '' else substring(d25,3,2) + '/' + substring(d25,5,2) end	as d25										\n"
					+ "      ,case d26 when '' then '' else substring(d26,3,2) + '/' + substring(d26,5,2) end	as d26										\n"
					+ "      ,case d27 when '' then '' else substring(d27,3,2) + '/' + substring(d27,5,2) end	as d27										\n"
					+ "      ,case d28 when '' then '' else substring(d28,3,2) + '/' + substring(d28,5,2) end	as d28										\n"
					+ "      ,case d29 when '' then '' else substring(d29,3,2) + '/' + substring(d29,5,2) end	as d29										\n"
					+ "      ,case d30 when '' then '' else substring(d30,3,2) + '/' + substring(d30,5,2) end	as d30										\n"
					+ "      ,case d31 when '' then '' else substring(d31,3,2) + '/' + substring(d31,5,2) end	as d31										\n"
					+ "from (																							\n"
					+ "			  select  max(case v2.rowNum when 1 then v2.LogDay	else '' end)	as d01			\n"
					+ "					, max(case v2.rowNum when 2 then v2.LogDay	else '' end)	as d02			\n"
					+ "					, max(case v2.rowNum when 3 then v2.LogDay	else '' end)	as d03			\n"
					+ "					, max(case v2.rowNum when 4 then v2.LogDay	else '' end)	as d04			\n"
					+ "					, max(case v2.rowNum when 5 then v2.LogDay	else '' end)	as d05			\n"
					+ "					, max(case v2.rowNum when 6 then v2.LogDay	else '' end)	as d06			\n"
					+ "					, max(case v2.rowNum when 7 then v2.LogDay	else '' end)	as d07			\n"
					+ "					, max(case v2.rowNum when 8 then v2.LogDay	else '' end)	as d08			\n"
					+ "					, max(case v2.rowNum when 9 then v2.LogDay	else '' end)	as d09			\n"
					+ "					, max(case v2.rowNum when 10 then v2.LogDay else '' end)	as d10			\n"
					+ "					, max(case v2.rowNum when 11 then v2.LogDay else '' end)	as d11			\n"
					+ "					, max(case v2.rowNum when 12 then v2.LogDay else '' end)	as d12			\n"
					+ "					, max(case v2.rowNum when 13 then v2.LogDay else '' end)	as d13			\n"
					+ "					, max(case v2.rowNum when 14 then v2.LogDay else '' end)	as d14			\n"
					+ "					, max(case v2.rowNum when 15 then v2.LogDay else '' end)	as d15			\n"
					+ "					, max(case v2.rowNum when 16 then v2.LogDay else '' end)	as d16			\n"
					+ "					, max(case v2.rowNum when 17 then v2.LogDay else '' end)	as d17			\n"
					+ "					, max(case v2.rowNum when 18 then v2.LogDay else '' end)	as d18			\n"
					+ "					, max(case v2.rowNum when 19 then v2.LogDay else '' end)	as d19			\n"
					+ "					, max(case v2.rowNum when 20 then v2.LogDay else '' end)	as d20			\n"
					+ "					, max(case v2.rowNum when 21 then v2.LogDay else '' end)	as d21			\n"
					+ "					, max(case v2.rowNum when 22 then v2.LogDay else '' end)	as d22			\n"
					+ "					, max(case v2.rowNum when 23 then v2.LogDay else '' end)	as d23			\n"
					+ "					, max(case v2.rowNum when 24 then v2.LogDay else '' end)	as d24			\n"
					+ "					, max(case v2.rowNum when 25 then v2.LogDay else '' end)	as d25			\n"
					+ "					, max(case v2.rowNum when 26 then v2.LogDay else '' end)	as d26			\n"
					+ "					, max(case v2.rowNum when 27 then v2.LogDay else '' end)	as d27			\n"
					+ "					, max(case v2.rowNum when 28 then v2.LogDay else '' end)	as d28			\n"
					+ "					, max(case v2.rowNum when 29 then v2.LogDay else '' end)	as d29			\n"
					+ "					, max(case v2.rowNum when 30 then v2.LogDay else '' end)	as d30			\n"
					+ "					, max(case v2.rowNum when 31 then v2.LogDay else '' end)	as d31			\n"					
					+ "      from (	select LogDay,row_number() over(order by LogDay) as rowNum					    \n"
					+ "					from	SummaryBase base with(nolock)										\n"
					+ "					where	base.LogDay between '" + data.StartDay + "' and '" + data.EndDay + "' ) v2 ) y1					\n"			
					+ "		 union all																				\n"							
					+ "      select	y2.Category																		\n"
                    + "      		, (select CategoryName from AdTargetsHanaTV.dbo.Category where CategoryCode = y2.Category)																\n" 
					+ "      		, y2.Genre																															\n"
                    + "      		, (select GenreName    from AdTargetsHanaTV.dbo.Genre where GenreCode = y2.Genre)																			\n" 
					+ "      		, y2.Progkey																														\n"
                    + "      		, (select ProgramNm    from AdTargetsHanaTV.dbo.Program  where Programkey= y2.Progkey)       as ChannelNm												\n" 
					+ "      		, replace( Convert( varchar(12),Convert(money,y2.d01),1),'.00','')	as d01															\n"
					+ "      		, replace( Convert( varchar(12),Convert(money,y2.d02),1),'.00','')	as d02															\n" 
					+ "      		, replace( Convert( varchar(12),Convert(money,y2.d03),1),'.00','')	as d03															\n"
					+ "      		, replace( Convert( varchar(12),Convert(money,y2.d04),1),'.00','')	as d04															\n" 
					+ "      		, replace( Convert( varchar(12),Convert(money,y2.d05),1),'.00','')	as d05															\n"
					+ "      		, replace( Convert( varchar(12),Convert(money,y2.d06),1),'.00','')	as d06															\n" 
					+ "      		, replace( Convert( varchar(12),Convert(money,y2.d07),1),'.00','')	as d07															\n"
					+ "      		, replace( Convert( varchar(12),Convert(money,y2.d08),1),'.00','')	as d08															\n" 
					+ "      		, replace( Convert( varchar(12),Convert(money,y2.d09),1),'.00','')	as d09															\n"
					+ "      		, replace( Convert( varchar(12),Convert(money,y2.d10),1),'.00','')	as d10															\n" 
					+ "      		, replace( Convert( varchar(12),Convert(money,y2.d11),1),'.00','')	as d11															\n"
					+ "      		, replace( Convert( varchar(12),Convert(money,y2.d12),1),'.00','')	as d12															\n" 
					+ "      		, replace( Convert( varchar(12),Convert(money,y2.d13),1),'.00','')	as d13															\n"
					+ "      		, replace( Convert( varchar(12),Convert(money,y2.d14),1),'.00','')	as d14															\n" 
					+ "      		, replace( Convert( varchar(12),Convert(money,y2.d15),1),'.00','')	as d15															\n"
					+ "      		, replace( Convert( varchar(12),Convert(money,y2.d16),1),'.00','')	as d16															\n" 
					+ "      		, replace( Convert( varchar(12),Convert(money,y2.d17),1),'.00','')	as d17															\n"
					+ "      		, replace( Convert( varchar(12),Convert(money,y2.d18),1),'.00','')	as d18															\n" 
					+ "      		, replace( Convert( varchar(12),Convert(money,y2.d19),1),'.00','')	as d19															\n"
					+ "      		, replace( Convert( varchar(12),Convert(money,y2.d20),1),'.00','')	as d20															\n" 
					+ "      		, replace( Convert( varchar(12),Convert(money,y2.d21),1),'.00','')	as d21															\n"
					+ "      		, replace( Convert( varchar(12),Convert(money,y2.d22),1),'.00','')	as d22															\n" 
					+ "      		, replace( Convert( varchar(12),Convert(money,y2.d23),1),'.00','')	as d23															\n"
					+ "      		, replace( Convert( varchar(12),Convert(money,y2.d24),1),'.00','')	as d24															\n" 
					+ "      		, replace( Convert( varchar(12),Convert(money,y2.d25),1),'.00','')	as d25															\n"
					+ "      		, replace( Convert( varchar(12),Convert(money,y2.d26),1),'.00','')	as d26															\n" 
					+ "      		, replace( Convert( varchar(12),Convert(money,y2.d27),1),'.00','')	as d27															\n"
					+ "      		, replace( Convert( varchar(12),Convert(money,y2.d28),1),'.00','')	as d28															\n" 
					+ "      		, replace( Convert( varchar(12),Convert(money,y2.d29),1),'.00','')	as d29															\n"
					+ "      		, replace( Convert( varchar(12),Convert(money,y2.d30),1),'.00','')	as d30															\n" 
					+ "      		, replace( Convert( varchar(12),Convert(money,y2.d31),1),'.00','')	as d31															\n"
					+ "		from (																																		\n"
					+ "      		select		 v1.Category																													\n"
					+ "      					,v1.Genre																														\n" 
					+ "      					,v1.Progkey																														\n" 
					+ "      					, sum(case v2.rowNum when 1 then v1.adCnt else 0 end)		as d01																\n" 
					+ "      					, sum(case v2.rowNum when 2 then v1.adCnt else 0 end)		as d02																\n"
					+ "      					, sum(case v2.rowNum when 3 then v1.adCnt else 0 end)		as d03																\n" 
					+ "      					, sum(case v2.rowNum when 4 then v1.adCnt else 0 end)		as d04																\n"
					+ "      					, sum(case v2.rowNum when 5 then v1.adCnt else 0 end)		as d05																\n" 
					+ "      					, sum(case v2.rowNum when 6 then v1.adCnt else 0 end)		as d06																\n"
					+ "      					, sum(case v2.rowNum when 7 then v1.adCnt else 0 end)		as d07																\n" 
					+ "      					, sum(case v2.rowNum when 8 then v1.adCnt else 0 end)		as d08																\n"
					+ "      					, sum(case v2.rowNum when 9 then v1.adCnt else 0 end)		as d09																\n" 
					+ "      					, sum(case v2.rowNum when 10 then v1.adCnt else 0 end)	as d10																	\n"
					+ "      					, sum(case v2.rowNum when 11 then v1.adCnt else 0 end)	as d11																	\n" 
					+ "      					, sum(case v2.rowNum when 12 then v1.adCnt else 0 end)	as d12																	\n"
					+ "      					, sum(case v2.rowNum when 13 then v1.adCnt else 0 end)	as d13																	\n" 
					+ "      					, sum(case v2.rowNum when 14 then v1.adCnt else 0 end)	as d14																	\n"
					+ "      					, sum(case v2.rowNum when 15 then v1.adCnt else 0 end)	as d15																	\n" 
					+ "      					, sum(case v2.rowNum when 16 then v1.adCnt else 0 end)	as d16																	\n"
					+ "      					, sum(case v2.rowNum when 17 then v1.adCnt else 0 end)	as d17																	\n" 
					+ "      					, sum(case v2.rowNum when 18 then v1.adCnt else 0 end)	as d18																	\n"
					+ "      					, sum(case v2.rowNum when 19 then v1.adCnt else 0 end)	as d19																	\n" 
					+ "      					, sum(case v2.rowNum when 20 then v1.adCnt else 0 end)	as d20																	\n"
					+ "      					, sum(case v2.rowNum when 21 then v1.adCnt else 0 end)	as d21																	\n" 
					+ "      					, sum(case v2.rowNum when 22 then v1.adCnt else 0 end)	as d22																	\n"
					+ "      					, sum(case v2.rowNum when 23 then v1.adCnt else 0 end)	as d23																	\n" 
					+ "      					, sum(case v2.rowNum when 24 then v1.adCnt else 0 end)	as d24																	\n"
					+ "      					, sum(case v2.rowNum when 25 then v1.adCnt else 0 end)	as d25																	\n" 
					+ "      					, sum(case v2.rowNum when 26 then v1.adCnt else 0 end)	as d26																	\n"
					+ "      					, sum(case v2.rowNum when 27 then v1.adCnt else 0 end)	as d27																	\n" 
					+ "      					, sum(case v2.rowNum when 28 then v1.adCnt else 0 end)	as d28																	\n"
					+ "      					, sum(case v2.rowNum when 29 then v1.adCnt else 0 end)	as d29																	\n" 
					+ "      					, sum(case v2.rowNum when 30 then v1.adCnt else 0 end)	as d30																	\n"
					+ "      					, sum(case v2.rowNum when 31 then v1.adCnt else 0 end)	as d31																	\n" 
					+ "				from (	select   LogDay															    														\n"
					+ "							    ,Category															    														\n"
					+ "							    ,Genre															    														\n"
					+ "							    ,Progkey															    														\n"
					+ "							    ,sum(AdCnt) as adCnt															    													\n"
					+ "						from	SummaryAdDaily3 a with(noLock)															    										\n"
					+ "                     where   a.LogDay between '" + data.StartDay + "' and '" + data.EndDay + "' " + "\n"
					+ "                     and     a.ItemNo in(" + sqlItems + ") " + "\n"
					+ "						and     a.ContractSeq = 0															    														\n"
					+ "						and		a.progKey > 0															    														\n"
					+ "						and		a.Category> 0															    														\n"
					+ "						and		a.genre		> 0															    														\n"
					+ "						and		a.Progkey		> 0															    													\n"
					+ "						group by LogDay,Category,Genre,Progkey ) v1															    											\n"
					+ "				inner join 															    																			\n"
					+ "					(	select LogDay,row_number() over(order by LogDay) as rowNum 															    					\n"
					+ "						from	SummaryBase base with(nolock) 															    										\n"
					+ "						where	base.LogDay between '" + data.StartDay + "' and '" + data.EndDay + "') v2 on v1.LogDay = v2.LogDay 															\n"
					+ "						group by v1.Category, v1.Genre,v1.Progkey ) y2 															    										\n"										
					+ "         order by Category,Genre;" + "\n");

			
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
				_log.Debug(this.ToString() + "GetDateChannel() End");
				_log.Debug("-----------------------------------------");
			}
			catch(Exception ex)
			{
				data.ResultCD = "3000";
				data.ResultDesc = "기간별 채널집계 조회중 오류가 발생하였습니다";
				_log.Exception(ex);
			}
			finally
			{
				// 데이터베이스를  Close한다
				_db.Close();
			}

		}
		#endregion

		#region 지역별 장르집계(자바용, 현재 사용안함. 추후 안정화 후 지울 것.)
		/// <summary>
		/// 지역별 장르집계(자바용, 현재 사용안함. 추후 안정화 후 지울 것.)
		/// </summary>
		/// <param name="dateSummaryModel"></param>
		public void GetChannelAreaByJava(HeaderModel header, DateSummaryModel data)
		{

			try
			{
				StringBuilder sbQuery = new StringBuilder();

				// 데이터베이스를 OPEN한다
				_db.Open();


				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetDateChannel() Start");
				_log.Debug("-----------------------------------------");

				// 일자가 6자리 이상이면 6자리로 만든다.
				if(data.StartDay.Length > 6) data.StartDay = data.StartDay.Substring(2,6);
				if(data.EndDay.Length > 6) data.EndDay = data.EndDay.Substring(2,6);

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
				_log.Debug("StartDay :[" + data.StartDay    + "]");		// 검색 집계일자           
				_log.Debug("EndDay   :[" + data.EndDay      + "]");		// 검색 키워드   
				_log.Debug("Items    :[" + sqlItems                     + "]");
				// __DEBUG__


				// 실제데이터 쿼리
				sbQuery.Length = 0;
				sbQuery.Append("SELECT a.SummaryCode, SummaryCode2, SummaryCode3																\n");
				sbQuery.Append("	, CategoryName Category																						\n");
				sbQuery.Append("	, GenreName Genre																							\n");
				sbQuery.Append("	, CONVERT(CHAR(3), REPLICATE(0, 3-LEN(SortNo)) + CONVERT(VARCHAR(3), SortNo)) CateOrdr						\n");
				sbQuery.Append("	, CONVERT(CHAR(3), REPLICATE(0, 3-LEN(SortOrder)) + CONVERT(VARCHAR(3), SortOrder)) GenreOrdr				\n");
				sbQuery.Append("	, (select b.SummaryName from SummaryCode b where SummaryType = 5 and b.SummaryCode = a.SummaryCode) Area	\n");
				sbQuery.Append("	, SUM(H00+H01+H02+H03+H04+H05+H06+H07+H08+H09+H10+H11+H12+H13+H14+H15+H16+H17+H18+H19+H20+H21+H22+H23) Hcnt	\n");
				sbQuery.Append("FROM SummaryAdDaily0_1 A with(NoLock)																			\n");
                sbQuery.Append("	inner join AdTargetsHanaTV.dbo.Category c on (CategoryCode = a.SummaryCode2)													\n");
                sbQuery.Append("	inner join AdTargetsHanaTV.dbo.Genre d on (GenreCode = a.SummaryCode3)															\n");
				sbQuery.Append("WHERE	a.LogDay	between  '" + data.StartDay + "' AND '" + data.EndDay + "'									\n");
				sbQuery.Append("	and a.ItemNo in(" + sqlItems + ")																			\n");
				sbQuery.Append("	and a.SummaryType = 6																						\n");
				sbQuery.Append("group by a.SummaryCode, SummaryCode2, SummaryCode3, CategoryName, GenreName, SortNo, SortOrder					\n");
				sbQuery.Append("union all																										\n");
				sbQuery.Append("SELECT b.UpperCode, SummaryCode2, SummaryCode3																	\n");
				sbQuery.Append("	, CategoryName Category																						\n");
				sbQuery.Append("	, GenreName Genre																							\n");
				sbQuery.Append("	, CONVERT(CHAR(3), REPLICATE(0, 3-LEN(SortNo)) + CONVERT(VARCHAR(3), SortNo)) CateOrdr						\n");
				sbQuery.Append("	, CONVERT(CHAR(3), REPLICATE(0, 3-LEN(SortOrder)) + CONVERT(VARCHAR(3), SortOrder)) GenreOrdr				\n");
				sbQuery.Append("	, (select c.SummaryName from SummaryCode c where SummaryType = 5 and b.UpperCode = c.SummaryCode) Area		\n");
				sbQuery.Append("	, SUM(H00+H01+H02+H03+H04+H05+H06+H07+H08+H09+H10+H11+H12+H13+H14+H15+H16+H17+H18+H19+H20+H21+H22+H23) Hcnt	\n");
				sbQuery.Append("FROM SummaryAdDaily0_1 A with(NoLock)																			\n");
				sbQuery.Append("	inner join SummaryCode b on (b.SummaryType = 5 and b.UpperCode > 0 and b.SummaryCode = a.SummaryCode)		\n");
                sbQuery.Append("	inner join AdTargetsHanaTV.dbo.Category c on (CategoryCode = a.SummaryCode2)													\n");
                sbQuery.Append("	inner join AdTargetsHanaTV.dbo.Genre d on (GenreCode = a.SummaryCode3)															\n");
				sbQuery.Append("WHERE	a.LogDay	between  '" + data.StartDay + "' AND '" + data.EndDay + "'									\n");
				sbQuery.Append("	and a.ItemNo in(" + sqlItems + ")																			\n");
				sbQuery.Append("	and a.SummaryType = 6																						\n");
				sbQuery.Append("group by b.UpperCode, SummaryCode2, SummaryCode3, CategoryName, GenreName, SortNo, SortOrder					\n");

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
				_log.Debug(this.ToString() + "GetDateChannel() End");
				_log.Debug("-----------------------------------------");
			}
			catch(Exception ex)
			{
				data.ResultCD = "3000";
				data.ResultDesc = "기간별 채널집계 조회중 오류가 발생하였습니다";
				_log.Exception(ex);
			}
			finally
			{
				// 데이터베이스를  Close한다
				_db.Close();
			}

		}
		#endregion

		#region 지역별 장르집계
		/// <summary>
		/// 지역별 장르집계
		/// </summary>
		/// <param name="dateSummaryModel"></param>
		public void GetChannelArea(HeaderModel header, DateSummaryModel data)
		{

			try
			{
				StringBuilder sbQuery = new StringBuilder();

				// 데이터베이스를 OPEN한다
				_db.Open();


				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetDateChannel() Start");
				_log.Debug("-----------------------------------------");

				// 일자가 6자리 이상이면 6자리로 만든다.
				if(data.StartDay.Length > 6) data.StartDay = data.StartDay.Substring(2,6);
				if(data.EndDay.Length > 6) data.EndDay = data.EndDay.Substring(2,6);

				string sqlItems = "";
				if( data.AdListStr.Length > 0 )
				{
					for( int i = 0; i < data.AdListStr.Length;i++)
					{
						if( i == 0 )    sqlItems = data.AdListStr[i];
						else            sqlItems += "," + data.AdListStr[i];
					}
				}

				sbQuery = new StringBuilder();
				sbQuery.Append("select SummaryCode, SummaryName from SummaryCode where SummaryType = 5");
				// __DEBUG__
				_log.Debug(sbQuery.ToString());
				// __DEBUG__

				// 쿼리실행
				DataSet ds = new DataSet();
				_db.ExecuteQuery(ds,sbQuery.ToString());

				// __DEBUG__
				_log.Debug("<입력정보>");
				_log.Debug("StartDay :[" + data.StartDay    + "]");		// 검색 집계일자           
				_log.Debug("EndDay   :[" + data.EndDay      + "]");		// 검색 키워드   
				_log.Debug("Items    :[" + sqlItems                     + "]");
				// __DEBUG__


				// 실제데이터 쿼리
				sbQuery.Length = 0;

				sbQuery.Append("select	\n");
				sbQuery.Append("	Category, Genre	\n");
				for(int i=0; i<ds.Tables[0].Rows.Count; i++)
				{
					sbQuery.Append(", c"+ i);
				}
				sbQuery.Append("	\n");
				sbQuery.Append("from (	\n");
				sbQuery.Append("	select	convert(varchar(20), '') Category, convert(varchar(30), '') Genre, '' CategoryCd, '' GenreCode, 0 SortNo, 0 SortOrder	\n");
				for(int i=0; i<ds.Tables[0].Rows.Count; i++)
				{
					sbQuery.Append(", '"+ ds.Tables[0].Rows[i][1] +"' c"+ i);
				}
				sbQuery.Append("	\n");
				sbQuery.Append("	union all	\n");
				sbQuery.Append("	SELECT	c.CategoryName		\n");
				sbQuery.Append("			, d.GenreName		\n");
				sbQuery.Append("			, SummaryCode2		\n");
				sbQuery.Append("			, SummaryCode3		\n");
				sbQuery.Append("			, c.SortNo		\n");
				sbQuery.Append("			, d.SortOrder		\n");
				for(int i=0; i<ds.Tables[0].Rows.Count; i++)
				{
					sbQuery.Append("		, replace(convert(varchar(20), convert(money, sum(case when SummaryCode = "+ ds.Tables[0].Rows[i][0] +" then Hcnt else 0 end)), 1), '.00', '')	\n");
				}
				sbQuery.Append("	FROM (		\n");
				sbQuery.Append("		SELECT	SummaryCode, SummaryCode2, SummaryCode3		\n");
				sbQuery.Append("			, SUM(H00+H01+H02+H03+H04+H05+H06+H07+H08+H09+H10+H11+H12+H13+H14+H15+H16+H17+H18+H19+H20+H21+H22+H23) Hcnt		\n");
				sbQuery.Append("		FROM SummaryAdDaily0_1 A with(NoLock)		\n");
				//sbQuery.Append("		WHERE	a.LogDay	between  '081101' AND '081102'			\n");
				//sbQuery.Append("			and a.ItemNo in(2965, 2926, 2750)		\n");
				sbQuery.Append("		WHERE	a.LogDay	between  '"+ data.StartDay +"' AND '"+ data.EndDay +"'		\n");
				sbQuery.Append("			and a.ItemNo in("+ sqlItems +")	\n");
				sbQuery.Append("			and a.SummaryType = 6		\n");
				sbQuery.Append("		group by SummaryCode, SummaryCode2, SummaryCode3		\n");
				sbQuery.Append("		union all		\n");
				sbQuery.Append("		SELECT b.UpperCode, SummaryCode2, SummaryCode3		\n");
				sbQuery.Append("			, SUM(H00+H01+H02+H03+H04+H05+H06+H07+H08+H09+H10+H11+H12+H13+H14+H15+H16+H17+H18+H19+H20+H21+H22+H23) Hcnt		\n");
				sbQuery.Append("		FROM SummaryAdDaily0_1 A with(NoLock)		\n");
				sbQuery.Append("			inner join SummaryCode b on (b.SummaryType = 5 and b.UpperCode > 0 and b.SummaryCode = a.SummaryCode)		\n");
				sbQuery.Append("		WHERE	a.LogDay	between  '"+ data.StartDay +"' AND '"+ data.EndDay +"'		\n");
				sbQuery.Append("			and a.ItemNo in("+ sqlItems +")	\n");
				//sbQuery.Append("		WHERE	a.LogDay	between  '081101' AND '081102'			\n");
				//sbQuery.Append("			and a.ItemNo in(2965, 2926, 2750)		\n");
				sbQuery.Append("			and a.SummaryType = 6		\n");
				sbQuery.Append("		group by b.UpperCode, SummaryCode2, SummaryCode3		\n");
				sbQuery.Append("	) B		\n");
                sbQuery.Append("		inner join AdTargetsHanaTV.dbo.Category c on (CategoryCode = B.SummaryCode2)			\n");
                sbQuery.Append("		inner join AdTargetsHanaTV.dbo.Genre d on (GenreCode = B.SummaryCode3)			\n");
				sbQuery.Append("	GROUP BY c.CategoryName, d.GenreName, SummaryCode2, SummaryCode3, c.SortNo, d.SortOrder		\n");
				sbQuery.Append(") xx	\n");
				sbQuery.Append("ORDER BY SortNo, SortOrder	\n");

				_log.Debug("Items    :[" + sbQuery.ToString()                     + "]");

				// 쿼리실행
				ds = new DataSet();	
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
				_log.Debug(this.ToString() + "GetDateChannel() End");
				_log.Debug("-----------------------------------------");
			}
			catch(Exception ex)
			{
				data.ResultCD = "3000";
				data.ResultDesc = "기간별 채널집계 조회중 오류가 발생하였습니다";
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