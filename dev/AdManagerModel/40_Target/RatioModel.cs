using System;
using System.Data;

using WinFramework.Base;

namespace AdManagerModel
{
	/// <summary>
	/// RatioModel�� ���� ��� �����Դϴ�.
	/// </summary>
	public class RatioModel : BaseModel
	{
		// ��������		
		private string _ItemNo				= null;		// ��೻��Key		
		private string _MediaCode           = null;		// ��ü�ڵ�		
		private string _GenreCode           = null;		// �帣(�޴�)�ڵ�
		private string _GenreName			= null;		// �帣��
		private string _CategoryCode        = null;		// ī�װ�
		private string _EntrySeq			= null;		// ��������
		private string _EntryRate			= null;		// ����
		private string _EntryYN             = null;		// ��뿩��

		// �����ȸ��
		private DataSet  _MenuDataSet;
		private DataSet  _SchRateDataSet;
		private DataSet  _SchRateDetailDataSet;
		private DataSet  _Group1DataSet;
		private DataSet  _Group2DataSet;
		private DataSet  _Group3DataSet;
		private DataSet  _Group4DataSet;
		private DataSet  _Group5DataSet;
		private DataSet  _DataSet;

		public RatioModel() : base () 
		{
			Init();
		}

		#region Public �޼ҵ�
		public void Init()
		{			
			_ItemNo				   = "";			
			_MediaCode             = "";			
			_GenreCode             = "";
			_GenreName             = "";
			_CategoryCode             = "";
			_EntrySeq                 = "";
			_EntryRate                = "";
			_EntryYN                = "";
			            
			_MenuDataSet = new DataSet();
			_SchRateDataSet = new DataSet();
			_SchRateDetailDataSet = new DataSet();
			_Group1DataSet = new DataSet();
			_Group2DataSet = new DataSet();
			_Group3DataSet = new DataSet();
			_Group4DataSet = new DataSet();
			_Group5DataSet = new DataSet();
			_DataSet = new DataSet();
		}

		#endregion

		#region  ������Ƽ
 
		public DataSet MenuDataSet
		{
			get { return _MenuDataSet;	}
			set { _MenuDataSet = value;	}
		}

		public DataSet SchRateDataSet
		{
			get { return _SchRateDataSet;	}
			set { _SchRateDataSet = value;	}
		}

		public DataSet SchRateDetailDataSet
		{
			get { return _SchRateDetailDataSet;	}
			set { _SchRateDetailDataSet = value;	}
		}

		public DataSet Group1DataSet
		{
			get { return _Group1DataSet;	}
			set { _Group1DataSet = value;	}
		}

		public DataSet Group2DataSet
		{
			get { return _Group2DataSet;	}
			set { _Group2DataSet = value;	}
		}

		public DataSet Group3DataSet
		{
			get { return _Group3DataSet;	}
			set { _Group3DataSet = value;	}
		}

		public DataSet Group4DataSet
		{
			get { return _Group4DataSet;	}
			set { _Group4DataSet = value;	}
		}

		public DataSet Group5DataSet
		{
			get { return _Group5DataSet;	}
			set { _Group5DataSet = value;	}
		}

		public DataSet ScheduleDataSet
		{
			get { return _DataSet;	}
			set { _DataSet = value;	}
		}
		
		public string ItemNo
		{
			get { return _ItemNo;	}
			set { _ItemNo = value;	}
		}

		public string MediaCode
		{
			get { return _MediaCode;	}
			set { _MediaCode = value;	}
		}
		
		public string GenreCode
		{
			get { return _GenreCode;	}
			set { _GenreCode = value;	}
		}

		public string GenreName
		{
			get { return _GenreName;	}
			set { _GenreName = value;	}
		}

		public string CategoryCode
		{
			get { return _CategoryCode;	}
			set { _CategoryCode = value;	}
		}

		public string EntrySeq
		{
			get { return _EntrySeq;	}
			set { _EntrySeq = value;	}
		}

		public string EntryRate
		{
			get { return _EntryRate;	}
			set { _EntryRate = value;	}
		}

		public string EntryYN
		{
			get { return _EntryYN;	}
			set { _EntryYN = value;	}
		}


		#endregion

	}
}