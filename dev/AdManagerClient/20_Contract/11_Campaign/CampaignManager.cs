// ===============================================================================
// UserUpdate Manager  for Charites Project
//
// UserUpdateManager.cs
//
// 광고계약정보 저장 서비스를 호출합니다. 
//
// ===============================================================================
// Release history
// 2007.06.26 송명환 v1.0
// 2007.10.03 RH.Jung 목록조회 메소드 추가
// ===============================================================================
// Copyright (C) 2007 G-Inno Systems Inc.
// All rights reserved.
// ==============================================================================
/*
 * -------------------------------------------------------
 * [수정사항]
 * -------------------------------------------------------
 * 수정코드  : [A_01]
 * 수정자    : JH.Kim
 * 수정일    : 2015.11.
 * 수정내용  : 영업관리 대상 플래그 추가
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
	/// <summary>
	/// 광고계약정보 웹서비스를 호출합니다. 
	/// </summary>
	public class CampaignManager : BaseManager
	{
		/// <summary>
		/// 생성자
		/// </summary>
		/// <param name="systemModel"></param>
		/// <param name="commonModel"></param>

		public CampaignManager(SystemModel systemModel, CommonModel commonModel) : base(systemModel, commonModel)
		{
			_log = FrameSystem.oLog;
			_module = "CONTENT";
			_Host  = FrameSystem.m_WebServer_Host;
			_Port  = FrameSystem.m_WebServer_Port;
			_Path  = FrameSystem.m_WebServer_App + "/Contract/CampaignService.asmx";
       
		}

		/// <summary>
		/// 광고계약정보조회
		/// </summary>
		/// <param name="campaignModel"></param>
		public void GetContractList(CampaignModel campaignModel)
		{

			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("광고계약목록조회 Start");
				_log.Debug("-----------------------------------------");
				
				// 웹서비스 인스턴스 생성
				CampaignServicePloxy.CampaignService svc = new CampaignServicePloxy.CampaignService();

				svc.Url = _WebServiceUrl;

				// 리모트 모델 생성
				CampaignServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.CampaignServicePloxy.HeaderModel();
				CampaignServicePloxy.CampaignModel remoteData   = new AdManagerClient.CampaignServicePloxy.CampaignModel();

				// 헤더정보 셋트
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;

				// 호출정보 셋트
				remoteData.SearchKey       = campaignModel.SearchKey;
				remoteData.MediaCode       = campaignModel.MediaCode;
				remoteData.RapCode         = campaignModel.RapCode;        
				remoteData.AgencyCode      = campaignModel.AgencyCode;     
				remoteData.AdvertiserCode  = campaignModel.AdvertiserCode; 
				remoteData.State           = campaignModel.State;          
				
				// 웹서비스 호출 타임아웃설정
				svc.Timeout = FrameSystem.m_SystemTimeout;

				// 웹서비스 메소드 호출
				remoteData = svc.GetContractList(remoteHeader, remoteData);

				// 결과코드검사
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// 결과 셋트
				campaignModel.ContractDataSet = remoteData.ContractDataSet.Copy();
				campaignModel.ResultCnt   = remoteData.ResultCnt;
				campaignModel.ResultCD    = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("광고계약목록조회 End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":GetContractList():" + fe.ErrCode + ":" + fe.ResultMsg);
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
   

		// 2007.10.03 RH.Jung 목록조회 메소드 추가
		/// <summary>
		/// 광고계약정보조회2
		/// </summary>
		/// <param name="campaignModel"></param>
		public void GetContractList2(CampaignModel campaignModel)
		{

			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("광고계약목록조회2 Start");
				_log.Debug("-----------------------------------------");
				
				// 웹서비스 인스턴스 생성
				CampaignServicePloxy.CampaignService svc = new CampaignServicePloxy.CampaignService();

				svc.Url = _WebServiceUrl;

				// 리모트 모델 생성
				CampaignServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.CampaignServicePloxy.HeaderModel();
				CampaignServicePloxy.CampaignModel remoteData   = new AdManagerClient.CampaignServicePloxy.CampaignModel();

				// 헤더정보 셋트
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;

				// 호출정보 셋트
				remoteData.SearchKey       = campaignModel.SearchKey;
				remoteData.SearchState_10  = campaignModel.SearchState_10;          
				remoteData.SearchState_20  = campaignModel.SearchState_20;          
				remoteData.MediaCode       = campaignModel.MediaCode;
				remoteData.RapCode         = campaignModel.RapCode;        
				remoteData.AgencyCode      = campaignModel.AgencyCode;     
				
				// 웹서비스 호출 타임아웃설정
				svc.Timeout = FrameSystem.m_SystemTimeout;

				// 웹서비스 메소드 호출
				remoteData = svc.GetContractList2(remoteHeader, remoteData);

				// 결과코드검사
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// 결과 셋트
				campaignModel.ContractDataSet = remoteData.ContractDataSet.Copy();
				campaignModel.ResultCnt   = remoteData.ResultCnt;
				campaignModel.ResultCD    = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("광고계약목록조회2 End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":GetContractList2():" + fe.ErrCode + ":" + fe.ResultMsg);
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
		/// 광고계약정보조회
		/// </summary>
		/// <param name="campaignModel"></param>
		public void GetContractItemList(CampaignModel campaignModel)
		{

			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("관리내역목록조회 Start");
				_log.Debug("-----------------------------------------");
				
				// 웹서비스 인스턴스 생성
				CampaignServicePloxy.CampaignService svc = new CampaignServicePloxy.CampaignService();

				svc.Url = _WebServiceUrl;

				// 리모트 모델 생성
				CampaignServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.CampaignServicePloxy.HeaderModel();
				CampaignServicePloxy.CampaignModel remoteData   = new AdManagerClient.CampaignServicePloxy.CampaignModel();

				// 헤더정보 셋트
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;

				// 호출정보 셋트
				remoteData.SearchKey             = campaignModel.SearchKey;
				remoteData.MediaCode             = campaignModel.MediaCode;
				remoteData.RapCode               = campaignModel.RapCode;        
				remoteData.AgencyCode            = campaignModel.AgencyCode;     
				remoteData.AdvertiserCode        = campaignModel.AdvertiserCode; 
				remoteData.CampaignCode			 = campaignModel.CampaignCode; 
								
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
				campaignModel.ContractDataSet = remoteData.ContractDataSet.Copy();  // 캠페인 하위 광고
                campaignModel.CampaignDataSet = remoteData.CampaignDataSet.Copy();  // 캠페인 하위 팝업
				campaignModel.ResultCnt   = remoteData.ResultCnt;
				campaignModel.ResultCD    = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("관리내역목록조회 End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":GetContractList():" + fe.ErrCode + ":" + fe.ResultMsg);
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
        /// 캠페인 대상 광고내역 목록 읽기
        /// </summary>
        /// <param name="campaignModel"></param>
		public void GetContractItemPopList(CampaignModel campaignModel)
		{

			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("관리내역목록조회 Start");
				_log.Debug("-----------------------------------------");
				
				// 웹서비스 인스턴스 생성
				CampaignServicePloxy.CampaignService svc = new CampaignServicePloxy.CampaignService();

				svc.Url = _WebServiceUrl;

				// 리모트 모델 생성
				CampaignServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.CampaignServicePloxy.HeaderModel();
				CampaignServicePloxy.CampaignModel remoteData   = new AdManagerClient.CampaignServicePloxy.CampaignModel();

				// 헤더정보 셋트
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;
				remoteHeader.UserLevel     = Header.UserLevel;
				remoteHeader.UserClass     = Header.UserClass;

				// 호출정보 셋트
				remoteData.SearchKey             =  campaignModel.SearchKey;               
				remoteData.SearchMediaCode		 =  campaignModel.SearchMediaCode;	  
				remoteData.SearchRapCode		 =  campaignModel.SearchRapCode;       
				remoteData.SearchAgencyCode	     =  campaignModel.SearchAgencyCode;    
				remoteData.SearchAdvertiserCode  =  campaignModel.SearchAdvertiserCode;
				remoteData.ContractSeq			 =  campaignModel.ContractSeq;
				remoteData.SearchchkAdState_10	 =  campaignModel.SearchchkAdState_10; 
				remoteData.SearchchkAdState_20	 =  campaignModel.SearchchkAdState_20; 
				remoteData.SearchchkAdState_30	 =  campaignModel.SearchchkAdState_30; 
				remoteData.SearchchkAdState_40	 =  campaignModel.SearchchkAdState_40; 
								
				// 웹서비스 호출 타임아웃설정
				svc.Timeout = FrameSystem.m_SystemTimeout;

				// 웹서비스 메소드 호출
				remoteData = svc.GetContractItemPopList(remoteHeader, remoteData);

				// 결과코드검사
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// 결과 셋트
				campaignModel.ContractDataSet = remoteData.ContractDataSet.Copy();
				campaignModel.ResultCnt   = remoteData.ResultCnt;
				campaignModel.ResultCD    = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("관리내역목록조회 End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":GetContractItemPopList():" + fe.ErrCode + ":" + fe.ResultMsg);
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
        /// 캠페인 대상 팝업내역 목록 읽기
        /// </summary>
        /// <param name="campaignModel"></param>
        public void GetPnsPopList(CampaignModel campaignModel)
        {
            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug("팝업내역목록조회 Start");
                _log.Debug("-----------------------------------------");

                // 웹서비스 인스턴스 생성
                CampaignServicePloxy.CampaignService svc = new CampaignServicePloxy.CampaignService();

                // 리모트 모델 생성
                CampaignServicePloxy.HeaderModel remoteHeader = new AdManagerClient.CampaignServicePloxy.HeaderModel();
                CampaignServicePloxy.CampaignModel remoteData = new AdManagerClient.CampaignServicePloxy.CampaignModel();

                // 헤더정보 셋트
                remoteHeader.ClientKey = Header.ClientKey;
                remoteHeader.UserID = Header.UserID;
                remoteHeader.UserLevel = Header.UserLevel;
                remoteHeader.UserClass = Header.UserClass;

                // 호출정보 셋트
                remoteData.SearchKey = campaignModel.SearchKey;
                remoteData.SearchchkAdState_10 = campaignModel.SearchchkAdState_10;
                remoteData.SearchchkAdState_20 = campaignModel.SearchchkAdState_20;

                // 웹서비스 호출 설정
                svc.Url = _WebServiceUrl;
                svc.Timeout = FrameSystem.m_SystemTimeout;

                // 웹서비스 메소드 호출
                remoteData = svc.svcGetPnsList(remoteHeader, remoteData);

                // 결과코드검사
                if (!remoteData.ResultCD.Equals("0000"))
                {
                    throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
                }

                // 결과 셋트
                campaignModel.ContractDataSet = remoteData.ContractDataSet.Copy();
                campaignModel.ResultCnt = remoteData.ResultCnt;
                campaignModel.ResultCD = remoteData.ResultCD;

                _log.Debug("-----------------------------------------");
                _log.Debug("팝업내역목록조회 End");
                _log.Debug("-----------------------------------------");
            }
            catch (FrameException fe)
            {
                _log.Warning("-----------------------------------------");
                _log.Warning(this.ToString() + ":GetPnsPopList():" + fe.ErrCode + ":" + fe.ResultMsg);
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

		public void SetCampaignCreate(CampaignModel campaignModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("캠페인추가 Start");
				_log.Debug("-----------------------------------------");
                        
				// 입력데이터의 Validation 검사
				if(campaignModel.CampaignName.Trim().Length < 1) 
				{
					throw new FrameException("캠페인명이 존재하지 않습니다.");
				}
				if(campaignModel.CampaignName.Trim().Length > 1000) 
				{
					throw new FrameException("캠페인명은 1000Byte를 초과할 수 없습니다.");
				}
				
				// 웹서비스 인스턴스 생성
				CampaignServicePloxy.CampaignService svc = new CampaignServicePloxy.CampaignService();
				svc.Url = _WebServiceUrl;			
				// 리모트 모델 생성
				CampaignServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.CampaignServicePloxy.HeaderModel();
				CampaignServicePloxy.CampaignModel remoteData   = new AdManagerClient.CampaignServicePloxy.CampaignModel();
                        
				// 헤더정보 셋트
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;
                        
				// 호출정보셋트
				remoteData.MediaCode       = campaignModel.MediaCode;				     
				remoteData.ContractSeq     = campaignModel.ContractSeq;     
				remoteData.CampaignName    = campaignModel.CampaignName; 
				remoteData.Price		   = campaignModel.Price;
                remoteData.BizManageTarget = campaignModel.BizManageTarget; // [A_01]
				                            
				// 웹서비스 호출 타임아웃설정
				svc.Timeout = FrameSystem.m_SystemTimeout;
				// 웹서비스 메소드 호출
				remoteData = svc.SetCampaignCreate(remoteHeader, remoteData);
                        
				// 결과코드검사
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}
                        
				// 결과 셋트
				campaignModel.ResultCnt   = remoteData.ResultCnt;
				_log.Debug("campaignModel.ResultCnt = "+campaignModel.ResultCnt);
                        			
				campaignModel.ResultCD    = remoteData.ResultCD;
                        
				_log.Debug("-----------------------------------------");
				_log.Debug("캠페인추가 End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				campaignModel.ResultCD    = "3101";
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":SetCampaignCreate():" + fe.ErrCode + ":" + fe.ResultMsg);
				_log.Warning("-----------------------------------------");
				throw fe;
			}
			catch(Exception e)
			{
				campaignModel.ResultCD    = "3101";
				_log.Error("-----------------------------------------");
				_log.Exception(e);
				_log.Error("-----------------------------------------");
				throw new FrameException(e.Message);
			}
		}

		/// <summary>
		/// 캠페인정보수정
		/// </summary>
		/// <param name="campaignModel"></param>
		public void SetCampaignUpdate(CampaignModel campaignModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("광고계약정보수정 Start");
				_log.Debug("-----------------------------------------");
                        
                        
				// 입력데이터의 Validation 검사
				if(campaignModel.CampaignName.Trim().Length < 1) 
				{
					throw new FrameException("캠페인명이 존재하지 않습니다.");
				}
				if(campaignModel.CampaignName.Trim().Length > 1000) 
				{
					throw new FrameException("캠페인명은 1000Byte를 초과할 수 없습니다.");
				}                        	
                        
				// 웹서비스 인스턴스 생성
				CampaignServicePloxy.CampaignService svc = new CampaignServicePloxy.CampaignService();
				svc.Url = _WebServiceUrl;		
				// 리모트 모델 생성
				CampaignServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.CampaignServicePloxy.HeaderModel();
				CampaignServicePloxy.CampaignModel remoteData   = new AdManagerClient.CampaignServicePloxy.CampaignModel();
                        
				// 헤더정보 셋트
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;
                        
				// 호출정보셋트
				remoteData.MediaCode	   = campaignModel.MediaCode;
				remoteData.CampaignCode    = campaignModel.CampaignCode;        
				remoteData.ContractSeq     = campaignModel.ContractSeq;     
				remoteData.CampaignName	   = campaignModel.CampaignName; 
				remoteData.Price		   = campaignModel.Price;
				remoteData.UseYn           = campaignModel.UseYn;
                remoteData.BizManageTarget = campaignModel.BizManageTarget; // [A_01]
        				
				// 웹서비스 호출 타임아웃설정
				svc.Timeout = FrameSystem.m_SystemTimeout;
				// 웹서비스 메소드 호출
				remoteData = svc.SetCampaignUpdate(remoteHeader, remoteData);
                        
				// 결과코드검사
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}
                        
				// 결과 셋트
				campaignModel.ResultCnt   = remoteData.ResultCnt;
				campaignModel.ResultCD    = remoteData.ResultCD;
                        
				_log.Debug("-----------------------------------------");
				_log.Debug("캠페인정보수정 End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":SetCampaignUpdate():" + fe.ErrCode + ":" + fe.ResultMsg);
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
		/// 캠페인 삭제
		/// </summary>
		/// <param name="baseModel"></param>
		public void SetCampaignDelete(CampaignModel campaignModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("캠페인삭제 Start");
				_log.Debug("-----------------------------------------");

				// 웹서비스 인스턴스 생성
				CampaignServicePloxy.CampaignService svc = new CampaignServicePloxy.CampaignService();
              
				// URL의 동적셋트
				svc.Url = _WebServiceUrl;
				
				// 리모트 모델 생성
				CampaignServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.CampaignServicePloxy.HeaderModel();
				CampaignServicePloxy.CampaignModel remoteData   = new AdManagerClient.CampaignServicePloxy.CampaignModel();
              
				// 헤더정보 셋트
				remoteHeader.ClientKey  = Header.ClientKey;
				remoteHeader.UserID     = Header.UserID;

				// 호출정보셋트
				remoteData.MediaCode		 = campaignModel.MediaCode;
				remoteData.CampaignCode      = campaignModel.CampaignCode;
				remoteData.ContractSeq       = campaignModel.ContractSeq;
					
				// 웹서비스 호출 타임아웃설정
				svc.Timeout = FrameSystem.m_SystemTimeout;
				// 웹서비스 메소드 호출
				remoteData = svc.SetCampaignDelete(remoteHeader, remoteData);

				// 결과코드검사
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// 결과 셋트
				campaignModel.ResultCnt   = remoteData.ResultCnt;
				campaignModel.ResultCD    = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("캠페인삭제 End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":SetCampaignDelete():" + fe.ErrCode + ":" + fe.ResultMsg);
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

		public void SetCampaignDetailCreate(CampaignModel campaignModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("캠페인디테일추가 Start");
				_log.Debug("-----------------------------------------");
                        
				
				// 웹서비스 인스턴스 생성
				CampaignServicePloxy.CampaignService svc = new CampaignServicePloxy.CampaignService();
				svc.Url = _WebServiceUrl;			
				// 리모트 모델 생성
				CampaignServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.CampaignServicePloxy.HeaderModel();
				CampaignServicePloxy.CampaignModel remoteData   = new AdManagerClient.CampaignServicePloxy.CampaignModel();
                        
				// 헤더정보 셋트
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;
                        
				// 호출정보셋트				
				remoteData.CampaignCode         = campaignModel.CampaignCode;        
				remoteData.ItemNo	            = campaignModel.ItemNo;     
								                            
				// 웹서비스 호출 타임아웃설정
				svc.Timeout = FrameSystem.m_SystemTimeout;
				// 웹서비스 메소드 호출
				remoteData = svc.SetCampaignDetailCreate(remoteHeader, remoteData);
                        
				// 결과코드검사
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}
                        
				// 결과 셋트
				campaignModel.ResultCnt   = remoteData.ResultCnt;
				_log.Debug("campaignModel.ResultCnt = "+campaignModel.ResultCnt);
                        			
				campaignModel.ResultCD    = remoteData.ResultCD;
                        
				_log.Debug("-----------------------------------------");
				_log.Debug("캠페인디테일추가 End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				campaignModel.ResultCD    = "3101";
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":SetCampaignDetailCreate():" + fe.ErrCode + ":" + fe.ResultMsg);
				_log.Warning("-----------------------------------------");
				throw fe;
			}
			catch(Exception e)
			{
				campaignModel.ResultCD    = "3101";
				_log.Error("-----------------------------------------");
				_log.Exception(e);
				_log.Error("-----------------------------------------");
				throw new FrameException(e.Message);
			}
		}

		/// <summary>
		/// 캠페인디테일 삭제
		/// </summary>
		/// <param name="baseModel"></param>
		public void SetCampaignDetailDelete(CampaignModel campaignModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("캠페인디테일삭제 Start");
				_log.Debug("-----------------------------------------");

				// 웹서비스 인스턴스 생성
				CampaignServicePloxy.CampaignService svc = new CampaignServicePloxy.CampaignService();
              
				// URL의 동적셋트
				svc.Url = _WebServiceUrl;
				
				// 리모트 모델 생성
				CampaignServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.CampaignServicePloxy.HeaderModel();
				CampaignServicePloxy.CampaignModel remoteData   = new AdManagerClient.CampaignServicePloxy.CampaignModel();
              
				// 헤더정보 셋트
				remoteHeader.ClientKey  = Header.ClientKey;
				remoteHeader.UserID     = Header.UserID;

				// 호출정보셋트				
				remoteData.CampaignCode      = campaignModel.CampaignCode;
				remoteData.ItemNo	         = campaignModel.ItemNo;
					
				// 웹서비스 호출 타임아웃설정
				svc.Timeout = FrameSystem.m_SystemTimeout;
				// 웹서비스 메소드 호출
				remoteData = svc.SetCampaignDetailDelete(remoteHeader, remoteData);

				// 결과코드검사
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// 결과 셋트
				campaignModel.ResultCnt   = remoteData.ResultCnt;
				campaignModel.ResultCD    = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("캠페인디테일삭제 End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":SetCampaignDetailDelete():" + fe.ErrCode + ":" + fe.ResultMsg);
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
		/// 캠페인정보조회
		/// </summary>
		/// <param name="campaignModel"></param>
		public void GetCampaignList(CampaignModel campaignModel)
		{

			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("캠페인목록조회 Start");
				_log.Debug("-----------------------------------------");
				
				// 웹서비스 인스턴스 생성
				CampaignServicePloxy.CampaignService svc = new CampaignServicePloxy.CampaignService();

				svc.Url = _WebServiceUrl;

				// 리모트 모델 생성
				CampaignServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.CampaignServicePloxy.HeaderModel();
				CampaignServicePloxy.CampaignModel remoteData   = new AdManagerClient.CampaignServicePloxy.CampaignModel();

				// 헤더정보 셋트
				remoteHeader.ClientKey  = Header.ClientKey;
				remoteHeader.UserID     = Header.UserID;

				// 호출정보 셋트				
				remoteData.MediaCode    = campaignModel.MediaCode;        
				remoteData.SearchUse    = campaignModel.SearchUse;   
				remoteData.ContractSeq	= campaignModel.ContractSeq; 
        
				
				// 웹서비스 호출 타임아웃설정
				svc.Timeout = FrameSystem.m_SystemTimeout;

				// 웹서비스 메소드 호출
				remoteData = svc.GetCampaignList(remoteHeader, remoteData);

				// 결과코드검사
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// 결과 셋트
				campaignModel.CampaignDataSet = remoteData.CampaignDataSet.Copy();
				campaignModel.ResultCnt   = remoteData.ResultCnt;
				campaignModel.ResultCD    = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("캠페인목록조회 End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":GetCampaignList():" + fe.ErrCode + ":" + fe.ResultMsg);
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
        /// 캠페인내역-팝업 추가
        /// </summary>
        /// <param name="campaignModel"></param>
        public void SetCampaignPnsCreate(CampaignModel campaignModel)
        {
            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug("SetCampaignPnsCreate Start");
                _log.Debug("-----------------------------------------");

                CampaignServicePloxy.CampaignService svc = new CampaignServicePloxy.CampaignService();
                
                // 리모트 모델 생성
                CampaignServicePloxy.HeaderModel remoteHeader = new AdManagerClient.CampaignServicePloxy.HeaderModel();
                CampaignServicePloxy.CampaignModel remoteData = new AdManagerClient.CampaignServicePloxy.CampaignModel();

                // 헤더정보 셋트
                remoteHeader.ClientKey = Header.ClientKey;
                remoteHeader.UserID = Header.UserID;

                // 호출정보셋트
                // 항목들은 전용한 것임. 이름은 별 상관없다(주의할것)
                remoteData.CampaignCode = campaignModel.CampaignCode;
                remoteData.ItemNo = campaignModel.ItemNo;
                remoteData.CampaignName = campaignModel.CampaignName;
                remoteData.AgencyCode = campaignModel.AgencyCode;
                remoteData.AdvertiserCode = campaignModel.AdvertiserCode;

                svc.Url = _WebServiceUrl;
                svc.Timeout = FrameSystem.m_SystemTimeout;
                remoteData = svc.svcSetCampaignPnsCreate(remoteHeader, remoteData);

                // 결과코드검사
                if (!remoteData.ResultCD.Equals("0000"))
                {
                    throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
                }

                // 결과 셋트
                campaignModel.ResultCnt = remoteData.ResultCnt;
                _log.Debug("campaignModel.ResultCnt = " + campaignModel.ResultCnt);

                campaignModel.ResultCD = remoteData.ResultCD;

                _log.Debug("-----------------------------------------");
                _log.Debug("SetCampaignPnsCreate End");
                _log.Debug("-----------------------------------------");
            }
            catch (FrameException fe)
            {
                campaignModel.ResultCD = "3101";
                _log.Warning("-----------------------------------------");
                _log.Warning(this.ToString() + ":SetCampaignPnsCreate():" + fe.ErrCode + ":" + fe.ResultMsg);
                _log.Warning("-----------------------------------------");
                throw fe;
            }
            catch (Exception e)
            {
                campaignModel.ResultCD = "3101";
                _log.Error("-----------------------------------------");
                _log.Exception(e);
                _log.Error("-----------------------------------------");
                throw new FrameException(e.Message);
            }
        }

        /// <summary>
        /// 캠페인내역-팝업 삭제
        /// </summary>
        /// <param name="campaignModel"></param>
        public void SetCampaignPnsDelete(CampaignModel campaignModel)
        {
            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug("SetCampaignPnsDelete Start");
                _log.Debug("-----------------------------------------");

                CampaignServicePloxy.CampaignService svc = new CampaignServicePloxy.CampaignService();

                // 리모트 모델 생성
                CampaignServicePloxy.HeaderModel remoteHeader = new AdManagerClient.CampaignServicePloxy.HeaderModel();
                CampaignServicePloxy.CampaignModel remoteData = new AdManagerClient.CampaignServicePloxy.CampaignModel();

                // 헤더정보 셋트
                remoteHeader.ClientKey = Header.ClientKey;
                remoteHeader.UserID = Header.UserID;

                // 호출정보셋트				
                remoteData.CampaignCode = campaignModel.CampaignCode;
                remoteData.ItemNo = campaignModel.ItemNo;

                // 웹서비스 호출 타임아웃설정
                svc.Url = _WebServiceUrl;
                svc.Timeout = FrameSystem.m_SystemTimeout;
                // 웹서비스 메소드 호출
                remoteData = svc.svcSetCampaignPnsDelete(remoteHeader, remoteData);

                // 결과코드검사
                if (!remoteData.ResultCD.Equals("0000"))
                {
                    throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
                }

                // 결과 셋트
                campaignModel.ResultCnt = remoteData.ResultCnt;
                campaignModel.ResultCD = remoteData.ResultCD;

                _log.Debug("-----------------------------------------");
                _log.Debug("SetCampaignPnsDelete End");
                _log.Debug("-----------------------------------------");
            }
            catch (FrameException fe)
            {
                _log.Warning("-----------------------------------------");
                _log.Warning(this.ToString() + ":SetCampaignPnsDelete():" + fe.ErrCode + ":" + fe.ResultMsg);
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