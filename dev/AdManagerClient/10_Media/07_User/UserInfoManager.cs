// ===============================================================================
// UserUpdate Manager  for Charites Project
//
// UserUpdateManager.cs
//
// ��������� ���� ���񽺸� ȣ���մϴ�. 
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
	/// ��������� �����񽺸� ȣ���մϴ�. 
	/// </summary>
	public class UserInfoManager : BaseManager
	{
		/// <summary>
		/// ������
		/// </summary>
		/// <param name="systemModel"></param>
		/// <param name="commonModel"></param>

		public UserInfoManager(SystemModel systemModel, CommonModel commonModel) : base(systemModel, commonModel)
		{
			_log = FrameSystem.oLog;
			_module = "USERINFO";
			_Host  = FrameSystem.m_WebServer_Host;
			_Port  = FrameSystem.m_WebServer_Port;
			_Path  = FrameSystem.m_WebServer_App + "/Media/UserInfoService.asmx";
		}

		/// <summary>
		/// �����������ȸ
		/// </summary>
		/// <param name="userModel"></param>
		public void GetUserList(UserInfoModel userModel)
		{

			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("����ڸ����ȸ Start");
				_log.Debug("-----------------------------------------");
				
				// ������ �ν��Ͻ� ����
				UserInfoServicePloxy.UserInfoService svc = new UserInfoServicePloxy.UserInfoService();
				svc.Url = _WebServiceUrl;
			
				// ����Ʈ �� ����
				UserInfoServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.UserInfoServicePloxy.HeaderModel();
				UserInfoServicePloxy.UserInfoModel remoteData   = new AdManagerClient.UserInfoServicePloxy.UserInfoModel();

				// ������� ��Ʈ
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;
				remoteHeader.UserLevel     = Header.UserLevel;
				remoteHeader.UserClass     = Header.UserClass;

				// ȣ������ ��Ʈ
				remoteData.SearchKey       = userModel.SearchKey;
				remoteData.SearchUserLevel = userModel.SearchUserLevel;
				remoteData.SearchUserClass = userModel.SearchUserClass;

				remoteData.SearchchkAdState_10	 = userModel.SearchchkAdState_10; 
				
                // ������ ȣ�� Ÿ�Ӿƿ�����
                svc.Timeout = FrameSystem.m_SystemTimeout;

				// ������ �޼ҵ� ȣ��
				remoteData = svc.GetUsersList(remoteHeader, remoteData);

				// ����ڵ�˻�
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// ��� ��Ʈ
				userModel.UserDataSet = remoteData.UserDataSet.Copy();
				userModel.ResultCnt   = remoteData.ResultCnt;
				userModel.ResultCD    = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("����ڸ����ȸ End");
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
		/// Service ȣ���� ���� �޼ҵ�
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
		/// ��������� ����
		/// </summary>
		/// <param name="userModel"></param>
		public void SetUserUpdate(UserInfoModel userModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("������������� Start");
				_log.Debug("-----------------------------------------");


				//�Էµ������� Validation �˻�
				if(userModel.UserID.Length < 1) 
				{
					throw new FrameException("�����ID�� �������� �ʽ��ϴ�.");
				}
				if(userModel.UserDept.Length > 20) 
				{
					throw new FrameException("�ҼӺμ����� ���̴� 20Bytes�� �ʰ��� �� �����ϴ�.");
				}
				if(userModel.UserTitle.Length > 20) 
				{
					throw new FrameException("��å���Ը��� ���̴� 20Bytes�� �ʰ��� �� �����ϴ�.");
				}
				if(userModel.UserTell.Length > 15) 
				{
					throw new FrameException("��ȭ��ȣ�� ���̴� 15Bytes�� �ʰ��� �� �����ϴ�");
				}
				if(userModel.UserTell.Length > 15) 
				{
					throw new FrameException("�޴���ȭ��ȣ�� ���̴� 15Bytes�� �ʰ��� �� �����ϴ�.");
				}
				if(userModel.UserComment.Length > 50) 
				{
					throw new FrameException("������ ���̴� 50Bytes�� �ʰ��� �� �����ϴ�.");
				}

	

				// ������ �ν��Ͻ� ����
				UserInfoServicePloxy.UserInfoService svc = new UserInfoServicePloxy.UserInfoService();
				svc.Url = _WebServiceUrl;
			
				// ����Ʈ �� ����
				UserInfoServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.UserInfoServicePloxy.HeaderModel();
				UserInfoServicePloxy.UserInfoModel remoteData   = new AdManagerClient.UserInfoServicePloxy.UserInfoModel();

				// ������� ��Ʈ
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;

				// ȣ��������Ʈ
				remoteData.UserID       = userModel.UserID;
				remoteData.UserName     = userModel.UserName;
				remoteData.UserPassword = userModel.UserPassword;
				remoteData.UserLevel    = userModel.UserLevel;
				remoteData.UserClass    = userModel.UserClass;
				remoteData.MediaCode    = userModel.MediaCode;
				remoteData.RapCode    = userModel.RapCode;
				remoteData.AgencyCode    = userModel.AgencyCode;
				remoteData.UserDept     = userModel.UserDept;
				remoteData.UserTitle    = userModel.UserTitle;
				remoteData.UserTell     = userModel.UserTell;
				remoteData.UserMobile   = userModel.UserMobile;
				remoteData.UserEMail   = userModel.UserEMail;
				remoteData.UserComment  = userModel.UserComment;
				remoteData.UseYn     = userModel.UseYn;				
				
                // ������ ȣ�� Ÿ�Ӿƿ�����
                svc.Timeout = FrameSystem.m_SystemTimeout;

				// ������ �޼ҵ� ȣ��
				remoteData = svc.SetUserUpdate(remoteHeader, remoteData);

				// ����ڵ�˻�
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// ��� ��Ʈ
				userModel.ResultCnt   = remoteData.ResultCnt;
				userModel.ResultCD    = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("������������� End");
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
		/// ������߰�
		/// </summary>
		/// <param name="userModel"></param>
		/// <returns></returns>
		public void SetUserAdd(UserInfoModel userModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("������߰� Start");
				_log.Debug("-----------------------------------------");

				if(userModel.UserID.Trim().Length < 1) 
				{
					throw new FrameException("�����ID�� �������� �ʽ��ϴ�.");
				}
				if(userModel.UserID.Trim().Length > 10) 
				{
					throw new FrameException("�����ID�� 10Bytes�� �ʰ��� �� �����ϴ�.");
				}
				if(userModel.UserDept.Length > 20) 
				{
					throw new FrameException("�ҼӺμ����� ���̴� 20Bytes�� �ʰ��� �� �����ϴ�.");
				}
				if(userModel.UserTitle.Length > 20) 
				{
					throw new FrameException("��å���Ը��� ���̴� 20Bytes�� �ʰ��� �� �����ϴ�.");
				}
				if(userModel.UserTell.Length > 15) 
				{
					throw new FrameException("��ȭ��ȣ�� ���̴� 15Bytes�� �ʰ��� �� �����ϴ�");
				}
				if(userModel.UserTell.Length > 15) 
				{
					throw new FrameException("�޴���ȭ��ȣ�� ���̴� 15Bytes�� �ʰ��� �� �����ϴ�.");
				}
				if(userModel.UserComment.Length > 50) 
				{
					throw new FrameException("������ ���̴� 50Bytes�� �ʰ��� �� �����ϴ�.");
				}


				// ������ �ν��Ͻ� ����
				UserInfoServicePloxy.UserInfoService svc = new UserInfoServicePloxy.UserInfoService();
				svc.Url = _WebServiceUrl;
				
				// ����Ʈ �� ����
				UserInfoServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.UserInfoServicePloxy.HeaderModel();
				UserInfoServicePloxy.UserInfoModel remoteData   = new AdManagerClient.UserInfoServicePloxy.UserInfoModel();

				// ������� ��Ʈ
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;

				// ȣ��������Ʈ
				remoteData.UserID       = userModel.UserID;
				remoteData.UserName     = userModel.UserName;
				remoteData.UserPassword = userModel.UserPassword;
				remoteData.UserLevel    = userModel.UserLevel;
				remoteData.UserClass    = userModel.UserClass;
				remoteData.MediaCode    = userModel.MediaCode;
				remoteData.RapCode    = userModel.RapCode;
				remoteData.AgencyCode    = userModel.AgencyCode;
				remoteData.UserDept     = userModel.UserDept;
				remoteData.UserTitle    = userModel.UserTitle;
				remoteData.UserTell     = userModel.UserTell;
				remoteData.UserMobile   = userModel.UserMobile;
				remoteData.UserEMail   = userModel.UserEMail;
				remoteData.UserComment  = userModel.UserComment;
				remoteData.UseYn     = userModel.UseYn;				
					
                // ������ ȣ�� Ÿ�Ӿƿ�����
                svc.Timeout = FrameSystem.m_SystemTimeout;

				// ������ �޼ҵ� ȣ��
				remoteData = svc.SetUserCreate(remoteHeader, remoteData);

				// ����ڵ�˻�
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// ��� ��Ʈ
				userModel.ResultCnt   = remoteData.ResultCnt;
				userModel.ResultCD    = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("������߰� End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":SetUserCreate():" + fe.ErrCode + ":" + fe.ResultMsg);
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
		/// ����� ����
		/// </summary>
		/// <param name="baseModel"></param>
		public void SetUserDelete(UserInfoModel userModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("����ڻ��� Start");
				_log.Debug("-----------------------------------------");

				// ������ �ν��Ͻ� ����
				UserInfoServicePloxy.UserInfoService svc = new UserInfoServicePloxy.UserInfoService();
				svc.Url = _WebServiceUrl;
				
				// ����Ʈ �� ����
				UserInfoServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.UserInfoServicePloxy.HeaderModel();
				UserInfoServicePloxy.UserInfoModel remoteData   = new AdManagerClient.UserInfoServicePloxy.UserInfoModel();

				// ������� ��Ʈ
				remoteHeader.ClientKey  = Header.ClientKey;
				remoteHeader.UserID     = Header.UserID;

				// ȣ��������Ʈ
				remoteData.UserID       = userModel.UserID;
					
                // ������ ȣ�� Ÿ�Ӿƿ�����
                svc.Timeout = FrameSystem.m_SystemTimeout;

				// ������ �޼ҵ� ȣ��
				remoteData = svc.SetUserDelete(remoteHeader, remoteData);

				// ����ڵ�˻�
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// ��� ��Ʈ
				userModel.ResultCnt   = remoteData.ResultCnt;
				userModel.ResultCD    = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("����ڻ��� End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":SetUserDelete():" + fe.ErrCode + ":" + fe.ResultMsg);
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
