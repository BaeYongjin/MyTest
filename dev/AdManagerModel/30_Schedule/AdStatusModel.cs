using System;
using System.Data;
using System.Collections;

using WinFramework.Base;

namespace AdManagerModel
{
	/// <summary>
	/// AdStatusModel에 대한 요약 설명입니다.
	/// </summary>
	public class AdStatusModel : BaseModel
	{

		// 조회용
		private string _SearchKey              = null;		// 검색어
		private string _SearchMediaCode	       = null;		// 검색 매체		
		private string _SearchchkAdState_10    = null;		// 검색 광고상태 : 준비
		private string _SearchchkAdState_20    = null;		// 검색 광고상태 : 편성
		private string _SearchchkAdState_30    = null;		// 검색 광고상태 : 중지
		private string _SearchchkAdState_40    = null;		// 검색 광고상태 : 종료
		private string _SearchFileState_20     = null;		// 검색 파일상태 : FTP등록
		private string _SearchFileState_30     = null;		// 검색 파일상태 : CDN배포
		private string _SearchFileState_90     = null;		// 검색 파일상태 : 셋탑삭제

		// 상세정보용
		private string _ItemNo              = null;		// 광고 계약내역번호
		private string _ItemName            = null;		// 광고명
		private string _RealEndDay          = null;		// 실제종료일
		private string _FileName            = null;		// 파일명
		private string _FileType            = null;		// 파일타입 : 코드구분 24 : 10:동영상 20:이미지 30:플레쉬
		private string _FileLength          = null;		// 파일길이
		private string _FilePath            = null;		// 저장위치
		private string _DownLevel           = null;		// 다운레벨

		// 목록조회용
		private DataSet  _DataSet;

		public AdStatusModel() : base () 
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

		#region  프로퍼티 


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