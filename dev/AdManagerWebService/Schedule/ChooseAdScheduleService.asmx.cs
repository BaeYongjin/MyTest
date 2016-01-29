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
	/// ChooseAdScheduleService에 대한 요약 설명입니다.
	/// </summary>
	public class ChooseAdScheduleService : System.Web.Services.WebService
	{

		private ChooseAdScheduleBiz chooseAdScheduleBiz = null;

		public ChooseAdScheduleService()
		{
			//CODEGEN: 이 호출은 ASP.NET 웹 서비스 디자이너에 필요합니다.
			InitializeComponent();

			chooseAdScheduleBiz = new ChooseAdScheduleBiz();
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
		public ChooseAdScheduleModel GetMenuList(HeaderModel header, ChooseAdScheduleModel chooseAdScheduleModel)
		{
			chooseAdScheduleBiz.GetMenuList(header, chooseAdScheduleModel);
			return chooseAdScheduleModel;
		}
        
        [WebMethod(Description="카테고리/메뉴 목록데이터")]
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
