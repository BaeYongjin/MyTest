using System;
using System.Data;

using WinFramework.Base;

namespace AdManagerModel
{
    /// <summary>
    /// AdHitStatusModel에 대한 요약 설명입니다.
    /// 2012/02/09 조회조건 광고타입 추가함
    /// </summary>
    public class AdHitStatusModel : BaseModel
    {
        // 조회용
		private string _SearchMediaCode	    = null;		// 검색 매체
		private string _SearchRapCode	    = null;		// 검색 매체
		private string _SearchAgencyCode    = null;		// 검색 매체
		private string _SearchDay           = null;		// 조회 집계일자
		private string _SearchKey           = null;		// 조회 검색어
        private string _SearchAdType        = null;     // 검색 광고종류

        // 목록조회용
        private DataSet  _DataSet;

        public AdHitStatusModel() : base () 
        {
            Init();
        }

        #region Public 메소드
        public void Init()
        {
			_SearchMediaCode    = "";
			_SearchRapCode      = "";
			_SearchAgencyCode   = "";
			_SearchDay          = "";
			_SearchKey          = "";
            _SearchAdType       = "";
			_DataSet = new DataSet();
		}
        #endregion

        #region  프로퍼티 

        public DataSet ReportDataSet
        {
            get { return _DataSet;	}
            set { _DataSet = value;	}
        }

		public string SearchMediaCode 
		{
			get { return _SearchMediaCode;	}
			set { _SearchMediaCode = value;	}
		}

		public string SearchRapCode 
		{
			get { return _SearchRapCode;	}
			set { _SearchRapCode = value;	}
		}

		public string SearchAgencyCode 
		{
			get { return _SearchAgencyCode;	}
			set { _SearchAgencyCode = value;	}
		}

		public string SearchDay 
		{
			get { return _SearchDay;	}
			set { _SearchDay = value;	}
		}

		public string SearchKey
		{
			get { return _SearchKey;	}
			set { _SearchKey = value;	}
		}

        public string SearchAdType
        {
            get { return _SearchAdType;     }
            set { _SearchAdType = value;    }
        }
		#endregion
    }
}