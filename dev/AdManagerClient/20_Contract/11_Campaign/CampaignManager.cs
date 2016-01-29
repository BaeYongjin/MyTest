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
/*
 * -------------------------------------------------------
 * [��������]
 * -------------------------------------------------------
 * �����ڵ�  : [A_01]
 * ������    : JH.Kim
 * ������    : 2015.11.
 * ��������  : �������� ��� �÷��� �߰�
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
	/// ���������� �����񽺸� ȣ���մϴ�. 
	/// </summary>
	public class CampaignManager : BaseManager
	{
		/// <summary>
		/// ������
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
		/// ������������ȸ
		/// </summary>
		/// <param name="campaignModel"></param>
		public void GetContractList(CampaignModel campaignModel)
		{

			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("����������ȸ Start");
				_log.Debug("-----------------------------------------");
				
				// ������ �ν��Ͻ� ����
				CampaignServicePloxy.CampaignService svc = new CampaignServicePloxy.CampaignService();

				svc.Url = _WebServiceUrl;

				// ����Ʈ �� ����
				CampaignServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.CampaignServicePloxy.HeaderModel();
				CampaignServicePloxy.CampaignModel remoteData   = new AdManagerClient.CampaignServicePloxy.CampaignModel();

				// ������� ��Ʈ
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;

				// ȣ������ ��Ʈ
				remoteData.SearchKey       = campaignModel.SearchKey;
				remoteData.MediaCode       = campaignModel.MediaCode;
				remoteData.RapCode         = campaignModel.RapCode;        
				remoteData.AgencyCode      = campaignModel.AgencyCode;     
				remoteData.AdvertiserCode  = campaignModel.AdvertiserCode; 
				remoteData.State           = campaignModel.State;          
				
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
				campaignModel.ContractDataSet = remoteData.ContractDataSet.Copy();
				campaignModel.ResultCnt   = remoteData.ResultCnt;
				campaignModel.ResultCD    = remoteData.ResultCD;

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
		/// <param name="campaignModel"></param>
		public void GetContractList2(CampaignModel campaignModel)
		{

			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("����������ȸ2 Start");
				_log.Debug("-----------------------------------------");
				
				// ������ �ν��Ͻ� ����
				CampaignServicePloxy.CampaignService svc = new CampaignServicePloxy.CampaignService();

				svc.Url = _WebServiceUrl;

				// ����Ʈ �� ����
				CampaignServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.CampaignServicePloxy.HeaderModel();
				CampaignServicePloxy.CampaignModel remoteData   = new AdManagerClient.CampaignServicePloxy.CampaignModel();

				// ������� ��Ʈ
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;

				// ȣ������ ��Ʈ
				remoteData.SearchKey       = campaignModel.SearchKey;
				remoteData.SearchState_10  = campaignModel.SearchState_10;          
				remoteData.SearchState_20  = campaignModel.SearchState_20;          
				remoteData.MediaCode       = campaignModel.MediaCode;
				remoteData.RapCode         = campaignModel.RapCode;        
				remoteData.AgencyCode      = campaignModel.AgencyCode;     
				
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
				campaignModel.ContractDataSet = remoteData.ContractDataSet.Copy();
				campaignModel.ResultCnt   = remoteData.ResultCnt;
				campaignModel.ResultCD    = remoteData.ResultCD;

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
		/// <param name="campaignModel"></param>
		public void GetContractItemList(CampaignModel campaignModel)
		{

			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("�������������ȸ Start");
				_log.Debug("-----------------------------------------");
				
				// ������ �ν��Ͻ� ����
				CampaignServicePloxy.CampaignService svc = new CampaignServicePloxy.CampaignService();

				svc.Url = _WebServiceUrl;

				// ����Ʈ �� ����
				CampaignServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.CampaignServicePloxy.HeaderModel();
				CampaignServicePloxy.CampaignModel remoteData   = new AdManagerClient.CampaignServicePloxy.CampaignModel();

				// ������� ��Ʈ
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;

				// ȣ������ ��Ʈ
				remoteData.SearchKey             = campaignModel.SearchKey;
				remoteData.MediaCode             = campaignModel.MediaCode;
				remoteData.RapCode               = campaignModel.RapCode;        
				remoteData.AgencyCode            = campaignModel.AgencyCode;     
				remoteData.AdvertiserCode        = campaignModel.AdvertiserCode; 
				remoteData.CampaignCode			 = campaignModel.CampaignCode; 
								
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
				campaignModel.ContractDataSet = remoteData.ContractDataSet.Copy();  // ķ���� ���� ����
                campaignModel.CampaignDataSet = remoteData.CampaignDataSet.Copy();  // ķ���� ���� �˾�
				campaignModel.ResultCnt   = remoteData.ResultCnt;
				campaignModel.ResultCD    = remoteData.ResultCD;

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
        
        /// <summary>
        /// ķ���� ��� ������ ��� �б�
        /// </summary>
        /// <param name="campaignModel"></param>
		public void GetContractItemPopList(CampaignModel campaignModel)
		{

			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("�������������ȸ Start");
				_log.Debug("-----------------------------------------");
				
				// ������ �ν��Ͻ� ����
				CampaignServicePloxy.CampaignService svc = new CampaignServicePloxy.CampaignService();

				svc.Url = _WebServiceUrl;

				// ����Ʈ �� ����
				CampaignServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.CampaignServicePloxy.HeaderModel();
				CampaignServicePloxy.CampaignModel remoteData   = new AdManagerClient.CampaignServicePloxy.CampaignModel();

				// ������� ��Ʈ
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;
				remoteHeader.UserLevel     = Header.UserLevel;
				remoteHeader.UserClass     = Header.UserClass;

				// ȣ������ ��Ʈ
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
								
				// ������ ȣ�� Ÿ�Ӿƿ�����
				svc.Timeout = FrameSystem.m_SystemTimeout;

				// ������ �޼ҵ� ȣ��
				remoteData = svc.GetContractItemPopList(remoteHeader, remoteData);

				// ����ڵ�˻�
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// ��� ��Ʈ
				campaignModel.ContractDataSet = remoteData.ContractDataSet.Copy();
				campaignModel.ResultCnt   = remoteData.ResultCnt;
				campaignModel.ResultCD    = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("�������������ȸ End");
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
        /// ķ���� ��� �˾����� ��� �б�
        /// </summary>
        /// <param name="campaignModel"></param>
        public void GetPnsPopList(CampaignModel campaignModel)
        {
            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug("�˾����������ȸ Start");
                _log.Debug("-----------------------------------------");

                // ������ �ν��Ͻ� ����
                CampaignServicePloxy.CampaignService svc = new CampaignServicePloxy.CampaignService();

                // ����Ʈ �� ����
                CampaignServicePloxy.HeaderModel remoteHeader = new AdManagerClient.CampaignServicePloxy.HeaderModel();
                CampaignServicePloxy.CampaignModel remoteData = new AdManagerClient.CampaignServicePloxy.CampaignModel();

                // ������� ��Ʈ
                remoteHeader.ClientKey = Header.ClientKey;
                remoteHeader.UserID = Header.UserID;
                remoteHeader.UserLevel = Header.UserLevel;
                remoteHeader.UserClass = Header.UserClass;

                // ȣ������ ��Ʈ
                remoteData.SearchKey = campaignModel.SearchKey;
                remoteData.SearchchkAdState_10 = campaignModel.SearchchkAdState_10;
                remoteData.SearchchkAdState_20 = campaignModel.SearchchkAdState_20;

                // ������ ȣ�� ����
                svc.Url = _WebServiceUrl;
                svc.Timeout = FrameSystem.m_SystemTimeout;

                // ������ �޼ҵ� ȣ��
                remoteData = svc.svcGetPnsList(remoteHeader, remoteData);

                // ����ڵ�˻�
                if (!remoteData.ResultCD.Equals("0000"))
                {
                    throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
                }

                // ��� ��Ʈ
                campaignModel.ContractDataSet = remoteData.ContractDataSet.Copy();
                campaignModel.ResultCnt = remoteData.ResultCnt;
                campaignModel.ResultCD = remoteData.ResultCD;

                _log.Debug("-----------------------------------------");
                _log.Debug("�˾����������ȸ End");
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
				_log.Debug("ķ�����߰� Start");
				_log.Debug("-----------------------------------------");
                        
				// �Էµ������� Validation �˻�
				if(campaignModel.CampaignName.Trim().Length < 1) 
				{
					throw new FrameException("ķ���θ��� �������� �ʽ��ϴ�.");
				}
				if(campaignModel.CampaignName.Trim().Length > 1000) 
				{
					throw new FrameException("ķ���θ��� 1000Byte�� �ʰ��� �� �����ϴ�.");
				}
				
				// ������ �ν��Ͻ� ����
				CampaignServicePloxy.CampaignService svc = new CampaignServicePloxy.CampaignService();
				svc.Url = _WebServiceUrl;			
				// ����Ʈ �� ����
				CampaignServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.CampaignServicePloxy.HeaderModel();
				CampaignServicePloxy.CampaignModel remoteData   = new AdManagerClient.CampaignServicePloxy.CampaignModel();
                        
				// ������� ��Ʈ
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;
                        
				// ȣ��������Ʈ
				remoteData.MediaCode       = campaignModel.MediaCode;				     
				remoteData.ContractSeq     = campaignModel.ContractSeq;     
				remoteData.CampaignName    = campaignModel.CampaignName; 
				remoteData.Price		   = campaignModel.Price;
                remoteData.BizManageTarget = campaignModel.BizManageTarget; // [A_01]
				                            
				// ������ ȣ�� Ÿ�Ӿƿ�����
				svc.Timeout = FrameSystem.m_SystemTimeout;
				// ������ �޼ҵ� ȣ��
				remoteData = svc.SetCampaignCreate(remoteHeader, remoteData);
                        
				// ����ڵ�˻�
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}
                        
				// ��� ��Ʈ
				campaignModel.ResultCnt   = remoteData.ResultCnt;
				_log.Debug("campaignModel.ResultCnt = "+campaignModel.ResultCnt);
                        			
				campaignModel.ResultCD    = remoteData.ResultCD;
                        
				_log.Debug("-----------------------------------------");
				_log.Debug("ķ�����߰� End");
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
		/// ķ������������
		/// </summary>
		/// <param name="campaignModel"></param>
		public void SetCampaignUpdate(CampaignModel campaignModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("�������������� Start");
				_log.Debug("-----------------------------------------");
                        
                        
				// �Էµ������� Validation �˻�
				if(campaignModel.CampaignName.Trim().Length < 1) 
				{
					throw new FrameException("ķ���θ��� �������� �ʽ��ϴ�.");
				}
				if(campaignModel.CampaignName.Trim().Length > 1000) 
				{
					throw new FrameException("ķ���θ��� 1000Byte�� �ʰ��� �� �����ϴ�.");
				}                        	
                        
				// ������ �ν��Ͻ� ����
				CampaignServicePloxy.CampaignService svc = new CampaignServicePloxy.CampaignService();
				svc.Url = _WebServiceUrl;		
				// ����Ʈ �� ����
				CampaignServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.CampaignServicePloxy.HeaderModel();
				CampaignServicePloxy.CampaignModel remoteData   = new AdManagerClient.CampaignServicePloxy.CampaignModel();
                        
				// ������� ��Ʈ
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;
                        
				// ȣ��������Ʈ
				remoteData.MediaCode	   = campaignModel.MediaCode;
				remoteData.CampaignCode    = campaignModel.CampaignCode;        
				remoteData.ContractSeq     = campaignModel.ContractSeq;     
				remoteData.CampaignName	   = campaignModel.CampaignName; 
				remoteData.Price		   = campaignModel.Price;
				remoteData.UseYn           = campaignModel.UseYn;
                remoteData.BizManageTarget = campaignModel.BizManageTarget; // [A_01]
        				
				// ������ ȣ�� Ÿ�Ӿƿ�����
				svc.Timeout = FrameSystem.m_SystemTimeout;
				// ������ �޼ҵ� ȣ��
				remoteData = svc.SetCampaignUpdate(remoteHeader, remoteData);
                        
				// ����ڵ�˻�
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}
                        
				// ��� ��Ʈ
				campaignModel.ResultCnt   = remoteData.ResultCnt;
				campaignModel.ResultCD    = remoteData.ResultCD;
                        
				_log.Debug("-----------------------------------------");
				_log.Debug("ķ������������ End");
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
		/// ķ���� ����
		/// </summary>
		/// <param name="baseModel"></param>
		public void SetCampaignDelete(CampaignModel campaignModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("ķ���λ��� Start");
				_log.Debug("-----------------------------------------");

				// ������ �ν��Ͻ� ����
				CampaignServicePloxy.CampaignService svc = new CampaignServicePloxy.CampaignService();
              
				// URL�� ������Ʈ
				svc.Url = _WebServiceUrl;
				
				// ����Ʈ �� ����
				CampaignServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.CampaignServicePloxy.HeaderModel();
				CampaignServicePloxy.CampaignModel remoteData   = new AdManagerClient.CampaignServicePloxy.CampaignModel();
              
				// ������� ��Ʈ
				remoteHeader.ClientKey  = Header.ClientKey;
				remoteHeader.UserID     = Header.UserID;

				// ȣ��������Ʈ
				remoteData.MediaCode		 = campaignModel.MediaCode;
				remoteData.CampaignCode      = campaignModel.CampaignCode;
				remoteData.ContractSeq       = campaignModel.ContractSeq;
					
				// ������ ȣ�� Ÿ�Ӿƿ�����
				svc.Timeout = FrameSystem.m_SystemTimeout;
				// ������ �޼ҵ� ȣ��
				remoteData = svc.SetCampaignDelete(remoteHeader, remoteData);

				// ����ڵ�˻�
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// ��� ��Ʈ
				campaignModel.ResultCnt   = remoteData.ResultCnt;
				campaignModel.ResultCD    = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("ķ���λ��� End");
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
				_log.Debug("ķ���ε������߰� Start");
				_log.Debug("-----------------------------------------");
                        
				
				// ������ �ν��Ͻ� ����
				CampaignServicePloxy.CampaignService svc = new CampaignServicePloxy.CampaignService();
				svc.Url = _WebServiceUrl;			
				// ����Ʈ �� ����
				CampaignServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.CampaignServicePloxy.HeaderModel();
				CampaignServicePloxy.CampaignModel remoteData   = new AdManagerClient.CampaignServicePloxy.CampaignModel();
                        
				// ������� ��Ʈ
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;
                        
				// ȣ��������Ʈ				
				remoteData.CampaignCode         = campaignModel.CampaignCode;        
				remoteData.ItemNo	            = campaignModel.ItemNo;     
								                            
				// ������ ȣ�� Ÿ�Ӿƿ�����
				svc.Timeout = FrameSystem.m_SystemTimeout;
				// ������ �޼ҵ� ȣ��
				remoteData = svc.SetCampaignDetailCreate(remoteHeader, remoteData);
                        
				// ����ڵ�˻�
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}
                        
				// ��� ��Ʈ
				campaignModel.ResultCnt   = remoteData.ResultCnt;
				_log.Debug("campaignModel.ResultCnt = "+campaignModel.ResultCnt);
                        			
				campaignModel.ResultCD    = remoteData.ResultCD;
                        
				_log.Debug("-----------------------------------------");
				_log.Debug("ķ���ε������߰� End");
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
		/// ķ���ε����� ����
		/// </summary>
		/// <param name="baseModel"></param>
		public void SetCampaignDetailDelete(CampaignModel campaignModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("ķ���ε����ϻ��� Start");
				_log.Debug("-----------------------------------------");

				// ������ �ν��Ͻ� ����
				CampaignServicePloxy.CampaignService svc = new CampaignServicePloxy.CampaignService();
              
				// URL�� ������Ʈ
				svc.Url = _WebServiceUrl;
				
				// ����Ʈ �� ����
				CampaignServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.CampaignServicePloxy.HeaderModel();
				CampaignServicePloxy.CampaignModel remoteData   = new AdManagerClient.CampaignServicePloxy.CampaignModel();
              
				// ������� ��Ʈ
				remoteHeader.ClientKey  = Header.ClientKey;
				remoteHeader.UserID     = Header.UserID;

				// ȣ��������Ʈ				
				remoteData.CampaignCode      = campaignModel.CampaignCode;
				remoteData.ItemNo	         = campaignModel.ItemNo;
					
				// ������ ȣ�� Ÿ�Ӿƿ�����
				svc.Timeout = FrameSystem.m_SystemTimeout;
				// ������ �޼ҵ� ȣ��
				remoteData = svc.SetCampaignDetailDelete(remoteHeader, remoteData);

				// ����ڵ�˻�
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// ��� ��Ʈ
				campaignModel.ResultCnt   = remoteData.ResultCnt;
				campaignModel.ResultCD    = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("ķ���ε����ϻ��� End");
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
		/// ķ����������ȸ
		/// </summary>
		/// <param name="campaignModel"></param>
		public void GetCampaignList(CampaignModel campaignModel)
		{

			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("ķ���θ����ȸ Start");
				_log.Debug("-----------------------------------------");
				
				// ������ �ν��Ͻ� ����
				CampaignServicePloxy.CampaignService svc = new CampaignServicePloxy.CampaignService();

				svc.Url = _WebServiceUrl;

				// ����Ʈ �� ����
				CampaignServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.CampaignServicePloxy.HeaderModel();
				CampaignServicePloxy.CampaignModel remoteData   = new AdManagerClient.CampaignServicePloxy.CampaignModel();

				// ������� ��Ʈ
				remoteHeader.ClientKey  = Header.ClientKey;
				remoteHeader.UserID     = Header.UserID;

				// ȣ������ ��Ʈ				
				remoteData.MediaCode    = campaignModel.MediaCode;        
				remoteData.SearchUse    = campaignModel.SearchUse;   
				remoteData.ContractSeq	= campaignModel.ContractSeq; 
        
				
				// ������ ȣ�� Ÿ�Ӿƿ�����
				svc.Timeout = FrameSystem.m_SystemTimeout;

				// ������ �޼ҵ� ȣ��
				remoteData = svc.GetCampaignList(remoteHeader, remoteData);

				// ����ڵ�˻�
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// ��� ��Ʈ
				campaignModel.CampaignDataSet = remoteData.CampaignDataSet.Copy();
				campaignModel.ResultCnt   = remoteData.ResultCnt;
				campaignModel.ResultCD    = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("ķ���θ����ȸ End");
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
        /// ķ���γ���-�˾� �߰�
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
                
                // ����Ʈ �� ����
                CampaignServicePloxy.HeaderModel remoteHeader = new AdManagerClient.CampaignServicePloxy.HeaderModel();
                CampaignServicePloxy.CampaignModel remoteData = new AdManagerClient.CampaignServicePloxy.CampaignModel();

                // ������� ��Ʈ
                remoteHeader.ClientKey = Header.ClientKey;
                remoteHeader.UserID = Header.UserID;

                // ȣ��������Ʈ
                // �׸���� ������ ����. �̸��� �� �������(�����Ұ�)
                remoteData.CampaignCode = campaignModel.CampaignCode;
                remoteData.ItemNo = campaignModel.ItemNo;
                remoteData.CampaignName = campaignModel.CampaignName;
                remoteData.AgencyCode = campaignModel.AgencyCode;
                remoteData.AdvertiserCode = campaignModel.AdvertiserCode;

                svc.Url = _WebServiceUrl;
                svc.Timeout = FrameSystem.m_SystemTimeout;
                remoteData = svc.svcSetCampaignPnsCreate(remoteHeader, remoteData);

                // ����ڵ�˻�
                if (!remoteData.ResultCD.Equals("0000"))
                {
                    throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
                }

                // ��� ��Ʈ
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
        /// ķ���γ���-�˾� ����
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

                // ����Ʈ �� ����
                CampaignServicePloxy.HeaderModel remoteHeader = new AdManagerClient.CampaignServicePloxy.HeaderModel();
                CampaignServicePloxy.CampaignModel remoteData = new AdManagerClient.CampaignServicePloxy.CampaignModel();

                // ������� ��Ʈ
                remoteHeader.ClientKey = Header.ClientKey;
                remoteHeader.UserID = Header.UserID;

                // ȣ��������Ʈ				
                remoteData.CampaignCode = campaignModel.CampaignCode;
                remoteData.ItemNo = campaignModel.ItemNo;

                // ������ ȣ�� Ÿ�Ӿƿ�����
                svc.Url = _WebServiceUrl;
                svc.Timeout = FrameSystem.m_SystemTimeout;
                // ������ �޼ҵ� ȣ��
                remoteData = svc.svcSetCampaignPnsDelete(remoteHeader, remoteData);

                // ����ڵ�˻�
                if (!remoteData.ResultCD.Equals("0000"))
                {
                    throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
                }

                // ��� ��Ʈ
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