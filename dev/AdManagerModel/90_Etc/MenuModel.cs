using System;
using System.Data;

using WinFramework.Base;

namespace AdManagerModel
{
	/// <summary>
	/// MenuModel에 대한 요약 설명입니다.
	/// </summary>
	public class MenuModel : BaseModel
	{
		private string _MenuCode      = null;		// 메뉴코드
		private string _MenuLevel     = null;		// 메뉴레벨
		private string _UpperMenu     = null;		// 상위메뉴
		private string _MenuName      = null;		// 메뉴명
		private string _MenuOrder     = null;		// 메뉴순서
        private string _UseYn         = null;		// 사용여부
      
        private string _UserClassCode = null;		// 사용자구분코드
        private string _UserClassName  = null;		// 사용자구분코드명
        
        private string _CheckCreate   = null;       // 생성권한 여부
        private string _CheckRead     = null;       // 조회권한 여부
        private string _CheckUpdate   = null;       // 수정권한 여부
        private string _CheckDelete   = null;       // 삭제권한 여부


		// 목록조회용
		private DataSet  _DataSet;
	
		public MenuModel() : base()
		{
			Init();
		}

		#region Public 메소드
		public void Init()
		{
			_MenuCode		= "";
			_MenuLevel		= "";
			_UpperMenu		= "";
			_MenuName		= "";
			_MenuOrder		= "";
			_UseYn			= "";

			_DataSet		= null;

            _UserClassCode  = ""; 
            _UserClassName   = "";

            _MenuCode       = "";
            _MenuName       = "";
            _CheckCreate    = "";
            _CheckRead      = "";
            _CheckUpdate    = "";
            _CheckDelete    = "";
        
		}

		#endregion

		#region  프로퍼티 	

		public string MenuCode 
		{
			get { return _MenuCode;  }
			set { _MenuCode = value; }
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

        public DataSet MenuDataSet
        {
            get { return _DataSet;	}
            set { _DataSet = value;	}
        }

        public string UserClassCode
        {
            get { return _UserClassCode;	}
            set { _UserClassCode = value;	}
        }

        public string UserClassName
        {
            get { return _UserClassName;	}
            set { _UserClassName = value;	}
        }


        public string CheckCreate
        {
            get { return _CheckCreate;	}
            set { _CheckCreate = value;	}
        }

        public string CheckRead
        {
            get { return _CheckRead;	}
            set { _CheckRead = value;	}
        }

        public string CheckUpdate
        {
            get { return _CheckUpdate;	}
            set { _CheckUpdate = value;	}
        }

        public string CheckDelete
        {
            get { return _CheckDelete;	}
            set { _CheckDelete = value;	}
        }

        

		#endregion

	}
}