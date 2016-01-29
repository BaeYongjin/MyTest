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
	/// SchPopularChannelService�� ���� ��� �����Դϴ�.
	/// </summary>
	public class SchPopularChannelService : System.Web.Services.WebService
	{

		private SchPopularChannelBiz schPopularChannelBiz = null;

		public SchPopularChannelService()
		{
			//CODEGEN: �� ȣ���� ASP.NET �� ���� �����̳ʿ� �ʿ��մϴ�.
			InitializeComponent();

			schPopularChannelBiz = new SchPopularChannelBiz();
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
		public SchPopularChannelModel GetMenuList(HeaderModel header, SchPopularChannelModel schPopularChannelModel)
		{
			schPopularChannelBiz.GetMenuList(header, schPopularChannelModel);
			return schPopularChannelModel;
		}
        
		[WebMethod]
		public SchPopularChannelModel GetChannelList(HeaderModel header, SchPopularChannelModel schPopularChannelModel)
		{
			schPopularChannelBiz.GetChannelList(header, schPopularChannelModel);
			return schPopularChannelModel;
		}

		[WebMethod]
		public SchPopularChannelModel GetChooseAdScheduleListMenu(HeaderModel header, SchPopularChannelModel schPopularChannelModel)
		{
			schPopularChannelBiz.GetChooseAdScheduleListMenu(header, schPopularChannelModel);
			return schPopularChannelModel;
		}
		
		[WebMethod]
		public SchPopularChannelModel GetChooseAdScheduleListChannel(HeaderModel header, SchPopularChannelModel schPopularChannelModel)
		{
			schPopularChannelBiz.GetChooseAdScheduleListChannel(header, schPopularChannelModel);
			return schPopularChannelModel;
		}

		[WebMethod]
		public SchPopularChannelModel GetGenreGroupList_p(HeaderModel header, SchPopularChannelModel schPopularChannelModel)
		{
			schPopularChannelBiz.GetGenreGroupList_p(header, schPopularChannelModel);
			return schPopularChannelModel;
		}

		[WebMethod]
		public SchPopularChannelModel GetChannelList_p(HeaderModel header, SchPopularChannelModel schPopularChannelModel)
		{
			schPopularChannelBiz.GetChannelList_p(header, schPopularChannelModel);
			return schPopularChannelModel;
		}

		[WebMethod]
		public SchPopularChannelModel GetStatisticsPgChannel(HeaderModel header, SchPopularChannelModel schPopularChannelModel)
		{
			schPopularChannelBiz.GetStatisticsPgChannel(header, schPopularChannelModel);
			return schPopularChannelModel;
		}

		[WebMethod]
		public SchPopularChannelModel SetSchChoiceChannelDetailCreate(HeaderModel header, SchPopularChannelModel schPopularChannelModel)
		{
			schPopularChannelBiz.SetSchChoiceChannelDetailCreate(header, schPopularChannelModel);
			return schPopularChannelModel;
		}

		[WebMethod]
		public SchPopularChannelModel SetPopularChannelUpdate(HeaderModel header, SchPopularChannelModel schPopularChannelModel)
		{
			schPopularChannelBiz.SetPopularChannelUpdate(header, schPopularChannelModel);
			return schPopularChannelModel;
		}


		[WebMethod]
		public SchPopularChannelModel SetSchPopularChannelDetailCreate(HeaderModel header, SchPopularChannelModel schPopularChannelModel)
		{
			schPopularChannelBiz.SetSchPopularChannelDetailCreate(header, schPopularChannelModel);
			return schPopularChannelModel;
		}

		[WebMethod]
		public SchPopularChannelModel SetSchPopularChannelDelete(HeaderModel header, SchPopularChannelModel schPopularChannelModel)
		{
			schPopularChannelBiz.SetSchPopularChannelDelete(header, schPopularChannelModel);
			return schPopularChannelModel;
		}

		[WebMethod]
		public SchPopularChannelModel SetPopularChannelDelete(HeaderModel header, SchPopularChannelModel schPopularChannelModel)
		{
			schPopularChannelBiz.SetPopularChannelDelete(header, schPopularChannelModel);
			return schPopularChannelModel;
		}

		
		[WebMethod]
		public SchPopularChannelModel SetSchMenuAdOrderFirst(HeaderModel header, SchPopularChannelModel schPopularChannelModel)
		{
			schPopularChannelBiz.SetSchMenuAdOrderFirst(header, schPopularChannelModel);
			return schPopularChannelModel;
		}

		[WebMethod]
		public SchPopularChannelModel SetSchMenuAdOrderUp(HeaderModel header, SchPopularChannelModel schPopularChannelModel)
		{
			schPopularChannelBiz.SetSchMenuAdOrderUp(header, schPopularChannelModel);
			return schPopularChannelModel;
		}

		[WebMethod]
		public SchPopularChannelModel SetSchMenuAdOrderDown(HeaderModel header, SchPopularChannelModel schPopularChannelModel)
		{
			schPopularChannelBiz.SetSchMenuAdOrderDown(header, schPopularChannelModel);
			return schPopularChannelModel;
		}	

		[WebMethod]
		public SchPopularChannelModel SetSchMenuAdOrderLast(HeaderModel header, SchPopularChannelModel schPopularChannelModel)
		{
			schPopularChannelBiz.SetSchMenuAdOrderLast(header, schPopularChannelModel);
			return schPopularChannelModel;
		}		


		[WebMethod]
		public SchPopularChannelModel SetSchChannelAdOrderFirst(HeaderModel header, SchPopularChannelModel schPopularChannelModel)
		{
			schPopularChannelBiz.SetSchChannelAdOrderFirst(header, schPopularChannelModel);
			return schPopularChannelModel;
		}

		[WebMethod]
		public SchPopularChannelModel SetSchChannelAdOrderUp(HeaderModel header, SchPopularChannelModel schPopularChannelModel)
		{
			schPopularChannelBiz.SetSchChannelAdOrderUp(header, schPopularChannelModel);
			return schPopularChannelModel;
		}

		[WebMethod]
		public SchPopularChannelModel SetSchChannelAdOrderDown(HeaderModel header, SchPopularChannelModel schPopularChannelModel)
		{
			schPopularChannelBiz.SetSchChannelAdOrderDown(header, schPopularChannelModel);
			return schPopularChannelModel;
		}	

		[WebMethod]
		public SchPopularChannelModel SetSchChannelAdOrderLast(HeaderModel header, SchPopularChannelModel schPopularChannelModel)
		{
			schPopularChannelBiz.SetSchChannelAdOrderLast(header, schPopularChannelModel);
			return schPopularChannelModel;
		}		

	}
}
