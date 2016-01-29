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
	/// SchOrgGenreService�� ���� ��� �����Դϴ�.
	/// </summary>
	public class SchOrgGenreService : System.Web.Services.WebService
	{

		private SchOrgGenreBiz SchOrgGenreBiz = null;

		public SchOrgGenreService()
		{
			//CODEGEN: �� ȣ���� ASP.NET �� ���� �����̳ʿ� �ʿ��մϴ�.
			InitializeComponent();

			SchOrgGenreBiz = new SchOrgGenreBiz();
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

        #region ���帣��� OAP �� ���

        [WebMethod]
		public SchOrgGenreModel GetMenuList(HeaderModel header, SchOrgGenreModel SchOrgGenreModel)
		{
			SchOrgGenreBiz.GetMenuList(header, SchOrgGenreModel);
			return SchOrgGenreModel;
		}
        
        #endregion

        #region ���帣��� OAP �� ����
        
        [WebMethod]
        public SchOrgGenreModel UpdateSchOrgGenre(HeaderModel header, SchOrgGenreModel SchOrgGenreModel)
		{
            SchOrgGenreBiz.UpdateSchOrgGenre(header, SchOrgGenreModel);
			return SchOrgGenreModel;
		}

        #endregion

        #region ���帣��� OAP �� �߰�

        [WebMethod]
        public SchOrgGenreModel InsertSchOrgGenre(HeaderModel header, SchOrgGenreModel SchOrgGenreModel)
        {
            SchOrgGenreBiz.InsertSchOrgGenre(header, SchOrgGenreModel);
            return SchOrgGenreModel;
        }

        #endregion

        #region ���帣��� OAP �� ����

        [WebMethod]
        public SchOrgGenreModel DeleteSchOrgGenre(HeaderModel header, SchOrgGenreModel SchOrgGenreModel)
        {
            SchOrgGenreBiz.DeleteSchOrgGenre(header, SchOrgGenreModel);
            return SchOrgGenreModel;
        }

        #endregion


    }
}
