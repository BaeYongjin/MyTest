using System;
using System.Data;

using WinFramework.Base;

namespace AdManagerModel
{
	/// <summary>
	/// SystemConfigModel�� ���� ��� �����Դϴ�.
	/// </summary>
	public class SystemConfigModel : BaseModel
	{
		private string _FtpUploadID       = null;		// ftp ID
		private string _FtpUploadPW       = null;		// ftp pwd
		private string _FtpUploadHost     = null;		// ftp ȣ��Ʈ
		private string _FtpUploadPort     = null;		// ftp ��Ʈ
		private string _FtpUploadPath     = null;		// ftp ��ġ

		private string _FtpCdnID		= null;			// CDN ID
		private string _FtpCdnPW		= null;			// CDN pwd
		private string _FtpCdnHost		= null;			// CDN ȣ��Ʈ
		private string _FtpCdnPort      = null;			// CDN ��Ʈ
		
		private string _FtpMovePath     = null;			// �����̵���û��ġ
		private string _FtpMoveUseYn    = null;			// �����̵���û ��뿩��

		private string _FileQueueUseYn  = null;			// ���Ϲ���ť ��뿩��
		private string _FileQueueInterval = null;		// ���Ϲ���ť �ֱ�(��)
		private string _FileQueueCnt      = null;		// ���Ϲ���ť ��ȸ �߰��Ǵ� ���� ����

		private string _URLGetAdPopList   = null;		// �˾��������� �˾���������Ʈ��ȸ
		private string _URLSetAdPop       = null;		// �˾��������� �˾���������

		private string _CmsMasUrl		= null;			// CMS���� URL
		private string _CmsMasQuery		= null;			// CMS���� Query�� Fix�׸�
		

		// �����ȸ��
		private DataSet  _SystemConfigDataSet;
	
		public SystemConfigModel() : base()
		{
			Init();
		}

		#region Public �޼ҵ�
		public void Init()
		{
			_FtpUploadID	= "";
			_FtpUploadPW	= "";
			_FtpUploadHost	= "";
			_FtpUploadPort	= "";
			_FtpUploadPath	= "";

			_FtpCdnID		= "";
			_FtpCdnPW		= "";
			_FtpCdnHost		= "";
			_FtpCdnPort		= "";

			_FtpMovePath    = "";
			_FtpMoveUseYn   = "";

			_FileQueueUseYn = "";
			_FileQueueInterval  = "";
			_FileQueueCnt   = "";

			_URLGetAdPopList= "";
			_URLSetAdPop    = "";

			_CmsMasUrl		= "";
			_CmsMasQuery	= "";

			_SystemConfigDataSet = new DataSet();
		}

		#endregion

		#region  ������Ƽ 	

		public string FtpUploadID 
		{
			get { return _FtpUploadID;  }
			set { _FtpUploadID = value; }
		}

		public string FtpUploadPW 
		{
			get { return _FtpUploadPW;  }
			set { _FtpUploadPW = value; }
		}

		public string FtpUploadHost 
		{
			get { return _FtpUploadHost;  }
			set { _FtpUploadHost = value; }
		}

		public string FtpUploadPort 
		{
			get { return _FtpUploadPort;  }
			set { _FtpUploadPort = value; }
		}

		public string FtpUploadPath 
		{
			get { return _FtpUploadPath;  }
			set { _FtpUploadPath = value; }
		}

		public string FtpCdnID 
		{
			get { return _FtpCdnID;  }
			set { _FtpCdnID = value; }
		}

		public string FtpCdnPW 
		{
			get { return _FtpCdnPW;  }
			set { _FtpCdnPW = value; }
		}

		public string FtpCdnHost 
		{
			get { return _FtpCdnHost;  }
			set { _FtpCdnHost = value; }
		}

		public string FtpCdnPort 
		{
			get { return _FtpCdnPort;  }
			set { _FtpCdnPort = value; }
		}


		public string FtpMovePath 
		{
			get { return _FtpMovePath;  }
			set { _FtpMovePath = value; }
		}

		public string FtpMoveUseYn 
		{
			get { return _FtpMoveUseYn;  }
			set { _FtpMoveUseYn = value; }
		}

		public string FileQueueUseYn 
		{
			get { return _FileQueueUseYn;  }
			set { _FileQueueUseYn = value; }
		}

		public string FileQueueInterval
		{
			get { return _FileQueueInterval;  }
			set { _FileQueueInterval = value; }
		}

		public string FileQueueCnt
		{
			get { return _FileQueueCnt;  }
			set { _FileQueueCnt = value; }
		}

		public string URLGetAdPopList
		{
			get { return _URLGetAdPopList;  }
			set { _URLGetAdPopList = value; }
		}

		public string URLSetAdPop
		{
			get { return _URLSetAdPop;  }
			set { _URLSetAdPop = value; }
		}

		/// <summary>
		/// CMS���� Rul�� �������ų� �����մϴ�
		/// </summary>
		public string CmsMasUrl
		{
			get
			{
				return _CmsMasUrl;
			}
			set
			{
				_CmsMasUrl = value;
			}

		}

		/// <summary>
		/// CMS���� Query���� �������ų� �����մϴ�
		/// </summary>
		public string CmsMasQuery
		{
			get
			{
				return _CmsMasQuery;
			}
			set
			{
				_CmsMasQuery = value;
			}

		}

		public DataSet SystemConfigDataSet
		{
			get { return _SystemConfigDataSet;	}
			set { _SystemConfigDataSet = value;	}
		}
      

		#endregion

	}
}