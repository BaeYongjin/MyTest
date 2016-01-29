// ===============================================================================
//
// TimeSummaryManager.cs
//
// 광고리포트 일별통계 조회 서비스를 호출합니다. 
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
	/// 광고리포트 일별통계 조회 웹서비스를 호출합니다. 
	/// </summary>
	public class TimeSummaryManager : BaseManager
	{
		/// <summary>
		/// 생성자
		/// </summary>
		/// <param name="systemModel"></param>
		/// <param name="commonModel"></param>

		public TimeSummaryManager(SystemModel systemModel, CommonModel commonModel) : base(systemModel, commonModel)
		{
			_log = FrameSystem.oLog;
			_module = "StatisticsDaily";
			_Host  = FrameSystem.m_WebServer_Host;
			_Port  = FrameSystem.m_WebServer_Port;
			_Path  = FrameSystem.m_WebServer_App + "/ReportAd/TimeSummaryService.asmx";
		}

		/// <summary>
		/// 지역별 시간통계 조회
		/// </summary>
		/// <param name="userModel"></param>
		public void GetAreaTime(TimeSummaryModel timeSummaryModel)
		{

			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("지역별 시간통계 조회 Start");
				_log.Debug("-----------------------------------------");
				
				// 웹서비스 인스턴스 생성
				TimeSummaryServicePloxy.TimeSummaryService svc = new TimeSummaryServicePloxy.TimeSummaryService();
			
				// 웹서비스URL 동적 셋트
				svc.Url = _WebServiceUrl;

				// 리모트 모델 생성
				TimeSummaryServicePloxy.HeaderModel      remoteHeader = new AdManagerClient.TimeSummaryServicePloxy.HeaderModel();
				TimeSummaryServicePloxy.TimeSummaryModel   remoteData   = new AdManagerClient.TimeSummaryServicePloxy.TimeSummaryModel();

				// 헤더정보 셋트
				remoteHeader.ClientKey  = Header.ClientKey;
				remoteHeader.UserID     = Header.UserID;
				remoteHeader.UserLevel  = Header.UserLevel;

				// 호출정보 셋트
				remoteData.StartDay     = timeSummaryModel.StartDay;       
				remoteData.EndDay       = timeSummaryModel.EndDay;       
                remoteData.AdList       = timeSummaryModel.AdList.ToArray();
				
				// 웹서비스 호출 타임아웃설정
				svc.Timeout = FrameSystem.m_SystemTimeout * 20;   // 작업시간이 길다...

				// 웹서비스 메소드 호출
				remoteData = svc.GetAreaTime(remoteHeader, remoteData);

				// 결과코드검사
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// 결과 셋트				
				timeSummaryModel.ReportDataSet = remoteData.ReportDataSet.Copy();
				timeSummaryModel.ResultCnt     = remoteData.ResultCnt;
				timeSummaryModel.ResultCD      = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("지역별 시간통계 조회 End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":GetAreaTime():" + fe.ErrCode + ":" + fe.ResultMsg);
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
		/// 일자별 시간통계 조회
		/// </summary>
		/// <param name="userModel"></param>
		public void GetDateTime(TimeSummaryModel timeSummaryModel)
		{

			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("일자별 시간통계 조회 Start");
				_log.Debug("-----------------------------------------");
				
				// 웹서비스 인스턴스 생성
				TimeSummaryServicePloxy.TimeSummaryService svc = new TimeSummaryServicePloxy.TimeSummaryService();
			
				// 웹서비스URL 동적 셋트
				svc.Url = _WebServiceUrl;

				// 리모트 모델 생성
				TimeSummaryServicePloxy.HeaderModel      remoteHeader = new AdManagerClient.TimeSummaryServicePloxy.HeaderModel();
				TimeSummaryServicePloxy.TimeSummaryModel   remoteData   = new AdManagerClient.TimeSummaryServicePloxy.TimeSummaryModel();

				// 헤더정보 셋트
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;
				remoteHeader.UserLevel     = Header.UserLevel;

				// 호출정보 셋트				  
 
				remoteData.StartDay     = timeSummaryModel.StartDay;       
				remoteData.EndDay       = timeSummaryModel.EndDay;       
                remoteData.AdList       = timeSummaryModel.AdList.ToArray();
				
				// 웹서비스 호출 타임아웃설정
				svc.Timeout = FrameSystem.m_SystemTimeout * 20;   // 작업시간이 길다...

				// 웹서비스 메소드 호출
				remoteData = svc.GetDateTime(remoteHeader, remoteData);

				// 결과코드검사
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// 결과 셋트				
				timeSummaryModel.ReportDataSet = remoteData.ReportDataSet.Copy();
				timeSummaryModel.ResultCnt     = remoteData.ResultCnt;
				timeSummaryModel.ResultCD      = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("일자별 시간통계 조회 End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":GetDateTime():" + fe.ErrCode + ":" + fe.ResultMsg);
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
		/// 장르별 요일통계 조회
		/// </summary>
		/// <param name="userModel"></param>
		public void GetGenreTime(TimeSummaryModel timeSummaryModel)
		{

			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("장르별 시간통계 조회 Start");
				_log.Debug("-----------------------------------------");
				
				// 웹서비스 인스턴스 생성
				TimeSummaryServicePloxy.TimeSummaryService svc = new TimeSummaryServicePloxy.TimeSummaryService();
			
				// 웹서비스URL 동적 셋트
				svc.Url = _WebServiceUrl;

				// 리모트 모델 생성
				TimeSummaryServicePloxy.HeaderModel      remoteHeader = new AdManagerClient.TimeSummaryServicePloxy.HeaderModel();
				TimeSummaryServicePloxy.TimeSummaryModel   remoteData   = new AdManagerClient.TimeSummaryServicePloxy.TimeSummaryModel();

				// 헤더정보 셋트
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;
				remoteHeader.UserLevel     = Header.UserLevel;

				// 호출정보 셋트				  
				remoteData.StartDay        = timeSummaryModel.StartDay;       
				remoteData.EndDay          = timeSummaryModel.EndDay;       
                remoteData.AdList       = timeSummaryModel.AdList.ToArray();
				
				// 웹서비스 호출 타임아웃설정
				svc.Timeout = FrameSystem.m_SystemTimeout * 20;   // 작업시간이 길다...

				// 웹서비스 메소드 호출
				remoteData = svc.GetGenreTime(remoteHeader, remoteData);

				// 결과코드검사
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// 결과 셋트				
				timeSummaryModel.ReportDataSet = remoteData.ReportDataSet.Copy();
				timeSummaryModel.ResultCnt     = remoteData.ResultCnt;
				timeSummaryModel.ResultCD      = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("장르별 시간통계 조회 End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":GetGenreTime():" + fe.ErrCode + ":" + fe.ResultMsg);
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
		/// 장르별 시간통계 조회
		/// </summary>
		/// <param name="userModel"></param>
		public void GetChannelTime(TimeSummaryModel timeSummaryModel)
		{

			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("채널별 시간통계 조회 Start");
				_log.Debug("-----------------------------------------");
				
				// 웹서비스 인스턴스 생성
				TimeSummaryServicePloxy.TimeSummaryService svc = new TimeSummaryServicePloxy.TimeSummaryService();
			
				// 웹서비스URL 동적 셋트
				svc.Url = _WebServiceUrl;

				// 리모트 모델 생성
				TimeSummaryServicePloxy.HeaderModel      remoteHeader = new AdManagerClient.TimeSummaryServicePloxy.HeaderModel();
				TimeSummaryServicePloxy.TimeSummaryModel   remoteData   = new AdManagerClient.TimeSummaryServicePloxy.TimeSummaryModel();

				// 헤더정보 셋트
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;
				remoteHeader.UserLevel     = Header.UserLevel;

				// 호출정보 셋트				  
				remoteData.StartDay        = timeSummaryModel.StartDay;       
				remoteData.EndDay          = timeSummaryModel.EndDay;       
                remoteData.AdList       = timeSummaryModel.AdList.ToArray();
				
				// 웹서비스 호출 타임아웃설정
				svc.Timeout = FrameSystem.m_SystemTimeout * 20;   // 작업시간이 길다...

				// 웹서비스 메소드 호출
				remoteData = svc.GetChannelTime(remoteHeader, remoteData);

				// 결과코드검사
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// 결과 셋트				
				timeSummaryModel.ReportDataSet = remoteData.ReportDataSet.Copy();
				timeSummaryModel.ResultCnt     = remoteData.ResultCnt;
				timeSummaryModel.ResultCD      = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("채널별 시간통계 조회 End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":GetChannelTime():" + fe.ErrCode + ":" + fe.ResultMsg);
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
