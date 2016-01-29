using System;
using System.Data;

using WinFramework.Base;

namespace AdManagerModel
{
	/// <summary>
	/// CugCodeModel�� ���� ��� �����Դϴ�.
	/// </summary>
	public class CugCodeModel : BaseModel
	{

		private string _SearchKey       = null;		// �˻���
		
		// �����
		private string _CugCode = null;				// �ڵ�
		private string _CugName  = null;			// �ڵ��


		// �����ȸ��
		private DataSet  _DataSet;
	
		public CugCodeModel() : base()
		{
			Init();
		}

		#region Public �޼ҵ�
		public void Init()
		{			
			_SearchKey	= "";
			_CugCode	= "";
			_CugName	= "";

			_DataSet = null;
		}

		#endregion

		#region  ������Ƽ 	

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
