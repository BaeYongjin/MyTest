using System;
using System.Data;

using WinFramework.Base;

namespace AdManagerModel
{
	/// <summary>
	/// RegionDateModel�� ���� ��� �����Դϴ�.
	/// </summary>
	public class RegionDateModel : BaseModel
	{	

		// ��ȸ��
		private string _ItemNo1     = null;		// �˻� ��ü
		private string _ItemNo2     = null;		// �˻� ��ü
		private string _ItemNo3     = null;		// �˻� ��ü
		private string _ItemNo4     = null;		// ��ȸ ��������
		private string _ItemNo5     = null;		// ��ȸ �˻���
		private string _ItemNo6     = null;		// �˻� ��ü
		private string _ItemNo7     = null;		// �˻� ��ü
		private string _ItemNo8     = null;		// �˻� ��ü
		private string _ItemNo9     = null;		// ��ȸ ��������
		private string _ItemNo10    = null;		// ��ȸ �˻���
		private string _StartDay    = null;		// ��ȸ ��������
		private string _EndDay      = null;		// ��ȸ �˻���

		private string _ContractSeq    = null;		// ��ȸ �˻���

		// �����ȸ��
		private DataSet  _DataSet;
		private DataSet  _DataSet2;

		public RegionDateModel() : base () 
		{
			Init();
		}

		#region Public �޼ҵ�
		public void Init()
		{
			_ItemNo1 	   = "";
			_ItemNo2       = "";
			_ItemNo3  	   = "";
			_ItemNo4       = "";
			_ItemNo5       = "";		
			_ItemNo6 	   = "";
			_ItemNo7       = "";
			_ItemNo8  	   = "";
			_ItemNo9       = "";
			_ItemNo10      = "";		
			_StartDay	   = "";
			_EndDay        = "";

			_ContractSeq      = "";		
			
			_DataSet = new DataSet();
			_DataSet2 = new DataSet();
		}

		#endregion

		#region  ������Ƽ 

		public DataSet ReportDataSet
		{
			get { return _DataSet;	}
			set { _DataSet = value;	}
		}

		public DataSet HeaderDataSet
		{
			get { return _DataSet2;	}
			set { _DataSet2 = value;	}
		}

		public string ItemNo1 
		{
			get { return _ItemNo1;	}
			set { _ItemNo1 = value;	}
		}

		public string ItemNo2 
		{
			get { return _ItemNo2;	}
			set { _ItemNo2 = value;	}
		}

		public string ItemNo3 
		{
			get { return _ItemNo3;	}
			set { _ItemNo3 = value;	}
		}

		public string ItemNo4 
		{
			get { return _ItemNo4;	}
			set { _ItemNo4 = value;	}
		}

		public string ItemNo5
		{
			get { return _ItemNo5;	}
			set { _ItemNo5 = value;	}
		}

		public string ItemNo6
		{
			get { return _ItemNo6;	}
			set { _ItemNo6 = value;	}
		}

		public string ItemNo7
		{
			get { return _ItemNo7;	}
			set { _ItemNo7 = value;	}
		}

		public string ItemNo8
		{
			get { return _ItemNo8;	}
			set { _ItemNo8 = value;	}
		}

		public string ItemNo9
		{
			get { return _ItemNo9;	}
			set { _ItemNo9 = value;	}
		}

		public string ItemNo10
		{
			get { return _ItemNo10;	}
			set { _ItemNo10 = value;	}
		}

		public string StartDay
		{
			get { return _StartDay;	}
			set { _StartDay = value;	}
		}

		public string EndDay
		{
			get { return _EndDay;	}
			set { _EndDay = value;	}
		}

		public string ContractSeq
		{
			get { return _ContractSeq;	}
			set { _ContractSeq = value;	}
		}

		#endregion

	}
}