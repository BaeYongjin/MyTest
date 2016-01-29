// ===============================================================================
// UserUpdate Manager  for Charites Project
//
// UserUpdateManager.cs
//
// ���������� ���� ���񽺸� ȣ���մϴ�. 
//
// ===============================================================================
// Release history
// 2007.06.26 �۸�ȯ v1.0
// 2007.10.03 RH.Jung �����ȸ �޼ҵ� �߰�
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
    /// ���������� �����񽺸� ȣ���մϴ�. 
    /// </summary>
    public class ContractManager : BaseManager
    {
        /// <summary>
        /// ������
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
        /// ������������ȸ
        /// </summary>
        /// <param name="contractModel"></param>
        public void GetContractList(ContractModel contractModel)
        {

            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug("����������ȸ Start");
                _log.Debug("-----------------------------------------");
				
                // ������ �ν��Ͻ� ����
                ContractServicePloxy.ContractService svc = new ContractServicePloxy.ContractService();

			    svc.Url = _WebServiceUrl;

                // ����Ʈ �� ����
                ContractServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.ContractServicePloxy.HeaderModel();
                ContractServicePloxy.ContractModel remoteData   = new AdManagerClient.ContractServicePloxy.ContractModel();

                // ������� ��Ʈ
                remoteHeader.ClientKey     = Header.ClientKey;
                remoteHeader.UserID        = Header.UserID;

                // ȣ������ ��Ʈ
                remoteData.SearchKey       = contractModel.SearchKey;
                remoteData.MediaCode       = contractModel.MediaCode;
                remoteData.RapCode         = contractModel.RapCode;        
                remoteData.AgencyCode      = contractModel.AgencyCode;     
                remoteData.AdvertiserCode  = contractModel.AdvertiserCode; 
                remoteData.State           = contractModel.State;          
				
                // ������ ȣ�� Ÿ�Ӿƿ�����
                svc.Timeout = FrameSystem.m_SystemTimeout;

                // ������ �޼ҵ� ȣ��
                remoteData = svc.GetContractList(remoteHeader, remoteData);

                // ����ڵ�˻�
                if(!remoteData.ResultCD.Equals("0000"))
                {
                    throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
                }

                // ��� ��Ʈ
                contractModel.ContractDataSet = remoteData.ContractDataSet.Copy();
                contractModel.ResultCnt   = remoteData.ResultCnt;
                contractModel.ResultCD    = remoteData.ResultCD;

                _log.Debug("-----------------------------------------");
                _log.Debug("����������ȸ End");
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
   

		// 2007.10.03 RH.Jung �����ȸ �޼ҵ� �߰�
		/// <summary>
		/// ������������ȸ2
		/// </summary>
		/// <param name="contractModel"></param>
		public void GetContractList2(ContractModel contractModel)
		{

			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("����������ȸ2 Start");
				_log.Debug("-----------------------------------------");
				
				// ������ �ν��Ͻ� ����
				ContractServicePloxy.ContractService svc = new ContractServicePloxy.ContractService();

				svc.Url = _WebServiceUrl;

				// ����Ʈ �� ����
				ContractServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.ContractServicePloxy.HeaderModel();
				ContractServicePloxy.ContractModel remoteData   = new AdManagerClient.ContractServicePloxy.ContractModel();

				// ������� ��Ʈ
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;

				// ȣ������ ��Ʈ
				remoteData.SearchKey       = contractModel.SearchKey;
				remoteData.SearchState_10  = contractModel.SearchState_10;          
				remoteData.SearchState_20  = contractModel.SearchState_20;          
				remoteData.MediaCode       = contractModel.MediaCode;
				remoteData.RapCode         = contractModel.RapCode;        
				remoteData.AgencyCode      = contractModel.AgencyCode;     
				
				// ������ ȣ�� Ÿ�Ӿƿ�����
				svc.Timeout = FrameSystem.m_SystemTimeout;

				// ������ �޼ҵ� ȣ��
				remoteData = svc.GetContractList2(remoteHeader, remoteData);

				// ����ڵ�˻�
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// ��� ��Ʈ
				contractModel.ContractDataSet = remoteData.ContractDataSet.Copy();
				contractModel.ResultCnt   = remoteData.ResultCnt;
				contractModel.ResultCD    = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("����������ȸ2 End");
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
		/// ������������ȸ
		/// </summary>
		/// <param name="contractModel"></param>
		public void GetContractItemList(ContractModel contractModel)
		{

			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("�������������ȸ Start");
				_log.Debug("-----------------------------------------");
				
				// ������ �ν��Ͻ� ����
				ContractServicePloxy.ContractService svc = new ContractServicePloxy.ContractService();

				svc.Url = _WebServiceUrl;

				// ����Ʈ �� ����
				ContractServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.ContractServicePloxy.HeaderModel();
				ContractServicePloxy.ContractModel remoteData   = new AdManagerClient.ContractServicePloxy.ContractModel();

				// ������� ��Ʈ
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;

				// ȣ������ ��Ʈ
				remoteData.SearchKey             = contractModel.SearchKey;
				remoteData.MediaCode             = contractModel.MediaCode;
				remoteData.RapCode               = contractModel.RapCode;        
				remoteData.AgencyCode            = contractModel.AgencyCode;     
				remoteData.AdvertiserCode        = contractModel.AdvertiserCode; 
				remoteData.ContractSeq			 = contractModel.ContractSeq; 
								
				// ������ ȣ�� Ÿ�Ӿƿ�����
				svc.Timeout = FrameSystem.m_SystemTimeout;

				// ������ �޼ҵ� ȣ��
				remoteData = svc.GetContractItemList(remoteHeader, remoteData);

				// ����ڵ�˻�
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// ��� ��Ʈ
				contractModel.ContractDataSet = remoteData.ContractDataSet.Copy();
				contractModel.ResultCnt   = remoteData.ResultCnt;
				contractModel.ResultCD    = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("�������������ȸ End");
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
                            _log.Debug("�������߰� Start");
                            _log.Debug("-----------------------------------------");
                        
                            // �Էµ������� Validation �˻�
                            if(contractModel.ContractName.Trim().Length < 1) 
                            {
                                throw new FrameException("���� ������ �������� �ʽ��ϴ�.");
                            }
                            if(contractModel.ContractName.Trim().Length > 50) 
                            {
                                throw new FrameException("���� ������ 50Byte�� �ʰ��� �� �����ϴ�.");
                            }
                            if(contractModel.Comment.Trim().Length > 50) 
                            {
                                throw new FrameException("�޸� 50Byte�� �ʰ��� �� �����ϴ�.");
                            }            
                        
                            // ������ �ν��Ͻ� ����
                            ContractServicePloxy.ContractService svc = new ContractServicePloxy.ContractService();
                        	svc.Url = _WebServiceUrl;			
                            // ����Ʈ �� ����
                            ContractServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.ContractServicePloxy.HeaderModel();
                            ContractServicePloxy.ContractModel remoteData   = new AdManagerClient.ContractServicePloxy.ContractModel();
                        
                            // ������� ��Ʈ
                            remoteHeader.ClientKey     = Header.ClientKey;
                            remoteHeader.UserID        = Header.UserID;
                        
                            // ȣ��������Ʈ
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
                            
                            // ������ ȣ�� Ÿ�Ӿƿ�����
                            svc.Timeout = FrameSystem.m_SystemTimeout;
                            // ������ �޼ҵ� ȣ��
                            remoteData = svc.SetContractCreate(remoteHeader, remoteData);
                        
                            // ����ڵ�˻�
                            if(!remoteData.ResultCD.Equals("0000"))
                            {
                                throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
                            }
                        
                            // ��� ��Ʈ
                            contractModel.ResultCnt   = remoteData.ResultCnt;
                            _log.Debug("contractModel.ResultCnt = "+contractModel.ResultCnt);
                        			
                            contractModel.ResultCD    = remoteData.ResultCD;
                        
                            _log.Debug("-----------------------------------------");
                            _log.Debug("�������߰� End");
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
        /// ���������� ����
        /// </summary>
        /// <param name="contractModel"></param>
        public void SetContractUpdate(ContractModel contractModel)
        {
                        try
                        {
                            _log.Debug("-----------------------------------------");
                            _log.Debug("�������������� Start");
                            _log.Debug("-----------------------------------------");
                        
                        
                            // �Էµ������� Validation �˻�
                            if(contractModel.ContractName.Trim().Length < 1) 
                            {
                                throw new FrameException("���� ������ �������� �ʽ��ϴ�.");
                            }
                            if(contractModel.ContractName.Trim().Length > 50) 
                            {
                                throw new FrameException("���� ������ 50Byte�� �ʰ��� �� �����ϴ�.");
                            }
                            if(contractModel.Comment.Trim().Length > 50) 
                            {
                                throw new FrameException("�޸� 50Byte�� �ʰ��� �� �����ϴ�.");
                            }     
                        	
                        
                            // ������ �ν��Ͻ� ����
                            ContractServicePloxy.ContractService svc = new ContractServicePloxy.ContractService();
                        	svc.Url = _WebServiceUrl;		
                            // ����Ʈ �� ����
                            ContractServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.ContractServicePloxy.HeaderModel();
                            ContractServicePloxy.ContractModel remoteData   = new AdManagerClient.ContractServicePloxy.ContractModel();
                        
                            // ������� ��Ʈ
                            remoteHeader.ClientKey     = Header.ClientKey;
                            remoteHeader.UserID        = Header.UserID;
                        
                            // ȣ��������Ʈ
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
                        				
                            // ������ ȣ�� Ÿ�Ӿƿ�����
                            svc.Timeout = FrameSystem.m_SystemTimeout;
                            // ������ �޼ҵ� ȣ��
                            remoteData = svc.SetContractUpdate(remoteHeader, remoteData);
                        
                            // ����ڵ�˻�
                            if(!remoteData.ResultCD.Equals("0000"))
                            {
                                throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
                            }
                        
                            // ��� ��Ʈ
                            contractModel.ResultCnt   = remoteData.ResultCnt;
                            contractModel.ResultCD    = remoteData.ResultCD;
                        
                            _log.Debug("-----------------------------------------");
                            _log.Debug("�������������� End");
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
        public void SetContractDelete(ContractModel contractModel)
        {
            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug("��������� Start");
                _log.Debug("-----------------------------------------");

                // ������ �ν��Ͻ� ����
                ContractServicePloxy.ContractService svc = new ContractServicePloxy.ContractService();
              
                // URL�� ������Ʈ
                svc.Url = _WebServiceUrl;
				
                // ����Ʈ �� ����
                ContractServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.ContractServicePloxy.HeaderModel();
                ContractServicePloxy.ContractModel remoteData   = new AdManagerClient.ContractServicePloxy.ContractModel();
              
                // ������� ��Ʈ
                remoteHeader.ClientKey  = Header.ClientKey;
                remoteHeader.UserID     = Header.UserID;

                // ȣ��������Ʈ
                remoteData.ContractSeq       = contractModel.ContractSeq;
					
                // ������ ȣ�� Ÿ�Ӿƿ�����
                svc.Timeout = FrameSystem.m_SystemTimeout;
                // ������ �޼ҵ� ȣ��
                remoteData = svc.SetContractDelete(remoteHeader, remoteData);

                // ����ڵ�˻�
                if(!remoteData.ResultCD.Equals("0000"))
                {
                    throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
                }

                // ��� ��Ʈ
                contractModel.ResultCnt   = remoteData.ResultCnt;
                contractModel.ResultCD    = remoteData.ResultCD;

                _log.Debug("-----------------------------------------");
                _log.Debug("��������� End");
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
		/// ������Ű��������ȸ
		/// </summary>
		/// <param name="contractModel"></param>
		public void GetContractPackageList(ContractModel contractModel)
		{

			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("������Ű�������ȸ Start");
				_log.Debug("-----------------------------------------");
				
				// ������ �ν��Ͻ� ����
				ContractServicePloxy.ContractService svc = new ContractServicePloxy.ContractService();

				svc.Url = _WebServiceUrl;

				// ����Ʈ �� ����
				ContractServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.ContractServicePloxy.HeaderModel();
				ContractServicePloxy.ContractModel remoteData   = new AdManagerClient.ContractServicePloxy.ContractModel();

				// ������� ��Ʈ
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;

				// ȣ������ ��Ʈ
				remoteData.SearchKey       = contractModel.SearchKey;
				remoteData.SearchRap       = contractModel.SearchRap;        
				remoteData.SearchUse       = contractModel.SearchUse;        
        
				
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
				contractModel.ContractDataSet = remoteData.ContractDataSet.Copy();
				contractModel.ResultCnt   = remoteData.ResultCnt;
				contractModel.ResultCD    = remoteData.ResultCD;

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

		/// <summary>
		/// ������ȸ
		/// </summary>
		/// <param name="contractModel"></param>
		public void GetLevel1List(ContractModel contractModel)
		{

			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("���������ȸ Start");
				_log.Debug("-----------------------------------------");
				
				// ������ �ν��Ͻ� ����
				ContractServicePloxy.ContractService svc = new ContractServicePloxy.ContractService();

				svc.Url = _WebServiceUrl;

				// ����Ʈ �� ����
				ContractServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.ContractServicePloxy.HeaderModel();
				ContractServicePloxy.ContractModel remoteData   = new AdManagerClient.ContractServicePloxy.ContractModel();

				// ������� ��Ʈ
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;
			      
				
				// ������ ȣ�� Ÿ�Ӿƿ�����
				svc.Timeout = FrameSystem.m_SystemTimeout;

				// ������ �޼ҵ� ȣ��
				remoteData = svc.GetLevel1List(remoteHeader, remoteData);

				// ����ڵ�˻�
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// ��� ��Ʈ
				contractModel.ContractDataSet = remoteData.ContractDataSet.Copy();
				contractModel.ResultCnt   = remoteData.ResultCnt;
				contractModel.ResultCD    = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("������Ű�������ȸ End");
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
		/// ������ȸ
		/// </summary>
		/// <param name="contractModel"></param>
		public void GetJobList(ContractModel contractModel)
		{

			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("���������ȸ Start");
				_log.Debug("-----------------------------------------");
				
				// ������ �ν��Ͻ� ����
				ContractServicePloxy.ContractService svc = new ContractServicePloxy.ContractService();

				svc.Url = _WebServiceUrl;

				// ����Ʈ �� ����
				ContractServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.ContractServicePloxy.HeaderModel();
				ContractServicePloxy.ContractModel remoteData   = new AdManagerClient.ContractServicePloxy.ContractModel();

				// ������� ��Ʈ
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;

				// ȣ������ ��Ʈ
				remoteData.SearchKey       = contractModel.SearchKey;
				remoteData.JobCode       = contractModel.JobCode;        
				      
				
				// ������ ȣ�� Ÿ�Ӿƿ�����
				svc.Timeout = FrameSystem.m_SystemTimeout;

				// ������ �޼ҵ� ȣ��
				remoteData = svc.GetJobList(remoteHeader, remoteData);

				// ����ڵ�˻�
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// ��� ��Ʈ
				contractModel.ContractDataSet = remoteData.ContractDataSet.Copy();
				contractModel.ResultCnt   = remoteData.ResultCnt;
				contractModel.ResultCD    = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("������Ű�������ȸ End");
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
		/// ������ȸ
		/// </summary>
		/// <param name="contractModel"></param>
		public void GetLevel3List(ContractModel contractModel)
		{

			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("���������ȸ Start");
				_log.Debug("-----------------------------------------");
				
				// ������ �ν��Ͻ� ����
				ContractServicePloxy.ContractService svc = new ContractServicePloxy.ContractService();

				svc.Url = _WebServiceUrl;

				// ����Ʈ �� ����
				ContractServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.ContractServicePloxy.HeaderModel();
				ContractServicePloxy.ContractModel remoteData   = new AdManagerClient.ContractServicePloxy.ContractModel();

				// ������� ��Ʈ
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;

				// ȣ������ ��Ʈ
				remoteData.Level2Code       = contractModel.Level2Code;
								
				// ������ ȣ�� Ÿ�Ӿƿ�����
				svc.Timeout = FrameSystem.m_SystemTimeout;

				// ������ �޼ҵ� ȣ��
				remoteData = svc.GetLevel3List(remoteHeader, remoteData);

				// ����ڵ�˻�
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// ��� ��Ʈ
				contractModel.ContractDataSet = remoteData.ContractDataSet.Copy();
				contractModel.ResultCnt   = remoteData.ResultCnt;
				contractModel.ResultCD    = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("������Ű�������ȸ End");
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