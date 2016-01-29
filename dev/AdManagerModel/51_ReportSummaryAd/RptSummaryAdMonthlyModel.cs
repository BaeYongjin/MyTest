using System;
using System.Data;

using WinFramework.Base;

namespace AdManagerModel
{
	/// <summary>
	/// RptSummaryAdMonthlyModel에 대한 요약 설명입니다.
	/// </summary>
	public class RptSummaryAdMonthlyModel : BaseModel
	{
		//조회용
		private string _SearchDay = null;			//조회 집계날짜		 

		//목록조회용
		private DataSet _DataSet = null;

		public RptSummaryAdMonthlyModel() : base()
		{
			Init();	

		}

		#region public Init()
		public void Init()
		{
			_SearchDay = "";
			_DataSet = new DataSet();
		}
		#endregion

		#region 프로퍼티
		public DataSet RptMonthlyDataSet
		{
			get {return _DataSet; }
			set {_DataSet = value; }
		}

		public string SearchDay
		{
			get { return _SearchDay; }
			set { _SearchDay = value; }
		}

		#endregion
	}
}
