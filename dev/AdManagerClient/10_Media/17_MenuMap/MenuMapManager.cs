// ===============================================================================
// 
// MenuMapManager.cs
//
// ===============================================================================
// Copyright (C) Dartmedia.co.
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
	public class MenuMapManager : BaseManager
	{
		/// <summary>
		/// 생성자
		/// </summary>
		/// <param name="systemModel"></param>
		/// <param name="commonModel"></param>
		public MenuMapManager(SystemModel systemModel, CommonModel commonModel)
			: base(systemModel, commonModel)
		{
			_log = FrameSystem.oLog;
			_module = "MENUMAP";
			_Host  = FrameSystem.m_WebServer_Host;
			_Port  = FrameSystem.m_WebServer_Port;
			_Path = FrameSystem.m_WebServer_App + "/Media/MenuMapService.asmx";
		}

		/// <summary>
		/// 카테고리 목록조회
		/// </summary>
		public void GetCategoryList(MenuMapModel model)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("GetCategoryList Start");
				_log.Debug("-----------------------------------------");
								
				MenuMapServiceProxy.MenuMapService svc = new MenuMapServiceProxy.MenuMapService();

				// 리모트 모델 생성
				MenuMapServiceProxy.HeaderModel remoteHeader = new MenuMapServiceProxy.HeaderModel();
				MenuMapServiceProxy.MenuMapModel remoteData = new MenuMapServiceProxy.MenuMapModel();

				remoteHeader.UserID        = Header.UserID;

				svc.Url = _WebServiceUrl;
                svc.Timeout = FrameSystem.m_SystemTimeout;
                remoteData = svc.GetCategoryList(remoteHeader, remoteData);

				// 결과코드검사
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// 결과 셋트
				model.ResultCD = remoteData.ResultCD;
				model.ResultCnt = remoteData.ResultCnt;
				model.ResultDesc = remoteData.ResultDesc;
				model.CategoryDs = remoteData.CategoryDs.Copy();
				
				_log.Debug("-----------------------------------------");
				_log.Debug("GetCategoryList End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":GetCategenList():" + fe.ErrCode + ":" + fe.ResultMsg);
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
        /// 메뉴매핑 목록조회
        /// </summary>
        public void GetMenuMapList(MenuMapModel model)
        {
            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug("GetMenuMapList Start");
                _log.Debug("-----------------------------------------");

                MenuMapServiceProxy.MenuMapService svc = new MenuMapServiceProxy.MenuMapService();

                // 리모트 모델 생성
                MenuMapServiceProxy.HeaderModel remoteHeader = new MenuMapServiceProxy.HeaderModel();
                MenuMapServiceProxy.MenuMapModel remoteData = new MenuMapServiceProxy.MenuMapModel();

                remoteHeader.UserID = Header.UserID;

                svc.Url = _WebServiceUrl;
                svc.Timeout = FrameSystem.m_SystemTimeout;
                remoteData.SearchCategory = model.SearchCategory;
                remoteData = svc.GetMenuMapList(remoteHeader, remoteData);

                // 결과코드검사
                if (!remoteData.ResultCD.Equals("0000"))
                {
                    throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
                }

                // 결과 셋트
                model.ResultCD = remoteData.ResultCD;
                model.ResultCnt = remoteData.ResultCnt;
                model.ResultDesc = remoteData.ResultDesc;
                model.MenuMapDs = remoteData.MenuMapDs.Copy();

                _log.Debug("-----------------------------------------");
                _log.Debug("GetMenuMapList End");
                _log.Debug("-----------------------------------------");
            }
            catch (FrameException fe)
            {
                _log.Warning("-----------------------------------------");
                _log.Warning(this.ToString() + ":GetMenuMapList():" + fe.ErrCode + ":" + fe.ResultMsg);
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

        /// <summary>
        /// 메뉴매핑 정보 수정
        /// </summary>
        /// <param name="mediaMenuModel"></param>
        public void SetMenuMapUpdate(MenuMapModel model)
        {
            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug("메뉴매핑 수정 Start");
                _log.Debug("-----------------------------------------");

                // 웹서비스 인스턴스 생성
                MenuMapServiceProxy.MenuMapService svc = new MenuMapServiceProxy.MenuMapService();
                svc.Url = _WebServiceUrl;

                // 리모트 모델 생성
                MenuMapServiceProxy.HeaderModel remoteHeader = new MenuMapServiceProxy.HeaderModel();
                MenuMapServiceProxy.MenuMapModel remoteData = new MenuMapServiceProxy.MenuMapModel();

                // 헤더정보 셋트
                remoteHeader.ClientKey = Header.ClientKey;
                remoteHeader.UserID = Header.UserID;

                remoteData.MenuCode4 = model.MenuCode4;
                remoteData.AdGenre = model.AdGenre;

                // 웹서비스 호출 타임아웃설정
                svc.Timeout = FrameSystem.m_SystemTimeout;

                // 웹서비스 메소드 호출
                remoteData = svc.SetMenuMapUpdate(remoteHeader, remoteData);

                // 결과코드검사
                if (!remoteData.ResultCD.Equals("0000"))
                {
                    throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
                }

                // 결과 셋트
                model.ResultCnt = remoteData.ResultCnt;
                model.ResultCD = remoteData.ResultCD;

                _log.Debug("-----------------------------------------");
                _log.Debug("메뉴매핑 수정 End");
                _log.Debug("-----------------------------------------");
            }
            catch (FrameException fe)
            {
                _log.Warning("-----------------------------------------");
                _log.Warning(this.ToString() + ":SetMenuMapUpdate():" + fe.ErrCode + ":" + fe.ResultMsg);
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

        /// <summary>
        /// 메뉴매핑 설정
        /// </summary>
        /// <param name="mediaMenuModel"></param>
        public void SetMenuMapCreate(MenuMapModel model)
        {
            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug("메뉴매핑 설정 Start");
                _log.Debug("-----------------------------------------");

                // 웹서비스 인스턴스 생성
                MenuMapServiceProxy.MenuMapService svc = new MenuMapServiceProxy.MenuMapService();
                svc.Url = _WebServiceUrl;

                // 리모트 모델 생성
                MenuMapServiceProxy.HeaderModel remoteHeader = new MenuMapServiceProxy.HeaderModel();
                MenuMapServiceProxy.MenuMapModel remoteData = new MenuMapServiceProxy.MenuMapModel();

                // 헤더정보 셋트
                remoteHeader.ClientKey = Header.ClientKey;
                remoteHeader.UserID = Header.UserID;

                // 호출정보셋트
                remoteData.MenuCode4 = model.MenuCode4;
                remoteData.AdGenre = model.AdGenre;

                // 웹서비스 호출 타임아웃설정
                svc.Timeout = FrameSystem.m_SystemTimeout;

                // 웹서비스 메소드 호출
                remoteData = svc.SetMenuMapCreate(remoteHeader, remoteData);

                // 결과코드검사
                if (!remoteData.ResultCD.Equals("0000"))
                {
                    throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
                }

                // 결과 셋트
                model.ResultCnt = remoteData.ResultCnt;
                model.ResultCD = remoteData.ResultCD;

                _log.Debug("-----------------------------------------");
                _log.Debug("메뉴매핑 설정 End");
                _log.Debug("-----------------------------------------");
            }
            catch (FrameException fe)
            {
                model.ResultCD = "3101";
                _log.Warning("-----------------------------------------");
                _log.Warning(this.ToString() + ":SetMenuMapCreate():" + fe.ErrCode + ":" + fe.ResultMsg);
                _log.Warning("-----------------------------------------");
                throw fe;
            }
            catch (Exception e)
            {
                model.ResultCD = "3101";
                _log.Error("-----------------------------------------");
                _log.Exception(e);
                _log.Error("-----------------------------------------");
                throw new FrameException(e.Message);
            }
        }

        /// <summary>
        /// 메뉴매핑 정보 삭제
        /// </summary>
        /// <param name="mediaMenuModel"></param>
        public void SetMenuMapDelete(MenuMapModel model)
        {
            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug("메뉴매핑 정보삭제 Start");
                _log.Debug("-----------------------------------------");

                // 웹서비스 인스턴스 생성
                MenuMapServiceProxy.MenuMapService svc = new MenuMapServiceProxy.MenuMapService();
                svc.Url = _WebServiceUrl;

                // 리모트 모델 생성
                MenuMapServiceProxy.HeaderModel remoteHeader = new MenuMapServiceProxy.HeaderModel();
                MenuMapServiceProxy.MenuMapModel remoteData = new MenuMapServiceProxy.MenuMapModel();

                // 헤더정보 셋트
                remoteHeader.ClientKey = Header.ClientKey;
                remoteHeader.UserID = Header.UserID;

                remoteData.AdGenre = model.AdGenre;

                // 웹서비스 호출 타임아웃설정
                svc.Timeout = FrameSystem.m_SystemTimeout;

                // 웹서비스 메소드 호출
                remoteData = svc.SetMenuMapDelete(remoteHeader, remoteData);

                // 결과코드검사
                if (!remoteData.ResultCD.Equals("0000"))
                {
                    throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
                }

                // 결과 셋트
                model.ResultCnt = remoteData.ResultCnt;
                model.ResultCD = remoteData.ResultCD;

                _log.Debug("-----------------------------------------");
                _log.Debug("메뉴매핑 정보삭제 End");
                _log.Debug("-----------------------------------------");
            }
            catch (FrameException fe)
            {
                _log.Warning("-----------------------------------------");
                _log.Warning(this.ToString() + ":SetMenuMapDelete():" + fe.ErrCode + ":" + fe.ResultMsg);
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

        /// <summary>
        /// 기준 정보 메뉴 생성
        /// </summary>
        /// <param name="mediaMenuModel"></param>
        public void SetMenuCreate(MenuMapModel model)
        {
            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug("기준 메뉴 추가 Start");
                _log.Debug("-----------------------------------------");

                // 웹서비스 인스턴스 생성
                MenuMapServiceProxy.MenuMapService svc = new MenuMapServiceProxy.MenuMapService();
                svc.Url = _WebServiceUrl;

                // 리모트 모델 생성
                MenuMapServiceProxy.HeaderModel remoteHeader = new MenuMapServiceProxy.HeaderModel();
                MenuMapServiceProxy.MenuMapModel remoteData = new MenuMapServiceProxy.MenuMapModel();

                // 헤더정보 셋트
                remoteHeader.ClientKey = Header.ClientKey;
                remoteHeader.UserID = Header.UserID;

                // 호출정보셋트
                remoteData.MenuCode4 = model.MenuCode4;
                remoteData.MenuName4 = model.MenuName4;
                remoteData.UpperMenuCode4 = model.UpperMenuCode4;

                // 웹서비스 호출 타임아웃설정
                svc.Timeout = FrameSystem.m_SystemTimeout;

                // 웹서비스 메소드 호출
                remoteData = svc.SetMenuCreate(remoteHeader, remoteData);

                // 결과코드검사
                if (!remoteData.ResultCD.Equals("0000"))
                {
                    throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
                }

                // 결과 셋트
                model.ResultCnt = remoteData.ResultCnt;
                model.ResultCD = remoteData.ResultCD;

                _log.Debug("-----------------------------------------");
                _log.Debug("기준 메뉴 추가 End");
                _log.Debug("-----------------------------------------");
            }
            catch (FrameException fe)
            {
                model.ResultCD = "3101";
                _log.Warning("-----------------------------------------");
                _log.Warning(this.ToString() + ":SetMenuCreate():" + fe.ErrCode + ":" + fe.ResultMsg);
                _log.Warning("-----------------------------------------");
                throw fe;
            }
            catch (Exception e)
            {
                model.ResultCD = "3101";
                _log.Error("-----------------------------------------");
                _log.Exception(e);
                _log.Error("-----------------------------------------");
                throw new FrameException(e.Message);
            }
        }
	}
}