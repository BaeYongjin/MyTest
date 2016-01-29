// ===============================================================================
// Contract Data Model for Charites Project
//
// ChannelJumpModel.cs
//
// 채널점핑 클래스를 정의합니다. 
//
// ===============================================================================
// Release history
// ===============================================================================
// Copyright (C) 2007 G-Inno Systems Inc.
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
    public class ChannelJumpModel : BaseModel
    {
        #region 변수선언
        // 조회용
        private string _SearchKey				= null;		// 검색어
		private string _SearchMediaCode			= null;	
		private string _SearchRapCode			= null;		
		private string _SearchchkAdState_10		= null;		// 검색 광고상태 : 준비
		private string _SearchchkAdState_20		= null;		// 검색 광고상태 : 편성
		private string _SearchchkAdState_30		= null;		// 검색 광고상태 : 중지
		private string _SearchchkAdState_40		= null;		// 검색 광고상태 : 종료
		private string _SearchAdType			= null;		// 검색 광고종류
		private string _SearchJumpType			= null;		// 검색 점핑구분

        // 상세정보용
        private string _ItemNo          = null;		// 광고번호
        private string _ItemName        = null;		// 광고명
		private string _MediaCode       = null;		// 매체코드
		private string _JumpType		= null;		// 점프구분
        private string _GenreCode       = null;		// 장르코드
        private string _GenreName       = null;		// 장르명
        private string _ChannelNo       = null;		// 채널번호
        private string _Title           = null;		// 프로그램명
        private string _ContentID       = null;		// 컨텐츠ID
		private string _PopupID         = null;		// 공지ID
		private string _PopupTitle      = null;		// 공지제목
        private string  _ChannelManager = null;		// 채널매니져
        private string _HomeYn          = null;		// 홈 노출여부
        private string _ChannelYn       = null;		// 채널 노출여부
		private string _Type			= null;		// 채널 노출여부

		// 2012/06/01 추가
		private string _StbTypeYn = null;			// 셋탑별 타겟팅 여부
		private string _StbTypeStr = null;			// 셋탑별 타겟팅 값

        // 목록조회용
		private DataSet  _DataSet;
		private DataSet  _DataSet2;	// 광고검색용
		private DataSet  _DataSet3;	// 채널검색용
		private DataSet  _DataSet4;	// 컨텐츠검색용
		
		private DataSet  _DataSet5; // 공지시스템 팝업공지리스트조회용
        #endregion

        public ChannelJumpModel() : base () {   Init(); }

        #region Public 메소드
        public void Init()
        {
			_SearchKey				= "";
			_SearchMediaCode		= "";
			_SearchRapCode			= "";
			_SearchchkAdState_10	= "";
			_SearchchkAdState_20	= "";
			_SearchchkAdState_30	= "";
			_SearchchkAdState_40	= "";
			_SearchAdType			= "";
			_SearchJumpType			= "";

			_ItemNo			= "";		
			_ItemName		= "";		
			_MediaCode		= "";		
			_JumpType		= "";		
			_GenreCode		= "";		
			_GenreName		= "";		
			_ChannelNo		= "";		
			_Title			= "";		
			_ContentID		= "";      
			_PopupID        = "";
			_PopupTitle     = "";
            _ChannelManager = "";
			_HomeYn			= "";      
			_ChannelYn		= "";
			_Type		= "";

			_DataSet = new DataSet();
			_DataSet2 = new DataSet();
			_DataSet3 = new DataSet();
			_DataSet4 = new DataSet();
			_DataSet5 = new DataSet();
		}

        #endregion

        #region  프로퍼티 

        public DataSet ChannelJumpDataSet
        {
            get { return _DataSet;	}
            set { _DataSet = value;	}
        }

		public DataSet ContractItemDataSet
		{
			get { return _DataSet2;	}
			set { _DataSet2 = value;	}
		}

		public DataSet ChannelListDataSet
		{
			get { return _DataSet3;	}
			set { _DataSet3 = value;	}
		}

		public DataSet ContentListDataSet
		{
			get { return _DataSet4;	}
			set { _DataSet4 = value;	}
		}

		public DataSet AdPopListDataSet
		{
			get { return _DataSet5;	}
			set { _DataSet5 = value;	}
		}

        public string SearchKey 
        {
            get { return _SearchKey;	}
            set { _SearchKey = value;	}
        }
	
        public string SearchMediaCode
        {
            get { return _SearchMediaCode; }
            set { _SearchMediaCode = value;}
        }

        public string SearchRapCode
        {
            get { return _SearchRapCode; }
            set { _SearchRapCode = value;}
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
			get { return _SearchAdType;		}
			set { _SearchAdType = value;		}
		}

		public string SearchJumpType 
		{
			get { return _SearchJumpType;		}
			set { _SearchJumpType = value;		}
		}

		public string ItemNo           
        {
            get { return _ItemNo          ; }
            set { _ItemNo           = value;}
        }

        public string ItemName 
        {
            get { return _ItemName; }
            set { _ItemName = value;}
        }

        public string MediaCode 
        {
            get { return _MediaCode; }
            set { _MediaCode = value;}
        }

        public string JumpType 
        {
            get { return _JumpType; }
            set { _JumpType = value;}
        }

        public string GenreCode 
        {
            get { return _GenreCode; }
            set { _GenreCode = value;}
        }

        public string GenreName 
        {
            get { return _GenreName; }
            set { _GenreName = value;}
        }

        public string ChannelNo 
        {
            get { return _ChannelNo; }
            set { _ChannelNo = value;}
        }

        public string Title 
        {
            get { return _Title; }
            set { _Title = value;}
        }

		public string ContentID 
		{
			get { return _ContentID; }
			set { _ContentID = value;}
		}

		public string PopupID 
		{
			get { return _PopupID; }
			set { _PopupID = value;}
		}

		public string PopupTitle
		{
			get { return _PopupTitle; }
			set { _PopupTitle = value;}
		}

        public string ChannelManager
        {
            get { return _ChannelManager; }
            set { _ChannelManager = value;}
        }

		public string HomeYn 
        {
            get { return _HomeYn; }
            set { _HomeYn = value;}
        }

        public string ChannelYn 
        {
            get { return _ChannelYn; }
            set { _ChannelYn = value;}
        }

		public string Type 
		{
			get { return _Type; }
			set { _Type = value;}
		}

		public string StbTypeYn
		{
			get { return _StbTypeYn; }
			set { _StbTypeYn = value; }
		}

		public string StbTypeString
		{
			get { return _StbTypeStr; }
			set { _StbTypeStr = value; }
		}
 
        #endregion
    }
}