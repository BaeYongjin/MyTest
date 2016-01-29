// ===============================================================================
// ContractPackage Manager  
//
// UserUpdateManager.cs
//
// 광고팩키지  서비스를 호출합니다. 
//
// ===============================================================================
// Release history
// 2007.10.22 RH.Jung
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
    /// 광고팩키지정보 웹서비스를 호출합니다. 
    /// </summary>
    public class ContractPackageManager : BaseManager
    {
        /// <summary>
        /// 생성자
        /// </summary>
        /// <param name="systemModel"></param>
        /// <param name="commonModel"></param>

        public ContractPackageManager(SystemModel systemModel, CommonModel commonModel) : base(systemModel, commonModel)
        {
            _log = FrameSystem.oLog;
            _module = "CONTENT";
            _Host  = FrameSystem.m_WebServer_Host;
            _Port  = FrameSystem.m_WebServer_Port;
            _Path  = FrameSystem.m_WebServer_App + "/Contract/ContractPackageService.asmx";
       
        }

        /// <summary>
        /// 광고팩키지정보조회
        /// </summary>
        /// <param name="contractPackageModel"></param>
        public void GetContractPackageList(ContractPackageModel contractPackageModel)
        {

            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug("광고팩키지목록조회 Start");
                _log.Debug("-----------------------------------------");
				
                // 웹서비스 인스턴스 생성
                ContractPackageServicePloxy.ContractPackageService svc = new ContractPackageServicePloxy.ContractPackageService();

			    svc.Url = _WebServiceUrl;

                // 리모트 모델 생성
                ContractPackageServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.ContractPackageServicePloxy.HeaderModel();
                ContractPackageServicePloxy.ContractPackageModel remoteData   = new AdManagerClient.ContractPackageServicePloxy.ContractPackageModel();

                // 헤더정보 셋트
                remoteHeader.ClientKey     = Header.ClientKey;
                remoteHeader.UserID        = Header.UserID;

                // 호출정보 셋트
                remoteData.SearchKey       = contractPackageModel.SearchKey;
				remoteData.SearchRap       = contractPackageModel.SearchRap;        
				remoteData.SearchUse       = contractPackageModel.SearchUse;        
        
				
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
                contractPackageModel.PackageDataSet = remoteData.PackageDataSet.Copy();
                contractPackageModel.ResultCnt   = remoteData.ResultCnt;
                contractPackageModel.ResultCD    = remoteData.ResultCD;

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
 
        public void SetContractPackageAdd(ContractPackageModel contractPackageModel)
        {
            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug("광고팩키지추가 Start");
                _log.Debug("-----------------------------------------");
            
                // 입력데이터의 Validation 검사
                if(contractPackageModel.PackageName.Trim().Length < 1) 
                {
                    throw new FrameException("광고상품명이 존재하지 않습니다.");
                }
                if(contractPackageModel.PackageName.Trim().Length > 50) 
                {
                    throw new FrameException("광고상품명은 50Byte를 초과할 수 없습니다.");
                }
                if(contractPackageModel.Comment.Trim().Length > 2000) 
                {
                    throw new FrameException("메모는 2000Byte를 초과할 수 없습니다.");
                }            
            
                // 웹서비스 인스턴스 생성
                ContractPackageServicePloxy.ContractPackageService svc = new ContractPackageServicePloxy.ContractPackageService();
                svc.Url = _WebServiceUrl;			
                // 리모트 모델 생성
                ContractPackageServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.ContractPackageServicePloxy.HeaderModel();
                ContractPackageServicePloxy.ContractPackageModel remoteData   = new AdManagerClient.ContractPackageServicePloxy.ContractPackageModel();
            
                // 헤더정보 셋트
                remoteHeader.ClientKey     = Header.ClientKey;
                remoteHeader.UserID        = Header.UserID;
            
                // 호출정보셋트
				remoteData.PackageNo      = contractPackageModel.PackageNo;  
				remoteData.RapCode        = contractPackageModel.RapCode;    
														
				remoteData.PackageName    = contractPackageModel.PackageName;
				remoteData.AdTime         = contractPackageModel.AdTime;     
				remoteData.BonusRate      = contractPackageModel.BonusRate;  
				remoteData.Price          = contractPackageModel.Price;      
				remoteData.ContractAmt    = contractPackageModel.ContractAmt;
				remoteData.Comment        = contractPackageModel.Comment;    
				remoteData.UseYn          = contractPackageModel.UseYn;
                
                // 웹서비스 호출 타임아웃설정
                svc.Timeout = FrameSystem.m_SystemTimeout;
                // 웹서비스 메소드 호출
                remoteData = svc.SetContractPackageCreate(remoteHeader, remoteData);
            
                // 결과코드검사
                if(!remoteData.ResultCD.Equals("0000"))
                {
                    throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
                }
            
                // 결과 셋트
                contractPackageModel.ResultCnt   = remoteData.ResultCnt;
                _log.Debug("contractPackageModel.ResultCnt = "+contractPackageModel.ResultCnt);
                        
                contractPackageModel.ResultCD    = remoteData.ResultCD;
            
                _log.Debug("-----------------------------------------");
                _log.Debug("광고팩키지추가 End");
                _log.Debug("-----------------------------------------");
            }
            catch(FrameException fe)
            {
                contractPackageModel.ResultCD    = "3101";
                _log.Warning("-----------------------------------------");
                _log.Warning( this.ToString() + ":SetContractCreate():" + fe.ErrCode + ":" + fe.ResultMsg);
                _log.Warning("-----------------------------------------");
                throw fe;
            }
            catch(Exception e)
            {
                contractPackageModel.ResultCD    = "3101";
                _log.Error("-----------------------------------------");
                _log.Exception(e);
                _log.Error("-----------------------------------------");
                throw new FrameException(e.Message);
            }
        }

        /// <summary>
        /// 광고팩키지정보 수정
        /// </summary>
        /// <param name="contractPackageModel"></param>
        public void SetContractPackageUpdate(ContractPackageModel contractPackageModel)
        {
            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug("광고팩키지정보수정 Start");
                _log.Debug("-----------------------------------------");
            
            
                // 입력데이터의 Validation 검사
                if(contractPackageModel.PackageName.Trim().Length < 1) 
                {
                    throw new FrameException("광고상품명이 존재하지 않습니다.");
                }
                if(contractPackageModel.PackageName.Trim().Length > 30) 
                {
                    throw new FrameException("광고상품명은 30Byte를 초과할 수 없습니다.");
                }
                if(contractPackageModel.Comment.Trim().Length > 200) 
                {
                    throw new FrameException("메모 200Byte를 초과할 수 없습니다.");
                }     
                
            
                // 웹서비스 인스턴스 생성
                ContractPackageServicePloxy.ContractPackageService svc = new ContractPackageServicePloxy.ContractPackageService();
                svc.Url = _WebServiceUrl;		
                // 리모트 모델 생성
                ContractPackageServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.ContractPackageServicePloxy.HeaderModel();
                ContractPackageServicePloxy.ContractPackageModel remoteData   = new AdManagerClient.ContractPackageServicePloxy.ContractPackageModel();
            
                // 헤더정보 셋트
                remoteHeader.ClientKey     = Header.ClientKey;
                remoteHeader.UserID        = Header.UserID;
            
                // 호출정보셋트
				remoteData.PackageNo      = contractPackageModel.PackageNo;  
				remoteData.RapCode        = contractPackageModel.RapCode;    
														
				remoteData.PackageName    = contractPackageModel.PackageName;
				remoteData.AdTime         = contractPackageModel.AdTime;     
				remoteData.BonusRate      = contractPackageModel.BonusRate;  
				remoteData.Price          = contractPackageModel.Price;      
				remoteData.ContractAmt    = contractPackageModel.ContractAmt;
				remoteData.Comment        = contractPackageModel.Comment;    
				remoteData.UseYn          = contractPackageModel.UseYn;
                        	
                // 웹서비스 호출 타임아웃설정
                svc.Timeout = FrameSystem.m_SystemTimeout;
                // 웹서비스 메소드 호출
                remoteData = svc.SetContractPackageUpdate(remoteHeader, remoteData);
            
                // 결과코드검사
                if(!remoteData.ResultCD.Equals("0000"))
                {
                    throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
                }
            
                // 결과 셋트
                contractPackageModel.ResultCnt   = remoteData.ResultCnt;
                contractPackageModel.ResultCD    = remoteData.ResultCD;
            
                _log.Debug("-----------------------------------------");
                _log.Debug("광고팩키지정보수정 End");
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
        public void SetContractPackageDelete(ContractPackageModel contractPackageModel)
        {
            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug("광고팩키지삭제 Start");
                _log.Debug("-----------------------------------------");

                // 웹서비스 인스턴스 생성
                ContractPackageServicePloxy.ContractPackageService svc = new ContractPackageServicePloxy.ContractPackageService();
              
                // URL의 동적셋트
                svc.Url = _WebServiceUrl;
				
                // 리모트 모델 생성
                ContractPackageServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.ContractPackageServicePloxy.HeaderModel();
                ContractPackageServicePloxy.ContractPackageModel remoteData   = new AdManagerClient.ContractPackageServicePloxy.ContractPackageModel();
              
                // 헤더정보 셋트
                remoteHeader.ClientKey  = Header.ClientKey;
                remoteHeader.UserID     = Header.UserID;

                // 호출정보셋트
                remoteData.PackageNo       = contractPackageModel.PackageNo;
				remoteData.PackageName    = contractPackageModel.PackageName;
				
				// 웹서비스 호출 타임아웃설정
                svc.Timeout = FrameSystem.m_SystemTimeout;
                // 웹서비스 메소드 호출
                remoteData = svc.SetContractPackageDelete(remoteHeader, remoteData);

                // 결과코드검사
                if(!remoteData.ResultCD.Equals("0000"))
                {
                    throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
                }

                // 결과 셋트
                contractPackageModel.ResultCnt   = remoteData.ResultCnt;
                contractPackageModel.ResultCD    = remoteData.ResultCD;

                _log.Debug("-----------------------------------------");
                _log.Debug("광고팩키지삭제 End");
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

    }
}