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
	/// StatisticsPgWeekService�� ���� ��� �����Դϴ�.
	/// </summary>
	public class StatisticsPgWeekService : System.Web.Services.WebService
	{
		public StatisticsPgWeekService()
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
		public  StatisticsPgWeekModel GetCategoryList(HeaderModel header, StatisticsPgWeekModel model)
		{
			new StatisticsPgWeekBiz().GetCategoryList(header, model);
			return model;
		}

		[WebMethod]
		public  StatisticsPgWeekModel GetGenreList(HeaderModel header, StatisticsPgWeekModel model)
		{
			new StatisticsPgWeekBiz().GetGenreList(header, model);
			return model;
		}

		[WebMethod]
		public  StatisticsPgWeekModel GetStatisticsPgWeek(HeaderModel header, StatisticsPgWeekModel model)
		{
			new StatisticsPgWeekBiz().GetStatisticsPgWeek(header, model);
			return model;
		}

		[WebMethod]
		public  StatisticsPgWeekModel GetStatisticsPgWeekAVG(HeaderModel header, StatisticsPgWeekModel model)
		{
			new StatisticsPgWeekBiz().GetStatisticsPgWeekAVG(header, model);
			return model;
		}
	}
}
