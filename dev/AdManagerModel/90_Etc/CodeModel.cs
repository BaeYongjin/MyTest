using System;
using System.Data;

using WinFramework.Base;

namespace AdManagerModel
{
	/// <summary>
	/// CodeModel�� ���� ��� �����Դϴ�.
	/// </summary>
	public class CodeModel : BaseModel
	{

		private string _SearchKey       = null;		// �˻���
		private string _SearchSection       = null;		// �ڵ屸�а˻�

		// ��û��
		private string _Section = null;				// �ڵ屸��
		private string _Section_old = null;				// �ڵ屸��_OLD

		// �����
		private string _Code = null;				// �ڵ�
		private string _Code_old = null;				// �ڵ�_OLD
		private string _CodeName  = null;			// �ڵ��


		// �����ȸ��
		private DataSet  _CodeDataSet;
	
		public CodeModel() : base()
		{
			Init();
		}

		#region Public �޼ҵ�
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

		#region  ������Ƽ 	

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
