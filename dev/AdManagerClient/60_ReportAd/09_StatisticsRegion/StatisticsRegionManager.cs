// ===============================================================================
//
// StatisticsRegionManager.cs
//
// ������Ʈ ��������� ��ȸ ���񽺸� ȣ���մϴ�. 
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
	/// ������Ʈ ��������� ��ȸ �����񽺸� ȣ���մϴ�. 
	/// </summary>
	public class StatisticsRegionManager : BaseManager
	{
		/// <summary>
		/// ������
		/// </summary>
		/// <param name="systemModel"></param>
		/// <param name="commonModel"></param>

		public StatisticsRegionManager(SystemModel systemModel, CommonModel commonModel) : base(systemModel, commonModel)
		{
			_log = FrameSystem.oLog;
			_module = "StatisticsRegion";
			_Host  = FrameSystem.m_WebServer_Host;
			_Port  = FrameSystem.m_WebServer_Port;
			_Path  = FrameSystem.m_WebServer_App + "/ReportAd/StatisticsRegionService.asmx";
		}

		/// <summary>
		/// ������Ʈ ��������� ��ȸ
		/// </summary>
		/// <param name="userModel"></param>
		public void GetStatisticsRegionReport(StatisticsRegionModel statisticsRegionModel)
		{

			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("������Ʈ ��������� ��ȸ Start");
				_log.Debug("-----------------------------------------");
				
				// ������ �ν��Ͻ� ����
				StatisticsRegionServicePloxy.StatisticsRegionService svc = new StatisticsRegionServicePloxy.StatisticsRegionService();
			
				// ������URL ���� ��Ʈ
				svc.Url = _WebServiceUrl;

				// ����Ʈ �� ����
				StatisticsRegionServicePloxy.HeaderModel      remoteHeader = new AdManagerClient.StatisticsRegionServicePloxy.HeaderModel();
				StatisticsRegionServicePloxy.StatisticsRegionModel   remoteData   = new AdManagerClient.StatisticsRegionServicePloxy.StatisticsRegionModel();

				// ������� ��Ʈ
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;
				remoteHeader.UserLevel     = Header.UserLevel;

				// ȣ������ ��Ʈ
				remoteData.SearchMediaCode       = statisticsRegionModel.SearchMediaCode;
				remoteData.SearchContractSeq     = statisticsRegionModel.SearchContractSeq;	  
				remoteData.SearchItemNo          = statisticsRegionModel.SearchItemNo;	
				remoteData.CampaignCode  		 = statisticsRegionModel.CampaignCode;	
				remoteData.SearchType            = statisticsRegionModel.SearchType;       
				remoteData.SearchStartDay        = statisticsRegionModel.SearchStartDay;       
				remoteData.SearchEndDay          = statisticsRegionModel.SearchEndDay;       
				
                // ������ ȣ�� Ÿ�Ӿƿ�����
                svc.Timeout = FrameSystem.m_SystemTimeout * 20;   // �۾��ð��� ���...

				// ������ �޼ҵ� ȣ��
				remoteData = svc.GetStatisticsRegion(remoteHeader, remoteData);

				// ����ڵ�˻�
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// ��� ��Ʈ
				statisticsRegionModel.ContractAmt   = remoteData.ContractAmt;
				statisticsRegionModel.TotalAdCnt    = remoteData.TotalAdCnt;
				statisticsRegionModel.ReportDataSet = remoteData.ReportDataSet.Copy();
				statisticsRegionModel.ResultCnt     = remoteData.ResultCnt;
				statisticsRegionModel.ResultCD      = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("������Ʈ ��������� ��ȸ End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":GetStatisticsRegionReport():" + fe.ErrCode + ":" + fe.ResultMsg);
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
