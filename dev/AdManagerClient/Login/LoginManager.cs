// ==============================================================================
// LoginManager.cs
// 서버 Login 서비스를 호출합니다. 
// ==============================================================================
// Release history
// 2007.08.15 RH.Jung Initialize
// ==============================================================================
// Copyright (C) 2007 G-Inno Systems Inc.
// All rights reserved.
// ==============================================================================
using System;
using System.Data;
using System.Data.SqlClient;

using WinFramework.Misc;
using WinFramework.Base;
using WinFramework.Data;

using AdManagerModel;

namespace AdManagerClient
{
	/// <summary>
	/// 서버 Login 서비스를 호출합니다. 
	/// </summary>
	public class LoginManager : BaseManager
	{
		/// <summary>
		/// 생성자
		/// </summary>
		/// <param name="systemModel"></param>
		/// <param name="commonModel"></param>
		public LoginManager(SystemModel systemModel) : base(systemModel)
		{
			_log = FrameSystem.oLog;
			_module = "LOGIN";

			_Host  = FrameSystem.m_WebServer_Host;
			_Port  = FrameSystem.m_WebServer_Port;
			_Path  = FrameSystem.m_WebServer_App + "/Login/LoginService.asmx";
		}

		/// <summary>
		/// Service 호출을 위한 메소드
		/// </summary>
		public void Login(LoginModel loginModel )
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("로그인 Start");
				_log.Debug("-----------------------------------------");
	
				// 웹서비스 인스턴스 생성
				LoginServicePloxy.LoginService svc = new LoginServicePloxy.LoginService();

				// 웹서비스 URL의 동적 셋트
				svc.Url = _WebServiceUrl;
			
				// 리모트 모델 생성
				LoginServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.LoginServicePloxy.HeaderModel();
				LoginServicePloxy.LoginModel    remoteData   = new AdManagerClient.LoginServicePloxy.LoginModel();

				// 헤더정보 셋트
				remoteHeader.ClientKey     = Header.ClientKey;

				// 호출정보 셋트
				remoteData.UserID          = loginModel.UserID;
				remoteData.UserPassword    = loginModel.UserPassword;
				remoteData.SystemVersion   = loginModel.SystemVersion;

				// 웹서비스 호출 타임아웃설정
				svc.Timeout = FrameSystem.m_SystemTimeout;
	
				// 웹서비스 메소드 호출
				remoteData = svc.Login(remoteHeader, remoteData);

				// 결과 셋트
				loginModel.UserName  = remoteData.UserName;
				loginModel.UserLevel = remoteData.UserLevel;
				loginModel.LevelName = remoteData.LevelName;
				loginModel.UserClass = remoteData.UserClass;
				loginModel.ClassName = remoteData.ClassName;
				loginModel.MediaCode = remoteData.MediaCode;
				loginModel.MediaName = remoteData.MediaName;
				loginModel.RapCode = remoteData.RapCode;				
				loginModel.RapName = remoteData.RapName;
				loginModel.AgencyCode = remoteData.AgencyCode;
				loginModel.AgencyName = remoteData.AgencyName;
				loginModel.LoginTime = remoteData.LoginTime;
				loginModel.LastLogin = remoteData.LastLogin;
				loginModel.ResultCD    = remoteData.ResultCD;
				
				if(!loginModel.ResultCD.Equals("0000"))
				{

					throw new FrameException(remoteData.ResultDesc, _module, loginModel.ResultCD);
				}
				_log.Message(loginModel.UserName + "(" + loginModel.UserID +")님으로 로그인하였습니다.");

				_log.Debug("-----------------------------------------");
				_log.Debug("로그인 End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":" + fe.ResultMsg);
				_log.Warning("-----------------------------------------");
				throw fe;
			}
			catch(Exception e)
			{
				_log.Error("-----------------------------------------");
				_log.Exception(e);
				_log.Error("-----------------------------------------");
				throw new FrameException(e.Message,"","1005");
			}
		}
	}
}