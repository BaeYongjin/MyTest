using System;
using System.Data;

using WinFramework.Base;

namespace AdManagerModel
{
	/// <summary>
	/// RptSummaryAdWeeklyModel�� ���� ��� �����Դϴ�.
	/// </summary>
	public class RptSummaryAdWeeklyModel : BaseModel
	{
		//��ȸ��
		private string _SearchStartDay       = null;		// ��ȸ ��������(����)
		private string _SearchDay = null;				//��ȸ ��������

		//�����ȸ��
		private DataSet _DataSet;

		public RptSummaryAdWeeklyModel() : base()
		{
			Init();
		}

		#region public Init()
		public void Init()
		{
			_SearchStartDay	= "";
			_SearchDay = "";

			_DataSet = new DataSet();
		}
		#endregion

		#region ������Ƽ
		public DataSet RptWeeklyDataSet
		{
			get { return _DataSet;	}
			set { _DataSet = value;	}
		}

		public string SearchStartDay 
		{
			get { return _SearchStartDay;	}
			set { _SearchStartDay = value;	}
		}

		public string SearchDay 
		{
			get { return _SearchDay;	}
			set { _SearchDay = value;	}
		}
		#endregion
	}
}
