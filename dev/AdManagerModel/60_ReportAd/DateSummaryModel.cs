using System;
using System.Collections;
using System.Data;

using WinFramework.Base;

namespace AdManagerModel
{
	/// <summary>
	/// DateSummaryModel에 대한 요약 설명입니다.
	/// </summary>
	public class DateSummaryModel : BaseModel
	{	

		// 조회용
		private string      _StartDay   = null;		// 조회 집계일자
		private string      _EndDay     = null;		// 조회 검색어

		private ArrayList   _AdList;
		private string[]    _AdListStr;
		private DataSet     _DataSet;
		private DataSet     _DataSet2;

		public DateSummaryModel() : base () 
		{
			Init();
		}

		#region Public 메소드
		public void Init()
		{
			_StartDay	= "";
			_EndDay     = "";
			_AdList     = new ArrayList();
			_DataSet    = new DataSet();
			_DataSet2   = new DataSet();
			_AdListStr  = new string[0];
		}
		#endregion

		#region  프로퍼티 

		public DataSet ReportDataSet
		{
			get { return _DataSet;	}
			set { _DataSet = value;	}
		}

		public DataSet HeaderDataSet
		{
			get { return _DataSet2;	}
			set { _DataSet2 = value;	}
		}

		public string StartDay
		{
			get { return _StartDay;	}
			set { _StartDay = value;	}
		}

		public string EndDay
		{
			get { return _EndDay;	}
			set { _EndDay = value;	}
		}

		/// <summary>
		/// 선택광고 리스트
		/// </summary>
		public ArrayList    AdList
		{
			get { return _AdList;	}
			set { _AdList = value;	}
		}
		
		public string[] AdListStr
		{
			get { return _AdListStr;	}
			set { _AdListStr = value;	}
		}

		#endregion

	}
}