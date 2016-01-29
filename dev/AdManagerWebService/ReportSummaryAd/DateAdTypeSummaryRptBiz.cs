// ===============================================================================
//
// DateAdTypeSummaryRptBiz.cs
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
 * Class Name: DateAdTypeSummaryRptBiz
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
using System.Collections;

using WinFramework.Data;
using WinFramework.Misc;
using WinFramework.Base;

using AdManagerModel;

namespace AdManagerWebService.ReportSummaryAd
{
	/// <summary>
	/// DateAdTypeSummaryRptBiz에 대한 요약 설명입니다.
	/// </summary>
	public class DateAdTypeSummaryRptBiz : BaseBiz
	{

		#region  생성자
		public DateAdTypeSummaryRptBiz() : base(FrameSystem.connSummaryDbString)
		{
			_log = FrameSystem.oLog;
		}
		#endregion

		#region 일별 광고종류별 보고서
		/// <summary>
		///  총기간 광고집행 집계
		/// </summary>
		/// <param name="dateAdTypeSummaryRptModel"></param>
		public void GetDateAdTypeSummaryRpt(HeaderModel header, DateAdTypeSummaryRptModel dateAdTypeSummaryRptModel)
		{
			bool isNotReady = false; // 데이터가 집계가 되지않아 존재하지 않을때.
			try
			{
				StringBuilder sbQuery = null;

				// 데이터베이스를 OPEN한다
				_db.Open();


				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetDateAdTypeSummaryRpt() Start");
				_log.Debug("-----------------------------------------");

				// 일자가 6자리 이상(yyyymmdd)이면 6자리로 만든다.
				if(dateAdTypeSummaryRptModel.LogDay1.Length > 6) dateAdTypeSummaryRptModel.LogDay1 = dateAdTypeSummaryRptModel.LogDay1.Substring(2,6);
				if(dateAdTypeSummaryRptModel.LogDay2.Length > 6) dateAdTypeSummaryRptModel.LogDay2 = dateAdTypeSummaryRptModel.LogDay2.Substring(2,6);			
				
				// __DEBUG__
				_log.Debug("<입력정보>");
				_log.Debug("LogDay1	  :[" + dateAdTypeSummaryRptModel.LogDay1   + "]");		// 검색 매체
				_log.Debug("LogDay2	  :[" + dateAdTypeSummaryRptModel.LogDay2   + "]");					
				// __DEBUG__

				string logDay1   = dateAdTypeSummaryRptModel.LogDay1;
				string logDay2   = dateAdTypeSummaryRptModel.LogDay2;
				
				SqlParameter[] sqlParams = new SqlParameter[1];
				sqlParams[0] = new SqlParameter("@LogDay1", SqlDbType.VarChar, 6);
				sqlParams[0].Value = dateAdTypeSummaryRptModel.LogDay1;
								
				// 쿼리생성
				sbQuery = new StringBuilder();
				sbQuery.Append("\n"
					+ "/* 총기간 광고집행 집계																				\n"
					+ "   조회조건 : 매체코드, 계약번호 또는 광고번호 */														\n"
					+ "																									\n"
					+ "DECLARE @LogDay1 varchar(6);   -- 매체코드															\n"
					+ "DECLARE @LogDay2 varchar(6);  -- 집행일자														\n"					
															
					+ "SET @LogDay1   =		'" + logDay1  + "'\n"
					+ "SET @LogDay2    = '" + logDay2  + "';\n"	
															
					+ "with adData( LogDay, AdTypeCode, AdHitSum)													  \n"
					+ "as                                 															  \n"
					+ "(                                  															  \n"
					+ "		select LogDay                             \n"
					+ " 		  ,case t.RowNum when 1 then AdType when 2 then 100	end	as AdTypeCode																  \n"
					+ " 		  ,Sum(AdHitSum)	as AdHitSum																  \n"
					+ " 	from	(	select a.LogDay																		  \n"
					+ " 					  ,b.AdType															  \n"
					+ " 					  ,sum(AdCnt)		as AdHitSum																													\n"
					+ " 				from	dbo.SummaryAdDaily0 a with(NoLock)                                                                 \n"
                    + " 				inner join AdTargetsHanaTV.dbo.ContractItem b with(NoLock) on a.ItemNo = b.ItemNo                                                                \n"
					+ " 				where	a.LogDay Between @LogDay1 and @LogDay2                                                                \n"
					+ " 				and		a.ItemNo > 0                                                                \n"
					+ " 				and		a.SummaryType = 1                                                                \n"
					+ " 				and		b.AdType < '90'                                                                \n"
					+ " 				Group by a.LogDay,b.AdType ) d                                                                \n"
                    + " 				inner join AdTargetsHanaTV.dbo.copy_t t on t.RowNum <= 2                                                                 \n"
					+ " 				Group by LogDay,case t.RowNum when 1 then AdType when 2	then 100 end                                                                 \n"
					+ " 				union all																				\n"
					+ " 				select LogDay                                                                 \n"
					+ " 							,888	as AdTypeCode                                                                 \n"
					+ " 							,sum(ad.Hit00+ad.Hit01+ad.Hit02+ad.Hit03+ad.Hit04+ad.Hit05+ad.Hit06+ad.Hit07+ad.Hit08+ad.Hit09+ad.Hit10+ad.Hit11+ad.Hit12+ad.Hit13+ad.Hit14+ad.Hit15+ad.Hit16+ad.Hit17+ad.Hit18+ad.Hit19+ad.Hit20+ad.Hit21+ad.Hit22+ad.Hit23) as hit                                                                 \n"
					+ " 				from	dbo.SummaryAdHome ad with(NoLock)                                                                 \n"
					+ " 				where	LogDay Between @LogDay1 and @LogDay2                                                                 \n"
					+ " 				group by LogDay                                                                 \n"
					+ " 				union all                                                                 \n"
					+ " 				select	LogDay                                                                 \n"
					+ " 							,999	as AdTypeCode                                                                 \n"
					+ " 							,sum(ad.Hit00+ad.Hit01+ad.Hit02+ad.Hit03+ad.Hit04+ad.Hit05+ad.Hit06+ad.Hit07+ad.Hit08+ad.Hit09+ad.Hit10+ad.Hit11+ad.Hit12+ad.Hit13+ad.Hit14+ad.Hit15+ad.Hit16+ad.Hit17+ad.Hit18+ad.Hit19+ad.Hit20+ad.Hit21+ad.Hit22+ad.Hit23) as hit                                                                 \n"
					+ " 				from	SummaryPgJump ad with(NoLock)																			\n"
					+ " 				where	LogDay Between @LogDay1 and @LogDay2																							\n"
					+ " 				group by LogDay																															\n"										
					+ " )																					\n"
					+ " select v3.gubun,v3.adTypeCode	-- Order순서																					\n"
					+ " ,case v3.gubun																\n"
					+ " 		when 0 then																\n"
					+ " 				case adTypeCode																\n"					
					+ " 					when 100 then	'노출물량 ( 로딩 )'							\n"
					+ " 					when 888 then	'노출물량 ( 홈 )'							\n"
					+ " 					when 999 then	'노출물량 ( 점핑 )'							\n"
                    + "						else  (select '노출물량 ( ' + CodeName + ' )' from AdTargetsHanaTV.dbo.SystemCode sy with(NoLock) where sy.Section = 26 and sy.Code = adTypeCode )							\n"
					+ "					end							\n"
					+ "			when 1 then						  \n"
                    + "									(select '점 유 율 ( ' + CodeName + ' )' from AdTargetsHanaTV.dbo.SystemCode sy with(NoLock) where sy.Section = 26 and sy.Code = AdTypeCode )						  \n"
					+ "			when 2 then						  \n"
					+ "								case adTypeCode						  \n"
					+ "									when 100   then	'도 달 률 ( 전체 )'						  \n"
                    + "									else  (select '도 달 률 ( ' + CodeName + ' )' from AdTargetsHanaTV.dbo.SystemCode sy with(NoLock) where sy.Section = 26 and sy.Code = adTypeCode )						  \n"
					+ "								end																						\n"
					+ "			when 3 then																									\n"
					+ "								case AdTypeCode																			\n"
					+ "									when 100  then	'노출빈도 ( 전체 )'													\n"
                    + "									else  (select '노출빈도 ( ' + CodeName + ' )' from AdTargetsHanaTV.dbo.SystemCode sy with(NoLock) where sy.Section = 26 and sy.Code = AdTypeCode )						  \n"
					+ "								end																					  \n"
					+ "		  	end	as adTypeName																									  \n"
					+ "		  	,case gubun when 0 then replace( Convert( varchar(20),Convert(money,v3.d01),1),'.00','') else	Convert( varchar(20),Convert(money,v3.d01),1)	end	as c0															  \n"
					+ "		  	,case gubun when 0 then replace( Convert( varchar(20),Convert(money,v3.d02),1),'.00','') else	Convert( varchar(20),Convert(money,v3.d02),1)	end	as c1															  \n"
					+ "		  	,case gubun when 0 then replace( Convert( varchar(20),Convert(money,v3.d03),1),'.00','') else	Convert( varchar(20),Convert(money,v3.d03),1)	end	as c2															  \n"
					+ "		  	,case gubun when 0 then replace( Convert( varchar(20),Convert(money,v3.d04),1),'.00','') else	Convert( varchar(20),Convert(money,v3.d04),1)	end	as c3															  \n"
					+ "		  	,case gubun when 0 then replace( Convert( varchar(20),Convert(money,v3.d05),1),'.00','') else	Convert( varchar(20),Convert(money,v3.d05),1)	end	as c4															  \n"
					+ "		  	,case gubun when 0 then replace( Convert( varchar(20),Convert(money,v3.d06),1),'.00','') else	Convert( varchar(20),Convert(money,v3.d06),1)	end	as c5															  \n"
					+ "		  	,case gubun when 0 then replace( Convert( varchar(20),Convert(money,v3.d07),1),'.00','') else	Convert( varchar(20),Convert(money,v3.d07),1)	end	as c6															  \n"
					+ "		  	,case gubun when 0 then replace( Convert( varchar(20),Convert(money,v3.d08),1),'.00','') else	Convert( varchar(20),Convert(money,v3.d08),1)	end	as c7															  \n"
					+ "		  	,case gubun when 0 then replace( Convert( varchar(20),Convert(money,v3.d09),1),'.00','') else	Convert( varchar(20),Convert(money,v3.d09),1)	end	as c8															  \n"
					+ "		  	,case gubun when 0 then replace( Convert( varchar(20),Convert(money,v3.d10),1),'.00','') else	Convert( varchar(20),Convert(money,v3.d10),1)	end	as c9															  \n"
					+ "		  	,case gubun when 0 then replace( Convert( varchar(20),Convert(money,v3.d11),1),'.00','') else	Convert( varchar(20),Convert(money,v3.d11),1)	end	as c10															  \n"
					+ "		  	,case gubun when 0 then replace( Convert( varchar(20),Convert(money,v3.d12),1),'.00','') else	Convert( varchar(20),Convert(money,v3.d12),1)	end	as c11															  \n"
					+ "		  	,case gubun when 0 then replace( Convert( varchar(20),Convert(money,v3.d13),1),'.00','') else	Convert( varchar(20),Convert(money,v3.d13),1)	end	as c12															  \n"
					+ "		  	,case gubun when 0 then replace( Convert( varchar(20),Convert(money,v3.d14),1),'.00','') else	Convert( varchar(20),Convert(money,v3.d14),1)	end	as c13															  \n"
					+ "		  	,case gubun when 0 then replace( Convert( varchar(20),Convert(money,v3.d15),1),'.00','') else	Convert( varchar(20),Convert(money,v3.d15),1)	end	as c14															  \n"
					+ "		  	,case gubun when 0 then replace( Convert( varchar(20),Convert(money,v3.d16),1),'.00','') else	Convert( varchar(20),Convert(money,v3.d16),1)	end	as c15															  \n"
					+ "		  	,case gubun when 0 then replace( Convert( varchar(20),Convert(money,v3.d17),1),'.00','') else	Convert( varchar(20),Convert(money,v3.d17),1)	end	as c16															  \n"
					+ "		  	,case gubun when 0 then replace( Convert( varchar(20),Convert(money,v3.d18),1),'.00','') else	Convert( varchar(20),Convert(money,v3.d18),1)	end	as c17															  \n"
					+ "		  	,case gubun when 0 then replace( Convert( varchar(20),Convert(money,v3.d19),1),'.00','') else	Convert( varchar(20),Convert(money,v3.d19),1)	end	as c18															  \n"
					+ "		  	,case gubun when 0 then replace( Convert( varchar(20),Convert(money,v3.d20),1),'.00','') else	Convert( varchar(20),Convert(money,v3.d20),1)	end	as c19															  \n"
					+ "		  	,case gubun when 0 then replace( Convert( varchar(20),Convert(money,v3.d21),1),'.00','') else	Convert( varchar(20),Convert(money,v3.d21),1)	end	as c20															  \n"
					+ "		  	,case gubun when 0 then replace( Convert( varchar(20),Convert(money,v3.d22),1),'.00','') else	Convert( varchar(20),Convert(money,v3.d22),1)	end	as c21															  \n"
					+ "		  	,case gubun when 0 then replace( Convert( varchar(20),Convert(money,v3.d23),1),'.00','') else	Convert( varchar(20),Convert(money,v3.d23),1)	end	as c22															  \n"
					+ "		  	,case gubun when 0 then replace( Convert( varchar(20),Convert(money,v3.d24),1),'.00','') else	Convert( varchar(20),Convert(money,v3.d24),1)	end	as c23															  \n"
					+ "		  	,case gubun when 0 then replace( Convert( varchar(20),Convert(money,v3.d25),1),'.00','') else	Convert( varchar(20),Convert(money,v3.d25),1)	end	as c24															  \n"
					+ "		  	,case gubun when 0 then replace( Convert( varchar(20),Convert(money,v3.d26),1),'.00','') else	Convert( varchar(20),Convert(money,v3.d26),1)	end	as c25															  \n"
					+ "		  	,case gubun when 0 then replace( Convert( varchar(20),Convert(money,v3.d27),1),'.00','') else	Convert( varchar(20),Convert(money,v3.d27),1)	end	as c26															  \n"
					+ "		  	,case gubun when 0 then replace( Convert( varchar(20),Convert(money,v3.d28),1),'.00','') else	Convert( varchar(20),Convert(money,v3.d28),1)	end	as c27															  \n"
					+ "		  	,case gubun when 0 then replace( Convert( varchar(20),Convert(money,v3.d29),1),'.00','') else	Convert( varchar(20),Convert(money,v3.d29),1)	end	as c28															  \n"
					+ "		  	,case gubun when 0 then replace( Convert( varchar(20),Convert(money,v3.d30),1),'.00','') else	Convert( varchar(20),Convert(money,v3.d30),1)	end	as c29															  \n"
					+ "		  	,case gubun when 0 then replace( Convert( varchar(20),Convert(money,v3.d31),1),'.00','') else	Convert( varchar(20),Convert(money,v3.d31),1)	end	as c30															  \n"					
					+ "	from (																	  \n"
					+ "		  		select v1.gubun																  \n"
					+ "		  			,v1.adTypeCode																  \n"
					+ "		  			,sum(case v2.rowNum when 1  then v1.AdHitSum else 0 end )	as d01																  \n"
					+ "		  			,sum(case v2.rowNum when 2  then v1.AdHitSum else 0 end )	as d02																  \n"
					+ "		  			,sum(case v2.rowNum when 3  then v1.AdHitSum else 0 end )	as d03																  \n"
					+ "		  			,sum(case v2.rowNum when 4  then v1.AdHitSum else 0 end )	as d04																  \n"
					+ "		  			,sum(case v2.rowNum when 5  then v1.AdHitSum else 0 end )	as d05																  \n"
					+ "		  			,sum(case v2.rowNum when 6  then v1.AdHitSum else 0 end )	as d06																  \n"
					+ "		  			,sum(case v2.rowNum when 7  then v1.AdHitSum else 0 end )	as d07																  \n"
					+ "		  			,sum(case v2.rowNum when 8  then v1.AdHitSum else 0 end )	as d08																  \n"
					+ "		  			,sum(case v2.rowNum when 9  then v1.AdHitSum else 0 end )	as d09																  \n"
					+ "		  			,sum(case v2.rowNum when 10 then v1.AdHitSum else 0 end )	as d10																  \n"	
					+ "		  			,sum(case v2.rowNum when 11 then v1.AdHitSum else 0 end )	as d11																  \n"
					+ "		  			,sum(case v2.rowNum when 12 then v1.AdHitSum else 0 end )	as d12																  \n"
					+ "		  			,sum(case v2.rowNum when 13 then v1.AdHitSum else 0 end )	as d13																  \n"
					+ "		  			,sum(case v2.rowNum when 14 then v1.AdHitSum else 0 end )	as d14																  \n"
					+ "		  			,sum(case v2.rowNum when 15 then v1.AdHitSum else 0 end )	as d15																  \n"
					+ "		  			,sum(case v2.rowNum when 16 then v1.AdHitSum else 0 end )	as d16																  \n"
					+ "		  			,sum(case v2.rowNum when 17 then v1.AdHitSum else 0 end )	as d17																  \n"
					+ "		  			,sum(case v2.rowNum when 18 then v1.AdHitSum else 0 end )	as d18																  \n"
					+ "		  			,sum(case v2.rowNum when 19 then v1.AdHitSum else 0 end )	as d19																  \n"
					+ "		  			,sum(case v2.rowNum when 20 then v1.AdHitSum else 0 end )	as d20																  \n"
					+ "		  			,sum(case v2.rowNum when 21 then v1.AdHitSum else 0 end )	as d21																  \n"					
					+ "		  			,sum(case v2.rowNum when 22 then v1.AdHitSum else 0 end )	as d22																  \n"
					+ "		  			,sum(case v2.rowNum when 23 then v1.AdHitSum else 0 end )	as d23																  \n"
					+ "		  			,sum(case v2.rowNum when 24 then v1.AdHitSum else 0 end )	as d24																  \n"
					+ "		  			,sum(case v2.rowNum when 25 then v1.AdHitSum else 0 end )	as d25																  \n"
					+ "		  			,sum(case v2.rowNum when 26 then v1.AdHitSum else 0 end )	as d26																  \n"
					+ "		  			,sum(case v2.rowNum when 27 then v1.AdHitSum else 0 end )	as d27																  \n"
					+ "		  			,sum(case v2.rowNum when 28 then v1.AdHitSum else 0 end )	as d28																  \n"
					+ "		  			,sum(case v2.rowNum when 29 then v1.AdHitSum else 0 end )	as d29																  \n"
					+ "		  			,sum(case v2.rowNum when 30 then v1.AdHitSum else 0 end )	as d30																  \n"
					+ "		  			,sum(case v2.rowNum when 31 then v1.AdHitSum else 0 end )	as d31																  \n"					
					+ "			from	(																  \n"
					+ "		  			select LogDay																  \n"
					+ "		  						,0	as gubun																  \n"
					+ "		  						,AdTypeCode																  \n"
					+ "		  						,AdHitSum																  \n"
					+ "		  			from	adData																  \n"
					+ "		  			union all																  \n"
					+ "		  			select adData.LogDay																  \n"
					+ "		  						,1																  \n"
					+ "		  						,AdTypeCode																  \n"
					+ "		  						,cast(adHitSum as real) / (select base.ChannelAdHit from SummaryBase base with(NoLock) where base.LogDay = adData.LogDay ) * 100																  \n"					
					+ "					from adData																										  \n"
					+ "		 			where	adData.AdTypeCode < 100																	\n"
					+ "		 			union all						  \n"					
					+ "		  			select ht.LogDay		  \n"
					+ "		  						,2				  \n"
					+ "		  						,case ht.TypeCode when 0 then 100 else ht.TypeCode end			  \n"
					+ "		  						,cast(ht.DayUsers as real) / (select base.HouseTotal from SummaryBase base with(NoLock) where base.LogDay = ht.LogDay ) * 100			  \n"
					+ "		  			from	SummaryHouseHoldType ht with(NoLock)																  \n"
					+ "		  			where	ht.LogDay Between @LogDay1 and @LogDay2															  \n"
					+ "		  			and		ht.TypeSection = 26															  \n"
					+ "		  			union all															  \n"
					+ "		  			select adData.LogDay															  \n"
					+ "		  						,3															  \n"
					+ "		  						,adData.AdTypeCode															  \n"
					+ "		  						,case when (select ht.DayUsers from dbo.SummaryHouseHoldType ht with(NoLock) where ht.LogDay = adData.LogDay and ht.TypeSection=26 and case ht.TypeCode when 0 then 100 else ht.TypeCode end = adData.AdTypeCode ) = 0 then 0	  \n"
					+ "		  								else cast(adHitSum as real) / (select ht.DayUsers from dbo.SummaryHouseHoldType ht with(NoLock) where ht.LogDay = adData.LogDay and ht.TypeSection=26 and case ht.TypeCode when 0 then 100 else ht.TypeCode end = adData.AdTypeCode ) end		\n"
					+ "		  			from	adData															  \n"
					+ "		  			where	adData.AdTypeCode < 888 ) v1															  \n"					
					+ "		  inner join 															  \n"
					+ "		  			(	select LogDay,row_number() over(order by LogDay) as rowNum																  \n"
					+ "		  				from	SummaryBase base with(nolock)																  \n"
					+ "		  				where	base.LogDay Between @LogDay1 and @LogDay2 ) v2 on v1.LogDay = v2.LogDay																  \n"
					+ "		  		group by v1.gubun,v1.adTypeCode ) v3																  \n"
					+ "			union all																  \n"
					+ "			select 0	as gubun																  \n"
					+ "		  			,0	as adTypeCode																  \n"
					+ "		  			,'구분'	as TypeName																  \n"
					+ "		  			,case d01 when '' then '' else substring(d01,3,2) + '/' + substring(d01,5,2) end	as d01																  \n"
					+ "		  			,case d02 when '' then '' else substring(d02,3,2) + '/' + substring(d02,5,2) end	as d02																  \n"
					+ "		  			,case d03 when '' then '' else substring(d03,3,2) + '/' + substring(d03,5,2) end	as d03																  \n"
					+ "		  			,case d04 when '' then '' else substring(d04,3,2) + '/' + substring(d04,5,2) end	as d04																  \n"
					+ "		  			,case d05 when '' then '' else substring(d05,3,2) + '/' + substring(d05,5,2) end	as d05																  \n"
					+ "		  			,case d06 when '' then '' else substring(d06,3,2) + '/' + substring(d06,5,2) end	as d06																  \n"
					+ "		  			,case d07 when '' then '' else substring(d07,3,2) + '/' + substring(d07,5,2) end	as d07																  \n"
					+ "		  			,case d08 when '' then '' else substring(d08,3,2) + '/' + substring(d08,5,2) end	as d08																  \n"					
					+ "		  			,case d09 when '' then '' else substring(d09,3,2) + '/' + substring(d09,5,2) end	as d09																  \n"
					+ "		  			,case d10 when '' then '' else substring(d10,3,2) + '/' + substring(d10,5,2) end	as d10																  \n"
					+ "		  			,case d11 when '' then '' else substring(d11,3,2) + '/' + substring(d11,5,2) end	as d11																  \n"
					+ "		  			,case d12 when '' then '' else substring(d12,3,2) + '/' + substring(d12,5,2) end	as d12																  \n"
					+ "		  			,case d13 when '' then '' else substring(d13,3,2) + '/' + substring(d13,5,2) end	as d13																  \n"
					+ "		  			,case d14 when '' then '' else substring(d14,3,2) + '/' + substring(d14,5,2) end	as d14																  \n"
					+ "		  			,case d15 when '' then '' else substring(d15,3,2) + '/' + substring(d15,5,2) end	as d15																  \n"
					+ "		  			,case d16 when '' then '' else substring(d16,3,2) + '/' + substring(d16,5,2) end	as d16																  \n"
					+ "		  			,case d17 when '' then '' else substring(d17,3,2) + '/' + substring(d17,5,2) end	as d17																  \n"
					+ "		  			,case d18 when '' then '' else substring(d18,3,2) + '/' + substring(d18,5,2) end	as d18																  \n"					
					+ "		  			,case d19 when '' then '' else substring(d19,3,2) + '/' + substring(d19,5,2) end	as d19																  \n"
					+ "		  			,case d20 when '' then '' else substring(d20,3,2) + '/' + substring(d20,5,2) end	as d20																  \n"
					+ "		  			,case d21 when '' then '' else substring(d21,3,2) + '/' + substring(d21,5,2) end	as d21																  \n"
					+ "		  			,case d22 when '' then '' else substring(d22,3,2) + '/' + substring(d22,5,2) end	as d22																  \n"
					+ "		  			,case d23 when '' then '' else substring(d23,3,2) + '/' + substring(d23,5,2) end	as d23																  \n"
					+ "		  			,case d24 when '' then '' else substring(d24,3,2) + '/' + substring(d24,5,2) end	as d24																  \n"
					+ "		  			,case d25 when '' then '' else substring(d25,3,2) + '/' + substring(d25,5,2) end	as d25																  \n"
					+ "		  			,case d26 when '' then '' else substring(d26,3,2) + '/' + substring(d26,5,2) end	as d26																  \n"
					+ "		  			,case d27 when '' then '' else substring(d27,3,2) + '/' + substring(d27,5,2) end	as d27																  \n"
					+ "		  			,case d28 when '' then '' else substring(d28,3,2) + '/' + substring(d28,5,2) end	as d28																  \n"					
					+ "		  			,case d29 when '' then '' else substring(d29,3,2) + '/' + substring(d29,5,2) end	as d29																  \n"
					+ "		  			,case d30 when '' then '' else substring(d30,3,2) + '/' + substring(d30,5,2) end	as d30																  \n"
					+ "		  			,case d31 when '' then '' else substring(d31,3,2) + '/' + substring(d31,5,2) end	as d31																  \n"
					+ "		  	from (																  \n"
					+ "		  			select  max(case v2.rowNum when 1 then v2.LogDay	else '' end)  as d01															  \n"
					+ "		  						, max(case v2.rowNum when 2 then v2.LogDay	else '' end)	as d02															  \n"
					+ "		  						, max(case v2.rowNum when 3 then v2.LogDay	else '' end)	as d03															  \n"
					+ "		  						, max(case v2.rowNum when 4 then v2.LogDay	else '' end)	as d04															  \n"
					+ "		  						, max(case v2.rowNum when 5 then v2.LogDay	else '' end)	as d05															  \n"
					+ "		  						, max(case v2.rowNum when 6 then v2.LogDay	else '' end)	as d06															  \n"					
					+ "		  						, max(case v2.rowNum when 7 then v2.LogDay	else '' end)	as d07															  \n"
					+ "		  						, max(case v2.rowNum when 8 then v2.LogDay	else '' end)	as d08															  \n"
					+ "		  						, max(case v2.rowNum when 9 then v2.LogDay	else '' end)	as d09															  \n"
					+ "		  						, max(case v2.rowNum when 10 then v2.LogDay else '' end)	as d10															  \n"
					+ "		  						, max(case v2.rowNum when 11 then v2.LogDay else '' end)	as d11															  \n"
					+ "		  						, max(case v2.rowNum when 12 then v2.LogDay else '' end)	as d12															  \n"					
					+ "		  						, max(case v2.rowNum when 13 then v2.LogDay else '' end)	as d13															  \n"
					+ "		  						, max(case v2.rowNum when 14 then v2.LogDay else '' end)	as d14															  \n"
					+ "		  						, max(case v2.rowNum when 15 then v2.LogDay else '' end)	as d15															  \n"
					+ "		  						, max(case v2.rowNum when 16 then v2.LogDay else '' end)	as d16															  \n"
					+ "		  						, max(case v2.rowNum when 17 then v2.LogDay else '' end)	as d17															  \n"
					+ "		  						, max(case v2.rowNum when 18 then v2.LogDay else '' end)	as d18															  \n"					
					+ "		  						, max(case v2.rowNum when 19 then v2.LogDay else '' end)	as d19															  \n"
					+ "		  						, max(case v2.rowNum when 20 then v2.LogDay else '' end)	as d20															  \n"
					+ "		  						, max(case v2.rowNum when 21 then v2.LogDay else '' end)	as d21															  \n"
					+ "		  						, max(case v2.rowNum when 22 then v2.LogDay else '' end)	as d22															  \n"
					+ "		  						, max(case v2.rowNum when 23 then v2.LogDay else '' end)	as d23															  \n"
					+ "		  						, max(case v2.rowNum when 24 then v2.LogDay else '' end)	as d24															  \n"					
					+ "		  						, max(case v2.rowNum when 25 then v2.LogDay else '' end)	as d25															  \n"
					+ "		  						, max(case v2.rowNum when 26 then v2.LogDay else '' end)	as d26															  \n"
					+ "		  						, max(case v2.rowNum when 27 then v2.LogDay else '' end)	as d27															  \n"
					+ "		  						, max(case v2.rowNum when 28 then v2.LogDay else '' end)	as d28															  \n"
					+ "		  						, max(case v2.rowNum when 29 then v2.LogDay else '' end)	as d29															  \n"
					+ "		  						, max(case v2.rowNum when 30 then v2.LogDay else '' end)	as d30															  \n"					
					+ "		  						, max(case v2.rowNum when 31 then v2.LogDay else '' end)	as d31															  \n"
					+ "		  			from (	select LogDay,row_number() over(order by LogDay) as rowNum															  \n"
					+ "		  							from	SummaryBase base with(nolock)															  \n"
					+ "		  							where	base.LogDay Between @LogDay1 and @LogDay2  ) v2 ) y1															  \n"							
					+ "		  order by 1,2																												  \n"													
					);

				// __DEBUG__
				_log.Debug(sbQuery.ToString());
				// __DEBUG__
				
				// 쿼리실행
				DataSet ds = new DataSet();
				_db.ExecuteQuery(ds,sbQuery.ToString());

				// 결과 DataSet의 데이터모델에 복사
				dateAdTypeSummaryRptModel.ReportDataSet = ds.Copy();

				// 결과
				dateAdTypeSummaryRptModel.ResultCnt = Utility.GetDatasetCount(dateAdTypeSummaryRptModel.ReportDataSet);

				ds.Dispose();

				// 결과코드 셋트
				dateAdTypeSummaryRptModel.ResultCD = "0000";

				// __DEBUG__
				_log.Debug("<출력정보>");
				_log.Debug("ResultCnt:[" + dateAdTypeSummaryRptModel.ResultCnt + "]");
				// __DEBUG__

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetDateAdTypeSummaryRpt() End");
				_log.Debug("-----------------------------------------");
			}
			catch(Exception ex)
			{
				dateAdTypeSummaryRptModel.ResultCD = "3000";
				if(isNotReady)
				{
					dateAdTypeSummaryRptModel.ResultDesc = "해당 기간은 데이터가 집계되지 않았습니다.";
				}
				else
				{
					dateAdTypeSummaryRptModel.ResultDesc = "일별 광고종류별 조회중 오류가 발생하였습니다";
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

		#region 주별 광고종류별 보고서
		/// <summary>
		///  주별 광고집행 집계
		/// </summary>
		/// <param name="dateAdTypeSummaryRptModel"></param>
		public void GetWeeklyAdTypeSummaryRpt(HeaderModel header, DateAdTypeSummaryRptModel dateAdTypeSummaryRptModel)
		{
			bool isNotReady = false; // 데이터가 집계가 되지않아 존재하지 않을때.
			try
			{
				StringBuilder sbQuery = null;

				// 데이터베이스를 OPEN한다
				_db.Open();


				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetWeeklyAdTypeSummaryRpt() Start");
				_log.Debug("-----------------------------------------");

				// 일자가 6자리 이상(yyyymmdd)이면 6자리로 만든다.
				if(dateAdTypeSummaryRptModel.LogDay1.Length > 6) dateAdTypeSummaryRptModel.LogDay1 = dateAdTypeSummaryRptModel.LogDay1.Substring(2,6);
				if(dateAdTypeSummaryRptModel.LogDay2.Length > 6) dateAdTypeSummaryRptModel.LogDay2 = dateAdTypeSummaryRptModel.LogDay2.Substring(2,6);			
				
				// __DEBUG__
				_log.Debug("<입력정보>");
				_log.Debug("LogDay1	  :[" + dateAdTypeSummaryRptModel.LogDay1   + "]");		// 검색 매체
				_log.Debug("LogDay2	  :[" + dateAdTypeSummaryRptModel.LogDay2   + "]");					
				// __DEBUG__

				ArrayList dtList = new ArrayList();
				DateTime sdt = DateTime.ParseExact(dateAdTypeSummaryRptModel.LogDay1, "yyMMdd", System.Globalization.CultureInfo.CurrentCulture);
				DateTime fdt = DateTime.ParseExact(dateAdTypeSummaryRptModel.LogDay2, "yyMMdd", System.Globalization.CultureInfo.CurrentCulture);

				sdt = DateUtil.getLastDayOfWeekForStartMonday(sdt);
				fdt = fdt.AddDays(7);
				while(sdt < fdt)
				{
					dtList.Add(sdt.ToString("yyMMdd"));
					_log.Debug(sdt.ToString("yyMMdd"));
					sdt = sdt.AddDays(7);
				}
				Object[] adt = dtList.ToArray();

				string logDay1   = dateAdTypeSummaryRptModel.LogDay1;
				string logDay2   = dateAdTypeSummaryRptModel.LogDay2;
				
				SqlParameter[] sqlParams = new SqlParameter[1];
				sqlParams[0] = new SqlParameter("@LogDay1", SqlDbType.VarChar, 6);
				sqlParams[0].Value = dateAdTypeSummaryRptModel.LogDay1;
								
				// 쿼리생성
				sbQuery = new StringBuilder();
				sbQuery.Append("/* 총기간 광고집행 집계																	\n");
				sbQuery.Append("   조회조건 : 매체코드, 계약번호 또는 광고번호 */										\n");
				sbQuery.Append("																						\n");
				sbQuery.Append("DECLARE @LogDay1 varchar(6);   -- 매체코드												\n");
				sbQuery.Append("DECLARE @LogDay2 varchar(6);  -- 집행일자												\n");
				sbQuery.Append("SET @LogDay1   = '"+ logDay1 +"';														\n");
				sbQuery.Append("SET @LogDay2   = '"+ logDay2 +"';														\n");
				sbQuery.Append("with adData(LogEndWeek, AdTypeCode, AdHitSum)											\n");
				sbQuery.Append("as																						\n");
				sbQuery.Append("(																						\n");
				sbQuery.Append("		select																			\n");
				sbQuery.Append("				LogEndWeek																\n");
				sbQuery.Append("				, case t.RowNum when 1 then AdType when 2 then 100 end as AdTypeCode	\n");
				sbQuery.Append("				, sum(AdCnt) AdHitSum													\n");
				sbQuery.Append("		from (select																	\n");
                sbQuery.Append("						AdTargetsHanaTV.dbo.ufnLastDayOfWeekForStartMon(LogDay) LogEndWeek,				\n");
				sbQuery.Append("						ItemNo, sum(AdCnt) AdCnt										\n");
				sbQuery.Append("				from SummaryAdDaily0 with(nolock)										\n");
				sbQuery.Append("				where logday >= @LogDay1 and logday <= @LogDay2							\n");
				sbQuery.Append("				and ItemNo > 0															\n");
				sbQuery.Append("				and ContractSeq = 0														\n");
				sbQuery.Append("				and SummaryType = 1														\n");
                sbQuery.Append("				group by AdTargetsHanaTV.dbo.ufnLastDayOfWeekForStartMon(logday), itemno				\n");
				sbQuery.Append("		) a																				\n");
                sbQuery.Append("		inner join AdTargetsHanaTV.dbo.ContractItem b with(NoLock) on a.ItemNo = b.ItemNo					\n");
                sbQuery.Append("		inner join AdTargetsHanaTV.dbo.Copy_t t on t.RowNum <= 2											\n");
				sbQuery.Append("		where																			\n");
				sbQuery.Append("				b.AdType < '90'															\n");
				sbQuery.Append("				and b.AdType <> 15														\n");
				sbQuery.Append("		group by LogEndWeek, case t.RowNum when 1 then AdType when 2 then 100 end		\n");
				sbQuery.Append("		union all																		\n");
                sbQuery.Append("		select AdTargetsHanaTV.dbo.ufnLastDayOfWeekForStartMon(LogDay)									\n");
				sbQuery.Append("				,888		as AdTypeCode												\n");
				sbQuery.Append("				,sum(ad.Hit00+ad.Hit01+ad.Hit02+ad.Hit03+ad.Hit04+ad.Hit05+ad.Hit06+ad.Hit07+ad.Hit08+ad.Hit09+ad.Hit10+ad.Hit11+ad.Hit12+ad.Hit13+ad.Hit14+ad.Hit15+ad.Hit16+ad.Hit17+ad.Hit18+ad.Hit19+ad.Hit20+ad.Hit21+ad.Hit22+ad.Hit23) as hit	\n");
				sbQuery.Append("		from		SummaryAdHome ad with(NoLock)										\n");
				sbQuery.Append("		where		LogDay Between @LogDay1 and @LogDay2								\n");
                sbQuery.Append("		group by AdTargetsHanaTV.dbo.ufnLastDayOfWeekForStartMon(LogDay)								\n");
				sbQuery.Append("		union all																		\n");
                sbQuery.Append("		select AdTargetsHanaTV.dbo.ufnLastDayOfWeekForStartMon(LogDay)									\n");
				sbQuery.Append("				,999		as AdTypeCode												\n");
				sbQuery.Append("				,sum(ad.Hit00+ad.Hit01+ad.Hit02+ad.Hit03+ad.Hit04+ad.Hit05+ad.Hit06+ad.Hit07+ad.Hit08+ad.Hit09+ad.Hit10+ad.Hit11+ad.Hit12+ad.Hit13+ad.Hit14+ad.Hit15+ad.Hit16+ad.Hit17+ad.Hit18+ad.Hit19+ad.Hit20+ad.Hit21+ad.Hit22+ad.Hit23) as hit	\n");
				sbQuery.Append("		from		SummaryPgJump ad with(NoLock)										\n");
				sbQuery.Append("		where		LogDay Between @LogDay1 and @LogDay2								\n");
                sbQuery.Append("		group by AdTargetsHanaTV.dbo.ufnLastDayOfWeekForStartMon(LogDay)								\n");
				sbQuery.Append(")																						\n");
				sbQuery.Append("select -1 gubun, -1 adTypeCode, '' adTypeName											\n");
				for(int i=0; i<adt.Length; i++)
				{
					sbQuery.Append("		, '"+ DateUtil.getDispWeekForStartMonday((string)adt[i]) +"' c"+ i +"		\n");
				}
				sbQuery.Append("union all																				\n");
				sbQuery.Append("select v3.gubun, v3.adTypeCode		-- Order순서										\n");
				sbQuery.Append("		, case v3.gubun																	\n");
				sbQuery.Append(" 		when 0 then																		\n");
				sbQuery.Append(" 				case adTypeCode															\n");
				sbQuery.Append(" 						when 100 then		'노출물량 ( 로딩 )'							\n");
				sbQuery.Append(" 						when 888 then		'노출물량 ( 홈 )'							\n");
				sbQuery.Append(" 						when 999 then		'노출물량 ( 점핑 )'							\n");
                sbQuery.Append("						else  (select '노출물량 ( ' + CodeName + ' )' from AdTargetsHanaTV.dbo.SystemCode sy with(NoLock) where sy.Section = 26 and sy.Code = adTypeCode )	\n");
				sbQuery.Append("				end																		\n");
				sbQuery.Append("						when 1 then														\n");
                sbQuery.Append("								(select '점 유 율 ( ' + CodeName + ' )' from AdTargetsHanaTV.dbo.SystemCode sy with(NoLock) where sy.Section = 26 and sy.Code = AdTypeCode )	\n");
				sbQuery.Append("						when 2 then														\n");
				sbQuery.Append("								case adTypeCode											\n");
				sbQuery.Append("										when 100   then		'도 달 률 ( 전체 )'			\n");
                sbQuery.Append("										else  (select '도 달 률 ( ' + CodeName + ' )' from AdTargetsHanaTV.dbo.SystemCode sy with(NoLock) where sy.Section = 26 and sy.Code = adTypeCode )	\n");
				sbQuery.Append("								end														\n");
				sbQuery.Append("						when 3 then														\n");
				sbQuery.Append("								case AdTypeCode											\n");
				sbQuery.Append("										when 100  then		'노출빈도 ( 전체 )'			\n");
                sbQuery.Append("										else  (select '노출빈도 ( ' + CodeName + ' )' from AdTargetsHanaTV.dbo.SystemCode sy with(NoLock) where sy.Section = 26 and sy.Code = AdTypeCode )	\n");
				sbQuery.Append("								end														\n");
				sbQuery.Append("				  		end		as adTypeName											\n");
				for(int i=0; i<adt.Length; i++)
				{
					sbQuery.Append("				  		, case when gubun = 0 then replace( Convert( varchar(20),Convert(money,v3.c"+ i +"),1),'.00','') when adTypeCode = -1 then '' else Convert( varchar(20),Convert(money,v3.c"+ i +"),1)		end		as c"+ i +"	\n");
				}
				sbQuery.Append("		from (																			\n");
				sbQuery.Append("				select v1.gubun															\n");
				sbQuery.Append("  						,v1.adTypeCode													\n");
				for(int i=0; i<adt.Length; i++)
				{
					sbQuery.Append("						, sum(isnull(case when LogEndWeek = '"+ (string)adt[i] +"' then v1.AdHitSum else 0 end, 0)) c"+ i +"	\n");
				}
				sbQuery.Append("				from (																	\n");
				sbQuery.Append("  						select LogEndWeek												\n");
				sbQuery.Append("  								,0 gubun												\n");
				sbQuery.Append("  								,AdTypeCode												\n");
				sbQuery.Append("  								,AdHitSum												\n");
				sbQuery.Append("  						from adData														\n");
				sbQuery.Append("						union all select '', 1 gubun, -1 AdTypeCode, 0 AdHitSum			\n");
				sbQuery.Append("  						union all														\n");
				sbQuery.Append("  						select adData.LogEndWeek										\n");
				sbQuery.Append("  								,1 gubun												\n");
				sbQuery.Append("  								,AdTypeCode												\n");
				sbQuery.Append("  								,cast(adHitSum as real) / sum(adHitSum) over(PARTITION BY LogEndWeek)*100	\n");
				sbQuery.Append("						from adData														\n");
				sbQuery.Append(" 						where		adData.AdTypeCode < 100								\n");
				sbQuery.Append("						union all select '', 2 gubun, -1 AdTypeCode, 0 AdHitSum			\n");
				sbQuery.Append(" 						union all														\n");
				sbQuery.Append("  						select adData.LogEndWeek										\n");
				sbQuery.Append("  								,2 gubun												\n");
				sbQuery.Append("  								,case ht.TypeCode when 0 then 100 else ht.TypeCode end	\n");
				sbQuery.Append("  								,cast(ht.DayUsers as real) / (select base.HouseTotal from SummaryBase base with(NoLock) where base.LogDay = adData.LogEndWeek ) * 100	\n");
				sbQuery.Append("  						from		SummaryHouseHoldType ht with(NoLock)				\n");
				sbQuery.Append("						inner join adData on (ht.LogDay = adData.LogEndWeek)			\n");
				sbQuery.Append(" 						where		adData.AdTypeCode = 100								\n");
				sbQuery.Append("  						and				ht.TypeSection = 26								\n");
				sbQuery.Append("						and				ht.TypeCode <> 15								\n");
				sbQuery.Append("						union all select '', 3 gubun, -1 AdTypeCode, 0 AdHitSum			\n");
				sbQuery.Append("  						union all														\n");
				sbQuery.Append("  						select adData.LogEndWeek										\n");
				sbQuery.Append("  								,3														\n");
				sbQuery.Append("  								,adData.AdTypeCode										\n");
				sbQuery.Append("  								,case when (select ht.WeekUsers from dbo.SummaryHouseHoldType ht with(NoLock) where ht.LogDay = adData.LogEndWeek and ht.TypeSection=26 and case ht.TypeCode when 0 then 100 else ht.TypeCode end = adData.AdTypeCode ) = 0 then 0	\n");
				sbQuery.Append("  										else cast(adHitSum as real) / (select ht.WeekUsers from dbo.SummaryHouseHoldType ht with(NoLock) where ht.LogDay = adData.LogEndWeek and ht.TypeSection=26 and case ht.TypeCode when 0 then 100 else ht.TypeCode end = adData.AdTypeCode ) end	\n");
				sbQuery.Append("  						from		adData												\n");
				sbQuery.Append("  						where		adData.AdTypeCode < 888								\n");
				sbQuery.Append("				) v1																	\n");
				sbQuery.Append("				group by v1.gubun,v1.adTypeCode											\n");
				sbQuery.Append("		)v3																				\n");
				sbQuery.Append("order by 1, 2																			\n");

				// __DEBUG__
				_log.Debug(sbQuery.ToString());
				// __DEBUG__
				
				// 쿼리실행
				DataSet ds = new DataSet();
				_db.ExecuteQuery(ds,sbQuery.ToString());

				// 결과 DataSet의 데이터모델에 복사
				dateAdTypeSummaryRptModel.ReportDataSet = ds.Copy();

				// 결과
				dateAdTypeSummaryRptModel.ResultCnt = Utility.GetDatasetCount(dateAdTypeSummaryRptModel.ReportDataSet);

				ds.Dispose();

				// 결과코드 셋트
				dateAdTypeSummaryRptModel.ResultCD = "0000";

				// __DEBUG__
				_log.Debug("<출력정보>");
				_log.Debug("ResultCnt:[" + dateAdTypeSummaryRptModel.ResultCnt + "]");
				// __DEBUG__

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetWeeklyAdTypeSummaryRpt() End");
				_log.Debug("-----------------------------------------");
			}
			catch(Exception ex)
			{
				dateAdTypeSummaryRptModel.ResultCD = "3000";
				if(isNotReady)
				{
					dateAdTypeSummaryRptModel.ResultDesc = "해당 기간은 데이터가 집계되지 않았습니다.";
				}
				else
				{
					dateAdTypeSummaryRptModel.ResultDesc = "주별 광고종류별 조회중 오류가 발생하였습니다";
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

		#region 월별 광고종류별 보고서
		/// <summary>
		///  월별 광고집행 집계
		/// </summary>
		/// <param name="dateAdTypeSummaryRptModel"></param>
		public void GetMonthlyAdTypeSummaryRpt(HeaderModel header, DateAdTypeSummaryRptModel dateAdTypeSummaryRptModel)
		{
			bool isNotReady = false; // 데이터가 집계가 되지않아 존재하지 않을때.
			try
			{
				StringBuilder sbQuery = null;

				// 데이터베이스를 OPEN한다
				_db.Open();


				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetMonthlyAdTypeSummaryRpt() Start");
				_log.Debug("-----------------------------------------");

				// 일자가 6자리 이상(yyyymmdd)이면 6자리로 만든다.
				if(dateAdTypeSummaryRptModel.LogDay1.Length > 6) dateAdTypeSummaryRptModel.LogDay1 = dateAdTypeSummaryRptModel.LogDay1.Substring(2,6);
				if(dateAdTypeSummaryRptModel.LogDay2.Length > 6) dateAdTypeSummaryRptModel.LogDay2 = dateAdTypeSummaryRptModel.LogDay2.Substring(2,6);			
				
				// __DEBUG__
				_log.Debug("<입력정보>");
				_log.Debug("LogDay1	  :[" + dateAdTypeSummaryRptModel.LogDay1   + "]");		// 검색 매체
				_log.Debug("LogDay2	  :[" + dateAdTypeSummaryRptModel.LogDay2   + "]");					
				// __DEBUG__

				ArrayList dtList = new ArrayList();
				DateTime sdt = DateTime.ParseExact(dateAdTypeSummaryRptModel.LogDay1, "yyMMdd", System.Globalization.CultureInfo.CurrentCulture);
				DateTime fdt = DateTime.ParseExact(dateAdTypeSummaryRptModel.LogDay2, "yyMMdd", System.Globalization.CultureInfo.CurrentCulture);

				while(sdt < fdt)
				{
					dtList.Add(sdt.ToString("yyMMdd"));
					sdt = sdt.AddMonths(1);
				}
				Object[] adt = dtList.ToArray();

				string logDay1   = dateAdTypeSummaryRptModel.LogDay1;
				string logDay2   = dateAdTypeSummaryRptModel.LogDay2;
				
				SqlParameter[] sqlParams = new SqlParameter[1];
				sqlParams[0] = new SqlParameter("@LogDay1", SqlDbType.VarChar, 6);
				sqlParams[0].Value = dateAdTypeSummaryRptModel.LogDay1;
								
				// 쿼리생성
				sbQuery = new StringBuilder();
				sbQuery.Append("																							\n");
				sbQuery.Append("/* 총기간 광고집행 집계																		\n");
				sbQuery.Append("   조회조건 : 매체코드, 계약번호 또는 광고번호 */											\n");
				sbQuery.Append("																							\n");
				sbQuery.Append("DECLARE @LogDay1 varchar(6);   -- 매체코드													\n");
				sbQuery.Append("DECLARE @LogDay2 varchar(6);  -- 집행일자													\n");
				sbQuery.Append("SET @LogDay1   = '"+ logDay1 +"';															\n");
				sbQuery.Append("SET @LogDay2   = '"+ logDay2 +"';															\n");
				sbQuery.Append("with adData( LogMon, EndMon, AdTypeCode, AdHitSum)											\n");
				sbQuery.Append("as																							\n");
				sbQuery.Append("(																							\n");
				sbQuery.Append("		select																				\n");
				sbQuery.Append("				LogMon																		\n");
                sbQuery.Append("				, AdTargetsHanaTV.dbo.ufnLastDayOfMonth(LogMon) EndMon										\n");
				sbQuery.Append("				, case t.RowNum when 1 then AdType when 2 then 100 end as AdTypeCode		\n");
				sbQuery.Append("				, sum(AdCnt) AdHitSum														\n");
				sbQuery.Append("		from (select																		\n");
				sbQuery.Append("						LEFT(logday, 4) LogMon,												\n");
				sbQuery.Append("						ItemNo, sum(AdCnt) AdCnt											\n");
				sbQuery.Append("				from SummaryAdDaily0 with(nolock)											\n");
				sbQuery.Append("				where logday >= @LogDay1 and logday <= @LogDay2								\n");
				sbQuery.Append("				and ItemNo > 0																\n");
				sbQuery.Append("				and ContractSeq = 0															\n");
				sbQuery.Append("				and SummaryType = 1															\n");
				sbQuery.Append("				group by LEFT(LogDay, 4),itemno												\n");
				sbQuery.Append("		) a																					\n");
                sbQuery.Append("		inner join AdTargetsHanaTV.dbo.ContractItem b with(NoLock) on a.ItemNo = b.ItemNo						\n");
                sbQuery.Append("		inner join AdTargetsHanaTV.dbo.Copy_t t on t.RowNum <= 2												\n");
				sbQuery.Append("		where																				\n");
				sbQuery.Append("				b.AdType < '90'																\n");
				sbQuery.Append("				and b.AdType <> 15															\n");
				sbQuery.Append("		group by LogMon, case t.RowNum when 1 then AdType when 2 then 100 end				\n");
				sbQuery.Append("		union all																			\n");
				sbQuery.Append("		select LEFT(LogDay, 4)																\n");
                sbQuery.Append("				, AdTargetsHanaTV.dbo.ufnLastDayOfMonth(LEFT(LogDay, 4))									\n");
				sbQuery.Append("				,888		as AdTypeCode													\n");
				sbQuery.Append("				,sum(ad.Hit00+ad.Hit01+ad.Hit02+ad.Hit03+ad.Hit04+ad.Hit05+ad.Hit06+ad.Hit07+ad.Hit08+ad.Hit09+ad.Hit10+ad.Hit11+ad.Hit12+ad.Hit13+ad.Hit14+ad.Hit15+ad.Hit16+ad.Hit17+ad.Hit18+ad.Hit19+ad.Hit20+ad.Hit21+ad.Hit22+ad.Hit23) as hit		\n");
				sbQuery.Append("		from		SummaryAdHome ad with(NoLock)											\n");
				sbQuery.Append("		where		LogDay Between @LogDay1 and @LogDay2									\n");
				sbQuery.Append("		group by LEFT(LogDay, 4)															\n");
				sbQuery.Append("		union all																			\n");
				sbQuery.Append("		select		LEFT(LogDay, 4)															\n");
                sbQuery.Append("				, AdTargetsHanaTV.dbo.ufnLastDayOfMonth(LEFT(LogDay, 4))									\n");
				sbQuery.Append("				,999		as AdTypeCode													\n");
				sbQuery.Append("				,sum(ad.Hit00+ad.Hit01+ad.Hit02+ad.Hit03+ad.Hit04+ad.Hit05+ad.Hit06+ad.Hit07+ad.Hit08+ad.Hit09+ad.Hit10+ad.Hit11+ad.Hit12+ad.Hit13+ad.Hit14+ad.Hit15+ad.Hit16+ad.Hit17+ad.Hit18+ad.Hit19+ad.Hit20+ad.Hit21+ad.Hit22+ad.Hit23) as hit		\n");
				sbQuery.Append("		from		SummaryPgJump ad with(NoLock)											\n");
				sbQuery.Append("		where		LogDay Between @LogDay1 and @LogDay2									\n");
				sbQuery.Append("		group by LEFT(LogDay, 4)															\n");
				sbQuery.Append(")																							\n");
				sbQuery.Append("select -1 gubun, -1 adTypeCode, '' adTypeName												\n");
				for(int i=0; i<adt.Length; i++)
				{
					sbQuery.Append("		, '"+ ((string)adt[i]).Substring(0, 2) +"."+ ((string)adt[i]).Substring(2, 2) +"' c"+ i +"		\n");
				}
				sbQuery.Append("union all																					\n");
				sbQuery.Append("select v3.gubun, v3.adTypeCode		-- Order순서											\n");
				sbQuery.Append("		, case v3.gubun																		\n");
				sbQuery.Append(" 		when 0 then																			\n");
				sbQuery.Append(" 				case adTypeCode																\n");
				sbQuery.Append(" 						when 100 then		'노출물량 ( 로딩 )'								\n");
				sbQuery.Append(" 						when 888 then		'노출물량 ( 홈 )'								\n");
				sbQuery.Append(" 						when 999 then		'노출물량 ( 점핑 )'								\n");
                sbQuery.Append("						else  (select '노출물량 ( ' + CodeName + ' )' from AdTargetsHanaTV.dbo.SystemCode sy with(NoLock) where sy.Section = 26 and sy.Code = adTypeCode )		\n");
				sbQuery.Append("				end																			\n");
				sbQuery.Append("						when 1 then															\n");
                sbQuery.Append("								(select '점 유 율 ( ' + CodeName + ' )' from AdTargetsHanaTV.dbo.SystemCode sy with(NoLock) where sy.Section = 26 and sy.Code = AdTypeCode )		\n");
				sbQuery.Append("						when 2 then															\n");
				sbQuery.Append("								case adTypeCode												\n");
				sbQuery.Append("										when 100   then		'도 달 률 ( 전체 )'				\n");
                sbQuery.Append("										else  (select '도 달 률 ( ' + CodeName + ' )' from AdTargetsHanaTV.dbo.SystemCode sy with(NoLock) where sy.Section = 26 and sy.Code = adTypeCode )		\n");
				sbQuery.Append("								end															\n");
				sbQuery.Append("						when 3 then															\n");
				sbQuery.Append("								case AdTypeCode												\n");
				sbQuery.Append("										when 100  then		'노출빈도 ( 전체 )'				\n");
                sbQuery.Append("										else  (select '노출빈도 ( ' + CodeName + ' )' from AdTargetsHanaTV.dbo.SystemCode sy with(NoLock) where sy.Section = 26 and sy.Code = AdTypeCode )		\n");
				sbQuery.Append("								end															\n");
				sbQuery.Append("				  		end		as adTypeName												\n");
				for(int i=0; i<adt.Length; i++)
				{
					sbQuery.Append("				  		, case when gubun = 0 then replace( Convert( varchar(20),Convert(money,v3.c"+ i +"),1),'.00','') when adTypeCode = -1 then '' else Convert( varchar(20),Convert(money,v3.c"+ i +"),1)		end		as c"+ i +"		\n");
				}
				sbQuery.Append("		from (																				\n");
				sbQuery.Append("				select v1.gubun																\n");
				sbQuery.Append("  						,v1.adTypeCode														\n");
				for(int i=0; i<adt.Length; i++)
				{
					sbQuery.Append("						, sum(isnull(case when LogMon = '"+ ((string)adt[i]).Substring(0, 4) +"' then v1.AdHitSum else 0 end, 0)) c"+ i +"		\n");
				}
				sbQuery.Append("				from (																		\n");
				sbQuery.Append("  						select LogMon														\n");
				sbQuery.Append("  								,0 gubun													\n");
				sbQuery.Append("  								,AdTypeCode													\n");
				sbQuery.Append("  								,AdHitSum													\n");
				sbQuery.Append("  						from adData															\n");
				sbQuery.Append("						union all select '', 1 gubun, -1 AdTypeCode, 0 AdHitSum				\n");
				sbQuery.Append("  						union all															\n");
				sbQuery.Append("  						select adData.LogMon												\n");
				sbQuery.Append("  								,1 gubun													\n");
				sbQuery.Append("  								,AdTypeCode													\n");
				sbQuery.Append("  								,cast(adHitSum as real) / sum(adHitSum) over(PARTITION BY LogMon)*100		\n");
				sbQuery.Append("						from adData															\n");
				sbQuery.Append(" 						where		adData.AdTypeCode < 100									\n");
				sbQuery.Append("						union all select '', 2 gubun, -1 AdTypeCode, 0 AdHitSum				\n");
				sbQuery.Append(" 						union all															\n");
				sbQuery.Append("  						select adData.LogMon												\n");
				sbQuery.Append("  								,2 gubun													\n");
				sbQuery.Append("  								,case ht.TypeCode when 0 then 100 else ht.TypeCode end		\n");
				sbQuery.Append("  								,cast(ht.DayUsers as real) / (select base.HouseTotal from SummaryBase base with(NoLock) where base.LogDay = adData.EndMon ) * 100		\n");
				sbQuery.Append("  						from		SummaryHouseHoldType ht with(NoLock)					\n");
				sbQuery.Append("						inner join adData on (ht.LogDay = adData.EndMon)					\n");
				sbQuery.Append(" 						where		adData.AdTypeCode = 100									\n");
				sbQuery.Append("  						and				ht.TypeSection = 26									\n");
				sbQuery.Append("						and				ht.TypeCode <> 15									\n");
				sbQuery.Append("						union all select '', 3 gubun, -1 AdTypeCode, 0 AdHitSum				\n");
				sbQuery.Append("  						union all															\n");
				sbQuery.Append("  						select adData.LogMon												\n");
				sbQuery.Append("  								,3															\n");
				sbQuery.Append("  								,adData.AdTypeCode											\n");
                sbQuery.Append("  								,case when (select ht.MonthUsers from dbo.SummaryHouseHoldType ht with(NoLock) where ht.LogDay = AdTargetsHanaTV.dbo.ufnLastDayOfMonth(adData.LogMon) and ht.TypeSection=26 and case ht.TypeCode when 0 then 100 else ht.TypeCode end = adData.AdTypeCode ) = 0 then 0	\n");
                sbQuery.Append("  										else cast(adHitSum as real) / (select ht.MonthUsers from dbo.SummaryHouseHoldType ht with(NoLock) where ht.LogDay = AdTargetsHanaTV.dbo.ufnLastDayOfMonth(adData.LogMon) and ht.TypeSection=26 and case ht.TypeCode when 0 then 100 else ht.TypeCode end = adData.AdTypeCode ) end	\n");
				sbQuery.Append("  						from		adData													\n");
				sbQuery.Append("  						where		adData.AdTypeCode < 888									\n");
				sbQuery.Append("				) v1																		\n");
				sbQuery.Append("				group by v1.gubun,v1.adTypeCode												\n");
				sbQuery.Append("		)v3																					\n");
				sbQuery.Append("order by 1, 2																				\n");

				// __DEBUG__
				_log.Debug(sbQuery.ToString());
				// __DEBUG__
				
				// 쿼리실행
				DataSet ds = new DataSet();
				_db.ExecuteQuery(ds,sbQuery.ToString());

				// 결과 DataSet의 데이터모델에 복사
				dateAdTypeSummaryRptModel.ReportDataSet = ds.Copy();

				// 결과
				dateAdTypeSummaryRptModel.ResultCnt = Utility.GetDatasetCount(dateAdTypeSummaryRptModel.ReportDataSet);

				ds.Dispose();

				// 결과코드 셋트
				dateAdTypeSummaryRptModel.ResultCD = "0000";

				// __DEBUG__
				_log.Debug("<출력정보>");
				_log.Debug("ResultCnt:[" + dateAdTypeSummaryRptModel.ResultCnt + "]");
				// __DEBUG__

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetMonthlyAdTypeSummaryRpt() End");
				_log.Debug("-----------------------------------------");
			}
			catch(Exception ex)
			{
				dateAdTypeSummaryRptModel.ResultCD = "3000";
				if(isNotReady)
				{
					dateAdTypeSummaryRptModel.ResultDesc = "해당 기간은 데이터가 집계되지 않았습니다.";
				}
				else
				{
					dateAdTypeSummaryRptModel.ResultDesc = "월별 광고종류별 조회중 오류가 발생하였습니다";
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

	#region  일자 계산 유틸클래스
	/// <summary>
	/// 일자 계산 유틸클래스
	/// </summary>
	public class DateUtil 
	{
		/// <summary>
		/// 월요일을 주초로 볼 때 화면에 출력되는 일자 형식을 표시한다.
		/// </summary>
		/// <param name="src"></param>
		/// <returns></returns>
		public static string getDispWeekForStartMonday(string src)
		{
			try
			{
				DateTime date = DateTime.ParseExact(src, "yyMMdd", System.Globalization.CultureInfo.CurrentCulture);
				date = (getFirstDayOfWeekForStartMonday(date));
				string rtnStr = date.ToString("yyMMdd");
				return rtnStr.Substring(0, 2) +"."+ rtnStr.Substring(2, 2) +"."+ rtnStr.Substring(4, 2);
			}
			catch(Exception)
			{
				return src;
			}
		}

		/// <summary>
		/// 월요일을 주초로 볼 때 해당하는 주의 첫 일자를 가지고 온다.
		/// </summary>
		/// <param name="date"></param>
		/// <returns></returns>
		public static DateTime getFirstDayOfWeekForStartMonday(DateTime date)
		{
			//월요일이 처음 시작으로 보기 때문에 빼야하는 날짜를 이렇게 산술적으로 구한다.
			int dof = (1 - (int)date.DayOfWeek);
			if(dof == 1)
			{
				dof = -6;
			}
			return date.AddDays(dof);
		}

		/// <summary>
		/// 월요일을 주초로 볼 때 해당하는 주의 마지막일자를 가지고 온다.
		/// </summary>
		/// <param name="date"></param>
		/// <returns></returns>
		public static DateTime getLastDayOfWeekForStartMonday(DateTime date)
		{
			//월요일이 처음 시작으로 보기 때문에 더해야하는 날짜를 이렇게 산술적으로 구한다.
			return date.AddDays((7 - (int)date.DayOfWeek) % 7);
		}

		/// <summary>
		/// 월말을 리턴한다.
		/// </summary>
		/// <param name="date"></param>
		/// <returns></returns>
		public static DateTime getLastDayOfMonth(DateTime date)
		{
			date = date.AddMonths(1);
			return date.AddDays(-1 * date.Day);
		}

		/// <summary>
		/// 월초를 리턴한다.
		/// </summary>
		/// <param name="date"></param>
		/// <returns></returns>
		public static DateTime getFirstDayOfMonth(DateTime date)
		{
			return date.AddDays(-1 * date.Day + 1);
		}

	}

	#endregion
}