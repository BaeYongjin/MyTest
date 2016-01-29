// ===============================================================================
// UserUpdate Manager  for Charites Project
//
// UserUpdateManager.cs
//
// 장르그룹정보 저장 서비스를 호출합니다. 
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
    /// 장르그룹정보 웹서비스를 호출합니다. 
    /// </summary>
    public class ChannelGroupManager : BaseManager
    {
        /// <summary>
        /// 생성자
        /// </summary>
        /// <param name="systemModel"></param>
        /// <param name="commonModel"></param>

        public ChannelGroupManager(SystemModel systemModel, CommonModel commonModel) : base(systemModel, commonModel)
        {
            _log = FrameSystem.oLog;
            _module = "CONTENT";
            _Host  = FrameSystem.m_WebServer_Host;
            _Port  = FrameSystem.m_WebServer_Port;
            _Path  = FrameSystem.m_WebServer_App + "/Schedule/ChannelGroupService.asmx";
        }

        /// <summary>
        /// 미디어콤보정보조회
        /// </summary>
        /// <param name="channelGroupModel"></param>
        public void GetMediaList(ChannelGroupModel channelGroupModel)
        {

            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug("미디어콤보 목록조회 Start");
                _log.Debug("-----------------------------------------");
				
                // 웹서비스 인스턴스 생성
                MediaInfoServicePloxy.MediaInfoService svc = new MediaInfoServicePloxy.MediaInfoService();
			    svc.Url = "http://" + _Host + ":" + _Port + "/AdManagerWebService/Media/MediaInfoService.asmx";
                // 리모트 모델 생성
                MediaInfoServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.MediaInfoServicePloxy.HeaderModel();
                MediaInfoServicePloxy.MediaInfoModel remoteData   = new AdManagerClient.MediaInfoServicePloxy.MediaInfoModel();

                // 헤더정보 셋트
                remoteHeader.ClientKey     = Header.ClientKey;
                remoteHeader.UserID        = Header.UserID;

                // 호출정보 셋트
                remoteData.SearchKey       = channelGroupModel.SearchKey;
                remoteData.MediaCode = channelGroupModel.MediaCode;
				
                // 웹서비스 호출 타임아웃설정
                svc.Timeout = FrameSystem.m_SystemTimeout;
                // 웹서비스 메소드 호출
                remoteData = svc.GetUsersList(remoteHeader, remoteData);

                // 결과코드검사
                if(!remoteData.ResultCD.Equals("0000"))
                {
                    throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
                }

                // 결과 셋트
                channelGroupModel.MediaDataSet = remoteData.UserDataSet.Copy();
                channelGroupModel.ResultCnt   = remoteData.ResultCnt;
                channelGroupModel.ResultCD    = remoteData.ResultCD;

                _log.Debug("-----------------------------------------");
                _log.Debug("미디어콤보 목록조회 End");
                _log.Debug("-----------------------------------------");
            }
            catch(FrameException fe)
            {
                _log.Warning("-----------------------------------------");
                _log.Warning( this.ToString() + ":GetMediaList():" + fe.ErrCode + ":" + fe.ResultMsg);
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
        /// 장르그룹정보조회
        /// </summary>
        /// <param name="channelGroupModel"></param>
        public void GetChannelGroupList(ChannelGroupModel channelGroupModel)
        {

            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug("장르그룹목록조회 Start");
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
                remoteData = svc.GetChannelGroupList(remoteHeader, remoteData);

                // 결과코드검사
                if(!remoteData.ResultCD.Equals("0000"))
                {
                    throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
                }

                // 결과 셋트
                channelGroupModel.ChannelGroupDataSet = remoteData.ChannelGroupDataSet.Copy();
                channelGroupModel.ResultCnt   = remoteData.ResultCnt;
                channelGroupModel.ResultCD    = remoteData.ResultCD;

                _log.Debug("-----------------------------------------");
                _log.Debug("장르그룹목록조회 End");
                _log.Debug("-----------------------------------------");
            }
            catch(FrameException fe)
            {
                _log.Warning("-----------------------------------------");
                _log.Warning( this.ToString() + ":GetChannelGroupList():" + fe.ErrCode + ":" + fe.ResultMsg);
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
        /// 장르그룹 디테일 목록조회
        /// </summary>
        /// <param name="channelGroupModel"></param>
        public void GetChannelGroupDetailList(ChannelGroupModel channelGroupModel)
        {

            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug("장르그룹 디테일 조회 Start");
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
                remoteData.MediaCode       = channelGroupModel.MediaCode;
                remoteData.AdGroupCode      = channelGroupModel.AdGroupCode;
				
                // 웹서비스 호출 타임아웃설정
                svc.Timeout = FrameSystem.m_SystemTimeout;
                // 웹서비스 메소드 호출
                remoteData = svc.GetChannelGroupDetailList(remoteHeader, remoteData);

                // 결과코드검사
                if(!remoteData.ResultCD.Equals("0000"))
                {
                    throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
                }

                // 결과 셋트
                channelGroupModel.ChannelGroupDataSet = remoteData.ChannelGroupDataSet.Copy();
                channelGroupModel.ResultCnt   = remoteData.ResultCnt;
                channelGroupModel.ResultCD    = remoteData.ResultCD;

                _log.Debug("-----------------------------------------");
                _log.Debug("장르그룹 디테일 목록조회 End");
                _log.Debug("-----------------------------------------");
            }
            catch(FrameException fe)
            {
                _log.Warning("-----------------------------------------");
                _log.Warning( this.ToString() + ":GetChannelGroupList():" + fe.ErrCode + ":" + fe.ResultMsg);
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
   
        public void SetChannelGroupAdd(ChannelGroupModel channelGroupModel)
        {
            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug("장르그룹추가 Start");
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
            
                // 호출정보셋트
                remoteData.MediaCode            = channelGroupModel.MediaCode;
                remoteData.AdGroupName          = channelGroupModel.AdGroupName;
                remoteData.Comment              = channelGroupModel.Comment;
                remoteData.UseYn                = channelGroupModel.UseYn;
                remoteData.ChannelGroupDataSet    = channelGroupModel.ChannelGroupDataSet.Copy();

                // 웹서비스 호출 타임아웃설정
                svc.Timeout = FrameSystem.m_SystemTimeout;
                // 웹서비스 메소드 호출
                remoteData = svc.SetChannelGroupCreate(remoteHeader, remoteData);
            
                // 결과코드검사
                if(!remoteData.ResultCD.Equals("0000"))
                {
                    throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
                }
            
                // 결과 셋트
                channelGroupModel.ResultCnt   = remoteData.ResultCnt;
                _log.Debug("channelGroupModel.ResultCnt = "+channelGroupModel.ResultCnt);
            			
                channelGroupModel.ResultCD    = remoteData.ResultCD;
            
                _log.Debug("-----------------------------------------");
                _log.Debug("장르그룹추가 End");
                _log.Debug("-----------------------------------------");
            }
            catch(FrameException fe)
            {
                channelGroupModel.ResultCD    = "3101";
                _log.Warning("-----------------------------------------");
                _log.Warning( this.ToString() + ":SetChannelGroupCreate():" + fe.ErrCode + ":" + fe.ResultMsg);
                _log.Warning("-----------------------------------------");
                throw fe;
            }
            catch(Exception e)
            {
                channelGroupModel.ResultCD    = "3101";
                _log.Error("-----------------------------------------");
                _log.Exception(e);
                _log.Error("-----------------------------------------");
                throw new FrameException(e.Message);
            }
        }

        /// <summary>
        /// 장르그룹정보 수정
        /// </summary>
        /// <param name="channelGroupModel"></param>
        public void SetChannelGroupUpdate(ChannelGroupModel channelGroupModel)
        {
            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug("장르그룹정보수정 Start");
                _log.Debug("-----------------------------------------");
            
            
                //				입력데이터의 Validation 검사
                if(channelGroupModel.AdGroupName.Trim().Length < 1) 
                {
                    throw new FrameException("광고 상품명이 존재하지 않습니다.");
                }
                if(channelGroupModel.AdGroupName.Trim().Length > 50) 
                {
                    throw new FrameException("광고 상품명은 50Byte를 초과할 수 없습니다.");
                }
                if(channelGroupModel.Comment.Trim().Length > 100) 
                {
                    throw new FrameException("그룹설명은 100Byte를 초과할 수 없습니다.");
                }     
            	
            
                // 웹서비스 인스턴스 생성
                ChannelGroupServicePloxy.ChannelGroupService svc = new ChannelGroupServicePloxy.ChannelGroupService();
            	svc.Url = _WebServiceUrl;		
                // 리모트 모델 생성
                ChannelGroupServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.ChannelGroupServicePloxy.HeaderModel();
                ChannelGroupServicePloxy.ChannelGroupModel remoteData   = new AdManagerClient.ChannelGroupServicePloxy.ChannelGroupModel();
            
                // 헤더정보 셋트
                remoteHeader.ClientKey     = Header.ClientKey;
                remoteHeader.UserID        = Header.UserID;
            
                // 호출정보셋트
                remoteData.MediaCode            = channelGroupModel.MediaCode;
                remoteData.AdGroupCode          = channelGroupModel.AdGroupCode;
                remoteData.AdGroupName          = channelGroupModel.AdGroupName;
                remoteData.Comment              = channelGroupModel.Comment;
                remoteData.UseYn                = channelGroupModel.UseYn;
                remoteData.ChannelGroupDataSet    = channelGroupModel.ChannelGroupDataSet.Copy();
            				
                // 웹서비스 호출 타임아웃설정
                svc.Timeout = FrameSystem.m_SystemTimeout;
                // 웹서비스 메소드 호출
                remoteData = svc.SetChannelGroupUpdate(remoteHeader, remoteData);
            
                // 결과코드검사
                if(!remoteData.ResultCD.Equals("0000"))
                {
                    throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
                }
            
                // 결과 셋트
                channelGroupModel.ResultCnt   = remoteData.ResultCnt;
                channelGroupModel.ResultCD    = remoteData.ResultCD;
            
                _log.Debug("-----------------------------------------");
                _log.Debug("장르그룹정보수정 End");
                _log.Debug("-----------------------------------------");
            }
            catch(FrameException fe)
            {
                _log.Warning("-----------------------------------------");
                _log.Warning( this.ToString() + ":SetChannelGroupUpdate():" + fe.ErrCode + ":" + fe.ResultMsg);
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
        /// 장르그룹추가
        /// </summary>
        /// <param name="channelGroupModel"></param>
        /// <returns></returns>	
        /// <summary>
        /// 장르그룹 삭제
        /// </summary>
        /// <param name="baseModel"></param>
        public void SetChannelGroupDelete(ChannelGroupModel channelGroupModel)
        {
          
        }

    }
}