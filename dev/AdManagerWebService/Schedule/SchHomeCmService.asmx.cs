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
	/// SchHomeAdService�� ���� ��� �����Դϴ�.
	/// </summary>
	public class SchHomeCmService : System.Web.Services.WebService
	{

		private SchHomeCmBiz schHomeCmBiz = null;

		public SchHomeCmService()
		{
			//CODEGEN: �� ȣ���� ASP.NET �� ���� �����̳ʿ� �ʿ��մϴ�.
			InitializeComponent();

			schHomeCmBiz = new SchHomeCmBiz();
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
		public SchHomeCmModel GetSchHomeCmList(HeaderModel header, SchHomeCmModel data)
		{
			schHomeCmBiz.GetSchHomeCMList(header, data);
			return data;
		}

		[WebMethod]
		public SchHomeCmModel GetSchHomeCmDetail(HeaderModel header, SchHomeCmModel data)
		{
			schHomeCmBiz.GetSchHomeCMDetail(header, data);
			return data;
		}

		[WebMethod]
		public SchHomeCmModel SetSchHomeCmCreate(HeaderModel header, SchHomeCmModel data)
		{
			schHomeCmBiz.SetSchHomeCmCreate(header, data);
			return data;
		}

		[WebMethod]
		public SchHomeCmModel SetSchHomeCmUpdate(HeaderModel header, SchHomeCmModel data)
		{
			schHomeCmBiz.SetSchHomeCmUpdate(header, data);
			return data;
		}

		[WebMethod]
		public SchHomeCmModel SetSchHomeCmDelete(HeaderModel header, SchHomeCmModel data)
		{
			schHomeCmBiz.SetSchHomeCmDelete(header, data);
			return data;
		}

        [WebMethod]
        public SchHomeCmModel GetSlotState(HeaderModel header, SchHomeCmModel data)
        {
            schHomeCmBiz.GetSlotState(header, data);
            return data;
        }
        [WebMethod]
        public SchHomeCmModel GetSlotCount(HeaderModel header, SchHomeCmModel data)
        {
            schHomeCmBiz.GetSlotCount(header, data);
            return data;
        }
        [WebMethod]
        public SchHomeCmModel GetSlotAd(HeaderModel header, SchHomeCmModel data)
        {
            schHomeCmBiz.GetSlotAd(header, data);
            return data;
        }
	}
}
