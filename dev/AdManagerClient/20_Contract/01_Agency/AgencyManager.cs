// ===============================================================================
// Agency Manager  for Charites Project
//
// AgencyManager.cs
//
// 대행사정보 서비스를 호출합니다. 
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
	/// 대행사정보 웹서비스를 호출합니다. 
	/// </summary>
	public class AgencyManager : BaseManager
	{
		/// <summary>
		/// 생성자
		/// </summary>
		/// <param name="systemModel"></param>
		/// <param name="commonModel"></param>

		public AgencyManager(SystemModel systemModel, CommonModel commonModel) : base(systemModel, commonModel)
		{
			_log = FrameSystem.oLog;
			_module = "Agency";
			_Host  = FrameSystem.m_WebServer_Host;
			_Port  = FrameSystem.m_WebServer_Port;
			_Path  = FrameSystem.m_WebServer_App + "/Contract/AgencyService.asmx";
		}

		/// <summary>
		/// 대행사정보조회
		/// </summary>
		/// <param name="userModel"></param>
		public void GetAgencyList(AgencyModel agencyModel)
		{

			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("대행사목록조회 Start");
				_log.Debug("-----------------------------------------");
				
				// 웹서비스 인스턴스 생성
				AgencyServicePloxy.AgencyService svc = new AgencyServicePloxy.AgencyService();
			
				// URL의 동적셋트
				svc.Url = _WebServiceUrl;

				// 리모트 모델 생성
				AgencyServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.AgencyServicePloxy.HeaderModel();
				AgencyServicePloxy.AgencyModel    remoteData   = new AdManagerClient.AgencyServicePloxy.AgencyModel();

				// 헤더정보 셋트
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;
				remoteHeader.UserLevel     = Header.UserLevel;
				remoteHeader.UserClass	   = Header.UserClass;

				// 호출정보 셋트
				remoteData.SearchKey       = agencyModel.SearchKey;
				remoteData.SearchAgencyType = agencyModel.SearchAgencyType;
				remoteData.SearchRap         = agencyModel.SearchRap;
				remoteData.SearchchkAdState_10	 = agencyModel.SearchchkAdState_10; 
				
                // 웹서비스 호출 타임아웃설정
                svc.Timeout = FrameSystem.m_SystemTimeout;

				// 웹서비스 메소드 호출
				remoteData = svc.GetAgencyList(remoteHeader, remoteData);

				// 결과코드검사
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// 결과 셋트
				agencyModel.AgencyDataSet = remoteData.AgencyDataSet.Copy();
				agencyModel.ResultCnt   = remoteData.ResultCnt;
				agencyModel.ResultCD    = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("대행사목록조회 End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":GetAgencyList():" + fe.ErrCode + ":" + fe.ResultMsg);
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
		public bool GetAgencyDetail(BaseModel baseModel)
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
		/// 대행사정보 수정
		/// </summary>
		/// <param name="userModel"></param>
		public void SetAgencyUpdate(AgencyModel agencyModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("대행사정보수정 Start");
				_log.Debug("-----------------------------------------");

				//입력데이터의 Validation 검사
				if(agencyModel.AgencyCode.Length < 1) 
				{
					throw new FrameException("대행사가 선택되지 않았습니다.");
				}
				if(agencyModel.AgencyName.Length > 40) 
				{
					throw new FrameException("대행사명의 길이는 40Bytes를 초과할 수 없습니다.");
				}
				if(agencyModel.Tell.Length > 15) 
				{
					throw new FrameException("전화번호의 길이는 50Bytes를 초과할 수 없습니다.");
				}
				if(agencyModel.Address.Length > 50) 
				{
					throw new FrameException("주소의 길이는 50Bytes를 초과할 수 없습니다.");
				}				
				if(agencyModel.Comment.Length > 50) 
				{
					throw new FrameException("비고란의 길이는 50Bytes를 초과할 수 없습니다.");
				}

				// 웹서비스 인스턴스 생성
				AgencyServicePloxy.AgencyService svc = new AgencyServicePloxy.AgencyService();
		
				// URL의 동적셋트
				svc.Url = _WebServiceUrl;
	
				// 리모트 모델 생성
				AgencyServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.AgencyServicePloxy.HeaderModel();
				AgencyServicePloxy.AgencyModel    remoteData   = new AdManagerClient.AgencyServicePloxy.AgencyModel();

				// 헤더정보 셋트
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;
				remoteHeader.UserLevel     = Header.UserLevel;

				// 호출정보셋트
				remoteData.AgencyCode	   = agencyModel.AgencyCode;
				remoteData.AgencyName      = agencyModel.AgencyName;
				remoteData.RapCode         = agencyModel.RapCode;
				remoteData.AgencyType      = agencyModel.AgencyType;
				remoteData.Address         = agencyModel.Address;
				remoteData.Tell            = agencyModel.Tell;
				remoteData.Comment         = agencyModel.Comment;
				remoteData.UseYn           = agencyModel.UseYn;

                // 웹서비스 호출 타임아웃설정
                svc.Timeout = FrameSystem.m_SystemTimeout;

				// 웹서비스 메소드 호출
				remoteData = svc.SetAgencyUpdate(remoteHeader, remoteData);

				// 결과코드검사
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// 결과 셋트
				agencyModel.ResultCnt   = remoteData.ResultCnt;
				agencyModel.ResultCD    = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("대행사정보수정 End");
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
		/// 대행사추가
		/// </summary>
		/// <param name="userModel"></param>
		/// <returns></returns>
		public void SetAgencyAdd(AgencyModel agencyModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("대행사추가 Start");
				_log.Debug("-----------------------------------------");

				//입력데이터의 Validation 검사
				if(agencyModel.AgencyName.Length > 40) 
				{
					throw new FrameException("대행사명의 길이는 40Bytes를 초과할 수 없습니다.");
				}
				if(agencyModel.Tell.Length > 15) 
				{
					throw new FrameException("전화번호의 길이는 50Bytes를 초과할 수 없습니다.");
				}
				if(agencyModel.Address.Length > 50) 
				{
					throw new FrameException("주소의 길이는 50Bytes를 초과할 수 없습니다.");
				}				
				if(agencyModel.Comment.Length > 50) 
				{
					throw new FrameException("비고란의 길이는 50Bytes를 초과할 수 없습니다.");
				}

	

				// 웹서비스 인스턴스 생성
				AgencyServicePloxy.AgencyService svc = new AgencyServicePloxy.AgencyService();
			
				// URL의 동적셋트
				svc.Url = _WebServiceUrl;

				// 리모트 모델 생성
				AgencyServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.AgencyServicePloxy.HeaderModel();
				AgencyServicePloxy.AgencyModel    remoteData   = new AdManagerClient.AgencyServicePloxy.AgencyModel();

				// 헤더정보 셋트
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;
				remoteHeader.UserLevel     = Header.UserLevel;

				// 호출정보셋트
				remoteData.AgencyCode	   = agencyModel.AgencyCode;
				remoteData.AgencyName       = agencyModel.AgencyName;
				remoteData.RapCode        = agencyModel.RapCode;
				remoteData.AgencyType       = agencyModel.AgencyType;
				remoteData.Address         = agencyModel.Address;
				remoteData.Tell            = agencyModel.Tell;
				remoteData.Comment         = agencyModel.Comment;
				remoteData.UseYn           = agencyModel.UseYn;

                // 웹서비스 호출 타임아웃설정
                svc.Timeout = FrameSystem.m_SystemTimeout;

				// 웹서비스 메소드 호출
				remoteData = svc.SetAgencyCreate(remoteHeader, remoteData);

				// 결과코드검사
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// 결과 셋트
				agencyModel.ResultCnt   = remoteData.ResultCnt;
				agencyModel.ResultCD    = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("대행사추가 End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":SetAgebtCreate():" + fe.ErrCode + ":" + fe.ResultMsg);
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
		/// 대행사 삭제
		/// </summary>
		/// <param name="baseModel"></param>
		public void SetAgencyDelete(AgencyModel agencyModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("대행사삭제 Start");
				_log.Debug("-----------------------------------------");


				//입력데이터의 Validation 검사
				if(agencyModel.AgencyCode.Length < 1) 
				{
					throw new FrameException("대행사가 선택되지 않았습니다.");
				}

				// 웹서비스 인스턴스 생성
				AgencyServicePloxy.AgencyService svc = new AgencyServicePloxy.AgencyService();

				// URL의 동적셋트
				svc.Url = _WebServiceUrl;
			
				// 리모트 모델 생성
				AgencyServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.AgencyServicePloxy.HeaderModel();
				AgencyServicePloxy.AgencyModel    remoteData   = new AdManagerClient.AgencyServicePloxy.AgencyModel();

				// 헤더정보 셋트
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;
				remoteHeader.UserLevel     = Header.UserLevel;

				// 호출정보셋트
				remoteData.AgencyCode	   = agencyModel.AgencyCode;
				
                // 웹서비스 호출 타임아웃설정
                svc.Timeout = FrameSystem.m_SystemTimeout;

				// 웹서비스 메소드 호출
				remoteData = svc.SetAgencyDelete(remoteHeader, remoteData);

				// 결과코드검사
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// 결과 셋트
				agencyModel.ResultCnt   = remoteData.ResultCnt;
				agencyModel.ResultCD    = remoteData.ResultCD;


				_log.Debug("-----------------------------------------");
				_log.Debug("대행사삭제 End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":SetAgencyDelete():" + fe.ErrCode + ":" + fe.ResultMsg);
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
