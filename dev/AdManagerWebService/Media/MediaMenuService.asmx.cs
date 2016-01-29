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


    public class MediaMenuService : System.Web.Services.WebService
    {
        private MediaMenuBiz Biz = null;

        public MediaMenuService()
        {
            //CODEGEN: 이 호출은 ASP.NET 웹 서비스 디자이너에 필요합니다.
            InitializeComponent();
            Biz = new MediaMenuBiz();
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
        public MediaMenuModel GetCategoryList(HeaderModel header, MediaMenuModel model)
        {
            Biz.GetCategoryList(header, model);
            return model;
        }
        [WebMethod]
        public MediaMenuModel GetMenuList(HeaderModel header, MediaMenuModel model)
        {
            Biz.GetMenuList(header, model);
            return model;
        }

        [WebMethod]
        public MediaMenuModel SetCategoryUpdate(HeaderModel header, MediaMenuModel model)
        {
            Biz.SetCategoryUpdate(header, model);
            return model;
        }

		[WebMethod]
		public MediaMenuModel SetCategoryUpdateOption(HeaderModel header, MediaMenuModel model)
		{
			Biz.SetCategoryUpdateOption(header, model);
			return model;
		}

        [WebMethod]
        public MediaMenuModel SetMenuUpdate(HeaderModel header, MediaMenuModel model)
        {
            Biz.SetMenuUpdate(header, model);
            return model;
        }

		[WebMethod]
		public MediaMenuModel SetMenuUpdateOption(HeaderModel header, MediaMenuModel model)
		{
			Biz.SetMenuUpdateOption(header, model);
			return model;
		}

        [WebMethod]
        public MediaMenuModel DeleteMenu(HeaderModel header, MediaMenuModel model)
        {
            Biz.DeleteMenu(header, model);
            return model;
        }
        [WebMethod]
        public MediaMenuModel SetCategoryCreate(HeaderModel header, MediaMenuModel model)
        {
            Biz.SetCategoryCreate(header, model);
            return model;
        }

        [WebMethod]
        public MediaMenuModel SetMenuCreate(HeaderModel header, MediaMenuModel model)
        {
            Biz.SetMenuCreate(header, model);
            return model;
        }
    }
}
