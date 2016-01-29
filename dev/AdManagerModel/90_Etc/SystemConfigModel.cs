using System;
using System.Data;

using WinFramework.Base;

namespace AdManagerModel
{
	/// <summary>
	/// SystemConfigModel에 대한 요약 설명입니다.
	/// </summary>
	public class SystemConfigModel : BaseModel
	{
		private string _FtpUploadID       = null;		// ftp ID
		private string _FtpUploadPW       = null;		// ftp pwd
		private string _FtpUploadHost     = null;		// ftp 호스트
		private string _FtpUploadPort     = null;		// ftp 포트
		private string _FtpUploadPath     = null;		// ftp 위치

		private string _FtpCdnID		= null;			// CDN ID
		private string _FtpCdnPW		= null;			// CDN pwd
		private string _FtpCdnHost		= null;			// CDN 호스트
		private string _FtpCdnPort      = null;			// CDN 포트
		
		private string _FtpMovePath     = null;			// 파일이동요청위치
		private string _FtpMoveUseYn    = null;			// 파일이동요청 사용여부

		private string _FileQueueUseYn  = null;			// 파일배포큐 사용여부
		private string _FileQueueInterval = null;		// 파일배포큐 주기(분)
		private string _FileQueueCnt      = null;		// 파일배포큐 매회 추가되는 파일 갯수

		private string _URLGetAdPopList   = null;		// 팝업공지연계 팝업공지리스트조회
		private string _URLSetAdPop       = null;		// 팝업공지연계 팝업공지설정

		private string _CmsMasUrl		= null;			// CMS연동 URL
		private string _CmsMasQuery		= null;			// CMS연동 Query중 Fix항목
		

		// 목록조회용
		private DataSet  _SystemConfigDataSet;
	
		public SystemConfigModel() : base()
		{
			Init();
		}

		#region Public 메소드
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

		#region  프로퍼티 	

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
		/// CMS연동 Rul을 가져오거나 설정합니다
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
		/// CMS연동 Query문을 가져오거나 설정합니다
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