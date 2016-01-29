// ===============================================================================
//
// StatisticsPgRegionManager.cs
//
// 컨텐츠리포트 지역별통계 조회 서비스를 호출합니다. 
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
	/// 컨텐츠리포트 지역별통계 조회 웹서비스를 호출합니다. 
	/// </summary>
	public class StatisticsPgRegionManager : BaseManager
	{
		/// <summary>
		/// 생성자
		/// </summary>
		/// <param name="systemModel"></param>
		/// <param name="commonModel"></param>

		public StatisticsPgRegionManager(SystemModel systemModel, CommonModel commonModel) : base(systemModel, commonModel)
		{
			_log = FrameSystem.oLog;
			_module = "StatisticsPgRegion";
			_Host  = FrameSystem.m_WebServer_Host;
			_Port  = FrameSystem.m_WebServer_Port;
			_Path  = FrameSystem.m_WebServer_App + "/ReportMedia/StatisticsPgRegionService.asmx";
		}

		/// <summary>
		/// 카테고리목록 조회
		/// </summary>
		/// <param name="userModel"></param>
		public void GetCategoryList(StatisticsPgRegionModel statisticsPgRegionModel)
		{

			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("카테고리목록 조회 Start");
				_log.Debug("-----------------------------------------");
				
				// 웹서비스 인스턴스 생성
				StatisticsPgRegionServicePloxy.StatisticsPgRegionService svc = new StatisticsPgRegionServicePloxy.StatisticsPgRegionService();
			
				// 웹서비스URL 동적 셋트
				svc.Url = _WebServiceUrl;

				// 리모트 모델 생성
				StatisticsPgRegionServicePloxy.HeaderModel      remoteHeader = new AdManagerClient.StatisticsPgRegionServicePloxy.HeaderModel();
				StatisticsPgRegionServicePloxy.StatisticsPgRegionModel   remoteData   = new AdManagerClient.StatisticsPgRegionServicePloxy.StatisticsPgRegionModel();

				// 헤더정보 셋트
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;
				remoteHeader.UserLevel     = Header.UserLevel;

				// 호출정보 셋트
				remoteData.SearchMediaCode       = statisticsPgRegionModel.SearchMediaCode;	  
				
				// 웹서비스 호출 타임아웃설정
				svc.Timeout = FrameSystem.m_SystemTimeout;   // 작업시간이 길다...

				// 웹서비스 메소드 호출
				remoteData = svc.GetCategoryList(remoteHeader, remoteData);

				// 결과코드검사
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// 결과 셋트
				statisticsPgRegionModel.CategoryDataSet = remoteData.CategoryDataSet.Copy();
				statisticsPgRegionModel.ResultCnt       = remoteData.ResultCnt;
				statisticsPgRegionModel.ResultCD        = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("카테고리목록 조회 End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":GetCategoryList():" + fe.ErrCode + ":" + fe.ResultMsg);
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
		/// 장르목록 조회
		/// </summary>
		/// <param name="userModel"></param>
		public void GetGenreList(StatisticsPgRegionModel statisticsPgRegionModel)
		{

			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("장르목록 조회 Start");
				_log.Debug("-----------------------------------------");
				
				// 웹서비스 인스턴스 생성
				StatisticsPgRegionServicePloxy.StatisticsPgRegionService svc = new StatisticsPgRegionServicePloxy.StatisticsPgRegionService();
			
				// 웹서비스URL 동적 셋트
				svc.Url = _WebServiceUrl;

				// 리모트 모델 생성
				StatisticsPgRegionServicePloxy.HeaderModel      remoteHeader = new AdManagerClient.StatisticsPgRegionServicePloxy.HeaderModel();
				StatisticsPgRegionServicePloxy.StatisticsPgRegionModel   remoteData   = new AdManagerClient.StatisticsPgRegionServicePloxy.StatisticsPgRegionModel();

				// 헤더정보 셋트
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;
				remoteHeader.UserLevel     = Header.UserLevel;

				// 호출정보 셋트
				remoteData.SearchMediaCode       = statisticsPgRegionModel.SearchMediaCode;	  
				
				// 웹서비스 호출 타임아웃설정
				svc.Timeout = FrameSystem.m_SystemTimeout;   // 작업시간이 길다...

				// 웹서비스 메소드 호출
				remoteData = svc.GetGenreList(remoteHeader, remoteData);

				// 결과코드검사
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// 결과 셋트
				statisticsPgRegionModel.GenreDataSet = remoteData.GenreDataSet.Copy();
				statisticsPgRegionModel.ResultCnt    = remoteData.ResultCnt;
				statisticsPgRegionModel.ResultCD     = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("장르목록 조회 End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":GetGenreList():" + fe.ErrCode + ":" + fe.ResultMsg);
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
		/// 컨텐츠리포트 지역별통계 조회
		/// </summary>
		/// <param name="userModel"></param>
		public void GetStatisticsPgRegionReport(StatisticsPgRegionModel statisticsPgRegionModel)
		{

			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("컨텐츠리포트 지역별통계 조회 Start");
				_log.Debug("-----------------------------------------");
				
				// 웹서비스 인스턴스 생성
				StatisticsPgRegionServicePloxy.StatisticsPgRegionService svc = new StatisticsPgRegionServicePloxy.StatisticsPgRegionService();
			
				// 웹서비스URL 동적 셋트
				svc.Url = _WebServiceUrl;

				// 리모트 모델 생성
				StatisticsPgRegionServicePloxy.HeaderModel      remoteHeader = new AdManagerClient.StatisticsPgRegionServicePloxy.HeaderModel();
				StatisticsPgRegionServicePloxy.StatisticsPgRegionModel   remoteData   = new AdManagerClient.StatisticsPgRegionServicePloxy.StatisticsPgRegionModel();

				// 헤더정보 셋트
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;
				remoteHeader.UserLevel     = Header.UserLevel;

				// 호출정보 셋트
				remoteData.SearchMediaCode       = statisticsPgRegionModel.SearchMediaCode;	  
				remoteData.SearchCategoryCode    = statisticsPgRegionModel.SearchCategoryCode;	  
				remoteData.SearchGenreCode       = statisticsPgRegionModel.SearchGenreCode;	  
				remoteData.SearchKey             = statisticsPgRegionModel.SearchKey;	  
				remoteData.SearchType            = statisticsPgRegionModel.SearchType;       
				remoteData.SearchStartDay        = statisticsPgRegionModel.SearchStartDay;       
				remoteData.SearchEndDay          = statisticsPgRegionModel.SearchEndDay;       
				
                // 웹서비스 호출 타임아웃설정
                svc.Timeout = FrameSystem.m_SystemTimeout * 20;   // 작업시간이 길다...

				// 웹서비스 메소드 호출
				remoteData = svc.GetStatisticsPgRegion(remoteHeader, remoteData);

				// 결과코드검사
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// 결과 셋트
				statisticsPgRegionModel.ReportDataSet = remoteData.ReportDataSet.Copy();
				statisticsPgRegionModel.ResultCnt     = remoteData.ResultCnt;
				statisticsPgRegionModel.ResultCD      = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("컨텐츠리포트 지역별통계 조회 End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":GetStatisticsPgRegionReport():" + fe.ErrCode + ":" + fe.ResultMsg);
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
		/// 컨텐츠리포트 지역별통계 조회
		/// </summary>
		/// <param name="userModel"></param>
		public void GetStatisticsPgRegionReportAVG(StatisticsPgRegionModel statisticsPgRegionModel)
		{

			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("컨텐츠리포트 지역별통계 조회 Start");
				_log.Debug("-----------------------------------------");
				
				// 웹서비스 인스턴스 생성
				StatisticsPgRegionServicePloxy.StatisticsPgRegionService svc = new StatisticsPgRegionServicePloxy.StatisticsPgRegionService();
			
				// 웹서비스URL 동적 셋트
				svc.Url = _WebServiceUrl;

				// 리모트 모델 생성
				StatisticsPgRegionServicePloxy.HeaderModel      remoteHeader = new AdManagerClient.StatisticsPgRegionServicePloxy.HeaderModel();
				StatisticsPgRegionServicePloxy.StatisticsPgRegionModel   remoteData   = new AdManagerClient.StatisticsPgRegionServicePloxy.StatisticsPgRegionModel();

				// 헤더정보 셋트
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;
				remoteHeader.UserLevel     = Header.UserLevel;

				// 호출정보 셋트
				remoteData.SearchMediaCode       = statisticsPgRegionModel.SearchMediaCode;	  
				remoteData.SearchCategoryCode    = statisticsPgRegionModel.SearchCategoryCode;	  
				remoteData.SearchGenreCode       = statisticsPgRegionModel.SearchGenreCode;	  
				remoteData.SearchKey             = statisticsPgRegionModel.SearchKey;	  
				remoteData.SearchType            = statisticsPgRegionModel.SearchType;       
				remoteData.SearchStartDay        = statisticsPgRegionModel.SearchStartDay;       
				remoteData.SearchEndDay          = statisticsPgRegionModel.SearchEndDay;       
				
				// 웹서비스 호출 타임아웃설정
				svc.Timeout = FrameSystem.m_SystemTimeout * 20;   // 작업시간이 길다...

				// 웹서비스 메소드 호출
				remoteData = svc.GetStatisticsPgRegionAVG(remoteHeader, remoteData);

				// 결과코드검사
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// 결과 셋트
				statisticsPgRegionModel.ReportDataSet = remoteData.ReportDataSet.Copy();
				statisticsPgRegionModel.ResultCnt     = remoteData.ResultCnt;
				statisticsPgRegionModel.ResultCD      = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("컨텐츠리포트 지역별통계 조회 End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":GetStatisticsPgRegionReport():" + fe.ErrCode + ":" + fe.ResultMsg);
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
