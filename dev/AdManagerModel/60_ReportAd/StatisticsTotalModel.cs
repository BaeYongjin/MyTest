using System;
using System.Data;

using WinFramework.Base;

namespace AdManagerModel
{
	/// <summary>
	/// StatisticsTotalModel�� ���� ��� �����Դϴ�.
	/// </summary>
	public class StatisticsTotalModel : BaseModel
	{

		// ��ȸ��
		private string _SearchMediaCode	     = null;		// �˻� ��ü
		private string _SearchRapCode	     = null;		// �˻� ��ü
		private string _SearchAgencyCode     = null;		// �˻� ��ü
		private string _SearchKey            = null;		// ��ȸ �˻���

		// �����ȸ��
		private DataSet  _DataSet;

		public StatisticsTotalModel() : base () 
		{
			Init();
		}

		#region Public �޼ҵ�
		public void Init()
		{
			_SearchMediaCode 	   = "";
			_SearchRapCode         = "";
			_SearchAgencyCode 	   = "";
			_SearchKey             = "";				
			_DataSet = new DataSet();
		}

		#endregion

		#region  ������Ƽ 

		public DataSet ReportDataSet
		{
			get { return _DataSet;	}
			set { _DataSet = value;	}
		}

		public string SearchMediaCode 
		{
			get { return _SearchMediaCode;	}
			set { _SearchMediaCode = value;	}
		}

		public string SearchRapCode 
		{
			get { return _SearchRapCode;	}
			set { _SearchRapCode = value;	}
		}

		public string SearchAgencyCode 
		{
			get { return _SearchAgencyCode;	}
			set { _SearchAgencyCode = value;	}
		}

		public string SearchKey
		{
			get { return _SearchKey;	}
			set { _SearchKey = value;	}
		}

		#endregion

	}
}