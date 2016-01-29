
/*
 * -------------------------------------------------------
 * Class Name: TargetingHomeBiz
 * 주요기능  : 홈 타겟팅 처리 로직
 * 작성자    : 모름
 * 작성일    : 모름
 * 특이사항  : 지역정보 확장을 위해 기능 추가 -bae
 * -------------------------------------------------------
 * [수정사항]
 * -------------------------------------------------------
 * 수정코드  : [E_01]
 * 수정자    : bae
 * 수정일    : 2010.07.29
 * 수정부분  : 
 *             - GetRegionList(..)
 * 수정내용  : 
 *            - 지역정보 3단계 구조 확장으로 질의문 수정
 *              
 * --------------------------------------------------------
 * 수정코드  : [E_02]
 * 수정자    : 김보배
 * 수정일    : 2012.01.17
 * 수정부분  : 
 *             - GetRegionList(..)
 * 수정내용  : 
 *            - 지역정보 컨트롤 변경으로 수정
 *   
 * --------------------------------------------------------
 * 수정코드  : [E_03]
 * 수정자    : H.J.LEE
 * 수정일    : 2014.08.19
 * 수정부분  :
 *			  - GetAgeList(..)
 * 수정내용  : 
 *            - DB 이중화 작업으로 HanaTV , Summary로 분리됨
 *            - Summary의 테이블을 보는 함수가 있어서 해당
 *            함수만 Summary를 보도록 수정함
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

namespace AdManagerWebService.Target
{
	/// <summary>
	/// TargetingHomeBiz에 대한 요약 설명입니다.
	/// </summary>
	public class TargetingHomeBiz : BaseBiz
	{

		#region  생성자
		public TargetingHomeBiz() : base(FrameSystem.connDbString)
		{
			_log = FrameSystem.oLog;
		}
		#endregion

		#region 광고내역 조회
		/// <summary>
		/// 홈광고 목록조회,상단 그리드용
		/// </summary>
		/// <param name="targetingHomeModel"></param>
		public void GetTargetingList(HeaderModel header, TargetingHomeModel targetingHomeModel)
		{
			bool isState = false;

			try
			{
				// 데이터베이스를 OPEN한다
				_db.Open();

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetTargetingList() Start");
				_log.Debug("-----------------------------------------");

				// __DEBUG__
				_log.Debug("<입력정보>");
				_log.Debug("SearchKey            :[" + targetingHomeModel.SearchKey            + "]");		// 검색어
				_log.Debug("SearchMediaCode	     :[" + targetingHomeModel.SearchMediaCode	   + "]");		// 검색 매체
				_log.Debug("SearchRapCode        :[" + targetingHomeModel.SearchRapCode        + "]");		// 검색 랩
				_log.Debug("SearchAgencyCode     :[" + targetingHomeModel.SearchAgencyCode     + "]");		// 검색 대행사
				_log.Debug("SearchAdvertiserCode :[" + targetingHomeModel.SearchAdvertiserCode + "]");		// 검색 광고주
				_log.Debug("SearchContractState  :[" + targetingHomeModel.SearchContractState  + "]");		// 검색 계약상태
				_log.Debug("SearchAdType         :[" + targetingHomeModel.SearchAdType         + "]");		// 검색 광고종류
				_log.Debug("SearchchkAdState_20  :[" + targetingHomeModel.SearchchkAdState_20  + "]");		// 검색 광고상태 : 편성
				_log.Debug("SearchchkAdState_30  :[" + targetingHomeModel.SearchchkAdState_30  + "]");		// 검색 광고상태 : 중지	
				_log.Debug("SearchchkAdState_40  :[" + targetingHomeModel.SearchchkAdState_40  + "]");		// 검색 광고상태 : 종료           
				_log.Debug("SearchchkTimeY  :[" + targetingHomeModel.SearchchkTimeY  + "]");		// 검색 타겟팅여부 : 설정           
				_log.Debug("SearchchkTimeN  :[" + targetingHomeModel.SearchchkTimeN  + "]");		// 검색 타겟팅여부 : 미설정           

				// __DEBUG__

				StringBuilder sbQuery = new StringBuilder();

				// 쿼리생성
				sbQuery.Append("\n"
					+ "SELECT B.ItemNo                      \n"
					+ "      ,B.ItemName                    \n"
					+ "      ,C.ContractName                \n"
					+ "      ,D.AdvertiserName              \n"
					+ "      ,E.CodeName as ContStateName   \n"
					+ "      ,B.ExcuteStartDay              \n"
					+ "      ,B.ExcuteEndDay                \n"
					+ "      ,B.RealEndDay                  \n"
					+ "      ,F.CodeName as AdTypeName      \n"
					+ "      ,G.CodeName as AdStatename     \n"
					+ "      ,I.CodeName as FileStateName   \n"
					+ "      ,C.ContractAmt                 \n"
					+ "      ,H.PriorityCd                  \n"
					+ "      ,B.MediaCode                   \n"
					+ "      ,ISNULL(H.ItemNo,0)  As TgtItemNo \n"
					+ "      ,H.ContractAmt AS TgtAmount    \n"
					+ "		 ,isNull(H.TgtCollectionYn,'N')	as TgtCollection \n"
					+ "		 ,H.TgtRegion1Yn \n"
					+ "		 ,H.TgtTimeYn \n"
					+ "		 ,H.TgtWeekYn \n"
					+ "  FROM ContractItem B with(nolock)  \n"
					+ "       INNER JOIN Contract   C with(nolock) ON (B.MediaCode      = C.MediaCode AND B.RapCode = C.RapCode AND B.AgencyCode = C.AgencyCode AND B.AdvertiserCode = C.AdvertiserCode AND B.ContractSeq = C.ContractSeq) \n"
					+ "       LEFT  JOIN Advertiser D with(nolock) ON (B.AdvertiserCode = D.AdvertiserCode)      \n"
					+ "       LEFT  JOIN SystemCode E with(nolock) ON (C.State          = E.Code AND E.Section = '23')  \n"	// 23 : 계약상태
					+ "       LEFT  JOIN SystemCode F with(nolock) ON (B.AdType         = F.Code AND F.Section = '26')  \n"	// 26 : 광고종류
					+ "       LEFT  JOIN SystemCode G with(nolock) ON (B.AdState        = G.Code AND G.Section = '25')  \n"	// 25 : 광고상태
					+ "       LEFT  JOIN SystemCode I with(nolock) ON (B.FileState      = I.Code AND I.Section = '31')  \n"	// 25 : 광고상태
					+ "       LEFT  JOIN TargetingHome  H with(nolock) ON (B.ItemNo         = H.ItemNo)  \n"	
					+ " WHERE 1 = 1   \n"
					+ "   AND B.AdType != '13'" // 광고종류  10~19:필수광고
					);

				// 매체을 선택했으면
				if(targetingHomeModel.SearchMediaCode.Trim().Length > 0 && !targetingHomeModel.SearchMediaCode.Trim().Equals("00"))
				{
					sbQuery.Append(" AND B.MediaCode  = " + targetingHomeModel.SearchMediaCode.Trim() + " \n");
				}	
				
				// 랩사를 선택했으면
				if(targetingHomeModel.SearchRapCode.Trim().Length > 0 && !targetingHomeModel.SearchRapCode.Trim().Equals("00"))
				{
					sbQuery.Append(" AND B.RapCode  = " + targetingHomeModel.SearchRapCode.Trim() + " \n");
				}	

				// 대행사를 선택했으면
				if(targetingHomeModel.SearchAgencyCode.Trim().Length > 0 && !targetingHomeModel.SearchAgencyCode.Trim().Equals("00"))
				{
					sbQuery.Append(" AND B.AgencyCode  = " + targetingHomeModel.SearchAgencyCode.Trim() + " \n");
				}	

				// 광고주를 선택했으면
				if(targetingHomeModel.SearchAdvertiserCode.Trim().Length > 0 && !targetingHomeModel.SearchAdvertiserCode.Trim().Equals("00"))
				{
					sbQuery.Append(" AND B.AdvertiserCode  = " + targetingHomeModel.SearchAdvertiserCode.Trim() + " \n");
				}	

				// 계약상태를 선택했으면
				if(targetingHomeModel.SearchContractState.Trim().Length > 0 && !targetingHomeModel.SearchContractState.Trim().Equals("00"))
				{
					sbQuery.Append(" AND C.State  = '" + targetingHomeModel.SearchContractState.Trim() + "' \n");
				}	

				// 광고종류를 선택했으면
				if(targetingHomeModel.SearchAdType.Trim().Length > 0 && !targetingHomeModel.SearchAdType.Trim().Equals("00"))
				{
					sbQuery.Append(" AND B.AdType  = '" + targetingHomeModel.SearchAdType.Trim() + "' \n");
				}

				// 광고상태 선택에 따라
				// 광고상태는 20:편성 과 40:종료 사이에 있는 것만 조회한다.
				sbQuery.Append(" AND B.AdState >= '20' AND B.AdState <= '40' \n");

				// 광고상태 편성
				if(targetingHomeModel.SearchchkAdState_20.Trim().Length > 0 && targetingHomeModel.SearchchkAdState_20.Trim().Equals("Y"))
				{
					sbQuery.Append(" AND ( B.AdState  = '20' \n");
					isState = true;
				}	
				// 광고상태 중지
				if(targetingHomeModel.SearchchkAdState_30.Trim().Length > 0 && targetingHomeModel.SearchchkAdState_30.Trim().Equals("Y"))
				{
					if(isState) sbQuery.Append(" OR ");
					else sbQuery.Append(" AND ( ");
					sbQuery.Append(" B.AdState  = '30' \n");
					isState = true;
				}	
				// 광고상태 종료
				if(targetingHomeModel.SearchchkAdState_40.Trim().Length > 0 && targetingHomeModel.SearchchkAdState_40.Trim().Equals("Y"))
				{
					if(isState) sbQuery.Append(" OR ");
					else sbQuery.Append(" AND ( ");
					sbQuery.Append(" B.AdState  = '40' \n");
					isState = true;
				}	

				if(isState) sbQuery.Append(" ) \n");
				
				// 검색어가 있으면
				if (targetingHomeModel.SearchKey.Trim().Length > 0)
				{
					// 여러컬럼에 대하여 LIKE 검색을 한다.
					sbQuery.Append("\n"
						+ "  AND ( B.ItemNo         LIKE '%" + targetingHomeModel.SearchKey.Trim() + "%' \n"
						+ "     OR B.ItemName       LIKE '%" + targetingHomeModel.SearchKey.Trim() + "%' \n"
						+ "     OR C.ContractName   LIKE '%" + targetingHomeModel.SearchKey.Trim() + "%' \n"
						+ "     OR D.AdvertiserName LIKE '%" + targetingHomeModel.SearchKey.Trim() + "%' \n"
						+ "  ) \n"
						);
				}

				// 타겟팅여부-설정를 선택했으면
				if(targetingHomeModel.SearchchkTimeY.Trim().Length > 0 && targetingHomeModel.SearchchkTimeY.Trim().Equals("Y") && targetingHomeModel.SearchchkTimeN.Trim().Equals(""))
				{
					sbQuery.Append(" AND H.TgtTimeYn IS NOT NULL \n");
				}

				// 타겟팅여부-미설정를 선택했으면
				if(targetingHomeModel.SearchchkTimeN.Trim().Length > 0 && targetingHomeModel.SearchchkTimeN.Trim().Equals("Y") && targetingHomeModel.SearchchkTimeY.Trim().Equals(""))
				{
					sbQuery.Append(" AND H.TgtTimeYn IS NULL \n");
				}
				// 타겟팅여부-설정,미설정를 선택했으면..전체를 보여준다.
				if(targetingHomeModel.SearchchkTimeY.Trim().Length > 0 && targetingHomeModel.SearchchkTimeN.Trim().Length > 0 && targetingHomeModel.SearchchkTimeY.Trim().Equals("Y") && targetingHomeModel.SearchchkTimeN.Trim().Equals("Y"))
				{					
				}
       
				sbQuery.Append("  ORDER BY B.ItemNo DESC ");

				// __DEBUG__
				_log.Debug(sbQuery.ToString());
				// __DEBUG__
				
				// 쿼리실행
				DataSet ds = new DataSet();
				_db.ExecuteQuery(ds,sbQuery.ToString());

				// 결과 DataSet의 장르그룹모델에 복사
				targetingHomeModel.TargetingDataSet = ds.Copy();
				// 결과
				targetingHomeModel.ResultCnt = Utility.GetDatasetCount(targetingHomeModel.TargetingDataSet);
				// 결과코드 셋트
				targetingHomeModel.ResultCD = "0000";

				ds.Dispose();

				// __DEBUG__
				_log.Debug("<출력정보>");
				_log.Debug("ResultCnt:[" + targetingHomeModel.ResultCnt + "]");
				// __DEBUG__

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetTargetingList() End");
				_log.Debug("-----------------------------------------");
			}
			catch(Exception ex)
			{
				targetingHomeModel.ResultCD = "3000";
				targetingHomeModel.ResultDesc = "지정광고편성현황 조회중 오류가 발생하였습니다";
				_log.Exception(ex);
			}
			finally
			{
				// 데이터베이스를  Close한다
				_db.Close();
			}

		}
        /// <summary>
        /// 홈광고 목록조회,상단 그리드용
        /// </summary>
        /// <param name="targetingHomeModel"></param>
        public void GetTargetingList2(HeaderModel header, TargetingHomeModel targetingHomeModel)
        {
            bool isState = false;

            try
            {
                // 데이터베이스를 OPEN한다
                _db.Open();

                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "GetTargetingList() Start");
                _log.Debug("-----------------------------------------");

                // __DEBUG__
                _log.Debug("<입력정보>");
                _log.Debug("SearchKey            :[" + targetingHomeModel.SearchKey + "]");		// 검색어
                _log.Debug("SearchMediaCode	     :[" + targetingHomeModel.SearchMediaCode + "]");		// 검색 매체
                _log.Debug("SearchRapCode        :[" + targetingHomeModel.SearchRapCode + "]");		// 검색 랩
                _log.Debug("SearchAgencyCode     :[" + targetingHomeModel.SearchAgencyCode + "]");		// 검색 대행사
                _log.Debug("SearchAdvertiserCode :[" + targetingHomeModel.SearchAdvertiserCode + "]");		// 검색 광고주
                _log.Debug("SearchContractState  :[" + targetingHomeModel.SearchContractState + "]");		// 검색 계약상태
                _log.Debug("SearchAdType         :[" + targetingHomeModel.SearchAdType + "]");		// 검색 광고종류
                _log.Debug("SearchchkAdState_20  :[" + targetingHomeModel.SearchchkAdState_20 + "]");		// 검색 광고상태 : 편성
                _log.Debug("SearchchkAdState_30  :[" + targetingHomeModel.SearchchkAdState_30 + "]");		// 검색 광고상태 : 중지	
                _log.Debug("SearchchkAdState_40  :[" + targetingHomeModel.SearchchkAdState_40 + "]");		// 검색 광고상태 : 종료           
                _log.Debug("SearchchkTimeY  :[" + targetingHomeModel.SearchchkTimeY + "]");		// 검색 타겟팅여부 : 설정           
                _log.Debug("SearchchkTimeN  :[" + targetingHomeModel.SearchchkTimeN + "]");		// 검색 타겟팅여부 : 미설정           

                // __DEBUG__

                StringBuilder sbQuery = new StringBuilder();

                // 쿼리생성
                sbQuery.Append("\n"
                    + "SELECT B.ItemNo                      \n"
                    + "      ,B.ItemName                    \n"
                    + "      ,C.ContractName                \n"
                    + "      ,D.AdvertiserName              \n"
                    + "      ,E.CodeName as ContStateName   \n"
                    + "      ,B.ExcuteStartDay              \n"
                    + "      ,B.ExcuteEndDay                \n"
                    + "      ,B.RealEndDay                  \n"
                    + "      ,F.CodeName as AdTypeName      \n"
                    + "      ,G.CodeName as AdStatename     \n"
                    + "      ,I.CodeName as FileStateName   \n"
                    + "      ,C.ContractAmt                 \n"
                    + "      ,H.PriorityCd                  \n"
                    + "      ,B.MediaCode                   \n"
                    + "      ,ISNULL(H.ItemNo,0)  As TgtItemNo \n"
                    + "      ,H.ContractAmt AS TgtAmount    \n"
                    + "		 ,isNull(H.TgtCollectionYn,'N')	as TgtCollection \n"
                    + "		 ,H.TgtRegion1Yn \n"
                    + "		 ,H.TgtTimeYn \n"
                    + "		 ,H.TgtWeekYn \n"
                    + "      ,H.TgtStbTypeYn  \n"
                    + "  FROM ContractItem B with(nolock)  \n"
                    + "       INNER JOIN Contract   C with(nolock) ON (B.MediaCode      = C.MediaCode AND B.RapCode = C.RapCode AND B.AgencyCode = C.AgencyCode AND B.AdvertiserCode = C.AdvertiserCode AND B.ContractSeq = C.ContractSeq) \n"
                    + "       LEFT  JOIN Advertiser D with(nolock) ON (B.AdvertiserCode = D.AdvertiserCode)      \n"
                    + "       LEFT  JOIN SystemCode E with(nolock) ON (C.State          = E.Code AND E.Section = '23')  \n"	// 23 : 계약상태
                    + "       LEFT  JOIN SystemCode F with(nolock) ON (B.AdType         = F.Code AND F.Section = '26')  \n"	// 26 : 광고종류
                    + "       LEFT  JOIN SystemCode G with(nolock) ON (B.AdState        = G.Code AND G.Section = '25')  \n"	// 25 : 광고상태
                    + "       LEFT  JOIN SystemCode I with(nolock) ON (B.FileState      = I.Code AND I.Section = '31')  \n"	// 25 : 광고상태
                    + "       LEFT  JOIN TargetingHome  H with(nolock) ON (B.ItemNo         = H.ItemNo)  \n"
                    + " WHERE 1 = 1   \n"
                    + "   AND B.AdType != '13'" // 광고종류  10~19:필수광고
                    );

                // 매체을 선택했으면
                if (targetingHomeModel.SearchMediaCode.Trim().Length > 0 && !targetingHomeModel.SearchMediaCode.Trim().Equals("00"))
                {
                    sbQuery.Append(" AND B.MediaCode  = " + targetingHomeModel.SearchMediaCode.Trim() + " \n");
                }

                // 랩사를 선택했으면
                if (targetingHomeModel.SearchRapCode.Trim().Length > 0 && !targetingHomeModel.SearchRapCode.Trim().Equals("00"))
                {
                    sbQuery.Append(" AND B.RapCode  = " + targetingHomeModel.SearchRapCode.Trim() + " \n");
                }

                // 대행사를 선택했으면
                if (targetingHomeModel.SearchAgencyCode.Trim().Length > 0 && !targetingHomeModel.SearchAgencyCode.Trim().Equals("00"))
                {
                    sbQuery.Append(" AND B.AgencyCode  = " + targetingHomeModel.SearchAgencyCode.Trim() + " \n");
                }

                // 광고주를 선택했으면
                if (targetingHomeModel.SearchAdvertiserCode.Trim().Length > 0 && !targetingHomeModel.SearchAdvertiserCode.Trim().Equals("00"))
                {
                    sbQuery.Append(" AND B.AdvertiserCode  = " + targetingHomeModel.SearchAdvertiserCode.Trim() + " \n");
                }

                // 계약상태를 선택했으면
                if (targetingHomeModel.SearchContractState.Trim().Length > 0 && !targetingHomeModel.SearchContractState.Trim().Equals("00"))
                {
                    sbQuery.Append(" AND C.State  = '" + targetingHomeModel.SearchContractState.Trim() + "' \n");
                }

                // 광고종류를 선택했으면
                if (targetingHomeModel.SearchAdType.Trim().Length > 0 && !targetingHomeModel.SearchAdType.Trim().Equals("00"))
                {
                    sbQuery.Append(" AND B.AdType  = '" + targetingHomeModel.SearchAdType.Trim() + "' \n");
                }

                // 광고상태 선택에 따라
                // 광고상태는 20:편성 과 40:종료 사이에 있는 것만 조회한다.
                sbQuery.Append(" AND B.AdState >= '20' AND B.AdState <= '40' \n");

                // 광고상태 편성
                if (targetingHomeModel.SearchchkAdState_20.Trim().Length > 0 && targetingHomeModel.SearchchkAdState_20.Trim().Equals("Y"))
                {
                    sbQuery.Append(" AND ( B.AdState  = '20' \n");
                    isState = true;
                }
                // 광고상태 중지
                if (targetingHomeModel.SearchchkAdState_30.Trim().Length > 0 && targetingHomeModel.SearchchkAdState_30.Trim().Equals("Y"))
                {
                    if (isState) sbQuery.Append(" OR ");
                    else sbQuery.Append(" AND ( ");
                    sbQuery.Append(" B.AdState  = '30' \n");
                    isState = true;
                }
                // 광고상태 종료
                if (targetingHomeModel.SearchchkAdState_40.Trim().Length > 0 && targetingHomeModel.SearchchkAdState_40.Trim().Equals("Y"))
                {
                    if (isState) sbQuery.Append(" OR ");
                    else sbQuery.Append(" AND ( ");
                    sbQuery.Append(" B.AdState  = '40' \n");
                    isState = true;
                }

                if (isState) sbQuery.Append(" ) \n");

                // 검색어가 있으면
                if (targetingHomeModel.SearchKey.Trim().Length > 0)
                {
                    // 여러컬럼에 대하여 LIKE 검색을 한다.
                    sbQuery.Append("\n"
                        + "  AND ( B.ItemNo         LIKE '%" + targetingHomeModel.SearchKey.Trim() + "%' \n"
                        + "     OR B.ItemName       LIKE '%" + targetingHomeModel.SearchKey.Trim() + "%' \n"
                        + "     OR C.ContractName   LIKE '%" + targetingHomeModel.SearchKey.Trim() + "%' \n"
                        + "     OR D.AdvertiserName LIKE '%" + targetingHomeModel.SearchKey.Trim() + "%' \n"
                        + "  ) \n"
                        );
                }

                // 타겟팅여부-설정를 선택했으면
                if (targetingHomeModel.SearchchkTimeY.Trim().Length > 0 && targetingHomeModel.SearchchkTimeY.Trim().Equals("Y") && targetingHomeModel.SearchchkTimeN.Trim().Equals(""))
                {
                    sbQuery.Append(" AND H.TgtTimeYn IS NOT NULL \n");
                }

                // 타겟팅여부-미설정를 선택했으면
                if (targetingHomeModel.SearchchkTimeN.Trim().Length > 0 && targetingHomeModel.SearchchkTimeN.Trim().Equals("Y") && targetingHomeModel.SearchchkTimeY.Trim().Equals(""))
                {
                    sbQuery.Append(" AND H.TgtTimeYn IS NULL \n");
                }
                // 타겟팅여부-설정,미설정를 선택했으면..전체를 보여준다.
                if (targetingHomeModel.SearchchkTimeY.Trim().Length > 0 && targetingHomeModel.SearchchkTimeN.Trim().Length > 0 && targetingHomeModel.SearchchkTimeY.Trim().Equals("Y") && targetingHomeModel.SearchchkTimeN.Trim().Equals("Y"))
                {
                }

                sbQuery.Append("  ORDER BY B.ItemNo DESC ");

                // __DEBUG__
                _log.Debug(sbQuery.ToString());
                // __DEBUG__

                // 쿼리실행
                DataSet ds = new DataSet();
                _db.ExecuteQuery(ds, sbQuery.ToString());

                // 결과 DataSet의 장르그룹모델에 복사
                targetingHomeModel.TargetingDataSet = ds.Copy();
                // 결과
                targetingHomeModel.ResultCnt = Utility.GetDatasetCount(targetingHomeModel.TargetingDataSet);
                // 결과코드 셋트
                targetingHomeModel.ResultCD = "0000";

                ds.Dispose();

                // __DEBUG__
                _log.Debug("<출력정보>");
                _log.Debug("ResultCnt:[" + targetingHomeModel.ResultCnt + "]");
                // __DEBUG__

                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "GetTargetingList() End");
                _log.Debug("-----------------------------------------");
            }
            catch (Exception ex)
            {
                targetingHomeModel.ResultCD = "3000";
                targetingHomeModel.ResultDesc = "지정광고편성현황 조회중 오류가 발생하였습니다";
                _log.Exception(ex);
            }
            finally
            {
                // 데이터베이스를  Close한다
                _db.Close();
            }

        }
		#endregion

		#region 타겟군목록조회

		/// <summary>
		/// 타겟군목록조회
		/// </summary>
		/// <param name="targetingHomeModel"></param>
		public void GetCollectionList(HeaderModel header, TargetingHomeModel targetingHomeModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetCollectionList() Start");
				_log.Debug("-----------------------------------------");

				// 데이터베이스를 OPEN한다
				_db.Open();

				// __DEBUG__
				_log.Debug("<입력정보>");
				_log.Debug("SearchKey      :[" + targetingHomeModel.SearchKey       + "]");				
				// __DEBUG__

				StringBuilder sbQuery = new StringBuilder();

				// 쿼리생성
				sbQuery.Append("\n"
					+ "SELECT 'False' AS CheckYn            \n"
					+ "		  ,A.CollectionCode, A.CollectionName, A.Comment  \n"								
					+ "       ,A.UseYn              \n"
					+ "       ,CASE A.UseYn WHEN 'Y' THEN '사용' WHEN 'N' THEN '사용안함' END AS UseYn_N  \n"
					+ "       ,convert(char(19), A.RegDt, 120) RegDt              \n"
					+ "       ,convert(char(19), A.ModDt, 120) ModDt              \n"					
					+ "       ,B.UserName RegName                                 \n"
					+ "       ,(Select count(*) from ClientList where CollectionCode = A.CollectionCode AND CollectionCode = A.CollectionCode) Cnt           \n"
					+ "  FROM Collection A LEFT JOIN SystemUser B with(NoLock) ON (A.RegId          = B.UserId)        \n"					
					+ " WHERE 1 = 1  \n"
                    + "   AND A.UseYn = 'Y' "
                    );
			
				// 검색어가 있으면
				if (targetingHomeModel.SearchKey.Trim().Length > 0)
				{
					// 여러컬럼에 대하여 LIKE 검색을 한다.
					sbQuery.Append(" AND ("
						+ "  A.CollectionName      LIKE '%" + targetingHomeModel.SearchKey.Trim() + "%' \n"	
						+ " OR A.Comment    LIKE '%" + targetingHomeModel.SearchKey.Trim() + "%' \n"
						+ " OR B.UserName    LIKE '%" + targetingHomeModel.SearchKey.Trim() + "%' \n"						
						+ " ) ");
				}
				
				sbQuery.Append(" ORDER BY A.CollectionCode desc \n");

				// __DEBUG__
				_log.Debug(sbQuery.ToString());
				// __DEBUG__
				
				// 쿼리실행
				DataSet ds = new DataSet();
				_db.ExecuteQuery(ds,sbQuery.ToString());

				// 결과 DataSet의 미디어렙모델에 복사
				targetingHomeModel.CollectionsDataSet = ds.Copy();
				// 결과
				targetingHomeModel.ResultCnt = Utility.GetDatasetCount(targetingHomeModel.CollectionsDataSet);
				// 결과코드 셋트
				targetingHomeModel.ResultCD = "0000";

				// __DEBUG__
				_log.Debug("<출력정보>");
				_log.Debug("ResultCnt:[" + targetingHomeModel.ResultCnt + "]");
				// __DEBUG__

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetCollectionList() End");
				_log.Debug("-----------------------------------------");
			}
			catch(Exception ex)
			{
				targetingHomeModel.ResultCD = "3000";
				targetingHomeModel.ResultDesc = "타겟고객군정보 조회중 오류가 발생하였습니다";
				_log.Exception(ex);
			}
			finally
			{
				// 데이터베이스를  Close한다
				_db.Close();			
			}		
		}
		#endregion

		#region 광고 타겟팅 상세 조회
		/// <summary>
		/// 광고 타겟팅 상세 조회
		/// </summary>
		/// <param name="targetingHomeModel"></param>
		public void GetTargetingDetail(HeaderModel header, TargetingHomeModel targetingHomeModel)
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
                _log.Debug("ItemNo :[" + targetingHomeModel.ItemNo + "]");		// 광고번호


                // __DEBUG__

                StringBuilder sbQuery = new StringBuilder();

                // 쿼리생성
                sbQuery.Append("\n"
                    + "SELECT ItemNo                     \n"
                    + "      ,ContractAmt                 \n"
                    + "      ,PriorityValue               \n"
                    + "      ,PriorityCd                  \n"
                    + "      ,AmtVariableHour             \n"
                    + "      ,AmtControlYn                \n"
                    + "      ,AmtControlRate              \n"
                    + "      ,TgtRegion1Yn                \n"
                    + "      ,TgtRegion1                  \n"
                    + "      ,TgtTimeYn                   \n"
                    + "      ,TgtTime                     \n"
                    + "      ,TgtAgeYn                    \n"
                    + "      ,TgtAge                      \n"
                    + "      ,TgtAgeBtnYn                 \n"
                    + "      ,TgtAgeBtnBegin              \n"
                    + "      ,TgtAgeBtnEnd                \n"
                    + "      ,TgtSexYn                    \n"
                    + "      ,TgtSexMan                   \n"
                    + "      ,TgtSexWoman                 \n"
                    + "      ,TgtWeekYn                   \n"
                    + "      ,TgtWeek                     \n"
                    + "      ,TgtCollectionYn                   \n"
                    + "      ,( select count(*) from  TargetingCollection x with(nolock) where x.ItemNo = y.ItemNo and x.SetType = '2') as TgtCollection    \n"
                    + "  FROM TargetingHome y with(nolock)     \n"
                    + " WHERE ItemNo = " + targetingHomeModel.ItemNo + "   \n"
                    );

                // __DEBUG__
                _log.Debug(sbQuery.ToString());
                // __DEBUG__

                // 쿼리실행
                DataSet ds = new DataSet();
                _db.ExecuteQuery(ds, sbQuery.ToString());

                // 결과 DataSet의 장르그룹모델에 복사
                targetingHomeModel.DetailDataSet = ds.Copy();
                // 결과
                targetingHomeModel.ResultCnt = Utility.GetDatasetCount(targetingHomeModel.DetailDataSet);
                // 결과코드 셋트
                targetingHomeModel.ResultCD = "0000";

                ds.Dispose();

                // __DEBUG__
                _log.Debug("<출력정보>");
                _log.Debug("ResultCnt:[" + targetingHomeModel.ResultCnt + "]");
                // __DEBUG__

                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "GetTargetingDetail() End");
                _log.Debug("-----------------------------------------");
			}
			catch(Exception ex)
			{
				targetingHomeModel.ResultCD = "3000";
				targetingHomeModel.ResultDesc = "광고 타겟팅 상세 조회중 오류가 발생하였습니다";
				_log.Exception(ex);
			}
			finally
			{
				// 데이터베이스를  Close한다
				_db.Close();
			}

		}

        /// <summary>
        /// 광고 타겟팅 상세 조회
        /// </summary>
        /// <param name="targetingHomeModel"></param>
        public void GetTargetingDetail2(HeaderModel header, TargetingHomeModel targetingHomeModel)
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
                _log.Debug("ItemNo :[" + targetingHomeModel.ItemNo + "]");		// 광고번호


                // __DEBUG__

                StringBuilder sbQuery = new StringBuilder();

                // 쿼리생성
                sbQuery.Append("\n"
                    + "SELECT ItemNo                     \n"
                    + "      ,ContractAmt                 \n"
                    + "      ,PriorityValue               \n"
                    + "      ,PriorityCd                  \n"
                    + "      ,AmtVariableHour             \n"
                    + "      ,AmtControlYn                \n"
                    + "      ,AmtControlRate              \n"
                    + "      ,TgtRegion1Yn                \n"
                    + "      ,TgtRegion1                  \n"
                    + "      ,TgtTimeYn                   \n"
                    + "      ,TgtTime                     \n"
                    + "      ,TgtAgeYn                    \n"
                    + "      ,TgtAge                      \n"
                    + "      ,TgtAgeBtnYn                 \n"
                    + "      ,TgtAgeBtnBegin              \n"
                    + "      ,TgtAgeBtnEnd                \n"
                    + "      ,TgtSexYn                    \n"
                    + "      ,TgtSexMan                   \n"
                    + "      ,TgtSexWoman                 \n"
                    + "      ,TgtWeekYn                   \n"
                    + "      ,TgtWeek                     \n"
                    + "      ,TgtCollectionYn             \n"
                    + "      ,( select count(*) from  TargetingCollection x with(nolock) where x.ItemNo = y.ItemNo and x.SetType = '2') as TgtCollection    \n"
                    + "      ,TgtStbTypeYn                \n"
                    + "      ,TgtStbType                  \n"
                    + "      ,TgtPocYn                    \n"
                    + "      ,TgtPoc       	              \n"
                    + "  FROM TargetingHome y with(nolock)     \n"
                    + " WHERE ItemNo = " + targetingHomeModel.ItemNo + "   \n"
                    );

                // __DEBUG__
                _log.Debug(sbQuery.ToString());
                // __DEBUG__

                // 쿼리실행
                DataSet ds = new DataSet();
                _db.ExecuteQuery(ds, sbQuery.ToString());

                // 결과 DataSet의 장르그룹모델에 복사
                targetingHomeModel.DetailDataSet = ds.Copy();
                // 결과
                targetingHomeModel.ResultCnt = Utility.GetDatasetCount(targetingHomeModel.DetailDataSet);
                // 결과코드 셋트
                targetingHomeModel.ResultCD = "0000";

                ds.Dispose();

                // __DEBUG__
                _log.Debug("<출력정보>");
                _log.Debug("ResultCnt:[" + targetingHomeModel.ResultCnt + "]");
                // __DEBUG__

                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "GetTargetingDetail() End");
                _log.Debug("-----------------------------------------");
            }
            catch (Exception ex)
            {
                targetingHomeModel.ResultCD = "3000";
                targetingHomeModel.ResultDesc = "광고 타겟팅 상세 조회중 오류가 발생하였습니다";
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
		public void SetTargetingDetailUpdate(HeaderModel header, TargetingHomeModel targetingHomeModel)
		{

			try
			{
				// 데이터베이스를 OPEN한다
				_db.Open();

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + ".SetTargetingDetailUpdate() Start");
				_log.Debug("-----------------------------------------");

				_log.Debug("ItemNo         :[" + targetingHomeModel.ItemNo         + "]");		
				_log.Debug("ItemName       :[" + targetingHomeModel.ItemName       + "]");		
				_log.Debug("ContractAmt    :[" + targetingHomeModel.ContractAmt    + "]");		
				_log.Debug("PriorityCd     :[" + targetingHomeModel.PriorityCd     + "]");		
				_log.Debug("AmtControlYn   :[" + targetingHomeModel.AmtControlYn   + "]");		
				_log.Debug("AmtControlRate :[" + targetingHomeModel.AmtControlRate + "]");		
				_log.Debug("TgtRegion1Yn   :[" + targetingHomeModel.TgtRegion1Yn   + "]");		
				_log.Debug("TgtRegion1     :[" + targetingHomeModel.TgtRegion1     + "]");		
				_log.Debug("TgtTimeYn      :[" + targetingHomeModel.TgtTimeYn      + "]");		
				_log.Debug("TgtTime        :[" + targetingHomeModel.TgtTime        + "]");		
				_log.Debug("TgtAgeYn       :[" + targetingHomeModel.TgtAgeYn       + "]");		
				_log.Debug("TgtAge         :[" + targetingHomeModel.TgtAge         + "]");		
				_log.Debug("TgtAgeBtnYn    :[" + targetingHomeModel.TgtAgeBtnYn    + "]");		
				_log.Debug("TgtAgeBtnBegin :[" + targetingHomeModel.TgtAgeBtnBegin + "]");		
				_log.Debug("TgtAgeBtnEnd   :[" + targetingHomeModel.TgtAgeBtnEnd   + "]");		
				_log.Debug("TgtSexYn       :[" + targetingHomeModel.TgtSexYn       + "]");		
				_log.Debug("TgtSexMan      :[" + targetingHomeModel.TgtSexMan      + "]");		
				_log.Debug("TgtSexWoman    :[" + targetingHomeModel.TgtSexWoman    + "]");		
//				_log.Debug("TgtRateYn      :[" + targetingHomeModel.TgtRateYn      + "]");		
//				_log.Debug("TgtRate        :[" + targetingHomeModel.TgtRate        + "]");		
				_log.Debug("TgtWeekYn      :[" + targetingHomeModel.TgtWeekYn      + "]");		
				_log.Debug("TgtWeek        :[" + targetingHomeModel.TgtWeek        + "]");		
				_log.Debug("TgtCollectionYn      :[" + targetingHomeModel.TgtCollectionYn      + "]");		
                //_log.Debug("TgtCollection        :[" + targetingHomeModel.TgtCollection        + "]");	
                _log.Debug("TgtStbYn        :[" + targetingHomeModel.TgtStbModelYn  + "]");
                _log.Debug("TgtStb          :[" + targetingHomeModel.TgtStbModel    + "]");
                _log.Debug("TgtPocYn        :[" + targetingHomeModel.TgtPocYn       + "]");
                _log.Debug("TgtPoc          :[" + targetingHomeModel.TgtPoc         + "]");	

				int i = 0;
				int rc = 0;
				StringBuilder  sbQuery;
				SqlParameter[] sqlParams = new SqlParameter[24];
	            
				i = 0;
				sqlParams[i++] = new SqlParameter("@ItemNo"          , SqlDbType.Int        );
				sqlParams[i++] = new SqlParameter("@ContractAmt"     , SqlDbType.Decimal    );
				sqlParams[i++] = new SqlParameter("@PriorityCd"      , SqlDbType.TinyInt    );
				sqlParams[i++] = new SqlParameter("@AmtControlYn"    , SqlDbType.Char     ,1);
				sqlParams[i++] = new SqlParameter("@AmtControlRate"  , SqlDbType.SmallInt   );
				sqlParams[i++] = new SqlParameter("@TgtRegion1Yn"    , SqlDbType.Char     ,1);
				//sqlParams[i++] = new SqlParameter("@TgtRegion1"      , SqlDbType.VarChar,512);
				sqlParams[i++] = new SqlParameter("@TgtRegion1"      , SqlDbType.VarChar,2000); // [E_01] 지역정보 확대로 인한 Length 확장
                sqlParams[i++] = new SqlParameter("@TgtTimeYn"       , SqlDbType.Char     ,1);
				sqlParams[i++] = new SqlParameter("@TgtTime"         , SqlDbType.VarChar,128);
				sqlParams[i++] = new SqlParameter("@TgtAgeYn"        , SqlDbType.Char     ,1);
				sqlParams[i++] = new SqlParameter("@TgtAge"          , SqlDbType.VarChar,128);
				sqlParams[i++] = new SqlParameter("@TgtAgeBtnYn"     , SqlDbType.Char     ,1);
				sqlParams[i++] = new SqlParameter("@TgtAgeBtnBegin"  , SqlDbType.TinyInt    );
				sqlParams[i++] = new SqlParameter("@TgtAgeBtnEnd"    , SqlDbType.TinyInt    );
				sqlParams[i++] = new SqlParameter("@TgtSexYn"        , SqlDbType.Char     ,1);
				sqlParams[i++] = new SqlParameter("@TgtSexMan"       , SqlDbType.Char     ,1);
				sqlParams[i++] = new SqlParameter("@TgtSexWoman"     , SqlDbType.Char     ,1);
//				sqlParams[i++] = new SqlParameter("@TgtRateYn"       , SqlDbType.Char     ,1);
//				sqlParams[i++] = new SqlParameter("@TgtRate"         , SqlDbType.SmallInt   );
				sqlParams[i++] = new SqlParameter("@TgtWeekYn"       , SqlDbType.Char     ,1);
				sqlParams[i++] = new SqlParameter("@TgtWeek"         , SqlDbType.VarChar ,13);
				sqlParams[i++] = new SqlParameter("@TgtCollectionYn"       , SqlDbType.Char     ,1);
                //sqlParams[i++] = new SqlParameter("@TgtCollection"         , SqlDbType.Int		  );
                sqlParams[i++] = new SqlParameter("@TgtStbYn", SqlDbType.Char, 1);
                sqlParams[i++] = new SqlParameter("@TgtStb", SqlDbType.VarChar, 512);
                sqlParams[i++] = new SqlParameter("@TgtPocYn", SqlDbType.Char, 1);
                sqlParams[i++] = new SqlParameter("@TgtPoc", SqlDbType.VarChar, 512);

				i = 0;
				sqlParams[i++].Value = Convert.ToInt32(targetingHomeModel.ItemNo);        
				sqlParams[i++].Value = Convert.ToDecimal(targetingHomeModel.ContractAmt);   
				sqlParams[i++].Value = Convert.ToInt16(targetingHomeModel.PriorityCd);    
				sqlParams[i++].Value = targetingHomeModel.AmtControlYn;    
				sqlParams[i++].Value = Convert.ToInt16(targetingHomeModel.AmtControlRate);    
				sqlParams[i++].Value = targetingHomeModel.TgtRegion1Yn;    
				sqlParams[i++].Value = targetingHomeModel.TgtRegion1;    
				sqlParams[i++].Value = targetingHomeModel.TgtTimeYn;    
				sqlParams[i++].Value = targetingHomeModel.TgtTime;    
				sqlParams[i++].Value = targetingHomeModel.TgtAgeYn;    
				sqlParams[i++].Value = targetingHomeModel.TgtAge;    
				sqlParams[i++].Value = targetingHomeModel.TgtAgeBtnYn;    
				sqlParams[i++].Value = Convert.ToInt16(targetingHomeModel.TgtAgeBtnBegin);    
				sqlParams[i++].Value = Convert.ToInt16(targetingHomeModel.TgtAgeBtnEnd);    
				sqlParams[i++].Value = targetingHomeModel.TgtSexYn;    
				sqlParams[i++].Value = targetingHomeModel.TgtSexMan;    
				sqlParams[i++].Value = targetingHomeModel.TgtSexWoman;    
//				sqlParams[i++].Value = targetingHomeModel.TgtRateYn;    
//				sqlParams[i++].Value = Convert.ToInt16(targetingHomeModel.TgtRate);    
				sqlParams[i++].Value = targetingHomeModel.TgtWeekYn;    
				sqlParams[i++].Value = targetingHomeModel.TgtWeek;    
				sqlParams[i++].Value = targetingHomeModel.TgtCollectionYn;    
                //sqlParams[i++].Value = Convert.ToInt32(targetingHomeModel.TgtCollection);  
                sqlParams[i++].Value = targetingHomeModel.TgtStbModelYn;
                sqlParams[i++].Value = targetingHomeModel.TgtStbModel;
                sqlParams[i++].Value = targetingHomeModel.TgtPocYn;
                sqlParams[i++].Value = targetingHomeModel.TgtPoc;    

				// 쿼리실행
				try
				{
					_db.BeginTran();


					sbQuery = new StringBuilder();
					sbQuery.Append(" UPDATE TargetingHome						  \n");
					sbQuery.Append("    SET ContractAmt		= @ContractAmt	  \n");
					sbQuery.Append("       ,PriorityCd		= @PriorityCd	  \n");
					sbQuery.Append("       ,AmtControlYn  	= @AmtControlYn    \n");
					sbQuery.Append("       ,AmtControlRate	= @AmtControlRate  \n");
					sbQuery.Append("       ,TgtRegion1Yn  	= @TgtRegion1Yn    \n");
					sbQuery.Append("       ,TgtRegion1    	= @TgtRegion1      \n");
					sbQuery.Append("       ,TgtTimeYn     	= @TgtTimeYn       \n");
					sbQuery.Append("       ,TgtTime       	= @TgtTime         \n");
					sbQuery.Append("       ,TgtAgeYn      	= @TgtAgeYn        \n");
					sbQuery.Append("       ,TgtAge        	= @TgtAge          \n");
					sbQuery.Append("       ,TgtAgeBtnYn   	= @TgtAgeBtnYn     \n");
					sbQuery.Append("       ,TgtAgeBtnBegin	= @TgtAgeBtnBegin  \n");
					sbQuery.Append("       ,TgtAgeBtnEnd  	= @TgtAgeBtnEnd    \n");
					sbQuery.Append("       ,TgtSexYn      	= @TgtSexYn        \n");
					sbQuery.Append("       ,TgtSexMan     	= @TgtSexMan       \n");
					sbQuery.Append("       ,TgtSexWoman   	= @TgtSexWoman     \n");
//					sbQuery.Append("       ,TgtRateYn     	= @TgtRateYn       \n");
//					sbQuery.Append("       ,TgtRate       	= @TgtRate         \n");
					sbQuery.Append("       ,TgtWeekYn     	= @TgtWeekYn       \n");
					sbQuery.Append("       ,TgtWeek       	= @TgtWeek         \n");
					sbQuery.Append("       ,TgtCollectionYn     	= @TgtCollectionYn       \n");

                    sbQuery.Append("       ,TgtStbTypeYn    = @TgtStbYn         \n");
                    sbQuery.Append("       ,TgtStbType      = @TgtStb           \n");
                    sbQuery.Append("       ,TgtPocYn       	= @TgtPocYn         \n");
                    sbQuery.Append("       ,TgtPoc       	= @TgtPoc           \n");

                    //sbQuery.Append("       ,TgtCollection       	= @TgtCollection         \n");
					sbQuery.Append("  WHERE ItemNo          = @ItemNo		  \n");


					rc =  _db.ExecuteNonQueryParams(sbQuery.ToString(), sqlParams);

					if(rc < 1)
					{
						// 지정채널 편성 테이블에 추가
						sbQuery = new StringBuilder();
						sbQuery.Append("\n");
						sbQuery.Append(" INSERT INTO TargetingHome    \n");
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
//						sbQuery.Append("            ,TgtRateYn       \n");
//						sbQuery.Append("            ,TgtRate         \n");
						sbQuery.Append("            ,TgtWeekYn       \n");
						sbQuery.Append("            ,TgtWeek         \n");
						sbQuery.Append("            ,TgtCollectionYn       \n");
                        sbQuery.Append("            ,TgtStbTypeYn    \n");
                        sbQuery.Append("            ,TgtStbType      \n");
                        sbQuery.Append("            ,TgtPocYn        \n");
                        sbQuery.Append("            ,TgtPoc       	 \n");
                        //sbQuery.Append("            ,TgtCollection         \n");
						sbQuery.Append("            )				  \n");
						sbQuery.Append("      VALUES				  \n");
						sbQuery.Append("            (@ItemNo		  \n");
						sbQuery.Append("            ,@ContractAmt	  \n");
                        sbQuery.Append("            ,1000000            \n");
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
//						sbQuery.Append("            ,@TgtRateYn       \n");
//						sbQuery.Append("            ,@TgtRate         \n");
						sbQuery.Append("            ,@TgtWeekYn       \n");
						sbQuery.Append("            ,@TgtWeek         \n");
						sbQuery.Append("            ,@TgtCollectionYn       \n");
                        sbQuery.Append("            ,@TgtStbYn         \n");
                        sbQuery.Append("            ,@TgtStb           \n");
                        sbQuery.Append("            ,@TgtPocYn         \n");
                        sbQuery.Append("            ,@TgtPoc           \n");
                        //sbQuery.Append("            ,@TgtCollection         \n");
						sbQuery.Append(" 			)				  \n");

						rc =  _db.ExecuteNonQueryParams(sbQuery.ToString(), sqlParams);

					}

					// 2007.10.25 RH.Jung 노출값재설정을 이제는 안해도 됨. 서버에서 실시간 처리함
					//SetPriorityValues(targetingHomeModel.ItemNo);

					_db.CommitTran();
            
					// __MESSAGE__
					_log.Message("광고 타겟팅 수정:["+targetingHomeModel.ItemNo +"] " + targetingHomeModel.ItemName + " 등록자:[" + header.UserID + "]");
            
				}
				catch(Exception ex)
				{
					_db.RollbackTran();
					throw ex;
				}
            
				targetingHomeModel.ResultCD = "0000";  // 정상
            
				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "SetTargetingDetailUpdate() End");
				_log.Debug("-----------------------------------------");	
			}
			catch(Exception ex)
			{
				targetingHomeModel.ResultCD   = "3101";
				targetingHomeModel.ResultDesc = "광고 타겟팅 수정 중 오류가 발생하였습니다";
				_log.Exception(ex);
			}
			finally
			{
				// 데이터베이스를  Close한다
				_db.Close();
			}

		}

		#endregion

		#region 노출값 재설정

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
					+ "  FROM TargetingHome  A INNER JOIN ContractItem B ON (A.ItemNo = B.ItemNo ) \n"
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
					+ "  FROM TargetingHome A with(nolock)    \n"
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
					+ "          FROM TargetingHome A  with(nolock) \n"
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
					+ "    UPDATE TargetingHome                 \n"
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

        #region [E_02] 광고지역 조회
        /// <summary>
		/// 광고지역 조회
		/// </summary>
		/// <param name="targetingHomeModel"></param>
		public void GetRegionList(HeaderModel header, TargetingHomeModel targetingHomeModel)
		{
			try
			{
				// 데이터베이스를 OPEN한다
				_db.Open();


				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetRegionList() Start");
				_log.Debug("-----------------------------------------");

				StringBuilder sbQuery = new StringBuilder();
				
				/* [E_01]로 인해서 주석
				// 쿼리생성
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
                /*
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

                // [E_02] 수정
                sbQuery.Append(" Select                           \n");
                sbQuery.Append(" 	 RegionCode As RegionCode    \n");
                sbQuery.Append(" 	,RegionName As RegionName    \n");
                sbQuery.Append(" 	,UpperCode                    \n");
                sbQuery.Append(" 	,[Level]                      \n");
                sbQuery.Append(" From  TargetRegion nolock        \n");
                sbQuery.Append(" Order By Orders                  \n");
       
				// __DEBUG__
				_log.Debug(sbQuery.ToString());
				// __DEBUG__
				
				// 쿼리실행
				DataSet ds = new DataSet();
				_db.ExecuteQuery(ds,sbQuery.ToString());

				// 결과 DataSet의 장르그룹모델에 복사
				targetingHomeModel.RegionDataSet = ds.Copy();
				// 결과
				targetingHomeModel.ResultCnt = Utility.GetDatasetCount(targetingHomeModel.RegionDataSet);
				// 결과코드 셋트
				targetingHomeModel.ResultCD = "0000";

				ds.Dispose();

				// __DEBUG__
				_log.Debug("<출력정보>");
				_log.Debug("ResultCnt:[" + targetingHomeModel.ResultCnt + "]");
				// __DEBUG__

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetRegionList() End");
				_log.Debug("-----------------------------------------");
			}
			catch(Exception ex)
			{
				targetingHomeModel.ResultCD = "3000";
				targetingHomeModel.ResultDesc = "광고지역 조회중 오류가 발생하였습니다";
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
		/// <param name="targetingHomeModel"></param>
		public void GetAgeList(HeaderModel header, TargetingHomeModel targetingHomeModel)
		{
			try
			{
                // [E_03] GetAgeList() 함수만 AdtargetsSummary를 보도록 수정
                _db.ConnectionString = FrameSystem.connSummaryDbString;

				// 데이터베이스를 OPEN한다
				_db.Open();


				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetAgeList() Start");
				_log.Debug("-----------------------------------------");

				StringBuilder sbQuery = new StringBuilder();

				// 쿼리생성
				sbQuery.Append("\n"
					+ "-- 연령대 조회      \n"
					+ "SELECT SummaryCode AS AgeCode    \n"
					+ "      ,SummaryName AS AgeName    \n"
					+ "      ,UpperCode       \n"
					+ "      ,Level           \n"
					+ "  FROM SummaryCode   with(nolock)    \n"	
					+ " WHERE SummaryType = 3 -- 3:연령대\n"
					+ "   AND SummaryCode < 9000        \n"
					+ " ORDER BY SummaryCode  \n"
					);
       
				// __DEBUG__
				_log.Debug(sbQuery.ToString());
				// __DEBUG__
				
				// 쿼리실행
				DataSet ds = new DataSet();
				_db.ExecuteQuery(ds,sbQuery.ToString());

				// 결과 DataSet의 장르그룹모델에 복사
				targetingHomeModel.AgeDataSet = ds.Copy();
				// 결과
				targetingHomeModel.ResultCnt = Utility.GetDatasetCount(targetingHomeModel.AgeDataSet);
				// 결과코드 셋트
				targetingHomeModel.ResultCD = "0000";

				ds.Dispose();

				// __DEBUG__
				_log.Debug("<출력정보>");
				_log.Debug("ResultCnt:[" + targetingHomeModel.ResultCnt + "]");
				// __DEBUG__

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetAgeList() End");
				_log.Debug("-----------------------------------------");
			}
			catch(Exception ex)
			{
				targetingHomeModel.ResultCD = "3000";
				targetingHomeModel.ResultDesc = "연령대 조회중 오류가 발생하였습니다";
				_log.Exception(ex);
			}
			finally
			{
				// 데이터베이스를  Close한다
				_db.Close();
			}

		}
		#endregion

        #region 고객군타겟팅 조회
        /// <summary>
        /// 연령대 조회
        /// </summary>
        /// <param name="targetingModel"></param>
        public void GetTargetingCollectionList(HeaderModel header, TargetingHomeModel targetingHomeModel)
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
                    + "-- 고객군타겟팅 조회					\n"
                    + "SELECT 'False' AS CheckYn            \n"
                    + "		  ,A.CollectionCode, B.CollectionName, isnull(A.Sign,'-') as Sign   \n"
                    + "  FROM TargetingCollection A with(nolock) LEFT JOIN Collection B with(nolock) ON (A.CollectionCode = B.CollectionCode)   \n"
                    + " WHERE A.ItemNo = " + targetingHomeModel.ItemNo + "   \n"
                    + "   AND A.SetType = '2' -- 1:일반 2:홈광고  \n"
                    + " ORDER BY CollectionCode  \n"
                    );

                // __DEBUG__
                _log.Debug(sbQuery.ToString());
                // __DEBUG__

                // 쿼리실행
                DataSet ds = new DataSet();
                _db.ExecuteQuery(ds, sbQuery.ToString());

                // 결과 DataSet의 장르그룹모델에 복사
                targetingHomeModel.TargetingCollectionDataSet = ds.Copy();
                // 결과
                targetingHomeModel.ResultCnt = Utility.GetDatasetCount(targetingHomeModel.TargetingCollectionDataSet);
                // 결과코드 셋트
                targetingHomeModel.ResultCD = "0000";

                ds.Dispose();

                // __DEBUG__
                _log.Debug("<출력정보>");
                _log.Debug("ResultCnt:[" + targetingHomeModel.ResultCnt + "]");
                // __DEBUG__

                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "GetTagetingCollectionList() End");
                _log.Debug("-----------------------------------------");
            }
            catch (Exception ex)
            {
                targetingHomeModel.ResultCD = "3000";
                targetingHomeModel.ResultDesc = "고객군타겟팅 조회중 오류가 발생하였습니다";
                _log.Exception(ex);
            }
            finally
            {
                // 데이터베이스를  Close한다
                _db.Close();
            }

        }
        #endregion

        #region 고객군타겟팅 추가
        /// <summary>
        /// 비율 등록
        /// </summary>
        /// <param name="header"></param>
        /// <param name="ratioModel"></param>
        public void SetTargetingCollectionAdd(HeaderModel header, TargetingHomeModel targetingHomeModel)
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
					+ "		,Sign			           \n"
                    + "      )                        \n"
                    + " VALUES(                       \n"
                    + "       '2'  -- 1:일반 2:홈광고 \n"
                    + "      ,@ItemNo				  \n"
                    + "      ,@CollectionCode	      \n"
					+ "      ,@Sign				      \n"
                    + "		)						  \n"

                    );

                sqlParams[i++] = new SqlParameter("@ItemNo", SqlDbType.Int);
                sqlParams[i++] = new SqlParameter("@CollectionCode", SqlDbType.Int);
				sqlParams[i++] = new SqlParameter("@Sign", SqlDbType.Char, 1);

                i = 0;
                sqlParams[i++].Value = Convert.ToInt32(targetingHomeModel.ItemNo);
                sqlParams[i++].Value = Convert.ToInt32(targetingHomeModel.CollectionCode);
				sqlParams[i++].Value = targetingHomeModel.TgtCollectionYn.Trim();

                _log.Debug("ItemNo      :[" + targetingHomeModel.ItemNo + "]");
                _log.Debug("CollCode	:[" + targetingHomeModel.CollectionCode + "]");
				_log.Debug("Sign		:[" + targetingHomeModel.TgtCollectionYn + "]");

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

                targetingHomeModel.ResultCD = "0000";  // 정상

                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "." + MethodBase.GetCurrentMethod().Name + " End");
                _log.Debug("-----------------------------------------");
            }
            catch (Exception ex)
            {
                targetingHomeModel.ResultCD = "3101";
                targetingHomeModel.ResultDesc = "고객군타겟팅 추가중 오류가 발생하였습니다";
                _log.Exception(ex);
            }
            finally
            {
                // 데이터베이스를  Close한다
                _db.Close();
            }
        }


        #endregion

        #region 고객군타겟팅 삭제
        /// <summary>
        /// 비율 등록
        /// </summary>
        /// <param name="header"></param>
        /// <param name="ratioModel"></param>
        public void SetTargetingCollectionDelete(HeaderModel header, TargetingHomeModel targetingHomeModel)
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
                    + "	WHERE SetType        = '2'  -- 1:일반 2:홈광고  \n"
                    + "   AND ItemNo         = @ItemNo         \n" 
                    + "	  AND CollectionCode = @CollectionCode \n"
                    );

                sqlParams[i++] = new SqlParameter("@ItemNo", SqlDbType.Int);
                sqlParams[i++] = new SqlParameter("@CollectionCode", SqlDbType.Int);

                i = 0;
                sqlParams[i++].Value = Convert.ToInt32(targetingHomeModel.ItemNo);
                sqlParams[i++].Value = Convert.ToInt32(targetingHomeModel.CollectionCode);

                _log.Debug("ItemNo        :[" + targetingHomeModel.ItemNo + "]");
                _log.Debug("CollectionCode:[" + targetingHomeModel.CollectionCode + "]");

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

                targetingHomeModel.ResultCD = "0000";  // 정상

                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "." + MethodBase.GetCurrentMethod().Name + " End");
                _log.Debug("-----------------------------------------");
            }
            catch (Exception ex)
            {
                targetingHomeModel.ResultCD = "3101";
                targetingHomeModel.ResultDesc = "고객군타겟팅 추가중 오류가 발생하였습니다";
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