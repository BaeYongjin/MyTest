using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Web;
using System.Web.Services;

using WinFramework.Base;

using AdManagerModel;

namespace AdManagerWebService.Contract
{

	/// <summary>
	/// 기본네임스페이스를 설정한다.
	/// </summary>
	[WebService(Namespace="http://advmgt.hanafostv.com/AdManagerWebService/")]
    [System.ComponentModel.ToolboxItem(false)]

	/// <summary>
	/// AgencyService에 대한 요약 설명입니다.
	/// </summary>
	public class AgencyService : System.Web.Services.WebService
	{

		private AgencyBiz agencyBiz = null;

		public AgencyService()
		{
			//CODEGEN: 이 호출은 ASP.NET 웹 서비스 디자이너에 필요합니다.
			InitializeComponent();

			agencyBiz = new AgencyBiz();
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
		public AgencyModel GetAgencyList(HeaderModel header, AgencyModel agencyModel)
		{
			agencyBiz.GetAgencyList(header, agencyModel);
			return agencyModel;
		}

		[WebMethod]
		public AgencyModel SetAgencyUpdate(HeaderModel header, AgencyModel agencyModel)
		{
			agencyBiz.SetAgencyUpdate(header, agencyModel);
			return agencyModel;
		}

		[WebMethod]
		public AgencyModel SetAgencyCreate(HeaderModel header, AgencyModel agencyModel)
		{
			agencyBiz.SetAgencyCreate(header, agencyModel);
			return agencyModel;
		}

		[WebMethod]
		public AgencyModel SetAgencyDelete(HeaderModel header, AgencyModel agencyModel)
		{
			agencyBiz.SetAgencyDelete(header, agencyModel);
			return agencyModel;
		}
	}
}
