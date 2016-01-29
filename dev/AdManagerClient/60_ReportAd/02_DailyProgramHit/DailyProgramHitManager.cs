// ===============================================================================
//
// DailyProgramHitManager.cs
//
// 일별 프로그램 시청횟수집계 조회 서비스를 호출합니다. 
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
	/// 일별 프로그램 시청횟수집계 조회 웹서비스를 호출합니다. 
	/// </summary>
	public class DailyProgramHitManager : BaseManager
	{
		/// <summary>
		/// 생성자
		/// </summary>
		/// <param name="systemModel"></param>
		/// <param name="commonModel"></param>

		public DailyProgramHitManager(SystemModel systemModel, CommonModel commonModel) : base(systemModel, commonModel)
		{
			_log = FrameSystem.oLog;
			_module = "DailyProgramHit";
			_Host  = FrameSystem.m_WebServer_Host;
			_Port  = FrameSystem.m_WebServer_Port;
			_Path  = FrameSystem.m_WebServer_App + "/ReportAd/DailyProgramHitService.asmx";
		}

		/// <summary>
		/// 일별 프로그램 시청횟수집계 조회조회
		/// </summary>
		/// <param name="userModel"></param>
		public void GetDailyProgramHitReport(DailyProgramHitModel dailyProgramHitModel)
		{

			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("일별 프로그램 시청횟수집계 조회 Start");
				_log.Debug("-----------------------------------------");
				
				// 웹서비스 인스턴스 생성
				DailyProgramHitServicePloxy.DailyProgramHitService svc = new DailyProgramHitServicePloxy.DailyProgramHitService();
			
				// 웹서비스URL 동적 셋트
				svc.Url = _WebServiceUrl;

				// 리모트 모델 생성
				DailyProgramHitServicePloxy.HeaderModel          remoteHeader = new AdManagerClient.DailyProgramHitServicePloxy.HeaderModel();
				DailyProgramHitServicePloxy.DailyProgramHitModel   remoteData   = new AdManagerClient.DailyProgramHitServicePloxy.DailyProgramHitModel();

				// 헤더정보 셋트
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;
				remoteHeader.UserLevel     = Header.UserLevel;

				// 호출정보 셋트
				remoteData.SearchMediaCode		 = dailyProgramHitModel.SearchMediaCode;	  
				remoteData.SearchContractSeq	 = dailyProgramHitModel.SearchContractSeq;	  
				remoteData.SearchItemNo  		 = dailyProgramHitModel.SearchItemNo;	
				remoteData.CampaignCode  		 = dailyProgramHitModel.CampaignCode;	
				remoteData.SearchType 			 = dailyProgramHitModel.SearchType;       
				remoteData.SearchBgnDay 		 = dailyProgramHitModel.SearchBgnDay;       
				remoteData.SearchEndDay 		 = dailyProgramHitModel.SearchEndDay;       
				
                // 웹서비스 호출 타임아웃설정
                svc.Timeout = FrameSystem.m_SystemTimeout * 20;   // 작업시간이 길다...
				// 웹서비스 메소드 호출
				remoteData = svc.GetDailyProgramHit(remoteHeader, remoteData);

				// 결과코드검사
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// 결과 셋트
				dailyProgramHitModel.ReportDataSet = remoteData.ReportDataSet.Copy();
				dailyProgramHitModel.HeaderDataSet = remoteData.HeaderDataSet.Copy();
				dailyProgramHitModel.ResultCnt     = remoteData.ResultCnt;
				dailyProgramHitModel.ResultCD      = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("일별 프로그램 시청횟수집계 조회 End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":GetDailyProgramHit():" + fe.ErrCode + ":" + fe.ResultMsg);
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
