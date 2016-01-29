
using System;
using System.Data;

using WinFramework.Base;

namespace AdManagerModel
{
    /// <summary>
    /// 사용자정보의 클래스 모델.
    /// </summary>
    public class SlotAdInfoModel : BaseModel
    {
        #region 변수 선언

        // 조회용
        private string _SearchMediaCode    = null;		// 검색매체

		// 상세조회용
		private string _MediaCode     = null;
		private string _CategoryCode  = null;
		private string _MenuCode     = null;
        private int     _MaxCount = 0;		        // 무료광고의 최대갯수
        private int     _MaxTime = 0;		        // 무료광고의 최대시간
        private int     _MaxCountPay = 0;		    // 무료광고의 최대갯수
        private int     _MaxTimePay = 0;		    // 무료광고의 최대시간
        private string _UseDate = null;		        // 적용시작일시
        private string _UseYn = null;               // 사용여부
        private string _PromotionYn = null;         // 프로모션 송출 여부

        private bool _IsSetDataOnly = true;
		
        // 목록조회용
        private DataSet  _SlotAdInfoDataSet;

        #endregion 

        #region 생성자
        public SlotAdInfoModel() : base () 
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

            _UseYn = "";
            _PromotionYn = "";

            _IsSetDataOnly = true;

            _SlotAdInfoDataSet = new DataSet();
        }

        #endregion

        #region  프로퍼티 

        public DataSet SlotAdInfoDataSet
        {
            get { return _SlotAdInfoDataSet;	}
            set { _SlotAdInfoDataSet = value;	}
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

        public string PromotionYn
        {
            get { return _PromotionYn; }
            set { _PromotionYn = value; }
        }

        public string UseYn
        {
            get { return _UseYn; }
            set { _UseYn = value; }
        }

        public bool IsSetDataOnly
        {
            get { return _IsSetDataOnly; }
            set { _IsSetDataOnly = value; }
        }

        #endregion

    }
}
