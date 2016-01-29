using System;
using System.Data;

using WinFramework.Base;

namespace AdManagerModel
{
	/// <summary>
	/// CodeModel에 대한 요약 설명입니다.
	/// </summary>
	public class CodeModel : BaseModel
	{

		private string _SearchKey       = null;		// 검색어
		private string _SearchSection       = null;		// 코드구분검색

		// 요청용
		private string _Section = null;				// 코드구분
		private string _Section_old = null;				// 코드구분_OLD

		// 결과용
		private string _Code = null;				// 코드
		private string _Code_old = null;				// 코드_OLD
		private string _CodeName  = null;			// 코드명


		// 목록조회용
		private DataSet  _CodeDataSet;
	
		public CodeModel() : base()
		{
			Init();
		}

		#region Public 메소드
		public void Init()
		{
			_SearchKey	= "";
			_SearchSection	= "";

			_Section	= "";
			_Section_old	= "";
			_Code		= "";
			_Code_old		= "";
			_CodeName	= "";

			_CodeDataSet = null;
		}

		#endregion

		#region  프로퍼티 	

		public string SearchKey 
		{
			get { return _SearchKey;	}
			set { _SearchKey = value;	}
		}

		public string SearchSection 
		{
			get { return _SearchSection;	}
			set { _SearchSection = value;	}
		}

		public string Section 
		{
			get { return _Section;  }
			set { _Section = value; }
		}

		public string Section_old 
		{
			get { return _Section_old;  }
			set { _Section_old = value; }
		}

		public string Code 
		{
			get { return _Code;  }
			set { _Code = value; }
		}

		public string Code_old 
		{
			get { return _Code_old;  }
			set { _Code_old = value; }
		}

		public string CodeName 
		{
			get { return _CodeName;  }
			set { _CodeName = value; }
		}

		public DataSet CodeDataSet
		{
			get { return _CodeDataSet;	}
			set { _CodeDataSet = value;	}
		}


		#endregion

	}
}
