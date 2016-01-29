// ===============================================================================
//
// SystemMenuManager.cs
//
// ����޴���ȸ ���񽺸� ȣ���մϴ�. 
// �ں���
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
	/// ��������� �����񽺸� ȣ���մϴ�. 
	/// </summary>
	public class SystemMenuManager : BaseManager
	{
		const int ORDER_UP    = 3;
		const int ORDER_DOWN  = 4;
		/// <summary>
		/// ������
		/// </summary>
		/// <param name="systemModel"></param>
		/// <param name="commonModel"></param>

		public SystemMenuManager(SystemModel systemModel, CommonModel commonModel) : base(systemModel, commonModel)
		{
			_log = FrameSystem.oLog;
			_Host  = FrameSystem.m_WebServer_Host;
			_Port  = FrameSystem.m_WebServer_Port;
			_Path  = FrameSystem.m_WebServer_App + "/Common/SystemMenuService.asmx";
		}

		/// <summary>
		/// �޴��޺����
		/// </summary>
		/// <param name="baseModel"></param>
		public void GetComboList(SystemMenuModel systemMenuModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("�޴��޺���� Start");
				_log.Debug("-----------------------------------------");

				// ������ �ν��Ͻ� ����
				SystemMenuServicePloxy.SystemMenuService svc = new SystemMenuServicePloxy.SystemMenuService();
				svc.Url = _WebServiceUrl;
			
				// ����Ʈ �� ����
				SystemMenuServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.SystemMenuServicePloxy.HeaderModel();
				SystemMenuServicePloxy.SystemMenuModel     remoteData   = new AdManagerClient.SystemMenuServicePloxy.SystemMenuModel();

				// ������� ��Ʈ
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;

				// ȣ������ ��Ʈ				
				//remoteData.SearchMenuCode         = systemMenuModel.SearchMenuCode;
				
				// ������ ȣ�� Ÿ�Ӿƿ�����
				svc.Timeout = FrameSystem.m_SystemTimeout;
				// ������ �޼ҵ� ȣ��
				remoteData = svc.GetComboList(remoteHeader, remoteData);

				// ����޴��˻�
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// ��� ��Ʈ
				systemMenuModel.SystemMenuDataSet = remoteData.SystemMenuDataSet.Copy();
				systemMenuModel.ResultCnt   = remoteData.ResultCnt;
				systemMenuModel.ResultCD    = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("�޴��޺���� End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":GetComboList():" + fe.ErrCode + ":" + fe.ResultMsg);
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
		/// �޴����и��
		/// </summary>
		/// <param name="baseModel"></param>
		public void GetUpperMenuList(SystemMenuModel systemMenuModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("�޴����и�� Start");
				_log.Debug("-----------------------------------------");

				// ������ �ν��Ͻ� ����
				SystemMenuServicePloxy.SystemMenuService svc = new SystemMenuServicePloxy.SystemMenuService();
				svc.Url = _WebServiceUrl;
			
				// ����Ʈ �� ����
				SystemMenuServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.SystemMenuServicePloxy.HeaderModel();
				SystemMenuServicePloxy.SystemMenuModel     remoteData   = new AdManagerClient.SystemMenuServicePloxy.SystemMenuModel();

				// ������� ��Ʈ
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;

				// ȣ������ ��Ʈ				
				remoteData.SearchMenuCode         = systemMenuModel.SearchMenuCode;
				
				// ������ ȣ�� Ÿ�Ӿƿ�����
				svc.Timeout = FrameSystem.m_SystemTimeout;
				// ������ �޼ҵ� ȣ��
				remoteData = svc.GetUpperMenuList(remoteHeader, remoteData);

				// ����޴��˻�
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// ��� ��Ʈ
				systemMenuModel.SystemMenuDataSet = remoteData.SystemMenuDataSet.Copy();
				systemMenuModel.ResultCnt   = remoteData.ResultCnt;
				systemMenuModel.ResultCD    = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("�޴����и�� End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":GetUpperMenuList():" + fe.ErrCode + ":" + fe.ResultMsg);
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
		/// �޴����
		/// </summary>
		/// <param name="baseModel"></param>
		public void GetMenuList(SystemMenuModel systemMenuModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("����޴���ȸ Start");
				_log.Debug("-----------------------------------------");

				// ������ �ν��Ͻ� ����
				SystemMenuServicePloxy.SystemMenuService svc = new SystemMenuServicePloxy.SystemMenuService();
				svc.Url = _WebServiceUrl;
			
				// ����Ʈ �� ����
				SystemMenuServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.SystemMenuServicePloxy.HeaderModel();
				SystemMenuServicePloxy.SystemMenuModel     remoteData   = new AdManagerClient.SystemMenuServicePloxy.SystemMenuModel();

				// ������� ��Ʈ
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;

				// ȣ������ ��Ʈ
				remoteData.SearchKey       = systemMenuModel.SearchKey;
				remoteData.UpperMenu         = systemMenuModel.UpperMenu;
				remoteData.MenuCode         = systemMenuModel.MenuCode;
				
				// ������ ȣ�� Ÿ�Ӿƿ�����
				svc.Timeout = FrameSystem.m_SystemTimeout;
				// ������ �޼ҵ� ȣ��
				remoteData = svc.GetMenuList(remoteHeader, remoteData);

				// ����޴��˻�
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// ��� ��Ʈ
				systemMenuModel.SystemMenuDataSet = remoteData.SystemMenuDataSet.Copy();
				systemMenuModel.LastOrder   = remoteData.LastOrder;
				systemMenuModel.ResultCnt   = remoteData.ResultCnt;
				systemMenuModel.ResultCD    = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("����޴���ȸ End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":GetMenuList():" + fe.ErrCode + ":" + fe.ResultMsg);
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
		/// �޴��������� ����
		/// </summary>
		/// <param name="systemMenuModel"></param>
		public void SetUpperMenuUpdate(SystemMenuModel systemMenuModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("�޴������������� Start");
				_log.Debug("-----------------------------------------");

				// ������ �ν��Ͻ� ����
				SystemMenuServicePloxy.SystemMenuService svc = new SystemMenuServicePloxy.SystemMenuService();
				svc.Url = _WebServiceUrl;
			
				// ����Ʈ �� ����
				SystemMenuServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.SystemMenuServicePloxy.HeaderModel();
				SystemMenuServicePloxy.SystemMenuModel     remoteData   = new AdManagerClient.SystemMenuServicePloxy.SystemMenuModel();

				// ������� ��Ʈ
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;

				// ȣ��������Ʈ
				remoteData.MenuName       = systemMenuModel.MenuName;
				remoteData.UseYn       = systemMenuModel.UseYn;
				remoteData.MenuCode     = systemMenuModel.MenuCode;				
											
				// ������ ȣ�� Ÿ�Ӿƿ�����
				svc.Timeout = FrameSystem.m_SystemTimeout;

				// ������ �޼ҵ� ȣ��
				remoteData = svc.SetUpperMenuUpdate(remoteHeader, remoteData);

				// ����޴��˻�
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// ��� ��Ʈ
				systemMenuModel.ResultCnt   = remoteData.ResultCnt;
				systemMenuModel.ResultCD    = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("�޴������������� End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":SetUpperMenuUpdate():" + fe.ErrCode + ":" + fe.ResultMsg);
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
		/// �޴����� ����
		/// </summary>
		/// <param name="systemMenuModel"></param>
		public void SetMenuCodeUpdate(SystemMenuModel systemMenuModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("�޴��������� Start");
				_log.Debug("-----------------------------------------");

				// ������ �ν��Ͻ� ����
				SystemMenuServicePloxy.SystemMenuService svc = new SystemMenuServicePloxy.SystemMenuService();
				svc.Url = _WebServiceUrl;
			
				// ����Ʈ �� ����
				SystemMenuServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.SystemMenuServicePloxy.HeaderModel();
				SystemMenuServicePloxy.SystemMenuModel     remoteData   = new AdManagerClient.SystemMenuServicePloxy.SystemMenuModel();

				// ������� ��Ʈ
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;

				// ȣ��������Ʈ
				remoteData.MenuName       = systemMenuModel.MenuName;
				remoteData.UseYn       = systemMenuModel.UseYn;
				remoteData.MenuCode       = systemMenuModel.MenuCode;
															
				// ������ ȣ�� Ÿ�Ӿƿ�����
				svc.Timeout = FrameSystem.m_SystemTimeout;

				// ������ �޼ҵ� ȣ��
				remoteData = svc.SetMenuCodeUpdate(remoteHeader, remoteData);

				// ����޴��˻�
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// ��� ��Ʈ
				systemMenuModel.ResultCnt   = remoteData.ResultCnt;
				systemMenuModel.ResultCD    = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("�޴��������� End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":SetMenuCodeUpdate():" + fe.ErrCode + ":" + fe.ResultMsg);
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
		/// �޴����� ���
		/// </summary>
		/// <param name="systemMenuModel"></param>
		public void SetUpperMenuCreate(SystemMenuModel systemMenuModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("�޴�������� Start");
				_log.Debug("-----------------------------------------");

				// ������ �ν��Ͻ� ����
				SystemMenuServicePloxy.SystemMenuService svc = new SystemMenuServicePloxy.SystemMenuService();
				svc.Url = _WebServiceUrl;
			
				// ����Ʈ �� ����
				SystemMenuServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.SystemMenuServicePloxy.HeaderModel();
				SystemMenuServicePloxy.SystemMenuModel     remoteData   = new AdManagerClient.SystemMenuServicePloxy.SystemMenuModel();

				// ������� ��Ʈ
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;

				// ȣ��������Ʈ
				remoteData.MenuCode       = systemMenuModel.MenuCode;				
				remoteData.MenuCode_2       = systemMenuModel.MenuCode_2;				
				remoteData.MenuName       = systemMenuModel.MenuName;				
															
				// ������ ȣ�� Ÿ�Ӿƿ�����
				svc.Timeout = FrameSystem.m_SystemTimeout;

				// ������ �޼ҵ� ȣ��
				remoteData = svc.SetUpperMenuCreate(remoteHeader, remoteData);

				// ����޴��˻�
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// ��� ��Ʈ
				systemMenuModel.ResultCnt   = remoteData.ResultCnt;
				systemMenuModel.ResultCD    = remoteData.ResultCD;
				systemMenuModel.MenuCode  = remoteData.MenuCode;

				_log.Debug("-----------------------------------------");
				_log.Debug("�޴�������� End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":SetUpperMenuCreate():" + fe.ErrCode + ":" + fe.ResultMsg);
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
		/// �޴����� ���
		/// </summary>
		/// <param name="systemMenuModel"></param>
		public void SetMenuCodeCreate(SystemMenuModel systemMenuModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("�޴�������� Start");
				_log.Debug("-----------------------------------------");

				// ������ �ν��Ͻ� ����
				SystemMenuServicePloxy.SystemMenuService svc = new SystemMenuServicePloxy.SystemMenuService();
				svc.Url = _WebServiceUrl;
			
				// ����Ʈ �� ����
				SystemMenuServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.SystemMenuServicePloxy.HeaderModel();
				SystemMenuServicePloxy.SystemMenuModel     remoteData   = new AdManagerClient.SystemMenuServicePloxy.SystemMenuModel();

				// ������� ��Ʈ
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;

				// ȣ��������Ʈ
				remoteData.MenuCode       = systemMenuModel.MenuCode;								
				remoteData.MenuCode_2       = systemMenuModel.MenuCode_2;			
				remoteData.MenuName       = systemMenuModel.MenuName;	
				remoteData.UpperMenu       = systemMenuModel.UpperMenu;				
															
				// ������ ȣ�� Ÿ�Ӿƿ�����
				svc.Timeout = FrameSystem.m_SystemTimeout;

				// ������ �޼ҵ� ȣ��
				remoteData = svc.SetMenuCodeCreate(remoteHeader, remoteData);

				// ����޴��˻�
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// ��� ��Ʈ
				systemMenuModel.ResultCnt   = remoteData.ResultCnt;
				systemMenuModel.ResultCD    = remoteData.ResultCD;
				systemMenuModel.MenuCode  = remoteData.MenuCode;

				_log.Debug("-----------------------------------------");
				_log.Debug("�޴�������� End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":SetMenuCodeCreate():" + fe.ErrCode + ":" + fe.ResultMsg);
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
		/// �޴����� ����
		/// </summary>
		/// <param name="systemMenuModel"></param>
		public void SetUpperMenuDelete(SystemMenuModel systemMenuModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("�޴��������� Start");
				_log.Debug("-----------------------------------------");

				// ������ �ν��Ͻ� ����
				SystemMenuServicePloxy.SystemMenuService svc = new SystemMenuServicePloxy.SystemMenuService();
				svc.Url = _WebServiceUrl;
			
				// ����Ʈ �� ����
				SystemMenuServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.SystemMenuServicePloxy.HeaderModel();
				SystemMenuServicePloxy.SystemMenuModel     remoteData   = new AdManagerClient.SystemMenuServicePloxy.SystemMenuModel();

				// ������� ��Ʈ
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;

				// ȣ��������Ʈ				
				remoteData.MenuCode       = systemMenuModel.MenuCode;			
				remoteData.UpperMenu       = systemMenuModel.UpperMenu;			
											
				// ������ ȣ�� Ÿ�Ӿƿ�����
				svc.Timeout = FrameSystem.m_SystemTimeout;

				// ������ �޼ҵ� ȣ��
				remoteData = svc.SetUpperMenuDelete(remoteHeader, remoteData);

				// ����޴��˻�
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// ��� ��Ʈ
				systemMenuModel.ResultCnt   = remoteData.ResultCnt;
				systemMenuModel.ResultCD    = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("�޴��������� End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":SetUpperMenuDelete():" + fe.ErrCode + ":" + fe.ResultMsg);
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
		/// �޴����� ����
		/// </summary>
		/// <param name="systemMenuModel"></param>
		public void SetMenuCodeDelete(SystemMenuModel systemMenuModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("�޴��������� Start");
				_log.Debug("-----------------------------------------");

				// ������ �ν��Ͻ� ����
				SystemMenuServicePloxy.SystemMenuService svc = new SystemMenuServicePloxy.SystemMenuService();
				svc.Url = _WebServiceUrl;
			
				// ����Ʈ �� ����
				SystemMenuServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.SystemMenuServicePloxy.HeaderModel();
				SystemMenuServicePloxy.SystemMenuModel     remoteData   = new AdManagerClient.SystemMenuServicePloxy.SystemMenuModel();

				// ������� ��Ʈ
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;

				// ȣ��������Ʈ				
				remoteData.MenuCode       = systemMenuModel.MenuCode;			
											
				// ������ ȣ�� Ÿ�Ӿƿ�����
				svc.Timeout = FrameSystem.m_SystemTimeout;

				// ������ �޼ҵ� ȣ��
				remoteData = svc.SetMenuCodeDelete(remoteHeader, remoteData);

				// ����޴��˻�
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// ��� ��Ʈ
				systemMenuModel.ResultCnt   = remoteData.ResultCnt;
				systemMenuModel.ResultCD    = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("�޴��������� End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":SetMenuCodeDelete():" + fe.ErrCode + ":" + fe.ResultMsg);
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
		/// �޴� ���� ����
		/// </summary>
		/// <param name="systemMenuModel"></param>
		/// <returns></returns>
		public void SetMenuCodeOrderSet(SystemMenuModel systemMenuModel, int OrderSet)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("�޴� ���� ���� Start");
				_log.Debug("-----------------------------------------");

				//�Էµ������� Validation �˻�
				if(systemMenuModel.MenuCode.Length < 1) 
				{
					throw new FrameException("�Ŵ��� ���õ��� �ʾҽ��ϴ�.");
				}

				// ������ �ν��Ͻ� ����
				SystemMenuServicePloxy.SystemMenuService svc = new SystemMenuServicePloxy.SystemMenuService();
	
				// ������ URL���� ����
				svc.Url = _WebServiceUrl;			

				// ����Ʈ �� ����
				SystemMenuServicePloxy.HeaderModel      remoteHeader = new AdManagerClient.SystemMenuServicePloxy.HeaderModel();
				SystemMenuServicePloxy.SystemMenuModel   remoteData   = new AdManagerClient.SystemMenuServicePloxy.SystemMenuModel();

				// ������� ��Ʈ
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID; 
				remoteHeader.UserLevel     = Header.UserLevel;
				remoteHeader.UserClass     = Header.UserClass;

				// ȣ������ ��Ʈ

				remoteData.MenuCode       =  systemMenuModel.MenuCode;				
				remoteData.MenuOrder   =  systemMenuModel.MenuOrder;
				remoteData.UpperMenu   =  systemMenuModel.UpperMenu;
 
				switch(OrderSet)
				{					
					case ORDER_UP:
						// ������ ȣ�� Ÿ�Ӿƿ�����
						svc.Timeout = FrameSystem.m_SystemTimeout;
						// ������ �޼ҵ� ȣ��
						remoteData = svc.SetMenuCodeOrderUp(remoteHeader, remoteData);						
						break;
					case ORDER_DOWN:
						// ������ ȣ�� Ÿ�Ӿƿ�����
						svc.Timeout = FrameSystem.m_SystemTimeout;
						// ������ �޼ҵ� ȣ��
						remoteData = svc.SetMenuCodeOrderDown(remoteHeader, remoteData);						
						break;					
					default:
						throw new FrameException("�������� ������ ���õ��� �ʾҽ��ϴ�.");
				}


				// ����ڵ�˻�
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// ��� ��Ʈ
				systemMenuModel.ResultCnt      = remoteData.ResultCnt;
				systemMenuModel.ResultCD       = remoteData.ResultCD;		
				systemMenuModel.MenuOrder  = remoteData.MenuOrder;

				_log.Debug("-----------------------------------------");
				_log.Debug("�޴� ù��° ���� ���� End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":SetSchHomeAdOrderFirst():" + fe.ErrCode + ":" + fe.ResultMsg);
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
