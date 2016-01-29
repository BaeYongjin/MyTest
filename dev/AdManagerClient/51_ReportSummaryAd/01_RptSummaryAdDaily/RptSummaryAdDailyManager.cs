// ===============================================================================
//
// RptSummaryAdDailyManager.cs
//
// �����ϰ�����Ʈ ��ȸ ���񽺸� ȣ���մϴ�. 
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
	/// RptSummaryAdDailyManager�� ���� ��� �����Դϴ�.
	/// </summary>
	public class RptSummaryAdDailyManager : BaseManager
	{
		public RptSummaryAdDailyManager(SystemModel systemModel, CommonModel commonModel) : base(systemModel, commonModel)
		{
			_log = FrameSystem.oLog;
			_module = "RptSummaryAdDaily";
			_Host  = FrameSystem.m_WebServer_Host;
			_Port  = FrameSystem.m_WebServer_Port;
			_Path  = FrameSystem.m_WebServer_App + "/ReportSummaryAd/RptSummaryAdDailyService.asmx";
		}

		/// <summary>
		/// �����ϰ�����Ʈ ��ȸ
		/// </summary>
		/// <param name="RptSummaryAdDaily"></param>
		public void	GetRptSummaryAdDailyList(RptSummaryAdDailyModel rptSummaryAdDailyModel)
		{

			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("�����ϰ�����Ʈ��� ��ȸ Start");
				_log.Debug("-----------------------------------------");
				
				// ������ �ν��Ͻ� ����
				RptSummaryAdDailyServicePloxy.RptSummaryAdDailyService svc = new AdManagerClient.RptSummaryAdDailyServicePloxy.RptSummaryAdDailyService();
			
				// ������URL ���� ��Ʈ
				svc.Url = _WebServiceUrl;

				// ����Ʈ �� ����
				RptSummaryAdDailyServicePloxy.HeaderModel remoteHeader = new AdManagerClient.RptSummaryAdDailyServicePloxy.HeaderModel();
				RptSummaryAdDailyServicePloxy.RptSummaryAdDailyModel remoteData = new AdManagerClient.RptSummaryAdDailyServicePloxy.RptSummaryAdDailyModel();

				// ������� ��Ʈ
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;
				remoteHeader.UserLevel     = Header.UserLevel;

				// ȣ������ ��Ʈ
				remoteData.SearchDay = rptSummaryAdDailyModel.SearchDay;
				
				// ������ ȣ�� Ÿ�Ӿƿ�����
				svc.Timeout = FrameSystem.m_SystemTimeout;  

				// ������ �޼ҵ� ȣ��
				remoteData = svc.GetRptSummaryAdDailyList(remoteHeader, remoteData);

				// ����ڵ�˻�
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// ��� ��Ʈ
				rptSummaryAdDailyModel.RptDailyDataSet = remoteData.RptDailyDataSet.Copy();
				rptSummaryAdDailyModel.ResultCnt = remoteData.ResultCnt;
				rptSummaryAdDailyModel.ResultCD = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("�����ϰ�����Ʈ��� ��ȸ End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":GetRptSummaryAdDailyList():" + fe.ErrCode + ":" + fe.ResultMsg);
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
