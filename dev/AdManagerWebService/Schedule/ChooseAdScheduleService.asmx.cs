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
	/// ChooseAdScheduleService�� ���� ��� �����Դϴ�.
	/// </summary>
	public class ChooseAdScheduleService : System.Web.Services.WebService
	{

		private ChooseAdScheduleBiz chooseAdScheduleBiz = null;

		public ChooseAdScheduleService()
		{
			//CODEGEN: �� ȣ���� ASP.NET �� ���� �����̳ʿ� �ʿ��մϴ�.
			InitializeComponent();

			chooseAdScheduleBiz = new ChooseAdScheduleBiz();
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
			chooseAdScheduleBiz.GetMenuList(header, chooseAdScheduleModel);
			return chooseAdScheduleModel;
		}
        
        [WebMethod(Description="ī�װ�/�޴� ��ϵ�����")]
        public ChooseAdScheduleModel wMenuList1(HeaderModel header, ChooseAdScheduleModel chooseAdScheduleModel)
        {
            chooseAdScheduleBiz.svcMenuList1(header, chooseAdScheduleModel);
            return chooseAdScheduleModel;
        }


		[WebMethod]
		public ChooseAdScheduleModel GetChannelList(HeaderModel header, ChooseAdScheduleModel chooseAdScheduleModel)
		{
			chooseAdScheduleBiz.GetChannelList(header, chooseAdScheduleModel);
			return chooseAdScheduleModel;
		}

        [WebMethod]
        public ChooseAdScheduleModel GetChannelList_0907a(HeaderModel header, ChooseAdScheduleModel chooseAdScheduleModel)
        {
            chooseAdScheduleBiz.GetChannelList_0907a(header, chooseAdScheduleModel);
            return chooseAdScheduleModel;
        }

		[WebMethod]
		public ChooseAdScheduleModel GetSeriesList_0907a(HeaderModel header, ChooseAdScheduleModel chooseAdScheduleModel)
		{
			chooseAdScheduleBiz.GetSeriesList_0907a(header, chooseAdScheduleModel);
			return chooseAdScheduleModel;
		}

		[WebMethod]
		public ChooseAdScheduleModel GetChooseAdScheduleListMenu(HeaderModel header, ChooseAdScheduleModel chooseAdScheduleModel)
		{
			chooseAdScheduleBiz.GetChooseAdScheduleListMenu(header, chooseAdScheduleModel);
			return chooseAdScheduleModel;
		}
		
		[WebMethod]
		public ChooseAdScheduleModel GetChooseAdScheduleListChannel(HeaderModel header, ChooseAdScheduleModel chooseAdScheduleModel)
		{
			chooseAdScheduleBiz.GetChooseAdScheduleListChannel(header, chooseAdScheduleModel);
			return chooseAdScheduleModel;
		}

		[WebMethod]
		public ChooseAdScheduleModel GetChooseAdScheduleListSeries(HeaderModel header, ChooseAdScheduleModel chooseAdScheduleModel)
		{
			chooseAdScheduleBiz.GetChooseAdScheduleListSeries(header, chooseAdScheduleModel);
			return chooseAdScheduleModel;
		}

		
		[WebMethod]
		public ChooseAdScheduleModel SetSchMenuAdOrderFirst(HeaderModel header, ChooseAdScheduleModel chooseAdScheduleModel)
		{
			chooseAdScheduleBiz.SetSchMenuAdOrderFirst(header, chooseAdScheduleModel);
			return chooseAdScheduleModel;
		}

		[WebMethod]
		public ChooseAdScheduleModel SetSchMenuAdOrderUp(HeaderModel header, ChooseAdScheduleModel chooseAdScheduleModel)
		{
			chooseAdScheduleBiz.SetSchMenuAdOrderUp(header, chooseAdScheduleModel);
			return chooseAdScheduleModel;
		}

		[WebMethod]
		public ChooseAdScheduleModel SetSchMenuAdOrderDown(HeaderModel header, ChooseAdScheduleModel chooseAdScheduleModel)
		{
			chooseAdScheduleBiz.SetSchMenuAdOrderDown(header, chooseAdScheduleModel);
			return chooseAdScheduleModel;
		}	

		[WebMethod]
		public ChooseAdScheduleModel SetSchMenuAdOrderLast(HeaderModel header, ChooseAdScheduleModel chooseAdScheduleModel)
		{
			chooseAdScheduleBiz.SetSchMenuAdOrderLast(header, chooseAdScheduleModel);
			return chooseAdScheduleModel;
		}		


		[WebMethod]
		public ChooseAdScheduleModel SetSchChannelAdOrderFirst(HeaderModel header, ChooseAdScheduleModel chooseAdScheduleModel)
		{
			chooseAdScheduleBiz.SetSchChannelAdOrderFirst(header, chooseAdScheduleModel);
			return chooseAdScheduleModel;
		}

		[WebMethod]
		public ChooseAdScheduleModel SetSchChannelAdOrderUp(HeaderModel header, ChooseAdScheduleModel chooseAdScheduleModel)
		{
			chooseAdScheduleBiz.SetSchChannelAdOrderUp(header, chooseAdScheduleModel);
			return chooseAdScheduleModel;
		}

		[WebMethod]
		public ChooseAdScheduleModel SetSchChannelAdOrderDown(HeaderModel header, ChooseAdScheduleModel chooseAdScheduleModel)
		{
			chooseAdScheduleBiz.SetSchChannelAdOrderDown(header, chooseAdScheduleModel);
			return chooseAdScheduleModel;
		}	

		[WebMethod]
		public ChooseAdScheduleModel SetSchChannelAdOrderLast(HeaderModel header, ChooseAdScheduleModel chooseAdScheduleModel)
		{
			chooseAdScheduleBiz.SetSchChannelAdOrderLast(header, chooseAdScheduleModel);
			return chooseAdScheduleModel;
		}		
	}
}
