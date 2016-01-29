// ===============================================================================
//
// MenuManager.cs
//
// �޴����� ���񽺸� ȣ���մϴ�. 
//
// ===============================================================================
// Release history
//
// ===============================================================================
// Copyright (C) 2006 G-Inno Systems Inc.
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
	/// �޴����� �����񽺸� ȣ���մϴ�. 
	/// </summary>
	public class MenuManager : BaseManager
	{
		/// <summary>
		/// ������
		/// </summary>
		/// <param name="systemModel"></param>
		/// <param name="commonModel"></param>
		public MenuManager(SystemModel systemModel, CommonModel commonModel) : base(systemModel, commonModel)
		{
			_log	= FrameSystem.oLog;
			_Host	= FrameSystem.m_WebServer_Host;
			_Port	= FrameSystem.m_WebServer_Port;
			_Path	= FrameSystem.m_WebServer_App + "/Common/MenuService.asmx";
		}

        /// <summary>
        /// �޴����� ��� ��ȸ
        /// </summary>
        /// <param name="baseModel"></param>
        public void GetUserMenuList(MenuModel menuModel)
        {
            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug("����ڸ޴���ȸ Start");
                _log.Debug("-----------------------------------------");

                // ������ �ν��Ͻ� ����
                MenuServicePloxy.MenuService svc = new MenuServicePloxy.MenuService();
                svc.Url = _WebServiceUrl;
			
                // ����Ʈ �� ����
                MenuServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.MenuServicePloxy.HeaderModel();
                MenuServicePloxy.MenuModel     remoteData   = new AdManagerClient.MenuServicePloxy.MenuModel();

                // ������� ��Ʈ
                remoteHeader.ClientKey     = Header.ClientKey;
                remoteHeader.UserID        = Header.UserID;
                remoteHeader.UserLevel     = Header.UserLevel;
                remoteHeader.UserClass	   = Header.UserClass;
			
                // ������ ȣ�� Ÿ�Ӿƿ�����
                svc.Timeout = FrameSystem.m_SystemTimeout;
                // ������ �޼ҵ� ȣ��
                remoteData = svc.GetUserMenuList(remoteHeader, remoteData);

                // ����ڵ�˻�
                if(!remoteData.ResultCD.Equals("0000"))
                {
                    throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
                }

                // ��� ��Ʈ
                menuModel.MenuDataSet = remoteData.MenuDataSet.Copy();
                menuModel.ResultCnt   = remoteData.ResultCnt;
                menuModel.ResultCD    = remoteData.ResultCD;

                _log.Debug("-----------------------------------------");
                _log.Debug("����ڸ޴���ȸ End");
                _log.Debug("-----------------------------------------");
            }
            catch(FrameException fe)
            {
                _log.Warning("-----------------------------------------");
                _log.Warning( this.ToString() + ":GetUserMenuList():" + fe.ErrCode + ":" + fe.ResultMsg);
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
        /// �޴����� ��� ��ȸ
        /// </summary>
        /// <param name="baseModel"></param>
        public void GetUserClassList(MenuModel menuModel)
        {
            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug("����ڸ޴���ȸ Start");
                _log.Debug("-----------------------------------------");

                // ������ �ν��Ͻ� ����
                MenuServicePloxy.MenuService svc = new MenuServicePloxy.MenuService();
                svc.Url = _WebServiceUrl;
			
                // ����Ʈ �� ����
                MenuServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.MenuServicePloxy.HeaderModel();
                MenuServicePloxy.MenuModel     remoteData   = new AdManagerClient.MenuServicePloxy.MenuModel();

                // ������� ��Ʈ
                remoteHeader.ClientKey     = Header.ClientKey;
                remoteHeader.UserID        = Header.UserID;
                remoteHeader.UserLevel     = Header.UserLevel;
                remoteHeader.UserClass	   = Header.UserClass;
			
                // ������ ȣ�� Ÿ�Ӿƿ�����
                svc.Timeout = FrameSystem.m_SystemTimeout;
                // ������ �޼ҵ� ȣ��
                remoteData = svc.GetUserClassList(remoteHeader, remoteData);

                // ����ڵ�˻�
                if(!remoteData.ResultCD.Equals("0000"))
                {
                    throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
                }

                // ��� ��Ʈ
                menuModel.MenuDataSet = remoteData.MenuDataSet.Copy();
                menuModel.ResultCnt   = remoteData.ResultCnt;
                menuModel.ResultCD    = remoteData.ResultCD;

                _log.Debug("-----------------------------------------");
                _log.Debug("����ڸ޴���ȸ End");
                _log.Debug("-----------------------------------------");
            }
            catch(FrameException fe)
            {
                _log.Warning("-----------------------------------------");
                _log.Warning( this.ToString() + ":GetUserMenuList():" + fe.ErrCode + ":" + fe.ResultMsg);
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
        /// �޴����� ��ȸ
        /// </summary>
        /// <param name="baseModel"></param>
        public void GetMenuPowerList(MenuModel menuModel)
        {
            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug("����ڸ޴���ȸ Start");
                _log.Debug("-----------------------------------------");

                // ������ �ν��Ͻ� ����
                MenuServicePloxy.MenuService svc = new MenuServicePloxy.MenuService();
                svc.Url = _WebServiceUrl;
			
                // ����Ʈ �� ����
                MenuServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.MenuServicePloxy.HeaderModel();
                MenuServicePloxy.MenuModel     remoteData   = new AdManagerClient.MenuServicePloxy.MenuModel();

                // ������� ��Ʈ
                remoteHeader.ClientKey     = Header.ClientKey;
                remoteHeader.UserID        = Header.UserID;
                remoteHeader.UserLevel     = Header.UserLevel;
                remoteHeader.UserClass	   = Header.UserClass;
			
                // ȣ������ ��Ʈ
                remoteData.UserClassCode         = menuModel.UserClassCode;


                // ������ ȣ�� Ÿ�Ӿƿ�����
                svc.Timeout = FrameSystem.m_SystemTimeout;
                // ������ �޼ҵ� ȣ��
                remoteData = svc.GetMenuPowerList(remoteHeader, remoteData);

                // ����ڵ�˻�
                if(!remoteData.ResultCD.Equals("0000"))
                {
                    throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
                }

                // ��� ��Ʈ
                menuModel.MenuDataSet = remoteData.MenuDataSet.Copy();
                menuModel.ResultCnt   = remoteData.ResultCnt;
                menuModel.ResultCD    = remoteData.ResultCD;

                _log.Debug("-----------------------------------------");
                _log.Debug("����ڸ޴���ȸ End");
                _log.Debug("-----------------------------------------");
            }
            catch(FrameException fe)
            {
                _log.Warning("-----------------------------------------");
                _log.Warning( this.ToString() + ":GetUserMenuList():" + fe.ErrCode + ":" + fe.ResultMsg);
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


        public void SetUserClassAdd(MenuModel menuModel)
        {
            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug("����� ���� �߰� Start");
                _log.Debug("-----------------------------------------");

            
                // ������ �ν��Ͻ� ����
                MenuServicePloxy.MenuService svc = new MenuServicePloxy.MenuService();
                svc.Url = _WebServiceUrl;			
                // ����Ʈ �� ����
                MenuServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.MenuServicePloxy.HeaderModel();
                MenuServicePloxy.MenuModel remoteData   = new AdManagerClient.MenuServicePloxy.MenuModel();
            
                // ������� ��Ʈ
                remoteHeader.ClientKey     = Header.ClientKey;
                remoteHeader.UserID        = Header.UserID;
            
                // ȣ��������Ʈ
                remoteData.UserClassCode        = menuModel.UserClassCode;
                remoteData.UserClassName    = menuModel.UserClassName;

                // ������ ȣ�� Ÿ�Ӿƿ�����
                svc.Timeout = FrameSystem.m_SystemTimeout;
                // ������ �޼ҵ� ȣ��
                remoteData = svc.SetUserClassCreate(remoteHeader, remoteData);
            
                // ����ڵ�˻�
                if(!remoteData.ResultCD.Equals("0000"))
                {
                    throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
                }
            
                // ��� ��Ʈ
                menuModel.ResultCnt   = remoteData.ResultCnt;
                _log.Debug("menuModel.ResultCnt = "+menuModel.ResultCnt);
            			
                menuModel.ResultCD    = remoteData.ResultCD;
            
                _log.Debug("-----------------------------------------");
                _log.Debug("����� ���� �߰� End");
                _log.Debug("-----------------------------------------");
            }
            catch(FrameException fe)
            {
                menuModel.ResultCD    = "3101";
                _log.Warning("-----------------------------------------");
                _log.Warning( this.ToString() + ":SetMenuCreate():" + fe.ErrCode + ":" + fe.ResultMsg);
                _log.Warning("-----------------------------------------");
                throw fe;
            }
            catch(Exception e)
            {
                menuModel.ResultCD    = "3101";
                _log.Error("-----------------------------------------");
                _log.Exception(e);
                _log.Error("-----------------------------------------");
                throw new FrameException(e.Message);
            }
        }

        public void SetUserClassUpdate(MenuModel menuModel)
        {
            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug("����� ���� �߰� Start");
                _log.Debug("-----------------------------------------");

            
                // ������ �ν��Ͻ� ����
                MenuServicePloxy.MenuService svc = new MenuServicePloxy.MenuService();
                svc.Url = _WebServiceUrl;			
                // ����Ʈ �� ����
                MenuServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.MenuServicePloxy.HeaderModel();
                MenuServicePloxy.MenuModel remoteData   = new AdManagerClient.MenuServicePloxy.MenuModel();
            
                // ������� ��Ʈ
                remoteHeader.ClientKey     = Header.ClientKey;
                remoteHeader.UserID        = Header.UserID;
            
                // ȣ��������Ʈ
                remoteData.UserClassCode   = menuModel.UserClassCode;
                remoteData.UserClassName   = menuModel.UserClassName;
                remoteData.MenuDataSet     = menuModel.MenuDataSet.Copy();

                // ������ ȣ�� Ÿ�Ӿƿ�����
                svc.Timeout = FrameSystem.m_SystemTimeout;
                // ������ �޼ҵ� ȣ��
                remoteData = svc.SetUserClassUpdate(remoteHeader, remoteData);
            
                // ����ڵ�˻�
                if(!remoteData.ResultCD.Equals("0000"))
                {
                    throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
                }
            
                // ��� ��Ʈ
                menuModel.ResultCnt   = remoteData.ResultCnt;
                _log.Debug("menuModel.ResultCnt = "+menuModel.ResultCnt);
            			
                menuModel.ResultCD    = remoteData.ResultCD;
            
                _log.Debug("-----------------------------------------");
                _log.Debug("�帣�׷��߰� End");
                _log.Debug("-----------------------------------------");
            }
            catch(FrameException fe)
            {
                menuModel.ResultCD    = "3101";
                _log.Warning("-----------------------------------------");
                _log.Warning( this.ToString() + ":SetMenuCreate():" + fe.ErrCode + ":" + fe.ResultMsg);
                _log.Warning("-----------------------------------------");
                throw fe;
            }
            catch(Exception e)
            {
                menuModel.ResultCD    = "3101";
                _log.Error("-----------------------------------------");
                _log.Exception(e);
                _log.Error("-----------------------------------------");
                throw new FrameException(e.Message);
            }
        }


        /// <summary>
        /// �帣�׷����� ����
        /// </summary>
        /// <param name="menuModel"></param>
        public void SetMenuPowerUpdate(MenuModel menuModel)
        {
            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug("�޴����� �������� Start");
                _log.Debug("-----------------------------------------");
            
                // ������ �ν��Ͻ� ����
                MenuServicePloxy.MenuService svc = new MenuServicePloxy.MenuService();
                svc.Url = _WebServiceUrl;		
                // ����Ʈ �� ����
                MenuServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.MenuServicePloxy.HeaderModel();
                MenuServicePloxy.MenuModel remoteData   = new AdManagerClient.MenuServicePloxy.MenuModel();
            
                // ������� ��Ʈ
                remoteHeader.ClientKey     = Header.ClientKey;
                remoteHeader.UserID        = Header.UserID;
            
                // ȣ��������Ʈ
                remoteData.MenuDataSet    = menuModel.MenuDataSet.Copy();
            				
                // ������ ȣ�� Ÿ�Ӿƿ�����
                svc.Timeout = FrameSystem.m_SystemTimeout;
                // ������ �޼ҵ� ȣ��
                remoteData = svc.SetMenuPowerUpdate(remoteHeader, remoteData);
            
                // ����ڵ�˻�
                if(!remoteData.ResultCD.Equals("0000"))
                {
                    throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
                }
            
                // ��� ��Ʈ
                menuModel.ResultCnt   = remoteData.ResultCnt;
                menuModel.ResultCD    = remoteData.ResultCD;
            
                _log.Debug("-----------------------------------------");
                _log.Debug("�޴����� �������� End");
                _log.Debug("-----------------------------------------");
            }
            catch(FrameException fe)
            {
                _log.Warning("-----------------------------------------");
                _log.Warning( this.ToString() + ":SetMenuPowerUpdate():" + fe.ErrCode + ":" + fe.ResultMsg);
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
