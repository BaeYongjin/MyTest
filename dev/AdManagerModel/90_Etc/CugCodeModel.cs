using System;
using System.Data;

using WinFramework.Base;

namespace AdManagerModel
{
	/// <summary>
	/// CugCodeModel에 대한 요약 설명입니다.
	/// </summary>
	public class CugCodeModel : BaseModel
	{

		private string _SearchKey       = null;		// 검색어
		
		// 결과용
		private string _CugCode = null;				// 코드
		private string _CugName  = null;			// 코드명


		// 목록조회용
		private DataSet  _DataSet;
	
		public CugCodeModel() : base()
		{
			Init();
		}

		#region Public 메소드
		public void Init()
		{			
			_SearchKey	= "";
			_CugCode	= "";
			_CugName	= "";

			_DataSet = null;
		}

		#endregion

		#region  프로퍼티 	

		public string SearchKey 
		{
			get { return _SearchKey;	}
			set { _SearchKey = value;	}
		}

		public string CugCode 
		{
			get { return _CugCode;  }
			set { _CugCode = value; }
		}

		public string CugName 
		{
			get { return _CugName;  }
			set { _CugName = value; }
		}

		public DataSet CugCodeDataSet
		{
			get { return _DataSet;	}
			set { _DataSet = value;	}
		}


		#endregion

	}
}
