using System;
using System.Data;

using WinFramework.Base;
using WinFramework.Data;
using WinFramework.Misc;

using AdManagerModel;

namespace AdManagerClient
{
    class AnalysisItemGroupManager : BaseManager
	{
		/// <summary>
		/// 생성자
		/// </summary>
		/// <param name="systemModel"></param>
		/// <param name="commonModel"></param>
        public AnalysisItemGroupManager(SystemModel systemModel, CommonModel commonModel)
            : base(systemModel, commonModel)
		{
			_log = FrameSystem.oLog;
			_module = "USERINFO";
			_Host  = FrameSystem.m_WebServer_Host;
			_Port  = FrameSystem.m_WebServer_Port;
            _Path = FrameSystem.m_WebServer_App + "/Media/AnalysisItemGroupService.asmx";
		}

        /// <summary>
        /// 수행월 조회
        /// </summary>
        /// <param name="mediarapModel"></param>
        public void GetAnalysisMonths(AnalysisItemGroupModel analysisItemGroupModel)
        {

            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug("수행월 조회 Start");
                _log.Debug("-----------------------------------------");

                // 웹서비스 인스턴스 생성
                AnalysisItemGroupServicePloxy.AnalysisItemGroupService svc = new AnalysisItemGroupServicePloxy.AnalysisItemGroupService();
                svc.Url = _WebServiceUrl;

                // 리모트 모델 생성
                AnalysisItemGroupServicePloxy.HeaderModel remoteHeader = new AdManagerClient.AnalysisItemGroupServicePloxy.HeaderModel();
                AnalysisItemGroupServicePloxy.AnalysisItemGroupModel remoteData = new AdManagerClient.AnalysisItemGroupServicePloxy.AnalysisItemGroupModel();

                // 헤더정보 셋트
                remoteHeader.ClientKey = Header.ClientKey;
                remoteHeader.UserID = Header.UserID;
                remoteHeader.UserLevel = Header.UserLevel;
                remoteHeader.UserClass = Header.UserClass;

                // 호출정보 셋트


                // 웹서비스 호출 타임아웃설정
                svc.Timeout = FrameSystem.m_SystemTimeout;

                // 웹서비스 메소드 호출
                remoteData = svc.GetAnalysisMonths(remoteHeader, remoteData);

                // 결과코드검사
                if (!remoteData.ResultCD.Equals("0000"))
                {
                    throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
                }

                // 결과 셋트
                analysisItemGroupModel.AnalysisMonthsDataSet = remoteData.AnalysisMonthsDataSet.Copy();
                analysisItemGroupModel.ResultCnt = remoteData.ResultCnt;
                analysisItemGroupModel.ResultCD = remoteData.ResultCD;

                _log.Debug("-----------------------------------------");
                _log.Debug("수행월 조회 End");
                _log.Debug("-----------------------------------------");
            }
            catch (FrameException fe)
            {
                _log.Warning("-----------------------------------------");
                _log.Warning(this.ToString() + ":SearchAnalysisMonths():" + fe.ErrCode + ":" + fe.ResultMsg);
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
        /// 분석용광고묶음조회
        /// </summary>
        /// <param name="mediarapModel"></param>
        public void GetAnalysisItemGroup(AnalysisItemGroupModel analysisItemGroupModel)
        {

            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug("분석용광고묶음조회 Start");
                _log.Debug("-----------------------------------------");

                // 웹서비스 인스턴스 생성
                AnalysisItemGroupServicePloxy.AnalysisItemGroupService svc = new AnalysisItemGroupServicePloxy.AnalysisItemGroupService();
                svc.Url = _WebServiceUrl;

                // 리모트 모델 생성
                AnalysisItemGroupServicePloxy.HeaderModel remoteHeader = new AdManagerClient.AnalysisItemGroupServicePloxy.HeaderModel();
                AnalysisItemGroupServicePloxy.AnalysisItemGroupModel remoteData = new AdManagerClient.AnalysisItemGroupServicePloxy.AnalysisItemGroupModel();

                // 헤더정보 셋트
                remoteHeader.ClientKey = Header.ClientKey;
                remoteHeader.UserID = Header.UserID;
                remoteHeader.UserLevel = Header.UserLevel;
                remoteHeader.UserClass = Header.UserClass;

                // 호출정보 셋트
                remoteData.SearchKey = analysisItemGroupModel.SearchKey;
                remoteData.SearchMonth = analysisItemGroupModel.SearchMonth;
                remoteData.AdvertiserCode = analysisItemGroupModel.AdvertiserCode;
                remoteData.AgencyCode = analysisItemGroupModel.AgencyCode;
                remoteData.RapCode = analysisItemGroupModel.RapCode;

                // 웹서비스 호출 타임아웃설정
                svc.Timeout = FrameSystem.m_SystemTimeout;

                // 웹서비스 메소드 호출
                remoteData = svc.GetAnalysisItemGroup(remoteHeader, remoteData);

                // 결과코드검사
                if (!remoteData.ResultCD.Equals("0000"))
                {
                    throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
                }

                // 결과 셋트
                analysisItemGroupModel.AnalysisItemGroupDataSet = remoteData.AnalysisItemGroupDataSet.Copy();
                analysisItemGroupModel.ResultCnt = remoteData.ResultCnt;
                analysisItemGroupModel.ResultCD = remoteData.ResultCD;

                _log.Debug("-----------------------------------------");
                _log.Debug("분석용광고묶음조회 End");
                _log.Debug("-----------------------------------------");
            }
            catch (FrameException fe)
            {
                _log.Warning("-----------------------------------------");
                _log.Warning(this.ToString() + ":GetAnalysisItemGroup():" + fe.ErrCode + ":" + fe.ResultMsg);
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
        /// 분석용광고묶음상세조회
        /// </summary>
        /// <param name="mediarapModel"></param>
        public void GetAnalysisItemGroupDetail(AnalysisItemGroupModel analysisItemGroupModel)
        {

            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug("분석용광고묶음상세조회 Start");
                _log.Debug("-----------------------------------------");

                // 웹서비스 인스턴스 생성
                AnalysisItemGroupServicePloxy.AnalysisItemGroupService svc = new AnalysisItemGroupServicePloxy.AnalysisItemGroupService();
                svc.Url = _WebServiceUrl;

                // 리모트 모델 생성
                AnalysisItemGroupServicePloxy.HeaderModel remoteHeader = new AdManagerClient.AnalysisItemGroupServicePloxy.HeaderModel();
                AnalysisItemGroupServicePloxy.AnalysisItemGroupModel remoteData = new AdManagerClient.AnalysisItemGroupServicePloxy.AnalysisItemGroupModel();

                // 헤더정보 셋트
                remoteHeader.ClientKey = Header.ClientKey;
                remoteHeader.UserID = Header.UserID;
                remoteHeader.UserLevel = Header.UserLevel;
                remoteHeader.UserClass = Header.UserClass;

                // 호출정보 셋트
                remoteData.AnalysisItemGroupNo = analysisItemGroupModel.AnalysisItemGroupNo;

                // 웹서비스 호출 타임아웃설정
                svc.Timeout = FrameSystem.m_SystemTimeout;

                // 웹서비스 메소드 호출
                remoteData = svc.GetAnalysisItemGroupDetail(remoteHeader, remoteData);

                // 결과코드검사
                if (!remoteData.ResultCD.Equals("0000"))
                {
                    throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
                }

                // 결과 셋트
                analysisItemGroupModel.AnalysisItemGroupDetailDataSet = remoteData.AnalysisItemGroupDetailDataSet.Copy();
                analysisItemGroupModel.ResultCnt = remoteData.ResultCnt;
                analysisItemGroupModel.ResultCD = remoteData.ResultCD;

                _log.Debug("-----------------------------------------");
                _log.Debug("분석용광고묶음상세조회 End");
                _log.Debug("-----------------------------------------");
            }
            catch (FrameException fe)
            {
                _log.Warning("-----------------------------------------");
                _log.Warning(this.ToString() + ":GetAnalysisItemGroupDetail():" + fe.ErrCode + ":" + fe.ResultMsg);
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
        /// 분석용광고내역조회
        /// </summary>
        /// <param name="mediarapModel"></param>
        public void GetContractItemList(AnalysisItemGroupModel analysisItemGroupModel)
        {

            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug("분석용광고내역조회 Start");
                _log.Debug("-----------------------------------------");

                // 웹서비스 인스턴스 생성
                AnalysisItemGroupServicePloxy.AnalysisItemGroupService svc = new AnalysisItemGroupServicePloxy.AnalysisItemGroupService();
                svc.Url = _WebServiceUrl;

                // 리모트 모델 생성
                AnalysisItemGroupServicePloxy.HeaderModel remoteHeader = new AdManagerClient.AnalysisItemGroupServicePloxy.HeaderModel();
                AnalysisItemGroupServicePloxy.AnalysisItemGroupModel remoteData = new AdManagerClient.AnalysisItemGroupServicePloxy.AnalysisItemGroupModel();

                // 헤더정보 셋트
                remoteHeader.ClientKey = Header.ClientKey;
                remoteHeader.UserID = Header.UserID;
                remoteHeader.UserLevel = Header.UserLevel;
                remoteHeader.UserClass = Header.UserClass;

                // 호출정보 셋트
                remoteData.SearchKey = analysisItemGroupModel.SearchKey;
                remoteData.SearchMonth = analysisItemGroupModel.SearchMonth;
                remoteData.AdvertiserCode = analysisItemGroupModel.AdvertiserCode;
                remoteData.AgencyCode = analysisItemGroupModel.AgencyCode;
                remoteData.RapCode = analysisItemGroupModel.RapCode;

                // 웹서비스 호출 타임아웃설정
                svc.Timeout = FrameSystem.m_SystemTimeout;

                // 웹서비스 메소드 호출
                remoteData = svc.GetContractItemList(remoteHeader, remoteData);

                // 결과코드검사
                if (!remoteData.ResultCD.Equals("0000"))
                {
                    throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
                }

                // 결과 셋트
                analysisItemGroupModel.ContractItemDataSet = remoteData.ContractItemDataSet.Copy();
                analysisItemGroupModel.ResultCnt = remoteData.ResultCnt;
                analysisItemGroupModel.ResultCD = remoteData.ResultCD;

                _log.Debug("-----------------------------------------");
                _log.Debug("분석용광고내역조회 End");
                _log.Debug("-----------------------------------------");
            }
            catch (FrameException fe)
            {
                _log.Warning("-----------------------------------------");
                _log.Warning(this.ToString() + ":GetContractItemList():" + fe.ErrCode + ":" + fe.ResultMsg);
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
        /// 분석용광고묶음생성
        /// </summary>
        /// <param name="mediarapModel"></param>
        public void SetAnalysisItemGroupCreate(AnalysisItemGroupModel analysisItemGroupModel)
        {

            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug("분석용광고묶음생성 Start");
                _log.Debug("-----------------------------------------");

                // 웹서비스 인스턴스 생성
                AnalysisItemGroupServicePloxy.AnalysisItemGroupService svc = new AnalysisItemGroupServicePloxy.AnalysisItemGroupService();
                svc.Url = _WebServiceUrl;

                // 리모트 모델 생성
                AnalysisItemGroupServicePloxy.HeaderModel remoteHeader = new AdManagerClient.AnalysisItemGroupServicePloxy.HeaderModel();
                AnalysisItemGroupServicePloxy.AnalysisItemGroupModel remoteData = new AdManagerClient.AnalysisItemGroupServicePloxy.AnalysisItemGroupModel();

                // 헤더정보 셋트
                remoteHeader.ClientKey = Header.ClientKey;
                remoteHeader.UserID = Header.UserID;
                remoteHeader.UserLevel = Header.UserLevel;
                remoteHeader.UserClass = Header.UserClass;

                // 호출정보 셋트
                remoteData.AnalysisItemGroupName = analysisItemGroupModel.AnalysisItemGroupName;
                remoteData.AnalysisItemGroupMonth = analysisItemGroupModel.AnalysisItemGroupMonth;
                remoteData.AnalysisItemGroupType = analysisItemGroupModel.AnalysisItemGroupType;
                remoteData.Comment = analysisItemGroupModel.Comment;

                // 웹서비스 호출 타임아웃설정
                svc.Timeout = FrameSystem.m_SystemTimeout;

                // 웹서비스 메소드 호출
                remoteData = svc.SetAnalysisItemGroupCreate(remoteHeader, remoteData);

                // 결과코드검사
                if (!remoteData.ResultCD.Equals("0000"))
                {
                    throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
                }

                // 결과 셋트
                analysisItemGroupModel.AnalysisItemGroupNo = remoteData.AnalysisItemGroupNo;
                analysisItemGroupModel.ResultCnt = remoteData.ResultCnt;
                analysisItemGroupModel.ResultCD = remoteData.ResultCD;

                _log.Debug("-----------------------------------------");
                _log.Debug("분석용광고묶음조회 End");
                _log.Debug("-----------------------------------------");
            }
            catch (FrameException fe)
            {
                _log.Warning("-----------------------------------------");
                _log.Warning(this.ToString() + ":SetAnalysisItemGroupCreate():" + fe.ErrCode + ":" + fe.ResultMsg);
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
        /// 분석용광고묶음상세생성
        /// </summary>
        /// <param name="mediarapModel"></param>
        public void SetAnalysisItemGroupDetailCreate(AnalysisItemGroupModel analysisItemGroupModel)
        {

            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug("분석용광고묶음상세생성 Start");
                _log.Debug("-----------------------------------------");

                // 웹서비스 인스턴스 생성
                AnalysisItemGroupServicePloxy.AnalysisItemGroupService svc = new AnalysisItemGroupServicePloxy.AnalysisItemGroupService();
                svc.Url = _WebServiceUrl;

                // 리모트 모델 생성
                AnalysisItemGroupServicePloxy.HeaderModel remoteHeader = new AdManagerClient.AnalysisItemGroupServicePloxy.HeaderModel();
                AnalysisItemGroupServicePloxy.AnalysisItemGroupModel remoteData = new AdManagerClient.AnalysisItemGroupServicePloxy.AnalysisItemGroupModel();

                // 헤더정보 셋트
                remoteHeader.ClientKey = Header.ClientKey;
                remoteHeader.UserID = Header.UserID;
                remoteHeader.UserLevel = Header.UserLevel;
                remoteHeader.UserClass = Header.UserClass;

                // 호출정보 셋트
                remoteData.AnalysisItemGroupNo = analysisItemGroupModel.AnalysisItemGroupNo;
                remoteData.ItemNo = analysisItemGroupModel.ItemNo;

                // 웹서비스 호출 타임아웃설정
                svc.Timeout = FrameSystem.m_SystemTimeout;

                // 웹서비스 메소드 호출
                remoteData = svc.SetAnalysisItemGroupDetailCreate(remoteHeader, remoteData);

                // 결과코드검사
                if (!remoteData.ResultCD.Equals("0000"))
                {
                    throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
                }

                // 결과 셋트
                analysisItemGroupModel.ResultCnt = remoteData.ResultCnt;
                analysisItemGroupModel.ResultCD = remoteData.ResultCD;

                _log.Debug("-----------------------------------------");
                _log.Debug("분석용광고묶음상세생성 End");
                _log.Debug("-----------------------------------------");
            }
            catch (FrameException fe)
            {
                _log.Warning("-----------------------------------------");
                _log.Warning(this.ToString() + ":SetAnalysisItemGroupDetailCreate():" + fe.ErrCode + ":" + fe.ResultMsg);
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
        /// 분석용광고묶음수정
        /// </summary>
        /// <param name="mediarapModel"></param>
        public void SetAnalysisItemGroupUpdate(AnalysisItemGroupModel analysisItemGroupModel)
        {

            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug("분석용광고묶음수정 Start");
                _log.Debug("-----------------------------------------");

                // 웹서비스 인스턴스 생성
                AnalysisItemGroupServicePloxy.AnalysisItemGroupService svc = new AnalysisItemGroupServicePloxy.AnalysisItemGroupService();
                svc.Url = _WebServiceUrl;

                // 리모트 모델 생성
                AnalysisItemGroupServicePloxy.HeaderModel remoteHeader = new AdManagerClient.AnalysisItemGroupServicePloxy.HeaderModel();
                AnalysisItemGroupServicePloxy.AnalysisItemGroupModel remoteData = new AdManagerClient.AnalysisItemGroupServicePloxy.AnalysisItemGroupModel();

                // 헤더정보 셋트
                remoteHeader.ClientKey = Header.ClientKey;
                remoteHeader.UserID = Header.UserID;
                remoteHeader.UserLevel = Header.UserLevel;
                remoteHeader.UserClass = Header.UserClass;

                // 호출정보 셋트
                remoteData.AnalysisItemGroupNo = analysisItemGroupModel.AnalysisItemGroupNo;
                remoteData.AnalysisItemGroupName = analysisItemGroupModel.AnalysisItemGroupName;
                remoteData.AnalysisItemGroupType = analysisItemGroupModel.AnalysisItemGroupType;
                remoteData.Comment = analysisItemGroupModel.Comment;

                // 웹서비스 호출 타임아웃설정
                svc.Timeout = FrameSystem.m_SystemTimeout;

                // 웹서비스 메소드 호출
                remoteData = svc.SetAnalysisItemGroupUpdate(remoteHeader, remoteData);

                // 결과코드검사
                if (!remoteData.ResultCD.Equals("0000"))
                {
                    throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
                }

                // 결과 셋트
                analysisItemGroupModel.AnalysisItemGroupNo = remoteData.AnalysisItemGroupNo;
                analysisItemGroupModel.ResultCnt = remoteData.ResultCnt;
                analysisItemGroupModel.ResultCD = remoteData.ResultCD;

                _log.Debug("-----------------------------------------");
                _log.Debug("분석용광고묶음수정 End");
                _log.Debug("-----------------------------------------");
            }
            catch (FrameException fe)
            {
                _log.Warning("-----------------------------------------");
                _log.Warning(this.ToString() + ":SetAnalysisItemGroupUpdate():" + fe.ErrCode + ":" + fe.ResultMsg);
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
        /// 분석용광고묶음삭제
        /// </summary>
        /// <param name="mediarapModel"></param>
        public void SetAnalysisItemGroupDelete(AnalysisItemGroupModel analysisItemGroupModel)
        {

            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug("분석용광고묶음삭제 Start");
                _log.Debug("-----------------------------------------");

                // 웹서비스 인스턴스 생성
                AnalysisItemGroupServicePloxy.AnalysisItemGroupService svc = new AnalysisItemGroupServicePloxy.AnalysisItemGroupService();
                svc.Url = _WebServiceUrl;

                // 리모트 모델 생성
                AnalysisItemGroupServicePloxy.HeaderModel remoteHeader = new AdManagerClient.AnalysisItemGroupServicePloxy.HeaderModel();
                AnalysisItemGroupServicePloxy.AnalysisItemGroupModel remoteData = new AdManagerClient.AnalysisItemGroupServicePloxy.AnalysisItemGroupModel();

                // 헤더정보 셋트
                remoteHeader.ClientKey = Header.ClientKey;
                remoteHeader.UserID = Header.UserID;
                remoteHeader.UserLevel = Header.UserLevel;
                remoteHeader.UserClass = Header.UserClass;

                // 호출정보 셋트
                remoteData.AnalysisItemGroupNo = analysisItemGroupModel.AnalysisItemGroupNo;

                // 웹서비스 호출 타임아웃설정
                svc.Timeout = FrameSystem.m_SystemTimeout;

                // 웹서비스 메소드 호출
                remoteData = svc.SetAnalysisItemGroupDelete(remoteHeader, remoteData);

                // 결과코드검사
                if (!remoteData.ResultCD.Equals("0000"))
                {
                    throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
                }

                // 결과 셋트
                analysisItemGroupModel.AnalysisItemGroupNo = remoteData.AnalysisItemGroupNo;
                analysisItemGroupModel.ResultCnt = remoteData.ResultCnt;
                analysisItemGroupModel.ResultCD = remoteData.ResultCD;

                _log.Debug("-----------------------------------------");
                _log.Debug("분석용광고묶음삭제 End");
                _log.Debug("-----------------------------------------");
            }
            catch (FrameException fe)
            {
                _log.Warning("-----------------------------------------");
                _log.Warning(this.ToString() + ":SetAnalysisItemGroupDelete():" + fe.ErrCode + ":" + fe.ResultMsg);
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
        /// 분석용광고묶음상세삭제
        /// </summary>
        /// <param name="mediarapModel"></param>
        public void SetAnalysisItemGroupDetailDelete(AnalysisItemGroupModel analysisItemGroupModel)
        {

            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug("분석용광고묶음상세삭제 Start");
                _log.Debug("-----------------------------------------");

                // 웹서비스 인스턴스 생성
                AnalysisItemGroupServicePloxy.AnalysisItemGroupService svc = new AnalysisItemGroupServicePloxy.AnalysisItemGroupService();
                svc.Url = _WebServiceUrl;

                // 리모트 모델 생성
                AnalysisItemGroupServicePloxy.HeaderModel remoteHeader = new AdManagerClient.AnalysisItemGroupServicePloxy.HeaderModel();
                AnalysisItemGroupServicePloxy.AnalysisItemGroupModel remoteData = new AdManagerClient.AnalysisItemGroupServicePloxy.AnalysisItemGroupModel();

                // 헤더정보 셋트
                remoteHeader.ClientKey = Header.ClientKey;
                remoteHeader.UserID = Header.UserID;
                remoteHeader.UserLevel = Header.UserLevel;
                remoteHeader.UserClass = Header.UserClass;

                // 호출정보 셋트
                remoteData.AnalysisItemGroupNo = analysisItemGroupModel.AnalysisItemGroupNo;
                remoteData.ItemNo = analysisItemGroupModel.ItemNo;

                // 웹서비스 호출 타임아웃설정
                svc.Timeout = FrameSystem.m_SystemTimeout;

                // 웹서비스 메소드 호출
                remoteData = svc.SetAnalysisItemGroupDetailDelete(remoteHeader, remoteData);

                // 결과코드검사
                if (!remoteData.ResultCD.Equals("0000"))
                {
                    throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
                }

                // 결과 셋트
                analysisItemGroupModel.ResultCnt = remoteData.ResultCnt;
                analysisItemGroupModel.ResultCD = remoteData.ResultCD;

                _log.Debug("-----------------------------------------");
                _log.Debug("분석용광고묶음상세삭제 End");
                _log.Debug("-----------------------------------------");
            }
            catch (FrameException fe)
            {
                _log.Warning("-----------------------------------------");
                _log.Warning(this.ToString() + ":SetAnalysisItemGroupDetailDelete():" + fe.ErrCode + ":" + fe.ResultMsg);
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
