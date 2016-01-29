using System;
using System.Collections;
using System.Data;

using WinFramework.Base;

namespace AdManagerModel
{
	/// <summary>
	/// DateSummaryModel�� ���� ��� �����Դϴ�.
	/// </summary>
	public class DateSummaryModel : BaseModel
	{	

		// ��ȸ��
		private string      _StartDay   = null;		// ��ȸ ��������
		private string      _EndDay     = null;		// ��ȸ �˻���

		private ArrayList   _AdList;
		private string[]    _AdListStr;
		private DataSet     _DataSet;
		private DataSet     _DataSet2;

		public DateSummaryModel() : base () 
		{
			Init();
		}

		#region Public �޼ҵ�
		public void Init()
		{
			_StartDay	= "";
			_EndDay     = "";
			_AdList     = new ArrayList();
			_DataSet    = new DataSet();
			_DataSet2   = new DataSet();
			_AdListStr  = new string[0];
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

		/// <summary>
		/// ���ñ��� ����Ʈ
		/// </summary>
		public ArrayList    AdList
		{
			get { return _AdList;	}
			set { _AdList = value;	}
		}
		
		public string[] AdListStr
		{
			get { return _AdListStr;	}
			set { _AdListStr = value;	}
		}

		#endregion

	}
}