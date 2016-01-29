// ===============================================================================
// Agency Manager  for Charites Project
//
// AgencyManager.cs
//
// ��������� ���񽺸� ȣ���մϴ�. 
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
	public class AgencyManager : BaseManager
	{
		/// <summary>
		/// ������
		/// </summary>
		/// <param name="systemModel"></param>
		/// <param name="commonModel"></param>

		public AgencyManager(SystemModel systemModel, CommonModel commonModel) : base(systemModel, commonModel)
		{
			_log = FrameSystem.oLog;
			_module = "Agency";
			_Host  = FrameSystem.m_WebServer_Host;
			_Port  = FrameSystem.m_WebServer_Port;
			_Path  = FrameSystem.m_WebServer_App + "/Contract/AgencyService.asmx";
		}

		/// <summary>
		/// �����������ȸ
		/// </summary>
		/// <param name="userModel"></param>
		public void GetAgencyList(AgencyModel agencyModel)
		{

			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("���������ȸ Start");
				_log.Debug("-----------------------------------------");
				
				// ������ �ν��Ͻ� ����
				AgencyServicePloxy.AgencyService svc = new AgencyServicePloxy.AgencyService();
			
				// URL�� ������Ʈ
				svc.Url = _WebServiceUrl;

				// ����Ʈ �� ����
				AgencyServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.AgencyServicePloxy.HeaderModel();
				AgencyServicePloxy.AgencyModel    remoteData   = new AdManagerClient.AgencyServicePloxy.AgencyModel();

				// ������� ��Ʈ
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;
				remoteHeader.UserLevel     = Header.UserLevel;
				remoteHeader.UserClass	   = Header.UserClass;

				// ȣ������ ��Ʈ
				remoteData.SearchKey       = agencyModel.SearchKey;
				remoteData.SearchAgencyType = agencyModel.SearchAgencyType;
				remoteData.SearchRap         = agencyModel.SearchRap;
				remoteData.SearchchkAdState_10	 = agencyModel.SearchchkAdState_10; 
				
                // ������ ȣ�� Ÿ�Ӿƿ�����
                svc.Timeout = FrameSystem.m_SystemTimeout;

				// ������ �޼ҵ� ȣ��
				remoteData = svc.GetAgencyList(remoteHeader, remoteData);

				// ����ڵ�˻�
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// ��� ��Ʈ
				agencyModel.AgencyDataSet = remoteData.AgencyDataSet.Copy();
				agencyModel.ResultCnt   = remoteData.ResultCnt;
				agencyModel.ResultCD    = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("���������ȸ End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":GetAgencyList():" + fe.ErrCode + ":" + fe.ResultMsg);
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
		public bool GetAgencyDetail(BaseModel baseModel)
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
		public void SetAgencyUpdate(AgencyModel agencyModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("������������� Start");
				_log.Debug("-----------------------------------------");

				//�Էµ������� Validation �˻�
				if(agencyModel.AgencyCode.Length < 1) 
				{
					throw new FrameException("����簡 ���õ��� �ʾҽ��ϴ�.");
				}
				if(agencyModel.AgencyName.Length > 40) 
				{
					throw new FrameException("�������� ���̴� 40Bytes�� �ʰ��� �� �����ϴ�.");
				}
				if(agencyModel.Tell.Length > 15) 
				{
					throw new FrameException("��ȭ��ȣ�� ���̴� 50Bytes�� �ʰ��� �� �����ϴ�.");
				}
				if(agencyModel.Address.Length > 50) 
				{
					throw new FrameException("�ּ��� ���̴� 50Bytes�� �ʰ��� �� �����ϴ�.");
				}				
				if(agencyModel.Comment.Length > 50) 
				{
					throw new FrameException("������ ���̴� 50Bytes�� �ʰ��� �� �����ϴ�.");
				}

				// ������ �ν��Ͻ� ����
				AgencyServicePloxy.AgencyService svc = new AgencyServicePloxy.AgencyService();
		
				// URL�� ������Ʈ
				svc.Url = _WebServiceUrl;
	
				// ����Ʈ �� ����
				AgencyServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.AgencyServicePloxy.HeaderModel();
				AgencyServicePloxy.AgencyModel    remoteData   = new AdManagerClient.AgencyServicePloxy.AgencyModel();

				// ������� ��Ʈ
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;
				remoteHeader.UserLevel     = Header.UserLevel;

				// ȣ��������Ʈ
				remoteData.AgencyCode	   = agencyModel.AgencyCode;
				remoteData.AgencyName      = agencyModel.AgencyName;
				remoteData.RapCode         = agencyModel.RapCode;
				remoteData.AgencyType      = agencyModel.AgencyType;
				remoteData.Address         = agencyModel.Address;
				remoteData.Tell            = agencyModel.Tell;
				remoteData.Comment         = agencyModel.Comment;
				remoteData.UseYn           = agencyModel.UseYn;

                // ������ ȣ�� Ÿ�Ӿƿ�����
                svc.Timeout = FrameSystem.m_SystemTimeout;

				// ������ �޼ҵ� ȣ��
				remoteData = svc.SetAgencyUpdate(remoteHeader, remoteData);

				// ����ڵ�˻�
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// ��� ��Ʈ
				agencyModel.ResultCnt   = remoteData.ResultCnt;
				agencyModel.ResultCD    = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("������������� End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":SetAgntUpdate():" + fe.ErrCode + ":" + fe.ResultMsg);
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
		public void SetAgencyAdd(AgencyModel agencyModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("������߰� Start");
				_log.Debug("-----------------------------------------");

				//�Էµ������� Validation �˻�
				if(agencyModel.AgencyName.Length > 40) 
				{
					throw new FrameException("�������� ���̴� 40Bytes�� �ʰ��� �� �����ϴ�.");
				}
				if(agencyModel.Tell.Length > 15) 
				{
					throw new FrameException("��ȭ��ȣ�� ���̴� 50Bytes�� �ʰ��� �� �����ϴ�.");
				}
				if(agencyModel.Address.Length > 50) 
				{
					throw new FrameException("�ּ��� ���̴� 50Bytes�� �ʰ��� �� �����ϴ�.");
				}				
				if(agencyModel.Comment.Length > 50) 
				{
					throw new FrameException("������ ���̴� 50Bytes�� �ʰ��� �� �����ϴ�.");
				}

	

				// ������ �ν��Ͻ� ����
				AgencyServicePloxy.AgencyService svc = new AgencyServicePloxy.AgencyService();
			
				// URL�� ������Ʈ
				svc.Url = _WebServiceUrl;

				// ����Ʈ �� ����
				AgencyServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.AgencyServicePloxy.HeaderModel();
				AgencyServicePloxy.AgencyModel    remoteData   = new AdManagerClient.AgencyServicePloxy.AgencyModel();

				// ������� ��Ʈ
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;
				remoteHeader.UserLevel     = Header.UserLevel;

				// ȣ��������Ʈ
				remoteData.AgencyCode	   = agencyModel.AgencyCode;
				remoteData.AgencyName       = agencyModel.AgencyName;
				remoteData.RapCode        = agencyModel.RapCode;
				remoteData.AgencyType       = agencyModel.AgencyType;
				remoteData.Address         = agencyModel.Address;
				remoteData.Tell            = agencyModel.Tell;
				remoteData.Comment         = agencyModel.Comment;
				remoteData.UseYn           = agencyModel.UseYn;

                // ������ ȣ�� Ÿ�Ӿƿ�����
                svc.Timeout = FrameSystem.m_SystemTimeout;

				// ������ �޼ҵ� ȣ��
				remoteData = svc.SetAgencyCreate(remoteHeader, remoteData);

				// ����ڵ�˻�
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// ��� ��Ʈ
				agencyModel.ResultCnt   = remoteData.ResultCnt;
				agencyModel.ResultCD    = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("������߰� End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":SetAgebtCreate():" + fe.ErrCode + ":" + fe.ResultMsg);
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
		public void SetAgencyDelete(AgencyModel agencyModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("�������� Start");
				_log.Debug("-----------------------------------------");


				//�Էµ������� Validation �˻�
				if(agencyModel.AgencyCode.Length < 1) 
				{
					throw new FrameException("����簡 ���õ��� �ʾҽ��ϴ�.");
				}

				// ������ �ν��Ͻ� ����
				AgencyServicePloxy.AgencyService svc = new AgencyServicePloxy.AgencyService();

				// URL�� ������Ʈ
				svc.Url = _WebServiceUrl;
			
				// ����Ʈ �� ����
				AgencyServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.AgencyServicePloxy.HeaderModel();
				AgencyServicePloxy.AgencyModel    remoteData   = new AdManagerClient.AgencyServicePloxy.AgencyModel();

				// ������� ��Ʈ
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;
				remoteHeader.UserLevel     = Header.UserLevel;

				// ȣ��������Ʈ
				remoteData.AgencyCode	   = agencyModel.AgencyCode;
				
                // ������ ȣ�� Ÿ�Ӿƿ�����
                svc.Timeout = FrameSystem.m_SystemTimeout;

				// ������ �޼ҵ� ȣ��
				remoteData = svc.SetAgencyDelete(remoteHeader, remoteData);

				// ����ڵ�˻�
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// ��� ��Ʈ
				agencyModel.ResultCnt   = remoteData.ResultCnt;
				agencyModel.ResultCD    = remoteData.ResultCD;


				_log.Debug("-----------------------------------------");
				_log.Debug("�������� End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":SetAgencyDelete():" + fe.ErrCode + ":" + fe.ResultMsg);
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
