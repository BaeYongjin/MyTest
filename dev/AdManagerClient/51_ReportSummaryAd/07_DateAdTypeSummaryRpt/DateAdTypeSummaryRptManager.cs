// ===============================================================================
//
// DateAdTypeSummaryRptManager.cs
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
	/// DateAdTypeSummaryRptManager에 대한 요약 설명입니다.
	/// </summary>
	public class DateAdTypeSummaryRptManager : BaseManager
	{
		public DateAdTypeSummaryRptManager(SystemModel systemModel, CommonModel commonModel) : base(systemModel, commonModel)
		{
			_log = FrameSystem.oLog;
			_module = "DailyAdExecSummaryRpt";
			_Host  = FrameSystem.m_WebServer_Host;
			_Port  = FrameSystem.m_WebServer_Port;
			_Path  = FrameSystem.m_WebServer_App + "/ReportSummaryAd/DateAdTypeSummaryRptService.asmx";
		}

		/// <summary>
		/// 일별 광고종류별 조회
		/// </summary>
		/// <param name="DailyAdExecSummaryRpt"></param>
		public void	GetDateAdTypeSummaryRpt(DateAdTypeSummaryRptModel dateAdTypeSummaryRptModel)
		{

			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("일별 광고종류별 조회 Start");
				_log.Debug("-----------------------------------------");
				
				// 웹서비스 인스턴스 생성
				DateAdTypeSummaryRptServicePloxy.DateAdTypeSummaryRptService svc = new AdManagerClient.DateAdTypeSummaryRptServicePloxy.DateAdTypeSummaryRptService();
			
				// 웹서비스URL 동적 셋트
				svc.Url = _WebServiceUrl;

				// 리모트 모델 생성
				DateAdTypeSummaryRptServicePloxy.HeaderModel remoteHeader = new AdManagerClient.DateAdTypeSummaryRptServicePloxy.HeaderModel();
				DateAdTypeSummaryRptServicePloxy.DateAdTypeSummaryRptModel remoteData = new AdManagerClient.DateAdTypeSummaryRptServicePloxy.DateAdTypeSummaryRptModel();

				// 헤더정보 셋트
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;
				remoteHeader.UserLevel     = Header.UserLevel;

				// 호출정보 셋트
				remoteData.LogDay1 = dateAdTypeSummaryRptModel.LogDay1;
				remoteData.LogDay2 = dateAdTypeSummaryRptModel.LogDay2;
				
				// 웹서비스 호출 타임아웃설정
				svc.Timeout = FrameSystem.m_SystemTimeout;  

				// 웹서비스 메소드 호출
				remoteData = svc.GetDateAdTypeSummaryRpt(remoteHeader, remoteData);

				// 결과코드검사
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// 결과 셋트
				dateAdTypeSummaryRptModel.ReportDataSet = remoteData.ReportDataSet.Copy();
				dateAdTypeSummaryRptModel.ItemDataSet = remoteData.ItemDataSet.Copy();
				dateAdTypeSummaryRptModel.ResultCnt = remoteData.ResultCnt;
				dateAdTypeSummaryRptModel.ResultCD = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("일별 광고종류별 조회 End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":GetDateAdTypeSummaryRpt():" + fe.ErrCode + ":" + fe.ResultMsg);
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
		/// 주별 광고종류별 조회
		/// </summary>
		/// <param name="DailyAdExecSummaryRpt"></param>
		public void	GetWeeklyAdTypeSummaryRpt(DateAdTypeSummaryRptModel dateAdTypeSummaryRptModel)
		{

			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("주별 광고종류별 조회 Start");
				_log.Debug("-----------------------------------------");
				
				// 웹서비스 인스턴스 생성
				DateAdTypeSummaryRptServicePloxy.DateAdTypeSummaryRptService svc = new AdManagerClient.DateAdTypeSummaryRptServicePloxy.DateAdTypeSummaryRptService();
			
				// 웹서비스URL 동적 셋트
				svc.Url = _WebServiceUrl;

				// 리모트 모델 생성
				DateAdTypeSummaryRptServicePloxy.HeaderModel remoteHeader = new AdManagerClient.DateAdTypeSummaryRptServicePloxy.HeaderModel();
				DateAdTypeSummaryRptServicePloxy.DateAdTypeSummaryRptModel remoteData = new AdManagerClient.DateAdTypeSummaryRptServicePloxy.DateAdTypeSummaryRptModel();

				// 헤더정보 셋트
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;
				remoteHeader.UserLevel     = Header.UserLevel;

				// 호출정보 셋트
				remoteData.LogDay1 = dateAdTypeSummaryRptModel.LogDay1;
				remoteData.LogDay2 = dateAdTypeSummaryRptModel.LogDay2;
				
				// 웹서비스 호출 타임아웃설정
				svc.Timeout = FrameSystem.m_SystemTimeout;  

				// 웹서비스 메소드 호출
				remoteData = svc.GetWeeklyAdTypeSummaryRpt(remoteHeader, remoteData);

				// 결과코드검사
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// 결과 셋트
				dateAdTypeSummaryRptModel.ReportDataSet = remoteData.ReportDataSet.Copy();
				dateAdTypeSummaryRptModel.ItemDataSet = remoteData.ItemDataSet.Copy();
				dateAdTypeSummaryRptModel.ResultCnt = remoteData.ResultCnt;
				dateAdTypeSummaryRptModel.ResultCD = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("주별 광고종류별 조회 End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":GetDateAdTypeSummaryRpt():" + fe.ErrCode + ":" + fe.ResultMsg);
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
		/// 월별 광고종류별 조회
		/// </summary>
		/// <param name="DailyAdExecSummaryRpt"></param>
		public void	GetMonthlyAdTypeSummaryRpt(DateAdTypeSummaryRptModel dateAdTypeSummaryRptModel)
		{

			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("월별 광고종류별 조회 Start");
				_log.Debug("-----------------------------------------");
				
				// 웹서비스 인스턴스 생성
				DateAdTypeSummaryRptServicePloxy.DateAdTypeSummaryRptService svc = new AdManagerClient.DateAdTypeSummaryRptServicePloxy.DateAdTypeSummaryRptService();
			
				// 웹서비스URL 동적 셋트
				svc.Url = _WebServiceUrl;

				// 리모트 모델 생성
				DateAdTypeSummaryRptServicePloxy.HeaderModel remoteHeader = new AdManagerClient.DateAdTypeSummaryRptServicePloxy.HeaderModel();
				DateAdTypeSummaryRptServicePloxy.DateAdTypeSummaryRptModel remoteData = new AdManagerClient.DateAdTypeSummaryRptServicePloxy.DateAdTypeSummaryRptModel();

				// 헤더정보 셋트
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;
				remoteHeader.UserLevel     = Header.UserLevel;

				// 호출정보 셋트
				remoteData.LogDay1 = dateAdTypeSummaryRptModel.LogDay1;
				remoteData.LogDay2 = dateAdTypeSummaryRptModel.LogDay2;
				
				// 웹서비스 호출 타임아웃설정
				svc.Timeout = FrameSystem.m_SystemTimeout;  

				// 웹서비스 메소드 호출
				remoteData = svc.GetMonthlyAdTypeSummaryRpt(remoteHeader, remoteData);

				// 결과코드검사
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// 결과 셋트
				dateAdTypeSummaryRptModel.ReportDataSet = remoteData.ReportDataSet.Copy();
				dateAdTypeSummaryRptModel.ItemDataSet = remoteData.ItemDataSet.Copy();
				dateAdTypeSummaryRptModel.ResultCnt = remoteData.ResultCnt;
				dateAdTypeSummaryRptModel.ResultCD = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("월별 광고종류별 조회 End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":GetDateAdTypeSummaryRpt():" + fe.ErrCode + ":" + fe.ResultMsg);
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
