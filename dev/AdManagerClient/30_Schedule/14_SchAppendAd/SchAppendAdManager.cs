// ===============================================================================
// SchAppendAd Manager  for Charites Project
//
// SchAppendAdManager.cs
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

using System.Diagnostics;
using WinFramework.Base;
using WinFramework.Data;
using WinFramework.Misc;

using AdManagerModel;

namespace AdManagerClient
{
	/// <summary>
	/// 홈광고편성정보 웹서비스를 호출합니다. 
	/// </summary>
	public class SchAppendAdManager : BaseManager
	{

		// 순위변경구분
		const int ORDER_FIRST = 1;
		const int ORDER_LAST  = 2;
		const int ORDER_UP    = 3;
		const int ORDER_DOWN  = 4;


		/// <summary>
		/// 생성자
		/// </summary>
		/// <param name="systemModel"></param>
		/// <param name="commonModel"></param>

		public SchAppendAdManager(SystemModel systemModel, CommonModel commonModel) : base(systemModel, commonModel)
		{
			_log = FrameSystem.oLog;
			_module = "SchAppendAd";
			_Host  = FrameSystem.m_WebServer_Host;
			_Port  = FrameSystem.m_WebServer_Port;
			_Path  = FrameSystem.m_WebServer_App + "/Schedule/SchAppendAdService.asmx";
		}

		/// <summary>
		/// 홈광고편성현황조회
		/// </summary>
		/// <param name="schAppendAdModel"></param>
		public void GetSchAppendAdList(SchAppendAdModel schAppendAdModel)
		{

			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("홈광고편성현황조회 Start");
				_log.Debug("-----------------------------------------");
				
				// 웹서비스 인스턴스 생성
				SchAppendAdServicePloxy.SchAppendAdService svc = new SchAppendAdServicePloxy.SchAppendAdService();

				// 웹서비스 URL동적 생성
				svc.Url = _WebServiceUrl;			

				// 리모트 모델 생성
				SchAppendAdServicePloxy.HeaderModel      remoteHeader = new AdManagerClient.SchAppendAdServicePloxy.HeaderModel();
				SchAppendAdServicePloxy.SchAppendAdModel   remoteData   = new AdManagerClient.SchAppendAdServicePloxy.SchAppendAdModel();

				// 헤더정보 셋트
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;
				remoteHeader.UserLevel     = Header.UserLevel;
				remoteHeader.UserClass     = Header.UserClass;

				// 호출정보 셋트
				remoteData.SearchMediaCode		 =  schAppendAdModel.SearchMediaCode;	  

				// 웹서비스 호출 타임아웃설정
				svc.Timeout = FrameSystem.m_SystemTimeout;
				// 웹서비스 메소드 호출
				remoteData = svc.GetSchAppendAdList(remoteHeader, remoteData);

				// 결과코드검사
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// 결과 셋트
				schAppendAdModel.FileListCount = remoteData.FileListCount;  // 2007.10.02 RH.Jung 파일리스트건수검사

				schAppendAdModel.ScheduleDataSet = remoteData.ScheduleDataSet.Copy();
				schAppendAdModel.LastOrder   = remoteData.LastOrder;
				schAppendAdModel.ResultCnt   = remoteData.ResultCnt;
				schAppendAdModel.ResultCD    = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("광고파일목록조회 End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":GetSchAppendAdList():" + fe.ErrCode + ":" + fe.ResultMsg);
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
		/// 홈광고편성대상조회
		/// </summary>
		/// <param name="schAppendAdModel"></param>
		public void GetContractItemList(SchAppendAdModel schAppendAdModel)
		{

			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("홈광고편성대상조회 Start");
				_log.Debug("-----------------------------------------");
				
				// 웹서비스 인스턴스 생성
				SchAppendAdServicePloxy.SchAppendAdService svc = new SchAppendAdServicePloxy.SchAppendAdService();
				
				// 웹서비스 URL동적 생성
				svc.Url = _WebServiceUrl;			

				// 리모트 모델 생성
				SchAppendAdServicePloxy.HeaderModel      remoteHeader = new AdManagerClient.SchAppendAdServicePloxy.HeaderModel();
				SchAppendAdServicePloxy.SchAppendAdModel   remoteData   = new AdManagerClient.SchAppendAdServicePloxy.SchAppendAdModel();

				// 헤더정보 셋트
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;
				remoteHeader.UserLevel     = Header.UserLevel;
				remoteHeader.UserClass     = Header.UserClass;

				// 호출정보 셋트
				remoteData.SearchKey             =  schAppendAdModel.SearchKey;               
				remoteData.SearchMediaCode		 =  schAppendAdModel.SearchMediaCode;	  
				remoteData.SearchRapCode		 =  schAppendAdModel.SearchRapCode;       
				remoteData.SearchAgencyCode	     =  schAppendAdModel.SearchAgencyCode;    
				remoteData.SearchAdvertiserCode  =  schAppendAdModel.SearchAdvertiserCode;
				remoteData.SearchchkAdState_10	 =  schAppendAdModel.SearchchkAdState_10; 
				remoteData.SearchchkAdState_20	 =  schAppendAdModel.SearchchkAdState_20; 
				remoteData.SearchchkAdState_30	 =  schAppendAdModel.SearchchkAdState_30; 
				remoteData.SearchchkAdState_40	 =  schAppendAdModel.SearchchkAdState_40; 

				// 웹서비스 호출 타임아웃설정
				svc.Timeout = FrameSystem.m_SystemTimeout;
				// 웹서비스 메소드 호출
				remoteData = svc.GetContractItemList(remoteHeader, remoteData);

				// 결과코드검사
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// 결과 셋트

				schAppendAdModel.FileListCount = remoteData.FileListCount;  // 2007.10.02 RH.Jung 파일리스트건수검사

				schAppendAdModel.ScheduleDataSet = remoteData.ScheduleDataSet.Copy();
				schAppendAdModel.ResultCnt   = remoteData.ResultCnt;
				schAppendAdModel.ResultCD    = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("광고파일목록조회 End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":GetContractItemList():" + fe.ErrCode + ":" + fe.ResultMsg);
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
		/// 홈광고 조회(엑셀파일을 셀렉트하여 DB에 없는 데이터만 인서트 하기위해...홈 테이블 셀렉트)
		/// </summary>
		/// <param name="schAppendAdModel"></param>
		/// <returns></returns>
		public void SetSchAppendList(SchAppendAdModel schAppendAdModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("홈광고조회 Start");
				_log.Debug("-----------------------------------------");

				//입력데이터의 Validation 검사
				//				if(schAppendAdModel.ItemNo.Length < 1) 
				//				{
				//					throw new FrameException("광고가 선택되지 않았습니다.");
				//				}

				// 웹서비스 인스턴스 생성
				SchAppendAdServicePloxy.SchAppendAdService svc = new SchAppendAdServicePloxy.SchAppendAdService();
			
				// 웹서비스 URL동적 생성
				svc.Url = _WebServiceUrl;			

				// 리모트 모델 생성
				SchAppendAdServicePloxy.HeaderModel      remoteHeader = new AdManagerClient.SchAppendAdServicePloxy.HeaderModel();
				SchAppendAdServicePloxy.SchAppendAdModel   remoteData   = new AdManagerClient.SchAppendAdServicePloxy.SchAppendAdModel();

				// 헤더정보 셋트
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID; 
				remoteHeader.UserLevel     = Header.UserLevel;
				remoteHeader.UserClass     = Header.UserClass;

				// 호출정보 셋트
				//				remoteData.MediaCode       =  schAppendAdModel.MediaCode;
				//				remoteData.ItemNo          =  schAppendAdModel.ItemNo;
				//				remoteData.ItemName        =  schAppendAdModel.ItemName;

				// 웹서비스 호출 타임아웃설정
				svc.Timeout = FrameSystem.m_SystemTimeout;
				// 웹서비스 메소드 호출
				remoteData = svc.SetSchAppendSearch(remoteHeader, remoteData);

				// 결과코드검사
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// 결과 셋트
				//schAppendAdModel.ScheduleOrder = remoteData.ScheduleOrder;
				schAppendAdModel.ScheduleDataSet = remoteData.ScheduleDataSet.Copy();
				schAppendAdModel.ResultCnt     = remoteData.ResultCnt;
				schAppendAdModel.ResultCD      = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("홈광고조회 End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":SetSchAppendList():" + fe.ErrCode + ":" + fe.ResultMsg);
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
		/// 홈광고편성추가
		/// </summary>
		/// <param name="schAppendAdModel"></param>
		/// <returns></returns>
		public void SetSchAppendAdAdd(SchAppendAdModel schAppendAdModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("홈광고편성추가 Start");
				_log.Debug("-----------------------------------------");

				//입력데이터의 Validation 검사
				if(schAppendAdModel.ItemNo.Length < 1) 
				{
					throw new FrameException("광고가 선택되지 않았습니다.");
				}

				// 웹서비스 인스턴스 생성
				SchAppendAdServicePloxy.SchAppendAdService svc = new SchAppendAdServicePloxy.SchAppendAdService();
			
				// 웹서비스 URL동적 생성
				svc.Url = _WebServiceUrl;			

				// 리모트 모델 생성
				SchAppendAdServicePloxy.HeaderModel      remoteHeader = new AdManagerClient.SchAppendAdServicePloxy.HeaderModel();
				SchAppendAdServicePloxy.SchAppendAdModel   remoteData   = new AdManagerClient.SchAppendAdServicePloxy.SchAppendAdModel();

				// 헤더정보 셋트
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID; 
				remoteHeader.UserLevel     = Header.UserLevel;
				remoteHeader.UserClass     = Header.UserClass;

				// 호출정보 셋트
				remoteData.MediaCode       =  schAppendAdModel.MediaCode;
				remoteData.ItemNo          =  schAppendAdModel.ItemNo;
				remoteData.ItemName        =  schAppendAdModel.ItemName;

				// 웹서비스 호출 타임아웃설정
				svc.Timeout = FrameSystem.m_SystemTimeout;
				// 웹서비스 메소드 호출
				remoteData = svc.SetSchAppendAdCreate(remoteHeader, remoteData);

				// 결과코드검사
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// 결과 셋트
				schAppendAdModel.ScheduleOrder = remoteData.ScheduleOrder;
				schAppendAdModel.ResultCnt     = remoteData.ResultCnt;
				schAppendAdModel.ResultCD      = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("홈광고편성추가 End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":SetSchAppendAdAdd():" + fe.ErrCode + ":" + fe.ResultMsg);
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
		/// 홈광고편성 삭제
		/// </summary>
		/// <param name="schAppendAdModel"></param>
		/// <returns></returns>
		public void SetSchAppendAdDelete_To(SchAppendAdModel schAppendAdModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("홈광고편성삭제 Start");
				_log.Debug("-----------------------------------------");

				//입력데이터의 Validation 검사
				//				if(schAppendAdModel.ItemNo.Length < 1) 
				//				{
				//					throw new FrameException("광고가 선택되지 않았습니다.");
				//				}

				// 웹서비스 인스턴스 생성
				SchAppendAdServicePloxy.SchAppendAdService svc = new SchAppendAdServicePloxy.SchAppendAdService();
			
				// 웹서비스 URL동적 생성
				svc.Url = _WebServiceUrl;			

				// 리모트 모델 생성
				SchAppendAdServicePloxy.HeaderModel      remoteHeader = new AdManagerClient.SchAppendAdServicePloxy.HeaderModel();
				SchAppendAdServicePloxy.SchAppendAdModel   remoteData   = new AdManagerClient.SchAppendAdServicePloxy.SchAppendAdModel();

				// 헤더정보 셋트
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID; 
				remoteHeader.UserLevel     = Header.UserLevel;
				remoteHeader.UserClass     = Header.UserClass;

				// 호출정보 셋트
				remoteData.MediaCode       =  schAppendAdModel.MediaCode;
				
				// 웹서비스 호출 타임아웃설정
				svc.Timeout = FrameSystem.m_SystemTimeout;
				// 웹서비스 메소드 호출
				remoteData = svc.SetSchAppendAdDelete_To(remoteHeader, remoteData);

				// 결과코드검사
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// 결과 셋트
				schAppendAdModel.ScheduleOrder = remoteData.ScheduleOrder;
				schAppendAdModel.ResultCnt     = remoteData.ResultCnt;
				schAppendAdModel.ResultCD      = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("홈광고편성추가 End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":SetSchAppendAdDelete_To():" + fe.ErrCode + ":" + fe.ResultMsg);
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
		/// 홈광고편성 추가
		/// </summary>
		/// <param name="schAppendAdModel"></param>
		/// <returns></returns>
		public void SetSchAppendAdCreate_To(SchAppendAdModel schAppendAdModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("홈광고편성추가 Start");
				_log.Debug("-----------------------------------------");

				//입력데이터의 Validation 검사
				//				if(schAppendAdModel.ItemNo.Length < 1) 
				//				{
				//					throw new FrameException("광고가 선택되지 않았습니다.");
				//				}

				// 웹서비스 인스턴스 생성
				SchAppendAdServicePloxy.SchAppendAdService svc = new SchAppendAdServicePloxy.SchAppendAdService();
			
				// 웹서비스 URL동적 생성
				svc.Url = _WebServiceUrl;			

				// 리모트 모델 생성
				SchAppendAdServicePloxy.HeaderModel      remoteHeader = new AdManagerClient.SchAppendAdServicePloxy.HeaderModel();
				SchAppendAdServicePloxy.SchAppendAdModel   remoteData   = new AdManagerClient.SchAppendAdServicePloxy.SchAppendAdModel();

				// 헤더정보 셋트
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID; 
				remoteHeader.UserLevel     = Header.UserLevel;
				remoteHeader.UserClass     = Header.UserClass;

				// 호출정보 셋트
				remoteData.MediaCode       =  schAppendAdModel.MediaCode;
				remoteData.ItemNo          =  schAppendAdModel.ItemNo;
				remoteData.ScheduleOrder        =  schAppendAdModel.ScheduleOrder;

				// 웹서비스 호출 타임아웃설정
				svc.Timeout = FrameSystem.m_SystemTimeout;
				// 웹서비스 메소드 호출
				remoteData = svc.SetSchAppendAdCreate_To(remoteHeader, remoteData);

				// 결과코드검사
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// 결과 셋트
				schAppendAdModel.ScheduleOrder = remoteData.ScheduleOrder;
				schAppendAdModel.ResultCnt     = remoteData.ResultCnt;
				schAppendAdModel.ResultCD      = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("홈광고편성추가 End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":SetSchAppendAd_DeleteCreate():" + fe.ErrCode + ":" + fe.ResultMsg);
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
		/// 홈광고편성삭제
		/// </summary>
		/// <param name="schAppendAdModel"></param>
		/// <returns></returns>
		public void SetSchAppendAdDelete(SchAppendAdModel schAppendAdModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("홈광고편성삭제 Start");
				_log.Debug("-----------------------------------------");

				//입력데이터의 Validation 검사
				if(schAppendAdModel.ItemNo.Length < 1) 
				{
					throw new FrameException("광고내역이 선택되지 않았습니다.");
				}

				// 웹서비스 인스턴스 생성
				SchAppendAdServicePloxy.SchAppendAdService svc = new SchAppendAdServicePloxy.SchAppendAdService();
			
				// 웹서비스 URL동적 생성
				svc.Url = _WebServiceUrl;			

				// 리모트 모델 생성
				SchAppendAdServicePloxy.HeaderModel      remoteHeader = new AdManagerClient.SchAppendAdServicePloxy.HeaderModel();
				SchAppendAdServicePloxy.SchAppendAdModel   remoteData   = new AdManagerClient.SchAppendAdServicePloxy.SchAppendAdModel();

				// 헤더정보 셋트
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID; 
				remoteHeader.UserLevel     = Header.UserLevel;
				remoteHeader.UserClass     = Header.UserClass;

				// 호출정보 셋트

				remoteData.MediaCode       =  schAppendAdModel.MediaCode;
				remoteData.ItemNo          =  schAppendAdModel.ItemNo;
				remoteData.ItemName        =  schAppendAdModel.ItemName;
				remoteData.ScheduleOrder   =  schAppendAdModel.ScheduleOrder;
 
				// 웹서비스 호출 타임아웃설정
				svc.Timeout = FrameSystem.m_SystemTimeout;
				// 웹서비스 메소드 호출
				remoteData = svc.SetSchAppendAdDelete(remoteHeader, remoteData);

				// 결과코드검사
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// 결과 셋트
				schAppendAdModel.ScheduleOrder = remoteData.ScheduleOrder;
				schAppendAdModel.ResultCnt     = remoteData.ResultCnt;
				schAppendAdModel.ResultCD      = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("홈광고편성삭제 End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":SetSchAppendAdDelete():" + fe.ErrCode + ":" + fe.ResultMsg);
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
		/// 홈광고편성 첫번째 순위 변경
		/// </summary>
		/// <param name="schAppendAdModel"></param>
		/// <returns></returns>
		public void SetSchAppendAdOrderSet(SchAppendAdModel schAppendAdModel, int OrderSet)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("홈광고편성 순위 변경 Start");
				_log.Debug("-----------------------------------------");

				//입력데이터의 Validation 검사
				if(schAppendAdModel.ItemNo.Length < 1) 
				{
					throw new FrameException("광고내역이 선택되지 않았습니다.");
				}

				// 웹서비스 인스턴스 생성
				SchAppendAdServicePloxy.SchAppendAdService svc = new SchAppendAdServicePloxy.SchAppendAdService();
	
				// 웹서비스 URL동적 생성
				svc.Url = _WebServiceUrl;			

				// 리모트 모델 생성
				SchAppendAdServicePloxy.HeaderModel      remoteHeader = new AdManagerClient.SchAppendAdServicePloxy.HeaderModel();
				SchAppendAdServicePloxy.SchAppendAdModel   remoteData   = new AdManagerClient.SchAppendAdServicePloxy.SchAppendAdModel();

				// 헤더정보 셋트
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID; 
				remoteHeader.UserLevel     = Header.UserLevel;
				remoteHeader.UserClass     = Header.UserClass;

				// 호출정보 셋트

				remoteData.MediaCode       =  schAppendAdModel.MediaCode;
				remoteData.ItemNo          =  schAppendAdModel.ItemNo;
				remoteData.ItemName        =  schAppendAdModel.ItemName;
				remoteData.ScheduleOrder   =  schAppendAdModel.ScheduleOrder;
 
				switch(OrderSet)
				{
					case ORDER_FIRST:
						// 웹서비스 호출 타임아웃설정
						svc.Timeout = FrameSystem.m_SystemTimeout;
						// 웹서비스 메소드 호출
						remoteData = svc.SetSchAppendAdOrderFirst(remoteHeader, remoteData);
						break;
					case ORDER_UP:
						// 웹서비스 호출 타임아웃설정
						svc.Timeout = FrameSystem.m_SystemTimeout;
						// 웹서비스 메소드 호출
						remoteData = svc.SetSchAppendAdOrderUp(remoteHeader, remoteData);						
						break;
					case ORDER_DOWN:
						// 웹서비스 호출 타임아웃설정
						svc.Timeout = FrameSystem.m_SystemTimeout;
						// 웹서비스 메소드 호출
						remoteData = svc.SetSchAppendAdOrderDown(remoteHeader, remoteData);						
						break;
					case ORDER_LAST:
						// 웹서비스 호출 타임아웃설정
						svc.Timeout = FrameSystem.m_SystemTimeout;
						// 웹서비스 메소드 호출
						remoteData = svc.SetSchAppendAdOrderLast(remoteHeader, remoteData);
						break;
					default:
						throw new FrameException("순위변경 구분이 선택되지 않았습니다.");
				}


				// 결과코드검사
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// 결과 셋트
				schAppendAdModel.ResultCnt      = remoteData.ResultCnt;
				schAppendAdModel.ResultCD       = remoteData.ResultCD;		
				schAppendAdModel.ScheduleOrder  = remoteData.ScheduleOrder;

				_log.Debug("-----------------------------------------");
				_log.Debug("홈광고편성 첫번째 순위 변경 End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":SetSchAppendAdOrderFirst():" + fe.ErrCode + ":" + fe.ResultMsg);
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
