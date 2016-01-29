// ===============================================================================
//
// AdHitStatusManager.cs
//
// ���� ��û��Ȳ ��ȸ ���񽺸� ȣ���մϴ�. 
//
// ===============================================================================
// Release history
//
// ===============================================================================
// Copyright (C) 2007 G-Inno Systems Inc.
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
	/// ���� ��û��Ȳ ��ȸ �����񽺸� ȣ���մϴ�. 
	/// </summary>
	public class AdHitStatusManager : BaseManager
	{
		/// <summary>
		/// ������
		/// </summary>
		/// <param name="systemModel"></param>
		/// <param name="commonModel"></param>

		public AdHitStatusManager(SystemModel systemModel, CommonModel commonModel) : base(systemModel, commonModel)
		{
			_log = FrameSystem.oLog;
			_module = "AdHitStatus";
			_Host  = FrameSystem.m_WebServer_Host;
			_Port  = FrameSystem.m_WebServer_Port;
			_Path  = FrameSystem.m_WebServer_App + "/ReportAd/AdHitStatusService.asmx";
		}


		/// <summary>
		/// ���� ��û��Ȳ  ��ȸ
        /// 2012/02/09 ����Ÿ������ �߰���.
		/// </summary>
		/// <param name="userModel"></param>
		public void GetAdHitStatus(AdHitStatusModel adHitStatusModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("���� ��û��Ȳ ��ȸ Start");
				_log.Debug("-----------------------------------------");
				
				// ������ �ν��Ͻ� ����
				AdHitStatusServicePloxy.AdHitStatusService svc = new AdHitStatusServicePloxy.AdHitStatusService();
			
				// ������URL ���� ��Ʈ
				svc.Url = _WebServiceUrl;

				// ����Ʈ �� ����
				AdHitStatusServicePloxy.HeaderModel         remoteHeader = new AdManagerClient.AdHitStatusServicePloxy.HeaderModel();
				AdHitStatusServicePloxy.AdHitStatusModel    remoteData   = new AdManagerClient.AdHitStatusServicePloxy.AdHitStatusModel();

				// ������� ��Ʈ
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;
				remoteHeader.UserLevel     = Header.UserLevel;

				// ȣ������ ��Ʈ
				remoteData.SearchMediaCode  = adHitStatusModel.SearchMediaCode;	  
				remoteData.SearchRapCode    = adHitStatusModel.SearchRapCode;	  
				remoteData.SearchAgencyCode = adHitStatusModel.SearchAgencyCode;	  
				remoteData.SearchDay        = adHitStatusModel.SearchDay;       
				remoteData.SearchKey        = adHitStatusModel.SearchKey;
                remoteData.SearchAdType     = adHitStatusModel.SearchAdType;
      				
                // ������ ȣ�� Ÿ�Ӿƿ�����
                svc.Timeout = FrameSystem.m_SystemTimeout * 20;   // �۾��ð��� ���...
				remoteData = svc.GetAdHitStatus(remoteHeader, remoteData);

				// ����ڵ�˻�
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// ��� ��Ʈ
				adHitStatusModel.ReportDataSet = remoteData.ReportDataSet.Copy();
				adHitStatusModel.ResultCnt     = remoteData.ResultCnt;
				adHitStatusModel.ResultCD      = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("���� ��û��Ȳ ��ȸ End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":GetAdHitStatus():" + fe.ErrCode + ":" + fe.ResultMsg);
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
