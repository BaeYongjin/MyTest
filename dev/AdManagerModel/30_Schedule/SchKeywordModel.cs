using System;
using System.Data;

using WinFramework.Base;

namespace AdManagerModel
{
	/// <summary>
	/// SchKeywordModel�� ���� ��� �����Դϴ�.
	/// </summary>
	public class SchKeywordModel : BaseModel
	{

		// ��ȸ��
		private string _SearchKey            = null;		// �˻���
		private string _SearchMediaCode	     = null;		// �˻� ��ü
		private string _SearchRapCode			= null;		
		private string _SearchchkAdState_10		= null;		// �˻� ������� : �غ�
		private string _SearchchkAdState_20		= null;		// �˻� ������� : ��
		private string _SearchchkAdState_30		= null;		// �˻� ������� : ����
		private string _SearchchkAdState_40		= null;		// �˻� ������� : ����

		// ��������		
		private string _ItemNo				= null;		// ��೻��Key
		private string _ItemName			= null;		// �����
		private string _MediaCode           = null;		// ��ü�ڵ�
		private string _GenreCode           = null;		// �帣(�޴�)�ڵ�
		private string _GenreName			= null;		// �帣��
		private string _ChannelNo           = null;		// ä�ι�ȣ
		private string _Title   			= null;		// ����
		private string _AdType              = null;		// ��������

		// �����ȸ��
		private DataSet  _DataSet;
		private DataSet  _DataSet2;
		private DataSet  _DataSet3;	// ����˻���
		private DataSet  _DataSet4;

		public SchKeywordModel() : base () 
		{
			Init();
		}

		#region Public �޼ҵ�
		public void Init()
		{
			_SearchKey		       = "";
			_SearchMediaCode 	   = "";
			_SearchRapCode			= "";
			_SearchchkAdState_10	= "";
			_SearchchkAdState_20	= "";
			_SearchchkAdState_30	= "";
			_SearchchkAdState_40	= "";
			
			_ItemNo				   = "";
			_ItemName              = "";
			_MediaCode             = "";
			_GenreCode             = "";
			_GenreName             = "";
			_ChannelNo             = "";
			_Title                 = "";
			_AdType                = "";
			            
			_DataSet  = new DataSet();
			_DataSet2 = new DataSet();
			_DataSet3 = new DataSet();
			_DataSet4 = new DataSet();
		}

		#endregion

		#region  ������Ƽ 

		public DataSet ChannelDataSet
		{
			get { return _DataSet;	}
			set { _DataSet = value;	}
		}
		
		public DataSet ScheduleDataSet
		{
			get { return _DataSet2;	}
			set { _DataSet2 = value;	}
		}

		public DataSet ContractItemDataSet
		{
			get { return _DataSet3;	}
			set { _DataSet3 = value;	}
		}

		public DataSet ScheduleItemDataSet
		{
			get { return _DataSet4;	}
			set { _DataSet4 = value;	}
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

		public string SearchRapCode
		{
			get { return _SearchRapCode; }
			set { _SearchRapCode = value;}
		}

		public string SearchchkAdState_10 
		{
			get { return _SearchchkAdState_10;		}
			set { _SearchchkAdState_10 = value;		}
		}

		public string SearchchkAdState_20 
		{
			get { return _SearchchkAdState_20;		}
			set { _SearchchkAdState_20 = value;		}
		}

		public string SearchchkAdState_30 
		{
			get { return _SearchchkAdState_30;		}
			set { _SearchchkAdState_30 = value;		}
		}

		public string SearchchkAdState_40 
		{
			get { return _SearchchkAdState_40;		}
			set { _SearchchkAdState_40 = value;		}
		}

		public string ItemNo
		{
			get { return _ItemNo;	}
			set { _ItemNo = value;	}
		}

		public string ItemName
		{
			get { return _ItemName;	}
			set { _ItemName = value;	}
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

		public string ChannelNo
		{
			get { return _ChannelNo;	}
			set { _ChannelNo = value;	}
		}

		public string Title
		{
			get { return _Title;	}
			set { _Title = value;	}
		}

		public string AdType
		{
			get { return _AdType;	}
			set { _AdType = value;	}
		}

		#endregion

	}
}