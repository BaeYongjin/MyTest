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
    /// 기본네임스페이스를 설정한다.
    /// </summary>
    [WebService(Namespace = "http://advmgt.hanafostv.com/AdManagerWebService/")]
    [System.ComponentModel.ToolboxItem(false)]

	/// <summary>
	/// GroupService에 대한 요약 설명입니다.
	/// </summary>
	public class GroupService : System.Web.Services.WebService
	{

		private GroupBiz groupBiz = null;

		public GroupService()
		{
			//CODEGEN: 이 호출은 ASP.NET 웹 서비스 디자이너에 필요합니다.
			InitializeComponent();

			groupBiz = new GroupBiz();
		}

		#region 구성 요소 디자이너에서 생성한 코드
		
		//웹 서비스 디자이너에 필요합니다. 
		private IContainer components = null;
				
		/// <summary>
		/// 디자이너 지원에 필요한 메서드입니다.
		/// 이 메서드의 내용을 코드 편집기로 수정하지 마십시오.
		/// </summary>
		private void InitializeComponent()
		{
		}

		/// <summary>
		/// 사용 중인 모든 리소스를 정리합니다.
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