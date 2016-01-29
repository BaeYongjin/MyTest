/*
 * -------------------------------------------------------
 * Class Name: TargetingBiz
 * 주요기능  : 상업광고 타겟팅 처리 로직
 * 작성자    : 모름
 * 작성일    : 모름
 * 특이사항  : 지역정보 확장을 위해 기능 추가 -bae
 * -------------------------------------------------------
 * [수정사항]
 * -------------------------------------------------------
 * 수정코드  : [E_01]
 * 수정자    : bae
 * 수정일    : 2010.07.28
 * 수정부분  :
 *			  - GetRegionList(..)
 *            - SetTargetingDetailUpdate(..)
 * 수정내용  : 
 *            - 지역정보 3단계 구조 확장으로 질의문 수정
 *            - 우편번호 복수개 등록  
 * --------------------------------------------------------
 * 수정코드  : [E_02]
 * 수정자    : bae
 * 수정일    : 2010.09.01
 * 수정부분  :
 *			  - 
 *            - 
 * 수정내용  : 
 *            - 시간 타겟팅 주중/주말 타겟팅으로 Column 길이변경
 *             128 -> 146 증가.(24x2x2+((3개 구분자)+(^)문자최대수) 총합계
 *            - 
 * --------------------------------------------------------
 * 수정코드  : [E_03]
 * 수정자    : bae
 * 수정일    : 2010.10.04
 * 수정부분  :
 *            
 *			  - SetTargetingDetailUpdate_10_04(..)추가
 *            - GetTargetingDetail_10_04(..) 추가
 * 수정내용  : 
 *            - 2slot column 추가 : 롤백쉽게하기 위해서 method 추가.
 * 
 * 
 * ---------------------------------------------------------
 * 2012.02.16 고객군타겟팅 추가 RH.Jung
 * --------------------------------------------------------
 * 수정코드  : [E_05]
 * 수정자    : 김보배
 * 수정일    : 2013.04.01
 * 수정부분  :
 *			  - GetTargetingDetail_10_04() 수정
 *			  - SetTargetingDetailUpdate_10_04() 수정
 *            - GetStbList() 추가
 * 수정내용  : 
 *            - 셋탑별 타겟팅 기능 추가
 * ---------------------------------------------------------
 * 수정코드  : [E_06]
 * 수정자    : 김보배
 * 수정일    : 2013.07.09
 * 수정부분  :
 *            - GetTargetingDetail_10_04() 수정
 *            - SetTargetingDetailUpdate_10_04() 수정
 * 수정내용  : 
 *            - 선호도조사팝업 기능 추가
 * --------------------------------------------------------- 
 * 수정코드  : [E_07]
 * 수정자    : 김보배
 * 수정일    : 2013.10.16
 * 수정부분  :
 *            - GetTargetingDetail_10_04() 수정
 *            - SetTargetingDetailUpdate_10_04() 수정
 *            - SetTargetingProfileAdd() 추가
 * 수정내용  : 
 *            - 프로파일 타겟팅 사용 기능 추가
 * ---------------------------------------------------------
 * 수정코드  : [E_08]
 * 수정자    : H.J.LEE
 * 수정일    : 2014.08.19
 * 수정부분  :
 *			  - GetAgeList() 수정
 * 수정내용  : 
 *            - DB 이중화 작업으로 HanaTV , Summary로 분리됨
 *            - Summary의 테이블을 보는 함수가 있어서 해당
 *            함수만 Summary를 보도록 수정함
 * -------------------------------------------------------- 
 * 수정코드  : [E_09]
 * 수정자    : YH.Kim
 * 수정일    : 2015.01.14
 * 수정부분  :
 *			  - GetTargetingList() 수정
 * 수정내용  : 
 *            - 최명환M 요청으로 AdTime(광고시간) 컬럼 추가
  * --------------------------------------------------------
 */


using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Reflection;

using WinFramework.Data;
using WinFramework.Misc;
using WinFramework.Base;

using AdManagerModel;
using Oracle.DataAccess.Client;

namespace AdManagerWebService.Target
{
    /// <summary>
    /// TargetingBiz에 대한 요약 설명입니다.
    /// </summary>
    public class TargetingBiz : BaseBiz
    {
		#region  생성자
        public TargetingBiz() : base(FrameSystem.connDbString, true)
        {
            _log = FrameSystem.oLog;
        }
		#endregion

		#region [S1] 타겟팅 리스트 조회
        /// <summary>
        /// 타겟팅 리스트 조회
        /// </summary>
        /// <param name="targetingModel"></param>
        public void GetTargetingList(HeaderModel header, TargetingModel targetingModel)
        {
			bool isState = false;
            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + ".GetTargetingList() Start");
                _log.Debug("-----------------------------------------");

				_db.Open();
                StringBuilder sb = new StringBuilder();

				#region [ 쿼리생성 ]
                //sb.Append("\n ");
                //sb.Append("\n " + "SELECT B.ItemNo ");
                //sb.Append("\n " + "      ,B.ItemName ");
                //sb.Append("\n " + "      ,C.ContractName ");
                //sb.Append("\n " + "      ,D.AdvertiserName ");
                //sb.Append("\n " + "      ,'*' as ContStateName ");
                //sb.Append("\n " + "      ,B.ExcuteStartDay ");
                //sb.Append("\n " + "      ,B.ExcuteEndDay ");
                //sb.Append("\n " + "      ,B.RealEndDay ");
                //sb.Append("\n " + "      ,F.CodeName as AdTypeName ");
                //sb.Append("\n " + "      ,G.CodeName as AdStatename ");
                //sb.Append("\n " + "      ,C.ContractAmt ");
                //sb.Append("\n " + "      ,H.PriorityCd ");
                //sb.Append("\n " + "      ,B.MediaCode ");
                //sb.Append("\n " + "      ,ISNULL(H.ItemNo,0)  As TgtItemNo ");
                //sb.Append("\n " + "		 , case when h.ItemNo is null																					then 0 ");
                //sb.Append("\n " + "             when h.ItemNo > 0 and b.AdType in(10,15,16,17,19) and ( h.tgtRegion1Yn = 'N' and h.tgtTimeYn = 'N' and h.tgtWeekYn = 'N') then 0 ");
                //sb.Append("\n " + "				when h.ItemNo > 0 and b.AdType in(10,15,16,17,19) and ( h.tgtRegion1Yn = 'Y' or h.tgtTimeYn = 'Y' or h.tgtWeekYn = 'Y') ");
                //sb.Append("\n " + "      			then case when ( select count(*) from TargetingRate with(noLock) where ItemNo = b.ItemNo ) > 0 then 2 else 1 end ");
                //sb.Append("\n " + "             when h.ItemNo > 0 and b.AdType in(11,12,20)  then 0  ");
                //sb.Append("\n " + "      		else            2	 ");
                //sb.Append("\n " + "        end as RateItemNo ");
                //sb.Append("\n " + "      ,H.ContractAmt AS TgtAmount ");
                //sb.Append("\n " + "      ,isnull(H.TgtCollectionYn,'N') as TgtCollection ");
                //sb.Append("\n " + "      ,B.AdType ");
                //sb.Append("\n " + "      ,I.CodeName ScheduleTypeName ");
                //sb.Append("\n " + "		 ,H.TgtRegion1Yn ");
                //sb.Append("\n " + "		 ,H.TgtTimeYn ");
                //sb.Append("\n " + "		 ,H.TgtWeekYn ");
                //// 2014.02.06 추가
                //sb.Append("\n " + "		 ,H.TgtRateYn       as TgtRateYn ");
                //sb.Append("\n " + "		 ,isnull(H.SlotExt,0)     as TgtSlotYn ");
                //sb.Append("\n " + "		 ,H.TgtPPxYn        as TgtPPxYn");
                //sb.Append("\n " + "		 ,H.TgtStbTypeYn    as TgtStbYn");
                //// 2015.01.14	"광고시간" 컬럼추가 [E_09]
                //sb.Append("\n " + "		 ,B.AdTime ");

                //sb.Append("\n " + "  FROM ContractItem B with(nolock) ");
                //sb.Append("\n " + "       INNER JOIN Contract   C with(nolock)  ON (B.MediaCode     = C.MediaCode ");
                //sb.Append("\n " + "                                             AND B.RapCode       = C.RapCode "); 
                //sb.Append("\n " + "                                             AND B.AgencyCode    = C.AgencyCode "); 
                //sb.Append("\n " + "                                             AND B.AdvertiserCode= C.AdvertiserCode "); 
                //sb.Append("\n " + "                                             AND B.ContractSeq   = C.ContractSeq) ");
                //sb.Append("\n " + "       LEFT  JOIN Advertiser D with(nolock) ON (B.AdvertiserCode = D.AdvertiserCode) ");
                //sb.Append("\n " + "       LEFT  JOIN SystemCode F with(nolock) ON (B.AdType         = F.Code AND F.Section = '26') ");
                //sb.Append("\n " + "       LEFT  JOIN SystemCode G with(nolock) ON (B.AdState        = G.Code AND G.Section = '25') ");
                //sb.Append("\n " + "       LEFT  JOIN Targeting  H with(nolock) ON (B.ItemNo         = H.ItemNo) ");
                //sb.Append("\n " + "       LEFT  JOIN SystemCode I with(NoLock) ON (B.ScheduleType   = I.Code AND I.Section = '27' ) ");
                //sb.Append("\n " + " WHERE B.ItemNo > 0 ");  
                sb.AppendLine();
                sb.AppendLine("     SELECT	B.ITEM_NO        AS ItemNo                                                       ");
                sb.AppendLine("         ,	B.ITEM_NM        AS ItemName                                                     ");
                sb.AppendLine("         ,	C.CNTR_NM        AS ContractName                                                 ");
                sb.AppendLine("         ,	D.ADVTER_NM      AS AdvertiserName                                               ");
                sb.AppendLine("         ,	'*'              AS ContStateName                                                ");
                sb.AppendLine("         ,	B.BEGIN_DY       AS ExcuteStartDay                                               ");
                sb.AppendLine("         ,	B.END_DY         AS ExcuteEndDay                                                 ");
                sb.AppendLine("         ,	B.RL_END_DY      AS RealEndDay                                                   ");
                sb.AppendLine("         ,	F.STM_COD_NM     AS AdTypeName                                                   ");
                sb.AppendLine("         ,	G.STM_COD_NM     AS AdStatename                                                  ");
                sb.AppendLine("         ,	C.CNTR_AMT       AS ContractAmt                                                  ");
                sb.AppendLine("         ,	0                AS PriorityCd                                                   ");
                sb.AppendLine("         ,	C.MDA_COD        AS MediaCode                                                    ");
                sb.AppendLine("         ,	NVL(H.ITEM_NO,0) As TgtItemNo                                                    ");
                sb.AppendLine("         ,	0                As RateItemNo                                                   ");
                sb.AppendLine("         ,	H.CONTR_QTY      AS TgtAmount                                                    ");
                sb.AppendLine("         ,	'N'              AS TgtCollection                                                ");
                sb.AppendLine("         ,	B.ADVT_TYP       AS AdType                                                       ");
                sb.AppendLine("         ,	I.STM_COD_NM     AS ScheduleTypeName                                             ");
                sb.AppendLine("         ,	H.TAR_AREA_YN    AS TgtRegion1Yn                                                 ");
                sb.AppendLine("         ,	H.TAR_HRZONE_YN  AS TgtTimeYn                                                    ");
                sb.AppendLine("         ,	H.TAR_WEEK_YN    AS TgtWeekYn                                                    ");
                sb.AppendLine("         ,	H.TAR_RATE_YN    AS TgtRateYn                                                    ");
                sb.AppendLine("         ,	0                AS TgtSlotYn	--,NVL(H.SlotExt,0)     as TgtSlotYn  -- 임시제외");
                sb.AppendLine("         ,	H.TAR_PPX_YN     AS TgtPPxYn                                                     ");
                sb.AppendLine("         ,	H.TAR_OS_YN      AS TgtStbYn                                                     ");
                sb.AppendLine("         ,	B.ADVT_TM        AS AdTime                                                       ");
                sb.AppendLine("     FROM ADVT_MST B                                                                          ");
                sb.AppendLine("         INNER JOIN CNTR    C ON B.CNTR_SEQ = C.CNTR_SEQ                                      ");
                sb.AppendLine("         LEFT  JOIN ADVTER  D ON C.ADVTER_COD = D.ADVTER_COD                                  ");
                sb.AppendLine("         LEFT  JOIN STM_COD F ON (B.ADVT_TYP = F.STM_COD AND F.STM_COD_CLS = '26')            ");
                sb.AppendLine("         LEFT  JOIN STM_COD G ON (B.ADVT_STT = G.STM_COD AND G.STM_COD_CLS = '25')            ");
                sb.AppendLine("         LEFT  JOIN TAR_MST H ON B.ITEM_NO = H.ITEM_NO                                        ");
                sb.AppendLine("         LEFT  JOIN STM_COD I ON (B.SCH_TYP = I.STM_COD AND I.STM_COD_CLS = '27')             ");
                sb.AppendLine("     WHERE   B.ITEM_NO > 0                                                                    ");
                sb.AppendLine("         AND C.MDA_COD = 1                                                                    ");
				// 매체을 선택했으면
                //if(targetingModel.SearchMediaCode.Trim().Length > 0 && !targetingModel.SearchMediaCode.Trim().Equals("00"))
                //{
                //    sb.AppendLine(" AND C.MDA_COD  = " + targetingModel.SearchMediaCode.Trim());
                //}	
				
				// 랩사를 선택했으면
				if(targetingModel.SearchRapCode.Trim().Length > 0 && !targetingModel.SearchRapCode.Trim().Equals("00"))
				{
                    sb.AppendLine("         AND C.REP_COD  = " + targetingModel.SearchRapCode.Trim());
				}	

				// 대행사를 선택했으면
				if(targetingModel.SearchAgencyCode.Trim().Length > 0 && !targetingModel.SearchAgencyCode.Trim().Equals("00"))
				{
                    sb.AppendLine("         AND C.AGNC_COD  = " + targetingModel.SearchAgencyCode.Trim());
				}	

				// 광고종류를 선택했으면
				if(targetingModel.SearchAdType.Trim().Length > 0 && targetingModel.SearchAdType.Trim().Equals("10"))
				{
					// 15코너광고는 편성이 아직 없고, 인터페이스만 구축되어 있슴
                    sb.AppendLine("         AND B.ADVT_TYP  in(10,13,14,16,17,19)");
				}
				else if(targetingModel.SearchAdType.Trim().Length > 0 && targetingModel.SearchAdType.Trim().Equals("20"))
				{
                    sb.AppendLine("         AND B.ADVT_TYP  in(11,12,20)");
				}

                // 광고타입을 지정한 경우
                if (targetingModel.SearchAdClass.Trim().Length > 0 && !targetingModel.SearchAdClass.Trim().Equals("00"))
                {
                    sb.AppendLine("         AND B.ADVT_TYP = " + targetingModel.SearchAdClass.Trim());  
                }

				// 광고상태 선택에 따라
				// 광고상태는 20:편성 과 40:종료 사이에 있는 것만 조회한다.
                sb.AppendLine("         AND B.ADVT_STT >= '20' AND B.ADVT_STT <= '40' ");

				// 광고상태 편성
				if(targetingModel.SearchchkAdState_20.Trim().Length > 0 && targetingModel.SearchchkAdState_20.Trim().Equals("Y"))
				{
                    sb.AppendLine("         AND ( B.ADVT_STT  = '20'");
					isState = true;
				}	
				// 광고상태 중지
				if(targetingModel.SearchchkAdState_30.Trim().Length > 0 && targetingModel.SearchchkAdState_30.Trim().Equals("Y"))
				{
                    if (isState) sb.Append("            OR ");
                    else sb.Append("                AND ( ");
                    sb.AppendLine(" B.ADVT_STT  = '30'");
					isState = true;
				}	
				// 광고상태 종료
				if(targetingModel.SearchchkAdState_40.Trim().Length > 0 && targetingModel.SearchchkAdState_40.Trim().Equals("Y"))
				{
                    if (isState) sb.Append("            OR ");
                    else sb.Append("                AND ( ");
                    sb.AppendLine(" B.ADVT_STT  = '40' ");
					isState = true;
				}

                if (isState) sb.AppendLine("                )");
				
				// 검색어가 있으면
				if (targetingModel.SearchKey.Trim().Length > 0)
				{
					// 여러컬럼에 대하여 LIKE 검색을 한다.
                    //sb.Append("\n"
                    //    + "  AND ( B.ItemNo         LIKE '%" + targetingModel.SearchKey.Trim() + "%' \n"
                    //    + "     OR B.ItemName       LIKE '%" + targetingModel.SearchKey.Trim() + "%' \n"
                    //    + "     OR C.ContractName   LIKE '%" + targetingModel.SearchKey.Trim() + "%' \n"
                    //    + "     OR D.AdvertiserName LIKE '%" + targetingModel.SearchKey.Trim() + "%' \n"
                    //    + "  ) \n");

                    sb.AppendLine("         AND ( B.ITEM_NO   LIKE '%" + targetingModel.SearchKey.Trim() + "%' ");
                    sb.AppendLine("             OR B.ITEM_NM   LIKE '%" + targetingModel.SearchKey.Trim() + "%' ");
                    sb.AppendLine("             OR C.CNTR_NM   LIKE '%" + targetingModel.SearchKey.Trim() + "%' ");
                    sb.AppendLine("             OR D.ADVTER_NM LIKE '%" + targetingModel.SearchKey.Trim() + "%' ");
                    sb.AppendLine("         )");
				}
       
				sb.Append("  ORDER BY B.ITEM_NO DESC ");

                // __DEBUG__
                _log.Debug(sb.ToString());
                // __DEBUG__
				#endregion
			
                // 쿼리실행
                DataSet ds = new DataSet();
                _db.ExecuteQuery(ds,sb.ToString());

                // 결과 처리	
				targetingModel.TargetingDataSet = ds.Copy();
                targetingModel.ResultCnt = Utility.GetDatasetCount(targetingModel.TargetingDataSet);
                targetingModel.ResultCD = "0000";

				ds.Dispose();

                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + ".GetTargetingList() End");
                _log.Debug("-----------------------------------------");	
            }
            catch(Exception ex)
            {
                targetingModel.ResultCD = "3000";
                targetingModel.ResultDesc = "타겟팅목록 조회중 오류가 발생하였습니다";
                _log.Exception(ex);
            }
			finally
			{
				_db.Close();
			}
        }
		#endregion

		#region 고객군목록조회 -------------------------------------------------------- 기능 제외 임시 폐기

		/// <summary>
		/// 고객군목록조회
		/// 미사용
		/// </summary>
		/// <param name="targetingModel"></param>
		public void GetCollectionList(HeaderModel header, TargetingModel targetingModel)
		{
            //try
            //{
            //    _log.Debug("-----------------------------------------");
            //    _log.Debug(this.ToString() + "GetCollectionList() Start");
            //    _log.Debug("-----------------------------------------");

            //    // 데이터베이스를 OPEN한다
            //    _db.Open();

            //    // __DEBUG__
            //    _log.Debug("<입력정보>");
            //    _log.Debug("SearchKey      :[" + targetingModel.SearchKey       + "]");				
            //    // __DEBUG__

            //    StringBuilder sbQuery = new StringBuilder();

            //    // 쿼리생성
            //    sbQuery.Append("\n"
            //        + "SELECT 'False' AS CheckYn            \n"
            //        + "		  ,A.CollectionCode, A.CollectionName, A.Comment  \n"								
            //        + "       ,A.UseYn              \n"
            //        + "       ,CASE A.UseYn WHEN 'Y' THEN '사용' WHEN 'N' THEN '사용안함' END AS UseYn_N  \n"
            //        + "       ,convert(char(19), A.RegDt, 120) RegDt              \n"
            //        + "       ,convert(char(19), A.ModDt, 120) ModDt              \n"					
            //        + "       ,B.UserName RegName                                 \n"
            //        + "       ,(Select count(*) from ClientList where CollectionCode = A.CollectionCode AND CollectionCode = A.CollectionCode) Cnt           \n"
            //        + "  FROM Collection A LEFT JOIN SystemUser B with(NoLock) ON (A.RegId          = B.UserId)        \n"					
            //        + " WHERE 1 = 1  \n"
            //        + "   AND A.UseYn = 'Y' "
            //        );
			
            //    // 검색어가 있으면
            //    if (targetingModel.SearchKey.Trim().Length > 0)
            //    {
            //        // 여러컬럼에 대하여 LIKE 검색을 한다.
            //        sbQuery.Append(" AND ("
            //            + "  A.CollectionName      LIKE '%" + targetingModel.SearchKey.Trim() + "%' \n"	
            //            + " OR A.Comment    LIKE '%" + targetingModel.SearchKey.Trim() + "%' \n"
            //            + " OR B.UserName    LIKE '%" + targetingModel.SearchKey.Trim() + "%' \n"						
            //            + " ) ");
            //    }
				
            //    sbQuery.Append(" ORDER BY A.CollectionCode desc \n");

            //    // __DEBUG__
            //    _log.Debug(sbQuery.ToString());
            //    // __DEBUG__
				
            //    // 쿼리실행
            //    DataSet ds = new DataSet();
            //    _db.ExecuteQuery(ds,sbQuery.ToString());

            //    // 결과 DataSet의 미디어렙모델에 복사
            //    targetingModel.CollectionsDataSet = ds.Copy();
            //    // 결과
            //    targetingModel.ResultCnt = Utility.GetDatasetCount(targetingModel.CollectionsDataSet);
            //    // 결과코드 셋트
            //    targetingModel.ResultCD = "0000";

            //    // __DEBUG__
            //    _log.Debug("<출력정보>");
            //    _log.Debug("ResultCnt:[" + targetingModel.ResultCnt + "]");
            //    // __DEBUG__

            //    _log.Debug("-----------------------------------------");
            //    _log.Debug(this.ToString() + "GetCollectionList() End");
            //    _log.Debug("-----------------------------------------");
            //}
            //catch(Exception ex)
            //{
            //    targetingModel.ResultCD = "3000";
            //    targetingModel.ResultDesc = "고객군정보 조회중 오류가 발생하였습니다";
            //    _log.Exception(ex);
            //}
            //finally
            //{
            //    // 데이터베이스를  Close한다
            //    _db.Close();			
            //}		
		}
		#endregion

		#region 광고 타겟팅 상세 조회
        /// <summary>
        /// 광고 타겟팅 상세 조회
        /// 2010.02.08일 타겟팅정보 항목추가(우편번호,유료광고여부,프리퀀시)
        /// 2012.02.10일 해당광고의 총괄 계약물량 구하는 부분 추가함.GetItemAmtTot()
        /// </summary>
        /// <param name="targetingModel"></param>
        public void GetTargetingDetail(HeaderModel header, TargetingModel targetingModel)
        {
            try
            {
                // 데이터베이스를 OPEN한다
                _db.Open();

                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "GetTargetingDetail() Start");
                _log.Debug("-----------------------------------------");

                // __DEBUG__
                _log.Debug("<입력정보>");
                _log.Debug("ItemNo :[" + targetingModel.ItemNo + "]");		// 광고번호


                // __DEBUG__
                StringBuilder sbQuery = new StringBuilder();

                // 쿼리생성
                sbQuery.Append("\n"
                    + "SELECT ItemNo						                 \n"
                    + "      ,ContractAmt					                 \n"
                    + "      ,PriorityValue					                 \n"
                    + "      ,PriorityCd				                 	 \n"
                    + "      ,AmtVariableHour				                 \n"
                    + "      ,AmtControlYn	,AmtControlRate	                 \n"
                    + "      ,TgtRegion1Yn	,TgtRegion1	                 	 \n"
                    + "      ,TgtTimeYn		,TgtTime		                 \n"
                    + "      ,TgtAgeYn		,TgtAge			                 \n"
                    + "      ,TgtAgeBtnYn	,TgtAgeBtnBegin	,TgtAgeBtnEnd	 \n"
                    + "      ,TgtSexYn		,TgtSexMan		,TgtSexWoman	 \n"
                    + "      ,TgtRateYn		,TgtRate                         \n"
                    + "      ,TgtWeekYn		,TgtWeek		                 \n"
                    + "      ,TgtCollectionYn                                \n"
                    + "      ,(select count(*) from  TargetingCollection x with(nolock) where x.ItemNo = y.ItemNo and x.SetType = '1') as TgtCollection    \n"
                    + "      ,TgtZipYn		,TgtZip			                 \n"
                    + "      ,TgtPPxYn			                             \n"
                    + "      ,TgtFreqYn		,isnull(TgtFreqDay,0) as TgtFreqDay , isnull(TgtFreqPeriod,0) as TgtFreqPeriod	\n"
                    + "      ,isnull(TgtPVDBYn,'N') as TgtPVSYn              \n"
                    + "      ,dbo.GetItemAmtTot( ItemNo )   as SumAmt        \n"
                    + " FROM	Targeting y with(nolock)			         \n"
                    + " WHERE	ItemNo = " + targetingModel.ItemNo + "      \n"
                    );

                // __DEBUG__
                _log.Debug(sbQuery.ToString());

                // 쿼리실행
                DataSet ds = new DataSet();
                _db.ExecuteQuery(ds, sbQuery.ToString());

                // 결과 DataSet의 장르그룹모델에 복사
                targetingModel.DetailDataSet = ds.Copy();
                targetingModel.ResultCnt = Utility.GetDatasetCount(targetingModel.DetailDataSet);
                targetingModel.ResultCD = "0000";
                ds.Dispose();

                // __DEBUG__
                _log.Debug("<출력정보>");
                _log.Debug("ResultCnt:[" + targetingModel.ResultCnt + "]");
                // __DEBUG__

                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "GetTargetingDetail() End");
                _log.Debug("-----------------------------------------");
            }
            catch (Exception ex)
            {
                targetingModel.ResultCD = "3000";
                targetingModel.ResultDesc = "광고 타겟팅 상세 조회중 오류가 발생하였습니다";
                _log.Exception(ex);
            }
            finally
            {
                _db.Close();
            }
        }

        /// <summary>
        /// 광고 타겟팅 상세 조회[2slot추가]
        /// 2010.02.08일 타겟팅정보 항목추가(우편번호,유료광고여부,프리퀀시)
        /// 2010.10.20일 SlotExt항목 isnull추가
        /// 2013.04.     셋탑모델 항목 추가
        /// 2013.07.09   선호도조사팝업 항목 추가
        /// 2013.10.16   프로파일 타겟팅 항목 추가
        /// 2014.02.06   고객군 검증함수 추가
        /// </summary>
        public void GetTargetingDetail_10_04(HeaderModel header, TargetingModel targetingModel)
        {
            try
            {
                // 데이터베이스를 OPEN한다
                _db.Open();

                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "GetTargetingDetail() Start");
                _log.Debug("-----------------------------------------");

                // __DEBUG__
                _log.Debug("<입력정보>");
                _log.Debug("ItemNo :[" + targetingModel.ItemNo + "]");		// 광고번호

                // __DEBUG__
                StringBuilder sb = new StringBuilder();

                //TgtCollection
                // 쿼리생성
                sb.Append("\n " + "SELECT   ItemNo ");
                sb.Append("\n " + "      ,  ContractAmt ");
                sb.Append("\n " + "      ,  PriorityValue ");
                sb.Append("\n " + "      ,  PriorityCd ");
                sb.Append("\n " + "      ,  AmtVariableHour ");
                sb.Append("\n " + "      ,  AmtControlYn	,AmtControlRate ");
                sb.Append("\n " + "      ,  TgtRegion1Yn	,TgtRegion1 ");
                sb.Append("\n " + "      ,  TgtTimeYn		,TgtTime ");
                sb.Append("\n " + "      ,  TgtAgeYn		,TgtAge ");
                sb.Append("\n " + "      ,  TgtAgeBtnYn	    ,TgtAgeBtnBegin	,TgtAgeBtnEnd ");
                sb.Append("\n " + "      ,  TgtSexYn		,TgtSexMan		,TgtSexWoman ");
                sb.Append("\n " + "      ,  TgtRateYn		,TgtRate ");
                sb.Append("\n " + "      ,  TgtWeekYn		,TgtWeek ");
                sb.Append("\n " + "      ,  TgtCollectionYn ");

                // 검증방식 변경
                // 기존엔 단순히 건수만 리턴했으나, 상세검증후 코드를 리턴함.
                // 0 : 정상
                // 1 : 경고, 신규에는 미등록되어 있슴
                // 2 : 오류, 양쪽에 등록되어 있지만 포함/제외 코드가 상이
                // 3 : 오류, 신규쪽에 복수개가 등록되어 있슴( 포함/제외 각각 1건씩?)
                sb.Append("\n " + "      ,  dbo.GetCollCheck(ItemNo, TgtCollectionYn,TgtCollection) as TgtCollection ");
                //sb.Append("\n " + "      ,( select count(*) from  TargetingCollection x with(nolock) ");
                //sb.Append("\n " + "      ,  where   x.ItemNo = y.ItemNo  ");
                //sb.Append("\n " + "      ,  and     x.SetType = '1') as TgtCollection ");

                sb.Append("\n " + "      ,  TgtZipYn		,TgtZip ");
                sb.Append("\n " + "      ,  TgtPPxYn ");
                sb.Append("\n " + "      ,  TgtFreqYn		,isnull(TgtFreqDay,0) as TgtFreqDay , isnull(TgtFreqPeriod,0) as TgtFreqPeriod ");
                sb.Append("\n " + "      ,  isnull(TgtPVDBYn,'N') as TgtPVSYn ");
                sb.Append("\n " + "      ,  isnull(SlotExt,0)     as SlotExt ");
                sb.Append("\n " + "      ,  dbo.GetItemAmtTot( ItemNo )   as SumAmt ");
                sb.Append("\n " + "      ,  TgtStbTypeYn    ,TgtStbType ");
                sb.Append("\n " + "      ,  TgtPrefYn       ,TgtPrefRate   ,TgtPrefNosendYn ");
                sb.Append("\n " + "      ,  TgtProfileYn    ,TgtProfile    ,TgtReliability ");
                sb.Append("\n " + " FROM	Targeting y with(nolock) ");
                sb.Append("\n " + " WHERE	ItemNo = " + targetingModel.ItemNo);

                // __DEBUG__
                _log.Debug(sb.ToString());

                // 쿼리실행
                DataSet ds = new DataSet();
                _db.ExecuteQuery(ds, sb.ToString());

                // 결과 DataSet의 장르그룹모델에 복사
                targetingModel.DetailDataSet = ds.Copy();
                targetingModel.ResultCnt = Utility.GetDatasetCount(targetingModel.DetailDataSet);
                targetingModel.ResultCD = "0000";
                ds.Dispose();

                // __DEBUG__
                _log.Debug("<출력정보>");
                _log.Debug("ResultCnt:[" + targetingModel.ResultCnt + "]");
                // __DEBUG__

                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "GetTargetingDetail() End");
                _log.Debug("-----------------------------------------");
            }
            catch (Exception ex)
            {
                targetingModel.ResultCD = "3000";
                targetingModel.ResultDesc = "광고 타겟팅 상세 조회중 오류가 발생하였습니다";
                _log.Exception(ex);
            }
            finally
            {
                _db.Close();
            }
        }
        /// <summary>
        /// 광고 타겟팅 상세 조회[2slot추가]
        /// 2010.02.08일 타겟팅정보 항목추가(우편번호,유료광고여부,프리퀀시)
        /// 2010.10.20일 SlotExt항목 isnull추가
        /// 2013.04.     셋탑모델 항목 추가
        /// 2013.07.09   선호도조사팝업 항목 추가
        /// 2013.10.16   프로파일 타겟팅 항목 추가
        /// 2014.02.06   고객군 검증함수 추가
        /// </summary>
        public void GetTargetingDetail2(HeaderModel header, TargetingModel targetingModel)
        {
            try
            {
                // 데이터베이스를 OPEN한다
                _db.Open();

                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "GetTargetingDetail() Start");
                _log.Debug("-----------------------------------------");

                // __DEBUG__
                _log.Debug("<입력정보>");
                _log.Debug("ItemNo :[" + targetingModel.ItemNo + "]");		// 광고번호

                // __DEBUG__
                StringBuilder sb = new StringBuilder();

                //TgtCollection
                // 쿼리생성
                //sb.Append("\n " + "SELECT   ItemNo ");
                //sb.Append("\n " + "      ,  ContractAmt ");
                //sb.Append("\n " + "      ,  PriorityValue ");
                //sb.Append("\n " + "      ,  PriorityCd ");
                //sb.Append("\n " + "      ,  AmtVariableHour ");
                //sb.Append("\n " + "      ,  AmtControlYn	,AmtControlRate ");
                //sb.Append("\n " + "      ,  TgtRegion1Yn	,TgtRegion1 ");
                //sb.Append("\n " + "      ,  TgtTimeYn		,TgtTime ");
                //sb.Append("\n " + "      ,  TgtAgeYn		,TgtAge ");
                //sb.Append("\n " + "      ,  TgtAgeBtnYn	    ,TgtAgeBtnBegin	,TgtAgeBtnEnd ");
                //sb.Append("\n " + "      ,  TgtSexYn		,TgtSexMan		,TgtSexWoman ");
                //sb.Append("\n " + "      ,  TgtRateYn		,TgtRate ");
                //sb.Append("\n " + "      ,  TgtWeekYn		,TgtWeek ");
                //sb.Append("\n " + "      ,  TgtCollectionYn ");

                //// 검증방식 변경
                //// 기존엔 단순히 건수만 리턴했으나, 상세검증후 코드를 리턴함.
                //// 0 : 정상
                //// 1 : 경고, 신규에는 미등록되어 있슴
                //// 2 : 오류, 양쪽에 등록되어 있지만 포함/제외 코드가 상이
                //// 3 : 오류, 신규쪽에 복수개가 등록되어 있슴( 포함/제외 각각 1건씩?)
                //sb.Append("\n " + "      ,  dbo.GetCollCheck(ItemNo, TgtCollectionYn,TgtCollection) as TgtCollection ");
                ////sb.Append("\n " + "      ,( select count(*) from  TargetingCollection x with(nolock) ");
                ////sb.Append("\n " + "      ,  where   x.ItemNo = y.ItemNo  ");
                ////sb.Append("\n " + "      ,  and     x.SetType = '1') as TgtCollection ");

                //sb.Append("\n " + "      ,  TgtZipYn		,TgtZip ");
                //sb.Append("\n " + "      ,  TgtPPxYn ");
                //sb.Append("\n " + "      ,  TgtFreqYn		,isnull(TgtFreqDay,0) as TgtFreqDay , isnull(TgtFreqPeriod,0) as TgtFreqPeriod ");
                //sb.Append("\n " + "      ,  isnull(TgtPVDBYn,'N') as TgtPVSYn ");
                //sb.Append("\n " + "      ,  isnull(SlotExt,0)     as SlotExt ");
                //sb.Append("\n " + "      ,  dbo.GetItemAmtTot( ItemNo )   as SumAmt ");
                //sb.Append("\n " + "      ,  TgtStbTypeYn    ,TgtStbType ");
                //sb.Append("\n " + "      ,  TgtPrefYn       ,TgtPrefRate   ,TgtPrefNosendYn ");
                //sb.Append("\n " + "      ,  TgtProfileYn    ,TgtProfile    ,TgtReliability ");
                //sb.Append("\n " + "      ,  TgtPocYn    ,TgtPoc ");
                //sb.Append("\n " + " FROM	Targeting y with(nolock) ");
                //sb.Append("\n " + " WHERE	ItemNo = " + targetingModel.ItemNo);

                sb.AppendLine();
                sb.AppendLine(" SELECT	ITEM_NO         AS ItemNo                                                                                               ");
	            sb.AppendLine("     ,	CONTR_QTY       AS ContractAmt                                                                                          ");
	            sb.AppendLine("     ,	0               AS PriorityValue                                                                                        ");
	            sb.AppendLine("     ,	0               AS PriorityCd                                                                                           ");
	            sb.AppendLine("     ,	0               AS AmtVariableHour                                                                                      ");
	            sb.AppendLine("     ,	QTY_YN          AS AmtControlYn     ,QTY_RT             AS AmtControlRate                                               ");
	            sb.AppendLine("     ,	TAR_AREA_YN	    AS TgtRegion1Yn     ,TAR_AREA_VAL       AS TgtRegion1                                                   ");
	            sb.AppendLine("     ,	TAR_HRZONE_YN   AS TgtTimeYn        ,TAR_HRZONE_VAL     AS TgtTime                                                      ");
	            sb.AppendLine("     ,	TAR_AGE_YN      AS TgtAgeYn         ,TAR_AGE_VAL        AS TgtAge                                                       ");
	            sb.AppendLine("     ,	''              AS TgtAgeBtnYn      ,''                 AS TgtAgeBtnBegin   , ''                AS TgtAgeBtnEnd         ");
	            sb.AppendLine("     ,	TAR_SEX_YN      AS TgtSexYn         ,TAR_SEX_MAN_YN     AS TgtSexMan        ,TAR_SEX_WOMAN_YN   AS TgtSexWoman          ");
	            sb.AppendLine("     ,	TAR_RATE_YN     AS TgtRateYn        ,TAR_RATE_VAL       AS TgtRate                                                      ");
	            sb.AppendLine("     ,	TAR_WEEK_YN     AS TgtWeekYn        ,TAR_WEEK_VAL       AS TgtWeek                                                      ");
	            sb.AppendLine("     ,	''              AS TgtCollectionYn                                                                                      ");
	            sb.AppendLine("     ,	''              AS TgtCollection --dbo.GetCollCheck(ItemNo, TgtCollectionYn,TgtCollection) as TgtCollection             ");
	            sb.AppendLine("     ,	''              AS TgtZipYn         , ''                AS TgtZip                                                       ");
	            sb.AppendLine("     ,	TAR_PPX_YN      AS TgtPPxYn                                                                                             ");
	            sb.AppendLine("     ,	TAR_FRQ_YN      AS TgtFreqYn        ,NVL(TAR_FRQ_DY,0)  AS TgtFreqDay       , NVL(TAR_FRQ_PERD,0) AS TgtFreqPeriod      ");
	            sb.AppendLine("     ,	'N'             AS TgtPVSYn --isnull(TgtPVDBYn,'N') as TgtPVSYn                                                         ");
	            sb.AppendLine("     ,	0               AS SlotExt --isnull(SlotExt,0)     as SlotExt                                                           ");
	            sb.AppendLine("     ,	GetItemAmtTot( ITEM_NO ) AS SumAmt                                                                                      ");
	            sb.AppendLine("     ,	TAR_OS_YN       AS TgtStbTypeYn     ,TAR_OS_VAL         AS TgtStbType                                                   ");
	            sb.AppendLine("     ,	''              AS TgtPrefYn        ,0                  AS TgtPrefRate      ,''     AS TgtPrefNosendYn                  ");
	            sb.AppendLine("     ,	''              AS TgtProfileYn     ,''                 AS TgtProfile       ,0      AS TgtReliability                   ");
	            sb.AppendLine("     ,	''              AS TgtPocYn         ,''                 AS TgtPoc                                                       ");
                sb.AppendLine(" FROM	TAR_MST                                                                                                                 ");
                sb.AppendLine(" WHERE	ITEM_NO = " + targetingModel.ItemNo                                                                                      );

                // __DEBUG__
                _log.Debug(sb.ToString());

                // 쿼리실행
                DataSet ds = new DataSet();
                _db.ExecuteQuery(ds, sb.ToString());

                // 결과 DataSet의 장르그룹모델에 복사
                targetingModel.DetailDataSet = ds.Copy();
                targetingModel.ResultCnt = Utility.GetDatasetCount(targetingModel.DetailDataSet);
                targetingModel.ResultCD = "0000";
                ds.Dispose();

                // __DEBUG__
                _log.Debug("<출력정보>");
                _log.Debug("ResultCnt:[" + targetingModel.ResultCnt + "]");
                // __DEBUG__

                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "GetTargetingDetail() End");
                _log.Debug("-----------------------------------------");
            }
            catch (Exception ex)
            {
                targetingModel.ResultCD = "3000";
                targetingModel.ResultDesc = "광고 타겟팅 상세 조회중 오류가 발생하였습니다";
                _log.Exception(ex);
            }
            finally
            {
                _db.Close();
            }

        }
		#endregion

        #region 광고 타겟팅 비율 조회 ------------------------------------------------- 기능 제외 임시 폐기
        /// <summary>
		/// 광고 타겟팅 비율 조회
		/// </summary>
		/// <param name="targetingModel"></param>
		public void GetTargetingRate(HeaderModel header, TargetingModel targetingModel)
		{
			try
			{
				// 데이터베이스를 OPEN한다
				_db.Open();


				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetTargetingRate() Start");
				_log.Debug("-----------------------------------------");

				// __DEBUG__
				_log.Debug("<입력정보>");
				_log.Debug("ItemNo :[" + targetingModel.ItemNo  + "]");		// 광고번호
     

				// __DEBUG__

				StringBuilder sbQuery = new StringBuilder();

				// 쿼리생성
				sbQuery.Append("\n"
					+ "SELECT ItemNo                      \n"
					+ "      ,Type		                  \n"
					+ "      ,Rate1		                  \n"
					+ "      ,Rate2						  \n"
					+ "      ,Rate3						  \n"
					+ "      ,Rate4						  \n"
					+ "      ,Rate5						  \n"
					+ "      ,Rate6						  \n"
					+ "      ,Rate7						  \n"
					+ "      ,Rate8						  \n"
					+ "      ,Rate9						  \n"
					+ "      ,Rate10                      \n"
					+ "      ,Rate11                      \n"
					+ "      ,Rate12					  \n"
					+ "      ,Rate13					  \n"
					+ "      ,Rate14					  \n"
					+ "      ,Rate15                      \n"
					+ "      ,Rate16					  \n"
					+ "      ,Rate17					  \n"
					+ "      ,Rate18					  \n"
					+ "      ,Rate19                      \n"
					+ "      ,Rate20					  \n"
					+ "      ,Rate21                      \n"
					+ "      ,Rate22					  \n"
					+ "      ,Rate23                      \n"
					+ "      ,Rate24                      \n"
					+ "  FROM TargetingRate with(nolock)		\n"	
					+ " WHERE ItemNo = " + targetingModel.ItemNo  + "   \n"
					);

				// __DEBUG__
				_log.Debug(sbQuery.ToString());
				// __DEBUG__
				
				// 쿼리실행
				DataSet ds = new DataSet();
				_db.ExecuteQuery(ds,sbQuery.ToString());

				// 결과 DataSet의 장르그룹모델에 복사
				targetingModel.RateDataSet = ds.Copy();
				// 결과
				targetingModel.ResultCnt = Utility.GetDatasetCount(targetingModel.RateDataSet);
				// 결과코드 셋트
				targetingModel.ResultCD = "0000";

				ds.Dispose();

				// __DEBUG__
				_log.Debug("<출력정보>");
				_log.Debug("ResultCnt:[" + targetingModel.ResultCnt + "]");
				// __DEBUG__

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetTargetingRate() End");
				_log.Debug("-----------------------------------------");
			}
			catch(Exception ex)
			{
				targetingModel.ResultCD = "3000";
				targetingModel.ResultDesc = "광고 타겟팅 비율 조회중 오류가 발생하였습니다";
				_log.Exception(ex);
			}
			finally
			{
				// 데이터베이스를  Close한다
				_db.Close();
			}
		}
		#endregion

		#region 광고 타겟팅 저장

        /// <summary>
        /// 광고 타겟팅 상세 저장
        /// </summary>
        /// <returns></returns>
        public void SetTargetingDetailUpdate(HeaderModel header, TargetingModel targetingModel)
        {

            try
            {
                // 데이터베이스를 OPEN한다
                _db.Open();

                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + ".SetTargetingDetailUpdate() Start");
                _log.Debug("-----------------------------------------");

				_log.Debug("ItemNo         :[" + targetingModel.ItemNo         + "]");		
				_log.Debug("ItemName       :[" + targetingModel.ItemName       + "]");		
				_log.Debug("ContractAmt    :[" + targetingModel.ContractAmt    + "]");		
				_log.Debug("PriorityCd     :[" + targetingModel.PriorityCd     + "]");		
				_log.Debug("AmtControlYn   :[" + targetingModel.AmtControlYn   + "]");		
				_log.Debug("AmtControlRate :[" + targetingModel.AmtControlRate + "]");		
				_log.Debug("TgtRegion1Yn   :[" + targetingModel.TgtRegion1Yn   + "]");		
				_log.Debug("TgtRegion1     :[" + targetingModel.TgtRegion1     + "]");		
				_log.Debug("TgtTimeYn      :[" + targetingModel.TgtTimeYn      + "]");		
				_log.Debug("TgtTime        :[" + targetingModel.TgtTime        + "]");		
				_log.Debug("TgtAgeYn       :[" + targetingModel.TgtAgeYn       + "]");		
				_log.Debug("TgtAge         :[" + targetingModel.TgtAge         + "]");		
				_log.Debug("TgtAgeBtnYn    :[" + targetingModel.TgtAgeBtnYn    + "]");		
				_log.Debug("TgtAgeBtnBegin :[" + targetingModel.TgtAgeBtnBegin + "]");		
				_log.Debug("TgtAgeBtnEnd   :[" + targetingModel.TgtAgeBtnEnd   + "]");		
				_log.Debug("TgtSexYn       :[" + targetingModel.TgtSexYn       + "]");		
				_log.Debug("TgtSexMan      :[" + targetingModel.TgtSexMan      + "]");		
				_log.Debug("TgtSexWoman    :[" + targetingModel.TgtSexWoman    + "]");		
				_log.Debug("TgtRateYn      :[" + targetingModel.TgtRateYn      + "]");		
				_log.Debug("TgtRate        :[" + targetingModel.TgtRate        + "]");		
				_log.Debug("TgtWeekYn      :[" + targetingModel.TgtWeekYn      + "]");		
				_log.Debug("TgtWeek        :[" + targetingModel.TgtWeek        + "]");		
				_log.Debug("TgtCollectionYn      :[" + targetingModel.TgtCollectionYn      + "]");		
//2012.02.21	_log.Debug("TgtCollection        :[" + targetingModel.TgtCollection        + "]");

                int i = 0;
                int rc = 0;
                StringBuilder  sbQuery;
                SqlParameter[] sqlParams = new SqlParameter[29];
	            
                i = 0;
				sqlParams[i++] = new SqlParameter("@ItemNo"          , SqlDbType.Int		,4	);
				sqlParams[i++] = new SqlParameter("@ContractAmt"     , SqlDbType.Decimal    );
                sqlParams[i++] = new SqlParameter("@PriorityCd"      , SqlDbType.TinyInt    );
				sqlParams[i++] = new SqlParameter("@AmtControlYn"    , SqlDbType.Char     ,1);
				sqlParams[i++] = new SqlParameter("@AmtControlRate"  , SqlDbType.SmallInt   );
				sqlParams[i++] = new SqlParameter("@TgtRegion1Yn"    , SqlDbType.Char     ,1);
				//sqlParams[i++] = new SqlParameter("@TgtRegion1"      , SqlDbType.VarChar,512);
				sqlParams[i++] = new SqlParameter("@TgtRegion1"      , SqlDbType.VarChar,2000); // [E_01] 지역정보확대로 인한 Length 증가
				sqlParams[i++] = new SqlParameter("@TgtTimeYn"       , SqlDbType.Char     ,1);
				//sqlParams[i++] = new SqlParameter("@TgtTime"         , SqlDbType.VarChar,128);
				sqlParams[i++] = new SqlParameter("@TgtTime"         , SqlDbType.VarChar,148);  // [E_02] 시간 타겟팅 변경으로 인한 Length증가
				sqlParams[i++] = new SqlParameter("@TgtAgeYn"        , SqlDbType.Char     ,1);
				sqlParams[i++] = new SqlParameter("@TgtAge"          , SqlDbType.VarChar,128);
				sqlParams[i++] = new SqlParameter("@TgtAgeBtnYn"     , SqlDbType.Char     ,1);
				sqlParams[i++] = new SqlParameter("@TgtAgeBtnBegin"  , SqlDbType.TinyInt    );
				sqlParams[i++] = new SqlParameter("@TgtAgeBtnEnd"    , SqlDbType.TinyInt    );
				sqlParams[i++] = new SqlParameter("@TgtSexYn"        , SqlDbType.Char     ,1);
				sqlParams[i++] = new SqlParameter("@TgtSexMan"       , SqlDbType.Char     ,1);
				sqlParams[i++] = new SqlParameter("@TgtSexWoman"     , SqlDbType.Char     ,1);
				sqlParams[i++] = new SqlParameter("@TgtRateYn"       , SqlDbType.Char     ,1);
				sqlParams[i++] = new SqlParameter("@TgtRate"         , SqlDbType.SmallInt   );
				sqlParams[i++] = new SqlParameter("@TgtWeekYn"       , SqlDbType.Char     ,1);
				sqlParams[i++] = new SqlParameter("@TgtWeek"         , SqlDbType.VarChar ,13);
				sqlParams[i++] = new SqlParameter("@TgtCollectionYn" , SqlDbType.Char     ,1);
//2012.02.21    sqlParams[i++] = new SqlParameter("@TgtCollection"   , SqlDbType.Int		);
				
				// S1추가
				sqlParams[i++] = new SqlParameter("@TgtZipYn"		,	SqlDbType.Char		,	1);
				sqlParams[i++] = new SqlParameter("@TgtZip"			,	SqlDbType.VarChar	,	800); //[E_01] 우편번호 멀티 입력으로 Length 50->800으로 증가
				sqlParams[i++] = new SqlParameter("@TgtPPxYn"		,	SqlDbType.Char		,	1);
				sqlParams[i++] = new SqlParameter("@TgtFreqYn"		,	SqlDbType.Char		,	1);
				sqlParams[i++] = new SqlParameter("@TgtFreqDay"		,	SqlDbType.Int);
				sqlParams[i++] = new SqlParameter("@TgtFreqPeriod"	,	SqlDbType.Int);
				sqlParams[i++] = new SqlParameter("@TgtPVSYn"		,	SqlDbType.Char		,	1);

                i = 0;
				sqlParams[i++].Value = Convert.ToInt32(targetingModel.ItemNo);        
				sqlParams[i++].Value = Convert.ToDecimal(targetingModel.ContractAmt);   
                sqlParams[i++].Value = Convert.ToInt16(targetingModel.PriorityCd);    
				sqlParams[i++].Value = targetingModel.AmtControlYn;    
				sqlParams[i++].Value = Convert.ToInt16(targetingModel.AmtControlRate);    
				sqlParams[i++].Value = targetingModel.TgtRegion1Yn;    
				sqlParams[i++].Value = targetingModel.TgtRegion1;    
				sqlParams[i++].Value = targetingModel.TgtTimeYn;    
				sqlParams[i++].Value = targetingModel.TgtTime;    
				sqlParams[i++].Value = targetingModel.TgtAgeYn;    
				sqlParams[i++].Value = targetingModel.TgtAge;    
				sqlParams[i++].Value = targetingModel.TgtAgeBtnYn;    
				sqlParams[i++].Value = Convert.ToInt16(targetingModel.TgtAgeBtnBegin);    
				sqlParams[i++].Value = Convert.ToInt16(targetingModel.TgtAgeBtnEnd);    
				sqlParams[i++].Value = targetingModel.TgtSexYn;    
				sqlParams[i++].Value = targetingModel.TgtSexMan;    
				sqlParams[i++].Value = targetingModel.TgtSexWoman;    
				sqlParams[i++].Value = targetingModel.TgtRateYn;    
				sqlParams[i++].Value = Convert.ToInt16(targetingModel.TgtRate);    
				sqlParams[i++].Value = targetingModel.TgtWeekYn;    
				sqlParams[i++].Value = targetingModel.TgtWeek;    
				sqlParams[i++].Value = targetingModel.TgtCollectionYn;    
				sqlParams[i++].Value = Convert.ToInt32(targetingModel.TgtCollection);  
				
				//S1추가
				sqlParams[i++].Value = targetingModel.TgtZipYn;
				sqlParams[i++].Value = targetingModel.TgtZip;
				sqlParams[i++].Value = targetingModel.TgtPPxYn;
				sqlParams[i++].Value = targetingModel.TgtFreqYn;
				sqlParams[i++].Value = Convert.ToInt32(targetingModel.TgtFreqDay);  
				sqlParams[i++].Value = Convert.ToInt32(targetingModel.TgtFreqFeriod);  
				sqlParams[i++].Value = targetingModel.TgtPVSYn;
    
                // 쿼리실행
                try
                {
                    _db.BeginTran();

					sbQuery = new StringBuilder();
					sbQuery.Append(" UPDATE Targeting							\n");
					sbQuery.Append("    SET ContractAmt		= @ContractAmt		\n");
					sbQuery.Append("       ,PriorityCd		= @PriorityCd		\n");
					sbQuery.Append("       ,AmtControlYn  	= @AmtControlYn		\n");
					sbQuery.Append("       ,AmtControlRate	= @AmtControlRate	\n");
					sbQuery.Append("       ,TgtRegion1Yn  	= @TgtRegion1Yn		\n");
					sbQuery.Append("       ,TgtRegion1    	= @TgtRegion1		\n");
					sbQuery.Append("       ,TgtTimeYn     	= @TgtTimeYn		\n");
					sbQuery.Append("       ,TgtTime       	= @TgtTime			\n");
					sbQuery.Append("       ,TgtAgeYn      	= @TgtAgeYn			\n");
					sbQuery.Append("       ,TgtAge        	= @TgtAge			\n");
					sbQuery.Append("       ,TgtAgeBtnYn   	= @TgtAgeBtnYn		\n");
					sbQuery.Append("       ,TgtAgeBtnBegin	= @TgtAgeBtnBegin	\n");
					sbQuery.Append("       ,TgtAgeBtnEnd  	= @TgtAgeBtnEnd		\n");
					sbQuery.Append("       ,TgtSexYn      	= @TgtSexYn			\n");
					sbQuery.Append("       ,TgtSexMan     	= @TgtSexMan		\n");
					sbQuery.Append("       ,TgtSexWoman   	= @TgtSexWoman		\n");
					sbQuery.Append("       ,TgtRateYn     	= @TgtRateYn		\n");
					sbQuery.Append("       ,TgtRate       	= @TgtRate			\n");
					sbQuery.Append("       ,TgtWeekYn     	= @TgtWeekYn		\n");
					sbQuery.Append("       ,TgtWeek       	= @TgtWeek			\n");
					sbQuery.Append("       ,TgtCollectionYn	= @TgtCollectionYn  \n");
//2012.02.21		sbQuery.Append("       ,TgtCollection   = @TgtCollection	\n");

					sbQuery.Append("       ,TgtZipYn		= @TgtZipYn			\n");
					sbQuery.Append("       ,TgtZip			= @TgtZip			\n");
					sbQuery.Append("       ,TgtPPxYn		= @TgtPPxYn			\n");
					sbQuery.Append("       ,TgtFreqYn		= @TgtFreqYn		\n");
					sbQuery.Append("       ,TgtFreqDay		= @TgtFreqDay		\n");
					sbQuery.Append("       ,TgtFreqPeriod	= @TgtFreqPeriod	\n");
					sbQuery.Append("       ,TgtPVDBYn		= @TgtPVSYn			\n");

					sbQuery.Append("  WHERE ItemNo          = @ItemNo		  \n");

                    rc =  _db.ExecuteNonQueryParams(sbQuery.ToString(), sqlParams);

					if(rc < 1)
					{
						// 지정채널 편성 테이블에 추가
						sbQuery = new StringBuilder();
						sbQuery.Append("\n");
						sbQuery.Append(" INSERT INTO Targeting		  \n");
						sbQuery.Append("            (ItemNo			  \n");
						sbQuery.Append("            ,ContractAmt	  \n");
						sbQuery.Append("            ,PriorityAmt	  \n");
						sbQuery.Append("            ,PriorityValue	  \n");
						sbQuery.Append("            ,PriorityCd		  \n");
						sbQuery.Append("            ,AmtControlYn    \n");
						sbQuery.Append("            ,AmtControlRate  \n");
						sbQuery.Append("            ,TgtRegion1Yn    \n");
						sbQuery.Append("            ,TgtRegion1      \n");
						sbQuery.Append("            ,TgtTimeYn       \n");
						sbQuery.Append("            ,TgtTime         \n");
						sbQuery.Append("            ,TgtAgeYn        \n");
						sbQuery.Append("            ,TgtAge          \n");
						sbQuery.Append("            ,TgtAgeBtnYn     \n");
						sbQuery.Append("            ,TgtAgeBtnBegin  \n");
						sbQuery.Append("            ,TgtAgeBtnEnd    \n");
						sbQuery.Append("            ,TgtSexYn        \n");
						sbQuery.Append("            ,TgtSexMan       \n");
						sbQuery.Append("            ,TgtSexWoman     \n");
						sbQuery.Append("            ,TgtRateYn       \n");
						sbQuery.Append("            ,TgtRate         \n");
						sbQuery.Append("            ,TgtWeekYn       \n");
						sbQuery.Append("            ,TgtWeek         \n");
						sbQuery.Append("            ,TgtCollectionYn       \n");
//2012.02.21    		sbQuery.Append("            ,TgtCollection         \n");

						sbQuery.Append("			,TgtZipYn		\n");
						sbQuery.Append("			,TgtZip			\n");
						sbQuery.Append("			,TgtPPxYn		\n");
						sbQuery.Append("			,TgtFreqYn		\n");
						sbQuery.Append("			,TgtFreqDay		\n");
						sbQuery.Append("			,TgtFreqPeriod	\n");
						sbQuery.Append("			,TgtPVDBYn		\n");

						sbQuery.Append("            )				  \n");
						sbQuery.Append("    VALUES	(@ItemNo		  \n");
						sbQuery.Append("            ,@ContractAmt	  \n");
						sbQuery.Append("            ,10000000		  \n");		// 기본값 설정
						sbQuery.Append("            ,0           	  \n");
						sbQuery.Append("            ,@PriorityCd	  \n");
						sbQuery.Append("            ,@AmtControlYn    \n");
						sbQuery.Append("            ,@AmtControlRate  \n");
						sbQuery.Append("            ,@TgtRegion1Yn    \n");
						sbQuery.Append("            ,@TgtRegion1      \n");
						sbQuery.Append("            ,@TgtTimeYn       \n");
						sbQuery.Append("            ,@TgtTime         \n");
						sbQuery.Append("            ,@TgtAgeYn        \n");
						sbQuery.Append("            ,@TgtAge          \n");
						sbQuery.Append("            ,@TgtAgeBtnYn     \n");
						sbQuery.Append("            ,@TgtAgeBtnBegin  \n");
						sbQuery.Append("            ,@TgtAgeBtnEnd    \n");
						sbQuery.Append("            ,@TgtSexYn        \n");
						sbQuery.Append("            ,@TgtSexMan       \n");
						sbQuery.Append("            ,@TgtSexWoman     \n");
						sbQuery.Append("            ,@TgtRateYn       \n");
						sbQuery.Append("            ,@TgtRate         \n");
						sbQuery.Append("            ,@TgtWeekYn       \n");
						sbQuery.Append("            ,@TgtWeek			\n");
						sbQuery.Append("            ,@TgtCollectionYn	\n");
//2012.02.21			sbQuery.Append("            ,@TgtCollection		\n");
						sbQuery.Append("			,@TgtZipYn		\n");
						sbQuery.Append("			,@TgtZip			\n");
						sbQuery.Append("			,@TgtPPxYn		\n");
						sbQuery.Append("			,@TgtFreqYn		\n");
						sbQuery.Append("			,@TgtFreqDay		\n");
						sbQuery.Append("			,@TgtFreqPeriod	\n");
						sbQuery.Append("			,@TgtPVSYn	\n");
						sbQuery.Append(" 			)					\n");

						rc =  _db.ExecuteNonQueryParams(sbQuery.ToString(), sqlParams);

					}

					// 2007.10.25 RH.Jung 노출값재설정을 이제는 안해도 됨. 서버에서 실시간 처리함
					//SetPriorityValues(targetingModel.ItemNo);

                    _db.CommitTran();
            
                    // __MESSAGE__
                    _log.Message("광고 타겟팅 수정:["+targetingModel.ItemNo +"] " + targetingModel.ItemName + " 등록자:[" + header.UserID + "]");
            
                }
                catch(Exception ex)
                {
                    _db.RollbackTran();
                    throw ex;
                }
            
                targetingModel.ResultCD = "0000";  // 정상
            
                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "SetTargetingDetailUpdate() End");
                _log.Debug("-----------------------------------------");	
            }
            catch(Exception ex)
            {
                targetingModel.ResultCD   = "3101";
                targetingModel.ResultDesc = "광고 타겟팅 수정 중 오류가 발생하였습니다";
                _log.Exception(ex);
            }
            finally
            {
                // 데이터베이스를  Close한다
                _db.Close();
            }

        }


		/// <summary>
		/// 광고 타겟팅 상세 저장(slot 기능추가)[E_03]
		/// </summary>		
		public void SetTargetingDetailUpdate_10_04(HeaderModel header, TargetingModel targetingModel)
		{
			try
			{
				// 데이터베이스를 OPEN한다
				_db.Open();

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + ".SetTargetingDetailUpdate() Start");
				_log.Debug("-----------------------------------------");

				_log.Debug("ItemNo         :[" + targetingModel.ItemNo         + "]");		
				_log.Debug("ItemName       :[" + targetingModel.ItemName       + "]");		
				_log.Debug("ContractAmt    :[" + targetingModel.ContractAmt    + "]");		
				//_log.Debug("PriorityCd     :[" + targetingModel.PriorityCd     + "]");		
				_log.Debug("AmtControlYn   :[" + targetingModel.AmtControlYn   + "]");		
				_log.Debug("AmtControlRate :[" + targetingModel.AmtControlRate + "]");		
				_log.Debug("TgtRegion1Yn   :[" + targetingModel.TgtRegion1Yn   + "]");		
				_log.Debug("TgtRegion1     :[" + targetingModel.TgtRegion1     + "]");		
				_log.Debug("TgtTimeYn      :[" + targetingModel.TgtTimeYn      + "]");		
				_log.Debug("TgtTime        :[" + targetingModel.TgtTime        + "]");		
				_log.Debug("TgtAgeYn       :[" + targetingModel.TgtAgeYn       + "]");		
				_log.Debug("TgtAge         :[" + targetingModel.TgtAge         + "]");		
				//_log.Debug("TgtAgeBtnYn    :[" + targetingModel.TgtAgeBtnYn    + "]");		
				//_log.Debug("TgtAgeBtnBegin :[" + targetingModel.TgtAgeBtnBegin + "]");		
				//_log.Debug("TgtAgeBtnEnd   :[" + targetingModel.TgtAgeBtnEnd   + "]");		
				_log.Debug("TgtSexYn       :[" + targetingModel.TgtSexYn       + "]");		
				_log.Debug("TgtSexMan      :[" + targetingModel.TgtSexMan      + "]");		
				_log.Debug("TgtSexWoman    :[" + targetingModel.TgtSexWoman    + "]");		
				_log.Debug("TgtRateYn      :[" + targetingModel.TgtRateYn      + "]");		
				_log.Debug("TgtRate        :[" + targetingModel.TgtRate        + "]");		
				_log.Debug("TgtWeekYn      :[" + targetingModel.TgtWeekYn      + "]");		
				_log.Debug("TgtWeek        :[" + targetingModel.TgtWeek        + "]");		
				//_log.Debug("TgtCollectionYn      :[" + targetingModel.TgtCollectionYn      + "]");		
                //_log.Debug("TgtCollection        :[" + targetingModel.TgtCollection        + "]");
                // 임시 제외 _log.Debug("SlotExt        :[" + targetingModel.SlotExt.ToString()   + "]");
                _log.Debug("TgtStbTypeYn   :[" + targetingModel.TgtStbModelYn  + "]");
                _log.Debug("TgtStbType     :[" + targetingModel.TgtStbModel    + "]");
                //_log.Debug("TgtPrefYn      :[" + targetingModel.TgtPrefYn      + "]");
                //_log.Debug("TgtPrefRate    :[" + targetingModel.TgtPrefRate    + "]");
                //_log.Debug("TgtPrefNosend  :[" + targetingModel.TgtPrefNosend  + "]");
                //_log.Debug("TgtProfileYn   :[" + targetingModel.TgtProfileYn   + "]");
                //_log.Debug("TgtProfile     :[" + targetingModel.TgtProfile     + "]");
                //_log.Debug("TgtReliability :[" + targetingModel.TgtReliablilty + "]");
                //_log.Debug("TgtPocYn       :[" + targetingModel.TgtPocYn       + "]");
                //_log.Debug("TgtPoc         :[" + targetingModel.TgtPoc         + "]");

				int i = 0;
				int rc = 0;
				StringBuilder  sbQuery;
                OracleParameter[] sqlParams = new OracleParameter[24];
	            
				i = 0;
				sqlParams[i++] = new OracleParameter(":ContractAmt"     , OracleDbType.Int32);
				//sqlParams[i++] = new OracleParameter(":PriorityCd"      , OracleDbType.Int32);
				sqlParams[i++] = new OracleParameter(":AmtControlYn"    , OracleDbType.Char     , 1);
				sqlParams[i++] = new OracleParameter(":AmtControlRate"  , OracleDbType.Int32);
				sqlParams[i++] = new OracleParameter(":TgtRegion1Yn"    , OracleDbType.Char     , 1);				
				sqlParams[i++] = new OracleParameter(":TgtRegion1"      , OracleDbType.Varchar2 , 2000); // [E_01] 지역정보확대로 인한 Length 증가(기존512)
				sqlParams[i++] = new OracleParameter(":TgtTimeYn"       , OracleDbType.Char     , 1);				
				sqlParams[i++] = new OracleParameter(":TgtTime"         , OracleDbType.Varchar2 , 200);  // [E_02] 시간 타겟팅 변경으로 인한 Length증가(기존128)
				sqlParams[i++] = new OracleParameter(":TgtAgeYn"        , OracleDbType.Char     , 1);
				sqlParams[i++] = new OracleParameter(":TgtAge"          , OracleDbType.Varchar2 , 200);
				//sqlParams[i++] = new OracleParameter(":TgtAgeBtnYn"     , OracleDbType.Char     ,1);
				//sqlParams[i++] = new OracleParameter(":TgtAgeBtnBegin"  , OracleDbType.TinyInt    );
				//sqlParams[i++] = new OracleParameter(":TgtAgeBtnEnd"    , OracleDbType.TinyInt    );
				sqlParams[i++] = new OracleParameter(":TgtSexYn"        , OracleDbType.Char     , 1);
				sqlParams[i++] = new OracleParameter(":TgtSexMan"       , OracleDbType.Char     , 1);
				sqlParams[i++] = new OracleParameter(":TgtSexWoman"     , OracleDbType.Char     , 1);
				sqlParams[i++] = new OracleParameter(":TgtRateYn"       , OracleDbType.Char     , 1);
				sqlParams[i++] = new OracleParameter(":TgtRate"         , OracleDbType.Int32);
				sqlParams[i++] = new OracleParameter(":TgtWeekYn"       , OracleDbType.Char     , 1);
				sqlParams[i++] = new OracleParameter(":TgtWeek"         , OracleDbType.Varchar2 , 20);
				//sqlParams[i++] = new OracleParameter(":TgtCollectionYn" , OracleDbType.Char     ,1);
                //sqlParams[i++] = new SqlParameter("@TgtCollection"   , SqlDbType.Int		);
				
				// S1추가
				//sqlParams[i++] = new OracleParameter(":TgtZipYn"		, OracleDbType.Char		,	1);
				//sqlParams[i++] = new OracleParameter(":TgtZip"			, OracleDbType.VarChar	,	800); //[E_01] 우편번호 멀티 입력으로 Length 50->800으로 증가
				sqlParams[i++] = new OracleParameter(":TgtPPxYn"		, OracleDbType.Char     , 1);
				sqlParams[i++] = new OracleParameter(":TgtFreqYn"		, OracleDbType.Char		, 1);
				sqlParams[i++] = new OracleParameter(":TgtFreqDay"		, OracleDbType.Int32);
				sqlParams[i++] = new OracleParameter(":TgtFreqPeriod"	, OracleDbType.Int32);
				//sqlParams[i++] = new OracleParameter(":TgtPVSYn"		, OracleDbType.Char		,	1);
                sqlParams[i++] = new OracleParameter(":IdUpdate", OracleDbType.Varchar2, 10);                                      
				// SlotExt 추가[E_04]Oracle           
				// 임시 제외 sqlParams[i++] = new OracleParameter(":SlotExt"	        , OracleDbType.Int);
                sqlParams[i++] = new OracleParameter(":TgtStbTypeYn"    , OracleDbType.Char     , 1);     // [E_05] 셋탑모델 사용여부
                sqlParams[i++] = new OracleParameter(":TgtStbType"      , OracleDbType.Varchar2 , 20);   // [E_05] 셋탑모델별
                                                         
                //sqlParams[i++] = new OracleParameter(":TgtPrefYn"       , OracleDbType.Char, 1); // [E_06] 선호도조사팝업
                //sqlParams[i++] = new OracleParameter(":TgtPrefRate"     , OracleDbType.Int);     // [E_06] 선호도조사팝업
                //sqlParams[i++] = new OracleParameter(":TgtPrefNosendYn" , OracleDbType.Char, 1); // [E_06] 선호도조사팝업
                                                                          
                //sqlParams[i++] = new OracleParameter(":TgtProfileYn"    , OracleDbType.Char, 1);  // [E_07] 프로파일 타겟팅 사용여부
                //sqlParams[i++] = new OracleParameter(":TgtProfile"      , OracleDbType.VarChar, 51);   // [E_07] 프로파일 타겟팅 연령
                //sqlParams[i++] = new OracleParameter(":TgtReliability"  , OracleDbType.Int);       // [E_07] 프로파일 타겟팅 신뢰도
                //sqlParams[i++] = new OracleParameter(":TgtPocYn"        , SqlDbType.Char, 1);         
                //sqlParams[i++] = new OracleParameter(":TgtPoc"          , SqlDbType.VarChar, 512);      
                sqlParams[i++] = new OracleParameter(":ItemNo", OracleDbType.Int32);

				i = 0;     
				sqlParams[i++].Value = Convert.ToInt32(targetingModel.ContractAmt);   
				//sqlParams[i++].Value = Convert.ToInt16(targetingModel.PriorityCd);    
				sqlParams[i++].Value = targetingModel.AmtControlYn;    
				sqlParams[i++].Value = Convert.ToInt32(targetingModel.AmtControlRate);    
				sqlParams[i++].Value = targetingModel.TgtRegion1Yn;    
				sqlParams[i++].Value = targetingModel.TgtRegion1;    
				sqlParams[i++].Value = targetingModel.TgtTimeYn;    
				sqlParams[i++].Value = targetingModel.TgtTime;    
				sqlParams[i++].Value = targetingModel.TgtAgeYn;    
				sqlParams[i++].Value = targetingModel.TgtAge;    
				//sqlParams[i++].Value = targetingModel.TgtAgeBtnYn;    
				//sqlParams[i++].Value = Convert.ToInt16(targetingModel.TgtAgeBtnBegin);    
				//sqlParams[i++].Value = Convert.ToInt16(targetingModel.TgtAgeBtnEnd);    
				sqlParams[i++].Value = targetingModel.TgtSexYn;    
				sqlParams[i++].Value = targetingModel.TgtSexMan;    
				sqlParams[i++].Value = targetingModel.TgtSexWoman;    
				sqlParams[i++].Value = targetingModel.TgtRateYn;
                sqlParams[i++].Value = Convert.ToInt32(targetingModel.TgtRate);    
				sqlParams[i++].Value = targetingModel.TgtWeekYn;    
				sqlParams[i++].Value = targetingModel.TgtWeek;    
				//sqlParams[i++].Value = targetingModel.TgtCollectionYn;    
                //sqlParams[i++].Value = Convert.ToInt32(targetingModel.TgtCollection);  
				
				//S1추가
				//sqlParams[i++].Value = targetingModel.TgtZipYn;
				//sqlParams[i++].Value = targetingModel.TgtZip;
				sqlParams[i++].Value = targetingModel.TgtPPxYn;
				sqlParams[i++].Value = targetingModel.TgtFreqYn;
				sqlParams[i++].Value = Convert.ToInt32(targetingModel.TgtFreqDay);  
				sqlParams[i++].Value = Convert.ToInt32(targetingModel.TgtFreqFeriod);
                sqlParams[i++].Value = header.UserID;  
				//sqlParams[i++].Value = targetingModel.TgtPVSYn;
                // 임시 제외 sqlParams[i++].Value = targetingModel.SlotExt;
                sqlParams[i++].Value = targetingModel.TgtStbModelYn;    // [E_05]
                sqlParams[i++].Value = targetingModel.TgtStbModel;      // [E_05]
                //sqlParams[i++].Value = targetingModel.TgtPrefYn;        // [E_06] 선호도조사팝업
                //sqlParams[i++].Value = targetingModel.TgtPrefRate;      // [E_06] 선호도조사팝업
                //sqlParams[i++].Value = targetingModel.TgtPrefNosend;    // [E_06] 선호도조사팝업
                //sqlParams[i++].Value = targetingModel.TgtProfileYn;     // [E_07] 프로파일 타겟팅 사용여부
                //sqlParams[i++].Value = targetingModel.TgtProfile;       // [E_07] 프로파일 타겟팅 연령
                //sqlParams[i++].Value = targetingModel.TgtReliablilty;   // [E_07] 프로파일 타겟팅 신뢰도
                //sqlParams[i++].Value = targetingModel.TgtPocYn;       
                //sqlParams[i++].Value = targetingModel.TgtPoc;   
                sqlParams[i++].Value = Convert.ToInt32(targetingModel.ItemNo);

                i = 0;
                sqlParams[i++].Direction = ParameterDirection.Input;
                sqlParams[i++].Direction = ParameterDirection.Input;
                sqlParams[i++].Direction = ParameterDirection.Input;
                sqlParams[i++].Direction = ParameterDirection.Input;
                sqlParams[i++].Direction = ParameterDirection.Input;
                sqlParams[i++].Direction = ParameterDirection.Input;
                sqlParams[i++].Direction = ParameterDirection.Input;
                sqlParams[i++].Direction = ParameterDirection.Input;
                sqlParams[i++].Direction = ParameterDirection.Input;
                sqlParams[i++].Direction = ParameterDirection.Input;
                sqlParams[i++].Direction = ParameterDirection.Input;
                sqlParams[i++].Direction = ParameterDirection.Input;
                sqlParams[i++].Direction = ParameterDirection.Input;
                sqlParams[i++].Direction = ParameterDirection.Input;
                sqlParams[i++].Direction = ParameterDirection.Input;
                sqlParams[i++].Direction = ParameterDirection.Input;
                sqlParams[i++].Direction = ParameterDirection.Input;
                sqlParams[i++].Direction = ParameterDirection.Input;
                sqlParams[i++].Direction = ParameterDirection.Input;
                sqlParams[i++].Direction = ParameterDirection.Input;
                sqlParams[i++].Direction = ParameterDirection.Input;
                sqlParams[i++].Direction = ParameterDirection.Input;
                sqlParams[i++].Direction = ParameterDirection.Input;
                sqlParams[i++].Direction = ParameterDirection.Input;

				// 쿼리실행
				try
				{
					_db.BeginTran();

					sbQuery = new StringBuilder();
                    //sbQuery.AppendLine(" UPDATE Targeting							");
                    //sbQuery.AppendLine("    SET ContractAmt		= :ContractAmt		");
                    ////sbQuery.AppendLine("       ,PriorityCd		= :PriorityCd		");
                    //sbQuery.AppendLine("       ,AmtControlYn  	= :AmtControlYn		");
                    //sbQuery.AppendLine("       ,AmtControlRate	= :AmtControlRate	");
                    //sbQuery.AppendLine("       ,TgtRegion1Yn  	= :TgtRegion1Yn		");
                    //sbQuery.AppendLine("       ,TgtRegion1    	= :TgtRegion1		");
                    //sbQuery.AppendLine("       ,TgtTimeYn     	= :TgtTimeYn		");
                    //sbQuery.AppendLine("       ,TgtTime       	= :TgtTime			");
                    //sbQuery.AppendLine("       ,TgtAgeYn      	= :TgtAgeYn			");
                    //sbQuery.AppendLine("       ,TgtAge        	= :TgtAge			");
                    ////sbQuery.AppendLine("       ,TgtAgeBtnYn   	= :TgtAgeBtnYn		");
                    ////sbQuery.AppendLine("       ,TgtAgeBtnBegin	= :TgtAgeBtnBegin	");
                    ////sbQuery.AppendLine("       ,TgtAgeBtnEnd  	= :TgtAgeBtnEnd		");
                    //sbQuery.AppendLine("       ,TgtSexYn      	= :TgtSexYn			");
                    //sbQuery.AppendLine("       ,TgtSexMan     	= :TgtSexMan		");
                    //sbQuery.AppendLine("       ,TgtSexWoman   	= :TgtSexWoman		");
                    //sbQuery.AppendLine("       ,TgtRateYn     	= :TgtRateYn		");
                    //sbQuery.AppendLine("       ,TgtRate       	= :TgtRate			");
                    //sbQuery.AppendLine("       ,TgtWeekYn     	= :TgtWeekYn		");
                    //sbQuery.AppendLine("       ,TgtWeek       	= :TgtWeek			");
                    ////sbQuery.AppendLine("       ,TgtCollectionYn	= :TgtCollectionYn  ");
                    ////sbQuery.AppendLine("       ,TgtZipYn		= :TgtZipYn			");
                    ////sbQuery.AppendLine("       ,TgtZip			= :TgtZip			");
                    //sbQuery.AppendLine("       ,TgtPPxYn		= :TgtPPxYn			");
                    //sbQuery.AppendLine("       ,TgtFreqYn		= :TgtFreqYn		");
                    //sbQuery.AppendLine("       ,TgtFreqDay		= :TgtFreqDay		");
                    //sbQuery.AppendLine("       ,TgtFreqPeriod	= :TgtFreqPeriod	");
                    ////sbQuery.AppendLine("       ,TgtPVDBYn		= :TgtPVSYn			");
                    //// 임시 제외 sbQuery.AppendLine("       ,SlotExt			= :SlotExt			");
                    //sbQuery.AppendLine("       ,TgtStbTypeYn	= :TgtStbTypeYn		");   // [E_05]
                    //sbQuery.AppendLine("       ,TgtStbType		= :TgtStbType		");   // [E_05]
                    ////sbQuery.AppendLine("       ,TgtPrefYn		= :TgtPrefYn		");   // [E_06] 선호도조사팝업
                    ////sbQuery.AppendLine("       ,TgtPrefRate		= :TgtPrefRate		");   // [E_06] 선호도조사팝업
                    ////sbQuery.AppendLine("       ,TgtPrefNosendYn	= :TgtPrefNosendYn	");   // [E_06] 선호도조사팝업
                    ////sbQuery.AppendLine("       ,TgtProfileYn    = :TgtProfileYn     ");   // [E_07] 프로파일 타겟팅 사용유무
                    ////sbQuery.AppendLine("       ,TgtProfile      = :TgtProfile       ");   // [E_07] 프로파일 타겟팅 연령대
                    ////sbQuery.AppendLine("       ,TgtReliability  = :TgtReliability   ");   // [E_07] 프로파일 타겟팅 신뢰도
                    ////sbQuery.AppendLine("       ,TgtPocYn		= :TgtPocYn		    ");
                    ////sbQuery.AppendLine("       ,TgtPoc		    = :TgtPoc		    ");
                    //sbQuery.AppendLine("  WHERE ItemNo          = :ItemNo			");

                    sbQuery.AppendLine();
                    sbQuery.AppendLine("    UPDATE TAR_MST							        ");
                    sbQuery.AppendLine("    SET     CONTR_QTY		 = :ContractAmt	        ");
                    sbQuery.AppendLine("        ,   QTY_YN  		 = :AmtControlYn		");
                    sbQuery.AppendLine("        ,   QTY_RT			 = :AmtControlRate	    ");
                    sbQuery.AppendLine("        ,   TAR_AREA_YN  	 = :TgtRegion1Yn		");
                    sbQuery.AppendLine("        ,   TAR_AREA_VAL     = :TgtRegion1		    ");
                    sbQuery.AppendLine("        ,   TAR_HRZONE_YN    = :TgtTimeYn		    ");
                    sbQuery.AppendLine("        ,   TAR_HRZONE_VAL   = :TgtTime			    ");
                    sbQuery.AppendLine("        ,   TAR_AGE_YN       = :TgtAgeYn			");
                    sbQuery.AppendLine("        ,   TAR_AGE_VAL      = :TgtAge	            ");
                    sbQuery.AppendLine("        ,   TAR_SEX_YN       = :TgtSexYn			");
                    sbQuery.AppendLine("        ,   TAR_SEX_MAN_YN   = :TgtSexMan		    ");
                    sbQuery.AppendLine("        ,   TAR_SEX_WOMAN_YN = :TgtSexWoman		    ");
                    sbQuery.AppendLine("        ,   TAR_RATE_YN      = :TgtRateYn		    ");
                    sbQuery.AppendLine("        ,   TAR_RATE_VAL     = :TgtRate			    ");
                    sbQuery.AppendLine("        ,   TAR_WEEK_YN      = :TgtWeekYn		    ");
                    sbQuery.AppendLine("        ,   TAR_WEEK_VAL     = :TgtWeek			    ");
                    sbQuery.AppendLine("        ,   TAR_PPX_YN		 = :TgtPPxYn			");
                    sbQuery.AppendLine("        ,   TAR_FRQ_YN		 = :TgtFreqYn		    ");
                    sbQuery.AppendLine("        ,   TAR_FRQ_DY		 = :TgtFreqDay		    ");
                    sbQuery.AppendLine("        ,   TAR_FRQ_PERD	 = :TgtFreqPeriod		");
                    sbQuery.AppendLine("        ,   ID_UPDATE		 = :IdUpdate		    ");
                    sbQuery.AppendLine("        ,   DT_UPDATE		 = SYSDATE  		    ");
                    sbQuery.AppendLine("        ,   TAR_OS_YN		 = :TgtStbTypeYn		");
                    sbQuery.AppendLine("        ,   TAR_OS_VAL		 = :TgtStbType			");
                    sbQuery.AppendLine("    WHERE ITEM_NO			 = :ItemNo              ");

					rc =  _db.ExecuteNonQueryParams(sbQuery.ToString(), sqlParams);

					if(rc < 1)
					{
						// 지정채널 편성 테이블에 추가
						sbQuery = new StringBuilder();
                        //sbQuery.Append("\n");
                        //sbQuery.Append(" INSERT INTO Targeting		  \n");
                        //sbQuery.Append("            (ItemNo			  \n");
                        //sbQuery.Append("            ,ContractAmt	  \n");
                        //sbQuery.Append("            ,PriorityAmt	  \n");
                        //sbQuery.Append("            ,PriorityValue	  \n");
                        //sbQuery.Append("            ,PriorityCd		  \n");
                        //sbQuery.Append("            ,AmtControlYn    \n");
                        //sbQuery.Append("            ,AmtControlRate  \n");
                        //sbQuery.Append("            ,TgtRegion1Yn    \n");
                        //sbQuery.Append("            ,TgtRegion1      \n");
                        //sbQuery.Append("            ,TgtTimeYn       \n");
                        //sbQuery.Append("            ,TgtTime         \n");
                        //sbQuery.Append("            ,TgtAgeYn        \n");
                        //sbQuery.Append("            ,TgtAge          \n");
                        //sbQuery.Append("            ,TgtAgeBtnYn     \n");
                        //sbQuery.Append("            ,TgtAgeBtnBegin  \n");
                        //sbQuery.Append("            ,TgtAgeBtnEnd    \n");
                        //sbQuery.Append("            ,TgtSexYn        \n");
                        //sbQuery.Append("            ,TgtSexMan       \n");
                        //sbQuery.Append("            ,TgtSexWoman     \n");
                        //sbQuery.Append("            ,TgtRateYn       \n");
                        //sbQuery.Append("            ,TgtRate         \n");
                        //sbQuery.Append("            ,TgtWeekYn       \n");
                        //sbQuery.Append("            ,TgtWeek         \n");
                        //sbQuery.Append("            ,TgtCollectionYn       \n");
                        ////sbQuery.Append("            ,TgtCollection         \n");

                        //sbQuery.Append("			,TgtZipYn		\n");
                        //sbQuery.Append("			,TgtZip			\n");
                        //sbQuery.Append("			,TgtPPxYn		\n");
                        //sbQuery.Append("			,TgtFreqYn		\n");
                        //sbQuery.Append("			,TgtFreqDay		\n");
                        //sbQuery.Append("			,TgtFreqPeriod	\n");
                        //sbQuery.Append("			,TgtPVDBYn		\n");

                        //sbQuery.Append("            ,SlotExt         \n"); // 2slot
                        //sbQuery.Append("            ,TgtStbTypeYn    \n"); // [E_05] 셋탑모델정보
                        //sbQuery.Append("            ,TgtStbType      \n"); // [E_05] 셋탑모델정보
                        //sbQuery.Append("            ,TgtPrefYn       \n"); // [E_06] 선호도조사팝업
                        //sbQuery.Append("            ,TgtPrefRate     \n"); // [E_06] 선호도조사팝업
                        //sbQuery.Append("            ,TgtPrefNosendYn \n"); // [E_06] 선호도조사팝업
                        //sbQuery.Append("            ,TgtProfileYn    \n"); // [E_07] 프로파일 타겟팅 사용유무
                        //sbQuery.Append("            ,TgtProfile      \n"); // [E_07] 프로파일 타겟팅 연령대
                        //sbQuery.Append("            ,TgtReliability  \n"); // [E_07] 프로파일 타겟팅 신뢰도
                        //sbQuery.Append("            ,TgtPocYn        \n"); // [E_07] 프로파일 타겟팅 연령대
                        //sbQuery.Append("            ,TgtPoc          \n"); // [E_07] 프로파일 타겟팅 신뢰도

                        //sbQuery.Append("            )				  \n");
                        //sbQuery.Append("    VALUES	(@ItemNo		  \n");
                        //sbQuery.Append("            ,@ContractAmt	  \n");
                        //sbQuery.Append("            ,10000000		  \n");		// 기본값 설정
                        //sbQuery.Append("            ,0           	  \n");
                        //sbQuery.Append("            ,@PriorityCd	  \n");
                        //sbQuery.Append("            ,@AmtControlYn    \n");
                        //sbQuery.Append("            ,@AmtControlRate  \n");
                        //sbQuery.Append("            ,@TgtRegion1Yn    \n");
                        //sbQuery.Append("            ,@TgtRegion1      \n");
                        //sbQuery.Append("            ,@TgtTimeYn       \n");
                        //sbQuery.Append("            ,@TgtTime         \n");
                        //sbQuery.Append("            ,@TgtAgeYn        \n");
                        //sbQuery.Append("            ,@TgtAge          \n");
                        //sbQuery.Append("            ,@TgtAgeBtnYn     \n");
                        //sbQuery.Append("            ,@TgtAgeBtnBegin  \n");
                        //sbQuery.Append("            ,@TgtAgeBtnEnd    \n");
                        //sbQuery.Append("            ,@TgtSexYn        \n");
                        //sbQuery.Append("            ,@TgtSexMan       \n");
                        //sbQuery.Append("            ,@TgtSexWoman     \n");
                        //sbQuery.Append("            ,@TgtRateYn       \n");
                        //sbQuery.Append("            ,@TgtRate         \n");
                        //sbQuery.Append("            ,@TgtWeekYn       \n");
                        //sbQuery.Append("            ,@TgtWeek		  \n");
                        //sbQuery.Append("            ,@TgtCollectionYn \n");
                        ////sbQuery.Append("            ,@TgtCollection	  \n");
                        //sbQuery.Append("			,@TgtZipYn		  \n");
                        //sbQuery.Append("			,@TgtZip		  \n");
                        //sbQuery.Append("			,@TgtPPxYn		  \n");
                        //sbQuery.Append("			,@TgtFreqYn		  \n");
                        //sbQuery.Append("			,@TgtFreqDay	  \n");
                        //sbQuery.Append("			,@TgtFreqPeriod	  \n");
                        //sbQuery.Append("			,@TgtPVSYn	      \n");
                        //sbQuery.Append("            ,@SlotExt         \n"); // 2slot
                        //sbQuery.Append("            ,@TgtStbTypeYn    \n"); // [E_05] 셋탑모델정보
                        //sbQuery.Append("            ,@TgtStbType      \n"); // [E_05] 셋탑모델정보
                        //sbQuery.Append("            ,@TgtPrefYn       \n"); // [E_06] 선호도조사팝업
                        //sbQuery.Append("            ,@TgtPrefRate     \n"); // [E_06] 선호도조사팝업
                        //sbQuery.Append("            ,@TgtPrefNosendYn \n"); // [E_06] 선호도조사팝업
                        //sbQuery.Append("            ,@TgtProfileYn    \n"); // [E_07] 프로파일 타겟팅 사용유무
                        //sbQuery.Append("            ,@TgtProfile      \n"); // [E_07] 프로파일 타겟팅 연령대
                        //sbQuery.Append("            ,@TgtReliability  \n"); // [E_07] 프로파일 타겟팅 신뢰도
                        //sbQuery.Append("			,@TgtPocYn		  \n");
                        //sbQuery.Append("			,@TgtPoc		  \n");
                        //sbQuery.Append(" 			)				  \n");
                        sbQuery.AppendLine();
                        sbQuery.AppendLine("        INSERT INTO TAR_MST		            ");
                        sbQuery.AppendLine("                (	ITEM_NO	    	  	  	");
                        sbQuery.AppendLine("                ,	CONTR_QTY               ");
                        sbQuery.AppendLine("                ,	QTY_YN                  ");
                        sbQuery.AppendLine("                ,	QTY_RT                  ");
                        sbQuery.AppendLine("                ,	TAR_AREA_YN             ");
                        sbQuery.AppendLine("                ,	TAR_AREA_VAL            ");
                        sbQuery.AppendLine("                ,	TAR_HRZONE_YN           ");
                        sbQuery.AppendLine("                ,	TAR_HRZONE_VAL          ");
                        sbQuery.AppendLine("                ,	TAR_AGE_YN              ");
                        sbQuery.AppendLine("                ,	TAR_AGE_VAL             ");
                        sbQuery.AppendLine("                ,	TAR_SEX_YN              ");
                        sbQuery.AppendLine("                ,	TAR_SEX_MAN_YN          ");
                        sbQuery.AppendLine("                ,	TAR_SEX_WOMAN_YN        ");
                        sbQuery.AppendLine("                ,	TAR_RATE_YN             ");
                        sbQuery.AppendLine("                ,	TAR_RATE_VAL            ");
                        sbQuery.AppendLine("                ,	TAR_WEEK_YN             ");
                        sbQuery.AppendLine("                ,	TAR_WEEK_VAL         	");
                        sbQuery.AppendLine("                ,	TAR_PPX_YN		        ");
                        sbQuery.AppendLine("                ,	TAR_FRQ_YN		        ");
                        sbQuery.AppendLine("                ,	TAR_FRQ_DY		        ");
                        sbQuery.AppendLine("                ,	TAR_FRQ_PERD	        ");
                        sbQuery.AppendLine("                ,	ID_INSERT   	        ");
                        sbQuery.AppendLine("                ,	DT_INSERT   	        ");
                        sbQuery.AppendLine("                ,	TAR_OS_YN               ");
                        sbQuery.AppendLine("                ,	TAR_OS_VAL              ");
		                sbQuery.AppendLine("                )				            ");
                        sbQuery.AppendLine("        VALUES	(	:ItemNo	  	            ");
                        sbQuery.AppendLine("                ,	:ContractAmt            ");
		                sbQuery.AppendLine("                ,	:AmtControlYn           ");
		                sbQuery.AppendLine("                ,	:AmtControlRate         ");
		                sbQuery.AppendLine("                ,	:TgtRegion1Yn           ");
		                sbQuery.AppendLine("                ,	:TgtRegion1             ");
		                sbQuery.AppendLine("                ,	:TgtTimeYn              ");
		                sbQuery.AppendLine("                ,	:TgtTime                ");
		                sbQuery.AppendLine("                ,	:TgtAgeYn               ");
		                sbQuery.AppendLine("                ,	:TgtAge                 ");
		                sbQuery.AppendLine("                ,	:TgtSexYn               ");
		                sbQuery.AppendLine("                ,	:TgtSexMan              ");
		                sbQuery.AppendLine("                ,	:TgtSexWoman            ");
		                sbQuery.AppendLine("                ,	:TgtRateYn              ");
		                sbQuery.AppendLine("                ,	:TgtRate                ");
		                sbQuery.AppendLine("                ,	:TgtWeekYn              ");
		                sbQuery.AppendLine("                ,	:TgtWeek		  		");  
		                sbQuery.AppendLine("                ,	:TgtPPxYn		        ");
		                sbQuery.AppendLine("                ,	:TgtFreqYn		        ");
		                sbQuery.AppendLine("                ,	:TgtFreqDay	            ");
		                sbQuery.AppendLine("                ,	:TgtFreqPeriod	        ");
                        sbQuery.AppendLine("                ,	:IdInsert	            ");
                        sbQuery.AppendLine("                ,	SYSDATE      	        ");
		                sbQuery.AppendLine("                ,	:TgtStbTypeYn           ");
		                sbQuery.AppendLine("                ,	:TgtStbType             ");
		                sbQuery.AppendLine("                )                           ");

                        sqlParams = new OracleParameter[24];

                        i = 0;
                        sqlParams[i++] = new OracleParameter(":ItemNo", OracleDbType.Int32);
                        sqlParams[i++] = new OracleParameter(":ContractAmt", OracleDbType.Int32);
                        sqlParams[i++] = new OracleParameter(":AmtControlYn", OracleDbType.Char, 1);
                        sqlParams[i++] = new OracleParameter(":AmtControlRate", OracleDbType.Int32);
                        sqlParams[i++] = new OracleParameter(":TgtRegion1Yn", OracleDbType.Char, 1);
                        sqlParams[i++] = new OracleParameter(":TgtRegion1", OracleDbType.Varchar2, 2000); // [E_01] 지역정보확대로 인한 Length 증가(기존512)
                        sqlParams[i++] = new OracleParameter(":TgtTimeYn", OracleDbType.Char, 1);
                        sqlParams[i++] = new OracleParameter(":TgtTime", OracleDbType.Varchar2, 200);  // [E_02] 시간 타겟팅 변경으로 인한 Length증가(기존128)
                        sqlParams[i++] = new OracleParameter(":TgtAgeYn", OracleDbType.Char, 1);
                        sqlParams[i++] = new OracleParameter(":TgtAge", OracleDbType.Varchar2, 200);
                        sqlParams[i++] = new OracleParameter(":TgtSexYn", OracleDbType.Char, 1);
                        sqlParams[i++] = new OracleParameter(":TgtSexMan", OracleDbType.Char, 1);
                        sqlParams[i++] = new OracleParameter(":TgtSexWoman", OracleDbType.Char, 1);
                        sqlParams[i++] = new OracleParameter(":TgtRateYn", OracleDbType.Char, 1);
                        sqlParams[i++] = new OracleParameter(":TgtRate", OracleDbType.Int32);
                        sqlParams[i++] = new OracleParameter(":TgtWeekYn", OracleDbType.Char, 1);
                        sqlParams[i++] = new OracleParameter(":TgtWeek", OracleDbType.Varchar2, 20);
                        sqlParams[i++] = new OracleParameter(":TgtPPxYn", OracleDbType.Char, 1);
                        sqlParams[i++] = new OracleParameter(":TgtFreqYn", OracleDbType.Char, 1);
                        sqlParams[i++] = new OracleParameter(":TgtFreqDay", OracleDbType.Int32);
                        sqlParams[i++] = new OracleParameter(":TgtFreqPeriod", OracleDbType.Int32);
                        sqlParams[i++] = new OracleParameter(":IdInsert", OracleDbType.Varchar2, 10);
                        sqlParams[i++] = new OracleParameter(":TgtStbTypeYn", OracleDbType.Char, 1);     // [E_05] 셋탑모델 사용여부
                        sqlParams[i++] = new OracleParameter(":TgtStbType", OracleDbType.Varchar2, 20);   // [E_05] 셋탑모델별    

                        i = 0;
                        sqlParams[i++].Value = Convert.ToInt32(targetingModel.ItemNo);
                        sqlParams[i++].Value = Convert.ToInt32(targetingModel.ContractAmt);   
                        sqlParams[i++].Value = targetingModel.AmtControlYn;
                        sqlParams[i++].Value = Convert.ToInt32(targetingModel.AmtControlRate);
                        sqlParams[i++].Value = targetingModel.TgtRegion1Yn;
                        sqlParams[i++].Value = targetingModel.TgtRegion1;
                        sqlParams[i++].Value = targetingModel.TgtTimeYn;
                        sqlParams[i++].Value = targetingModel.TgtTime;
                        sqlParams[i++].Value = targetingModel.TgtAgeYn;
                        sqlParams[i++].Value = targetingModel.TgtAge;  
                        sqlParams[i++].Value = targetingModel.TgtSexYn;
                        sqlParams[i++].Value = targetingModel.TgtSexMan;
                        sqlParams[i++].Value = targetingModel.TgtSexWoman;
                        sqlParams[i++].Value = targetingModel.TgtRateYn;
                        sqlParams[i++].Value = Convert.ToInt32(targetingModel.TgtRate);
                        sqlParams[i++].Value = targetingModel.TgtWeekYn;
                        sqlParams[i++].Value = targetingModel.TgtWeek;
                        sqlParams[i++].Value = targetingModel.TgtPPxYn;
                        sqlParams[i++].Value = targetingModel.TgtFreqYn;
                        sqlParams[i++].Value = Convert.ToInt32(targetingModel.TgtFreqDay);
                        sqlParams[i++].Value = Convert.ToInt32(targetingModel.TgtFreqFeriod);
                        sqlParams[i++].Value = header.UserID;
                        sqlParams[i++].Value = targetingModel.TgtStbModelYn;    // [E_05]
                        sqlParams[i++].Value = targetingModel.TgtStbModel;      // [E_05]

                        i = 0;
                        sqlParams[i++].Direction = ParameterDirection.Input;
                        sqlParams[i++].Direction = ParameterDirection.Input;
                        sqlParams[i++].Direction = ParameterDirection.Input;
                        sqlParams[i++].Direction = ParameterDirection.Input;
                        sqlParams[i++].Direction = ParameterDirection.Input;
                        sqlParams[i++].Direction = ParameterDirection.Input;
                        sqlParams[i++].Direction = ParameterDirection.Input;
                        sqlParams[i++].Direction = ParameterDirection.Input;
                        sqlParams[i++].Direction = ParameterDirection.Input;
                        sqlParams[i++].Direction = ParameterDirection.Input;
                        sqlParams[i++].Direction = ParameterDirection.Input;
                        sqlParams[i++].Direction = ParameterDirection.Input;
                        sqlParams[i++].Direction = ParameterDirection.Input;
                        sqlParams[i++].Direction = ParameterDirection.Input;
                        sqlParams[i++].Direction = ParameterDirection.Input;
                        sqlParams[i++].Direction = ParameterDirection.Input;
                        sqlParams[i++].Direction = ParameterDirection.Input;
                        sqlParams[i++].Direction = ParameterDirection.Input;
                        sqlParams[i++].Direction = ParameterDirection.Input;
                        sqlParams[i++].Direction = ParameterDirection.Input;
                        sqlParams[i++].Direction = ParameterDirection.Input;
                        sqlParams[i++].Direction = ParameterDirection.Input;
                        sqlParams[i++].Direction = ParameterDirection.Input;
                        sqlParams[i++].Direction = ParameterDirection.Input;

						rc =  _db.ExecuteNonQueryParams(sbQuery.ToString(), sqlParams);
					}

					// 2007.10.25 RH.Jung 노출값재설정을 이제는 안해도 됨. 서버에서 실시간 처리함
					//SetPriorityValues(targetingModel.ItemNo);
                    _log.Debug(sbQuery.ToString());
					_db.CommitTran();
            
					// __MESSAGE__
					_log.Message("광고 타겟팅 수정:["+targetingModel.ItemNo +"] " + targetingModel.ItemName + " 등록자:[" + header.UserID + "]");
            
				}
				catch(Exception ex)
				{
					_db.RollbackTran();
					throw ex;
				}
            
				targetingModel.ResultCD = "0000";  // 정상
            
				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "SetTargetingDetailUpdate() End");
				_log.Debug("-----------------------------------------");	
			}
			catch(Exception ex)
			{
				targetingModel.ResultCD   = "3101";
				targetingModel.ResultDesc = "광고 타겟팅 수정 중 오류가 발생하였습니다";
				_log.Exception(ex);
			}
			finally
			{
				// 데이터베이스를  Close한다
				_db.Close();
			}
		}
		#endregion

        #region 광고 타겟팅 비율 저장 ------------------------------------------------- 기능 제외 임시 폐기

        /// <summary>
		/// 광고 타겟팅 비율 저장
		/// </summary>
		/// <returns></returns>
		public void SetTargetingRateUpdate(HeaderModel header, TargetingModel targetingModel)
		{

			try
			{
				// 데이터베이스를 OPEN한다
				_db.Open();

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + ".SetTargetingRateUpdate() Start");
				_log.Debug("-----------------------------------------");

				_log.Debug("ItemNo         :[" + targetingModel.ItemNo         + "]");		
				_log.Debug("Type   :[" + targetingModel.Type   + "]");		
				_log.Debug("Rate1  :[" + targetingModel.Rate1  + "]");		
				_log.Debug("Rate2  :[" + targetingModel.Rate2  + "]");		
				_log.Debug("Rate3  :[" + targetingModel.Rate3  + "]");		
				_log.Debug("Rate4  :[" + targetingModel.Rate4  + "]");		
				_log.Debug("Rate5  :[" + targetingModel.Rate5  + "]");		
				_log.Debug("Rate6  :[" + targetingModel.Rate6  + "]");		
				_log.Debug("Rate7  :[" + targetingModel.Rate7  + "]");		
				_log.Debug("Rate8  :[" + targetingModel.Rate8  + "]");		
				_log.Debug("Rate9  :[" + targetingModel.Rate9  + "]");		
				_log.Debug("Rate10 :[" + targetingModel.Rate10 + "]");		
				_log.Debug("Rate11 :[" + targetingModel.Rate11 + "]");		
				_log.Debug("Rate12 :[" + targetingModel.Rate12 + "]");		
				_log.Debug("Rate13 :[" + targetingModel.Rate13 + "]");		
				_log.Debug("Rate14 :[" + targetingModel.Rate14 + "]");		
				_log.Debug("Rate15 :[" + targetingModel.Rate15 + "]");		
				_log.Debug("Rate16 :[" + targetingModel.Rate16 + "]");		
				_log.Debug("Rate17 :[" + targetingModel.Rate17 + "]");		
				_log.Debug("Rate18 :[" + targetingModel.Rate18 + "]");		
				_log.Debug("Rate19 :[" + targetingModel.Rate19 + "]");		
				_log.Debug("Rate20 :[" + targetingModel.Rate20 + "]");		
				_log.Debug("Rate21 :[" + targetingModel.Rate21 + "]");		
				_log.Debug("Rate22 :[" + targetingModel.Rate22 + "]");		
				_log.Debug("Rate23 :[" + targetingModel.Rate23 + "]");		
				_log.Debug("Rate24 :[" + targetingModel.Rate24 + "]");		
				
				int i = 0;
				int rc = 0;
				StringBuilder  sbQuery;
				SqlParameter[] sqlParams = new SqlParameter[26];
	            
				i = 0;
				sqlParams[i++] = new SqlParameter("@ItemNo"          , SqlDbType.Int);
				sqlParams[i++] = new SqlParameter("@Type"			 , SqlDbType.Char     ,2);
				sqlParams[i++] = new SqlParameter("@Rate1"   , SqlDbType.TinyInt);
				sqlParams[i++] = new SqlParameter("@Rate2"   , SqlDbType.TinyInt);
				sqlParams[i++] = new SqlParameter("@Rate3"   , SqlDbType.TinyInt);
				sqlParams[i++] = new SqlParameter("@Rate4"   , SqlDbType.TinyInt);
				sqlParams[i++] = new SqlParameter("@Rate5"   , SqlDbType.TinyInt);
				sqlParams[i++] = new SqlParameter("@Rate6"   , SqlDbType.TinyInt);
				sqlParams[i++] = new SqlParameter("@Rate7"   , SqlDbType.TinyInt);
				sqlParams[i++] = new SqlParameter("@Rate8"   , SqlDbType.TinyInt);
				sqlParams[i++] = new SqlParameter("@Rate9"   , SqlDbType.TinyInt);
				sqlParams[i++] = new SqlParameter("@Rate10"  , SqlDbType.TinyInt);
				sqlParams[i++] = new SqlParameter("@Rate11"  , SqlDbType.TinyInt);
				sqlParams[i++] = new SqlParameter("@Rate12"  , SqlDbType.TinyInt);
				sqlParams[i++] = new SqlParameter("@Rate13"  , SqlDbType.TinyInt);
				sqlParams[i++] = new SqlParameter("@Rate14"  , SqlDbType.TinyInt);
				sqlParams[i++] = new SqlParameter("@Rate15"  , SqlDbType.TinyInt);
				sqlParams[i++] = new SqlParameter("@Rate16"  , SqlDbType.TinyInt);
				sqlParams[i++] = new SqlParameter("@Rate17"  , SqlDbType.TinyInt);
				sqlParams[i++] = new SqlParameter("@Rate18"  , SqlDbType.TinyInt);
				sqlParams[i++] = new SqlParameter("@Rate19"  , SqlDbType.TinyInt);
				sqlParams[i++] = new SqlParameter("@Rate20"        , SqlDbType.TinyInt);
				sqlParams[i++] = new SqlParameter("@Rate21"        , SqlDbType.TinyInt		  );
				sqlParams[i++] = new SqlParameter("@Rate22"        , SqlDbType.TinyInt		  );
				sqlParams[i++] = new SqlParameter("@Rate23"        , SqlDbType.TinyInt		  );
				sqlParams[i++] = new SqlParameter("@Rate24"        , SqlDbType.TinyInt		  );

				i = 0;
				sqlParams[i++].Value = Convert.ToInt32(targetingModel.ItemNo);        
				sqlParams[i++].Value = targetingModel.Type;
				sqlParams[i++].Value = Convert.ToInt16(targetingModel.Rate1);   
				sqlParams[i++].Value = Convert.ToInt16(targetingModel.Rate2);
				sqlParams[i++].Value = Convert.ToInt16(targetingModel.Rate3);
				sqlParams[i++].Value = Convert.ToInt16(targetingModel.Rate4);
				sqlParams[i++].Value = Convert.ToInt16(targetingModel.Rate5);
				sqlParams[i++].Value = Convert.ToInt16(targetingModel.Rate6);
				sqlParams[i++].Value = Convert.ToInt16(targetingModel.Rate7);
				sqlParams[i++].Value = Convert.ToInt16(targetingModel.Rate8);
				sqlParams[i++].Value = Convert.ToInt16(targetingModel.Rate9);
				sqlParams[i++].Value = Convert.ToInt16(targetingModel.Rate10);
				sqlParams[i++].Value = Convert.ToInt16(targetingModel.Rate11);
				sqlParams[i++].Value = Convert.ToInt16(targetingModel.Rate12);
				sqlParams[i++].Value = Convert.ToInt16(targetingModel.Rate13);
				sqlParams[i++].Value = Convert.ToInt16(targetingModel.Rate14);
				sqlParams[i++].Value = Convert.ToInt16(targetingModel.Rate15);
				sqlParams[i++].Value = Convert.ToInt16(targetingModel.Rate16);
				sqlParams[i++].Value = Convert.ToInt16(targetingModel.Rate17);
				sqlParams[i++].Value = Convert.ToInt16(targetingModel.Rate18);
				sqlParams[i++].Value = Convert.ToInt16(targetingModel.Rate19);
				sqlParams[i++].Value = Convert.ToInt16(targetingModel.Rate20);
				sqlParams[i++].Value = Convert.ToInt16(targetingModel.Rate21);
				sqlParams[i++].Value = Convert.ToInt16(targetingModel.Rate22);
				sqlParams[i++].Value = Convert.ToInt16(targetingModel.Rate23);
				sqlParams[i++].Value = Convert.ToInt16(targetingModel.Rate24);
				
				// 쿼리실행
				try
				{
					_db.BeginTran();


					sbQuery = new StringBuilder();
					sbQuery.Append(" UPDATE TargetingRate       \n");
					sbQuery.Append("    SET Rate1		= @Rate1\n");
					sbQuery.Append("       ,Rate2		= @Rate2\n");
					sbQuery.Append("       ,Rate3		= @Rate3 \n");
					sbQuery.Append("       ,Rate4		= @Rate4 \n");
					sbQuery.Append("       ,Rate5		= @Rate5 \n");
					sbQuery.Append("       ,Rate6		= @Rate6 \n");
					sbQuery.Append("       ,Rate7		= @Rate7 \n");
					sbQuery.Append("       ,Rate8		= @Rate8 \n");
					sbQuery.Append("       ,Rate9		= @Rate9 \n");
					sbQuery.Append("       ,Rate10		= @Rate10 \n");
					sbQuery.Append("       ,Rate11		= @Rate11 \n");
					sbQuery.Append("       ,Rate12		= @Rate12 \n");
					sbQuery.Append("       ,Rate13		= @Rate13 \n");
					sbQuery.Append("       ,Rate14		= @Rate14 \n");
					sbQuery.Append("       ,Rate15		= @Rate15 \n");
					sbQuery.Append("       ,Rate16		= @Rate16 \n");
					sbQuery.Append("       ,Rate17		= @Rate17 \n");
					sbQuery.Append("       ,Rate18		= @Rate18 \n");
					sbQuery.Append("       ,Rate19		= @Rate19 \n");
					sbQuery.Append("       ,Rate20		= @Rate20 \n");
					sbQuery.Append("       ,Rate21     	= @Rate21       \n");
					sbQuery.Append("       ,Rate22     	= @Rate22       \n");
					sbQuery.Append("       ,Rate23     	= @Rate23       \n");
					sbQuery.Append("       ,Rate24     	= @Rate24       \n");
					sbQuery.Append("  WHERE ItemNo      = @ItemNo		  \n");
					sbQuery.Append("	AND Type        = @Type			  \n");


					rc =  _db.ExecuteNonQueryParams(sbQuery.ToString(), sqlParams);

					if(rc < 1)
					{
						// 지정채널 편성 테이블에 추가
						sbQuery = new StringBuilder();
						sbQuery.Append("\n");
						sbQuery.Append(" INSERT INTO TargetingRate    \n");
						sbQuery.Append("            (ItemNo			  \n");
						sbQuery.Append("            ,Type			  \n");
						sbQuery.Append("            ,Rate1			  \n");
						sbQuery.Append("            ,Rate2			  \n");
						sbQuery.Append("            ,Rate3			  \n");
						sbQuery.Append("            ,Rate4			  \n");
						sbQuery.Append("            ,Rate5			  \n");
						sbQuery.Append("            ,Rate6			  \n");
						sbQuery.Append("            ,Rate7			  \n");
						sbQuery.Append("            ,Rate8			  \n");
						sbQuery.Append("            ,Rate9			  \n");
						sbQuery.Append("            ,Rate10			  \n");
						sbQuery.Append("            ,Rate11			  \n");
						sbQuery.Append("            ,Rate12			  \n");
						sbQuery.Append("            ,Rate13			  \n");
						sbQuery.Append("            ,Rate14			  \n");
						sbQuery.Append("            ,Rate15			  \n");
						sbQuery.Append("            ,Rate16			  \n");
						sbQuery.Append("            ,Rate17			  \n");
						sbQuery.Append("            ,Rate18			  \n");
						sbQuery.Append("            ,Rate19			  \n");
						sbQuery.Append("            ,Rate20			  \n");
						sbQuery.Append("            ,Rate21			  \n");
						sbQuery.Append("            ,Rate22			  \n");
						sbQuery.Append("            ,Rate23           \n");
						sbQuery.Append("            ,Rate24			  \n");						
						sbQuery.Append("            )				  \n");
						sbQuery.Append("      VALUES				  \n");
						sbQuery.Append("            (@ItemNo		  \n");
						sbQuery.Append("            ,@Type			  \n");
						sbQuery.Append("            ,@Rate1			  \n");
						sbQuery.Append("            ,@Rate2			  \n");
						sbQuery.Append("            ,@Rate3			  \n");
						sbQuery.Append("            ,@Rate4			  \n");
						sbQuery.Append("            ,@Rate5			  \n");
						sbQuery.Append("            ,@Rate6			  \n");
						sbQuery.Append("            ,@Rate7			  \n");
						sbQuery.Append("            ,@Rate8			  \n");
						sbQuery.Append("            ,@Rate9			  \n");
						sbQuery.Append("            ,@Rate10		  \n");
						sbQuery.Append("            ,@Rate11		  \n");
						sbQuery.Append("            ,@Rate12		  \n");
						sbQuery.Append("            ,@Rate13		  \n");
						sbQuery.Append("            ,@Rate14		  \n");
						sbQuery.Append("            ,@Rate15		  \n");
						sbQuery.Append("            ,@Rate16		  \n");
						sbQuery.Append("            ,@Rate17		  \n");
						sbQuery.Append("            ,@Rate18		  \n");
						sbQuery.Append("            ,@Rate19		  \n");
						sbQuery.Append("            ,@Rate20		  \n");
						sbQuery.Append("            ,@Rate21	      \n");
						sbQuery.Append("            ,@Rate22	      \n");
						sbQuery.Append("            ,@Rate23		  \n");
						sbQuery.Append("            ,@Rate24	      \n");
						sbQuery.Append(" 			)				  \n");

						rc =  _db.ExecuteNonQueryParams(sbQuery.ToString(), sqlParams);

					}

					// 2007.10.25 RH.Jung 노출값재설정을 이제는 안해도 됨. 서버에서 실시간 처리함
					//SetPriorityValues(targetingModel.ItemNo);

					_db.CommitTran();
            
					// __MESSAGE__
					_log.Message("광고 타겟팅 수정:["+targetingModel.ItemNo +"] " + targetingModel.Type + " 등록자:[" + header.UserID + "]");
            
				}
				catch(Exception ex)
				{
					_db.RollbackTran();
					throw ex;
				}
            
				targetingModel.ResultCD = "0000";  // 정상
            
				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "SetTargetingRateUpdate() End");
				_log.Debug("-----------------------------------------");	
			}
			catch(Exception ex)
			{
				targetingModel.ResultCD   = "3101";
				targetingModel.ResultDesc = "광고 타겟팅 비율수정 중 오류가 발생하였습니다";
				_log.Exception(ex);
			}
			finally
			{
				// 데이터베이스를  Close한다
				_db.Close();
			}

		}

		#endregion

        #region 노출값 재설정 --------------------------------------------------------- 기능 제외 임시 폐기

        /// <summary>
		/// 노출값 재설정
		/// </summary>
		/// <returns></returns>
		private void SetPriorityValues(string ItemNo)
		{

			try
			{
				string MediaCode = "";

				StringBuilder  sbQuery   = null;

				// 해당 광고가 속하여 있는 매체를 찾는다.
				sbQuery   = new StringBuilder();
				sbQuery.Append( "\n"
					+ " SELECT B.MediaCode \n"
					+ "  FROM Targeting  A INNER JOIN ContractItem B ON (A.ItemNo = B.ItemNo ) \n"
					);

				// 쿼리실행
				DataSet ds = new DataSet();
				_db.ExecuteQuery(ds,sbQuery.ToString());

				if (Utility.GetDatasetCount(ds) != 0)
				{
					MediaCode = Utility.GetDatasetString(ds, 0, "MediaCode");					
				}
				else
				{
					throw new Exception();
				}
				ds.Dispose();


				// 해당 매체의 운영중이며 편선중인 광고들의 노출빈도값의 합계를 구한다.
				// 해당 매체의 운영중이며 편선중인 광고들의 노출값을 수정한다.
				// 노출값 = 노출빈도의 합계 / 노출빈도 * 10

				sbQuery   = new StringBuilder();
				sbQuery.Append( "\n"
					+ " DECLARE @SumCd int, @ItemNo int;  \n"
					+ "                                   \n"
					+ "SELECT @SumCd  = SUM(A.PriorityCd) \n"
					+ "  FROM Targeting A with(nolock)    \n"
					+ "       INNER JOIN ContractItem B  with(nolock) ON (A.ItemNo = B.ItemNo AND B.AdState = '20' AND B.AdType BETWEEN '10' AND '19') -- 광고상태 20:편성  광고종류 10~19:필수광고 \n"
					+ "       INNER JOIN Contract     C  with(nolock) ON (B.MediaCode      = C.MediaCode      AND \n"
					+ "                                                 B.RapCode        = C.RapCode        AND \n"
					+ "                                                 B.AgencyCode     = C.AgencyCode     AND \n"
					+ "                                                 B.AdvertiserCode = C.AdvertiserCode AND \n"
					+ "                                                 B.ContractSeq    = C.ContractSeq    AND \n"
					+ "                                                 C.State  = '10' ) -- 계약상태 10:운영중 \n"
					+ " WHERE B.MediaCode = " + MediaCode + "\n"
					+ "                                      \n"
					+ "DECLARE SRC_CUR CURSOR                \n"
					+ "    FOR SELECT A.ItemNo               \n"
					+ "          FROM Targeting A  with(nolock) \n"
					+ "               INNER JOIN ContractItem B with(nolock) ON (A.ItemNo = B.ItemNo AND B.AdState = '20' AND B.AdType BETWEEN '10' AND '19') -- 광고상태 20:편성  광 광고종류 10~19:필수광고\n"
					+ "               INNER JOIN Contract     C with(nolock) ON (B.MediaCode      = C.MediaCode      AND \n"
					+ "                                                         B.RapCode        = C.RapCode        AND \n" 
					+ "                                                         B.AgencyCode     = C.AgencyCode     AND \n"
					+ "                                                         B.AdvertiserCode = C.AdvertiserCode AND \n"
					+ "                                                         B.ContractSeq    = C.ContractSeq    AND \n"
					+ "                                                         C.State  = '10' ) -- 계약상태 10:운영중 \n"
					+ "        WHERE B.MediaCode = " + MediaCode + "\n"
					+ "                                     \n"
					+ "OPEN SRC_CUR                         \n"
					+ "                                     \n"
					+ "FETCH NEXT FROM SRC_CUR INTO @ItemNo \n"
					+ "                                     \n"
					+ "WHILE @@FETCH_STATUS = 0             \n"
					+ "BEGIN                                \n"
					+ "                                     \n"
					+ "    UPDATE Targeting                 \n"
	   				+ "       SET PriorityValue =  FLOOR( (@SumCd/CAST(PriorityCd as float)) * 10)  \n"
					+ "	    WHERE ItemNo = @ItemNo          \n"
					+ "                                      \n"
					+ "	FETCH NEXT FROM SRC_CUR INTO @ItemNo \n"
					+ "                                      \n"    
					+ "END                                   \n"
					+ "                                      \n"
					+ "CLOSE SRC_CUR                         \n"
					+ "DEALLOCATE SRC_CUR                    \n"
					);

				int rc =  _db.ExecuteNonQuery(sbQuery.ToString());

			}
			catch(Exception ex)
			{
				_log.Exception(ex);
				throw ex;
			}

		}


		
		

		#endregion

		#region 지역정보 가져오기
		/// <summary>
		/// 광고지역 조회
		/// </summary>
		/// <param name="targetingModel"></param>
		public void GetRegionList(HeaderModel header, TargetingModel targetingModel)
		{
			try
			{
				// 데이터베이스를 OPEN한다
				_db.Open();
				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetRegionList() Start");
				_log.Debug("-----------------------------------------");

				StringBuilder sbQuery = new StringBuilder();

				// 쿼리생성
				/* [E_01]로 인해서 주석
				sbQuery.Append("\n"
					+ "-- 지역코드조회      \n"
					+ "SELECT SummaryCode AS RegionCode    \n"
					+ "      ,SummaryName AS RegionName    \n"
					+ "      ,UpperCode       \n"
					+ "      ,Level           \n"
					+ "  FROM SummaryCode   with(nolock)    \n"	
					+ " WHERE SummaryType = 5 \n"
					+ " ORDER BY SummaryCode  \n"
					);
				*/

				// [E_01] 기능 추가
                /* 기존운영을 위해서 테이블을 다른걸 사용함
				sbQuery.Append(" Select                           \n");
				sbQuery.Append(" 	 SummaryCode As RegionCode    \n");
				sbQuery.Append(" 	,SummaryName As RegionName    \n");
				sbQuery.Append(" 	,UpperCode                    \n");
				sbQuery.Append(" 	,[Level]                      \n");
				sbQuery.Append(" From  SummaryCode                \n"); 
				sbQuery.Append(" Where 1 = 1                      \n");
				sbQuery.Append(" And   SummaryType = 5            \n");
				sbQuery.Append(" Order By Orders                  \n");
                */

                //sbQuery.Append(" Select                           \n");
                //sbQuery.Append(" 	 RegionCode As RegionCode    \n");
                //sbQuery.Append(" 	,RegionName As RegionName    \n");
                //sbQuery.Append(" 	,UpperCode                    \n");
                //sbQuery.Append(" 	,[Level]                      \n");
                //sbQuery.Append(" From  TargetRegion nolock        \n"); 
                //sbQuery.Append(" Order By Orders                  \n");

                sbQuery.AppendLine();
                sbQuery.AppendLine("    SELECT	REGION_COD  AS RegionCode   ");
	            sbQuery.AppendLine("        ,	REGION_NM   AS RegionName   ");
	            sbQuery.AppendLine("        ,	UPPER_COD   AS UpperCode    ");
	            sbQuery.AppendLine("        ,	LVL         AS \"Level\"    ");
                sbQuery.AppendLine("    FROM TAR_REGION                     ");
                sbQuery.AppendLine("    ORDER BY ORDS                       ");

				_log.Debug(sbQuery.ToString());
				
				// 쿼리실행
				DataSet ds = new DataSet();
				_db.ExecuteQuery(ds,sbQuery.ToString());

				targetingModel.RegionDataSet = ds.Copy();
				targetingModel.ResultCnt = Utility.GetDatasetCount(targetingModel.RegionDataSet);
				targetingModel.ResultCD = "0000";
				ds.Dispose();

				// __DEBUG__
				_log.Debug("<출력정보>");
				_log.Debug("ResultCnt:[" + targetingModel.ResultCnt + "]");
				// __DEBUG__

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetRegionList() End");
				_log.Debug("-----------------------------------------");
			}
			catch(Exception ex)
			{
				targetingModel.ResultCD = "3000";
				targetingModel.ResultDesc = "광고지역 조회중 오류가 발생하였습니다";
				_log.Exception(ex);
			}
			finally
			{
				// 데이터베이스를  Close한다
				_db.Close();
			}
		}
		#endregion

		#region 연령대 조회
		/// <summary>
		/// 연령대 조회
		/// </summary>
		/// <param name="targetingModel"></param>
		public void GetAgeList(HeaderModel header, TargetingModel targetingModel)
		{
			try
			{
                // Summary DB가 별도로 존재하지 않음.
                //// [E_08] GetAgeList() 함수만 AdtargetsSummary를 보도록 수정
                //_db.ConnectionString = FrameSystem.connSummaryDbString;
                
                // 데이터베이스를 OPEN한다
                _db.Open();
				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetAgeList() Start");
				_log.Debug("-----------------------------------------");

				StringBuilder sbQuery = new StringBuilder();

				// 쿼리생성
                //sbQuery.Append("\n"
                //    + "-- 연령대 조회      \n"
                //    + "SELECT SummaryCode AS AgeCode    \n"
                //    + "      ,SummaryName AS AgeName    \n"
                //    + "      ,UpperCode       \n"
                //    + "      ,Level           \n"
                //    + "  FROM SummaryCode   with(nolock)    \n"	
                //    + " WHERE SummaryType = 3 -- 3:연령대\n"
                //    + "   AND SummaryCode < 9000        \n"
                //    + " ORDER BY SummaryCode  \n"
                //    );
                sbQuery.AppendLine();
                sbQuery.AppendLine("     SELECT	SMRY_COD	AS AgeCode      ");
	            sbQuery.AppendLine("         ,	SMRY_NM		AS AgeName      ");
                sbQuery.AppendLine("         ,	UPPER_COD	AS UpperCode    ");   
                sbQuery.AppendLine("         ,	LVL			AS \"Level\"    ");
                sbQuery.AppendLine("     FROM SMRY_COD                      ");
                sbQuery.AppendLine("     WHERE	SMRY_TYP = 3 -- 3:연령대    ");
	            sbQuery.AppendLine("         AND SMRY_COD < 9000            ");
                sbQuery.AppendLine("     ORDER BY SMRY_COD                  ");
       
				// __DEBUG__
				_log.Debug(sbQuery.ToString());
				// __DEBUG__
				
				// 쿼리실행
				DataSet ds = new DataSet();
				_db.ExecuteQuery(ds,sbQuery.ToString());

				// 결과 DataSet의 장르그룹모델에 복사
				targetingModel.AgeDataSet = ds.Copy();
				// 결과
				targetingModel.ResultCnt = Utility.GetDatasetCount(targetingModel.AgeDataSet);
				// 결과코드 셋트
				targetingModel.ResultCD = "0000";

				ds.Dispose();

				// __DEBUG__
				_log.Debug("<출력정보>");
				_log.Debug("ResultCnt:[" + targetingModel.ResultCnt + "]");
				// __DEBUG__

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetAgeList() End");
				_log.Debug("-----------------------------------------");
			}
			catch(Exception ex)
			{
				targetingModel.ResultCD = "3000";
				targetingModel.ResultDesc = "연령대 조회중 오류가 발생하였습니다";
				_log.Exception(ex);
			}
			finally
			{
				// 데이터베이스를  Close한다
				_db.Close();
			}

		}
		#endregion

        #region 셋탑모델별 조회 [E_05] ------------------------------------------------- OS구분 정보가 STM_COD에 추가됨에따라 폐기

        /// <summary>
        /// [추가]셋탑모델별 조회
        /// </summary>
        /// <param name="targetingModel"></param>
        public void GetStbList(HeaderModel header, TargetingModel targetingModel)
        {
            try
            {
                // 데이터베이스를 OPEN한다
                _db.Open();

                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "GetStbList() Start");
                _log.Debug("-----------------------------------------");

                StringBuilder sbQuery = new StringBuilder();

                // 쿼리생성
                sbQuery.Append("-- 셋탑모델목록 조회             \n");
                sbQuery.Append("SELECT StbModel                  \n");
                sbQuery.Append("      ,StbName                   \n");
                sbQuery.Append("  FROM StbType with(nolock)      \n");
                sbQuery.Append(" ORDER BY StbModel               \n");

                // __DEBUG__
                _log.Debug(sbQuery.ToString());
                // __DEBUG__

                // 쿼리실행
                DataSet ds = new DataSet();
                _db.ExecuteQuery(ds, sbQuery.ToString());

                // 결과 DataSet에 복사
                targetingModel.TargetingDataSet = ds.Copy();
                // 결과
                targetingModel.ResultCnt = Utility.GetDatasetCount(targetingModel.TargetingDataSet);
                // 결과코드 셋트
                targetingModel.ResultCD = "0000";

                ds.Dispose();

                // __DEBUG__
                _log.Debug("<출력정보>");
                _log.Debug("ResultCnt:[" + targetingModel.ResultCnt + "]");
                // __DEBUG__

                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "GetStbList() End");
                _log.Debug("-----------------------------------------");
            }
            catch (Exception ex)
            {
                targetingModel.ResultCD = "3000";
                targetingModel.ResultDesc = "셋탑목록 조회중 오류가 발생하였습니다";
                _log.Exception(ex);
            }
            finally
            {
                // 데이터베이스를  Close한다
                _db.Close();
            }
        }

        #endregion

        #region 고객군타겟팅 조회 ----------------------------------------------------- 기능 제외 임시 폐기
        /// <summary>
        /// 연령대 조회
        /// </summary>
        /// <param name="targetingModel"></param>
        public void GetTargetingCollectionList(HeaderModel header, TargetingModel targetingModel)
        {
            try
            {
                // 데이터베이스를 OPEN한다
                _db.Open();


                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "GetTagetingCollectionList() Start");
                _log.Debug("-----------------------------------------");

                StringBuilder sbQuery = new StringBuilder();

                // 쿼리생성
                sbQuery.Append("\n"
                    + "-- 고객군타겟팅 조회      \n"
                    + "SELECT 'False' AS CheckYn            \n"
                    + "		  ,A.CollectionCode, B.CollectionName, A.Sign   \n"
                    + "  FROM TargetingCollection A with(noLock) LEFT JOIN Collection B with(nolock) ON (A.CollectionCode = B.CollectionCode)   \n"
                    + " WHERE A.ItemNo = " + targetingModel.ItemNo + "   \n"
                    + "   AND A.SetType = '1' -- 1:일반 2:홈광고\n"        
                    + " ORDER BY CollectionCode  \n"
                    );

                // __DEBUG__
                _log.Debug(sbQuery.ToString());
                // __DEBUG__

                // 쿼리실행
                DataSet ds = new DataSet();
                _db.ExecuteQuery(ds, sbQuery.ToString());

                // 결과 DataSet의 장르그룹모델에 복사
                targetingModel.TargetingCollectionDataSet = ds.Copy();
                // 결과
                targetingModel.ResultCnt = Utility.GetDatasetCount(targetingModel.TargetingCollectionDataSet);
                // 결과코드 셋트
                targetingModel.ResultCD = "0000";

                ds.Dispose();

                // __DEBUG__
                _log.Debug("<출력정보>");
                _log.Debug("ResultCnt:[" + targetingModel.ResultCnt + "]");
                // __DEBUG__

                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "GetTagetingCollectionList() End");
                _log.Debug("-----------------------------------------");
            }
            catch (Exception ex)
            {
                targetingModel.ResultCD = "3000";
                targetingModel.ResultDesc = "고객군타겟팅 조회중 오류가 발생하였습니다";
                _log.Exception(ex);
            }
            finally
            {
                // 데이터베이스를  Close한다
                _db.Close();
            }

        }
        #endregion

        #region 고객군타겟팅 추가 ----------------------------------------------------- 기능 제외 임시 폐기
        /// <summary>
        /// 비율 등록
        /// </summary>
        /// <param name="header"></param>
        /// <param name="ratioModel"></param>
        public void SetTargetingCollectionAdd(HeaderModel header, TargetingModel targetingModel)
        {
            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "." + MethodBase.GetCurrentMethod().Name + " Start");
                _log.Debug("-----------------------------------------");

                // 데이터베이스를 OPEN한다
                _db.Open();

                StringBuilder sbQuery = new StringBuilder();

                int i = 0;
                int rc = 0;
                SqlParameter[] sqlParams = new SqlParameter[3];

                sbQuery.Append(""
                    + "INSERT INTO TargetingCollection ( \n"
                    + "      SetType                  \n"
                    + "		,ItemNo                   \n"
                    + "		,CollectionCode           \n"
                    + "     ,Sign )                   \n"
                    + " VALUES(                       \n"
                    + "       '1'  -- 1:일반 2:홈광고 \n"
                    + "      ,@ItemNo				  \n"
                    + "      ,@CollectionCode	      \n"
                    + "		 ,@Sign	)				  \n"
                    );

                sqlParams[i++] = new SqlParameter("@ItemNo", SqlDbType.Int);
                sqlParams[i++] = new SqlParameter("@CollectionCode", SqlDbType.Int);
				sqlParams[i++] = new SqlParameter("@Sign", SqlDbType.Char, 1);

                i = 0;
                sqlParams[i++].Value = Convert.ToInt32(targetingModel.ItemNo);
                sqlParams[i++].Value = Convert.ToInt32(targetingModel.CollectionCode);
				sqlParams[i++].Value = targetingModel.TgtCollectionYn.Trim();

                _log.Debug("ItemNo			[" + targetingModel.ItemNo + "]");
                _log.Debug("CollectionCode	[" + targetingModel.CollectionCode + "]");
				_log.Debug("CollType		[" + targetingModel.TgtCollectionYn + "]");

                _log.Debug(sbQuery.ToString());

                // 쿼리실행
                try
                {
                    _db.BeginTran();
                    rc = _db.ExecuteNonQueryParams(sbQuery.ToString(), sqlParams);
                    _db.CommitTran();

                }
                catch (Exception ex)
                {
                    _db.RollbackTran();
                    throw ex;
                }

                targetingModel.ResultCD = "0000";  // 정상

                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "." + MethodBase.GetCurrentMethod().Name + " End");
                _log.Debug("-----------------------------------------");
            }
            catch (Exception ex)
            {
                targetingModel.ResultCD = "3101";
                targetingModel.ResultDesc = "고객군타겟팅 추가중 오류가 발생하였습니다";
                _log.Exception(ex);
            }
            finally
            {
                // 데이터베이스를  Close한다
                _db.Close();
            }
        }
        #endregion

        #region 고객군타겟팅 삭제 ----------------------------------------------------- 기능 제외 임시 폐기
        /// <summary>
        /// 비율 등록
        /// </summary>
        /// <param name="header"></param>
        /// <param name="ratioModel"></param>
        public void SetTargetingCollectionDelete(HeaderModel header, TargetingModel targetingModel)
        {
            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "." + MethodBase.GetCurrentMethod().Name + " Start");
                _log.Debug("-----------------------------------------");

                // 데이터베이스를 OPEN한다
                _db.Open();

                StringBuilder sbQuery = new StringBuilder();

                int i = 0;
                int rc = 0;
                SqlParameter[] sqlParams = new SqlParameter[2];

                sbQuery.Append(""
                    + "DELETE FROM TargetingCollection         \n"
                    + "	WHERE SetType        = '1'  -- 1:일반 2:홈광고  \n"
                    + "   AND ItemNo         = @ItemNo         \n"
                    + "	  AND CollectionCode = @CollectionCode \n"
                    );

                sqlParams[i++] = new SqlParameter("@ItemNo", SqlDbType.Int);
                sqlParams[i++] = new SqlParameter("@CollectionCode", SqlDbType.Int);

                i = 0;
                sqlParams[i++].Value = Convert.ToInt32(targetingModel.ItemNo);
                sqlParams[i++].Value = Convert.ToInt32(targetingModel.CollectionCode);

                _log.Debug("ItemNo        :[" + targetingModel.ItemNo + "]");
                _log.Debug("CollectionCode:[" + targetingModel.CollectionCode + "]");

                _log.Debug(sbQuery.ToString());

                // 쿼리실행
                try
                {
                    _db.BeginTran();
                    rc = _db.ExecuteNonQueryParams(sbQuery.ToString(), sqlParams);
                    _db.CommitTran();

                }
                catch (Exception ex)
                {
                    _db.RollbackTran();
                    throw ex;
                }

                targetingModel.ResultCD = "0000";  // 정상

                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "." + MethodBase.GetCurrentMethod().Name + " End");
                _log.Debug("-----------------------------------------");
            }
            catch (Exception ex)
            {
                targetingModel.ResultCD = "3101";
                targetingModel.ResultDesc = "고객군타겟팅 추가중 오류가 발생하였습니다";
                _log.Exception(ex);
            }
            finally
            {
                // 데이터베이스를  Close한다
                _db.Close();
            }
        }


        #endregion

        #region 프로파일 타겟팅 추가 -------------------------------------------------- 기능 제외 임시 폐기

        public void SetTargetingProfileAdd(HeaderModel header, TargetingModel targetingModel)
        {
            try
            {
                // 데이터베이스를 OPEN한다
                _db.Open();

                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + ".SetTargetingProfileAdd() Start");
                _log.Debug("-----------------------------------------");

                _log.Debug("ItemNo         :[" + targetingModel.ItemNo         + "]");	
                _log.Debug("TgtProfile     :[" + targetingModel.TgtProfile     + "]");
                _log.Debug("TgtReliability :[" + targetingModel.TgtReliablilty + "]");

                int i = 0;
                int rc = 0;
                StringBuilder sbQuery;
                SqlParameter[] sqlParams = new SqlParameter[3];

                i = 0;
                sqlParams[i++] = new SqlParameter("@ItemNo", SqlDbType.Int); 
                sqlParams[i++] = new SqlParameter("@TgtProfile", SqlDbType.VarChar, 51);   // [E_07] 프로파일 타겟팅 연령
                sqlParams[i++] = new SqlParameter("@TgtReliability", SqlDbType.Int);       // [E_07] 프로파일 타겟팅 신뢰도

                i = 0;
                sqlParams[i++].Value = targetingModel.ItemNo;
                sqlParams[i++].Value = targetingModel.TgtProfile;       // [E_07] 프로파일 타겟팅 연령
                sqlParams[i++].Value = targetingModel.TgtReliablilty;   // [E_07] 프로파일 타겟팅 신뢰도

                // 쿼리실행
                try
                {
                    _db.BeginTran();

                    sbQuery = new StringBuilder();
                    sbQuery.Append(" UPDATE Targeting							\n");
                    sbQuery.Append("    SET TgtProfile      = @TgtProfile       \n");   // [E_07] 프로파일 타겟팅 연령대
                    sbQuery.Append("       ,TgtReliability  = @TgtReliability   \n");   // [E_07] 프로파일 타겟팅 신뢰도
                    sbQuery.Append("  WHERE ItemNo          = @ItemNo			\n");

                    rc = _db.ExecuteNonQueryParams(sbQuery.ToString(), sqlParams);

                    _log.Debug(sbQuery.ToString());
                    _db.CommitTran();

                    // __MESSAGE__
                    _log.Message("광고 타겟팅 프로파일타겟팅 수정:[" + targetingModel.ItemNo + "] " + targetingModel.TgtProfile + ", " + targetingModel.TgtReliablilty + " 등록자:[" + header.UserID + "]");

                }
                catch (Exception ex)
                {
                    _db.RollbackTran();
                    throw ex;
                }

                targetingModel.ResultCD = "0000";  // 정상

                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "SetTargetingProfileAdd() End");
                _log.Debug("-----------------------------------------");
            }
            catch (Exception ex)
            {
                targetingModel.ResultCD = "3101";
                targetingModel.ResultDesc = "광고 프로파일 타겟팅 수정 중 오류가 발생하였습니다";
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