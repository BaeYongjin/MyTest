// ===============================================================================
//
// StatisticsPgCategoryManager.cs
//
// 컨텐츠리포트 카테고리통계 조회 서비스를 호출합니다. 
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
	/// 컨텐츠리포트 카테고리통계 조회 웹서비스를 호출합니다. 
	/// </summary>
	public class StatisticsPgCategoryManager : BaseManager
	{
		/// <summary>
		/// 생성자
		/// </summary>
		/// <param name="systemModel"></param>
		/// <param name="commonModel"></param>

		public StatisticsPgCategoryManager(SystemModel systemModel, CommonModel commonModel) : base(systemModel, commonModel)
		{
			_log = FrameSystem.oLog;
			_module = "StatisticsPgCategory";
			_Host  = FrameSystem.m_WebServer_Host;
			_Port  = FrameSystem.m_WebServer_Port;
			_Path  = FrameSystem.m_WebServer_App + "/ReportMedia/StatisticsPgCategoryService.asmx";
		}

		/// <summary>
		/// 컨텐츠리포트 카테고리통계 조회
		/// </summary>
		/// <param name="userModel"></param>
		public void GetStatisticsPgCategoryReport(StatisticsPgCategoryModel statisticsPgCategoryModel)
		{

			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("컨텐츠리포트 카테고리통계 조회 Start");
				_log.Debug("-----------------------------------------");
				
				// 웹서비스 인스턴스 생성
				StatisticsPgCategoryServicePloxy.StatisticsPgCategoryService svc = new StatisticsPgCategoryServicePloxy.StatisticsPgCategoryService();
			
				// 웹서비스URL 동적 셋트
				svc.Url = _WebServiceUrl;

				// 리모트 모델 생성
				StatisticsPgCategoryServicePloxy.HeaderModel      remoteHeader = new AdManagerClient.StatisticsPgCategoryServicePloxy.HeaderModel();
				StatisticsPgCategoryServicePloxy.StatisticsPgCategoryModel   remoteData   = new AdManagerClient.StatisticsPgCategoryServicePloxy.StatisticsPgCategoryModel();

				// 헤더정보 셋트
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;
				remoteHeader.UserLevel     = Header.UserLevel;

				// 호출정보 셋트
				remoteData.SearchMediaCode       = statisticsPgCategoryModel.SearchMediaCode;	  
				remoteData.SearchType            = statisticsPgCategoryModel.SearchType;       
				remoteData.SearchStartDay        = statisticsPgCategoryModel.SearchStartDay;       
				remoteData.SearchEndDay          = statisticsPgCategoryModel.SearchEndDay;       
				
                // 웹서비스 호출 타임아웃설정
                svc.Timeout = FrameSystem.m_SystemTimeout * 20;   // 작업시간이 길다...

				// 웹서비스 메소드 호출
				remoteData = svc.GetStatisticsPgCategory(remoteHeader, remoteData);

				// 결과코드검사
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// 결과 셋트
				statisticsPgCategoryModel.ReportDataSet = remoteData.ReportDataSet.Copy();
				statisticsPgCategoryModel.ResultCnt     = remoteData.ResultCnt;
				statisticsPgCategoryModel.ResultCD      = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("컨텐츠리포트 카테고리통계 조회 End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":GetStatisticsPgCategoryReport():" + fe.ErrCode + ":" + fe.ResultMsg);
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
