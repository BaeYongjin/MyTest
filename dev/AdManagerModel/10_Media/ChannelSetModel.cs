
using System;
using System.Data;

using WinFramework.Base;

namespace AdManagerModel
{
	/// <summary>
	/// 사용자정보의 클래스 모델.
	/// </summary>
	public class ChannelSetModel : BaseModel
	{

		// 조회용
		private string _SearchKey       = null;		// 검색어
		private string _SearchMediaName = null;		// 검색레벨
		private string _SearchCategoryName = null;		// 검색레벨
		private string _SearchGenreName = null;		// 검색레벨
		
		// 상세정보용
		private string _MediaCode       = null;		// 매체코드		
		private string _MediaCode_P       = null;		// 매체코드		
		private string _MediaCode_old       = null;		// 매체코드		
		private string _MediaName       = null;		// 매체명칭
		private string _CategoryCode       = null;		// 매체코드
		private string _CategoryCode_P       = null;		// 매체코드
		private string _CategoryCode_old       = null;		// 매체코드
		private string _CategoryName       = null;		// 매체코드
		private string _ChannelNo       = null;		// 컨텐츠 등급
		private string _ChannelNo_old       = null;		// 컨텐츠 등급
		private string _Title           = null;		// 컨텐츠명
		private string _SeriesNo           = null;		// 컨텐츠명
		private string _ContentId           = null;		// 컨텐츠명
		private string _TotalSeries           = null;		// 컨텐츠명		
		private string _GenreCode       = null;		// 매체코드		
		private string _GenreCode_P       = null;		// 매체코드		
		private string _GenreCode_old       = null;		// 매체코드		
		private string _GenreName       = null;		// 매체코드		
		private string _ModDt           = null;		// 최종수정일시
		private string _CheckYn           = null;		// 최종수정일시
			
		// 목록조회용
		private DataSet  _ChannelSetDataSet;
		private DataSet  _CategoryDataSet;
		private DataSet  _GenreDataSet;

		// 미디어 콤보 조회용
			
		public ChannelSetModel() : base () 
		{
			Init();
		}

		#region Public 메소드
		public void Init()
		{			
			_SearchKey		= "";
			_SearchMediaName		= "";
			_SearchCategoryName		= "";
			_SearchGenreName		= "";

			_MediaCode       = "";		// 매체코드
			_MediaCode_P       = "";		// 매체코드
			_MediaCode_old       = "";		// 매체코드
			_MediaName       = "";
			_CategoryCode       = "";		// 매체코드
			_CategoryCode_P       = "";		// 매체코드
			_CategoryCode_old       = "";		// 매체코드
			_CategoryName       = "";		// 매체코드
			_ChannelNo       = "";		// 채널코드
			_ChannelNo_old       = "";		// 채널코드
			_Title           = "";      // 컨텐츠명
			_SeriesNo        = "";		// 시리즈 번호
			_ContentId       = "";		// 컨텐츠 아이디
			_TotalSeries	 = "";		// 시리즈 편수
			_GenreCode       = "";		// 매체코드
			_GenreCode_P       = "";		// 매체코드
			_GenreCode_old       = "";		// 매체코드
			_GenreName       = "";		// 매체코드
			_ModDt           = "";		// 등록일자
			_CheckYn         = "";
	

			_ChannelSetDataSet = new DataSet();
			_CategoryDataSet   = new DataSet();
			_GenreDataSet   = new DataSet();
		}

		#endregion

		#region  프로퍼티 

		public DataSet ChannelSetDataSet
		{
			get { return _ChannelSetDataSet;	}
			set { _ChannelSetDataSet = value;	}
		}
		public DataSet CategoryDataSet
		{
			get { return _CategoryDataSet;	}
			set { _CategoryDataSet = value;	}
		}

		public DataSet GenreDataSet
		{
			get { return _GenreDataSet;	}
			set { _GenreDataSet = value;	}
		}

		public string SearchKey 
		{
			get { return _SearchKey;	}
			set { _SearchKey = value;	}
		}

		public string SearchMediaName 
		{
			get { return _SearchMediaName;	}
			set { _SearchMediaName = value;	}
		}

		public string SearchCategoryName 
		{
			get { return _SearchCategoryName;	}
			set { _SearchCategoryName = value;	}
		}

		public string SearchGenreName 
		{
			get { return _SearchGenreName;	}
			set { _SearchGenreName = value;	}
		}
	

		public string ContentId 
		{
			get { return _ContentId;		}
			set { _ContentId = value;		}
		}

		public string MediaCode
		{
			get { return _MediaCode;		}
			set { _MediaCode= value;	}
		}

		public string MediaCode_P
		{
			get { return _MediaCode_P;		}
			set { _MediaCode_P= value;	}
		}

		public string MediaCode_old
		{
			get { return _MediaCode_old;		}
			set { _MediaCode_old= value;	}
		}

		public string MediaName
		{
			get { return _MediaName;		}
			set { _MediaName= value;	}
		}

		public string CategoryCode
		{
			get { return _CategoryCode;		}
			set { _CategoryCode= value;	}
		}

		public string CategoryCode_P
		{
			get { return _CategoryCode_P;		}
			set { _CategoryCode_P= value;	}
		}

		public string CategoryCode_old
		{
			get { return _CategoryCode_old;		}
			set { _CategoryCode_old= value;	}
		}

		public string CategoryName
		{
			get { return _CategoryName;		}
			set { _CategoryName= value;	}
		}

		public string ChannelNo
		{
			get { return _ChannelNo; }
			set { _ChannelNo = value;}
		}
        
		public string ChannelNo_old
		{
			get { return _ChannelNo_old; }
			set { _ChannelNo_old = value;}
		}

		public string Title
		{
			get { return _Title; }
			set { _Title = value;}
		}

		public string SeriesNo 
		{
			get { return _SeriesNo;	}
			set { _SeriesNo = value;	}
		}

		public string GenreCode
		{
			get { return _GenreCode;		}
			set { _GenreCode= value;		}
		}

		public string GenreCode_P
		{
			get { return _GenreCode_P;		}
			set { _GenreCode_P= value;		}
		}

		public string GenreCode_old
		{
			get { return _GenreCode_old;		}
			set { _GenreCode_old= value;		}
		}

		public string GenreName
		{
			get { return _GenreName;		}
			set { _GenreName= value;		}
		}

		public string ModDt
		{
			get { return _ModDt;		}
			set { _ModDt= value;		}
		}

		public string TotalSeries 
		{
			get { return _TotalSeries;		}
			set { _TotalSeries = value;		}
		}

		public string CheckYn 
		{
			get { return _CheckYn;		}
			set { _CheckYn = value;		}
		}

        

		#endregion

	}
}
