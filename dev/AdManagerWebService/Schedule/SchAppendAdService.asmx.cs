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
	/// SchAppendAdService에 대한 요약 설명입니다.
	/// </summary>
	public class SchAppendAdService : System.Web.Services.WebService
	{

		private SchAppendAdBiz schAppendAdBiz = null;

		public SchAppendAdService()
		{
			//CODEGEN: 이 호출은 ASP.NET 웹 서비스 디자이너에 필요합니다.
			InitializeComponent();

			schAppendAdBiz = new SchAppendAdBiz();
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
		public SchAppendAdModel GetSchAppendAdList(HeaderModel header, SchAppendAdModel schAppendAdModel)
		{
			schAppendAdBiz.GetSchAppendAdList(header, schAppendAdModel);
			return schAppendAdModel;
		}

		[WebMethod]
		public SchAppendAdModel GetContractItemList(HeaderModel header, SchAppendAdModel schAppendAdModel)
		{
			schAppendAdBiz.GetContractItemList(header, schAppendAdModel);
			return schAppendAdModel;
		}

		[WebMethod]
		public SchAppendAdModel SetSchAppendSearch(HeaderModel header, SchAppendAdModel schAppendAdModel)
		{
			schAppendAdBiz.SetSchAppendSearch(header, schAppendAdModel);
			return schAppendAdModel;
		}

		[WebMethod]
		public SchAppendAdModel SetSchAppendAdCreate(HeaderModel header, SchAppendAdModel schAppendAdModel)
		{
			schAppendAdBiz.SetSchAppendAdCreate(header, schAppendAdModel);
			return schAppendAdModel;
		}

		[WebMethod]
		public SchAppendAdModel SetSchAppendAdDelete_To(HeaderModel header, SchAppendAdModel schAppendAdModel)
		{
			schAppendAdBiz.SetSchAppendAdDelete_To(header, schAppendAdModel);
			return schAppendAdModel;
		}

		[WebMethod]
		public SchAppendAdModel SetSchAppendAdCreate_To(HeaderModel header, SchAppendAdModel schAppendAdModel)
		{
			schAppendAdBiz.SetSchAppendAdCreate_To(header, schAppendAdModel);
			return schAppendAdModel;
		}
		
		[WebMethod]
		public SchAppendAdModel SetSchAppendAdDelete(HeaderModel header, SchAppendAdModel schAppendAdModel)
		{
			schAppendAdBiz.SetSchAppendAdDelete(header, schAppendAdModel);
			return schAppendAdModel;
		}

		[WebMethod]
		public SchAppendAdModel SetSchAppendAdOrderFirst(HeaderModel header, SchAppendAdModel schAppendAdModel)
		{
			schAppendAdBiz.SetSchAppendAdOrderFirst(header, schAppendAdModel);
			return schAppendAdModel;
		}

		[WebMethod]
		public SchAppendAdModel SetSchAppendAdOrderUp(HeaderModel header, SchAppendAdModel schAppendAdModel)
		{
			schAppendAdBiz.SetSchAppendAdOrderUp(header, schAppendAdModel);
			return schAppendAdModel;
		}

		[WebMethod]
		public SchAppendAdModel SetSchAppendAdOrderDown(HeaderModel header, SchAppendAdModel schAppendAdModel)
		{
			schAppendAdBiz.SetSchAppendAdOrderDown(header, schAppendAdModel);
			return schAppendAdModel;
		}

		[WebMethod]
		public SchAppendAdModel SetSchAppendAdOrderLast(HeaderModel header, SchAppendAdModel schAppendAdModel)
		{
			schAppendAdBiz.SetSchAppendAdOrderLast(header, schAppendAdModel);
			return schAppendAdModel;
		}

	}
}
