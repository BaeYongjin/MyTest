using System;
using System.Data;

using WinFramework.Base;

namespace AdManagerModel
{
	/// <summary>
	/// AdTypeMoniteringModel�� ���� ��� �����Դϴ�.
	/// </summary>
	public class AdExecuteRateModel : BaseModel
	{
		// ��ȸ��
		private string _StartDay	=   null;
		private string _EndDay	=   null;
		
		// �����ȸ��
		private DataSet  _DataSet;
		
		public AdExecuteRateModel() : base () 
		{
			Init();
		}

		#region Public �޼ҵ�
		public void Init()
		{
			_StartDay 	= "";
			_EndDay	= "";
			_DataSet	= new DataSet();
		}

		#endregion

		#region  ������Ƽ 

		public DataSet ReportDataSet
		{
			get { return _DataSet;	}
			set { _DataSet = value;	}
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

		#endregion

	}
}