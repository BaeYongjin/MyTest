using System;
using System.Data;

using WinFramework.Base;

namespace AdManagerModel
{
	/// <summary>
	/// AdFileWideModel�� ���� ��� �����Դϴ�.
	/// </summary>
	public class AdFileWideModel : BaseModel
	{

		// ��ȸ��
		private string _SearchKey              = null;		// �˻���
		private string _SearchMediaCode	       = null;		// �˻� ��ü
		private string _SearchchkAdState_10    = null;		// �˻� ������� : �غ�
		private string _SearchchkAdState_20    = null;		// �˻� ������� : ��
		private string _SearchchkAdState_30    = null;		// �˻� ������� : ����
		private string _SearchchkAdState_40    = null;		// �˻� ������� : ����
		private string _SearchFileState        = null;		// �˻� ���ϻ��� 

		// ��������
		private string _MediaCode           = null;		// ��ü�ڵ�
		private string _ItemNo              = null;		// ���� ��೻����ȣ
		private string _ItemName            = null;		// �����
		private string _FileName            = null;		// ���ϸ�
		private string _FileState           = null;		// ���ϻ���

		// CMS ��������
		private string _FilePath		= null;			// ����Path
		private	string _cid				= null;			// ����Key
		private string _cmd				= null;			// ��������(UPLOAD_CDN���� ����)
		private string _requestStatus	= null;			// ����ȣ�� �������� 1:����
		private string _procStatus		= null;			// ó������ ���� 1, �������� ����
		private int	   _syncServer		= 0;			// ������ ������
		private int	   _descServer		= 0;			// ������ü ������

		// �����ȸ��
		private DataSet  _FileDataSet;
		private DataSet  _CountDataSet;
		private DataSet  _ScheduleDataSet;
		private DataSet	 _CmsDataSet;

		// ���ϰǼ��˻��
		private int _FileListCount = 0;

		public AdFileWideModel() : base () 
		{
			Init();
		}

		#region Public �޼ҵ�
		public void Init()
		{
			_SearchKey              = "";
			_SearchMediaCode	    = "";
			_SearchchkAdState_10    = "";
			_SearchchkAdState_20    = "";
			_SearchchkAdState_30    = "";
			_SearchchkAdState_40    = "";
			_SearchFileState        = "";

			_MediaCode		= "";
			_ItemNo         = "";
			_ItemName		= "";
			_FileName       = "";
			_FileState      = "";

			_FilePath		= "";
			_cid			= "";
			_cmd			= "";
			_requestStatus	= "";
			_procStatus		= "";
			_syncServer		= 0;
			_descServer		= 0;
			            
			if(_FileDataSet != null) _FileDataSet.Dispose();
			_FileDataSet = new DataSet();

			if(_CountDataSet != null) _CountDataSet.Dispose();
			_CountDataSet = new DataSet();

			if(_ScheduleDataSet != null) _ScheduleDataSet.Dispose();
			_ScheduleDataSet = new DataSet();

			if( _CmsDataSet != null )	_CmsDataSet.Dispose();
			_CmsDataSet = new DataSet();

			_FileListCount = 0;
		}

		#endregion

		#region  ������Ƽ 

		/// <summary>
		/// �������� �н�
		/// </summary>
		public string FilePath
		{
			get { return _FilePath;		}
			set { _FilePath = value;	}
		}

		/// <summary>
		/// CMS���� Key
		/// </summary>
		public string CmsCid
		{
			get { return _cid;		}
			set { _cid = value;	}
		}

		/// <summary>
		/// CMS���� ��ɾ�
		/// </summary>
		public string CmsCmd
		{
			get { return _cmd;		}
			set { _cmd = value;	}
		}

		/// <summary>
		/// CMS���� ȣ�⼺������
		/// </summary>
		public string CmsRequestStatus
		{
			get { return _requestStatus;		}
			set { _requestStatus = value;	}
		}

		/// <summary>
		/// CMS���� ��� ó������
		/// </summary>
		public string CmsProcessStatus
		{
			get { return _procStatus;		}
			set { _procStatus = value;	}
		}

		/// <summary>
		/// CMS���� ����������
		/// </summary>
		public int CmsSyncCount
		{
			get { return _syncServer;		}
			set { _syncServer = value;	}
		}

		/// <summary>
		/// CMS���� ������ü������
		/// </summary>
		public int CmsDescCount
		{
			get { return _descServer;		}
			set { _descServer = value;	}
		}

		/// <summary>
		/// CMS���� Key
		/// </summary>
		public DataSet CmsDataSet
		{
			get { return _CmsDataSet;		}
			set { _CmsDataSet = value;	}
		}

		public int FileListCount
		{
			get { return _FileListCount;	}
			set { _FileListCount = value;	}
		}

		public DataSet FileDataSet
		{
			get { return _FileDataSet;	}
			set { _FileDataSet = value;	}
		}

		public DataSet CountDataSet
		{
			get { return _CountDataSet;	}
			set { _CountDataSet = value;	}
		}

		public DataSet ScheduleDataSet
		{
			get { return _ScheduleDataSet;	}
			set { _ScheduleDataSet = value;	}
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


		public string SearchFileState
		{
			get { return _SearchFileState;	}
			set { _SearchFileState = value;	}
		}

		public string MediaCode 
		{
			get { return _MediaCode;	}
			set { _MediaCode = value;	}
		}

		public string ItemNo
		{
			get { return _ItemNo;		}
			set { _ItemNo = value;		}
		}

		public string ItemName 
		{
			get { return _ItemName;		}
			set { _ItemName = value;		}
		}

		public string FileName
		{
			get { return _FileName;	}
			set { _FileName = value;	}
		}

		public string FileState
		{
			get { return _FileState;	}
			set { _FileState = value;	}
		}
		
		#endregion

	}
}