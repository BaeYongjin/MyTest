using System;
using System.Data;

using WinFramework.Base;

namespace AdManagerModel
{
	/// <summary>
	/// SchAppendAdModel에 대한 요약 설명입니다.
	/// </summary>
	public class SchAppendAdModel : BaseModel
	{

		// 조회용
		private string _SearchKey            = null;		// 검색어
		private string _SearchMediaCode	     = null;		// 검색 매체
		private string _SearchRapCode        = null;		// 검색 랩
		private string _SearchAgencyCode     = null;		// 검색 대행사
		private string _SearchAdvertiserCode = null;		// 검색 광고주
		private string _SearchContractState  = null;		// 검색 계약상태
		private string _SearchAdClass        = null;		// 검색 광고용도
		private string _SearchchkAdState_10    = null;		// 검색 광고상태 : 준비
		private string _SearchchkAdState_20    = null;		// 검색 광고상태 : 편성
		private string _SearchchkAdState_30    = null;		// 검색 광고상태 : 중지
		private string _SearchchkAdState_40    = null;		// 검색 광고상태 : 종료


		// 상세정보용		
		private string _MediaCode           = null;		// 매체코드
		private string _ScheduleOrder		= null;		// 편성순번
		private string _ItemNo				= null;		// 계약내역Key
		private string _ItemName			= null;		// 광고명

		// 목록조회용
		private DataSet  _DataSet;

		// 마지막 순서
		private string _LastOrder     = null;

		// 파일건수검사용
		private int _FileListCount = 0;

		public SchAppendAdModel() : base () 
		{
			Init();
		}

		#region Public 메소드
		public void Init()
		{
			_SearchKey		       = "";
			_SearchMediaCode 	   = "";
			_SearchRapCode         = "";
			_SearchAgencyCode      = "";
			_SearchAdvertiserCode  = "";
			_SearchContractState   = "";
			_SearchAdClass         = "";
			_SearchchkAdState_20   = "";
			_SearchchkAdState_30   = "";
			_SearchchkAdState_40   = "";
			
			_MediaCode   		   = "";
			_ScheduleOrder		   = "";
			_ItemNo				   = "";
			_ItemName              = "";

			_LastOrder		   = "";

			_DataSet = new DataSet();

			_FileListCount = 0;
		}

		#endregion

		#region  프로퍼티 

		public int FileListCount
		{
			get { return _FileListCount;	}
			set { _FileListCount = value;	}
		}

		public DataSet ScheduleDataSet
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

		public string SearchContractState 
		{
			get { return _SearchContractState;		}
			set { _SearchContractState = value;		}
		}

		public string SearchAdClass 
		{
			get { return _SearchAdClass;		}
			set { _SearchAdClass = value;		}
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

		public string MediaCode
		{
			get { return _MediaCode;		}
			set { _MediaCode = value;		}
		}

		public string ScheduleOrder
		{
			get { return _ScheduleOrder;		}
			set { _ScheduleOrder = value;		}
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

		public string LastOrder
		{
			get { return _LastOrder;		}
			set { _LastOrder = value;		}
		}

		#endregion

	}
}