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
	/// CampaignService에 대한 요약 설명입니다.
	/// </summary>
	public class CampaignService : System.Web.Services.WebService
	{

		private CampaignBiz campaignBiz = null;

		public CampaignService()
		{
			//CODEGEN: 이 호출은 ASP.NET 웹 서비스 디자이너에 필요합니다.
			InitializeComponent();

			campaignBiz = new CampaignBiz();
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
		public CampaignModel GetContractList(HeaderModel header, CampaignModel campaignModel)
		{
			campaignBiz.GetContractList(header, campaignModel);
			return campaignModel;
		} 

		[WebMethod]
		public CampaignModel GetCampaignList(HeaderModel header, CampaignModel campaignModel)
		{
			campaignBiz.GetCampaignList(header, campaignModel);
			return campaignModel;
		} 

		[WebMethod]
		public CampaignModel GetContractList2(HeaderModel header, CampaignModel campaignModel)
		{
			campaignBiz.GetContractList2(header, campaignModel);
			return campaignModel;
		} 

		[WebMethod]
		public CampaignModel GetContractItemList(HeaderModel header, CampaignModel campaignModel)
		{
			campaignBiz.GetContractItemList(header, campaignModel);
			return campaignModel;
		} 

		[WebMethod]
		public CampaignModel GetContractItemPopList(HeaderModel header, CampaignModel campaignModel)
		{
			campaignBiz.GetContractItemPopList(header, campaignModel);
			return campaignModel;
		}

        [WebMethod]
        public CampaignModel svcGetPnsList(HeaderModel header, CampaignModel campaignModel)
        {
            campaignBiz.GetPnsList(header, campaignModel);
            return campaignModel;
        } 

		[WebMethod]
		public CampaignModel SetCampaignCreate(HeaderModel header, CampaignModel campaignModel)
		{
			campaignBiz.SetCampaignCreate(header, campaignModel);
			return campaignModel;
		}

		[WebMethod]
		public CampaignModel SetCampaignUpdate(HeaderModel header, CampaignModel campaignModel)
		{
			campaignBiz.SetCampaignUpdate(header, campaignModel);
			return campaignModel;
		}

		[WebMethod]
		public CampaignModel SetCampaignDelete(HeaderModel header, CampaignModel campaignModel)
		{
			campaignBiz.SetCampaignDelete(header, campaignModel);
			return campaignModel;
		}

		[WebMethod]
		public CampaignModel SetCampaignDetailCreate(HeaderModel header, CampaignModel campaignModel)
		{
			campaignBiz.SetCampaignDetailCreate(header, campaignModel);
			return campaignModel;
		}

		[WebMethod]
		public CampaignModel SetCampaignDetailDelete(HeaderModel header, CampaignModel campaignModel)
		{
			campaignBiz.SetCampaignDetailDelete(header, campaignModel);
			return campaignModel;
		}

        [WebMethod]
        public CampaignModel svcSetCampaignPnsCreate(HeaderModel header, CampaignModel campaignModel)
        {
            campaignBiz.SetCampaignPnsCreate(header, campaignModel);
            return campaignModel;
        }

        [WebMethod]
        public CampaignModel svcSetCampaignPnsDelete(HeaderModel header, CampaignModel campaignModel)
        {
            campaignBiz.SetCampaignPnsDelete(header, campaignModel);
            return campaignModel;
        }
	}
}
