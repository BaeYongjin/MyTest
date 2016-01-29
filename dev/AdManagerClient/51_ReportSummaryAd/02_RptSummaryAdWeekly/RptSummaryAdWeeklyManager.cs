// ===============================================================================
//
// RptSummaryAdWeeklyManager.cs
//
// �����ְ�����Ʈ ��ȸ ���񽺸� ȣ���մϴ�. 
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
	public class RptSummaryAdWeeklyManager : BaseManager
	{
		public RptSummaryAdWeeklyManager(SystemModel systemModel, CommonModel commonModel) : base(systemModel, commonModel)
		{
			_log = FrameSystem.oLog;
			_module = "RptSummaryAdDaily";
			_Host  = FrameSystem.m_WebServer_Host;
			_Port  = FrameSystem.m_WebServer_Port;
			_Path  = FrameSystem.m_WebServer_App + "/ReportSummaryAd/RptSummaryAdWeeklyService.asmx";
		}

		/// <summary>
		/// �����ְ�����Ʈ ��ȸ
		/// </summary>
		/// <param name="RptSummaryAdWeekly"></param>
		public void	GetRptSummaryAdWeeklyList(RptSummaryAdWeeklyModel rptSummaryAdWeeklyModel)
		{

			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("�����ְ�����Ʈ��� ��ȸ Start");
				_log.Debug("-----------------------------------------");
				
				// ������ �ν��Ͻ� ����
				RptSummaryAdWeeklyServicePloxy.RptSummaryAdWeeklyService svc = new AdManagerClient.RptSummaryAdWeeklyServicePloxy.RptSummaryAdWeeklyService();
			
				// ������URL ���� ��Ʈ
				svc.Url = _WebServiceUrl;

				// ����Ʈ �� ����
				RptSummaryAdWeeklyServicePloxy.HeaderModel remoteHeader = new AdManagerClient.RptSummaryAdWeeklyServicePloxy.HeaderModel();
				RptSummaryAdWeeklyServicePloxy.RptSummaryAdWeeklyModel remoteData = new AdManagerClient.RptSummaryAdWeeklyServicePloxy.RptSummaryAdWeeklyModel();
				

				// ������� ��Ʈ
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;
				remoteHeader.UserLevel     = Header.UserLevel;

				// ȣ������ ��Ʈ
				remoteData.SearchStartDay = rptSummaryAdWeeklyModel.SearchStartDay;
				remoteData.SearchDay = rptSummaryAdWeeklyModel.SearchDay;
				
				// ������ ȣ�� Ÿ�Ӿƿ�����
				svc.Timeout = 1000 * 60;

				// ������ �޼ҵ� ȣ��
				remoteData = svc.GetRptSummaryAdWeeklyList(remoteHeader, remoteData);
				
				// ����ڵ�˻�
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// ��� ��Ʈ
				rptSummaryAdWeeklyModel.RptWeeklyDataSet = remoteData.RptWeeklyDataSet.Copy();
				rptSummaryAdWeeklyModel.ResultCnt = remoteData.ResultCnt;
				rptSummaryAdWeeklyModel.ResultCD = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("�����ְ�����Ʈ��� ��ȸ End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":GetRptSummaryAdWeeklyList():" + fe.ErrCode + ":" + fe.ResultMsg);
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
