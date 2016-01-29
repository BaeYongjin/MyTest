// ===============================================================================
//
// StatisticsPgAgeManager.cs
//
// 컨텐츠리포트 연령별통계 조회 서비스를 호출합니다. 
//
// ===============================================================================
// Release history
//
// ===============================================================================
// Copyright (C) 2007 G-Inno Systems Inc.
// All rights reserved.
// ==============================================================================

using System;

using WinFramework.Base;

using AdManagerModel;
using WinFramework.Misc;

namespace AdManagerClient
{
	/// <summary>
	/// 컨텐츠리포트 연령별통계 조회 웹서비스를 호출합니다. 
	/// </summary>
	public class StatisticsPgAgeManager : BaseManager
	{
		/// <summary>
		/// 생성자
		/// </summary>
		/// <param name="systemModel"></param>
		/// <param name="commonModel"></param>

		public StatisticsPgAgeManager(SystemModel systemModel, CommonModel commonModel, String kind) : base(systemModel, commonModel)
		{
			_log = FrameSystem.oLog;
			_Host  = FrameSystem.m_WebServer_Host;
			_Port  = FrameSystem.m_WebServer_Port;

            if ("Age".Equals(kind))
            {
                _module = "StatisticsPg";
                _Path = FrameSystem.m_WebServer_App + "/ReportMedia/StatisticsPgService.asmx";
            }
            else
            {
                _module = "StatisticsPgWeek";
                _Path = FrameSystem.m_WebServer_App + "/ReportMedia/StatisticsPgWeekService.asmx";
            }
		}

        public StatisticsPgAgeManager(SystemModel systemModel, CommonModel commonModel) : base(systemModel, commonModel)
        {
            _log = FrameSystem.oLog;
            _module = "StatisticsPgWeek";
            _Host = FrameSystem.m_WebServer_Host;
            _Port = FrameSystem.m_WebServer_Port;
            _Path = FrameSystem.m_WebServer_App + "/ReportMedia/StatisticsPgWeekService.asmx";
        }

		/// <summary>
		/// 카테고리목록 조회
		/// </summary>
		/// <param name="userModel"></param>
		public void GetCategoryList(StatisticsPgWeekModel statisticsPgWeekModel)
		{

			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("카테고리목록 조회 Start");
				_log.Debug("-----------------------------------------");
				
				// 웹서비스 인스턴스 생성
				StatisticsPgWeekServicePloxy.StatisticsPgWeekService svc = new StatisticsPgWeekServicePloxy.StatisticsPgWeekService();
			
				// 웹서비스URL 동적 셋트
				svc.Url = _WebServiceUrl;

				// 리모트 모델 생성
				StatisticsPgWeekServicePloxy.HeaderModel      remoteHeader = new AdManagerClient.StatisticsPgWeekServicePloxy.HeaderModel();
				StatisticsPgWeekServicePloxy.StatisticsPgWeekModel   remoteData   = new AdManagerClient.StatisticsPgWeekServicePloxy.StatisticsPgWeekModel();

				// 헤더정보 셋트
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;
				remoteHeader.UserLevel     = Header.UserLevel;

				// 호출정보 셋트
				remoteData.SearchMediaCode       = statisticsPgWeekModel.SearchMediaCode;	  
				
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
				statisticsPgWeekModel.CategoryDataSet = remoteData.CategoryDataSet.Copy();
				statisticsPgWeekModel.ResultCnt       = remoteData.ResultCnt;
				statisticsPgWeekModel.ResultCD        = remoteData.ResultCD;

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
		public void GetGenreList(StatisticsPgWeekModel statisticsPgWeekModel)
		{

			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("장르목록 조회 Start");
				_log.Debug("-----------------------------------------");
				
				// 웹서비스 인스턴스 생성
				StatisticsPgWeekServicePloxy.StatisticsPgWeekService svc = new StatisticsPgWeekServicePloxy.StatisticsPgWeekService();
			
				// 웹서비스URL 동적 셋트
				svc.Url = _WebServiceUrl;

				// 리모트 모델 생성
				StatisticsPgWeekServicePloxy.HeaderModel      remoteHeader = new AdManagerClient.StatisticsPgWeekServicePloxy.HeaderModel();
				StatisticsPgWeekServicePloxy.StatisticsPgWeekModel   remoteData   = new AdManagerClient.StatisticsPgWeekServicePloxy.StatisticsPgWeekModel();

				// 헤더정보 셋트
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;
				remoteHeader.UserLevel     = Header.UserLevel;

				// 호출정보 셋트
				remoteData.SearchMediaCode       = statisticsPgWeekModel.SearchMediaCode;	  
				
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
				statisticsPgWeekModel.GenreDataSet = remoteData.GenreDataSet.Copy();
				statisticsPgWeekModel.ResultCnt    = remoteData.ResultCnt;
				statisticsPgWeekModel.ResultCD     = remoteData.ResultCD;

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
		/// 컨텐츠리포트 연령별통계 조회
		/// </summary>
		/// <param name="userModel"></param>
		public void GetStatisticsPgAgeReport(StatisticsPgModel statisticsPgModel)
		{

			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("컨텐츠리포트 연령별통계 조회 Start");
				_log.Debug("-----------------------------------------");
				
				// 웹서비스 인스턴스 생성
                StatisticsPgServicePloxy.StatisticsPgService svc = new StatisticsPgServicePloxy.StatisticsPgService();
			
				// 웹서비스URL 동적 셋트
				svc.Url = _WebServiceUrl;

				// 리모트 모델 생성
                StatisticsPgServicePloxy.HeaderModel remoteHeader = new AdManagerClient.StatisticsPgServicePloxy.HeaderModel();
                StatisticsPgServicePloxy.StatisticsPgModel remoteData = new AdManagerClient.StatisticsPgServicePloxy.StatisticsPgModel();

				// 헤더정보 셋트
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;
				remoteHeader.UserLevel     = Header.UserLevel;

				// 호출정보 셋트
				remoteData.SearchMediaCode       = statisticsPgModel.SearchMediaCode;	  
				remoteData.SearchCategoryCode    = statisticsPgModel.SearchCategoryCode;	  
				remoteData.SearchGenreCode       = statisticsPgModel.SearchGenreCode;	  
				remoteData.SearchKey             = statisticsPgModel.SearchKey;	  
				remoteData.SearchType            = statisticsPgModel.SearchType;       
				remoteData.SearchStartDay        = statisticsPgModel.SearchStartDay;       
				remoteData.SearchEndDay          = statisticsPgModel.SearchEndDay;       
				
                // 웹서비스 호출 타임아웃설정
                svc.Timeout = FrameSystem.m_SystemTimeout * 20;   // 작업시간이 길다...

				// 웹서비스 메소드 호출
				remoteData = svc.GetStatisticsPgAge(remoteHeader, remoteData);

				// 결과코드검사
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// 결과 셋트
				statisticsPgModel.ReportDataSet = remoteData.ReportDataSet.Copy();
				statisticsPgModel.ResultCnt     = remoteData.ResultCnt;
				statisticsPgModel.ResultCD      = remoteData.ResultCD;

                statisticsPgModel.ResultCnt = Utility.GetDatasetCount(statisticsPgModel.ReportDataSet);

				_log.Debug("-----------------------------------------");
				_log.Debug("컨텐츠리포트 연령별통계 조회 End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":GetStatisticsPgAgeReport():" + fe.ErrCode + ":" + fe.ResultMsg);
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
		/// 컨텐츠리포트 연령별통계 조회
		/// </summary>
		/// <param name="userModel"></param>
		public void GetStatisticsPgAgeReportAVG(StatisticsPgModel statisticsPgModel)
		{

			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("컨텐츠리포트 연령별통계 조회 Start");
				_log.Debug("-----------------------------------------");
				
				// 웹서비스 인스턴스 생성
                StatisticsPgServicePloxy.StatisticsPgService svc = new StatisticsPgServicePloxy.StatisticsPgService();
			
				// 웹서비스URL 동적 셋트
				svc.Url = _WebServiceUrl;

				// 리모트 모델 생성
                StatisticsPgServicePloxy.HeaderModel remoteHeader = new AdManagerClient.StatisticsPgServicePloxy.HeaderModel();
                StatisticsPgServicePloxy.StatisticsPgModel remoteData = new AdManagerClient.StatisticsPgServicePloxy.StatisticsPgModel();

				// 헤더정보 셋트
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;
				remoteHeader.UserLevel     = Header.UserLevel;

				// 호출정보 셋트
				remoteData.SearchMediaCode       = statisticsPgModel.SearchMediaCode;	  
				remoteData.SearchCategoryCode    = statisticsPgModel.SearchCategoryCode;	  
				remoteData.SearchGenreCode       = statisticsPgModel.SearchGenreCode;	  
				remoteData.SearchKey             = statisticsPgModel.SearchKey;	  
				remoteData.SearchType            = statisticsPgModel.SearchType;       
				remoteData.SearchStartDay        = statisticsPgModel.SearchStartDay;       
				remoteData.SearchEndDay          = statisticsPgModel.SearchEndDay;       
				
				// 웹서비스 호출 타임아웃설정
				svc.Timeout = FrameSystem.m_SystemTimeout * 20;   // 작업시간이 길다...

				// 웹서비스 메소드 호출
				remoteData = svc.GetStatisticsPgAgeAVG(remoteHeader, remoteData);

				// 결과코드검사
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// 결과 셋트
				statisticsPgModel.ReportDataSet = remoteData.ReportDataSet.Copy();
				statisticsPgModel.ResultCnt     = remoteData.ResultCnt;
				statisticsPgModel.ResultCD      = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("컨텐츠리포트 연령별통계 조회 End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":GetStatisticsPgAgeReport():" + fe.ErrCode + ":" + fe.ResultMsg);
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
