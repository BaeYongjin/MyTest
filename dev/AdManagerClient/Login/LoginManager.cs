// ==============================================================================
// LoginManager.cs
// ���� Login ���񽺸� ȣ���մϴ�. 
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
	/// ���� Login ���񽺸� ȣ���մϴ�. 
	/// </summary>
	public class LoginManager : BaseManager
	{
		/// <summary>
		/// ������
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
		/// Service ȣ���� ���� �޼ҵ�
		/// </summary>
		public void Login(LoginModel loginModel )
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("�α��� Start");
				_log.Debug("-----------------------------------------");
	
				// ������ �ν��Ͻ� ����
				LoginServicePloxy.LoginService svc = new LoginServicePloxy.LoginService();

				// ������ URL�� ���� ��Ʈ
				svc.Url = _WebServiceUrl;
			
				// ����Ʈ �� ����
				LoginServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.LoginServicePloxy.HeaderModel();
				LoginServicePloxy.LoginModel    remoteData   = new AdManagerClient.LoginServicePloxy.LoginModel();

				// ������� ��Ʈ
				remoteHeader.ClientKey     = Header.ClientKey;

				// ȣ������ ��Ʈ
				remoteData.UserID          = loginModel.UserID;
				remoteData.UserPassword    = loginModel.UserPassword;
				remoteData.SystemVersion   = loginModel.SystemVersion;

				// ������ ȣ�� Ÿ�Ӿƿ�����
				svc.Timeout = FrameSystem.m_SystemTimeout;
	
				// ������ �޼ҵ� ȣ��
				remoteData = svc.Login(remoteHeader, remoteData);

				// ��� ��Ʈ
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
				_log.Message(loginModel.UserName + "(" + loginModel.UserID +")������ �α����Ͽ����ϴ�.");

				_log.Debug("-----------------------------------------");
				_log.Debug("�α��� End");
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