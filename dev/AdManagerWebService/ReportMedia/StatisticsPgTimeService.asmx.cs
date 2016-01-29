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
	/// StatisticsPgTimeService�� ���� ��� �����Դϴ�.
	/// </summary>
	public class StatisticsPgTimeService : System.Web.Services.WebService
	{
		public StatisticsPgTimeService()
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
		public  StatisticsPgTimeModel GetCategoryList(HeaderModel header, StatisticsPgTimeModel model)
		{
			new StatisticsPgTimeBiz().GetCategoryList(header, model);
			return model;
		}

		[WebMethod]
		public  StatisticsPgTimeModel GetGenreList(HeaderModel header, StatisticsPgTimeModel model)
		{
			new StatisticsPgTimeBiz().GetGenreList(header, model);
			return model;
		}

		[WebMethod]
		public  StatisticsPgTimeModel GetStatisticsPgTime(HeaderModel header, StatisticsPgTimeModel model)
		{
			new StatisticsPgTimeBiz().GetStatisticsPgTime(header, model);
			return model;
		}

		[WebMethod]
		public  StatisticsPgTimeModel GetStatisticsPgTimeAVG(HeaderModel header, StatisticsPgTimeModel model)
		{
			new StatisticsPgTimeBiz().GetStatisticsPgTimeAVG(header, model);
			return model;
		}
	}
}
