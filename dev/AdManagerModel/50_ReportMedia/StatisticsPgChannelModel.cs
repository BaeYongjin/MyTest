using System;
using System.Data;

using WinFramework.Base;

namespace AdManagerModel
{
	/// <summary>
	/// StatisticsPgChannelModel에 대한 요약 설명입니다.
	/// </summary>
	public class StatisticsPgChannelModel : BaseModel
	{

		// 조회용
		private string _SearchMediaCode	     = null;		// 검색 매체
		private string _SearchType           = null;		// 조회 구분 B:선택기간 D:일간 W:주간 M:월간
		private string _SearchStartDay       = null;		// 조회 집계시작 일자
		private string _SearchEndDay         = null;		// 조회 집계시작 일자

		// 목록조회용
		private DataSet  _DataSet;

		public StatisticsPgChannelModel() : base () 
		{
			Init();
		}

		#region Public 메소드
		public void Init()
		{
			_SearchMediaCode 	   = "";
			_SearchType            = "";
			_SearchStartDay          = "";
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

		public string SearchStartDay 
		{
			get { return _SearchStartDay;	}
			set { _SearchStartDay = value;	}
		}

		public string SearchEndDay 
		{
			get { return _SearchEndDay;	}
			set { _SearchEndDay = value;	}
		}

		#endregion

	}
}