using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Web;
using System.Web.Services;

using WinFramework.Base;
using AdManagerModel;

namespace AdManagerWebService.ContractPackage
{
    /// <summary>
    /// �⺻���ӽ����̽��� �����Ѵ�.
    /// </summary>
    [WebService(Namespace = "http://advmgt.hanafostv.com/AdManagerWebService/")]
    [System.ComponentModel.ToolboxItem(false)]

	/// <summary>
	/// ContractPackageService�� ���� ��� �����Դϴ�.
	/// </summary>
	public class ContractPackageService : System.Web.Services.WebService
	{
		public ContractPackageService()
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
		public ContractPackageModel GetContractPackageList(HeaderModel header, ContractPackageModel model)
		{
			new ContractPackageBiz().GetContractPackageList(header, model);
			return model;
		} 

		[WebMethod]
		public ContractPackageModel SetContractPackageUpdate(HeaderModel header, ContractPackageModel model)
		{
			new ContractPackageBiz().SetContractPackageUpdate(header, model);
			return model;
		}

		[WebMethod]
		public ContractPackageModel SetContractPackageCreate(HeaderModel header, ContractPackageModel model)
		{
			new ContractPackageBiz().SetContractPackageCreate(header, model);
			return model;
		}

		[WebMethod]
		public ContractPackageModel SetContractPackageDelete(HeaderModel header, ContractPackageModel model)
		{
			new ContractPackageBiz().SetContractPackageDelete(header, model);
			return model;
		}
	}
}
