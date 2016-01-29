// ===============================================================================
//
// FilePublishManager.cs
//
// 파일배포승인 서비스를 호출합니다. 
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
	/// 파일배포승인 웹서비스를 호출합니다. 
	/// </summary>
	public class FilePublishManager : BaseManager
	{
		/// <summary>
		/// 생성자
		/// </summary>
		/// <param name="systemModel"></param>
		/// <param name="commonModel"></param>

		public FilePublishManager(SystemModel systemModel, CommonModel commonModel) : base(systemModel, commonModel)
		{
			_log = FrameSystem.oLog;
			_module = "FilePublish";
			_Host  = FrameSystem.m_WebServer_Host;
			_Port  = FrameSystem.m_WebServer_Port;
			_Path  = FrameSystem.m_WebServer_App + "/Contract/FilePublishService.asmx";
		}


		/// <summary>
		/// 승인내역 조회
		/// </summary>
		/// <param name="filePublishModel"></param>
		public void GetPublishList(FilePublishModel filePublishModel)
		{

			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("승인내역 조회 Start");
				_log.Debug("-----------------------------------------");
				
				// 웹서비스 인스턴스 생성
				FilePublishServicePloxy.FilePublishService svc = new FilePublishServicePloxy.FilePublishService();
			
				// 웹서비스URL 동적 셋트
				svc.Url = _WebServiceUrl;

				// 리모트 모델 생성
				FilePublishServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.FilePublishServicePloxy.HeaderModel();
				FilePublishServicePloxy.FilePublishModel   remoteData   = new AdManagerClient.FilePublishServicePloxy.FilePublishModel();

				// 헤더정보 셋트
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;
				remoteHeader.UserLevel     = Header.UserLevel;

				// 호출정보 셋트
				remoteData.SearchMediaCode		 = filePublishModel.SearchMediaCode;	  				
				
				// 웹서비스 호출 타임아웃설정
				svc.Timeout = FrameSystem.m_SystemTimeout;
				// 웹서비스 메소드 호출
				remoteData = svc.GetPublishList(remoteHeader, remoteData);

				// 결과코드검사
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// 결과 셋트
				filePublishModel.PublishDataSet = remoteData.PublishDataSet.Copy();
				filePublishModel.ResultCnt   = remoteData.ResultCnt;
				filePublishModel.ResultCD    = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("승인이력 조회 End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":GetPublishList():" + fe.ErrCode + ":" + fe.ResultMsg);
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
		/// 변경상세 조회
		/// </summary>
		/// <param name="filePublishModel"></param>
		public void GetPublishHistory(FilePublishModel filePublishModel)
		{

			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("변경상세 조회 Start");
				_log.Debug("-----------------------------------------");
				
				// 웹서비스 인스턴스 생성
				FilePublishServicePloxy.FilePublishService svc = new FilePublishServicePloxy.FilePublishService();
			
				// 웹서비스URL 동적 셋트
				svc.Url = _WebServiceUrl;

				// 리모트 모델 생성
				FilePublishServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.FilePublishServicePloxy.HeaderModel();
				FilePublishServicePloxy.FilePublishModel   remoteData   = new AdManagerClient.FilePublishServicePloxy.FilePublishModel();

				// 헤더정보 셋트
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;
				remoteHeader.UserLevel     = Header.UserLevel;

				// 호출정보 셋트
				remoteData.MediaCode  = filePublishModel.MediaCode;	  				
				remoteData.AckNo      = filePublishModel.AckNo;	  				
				
				// 웹서비스 호출 타임아웃설정
				svc.Timeout = FrameSystem.m_SystemTimeout;
				// 웹서비스 메소드 호출
				remoteData = svc.GetPublishHistory(remoteHeader, remoteData);

				// 결과코드검사
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// 결과 셋트
				filePublishModel.HistoryDataSet = remoteData.HistoryDataSet.Copy();
				filePublishModel.ResultCnt   = remoteData.ResultCnt;
				filePublishModel.ResultCD    = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("변경상세 조회 End");
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
		/// 현재승인상태 조회
		/// </summary>
		/// <param name="filePublishModel"></param>
		public void GetPublishState(FilePublishModel filePublishModel)
		{

			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("현재승인상태 조회 Start");
				_log.Debug("-----------------------------------------");
				
				// 웹서비스 인스턴스 생성
				FilePublishServicePloxy.FilePublishService svc = new FilePublishServicePloxy.FilePublishService();
			
				// 웹서비스URL 동적 셋트
				svc.Url = _WebServiceUrl;

				// 리모트 모델 생성
				FilePublishServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.FilePublishServicePloxy.HeaderModel();
				FilePublishServicePloxy.FilePublishModel   remoteData   = new AdManagerClient.FilePublishServicePloxy.FilePublishModel();

				// 헤더정보 셋트
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;
				remoteHeader.UserLevel     = Header.UserLevel;

				// 호출정보 셋트
				remoteData.SearchMediaCode		 = filePublishModel.SearchMediaCode;	  				
				
				// 웹서비스 호출 타임아웃설정
				svc.Timeout = FrameSystem.m_SystemTimeout;
				// 웹서비스 메소드 호출
				remoteData = svc.GetPublishState(remoteHeader, remoteData);

				// 결과코드검사
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// 결과 셋트
				filePublishModel.AckNo       = remoteData.AckNo;
				filePublishModel.State       = remoteData.State;

				filePublishModel.ResultCnt   = remoteData.ResultCnt;
				filePublishModel.ResultCD    = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("현재승인상태 조회 End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":GetScheduleList():" + fe.ErrCode + ":" + fe.ResultMsg);
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
		/// 파일배포승인
		/// </summary>
		/// <param name="filePublishModel"></param>
		public void SetFilePublish(FilePublishModel filePublishModel)
		{

			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("파일배포승인 Start");
				_log.Debug("-----------------------------------------");
				
				// 웹서비스 인스턴스 생성
				FilePublishServicePloxy.FilePublishService svc = new FilePublishServicePloxy.FilePublishService();
			
				// 웹서비스URL 동적 셋트
				svc.Url = _WebServiceUrl;

				// 리모트 모델 생성
				FilePublishServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.FilePublishServicePloxy.HeaderModel();
				FilePublishServicePloxy.FilePublishModel   remoteData   = new AdManagerClient.FilePublishServicePloxy.FilePublishModel();

				// 헤더정보 셋트
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;
				remoteHeader.UserLevel     = Header.UserLevel;

				// 호출정보 셋트
				remoteData.AckNo		 = filePublishModel.AckNo;	  				
				remoteData.PublishDesc	 = filePublishModel.PublishDesc;	  				
				remoteData.MediaCode     = filePublishModel.MediaCode;	  				
				
				// 웹서비스 호출 타임아웃설정
				svc.Timeout = FrameSystem.m_SystemTimeout;

				// 웹서비스 메소드 호출
				remoteData = svc.SetFilePublish(remoteHeader, remoteData);

				// 결과코드검사
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// 결과 셋트
				filePublishModel.ResultCD    = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("파일배포승인 End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":SetFilePublish():" + fe.ErrCode + ":" + fe.ResultMsg);
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
		/// 승인별 파일리스트 조회
		/// </summary>
		/// <param name="filePublishModel"></param>
		public void GetPublishFileList(FilePublishModel filePublishModel)
		{

			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("파일리스트 조회 Start");
				_log.Debug("-----------------------------------------");
				
				// 웹서비스 인스턴스 생성
				FilePublishServicePloxy.FilePublishService svc = new FilePublishServicePloxy.FilePublishService();
			
				// 웹서비스URL 동적 셋트
				svc.Url = _WebServiceUrl;

				// 리모트 모델 생성
				FilePublishServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.FilePublishServicePloxy.HeaderModel();
				FilePublishServicePloxy.FilePublishModel   remoteData   = new AdManagerClient.FilePublishServicePloxy.FilePublishModel();

				// 헤더정보 셋트
				remoteHeader.ClientKey  = Header.ClientKey;
				remoteHeader.UserID     = Header.UserID;
				remoteHeader.UserLevel  = Header.UserLevel;

				// 호출정보 셋트
				remoteData.MediaCode    = filePublishModel.MediaCode;	  				
				remoteData.AckNo        = filePublishModel.AckNo;
                remoteData.ReserveDt    = filePublishModel.ReserveDt;
				
				// 웹서비스 호출 타임아웃설정
				svc.Timeout = FrameSystem.m_SystemTimeout;
				// 웹서비스 메소드 호출
				remoteData = svc.GetPublishFileList(remoteHeader, remoteData);

				// 결과코드검사
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// 결과 셋트
				filePublishModel.FileListDataSet = remoteData.FileListDataSet.Copy();
				filePublishModel.ResultCnt   = remoteData.ResultCnt;
				filePublishModel.ResultCD    = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("파일리스트 조회 End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":GetPublishFileList():" + fe.ErrCode + ":" + fe.ResultMsg);
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


		#region [ 예약작업 처리 부분 ]
		
		/// <summary>
		/// 예약파일 리스트 가져오기
		/// </summary>
		/// <param name="filePublishModel"></param>
		public void GetReserveFileList(FilePublishModel filePublishModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("예약파일 리스트 조회 Start");
				_log.Debug("-----------------------------------------");
				
				FilePublishServicePloxy.FilePublishService svc = new FilePublishServicePloxy.FilePublishService();
				svc.Url = _WebServiceUrl;
				svc.Timeout = FrameSystem.m_SystemTimeout;

				FilePublishServicePloxy.HeaderModel			remoteHeader = new AdManagerClient.FilePublishServicePloxy.HeaderModel();
				FilePublishServicePloxy.FilePublishModel	remoteData   = new AdManagerClient.FilePublishServicePloxy.FilePublishModel();

				// 헤더정보 셋트
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;
				remoteHeader.UserLevel     = Header.UserLevel;

				// 호출정보 셋트
				remoteData.MediaCode  = filePublishModel.MediaCode;	  				
				remoteData.AckNo      = filePublishModel.AckNo;	  				
								
				// 웹서비스 메소드 호출
				remoteData = svc.GetReserveFiles(remoteHeader, remoteData);

				// 결과코드검사
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// 결과 셋트
				filePublishModel.FileListDataSet = remoteData.FileListDataSet.Copy();
				filePublishModel.ResultCnt   = remoteData.ResultCnt;
				filePublishModel.ResultCD    = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("예약파일 리스트 조회 End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":GetReserveFileList():" + fe.ErrCode + ":" + fe.ResultMsg);
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
		/// 예약작업 리스트 가져오기
		/// </summary>
		/// <param name="filePublishModel"></param>
		public void GetReserveWorkList(FilePublishModel filePublishModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("예약작업 리스트 조회 Start");
				_log.Debug("-----------------------------------------");
				
				FilePublishServicePloxy.FilePublishService svc = new FilePublishServicePloxy.FilePublishService();
				svc.Url = _WebServiceUrl;
				svc.Timeout = FrameSystem.m_SystemTimeout;

				FilePublishServicePloxy.HeaderModel			remoteHeader = new AdManagerClient.FilePublishServicePloxy.HeaderModel();
				FilePublishServicePloxy.FilePublishModel	remoteData   = new AdManagerClient.FilePublishServicePloxy.FilePublishModel();

				// 헤더정보 셋트
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;
				remoteHeader.UserLevel     = Header.UserLevel;

				// 호출정보 셋트
				remoteData.MediaCode  = filePublishModel.MediaCode;	  				
				remoteData.AckNo      = filePublishModel.AckNo;	  				
								
				// 웹서비스 메소드 호출
				remoteData = svc.GetReserveWorks(remoteHeader, remoteData);

				// 결과코드검사
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// 결과 셋트
				filePublishModel.FileListDataSet = remoteData.FileListDataSet.Copy();
				filePublishModel.ResultCnt   = remoteData.ResultCnt;
				filePublishModel.ResultCD    = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("예약작업 리스트 조회 End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":GetReserveWorkList():" + fe.ErrCode + ":" + fe.ResultMsg);
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
		/// 예약작업 상세정보 가져오기
		/// </summary>
		/// <param name="filePublishModel"></param>
		public void GetReserveWorkDetail(FilePublishModel filePublishModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("예약작업 상세정보 조회 Start");
				_log.Debug("-----------------------------------------");
				
				FilePublishServicePloxy.FilePublishService svc = new FilePublishServicePloxy.FilePublishService();
				svc.Url = _WebServiceUrl;
				svc.Timeout = FrameSystem.m_SystemTimeout;

				FilePublishServicePloxy.HeaderModel			remoteHeader = new AdManagerClient.FilePublishServicePloxy.HeaderModel();
				FilePublishServicePloxy.FilePublishModel	remoteData   = new AdManagerClient.FilePublishServicePloxy.FilePublishModel();

				// 헤더정보 셋트
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;
				remoteHeader.UserLevel     = Header.UserLevel;

				// 호출정보 셋트
				remoteData.MediaCode	= filePublishModel.MediaCode;	  				
				remoteData.AckNo		= filePublishModel.AckNo;
				remoteData.ReserveDt	= filePublishModel.ReserveDt;
												
				// 웹서비스 메소드 호출
				remoteData = svc.getReserveWorkSelect(remoteHeader, remoteData);

				// 결과코드검사
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// 결과 셋트
				filePublishModel.State			= remoteData.State;
				filePublishModel.ReserveDt		= remoteData.ReserveDt;
				filePublishModel.ReserveUserName= remoteData.ReserveUserName;
				filePublishModel.ModifyUserName	= remoteData.ModifyUserName;
				filePublishModel.ModDt			= remoteData.ModDt;
				filePublishModel.PublishDesc	= remoteData.PublishDesc;
				

				filePublishModel.ResultCnt		= remoteData.ResultCnt;
				filePublishModel.ResultCD		= remoteData.ResultCD;
				filePublishModel.ResultDesc		= remoteData.ResultDesc;

				_log.Debug("-----------------------------------------");
				_log.Debug("예약작업 상세정보 조회 End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":GetReserveWorkList():" + fe.ErrCode + ":" + fe.ResultMsg);
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
		/// 예약작업 상세정보 저장
		/// </summary>
		/// <param name="filePublishModel"></param>
		public void SetReserveWorkDetail(FilePublishModel filePublishModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("예약작업 상세정보 저장 Start");
				_log.Debug("-----------------------------------------");
				
				FilePublishServicePloxy.FilePublishService svc = new FilePublishServicePloxy.FilePublishService();
				svc.Url = _WebServiceUrl;
				svc.Timeout = FrameSystem.m_SystemTimeout;

				FilePublishServicePloxy.HeaderModel			remoteHeader = new AdManagerClient.FilePublishServicePloxy.HeaderModel();
				FilePublishServicePloxy.FilePublishModel	remoteData   = new AdManagerClient.FilePublishServicePloxy.FilePublishModel();

				// 헤더정보 셋트
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;
				remoteHeader.UserLevel     = Header.UserLevel;

				// 호출정보 셋트
				remoteData.MediaCode	= filePublishModel.MediaCode;	  				
				remoteData.AckNo		= filePublishModel.AckNo;
				remoteData.ReserveDt	= filePublishModel.ReserveDt;
				remoteData.PublishDesc	= filePublishModel.PublishDesc;
				remoteData.State		= filePublishModel.State;
				remoteData.SearchReserveKey = filePublishModel.SearchReserveKey;
												
				// 웹서비스 메소드 호출
				remoteData = svc.setReserveWorkUpdate(remoteHeader, remoteData);

				// 결과코드검사
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				filePublishModel.ResultCnt		= remoteData.ResultCnt;
				filePublishModel.ResultCD		= remoteData.ResultCD;
				filePublishModel.ResultDesc		= remoteData.ResultDesc;

				_log.Debug("-----------------------------------------");
				_log.Debug("예약작업 상세정보 조회 End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":GetReserveWorkList():" + fe.ErrCode + ":" + fe.ResultMsg);
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
		/// 예약작업 상세정보 추가
		/// </summary>
		/// <param name="filePublishModel"></param>
		public void NewReserveWorkDetail(FilePublishModel filePublishModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("예약작업 상세정보 입력 Start");
				_log.Debug("-----------------------------------------");
				
				FilePublishServicePloxy.FilePublishService svc = new FilePublishServicePloxy.FilePublishService();
				svc.Url = _WebServiceUrl;
				svc.Timeout = FrameSystem.m_SystemTimeout;

				FilePublishServicePloxy.HeaderModel			remoteHeader = new AdManagerClient.FilePublishServicePloxy.HeaderModel();
				FilePublishServicePloxy.FilePublishModel	remoteData   = new AdManagerClient.FilePublishServicePloxy.FilePublishModel();

				// 헤더정보 셋트
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;
				remoteHeader.UserLevel     = Header.UserLevel;

				// 호출정보 셋트
				remoteData.MediaCode	= filePublishModel.MediaCode;	  				
				remoteData.AckNo		= filePublishModel.AckNo;
				remoteData.ReserveDt	= filePublishModel.ReserveDt;
				remoteData.PublishDesc	= filePublishModel.PublishDesc;
				remoteData.State		= filePublishModel.State;
												
				// 웹서비스 메소드 호출
				remoteData = svc.setReserveWorkInsert(remoteHeader, remoteData);

				// 결과코드검사
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				filePublishModel.ResultCnt		= remoteData.ResultCnt;
				filePublishModel.ResultCD		= remoteData.ResultCD;
				filePublishModel.ResultDesc		= remoteData.ResultDesc;

				_log.Debug("-----------------------------------------");
				_log.Debug("예약작업 상세정보 입력 End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":NewReserveWorkDetail():" + fe.ErrCode + ":" + fe.ResultMsg);
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
		/// 예약파일 처리
		/// </summary>
		/// <param name="filePublishModel"></param>
		public void SetReserveFileDetail(FilePublishModel filePublishModel)
		{
			try
			{
				FilePublishServicePloxy.FilePublishService svc = new FilePublishServicePloxy.FilePublishService();
				svc.Url = _WebServiceUrl;
				svc.Timeout = FrameSystem.m_SystemTimeout;

				FilePublishServicePloxy.HeaderModel			remoteHeader = new AdManagerClient.FilePublishServicePloxy.HeaderModel();
				FilePublishServicePloxy.FilePublishModel	remoteData   = new AdManagerClient.FilePublishServicePloxy.FilePublishModel();

				// 헤더정보 셋트
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;
				remoteHeader.UserLevel     = Header.UserLevel;

				// 호출정보 셋트
				remoteData.MediaCode	= filePublishModel.MediaCode;	  				
				remoteData.AckNo		= filePublishModel.AckNo;
				remoteData.ReserveDt	= filePublishModel.ReserveDt;
				remoteData.ItemNo		= filePublishModel.ItemNo;
				remoteData.JobType		= filePublishModel.JobType;
												
				// 웹서비스 메소드 호출
				remoteData = svc.setReserveFileUpdate(remoteHeader, remoteData);

				// 결과코드검사
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				filePublishModel.ResultCnt		= remoteData.ResultCnt;
				filePublishModel.ResultCD		= remoteData.ResultCD;
				filePublishModel.ResultDesc		= remoteData.ResultDesc;

				_log.Debug("-----------------------------------------");
				_log.Debug("예약작업 상세정보 조회 End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":GetReserveWorkList():" + fe.ErrCode + ":" + fe.ResultMsg);
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


		#endregion

	}
}
