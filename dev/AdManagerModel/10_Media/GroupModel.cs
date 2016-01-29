// ===============================================================================
// Group Data Model for Charites Project
//
// GroupModel.cs
//
// 사용자정보 클래스를 정의합니다. 
//
// ===============================================================================
// Release history
//
// ===============================================================================
// Copyright (C) 2006 G-Inno Systems Inc.
// All rights reserved.
// ==============================================================================

using System;
using System.Data;

using WinFramework.Base;

namespace AdManagerModel
{
	/// <summary>
	/// 사용자정보의 클래스 모델.
	/// </summary>
	public class GroupModel : BaseModel
	{

		// 조회용
		private string _SearchKey   = null;
		private string _SearchMedia = null;
		private string _SearchGroup = null;
		
		// 상세정보용
		private string _GroupCode   = null;
		private string _GroupName   = null;
		private string _RegDt		= null;
		private string _Comment     = null;
		private string _UseYn       = null;

		private string _MediaCode       = null;
		private string _CategoryCode    = null;
		private string _CategoryName    = null;
		private string _GenreCode		= null;
		private string _GenreName       = null;
		private string _ChannelNo       = null;
		private	string	_SeriesNo		= null;
		private string _Title           = null;
        private bool _InvalidYn = false;
        private string _SearchType = "";
		
		// 목록조회용
		private DataSet _GroupDataSet;
		private DataSet _GroupDetailDataSet;
		private DataSet _CategoryDataSet;
		private DataSet _GenreDataSet;
		private DataSet _ChannelDataSet;
		private DataSet _SeriesDataSet;
        private DataSet _GroupMapDataSet;
	
		public GroupModel() : base () 
		{
			Init();
		}

		#region Public 메소드
		public void Init()
		{
			_SearchKey		= "";
			_SearchMedia = "";
			_SearchGroup = "";

			_GroupCode			= "";
			_GroupName		= "";			
			_RegDt			= "";
			_Comment 	= "";
			_UseYn 	= "";

			_MediaCode			= "";
			_CategoryCode			= "";
			_CategoryName		= "";			
			_GenreCode			= "";
			_GenreName 	= "";
			_ChannelNo 	= "";
			_Title 	= "";
            _InvalidYn = false;
            _SearchType = "";

			_GroupDataSet       = new DataSet();
			_GroupDetailDataSet = new DataSet();
			_CategoryDataSet    = new DataSet();
			_GenreDataSet       = new DataSet();
			_ChannelDataSet     = new DataSet();
			_SeriesDataSet		= new DataSet();
            _GroupMapDataSet    = new DataSet();
		}

		#endregion

		#region  프로퍼티 

		public DataSet GroupDataSet
		{
			get { return _GroupDataSet;	}
			set { _GroupDataSet = value;	}
		}

		public DataSet GroupDetailDataSet
		{
			get { return _GroupDetailDataSet;	}
			set { _GroupDetailDataSet = value;	}
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

		public DataSet ChannelDataSet
		{
			get { return _ChannelDataSet;	}
			set { _ChannelDataSet = value;	}
		}

		/// <summary>
		/// 시리즈정보를 담은 DataSet
		/// </summary>
		public DataSet SeriesDataSet
		{
			get { return _SeriesDataSet;	}
			set { _SeriesDataSet = value;	}
		}

        public DataSet GroupMapDataSet
        {
            get { return _GroupMapDataSet;  }
            set { _GroupMapDataSet = value; }
        }

		public string SearchKey 
		{
			get { return _SearchKey;	}
			set { _SearchKey = value;	}
		}
	
		public string SearchMedia
		{
			get { return _SearchMedia;	}
			set { _SearchMedia = value;	}
		}

		public string SearchGroup
		{
			get { return _SearchGroup;	}
			set { _SearchGroup = value;	}
		}

		public string GroupCode 
		{
			get { return _GroupCode;		}
			set { _GroupCode = value;		}
		}

		public string GroupName 
		{
			get { return _GroupName;		}
			set { _GroupName = value;	}
		}

		public string RegDt 
		{
			get { return _RegDt;		}
			set { _RegDt = value;		}
		}

		public string Comment 
		{
			get { return _Comment;  }
			set { _Comment = value; }
		}

		public string UseYn
		{
			get { return _UseYn;	}
			set { _UseYn = value;	}
		}

		public string MediaCode 
		{
			get { return _MediaCode;		}
			set { _MediaCode = value;		}
		}

		public string CategoryCode 
		{
			get { return _CategoryCode;		}
			set { _CategoryCode = value;		}
		}

		public string CategoryName 
		{
			get { return _CategoryName;		}
			set { _CategoryName = value;	}
		}

		public string GenreCode 
		{
			get { return _GenreCode;		}
			set { _GenreCode = value;		}
		}

		public string GenreName 
		{
			get { return _GenreName;  }
			set { _GenreName = value; }
		}

		public string ChannelNo
		{
			get { return _ChannelNo;	}
			set { _ChannelNo = value;	}
		}

		public string Title
		{
			get { return _Title;	}
			set { _Title = value;	}
		}

		/// <summary>
		/// 시리즈번호를 설정하거나 가져옵니다
		/// </summary>
		public string SeriesNo
		{
			get { return _SeriesNo;	}
			set { _SeriesNo = value;	}
		}

        /// <summary>
        /// 무효처리된 메뉴리스트를 가져 올것인지 여부
        /// </summary>
        public bool InvalidYn
        {
            get { return _InvalidYn; }
            set { _InvalidYn = value; }
        }

        /// <summary>
        /// 어디에서 검색어를 찾을것인가? 카테고리(C), 장르(G), 프로그램(P), 회차(S) 인가?
        /// </summary>
        public string SearchType
        {
            get { return _SearchType; }
            set { _SearchType = value; }
        }

		#endregion

	}
}
