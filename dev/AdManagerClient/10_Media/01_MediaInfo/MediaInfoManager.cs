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
	public class MediaInfoManager : BaseManager
	{
		/// <summary>
		/// ������
		/// </summary>
		/// <param name="systemModel"></param>
		/// <param name="commonModel"></param>

		public MediaInfoManager(SystemModel systemModel, CommonModel commonModel) : base(systemModel, commonModel)
		{
			_log = FrameSystem.oLog;
			_module = "USERINFO";
			_Host  = FrameSystem.m_WebServer_Host;
			_Port  = FrameSystem.m_WebServer_Port;
			_Path  = FrameSystem.m_WebServer_App + "/Media/MediaInfoService.asmx";
		}

		/// <summary>
		/// �����������ȸ
		/// </summary>
		/// <param name="mediaModel"></param>
		public void GetUserList(MediaInfoModel mediaModel)
		{

			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("����ڸ����ȸ Start");
				_log.Debug("-----------------------------------------");
				
				// ������ �ν��Ͻ� ����
				MediaInfoServicePloxy.MediaInfoService svc = new MediaInfoServicePloxy.MediaInfoService();

				svc.Url = _WebServiceUrl;
			
				// ����Ʈ �� ����
				MediaInfoServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.MediaInfoServicePloxy.HeaderModel();
				MediaInfoServicePloxy.MediaInfoModel remoteData   = new AdManagerClient.MediaInfoServicePloxy.MediaInfoModel();

				// ������� ��Ʈ
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;
				remoteHeader.UserClass	   = Header.UserClass;

				// ȣ������ ��Ʈ
				remoteData.SearchKey       = mediaModel.SearchKey;
				remoteData.MediaCode = mediaModel.MediaCode;

				remoteData.SearchchkAdState_10	 = mediaModel.SearchchkAdState_10; 
				
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
				mediaModel.UserDataSet = remoteData.UserDataSet.Copy();
				mediaModel.ResultCnt   = remoteData.ResultCnt;
				mediaModel.ResultCD    = remoteData.ResultCD;

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
		/// <param name="mediaModel"></param>
		public void SetUserUpdate(MediaInfoModel mediaModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("������������� Start");
				_log.Debug("-----------------------------------------");


				//�Էµ������� Validation �˻�
				/*if(mediaModel.MediaCode.Length < 1) 
				{
					throw new FrameException("�����ID�� �������� �ʽ��ϴ�.");
				}*/
				if(mediaModel.MediaName.Length > 20) 
				{
					throw new FrameException("��ü��Ī�� 20Bytes�� �ʰ��� �� �����ϴ�.");
				}
				if(mediaModel.Charger.Length > 10) 
				{
					throw new FrameException("����ڸ��� 10Bytes�� �ʰ��� �� �����ϴ�.");
				}
				if(mediaModel.Tell.Length > 15) 
				{
					throw new FrameException("��ȭ��ȣ�� ���̴� 15Bytes�� �ʰ��� �� �����ϴ�");
				}
				if(mediaModel.Email.Length > 40) 
				{
					throw new FrameException("Email�� ���̴� 40Bytes�� �ʰ��� �� �����ϴ�.");
				}
				

	

				// ������ �ν��Ͻ� ����
				MediaInfoServicePloxy.MediaInfoService svc = new MediaInfoServicePloxy.MediaInfoService();
				svc.Url = _WebServiceUrl;
			
				// ����Ʈ �� ����
				MediaInfoServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.MediaInfoServicePloxy.HeaderModel();
				MediaInfoServicePloxy.MediaInfoModel remoteData   = new AdManagerClient.MediaInfoServicePloxy.MediaInfoModel();

				// ������� ��Ʈ
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;

				// ȣ��������Ʈ
				remoteData.MediaCode       = mediaModel.MediaCode;
				remoteData.MediaName     = mediaModel.MediaName;
				remoteData.Charger = mediaModel.Charger;
				remoteData.Tell    = mediaModel.Tell;
				remoteData.Email     = mediaModel.Email;
				remoteData.UseYn     = mediaModel.UseYn;				
				remoteData.RegDt    = mediaModel.RegDt;
				remoteData.ModDt     = mediaModel.ModDt;
								
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
				mediaModel.ResultCnt   = remoteData.ResultCnt;
				mediaModel.ResultCD    = remoteData.ResultCD;

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
		/// <param name="mediaModel"></param>
		/// <returns></returns>
		public void SetUserAdd(MediaInfoModel mediaModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("������߰� Start");
				_log.Debug("-----------------------------------------");

				
				if(mediaModel.MediaName.Length > 20) 
				{
					throw new FrameException("��ü��Ī�� 20Bytes�� �ʰ��� �� �����ϴ�.");
				}
				if(mediaModel.Charger.Length > 10) 
				{
					throw new FrameException("����ڸ��� 10Bytes�� �ʰ��� �� �����ϴ�.");
				}
				if(mediaModel.Tell.Length > 15) 
				{
					throw new FrameException("��ȭ��ȣ�� ���̴� 15Bytes�� �ʰ��� �� �����ϴ�");
				}				
				if(mediaModel.Email.Length > 40) 
				{
					throw new FrameException("Email�� 40Bytes�� �ʰ��� �� �����ϴ�.");
				}


				// ������ �ν��Ͻ� ����
				MediaInfoServicePloxy.MediaInfoService svc = new MediaInfoServicePloxy.MediaInfoService();
				svc.Url = _WebServiceUrl;
				// ����Ʈ �� ����
				MediaInfoServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.MediaInfoServicePloxy.HeaderModel();
				MediaInfoServicePloxy.MediaInfoModel remoteData   = new AdManagerClient.MediaInfoServicePloxy.MediaInfoModel();

				// ������� ��Ʈ
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;

				// ȣ��������Ʈ
				remoteData.MediaCode       = mediaModel.MediaCode;
				remoteData.MediaName     = mediaModel.MediaName;
				remoteData.Charger = mediaModel.Charger;
				remoteData.Tell    = mediaModel.Tell;
				remoteData.Email     = mediaModel.Email;
				remoteData.UseYn     = mediaModel.UseYn;				
				remoteData.RegDt    = mediaModel.RegDt;
				remoteData.ModDt     = mediaModel.ModDt;
				
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
				mediaModel.ResultCnt   = remoteData.ResultCnt;
				mediaModel.ResultCD    = remoteData.ResultCD;

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
		public void SetUserDelete(MediaInfoModel mediaModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("����ڻ��� Start");
				_log.Debug("-----------------------------------------");

				// ������ �ν��Ͻ� ����
				MediaInfoServicePloxy.MediaInfoService svc = new MediaInfoServicePloxy.MediaInfoService();
				svc.Url = _WebServiceUrl;
				
				// ����Ʈ �� ����
				MediaInfoServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.MediaInfoServicePloxy.HeaderModel();
				MediaInfoServicePloxy.MediaInfoModel remoteData   = new AdManagerClient.MediaInfoServicePloxy.MediaInfoModel();

				// ������� ��Ʈ
				remoteHeader.ClientKey  = Header.ClientKey;
				remoteHeader.UserID     = Header.UserID;

				// ȣ��������Ʈ
				remoteData.MediaCode       = mediaModel.MediaCode;
					
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
				mediaModel.ResultCnt   = remoteData.ResultCnt;
				mediaModel.ResultCD    = remoteData.ResultCD;

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
