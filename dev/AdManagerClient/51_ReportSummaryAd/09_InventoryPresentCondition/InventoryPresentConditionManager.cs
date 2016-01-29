// ===============================================================================
//
// InventoryPresentCondition.cs
//
// 광고일간레포트 조회 서비스를 호출합니다. 
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
	/// InventoryPresentConditionManager에 대한 요약 설명입니다.
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
		/// 인벤토리현황 조회
		/// </summary>
		/// <param name="InventoryPresentConditionModel"></param>
		public void	GetInventoryPresentCondition(InventoryPresentConditionModel inventoryPresentConditionModel)
		{

			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("인벤토리현황 Start");
				_log.Debug("-----------------------------------------");
				
				// 웹서비스 인스턴스 생성
				InventoryPresentConditionPloxy.InventoryPresentConditionService svc = new AdManagerClient.InventoryPresentConditionPloxy.InventoryPresentConditionService();
			
				// 웹서비스URL 동적 셋트
				svc.Url = _WebServiceUrl;

				// 리모트 모델 생성
				InventoryPresentConditionPloxy.HeaderModel remoteHeader = new AdManagerClient.InventoryPresentConditionPloxy.HeaderModel();
				InventoryPresentConditionPloxy.InventoryPresentConditionModel remoteData = new AdManagerClient.InventoryPresentConditionPloxy.InventoryPresentConditionModel();

				// 헤더정보 셋트
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;
				remoteHeader.UserLevel     = Header.UserLevel;

				// 호출정보 셋트
				remoteData.LogDay1    = inventoryPresentConditionModel.LogDay1;
				remoteData.LogDay2    = inventoryPresentConditionModel.LogDay2;
				remoteData.SearchType = inventoryPresentConditionModel.SearchType;
                				
				// 웹서비스 호출 타임아웃설정
				svc.Timeout = FrameSystem.m_SystemTimeout * 10;  

				// 웹서비스 메소드 호출
				remoteData = svc.GetInventoryPresentCondition(remoteHeader, remoteData);

				// 결과코드검사
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// 결과 셋트
				inventoryPresentConditionModel.ReportDataSet = remoteData.ReportDataSet.Copy();
				inventoryPresentConditionModel.ItemDataSet = remoteData.ItemDataSet.Copy();
				inventoryPresentConditionModel.ResultCnt = remoteData.ResultCnt;
				inventoryPresentConditionModel.ResultCD = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("인벤토리현황 End");
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
		/// 인벤토리 기준데이터 생성
		/// </summary>
		/// <param name="InventoryPresentConditionModel"></param>
		public void	SetInventorySummuryData(InventoryPresentConditionModel inventoryPresentConditionModel)
		{

			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("인벤토리기준데이터생성 Start");
				_log.Debug("-----------------------------------------");
				
				// 웹서비스 인스턴스 생성
				InventoryPresentConditionPloxy.InventoryPresentConditionService svc = new AdManagerClient.InventoryPresentConditionPloxy.InventoryPresentConditionService();
			
				// 웹서비스URL 동적 셋트
				svc.Url = _WebServiceUrl;

				// 리모트 모델 생성
				InventoryPresentConditionPloxy.HeaderModel remoteHeader = new AdManagerClient.InventoryPresentConditionPloxy.HeaderModel();
				InventoryPresentConditionPloxy.InventoryPresentConditionModel remoteData = new AdManagerClient.InventoryPresentConditionPloxy.InventoryPresentConditionModel();

				// 헤더정보 셋트
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;
				remoteHeader.UserLevel     = Header.UserLevel;

				// 호출정보 셋트
				remoteData.SearchType = inventoryPresentConditionModel.SearchType;
                				
				// 웹서비스 호출 타임아웃설정
				svc.Timeout = FrameSystem.m_SystemTimeout * 100;  

				// 웹서비스 메소드 호출
				remoteData = svc.SetInventorySummuryData(remoteHeader, remoteData);

				// 결과코드검사
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// 결과 셋트
				inventoryPresentConditionModel.ResultCD = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("인벤토리기준데이터생성 End");
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
