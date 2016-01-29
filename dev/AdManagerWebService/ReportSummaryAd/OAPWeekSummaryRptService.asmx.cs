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
	/// OAPWeekSummaryRptService�� ���� ��� �����Դϴ�.
	/// </summary>
	public class OAPWeekSummaryRptService : System.Web.Services.WebService
	{
		public OAPWeekSummaryRptService()
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
		public  OAPWeekSummaryRptModel GetOAPWeekHomeAd(HeaderModel header, OAPWeekSummaryRptModel model)
		{
			new OAPWeekSummaryRptBiz().GetOAPWeekHomeAd(header, model);
			return model;
		}

		[WebMethod]
		public  OAPWeekSummaryRptModel GetOAPWeekChannelJump(HeaderModel header, OAPWeekSummaryRptModel model)
		{
			new OAPWeekSummaryRptBiz().GetOAPWeekChannelJump(header, model);
			return model;
		}

        [WebMethod]
        public  DataSet mGetHomeCmReport(string beginDay,string endDay , string mediaRep)
        {
            return new OAPWeekSummaryRptBiz().GetHomeCmReport( beginDay,endDay, mediaRep);
        }
	}
}
