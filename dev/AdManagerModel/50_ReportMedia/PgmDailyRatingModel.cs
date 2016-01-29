using System;
using System.Data;

using WinFramework.Base;

namespace AdManagerModel
{
    /// <summary>
    /// PgmDailyRatingModel에 대한 요약 설명입니다.
    /// </summary>
    public class PgmDailyRatingModel : BaseModel
    {

        // 조회용
        private string _SearchMediaCode	     = null;		// 검색 매체
		private string _SearchDate           = null;		// 조회 일자

        // 목록조회용
        private DataSet  _DataSet;

        public PgmDailyRatingModel() : base () 
        {
            Init();
        }

        #region Public 메소드
        public void Init()
        {
            _SearchMediaCode 	   = "";
			_SearchDate            = "";
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

		public string SearchDate 
		{
			get { return _SearchDate;	}
			set { _SearchDate = value;	}
		}

        #endregion

    }
}