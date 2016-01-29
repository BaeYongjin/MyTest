using System;
using System.Data;

using WinFramework.Base;

namespace AdManagerModel
{
	/// <summary>
	/// AgencyCodeModel에 대한 요약 설명입니다.
	/// </summary>
	public class AgencyCodeModel : BaseModel
	{

		private string _SearchKey       = null;		// 검색어
		private string _SearchRap       = null;		// 검색 미디어렙
		
		// 결과용
		private string _AgencyCode = null;				// 코드
		private string _AgencyName  = null;			// 코드명


		// 목록조회용
		private DataSet  _AgencyCodeDataSet;
	
		public AgencyCodeModel() : base()
		{
			Init();
		}

		#region Public 메소드
		public void Init()
		{			
			_SearchKey			= "";
			_SearchRap			= "";
			_AgencyCode		= "";
			_AgencyName	= "";

			_AgencyCodeDataSet = null;
		}

		#endregion

		#region  프로퍼티 	

		public string SearchKey 
		{
			get { return _SearchKey;	}
			set { _SearchKey = value;	}
		}

		public string SearchRap 
		{
			get { return _SearchRap;	}
			set { _SearchRap = value;	}
		}

		public string AgencyCode 
		{
			get { return _AgencyCode;  }
			set { _AgencyCode = value; }
		}

		public string AgencyName 
		{
			get { return _AgencyName;  }
			set { _AgencyName = value; }
		}

		public DataSet AgencyCodeDataSet
		{
			get { return _AgencyCodeDataSet;	}
			set { _AgencyCodeDataSet = value;	}
		}


		#endregion

	}
}
