// ===============================================================================
// ChannelJump Manager  for Charites Project
//
// ChannelJumpManager.cs
//
// 채널점핑 서비스를 호출합니다. 
//
// ===============================================================================
// Release history
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
    /// 채널점핑정보 웹서비스를 호출합니다. 
    /// </summary>
    public class ChannelJumpManager : BaseManager
    {
        #region [ 생 성 자 ]
        /// <summary>
        /// 생성자
        /// </summary>
        /// <param name="systemModel"></param>
        /// <param name="commonModel"></param>
        public ChannelJumpManager(SystemModel systemModel, CommonModel commonModel) : base(systemModel, commonModel)
        {
            _log = FrameSystem.oLog;
            _module = "CONTENT";
            _Host  = FrameSystem.m_WebServer_Host;
            _Port  = FrameSystem.m_WebServer_Port;
            _Path  = FrameSystem.m_WebServer_App + "/Schedule/ChannelJumpService.asmx";
        }
        #endregion

        #region[ 메소드 : 채널점핑 ]

        /// <summary>
        /// 채널점핑정보 가져오기(단일)
        /// </summary>
        /// <param name="channelJumpModel"></param>
        public void GetChannelJump(ChannelJumpModel channelJumpModel)
        {
            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug("채널점핑정보조회 Start");
                _log.Debug("-----------------------------------------");
				
                // 웹서비스 인스턴스 생성
                ChannelJumpServicePloxy.ChannelJumpService svc = new ChannelJumpServicePloxy.ChannelJumpService();
                svc.Url = _WebServiceUrl;
                // 리모트 모델 생성
                ChannelJumpServicePloxy.HeaderModel         remoteHeader = new AdManagerClient.ChannelJumpServicePloxy.HeaderModel();
                ChannelJumpServicePloxy.ChannelJumpModel    remoteData   = new AdManagerClient.ChannelJumpServicePloxy.ChannelJumpModel();

                // 헤더정보 셋트
                remoteHeader.ClientKey     = Header.ClientKey;
                remoteHeader.UserID        = Header.UserID;

                // 호출정보 셋트
                remoteData.ItemNo           = channelJumpModel.ItemNo;
                remoteData.JumpType         = channelJumpModel.JumpType;
				
                // 웹서비스 호출 타임아웃설정
                svc.Timeout = FrameSystem.m_SystemTimeout;
                // 웹서비스 메소드 호출
                remoteData = svc.GetChannelJump(remoteHeader, remoteData);

                // 결과코드검사
                if(!remoteData.ResultCD.Equals("0000"))
                {
                    throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
                }

                // 결과 셋트
                channelJumpModel.PopupID    = remoteData.PopupID;
                channelJumpModel.ResultCnt  = remoteData.ResultCnt;
                channelJumpModel.ResultCD   = remoteData.ResultCD;

                _log.Debug("-----------------------------------------");
                _log.Debug("채널점핑목록조회 End");
                _log.Debug("-----------------------------------------");
            }
            catch(FrameException fe)
            {
                _log.Warning("-----------------------------------------");
                _log.Warning( this.ToString() + ":GetChannelJumpList():" + fe.ErrCode + ":" + fe.ResultMsg);
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
        /// 컨텐츠리스트 팝업 연동 URL DB에서 가져오기
        /// </summary>
        /// <returns></returns>
        public string GetContentListPopUrl()
        {
            string rtnValue = "";
            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug("채널점핑정보조회 Start");
                _log.Debug("-----------------------------------------");
				
                // 웹서비스 인스턴스 생성
                ChannelJumpServicePloxy.ChannelJumpService svc = new ChannelJumpServicePloxy.ChannelJumpService();
                svc.Url     = _WebServiceUrl;
                svc.Timeout = FrameSystem.m_SystemTimeout;
                rtnValue    = svc.GetContentListPopUrl();

                if( rtnValue.Length < 10 )
                {
                    throw new FrameException("컨텐츠리스트팝업URL을 읽는중 오류가 발생하였습니다", _module, "9999");
                }
            }
            catch(FrameException fe)
            {
                _log.Warning("-----------------------------------------");
                _log.Warning( this.ToString() + ":GetChannelJumpList():" + fe.ErrCode + ":" + fe.ResultMsg);
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
            return rtnValue;
        }


        /// <summary>
        /// 채널점핑정보 가져오기(목록)
        /// </summary>
        /// <param name="channelJumpModel"></param>
        public void GetChannelJumpList(ChannelJumpModel channelJumpModel)
        {

            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug("채널점핑목록조회 Start");
                _log.Debug("-----------------------------------------");
				
                // 웹서비스 인스턴스 생성
                ChannelJumpServicePloxy.ChannelJumpService svc = new ChannelJumpServicePloxy.ChannelJumpService();
			    svc.Url = _WebServiceUrl;
                // 리모트 모델 생성
                ChannelJumpServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.ChannelJumpServicePloxy.HeaderModel();
                ChannelJumpServicePloxy.ChannelJumpModel remoteData   = new AdManagerClient.ChannelJumpServicePloxy.ChannelJumpModel();

                // 헤더정보 셋트
                remoteHeader.ClientKey     = Header.ClientKey;
                remoteHeader.UserID        = Header.UserID;

                // 호출정보 셋트
                remoteData.SearchKey             = channelJumpModel.SearchKey;
                remoteData.SearchMediaCode       = channelJumpModel.SearchMediaCode;
                remoteData.SearchRapCode         = channelJumpModel.SearchRapCode;        
                remoteData.SearchchkAdState_10	 = channelJumpModel.SearchchkAdState_10; 
                remoteData.SearchchkAdState_20	 = channelJumpModel.SearchchkAdState_20; 
                remoteData.SearchchkAdState_30	 = channelJumpModel.SearchchkAdState_30; 
                remoteData.SearchchkAdState_40	 = channelJumpModel.SearchchkAdState_40; 
				remoteData.SearchAdType          = channelJumpModel.SearchAdType;          
				remoteData.SearchJumpType		 = channelJumpModel.SearchJumpType; 
				
				// 웹서비스 호출 타임아웃설정
                svc.Timeout = FrameSystem.m_SystemTimeout;
                // 웹서비스 메소드 호출
                remoteData = svc.GetChannelJumpList(remoteHeader, remoteData);

                // 결과코드검사
                if(!remoteData.ResultCD.Equals("0000"))
                {
                    throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
                }

                // 결과 셋트
                channelJumpModel.ChannelJumpDataSet = remoteData.ChannelJumpDataSet.Copy();
                channelJumpModel.ResultCnt   = remoteData.ResultCnt;
                channelJumpModel.ResultCD    = remoteData.ResultCD;

                _log.Debug("-----------------------------------------");
                _log.Debug("채널점핑목록조회 End");
                _log.Debug("-----------------------------------------");
            }
            catch(FrameException fe)
            {
                _log.Warning("-----------------------------------------");
                _log.Warning( this.ToString() + ":GetChannelJumpList():" + fe.ErrCode + ":" + fe.ResultMsg);
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
        /// [M]채널점핑 추가
        /// </summary>
        /// <param name="channelJumpModel"></param>
        public void SetChannelJumpCreate(ChannelJumpModel channelJumpModel)
        {
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("채널점핑추가 Start");
				_log.Debug("-----------------------------------------");
                        
				// 입력데이터의 Validation 검사
				if(channelJumpModel.ItemNo.Length < 1)  throw new FrameException("광고가 선택되지 않습니다.");
                      
				// 웹서비스 인스턴스 생성
				ChannelJumpServicePloxy.ChannelJumpService svc = new ChannelJumpServicePloxy.ChannelJumpService();
				svc.Url = _WebServiceUrl;        				
				// 리모트 모델 생성
				ChannelJumpServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.ChannelJumpServicePloxy.HeaderModel();
				ChannelJumpServicePloxy.ChannelJumpModel remoteData   = new AdManagerClient.ChannelJumpServicePloxy.ChannelJumpModel();
                        
				// 헤더정보 셋트
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;
                        
				// 호출정보셋트
				//Key 설정
				remoteData.ItemNo     = channelJumpModel.ItemNo;		// 광고번호
				remoteData.ItemName   = channelJumpModel.ItemName;		// 광고명
				remoteData.MediaCode  = channelJumpModel.MediaCode;		// 매체코드
				remoteData.JumpType   = channelJumpModel.JumpType;		// 점프구분
				remoteData.GenreCode  = channelJumpModel.GenreCode;		// 장르코드    
				remoteData.GenreName  = channelJumpModel.GenreName;		// 장르명
				remoteData.ChannelNo  = channelJumpModel.ChannelNo;		// 채널번호
				remoteData.Title      = channelJumpModel.Title;			// 프로그램명  
				remoteData.ContentID  = channelJumpModel.ContentID;		// 컨텐츠ID
				remoteData.PopupID    = channelJumpModel.PopupID;		// 공지ID
				remoteData.PopupTitle = channelJumpModel.PopupTitle;	        // 공지제목
                remoteData.ChannelManager = channelJumpModel.ChannelManager;    //
				remoteData.HomeYn     = channelJumpModel.HomeYn;		        // 홈 노출여부
				remoteData.ChannelYn  = channelJumpModel.ChannelYn;		        // 채널 노출여부
				remoteData.StbTypeYn = channelJumpModel.StbTypeYn;
				remoteData.StbTypeString = channelJumpModel.StbTypeString;
                
				// 웹서비스 호출 타임아웃설정
				svc.Timeout = FrameSystem.m_SystemTimeout;
				// 웹서비스 메소드 호출
				remoteData = svc.SetChannelJumpCreate(remoteHeader, remoteData);
                        
				// 결과코드검사
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}
                        
				// 결과 셋트
				channelJumpModel.ResultCnt   = remoteData.ResultCnt;
				_log.Debug("channelJumpModel.ResultCnt = "+channelJumpModel.ResultCnt);
                        			
				channelJumpModel.ResultCD    = remoteData.ResultCD;
                        
				_log.Debug("-----------------------------------------");
				_log.Debug("채널점핑추가 End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				channelJumpModel.ResultCD    = "3101";
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":SetChannelJumpCreate():" + fe.ErrCode + ":" + fe.ResultMsg);
				_log.Warning("-----------------------------------------");
				throw fe;
			}
			catch(Exception e)
			{
				channelJumpModel.ResultCD    = "3101";
				_log.Error("-----------------------------------------");
				_log.Exception(e);
				_log.Error("-----------------------------------------");
				throw new FrameException(e.Message);
			}

        }


        /// <summary>
        /// [M]채널점핑정보 수정
        /// </summary>
        /// <param name="channelJumpModel"></param>
        public void SetChannelJumpUpdate(ChannelJumpModel channelJumpModel)
        {
            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug("채널점핑정보수정 Start");
                _log.Debug("-----------------------------------------");
                        
				if(channelJumpModel.ItemNo.Length < 1)  throw new FrameException("광고가 선택되지 않습니다.");
                         
                ChannelJumpServicePloxy.ChannelJumpService svc = new ChannelJumpServicePloxy.ChannelJumpService();
                svc.Url = _WebServiceUrl;                       			
                ChannelJumpServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.ChannelJumpServicePloxy.HeaderModel();
                ChannelJumpServicePloxy.ChannelJumpModel remoteData   = new AdManagerClient.ChannelJumpServicePloxy.ChannelJumpModel();
                        
                // 헤더정보 셋트
                remoteHeader.ClientKey     = Header.ClientKey;
                remoteHeader.UserID        = Header.UserID;
                        
                // 호출정보셋트
				remoteData.ItemNo     = channelJumpModel.ItemNo;		// 광고번호
				remoteData.ItemName   = channelJumpModel.ItemName;		// 광고명
				remoteData.MediaCode  = channelJumpModel.MediaCode;		// 매체코드
				remoteData.JumpType   = channelJumpModel.JumpType;		// 점프구분
				remoteData.GenreCode  = channelJumpModel.GenreCode;		// 장르코드    
				remoteData.GenreName  = channelJumpModel.GenreName;		// 장르명
				remoteData.ChannelNo  = channelJumpModel.ChannelNo;		// 채널번호
				remoteData.Title      = channelJumpModel.Title;			// 프로그램명 / 제목 
				remoteData.ContentID  = channelJumpModel.ContentID;		// 컨텐츠ID
				remoteData.PopupID    = channelJumpModel.PopupID;		// 공지ID
				remoteData.PopupTitle = channelJumpModel.PopupTitle;	// 공지제목
                remoteData.ChannelManager = channelJumpModel.ChannelManager;    //
				remoteData.HomeYn     = channelJumpModel.HomeYn;		// 홈 노출여부
				remoteData.ChannelYn  = channelJumpModel.ChannelYn;		// 채널 노출여부
				remoteData.StbTypeYn = channelJumpModel.StbTypeYn;
				remoteData.StbTypeString = channelJumpModel.StbTypeString;

                           				
                svc.Timeout = FrameSystem.m_SystemTimeout;
                remoteData = svc.SetChannelJumpUpdate(remoteHeader, remoteData);
                        
                if(!remoteData.ResultCD.Equals("0000")) throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
                        
                channelJumpModel.ResultCnt   = remoteData.ResultCnt;
                channelJumpModel.ResultCD    = remoteData.ResultCD;
                        
                _log.Debug("-----------------------------------------");
                _log.Debug("채널점핑정보수정 End");
                _log.Debug("-----------------------------------------");
            }
            catch(FrameException fe)
            {
                _log.Warning("-----------------------------------------");
                _log.Warning( this.ToString() + ":SetChannelJumpUpdate():" + fe.ErrCode + ":" + fe.ResultMsg);
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
        /// 채널점핑 삭제
        /// </summary>
        /// <param name="baseModel"></param>
        public void SetChannelJumpDelete(ChannelJumpModel channelJumpModel)
        {
            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug("채널점핑 삭제 Start");
                _log.Debug("-----------------------------------------");

                // 웹서비스 인스턴스 생성
                ChannelJumpServicePloxy.ChannelJumpService svc = new ChannelJumpServicePloxy.ChannelJumpService();
              
                // URL의 동적셋트
                svc.Url = _WebServiceUrl;
				
                // 리모트 모델 생성
                ChannelJumpServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.ChannelJumpServicePloxy.HeaderModel();
                ChannelJumpServicePloxy.ChannelJumpModel remoteData   = new AdManagerClient.ChannelJumpServicePloxy.ChannelJumpModel();
              
                // 헤더정보 셋트
                remoteHeader.ClientKey  = Header.ClientKey;
                remoteHeader.UserID     = Header.UserID;

                // 호출정보셋트
				remoteData.ItemNo    = channelJumpModel.ItemNo;     // 광고번호
				remoteData.ItemName  = channelJumpModel.ItemName;	// 광고명
				remoteData.MediaCode = channelJumpModel.MediaCode;	// 매체코드
				
				// 웹서비스 호출 타임아웃설정
                svc.Timeout = FrameSystem.m_SystemTimeout;	
                // 웹서비스 메소드 호출
                remoteData = svc.SetChannelJumpDelete(remoteHeader, remoteData);

                // 결과코드검사
                if(!remoteData.ResultCD.Equals("0000"))
                {
                    throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
                }

                // 결과 셋트
                channelJumpModel.ResultCnt   = remoteData.ResultCnt;
                channelJumpModel.ResultCD    = remoteData.ResultCD;

                _log.Debug("-----------------------------------------");
                _log.Debug("채널점핑삭제 End");
                _log.Debug("-----------------------------------------");
            }
            catch(FrameException fe)
            {
                _log.Warning("-----------------------------------------");
                _log.Warning( this.ToString() + ":SetChannelJumpDelete():" + fe.ErrCode + ":" + fe.ResultMsg);
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

        #endregion

        #region [ 메소드 : 검색용 ]
		/// <summary>
		/// 광고검색
		/// </summary>
		/// <param name="channelJumpModel"></param>
		public void GetContractItemList(ChannelJumpModel channelJumpModel)
		{

			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("광고검색 Start");
				_log.Debug("-----------------------------------------");
				
				// 웹서비스 인스턴스 생성
				ChannelJumpServicePloxy.ChannelJumpService svc = new ChannelJumpServicePloxy.ChannelJumpService();
				svc.Url = _WebServiceUrl;
				// 리모트 모델 생성
				ChannelJumpServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.ChannelJumpServicePloxy.HeaderModel();
				ChannelJumpServicePloxy.ChannelJumpModel remoteData   = new AdManagerClient.ChannelJumpServicePloxy.ChannelJumpModel();

				// 헤더정보 셋트
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;

				// 호출정보 셋트
				remoteData.SearchKey             = channelJumpModel.SearchKey;
				remoteData.SearchMediaCode       = channelJumpModel.SearchMediaCode;
				remoteData.SearchRapCode         = channelJumpModel.SearchRapCode;        
				remoteData.SearchchkAdState_10	 = channelJumpModel.SearchchkAdState_10; 
				remoteData.SearchchkAdState_20	 = channelJumpModel.SearchchkAdState_20; 
				remoteData.SearchchkAdState_30	 = channelJumpModel.SearchchkAdState_30; 
				remoteData.SearchchkAdState_40	 = channelJumpModel.SearchchkAdState_40; 
				
				// 웹서비스 호출 타임아웃설정
				svc.Timeout = FrameSystem.m_SystemTimeout;
				// 웹서비스 메소드 호출
				remoteData = svc.GetContractItemList(remoteHeader, remoteData);

				// 결과코드검사
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// 결과 셋트
				channelJumpModel.ContractItemDataSet = remoteData.ContractItemDataSet.Copy();
				channelJumpModel.ResultCnt   = remoteData.ResultCnt;
				channelJumpModel.ResultCD    = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("광고검색 End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":GetContractItemList():" + fe.ErrCode + ":" + fe.ResultMsg);
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
		/// 채널검색
		/// </summary>
		/// <param name="channelJumpModel"></param>
		public void GetChannelList(ChannelJumpModel channelJumpModel)
		{

			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("채널검색 Start");
				_log.Debug("-----------------------------------------");
				
				// 웹서비스 인스턴스 생성
				ChannelJumpServicePloxy.ChannelJumpService svc = new ChannelJumpServicePloxy.ChannelJumpService();
				svc.Url = _WebServiceUrl;
				// 리모트 모델 생성
				ChannelJumpServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.ChannelJumpServicePloxy.HeaderModel();
				ChannelJumpServicePloxy.ChannelJumpModel remoteData   = new AdManagerClient.ChannelJumpServicePloxy.ChannelJumpModel();

				// 헤더정보 셋트
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;

				// 호출정보 셋트
				remoteData.SearchKey             = channelJumpModel.SearchKey;
				remoteData.SearchMediaCode       = channelJumpModel.SearchMediaCode;
				
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
				channelJumpModel.ChannelListDataSet = remoteData.ChannelListDataSet.Copy();
				channelJumpModel.ResultCnt   = remoteData.ResultCnt;
				channelJumpModel.ResultCD    = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("채널검색 End");
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
		/// 컨텐츠검색
		/// </summary>
		/// <param name="channelJumpModel"></param>
		public void GetContentList(ChannelJumpModel channelJumpModel)
		{

			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("컨텐츠검색 Start");
				_log.Debug("-----------------------------------------");
				
				// 웹서비스 인스턴스 생성
				ChannelJumpServicePloxy.ChannelJumpService svc = new ChannelJumpServicePloxy.ChannelJumpService();
				svc.Url = _WebServiceUrl;
				// 리모트 모델 생성
				ChannelJumpServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.ChannelJumpServicePloxy.HeaderModel();
				ChannelJumpServicePloxy.ChannelJumpModel remoteData   = new AdManagerClient.ChannelJumpServicePloxy.ChannelJumpModel();

				// 헤더정보 셋트
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;

				// 호출정보 셋트
				remoteData.SearchKey             = channelJumpModel.SearchKey;
				remoteData.SearchMediaCode       = channelJumpModel.SearchMediaCode;
				
				// 웹서비스 호출 타임아웃설정
				svc.Timeout = FrameSystem.m_SystemTimeout;
				// 웹서비스 메소드 호출
				remoteData = svc.GetContentList(remoteHeader, remoteData);

				// 결과코드검사
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// 결과 셋트
				channelJumpModel.ContentListDataSet = remoteData.ContentListDataSet.Copy();
				channelJumpModel.ResultCnt   = remoteData.ResultCnt;
				channelJumpModel.ResultCD    = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("컨텐츠검색 End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":GetContentList():" + fe.ErrCode + ":" + fe.ResultMsg);
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
		/// 팝업공지리스트 조회
		/// </summary>
		/// <param name="channelJumpModel"></param>
		public void GetAdPopList(ChannelJumpModel channelJumpModel)
		{

			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("팝업공지리스트 조회 Start");
				_log.Debug("-----------------------------------------");
				
				// 웹서비스 인스턴스 생성
				ChannelJumpServicePloxy.ChannelJumpService svc = new ChannelJumpServicePloxy.ChannelJumpService();
				svc.Url = _WebServiceUrl;
				// 리모트 모델 생성
				ChannelJumpServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.ChannelJumpServicePloxy.HeaderModel();
				ChannelJumpServicePloxy.ChannelJumpModel remoteData   = new AdManagerClient.ChannelJumpServicePloxy.ChannelJumpModel();

				// 헤더정보 셋트
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;

				// 호출정보 셋트
				remoteData.Type       = channelJumpModel.Type;
				
				// 웹서비스 호출 타임아웃설정
				svc.Timeout = FrameSystem.m_SystemTimeout * 5; // 타 시스템을 연동해야하므로 길게...
				// 웹서비스 메소드 호출
				remoteData = svc.GetAdPopList(remoteHeader, remoteData);

				// 결과코드검사
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// 결과 셋트
				if(remoteData.ResultCnt > 0)
				{
					channelJumpModel.AdPopListDataSet = remoteData.AdPopListDataSet.Copy();
				}
				channelJumpModel.ResultCnt   = remoteData.ResultCnt;
				channelJumpModel.ResultCD    = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("컨텐츠검색 End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ".GetAdPopList():" + fe.ErrCode + ":" + fe.ResultMsg);
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

        #endregion
    }
}