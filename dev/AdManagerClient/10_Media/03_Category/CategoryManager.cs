// ===============================================================================
// UserUpdate Manager  for Charites Project
//
// UserUpdateManager.cs
//
// ��������� ���� ���񽺸� ȣ���մϴ�. 
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
	/// ��������� �����񽺸� ȣ���մϴ�. 
	/// </summary>
	public class CategoryManager : BaseManager
	{
		/// <summary>
		/// ������
		/// </summary>
		/// <param name="systemModel"></param>
		/// <param name="commonModel"></param>

		public CategoryManager(SystemModel systemModel, CommonModel commonModel) : base(systemModel, commonModel)
		{
			_log = FrameSystem.oLog;
			_module = "USERINFO";
			_Host  = FrameSystem.m_WebServer_Host;
			_Port  = FrameSystem.m_WebServer_Port;
			_Path  = FrameSystem.m_WebServer_App + "/Media/CategoryService.asmx";
		}

		/// <summary>
		/// �����������ȸ
		/// </summary>
		/// <param name="categoryModel"></param>
		public void GetCategoryList(CategoryModel categoryModel)
		{

			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("����ڸ����ȸ Start");
				_log.Debug("-----------------------------------------");
				
				// ������ �ν��Ͻ� ����
				CategoryServicePloxy.CategoryService svc = new CategoryServicePloxy.CategoryService();
				svc.Url = _WebServiceUrl;
			
				// ����Ʈ �� ����
				CategoryServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.CategoryServicePloxy.HeaderModel();
				CategoryServicePloxy.CategoryModel remoteData   = new AdManagerClient.CategoryServicePloxy.CategoryModel();

				// ������� ��Ʈ
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;
				remoteHeader.UserLevel     = Header.UserLevel;

				// ȣ������ ��Ʈ
				remoteData.SearchKey       = categoryModel.SearchKey;
				remoteData.SearchCategoryLevel = categoryModel.SearchCategoryLevel;
                
                // ������ ȣ�� Ÿ�Ӿƿ�����
                svc.Timeout = FrameSystem.m_SystemTimeout;
				
                // ������ �޼ҵ� ȣ��
				remoteData = svc.GetCategoryList(remoteHeader, remoteData);

				// ����ڵ�˻�
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// ��� ��Ʈ
				categoryModel.UserDataSet = remoteData.UserDataSet.Copy();
				categoryModel.ResultCnt   = remoteData.ResultCnt;
				categoryModel.ResultCD    = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("����ڸ����ȸ End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":GetUserList():" + fe.ErrCode + ":" + fe.ResultMsg);
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
		/// Service ȣ���� ���� �޼ҵ�
		/// </summary>
		public bool GetUserDetail(BaseModel baseModel)
		{
			
			_log.Debug("-----------------------------------------");
			_log.Debug( this.ToString() + " Start");
			_log.Debug("-----------------------------------------");

			_log.Debug("-----------------------------------------");
			_log.Debug( this.ToString() + " End");
			_log.Debug("-----------------------------------------");

			return true;
		}

		/// <summary>
		/// ��������� ����
		/// </summary>
		/// <param name="categoryModel"></param>
		public void SetCategoryUpdate(CategoryModel categoryModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("ī�װ����� Start");
				_log.Debug("-----------------------------------------");

				if(categoryModel.CategoryName.Length > 20) 
				{
					throw new FrameException("ī�װ���Ī�� 20Bytes�� �ʰ��� �� �����ϴ�.");
				}

				// ������ �ν��Ͻ� ����
				CategoryServicePloxy.CategoryService svc = new CategoryServicePloxy.CategoryService();
				svc.Url = _WebServiceUrl;
			
				// ����Ʈ �� ����
				CategoryServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.CategoryServicePloxy.HeaderModel();
				CategoryServicePloxy.CategoryModel remoteData   = new AdManagerClient.CategoryServicePloxy.CategoryModel();

				// ������� ��Ʈ
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;

				remoteData.MediaCode		=	categoryModel.MediaCode;
				remoteData.CategoryCode		=	categoryModel.CategoryCode;
				remoteData.CategoryName		=	categoryModel.CategoryName;
				remoteData.Flag				=	categoryModel.Flag;
				remoteData.SortNo			=	categoryModel.SortNo;
				remoteData.CssFlag			=	categoryModel.CssFlag;
				remoteData.InventoryYn		=	categoryModel.InventoryYn;
				remoteData.InventoryRate	=	categoryModel.InventoryRate;
								
                // ������ ȣ�� Ÿ�Ӿƿ�����
                svc.Timeout = FrameSystem.m_SystemTimeout;

				// ������ �޼ҵ� ȣ��
				remoteData = svc.SetCategoryUpdate(remoteHeader, remoteData);

				// ����ڵ�˻�
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// ��� ��Ʈ
				categoryModel.ResultCnt   = remoteData.ResultCnt;
				categoryModel.ResultCD    = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("ī�װ����� End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":SetUserUpdate():" + fe.ErrCode + ":" + fe.ResultMsg);
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
		/// ������߰�
		/// </summary>
		/// <param name="categoryModel"></param>
		/// <returns></returns>
		public void SetCategoryAdd(CategoryModel categoryModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("������߰� Start");
				_log.Debug("-----------------------------------------");

				/*if(categoryModel.MediaCode.Trim().Length < 1) 
				{
					throw new FrameException("��ü�ڵ尡 �������� �ʽ��ϴ�.");
				}
				if(categoryModel.MediaCode.Trim().Length > 10) 
				{
					throw new FrameException("��ü�ڵ�� 10Bytes�� �ʰ��� �� �����ϴ�.");
				}*/
				if(categoryModel.CategoryName.Length > 20) 
				{
					throw new FrameException("ī�װ���Ī�� 20Bytes�� �ʰ��� �� �����ϴ�.");
				}
				/*if(categoryModel.Charger.Length > 10) 
				{
					throw new FrameException("����ڸ��� 10Bytes�� �ʰ��� �� �����ϴ�.");
				}
				if(categoryModel.Tell.Length > 15) 
				{
					throw new FrameException("��ȭ��ȣ�� ���̴� 15Bytes�� �ʰ��� �� �����ϴ�");
				}				
				if(categoryModel.Email.Length > 40) 
				{
					throw new FrameException("Email�� 40Bytes�� �ʰ��� �� �����ϴ�.");
				}*/


				// ������ �ν��Ͻ� ����
				CategoryServicePloxy.CategoryService svc = new CategoryServicePloxy.CategoryService();
				svc.Url = _WebServiceUrl;
				
				// ����Ʈ �� ����
				CategoryServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.CategoryServicePloxy.HeaderModel();
				CategoryServicePloxy.CategoryModel remoteData   = new AdManagerClient.CategoryServicePloxy.CategoryModel();

				// ������� ��Ʈ
				remoteHeader.ClientKey     = Header.ClientKey;
				remoteHeader.UserID        = Header.UserID;

				// ȣ��������Ʈ
				//remoteData.CheckYn				= categoryModel.CheckYn;
				remoteData.MediaCode       = categoryModel.MediaCode;
				remoteData.CategoryCode       = categoryModel.CategoryCode;
				remoteData.CategoryName     = categoryModel.CategoryName;				
				remoteData.ModDt     = categoryModel.ModDt;
				
                // ������ ȣ�� Ÿ�Ӿƿ�����
                svc.Timeout = FrameSystem.m_SystemTimeout;
					
				// ������ �޼ҵ� ȣ��
				remoteData = svc.SetCategoryCreate(remoteHeader, remoteData);

				// ����ڵ�˻�
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// ��� ��Ʈ
				categoryModel.ResultCnt   = remoteData.ResultCnt;
				categoryModel.ResultCD    = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("������߰� End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				categoryModel.ResultCD    = "3101";
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":SetUserCreate():" + fe.ErrCode + ":" + fe.ResultMsg);
				_log.Warning("-----------------------------------------");
				throw fe;
			}
			catch(Exception e)
			{
				categoryModel.ResultCD    = "3101";
				_log.Error("-----------------------------------------");
				_log.Exception(e);
				_log.Error("-----------------------------------------");
				throw new FrameException(e.Message);
			}
		}

		
		/// <summary>
		/// ����� ����
		/// </summary>
		/// <param name="baseModel"></param>
		public void SetCategoryDelete(CategoryModel categoryModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug("����ڻ��� Start");
				_log.Debug("-----------------------------------------");

				// ������ �ν��Ͻ� ����
				CategoryServicePloxy.CategoryService svc = new CategoryServicePloxy.CategoryService();
				svc.Url = _WebServiceUrl;
				
				// ����Ʈ �� ����
				CategoryServicePloxy.HeaderModel   remoteHeader = new AdManagerClient.CategoryServicePloxy.HeaderModel();
				CategoryServicePloxy.CategoryModel remoteData   = new AdManagerClient.CategoryServicePloxy.CategoryModel();

				// ������� ��Ʈ
				remoteHeader.ClientKey  = Header.ClientKey;
				remoteHeader.UserID     = Header.UserID;

				// ȣ��������Ʈ
				remoteData.MediaCode       = categoryModel.MediaCode;
				remoteData.CategoryCode       = categoryModel.CategoryCode;
					
                // ������ ȣ�� Ÿ�Ӿƿ�����
                svc.Timeout = FrameSystem.m_SystemTimeout;

				// ������ �޼ҵ� ȣ��
				remoteData = svc.SetCategoryDelete(remoteHeader, remoteData);

				// ����ڵ�˻�
				if(!remoteData.ResultCD.Equals("0000"))
				{
					throw new FrameException(remoteData.ResultDesc, _module, remoteData.ResultCD);
				}

				// ��� ��Ʈ
				categoryModel.ResultCnt   = remoteData.ResultCnt;
				categoryModel.ResultCD    = remoteData.ResultCD;

				_log.Debug("-----------------------------------------");
				_log.Debug("����ڻ��� End");
				_log.Debug("-----------------------------------------");
			}
			catch(FrameException fe)
			{
				_log.Warning("-----------------------------------------");
				_log.Warning( this.ToString() + ":SetCategoryDelete():" + fe.ErrCode + ":" + fe.ResultMsg);
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