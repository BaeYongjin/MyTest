using System;
using System.Data;

using WinFramework.Base;

namespace AdManagerModel
{
	/// <summary>
	/// AdFileModel�� ���� ��� �����Դϴ�.
	/// </summary>
	public class AdFileModel : BaseModel
	{

		// ��ȸ��
		private string _SearchKey              = null;		// �˻���
		private string _SearchMediaCode	       = null;		// �˻� ��ü
		private string _SearchRapCode          = null;		// �˻� ��
		private string _SearchAgencyCode       = null;		// �˻� �����
		private string _SearchAdvertiserCode   = null;		// �˻� ������
		private string _SearchAdClass          = null;		// �˻� ����뵵
		private string _SearchFileType	       = null;		// �˻� ���ϱ���
		private string _SearchchkAdState_10    = null;		// �˻� ������� : �غ�
		private string _SearchchkAdState_20    = null;		// �˻� ������� : ��
		private string _SearchchkAdState_30    = null;		// �˻� ������� : ����
		private string _SearchchkAdState_40    = null;		// �˻� ������� : ����
		private string _SearchchkFileState_10  = null;		// �˻� ���ϻ��� : �̵��
		private string _SearchchkFileState_11  = null;		// �˻� ���ϻ��� : ���米ü���
		private string _SearchchkFileState_12  = null;		// �˻� ���ϻ��� : ������
		private string _SearchchkFileState_15  = null;		// �˻� ���ϻ��� : �������
		private string _SearchchkFileState_20  = null;		// �˻� ���ϻ��� : ��������(FTP���)
		private string _SearchchkFileState_30  = null;		// �˻� ���ϻ��� : CDN����
		private string _SearchchkFileState_40  = null;		// �˻� ���ϻ��� : ��ž����
		private string _SearchchkFileState_90  = null;		// �˻� ���ϻ��� : ����
		private string _SearchAdType          = null;		// �˻� ��������

		// ��������
		private string _ItemNo              = null;		// ���� ��೻����ȣ
		private string _newItemNo              = null;		// ���� ��೻����ȣ
		private string _ItemName            = null;		// �����
		private string _PreFileName   = null;		
		private string _FileName            = null;		// ���ϸ�
		private string _FileType            = null;		// ����Ÿ�� : �ڵ屸�� 24 : 10:������ 20:�̹��� 30:�÷���
		private string _FileLength          = null;		// ���ϱ���
		private string _FilePath            = null;		// ������ġ
		private string _DownLevel           = null;		// �ٿ��
		private string _FileState           = null;		// ���ϻ��� : �ڵ屸�� 31 : 10:�̵�� 12:������ 15:������� 20:�������� 30:CDN���� 90:��ž����


		// FTP���ε�����
		private string _FtpUploadID     = null;		// FTP���ε� ID
		private string _FtpUploadPW     = null;		// FTP���ε� PW
		private string _FtpUploadHost   = null;		// FTP���ε� Host
		private string _FtpUploadPort   = null;		// FTP���ε� Port
		private string _FtpUploadPath   = null;		// FTP���ε� Path

		private string _FtpMovePath     = null;		// FTP�̵���û Path
		private string _FtpMoveUseYn    = null;		// �̵���û��뿩��

		// CDN��������
		private string _FtpCdnID     = null;		// CDN ID
		private string _FtpCdnPW     = null;		// CDN PW
		private string _FtpCdnHost   = null;		// CDN Host
		private string _FtpCdnPort   = null;		// CDN Port		

		private string _CmsMasUrl	= null;
		private string _CmsMasQuery	= null;

		// �������������
		//private string _CopyItemNo   = null;		// ������ ���������� �����ȣ
		private string _Flag   = null;		// ������ ���������� �����ȣ

		// �����ȸ��
		private DataSet  _DataSet;
		private DataSet  _DataSet2;

		public AdFileModel() : base () 
		{
			Init();
		}

		#region Public �޼ҵ�
		public void Init()
		{
			_SearchKey              = "";
			_SearchMediaCode	    = "";
			_SearchRapCode          = "";
			_SearchAgencyCode       = "";
			_SearchAdvertiserCode   = "";
			_SearchAdClass          = "";
			_SearchFileType	        = "";
			_SearchchkAdState_10    = "";
			_SearchchkAdState_20    = "";
			_SearchchkAdState_30    = "";
			_SearchchkAdState_40    = "";
			_SearchchkFileState_10  = "";
			_SearchchkFileState_11  = "";
			_SearchchkFileState_12  = "";
			_SearchchkFileState_15  = "";
			_SearchchkFileState_20  = "";
			_SearchchkFileState_30  = "";
			_SearchchkFileState_40  = "";
			_SearchchkFileState_90  = "";
			_SearchAdType           = "";

			_ItemNo              = "";
			_newItemNo              = "";
			_ItemName			 = "";
			_PreFileName            = "";
			_FileName            = "";
			_FileType            = "";
			_FileLength          = "";
			_FilePath            = "";
			_DownLevel           = "";
			_FileState           = "";

			_FtpUploadID   = "";
			_FtpUploadPW   = "";
			_FtpUploadHost = "";
			_FtpUploadPort = "";
			_FtpUploadPath = "";
			            
			_FtpMovePath   = "";
			_FtpMoveUseYn  = "";

			_FtpCdnID   = "";
			_FtpCdnPW   = "";
			_FtpCdnHost = "";
			_FtpCdnPort = "";

			_CmsMasUrl	= "";
			_CmsMasQuery= "";

			//_CopyItemNo = "";
			_Flag = "";
			
			_DataSet = new DataSet();
			_DataSet2 = new DataSet();
		}

		#endregion

		#region  ������Ƽ 

		public DataSet AdFileDataSet
		{
			get { return _DataSet;	}
			set { _DataSet = value;	}
		}

		public DataSet HistoryDataSet
		{
			get { return _DataSet;	}
			set { _DataSet = value;	}
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
			get { return _SearchRapCode;		}
			set { _SearchRapCode = value;		}
		}

		public string SearchAgencyCode 
		{
			get { return _SearchAgencyCode;		}
			set { _SearchAgencyCode = value;	}
		}

		public string SearchAdvertiserCode 
		{
			get { return _SearchAdvertiserCode;		}
			set { _SearchAdvertiserCode = value;	}
		}

		public string SearchAdClass 
		{
			get { return _SearchAdClass;		}
			set { _SearchAdClass = value;		}
		}

		public string SearchAdType 
		{
			get { return _SearchAdType;		}
			set { _SearchAdType = value;		}
		}

		public string SearchFileType 
		{
			get { return _SearchFileType;	}
			set { _SearchFileType = value;	}
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

		public string SearchchkFileState_11 
		{
			get { return _SearchchkFileState_11;		}
			set { _SearchchkFileState_11 = value;		}
		}

		public string SearchchkFileState_10 
		{
			get { return _SearchchkFileState_10;		}
			set { _SearchchkFileState_10 = value;		}
		}

		public string SearchchkFileState_12 
		{
			get { return _SearchchkFileState_12;		}
			set { _SearchchkFileState_12 = value;		}
		}

		public string SearchchkFileState_15 
		{
			get { return _SearchchkFileState_15;		}
			set { _SearchchkFileState_15 = value;		}
		}

		public string SearchchkFileState_20 
		{
			get { return _SearchchkFileState_20;		}
			set { _SearchchkFileState_20 = value;		}
		}

		public string SearchchkFileState_30 
		{
			get { return _SearchchkFileState_30;		}
			set { _SearchchkFileState_30 = value;		}
		}

		public string SearchchkFileState_40 
		{
			get { return _SearchchkFileState_40;		}
			set { _SearchchkFileState_40 = value;		}
		}

		public string SearchchkFileState_90 
		{
			get { return _SearchchkFileState_90;		}
			set { _SearchchkFileState_90 = value;		}
		}

		public string ItemNo
		{
			get { return _ItemNo;		}
			set { _ItemNo = value;		}
		}

		public string newItemNo
		{
			get { return _newItemNo;		}
			set { _newItemNo = value;		}
		}

		public string ItemName 
		{
			get { return _ItemName;		}
			set { _ItemName = value;		}
		}

		public string PreFileName
		{
			get { return _PreFileName;	}
			set { _PreFileName = value;	}
		}

		public string FileName
		{
			get { return _FileName;	}
			set { _FileName = value;	}
		}

		public string FileType
		{
			get { return _FileType;	}
			set { _FileType = value;	}
		}

		public string FileLength
		{
			get { return _FileLength;	}
			set { _FileLength = value;	}
		}

		public string FilePath
		{
			get { return _FilePath;	}
			set { _FilePath = value;	}
		}

		public string DownLevel 
		{
			get { return _DownLevel;		}
			set { _DownLevel = value;	}
		}
		
		public string FileState
		{
			get { return _FileState;	}
			set { _FileState = value;	}
		}

		public string FtpUploadID 
		{
			get { return _FtpUploadID;		}
			set { _FtpUploadID = value;	}
		}
		
		public string FtpUploadPW 
		{
			get { return _FtpUploadPW;	}
			set { _FtpUploadPW = value;	}
		}
		
		public string FtpUploadHost 
		{
			get { return _FtpUploadHost;	}
			set { _FtpUploadHost = value;	}
		}
		
		public string FtpUploadPort
		{
			get { return _FtpUploadPort;	}
			set { _FtpUploadPort = value;	}
		}
		
		public string FtpUploadPath 
		{
			get { return _FtpUploadPath; }
			set { _FtpUploadPath = value;}
		}
		
		public string FtpMovePath 
		{
			get { return _FtpMovePath;	}
			set { _FtpMovePath = value;	}
		}
		
		public string FtpMoveUseYn 
		{
			get { return _FtpMoveUseYn;	}
			set { _FtpMoveUseYn = value;}
		}


		public string FtpCdnID 
		{
			get { return _FtpCdnID;		}
			set { _FtpCdnID = value;	}
		}
		
		public string FtpCdnPW 
		{
			get { return _FtpCdnPW;		}
			set { _FtpCdnPW = value;	}
		}
		
		public string FtpCdnHost 
		{
			get { return _FtpCdnHost;	}
			set { _FtpCdnHost = value;	}
		}
		
		public string FtpCdnPort
		{
			get { return _FtpCdnPort;	}
			set { _FtpCdnPort = value;	}
		}

		/// <summary>
		/// CMS���� Url
		/// </summary>
		public string CmsMasUrl
		{
			get { return _CmsMasUrl;	}
			set { _CmsMasUrl = value;	}
		}

		/// <summary>
		/// CMS���� Post Query
		/// </summary>
		public string CmsMasQuery
		{
			get { return _CmsMasQuery;	}
			set { _CmsMasQuery = value;	}
		}




		public string Flag
		{
			get { return _Flag;		}
			set { _Flag = value;	}
		}


		#endregion

	}
}