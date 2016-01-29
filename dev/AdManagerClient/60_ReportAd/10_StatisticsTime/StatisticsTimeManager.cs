// ===============================================================================
//
// StatisticsTimeManager.cs
//
// ������Ʈ �ð��뺰��� ��ȸ ���񽺸� ȣ���մϴ�. 
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
	/// ������Ʈ �ð��뺰��� ��ȸ �����񽺸� ȣ���մϴ�. 
	/// </summary>
	public class StatisticsTimeManager : BaseManager
	{
		/// <summary>
		/// ������
		/// </summary>
		/// <param name="systemModel"></param>
		/// <param name="commonModel"></param>

		public StatisticsTimeManager(SystemModel systemModel, CommonModel commonModel) : base(systemModel, commonModel)
		{
			_log = FrameSystem.oLog;
			_module = "StatisticsTime";
			_Host  = FrameSystem.m_WebServer_Host;
			_Port  = FrameSystem.m_WebServer_Port;
			_Path  = FrameSystem.m_WebServer_App + "/ReportAd/StatisticsTimeService.asmx";
		}

		/// <summary>
		/// ������Ʈ �ð��뺰��� ��ȸ
		/// </summary>
		/// <param name="userModel"></param>
		public void GetStatisticsTimeReport(StatisticsTimeModel statisticsTimeModel)
		{

			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("������Ʈ �ð��뺰��� ��ȸ Start");
				_log.Debug("-----------------------------------------");
				
				// ������ �ν��Ͻ� ����
				StatisticsTimeServicePloxy.StatisticsTimeService svc = new StatisticsTimeServicePloxy.StatisticsTimeService();
			
				// ������URL ���� ��Ʈ
				svc.Url = _WebServiceUrl;

				// ����Ʈ �� ����
				StatisticsTimeServicePloxy.HeaderModel      remoteHeader = new AdManagerClient.StatisticsTimeServicePloxy.HeaderModel();
				StatisticsTimeServicePloxy.StatisticsTimeModel   remoteData   = new AdManagerClient.StatisticsTimeServicePloxy.StatisticsTimeModel();

				// ������� ��Ʈ
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;
				remoteHeader.UserLevel     = Header.UserLevel;

				// ȣ������ ��Ʈ
				remoteData.SearchMediaCode       = statisticsTimeModel.SearchMediaCode;	  
				remoteData.SearchContractSeq     = statisticsTimeModel.SearchContractSeq;
				remoteData.SearchItemNo          = statisticsTimeModel.SearchItemNo;	
				remoteData.CampaignCode  		 = statisticsTimeModel.CampaignCode;	
				remoteData.SearchType            = statisticsTimeModel.SearchType;       
				remoteData.SearchStartDay        = statisticsTimeModel.SearchStartDay;       
				remoteData.SearchEndDay          = statisticsTimeModel.SearchEndDay;       
				
                // ������ ȣ�� Ÿ�Ӿƿ�����
                svc.Timeout = FrameSystem.m_SystemTimeout * 20;   // �۾��ð��� ���...

				// ������ �޼ҵ� ȣ��
				remoteData = svc.GetStatisticsTime(remoteHeader, remoteData);

				// ����ڵ�˻�
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// ��� ��Ʈ
				statisticsTimeModel.ContractAmt   = remoteData.ContractAmt;
				statisticsTimeModel.TotalAdCnt    = remoteData.TotalAdCnt;
				statisticsTimeModel.ReportDataSet = remoteData.ReportDataSet.Copy();
				statisticsTimeModel.ResultCnt     = remoteData.ResultCnt;
				statisticsTimeModel.ResultCD      = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("������Ʈ �ð��뺰��� ��ȸ End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":GetStatisticsTimeReport():" + fe.ErrCode + ":" + fe.ResultMsg);
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
