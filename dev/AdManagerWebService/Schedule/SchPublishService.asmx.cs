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
	/// SchPublishService�� ���� ��� �����Դϴ�.
	/// </summary>
	public class SchPublishService : System.Web.Services.WebService
	{
		private SchPublishBiz schPublishBiz = null;
		public SchPublishService()
		{
			//CODEGEN: �� ȣ���� ASP.NET �� ���� �����̳ʿ� �ʿ��մϴ�.
			InitializeComponent();
			schPublishBiz = new SchPublishBiz();
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
		public SchPublishModel GetPublishState(HeaderModel header, SchPublishModel schPublishModel)
		{
			schPublishBiz.GetPublishState(header, schPublishModel);
			return schPublishModel;
		}

		[WebMethod]
		public SchPublishModel GetHomePublishState(HeaderModel header, SchPublishModel schPublishModel)
		{
			schPublishBiz.GetHomePublishState(header, schPublishModel);
			return schPublishModel;
		}

		[WebMethod]
		public SchPublishModel GetCmPublishState(HeaderModel header, SchPublishModel schPublishModel)
		{
			schPublishBiz.GetCMPublishState(header, schPublishModel);
			return schPublishModel;
		}

		[WebMethod]
		public SchPublishModel GetOapPublishState(HeaderModel header, SchPublishModel schPublishModel)
		{
			schPublishBiz.GetOAPPublishState(header, schPublishModel);
			return schPublishModel;
		}

		[WebMethod]
		public SchPublishModel GetHomeKidsPublishState(HeaderModel header, SchPublishModel schPublishModel)
		{
			schPublishBiz.GetHomeKidsPublishState(header, schPublishModel);
			return schPublishModel;
		}

        [WebMethod]
        public SchPublishModel GetHomeTargetPublishState(HeaderModel header, SchPublishModel schPublishModel)
        {
            schPublishBiz.GetHomeTargetPublishState(header, schPublishModel);
            return schPublishModel;
        }

		[WebMethod]
		public SchPublishModel GetSchPublishList(HeaderModel header, SchPublishModel schPublishModel)
		{
			schPublishBiz.GetSchPublishList(header, schPublishModel);
			return schPublishModel;
		}

		[WebMethod]
		public SchPublishModel GetScheduleList(HeaderModel header, SchPublishModel schPublishModel)
		{
			schPublishBiz.GetScheduleList_S1(header, schPublishModel);
			return schPublishModel;
		}

		[WebMethod]
		public SchPublishModel SetScheduleAck(HeaderModel header, SchPublishModel schPublishModel)
		{
			lock(schPublishBiz)
			{
				schPublishBiz.SetScheduleAck(header, schPublishModel);
			}
			return schPublishModel;
		}

		[WebMethod]
		public SchPublishModel SetScheduleAckCancel(HeaderModel header, SchPublishModel schPublishModel)
		{
			schPublishBiz.SetScheduleAckCancel(header, schPublishModel);
			return schPublishModel;
		}

		[WebMethod]
		public SchPublishModel SetScheduleChk(HeaderModel header, SchPublishModel schPublishModel)
		{
			schPublishBiz.SetScheduleChk(header, schPublishModel);
			return schPublishModel;
		}

		[WebMethod]
		public SchPublishModel SetScheduleChkCancel(HeaderModel header, SchPublishModel schPublishModel)
		{
			schPublishBiz.SetScheduleChkCancel(header, schPublishModel);
			return schPublishModel;
		}

		
		[WebMethod]
		public SchPublishModel SetSchedulePublish(HeaderModel header, SchPublishModel schPublishModel)
		{
			schPublishBiz.SetSchedulePublish_S1(header, schPublishModel);
			return schPublishModel;
		}

		[WebMethod]
		public SchPublishModel SetSchedulePublishCancel(HeaderModel header, SchPublishModel schPublishModel)
		{
			schPublishBiz.SetSchedulePublishCancel(header, schPublishModel);
			return schPublishModel;
		}

	}
}
