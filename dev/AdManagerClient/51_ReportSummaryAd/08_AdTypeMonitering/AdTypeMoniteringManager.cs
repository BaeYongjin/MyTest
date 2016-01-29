// ===============================================================================
//
// AdTypeMoniteringManager.cs
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
	/// AdTypeMoniteringManager에 대한 요약 설명입니다.
	/// </summary>
	public class AdTypeMoniteringManager : BaseManager
	{
		public AdTypeMoniteringManager(SystemModel systemModel, CommonModel commonModel) : base(systemModel, commonModel)
		{
			_log = FrameSystem.oLog;
			_module = "AdTypeMoniteringService";
			_Host  = FrameSystem.m_WebServer_Host;
			_Port  = FrameSystem.m_WebServer_Port;
			_Path  = FrameSystem.m_WebServer_App + "/ReportSummaryAd/AdTypeMoniteringService.asmx";
		}

		/// <summary>
		/// 광고집행모니터링 마스터 조회
		/// </summary>
		/// <param name="DailyAdExecSummaryRpt"></param>
		public void GetAdTypeMaster(AdTypeMoniteringModel adTypeMoniteringModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("광고집행모니터링 마스터 조회 Start");
				_log.Debug("-----------------------------------------");

				// 웹서비스 인스턴스 생성
				AdTypeMoniteringServicePloxy.AdTypeMoniteringService svc = new AdManagerClient.AdTypeMoniteringServicePloxy.AdTypeMoniteringService();

				// 웹서비스URL 동적 셋트
				svc.Url = _WebServiceUrl;

				// 리모트 모델 생성
				AdTypeMoniteringServicePloxy.HeaderModel remoteHeader = new AdManagerClient.AdTypeMoniteringServicePloxy.HeaderModel();
				AdTypeMoniteringServicePloxy.AdTypeMoniteringModel remoteData = new AdManagerClient.AdTypeMoniteringServicePloxy.AdTypeMoniteringModel();

				// 헤더정보 셋트
				remoteHeader.ClientKey = Header.ClientKey;
				remoteHeader.UserID = Header.UserID;
				remoteHeader.UserLevel = Header.UserLevel;

				// 호출정보 셋트
				remoteData.LogDay = adTypeMoniteringModel.LogDay;

				// 웹서비스 호출 타임아웃설정
				svc.Timeout = FrameSystem.m_SystemTimeout;

				// 웹서비스 메소드 호출
				remoteData = svc.GetAdTypeMaster(remoteHeader, remoteData);

				// 결과코드검사
				if (!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// 결과 셋트
				adTypeMoniteringModel.ReportDataSet = remoteData.ReportDataSet.Copy();
				adTypeMoniteringModel.ItemDataSet = remoteData.ItemDataSet.Copy();
				adTypeMoniteringModel.ResultCnt = remoteData.ResultCnt;
				adTypeMoniteringModel.ResultCD = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("광고집행모니터링 마스터 조회 End");
				_log.Debug("-----------------------------------------");
			}
			catch (FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning(this.ToString() + ":GetAdTypeMaster():" + fe.ErrCode + ":" + fe.ResultMsg);
				_log.Warning("-----------------------------------------");
				throw fe;
			}
			catch (Exception e)
			{
				_log.Error("-----------------------------------------");
				_log.Exception(e);
				_log.Error("-----------------------------------------");
				throw new FrameException(e.Message);
			}
		}

		/// <summary>
		/// 광고집행모니터링 디테일 조회
		/// </summary>
		/// <param name="DailyAdExecSummaryRpt"></param>
		public void	GetAdTypeDetail(AdTypeMoniteringModel adTypeMoniteringModel)
		{

			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("광고집행모니터링 디테일 조회 Start");
				_log.Debug("-----------------------------------------");
				
				// 웹서비스 인스턴스 생성
				AdTypeMoniteringServicePloxy.AdTypeMoniteringService svc = new AdManagerClient.AdTypeMoniteringServicePloxy.AdTypeMoniteringService();
			
				// 웹서비스URL 동적 셋트
				svc.Url = _WebServiceUrl;

				// 리모트 모델 생성
				AdTypeMoniteringServicePloxy.HeaderModel remoteHeader = new AdManagerClient.AdTypeMoniteringServicePloxy.HeaderModel();
				AdTypeMoniteringServicePloxy.AdTypeMoniteringModel remoteData = new AdManagerClient.AdTypeMoniteringServicePloxy.AdTypeMoniteringModel();

				// 헤더정보 셋트
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;
				remoteHeader.UserLevel     = Header.UserLevel;

				// 호출정보 셋트
				remoteData.LogDay   = adTypeMoniteringModel.LogDay;
				remoteData.AdType   = adTypeMoniteringModel.AdType;
				remoteData.Rap      = adTypeMoniteringModel.Rap;

				// 웹서비스 호출 타임아웃설정
				svc.Timeout = FrameSystem.m_SystemTimeout;  

				// 웹서비스 메소드 호출
				remoteData = svc.GetAdTypeDetail(remoteHeader, remoteData);

				// 결과코드검사
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// 결과 셋트
				adTypeMoniteringModel.ReportDataSet = remoteData.ReportDataSet.Copy();
				adTypeMoniteringModel.ItemDataSet = remoteData.ItemDataSet.Copy();
				adTypeMoniteringModel.ResultCnt = remoteData.ResultCnt;
				adTypeMoniteringModel.ResultCD = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("광고집행모니터링 디테일 조회 End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":GetAdTypeDetail():" + fe.ErrCode + ":" + fe.ResultMsg);
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
