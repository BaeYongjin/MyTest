// ===============================================================================
// GroupUpdate Manager  for Charites Project
//
// GroupUpdateManager.cs
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

/*
 * -------------------------------------------------------
 * Class Name: GroupManager
 * 주요기능  : 그룹정보관리 서비스 호출
 * 작성자    : 모름
 * 작성일    : 모름
 * -------------------------------------------------------
 * [수정사항]
 * -------------------------------------------------------
 * 수정코드  : [E_01]
 * 수정자    : 김보배
 * 수정일    : 2013.02
 * 수정내용  :        
 *            - 검색어로 조회 안함
 * 수정함수  :
 *            - SearchGroupList 검색어 
 * --------------------------------------------------------
 */



/*
 * -------------------------------------------------------
 * Class Name: GroupManager
 * 주요기능  : 그룹정보관리 서비스 호출
 * -------------------------------------------------------
 * [수정사항]
 * -------------------------------------------------------
 * 수정코드  : [E_02]
 * 수정자    : 이영일
 * 수정일    : 2015.05.26
 * 수정내용  :        
 *            - 무효처리된 메뉴를 메뉴리스트에 포함할것인지 여부 추가
 *            InvalidYn
 * --------------------------------------------------------
 */

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
	public class GroupManager : BaseManager
	{
		/// <summary>
		/// 생성자
		/// </summary>
		/// <param name="systemModel"></param>
		/// <param name="commonModel"></param>

		public GroupManager(SystemModel systemModel, CommonModel commonModel) : base(systemModel, commonModel)
		{
			_log = FrameSystem.oLog;
			_module = "USERINFO";
			_Host  = FrameSystem.m_WebServer_Host;
			_Port  = FrameSystem.m_WebServer_Port;
			_Path  = FrameSystem.m_WebServer_App + "/Media/GroupService.asmx";
		}

		/// <summary>
		/// 사용자정보조회
		/// </summary>
		/// <param name="groupModel"></param>
		public void GetGroupList(GroupModel groupModel)
		{

			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("사용자목록조회 Start");
				_log.Debug("-----------------------------------------");
				
				// 웹서비스 인스턴스 생성
				GroupServicePloxy.GroupService svc = new GroupServicePloxy.GroupService();
				svc.Url = _WebServiceUrl;
			
				// 리모트 모델 생성
				GroupServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.GroupServicePloxy.HeaderModel();
				GroupServicePloxy.GroupModel remoteData   = new AdManagerClient.GroupServicePloxy.GroupModel();

				// 헤더정보 셋트
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;
				remoteHeader.UserLevel     = Header.UserLevel;
				remoteHeader.UserClass	   = Header.UserClass;

				// 호출정보 셋트
                //remoteData.SearchKey       = groupModel.SearchKey; // [E_01]
				remoteData.SearchMedia       = groupModel.SearchMedia;
				
				// 웹서비스 호출 타임아웃설정
				svc.Timeout = FrameSystem.m_SystemTimeout;

				// 웹서비스 메소드 호출
				remoteData = svc.GetGroupList(remoteHeader, remoteData);

				// 결과코드검사
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// 결과 셋트
				groupModel.GroupDataSet = remoteData.GroupDataSet.Copy();
				groupModel.ResultCnt   = remoteData.ResultCnt;
				groupModel.ResultCD    = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("그룹목록조회 End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":GetGroupList():" + fe.ErrCode + ":" + fe.ResultMsg);
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
		/// 그룹상세조회
		/// </summary>
		/// <param name="groupModel"></param>
		public void GetGroupDetailList(GroupModel groupModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("그룹상세조회 Start");
				_log.Debug("-----------------------------------------");
				
				// 웹서비스 인스턴스 생성
				GroupServicePloxy.GroupService svc = new GroupServicePloxy.GroupService();
				svc.Url = _WebServiceUrl;
			
				// 리모트 모델 생성
				GroupServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.GroupServicePloxy.HeaderModel();
				GroupServicePloxy.GroupModel remoteData   = new AdManagerClient.GroupServicePloxy.GroupModel();

				// 헤더정보 셋트
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;
				remoteHeader.UserLevel     = Header.UserLevel;
				remoteHeader.UserClass	   = Header.UserClass;

				// 호출정보 셋트
				remoteData.SearchGroup       = groupModel.SearchGroup;
				
				// 웹서비스 호출 타임아웃설정
				svc.Timeout = FrameSystem.m_SystemTimeout;

				// 웹서비스 메소드 호출
				remoteData = svc.GetGroupDetailList(remoteHeader, remoteData);

				// 결과코드검사
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// 결과 셋트
				groupModel.GroupDetailDataSet = remoteData.GroupDetailDataSet.Copy();
				groupModel.ResultCnt   = remoteData.ResultCnt;
				groupModel.ResultCD    = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("그룹상세조회 End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":GetGroupDetailList():" + fe.ErrCode + ":" + fe.ResultMsg);
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
		/// 카테고리조회
		/// </summary>
		/// <param name="groupModel"></param>
		public void GetCategoryList(GroupModel groupModel)
		{

			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("카테고리조회 Start");
				_log.Debug("-----------------------------------------");
				
				// 웹서비스 인스턴스 생성
				GroupServicePloxy.GroupService svc = new GroupServicePloxy.GroupService();
				svc.Url = _WebServiceUrl;
			
				// 리모트 모델 생성
				GroupServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.GroupServicePloxy.HeaderModel();
				GroupServicePloxy.GroupModel remoteData   = new AdManagerClient.GroupServicePloxy.GroupModel();

				// 헤더정보 셋트
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;
				remoteHeader.UserLevel     = Header.UserLevel;
                remoteHeader.UserClass = Header.UserClass;

				// 호출정보 셋트
                remoteData.SearchKey = groupModel.SearchKey;
                remoteData.SearchType = groupModel.SearchType;      //검색 타입
                remoteData.InvalidYn = groupModel.InvalidYn;        //무효메뉴 목록 포함 여부

				
				// 웹서비스 호출 타임아웃설정
				svc.Timeout = FrameSystem.m_SystemTimeout;

				// 웹서비스 메소드 호출
				remoteData = svc.GetCategoryList(remoteHeader, remoteData);

				// 결과코드검사
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// 결과 셋트
				groupModel.CategoryDataSet = remoteData.CategoryDataSet.Copy();
				groupModel.ResultCnt   = remoteData.ResultCnt;
				groupModel.ResultCD    = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("카테고리조회 End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":GetCategoryList():" + fe.ErrCode + ":" + fe.ResultMsg);
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
        /// 카테고리조회
        /// </summary>
        /// <param name="groupModel"></param>
        public void GetCategoryList2(GroupModel groupModel)
        {

            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug("카테고리조회 Start");
                _log.Debug("-----------------------------------------");

                // 웹서비스 인스턴스 생성
                GroupServicePloxy.GroupService svc = new GroupServicePloxy.GroupService();
                svc.Url = _WebServiceUrl;

                // 리모트 모델 생성
                GroupServicePloxy.HeaderModel remoteHeader = new AdManagerClient.GroupServicePloxy.HeaderModel();
                GroupServicePloxy.GroupModel remoteData = new AdManagerClient.GroupServicePloxy.GroupModel();

                // 헤더정보 셋트
                remoteHeader.ClientKey = Header.ClientKey;
                remoteHeader.UserID = Header.UserID;
                remoteHeader.UserLevel = Header.UserLevel;
                remoteHeader.UserClass = Header.UserClass;

                // 호출정보 셋트
                remoteData.SearchKey = groupModel.SearchKey;
                remoteData.SearchType = groupModel.SearchType;      //검색 타입
                remoteData.InvalidYn = groupModel.InvalidYn;        //무효메뉴 목록 포함 여부


                // 웹서비스 호출 타임아웃설정
                svc.Timeout = FrameSystem.m_SystemTimeout;

                // 웹서비스 메소드 호출
                remoteData = svc.GetCategoryList2(remoteHeader, remoteData);

                // 결과코드검사
                if (!remoteData.ResultCD.Equals("0000"))
                {
                    throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
                }

                // 결과 셋트
                groupModel.CategoryDataSet = remoteData.CategoryDataSet.Copy();
                groupModel.ResultCnt = remoteData.ResultCnt;
                groupModel.ResultCD = remoteData.ResultCD;

                _log.Debug("-----------------------------------------");
                _log.Debug("카테고리조회 End");
                _log.Debug("-----------------------------------------");
            }
            catch (FrameException fe)
            {
                _log.Warning("-----------------------------------------");
                _log.Warning(this.ToString() + ":GetCategoryList():" + fe.ErrCode + ":" + fe.ResultMsg);
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
		/// 장르조회
		/// </summary>
		/// <param name="groupModel"></param>
		public void GetGenreList(GroupModel groupModel)
		{

			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("장르조회 Start");
				_log.Debug("-----------------------------------------");
				
				// 웹서비스 인스턴스 생성
				GroupServicePloxy.GroupService svc = new GroupServicePloxy.GroupService();
				svc.Url = _WebServiceUrl;
			
				// 리모트 모델 생성
				GroupServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.GroupServicePloxy.HeaderModel();
				GroupServicePloxy.GroupModel remoteData   = new AdManagerClient.GroupServicePloxy.GroupModel();

				// 헤더정보 셋트
				remoteHeader.ClientKey  = Header.ClientKey;
				remoteHeader.UserID     = Header.UserID;
				remoteHeader.UserLevel  = Header.UserLevel;
				remoteHeader.UserClass	= Header.UserClass;

				// 호출정보 셋트
                remoteData.SearchType = groupModel.SearchType;
                remoteData.SearchKey = groupModel.SearchKey;
                remoteData.CategoryCode = groupModel.CategoryCode;
                remoteData.InvalidYn = groupModel.InvalidYn;        //무효메뉴 목록 포함 여부
				
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
				groupModel.GenreDataSet = remoteData.GenreDataSet.Copy();
				groupModel.ResultCnt   = remoteData.ResultCnt;
				groupModel.ResultCD    = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("장르조회 End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":GetCategoryList():" + fe.ErrCode + ":" + fe.ResultMsg);
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
        /// 장르조회
        /// </summary>
        /// <param name="groupModel"></param>
        public void GetGenreList2(GroupModel groupModel)
        {

            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug("장르조회 Start");
                _log.Debug("-----------------------------------------");

                // 웹서비스 인스턴스 생성
                GroupServicePloxy.GroupService svc = new GroupServicePloxy.GroupService();
                svc.Url = _WebServiceUrl;

                // 리모트 모델 생성
                GroupServicePloxy.HeaderModel remoteHeader = new AdManagerClient.GroupServicePloxy.HeaderModel();
                GroupServicePloxy.GroupModel remoteData = new AdManagerClient.GroupServicePloxy.GroupModel();

                // 헤더정보 셋트
                remoteHeader.ClientKey = Header.ClientKey;
                remoteHeader.UserID = Header.UserID;
                remoteHeader.UserLevel = Header.UserLevel;
                remoteHeader.UserClass = Header.UserClass;

                // 호출정보 셋트
                remoteData.SearchType = groupModel.SearchType;
                remoteData.SearchKey = groupModel.SearchKey;
                remoteData.CategoryCode = groupModel.CategoryCode;
                remoteData.InvalidYn = groupModel.InvalidYn;        //무효메뉴 목록 포함 여부

                // 웹서비스 호출 타임아웃설정
                svc.Timeout = FrameSystem.m_SystemTimeout;

                // 웹서비스 메소드 호출
                remoteData = svc.GetGenreList2(remoteHeader, remoteData);

                // 결과코드검사
                if (!remoteData.ResultCD.Equals("0000"))
                {
                    throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
                }

                // 결과 셋트
                groupModel.GenreDataSet = remoteData.GenreDataSet.Copy();
                groupModel.ResultCnt = remoteData.ResultCnt;
                groupModel.ResultCD = remoteData.ResultCD;

                _log.Debug("-----------------------------------------");
                _log.Debug("장르조회 End");
                _log.Debug("-----------------------------------------");
            }
            catch (FrameException fe)
            {
                _log.Warning("-----------------------------------------");
                _log.Warning(this.ToString() + ":GetCategoryList():" + fe.ErrCode + ":" + fe.ResultMsg);
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
        /// 그룹매핑정보 조회
        /// </summary>
        /// <param name="groupModel"></param>
        public void GetGroupMapList(GroupModel groupModel)
        {

            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug(" 그룹매핑정보 조회 Start");
                _log.Debug("-----------------------------------------");
				
                // 웹서비스 인스턴스 생성
                GroupServicePloxy.GroupService svc = new GroupServicePloxy.GroupService();
                svc.Url = _WebServiceUrl;
			
                // 리모트 모델 생성
                GroupServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.GroupServicePloxy.HeaderModel();
                GroupServicePloxy.GroupModel    remoteData   = new AdManagerClient.GroupServicePloxy.GroupModel();

                // 헤더정보 셋트
                remoteHeader.ClientKey     = Header.ClientKey;
                remoteHeader.UserID        = Header.UserID;
                remoteHeader.UserLevel     = Header.UserLevel;
                remoteHeader.UserClass	   = Header.UserClass;

                // 호출정보 셋트
                remoteData.SearchKey = "";
                remoteData.CategoryCode = "";
                remoteData.InvalidYn = groupModel.InvalidYn;        //무효메뉴 목록 포함 여부
				
                // 웹서비스 호출 타임아웃설정
                svc.Timeout = FrameSystem.m_SystemTimeout;

                // 웹서비스 메소드 호출
                remoteData = svc.GetGroupMapList(remoteHeader, remoteData);

                // 결과코드검사
                if(!remoteData.ResultCD.Equals("0000"))
                {
                    throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
                }

                // 결과 셋트
                groupModel.GroupMapDataSet = remoteData.GroupMapDataSet.Copy();
                groupModel.ResultCnt   = remoteData.ResultCnt;
                groupModel.ResultCD    = remoteData.ResultCD;

                _log.Debug("-----------------------------------------");
                _log.Debug("그룹매핑정보 End");
                _log.Debug("-----------------------------------------");
            }
            catch(FrameException fe)
            {
                _log.Warning("-----------------------------------------");
                _log.Warning( this.ToString() + ":GetCategoryList():" + fe.ErrCode + ":" + fe.ResultMsg);
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
		/// 채널조회
		/// </summary>
		/// <param name="groupModel"></param>
		public void GetChannelNoPopList(GroupModel groupModel)
		{

			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("채널조회 Start");
				_log.Debug("-----------------------------------------");
				
				// 웹서비스 인스턴스 생성
				GroupServicePloxy.GroupService svc = new GroupServicePloxy.GroupService();
				svc.Url = _WebServiceUrl;
			
				// 리모트 모델 생성
				GroupServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.GroupServicePloxy.HeaderModel();
				GroupServicePloxy.GroupModel remoteData   = new AdManagerClient.GroupServicePloxy.GroupModel();

				// 헤더정보 셋트
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;
				remoteHeader.UserLevel     = Header.UserLevel;
				remoteHeader.UserClass	   = Header.UserClass;

				// 호출정보 셋트

				remoteData.SearchKey       = groupModel.SearchKey;				
				remoteData.MediaCode       = groupModel.MediaCode;
				remoteData.CategoryCode    = groupModel.CategoryCode;
				remoteData.GenreCode       = groupModel.GenreCode;
				
				// 웹서비스 호출 타임아웃설정
				svc.Timeout = FrameSystem.m_SystemTimeout;

				// 웹서비스 메소드 호출
				remoteData = svc.GetChannelNoPopList(remoteHeader, remoteData);

				// 결과코드검사
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// 결과 셋트
				groupModel.ChannelDataSet = remoteData.ChannelDataSet.Copy();
				groupModel.ResultCnt   = remoteData.ResultCnt;
				groupModel.ResultCD    = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("채널조회 End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":GetCategoryList():" + fe.ErrCode + ":" + fe.ResultMsg);
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
        /// 채널조회
        /// </summary>
        /// <param name="groupModel"></param>
        public void GetChannelNoPopList2(GroupModel groupModel)
        {

            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug("채널조회 Start");
                _log.Debug("-----------------------------------------");

                // 웹서비스 인스턴스 생성
                GroupServicePloxy.GroupService svc = new GroupServicePloxy.GroupService();
                svc.Url = _WebServiceUrl;

                // 리모트 모델 생성
                GroupServicePloxy.HeaderModel remoteHeader = new AdManagerClient.GroupServicePloxy.HeaderModel();
                GroupServicePloxy.GroupModel remoteData = new AdManagerClient.GroupServicePloxy.GroupModel();

                // 헤더정보 셋트
                remoteHeader.ClientKey = Header.ClientKey;
                remoteHeader.UserID = Header.UserID;
                remoteHeader.UserLevel = Header.UserLevel;
                remoteHeader.UserClass = Header.UserClass;

                // 호출정보 셋트

                remoteData.SearchKey = groupModel.SearchKey;
                remoteData.MediaCode = groupModel.MediaCode;
                remoteData.CategoryCode = groupModel.CategoryCode;
                remoteData.GenreCode = groupModel.GenreCode;

                // 웹서비스 호출 타임아웃설정
                svc.Timeout = FrameSystem.m_SystemTimeout;

                // 웹서비스 메소드 호출
                remoteData = svc.GetChannelNoPopList2(remoteHeader, remoteData);

                // 결과코드검사
                if (!remoteData.ResultCD.Equals("0000"))
                {
                    throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
                }

                // 결과 셋트
                groupModel.ChannelDataSet = remoteData.ChannelDataSet.Copy();
                groupModel.ResultCnt = remoteData.ResultCnt;
                groupModel.ResultCD = remoteData.ResultCD;

                _log.Debug("-----------------------------------------");
                _log.Debug("채널조회 End");
                _log.Debug("-----------------------------------------");
            }
            catch (FrameException fe)
            {
                _log.Warning("-----------------------------------------");
                _log.Warning(this.ToString() + ":GetCategoryList():" + fe.ErrCode + ":" + fe.ResultMsg);
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
		/// [S1] 회차가져오기 웹서비스를 호출합니다
		/// </summary>
		/// <param name="groupModel"></param>
		public void GetSeriesList(GroupModel groupModel)
		{
			try
			{
				// 웹서비스 인스턴스 생성
				GroupServicePloxy.GroupService svc = new GroupServicePloxy.GroupService();
				svc.Url = _WebServiceUrl;
			
				// 리모트 모델 생성
				GroupServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.GroupServicePloxy.HeaderModel();
				GroupServicePloxy.GroupModel	remoteData   = new AdManagerClient.GroupServicePloxy.GroupModel();

				// 헤더정보 셋트
				remoteHeader.ClientKey  = Header.ClientKey;
				remoteHeader.UserID     = Header.UserID;
				remoteHeader.UserLevel  = Header.UserLevel;
				remoteHeader.UserClass	= Header.UserClass;

				// 호출정보 셋트
				remoteData.SearchKey    = groupModel.SearchKey;				
				remoteData.MediaCode    = groupModel.MediaCode;
				remoteData.CategoryCode = groupModel.CategoryCode;
				remoteData.GenreCode	= groupModel.GenreCode;
				remoteData.ChannelNo	= groupModel.ChannelNo;
                remoteData.InvalidYn = groupModel.InvalidYn;        //무효메뉴 목록 포함 여부
				
				svc.Timeout = FrameSystem.m_SystemTimeout;
				remoteData = svc.GetSeriesList(remoteHeader, remoteData);

				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// 결과 셋트
				groupModel.SeriesDataSet = remoteData.SeriesDataSet.Copy();
				groupModel.ResultCnt   = remoteData.ResultCnt;
				groupModel.ResultCD    = remoteData.ResultCD;
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":GetSeriesList():" + fe.ErrCode + ":" + fe.ResultMsg);
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
        /// [S1] 회차가져오기 웹서비스를 호출합니다
        /// </summary>
        /// <param name="groupModel"></param>
        public void GetSeriesList2(GroupModel groupModel)
        {
            try
            {
                // 웹서비스 인스턴스 생성
                GroupServicePloxy.GroupService svc = new GroupServicePloxy.GroupService();
                svc.Url = _WebServiceUrl;

                // 리모트 모델 생성
                GroupServicePloxy.HeaderModel remoteHeader = new AdManagerClient.GroupServicePloxy.HeaderModel();
                GroupServicePloxy.GroupModel remoteData = new AdManagerClient.GroupServicePloxy.GroupModel();

                // 헤더정보 셋트
                remoteHeader.ClientKey = Header.ClientKey;
                remoteHeader.UserID = Header.UserID;
                remoteHeader.UserLevel = Header.UserLevel;
                remoteHeader.UserClass = Header.UserClass;

                // 호출정보 셋트
                remoteData.SearchKey = groupModel.SearchKey;
                remoteData.MediaCode = groupModel.MediaCode;
                remoteData.CategoryCode = groupModel.CategoryCode;
                remoteData.GenreCode = groupModel.GenreCode;
                remoteData.ChannelNo = groupModel.ChannelNo;
                remoteData.InvalidYn = groupModel.InvalidYn;        //무효메뉴 목록 포함 여부

                svc.Timeout = FrameSystem.m_SystemTimeout;
                remoteData = svc.GetSeriesList2(remoteHeader, remoteData);

                if (!remoteData.ResultCD.Equals("0000"))
                {
                    throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
                }

                // 결과 셋트
                groupModel.SeriesDataSet = remoteData.SeriesDataSet.Copy();
                groupModel.ResultCnt = remoteData.ResultCnt;
                groupModel.ResultCD = remoteData.ResultCD;
            }
            catch (FrameException fe)
            {
                _log.Warning("-----------------------------------------");
                _log.Warning(this.ToString() + ":GetSeriesList():" + fe.ErrCode + ":" + fe.ResultMsg);
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
		/// Service 호출을 위한 메소드
		/// </summary>
		public bool GetGroupDetail(BaseModel baseModel)
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
		/// <param name="groupModel"></param>
		public void SetGroupUpdate(GroupModel groupModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("사용자정보수정 Start");
				_log.Debug("-----------------------------------------");


				//입력데이터의 Validation 검사			
				if(groupModel.GroupName.Length > 1000) 
				{
					throw new FrameException("그룹명의 길이는 1000Bytes를 초과할 수 없습니다.");
				}						
				if(groupModel.Comment.Length > 1000) 
				{
					throw new FrameException("비고란의 길이는 1000Bytes를 초과할 수 없습니다.");
				}

	

				// 웹서비스 인스턴스 생성
				GroupServicePloxy.GroupService svc = new GroupServicePloxy.GroupService();
				svc.Url = _WebServiceUrl;
			
				// 리모트 모델 생성
				GroupServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.GroupServicePloxy.HeaderModel();
				GroupServicePloxy.GroupModel remoteData   = new AdManagerClient.GroupServicePloxy.GroupModel();

				// 헤더정보 셋트
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;

				// 호출정보셋트
				remoteData.MediaCode       = groupModel.MediaCode;
				remoteData.GroupCode       = groupModel.GroupCode;
				remoteData.GroupName     = groupModel.GroupName;		
				remoteData.Comment  = groupModel.Comment;		
				remoteData.UseYn     = groupModel.UseYn;		
		
				

				// 웹서비스 호출 타임아웃설정
				svc.Timeout = FrameSystem.m_SystemTimeout;

				// 웹서비스 메소드 호출
				remoteData = svc.SetGroupUpdate(remoteHeader, remoteData);

				// 결과코드검사
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// 결과 셋트
				groupModel.ResultCnt   = remoteData.ResultCnt;
				groupModel.ResultCD    = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("사용자정보수정 End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":SetGroupUpdate():" + fe.ErrCode + ":" + fe.ResultMsg);
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
		/// 사용자정보 수정
		/// </summary>
		/// <param name="groupModel"></param>
		public void SetGroupGenreUpdate(GroupModel groupModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("사용자정보수정 Start");
				_log.Debug("-----------------------------------------");


				//입력데이터의 Validation 검사			
				if(groupModel.GroupName.Length > 1000) 
				{
					throw new FrameException("그룹명의 길이는 1000Bytes를 초과할 수 없습니다.");
				}						
				if(groupModel.Comment.Length > 1000) 
				{
					throw new FrameException("비고란의 길이는 1000Bytes를 초과할 수 없습니다.");
				}

	

				// 웹서비스 인스턴스 생성
				GroupServicePloxy.GroupService svc = new GroupServicePloxy.GroupService();
				svc.Url = _WebServiceUrl;
			
				// 리모트 모델 생성
				GroupServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.GroupServicePloxy.HeaderModel();
				GroupServicePloxy.GroupModel remoteData   = new AdManagerClient.GroupServicePloxy.GroupModel();

				// 헤더정보 셋트
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;

				// 호출정보셋트				
				remoteData.GroupCode       = groupModel.GroupCode;
				remoteData.CategoryCode     = groupModel.CategoryCode;		
				remoteData.GenreCode     = groupModel.GenreCode;							

				// 웹서비스 호출 타임아웃설정
				svc.Timeout = FrameSystem.m_SystemTimeout;

				// 웹서비스 메소드 호출
				remoteData = svc.SetGroupGenreUpdate(remoteHeader, remoteData);

				// 결과코드검사
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// 결과 셋트
				groupModel.ResultCnt   = remoteData.ResultCnt;
				groupModel.ResultCD    = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("사용자정보수정 End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":SetGroupUpdate():" + fe.ErrCode + ":" + fe.ResultMsg);
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
		/// 사용자정보 수정
		/// </summary>
		/// <param name="groupModel"></param>
		public void SetGroupChannelUpdate(GroupModel groupModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("사용자정보수정 Start");
				_log.Debug("-----------------------------------------");


				//입력데이터의 Validation 검사			
				if(groupModel.GroupName.Length > 1000) 
				{
					throw new FrameException("그룹명의 길이는 1000Bytes를 초과할 수 없습니다.");
				}						
				if(groupModel.Comment.Length > 1000) 
				{
					throw new FrameException("비고란의 길이는 1000Bytes를 초과할 수 없습니다.");
				}

	

				// 웹서비스 인스턴스 생성
				GroupServicePloxy.GroupService svc = new GroupServicePloxy.GroupService();
				svc.Url = _WebServiceUrl;
			
				// 리모트 모델 생성
				GroupServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.GroupServicePloxy.HeaderModel();
				GroupServicePloxy.GroupModel remoteData   = new AdManagerClient.GroupServicePloxy.GroupModel();

				// 헤더정보 셋트
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;

				// 호출정보셋트
				remoteData.GroupCode       = groupModel.GroupCode;
				remoteData.CategoryCode     = groupModel.CategoryCode;		
				remoteData.GenreCode     = groupModel.GenreCode;
				remoteData.ChannelNo     = groupModel.ChannelNo;			

				// 웹서비스 호출 타임아웃설정
				svc.Timeout = FrameSystem.m_SystemTimeout;

				// 웹서비스 메소드 호출
				remoteData = svc.SetGroupChannelUpdate(remoteHeader, remoteData);

				// 결과코드검사
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// 결과 셋트
				groupModel.ResultCnt   = remoteData.ResultCnt;
				groupModel.ResultCD    = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("사용자정보수정 End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":SetGroupUpdate():" + fe.ErrCode + ":" + fe.ResultMsg);
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
		/// 그룹에 회차를 추가합니다
		/// </summary>
		/// <param name="groupModel"></param>
		public void SetGroupSerieslUpdate(GroupModel groupModel)
		{
			try
			{
				// 웹서비스 인스턴스 생성
				GroupServicePloxy.GroupService svc = new GroupServicePloxy.GroupService();
				svc.Url = _WebServiceUrl;
			
				// 리모트 모델 생성
				GroupServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.GroupServicePloxy.HeaderModel();
				GroupServicePloxy.GroupModel remoteData   = new AdManagerClient.GroupServicePloxy.GroupModel();

				// 헤더정보 셋트
				remoteHeader.ClientKey  = Header.ClientKey;
				remoteHeader.UserID     = Header.UserID;

				// 호출정보셋트
				remoteData.GroupCode    = groupModel.GroupCode;
				remoteData.CategoryCode	= groupModel.CategoryCode;		
				remoteData.GenreCode    = groupModel.GenreCode;
				remoteData.ChannelNo    = groupModel.ChannelNo;			
				remoteData.SeriesNo		= groupModel.SeriesNo;

				svc.Timeout = FrameSystem.m_SystemTimeout;
				remoteData = svc.SetGroupSeriesUpdate(remoteHeader, remoteData);

				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				groupModel.ResultCnt   = remoteData.ResultCnt;
				groupModel.ResultCD    = remoteData.ResultCD;
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":SetGroupSerieslUpdate():" + fe.ErrCode + ":" + fe.ResultMsg);
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
		/// <param name="groupModel"></param>
		/// <returns></returns>
		public void SetGroupAdd(GroupModel groupModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("사용자추가 Start");
				_log.Debug("-----------------------------------------");
				
				if(groupModel.GroupName.Length > 1000) 
				{
					throw new FrameException("그룹명의 길이는 1000Bytes를 초과할 수 없습니다.");
				}						
				if(groupModel.Comment.Length > 1000) 
				{
					throw new FrameException("비고란의 길이는 1000Bytes를 초과할 수 없습니다.");
				}

				// 웹서비스 인스턴스 생성
				GroupServicePloxy.GroupService svc = new GroupServicePloxy.GroupService();
				svc.Url = _WebServiceUrl;
				
				// 리모트 모델 생성
				GroupServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.GroupServicePloxy.HeaderModel();
				GroupServicePloxy.GroupModel remoteData   = new AdManagerClient.GroupServicePloxy.GroupModel();

				// 헤더정보 셋트
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;

				// 호출정보셋트				
				remoteData.MediaCode       = groupModel.MediaCode;
				remoteData.GroupName     = groupModel.GroupName;					
				remoteData.Comment  = groupModel.Comment;		
				remoteData.UseYn     = groupModel.UseYn;				
					
				// 웹서비스 호출 타임아웃설정
				svc.Timeout = FrameSystem.m_SystemTimeout;

				// 웹서비스 메소드 호출
				remoteData = svc.SetGroupCreate(remoteHeader, remoteData);

				// 결과코드검사
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// 결과 셋트
				groupModel.ResultCnt   = remoteData.ResultCnt;
				groupModel.ResultCD    = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("사용자추가 End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":SetGroupCreate():" + fe.ErrCode + ":" + fe.ResultMsg);
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
		/// <param name="groupModel"></param>
		/// <returns></returns>
		public void SetGroupDetailAdd(GroupModel groupModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("사용자추가 Start");
				_log.Debug("-----------------------------------------");
				
				if(groupModel.GroupName.Length > 1000) 
				{
					throw new FrameException("그룹명의 길이는 1000Bytes를 초과할 수 없습니다.");
				}						
				if(groupModel.Comment.Length > 1000) 
				{
					throw new FrameException("비고란의 길이는 1000Bytes를 초과할 수 없습니다.");
				}

				// 웹서비스 인스턴스 생성
				GroupServicePloxy.GroupService svc = new GroupServicePloxy.GroupService();
				svc.Url = _WebServiceUrl;
				
				// 리모트 모델 생성
				GroupServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.GroupServicePloxy.HeaderModel();
				GroupServicePloxy.GroupModel remoteData   = new AdManagerClient.GroupServicePloxy.GroupModel();

				// 헤더정보 셋트
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;

				// 호출정보셋트				
				remoteData.GroupCode     = groupModel.GroupCode;					
				remoteData.CategoryCode  = groupModel.CategoryCode;		
									
				// 웹서비스 호출 타임아웃설정
				svc.Timeout = FrameSystem.m_SystemTimeout;

				// 웹서비스 메소드 호출
				remoteData = svc.SetGroupDetailCreate(remoteHeader, remoteData);

				// 결과코드검사
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// 결과 셋트
				groupModel.ResultCnt   = remoteData.ResultCnt;
				groupModel.ResultCD    = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("사용자추가 End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":SetGroupCreate():" + fe.ErrCode + ":" + fe.ResultMsg);
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
		public void SetGroupDelete(GroupModel groupModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("그룹삭제 Start");
				_log.Debug("-----------------------------------------");

				// 웹서비스 인스턴스 생성
				GroupServicePloxy.GroupService svc = new GroupServicePloxy.GroupService();
				svc.Url = _WebServiceUrl;
				
				// 리모트 모델 생성
				GroupServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.GroupServicePloxy.HeaderModel();
				GroupServicePloxy.GroupModel remoteData   = new AdManagerClient.GroupServicePloxy.GroupModel();

				// 헤더정보 셋트
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;
				// 호출정보셋트
				remoteData.MediaCode       = groupModel.MediaCode;
				remoteData.GroupCode       = groupModel.GroupCode;
					
				// 웹서비스 호출 타임아웃설정
				svc.Timeout = FrameSystem.m_SystemTimeout;

				// 웹서비스 메소드 호출
				remoteData = svc.SetGroupDelete(remoteHeader, remoteData);

				// 결과코드검사
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// 결과 셋트
				groupModel.ResultCnt   = remoteData.ResultCnt;
				groupModel.ResultCD    = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("그룹삭제 End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":SetGroupDelete():" + fe.ErrCode + ":" + fe.ResultMsg);
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
		public void SetGroupDetailDelete(GroupModel groupModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("편성그룹 내역삭제 Start");
				_log.Debug("-----------------------------------------");

				GroupServicePloxy.GroupService svc = new GroupServicePloxy.GroupService();
				svc.Url = _WebServiceUrl;
				svc.Timeout = FrameSystem.m_SystemTimeout;
				
				GroupServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.GroupServicePloxy.HeaderModel();
				GroupServicePloxy.GroupModel remoteData   = new AdManagerClient.GroupServicePloxy.GroupModel();


				remoteHeader.ClientKey  = Header.ClientKey;
				remoteHeader.UserID     = Header.UserID;
				remoteData.GroupCode    = groupModel.GroupCode;
				remoteData.CategoryCode	= groupModel.CategoryCode;
				remoteData.GenreCode    = groupModel.GenreCode;
				remoteData.ChannelNo    = groupModel.ChannelNo;
				remoteData.SeriesNo		= groupModel.SeriesNo;
					

				remoteData = svc.SetGroupDetailDelete(remoteHeader, remoteData);

				// 결과코드검사
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// 결과 셋트
				groupModel.ResultCnt   = remoteData.ResultCnt;
				groupModel.ResultCD    = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("편성그룹 내역삭제 End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":SetGroupDelete():" + fe.ErrCode + ":" + fe.ResultMsg);
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
