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
	/// ClientService�� ���� ��� �����Դϴ�.
	/// </summary>
	public class ClientService : System.Web.Services.WebService
	{
		private ClientBiz clientBiz = null;
		public ClientService()
		{
			//CODEGEN: �� ȣ���� ASP.NET �� ���� �����̳ʿ� �ʿ��մϴ�.
			InitializeComponent();
			clientBiz = new ClientBiz();
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