using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Web;
using System.Web.Services;

using WinFramework.Base;
using AdManagerModel;

namespace AdManagerWebService.ReportAd
{
    /// <summary>
    /// 기본네임스페이스를 설정한다.
    /// </summary>
    [WebService(Namespace = "http://advmgt.hanafostv.com/AdManagerWebService/")]
    [System.ComponentModel.ToolboxItem(false)]
	/// <summary>
	/// TimeSummaryService에 대한 요약 설명입니다.
	/// </summary>
	public class TimeSummaryService : System.Web.Services.WebService
	{
		public TimeSummaryService()
		{
			//CODEGEN: 이 호출은 ASP.NET 웹 서비스 디자이너에 필요합니다.
			InitializeComponent();
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
		public  TimeSummaryModel GetAreaTime(HeaderModel header, TimeSummaryModel model)
		{
			new TimeSummaryBiz().GetAreaTime(header, model);
			return model;
		}

		[WebMethod]
		public  TimeSummaryModel GetDateTime(HeaderModel header, TimeSummaryModel model)
		{
			new TimeSummaryBiz().GetDateTime(header, model);
			return model;
		}

		[WebMethod]
		public  TimeSummaryModel GetGenreTime(HeaderModel header, TimeSummaryModel model)
		{
			new TimeSummaryBiz().GetGenreTime(header, model);
			return model;
		}

		[WebMethod]
		public  TimeSummaryModel GetChannelTime(HeaderModel header, TimeSummaryModel model)
		{
			new TimeSummaryBiz().GetChannelTime(header, model);
			return model;
		}

		[WebMethod]
		public  TimeSummaryModel GetAreaTimeByJava(HeaderModel header, TimeSummaryModel model)
		{
			new TimeSummaryBiz().GetAreaTimeByJava(header, model);
			return model;
		}

		[WebMethod]
		public  TimeSummaryModel GetDateTimeByJava(HeaderModel header, TimeSummaryModel model)
		{
			new TimeSummaryBiz().GetDateTimeByJava(header, model);
			return model;
		}

		[WebMethod]
		public  TimeSummaryModel GetGenreTimeByJava(HeaderModel header, TimeSummaryModel model)
		{
			new TimeSummaryBiz().GetGenreTimeByJava(header, model);
			return model;
		}

		[WebMethod]
		public  TimeSummaryModel GetChannelTimeByJava(HeaderModel header, TimeSummaryModel model)
		{
			new TimeSummaryBiz().GetChannelTimeByJava(header, model);
			return model;
		}

	}
}
