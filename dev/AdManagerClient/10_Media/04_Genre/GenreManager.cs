// ===============================================================================
// UserUpdate Manager  for Charites Project
//
// UserUpdateManager.cs
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
	public class GenreManager : BaseManager
	{
		/// <summary>
		/// 생성자
		/// </summary>
		/// <param name="systemModel"></param>
		/// <param name="commonModel"></param>

		public GenreManager(SystemModel systemModel, CommonModel commonModel) : base(systemModel, commonModel)
		{
			_log = FrameSystem.oLog;
			_module = "USERINFO";			
			_Host  = FrameSystem.m_WebServer_Host;
			_Port  = FrameSystem.m_WebServer_Port;
			_Path  = FrameSystem.m_WebServer_App + "/Media/GenreService.asmx";
		}

		/// <summary>
		/// 사용자정보조회
		/// </summary>
		/// <param name="genreModel"></param>
		public void GetGenreList(GenreModel genreModel)
		{

			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("사용자목록조회 Start");
				_log.Debug("-----------------------------------------");
				
				// 웹서비스 인스턴스 생성
				GenreServicePloxy.GenreService svc = new GenreServicePloxy.GenreService();
				svc.Url = _WebServiceUrl;
			
				// 리모트 모델 생성
				GenreServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.GenreServicePloxy.HeaderModel();
				GenreServicePloxy.GenreModel remoteData   = new AdManagerClient.GenreServicePloxy.GenreModel();

				// 헤더정보 셋트
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;
				remoteHeader.UserLevel     = Header.UserLevel;

				// 호출정보 셋트
				remoteData.SearchKey       = genreModel.SearchKey;
				remoteData.SearchGenreLevel = genreModel.SearchGenreLevel;
				

                // 웹서비스 호출 타임아웃설정
                svc.Timeout = FrameSystem.m_SystemTimeout;

				// 웹서비스 메소드 호출
				remoteData = svc.GetGenreList(remoteHeader, remoteData);

				// 결과코드검사
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// 결과 셋트
				genreModel.UserDataSet = remoteData.UserDataSet.Copy();
				genreModel.ResultCnt   = remoteData.ResultCnt;
				genreModel.ResultCD    = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("사용자목록조회 End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":GetUserList():" + fe.ErrCode + ":" + fe.ResultMsg);
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
		public bool GetUserDetail(BaseModel baseModel)
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
		/// <param name="genreModel"></param>
		public void SetGenreUpdate(GenreModel genreModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("사용자정보수정 Start");
				_log.Debug("-----------------------------------------");


				//입력데이터의 Validation 검사
				/*if(genreModel.MediaCode.Length < 1) 
				{
					throw new FrameException("사용자ID가 존재하지 않습니다.");
				}*/
				if(genreModel.GenreName.Length > 20) 
				{
					throw new FrameException("장르명은 20Bytes를 초과할 수 없습니다.");
				}
				/*if(genreModel.Charger.Length > 10) 
				{
					throw new FrameException("담당자명은 10Bytes를 초과할 수 없습니다.");
				}
				if(genreModel.Tell.Length > 15) 
				{
					throw new FrameException("전화번호의 길이는 15Bytes를 초과할 수 없습니다");
				}
				if(genreModel.Email.Length > 40) 
				{
					throw new FrameException("Email의 길이는 40Bytes를 초과할 수 없습니다.");
				}*/
				

	

				// 웹서비스 인스턴스 생성
				GenreServicePloxy.GenreService svc = new GenreServicePloxy.GenreService();
				svc.Url = _WebServiceUrl;
			
				// 리모트 모델 생성
				GenreServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.GenreServicePloxy.HeaderModel();
				GenreServicePloxy.GenreModel remoteData   = new AdManagerClient.GenreServicePloxy.GenreModel();

				// 헤더정보 셋트
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;

				// 호출정보셋트
				remoteData.MediaCode       = genreModel.MediaCode;
				remoteData.GenreCode       = genreModel.GenreCode;
				remoteData.GenreName     = genreModel.GenreName;				
				remoteData.ModDt     = genreModel.ModDt;
								
                // 웹서비스 호출 타임아웃설정
                svc.Timeout = FrameSystem.m_SystemTimeout;

				// 웹서비스 메소드 호출
				remoteData = svc.SetGenreUpdate(remoteHeader, remoteData);

				// 결과코드검사
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// 결과 셋트
				genreModel.ResultCnt   = remoteData.ResultCnt;
				genreModel.ResultCD    = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("사용자정보수정 End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":SetUserUpdate():" + fe.ErrCode + ":" + fe.ResultMsg);
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
		/// <param name="genreModel"></param>
		/// <returns></returns>
		public void SetGenreAdd(GenreModel genreModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("사용자추가 Start");
				_log.Debug("-----------------------------------------");

				/*if(genreModel.MediaCode.Trim().Length < 1) 
				{
					throw new FrameException("매체코드가 존재하지 않습니다.");
				}
				if(genreModel.MediaCode.Trim().Length > 10) 
				{
					throw new FrameException("매체코드는 10Bytes를 초과할 수 없습니다.");
				}*/
				if(genreModel.GenreName.Length > 20) 
				{
					throw new FrameException("장르명은 20Bytes를 초과할 수 없습니다.");
				}
				/*if(genreModel.Charger.Length > 10) 
				{
					throw new FrameException("담당자명은 10Bytes를 초과할 수 없습니다.");
				}
				if(genreModel.Tell.Length > 15) 
				{
					throw new FrameException("전화번호의 길이는 15Bytes를 초과할 수 없습니다");
				}				
				if(genreModel.Email.Length > 40) 
				{
					throw new FrameException("Email은 40Bytes를 초과할 수 없습니다.");
				}*/


				// 웹서비스 인스턴스 생성
				GenreServicePloxy.GenreService svc = new GenreServicePloxy.GenreService();
				svc.Url = _WebServiceUrl;
				
				// 리모트 모델 생성
				GenreServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.GenreServicePloxy.HeaderModel();
				GenreServicePloxy.GenreModel remoteData   = new AdManagerClient.GenreServicePloxy.GenreModel();

				// 헤더정보 셋트
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;

				// 호출정보셋트
				remoteData.MediaCode       = genreModel.MediaCode;
				remoteData.GenreCode       = genreModel.GenreCode;
				remoteData.GenreName     = genreModel.GenreName;				
				remoteData.ModDt     = genreModel.ModDt;
				
					
                // 웹서비스 호출 타임아웃설정
                svc.Timeout = FrameSystem.m_SystemTimeout;

				// 웹서비스 메소드 호출
				remoteData = svc.SetGenreCreate(remoteHeader, remoteData);

				// 결과코드검사
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// 결과 셋트
				genreModel.ResultCnt   = remoteData.ResultCnt;
				genreModel.ResultCD    = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("사용자추가 End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				genreModel.ResultCD    = "3101";
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":SetUserCreate():" + fe.ErrCode + ":" + fe.ResultMsg);
				_log.Warning("-----------------------------------------");
				throw fe;
			}
			catch(Exception e)
			{
				genreModel.ResultCD    = "3101";
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
		public void SetGenreDelete(GenreModel genreModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("사용자삭제 Start");
				_log.Debug("-----------------------------------------");

				// 웹서비스 인스턴스 생성
				GenreServicePloxy.GenreService svc = new GenreServicePloxy.GenreService();
				svc.Url = _WebServiceUrl;
				
				// 리모트 모델 생성
				GenreServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.GenreServicePloxy.HeaderModel();
				GenreServicePloxy.GenreModel remoteData   = new AdManagerClient.GenreServicePloxy.GenreModel();

				// 헤더정보 셋트
				remoteHeader.ClientKey  = Header.ClientKey;
				remoteHeader.UserID     = Header.UserID;

				// 호출정보셋트
				remoteData.MediaCode       = genreModel.MediaCode;
				remoteData.GenreCode       = genreModel.GenreCode;
				
                // 웹서비스 호출 타임아웃설정
                svc.Timeout = FrameSystem.m_SystemTimeout;

				// 웹서비스 메소드 호출
				remoteData = svc.SetGenreDelete(remoteHeader, remoteData);

				// 결과코드검사
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// 결과 셋트
				genreModel.ResultCnt   = remoteData.ResultCnt;
				genreModel.ResultCD    = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("사용자삭제 End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":SetGenreDelete():" + fe.ErrCode + ":" + fe.ResultMsg);
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
