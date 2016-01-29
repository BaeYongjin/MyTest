using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Web;
using System.Web.Services;

using WinFramework.Base;
using AdManagerModel;

namespace AdManagerWebService.ReportMedia
{
    /// <summary>
    /// �⺻���ӽ����̽��� �����Ѵ�.
    /// </summary>
    [WebService(Namespace = "http://advmgt.hanafostv.com/AdManagerWebService/")]
    [System.ComponentModel.ToolboxItem(false)]
	/// <summary>
	/// StatisticsPgRegionService�� ���� ��� �����Դϴ�.
	/// </summary>
	public class StatisticsPgRegionService : System.Web.Services.WebService
	{
		public StatisticsPgRegionService()
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
		public  StatisticsPgRegionModel GetCategoryList(HeaderModel header, StatisticsPgRegionModel model)
		{
			new StatisticsPgRegionBiz().GetCategoryList(header, model);
			return model;
		}

		[WebMethod]
		public  StatisticsPgRegionModel GetGenreList(HeaderModel header, StatisticsPgRegionModel model)
		{
			new StatisticsPgRegionBiz().GetGenreList(header, model);
			return model;
		}

		[WebMethod]
		public  StatisticsPgRegionModel GetStatisticsPgRegion(HeaderModel header, StatisticsPgRegionModel model)
		{
			new StatisticsPgRegionBiz().GetStatisticsPgRegion(header, model);
			return model;
		}

		[WebMethod]
		public  StatisticsPgRegionModel GetStatisticsPgRegionAVG(HeaderModel header, StatisticsPgRegionModel model)
		{
			new StatisticsPgRegionBiz().GetStatisticsPgRegionAVG(header, model);
			return model;
		}
	}
}
