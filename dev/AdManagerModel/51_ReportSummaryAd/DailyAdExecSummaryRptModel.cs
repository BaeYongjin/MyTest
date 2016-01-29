using System;
using System.Data;

using WinFramework.Base;

namespace AdManagerModel
{
	/// <summary>
	/// DailyAdExecSummaryRptModel�� ���� ��� �����Դϴ�.
	/// </summary>
	public class DailyAdExecSummaryRptModel : BaseModel
	{
		// ��ȸ��
		private string _LogDay1			     = null;		// �˻� ��ü
		
		// �����ȸ��
		private DataSet  _DataSet;
		
		// ��������
		private DataSet _DataSet2;

		public DailyAdExecSummaryRptModel() : base () 
		{
			Init();
		}

		#region Public �޼ҵ�
		public void Init()
		{
			_LogDay1 	   = "";			
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

		public string LogDay1 
		{
			get { return _LogDay1;	}
			set { _LogDay1 = value;	}
		}
	
		#endregion

	}
}