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
    public class ContractManager : BaseManager
    {
        /// <summary>
        /// 생성자
        /// </summary>
        /// <param name="systemModel"></param>
        /// <param name="commonModel"></param>

        public ContractManager(SystemModel systemModel, CommonModel commonModel) : base(systemModel, commonModel)
        {
            _log = FrameSystem.oLog;
            _module = "CONTENT";
            _Host  = FrameSystem.m_WebServer_Host;
            _Port  = FrameSystem.m_WebServer_Port;
            _Path  = FrameSystem.m_WebServer_App + "/Contract/ContractService.asmx";
       
        }

        /// <summary>
        /// 광고계약정보조회
        /// </summary>
        /// <param name="contractModel"></param>
        public void GetContractList(ContractModel contractModel)
        {

            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug("광고계약목록조회 Start");
                _log.Debug("-----------------------------------------");
				
                // 웹서비스 인스턴스 생성
                ContractServicePloxy.ContractService svc = new ContractServicePloxy.ContractService();

			    svc.Url = _WebServiceUrl;

                // 리모트 모델 생성
                ContractServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.ContractServicePloxy.HeaderModel();
                ContractServicePloxy.ContractModel remoteData   = new AdManagerClient.ContractServicePloxy.ContractModel();

                // 헤더정보 셋트
                remoteHeader.ClientKey     = Header.ClientKey;
                remoteHeader.UserID        = Header.UserID;

                // 호출정보 셋트
                remoteData.SearchKey       = contractModel.SearchKey;
                remoteData.MediaCode       = contractModel.MediaCode;
                remoteData.RapCode         = contractModel.RapCode;        
                remoteData.AgencyCode      = contractModel.AgencyCode;     
                remoteData.AdvertiserCode  = contractModel.AdvertiserCode; 
                remoteData.State           = contractModel.State;          
				
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
                contractModel.ContractDataSet = remoteData.ContractDataSet.Copy();
                contractModel.ResultCnt   = remoteData.ResultCnt;
                contractModel.ResultCD    = remoteData.ResultCD;

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
		/// <param name="contractModel"></param>
		public void GetContractList2(ContractModel contractModel)
		{

			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("광고계약목록조회2 Start");
				_log.Debug("-----------------------------------------");
				
				// 웹서비스 인스턴스 생성
				ContractServicePloxy.ContractService svc = new ContractServicePloxy.ContractService();

				svc.Url = _WebServiceUrl;

				// 리모트 모델 생성
				ContractServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.ContractServicePloxy.HeaderModel();
				ContractServicePloxy.ContractModel remoteData   = new AdManagerClient.ContractServicePloxy.ContractModel();

				// 헤더정보 셋트
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;

				// 호출정보 셋트
				remoteData.SearchKey       = contractModel.SearchKey;
				remoteData.SearchState_10  = contractModel.SearchState_10;          
				remoteData.SearchState_20  = contractModel.SearchState_20;          
				remoteData.MediaCode       = contractModel.MediaCode;
				remoteData.RapCode         = contractModel.RapCode;        
				remoteData.AgencyCode      = contractModel.AgencyCode;     
				
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
				contractModel.ContractDataSet = remoteData.ContractDataSet.Copy();
				contractModel.ResultCnt   = remoteData.ResultCnt;
				contractModel.ResultCD    = remoteData.ResultCD;

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
		/// <param name="contractModel"></param>
		public void GetContractItemList(ContractModel contractModel)
		{

			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("관리내역목록조회 Start");
				_log.Debug("-----------------------------------------");
				
				// 웹서비스 인스턴스 생성
				ContractServicePloxy.ContractService svc = new ContractServicePloxy.ContractService();

				svc.Url = _WebServiceUrl;

				// 리모트 모델 생성
				ContractServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.ContractServicePloxy.HeaderModel();
				ContractServicePloxy.ContractModel remoteData   = new AdManagerClient.ContractServicePloxy.ContractModel();

				// 헤더정보 셋트
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;

				// 호출정보 셋트
				remoteData.SearchKey             = contractModel.SearchKey;
				remoteData.MediaCode             = contractModel.MediaCode;
				remoteData.RapCode               = contractModel.RapCode;        
				remoteData.AgencyCode            = contractModel.AgencyCode;     
				remoteData.AdvertiserCode        = contractModel.AdvertiserCode; 
				remoteData.ContractSeq			 = contractModel.ContractSeq; 
								
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
				contractModel.ContractDataSet = remoteData.ContractDataSet.Copy();
				contractModel.ResultCnt   = remoteData.ResultCnt;
				contractModel.ResultCD    = remoteData.ResultCD;

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

        public void SetContractAdd(ContractModel contractModel)
        {
                        try
                        {
                            _log.Debug("-----------------------------------------");
                            _log.Debug("광고계약추가 Start");
                            _log.Debug("-----------------------------------------");
                        
                            // 입력데이터의 Validation 검사
                            if(contractModel.ContractName.Trim().Length < 1) 
                            {
                                throw new FrameException("광고 계약명이 존재하지 않습니다.");
                            }
                            if(contractModel.ContractName.Trim().Length > 50) 
                            {
                                throw new FrameException("광고 계약명은 50Byte를 초과할 수 없습니다.");
                            }
                            if(contractModel.Comment.Trim().Length > 50) 
                            {
                                throw new FrameException("메모 50Byte를 초과할 수 없습니다.");
                            }            
                        
                            // 웹서비스 인스턴스 생성
                            ContractServicePloxy.ContractService svc = new ContractServicePloxy.ContractService();
                        	svc.Url = _WebServiceUrl;			
                            // 리모트 모델 생성
                            ContractServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.ContractServicePloxy.HeaderModel();
                            ContractServicePloxy.ContractModel remoteData   = new AdManagerClient.ContractServicePloxy.ContractModel();
                        
                            // 헤더정보 셋트
                            remoteHeader.ClientKey     = Header.ClientKey;
                            remoteHeader.UserID        = Header.UserID;
                        
                            // 호출정보셋트
                            remoteData.MediaCode            = contractModel.MediaCode;
                            remoteData.RapCode              = contractModel.RapCode;        
                            remoteData.AgencyCode           = contractModel.AgencyCode;     
                            remoteData.AdvertiserCode       = contractModel.AdvertiserCode; 
                            remoteData.State                = contractModel.State;      
							remoteData.AdTime				= contractModel.AdTime;     
							remoteData.BonusRate			= contractModel.BonusRate;  
							remoteData.LongBonus			= contractModel.LongBonus;  
							remoteData.SpecialBonus			= contractModel.SpecialBonus;  
							remoteData.TotalBonus			= contractModel.TotalBonus;  
							remoteData.SecurityTgt			= contractModel.SecurityTgt;  
							remoteData.PackageName			= contractModel.PackageName;  
							remoteData.JobClass			    = contractModel.JobClass;  
							remoteData.Price				= contractModel.Price; 		
                            remoteData.ContractName         = contractModel.ContractName;   
                            remoteData.ContStartDay         = contractModel.ContStartDay;   
                            remoteData.ContEndDay           = contractModel.ContEndDay;     
							remoteData.ContractAmt           = contractModel.ContractAmt;     
                            remoteData.Comment              = contractModel.Comment;    
                            
                            // 웹서비스 호출 타임아웃설정
                            svc.Timeout = FrameSystem.m_SystemTimeout;
                            // 웹서비스 메소드 호출
                            remoteData = svc.SetContractCreate(remoteHeader, remoteData);
                        
                            // 결과코드검사
                            if(!remoteData.ResultCD.Equals("0000"))
                            {
                                throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
                            }
                        
                            // 결과 셋트
                            contractModel.ResultCnt   = remoteData.ResultCnt;
                            _log.Debug("contractModel.ResultCnt = "+contractModel.ResultCnt);
                        			
                            contractModel.ResultCD    = remoteData.ResultCD;
                        
                            _log.Debug("-----------------------------------------");
                            _log.Debug("광고계약추가 End");
                            _log.Debug("-----------------------------------------");
                        }
                        catch(FrameException fe)
                        {
                            contractModel.ResultCD    = "3101";
                            _log.Warning("-----------------------------------------");
                            _log.Warning( this.ToString() + ":SetContractCreate():" + fe.ErrCode + ":" + fe.ResultMsg);
                            _log.Warning("-----------------------------------------");
                            throw fe;
                        }
                        catch(Exception e)
                        {
                            contractModel.ResultCD    = "3101";
                            _log.Error("-----------------------------------------");
                            _log.Exception(e);
                            _log.Error("-----------------------------------------");
                            throw new FrameException(e.Message);
                        }
        }

        /// <summary>
        /// 광고계약정보 수정
        /// </summary>
        /// <param name="contractModel"></param>
        public void SetContractUpdate(ContractModel contractModel)
        {
                        try
                        {
                            _log.Debug("-----------------------------------------");
                            _log.Debug("광고계약정보수정 Start");
                            _log.Debug("-----------------------------------------");
                        
                        
                            // 입력데이터의 Validation 검사
                            if(contractModel.ContractName.Trim().Length < 1) 
                            {
                                throw new FrameException("광고 계약명이 존재하지 않습니다.");
                            }
                            if(contractModel.ContractName.Trim().Length > 50) 
                            {
                                throw new FrameException("광고 계약명은 50Byte를 초과할 수 없습니다.");
                            }
                            if(contractModel.Comment.Trim().Length > 50) 
                            {
                                throw new FrameException("메모 50Byte를 초과할 수 없습니다.");
                            }     
                        	
                        
                            // 웹서비스 인스턴스 생성
                            ContractServicePloxy.ContractService svc = new ContractServicePloxy.ContractService();
                        	svc.Url = _WebServiceUrl;		
                            // 리모트 모델 생성
                            ContractServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.ContractServicePloxy.HeaderModel();
                            ContractServicePloxy.ContractModel remoteData   = new AdManagerClient.ContractServicePloxy.ContractModel();
                        
                            // 헤더정보 셋트
                            remoteHeader.ClientKey     = Header.ClientKey;
                            remoteHeader.UserID        = Header.UserID;
                        
                            // 호출정보셋트
                            remoteData.MediaCode            = contractModel.MediaCode;
                            remoteData.RapCode              = contractModel.RapCode;        
                            remoteData.AgencyCode           = contractModel.AgencyCode;     
                            remoteData.AdvertiserCode       = contractModel.AdvertiserCode; 
                            remoteData.State                = contractModel.State;          
							remoteData.AdTime				= contractModel.AdTime;     
							remoteData.BonusRate			= contractModel.BonusRate; 
							remoteData.LongBonus			= contractModel.LongBonus;  
							remoteData.SpecialBonus			= contractModel.SpecialBonus;  
							remoteData.TotalBonus			= contractModel.TotalBonus;  
							remoteData.SecurityTgt			= contractModel.SecurityTgt;  
							remoteData.PackageName			= contractModel.PackageName;  
							remoteData.JobClass			= contractModel.JobClass;  
							remoteData.Price				= contractModel.Price; 		
                            remoteData.ContractName         = contractModel.ContractName;   
                            remoteData.ContStartDay         = contractModel.ContStartDay;   
                            remoteData.ContEndDay           = contractModel.ContEndDay; 
							remoteData.ContractAmt           = contractModel.ContractAmt;     
                            remoteData.Comment              = contractModel.Comment;    
                            remoteData.ContractSeq          = contractModel.ContractSeq;        
                        				
                            // 웹서비스 호출 타임아웃설정
                            svc.Timeout = FrameSystem.m_SystemTimeout;
                            // 웹서비스 메소드 호출
                            remoteData = svc.SetContractUpdate(remoteHeader, remoteData);
                        
                            // 결과코드검사
                            if(!remoteData.ResultCD.Equals("0000"))
                            {
                                throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
                            }
                        
                            // 결과 셋트
                            contractModel.ResultCnt   = remoteData.ResultCnt;
                            contractModel.ResultCD    = remoteData.ResultCD;
                        
                            _log.Debug("-----------------------------------------");
                            _log.Debug("광고계약정보수정 End");
                            _log.Debug("-----------------------------------------");
                        }
                        catch(FrameException fe)
                        {
                            _log.Warning("-----------------------------------------");
                            _log.Warning( this.ToString() + ":SetContractUpdate():" + fe.ErrCode + ":" + fe.ResultMsg);
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
        /// 광고정보 삭제
        /// </summary>
        /// <param name="baseModel"></param>
        public void SetContractDelete(ContractModel contractModel)
        {
            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug("광고계약삭제 Start");
                _log.Debug("-----------------------------------------");

                // 웹서비스 인스턴스 생성
                ContractServicePloxy.ContractService svc = new ContractServicePloxy.ContractService();
              
                // URL의 동적셋트
                svc.Url = _WebServiceUrl;
				
                // 리모트 모델 생성
                ContractServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.ContractServicePloxy.HeaderModel();
                ContractServicePloxy.ContractModel remoteData   = new AdManagerClient.ContractServicePloxy.ContractModel();
              
                // 헤더정보 셋트
                remoteHeader.ClientKey  = Header.ClientKey;
                remoteHeader.UserID     = Header.UserID;

                // 호출정보셋트
                remoteData.ContractSeq       = contractModel.ContractSeq;
					
                // 웹서비스 호출 타임아웃설정
                svc.Timeout = FrameSystem.m_SystemTimeout;
                // 웹서비스 메소드 호출
                remoteData = svc.SetContractDelete(remoteHeader, remoteData);

                // 결과코드검사
                if(!remoteData.ResultCD.Equals("0000"))
                {
                    throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
                }

                // 결과 셋트
                contractModel.ResultCnt   = remoteData.ResultCnt;
                contractModel.ResultCD    = remoteData.ResultCD;

                _log.Debug("-----------------------------------------");
                _log.Debug("광고계약삭제 End");
                _log.Debug("-----------------------------------------");
            }
            catch(FrameException fe)
            {
                _log.Warning("-----------------------------------------");
                _log.Warning( this.ToString() + ":SetContractDelete():" + fe.ErrCode + ":" + fe.ResultMsg);
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
		/// 광고팩키지정보조회
		/// </summary>
		/// <param name="contractModel"></param>
		public void GetContractPackageList(ContractModel contractModel)
		{

			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("광고팩키지목록조회 Start");
				_log.Debug("-----------------------------------------");
				
				// 웹서비스 인스턴스 생성
				ContractServicePloxy.ContractService svc = new ContractServicePloxy.ContractService();

				svc.Url = _WebServiceUrl;

				// 리모트 모델 생성
				ContractServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.ContractServicePloxy.HeaderModel();
				ContractServicePloxy.ContractModel remoteData   = new AdManagerClient.ContractServicePloxy.ContractModel();

				// 헤더정보 셋트
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;

				// 호출정보 셋트
				remoteData.SearchKey       = contractModel.SearchKey;
				remoteData.SearchRap       = contractModel.SearchRap;        
				remoteData.SearchUse       = contractModel.SearchUse;        
        
				
				// 웹서비스 호출 타임아웃설정
				svc.Timeout = FrameSystem.m_SystemTimeout;

				// 웹서비스 메소드 호출
				remoteData = svc.GetContractPackageList(remoteHeader, remoteData);

				// 결과코드검사
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// 결과 셋트
				contractModel.ContractDataSet = remoteData.ContractDataSet.Copy();
				contractModel.ResultCnt   = remoteData.ResultCnt;
				contractModel.ResultCD    = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("광고팩키지목록조회 End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":GetContractPackageList():" + fe.ErrCode + ":" + fe.ResultMsg);
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
		/// 업종조회
		/// </summary>
		/// <param name="contractModel"></param>
		public void GetLevel1List(ContractModel contractModel)
		{

			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("업종목록조회 Start");
				_log.Debug("-----------------------------------------");
				
				// 웹서비스 인스턴스 생성
				ContractServicePloxy.ContractService svc = new ContractServicePloxy.ContractService();

				svc.Url = _WebServiceUrl;

				// 리모트 모델 생성
				ContractServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.ContractServicePloxy.HeaderModel();
				ContractServicePloxy.ContractModel remoteData   = new AdManagerClient.ContractServicePloxy.ContractModel();

				// 헤더정보 셋트
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;
			      
				
				// 웹서비스 호출 타임아웃설정
				svc.Timeout = FrameSystem.m_SystemTimeout;

				// 웹서비스 메소드 호출
				remoteData = svc.GetLevel1List(remoteHeader, remoteData);

				// 결과코드검사
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// 결과 셋트
				contractModel.ContractDataSet = remoteData.ContractDataSet.Copy();
				contractModel.ResultCnt   = remoteData.ResultCnt;
				contractModel.ResultCD    = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("광고팩키지목록조회 End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":GetLevel1List():" + fe.ErrCode + ":" + fe.ResultMsg);
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
		/// 업종조회
		/// </summary>
		/// <param name="contractModel"></param>
		public void GetJobList(ContractModel contractModel)
		{

			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("업종목록조회 Start");
				_log.Debug("-----------------------------------------");
				
				// 웹서비스 인스턴스 생성
				ContractServicePloxy.ContractService svc = new ContractServicePloxy.ContractService();

				svc.Url = _WebServiceUrl;

				// 리모트 모델 생성
				ContractServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.ContractServicePloxy.HeaderModel();
				ContractServicePloxy.ContractModel remoteData   = new AdManagerClient.ContractServicePloxy.ContractModel();

				// 헤더정보 셋트
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;

				// 호출정보 셋트
				remoteData.SearchKey       = contractModel.SearchKey;
				remoteData.JobCode       = contractModel.JobCode;        
				      
				
				// 웹서비스 호출 타임아웃설정
				svc.Timeout = FrameSystem.m_SystemTimeout;

				// 웹서비스 메소드 호출
				remoteData = svc.GetJobList(remoteHeader, remoteData);

				// 결과코드검사
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// 결과 셋트
				contractModel.ContractDataSet = remoteData.ContractDataSet.Copy();
				contractModel.ResultCnt   = remoteData.ResultCnt;
				contractModel.ResultCD    = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("광고팩키지목록조회 End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":GetJobList():" + fe.ErrCode + ":" + fe.ResultMsg);
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
		/// 업종조회
		/// </summary>
		/// <param name="contractModel"></param>
		public void GetLevel3List(ContractModel contractModel)
		{

			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("업종목록조회 Start");
				_log.Debug("-----------------------------------------");
				
				// 웹서비스 인스턴스 생성
				ContractServicePloxy.ContractService svc = new ContractServicePloxy.ContractService();

				svc.Url = _WebServiceUrl;

				// 리모트 모델 생성
				ContractServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.ContractServicePloxy.HeaderModel();
				ContractServicePloxy.ContractModel remoteData   = new AdManagerClient.ContractServicePloxy.ContractModel();

				// 헤더정보 셋트
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;

				// 호출정보 셋트
				remoteData.Level2Code       = contractModel.Level2Code;
								
				// 웹서비스 호출 타임아웃설정
				svc.Timeout = FrameSystem.m_SystemTimeout;

				// 웹서비스 메소드 호출
				remoteData = svc.GetLevel3List(remoteHeader, remoteData);

				// 결과코드검사
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// 결과 셋트
				contractModel.ContractDataSet = remoteData.ContractDataSet.Copy();
				contractModel.ResultCnt   = remoteData.ResultCnt;
				contractModel.ResultCD    = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("광고팩키지목록조회 End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":GetJobList():" + fe.ErrCode + ":" + fe.ResultMsg);
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

    }
}