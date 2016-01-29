// ===============================================================================
//
// DailyAdHitManager.cs
//
// 일별 광고 시청횟수 집계 조회 서비스를 호출합니다. 
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
	/// 일별 광고 시청횟수집계 조회 웹서비스를 호출합니다. 
	/// </summary>
	public class DailyAdHitManager : BaseManager
	{
		/// <summary>
		/// 생성자
		/// </summary>
		/// <param name="systemModel"></param>
		/// <param name="commonModel"></param>

		public DailyAdHitManager(SystemModel systemModel, CommonModel commonModel) : base(systemModel, commonModel)
		{
			_log = FrameSystem.oLog;
			_module = "DailyAdHit";
			_Host  = FrameSystem.m_WebServer_Host;
			_Port  = FrameSystem.m_WebServer_Port;
			_Path  = FrameSystem.m_WebServer_App + "/ReportAd/DailyAdHitService.asmx";
		}

		/// <summary>
		/// 일별 광고 시청횟수집계 조회조회
		/// </summary>
		/// <param name="userModel"></param>
		public void GetDailyAdHitReport(DailyAdHitModel dailyAdHitModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("일별 광고 시청횟수 집계 조회 Start");
				_log.Debug("-----------------------------------------");
				
				// 웹서비스 인스턴스 생성
				DailyAdHitServicePloxy.DailyAdHitService svc = new DailyAdHitServicePloxy.DailyAdHitService();
			
				// 웹서비스URL 동적 셋트
				svc.Url = _WebServiceUrl;

				// 리모트 모델 생성
				DailyAdHitServicePloxy.HeaderModel      remoteHeader = new AdManagerClient.DailyAdHitServicePloxy.HeaderModel();
				DailyAdHitServicePloxy.DailyAdHitModel  remoteData   = new AdManagerClient.DailyAdHitServicePloxy.DailyAdHitModel();

				// 헤더정보 셋트
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;
				remoteHeader.UserLevel     = Header.UserLevel;

				// 호출정보 셋트
				remoteData.SearchMediaCode		 = dailyAdHitModel.SearchMediaCode;	  
				remoteData.SearchContractSeq	 = dailyAdHitModel.SearchContractSeq;	  
				remoteData.SearchItemNo  		 = dailyAdHitModel.SearchItemNo;	
				remoteData.CampaignCode  		 = dailyAdHitModel.CampaignCode;	
				remoteData.SearchType 			 = dailyAdHitModel.SearchType;       
				remoteData.SearchBgnDay 		 = dailyAdHitModel.SearchBgnDay;       
				remoteData.SearchEndDay 		 = dailyAdHitModel.SearchEndDay;       
				
                // 웹서비스 호출 타임아웃설정
                svc.Timeout = FrameSystem.m_SystemTimeout * 20;   // 작업시간이 길다...
				// 웹서비스 메소드 호출
				remoteData = svc.GetDailyAdHit(remoteHeader, remoteData);

				// 결과코드검사
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// 결과 셋트
				dailyAdHitModel.ReportDataSet = remoteData.ReportDataSet.Copy();
				dailyAdHitModel.HeaderDataSet = remoteData.HeaderDataSet.Copy();
				dailyAdHitModel.ResultCnt     = remoteData.ResultCnt;
				dailyAdHitModel.ResultCD      = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("일별 광고 시청횟수 집계 조회 End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":GetDailyAdHit():" + fe.ErrCode + ":" + fe.ResultMsg);
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
