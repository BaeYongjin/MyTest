// ===============================================================================
//
// FilePublishManager.cs
//
// ���Ϲ������� ���񽺸� ȣ���մϴ�. 
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
	/// ���Ϲ������� �����񽺸� ȣ���մϴ�. 
	/// </summary>
	public class FilePublishManager : BaseManager
	{
		/// <summary>
		/// ������
		/// </summary>
		/// <param name="systemModel"></param>
		/// <param name="commonModel"></param>

		public FilePublishManager(SystemModel systemModel, CommonModel commonModel) : base(systemModel, commonModel)
		{
			_log = FrameSystem.oLog;
			_module = "FilePublish";
			_Host  = FrameSystem.m_WebServer_Host;
			_Port  = FrameSystem.m_WebServer_Port;
			_Path  = FrameSystem.m_WebServer_App + "/Contract/FilePublishService.asmx";
		}


		/// <summary>
		/// ���γ��� ��ȸ
		/// </summary>
		/// <param name="filePublishModel"></param>
		public void GetPublishList(FilePublishModel filePublishModel)
		{

			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("���γ��� ��ȸ Start");
				_log.Debug("-----------------------------------------");
				
				// ������ �ν��Ͻ� ����
				FilePublishServicePloxy.FilePublishService svc = new FilePublishServicePloxy.FilePublishService();
			
				// ������URL ���� ��Ʈ
				svc.Url = _WebServiceUrl;

				// ����Ʈ �� ����
				FilePublishServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.FilePublishServicePloxy.HeaderModel();
				FilePublishServicePloxy.FilePublishModel   remoteData   = new AdManagerClient.FilePublishServicePloxy.FilePublishModel();

				// ������� ��Ʈ
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;
				remoteHeader.UserLevel     = Header.UserLevel;

				// ȣ������ ��Ʈ
				remoteData.SearchMediaCode		 = filePublishModel.SearchMediaCode;	  				
				
				// ������ ȣ�� Ÿ�Ӿƿ�����
				svc.Timeout = FrameSystem.m_SystemTimeout;
				// ������ �޼ҵ� ȣ��
				remoteData = svc.GetPublishList(remoteHeader, remoteData);

				// ����ڵ�˻�
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// ��� ��Ʈ
				filePublishModel.PublishDataSet = remoteData.PublishDataSet.Copy();
				filePublishModel.ResultCnt   = remoteData.ResultCnt;
				filePublishModel.ResultCD    = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("�����̷� ��ȸ End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":GetPublishList():" + fe.ErrCode + ":" + fe.ResultMsg);
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
		/// ����� ��ȸ
		/// </summary>
		/// <param name="filePublishModel"></param>
		public void GetPublishHistory(FilePublishModel filePublishModel)
		{

			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("����� ��ȸ Start");
				_log.Debug("-----------------------------------------");
				
				// ������ �ν��Ͻ� ����
				FilePublishServicePloxy.FilePublishService svc = new FilePublishServicePloxy.FilePublishService();
			
				// ������URL ���� ��Ʈ
				svc.Url = _WebServiceUrl;

				// ����Ʈ �� ����
				FilePublishServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.FilePublishServicePloxy.HeaderModel();
				FilePublishServicePloxy.FilePublishModel   remoteData   = new AdManagerClient.FilePublishServicePloxy.FilePublishModel();

				// ������� ��Ʈ
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;
				remoteHeader.UserLevel     = Header.UserLevel;

				// ȣ������ ��Ʈ
				remoteData.MediaCode  = filePublishModel.MediaCode;	  				
				remoteData.AckNo      = filePublishModel.AckNo;	  				
				
				// ������ ȣ�� Ÿ�Ӿƿ�����
				svc.Timeout = FrameSystem.m_SystemTimeout;
				// ������ �޼ҵ� ȣ��
				remoteData = svc.GetPublishHistory(remoteHeader, remoteData);

				// ����ڵ�˻�
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// ��� ��Ʈ
				filePublishModel.HistoryDataSet = remoteData.HistoryDataSet.Copy();
				filePublishModel.ResultCnt   = remoteData.ResultCnt;
				filePublishModel.ResultCD    = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("����� ��ȸ End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":GetPublishHistory():" + fe.ErrCode + ":" + fe.ResultMsg);
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
		/// ������λ��� ��ȸ
		/// </summary>
		/// <param name="filePublishModel"></param>
		public void GetPublishState(FilePublishModel filePublishModel)
		{

			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("������λ��� ��ȸ Start");
				_log.Debug("-----------------------------------------");
				
				// ������ �ν��Ͻ� ����
				FilePublishServicePloxy.FilePublishService svc = new FilePublishServicePloxy.FilePublishService();
			
				// ������URL ���� ��Ʈ
				svc.Url = _WebServiceUrl;

				// ����Ʈ �� ����
				FilePublishServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.FilePublishServicePloxy.HeaderModel();
				FilePublishServicePloxy.FilePublishModel   remoteData   = new AdManagerClient.FilePublishServicePloxy.FilePublishModel();

				// ������� ��Ʈ
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;
				remoteHeader.UserLevel     = Header.UserLevel;

				// ȣ������ ��Ʈ
				remoteData.SearchMediaCode		 = filePublishModel.SearchMediaCode;	  				
				
				// ������ ȣ�� Ÿ�Ӿƿ�����
				svc.Timeout = FrameSystem.m_SystemTimeout;
				// ������ �޼ҵ� ȣ��
				remoteData = svc.GetPublishState(remoteHeader, remoteData);

				// ����ڵ�˻�
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// ��� ��Ʈ
				filePublishModel.AckNo       = remoteData.AckNo;
				filePublishModel.State       = remoteData.State;

				filePublishModel.ResultCnt   = remoteData.ResultCnt;
				filePublishModel.ResultCD    = remoteData.ResultCD;

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
		/// ���Ϲ�������
		/// </summary>
		/// <param name="filePublishModel"></param>
		public void SetFilePublish(FilePublishModel filePublishModel)
		{

			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("���Ϲ������� Start");
				_log.Debug("-----------------------------------------");
				
				// ������ �ν��Ͻ� ����
				FilePublishServicePloxy.FilePublishService svc = new FilePublishServicePloxy.FilePublishService();
			
				// ������URL ���� ��Ʈ
				svc.Url = _WebServiceUrl;

				// ����Ʈ �� ����
				FilePublishServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.FilePublishServicePloxy.HeaderModel();
				FilePublishServicePloxy.FilePublishModel   remoteData   = new AdManagerClient.FilePublishServicePloxy.FilePublishModel();

				// ������� ��Ʈ
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;
				remoteHeader.UserLevel     = Header.UserLevel;

				// ȣ������ ��Ʈ
				remoteData.AckNo		 = filePublishModel.AckNo;	  				
				remoteData.PublishDesc	 = filePublishModel.PublishDesc;	  				
				remoteData.MediaCode     = filePublishModel.MediaCode;	  				
				
				// ������ ȣ�� Ÿ�Ӿƿ�����
				svc.Timeout = FrameSystem.m_SystemTimeout;

				// ������ �޼ҵ� ȣ��
				remoteData = svc.SetFilePublish(remoteHeader, remoteData);

				// ����ڵ�˻�
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// ��� ��Ʈ
				filePublishModel.ResultCD    = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("���Ϲ������� End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":SetFilePublish():" + fe.ErrCode + ":" + fe.ResultMsg);
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
		/// ���κ� ���ϸ���Ʈ ��ȸ
		/// </summary>
		/// <param name="filePublishModel"></param>
		public void GetPublishFileList(FilePublishModel filePublishModel)
		{

			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("���ϸ���Ʈ ��ȸ Start");
				_log.Debug("-----------------------------------------");
				
				// ������ �ν��Ͻ� ����
				FilePublishServicePloxy.FilePublishService svc = new FilePublishServicePloxy.FilePublishService();
			
				// ������URL ���� ��Ʈ
				svc.Url = _WebServiceUrl;

				// ����Ʈ �� ����
				FilePublishServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.FilePublishServicePloxy.HeaderModel();
				FilePublishServicePloxy.FilePublishModel   remoteData   = new AdManagerClient.FilePublishServicePloxy.FilePublishModel();

				// ������� ��Ʈ
				remoteHeader.ClientKey  = Header.ClientKey;
				remoteHeader.UserID     = Header.UserID;
				remoteHeader.UserLevel  = Header.UserLevel;

				// ȣ������ ��Ʈ
				remoteData.MediaCode    = filePublishModel.MediaCode;	  				
				remoteData.AckNo        = filePublishModel.AckNo;
                remoteData.ReserveDt    = filePublishModel.ReserveDt;
				
				// ������ ȣ�� Ÿ�Ӿƿ�����
				svc.Timeout = FrameSystem.m_SystemTimeout;
				// ������ �޼ҵ� ȣ��
				remoteData = svc.GetPublishFileList(remoteHeader, remoteData);

				// ����ڵ�˻�
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// ��� ��Ʈ
				filePublishModel.FileListDataSet = remoteData.FileListDataSet.Copy();
				filePublishModel.ResultCnt   = remoteData.ResultCnt;
				filePublishModel.ResultCD    = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("���ϸ���Ʈ ��ȸ End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":GetPublishFileList():" + fe.ErrCode + ":" + fe.ResultMsg);
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


		#region [ �����۾� ó�� �κ� ]
		
		/// <summary>
		/// �������� ����Ʈ ��������
		/// </summary>
		/// <param name="filePublishModel"></param>
		public void GetReserveFileList(FilePublishModel filePublishModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("�������� ����Ʈ ��ȸ Start");
				_log.Debug("-----------------------------------------");
				
				FilePublishServicePloxy.FilePublishService svc = new FilePublishServicePloxy.FilePublishService();
				svc.Url = _WebServiceUrl;
				svc.Timeout = FrameSystem.m_SystemTimeout;

				FilePublishServicePloxy.HeaderModel			remoteHeader = new AdManagerClient.FilePublishServicePloxy.HeaderModel();
				FilePublishServicePloxy.FilePublishModel	remoteData   = new AdManagerClient.FilePublishServicePloxy.FilePublishModel();

				// ������� ��Ʈ
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;
				remoteHeader.UserLevel     = Header.UserLevel;

				// ȣ������ ��Ʈ
				remoteData.MediaCode  = filePublishModel.MediaCode;	  				
				remoteData.AckNo      = filePublishModel.AckNo;	  				
								
				// ������ �޼ҵ� ȣ��
				remoteData = svc.GetReserveFiles(remoteHeader, remoteData);

				// ����ڵ�˻�
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// ��� ��Ʈ
				filePublishModel.FileListDataSet = remoteData.FileListDataSet.Copy();
				filePublishModel.ResultCnt   = remoteData.ResultCnt;
				filePublishModel.ResultCD    = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("�������� ����Ʈ ��ȸ End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":GetReserveFileList():" + fe.ErrCode + ":" + fe.ResultMsg);
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
		/// �����۾� ����Ʈ ��������
		/// </summary>
		/// <param name="filePublishModel"></param>
		public void GetReserveWorkList(FilePublishModel filePublishModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("�����۾� ����Ʈ ��ȸ Start");
				_log.Debug("-----------------------------------------");
				
				FilePublishServicePloxy.FilePublishService svc = new FilePublishServicePloxy.FilePublishService();
				svc.Url = _WebServiceUrl;
				svc.Timeout = FrameSystem.m_SystemTimeout;

				FilePublishServicePloxy.HeaderModel			remoteHeader = new AdManagerClient.FilePublishServicePloxy.HeaderModel();
				FilePublishServicePloxy.FilePublishModel	remoteData   = new AdManagerClient.FilePublishServicePloxy.FilePublishModel();

				// ������� ��Ʈ
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;
				remoteHeader.UserLevel     = Header.UserLevel;

				// ȣ������ ��Ʈ
				remoteData.MediaCode  = filePublishModel.MediaCode;	  				
				remoteData.AckNo      = filePublishModel.AckNo;	  				
								
				// ������ �޼ҵ� ȣ��
				remoteData = svc.GetReserveWorks(remoteHeader, remoteData);

				// ����ڵ�˻�
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// ��� ��Ʈ
				filePublishModel.FileListDataSet = remoteData.FileListDataSet.Copy();
				filePublishModel.ResultCnt   = remoteData.ResultCnt;
				filePublishModel.ResultCD    = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("�����۾� ����Ʈ ��ȸ End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":GetReserveWorkList():" + fe.ErrCode + ":" + fe.ResultMsg);
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
		/// �����۾� ������ ��������
		/// </summary>
		/// <param name="filePublishModel"></param>
		public void GetReserveWorkDetail(FilePublishModel filePublishModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("�����۾� ������ ��ȸ Start");
				_log.Debug("-----------------------------------------");
				
				FilePublishServicePloxy.FilePublishService svc = new FilePublishServicePloxy.FilePublishService();
				svc.Url = _WebServiceUrl;
				svc.Timeout = FrameSystem.m_SystemTimeout;

				FilePublishServicePloxy.HeaderModel			remoteHeader = new AdManagerClient.FilePublishServicePloxy.HeaderModel();
				FilePublishServicePloxy.FilePublishModel	remoteData   = new AdManagerClient.FilePublishServicePloxy.FilePublishModel();

				// ������� ��Ʈ
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;
				remoteHeader.UserLevel     = Header.UserLevel;

				// ȣ������ ��Ʈ
				remoteData.MediaCode	= filePublishModel.MediaCode;	  				
				remoteData.AckNo		= filePublishModel.AckNo;
				remoteData.ReserveDt	= filePublishModel.ReserveDt;
												
				// ������ �޼ҵ� ȣ��
				remoteData = svc.getReserveWorkSelect(remoteHeader, remoteData);

				// ����ڵ�˻�
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// ��� ��Ʈ
				filePublishModel.State			= remoteData.State;
				filePublishModel.ReserveDt		= remoteData.ReserveDt;
				filePublishModel.ReserveUserName= remoteData.ReserveUserName;
				filePublishModel.ModifyUserName	= remoteData.ModifyUserName;
				filePublishModel.ModDt			= remoteData.ModDt;
				filePublishModel.PublishDesc	= remoteData.PublishDesc;
				

				filePublishModel.ResultCnt		= remoteData.ResultCnt;
				filePublishModel.ResultCD		= remoteData.ResultCD;
				filePublishModel.ResultDesc		= remoteData.ResultDesc;

				_log.Debug("-----------------------------------------");
				_log.Debug("�����۾� ������ ��ȸ End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":GetReserveWorkList():" + fe.ErrCode + ":" + fe.ResultMsg);
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
		/// �����۾� ������ ����
		/// </summary>
		/// <param name="filePublishModel"></param>
		public void SetReserveWorkDetail(FilePublishModel filePublishModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("�����۾� ������ ���� Start");
				_log.Debug("-----------------------------------------");
				
				FilePublishServicePloxy.FilePublishService svc = new FilePublishServicePloxy.FilePublishService();
				svc.Url = _WebServiceUrl;
				svc.Timeout = FrameSystem.m_SystemTimeout;

				FilePublishServicePloxy.HeaderModel			remoteHeader = new AdManagerClient.FilePublishServicePloxy.HeaderModel();
				FilePublishServicePloxy.FilePublishModel	remoteData   = new AdManagerClient.FilePublishServicePloxy.FilePublishModel();

				// ������� ��Ʈ
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;
				remoteHeader.UserLevel     = Header.UserLevel;

				// ȣ������ ��Ʈ
				remoteData.MediaCode	= filePublishModel.MediaCode;	  				
				remoteData.AckNo		= filePublishModel.AckNo;
				remoteData.ReserveDt	= filePublishModel.ReserveDt;
				remoteData.PublishDesc	= filePublishModel.PublishDesc;
				remoteData.State		= filePublishModel.State;
				remoteData.SearchReserveKey = filePublishModel.SearchReserveKey;
												
				// ������ �޼ҵ� ȣ��
				remoteData = svc.setReserveWorkUpdate(remoteHeader, remoteData);

				// ����ڵ�˻�
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				filePublishModel.ResultCnt		= remoteData.ResultCnt;
				filePublishModel.ResultCD		= remoteData.ResultCD;
				filePublishModel.ResultDesc		= remoteData.ResultDesc;

				_log.Debug("-----------------------------------------");
				_log.Debug("�����۾� ������ ��ȸ End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":GetReserveWorkList():" + fe.ErrCode + ":" + fe.ResultMsg);
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
		/// �����۾� ������ �߰�
		/// </summary>
		/// <param name="filePublishModel"></param>
		public void NewReserveWorkDetail(FilePublishModel filePublishModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("�����۾� ������ �Է� Start");
				_log.Debug("-----------------------------------------");
				
				FilePublishServicePloxy.FilePublishService svc = new FilePublishServicePloxy.FilePublishService();
				svc.Url = _WebServiceUrl;
				svc.Timeout = FrameSystem.m_SystemTimeout;

				FilePublishServicePloxy.HeaderModel			remoteHeader = new AdManagerClient.FilePublishServicePloxy.HeaderModel();
				FilePublishServicePloxy.FilePublishModel	remoteData   = new AdManagerClient.FilePublishServicePloxy.FilePublishModel();

				// ������� ��Ʈ
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;
				remoteHeader.UserLevel     = Header.UserLevel;

				// ȣ������ ��Ʈ
				remoteData.MediaCode	= filePublishModel.MediaCode;	  				
				remoteData.AckNo		= filePublishModel.AckNo;
				remoteData.ReserveDt	= filePublishModel.ReserveDt;
				remoteData.PublishDesc	= filePublishModel.PublishDesc;
				remoteData.State		= filePublishModel.State;
												
				// ������ �޼ҵ� ȣ��
				remoteData = svc.setReserveWorkInsert(remoteHeader, remoteData);

				// ����ڵ�˻�
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				filePublishModel.ResultCnt		= remoteData.ResultCnt;
				filePublishModel.ResultCD		= remoteData.ResultCD;
				filePublishModel.ResultDesc		= remoteData.ResultDesc;

				_log.Debug("-----------------------------------------");
				_log.Debug("�����۾� ������ �Է� End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":NewReserveWorkDetail():" + fe.ErrCode + ":" + fe.ResultMsg);
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
		/// �������� ó��
		/// </summary>
		/// <param name="filePublishModel"></param>
		public void SetReserveFileDetail(FilePublishModel filePublishModel)
		{
			try
			{
				FilePublishServicePloxy.FilePublishService svc = new FilePublishServicePloxy.FilePublishService();
				svc.Url = _WebServiceUrl;
				svc.Timeout = FrameSystem.m_SystemTimeout;

				FilePublishServicePloxy.HeaderModel			remoteHeader = new AdManagerClient.FilePublishServicePloxy.HeaderModel();
				FilePublishServicePloxy.FilePublishModel	remoteData   = new AdManagerClient.FilePublishServicePloxy.FilePublishModel();

				// ������� ��Ʈ
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;
				remoteHeader.UserLevel     = Header.UserLevel;

				// ȣ������ ��Ʈ
				remoteData.MediaCode	= filePublishModel.MediaCode;	  				
				remoteData.AckNo		= filePublishModel.AckNo;
				remoteData.ReserveDt	= filePublishModel.ReserveDt;
				remoteData.ItemNo		= filePublishModel.ItemNo;
				remoteData.JobType		= filePublishModel.JobType;
												
				// ������ �޼ҵ� ȣ��
				remoteData = svc.setReserveFileUpdate(remoteHeader, remoteData);

				// ����ڵ�˻�
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				filePublishModel.ResultCnt		= remoteData.ResultCnt;
				filePublishModel.ResultCD		= remoteData.ResultCD;
				filePublishModel.ResultDesc		= remoteData.ResultDesc;

				_log.Debug("-----------------------------------------");
				_log.Debug("�����۾� ������ ��ȸ End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":GetReserveWorkList():" + fe.ErrCode + ":" + fe.ResultMsg);
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


		#endregion

	}
}
