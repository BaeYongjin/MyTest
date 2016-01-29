// ===============================================================================
//
// RptAdCategorySummaryManager.cs
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
	/// RptAdCategorySummaryManager에 대한 요약 설명입니다.
	/// </summary>
	public class RptAdCategorySummaryManager : BaseManager
	{
		public RptAdCategorySummaryManager(SystemModel systemModel, CommonModel commonModel) : base(systemModel, commonModel)
		{
			_log = FrameSystem.oLog;
			_module = "DailyAdExecSummaryRpt";
			_Host  = FrameSystem.m_WebServer_Host;
			_Port  = FrameSystem.m_WebServer_Port;
			_Path  = FrameSystem.m_WebServer_App + "/ReportSummaryAd/RptAdCategorySummaryService.asmx";
		}

		/// <summary>
		/// 광고일간레포트 조회
		/// </summary>
		/// <param name="DailyAdExecSummaryRpt"></param>
		public void	GetRptAdCategorySummary(RptAdCategorySummaryModel rptAdCategorySummaryModel)
		{

			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("일간광고집행종합 조회 Start");
				_log.Debug("-----------------------------------------");
				
				// 웹서비스 인스턴스 생성
				RptAdCategorySummaryServicePloxy.RptAdCategorySummaryService svc = new AdManagerClient.RptAdCategorySummaryServicePloxy.RptAdCategorySummaryService();
			
				// 웹서비스URL 동적 셋트
				svc.Url = _WebServiceUrl;

				// 리모트 모델 생성
				RptAdCategorySummaryServicePloxy.HeaderModel remoteHeader = new AdManagerClient.RptAdCategorySummaryServicePloxy.HeaderModel();
				RptAdCategorySummaryServicePloxy.RptAdCategorySummaryModel remoteData = new AdManagerClient.RptAdCategorySummaryServicePloxy.RptAdCategorySummaryModel();

				// 헤더정보 셋트
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;
				remoteHeader.UserLevel     = Header.UserLevel;

				// 호출정보 셋트
				remoteData.LogDay = rptAdCategorySummaryModel.LogDay;
				remoteData.LogDayEnd = rptAdCategorySummaryModel.LogDayEnd;
				remoteData.AdType = rptAdCategorySummaryModel.AdType;
				
				// 웹서비스 호출 타임아웃설정
				svc.Timeout = FrameSystem.m_SystemTimeout;  

				// 웹서비스 메소드 호출
				remoteData = svc.GetRptAdCategorySummary(remoteHeader, remoteData);

				// 결과코드검사
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// 결과 셋트
				rptAdCategorySummaryModel.ReportDataSet = remoteData.ReportDataSet.Copy();
				rptAdCategorySummaryModel.ItemDataSet = remoteData.ItemDataSet.Copy();
				rptAdCategorySummaryModel.ResultCnt = remoteData.ResultCnt;
				rptAdCategorySummaryModel.ResultCD = remoteData.ResultCD;

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
