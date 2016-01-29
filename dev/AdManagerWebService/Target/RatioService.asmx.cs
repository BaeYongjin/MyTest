using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Web;
using System.Web.Services;

using WinFramework.Base;
using AdManagerModel;

namespace AdManagerWebService.Target
{
    /// <summary>
    /// �⺻���ӽ����̽��� �����Ѵ�.
    /// </summary>
    [WebService(Namespace = "http://advmgt.hanafostv.com/AdManagerWebService/")]
    [System.ComponentModel.ToolboxItem(false)]

	/// <summary>
	/// RatioService�� ���� ��� �����Դϴ�.
	/// </summary>
	public class RatioService : System.Web.Services.WebService
	{

		private RatioBiz ratioBiz = null;

		public RatioService()
		{
			//CODEGEN: �� ȣ���� ASP.NET �� ���� �����̳ʿ� �ʿ��մϴ�.
			InitializeComponent();

			ratioBiz = new RatioBiz();
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
		public RatioModel GetSchChoiceMenuDetailList(HeaderModel header, RatioModel ratioModel)
		{
			ratioBiz.GetSchChoiceMenuDetailList(header, ratioModel);
			return ratioModel;
		}

		[WebMethod]
		public RatioModel GetSchRateList(HeaderModel header, RatioModel ratioModel)
		{
			ratioBiz.GetSchRateList(header, ratioModel);
			return ratioModel;
		}

		[WebMethod]
		public RatioModel GetSchRateDetailList(HeaderModel header, RatioModel ratioModel)
		{
			ratioBiz.GetSchRateDetailList(header, ratioModel);
			return ratioModel;
		}

		[WebMethod]
		public RatioModel GetGroup1List(HeaderModel header, RatioModel ratioModel)
		{
			ratioBiz.GetGroup1List(header, ratioModel);
			return ratioModel;
		}

		[WebMethod]
		public RatioModel GetGroup2List(HeaderModel header, RatioModel ratioModel)
		{
			ratioBiz.GetGroup2List(header, ratioModel);
			return ratioModel;
		}

		[WebMethod]
		public RatioModel GetGroup3List(HeaderModel header, RatioModel ratioModel)
		{
			ratioBiz.GetGroup3List(header, ratioModel);
			return ratioModel;
		}

		[WebMethod]
		public RatioModel GetGroup4List(HeaderModel header, RatioModel ratioModel)
		{
			ratioBiz.GetGroup4List(header, ratioModel);
			return ratioModel;
		}

		[WebMethod]
		public RatioModel GetGroup5List(HeaderModel header, RatioModel ratioModel)
		{
			ratioBiz.GetGroup5List(header, ratioModel);
			return ratioModel;
		}	

		[WebMethod]
		public RatioModel SetSchRateUpdate(HeaderModel header, RatioModel ratioModel)
		{
			ratioBiz.SetSchRateUpdate(header, ratioModel);
			return ratioModel;
		}

		[WebMethod]
		public RatioModel SetSchRateCreate(HeaderModel header, RatioModel ratioModel)
		{
			ratioBiz.SetSchRateCreate(header, ratioModel);
			return ratioModel;
		}

		[WebMethod]
		public RatioModel SetSchRateDetailCreate(HeaderModel header, RatioModel ratioModel)
		{
			ratioBiz.SetSchRateDetailCreate(header, ratioModel);
			return ratioModel;
		}

		[WebMethod]
		public RatioModel SetSchRateDelete(HeaderModel header, RatioModel ratioModel)
		{
			ratioBiz.SetSchRateDelete(header, ratioModel);
			return ratioModel;
		}
		
		[WebMethod]
		public RatioModel SetSchRateDetailDelete(HeaderModel header, RatioModel ratioModel)
		{
			ratioBiz.SetSchRateDetailDelete(header, ratioModel);
			return ratioModel;
		}

        [WebMethod]
        public RatioModel mDeleteSync(HeaderModel header, RatioModel ratioModel)
        {
            ratioBiz.DeleteSync(header, ratioModel);
            return ratioModel;
        }
	}
}
