// ===============================================================================
// AdFile Manager  for Charites Project
//
// AdFileWideManager.cs
//
// �������Ϲ������� ���񽺸� ȣ���մϴ�. 
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
	/// �������Ϲ������� �����񽺸� ȣ���մϴ�. 
	/// </summary>
	public class AdFileWideManager : BaseManager
	{
		/// <summary>
		/// ������
		/// </summary>
		/// <param name="systemModel"></param>
		/// <param name="commonModel"></param>

		public AdFileWideManager(SystemModel systemModel, CommonModel commonModel) : base(systemModel, commonModel)
		{
			_log = FrameSystem.oLog;
			_module = "AdFileWide";
			_Host  = FrameSystem.m_WebServer_Host;
			_Port  = FrameSystem.m_WebServer_Port;
			_Path  = FrameSystem.m_WebServer_App + "/Contract/AdFileWideService.asmx";
		}

		/// <summary>
		/// �������ϰǼ� ��ȸ
		/// </summary>
		/// <param name="userModel"></param>
		public void GetFileCount(AdFileWideModel adFileWideModel)
		{

			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("�������ϰǼ� ��ȸ Start");
				_log.Debug("-----------------------------------------");
				
				// ������ �ν��Ͻ� ����
				AdFileWideServicePloxy.AdFileWideService svc = new AdFileWideServicePloxy.AdFileWideService();
			
				// ������URL ���� ��Ʈ
				svc.Url = _WebServiceUrl;

				// ����Ʈ �� ����
				AdFileWideServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.AdFileWideServicePloxy.HeaderModel();
				AdFileWideServicePloxy.AdFileWideModel   remoteData   = new AdManagerClient.AdFileWideServicePloxy.AdFileWideModel();

				// ������� ��Ʈ
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;
				remoteHeader.UserLevel     = Header.UserLevel;

				// ȣ������ ��Ʈ
				remoteData.SearchKey             = adFileWideModel.SearchKey;
				remoteData.SearchMediaCode		 = adFileWideModel.SearchMediaCode;	  
				remoteData.SearchchkAdState_10	 = adFileWideModel.SearchchkAdState_10; 
				remoteData.SearchchkAdState_20	 = adFileWideModel.SearchchkAdState_20; 
				remoteData.SearchchkAdState_30	 = adFileWideModel.SearchchkAdState_30; 
				remoteData.SearchchkAdState_40	 = adFileWideModel.SearchchkAdState_40; 
                
				// ������ ȣ�� Ÿ�Ӿƿ�����
				svc.Timeout = FrameSystem.m_SystemTimeout;
				// ������ �޼ҵ� ȣ��
				remoteData = svc.GetFileCount(remoteHeader, remoteData);

				// ����ڵ�˻�
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// ��� ��Ʈ

				adFileWideModel.FileListCount = remoteData.FileListCount;  // 2007.10.01 RH.Jung ���ϸ���Ʈ�Ǽ��˻�

				adFileWideModel.CountDataSet = remoteData.CountDataSet.Copy();
				adFileWideModel.ResultCnt    = remoteData.ResultCnt;
				adFileWideModel.ResultCD     = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("�������ϰǼ� ��ȸ End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":GetAdFileWideList():" + fe.ErrCode + ":" + fe.ResultMsg);
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
		/// �������Ϲ�����ȸ
		/// </summary>
		/// <param name="userModel"></param>
		public void GetAdFileWideList(AdFileWideModel adFileWideModel)
		{

			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("�������Ϲ��������ȸ Start");
				_log.Debug("-----------------------------------------");
				
				// ������ �ν��Ͻ� ����
				AdFileWideServicePloxy.AdFileWideService svc = new AdFileWideServicePloxy.AdFileWideService();
			
				// ������URL ���� ��Ʈ
				svc.Url = _WebServiceUrl;

				// ����Ʈ �� ����
				AdFileWideServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.AdFileWideServicePloxy.HeaderModel();
				AdFileWideServicePloxy.AdFileWideModel   remoteData   = new AdManagerClient.AdFileWideServicePloxy.AdFileWideModel();

				// ������� ��Ʈ
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;
				remoteHeader.UserLevel     = Header.UserLevel;

				// ȣ������ ��Ʈ
				remoteData.SearchMediaCode		= adFileWideModel.SearchMediaCode;	  
				remoteData.SearchKey			= adFileWideModel.SearchKey;
				remoteData.SearchFileState		= adFileWideModel.SearchFileState;				
				remoteData.SearchchkAdState_10	= adFileWideModel.SearchchkAdState_10; 
				remoteData.SearchchkAdState_20	= adFileWideModel.SearchchkAdState_20; 
				remoteData.SearchchkAdState_30	= adFileWideModel.SearchchkAdState_30; 
				remoteData.SearchchkAdState_40	= adFileWideModel.SearchchkAdState_40; 
                
				// ������ ȣ�� Ÿ�Ӿƿ�����
				svc.Timeout = FrameSystem.m_SystemTimeout;
				// ������ �޼ҵ� ȣ��
				remoteData = svc.GetAdFileWideList(remoteHeader, remoteData);

				// ����ڵ�˻�
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// ��� ��Ʈ
				adFileWideModel.FileDataSet = remoteData.FileDataSet.Copy();
				adFileWideModel.ResultCnt   = remoteData.ResultCnt;
				adFileWideModel.ResultCD    = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("�������Ϲ��������ȸ End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":GetAdFileWideList():" + fe.ErrCode + ":" + fe.ResultMsg);
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
		/// �������� ����Ȳ ��ȸ
		/// </summary>
		/// <param name="userModel"></param>
		public void GetAdFileSchedule(AdFileWideModel adFileWideModel)
		{

			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug(" �������� ����Ȳ ��ȸ Start");
				_log.Debug("-----------------------------------------");
				
				// ������ �ν��Ͻ� ����
				AdFileWideServicePloxy.AdFileWideService svc = new AdFileWideServicePloxy.AdFileWideService();
			
				// ������URL ���� ��Ʈ
				svc.Url = _WebServiceUrl;

				// ����Ʈ �� ����
				AdFileWideServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.AdFileWideServicePloxy.HeaderModel();
				AdFileWideServicePloxy.AdFileWideModel   remoteData   = new AdManagerClient.AdFileWideServicePloxy.AdFileWideModel();

				// ������� ��Ʈ
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;
				remoteHeader.UserLevel     = Header.UserLevel;

				// ȣ������ ��Ʈ
				remoteData.SearchMediaCode		= adFileWideModel.SearchMediaCode;	  
				remoteData.ItemNo			= adFileWideModel.ItemNo;
                
				// ������ ȣ�� Ÿ�Ӿƿ�����
				svc.Timeout = FrameSystem.m_SystemTimeout;
				// ������ �޼ҵ� ȣ��
				remoteData = svc.GetAdFileSchedule(remoteHeader, remoteData);

				// ����ڵ�˻�
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// ��� ��Ʈ
				adFileWideModel.ScheduleDataSet = remoteData.ScheduleDataSet.Copy();
				adFileWideModel.ResultCnt		= remoteData.ResultCnt;
				adFileWideModel.ResultCD		= remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug(" �������� ����Ȳ ��ȸ End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":GetAdFileSchedule():" + fe.ErrCode + ":" + fe.ResultMsg);
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
		/// �������� �˼���û
		/// </summary>
		/// <param name="userModel"></param>
		public void SetAdFileChkRequest(AdFileWideModel adFileWideModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("�������� �˼���û Start");
				_log.Debug("-----------------------------------------");

				//�Էµ������� Validation �˻�
				if(adFileWideModel.ItemNo.Length < 1) 
				{
					throw new FrameException("�������� ���õ��� �ʾҽ��ϴ�.");
				}
				
				// ������ �ν��Ͻ� ����
				AdFileWideServicePloxy.AdFileWideService svc = new AdFileWideServicePloxy.AdFileWideService();

				// ������URL ���� ��Ʈ
				svc.Url = _WebServiceUrl;
			
				// ����Ʈ �� ����
				AdFileWideServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.AdFileWideServicePloxy.HeaderModel();
				AdFileWideServicePloxy.AdFileWideModel    remoteData   = new AdManagerClient.AdFileWideServicePloxy.AdFileWideModel();

				// ������� ��Ʈ
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;
				remoteHeader.UserLevel     = Header.UserLevel;

				// ȣ��������Ʈ
				remoteData.MediaCode       = adFileWideModel.MediaCode;
				remoteData.ItemNo	       = adFileWideModel.ItemNo;
				remoteData.ItemName	       = adFileWideModel.ItemName;
				remoteData.FileName	       = adFileWideModel.FileName;
             
				// ������ ȣ�� Ÿ�Ӿƿ�����
				svc.Timeout = FrameSystem.m_SystemTimeout;
				// ������ �޼ҵ� ȣ��
				remoteData = svc.SetAdFileChkRequest(remoteHeader, remoteData);

				// ����ڵ�˻�
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// ��� ��Ʈ
				adFileWideModel.ResultCnt   = remoteData.ResultCnt;
				adFileWideModel.ResultCD    = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("�������� �˼���û End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":SetAdFileChkRequest():" + fe.ErrCode + ":" + fe.ResultMsg);
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
		/// �������� �˼���û ���
		/// </summary>
		/// <param name="userModel"></param>
		public void SetAdFileChkRequestCancel(AdFileWideModel adFileWideModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("�������� �˼���û Start");
				_log.Debug("-----------------------------------------");

				//�Էµ������� Validation �˻�
				if(adFileWideModel.ItemNo.Length < 1) 
				{
					throw new FrameException("�������� ���õ��� �ʾҽ��ϴ�.");
				}
				
				// ������ �ν��Ͻ� ����
				AdFileWideServicePloxy.AdFileWideService svc = new AdFileWideServicePloxy.AdFileWideService();

				// ������URL ���� ��Ʈ
				svc.Url = _WebServiceUrl;
			
				// ����Ʈ �� ����
				AdFileWideServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.AdFileWideServicePloxy.HeaderModel();
				AdFileWideServicePloxy.AdFileWideModel    remoteData   = new AdManagerClient.AdFileWideServicePloxy.AdFileWideModel();

				// ������� ��Ʈ
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;
				remoteHeader.UserLevel     = Header.UserLevel;

				// ȣ��������Ʈ
				remoteData.MediaCode       = adFileWideModel.MediaCode;
				remoteData.ItemNo	       = adFileWideModel.ItemNo;
				remoteData.ItemName	       = adFileWideModel.ItemName;
				remoteData.FileName	       = adFileWideModel.FileName;
             
				// ������ ȣ�� Ÿ�Ӿƿ�����
				svc.Timeout = FrameSystem.m_SystemTimeout;
				// ������ �޼ҵ� ȣ��
				remoteData = svc.SetAdFileChkRequestCancel(remoteHeader, remoteData);

				// ����ڵ�˻�
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// ��� ��Ʈ
				adFileWideModel.ResultCnt   = remoteData.ResultCnt;
				adFileWideModel.ResultCD    = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("�������� �˼���û ��� End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":SetAdFileChkRequestCancel():" + fe.ErrCode + ":" + fe.ResultMsg);
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
		/// CMS���� ���� ����(������, �˼��Ϸῡ ���յ�)
		/// </summary>
		/// <param name="adFileWideModel"></param>
		public void SetCmsRequestBegin(AdFileWideModel adFileWideModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("�������� CMS���� Start");
				_log.Debug("-----------------------------------------");

				//�Էµ������� Validation �˻�
				if(adFileWideModel.ItemNo.Length < 1) 
				{
					throw new FrameException("�������� ���õ��� �ʾҽ��ϴ�.");
				}
				
				// ������ �ν��Ͻ� ����
				AdFileWideServicePloxy.AdFileWideService svc = new AdFileWideServicePloxy.AdFileWideService();

				// ������URL ���� ��Ʈ
				svc.Url = _WebServiceUrl;
			
				// ����Ʈ �� ����
				AdFileWideServicePloxy.HeaderModel		remoteHeader = new AdManagerClient.AdFileWideServicePloxy.HeaderModel();
				AdFileWideServicePloxy.AdFileWideModel  remoteData   = new AdManagerClient.AdFileWideServicePloxy.AdFileWideModel();

				// ������� ��Ʈ
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;
				remoteHeader.UserLevel     = Header.UserLevel;

				// ȣ��������Ʈ
				remoteData.MediaCode       = adFileWideModel.MediaCode;
				remoteData.ItemNo	       = adFileWideModel.ItemNo;
				remoteData.ItemName	       = adFileWideModel.ItemName;
				remoteData.FileName	       = adFileWideModel.FileName;

				remoteData.FilePath			= adFileWideModel.FilePath;
				remoteData.CmsCid			= adFileWideModel.CmsCid;
				remoteData.CmsCmd			= adFileWideModel.CmsCmd;
				remoteData.CmsRequestStatus	= adFileWideModel.CmsRequestStatus;
				remoteData.CmsProcessStatus	= adFileWideModel.CmsProcessStatus;
				remoteData.CmsSyncCount		= adFileWideModel.CmsSyncCount;
				remoteData.CmsDescCount		= adFileWideModel.CmsDescCount;
				
             
				// ������ ȣ�� Ÿ�Ӿƿ�����
				svc.Timeout = FrameSystem.m_SystemTimeout;
				// ������ �޼ҵ� ȣ��
				remoteData = svc.SetCmsRequestBegin(remoteHeader, remoteData);

				// ����ڵ�˻�
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// ��� ��Ʈ
				adFileWideModel.ResultCnt   = remoteData.ResultCnt;
				adFileWideModel.ResultCD    = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("�������� CMS���� End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":SetAdFileChkComplete():" + fe.ErrCode + ":" + fe.ResultMsg);
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
		/// �������� �˼��Ϸ�
		/// </summary>
		/// <param name="userModel"></param>
		public void SetAdFileChkComplete(AdFileWideModel adFileWideModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("�������� �˼��Ϸ� Start");
				_log.Debug("-----------------------------------------");

				//�Էµ������� Validation �˻�
				if(adFileWideModel.ItemNo.Length < 1) 
				{
					throw new FrameException("�������� ���õ��� �ʾҽ��ϴ�.");
				}
				
				// ������ �ν��Ͻ� ����
				AdFileWideServicePloxy.AdFileWideService svc = new AdFileWideServicePloxy.AdFileWideService();

				// ������URL ���� ��Ʈ
				svc.Url = _WebServiceUrl;
			
				// ����Ʈ �� ����
				AdFileWideServicePloxy.HeaderModel		remoteHeader = new AdManagerClient.AdFileWideServicePloxy.HeaderModel();
				AdFileWideServicePloxy.AdFileWideModel  remoteData   = new AdManagerClient.AdFileWideServicePloxy.AdFileWideModel();

				// ������� ��Ʈ
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;
				remoteHeader.UserLevel     = Header.UserLevel;

				// ȣ��������Ʈ
				remoteData.MediaCode       = adFileWideModel.MediaCode;
				remoteData.ItemNo	       = adFileWideModel.ItemNo;
				remoteData.ItemName	       = adFileWideModel.ItemName;
				remoteData.FileName	       = adFileWideModel.FileName;

				// CMSȣ������ ��Ʈ
				remoteData.FilePath			= adFileWideModel.FilePath;
				remoteData.CmsCid			= adFileWideModel.CmsCid;
				remoteData.CmsCmd			= adFileWideModel.CmsCmd;
				remoteData.CmsRequestStatus	= adFileWideModel.CmsRequestStatus;
				remoteData.CmsProcessStatus	= adFileWideModel.CmsProcessStatus;
				remoteData.CmsSyncCount		= adFileWideModel.CmsSyncCount;
				remoteData.CmsDescCount		= adFileWideModel.CmsDescCount;
             
				// ������ ȣ�� Ÿ�Ӿƿ�����
				svc.Timeout = FrameSystem.m_SystemTimeout;
				// ������ �޼ҵ� ȣ��
				remoteData = svc.SetAdFileChkComplete(remoteHeader, remoteData);

				// ����ڵ�˻�
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// ��� ��Ʈ
				adFileWideModel.ResultCnt   = remoteData.ResultCnt;
				adFileWideModel.ResultCD    = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("�������� �˼��Ϸ� End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":SetAdFileChkComplete():" + fe.ErrCode + ":" + fe.ResultMsg);
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
		/// �������� �˼��Ϸ� ���
		/// </summary>
		/// <param name="userModel"></param>
		public void SetAdFileChkCompleteCancel(AdFileWideModel adFileWideModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("�������� �˼��Ϸ� ��� Start");
				_log.Debug("-----------------------------------------");

				//�Էµ������� Validation �˻�
				if(adFileWideModel.ItemNo.Length < 1) 
				{
					throw new FrameException("�������� ���õ��� �ʾҽ��ϴ�.");
				}
				
				// ������ �ν��Ͻ� ����
				AdFileWideServicePloxy.AdFileWideService svc = new AdFileWideServicePloxy.AdFileWideService();

				// ������URL ���� ��Ʈ
				svc.Url = _WebServiceUrl;
			
				// ����Ʈ �� ����
				AdFileWideServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.AdFileWideServicePloxy.HeaderModel();
				AdFileWideServicePloxy.AdFileWideModel    remoteData   = new AdManagerClient.AdFileWideServicePloxy.AdFileWideModel();

				// ������� ��Ʈ
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;
				remoteHeader.UserLevel     = Header.UserLevel;

				// ȣ��������Ʈ
				remoteData.MediaCode       = adFileWideModel.MediaCode;
				remoteData.ItemNo	       = adFileWideModel.ItemNo;
				remoteData.ItemName	       = adFileWideModel.ItemName;
				remoteData.FileName	       = adFileWideModel.FileName;
             
				// ������ ȣ�� Ÿ�Ӿƿ�����
				svc.Timeout = FrameSystem.m_SystemTimeout;
				// ������ �޼ҵ� ȣ��
				remoteData = svc.SetAdFileChkCompleteCancel(remoteHeader, remoteData);

				// ����ڵ�˻�
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// ��� ��Ʈ
				adFileWideModel.ResultCnt   = remoteData.ResultCnt;
				adFileWideModel.ResultCD    = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("�������� �˼��Ϸ� ��� End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":SetAdFileChkCompleteCancel():" + fe.ErrCode + ":" + fe.ResultMsg);
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
		/// �������� CDN����Ȯ��
		/// </summary>
		/// <param name="userModel"></param>
		public void SetAdFileCDNSync(AdFileWideModel adFileWideModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("�������� CDN����Ȯ�� Start");
				_log.Debug("-----------------------------------------");

				//�Էµ������� Validation �˻�
				if(adFileWideModel.ItemNo.Length < 1) 
				{
					throw new FrameException("�������� ���õ��� �ʾҽ��ϴ�.");
				}
				
				// ������ �ν��Ͻ� ����
				AdFileWideServicePloxy.AdFileWideService svc = new AdFileWideServicePloxy.AdFileWideService();

				// ������URL ���� ��Ʈ
				svc.Url = _WebServiceUrl;
			
				// ����Ʈ �� ����
				AdFileWideServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.AdFileWideServicePloxy.HeaderModel();
				AdFileWideServicePloxy.AdFileWideModel    remoteData   = new AdManagerClient.AdFileWideServicePloxy.AdFileWideModel();

				// ������� ��Ʈ
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;
				remoteHeader.UserLevel     = Header.UserLevel;

				// ȣ��������Ʈ
				remoteData.MediaCode       = adFileWideModel.MediaCode;
				remoteData.ItemNo	       = adFileWideModel.ItemNo;
				remoteData.ItemName	       = adFileWideModel.ItemName;
				remoteData.FileName	       = adFileWideModel.FileName;
             
				// ������ ȣ�� Ÿ�Ӿƿ�����
				svc.Timeout = FrameSystem.m_SystemTimeout;
				// ������ �޼ҵ� ȣ��
				remoteData = svc.SetAdFileCDNSync(remoteHeader, remoteData);

				// ����ڵ�˻�
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// ��� ��Ʈ
				adFileWideModel.ResultCnt   = remoteData.ResultCnt;
				adFileWideModel.ResultCD    = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("�������� CDN����Ȯ�� End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":SetAdFileCDNSync():" + fe.ErrCode + ":" + fe.ResultMsg);
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
		/// �������� CDN����Ȯ�� ���
		/// </summary>
		/// <param name="userModel"></param>
		public void SetAdFileCDNSyncCancel(AdFileWideModel adFileWideModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("�������� CDN����Ȯ�� ��� Start");
				_log.Debug("-----------------------------------------");

				//�Էµ������� Validation �˻�
				if(adFileWideModel.ItemNo.Length < 1) 
				{
					throw new FrameException("�������� ���õ��� �ʾҽ��ϴ�.");
				}
				
				// ������ �ν��Ͻ� ����
				AdFileWideServicePloxy.AdFileWideService svc = new AdFileWideServicePloxy.AdFileWideService();

				// ������URL ���� ��Ʈ
				svc.Url = _WebServiceUrl;
			
				// ����Ʈ �� ����
				AdFileWideServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.AdFileWideServicePloxy.HeaderModel();
				AdFileWideServicePloxy.AdFileWideModel    remoteData   = new AdManagerClient.AdFileWideServicePloxy.AdFileWideModel();

				// ������� ��Ʈ
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;
				remoteHeader.UserLevel     = Header.UserLevel;

				// ȣ��������Ʈ
				remoteData.MediaCode       = adFileWideModel.MediaCode;
				remoteData.ItemNo	       = adFileWideModel.ItemNo;
				remoteData.ItemName	       = adFileWideModel.ItemName;
				remoteData.FileName	       = adFileWideModel.FileName;
             
				// ������ ȣ�� Ÿ�Ӿƿ�����
				svc.Timeout = FrameSystem.m_SystemTimeout;
				// ������ �޼ҵ� ȣ��
				remoteData = svc.SetAdFileCDNSyncCancel(remoteHeader, remoteData);

				// ����ڵ�˻�
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// ��� ��Ʈ
				adFileWideModel.ResultCnt   = remoteData.ResultCnt;
				adFileWideModel.ResultCD    = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("�������� CDN����Ȯ�� ��� End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":SetAdFileCDNSyncCancel():" + fe.ErrCode + ":" + fe.ResultMsg);
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
		/// �������� CDN����Ȯ��
		/// </summary>
		/// <param name="userModel"></param>
		public void SetAdFileCDNPublish(AdFileWideModel adFileWideModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("�������� CDN����Ȯ�� Start");
				_log.Debug("-----------------------------------------");

				//�Էµ������� Validation �˻�
				if(adFileWideModel.ItemNo.Length < 1) 
				{
					throw new FrameException("�������� ���õ��� �ʾҽ��ϴ�.");
				}
				
				// ������ �ν��Ͻ� ����
				AdFileWideServicePloxy.AdFileWideService svc = new AdFileWideServicePloxy.AdFileWideService();

				// ������URL ���� ��Ʈ
				svc.Url = _WebServiceUrl;
			
				// ����Ʈ �� ����
				AdFileWideServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.AdFileWideServicePloxy.HeaderModel();
				AdFileWideServicePloxy.AdFileWideModel    remoteData   = new AdManagerClient.AdFileWideServicePloxy.AdFileWideModel();

				// ������� ��Ʈ
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;
				remoteHeader.UserLevel     = Header.UserLevel;

				// ȣ��������Ʈ
				remoteData.MediaCode       = adFileWideModel.MediaCode;
				remoteData.ItemNo	       = adFileWideModel.ItemNo;
				remoteData.ItemName	       = adFileWideModel.ItemName;
				remoteData.FileName	       = adFileWideModel.FileName;
             
				// ������ ȣ�� Ÿ�Ӿƿ�����
				svc.Timeout = FrameSystem.m_SystemTimeout;
				// ������ �޼ҵ� ȣ��
				remoteData = svc.SetAdFileCDNPublish(remoteHeader, remoteData);

				// ����ڵ�˻�
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// ��� ��Ʈ
				adFileWideModel.ResultCnt   = remoteData.ResultCnt;
				adFileWideModel.ResultCD    = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("�������� CDN����Ȯ�� End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":SetAdFileCDNPublish():" + fe.ErrCode + ":" + fe.ResultMsg);
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
		/// �������� CDN����Ȯ�� ���
		/// </summary>
		/// <param name="userModel"></param>
		public void SetAdFileCDNPublishCancel(AdFileWideModel adFileWideModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("�������� CDN����Ȯ�� ��� Start");
				_log.Debug("-----------------------------------------");

				//�Էµ������� Validation �˻�
				if(adFileWideModel.ItemNo.Length < 1) 
				{
					throw new FrameException("�������� ���õ��� �ʾҽ��ϴ�.");
				}
				
				// ������ �ν��Ͻ� ����
				AdFileWideServicePloxy.AdFileWideService svc = new AdFileWideServicePloxy.AdFileWideService();

				// ������URL ���� ��Ʈ
				svc.Url = _WebServiceUrl;
			
				// ����Ʈ �� ����
				AdFileWideServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.AdFileWideServicePloxy.HeaderModel();
				AdFileWideServicePloxy.AdFileWideModel    remoteData   = new AdManagerClient.AdFileWideServicePloxy.AdFileWideModel();

				// ������� ��Ʈ
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;
				remoteHeader.UserLevel     = Header.UserLevel;

				// ȣ��������Ʈ
				remoteData.MediaCode       = adFileWideModel.MediaCode;
				remoteData.ItemNo	       = adFileWideModel.ItemNo;
				remoteData.ItemName	       = adFileWideModel.ItemName;
				remoteData.FileName	       = adFileWideModel.FileName;
             
				// ������ ȣ�� Ÿ�Ӿƿ�����
				svc.Timeout = FrameSystem.m_SystemTimeout;
				// ������ �޼ҵ� ȣ��
				remoteData = svc.SetAdFileCDNPublishCancel(remoteHeader, remoteData);

				// ����ڵ�˻�
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// ��� ��Ʈ
				adFileWideModel.ResultCnt   = remoteData.ResultCnt;
				adFileWideModel.ResultCD    = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("�������� CDN����Ȯ�� ��� End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":SetAdFileCDNPublishCancel():" + fe.ErrCode + ":" + fe.ResultMsg);
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
		/// �������� ��ž����
		/// </summary>
		/// <param name="userModel"></param>
		public void SetAdFileSTBDelete(AdFileWideModel adFileWideModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("�������� ��ž���� Start");
				_log.Debug("-----------------------------------------");

				//�Էµ������� Validation �˻�
				if(adFileWideModel.ItemNo.Length < 1) 
				{
					throw new FrameException("�������� ���õ��� �ʾҽ��ϴ�.");
				}				

				// ������ �ν��Ͻ� ����
				AdFileWideServicePloxy.AdFileWideService svc = new AdFileWideServicePloxy.AdFileWideService();

				// ������URL ���� ��Ʈ
				svc.Url = _WebServiceUrl;
			
				// ����Ʈ �� ����
				AdFileWideServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.AdFileWideServicePloxy.HeaderModel();
				AdFileWideServicePloxy.AdFileWideModel    remoteData   = new AdManagerClient.AdFileWideServicePloxy.AdFileWideModel();

				// ������� ��Ʈ
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;
				remoteHeader.UserLevel     = Header.UserLevel;

				// ȣ��������Ʈ
				remoteData.MediaCode       = adFileWideModel.MediaCode;
				remoteData.ItemNo	       = adFileWideModel.ItemNo;
				remoteData.ItemName	       = adFileWideModel.ItemName;
				remoteData.FileName	       = adFileWideModel.FileName;
                
				// ������ ȣ�� Ÿ�Ӿƿ�����
				svc.Timeout = FrameSystem.m_SystemTimeout;

				// ������ �޼ҵ� ȣ��
				remoteData = svc.SetAdFileSTBDelete(remoteHeader, remoteData);

				// ����ڵ�˻�
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// ��� ��Ʈ
				adFileWideModel.ResultCnt   = remoteData.ResultCnt;
				adFileWideModel.ResultCD    = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("�������� ��ž���� End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":SetAdFileSTBDelete():" + fe.ErrCode + ":" + fe.ResultMsg);
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
		/// �������� ��ž���� ���
		/// </summary>
		/// <param name="userModel"></param>
		public void SetAdFileSTBDeleteCancel(AdFileWideModel adFileWideModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("�������� ��ž���� Start");
				_log.Debug("-----------------------------------------");

				//�Էµ������� Validation �˻�
				if(adFileWideModel.ItemNo.Length < 1) 
				{
					throw new FrameException("�������� ���õ��� �ʾҽ��ϴ�.");
				}				

				// ������ �ν��Ͻ� ����
				AdFileWideServicePloxy.AdFileWideService svc = new AdFileWideServicePloxy.AdFileWideService();

				// ������URL ���� ��Ʈ
				svc.Url = _WebServiceUrl;
			
				// ����Ʈ �� ����
				AdFileWideServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.AdFileWideServicePloxy.HeaderModel();
				AdFileWideServicePloxy.AdFileWideModel    remoteData   = new AdManagerClient.AdFileWideServicePloxy.AdFileWideModel();

				// ������� ��Ʈ
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;
				remoteHeader.UserLevel     = Header.UserLevel;

				// ȣ��������Ʈ
				remoteData.MediaCode       = adFileWideModel.MediaCode;
				remoteData.ItemNo	       = adFileWideModel.ItemNo;
				remoteData.ItemName	       = adFileWideModel.ItemName;
				remoteData.FileName	       = adFileWideModel.FileName;
                
				// ������ ȣ�� Ÿ�Ӿƿ�����
				svc.Timeout = FrameSystem.m_SystemTimeout;

				// ������ �޼ҵ� ȣ��
				remoteData = svc.SetAdFileSTBDeleteCancel(remoteHeader, remoteData);

				// ����ڵ�˻�
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// ��� ��Ʈ
				adFileWideModel.ResultCnt   = remoteData.ResultCnt;
				adFileWideModel.ResultCD    = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("�������� ��ž���� ��� End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":SetAdFileSTBDeleteCancel():" + fe.ErrCode + ":" + fe.ResultMsg);
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
		/// �������� ���米ü
		/// </summary>
		/// <param name="userModel"></param>
		public void SetAdFileChange(AdFileWideModel adFileWideModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("�������� ���米ü Start");
				_log.Debug("-----------------------------------------");

				//�Էµ������� Validation �˻�
				if(adFileWideModel.ItemNo.Length < 1) 
				{
					throw new FrameException("�������� ���õ��� �ʾҽ��ϴ�.");
				}				

				// ������ �ν��Ͻ� ����
				AdFileWideServicePloxy.AdFileWideService svc = new AdFileWideServicePloxy.AdFileWideService();

				// ������URL ���� ��Ʈ
				svc.Url = _WebServiceUrl;
			
				// ����Ʈ �� ����
				AdFileWideServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.AdFileWideServicePloxy.HeaderModel();
				AdFileWideServicePloxy.AdFileWideModel    remoteData   = new AdManagerClient.AdFileWideServicePloxy.AdFileWideModel();

				// ������� ��Ʈ
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;
				remoteHeader.UserLevel     = Header.UserLevel;

				// ȣ��������Ʈ
				remoteData.MediaCode       = adFileWideModel.MediaCode;
				remoteData.ItemNo	       = adFileWideModel.ItemNo;
				remoteData.ItemName	       = adFileWideModel.ItemName;
				remoteData.FileName	       = adFileWideModel.FileName;
				remoteData.FileState       = adFileWideModel.FileState;
                
				// ������ ȣ�� Ÿ�Ӿƿ�����
				svc.Timeout = FrameSystem.m_SystemTimeout * 5;

				// ������ �޼ҵ� ȣ��
				remoteData = svc.SetAdFileChange(remoteHeader, remoteData);

				// ����ڵ�˻�
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// ��� ��Ʈ
				adFileWideModel.ResultCnt   = remoteData.ResultCnt;
				adFileWideModel.ResultCD    = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("�������� ���米ü End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":SetAdFileChange():" + fe.ErrCode + ":" + fe.ResultMsg);
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
