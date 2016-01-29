// ===============================================================================
// UserUpdate Manager  for Charites Project
//
// UserUpdateManager.cs
//
// 채널정보 저장 서비스를 호출합니다. 
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
    /// 채널정보 웹서비스를 호출합니다. 
    /// </summary>
    public class ChannelManager : BaseManager
    {
        /// <summary>
        /// 생성자
        /// </summary>
        /// <param name="systemModel"></param>
        /// <param name="commonModel"></param>

        public ChannelManager(SystemModel systemModel, CommonModel commonModel) : base(systemModel, commonModel)
        {
            _log = FrameSystem.oLog;
            _module = "CONTENT";
			_Host  = FrameSystem.m_WebServer_Host;
			_Port  = FrameSystem.m_WebServer_Port;
			_Path  = FrameSystem.m_WebServer_App + "/Media/ChannelService.asmx";
        }

        /// <summary>
        /// 미디어콤보정보조회
        /// </summary>
        /// <param name="channelModel"></param>
        public void GetMediaList(ChannelModel channelModel)
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
                remoteData.SearchKey       = channelModel.SearchKey;
                remoteData.MediaCode = channelModel.MediaCode;
				
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
                channelModel.MediaDataSet = remoteData.UserDataSet.Copy();
                channelModel.ResultCnt   = remoteData.ResultCnt;
                channelModel.ResultCD    = remoteData.ResultCD;

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
        /// 채널장르정보조회
        /// </summary>
        /// <param name="channelModel"></param>
        public void GetChannelList(ChannelModel channelModel)
        {

            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug("채널목록조회 Start");
                _log.Debug("-----------------------------------------");
				
                // 웹서비스 인스턴스 생성
                ChannelServicePloxy.ChannelService svc = new ChannelServicePloxy.ChannelService();
				svc.Url = _WebServiceUrl;
			
                // 리모트 모델 생성
                ChannelServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.ChannelServicePloxy.HeaderModel();
                ChannelServicePloxy.ChannelModel remoteData   = new AdManagerClient.ChannelServicePloxy.ChannelModel();

                // 헤더정보 셋트
                remoteHeader.ClientKey     = Header.ClientKey;
                remoteHeader.UserID        = Header.UserID;

                // 호출정보 셋트
                remoteData.SearchKey       = channelModel.SearchKey;
                remoteData.MediaCode       = channelModel.MediaCode;
				
                // 웹서비스 호출 타임아웃설정
                svc.Timeout = FrameSystem.m_SystemTimeout;

                // 웹서비스 메소드 호출
                remoteData = svc.GetChannelList(remoteHeader, remoteData);

                // 결과코드검사
                if(!remoteData.ResultCD.Equals("0000"))
                {
                    throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
                }

                // 결과 셋트
                channelModel.ChannelDataSet = remoteData.ChannelDataSet.Copy();
                channelModel.ResultCnt   = remoteData.ResultCnt;
                channelModel.ResultCD    = remoteData.ResultCD;

                _log.Debug("-----------------------------------------");
                _log.Debug("채널목록조회 End");
                _log.Debug("-----------------------------------------");
            }
            catch(FrameException fe)
            {
                _log.Warning("-----------------------------------------");
                _log.Warning( this.ToString() + ":GetChannelList():" + fe.ErrCode + ":" + fe.ResultMsg);
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
        /// 채널 디테일 목록조회
        /// </summary>
        /// <param name="channelModel"></param>
        public void GetChannelDetailList(ChannelModel channelModel)
        {

            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug("채널 디테일 조회 Start");
                _log.Debug("-----------------------------------------");
				
                // 웹서비스 인스턴스 생성
                ChannelServicePloxy.ChannelService svc = new ChannelServicePloxy.ChannelService();
                svc.Url = _WebServiceUrl;
			
                // 리모트 모델 생성
                ChannelServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.ChannelServicePloxy.HeaderModel();
                ChannelServicePloxy.ChannelModel remoteData   = new AdManagerClient.ChannelServicePloxy.ChannelModel();

                // 헤더정보 셋트
                remoteHeader.ClientKey     = Header.ClientKey;
                remoteHeader.UserID        = Header.UserID;

                // 호출정보 셋트
                remoteData.SearchKey       = channelModel.SearchKey;
                remoteData.GenreCode       = channelModel.GenreCode;
                remoteData.CheckYn         = channelModel.CheckYn;
				
                // 웹서비스 호출 타임아웃설정
                svc.Timeout = FrameSystem.m_SystemTimeout;

                // 웹서비스 메소드 호출
                remoteData = svc.GetChannelDetailList(remoteHeader, remoteData);

                // 결과코드검사
                if(!remoteData.ResultCD.Equals("0000"))
                {
                    throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
                }

                // 결과 셋트
                channelModel.ChannelDataSet = remoteData.ChannelDataSet.Copy();
                channelModel.ResultCnt   = remoteData.ResultCnt;
                channelModel.ResultCD    = remoteData.ResultCD;

                _log.Debug("-----------------------------------------");
                _log.Debug("채널 디테일 목록조회 End");
                _log.Debug("-----------------------------------------");
            }
            catch(FrameException fe)
            {
                _log.Warning("-----------------------------------------");
                _log.Warning( this.ToString() + ":GetChannelList():" + fe.ErrCode + ":" + fe.ResultMsg);
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
        /// 채널 디테일 목록조회 - 사용안함
        /// </summary>
        /// <param name="channelModel"></param>
        public void GetChannelSetDetailList(ChannelModel channelModel)
        {

            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug("채널구성 디테일 조회 Start");
                _log.Debug("-----------------------------------------");
				
                // 웹서비스 인스턴스 생성
                ChannelServicePloxy.ChannelService svc = new ChannelServicePloxy.ChannelService();
                svc.Url = _WebServiceUrl;
			
                // 리모트 모델 생성
                ChannelServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.ChannelServicePloxy.HeaderModel();
                ChannelServicePloxy.ChannelModel remoteData   = new AdManagerClient.ChannelServicePloxy.ChannelModel();

                // 헤더정보 셋트
                remoteHeader.ClientKey     = Header.ClientKey;
                remoteHeader.UserID        = Header.UserID;

                // 호출정보 셋트
                remoteData.SearchKey       = channelModel.SearchKey;
                remoteData.MediaCode       = channelModel.MediaCode;
                remoteData.ChannelNo       = channelModel.ChannelNo;
				
                // 웹서비스 호출 타임아웃설정
                svc.Timeout = FrameSystem.m_SystemTimeout;
				
                // 웹서비스 메소드 호출
                remoteData = svc.GetChannelSetDetailList(remoteHeader, remoteData);

                // 결과코드검사
                if(!remoteData.ResultCD.Equals("0000"))
                {
                    throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
                }

                // 결과 셋트
                channelModel.ChannelDataSet = remoteData.ChannelDataSet.Copy();
                channelModel.ResultCnt   = remoteData.ResultCnt;
                channelModel.ResultCD    = remoteData.ResultCD;

                _log.Debug("-----------------------------------------");
                _log.Debug("채널구성 디테일 목록조회 End");
                _log.Debug("-----------------------------------------");
            }
            catch(FrameException fe)
            {
                _log.Warning("-----------------------------------------");
                _log.Warning( this.ToString() + ":GetChannelSetDetailList():" + fe.ErrCode + ":" + fe.ResultMsg);
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
        /// 채널추가
        /// </summary>
        /// <param name="channelModel"></param>
        /// <returns></returns>
        public void SetChannelAdd(ChannelModel channelModel)
        {
            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug("채널추가 Start");
                _log.Debug("-----------------------------------------");
            
                if(channelModel.TotalSeries.Length > 0 &&  Convert.ToInt32(channelModel.TotalSeries) > 255) 
                {
                    throw new FrameException("시리즈편수는 255를 초과할 수 없습니다.");
                }

                // 웹서비스 인스턴스 생성
                ChannelServicePloxy.ChannelService svc = new ChannelServicePloxy.ChannelService();
				svc.Url = _WebServiceUrl;
            				
                // 리모트 모델 생성
                ChannelServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.ChannelServicePloxy.HeaderModel();
                ChannelServicePloxy.ChannelModel remoteData   = new AdManagerClient.ChannelServicePloxy.ChannelModel();
            
                // 헤더정보 셋트
                remoteHeader.ClientKey     = Header.ClientKey;
                remoteHeader.UserID        = Header.UserID;
            
                // 호출정보셋트
                remoteData.ServiceID = channelModel.ServiceID;
                remoteData.ChannelNumber     = channelModel.ChannelNumber;
                remoteData.UseYn     = channelModel.UseYn;
                remoteData.AdYn = channelModel.AdYn;
                remoteData.AdRate = channelModel.AdRate;
                remoteData.AdnRate = channelModel.AdnRate;

                // 웹서비스 호출 타임아웃설정
                svc.Timeout = FrameSystem.m_SystemTimeout;

                // 웹서비스 메소드 호출
                remoteData = svc.SetChannelUpdate(remoteHeader, remoteData);
            
                // 결과코드검사
                if(!remoteData.ResultCD.Equals("0000"))
                {
                    throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
                }
            
                // 결과 셋트
                channelModel.ResultCnt   = remoteData.ResultCnt;
                _log.Debug("channelModel.ResultCnt = "+channelModel.ResultCnt);
            			
                channelModel.ResultCD    = remoteData.ResultCD;
            
                _log.Debug("-----------------------------------------");
                _log.Debug("채널추가 End");
                _log.Debug("-----------------------------------------");
            }
            catch(FrameException fe)
            {
                channelModel.ResultCD    = "3101";
                _log.Warning("-----------------------------------------");
                _log.Warning( this.ToString() + ":SetChannelCreate():" + fe.ErrCode + ":" + fe.ResultMsg);
                _log.Warning("-----------------------------------------");
                throw fe;
            }
            catch(Exception e)
            {
                channelModel.ResultCD    = "3101";
                _log.Error("-----------------------------------------");
                _log.Exception(e);
                _log.Error("-----------------------------------------");
                throw new FrameException(e.Message);
            }
        }

		
        /// <summary>
        /// 채널 삭제
        /// </summary>
        /// <param name="baseModel"></param>
        public void SetChannelDelete(ChannelModel channelModel)
        {
            			try
            			{
            				_log.Debug("-----------------------------------------");
            				_log.Debug("채널삭제 start");
            				_log.Debug("-----------------------------------------");
            
                            // 웹서비스 인스턴스 생성
                            ChannelServicePloxy.ChannelService svc = new ChannelServicePloxy.ChannelService();
							svc.Url = _WebServiceUrl;
            				
                            // 리모트 모델 생성
                            ChannelServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.ChannelServicePloxy.HeaderModel();
                            ChannelServicePloxy.ChannelModel remoteData   = new AdManagerClient.ChannelServicePloxy.ChannelModel();
            
                            // 헤더정보 셋트
                            remoteHeader.ClientKey     = Header.ClientKey;
                            remoteHeader.UserID        = Header.UserID;
            
                            // 호출정보셋트
                            remoteData.MediaCode     = channelModel.MediaCode;
                            remoteData.ChannelNo     = channelModel.ChannelNo;
                            
                            // 웹서비스 호출 타임아웃설정
                            svc.Timeout = FrameSystem.m_SystemTimeout;
	
            				// 웹서비스 메소드 호출
            				remoteData = svc.SetChannelDelete(remoteHeader, remoteData);
            
            				// 결과코드검사
            				if(!remoteData.ResultCD.Equals("0000"))
            				{
            					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
            				}
            
            				// 결과 셋트
            				channelModel.ResultCnt   = remoteData.ResultCnt;
            				channelModel.ResultCD    = remoteData.ResultCD;
            
            				_log.Debug("-----------------------------------------");
            				_log.Debug("채널삭제 end");
            				_log.Debug("-----------------------------------------");
            			}
            			catch(FrameException fe)
            			{
            				_log.Warning("-----------------------------------------");
            				_log.Warning( this.ToString() + ":setchanneldelete():" + fe.ErrCode + ":" + fe.ResultMsg);
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