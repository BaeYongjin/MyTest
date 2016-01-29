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
    [WebService(Namespace = "http://advmgt.hanafostv.com/AdManagerWebService/")]
    [System.ComponentModel.ToolboxItem(false)]

	/// <summary>
	/// MediaAgencyService�� ���� ��� �����Դϴ�.
	/// </summary>
	public class MediaAgencyService : System.Web.Services.WebService
	{
		private MediaAgencyBiz mediaAgencyBiz = null;
		public MediaAgencyService()
		{
			//CODEGEN: �� ȣ���� ASP.NET �� ���� �����̳ʿ� �ʿ��մϴ�.
			InitializeComponent();
			mediaAgencyBiz = new MediaAgencyBiz();
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
		public MediaAgencyModel GetMediaAgencyList(HeaderModel header, MediaAgencyModel mediaAgencyModel)
		{
			mediaAgencyBiz.GetMediaAgencyList(header, mediaAgencyModel);
			return mediaAgencyModel;
		}

		[WebMethod]
		public MediaAgencyModel GetMediaAgencyPop(HeaderModel header, MediaAgencyModel mediaAgencyModel)
		{
			mediaAgencyBiz.GetMediaAgencyPop(header, mediaAgencyModel);
			return mediaAgencyModel;
		}

		[WebMethod]
		public MediaAgencyModel SetMediaAgencyUpdate(HeaderModel header, MediaAgencyModel mediaAgencyModel)
		{
			mediaAgencyBiz.SetMediaAgencyUpdate(header, mediaAgencyModel);
			return mediaAgencyModel;
		}

		[WebMethod]
		public MediaAgencyModel SetMediaAgencyCreate(HeaderModel header, MediaAgencyModel mediaAgencyModel)
		{
			mediaAgencyBiz.SetMediaAgencyCreate(header, mediaAgencyModel);
			return mediaAgencyModel;
		}

		[WebMethod]
		public MediaAgencyModel SetMediaAgencyDelete(HeaderModel header, MediaAgencyModel mediaAgencyModel)
		{
			mediaAgencyBiz.SetMediaAgencyDelete(header, mediaAgencyModel);
			return mediaAgencyModel;
		}
	}
}