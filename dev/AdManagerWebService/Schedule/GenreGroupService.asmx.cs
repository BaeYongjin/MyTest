using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Web;
using System.Web.Services;

using WinFramework.Base;
using AdManagerModel;

namespace AdManagerWebService.Contract
{
    /// <summary>
    /// �⺻���ӽ����̽��� �����Ѵ�.
    /// </summary>
    [WebService(Namespace = "http://advmgt.hanafostv.com/AdManagerWebService/")]
    [System.ComponentModel.ToolboxItem(false)]

	/// <summary>
	/// GenreGroupService�� ���� ��� �����Դϴ�.
	/// </summary>
	public class GenreGroupService : System.Web.Services.WebService
	{

		private GenreGroupBiz genreGroupBiz = null;

		public GenreGroupService()
		{
			//CODEGEN: �� ȣ���� ASP.NET �� ���� �����̳ʿ� �ʿ��մϴ�.
			InitializeComponent();

			genreGroupBiz = new GenreGroupBiz();
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
        public GenreGroupModel GetGenreGroupList(HeaderModel header, GenreGroupModel genreGroupModel)
        {
            genreGroupBiz.GetGenreGroupList(header, genreGroupModel);
            return genreGroupModel;
        }
        [WebMethod]
        public GenreGroupModel GetGenreGroupDetailList(HeaderModel header, GenreGroupModel genreGroupModel)
        {
            genreGroupBiz.GetGenreGroupDetailList(header, genreGroupModel);
            return genreGroupModel;
        }

		[WebMethod]
		public GenreGroupModel GetInspectGenreGroupList_p(HeaderModel header, GenreGroupModel genreGroupModel)
		{
			genreGroupBiz.GetInspectGenreGroupList_p(header, genreGroupModel);
			return genreGroupModel;
		}

        [WebMethod]
        public GenreGroupModel GetGenreGroupList_p(HeaderModel header, GenreGroupModel genreGroupModel)
        {
            genreGroupBiz.GetGenreGroupList_p(header, genreGroupModel);
            return genreGroupModel;
        }

		[WebMethod, Description("�ǽð�ä�������� ä���帣-ä������ �������� ������. �˾�â���� ���")]
		public GenreGroupModel GetChannelList_p(HeaderModel header, GenreGroupModel genreGroupModel)
        {
			genreGroupBiz.GetChannelList_p(header, genreGroupModel);
            return genreGroupModel;
        }
		
		[WebMethod]
		public GenreGroupModel SetGenreGroupUpdate(HeaderModel header, GenreGroupModel genreGroupModel)
		{
			genreGroupBiz.SetGenreGroupUpdate(header, genreGroupModel);
			return genreGroupModel;
		}

		[WebMethod]
		public GenreGroupModel SetGenreGroupCreate(HeaderModel header, GenreGroupModel genreGroupModel)
		{
			genreGroupBiz.SetGenreGroupCreate(header, genreGroupModel);
			return genreGroupModel;
		}

		[WebMethod]
		public GenreGroupModel SetGenreGroupDelete(HeaderModel header, GenreGroupModel genreGroupModel)
		{
			genreGroupBiz.SetGenreGroupDelete(header, genreGroupModel);
			return genreGroupModel;
		}
	}
}
