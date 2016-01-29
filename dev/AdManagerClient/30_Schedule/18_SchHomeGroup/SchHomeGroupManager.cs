using System;
using System.Data;

using WinFramework.Base;
using WinFramework.Data;
using WinFramework.Misc;

using AdManagerModel;

namespace AdManagerClient
{
    public class SchHomeGroupManager : BaseManager
    {
        public SchHomeGroupManager(SystemModel systemModel, CommonModel commonModel)
            : base(systemModel, commonModel)
        {
            _log = FrameSystem.oLog;
            _module = "SchHomeGroup";
            _Host = FrameSystem.m_WebServer_Host;
            _Port = FrameSystem.m_WebServer_Port;
            _Path = FrameSystem.m_WebServer_App + "/Schedule/SchHomeGroupService.asmx";
        }

        /// <summary>
        /// 홈OAP그룹편성 주중 조회
        /// </summary>
        /// <param name="schHomeGroupModel"></param>
        public void GetSchHomeList1(SchHomeGroupModel schHomeGroupModel)
        {
            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug("홈OAP그룹편성 주중 조회 Start");
                _log.Debug("-----------------------------------------");

                // 웹서비스 인스턴스 생성
                SchHomeGroupServicePloxy.SchHomeGroupService svc = new SchHomeGroupServicePloxy.SchHomeGroupService();
                svc.Url = _WebServiceUrl;

                // 리모트 모델 생성
                SchHomeGroupServicePloxy.HeaderModel remoteHeader = new AdManagerClient.SchHomeGroupServicePloxy.HeaderModel();
                SchHomeGroupServicePloxy.SchHomeGroupModel remoteData = new SchHomeGroupServicePloxy.SchHomeGroupModel();

                // 헤더정보 셋트
                remoteHeader.ClientKey = Header.ClientKey;
                remoteHeader.UserID = Header.UserID;
                remoteHeader.UserLevel = Header.UserLevel;
                remoteHeader.UserClass = Header.UserClass;

                // 웹서비스 호출 타임아웃설정
                svc.Timeout = FrameSystem.m_SystemTimeout;

                remoteData.GroupCode = schHomeGroupModel.GroupCode;

                // 웹서비스 메소드 호출
                remoteData = svc.GetSchHomeList1(remoteHeader, remoteData);

                // 결과코드검사
                if (!remoteData.ResultCD.Equals("0000"))
                {
                    throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
                }

                // 결과 셋트
                schHomeGroupModel.SchHomeGroupModelDataSet = remoteData.SchHomeGroupModelDataSet.Copy();
                schHomeGroupModel.ResultCnt = remoteData.ResultCnt;
                schHomeGroupModel.ResultCD = remoteData.ResultCD;

                _log.Debug("-----------------------------------------");
                _log.Debug("홈OAP그룹편성 주중 조회 End");
                _log.Debug("-----------------------------------------");
            }
            catch (FrameException fe)
            {
                _log.Warning("-----------------------------------------");
                _log.Warning(this.ToString() + ":GetSchHomeList1():" + fe.ErrCode + ":" + fe.ResultMsg);
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
        /// 홈OAP그룹편성 주말 조회
        /// </summary>
        /// <param name="schHomeGroupModel"></param>
        public void GetSchHomeList2(SchHomeGroupModel schHomeGroupModel)
        {
            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug("홈OAP그룹편성 주말 조회 Start");
                _log.Debug("-----------------------------------------");

                // 웹서비스 인스턴스 생성
                SchHomeGroupServicePloxy.SchHomeGroupService svc = new SchHomeGroupServicePloxy.SchHomeGroupService();
                svc.Url = _WebServiceUrl;

                // 리모트 모델 생성
                SchHomeGroupServicePloxy.HeaderModel remoteHeader = new AdManagerClient.SchHomeGroupServicePloxy.HeaderModel();
                SchHomeGroupServicePloxy.SchHomeGroupModel remoteData = new SchHomeGroupServicePloxy.SchHomeGroupModel();

                // 헤더정보 셋트
                remoteHeader.ClientKey = Header.ClientKey;
                remoteHeader.UserID = Header.UserID;
                remoteHeader.UserLevel = Header.UserLevel;
                remoteHeader.UserClass = Header.UserClass;

                // 웹서비스 호출 타임아웃설정
                svc.Timeout = FrameSystem.m_SystemTimeout;

                remoteData.GroupCode = schHomeGroupModel.GroupCode;

                // 웹서비스 메소드 호출
                remoteData = svc.GetSchHomeList2(remoteHeader, remoteData);

                // 결과코드검사
                if (!remoteData.ResultCD.Equals("0000"))
                {
                    throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
                }

                // 결과 셋트
                schHomeGroupModel.SchHomeGroupModelDataSet = remoteData.SchHomeGroupModelDataSet.Copy();
                schHomeGroupModel.ResultCnt = remoteData.ResultCnt;
                schHomeGroupModel.ResultCD = remoteData.ResultCD;

                _log.Debug("-----------------------------------------");
                _log.Debug("홈OAP그룹편성 주말 조회 End");
                _log.Debug("-----------------------------------------");
            }
            catch (FrameException fe)
            {
                _log.Warning("-----------------------------------------");
                _log.Warning(this.ToString() + ":GetSchHomeList2():" + fe.ErrCode + ":" + fe.ResultMsg);
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
        /// 홈OAP그룹편성갯수 조회
        /// </summary>
        /// <param name="schHomeGroupModel"></param>
        public void GetSchHomeListCount(SchHomeGroupModel schHomeGroupModel)
        {
            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug("홈OAP그룹편성 조회 Start");
                _log.Debug("-----------------------------------------");

                // 웹서비스 인스턴스 생성
                SchHomeGroupServicePloxy.SchHomeGroupService svc = new SchHomeGroupServicePloxy.SchHomeGroupService();
                svc.Url = _WebServiceUrl;

                // 리모트 모델 생성
                SchHomeGroupServicePloxy.HeaderModel remoteHeader = new AdManagerClient.SchHomeGroupServicePloxy.HeaderModel();
                SchHomeGroupServicePloxy.SchHomeGroupModel remoteData = new SchHomeGroupServicePloxy.SchHomeGroupModel();

                // 헤더정보 셋트
                remoteHeader.ClientKey = Header.ClientKey;
                remoteHeader.UserID = Header.UserID;
                remoteHeader.UserLevel = Header.UserLevel;
                remoteHeader.UserClass = Header.UserClass;

                // 웹서비스 호출 타임아웃설정
                svc.Timeout = FrameSystem.m_SystemTimeout;

                remoteData.GroupCode = schHomeGroupModel.GroupCode;

                // 웹서비스 메소드 호출
                remoteData = svc.GetSchHomeListCount(remoteHeader, remoteData);

                // 결과코드검사
                if (!remoteData.ResultCD.Equals("0000"))
                {
                    throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
                }

                // 결과 셋트
                schHomeGroupModel.SchHomeGroupModelDataSet = remoteData.SchHomeGroupModelDataSet.Copy();
                schHomeGroupModel.Count = remoteData.Count;
                schHomeGroupModel.ResultCD = remoteData.ResultCD;

                _log.Debug("-----------------------------------------");
                _log.Debug("홈OAP그룹편성 조회 End");
                _log.Debug("-----------------------------------------");
            }
            catch (FrameException fe)
            {
                _log.Warning("-----------------------------------------");
                _log.Warning(this.ToString() + ":GetSchHomeList():" + fe.ErrCode + ":" + fe.ResultMsg);
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
        /// 홈광고그룹 목록 조회
        /// </summary>
        /// <param name="schHomeGroupModel"></param>
        public void GetSchHomeGroupList(SchHomeGroupModel schHomeGroupModel)
        {
            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug("홈OAP그룹편성 조회 Start");
                _log.Debug("-----------------------------------------");

                // 웹서비스 인스턴스 생성
                SchHomeGroupServicePloxy.SchHomeGroupService svc = new SchHomeGroupServicePloxy.SchHomeGroupService();
                svc.Url = _WebServiceUrl;

                // 리모트 모델 생성
                SchHomeGroupServicePloxy.HeaderModel remoteHeader = new AdManagerClient.SchHomeGroupServicePloxy.HeaderModel();
                SchHomeGroupServicePloxy.SchHomeGroupModel remoteData = new SchHomeGroupServicePloxy.SchHomeGroupModel();

                // 헤더정보 셋트
                remoteHeader.ClientKey = Header.ClientKey;
                remoteHeader.UserID = Header.UserID;
                remoteHeader.UserLevel = Header.UserLevel;
                remoteHeader.UserClass = Header.UserClass;

                remoteData.GroupCode = schHomeGroupModel.GroupCode;
                remoteData.GroupName = schHomeGroupModel.GroupName;

                // 웹서비스 호출 타임아웃설정
                svc.Timeout = FrameSystem.m_SystemTimeout;

                // 웹서비스 메소드 호출
                remoteData = svc.GetSchHomeGroupList(remoteHeader, remoteData);

                // 결과코드검사
                if (!remoteData.ResultCD.Equals("0000"))
                {
                    throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
                }

                // 결과 셋트
                schHomeGroupModel.SchHomeGroupModelDataSet = remoteData.SchHomeGroupModelDataSet.Copy();
                schHomeGroupModel.ResultCnt = remoteData.ResultCnt;
                schHomeGroupModel.ResultCD = remoteData.ResultCD;

                _log.Debug("-----------------------------------------");
                _log.Debug("홈OAP그룹편성 조회 End");
                _log.Debug("-----------------------------------------");
            }
            catch (FrameException fe)
            {
                _log.Warning("-----------------------------------------");
                _log.Warning(this.ToString() + ":GetSchHomeList():" + fe.ErrCode + ":" + fe.ResultMsg);
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
        /// 그룹별 광고목록 조회
        /// </summary>
        /// <param name="schHomeGroupModel"></param>
        public void GetSchHomeGroupDetailList(SchHomeGroupModel schHomeGroupModel)
        {
            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug("그룹별 광고목록 조회 Start");
                _log.Debug("-----------------------------------------");

                // 웹서비스 인스턴스 생성
                SchHomeGroupServicePloxy.SchHomeGroupService svc = new SchHomeGroupServicePloxy.SchHomeGroupService();
                svc.Url = _WebServiceUrl;

                // 리모트 모델 생성
                SchHomeGroupServicePloxy.HeaderModel remoteHeader = new AdManagerClient.SchHomeGroupServicePloxy.HeaderModel();
                SchHomeGroupServicePloxy.SchHomeGroupModel remoteData = new SchHomeGroupServicePloxy.SchHomeGroupModel();

                // 헤더정보 셋트
                remoteHeader.ClientKey = Header.ClientKey;
                remoteHeader.UserID = Header.UserID;
                remoteHeader.UserLevel = Header.UserLevel;
                remoteHeader.UserClass = Header.UserClass;

                remoteData.GroupCode = schHomeGroupModel.GroupCode;

                // 웹서비스 호출 타임아웃설정
                svc.Timeout = FrameSystem.m_SystemTimeout;

                // 웹서비스 메소드 호출
                remoteData = svc.GetSchHomeGroupDetailList(remoteHeader, remoteData);

                // 결과코드검사
                if (!remoteData.ResultCD.Equals("0000"))
                {
                    throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
                }

                // 결과 셋트
                schHomeGroupModel.SchHomeGroupModelDataSet = remoteData.SchHomeGroupModelDataSet.Copy();
                schHomeGroupModel.ResultCnt = remoteData.ResultCnt;
                schHomeGroupModel.ResultCD = remoteData.ResultCD;

                _log.Debug("-----------------------------------------");
                _log.Debug("홈OAP그룹편성 조회 End");
                _log.Debug("-----------------------------------------");
            }
            catch (FrameException fe)
            {
                _log.Warning("-----------------------------------------");
                _log.Warning(this.ToString() + ":GetSchHomeList():" + fe.ErrCode + ":" + fe.ResultMsg);
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
        /// 홈광고편성추가
        /// </summary>
        /// <param name="schHomeAdModel"></param>
        /// <returns></returns>
        public void SetSchHomeCreate(SchHomeGroupModel schHomeGroupModel)
        {
            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug("홈광고편성추가 Start");
                _log.Debug("-----------------------------------------");

                // 웹서비스 인스턴스 생성
                SchHomeGroupServicePloxy.SchHomeGroupService svc = new SchHomeGroupServicePloxy.SchHomeGroupService();
                svc.Url = _WebServiceUrl;

                // 리모트 모델 생성
                SchHomeGroupServicePloxy.HeaderModel remoteHeader = new AdManagerClient.SchHomeGroupServicePloxy.HeaderModel();
                SchHomeGroupServicePloxy.SchHomeGroupModel remoteData = new SchHomeGroupServicePloxy.SchHomeGroupModel();

                // 헤더정보 셋트
                remoteHeader.ClientKey = Header.ClientKey;
                remoteHeader.UserID = Header.UserID;
                remoteHeader.UserLevel = Header.UserLevel;
                remoteHeader.UserClass = Header.UserClass;

                // 호출정보 셋트
                remoteData.TgtWeek = schHomeGroupModel.TgtWeek;
                remoteData.TgtTime = schHomeGroupModel.TgtTime;
                remoteData.GroupCode = schHomeGroupModel.GroupCode;

                // 웹서비스 호출 타임아웃설정
                svc.Timeout = FrameSystem.m_SystemTimeout;
                // 웹서비스 메소드 호출
                remoteData = svc.SetSchHomeCreate(remoteHeader, remoteData);

                // 결과코드검사
                if (!remoteData.ResultCD.Equals("0000"))
                {
                    throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
                }

                // 결과 셋트
                schHomeGroupModel.ResultCnt = remoteData.ResultCnt;
                schHomeGroupModel.ResultCD = remoteData.ResultCD;

                _log.Debug("-----------------------------------------");
                _log.Debug("홈광고편성추가 End");
                _log.Debug("-----------------------------------------");
            }
            catch (FrameException fe)
            {
                _log.Warning("-----------------------------------------");
                _log.Warning(this.ToString() + ":SetSchHomeAdAdd():" + fe.ErrCode + ":" + fe.ResultMsg);
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
        /// 홈OAP그룹편성 수정 //??
        /// </summary>
        /// <param name="schHomeAdModel"></param>
        /// <returns></returns>
        public void SetSchHomeSave(SchHomeGroupModel schHomeGroupModel)
        {
            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug("홈광고편성추가 Start");
                _log.Debug("-----------------------------------------");

                // 웹서비스 인스턴스 생성
                SchHomeGroupServicePloxy.SchHomeGroupService svc = new SchHomeGroupServicePloxy.SchHomeGroupService();
                svc.Url = _WebServiceUrl;

                // 리모트 모델 생성
                SchHomeGroupServicePloxy.HeaderModel remoteHeader = new AdManagerClient.SchHomeGroupServicePloxy.HeaderModel();
                SchHomeGroupServicePloxy.SchHomeGroupModel remoteData = new SchHomeGroupServicePloxy.SchHomeGroupModel();

                // 헤더정보 셋트
                remoteHeader.ClientKey = Header.ClientKey;
                remoteHeader.UserID = Header.UserID;
                remoteHeader.UserLevel = Header.UserLevel;
                remoteHeader.UserClass = Header.UserClass;

                // 호출정보 셋트

                // 웹서비스 호출 타임아웃설정
                svc.Timeout = FrameSystem.m_SystemTimeout;
                // 웹서비스 메소드 호출
                remoteData = svc.SetSchHomeSave(remoteHeader, remoteData);

                // 결과코드검사
                if (!remoteData.ResultCD.Equals("0000"))
                {
                    throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
                }

                // 결과 셋트
                schHomeGroupModel.ResultCnt = remoteData.ResultCnt;
                schHomeGroupModel.ResultCD = remoteData.ResultCD;

                _log.Debug("-----------------------------------------");
                _log.Debug("홈광고편성추가 End");
                _log.Debug("-----------------------------------------");
            }
            catch (FrameException fe)
            {
                _log.Warning("-----------------------------------------");
                _log.Warning(this.ToString() + ":SetSchHomeAdAdd():" + fe.ErrCode + ":" + fe.ResultMsg);
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
        /// 홈OAP그룹편성 삭제
        /// </summary>
        /// <param name="schHomeAdModel"></param>
        /// <returns></returns>
        public void SetSchHomeDelete(SchHomeGroupModel schHomeGroupModel)
        {
            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug("홈OAP그룹편성 삭제 Start");
                _log.Debug("-----------------------------------------");

                // 웹서비스 인스턴스 생성
                SchHomeGroupServicePloxy.SchHomeGroupService svc = new SchHomeGroupServicePloxy.SchHomeGroupService();
                svc.Url = _WebServiceUrl;

                // 리모트 모델 생성
                SchHomeGroupServicePloxy.HeaderModel remoteHeader = new AdManagerClient.SchHomeGroupServicePloxy.HeaderModel();
                SchHomeGroupServicePloxy.SchHomeGroupModel remoteData = new SchHomeGroupServicePloxy.SchHomeGroupModel();

                // 헤더정보 셋트
                remoteHeader.ClientKey = Header.ClientKey;
                remoteHeader.UserID = Header.UserID;
                remoteHeader.UserLevel = Header.UserLevel;
                remoteHeader.UserClass = Header.UserClass;

                // 호출정보 셋트
                remoteData.TgtWeek = schHomeGroupModel.TgtWeek;
                remoteData.TgtTime = schHomeGroupModel.TgtTime;
                remoteData.GroupCode = schHomeGroupModel.GroupCode;
                remoteData.GroupName = schHomeGroupModel.GroupName;

                // 웹서비스 호출 타임아웃설정
                svc.Timeout = FrameSystem.m_SystemTimeout;
                // 웹서비스 메소드 호출
                remoteData = svc.SetSchHomeDelete(remoteHeader, remoteData);

                // 결과코드검사
                if (!remoteData.ResultCD.Equals("0000"))
                {
                    throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
                }

                // 결과 셋트
                schHomeGroupModel.ResultCnt = remoteData.ResultCnt;
                schHomeGroupModel.ResultCD = remoteData.ResultCD;

                _log.Debug("-----------------------------------------");
                _log.Debug("홈OAP그룹편성 삭제 End");
                _log.Debug("-----------------------------------------");
            }
            catch (FrameException fe)
            {
                _log.Warning("-----------------------------------------");
                _log.Warning(this.ToString() + ":SetSchHomeDelete():" + fe.ErrCode + ":" + fe.ResultMsg);
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
