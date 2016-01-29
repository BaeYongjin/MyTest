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
	/// TimeSummaryService�� ���� ��� �����Դϴ�.
	/// </summary>
	public class TimeSummaryService : System.Web.Services.WebService
	{
		public TimeSummaryService()
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
		public  TimeSummaryModel GetAreaTime(HeaderModel header, TimeSummaryModel model)
		{
			new TimeSummaryBiz().GetAreaTime(header, model);
			return model;
		}

		[WebMethod]
		public  TimeSummaryModel GetDateTime(HeaderModel header, TimeSummaryModel model)
		{
			new TimeSummaryBiz().GetDateTime(header, model);
			return model;
		}

		[WebMethod]
		public  TimeSummaryModel GetGenreTime(HeaderModel header, TimeSummaryModel model)
		{
			new TimeSummaryBiz().GetGenreTime(header, model);
			return model;
		}

		[WebMethod]
		public  TimeSummaryModel GetChannelTime(HeaderModel header, TimeSummaryModel model)
		{
			new TimeSummaryBiz().GetChannelTime(header, model);
			return model;
		}

		[WebMethod]
		public  TimeSummaryModel GetAreaTimeByJava(HeaderModel header, TimeSummaryModel model)
		{
			new TimeSummaryBiz().GetAreaTimeByJava(header, model);
			return model;
		}

		[WebMethod]
		public  TimeSummaryModel GetDateTimeByJava(HeaderModel header, TimeSummaryModel model)
		{
			new TimeSummaryBiz().GetDateTimeByJava(header, model);
			return model;
		}

		[WebMethod]
		public  TimeSummaryModel GetGenreTimeByJava(HeaderModel header, TimeSummaryModel model)
		{
			new TimeSummaryBiz().GetGenreTimeByJava(header, model);
			return model;
		}

		[WebMethod]
		public  TimeSummaryModel GetChannelTimeByJava(HeaderModel header, TimeSummaryModel model)
		{
			new TimeSummaryBiz().GetChannelTimeByJava(header, model);
			return model;
		}

	}
}
