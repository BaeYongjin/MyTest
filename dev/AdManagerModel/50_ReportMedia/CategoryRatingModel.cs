using System;
using System.Data;

using WinFramework.Base;

namespace AdManagerModel
{
    /// <summary>
    /// CategoryRating에 대한 요약 설명입니다.
    /// </summary>
    public class CategoryRatingModel : BaseModel
    {

        // 조회용
        private string _SearchMediaCode	     = null;		// 검색 매체
		private string _SearchType           = null;		// 조회 구분 D:일간 W:주간 M:월간
		private string _SearchBgnDay         = null;		// 조회 집계 시작일자
		private string _SearchEndDay         = null;		// 조회 집계 종료일자	

        // 목록조회용
        private DataSet  _DataSet;

        public CategoryRatingModel() : base () 
        {
            Init();
        }

        #region Public 메소드
        public void Init()
        {
            _SearchMediaCode 	   = "";
			_SearchType            = "";
			_SearchBgnDay          = "";
			_SearchEndDay          = "";
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