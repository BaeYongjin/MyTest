using System;
using System.Data;

using WinFramework.Base;

namespace AdManagerModel 
{
	/// <summary>
	/// RptSummaryAdDailyModel에 대한 요약 설명입니다.
	/// </summary>
	public class RptSummaryAdDailyModel : BaseModel
	{
		// 조회용
		private string _SearchDay            = null;		// 조회 집계일자(종료)

		// 목록조회용
		private DataSet  _DataSet;

		public RptSummaryAdDailyModel() : base()
		{
			Init();
		}

		#region Public 메소드
		public void Init()
		{
			_SearchDay				= "";
			
			_DataSet = new DataSet();
		}
		#endregion

		#region  프로퍼티 
		public DataSet RptDailyDataSet
		{
			get { return _DataSet;	}
			set { _DataSet = value;	}
		}

		public string SearchDay 
		{
			get { return _SearchDay;	}
			set { _SearchDay = value;	}
		}
		#endregion

	}
}
