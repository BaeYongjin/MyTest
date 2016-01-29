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
	/// ClientService에 대한 요약 설명입니다.
	/// </summary>
	public class ClientService : System.Web.Services.WebService
	{
		private ClientBiz clientBiz = null;
		public ClientService()
		{
			//CODEGEN: 이 호출은 ASP.NET 웹 서비스 디자이너에 필요합니다.
			InitializeComponent();
			clientBiz = new ClientBiz();
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
        public ClientModel GetAdvertisertList(HeaderModel header, ClientModel clientModel)
        {
            clientBiz.GetAdvertiserList(header, clientModel);
            return clientModel;
        }
        [WebMethod]
        public ClientModel GetClientAdvertiserListByCombo(HeaderModel header, ClientModel clientModel)
        {
            clientBiz.GetClientAdvertiserListByCombo(header, clientModel);
            return clientModel;
        }
        

		[WebMethod]
		public ClientModel GetClientMediaList(HeaderModel header, ClientModel clientModel)
		{
			clientBiz.GetClientMediaList(header, clientModel);
			return clientModel;
		}

		[WebMethod]
		public ClientModel GetClientRapList(HeaderModel header, ClientModel clientModel)
		{
			clientBiz.GetClientRapList(header, clientModel);
			return clientModel;
		}

		[WebMethod]
		public ClientModel GetClientAgencyList(HeaderModel header, ClientModel clientModel)
		{
			clientBiz.GetClientAgencyList(header, clientModel);
			return clientModel;
		}

		[WebMethod]
		public ClientModel GetClientAdvertiserList(HeaderModel header, ClientModel clientModel)
		{
			clientBiz.GetClientAdvertiserList(header, clientModel);
			return clientModel;
		}

		[WebMethod]
		public ClientModel GetClientList(HeaderModel header, ClientModel clientModel)
		{
			clientBiz.GetClientList(header, clientModel);
			return clientModel;
		}

		[WebMethod]
		public ClientModel SetClientUpdate(HeaderModel header, ClientModel clientModel)
		{
			clientBiz.SetClientUpdate(header, clientModel);
			return clientModel;
		}

		[WebMethod]
		public ClientModel SetClientCreate(HeaderModel header, ClientModel clientModel)
		{
			clientBiz.SetClientCreate(header, clientModel);
			return clientModel;
		}

		[WebMethod]
		public ClientModel SetClientDelete(HeaderModel header, ClientModel clientModel)
		{
			clientBiz.SetClientDelete(header, clientModel);
			return clientModel;
		}
	}
}