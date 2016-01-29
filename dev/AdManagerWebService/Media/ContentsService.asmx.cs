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
    /// �⺻���ӽ����̽��� �����Ѵ�.
    /// </summary>
    [WebService(Namespace = "http://advmgt.hanafostv.com/AdManagerWebService/")]
    [System.ComponentModel.ToolboxItem(false)]

	/// <summary>
    /// ContentsService�� ���� ��� �����Դϴ�.
    /// </summary>
    public class ContentsService : System.Web.Services.WebService
    {

        private ContentsBiz contentsBiz = null;

        public ContentsService()
        {
            //CODEGEN: �� ȣ���� ASP.NET �� ���� �����̳ʿ� �ʿ��մϴ�.
            InitializeComponent();

            contentsBiz = new ContentsBiz();
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
