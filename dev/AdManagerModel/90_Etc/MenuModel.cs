using System;
using System.Data;

using WinFramework.Base;

namespace AdManagerModel
{
	/// <summary>
	/// MenuModel�� ���� ��� �����Դϴ�.
	/// </summary>
	public class MenuModel : BaseModel
	{
		private string _MenuCode      = null;		// �޴��ڵ�
		private string _MenuLevel     = null;		// �޴�����
		private string _UpperMenu     = null;		// �����޴�
		private string _MenuName      = null;		// �޴���
		private string _MenuOrder     = null;		// �޴�����
        private string _UseYn         = null;		// ��뿩��
      
        private string _UserClassCode = null;		// ����ڱ����ڵ�
        private string _UserClassName  = null;		// ����ڱ����ڵ��
        
        private string _CheckCreate   = null;       // �������� ����
        private string _CheckRead     = null;       // ��ȸ���� ����
        private string _CheckUpdate   = null;       // �������� ����
        private string _CheckDelete   = null;       // �������� ����


		// �����ȸ��
		private DataSet  _DataSet;
	
		public MenuModel() : base()
		{
			Init();
		}

		#region Public �޼ҵ�
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

		#region  ������Ƽ 	

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