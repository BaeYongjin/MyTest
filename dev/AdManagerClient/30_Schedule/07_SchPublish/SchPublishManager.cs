// ===============================================================================
//
// SchPublishManager.cs
//
// 편성배포승인 서비스를 호출합니다. 
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
	/// 편성배포승인 웹서비스를 호출합니다. 
	/// </summary>
	public class SchPublishManager : BaseManager
	{
		/// <summary>
		/// 생성자
		/// </summary>
		/// <param name="systemModel"></param>
		/// <param name="commonModel"></param>

		public SchPublishManager(SystemModel systemModel, CommonModel commonModel) : base(systemModel, commonModel)
		{
			_log = FrameSystem.oLog;
			_module = "SchPublish";
			_Host  = FrameSystem.m_WebServer_Host;
			_Port  = FrameSystem.m_WebServer_Port;
			_Path  = FrameSystem.m_WebServer_App + "/Schedule/SchPublishService.asmx";
		}

		/// <summary>
		/// 승인이력 조회
		/// </summary>
		/// <param name="schPublishModel"></param>
		public void GetSchPublishList(SchPublishModel schPublishModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("승인이력 조회 Start");
				_log.Debug("-----------------------------------------");
				
				// 웹서비스 인스턴스 생성
				SchPublishServicePloxy.SchPublishService svc = new SchPublishServicePloxy.SchPublishService();
			
				// 웹서비스URL 동적 셋트
				svc.Url = _WebServiceUrl;

				// 리모트 모델 생성
				SchPublishServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.SchPublishServicePloxy.HeaderModel();
				SchPublishServicePloxy.SchPublishModel   remoteData   = new AdManagerClient.SchPublishServicePloxy.SchPublishModel();

				// 헤더정보 셋트
				remoteHeader.ClientKey		= Header.ClientKey;
				remoteHeader.UserID			= Header.UserID;
				remoteHeader.UserLevel		= Header.UserLevel;

				// 호출정보 셋트
				remoteData.SearchMediaCode	= schPublishModel.SearchMediaCode;	  				
				
				// 웹서비스 호출 타임아웃설정
				svc.Timeout = FrameSystem.m_SystemTimeout;
				// 웹서비스 메소드 호출
				remoteData = svc.GetSchPublishList(remoteHeader, remoteData);

				// 결과코드검사
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// 결과 셋트
				schPublishModel.PublishDataSet = remoteData.PublishDataSet.Copy();
				schPublishModel.ResultCnt   = remoteData.ResultCnt;
				schPublishModel.ResultCD    = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("승인이력 조회 End");
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
		/// 편성현황 조회
		/// </summary>
		/// <param name="schPublishModel"></param>
		public void GetScheduleList(SchPublishModel schPublishModel)
		{

			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("승인이력 조회 Start");
				_log.Debug("-----------------------------------------");
				
				// 웹서비스 인스턴스 생성
				SchPublishServicePloxy.SchPublishService svc = new SchPublishServicePloxy.SchPublishService();
			
				// 웹서비스URL 동적 셋트
				svc.Url = _WebServiceUrl;

				// 리모트 모델 생성
				SchPublishServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.SchPublishServicePloxy.HeaderModel();
				SchPublishServicePloxy.SchPublishModel   remoteData   = new AdManagerClient.SchPublishServicePloxy.SchPublishModel();

				// 헤더정보 셋트
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;
				remoteHeader.UserLevel     = Header.UserLevel;

				// 호출정보 셋트
				remoteData.SearchMediaCode		 = schPublishModel.SearchMediaCode;
	  			remoteData.AckNo				 = schPublishModel.AckNo;	
				
				// 웹서비스 호출 타임아웃설정
				svc.Timeout = FrameSystem.m_SystemTimeout;
				// 웹서비스 메소드 호출
				remoteData = svc.GetScheduleList(remoteHeader, remoteData);

				// 결과코드검사
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// 결과 셋트
				schPublishModel.ScheduleDataSet = remoteData.ScheduleDataSet.Copy();
				schPublishModel.ResultCnt   = remoteData.ResultCnt;
				schPublishModel.ResultCD    = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("승인이력 조회 End");
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
		/// [S1] 현재승인상태 조회
		/// 일단 10:상업광고(로딩,중간,엔딩), 나머지는 추후에 다시 생각해 보자
		/// </summary>
		/// <param name="schPublishModel"></param>
		/// <param name="adSchType">승인종류 [0]홈, [10]상업, [20]OAP, [30]홈(키즈), [40]홈(타겟) </param>
		public void GetPublishState(SchPublishModel schPublishModel, int adSchType)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("현재승인상태 조회 Start");
				_log.Debug("-----------------------------------------");
				
				// 웹서비스 인스턴스 생성
				SchPublishServicePloxy.SchPublishService svc = new SchPublishServicePloxy.SchPublishService();
			
				// 웹서비스URL 동적 셋트
				svc.Url = _WebServiceUrl;

				// 리모트 모델 생성
				SchPublishServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.SchPublishServicePloxy.HeaderModel();
				SchPublishServicePloxy.SchPublishModel   remoteData   = new AdManagerClient.SchPublishServicePloxy.SchPublishModel();

				// 헤더정보 셋트
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;
				remoteHeader.UserLevel     = Header.UserLevel;

				// 호출정보 셋트
				remoteData.SearchMediaCode		 = schPublishModel.SearchMediaCode;	  				
				
				// 웹서비스 호출 타임아웃설정
				svc.Timeout = FrameSystem.m_SystemTimeout;
				// 웹서비스 메소드 호출
				if(			adSchType.Equals(0) )		remoteData = svc.GetHomePublishState(remoteHeader, remoteData);
				else if(	adSchType.Equals(10))		remoteData = svc.GetCmPublishState(remoteHeader, remoteData);
				else if(	adSchType.Equals(20))		remoteData = svc.GetOapPublishState(remoteHeader, remoteData);
				else if(	adSchType.Equals(30))		remoteData = svc.GetHomeKidsPublishState(remoteHeader, remoteData);
                else if(    adSchType.Equals(40))       remoteData = svc.GetHomeTargetPublishState(remoteHeader, remoteData);
				else	throw new FrameException("승인종류가 잘못되었습니다", _module, "3000");

				// 결과코드검사
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// 결과 셋트
				schPublishModel.AckNo       = remoteData.AckNo;
				schPublishModel.State       = remoteData.State;

				schPublishModel.ResultCnt   = remoteData.ResultCnt;
				schPublishModel.ResultCD    = remoteData.ResultCD;

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
		/// 광고편성승인
		/// </summary>
		/// <param name="schPublishModel"></param>
		public void SetScheduleAck(SchPublishModel schPublishModel)
		{

			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("광고편성승인 Start");
				_log.Debug("-----------------------------------------");
				
				// 웹서비스 인스턴스 생성
				SchPublishServicePloxy.SchPublishService svc = new SchPublishServicePloxy.SchPublishService();
			
				// 웹서비스URL 동적 셋트
				svc.Url = _WebServiceUrl;

				// 리모트 모델 생성
				SchPublishServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.SchPublishServicePloxy.HeaderModel();
				SchPublishServicePloxy.SchPublishModel   remoteData   = new AdManagerClient.SchPublishServicePloxy.SchPublishModel();

				// 헤더정보 셋트
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;
				remoteHeader.UserLevel     = Header.UserLevel;

				// 호출정보 셋트
				remoteData.AckNo		 = schPublishModel.AckNo;	  				
				remoteData.AckDesc		 = schPublishModel.AckDesc;	  				
				
				// 웹서비스 호출 타임아웃설정
				svc.Timeout = FrameSystem.m_SystemTimeout;

				// 웹서비스 메소드 호출
				remoteData = svc.SetScheduleAck(remoteHeader, remoteData);

				// 결과코드검사
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// 결과 셋트
				schPublishModel.ResultCD    = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("광고편성승인 End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":SetScheduleAck():" + fe.ErrCode + ":" + fe.ResultMsg);
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
		/// 광고편성승인 취소
		/// </summary>
		/// <param name="schPublishModel"></param>
		public void SetScheduleAckCancel(SchPublishModel schPublishModel)
		{

			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("광고편성승인 취소 Start");
				_log.Debug("-----------------------------------------");
				
				// 웹서비스 인스턴스 생성
				SchPublishServicePloxy.SchPublishService svc = new SchPublishServicePloxy.SchPublishService();
			
				// 웹서비스URL 동적 셋트
				svc.Url = _WebServiceUrl;

				// 리모트 모델 생성
				SchPublishServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.SchPublishServicePloxy.HeaderModel();
				SchPublishServicePloxy.SchPublishModel   remoteData   = new AdManagerClient.SchPublishServicePloxy.SchPublishModel();

				// 헤더정보 셋트
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;
				remoteHeader.UserLevel     = Header.UserLevel;

				// 호출정보 셋트
				remoteData.AckNo		 = schPublishModel.AckNo;	  				
				remoteData.AckDesc		 = schPublishModel.AckDesc;	  				
				
				// 웹서비스 호출 타임아웃설정
				svc.Timeout = FrameSystem.m_SystemTimeout;

				// 웹서비스 메소드 호출
				remoteData = svc.SetScheduleAckCancel(remoteHeader, remoteData);

				// 결과코드검사
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// 결과 셋트
				schPublishModel.ResultCD    = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("광고편성승인 취소 End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":SetScheduleAckCancel():" + fe.ErrCode + ":" + fe.ResultMsg);
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
		/// 광고검수승인
		/// </summary>
		/// <param name="schPublishModel"></param>
		public void SetScheduleChk(SchPublishModel schPublishModel)
		{

			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("광고검수승인 Start");
				_log.Debug("-----------------------------------------");
				
				// 웹서비스 인스턴스 생성
				SchPublishServicePloxy.SchPublishService svc = new SchPublishServicePloxy.SchPublishService();
			
				// 웹서비스URL 동적 셋트
				svc.Url = _WebServiceUrl;

				// 리모트 모델 생성
				SchPublishServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.SchPublishServicePloxy.HeaderModel();
				SchPublishServicePloxy.SchPublishModel   remoteData   = new AdManagerClient.SchPublishServicePloxy.SchPublishModel();

				// 헤더정보 셋트
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;
				remoteHeader.UserLevel     = Header.UserLevel;

				// 호출정보 셋트
				remoteData.AckNo		 = schPublishModel.AckNo;	  				
				remoteData.ChkDesc		 = schPublishModel.ChkDesc;	  				
				
				// 웹서비스 호출 타임아웃설정
				svc.Timeout = FrameSystem.m_SystemTimeout;

				// 웹서비스 메소드 호출
				remoteData = svc.SetScheduleChk(remoteHeader, remoteData);

				// 결과코드검사
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// 결과 셋트
				schPublishModel.ResultCD    = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("광고검수승인 End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":SetScheduleChk():" + fe.ErrCode + ":" + fe.ResultMsg);
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
		/// 광고검수승인 취소
		/// </summary>
		/// <param name="schPublishModel"></param>
		public void SetScheduleChkCancel(SchPublishModel schPublishModel)
		{

			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("광고검수승인 취소 Start");
				_log.Debug("-----------------------------------------");
				
				// 웹서비스 인스턴스 생성
				SchPublishServicePloxy.SchPublishService svc = new SchPublishServicePloxy.SchPublishService();
			
				// 웹서비스URL 동적 셋트
				svc.Url = _WebServiceUrl;

				// 리모트 모델 생성
				SchPublishServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.SchPublishServicePloxy.HeaderModel();
				SchPublishServicePloxy.SchPublishModel   remoteData   = new AdManagerClient.SchPublishServicePloxy.SchPublishModel();

				// 헤더정보 셋트
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;
				remoteHeader.UserLevel     = Header.UserLevel;

				// 호출정보 셋트
				remoteData.AckNo		 = schPublishModel.AckNo;	  				
				remoteData.ChkDesc		 = schPublishModel.ChkDesc;	  				
				
				// 웹서비스 호출 타임아웃설정
				svc.Timeout = FrameSystem.m_SystemTimeout;

				// 웹서비스 메소드 호출
				remoteData = svc.SetScheduleChkCancel(remoteHeader, remoteData);

				// 결과코드검사
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// 결과 셋트
				schPublishModel.ResultCD    = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("광고검수승인 취소 End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":SetScheduleChkCancel():" + fe.ErrCode + ":" + fe.ResultMsg);
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
		/// 광고배포승인
		/// </summary>
		/// <param name="schPublishModel"></param>
		public void SetSchedulePublish(SchPublishModel schPublishModel)
		{

			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("광고배포승인 Start");
				_log.Debug("-----------------------------------------");
				
				// 웹서비스 인스턴스 생성
				SchPublishServicePloxy.SchPublishService svc = new SchPublishServicePloxy.SchPublishService();
			
				// 웹서비스URL 동적 셋트
				svc.Url = _WebServiceUrl;

				// 리모트 모델 생성
				SchPublishServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.SchPublishServicePloxy.HeaderModel();
				SchPublishServicePloxy.SchPublishModel   remoteData   = new AdManagerClient.SchPublishServicePloxy.SchPublishModel();

				// 헤더정보 셋트
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;
				remoteHeader.UserLevel     = Header.UserLevel;

				// 호출정보 셋트
				remoteData.AckNo		 = schPublishModel.AckNo;	  				
				remoteData.PublishDesc	 = schPublishModel.PublishDesc;	  				
				
				// 웹서비스 호출 타임아웃설정
				svc.Timeout = FrameSystem.m_SystemTimeout;

				// 웹서비스 메소드 호출
				remoteData = svc.SetSchedulePublish(remoteHeader, remoteData);

				// 결과코드검사
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// 결과 셋트
				schPublishModel.ResultCD    = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("광고배포승인 End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":SetSchedulePublish():" + fe.ErrCode + ":" + fe.ResultMsg);
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
