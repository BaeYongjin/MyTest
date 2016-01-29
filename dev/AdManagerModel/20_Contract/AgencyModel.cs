// ===============================================================================
// Agency Data Model for Charites Project
//
// AgencyModel.cs
//
// 사용자정보 클래스를 정의합니다. 
//
// ===============================================================================
// Release history
//
// ===============================================================================
// Copyright (C) 2006 G-Inno Systems Inc.
// All rights reserved.
// ==============================================================================

using System;
using System.Data;

using WinFramework.Base;

namespace AdManagerModel
{
	/// <summary>
	/// 사용자정보의 클래스 모델.
	/// </summary>
	public class AgencyModel : BaseModel
	{

		// 조회용
		private string _SearchKey       = null;		// 검색어
		private string _SearchAgencyType = null;		// 검색구분
		private string _SearchchkAdState_10    = null;		// 검색 사용여부상태
		private string _SearchRap       = null;		// 검색 미디어렙

		// 상세정보용
		private string _AgencyCode		= null;		// 대행사코드
		private string _AgencyName       = null;	// 대행사명
		private string _RapCode	 		= null;		// 대행사구분
		private string _RapName			= null;		// 대행사구분
		private string _AgencyType		= null;		// 대행사구분
		private string _AgencyTypeName	= null;		// 대행사구분명
		private string _Address			= null;		// 주소
		private string _Tell			= null;		// 전화
		private string _Comment			= null;		// 비고
		private string _UseYn			= null;		// 사용여부
		private string _RegDt			= null;		// 등록일시
		private string _ModDt			= null;		// 수정일시
		private string _RegID			= null;		// 등록자(최종수정자)

		// 목록조회용
		private DataSet  _DataSet;
	
		public AgencyModel() : base () 
		{
			Init();
		}

		#region Public 메소드
		public void Init()
		{
			_SearchKey			= "";
			_SearchAgencyType	= "";
			_SearchchkAdState_10	= "";
			_SearchRap	= "";

			_AgencyCode			= "";
			_AgencyName			= "";
			_RapCode			= "";
			_RapName			= "";
			_AgencyType			= "";
			_AgencyTypeName		= "";
			_Address			= "";
			_Tell				= "";
			_Comment			= "";
			_UseYn				= "";
			_RegDt				= "";
			_ModDt				= "";
			_RegID				= "";
				
			_DataSet = new DataSet();
		}

		#endregion

		#region  프로퍼티 

		public DataSet AgencyDataSet
		{
			get { return _DataSet;	}
			set { _DataSet = value;	}
		}

		public string SearchKey 
		{
			get { return _SearchKey;	}
			set { _SearchKey = value;	}
		}
	
		public string SearchAgencyType
		{
			get { return _SearchAgencyType;	}
			set { _SearchAgencyType = value;	}
		}

		public string SearchchkAdState_10 
		{
			get { return _SearchchkAdState_10;	}
			set { _SearchchkAdState_10 = value;	}
		}

		public string SearchRap 
		{
			get { return _SearchRap;	}
			set { _SearchRap = value;	}
		}

		public string AgencyCode
		{
			get { return _AgencyCode;	}
			set { _AgencyCode = value;	}
		}

		public string AgencyName
		{
			get { return _AgencyName;	}
			set { _AgencyName = value;	}
		}

		public string RapCode
		{
			get { return _RapCode;	}
			set { _RapCode = value;	}
		}

		public string RapName
		{
			get { return _RapName;	}
			set { _RapName = value;	}
		}

		public string AgencyType
		{
			get { return _AgencyType;	}
			set { _AgencyType = value;	}
		}

		public string AgencyTypeName
		{
			get { return _AgencyTypeName;	}
			set { _AgencyTypeName = value;	}
		}

		public string Address
		{
			get { return _Address;	}
			set { _Address = value;	}
		}

		public string Tell
		{
			get { return _Tell;	}
			set { _Tell = value;	}
		}

		public string Comment
		{
			get { return _Comment;	}
			set { _Comment = value;	}
		}

		public string UseYn
		{
			get { return _UseYn;	}
			set { _UseYn = value;	}
		}

		public string RegDt
		{
			get { return _RegDt;	}
			set { _RegDt = value;	}
		}

		public string ModDt
		{
			get { return _ModDt;	}
			set { _ModDt = value;	}
		}

		public string RegID
		{
			get { return _RegID;	}
			set { _RegID = value;	}
		}

		#endregion

	}
}
