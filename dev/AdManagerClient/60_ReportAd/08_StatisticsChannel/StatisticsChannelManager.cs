// ===============================================================================
//
// StatisticsChannelManager.cs
//
// ������Ʈ ä����� ��ȸ ���񽺸� ȣ���մϴ�. 
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
	/// ������Ʈ ä����� ��ȸ �����񽺸� ȣ���մϴ�. 
	/// </summary>
	public class StatisticsChannelManager : BaseManager
	{
		/// <summary>
		/// ������
		/// </summary>
		/// <param name="systemModel"></param>
		/// <param name="commonModel"></param>

		public StatisticsChannelManager(SystemModel systemModel, CommonModel commonModel) : base(systemModel, commonModel)
		{
			_log = FrameSystem.oLog;
			_module = "StatisticsChannel";
			_Host  = FrameSystem.m_WebServer_Host;
			_Port  = FrameSystem.m_WebServer_Port;
			_Path  = FrameSystem.m_WebServer_App + "/ReportAd/StatisticsChannelService.asmx";
		}

		/// <summary>
		/// ������Ʈ ä����� ��ȸ
		/// </summary>
		/// <param name="userModel"></param>
		public void GetStatisticsChannelReport(StatisticsChannelModel statisticsChannelModel)
		{

			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("������Ʈ ä����� ��ȸ Start");
				_log.Debug("-----------------------------------------");
				
				// ������ �ν��Ͻ� ����
				StatisticsChannelServicePloxy.StatisticsChannelService svc = new StatisticsChannelServicePloxy.StatisticsChannelService();
			
				// ������URL ���� ��Ʈ
				svc.Url = _WebServiceUrl;

				// ����Ʈ �� ����
				StatisticsChannelServicePloxy.HeaderModel      remoteHeader = new AdManagerClient.StatisticsChannelServicePloxy.HeaderModel();
				StatisticsChannelServicePloxy.StatisticsChannelModel   remoteData   = new AdManagerClient.StatisticsChannelServicePloxy.StatisticsChannelModel();

				// ������� ��Ʈ
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;
				remoteHeader.UserLevel     = Header.UserLevel;

				// ȣ������ ��Ʈ
				remoteData.SearchMediaCode       = statisticsChannelModel.SearchMediaCode;	  
				remoteData.SearchContractSeq     = statisticsChannelModel.SearchContractSeq;	  
				remoteData.SearchItemNo          = statisticsChannelModel.SearchItemNo;	 
				remoteData.CampaignCode  		 = statisticsChannelModel.CampaignCode;	
				remoteData.SearchType            = statisticsChannelModel.SearchType;       
				remoteData.SearchStartDay        = statisticsChannelModel.SearchStartDay;       
				remoteData.SearchEndDay          = statisticsChannelModel.SearchEndDay;       
				
                // ������ ȣ�� Ÿ�Ӿƿ�����
                svc.Timeout = FrameSystem.m_SystemTimeout * 20;   // �۾��ð��� ���...

				// ������ �޼ҵ� ȣ��
				remoteData = svc.GetStatisticsChannel(remoteHeader, remoteData);

				// ����ڵ�˻�
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// ��� ��Ʈ
				statisticsChannelModel.ContractAmt   = remoteData.ContractAmt;
				statisticsChannelModel.TotalAdCnt    = remoteData.TotalAdCnt;
				statisticsChannelModel.ReportDataSet = remoteData.ReportDataSet.Copy();
				statisticsChannelModel.ResultCnt     = remoteData.ResultCnt;
				statisticsChannelModel.ResultCD      = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("������Ʈ ä����� ��ȸ End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":GetStatisticsChannelReport():" + fe.ErrCode + ":" + fe.ResultMsg);
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
