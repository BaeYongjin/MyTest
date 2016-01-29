using System;
using System.Data;

using WinFramework.Base;

namespace AdManagerModel
{
	/// <summary>
	/// InventoryPresentConditionModel에 대한 요약 설명입니다.
	/// </summary>
	public class InventoryPresentConditionModel : BaseModel
	{
		// 조회용
		private string _LogDay1			     = null;		// 검색 매체
		private string _LogDay2			     = null;		// 검색 매체
		private int    _SearchType			 = 0;		// 검색 종류 1 : 3개월평균, 2 : 1년최저
				
		// 목록조회용
		private DataSet  _DataSet;
		
		// 광고내역용
		private DataSet _DataSet2;

		public InventoryPresentConditionModel() : base () 
		{
			Init();
		}

		#region Public 메소드
		public void Init()
		{
			_LogDay1 	   = "";
			_LogDay2 	   = "";
			_SearchType    = 0;
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

		public string LogDay1 
		{
			get { return _LogDay1;	}
			set { _LogDay1 = value;	}
		}

		public string LogDay2 
		{
			get { return _LogDay2;	}
			set { _LogDay2 = value;	}
		}

		public int SearchType
		{
			get { return _SearchType;	}
			set { _SearchType = value;	}
		}
			
		#endregion

	}
}