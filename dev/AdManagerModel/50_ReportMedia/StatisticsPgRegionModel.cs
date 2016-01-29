using System;
using System.Data;

using WinFramework.Base;

namespace AdManagerModel
{
	/// <summary>
	/// StatisticsPgRegionModel�� ���� ��� �����Դϴ�.
	/// </summary>
	public class StatisticsPgRegionModel : BaseModel
	{

		// ��ȸ��
		private string _SearchMediaCode	     = null;		// �˻� ��ü
		private string _SearchCategoryCode   = null;		// ��ȸ ī�װ��ڵ�
		private string _SearchGenreCode      = null;		// ��ȸ �帣�ڵ�
		private string _SearchKey            = null;		// ��ȸ ���α׷���
		private string _SearchType           = null;		// ��ȸ ���� B:���ñⰣ D:�ϰ� W:�ְ� M:����
		private string _SearchStartDay       = null;		// ��ȸ ������� ����
		private string _SearchEndDay         = null;		// ��ȸ ������� ����

		// �����ȸ��
		private DataSet  _DataSet;
		private DataSet  _DataSet2;
		private DataSet  _DataSet3;

		public StatisticsPgRegionModel() : base () 
		{
			Init();
		}

		#region Public �޼ҵ�
		public void Init()
		{
			_SearchMediaCode 	   = "";
			_SearchCategoryCode    = "";
			_SearchGenreCode       = "";
			_SearchKey             = "";
			_SearchType            = "";
			_SearchStartDay          = "";
			_SearchEndDay          = "";
			_DataSet = new DataSet();
			_DataSet2 = new DataSet();
			_DataSet3 = new DataSet();
		}

		#endregion

		#region  ������Ƽ 

		public DataSet ReportDataSet
		{
			get { return _DataSet;	}
			set { _DataSet = value;	}
		}

		public DataSet CategoryDataSet
		{
			get { return _DataSet2;	}
			set { _DataSet2 = value;	}
		}

		public DataSet GenreDataSet
		{
			get { return _DataSet3;	}
			set { _DataSet3 = value;	}
		}


		public string SearchMediaCode 
		{
			get { return _SearchMediaCode;	}
			set { _SearchMediaCode = value;	}
		}

		public string SearchCategoryCode
		{
			get { return _SearchCategoryCode;	}
			set { _SearchCategoryCode = value;	}
		}

		public string SearchGenreCode 
		{
			get { return _SearchGenreCode;	}
			set { _SearchGenreCode = value;	}
		}

		public string SearchKey 
		{
			get { return _SearchKey;	}
			set { _SearchKey = value;	}
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