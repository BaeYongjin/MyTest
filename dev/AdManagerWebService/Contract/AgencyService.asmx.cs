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
	/// AgencyService�� ���� ��� �����Դϴ�.
	/// </summary>
	public class AgencyService : System.Web.Services.WebService
	{

		private AgencyBiz agencyBiz = null;

		public AgencyService()
		{
			//CODEGEN: �� ȣ���� ASP.NET �� ���� �����̳ʿ� �ʿ��մϴ�.
			InitializeComponent();

			agencyBiz = new AgencyBiz();
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
		public AgencyModel GetAgencyList(HeaderModel header, AgencyModel agencyModel)
		{
			agencyBiz.GetAgencyList(header, agencyModel);
			return agencyModel;
		}

		[WebMethod]
		public AgencyModel SetAgencyUpdate(HeaderModel header, AgencyModel agencyModel)
		{
			agencyBiz.SetAgencyUpdate(header, agencyModel);
			return agencyModel;
		}

		[WebMethod]
		public AgencyModel SetAgencyCreate(HeaderModel header, AgencyModel agencyModel)
		{
			agencyBiz.SetAgencyCreate(header, agencyModel);
			return agencyModel;
		}

		[WebMethod]
		public AgencyModel SetAgencyDelete(HeaderModel header, AgencyModel agencyModel)
		{
			agencyBiz.SetAgencyDelete(header, agencyModel);
			return agencyModel;
		}
	}
}
