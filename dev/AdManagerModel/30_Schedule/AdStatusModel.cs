using System;
using System.Data;
using System.Collections;

using WinFramework.Base;

namespace AdManagerModel
{
	/// <summary>
	/// AdStatusModel�� ���� ��� �����Դϴ�.
	/// </summary>
	public class AdStatusModel : BaseModel
	{

		// ��ȸ��
		private string _SearchKey              = null;		// �˻���
		private string _SearchMediaCode	       = null;		// �˻� ��ü		
		private string _SearchchkAdState_10    = null;		// �˻� ������� : �غ�
		private string _SearchchkAdState_20    = null;		// �˻� ������� : ��
		private string _SearchchkAdState_30    = null;		// �˻� ������� : ����
		private string _SearchchkAdState_40    = null;		// �˻� ������� : ����
		private string _SearchFileState_20     = null;		// �˻� ���ϻ��� : FTP���
		private string _SearchFileState_30     = null;		// �˻� ���ϻ��� : CDN����
		private string _SearchFileState_90     = null;		// �˻� ���ϻ��� : ��ž����

		// ��������
		private string _ItemNo              = null;		// ���� ��೻����ȣ
		private string _ItemName            = null;		// �����
		private string _RealEndDay          = null;		// ����������
		private string _FileName            = null;		// ���ϸ�
		private string _FileType            = null;		// ����Ÿ�� : �ڵ屸�� 24 : 10:������ 20:�̹��� 30:�÷���
		private string _FileLength          = null;		// ���ϱ���
		private string _FilePath            = null;		// ������ġ
		private string _DownLevel           = null;		// �ٿ��

		// �����ȸ��
		private DataSet  _DataSet;

		public AdStatusModel() : base () 
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
			_SearchFileState_20     = "";
			_SearchFileState_30     = "";
			_SearchFileState_90     = "";

			_ItemNo              = "";
			_ItemName			 = "";
			_RealEndDay			 = "";
			_FileName            = "";
			_FileType            = "";
			_FileLength          = "";
			_FilePath            = "";
			_DownLevel           = "";
			            
			_DataSet = new DataSet();
		}

		#endregion

		#region  ������Ƽ 


		public DataSet AdStatusDataSet
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


		public string SearchFileState_20
		{
			get { return _SearchFileState_20;	}
			set { _SearchFileState_20 = value;	}
		}

		public string SearchFileState_30
		{
			get { return _SearchFileState_30;	}
			set { _SearchFileState_30 = value;	}
		}

		public string SearchFileState_90
		{
			get { return _SearchFileState_90;	}
			set { _SearchFileState_90 = value;	}
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

		public string RealEndDay 
		{
			get { return _RealEndDay;		}
			set { _RealEndDay = value;		}
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
		
		#endregion
	
	}

}