// ===============================================================================
// SchChoiceAd Manager  for Charites Project
//
// SchChoiceAdManager.cs
//
// 광고파일정보 서비스를 호출합니다. 
//
// ===============================================================================
// Release history
//
// ===============================================================================
// Copyright (C) 2006 G-Inno Systems Inc.
// All rights reserved.
// ==============================================================================
/*
 * [수정사항]
 * -------------------------------------------------------
 * 수정코드  : [E_01]
 * 수정자    : YJ.Park
 * 수정일    : 2014.08.8
 * 수정내용  :        
 *            - 편성복사 기능 추가
 * --------------------------------------------------------
 */
using System;
using System.Data;

using AdManagerModel;
using WinFramework.Base;
using WinFramework.Data;
using WinFramework.Misc;

namespace AdManagerClient
{
    /// <summary>
    /// 지정광고편성정보 웹서비스를 호출합니다. 
    /// </summary>
    public class SchChoiceAdManager : BaseManager
    {
        /// <summary>
        /// 생성자
        /// </summary>
        /// <param name="systemModel"></param>
        /// <param name="commonModel"></param>

        public SchChoiceAdManager(SystemModel systemModel, CommonModel commonModel)
            : base(systemModel, commonModel)
        {
            _log = FrameSystem.oLog;
            _module = "SchChoiceAd";
            _Host = FrameSystem.m_WebServer_Host;
            _Port = FrameSystem.m_WebServer_Port;
            _Path = FrameSystem.m_WebServer_App + "/Schedule/SchChoiceAdService.asmx";
        }

        /// <summary>
        /// 광고목록조회
        /// 편성비율조정에서 호출한다
        /// </summary>
        /// <param name="schChoiceAdModel"></param>
        public void GetAdList10(SchChoiceAdModel schChoiceAdModel)
        {
            try
            {
                SchChoiceAdServicePloxy.SchChoiceAdService svc = new SchChoiceAdServicePloxy.SchChoiceAdService();

                // 리모트 모델 생성
                SchChoiceAdServicePloxy.HeaderModel remoteHeader = new AdManagerClient.SchChoiceAdServicePloxy.HeaderModel();
                SchChoiceAdServicePloxy.SchChoiceAdModel remoteData = new AdManagerClient.SchChoiceAdServicePloxy.SchChoiceAdModel();

                // 헤더정보 셋트
                remoteHeader.ClientKey = Header.ClientKey;
                remoteHeader.UserID = Header.UserID;
                remoteHeader.UserLevel = Header.UserLevel;
                remoteHeader.UserClass = Header.UserClass;

                // 호출정보 셋트
                remoteData.SearchKey = schChoiceAdModel.SearchKey;
                remoteData.SearchMediaCode = schChoiceAdModel.SearchMediaCode;
                remoteData.SearchRapCode = schChoiceAdModel.SearchRapCode;
                remoteData.SearchAgencyCode = schChoiceAdModel.SearchAgencyCode;
                remoteData.SearchAdvertiserCode = schChoiceAdModel.SearchAdvertiserCode;
                remoteData.SearchContractState = schChoiceAdModel.SearchContractState;
                remoteData.SearchAdClass = schChoiceAdModel.SearchAdClass;
                remoteData.SearchchkAdState_20 = schChoiceAdModel.SearchchkAdState_20;
                remoteData.SearchchkAdState_30 = schChoiceAdModel.SearchchkAdState_30;
                remoteData.SearchchkAdState_40 = schChoiceAdModel.SearchchkAdState_40;
                remoteData.AdType = schChoiceAdModel.AdType;

                svc.Url = _WebServiceUrl;
                svc.Timeout = FrameSystem.m_SystemTimeout;

                remoteData = svc.mGetAdList10(remoteHeader, remoteData);

                // 결과코드검사
                if (!remoteData.ResultCD.Equals("0000"))
                {
                    throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
                }

                // 결과 셋트
                schChoiceAdModel.ScheduleDataSet = remoteData.ScheduleDataSet.Copy();
                schChoiceAdModel.ResultCnt = remoteData.ResultCnt;
                schChoiceAdModel.ResultCD = remoteData.ResultCD;
            }
            catch (FrameException fe)
            {
                _log.Warning("-----------------------------------------");
                _log.Warning(this.ToString() + ":GetSchChoiceAdList():" + fe.ErrCode + ":" + fe.ResultMsg);
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
        /// 지정광고편성현황조회
        /// </summary>
        /// <param name="schChoiceAdModel"></param>
        public void GetSchChoiceAdList(SchChoiceAdModel schChoiceAdModel)
        {

            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug("지정광고편성현황조회 Start");
                _log.Debug("-----------------------------------------");

                // 웹서비스 인스턴스 생성
                SchChoiceAdServicePloxy.SchChoiceAdService svc = new SchChoiceAdServicePloxy.SchChoiceAdService();

                // 웹서비스 URL동적 생성
                svc.Url = _WebServiceUrl;

                // 리모트 모델 생성
                SchChoiceAdServicePloxy.HeaderModel remoteHeader = new AdManagerClient.SchChoiceAdServicePloxy.HeaderModel();
                SchChoiceAdServicePloxy.SchChoiceAdModel remoteData = new AdManagerClient.SchChoiceAdServicePloxy.SchChoiceAdModel();

                // 헤더정보 셋트
                remoteHeader.ClientKey = Header.ClientKey;
                remoteHeader.UserID = Header.UserID;
                remoteHeader.UserLevel = Header.UserLevel;
                remoteHeader.UserClass = Header.UserClass;

                // 호출정보 셋트
                remoteData.SearchKey = schChoiceAdModel.SearchKey;
                remoteData.SearchMediaCode = schChoiceAdModel.SearchMediaCode;
                remoteData.SearchRapCode = schChoiceAdModel.SearchRapCode;
                remoteData.SearchAgencyCode = schChoiceAdModel.SearchAgencyCode;
                remoteData.SearchAdvertiserCode = schChoiceAdModel.SearchAdvertiserCode;
                remoteData.SearchContractState = schChoiceAdModel.SearchContractState;
                remoteData.SearchAdClass = schChoiceAdModel.SearchAdClass;
                remoteData.SearchchkAdState_20 = schChoiceAdModel.SearchchkAdState_20;
                remoteData.SearchchkAdState_30 = schChoiceAdModel.SearchchkAdState_30;
                remoteData.SearchchkAdState_40 = schChoiceAdModel.SearchchkAdState_40;
                remoteData.AdType = schChoiceAdModel.AdType;

                // 웹서비스 호출 타임아웃설정
                svc.Timeout = FrameSystem.m_SystemTimeout;
                // 웹서비스 메소드 호출
                remoteData = svc.GetSchChoiceAdList(remoteHeader, remoteData);

                // 결과코드검사
                if (!remoteData.ResultCD.Equals("0000"))
                {
                    throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
                }

                // 결과 셋트
                schChoiceAdModel.ScheduleDataSet = remoteData.ScheduleDataSet.Copy();
                schChoiceAdModel.ResultCnt = remoteData.ResultCnt;
                schChoiceAdModel.ResultCD = remoteData.ResultCD;

                _log.Debug("-----------------------------------------");
                _log.Debug("광고파일목록조회 End");
                _log.Debug("-----------------------------------------");
            }
            catch (FrameException fe)
            {
                _log.Warning("-----------------------------------------");
                _log.Warning(this.ToString() + ":GetSchChoiceAdList():" + fe.ErrCode + ":" + fe.ResultMsg);
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
        /// 엑셀데이터 검증-파일명과 파일위치
        /// </summary>
        /// <param name="schChoiceAdModel"></param>
        public void GetInspectItemList(SchChoiceAdModel schChoiceAdModel)
        {

            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug("지정광고편성대상조회 Start");
                _log.Debug("-----------------------------------------");

                // 웹서비스 인스턴스 생성
                SchChoiceAdServicePloxy.SchChoiceAdService svc = new SchChoiceAdServicePloxy.SchChoiceAdService();

                // 웹서비스 URL동적 생성
                svc.Url = _WebServiceUrl;

                // 리모트 모델 생성
                SchChoiceAdServicePloxy.HeaderModel remoteHeader = new AdManagerClient.SchChoiceAdServicePloxy.HeaderModel();
                SchChoiceAdServicePloxy.SchChoiceAdModel remoteData = new AdManagerClient.SchChoiceAdServicePloxy.SchChoiceAdModel();

                // 헤더정보 셋트
                remoteHeader.ClientKey = Header.ClientKey;
                remoteHeader.UserID = Header.UserID;
                remoteHeader.UserLevel = Header.UserLevel;
                remoteHeader.UserClass = Header.UserClass;

                // 호출정보 셋트
                remoteData.SearchKey = schChoiceAdModel.SearchKey;
                remoteData.SearchMediaCode = schChoiceAdModel.SearchMediaCode;
                remoteData.SearchRapCode = schChoiceAdModel.SearchRapCode;
                remoteData.SearchAgencyCode = schChoiceAdModel.SearchAgencyCode;
                remoteData.SearchAdvertiserCode = schChoiceAdModel.SearchAdvertiserCode;
                remoteData.SearchchkAdState_10 = schChoiceAdModel.SearchchkAdState_10;
                remoteData.SearchchkAdState_20 = schChoiceAdModel.SearchchkAdState_20;
                remoteData.SearchchkAdState_30 = schChoiceAdModel.SearchchkAdState_30;
                remoteData.SearchchkAdState_40 = schChoiceAdModel.SearchchkAdState_40;

                // 웹서비스 호출 타임아웃설정
                svc.Timeout = FrameSystem.m_SystemTimeout;
                // 웹서비스 메소드 호출
                remoteData = svc.GetInspectItemList(remoteHeader, remoteData);

                // 결과코드검사
                if (!remoteData.ResultCD.Equals("0000"))
                {
                    throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
                }

                // 결과 셋트
                schChoiceAdModel.ScheduleDataSet = remoteData.ScheduleDataSet.Copy();
                schChoiceAdModel.ResultCnt = remoteData.ResultCnt;
                schChoiceAdModel.ResultCD = remoteData.ResultCD;

                _log.Debug("-----------------------------------------");
                _log.Debug("광고파일목록조회 End");
                _log.Debug("-----------------------------------------");
            }
            catch (FrameException fe)
            {
                _log.Warning("-----------------------------------------");
                _log.Warning(this.ToString() + ":GetInspectItemList():" + fe.ErrCode + ":" + fe.ResultMsg);
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
        /// 지정광고편성대상조회
        /// </summary>
        /// <param name="schChoiceAdModel"></param>
        public void GetContractItemList(SchChoiceAdModel schChoiceAdModel)
        {
            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug("지정광고편성대상조회 Start");
                _log.Debug("-----------------------------------------");

                // 웹서비스 인스턴스 생성
                SchChoiceAdServicePloxy.SchChoiceAdService svc = new SchChoiceAdServicePloxy.SchChoiceAdService();

                // 웹서비스 URL동적 생성
                svc.Url = _WebServiceUrl;

                // 리모트 모델 생성
                SchChoiceAdServicePloxy.HeaderModel remoteHeader = new AdManagerClient.SchChoiceAdServicePloxy.HeaderModel();
                SchChoiceAdServicePloxy.SchChoiceAdModel remoteData = new AdManagerClient.SchChoiceAdServicePloxy.SchChoiceAdModel();

                // 헤더정보 셋트
                remoteHeader.ClientKey = Header.ClientKey;
                remoteHeader.UserID = Header.UserID;
                remoteHeader.UserLevel = Header.UserLevel;
                remoteHeader.UserClass = Header.UserClass;

                // 호출정보 셋트
                remoteData.SearchKey = schChoiceAdModel.SearchKey;
                remoteData.SearchMediaCode = schChoiceAdModel.SearchMediaCode;
                remoteData.SearchRapCode = schChoiceAdModel.SearchRapCode;
                remoteData.SearchAgencyCode = schChoiceAdModel.SearchAgencyCode;
                remoteData.SearchAdvertiserCode = schChoiceAdModel.SearchAdvertiserCode;
                remoteData.SearchchkAdState_10 = schChoiceAdModel.SearchchkAdState_10;
                remoteData.SearchchkAdState_20 = schChoiceAdModel.SearchchkAdState_20;
                remoteData.SearchchkAdState_30 = schChoiceAdModel.SearchchkAdState_30;
                remoteData.SearchchkAdState_40 = schChoiceAdModel.SearchchkAdState_40;
                remoteData.SearchAdType = schChoiceAdModel.SearchAdType;
                remoteData.AdType = schChoiceAdModel.AdType;

                // 웹서비스 호출 타임아웃설정
                svc.Timeout = FrameSystem.m_SystemTimeout;
                // 웹서비스 메소드 호출
                remoteData = svc.GetContractItemList_0907a(remoteHeader, remoteData);

                // 결과코드검사
                if (!remoteData.ResultCD.Equals("0000"))
                {
                    throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
                }

                // 결과 셋트
                schChoiceAdModel.ScheduleDataSet = remoteData.ScheduleDataSet.Copy();
                schChoiceAdModel.ResultCnt = remoteData.ResultCnt;
                schChoiceAdModel.ResultCD = remoteData.ResultCD;

                _log.Debug("-----------------------------------------");
                _log.Debug("광고파일목록조회 End");
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
        /// 지정광고 편성추가
        /// </summary>
        /// <param name="schChoiceAdModel"></param>
        /// <returns></returns>
        public void SetSchChoiceAdAdd(SchChoiceAdModel schChoiceAdModel)
        {
            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug("지정광고 편성추가 Start");
                _log.Debug("-----------------------------------------");

                //입력데이터의 Validation 검사
                if (schChoiceAdModel.ItemNo.Length < 1)
                {
                    throw new FrameException("광고가 선택되지 않았습니다.");
                }

                // 웹서비스 인스턴스 생성
                SchChoiceAdServicePloxy.SchChoiceAdService svc = new SchChoiceAdServicePloxy.SchChoiceAdService();

                // 웹서비스 URL동적 생성
                svc.Url = _WebServiceUrl;

                // 리모트 모델 생성
                SchChoiceAdServicePloxy.HeaderModel remoteHeader = new AdManagerClient.SchChoiceAdServicePloxy.HeaderModel();
                SchChoiceAdServicePloxy.SchChoiceAdModel remoteData = new AdManagerClient.SchChoiceAdServicePloxy.SchChoiceAdModel();

                // 헤더정보 셋트
                remoteHeader.ClientKey = Header.ClientKey;
                remoteHeader.UserID = Header.UserID;
                remoteHeader.UserLevel = Header.UserLevel;
                remoteHeader.UserClass = Header.UserClass;

                // 호출정보 셋트

                remoteData.MediaCode = schChoiceAdModel.MediaCode;
                remoteData.ItemNo = schChoiceAdModel.ItemNo;
                remoteData.ItemName = schChoiceAdModel.ItemName;

                // 웹서비스 호출 타임아웃설정
                svc.Timeout = FrameSystem.m_SystemTimeout;
                // 웹서비스 메소드 호출
                // 지정메뉴광고 생성
                remoteData = svc.SetSchChoiceMenuCreate(remoteHeader, remoteData);

                // 결과코드검사
                if (!remoteData.ResultCD.Equals("0000"))
                {
                    throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
                }

                // 웹서비스 호출 타임아웃설정
                //svc.Timeout = FrameSystem.m_SystemTimeout;
                //remoteData = svc.SetSchChoiceChannelCreate(remoteHeader, remoteData);

                //// 결과코드검사
                //if(!remoteData.ResultCD.Equals("0000"))
                //{
                //    throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
                //}

                // 웹서비스 호출 타임아웃설정
                //svc.Timeout = FrameSystem.m_SystemTimeout;
                //remoteData = svc.GetSchChoiceLastItemNo(remoteHeader, remoteData);

                //// 결과코드검사
                //if(!remoteData.ResultCD.Equals("0000"))
                //{
                //    throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
                //}

                // 결과 셋트
                schChoiceAdModel.ItemNo = remoteData.ItemNo;
                schChoiceAdModel.ResultCnt = remoteData.ResultCnt;
                schChoiceAdModel.ResultCD = remoteData.ResultCD;

                _log.Debug("-----------------------------------------");
                _log.Debug("지정광고 편성추가 End");
                _log.Debug("-----------------------------------------");
            }
            catch (FrameException fe)
            {
                _log.Warning("-----------------------------------------");
                _log.Warning(this.ToString() + ":SetSchChoiceMenuAdd():" + fe.ErrCode + ":" + fe.ResultMsg);
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

        #region 편성복사 [E_01]
        /// <summary>
        /// 지정광고 편성복사 
        /// </summary>
        /// <param name="schChoiceAdModel"></param>
        /// <returns></returns>
        public void SetSchChoiceAdCopy(SchChoiceAdModel schChoiceAdModel)
        {
            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug("지정광고 편성 복사 Start");
                _log.Debug("-----------------------------------------");

                //입력데이터의 Validation 검사
                if (schChoiceAdModel.ItemNo.Length < 1)
                {
                    throw new FrameException("광고가 선택되지 않았습니다.");
                }

                // 웹서비스 인스턴스 생성
                SchChoiceAdServicePloxy.SchChoiceAdService svc = new SchChoiceAdServicePloxy.SchChoiceAdService();

                // 웹서비스 URL동적 생성
                svc.Url = _WebServiceUrl;

                // 리모트 모델 생성
                SchChoiceAdServicePloxy.HeaderModel remoteHeader = new AdManagerClient.SchChoiceAdServicePloxy.HeaderModel();
                SchChoiceAdServicePloxy.SchChoiceAdModel remoteData = new AdManagerClient.SchChoiceAdServicePloxy.SchChoiceAdModel();

                // 헤더정보 셋트
                remoteHeader.ClientKey = Header.ClientKey;
                remoteHeader.UserID = Header.UserID;
                remoteHeader.UserLevel = Header.UserLevel;
                remoteHeader.UserClass = Header.UserClass;

                // 호출정보 셋트

                remoteData.ItemNo = schChoiceAdModel.ItemNo;
                remoteData.ItemNoCopy = schChoiceAdModel.ItemNoCopy;
                remoteData.AdType = schChoiceAdModel.AdType;
                remoteData.MediaCode = schChoiceAdModel.MediaCode;
                remoteData.CheckSchResult = schChoiceAdModel.CheckSchResult;

                // 웹서비스 호출 타임아웃설정
                svc.Timeout = FrameSystem.m_SystemTimeout;

                // 웹서비스 메소드 호출
                remoteData = svc.SetSchChoiceAdCopy(remoteHeader, remoteData);

                // 결과코드검사
                if (!remoteData.ResultCD.Equals("0000"))
                {
                    throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
                }

                // 결과 셋트
                //schChoiceAdModel.ItemNo = remoteData.ItemNo;
                schChoiceAdModel.ResultCnt = remoteData.ResultCnt;
                schChoiceAdModel.ResultCD = remoteData.ResultCD;

                _log.Debug("-----------------------------------------");
                _log.Debug("지정광고 편성 복사 End");
                _log.Debug("-----------------------------------------");
            }
            catch (FrameException fe)
            {
                _log.Warning("-----------------------------------------");
                _log.Warning(this.ToString() + ":SetSchChoiceAdCopy():" + fe.ErrCode + ":" + fe.ResultMsg);
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

        #region 편성내역 조회
        /// <summary>
        /// 편성을 넣을 광고 편성내역 조회
        /// </summary>
        /// <param name="schChoiceAdModel"></param>
        /// <returns></returns>
        public void CheckSchChoice(SchChoiceAdModel schChoiceAdModel)
        {
            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug("광고 기존 편성 조회 Start");
                _log.Debug("-----------------------------------------");

                // 웹서비스 인스턴스 생성
                SchChoiceAdServicePloxy.SchChoiceAdService svc = new SchChoiceAdServicePloxy.SchChoiceAdService();

                // 웹서비스 URL동적 생성
                svc.Url = _WebServiceUrl;

                // 리모트 모델 생성
                SchChoiceAdServicePloxy.HeaderModel remoteHeader = new AdManagerClient.SchChoiceAdServicePloxy.HeaderModel();
                SchChoiceAdServicePloxy.SchChoiceAdModel remoteData = new AdManagerClient.SchChoiceAdServicePloxy.SchChoiceAdModel();

                // 헤더정보 셋트
                remoteHeader.ClientKey = Header.ClientKey;
                remoteHeader.UserID = Header.UserID;
                remoteHeader.UserLevel = Header.UserLevel;
                remoteHeader.UserClass = Header.UserClass;

                // 호출정보 셋트
                remoteData.ItemNo = schChoiceAdModel.ItemNo;

                // 웹서비스 호출 타임아웃설정
                svc.Timeout = FrameSystem.m_SystemTimeout;
                // 웹서비스 메소드 호출
                remoteData = svc.CheckSchChoice(remoteHeader, remoteData);

                // 결과코드검사
                if (!remoteData.ResultCD.Equals("0000"))
                {
                    throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
                }

                // 결과 셋트
                schChoiceAdModel.ResultCnt = remoteData.ResultCnt;
                schChoiceAdModel.ResultCD = remoteData.ResultCD;
                schChoiceAdModel.CheckMenu = remoteData.CheckMenu;
                schChoiceAdModel.CheckChannel = remoteData.CheckChannel;
                schChoiceAdModel.CheckSeries = remoteData.CheckSeries;
                schChoiceAdModel.CheckDetail = remoteData.CheckDetail;

                _log.Debug("-----------------------------------------");
                _log.Debug("광고 기존 편성 조회 End");
                _log.Debug("-----------------------------------------");
            }
            catch (FrameException fe)
            {
                _log.Warning("-----------------------------------------");
                _log.Warning(this.ToString() + ":CheckSchChoice():" + fe.ErrCode + ":" + fe.ResultMsg);
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


        /// <summary>
        /// 지정광고편성삭제
        /// </summary>
        /// <param name="schChoiceAdModel"></param>
        /// <returns></returns>
        public void SetSchChoiceAdDelete(SchChoiceAdModel schChoiceAdModel)
        {
            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug("지정광고 편성삭제 Start");
                _log.Debug("-----------------------------------------");

                //입력데이터의 Validation 검사
                if (schChoiceAdModel.ItemNo.Length < 1)
                {
                    throw new FrameException("광고내역이 선택되지 않았습니다.");
                }

                // 웹서비스 인스턴스 생성
                SchChoiceAdServicePloxy.SchChoiceAdService svc = new SchChoiceAdServicePloxy.SchChoiceAdService();

                // 웹서비스 URL동적 생성
                svc.Url = _WebServiceUrl;

                // 리모트 모델 생성
                SchChoiceAdServicePloxy.HeaderModel remoteHeader = new AdManagerClient.SchChoiceAdServicePloxy.HeaderModel();
                SchChoiceAdServicePloxy.SchChoiceAdModel remoteData = new AdManagerClient.SchChoiceAdServicePloxy.SchChoiceAdModel();

                // 헤더정보 셋트
                remoteHeader.ClientKey = Header.ClientKey;
                remoteHeader.UserID = Header.UserID;
                remoteHeader.UserLevel = Header.UserLevel;
                remoteHeader.UserClass = Header.UserClass;

                // 호출정보 셋트
                remoteData.MediaCode = schChoiceAdModel.MediaCode;
                remoteData.ItemNo = schChoiceAdModel.ItemNo;
                remoteData.ItemName = schChoiceAdModel.ItemName;

                // 웹서비스 호출 타임아웃설정
                svc.Timeout = FrameSystem.m_SystemTimeout;
                // 웹서비스 메소드 호출
                // 지정메뉴광고 생성
                remoteData = svc.SetSchChoiceMenuDelete(remoteHeader, remoteData);

                // 결과코드검사
                if (!remoteData.ResultCD.Equals("0000"))
                {
                    throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
                }

                //// 호출정보 셋트
                //remoteData.MediaCode       =  schChoiceAdModel.MediaCode;
                //remoteData.ItemNo          =  schChoiceAdModel.ItemNo;
                //remoteData.ItemName        =  schChoiceAdModel.ItemName;

                //// 웹서비스 호출 타임아웃설정
                //svc.Timeout = FrameSystem.m_SystemTimeout;

                //// 지정채널광고 생성
                //remoteData = svc.SetSchChoiceChannelDelete(remoteHeader, remoteData);

                //// 결과코드검사
                //if(!remoteData.ResultCD.Equals("0000"))
                //{
                //    throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
                //}

                //// 웹서비스 호출 타임아웃설정
                //svc.Timeout = FrameSystem.m_SystemTimeout;
                //// 삭제후 마지막 광고 조회
                //remoteData = svc.GetSchChoiceLastItemNoDelete(remoteHeader, remoteData);

                //// 결과코드검사
                //if(!remoteData.ResultCD.Equals("0000"))
                //{
                //    throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
                //}


                // 결과 셋트
                schChoiceAdModel.ItemNo = remoteData.ItemNo;
                schChoiceAdModel.ResultCnt = remoteData.ResultCnt;
                schChoiceAdModel.ResultCD = remoteData.ResultCD;

                _log.Debug("-----------------------------------------");
                _log.Debug("지정광고 편성삭제 End");
                _log.Debug("-----------------------------------------");
            }
            catch (FrameException fe)
            {
                _log.Warning("-----------------------------------------");
                _log.Warning(this.ToString() + ":SetSchChoiceAdDelete():" + fe.ErrCode + ":" + fe.ResultMsg);
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
        /// 지정광고편성엑셀삭제
        /// </summary>
        /// <param name="schChoiceAdModel"></param>
        /// <returns></returns>
        public void SetSchChoiceAdDelete_To(SchChoiceAdModel schChoiceAdModel)
        {
            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug("지정광고 편성엑셀삭제 Start");
                _log.Debug("-----------------------------------------");

                //입력데이터의 Validation 검사
                //				if(schChoiceAdModel.ItemNo.Length < 1) 
                //				{
                //					throw new FrameException("광고내역이 선택되지 않았습니다.");
                //				}

                // 웹서비스 인스턴스 생성
                SchChoiceAdServicePloxy.SchChoiceAdService svc = new SchChoiceAdServicePloxy.SchChoiceAdService();

                // 웹서비스 URL동적 생성
                svc.Url = _WebServiceUrl;

                // 리모트 모델 생성
                SchChoiceAdServicePloxy.HeaderModel remoteHeader = new AdManagerClient.SchChoiceAdServicePloxy.HeaderModel();
                SchChoiceAdServicePloxy.SchChoiceAdModel remoteData = new AdManagerClient.SchChoiceAdServicePloxy.SchChoiceAdModel();

                // 헤더정보 셋트
                remoteHeader.ClientKey = Header.ClientKey;
                remoteHeader.UserID = Header.UserID;
                remoteHeader.UserLevel = Header.UserLevel;
                remoteHeader.UserClass = Header.UserClass;

                // 호출정보 셋트

                remoteData.MediaCode = schChoiceAdModel.MediaCode;
                remoteData.ItemNo = schChoiceAdModel.ItemNo;

                // 웹서비스 호출 타임아웃설정
                svc.Timeout = FrameSystem.m_SystemTimeout;
                // 웹서비스 메소드 호출
                // 지정메뉴광고 생성
                remoteData = svc.SetSchChoiceMenuDelete_To(remoteHeader, remoteData);

                // 결과코드검사
                if (!remoteData.ResultCD.Equals("0000"))
                {
                    throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
                }

                // 웹서비스 호출 타임아웃설정
                svc.Timeout = FrameSystem.m_SystemTimeout;
                // 삭제후 마지막 광고 조회
                remoteData = svc.GetSchChoiceLastItemNoDelete(remoteHeader, remoteData);

                // 결과코드검사
                if (!remoteData.ResultCD.Equals("0000"))
                {
                    throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
                }


                // 결과 셋트
                schChoiceAdModel.ItemNo = remoteData.ItemNo;
                schChoiceAdModel.ResultCnt = remoteData.ResultCnt;
                schChoiceAdModel.ResultCD = remoteData.ResultCD;

                _log.Debug("-----------------------------------------");
                _log.Debug("지정광고 편성엑셀삭제 End");
                _log.Debug("-----------------------------------------");
            }
            catch (FrameException fe)
            {
                _log.Warning("-----------------------------------------");
                _log.Warning(this.ToString() + ":SetSchChoiceAdDelete():" + fe.ErrCode + ":" + fe.ResultMsg);
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
        /// 메뉴광고 조회(엑셀파일을 셀렉트하여 DB에 없는 데이터만 인서트 하기위해...메뉴 테이블 셀렉트)
        /// </summary>
        /// <param name="schHomeAdModel"></param>
        /// <returns></returns>
        public void SetSchChoiceList(SchChoiceAdModel schChoiceAdModel)
        {
            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug("지정메뉴광고 상세편성 조회 Start");
                _log.Debug("-----------------------------------------");

                // 웹서비스 인스턴스 생성
                SchChoiceAdServicePloxy.SchChoiceAdService svc = new SchChoiceAdServicePloxy.SchChoiceAdService();

                // 웹서비스 URL동적 생성
                svc.Url = _WebServiceUrl;

                // 리모트 모델 생성
                SchChoiceAdServicePloxy.HeaderModel remoteHeader = new AdManagerClient.SchChoiceAdServicePloxy.HeaderModel();
                SchChoiceAdServicePloxy.SchChoiceAdModel remoteData = new AdManagerClient.SchChoiceAdServicePloxy.SchChoiceAdModel();

                // 헤더정보 셋트
                remoteHeader.ClientKey = Header.ClientKey;
                remoteHeader.UserID = Header.UserID;
                remoteHeader.UserLevel = Header.UserLevel;
                remoteHeader.UserClass = Header.UserClass;

                // 호출정보 셋트
                remoteData.ItemNo = schChoiceAdModel.ItemNo;
                remoteData.MediaCode = schChoiceAdModel.MediaCode;
                remoteData.GenreCode = schChoiceAdModel.GenreCode;
                remoteData.ScheduleOrder = schChoiceAdModel.ScheduleOrder;
                remoteData.AdType = schChoiceAdModel.AdType;

                // 웹서비스 호출 타임아웃설정
                svc.Timeout = FrameSystem.m_SystemTimeout;
                // 웹서비스 메소드 호출
                remoteData = svc.SetSchChoiceSearch(remoteHeader, remoteData);

                // 결과코드검사
                if (!remoteData.ResultCD.Equals("0000"))
                {
                    throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
                }

                // 결과 셋트
                schChoiceAdModel.ScheduleDataSet = remoteData.ScheduleDataSet.Copy();
                schChoiceAdModel.ResultCnt = remoteData.ResultCnt;
                schChoiceAdModel.ResultCD = remoteData.ResultCD;

                _log.Debug("-----------------------------------------");
                _log.Debug("지정메뉴광고 상세편성 조회 End");
                _log.Debug("-----------------------------------------");
            }
            catch (FrameException fe)
            {
                _log.Warning("-----------------------------------------");
                _log.Warning(this.ToString() + ":SetSchChoiceList():" + fe.ErrCode + ":" + fe.ResultMsg);
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
        /// 채널광고 조회(엑셀파일을 셀렉트하여 DB에 없는 데이터만 인서트 하기위해...채널 테이블 셀렉트)
        /// </summary>
        /// <param name="schHomeAdModel"></param>
        /// <returns></returns>
        public void SetSchChoiceChannelList(SchChoiceAdModel schChoiceAdModel)
        {
            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug("지정채널광고 상세편성 조회 Start");
                _log.Debug("-----------------------------------------");

                // 웹서비스 인스턴스 생성
                SchChoiceAdServicePloxy.SchChoiceAdService svc = new SchChoiceAdServicePloxy.SchChoiceAdService();

                // 웹서비스 URL동적 생성
                svc.Url = _WebServiceUrl;

                // 리모트 모델 생성
                SchChoiceAdServicePloxy.HeaderModel remoteHeader = new AdManagerClient.SchChoiceAdServicePloxy.HeaderModel();
                SchChoiceAdServicePloxy.SchChoiceAdModel remoteData = new AdManagerClient.SchChoiceAdServicePloxy.SchChoiceAdModel();

                // 헤더정보 셋트
                remoteHeader.ClientKey = Header.ClientKey;
                remoteHeader.UserID = Header.UserID;
                remoteHeader.UserLevel = Header.UserLevel;
                remoteHeader.UserClass = Header.UserClass;

                // 호출정보 셋트
                remoteData.ItemNo = schChoiceAdModel.ItemNo;
                remoteData.MediaCode = schChoiceAdModel.MediaCode;
                remoteData.ChannelNo = schChoiceAdModel.ChannelNo;
                remoteData.ScheduleOrder = schChoiceAdModel.ScheduleOrder;
                remoteData.AdType = schChoiceAdModel.AdType;

                // 웹서비스 호출 타임아웃설정
                svc.Timeout = FrameSystem.m_SystemTimeout;
                // 웹서비스 메소드 호출
                remoteData = svc.SetSchChoiceChannelSearch(remoteHeader, remoteData);

                // 결과코드검사
                if (!remoteData.ResultCD.Equals("0000"))
                {
                    throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
                }

                // 결과 셋트
                schChoiceAdModel.ScheduleDataSet = remoteData.ScheduleDataSet.Copy();
                schChoiceAdModel.ResultCnt = remoteData.ResultCnt;
                schChoiceAdModel.ResultCD = remoteData.ResultCD;

                _log.Debug("-----------------------------------------");
                _log.Debug("지정채널광고 상세편성 조회 End");
                _log.Debug("-----------------------------------------");
            }
            catch (FrameException fe)
            {
                _log.Warning("-----------------------------------------");
                _log.Warning(this.ToString() + ":SetSchChoiceChannelList():" + fe.ErrCode + ":" + fe.ResultMsg);
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
        /// 지정메뉴광고 상세편성 엑셀저장
        /// </summary>
        /// <param name="schChoiceAdModel"></param>
        public void SetSchChoiceMenuDetailAdd_To(SchChoiceAdModel schChoiceAdModel)
        {

            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug("지정메뉴광고 상세편성 엑셀저장 Start");
                _log.Debug("-----------------------------------------");

                // 웹서비스 인스턴스 생성
                SchChoiceAdServicePloxy.SchChoiceAdService svc = new SchChoiceAdServicePloxy.SchChoiceAdService();

                // 웹서비스 URL동적 생성
                svc.Url = _WebServiceUrl;

                // 리모트 모델 생성
                SchChoiceAdServicePloxy.HeaderModel remoteHeader = new AdManagerClient.SchChoiceAdServicePloxy.HeaderModel();
                SchChoiceAdServicePloxy.SchChoiceAdModel remoteData = new AdManagerClient.SchChoiceAdServicePloxy.SchChoiceAdModel();

                // 헤더정보 셋트
                remoteHeader.ClientKey = Header.ClientKey;
                remoteHeader.UserID = Header.UserID;
                remoteHeader.UserLevel = Header.UserLevel;
                remoteHeader.UserClass = Header.UserClass;

                // 호출정보 셋트
                remoteData.ItemNo = schChoiceAdModel.ItemNo;
                remoteData.MediaCode = schChoiceAdModel.MediaCode;
                remoteData.GenreCode = schChoiceAdModel.GenreCode;
                remoteData.ScheduleOrder = schChoiceAdModel.ScheduleOrder;
                remoteData.AdType = schChoiceAdModel.AdType;

                // 웹서비스 호출 타임아웃설정
                svc.Timeout = FrameSystem.m_SystemTimeout;
                // 웹서비스 메소드 호출
                remoteData = svc.SetSchChoiceMenuDetailCreate_To(remoteHeader, remoteData);

                // 결과코드검사
                if (!remoteData.ResultCD.Equals("0000"))
                {
                    throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
                }

                // 결과 셋트
                schChoiceAdModel.ResultCnt = remoteData.ResultCnt;
                schChoiceAdModel.ResultCD = remoteData.ResultCD;

                _log.Debug("-----------------------------------------");
                _log.Debug("지정메뉴광고 상세편성 엑셀저장 End");
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

        /// <summary>
        /// 지정메뉴광고 상세편성 조회
        /// </summary>
        /// <param name="schChoiceAdModel"></param>
        public void GetSchChoiceMenuDetailList(SchChoiceAdModel schChoiceAdModel)
        {

            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug("지정메뉴광고 상세편성 조회 Start");
                _log.Debug("-----------------------------------------");

                // 웹서비스 인스턴스 생성
                SchChoiceAdServicePloxy.SchChoiceAdService svc = new SchChoiceAdServicePloxy.SchChoiceAdService();

                // 웹서비스 URL동적 생성
                svc.Url = _WebServiceUrl;

                // 리모트 모델 생성
                SchChoiceAdServicePloxy.HeaderModel remoteHeader = new AdManagerClient.SchChoiceAdServicePloxy.HeaderModel();
                SchChoiceAdServicePloxy.SchChoiceAdModel remoteData = new AdManagerClient.SchChoiceAdServicePloxy.SchChoiceAdModel();

                // 헤더정보 셋트
                remoteHeader.ClientKey = Header.ClientKey;
                remoteHeader.UserID = Header.UserID;
                remoteHeader.UserLevel = Header.UserLevel;
                remoteHeader.UserClass = Header.UserClass;

                // 호출정보 셋트
                remoteData.ItemNo = schChoiceAdModel.ItemNo;

                // 웹서비스 호출 타임아웃설정
                svc.Timeout = FrameSystem.m_SystemTimeout;
                // 웹서비스 메소드 호출
                remoteData = svc.GetSchChoiceMenuDetailList(remoteHeader, remoteData);

                // 결과코드검사
                if (!remoteData.ResultCD.Equals("0000"))
                {
                    throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
                }

                // 결과 셋트
                schChoiceAdModel.ScheduleDataSet = remoteData.ScheduleDataSet.Copy();
                schChoiceAdModel.ResultCnt = remoteData.ResultCnt;
                schChoiceAdModel.ResultCD = remoteData.ResultCD;

                _log.Debug("-----------------------------------------");
                _log.Debug("지정메뉴광고 상세편성 조회 End");
                _log.Debug("-----------------------------------------");
            }
            catch (FrameException fe)
            {
                _log.Warning("-----------------------------------------");
                _log.Warning(this.ToString() + ":GetSchChoiceMenuDetailList():" + fe.ErrCode + ":" + fe.ResultMsg);
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
        /// 지정채널광고 상세편성 조회
        /// </summary>
        /// <param name="schChoiceAdModel"></param>
        public void GetSchChoiceChannelDetailList(SchChoiceAdModel schChoiceAdModel)
        {

            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug("지정채널광고 상세편성 조회 Start");
                _log.Debug("-----------------------------------------");

                // 웹서비스 인스턴스 생성
                SchChoiceAdServicePloxy.SchChoiceAdService svc = new SchChoiceAdServicePloxy.SchChoiceAdService();

                // 웹서비스 URL동적 생성
                svc.Url = _WebServiceUrl;

                // 리모트 모델 생성
                SchChoiceAdServicePloxy.HeaderModel remoteHeader = new AdManagerClient.SchChoiceAdServicePloxy.HeaderModel();
                SchChoiceAdServicePloxy.SchChoiceAdModel remoteData = new AdManagerClient.SchChoiceAdServicePloxy.SchChoiceAdModel();

                // 헤더정보 셋트
                remoteHeader.ClientKey = Header.ClientKey;
                remoteHeader.UserID = Header.UserID;
                remoteHeader.UserLevel = Header.UserLevel;
                remoteHeader.UserClass = Header.UserClass;

                // 호출정보 셋트
                remoteData.ItemNo = schChoiceAdModel.ItemNo;

                // 웹서비스 호출 타임아웃설정
                svc.Timeout = FrameSystem.m_SystemTimeout;
                // 웹서비스 메소드 호출
                remoteData = svc.GetSchChoiceChannelDetailList(remoteHeader, remoteData);

                // 결과코드검사
                if (!remoteData.ResultCD.Equals("0000"))
                {
                    throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
                }

                // 결과 셋트
                schChoiceAdModel.ScheduleDataSet = remoteData.ScheduleDataSet.Copy();
                schChoiceAdModel.ResultCnt = remoteData.ResultCnt;
                schChoiceAdModel.ResultCD = remoteData.ResultCD;

                _log.Debug("-----------------------------------------");
                _log.Debug("지정채널광고 상세편성 조회 End");
                _log.Debug("-----------------------------------------");
            }
            catch (FrameException fe)
            {
                _log.Warning("-----------------------------------------");
                _log.Warning(this.ToString() + ":GetSchChoiceMenuDetailList():" + fe.ErrCode + ":" + fe.ResultMsg);
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
        /// 지정메뉴광고 상세편성 저장
        /// </summary>
        /// <param name="schChoiceAdModel"></param>
        public void SetSchChoiceMenuDetailAdd(SchChoiceAdModel schChoiceAdModel)
        {

            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug("지정메뉴광고 상세편성 저장 Start");
                _log.Debug("-----------------------------------------");

                // 웹서비스 인스턴스 생성
                SchChoiceAdServicePloxy.SchChoiceAdService svc = new SchChoiceAdServicePloxy.SchChoiceAdService();

                // 웹서비스 URL동적 생성
                svc.Url = _WebServiceUrl;

                // 리모트 모델 생성
                SchChoiceAdServicePloxy.HeaderModel remoteHeader = new AdManagerClient.SchChoiceAdServicePloxy.HeaderModel();
                SchChoiceAdServicePloxy.SchChoiceAdModel remoteData = new AdManagerClient.SchChoiceAdServicePloxy.SchChoiceAdModel();

                // 헤더정보 셋트
                remoteHeader.ClientKey = Header.ClientKey;
                remoteHeader.UserID = Header.UserID;
                remoteHeader.UserLevel = Header.UserLevel;
                remoteHeader.UserClass = Header.UserClass;

                // 호출정보 셋트
                remoteData.ItemNo = schChoiceAdModel.ItemNo;
                remoteData.MediaCode = schChoiceAdModel.MediaCode;
                remoteData.MediaName = schChoiceAdModel.MediaName;
                remoteData.GenreCode = schChoiceAdModel.GenreCode;
                remoteData.GenreName = schChoiceAdModel.GenreName;
                remoteData.CategoryCode = schChoiceAdModel.CategoryCode;
                remoteData.AdType = schChoiceAdModel.AdType;

                // 웹서비스 호출 타임아웃설정
                svc.Timeout = FrameSystem.m_SystemTimeout;
                remoteData = svc.SetSchChoiceMenuDetailCreate(remoteHeader, remoteData);

                // 결과코드검사
                if (!remoteData.ResultCD.Equals("0000"))
                {
                    throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
                }

                // 결과 셋트
                schChoiceAdModel.ResultCnt = remoteData.ResultCnt;
                schChoiceAdModel.ResultCD = remoteData.ResultCD;

                _log.Debug("-----------------------------------------");
                _log.Debug("지정메뉴광고 상세편성 저장 End");
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

        /// <summary>
        /// 재핑광고 상세편성 저장
        /// </summary>
        /// <param name="schChoiceAdModel"></param>
        public void SetSchChoiceRealChDetailAdd(SchChoiceAdModel schChoiceAdModel)
        {
            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug("지정메뉴광고 상세편성 저장 Start");
                _log.Debug("-----------------------------------------");

                // 웹서비스 인스턴스 생성
                SchChoiceAdServicePloxy.SchChoiceAdService svc = new SchChoiceAdServicePloxy.SchChoiceAdService();

                // 웹서비스 URL동적 생성
                svc.Url = _WebServiceUrl;

                // 리모트 모델 생성
                SchChoiceAdServicePloxy.HeaderModel remoteHeader = new AdManagerClient.SchChoiceAdServicePloxy.HeaderModel();
                SchChoiceAdServicePloxy.SchChoiceAdModel remoteData = new AdManagerClient.SchChoiceAdServicePloxy.SchChoiceAdModel();

                // 헤더정보 셋트
                remoteHeader.ClientKey = Header.ClientKey;
                remoteHeader.UserID = Header.UserID;
                remoteHeader.UserLevel = Header.UserLevel;
                remoteHeader.UserClass = Header.UserClass;

                // 호출정보 셋트
                remoteData.ItemNo = schChoiceAdModel.ItemNo;
                remoteData.MediaCode = schChoiceAdModel.MediaCode;
                remoteData.MediaName = schChoiceAdModel.MediaName;
                remoteData.GenreCode = schChoiceAdModel.GenreCode;
                remoteData.GenreName = schChoiceAdModel.GenreName;
                remoteData.CategoryCode = schChoiceAdModel.CategoryCode;
                remoteData.AdType = schChoiceAdModel.AdType;

                // 웹서비스 호출 타임아웃설정
                svc.Timeout = FrameSystem.m_SystemTimeout;
                remoteData = svc.SetSchChoiceRealChDetailCreate(remoteHeader, remoteData);

                // 결과코드검사
                if (!remoteData.ResultCD.Equals("0000"))
                {
                    throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
                }

                // 결과 셋트
                schChoiceAdModel.ResultCnt = remoteData.ResultCnt;
                schChoiceAdModel.ResultCD = remoteData.ResultCD;

                _log.Debug("-----------------------------------------");
                _log.Debug("지정메뉴광고 상세편성 저장 End");
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


        /// <summary>
        /// 지정채널광고 상세편성 저장
        /// </summary>
        /// <param name="schChoiceAdModel"></param>
        public void SetSchChoiceChannelDetailAdd(SchChoiceAdModel schChoiceAdModel)
        {

            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug("지정채널광고 상세편성 저장 Start");
                _log.Debug("-----------------------------------------");

                // 웹서비스 인스턴스 생성
                SchChoiceAdServicePloxy.SchChoiceAdService svc = new SchChoiceAdServicePloxy.SchChoiceAdService();

                // 웹서비스 URL동적 생성
                svc.Url = _WebServiceUrl;

                // 리모트 모델 생성
                SchChoiceAdServicePloxy.HeaderModel remoteHeader = new AdManagerClient.SchChoiceAdServicePloxy.HeaderModel();
                SchChoiceAdServicePloxy.SchChoiceAdModel remoteData = new AdManagerClient.SchChoiceAdServicePloxy.SchChoiceAdModel();

                // 헤더정보 셋트
                remoteHeader.ClientKey = Header.ClientKey;
                remoteHeader.UserID = Header.UserID;
                remoteHeader.UserLevel = Header.UserLevel;
                remoteHeader.UserClass = Header.UserClass;

                // 호출정보 셋트
                remoteData.ItemNo = schChoiceAdModel.ItemNo;
                remoteData.MediaCode = schChoiceAdModel.MediaCode;
                remoteData.MediaName = schChoiceAdModel.MediaName;
                remoteData.ChannelNo = schChoiceAdModel.ChannelNo;
                remoteData.Title = schChoiceAdModel.Title;
                remoteData.AdType = schChoiceAdModel.AdType;

                // 웹서비스 호출 타임아웃설정
                svc.Timeout = FrameSystem.m_SystemTimeout;
                // 웹서비스 메소드 호출
                remoteData = svc.SetSchChoiceChannelDetailCreate(remoteHeader, remoteData);

                // 결과코드검사
                if (!remoteData.ResultCD.Equals("0000"))
                {
                    throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
                }

                // 결과 셋트
                schChoiceAdModel.ResultCnt = remoteData.ResultCnt;
                schChoiceAdModel.ResultCD = remoteData.ResultCD;

                _log.Debug("-----------------------------------------");
                _log.Debug("지정채널광고 상세편성 저장 End");
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



        /// <summary>
        /// 회차편성 저장
        /// </summary>
        /// <param name="schChoiceAdModel"></param>
        public void SetSchChoiceSeriesDetailAdd(SchChoiceAdModel schChoiceAdModel)
        {
            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug("편성단위별 회차편성상세 저장 Start");
                _log.Debug("-----------------------------------------");

                // 웹서비스 인스턴스 생성
                SchChoiceAdServicePloxy.SchChoiceAdService svc = new SchChoiceAdServicePloxy.SchChoiceAdService();

                // 웹서비스 URL동적 생성
                svc.Url = _WebServiceUrl;

                // 리모트 모델 생성
                SchChoiceAdServicePloxy.HeaderModel remoteHeader = new AdManagerClient.SchChoiceAdServicePloxy.HeaderModel();
                SchChoiceAdServicePloxy.SchChoiceAdModel remoteData = new AdManagerClient.SchChoiceAdServicePloxy.SchChoiceAdModel();

                // 헤더정보 셋트
                remoteHeader.ClientKey = Header.ClientKey;
                remoteHeader.UserID = Header.UserID;
                remoteHeader.UserLevel = Header.UserLevel;
                remoteHeader.UserClass = Header.UserClass;

                // 호출정보 셋트
                remoteData.ItemNo = schChoiceAdModel.ItemNo;
                remoteData.MediaCode = schChoiceAdModel.MediaCode;
                remoteData.MediaName = schChoiceAdModel.MediaName;
                remoteData.ChannelNo = schChoiceAdModel.ChannelNo;
                remoteData.SeriesNo = schChoiceAdModel.SeriesNo;
                remoteData.Title = schChoiceAdModel.Title;
                remoteData.AdType = schChoiceAdModel.AdType;

                // 웹서비스 호출 타임아웃설정
                svc.Timeout = FrameSystem.m_SystemTimeout;
                // 웹서비스 메소드 호출
                remoteData = svc.SetSchChoiceSeriesDetailCreate(remoteHeader, remoteData);

                // 결과코드검사
                if (!remoteData.ResultCD.Equals("0000"))
                {
                    throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
                }

                // 결과 셋트
                schChoiceAdModel.ResultCnt = remoteData.ResultCnt;
                schChoiceAdModel.ResultCD = remoteData.ResultCD;

                _log.Debug("-----------------------------------------");
                _log.Debug("지정채널광고 상세편성 저장 End");
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


        /// <summary>
        /// 지정메뉴광고 상세편성 삭제
        /// </summary>
        /// <param name="schChoiceAdModel"></param>
        public void SetSchChoiceMenuDetailDelete(SchChoiceAdModel schChoiceAdModel)
        {

            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug("지정메뉴광고 상세편성 삭제 Start");
                _log.Debug("-----------------------------------------");

                // 웹서비스 인스턴스 생성
                SchChoiceAdServicePloxy.SchChoiceAdService svc = new SchChoiceAdServicePloxy.SchChoiceAdService();

                // 웹서비스 URL동적 생성
                svc.Url = _WebServiceUrl;

                // 리모트 모델 생성
                SchChoiceAdServicePloxy.HeaderModel remoteHeader = new AdManagerClient.SchChoiceAdServicePloxy.HeaderModel();
                SchChoiceAdServicePloxy.SchChoiceAdModel remoteData = new AdManagerClient.SchChoiceAdServicePloxy.SchChoiceAdModel();

                // 헤더정보 셋트
                remoteHeader.ClientKey = Header.ClientKey;
                remoteHeader.UserID = Header.UserID;
                remoteHeader.UserLevel = Header.UserLevel;
                remoteHeader.UserClass = Header.UserClass;

                // 호출정보 셋트
                remoteData.ItemNo = schChoiceAdModel.ItemNo;
                remoteData.MediaCode = schChoiceAdModel.MediaCode;
                remoteData.MediaName = schChoiceAdModel.MediaName;
                remoteData.GenreCode = schChoiceAdModel.GenreCode;
                remoteData.GenreName = schChoiceAdModel.GenreName;
                remoteData.CategoryCode = schChoiceAdModel.CategoryCode;

                // 웹서비스 호출 타임아웃설정
                svc.Timeout = FrameSystem.m_SystemTimeout;
                remoteData = svc.SetSchChoiceMenuDetailDelete(remoteHeader, remoteData);

                // 결과코드검사
                if (!remoteData.ResultCD.Equals("0000"))
                {
                    throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
                }

                // 결과 셋트
                schChoiceAdModel.ResultCnt = remoteData.ResultCnt;
                schChoiceAdModel.ResultCD = remoteData.ResultCD;

                _log.Debug("-----------------------------------------");
                _log.Debug("지정메뉴광고 상세편성 삭제 End");
                _log.Debug("-----------------------------------------");
            }
            catch (FrameException fe)
            {
                _log.Warning("-----------------------------------------");
                _log.Warning(this.ToString() + ":SetSchChoiceMenuDetailDelete():" + fe.ErrCode + ":" + fe.ResultMsg);
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

        public void SetSchChoiceRealChDetailDelete(SchChoiceAdModel schChoiceAdModel)
        {
            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug("지정메뉴광고 상세편성 삭제 Start");
                _log.Debug("-----------------------------------------");

                // 웹서비스 인스턴스 생성
                SchChoiceAdServicePloxy.SchChoiceAdService svc = new SchChoiceAdServicePloxy.SchChoiceAdService();

                // 웹서비스 URL동적 생성
                svc.Url = _WebServiceUrl;

                // 리모트 모델 생성
                SchChoiceAdServicePloxy.HeaderModel remoteHeader = new AdManagerClient.SchChoiceAdServicePloxy.HeaderModel();
                SchChoiceAdServicePloxy.SchChoiceAdModel remoteData = new AdManagerClient.SchChoiceAdServicePloxy.SchChoiceAdModel();

                // 헤더정보 셋트
                remoteHeader.ClientKey = Header.ClientKey;
                remoteHeader.UserID = Header.UserID;
                remoteHeader.UserLevel = Header.UserLevel;
                remoteHeader.UserClass = Header.UserClass;

                // 호출정보 셋트
                remoteData.ItemNo = schChoiceAdModel.ItemNo;
                remoteData.GenreCode = schChoiceAdModel.GenreCode;

                // 웹서비스 호출 타임아웃설정
                svc.Timeout = FrameSystem.m_SystemTimeout;
                remoteData = svc.SetSchChoiceRealChDetailDelete(remoteHeader, remoteData);

                // 결과코드검사
                if (!remoteData.ResultCD.Equals("0000"))
                {
                    throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
                }

                // 결과 셋트
                schChoiceAdModel.ResultCnt = remoteData.ResultCnt;
                schChoiceAdModel.ResultCD = remoteData.ResultCD;

                _log.Debug("-----------------------------------------");
                _log.Debug("지정메뉴광고 상세편성 삭제 End");
                _log.Debug("-----------------------------------------");
            }
            catch (FrameException fe)
            {
                _log.Warning("-----------------------------------------");
                _log.Warning(this.ToString() + ":SetSchChoiceMenuDetailDelete():" + fe.ErrCode + ":" + fe.ResultMsg);
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
        /// 채널광고편성엑셀삭제
        /// </summary>
        /// <param name="schChoiceAdModel"></param>
        /// <returns></returns>
        public void SetSchChannelAdDelete_To(SchChoiceAdModel schChoiceAdModel)
        {
            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug("채널광고 편성엑셀삭제 Start");
                _log.Debug("-----------------------------------------");

                //입력데이터의 Validation 검사
                //				if(schChoiceAdModel.ItemNo.Length < 1) 
                //				{
                //					throw new FrameException("광고내역이 선택되지 않았습니다.");
                //				}

                // 웹서비스 인스턴스 생성
                SchChoiceAdServicePloxy.SchChoiceAdService svc = new SchChoiceAdServicePloxy.SchChoiceAdService();

                // 웹서비스 URL동적 생성
                svc.Url = _WebServiceUrl;

                // 리모트 모델 생성
                SchChoiceAdServicePloxy.HeaderModel remoteHeader = new AdManagerClient.SchChoiceAdServicePloxy.HeaderModel();
                SchChoiceAdServicePloxy.SchChoiceAdModel remoteData = new AdManagerClient.SchChoiceAdServicePloxy.SchChoiceAdModel();

                // 헤더정보 셋트
                remoteHeader.ClientKey = Header.ClientKey;
                remoteHeader.UserID = Header.UserID;
                remoteHeader.UserLevel = Header.UserLevel;
                remoteHeader.UserClass = Header.UserClass;

                // 호출정보 셋트

                remoteData.MediaCode = schChoiceAdModel.MediaCode;
                remoteData.ItemNo = schChoiceAdModel.ItemNo;

                // 웹서비스 호출 타임아웃설정
                svc.Timeout = FrameSystem.m_SystemTimeout;
                // 웹서비스 메소드 호출
                // 지정메뉴광고 생성
                remoteData = svc.SetSchChoiceChannelDelete_To(remoteHeader, remoteData);

                // 결과코드검사
                if (!remoteData.ResultCD.Equals("0000"))
                {
                    throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
                }

                // 웹서비스 호출 타임아웃설정
                svc.Timeout = FrameSystem.m_SystemTimeout;
                // 삭제후 마지막 광고 조회
                remoteData = svc.GetSchChoiceLastItemNoDelete(remoteHeader, remoteData);

                // 결과코드검사
                if (!remoteData.ResultCD.Equals("0000"))
                {
                    throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
                }


                // 결과 셋트
                schChoiceAdModel.ItemNo = remoteData.ItemNo;
                schChoiceAdModel.ResultCnt = remoteData.ResultCnt;
                schChoiceAdModel.ResultCD = remoteData.ResultCD;

                _log.Debug("-----------------------------------------");
                _log.Debug("채널광고 편성엑셀삭제 End");
                _log.Debug("-----------------------------------------");
            }
            catch (FrameException fe)
            {
                _log.Warning("-----------------------------------------");
                _log.Warning(this.ToString() + ":SetSchChoiceAdDelete():" + fe.ErrCode + ":" + fe.ResultMsg);
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
        /// 지정채널광고 상세편성 엑셀저장
        /// </summary>
        /// <param name="schChoiceAdModel"></param>
        public void SetSchChoiceChannelDetailAdd_To(SchChoiceAdModel schChoiceAdModel)
        {

            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug("지정채널광고 상세편성 엑셀저장 Start");
                _log.Debug("-----------------------------------------");

                // 웹서비스 인스턴스 생성
                SchChoiceAdServicePloxy.SchChoiceAdService svc = new SchChoiceAdServicePloxy.SchChoiceAdService();

                // 웹서비스 URL동적 생성
                svc.Url = _WebServiceUrl;

                // 리모트 모델 생성
                SchChoiceAdServicePloxy.HeaderModel remoteHeader = new AdManagerClient.SchChoiceAdServicePloxy.HeaderModel();
                SchChoiceAdServicePloxy.SchChoiceAdModel remoteData = new AdManagerClient.SchChoiceAdServicePloxy.SchChoiceAdModel();

                // 헤더정보 셋트
                remoteHeader.ClientKey = Header.ClientKey;
                remoteHeader.UserID = Header.UserID;
                remoteHeader.UserLevel = Header.UserLevel;
                remoteHeader.UserClass = Header.UserClass;

                // 호출정보 셋트
                remoteData.ItemNo = schChoiceAdModel.ItemNo;
                remoteData.MediaCode = schChoiceAdModel.MediaCode;
                remoteData.ChannelNo = schChoiceAdModel.ChannelNo;
                remoteData.ScheduleOrder = schChoiceAdModel.ScheduleOrder;
                remoteData.AdType = schChoiceAdModel.AdType;

                // 웹서비스 호출 타임아웃설정
                svc.Timeout = FrameSystem.m_SystemTimeout;
                // 웹서비스 메소드 호출
                remoteData = svc.SetSchChoiceChannelDetailCreate_To(remoteHeader, remoteData);

                // 결과코드검사
                if (!remoteData.ResultCD.Equals("0000"))
                {
                    throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
                }

                // 결과 셋트
                schChoiceAdModel.ResultCnt = remoteData.ResultCnt;
                schChoiceAdModel.ResultCD = remoteData.ResultCD;

                _log.Debug("-----------------------------------------");
                _log.Debug("지정채널광고 상세편성 엑셀저장 End");
                _log.Debug("-----------------------------------------");
            }
            catch (FrameException fe)
            {
                _log.Warning("-----------------------------------------");
                _log.Warning(this.ToString() + ":SetSchChoiceChannelDetailAdd_To():" + fe.ErrCode + ":" + fe.ResultMsg);
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
        /// 지정채널광고 상세편성 삭제
        /// </summary>
        /// <param name="schChoiceAdModel"></param>
        public void SetSchChoiceChannelDetailDelete(SchChoiceAdModel schChoiceAdModel)
        {
            try
            {
                // 웹서비스 인스턴스 생성
                SchChoiceAdServicePloxy.SchChoiceAdService svc = new SchChoiceAdServicePloxy.SchChoiceAdService();

                // 웹서비스 URL동적 생성
                svc.Url = _WebServiceUrl;

                // 리모트 모델 생성
                SchChoiceAdServicePloxy.HeaderModel remoteHeader = new AdManagerClient.SchChoiceAdServicePloxy.HeaderModel();
                SchChoiceAdServicePloxy.SchChoiceAdModel remoteData = new AdManagerClient.SchChoiceAdServicePloxy.SchChoiceAdModel();

                // 헤더정보 셋트
                remoteHeader.ClientKey = Header.ClientKey;
                remoteHeader.UserID = Header.UserID;
                remoteHeader.UserLevel = Header.UserLevel;
                remoteHeader.UserClass = Header.UserClass;

                // 호출정보 셋트
                remoteData.ItemNo = schChoiceAdModel.ItemNo;
                remoteData.MediaCode = schChoiceAdModel.MediaCode;
                remoteData.MediaName = schChoiceAdModel.MediaName;
                remoteData.ChannelNo = schChoiceAdModel.ChannelNo;
                remoteData.Title = schChoiceAdModel.Title;

                // 웹서비스 호출 타임아웃설정
                svc.Timeout = FrameSystem.m_SystemTimeout;
                // 웹서비스 메소드 호출
                remoteData = svc.SetSchChoiceChannelDetailDelete(remoteHeader, remoteData);

                // 결과코드검사
                if (!remoteData.ResultCD.Equals("0000"))
                {
                    throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
                }

                // 결과 셋트
                schChoiceAdModel.ResultCnt = remoteData.ResultCnt;
                schChoiceAdModel.ResultCD = remoteData.ResultCD;
            }
            catch (FrameException fe)
            {
                _log.Warning("-----------------------------------------");
                _log.Warning(this.ToString() + ":SetSchChoiceChannelDetailDelete():" + fe.ErrCode + ":" + fe.ResultMsg);
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
