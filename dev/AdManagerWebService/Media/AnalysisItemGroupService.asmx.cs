using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Web;
using System.Web.Services;

using WinFramework.Base;
using AdManagerModel;


namespace AdManagerWebService.Dev
{

    /// <summary>
    /// 기본네임스페이스를 설정한다.
    /// </summary>
    [WebService(Namespace = "http://advmgt.hanafostv.com/AdManagerWebService/")]
    [System.ComponentModel.ToolboxItem(false)]

    /// <summary>
    /// AnalysisItemGroupService에 대한 요약 설명입니다.
    /// </summary>
    public class AnalysisItemGroupService : System.Web.Services.WebService
    {
        private AnalysisItemGroupBiz AnalysisItemGroupBiz = null;
        public AnalysisItemGroupService()
        {
            //CODEGEN: 이 호출은 ASP.NET 웹 서비스 디자이너에 필요합니다.
            InitializeComponent();
            AnalysisItemGroupBiz = new AnalysisItemGroupBiz();
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
        public AnalysisItemGroupModel GetAdvertiserList(HeaderModel header, AnalysisItemGroupModel analysisItemGroupModel)
        {
            //AnalysisItemGroupBiz.GetAdvertiserList(header, analysisItemGroupModel);
            return analysisItemGroupModel;
        }

        [WebMethod]
        public AnalysisItemGroupModel GetContractItemList(HeaderModel header, AnalysisItemGroupModel analysisItemGroupModel)
        {
            AnalysisItemGroupBiz.GetContractItemList(header, analysisItemGroupModel);
            return analysisItemGroupModel;
        }

        [WebMethod]
        public AnalysisItemGroupModel GetAnalysisItemGroup(HeaderModel header, AnalysisItemGroupModel analysisItemGroupModel)
        {
            AnalysisItemGroupBiz.GetAnalysisItemGroup(header, analysisItemGroupModel);
            return analysisItemGroupModel;
        }

        [WebMethod]
        public AnalysisItemGroupModel GetAnalysisItemGroupDetail(HeaderModel header, AnalysisItemGroupModel analysisItemGroupModel)
        {
            AnalysisItemGroupBiz.GetAnalysisItemGroupDetail(header, analysisItemGroupModel);
            return analysisItemGroupModel;
        }

        [WebMethod]
        public AnalysisItemGroupModel GetAnalysisMonths(HeaderModel header, AnalysisItemGroupModel analysisItemGroupModel)
        {
            AnalysisItemGroupBiz.GetAnalysisMonths(header, analysisItemGroupModel);
            return analysisItemGroupModel;
        }

        [WebMethod]
        public AnalysisItemGroupModel SetAnalysisItemGroupCreate(HeaderModel header, AnalysisItemGroupModel analysisItemGroupModel)
        {
            AnalysisItemGroupBiz.SetAnalysisItemGroupCreate(header, analysisItemGroupModel);
            return analysisItemGroupModel;
        }

        [WebMethod]
        public AnalysisItemGroupModel SetAnalysisItemGroupDetailCreate(HeaderModel header, AnalysisItemGroupModel analysisItemGroupModel)
        {
            AnalysisItemGroupBiz.SetAnalysisItemGroupDetailCreate(header, analysisItemGroupModel);
            return analysisItemGroupModel;
        }

        [WebMethod]
        public AnalysisItemGroupModel SetAnalysisItemGroupUpdate(HeaderModel header, AnalysisItemGroupModel analysisItemGroupModel)
        {
            AnalysisItemGroupBiz.SetAnalysisItemGroupUpdate(header, analysisItemGroupModel);
            return analysisItemGroupModel;
        }

        [WebMethod]
        public AnalysisItemGroupModel SetAnalysisItemGroupDelete(HeaderModel header, AnalysisItemGroupModel analysisItemGroupModel)
        {
            AnalysisItemGroupBiz.SetAnalysisItemGroupDelete(header, analysisItemGroupModel);
            return analysisItemGroupModel;
        }

        [WebMethod]
        public AnalysisItemGroupModel SetAnalysisItemGroupDetailDelete(HeaderModel header, AnalysisItemGroupModel analysisItemGroupModel)
        {
            AnalysisItemGroupBiz.SetAnalysisItemGroupDetailDelete(header, analysisItemGroupModel);
            return analysisItemGroupModel;
        }
    }
}