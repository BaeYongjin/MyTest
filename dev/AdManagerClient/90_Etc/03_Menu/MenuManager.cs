// ===============================================================================
//
// MenuManager.cs
//
// 메뉴관리 서비스를 호출합니다. 
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
	/// 메뉴정보 웹서비스를 호출합니다. 
	/// </summary>
	public class MenuManager : BaseManager
	{
		/// <summary>
		/// 생성자
		/// </summary>
		/// <param name="systemModel"></param>
		/// <param name="commonModel"></param>
		public MenuManager(SystemModel systemModel, CommonModel commonModel) : base(systemModel, commonModel)
		{
			_log	= FrameSystem.oLog;
			_Host	= FrameSystem.m_WebServer_Host;
			_Port	= FrameSystem.m_WebServer_Port;
			_Path	= FrameSystem.m_WebServer_App + "/Common/MenuService.asmx";
		}

        /// <summary>
        /// 메뉴정보 목록 조회
        /// </summary>
        /// <param name="baseModel"></param>
        public void GetUserMenuList(MenuModel menuModel)
        {
            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug("사용자메뉴조회 Start");
                _log.Debug("-----------------------------------------");

                // 웹서비스 인스턴스 생성
                MenuServicePloxy.MenuService svc = new MenuServicePloxy.MenuService();
                svc.Url = _WebServiceUrl;
			
                // 리모트 모델 생성
                MenuServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.MenuServicePloxy.HeaderModel();
                MenuServicePloxy.MenuModel     remoteData   = new AdManagerClient.MenuServicePloxy.MenuModel();

                // 헤더정보 셋트
                remoteHeader.ClientKey     = Header.ClientKey;
                remoteHeader.UserID        = Header.UserID;
                remoteHeader.UserLevel     = Header.UserLevel;
                remoteHeader.UserClass	   = Header.UserClass;
			
                // 웹서비스 호출 타임아웃설정
                svc.Timeout = FrameSystem.m_SystemTimeout;
                // 웹서비스 메소드 호출
                remoteData = svc.GetUserMenuList(remoteHeader, remoteData);

                // 결과코드검사
                if(!remoteData.ResultCD.Equals("0000"))
                {
                    throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
                }

                // 결과 셋트
                menuModel.MenuDataSet = remoteData.MenuDataSet.Copy();
                menuModel.ResultCnt   = remoteData.ResultCnt;
                menuModel.ResultCD    = remoteData.ResultCD;

                _log.Debug("-----------------------------------------");
                _log.Debug("사용자메뉴조회 End");
                _log.Debug("-----------------------------------------");
            }
            catch(FrameException fe)
            {
                _log.Warning("-----------------------------------------");
                _log.Warning( this.ToString() + ":GetUserMenuList():" + fe.ErrCode + ":" + fe.ResultMsg);
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
        /// 메뉴정보 목록 조회
        /// </summary>
        /// <param name="baseModel"></param>
        public void GetUserClassList(MenuModel menuModel)
        {
            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug("사용자메뉴조회 Start");
                _log.Debug("-----------------------------------------");

                // 웹서비스 인스턴스 생성
                MenuServicePloxy.MenuService svc = new MenuServicePloxy.MenuService();
                svc.Url = _WebServiceUrl;
			
                // 리모트 모델 생성
                MenuServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.MenuServicePloxy.HeaderModel();
                MenuServicePloxy.MenuModel     remoteData   = new AdManagerClient.MenuServicePloxy.MenuModel();

                // 헤더정보 셋트
                remoteHeader.ClientKey     = Header.ClientKey;
                remoteHeader.UserID        = Header.UserID;
                remoteHeader.UserLevel     = Header.UserLevel;
                remoteHeader.UserClass	   = Header.UserClass;
			
                // 웹서비스 호출 타임아웃설정
                svc.Timeout = FrameSystem.m_SystemTimeout;
                // 웹서비스 메소드 호출
                remoteData = svc.GetUserClassList(remoteHeader, remoteData);

                // 결과코드검사
                if(!remoteData.ResultCD.Equals("0000"))
                {
                    throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
                }

                // 결과 셋트
                menuModel.MenuDataSet = remoteData.MenuDataSet.Copy();
                menuModel.ResultCnt   = remoteData.ResultCnt;
                menuModel.ResultCD    = remoteData.ResultCD;

                _log.Debug("-----------------------------------------");
                _log.Debug("사용자메뉴조회 End");
                _log.Debug("-----------------------------------------");
            }
            catch(FrameException fe)
            {
                _log.Warning("-----------------------------------------");
                _log.Warning( this.ToString() + ":GetUserMenuList():" + fe.ErrCode + ":" + fe.ResultMsg);
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
        /// 메뉴권한 조회
        /// </summary>
        /// <param name="baseModel"></param>
        public void GetMenuPowerList(MenuModel menuModel)
        {
            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug("사용자메뉴조회 Start");
                _log.Debug("-----------------------------------------");

                // 웹서비스 인스턴스 생성
                MenuServicePloxy.MenuService svc = new MenuServicePloxy.MenuService();
                svc.Url = _WebServiceUrl;
			
                // 리모트 모델 생성
                MenuServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.MenuServicePloxy.HeaderModel();
                MenuServicePloxy.MenuModel     remoteData   = new AdManagerClient.MenuServicePloxy.MenuModel();

                // 헤더정보 셋트
                remoteHeader.ClientKey     = Header.ClientKey;
                remoteHeader.UserID        = Header.UserID;
                remoteHeader.UserLevel     = Header.UserLevel;
                remoteHeader.UserClass	   = Header.UserClass;
			
                // 호출정보 셋트
                remoteData.UserClassCode         = menuModel.UserClassCode;


                // 웹서비스 호출 타임아웃설정
                svc.Timeout = FrameSystem.m_SystemTimeout;
                // 웹서비스 메소드 호출
                remoteData = svc.GetMenuPowerList(remoteHeader, remoteData);

                // 결과코드검사
                if(!remoteData.ResultCD.Equals("0000"))
                {
                    throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
                }

                // 결과 셋트
                menuModel.MenuDataSet = remoteData.MenuDataSet.Copy();
                menuModel.ResultCnt   = remoteData.ResultCnt;
                menuModel.ResultCD    = remoteData.ResultCD;

                _log.Debug("-----------------------------------------");
                _log.Debug("사용자메뉴조회 End");
                _log.Debug("-----------------------------------------");
            }
            catch(FrameException fe)
            {
                _log.Warning("-----------------------------------------");
                _log.Warning( this.ToString() + ":GetUserMenuList():" + fe.ErrCode + ":" + fe.ResultMsg);
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


        public void SetUserClassAdd(MenuModel menuModel)
        {
            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug("사용자 정보 추가 Start");
                _log.Debug("-----------------------------------------");

            
                // 웹서비스 인스턴스 생성
                MenuServicePloxy.MenuService svc = new MenuServicePloxy.MenuService();
                svc.Url = _WebServiceUrl;			
                // 리모트 모델 생성
                MenuServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.MenuServicePloxy.HeaderModel();
                MenuServicePloxy.MenuModel remoteData   = new AdManagerClient.MenuServicePloxy.MenuModel();
            
                // 헤더정보 셋트
                remoteHeader.ClientKey     = Header.ClientKey;
                remoteHeader.UserID        = Header.UserID;
            
                // 호출정보셋트
                remoteData.UserClassCode        = menuModel.UserClassCode;
                remoteData.UserClassName    = menuModel.UserClassName;

                // 웹서비스 호출 타임아웃설정
                svc.Timeout = FrameSystem.m_SystemTimeout;
                // 웹서비스 메소드 호출
                remoteData = svc.SetUserClassCreate(remoteHeader, remoteData);
            
                // 결과코드검사
                if(!remoteData.ResultCD.Equals("0000"))
                {
                    throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
                }
            
                // 결과 셋트
                menuModel.ResultCnt   = remoteData.ResultCnt;
                _log.Debug("menuModel.ResultCnt = "+menuModel.ResultCnt);
            			
                menuModel.ResultCD    = remoteData.ResultCD;
            
                _log.Debug("-----------------------------------------");
                _log.Debug("사용자 정보 추가 End");
                _log.Debug("-----------------------------------------");
            }
            catch(FrameException fe)
            {
                menuModel.ResultCD    = "3101";
                _log.Warning("-----------------------------------------");
                _log.Warning( this.ToString() + ":SetMenuCreate():" + fe.ErrCode + ":" + fe.ResultMsg);
                _log.Warning("-----------------------------------------");
                throw fe;
            }
            catch(Exception e)
            {
                menuModel.ResultCD    = "3101";
                _log.Error("-----------------------------------------");
                _log.Exception(e);
                _log.Error("-----------------------------------------");
                throw new FrameException(e.Message);
            }
        }

        public void SetUserClassUpdate(MenuModel menuModel)
        {
            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug("사용자 정보 추가 Start");
                _log.Debug("-----------------------------------------");

            
                // 웹서비스 인스턴스 생성
                MenuServicePloxy.MenuService svc = new MenuServicePloxy.MenuService();
                svc.Url = _WebServiceUrl;			
                // 리모트 모델 생성
                MenuServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.MenuServicePloxy.HeaderModel();
                MenuServicePloxy.MenuModel remoteData   = new AdManagerClient.MenuServicePloxy.MenuModel();
            
                // 헤더정보 셋트
                remoteHeader.ClientKey     = Header.ClientKey;
                remoteHeader.UserID        = Header.UserID;
            
                // 호출정보셋트
                remoteData.UserClassCode   = menuModel.UserClassCode;
                remoteData.UserClassName   = menuModel.UserClassName;
                remoteData.MenuDataSet     = menuModel.MenuDataSet.Copy();

                // 웹서비스 호출 타임아웃설정
                svc.Timeout = FrameSystem.m_SystemTimeout;
                // 웹서비스 메소드 호출
                remoteData = svc.SetUserClassUpdate(remoteHeader, remoteData);
            
                // 결과코드검사
                if(!remoteData.ResultCD.Equals("0000"))
                {
                    throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
                }
            
                // 결과 셋트
                menuModel.ResultCnt   = remoteData.ResultCnt;
                _log.Debug("menuModel.ResultCnt = "+menuModel.ResultCnt);
            			
                menuModel.ResultCD    = remoteData.ResultCD;
            
                _log.Debug("-----------------------------------------");
                _log.Debug("장르그룹추가 End");
                _log.Debug("-----------------------------------------");
            }
            catch(FrameException fe)
            {
                menuModel.ResultCD    = "3101";
                _log.Warning("-----------------------------------------");
                _log.Warning( this.ToString() + ":SetMenuCreate():" + fe.ErrCode + ":" + fe.ResultMsg);
                _log.Warning("-----------------------------------------");
                throw fe;
            }
            catch(Exception e)
            {
                menuModel.ResultCD    = "3101";
                _log.Error("-----------------------------------------");
                _log.Exception(e);
                _log.Error("-----------------------------------------");
                throw new FrameException(e.Message);
            }
        }


        /// <summary>
        /// 장르그룹정보 수정
        /// </summary>
        /// <param name="menuModel"></param>
        public void SetMenuPowerUpdate(MenuModel menuModel)
        {
            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug("메뉴권한 정보수정 Start");
                _log.Debug("-----------------------------------------");
            
                // 웹서비스 인스턴스 생성
                MenuServicePloxy.MenuService svc = new MenuServicePloxy.MenuService();
                svc.Url = _WebServiceUrl;		
                // 리모트 모델 생성
                MenuServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.MenuServicePloxy.HeaderModel();
                MenuServicePloxy.MenuModel remoteData   = new AdManagerClient.MenuServicePloxy.MenuModel();
            
                // 헤더정보 셋트
                remoteHeader.ClientKey     = Header.ClientKey;
                remoteHeader.UserID        = Header.UserID;
            
                // 호출정보셋트
                remoteData.MenuDataSet    = menuModel.MenuDataSet.Copy();
            				
                // 웹서비스 호출 타임아웃설정
                svc.Timeout = FrameSystem.m_SystemTimeout;
                // 웹서비스 메소드 호출
                remoteData = svc.SetMenuPowerUpdate(remoteHeader, remoteData);
            
                // 결과코드검사
                if(!remoteData.ResultCD.Equals("0000"))
                {
                    throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
                }
            
                // 결과 셋트
                menuModel.ResultCnt   = remoteData.ResultCnt;
                menuModel.ResultCD    = remoteData.ResultCD;
            
                _log.Debug("-----------------------------------------");
                _log.Debug("메뉴권한 정보수정 End");
                _log.Debug("-----------------------------------------");
            }
            catch(FrameException fe)
            {
                _log.Warning("-----------------------------------------");
                _log.Warning( this.ToString() + ":SetMenuPowerUpdate():" + fe.ErrCode + ":" + fe.ResultMsg);
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
