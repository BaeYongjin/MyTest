using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Web;
using System.Web.Services;

namespace AdManagerWebService.ReportSummaryAd
{
    /// <summary>
    /// �⺻���ӽ����̽��� �����Ѵ�.
    /// </summary>
    [WebService(Namespace = "http://advmgt.hanafostv.com/AdManagerWebService/")]
    [System.ComponentModel.ToolboxItem(false)]
	/// <summary>
	/// RptSummaryAdMonthlyService�� ���� ��� �����Դϴ�.
	/// </summary>
	public class RptSummaryAdMonthlyService : System.Web.Services.WebService
	{
		public RptSummaryAdMonthlyService()
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

		// �� ���� ����
		// HelloWorld() ���� ���񽺴� Hello World��� ���ڿ��� ��ȯ�մϴ�.
		// �����Ϸ��� ���� ���� �ּ� ó���� �����ϰ� ������ �� �ش� ������Ʈ�� �����մϴ�.
		// �� �� ���񽺸� �׽�Ʈ�Ϸ��� <F5> Ű�� �����ϴ�.

//		[WebMethod]
//		public string HelloWorld()
//		{
//			return "Hello World";
//		}
	}
}
