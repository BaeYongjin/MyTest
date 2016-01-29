using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Web;
using System.Web.Services;

using WinFramework.Base;
using AdManagerModel;

namespace AdManagerWebService.Media
{
    /// <summary>
    /// 기본네임스페이스를 설정한다.
    /// </summary>
    [WebService(Namespace = "http://advmgt.hanafostv.com/AdManagerWebService/")]
    [System.ComponentModel.ToolboxItem(false)]

	/// <summary>
    /// ContentsService에 대한 요약 설명입니다.
    /// </summary>
    public class ContentsService : System.Web.Services.WebService
    {

        private ContentsBiz contentsBiz = null;

        public ContentsService()
        {
            //CODEGEN: 이 호출은 ASP.NET 웹 서비스 디자이너에 필요합니다.
            InitializeComponent();

            contentsBiz = new ContentsBiz();
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
        public ContentsModel GetContentsListCommon(HeaderModel header, ContentsModel contentsModel)
        {
            contentsBiz.GetContentsListCommon(header, contentsModel);
            return contentsModel;
        }

        [WebMethod]
        public ContentsModel GetContentsList(HeaderModel header, ContentsModel contentsModel)
        {
            contentsBiz.GetContentsList(header, contentsModel);
            return contentsModel;
        }

        [WebMethod]
        public ContentsModel GetContentsListPopUp(HeaderModel header, ContentsModel contentsModel)
        {
            contentsBiz.GetContentsListPopUp(header, contentsModel);
            return contentsModel;
        }


        [WebMethod]
        public ContentsModel SetContentsUpdate(HeaderModel header, ContentsModel contentsModel)
        {
            contentsBiz.SetContentsUpdate(header, contentsModel);
            return contentsModel;
        }

        [WebMethod]
        public ContentsModel SetContentsCreate(HeaderModel header, ContentsModel contentsModel)
        {
            contentsBiz.SetContentsCreate(header, contentsModel);
            return contentsModel;
        }

        [WebMethod]
        public ContentsModel SetContentsDelete(HeaderModel header, ContentsModel contentsModel)
        {
            contentsBiz.SetContentsDelete(header, contentsModel);
            return contentsModel;
        }
    }
}
