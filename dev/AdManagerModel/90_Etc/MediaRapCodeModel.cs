using System;
using System.Data;

using WinFramework.Base;

namespace AdManagerModel
{
	/// <summary>
	/// MediaRapCodeModel에 대한 요약 설명입니다.
	/// </summary>
	public class MediaRapCodeModel : BaseModel
	{
		private string _SearchKey       = null;		// 검색어
		
		// 결과용
		private string _RapCode = null;				// 코드
		private string _RapName  = null;			// 코드명


		// 목록조회용
		private DataSet  _MediaRapCodeDataSet;
	
		public MediaRapCodeModel() : base()
		{
			Init();
		}

		#region Public 메소드
		public void Init()
		{			
			_SearchKey			= "";
			_RapCode		= "";
			_RapName	= "";

			_MediaRapCodeDataSet = null;
		}

		#endregion

		#region  프로퍼티 	

		public string SearchKey 
		{
			get { return _SearchKey;	}
			set { _SearchKey = value;	}
		}

		public string RapCode 
		{
			get { return _RapCode;  }
			set { _RapCode = value; }
		}

		public string RapName 
		{
			get { return _RapName;  }
			set { _RapName = value; }
		}

		public DataSet MediaRapCodeDataSet
		{
			get { return _MediaRapCodeDataSet;	}
			set { _MediaRapCodeDataSet = value;	}
		}


		#endregion

	}
}
