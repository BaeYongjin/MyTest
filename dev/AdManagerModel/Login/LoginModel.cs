// ===============================================================================
//
// LoginModel.cs
//
// 로그인을 위한 데이터 모델
//
// ===============================================================================
// Release history
// 2007.08.25 RH.Jung Initialize
// 
// ===============================================================================
// Copyright (C) 2007 G-Inno Systems Inc.
// All rights reserved.
// ==============================================================================


using System;

using WinFramework.Base;

namespace AdManagerModel
{
	/// <summary>
	/// LoginModel에 대한 요약 설명입니다.
	/// </summary>
	public class LoginModel : BaseModel
	{
		// 요청용
		private string _UserID = null;				// 사용자 ID
		private string _UserPassword = null;		// 사용자 비밀번호
		private string _SystemVersion = null;		// 시스템버전

		// 결과용
		private string _UserLevel = null;			// 사용자 레벨
		private string _LevelName = null;			// 사용자 레벨
		private string _UserClass = null;			// 사용자 구분
		private string _ClassName = null;			// 사용자 구분
		private string _UserName  = null;			// 사용자 명
		private string _MediaCode  = null;			// 매체
		private string _MediaName  = null;			// 매체
		private string _RapCode  = null;			// 랩
		private string _RapName  = null;			// 랩
		private string _AgencyCode  = null;			// 대행사
		private string _AgencyName  = null;			// 대행사
		private string _LoginTime = null;			// 로그인 시각
		private string _LastLogin = null;			// 마지막 로그인 시각
	
		public LoginModel() : base()
		{
			Init();
		}

		#region Public 메소드
		public void Init()
		{
			_UserID			= "";
			_UserName		= "";
			_MediaCode		= "";
			_MediaName		= "";
			_RapCode		= "";
			_RapName		= "";
			_AgencyCode	    = "";
			_AgencyName     = "";
			_UserPassword	= "";
			_UserLevel		= "";
			_LevelName		= "";
			_UserClass		= "";
			_ClassName		= "";
			_LoginTime      = "";
			_LastLogin      = "";

			_SystemVersion  = "";
		}

		#endregion

		#region  프로퍼티 	

		public string SystemVersion 
		{
			get { return _SystemVersion;  }
			set { _SystemVersion = value; }
		}

		public string UserID 
		{
			get { return _UserID;  }
			set { _UserID = value; }
		}

		public string UserName 
		{
			get { return _UserName;  }
			set { _UserName = value; }
		}

		public string MediaCode 
		{
			get { return _MediaCode;  }
			set { _MediaCode = value; }
		}

		public string MediaName 
		{
			get { return _MediaName;  }
			set { _MediaName = value; }
		}

		public string RapCode 
		{
			get { return _RapCode;  }
			set { _RapCode = value; }
		}

		public string RapName 
		{
			get { return _RapName;  }
			set { _RapName = value; }
		}

		public string AgencyCode 
		{
			get { return _AgencyCode;  }
			set { _AgencyCode = value; }
		}

		public string AgencyName 
		{
			get { return _AgencyName;  }
			set { _AgencyName = value; }
		}

		public string UserPassword 
		{
			get { return _UserPassword;  }
			set { _UserPassword = value; }
		}

		public string UserLevel 
		{
			get { return _UserLevel;  }
			set { _UserLevel = value; }
		}

		public string LevelName 
		{
			get { return _LevelName;  }
			set { _LevelName = value; }
		}

		public string UserClass
		{
			get { return _UserClass;  }
			set { _UserClass = value; }
		}

		public string ClassName
		{
			get { return _ClassName;  }
			set { _ClassName = value; }
		}

		public string LoginTime 
		{
			get { return _LoginTime;  }
			set { _LoginTime = value; }
		}

		public string LastLogin 
		{
			get { return _LastLogin;  }
			set { _LastLogin = value; }
		}

		#endregion

	}
}
