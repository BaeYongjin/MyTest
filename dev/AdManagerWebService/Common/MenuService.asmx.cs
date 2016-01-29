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
	/// MenuService�� ���� ��� �����Դϴ�.
	/// </summary>
	public class MenuService : System.Web.Services.WebService
	{

		private MenuBiz menuBiz = null;

		public MenuService()
		{
			//CODEGEN: �� ȣ���� ASP.NET �� ���� �����̳ʿ� �ʿ��մϴ�.
			InitializeComponent();

			menuBiz = new MenuBiz();
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
        public MenuModel GetUserMenuList(HeaderModel header, MenuModel menuModel)
        {
            menuBiz.GetUserMenuList(header, menuModel);
            return menuModel;
        }
        [WebMethod]
        public MenuModel GetUserClassList(HeaderModel header, MenuModel menuModel)
        {
            menuBiz.GetUserClassList(header, menuModel);
            return menuModel;
        }
        [WebMethod]
        public MenuModel GetMenuPowerList(HeaderModel header, MenuModel menuModel)
        {
            menuBiz.GetMenuPowerList(header, menuModel);
            return menuModel;
        }
        [WebMethod]
        public MenuModel SetUserClassCreate(HeaderModel header, MenuModel menuModel)
        {
            menuBiz.SetUserClassCreate(header, menuModel);
            return menuModel;
        }
        [WebMethod]
        public MenuModel SetUserClassUpdate(HeaderModel header, MenuModel menuModel)
        {
            menuBiz.SetUserClassUpdate(header, menuModel);
            return menuModel;
        }

        
        [WebMethod]
        public MenuModel SetMenuPowerUpdate(HeaderModel header, MenuModel menuModel)
        {
            menuBiz.SetMenuPowerUpdate(header, menuModel);
            return menuModel;
        }

        

	}
}
