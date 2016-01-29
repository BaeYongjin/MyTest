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
	/// AdFileWideService�� ���� ��� �����Դϴ�.
	/// </summary>
	public class AdFileWideService : System.Web.Services.WebService
	{
		private AdFileWideBiz adFileWideBiz = null;
		public AdFileWideService()
		{
			//CODEGEN: �� ȣ���� ASP.NET �� ���� �����̳ʿ� �ʿ��մϴ�.
			InitializeComponent();
			adFileWideBiz = new AdFileWideBiz();
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
		public AdFileWideModel GetFileCount(HeaderModel header, AdFileWideModel adFileWideModel)
		{
			adFileWideBiz.GetFileCount(header, adFileWideModel);
			return adFileWideModel;
		}

		[WebMethod]
		public AdFileWideModel GetAdFileWideList(HeaderModel header, AdFileWideModel adFileWideModel)
		{
			adFileWideBiz.GetAdFileWideList(header, adFileWideModel);
			return adFileWideModel;
		}

		[WebMethod]
		public AdFileWideModel GetAdFileSchedule(HeaderModel header, AdFileWideModel adFileWideModel)
		{
			adFileWideBiz.GetAdFileSchedule(header, adFileWideModel);
			return adFileWideModel;
		}

		[WebMethod]
		public AdFileWideModel SetAdFileChkRequest(HeaderModel header, AdFileWideModel adFileWideModel)
		{
			adFileWideBiz.SetAdFileChkRequest(header, adFileWideModel);
			return adFileWideModel;
		}

		[WebMethod]
		public AdFileWideModel SetAdFileChkRequestCancel(HeaderModel header, AdFileWideModel adFileWideModel)
		{
			adFileWideBiz.SetAdFileChkRequestCancel(header, adFileWideModel);
			return adFileWideModel;
		}

		[WebMethod]
		public AdFileWideModel SetAdFileChkComplete(HeaderModel header, AdFileWideModel adFileWideModel)
		{
			adFileWideBiz.SetAdFileChkComplete(header, adFileWideModel);
			return adFileWideModel;
		}

		[WebMethod]
		public AdFileWideModel SetCmsRequestBegin(HeaderModel header, AdFileWideModel adFileWideModel)
		{
			adFileWideBiz.SetCmsRequestBegin(header, adFileWideModel);
			return adFileWideModel;
		}

		[WebMethod]
		public AdFileWideModel SetAdFileChkCompleteCancel(HeaderModel header, AdFileWideModel adFileWideModel)
		{
			adFileWideBiz.SetAdFileChkCompleteCancel(header, adFileWideModel);
			return adFileWideModel;
		}

		[WebMethod]
		public AdFileWideModel SetAdFileCDNSync(HeaderModel header, AdFileWideModel adFileWideModel)
		{
			adFileWideBiz.SetAdFileCDNSync(header, adFileWideModel);
			return adFileWideModel;
		}

		[WebMethod]
		public AdFileWideModel SetAdFileCDNSyncCancel(HeaderModel header, AdFileWideModel adFileWideModel)
		{
			adFileWideBiz.SetAdFileCDNSyncCancel(header, adFileWideModel);
			return adFileWideModel;
		}

		[WebMethod]
		public AdFileWideModel SetAdFileCDNPublish(HeaderModel header, AdFileWideModel adFileWideModel)
		{
			adFileWideBiz.SetAdFileCDNPublish(header, adFileWideModel);
			return adFileWideModel;
		}

		[WebMethod]
		public AdFileWideModel SetAdFileCDNPublishCancel(HeaderModel header, AdFileWideModel adFileWideModel)
		{
			adFileWideBiz.SetAdFileCDNPublishCancel(header, adFileWideModel);
			return adFileWideModel;
		}


		[WebMethod]
		public AdFileWideModel SetAdFileSTBDelete(HeaderModel header, AdFileWideModel adFileWideModel)
		{
			adFileWideBiz.SetAdFileSTBDelete(header, adFileWideModel);
			return adFileWideModel;
		}

		[WebMethod]
		public AdFileWideModel SetAdFileSTBDeleteCancel(HeaderModel header, AdFileWideModel adFileWideModel)
		{
			adFileWideBiz.SetAdFileSTBDeleteCancel(header, adFileWideModel);
			return adFileWideModel;
		}

		[WebMethod]
		public AdFileWideModel SetAdFileChange(HeaderModel header, AdFileWideModel adFileWideModel)
		{
			adFileWideBiz.SetAdFileChange(header, adFileWideModel);
			return adFileWideModel;
		}

	}
}