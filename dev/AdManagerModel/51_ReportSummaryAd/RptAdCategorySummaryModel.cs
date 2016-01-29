using System;
using System.Data;

using WinFramework.Base;

namespace AdManagerModel
{
	/// <summary>
	/// RptAdCategorySummaryModel에 대한 요약 설명입니다.
	/// </summary>
	public class RptAdCategorySummaryModel : BaseModel
	{
		// 조회용
		private string _LogDay			     = null;		// 검색 매체
		private string _LogDayEnd			     = null;		// 검색 매체
		private string _AdType			     = null;		// 검색 매체
		
		// 목록조회용
		private DataSet  _DataSet;
		
		// 광고내역용
		private DataSet _DataSet2;

		public RptAdCategorySummaryModel() : base () 
		{
			Init();
		}

		#region Public 메소드
		public void Init()
		{
			_LogDay 	   = "";			
			_LogDayEnd 	   = "";			
			_AdType 	   = "";			
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

		public string LogDayEnd 
		{
			get { return _LogDayEnd;	}
			set { _LogDayEnd = value;	}
		}

		public string AdType 
		{
			get { return _AdType;	}
			set { _AdType = value;	}
		}
	
		#endregion

	}
}