// ===============================================================================
//
// InventoryPresentCondition.cs
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
	/// InventoryPresentConditionManager�� ���� ��� �����Դϴ�.
	/// </summary>
	public class InventoryPresentConditionManager : BaseManager
	{
		public InventoryPresentConditionManager(SystemModel systemModel, CommonModel commonModel) : base(systemModel, commonModel)
		{
			_log = FrameSystem.oLog;
			_module = "InventoryPresentCondition";
			_Host  = FrameSystem.m_WebServer_Host;
			_Port  = FrameSystem.m_WebServer_Port;
			_Path  = FrameSystem.m_WebServer_App + "/ReportSummaryAd/InventoryPresentConditionService.asmx";
		}

		/// <summary>
		/// �κ��丮��Ȳ ��ȸ
		/// </summary>
		/// <param name="InventoryPresentConditionModel"></param>
		public void	GetInventoryPresentCondition(InventoryPresentConditionModel inventoryPresentConditionModel)
		{

			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("�κ��丮��Ȳ Start");
				_log.Debug("-----------------------------------------");
				
				// ������ �ν��Ͻ� ����
				InventoryPresentConditionPloxy.InventoryPresentConditionService svc = new AdManagerClient.InventoryPresentConditionPloxy.InventoryPresentConditionService();
			
				// ������URL ���� ��Ʈ
				svc.Url = _WebServiceUrl;

				// ����Ʈ �� ����
				InventoryPresentConditionPloxy.HeaderModel remoteHeader = new AdManagerClient.InventoryPresentConditionPloxy.HeaderModel();
				InventoryPresentConditionPloxy.InventoryPresentConditionModel remoteData = new AdManagerClient.InventoryPresentConditionPloxy.InventoryPresentConditionModel();

				// ������� ��Ʈ
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;
				remoteHeader.UserLevel     = Header.UserLevel;

				// ȣ������ ��Ʈ
				remoteData.LogDay1    = inventoryPresentConditionModel.LogDay1;
				remoteData.LogDay2    = inventoryPresentConditionModel.LogDay2;
				remoteData.SearchType = inventoryPresentConditionModel.SearchType;
                				
				// ������ ȣ�� Ÿ�Ӿƿ�����
				svc.Timeout = FrameSystem.m_SystemTimeout * 10;  

				// ������ �޼ҵ� ȣ��
				remoteData = svc.GetInventoryPresentCondition(remoteHeader, remoteData);

				// ����ڵ�˻�
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// ��� ��Ʈ
				inventoryPresentConditionModel.ReportDataSet = remoteData.ReportDataSet.Copy();
				inventoryPresentConditionModel.ItemDataSet = remoteData.ItemDataSet.Copy();
				inventoryPresentConditionModel.ResultCnt = remoteData.ResultCnt;
				inventoryPresentConditionModel.ResultCD = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("�κ��丮��Ȳ End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":GetInventoryPresentCondition():" + fe.ErrCode + ":" + fe.ResultMsg);
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

		/// <summary>
		/// �κ��丮 ���ص����� ����
		/// </summary>
		/// <param name="InventoryPresentConditionModel"></param>
		public void	SetInventorySummuryData(InventoryPresentConditionModel inventoryPresentConditionModel)
		{

			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("�κ��丮���ص����ͻ��� Start");
				_log.Debug("-----------------------------------------");
				
				// ������ �ν��Ͻ� ����
				InventoryPresentConditionPloxy.InventoryPresentConditionService svc = new AdManagerClient.InventoryPresentConditionPloxy.InventoryPresentConditionService();
			
				// ������URL ���� ��Ʈ
				svc.Url = _WebServiceUrl;

				// ����Ʈ �� ����
				InventoryPresentConditionPloxy.HeaderModel remoteHeader = new AdManagerClient.InventoryPresentConditionPloxy.HeaderModel();
				InventoryPresentConditionPloxy.InventoryPresentConditionModel remoteData = new AdManagerClient.InventoryPresentConditionPloxy.InventoryPresentConditionModel();

				// ������� ��Ʈ
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;
				remoteHeader.UserLevel     = Header.UserLevel;

				// ȣ������ ��Ʈ
				remoteData.SearchType = inventoryPresentConditionModel.SearchType;
                				
				// ������ ȣ�� Ÿ�Ӿƿ�����
				svc.Timeout = FrameSystem.m_SystemTimeout * 100;  

				// ������ �޼ҵ� ȣ��
				remoteData = svc.SetInventorySummuryData(remoteHeader, remoteData);

				// ����ڵ�˻�
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// ��� ��Ʈ
				inventoryPresentConditionModel.ResultCD = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("�κ��丮���ص����ͻ��� End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":SetInventorySummuryData():" + fe.ErrCode + ":" + fe.ResultMsg);
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
