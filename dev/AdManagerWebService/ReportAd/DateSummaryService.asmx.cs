using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Web;
using System.Web.Services;

using WinFramework.Base;
using AdManagerModel;

namespace AdManagerWebService.ReportAd
{
    /// <summary>
    /// �⺻���ӽ����̽��� �����Ѵ�.
    /// </summary>
    [WebService(Namespace = "http://advmgt.hanafostv.com/AdManagerWebService/")]
    [System.ComponentModel.ToolboxItem(false)]
	/// <summary>
	/// DateSummaryService�� ���� ��� �����Դϴ�.
	/// </summary>
	public class DateSummaryService : System.Web.Services.WebService
	{
		public DateSummaryService()
		{
			//CODEGEN: �� ȣ���� ASP.NET �� ���� �����̳ʿ� �ʿ��մϴ�.
			InitializeComponent();
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
		public  DateSummaryModel GetDateGenre(HeaderModel header, DateSummaryModel model)
		{
			new DateSummaryBiz().GetDateGenre(header, model);
			return model;
		}

		[WebMethod]
		public  DateSummaryModel GetDateChannel(HeaderModel header, DateSummaryModel model)
		{
			new DateSummaryBiz().GetDateChannel(header, model);
			return model;
		}

		[WebMethod]
		public  DateSummaryModel GetDateGenreByJava(HeaderModel header, DateSummaryModel model)
		{
			new DateSummaryBiz().GetDateGenreByJava(header, model);
			return model;
		}

		[WebMethod]
		public  DateSummaryModel GetDateChannelByJava(HeaderModel header, DateSummaryModel model)
		{
			new DateSummaryBiz().GetDateChannelByJava(header, model);
			return model;
		}

		[WebMethod]
		public  DateSummaryModel GetChannelArea(HeaderModel header, DateSummaryModel model)
		{
			new DateSummaryBiz().GetChannelArea(header, model);
			return model;
		}

		[WebMethod]
		public  DateSummaryModel GetChannelAreaByJava(HeaderModel header, DateSummaryModel model)
		{
			new DateSummaryBiz().GetChannelAreaByJava(header, model);
			return model;
		}
	}
}
