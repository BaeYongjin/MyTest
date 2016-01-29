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
	/// MediaMenuSetService�� ���� ��� �����Դϴ�.
	/// </summary>
	public class MediaMenuSetService : System.Web.Services.WebService
	{
       
        private MediaMenuSetBiz Biz = null;

		public MediaMenuSetService()
		{
			//CODEGEN: �� ȣ���� ASP.NET �� ���� �����̳ʿ� �ʿ��մϴ�.
			InitializeComponent();
            Biz = new MediaMenuSetBiz();
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
        public MediaMenuSetModel GetCategoryList(HeaderModel header, MediaMenuSetModel    model)
        {
            Biz.GetCategoryList( header, model );
            return model;
        }

        [WebMethod]
        public MediaMenuSetModel GetGenreList(HeaderModel header, MediaMenuSetModel    model)
        {
            Biz.GetGenreList( header, model );
            return model;
        }

        [WebMethod]
        public MediaMenuSetModel CategoryInsert(HeaderModel header, MediaMenuSetModel    model)
        {
            Biz.CategoryInsert( header, model );
            return model;
        }

        [WebMethod]
        public MediaMenuSetModel CategoryDelete(HeaderModel header, MediaMenuSetModel    model)
        {
            Biz.CategoryDelete( header, model );
            return model;
        }

        [WebMethod]
        public MediaMenuSetModel GenreInsert(HeaderModel header, MediaMenuSetModel    model)
        {
            Biz.GenreInsert( header, model );
            return model;
        }

        [WebMethod]
        public MediaMenuSetModel GenreDelete(HeaderModel header, MediaMenuSetModel    model)
        {
            Biz.GenreDelete( header, model );
            return model;
        }
	}
}
