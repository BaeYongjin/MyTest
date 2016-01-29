// ===============================================================================
//
// SystemMenuManager.cs
//
// 공통메뉴조회 서비스를 호출합니다. 
// 박병준
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
	public class SystemMenuManager : BaseManager
	{
		const int ORDER_UP    = 3;
		const int ORDER_DOWN  = 4;
		/// <summary>
		/// 생성자
		/// </summary>
		/// <param name="systemModel"></param>
		/// <param name="commonModel"></param>

		public SystemMenuManager(SystemModel systemModel, CommonModel commonModel) : base(systemModel, commonModel)
		{
			_log = FrameSystem.oLog;
			_Host  = FrameSystem.m_WebServer_Host;
			_Port  = FrameSystem.m_WebServer_Port;
			_Path  = FrameSystem.m_WebServer_App + "/Common/SystemMenuService.asmx";
		}

		/// <summary>
		/// 메뉴콤보목록
		/// </summary>
		/// <param name="baseModel"></param>
		public void GetComboList(SystemMenuModel systemMenuModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("메뉴콤보목록 Start");
				_log.Debug("-----------------------------------------");

				// 웹서비스 인스턴스 생성
				SystemMenuServicePloxy.SystemMenuService svc = new SystemMenuServicePloxy.SystemMenuService();
				svc.Url = _WebServiceUrl;
			
				// 리모트 모델 생성
				SystemMenuServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.SystemMenuServicePloxy.HeaderModel();
				SystemMenuServicePloxy.SystemMenuModel     remoteData   = new AdManagerClient.SystemMenuServicePloxy.SystemMenuModel();

				// 헤더정보 셋트
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;

				// 호출정보 셋트				
				//remoteData.SearchMenuCode         = systemMenuModel.SearchMenuCode;
				
				// 웹서비스 호출 타임아웃설정
				svc.Timeout = FrameSystem.m_SystemTimeout;
				// 웹서비스 메소드 호출
				remoteData = svc.GetComboList(remoteHeader, remoteData);

				// 결과메뉴검사
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// 결과 셋트
				systemMenuModel.SystemMenuDataSet = remoteData.SystemMenuDataSet.Copy();
				systemMenuModel.ResultCnt   = remoteData.ResultCnt;
				systemMenuModel.ResultCD    = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("메뉴콤보목록 End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":GetComboList():" + fe.ErrCode + ":" + fe.ResultMsg);
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
		/// 메뉴구분목록
		/// </summary>
		/// <param name="baseModel"></param>
		public void GetUpperMenuList(SystemMenuModel systemMenuModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("메뉴구분목록 Start");
				_log.Debug("-----------------------------------------");

				// 웹서비스 인스턴스 생성
				SystemMenuServicePloxy.SystemMenuService svc = new SystemMenuServicePloxy.SystemMenuService();
				svc.Url = _WebServiceUrl;
			
				// 리모트 모델 생성
				SystemMenuServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.SystemMenuServicePloxy.HeaderModel();
				SystemMenuServicePloxy.SystemMenuModel     remoteData   = new AdManagerClient.SystemMenuServicePloxy.SystemMenuModel();

				// 헤더정보 셋트
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;

				// 호출정보 셋트				
				remoteData.SearchMenuCode         = systemMenuModel.SearchMenuCode;
				
				// 웹서비스 호출 타임아웃설정
				svc.Timeout = FrameSystem.m_SystemTimeout;
				// 웹서비스 메소드 호출
				remoteData = svc.GetUpperMenuList(remoteHeader, remoteData);

				// 결과메뉴검사
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// 결과 셋트
				systemMenuModel.SystemMenuDataSet = remoteData.SystemMenuDataSet.Copy();
				systemMenuModel.ResultCnt   = remoteData.ResultCnt;
				systemMenuModel.ResultCD    = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("메뉴구분목록 End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":GetUpperMenuList():" + fe.ErrCode + ":" + fe.ResultMsg);
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
		/// 메뉴목록
		/// </summary>
		/// <param name="baseModel"></param>
		public void GetMenuList(SystemMenuModel systemMenuModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("공통메뉴조회 Start");
				_log.Debug("-----------------------------------------");

				// 웹서비스 인스턴스 생성
				SystemMenuServicePloxy.SystemMenuService svc = new SystemMenuServicePloxy.SystemMenuService();
				svc.Url = _WebServiceUrl;
			
				// 리모트 모델 생성
				SystemMenuServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.SystemMenuServicePloxy.HeaderModel();
				SystemMenuServicePloxy.SystemMenuModel     remoteData   = new AdManagerClient.SystemMenuServicePloxy.SystemMenuModel();

				// 헤더정보 셋트
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;

				// 호출정보 셋트
				remoteData.SearchKey       = systemMenuModel.SearchKey;
				remoteData.UpperMenu         = systemMenuModel.UpperMenu;
				remoteData.MenuCode         = systemMenuModel.MenuCode;
				
				// 웹서비스 호출 타임아웃설정
				svc.Timeout = FrameSystem.m_SystemTimeout;
				// 웹서비스 메소드 호출
				remoteData = svc.GetMenuList(remoteHeader, remoteData);

				// 결과메뉴검사
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// 결과 셋트
				systemMenuModel.SystemMenuDataSet = remoteData.SystemMenuDataSet.Copy();
				systemMenuModel.LastOrder   = remoteData.LastOrder;
				systemMenuModel.ResultCnt   = remoteData.ResultCnt;
				systemMenuModel.ResultCD    = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("공통메뉴조회 End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":GetMenuList():" + fe.ErrCode + ":" + fe.ResultMsg);
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
		/// 메뉴구분정보 수정
		/// </summary>
		/// <param name="systemMenuModel"></param>
		public void SetUpperMenuUpdate(SystemMenuModel systemMenuModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("메뉴구분정보수정 Start");
				_log.Debug("-----------------------------------------");

				// 웹서비스 인스턴스 생성
				SystemMenuServicePloxy.SystemMenuService svc = new SystemMenuServicePloxy.SystemMenuService();
				svc.Url = _WebServiceUrl;
			
				// 리모트 모델 생성
				SystemMenuServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.SystemMenuServicePloxy.HeaderModel();
				SystemMenuServicePloxy.SystemMenuModel     remoteData   = new AdManagerClient.SystemMenuServicePloxy.SystemMenuModel();

				// 헤더정보 셋트
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;

				// 호출정보셋트
				remoteData.MenuName       = systemMenuModel.MenuName;
				remoteData.UseYn       = systemMenuModel.UseYn;
				remoteData.MenuCode     = systemMenuModel.MenuCode;				
											
				// 웹서비스 호출 타임아웃설정
				svc.Timeout = FrameSystem.m_SystemTimeout;

				// 웹서비스 메소드 호출
				remoteData = svc.SetUpperMenuUpdate(remoteHeader, remoteData);

				// 결과메뉴검사
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// 결과 셋트
				systemMenuModel.ResultCnt   = remoteData.ResultCnt;
				systemMenuModel.ResultCD    = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("메뉴구분정보수정 End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":SetUpperMenuUpdate():" + fe.ErrCode + ":" + fe.ResultMsg);
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
		/// 메뉴정보 수정
		/// </summary>
		/// <param name="systemMenuModel"></param>
		public void SetMenuCodeUpdate(SystemMenuModel systemMenuModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("메뉴정보수정 Start");
				_log.Debug("-----------------------------------------");

				// 웹서비스 인스턴스 생성
				SystemMenuServicePloxy.SystemMenuService svc = new SystemMenuServicePloxy.SystemMenuService();
				svc.Url = _WebServiceUrl;
			
				// 리모트 모델 생성
				SystemMenuServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.SystemMenuServicePloxy.HeaderModel();
				SystemMenuServicePloxy.SystemMenuModel     remoteData   = new AdManagerClient.SystemMenuServicePloxy.SystemMenuModel();

				// 헤더정보 셋트
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;

				// 호출정보셋트
				remoteData.MenuName       = systemMenuModel.MenuName;
				remoteData.UseYn       = systemMenuModel.UseYn;
				remoteData.MenuCode       = systemMenuModel.MenuCode;
															
				// 웹서비스 호출 타임아웃설정
				svc.Timeout = FrameSystem.m_SystemTimeout;

				// 웹서비스 메소드 호출
				remoteData = svc.SetMenuCodeUpdate(remoteHeader, remoteData);

				// 결과메뉴검사
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// 결과 셋트
				systemMenuModel.ResultCnt   = remoteData.ResultCnt;
				systemMenuModel.ResultCD    = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("메뉴정보수정 End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":SetMenuCodeUpdate():" + fe.ErrCode + ":" + fe.ResultMsg);
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
		/// 메뉴정보 등록
		/// </summary>
		/// <param name="systemMenuModel"></param>
		public void SetUpperMenuCreate(SystemMenuModel systemMenuModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("메뉴정보등록 Start");
				_log.Debug("-----------------------------------------");

				// 웹서비스 인스턴스 생성
				SystemMenuServicePloxy.SystemMenuService svc = new SystemMenuServicePloxy.SystemMenuService();
				svc.Url = _WebServiceUrl;
			
				// 리모트 모델 생성
				SystemMenuServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.SystemMenuServicePloxy.HeaderModel();
				SystemMenuServicePloxy.SystemMenuModel     remoteData   = new AdManagerClient.SystemMenuServicePloxy.SystemMenuModel();

				// 헤더정보 셋트
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;

				// 호출정보셋트
				remoteData.MenuCode       = systemMenuModel.MenuCode;				
				remoteData.MenuCode_2       = systemMenuModel.MenuCode_2;				
				remoteData.MenuName       = systemMenuModel.MenuName;				
															
				// 웹서비스 호출 타임아웃설정
				svc.Timeout = FrameSystem.m_SystemTimeout;

				// 웹서비스 메소드 호출
				remoteData = svc.SetUpperMenuCreate(remoteHeader, remoteData);

				// 결과메뉴검사
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// 결과 셋트
				systemMenuModel.ResultCnt   = remoteData.ResultCnt;
				systemMenuModel.ResultCD    = remoteData.ResultCD;
				systemMenuModel.MenuCode  = remoteData.MenuCode;

				_log.Debug("-----------------------------------------");
				_log.Debug("메뉴정보등록 End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":SetUpperMenuCreate():" + fe.ErrCode + ":" + fe.ResultMsg);
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
		/// 메뉴정보 등록
		/// </summary>
		/// <param name="systemMenuModel"></param>
		public void SetMenuCodeCreate(SystemMenuModel systemMenuModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("메뉴정보등록 Start");
				_log.Debug("-----------------------------------------");

				// 웹서비스 인스턴스 생성
				SystemMenuServicePloxy.SystemMenuService svc = new SystemMenuServicePloxy.SystemMenuService();
				svc.Url = _WebServiceUrl;
			
				// 리모트 모델 생성
				SystemMenuServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.SystemMenuServicePloxy.HeaderModel();
				SystemMenuServicePloxy.SystemMenuModel     remoteData   = new AdManagerClient.SystemMenuServicePloxy.SystemMenuModel();

				// 헤더정보 셋트
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;

				// 호출정보셋트
				remoteData.MenuCode       = systemMenuModel.MenuCode;								
				remoteData.MenuCode_2       = systemMenuModel.MenuCode_2;			
				remoteData.MenuName       = systemMenuModel.MenuName;	
				remoteData.UpperMenu       = systemMenuModel.UpperMenu;				
															
				// 웹서비스 호출 타임아웃설정
				svc.Timeout = FrameSystem.m_SystemTimeout;

				// 웹서비스 메소드 호출
				remoteData = svc.SetMenuCodeCreate(remoteHeader, remoteData);

				// 결과메뉴검사
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// 결과 셋트
				systemMenuModel.ResultCnt   = remoteData.ResultCnt;
				systemMenuModel.ResultCD    = remoteData.ResultCD;
				systemMenuModel.MenuCode  = remoteData.MenuCode;

				_log.Debug("-----------------------------------------");
				_log.Debug("메뉴정보등록 End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":SetMenuCodeCreate():" + fe.ErrCode + ":" + fe.ResultMsg);
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
		/// 메뉴정보 삭제
		/// </summary>
		/// <param name="systemMenuModel"></param>
		public void SetUpperMenuDelete(SystemMenuModel systemMenuModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("메뉴정보삭제 Start");
				_log.Debug("-----------------------------------------");

				// 웹서비스 인스턴스 생성
				SystemMenuServicePloxy.SystemMenuService svc = new SystemMenuServicePloxy.SystemMenuService();
				svc.Url = _WebServiceUrl;
			
				// 리모트 모델 생성
				SystemMenuServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.SystemMenuServicePloxy.HeaderModel();
				SystemMenuServicePloxy.SystemMenuModel     remoteData   = new AdManagerClient.SystemMenuServicePloxy.SystemMenuModel();

				// 헤더정보 셋트
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;

				// 호출정보셋트				
				remoteData.MenuCode       = systemMenuModel.MenuCode;			
				remoteData.UpperMenu       = systemMenuModel.UpperMenu;			
											
				// 웹서비스 호출 타임아웃설정
				svc.Timeout = FrameSystem.m_SystemTimeout;

				// 웹서비스 메소드 호출
				remoteData = svc.SetUpperMenuDelete(remoteHeader, remoteData);

				// 결과메뉴검사
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// 결과 셋트
				systemMenuModel.ResultCnt   = remoteData.ResultCnt;
				systemMenuModel.ResultCD    = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("메뉴정보수정 End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":SetUpperMenuDelete():" + fe.ErrCode + ":" + fe.ResultMsg);
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
		/// 메뉴정보 삭제
		/// </summary>
		/// <param name="systemMenuModel"></param>
		public void SetMenuCodeDelete(SystemMenuModel systemMenuModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("메뉴정보삭제 Start");
				_log.Debug("-----------------------------------------");

				// 웹서비스 인스턴스 생성
				SystemMenuServicePloxy.SystemMenuService svc = new SystemMenuServicePloxy.SystemMenuService();
				svc.Url = _WebServiceUrl;
			
				// 리모트 모델 생성
				SystemMenuServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.SystemMenuServicePloxy.HeaderModel();
				SystemMenuServicePloxy.SystemMenuModel     remoteData   = new AdManagerClient.SystemMenuServicePloxy.SystemMenuModel();

				// 헤더정보 셋트
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;

				// 호출정보셋트				
				remoteData.MenuCode       = systemMenuModel.MenuCode;			
											
				// 웹서비스 호출 타임아웃설정
				svc.Timeout = FrameSystem.m_SystemTimeout;

				// 웹서비스 메소드 호출
				remoteData = svc.SetMenuCodeDelete(remoteHeader, remoteData);

				// 결과메뉴검사
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// 결과 셋트
				systemMenuModel.ResultCnt   = remoteData.ResultCnt;
				systemMenuModel.ResultCD    = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("메뉴정보수정 End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":SetMenuCodeDelete():" + fe.ErrCode + ":" + fe.ResultMsg);
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
		/// 메뉴 순위 변경
		/// </summary>
		/// <param name="systemMenuModel"></param>
		/// <returns></returns>
		public void SetMenuCodeOrderSet(SystemMenuModel systemMenuModel, int OrderSet)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("메뉴 순위 변경 Start");
				_log.Debug("-----------------------------------------");

				//입력데이터의 Validation 검사
				if(systemMenuModel.MenuCode.Length < 1) 
				{
					throw new FrameException("매뉴가 선택되지 않았습니다.");
				}

				// 웹서비스 인스턴스 생성
				SystemMenuServicePloxy.SystemMenuService svc = new SystemMenuServicePloxy.SystemMenuService();
	
				// 웹서비스 URL동적 생성
				svc.Url = _WebServiceUrl;			

				// 리모트 모델 생성
				SystemMenuServicePloxy.HeaderModel      remoteHeader = new AdManagerClient.SystemMenuServicePloxy.HeaderModel();
				SystemMenuServicePloxy.SystemMenuModel   remoteData   = new AdManagerClient.SystemMenuServicePloxy.SystemMenuModel();

				// 헤더정보 셋트
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID; 
				remoteHeader.UserLevel     = Header.UserLevel;
				remoteHeader.UserClass     = Header.UserClass;

				// 호출정보 셋트

				remoteData.MenuCode       =  systemMenuModel.MenuCode;				
				remoteData.MenuOrder   =  systemMenuModel.MenuOrder;
				remoteData.UpperMenu   =  systemMenuModel.UpperMenu;
 
				switch(OrderSet)
				{					
					case ORDER_UP:
						// 웹서비스 호출 타임아웃설정
						svc.Timeout = FrameSystem.m_SystemTimeout;
						// 웹서비스 메소드 호출
						remoteData = svc.SetMenuCodeOrderUp(remoteHeader, remoteData);						
						break;
					case ORDER_DOWN:
						// 웹서비스 호출 타임아웃설정
						svc.Timeout = FrameSystem.m_SystemTimeout;
						// 웹서비스 메소드 호출
						remoteData = svc.SetMenuCodeOrderDown(remoteHeader, remoteData);						
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
				systemMenuModel.ResultCnt      = remoteData.ResultCnt;
				systemMenuModel.ResultCD       = remoteData.ResultCD;		
				systemMenuModel.MenuOrder  = remoteData.MenuOrder;

				_log.Debug("-----------------------------------------");
				_log.Debug("메뉴 첫번째 순위 변경 End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":SetSchHomeAdOrderFirst():" + fe.ErrCode + ":" + fe.ResultMsg);
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
