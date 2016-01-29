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
	/// Sch3FormService에 대한 요약 설명입니다.
	/// </summary>
	public class Sch3FormService : System.Web.Services.WebService
	{
		private Sch3FormBiz	biz	= null;

		public Sch3FormService()
		{
			InitializeComponent();
			biz = new Sch3FormBiz();
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

		[WebMethod( Description="CSS편성시 사용되는 팝업윈도우의 첫번째 리스트인 카테고리/장르 목록을 가져옵니다")]
		public Sch3FormModel GetGenreListCSS(HeaderModel header, Sch3FormModel	model)
		{
			biz.GetGenreListCSS( header, model );
			return model;
		}

		[WebMethod( Description="지정편성시 사용되는 팝업윈도우의 첫번째 리스트인 카테고리/장르 목록을 가져옵니다")]
		public Sch3FormModel GetGenreListDesign(HeaderModel header, Sch3FormModel	model)
		{
			biz.GetGenreListDesign( header, model );
			return model;
		}

		[WebMethod( Description="일반편성시 사용되는 팝업윈도우의 첫번째 리스트인 카테고리/장르 목록을 가져옵니다")]
		public Sch3FormModel GetGenreListTot(HeaderModel header, Sch3FormModel	model)
		{
			biz.GetGenreListTot( header, model );
			return model;
		}

		[WebMethod( Description="편성시 사용되는 팝업윈도우의 두번째 리스트인 채널 목록을 가져옵니다")]
		public Sch3FormModel GetChannelList(HeaderModel header, Sch3FormModel	model)
		{
			biz.GetChannelList( header, model );
			return model;
		}

		[WebMethod( Description="편성시 사용되는 팝업윈도우의 세번째 리스트인 시리즈채널 목록을 가져옵니다")]
		public Sch3FormModel GetSeriesList(HeaderModel header, Sch3FormModel	model)
		{
			biz.GetSeriesList( header, model );
			return model;
		}
	}
}
