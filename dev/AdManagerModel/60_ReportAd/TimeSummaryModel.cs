using System;
using System.Collections;
using System.Data;

using WinFramework.Base;

namespace AdManagerModel
{
	/// <summary>
	/// TimeSummaryModel에 대한 요약 설명입니다.
	/// </summary>
	public class TimeSummaryModel : BaseModel
	{	
		// 조회용
		private string      _StartDay    = null;		// 조회 집계일자
		private string      _EndDay      = null;		// 조회 검색어
		private ArrayList   _AdList;
		private string[]	_AdListStr	= null;
		private DataSet     _DataSet;

		public TimeSummaryModel() : base () 
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
			_AdListStr		= new string[0];
		}

		#endregion

		#region  프로퍼티 

		public DataSet ReportDataSet
		{
			get { return _DataSet;	}
			set { _DataSet = value;	}
		}

		/// <summary>
		/// 선택광고 리스트
		/// </summary>
		public ArrayList    AdList
		{
			get { return _AdList;	}
			set { _AdList = value;	}
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

		public string[]	AdListStr
		{
			get { return _AdListStr;	}
			set { _AdListStr = value;	}
		}
		#endregion
	}
}