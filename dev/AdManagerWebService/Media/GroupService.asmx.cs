using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Web;
using System.Web.Services;

using WinFramework.Base;
using AdManagerModel;

namespace AdManagerWebService.Media
{
    /// <summary>
    /// �⺻���ӽ����̽��� �����Ѵ�.
    /// </summary>
    [WebService(Namespace = "http://advmgt.hanafostv.com/AdManagerWebService/")]
    [System.ComponentModel.ToolboxItem(false)]

	/// <summary>
	/// GroupService�� ���� ��� �����Դϴ�.
	/// </summary>
	public class GroupService : System.Web.Services.WebService
	{

		private GroupBiz groupBiz = null;

		public GroupService()
		{
			//CODEGEN: �� ȣ���� ASP.NET �� ���� �����̳ʿ� �ʿ��մϴ�.
			InitializeComponent();

			groupBiz = new GroupBiz();
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
		public GroupModel GetGroupList(HeaderModel header, GroupModel groupModel)
		{
			groupBiz.GetGroupList(header, groupModel);
			return groupModel;
		}

		[WebMethod]
		public GroupModel GetGroupDetailList(HeaderModel header, GroupModel groupModel)
		{
			groupBiz.GetGroupDetailList(header, groupModel);
			return groupModel;
		}

		[WebMethod]
		public GroupModel GetCategoryList(HeaderModel header, GroupModel groupModel)
		{
			groupBiz.GetCategoryList(header, groupModel);
			return groupModel;
		}

        [WebMethod]
        public GroupModel GetCategoryList2(HeaderModel header, GroupModel groupModel)
        {
            groupBiz.GetCategoryList2(header, groupModel);
            return groupModel;
        }

		[WebMethod]
		public GroupModel GetGenreList(HeaderModel header, GroupModel groupModel)
		{
			groupBiz.GetGenreList(header, groupModel);
			return groupModel;
		}

        [WebMethod]
        public GroupModel GetGenreList2(HeaderModel header, GroupModel groupModel)
        {
            groupBiz.GetGenreList2(header, groupModel);
            return groupModel;
        }

		[WebMethod]
		public GroupModel GetChannelNoPopList(HeaderModel header, GroupModel groupModel)
		{
			groupBiz.GetChannelNoPopList(header, groupModel);
			return groupModel;
		}

        [WebMethod]
        public GroupModel GetChannelNoPopList2(HeaderModel header, GroupModel groupModel)
        {
            groupBiz.GetChannelNoPopList2(header, groupModel);
            return groupModel;
        }

		[WebMethod]
		public GroupModel GetSeriesList(HeaderModel header, GroupModel groupModel)
		{
			groupBiz.GetSeriesList(header, groupModel);
			return groupModel;
		}

        [WebMethod]
        public GroupModel GetSeriesList2(HeaderModel header, GroupModel groupModel)
        {
            groupBiz.GetSeriesList2(header, groupModel);
            return groupModel;
        }

        [WebMethod]
        public GroupModel GetGroupMapList(HeaderModel header, GroupModel groupModel)
        {
            groupBiz.GetGroupMapList(header, groupModel);
            return groupModel;
        }


		[WebMethod]
		public GroupModel SetGroupUpdate(HeaderModel header, GroupModel groupModel)
		{
			groupBiz.SetGroupUpdate(header, groupModel);
			return groupModel;
		}

		[WebMethod]
		public GroupModel SetGroupGenreUpdate(HeaderModel header, GroupModel groupModel)
		{
			groupBiz.SetGroupGenreUpdate(header, groupModel);
			return groupModel;
		}

		[WebMethod]
		public GroupModel SetGroupChannelUpdate(HeaderModel header, GroupModel groupModel)
		{
			groupBiz.SetGroupChannelUpdate(header, groupModel);
			return groupModel;
		}

		[WebMethod]
		public GroupModel SetGroupSeriesUpdate(HeaderModel header, GroupModel groupModel)
		{
			groupBiz.SetGroupSeriesUpdate(header, groupModel);
			return groupModel;
		}

		[WebMethod]
		public GroupModel SetGroupCreate(HeaderModel header, GroupModel groupModel)
		{
			groupBiz.SetGroupCreate(header, groupModel);
			return groupModel;
		}

		[WebMethod]
		public GroupModel SetGroupDetailCreate(HeaderModel header, GroupModel groupModel)
		{
			groupBiz.SetGroupDetailCreate(header, groupModel);
			return groupModel;
		}

		[WebMethod]
		public GroupModel SetGroupDelete(HeaderModel header, GroupModel groupModel)
		{
			groupBiz.SetGroupDelete(header, groupModel);
			return groupModel;
		}

		[WebMethod]
		public GroupModel SetGroupDetailDelete(HeaderModel header, GroupModel groupModel)
		{
			groupBiz.SetGroupDetailDelete(header, groupModel);
			return groupModel;
		}
	}
}