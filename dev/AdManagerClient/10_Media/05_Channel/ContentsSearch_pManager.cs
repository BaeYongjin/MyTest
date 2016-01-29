// ===============================================================================
// UserUpdate Manager  for Charites Project
//
// UserUpdateManager.cs
//
// 컨텐츠정보 저장 서비스를 호출합니다. 
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
    /// 컨텐츠정보 웹서비스를 호출합니다. 
    /// </summary>
    public class ContentsSearch_pManager : BaseManager
    {
        /// <summary>
        /// 생성자
        /// </summary>
        /// <param name="systemModel"></param>
        /// <param name="commonModel"></param>

        public ContentsSearch_pManager(SystemModel systemModel, CommonModel commonModel) : base(systemModel, commonModel)
        {
            _log = FrameSystem.oLog;
            _module = "CONTENT";
			_Host  = FrameSystem.m_WebServer_Host;
			_Port  = FrameSystem.m_WebServer_Port;
			_Path  = FrameSystem.m_WebServer_App + "/Media/ContentsService.asmx";
        }

        /// <summary>
        /// 컨텐츠정보조회
        /// </summary>
        /// <param name="contentsModel"></param>
        public void GetContentsListPopUp(ContentsModel contentsModel)
        {

            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug("컨텐츠목록조회 Start");
                _log.Debug("-----------------------------------------");
				
                // 웹서비스 인스턴스 생성
                ContentsServicePloxy.ContentsService svc = new ContentsServicePloxy.ContentsService();
				svc.Url = _WebServiceUrl;
			
                // 리모트 모델 생성
                ContentsServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.ContentsServicePloxy.HeaderModel();
                ContentsServicePloxy.ContentsModel remoteData   = new AdManagerClient.ContentsServicePloxy.ContentsModel();

                // 헤더정보 셋트
                remoteHeader.ClientKey     = Header.ClientKey;
                remoteHeader.UserID        = Header.UserID;

                // 호출정보 셋트
                remoteData.SearchKey       = contentsModel.SearchKey;
               
                // 웹서비스 호출 타임아웃설정
                svc.Timeout = FrameSystem.m_SystemTimeout;
				
                // 웹서비스 메소드 호출
                remoteData = svc.GetContentsListPopUp(remoteHeader, remoteData);

                // 결과코드검사
                if(!remoteData.ResultCD.Equals("0000"))
                {
                    throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
                }

                // 결과 셋트
                contentsModel.ContentsDataSet = remoteData.ContentsDataSet.Copy();
                contentsModel.ResultCnt   = remoteData.ResultCnt;
                contentsModel.ResultCD    = remoteData.ResultCD;

                _log.Debug("-----------------------------------------");
                _log.Debug("컨텐츠목록조회 End");
                _log.Debug("-----------------------------------------");
            }
            catch(FrameException fe)
            {
                _log.Warning("-----------------------------------------");
                _log.Warning( this.ToString() + ":GetContentsListPopUp():" + fe.ErrCode + ":" + fe.ResultMsg);
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