// ===============================================================================
//
// SystemConfigManager.cs
//
// �޴����� ���񽺸� ȣ���մϴ�. 
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
	/// ȯ�漳�� �����񽺸� ȣ���մϴ�. 
	/// </summary>
	public class SystemConfigManager : BaseManager
	{
		/// <summary>
		/// ������
		/// </summary>
		/// <param name="systemModel"></param>
		/// <param name="commonModel"></param>

		public SystemConfigManager(SystemModel systemModel, CommonModel commonModel) : base(systemModel, commonModel)
		{
			_log = FrameSystem.oLog;
			_Host  = FrameSystem.m_WebServer_Host;
			_Port  = FrameSystem.m_WebServer_Port;
			_Path  = FrameSystem.m_WebServer_App + "/Common/SystemConfigService.asmx";
		}

		/// <summary>
		/// ȯ�漳�� ��� ��ȸ
		/// </summary>
		/// <param name="baseModel"></param>
		public void GetSystemConfigList(SystemConfigModel systemConfigModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("ȯ�漳����ȸ Start");
				_log.Debug("-----------------------------------------");

				// ������ �ν��Ͻ� ����
				SystemConfigServicePloxy.SystemConfigService svc = new SystemConfigServicePloxy.SystemConfigService();
				svc.Url = _WebServiceUrl;
			
				// ����Ʈ �� ����
				SystemConfigServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.SystemConfigServicePloxy.HeaderModel();
				SystemConfigServicePloxy.SystemConfigModel     remoteData   = new AdManagerClient.SystemConfigServicePloxy.SystemConfigModel();

				// ������� ��Ʈ
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;
				remoteHeader.UserLevel     = Header.UserLevel;
				remoteHeader.UserClass	   = Header.UserClass;
			
				// ������ ȣ�� Ÿ�Ӿƿ�����
				svc.Timeout = FrameSystem.m_SystemTimeout;
				// ������ �޼ҵ� ȣ��
				remoteData = svc.GetSystemConfigList(remoteHeader, remoteData);

				// ����ڵ�˻�
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// ��� ��Ʈ
				systemConfigModel.SystemConfigDataSet = remoteData.SystemConfigDataSet.Copy();
				systemConfigModel.ResultCnt   = remoteData.ResultCnt;
				systemConfigModel.ResultCD    = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("ȯ�漳����ȸ End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":GetSystemConfigList():" + fe.ErrCode + ":" + fe.ResultMsg);
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
		/// ȯ�漳�� ������ ����
		/// </summary>
		/// <param name="systemConfigModel"></param>
		public void SetSystemConfigUpdate(SystemConfigModel systemConfigModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("ȯ�漳�� ���� Start");
				_log.Debug("-----------------------------------------");

            				// ������ �ν��Ͻ� ����
				SystemConfigServicePloxy.SystemConfigService svc = new SystemConfigServicePloxy.SystemConfigService();
				svc.Url = _WebServiceUrl;			
				// ����Ʈ �� ����
				SystemConfigServicePloxy.HeaderModel		remoteHeader = new AdManagerClient.SystemConfigServicePloxy.HeaderModel();
				SystemConfigServicePloxy.SystemConfigModel	remoteData   = new AdManagerClient.SystemConfigServicePloxy.SystemConfigModel();
            
				remoteHeader.ClientKey		= Header.ClientKey;
				remoteHeader.UserID			= Header.UserID;
				remoteData.FtpUploadID      = systemConfigModel.FtpUploadID;
				remoteData.FtpUploadPW      = systemConfigModel.FtpUploadPW;
				remoteData.FtpUploadHost    = systemConfigModel.FtpUploadHost;
				remoteData.FtpUploadPort    = systemConfigModel.FtpUploadPort;
				remoteData.FtpUploadPath    = systemConfigModel.FtpUploadPath;
				remoteData.FtpMovePath	    = systemConfigModel.FtpMovePath;
				remoteData.FtpMoveUseYn     = systemConfigModel.FtpMoveUseYn;
				remoteData.FtpCdnID         = systemConfigModel.FtpCdnID;
				remoteData.FtpCdnPW         = systemConfigModel.FtpCdnPW;
				remoteData.FtpCdnHost       = systemConfigModel.FtpCdnHost;
				remoteData.FtpCdnPort       = systemConfigModel.FtpCdnPort;
				remoteData.FileQueueUseYn   = systemConfigModel.FileQueueUseYn;
				remoteData.FileQueueInterval= systemConfigModel.FileQueueInterval;
				remoteData.FileQueueCnt     = systemConfigModel.FileQueueCnt;
				remoteData.URLGetAdPopList  = systemConfigModel.URLGetAdPopList;
				remoteData.URLSetAdPop		= systemConfigModel.URLSetAdPop;
				remoteData.CmsMasUrl		= systemConfigModel.CmsMasUrl;
				remoteData.CmsMasQuery		= systemConfigModel.CmsMasQuery;

				// ������ ȣ�� Ÿ�Ӿƿ�����
				svc.Timeout = FrameSystem.m_SystemTimeout;
				// ������ �޼ҵ� ȣ��
				remoteData = svc.SetSystemConfigUpdate(remoteHeader, remoteData);
            
				// ����ڵ�˻�
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}
            
				// ��� ��Ʈ
				systemConfigModel.ResultCnt   = remoteData.ResultCnt;
				_log.Debug("systemConfigModel.ResultCnt = "+systemConfigModel.ResultCnt);
            			
				systemConfigModel.ResultCD    = remoteData.ResultCD;
            
				_log.Debug("-----------------------------------------");
				_log.Debug("ȯ�漳�� End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				systemConfigModel.ResultCD    = "3101";
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":SetSystemConfigUpdate():" + fe.ErrCode + ":" + fe.ResultMsg);
				_log.Warning("-----------------------------------------");
				throw fe;
			}
			catch(Exception e)
			{
				systemConfigModel.ResultCD    = "3101";
				_log.Error("-----------------------------------------");
				_log.Exception(e);
				_log.Error("-----------------------------------------");
				throw new FrameException(e.Message);
			}
		}
	}
}