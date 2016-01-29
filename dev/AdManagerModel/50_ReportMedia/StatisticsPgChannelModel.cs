using System;
using System.Data;

using WinFramework.Base;

namespace AdManagerModel
{
	/// <summary>
	/// StatisticsPgChannelModel�� ���� ��� �����Դϴ�.
	/// </summary>
	public class StatisticsPgChannelModel : BaseModel
	{

		// ��ȸ��
		private string _SearchMediaCode	     = null;		// �˻� ��ü
		private string _SearchType           = null;		// ��ȸ ���� B:���ñⰣ D:�ϰ� W:�ְ� M:����
		private string _SearchStartDay       = null;		// ��ȸ ������� ����
		private string _SearchEndDay         = null;		// ��ȸ ������� ����

		// �����ȸ��
		private DataSet  _DataSet;

		public StatisticsPgChannelModel() : base () 
		{
			Init();
		}

		#region Public �޼ҵ�
		public void Init()
		{
			_SearchMediaCode 	   = "";
			_SearchType            = "";
			_SearchStartDay          = "";
			_SearchEndDay          = "";
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

		public string SearchType 
		{
			get { return _SearchType;	}
			set { _SearchType = value;	}
		}

		public string SearchStartDay 
		{
			get { return _SearchStartDay;	}
			set { _SearchStartDay = value;	}
		}

		public string SearchEndDay 
		{
			get { return _SearchEndDay;	}
			set { _SearchEndDay = value;	}
		}

		#endregion

	}
}