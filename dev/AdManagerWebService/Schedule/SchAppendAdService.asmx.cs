using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Web;
using System.Web.Services;

using WinFramework.Base;
using AdManagerModel;

namespace AdManagerWebService.Schedule
{
    /// <summary>
    /// �⺻���ӽ����̽��� �����Ѵ�.
    /// </summary>
    [WebService(Namespace = "http://advmgt.hanafostv.com/AdManagerWebService/")]
    [System.ComponentModel.ToolboxItem(false)]

	/// <summary>
	/// SchAppendAdService�� ���� ��� �����Դϴ�.
	/// </summary>
	public class SchAppendAdService : System.Web.Services.WebService
	{

		private SchAppendAdBiz schAppendAdBiz = null;

		public SchAppendAdService()
		{
			//CODEGEN: �� ȣ���� ASP.NET �� ���� �����̳ʿ� �ʿ��մϴ�.
			InitializeComponent();

			schAppendAdBiz = new SchAppendAdBiz();
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
		public SchAppendAdModel GetSchAppendAdList(HeaderModel header, SchAppendAdModel schAppendAdModel)
		{
			schAppendAdBiz.GetSchAppendAdList(header, schAppendAdModel);
			return schAppendAdModel;
		}

		[WebMethod]
		public SchAppendAdModel GetContractItemList(HeaderModel header, SchAppendAdModel schAppendAdModel)
		{
			schAppendAdBiz.GetContractItemList(header, schAppendAdModel);
			return schAppendAdModel;
		}

		[WebMethod]
		public SchAppendAdModel SetSchAppendSearch(HeaderModel header, SchAppendAdModel schAppendAdModel)
		{
			schAppendAdBiz.SetSchAppendSearch(header, schAppendAdModel);
			return schAppendAdModel;
		}

		[WebMethod]
		public SchAppendAdModel SetSchAppendAdCreate(HeaderModel header, SchAppendAdModel schAppendAdModel)
		{
			schAppendAdBiz.SetSchAppendAdCreate(header, schAppendAdModel);
			return schAppendAdModel;
		}

		[WebMethod]
		public SchAppendAdModel SetSchAppendAdDelete_To(HeaderModel header, SchAppendAdModel schAppendAdModel)
		{
			schAppendAdBiz.SetSchAppendAdDelete_To(header, schAppendAdModel);
			return schAppendAdModel;
		}

		[WebMethod]
		public SchAppendAdModel SetSchAppendAdCreate_To(HeaderModel header, SchAppendAdModel schAppendAdModel)
		{
			schAppendAdBiz.SetSchAppendAdCreate_To(header, schAppendAdModel);
			return schAppendAdModel;
		}
		
		[WebMethod]
		public SchAppendAdModel SetSchAppendAdDelete(HeaderModel header, SchAppendAdModel schAppendAdModel)
		{
			schAppendAdBiz.SetSchAppendAdDelete(header, schAppendAdModel);
			return schAppendAdModel;
		}

		[WebMethod]
		public SchAppendAdModel SetSchAppendAdOrderFirst(HeaderModel header, SchAppendAdModel schAppendAdModel)
		{
			schAppendAdBiz.SetSchAppendAdOrderFirst(header, schAppendAdModel);
			return schAppendAdModel;
		}

		[WebMethod]
		public SchAppendAdModel SetSchAppendAdOrderUp(HeaderModel header, SchAppendAdModel schAppendAdModel)
		{
			schAppendAdBiz.SetSchAppendAdOrderUp(header, schAppendAdModel);
			return schAppendAdModel;
		}

		[WebMethod]
		public SchAppendAdModel SetSchAppendAdOrderDown(HeaderModel header, SchAppendAdModel schAppendAdModel)
		{
			schAppendAdBiz.SetSchAppendAdOrderDown(header, schAppendAdModel);
			return schAppendAdModel;
		}

		[WebMethod]
		public SchAppendAdModel SetSchAppendAdOrderLast(HeaderModel header, SchAppendAdModel schAppendAdModel)
		{
			schAppendAdBiz.SetSchAppendAdOrderLast(header, schAppendAdModel);
			return schAppendAdModel;
		}

	}
}
