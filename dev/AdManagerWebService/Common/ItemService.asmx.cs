using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Web;
using System.Web.Services;

using WinFramework.Base;
using AdManagerModel;

namespace AdManagerWebService.Common
{
    /// <summary>
    /// ItemService의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://advmgt.hanafostv.com/AdManagerWebService/")]
    [System.ComponentModel.ToolboxItem(false)]

    public class ItemService : System.Web.Services.WebService
    {
        private ItemBiz itemBiz = null;

        public ItemService()
		{
            itemBiz = new ItemBiz();
		}


        [WebMethod]
        public ItemModel GetContractItemList(HeaderModel header, ItemModel itemModel)
        {
            itemBiz.GetContractItemList(header, itemModel);
            return itemModel;
        }

        [WebMethod]
        public ItemModel GetContractItemList_0907a(HeaderModel header, ItemModel itemModel)
        {
            // 위에꺼 업그레이드 버젼임
            // 패치 완료후 위에 함수는 제거할것
            itemBiz.GetContractItemList_0907a(header, itemModel);
            return itemModel;
        }

        [WebMethod]
        public ItemModel GetContractItemListCm(HeaderModel header, ItemModel itemModel)
        {
            // 위에꺼 업그레이드 버젼임
            // 패치 완료후 위에 함수는 제거할것
            itemBiz.GetContractItemListCm(header, itemModel);
            return itemModel;
        }

        [WebMethod]
        public ItemModel GetContractItemListForCug(HeaderModel header, ItemModel itemModel)
        {
            itemBiz.GetContractItemListForCug(header, itemModel);
            return itemModel;
        }

        [WebMethod]
        public ItemModel GetContractItemListDual(HeaderModel header, ItemModel itemModel)
        {
            itemBiz.GetContractItemListDual(header, itemModel);
            return itemModel;
        }

        [WebMethod]
        public ItemModel GetSchAdItemList(HeaderModel header, ItemModel itemModel)
        {
            itemBiz.GetSchAdItemList(header, itemModel);
            return itemModel;
        }
		[WebMethod]
		public ItemModel GetContractKidsItemList(HeaderModel header, ItemModel itemModel)
		{
			itemBiz.GetContractKidsItemList(header, itemModel);
			return itemModel;
		}
    }
}
