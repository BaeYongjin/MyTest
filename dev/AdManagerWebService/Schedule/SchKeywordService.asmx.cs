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
	/// SchKeywordService�� ���� ��� �����Դϴ�.
	/// </summary>
	public class SchKeywordService : System.Web.Services.WebService
	{
		public SchKeywordService()
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
		public  SchKeywordModel GetChannelList(HeaderModel header, SchKeywordModel model)
		{
			new SchKeywordBiz().GetChannelList(header, model);
			return model;
		}

		[WebMethod]
		public  SchKeywordModel GetScheduleChannel(HeaderModel header, SchKeywordModel model)
		{
			new SchKeywordBiz().GetScheduleChannel(header, model);
			return model;
		}

		[WebMethod]
		public  SchKeywordModel GetContractItemList(HeaderModel header, SchKeywordModel model)
		{
			new SchKeywordBiz().GetContractItemList(header, model);
			return model;
		}

		[WebMethod]
		public  SchKeywordModel GetScheduleItem(HeaderModel header, SchKeywordModel model)
		{
			new SchKeywordBiz().GetScheduleItem(header, model);
			return model;
		}

	}
}
