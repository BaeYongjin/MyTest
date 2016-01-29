using System;
using System.Data;

using WinFramework.Base;

namespace AdManagerModel
{
    /// <summary>
    /// SummaryAdModel에 대한 요약 설명입니다.
    /// </summary>
    public class HouseHoldAccountModel : BaseModel
    {

        // 조회용
        private string _SearchMediaCode	     = null;		// 검색 매체
		private string _SearchContractSeq    = null;		// 조회 계약번호
		private string _SearchItemNo         = null;		// 조회 광고번호 ''이면 계약건 전체
		private string _SearchType           = null;		// 조회 구분 T:총기간 D:일간 W:주간 M:월간
		private string _SearchStartDay       = null;		// 조회 집계시작 일자
		private string _SearchEndDay         = null;		// 조회 집계시작 일자
		private string _CampaignCode         = null;

        // 목록조회용
        private DataSet  _DataSet;
		
		// 광고내역용
		private DataSet _DataSet2;

		// 조회 이용자수
		private int		_TotalUser	= 0;

        public HouseHoldAccountModel() : base () 
        {
            Init();
        }

        #region Public 메소드
        public void Init()
        {
            _SearchMediaCode 	   = "";
			_SearchContractSeq     = "";
			_SearchItemNo          = "";
			_SearchType            = "";
			_SearchStartDay          = "";
			_SearchEndDay          = "";
			_CampaignCode          = "";
			_DataSet  = new DataSet();
			_DataSet2 = new DataSet();

			_TotalUser = 0;
		}

        #endregion

        #region  프로퍼티 

		public int TotalUser
		{
			get { return _TotalUser;	}
			set { _TotalUser = value;	}
		}

        public DataSet ReportDataSet
        {
            get { return _DataSet;	}
            set { _DataSet = value;	}
        }

		public DataSet ItemDataSet
		{
			get { return _DataSet2;	}
			set { _DataSet2 = value;	}
		}

		public string SearchMediaCode 
		{
			get { return _SearchMediaCode;	}
			set { _SearchMediaCode = value;	}
		}

		public string SearchContractSeq
		{
			get { return _SearchContractSeq;	}
			set { _SearchContractSeq = value;	}
		}

		public string SearchItemNo 
		{
			get { return _SearchItemNo;	}
			set { _SearchItemNo = value;	}
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

		public string CampaignCode 
		{
			get { return _CampaignCode;	}
			set { _CampaignCode = value;	}
		}

		#endregion

    }
}