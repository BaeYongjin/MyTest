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
    /// 기본네임스페이스를 설정한다.
    /// </summary>
    [WebService(Namespace = "http://advmgt.hanafostv.com/AdManagerWebService/")]
    [System.ComponentModel.ToolboxItem(false)]

	/// <summary>
	/// GenreGroupService에 대한 요약 설명입니다.
	/// </summary>
	public class GenreGroupService : System.Web.Services.WebService
	{

		private GenreGroupBiz genreGroupBiz = null;

		public GenreGroupService()
		{
			//CODEGEN: 이 호출은 ASP.NET 웹 서비스 디자이너에 필요합니다.
			InitializeComponent();

			genreGroupBiz = new GenreGroupBiz();
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

		[WebMethod, Description("실시간채널정보를 채널장르-채널정보 형식으로 제공함. 팝업창에서 사용")]
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
