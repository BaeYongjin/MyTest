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
	/// SchMidAdService�� ���� ��� �����Դϴ�.
	/// </summary>
	public class SchMidAdService : System.Web.Services.WebService
	{
        private SchMidAdBiz biz = null;
		public SchMidAdService()
		{
			//CODEGEN: �� ȣ���� ASP.NET �� ���� �����̳ʿ� �ʿ��մϴ�.
			InitializeComponent();
            biz = new SchMidAdBiz();

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
        public ChooseAdScheduleModel GetMenuList(HeaderModel header, ChooseAdScheduleModel chooseAdScheduleModel)
        {
            biz.GetMenuList(header, chooseAdScheduleModel);
            return chooseAdScheduleModel;
        }

        [WebMethod]
        public ChooseAdScheduleModel GetChannelList(HeaderModel header, ChooseAdScheduleModel chooseAdScheduleModel)
        {
            biz.GetChannelList(header, chooseAdScheduleModel);
            return chooseAdScheduleModel;
        }

        [WebMethod]
        public ChooseAdScheduleModel GetScheduleListChannel(HeaderModel header, ChooseAdScheduleModel chooseAdScheduleModel)
        {
            biz.GetScheduleListChannel(header, chooseAdScheduleModel);
            return chooseAdScheduleModel;
        }

        [WebMethod]
        public ChooseAdScheduleModel GetMidAdInfoListSeries(HeaderModel header, ChooseAdScheduleModel chooseAdScheduleModel)
        {
            biz.GetMidAdInfoListSeries(header, chooseAdScheduleModel);
            return chooseAdScheduleModel;
        }

        
	}
}