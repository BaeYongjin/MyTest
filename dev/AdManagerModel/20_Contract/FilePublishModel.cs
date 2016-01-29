using System;
using System.Data;

using WinFramework.Base;

namespace AdManagerModel
{
	/// <summary>
	/// FilePublishModel�� ���� ��� �����Դϴ�.
	/// </summary>
	public class FilePublishModel : BaseModel
	{

		// ��ȸ��
		private string _SearchKey       = null;	// �˻���
		private string _SearchMediaCode	= null;	// �˻� ��ü		
		private string _SearchReserveKey= null; // �����۾���ȣ

		// ��������
		private string _MediaCode       = null;	// ��ü�ڵ�
		private string _AckNo	        = null;	// ���Ϲ������ι�ȣ
		
		private string _ReserveDt		= null;	// �����Ͻ�
		private string _State	        = null;	// ���Ϲ������λ��� 10:���δ��  30:��������(����)
		private string _PublishDesc     = null;	

		private string _ReserveUserNm   = null;
		private string _ModUserNm		= null;
		private string _ModDt			= null;

		private int	   _ItemNo			= 0;	// �۾���� �����ȣ
		private string _ReserveJob		= null;	// �۾���� ���� ó������(+�߰�,-����)
		private	int    _TotalCount		= 0;	// ��ü   ���ϼ�
		private int	   _NotCount		= 0;	// �̼��� ���ϼ�
 
		// �����ȸ��
		private DataSet  _DataSet1;
		private DataSet  _DataSet2;
		private DataSet  _DataSet3;

		public FilePublishModel() : base () 
		{
			Init();
		}

		#region Public �޼ҵ�
		public void Init()
		{
			_SearchKey          = "";
			_SearchMediaCode	= "";		
			_SearchReserveKey	= "";

			_MediaCode          = "";
			_AckNo              = "";
			_ReserveDt			= "";
			_State              = "";
			_PublishDesc        = "";

			_ReserveUserNm		= "";
			_ModUserNm			= "";
			_ModDt				= "";
			_ItemNo				= 0;
			_ReserveJob			= "";
			_TotalCount			= 0;
			_NotCount			= 0;
			            
			_DataSet1  = new DataSet();
			_DataSet2  = new DataSet();
			_DataSet3  = new DataSet();
		}

		#endregion

		#region  ������Ƽ 
		/// <summary>
		/// �����۾��ڵ�
		/// </summary>
		public string SearchReserveKey
		{
			get { return _SearchReserveKey;	}
			set { _SearchReserveKey = value;	}
		}

		/// <summary>
		/// �̿���� ���ϼ�
		/// </summary>
		public int NotCount
		{
			get { return _NotCount;	}
			set { _NotCount = value;	}
		}

		/// <summary>
		/// �����Ͻ�
		/// </summary>
		public string ModDt
		{
			get { return _ModDt;	}
			set { _ModDt = value;	}
		}


		/// <summary>
		/// ��ü ��� ���ϼ�
		/// </summary>
		public int TotalCount
		{
			get { return _TotalCount;	}
			set { _TotalCount = value;	}
		}

		/// <summary>
		/// �����۾� ����(+,-)
		/// </summary>
		public string JobType
		{
			get { return _ReserveJob;	}
			set { _ReserveJob = value;	}
		}

		/// <summary>
		/// ���� �����ȣ
		/// </summary>
		public int ItemNo
		{
			get { return _ItemNo;	}
			set { _ItemNo = value;	}
		}

		/// <summary>
		/// �����۾��� �̸�
		/// </summary>
		public string ReserveUserName
		{
			get { return _ReserveUserNm;	}
			set { _ReserveUserNm = value;	}
		}


		/// <summary>
		/// �����۾� ������ �̸�
		/// </summary>
		public string ModifyUserName
		{
			get { return _ModUserNm;	}
			set { _ModUserNm = value;	}
		}


		public DataSet PublishDataSet
		{
			get { return _DataSet1;	}
			set { _DataSet1 = value;	}
		}

		public DataSet HistoryDataSet
		{
			get { return _DataSet2;	}
			set { _DataSet2 = value;	}
		}
		
		public DataSet FileListDataSet
		{
			get { return _DataSet3;	}
			set { _DataSet3 = value;	}
		}

		public string SearchKey 
		{
			get { return _SearchKey;	}
			set { _SearchKey = value;	}
		}
	
		public string SearchMediaCode 
		{
			get { return _SearchMediaCode;	}
			set { _SearchMediaCode = value;	}
		}
		
		public string MediaCode 
		{
			get { return _MediaCode;		}
			set { _MediaCode = value;		}
		}

		/// <summary>
		/// ���Ϲ������ι�ȣ
		/// </summary>
		public string AckNo 
		{
			get { return _AckNo;		}
			set { _AckNo = value;		}
		}

		/// <summary>
		/// �����۾� �Ͻø� �������ų� �����Ѵ�.
		/// </summary>
		public string ReserveDt
		{
			get { return _ReserveDt;		}
			set { _ReserveDt = value;		}
		}
		
		/// <summary>
		/// ���Ϲ����������
		/// </summary>
		public string State 
		{
			get { return _State;		}
			set { _State = value;		}
		}

		/// <summary>
		/// �����۾� �޼���
		/// </summary>
		public string PublishDesc 
		{
			get { return _PublishDesc;		}
			set { _PublishDesc = value;		}
		}


		#endregion

	}
}