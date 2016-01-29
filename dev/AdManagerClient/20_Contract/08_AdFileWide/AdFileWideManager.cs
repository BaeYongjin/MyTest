// ===============================================================================
// AdFile Manager  for Charites Project
//
// AdFileWideManager.cs
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
	public class AdFileWideManager : BaseManager
	{
		/// <summary>
		/// 생성자
		/// </summary>
		/// <param name="systemModel"></param>
		/// <param name="commonModel"></param>

		public AdFileWideManager(SystemModel systemModel, CommonModel commonModel) : base(systemModel, commonModel)
		{
			_log = FrameSystem.oLog;
			_module = "AdFileWide";
			_Host  = FrameSystem.m_WebServer_Host;
			_Port  = FrameSystem.m_WebServer_Port;
			_Path  = FrameSystem.m_WebServer_App + "/Contract/AdFileWideService.asmx";
		}

		/// <summary>
		/// 광고파일건수 조회
		/// </summary>
		/// <param name="userModel"></param>
		public void GetFileCount(AdFileWideModel adFileWideModel)
		{

			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("광고파일건수 조회 Start");
				_log.Debug("-----------------------------------------");
				
				// 웹서비스 인스턴스 생성
				AdFileWideServicePloxy.AdFileWideService svc = new AdFileWideServicePloxy.AdFileWideService();
			
				// 웹서비스URL 동적 셋트
				svc.Url = _WebServiceUrl;

				// 리모트 모델 생성
				AdFileWideServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.AdFileWideServicePloxy.HeaderModel();
				AdFileWideServicePloxy.AdFileWideModel   remoteData   = new AdManagerClient.AdFileWideServicePloxy.AdFileWideModel();

				// 헤더정보 셋트
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;
				remoteHeader.UserLevel     = Header.UserLevel;

				// 호출정보 셋트
				remoteData.SearchKey             = adFileWideModel.SearchKey;
				remoteData.SearchMediaCode		 = adFileWideModel.SearchMediaCode;	  
				remoteData.SearchchkAdState_10	 = adFileWideModel.SearchchkAdState_10; 
				remoteData.SearchchkAdState_20	 = adFileWideModel.SearchchkAdState_20; 
				remoteData.SearchchkAdState_30	 = adFileWideModel.SearchchkAdState_30; 
				remoteData.SearchchkAdState_40	 = adFileWideModel.SearchchkAdState_40; 
                
				// 웹서비스 호출 타임아웃설정
				svc.Timeout = FrameSystem.m_SystemTimeout;
				// 웹서비스 메소드 호출
				remoteData = svc.GetFileCount(remoteHeader, remoteData);

				// 결과코드검사
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// 결과 셋트

				adFileWideModel.FileListCount = remoteData.FileListCount;  // 2007.10.01 RH.Jung 파일리스트건수검사

				adFileWideModel.CountDataSet = remoteData.CountDataSet.Copy();
				adFileWideModel.ResultCnt    = remoteData.ResultCnt;
				adFileWideModel.ResultCD     = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("광고파일건수 조회 End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":GetAdFileWideList():" + fe.ErrCode + ":" + fe.ResultMsg);
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
		/// 광고파일배포조회
		/// </summary>
		/// <param name="userModel"></param>
		public void GetAdFileWideList(AdFileWideModel adFileWideModel)
		{

			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("광고파일배포목록조회 Start");
				_log.Debug("-----------------------------------------");
				
				// 웹서비스 인스턴스 생성
				AdFileWideServicePloxy.AdFileWideService svc = new AdFileWideServicePloxy.AdFileWideService();
			
				// 웹서비스URL 동적 셋트
				svc.Url = _WebServiceUrl;

				// 리모트 모델 생성
				AdFileWideServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.AdFileWideServicePloxy.HeaderModel();
				AdFileWideServicePloxy.AdFileWideModel   remoteData   = new AdManagerClient.AdFileWideServicePloxy.AdFileWideModel();

				// 헤더정보 셋트
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;
				remoteHeader.UserLevel     = Header.UserLevel;

				// 호출정보 셋트
				remoteData.SearchMediaCode		= adFileWideModel.SearchMediaCode;	  
				remoteData.SearchKey			= adFileWideModel.SearchKey;
				remoteData.SearchFileState		= adFileWideModel.SearchFileState;				
				remoteData.SearchchkAdState_10	= adFileWideModel.SearchchkAdState_10; 
				remoteData.SearchchkAdState_20	= adFileWideModel.SearchchkAdState_20; 
				remoteData.SearchchkAdState_30	= adFileWideModel.SearchchkAdState_30; 
				remoteData.SearchchkAdState_40	= adFileWideModel.SearchchkAdState_40; 
                
				// 웹서비스 호출 타임아웃설정
				svc.Timeout = FrameSystem.m_SystemTimeout;
				// 웹서비스 메소드 호출
				remoteData = svc.GetAdFileWideList(remoteHeader, remoteData);

				// 결과코드검사
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// 결과 셋트
				adFileWideModel.FileDataSet = remoteData.FileDataSet.Copy();
				adFileWideModel.ResultCnt   = remoteData.ResultCnt;
				adFileWideModel.ResultCD    = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("광고파일배포목록조회 End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":GetAdFileWideList():" + fe.ErrCode + ":" + fe.ResultMsg);
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
		/// 광고파일 편성현황 조회
		/// </summary>
		/// <param name="userModel"></param>
		public void GetAdFileSchedule(AdFileWideModel adFileWideModel)
		{

			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug(" 광고파일 편성현황 조회 Start");
				_log.Debug("-----------------------------------------");
				
				// 웹서비스 인스턴스 생성
				AdFileWideServicePloxy.AdFileWideService svc = new AdFileWideServicePloxy.AdFileWideService();
			
				// 웹서비스URL 동적 셋트
				svc.Url = _WebServiceUrl;

				// 리모트 모델 생성
				AdFileWideServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.AdFileWideServicePloxy.HeaderModel();
				AdFileWideServicePloxy.AdFileWideModel   remoteData   = new AdManagerClient.AdFileWideServicePloxy.AdFileWideModel();

				// 헤더정보 셋트
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;
				remoteHeader.UserLevel     = Header.UserLevel;

				// 호출정보 셋트
				remoteData.SearchMediaCode		= adFileWideModel.SearchMediaCode;	  
				remoteData.ItemNo			= adFileWideModel.ItemNo;
                
				// 웹서비스 호출 타임아웃설정
				svc.Timeout = FrameSystem.m_SystemTimeout;
				// 웹서비스 메소드 호출
				remoteData = svc.GetAdFileSchedule(remoteHeader, remoteData);

				// 결과코드검사
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// 결과 셋트
				adFileWideModel.ScheduleDataSet = remoteData.ScheduleDataSet.Copy();
				adFileWideModel.ResultCnt		= remoteData.ResultCnt;
				adFileWideModel.ResultCD		= remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug(" 광고파일 편성현황 조회 End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":GetAdFileSchedule():" + fe.ErrCode + ":" + fe.ResultMsg);
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
		/// 광고파일 검수요청
		/// </summary>
		/// <param name="userModel"></param>
		public void SetAdFileChkRequest(AdFileWideModel adFileWideModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("광고파일 검수요청 Start");
				_log.Debug("-----------------------------------------");

				//입력데이터의 Validation 검사
				if(adFileWideModel.ItemNo.Length < 1) 
				{
					throw new FrameException("광고내역이 선택되지 않았습니다.");
				}
				
				// 웹서비스 인스턴스 생성
				AdFileWideServicePloxy.AdFileWideService svc = new AdFileWideServicePloxy.AdFileWideService();

				// 웹서비스URL 동적 셋트
				svc.Url = _WebServiceUrl;
			
				// 리모트 모델 생성
				AdFileWideServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.AdFileWideServicePloxy.HeaderModel();
				AdFileWideServicePloxy.AdFileWideModel    remoteData   = new AdManagerClient.AdFileWideServicePloxy.AdFileWideModel();

				// 헤더정보 셋트
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;
				remoteHeader.UserLevel     = Header.UserLevel;

				// 호출정보셋트
				remoteData.MediaCode       = adFileWideModel.MediaCode;
				remoteData.ItemNo	       = adFileWideModel.ItemNo;
				remoteData.ItemName	       = adFileWideModel.ItemName;
				remoteData.FileName	       = adFileWideModel.FileName;
             
				// 웹서비스 호출 타임아웃설정
				svc.Timeout = FrameSystem.m_SystemTimeout;
				// 웹서비스 메소드 호출
				remoteData = svc.SetAdFileChkRequest(remoteHeader, remoteData);

				// 결과코드검사
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// 결과 셋트
				adFileWideModel.ResultCnt   = remoteData.ResultCnt;
				adFileWideModel.ResultCD    = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("광고파일 검수요청 End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":SetAdFileChkRequest():" + fe.ErrCode + ":" + fe.ResultMsg);
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
		/// 광고파일 검수요청 취소
		/// </summary>
		/// <param name="userModel"></param>
		public void SetAdFileChkRequestCancel(AdFileWideModel adFileWideModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("광고파일 검수요청 Start");
				_log.Debug("-----------------------------------------");

				//입력데이터의 Validation 검사
				if(adFileWideModel.ItemNo.Length < 1) 
				{
					throw new FrameException("광고내역이 선택되지 않았습니다.");
				}
				
				// 웹서비스 인스턴스 생성
				AdFileWideServicePloxy.AdFileWideService svc = new AdFileWideServicePloxy.AdFileWideService();

				// 웹서비스URL 동적 셋트
				svc.Url = _WebServiceUrl;
			
				// 리모트 모델 생성
				AdFileWideServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.AdFileWideServicePloxy.HeaderModel();
				AdFileWideServicePloxy.AdFileWideModel    remoteData   = new AdManagerClient.AdFileWideServicePloxy.AdFileWideModel();

				// 헤더정보 셋트
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;
				remoteHeader.UserLevel     = Header.UserLevel;

				// 호출정보셋트
				remoteData.MediaCode       = adFileWideModel.MediaCode;
				remoteData.ItemNo	       = adFileWideModel.ItemNo;
				remoteData.ItemName	       = adFileWideModel.ItemName;
				remoteData.FileName	       = adFileWideModel.FileName;
             
				// 웹서비스 호출 타임아웃설정
				svc.Timeout = FrameSystem.m_SystemTimeout;
				// 웹서비스 메소드 호출
				remoteData = svc.SetAdFileChkRequestCancel(remoteHeader, remoteData);

				// 결과코드검사
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// 결과 셋트
				adFileWideModel.ResultCnt   = remoteData.ResultCnt;
				adFileWideModel.ResultCD    = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("광고파일 검수요청 취소 End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":SetAdFileChkRequestCancel():" + fe.ErrCode + ":" + fe.ResultMsg);
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
		/// CMS연동 정보 생성(사용않함, 검수완료에 통합됨)
		/// </summary>
		/// <param name="adFileWideModel"></param>
		public void SetCmsRequestBegin(AdFileWideModel adFileWideModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("광고파일 CMS연동 Start");
				_log.Debug("-----------------------------------------");

				//입력데이터의 Validation 검사
				if(adFileWideModel.ItemNo.Length < 1) 
				{
					throw new FrameException("광고내역이 선택되지 않았습니다.");
				}
				
				// 웹서비스 인스턴스 생성
				AdFileWideServicePloxy.AdFileWideService svc = new AdFileWideServicePloxy.AdFileWideService();

				// 웹서비스URL 동적 셋트
				svc.Url = _WebServiceUrl;
			
				// 리모트 모델 생성
				AdFileWideServicePloxy.HeaderModel		remoteHeader = new AdManagerClient.AdFileWideServicePloxy.HeaderModel();
				AdFileWideServicePloxy.AdFileWideModel  remoteData   = new AdManagerClient.AdFileWideServicePloxy.AdFileWideModel();

				// 헤더정보 셋트
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;
				remoteHeader.UserLevel     = Header.UserLevel;

				// 호출정보셋트
				remoteData.MediaCode       = adFileWideModel.MediaCode;
				remoteData.ItemNo	       = adFileWideModel.ItemNo;
				remoteData.ItemName	       = adFileWideModel.ItemName;
				remoteData.FileName	       = adFileWideModel.FileName;

				remoteData.FilePath			= adFileWideModel.FilePath;
				remoteData.CmsCid			= adFileWideModel.CmsCid;
				remoteData.CmsCmd			= adFileWideModel.CmsCmd;
				remoteData.CmsRequestStatus	= adFileWideModel.CmsRequestStatus;
				remoteData.CmsProcessStatus	= adFileWideModel.CmsProcessStatus;
				remoteData.CmsSyncCount		= adFileWideModel.CmsSyncCount;
				remoteData.CmsDescCount		= adFileWideModel.CmsDescCount;
				
             
				// 웹서비스 호출 타임아웃설정
				svc.Timeout = FrameSystem.m_SystemTimeout;
				// 웹서비스 메소드 호출
				remoteData = svc.SetCmsRequestBegin(remoteHeader, remoteData);

				// 결과코드검사
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// 결과 셋트
				adFileWideModel.ResultCnt   = remoteData.ResultCnt;
				adFileWideModel.ResultCD    = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("광고파일 CMS연동 End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":SetAdFileChkComplete():" + fe.ErrCode + ":" + fe.ResultMsg);
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
		/// 광고파일 검수완료
		/// </summary>
		/// <param name="userModel"></param>
		public void SetAdFileChkComplete(AdFileWideModel adFileWideModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("광고파일 검수완료 Start");
				_log.Debug("-----------------------------------------");

				//입력데이터의 Validation 검사
				if(adFileWideModel.ItemNo.Length < 1) 
				{
					throw new FrameException("광고내역이 선택되지 않았습니다.");
				}
				
				// 웹서비스 인스턴스 생성
				AdFileWideServicePloxy.AdFileWideService svc = new AdFileWideServicePloxy.AdFileWideService();

				// 웹서비스URL 동적 셋트
				svc.Url = _WebServiceUrl;
			
				// 리모트 모델 생성
				AdFileWideServicePloxy.HeaderModel		remoteHeader = new AdManagerClient.AdFileWideServicePloxy.HeaderModel();
				AdFileWideServicePloxy.AdFileWideModel  remoteData   = new AdManagerClient.AdFileWideServicePloxy.AdFileWideModel();

				// 헤더정보 셋트
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;
				remoteHeader.UserLevel     = Header.UserLevel;

				// 호출정보셋트
				remoteData.MediaCode       = adFileWideModel.MediaCode;
				remoteData.ItemNo	       = adFileWideModel.ItemNo;
				remoteData.ItemName	       = adFileWideModel.ItemName;
				remoteData.FileName	       = adFileWideModel.FileName;

				// CMS호출정보 셋트
				remoteData.FilePath			= adFileWideModel.FilePath;
				remoteData.CmsCid			= adFileWideModel.CmsCid;
				remoteData.CmsCmd			= adFileWideModel.CmsCmd;
				remoteData.CmsRequestStatus	= adFileWideModel.CmsRequestStatus;
				remoteData.CmsProcessStatus	= adFileWideModel.CmsProcessStatus;
				remoteData.CmsSyncCount		= adFileWideModel.CmsSyncCount;
				remoteData.CmsDescCount		= adFileWideModel.CmsDescCount;
             
				// 웹서비스 호출 타임아웃설정
				svc.Timeout = FrameSystem.m_SystemTimeout;
				// 웹서비스 메소드 호출
				remoteData = svc.SetAdFileChkComplete(remoteHeader, remoteData);

				// 결과코드검사
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// 결과 셋트
				adFileWideModel.ResultCnt   = remoteData.ResultCnt;
				adFileWideModel.ResultCD    = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("광고파일 검수완료 End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":SetAdFileChkComplete():" + fe.ErrCode + ":" + fe.ResultMsg);
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
		/// 광고파일 검수완료 취소
		/// </summary>
		/// <param name="userModel"></param>
		public void SetAdFileChkCompleteCancel(AdFileWideModel adFileWideModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("광고파일 검수완료 취소 Start");
				_log.Debug("-----------------------------------------");

				//입력데이터의 Validation 검사
				if(adFileWideModel.ItemNo.Length < 1) 
				{
					throw new FrameException("광고내역이 선택되지 않았습니다.");
				}
				
				// 웹서비스 인스턴스 생성
				AdFileWideServicePloxy.AdFileWideService svc = new AdFileWideServicePloxy.AdFileWideService();

				// 웹서비스URL 동적 셋트
				svc.Url = _WebServiceUrl;
			
				// 리모트 모델 생성
				AdFileWideServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.AdFileWideServicePloxy.HeaderModel();
				AdFileWideServicePloxy.AdFileWideModel    remoteData   = new AdManagerClient.AdFileWideServicePloxy.AdFileWideModel();

				// 헤더정보 셋트
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;
				remoteHeader.UserLevel     = Header.UserLevel;

				// 호출정보셋트
				remoteData.MediaCode       = adFileWideModel.MediaCode;
				remoteData.ItemNo	       = adFileWideModel.ItemNo;
				remoteData.ItemName	       = adFileWideModel.ItemName;
				remoteData.FileName	       = adFileWideModel.FileName;
             
				// 웹서비스 호출 타임아웃설정
				svc.Timeout = FrameSystem.m_SystemTimeout;
				// 웹서비스 메소드 호출
				remoteData = svc.SetAdFileChkCompleteCancel(remoteHeader, remoteData);

				// 결과코드검사
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// 결과 셋트
				adFileWideModel.ResultCnt   = remoteData.ResultCnt;
				adFileWideModel.ResultCD    = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("광고파일 검수완료 취소 End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":SetAdFileChkCompleteCancel():" + fe.ErrCode + ":" + fe.ResultMsg);
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
		/// 광고파일 CDN동기확인
		/// </summary>
		/// <param name="userModel"></param>
		public void SetAdFileCDNSync(AdFileWideModel adFileWideModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("광고파일 CDN동기확인 Start");
				_log.Debug("-----------------------------------------");

				//입력데이터의 Validation 검사
				if(adFileWideModel.ItemNo.Length < 1) 
				{
					throw new FrameException("광고내역이 선택되지 않았습니다.");
				}
				
				// 웹서비스 인스턴스 생성
				AdFileWideServicePloxy.AdFileWideService svc = new AdFileWideServicePloxy.AdFileWideService();

				// 웹서비스URL 동적 셋트
				svc.Url = _WebServiceUrl;
			
				// 리모트 모델 생성
				AdFileWideServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.AdFileWideServicePloxy.HeaderModel();
				AdFileWideServicePloxy.AdFileWideModel    remoteData   = new AdManagerClient.AdFileWideServicePloxy.AdFileWideModel();

				// 헤더정보 셋트
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;
				remoteHeader.UserLevel     = Header.UserLevel;

				// 호출정보셋트
				remoteData.MediaCode       = adFileWideModel.MediaCode;
				remoteData.ItemNo	       = adFileWideModel.ItemNo;
				remoteData.ItemName	       = adFileWideModel.ItemName;
				remoteData.FileName	       = adFileWideModel.FileName;
             
				// 웹서비스 호출 타임아웃설정
				svc.Timeout = FrameSystem.m_SystemTimeout;
				// 웹서비스 메소드 호출
				remoteData = svc.SetAdFileCDNSync(remoteHeader, remoteData);

				// 결과코드검사
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// 결과 셋트
				adFileWideModel.ResultCnt   = remoteData.ResultCnt;
				adFileWideModel.ResultCD    = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("광고파일 CDN동기확인 End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":SetAdFileCDNSync():" + fe.ErrCode + ":" + fe.ResultMsg);
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
		/// 광고파일 CDN동기확인 취소
		/// </summary>
		/// <param name="userModel"></param>
		public void SetAdFileCDNSyncCancel(AdFileWideModel adFileWideModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("광고파일 CDN동기확인 취소 Start");
				_log.Debug("-----------------------------------------");

				//입력데이터의 Validation 검사
				if(adFileWideModel.ItemNo.Length < 1) 
				{
					throw new FrameException("광고내역이 선택되지 않았습니다.");
				}
				
				// 웹서비스 인스턴스 생성
				AdFileWideServicePloxy.AdFileWideService svc = new AdFileWideServicePloxy.AdFileWideService();

				// 웹서비스URL 동적 셋트
				svc.Url = _WebServiceUrl;
			
				// 리모트 모델 생성
				AdFileWideServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.AdFileWideServicePloxy.HeaderModel();
				AdFileWideServicePloxy.AdFileWideModel    remoteData   = new AdManagerClient.AdFileWideServicePloxy.AdFileWideModel();

				// 헤더정보 셋트
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;
				remoteHeader.UserLevel     = Header.UserLevel;

				// 호출정보셋트
				remoteData.MediaCode       = adFileWideModel.MediaCode;
				remoteData.ItemNo	       = adFileWideModel.ItemNo;
				remoteData.ItemName	       = adFileWideModel.ItemName;
				remoteData.FileName	       = adFileWideModel.FileName;
             
				// 웹서비스 호출 타임아웃설정
				svc.Timeout = FrameSystem.m_SystemTimeout;
				// 웹서비스 메소드 호출
				remoteData = svc.SetAdFileCDNSyncCancel(remoteHeader, remoteData);

				// 결과코드검사
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// 결과 셋트
				adFileWideModel.ResultCnt   = remoteData.ResultCnt;
				adFileWideModel.ResultCD    = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("광고파일 CDN동기확인 취소 End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":SetAdFileCDNSyncCancel():" + fe.ErrCode + ":" + fe.ResultMsg);
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
		/// 광고파일 CDN배포확인
		/// </summary>
		/// <param name="userModel"></param>
		public void SetAdFileCDNPublish(AdFileWideModel adFileWideModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("광고파일 CDN배포확인 Start");
				_log.Debug("-----------------------------------------");

				//입력데이터의 Validation 검사
				if(adFileWideModel.ItemNo.Length < 1) 
				{
					throw new FrameException("광고내역이 선택되지 않았습니다.");
				}
				
				// 웹서비스 인스턴스 생성
				AdFileWideServicePloxy.AdFileWideService svc = new AdFileWideServicePloxy.AdFileWideService();

				// 웹서비스URL 동적 셋트
				svc.Url = _WebServiceUrl;
			
				// 리모트 모델 생성
				AdFileWideServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.AdFileWideServicePloxy.HeaderModel();
				AdFileWideServicePloxy.AdFileWideModel    remoteData   = new AdManagerClient.AdFileWideServicePloxy.AdFileWideModel();

				// 헤더정보 셋트
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;
				remoteHeader.UserLevel     = Header.UserLevel;

				// 호출정보셋트
				remoteData.MediaCode       = adFileWideModel.MediaCode;
				remoteData.ItemNo	       = adFileWideModel.ItemNo;
				remoteData.ItemName	       = adFileWideModel.ItemName;
				remoteData.FileName	       = adFileWideModel.FileName;
             
				// 웹서비스 호출 타임아웃설정
				svc.Timeout = FrameSystem.m_SystemTimeout;
				// 웹서비스 메소드 호출
				remoteData = svc.SetAdFileCDNPublish(remoteHeader, remoteData);

				// 결과코드검사
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// 결과 셋트
				adFileWideModel.ResultCnt   = remoteData.ResultCnt;
				adFileWideModel.ResultCD    = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("광고파일 CDN배포확인 End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":SetAdFileCDNPublish():" + fe.ErrCode + ":" + fe.ResultMsg);
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
		/// 광고파일 CDN배포확인 취소
		/// </summary>
		/// <param name="userModel"></param>
		public void SetAdFileCDNPublishCancel(AdFileWideModel adFileWideModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("광고파일 CDN배포확인 취소 Start");
				_log.Debug("-----------------------------------------");

				//입력데이터의 Validation 검사
				if(adFileWideModel.ItemNo.Length < 1) 
				{
					throw new FrameException("광고내역이 선택되지 않았습니다.");
				}
				
				// 웹서비스 인스턴스 생성
				AdFileWideServicePloxy.AdFileWideService svc = new AdFileWideServicePloxy.AdFileWideService();

				// 웹서비스URL 동적 셋트
				svc.Url = _WebServiceUrl;
			
				// 리모트 모델 생성
				AdFileWideServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.AdFileWideServicePloxy.HeaderModel();
				AdFileWideServicePloxy.AdFileWideModel    remoteData   = new AdManagerClient.AdFileWideServicePloxy.AdFileWideModel();

				// 헤더정보 셋트
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;
				remoteHeader.UserLevel     = Header.UserLevel;

				// 호출정보셋트
				remoteData.MediaCode       = adFileWideModel.MediaCode;
				remoteData.ItemNo	       = adFileWideModel.ItemNo;
				remoteData.ItemName	       = adFileWideModel.ItemName;
				remoteData.FileName	       = adFileWideModel.FileName;
             
				// 웹서비스 호출 타임아웃설정
				svc.Timeout = FrameSystem.m_SystemTimeout;
				// 웹서비스 메소드 호출
				remoteData = svc.SetAdFileCDNPublishCancel(remoteHeader, remoteData);

				// 결과코드검사
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// 결과 셋트
				adFileWideModel.ResultCnt   = remoteData.ResultCnt;
				adFileWideModel.ResultCD    = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("광고파일 CDN배포확인 취소 End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":SetAdFileCDNPublishCancel():" + fe.ErrCode + ":" + fe.ResultMsg);
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
		/// 광고파일 셋탑삭제
		/// </summary>
		/// <param name="userModel"></param>
		public void SetAdFileSTBDelete(AdFileWideModel adFileWideModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("광고파일 셋탑삭제 Start");
				_log.Debug("-----------------------------------------");

				//입력데이터의 Validation 검사
				if(adFileWideModel.ItemNo.Length < 1) 
				{
					throw new FrameException("광고내역이 선택되지 않았습니다.");
				}				

				// 웹서비스 인스턴스 생성
				AdFileWideServicePloxy.AdFileWideService svc = new AdFileWideServicePloxy.AdFileWideService();

				// 웹서비스URL 동적 셋트
				svc.Url = _WebServiceUrl;
			
				// 리모트 모델 생성
				AdFileWideServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.AdFileWideServicePloxy.HeaderModel();
				AdFileWideServicePloxy.AdFileWideModel    remoteData   = new AdManagerClient.AdFileWideServicePloxy.AdFileWideModel();

				// 헤더정보 셋트
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;
				remoteHeader.UserLevel     = Header.UserLevel;

				// 호출정보셋트
				remoteData.MediaCode       = adFileWideModel.MediaCode;
				remoteData.ItemNo	       = adFileWideModel.ItemNo;
				remoteData.ItemName	       = adFileWideModel.ItemName;
				remoteData.FileName	       = adFileWideModel.FileName;
                
				// 웹서비스 호출 타임아웃설정
				svc.Timeout = FrameSystem.m_SystemTimeout;

				// 웹서비스 메소드 호출
				remoteData = svc.SetAdFileSTBDelete(remoteHeader, remoteData);

				// 결과코드검사
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// 결과 셋트
				adFileWideModel.ResultCnt   = remoteData.ResultCnt;
				adFileWideModel.ResultCD    = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("광고파일 셋탑삭제 End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":SetAdFileSTBDelete():" + fe.ErrCode + ":" + fe.ResultMsg);
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
		/// 광고파일 셋탑삭제 취소
		/// </summary>
		/// <param name="userModel"></param>
		public void SetAdFileSTBDeleteCancel(AdFileWideModel adFileWideModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("광고파일 셋탑삭제 Start");
				_log.Debug("-----------------------------------------");

				//입력데이터의 Validation 검사
				if(adFileWideModel.ItemNo.Length < 1) 
				{
					throw new FrameException("광고내역이 선택되지 않았습니다.");
				}				

				// 웹서비스 인스턴스 생성
				AdFileWideServicePloxy.AdFileWideService svc = new AdFileWideServicePloxy.AdFileWideService();

				// 웹서비스URL 동적 셋트
				svc.Url = _WebServiceUrl;
			
				// 리모트 모델 생성
				AdFileWideServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.AdFileWideServicePloxy.HeaderModel();
				AdFileWideServicePloxy.AdFileWideModel    remoteData   = new AdManagerClient.AdFileWideServicePloxy.AdFileWideModel();

				// 헤더정보 셋트
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;
				remoteHeader.UserLevel     = Header.UserLevel;

				// 호출정보셋트
				remoteData.MediaCode       = adFileWideModel.MediaCode;
				remoteData.ItemNo	       = adFileWideModel.ItemNo;
				remoteData.ItemName	       = adFileWideModel.ItemName;
				remoteData.FileName	       = adFileWideModel.FileName;
                
				// 웹서비스 호출 타임아웃설정
				svc.Timeout = FrameSystem.m_SystemTimeout;

				// 웹서비스 메소드 호출
				remoteData = svc.SetAdFileSTBDeleteCancel(remoteHeader, remoteData);

				// 결과코드검사
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// 결과 셋트
				adFileWideModel.ResultCnt   = remoteData.ResultCnt;
				adFileWideModel.ResultCD    = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("광고파일 셋탑삭제 취소 End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":SetAdFileSTBDeleteCancel():" + fe.ErrCode + ":" + fe.ResultMsg);
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
		/// 광고파일 소재교체
		/// </summary>
		/// <param name="userModel"></param>
		public void SetAdFileChange(AdFileWideModel adFileWideModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("광고파일 소재교체 Start");
				_log.Debug("-----------------------------------------");

				//입력데이터의 Validation 검사
				if(adFileWideModel.ItemNo.Length < 1) 
				{
					throw new FrameException("광고내역이 선택되지 않았습니다.");
				}				

				// 웹서비스 인스턴스 생성
				AdFileWideServicePloxy.AdFileWideService svc = new AdFileWideServicePloxy.AdFileWideService();

				// 웹서비스URL 동적 셋트
				svc.Url = _WebServiceUrl;
			
				// 리모트 모델 생성
				AdFileWideServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.AdFileWideServicePloxy.HeaderModel();
				AdFileWideServicePloxy.AdFileWideModel    remoteData   = new AdManagerClient.AdFileWideServicePloxy.AdFileWideModel();

				// 헤더정보 셋트
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;
				remoteHeader.UserLevel     = Header.UserLevel;

				// 호출정보셋트
				remoteData.MediaCode       = adFileWideModel.MediaCode;
				remoteData.ItemNo	       = adFileWideModel.ItemNo;
				remoteData.ItemName	       = adFileWideModel.ItemName;
				remoteData.FileName	       = adFileWideModel.FileName;
				remoteData.FileState       = adFileWideModel.FileState;
                
				// 웹서비스 호출 타임아웃설정
				svc.Timeout = FrameSystem.m_SystemTimeout * 5;

				// 웹서비스 메소드 호출
				remoteData = svc.SetAdFileChange(remoteHeader, remoteData);

				// 결과코드검사
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// 결과 셋트
				adFileWideModel.ResultCnt   = remoteData.ResultCnt;
				adFileWideModel.ResultCD    = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("광고파일 소재교체 End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":SetAdFileChange():" + fe.ErrCode + ":" + fe.ResultMsg);
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
