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
    /// BizManageBiz에 대한 요약 설명입니다.
    /// </summary>
    public class BizManageBiz : BaseBiz
    {
        public BizManageBiz()
            : base(FrameSystem.connSummaryDbString)
        {
            _log = FrameSystem.oLog;
        }

        /// <summary>
        /// 영업관리대상 광고 판매 목록조회
        /// </summary>
        /// <param name="header"></param>
        /// <param name="agencyModel"></param>
        public void GetBizManageTargetList(HeaderModel header, BizManageModel model)
        {
            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "GetBizManageTargetList() Start");
                _log.Debug("-----------------------------------------");

                // 데이터베이스를 OPEN한다
                _db.Open();

                // __DEBUG__
                _log.Debug("<입력정보>");
                _log.Debug("SearchStartKey  :[" + model.SearchStartDay + "]");
                _log.Debug("SearchEndKey    :[" + model.SearchEndDay + "]");
                // __DEBUG__

                string startDay = model.SearchStartDay; // yyyyMMdd
                string endDay = model.SearchEndDay;
                string shortStartDay = model.SearchStartDay.Substring(2, 6); // yyMMdd
                string shortEndDay = model.SearchEndDay.Substring(2, 6);

                StringBuilder sbQuery = new StringBuilder();
                // 쿼리생성
                sbQuery.AppendLine("    select	ati.AdTypeShort				as AdTypeName			-- 1. 타겟팅 종류 문자열 마지막 ',' 제거                                                               ");
                sbQuery.AppendLine("        ,	ad.AdvertiserName			as AdvertiserName		-- 2. 모든 명칭 표시                                                                                   ");
                sbQuery.AppendLine("        ,	v4.CampaignName				as BrandName                                                                                                                   ");
                sbQuery.AppendLine("        ,	jc2.JobName					as JobClassName                                                                                                                ");
                sbQuery.AppendLine("        ,	m.RapName					as RapName                                                                                                                     ");
                sbQuery.AppendLine("        ,	ag.AgencyName				as AgencyName                                                                                                                  ");
                sbQuery.AppendLine("        ,	v4.sDay						as StartDay                                                                                                                    ");
                sbQuery.AppendLine("        ,	v4.eDay						as EndDay                                                                                                                      ");
                sbQuery.AppendLine("        ,	substring(v4.eDay, 1, 6)	as EndMonth                                                                                                                    ");
                sbQuery.AppendLine("        ,	v4.Price					as Price                                                                                                                       ");
                sbQuery.AppendLine("        ,	v4.adTime					as AdTime                                                                                                                      ");
                sbQuery.AppendLine("        ,	v4.cAmt						as OfferExpCnt                                                                                                                 ");
                sbQuery.AppendLine("        ,	v4.adCnt					as RealExpCnt                                                                                                                  ");
                sbQuery.AppendLine("        ,	v4.OfferCPM					as OfferCPM                                                                                                                    ");
                sbQuery.AppendLine("        ,	v4.RealCPM					as RealCPM                                                                                                                     ");
                sbQuery.AppendLine("        ,	'TgtCategoryName' = CASE WHEN len(v4.tgtCategory) > 0 THEN substring(v4.tgtCategory, 1, len(v4.tgtCategory) - 1) ELSE '' END                               ");
                sbQuery.AppendLine("    from AdTargetsHanaTV.dbo.Contract as c with(nolock)                                                                                                                ");
                sbQuery.AppendLine("    inner join (                                                                                                                                                       ");
                sbQuery.AppendLine("                select	v3.adType					-- 1. 제안 CPM 계산                                                                                                ");
                sbQuery.AppendLine("                    ,	cm2.ContractSeq				-- 2. 결과 CPM 계산                                                                                                ");
                sbQuery.AppendLine("                    ,	cm2.CampaignName			-- 3. 타겟팅 종류 문자열 변환                                                                                      ");
                sbQuery.AppendLine("                    ,	v3.sDay                                                                                                                                        ");
                sbQuery.AppendLine("                    ,	v3.eDay                                                                                                                                        ");
                sbQuery.AppendLine("                    ,	cm2.Price                                                                                                                                      ");
                sbQuery.AppendLine("                    ,	v3.adTime                                                                                                                                      ");
                sbQuery.AppendLine("                    ,	v3.cAmt                                                                                                                                        ");
                sbQuery.AppendLine("                    ,	(select Convert(decimal(12, 0) ,Round(((cm2.Price / v3.cAmt) * 1000), 0)) where v3.cAmt > 0) as OfferCPM                                       ");
                sbQuery.AppendLine("                    ,	v3.adCnt                                                                                                                                       ");
                sbQuery.AppendLine("                    ,	(select Convert(decimal(12, 0) ,Round(((cm2.Price / convert(decimal(12, 0), v3.adCnt)) * 1000), 0)) where v3.adCnt > 0) as RealCPM             ");
                sbQuery.AppendLine("                    ,	'tgtCategory' =	  CASE WHEN v3.tgtRegion1Yn = 'Y'	 THEN '지역,'		ELSE '' END                                                            ");
                sbQuery.AppendLine("    					                + CASE WHEN v3.tgtTimeYn = 'Y'		 THEN '시간,'		ELSE '' END                                                            ");
                sbQuery.AppendLine("    					                + CASE WHEN v3.tgtAgeYn = 'Y'		 THEN '연령대,'		ELSE '' END                                                            ");
                sbQuery.AppendLine("    					                + CASE WHEN v3.tgtAgeBtnYn = 'Y'	 THEN '연령구간,'	ELSE '' END                                                            ");
                sbQuery.AppendLine("    					                + CASE WHEN v3.tgtSexYn = 'Y'		 THEN '성별,'		ELSE '' END                                                            ");
                sbQuery.AppendLine("    					                + CASE WHEN v3.tgtRateYn = 'Y'		 THEN '등급,'		ELSE '' END                                                            ");
                sbQuery.AppendLine("    					                + CASE WHEN v3.tgtWeekYn = 'Y'		 THEN '요일,'		ELSE '' END                                                            ");
                sbQuery.AppendLine("    					                + CASE WHEN v3.tgtCollectionYn = 'Y' THEN '고객군,'		ELSE '' END                                                            ");
                sbQuery.AppendLine("    					                + CASE WHEN v3.tgtZipYn = 'Y'		 THEN '우편번호,'	ELSE '' END                                                            ");
                sbQuery.AppendLine("    					                + CASE WHEN v3.tgtPPxYn = 'Y'		 THEN '유료채널,'	ELSE '' END                                                            ");
                sbQuery.AppendLine("    					                + CASE WHEN v3.tgtFreqYn = 'Y'		 THEN '빈도,'		ELSE '' END                                                            ");
                sbQuery.AppendLine("    					                + CASE WHEN v3.tgtPVDBYn = 'Y'		 THEN '개인시청DB,'	ELSE '' END                                                            ");
                sbQuery.AppendLine("    					                + CASE WHEN v3.tgtStbTypeYn = 'Y'	 THEN '셋탑,'		ELSE '' END                                                            ");
                sbQuery.AppendLine("    					                + CASE WHEN v3.tgtPrefYn = 'Y'		 THEN '선호도조사,'	ELSE '' END                                                            ");
                sbQuery.AppendLine("    					                + CASE WHEN v3.tgtProfileYn = 'Y'	 THEN '프로파일,'	ELSE '' END                                                            ");
                sbQuery.AppendLine("    					                + CASE WHEN v3.tgtPoCYn = 'Y'		 THEN 'POC,'		ELSE '' END                                                            ");
                sbQuery.AppendLine("                from AdTargetsHanaTV.dbo.CampaignMaster as cm2 with(noLock)                                                                                            ");
                sbQuery.AppendLine("                inner join (                                                                                                                                           ");
                sbQuery.AppendLine("    		                select	cm.CampaignCode							-- 캠페인 단위                                                                             ");
                sbQuery.AppendLine("    			                ,	max(v2.adType)	as adType				--	1. 실집행노출수 합                                                                     ");
                sbQuery.AppendLine("    			                ,	max(v2.sDay)	as sDay					--	2. 보장노출수 합                                                                       ");
                sbQuery.AppendLine("    			                ,	max(v2.eDay)	as eDay					--	3. 타겟팅 종류 결정(max()로 OR 연산 수행 : 'Y', 'N', '-' 중 max() 실행시 결과는 'Y')   ");
                sbQuery.AppendLine("    			                ,	max(v2.adTime)	as adTime                                                  ");
                sbQuery.AppendLine("    			                ,	sum(ISNULL(v2.adCnt, 0)) as adCnt                                          ");
                sbQuery.AppendLine("    			                ,	sum(ISNULL(v2.cAmt, 0))	 as cAmt                                           ");
                sbQuery.AppendLine("    			                ,	max(v2.tgtRegion1Yn)	 as tgtRegion1Yn                                   ");
                sbQuery.AppendLine("    			                ,	max(v2.tgtTimeYn)		 as tgtTimeYn                                      ");
                sbQuery.AppendLine("    			                ,	max(v2.tgtAgeYn)		 as tgtAgeYn                                       ");
                sbQuery.AppendLine("    			                ,	max(v2.tgtAgeBtnYn)		 as tgtAgeBtnYn                                    ");
                sbQuery.AppendLine("    			                ,	max(v2.tgtSexYn)		 as tgtSexYn                                       ");
                sbQuery.AppendLine("    			                ,	max(v2.tgtRateYn)		 as tgtRateYn                                      ");
                sbQuery.AppendLine("    			                ,	max(v2.tgtWeekYn)		 as tgtWeekYn                                      ");
                sbQuery.AppendLine("    			                ,	max(v2.tgtCollectionYn)	 as tgtCollectionYn                                ");
                sbQuery.AppendLine("    			                ,	max(v2.tgtZipYn)		 as tgtZipYn                                       ");
                sbQuery.AppendLine("    			                ,	max(v2.tgtPPxYn)		 as tgtPPxYn                                       ");
                sbQuery.AppendLine("    			                ,	max(v2.tgtFreqYn)		 as tgtFreqYn                                      ");
                sbQuery.AppendLine("    			                ,	max(v2.tgtPVDBYn)		 as tgtPVDBYn                                      ");
                sbQuery.AppendLine("    			                ,	max(v2.tgtStbTypeYn)	 as tgtStbTypeYn                                   ");
                sbQuery.AppendLine("    			                ,	max(v2.tgtPrefYn)		 as tgtPrefYn                                      ");
                sbQuery.AppendLine("    			                ,	max(v2.tgtProfileYn)	 as tgtProfileYn                                   ");
                sbQuery.AppendLine("    			                ,	max(v2.tgtPoCYn)		 as tgtPoCYn                                       ");
                sbQuery.AppendLine("    		                from AdTargetsHanaTV.dbo.CampaignMaster as cm with(noLock)                         ");
                sbQuery.AppendLine("    		                inner merge join (                                                                 ");
                sbQuery.AppendLine("    					                select	v.ItemNo								-- 캠페인 아이템 단위  ");
                sbQuery.AppendLine("    						                ,	v.CampaignCode							--	1. 실집행노출수 합 ");
                sbQuery.AppendLine("    						                ,	max(v.ExcuteEndDay)		as eDay			--	2. 타겟팅 종류 결정(max()로 OR 연산 수행 : 'Y', 'N', '-' 중 max() 실행시 결과는 'Y')   ");
                sbQuery.AppendLine("    						                ,	min(v.ExcuteStartDay)	as sDay                             ");
                sbQuery.AppendLine("    						                ,	max(v.AdTime)			as adTime                           ");
                sbQuery.AppendLine("    						                ,	min(v.AdType)			as adType                           ");
                sbQuery.AppendLine("    						                ,	min(v.ContractAmt)		as cAmt                             ");
                sbQuery.AppendLine("    						                ,	sum(sad.AdCnt)			as adCnt                            ");
                sbQuery.AppendLine("    						                ,	max(v.tgtRegion1Yn)		as tgtRegion1Yn                     ");
                sbQuery.AppendLine("    						                ,	max(v.tgtTimeYn)		as tgtTimeYn                        ");
                sbQuery.AppendLine("    						                ,	max(v.tgtAgeYn)			as tgtAgeYn                         ");
                sbQuery.AppendLine("    						                ,	max(v.tgtAgeBtnYn)		as tgtAgeBtnYn                      ");
                sbQuery.AppendLine("    						                ,	max(v.tgtSexYn)			as tgtSexYn                         ");
                sbQuery.AppendLine("    						                ,	max(v.tgtRateYn)		as tgtRateYn                        ");
                sbQuery.AppendLine("    						                ,	max(v.tgtWeekYn)		as tgtWeekYn                        ");
                sbQuery.AppendLine("    						                ,	max(v.tgtCollectionYn)	as tgtCollectionYn                  ");
                sbQuery.AppendLine("    						                ,	max(v.tgtZipYn)			as tgtZipYn                         ");
                sbQuery.AppendLine("    						                ,	max(v.tgtPPxYn)			as tgtPPxYn                         ");
                sbQuery.AppendLine("    						                ,	max(v.tgtFreqYn)		as tgtFreqYn                        ");
                sbQuery.AppendLine("    						                ,	max(v.tgtPVDBYn)		as tgtPVDBYn                        ");
                sbQuery.AppendLine("    						                ,	max(v.tgtStbTypeYn)		as tgtStbTypeYn                     ");
                sbQuery.AppendLine("    						                ,	max(v.tgtPrefYn)		as tgtPrefYn                        ");
                sbQuery.AppendLine("    						                ,	max(v.tgtProfileYn)		as tgtProfileYn                     ");
                sbQuery.AppendLine("    						                ,	max(v.tgtPoCYn)			as tgtPoCYn                         ");
                sbQuery.AppendLine("    					                from AdTargetsSummary.dbo.SummaryAdDaily3 as sad with(nolock)       ");
                sbQuery.AppendLine("    					                inner merge join (                                                  ");
                sbQuery.AppendLine("    								                select	ci.itemNo         			-- 1. 영업 관리 대상 캠페인 구간내 월별(종료일기준) 필수 정보 추출                                                          ");
                sbQuery.AppendLine("    									                ,   cd.CampaignCode				-- 2. 타겟팅 정보 NULL 값 -> 'N'으로 변환(DBMS에 따라 max 함수 실행시 Null이 있는 경우 무조건 Null로 반환하는 경우가 있음)  ");
                sbQuery.AppendLine("    									                ,   ci.ExcuteStartDay 		                                                        ");
                sbQuery.AppendLine("    									                ,   ci.ExcuteEndDay                                                                 ");
                sbQuery.AppendLine("    									                ,   ci.AdTime                                                                       ");
                sbQuery.AppendLine("    									                ,   ci.AdType                                                                       ");
                sbQuery.AppendLine("    									                ,   tg.ContractAmt                                                                  ");
                sbQuery.AppendLine("    									                ,	isnull(tg.TgtRegion1Yn, 'N')	as tgtRegion1Yn                                 ");
                sbQuery.AppendLine("    									                ,	isnull(tg.TgtTimeYn, 'N')		as tgtTimeYn                                    ");
                sbQuery.AppendLine("    									                ,	isnull(tg.TgtAgeYn, 'N')		as tgtAgeYn                                     ");
                sbQuery.AppendLine("    									                ,	isnull(tg.TgtAgeBtnYn, 'N')		as tgtAgeBtnYn                                  ");
                sbQuery.AppendLine("    									                ,	isnull(tg.TgtSexYn, 'N')		as tgtSexYn                                     ");
                sbQuery.AppendLine("    									                ,	isnull(tg.TgtRateYn, 'N')		as tgtRateYn                                    ");
                sbQuery.AppendLine("    									                ,	isnull(tg.TgtWeekYn, 'N')		as tgtWeekYn                                    ");
                sbQuery.AppendLine("    									                ,	isnull(tg.TgtCollectionYn, 'N') as tgtcollectionYn                              ");
                sbQuery.AppendLine("    									                ,	isnull(tg.TgtZipYn, 'N')		as tgtZipYn                                     ");
                sbQuery.AppendLine("    									                ,	isnull(tg.TgtPPxYn, 'N')		as tgtPPxYn                                     ");
                sbQuery.AppendLine("    									                ,	isnull(tg.TgtFreqYn, 'N')		as tgtFreqYn                                    ");
                sbQuery.AppendLine("    									                ,	isnull(tg.TgtPVDBYn, 'N')		as tgtPVDBYn                                    ");
                sbQuery.AppendLine("    									                ,	isnull(tg.TgtStbTypeYn, 'N')	as tgtStbTypeYn                                 ");
                sbQuery.AppendLine("    									                ,	isnull(tg.TgtPrefYn, 'N')		as tgtPrefYn                                    ");
                sbQuery.AppendLine("    									                ,	isnull(tg.TgtProfileYn, 'N')	as tgtProfileYn                                 ");
                sbQuery.AppendLine("    									                ,	isnull(tg.TgtPoCYn, 'N')		as tgtPocYn                                     ");
                sbQuery.AppendLine("    								                from AdTargetsHanaTV.dbo.CampaignDetail		as cd with(nolock)                          ");
                sbQuery.AppendLine("                                                    inner join AdTargetsHanaTV.dbo.CampaignMaster as useCamp with(nolock) on useCamp.CampaignCode = cd.CampaignCode ");
                sbQuery.AppendLine("    								                inner join AdTargetsHanaTV.dbo.ContractItem as ci with(nolock) on ci.ItemNo = cd.ItemNo ");
                sbQuery.AppendLine("    								                inner join AdTargetsHanaTV.dbo.Targeting    as tg with(nolock) on tg.ItemNo = cd.ItemNo ");
                sbQuery.AppendLine("    								                where 1 = 1                                                                             ");
                sbQuery.AppendLine("    								                    and useCamp.BizManageTarget = 'Y'                                                   ");
                //sbQuery.AppendLine("    								                    and ci.MediaCode = 1                                                                ");
                sbQuery.AppendLine("                                                        and ci.ExcuteEndDay between '" + startDay + "' and '" + endDay + "'                 ");

                if (!model.SearchAdvertiserCode.Equals("00"))
                {
                    sbQuery.AppendLine("                                                    and ci.AdvertiserCode = " + model.SearchAdvertiserCode);
                }
                if (!model.SearchAgencyCode.Equals("00"))
                {
                    sbQuery.AppendLine("                                                    and ci.AgencyCode = " + model.SearchAgencyCode);
                }
                if (!model.SearchRapCode.Equals("00"))
                {
                    sbQuery.AppendLine("                                                    and ci.RapCode = " + model.SearchRapCode);
                }
                if (!model.SearchAdType.Equals("00"))
                {
                    sbQuery.AppendLine("                                                    and ci.AdType = " + model.SearchAdType);
                }

                sbQuery.AppendLine("    							                ) as v on v.ItemNo = sad.ItemNo                                                             ");
                sbQuery.AppendLine("    					                where  sad.logday between '" + shortStartDay + "' and '" + shortEndDay + "'                         ");
                sbQuery.AppendLine("    					                group by v.itemNo, v.CampaignCode                                                                   ");
                sbQuery.AppendLine("    				                ) as v2 on v2.CampaignCode = cm.CampaignCode                                                            ");
                sbQuery.AppendLine("    		                where cm.BizManageTarget = 'Y'                                                                                  ");
                sbQuery.AppendLine("    		                group by cm.CampaignCode, substring(v2.eday,1,6)                                                                ");
                sbQuery.AppendLine("                    ) as v3 on v3.CampaignCode = cm2.CampaignCode                                                                           ");
                sbQuery.AppendLine("            ) as v4 on v4.ContractSeq = c.ContractSeq                                                                                       ");
                sbQuery.AppendLine("    inner join AdTargetsHanaTV.dbo.AdTypeInfo as ati with(nolock) on ati.AdType = v4.adType                                                 ");
                sbQuery.AppendLine("    inner join AdTargetsHanaTV.dbo.Advertiser as ad with(nolock) on ad.AdvertiserCode = c.AdvertiserCode                                    ");
                sbQuery.AppendLine("    left join AdTargetsHanaTV.dbo.JobClass as jc1 with(nolock) on jc1.JobCode = ad.JobClass                                                 ");
                sbQuery.AppendLine("    left join AdTargetsHanaTV.dbo.JobClass as jc2 with(nolock) on jc2.JobCode = jc1.Level1Code                                              ");
                sbQuery.AppendLine("    left join AdTargetsHanaTV.dbo.Agency as ag with(nolock) on ag.AgencyCode = c.AgencyCode                                                 ");
                sbQuery.AppendLine("    left join AdTargetsHanaTV.dbo.MediaRap as m with(nolock) on m.RapCode = c.RapCode                                                       ");
                sbQuery.AppendLine("    order by EndMonth, RapName, EndDay                                                                                                      ");

                // __DEBUG__
                _log.Debug(sbQuery.ToString());
                // __DEBUG__

                // 쿼리실행
                DataSet ds = new DataSet();
                _db.ExecuteQuery(ds, sbQuery.ToString());

                // 결과 DataSet의 대행사모델에 복사
                model.BizManageDataSet = ds.Copy();
                // 결과
                model.ResultCnt = Utility.GetDatasetCount(model.BizManageDataSet);
                // 결과코드 셋트
                model.ResultCD = "0000";

                // __DEBUG__
                _log.Debug("<출력정보>");
                _log.Debug("ResultCnt:[" + model.ResultCnt + "]");
                // __DEBUG__

                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "GetBizManageTargetList() End");
                _log.Debug("-----------------------------------------");
            }
            catch (Exception ex)
            {
                model.ResultCD = "3000";
                model.ResultDesc = "영업관리대상 광고 판매 목록 조회중 오류가 발생하였습니다";
                _log.Exception(ex);
            }
            finally
            {
                // 데이터베이스를  Close한다
                _db.Close();
            }
        }
    }
}