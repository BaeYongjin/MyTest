
using System;
using System.Data;

using WinFramework.Base;

namespace AdManagerModel
{
    /// <summary>
    /// 사용자정보의 클래스 모델.
    /// </summary>
    public class SchOrgGenreModel : BaseModel
    {
        #region 변수 선언

        // 조회용
        private string _SearchMediaCode    = null;		// 검색매체
        private string _SearchKey = null;

		// 상세조회용
		private string _MediaCode     = null;
		private string _CategoryCode  = null;
		private string _MenuCode     = null;
        private int     _MaxCount = 0;		        // 무료광고의 최대갯수
        private int     _MaxTime = 0;		        // 무료광고의 최대시간
        private int     _MaxCountPay = 0;		    // 무료광고의 최대갯수
        private int     _MaxTimePay = 0;		    // 무료광고의 최대시간
        private string _UseDate = null;		        // 적용시작일시

        private bool _IsSetDataOnly = true;
		
        // 목록조회용
        private DataSet  _SchOrgGenreDataSet;

        #endregion 

        #region 생성자
        public SchOrgGenreModel() : base () 
        {
            Init();
        }
        #endregion
 
        #region Public 메소드
        public void Init()
        {			
            _SearchMediaCode = "";

			_MediaCode      = "";
			_CategoryCode   = "";
			_MenuCode      = "";
            _MaxCount = 0;
            _MaxTime = 0;
            _MaxCountPay = 0;
            _MaxTimePay = 0;
            _UseDate = "";
            _IsSetDataOnly = true;
            _SearchKey = "";

            _SchOrgGenreDataSet = new DataSet();
        }

        #endregion

        #region  프로퍼티 

        public DataSet SchOrgGenreDataSet
        {
            get { return _SchOrgGenreDataSet;	}
            set { _SchOrgGenreDataSet = value;	}
        }

        public string SearchMediaCode
        {
            get { return _SearchMediaCode;	}
            set { _SearchMediaCode = value;	}
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

		public string MenuCode
		{
			get { return _MenuCode;	}
			set { _MenuCode = value;	}
		}
        
        public int MaxCount
		{
			get { return _MaxCount;	}
			set { _MaxCount = value;	}
		}
        
        public int MaxTime
		{
			get { return _MaxTime;	}
			set { _MaxTime = value;	}
		}

        public int MaxCountPay
		{
			get { return _MaxCountPay;	}
			set { _MaxCountPay = value;	}
		}
        
        public int MaxTimePay
		{
			get { return _MaxTimePay;	}
			set { _MaxTimePay = value;	}
		}

        public string UseDate
        {
            get { return _UseDate; }
            set { _UseDate = value; }
        }

        public bool IsSetDataOnly
        {
            get { return _IsSetDataOnly; }
            set { _IsSetDataOnly = value; }
        }

        public string SearchKey
        {
            get { return _SearchKey; }
            set { _SearchKey = value; }
        }
	
        #endregion

    }
}
