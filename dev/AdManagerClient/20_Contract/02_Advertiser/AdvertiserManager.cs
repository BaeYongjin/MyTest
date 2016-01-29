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
	public class AdvertiserManager : BaseManager
	{
		/// <summary>
		/// 생성자
		/// </summary>
		/// <param name="systemModel"></param>
		/// <param name="commonModel"></param>

		public AdvertiserManager(SystemModel systemModel, CommonModel commonModel) : base(systemModel, commonModel)
		{
			_log = FrameSystem.oLog;
			_module = "USERINFO";
			_Host  = FrameSystem.m_WebServer_Host;
			_Port  = FrameSystem.m_WebServer_Port;
			_Path  = FrameSystem.m_WebServer_App + "/Contract/AdvertiserService.asmx";
		}

		/// <summary>
		/// 사용자정보조회
		/// </summary>
		/// <param name="advertiserModel"></param>
		public void GetAdvertiserList(AdvertiserModel advertiserModel)
		{

			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("사용자목록조회 Start");
				_log.Debug("-----------------------------------------");
				
				// 웹서비스 인스턴스 생성
				AdvertiserServicePloxy.AdvertiserService svc = new AdvertiserServicePloxy.AdvertiserService();
			
				// URL의 동적셋트
				svc.Url = _WebServiceUrl;

				// 리모트 모델 생성
				AdvertiserServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.AdvertiserServicePloxy.HeaderModel();
				AdvertiserServicePloxy.AdvertiserModel remoteData   = new AdManagerClient.AdvertiserServicePloxy.AdvertiserModel();

				// 헤더정보 셋트
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;
				remoteHeader.UserLevel     = Header.UserLevel;
				remoteHeader.UserClass	   = Header.UserClass;

				// 호출정보 셋트
				remoteData.SearchKey       = advertiserModel.SearchKey;
				remoteData.SearchAdvertiserLevel = advertiserModel.SearchAdvertiserLevel;
				remoteData.SearchchkAdState_10	 = advertiserModel.SearchchkAdState_10; 
				remoteData.SearchRap         = advertiserModel.SearchRap;
				
                // 웹서비스 호출 타임아웃설정
                svc.Timeout = FrameSystem.m_SystemTimeout;

				// 웹서비스 메소드 호출
				remoteData = svc.GetAdvertiserList(remoteHeader, remoteData);

				// 결과코드검사
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// 결과 셋트
				advertiserModel.AdvertiserDataSet = remoteData.AdvertiserDataSet.Copy();
				advertiserModel.ResultCnt   = remoteData.ResultCnt;
				advertiserModel.ResultCD    = remoteData.ResultCD;

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
		/// <param name="advertiserModel"></param>
		public void SetAdvertiserUpdate(AdvertiserModel advertiserModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("사용자정보수정 Start");
				_log.Debug("-----------------------------------------");


				//입력데이터의 Validation 검사
				/*if(advertiserModel.MediaCode.Length < 1) 
				{
					throw new FrameException("사용자ID가 존재하지 않습니다.");
				}*/
				if(advertiserModel.AdvertiserName.Length > 40) 
				{
					throw new FrameException("광고주명은 40Bytes를 초과할 수 없습니다.");
				}
				if(advertiserModel.Comment.Length > 50) 
				{
					throw new FrameException("비고란의 길이는 50Bytes를 초과할 수 없습니다.");
				}
				/*if(advertiserModel.Tell.Length > 15) 
				{
					throw new FrameException("전화번호의 길이는 15Bytes를 초과할 수 없습니다");
				}
				if(advertiserModel.Email.Length > 40) 
				{
					throw new FrameException("Email의 길이는 40Bytes를 초과할 수 없습니다.");
				}*/
				

	

				// 웹서비스 인스턴스 생성
				AdvertiserServicePloxy.AdvertiserService svc = new AdvertiserServicePloxy.AdvertiserService();
			
				// URL의 동적셋트
				svc.Url = _WebServiceUrl;

				// 리모트 모델 생성
				AdvertiserServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.AdvertiserServicePloxy.HeaderModel();
				AdvertiserServicePloxy.AdvertiserModel remoteData   = new AdManagerClient.AdvertiserServicePloxy.AdvertiserModel();

				// 헤더정보 셋트
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;

				// 호출정보셋트
				//remoteData.MediaCode      = advertiserModel.MediaCode;
				remoteData.AdvertiserCode   = advertiserModel.AdvertiserCode;
				remoteData.AdvertiserName   = advertiserModel.AdvertiserName;		
				remoteData.RapCode          = advertiserModel.RapCode;
                remoteData.JobCode          = advertiserModel.JobCode;
				remoteData.Comment          = advertiserModel.Comment;				
				remoteData.UseYn            = advertiserModel.UseYn;				
				remoteData.ModDt            = advertiserModel.ModDt;
								
                // 웹서비스 호출 타임아웃설정
                svc.Timeout = FrameSystem.m_SystemTimeout;

				// 웹서비스 메소드 호출
				remoteData = svc.SetAdvertiserUpdate(remoteHeader, remoteData);

				// 결과코드검사
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// 결과 셋트
				advertiserModel.ResultCnt   = remoteData.ResultCnt;
				advertiserModel.ResultCD    = remoteData.ResultCD;

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
		/// <param name="advertiserModel"></param>
		/// <returns></returns>
		public void SetAdvertiserAdd(AdvertiserModel advertiserModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("사용자추가 Start");
				_log.Debug("-----------------------------------------");

				/*if(advertiserModel.MediaCode.Trim().Length < 1) 
				{
					throw new FrameException("매체코드가 존재하지 않습니다.");
				}
				if(advertiserModel.MediaCode.Trim().Length > 10) 
				{
					throw new FrameException("매체코드는 10Bytes를 초과할 수 없습니다.");
				}*/
				if(advertiserModel.AdvertiserName.Length > 40) 
				{
					throw new FrameException("광고주명은 40Bytes를 초과할 수 없습니다.");
				}
				if(advertiserModel.Comment.Length > 50) 
				{
					throw new FrameException("비고란의 길이는 50Bytes를 초과할 수 없습니다.");
				}
				/*if(advertiserModel.Tell.Length > 15) 
				{
					throw new FrameException("전화번호의 길이는 15Bytes를 초과할 수 없습니다");
				}				
				if(advertiserModel.Email.Length > 40) 
				{
					throw new FrameException("Email은 40Bytes를 초과할 수 없습니다.");
				}*/


				// 웹서비스 인스턴스 생성
				AdvertiserServicePloxy.AdvertiserService svc = new AdvertiserServicePloxy.AdvertiserService();
				
				// URL의 동적셋트
				svc.Url = _WebServiceUrl;

				// 리모트 모델 생성
				AdvertiserServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.AdvertiserServicePloxy.HeaderModel();
				AdvertiserServicePloxy.AdvertiserModel remoteData   = new AdManagerClient.AdvertiserServicePloxy.AdvertiserModel();

				// 헤더정보 셋트
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;

				// 호출정보셋트
				//remoteData.MediaCode      = advertiserModel.MediaCode;
				remoteData.AdvertiserCode   = advertiserModel.AdvertiserCode;
				remoteData.AdvertiserName   = advertiserModel.AdvertiserName;	
				remoteData.RapCode          = advertiserModel.RapCode;
                remoteData.JobCode          = advertiserModel.JobCode;
				remoteData.Comment          = advertiserModel.Comment;				
				remoteData.UseYn            = advertiserModel.UseYn;
				remoteData.RegDt            = advertiserModel.RegDt;
				
					
                // 웹서비스 호출 타임아웃설정
                svc.Timeout = FrameSystem.m_SystemTimeout;

				// 웹서비스 메소드 호출
				remoteData = svc.SetAdvertiserCreate(remoteHeader, remoteData);

				// 결과코드검사
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// 결과 셋트
				advertiserModel.ResultCnt   = remoteData.ResultCnt;
				advertiserModel.ResultCD    = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("사용자추가 End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				advertiserModel.ResultCD    = "3101";
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":SetUserCreate():" + fe.ErrCode + ":" + fe.ResultMsg);
				_log.Warning("-----------------------------------------");
				throw fe;
			}
			catch(Exception e)
			{
				advertiserModel.ResultCD    = "3101";
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
		public void SetAdvertiserDelete(AdvertiserModel advertiserModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("사용자삭제 Start");
				_log.Debug("-----------------------------------------");

				// 웹서비스 인스턴스 생성
				AdvertiserServicePloxy.AdvertiserService svc = new AdvertiserServicePloxy.AdvertiserService();
				
				// URL의 동적셋트
				svc.Url = _WebServiceUrl;

				// 리모트 모델 생성
				AdvertiserServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.AdvertiserServicePloxy.HeaderModel();
				AdvertiserServicePloxy.AdvertiserModel remoteData   = new AdManagerClient.AdvertiserServicePloxy.AdvertiserModel();

				// 헤더정보 셋트
				remoteHeader.ClientKey  = Header.ClientKey;
				remoteHeader.UserID     = Header.UserID;

				// 호출정보셋트
				//remoteData.MediaCode       = advertiserModel.MediaCode;
				remoteData.AdvertiserCode       = advertiserModel.AdvertiserCode;
					
                // 웹서비스 호출 타임아웃설정
                svc.Timeout = FrameSystem.m_SystemTimeout;

				// 웹서비스 메소드 호출
				remoteData = svc.SetAdvertiserDelete(remoteHeader, remoteData);

				// 결과코드검사
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// 결과 셋트
				advertiserModel.ResultCnt   = remoteData.ResultCnt;
				advertiserModel.ResultCD    = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("사용자삭제 End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":SetAdvertiserDelete():" + fe.ErrCode + ":" + fe.ResultMsg);
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
        /// 업종조회
        /// </summary>
        /// <param name="contractModel"></param>
        public void GetJobClassList(AdvertiserModel model)
        {
            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug("업종목록조회 Start");
                _log.Debug("-----------------------------------------");

                // 웹서비스 인스턴스 생성
                AdvertiserServicePloxy.AdvertiserService svc = new AdvertiserServicePloxy.AdvertiserService();

                svc.Url = _WebServiceUrl;

                // 리모트 모델 생성
                AdvertiserServicePloxy.HeaderModel remoteHeader = new AdvertiserServicePloxy.HeaderModel();
                AdvertiserServicePloxy.AdvertiserModel remoteData = new AdvertiserServicePloxy.AdvertiserModel();

                // 헤더정보 셋트
                remoteHeader.ClientKey = Header.ClientKey;
                remoteHeader.UserID = Header.UserID;

                // 웹서비스 호출 타임아웃설정
                svc.Timeout = FrameSystem.m_SystemTimeout;

                // 웹서비스 메소드 호출
                remoteData = svc.GetJobClassList(remoteHeader, remoteData);

                // 결과코드검사
                if (!remoteData.ResultCD.Equals("0000"))
                {
                    throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
                }

                // 결과 셋트
                model.AdvertiserDataSet = remoteData.AdvertiserDataSet.Copy();
                model.ResultCnt = remoteData.ResultCnt;
                model.ResultCD  = remoteData.ResultCD;

                _log.Debug("-----------------------------------------");
                _log.Debug("업종목록조회 End");
                _log.Debug("-----------------------------------------");
            }
            catch (FrameException fe)
            {
                _log.Warning("-----------------------------------------");
                _log.Warning(this.ToString() + ":GetJobClassList():" + fe.ErrCode + ":" + fe.ResultMsg);
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
	}
}
