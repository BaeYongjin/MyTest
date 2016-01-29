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
	public class MediaAgencyManager : BaseManager
	{
		/// <summary>
		/// ������
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
		/// �����������ȸ
		/// </summary>
		/// <param name="mediaAgencyModel"></param>
		public void GetMediaAgencyList(MediaAgencyModel mediaAgencyModel)
		{

			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("����ڸ����ȸ Start");
				_log.Debug("-----------------------------------------");
				
				// ������ �ν��Ͻ� ����
				MediaAgencyServicePloxy.MediaAgencyService svc = new MediaAgencyServicePloxy.MediaAgencyService();
			
				// URL�� ������Ʈ
				svc.Url = _WebServiceUrl;
			
				// ����Ʈ �� ����
				MediaAgencyServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.MediaAgencyServicePloxy.HeaderModel();
				MediaAgencyServicePloxy.MediaAgencyModel remoteData   = new AdManagerClient.MediaAgencyServicePloxy.MediaAgencyModel();

				// ������� ��Ʈ
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;
				remoteHeader.UserLevel     = Header.UserLevel;
				remoteHeader.UserClass	   = Header.UserClass;

				// ȣ������ ��Ʈ
				remoteData.SearchKey       = mediaAgencyModel.SearchKey;
				remoteData.SearchMediaName = mediaAgencyModel.SearchMediaName;
				remoteData.SearchRapName = mediaAgencyModel.SearchRapName;
				remoteData.SearchMediaAgency = mediaAgencyModel.SearchMediaAgency;
				remoteData.SearchchkAdState_10	 = mediaAgencyModel.SearchchkAdState_10; 
				
                // ������ ȣ�� Ÿ�Ӿƿ�����
                svc.Timeout = FrameSystem.m_SystemTimeout;

				// ������ �޼ҵ� ȣ��
				remoteData = svc.GetMediaAgencyList(remoteHeader, remoteData);

				// ����ڵ�˻�
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// ��� ��Ʈ
				mediaAgencyModel.MediaAgencyDataSet = remoteData.MediaAgencyDataSet.Copy();
				mediaAgencyModel.ResultCnt   = remoteData.ResultCnt;
				mediaAgencyModel.ResultCD    = remoteData.ResultCD;

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
		/// ��ü������˾����������ȸ(��ü����纰 �����ְ������� ���)
		/// </summary>
		/// <param name="mediaAgencyModel"></param>
		public void GetMediaAgencyPop(MediaAgencyModel mediaAgencyModel)
		{

			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("����ڸ����ȸ Start");
				_log.Debug("-----------------------------------------");
				
				// ������ �ν��Ͻ� ����
				MediaAgencyServicePloxy.MediaAgencyService svc = new MediaAgencyServicePloxy.MediaAgencyService();
			
				// URL�� ������Ʈ
				svc.Url = _WebServiceUrl;
			
				// ����Ʈ �� ����
				MediaAgencyServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.MediaAgencyServicePloxy.HeaderModel();
				MediaAgencyServicePloxy.MediaAgencyModel remoteData   = new AdManagerClient.MediaAgencyServicePloxy.MediaAgencyModel();

				// ������� ��Ʈ
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;
				remoteHeader.UserLevel     = Header.UserLevel;
				remoteHeader.UserClass	   = Header.UserClass;

				// ȣ������ ��Ʈ
				remoteData.SearchKey       = mediaAgencyModel.SearchKey;
				remoteData.SearchMediaName = mediaAgencyModel.SearchMediaName;
				remoteData.SearchRapName = mediaAgencyModel.SearchRapName;
				remoteData.SearchMediaAgency = mediaAgencyModel.SearchMediaAgency;
				
                // ������ ȣ�� Ÿ�Ӿƿ�����
                svc.Timeout = FrameSystem.m_SystemTimeout;

				// ������ �޼ҵ� ȣ��
				remoteData = svc.GetMediaAgencyPop(remoteHeader, remoteData);

				// ����ڵ�˻�
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// ��� ��Ʈ
				mediaAgencyModel.MediaAgencyDataSet = remoteData.MediaAgencyDataSet.Copy();
				mediaAgencyModel.ResultCnt   = remoteData.ResultCnt;
				mediaAgencyModel.ResultCD    = remoteData.ResultCD;

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
		/// <param name="mediaAgencyModel"></param>
		public void SetMediaAgencyUpdate(MediaAgencyModel mediaAgencyModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("������������� Start");
				_log.Debug("-----------------------------------------");


				//�Էµ������� Validation �˻�
				if(mediaAgencyModel.Charger.Length > 10) 
				{
					throw new FrameException("����ڸ��� 10Bytes�� �ʰ��� �� �����ϴ�.");
				}
				if(mediaAgencyModel.Charger.Length > 10) 
				{
					throw new FrameException("����ڸ��� 10Bytes�� �ʰ��� �� �����ϴ�.");
				}
				if(mediaAgencyModel.ContStartDay.Length > 8) 
				{
					throw new FrameException("����������� 8Bytes�� �ʰ��� �� �����ϴ�.");
				}
				if(mediaAgencyModel.ContEndDay.Length > 8) 
				{
					throw new FrameException("������������ 8Bytes�� �ʰ��� �� �����ϴ�.");
				}
				if(mediaAgencyModel.Tell.Length > 15) 
				{
					throw new FrameException("��ȭ��ȣ�� ���̴� 15Bytes�� �ʰ��� �� �����ϴ�");
				}				
				if(mediaAgencyModel.Email.Length > 40) 
				{
					throw new FrameException("Email�� 40Bytes�� �ʰ��� �� �����ϴ�.");
				}	

				// ������ �ν��Ͻ� ����
				MediaAgencyServicePloxy.MediaAgencyService svc = new MediaAgencyServicePloxy.MediaAgencyService();
			
				// URL�� ������Ʈ
				svc.Url = _WebServiceUrl;
			
				// ����Ʈ �� ����
				MediaAgencyServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.MediaAgencyServicePloxy.HeaderModel();
				MediaAgencyServicePloxy.MediaAgencyModel remoteData   = new AdManagerClient.MediaAgencyServicePloxy.MediaAgencyModel();

				// ������� ��Ʈ
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;

				// ȣ��������Ʈ
				remoteData.MediaCode       = mediaAgencyModel.MediaCode;
				remoteData.RapCode       = mediaAgencyModel.RapCode;
				remoteData.AgencyCode       = mediaAgencyModel.AgencyCode;
				remoteData.Charger       = mediaAgencyModel.Charger;
				remoteData.ContStartDay       = mediaAgencyModel.ContStartDay;
				remoteData.ContEndDay       = mediaAgencyModel.ContEndDay;
				remoteData.Tell       = mediaAgencyModel.Tell;
				remoteData.Email       = mediaAgencyModel.Email;
				remoteData.UseYn     = mediaAgencyModel.UseYn;				
								
                // ������ ȣ�� Ÿ�Ӿƿ�����
                svc.Timeout = FrameSystem.m_SystemTimeout;

				// ������ �޼ҵ� ȣ��
				remoteData = svc.SetMediaAgencyUpdate(remoteHeader, remoteData);

				// ����ڵ�˻�
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// ��� ��Ʈ
				mediaAgencyModel.ResultCnt   = remoteData.ResultCnt;
				mediaAgencyModel.ResultCD    = remoteData.ResultCD;

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
		/// <param name="mediaAgencyModel"></param>
		/// <returns></returns>
		public void SetMediaAgencyAdd(MediaAgencyModel mediaAgencyModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("������߰� Start");
				_log.Debug("-----------------------------------------");
				
				if(mediaAgencyModel.Charger.Length > 10) 
				{
					throw new FrameException("����ڸ��� 10Bytes�� �ʰ��� �� �����ϴ�.");
				}
				if(mediaAgencyModel.Charger.Length > 10) 
				{
					throw new FrameException("����ڸ��� 10Bytes�� �ʰ��� �� �����ϴ�.");
				}
				if(mediaAgencyModel.ContStartDay.Length > 8) 
				{
					throw new FrameException("����������� 8Bytes�� �ʰ��� �� �����ϴ�.");
				}
				if(mediaAgencyModel.ContEndDay.Length > 8) 
				{
					throw new FrameException("������������ 8Bytes�� �ʰ��� �� �����ϴ�.");
				}
				if(mediaAgencyModel.Tell.Length > 15) 
				{
					throw new FrameException("��ȭ��ȣ�� ���̴� 15Bytes�� �ʰ��� �� �����ϴ�");
				}				
				if(mediaAgencyModel.Email.Length > 40) 
				{
					throw new FrameException("Email�� 40Bytes�� �ʰ��� �� �����ϴ�.");
				}


				// ������ �ν��Ͻ� ����
				MediaAgencyServicePloxy.MediaAgencyService svc = new MediaAgencyServicePloxy.MediaAgencyService();
			
				// URL�� ������Ʈ
				svc.Url = _WebServiceUrl;
				
				// ����Ʈ �� ����
				MediaAgencyServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.MediaAgencyServicePloxy.HeaderModel();
				MediaAgencyServicePloxy.MediaAgencyModel remoteData   = new AdManagerClient.MediaAgencyServicePloxy.MediaAgencyModel();

				// ������� ��Ʈ
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;

				// ȣ��������Ʈ
				remoteData.MediaCode       = mediaAgencyModel.MediaCode;
				remoteData.RapCode       = mediaAgencyModel.RapCode;
				remoteData.AgencyCode       = mediaAgencyModel.AgencyCode;
				remoteData.Charger       = mediaAgencyModel.Charger;
				remoteData.ContStartDay       = mediaAgencyModel.ContStartDay;
				remoteData.ContEndDay       = mediaAgencyModel.ContEndDay;
				remoteData.Tell       = mediaAgencyModel.Tell;
				remoteData.Email       = mediaAgencyModel.Email;
				remoteData.UseYn     = mediaAgencyModel.UseYn;				
								
                // ������ ȣ�� Ÿ�Ӿƿ�����
                svc.Timeout = FrameSystem.m_SystemTimeout;

				// ������ �޼ҵ� ȣ��
				remoteData = svc.SetMediaAgencyCreate(remoteHeader, remoteData);

				// ����ڵ�˻�
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// ��� ��Ʈ
				mediaAgencyModel.ResultCnt   = remoteData.ResultCnt;
				mediaAgencyModel.ResultCD    = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("������߰� End");
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
		/// ����� ����
		/// </summary>
		/// <param name="baseModel"></param>
		public void SetMediaAgencyDelete(MediaAgencyModel mediaAgencyModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("����ڻ��� Start");
				_log.Debug("-----------------------------------------");

				// ������ �ν��Ͻ� ����
				MediaAgencyServicePloxy.MediaAgencyService svc = new MediaAgencyServicePloxy.MediaAgencyService();
			
				// URL�� ������Ʈ
				svc.Url = _WebServiceUrl;
				
				// ����Ʈ �� ����
				MediaAgencyServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.MediaAgencyServicePloxy.HeaderModel();
				MediaAgencyServicePloxy.MediaAgencyModel remoteData   = new AdManagerClient.MediaAgencyServicePloxy.MediaAgencyModel();

				// ������� ��Ʈ
				remoteHeader.ClientKey  = Header.ClientKey;
				remoteHeader.UserID     = Header.UserID;

				// ȣ��������Ʈ
				remoteData.MediaCode       = mediaAgencyModel.MediaCode;
				remoteData.RapCode       = mediaAgencyModel.RapCode;
				remoteData.AgencyCode       = mediaAgencyModel.AgencyCode;
				
                // ������ ȣ�� Ÿ�Ӿƿ�����
                svc.Timeout = FrameSystem.m_SystemTimeout;

				// ������ �޼ҵ� ȣ��
				remoteData = svc.SetMediaAgencyDelete(remoteHeader, remoteData);

				// ����ڵ�˻�
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// ��� ��Ʈ
				mediaAgencyModel.ResultCnt   = remoteData.ResultCnt;
				mediaAgencyModel.ResultCD    = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("����ڻ��� End");
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
