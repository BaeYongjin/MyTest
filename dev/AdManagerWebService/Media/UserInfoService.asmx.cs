using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Web;
using System.Web.Services;

using WinFramework.Base;
using AdManagerModel;

namespace AdManagerWebService.Media
{
    /// <summary>
    /// �⺻���ӽ����̽��� �����Ѵ�.
    /// </summary>
    [WebService(Namespace = "http://advmgt.hanafostv.com/AdManagerWebService/")]
    [System.ComponentModel.ToolboxItem(false)]

	/// <summary>
	/// UserInfoService�� ���� ��� �����Դϴ�.
	/// </summary>
	public class UserInfoService : System.Web.Services.WebService
	{

		private UserInfoBiz userInfoBiz = null;

		public UserInfoService()
		{
			//CODEGEN: �� ȣ���� ASP.NET �� ���� �����̳ʿ� �ʿ��մϴ�.
			InitializeComponent();

			userInfoBiz = new UserInfoBiz();
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
		public UserInfoModel GetUsersList(HeaderModel header, UserInfoModel usersModel)
		{
			userInfoBiz.GetUsersList(header, usersModel);
			return usersModel;
		}

		[WebMethod]
		public UserInfoModel SetUserUpdate(HeaderModel header, UserInfoModel usersModel)
		{
			userInfoBiz.SetUserUpdate(header, usersModel);
			return usersModel;
		}

		[WebMethod]
		public UserInfoModel SetUserCreate(HeaderModel header, UserInfoModel usersModel)
		{
			userInfoBiz.SetUserCreate(header, usersModel);
			return usersModel;
		}

		[WebMethod]
		public UserInfoModel SetUserDelete(HeaderModel header, UserInfoModel usersModel)
		{
			userInfoBiz.SetUserDelete(header, usersModel);
			return usersModel;
		}
	}
}
