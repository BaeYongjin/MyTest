// ===============================================================================
// UserUpdate Manager  for Charites Project
//
// UserUpdateManager.cs
//
// 사용자정보 저장 서비스를 호출합니다. 
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
	/// 사용자정보 웹서비스를 호출합니다. 
	/// </summary>
	public class MediaAgencyManager : BaseManager
	{
		/// <summary>
		/// 생성자
		/// </summary>
		/// <param name="systemModel"></param>
		/// <param name="commonModel"></param>

		public MediaAgencyManager(SystemModel systemModel, CommonModel commonModel) : base(systemModel, commonModel)
		{
			_log = FrameSystem.oLog;
			_module = "USERINFO";
			_Host  = FrameSystem.m_WebServer_Host;
			_Port  = FrameSystem.m_WebServer_Port;
			_Path  = FrameSystem.m_WebServer_App + "/Contract/MediaAgencyService.asmx";
		}

		/// <summary>
		/// 사용자정보조회
		/// </summary>
		/// <param name="mediaAgencyModel"></param>
		public void GetMediaAgencyList(MediaAgencyModel mediaAgencyModel)
		{

			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("사용자목록조회 Start");
				_log.Debug("-----------------------------------------");
				
				// 웹서비스 인스턴스 생성
				MediaAgencyServicePloxy.MediaAgencyService svc = new MediaAgencyServicePloxy.MediaAgencyService();
			
				// URL의 동적셋트
				svc.Url = _WebServiceUrl;
			
				// 리모트 모델 생성
				MediaAgencyServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.MediaAgencyServicePloxy.HeaderModel();
				MediaAgencyServicePloxy.MediaAgencyModel remoteData   = new AdManagerClient.MediaAgencyServicePloxy.MediaAgencyModel();

				// 헤더정보 셋트
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;
				remoteHeader.UserLevel     = Header.UserLevel;
				remoteHeader.UserClass	   = Header.UserClass;

				// 호출정보 셋트
				remoteData.SearchKey       = mediaAgencyModel.SearchKey;
				remoteData.SearchMediaName = mediaAgencyModel.SearchMediaName;
				remoteData.SearchRapName = mediaAgencyModel.SearchRapName;
				remoteData.SearchMediaAgency = mediaAgencyModel.SearchMediaAgency;
				remoteData.SearchchkAdState_10	 = mediaAgencyModel.SearchchkAdState_10; 
				
                // 웹서비스 호출 타임아웃설정
                svc.Timeout = FrameSystem.m_SystemTimeout;

				// 웹서비스 메소드 호출
				remoteData = svc.GetMediaAgencyList(remoteHeader, remoteData);

				// 결과코드검사
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// 결과 셋트
				mediaAgencyModel.MediaAgencyDataSet = remoteData.MediaAgencyDataSet.Copy();
				mediaAgencyModel.ResultCnt   = remoteData.ResultCnt;
				mediaAgencyModel.ResultCD    = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("사용자목록조회 End");
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
		/// 매체대행사팝업에서목록조회(매체대행사별 광고주관리에서 사용)
		/// </summary>
		/// <param name="mediaAgencyModel"></param>
		public void GetMediaAgencyPop(MediaAgencyModel mediaAgencyModel)
		{

			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("사용자목록조회 Start");
				_log.Debug("-----------------------------------------");
				
				// 웹서비스 인스턴스 생성
				MediaAgencyServicePloxy.MediaAgencyService svc = new MediaAgencyServicePloxy.MediaAgencyService();
			
				// URL의 동적셋트
				svc.Url = _WebServiceUrl;
			
				// 리모트 모델 생성
				MediaAgencyServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.MediaAgencyServicePloxy.HeaderModel();
				MediaAgencyServicePloxy.MediaAgencyModel remoteData   = new AdManagerClient.MediaAgencyServicePloxy.MediaAgencyModel();

				// 헤더정보 셋트
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;
				remoteHeader.UserLevel     = Header.UserLevel;
				remoteHeader.UserClass	   = Header.UserClass;

				// 호출정보 셋트
				remoteData.SearchKey       = mediaAgencyModel.SearchKey;
				remoteData.SearchMediaName = mediaAgencyModel.SearchMediaName;
				remoteData.SearchRapName = mediaAgencyModel.SearchRapName;
				remoteData.SearchMediaAgency = mediaAgencyModel.SearchMediaAgency;
				
                // 웹서비스 호출 타임아웃설정
                svc.Timeout = FrameSystem.m_SystemTimeout;

				// 웹서비스 메소드 호출
				remoteData = svc.GetMediaAgencyPop(remoteHeader, remoteData);

				// 결과코드검사
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// 결과 셋트
				mediaAgencyModel.MediaAgencyDataSet = remoteData.MediaAgencyDataSet.Copy();
				mediaAgencyModel.ResultCnt   = remoteData.ResultCnt;
				mediaAgencyModel.ResultCD    = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("사용자목록조회 End");
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
		/// 사용자정보 수정
		/// </summary>
		/// <param name="mediaAgencyModel"></param>
		public void SetMediaAgencyUpdate(MediaAgencyModel mediaAgencyModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("사용자정보수정 Start");
				_log.Debug("-----------------------------------------");


				//입력데이터의 Validation 검사
				if(mediaAgencyModel.Charger.Length > 10) 
				{
					throw new FrameException("담당자명은 10Bytes를 초과할 수 없습니다.");
				}
				if(mediaAgencyModel.Charger.Length > 10) 
				{
					throw new FrameException("담당자명은 10Bytes를 초과할 수 없습니다.");
				}
				if(mediaAgencyModel.ContStartDay.Length > 8) 
				{
					throw new FrameException("대행시작일은 8Bytes를 초과할 수 없습니다.");
				}
				if(mediaAgencyModel.ContEndDay.Length > 8) 
				{
					throw new FrameException("대행종료일은 8Bytes를 초과할 수 없습니다.");
				}
				if(mediaAgencyModel.Tell.Length > 15) 
				{
					throw new FrameException("전화번호의 길이는 15Bytes를 초과할 수 없습니다");
				}				
				if(mediaAgencyModel.Email.Length > 40) 
				{
					throw new FrameException("Email은 40Bytes를 초과할 수 없습니다.");
				}	

				// 웹서비스 인스턴스 생성
				MediaAgencyServicePloxy.MediaAgencyService svc = new MediaAgencyServicePloxy.MediaAgencyService();
			
				// URL의 동적셋트
				svc.Url = _WebServiceUrl;
			
				// 리모트 모델 생성
				MediaAgencyServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.MediaAgencyServicePloxy.HeaderModel();
				MediaAgencyServicePloxy.MediaAgencyModel remoteData   = new AdManagerClient.MediaAgencyServicePloxy.MediaAgencyModel();

				// 헤더정보 셋트
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;

				// 호출정보셋트
				remoteData.MediaCode       = mediaAgencyModel.MediaCode;
				remoteData.RapCode       = mediaAgencyModel.RapCode;
				remoteData.AgencyCode       = mediaAgencyModel.AgencyCode;
				remoteData.Charger       = mediaAgencyModel.Charger;
				remoteData.ContStartDay       = mediaAgencyModel.ContStartDay;
				remoteData.ContEndDay       = mediaAgencyModel.ContEndDay;
				remoteData.Tell       = mediaAgencyModel.Tell;
				remoteData.Email       = mediaAgencyModel.Email;
				remoteData.UseYn     = mediaAgencyModel.UseYn;				
								
                // 웹서비스 호출 타임아웃설정
                svc.Timeout = FrameSystem.m_SystemTimeout;

				// 웹서비스 메소드 호출
				remoteData = svc.SetMediaAgencyUpdate(remoteHeader, remoteData);

				// 결과코드검사
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// 결과 셋트
				mediaAgencyModel.ResultCnt   = remoteData.ResultCnt;
				mediaAgencyModel.ResultCD    = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("사용자정보수정 End");
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
		/// 사용자추가
		/// </summary>
		/// <param name="mediaAgencyModel"></param>
		/// <returns></returns>
		public void SetMediaAgencyAdd(MediaAgencyModel mediaAgencyModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("사용자추가 Start");
				_log.Debug("-----------------------------------------");
				
				if(mediaAgencyModel.Charger.Length > 10) 
				{
					throw new FrameException("담당자명은 10Bytes를 초과할 수 없습니다.");
				}
				if(mediaAgencyModel.Charger.Length > 10) 
				{
					throw new FrameException("담당자명은 10Bytes를 초과할 수 없습니다.");
				}
				if(mediaAgencyModel.ContStartDay.Length > 8) 
				{
					throw new FrameException("대행시작일은 8Bytes를 초과할 수 없습니다.");
				}
				if(mediaAgencyModel.ContEndDay.Length > 8) 
				{
					throw new FrameException("대행종료일은 8Bytes를 초과할 수 없습니다.");
				}
				if(mediaAgencyModel.Tell.Length > 15) 
				{
					throw new FrameException("전화번호의 길이는 15Bytes를 초과할 수 없습니다");
				}				
				if(mediaAgencyModel.Email.Length > 40) 
				{
					throw new FrameException("Email은 40Bytes를 초과할 수 없습니다.");
				}


				// 웹서비스 인스턴스 생성
				MediaAgencyServicePloxy.MediaAgencyService svc = new MediaAgencyServicePloxy.MediaAgencyService();
			
				// URL의 동적셋트
				svc.Url = _WebServiceUrl;
				
				// 리모트 모델 생성
				MediaAgencyServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.MediaAgencyServicePloxy.HeaderModel();
				MediaAgencyServicePloxy.MediaAgencyModel remoteData   = new AdManagerClient.MediaAgencyServicePloxy.MediaAgencyModel();

				// 헤더정보 셋트
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;

				// 호출정보셋트
				remoteData.MediaCode       = mediaAgencyModel.MediaCode;
				remoteData.RapCode       = mediaAgencyModel.RapCode;
				remoteData.AgencyCode       = mediaAgencyModel.AgencyCode;
				remoteData.Charger       = mediaAgencyModel.Charger;
				remoteData.ContStartDay       = mediaAgencyModel.ContStartDay;
				remoteData.ContEndDay       = mediaAgencyModel.ContEndDay;
				remoteData.Tell       = mediaAgencyModel.Tell;
				remoteData.Email       = mediaAgencyModel.Email;
				remoteData.UseYn     = mediaAgencyModel.UseYn;				
								
                // 웹서비스 호출 타임아웃설정
                svc.Timeout = FrameSystem.m_SystemTimeout;

				// 웹서비스 메소드 호출
				remoteData = svc.SetMediaAgencyCreate(remoteHeader, remoteData);

				// 결과코드검사
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// 결과 셋트
				mediaAgencyModel.ResultCnt   = remoteData.ResultCnt;
				mediaAgencyModel.ResultCD    = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("사용자추가 End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				mediaAgencyModel.ResultCD    = "3101";
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":SetUserCreate():" + fe.ErrCode + ":" + fe.ResultMsg);
				_log.Warning("-----------------------------------------");
				throw fe;
			}
			catch(Exception e)
			{
				mediaAgencyModel.ResultCD    = "3101";
				_log.Error("-----------------------------------------");
				_log.Exception(e);
				_log.Error("-----------------------------------------");
				throw new FrameException(e.Message);
			}
		}

		
		/// <summary>
		/// 사용자 삭제
		/// </summary>
		/// <param name="baseModel"></param>
		public void SetMediaAgencyDelete(MediaAgencyModel mediaAgencyModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("사용자삭제 Start");
				_log.Debug("-----------------------------------------");

				// 웹서비스 인스턴스 생성
				MediaAgencyServicePloxy.MediaAgencyService svc = new MediaAgencyServicePloxy.MediaAgencyService();
			
				// URL의 동적셋트
				svc.Url = _WebServiceUrl;
				
				// 리모트 모델 생성
				MediaAgencyServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.MediaAgencyServicePloxy.HeaderModel();
				MediaAgencyServicePloxy.MediaAgencyModel remoteData   = new AdManagerClient.MediaAgencyServicePloxy.MediaAgencyModel();

				// 헤더정보 셋트
				remoteHeader.ClientKey  = Header.ClientKey;
				remoteHeader.UserID     = Header.UserID;

				// 호출정보셋트
				remoteData.MediaCode       = mediaAgencyModel.MediaCode;
				remoteData.RapCode       = mediaAgencyModel.RapCode;
				remoteData.AgencyCode       = mediaAgencyModel.AgencyCode;
				
                // 웹서비스 호출 타임아웃설정
                svc.Timeout = FrameSystem.m_SystemTimeout;

				// 웹서비스 메소드 호출
				remoteData = svc.SetMediaAgencyDelete(remoteHeader, remoteData);

				// 결과코드검사
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// 결과 셋트
				mediaAgencyModel.ResultCnt   = remoteData.ResultCnt;
				mediaAgencyModel.ResultCD    = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("사용자삭제 End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":SetMediaAgencyDelete():" + fe.ErrCode + ":" + fe.ResultMsg);
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
