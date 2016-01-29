using System;
using System.Data;

using WinFramework.Base;

namespace AdManagerModel
{
	/// <summary>
	/// AdFileWideModel에 대한 요약 설명입니다.
	/// </summary>
	public class AdFileWideModel : BaseModel
	{

		// 조회용
		private string _SearchKey              = null;		// 검색어
		private string _SearchMediaCode	       = null;		// 검색 매체
		private string _SearchchkAdState_10    = null;		// 검색 광고상태 : 준비
		private string _SearchchkAdState_20    = null;		// 검색 광고상태 : 편성
		private string _SearchchkAdState_30    = null;		// 검색 광고상태 : 중지
		private string _SearchchkAdState_40    = null;		// 검색 광고상태 : 종료
		private string _SearchFileState        = null;		// 검색 파일상태 

		// 상세정보용
		private string _MediaCode           = null;		// 매체코드
		private string _ItemNo              = null;		// 광고 계약내역번호
		private string _ItemName            = null;		// 광고명
		private string _FileName            = null;		// 파일명
		private string _FileState           = null;		// 파일상태

		// CMS 연동정보
		private string _FilePath		= null;			// 파일Path
		private	string _cid				= null;			// 연동Key
		private string _cmd				= null;			// 연동종류(UPLOAD_CDN으로 고정)
		private string _requestStatus	= null;			// 연동호출 성공여부 1:성공
		private string _procStatus		= null;			// 처리상태 성공 1, 나머지는 오류
		private int	   _syncServer		= 0;			// 배포된 서버수
		private int	   _descServer		= 0;			// 배포전체 서버수

		// 목록조회용
		private DataSet  _FileDataSet;
		private DataSet  _CountDataSet;
		private DataSet  _ScheduleDataSet;
		private DataSet	 _CmsDataSet;

		// 파일건수검사용
		private int _FileListCount = 0;

		public AdFileWideModel() : base () 
		{
			Init();
		}

		#region Public 메소드
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

		#region  프로퍼티 

		/// <summary>
		/// 광고파일 패스
		/// </summary>
		public string FilePath
		{
			get { return _FilePath;		}
			set { _FilePath = value;	}
		}

		/// <summary>
		/// CMS연동 Key
		/// </summary>
		public string CmsCid
		{
			get { return _cid;		}
			set { _cid = value;	}
		}

		/// <summary>
		/// CMS연동 명령어
		/// </summary>
		public string CmsCmd
		{
			get { return _cmd;		}
			set { _cmd = value;	}
		}

		/// <summary>
		/// CMS연동 호출성공여부
		/// </summary>
		public string CmsRequestStatus
		{
			get { return _requestStatus;		}
			set { _requestStatus = value;	}
		}

		/// <summary>
		/// CMS연동 결과 처리상태
		/// </summary>
		public string CmsProcessStatus
		{
			get { return _procStatus;		}
			set { _procStatus = value;	}
		}

		/// <summary>
		/// CMS연동 배포서버수
		/// </summary>
		public int CmsSyncCount
		{
			get { return _syncServer;		}
			set { _syncServer = value;	}
		}

		/// <summary>
		/// CMS연동 배포전체서버수
		/// </summary>
		public int CmsDescCount
		{
			get { return _descServer;		}
			set { _descServer = value;	}
		}

		/// <summary>
		/// CMS연동 Key
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