// ===============================================================================
//
// SchPublishManager.cs
//
// ���������� ���񽺸� ȣ���մϴ�. 
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
	/// ���������� �����񽺸� ȣ���մϴ�. 
	/// </summary>
	public class SchPublishManager : BaseManager
	{
		/// <summary>
		/// ������
		/// </summary>
		/// <param name="systemModel"></param>
		/// <param name="commonModel"></param>

		public SchPublishManager(SystemModel systemModel, CommonModel commonModel) : base(systemModel, commonModel)
		{
			_log = FrameSystem.oLog;
			_module = "SchPublish";
			_Host  = FrameSystem.m_WebServer_Host;
			_Port  = FrameSystem.m_WebServer_Port;
			_Path  = FrameSystem.m_WebServer_App + "/Schedule/SchPublishService.asmx";
		}

		/// <summary>
		/// �����̷� ��ȸ
		/// </summary>
		/// <param name="schPublishModel"></param>
		public void GetSchPublishList(SchPublishModel schPublishModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("�����̷� ��ȸ Start");
				_log.Debug("-----------------------------------------");
				
				// ������ �ν��Ͻ� ����
				SchPublishServicePloxy.SchPublishService svc = new SchPublishServicePloxy.SchPublishService();
			
				// ������URL ���� ��Ʈ
				svc.Url = _WebServiceUrl;

				// ����Ʈ �� ����
				SchPublishServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.SchPublishServicePloxy.HeaderModel();
				SchPublishServicePloxy.SchPublishModel   remoteData   = new AdManagerClient.SchPublishServicePloxy.SchPublishModel();

				// ������� ��Ʈ
				remoteHeader.ClientKey		= Header.ClientKey;
				remoteHeader.UserID			= Header.UserID;
				remoteHeader.UserLevel		= Header.UserLevel;

				// ȣ������ ��Ʈ
				remoteData.SearchMediaCode	= schPublishModel.SearchMediaCode;	  				
				
				// ������ ȣ�� Ÿ�Ӿƿ�����
				svc.Timeout = FrameSystem.m_SystemTimeout;
				// ������ �޼ҵ� ȣ��
				remoteData = svc.GetSchPublishList(remoteHeader, remoteData);

				// ����ڵ�˻�
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// ��� ��Ʈ
				schPublishModel.PublishDataSet = remoteData.PublishDataSet.Copy();
				schPublishModel.ResultCnt   = remoteData.ResultCnt;
				schPublishModel.ResultCD    = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("�����̷� ��ȸ End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":GetScheduleList():" + fe.ErrCode + ":" + fe.ResultMsg);
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
		/// ����Ȳ ��ȸ
		/// </summary>
		/// <param name="schPublishModel"></param>
		public void GetScheduleList(SchPublishModel schPublishModel)
		{

			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("�����̷� ��ȸ Start");
				_log.Debug("-----------------------------------------");
				
				// ������ �ν��Ͻ� ����
				SchPublishServicePloxy.SchPublishService svc = new SchPublishServicePloxy.SchPublishService();
			
				// ������URL ���� ��Ʈ
				svc.Url = _WebServiceUrl;

				// ����Ʈ �� ����
				SchPublishServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.SchPublishServicePloxy.HeaderModel();
				SchPublishServicePloxy.SchPublishModel   remoteData   = new AdManagerClient.SchPublishServicePloxy.SchPublishModel();

				// ������� ��Ʈ
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;
				remoteHeader.UserLevel     = Header.UserLevel;

				// ȣ������ ��Ʈ
				remoteData.SearchMediaCode		 = schPublishModel.SearchMediaCode;
	  			remoteData.AckNo				 = schPublishModel.AckNo;	
				
				// ������ ȣ�� Ÿ�Ӿƿ�����
				svc.Timeout = FrameSystem.m_SystemTimeout;
				// ������ �޼ҵ� ȣ��
				remoteData = svc.GetScheduleList(remoteHeader, remoteData);

				// ����ڵ�˻�
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// ��� ��Ʈ
				schPublishModel.ScheduleDataSet = remoteData.ScheduleDataSet.Copy();
				schPublishModel.ResultCnt   = remoteData.ResultCnt;
				schPublishModel.ResultCD    = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("�����̷� ��ȸ End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":GetScheduleList():" + fe.ErrCode + ":" + fe.ResultMsg);
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
		/// [S1] ������λ��� ��ȸ
		/// �ϴ� 10:�������(�ε�,�߰�,����), �������� ���Ŀ� �ٽ� ������ ����
		/// </summary>
		/// <param name="schPublishModel"></param>
		/// <param name="adSchType">�������� [0]Ȩ, [10]���, [20]OAP, [30]Ȩ(Ű��), [40]Ȩ(Ÿ��) </param>
		public void GetPublishState(SchPublishModel schPublishModel, int adSchType)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("������λ��� ��ȸ Start");
				_log.Debug("-----------------------------------------");
				
				// ������ �ν��Ͻ� ����
				SchPublishServicePloxy.SchPublishService svc = new SchPublishServicePloxy.SchPublishService();
			
				// ������URL ���� ��Ʈ
				svc.Url = _WebServiceUrl;

				// ����Ʈ �� ����
				SchPublishServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.SchPublishServicePloxy.HeaderModel();
				SchPublishServicePloxy.SchPublishModel   remoteData   = new AdManagerClient.SchPublishServicePloxy.SchPublishModel();

				// ������� ��Ʈ
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;
				remoteHeader.UserLevel     = Header.UserLevel;

				// ȣ������ ��Ʈ
				remoteData.SearchMediaCode		 = schPublishModel.SearchMediaCode;	  				
				
				// ������ ȣ�� Ÿ�Ӿƿ�����
				svc.Timeout = FrameSystem.m_SystemTimeout;
				// ������ �޼ҵ� ȣ��
				if(			adSchType.Equals(0) )		remoteData = svc.GetHomePublishState(remoteHeader, remoteData);
				else if(	adSchType.Equals(10))		remoteData = svc.GetCmPublishState(remoteHeader, remoteData);
				else if(	adSchType.Equals(20))		remoteData = svc.GetOapPublishState(remoteHeader, remoteData);
				else if(	adSchType.Equals(30))		remoteData = svc.GetHomeKidsPublishState(remoteHeader, remoteData);
                else if(    adSchType.Equals(40))       remoteData = svc.GetHomeTargetPublishState(remoteHeader, remoteData);
				else	throw new FrameException("���������� �߸��Ǿ����ϴ�", _module, "3000");

				// ����ڵ�˻�
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// ��� ��Ʈ
				schPublishModel.AckNo       = remoteData.AckNo;
				schPublishModel.State       = remoteData.State;

				schPublishModel.ResultCnt   = remoteData.ResultCnt;
				schPublishModel.ResultCD    = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("������λ��� ��ȸ End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":GetScheduleList():" + fe.ErrCode + ":" + fe.ResultMsg);
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
		/// ����������
		/// </summary>
		/// <param name="schPublishModel"></param>
		public void SetScheduleAck(SchPublishModel schPublishModel)
		{

			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("���������� Start");
				_log.Debug("-----------------------------------------");
				
				// ������ �ν��Ͻ� ����
				SchPublishServicePloxy.SchPublishService svc = new SchPublishServicePloxy.SchPublishService();
			
				// ������URL ���� ��Ʈ
				svc.Url = _WebServiceUrl;

				// ����Ʈ �� ����
				SchPublishServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.SchPublishServicePloxy.HeaderModel();
				SchPublishServicePloxy.SchPublishModel   remoteData   = new AdManagerClient.SchPublishServicePloxy.SchPublishModel();

				// ������� ��Ʈ
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;
				remoteHeader.UserLevel     = Header.UserLevel;

				// ȣ������ ��Ʈ
				remoteData.AckNo		 = schPublishModel.AckNo;	  				
				remoteData.AckDesc		 = schPublishModel.AckDesc;	  				
				
				// ������ ȣ�� Ÿ�Ӿƿ�����
				svc.Timeout = FrameSystem.m_SystemTimeout;

				// ������ �޼ҵ� ȣ��
				remoteData = svc.SetScheduleAck(remoteHeader, remoteData);

				// ����ڵ�˻�
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// ��� ��Ʈ
				schPublishModel.ResultCD    = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("���������� End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":SetScheduleAck():" + fe.ErrCode + ":" + fe.ResultMsg);
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
		/// ���������� ���
		/// </summary>
		/// <param name="schPublishModel"></param>
		public void SetScheduleAckCancel(SchPublishModel schPublishModel)
		{

			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("���������� ��� Start");
				_log.Debug("-----------------------------------------");
				
				// ������ �ν��Ͻ� ����
				SchPublishServicePloxy.SchPublishService svc = new SchPublishServicePloxy.SchPublishService();
			
				// ������URL ���� ��Ʈ
				svc.Url = _WebServiceUrl;

				// ����Ʈ �� ����
				SchPublishServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.SchPublishServicePloxy.HeaderModel();
				SchPublishServicePloxy.SchPublishModel   remoteData   = new AdManagerClient.SchPublishServicePloxy.SchPublishModel();

				// ������� ��Ʈ
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;
				remoteHeader.UserLevel     = Header.UserLevel;

				// ȣ������ ��Ʈ
				remoteData.AckNo		 = schPublishModel.AckNo;	  				
				remoteData.AckDesc		 = schPublishModel.AckDesc;	  				
				
				// ������ ȣ�� Ÿ�Ӿƿ�����
				svc.Timeout = FrameSystem.m_SystemTimeout;

				// ������ �޼ҵ� ȣ��
				remoteData = svc.SetScheduleAckCancel(remoteHeader, remoteData);

				// ����ڵ�˻�
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// ��� ��Ʈ
				schPublishModel.ResultCD    = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("���������� ��� End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":SetScheduleAckCancel():" + fe.ErrCode + ":" + fe.ResultMsg);
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
		/// ����˼�����
		/// </summary>
		/// <param name="schPublishModel"></param>
		public void SetScheduleChk(SchPublishModel schPublishModel)
		{

			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("����˼����� Start");
				_log.Debug("-----------------------------------------");
				
				// ������ �ν��Ͻ� ����
				SchPublishServicePloxy.SchPublishService svc = new SchPublishServicePloxy.SchPublishService();
			
				// ������URL ���� ��Ʈ
				svc.Url = _WebServiceUrl;

				// ����Ʈ �� ����
				SchPublishServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.SchPublishServicePloxy.HeaderModel();
				SchPublishServicePloxy.SchPublishModel   remoteData   = new AdManagerClient.SchPublishServicePloxy.SchPublishModel();

				// ������� ��Ʈ
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;
				remoteHeader.UserLevel     = Header.UserLevel;

				// ȣ������ ��Ʈ
				remoteData.AckNo		 = schPublishModel.AckNo;	  				
				remoteData.ChkDesc		 = schPublishModel.ChkDesc;	  				
				
				// ������ ȣ�� Ÿ�Ӿƿ�����
				svc.Timeout = FrameSystem.m_SystemTimeout;

				// ������ �޼ҵ� ȣ��
				remoteData = svc.SetScheduleChk(remoteHeader, remoteData);

				// ����ڵ�˻�
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// ��� ��Ʈ
				schPublishModel.ResultCD    = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("����˼����� End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":SetScheduleChk():" + fe.ErrCode + ":" + fe.ResultMsg);
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
		/// ����˼����� ���
		/// </summary>
		/// <param name="schPublishModel"></param>
		public void SetScheduleChkCancel(SchPublishModel schPublishModel)
		{

			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("����˼����� ��� Start");
				_log.Debug("-----------------------------------------");
				
				// ������ �ν��Ͻ� ����
				SchPublishServicePloxy.SchPublishService svc = new SchPublishServicePloxy.SchPublishService();
			
				// ������URL ���� ��Ʈ
				svc.Url = _WebServiceUrl;

				// ����Ʈ �� ����
				SchPublishServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.SchPublishServicePloxy.HeaderModel();
				SchPublishServicePloxy.SchPublishModel   remoteData   = new AdManagerClient.SchPublishServicePloxy.SchPublishModel();

				// ������� ��Ʈ
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;
				remoteHeader.UserLevel     = Header.UserLevel;

				// ȣ������ ��Ʈ
				remoteData.AckNo		 = schPublishModel.AckNo;	  				
				remoteData.ChkDesc		 = schPublishModel.ChkDesc;	  				
				
				// ������ ȣ�� Ÿ�Ӿƿ�����
				svc.Timeout = FrameSystem.m_SystemTimeout;

				// ������ �޼ҵ� ȣ��
				remoteData = svc.SetScheduleChkCancel(remoteHeader, remoteData);

				// ����ڵ�˻�
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// ��� ��Ʈ
				schPublishModel.ResultCD    = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("����˼����� ��� End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":SetScheduleChkCancel():" + fe.ErrCode + ":" + fe.ResultMsg);
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
		/// �����������
		/// </summary>
		/// <param name="schPublishModel"></param>
		public void SetSchedulePublish(SchPublishModel schPublishModel)
		{

			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("����������� Start");
				_log.Debug("-----------------------------------------");
				
				// ������ �ν��Ͻ� ����
				SchPublishServicePloxy.SchPublishService svc = new SchPublishServicePloxy.SchPublishService();
			
				// ������URL ���� ��Ʈ
				svc.Url = _WebServiceUrl;

				// ����Ʈ �� ����
				SchPublishServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.SchPublishServicePloxy.HeaderModel();
				SchPublishServicePloxy.SchPublishModel   remoteData   = new AdManagerClient.SchPublishServicePloxy.SchPublishModel();

				// ������� ��Ʈ
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;
				remoteHeader.UserLevel     = Header.UserLevel;

				// ȣ������ ��Ʈ
				remoteData.AckNo		 = schPublishModel.AckNo;	  				
				remoteData.PublishDesc	 = schPublishModel.PublishDesc;	  				
				
				// ������ ȣ�� Ÿ�Ӿƿ�����
				svc.Timeout = FrameSystem.m_SystemTimeout;

				// ������ �޼ҵ� ȣ��
				remoteData = svc.SetSchedulePublish(remoteHeader, remoteData);

				// ����ڵ�˻�
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// ��� ��Ʈ
				schPublishModel.ResultCD    = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("����������� End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":SetSchedulePublish():" + fe.ErrCode + ":" + fe.ResultMsg);
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
