using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Web;
using System.Web.Services;

using WinFramework.Base;
using AdManagerModel;


namespace AdManagerWebService.Contract
{

	/// <summary>
	/// �⺻���ӽ����̽��� �����Ѵ�.
	/// </summary>
	[WebService(Namespace="http://advmgt.hanafostv.com/AdManagerWebService/")]
    [System.ComponentModel.ToolboxItem(false)]

	/// <summary>
	/// AdvertiserService�� ���� ��� �����Դϴ�.
	/// </summary>
	public class AdvertiserService : System.Web.Services.WebService
	{
		private AdvertiserBiz advertiserBiz = null;
		public AdvertiserService()
		{
			//CODEGEN: �� ȣ���� ASP.NET �� ���� �����̳ʿ� �ʿ��մϴ�.
			InitializeComponent();
			advertiserBiz = new AdvertiserBiz();
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
		public AdvertiserModel GetAdvertiserList(HeaderModel header, AdvertiserModel advertiserModel)
		{
			advertiserBiz.GetAdvertiserList(header, advertiserModel);
			return advertiserModel;
		}

		[WebMethod]
		public AdvertiserModel SetAdvertiserUpdate(HeaderModel header, AdvertiserModel advertiserModel)
		{
			advertiserBiz.SetAdvertiserUpdate(header, advertiserModel);
			return advertiserModel;
		}

		[WebMethod]
		public AdvertiserModel SetAdvertiserCreate(HeaderModel header, AdvertiserModel advertiserModel)
		{
			advertiserBiz.SetAdvertiserCreate(header, advertiserModel);
			return advertiserModel;
		}

		[WebMethod]
		public AdvertiserModel SetAdvertiserDelete(HeaderModel header, AdvertiserModel advertiserModel)
		{
			advertiserBiz.SetAdvertiserDelete(header, advertiserModel);
			return advertiserModel;
		}

        [WebMethod]
		public AdvertiserModel GetJobClassList(HeaderModel header, AdvertiserModel advertiserModel)
		{
			advertiserBiz.GetJobClassList(header, advertiserModel);
			return advertiserModel;
		}
	}
}