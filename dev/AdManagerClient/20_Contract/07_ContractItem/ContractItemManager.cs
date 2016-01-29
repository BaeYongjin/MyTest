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
// ===============================================================================
// Copyright (C) 2007 G-Inno Systems Inc.
// All rights reserved.
// ==============================================================================
/*
 * -------------------------------------------------------
 * Class Name: ContractItemManager
 * �ֿ���  : ������ ó�� ������ Client Manager
 * �ۼ���    : bae 
 * �ۼ���    : 2010.06.10
 * Ư�̻���  : 
 * -------------------------------------------------------
 * [��������]
 * -------------------------------------------------------
 * �����ڵ�  : [E_01]
 * ������    : bae
 * ������    : 2010.06.10
 * ��������  : 
 *            - ������� �ϰ����� ó�� �߰�
 *            - ����ä�� �ϰ����� ó�� �߰�
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
    /// ���������� �����񽺸� ȣ���մϴ�. 
    /// </summary>
    public class ContractItemManager : BaseManager
    {
        /// <summary>
        /// ������
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
		/// ������������ȸ
		/// </summary>
		/// <param name="contractItemModel"></param>
		public void GetGradeCodeList(ContractItemModel contractItemModel)
		{

			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("��޸����ȸ Start");
				_log.Debug("-----------------------------------------");
				
				// ������ �ν��Ͻ� ����
				ContractItemServicePloxy.ContractItemService svc = new ContractItemServicePloxy.ContractItemService();
				svc.Url = _WebServiceUrl;
				// ����Ʈ �� ����
				ContractItemServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.ContractItemServicePloxy.HeaderModel();
				ContractItemServicePloxy.ContractItemModel remoteData   = new AdManagerClient.ContractItemServicePloxy.ContractItemModel();

				// ������� ��Ʈ
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;

								
				// ������ ȣ�� Ÿ�Ӿƿ�����
				svc.Timeout = FrameSystem.m_SystemTimeout;
				// ������ �޼ҵ� ȣ��
				remoteData = svc.GetGradeCodeList(remoteHeader, remoteData);

				// ����ڵ�˻�
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// ��� ��Ʈ
				contractItemModel.GradeDataSet = remoteData.GradeDataSet.Copy();
				contractItemModel.ResultCnt   = remoteData.ResultCnt;
				contractItemModel.ResultCD    = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("��޸����ȸ End");
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
        /// ������������ȸ
        /// </summary>
        /// <param name="contractItemModel"></param>
        public void GetContractItemList(ContractItemModel contractItemModel)
        {

            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug("�����������ȸ Start");
                _log.Debug("-----------------------------------------");
				
                // ������ �ν��Ͻ� ����
                ContractItemServicePloxy.ContractItemService svc = new ContractItemServicePloxy.ContractItemService();
			    svc.Url = _WebServiceUrl;
                // ����Ʈ �� ����
                ContractItemServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.ContractItemServicePloxy.HeaderModel();
                ContractItemServicePloxy.ContractItemModel remoteData   = new AdManagerClient.ContractItemServicePloxy.ContractItemModel();

                // ������� ��Ʈ
                remoteHeader.ClientKey     = Header.ClientKey;
                remoteHeader.UserID        = Header.UserID;

                // ȣ������ ��Ʈ
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
                contractItemModel.ContractItemDataSet = remoteData.ContractItemDataSet.Copy();
                contractItemModel.ResultCnt   = remoteData.ResultCnt;
                contractItemModel.ResultCD    = remoteData.ResultCD;

                _log.Debug("-----------------------------------------");
                _log.Debug("�����������ȸ End");
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
        /// ������ ������ �����ȸ
        /// </summary>
        /// <param name="contractItemModel"></param>
        public void GetContractItemDetail(ContractItemModel contractItemModel)
        {
            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug("������ ������ ��ȸ Start");
                _log.Debug("-----------------------------------------");
            				
                // ������ �ν��Ͻ� ����
                ContractItemServicePloxy.ContractItemService svc = new ContractItemServicePloxy.ContractItemService();
            	svc.Url = _WebServiceUrl;		
                // ����Ʈ �� ����
                ContractItemServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.ContractItemServicePloxy.HeaderModel();
                ContractItemServicePloxy.ContractItemModel remoteData   = new AdManagerClient.ContractItemServicePloxy.ContractItemModel();
            
                // ������� ��Ʈ
                remoteHeader.ClientKey	= Header.ClientKey;
                remoteHeader.UserID     = Header.UserID;
                remoteData.ItemNo       = contractItemModel.ItemNo;
            		
                // ������ ȣ�� Ÿ�Ӿƿ�����
                svc.Timeout = FrameSystem.m_SystemTimeout;

                // ������ �޼ҵ� ȣ��
                remoteData = svc.GetContractItemDetail(remoteHeader, remoteData);

				if( null != svc)
				{
					svc.Dispose();
					svc = null;
				}
            
                // ����ڵ�˻�
                if(!remoteData.ResultCD.Equals("0000"))
                {
                    throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
                }
            
                // ��� ��Ʈ
                contractItemModel.ContractItemDataSet = remoteData.ContractItemDataSet.Copy();
                contractItemModel.ResultCnt   = remoteData.ResultCnt;
                contractItemModel.ResultCD    = remoteData.ResultCD;
            
                _log.Debug("-----------------------------------------");
                _log.Debug("������ ������ �����ȸ End");
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
		/// ��ũä�� �����ȸ
		/// </summary>
		/// <param name="contractItemModel"></param>
		public void GetLinkChannel(ContractItemModel contractItemModel)
		{
            
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("��ũä�� ��ȸ Start");
				_log.Debug("-----------------------------------------");
            				
				// ������ �ν��Ͻ� ����
				ContractItemServicePloxy.ContractItemService svc = new ContractItemServicePloxy.ContractItemService();
				svc.Url = _WebServiceUrl;		
				// ����Ʈ �� ����
				ContractItemServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.ContractItemServicePloxy.HeaderModel();
				ContractItemServicePloxy.ContractItemModel remoteData   = new AdManagerClient.ContractItemServicePloxy.ContractItemModel();
            
				// ������� ��Ʈ
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;
            
				// ȣ������ ��Ʈ
                remoteData.ItemNo = contractItemModel.ItemNo;
                remoteData.LinkType = contractItemModel.LinkType;
            		
				// ������ ȣ�� Ÿ�Ӿƿ�����
				svc.Timeout = FrameSystem.m_SystemTimeout;

				// ������ �޼ҵ� ȣ��
				remoteData = svc.GetLinkChannel2(remoteHeader, remoteData);

				if( null != svc)
				{
					svc.Dispose();
					svc = null;
				}
            
				// ����ڵ�˻�
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}
            
				// ��� ��Ʈ
				contractItemModel.LinkChannelDataSet = remoteData.LinkChannelDataSet.Copy();
				contractItemModel.ResultCnt   = remoteData.ResultCnt;
				contractItemModel.ResultCD    = remoteData.ResultCD;
            
				_log.Debug("-----------------------------------------");
				_log.Debug("��ũä�� �����ȸ End");
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
        /// �������� �������ȸ
        /// </summary>
        /// <param name="contractItemModel"></param>
        public void GetLinkItem(ContractItemModel contractItemModel)
        {
            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug("����� ��ȸ Start");
                _log.Debug("-----------------------------------------");

                // ������ �ν��Ͻ� ����
                ContractItemServicePloxy.ContractItemService svc = new ContractItemServicePloxy.ContractItemService();
                svc.Url = _WebServiceUrl;
                ContractItemServicePloxy.HeaderModel remoteHeader = new AdManagerClient.ContractItemServicePloxy.HeaderModel();
                ContractItemServicePloxy.ContractItemModel remoteData = new AdManagerClient.ContractItemServicePloxy.ContractItemModel();

                // ������� ��Ʈ
                remoteHeader.ClientKey = Header.ClientKey;
                remoteHeader.UserID = Header.UserID;

                // ȣ������ ��Ʈ
                remoteData.ItemNo = contractItemModel.ItemNo;
                remoteData.ScheduleType = contractItemModel.ScheduleType;

                // ������ ȣ�� Ÿ�Ӿƿ�����
                svc.Timeout = FrameSystem.m_SystemTimeout;

                // ������ �޼ҵ� ȣ��
                remoteData = svc.GetLinkItem(remoteHeader, remoteData);

                if (null != svc)
                {
                    svc.Dispose();
                    svc = null;
                }

                // ����ڵ�˻�
                if (!remoteData.ResultCD.Equals("0000"))
                {
                    throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
                }

                // ��� ��Ʈ
                contractItemModel.ContractItemDataSet = remoteData.ContractItemDataSet.Copy();
                contractItemModel.ResultCnt = remoteData.ResultCnt;
                contractItemModel.ResultCD = remoteData.ResultCD;

                _log.Debug("-----------------------------------------");
                _log.Debug("����� �����ȸ End");
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
        /// ������ ������ �����ȸ
        /// </summary>
        /// <param name="contractItemModel"></param>
        public void GetContractItemHIstoryList(ContractItemModel contractItemModel)
        {
            
            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug("������ ������ ��ȸ Start");
                _log.Debug("-----------------------------------------");
            				
                // ������ �ν��Ͻ� ����
                ContractItemServicePloxy.ContractItemService svc = new ContractItemServicePloxy.ContractItemService();
            	svc.Url = _WebServiceUrl;		
                // ����Ʈ �� ����
                ContractItemServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.ContractItemServicePloxy.HeaderModel();
                ContractItemServicePloxy.ContractItemModel remoteData   = new AdManagerClient.ContractItemServicePloxy.ContractItemModel();
            
                // ������� ��Ʈ
                remoteHeader.ClientKey     = Header.ClientKey;
                remoteHeader.UserID        = Header.UserID;
            
                // ȣ������ ��Ʈ
                remoteData.ItemNo       = contractItemModel.ItemNo;
            				
                // ������ ȣ�� Ÿ�Ӿƿ�����
                svc.Timeout = FrameSystem.m_SystemTimeout;
                // ������ �޼ҵ� ȣ��
                remoteData = svc.GetContractItemHIstoryList(remoteHeader, remoteData);

				if( null != svc)
				{
					svc.Dispose();
					svc = null;
				}
            
                // ����ڵ�˻�
                if(!remoteData.ResultCD.Equals("0000"))
                {
                    throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
                }
            
                // ��� ��Ʈ
                contractItemModel.ContractItemDataSet = remoteData.ContractItemDataSet.Copy();
                contractItemModel.ResultCnt   = remoteData.ResultCnt;
                contractItemModel.ResultCD    = remoteData.ResultCD;
            
                _log.Debug("-----------------------------------------");
                _log.Debug("������ ������ �����ȸ End");
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
		/// �ڷ� �����ȸ
		/// </summary>
		/// <param name="contractItemModel"></param>
		public void GetFileList(ContractItemModel contractItemModel)
		{
            
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("�ڷ� ������ ��ȸ Start");
				_log.Debug("-----------------------------------------");
            				
				// ������ �ν��Ͻ� ����
				ContractItemServicePloxy.ContractItemService svc = new ContractItemServicePloxy.ContractItemService();
				svc.Url = _WebServiceUrl;		
				// ����Ʈ �� ����
				ContractItemServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.ContractItemServicePloxy.HeaderModel();
				ContractItemServicePloxy.ContractItemModel remoteData   = new AdManagerClient.ContractItemServicePloxy.ContractItemModel();
            
				// ������� ��Ʈ
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;
            
				// ȣ������ ��Ʈ
				remoteData.ItemNo       = contractItemModel.ItemNo;
            				
				// ������ ȣ�� Ÿ�Ӿƿ�����
				svc.Timeout = FrameSystem.m_SystemTimeout;
				// ������ �޼ҵ� ȣ��
				remoteData = svc.GetFileList(remoteHeader, remoteData);

				if( null != svc)
				{
					svc.Dispose();
					svc = null;
				}
            
				// ����ڵ�˻�
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}
            
				// ��� ��Ʈ
				contractItemModel.ContractItemDataSet = remoteData.ContractItemDataSet.Copy();
				contractItemModel.ResultCnt   = remoteData.ResultCnt;
				contractItemModel.ResultCD    = remoteData.ResultCD;
            
				_log.Debug("-----------------------------------------");
				_log.Debug("�ڷ� ������ �����ȸ End");
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
		/// ���� ������ ��������
		/// </summary>
		/// <param name="contractItemModel"></param>
		public void GetFileInfo(ContractItemModel contractItemModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("�������� ��ȸ Start");
				_log.Debug("-----------------------------------------");

				// ������ �ν��Ͻ� ����
				ContractItemServicePloxy.ContractItemService svc = new ContractItemServicePloxy.ContractItemService();
				svc.Url = _WebServiceUrl;
				// ����Ʈ �� ����
				ContractItemServicePloxy.HeaderModel remoteHeader = new AdManagerClient.ContractItemServicePloxy.HeaderModel();
				ContractItemServicePloxy.ContractItemModel remoteData = new AdManagerClient.ContractItemServicePloxy.ContractItemModel();

				// ������� ��Ʈ
				remoteHeader.ClientKey = Header.ClientKey;
				remoteHeader.UserID = Header.UserID;

				// ������ ȣ�� Ÿ�Ӿƿ�����
				svc.Timeout = FrameSystem.m_SystemTimeout;
				remoteData = svc.wGetFileInfo(remoteHeader, remoteData);

				if (null != svc)
				{
					svc.Dispose();
					svc = null;
				}

				// ����ڵ�˻�
				if (!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// ��� ��Ʈ
				contractItemModel.ContractItemDataSet = remoteData.ContractItemDataSet.Copy();
				contractItemModel.ResultCnt = remoteData.ResultCnt;
				contractItemModel.ResultCD = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("�������� �����ȸ End");
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
		/// ä�ΰ˻�
		/// </summary>
		/// <param name="contractItemModel"></param>
		public void GetChannelList(ContractItemModel contractItemModel)
		{

			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("ä�ΰ˻� Start");
				_log.Debug("-----------------------------------------");
				
				// ������ �ν��Ͻ� ����
				ContractItemServicePloxy.ContractItemService svc = new ContractItemServicePloxy.ContractItemService();
				svc.Url = _WebServiceUrl;
				// ����Ʈ �� ����
				ContractItemServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.ContractItemServicePloxy.HeaderModel();
				ContractItemServicePloxy.ContractItemModel remoteData   = new AdManagerClient.ContractItemServicePloxy.ContractItemModel();

				// ������� ��Ʈ
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;

				// ȣ������ ��Ʈ
				remoteData.SearchKey             = contractItemModel.SearchKey;
				remoteData.SearchMediaCode       = contractItemModel.SearchMediaCode;
				
				// ������ ȣ�� Ÿ�Ӿƿ�����
				svc.Timeout = FrameSystem.m_SystemTimeout;
				// ������ �޼ҵ� ȣ��
				remoteData = svc.GetChannelList(remoteHeader, remoteData);

				// ����ڵ�˻�
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// ��� ��Ʈ
				contractItemModel.ContractItemDataSet = remoteData.ContractItemDataSet.Copy();
				contractItemModel.ResultCnt   = remoteData.ResultCnt;
				contractItemModel.ResultCD    = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("ä�ΰ˻� End");
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
				_log.Debug("�������߰� Start");
				_log.Debug("-----------------------------------------");
                                        
				// ������ �ν��Ͻ� ����
				ContractItemServicePloxy.ContractItemService svc = new ContractItemServicePloxy.ContractItemService();
				svc.Url = _WebServiceUrl;        				
				// ����Ʈ �� ����
				ContractItemServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.ContractItemServicePloxy.HeaderModel();
				ContractItemServicePloxy.ContractItemModel remoteData   = new AdManagerClient.ContractItemServicePloxy.ContractItemModel();
                        
				// ������� ��Ʈ
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;
                        
				// ȣ��������Ʈ
				//Key ����
				remoteData.ItemNo            = contractItemModel.ItemNo;
				
				remoteData.FileTitle             = contractItemModel.FileTitle;   
				remoteData.FileName              = contractItemModel.FileName;   
				remoteData.FilePath				 = contractItemModel.FilePath;     
				                
				// ������ ȣ�� Ÿ�Ӿƿ�����
				svc.Timeout = FrameSystem.m_SystemTimeout;
				// ������ �޼ҵ� ȣ��
				remoteData = svc.SetFileCreate(remoteHeader, remoteData);

				if( null != svc)
				{
					svc.Dispose();
					svc = null;
				}
                        
				// ����ڵ�˻�
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}
                        
				// ��� ��Ʈ
				contractItemModel.ResultCnt   = remoteData.ResultCnt;
				_log.Debug("contractItemModel.ResultCnt = "+contractItemModel.ResultCnt);
                        			
				contractItemModel.ResultCD    = remoteData.ResultCD;
                        
				_log.Debug("-----------------------------------------");
				_log.Debug("�������߰� End");
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
				_log.Debug("�ڷ�÷�μ��� Start");
				_log.Debug("-----------------------------------------");
                                        
				// ������ �ν��Ͻ� ����
				ContractItemServicePloxy.ContractItemService svc = new ContractItemServicePloxy.ContractItemService();
				svc.Url = _WebServiceUrl;        				
				// ����Ʈ �� ����
				ContractItemServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.ContractItemServicePloxy.HeaderModel();
				ContractItemServicePloxy.ContractItemModel remoteData   = new AdManagerClient.ContractItemServicePloxy.ContractItemModel();
                        
				// ������� ��Ʈ
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;
                        
				// ȣ��������Ʈ
				//Key ����
				remoteData.ItemNo            = contractItemModel.ItemNo;
				remoteData.FileNo            = contractItemModel.FileNo;
				
				remoteData.FileTitle             = contractItemModel.FileTitle;   
				remoteData.FileName              = contractItemModel.FileName;   
				remoteData.FilePath				 = contractItemModel.FilePath;     
				                
				// ������ ȣ�� Ÿ�Ӿƿ�����
				svc.Timeout = FrameSystem.m_SystemTimeout;
				// ������ �޼ҵ� ȣ��
				remoteData = svc.SetFileUpdate(remoteHeader, remoteData);

				if( null != svc)
				{
					svc.Dispose();
					svc = null;
				}
                        
				// ����ڵ�˻�
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}
                        
				// ��� ��Ʈ
				contractItemModel.ResultCnt   = remoteData.ResultCnt;
				_log.Debug("contractItemModel.ResultCnt = "+contractItemModel.ResultCnt);
                        			
				contractItemModel.ResultCD    = remoteData.ResultCD;
                        
				_log.Debug("-----------------------------------------");
				_log.Debug("�ڷ�÷�μ��� End");
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
				_log.Debug("�ڷ�÷�λ��� Start");
				_log.Debug("-----------------------------------------");
                                        
				// ������ �ν��Ͻ� ����
				ContractItemServicePloxy.ContractItemService svc = new ContractItemServicePloxy.ContractItemService();
				svc.Url = _WebServiceUrl;        				
				// ����Ʈ �� ����
				ContractItemServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.ContractItemServicePloxy.HeaderModel();
				ContractItemServicePloxy.ContractItemModel remoteData   = new AdManagerClient.ContractItemServicePloxy.ContractItemModel();
                        
				// ������� ��Ʈ
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;
                        
				// ȣ��������Ʈ
				//Key ����
				remoteData.ItemNo            = contractItemModel.ItemNo;
				remoteData.FileNo            = contractItemModel.FileNo;
								                
				// ������ ȣ�� Ÿ�Ӿƿ�����
				svc.Timeout = FrameSystem.m_SystemTimeout;
				// ������ �޼ҵ� ȣ��
				remoteData = svc.SetFileDelete(remoteHeader, remoteData);

				if( null != svc)
				{
					svc.Dispose();
					svc = null;
				}
                        
				// ����ڵ�˻�
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}
                        
				// ��� ��Ʈ
				contractItemModel.ResultCnt   = remoteData.ResultCnt;
				_log.Debug("contractItemModel.ResultCnt = "+contractItemModel.ResultCnt);
                        			
				contractItemModel.ResultCD    = remoteData.ResultCD;
                        
				_log.Debug("-----------------------------------------");
				_log.Debug("�ڷ�÷�λ��� End");
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
		/// ������ �ű��߰�
		/// </summary>
		/// <param name="contractItemModel"></param>
        public void SetContractItemAdd(ContractItemModel contractItemModel)
        {
            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug("�������߰� Start");
                _log.Debug("-----------------------------------------");
                        
                // �Էµ������� Validation �˻�
                if(contractItemModel.ItemName.Trim().Length < 1)    throw new FrameException("���� �������� �������� �ʽ��ϴ�.");
                if(contractItemModel.ItemName.Trim().Length > 50)   throw new FrameException("���� �������� 50Byte�� �ʰ��� �� �����ϴ�.");
                        
                // ������ �ν��Ͻ� ����
                ContractItemServicePloxy.ContractItemService svc = new ContractItemServicePloxy.ContractItemService();
                svc.Url = _WebServiceUrl;        				
                // ����Ʈ �� ����
                ContractItemServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.ContractItemServicePloxy.HeaderModel();
                ContractItemServicePloxy.ContractItemModel remoteData   = new AdManagerClient.ContractItemServicePloxy.ContractItemModel();
                        
                // ������� ��Ʈ
                remoteHeader.ClientKey     = Header.ClientKey;
                remoteHeader.UserID        = Header.UserID;
                        
                // ȣ��������Ʈ
                //Key ����
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
                // ������ ȣ�� Ÿ�Ӿƿ�����
                svc.Timeout = FrameSystem.m_SystemTimeout;
                // ������ �޼ҵ� ȣ��
                remoteData = svc.SetContractItemCreate(remoteHeader, remoteData);

				if( null != svc)
				{
					svc.Dispose();
					svc = null;
				}
                        
                // ����ڵ�˻�
                if(!remoteData.ResultCD.Equals("0000"))
                {
                    throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
                }
                        
                // ��� ��Ʈ
                contractItemModel.ResultCnt   = remoteData.ResultCnt;
                _log.Debug("contractItemModel.ResultCnt = "+contractItemModel.ResultCnt);
                        			
                contractItemModel.ResultCD    = remoteData.ResultCD;
                        
                _log.Debug("-----------------------------------------");
                _log.Debug("�������߰� End");
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
        /// ���������� ����
        /// </summary>
        /// <param name="contractItemModel"></param>
        public void SetContractItemUpdate(ContractItemModel contractItemModel)
        {
            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug("�������������� Start");
                _log.Debug("-----------------------------------------");
                        
                        
                // �Էµ������� Validation �˻�
                if(contractItemModel.ItemName.Trim().Length < 1) 
                {
                    throw new FrameException("���� �������� �������� �ʽ��ϴ�.");
                }
                if(contractItemModel.ItemName.Trim().Length > 50) 
                {
                    throw new FrameException("���� �������� 50Byte�� �ʰ��� �� �����ϴ�.");
                }
                      
                        	
                        
                // ������ �ν��Ͻ� ����
                ContractItemServicePloxy.ContractItemService svc = new ContractItemServicePloxy.ContractItemService();
                svc.Url = _WebServiceUrl;                       			
                // ����Ʈ �� ����
                ContractItemServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.ContractItemServicePloxy.HeaderModel();
                ContractItemServicePloxy.ContractItemModel remoteData   = new AdManagerClient.ContractItemServicePloxy.ContractItemModel();
                        
                // ������� ��Ʈ
                remoteHeader.ClientKey     = Header.ClientKey;
                remoteHeader.UserID        = Header.UserID;
                        
                // ȣ��������Ʈ
                //Key ����     
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
				// ������������ ����
				remoteData.AdTypeChangeType		= contractItemModel.AdTypeChangeType;

                        				
                // ������ ȣ�� Ÿ�Ӿƿ�����
                svc.Timeout = FrameSystem.m_SystemTimeout;
                // ������ �޼ҵ� ȣ��
                remoteData = svc.SetContractItemUpdate(remoteHeader, remoteData);

				if( null != svc)
				{
					svc.Dispose();
					svc = null;
				}
                        
                // ����ڵ�˻�
                if(!remoteData.ResultCD.Equals("0000"))
                {
                    throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
                }
                        
                // ��� ��Ʈ
                contractItemModel.ResultCnt   = remoteData.ResultCnt;
                contractItemModel.ResultCD    = remoteData.ResultCD;
                        
                _log.Debug("-----------------------------------------");
                _log.Debug("�������������� End");
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
				_log.Debug("����ä�λ��� Start");
				_log.Debug("-----------------------------------------");
                				                     
                        
				// ������ �ν��Ͻ� ����
				ContractItemServicePloxy.ContractItemService svc = new ContractItemServicePloxy.ContractItemService();
				svc.Url = _WebServiceUrl;        				
				// ����Ʈ �� ����
				ContractItemServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.ContractItemServicePloxy.HeaderModel();
				ContractItemServicePloxy.ContractItemModel remoteData   = new AdManagerClient.ContractItemServicePloxy.ContractItemModel();
                        
				// ������� ��Ʈ
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;
                        
				// ȣ��������Ʈ
				//Key ����

				remoteData.ItemNo			= contractItemModel.ItemNo;
                remoteData.LinkChannel      = contractItemModel.LinkChannel;
                remoteData.LinkType         = contractItemModel.LinkType;	
			
                
				// ������ ȣ�� Ÿ�Ӿƿ�����
				svc.Timeout = FrameSystem.m_SystemTimeout;
				// ������ �޼ҵ� ȣ��
				remoteData = svc.SetLinkChannelDelete2(remoteHeader, remoteData);

				if( null != svc)
				{
					svc.Dispose();
					svc = null;
				}
                        
				// ����ڵ�˻�
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}
                        
				// ��� ��Ʈ
				contractItemModel.ResultCnt   = remoteData.ResultCnt;
				_log.Debug("contractItemModel.ResultCnt = "+contractItemModel.ResultCnt);
                        			
				contractItemModel.ResultCD    = remoteData.ResultCD;
                        
				_log.Debug("-----------------------------------------");
				_log.Debug("����ä�λ��� End");
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
                _log.Debug("����ä���߰� Start");
                _log.Debug("-----------------------------------------");


                // ������ �ν��Ͻ� ����
                ContractItemServicePloxy.ContractItemService svc = new ContractItemServicePloxy.ContractItemService();
                svc.Url = _WebServiceUrl;
                // ����Ʈ �� ����
                ContractItemServicePloxy.HeaderModel remoteHeader = new AdManagerClient.ContractItemServicePloxy.HeaderModel();
                ContractItemServicePloxy.ContractItemModel remoteData = new AdManagerClient.ContractItemServicePloxy.ContractItemModel();

                // ������� ��Ʈ
                remoteHeader.ClientKey = Header.ClientKey;
                remoteHeader.UserID = Header.UserID;

                // ȣ��������Ʈ
                //Key ����

                remoteData.ItemNo = contractItemModel.ItemNo;
                remoteData.LinkChannel = contractItemModel.LinkChannel;
                remoteData.LinkType = contractItemModel.LinkType;

                // ������ ȣ�� Ÿ�Ӿƿ�����
                svc.Timeout = FrameSystem.m_SystemTimeout;
                // ������ �޼ҵ� ȣ��
                remoteData = svc.SetLinkChannelCreate2(remoteHeader, remoteData);

                if (null != svc)
                {
                    svc.Dispose();
                    svc = null;
                }

                // ����ڵ�˻�
                if (!remoteData.ResultCD.Equals("0000"))
                {
                    throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
                }

                // ��� ��Ʈ
                contractItemModel.ResultCnt = remoteData.ResultCnt;
                _log.Debug("contractItemModel.ResultCnt = " + contractItemModel.ResultCnt);

                contractItemModel.ResultCD = remoteData.ResultCD;

                _log.Debug("-----------------------------------------");
                _log.Debug("����ä���߰� End");
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
        /// �����೻�� ����
        /// </summary>
        /// <param name="baseModel"></param>
        public void SetContractItemDelete(ContractItemModel contractItemModel)
        {
            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug("��������� Start");
                _log.Debug("-----------------------------------------");

                // ������ �ν��Ͻ� ����
                ContractItemServicePloxy.ContractItemService svc = new ContractItemServicePloxy.ContractItemService();
              
                // URL�� ������Ʈ
                svc.Url = _WebServiceUrl;
				
                // ����Ʈ �� ����
                ContractItemServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.ContractItemServicePloxy.HeaderModel();
                ContractItemServicePloxy.ContractItemModel remoteData   = new AdManagerClient.ContractItemServicePloxy.ContractItemModel();
              
                // ������� ��Ʈ
                remoteHeader.ClientKey  = Header.ClientKey;
                remoteHeader.UserID     = Header.UserID;

                // ȣ��������Ʈ
                remoteData.ItemNo       = contractItemModel.ItemNo;
                // ������ ȣ�� Ÿ�Ӿƿ�����
                svc.Timeout = FrameSystem.m_SystemTimeout;	
                // ������ �޼ҵ� ȣ��
                remoteData = svc.SetContractItemDelete(remoteHeader, remoteData);

                // ����ڵ�˻�
                if(!remoteData.ResultCD.Equals("0000"))
                {
                    throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
                }

                // ��� ��Ʈ
                contractItemModel.ResultCnt   = remoteData.ResultCnt;
                contractItemModel.ResultCD    = remoteData.ResultCD;

                _log.Debug("-----------------------------------------");
                _log.Debug("��������� End");
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
		/// ������ ���� 
		/// S1���� �߰�
		/// </summary>
		/// <param name="data"></param>
		public void SetContractItemCopy(ItemCopyModel	data)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("���������� Start");
				_log.Debug("-----------------------------------------");
                        
				// �Էµ������� Validation �˻�
				//if(contractItemModel.ItemName.Trim().Length < 1)    throw new FrameException("���� �������� �������� �ʽ��ϴ�.");
				//if(contractItemModel.ItemName.Trim().Length > 50)   throw new FrameException("���� �������� 50Byte�� �ʰ��� �� �����ϴ�.");
                      
                        
				// ������ �ν��Ͻ� ����
				ContractItemServicePloxy.ContractItemService svc = new ContractItemServicePloxy.ContractItemService();
				svc.Url = _WebServiceUrl;        				
				// ����Ʈ �� ����
				ContractItemServicePloxy.HeaderModel	remoteHeader = new AdManagerClient.ContractItemServicePloxy.HeaderModel();
				ContractItemServicePloxy.ItemCopyModel	remoteData   = new AdManagerClient.ContractItemServicePloxy.ItemCopyModel();
                        
				// ������� ��Ʈ
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;
                        
				// ȣ��������Ʈ
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
                        
				// ����ڵ�˻�
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}
                        
				// ��� ��Ʈ
				data.ItemNoDes	=	remoteData.ItemNoDes;
				data.ResultCnt	= remoteData.ResultCnt;
				data.ResultCD    = remoteData.ResultCD;
                        
				_log.Debug("-----------------------------------------");
				_log.Debug("���������� End");
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
        /// ������� �ϰ�����ó�� Manager
        /// </summary>        
		public void SetMultiAdState(ContractItemModel contractItemModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("������� �ϰ����� Start");
				_log.Debug("-----------------------------------------");
                        
                        
				if (contractItemModel.ContractItemDataSet.Tables[0].Rows.Count == 0)
                    throw new Exception("ó���� Data�� ���õ��� �ʾҽ��ϴ�.");                          	
                        
				// ������ �ν��Ͻ� ����
				ContractItemServicePloxy.ContractItemService svc = new ContractItemServicePloxy.ContractItemService();
				svc.Url = _WebServiceUrl;                       			
				// ����Ʈ �� ����
				ContractItemServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.ContractItemServicePloxy.HeaderModel();
				ContractItemServicePloxy.ContractItemModel remoteData   = new AdManagerClient.ContractItemServicePloxy.ContractItemModel();
                        
				// ������� ��Ʈ
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;
                        
				// ȣ��������Ʈ																
				remoteData.AdState              = contractItemModel.AdState;   
				remoteData.ContractItemDataSet  = contractItemModel.ContractItemDataSet.Copy();
								                        				
				// ������ ȣ�� Ÿ�Ӿƿ�����
				svc.Timeout = FrameSystem.m_SystemTimeout;
			    
				// ������ �޼ҵ� ȣ��
				remoteData = svc.SetMultiAdState(remoteHeader, remoteData);
                
				if( null != svc)
				{
					svc.Dispose();
					svc = null;
				}
                        
				// ����ڵ�˻�
				if(!remoteData.ResultCD.Equals(FrameSystem.DBSuccess))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}
                        
				// ��� ��Ʈ
				contractItemModel.ResultCnt   = remoteData.ResultCnt;
				contractItemModel.ResultCD    = remoteData.ResultCD;
                        
				_log.Debug("-----------------------------------------");
				_log.Debug("������� �ϰ����� End");
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
		/// ������ä�� �ϰ�ó�� Manager
		/// </summary>		
		public void SetMultiChannel(ContractItemModel contractItemModel)
		{
		}

        /// <summary>
        /// ��󱤰� ���� ����( 1106���� �߰�)
        /// </summary>
        /// <param name="contractItemModel"></param>
        public void SetLinkItemDelete(ContractItemModel contractItemModel)
        {
            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug("��󱤰���� Start");
                _log.Debug("-----------------------------------------");


                // ������ �ν��Ͻ� ����
                ContractItemServicePloxy.ContractItemService svc = new ContractItemServicePloxy.ContractItemService();
                svc.Url = _WebServiceUrl;
                // ����Ʈ �� ����
                ContractItemServicePloxy.HeaderModel remoteHeader = new AdManagerClient.ContractItemServicePloxy.HeaderModel();
                ContractItemServicePloxy.ContractItemModel remoteData = new AdManagerClient.ContractItemServicePloxy.ContractItemModel();

                // ������� ��Ʈ
                remoteHeader.ClientKey = Header.ClientKey;
                remoteHeader.UserID = Header.UserID;

                // ������ �ε������ȣ�� LinkChannel�׸��� ����Ѵ�.
                remoteData.ItemNo = contractItemModel.ItemNo;
                remoteData.LinkChannel = contractItemModel.LinkChannel;

                // ������ ȣ�� Ÿ�Ӿƿ�����
                svc.Timeout = FrameSystem.m_SystemTimeout;
                // ������ �޼ҵ� ȣ��
                remoteData = svc.SetLinkItemDelete(remoteHeader, remoteData);

                if (null != svc)
                {
                    svc.Dispose();
                    svc = null;
                }

                // ����ڵ�˻�
                if (!remoteData.ResultCD.Equals("0000"))
                {
                    throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
                }

                // ��� ��Ʈ
                contractItemModel.ResultCnt = remoteData.ResultCnt;
                _log.Debug("contractItemModel.ResultCnt = " + contractItemModel.ResultCnt);

                contractItemModel.ResultCD = remoteData.ResultCD;

                _log.Debug("-----------------------------------------");
                _log.Debug("��󱤰���� End");
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
        /// ��󱤰� ���� �߰�( 1106���� �߰�)
        /// </summary>
        /// <param name="contractItemModel"></param>
        public void SetLinkItemCreate(ContractItemModel contractItemModel)
        {
            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug("��󱤰� ���� Start");
                _log.Debug("-----------------------------------------");


                // ������ �ν��Ͻ� ����
                ContractItemServicePloxy.ContractItemService svc = new ContractItemServicePloxy.ContractItemService();
                svc.Url = _WebServiceUrl;
                // ����Ʈ �� ����
                ContractItemServicePloxy.HeaderModel remoteHeader = new AdManagerClient.ContractItemServicePloxy.HeaderModel();
                ContractItemServicePloxy.ContractItemModel remoteData = new AdManagerClient.ContractItemServicePloxy.ContractItemModel();

                // ������� ��Ʈ
                remoteHeader.ClientKey = Header.ClientKey;
                remoteHeader.UserID = Header.UserID;

                // ������ �ε������ȣ�� LinkChannel�׸��� ����Ѵ�.
                remoteData.ItemNo = contractItemModel.ItemNo;
                remoteData.LinkChannel = contractItemModel.LinkChannel;

                // ������ ȣ�� Ÿ�Ӿƿ�����
                svc.Timeout = FrameSystem.m_SystemTimeout;
                remoteData = svc.SetLinkItemCreate(remoteHeader, remoteData);

                if (null != svc)
                {
                    svc.Dispose();
                    svc = null;
                }

                // ����ڵ�˻�
                if (!remoteData.ResultCD.Equals("0000"))
                {
                    throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
                }

                // ��� ��Ʈ
                contractItemModel.ResultCnt = remoteData.ResultCnt;
                _log.Debug("contractItemModel.ResultCnt = " + contractItemModel.ResultCnt);

                contractItemModel.ResultCD = remoteData.ResultCD;

                _log.Debug("-----------------------------------------");
                _log.Debug("��󱤰� End");
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