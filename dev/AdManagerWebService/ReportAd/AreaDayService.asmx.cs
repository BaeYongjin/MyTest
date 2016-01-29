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
	/// AreaDayService�� ���� ��� �����Դϴ�.
	/// </summary>
	public class AreaDayService : System.Web.Services.WebService
	{
		public AreaDayService()
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
		public  AreaDayModel GetAreaDay(HeaderModel header, AreaDayModel model)
		{
			new AreaDayBiz().GetAreaDay(header, model);
			return model;
		}

		[WebMethod]
		public  AreaDayModel GetAreaDayByJava(HeaderModel header, AreaDayModel model)
		{
			new AreaDayBiz().GetAreaDayByJava(header, model);
			return model;
		}

		[WebMethod]
		public  AreaDayModel GetTimeDay(HeaderModel header, AreaDayModel model)
		{
			new AreaDayBiz().GetTimeDay(header, model);
			return model;
		}

		[WebMethod]
		public  AreaDayModel GetTimeDayByJava(HeaderModel header, AreaDayModel model)
		{
			new AreaDayBiz().GetTimeDayByJava(header, model);
			return model;
		}

		[WebMethod]
		public  AreaDayModel GetGenreDay(HeaderModel header, AreaDayModel model)
		{
			new AreaDayBiz().GetGenreDay(header, model);
			return model;
		}

		[WebMethod]
		public  AreaDayModel GetGenreDayByJava(HeaderModel header, AreaDayModel model)
		{
			new AreaDayBiz().GetGenreDayByJava(header, model);
			return model;
		}

		[WebMethod]
		public  AreaDayModel GetChannelDay(HeaderModel header, AreaDayModel model)
		{
			new AreaDayBiz().GetChannelDay(header, model);
			return model;
		}

		[WebMethod]
		public  AreaDayModel GetChannelDayByJava(HeaderModel header, AreaDayModel model)
		{
			new AreaDayBiz().GetChannelDayByJava(header, model);
			return model;
		}

	}
}
