using System;
using System.Data;

using WinFramework.Base;

namespace AdManagerModel
{
	/// <summary>
	/// AdTypeMoniteringModel에 대한 요약 설명입니다.
	/// </summary>
	public class AdTypeMoniteringModel : BaseModel
	{
		// 조회용
		private string _LogDay	=   null;
		private string _AdType	=   null;
        private string _Rap     =   null;
		
		// 목록조회용
		private DataSet  _DataSet;
		
		// 광고내역용
		private DataSet _DataSet2;

		public AdTypeMoniteringModel() : base () 
		{
			Init();
		}

		#region Public 메소드
		public void Init()
		{
			_LogDay 	= "";
			_AdType 	= "";
            _Rap        = "";
			_DataSet  = new DataSet();
			_DataSet2 = new DataSet();			
		}

		#endregion

		#region  프로퍼티 

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

		public string LogDay 
		{
			get { return _LogDay;	}
			set { _LogDay = value;	}
		}

		public string AdType 
		{
			get { return _AdType;	}
			set { _AdType = value;	}
		}

        /// <summary>
        /// 미디어렙코드를 설정하거나 가져옵니다
        /// </summary>
        public string Rap
        {
            get { return _Rap;      }
            set { _Rap  = value;    }
        }
	
		#endregion

	}
}