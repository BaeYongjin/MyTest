using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Web;
using System.Web.Services;

using WinFramework.Base;
using AdManagerModel;

namespace AdManagerWebService.ContractItem
{
 
	/// <summary>
	/// �⺻���ӽ����̽��� �����Ѵ�.
	/// </summary>
	[WebService(Namespace="http://advmgt.hanafostv.com/AdManagerWebService/")]
    [System.ComponentModel.ToolboxItem(false)]

	/// <summary>
    /// ContractItemService�� ���� ��� �����Դϴ�.
    /// </summary>
    public class ContractItemService : System.Web.Services.WebService
    {

        private ContractItemBiz contractItemBiz = null;

        public ContractItemService()
        {
            //CODEGEN: �� ȣ���� ASP.NET �� ���� �����̳ʿ� �ʿ��մϴ�.
            InitializeComponent();

            contractItemBiz = new ContractItemBiz();
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
		public ContractItemModel GetGradeCodeList(HeaderModel header, ContractItemModel contractItemModel)
		{
			contractItemBiz.GetGradeCodeList(header, contractItemModel);
			return contractItemModel;
		} 

        [WebMethod]
        public ContractItemModel GetContractItemList(HeaderModel header, ContractItemModel contractItemModel)
        {
            contractItemBiz.GetContractItemList(header, contractItemModel);
            return contractItemModel;
        } 
        [WebMethod]
        public ContractItemModel GetContractItemDetail(HeaderModel header, ContractItemModel contractItemModel)
        {
            contractItemBiz.GetContractItemDetail(header, contractItemModel);
            return contractItemModel;
        }
        [WebMethod]
        public ContractItemModel GetLinkChannel(HeaderModel header, ContractItemModel contractItemModel)
        {
            contractItemBiz.GetLinkChannel(header, contractItemModel);
            return contractItemModel;
        }
        [WebMethod]
        public ContractItemModel GetLinkChannel2(HeaderModel header, ContractItemModel contractItemModel)
        {
            contractItemBiz.GetLinkChannel2(header, contractItemModel);
            return contractItemModel;
        }
        [WebMethod]
        public ContractItemModel GetLinkItem(HeaderModel header, ContractItemModel contractItemModel)
        {
            contractItemBiz.GetLinkItem(header, contractItemModel);
            return contractItemModel;
        } 
        [WebMethod]
        public ContractItemModel GetContractItemHIstoryList(HeaderModel header, ContractItemModel contractItemModel)
        {
            contractItemBiz.GetContractItemHIstoryList(header, contractItemModel);
            return contractItemModel;
        } 
		[WebMethod]
		public ContractItemModel GetFileList(HeaderModel header, ContractItemModel contractItemModel)
		{
			contractItemBiz.GetFileList(header, contractItemModel);
			return contractItemModel;
		}
        [WebMethod]
        public ContractItemModel GetChannelList(HeaderModel header, ContractItemModel contractItemModel)
        {
            contractItemBiz.GetChannelList(header, contractItemModel);
            return contractItemModel;
        }
        [WebMethod]
        public ContractItemModel GetContentsList(HeaderModel header, ContractItemModel contractItemModel)
        {
            contractItemBiz.GetContentsList(header, contractItemModel);
            return contractItemModel;
        }
		[WebMethod]
		public ContractItemModel wGetFileInfo(HeaderModel header, ContractItemModel contractItemModel)
		{
			contractItemBiz.GetFileInfo(header, contractItemModel);
			return contractItemModel;
		}

		[WebMethod]
		public ContractItemModel SetFileCreate(HeaderModel header, ContractItemModel contractItemModel)
		{
			contractItemBiz.SetFileCreate(header, contractItemModel);
			return contractItemModel;
		} 

		[WebMethod]
		public ContractItemModel SetFileUpdate(HeaderModel header, ContractItemModel contractItemModel)
		{
			contractItemBiz.SetFileUpdate(header, contractItemModel);
			return contractItemModel;
		} 

		[WebMethod]
		public ContractItemModel SetFileDelete(HeaderModel header, ContractItemModel contractItemModel)
		{
			contractItemBiz.SetFileDelete(header, contractItemModel);
			return contractItemModel;
		} 

		[WebMethod]
		public ContractItemModel SetLinkChannelCreate(HeaderModel header, ContractItemModel contractItemModel)
		{
			contractItemBiz.SetLinkChannelCreate(header, contractItemModel);
			return contractItemModel;
		}

        [WebMethod]
        public ContractItemModel SetLinkChannelCreate2(HeaderModel header, ContractItemModel contractItemModel)
        {
            contractItemBiz.SetLinkChannelCreate2(header, contractItemModel);
            return contractItemModel;
        }
        [WebMethod]
        public ContractItemModel SetContractItemUpdate(HeaderModel header, ContractItemModel contractItemModel)
        {
            contractItemBiz.SetContractItemUpdate(header, contractItemModel);
            return contractItemModel;
        }

        [WebMethod]
        public ContractItemModel SetContractItemCreate(HeaderModel header, ContractItemModel contractItemModel)
        {
            contractItemBiz.SetContractItemCreate(header, contractItemModel);
            return contractItemModel;
        }

		[WebMethod]
		public ContractItemModel SetLinkChannelDelete(HeaderModel header, ContractItemModel contractItemModel)
		{
			contractItemBiz.SetLinkChannelDelete(header, contractItemModel);
			return contractItemModel;
		}

        [WebMethod]
        public ContractItemModel SetLinkChannelDelete2(HeaderModel header, ContractItemModel contractItemModel)
        {
            contractItemBiz.SetLinkChannelDelete2(header, contractItemModel);
            return contractItemModel;
        }

        [WebMethod]
        public ContractItemModel SetContractItemDelete(HeaderModel header, ContractItemModel contractItemModel)
        {
            contractItemBiz.SetContractItemDelete(header, contractItemModel);
            return contractItemModel;
        }

		[WebMethod]
		public ItemCopyModel SetContractItemCopy(HeaderModel header, ItemCopyModel	data)
		{
			contractItemBiz.SetContractItemCopy(header, data);
			return data;
		}


        // �ټ��� ������¸� �ϰ������� ���� .bae
		[WebMethod]
		public ContractItemModel SetMultiAdState(HeaderModel header, ContractItemModel contractItemModel)
		{
			contractItemBiz.SetMultiAdState(header, contractItemModel);
			return contractItemModel;
		}

		[WebMethod]
		public ContractItemModel SetMultiChannel(HeaderModel header, ContractItemModel contractItemModel)
		{
			return contractItemModel;
		}

        [WebMethod]
        public ContractItemModel SetLinkItemCreate(HeaderModel header, ContractItemModel contractItemModel)
        {
            contractItemBiz.SetLinkItemCreate(header, contractItemModel);
            return contractItemModel;
        }

        [WebMethod]
        public ContractItemModel SetLinkItemDelete(HeaderModel header, ContractItemModel contractItemModel)
        {
            contractItemBiz.SetLinkItemDelete(header, contractItemModel);
            return contractItemModel;
        }
    }
}
