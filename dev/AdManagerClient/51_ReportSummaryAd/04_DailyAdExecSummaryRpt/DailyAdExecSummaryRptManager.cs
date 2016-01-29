// ===============================================================================
//
// DailyAdExecSummaryRptManager.cs
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
	/// DailyAdExecSummaryRptManager에 대한 요약 설명입니다.
	/// </summary>
	public class DailyAdExecSummaryRptManager : BaseManager
	{
		public DailyAdExecSummaryRptManager(SystemModel systemModel, CommonModel commonModel) : base(systemModel, commonModel)
		{
			_log = FrameSystem.oLog;
			_module = "DailyAdExecSummaryRpt";
			_Host  = FrameSystem.m_WebServer_Host;
			_Port  = FrameSystem.m_WebServer_Port;
			_Path  = FrameSystem.m_WebServer_App + "/ReportSummaryAd/DailyAdExecSummaryRptService.asmx";
		}

		/// <summary>
		/// 광고일간레포트 조회
		/// </summary>
		/// <param name="DailyAdExecSummaryRpt"></param>
		public void	GetDailyAdExecSummary(DailyAdExecSummaryRptModel dailyAdExecSummaryRptModel)
		{

			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("일간광고집행종합 조회 Start");
				_log.Debug("-----------------------------------------");
				
				// 웹서비스 인스턴스 생성
				DailyAdExecSummaryRptServicePloxy.DailyAdExecSummaryRptService svc = new AdManagerClient.DailyAdExecSummaryRptServicePloxy.DailyAdExecSummaryRptService();
			
				// 웹서비스URL 동적 셋트
				svc.Url = _WebServiceUrl;

				// 리모트 모델 생성
				DailyAdExecSummaryRptServicePloxy.HeaderModel remoteHeader = new AdManagerClient.DailyAdExecSummaryRptServicePloxy.HeaderModel();
				DailyAdExecSummaryRptServicePloxy.DailyAdExecSummaryRptModel remoteData = new AdManagerClient.DailyAdExecSummaryRptServicePloxy.DailyAdExecSummaryRptModel();

				// 헤더정보 셋트
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;
				remoteHeader.UserLevel     = Header.UserLevel;

				// 호출정보 셋트
				remoteData.LogDay1 = dailyAdExecSummaryRptModel.LogDay1;
				
				// 웹서비스 호출 타임아웃설정
				svc.Timeout = FrameSystem.m_SystemTimeout;  

				// 웹서비스 메소드 호출
				remoteData = svc.GetDailyAdExecSummary(remoteHeader, remoteData);

				// 결과코드검사
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// 결과 셋트
				dailyAdExecSummaryRptModel.ReportDataSet = remoteData.ReportDataSet.Copy();
				dailyAdExecSummaryRptModel.ItemDataSet = remoteData.ItemDataSet.Copy();
				dailyAdExecSummaryRptModel.ResultCnt = remoteData.ResultCnt;
				dailyAdExecSummaryRptModel.ResultCD = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("일간광고집행종합 조회 End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":GetDailyAdExecSummaryRptList():" + fe.ErrCode + ":" + fe.ResultMsg);
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
