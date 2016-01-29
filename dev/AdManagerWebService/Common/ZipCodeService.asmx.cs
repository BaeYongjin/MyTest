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
	public class ZipCodeService : System.Web.Services.WebService
	{
		private ZipCodeBiz biz = null;

		public ZipCodeService()
		{
			//CODEGEN: �� ȣ���� ASP.NET �� ���� �����̳ʿ� �ʿ��մϴ�.
			InitializeComponent();
			biz = new ZipCodeBiz();
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
		public ZipCodeModel GetZipList(HeaderModel header, ZipCodeModel data)
		{
			biz.GetAddressList(header,data);
			return data;
		}


		[WebMethod]
		public ZipCodeModel GetPreZipList(HeaderModel header, ZipCodeModel data)
		{
			biz.GetPreZipList(header,data);
			return data;
		}
		
		[WebMethod]
		public ZipCodeModel GetIncludeZipList(HeaderModel header, ZipCodeModel data)
		{
			biz.GetIncludeZipList(header,data);
			return data;
		}

	}
}
