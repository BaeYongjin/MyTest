// ===============================================================================
//
// DailyAdExecSummaryRptManager.cs
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
	/// DailyAdExecSummaryRptManager�� ���� ��� �����Դϴ�.
	/// </summary>
	public class DailyAdExecSummaryRptManager : BaseManager
	{
		public DailyAdExecSummaryRptManager(SystemModel systemModel, CommonModel commonModel) : base(systemModel, commonModel)
		{
			_log = FrameSystem.oLog;
			_module = "DailyAdExecSummaryRpt";
			_Host  = FrameSystem.m_WebServer_Host;
			_Port  = FrameSystem.m_WebServer_Port;
			_Path  = FrameSystem.m_WebServer_App + "/ReportSummaryAd/DailyAdExecSummaryRptService.asmx";
		}

		/// <summary>
		/// �����ϰ�����Ʈ ��ȸ
		/// </summary>
		/// <param name="DailyAdExecSummaryRpt"></param>
		public void	GetDailyAdExecSummary(DailyAdExecSummaryRptModel dailyAdExecSummaryRptModel)
		{

			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("�ϰ������������� ��ȸ Start");
				_log.Debug("-----------------------------------------");
				
				// ������ �ν��Ͻ� ����
				DailyAdExecSummaryRptServicePloxy.DailyAdExecSummaryRptService svc = new AdManagerClient.DailyAdExecSummaryRptServicePloxy.DailyAdExecSummaryRptService();
			
				// ������URL ���� ��Ʈ
				svc.Url = _WebServiceUrl;

				// ����Ʈ �� ����
				DailyAdExecSummaryRptServicePloxy.HeaderModel remoteHeader = new AdManagerClient.DailyAdExecSummaryRptServicePloxy.HeaderModel();
				DailyAdExecSummaryRptServicePloxy.DailyAdExecSummaryRptModel remoteData = new AdManagerClient.DailyAdExecSummaryRptServicePloxy.DailyAdExecSummaryRptModel();

				// ������� ��Ʈ
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;
				remoteHeader.UserLevel     = Header.UserLevel;

				// ȣ������ ��Ʈ
				remoteData.LogDay1 = dailyAdExecSummaryRptModel.LogDay1;
				
				// ������ ȣ�� Ÿ�Ӿƿ�����
				svc.Timeout = FrameSystem.m_SystemTimeout;  

				// ������ �޼ҵ� ȣ��
				remoteData = svc.GetDailyAdExecSummary(remoteHeader, remoteData);

				// ����ڵ�˻�
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// ��� ��Ʈ
				dailyAdExecSummaryRptModel.ReportDataSet = remoteData.ReportDataSet.Copy();
				dailyAdExecSummaryRptModel.ItemDataSet = remoteData.ItemDataSet.Copy();
				dailyAdExecSummaryRptModel.ResultCnt = remoteData.ResultCnt;
				dailyAdExecSummaryRptModel.ResultCD = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("�ϰ������������� ��ȸ End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":GetDailyAdExecSummaryRptList():" + fe.ErrCode + ":" + fe.ResultMsg);
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
