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
	/// SchPublishService에 대한 요약 설명입니다.
	/// </summary>
	public class SchPublishService : System.Web.Services.WebService
	{
		private SchPublishBiz schPublishBiz = null;
		public SchPublishService()
		{
			//CODEGEN: 이 호출은 ASP.NET 웹 서비스 디자이너에 필요합니다.
			InitializeComponent();
			schPublishBiz = new SchPublishBiz();
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
