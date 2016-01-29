using System;
using System.Data;

using WinFramework.Base;

namespace AdManagerModel 
{
	/// <summary>
	/// RptSummaryAdDailyModel�� ���� ��� �����Դϴ�.
	/// </summary>
	public class RptSummaryAdDailyModel : BaseModel
	{
		// ��ȸ��
		private string _SearchDay            = null;		// ��ȸ ��������(����)

		// �����ȸ��
		private DataSet  _DataSet;

		public RptSummaryAdDailyModel() : base()
		{
			Init();
		}

		#region Public �޼ҵ�
		public void Init()
		{
			_SearchDay				= "";
			
			_DataSet = new DataSet();
		}
		#endregion

		#region  ������Ƽ 
		public DataSet RptDailyDataSet
		{
			get { return _DataSet;	}
			set { _DataSet = value;	}
		}

		public string SearchDay 
		{
			get { return _SearchDay;	}
			set { _SearchDay = value;	}
		}
		#endregion

	}
}
