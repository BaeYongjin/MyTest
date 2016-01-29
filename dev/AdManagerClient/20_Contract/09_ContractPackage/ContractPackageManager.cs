// ===============================================================================
// ContractPackage Manager  
//
// UserUpdateManager.cs
//
// ������Ű��  ���񽺸� ȣ���մϴ�. 
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
    /// ������Ű������ �����񽺸� ȣ���մϴ�. 
    /// </summary>
    public class ContractPackageManager : BaseManager
    {
        /// <summary>
        /// ������
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
        /// ������Ű��������ȸ
        /// </summary>
        /// <param name="contractPackageModel"></param>
        public void GetContractPackageList(ContractPackageModel contractPackageModel)
        {

            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug("������Ű�������ȸ Start");
                _log.Debug("-----------------------------------------");
				
                // ������ �ν��Ͻ� ����
                ContractPackageServicePloxy.ContractPackageService svc = new ContractPackageServicePloxy.ContractPackageService();

			    svc.Url = _WebServiceUrl;

                // ����Ʈ �� ����
                ContractPackageServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.ContractPackageServicePloxy.HeaderModel();
                ContractPackageServicePloxy.ContractPackageModel remoteData   = new AdManagerClient.ContractPackageServicePloxy.ContractPackageModel();

                // ������� ��Ʈ
                remoteHeader.ClientKey     = Header.ClientKey;
                remoteHeader.UserID        = Header.UserID;

                // ȣ������ ��Ʈ
                remoteData.SearchKey       = contractPackageModel.SearchKey;
				remoteData.SearchRap       = contractPackageModel.SearchRap;        
				remoteData.SearchUse       = contractPackageModel.SearchUse;        
        
				
                // ������ ȣ�� Ÿ�Ӿƿ�����
                svc.Timeout = FrameSystem.m_SystemTimeout;

                // ������ �޼ҵ� ȣ��
                remoteData = svc.GetContractPackageList(remoteHeader, remoteData);

                // ����ڵ�˻�
                if(!remoteData.ResultCD.Equals("0000"))
                {
                    throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
                }

                // ��� ��Ʈ
                contractPackageModel.PackageDataSet = remoteData.PackageDataSet.Copy();
                contractPackageModel.ResultCnt   = remoteData.ResultCnt;
                contractPackageModel.ResultCD    = remoteData.ResultCD;

                _log.Debug("-----------------------------------------");
                _log.Debug("������Ű�������ȸ End");
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
                _log.Debug("������Ű���߰� Start");
                _log.Debug("-----------------------------------------");
            
                // �Էµ������� Validation �˻�
                if(contractPackageModel.PackageName.Trim().Length < 1) 
                {
                    throw new FrameException("�����ǰ���� �������� �ʽ��ϴ�.");
                }
                if(contractPackageModel.PackageName.Trim().Length > 50) 
                {
                    throw new FrameException("�����ǰ���� 50Byte�� �ʰ��� �� �����ϴ�.");
                }
                if(contractPackageModel.Comment.Trim().Length > 2000) 
                {
                    throw new FrameException("�޸�� 2000Byte�� �ʰ��� �� �����ϴ�.");
                }            
            
                // ������ �ν��Ͻ� ����
                ContractPackageServicePloxy.ContractPackageService svc = new ContractPackageServicePloxy.ContractPackageService();
                svc.Url = _WebServiceUrl;			
                // ����Ʈ �� ����
                ContractPackageServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.ContractPackageServicePloxy.HeaderModel();
                ContractPackageServicePloxy.ContractPackageModel remoteData   = new AdManagerClient.ContractPackageServicePloxy.ContractPackageModel();
            
                // ������� ��Ʈ
                remoteHeader.ClientKey     = Header.ClientKey;
                remoteHeader.UserID        = Header.UserID;
            
                // ȣ��������Ʈ
				remoteData.PackageNo      = contractPackageModel.PackageNo;  
				remoteData.RapCode        = contractPackageModel.RapCode;    
														
				remoteData.PackageName    = contractPackageModel.PackageName;
				remoteData.AdTime         = contractPackageModel.AdTime;     
				remoteData.BonusRate      = contractPackageModel.BonusRate;  
				remoteData.Price          = contractPackageModel.Price;      
				remoteData.ContractAmt    = contractPackageModel.ContractAmt;
				remoteData.Comment        = contractPackageModel.Comment;    
				remoteData.UseYn          = contractPackageModel.UseYn;
                
                // ������ ȣ�� Ÿ�Ӿƿ�����
                svc.Timeout = FrameSystem.m_SystemTimeout;
                // ������ �޼ҵ� ȣ��
                remoteData = svc.SetContractPackageCreate(remoteHeader, remoteData);
            
                // ����ڵ�˻�
                if(!remoteData.ResultCD.Equals("0000"))
                {
                    throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
                }
            
                // ��� ��Ʈ
                contractPackageModel.ResultCnt   = remoteData.ResultCnt;
                _log.Debug("contractPackageModel.ResultCnt = "+contractPackageModel.ResultCnt);
                        
                contractPackageModel.ResultCD    = remoteData.ResultCD;
            
                _log.Debug("-----------------------------------------");
                _log.Debug("������Ű���߰� End");
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
        /// ������Ű������ ����
        /// </summary>
        /// <param name="contractPackageModel"></param>
        public void SetContractPackageUpdate(ContractPackageModel contractPackageModel)
        {
            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug("������Ű���������� Start");
                _log.Debug("-----------------------------------------");
            
            
                // �Էµ������� Validation �˻�
                if(contractPackageModel.PackageName.Trim().Length < 1) 
                {
                    throw new FrameException("�����ǰ���� �������� �ʽ��ϴ�.");
                }
                if(contractPackageModel.PackageName.Trim().Length > 30) 
                {
                    throw new FrameException("�����ǰ���� 30Byte�� �ʰ��� �� �����ϴ�.");
                }
                if(contractPackageModel.Comment.Trim().Length > 200) 
                {
                    throw new FrameException("�޸� 200Byte�� �ʰ��� �� �����ϴ�.");
                }     
                
            
                // ������ �ν��Ͻ� ����
                ContractPackageServicePloxy.ContractPackageService svc = new ContractPackageServicePloxy.ContractPackageService();
                svc.Url = _WebServiceUrl;		
                // ����Ʈ �� ����
                ContractPackageServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.ContractPackageServicePloxy.HeaderModel();
                ContractPackageServicePloxy.ContractPackageModel remoteData   = new AdManagerClient.ContractPackageServicePloxy.ContractPackageModel();
            
                // ������� ��Ʈ
                remoteHeader.ClientKey     = Header.ClientKey;
                remoteHeader.UserID        = Header.UserID;
            
                // ȣ��������Ʈ
				remoteData.PackageNo      = contractPackageModel.PackageNo;  
				remoteData.RapCode        = contractPackageModel.RapCode;    
														
				remoteData.PackageName    = contractPackageModel.PackageName;
				remoteData.AdTime         = contractPackageModel.AdTime;     
				remoteData.BonusRate      = contractPackageModel.BonusRate;  
				remoteData.Price          = contractPackageModel.Price;      
				remoteData.ContractAmt    = contractPackageModel.ContractAmt;
				remoteData.Comment        = contractPackageModel.Comment;    
				remoteData.UseYn          = contractPackageModel.UseYn;
                        	
                // ������ ȣ�� Ÿ�Ӿƿ�����
                svc.Timeout = FrameSystem.m_SystemTimeout;
                // ������ �޼ҵ� ȣ��
                remoteData = svc.SetContractPackageUpdate(remoteHeader, remoteData);
            
                // ����ڵ�˻�
                if(!remoteData.ResultCD.Equals("0000"))
                {
                    throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
                }
            
                // ��� ��Ʈ
                contractPackageModel.ResultCnt   = remoteData.ResultCnt;
                contractPackageModel.ResultCD    = remoteData.ResultCD;
            
                _log.Debug("-----------------------------------------");
                _log.Debug("������Ű���������� End");
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
        /// �������� ����
        /// </summary>
        /// <param name="baseModel"></param>
        public void SetContractPackageDelete(ContractPackageModel contractPackageModel)
        {
            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug("������Ű������ Start");
                _log.Debug("-----------------------------------------");

                // ������ �ν��Ͻ� ����
                ContractPackageServicePloxy.ContractPackageService svc = new ContractPackageServicePloxy.ContractPackageService();
              
                // URL�� ������Ʈ
                svc.Url = _WebServiceUrl;
				
                // ����Ʈ �� ����
                ContractPackageServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.ContractPackageServicePloxy.HeaderModel();
                ContractPackageServicePloxy.ContractPackageModel remoteData   = new AdManagerClient.ContractPackageServicePloxy.ContractPackageModel();
              
                // ������� ��Ʈ
                remoteHeader.ClientKey  = Header.ClientKey;
                remoteHeader.UserID     = Header.UserID;

                // ȣ��������Ʈ
                remoteData.PackageNo       = contractPackageModel.PackageNo;
				remoteData.PackageName    = contractPackageModel.PackageName;
				
				// ������ ȣ�� Ÿ�Ӿƿ�����
                svc.Timeout = FrameSystem.m_SystemTimeout;
                // ������ �޼ҵ� ȣ��
                remoteData = svc.SetContractPackageDelete(remoteHeader, remoteData);

                // ����ڵ�˻�
                if(!remoteData.ResultCD.Equals("0000"))
                {
                    throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
                }

                // ��� ��Ʈ
                contractPackageModel.ResultCnt   = remoteData.ResultCnt;
                contractPackageModel.ResultCD    = remoteData.ResultCD;

                _log.Debug("-----------------------------------------");
                _log.Debug("������Ű������ End");
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