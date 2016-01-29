using System;
using System.Data;

using WinFramework.Base;
using WinFramework.Data;
using WinFramework.Misc;

using AdManagerModel;

namespace AdManagerClient
{
    public class SchExclusiveZoneAdManager : BaseManager
    {
        #region 생성자
        public SchExclusiveZoneAdManager(SystemModel systemModel, CommonModel commonModel) : base(systemModel, commonModel)
        {
            _log = FrameSystem.oLog;
            _module = "SchExclusiveZoneAd";
            _Host = FrameSystem.m_WebServer_Host;
            _Port = FrameSystem.m_WebServer_Port;
            _Path = FrameSystem.m_WebServer_App + "/Schedule/SchExclusiveZoneService.asmx";
        }
        #endregion

        #region 시간대 독점 편성 리스트 
        /// <summary>
        /// 시간대 독점 편성 리스트
        /// </summary>
        /// <param name="model"></param>
        public void GetSchExclusiveList(SchExclusiveZoneModel model)
        {
            try
            {
                _log.Debug("-----------------------------------------");
				_log.Debug("시간대 독점 편성 조회 Start");
				_log.Debug("-----------------------------------------");

                // 웹서비스 인스턴스 생성
                SchExclusiveZoneServicePloxy.SchExclusiveZoneService svc = new SchExclusiveZoneServicePloxy.SchExclusiveZoneService();
                svc.Url = _WebServiceUrl;

                // 리모트 모델 생성
                SchExclusiveZoneServicePloxy.HeaderModel remoteHeader = new AdManagerClient.SchExclusiveZoneServicePloxy.HeaderModel();
                SchExclusiveZoneServicePloxy.SchExclusiveZoneModel remoteData = new AdManagerClient.SchExclusiveZoneServicePloxy.SchExclusiveZoneModel();

                // 헤더정보 셋트
                remoteHeader.UserID = Header.UserID;

                // 호출정보 셋트 없음
                

                // 웹서비스 호출 타임아웃설정
                svc.Timeout = FrameSystem.m_SystemTimeout;
                // 웹서비스 메소드 호출
                remoteData = svc.GetSchExclusiveList(remoteHeader, remoteData);

                // 결과코드검사
                if (!remoteData.ResultCD.Equals("0000"))
                {
                    throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
                }

                // 결과 셋트
                model.SchExclusiveDataSet = remoteData.SchExclusiveDataSet.Copy();
                model.ResultCnt = remoteData.ResultCnt;
                model.ResultCD = remoteData.ResultCD;

                _log.Debug("-----------------------------------------");
                _log.Debug("시간대 독점 편성 조회 End");
                _log.Debug("-----------------------------------------");
                
            }
            catch (FrameException fe)
            {
                _log.Warning("-----------------------------------------");
                _log.Warning(this.ToString() + ":GetSchExclusiveList():" + fe.ErrCode + ":" + fe.ResultMsg);
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
        #endregion

        #region 타겟 목록 
        public void GetTargetingList(SchExclusiveZoneModel schExclusiveZoneModel)
        {
            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug("타겟팅 목록조회 Start");
                _log.Debug("-----------------------------------------");

                // 웹서비스 인스턴스 생성
                SchExclusiveZoneServicePloxy.SchExclusiveZoneService svc = new SchExclusiveZoneServicePloxy.SchExclusiveZoneService();

                // 웹서비스 URL동적 생성
                svc.Url = _WebServiceUrl;

                // 리모트 모델 생성
                SchExclusiveZoneServicePloxy.HeaderModel remoteHeader = new AdManagerClient.SchExclusiveZoneServicePloxy.HeaderModel();
                SchExclusiveZoneServicePloxy.SchExclusiveZoneModel remoteData = new AdManagerClient.SchExclusiveZoneServicePloxy.SchExclusiveZoneModel();

                // 헤더정보 셋트
                remoteHeader.ClientKey = Header.ClientKey;
                remoteHeader.UserID = Header.UserID;
                remoteHeader.UserLevel = Header.UserLevel;
                remoteHeader.UserClass = Header.UserClass;

                // 호출정보 셋트
                remoteData.SearchKey = schExclusiveZoneModel.SearchKey;
                remoteData.SearchMediaCode = schExclusiveZoneModel.SearchMediaCode;
                remoteData.SearchRapCode = schExclusiveZoneModel.SearchRapCode;
                remoteData.SearchAgencyCode = schExclusiveZoneModel.SearchAgencyCode;
                remoteData.SearchchkAdState_20 = schExclusiveZoneModel.SearchchkAdState_20;
                remoteData.SearchchkAdState_30 = schExclusiveZoneModel.SearchchkAdState_30;
                remoteData.SearchchkAdState_40 = schExclusiveZoneModel.SearchchkAdState_40;

                // 웹서비스 호출 타임아웃설정
                svc.Timeout = FrameSystem.m_SystemTimeout;
                // 웹서비스 메소드 호출
                remoteData = svc.GetTagetingList(remoteHeader, remoteData);

                // 결과코드검사
                if (!remoteData.ResultCD.Equals("0000"))
                {
                    throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
                }

                // 결과 셋트
                schExclusiveZoneModel.TargetingDataSet = remoteData.TargetingDataSet.Copy();
                schExclusiveZoneModel.ResultCnt = remoteData.ResultCnt;
                schExclusiveZoneModel.ResultCD = remoteData.ResultCD;

                _log.Debug("-----------------------------------------");
                _log.Debug("타겟팅 목록조회 End");
                _log.Debug("-----------------------------------------");
            }
            catch (FrameException fe)
            {
                _log.Warning("-----------------------------------------");
                _log.Warning(this.ToString() + ":GetTargetingList():" + fe.ErrCode + ":" + fe.ResultMsg);
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
        #endregion


        #region 시간대 독점 추가 적용
        /// <summary>
        /// 시간대 독점 추가 적용
        /// </summary>
        /// <param name="targetingModel"></param>
        public void SetTargetingDetailUpdate(SchExclusiveZoneModel schExclusiveZoneModel)
        {

            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug("시간대 독점 편성 추가  Start");
                _log.Debug("-----------------------------------------");

                // 웹서비스 인스턴스 생성
                SchExclusiveZoneServicePloxy.SchExclusiveZoneService svc = new SchExclusiveZoneServicePloxy.SchExclusiveZoneService();

                // 웹서비스 URL동적 생성
                svc.Url = _WebServiceUrl;

                // 리모트 모델 생성
                SchExclusiveZoneServicePloxy.HeaderModel remoteHeader = new AdManagerClient.SchExclusiveZoneServicePloxy.HeaderModel();
                SchExclusiveZoneServicePloxy.SchExclusiveZoneModel remoteData = new AdManagerClient.SchExclusiveZoneServicePloxy.SchExclusiveZoneModel();

                // 헤더정보 셋트
                remoteHeader.ClientKey = Header.ClientKey;
                remoteHeader.UserID = Header.UserID;
                remoteHeader.UserLevel = Header.UserLevel;
                remoteHeader.UserClass = Header.UserClass;

                // 호출정보 셋트
                remoteData.ItemNo = schExclusiveZoneModel.ItemNo;
                remoteData.ItemName = schExclusiveZoneModel.ItemName;
                remoteData.ContractAmt = schExclusiveZoneModel.ContractAmt;
                remoteData.TgtTime = schExclusiveZoneModel.TgtTime;
                

                // 웹서비스 호출 타임아웃설정
                svc.Timeout = FrameSystem.m_SystemTimeout;

                // 웹서비스 메소드 호출
                remoteData = svc.SetTargetingDetailUpdate(remoteHeader, remoteData);

                // 결과코드검사
                if (!remoteData.ResultCD.Equals("0000"))
                {
                    throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
                }

                // 결과 셋트
                schExclusiveZoneModel.ResultCnt = remoteData.ResultCnt;
                schExclusiveZoneModel.ResultCD = remoteData.ResultCD;

                _log.Debug("-----------------------------------------");
                _log.Debug("시간대 독점 편성 추가 End");
                _log.Debug("-----------------------------------------");
            }
            catch (FrameException fe)
            {
                _log.Warning("-----------------------------------------");
                _log.Warning(this.ToString() + ":SetSchChoiceMenuDetailAdd():" + fe.ErrCode + ":" + fe.ResultMsg);
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
        #endregion


        #region 선택한 시간대 독점 수정
        /// <summary>
        /// 선택한 시간대 독점 수정 
        /// </summary>
        /// <param name="schExclusiveZoneModel"></param>
        public void SetSchExclusivUpdate(SchExclusiveZoneModel schExclusiveZoneModel)
        {
            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug("선택한 시간대 독점 편성 수정 Start");
                _log.Debug("-----------------------------------------");

                // 웹서비스 인스턴스 생성
                SchExclusiveZoneServicePloxy.SchExclusiveZoneService svc = new SchExclusiveZoneServicePloxy.SchExclusiveZoneService();

                // 웹서비스 URL동적 생성
                svc.Url = _WebServiceUrl;

                // 리모트 모델 생성
                SchExclusiveZoneServicePloxy.HeaderModel remoteHeader = new AdManagerClient.SchExclusiveZoneServicePloxy.HeaderModel();
                SchExclusiveZoneServicePloxy.SchExclusiveZoneModel remoteData = new AdManagerClient.SchExclusiveZoneServicePloxy.SchExclusiveZoneModel();

                // 헤더정보 셋트
                remoteHeader.ClientKey  = Header.ClientKey;
                remoteHeader.UserID     = Header.UserID;
                remoteHeader.UserLevel  = Header.UserLevel;
                remoteHeader.UserClass  = Header.UserClass;

                // 호출정보 셋트
                remoteData.ItemNo       = schExclusiveZoneModel.ItemNo;
                remoteData.TgtTimeYn    = schExclusiveZoneModel.TgtTimeYn;
                remoteData.TgtTime      = schExclusiveZoneModel.TgtTime;
                remoteData.TgtWeekYn    = schExclusiveZoneModel.TgtWeekYn;
                remoteData.TgtWeek      = schExclusiveZoneModel.TgtWeek;

                // 웹서비스 호출 타임아웃설정
                svc.Timeout = FrameSystem.m_SystemTimeout;

                // 웹서비스 메소드 호출
                remoteData = svc.SetSchExclusivUpdate(remoteHeader, remoteData);

                // 결과코드검사
                if (!remoteData.ResultCD.Equals("0000"))
                {
                    throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
                }

                // 결과 셋트
                schExclusiveZoneModel.ResultCnt = remoteData.ResultCnt;
                schExclusiveZoneModel.ResultCD = remoteData.ResultCD;

                _log.Debug("-----------------------------------------");
                _log.Debug("선택한 시간대 독점 편성 수정 End");
                _log.Debug("-----------------------------------------");

            }
            catch (FrameException fe)
            {
                _log.Warning("-----------------------------------------");
                _log.Warning(this.ToString() + ":SetSchExclusivUpdate():" + fe.ErrCode + ":" + fe.ResultMsg);
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
        #endregion

        #region 선택한 시간대 독점 정보
        public void GetTimeTargetDetail(SchExclusiveZoneModel schExclusiveZoneModel)
        {
            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug("선택한 시간대 독점 정보 Start");
                _log.Debug("-----------------------------------------");

                // 웹서비스 인스턴스 생성
                SchExclusiveZoneServicePloxy.SchExclusiveZoneService svc = new SchExclusiveZoneServicePloxy.SchExclusiveZoneService();

                // 웹서비스 URL동적 생성
                svc.Url = _WebServiceUrl;

                // 리모트 모델 생성
                SchExclusiveZoneServicePloxy.HeaderModel remoteHeader = new AdManagerClient.SchExclusiveZoneServicePloxy.HeaderModel();
                SchExclusiveZoneServicePloxy.SchExclusiveZoneModel remoteData = new AdManagerClient.SchExclusiveZoneServicePloxy.SchExclusiveZoneModel();

                // 헤더정보 셋트
                remoteHeader.ClientKey = Header.ClientKey;
                remoteHeader.UserID = Header.UserID;
                remoteHeader.UserLevel = Header.UserLevel;
                remoteHeader.UserClass = Header.UserClass;

                // 호출정보 셋트
                remoteData.ItemNo = schExclusiveZoneModel.ItemNo;
                

                // 웹서비스 호출 타임아웃설정
                svc.Timeout = FrameSystem.m_SystemTimeout;
                // 웹서비스 메소드 호출
                remoteData = svc.GetTimeTargetDetail(remoteHeader, remoteData);

                // 결과코드검사
                if (!remoteData.ResultCD.Equals("0000"))
                {
                    throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
                }

                // 결과 셋트
                schExclusiveZoneModel.SchExDetailDataSet = remoteData.SchExDetailDataSet.Copy();
                schExclusiveZoneModel.ResultCnt = remoteData.ResultCnt;
                schExclusiveZoneModel.ResultCD = remoteData.ResultCD;

                _log.Debug("-----------------------------------------");
                _log.Debug("타겟팅 목록조회 End");
                _log.Debug("-----------------------------------------");
            }
            catch (FrameException fe)
            {
                _log.Warning("-----------------------------------------");
                _log.Warning(this.ToString() + ":GetTimeTargetDetail():" + fe.ErrCode + ":" + fe.ResultMsg);
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
        #endregion

    }
}
