// ===============================================================================
// MediaRapUpdate Manager  for Charites Project
//
// MediaRapUpdateManager.cs
//
// 사용자정보 저장 서비스를 호출합니다. 
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
	/// 사용자정보 웹서비스를 호출합니다. 
	/// </summary>
	public class MediaRapManager : BaseManager
	{
		/// <summary>
		/// 생성자
		/// </summary>
		/// <param name="systemModel"></param>
		/// <param name="commonModel"></param>

		public MediaRapManager(SystemModel systemModel, CommonModel commonModel) : base(systemModel, commonModel)
		{
			_log = FrameSystem.oLog;
			_module = "USERINFO";
			_Host  = FrameSystem.m_WebServer_Host;
			_Port  = FrameSystem.m_WebServer_Port;
			_Path  = FrameSystem.m_WebServer_App + "/Media/MediaRapService.asmx";
		}

		/// <summary>
		/// 사용자정보조회
		/// </summary>
		/// <param name="mediarapModel"></param>
		public void GetMediaRapList(MediaRapModel mediarapModel)
		{

			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("사용자목록조회 Start");
				_log.Debug("-----------------------------------------");
				
				// 웹서비스 인스턴스 생성
				MediaRapServicePloxy.MediaRapService svc = new MediaRapServicePloxy.MediaRapService();
				svc.Url = _WebServiceUrl;
			
				// 리모트 모델 생성
				MediaRapServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.MediaRapServicePloxy.HeaderModel();
				MediaRapServicePloxy.MediaRapModel remoteData   = new AdManagerClient.MediaRapServicePloxy.MediaRapModel();

				// 헤더정보 셋트
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;
				remoteHeader.UserLevel     = Header.UserLevel;
				remoteHeader.UserClass	   = Header.UserClass;

				// 호출정보 셋트
				remoteData.SearchKey       = mediarapModel.SearchKey;
				remoteData.SearchMediaRap = mediarapModel.SearchMediaRap;
				remoteData.SearchchkAdState_10	 = mediarapModel.SearchchkAdState_10; 

                // 웹서비스 호출 타임아웃설정
                svc.Timeout = FrameSystem.m_SystemTimeout;

				// 웹서비스 메소드 호출
				remoteData = svc.GetMediaRapList(remoteHeader, remoteData);

				// 결과코드검사
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// 결과 셋트
				mediarapModel.MediaRapDataSet = remoteData.MediaRapDataSet.Copy();
				mediarapModel.ResultCnt   = remoteData.ResultCnt;
				mediarapModel.ResultCD    = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("사용자목록조회 End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":GetMediaRapList():" + fe.ErrCode + ":" + fe.ResultMsg);
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
		/// Service 호출을 위한 메소드
		/// </summary>
		public bool GetMediaRapDetail(BaseModel baseModel)
		{
			
			_log.Debug("-----------------------------------------");
			_log.Debug( this.ToString() + " Start");
			_log.Debug("-----------------------------------------");

			_log.Debug("-----------------------------------------");
			_log.Debug( this.ToString() + " End");
			_log.Debug("-----------------------------------------");

			return true;
		}

		/// <summary>
		/// 사용자정보 수정
		/// </summary>
		/// <param name="mediarapModel"></param>
		public void SetMediaRapUpdate(MediaRapModel mediarapModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("사용자정보수정 Start");
				_log.Debug("-----------------------------------------");


				//입력데이터의 Validation 검사
				if(mediarapModel.RapCode.Length < 1) 
				{
					throw new FrameException("랩ID가 존재하지 않습니다.");
				}
				if(mediarapModel.RapName.Length > 10) 
				{
					throw new FrameException("랩명의 길이는 10Bytes를 초과할 수 없습니다.");
				}
				
				if(mediarapModel.Tell.Length > 15) 
				{
					throw new FrameException("전화번호의 길이는 15Bytes를 초과할 수 없습니다");
				}				
				if(mediarapModel.Comment.Length > 50) 
				{
					throw new FrameException("비고란의 길이는 50Bytes를 초과할 수 없습니다.");
				}

	

				// 웹서비스 인스턴스 생성
				MediaRapServicePloxy.MediaRapService svc = new MediaRapServicePloxy.MediaRapService();
				svc.Url = _WebServiceUrl;
			
				// 리모트 모델 생성
				MediaRapServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.MediaRapServicePloxy.HeaderModel();
				MediaRapServicePloxy.MediaRapModel remoteData   = new AdManagerClient.MediaRapServicePloxy.MediaRapModel();

				// 헤더정보 셋트
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;

				// 호출정보셋트
				remoteData.RapCode       = mediarapModel.RapCode;
				remoteData.RapName     = mediarapModel.RapName;
				remoteData.RapType = mediarapModel.RapType;
				remoteData.Tell     = mediarapModel.Tell;				
				remoteData.Comment  = mediarapModel.Comment;		
				remoteData.UseYn     = mediarapModel.UseYn;				
				

                // 웹서비스 호출 타임아웃설정
                svc.Timeout = FrameSystem.m_SystemTimeout;

				// 웹서비스 메소드 호출
				remoteData = svc.SetMediaRapUpdate(remoteHeader, remoteData);

				// 결과코드검사
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// 결과 셋트
				mediarapModel.ResultCnt   = remoteData.ResultCnt;
				mediarapModel.ResultCD    = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("사용자정보수정 End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":SetMediaRapUpdate():" + fe.ErrCode + ":" + fe.ResultMsg);
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
		/// 사용자추가
		/// </summary>
		/// <param name="mediarapModel"></param>
		/// <returns></returns>
		public void SetMediaRapAdd(MediaRapModel mediarapModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("사용자추가 Start");
				_log.Debug("-----------------------------------------");
				
				if(mediarapModel.RapName.Length > 10) 
				{
					throw new FrameException("랩명의 길이는 10Bytes를 초과할 수 없습니다.");
				}
				
				if(mediarapModel.Tell.Length > 15) 
				{
					throw new FrameException("전화번호의 길이는 15Bytes를 초과할 수 없습니다");
				}				
				if(mediarapModel.Comment.Length > 50) 
				{
					throw new FrameException("비고란의 길이는 50Bytes를 초과할 수 없습니다.");
				}

				// 웹서비스 인스턴스 생성
				MediaRapServicePloxy.MediaRapService svc = new MediaRapServicePloxy.MediaRapService();
				svc.Url = _WebServiceUrl;
				
				// 리모트 모델 생성
				MediaRapServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.MediaRapServicePloxy.HeaderModel();
				MediaRapServicePloxy.MediaRapModel remoteData   = new AdManagerClient.MediaRapServicePloxy.MediaRapModel();

				// 헤더정보 셋트
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;

				// 호출정보셋트
				remoteData.RapCode       = mediarapModel.RapCode;
				remoteData.RapName     = mediarapModel.RapName;
				remoteData.RapType = mediarapModel.RapType;
				remoteData.Tell     = mediarapModel.Tell;				
				remoteData.Comment  = mediarapModel.Comment;		
				remoteData.UseYn     = mediarapModel.UseYn;				
					
                // 웹서비스 호출 타임아웃설정
                svc.Timeout = FrameSystem.m_SystemTimeout;

				// 웹서비스 메소드 호출
				remoteData = svc.SetMediaRapCreate(remoteHeader, remoteData);

				// 결과코드검사
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// 결과 셋트
				mediarapModel.ResultCnt   = remoteData.ResultCnt;
				mediarapModel.ResultCD    = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("사용자추가 End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":SetMediaRapCreate():" + fe.ErrCode + ":" + fe.ResultMsg);
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
		/// 사용자 삭제
		/// </summary>
		/// <param name="baseModel"></param>
		public void SetMediaRapDelete(MediaRapModel mediarapModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("사용자삭제 Start");
				_log.Debug("-----------------------------------------");

				// 웹서비스 인스턴스 생성
				MediaRapServicePloxy.MediaRapService svc = new MediaRapServicePloxy.MediaRapService();
				svc.Url = _WebServiceUrl;
				
				// 리모트 모델 생성
				MediaRapServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.MediaRapServicePloxy.HeaderModel();
				MediaRapServicePloxy.MediaRapModel remoteData   = new AdManagerClient.MediaRapServicePloxy.MediaRapModel();

				// 헤더정보 셋트
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;
				// 호출정보셋트
				remoteData.RapCode       = mediarapModel.RapCode;
					
                // 웹서비스 호출 타임아웃설정
                svc.Timeout = FrameSystem.m_SystemTimeout;

				// 웹서비스 메소드 호출
				remoteData = svc.SetMediaRapDelete(remoteHeader, remoteData);

				// 결과코드검사
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// 결과 셋트
				mediarapModel.ResultCnt   = remoteData.ResultCnt;
				mediarapModel.ResultCD    = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("사용자삭제 End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":SetMediaRapDelete():" + fe.ErrCode + ":" + fe.ResultMsg);
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
