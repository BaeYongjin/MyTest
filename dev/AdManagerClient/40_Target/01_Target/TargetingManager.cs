// ===============================================================================
// Targeting Manager  for Charites Project
//
// TargetingManager.cs
//
// 광고파일정보 서비스를 호출합니다. 
//
// ===============================================================================
// Release history
//
// ===============================================================================
// Copyright (C) 2006 G-Inno Systems Inc.
// All rights reserved.
// ==============================================================================
/*
 * -------------------------------------------------------
 * Class Name: TargetingManager
 * 주요기능  : 상업광고 타겟팅
 * 작성자    : 모름
 * 작성일    : 모름
 * 특이사항  : 2SLOT 처리 추가
 * -------------------------------------------------------
 * [수정사항]
 * -------------------------------------------------------
 * 수정코드  : [E_01]
 * 수정자    : bae
 * 수정일    : 2010.10.04
 * 수정내용  :        
 *            - 2slot 처리위해서 TargetingModel의 SlotExt 처리.
 *              
 * 수정함수  :
 *            - SetTargetingDetailUpdate(..) 
 *            - 
 * -------------------------------------------------------- 
 * 수정코드  : [E_03]
 * 수정자    : 김보배
 * 수정일    : 2013.07.09
 * 수정내용  :        
 *            - 선호도조사팝업 사용여부, 송출비율, 응답자미송출여부 추가
 * 수정함수  :
 *            - SetTargetingDetailUpdate()
 * --------------------------------------------------------  
 * 수정코드  : [E_04]
 * 수정자    : 김보배
 * 수정일    : 2013.10.16
 * 수정내용  :        
 *            - 프로파일 타겟팅 사용여부, 연령대, 신뢰도 추가
 * 수정함수  :
 *            - SetTargetingDetailUpdate() 수정
 *            - SetTargetingProfileAdd() 추가
 * -------------------------------------------------------- 
 */

using System;
using System.Data;

using WinFramework.Base;
using WinFramework.Data;
using WinFramework.Misc;

using AdManagerModel;

namespace AdManagerClient
{
	/// <summary>
	/// 타겟팅 웹서비스를 호출합니다. 
	/// </summary>
	public class TargetingManager : BaseManager
	{
		/// <summary>
		/// 생성자
		/// </summary>
		/// <param name="systemModel"></param>
		/// <param name="commonModel"></param>

		public TargetingManager(SystemModel systemModel, CommonModel commonModel) : base(systemModel, commonModel)
		{
			_log = FrameSystem.oLog;
			_module = "Targeting";
			_Host  = FrameSystem.m_WebServer_Host;
			_Port  = FrameSystem.m_WebServer_Port;
			_Path  = FrameSystem.m_WebServer_App + "/Target/TargetingService.asmx";
		}


		/// <summary>
		/// 타겟팅 대상광고 리스트 조회
		/// </summary>
		/// <param name="targetingModel"></param>
		/// <param name="adType">10:상업광고류, 20:매체광고류</param>
		public void GetTargetingList(TargetingModel targetingModel, string adType )
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("타겟팅 목록조회 Start");
				_log.Debug("-----------------------------------------");
				
				// 웹서비스 인스턴스 생성
				TargetingServicePloxy.TargetingService svc = new TargetingServicePloxy.TargetingService();
			
				// 웹서비스 URL동적 생성
				svc.Url = _WebServiceUrl;			
			
				// 리모트 모델 생성
				TargetingServicePloxy.HeaderModel    remoteHeader = new AdManagerClient.TargetingServicePloxy.HeaderModel();
				TargetingServicePloxy.TargetingModel remoteData   = new AdManagerClient.TargetingServicePloxy.TargetingModel();

				// 헤더정보 셋트
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;
				remoteHeader.UserLevel     = Header.UserLevel;
				remoteHeader.UserClass     = Header.UserClass;

				// 호출정보 셋트
				remoteData.SearchKey             =  targetingModel.SearchKey;               
				remoteData.SearchMediaCode		 =  targetingModel.SearchMediaCode;	  
				remoteData.SearchRapCode		 =  targetingModel.SearchRapCode;       
				remoteData.SearchAgencyCode	     =  targetingModel.SearchAgencyCode;    
				remoteData.SearchAdvertiserCode  =  targetingModel.SearchAdvertiserCode;
				remoteData.SearchContractState	 =  targetingModel.SearchContractState;
				remoteData.SearchAdType  		 =  adType;
                remoteData.SearchAdClass  		 =  targetingModel.SearchAdClass;           // 광고타입지정하는 경우

				remoteData.SearchchkAdState_20	 =  targetingModel.SearchchkAdState_20; 
				remoteData.SearchchkAdState_30	 =  targetingModel.SearchchkAdState_30; 
				remoteData.SearchchkAdState_40	 =  targetingModel.SearchchkAdState_40; 
                
                // 웹서비스 호출 타임아웃설정
                svc.Timeout = FrameSystem.m_SystemTimeout;
				// 웹서비스 메소드 호출
				remoteData = svc.GetTargetingList(remoteHeader, remoteData);

				// 결과코드검사
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// 결과 셋트
				targetingModel.TargetingDataSet = remoteData.TargetingDataSet.Copy();
				targetingModel.ResultCnt   = remoteData.ResultCnt;
				targetingModel.ResultCD    = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("타겟팅 목록조회 End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":GetTargetingList():" + fe.ErrCode + ":" + fe.ResultMsg);
				_log.Warning("-----------------------------------------");
				throw fe;
			}
			catch(Exception e)
			{
				_log.Error("-----------------------------------------");
				_log.Exception(e);
				_log.Error("-----------------------------------------");
				throw new FrameException(e.Message);
			}
		}

        /// <summary>
        /// 고객군 목록조회
        /// </summary>
        /// <param name="targetingModel"></param>
        public void GetCollectionsList(TargetingModel targetingModel)
        {

            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug("고객군 목록조회 Start");
                _log.Debug("-----------------------------------------");

                // 웹서비스 인스턴스 생성
                TargetingServicePloxy.TargetingService svc = new TargetingServicePloxy.TargetingService();

                // 웹서비스 URL동적 생성
                svc.Url = _WebServiceUrl;

                // 리모트 모델 생성
                TargetingServicePloxy.HeaderModel remoteHeader = new AdManagerClient.TargetingServicePloxy.HeaderModel();
                TargetingServicePloxy.TargetingModel remoteData = new AdManagerClient.TargetingServicePloxy.TargetingModel();

                // 헤더정보 셋트
                remoteHeader.ClientKey = Header.ClientKey;
                remoteHeader.UserID = Header.UserID;
                remoteHeader.UserLevel = Header.UserLevel;
                remoteHeader.UserClass = Header.UserClass;

                // 웹서비스 호출 타임아웃설정
                svc.Timeout = FrameSystem.m_SystemTimeout;
                // 웹서비스 메소드 호출
                remoteData = svc.GetCollectionList(remoteHeader, remoteData);

                // 결과코드검사
                if (!remoteData.ResultCD.Equals("0000"))
                {
                    throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
                }

                // 결과 셋트
                targetingModel.CollectionsDataSet = remoteData.CollectionsDataSet.Copy();
                targetingModel.ResultCnt = remoteData.ResultCnt;
                targetingModel.ResultCD = remoteData.ResultCD;

                _log.Debug("-----------------------------------------");
                _log.Debug("고객군 목록조회 End");
                _log.Debug("-----------------------------------------");
            }
            catch (FrameException fe)
            {
                _log.Warning("-----------------------------------------");
                _log.Warning(this.ToString() + ":GetCollectionsList():" + fe.ErrCode + ":" + fe.ResultMsg);
                _log.Warning("-----------------------------------------");
                throw fe;
            }
            catch (Exception e)
            {
                _log.Error("-----------------------------------------");
                _log.Exception(e);
                _log.Error("-----------------------------------------");
                throw new FrameException(e.Message);
            }
        }	

		/// <summary>
		/// 타겟팅 상세 조회
		/// </summary>
		/// <param name="targetingModel"></param>
		public void GetTargetingDetail(TargetingModel targetingModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("타겟팅 상세 조회 Start");
				_log.Debug("-----------------------------------------");
				
				TargetingServicePloxy.TargetingService svc = new TargetingServicePloxy.TargetingService();
			
				// 리모트 모델 생성
				TargetingServicePloxy.HeaderModel		remoteHeader = new AdManagerClient.TargetingServicePloxy.HeaderModel();
				TargetingServicePloxy.TargetingModel	remoteData   = new AdManagerClient.TargetingServicePloxy.TargetingModel();

				// 헤더정보 셋트
				remoteHeader.ClientKey  = Header.ClientKey;
				remoteHeader.UserID     = Header.UserID;
				remoteHeader.UserLevel  = Header.UserLevel;
				remoteHeader.UserClass  = Header.UserClass;

				// 호출정보 셋트
				remoteData.ItemNo		= targetingModel.ItemNo;
                
				svc.Timeout = FrameSystem.m_SystemTimeout;
                svc.Url     = _WebServiceUrl;
				remoteData  = svc.GetTargetingDetail(remoteHeader, remoteData);

				// 결과코드검사
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// 결과 셋트
				targetingModel.DetailDataSet = remoteData.DetailDataSet.Copy();
				targetingModel.ResultCnt   = remoteData.ResultCnt;
				targetingModel.ResultCD    = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("타겟팅 상세 조회 End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":GetTargetingDetail():" + fe.ErrCode + ":" + fe.ResultMsg);
				_log.Warning("-----------------------------------------");
				throw fe;
			}
			catch(Exception e)
			{
				_log.Error("-----------------------------------------");
				_log.Exception(e);
				_log.Error("-----------------------------------------");
				throw new FrameException(e.Message);
			}
		}

        /// <summary>
        /// 타겟팅 상세 조회
        /// </summary>
        /// <param name="targetingModel"></param>
        public void GetTargetingDetail2(TargetingModel targetingModel)
        {
            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug("타겟팅 상세 조회 Start");
                _log.Debug("-----------------------------------------");

                TargetingServicePloxy.TargetingService svc = new TargetingServicePloxy.TargetingService();

                // 리모트 모델 생성
                TargetingServicePloxy.HeaderModel remoteHeader = new AdManagerClient.TargetingServicePloxy.HeaderModel();
                TargetingServicePloxy.TargetingModel remoteData = new AdManagerClient.TargetingServicePloxy.TargetingModel();

                // 헤더정보 셋트
                remoteHeader.ClientKey = Header.ClientKey;
                remoteHeader.UserID = Header.UserID;
                remoteHeader.UserLevel = Header.UserLevel;
                remoteHeader.UserClass = Header.UserClass;

                // 호출정보 셋트
                remoteData.ItemNo = targetingModel.ItemNo;

                svc.Timeout = FrameSystem.m_SystemTimeout;
                svc.Url = _WebServiceUrl;
                remoteData = svc.GetTargetingDetail2(remoteHeader, remoteData);

                // 결과코드검사
                if (!remoteData.ResultCD.Equals("0000"))
                {
                    throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
                }

                // 결과 셋트
                targetingModel.DetailDataSet = remoteData.DetailDataSet.Copy();
                targetingModel.ResultCnt = remoteData.ResultCnt;
                targetingModel.ResultCD = remoteData.ResultCD;

                _log.Debug("-----------------------------------------");
                _log.Debug("타겟팅 상세 조회 End");
                _log.Debug("-----------------------------------------");
            }
            catch (FrameException fe)
            {
                _log.Warning("-----------------------------------------");
                _log.Warning(this.ToString() + ":GetTargetingDetail():" + fe.ErrCode + ":" + fe.ResultMsg);
                _log.Warning("-----------------------------------------");
                throw fe;
            }
            catch (Exception e)
            {
                _log.Error("-----------------------------------------");
                _log.Exception(e);
                _log.Error("-----------------------------------------");
                throw new FrameException(e.Message);
            }
        }

		/// <summary>
		/// 타겟팅 비율 조회
		/// </summary>
		/// <param name="targetingModel"></param>
		public void GetTargetingRate(TargetingModel targetingModel)
		{

			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("타겟팅 비율 조회 Start");
				_log.Debug("-----------------------------------------");
				
				// 웹서비스 인스턴스 생성
				TargetingServicePloxy.TargetingService svc = new TargetingServicePloxy.TargetingService();
			
				// 웹서비스 URL동적 생성
				svc.Url = _WebServiceUrl;			
			
				// 리모트 모델 생성
				TargetingServicePloxy.HeaderModel		remoteHeader = new AdManagerClient.TargetingServicePloxy.HeaderModel();
				TargetingServicePloxy.TargetingModel	remoteData   = new AdManagerClient.TargetingServicePloxy.TargetingModel();

				// 헤더정보 셋트
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;
				remoteHeader.UserLevel     = Header.UserLevel;
				remoteHeader.UserClass     = Header.UserClass;

				// 호출정보 셋트
				remoteData.ItemNo             =  targetingModel.ItemNo;               
                
				// 웹서비스 호출 타임아웃설정
				svc.Timeout = FrameSystem.m_SystemTimeout;
				// 웹서비스 메소드 호출
				remoteData = svc.GetTargetingRate(remoteHeader, remoteData);

				// 결과코드검사
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// 결과 셋트
				targetingModel.RateDataSet = remoteData.RateDataSet.Copy();
				targetingModel.ResultCnt   = remoteData.ResultCnt;
				targetingModel.ResultCD    = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("타겟팅 비율 조회 End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":GetTargetingRate():" + fe.ErrCode + ":" + fe.ResultMsg);
				_log.Warning("-----------------------------------------");
				throw fe;
			}
			catch(Exception e)
			{
				_log.Error("-----------------------------------------");
				_log.Exception(e);
				_log.Error("-----------------------------------------");
				throw new FrameException(e.Message);
			}
		}	
		

		/// <summary>
		/// 타겟팅 상세정보 저장
		/// </summary>
		/// <param name="targetingModel"></param>
		public void SetTargetingDetailUpdate(TargetingModel targetingModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("타겟팅 상세정보 저장 Start");
				_log.Debug("-----------------------------------------");
				
				// 웹서비스 인스턴스 생성
				TargetingServicePloxy.TargetingService svc = new TargetingServicePloxy.TargetingService();
			
				// 리모트 모델 생성
				TargetingServicePloxy.HeaderModel       remoteHeader = new AdManagerClient.TargetingServicePloxy.HeaderModel();
				TargetingServicePloxy.TargetingModel    remoteData   = new AdManagerClient.TargetingServicePloxy.TargetingModel();

				// 헤더정보 셋트
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;
				remoteHeader.UserLevel     = Header.UserLevel;
				remoteHeader.UserClass     = Header.UserClass;

				// 호출정보 셋트
                remoteData.ItemNo          = targetingModel.ItemNo;
				remoteData.ItemName        = targetingModel.ItemName;
				remoteData.ContractAmt     = targetingModel.ContractAmt;
                remoteData.PriorityCd      = targetingModel.PriorityCd;
				remoteData.AmtControlYn    = targetingModel.AmtControlYn;
				remoteData.AmtControlRate  = targetingModel.AmtControlRate;
				remoteData.TgtRegion1Yn    = targetingModel.TgtRegion1Yn;
                remoteData.TgtRegion1      = targetingModel.TgtRegion1;
				remoteData.TgtTimeYn       = targetingModel.TgtTimeYn;
				remoteData.TgtTime         = targetingModel.TgtTime;
				remoteData.TgtAgeYn        = targetingModel.TgtAgeYn;
				remoteData.TgtAge          = targetingModel.TgtAge;
				remoteData.TgtAgeBtnYn     = targetingModel.TgtAgeBtnYn;
				remoteData.TgtAgeBtnBegin  = targetingModel.TgtAgeBtnBegin;
				remoteData.TgtAgeBtnEnd    = targetingModel.TgtAgeBtnEnd;
				remoteData.TgtSexYn        = targetingModel.TgtSexYn;
				remoteData.TgtSexMan       = targetingModel.TgtSexMan;
				remoteData.TgtSexWoman     = targetingModel.TgtSexWoman;
				remoteData.TgtRateYn       = targetingModel.TgtRateYn;
				remoteData.TgtRate         = targetingModel.TgtRate;
				remoteData.TgtWeekYn       = targetingModel.TgtWeekYn;
				remoteData.TgtWeek         = targetingModel.TgtWeek;
				remoteData.TgtCollectionYn = targetingModel.TgtCollectionYn;
				remoteData.TgtCollection   = targetingModel.TgtCollection;

				remoteData.TgtZipYn			= targetingModel.TgtZipYn;
				remoteData.TgtZip			= targetingModel.TgtZip;
				remoteData.TgtPPxYn			= targetingModel.TgtPPxYn;
				remoteData.TgtFreqYn		= targetingModel.TgtFreqYn;
				remoteData.TgtFreqDay		= targetingModel.TgtFreqDay;
				remoteData.TgtFreqFeriod	= targetingModel.TgtFreqFeriod;
				remoteData.TgtPVSYn			= targetingModel.TgtPVSYn;

                remoteData.SlotExt          = targetingModel.SlotExt;			//[E_01]
                remoteData.TgtStbModelYn    = targetingModel.TgtStbModelYn;  // [E_08]
                remoteData.TgtStbModel      = targetingModel.TgtStbModel;    // [E_08]

                remoteData.TgtPrefYn        = targetingModel.TgtPrefYn;      // [E_09]
                remoteData.TgtPrefRate      = targetingModel.TgtPrefRate;    // [E_09]
                remoteData.TgtPrefNosend    = targetingModel.TgtPrefNosend;  // [E_09]
                remoteData.TgtProfileYn     = targetingModel.TgtProfileYn;   // [E_04]
                remoteData.TgtProfile       = targetingModel.TgtProfile;     // [E_04]
                remoteData.TgtReliablilty   = targetingModel.TgtReliablilty; // [E_04]

                remoteData.TgtPocYn         = targetingModel.TgtPocYn;
                remoteData.TgtPoc           = targetingModel.TgtPoc;

				// 웹서비스 호출 타임아웃설정
				svc.Url = _WebServiceUrl;
				svc.Timeout = FrameSystem.m_SystemTimeout;

				// 웹서비스 메소드 호출
                remoteData = svc.SetTargetingDetailUpdate(remoteHeader, remoteData);

				// 결과코드검사
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// 결과 셋트
				targetingModel.ResultCnt   = remoteData.ResultCnt;
				targetingModel.ResultCD    = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("타겟팅 상세정보 저장 End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":SetSchChoiceMenuDetailAdd():" + fe.ErrCode + ":" + fe.ResultMsg);
				_log.Warning("-----------------------------------------");
				throw fe;
			}
			catch(Exception e)
			{
				_log.Error("-----------------------------------------");
				_log.Exception(e);
				_log.Error("-----------------------------------------");
				throw new FrameException(e.Message);
			}
		}

		/// <summary>
		/// 타겟팅 정보 저장
		/// </summary>
		/// <param name="targetingModel"></param>
		public void SetTargetingRateUpdate(TargetingModel targetingModel)
		{

			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("타겟팅 상세정보 저장 Start");
				_log.Debug("-----------------------------------------");
				
				// 웹서비스 인스턴스 생성
				TargetingServicePloxy.TargetingService svc = new TargetingServicePloxy.TargetingService();
			
				// 웹서비스 URL동적 생성
				svc.Url = _WebServiceUrl;			
			
				// 리모트 모델 생성
				TargetingServicePloxy.HeaderModel      remoteHeader = new AdManagerClient.TargetingServicePloxy.HeaderModel();
				TargetingServicePloxy.TargetingModel remoteData   = new AdManagerClient.TargetingServicePloxy.TargetingModel();

				// 헤더정보 셋트
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;
				remoteHeader.UserLevel     = Header.UserLevel;
				remoteHeader.UserClass     = Header.UserClass;

				// 호출정보 셋트
				remoteData.ItemNo     = targetingModel.ItemNo;
				remoteData.Type		  = targetingModel.Type;	
				remoteData.Rate1	  = targetingModel.Rate1;	
				remoteData.Rate2	  = targetingModel.Rate2;	
				remoteData.Rate3	  = targetingModel.Rate3;	
				remoteData.Rate4	  = targetingModel.Rate4;	
				remoteData.Rate5	  = targetingModel.Rate5;	
				remoteData.Rate6	  = targetingModel.Rate6;	
				remoteData.Rate7	  = targetingModel.Rate7;	
				remoteData.Rate8	  = targetingModel.Rate8;	
				remoteData.Rate9	  = targetingModel.Rate9;	
				remoteData.Rate10	  = targetingModel.Rate10;	
				remoteData.Rate11	  = targetingModel.Rate11;	
				remoteData.Rate12	  = targetingModel.Rate12;	
				remoteData.Rate13	  = targetingModel.Rate13;	
				remoteData.Rate14	  = targetingModel.Rate14;	
				remoteData.Rate15	  = targetingModel.Rate15;	
				remoteData.Rate16	  = targetingModel.Rate16;	
				remoteData.Rate17	  = targetingModel.Rate17;	
				remoteData.Rate18	  = targetingModel.Rate18;	
				remoteData.Rate19	  = targetingModel.Rate19;	
				remoteData.Rate20	  = targetingModel.Rate20;	
				remoteData.Rate21	  = targetingModel.Rate21;	
				remoteData.Rate22	  = targetingModel.Rate22;	
				remoteData.Rate23	  = targetingModel.Rate23;
				remoteData.Rate24	  = targetingModel.Rate24;	
				

				// 웹서비스 호출 타임아웃설정
				svc.Timeout = FrameSystem.m_SystemTimeout;

				// 웹서비스 메소드 호출
				remoteData = svc.SetTargetingRateUpdate(remoteHeader, remoteData);

				// 결과코드검사
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// 결과 셋트
				targetingModel.ResultCnt   = remoteData.ResultCnt;
				targetingModel.ResultCD    = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("타겟팅 상세정보 저장 End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":SetTargetingRateUpdate():" + fe.ErrCode + ":" + fe.ResultMsg);
				_log.Warning("-----------------------------------------");
				throw fe;
			}
			catch(Exception e)
			{
				_log.Error("-----------------------------------------");
				_log.Exception(e);
				_log.Error("-----------------------------------------");
				throw new FrameException(e.Message);
			}
		}

		/// <summary>
		/// 타겟팅지역구분조회
		/// </summary>
		/// <param name="targetingModel"></param>
		public void GetRegionList(TargetingModel targetingModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("타겟팅지역구분조회 Start");
				_log.Debug("-----------------------------------------");
				
				// 웹서비스 인스턴스 생성
				TargetingServicePloxy.TargetingService svc = new TargetingServicePloxy.TargetingService();
			
				// 웹서비스 URL동적 생성
				svc.Url = _WebServiceUrl;			
			
				// 리모트 모델 생성
				TargetingServicePloxy.HeaderModel      remoteHeader = new AdManagerClient.TargetingServicePloxy.HeaderModel();
				TargetingServicePloxy.TargetingModel remoteData   = new AdManagerClient.TargetingServicePloxy.TargetingModel();

				// 헤더정보 셋트
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;
				remoteHeader.UserLevel     = Header.UserLevel;
				remoteHeader.UserClass     = Header.UserClass;
                
				// 웹서비스 호출 타임아웃설정
				svc.Timeout = FrameSystem.m_SystemTimeout;
				// 웹서비스 메소드 호출
				remoteData = svc.GetRegionList(remoteHeader, remoteData);

				// 결과코드검사
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// 결과 셋트
				targetingModel.RegionDataSet = remoteData.RegionDataSet.Copy();
				targetingModel.ResultCnt     = remoteData.ResultCnt;
				targetingModel.ResultCD      = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("타겟팅지역구분조회 End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":GetRegionList():" + fe.ErrCode + ":" + fe.ResultMsg);
				_log.Warning("-----------------------------------------");
				throw fe;
			}
			catch(Exception e)
			{
				_log.Error("-----------------------------------------");
				_log.Exception(e);
				_log.Error("-----------------------------------------");
				throw new FrameException(e.Message);
			}
		}


		/// <summary>
		/// 타겟팅연령대조회
		/// </summary>
		/// <param name="targetingModel"></param>
		public void GetAgeList(TargetingModel targetingModel)
		{

			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("타겟팅연령대조회 Start");
				_log.Debug("-----------------------------------------");
				
				// 웹서비스 인스턴스 생성
				TargetingServicePloxy.TargetingService svc = new TargetingServicePloxy.TargetingService();
			
				// 웹서비스 URL동적 생성
				svc.Url = _WebServiceUrl;			
			
				// 리모트 모델 생성
				TargetingServicePloxy.HeaderModel      remoteHeader = new AdManagerClient.TargetingServicePloxy.HeaderModel();
				TargetingServicePloxy.TargetingModel remoteData   = new AdManagerClient.TargetingServicePloxy.TargetingModel();

				// 헤더정보 셋트
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;
				remoteHeader.UserLevel     = Header.UserLevel;
				remoteHeader.UserClass     = Header.UserClass;
                
				// 웹서비스 호출 타임아웃설정
				svc.Timeout = FrameSystem.m_SystemTimeout;
				// 웹서비스 메소드 호출
				remoteData = svc.GetAgeList(remoteHeader, remoteData);

				// 결과코드검사
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// 결과 셋트
				targetingModel.AgeDataSet = remoteData.AgeDataSet.Copy();
				targetingModel.ResultCnt     = remoteData.ResultCnt;
				targetingModel.ResultCD      = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("타겟팅연령대조회 End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":GetAgeList():" + fe.ErrCode + ":" + fe.ResultMsg);
				_log.Warning("-----------------------------------------");
				throw fe;
			}
			catch(Exception e)
			{
				_log.Error("-----------------------------------------");
				_log.Exception(e);
				_log.Error("-----------------------------------------");
				throw new FrameException(e.Message);
			}
		}



        /// <summary>
        /// 설정타겟팅 목록조회
        /// </summary>
        /// <param name="targetingModel"></param>
        public void GetTargetingCollectionList(TargetingModel targetingModel)
        {

            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug("설정타겟군 목록조회 Start");
                _log.Debug("-----------------------------------------");

                // 웹서비스 인스턴스 생성
                TargetingServicePloxy.TargetingService svc = new TargetingServicePloxy.TargetingService();

                // 웹서비스 URL동적 생성
                svc.Url = _WebServiceUrl;

                // 리모트 모델 생성
                TargetingServicePloxy.HeaderModel remoteHeader = new AdManagerClient.TargetingServicePloxy.HeaderModel();
                TargetingServicePloxy.TargetingModel remoteData = new AdManagerClient.TargetingServicePloxy.TargetingModel();

                // 헤더정보 셋트
                remoteHeader.ClientKey = Header.ClientKey;
                remoteHeader.UserID = Header.UserID;
                remoteHeader.UserLevel = Header.UserLevel;
                remoteHeader.UserClass = Header.UserClass;

                // 호출정보 셋트
                remoteData.ItemNo = targetingModel.ItemNo;

                // 웹서비스 호출 타임아웃설정
                svc.Timeout = FrameSystem.m_SystemTimeout;

                // 웹서비스 메소드 호출
                remoteData = svc.GetTargetingCollectionList(remoteHeader, remoteData);

                // 결과코드검사
                if (!remoteData.ResultCD.Equals("0000"))
                {
                    throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
                }

                // 결과 셋트
                targetingModel.TargetingCollectionDataSet = remoteData.TargetingCollectionDataSet.Copy();
                targetingModel.ResultCnt = remoteData.ResultCnt;
                targetingModel.ResultCD = remoteData.ResultCD;

                _log.Debug("-----------------------------------------");
                _log.Debug("타겟군 목록조회 End");
                _log.Debug("-----------------------------------------");
            }
            catch (FrameException fe)
            {
                _log.Warning("-----------------------------------------");
                _log.Warning(this.ToString() + ":GetTargetingCollectionList():" + fe.ErrCode + ":" + fe.ResultMsg);
                _log.Warning("-----------------------------------------");
                throw fe;
            }
            catch (Exception e)
            {
                _log.Error("-----------------------------------------");
                _log.Exception(e);
                _log.Error("-----------------------------------------");
                throw new FrameException(e.Message);
            }
        }	


        /// <summary>
        /// 고객군타켓팅 추가
        /// </summary>
        /// <param name="targetingModel"></param>
        public void SetTargetingCollectionAdd(TargetingModel targetingModel)
        {

            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug("고객군타겟팅 추가 Start");
                _log.Debug("-----------------------------------------");

                // 웹서비스 인스턴스 생성
                TargetingServicePloxy.TargetingService svc = new TargetingServicePloxy.TargetingService();

                // 웹서비스 URL동적 생성
                svc.Url = _WebServiceUrl;

                // 리모트 모델 생성
                TargetingServicePloxy.HeaderModel remoteHeader = new AdManagerClient.TargetingServicePloxy.HeaderModel();
                TargetingServicePloxy.TargetingModel remoteData = new AdManagerClient.TargetingServicePloxy.TargetingModel();

                // 헤더정보 셋트
                remoteHeader.ClientKey = Header.ClientKey;
                remoteHeader.UserID = Header.UserID;
                remoteHeader.UserLevel = Header.UserLevel;
                remoteHeader.UserClass = Header.UserClass;

                // 호출정보 셋트
                remoteData.ItemNo = targetingModel.ItemNo;
                remoteData.CollectionCode = targetingModel.CollectionCode;
				remoteData.TgtCollectionYn = targetingModel.TgtCollectionYn;

                // 웹서비스 호출 타임아웃설정
                svc.Timeout = FrameSystem.m_SystemTimeout;

                // 웹서비스 메소드 호출
                remoteData = svc.SetTargetingCollectionAdd(remoteHeader, remoteData);

                // 결과코드검사
                if (!remoteData.ResultCD.Equals("0000"))
                {
                    throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
                }

                // 결과 셋트
                targetingModel.ResultCnt = remoteData.ResultCnt;
                targetingModel.ResultCD = remoteData.ResultCD;

                _log.Debug("-----------------------------------------");
                _log.Debug("고객군타겟팅 추가 End");
                _log.Debug("-----------------------------------------");
            }
            catch (FrameException fe)
            {
                _log.Warning("-----------------------------------------");
                _log.Warning(this.ToString() + ":SetTargetingCollectionAdd():" + fe.ErrCode + ":" + fe.ResultMsg);
                _log.Warning("-----------------------------------------");
                throw fe;
            }
            catch (Exception e)
            {
                _log.Error("-----------------------------------------");
                _log.Exception(e);
                _log.Error("-----------------------------------------");
                throw new FrameException(e.Message);
            }
        }

        /// <summary>
        /// 고객군타켓팅 삭제
        /// </summary>
        /// <param name="targetingModel"></param>
        public void SetTargetingCollectionDelete(TargetingModel targetingModel)
        {

            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug("고객군타겟팅 삭제 Start");
                _log.Debug("-----------------------------------------");

                // 웹서비스 인스턴스 생성
                TargetingServicePloxy.TargetingService svc = new TargetingServicePloxy.TargetingService();

                // 웹서비스 URL동적 생성
                svc.Url = _WebServiceUrl;

                // 리모트 모델 생성
                TargetingServicePloxy.HeaderModel remoteHeader = new AdManagerClient.TargetingServicePloxy.HeaderModel();
                TargetingServicePloxy.TargetingModel remoteData = new AdManagerClient.TargetingServicePloxy.TargetingModel();

                // 헤더정보 셋트
                remoteHeader.ClientKey = Header.ClientKey;
                remoteHeader.UserID = Header.UserID;
                remoteHeader.UserLevel = Header.UserLevel;
                remoteHeader.UserClass = Header.UserClass;

                // 호출정보 셋트
                remoteData.ItemNo = targetingModel.ItemNo;
                remoteData.CollectionCode = targetingModel.CollectionCode;

                // 웹서비스 호출 타임아웃설정
                svc.Timeout = FrameSystem.m_SystemTimeout;

                // 웹서비스 메소드 호출
                remoteData = svc.SetTargetingCollectionDelete(remoteHeader, remoteData);

                // 결과코드검사
                if (!remoteData.ResultCD.Equals("0000"))
                {
                    throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
                }

                // 결과 셋트
                targetingModel.ResultCnt = remoteData.ResultCnt;
                targetingModel.ResultCD = remoteData.ResultCD;

                _log.Debug("-----------------------------------------");
                _log.Debug("고객군타겟팅 추가 End");
                _log.Debug("-----------------------------------------");
            }
            catch (FrameException fe)
            {
                _log.Warning("-----------------------------------------");
                _log.Warning(this.ToString() + ":SetTargetingCollectionDelete():" + fe.ErrCode + ":" + fe.ResultMsg);
                _log.Warning("-----------------------------------------");
                throw fe;
            }
            catch (Exception e)
            {
                _log.Error("-----------------------------------------");
                _log.Exception(e);
                _log.Error("-----------------------------------------");
                throw new FrameException(e.Message);
            }
        }

        /// <summary>
        /// [E_08] 셋탑모델 조회
        /// </summary>
        /// <param name="targetingModel"></param>
        public void GetStbList(TargetingModel targetingModel)
        {

            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug("셋탑모델조회 Start");
                _log.Debug("-----------------------------------------");

                // 웹서비스 인스턴스 생성
                TargetingServicePloxy.TargetingService svc = new TargetingServicePloxy.TargetingService();

                // 웹서비스 URL동적 생성
                svc.Url = _WebServiceUrl;

                // 리모트 모델 생성
                TargetingServicePloxy.HeaderModel remoteHeader = new AdManagerClient.TargetingServicePloxy.HeaderModel();
                TargetingServicePloxy.TargetingModel remoteData = new AdManagerClient.TargetingServicePloxy.TargetingModel();

                // 헤더정보 셋트
                remoteHeader.ClientKey = Header.ClientKey;
                remoteHeader.UserID = Header.UserID;
                remoteHeader.UserLevel = Header.UserLevel;
                remoteHeader.UserClass = Header.UserClass;

                // 웹서비스 호출 타임아웃설정
                svc.Timeout = FrameSystem.m_SystemTimeout;
                // 웹서비스 메소드 호출
                remoteData = svc.GetStbList(remoteHeader, remoteData);

                // 결과코드검사
                if (!remoteData.ResultCD.Equals("0000"))
                {
                    throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
                }

                // 결과 셋트
                targetingModel.TargetingDataSet = remoteData.TargetingDataSet.Copy();
                targetingModel.ResultCnt = remoteData.ResultCnt;
                targetingModel.ResultCD = remoteData.ResultCD;

                _log.Debug("-----------------------------------------");
                _log.Debug("셋탑모델리스트조회 End");
                _log.Debug("-----------------------------------------");
            }
            catch (FrameException fe)
            {
                _log.Warning("-----------------------------------------");
                _log.Warning(this.ToString() + ":GetStbList():" + fe.ErrCode + ":" + fe.ResultMsg);
                _log.Warning("-----------------------------------------");
                throw fe;
            }
            catch (Exception e)
            {
                _log.Error("-----------------------------------------");
                _log.Exception(e);
                _log.Error("-----------------------------------------");
                throw new FrameException(e.Message);
            }
        }

        /// <summary>
        /// [E_04] 프로파일 타겟팅 저장
        /// </summary>
        /// <param name="targetingModel"></param>
        public void SetTargetingProfileAdd(TargetingModel targetingModel)
        {

            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug("프로파일 타겟팅 저장 Start");
                _log.Debug("-----------------------------------------");

                // 웹서비스 인스턴스 생성
                TargetingServicePloxy.TargetingService svc = new TargetingServicePloxy.TargetingService();

                // 웹서비스 URL동적 생성
                svc.Url = _WebServiceUrl;

                // 리모트 모델 생성
                TargetingServicePloxy.HeaderModel remoteHeader = new AdManagerClient.TargetingServicePloxy.HeaderModel();
                TargetingServicePloxy.TargetingModel remoteData = new AdManagerClient.TargetingServicePloxy.TargetingModel();

                // 헤더정보 셋트
                remoteHeader.ClientKey = Header.ClientKey;
                remoteHeader.UserID = Header.UserID;
                remoteHeader.UserLevel = Header.UserLevel;
                remoteHeader.UserClass = Header.UserClass;

                // 호출정보 셋트
                remoteData.ItemNo = targetingModel.ItemNo;
                remoteData.TgtProfile = targetingModel.TgtProfile;
                remoteData.TgtReliablilty = targetingModel.TgtReliablilty;

                // 웹서비스 호출 타임아웃설정
                svc.Timeout = FrameSystem.m_SystemTimeout;

                // 웹서비스 메소드 호출
                remoteData = svc.SetTargetingProfileAdd(remoteHeader, remoteData);

                // 결과코드검사
                if (!remoteData.ResultCD.Equals("0000"))
                {
                    throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
                }

                // 결과 셋트
                targetingModel.ResultCnt = remoteData.ResultCnt;
                targetingModel.ResultCD = remoteData.ResultCD;

                _log.Debug("-----------------------------------------");
                _log.Debug("프로파일 타겟팅 저장 End");
                _log.Debug("-----------------------------------------");
            }
            catch (FrameException fe)
            {
                _log.Warning("-----------------------------------------");
                _log.Warning(this.ToString() + ":SetTargetingProfileAdd():" + fe.ErrCode + ":" + fe.ResultMsg);
                _log.Warning("-----------------------------------------");
                throw fe;
            }
            catch (Exception e)
            {
                _log.Error("-----------------------------------------");
                _log.Exception(e);
                _log.Error("-----------------------------------------");
                throw new FrameException(e.Message);
            }
        }
	}
}
