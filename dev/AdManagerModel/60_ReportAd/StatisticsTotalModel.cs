using System;
using System.Data;

using WinFramework.Base;

namespace AdManagerModel
{
	/// <summary>
	/// StatisticsTotalModel에 대한 요약 설명입니다.
	/// </summary>
	public class StatisticsTotalModel : BaseModel
	{

		// 조회용
		private string _SearchMediaCode	     = null;		// 검색 매체
		private string _SearchRapCode	     = null;		// 검색 매체
		private string _SearchAgencyCode     = null;		// 검색 매체
		private string _SearchKey            = null;		// 조회 검색어

		// 목록조회용
		private DataSet  _DataSet;

		public StatisticsTotalModel() : base () 
		{
			Init();
		}

		#region Public 메소드
		public void Init()
		{
			_SearchMediaCode 	   = "";
			_SearchRapCode         = "";
			_SearchAgencyCode 	   = "";
			_SearchKey             = "";				
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

		public string SearchRapCode 
		{
			get { return _SearchRapCode;	}
			set { _SearchRapCode = value;	}
		}

		public string SearchAgencyCode 
		{
			get { return _SearchAgencyCode;	}
			set { _SearchAgencyCode = value;	}
		}

		public string SearchKey
		{
			get { return _SearchKey;	}
			set { _SearchKey = value;	}
		}

		#endregion

	}
}