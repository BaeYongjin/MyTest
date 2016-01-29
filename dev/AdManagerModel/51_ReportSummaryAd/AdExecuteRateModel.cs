using System;
using System.Data;

using WinFramework.Base;

namespace AdManagerModel
{
	/// <summary>
	/// AdTypeMoniteringModel에 대한 요약 설명입니다.
	/// </summary>
	public class AdExecuteRateModel : BaseModel
	{
		// 조회용
		private string _StartDay	=   null;
		private string _EndDay	=   null;
		
		// 목록조회용
		private DataSet  _DataSet;
		
		public AdExecuteRateModel() : base () 
		{
			Init();
		}

		#region Public 메소드
		public void Init()
		{
			_StartDay 	= "";
			_EndDay	= "";
			_DataSet	= new DataSet();
		}

		#endregion

		#region  프로퍼티 

		public DataSet ReportDataSet
		{
			get { return _DataSet;	}
			set { _DataSet = value;	}
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

		#endregion

	}
}