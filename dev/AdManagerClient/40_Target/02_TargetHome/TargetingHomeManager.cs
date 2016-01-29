// ===============================================================================
// Targeting Manager  for Charites Project
//
// TargetingHomeManager.cs
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
	public class TargetingHomeManager : BaseManager
	{
		/// <summary>
		/// 생성자
		/// </summary>
		/// <param name="systemModel"></param>
		/// <param name="commonModel"></param>

		public TargetingHomeManager(SystemModel systemModel, CommonModel commonModel) : base(systemModel, commonModel)
		{
			_log = FrameSystem.oLog;
			_module = "TargetingHome";
			_Host  = FrameSystem.m_WebServer_Host;
			_Port  = FrameSystem.m_WebServer_Port;
			_Path  = FrameSystem.m_WebServer_App + "/Target/TargetingHomeService.asmx";
		}

		/// <summary>
		/// 타겟팅 목록조회
		/// </summary>
		/// <param name="targetingHomeModel"></param>
		public void GetTargetingList(TargetingHomeModel targetingHomeModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("타겟팅 목록조회 Start");
				_log.Debug("-----------------------------------------");
				
				// 웹서비스 인스턴스 생성
				TargetingHomeServicePloxy.TargetingHomeService svc = new TargetingHomeServicePloxy.TargetingHomeService();
			
				// 웹서비스 URL동적 생성
				svc.Url = _WebServiceUrl;			
			
				// 리모트 모델 생성
				TargetingHomeServicePloxy.HeaderModel      remoteHeader = new AdManagerClient.TargetingHomeServicePloxy.HeaderModel();
				TargetingHomeServicePloxy.TargetingHomeModel remoteData   = new AdManagerClient.TargetingHomeServicePloxy.TargetingHomeModel();

				// 헤더정보 셋트
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;
				remoteHeader.UserLevel     = Header.UserLevel;
				remoteHeader.UserClass     = Header.UserClass;

				// 호출정보 셋트
				remoteData.SearchKey             =  targetingHomeModel.SearchKey;               
				remoteData.SearchMediaCode		 =  targetingHomeModel.SearchMediaCode;	  
				remoteData.SearchRapCode		 =  targetingHomeModel.SearchRapCode;       
				remoteData.SearchAgencyCode	     =  targetingHomeModel.SearchAgencyCode;    
				remoteData.SearchAdvertiserCode  =  targetingHomeModel.SearchAdvertiserCode;
				remoteData.SearchContractState	 =  targetingHomeModel.SearchContractState;
				remoteData.SearchAdType  		 =  targetingHomeModel.SearchAdType;       
				remoteData.SearchchkAdState_20	 =  targetingHomeModel.SearchchkAdState_20; 
				remoteData.SearchchkAdState_30	 =  targetingHomeModel.SearchchkAdState_30; 
				remoteData.SearchchkAdState_40	 =  targetingHomeModel.SearchchkAdState_40; 
				remoteData.SearchchkTimeY		 =  targetingHomeModel.SearchchkTimeY; 
				remoteData.SearchchkTimeN		 =  targetingHomeModel.SearchchkTimeN; 
                
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
				targetingHomeModel.TargetingDataSet = remoteData.TargetingDataSet.Copy();
				targetingHomeModel.ResultCnt   = remoteData.ResultCnt;
				targetingHomeModel.ResultCD    = remoteData.ResultCD;

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
        /// 타겟팅 목록조회
        /// </summary>
        /// <param name="targetingHomeModel"></param>
        public void GetTargetingList2(TargetingHomeModel targetingHomeModel)
        {
            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug("타겟팅 목록조회 Start");
                _log.Debug("-----------------------------------------");

                // 웹서비스 인스턴스 생성
                TargetingHomeServicePloxy.TargetingHomeService svc = new TargetingHomeServicePloxy.TargetingHomeService();

                // 웹서비스 URL동적 생성
                svc.Url = _WebServiceUrl;

                // 리모트 모델 생성
                TargetingHomeServicePloxy.HeaderModel remoteHeader = new AdManagerClient.TargetingHomeServicePloxy.HeaderModel();
                TargetingHomeServicePloxy.TargetingHomeModel remoteData = new AdManagerClient.TargetingHomeServicePloxy.TargetingHomeModel();

                // 헤더정보 셋트
                remoteHeader.ClientKey = Header.ClientKey;
                remoteHeader.UserID = Header.UserID;
                remoteHeader.UserLevel = Header.UserLevel;
                remoteHeader.UserClass = Header.UserClass;

                // 호출정보 셋트
                remoteData.SearchKey = targetingHomeModel.SearchKey;
                remoteData.SearchMediaCode = targetingHomeModel.SearchMediaCode;
                remoteData.SearchRapCode = targetingHomeModel.SearchRapCode;
                remoteData.SearchAgencyCode = targetingHomeModel.SearchAgencyCode;
                remoteData.SearchAdvertiserCode = targetingHomeModel.SearchAdvertiserCode;
                remoteData.SearchContractState = targetingHomeModel.SearchContractState;
                remoteData.SearchAdType = targetingHomeModel.SearchAdType;
                remoteData.SearchchkAdState_20 = targetingHomeModel.SearchchkAdState_20;
                remoteData.SearchchkAdState_30 = targetingHomeModel.SearchchkAdState_30;
                remoteData.SearchchkAdState_40 = targetingHomeModel.SearchchkAdState_40;
                remoteData.SearchchkTimeY = targetingHomeModel.SearchchkTimeY;
                remoteData.SearchchkTimeN = targetingHomeModel.SearchchkTimeN;

                // 웹서비스 호출 타임아웃설정
                svc.Timeout = FrameSystem.m_SystemTimeout;
                // 웹서비스 메소드 호출
                remoteData = svc.GetTargetingList2(remoteHeader, remoteData);

                // 결과코드검사
                if (!remoteData.ResultCD.Equals("0000"))
                {
                    throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
                }

                // 결과 셋트
                targetingHomeModel.TargetingDataSet = remoteData.TargetingDataSet.Copy();
                targetingHomeModel.ResultCnt = remoteData.ResultCnt;
                targetingHomeModel.ResultCD = remoteData.ResultCD;

                _log.Debug("-----------------------------------------");
                _log.Debug("타겟팅 목록조회 End");
                _log.Debug("-----------------------------------------");
            }
            catch (FrameException fe)
            {
                _log.Warning("-----------------------------------------");
                _log.Warning(this.ToString() + ":GetTargetingList():" + fe.ErrCode + ":" + fe.ResultMsg);
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
		/// 타겟팅 목록조회
		/// </summary>
		/// <param name="targetingHomeModel"></param>
		public void GetCollectionList(TargetingHomeModel targetingHomeModel)
		{

			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("타겟군 목록조회 Start");
				_log.Debug("-----------------------------------------");
				
				// 웹서비스 인스턴스 생성
				TargetingHomeServicePloxy.TargetingHomeService svc = new TargetingHomeServicePloxy.TargetingHomeService();
			
				// 웹서비스 URL동적 생성
				svc.Url = _WebServiceUrl;			
			
				// 리모트 모델 생성
				TargetingHomeServicePloxy.HeaderModel      remoteHeader = new AdManagerClient.TargetingHomeServicePloxy.HeaderModel();
				TargetingHomeServicePloxy.TargetingHomeModel remoteData   = new AdManagerClient.TargetingHomeServicePloxy.TargetingHomeModel();

				// 헤더정보 셋트
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;
				remoteHeader.UserLevel     = Header.UserLevel;
				remoteHeader.UserClass     = Header.UserClass;
                
				// 웹서비스 호출 타임아웃설정
				svc.Timeout = FrameSystem.m_SystemTimeout;
				// 웹서비스 메소드 호출
				remoteData = svc.GetCollectionList(remoteHeader, remoteData);

				// 결과코드검사
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// 결과 셋트
				targetingHomeModel.CollectionsDataSet = remoteData.CollectionsDataSet.Copy();
				targetingHomeModel.ResultCnt   = remoteData.ResultCnt;
				targetingHomeModel.ResultCD    = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("타겟군 목록조회 End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":GetTargetCollectionList():" + fe.ErrCode + ":" + fe.ResultMsg);
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
		/// <param name="targetingHomeModel"></param>
		public void GetTargetingDetail(TargetingHomeModel targetingHomeModel)
		{

			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("타겟팅 상세 조회 Start");
				_log.Debug("-----------------------------------------");
				
				// 웹서비스 인스턴스 생성
				TargetingHomeServicePloxy.TargetingHomeService svc = new TargetingHomeServicePloxy.TargetingHomeService();
			
				// 웹서비스 URL동적 생성
				svc.Url = _WebServiceUrl;			
			
				// 리모트 모델 생성
				TargetingHomeServicePloxy.HeaderModel      remoteHeader = new AdManagerClient.TargetingHomeServicePloxy.HeaderModel();
				TargetingHomeServicePloxy.TargetingHomeModel remoteData   = new AdManagerClient.TargetingHomeServicePloxy.TargetingHomeModel();

				// 헤더정보 셋트
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;
				remoteHeader.UserLevel     = Header.UserLevel;
				remoteHeader.UserClass     = Header.UserClass;

				// 호출정보 셋트
				remoteData.ItemNo             =  targetingHomeModel.ItemNo;               
                
				// 웹서비스 호출 타임아웃설정
				svc.Timeout = FrameSystem.m_SystemTimeout;
				// 웹서비스 메소드 호출
				remoteData = svc.GetTargetingDetail(remoteHeader, remoteData);

				// 결과코드검사
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// 결과 셋트
				targetingHomeModel.DetailDataSet = remoteData.DetailDataSet.Copy();
				targetingHomeModel.ResultCnt   = remoteData.ResultCnt;
				targetingHomeModel.ResultCD    = remoteData.ResultCD;

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
        /// <param name="targetingHomeModel"></param>
        public void GetTargetingDetail2(TargetingHomeModel targetingHomeModel)
        {

            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug("타겟팅 상세 조회 Start");
                _log.Debug("-----------------------------------------");

                // 웹서비스 인스턴스 생성
                TargetingHomeServicePloxy.TargetingHomeService svc = new TargetingHomeServicePloxy.TargetingHomeService();

                // 웹서비스 URL동적 생성
                svc.Url = _WebServiceUrl;

                // 리모트 모델 생성
                TargetingHomeServicePloxy.HeaderModel remoteHeader = new AdManagerClient.TargetingHomeServicePloxy.HeaderModel();
                TargetingHomeServicePloxy.TargetingHomeModel remoteData = new AdManagerClient.TargetingHomeServicePloxy.TargetingHomeModel();

                // 헤더정보 셋트
                remoteHeader.ClientKey = Header.ClientKey;
                remoteHeader.UserID = Header.UserID;
                remoteHeader.UserLevel = Header.UserLevel;
                remoteHeader.UserClass = Header.UserClass;

                // 호출정보 셋트
                remoteData.ItemNo = targetingHomeModel.ItemNo;

                // 웹서비스 호출 타임아웃설정
                svc.Timeout = FrameSystem.m_SystemTimeout;
                // 웹서비스 메소드 호출
                remoteData = svc.GetTargetingDetail2(remoteHeader, remoteData);

                // 결과코드검사
                if (!remoteData.ResultCD.Equals("0000"))
                {
                    throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
                }

                // 결과 셋트
                targetingHomeModel.DetailDataSet = remoteData.DetailDataSet.Copy();
                targetingHomeModel.ResultCnt = remoteData.ResultCnt;
                targetingHomeModel.ResultCD = remoteData.ResultCD;

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
		/// 타겟팅 상세정보 저장
		/// </summary>
		/// <param name="targetingHomeModel"></param>
		public void SetTargetingDetailUpdate(TargetingHomeModel targetingHomeModel)
		{

			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("타겟팅 상세정보 저장 Start");
				_log.Debug("-----------------------------------------");
				
				// 웹서비스 인스턴스 생성
				TargetingHomeServicePloxy.TargetingHomeService svc = new TargetingHomeServicePloxy.TargetingHomeService();
			
				// 웹서비스 URL동적 생성
				svc.Url = _WebServiceUrl;			
			
				// 리모트 모델 생성
				TargetingHomeServicePloxy.HeaderModel      remoteHeader = new AdManagerClient.TargetingHomeServicePloxy.HeaderModel();
				TargetingHomeServicePloxy.TargetingHomeModel remoteData   = new AdManagerClient.TargetingHomeServicePloxy.TargetingHomeModel();

				// 헤더정보 셋트
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;
				remoteHeader.UserLevel     = Header.UserLevel;
				remoteHeader.UserClass     = Header.UserClass;

				// 호출정보 셋트
				remoteData.ItemNo          = targetingHomeModel.ItemNo;
				remoteData.ItemName        = targetingHomeModel.ItemName;
				remoteData.ContractAmt     = targetingHomeModel.ContractAmt;
				remoteData.PriorityCd      = targetingHomeModel.PriorityCd;
				remoteData.AmtControlYn    = targetingHomeModel.AmtControlYn;
				remoteData.AmtControlRate  = targetingHomeModel.AmtControlRate;
				remoteData.TgtRegion1Yn    = targetingHomeModel.TgtRegion1Yn;
				remoteData.TgtRegion1      = targetingHomeModel.TgtRegion1;
				remoteData.TgtTimeYn       = targetingHomeModel.TgtTimeYn;
				remoteData.TgtTime         = targetingHomeModel.TgtTime;
				remoteData.TgtAgeYn        = targetingHomeModel.TgtAgeYn;
				remoteData.TgtAge          = targetingHomeModel.TgtAge;
				remoteData.TgtAgeBtnYn     = targetingHomeModel.TgtAgeBtnYn;
				remoteData.TgtAgeBtnBegin  = targetingHomeModel.TgtAgeBtnBegin;
				remoteData.TgtAgeBtnEnd    = targetingHomeModel.TgtAgeBtnEnd;
				remoteData.TgtSexYn        = targetingHomeModel.TgtSexYn;
				remoteData.TgtSexMan       = targetingHomeModel.TgtSexMan;
				remoteData.TgtSexWoman     = targetingHomeModel.TgtSexWoman;
//				remoteData.TgtRateYn       = targetingHomeModel.TgtRateYn;
//				remoteData.TgtRate         = targetingHomeModel.TgtRate;
				remoteData.TgtWeekYn       = targetingHomeModel.TgtWeekYn;
				remoteData.TgtWeek         = targetingHomeModel.TgtWeek;
				remoteData.TgtCollectionYn       = targetingHomeModel.TgtCollectionYn;
				remoteData.TgtCollection         = targetingHomeModel.TgtCollection;

                remoteData.TgtStbModelYn    = targetingHomeModel.TgtStbModelYn;
                remoteData.TgtStbModel      = targetingHomeModel.TgtStbModel;
                remoteData.TgtPocYn         = targetingHomeModel.TgtPocYn;
                remoteData.TgtPoc           = targetingHomeModel.TgtPoc;

				// 웹서비스 호출 타임아웃설정
				svc.Timeout = FrameSystem.m_SystemTimeout;

				// 웹서비스 메소드 호출
				remoteData = svc.SetTargetingDetailUpdate(remoteHeader, remoteData);

				// 결과코드검사
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// 결과 셋트
				targetingHomeModel.ResultCnt   = remoteData.ResultCnt;
				targetingHomeModel.ResultCD    = remoteData.ResultCD;

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
		/// 타겟팅지역구분조회
		/// </summary>
		/// <param name="targetingHomeModel"></param>
		public void GetRegionList(TargetingHomeModel targetingHomeModel)
		{

			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("타겟팅지역구분조회 Start");
				_log.Debug("-----------------------------------------");
				
				// 웹서비스 인스턴스 생성
				TargetingHomeServicePloxy.TargetingHomeService svc = new TargetingHomeServicePloxy.TargetingHomeService();
			
				// 웹서비스 URL동적 생성
				svc.Url = _WebServiceUrl;			
			
				// 리모트 모델 생성
				TargetingHomeServicePloxy.HeaderModel      remoteHeader = new AdManagerClient.TargetingHomeServicePloxy.HeaderModel();
				TargetingHomeServicePloxy.TargetingHomeModel remoteData   = new AdManagerClient.TargetingHomeServicePloxy.TargetingHomeModel();

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
				targetingHomeModel.RegionDataSet = remoteData.RegionDataSet.Copy();
				targetingHomeModel.ResultCnt     = remoteData.ResultCnt;
				targetingHomeModel.ResultCD      = remoteData.ResultCD;

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
		/// <param name="targetingHomeModel"></param>
		public void GetAgeList(TargetingHomeModel targetingHomeModel)
		{

			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("타겟팅연령대조회 Start");
				_log.Debug("-----------------------------------------");
				
				// 웹서비스 인스턴스 생성
				TargetingHomeServicePloxy.TargetingHomeService svc = new TargetingHomeServicePloxy.TargetingHomeService();
			
				// 웹서비스 URL동적 생성
				svc.Url = _WebServiceUrl;			
			
				// 리모트 모델 생성
				TargetingHomeServicePloxy.HeaderModel      remoteHeader = new AdManagerClient.TargetingHomeServicePloxy.HeaderModel();
				TargetingHomeServicePloxy.TargetingHomeModel remoteData   = new AdManagerClient.TargetingHomeServicePloxy.TargetingHomeModel();

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
				targetingHomeModel.AgeDataSet = remoteData.AgeDataSet.Copy();
				targetingHomeModel.ResultCnt     = remoteData.ResultCnt;
				targetingHomeModel.ResultCD      = remoteData.ResultCD;

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
        public void GetTargetingCollectionList(TargetingHomeModel targetingHomeModel)
        {

            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug("설정타겟군 목록조회 Start");
                _log.Debug("-----------------------------------------");

                // 웹서비스 인스턴스 생성
                TargetingHomeServicePloxy.TargetingHomeService svc = new TargetingHomeServicePloxy.TargetingHomeService();

                // 웹서비스 URL동적 생성
                svc.Url = _WebServiceUrl;

                // 리모트 모델 생성
                TargetingHomeServicePloxy.HeaderModel remoteHeader = new AdManagerClient.TargetingHomeServicePloxy.HeaderModel();
                TargetingHomeServicePloxy.TargetingHomeModel remoteData = new AdManagerClient.TargetingHomeServicePloxy.TargetingHomeModel();

                // 헤더정보 셋트
                remoteHeader.ClientKey = Header.ClientKey;
                remoteHeader.UserID = Header.UserID;
                remoteHeader.UserLevel = Header.UserLevel;
                remoteHeader.UserClass = Header.UserClass;

                // 호출정보 셋트
                remoteData.ItemNo = targetingHomeModel.ItemNo;

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
                targetingHomeModel.TargetingCollectionDataSet = remoteData.TargetingCollectionDataSet.Copy();
                targetingHomeModel.ResultCnt = remoteData.ResultCnt;
                targetingHomeModel.ResultCD = remoteData.ResultCD;

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
        public void SetTargetingCollectionAdd(TargetingHomeModel targetingHomeModel)
        {

            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug("고객군타겟팅 추가 Start");
                _log.Debug("-----------------------------------------");

                // 웹서비스 인스턴스 생성
                TargetingHomeServicePloxy.TargetingHomeService svc = new TargetingHomeServicePloxy.TargetingHomeService();

                // 웹서비스 URL동적 생성
                svc.Url = _WebServiceUrl;

                // 리모트 모델 생성
                TargetingHomeServicePloxy.HeaderModel remoteHeader = new AdManagerClient.TargetingHomeServicePloxy.HeaderModel();
                TargetingHomeServicePloxy.TargetingHomeModel remoteData = new AdManagerClient.TargetingHomeServicePloxy.TargetingHomeModel();

                // 헤더정보 셋트
                remoteHeader.ClientKey = Header.ClientKey;
                remoteHeader.UserID = Header.UserID;
                remoteHeader.UserLevel = Header.UserLevel;
                remoteHeader.UserClass = Header.UserClass;

                // 호출정보 셋트
                remoteData.ItemNo = targetingHomeModel.ItemNo;
                remoteData.CollectionCode = targetingHomeModel.CollectionCode;
				remoteData.TgtCollectionYn = targetingHomeModel.TgtCollectionYn;

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
                targetingHomeModel.ResultCnt = remoteData.ResultCnt;
                targetingHomeModel.ResultCD = remoteData.ResultCD;

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
        public void SetTargetingCollectionDelete(TargetingHomeModel targetingHomeModel)
        {

            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug("고객군타겟팅 삭제 Start");
                _log.Debug("-----------------------------------------");

                // 웹서비스 인스턴스 생성
                TargetingHomeServicePloxy.TargetingHomeService svc = new TargetingHomeServicePloxy.TargetingHomeService();

                // 웹서비스 URL동적 생성
                svc.Url = _WebServiceUrl;

                // 리모트 모델 생성
                TargetingHomeServicePloxy.HeaderModel remoteHeader = new AdManagerClient.TargetingHomeServicePloxy.HeaderModel();
                TargetingHomeServicePloxy.TargetingHomeModel remoteData = new AdManagerClient.TargetingHomeServicePloxy.TargetingHomeModel();

                // 헤더정보 셋트
                remoteHeader.ClientKey = Header.ClientKey;
                remoteHeader.UserID = Header.UserID;
                remoteHeader.UserLevel = Header.UserLevel;
                remoteHeader.UserClass = Header.UserClass;

                // 호출정보 셋트
                remoteData.ItemNo = targetingHomeModel.ItemNo;
                remoteData.CollectionCode = targetingHomeModel.CollectionCode;

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
                targetingHomeModel.ResultCnt = remoteData.ResultCnt;
                targetingHomeModel.ResultCD = remoteData.ResultCD;

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

	}
}
