// ===============================================================================
// AdFile Manager  for Charites Project
//
// AdStatusManager.cs
//
// 광고파일배포배포 서비스를 호출합니다. 
//
// ===============================================================================
// Release history
//
// ===============================================================================
// Copyright (C) 2006 G-Inno Systems Inc.
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
	/// 광고파일배포배포 웹서비스를 호출합니다. 
	/// </summary>
	public class AdStatusManager : BaseManager
	{
		/// <summary>
		/// 생성자
		/// </summary>
		/// <param name="systemModel"></param>
		/// <param name="commonModel"></param>

		public AdStatusManager(SystemModel systemModel, CommonModel commonModel) : base(systemModel, commonModel)
		{
			_log = FrameSystem.oLog;
			_module = "AdStatus";
			_Host  = FrameSystem.m_WebServer_Host;
			_Port  = FrameSystem.m_WebServer_Port;
			_Path  = FrameSystem.m_WebServer_App + "/Schedule/AdStatusService.asmx";
		}

		/// <summary>
		/// 광고파일배포배포조회
		/// </summary>
		/// <param name="userModel"></param>
		public void GetAdStatusList(AdStatusModel adStatusModel)
		{

			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("광고파일배포목록조회 Start");
				_log.Debug("-----------------------------------------");
				
				// 웹서비스 인스턴스 생성
				AdStatusServicePloxy.AdStatusService svc = new AdStatusServicePloxy.AdStatusService();
			
				// 웹서비스URL 동적 셋트
				svc.Url = _WebServiceUrl;

				// 리모트 모델 생성
				AdStatusServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.AdStatusServicePloxy.HeaderModel();
				AdStatusServicePloxy.AdStatusModel   remoteData   = new AdManagerClient.AdStatusServicePloxy.AdStatusModel();

				// 헤더정보 셋트
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;
				remoteHeader.UserLevel     = Header.UserLevel;

				// 호출정보 셋트
				remoteData.SearchKey             = adStatusModel.SearchKey;
				remoteData.SearchMediaCode		 = adStatusModel.SearchMediaCode;	  				
				remoteData.SearchchkAdState_10	 = adStatusModel.SearchchkAdState_10; 
				remoteData.SearchchkAdState_20	 = adStatusModel.SearchchkAdState_20; 
				remoteData.SearchchkAdState_30	 = adStatusModel.SearchchkAdState_30; 
				remoteData.SearchchkAdState_40	 = adStatusModel.SearchchkAdState_40; 
				remoteData.SearchFileState_20	 = adStatusModel.SearchFileState_20;				
				remoteData.SearchFileState_30	 = adStatusModel.SearchFileState_30;				
				remoteData.SearchFileState_90	 = adStatusModel.SearchFileState_90;				
				
                // 웹서비스 호출 타임아웃설정
                svc.Timeout = FrameSystem.m_SystemTimeout * 10;
				// 웹서비스 메소드 호출
                _log.Debug("서비스호출시작");
                remoteData = svc.GetAdStatusList_Comp(remoteHeader, remoteData);
                _log.Debug("서비스호출끝");

				// 결과코드검사
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}
                

				// 결과 셋트
				//adStatusModel.AdStatusDataSet = remoteData.AdStatusDataSet.Copy();
                adStatusModel.AdStatusDataSet = FrameSystem.DeCompressData(Convert.FromBase64String(remoteData.FileName));
				adStatusModel.ResultCnt   = remoteData.ResultCnt;
				adStatusModel.ResultCD    = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("광고파일배포목록조회 End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":GetAdStatusList():" + fe.ErrCode + ":" + fe.ResultMsg);
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