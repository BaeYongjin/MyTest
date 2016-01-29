// ===============================================================================
//
// AdHitStatusManager.cs
//
// 광고별 시청현황 조회 서비스를 호출합니다. 
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
	/// 광고별 시청현황 조회 웹서비스를 호출합니다. 
	/// </summary>
	public class AdHitStatusManager : BaseManager
	{
		/// <summary>
		/// 생성자
		/// </summary>
		/// <param name="systemModel"></param>
		/// <param name="commonModel"></param>

		public AdHitStatusManager(SystemModel systemModel, CommonModel commonModel) : base(systemModel, commonModel)
		{
			_log = FrameSystem.oLog;
			_module = "AdHitStatus";
			_Host  = FrameSystem.m_WebServer_Host;
			_Port  = FrameSystem.m_WebServer_Port;
			_Path  = FrameSystem.m_WebServer_App + "/ReportAd/AdHitStatusService.asmx";
		}


		/// <summary>
		/// 광고별 시청현황  조회
        /// 2012/02/09 광고타입조건 추가함.
		/// </summary>
		/// <param name="userModel"></param>
		public void GetAdHitStatus(AdHitStatusModel adHitStatusModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("광고별 시청현황 조회 Start");
				_log.Debug("-----------------------------------------");
				
				// 웹서비스 인스턴스 생성
				AdHitStatusServicePloxy.AdHitStatusService svc = new AdHitStatusServicePloxy.AdHitStatusService();
			
				// 웹서비스URL 동적 셋트
				svc.Url = _WebServiceUrl;

				// 리모트 모델 생성
				AdHitStatusServicePloxy.HeaderModel         remoteHeader = new AdManagerClient.AdHitStatusServicePloxy.HeaderModel();
				AdHitStatusServicePloxy.AdHitStatusModel    remoteData   = new AdManagerClient.AdHitStatusServicePloxy.AdHitStatusModel();

				// 헤더정보 셋트
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;
				remoteHeader.UserLevel     = Header.UserLevel;

				// 호출정보 셋트
				remoteData.SearchMediaCode  = adHitStatusModel.SearchMediaCode;	  
				remoteData.SearchRapCode    = adHitStatusModel.SearchRapCode;	  
				remoteData.SearchAgencyCode = adHitStatusModel.SearchAgencyCode;	  
				remoteData.SearchDay        = adHitStatusModel.SearchDay;       
				remoteData.SearchKey        = adHitStatusModel.SearchKey;
                remoteData.SearchAdType     = adHitStatusModel.SearchAdType;
      				
                // 웹서비스 호출 타임아웃설정
                svc.Timeout = FrameSystem.m_SystemTimeout * 20;   // 작업시간이 길다...
				remoteData = svc.GetAdHitStatus(remoteHeader, remoteData);

				// 결과코드검사
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// 결과 셋트
				adHitStatusModel.ReportDataSet = remoteData.ReportDataSet.Copy();
				adHitStatusModel.ResultCnt     = remoteData.ResultCnt;
				adHitStatusModel.ResultCD      = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("광고별 시청현황 조회 End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":GetAdHitStatus():" + fe.ErrCode + ":" + fe.ResultMsg);
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
