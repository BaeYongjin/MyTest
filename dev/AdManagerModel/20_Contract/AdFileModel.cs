using System;
using System.Data;

using WinFramework.Base;

namespace AdManagerModel
{
	/// <summary>
	/// AdFileModel에 대한 요약 설명입니다.
	/// </summary>
	public class AdFileModel : BaseModel
	{

		// 조회용
		private string _SearchKey              = null;		// 검색어
		private string _SearchMediaCode	       = null;		// 검색 매체
		private string _SearchRapCode          = null;		// 검색 랩
		private string _SearchAgencyCode       = null;		// 검색 대행사
		private string _SearchAdvertiserCode   = null;		// 검색 광고주
		private string _SearchAdClass          = null;		// 검색 광고용도
		private string _SearchFileType	       = null;		// 검색 파일구분
		private string _SearchchkAdState_10    = null;		// 검색 광고상태 : 준비
		private string _SearchchkAdState_20    = null;		// 검색 광고상태 : 편성
		private string _SearchchkAdState_30    = null;		// 검색 광고상태 : 중지
		private string _SearchchkAdState_40    = null;		// 검색 광고상태 : 종료
		private string _SearchchkFileState_10  = null;		// 검색 파일상태 : 미등록
		private string _SearchchkFileState_11  = null;		// 검색 파일상태 : 소재교체대기
		private string _SearchchkFileState_12  = null;		// 검색 파일상태 : 시험대기
		private string _SearchchkFileState_15  = null;		// 검색 파일상태 : 배포대기
		private string _SearchchkFileState_20  = null;		// 검색 파일상태 : 배포승인(FTP등록)
		private string _SearchchkFileState_30  = null;		// 검색 파일상태 : CDN배포
		private string _SearchchkFileState_40  = null;		// 검색 파일상태 : 셋탑배포
		private string _SearchchkFileState_90  = null;		// 검색 파일상태 : 삭제
		private string _SearchAdType          = null;		// 검색 광고종류

		// 상세정보용
		private string _ItemNo              = null;		// 광고 계약내역번호
		private string _newItemNo              = null;		// 광고 계약내역번호
		private string _ItemName            = null;		// 광고명
		private string _PreFileName   = null;		
		private string _FileName            = null;		// 파일명
		private string _FileType            = null;		// 파일타입 : 코드구분 24 : 10:동영상 20:이미지 30:플레쉬
		private string _FileLength          = null;		// 파일길이
		private string _FilePath            = null;		// 저장위치
		private string _DownLevel           = null;		// 다운레벨
		private string _FileState           = null;		// 파일상태 : 코드구분 31 : 10:미등록 12:시험대기 15:배포대기 20:배포승인 30:CDN배포 90:셋탑삭제


		// FTP업로드정보
		private string _FtpUploadID     = null;		// FTP업로드 ID
		private string _FtpUploadPW     = null;		// FTP업로드 PW
		private string _FtpUploadHost   = null;		// FTP업로드 Host
		private string _FtpUploadPort   = null;		// FTP업로드 Port
		private string _FtpUploadPath   = null;		// FTP업로드 Path

		private string _FtpMovePath     = null;		// FTP이동요청 Path
		private string _FtpMoveUseYn    = null;		// 이동요청사용여부

		// CDN서버정보
		private string _FtpCdnID     = null;		// CDN ID
		private string _FtpCdnPW     = null;		// CDN PW
		private string _FtpCdnHost   = null;		// CDN Host
		private string _FtpCdnPort   = null;		// CDN Port		

		private string _CmsMasUrl	= null;
		private string _CmsMasQuery	= null;

		// 파일정보복사용
		//private string _CopyItemNo   = null;		// 복사할 파일정보의 광고번호
		private string _Flag   = null;		// 복사할 파일정보의 광고번호

		// 목록조회용
		private DataSet  _DataSet;
		private DataSet  _DataSet2;

		public AdFileModel() : base () 
		{
			Init();
		}

		#region Public 메소드
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

		#region  프로퍼티 

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
		/// CMS연동 Url
		/// </summary>
		public string CmsMasUrl
		{
			get { return _CmsMasUrl;	}
			set { _CmsMasUrl = value;	}
		}

		/// <summary>
		/// CMS연동 Post Query
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