using System;
using System.Data;

using WinFramework.Base;

namespace AdManagerModel
{
	/// <summary>
	/// StatisticsPgRegionModel에 대한 요약 설명입니다.
	/// </summary>
	public class StatisticsPgRegionModel : BaseModel
	{

		// 조회용
		private string _SearchMediaCode	     = null;		// 검색 매체
		private string _SearchCategoryCode   = null;		// 조회 카테고리코드
		private string _SearchGenreCode      = null;		// 조회 장르코드
		private string _SearchKey            = null;		// 조회 프로그램명
		private string _SearchType           = null;		// 조회 구분 B:선택기간 D:일간 W:주간 M:월간
		private string _SearchStartDay       = null;		// 조회 집계시작 일자
		private string _SearchEndDay         = null;		// 조회 집계시작 일자

		// 목록조회용
		private DataSet  _DataSet;
		private DataSet  _DataSet2;
		private DataSet  _DataSet3;

		public StatisticsPgRegionModel() : base () 
		{
			Init();
		}

		#region Public 메소드
		public void Init()
		{
			_SearchMediaCode 	   = "";
			_SearchCategoryCode    = "";
			_SearchGenreCode       = "";
			_SearchKey             = "";
			_SearchType            = "";
			_SearchStartDay          = "";
			_SearchEndDay          = "";
			_DataSet = new DataSet();
			_DataSet2 = new DataSet();
			_DataSet3 = new DataSet();
		}

		#endregion

		#region  프로퍼티 

		public DataSet ReportDataSet
		{
			get { return _DataSet;	}
			set { _DataSet = value;	}
		}

		public DataSet CategoryDataSet
		{
			get { return _DataSet2;	}
			set { _DataSet2 = value;	}
		}

		public DataSet GenreDataSet
		{
			get { return _DataSet3;	}
			set { _DataSet3 = value;	}
		}


		public string SearchMediaCode 
		{
			get { return _SearchMediaCode;	}
			set { _SearchMediaCode = value;	}
		}

		public string SearchCategoryCode
		{
			get { return _SearchCategoryCode;	}
			set { _SearchCategoryCode = value;	}
		}

		public string SearchGenreCode 
		{
			get { return _SearchGenreCode;	}
			set { _SearchGenreCode = value;	}
		}

		public string SearchKey 
		{
			get { return _SearchKey;	}
			set { _SearchKey = value;	}
		}

		public string SearchType 
		{
			get { return _SearchType;	}
			set { _SearchType = value;	}
		}

		public string SearchStartDay 
		{
			get { return _SearchStartDay;	}
			set { _SearchStartDay = value;	}
		}

		public string SearchEndDay 
		{
			get { return _SearchEndDay;	}
			set { _SearchEndDay = value;	}
		}

		#endregion

	}
}