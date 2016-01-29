using System;
using System.Data;

using WinFramework.Base;

namespace AdManagerModel
{
	/// <summary>
	/// DateAdTypeSummaryRptModel�� ���� ��� �����Դϴ�.
	/// </summary>
	public class DateAdTypeSummaryRptModel : BaseModel
	{
		// ��ȸ��
		private string _LogDay1			     = null;		// �˻� ��ü
		private string _LogDay2			     = null;		// �˻� ��ü
				
		// �����ȸ��
		private DataSet  _DataSet;
		
		// ��������
		private DataSet _DataSet2;

		public DateAdTypeSummaryRptModel() : base () 
		{
			Init();
		}

		#region Public �޼ҵ�
		public void Init()
		{
			_LogDay1 	   = "";			
			_LogDay2 	   = "";									
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

		public string LogDay2 
		{
			get { return _LogDay2;	}
			set { _LogDay2 = value;	}
		}
			
		#endregion

	}
}