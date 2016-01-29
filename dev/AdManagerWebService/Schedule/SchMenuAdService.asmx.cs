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
	/// SchMenuAdService에 대한 요약 설명입니다.
	/// </summary>
	public class SchMenuAdService : System.Web.Services.WebService
	{

		private SchMenuAdBiz schMenuAdBiz = null;

		public SchMenuAdService()
		{
			//CODEGEN: 이 호출은 ASP.NET 웹 서비스 디자이너에 필요합니다.
			InitializeComponent();

			schMenuAdBiz = new SchMenuAdBiz();

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

		[WebMethod( Description="특정광고에 선택한 편성데이터 삭제[장르/채널/회차 별 삭제처리 지원]")]
		public SchedulePerItemModel	SetSchedulePerItemDelete( HeaderModel header, SchedulePerItemModel data)
		{
			schMenuAdBiz.SetSchedulePerItemDelete( header, data );
			return data;
		}

		[WebMethod( Description="특정광고에 선택한 편성데이터 추가[장르/채널/회차 별 추가처리 지원]")]
		public SchedulePerItemModel	SetSchedulePerItemInsert( HeaderModel header, SchedulePerItemModel data)
		{
			schMenuAdBiz.SetSchedulePerItemInsert( header, data );
			return data;
		}

		[WebMethod]
		public SchMenuAdModel GetMenuList(HeaderModel header, SchMenuAdModel schMenuAdModel)
		{
			schMenuAdBiz.GetMenuList(header, schMenuAdModel);
			return schMenuAdModel;
		}

		[WebMethod]
		public SchMenuAdModel GetItemScheduleList(HeaderModel header, SchMenuAdModel schMenuAdModel)
		{
			schMenuAdBiz.GetItemScheduleList(header, schMenuAdModel);
			return schMenuAdModel;
		}

		[WebMethod]
		public SchMenuAdModel GetContractItemList(HeaderModel header, SchMenuAdModel schMenuAdModel)
		{
			schMenuAdBiz.GetContractItemList(header, schMenuAdModel);
			return schMenuAdModel;
		}

		[WebMethod]
		public SchMenuAdModel GetSchChoiceMenuDetailList(HeaderModel header, SchMenuAdModel schMenuAdModel)
		{
			schMenuAdBiz.GetSchChoiceMenuDetailList(header, schMenuAdModel);
			return schMenuAdModel;
		}

		[WebMethod]
		public SchMenuAdModel GetSchChoiceMenuDetailContractSeq(HeaderModel header, SchMenuAdModel schMenuAdModel)
		{
			schMenuAdBiz.GetSchChoiceMenuDetailContractSeq(header, schMenuAdModel);
			return schMenuAdModel;
		}

		[WebMethod]
		public SchMenuAdModel GetChooseAdScheduleListMenu(HeaderModel header, SchMenuAdModel schMenuAdModel)
		{
			schMenuAdBiz.GetChooseAdScheduleListMenu(header, schMenuAdModel);
			return schMenuAdModel;
		}

		[WebMethod]
		public SchMenuAdModel GetChooseAdScheduleListContract(HeaderModel header, SchMenuAdModel schMenuAdModel)
		{
			schMenuAdBiz.GetChooseAdScheduleListContract(header, schMenuAdModel);
			return schMenuAdModel;
		}
	}
}
