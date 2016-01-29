using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Web;
using System.Web.Services;

using WinFramework.Base;
using AdManagerModel;

namespace AdManagerWebService.Common
{

	/// <summary>
	/// �⺻���ӽ����̽��� �����Ѵ�.
	/// </summary>
	[WebService(Namespace="http://advmgt.hanafostv.com/AdManagerWebService/")]
    [System.ComponentModel.ToolboxItem(false)]

	/// <summary>
	/// SystemMenuService�� ���� ��� �����Դϴ�.
	/// </summary>
	public class SystemMenuService : System.Web.Services.WebService
	{
		private SystemMenuBiz systemMenuBiz = null;

		public SystemMenuService()
		{
			//CODEGEN: �� ȣ���� ASP.NET �� ���� �����̳ʿ� �ʿ��մϴ�.
			InitializeComponent();

			systemMenuBiz = new SystemMenuBiz();
		}

		#region ���� ��� �����̳ʿ��� ������ �ڵ�
		
		//�� ���� �����̳ʿ� �ʿ��մϴ�. 
		private IContainer components = null;
				
		/// <summary>
		/// �����̳� ������ �ʿ��� �޼����Դϴ�.
		/// �� �޼����� ������ �ڵ� ������� �������� ���ʽÿ�.
		/// </summary>
		private void InitializeComponent()
		{
		}

		/// <summary>
		/// ��� ���� ��� ���ҽ��� �����մϴ�.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if(disposing && components != null)
			{
				components.Dispose();
			}
			base.Dispose(disposing);		
		}
		
		#endregion

		[WebMethod]
		public SystemMenuModel GetComboList(HeaderModel header, SystemMenuModel systemMenuModel)
		{
			systemMenuBiz.GetComboList(header, systemMenuModel);
			return systemMenuModel;
		}

		[WebMethod]
		public SystemMenuModel GetUpperMenuList(HeaderModel header, SystemMenuModel systemMenuModel)
		{
			systemMenuBiz.GetUpperMenuList(header, systemMenuModel);
			return systemMenuModel;
		}

		[WebMethod]
		public SystemMenuModel GetMenuList(HeaderModel header, SystemMenuModel systemMenuModel)
		{
			systemMenuBiz.GetMenuList(header, systemMenuModel);
			return systemMenuModel;
		}

		[WebMethod]
		public SystemMenuModel SetUpperMenuUpdate(HeaderModel header, SystemMenuModel systemMenuModel)
		{
			systemMenuBiz.SetUpperMenuUpdate(header, systemMenuModel);
			return systemMenuModel;
		}

		[WebMethod]
		public SystemMenuModel SetMenuCodeUpdate(HeaderModel header, SystemMenuModel systemMenuModel)
		{
			systemMenuBiz.SetMenuCodeUpdate(header, systemMenuModel);
			return systemMenuModel;
		}

		[WebMethod]
		public SystemMenuModel SetUpperMenuCreate(HeaderModel header, SystemMenuModel systemMenuModel)
		{
			systemMenuBiz.SetUpperMenuCreate(header, systemMenuModel);
			return systemMenuModel;
		}

		[WebMethod]
		public SystemMenuModel SetMenuCodeCreate(HeaderModel header, SystemMenuModel systemMenuModel)
		{
			systemMenuBiz.SetMenuCodeCreate(header, systemMenuModel);
			return systemMenuModel;
		}

		[WebMethod]
		public SystemMenuModel SetUpperMenuDelete(HeaderModel header, SystemMenuModel systemMenuModel)
		{
			systemMenuBiz.SetUpperMenuDelete(header, systemMenuModel);
			return systemMenuModel;
		}

		[WebMethod]
		public SystemMenuModel SetMenuCodeDelete(HeaderModel header, SystemMenuModel systemMenuModel)
		{
			systemMenuBiz.SetMenuCodeDelete(header, systemMenuModel);
			return systemMenuModel;
		}

		[WebMethod]
		public SystemMenuModel SetMenuCodeOrderUp(HeaderModel header, SystemMenuModel systemMenuModel)
		{
			systemMenuBiz.SetMenuCodeOrderUp(header, systemMenuModel);
			return systemMenuModel;
		}

		[WebMethod]
		public SystemMenuModel SetMenuCodeOrderDown(HeaderModel header, SystemMenuModel systemMenuModel)
		{
			systemMenuBiz.SetMenuCodeOrderDown(header, systemMenuModel);
			return systemMenuModel;
		}

	}
}
