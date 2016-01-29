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
    public class SchHomeAdService : System.Web.Services.WebService
    {

        private SchHomeAdBiz schHomeAdBiz = null;

        public SchHomeAdService()
        {
            //CODEGEN: 이 호출은 ASP.NET 웹 서비스 디자이너에 필요합니다.
            InitializeComponent();

            schHomeAdBiz = new SchHomeAdBiz();
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
        public SchHomeAdModel GetSchHomeAdList(HeaderModel header, SchHomeAdModel schHomeAdModel)
        {
            schHomeAdBiz.GetSchHomeAdList(header, schHomeAdModel);
            return schHomeAdModel;
        }

        [WebMethod]
        public SchHomeAdModel GetSchHomeAdList2(HeaderModel header, SchHomeAdModel schHomeAdModel)
        {
            schHomeAdBiz.GetSchHomeAdList2(header, schHomeAdModel);
            return schHomeAdModel;
        }

        [WebMethod]
        public SchHomeAdModel GetContractItemList(HeaderModel header, SchHomeAdModel schHomeAdModel)
        {
            schHomeAdBiz.GetContractItemList(header, schHomeAdModel);
            return schHomeAdModel;
        }

        [WebMethod]
        public SchHomeAdModel GetContractItemListCm(HeaderModel header, SchHomeAdModel schHomeAdModel)
        {
            schHomeAdBiz.GetContractItemListCm(header, schHomeAdModel);
            return schHomeAdModel;
        }

        [WebMethod]
        public SchHomeAdModel SetSchHomeSearch(HeaderModel header, SchHomeAdModel schHomeAdModel)
        {
            schHomeAdBiz.SetSchHomeSearch(header, schHomeAdModel);
            return schHomeAdModel;
        }

        [WebMethod]
        public SchHomeAdModel SetSchHomeAdCreate(HeaderModel header, SchHomeAdModel schHomeAdModel)
        {
            schHomeAdBiz.SetSchHomeAdCreate(header, schHomeAdModel);
            return schHomeAdModel;
        }

        [WebMethod]
        public SchHomeAdModel SetSchHomeAdDelete_To(HeaderModel header, SchHomeAdModel schHomeAdModel)
        {
            schHomeAdBiz.SetSchHomeAdDelete_To(header, schHomeAdModel);
            return schHomeAdModel;
        }

        [WebMethod]
        public SchHomeAdModel SetSchHomeAdCreate_To(HeaderModel header, SchHomeAdModel schHomeAdModel)
        {
            schHomeAdBiz.SetSchHomeAdCreate_To(header, schHomeAdModel);
            return schHomeAdModel;
        }

        [WebMethod]
        public SchHomeAdModel SetSchHomeAdDelete(HeaderModel header, SchHomeAdModel schHomeAdModel)
        {
            schHomeAdBiz.SetSchHomeAdDelete(header, schHomeAdModel);
            return schHomeAdModel;
        }

        [WebMethod]
        public SchHomeAdModel SetSchHomeAdOrderFirst(HeaderModel header, SchHomeAdModel schHomeAdModel)
        {
            schHomeAdBiz.SetSchHomeAdOrderFirst(header, schHomeAdModel);
            return schHomeAdModel;
        }

        [WebMethod]
        public SchHomeAdModel SetSchHomeAdOrderUp(HeaderModel header, SchHomeAdModel schHomeAdModel)
        {
            schHomeAdBiz.SetSchHomeAdOrderUp(header, schHomeAdModel);
            return schHomeAdModel;
        }

        [WebMethod]
        public SchHomeAdModel SetSchHomeAdOrderDown(HeaderModel header, SchHomeAdModel schHomeAdModel)
        {
            schHomeAdBiz.SetSchHomeAdOrderDown(header, schHomeAdModel);
            return schHomeAdModel;
        }

        [WebMethod]
        public SchHomeAdModel SetSchHomeAdOrderLast(HeaderModel header, SchHomeAdModel schHomeAdModel)
        {
            schHomeAdBiz.SetSchHomeAdOrderLast(header, schHomeAdModel);
            return schHomeAdModel;
        }

        [WebMethod]
        public SchHomeAdModel SetSchHomeAdLogYn(HeaderModel header, SchHomeAdModel schHomeAdModel)
        {
            schHomeAdBiz.SetSchHomeAdLogYn(header, schHomeAdModel);
            return schHomeAdModel;
        }

        [WebMethod]
        public SchHomeAdModel SetSchHomeAdCommonYn(HeaderModel header, SchHomeAdModel schHomeAdModel)
        {
            schHomeAdBiz.SetSchHomeAdCommonYn(header, schHomeAdModel);
            return schHomeAdModel;
        }

        [WebMethod]
        public SchHomeAdModel getSchHomeCmSlot(HeaderModel header, SchHomeAdModel schHomeAdModel)
        {
            schHomeAdBiz.getSchHomeCmSlot(header, schHomeAdModel);
            return schHomeAdModel;
        }

        [WebMethod]
        public SchHomeAdModel getLogCount(HeaderModel header, SchHomeAdModel schHomeAdModel)
        {
            schHomeAdBiz.getLogCount(header, schHomeAdModel);
            return schHomeAdModel;
        }

        [WebMethod]
        public SchHomeAdModel setSchHomePlayType(HeaderModel header, SchHomeAdModel schHomeAdModel)
        {
            schHomeAdBiz.SetSchHomePlayType(header, schHomeAdModel);
            return schHomeAdModel;
        }
    }
}
