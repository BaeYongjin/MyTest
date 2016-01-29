// ===============================================================================
//
// StatisticsTotalManager.cs
//
// ��ü��� ���񽺸� ȣ���մϴ�. 
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
	/// ��ü��� �����񽺸� ȣ���մϴ�. 
	/// </summary>
	public class StatisticsTotalManager : BaseManager
	{
		/// <summary>
		/// ������
		/// </summary>
		/// <param name="systemModel"></param>
		/// <param name="commonModel"></param>

		public StatisticsTotalManager(SystemModel systemModel, CommonModel commonModel) : base(systemModel, commonModel)
		{
			_log = FrameSystem.oLog;
			_module = "StatisticsTotal";
			_Host  = FrameSystem.m_WebServer_Host;
			_Port  = FrameSystem.m_WebServer_Port;
			_Path  = FrameSystem.m_WebServer_App + "/ReportAd/StatisticsTotalService.asmx";
		}


		/// <summary>
		/// ��ü���  ��ȸ
		/// </summary>
		/// <param name="userModel"></param>
		public void GetStatisticsTotal(StatisticsTotalModel statisticsTotalModel)
		{

			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("��ü��� ��ȸ Start");
				_log.Debug("-----------------------------------------");
				
				// ������ �ν��Ͻ� ����
				StatisticsTotalServicePloxy.StatisticsTotalService svc = new StatisticsTotalServicePloxy.StatisticsTotalService();
			
				// ������URL ���� ��Ʈ
				svc.Url = _WebServiceUrl;

				// ����Ʈ �� ����
				StatisticsTotalServicePloxy.HeaderModel      remoteHeader = new AdManagerClient.StatisticsTotalServicePloxy.HeaderModel();
				StatisticsTotalServicePloxy.StatisticsTotalModel   remoteData   = new AdManagerClient.StatisticsTotalServicePloxy.StatisticsTotalModel();

				// ������� ��Ʈ
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;
				remoteHeader.UserLevel     = Header.UserLevel;

				// ȣ������ ��Ʈ
				remoteData.SearchMediaCode    = statisticsTotalModel.SearchMediaCode;	  
				remoteData.SearchRapCode      = statisticsTotalModel.SearchRapCode;	  
				remoteData.SearchAgencyCode   = statisticsTotalModel.SearchAgencyCode;	  
				remoteData.SearchKey          = statisticsTotalModel.SearchKey; 
      				
                // ������ ȣ�� Ÿ�Ӿƿ�����
                svc.Timeout = FrameSystem.m_SystemTimeout * 20;   // �۾��ð��� ���...

				// ������ �޼ҵ� ȣ��
				remoteData = svc.GetStatisticsTotal(remoteHeader, remoteData);

				// ����ڵ�˻�
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// ��� ��Ʈ
				statisticsTotalModel.ReportDataSet = remoteData.ReportDataSet.Copy();
				statisticsTotalModel.ResultCnt     = remoteData.ResultCnt;
				statisticsTotalModel.ResultCD      = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("��ü��� ��ȸ End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":GetStatisticsTotal():" + fe.ErrCode + ":" + fe.ResultMsg);
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
