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
	public class UserInfoManager : BaseManager
	{
		/// <summary>
		/// 생성자
		/// </summary>
		/// <param name="systemModel"></param>
		/// <param name="commonModel"></param>

		public UserInfoManager(SystemModel systemModel, CommonModel commonModel) : base(systemModel, commonModel)
		{
			_log = FrameSystem.oLog;
			_module = "USERINFO";
			_Host  = FrameSystem.m_WebServer_Host;
			_Port  = FrameSystem.m_WebServer_Port;
			_Path  = FrameSystem.m_WebServer_App + "/Media/UserInfoService.asmx";
		}

		/// <summary>
		/// 사용자정보조회
		/// </summary>
		/// <param name="userModel"></param>
		public void GetUserList(UserInfoModel userModel)
		{

			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("사용자목록조회 Start");
				_log.Debug("-----------------------------------------");
				
				// 웹서비스 인스턴스 생성
				UserInfoServicePloxy.UserInfoService svc = new UserInfoServicePloxy.UserInfoService();
				svc.Url = _WebServiceUrl;
			
				// 리모트 모델 생성
				UserInfoServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.UserInfoServicePloxy.HeaderModel();
				UserInfoServicePloxy.UserInfoModel remoteData   = new AdManagerClient.UserInfoServicePloxy.UserInfoModel();

				// 헤더정보 셋트
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;
				remoteHeader.UserLevel     = Header.UserLevel;
				remoteHeader.UserClass     = Header.UserClass;

				// 호출정보 셋트
				remoteData.SearchKey       = userModel.SearchKey;
				remoteData.SearchUserLevel = userModel.SearchUserLevel;
				remoteData.SearchUserClass = userModel.SearchUserClass;

				remoteData.SearchchkAdState_10	 = userModel.SearchchkAdState_10; 
				
                // 웹서비스 호출 타임아웃설정
                svc.Timeout = FrameSystem.m_SystemTimeout;

				// 웹서비스 메소드 호출
				remoteData = svc.GetUsersList(remoteHeader, remoteData);

				// 결과코드검사
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// 결과 셋트
				userModel.UserDataSet = remoteData.UserDataSet.Copy();
				userModel.ResultCnt   = remoteData.ResultCnt;
				userModel.ResultCD    = remoteData.ResultCD;

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
		/// <param name="userModel"></param>
		public void SetUserUpdate(UserInfoModel userModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("사용자정보수정 Start");
				_log.Debug("-----------------------------------------");


				//입력데이터의 Validation 검사
				if(userModel.UserID.Length < 1) 
				{
					throw new FrameException("사용자ID가 존재하지 않습니다.");
				}
				if(userModel.UserDept.Length > 20) 
				{
					throw new FrameException("소속부서명의 길이는 20Bytes를 초과할 수 없습니다.");
				}
				if(userModel.UserTitle.Length > 20) 
				{
					throw new FrameException("직책직함명의 길이는 20Bytes를 초과할 수 없습니다.");
				}
				if(userModel.UserTell.Length > 15) 
				{
					throw new FrameException("전화번호의 길이는 15Bytes를 초과할 수 없습니다");
				}
				if(userModel.UserTell.Length > 15) 
				{
					throw new FrameException("휴대전화번호의 길이는 15Bytes를 초과할 수 없습니다.");
				}
				if(userModel.UserComment.Length > 50) 
				{
					throw new FrameException("비고란의 길이는 50Bytes를 초과할 수 없습니다.");
				}

	

				// 웹서비스 인스턴스 생성
				UserInfoServicePloxy.UserInfoService svc = new UserInfoServicePloxy.UserInfoService();
				svc.Url = _WebServiceUrl;
			
				// 리모트 모델 생성
				UserInfoServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.UserInfoServicePloxy.HeaderModel();
				UserInfoServicePloxy.UserInfoModel remoteData   = new AdManagerClient.UserInfoServicePloxy.UserInfoModel();

				// 헤더정보 셋트
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;

				// 호출정보셋트
				remoteData.UserID       = userModel.UserID;
				remoteData.UserName     = userModel.UserName;
				remoteData.UserPassword = userModel.UserPassword;
				remoteData.UserLevel    = userModel.UserLevel;
				remoteData.UserClass    = userModel.UserClass;
				remoteData.MediaCode    = userModel.MediaCode;
				remoteData.RapCode    = userModel.RapCode;
				remoteData.AgencyCode    = userModel.AgencyCode;
				remoteData.UserDept     = userModel.UserDept;
				remoteData.UserTitle    = userModel.UserTitle;
				remoteData.UserTell     = userModel.UserTell;
				remoteData.UserMobile   = userModel.UserMobile;
				remoteData.UserEMail   = userModel.UserEMail;
				remoteData.UserComment  = userModel.UserComment;
				remoteData.UseYn     = userModel.UseYn;				
				
                // 웹서비스 호출 타임아웃설정
                svc.Timeout = FrameSystem.m_SystemTimeout;

				// 웹서비스 메소드 호출
				remoteData = svc.SetUserUpdate(remoteHeader, remoteData);

				// 결과코드검사
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// 결과 셋트
				userModel.ResultCnt   = remoteData.ResultCnt;
				userModel.ResultCD    = remoteData.ResultCD;

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
		/// <param name="userModel"></param>
		/// <returns></returns>
		public void SetUserAdd(UserInfoModel userModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("사용자추가 Start");
				_log.Debug("-----------------------------------------");

				if(userModel.UserID.Trim().Length < 1) 
				{
					throw new FrameException("사용자ID가 존재하지 않습니다.");
				}
				if(userModel.UserID.Trim().Length > 10) 
				{
					throw new FrameException("사용자ID는 10Bytes를 초과할 수 없습니다.");
				}
				if(userModel.UserDept.Length > 20) 
				{
					throw new FrameException("소속부서명의 길이는 20Bytes를 초과할 수 없습니다.");
				}
				if(userModel.UserTitle.Length > 20) 
				{
					throw new FrameException("직책직함명의 길이는 20Bytes를 초과할 수 없습니다.");
				}
				if(userModel.UserTell.Length > 15) 
				{
					throw new FrameException("전화번호의 길이는 15Bytes를 초과할 수 없습니다");
				}
				if(userModel.UserTell.Length > 15) 
				{
					throw new FrameException("휴대전화번호의 길이는 15Bytes를 초과할 수 없습니다.");
				}
				if(userModel.UserComment.Length > 50) 
				{
					throw new FrameException("비고란의 길이는 50Bytes를 초과할 수 없습니다.");
				}


				// 웹서비스 인스턴스 생성
				UserInfoServicePloxy.UserInfoService svc = new UserInfoServicePloxy.UserInfoService();
				svc.Url = _WebServiceUrl;
				
				// 리모트 모델 생성
				UserInfoServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.UserInfoServicePloxy.HeaderModel();
				UserInfoServicePloxy.UserInfoModel remoteData   = new AdManagerClient.UserInfoServicePloxy.UserInfoModel();

				// 헤더정보 셋트
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;

				// 호출정보셋트
				remoteData.UserID       = userModel.UserID;
				remoteData.UserName     = userModel.UserName;
				remoteData.UserPassword = userModel.UserPassword;
				remoteData.UserLevel    = userModel.UserLevel;
				remoteData.UserClass    = userModel.UserClass;
				remoteData.MediaCode    = userModel.MediaCode;
				remoteData.RapCode    = userModel.RapCode;
				remoteData.AgencyCode    = userModel.AgencyCode;
				remoteData.UserDept     = userModel.UserDept;
				remoteData.UserTitle    = userModel.UserTitle;
				remoteData.UserTell     = userModel.UserTell;
				remoteData.UserMobile   = userModel.UserMobile;
				remoteData.UserEMail   = userModel.UserEMail;
				remoteData.UserComment  = userModel.UserComment;
				remoteData.UseYn     = userModel.UseYn;				
					
                // 웹서비스 호출 타임아웃설정
                svc.Timeout = FrameSystem.m_SystemTimeout;

				// 웹서비스 메소드 호출
				remoteData = svc.SetUserCreate(remoteHeader, remoteData);

				// 결과코드검사
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// 결과 셋트
				userModel.ResultCnt   = remoteData.ResultCnt;
				userModel.ResultCD    = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("사용자추가 End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":SetUserCreate():" + fe.ErrCode + ":" + fe.ResultMsg);
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
		public void SetUserDelete(UserInfoModel userModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("사용자삭제 Start");
				_log.Debug("-----------------------------------------");

				// 웹서비스 인스턴스 생성
				UserInfoServicePloxy.UserInfoService svc = new UserInfoServicePloxy.UserInfoService();
				svc.Url = _WebServiceUrl;
				
				// 리모트 모델 생성
				UserInfoServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.UserInfoServicePloxy.HeaderModel();
				UserInfoServicePloxy.UserInfoModel remoteData   = new AdManagerClient.UserInfoServicePloxy.UserInfoModel();

				// 헤더정보 셋트
				remoteHeader.ClientKey  = Header.ClientKey;
				remoteHeader.UserID     = Header.UserID;

				// 호출정보셋트
				remoteData.UserID       = userModel.UserID;
					
                // 웹서비스 호출 타임아웃설정
                svc.Timeout = FrameSystem.m_SystemTimeout;

				// 웹서비스 메소드 호출
				remoteData = svc.SetUserDelete(remoteHeader, remoteData);

				// 결과코드검사
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// 결과 셋트
				userModel.ResultCnt   = remoteData.ResultCnt;
				userModel.ResultCD    = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("사용자삭제 End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":SetUserDelete():" + fe.ErrCode + ":" + fe.ResultMsg);
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
