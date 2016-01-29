// ===============================================================================
//
// StatisticsTimeManager.cs
//
// 광고리포트 시간대별통계 조회 서비스를 호출합니다. 
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
	/// 광고리포트 시간대별통계 조회 웹서비스를 호출합니다. 
	/// </summary>
	public class StatisticsTimeManager : BaseManager
	{
		/// <summary>
		/// 생성자
		/// </summary>
		/// <param name="systemModel"></param>
		/// <param name="commonModel"></param>

		public StatisticsTimeManager(SystemModel systemModel, CommonModel commonModel) : base(systemModel, commonModel)
		{
			_log = FrameSystem.oLog;
			_module = "StatisticsTime";
			_Host  = FrameSystem.m_WebServer_Host;
			_Port  = FrameSystem.m_WebServer_Port;
			_Path  = FrameSystem.m_WebServer_App + "/ReportAd/StatisticsTimeService.asmx";
		}

		/// <summary>
		/// 광고리포트 시간대별통계 조회
		/// </summary>
		/// <param name="userModel"></param>
		public void GetStatisticsTimeReport(StatisticsTimeModel statisticsTimeModel)
		{

			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("광고리포트 시간대별통계 조회 Start");
				_log.Debug("-----------------------------------------");
				
				// 웹서비스 인스턴스 생성
				StatisticsTimeServicePloxy.StatisticsTimeService svc = new StatisticsTimeServicePloxy.StatisticsTimeService();
			
				// 웹서비스URL 동적 셋트
				svc.Url = _WebServiceUrl;

				// 리모트 모델 생성
				StatisticsTimeServicePloxy.HeaderModel      remoteHeader = new AdManagerClient.StatisticsTimeServicePloxy.HeaderModel();
				StatisticsTimeServicePloxy.StatisticsTimeModel   remoteData   = new AdManagerClient.StatisticsTimeServicePloxy.StatisticsTimeModel();

				// 헤더정보 셋트
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;
				remoteHeader.UserLevel     = Header.UserLevel;

				// 호출정보 셋트
				remoteData.SearchMediaCode       = statisticsTimeModel.SearchMediaCode;	  
				remoteData.SearchContractSeq     = statisticsTimeModel.SearchContractSeq;
				remoteData.SearchItemNo          = statisticsTimeModel.SearchItemNo;	
				remoteData.CampaignCode  		 = statisticsTimeModel.CampaignCode;	
				remoteData.SearchType            = statisticsTimeModel.SearchType;       
				remoteData.SearchStartDay        = statisticsTimeModel.SearchStartDay;       
				remoteData.SearchEndDay          = statisticsTimeModel.SearchEndDay;       
				
                // 웹서비스 호출 타임아웃설정
                svc.Timeout = FrameSystem.m_SystemTimeout * 20;   // 작업시간이 길다...

				// 웹서비스 메소드 호출
				remoteData = svc.GetStatisticsTime(remoteHeader, remoteData);

				// 결과코드검사
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// 결과 셋트
				statisticsTimeModel.ContractAmt   = remoteData.ContractAmt;
				statisticsTimeModel.TotalAdCnt    = remoteData.TotalAdCnt;
				statisticsTimeModel.ReportDataSet = remoteData.ReportDataSet.Copy();
				statisticsTimeModel.ResultCnt     = remoteData.ResultCnt;
				statisticsTimeModel.ResultCD      = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("광고리포트 시간대별통계 조회 End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":GetStatisticsTimeReport():" + fe.ErrCode + ":" + fe.ResultMsg);
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
