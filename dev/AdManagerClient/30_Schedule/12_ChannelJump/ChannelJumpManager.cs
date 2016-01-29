// ===============================================================================
// ChannelJump Manager  for Charites Project
//
// ChannelJumpManager.cs
//
// ä������ ���񽺸� ȣ���մϴ�. 
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
    /// ä���������� �����񽺸� ȣ���մϴ�. 
    /// </summary>
    public class ChannelJumpManager : BaseManager
    {
        #region [ �� �� �� ]
        /// <summary>
        /// ������
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

        #region[ �޼ҵ� : ä������ ]

        /// <summary>
        /// ä���������� ��������(����)
        /// </summary>
        /// <param name="channelJumpModel"></param>
        public void GetChannelJump(ChannelJumpModel channelJumpModel)
        {
            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug("ä������������ȸ Start");
                _log.Debug("-----------------------------------------");
				
                // ������ �ν��Ͻ� ����
                ChannelJumpServicePloxy.ChannelJumpService svc = new ChannelJumpServicePloxy.ChannelJumpService();
                svc.Url = _WebServiceUrl;
                // ����Ʈ �� ����
                ChannelJumpServicePloxy.HeaderModel         remoteHeader = new AdManagerClient.ChannelJumpServicePloxy.HeaderModel();
                ChannelJumpServicePloxy.ChannelJumpModel    remoteData   = new AdManagerClient.ChannelJumpServicePloxy.ChannelJumpModel();

                // ������� ��Ʈ
                remoteHeader.ClientKey     = Header.ClientKey;
                remoteHeader.UserID        = Header.UserID;

                // ȣ������ ��Ʈ
                remoteData.ItemNo           = channelJumpModel.ItemNo;
                remoteData.JumpType         = channelJumpModel.JumpType;
				
                // ������ ȣ�� Ÿ�Ӿƿ�����
                svc.Timeout = FrameSystem.m_SystemTimeout;
                // ������ �޼ҵ� ȣ��
                remoteData = svc.GetChannelJump(remoteHeader, remoteData);

                // ����ڵ�˻�
                if(!remoteData.ResultCD.Equals("0000"))
                {
                    throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
                }

                // ��� ��Ʈ
                channelJumpModel.PopupID    = remoteData.PopupID;
                channelJumpModel.ResultCnt  = remoteData.ResultCnt;
                channelJumpModel.ResultCD   = remoteData.ResultCD;

                _log.Debug("-----------------------------------------");
                _log.Debug("ä�����θ����ȸ End");
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
        /// ����������Ʈ �˾� ���� URL DB���� ��������
        /// </summary>
        /// <returns></returns>
        public string GetContentListPopUrl()
        {
            string rtnValue = "";
            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug("ä������������ȸ Start");
                _log.Debug("-----------------------------------------");
				
                // ������ �ν��Ͻ� ����
                ChannelJumpServicePloxy.ChannelJumpService svc = new ChannelJumpServicePloxy.ChannelJumpService();
                svc.Url     = _WebServiceUrl;
                svc.Timeout = FrameSystem.m_SystemTimeout;
                rtnValue    = svc.GetContentListPopUrl();

                if( rtnValue.Length < 10 )
                {
                    throw new FrameException("����������Ʈ�˾�URL�� �д��� ������ �߻��Ͽ����ϴ�", _module, "9999");
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
        /// ä���������� ��������(���)
        /// </summary>
        /// <param name="channelJumpModel"></param>
        public void GetChannelJumpList(ChannelJumpModel channelJumpModel)
        {

            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug("ä�����θ����ȸ Start");
                _log.Debug("-----------------------------------------");
				
                // ������ �ν��Ͻ� ����
                ChannelJumpServicePloxy.ChannelJumpService svc = new ChannelJumpServicePloxy.ChannelJumpService();
			    svc.Url = _WebServiceUrl;
                // ����Ʈ �� ����
                ChannelJumpServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.ChannelJumpServicePloxy.HeaderModel();
                ChannelJumpServicePloxy.ChannelJumpModel remoteData   = new AdManagerClient.ChannelJumpServicePloxy.ChannelJumpModel();

                // ������� ��Ʈ
                remoteHeader.ClientKey     = Header.ClientKey;
                remoteHeader.UserID        = Header.UserID;

                // ȣ������ ��Ʈ
                remoteData.SearchKey             = channelJumpModel.SearchKey;
                remoteData.SearchMediaCode       = channelJumpModel.SearchMediaCode;
                remoteData.SearchRapCode         = channelJumpModel.SearchRapCode;        
                remoteData.SearchchkAdState_10	 = channelJumpModel.SearchchkAdState_10; 
                remoteData.SearchchkAdState_20	 = channelJumpModel.SearchchkAdState_20; 
                remoteData.SearchchkAdState_30	 = channelJumpModel.SearchchkAdState_30; 
                remoteData.SearchchkAdState_40	 = channelJumpModel.SearchchkAdState_40; 
				remoteData.SearchAdType          = channelJumpModel.SearchAdType;          
				remoteData.SearchJumpType		 = channelJumpModel.SearchJumpType; 
				
				// ������ ȣ�� Ÿ�Ӿƿ�����
                svc.Timeout = FrameSystem.m_SystemTimeout;
                // ������ �޼ҵ� ȣ��
                remoteData = svc.GetChannelJumpList(remoteHeader, remoteData);

                // ����ڵ�˻�
                if(!remoteData.ResultCD.Equals("0000"))
                {
                    throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
                }

                // ��� ��Ʈ
                channelJumpModel.ChannelJumpDataSet = remoteData.ChannelJumpDataSet.Copy();
                channelJumpModel.ResultCnt   = remoteData.ResultCnt;
                channelJumpModel.ResultCD    = remoteData.ResultCD;

                _log.Debug("-----------------------------------------");
                _log.Debug("ä�����θ����ȸ End");
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
        /// [M]ä������ �߰�
        /// </summary>
        /// <param name="channelJumpModel"></param>
        public void SetChannelJumpCreate(ChannelJumpModel channelJumpModel)
        {
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("ä�������߰� Start");
				_log.Debug("-----------------------------------------");
                        
				// �Էµ������� Validation �˻�
				if(channelJumpModel.ItemNo.Length < 1)  throw new FrameException("���� ���õ��� �ʽ��ϴ�.");
                      
				// ������ �ν��Ͻ� ����
				ChannelJumpServicePloxy.ChannelJumpService svc = new ChannelJumpServicePloxy.ChannelJumpService();
				svc.Url = _WebServiceUrl;        				
				// ����Ʈ �� ����
				ChannelJumpServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.ChannelJumpServicePloxy.HeaderModel();
				ChannelJumpServicePloxy.ChannelJumpModel remoteData   = new AdManagerClient.ChannelJumpServicePloxy.ChannelJumpModel();
                        
				// ������� ��Ʈ
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;
                        
				// ȣ��������Ʈ
				//Key ����
				remoteData.ItemNo     = channelJumpModel.ItemNo;		// �����ȣ
				remoteData.ItemName   = channelJumpModel.ItemName;		// �����
				remoteData.MediaCode  = channelJumpModel.MediaCode;		// ��ü�ڵ�
				remoteData.JumpType   = channelJumpModel.JumpType;		// ��������
				remoteData.GenreCode  = channelJumpModel.GenreCode;		// �帣�ڵ�    
				remoteData.GenreName  = channelJumpModel.GenreName;		// �帣��
				remoteData.ChannelNo  = channelJumpModel.ChannelNo;		// ä�ι�ȣ
				remoteData.Title      = channelJumpModel.Title;			// ���α׷���  
				remoteData.ContentID  = channelJumpModel.ContentID;		// ������ID
				remoteData.PopupID    = channelJumpModel.PopupID;		// ����ID
				remoteData.PopupTitle = channelJumpModel.PopupTitle;	        // ��������
                remoteData.ChannelManager = channelJumpModel.ChannelManager;    //
				remoteData.HomeYn     = channelJumpModel.HomeYn;		        // Ȩ ���⿩��
				remoteData.ChannelYn  = channelJumpModel.ChannelYn;		        // ä�� ���⿩��
				remoteData.StbTypeYn = channelJumpModel.StbTypeYn;
				remoteData.StbTypeString = channelJumpModel.StbTypeString;
                
				// ������ ȣ�� Ÿ�Ӿƿ�����
				svc.Timeout = FrameSystem.m_SystemTimeout;
				// ������ �޼ҵ� ȣ��
				remoteData = svc.SetChannelJumpCreate(remoteHeader, remoteData);
                        
				// ����ڵ�˻�
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}
                        
				// ��� ��Ʈ
				channelJumpModel.ResultCnt   = remoteData.ResultCnt;
				_log.Debug("channelJumpModel.ResultCnt = "+channelJumpModel.ResultCnt);
                        			
				channelJumpModel.ResultCD    = remoteData.ResultCD;
                        
				_log.Debug("-----------------------------------------");
				_log.Debug("ä�������߰� End");
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
        /// [M]ä���������� ����
        /// </summary>
        /// <param name="channelJumpModel"></param>
        public void SetChannelJumpUpdate(ChannelJumpModel channelJumpModel)
        {
            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug("ä�������������� Start");
                _log.Debug("-----------------------------------------");
                        
				if(channelJumpModel.ItemNo.Length < 1)  throw new FrameException("���� ���õ��� �ʽ��ϴ�.");
                         
                ChannelJumpServicePloxy.ChannelJumpService svc = new ChannelJumpServicePloxy.ChannelJumpService();
                svc.Url = _WebServiceUrl;                       			
                ChannelJumpServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.ChannelJumpServicePloxy.HeaderModel();
                ChannelJumpServicePloxy.ChannelJumpModel remoteData   = new AdManagerClient.ChannelJumpServicePloxy.ChannelJumpModel();
                        
                // ������� ��Ʈ
                remoteHeader.ClientKey     = Header.ClientKey;
                remoteHeader.UserID        = Header.UserID;
                        
                // ȣ��������Ʈ
				remoteData.ItemNo     = channelJumpModel.ItemNo;		// �����ȣ
				remoteData.ItemName   = channelJumpModel.ItemName;		// �����
				remoteData.MediaCode  = channelJumpModel.MediaCode;		// ��ü�ڵ�
				remoteData.JumpType   = channelJumpModel.JumpType;		// ��������
				remoteData.GenreCode  = channelJumpModel.GenreCode;		// �帣�ڵ�    
				remoteData.GenreName  = channelJumpModel.GenreName;		// �帣��
				remoteData.ChannelNo  = channelJumpModel.ChannelNo;		// ä�ι�ȣ
				remoteData.Title      = channelJumpModel.Title;			// ���α׷��� / ���� 
				remoteData.ContentID  = channelJumpModel.ContentID;		// ������ID
				remoteData.PopupID    = channelJumpModel.PopupID;		// ����ID
				remoteData.PopupTitle = channelJumpModel.PopupTitle;	// ��������
                remoteData.ChannelManager = channelJumpModel.ChannelManager;    //
				remoteData.HomeYn     = channelJumpModel.HomeYn;		// Ȩ ���⿩��
				remoteData.ChannelYn  = channelJumpModel.ChannelYn;		// ä�� ���⿩��
				remoteData.StbTypeYn = channelJumpModel.StbTypeYn;
				remoteData.StbTypeString = channelJumpModel.StbTypeString;

                           				
                svc.Timeout = FrameSystem.m_SystemTimeout;
                remoteData = svc.SetChannelJumpUpdate(remoteHeader, remoteData);
                        
                if(!remoteData.ResultCD.Equals("0000")) throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
                        
                channelJumpModel.ResultCnt   = remoteData.ResultCnt;
                channelJumpModel.ResultCD    = remoteData.ResultCD;
                        
                _log.Debug("-----------------------------------------");
                _log.Debug("ä�������������� End");
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
        /// ä������ ����
        /// </summary>
        /// <param name="baseModel"></param>
        public void SetChannelJumpDelete(ChannelJumpModel channelJumpModel)
        {
            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug("ä������ ���� Start");
                _log.Debug("-----------------------------------------");

                // ������ �ν��Ͻ� ����
                ChannelJumpServicePloxy.ChannelJumpService svc = new ChannelJumpServicePloxy.ChannelJumpService();
              
                // URL�� ������Ʈ
                svc.Url = _WebServiceUrl;
				
                // ����Ʈ �� ����
                ChannelJumpServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.ChannelJumpServicePloxy.HeaderModel();
                ChannelJumpServicePloxy.ChannelJumpModel remoteData   = new AdManagerClient.ChannelJumpServicePloxy.ChannelJumpModel();
              
                // ������� ��Ʈ
                remoteHeader.ClientKey  = Header.ClientKey;
                remoteHeader.UserID     = Header.UserID;

                // ȣ��������Ʈ
				remoteData.ItemNo    = channelJumpModel.ItemNo;     // �����ȣ
				remoteData.ItemName  = channelJumpModel.ItemName;	// �����
				remoteData.MediaCode = channelJumpModel.MediaCode;	// ��ü�ڵ�
				
				// ������ ȣ�� Ÿ�Ӿƿ�����
                svc.Timeout = FrameSystem.m_SystemTimeout;	
                // ������ �޼ҵ� ȣ��
                remoteData = svc.SetChannelJumpDelete(remoteHeader, remoteData);

                // ����ڵ�˻�
                if(!remoteData.ResultCD.Equals("0000"))
                {
                    throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
                }

                // ��� ��Ʈ
                channelJumpModel.ResultCnt   = remoteData.ResultCnt;
                channelJumpModel.ResultCD    = remoteData.ResultCD;

                _log.Debug("-----------------------------------------");
                _log.Debug("ä�����λ��� End");
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

        #region [ �޼ҵ� : �˻��� ]
		/// <summary>
		/// ����˻�
		/// </summary>
		/// <param name="channelJumpModel"></param>
		public void GetContractItemList(ChannelJumpModel channelJumpModel)
		{

			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("����˻� Start");
				_log.Debug("-----------------------------------------");
				
				// ������ �ν��Ͻ� ����
				ChannelJumpServicePloxy.ChannelJumpService svc = new ChannelJumpServicePloxy.ChannelJumpService();
				svc.Url = _WebServiceUrl;
				// ����Ʈ �� ����
				ChannelJumpServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.ChannelJumpServicePloxy.HeaderModel();
				ChannelJumpServicePloxy.ChannelJumpModel remoteData   = new AdManagerClient.ChannelJumpServicePloxy.ChannelJumpModel();

				// ������� ��Ʈ
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;

				// ȣ������ ��Ʈ
				remoteData.SearchKey             = channelJumpModel.SearchKey;
				remoteData.SearchMediaCode       = channelJumpModel.SearchMediaCode;
				remoteData.SearchRapCode         = channelJumpModel.SearchRapCode;        
				remoteData.SearchchkAdState_10	 = channelJumpModel.SearchchkAdState_10; 
				remoteData.SearchchkAdState_20	 = channelJumpModel.SearchchkAdState_20; 
				remoteData.SearchchkAdState_30	 = channelJumpModel.SearchchkAdState_30; 
				remoteData.SearchchkAdState_40	 = channelJumpModel.SearchchkAdState_40; 
				
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
				channelJumpModel.ContractItemDataSet = remoteData.ContractItemDataSet.Copy();
				channelJumpModel.ResultCnt   = remoteData.ResultCnt;
				channelJumpModel.ResultCD    = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("����˻� End");
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
		/// ä�ΰ˻�
		/// </summary>
		/// <param name="channelJumpModel"></param>
		public void GetChannelList(ChannelJumpModel channelJumpModel)
		{

			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("ä�ΰ˻� Start");
				_log.Debug("-----------------------------------------");
				
				// ������ �ν��Ͻ� ����
				ChannelJumpServicePloxy.ChannelJumpService svc = new ChannelJumpServicePloxy.ChannelJumpService();
				svc.Url = _WebServiceUrl;
				// ����Ʈ �� ����
				ChannelJumpServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.ChannelJumpServicePloxy.HeaderModel();
				ChannelJumpServicePloxy.ChannelJumpModel remoteData   = new AdManagerClient.ChannelJumpServicePloxy.ChannelJumpModel();

				// ������� ��Ʈ
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;

				// ȣ������ ��Ʈ
				remoteData.SearchKey             = channelJumpModel.SearchKey;
				remoteData.SearchMediaCode       = channelJumpModel.SearchMediaCode;
				
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
				channelJumpModel.ChannelListDataSet = remoteData.ChannelListDataSet.Copy();
				channelJumpModel.ResultCnt   = remoteData.ResultCnt;
				channelJumpModel.ResultCD    = remoteData.ResultCD;

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


		/// <summary>
		/// �������˻�
		/// </summary>
		/// <param name="channelJumpModel"></param>
		public void GetContentList(ChannelJumpModel channelJumpModel)
		{

			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("�������˻� Start");
				_log.Debug("-----------------------------------------");
				
				// ������ �ν��Ͻ� ����
				ChannelJumpServicePloxy.ChannelJumpService svc = new ChannelJumpServicePloxy.ChannelJumpService();
				svc.Url = _WebServiceUrl;
				// ����Ʈ �� ����
				ChannelJumpServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.ChannelJumpServicePloxy.HeaderModel();
				ChannelJumpServicePloxy.ChannelJumpModel remoteData   = new AdManagerClient.ChannelJumpServicePloxy.ChannelJumpModel();

				// ������� ��Ʈ
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;

				// ȣ������ ��Ʈ
				remoteData.SearchKey             = channelJumpModel.SearchKey;
				remoteData.SearchMediaCode       = channelJumpModel.SearchMediaCode;
				
				// ������ ȣ�� Ÿ�Ӿƿ�����
				svc.Timeout = FrameSystem.m_SystemTimeout;
				// ������ �޼ҵ� ȣ��
				remoteData = svc.GetContentList(remoteHeader, remoteData);

				// ����ڵ�˻�
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// ��� ��Ʈ
				channelJumpModel.ContentListDataSet = remoteData.ContentListDataSet.Copy();
				channelJumpModel.ResultCnt   = remoteData.ResultCnt;
				channelJumpModel.ResultCD    = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("�������˻� End");
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
		/// �˾���������Ʈ ��ȸ
		/// </summary>
		/// <param name="channelJumpModel"></param>
		public void GetAdPopList(ChannelJumpModel channelJumpModel)
		{

			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("�˾���������Ʈ ��ȸ Start");
				_log.Debug("-----------------------------------------");
				
				// ������ �ν��Ͻ� ����
				ChannelJumpServicePloxy.ChannelJumpService svc = new ChannelJumpServicePloxy.ChannelJumpService();
				svc.Url = _WebServiceUrl;
				// ����Ʈ �� ����
				ChannelJumpServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.ChannelJumpServicePloxy.HeaderModel();
				ChannelJumpServicePloxy.ChannelJumpModel remoteData   = new AdManagerClient.ChannelJumpServicePloxy.ChannelJumpModel();

				// ������� ��Ʈ
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;

				// ȣ������ ��Ʈ
				remoteData.Type       = channelJumpModel.Type;
				
				// ������ ȣ�� Ÿ�Ӿƿ�����
				svc.Timeout = FrameSystem.m_SystemTimeout * 5; // Ÿ �ý����� �����ؾ��ϹǷ� ���...
				// ������ �޼ҵ� ȣ��
				remoteData = svc.GetAdPopList(remoteHeader, remoteData);

				// ����ڵ�˻�
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// ��� ��Ʈ
				if(remoteData.ResultCnt > 0)
				{
					channelJumpModel.AdPopListDataSet = remoteData.AdPopListDataSet.Copy();
				}
				channelJumpModel.ResultCnt   = remoteData.ResultCnt;
				channelJumpModel.ResultCD    = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("�������˻� End");
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