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
	/// �⺻���ӽ����̽��� �����Ѵ�.
	/// </summary>
	[WebService(Namespace="http://advmgt.hanafostv.com/AdManagerWebService/")]
    [System.ComponentModel.ToolboxItem(false)]

	/// <summary>
	/// CampaignService�� ���� ��� �����Դϴ�.
	/// </summary>
	public class CampaignService : System.Web.Services.WebService
	{

		private CampaignBiz campaignBiz = null;

		public CampaignService()
		{
			//CODEGEN: �� ȣ���� ASP.NET �� ���� �����̳ʿ� �ʿ��մϴ�.
			InitializeComponent();

			campaignBiz = new CampaignBiz();
		}

		#region ���� ��� �����̳ʿ��� ������ �ڵ�
		
		//�� ���� �����̳ʿ� �ʿ��մϴ�. 
		private IContainer components = null;
				
		/// <summary>
		/// �����̳� ������ �ʿ��� �޼����Դϴ�.
		/// �� �޼����� ������ �ڵ� ������� �������� ���ʽÿ�.
		/// </summary>
		private void InitializeComponent()
		{
		}

		/// <summary>
		/// ��� ���� ��� ���ҽ��� �����մϴ�.
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
