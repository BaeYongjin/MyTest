using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Web;
using System.Web.Services;

using WinFramework.Base;
using AdManagerModel;

namespace AdManagerWebService.ReportSummaryAd
{
    /// <summary>
    /// �⺻���ӽ����̽��� �����Ѵ�.
    /// </summary>
    [WebService(Namespace = "http://advmgt.hanafostv.com/AdManagerWebService/")]
    [System.ComponentModel.ToolboxItem(false)]
	/// <summary>
	/// DateAdTypeSummaryRptService�� ���� ��� �����Դϴ�.
	/// </summary>
	public class DateAdTypeSummaryRptService : System.Web.Services.WebService
	{
		public DateAdTypeSummaryRptService()
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
		public  DateAdTypeSummaryRptModel GetDateAdTypeSummaryRpt(HeaderModel header, DateAdTypeSummaryRptModel model)
		{
			new DateAdTypeSummaryRptBiz().GetDateAdTypeSummaryRpt(header, model);
			return model;
		}

		[WebMethod]
		public  DateAdTypeSummaryRptModel GetWeeklyAdTypeSummaryRpt(HeaderModel header, DateAdTypeSummaryRptModel model)
		{
			new DateAdTypeSummaryRptBiz().GetWeeklyAdTypeSummaryRpt(header, model);
			return model;
		}

		[WebMethod]
		public  DateAdTypeSummaryRptModel GetMonthlyAdTypeSummaryRpt(HeaderModel header, DateAdTypeSummaryRptModel model)
		{
			new DateAdTypeSummaryRptBiz().GetMonthlyAdTypeSummaryRpt(header, model);
			return model;
		}
	}
}
