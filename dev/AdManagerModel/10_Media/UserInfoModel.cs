// ===============================================================================
// UserInfo Data Model for Charites Project
//
// UserInfoModel.cs
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
	public class UserInfoModel : BaseModel
	{

		// 조회용
		private string _SearchKey       = null;		// 검색어
		private string _SearchUserLevel = null;		// 검색레벨
		private string _SearchUserClass = null;		// 검색레벨
		private string _SearchchkAdState_10    = null;		// 검색 사용여부상태

		// 상세정보용
		private string _UserID          = null;		// 사용자 아이디
		private string _UserName        = null;		// 사용자 명
		private string _UserPassword    = null;		// 사용자 비밀번호
		private string _UserLevel       = null;		// 보안 레벨
		private string _UserClass       = null;		// 사용자구분
		private string _MediaCode       = null;		// 매체
		private string _RapCode       = null;		// 랩사
		private string _AgencyCode       = null;		// 대행사
		private string _UserLevelName   = null;		// 사용자 레벨명
		private string _UserDept        = null;		// 사용자 소속부서
		private string _UserTitle       = null;		// 사용자 직책직함
		private string _UserTell        = null;		// 사용자 전화번호
		private string _UserMobile		= null;		// 사용자 이동전화
		private string _UserEMail		= null;		// 사용자 이메일
		private string _LastLogin		= null;		// 최근접속
		private string _RegDt			= null;		// 등록일자
		private string _UserComment     = null;		// 사용자 비고
		private string _UseYn     = null;		// 사용여부

		// 목록조회용
		private DataSet  _UserDataSet;
	
		public UserInfoModel() : base () 
		{
			Init();
		}

		#region Public 메소드
		public void Init()
		{
			_SearchKey		= "";
			_SearchUserLevel = "";
			_SearchUserClass = "";
			_SearchchkAdState_10 = "";

			_UserID			= "";
			_UserName		= "";
			_UserPassword	= "";
			_UserLevel		= "";
			_UserClass		= "";
			_MediaCode		= "";
			_RapCode		= "";
			_AgencyCode		= "";
			_UserLevelName	= "";
			_UserDept   	= "";			
			_UserTitle		= "";
			_UserTell		= "";
			_UserMobile		= "";
			_UserEMail		= "";
			_LastLogin		= "";
			_RegDt			= "";
			_UserComment 	= "";
			_UseYn 	= "";

			_UserDataSet = new DataSet();
		}

		#endregion

		#region  프로퍼티 

		public DataSet UserDataSet
		{
			get { return _UserDataSet;	}
			set { _UserDataSet = value;	}
		}

		public string SearchKey 
		{
			get { return _SearchKey;	}
			set { _SearchKey = value;	}
		}
	
		public string SearchUserLevel 
		{
			get { return _SearchUserLevel;	}
			set { _SearchUserLevel = value;	}
		}

		public string SearchUserClass 
		{
			get { return _SearchUserClass;	}
			set { _SearchUserClass = value;	}
		}

		public string SearchchkAdState_10 
		{
			get { return _SearchchkAdState_10;	}
			set { _SearchchkAdState_10 = value;	}
		}

		public string UserID 
		{
			get { return _UserID;		}
			set { _UserID = value;		}
		}

		public string UserName 
		{
			get { return _UserName;		}
			set { _UserName = value;	}
		}

		public string UserPassword 
		{
			get { return _UserPassword; }
			set { _UserPassword = value;}
		}

		public string UserLevel 
		{
			get { return _UserLevel;	}
			set { _UserLevel = value;	}
		}

		public string UserClass 
		{
			get { return _UserClass;	}
			set { _UserClass = value;	}
		}

		public string MediaCode 
		{
			get { return _MediaCode;	}
			set { _MediaCode = value;	}
		}

		public string RapCode 
		{
			get { return _RapCode;	}
			set { _RapCode = value;	}
		}

		public string AgencyCode 
		{
			get { return _AgencyCode;	}
			set { _AgencyCode = value;	}
		}

		public string UserLevelName
		{
			get { return _UserLevelName;	}
			set { _UserLevelName = value;	}
		}

		public string UserDept
		{
			get { return _UserDept;		}
			set { _UserDept = value;	}
		}

		public string UserTitle 
		{
			get { return _UserTitle;	}
			set { _UserTitle = value;	}
		}

		public string UserTell 
		{
			get { return _UserTell;		}
			set { _UserTell = value;	}
		}

		public string UserMobile 
		{
			get { return _UserMobile;	}
			set { _UserMobile = value;	}
		}

		public string UserEMail 
		{
			get { return _UserEMail;	}
			set { _UserEMail = value;	}
		}

		public string LastLogin 
		{
			get { return _LastLogin;	}
			set { _LastLogin = value;	}
		}

		public string RegDt 
		{
			get { return _RegDt;		}
			set { _RegDt = value;		}
		}

		public string UserComment 
		{
			get { return _UserComment;  }
			set { _UserComment = value; }
		}

		public string UseYn
		{
			get { return _UseYn;	}
			set { _UseYn = value;	}
		}

		#endregion

	}
}
