using System;
using System.Data;

using WinFramework.Base;

namespace AdManagerModel
{
	/// <summary>
	/// ZipCodeModel�� ���� ��� �����Դϴ�.
	/// </summary>
	public class ZipCodeModel : BaseModel
	{
        private string	_SearchKey		= null;		// ����ڱ����ڵ�
		private string  _SearchZip     = null;

		private DataSet _DataSet;
	
		public ZipCodeModel() : base()
		{
			Init();
		}

		#region Public �޼ҵ�
		public void Init()
		{
			_SearchKey		= "";
			_DataSet		= new DataSet();
		}

		#endregion

		#region  ������Ƽ 	

		/// <summary>
		/// �����ȣ �˻����� �������ų� �����մϴ�
		/// </summary>
		public string SearchKey
		{
			get { return _SearchKey;  }
			set { _SearchKey = value; }
		}
		/// <summary>
		/// �����ȣ �˻�
		/// </summary>
		public string SearchZip
		{
			get{ return _SearchZip;}
			set{ _SearchZip = value;}
		}

		/// <summary>
		/// �˻���� DataSet�� �������ų� �����մϴ�
		/// </summary>
        public DataSet DsAddr
        {
            get { return _DataSet;	}
            set { _DataSet = value;	}
        }

		#endregion

	}
}