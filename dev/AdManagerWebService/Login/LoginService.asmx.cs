using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Web;
using System.Web.Services;
using System.Data.SqlClient;

using WinFramework.Base;
using AdManagerModel;

namespace AdManagerWebService.Login
{
    /// <summary>
    /// �⺻���ӽ����̽��� �����Ѵ�.
    /// </summary>
    [WebService(Namespace = "http://advmgt.hanafostv.com/AdManagerWebService/")]
    [System.ComponentModel.ToolboxItem(false)]

	/// <summary>
	/// LoginService�� ���� ��� �����Դϴ�.
	/// </summary>
	public class LoginService : System.Web.Services.WebService
	{
		private LoginBiz loginBiz = null;

		public LoginService()
		{
			//CODEGEN: �� ȣ���� ASP.NET �� ���� �����̳ʿ� �ʿ��մϴ�.
			InitializeComponent();
			loginBiz = new LoginBiz();
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

		// �� ���� ����
		// HelloWorld() ���� ���񽺴� Hello World��� ���ڿ��� ��ȯ�մϴ�.
		// �����Ϸ��� ���� ���� �ּ� ó���� �����ϰ� ������ �� �ش� ������Ʈ�� �����մϴ�.
		// �� �� ���񽺸� �׽�Ʈ�Ϸ��� <F5> Ű�� �����ϴ�.

		[WebMethod]
		public string PasswordEncrypt(string str)
		{
			return loginBiz.PasswordEncrypt(str);
		}

		[WebMethod]
		public string PasswordDecrypt(string str)
		{
			return loginBiz.PasswordDecrypt(str);
		}

		[WebMethod]
		public LoginModel Login(HeaderModel header, LoginModel loginModel)
		{
			loginBiz = new LoginBiz();
			loginBiz.LoginCheck(header, loginModel);
			return loginModel;
		}
	}
}
