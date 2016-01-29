// ===============================================================================
// UserUpdate Manager  for Charites Project
//
// UserUpdateManager.cs
//
// 광고매체대행광고주정보 저장 서비스를 호출합니다. 
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
	/// 광고매체대행광고주정보 웹서비스를 호출합니다. 
	/// </summary>
	public class ClientManager : BaseManager
	{
		/// <summary>
		/// 생성자
		/// </summary>
		/// <param name="systemModel"></param>
		/// <param name="commonModel"></param>

		public ClientManager(SystemModel systemModel, CommonModel commonModel) : base(systemModel, commonModel)
		{
			_log = FrameSystem.oLog;
			_module = "USERINFO";
			_Host  = FrameSystem.m_WebServer_Host;
			_Port  = FrameSystem.m_WebServer_Port;
			_Path  = FrameSystem.m_WebServer_App + "/Contract/ClientService.asmx";

		}
        /// <summary>
        /// 광고주코드 콤보
        /// </summary>
        /// <param name="clientModel"></param>

        public void GetAdvertiserList(ClientModel clientModel)
        {
            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug("공통코드조회 Start");
                _log.Debug("-----------------------------------------");

                // 웹서비스 인스턴스 생성
                ClientServicePloxy.ClientService svc = new ClientServicePloxy.ClientService();
			
				// URL의 동적셋트
				svc.Url = _WebServiceUrl;
			
                // 리모트 모델 생성
                ClientServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.ClientServicePloxy.HeaderModel();
                ClientServicePloxy.ClientModel     remoteData   = new AdManagerClient.ClientServicePloxy.ClientModel();

                // 헤더정보 셋트
                remoteHeader.ClientKey     = Header.ClientKey;
                remoteHeader.UserID        = Header.UserID;
				remoteHeader.UserClass	   = Header.UserClass;

                // 호출정보 셋트
                remoteData.AdvertiserCode         = clientModel.AdvertiserCode;				
				
                // 웹서비스 호출 타임아웃설정
                svc.Timeout = FrameSystem.m_SystemTimeout;

                // 웹서비스 메소드 호출
                remoteData = svc.GetAdvertisertList(remoteHeader, remoteData);

                // 결과코드검사
                if(!remoteData.ResultCD.Equals("0000"))
                {
                    throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
                }

                // 결과 셋트
                clientModel.ClientDataSet = remoteData.ClientDataSet.Copy();
                clientModel.ResultCnt   = remoteData.ResultCnt;
                clientModel.ResultCD    = remoteData.ResultCD;

                _log.Debug("-----------------------------------------");
                _log.Debug("공통코드조회 End");
                _log.Debug("-----------------------------------------");
            }
            catch(FrameException fe)
            {
                _log.Warning("-----------------------------------------");
                _log.Warning( this.ToString() + ":GetAdvertiserList():" + fe.ErrCode + ":" + fe.ResultMsg);
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
        /// 매체대행 광고주코드 콤보
        /// </summary>
        /// <param name="clientModel"></param>

        public void GetClientAdvertiserListByCombo(ClientModel clientModel)
        {
            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug("매체대행 광고주코드조회 Start");
                _log.Debug("-----------------------------------------");

                // 웹서비스 인스턴스 생성
                ClientServicePloxy.ClientService svc = new ClientServicePloxy.ClientService();
			
				// URL의 동적셋트
				svc.Url = _WebServiceUrl;
			
                // 리모트 모델 생성
                ClientServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.ClientServicePloxy.HeaderModel();
                ClientServicePloxy.ClientModel     remoteData   = new AdManagerClient.ClientServicePloxy.ClientModel();

                // 헤더정보 셋트
                remoteHeader.ClientKey     = Header.ClientKey;
                remoteHeader.UserID        = Header.UserID;

                // 호출정보 셋트
                remoteData.MediaCode       = clientModel.MediaCode;
                remoteData.RapCode         = clientModel.RapCode;
                remoteData.AgencyCode      = clientModel.AgencyCode;
                
                // 웹서비스 호출 타임아웃설정
                svc.Timeout = FrameSystem.m_SystemTimeout;
				
                // 웹서비스 메소드 호출
                remoteData = svc.GetClientAdvertiserListByCombo(remoteHeader, remoteData);

                // 결과코드검사
                if(!remoteData.ResultCD.Equals("0000"))
                {
                    throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
                }

                // 결과 셋트
                clientModel.ClientDataSet = remoteData.ClientDataSet.Copy();
                clientModel.ResultCnt   = remoteData.ResultCnt;
                clientModel.ResultCD    = remoteData.ResultCD;

                _log.Debug("-----------------------------------------");
                _log.Debug("공통코드조회 End");
                _log.Debug("-----------------------------------------");
            }
            catch(FrameException fe)
            {
                _log.Warning("-----------------------------------------");
                _log.Warning( this.ToString() + ":GetAdvertiserList():" + fe.ErrCode + ":" + fe.ResultMsg);
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
		/// 인서트콤보 필터
		/// </summary>
		/// <param name="clientModel"></param>
		public void GetClientMediaList(ClientModel clientModel)
		{

			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug(" 인서트콤보 필터 Start");
				_log.Debug("-----------------------------------------");
				
				// 웹서비스 인스턴스 생성
				ClientServicePloxy.ClientService svc = new ClientServicePloxy.ClientService();
			
				// URL의 동적셋트
				svc.Url = _WebServiceUrl;
			
				// 리모트 모델 생성
				ClientServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.ClientServicePloxy.HeaderModel();
				ClientServicePloxy.ClientModel remoteData   = new AdManagerClient.ClientServicePloxy.ClientModel();

				// 헤더정보 셋트
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;
				remoteHeader.UserLevel     = Header.UserLevel;

				// 호출정보 셋트
				//remoteData.MediaCode_C       = clientModel.MediaCode_C;
				
                // 웹서비스 호출 타임아웃설정
                svc.Timeout = FrameSystem.m_SystemTimeout;

				// 웹서비스 메소드 호출
				remoteData = svc.GetClientMediaList(remoteHeader, remoteData);

				// 결과코드검사
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// 결과 셋트
				clientModel.ClientDataSet = remoteData.ClientDataSet.Copy();
				clientModel.ResultCnt   = remoteData.ResultCnt;
				clientModel.ResultCD    = remoteData.ResultCD;				
				_log.Debug(clientModel.ResultCD);

				_log.Debug("-----------------------------------------");
				_log.Debug(" 인서트콤보 필터 End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":GetClientComboList():" + fe.ErrCode + ":" + fe.ResultMsg);
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
		/// 인서트콤보 필터
		/// </summary>
		/// <param name="clientModel"></param>
		public void GetClientRapList(ClientModel clientModel)
		{

			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug(" 인서트콤보 필터 Start");
				_log.Debug("-----------------------------------------");
				
				// 웹서비스 인스턴스 생성
				ClientServicePloxy.ClientService svc = new ClientServicePloxy.ClientService();
			
				// URL의 동적셋트
				svc.Url = _WebServiceUrl;
			
				// 리모트 모델 생성
				ClientServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.ClientServicePloxy.HeaderModel();
				ClientServicePloxy.ClientModel remoteData   = new AdManagerClient.ClientServicePloxy.ClientModel();

				// 헤더정보 셋트
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;
				remoteHeader.UserLevel     = Header.UserLevel;

				// 호출정보 셋트
				
				remoteData.MediaCode_C       = clientModel.MediaCode_C;
				
                // 웹서비스 호출 타임아웃설정
                svc.Timeout = FrameSystem.m_SystemTimeout;

				// 웹서비스 메소드 호출
				remoteData = svc.GetClientRapList(remoteHeader, remoteData);

				// 결과코드검사
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// 결과 셋트
				clientModel.ClientDataSet = remoteData.ClientDataSet.Copy();
				clientModel.ResultCnt   = remoteData.ResultCnt;
				clientModel.ResultCD    = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug(" 인서트콤보 필터 End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":GetClientComboList():" + fe.ErrCode + ":" + fe.ResultMsg);
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
		/// 인서트콤보 필터
		/// </summary>
		/// <param name="clientModel"></param>
		public void GetClientAgencyList(ClientModel clientModel)
		{

			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug(" 인서트콤보 필터 Start");
				_log.Debug("-----------------------------------------");
				
				// 웹서비스 인스턴스 생성
				ClientServicePloxy.ClientService svc = new ClientServicePloxy.ClientService();
			
				// URL의 동적셋트
				svc.Url = _WebServiceUrl;
			
				// 리모트 모델 생성
				ClientServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.ClientServicePloxy.HeaderModel();
				ClientServicePloxy.ClientModel remoteData   = new AdManagerClient.ClientServicePloxy.ClientModel();

				// 헤더정보 셋트
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;
				remoteHeader.UserLevel     = Header.UserLevel;

				// 호출정보 셋트
				remoteData.MediaCode_C       = clientModel.MediaCode_C;
				remoteData.RapCode_C = clientModel.RapCode_C;				
				
                // 웹서비스 호출 타임아웃설정
                svc.Timeout = FrameSystem.m_SystemTimeout;

				// 웹서비스 메소드 호출
				remoteData = svc.GetClientAgencyList(remoteHeader, remoteData);

				// 결과코드검사
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// 결과 셋트
				clientModel.ClientDataSet = remoteData.ClientDataSet.Copy();
				clientModel.ResultCnt   = remoteData.ResultCnt;
				clientModel.ResultCD    = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug(" 인서트콤보 필터 End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":GetClientComboList():" + fe.ErrCode + ":" + fe.ResultMsg);
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
		/// 인서트콤보 필터
		/// </summary>
		/// <param name="clientModel"></param>
		public void GetClientAdvertiserList(ClientModel clientModel)
		{

			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug(" 인서트콤보 필터 Start");
				_log.Debug("-----------------------------------------");
				
				// 웹서비스 인스턴스 생성
				ClientServicePloxy.ClientService svc = new ClientServicePloxy.ClientService();
			
				// URL의 동적셋트
				svc.Url = _WebServiceUrl;
			
				// 리모트 모델 생성
				ClientServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.ClientServicePloxy.HeaderModel();
				ClientServicePloxy.ClientModel remoteData   = new AdManagerClient.ClientServicePloxy.ClientModel();

				// 헤더정보 셋트
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;
				remoteHeader.UserLevel     = Header.UserLevel;
						
		        // 호출정보 셋트
                remoteData.MediaCode = clientModel.MediaCode;
                remoteData.RapCode = clientModel.RapCode;
                remoteData.AgencyCode = clientModel.AgencyCode;
				remoteData.SearchRap = clientModel.SearchRap;
				remoteData.SearchKey = clientModel.SearchKey;

                // 웹서비스 호출 타임아웃설정
                svc.Timeout = FrameSystem.m_SystemTimeout;
				
				// 웹서비스 메소드 호출
				remoteData = svc.GetClientAdvertiserList(remoteHeader, remoteData);

				// 결과코드검사
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// 결과 셋트
				clientModel.ClientDataSet = remoteData.ClientDataSet.Copy();
				clientModel.ResultCnt   = remoteData.ResultCnt;
				clientModel.ResultCD    = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug(" 인서트콤보 필터 End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":GetClientComboList():" + fe.ErrCode + ":" + fe.ResultMsg);
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
		/// 광고매체대행광고주정보조회
		/// </summary>
		/// <param name="clientModel"></param>
		public void GetClientList(ClientModel clientModel)
		{

			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("광고매체대행광고주목록조회 Start");
				_log.Debug("-----------------------------------------");
				
				// 웹서비스 인스턴스 생성
				ClientServicePloxy.ClientService svc = new ClientServicePloxy.ClientService();
			
				// URL의 동적셋트
				svc.Url = _WebServiceUrl;
			
				// 리모트 모델 생성
				ClientServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.ClientServicePloxy.HeaderModel();
				ClientServicePloxy.ClientModel remoteData   = new AdManagerClient.ClientServicePloxy.ClientModel();

				// 헤더정보 셋트
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;
				remoteHeader.UserLevel     = Header.UserLevel;
				remoteHeader.UserClass	   = Header.UserClass;

				// 호출정보 셋트
				remoteData.SearchKey       = clientModel.SearchKey;
				remoteData.SearchMediaName = clientModel.SearchMediaName;
				remoteData.SearchRapName = clientModel.SearchRapName;
                remoteData.SearchAdvertiserName = clientModel.SearchAdvertiserName;
				remoteData.SearchMediaAgency = clientModel.SearchMediaAgency;			
				remoteData.SearchchkAdState_10	 = clientModel.SearchchkAdState_10; 

                // 웹서비스 호출 타임아웃설정
                svc.Timeout = FrameSystem.m_SystemTimeout;

				// 웹서비스 메소드 호출
				remoteData = svc.GetClientList(remoteHeader, remoteData);

				// 결과코드검사
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}
				
				// 결과 셋트
				clientModel.ClientDataSet = remoteData.ClientDataSet.Copy();
				clientModel.ResultCnt   = remoteData.ResultCnt;
				clientModel.ResultCD    = remoteData.ResultCD;			
				
				_log.Debug("-----------------------------------------");
				_log.Debug("광고매체대행광고주목록조회 End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":GetUserList():" + fe.ErrCode + ":" + fe.ResultMsg);
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
		/// Service 호출을 위한 메소드
		/// </summary>
		public bool GetUserDetail(BaseModel baseModel)
		{
			
			_log.Debug("-----------------------------------------");
			_log.Debug( this.ToString() + " Start");
			_log.Debug("-----------------------------------------");

			_log.Debug("-----------------------------------------");
			_log.Debug( this.ToString() + " End");
			_log.Debug("-----------------------------------------");

			return true;
		}

		/// <summary>
		/// 광고매체대행광고주정보 수정
		/// </summary>
		/// <param name="clientModel"></param>
		public void SetClientUpdate(ClientModel clientModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("광고매체대행광고주정보수정 Start");
				_log.Debug("-----------------------------------------");


				//입력데이터의 Validation 검사
				if(clientModel.Comment.Length > 50) 
				{
					throw new FrameException("비고는 50Bytes를 초과할 수 없습니다.");
				}				

				// 웹서비스 인스턴스 생성
				ClientServicePloxy.ClientService svc = new ClientServicePloxy.ClientService();
			
				// URL의 동적셋트
				svc.Url = _WebServiceUrl;
			
				// 리모트 모델 생성
				ClientServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.ClientServicePloxy.HeaderModel();
				ClientServicePloxy.ClientModel remoteData   = new AdManagerClient.ClientServicePloxy.ClientModel();

				// 헤더정보 셋트
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;

				// 호출정보셋트
				remoteData.MediaCode       = clientModel.MediaCode_C;
				remoteData.RapCode       = clientModel.RapCode_C;
				remoteData.AgencyCode       = clientModel.AgencyCode_C;
				remoteData.AdvertiserCode       = clientModel.AdvertiserCode_C;				
				remoteData.Comment       = clientModel.Comment;
				remoteData.UseYn     = clientModel.UseYn;				
					
                // 웹서비스 호출 타임아웃설정
                svc.Timeout = FrameSystem.m_SystemTimeout;
			
				// 웹서비스 메소드 호출
				remoteData = svc.SetClientUpdate(remoteHeader, remoteData);

				// 결과코드검사
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// 결과 셋트
				clientModel.ResultCnt   = remoteData.ResultCnt;
				clientModel.ResultCD    = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("광고매체대행광고주정보수정 End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":SetUserUpdate():" + fe.ErrCode + ":" + fe.ResultMsg);
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
		/// 광고매체대행광고주추가
		/// </summary>
		/// <param name="clientModel"></param>
		/// <returns></returns>
		public void SetClientAdd(ClientModel clientModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("광고매체대행광고주추가 Start");
				_log.Debug("-----------------------------------------");
				
				if(clientModel.Comment.Length > 50) 
				{
					throw new FrameException("비고는 50Bytes를 초과할 수 없습니다.");
				}			
				
				// 웹서비스 인스턴스 생성
				ClientServicePloxy.ClientService svc = new ClientServicePloxy.ClientService();
			
				// URL의 동적셋트
				svc.Url = _WebServiceUrl;
				
				// 리모트 모델 생성
				ClientServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.ClientServicePloxy.HeaderModel();
				ClientServicePloxy.ClientModel remoteData   = new AdManagerClient.ClientServicePloxy.ClientModel();

				// 헤더정보 셋트
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;

				// 호출정보셋트
				remoteData.MediaCode       = clientModel.MediaCode_C;
				remoteData.RapCode       = clientModel.RapCode_C;
				remoteData.AgencyCode       = clientModel.AgencyCode_C;
				remoteData.AdvertiserCode       = clientModel.AdvertiserCode_C;				
				remoteData.Comment       = clientModel.Comment;				
				remoteData.UseYn     = clientModel.UseYn;				
				
                // 웹서비스 호출 타임아웃설정
                svc.Timeout = FrameSystem.m_SystemTimeout;
	
				// 웹서비스 메소드 호출
				remoteData = svc.SetClientCreate(remoteHeader, remoteData);

				// 결과코드검사
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// 결과 셋트
				clientModel.ResultCnt   = remoteData.ResultCnt;
				clientModel.ResultCD    = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("광고매체대행광고주추가 End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				clientModel.ResultCD    = "3101";
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":SetUserCreate():" + fe.ErrCode + ":" + fe.ResultMsg);
				_log.Warning("-----------------------------------------");
				throw fe;
			}
			catch(Exception e)
			{
				clientModel.ResultCD    = "3101";
				_log.Error("-----------------------------------------");
				_log.Exception(e);
				_log.Error("-----------------------------------------");
				throw new FrameException(e.Message);
			}
		}

		
		/// <summary>
		/// 광고매체대행광고주 삭제
		/// </summary>
		/// <param name="baseModel"></param>
		public void SetClientDelete(ClientModel clientModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("광고매체대행광고주삭제 Start");
				_log.Debug("-----------------------------------------");

				// 웹서비스 인스턴스 생성
				ClientServicePloxy.ClientService svc = new ClientServicePloxy.ClientService();
			
				// URL의 동적셋트
				svc.Url = _WebServiceUrl;
				
				// 리모트 모델 생성
				ClientServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.ClientServicePloxy.HeaderModel();
				ClientServicePloxy.ClientModel remoteData   = new AdManagerClient.ClientServicePloxy.ClientModel();

				// 헤더정보 셋트
				remoteHeader.ClientKey  = Header.ClientKey;
				remoteHeader.UserID     = Header.UserID;

				// 호출정보셋트
				remoteData.MediaCode       = clientModel.MediaCode;
				remoteData.RapCode       = clientModel.RapCode;
				remoteData.AgencyCode       = clientModel.AgencyCode;
				remoteData.AdvertiserCode       = clientModel.AdvertiserCode;				
					
                // 웹서비스 호출 타임아웃설정
                svc.Timeout = FrameSystem.m_SystemTimeout;

				// 웹서비스 메소드 호출
				remoteData = svc.SetClientDelete(remoteHeader, remoteData);

				// 결과코드검사
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// 결과 셋트
				clientModel.ResultCnt   = remoteData.ResultCnt;
				clientModel.ResultCD    = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("광고매체대행광고주삭제 End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":SetClientDelete():" + fe.ErrCode + ":" + fe.ResultMsg);
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

	}
}
