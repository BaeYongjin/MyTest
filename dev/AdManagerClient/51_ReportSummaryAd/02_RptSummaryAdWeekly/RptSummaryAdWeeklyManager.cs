// ===============================================================================
//
// RptSummaryAdWeeklyManager.cs
//
// 광고주간레포트 조회 서비스를 호출합니다. 
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
	/// RptSummaryAdDailyManager에 대한 요약 설명입니다.
	/// </summary>
	public class RptSummaryAdWeeklyManager : BaseManager
	{
		public RptSummaryAdWeeklyManager(SystemModel systemModel, CommonModel commonModel) : base(systemModel, commonModel)
		{
			_log = FrameSystem.oLog;
			_module = "RptSummaryAdDaily";
			_Host  = FrameSystem.m_WebServer_Host;
			_Port  = FrameSystem.m_WebServer_Port;
			_Path  = FrameSystem.m_WebServer_App + "/ReportSummaryAd/RptSummaryAdWeeklyService.asmx";
		}

		/// <summary>
		/// 광고주간레포트 조회
		/// </summary>
		/// <param name="RptSummaryAdWeekly"></param>
		public void	GetRptSummaryAdWeeklyList(RptSummaryAdWeeklyModel rptSummaryAdWeeklyModel)
		{

			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("광고주간레포트목록 조회 Start");
				_log.Debug("-----------------------------------------");
				
				// 웹서비스 인스턴스 생성
				RptSummaryAdWeeklyServicePloxy.RptSummaryAdWeeklyService svc = new AdManagerClient.RptSummaryAdWeeklyServicePloxy.RptSummaryAdWeeklyService();
			
				// 웹서비스URL 동적 셋트
				svc.Url = _WebServiceUrl;

				// 리모트 모델 생성
				RptSummaryAdWeeklyServicePloxy.HeaderModel remoteHeader = new AdManagerClient.RptSummaryAdWeeklyServicePloxy.HeaderModel();
				RptSummaryAdWeeklyServicePloxy.RptSummaryAdWeeklyModel remoteData = new AdManagerClient.RptSummaryAdWeeklyServicePloxy.RptSummaryAdWeeklyModel();
				

				// 헤더정보 셋트
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;
				remoteHeader.UserLevel     = Header.UserLevel;

				// 호출정보 셋트
				remoteData.SearchStartDay = rptSummaryAdWeeklyModel.SearchStartDay;
				remoteData.SearchDay = rptSummaryAdWeeklyModel.SearchDay;
				
				// 웹서비스 호출 타임아웃설정
				svc.Timeout = 1000 * 60;

				// 웹서비스 메소드 호출
				remoteData = svc.GetRptSummaryAdWeeklyList(remoteHeader, remoteData);
				
				// 결과코드검사
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// 결과 셋트
				rptSummaryAdWeeklyModel.RptWeeklyDataSet = remoteData.RptWeeklyDataSet.Copy();
				rptSummaryAdWeeklyModel.ResultCnt = remoteData.ResultCnt;
				rptSummaryAdWeeklyModel.ResultCD = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("광고주간레포트목록 조회 End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":GetRptSummaryAdWeeklyList():" + fe.ErrCode + ":" + fe.ResultMsg);
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
