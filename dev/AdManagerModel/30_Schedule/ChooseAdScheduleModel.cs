
using System;
using System.Data;

using WinFramework.Base;

namespace AdManagerModel
{
    /// <summary>
    /// 사용자정보의 클래스 모델.
    /// </summary>
    public class ChooseAdScheduleModel : BaseModel
    {

        // 조회용
        private string _SearchMediaCode    = null;		// 검색매체

		// 상세조회용
		private string _MediaCode     = null;
		private string _CategoryCode  = null;
		private string _GenreCode     = null;
		private string _ChannelNo     = null;
		private	string _Series		  = null;
		private string _ItemNo        = null;
		private string _ScheduleOrder = null;
		private string _ItemName      = null;
        private string _SearchchkAdState_10    = null;		// 검색 광고상태 : 준비
        private string _SearchchkAdState_20    = null;		// 검색 광고상태 : 편성
        private string _SearchchkAdState_30    = null;		// 검색 광고상태 : 중지
        private string _SearchchkAdState_40    = null;		// 검색 광고상태 : 종료
        
		
        // 목록조회용
        private DataSet  _ChooseAdScheduleDataSet;    

		private string _LastOrder     = null;

        // 미디어 콤보 조회용
			
        public ChooseAdScheduleModel() : base () 
        {
            Init();
        }

        #region Public 메소드
        public void Init()
        {			
            _SearchMediaCode = "";

			_MediaCode      = "";
			_CategoryCode   = "";
			_GenreCode      = "";
			_ChannelNo      = "";
			_Series			= "";
			_ItemNo         = "";
			_ScheduleOrder  = "";
			_ItemName       = "";
            _SearchchkAdState_10   = "";
            _SearchchkAdState_20   = "";
            _SearchchkAdState_30   = "";
            _SearchchkAdState_40   = "";

            _ChooseAdScheduleDataSet = new DataSet();
			_LastOrder       = "";
        }

        #endregion

        #region  프로퍼티 

        public DataSet ChooseAdScheduleDataSet
        {
            get { return _ChooseAdScheduleDataSet;	}
            set { _ChooseAdScheduleDataSet = value;	}
        }

        public string SearchMediaCode
        {
            get { return _SearchMediaCode;	}
            set { _SearchMediaCode = value;	}
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

		public string ChannelNo
		{
			get { return _ChannelNo;	}
			set { _ChannelNo = value;	}
		}

		/// <summary>
		/// 채널번호를 가져오거나 설정합니다.
		/// </summary>
		public string SeriesNo
		{
			get { return _Series;	}
			set { _Series = value;	}
		}

		public string ScheduleOrder
		{
			get { return _ScheduleOrder;	}
			set { _ScheduleOrder = value;	}
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

		public string ItemName
		{
			get { return _ItemName;	}
			set { _ItemName = value;	}
		}

		public string LastOrder
		{
			get { return _LastOrder;	}
			set { _LastOrder = value;	}
		}

		#endregion

    }
}
