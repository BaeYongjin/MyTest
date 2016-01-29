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
	/// SchHomeAdService에 대한 요약 설명입니다.
	/// </summary>
	public class SchHomeCmService : System.Web.Services.WebService
	{

		private SchHomeCmBiz schHomeCmBiz = null;

		public SchHomeCmService()
		{
			//CODEGEN: 이 호출은 ASP.NET 웹 서비스 디자이너에 필요합니다.
			InitializeComponent();

			schHomeCmBiz = new SchHomeCmBiz();
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
		public SchHomeCmModel GetSchHomeCmList(HeaderModel header, SchHomeCmModel data)
		{
			schHomeCmBiz.GetSchHomeCMList(header, data);
			return data;
		}

		[WebMethod]
		public SchHomeCmModel GetSchHomeCmDetail(HeaderModel header, SchHomeCmModel data)
		{
			schHomeCmBiz.GetSchHomeCMDetail(header, data);
			return data;
		}

		[WebMethod]
		public SchHomeCmModel SetSchHomeCmCreate(HeaderModel header, SchHomeCmModel data)
		{
			schHomeCmBiz.SetSchHomeCmCreate(header, data);
			return data;
		}

		[WebMethod]
		public SchHomeCmModel SetSchHomeCmUpdate(HeaderModel header, SchHomeCmModel data)
		{
			schHomeCmBiz.SetSchHomeCmUpdate(header, data);
			return data;
		}

		[WebMethod]
		public SchHomeCmModel SetSchHomeCmDelete(HeaderModel header, SchHomeCmModel data)
		{
			schHomeCmBiz.SetSchHomeCmDelete(header, data);
			return data;
		}

        [WebMethod]
        public SchHomeCmModel GetSlotState(HeaderModel header, SchHomeCmModel data)
        {
            schHomeCmBiz.GetSlotState(header, data);
            return data;
        }
        [WebMethod]
        public SchHomeCmModel GetSlotCount(HeaderModel header, SchHomeCmModel data)
        {
            schHomeCmBiz.GetSlotCount(header, data);
            return data;
        }
        [WebMethod]
        public SchHomeCmModel GetSlotAd(HeaderModel header, SchHomeCmModel data)
        {
            schHomeCmBiz.GetSlotAd(header, data);
            return data;
        }
	}
}
