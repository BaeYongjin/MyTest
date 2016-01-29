using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Web;
using System.Web.Services;

using WinFramework.Base;
using AdManagerModel;

namespace AdManagerWebService.Schedule
{
    /// <summary>
    /// 기본네임스페이스를 설정한다.
    /// </summary>
    [WebService(Namespace = "http://advmgt.hanafostv.com/AdManagerWebService/")]
    [System.ComponentModel.ToolboxItem(false)]
    /// <summary>
    /// SchHomeAdService에 대한 요약 설명입니다.
    /// </summary>
    public class SchTargetHomeAdService : System.Web.Services.WebService
    {

        private SchTargetHomeAdBiz schTargetHomeAdBiz = null;

        public SchTargetHomeAdService()
        {
            //CODEGEN: 이 호출은 ASP.NET 웹 서비스 디자이너에 필요합니다.
            InitializeComponent();

            schTargetHomeAdBiz = new SchTargetHomeAdBiz();
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
        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null)
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #endregion

        [WebMethod]
        public SchTargetHomeAdModel GetSchTargetHomeAdList(HeaderModel header, SchTargetHomeAdModel schTargetHomeAdModel)
        {
            schTargetHomeAdBiz.GetSchTargetHomeAdList(header, schTargetHomeAdModel);
            return schTargetHomeAdModel;
        }

        [WebMethod]
        public SchTargetHomeAdModel SetSchTargetHomeAdCreate(HeaderModel header, SchTargetHomeAdModel schTargetHomeAdModel)
        {
            schTargetHomeAdBiz.SetSchTargetHomeAdCreate(header, schTargetHomeAdModel);
            return schTargetHomeAdModel;
        }

        [WebMethod]
        public SchTargetHomeAdModel SetSchTargetHomeAdUpdate(HeaderModel header, SchTargetHomeAdModel schTargetHomeAdModel)
        {
            schTargetHomeAdBiz.SetSchTargetHomeAdUpdate(header, schTargetHomeAdModel);
            return schTargetHomeAdModel;
        }

        [WebMethod]
        public SchTargetHomeAdModel SetSchTargetHomeAdDelete(HeaderModel header, SchTargetHomeAdModel schTargetHomeAdModel)
        {
            schTargetHomeAdBiz.SetSchTargetHomeAdDelete(header, schTargetHomeAdModel);
            return schTargetHomeAdModel;
        }


        [WebMethod]
        public SchTargetHomeAdModel SetSchTargetHomeAdOrderFirst(HeaderModel header, SchTargetHomeAdModel schTargetHomeAdModel)
        {
            schTargetHomeAdBiz.SetSchTargetHomeAdOrderFirst(header, schTargetHomeAdModel);
            return schTargetHomeAdModel;
        }

        [WebMethod]
        public SchTargetHomeAdModel SetSchTargetHomeAdOrderUp(HeaderModel header, SchTargetHomeAdModel schTargetHomeAdModel)
        {
            schTargetHomeAdBiz.SetSchTargetHomeAdOrderUp(header, schTargetHomeAdModel);
            return schTargetHomeAdModel;
        }

        [WebMethod]
        public SchTargetHomeAdModel SetSchTargetHomeAdOrderDown(HeaderModel header, SchTargetHomeAdModel schTargetHomeAdModel)
        {
            schTargetHomeAdBiz.SetSchTargetHomeAdOrderDown(header, schTargetHomeAdModel);
            return schTargetHomeAdModel;
        }

        [WebMethod]
        public SchTargetHomeAdModel SetSchTargetHomeAdOrderLast(HeaderModel header, SchTargetHomeAdModel schTargetHomeAdModel)
        {
            schTargetHomeAdBiz.SetSchTargetHomeAdOrderLast(header, schTargetHomeAdModel);
            return schTargetHomeAdModel;
        }





        [WebMethod]
        public SchTargetHomeAdModel GetSchHomeAdList(HeaderModel header, SchTargetHomeAdModel schTargetHomeAdModel)
        {
            schTargetHomeAdBiz.GetSchHomeAdList(header, schTargetHomeAdModel);
            return schTargetHomeAdModel;
        }

        [WebMethod]
        public SchTargetHomeAdModel GetContractItemList(HeaderModel header, SchTargetHomeAdModel schTargetHomeAdModel)
        {
            schTargetHomeAdBiz.GetContractItemList(header, schTargetHomeAdModel);
            return schTargetHomeAdModel;
        }

        [WebMethod]
        public SchTargetHomeAdModel GetContractItemListCm(HeaderModel header, SchTargetHomeAdModel schTargetHomeAdModel)
        {
            schTargetHomeAdBiz.GetContractItemListCm(header, schTargetHomeAdModel);
            return schTargetHomeAdModel;
        }

        [WebMethod]
        public SchTargetHomeAdModel SetSchHomeSearch(HeaderModel header, SchTargetHomeAdModel schTargetHomeAdModel)
        {
            schTargetHomeAdBiz.SetSchHomeSearch(header, schTargetHomeAdModel);
            return schTargetHomeAdModel;
        }

        [WebMethod]
        public SchTargetHomeAdModel SetSchHomeAdCreate(HeaderModel header, SchTargetHomeAdModel schTargetHomeAdModel)
        {
            schTargetHomeAdBiz.SetSchHomeAdCreate(header, schTargetHomeAdModel);
            return schTargetHomeAdModel;
        }

        [WebMethod]
        public SchTargetHomeAdModel SetSchHomeAdDelete_To(HeaderModel header, SchTargetHomeAdModel schTargetHomeAdModel)
        {
            schTargetHomeAdBiz.SetSchHomeAdDelete_To(header, schTargetHomeAdModel);
            return schTargetHomeAdModel;
        }

        [WebMethod]
        public SchTargetHomeAdModel SetSchHomeAdCreate_To(HeaderModel header, SchTargetHomeAdModel schTargetHomeAdModel)
        {
            schTargetHomeAdBiz.SetSchHomeAdCreate_To(header, schTargetHomeAdModel);
            return schTargetHomeAdModel;
        }

        [WebMethod]
        public SchTargetHomeAdModel SetSchHomeAdDelete(HeaderModel header, SchTargetHomeAdModel schTargetHomeAdModel)
        {
            schTargetHomeAdBiz.SetSchHomeAdDelete(header, schTargetHomeAdModel);
            return schTargetHomeAdModel;
        }

        [WebMethod]
        public SchTargetHomeAdModel SetSchHomeAdOrderFirst(HeaderModel header, SchTargetHomeAdModel schTargetHomeAdModel)
        {
            schTargetHomeAdBiz.SetSchHomeAdOrderFirst(header, schTargetHomeAdModel);
            return schTargetHomeAdModel;
        }

        [WebMethod]
        public SchTargetHomeAdModel SetSchHomeAdOrderUp(HeaderModel header, SchTargetHomeAdModel schTargetHomeAdModel)
        {
            schTargetHomeAdBiz.SetSchHomeAdOrderUp(header, schTargetHomeAdModel);
            return schTargetHomeAdModel;
        }

        [WebMethod]
        public SchTargetHomeAdModel SetSchHomeAdOrderDown(HeaderModel header, SchTargetHomeAdModel schTargetHomeAdModel)
        {
            schTargetHomeAdBiz.SetSchHomeAdOrderDown(header, schTargetHomeAdModel);
            return schTargetHomeAdModel;
        }

        [WebMethod]
        public SchTargetHomeAdModel SetSchHomeAdOrderLast(HeaderModel header, SchTargetHomeAdModel schTargetHomeAdModel)
        {
            schTargetHomeAdBiz.SetSchHomeAdOrderLast(header, schTargetHomeAdModel);
            return schTargetHomeAdModel;
        }

        [WebMethod]
        public SchTargetHomeAdModel SetSchHomeAdLogYn(HeaderModel header, SchTargetHomeAdModel schTargetHomeAdModel)
        {
            schTargetHomeAdBiz.SetSchHomeAdLogYn(header, schTargetHomeAdModel);
            return schTargetHomeAdModel;
        }

        [WebMethod]
        public SchTargetHomeAdModel SetSchHomeAdCommonYn(HeaderModel header, SchTargetHomeAdModel schTargetHomeAdModel)
        {
            schTargetHomeAdBiz.SetSchHomeAdCommonYn(header, schTargetHomeAdModel);
            return schTargetHomeAdModel;
        }

        [WebMethod]
        public SchTargetHomeAdModel getSchHomeCmSlot(HeaderModel header, SchTargetHomeAdModel schTargetHomeAdModel)
        {
            schTargetHomeAdBiz.getSchHomeCmSlot(header, schTargetHomeAdModel);
            return schTargetHomeAdModel;
        }

        [WebMethod]
        public SchTargetHomeAdModel getLogCount(HeaderModel header, SchTargetHomeAdModel schTargetHomeAdModel)
        {
            schTargetHomeAdBiz.getLogCount(header, schTargetHomeAdModel);
            return schTargetHomeAdModel;
        }

        [WebMethod]
        public SchTargetHomeAdModel getSlotCount(HeaderModel header, SchTargetHomeAdModel schTargetHomeAdModel)
        {
            schTargetHomeAdBiz.GetSlotCount(header, schTargetHomeAdModel);
            return schTargetHomeAdModel;
        }

        [WebMethod]
        public SchTargetHomeAdModel setSchTargetHomePlayType(HeaderModel header, SchTargetHomeAdModel schTargetHomeAdModel)
        {
            schTargetHomeAdBiz.SetSchTargetHomePlayType(header, schTargetHomeAdModel);
            return schTargetHomeAdModel;
        }
    }
}
