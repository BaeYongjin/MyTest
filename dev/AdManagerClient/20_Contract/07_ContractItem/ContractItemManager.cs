// ===============================================================================
// UserUpdate Manager  for Charites Project
//
// UserUpdateManager.cs
//
// 광고내역정보 저장 서비스를 호출합니다. 
//
// ===============================================================================
// Release history
// 2007.06.26 송명환 v1.0
// ===============================================================================
// Copyright (C) 2007 G-Inno Systems Inc.
// All rights reserved.
// ==============================================================================
/*
 * -------------------------------------------------------
 * Class Name: ContractItemManager
 * 주요기능  : 계약관리 처리 서비스의 Client Manager
 * 작성자    : bae 
 * 작성일    : 2010.06.10
 * 특이사항  : 
 * -------------------------------------------------------
 * [수정사항]
 * -------------------------------------------------------
 * 수정코드  : [E_01]
 * 수정자    : bae
 * 수정일    : 2010.06.10
 * 수정내용  : 
 *            - 광고상태 일괄변경 처리 추가
 *            - 광고채널 일괄지정 처리 추가
 * --------------------------------------------------------
 * 
 */
using System;
using System.Data;

using WinFramework.Base;
using WinFramework.Data;
using WinFramework.Misc;

using AdManagerModel;

namespace AdManagerClient
{
    /// <summary>
    /// 광고내역정보 웹서비스를 호출합니다. 
    /// </summary>
    public class ContractItemManager : BaseManager
    {
        /// <summary>
        /// 생성자
        /// </summary>
        /// <param name="systemModel"></param>
        /// <param name="commonModel"></param>

        public ContractItemManager(SystemModel systemModel, CommonModel commonModel) : base(systemModel, commonModel)
        {
            _log = FrameSystem.oLog;
            _module = "CONTENT";
            _Host  = FrameSystem.m_WebServer_Host;
            _Port  = FrameSystem.m_WebServer_Port;
            _Path  = FrameSystem.m_WebServer_App + "/Contract/ContractItemService.asmx";
        }

		/// <summary>
		/// 광고내역정보조회
		/// </summary>
		/// <param name="contractItemModel"></param>
		public void GetGradeCodeList(ContractItemModel contractItemModel)
		{

			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("등급목록조회 Start");
				_log.Debug("-----------------------------------------");
				
				// 웹서비스 인스턴스 생성
				ContractItemServicePloxy.ContractItemService svc = new ContractItemServicePloxy.ContractItemService();
				svc.Url = _WebServiceUrl;
				// 리모트 모델 생성
				ContractItemServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.ContractItemServicePloxy.HeaderModel();
				ContractItemServicePloxy.ContractItemModel remoteData   = new AdManagerClient.ContractItemServicePloxy.ContractItemModel();

				// 헤더정보 셋트
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;

								
				// 웹서비스 호출 타임아웃설정
				svc.Timeout = FrameSystem.m_SystemTimeout;
				// 웹서비스 메소드 호출
				remoteData = svc.GetGradeCodeList(remoteHeader, remoteData);

				// 결과코드검사
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// 결과 셋트
				contractItemModel.GradeDataSet = remoteData.GradeDataSet.Copy();
				contractItemModel.ResultCnt   = remoteData.ResultCnt;
				contractItemModel.ResultCD    = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("등급목록조회 End");
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
        /// 광고내역정보조회
        /// </summary>
        /// <param name="contractItemModel"></param>
        public void GetContractItemList(ContractItemModel contractItemModel)
        {

            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug("광고내역목록조회 Start");
                _log.Debug("-----------------------------------------");
				
                // 웹서비스 인스턴스 생성
                ContractItemServicePloxy.ContractItemService svc = new ContractItemServicePloxy.ContractItemService();
			    svc.Url = _WebServiceUrl;
                // 리모트 모델 생성
                ContractItemServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.ContractItemServicePloxy.HeaderModel();
                ContractItemServicePloxy.ContractItemModel remoteData   = new AdManagerClient.ContractItemServicePloxy.ContractItemModel();

                // 헤더정보 셋트
                remoteHeader.ClientKey     = Header.ClientKey;
                remoteHeader.UserID        = Header.UserID;

                // 호출정보 셋트
                remoteData.SearchKey             = contractItemModel.SearchKey;
                remoteData.MediaCode             = contractItemModel.MediaCode;
                remoteData.RapCode               = contractItemModel.RapCode;        
                remoteData.AgencyCode            = contractItemModel.AgencyCode;     
                remoteData.AdvertiserCode        = contractItemModel.AdvertiserCode; 
                remoteData.AdType				 = contractItemModel.AdType;          
                remoteData.AdClass               = contractItemModel.AdClass;          
                remoteData.SearchchkAdState_10	 = contractItemModel.SearchchkAdState_10; 
                remoteData.SearchchkAdState_20	 = contractItemModel.SearchchkAdState_20; 
                remoteData.SearchchkAdState_30	 = contractItemModel.SearchchkAdState_30; 
                remoteData.SearchchkAdState_40	 = contractItemModel.SearchchkAdState_40; 
				
				remoteData.SearchChkSch_YN		 = contractItemModel.SearchChkSch_YN; 
				
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
                contractItemModel.ContractItemDataSet = remoteData.ContractItemDataSet.Copy();
                contractItemModel.ResultCnt   = remoteData.ResultCnt;
                contractItemModel.ResultCD    = remoteData.ResultCD;

                _log.Debug("-----------------------------------------");
                _log.Debug("광고내역목록조회 End");
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
        /// 광고내역 디테일 목록조회
        /// </summary>
        /// <param name="contractItemModel"></param>
        public void GetContractItemDetail(ContractItemModel contractItemModel)
        {
            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug("광고내역 디테일 조회 Start");
                _log.Debug("-----------------------------------------");
            				
                // 웹서비스 인스턴스 생성
                ContractItemServicePloxy.ContractItemService svc = new ContractItemServicePloxy.ContractItemService();
            	svc.Url = _WebServiceUrl;		
                // 리모트 모델 생성
                ContractItemServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.ContractItemServicePloxy.HeaderModel();
                ContractItemServicePloxy.ContractItemModel remoteData   = new AdManagerClient.ContractItemServicePloxy.ContractItemModel();
            
                // 헤더정보 셋트
                remoteHeader.ClientKey	= Header.ClientKey;
                remoteHeader.UserID     = Header.UserID;
                remoteData.ItemNo       = contractItemModel.ItemNo;
            		
                // 웹서비스 호출 타임아웃설정
                svc.Timeout = FrameSystem.m_SystemTimeout;

                // 웹서비스 메소드 호출
                remoteData = svc.GetContractItemDetail(remoteHeader, remoteData);

				if( null != svc)
				{
					svc.Dispose();
					svc = null;
				}
            
                // 결과코드검사
                if(!remoteData.ResultCD.Equals("0000"))
                {
                    throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
                }
            
                // 결과 셋트
                contractItemModel.ContractItemDataSet = remoteData.ContractItemDataSet.Copy();
                contractItemModel.ResultCnt   = remoteData.ResultCnt;
                contractItemModel.ResultCD    = remoteData.ResultCD;
            
                _log.Debug("-----------------------------------------");
                _log.Debug("광고내역 디테일 목록조회 End");
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
		/// 링크채널 목록조회
		/// </summary>
		/// <param name="contractItemModel"></param>
		public void GetLinkChannel(ContractItemModel contractItemModel)
		{
            
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("링크채널 조회 Start");
				_log.Debug("-----------------------------------------");
            				
				// 웹서비스 인스턴스 생성
				ContractItemServicePloxy.ContractItemService svc = new ContractItemServicePloxy.ContractItemService();
				svc.Url = _WebServiceUrl;		
				// 리모트 모델 생성
				ContractItemServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.ContractItemServicePloxy.HeaderModel();
				ContractItemServicePloxy.ContractItemModel remoteData   = new AdManagerClient.ContractItemServicePloxy.ContractItemModel();
            
				// 헤더정보 셋트
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;
            
				// 호출정보 셋트
                remoteData.ItemNo = contractItemModel.ItemNo;
                remoteData.LinkType = contractItemModel.LinkType;
            		
				// 웹서비스 호출 타임아웃설정
				svc.Timeout = FrameSystem.m_SystemTimeout;

				// 웹서비스 메소드 호출
				remoteData = svc.GetLinkChannel2(remoteHeader, remoteData);

				if( null != svc)
				{
					svc.Dispose();
					svc = null;
				}
            
				// 결과코드검사
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}
            
				// 결과 셋트
				contractItemModel.LinkChannelDataSet = remoteData.LinkChannelDataSet.Copy();
				contractItemModel.ResultCnt   = remoteData.ResultCnt;
				contractItemModel.ResultCD    = remoteData.ResultCD;
            
				_log.Debug("-----------------------------------------");
				_log.Debug("링크채널 목록조회 End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":GetLinkChannel():" + fe.ErrCode + ":" + fe.ResultMsg);
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
        /// 엔딩광고 듀얼설정조회
        /// </summary>
        /// <param name="contractItemModel"></param>
        public void GetLinkItem(ContractItemModel contractItemModel)
        {
            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug("듀얼설정 조회 Start");
                _log.Debug("-----------------------------------------");

                // 웹서비스 인스턴스 생성
                ContractItemServicePloxy.ContractItemService svc = new ContractItemServicePloxy.ContractItemService();
                svc.Url = _WebServiceUrl;
                ContractItemServicePloxy.HeaderModel remoteHeader = new AdManagerClient.ContractItemServicePloxy.HeaderModel();
                ContractItemServicePloxy.ContractItemModel remoteData = new AdManagerClient.ContractItemServicePloxy.ContractItemModel();

                // 헤더정보 셋트
                remoteHeader.ClientKey = Header.ClientKey;
                remoteHeader.UserID = Header.UserID;

                // 호출정보 셋트
                remoteData.ItemNo = contractItemModel.ItemNo;
                remoteData.ScheduleType = contractItemModel.ScheduleType;

                // 웹서비스 호출 타임아웃설정
                svc.Timeout = FrameSystem.m_SystemTimeout;

                // 웹서비스 메소드 호출
                remoteData = svc.GetLinkItem(remoteHeader, remoteData);

                if (null != svc)
                {
                    svc.Dispose();
                    svc = null;
                }

                // 결과코드검사
                if (!remoteData.ResultCD.Equals("0000"))
                {
                    throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
                }

                // 결과 셋트
                contractItemModel.ContractItemDataSet = remoteData.ContractItemDataSet.Copy();
                contractItemModel.ResultCnt = remoteData.ResultCnt;
                contractItemModel.ResultCD = remoteData.ResultCD;

                _log.Debug("-----------------------------------------");
                _log.Debug("듀얼설정 목록조회 End");
                _log.Debug("-----------------------------------------");
            }
            catch (FrameException fe)
            {
                _log.Warning("-----------------------------------------");
                _log.Warning(this.ToString() + ":GetLinkItem():" + fe.ErrCode + ":" + fe.ResultMsg);
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
        /// 광고내역 디테일 목록조회
        /// </summary>
        /// <param name="contractItemModel"></param>
        public void GetContractItemHIstoryList(ContractItemModel contractItemModel)
        {
            
            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug("광고내역 디테일 조회 Start");
                _log.Debug("-----------------------------------------");
            				
                // 웹서비스 인스턴스 생성
                ContractItemServicePloxy.ContractItemService svc = new ContractItemServicePloxy.ContractItemService();
            	svc.Url = _WebServiceUrl;		
                // 리모트 모델 생성
                ContractItemServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.ContractItemServicePloxy.HeaderModel();
                ContractItemServicePloxy.ContractItemModel remoteData   = new AdManagerClient.ContractItemServicePloxy.ContractItemModel();
            
                // 헤더정보 셋트
                remoteHeader.ClientKey     = Header.ClientKey;
                remoteHeader.UserID        = Header.UserID;
            
                // 호출정보 셋트
                remoteData.ItemNo       = contractItemModel.ItemNo;
            				
                // 웹서비스 호출 타임아웃설정
                svc.Timeout = FrameSystem.m_SystemTimeout;
                // 웹서비스 메소드 호출
                remoteData = svc.GetContractItemHIstoryList(remoteHeader, remoteData);

				if( null != svc)
				{
					svc.Dispose();
					svc = null;
				}
            
                // 결과코드검사
                if(!remoteData.ResultCD.Equals("0000"))
                {
                    throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
                }
            
                // 결과 셋트
                contractItemModel.ContractItemDataSet = remoteData.ContractItemDataSet.Copy();
                contractItemModel.ResultCnt   = remoteData.ResultCnt;
                contractItemModel.ResultCD    = remoteData.ResultCD;
            
                _log.Debug("-----------------------------------------");
                _log.Debug("광고내역 디테일 목록조회 End");
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
		/// 자료 목록조회
		/// </summary>
		/// <param name="contractItemModel"></param>
		public void GetFileList(ContractItemModel contractItemModel)
		{
            
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("자료 디테일 조회 Start");
				_log.Debug("-----------------------------------------");
            				
				// 웹서비스 인스턴스 생성
				ContractItemServicePloxy.ContractItemService svc = new ContractItemServicePloxy.ContractItemService();
				svc.Url = _WebServiceUrl;		
				// 리모트 모델 생성
				ContractItemServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.ContractItemServicePloxy.HeaderModel();
				ContractItemServicePloxy.ContractItemModel remoteData   = new AdManagerClient.ContractItemServicePloxy.ContractItemModel();
            
				// 헤더정보 셋트
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;
            
				// 호출정보 셋트
				remoteData.ItemNo       = contractItemModel.ItemNo;
            				
				// 웹서비스 호출 타임아웃설정
				svc.Timeout = FrameSystem.m_SystemTimeout;
				// 웹서비스 메소드 호출
				remoteData = svc.GetFileList(remoteHeader, remoteData);

				if( null != svc)
				{
					svc.Dispose();
					svc = null;
				}
            
				// 결과코드검사
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}
            
				// 결과 셋트
				contractItemModel.ContractItemDataSet = remoteData.ContractItemDataSet.Copy();
				contractItemModel.ResultCnt   = remoteData.ResultCnt;
				contractItemModel.ResultCD    = remoteData.ResultCD;
            
				_log.Debug("-----------------------------------------");
				_log.Debug("자료 디테일 목록조회 End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":GetFileList():" + fe.ErrCode + ":" + fe.ResultMsg);
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
		/// 파일 사이즈 관련정보
		/// </summary>
		/// <param name="contractItemModel"></param>
		public void GetFileInfo(ContractItemModel contractItemModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("파일정보 조회 Start");
				_log.Debug("-----------------------------------------");

				// 웹서비스 인스턴스 생성
				ContractItemServicePloxy.ContractItemService svc = new ContractItemServicePloxy.ContractItemService();
				svc.Url = _WebServiceUrl;
				// 리모트 모델 생성
				ContractItemServicePloxy.HeaderModel remoteHeader = new AdManagerClient.ContractItemServicePloxy.HeaderModel();
				ContractItemServicePloxy.ContractItemModel remoteData = new AdManagerClient.ContractItemServicePloxy.ContractItemModel();

				// 헤더정보 셋트
				remoteHeader.ClientKey = Header.ClientKey;
				remoteHeader.UserID = Header.UserID;

				// 웹서비스 호출 타임아웃설정
				svc.Timeout = FrameSystem.m_SystemTimeout;
				remoteData = svc.wGetFileInfo(remoteHeader, remoteData);

				if (null != svc)
				{
					svc.Dispose();
					svc = null;
				}

				// 결과코드검사
				if (!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// 결과 셋트
				contractItemModel.ContractItemDataSet = remoteData.ContractItemDataSet.Copy();
				contractItemModel.ResultCnt = remoteData.ResultCnt;
				contractItemModel.ResultCD = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("파일정보 목록조회 End");
				_log.Debug("-----------------------------------------");
			}
			catch (FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning(this.ToString() + ":GetFileInfo():" + fe.ErrCode + ":" + fe.ResultMsg);
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
		/// 채널검색
		/// </summary>
		/// <param name="contractItemModel"></param>
		public void GetChannelList(ContractItemModel contractItemModel)
		{

			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("채널검색 Start");
				_log.Debug("-----------------------------------------");
				
				// 웹서비스 인스턴스 생성
				ContractItemServicePloxy.ContractItemService svc = new ContractItemServicePloxy.ContractItemService();
				svc.Url = _WebServiceUrl;
				// 리모트 모델 생성
				ContractItemServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.ContractItemServicePloxy.HeaderModel();
				ContractItemServicePloxy.ContractItemModel remoteData   = new AdManagerClient.ContractItemServicePloxy.ContractItemModel();

				// 헤더정보 셋트
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;

				// 호출정보 셋트
				remoteData.SearchKey             = contractItemModel.SearchKey;
				remoteData.SearchMediaCode       = contractItemModel.SearchMediaCode;
				
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
				contractItemModel.ContractItemDataSet = remoteData.ContractItemDataSet.Copy();
				contractItemModel.ResultCnt   = remoteData.ResultCnt;
				contractItemModel.ResultCD    = remoteData.ResultCD;

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



		public void SetFileCreate(ContractItemModel contractItemModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("광고내역추가 Start");
				_log.Debug("-----------------------------------------");
                                        
				// 웹서비스 인스턴스 생성
				ContractItemServicePloxy.ContractItemService svc = new ContractItemServicePloxy.ContractItemService();
				svc.Url = _WebServiceUrl;        				
				// 리모트 모델 생성
				ContractItemServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.ContractItemServicePloxy.HeaderModel();
				ContractItemServicePloxy.ContractItemModel remoteData   = new AdManagerClient.ContractItemServicePloxy.ContractItemModel();
                        
				// 헤더정보 셋트
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;
                        
				// 호출정보셋트
				//Key 설정
				remoteData.ItemNo            = contractItemModel.ItemNo;
				
				remoteData.FileTitle             = contractItemModel.FileTitle;   
				remoteData.FileName              = contractItemModel.FileName;   
				remoteData.FilePath				 = contractItemModel.FilePath;     
				                
				// 웹서비스 호출 타임아웃설정
				svc.Timeout = FrameSystem.m_SystemTimeout;
				// 웹서비스 메소드 호출
				remoteData = svc.SetFileCreate(remoteHeader, remoteData);

				if( null != svc)
				{
					svc.Dispose();
					svc = null;
				}
                        
				// 결과코드검사
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}
                        
				// 결과 셋트
				contractItemModel.ResultCnt   = remoteData.ResultCnt;
				_log.Debug("contractItemModel.ResultCnt = "+contractItemModel.ResultCnt);
                        			
				contractItemModel.ResultCD    = remoteData.ResultCD;
                        
				_log.Debug("-----------------------------------------");
				_log.Debug("광고내역추가 End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				contractItemModel.ResultCD    = "3101";
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":SetFileCreate():" + fe.ErrCode + ":" + fe.ResultMsg);
				_log.Warning("-----------------------------------------");
				throw fe;
			}
			catch(Exception e)
			{
				contractItemModel.ResultCD    = "3101";
				_log.Error("-----------------------------------------");
				_log.Exception(e);
				_log.Error("-----------------------------------------");
				throw new FrameException(e.Message);
			}
		}

		public void SetFileUpdate(ContractItemModel contractItemModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("자료첨부수정 Start");
				_log.Debug("-----------------------------------------");
                                        
				// 웹서비스 인스턴스 생성
				ContractItemServicePloxy.ContractItemService svc = new ContractItemServicePloxy.ContractItemService();
				svc.Url = _WebServiceUrl;        				
				// 리모트 모델 생성
				ContractItemServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.ContractItemServicePloxy.HeaderModel();
				ContractItemServicePloxy.ContractItemModel remoteData   = new AdManagerClient.ContractItemServicePloxy.ContractItemModel();
                        
				// 헤더정보 셋트
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;
                        
				// 호출정보셋트
				//Key 설정
				remoteData.ItemNo            = contractItemModel.ItemNo;
				remoteData.FileNo            = contractItemModel.FileNo;
				
				remoteData.FileTitle             = contractItemModel.FileTitle;   
				remoteData.FileName              = contractItemModel.FileName;   
				remoteData.FilePath				 = contractItemModel.FilePath;     
				                
				// 웹서비스 호출 타임아웃설정
				svc.Timeout = FrameSystem.m_SystemTimeout;
				// 웹서비스 메소드 호출
				remoteData = svc.SetFileUpdate(remoteHeader, remoteData);

				if( null != svc)
				{
					svc.Dispose();
					svc = null;
				}
                        
				// 결과코드검사
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}
                        
				// 결과 셋트
				contractItemModel.ResultCnt   = remoteData.ResultCnt;
				_log.Debug("contractItemModel.ResultCnt = "+contractItemModel.ResultCnt);
                        			
				contractItemModel.ResultCD    = remoteData.ResultCD;
                        
				_log.Debug("-----------------------------------------");
				_log.Debug("자료첨부수정 End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				contractItemModel.ResultCD    = "3101";
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":SetFileUpdate():" + fe.ErrCode + ":" + fe.ResultMsg);
				_log.Warning("-----------------------------------------");
				throw fe;
			}
			catch(Exception e)
			{
				contractItemModel.ResultCD    = "3101";
				_log.Error("-----------------------------------------");
				_log.Exception(e);
				_log.Error("-----------------------------------------");
				throw new FrameException(e.Message);
			}
		}

		public void SetFileDelete(ContractItemModel contractItemModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("자료첨부삭제 Start");
				_log.Debug("-----------------------------------------");
                                        
				// 웹서비스 인스턴스 생성
				ContractItemServicePloxy.ContractItemService svc = new ContractItemServicePloxy.ContractItemService();
				svc.Url = _WebServiceUrl;        				
				// 리모트 모델 생성
				ContractItemServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.ContractItemServicePloxy.HeaderModel();
				ContractItemServicePloxy.ContractItemModel remoteData   = new AdManagerClient.ContractItemServicePloxy.ContractItemModel();
                        
				// 헤더정보 셋트
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;
                        
				// 호출정보셋트
				//Key 설정
				remoteData.ItemNo            = contractItemModel.ItemNo;
				remoteData.FileNo            = contractItemModel.FileNo;
								                
				// 웹서비스 호출 타임아웃설정
				svc.Timeout = FrameSystem.m_SystemTimeout;
				// 웹서비스 메소드 호출
				remoteData = svc.SetFileDelete(remoteHeader, remoteData);

				if( null != svc)
				{
					svc.Dispose();
					svc = null;
				}
                        
				// 결과코드검사
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}
                        
				// 결과 셋트
				contractItemModel.ResultCnt   = remoteData.ResultCnt;
				_log.Debug("contractItemModel.ResultCnt = "+contractItemModel.ResultCnt);
                        			
				contractItemModel.ResultCD    = remoteData.ResultCD;
                        
				_log.Debug("-----------------------------------------");
				_log.Debug("자료첨부삭제 End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				contractItemModel.ResultCD    = "3101";
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":SetFileDelete():" + fe.ErrCode + ":" + fe.ResultMsg);
				_log.Warning("-----------------------------------------");
				throw fe;
			}
			catch(Exception e)
			{
				contractItemModel.ResultCD    = "3101";
				_log.Error("-----------------------------------------");
				_log.Exception(e);
				_log.Error("-----------------------------------------");
				throw new FrameException(e.Message);
			}
		}
		   
		/// <summary>
		/// 광고내역 신규추가
		/// </summary>
		/// <param name="contractItemModel"></param>
        public void SetContractItemAdd(ContractItemModel contractItemModel)
        {
            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug("광고내역추가 Start");
                _log.Debug("-----------------------------------------");
                        
                // 입력데이터의 Validation 검사
                if(contractItemModel.ItemName.Trim().Length < 1)    throw new FrameException("광고 내역명이 존재하지 않습니다.");
                if(contractItemModel.ItemName.Trim().Length > 50)   throw new FrameException("광고 내역명은 50Byte를 초과할 수 없습니다.");
                        
                // 웹서비스 인스턴스 생성
                ContractItemServicePloxy.ContractItemService svc = new ContractItemServicePloxy.ContractItemService();
                svc.Url = _WebServiceUrl;        				
                // 리모트 모델 생성
                ContractItemServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.ContractItemServicePloxy.HeaderModel();
                ContractItemServicePloxy.ContractItemModel remoteData   = new AdManagerClient.ContractItemServicePloxy.ContractItemModel();
                        
                // 헤더정보 셋트
                remoteHeader.ClientKey     = Header.ClientKey;
                remoteHeader.UserID        = Header.UserID;
                        
                // 호출정보셋트
                //Key 설정
                remoteData.MediaCode            = contractItemModel.MediaCode;
                remoteData.RapCode              = contractItemModel.RapCode;        
                remoteData.AgencyCode           = contractItemModel.AgencyCode;     
                remoteData.AdvertiserCode       = contractItemModel.AdvertiserCode; 
                remoteData.ContractSeq          = contractItemModel.ContractSeq;          
                
                remoteData.ItemName             = contractItemModel.ItemName;   
                remoteData.AdState              = contractItemModel.AdState;   
                remoteData.ExcuteStartDay       = contractItemModel.ExcuteStartDay;     
                remoteData.ExcuteEndDay         = contractItemModel.ExcuteEndDay;    
                remoteData.RealEndDay           = contractItemModel.RealEndDay;     
                remoteData.ScheduleType         = contractItemModel.ScheduleType;    
                remoteData.AdClass              = contractItemModel.AdClass;     
                remoteData.AdType               = contractItemModel.AdType;    
                remoteData.AdRate               = contractItemModel.AdRate;     
                remoteData.AdTime               = contractItemModel.AdTime;    
				remoteData.LinkChannel          = contractItemModel.LinkChannel;   				
				remoteData.Mgrade               = contractItemModel.Mgrade;  
				remoteData.HomeYn               = contractItemModel.HomeYn;   
				remoteData.ChannelYn            = contractItemModel.ChannelYn;   
                remoteData.CugYn		        = contractItemModel.CugYn;
				remoteData.STBType              = contractItemModel.STBType;
                // 웹서비스 호출 타임아웃설정
                svc.Timeout = FrameSystem.m_SystemTimeout;
                // 웹서비스 메소드 호출
                remoteData = svc.SetContractItemCreate(remoteHeader, remoteData);

				if( null != svc)
				{
					svc.Dispose();
					svc = null;
				}
                        
                // 결과코드검사
                if(!remoteData.ResultCD.Equals("0000"))
                {
                    throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
                }
                        
                // 결과 셋트
                contractItemModel.ResultCnt   = remoteData.ResultCnt;
                _log.Debug("contractItemModel.ResultCnt = "+contractItemModel.ResultCnt);
                        			
                contractItemModel.ResultCD    = remoteData.ResultCD;
                        
                _log.Debug("-----------------------------------------");
                _log.Debug("광고내역추가 End");
                _log.Debug("-----------------------------------------");
            }
            catch(FrameException fe)
            {
                contractItemModel.ResultCD    = "3101";
                _log.Warning("-----------------------------------------");
                _log.Warning( this.ToString() + ":SetContractItemCreate():" + fe.ErrCode + ":" + fe.ResultMsg);
                _log.Warning("-----------------------------------------");
                throw fe;
            }
            catch(Exception e)
            {
                contractItemModel.ResultCD    = "3101";
                _log.Error("-----------------------------------------");
                _log.Exception(e);
                _log.Error("-----------------------------------------");
                throw new FrameException(e.Message);
            }
        }

        /// <summary>
        /// 광고내역정보 수정
        /// </summary>
        /// <param name="contractItemModel"></param>
        public void SetContractItemUpdate(ContractItemModel contractItemModel)
        {
            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug("광고내역정보수정 Start");
                _log.Debug("-----------------------------------------");
                        
                        
                // 입력데이터의 Validation 검사
                if(contractItemModel.ItemName.Trim().Length < 1) 
                {
                    throw new FrameException("광고 내역명이 존재하지 않습니다.");
                }
                if(contractItemModel.ItemName.Trim().Length > 50) 
                {
                    throw new FrameException("광고 내역명은 50Byte를 초과할 수 없습니다.");
                }
                      
                        	
                        
                // 웹서비스 인스턴스 생성
                ContractItemServicePloxy.ContractItemService svc = new ContractItemServicePloxy.ContractItemService();
                svc.Url = _WebServiceUrl;                       			
                // 리모트 모델 생성
                ContractItemServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.ContractItemServicePloxy.HeaderModel();
                ContractItemServicePloxy.ContractItemModel remoteData   = new AdManagerClient.ContractItemServicePloxy.ContractItemModel();
                        
                // 헤더정보 셋트
                remoteHeader.ClientKey     = Header.ClientKey;
                remoteHeader.UserID        = Header.UserID;
                        
                // 호출정보셋트
                //Key 설정     
				remoteData.MediaCode            = contractItemModel.MediaCode;   
				remoteData.ItemNo               = contractItemModel.ItemNo;   

                remoteData.ItemName             = contractItemModel.ItemName;   
                remoteData.AdState              = contractItemModel.AdState;   
                remoteData.ExcuteStartDay       = contractItemModel.ExcuteStartDay;     
                remoteData.ExcuteEndDay         = contractItemModel.ExcuteEndDay;    
                remoteData.RealEndDay           = contractItemModel.RealEndDay;     
                remoteData.ScheduleType         = contractItemModel.ScheduleType;    
                remoteData.AdClass              = contractItemModel.AdClass;     
                remoteData.AdType               = contractItemModel.AdType;    
                remoteData.AdRate               = contractItemModel.AdRate;     
                remoteData.AdTime               = contractItemModel.AdTime;    
				remoteData.LinkChannel          = contractItemModel.LinkChannel;   				 
				remoteData.Mgrade               = contractItemModel.Mgrade;   
				remoteData.HomeYn               = contractItemModel.HomeYn;   
				remoteData.ChannelYn            = contractItemModel.ChannelYn;   
				remoteData.CugYn                = contractItemModel.CugYn;
				remoteData.STBType = contractItemModel.STBType;
				// 광고종류변경 구분
				remoteData.AdTypeChangeType		= contractItemModel.AdTypeChangeType;

                        				
                // 웹서비스 호출 타임아웃설정
                svc.Timeout = FrameSystem.m_SystemTimeout;
                // 웹서비스 메소드 호출
                remoteData = svc.SetContractItemUpdate(remoteHeader, remoteData);

				if( null != svc)
				{
					svc.Dispose();
					svc = null;
				}
                        
                // 결과코드검사
                if(!remoteData.ResultCD.Equals("0000"))
                {
                    throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
                }
                        
                // 결과 셋트
                contractItemModel.ResultCnt   = remoteData.ResultCnt;
                contractItemModel.ResultCD    = remoteData.ResultCD;
                        
                _log.Debug("-----------------------------------------");
                _log.Debug("광고내역정보수정 End");
                _log.Debug("-----------------------------------------");
            }
            catch(FrameException fe)
            {
                _log.Warning("-----------------------------------------");
                _log.Warning( this.ToString() + ":SetContractItemUpdate():" + fe.ErrCode + ":" + fe.ResultMsg);
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

		public void SetLinkChannelDelete(ContractItemModel contractItemModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("연결채널삭제 Start");
				_log.Debug("-----------------------------------------");
                				                     
                        
				// 웹서비스 인스턴스 생성
				ContractItemServicePloxy.ContractItemService svc = new ContractItemServicePloxy.ContractItemService();
				svc.Url = _WebServiceUrl;        				
				// 리모트 모델 생성
				ContractItemServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.ContractItemServicePloxy.HeaderModel();
				ContractItemServicePloxy.ContractItemModel remoteData   = new AdManagerClient.ContractItemServicePloxy.ContractItemModel();
                        
				// 헤더정보 셋트
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;
                        
				// 호출정보셋트
				//Key 설정

				remoteData.ItemNo			= contractItemModel.ItemNo;
                remoteData.LinkChannel      = contractItemModel.LinkChannel;
                remoteData.LinkType         = contractItemModel.LinkType;	
			
                
				// 웹서비스 호출 타임아웃설정
				svc.Timeout = FrameSystem.m_SystemTimeout;
				// 웹서비스 메소드 호출
				remoteData = svc.SetLinkChannelDelete2(remoteHeader, remoteData);

				if( null != svc)
				{
					svc.Dispose();
					svc = null;
				}
                        
				// 결과코드검사
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}
                        
				// 결과 셋트
				contractItemModel.ResultCnt   = remoteData.ResultCnt;
				_log.Debug("contractItemModel.ResultCnt = "+contractItemModel.ResultCnt);
                        			
				contractItemModel.ResultCD    = remoteData.ResultCD;
                        
				_log.Debug("-----------------------------------------");
				_log.Debug("연결채널삭제 End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				contractItemModel.ResultCD    = "3101";
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":SetLinkChannelDelete():" + fe.ErrCode + ":" + fe.ResultMsg);
				_log.Warning("-----------------------------------------");
				throw fe;
			}
			catch(Exception e)
			{
				contractItemModel.ResultCD    = "3101";
				_log.Error("-----------------------------------------");
				_log.Exception(e);
				_log.Error("-----------------------------------------");
				throw new FrameException(e.Message);
			}
		}

        public void SetLinkChannelCreate(ContractItemModel contractItemModel)
        {
            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug("연결채널추가 Start");
                _log.Debug("-----------------------------------------");


                // 웹서비스 인스턴스 생성
                ContractItemServicePloxy.ContractItemService svc = new ContractItemServicePloxy.ContractItemService();
                svc.Url = _WebServiceUrl;
                // 리모트 모델 생성
                ContractItemServicePloxy.HeaderModel remoteHeader = new AdManagerClient.ContractItemServicePloxy.HeaderModel();
                ContractItemServicePloxy.ContractItemModel remoteData = new AdManagerClient.ContractItemServicePloxy.ContractItemModel();

                // 헤더정보 셋트
                remoteHeader.ClientKey = Header.ClientKey;
                remoteHeader.UserID = Header.UserID;

                // 호출정보셋트
                //Key 설정

                remoteData.ItemNo = contractItemModel.ItemNo;
                remoteData.LinkChannel = contractItemModel.LinkChannel;
                remoteData.LinkType = contractItemModel.LinkType;

                // 웹서비스 호출 타임아웃설정
                svc.Timeout = FrameSystem.m_SystemTimeout;
                // 웹서비스 메소드 호출
                remoteData = svc.SetLinkChannelCreate2(remoteHeader, remoteData);

                if (null != svc)
                {
                    svc.Dispose();
                    svc = null;
                }

                // 결과코드검사
                if (!remoteData.ResultCD.Equals("0000"))
                {
                    throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
                }

                // 결과 셋트
                contractItemModel.ResultCnt = remoteData.ResultCnt;
                _log.Debug("contractItemModel.ResultCnt = " + contractItemModel.ResultCnt);

                contractItemModel.ResultCD = remoteData.ResultCD;

                _log.Debug("-----------------------------------------");
                _log.Debug("연결채널추가 End");
                _log.Debug("-----------------------------------------");
            }
            catch (FrameException fe)
            {
                contractItemModel.ResultCD = "3101";
                _log.Warning("-----------------------------------------");
                _log.Warning(this.ToString() + ":SetLinkChannelCreate():" + fe.ErrCode + ":" + fe.ResultMsg);
                _log.Warning("-----------------------------------------");
                throw fe;
            }
            catch (Exception e)
            {
                contractItemModel.ResultCD = "3101";
                _log.Error("-----------------------------------------");
                _log.Exception(e);
                _log.Error("-----------------------------------------");
                throw new FrameException(e.Message);
            }
        }

        /// <summary>
        /// 광고계약내역 삭제
        /// </summary>
        /// <param name="baseModel"></param>
        public void SetContractItemDelete(ContractItemModel contractItemModel)
        {
            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug("광고계약삭제 Start");
                _log.Debug("-----------------------------------------");

                // 웹서비스 인스턴스 생성
                ContractItemServicePloxy.ContractItemService svc = new ContractItemServicePloxy.ContractItemService();
              
                // URL의 동적셋트
                svc.Url = _WebServiceUrl;
				
                // 리모트 모델 생성
                ContractItemServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.ContractItemServicePloxy.HeaderModel();
                ContractItemServicePloxy.ContractItemModel remoteData   = new AdManagerClient.ContractItemServicePloxy.ContractItemModel();
              
                // 헤더정보 셋트
                remoteHeader.ClientKey  = Header.ClientKey;
                remoteHeader.UserID     = Header.UserID;

                // 호출정보셋트
                remoteData.ItemNo       = contractItemModel.ItemNo;
                // 웹서비스 호출 타임아웃설정
                svc.Timeout = FrameSystem.m_SystemTimeout;	
                // 웹서비스 메소드 호출
                remoteData = svc.SetContractItemDelete(remoteHeader, remoteData);

                // 결과코드검사
                if(!remoteData.ResultCD.Equals("0000"))
                {
                    throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
                }

                // 결과 셋트
                contractItemModel.ResultCnt   = remoteData.ResultCnt;
                contractItemModel.ResultCD    = remoteData.ResultCD;

                _log.Debug("-----------------------------------------");
                _log.Debug("광고계약삭제 End");
                _log.Debug("-----------------------------------------");
            }
            catch(FrameException fe)
            {
                _log.Warning("-----------------------------------------");
                _log.Warning( this.ToString() + ":SetContractItemDelete():" + fe.ErrCode + ":" + fe.ResultMsg);
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
		/// 광고내역 복사 
		/// S1에서 추가
		/// </summary>
		/// <param name="data"></param>
		public void SetContractItemCopy(ItemCopyModel	data)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("광고내역복사 Start");
				_log.Debug("-----------------------------------------");
                        
				// 입력데이터의 Validation 검사
				//if(contractItemModel.ItemName.Trim().Length < 1)    throw new FrameException("광고 내역명이 존재하지 않습니다.");
				//if(contractItemModel.ItemName.Trim().Length > 50)   throw new FrameException("광고 내역명은 50Byte를 초과할 수 없습니다.");
                      
                        
				// 웹서비스 인스턴스 생성
				ContractItemServicePloxy.ContractItemService svc = new ContractItemServicePloxy.ContractItemService();
				svc.Url = _WebServiceUrl;        				
				// 리모트 모델 생성
				ContractItemServicePloxy.HeaderModel	remoteHeader = new AdManagerClient.ContractItemServicePloxy.HeaderModel();
				ContractItemServicePloxy.ItemCopyModel	remoteData   = new AdManagerClient.ContractItemServicePloxy.ItemCopyModel();
                        
				// 헤더정보 셋트
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;
                        
				// 호출정보셋트
				remoteData.ItemNoSou		=	data.ItemNoSou;
				remoteData.ItemName			=	data.ItemName;
				remoteData.ExcuteStartDay	=	data.ExcuteStartDay;
				remoteData.ExcuteEndDay     =	data.ExcuteEndDay;

				remoteData.ItemNoDes		=	0;

                
				svc.Timeout = FrameSystem.m_SystemTimeout;
				remoteData = svc.SetContractItemCopy(remoteHeader, remoteData);

				if( null != svc)
				{
					svc.Dispose();
					svc = null;
				}
                        
				// 결과코드검사
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}
                        
				// 결과 셋트
				data.ItemNoDes	=	remoteData.ItemNoDes;
				data.ResultCnt	= remoteData.ResultCnt;
				data.ResultCD    = remoteData.ResultCD;
                        
				_log.Debug("-----------------------------------------");
				_log.Debug("광고내역복사 End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				data.ResultCD   = "3101";
				data.ItemNoDes	= 0;
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":SetContractItemCreate():" + fe.ErrCode + ":" + fe.ResultMsg);
				_log.Warning("-----------------------------------------");
				throw fe;
			}
			catch(Exception e)
			{
				data.ResultCD    = "3101";
				_log.Error("-----------------------------------------");
				_log.Exception(e);
				_log.Error("-----------------------------------------");
				throw new FrameException(e.Message);
			}
		}
        
        /// <summary>
        /// 광고상태 일괄변경처리 Manager
        /// </summary>        
		public void SetMultiAdState(ContractItemModel contractItemModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("광고상태 일괄수정 Start");
				_log.Debug("-----------------------------------------");
                        
                        
				if (contractItemModel.ContractItemDataSet.Tables[0].Rows.Count == 0)
                    throw new Exception("처리할 Data가 선택되지 않았습니다.");                          	
                        
				// 웹서비스 인스턴스 생성
				ContractItemServicePloxy.ContractItemService svc = new ContractItemServicePloxy.ContractItemService();
				svc.Url = _WebServiceUrl;                       			
				// 리모트 모델 생성
				ContractItemServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.ContractItemServicePloxy.HeaderModel();
				ContractItemServicePloxy.ContractItemModel remoteData   = new AdManagerClient.ContractItemServicePloxy.ContractItemModel();
                        
				// 헤더정보 셋트
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;
                        
				// 호출정보셋트																
				remoteData.AdState              = contractItemModel.AdState;   
				remoteData.ContractItemDataSet  = contractItemModel.ContractItemDataSet.Copy();
								                        				
				// 웹서비스 호출 타임아웃설정
				svc.Timeout = FrameSystem.m_SystemTimeout;
			    
				// 웹서비스 메소드 호출
				remoteData = svc.SetMultiAdState(remoteHeader, remoteData);
                
				if( null != svc)
				{
					svc.Dispose();
					svc = null;
				}
                        
				// 결과코드검사
				if(!remoteData.ResultCD.Equals(FrameSystem.DBSuccess))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}
                        
				// 결과 셋트
				contractItemModel.ResultCnt   = remoteData.ResultCnt;
				contractItemModel.ResultCD    = remoteData.ResultCD;
                        
				_log.Debug("-----------------------------------------");
				_log.Debug("광고상태 일괄수정 End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":SetMultiAdState():" + fe.ErrCode + ":" + fe.ResultMsg);
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
		/// 광고연결채널 일괄처리 Manager
		/// </summary>		
		public void SetMultiChannel(ContractItemModel contractItemModel)
		{
		}

        /// <summary>
        /// 듀얼광고 설정 삭제( 1106에서 추가)
        /// </summary>
        /// <param name="contractItemModel"></param>
        public void SetLinkItemDelete(ContractItemModel contractItemModel)
        {
            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug("듀얼광고삭제 Start");
                _log.Debug("-----------------------------------------");


                // 웹서비스 인스턴스 생성
                ContractItemServicePloxy.ContractItemService svc = new ContractItemServicePloxy.ContractItemService();
                svc.Url = _WebServiceUrl;
                // 리모트 모델 생성
                ContractItemServicePloxy.HeaderModel remoteHeader = new AdManagerClient.ContractItemServicePloxy.HeaderModel();
                ContractItemServicePloxy.ContractItemModel remoteData = new AdManagerClient.ContractItemServicePloxy.ContractItemModel();

                // 헤더정보 셋트
                remoteHeader.ClientKey = Header.ClientKey;
                remoteHeader.UserID = Header.UserID;

                // 연결할 로딩광고번호는 LinkChannel항목을 사용한다.
                remoteData.ItemNo = contractItemModel.ItemNo;
                remoteData.LinkChannel = contractItemModel.LinkChannel;

                // 웹서비스 호출 타임아웃설정
                svc.Timeout = FrameSystem.m_SystemTimeout;
                // 웹서비스 메소드 호출
                remoteData = svc.SetLinkItemDelete(remoteHeader, remoteData);

                if (null != svc)
                {
                    svc.Dispose();
                    svc = null;
                }

                // 결과코드검사
                if (!remoteData.ResultCD.Equals("0000"))
                {
                    throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
                }

                // 결과 셋트
                contractItemModel.ResultCnt = remoteData.ResultCnt;
                _log.Debug("contractItemModel.ResultCnt = " + contractItemModel.ResultCnt);

                contractItemModel.ResultCD = remoteData.ResultCD;

                _log.Debug("-----------------------------------------");
                _log.Debug("듀얼광고삭제 End");
                _log.Debug("-----------------------------------------");
            }
            catch (FrameException fe)
            {
                contractItemModel.ResultCD = "3101";
                _log.Warning("-----------------------------------------");
                _log.Warning(this.ToString() + ":SetLinkItemDelete():" + fe.ErrCode + ":" + fe.ResultMsg);
                _log.Warning("-----------------------------------------");
                throw fe;
            }
            catch (Exception e)
            {
                contractItemModel.ResultCD = "3101";
                _log.Error("-----------------------------------------");
                _log.Exception(e);
                _log.Error("-----------------------------------------");
                throw new FrameException(e.Message);
            }
        }

        /// <summary>
        /// 듀얼광고 설정 추가( 1106에서 추가)
        /// </summary>
        /// <param name="contractItemModel"></param>
        public void SetLinkItemCreate(ContractItemModel contractItemModel)
        {
            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug("듀얼광고 설정 Start");
                _log.Debug("-----------------------------------------");


                // 웹서비스 인스턴스 생성
                ContractItemServicePloxy.ContractItemService svc = new ContractItemServicePloxy.ContractItemService();
                svc.Url = _WebServiceUrl;
                // 리모트 모델 생성
                ContractItemServicePloxy.HeaderModel remoteHeader = new AdManagerClient.ContractItemServicePloxy.HeaderModel();
                ContractItemServicePloxy.ContractItemModel remoteData = new AdManagerClient.ContractItemServicePloxy.ContractItemModel();

                // 헤더정보 셋트
                remoteHeader.ClientKey = Header.ClientKey;
                remoteHeader.UserID = Header.UserID;

                // 연결할 로딩광고번호는 LinkChannel항목을 사용한다.
                remoteData.ItemNo = contractItemModel.ItemNo;
                remoteData.LinkChannel = contractItemModel.LinkChannel;

                // 웹서비스 호출 타임아웃설정
                svc.Timeout = FrameSystem.m_SystemTimeout;
                remoteData = svc.SetLinkItemCreate(remoteHeader, remoteData);

                if (null != svc)
                {
                    svc.Dispose();
                    svc = null;
                }

                // 결과코드검사
                if (!remoteData.ResultCD.Equals("0000"))
                {
                    throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
                }

                // 결과 셋트
                contractItemModel.ResultCnt = remoteData.ResultCnt;
                _log.Debug("contractItemModel.ResultCnt = " + contractItemModel.ResultCnt);

                contractItemModel.ResultCD = remoteData.ResultCD;

                _log.Debug("-----------------------------------------");
                _log.Debug("듀얼광고 End");
                _log.Debug("-----------------------------------------");
            }
            catch (FrameException fe)
            {
                contractItemModel.ResultCD = "3101";
                _log.Warning("-----------------------------------------");
                _log.Warning(this.ToString() + ":SetLinkItemCreate():" + fe.ErrCode + ":" + fe.ResultMsg);
                _log.Warning("-----------------------------------------");
                throw fe;
            }
            catch (Exception e)
            {
                contractItemModel.ResultCD = "3101";
                _log.Error("-----------------------------------------");
                _log.Exception(e);
                _log.Error("-----------------------------------------");
                throw new FrameException(e.Message);
            }
        }
    }
}