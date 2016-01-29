// ===============================================================================
// UserUpdate Manager  for Charites Project
//
// UserUpdateManager.cs
//
// 광고계약정보 저장 서비스를 호출합니다. 
//
// ===============================================================================
// Release history
// 2015.11.10 김진호 v1.0
// ===============================================================================
// Copyright (C) 2015 Dartmedia Inc.
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
    /// 광고계약정보 웹서비스를 호출합니다. 
    /// </summary>
    public class BizManageTgtManager : BaseManager
    {
        /// <summary>
        /// 생성자
        /// </summary>
        /// <param name="systemModel"></param>
        /// <param name="commonModel"></param>

        public BizManageTgtManager(SystemModel systemModel, CommonModel commonModel)
            : base(systemModel, commonModel)
        {
            _log = FrameSystem.oLog;
            _module = "BizManageTgt";
            _Host = FrameSystem.m_WebServer_Host;
            _Port = FrameSystem.m_WebServer_Port;
            _Path = FrameSystem.m_WebServer_App + "/ReportSummaryAd/BizManageService.asmx";
        }

        /// <summary>
        /// 영업관리대상 광고판매 목록 정보조회
        /// </summary>
        /// <param name="campaignModel"></param>
        public void GetBizManageTargetList(BizManageModel model)
        {
            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug("영업관리대상 광고판매 목록조회 Start");
                _log.Debug("-----------------------------------------");

                // 웹서비스 인스턴스 생성
                BizManageServiceProxy.BizManageService svc = new BizManageServiceProxy.BizManageService();

                svc.Url = _WebServiceUrl;

                // 리모트 모델 생성
                BizManageServiceProxy.HeaderModel remoteHeader = new BizManageServiceProxy.HeaderModel();
                BizManageServiceProxy.BizManageModel remoteData = new BizManageServiceProxy.BizManageModel();

                // 헤더정보 셋트
                remoteHeader.ClientKey = Header.ClientKey;
                remoteHeader.UserID = Header.UserID;

                // 호출정보 셋트
                remoteData.SearchRapCode        = model.SearchRapCode;
                remoteData.SearchAgencyCode     = model.SearchAgencyCode;
                remoteData.SearchAdvertiserCode = model.SearchAdvertiserCode;
                remoteData.SearchAdType         = model.SearchAdType;
                remoteData.SearchStartDay = model.SearchStartDay;
                remoteData.SearchEndDay   = model.SearchEndDay;

                // 웹서비스 호출 타임아웃설정
                svc.Timeout = FrameSystem.m_SystemTimeout * 30;

                // 웹서비스 메소드 호출
                remoteData = svc.GetBizManageTargetList(remoteHeader, remoteData);

                // 결과코드검사
                if (!remoteData.ResultCD.Equals("0000"))
                {
                    throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
                }

                // 결과 셋트
                model.BizManageDataSet = remoteData.BizManageDataSet.Copy();
                model.ResultCnt = remoteData.ResultCnt;
                model.ResultCD = remoteData.ResultCD;

                _log.Debug("-----------------------------------------");
                _log.Debug("영업관리대상 광고판매 목록조회 End");
                _log.Debug("-----------------------------------------");
            }
            catch (FrameException fe)
            {
                _log.Warning("-----------------------------------------");
                _log.Warning(this.ToString() + ":GetBizManageTargetList():" + fe.ErrCode + ":" + fe.ResultMsg);
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
