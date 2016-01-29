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
	/// AdFileService�� ���� ��� �����Դϴ�.
	/// </summary>
	public class AdFileService : System.Web.Services.WebService
	{
		private AdFileBiz adFileBiz = null;
		public AdFileService()
		{
			//CODEGEN: �� ȣ���� ASP.NET �� ���� �����̳ʿ� �ʿ��մϴ�.
			InitializeComponent();
			adFileBiz = new AdFileBiz();
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
		public AdFileModel GetAdFileList(HeaderModel header, AdFileModel adFileModel)
		{
			adFileBiz.GetAdFileList(header, adFileModel);
			return adFileModel;
		}

		[WebMethod]
		public AdFileModel GetAdFileSearch(HeaderModel header, AdFileModel adFileModel)
		{
			adFileBiz.GetAdFileSearch(header, adFileModel);
			return adFileModel;
		}

		[WebMethod]
		public AdFileModel GetPublishHistory(HeaderModel header, AdFileModel adFileModel)
		{
			adFileBiz.GetPublishHistory(header, adFileModel);
			return adFileModel;
		}

		[WebMethod]
		public AdFileModel SetAdFileUpdate(HeaderModel header, AdFileModel adFileModel)
		{
			adFileBiz.SetAdFileUpdate(header, adFileModel);
			return adFileModel;
		}
		
		[WebMethod]
		public AdFileModel SetFileUpdate(HeaderModel header, AdFileModel adFileModel)
		{
			adFileBiz.SetFileUpdate(header, adFileModel);
			return adFileModel;
		}

		[WebMethod]
		public AdFileModel GetFtpConfig(HeaderModel header, AdFileModel adFileModel)
		{
			adFileBiz.GetFtpConfig(header, adFileModel);
			return adFileModel;
		}

		[WebMethod]
		public AdFileModel GetFileRepHistory(HeaderModel header, AdFileModel adFileModel)
		{
			adFileBiz.GetFileRepHistory(header, adFileModel);
			return adFileModel;
		}
	}
}