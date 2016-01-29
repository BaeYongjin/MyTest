// ===============================================================================
//
// SummaryAdManager.cs
//
// 광고 총괄집계 집계 조회 서비스를 호출합니다. 
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
using System.Threading;
using System.Diagnostics;

using WinFramework.Base;
using WinFramework.Data;
using WinFramework.Misc;

using AdManagerModel;

namespace AdManagerClient
{
	/// <summary>
	/// 광고 총괄집계 조회 웹서비스를 호출합니다. 
	/// </summary>
	public class SummaryAdManager : BaseManager
	{
		/// <summary>
		/// 생성자
		/// </summary>
		/// <param name="systemModel"></param>
		/// <param name="commonModel"></param>

		public SummaryAdManager(SystemModel systemModel, CommonModel commonModel) : base(systemModel, commonModel)
		{
			_log = FrameSystem.oLog;
			_module = "SummaryAd";
			_Host  = FrameSystem.m_WebServer_Host;
			_Port  = FrameSystem.m_WebServer_Port;
			_Path  = FrameSystem.m_WebServer_App + "/ReportAd/SummaryAdService.asmx";
		}


		/// <summary>
		/// 광고목록 조회
		/// </summary>
		/// <param name="userModel"></param>
		public void GetContractItemList(SummaryAdModel summaryAdModel)
		{

			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("광고목록 조회 Start");
				_log.Debug("-----------------------------------------");
					
				// 웹서비스 인스턴스 생성
				SummaryAdServicePloxy.SummaryAdService svc = new SummaryAdServicePloxy.SummaryAdService();
				
				// 웹서비스URL 동적 셋트
				svc.Url = _WebServiceUrl;

				// 리모트 모델 생성
				SummaryAdServicePloxy.HeaderModel      remoteHeader = new AdManagerClient.SummaryAdServicePloxy.HeaderModel();
				SummaryAdServicePloxy.SummaryAdModel   remoteData   = new AdManagerClient.SummaryAdServicePloxy.SummaryAdModel();

				// 헤더정보 셋트
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;
				remoteHeader.UserLevel     = Header.UserLevel;

				// 호출정보 셋트
				remoteData.SearchContractSeq     = summaryAdModel.SearchContractSeq;
				remoteData.CampaignCode     = summaryAdModel.CampaignCode;
					
				// 웹서비스 호출 타임아웃설정
				svc.Timeout = FrameSystem.m_SystemTimeout;  
				// 웹서비스 메소드 호출
				remoteData = svc.GetContractItemList(remoteHeader, remoteData);

				// 결과코드검사
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// 결과 셋트
				summaryAdModel.ItemDataSet   = remoteData.ItemDataSet.Copy();
				summaryAdModel.ResultCnt     = remoteData.ResultCnt;
				summaryAdModel.ResultCD      = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("광고목록 조회 End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":GetContractItemList():" + fe.ErrCode + ":" + fe.ResultMsg);
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
		/// 일간 광고 총괄집계 조회
		/// </summary>
		/// <param name="userModel"></param>
		public void GetSummaryAdReport(SummaryAdModel summaryAdModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("광고 총괄집계 조회 Start");
				_log.Debug("-----------------------------------------");
				
				// 웹서비스 인스턴스 생성
				SummaryAdServicePloxy.SummaryAdService svc = new SummaryAdServicePloxy.SummaryAdService();
			
				// 웹서비스URL 동적 셋트
				svc.Url = _WebServiceUrl;

				// 리모트 모델 생성
				SummaryAdServicePloxy.HeaderModel      remoteHeader = new AdManagerClient.SummaryAdServicePloxy.HeaderModel();
				SummaryAdServicePloxy.SummaryAdModel   remoteData   = new AdManagerClient.SummaryAdServicePloxy.SummaryAdModel();

				// 헤더정보 셋트
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;
				remoteHeader.UserLevel     = Header.UserLevel;

				// 호출정보 셋트
                remoteData.MenuLevel            = summaryAdModel.MenuLevel;
                remoteData.SearchMediaCode      = summaryAdModel.SearchMediaCode;
				remoteData.SearchContractSeq    = summaryAdModel.SearchContractSeq;
                remoteData.CampaignCode         = summaryAdModel.CampaignCode;
				remoteData.SearchItemNo         = summaryAdModel.SearchItemNo;
				remoteData.SearchType           = summaryAdModel.SearchType;
				remoteData.SearchStartDay       = summaryAdModel.SearchStartDay;
				remoteData.SearchEndDay         = summaryAdModel.SearchEndDay;

				remoteData.TotalUser = summaryAdModel.TotalUser;
				
                // 웹서비스 호출 타임아웃설정
                svc.Timeout = 1000 * 60 * 30;   // 30분
                
                remoteData = svc.GetSummaryAdDaily(remoteHeader, remoteData);

				#region [ 사용 않함 ]
				//				일간,기간,주간,월간을 개별 함수로 사용했으나, S1고도화 이후 단일함수로 통합함
				//				// 웹서비스 메소드 호출
				//				if(summaryAdModel.SearchType.Equals("C"))
				//				{
				//					remoteData = svc.GetSummaryAdTotality(remoteHeader, remoteData);
				//				}
				//				else if(summaryAdModel.SearchType.Equals("D"))
				//				{
				//					remoteData = svc.GetSummaryAdDaily(remoteHeader, remoteData);
				//				}
				//				else if(summaryAdModel.SearchType.Equals("W"))
				//				{
				//					remoteData = svc.GetSummaryAdWeekly(remoteHeader, remoteData);
				//				}
				//				else if(summaryAdModel.SearchType.Equals("M"))
				//				{
				//					remoteData = svc.GetSummaryAdMonthly(remoteHeader, remoteData);
				//				}
				#endregion

				// 결과코드검사
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// 결과 셋트
				summaryAdModel.TotalUser     = remoteData.TotalUser;
				summaryAdModel.ReportDataSet = remoteData.ReportDataSet.Copy();
				summaryAdModel.ResultCnt     = remoteData.ResultCnt;
				summaryAdModel.ResultCD      = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("광고 시청횟수 집계 조회 End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":GetSummaryAdReport():" + fe.ErrCode + ":" + fe.ResultMsg);
				_log.Warning("-----------------------------------------");
                Debug.WriteLine("오류 : " + DateTime.Now.ToString());
				throw fe;
			}
			catch(Exception e)
			{
				_log.Error("-----------------------------------------");
				_log.Exception(e);
				_log.Error("-----------------------------------------");
                Debug.WriteLine("오류 : " + DateTime.Now.ToString());
				throw new FrameException(e.Message);
			}
            Debug.WriteLine("종료 : " + DateTime.Now.ToString());
		}
	}
}
