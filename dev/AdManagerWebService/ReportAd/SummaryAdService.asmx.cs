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
	/// SummaryAdService�� ���� ��� �����Դϴ�.
	/// </summary>
	public class SummaryAdService : System.Web.Services.WebService
	{
		public SummaryAdService()
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
		public  SummaryAdModel GetContractItemList(HeaderModel header, SummaryAdModel model)
		{
			new SummaryAdBiz().GetContractItemList(header, model);
			return model;
		}

		[WebMethod]
		public  SummaryAdModel GetSummaryAdTotality(HeaderModel header, SummaryAdModel model)
		{
			new SummaryAdBiz().GetSummaryAdTotality(header, model);
			return model;
		}

		[WebMethod]
		public  SummaryAdModel GetSummaryAdDaily(HeaderModel header, SummaryAdModel model)
		{
			new SummaryAdBiz().GetSummaryAdDaily(header, model);
			return model;
		}

		[WebMethod]
		public  SummaryAdModel GetSummaryAdWeekly(HeaderModel header, SummaryAdModel model)
		{
			new SummaryAdBiz().GetSummaryAdWeekly(header, model);
			return model;
		}

		[WebMethod]
		public  SummaryAdModel GetSummaryAdMonthly(HeaderModel header, SummaryAdModel model)
		{
			new SummaryAdBiz().GetSummaryAdMonthly(header, model);
			return model;
		}

		[WebMethod]
		public  SummaryAdModel GetDateAccountHouseHold(HeaderModel header, SummaryAdModel model)
		{
			new SummaryAdBiz().GetDateAccountHouseHold(header, model);
			return model;
		}

	}
}
