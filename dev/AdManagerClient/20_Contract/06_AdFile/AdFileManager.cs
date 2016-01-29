// ===============================================================================
// AdFile Manager  for Charites Project
//
// AdFileManager.cs
//
// 광고파일정보 서비스를 호출합니다. 
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
	/// 광고파일정보 웹서비스를 호출합니다. 
	/// </summary>
	public class AdFileManager : BaseManager
	{
		/// <summary>
		/// 생성자
		/// </summary>
		/// <param name="systemModel"></param>
		/// <param name="commonModel"></param>
		public AdFileManager(SystemModel systemModel, CommonModel commonModel) : base(systemModel, commonModel)
		{
			_log = FrameSystem.oLog;
			_module = "AdFile";
			_Host  = FrameSystem.m_WebServer_Host;
			_Port  = FrameSystem.m_WebServer_Port;
			_Path  = FrameSystem.m_WebServer_App + "/Contract/AdFileService.asmx";
		}

		/// <summary>
		/// 광고파일정보조회
		/// </summary>
		/// <param name="userModel"></param>
		public void GetAdFileList(AdFileModel adFileModel)
		{

			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("광고파일목록조회 Start");
				_log.Debug("-----------------------------------------");
				
				// 웹서비스 인스턴스 생성
				AdFileServicePloxy.AdFileService svc = new AdFileServicePloxy.AdFileService();
			
				// 웹서비스URL 동적 셋트
				svc.Url = _WebServiceUrl;

				// 리모트 모델 생성
				AdFileServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.AdFileServicePloxy.HeaderModel();
				AdFileServicePloxy.AdFileModel   remoteData   = new AdManagerClient.AdFileServicePloxy.AdFileModel();

				// 헤더정보 셋트
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;
				remoteHeader.UserLevel     = Header.UserLevel;

				// 호출정보 셋트
				remoteData.SearchKey             = adFileModel.SearchKey;
				remoteData.SearchMediaCode		 = adFileModel.SearchMediaCode;	  
				remoteData.SearchRapCode		 = adFileModel.SearchRapCode;       
				remoteData.SearchAgencyCode	     = adFileModel.SearchAgencyCode;    
				remoteData.SearchAdvertiserCode  = adFileModel.SearchAdvertiserCode;
				remoteData.SearchAdType			 = adFileModel.SearchAdType;       
				remoteData.SearchFileType		 = adFileModel.SearchFileType;
				remoteData.SearchchkAdState_10	 = adFileModel.SearchchkAdState_10; 
				remoteData.SearchchkAdState_20	 = adFileModel.SearchchkAdState_20; 
				remoteData.SearchchkAdState_30	 = adFileModel.SearchchkAdState_30; 
				remoteData.SearchchkAdState_40	 = adFileModel.SearchchkAdState_40; 
				remoteData.SearchchkFileState_10 = adFileModel.SearchchkFileState_10; 
				remoteData.SearchchkFileState_11 = adFileModel.SearchchkFileState_11; 
				remoteData.SearchchkFileState_12 = adFileModel.SearchchkFileState_12; 
				remoteData.SearchchkFileState_15 = adFileModel.SearchchkFileState_15; 
				remoteData.SearchchkFileState_20 = adFileModel.SearchchkFileState_20; 
				remoteData.SearchchkFileState_30 = adFileModel.SearchchkFileState_30; 
				remoteData.SearchchkFileState_90 = adFileModel.SearchchkFileState_90; 
				
                // 웹서비스 호출 타임아웃설정
				svc.Timeout = FrameSystem.m_SystemTimeout;
				
                // 웹서비스 메소드 호출
				remoteData = svc.GetAdFileList(remoteHeader, remoteData);

				// 결과코드검사
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// 결과 셋트
				adFileModel.AdFileDataSet = remoteData.AdFileDataSet.Copy();
				adFileModel.ResultCnt   = remoteData.ResultCnt;
				adFileModel.ResultCD    = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("광고파일목록조회 End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":GetAdFileList():" + fe.ErrCode + ":" + fe.ResultMsg);
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
		/// 광고파일정보조회
		/// </summary>
		/// <param name="userModel"></param>
		public void GetAdFileSearch(AdFileModel adFileModel)
		{

			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("광고파일목록조회 Start");
				_log.Debug("-----------------------------------------");
				
				// 웹서비스 인스턴스 생성
				AdFileServicePloxy.AdFileService svc = new AdFileServicePloxy.AdFileService();
			
				// 웹서비스URL 동적 셋트
				svc.Url = _WebServiceUrl;

				// 리모트 모델 생성
				AdFileServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.AdFileServicePloxy.HeaderModel();
				AdFileServicePloxy.AdFileModel   remoteData   = new AdManagerClient.AdFileServicePloxy.AdFileModel();

				// 헤더정보 셋트
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;
				remoteHeader.UserLevel     = Header.UserLevel;

				// 호출정보 셋트
				remoteData.SearchKey             = adFileModel.SearchKey;
				remoteData.SearchMediaCode		 = adFileModel.SearchMediaCode;	  
				remoteData.SearchRapCode		 = adFileModel.SearchRapCode;       
								
				// 웹서비스 호출 타임아웃설정
				svc.Timeout = FrameSystem.m_SystemTimeout;
				
				// 웹서비스 메소드 호출
				remoteData = svc.GetAdFileSearch(remoteHeader, remoteData);

				// 결과코드검사
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// 결과 셋트
				adFileModel.AdFileDataSet = remoteData.AdFileDataSet.Copy();
				adFileModel.ResultCnt   = remoteData.ResultCnt;
				adFileModel.ResultCD    = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("광고파일목록조회 End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":GetAdFileSearch():" + fe.ErrCode + ":" + fe.ResultMsg);
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
		/// 파일배포이력 조회
		/// </summary>
		/// <param name="userModel"></param>
		public void GetPublishHistory(AdFileModel adFileModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("파일배포이력조회 Start");
				_log.Debug("-----------------------------------------");
				
				AdFileServicePloxy.AdFileService svc = new AdFileServicePloxy.AdFileService();
				svc.Url = _WebServiceUrl;

				AdFileServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.AdFileServicePloxy.HeaderModel();
				AdFileServicePloxy.AdFileModel   remoteData   = new AdManagerClient.AdFileServicePloxy.AdFileModel();

				remoteHeader.ClientKey		= Header.ClientKey;
				remoteHeader.UserID			= Header.UserID;
				remoteHeader.UserLevel		= Header.UserLevel;

				remoteData.SearchMediaCode	= adFileModel.SearchMediaCode;	  
				remoteData.ItemNo			= adFileModel.ItemNo;       
								
				svc.Timeout = FrameSystem.m_SystemTimeout;
				remoteData = svc.GetPublishHistory(remoteHeader, remoteData);

				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// 결과 셋트
				adFileModel.AdFileDataSet = remoteData.AdFileDataSet.Copy();
				adFileModel.ResultCnt   = remoteData.ResultCnt;
				adFileModel.ResultCD    = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("파일배포이력조회 End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":GetPublishHistory():" + fe.ErrCode + ":" + fe.ResultMsg);
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
		/// 광고파일 교체이력 조회
		/// </summary>
		/// <param name="adFileModel"></param>
		public void GetFileRePublishHistory(AdFileModel adFileModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("파일교체이력조회 Start");
				_log.Debug("-----------------------------------------");
				
				AdFileServicePloxy.AdFileService svc = new AdFileServicePloxy.AdFileService();
				svc.Url = _WebServiceUrl;

				AdFileServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.AdFileServicePloxy.HeaderModel();
				AdFileServicePloxy.AdFileModel   remoteData   = new AdManagerClient.AdFileServicePloxy.AdFileModel();

				remoteHeader.ClientKey		= Header.ClientKey;
				remoteHeader.UserID			= Header.UserID;
				remoteHeader.UserLevel		= Header.UserLevel;

				remoteData.SearchMediaCode	= adFileModel.SearchMediaCode;	  
				remoteData.ItemNo			= adFileModel.ItemNo;       
								
				svc.Timeout = FrameSystem.m_SystemTimeout;
				remoteData = svc.GetFileRepHistory(remoteHeader, remoteData);

				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// 결과 셋트
				adFileModel.AdFileDataSet	= remoteData.AdFileDataSet.Copy();
				adFileModel.ResultCnt		= remoteData.ResultCnt;
				adFileModel.ResultCD		= remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("파일교체이력조회 End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":GetFileRePublishHistory():" + fe.ErrCode + ":" + fe.ResultMsg);
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
		/// 광고파일정보 수정
		/// </summary>
		/// <param name="userModel"></param>
		public void SetAdFileUpdate(AdFileModel adFileModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("광고파일정보수정 Start");
				_log.Debug("-----------------------------------------");

				//입력데이터의 Validation 검사
				if(adFileModel.ItemNo.Length < 1) 
				{
					throw new FrameException("광고내역이 선택되지 않았습니다.");
				}
				if(adFileModel.FileName.Length > 40) 
				{
					throw new FrameException("광고파일명의 길이는 50Bytes를 초과할 수 없습니다.");
				}

				// 웹서비스 인스턴스 생성
				AdFileServicePloxy.AdFileService svc = new AdFileServicePloxy.AdFileService();

				// 웹서비스URL 동적 셋트
				svc.Url = _WebServiceUrl;
			
				// 리모트 모델 생성
				AdFileServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.AdFileServicePloxy.HeaderModel();
				AdFileServicePloxy.AdFileModel    remoteData   = new AdManagerClient.AdFileServicePloxy.AdFileModel();

				// 헤더정보 셋트
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;
				remoteHeader.UserLevel     = Header.UserLevel;

				// 호출정보셋트
				remoteData.ItemNo	       = adFileModel.ItemNo;
				remoteData.ItemName        = adFileModel.ItemName;
				remoteData.FileType        = adFileModel.FileType;
				remoteData.FileLength      = adFileModel.FileLength;
				remoteData.FilePath        = adFileModel.FilePath;
				remoteData.PreFileName     = adFileModel.PreFileName;
				remoteData.FileName        = adFileModel.FileName;
				remoteData.DownLevel       = adFileModel.DownLevel;

                // 웹서비스 호출 타임아웃설정
                svc.Timeout = FrameSystem.m_SystemTimeout;
				// 웹서비스 메소드 호출
				remoteData = svc.SetAdFileUpdate(remoteHeader, remoteData);

				// 결과코드검사
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// 결과 셋트
				adFileModel.ResultCnt   = remoteData.ResultCnt;
				adFileModel.ResultCD    = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("광고파일정보수정 End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":SetAgntUpdate():" + fe.ErrCode + ":" + fe.ResultMsg);
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
		/// 광고파일정보 수정
		/// </summary>
		/// <param name="userModel"></param>
		public void SetFileUpdate(AdFileModel adFileModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("광고파일정보수정 Start");
				_log.Debug("-----------------------------------------");
				
				// 웹서비스 인스턴스 생성
				AdFileServicePloxy.AdFileService svc = new AdFileServicePloxy.AdFileService();

				// 웹서비스URL 동적 셋트
				svc.Url = _WebServiceUrl;
			
				// 리모트 모델 생성
				AdFileServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.AdFileServicePloxy.HeaderModel();
				AdFileServicePloxy.AdFileModel    remoteData   = new AdManagerClient.AdFileServicePloxy.AdFileModel();

				// 헤더정보 셋트
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;
				remoteHeader.UserLevel     = Header.UserLevel;

				// 호출정보셋트
				remoteData.ItemNo	       = adFileModel.ItemNo;
				remoteData.newItemNo        = adFileModel.newItemNo;
				
				// 웹서비스 호출 타임아웃설정
				svc.Timeout = FrameSystem.m_SystemTimeout;
				// 웹서비스 메소드 호출
				remoteData = svc.SetFileUpdate(remoteHeader, remoteData);

				// 결과코드검사
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// 결과 셋트
				adFileModel.ResultCnt   = remoteData.ResultCnt;
				adFileModel.ResultCD    = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("광고파일정보수정 End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":SetAgntUpdate():" + fe.ErrCode + ":" + fe.ResultMsg);
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
		/// FTP업로드 정보 조회
		/// </summary>
		public void GetFtpConfig(AdFileModel adFileModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("FTP업로드 정보 조회 Start");
				_log.Debug("-----------------------------------------");
				
				// 웹서비스 인스턴스 생성
				AdFileServicePloxy.AdFileService svc = new AdFileServicePloxy.AdFileService();
			
				// 웹서비스URL 동적 셋트
				svc.Url = _WebServiceUrl;

				// 리모트 모델 생성
				AdFileServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.AdFileServicePloxy.HeaderModel();
				AdFileServicePloxy.AdFileModel   remoteData   = new AdManagerClient.AdFileServicePloxy.AdFileModel();

				// 헤더정보 셋트
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;
				remoteHeader.UserLevel     = Header.UserLevel;

				// 호출정보 셋트
				
				// 웹서비스 메소드 호출
				remoteData = svc.GetFtpConfig(remoteHeader, remoteData);


				// 결과코드검사
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// 결과 셋트
				adFileModel.FtpUploadID   = remoteData.FtpUploadID;
				adFileModel.FtpUploadPW   = remoteData.FtpUploadPW;
				adFileModel.FtpUploadHost = remoteData.FtpUploadHost;
				adFileModel.FtpUploadPort = remoteData.FtpUploadPort;
				adFileModel.FtpUploadPath = remoteData.FtpUploadPath;
				adFileModel.FtpMovePath   = remoteData.FtpMovePath;
				adFileModel.FtpMoveUseYn  = remoteData.FtpMoveUseYn;
				adFileModel.FtpCdnID   = remoteData.FtpCdnID;
				adFileModel.FtpCdnPW   = remoteData.FtpCdnPW;
				adFileModel.FtpCdnHost = remoteData.FtpCdnHost;
				adFileModel.FtpCdnPort = remoteData.FtpCdnPort;
				adFileModel.CmsMasUrl	= remoteData.CmsMasUrl;
				adFileModel.CmsMasQuery = remoteData.CmsMasQuery;

				adFileModel.ResultCnt   = remoteData.ResultCnt;
				adFileModel.ResultCD    = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("FTP업로드 정보 조회 End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":GetFtpConfig():" + fe.ErrCode + ":" + fe.ResultMsg);
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
