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
    [WebService(Namespace = "http://advmgt.hanafostv.com/AdManagerWebService/")]
    [System.ComponentModel.ToolboxItem(false)]

	/// <summary>
    /// ContractService에 대한 요약 설명입니다.
    /// </summary>
    public class ContractService : System.Web.Services.WebService
    {

        private ContractBiz contractBiz = null;

        public ContractService()
        {
            //CODEGEN: 이 호출은 ASP.NET 웹 서비스 디자이너에 필요합니다.
            InitializeComponent();

            contractBiz = new ContractBiz();
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
