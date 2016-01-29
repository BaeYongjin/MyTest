// ===============================================================================
// UserUpdate Manager  for Charites Project
//
// UserUpdateManager.cs
//
// 장르정보 저장 서비스를 호출합니다. 
//
// ===============================================================================
// Release history
// 2007.06.26 송명환 v1.0
// ===============================================================================
// Copyright (C) 2007 G-Inno Systems Inc.
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
    /// 장르정보 웹서비스를 호출합니다. 
    /// </summary>
    public class GenreGroupSearch_pManager : BaseManager
    {
        /// <summary>
        /// 생성자
        /// </summary>
        /// <param name="systemModel"></param>
        /// <param name="commonModel"></param>

        public GenreGroupSearch_pManager(SystemModel systemModel, CommonModel commonModel) : base(systemModel, commonModel)
        {
            _log = FrameSystem.oLog;
            _module = "CONTENT";
            _Host  = FrameSystem.m_WebServer_Host;
            _Port  = FrameSystem.m_WebServer_Port;
            _Path  = FrameSystem.m_WebServer_App + "/Schedule/GenreGroupService.asmx";
        }

		/// <summary>
		/// 장르정보조회(엑셀)
		/// </summary>
		/// <param name="genreGroupModel"></param>
		public void GetInspectGenreGroupList_p(GenreGroupModel genreGroupModel)
		{

			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("장르목록조회 Start");
				_log.Debug("-----------------------------------------");
				
				// 웹서비스 인스턴스 생성
				GenreGroupServicePloxy.GenreGroupService svc = new GenreGroupServicePloxy.GenreGroupService();
				svc.Url = _WebServiceUrl;			
				// 리모트 모델 생성
				GenreGroupServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.GenreGroupServicePloxy.HeaderModel();
				GenreGroupServicePloxy.GenreGroupModel remoteData   = new AdManagerClient.GenreGroupServicePloxy.GenreGroupModel();

				// 헤더정보 셋트
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;

				// 호출정보 셋트
				remoteData.SearchKey       = genreGroupModel.SearchKey;
				remoteData.MediaCode       = genreGroupModel.MediaCode;
				
				// 웹서비스 호출 타임아웃설정
				svc.Timeout = FrameSystem.m_SystemTimeout;
				// 웹서비스 메소드 호출
				remoteData = svc.GetInspectGenreGroupList_p(remoteHeader, remoteData);

				// 결과코드검사
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// 결과 셋트
				genreGroupModel.GenreGroupDataSet = remoteData.GenreGroupDataSet.Copy();
				genreGroupModel.ResultCnt   = remoteData.ResultCnt;
				genreGroupModel.ResultCD    = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("장르목록조회 End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":GetGenreGroupListPopUp():" + fe.ErrCode + ":" + fe.ResultMsg);
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
        /// 장르정보조회
        /// </summary>
        /// <param name="genreGroupModel"></param>
        public void GetGenreGroupList_p(GenreGroupModel genreGroupModel)
        {
            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug("장르목록조회 Start");
                _log.Debug("-----------------------------------------");
				
                // 웹서비스 인스턴스 생성
                GenreGroupServicePloxy.GenreGroupService svc = new GenreGroupServicePloxy.GenreGroupService();
                svc.Url = _WebServiceUrl;			
                // 리모트 모델 생성
                GenreGroupServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.GenreGroupServicePloxy.HeaderModel();
                GenreGroupServicePloxy.GenreGroupModel remoteData   = new AdManagerClient.GenreGroupServicePloxy.GenreGroupModel();

                // 헤더정보 셋트
                remoteHeader.ClientKey     = Header.ClientKey;
                remoteHeader.UserID        = Header.UserID;

                // 호출정보 셋트
                remoteData.SearchKey       = genreGroupModel.SearchKey;
                remoteData.MediaCode       = genreGroupModel.MediaCode;
				
                // 웹서비스 호출 타임아웃설정
                svc.Timeout = FrameSystem.m_SystemTimeout;
                // 웹서비스 메소드 호출
				remoteData = svc.GetGenreGroupList_p(remoteHeader, remoteData);

                // 결과코드검사
                if(!remoteData.ResultCD.Equals("0000"))
                {
                    throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
                }

                // 결과 셋트
				genreGroupModel.GenreGroup_pDataSet	= remoteData.GenreGroup_pDataSet.Copy();
                genreGroupModel.GenreGroupDataSet	= remoteData.GenreGroupDataSet.Copy();
                genreGroupModel.ResultCnt   = remoteData.ResultCnt;
                genreGroupModel.ResultCD    = remoteData.ResultCD;

                _log.Debug("-----------------------------------------");
                _log.Debug("장르목록조회 End");
                _log.Debug("-----------------------------------------");
            }
            catch(FrameException fe)
            {
                _log.Warning("-----------------------------------------");
                _log.Warning( this.ToString() + ":GetGenreGroupListPopUp():" + fe.ErrCode + ":" + fe.ResultMsg);
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
		/// 실시간채널 정보 조회
		/// </summary>
		/// <param name="genreGroupModel"></param>
		public void GetChannelList_p(GenreGroupModel genreGroupModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("채널목록조회 Start");
				_log.Debug("-----------------------------------------");

				GenreGroupServicePloxy.GenreGroupService svc = new GenreGroupServicePloxy.GenreGroupService();
				
				// 리모트 모델 생성
				GenreGroupServicePloxy.HeaderModel remoteHeader = new AdManagerClient.GenreGroupServicePloxy.HeaderModel();
				GenreGroupServicePloxy.GenreGroupModel remoteData = new AdManagerClient.GenreGroupServicePloxy.GenreGroupModel();

				// 헤더정보 셋트
				remoteHeader.ClientKey = Header.ClientKey;
				remoteHeader.UserID = Header.UserID;

				// 호출정보 셋트
				remoteData.SearchKey = genreGroupModel.SearchKey;
				remoteData.MediaCode = genreGroupModel.MediaCode;

				svc.Url = _WebServiceUrl;
				svc.Timeout = FrameSystem.m_SystemTimeout;
				// 웹서비스 메소드 호출
				remoteData = svc.GetChannelList_p(remoteHeader, remoteData);

				// 결과코드검사
				if (!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// 결과 셋트
				genreGroupModel.GenreGroup_pDataSet = remoteData.GenreGroup_pDataSet.Copy();
				genreGroupModel.GenreGroupDataSet = remoteData.GenreGroupDataSet.Copy();
				genreGroupModel.ResultCnt = remoteData.ResultCnt;
				genreGroupModel.ResultCD = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("채널목록조회 End");
				_log.Debug("-----------------------------------------");
			}
			catch (FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning(this.ToString() + ":GetChannelList_p():" + fe.ErrCode + ":" + fe.ResultMsg);
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