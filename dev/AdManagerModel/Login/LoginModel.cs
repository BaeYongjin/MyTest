// ===============================================================================
//
// LoginModel.cs
//
// �α����� ���� ������ ��
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
	/// LoginModel�� ���� ��� �����Դϴ�.
	/// </summary>
	public class LoginModel : BaseModel
	{
		// ��û��
		private string _UserID = null;				// ����� ID
		private string _UserPassword = null;		// ����� ��й�ȣ
		private string _SystemVersion = null;		// �ý��۹���

		// �����
		private string _UserLevel = null;			// ����� ����
		private string _LevelName = null;			// ����� ����
		private string _UserClass = null;			// ����� ����
		private string _ClassName = null;			// ����� ����
		private string _UserName  = null;			// ����� ��
		private string _MediaCode  = null;			// ��ü
		private string _MediaName  = null;			// ��ü
		private string _RapCode  = null;			// ��
		private string _RapName  = null;			// ��
		private string _AgencyCode  = null;			// �����
		private string _AgencyName  = null;			// �����
		private string _LoginTime = null;			// �α��� �ð�
		private string _LastLogin = null;			// ������ �α��� �ð�
	
		public LoginModel() : base()
		{
			Init();
		}

		#region Public �޼ҵ�
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

		#region  ������Ƽ 	

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
