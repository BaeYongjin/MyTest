// ===============================================================================
// UserUpdate Manager  for Charites Project
//
// UserUpdateManager.cs
//
// �����ü���౤�������� ���� ���񽺸� ȣ���մϴ�. 
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
	/// �����ü���౤�������� �����񽺸� ȣ���մϴ�. 
	/// </summary>
	public class ClientManager : BaseManager
	{
		/// <summary>
		/// ������
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
        /// �������ڵ� �޺�
        /// </summary>
        /// <param name="clientModel"></param>

        public void GetAdvertiserList(ClientModel clientModel)
        {
            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug("�����ڵ���ȸ Start");
                _log.Debug("-----------------------------------------");

                // ������ �ν��Ͻ� ����
                ClientServicePloxy.ClientService svc = new ClientServicePloxy.ClientService();
			
				// URL�� ������Ʈ
				svc.Url = _WebServiceUrl;
			
                // ����Ʈ �� ����
                ClientServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.ClientServicePloxy.HeaderModel();
                ClientServicePloxy.ClientModel     remoteData   = new AdManagerClient.ClientServicePloxy.ClientModel();

                // ������� ��Ʈ
                remoteHeader.ClientKey     = Header.ClientKey;
                remoteHeader.UserID        = Header.UserID;
				remoteHeader.UserClass	   = Header.UserClass;

                // ȣ������ ��Ʈ
                remoteData.AdvertiserCode         = clientModel.AdvertiserCode;				
				
                // ������ ȣ�� Ÿ�Ӿƿ�����
                svc.Timeout = FrameSystem.m_SystemTimeout;

                // ������ �޼ҵ� ȣ��
                remoteData = svc.GetAdvertisertList(remoteHeader, remoteData);

                // ����ڵ�˻�
                if(!remoteData.ResultCD.Equals("0000"))
                {
                    throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
                }

                // ��� ��Ʈ
                clientModel.ClientDataSet = remoteData.ClientDataSet.Copy();
                clientModel.ResultCnt   = remoteData.ResultCnt;
                clientModel.ResultCD    = remoteData.ResultCD;

                _log.Debug("-----------------------------------------");
                _log.Debug("�����ڵ���ȸ End");
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
        /// ��ü���� �������ڵ� �޺�
        /// </summary>
        /// <param name="clientModel"></param>

        public void GetClientAdvertiserListByCombo(ClientModel clientModel)
        {
            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug("��ü���� �������ڵ���ȸ Start");
                _log.Debug("-----------------------------------------");

                // ������ �ν��Ͻ� ����
                ClientServicePloxy.ClientService svc = new ClientServicePloxy.ClientService();
			
				// URL�� ������Ʈ
				svc.Url = _WebServiceUrl;
			
                // ����Ʈ �� ����
                ClientServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.ClientServicePloxy.HeaderModel();
                ClientServicePloxy.ClientModel     remoteData   = new AdManagerClient.ClientServicePloxy.ClientModel();

                // ������� ��Ʈ
                remoteHeader.ClientKey     = Header.ClientKey;
                remoteHeader.UserID        = Header.UserID;

                // ȣ������ ��Ʈ
                remoteData.MediaCode       = clientModel.MediaCode;
                remoteData.RapCode         = clientModel.RapCode;
                remoteData.AgencyCode      = clientModel.AgencyCode;
                
                // ������ ȣ�� Ÿ�Ӿƿ�����
                svc.Timeout = FrameSystem.m_SystemTimeout;
				
                // ������ �޼ҵ� ȣ��
                remoteData = svc.GetClientAdvertiserListByCombo(remoteHeader, remoteData);

                // ����ڵ�˻�
                if(!remoteData.ResultCD.Equals("0000"))
                {
                    throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
                }

                // ��� ��Ʈ
                clientModel.ClientDataSet = remoteData.ClientDataSet.Copy();
                clientModel.ResultCnt   = remoteData.ResultCnt;
                clientModel.ResultCD    = remoteData.ResultCD;

                _log.Debug("-----------------------------------------");
                _log.Debug("�����ڵ���ȸ End");
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
		/// �μ�Ʈ�޺� ����
		/// </summary>
		/// <param name="clientModel"></param>
		public void GetClientMediaList(ClientModel clientModel)
		{

			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug(" �μ�Ʈ�޺� ���� Start");
				_log.Debug("-----------------------------------------");
				
				// ������ �ν��Ͻ� ����
				ClientServicePloxy.ClientService svc = new ClientServicePloxy.ClientService();
			
				// URL�� ������Ʈ
				svc.Url = _WebServiceUrl;
			
				// ����Ʈ �� ����
				ClientServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.ClientServicePloxy.HeaderModel();
				ClientServicePloxy.ClientModel remoteData   = new AdManagerClient.ClientServicePloxy.ClientModel();

				// ������� ��Ʈ
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;
				remoteHeader.UserLevel     = Header.UserLevel;

				// ȣ������ ��Ʈ
				//remoteData.MediaCode_C       = clientModel.MediaCode_C;
				
                // ������ ȣ�� Ÿ�Ӿƿ�����
                svc.Timeout = FrameSystem.m_SystemTimeout;

				// ������ �޼ҵ� ȣ��
				remoteData = svc.GetClientMediaList(remoteHeader, remoteData);

				// ����ڵ�˻�
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// ��� ��Ʈ
				clientModel.ClientDataSet = remoteData.ClientDataSet.Copy();
				clientModel.ResultCnt   = remoteData.ResultCnt;
				clientModel.ResultCD    = remoteData.ResultCD;				
				_log.Debug(clientModel.ResultCD);

				_log.Debug("-----------------------------------------");
				_log.Debug(" �μ�Ʈ�޺� ���� End");
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
		/// �μ�Ʈ�޺� ����
		/// </summary>
		/// <param name="clientModel"></param>
		public void GetClientRapList(ClientModel clientModel)
		{

			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug(" �μ�Ʈ�޺� ���� Start");
				_log.Debug("-----------------------------------------");
				
				// ������ �ν��Ͻ� ����
				ClientServicePloxy.ClientService svc = new ClientServicePloxy.ClientService();
			
				// URL�� ������Ʈ
				svc.Url = _WebServiceUrl;
			
				// ����Ʈ �� ����
				ClientServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.ClientServicePloxy.HeaderModel();
				ClientServicePloxy.ClientModel remoteData   = new AdManagerClient.ClientServicePloxy.ClientModel();

				// ������� ��Ʈ
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;
				remoteHeader.UserLevel     = Header.UserLevel;

				// ȣ������ ��Ʈ
				
				remoteData.MediaCode_C       = clientModel.MediaCode_C;
				
                // ������ ȣ�� Ÿ�Ӿƿ�����
                svc.Timeout = FrameSystem.m_SystemTimeout;

				// ������ �޼ҵ� ȣ��
				remoteData = svc.GetClientRapList(remoteHeader, remoteData);

				// ����ڵ�˻�
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// ��� ��Ʈ
				clientModel.ClientDataSet = remoteData.ClientDataSet.Copy();
				clientModel.ResultCnt   = remoteData.ResultCnt;
				clientModel.ResultCD    = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug(" �μ�Ʈ�޺� ���� End");
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
		/// �μ�Ʈ�޺� ����
		/// </summary>
		/// <param name="clientModel"></param>
		public void GetClientAgencyList(ClientModel clientModel)
		{

			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug(" �μ�Ʈ�޺� ���� Start");
				_log.Debug("-----------------------------------------");
				
				// ������ �ν��Ͻ� ����
				ClientServicePloxy.ClientService svc = new ClientServicePloxy.ClientService();
			
				// URL�� ������Ʈ
				svc.Url = _WebServiceUrl;
			
				// ����Ʈ �� ����
				ClientServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.ClientServicePloxy.HeaderModel();
				ClientServicePloxy.ClientModel remoteData   = new AdManagerClient.ClientServicePloxy.ClientModel();

				// ������� ��Ʈ
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;
				remoteHeader.UserLevel     = Header.UserLevel;

				// ȣ������ ��Ʈ
				remoteData.MediaCode_C       = clientModel.MediaCode_C;
				remoteData.RapCode_C = clientModel.RapCode_C;				
				
                // ������ ȣ�� Ÿ�Ӿƿ�����
                svc.Timeout = FrameSystem.m_SystemTimeout;

				// ������ �޼ҵ� ȣ��
				remoteData = svc.GetClientAgencyList(remoteHeader, remoteData);

				// ����ڵ�˻�
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// ��� ��Ʈ
				clientModel.ClientDataSet = remoteData.ClientDataSet.Copy();
				clientModel.ResultCnt   = remoteData.ResultCnt;
				clientModel.ResultCD    = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug(" �μ�Ʈ�޺� ���� End");
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
		/// �μ�Ʈ�޺� ����
		/// </summary>
		/// <param name="clientModel"></param>
		public void GetClientAdvertiserList(ClientModel clientModel)
		{

			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug(" �μ�Ʈ�޺� ���� Start");
				_log.Debug("-----------------------------------------");
				
				// ������ �ν��Ͻ� ����
				ClientServicePloxy.ClientService svc = new ClientServicePloxy.ClientService();
			
				// URL�� ������Ʈ
				svc.Url = _WebServiceUrl;
			
				// ����Ʈ �� ����
				ClientServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.ClientServicePloxy.HeaderModel();
				ClientServicePloxy.ClientModel remoteData   = new AdManagerClient.ClientServicePloxy.ClientModel();

				// ������� ��Ʈ
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;
				remoteHeader.UserLevel     = Header.UserLevel;
						
		        // ȣ������ ��Ʈ
                remoteData.MediaCode = clientModel.MediaCode;
                remoteData.RapCode = clientModel.RapCode;
                remoteData.AgencyCode = clientModel.AgencyCode;
				remoteData.SearchRap = clientModel.SearchRap;
				remoteData.SearchKey = clientModel.SearchKey;

                // ������ ȣ�� Ÿ�Ӿƿ�����
                svc.Timeout = FrameSystem.m_SystemTimeout;
				
				// ������ �޼ҵ� ȣ��
				remoteData = svc.GetClientAdvertiserList(remoteHeader, remoteData);

				// ����ڵ�˻�
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// ��� ��Ʈ
				clientModel.ClientDataSet = remoteData.ClientDataSet.Copy();
				clientModel.ResultCnt   = remoteData.ResultCnt;
				clientModel.ResultCD    = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug(" �μ�Ʈ�޺� ���� End");
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
		/// �����ü���౤����������ȸ
		/// </summary>
		/// <param name="clientModel"></param>
		public void GetClientList(ClientModel clientModel)
		{

			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("�����ü���౤���ָ����ȸ Start");
				_log.Debug("-----------------------------------------");
				
				// ������ �ν��Ͻ� ����
				ClientServicePloxy.ClientService svc = new ClientServicePloxy.ClientService();
			
				// URL�� ������Ʈ
				svc.Url = _WebServiceUrl;
			
				// ����Ʈ �� ����
				ClientServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.ClientServicePloxy.HeaderModel();
				ClientServicePloxy.ClientModel remoteData   = new AdManagerClient.ClientServicePloxy.ClientModel();

				// ������� ��Ʈ
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;
				remoteHeader.UserLevel     = Header.UserLevel;
				remoteHeader.UserClass	   = Header.UserClass;

				// ȣ������ ��Ʈ
				remoteData.SearchKey       = clientModel.SearchKey;
				remoteData.SearchMediaName = clientModel.SearchMediaName;
				remoteData.SearchRapName = clientModel.SearchRapName;
                remoteData.SearchAdvertiserName = clientModel.SearchAdvertiserName;
				remoteData.SearchMediaAgency = clientModel.SearchMediaAgency;			
				remoteData.SearchchkAdState_10	 = clientModel.SearchchkAdState_10; 

                // ������ ȣ�� Ÿ�Ӿƿ�����
                svc.Timeout = FrameSystem.m_SystemTimeout;

				// ������ �޼ҵ� ȣ��
				remoteData = svc.GetClientList(remoteHeader, remoteData);

				// ����ڵ�˻�
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}
				
				// ��� ��Ʈ
				clientModel.ClientDataSet = remoteData.ClientDataSet.Copy();
				clientModel.ResultCnt   = remoteData.ResultCnt;
				clientModel.ResultCD    = remoteData.ResultCD;			
				
				_log.Debug("-----------------------------------------");
				_log.Debug("�����ü���౤���ָ����ȸ End");
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
		/// �����ü���౤�������� ����
		/// </summary>
		/// <param name="clientModel"></param>
		public void SetClientUpdate(ClientModel clientModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("�����ü���౤������������ Start");
				_log.Debug("-----------------------------------------");


				//�Էµ������� Validation �˻�
				if(clientModel.Comment.Length > 50) 
				{
					throw new FrameException("���� 50Bytes�� �ʰ��� �� �����ϴ�.");
				}				

				// ������ �ν��Ͻ� ����
				ClientServicePloxy.ClientService svc = new ClientServicePloxy.ClientService();
			
				// URL�� ������Ʈ
				svc.Url = _WebServiceUrl;
			
				// ����Ʈ �� ����
				ClientServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.ClientServicePloxy.HeaderModel();
				ClientServicePloxy.ClientModel remoteData   = new AdManagerClient.ClientServicePloxy.ClientModel();

				// ������� ��Ʈ
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;

				// ȣ��������Ʈ
				remoteData.MediaCode       = clientModel.MediaCode_C;
				remoteData.RapCode       = clientModel.RapCode_C;
				remoteData.AgencyCode       = clientModel.AgencyCode_C;
				remoteData.AdvertiserCode       = clientModel.AdvertiserCode_C;				
				remoteData.Comment       = clientModel.Comment;
				remoteData.UseYn     = clientModel.UseYn;				
					
                // ������ ȣ�� Ÿ�Ӿƿ�����
                svc.Timeout = FrameSystem.m_SystemTimeout;
			
				// ������ �޼ҵ� ȣ��
				remoteData = svc.SetClientUpdate(remoteHeader, remoteData);

				// ����ڵ�˻�
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// ��� ��Ʈ
				clientModel.ResultCnt   = remoteData.ResultCnt;
				clientModel.ResultCD    = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("�����ü���౤������������ End");
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
		/// �����ü���౤�����߰�
		/// </summary>
		/// <param name="clientModel"></param>
		/// <returns></returns>
		public void SetClientAdd(ClientModel clientModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("�����ü���౤�����߰� Start");
				_log.Debug("-----------------------------------------");
				
				if(clientModel.Comment.Length > 50) 
				{
					throw new FrameException("���� 50Bytes�� �ʰ��� �� �����ϴ�.");
				}			
				
				// ������ �ν��Ͻ� ����
				ClientServicePloxy.ClientService svc = new ClientServicePloxy.ClientService();
			
				// URL�� ������Ʈ
				svc.Url = _WebServiceUrl;
				
				// ����Ʈ �� ����
				ClientServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.ClientServicePloxy.HeaderModel();
				ClientServicePloxy.ClientModel remoteData   = new AdManagerClient.ClientServicePloxy.ClientModel();

				// ������� ��Ʈ
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;

				// ȣ��������Ʈ
				remoteData.MediaCode       = clientModel.MediaCode_C;
				remoteData.RapCode       = clientModel.RapCode_C;
				remoteData.AgencyCode       = clientModel.AgencyCode_C;
				remoteData.AdvertiserCode       = clientModel.AdvertiserCode_C;				
				remoteData.Comment       = clientModel.Comment;				
				remoteData.UseYn     = clientModel.UseYn;				
				
                // ������ ȣ�� Ÿ�Ӿƿ�����
                svc.Timeout = FrameSystem.m_SystemTimeout;
	
				// ������ �޼ҵ� ȣ��
				remoteData = svc.SetClientCreate(remoteHeader, remoteData);

				// ����ڵ�˻�
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// ��� ��Ʈ
				clientModel.ResultCnt   = remoteData.ResultCnt;
				clientModel.ResultCD    = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("�����ü���౤�����߰� End");
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
		/// �����ü���౤���� ����
		/// </summary>
		/// <param name="baseModel"></param>
		public void SetClientDelete(ClientModel clientModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("�����ü���౤���ֻ��� Start");
				_log.Debug("-----------------------------------------");

				// ������ �ν��Ͻ� ����
				ClientServicePloxy.ClientService svc = new ClientServicePloxy.ClientService();
			
				// URL�� ������Ʈ
				svc.Url = _WebServiceUrl;
				
				// ����Ʈ �� ����
				ClientServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.ClientServicePloxy.HeaderModel();
				ClientServicePloxy.ClientModel remoteData   = new AdManagerClient.ClientServicePloxy.ClientModel();

				// ������� ��Ʈ
				remoteHeader.ClientKey  = Header.ClientKey;
				remoteHeader.UserID     = Header.UserID;

				// ȣ��������Ʈ
				remoteData.MediaCode       = clientModel.MediaCode;
				remoteData.RapCode       = clientModel.RapCode;
				remoteData.AgencyCode       = clientModel.AgencyCode;
				remoteData.AdvertiserCode       = clientModel.AdvertiserCode;				
					
                // ������ ȣ�� Ÿ�Ӿƿ�����
                svc.Timeout = FrameSystem.m_SystemTimeout;

				// ������ �޼ҵ� ȣ��
				remoteData = svc.SetClientDelete(remoteHeader, remoteData);

				// ����ڵ�˻�
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// ��� ��Ʈ
				clientModel.ResultCnt   = remoteData.ResultCnt;
				clientModel.ResultCD    = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("�����ü���౤���ֻ��� End");
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
