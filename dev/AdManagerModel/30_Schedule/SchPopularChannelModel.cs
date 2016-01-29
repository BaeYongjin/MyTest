
using System;
using System.Data;

using WinFramework.Base;

namespace AdManagerModel
{
	/// <summary>
	/// 사용자정보의 클래스 모델.
	/// </summary>
	public class SchPopularChannelModel : BaseModel
	{

		// 조회용
		private string _SearchMediaCode    = null;		// 검색매체
		private string _SearchKey       = null;		// 검색어		
		private string _SearchType           = null;		// 조회 구분 B:선택기간 D:일간 W:주간 M:월간
		private string _SearchStartDay       = null;		// 조회 집계시작 일자
		private string _SearchEndDay         = null;		// 조회 집계시작 일자

		// 상세조회용
		private string _MediaCode     = null;
		private string _CategoryCode  = null;
		private string _GenreCode     = null;
		private string _Channel     = null;
		private string _ChannelNo     = null;
		private string _ItemNo        = null;
		private string _ScheduleOrder = null;
		private string _ItemName      = null;
		private string _Comment      = null;
		private string _GenreName		= null;		//장르명
		
		// 목록조회용
		private DataSet  _SchPopularChannelDataSet;    

		private string _LastOrder     = null;

		// 미디어 콤보 조회용
			
		public SchPopularChannelModel() : base () 
		{
			Init();
		}

		#region Public 메소드
		public void Init()
		{			
			_SearchMediaCode = "";
			_SearchKey = "";			
			_SearchType            = "";
			_SearchStartDay          = "";
			_SearchEndDay          = "";

			_MediaCode       = "";
			_CategoryCode    = "";
			_GenreCode       = "";
			_Channel       = "";
			_ChannelNo       = "";
			_ItemNo          = "";
			_ScheduleOrder   = "";
			_ItemName        = "";
			_Comment        = "";
			_GenreName        = "";

			_SchPopularChannelDataSet = new DataSet();
			_LastOrder       = "";
		}

		#endregion

		#region  프로퍼티 

		public DataSet SchPopularChannelDataSet
		{
			get { return _SchPopularChannelDataSet;	}
			set { _SchPopularChannelDataSet = value;	}
		}

		public string SearchMediaCode
		{
			get { return _SearchMediaCode;	}
			set { _SearchMediaCode = value;	}
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

		public string ItemNo
		{
			get { return _ItemNo;	}
			set { _ItemNo = value;	}
		}

		public string MediaCode
		{
			get { return _MediaCode;	}
			set { _MediaCode = value;	}
		}

		public string CategoryCode
		{
			get { return _CategoryCode;	}
			set { _CategoryCode = value;	}
		}

		public string GenreCode
		{
			get { return _GenreCode;	}
			set { _GenreCode = value;	}
		}

		public string Channel
		{
			get { return _Channel;	}
			set { _Channel = value;	}
		}

		public string ChannelNo
		{
			get { return _ChannelNo;	}
			set { _ChannelNo = value;	}
		}

		public string ScheduleOrder
		{
			get { return _ScheduleOrder;	}
			set { _ScheduleOrder = value;	}
		}

		public string ItemName
		{
			get { return _ItemName;	}
			set { _ItemName = value;	}
		}

		public string Comment
		{
			get { return _Comment;	}
			set { _Comment = value;	}
		}

		public string GenreName
		{
			get { return _GenreName;	}
			set { _GenreName = value;	}
		}


		public string LastOrder
		{
			get { return _LastOrder;	}
			set { _LastOrder = value;	}
		}

		#endregion

	}
}
