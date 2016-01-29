using System;
using System.Data;
using System.Diagnostics;

using WinFramework.Base;
using WinFramework.Data;
using WinFramework.Misc;

using AdManagerModel;

namespace AdManagerClient
{
    public class GroupOrganizationManager : BaseManager
    {
        // 순위변경구분
        const int ORDER_FIRST = 1;
        const int ORDER_LAST = 2;
        const int ORDER_UP = 3;
        const int ORDER_DOWN = 4;

        /// <summary>
        /// 생성자
        /// </summary>
        /// <param name="systemModel"></param>
        /// <param name="commonModel"></param>
        public GroupOrganizationManager(SystemModel systemModel, CommonModel commonModel) : base(systemModel, commonModel)
        {
            _log = FrameSystem.oLog;
            _module = "GroupOrganization";
            _Host = FrameSystem.m_WebServer_Host;
            _Port = FrameSystem.m_WebServer_Port;
            _Path = FrameSystem.m_WebServer_App + "/Media/GroupOrganizationService.asmx";
        }

        /// <summary>
        /// Service 호출을 위한 메소드
        /// </summary>
        public bool GetGroupDetail(BaseModel baseModel)
        {
            _log.Debug("-----------------------------------------");
            _log.Debug(this.ToString() + " Start");
            _log.Debug("-----------------------------------------");

            _log.Debug("-----------------------------------------");
            _log.Debug(this.ToString() + " End");
            _log.Debug("-----------------------------------------");

            return true;
        }

        /// <summary>
        /// 그룹목록 조회
        /// </summary>
        /// <param name="groupModel"></param>
        public void GetGroupList(GroupOrganizationModel groupOrganizationModel)
        {
            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug("그룹목록조회 Start");
                _log.Debug("-----------------------------------------");

                // 웹서비스 인스턴스 생성
                GroupOrganizationServicePloxy.GroupOrganizationService svc = new GroupOrganizationServicePloxy.GroupOrganizationService();
                svc.Url = _WebServiceUrl;

                // 리모트 모델 생성
                GroupOrganizationServicePloxy.HeaderModel remoteHeader = new AdManagerClient.GroupOrganizationServicePloxy.HeaderModel();
                GroupOrganizationServicePloxy.GroupOrganizationModel remoteData = new AdManagerClient.GroupOrganizationServicePloxy.GroupOrganizationModel();

                // 헤더정보 셋트
                remoteHeader.ClientKey = Header.ClientKey;
                remoteHeader.UserID    = Header.UserID;
                remoteHeader.UserLevel = Header.UserLevel;
                remoteHeader.UserClass = Header.UserClass;

                // 웹서비스 호출 타임아웃설정
                svc.Timeout = FrameSystem.m_SystemTimeout;

                // 웹서비스 메소드 호출
                remoteData = svc.GetGroupList(remoteHeader, remoteData);

                // 결과코드검사
                if (!remoteData.ResultCD.Equals("0000"))
                {
                    throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
                }

                // 결과 셋트
                groupOrganizationModel.GroupOrganizationDataSet = remoteData.GroupOrganizationDataSet.Copy();
                groupOrganizationModel.ResultCnt = remoteData.ResultCnt;
                groupOrganizationModel.ResultCD = remoteData.ResultCD;

                _log.Debug("-----------------------------------------");
                _log.Debug("그룹목록조회 End");
                _log.Debug("-----------------------------------------");
            }
            catch (FrameException fe)
            {
                _log.Warning("-----------------------------------------");
                _log.Warning(this.ToString() + ":GetGroupList():" + fe.ErrCode + ":" + fe.ResultMsg);
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
        /// 그룹목록 추가
        /// </summary>
        /// <param name="groupModel"></param>
        /// <returns></returns>
        public void SetGroupAdd(GroupOrganizationModel groupOrganizationModel)
        {
            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug("그룹목록추가 Start");
                _log.Debug("-----------------------------------------");

                if (groupOrganizationModel.GroupName.Length > 1000)
                {
                    throw new FrameException("그룹명의 길이는 1000Bytes를 초과할 수 없습니다.");
                }
                if (groupOrganizationModel.Comment.Length > 1000)
                {
                    throw new FrameException("비고란의 길이는 1000Bytes를 초과할 수 없습니다.");
                }

                // 웹서비스 인스턴스 생성
                GroupOrganizationServicePloxy.GroupOrganizationService svc = new GroupOrganizationServicePloxy.GroupOrganizationService();
                svc.Url = _WebServiceUrl;

                // 리모트 모델 생성
                GroupOrganizationServicePloxy.HeaderModel remoteHeader = new AdManagerClient.GroupOrganizationServicePloxy.HeaderModel();
                GroupOrganizationServicePloxy.GroupOrganizationModel remoteData = new AdManagerClient.GroupOrganizationServicePloxy.GroupOrganizationModel();

                // 헤더정보 셋트
                remoteHeader.ClientKey = Header.ClientKey;
                remoteHeader.UserID = Header.UserID;

                // 호출정보셋트				
                remoteData.GroupCode = groupOrganizationModel.GroupCode;
                remoteData.GroupName = groupOrganizationModel.GroupName;
                remoteData.Comment = groupOrganizationModel.Comment;
                remoteData.UseYn = groupOrganizationModel.UseYn;

                // 웹서비스 호출 타임아웃설정
                svc.Timeout = FrameSystem.m_SystemTimeout;

                // 웹서비스 메소드 호출
                remoteData = svc.SetGroupAdd(remoteHeader, remoteData);
                              
                // 결과코드검사
                if (!remoteData.ResultCD.Equals("0000"))
                {
                    throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
                }

                // 결과 셋트
                groupOrganizationModel.ResultCnt = remoteData.ResultCnt;
                groupOrganizationModel.ResultCD = remoteData.ResultCD;

                _log.Debug("-----------------------------------------");
                _log.Debug("그룹목록추가 End");
                _log.Debug("-----------------------------------------");
            }
            catch (FrameException fe)
            {
                _log.Warning("-----------------------------------------");
                _log.Warning(this.ToString() + ":SetGroupCreate():" + fe.ErrCode + ":" + fe.ResultMsg);
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
        /// 그룹목록 수정
        /// </summary>
        /// <param name="groupModel"></param>
        public void SetGroupUpdate(GroupOrganizationModel groupOrganizationModel)
        {
            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug("그룹목록수정 Start");
                _log.Debug("-----------------------------------------");

                //입력데이터의 Validation 검사			
                if (groupOrganizationModel.GroupName.Length > 1000)
                {
                    throw new FrameException("그룹명의 길이는 1000Bytes를 초과할 수 없습니다.");
                }
                if (groupOrganizationModel.Comment.Length > 1000)
                {
                    throw new FrameException("비고란의 길이는 1000Bytes를 초과할 수 없습니다.");
                }
                
                // 웹서비스 인스턴스 생성
                GroupOrganizationServicePloxy.GroupOrganizationService svc = new GroupOrganizationServicePloxy.GroupOrganizationService();
                svc.Url = _WebServiceUrl;

                // 리모트 모델 생성
                GroupOrganizationServicePloxy.HeaderModel remoteHeader = new AdManagerClient.GroupOrganizationServicePloxy.HeaderModel();
                GroupOrganizationServicePloxy.GroupOrganizationModel remoteData = new AdManagerClient.GroupOrganizationServicePloxy.GroupOrganizationModel();

                // 헤더정보 셋트
                remoteHeader.ClientKey = Header.ClientKey;
                remoteHeader.UserID = Header.UserID;

                // 호출정보셋트
                remoteData.GroupCode = groupOrganizationModel.GroupCode;
                remoteData.GroupName = groupOrganizationModel.GroupName;
                remoteData.Comment = groupOrganizationModel.Comment;
                remoteData.UseYn = groupOrganizationModel.UseYn;

                // 웹서비스 호출 타임아웃설정
                svc.Timeout = FrameSystem.m_SystemTimeout;

                // 웹서비스 메소드 호출
                remoteData = svc.SetGroupUpdate(remoteHeader, remoteData);

                // 결과코드검사
                if (!remoteData.ResultCD.Equals("0000"))
                {
                    throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
                }

                // 결과 셋트
                groupOrganizationModel.ResultCnt = remoteData.ResultCnt;
                groupOrganizationModel.ResultCD = remoteData.ResultCD;

                _log.Debug("-----------------------------------------");
                _log.Debug("그룹목록수정 End");
                _log.Debug("-----------------------------------------");
            }
            catch (FrameException fe)
            {
                _log.Warning("-----------------------------------------");
                _log.Warning(this.ToString() + ":SetGroupUpdate():" + fe.ErrCode + ":" + fe.ResultMsg);
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
        /// 그룹목록 삭제
        /// </summary>
        /// <param name="baseModel"></param>
        public void SetGroupDelete(GroupOrganizationModel groupOrganizationModel)
        {
            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug("그룹목록삭제 Start");
                _log.Debug("-----------------------------------------");

                // 웹서비스 인스턴스 생성
                GroupOrganizationServicePloxy.GroupOrganizationService svc = new GroupOrganizationServicePloxy.GroupOrganizationService();
                svc.Url = _WebServiceUrl;

                // 리모트 모델 생성
                GroupOrganizationServicePloxy.HeaderModel remoteHeader = new AdManagerClient.GroupOrganizationServicePloxy.HeaderModel();
                GroupOrganizationServicePloxy.GroupOrganizationModel remoteData = new AdManagerClient.GroupOrganizationServicePloxy.GroupOrganizationModel();

                // 헤더정보 셋트
                remoteHeader.ClientKey = Header.ClientKey;
                remoteHeader.UserID = Header.UserID;

                // 호출정보셋트
                remoteData.GroupCode = groupOrganizationModel.GroupCode;

                // 웹서비스 호출 타임아웃설정
                svc.Timeout = FrameSystem.m_SystemTimeout;

                // 웹서비스 메소드 호출
                remoteData = svc.SetGroupDelete(remoteHeader, remoteData);

                // 결과코드검사
                if (!remoteData.ResultCD.Equals("0000"))
                {
                    throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
                }

                // 결과 셋트
                groupOrganizationModel.ResultCnt = remoteData.ResultCnt;
                groupOrganizationModel.ResultCD = remoteData.ResultCD;

                _log.Debug("-----------------------------------------");
                _log.Debug("그룹목록삭제 End");
                _log.Debug("-----------------------------------------");
            }
            catch (FrameException fe)
            {
                _log.Warning("-----------------------------------------");
                _log.Warning(this.ToString() + ":SetGroupDelete():" + fe.ErrCode + ":" + fe.ResultMsg);
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
        /// 홈광고편성현황조회
        /// </summary>
        /// <param name="schHomeAdModel"></param>
        public void GetSchHomeAdList(GroupOrganizationModel groupOrganizationModel)
        {
            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug("홈광고편성현황조회 Start");
                _log.Debug("-----------------------------------------");

                // 웹서비스 인스턴스 생성
                GroupOrganizationServicePloxy.GroupOrganizationService svc = new GroupOrganizationServicePloxy.GroupOrganizationService();

                // 웹서비스 URL동적 생성
                svc.Url = _WebServiceUrl;

                // 리모트 모델 생성
                GroupOrganizationServicePloxy.HeaderModel remoteHeader = new AdManagerClient.GroupOrganizationServicePloxy.HeaderModel();
                GroupOrganizationServicePloxy.GroupOrganizationModel remoteData = new AdManagerClient.GroupOrganizationServicePloxy.GroupOrganizationModel();

                // 헤더정보 셋트
                remoteHeader.ClientKey = Header.ClientKey;
                remoteHeader.UserID = Header.UserID;
                remoteHeader.UserLevel = Header.UserLevel;
                remoteHeader.UserClass = Header.UserClass;

                // 호출정보 셋트
                remoteData.GroupCode = groupOrganizationModel.GroupCode;
                remoteData.ScheduleOrder = groupOrganizationModel.ScheduleOrder;

                // 웹서비스 호출 타임아웃설정
                svc.Timeout = FrameSystem.m_SystemTimeout;

                // 웹서비스 메소드 호출
                remoteData = svc.GetSchHomeAdList(remoteHeader, remoteData);

                // 결과코드검사
                if (!remoteData.ResultCD.Equals("0000"))
                {
                    throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
                }

                // 결과 셋트
                groupOrganizationModel.FileListCount = remoteData.FileListCount;  // 2007.10.02 RH.Jung 파일리스트건수검사

                groupOrganizationModel.GroupOrganizationDataSet = remoteData.GroupOrganizationDataSet.Copy();
                groupOrganizationModel.LastOrder = remoteData.LastOrder;
                groupOrganizationModel.ResultCnt = remoteData.ResultCnt;
                groupOrganizationModel.ResultCD = remoteData.ResultCD;

                _log.Debug("-----------------------------------------");
                _log.Debug("광고파일목록조회 End");
                _log.Debug("-----------------------------------------");
            }
            catch (FrameException fe)
            {
                _log.Warning("-----------------------------------------");
                _log.Warning(this.ToString() + ":GetSchHomeAdList():" + fe.ErrCode + ":" + fe.ResultMsg);
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
        /// 현재승인상태 조회
        /// </summary>
        /// <param name="schPublishModel"></param>
        /// <param name="adSchType"></param>
        public void GetPublishState(GroupOrganizationModel groupOrganizationModel, int adSchType)
        {
            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug("현재승인상태 조회 Start");
                _log.Debug("-----------------------------------------");

                // 웹서비스 인스턴스 생성
                GroupOrganizationServicePloxy.GroupOrganizationService svc = new GroupOrganizationServicePloxy.GroupOrganizationService();

                // 웹서비스URL 동적 셋트
                svc.Url = _WebServiceUrl;

                // 리모트 모델 생성
                GroupOrganizationServicePloxy.HeaderModel remoteHeader = new AdManagerClient.GroupOrganizationServicePloxy.HeaderModel();
                GroupOrganizationServicePloxy.GroupOrganizationModel remoteData = new AdManagerClient.GroupOrganizationServicePloxy.GroupOrganizationModel();

                // 헤더정보 셋트
                remoteHeader.ClientKey = Header.ClientKey;
                remoteHeader.UserID = Header.UserID;
                remoteHeader.UserLevel = Header.UserLevel;

                // 호출정보 셋트
                remoteData.GroupCode = groupOrganizationModel.GroupCode;

                // 웹서비스 호출 타임아웃설정
                svc.Timeout = FrameSystem.m_SystemTimeout;

                // 웹서비스 메소드 호출
                remoteData = svc.GetHomePublishState(remoteHeader, remoteData);

                // 결과코드검사
                if (!remoteData.ResultCD.Equals("0000"))
                {
                    throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
                }

                // 결과 셋트
                groupOrganizationModel.AckNo = remoteData.AckNo;
                groupOrganizationModel.State = remoteData.State;

                groupOrganizationModel.ResultCnt = remoteData.ResultCnt;
                groupOrganizationModel.ResultCD = remoteData.ResultCD;

                _log.Debug("-----------------------------------------");
                _log.Debug("현재승인상태 조회 End");
                _log.Debug("-----------------------------------------");
            }
            catch (FrameException fe)
            {
                _log.Warning("-----------------------------------------");
                _log.Warning(this.ToString() + ":GetScheduleList():" + fe.ErrCode + ":" + fe.ResultMsg);
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
        /// 광고편성추가
        /// </summary>
        /// <param name="schHomeAdModel"></param>
        /// <returns></returns>
        public void SetSchHomeAdAdd(GroupOrganizationModel groupOrganizationModel)
        {
            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug("홈광고편성추가 Start");
                _log.Debug("-----------------------------------------");

                //입력데이터의 Validation 검사
                if (groupOrganizationModel.ItemNo.Length < 1) throw new FrameException("광고가 선택되지 않았습니다.");

                // 웹서비스 인스턴스 생성
                GroupOrganizationServicePloxy.GroupOrganizationService svc = new GroupOrganizationServicePloxy.GroupOrganizationService();

                // 웹서비스 URL동적 생성
                svc.Url = _WebServiceUrl;

                // 리모트 모델 생성
                GroupOrganizationServicePloxy.HeaderModel remoteHeader = new AdManagerClient.GroupOrganizationServicePloxy.HeaderModel();
                GroupOrganizationServicePloxy.GroupOrganizationModel remoteData = new AdManagerClient.GroupOrganizationServicePloxy.GroupOrganizationModel();

                // 헤더정보 셋트
                remoteHeader.ClientKey = Header.ClientKey;
                remoteHeader.UserID = Header.UserID;
                remoteHeader.UserLevel = Header.UserLevel;
                remoteHeader.UserClass = Header.UserClass;

                // 호출정보 셋트
                remoteData.GroupCode = groupOrganizationModel.GroupCode;
                remoteData.MediaCode = groupOrganizationModel.MediaCode;
                remoteData.ItemNo = groupOrganizationModel.ItemNo;
                remoteData.ItemName = groupOrganizationModel.ItemName;

                // 웹서비스 호출 타임아웃설정
                svc.Timeout = FrameSystem.m_SystemTimeout;

                // 웹서비스 메소드 호출
                remoteData = svc.SetSchHomeAdCreate(remoteHeader, remoteData);

                // 결과코드검사
                if (!remoteData.ResultCD.Equals("0000"))
                {
                    throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
                }

                // 결과 셋트
                groupOrganizationModel.ScheduleOrder = remoteData.ScheduleOrder;
                groupOrganizationModel.ResultCnt = remoteData.ResultCnt;
                groupOrganizationModel.ResultCD = remoteData.ResultCD;

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
        /// 홈광고편성삭제
        /// </summary>
        /// <param name="schHomeAdModel"></param>
        /// <returns></returns>
        public void SetSchHomeAdDelete(GroupOrganizationModel groupOrganizationModel)
        {
            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug("홈광고편성삭제 Start");
                _log.Debug("-----------------------------------------");

                //입력데이터의 Validation 검사
                if (groupOrganizationModel.ItemNo.Length < 1)
                {
                    throw new FrameException("광고내역이 선택되지 않았습니다.");
                }

                // 웹서비스 인스턴스 생성
                GroupOrganizationServicePloxy.GroupOrganizationService svc = new GroupOrganizationServicePloxy.GroupOrganizationService();

                // 웹서비스 URL동적 생성
                svc.Url = _WebServiceUrl;

                // 리모트 모델 생성
                GroupOrganizationServicePloxy.HeaderModel remoteHeader = new AdManagerClient.GroupOrganizationServicePloxy.HeaderModel();
                GroupOrganizationServicePloxy.GroupOrganizationModel remoteData = new AdManagerClient.GroupOrganizationServicePloxy.GroupOrganizationModel();

                // 헤더정보 셋트
                remoteHeader.ClientKey = Header.ClientKey;
                remoteHeader.UserID = Header.UserID;
                remoteHeader.UserLevel = Header.UserLevel;
                remoteHeader.UserClass = Header.UserClass;

                // 호출정보 셋트
                remoteData.GroupCode = groupOrganizationModel.GroupCode;
                remoteData.MediaCode = groupOrganizationModel.MediaCode;
                remoteData.ItemNo = groupOrganizationModel.ItemNo;
                remoteData.ItemName = groupOrganizationModel.ItemName;
                remoteData.ScheduleOrder = groupOrganizationModel.ScheduleOrder;

                // 웹서비스 호출 타임아웃설정
                svc.Timeout = FrameSystem.m_SystemTimeout;

                // 웹서비스 메소드 호출
                remoteData = svc.SetSchHomeAdDelete(remoteHeader, remoteData);

                // 결과코드검사
                if (!remoteData.ResultCD.Equals("0000"))
                {
                    throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
                }

                // 결과 셋트
                groupOrganizationModel.ScheduleOrder = remoteData.ScheduleOrder;
                groupOrganizationModel.ResultCnt = remoteData.ResultCnt;
                groupOrganizationModel.ResultCD = remoteData.ResultCD;

                _log.Debug("-----------------------------------------");
                _log.Debug("홈광고편성삭제 End");
                _log.Debug("-----------------------------------------");
            }
            catch (FrameException fe)
            {
                _log.Warning("-----------------------------------------");
                _log.Warning(this.ToString() + ":SetSchHomeAdDelete():" + fe.ErrCode + ":" + fe.ResultMsg);
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
        /// 셋탑구분여부 편성여부 변경
        /// </summary>
        /// <param name="schHomeAdModel"></param>
        public void SetSchHomeAdCommonYn(GroupOrganizationModel groupOrganizationModel)
        {
            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug("홈광고편성 셋탑분류 변경");
                _log.Debug("-----------------------------------------");

                // 웹서비스 인스턴스 생성
                GroupOrganizationServicePloxy.GroupOrganizationService svc = new GroupOrganizationServicePloxy.GroupOrganizationService();
                svc.Url = _WebServiceUrl;

                // 리모트 모델 생성
                GroupOrganizationServicePloxy.HeaderModel remoteHeader = new AdManagerClient.GroupOrganizationServicePloxy.HeaderModel();
                GroupOrganizationServicePloxy.GroupOrganizationModel remoteData = new AdManagerClient.GroupOrganizationServicePloxy.GroupOrganizationModel();

                // 헤더정보 셋트
                remoteHeader.ClientKey = Header.ClientKey;
                remoteHeader.UserID = Header.UserID;
                remoteHeader.UserLevel = Header.UserLevel;
                remoteHeader.UserClass = Header.UserClass;

                // 호출정보 셋트
                remoteData.GroupCode = groupOrganizationModel.GroupCode;
                remoteData.ScheduleOrder = groupOrganizationModel.ScheduleOrder;
                remoteData.LogYn = groupOrganizationModel.LogYn;

                // 웹서비스 호출 타임아웃설정
                svc.Timeout = FrameSystem.m_SystemTimeout;

                // 웹서비스 메소드 호출
                remoteData = svc.SetSchHomeCommonYn(remoteHeader, remoteData);

                // 결과코드검사
                if (!remoteData.ResultCD.Equals("0000"))
                {
                    throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
                }

                // 결과 셋트
                groupOrganizationModel.ScheduleOrder = remoteData.ScheduleOrder;
                groupOrganizationModel.ResultCnt = remoteData.ResultCnt;
                groupOrganizationModel.ResultCD = remoteData.ResultCD;

                _log.Debug("-----------------------------------------");
                _log.Debug("홈광고편성 셋탑분류 End");
                _log.Debug("-----------------------------------------");
            }
            catch (FrameException fe)
            {
                _log.Warning("-----------------------------------------");
                _log.Warning(this.ToString() + ":SetSchHomeAdCommonYn():" + fe.ErrCode + ":" + fe.ResultMsg);
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
        /// 홈광고 로그 설정및 해제
        /// </summary>
        /// <param name="schHomeAdModel"></param>
        public void SetSchHomeAdLogYn(GroupOrganizationModel groupOrganizationModel)
        {
            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug("홈광고편성로그변경");
                _log.Debug("-----------------------------------------");

                // 웹서비스 인스턴스 생성
                GroupOrganizationServicePloxy.GroupOrganizationService svc = new GroupOrganizationServicePloxy.GroupOrganizationService();

                // 웹서비스 URL동적 생성
                svc.Url = _WebServiceUrl;

                // 리모트 모델 생성
                GroupOrganizationServicePloxy.HeaderModel remoteHeader = new AdManagerClient.GroupOrganizationServicePloxy.HeaderModel();
                GroupOrganizationServicePloxy.GroupOrganizationModel remoteData = new AdManagerClient.GroupOrganizationServicePloxy.GroupOrganizationModel();

                // 헤더정보 셋트
                remoteHeader.ClientKey = Header.ClientKey;
                remoteHeader.UserID = Header.UserID;
                remoteHeader.UserLevel = Header.UserLevel;
                remoteHeader.UserClass = Header.UserClass;

                // 호출정보 셋트
                remoteData.GroupCode = groupOrganizationModel.GroupCode;
                remoteData.ItemNo = groupOrganizationModel.ItemNo;
                remoteData.ScheduleOrder = groupOrganizationModel.ScheduleOrder;
                remoteData.LogYn = groupOrganizationModel.LogYn;

                // 웹서비스 호출 타임아웃설정
                svc.Timeout = FrameSystem.m_SystemTimeout;

                // 웹서비스 메소드 호출
                remoteData = svc.SetSchHomeAdLogYn(remoteHeader, remoteData);
                
                // 결과코드검사
                if (!remoteData.ResultCD.Equals("0000"))
                {
                    throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
                }

                // 결과 셋트
                groupOrganizationModel.ScheduleOrder = remoteData.ScheduleOrder;
                groupOrganizationModel.ResultCnt = remoteData.ResultCnt;
                groupOrganizationModel.ResultCD = remoteData.ResultCD;

                _log.Debug("-----------------------------------------");
                _log.Debug("홈광고편성추가 End");
                _log.Debug("-----------------------------------------");
            }
            catch (FrameException fe)
            {
                _log.Warning("-----------------------------------------");
                _log.Warning(this.ToString() + ":SetSchHomeAdLogYn():" + fe.ErrCode + ":" + fe.ResultMsg);
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
        /// 홈광고 편성내역 순위변경
        /// </summary>
        /// <param name="schHomeAdModel"></param>
        /// <returns></returns>
        public void SetSchHomeAdOrderSet(GroupOrganizationModel groupOrganizationModel, int OrderSet)
        {
            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug("홈광고편성 순위 변경 Start");
                _log.Debug("-----------------------------------------");

                //입력데이터의 Validation 검사
                if (groupOrganizationModel.ItemNo.Length < 1)
                {
                    throw new FrameException("광고내역이 선택되지 않았습니다.");
                }

                // 웹서비스 인스턴스 생성
                GroupOrganizationServicePloxy.GroupOrganizationService svc = new GroupOrganizationServicePloxy.GroupOrganizationService();

                // 웹서비스 URL동적 생성
                svc.Url = _WebServiceUrl;

                // 리모트 모델 생성
                GroupOrganizationServicePloxy.HeaderModel remoteHeader = new AdManagerClient.GroupOrganizationServicePloxy.HeaderModel();
                GroupOrganizationServicePloxy.GroupOrganizationModel remoteData = new AdManagerClient.GroupOrganizationServicePloxy.GroupOrganizationModel();

                // 헤더정보 셋트
                remoteHeader.ClientKey = Header.ClientKey;
                remoteHeader.UserID = Header.UserID;
                remoteHeader.UserLevel = Header.UserLevel;
                remoteHeader.UserClass = Header.UserClass;

                // 호출정보 셋트
                remoteData.GroupCode = groupOrganizationModel.GroupCode;
                remoteData.MediaCode = groupOrganizationModel.MediaCode;
                remoteData.ItemNo = groupOrganizationModel.ItemNo;
                remoteData.ItemName = groupOrganizationModel.ItemName;
                remoteData.ScheduleOrder = groupOrganizationModel.ScheduleOrder;

                switch (OrderSet)
                {
                    case ORDER_FIRST:
                        // 웹서비스 호출 타임아웃설정
                        svc.Timeout = FrameSystem.m_SystemTimeout;
                        // 웹서비스 메소드 호출
                        remoteData = svc.SetSchHomeAdOrderFirst(remoteHeader, remoteData);
                        break;
                    case ORDER_UP:
                        // 웹서비스 호출 타임아웃설정
                        svc.Timeout = FrameSystem.m_SystemTimeout;
                        // 웹서비스 메소드 호출
                        remoteData = svc.SetSchHomeAdOrderUp(remoteHeader, remoteData);
                        break;
                    case ORDER_DOWN:
                        // 웹서비스 호출 타임아웃설정
                        svc.Timeout = FrameSystem.m_SystemTimeout;
                        // 웹서비스 메소드 호출
                        remoteData = svc.SetSchHomeAdOrderDown(remoteHeader, remoteData);
                        break;
                    case ORDER_LAST:
                        // 웹서비스 호출 타임아웃설정
                        svc.Timeout = FrameSystem.m_SystemTimeout;
                        // 웹서비스 메소드 호출
                        remoteData = svc.SetSchHomeAdOrderLast(remoteHeader, remoteData);
                        break;
                    default:
                        throw new FrameException("순위변경 구분이 선택되지 않았습니다.");
                }

                // 결과코드검사
                if (!remoteData.ResultCD.Equals("0000"))
                {
                    throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
                }

                // 결과 셋트
                groupOrganizationModel.ResultCnt = remoteData.ResultCnt;
                groupOrganizationModel.ResultCD = remoteData.ResultCD;
                groupOrganizationModel.ScheduleOrder = remoteData.ScheduleOrder;

                _log.Debug("-----------------------------------------");
                _log.Debug("홈광고편성 첫번째 순위 변경 End");
                _log.Debug("-----------------------------------------");
            }
            catch (FrameException fe)
            {
                _log.Warning("-----------------------------------------");
                _log.Warning(this.ToString() + ":SetSchHomeAdOrderFirst():" + fe.ErrCode + ":" + fe.ResultMsg);
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
