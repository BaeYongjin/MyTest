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
	/// SchChoiceAdService에 대한 요약 설명입니다.
	/// </summary>
	public class SchChoiceAdService : System.Web.Services.WebService
	{

		private SchChoiceAdBiz schChoiceAdBiz = null;

		public SchChoiceAdService()
		{
			//CODEGEN: 이 호출은 ASP.NET 웹 서비스 디자이너에 필요합니다.
			InitializeComponent();

			schChoiceAdBiz = new SchChoiceAdBiz();
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


        [WebMethod(Description="상업광고 목록을 가져옵니다")]
        public SchChoiceAdModel mGetAdList10(HeaderModel header, SchChoiceAdModel schChoiceAdModel)
        {
            schChoiceAdBiz.GetAdList10(header, schChoiceAdModel);
            return schChoiceAdModel;
        }


		[WebMethod]
		public SchChoiceAdModel GetSchChoiceAdList(HeaderModel header, SchChoiceAdModel schChoiceAdModel)
		{
			schChoiceAdBiz.GetSchChoiceAdList(header, schChoiceAdModel);
			return schChoiceAdModel;
		}

		[WebMethod]
		public SchChoiceAdModel GetInspectItemList(HeaderModel header, SchChoiceAdModel schChoiceAdModel)
		{
			schChoiceAdBiz.GetInspectItemList(header, schChoiceAdModel);
			return schChoiceAdModel;
		}

		[WebMethod]
		public SchChoiceAdModel GetContractItemList(HeaderModel header, SchChoiceAdModel schChoiceAdModel)
		{
			schChoiceAdBiz.GetContractItemList(header, schChoiceAdModel);
			return schChoiceAdModel;
		}

        [WebMethod]
        public SchChoiceAdModel GetContractItemList_0907a(HeaderModel header, SchChoiceAdModel schChoiceAdModel)
        {
            // 위에꺼 업그레이드 버젼임
            // 패치 완료후 위에 함수는 제거할것
            schChoiceAdBiz.GetContractItemList_0907a(header, schChoiceAdModel);
            return schChoiceAdModel;
        }
		
		[WebMethod]
		public SchChoiceAdModel SetSchChoiceChannelCreate(HeaderModel header, SchChoiceAdModel schChoiceAdModel)
		{
			schChoiceAdBiz.SetSchChoiceChannelCreate(header, schChoiceAdModel);
			return schChoiceAdModel;
		}

		[WebMethod]
		public SchChoiceAdModel SetSchChoiceChannelDelete(HeaderModel header, SchChoiceAdModel schChoiceAdModel)
		{
			schChoiceAdBiz.SetSchChoiceChannelDelete(header, schChoiceAdModel);
			return schChoiceAdModel;
		}

		

		[WebMethod]
		public SchChoiceAdModel SetSchChoiceChannelDetailCreate(HeaderModel header, SchChoiceAdModel schChoiceAdModel)
		{
			schChoiceAdBiz.SetSchChoiceChannelDetailCreate(header, schChoiceAdModel);
			return schChoiceAdModel;
		}

		[WebMethod]
		public SchChoiceAdModel SetSchChoiceSeriesDetailCreate(HeaderModel header, SchChoiceAdModel schChoiceAdModel)
		{
			schChoiceAdBiz.SetSchChoiceSeriesDetailCreate(header, schChoiceAdModel);
			return schChoiceAdModel;
		}

		[WebMethod]
		public SchChoiceAdModel SetSchChoiceChannelDetailDelete(HeaderModel header, SchChoiceAdModel schChoiceAdModel)
		{
			schChoiceAdBiz.SetSchChoiceChannelDetailDelete(header, schChoiceAdModel);
			return schChoiceAdModel;
		}

		[WebMethod]
		public SchChoiceAdModel SetSchChoiceMenuCreate(HeaderModel header, SchChoiceAdModel schChoiceAdModel)
		{
			schChoiceAdBiz.SetSchChoiceMenuCreate(header, schChoiceAdModel);
			return schChoiceAdModel;
		}

		[WebMethod]
		public SchChoiceAdModel SetSchChoiceMenuDelete(HeaderModel header, SchChoiceAdModel schChoiceAdModel)
		{
			schChoiceAdBiz.SetSchChoiceMenuDelete(header, schChoiceAdModel);
			return schChoiceAdModel;
		}

		[WebMethod]
		public SchChoiceAdModel SetSchChoiceMenuDetailCreate(HeaderModel header, SchChoiceAdModel schChoiceAdModel)
		{
			schChoiceAdBiz.SetSchChoiceMenuDetailCreate(header, schChoiceAdModel);
			return schChoiceAdModel;
		}

		[WebMethod(Description = "재핑광고 상세내역 편성 처리")]
		public SchChoiceAdModel SetSchChoiceRealChDetailCreate(HeaderModel header, SchChoiceAdModel schChoiceAdModel)
		{
			schChoiceAdBiz.SetSchChoiceRealChDetailCreate(header, schChoiceAdModel);
			return schChoiceAdModel;
		}

		[WebMethod]
		public SchChoiceAdModel SetSchChoiceMenuDetailDelete(HeaderModel header, SchChoiceAdModel schChoiceAdModel)
		{
			schChoiceAdBiz.SetSchChoiceMenuDetailDelete(header, schChoiceAdModel);
			return schChoiceAdModel;
		}

		[WebMethod(Description = "재핑광고 상세내역 편성 삭제")]
		public SchChoiceAdModel SetSchChoiceRealChDetailDelete(HeaderModel header, SchChoiceAdModel schChoiceAdModel)
		{
			schChoiceAdBiz.SetSchChoiceRealChDetailDelete(header, schChoiceAdModel);
			return schChoiceAdModel;
		}

		[WebMethod]
		public SchChoiceAdModel GetSchChoiceMenuDetailList(HeaderModel header, SchChoiceAdModel schChoiceAdModel)
		{
			schChoiceAdBiz.GetSchChoiceMenuDetailList(header, schChoiceAdModel);
			return schChoiceAdModel;
		}

		[WebMethod]
		public SchChoiceAdModel GetSchChoiceChannelDetailList(HeaderModel header, SchChoiceAdModel schChoiceAdModel)
		{
			schChoiceAdBiz.GetSchChoiceChannelDetailList(header, schChoiceAdModel);
			return schChoiceAdModel;
		}

		[WebMethod]
		public SchChoiceAdModel GetSchChoiceLastItemNo(HeaderModel header, SchChoiceAdModel schChoiceAdModel)
		{
			schChoiceAdBiz.GetSchChoiceLastItemNo(header, schChoiceAdModel);
			return schChoiceAdModel;
		}

		[WebMethod]
		public SchChoiceAdModel SetSchChoiceMenuDelete_To(HeaderModel header, SchChoiceAdModel schChoiceAdModel)
		{
			schChoiceAdBiz.SetSchChoiceMenuDelete_To(header, schChoiceAdModel);
			return schChoiceAdModel;
		}

		[WebMethod]
		public SchChoiceAdModel SetSchChoiceMenuDetailCreate_To(HeaderModel header, SchChoiceAdModel schChoiceAdModel)
		{
			schChoiceAdBiz.SetSchChoiceMenuDetailCreate_To(header, schChoiceAdModel);
			return schChoiceAdModel;
		}

		[WebMethod]
		public SchChoiceAdModel SetSchChoiceSearch(HeaderModel header, SchChoiceAdModel schChoiceAdModel)
		{
			schChoiceAdBiz.SetSchChoiceSearch(header, schChoiceAdModel);
			return schChoiceAdModel;
		}	
	
		[WebMethod]
		public SchChoiceAdModel SetSchChoiceChannelSearch(HeaderModel header, SchChoiceAdModel schChoiceAdModel)
		{
			schChoiceAdBiz.SetSchChoiceChannelSearch(header, schChoiceAdModel);
			return schChoiceAdModel;
		}	

		[WebMethod]
		public SchChoiceAdModel SetSchChoiceChannelDelete_To(HeaderModel header, SchChoiceAdModel schChoiceAdModel)
		{
			schChoiceAdBiz.SetSchChoiceChannelDelete_To(header, schChoiceAdModel);
			return schChoiceAdModel;
		}

		[WebMethod]
		public SchChoiceAdModel SetSchChoiceChannelDetailCreate_To(HeaderModel header, SchChoiceAdModel schChoiceAdModel)
		{
			schChoiceAdBiz.SetSchChoiceChannelDetailCreate_To(header, schChoiceAdModel);
			return schChoiceAdModel;
		}

		[WebMethod]
		public SchChoiceAdModel GetSchChoiceLastItemNoDelete(HeaderModel header, SchChoiceAdModel schChoiceAdModel)
		{
			schChoiceAdBiz.GetSchChoiceLastItemNoDelete(header, schChoiceAdModel);
			return schChoiceAdModel;
		}

        /// <summary>
        /// 기편성 내역 삭제 및 편성내역 복사하기 위해 추가.
        /// </summary>
        /// <param name="header"></param>
        /// <param name="schChoiceAdModel"></param>
        /// <returns></returns>
        [WebMethod]
        public SchChoiceAdModel SetSchChoiceAdCopy(HeaderModel header, SchChoiceAdModel schChoiceAdModel)
        {
            schChoiceAdBiz.SetSchChoiceAdCopy(header, schChoiceAdModel);
            return schChoiceAdModel;
        }


        /// <summary>
        /// 광고의 편성상태를 확인하기 위해 추가.
        /// </summary>
        /// <param name="header"></param>
        /// <param name="schChoiceAdModel"></param>
        /// <returns></returns>
        [WebMethod]
        public SchChoiceAdModel CheckSchChoice(HeaderModel header, SchChoiceAdModel schChoiceAdModel)
        {
            schChoiceAdBiz.CheckSchChoice(header, schChoiceAdModel);
            return schChoiceAdModel;
        }
	}
}
