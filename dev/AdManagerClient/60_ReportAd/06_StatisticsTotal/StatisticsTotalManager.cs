// ===============================================================================
//
// StatisticsTotalManager.cs
//
// 전체통계 서비스를 호출합니다. 
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
	/// 전체통계 웹서비스를 호출합니다. 
	/// </summary>
	public class StatisticsTotalManager : BaseManager
	{
		/// <summary>
		/// 생성자
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
		/// 전체통계  조회
		/// </summary>
		/// <param name="userModel"></param>
		public void GetStatisticsTotal(StatisticsTotalModel statisticsTotalModel)
		{

			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("전체통계 조회 Start");
				_log.Debug("-----------------------------------------");
				
				// 웹서비스 인스턴스 생성
				StatisticsTotalServicePloxy.StatisticsTotalService svc = new StatisticsTotalServicePloxy.StatisticsTotalService();
			
				// 웹서비스URL 동적 셋트
				svc.Url = _WebServiceUrl;

				// 리모트 모델 생성
				StatisticsTotalServicePloxy.HeaderModel      remoteHeader = new AdManagerClient.StatisticsTotalServicePloxy.HeaderModel();
				StatisticsTotalServicePloxy.StatisticsTotalModel   remoteData   = new AdManagerClient.StatisticsTotalServicePloxy.StatisticsTotalModel();

				// 헤더정보 셋트
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;
				remoteHeader.UserLevel     = Header.UserLevel;

				// 호출정보 셋트
				remoteData.SearchMediaCode    = statisticsTotalModel.SearchMediaCode;	  
				remoteData.SearchRapCode      = statisticsTotalModel.SearchRapCode;	  
				remoteData.SearchAgencyCode   = statisticsTotalModel.SearchAgencyCode;	  
				remoteData.SearchKey          = statisticsTotalModel.SearchKey; 
      				
                // 웹서비스 호출 타임아웃설정
                svc.Timeout = FrameSystem.m_SystemTimeout * 20;   // 작업시간이 길다...

				// 웹서비스 메소드 호출
				remoteData = svc.GetStatisticsTotal(remoteHeader, remoteData);

				// 결과코드검사
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// 결과 셋트
				statisticsTotalModel.ReportDataSet = remoteData.ReportDataSet.Copy();
				statisticsTotalModel.ResultCnt     = remoteData.ResultCnt;
				statisticsTotalModel.ResultCD      = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("전체통계 조회 End");
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
