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
    [WebService(Namespace = "http://advmgt.hanafostv.com/AdManagerWebService/")]
    [System.ComponentModel.ToolboxItem(false)]

	/// <summary>
    /// ContractService�� ���� ��� �����Դϴ�.
    /// </summary>
    public class ContractService : System.Web.Services.WebService
    {

        private ContractBiz contractBiz = null;

        public ContractService()
        {
            //CODEGEN: �� ȣ���� ASP.NET �� ���� �����̳ʿ� �ʿ��մϴ�.
            InitializeComponent();

            contractBiz = new ContractBiz();
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
        public ContractModel GetContractList(HeaderModel header, ContractModel contractModel)
        {
            contractBiz.GetContractList(header, contractModel);
            return contractModel;
        } 

		[WebMethod]
		public ContractModel GetContractPackageList(HeaderModel header, ContractModel contractModel)
		{
			contractBiz.GetContractPackageList(header, contractModel);
			return contractModel;
		} 

		[WebMethod]
		public ContractModel GetLevel1List(HeaderModel header, ContractModel contractModel)
		{
			contractBiz.GetLevel1List(header, contractModel);
			return contractModel;
		} 
		
		[WebMethod]
		public ContractModel GetJobList(HeaderModel header, ContractModel contractModel)
		{
			contractBiz.GetJobList(header, contractModel);
			return contractModel;
		} 

		[WebMethod]
		public ContractModel GetLevel3List(HeaderModel header, ContractModel contractModel)
		{
			contractBiz.GetLevel3List(header, contractModel);
			return contractModel;
		} 

		[WebMethod]
		public ContractModel GetContractList2(HeaderModel header, ContractModel contractModel)
		{
			contractBiz.GetContractList2(header, contractModel);
			return contractModel;
		} 

		[WebMethod]
		public ContractModel GetContractItemList(HeaderModel header, ContractModel contractModel)
		{
			contractBiz.GetContractItemList(header, contractModel);
			return contractModel;
		} 

        [WebMethod]
        public ContractModel SetContractUpdate(HeaderModel header, ContractModel contractModel)
        {
            contractBiz.SetContractUpdate(header, contractModel);
            return contractModel;
        }		

        [WebMethod]
        public ContractModel SetContractCreate(HeaderModel header, ContractModel contractModel)
        {
            contractBiz.SetContractCreate(header, contractModel);
            return contractModel;
        }

        [WebMethod]
        public ContractModel SetContractDelete(HeaderModel header, ContractModel contractModel)
        {
            contractBiz.SetContractDelete(header, contractModel);
            return contractModel;
        }
    }
}
