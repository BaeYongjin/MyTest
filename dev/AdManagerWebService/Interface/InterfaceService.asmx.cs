using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Web;
using System.Web.Services;

namespace AdManagerWebService.Interface
{

	/// <summary>
	/// �⺻���ӽ����̽��� �����Ѵ�.
	/// </summary>
	[WebService(Namespace="http://advmgt.hanafostv.com/AdManagerWebService/")]

	/// <summary>
	/// InterfaceService�� ���� ��� �����Դϴ�.
	/// </summary>
	public class InterfaceService : System.Web.Services.WebService
	{
		public InterfaceService()
		{
			//CODEGEN: �� ȣ���� ASP.NET �� ���� �����̳ʿ� �ʿ��մϴ�.
			InitializeComponent();
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
		public string GetAdvertiserList(string SearchKey)
		{
			return new InterfaceBiz().GetAdvertiserList(SearchKey);
		}

		[WebMethod]
		public string GetContractItemList()
		{
			return new InterfaceBiz().GetContractItemList();
		}

		[WebMethod]
		public string GetContractList(string SearchKey)
		{
			return new InterfaceBiz().GetContractList(SearchKey);
		} 

		[WebMethod]
		public string SetFileCheckReady(int itemNo, string FileName)
		{
			return new InterfaceBiz().SetFileCheckReady(itemNo, FileName);
		} 

		[WebMethod]
		public string SetFileCDNSync(int itemNo, string SuccessYN, string contents_state)
		{
			return new InterfaceBiz().SetFileCDNSync(itemNo, SuccessYN, contents_state);
		} 

	}
}
