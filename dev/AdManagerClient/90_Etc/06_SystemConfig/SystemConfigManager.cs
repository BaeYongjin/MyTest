// ===============================================================================
//
// SystemConfigManager.cs
//
// 메뉴관리 서비스를 호출합니다. 
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
	/// 환경설정 웹서비스를 호출합니다. 
	/// </summary>
	public class SystemConfigManager : BaseManager
	{
		/// <summary>
		/// 생성자
		/// </summary>
		/// <param name="systemModel"></param>
		/// <param name="commonModel"></param>

		public SystemConfigManager(SystemModel systemModel, CommonModel commonModel) : base(systemModel, commonModel)
		{
			_log = FrameSystem.oLog;
			_Host  = FrameSystem.m_WebServer_Host;
			_Port  = FrameSystem.m_WebServer_Port;
			_Path  = FrameSystem.m_WebServer_App + "/Common/SystemConfigService.asmx";
		}

		/// <summary>
		/// 환경설정 목록 조회
		/// </summary>
		/// <param name="baseModel"></param>
		public void GetSystemConfigList(SystemConfigModel systemConfigModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("환경설정조회 Start");
				_log.Debug("-----------------------------------------");

				// 웹서비스 인스턴스 생성
				SystemConfigServicePloxy.SystemConfigService svc = new SystemConfigServicePloxy.SystemConfigService();
				svc.Url = _WebServiceUrl;
			
				// 리모트 모델 생성
				SystemConfigServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.SystemConfigServicePloxy.HeaderModel();
				SystemConfigServicePloxy.SystemConfigModel     remoteData   = new AdManagerClient.SystemConfigServicePloxy.SystemConfigModel();

				// 헤더정보 셋트
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;
				remoteHeader.UserLevel     = Header.UserLevel;
				remoteHeader.UserClass	   = Header.UserClass;
			
				// 웹서비스 호출 타임아웃설정
				svc.Timeout = FrameSystem.m_SystemTimeout;
				// 웹서비스 메소드 호출
				remoteData = svc.GetSystemConfigList(remoteHeader, remoteData);

				// 결과코드검사
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// 결과 셋트
				systemConfigModel.SystemConfigDataSet = remoteData.SystemConfigDataSet.Copy();
				systemConfigModel.ResultCnt   = remoteData.ResultCnt;
				systemConfigModel.ResultCD    = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("환경설정조회 End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":GetSystemConfigList():" + fe.ErrCode + ":" + fe.ResultMsg);
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
		/// 환경설정 데이터 저장
		/// </summary>
		/// <param name="systemConfigModel"></param>
		public void SetSystemConfigUpdate(SystemConfigModel systemConfigModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("환경설정 저장 Start");
				_log.Debug("-----------------------------------------");

            				// 웹서비스 인스턴스 생성
				SystemConfigServicePloxy.SystemConfigService svc = new SystemConfigServicePloxy.SystemConfigService();
				svc.Url = _WebServiceUrl;			
				// 리모트 모델 생성
				SystemConfigServicePloxy.HeaderModel		remoteHeader = new AdManagerClient.SystemConfigServicePloxy.HeaderModel();
				SystemConfigServicePloxy.SystemConfigModel	remoteData   = new AdManagerClient.SystemConfigServicePloxy.SystemConfigModel();
            
				remoteHeader.ClientKey		= Header.ClientKey;
				remoteHeader.UserID			= Header.UserID;
				remoteData.FtpUploadID      = systemConfigModel.FtpUploadID;
				remoteData.FtpUploadPW      = systemConfigModel.FtpUploadPW;
				remoteData.FtpUploadHost    = systemConfigModel.FtpUploadHost;
				remoteData.FtpUploadPort    = systemConfigModel.FtpUploadPort;
				remoteData.FtpUploadPath    = systemConfigModel.FtpUploadPath;
				remoteData.FtpMovePath	    = systemConfigModel.FtpMovePath;
				remoteData.FtpMoveUseYn     = systemConfigModel.FtpMoveUseYn;
				remoteData.FtpCdnID         = systemConfigModel.FtpCdnID;
				remoteData.FtpCdnPW         = systemConfigModel.FtpCdnPW;
				remoteData.FtpCdnHost       = systemConfigModel.FtpCdnHost;
				remoteData.FtpCdnPort       = systemConfigModel.FtpCdnPort;
				remoteData.FileQueueUseYn   = systemConfigModel.FileQueueUseYn;
				remoteData.FileQueueInterval= systemConfigModel.FileQueueInterval;
				remoteData.FileQueueCnt     = systemConfigModel.FileQueueCnt;
				remoteData.URLGetAdPopList  = systemConfigModel.URLGetAdPopList;
				remoteData.URLSetAdPop		= systemConfigModel.URLSetAdPop;
				remoteData.CmsMasUrl		= systemConfigModel.CmsMasUrl;
				remoteData.CmsMasQuery		= systemConfigModel.CmsMasQuery;

				// 웹서비스 호출 타임아웃설정
				svc.Timeout = FrameSystem.m_SystemTimeout;
				// 웹서비스 메소드 호출
				remoteData = svc.SetSystemConfigUpdate(remoteHeader, remoteData);
            
				// 결과코드검사
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}
            
				// 결과 셋트
				systemConfigModel.ResultCnt   = remoteData.ResultCnt;
				_log.Debug("systemConfigModel.ResultCnt = "+systemConfigModel.ResultCnt);
            			
				systemConfigModel.ResultCD    = remoteData.ResultCD;
            
				_log.Debug("-----------------------------------------");
				_log.Debug("환경설정 End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				systemConfigModel.ResultCD    = "3101";
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":SetSystemConfigUpdate():" + fe.ErrCode + ":" + fe.ResultMsg);
				_log.Warning("-----------------------------------------");
				throw fe;
			}
			catch(Exception e)
			{
				systemConfigModel.ResultCD    = "3101";
				_log.Error("-----------------------------------------");
				_log.Exception(e);
				_log.Error("-----------------------------------------");
				throw new FrameException(e.Message);
			}
		}
	}
}