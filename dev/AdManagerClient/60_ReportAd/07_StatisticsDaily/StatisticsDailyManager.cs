// ===============================================================================
//
// StatisticsDailyManager.cs
//
// ������Ʈ �Ϻ���� ��ȸ ���񽺸� ȣ���մϴ�. 
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
	/// ������Ʈ �Ϻ���� ��ȸ �����񽺸� ȣ���մϴ�. 
	/// </summary>
	public class StatisticsDailyManager : BaseManager
	{
		/// <summary>
		/// ������
		/// </summary>
		/// <param name="systemModel"></param>
		/// <param name="commonModel"></param>

		public StatisticsDailyManager(SystemModel systemModel, CommonModel commonModel) : base(systemModel, commonModel)
		{
			_log = FrameSystem.oLog;
			_module = "StatisticsDaily";
			_Host  = FrameSystem.m_WebServer_Host;
			_Port  = FrameSystem.m_WebServer_Port;
			_Path  = FrameSystem.m_WebServer_App + "/ReportAd/StatisticsDailyService.asmx";
		}

		/// <summary>
		/// ������Ʈ �Ϻ���� ��ȸ
		/// </summary>
		/// <param name="userModel"></param>
		public void GetStatisticsDailyReport(StatisticsDailyModel statisticsDailyModel)
		{

			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("������Ʈ �Ϻ���� ��ȸ Start");
				_log.Debug("-----------------------------------------");
				
				// ������ �ν��Ͻ� ����
				StatisticsDailyServicePloxy.StatisticsDailyService svc = new StatisticsDailyServicePloxy.StatisticsDailyService();
			
				// ������URL ���� ��Ʈ
				svc.Url = _WebServiceUrl;

				// ����Ʈ �� ����
				StatisticsDailyServicePloxy.HeaderModel      remoteHeader = new AdManagerClient.StatisticsDailyServicePloxy.HeaderModel();
				StatisticsDailyServicePloxy.StatisticsDailyModel   remoteData   = new AdManagerClient.StatisticsDailyServicePloxy.StatisticsDailyModel();

				// ������� ��Ʈ
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;
				remoteHeader.UserLevel     = Header.UserLevel;

				// ȣ������ ��Ʈ
				remoteData.SearchMediaCode       = statisticsDailyModel.SearchMediaCode;	  
				remoteData.SearchContractSeq     = statisticsDailyModel.SearchContractSeq;	  
				remoteData.SearchItemNo          = statisticsDailyModel.SearchItemNo;	 
				remoteData.CampaignCode  		 = statisticsDailyModel.CampaignCode;	
				remoteData.SearchType            = statisticsDailyModel.SearchType;       
				remoteData.SearchStartDay        = statisticsDailyModel.SearchStartDay;       
				remoteData.SearchEndDay          = statisticsDailyModel.SearchEndDay;       
				
                // ������ ȣ�� Ÿ�Ӿƿ�����
                svc.Timeout = FrameSystem.m_SystemTimeout * 20;   // �۾��ð��� ���...

				// ������ �޼ҵ� ȣ��
				remoteData = svc.GetStatisticsDaily(remoteHeader, remoteData);

				// ����ڵ�˻�
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// ��� ��Ʈ
				statisticsDailyModel.ContractAmt   = remoteData.ContractAmt;
				statisticsDailyModel.TotalAdCnt    = remoteData.TotalAdCnt;
				statisticsDailyModel.ReportDataSet = remoteData.ReportDataSet.Copy();
				statisticsDailyModel.ResultCnt     = remoteData.ResultCnt;
				statisticsDailyModel.ResultCD      = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("������Ʈ �Ϻ���� ��ȸ End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":GetStatisticsDailyReport():" + fe.ErrCode + ":" + fe.ResultMsg);
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
