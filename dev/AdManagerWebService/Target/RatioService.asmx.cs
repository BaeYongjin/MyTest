using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Web;
using System.Web.Services;

using WinFramework.Base;
using AdManagerModel;

namespace AdManagerWebService.Target
{
    /// <summary>
    /// 기본네임스페이스를 설정한다.
    /// </summary>
    [WebService(Namespace = "http://advmgt.hanafostv.com/AdManagerWebService/")]
    [System.ComponentModel.ToolboxItem(false)]

	/// <summary>
	/// RatioService에 대한 요약 설명입니다.
	/// </summary>
	public class RatioService : System.Web.Services.WebService
	{

		private RatioBiz ratioBiz = null;

		public RatioService()
		{
			//CODEGEN: 이 호출은 ASP.NET 웹 서비스 디자이너에 필요합니다.
			InitializeComponent();

			ratioBiz = new RatioBiz();
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
		public RatioModel GetSchChoiceMenuDetailList(HeaderModel header, RatioModel ratioModel)
		{
			ratioBiz.GetSchChoiceMenuDetailList(header, ratioModel);
			return ratioModel;
		}

		[WebMethod]
		public RatioModel GetSchRateList(HeaderModel header, RatioModel ratioModel)
		{
			ratioBiz.GetSchRateList(header, ratioModel);
			return ratioModel;
		}

		[WebMethod]
		public RatioModel GetSchRateDetailList(HeaderModel header, RatioModel ratioModel)
		{
			ratioBiz.GetSchRateDetailList(header, ratioModel);
			return ratioModel;
		}

		[WebMethod]
		public RatioModel GetGroup1List(HeaderModel header, RatioModel ratioModel)
		{
			ratioBiz.GetGroup1List(header, ratioModel);
			return ratioModel;
		}

		[WebMethod]
		public RatioModel GetGroup2List(HeaderModel header, RatioModel ratioModel)
		{
			ratioBiz.GetGroup2List(header, ratioModel);
			return ratioModel;
		}

		[WebMethod]
		public RatioModel GetGroup3List(HeaderModel header, RatioModel ratioModel)
		{
			ratioBiz.GetGroup3List(header, ratioModel);
			return ratioModel;
		}

		[WebMethod]
		public RatioModel GetGroup4List(HeaderModel header, RatioModel ratioModel)
		{
			ratioBiz.GetGroup4List(header, ratioModel);
			return ratioModel;
		}

		[WebMethod]
		public RatioModel GetGroup5List(HeaderModel header, RatioModel ratioModel)
		{
			ratioBiz.GetGroup5List(header, ratioModel);
			return ratioModel;
		}	

		[WebMethod]
		public RatioModel SetSchRateUpdate(HeaderModel header, RatioModel ratioModel)
		{
			ratioBiz.SetSchRateUpdate(header, ratioModel);
			return ratioModel;
		}

		[WebMethod]
		public RatioModel SetSchRateCreate(HeaderModel header, RatioModel ratioModel)
		{
			ratioBiz.SetSchRateCreate(header, ratioModel);
			return ratioModel;
		}

		[WebMethod]
		public RatioModel SetSchRateDetailCreate(HeaderModel header, RatioModel ratioModel)
		{
			ratioBiz.SetSchRateDetailCreate(header, ratioModel);
			return ratioModel;
		}

		[WebMethod]
		public RatioModel SetSchRateDelete(HeaderModel header, RatioModel ratioModel)
		{
			ratioBiz.SetSchRateDelete(header, ratioModel);
			return ratioModel;
		}
		
		[WebMethod]
		public RatioModel SetSchRateDetailDelete(HeaderModel header, RatioModel ratioModel)
		{
			ratioBiz.SetSchRateDetailDelete(header, ratioModel);
			return ratioModel;
		}

        [WebMethod]
        public RatioModel mDeleteSync(HeaderModel header, RatioModel ratioModel)
        {
            ratioBiz.DeleteSync(header, ratioModel);
            return ratioModel;
        }
	}
}
