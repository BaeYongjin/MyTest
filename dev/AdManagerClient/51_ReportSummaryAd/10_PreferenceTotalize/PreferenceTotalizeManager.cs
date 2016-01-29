/*
 * -------------------------------------------------------
 * Class Name: PreferenceTotalizeManager
 * 주요기능  : 선호도 조사 팝업 집계
 * -------------------------------------------------------
 * [수정사항]
 * -------------------------------------------------------
 * 수정코드  : [E_01]
 * 수정자    : 김보배
 * 수정일    : 2013.09.05
 * 수정내용  :        
 *            - 광고 노출수 / 팝업 노출수 계산 되도록 변경
 *              
 * 수정함수  :
 *            - AdExpCount(), PopExpCount() 추가
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
    public class PreferenceTotalizeManager : BaseManager
    {
        public PreferenceTotalizeManager(SystemModel systemModel, CommonModel commonModel)
            : base(systemModel, commonModel)
        {
            _log = FrameSystem.oLog;
            _module = "PreferenceTotalize";
            _Host = FrameSystem.m_WebServer_Host;
            _Port = FrameSystem.m_WebServer_Port;
            _Path = FrameSystem.m_WebServer_App + "/ReportSummaryAd/PreferenceTotalizeService.asmx";
        }

        /// <summary>
        /// 광고목록 리스트
        /// </summary>
        /// <param name="model"></param>
        public void GetAdList(PreferenceTotalizeModel model)
        {
            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug("GetAdList() Start");
                _log.Debug("-----------------------------------------");

                // 웹서비스 인스턴스 생성
                PreferenceTotalizeServicePloxy.PreferenceTotalizeService svc = new PreferenceTotalizeServicePloxy.PreferenceTotalizeService();

                // 웹서비스URL 동적 셋트
                svc.Url = _WebServiceUrl;

                // 리모트 모델 생성
                PreferenceTotalizeServicePloxy.HeaderModel remoteHeader = new PreferenceTotalizeServicePloxy.HeaderModel();
                PreferenceTotalizeServicePloxy.PreferenceTotalizeModel remoteData = new PreferenceTotalizeServicePloxy.PreferenceTotalizeModel();

                // 헤더정보 셋트
                remoteHeader.ClientKey = Header.ClientKey;
                remoteHeader.UserID = Header.UserID;
                remoteHeader.UserLevel = Header.UserLevel;

                // 호출정보 셋트
                remoteData.KeySearch = model.KeySearch;

                // 웹서비스 호출 타임아웃설정
                svc.Timeout = FrameSystem.m_SystemTimeout * 10;

                // 웹서비스 메소드 호출
                remoteData = svc.GetAdList(remoteHeader, remoteData);

                // 결과코드검사
                if (!remoteData.ResultCD.Equals("0000"))
                {
                    throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
                }

                // 결과 셋트
                model.PreferenceDataSet = remoteData.PreferenceDataSet.Copy(); ;
                model.ResultCnt = remoteData.ResultCnt;
                model.ResultCD = remoteData.ResultCD;

                _log.Debug("-----------------------------------------");
                _log.Debug("GetAdList() End");
                _log.Debug("-----------------------------------------");
            }
            catch (FrameException fe)
            {
                _log.Warning("-----------------------------------------");
                _log.Warning(this.ToString() + ":GetAdList():" + fe.ErrCode + ":" + fe.ResultMsg);
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
        /// 선호도 상세정보 가져오기
        /// </summary>
        /// <param name="model"></param>
        public void getPopupDetail(PreferenceTotalizeModel model)
        {
            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug("getPopupDetail() Start");
                _log.Debug("-----------------------------------------");
                                
                PreferenceTotalizeServicePloxy.PreferenceTotalizeService svc = new PreferenceTotalizeServicePloxy.PreferenceTotalizeService();

                PreferenceTotalizeServicePloxy.HeaderModel remoteHeader = new PreferenceTotalizeServicePloxy.HeaderModel();
                PreferenceTotalizeServicePloxy.PreferenceTotalizeModel remoteData = new PreferenceTotalizeServicePloxy.PreferenceTotalizeModel();

                // 헤더정보 셋트
                remoteHeader.ClientKey  = Header.ClientKey;
                remoteHeader.UserID     = Header.UserID;
                remoteHeader.UserLevel  = Header.UserLevel;

                // 호출조건 셋트
                remoteData.KeySearch    = model.KeySearch;
                remoteData.KeyItemNo    = model.KeyItemNo;
                remoteData.KeyNoticeId  = model.KeyNoticeId;
                remoteData.KeyExmNo     = model.KeyExmNo;
                remoteData.KeyStartDay  = model.KeyStartDay;
                remoteData.KeyEndDay    = model.KeyEndDay;
                                				
                // 웹서비스 호출 타임아웃설정
                svc.Url = _WebServiceUrl;
                svc.Timeout = FrameSystem.m_SystemTimeout * 5;

                // 웹서비스 메소드 호출
                remoteData = svc.getPopupList(remoteHeader, remoteData);

                // 결과코드검사
                if (!remoteData.ResultCD.Equals("0000"))
                {
                    throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
                }

                // 결과 셋트
                model.EventName     = remoteData.EventName;
                model.AdExpCount    = remoteData.AdExpCount;
                model.PopExpCount   = remoteData.PopExpCount;
                model.RepCount      = remoteData.RepCount;
                model.RepRate       = remoteData.RepRate;
                model.PopExpType    = remoteData.PopExpType;
                model.PreferenceDataSet = remoteData.PreferenceDataSet.Copy();
                model.ResultCnt     = remoteData.ResultCnt;
                model.ResultCD      = remoteData.ResultCD;

                _log.Debug("-----------------------------------------");
                _log.Debug("getPopupDetail() End");
                _log.Debug("-----------------------------------------");
            }
            catch (FrameException fe)
            {
                _log.Warning("-----------------------------------------");
                _log.Warning(this.ToString() + ":getPopupDetail():" + fe.ErrCode + ":" + fe.ResultMsg);
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