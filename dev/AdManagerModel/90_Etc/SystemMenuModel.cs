using System;
using System.Data;

using WinFramework.Base;

namespace AdManagerModel
{
	/// <summary>
	/// SystemMenuModel에 대한 요약 설명입니다.
	/// </summary>
	public class SystemMenuModel : BaseModel
	{

		private string _SearchKey       = null;		// 검색어
		private string _SearchMenuCode       = null;		// 코드구분검색

		// 요청용
		private string _MenuCode = null;				// 메뉴코드		
		private string _MenuCode_2 = null;				// 메뉴코드		
		private string _MenuCode_3 = null;				// 메뉴코드		
		private string _MenuLevel = null;				// 레벨

		// 결과용
		private string _UpperMenu = null;				// 상위메뉴
		private string _MenuName = null;				// 메뉴명
		private string _UpperName = null;				// 상위메뉴명
		private string _MenuOrder  = null;			// 순서
		private string _UseYn  = null;			// 사용여부

		// 마지막 순서
		private string _LastOrder     = null;

		// 목록조회용
		private DataSet  _SystemMenuDataSet;
	
		public SystemMenuModel() : base()
		{
			Init();
		}

		#region Public 메소드
		public void Init()
		{
			_SearchKey	= "";
			_SearchMenuCode	= "";

			_MenuCode	= "";
			_MenuCode_2	= "";
			_MenuCode_3	= "";
			_MenuLevel	= "";
			_UpperMenu		= "";
			_MenuName		= "";
			_UpperName		= "";
			_MenuOrder	= "";
			_UseYn	= "";

			_LastOrder		   = "";

			_SystemMenuDataSet = null;
		}

		#endregion

		#region  프로퍼티 	

		public string SearchKey 
		{
			get { return _SearchKey;	}
			set { _SearchKey = value;	}
		}

		public string SearchMenuCode 
		{
			get { return _SearchMenuCode;	}
			set { _SearchMenuCode = value;	}
		}

		public string MenuCode 
		{
			get { return _MenuCode;  }
			set { _MenuCode = value; }
		}

		public string MenuCode_2 
		{
			get { return _MenuCode_2;  }
			set { _MenuCode_2 = value; }
		}

		public string MenuCode_3 
		{
			get { return _MenuCode_3;  }
			set { _MenuCode_3 = value; }
		}

		public string MenuLevel 
		{
			get { return _MenuLevel;  }
			set { _MenuLevel = value; }
		}

		public string UpperMenu 
		{
			get { return _UpperMenu;  }
			set { _UpperMenu = value; }
		}

		public string MenuName 
		{
			get { return _MenuName;  }
			set { _MenuName = value; }
		}

		public string UpperName 
		{
			get { return _UpperName;  }
			set { _UpperName = value; }
		}

		public string MenuOrder 
		{
			get { return _MenuOrder;  }
			set { _MenuOrder = value; }
		}

		public string UseYn 
		{
			get { return _UseYn;  }
			set { _UseYn = value; }
		}

		public string LastOrder
		{
			get { return _LastOrder;		}
			set { _LastOrder = value;		}
		}

		public DataSet SystemMenuDataSet
		{
			get { return _SystemMenuDataSet;	}
			set { _SystemMenuDataSet = value;	}
		}


		#endregion

	}
}
