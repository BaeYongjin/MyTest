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
	/// GenreService�� ���� ��� �����Դϴ�.
	/// </summary>
	public class GenreService : System.Web.Services.WebService
	{
		private GenreBiz genreBiz = null;
		public GenreService()
		{
			//CODEGEN: �� ȣ���� ASP.NET �� ���� �����̳ʿ� �ʿ��մϴ�.
			InitializeComponent();
			genreBiz = new GenreBiz();
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
		public GenreModel GetGenreList(HeaderModel header, GenreModel genreModel)
		{
			genreBiz.GetGenreList(header, genreModel);
			return genreModel;
		}

		[WebMethod]
		public GenreModel SetGenreUpdate(HeaderModel header, GenreModel genreModel)
		{
			genreBiz.SetGenreUpdate(header, genreModel);
			return genreModel;
		}

		[WebMethod]
		public GenreModel SetGenreCreate(HeaderModel header, GenreModel genreModel)
		{
			genreBiz.SetGenreCreate(header, genreModel);
			return genreModel;
		}

		[WebMethod]
		public GenreModel SetGenreDelete(HeaderModel header, GenreModel genreModel)
		{
			genreBiz.SetGenreDelete(header, genreModel);
			return genreModel;
		}
	}
}