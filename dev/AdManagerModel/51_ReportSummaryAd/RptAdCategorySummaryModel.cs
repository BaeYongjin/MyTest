using System;
using System.Data;

using WinFramework.Base;

namespace AdManagerModel
{
	/// <summary>
	/// RptAdCategorySummaryModel�� ���� ��� �����Դϴ�.
	/// </summary>
	public class RptAdCategorySummaryModel : BaseModel
	{
		// ��ȸ��
		private string _LogDay			     = null;		// �˻� ��ü
		private string _LogDayEnd			     = null;		// �˻� ��ü
		private string _AdType			     = null;		// �˻� ��ü
		
		// �����ȸ��
		private DataSet  _DataSet;
		
		// ��������
		private DataSet _DataSet2;

		public RptAdCategorySummaryModel() : base () 
		{
			Init();
		}

		#region Public �޼ҵ�
		public void Init()
		{
			_LogDay 	   = "";			
			_LogDayEnd 	   = "";			
			_AdType 	   = "";			
			_DataSet  = new DataSet();
			_DataSet2 = new DataSet();			
		}

		#endregion

		#region  ������Ƽ 

		public DataSet ReportDataSet
		{
			get { return _DataSet;	}
			set { _DataSet = value;	}
		}

		public DataSet ItemDataSet
		{
			get { return _DataSet2;	}
			set { _DataSet2 = value;	}
		}

		public string LogDay 
		{
			get { return _LogDay;	}
			set { _LogDay = value;	}
		}

		public string LogDayEnd 
		{
			get { return _LogDayEnd;	}
			set { _LogDayEnd = value;	}
		}

		public string AdType 
		{
			get { return _AdType;	}
			set { _AdType = value;	}
		}
	
		#endregion

	}
}