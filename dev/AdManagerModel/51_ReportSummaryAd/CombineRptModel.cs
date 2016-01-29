using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

using WinFramework.Base;

namespace AdManagerModel
{
    /// <summary>
    /// DailyAdHitModel에 대한 요약 설명입니다.
    /// </summary>
    public class CombineRptModel : BaseModel
    {

        // 조회용
        private string _SearchMediaCode	     = null;		// 검색 매체
		private string _SearchContractSeq    = null;		// 조회 계약 번호
        private string _CampaignCode         = null;        // 조회 캠페인 번호
		private string _SearchItemNo         = null;		// 조회 광고번호
		private string _SearchType           = null;		// 조회 구분 D:선택기간 C:계약기간
		private string _SearchBgnDay         = null;		// 조회 집계 시작일자
		private string _SearchEndDay         = null;		// 조회 집계 종료일자	
		
        // 목록조회용
        private DataSet  _DataSet;

        public CombineRptModel() : base () 
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
			_SearchBgnDay          = "";
			_SearchEndDay          = "";
			_CampaignCode          = "";
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

		public string SearchContractSeq
		{
			get { return _SearchContractSeq;	}
			set { _SearchContractSeq = value;	}
		}

        public string CampaignCode
        {
            get { return _CampaignCode; }
            set { _CampaignCode = value; }
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

		public string SearchBgnDay 
		{
			get { return _SearchBgnDay;	}
			set { _SearchBgnDay = value;	}
		}

		public string SearchEndDay
		{
			get { return _SearchEndDay;	}
			set { _SearchEndDay = value;	}
		}
		#endregion

    }
}