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
    public class ChannelGroupSearch_pManager : BaseManager
    {
        /// <summary>
        /// 생성자
        /// </summary>
        /// <param name="systemModel"></param>
        /// <param name="commonModel"></param>

        public ChannelGroupSearch_pManager(SystemModel systemModel, CommonModel commonModel) : base(systemModel, commonModel)
        {
            _log = FrameSystem.oLog;
            _module = "CONTENT";
            _Host  = FrameSystem.m_WebServer_Host;
            _Port  = FrameSystem.m_WebServer_Port;
            _Path  = FrameSystem.m_WebServer_App + "/Schedule/ChannelGroupService.asmx";
        }

        /// <summary>
        /// 장르정보조회
        /// </summary>
        /// <param name="channelGroupModel"></param>
        public void GetGenreList(ChannelGroupModel channelGroupModel)
        {

            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug("장르목록조회 Start");
                _log.Debug("-----------------------------------------");
				
                // 웹서비스 인스턴스 생성
                GenreGroupServicePloxy.GenreGroupService svc = new GenreGroupServicePloxy.GenreGroupService();
                svc.Url = "http://" + _Host + ":" + _Port + "/" + FrameSystem.m_WebServer_App + "/Schedule/GenreGroupService.asmx";			
                // 리모트 모델 생성
                GenreGroupServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.GenreGroupServicePloxy.HeaderModel();
                GenreGroupServicePloxy.GenreGroupModel remoteData   = new AdManagerClient.GenreGroupServicePloxy.GenreGroupModel();

                // 헤더정보 셋트
                remoteHeader.ClientKey     = Header.ClientKey;
                remoteHeader.UserID        = Header.UserID;

                // 호출정보 셋트
                remoteData.SearchKey       = channelGroupModel.SearchKey;
                remoteData.MediaCode       = channelGroupModel.MediaCode;
				

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
                channelGroupModel.ChannelGroup_pDataSet = remoteData.GenreGroupDataSet.Copy();
                channelGroupModel.ResultCnt   = remoteData.ResultCnt;
                channelGroupModel.ResultCD    = remoteData.ResultCD;

                
                _log.Debug("-----------------------------------------");
                _log.Debug("장르목록조회 End");
                _log.Debug("-----------------------------------------");
            }
            catch(FrameException fe)
            {
                _log.Warning("-----------------------------------------");
                _log.Warning( this.ToString() + ":GetChannelGroupListPopUp():" + fe.ErrCode + ":" + fe.ResultMsg);
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
        /// 채널정보조회
        /// </summary>
        /// <param name="channelGroupModel"></param>
        public void GetChannelList(ChannelGroupModel channelGroupModel)
        {

            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug("장르목록조회 Start");
                _log.Debug("-----------------------------------------");
				
                // 웹서비스 인스턴스 생성
                ChannelGroupServicePloxy.ChannelGroupService svc = new ChannelGroupServicePloxy.ChannelGroupService();
			    svc.Url = _WebServiceUrl;
                // 리모트 모델 생성
                ChannelGroupServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.ChannelGroupServicePloxy.HeaderModel();
                ChannelGroupServicePloxy.ChannelGroupModel remoteData   = new AdManagerClient.ChannelGroupServicePloxy.ChannelGroupModel();

                // 헤더정보 셋트
                remoteHeader.ClientKey     = Header.ClientKey;
                remoteHeader.UserID        = Header.UserID;


                // 호출정보 셋트
                remoteData.SearchKey       = channelGroupModel.SearchKey;
                remoteData.MediaCode       = channelGroupModel.MediaCode;
                remoteData.CategoryCode    = channelGroupModel.CategoryCode;
                remoteData.GenreCode    = channelGroupModel.GenreCode;
				
                // 웹서비스 호출 타임아웃설정
                svc.Timeout = FrameSystem.m_SystemTimeout;
                // 웹서비스 메소드 호출
                remoteData = svc.GetChannelList_p(remoteHeader, remoteData);

                // 결과코드검사
                if(!remoteData.ResultCD.Equals("0000"))
                {
                    throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
                }

                // 결과 셋트
                channelGroupModel.ChannelGroup_pDataSet = remoteData.ChannelGroupDataSet.Copy();
                channelGroupModel.ResultCnt   = remoteData.ResultCnt;
                channelGroupModel.ResultCD    = remoteData.ResultCD;

                
                _log.Debug("-----------------------------------------");
                _log.Debug("장르목록조회 End");
                _log.Debug("-----------------------------------------");
            }
            catch(FrameException fe)
            {
                _log.Warning("-----------------------------------------");
                _log.Warning( this.ToString() + ":GetChannelGroupListPopUp():" + fe.ErrCode + ":" + fe.ResultMsg);
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
		/// 엑셀_채널정보조회
		/// </summary>
		/// <param name="channelGroupModel"></param>
		public void GetChannelList_Excel(ChannelGroupModel channelGroupModel)
		{

			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("엑셀 채널목록조회 Start");
				_log.Debug("-----------------------------------------");
				
				// 웹서비스 인스턴스 생성
				ChannelGroupServicePloxy.ChannelGroupService svc = new ChannelGroupServicePloxy.ChannelGroupService();
				svc.Url = _WebServiceUrl;
				// 리모트 모델 생성
				ChannelGroupServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.ChannelGroupServicePloxy.HeaderModel();
				ChannelGroupServicePloxy.ChannelGroupModel remoteData   = new AdManagerClient.ChannelGroupServicePloxy.ChannelGroupModel();

				// 헤더정보 셋트
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;


				// 호출정보 셋트
				remoteData.SearchKey       = channelGroupModel.SearchKey;
				remoteData.MediaCode       = channelGroupModel.MediaCode;				
				
				// 웹서비스 호출 타임아웃설정
				svc.Timeout = FrameSystem.m_SystemTimeout;
				// 웹서비스 메소드 호출
				remoteData = svc.GetChannelList_Excel(remoteHeader, remoteData);

				// 결과코드검사
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// 결과 셋트
				channelGroupModel.ChannelGroup_pDataSet = remoteData.ChannelGroupDataSet.Copy();
				channelGroupModel.ResultCnt   = remoteData.ResultCnt;
				channelGroupModel.ResultCD    = remoteData.ResultCD;

                
				_log.Debug("-----------------------------------------");
				_log.Debug("엑셀 채널목록조회 End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":GetChannelList_Excel():" + fe.ErrCode + ":" + fe.ResultMsg);
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