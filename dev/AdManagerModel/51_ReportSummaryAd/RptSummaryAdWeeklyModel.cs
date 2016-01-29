using System;
using System.Data;

using WinFramework.Base;

namespace AdManagerModel
{
	/// <summary>
	/// RptSummaryAdWeeklyModel에 대한 요약 설명입니다.
	/// </summary>
	public class RptSummaryAdWeeklyModel : BaseModel
	{
		//조회용
		private string _SearchStartDay       = null;		// 조회 집계일자(시작)
		private string _SearchDay = null;				//조회 집계일자

		//목록조회용
		private DataSet _DataSet;

		public RptSummaryAdWeeklyModel() : base()
		{
			Init();
		}

		#region public Init()
		public void Init()
		{
			_SearchStartDay	= "";
			_SearchDay = "";

			_DataSet = new DataSet();
		}
		#endregion

		#region 프로퍼티
		public DataSet RptWeeklyDataSet
		{
			get { return _DataSet;	}
			set { _DataSet = value;	}
		}

		public string SearchStartDay 
		{
			get { return _SearchStartDay;	}
			set { _SearchStartDay = value;	}
		}

		public string SearchDay 
		{
			get { return _SearchDay;	}
			set { _SearchDay = value;	}
		}
		#endregion
	}
}
