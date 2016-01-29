// ===============================================================================
//
// RptAdCategorySummaryManager.cs
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
	/// RptAdCategorySummaryManager�� ���� ��� �����Դϴ�.
	/// </summary>
	public class RptAdCategorySummaryManager : BaseManager
	{
		public RptAdCategorySummaryManager(SystemModel systemModel, CommonModel commonModel) : base(systemModel, commonModel)
		{
			_log = FrameSystem.oLog;
			_module = "DailyAdExecSummaryRpt";
			_Host  = FrameSystem.m_WebServer_Host;
			_Port  = FrameSystem.m_WebServer_Port;
			_Path  = FrameSystem.m_WebServer_App + "/ReportSummaryAd/RptAdCategorySummaryService.asmx";
		}

		/// <summary>
		/// �����ϰ�����Ʈ ��ȸ
		/// </summary>
		/// <param name="DailyAdExecSummaryRpt"></param>
		public void	GetRptAdCategorySummary(RptAdCategorySummaryModel rptAdCategorySummaryModel)
		{

			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("�ϰ������������� ��ȸ Start");
				_log.Debug("-----------------------------------------");
				
				// ������ �ν��Ͻ� ����
				RptAdCategorySummaryServicePloxy.RptAdCategorySummaryService svc = new AdManagerClient.RptAdCategorySummaryServicePloxy.RptAdCategorySummaryService();
			
				// ������URL ���� ��Ʈ
				svc.Url = _WebServiceUrl;

				// ����Ʈ �� ����
				RptAdCategorySummaryServicePloxy.HeaderModel remoteHeader = new AdManagerClient.RptAdCategorySummaryServicePloxy.HeaderModel();
				RptAdCategorySummaryServicePloxy.RptAdCategorySummaryModel remoteData = new AdManagerClient.RptAdCategorySummaryServicePloxy.RptAdCategorySummaryModel();

				// ������� ��Ʈ
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;
				remoteHeader.UserLevel     = Header.UserLevel;

				// ȣ������ ��Ʈ
				remoteData.LogDay = rptAdCategorySummaryModel.LogDay;
				remoteData.LogDayEnd = rptAdCategorySummaryModel.LogDayEnd;
				remoteData.AdType = rptAdCategorySummaryModel.AdType;
				
				// ������ ȣ�� Ÿ�Ӿƿ�����
				svc.Timeout = FrameSystem.m_SystemTimeout;  

				// ������ �޼ҵ� ȣ��
				remoteData = svc.GetRptAdCategorySummary(remoteHeader, remoteData);

				// ����ڵ�˻�
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// ��� ��Ʈ
				rptAdCategorySummaryModel.ReportDataSet = remoteData.ReportDataSet.Copy();
				rptAdCategorySummaryModel.ItemDataSet = remoteData.ItemDataSet.Copy();
				rptAdCategorySummaryModel.ResultCnt = remoteData.ResultCnt;
				rptAdCategorySummaryModel.ResultCD = remoteData.ResultCD;

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
