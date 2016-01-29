using System;
using System.Data;

using WinFramework.Base;

namespace AdManagerModel
{
	/// <summary>
	/// SystemMenuModel�� ���� ��� �����Դϴ�.
	/// </summary>
	public class SystemMenuModel : BaseModel
	{

		private string _SearchKey       = null;		// �˻���
		private string _SearchMenuCode       = null;		// �ڵ屸�а˻�

		// ��û��
		private string _MenuCode = null;				// �޴��ڵ�		
		private string _MenuCode_2 = null;				// �޴��ڵ�		
		private string _MenuCode_3 = null;				// �޴��ڵ�		
		private string _MenuLevel = null;				// ����

		// �����
		private string _UpperMenu = null;				// �����޴�
		private string _MenuName = null;				// �޴���
		private string _UpperName = null;				// �����޴���
		private string _MenuOrder  = null;			// ����
		private string _UseYn  = null;			// ��뿩��

		// ������ ����
		private string _LastOrder     = null;

		// �����ȸ��
		private DataSet  _SystemMenuDataSet;
	
		public SystemMenuModel() : base()
		{
			Init();
		}

		#region Public �޼ҵ�
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

		#region  ������Ƽ 	

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
