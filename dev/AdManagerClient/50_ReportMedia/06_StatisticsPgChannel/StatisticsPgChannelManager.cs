// ===============================================================================
//
// StatisticsPgChannelManager.cs
//
// ����������Ʈ ä����� ��ȸ ���񽺸� ȣ���մϴ�. 
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
	/// ����������Ʈ ä����� ��ȸ �����񽺸� ȣ���մϴ�. 
	/// </summary>
	public class StatisticsPgChannelManager : BaseManager
	{
		/// <summary>
		/// ������
		/// </summary>
		/// <param name="systemModel"></param>
		/// <param name="commonModel"></param>

		public StatisticsPgChannelManager(SystemModel systemModel, CommonModel commonModel) : base(systemModel, commonModel)
		{
			_log = FrameSystem.oLog;
			_module = "StatisticsPgChannel";
			_Host  = FrameSystem.m_WebServer_Host;
			_Port  = FrameSystem.m_WebServer_Port;
			_Path  = FrameSystem.m_WebServer_App + "/ReportMedia/StatisticsPgChannelService.asmx";
		}

		/// <summary>
		/// ����������Ʈ ä����� ��ȸ
		/// </summary>
		/// <param name="userModel"></param>
		public void GetStatisticsPgChannelReport(StatisticsPgChannelModel statisticsPgChannelModel)
		{

			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("����������Ʈ ä����� ��ȸ Start");
				_log.Debug("-----------------------------------------");
				
				// ������ �ν��Ͻ� ����
				StatisticsPgChannelServicePloxy.StatisticsPgChannelService svc = new StatisticsPgChannelServicePloxy.StatisticsPgChannelService();
			
				// ������URL ���� ��Ʈ
				svc.Url = _WebServiceUrl;

				// ����Ʈ �� ����
				StatisticsPgChannelServicePloxy.HeaderModel      remoteHeader = new AdManagerClient.StatisticsPgChannelServicePloxy.HeaderModel();
				StatisticsPgChannelServicePloxy.StatisticsPgChannelModel   remoteData   = new AdManagerClient.StatisticsPgChannelServicePloxy.StatisticsPgChannelModel();

				// ������� ��Ʈ
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;
				remoteHeader.UserLevel     = Header.UserLevel;

				// ȣ������ ��Ʈ
				remoteData.SearchMediaCode       = statisticsPgChannelModel.SearchMediaCode;	  
				remoteData.SearchType            = statisticsPgChannelModel.SearchType;       
				remoteData.SearchStartDay        = statisticsPgChannelModel.SearchStartDay;       
				remoteData.SearchEndDay          = statisticsPgChannelModel.SearchEndDay;       
				
                // ������ ȣ�� Ÿ�Ӿƿ�����
                svc.Timeout = FrameSystem.m_SystemTimeout * 20;   // �۾��ð��� ���...

				// ������ �޼ҵ� ȣ��
				remoteData = svc.GetStatisticsPgChannel(remoteHeader, remoteData);

				// ����ڵ�˻�
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// ��� ��Ʈ
				statisticsPgChannelModel.ReportDataSet = remoteData.ReportDataSet.Copy();
				statisticsPgChannelModel.ResultCnt     = remoteData.ResultCnt;
				statisticsPgChannelModel.ResultCD      = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("����������Ʈ ä����� ��ȸ End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":GetStatisticsPgChannelReport():" + fe.ErrCode + ":" + fe.ResultMsg);
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
