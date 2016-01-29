using System;
using System.Data;

using WinFramework.Base;

namespace AdManagerModel
{
	/// <summary>
	/// ItemModel에 대한 요약 설명입니다.
	/// </summary>
	public class ItemModel : BaseModel
	{

		// 조회용
		private string _SearchKey           = null;		// 검색어
		private string _SearchMediaCode	    = null;		// 검색 매체
        private string _SearchCugCode       = null;		// 검색 CUG
		private string _SearchRapCode       = null;		// 검색 랩
		private string _SearchAgencyCode    = null;		// 검색 대행사
		private string _SearchAdvertiserCode= null;		// 검색 광고주
		private string _SearchContractState = null;		// 검색 계약상태
		private string _SearchAdClass       = null;		// 검색 광고용도
		private string _SearchchkAdState_10 = null;		// 검색 광고상태 : 준비
		private string _SearchchkAdState_20 = null;		// 검색 광고상태 : 편성
		private string _SearchchkAdState_30 = null;		// 검색 광고상태 : 중지
		private string _SearchchkAdState_40 = null;		// 검색 광고상태 : 종료
        private string _SearchAdType        = null;     // 검색 광고종류

		// 상세정보용		
		private string _ItemNo				= null;		// 계약내역Key
		private string _ItemName			= null;		// 광고명
		private string _MediaCode           = null;		// 매체코드

        //-------------------------------------
        private string _RapCode             = null;		// 매체코드
		private string _MediaName			= null;		// 매체명
		private string _GenreCode           = null;		// 장르(메뉴)코드
		private string _GenreName		= null;		// 장르명
		private string _ChannelNo       = null;		// 채널번호
		private	string _SeriesNo		= null;		// 회차번호
		private string _Title   			= null;		// 제목
		private string _AdType              = null;		// 광고종류
		private TYPE_Schedule	_SchType;

		private string _ScheduleOrder       = null;

		// 목록조회용
		private DataSet  _DataSet;

        // 파일건수검사용
        private int _FileListCount = 0;

		public ItemModel() : base () 
		{
			Init();
		}

		#region Public 메소드
		public void Init()
		{
			_SearchKey		       = "";
			_SearchMediaCode 	   = "";
            _SearchCugCode         = "";
			_SearchRapCode         = "";
			_SearchAgencyCode      = "";
			_SearchAdvertiserCode  = "";
			_SearchContractState   = "";
			_SearchAdClass         = "";
			_SearchchkAdState_10   = "";
			_SearchchkAdState_20   = "";
			_SearchchkAdState_30   = "";
			_SearchchkAdState_40   = "";
            _SearchAdType          = "00";
			
			_ItemNo				   = "";
			_ItemName              = "";
            _RapCode               = "";
			_MediaCode             = "";
			_MediaName             = "";
			_GenreCode             = "";
			_GenreName             = "";
			_ChannelNo             = "";
			_Title                 = "";
			_AdType                = "";

			_ScheduleOrder         = "";
            _FileListCount         = 0;
			            
			_DataSet = new DataSet();
		}

		#endregion

		#region  프로퍼티 

		public DataSet ScheduleDataSet
		{
			get { return _DataSet;	}
			set { _DataSet = value;	}
		}

		public string SearchKey 
		{
			get { return _SearchKey;	}
			set { _SearchKey = value;	}
		}
	
		public string SearchMediaCode 
		{
			get { return _SearchMediaCode;	}
			set { _SearchMediaCode = value;	}
		}

        public string SearchCugCode
        {
            get { return _SearchCugCode; }
            set { _SearchCugCode = value; }
        }

		public string SearchRapCode 
		{
			get { return _SearchRapCode;		}
			set { _SearchRapCode = value;		}
		}

		public string SearchAgencyCode 
		{
			get { return _SearchAgencyCode;		}
			set { _SearchAgencyCode = value;	}
		}

		public string SearchAdvertiserCode 
		{
			get { return _SearchAdvertiserCode;		}
			set { _SearchAdvertiserCode = value;	}
		}

		public string SearchContractState 
		{
			get { return _SearchContractState;		}
			set { _SearchContractState = value;		}
		}

		public string SearchAdClass 
		{
			get { return _SearchAdClass;		}
			set { _SearchAdClass = value;		}
		}

		public string SearchchkAdState_10 
		{
			get { return _SearchchkAdState_10;		}
			set { _SearchchkAdState_10 = value;		}
		}

		public string SearchchkAdState_20 
		{
			get { return _SearchchkAdState_20;		}
			set { _SearchchkAdState_20 = value;		}
		}

		public string SearchchkAdState_30 
		{
			get { return _SearchchkAdState_30;		}
			set { _SearchchkAdState_30 = value;		}
		}

		public string SearchchkAdState_40 
		{
			get { return _SearchchkAdState_40;		}
			set { _SearchchkAdState_40 = value;		}
		}

        public string SearchAdType
        {
            get { return _SearchAdType;	}
            set { _SearchAdType = value;	}
        }

		public string ItemNo
		{
			get { return _ItemNo;	}
			set { _ItemNo = value;	}
		}

		public string ItemName
		{
			get { return _ItemName;	}
			set { _ItemName = value;	}
		}

		public string MediaCode
		{
			get { return _MediaCode;	}
			set { _MediaCode = value;	}
		}

		public string MediaName
		{
			get { return _MediaName;	}
			set { _MediaName = value;	}
		}
		
		public string GenreCode
		{
			get { return _GenreCode;	}
			set { _GenreCode = value;	}
		}

		public string GenreName
		{
			get { return _GenreName;	}
			set { _GenreName = value;	}
		}

		public string ChannelNo
		{
			get { return _ChannelNo;	}
			set { _ChannelNo = value;	}
		}

		public string SeriesNo
		{
			get
			{
				return _SeriesNo;
			}
			set
			{
				_SeriesNo = value;
			}
		}

		public TYPE_Schedule	ScheduleType
		{
			get
			{
				return _SchType;
			}
			set
			{
				_SchType = value;
			}
		}

		public string Title
		{
			get { return _Title;	}
			set { _Title = value;	}
		}

		public string AdType
		{
			get { return _AdType;	}
			set { _AdType = value;	}
		}

		public string ScheduleOrder
		{
			get { return _ScheduleOrder;	}
			set { _ScheduleOrder = value;	}
		}

        public int FileListCount
        {
            get { return _FileListCount; }
            set { _FileListCount = value; }
        }

        public string RapCode
		{
            get { return _RapCode; }
            set { _RapCode = value; }
		}
        
		#endregion

	}
}