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
    public class ContentsManager : BaseManager
    {
        /// <summary>
        /// 생성자
        /// </summary>
        /// <param name="systemModel"></param>
        /// <param name="commonModel"></param>

        public ContentsManager(SystemModel systemModel, CommonModel commonModel) : base(systemModel, commonModel)
        {
            _log = FrameSystem.oLog;
            _module = "CONTENT";
			_Host  = FrameSystem.m_WebServer_Host;
			_Port  = FrameSystem.m_WebServer_Port;
			_Path  = FrameSystem.m_WebServer_App + "/Media/ContentsService.asmx";
        }

        /// <summary>
        /// 컨텐츠정보조회(공용)
        /// </summary>
        /// <param name="contentsModel"></param>
        public void GetContentsListCommon(ContentsModel contentsModel)
        {
            try
            {
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
                remoteData = svc.GetContentsListCommon(remoteHeader, remoteData);

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
                _log.Warning( this.ToString() + ":GetContentsList():" + fe.ErrCode + ":" + fe.ResultMsg);
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
        /// 컨텐츠정보조회
        /// </summary>
        /// <param name="contentsModel"></param>
        public void GetContentsList(ContentsModel contentsModel)
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
                remoteData = svc.GetContentsList(remoteHeader, remoteData);

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
                _log.Warning( this.ToString() + ":GetContentsList():" + fe.ErrCode + ":" + fe.ResultMsg);
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
        /// Service 호출을 위한 메소드
        /// </summary>
        public bool GetContentsDetail(BaseModel baseModel)
        {
			
            _log.Debug("-----------------------------------------");
            _log.Debug( this.ToString() + " Start");
            _log.Debug("-----------------------------------------");

            _log.Debug("-----------------------------------------");
            _log.Debug( this.ToString() + " End");
            _log.Debug("-----------------------------------------");

            return true;
        }

        /// <summary>
        /// 컨텐츠추가
        /// </summary>
        /// <param name="contentsModel"></param>
        /// <returns></returns>
        public void SetContentsAdd(ContentsModel contentsModel)
        {
            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug("컨텐츠추가 Start");
                _log.Debug("-----------------------------------------");

                if(contentsModel.Title.Trim().Length < 1) 
                {
                    throw new FrameException("제목이 존재하지 않습니다.");
                }
                if(contentsModel.Title.Trim().Length > 120) 
                {
                    throw new FrameException("제목이 120Byte를 초과할 수 없습니다.");
                }
                if(contentsModel.Rate.Length >1 &&  Convert.ToInt32(contentsModel.Rate) > 255) 
                {
                    throw new FrameException("등급값은 255를 초과할 수 없습니다.");
                }
                if(contentsModel.ContentsState.Length >1 &&  Convert.ToInt32(contentsModel.ContentsState) > 255) 
                {
                    throw new FrameException("상태값은 255를 초과할 수 없습니다.");
                }
                if(contentsModel.SubTitle.Trim().Length > 40) 
                {
                    throw new FrameException("Sub제목은 40Byte를 초과할 수 없습니다.");
                }
                if(contentsModel.OrgTitle.Trim().Length > 40) 
                {
                    throw new FrameException("오리지널장르는 40Byte를 초과할 수 없습니다.");
                }


                // 웹서비스 인스턴스 생성
                ContentsServicePloxy.ContentsService svc = new ContentsServicePloxy.ContentsService();
				svc.Url = _WebServiceUrl;
				
                // 리모트 모델 생성
                ContentsServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.ContentsServicePloxy.HeaderModel();
                ContentsServicePloxy.ContentsModel remoteData   = new AdManagerClient.ContentsServicePloxy.ContentsModel();

                // 헤더정보 셋트
                remoteHeader.ClientKey     = Header.ClientKey;
                remoteHeader.UserID        = Header.UserID;

                // 호출정보셋트
                remoteData.Title      = contentsModel.Title;
                remoteData.ContentsState    = contentsModel.ContentsState;
                remoteData.Rate     = contentsModel.Rate;
                remoteData.SubTitle     = contentsModel.SubTitle;
                remoteData.OrgTitle     = contentsModel.OrgTitle;
					
                // 웹서비스 호출 타임아웃설정
                svc.Timeout = FrameSystem.m_SystemTimeout;

                // 웹서비스 메소드 호출
                remoteData = svc.SetContentsCreate(remoteHeader, remoteData);

                // 결과코드검사
                if(!remoteData.ResultCD.Equals("0000"))
                {
                    throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
                }

                // 결과 셋트
                contentsModel.ResultCnt   = remoteData.ResultCnt;
                _log.Debug("contentsModel.ResultCnt = "+contentsModel.ResultCnt);
			
                contentsModel.ResultCD    = remoteData.ResultCD;

                _log.Debug("-----------------------------------------");
                _log.Debug("컨텐츠추가 End");
                _log.Debug("-----------------------------------------");
            }
            catch(FrameException fe)
            {
                contentsModel.ResultCD    = "3101";
                _log.Warning("-----------------------------------------");
                _log.Warning( this.ToString() + ":SetContentsCreate():" + fe.ErrCode + ":" + fe.ResultMsg);
                _log.Warning("-----------------------------------------");
                throw fe;
            }
            catch(Exception e)
            {
                contentsModel.ResultCD    = "3101";
                _log.Error("-----------------------------------------");
                _log.Exception(e);
                _log.Error("-----------------------------------------");
                throw new FrameException(e.Message);
            }
        }

        /// <summary>
        /// 컨텐츠정보 수정
        /// </summary>
        /// <param name="contentsModel"></param>
        public void SetContentsUpdate(ContentsModel contentsModel)
        {
            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug("컨텐츠정보수정 Start");
                _log.Debug("-----------------------------------------");


                //				입력데이터의 Validation 검사
                if(contentsModel.Title.Trim().Length < 1) 
                {
                    throw new FrameException("제목이 존재하지 않습니다.");
                }
                if(contentsModel.Title.Trim().Length > 120) 
                {
                    throw new FrameException("제목이 120Byte를 초과할 수 없습니다.");
                }
                if(contentsModel.Rate.Length >1 &&  Convert.ToInt32(contentsModel.Rate) > 255) 
                {
                    throw new FrameException("등급값은 255이상을 초과할 수 없습니다.");
                }
                if(contentsModel.ContentsState.Length >1 &&  Convert.ToInt32(contentsModel.ContentsState) > 255) 
                {
                    throw new FrameException("상태값은 255이상을 초과할 수 없습니다.");
                }
                if(contentsModel.SubTitle.Trim().Length > 40) 
                {
                    throw new FrameException("Sub제목은 40Byte를 초과할 수 없습니다.");
                }
                if(contentsModel.OrgTitle.Trim().Length > 40) 
                {
                    throw new FrameException("오리지널장르는 40Byte를 초과할 수 없습니다.");
                }

                // 웹서비스 인스턴스 생성
                ContentsServicePloxy.ContentsService svc = new ContentsServicePloxy.ContentsService();
				svc.Url = _WebServiceUrl;
			
                // 리모트 모델 생성
                ContentsServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.ContentsServicePloxy.HeaderModel();
                ContentsServicePloxy.ContentsModel remoteData   = new AdManagerClient.ContentsServicePloxy.ContentsModel();

                // 헤더정보 셋트
                remoteHeader.ClientKey     = Header.ClientKey;
                remoteHeader.UserID        = Header.UserID;

                // 호출정보셋트
                remoteData.ContentKey      = contentsModel.ContentKey;
                remoteData.Title           = contentsModel.Title;
                remoteData.ContentsState   = contentsModel.ContentsState;
                remoteData.Rate            = contentsModel.Rate;
                remoteData.SubTitle        = contentsModel.SubTitle;
                remoteData.OrgTitle        = contentsModel.OrgTitle;
				
                // 웹서비스 호출 타임아웃설정
                svc.Timeout = FrameSystem.m_SystemTimeout;

                // 웹서비스 메소드 호출
                remoteData = svc.SetContentsUpdate(remoteHeader, remoteData);

                // 결과코드검사
                if(!remoteData.ResultCD.Equals("0000"))
                {
                    throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
                }

                // 결과 셋트
                contentsModel.ResultCnt   = remoteData.ResultCnt;
                contentsModel.ResultCD    = remoteData.ResultCD;

                _log.Debug("-----------------------------------------");
                _log.Debug("컨텐츠정보수정 End");
                _log.Debug("-----------------------------------------");
            }
            catch(FrameException fe)
            {
                contentsModel.ResultCD   = "3201";
                _log.Warning("-----------------------------------------");
                _log.Warning( this.ToString() + ":SetContentsUpdate():" + fe.ErrCode + ":" + fe.ResultMsg);
                _log.Warning("-----------------------------------------");
                throw fe;
            }
            catch(Exception e)
            {
                contentsModel.ResultCD   = "3201";
                _log.Error("-----------------------------------------");
                _log.Exception(e);
                _log.Error("-----------------------------------------");
                throw new FrameException(e.Message);
            }
        }

		
        /// <summary>
        /// 컨텐츠 삭제
        /// </summary>
        /// <param name="baseModel"></param>
        public void SetContentsDelete(ContentsModel contentsModel)
        {
            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug("컨텐츠삭제 start");
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

                // 호출정보셋트
                remoteData.ContentKey       = contentsModel.ContentKey;
					
                // 웹서비스 호출 타임아웃설정
                svc.Timeout = FrameSystem.m_SystemTimeout;

                // 웹서비스 메소드 호출
                remoteData = svc.SetContentsDelete(remoteHeader, remoteData);

                // 결과코드검사
                if(!remoteData.ResultCD.Equals("0000"))
                {
                    throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
                }

                // 결과 셋트
                contentsModel.ResultCnt   = remoteData.ResultCnt;
                contentsModel.ResultCD    = remoteData.ResultCD;

                _log.Debug("-----------------------------------------");
                _log.Debug("컨텐츠삭제 end");
                _log.Debug("-----------------------------------------");
            }
            catch(FrameException fe)
            {
                contentsModel.ResultCD   = "3301";
                _log.Warning("-----------------------------------------");
                _log.Warning( this.ToString() + ":setcontentsdelete():" + fe.ErrCode + ":" + fe.ResultMsg);
                _log.Warning("-----------------------------------------");
                throw fe;
            }
            catch(Exception e)
            {
                contentsModel.ResultCD   = "3301";
                _log.Error("-----------------------------------------");
                _log.Exception(e);
                _log.Error("-----------------------------------------");
                throw new FrameException(e.Message);
            }
        }

    }
}
