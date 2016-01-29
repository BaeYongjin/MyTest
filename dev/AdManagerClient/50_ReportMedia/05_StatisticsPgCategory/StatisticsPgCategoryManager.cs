// ===============================================================================
//
// StatisticsPgCategoryManager.cs
//
// ����������Ʈ ī�װ���� ��ȸ ���񽺸� ȣ���մϴ�. 
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
	/// ����������Ʈ ī�װ���� ��ȸ �����񽺸� ȣ���մϴ�. 
	/// </summary>
	public class StatisticsPgCategoryManager : BaseManager
	{
		/// <summary>
		/// ������
		/// </summary>
		/// <param name="systemModel"></param>
		/// <param name="commonModel"></param>

		public StatisticsPgCategoryManager(SystemModel systemModel, CommonModel commonModel) : base(systemModel, commonModel)
		{
			_log = FrameSystem.oLog;
			_module = "StatisticsPgCategory";
			_Host  = FrameSystem.m_WebServer_Host;
			_Port  = FrameSystem.m_WebServer_Port;
			_Path  = FrameSystem.m_WebServer_App + "/ReportMedia/StatisticsPgCategoryService.asmx";
		}

		/// <summary>
		/// ����������Ʈ ī�װ���� ��ȸ
		/// </summary>
		/// <param name="userModel"></param>
		public void GetStatisticsPgCategoryReport(StatisticsPgCategoryModel statisticsPgCategoryModel)
		{

			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("����������Ʈ ī�װ���� ��ȸ Start");
				_log.Debug("-----------------------------------------");
				
				// ������ �ν��Ͻ� ����
				StatisticsPgCategoryServicePloxy.StatisticsPgCategoryService svc = new StatisticsPgCategoryServicePloxy.StatisticsPgCategoryService();
			
				// ������URL ���� ��Ʈ
				svc.Url = _WebServiceUrl;

				// ����Ʈ �� ����
				StatisticsPgCategoryServicePloxy.HeaderModel      remoteHeader = new AdManagerClient.StatisticsPgCategoryServicePloxy.HeaderModel();
				StatisticsPgCategoryServicePloxy.StatisticsPgCategoryModel   remoteData   = new AdManagerClient.StatisticsPgCategoryServicePloxy.StatisticsPgCategoryModel();

				// ������� ��Ʈ
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;
				remoteHeader.UserLevel     = Header.UserLevel;

				// ȣ������ ��Ʈ
				remoteData.SearchMediaCode       = statisticsPgCategoryModel.SearchMediaCode;	  
				remoteData.SearchType            = statisticsPgCategoryModel.SearchType;       
				remoteData.SearchStartDay        = statisticsPgCategoryModel.SearchStartDay;       
				remoteData.SearchEndDay          = statisticsPgCategoryModel.SearchEndDay;       
				
                // ������ ȣ�� Ÿ�Ӿƿ�����
                svc.Timeout = FrameSystem.m_SystemTimeout * 20;   // �۾��ð��� ���...

				// ������ �޼ҵ� ȣ��
				remoteData = svc.GetStatisticsPgCategory(remoteHeader, remoteData);

				// ����ڵ�˻�
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// ��� ��Ʈ
				statisticsPgCategoryModel.ReportDataSet = remoteData.ReportDataSet.Copy();
				statisticsPgCategoryModel.ResultCnt     = remoteData.ResultCnt;
				statisticsPgCategoryModel.ResultCD      = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("����������Ʈ ī�װ���� ��ȸ End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":GetStatisticsPgCategoryReport():" + fe.ErrCode + ":" + fe.ResultMsg);
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
