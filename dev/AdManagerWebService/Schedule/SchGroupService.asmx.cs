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
    /// 기본네임스페이스를 설정한다.
    /// </summary>
    [WebService(Namespace = "http://advmgt.hanafostv.com/AdManagerWebService/")]
    [System.ComponentModel.ToolboxItem(false)]

	/// <summary>
	/// SchGroupService에 대한 요약 설명입니다.
	/// </summary>
	public class SchGroupService : System.Web.Services.WebService
	{

		private SchGroupBiz schGroupBiz = null;

		public SchGroupService()
		{
			//CODEGEN: 이 호출은 ASP.NET 웹 서비스 디자이너에 필요합니다.
			InitializeComponent();

			schGroupBiz = new SchGroupBiz();
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
		public SchGroupModel GetGroupScheduleList(HeaderModel header, SchGroupModel schGroupModel)
		{
			schGroupBiz.GetGroupScheduleList(header, schGroupModel);
			return schGroupModel;
		}

        [WebMethod]
        public SchGroupModel GetGroupList(HeaderModel header, SchGroupModel schGroupModel)
        {
            schGroupBiz.GetGroupList(header, schGroupModel);
            return schGroupModel;
        }
        
		[WebMethod]
		public SchGroupModel GetChannelList(HeaderModel header, SchGroupModel schGroupModel)
		{
			schGroupBiz.GetChannelList(header, schGroupModel);
			return schGroupModel;
		}

		[WebMethod]
		public SchGroupModel GetChooseAdScheduleListMenu(HeaderModel header, SchGroupModel schGroupModel)
		{
			schGroupBiz.GetChooseAdScheduleListMenu(header, schGroupModel);
			return schGroupModel;
		}
		
		[WebMethod]
		public SchGroupModel GetChooseAdScheduleListChannel(HeaderModel header, SchGroupModel schGroupModel)
		{
			schGroupBiz.GetChooseAdScheduleListChannel(header, schGroupModel);
			return schGroupModel;
		}

		[WebMethod]
		public SchGroupModel SetSchGroupDelete(HeaderModel header, SchGroupModel schGroupModel)
		{
			schGroupBiz.SetSchGroupDelete(header, schGroupModel);
			return schGroupModel;
		}

		[WebMethod]
		public SchGroupModel SetSchGroupCreate(HeaderModel header, SchGroupModel schGroupModel)
		{
			schGroupBiz.SetSchGroupCreate(header, schGroupModel);
			return schGroupModel;
		}

		
		[WebMethod]
		public SchGroupModel SetSchMenuAdOrderFirst(HeaderModel header, SchGroupModel schGroupModel)
		{
			schGroupBiz.SetSchMenuAdOrderFirst(header, schGroupModel);
			return schGroupModel;
		}

		[WebMethod]
		public SchGroupModel SetSchMenuAdOrderUp(HeaderModel header, SchGroupModel schGroupModel)
		{
			schGroupBiz.SetSchMenuAdOrderUp(header, schGroupModel);
			return schGroupModel;
		}

		[WebMethod]
		public SchGroupModel SetSchMenuAdOrderDown(HeaderModel header, SchGroupModel schGroupModel)
		{
			schGroupBiz.SetSchMenuAdOrderDown(header, schGroupModel);
			return schGroupModel;
		}	

		[WebMethod]
		public SchGroupModel SetSchMenuAdOrderLast(HeaderModel header, SchGroupModel schGroupModel)
		{
			schGroupBiz.SetSchMenuAdOrderLast(header, schGroupModel);
			return schGroupModel;
		}		


		[WebMethod]
		public SchGroupModel SetSchChannelAdOrderFirst(HeaderModel header, SchGroupModel schGroupModel)
		{
			schGroupBiz.SetSchChannelAdOrderFirst(header, schGroupModel);
			return schGroupModel;
		}

		[WebMethod]
		public SchGroupModel SetSchChannelAdOrderUp(HeaderModel header, SchGroupModel schGroupModel)
		{
			schGroupBiz.SetSchChannelAdOrderUp(header, schGroupModel);
			return schGroupModel;
		}

		[WebMethod]
		public SchGroupModel SetSchChannelAdOrderDown(HeaderModel header, SchGroupModel schGroupModel)
		{
			schGroupBiz.SetSchChannelAdOrderDown(header, schGroupModel);
			return schGroupModel;
		}

        [WebMethod]
        public SchGroupModel SetSchChannelAdOrderLast(HeaderModel header, SchGroupModel schGroupModel)
        {
            schGroupBiz.SetSchChannelAdOrderLast(header, schGroupModel);
            return schGroupModel;
        }

		[WebMethod]
        public SchGroupModel GetSchGroupReport(HeaderModel header, SchGroupModel schGroupModel)
		{
            schGroupBiz.getSchGroupReport(header, schGroupModel);
			return schGroupModel;
        }

	}
}
