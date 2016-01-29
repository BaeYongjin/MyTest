// ===============================================================================
//
// RegionDateBiz.cs
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
 * Class Name: RegionDateBiz
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
	/// RegionDateBiz에 대한 요약 설명입니다.
	/// </summary>
	public class RegionDateBiz : BaseBiz
	{

		#region  생성자
		public RegionDateBiz() : base(FrameSystem.connSummaryDbString)
		{
			_log = FrameSystem.oLog;
		}
		#endregion

		#region 지역별 기간집계
		/// <summary>
		/// 광고별 시청현황 집계
		/// </summary>
		/// <param name="regionDateModel"></param>
		public void GetRegionDate(HeaderModel header, RegionDateModel regionDateModel)
		{

			try
			{
				StringBuilder sbQuery = null;

				// 데이터베이스를 OPEN한다
				_db.Open();


				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetRegionDate() Start");
				_log.Debug("-----------------------------------------");

				// 일자가 6자리 이상이면 6자리로 만든다.
				if(regionDateModel.StartDay.Length > 6) regionDateModel.StartDay = regionDateModel.StartDay.Substring(2,6);
				if(regionDateModel.EndDay.Length > 6) regionDateModel.EndDay = regionDateModel.EndDay.Substring(2,6);
				
				// __DEBUG__
				_log.Debug("<입력정보>");
				_log.Debug("ItemNo1	 :[" + regionDateModel.ItemNo1 + "]");		// 검색 매체
				_log.Debug("ItemNo2	 :[" + regionDateModel.ItemNo2 + "]");		// 검색 미디어렙
				_log.Debug("ItemNo3	 :[" + regionDateModel.ItemNo3 + "]");		// 검색 대행사
				_log.Debug("ItemNo4	 :[" + regionDateModel.ItemNo4 + "]");		// 검색 대행사
				_log.Debug("ItemNo5	 :[" + regionDateModel.ItemNo5 + "]");		// 검색 대행사
				_log.Debug("ItemNo6	 :[" + regionDateModel.ItemNo6 + "]");		// 검색 대행사
				_log.Debug("ItemNo7	 :[" + regionDateModel.ItemNo7 + "]");		// 검색 대행사
				_log.Debug("ItemNo8	 :[" + regionDateModel.ItemNo8 + "]");		// 검색 대행사
				_log.Debug("ItemNo9	 :[" + regionDateModel.ItemNo9 + "]");		// 검색 대행사
				_log.Debug("ItemNo10 :[" + regionDateModel.ItemNo10 + "]");		// 검색 대행사
				_log.Debug("StartDay :[" + regionDateModel.StartDay       + "]");		// 검색 집계일자           
				_log.Debug("EndDay   :[" + regionDateModel.EndDay       + "]");		// 검색 키워드           
				// __DEBUG__

				SqlParameter[] sqlParams = new SqlParameter[12];

				sqlParams[0] = new SqlParameter("@StartDay", SqlDbType.VarChar, 6);
				sqlParams[0].Value = regionDateModel.StartDay;

				sqlParams[1] = new SqlParameter("@EndDay", SqlDbType.VarChar, 6);
				sqlParams[1].Value = regionDateModel.EndDay;

				sqlParams[2] = new SqlParameter("@ItemNo1", SqlDbType.Int);
				sqlParams[2].Value = regionDateModel.ItemNo1;

				sqlParams[3] = new SqlParameter("@ItemNo2", SqlDbType.Int);
				sqlParams[3].Value = regionDateModel.ItemNo2;

				sqlParams[4] = new SqlParameter("@ItemNo3", SqlDbType.Int);
				sqlParams[4].Value = regionDateModel.ItemNo3;

				sqlParams[5] = new SqlParameter("@ItemNo4", SqlDbType.Int);
				sqlParams[5].Value = regionDateModel.ItemNo4;

				sqlParams[6] = new SqlParameter("@ItemNo5", SqlDbType.Int);
				sqlParams[6].Value = regionDateModel.ItemNo5;

				sqlParams[7] = new SqlParameter("@ItemNo6", SqlDbType.Int);
				sqlParams[7].Value = regionDateModel.ItemNo6;

				sqlParams[8] = new SqlParameter("@ItemNo7", SqlDbType.Int);
				sqlParams[8].Value = regionDateModel.ItemNo7;

				sqlParams[9] = new SqlParameter("@ItemNo8", SqlDbType.Int);
				sqlParams[9].Value = regionDateModel.ItemNo8;

				sqlParams[10] = new SqlParameter("@ItemNo9", SqlDbType.Int);
				sqlParams[10].Value = regionDateModel.ItemNo9;

				sqlParams[11] = new SqlParameter("@ItemNo10", SqlDbType.Int);
				sqlParams[11].Value = regionDateModel.ItemNo10;

				// 쿼리생성
				sbQuery = new StringBuilder();
				sbQuery.Append("\n"
					+ "-- 광고별 시청현황        \n"					
					+ "SELECT 0 as region        -- 광고번호																\n"   					
					+ "      ,'지역'  as RegionName								\n"							
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
					+ "					where	base.LogDay between @StartDay and @EndDay ) v2 ) y1					\n"			
					+ "		 union all																				\n"							
					+ "      select	 y2.region																															\n"
					+ "      		,( select summaryName from SummaryCode nolock where SummaryType = 5 and SummaryCode = y2.region) as RegionName						\n" 					
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
					+ "      		select		 v1.region																													\n"
					+ "      					,( select summaryName from SummaryCode nolock where SummaryType = 5 and SummaryCode = v1.region) as RegionName																													\n" 
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
					+ "				from (	select a.LogDay														    															\n"
					+ "							  ,case b.UpperCode when 0 then b.SummaryCode else b.UpperCode end  as region														    \n"					
					+ "							  ,sum(AdCnt) as adCnt															    													\n"
					+ "						from	SummaryAdDaily0 a with(noLock)															    										\n"
					+ "						inner join dbo.SummaryCode b with(NoLock) on a.Summarytype = b.SummaryType and a.SummaryCode = b.SummaryCode															    										\n"
					+ "						where	a.LogDay between @StartDay and @EndDay															    								\n"
					+ "						and   a.ItemNo in(@ItemNo1,@ItemNo2,@ItemNo3,@ItemNo4,@ItemNo5,@ItemNo6,@ItemNo7,@ItemNo8,@ItemNo9,@ItemNo10)															    												\n"
					+ "						and   a.ContractSeq = 0															    														\n"
					+ "						and	  a.SummaryType = 5															    														\n"				
					+ "						group by a.LogDay, case b.UpperCode when 0 then b.SummaryCode else b.UpperCode end ) v1															    												\n"
					+ "				inner join 															    																			\n"
					+ "					(	select LogDay,row_number() over(order by LogDay) as rowNum 															    					\n"
					+ "						from	SummaryBase base with(nolock) 															    										\n"
					+ "						where	base.LogDay between @StartDay and @EndDay ) v2 on v1.LogDay = v2.LogDay 															\n"
					+ "						group by v1.region ) y2 															    													\n"										
					);

			
				// __DEBUG__
				_log.Debug(sbQuery.ToString());
				// __DEBUG__
				
				// 쿼리실행
				DataSet ds = new DataSet();	
				_db.ExecuteQueryParams(ds,sbQuery.ToString(),sqlParams);

				// 결과 DataSet의 데이터모델에 복사
				regionDateModel.ReportDataSet = ds.Copy();

				// 결과
				regionDateModel.ResultCnt = Utility.GetDatasetCount(regionDateModel.ReportDataSet);

				ds.Dispose();


				// 결과코드 셋트
				regionDateModel.ResultCD = "0000";

				// __DEBUG__
				_log.Debug("<출력정보>");
				_log.Debug("ResultCnt:[" + regionDateModel.ResultCnt + "]");
				// __DEBUG__

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetRegionDate() End");
				_log.Debug("-----------------------------------------");
			}
			catch(Exception ex)
			{
				regionDateModel.ResultCD = "3000";
				regionDateModel.ResultDesc = "지역별 기간집계 조회중 오류가 발생하였습니다";
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