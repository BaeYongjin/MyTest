// ===============================================================================
// UserInfo Data Model for Charites Project
//
// UserInfoModel.cs
//
// ��������� Ŭ������ �����մϴ�. 
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
	/// ����������� Ŭ���� ��.
	/// </summary>
	public class UserInfoModel : BaseModel
	{

		// ��ȸ��
		private string _SearchKey       = null;		// �˻���
		private string _SearchUserLevel = null;		// �˻�����
		private string _SearchUserClass = null;		// �˻�����
		private string _SearchchkAdState_10    = null;		// �˻� ��뿩�λ���

		// ��������
		private string _UserID          = null;		// ����� ���̵�
		private string _UserName        = null;		// ����� ��
		private string _UserPassword    = null;		// ����� ��й�ȣ
		private string _UserLevel       = null;		// ���� ����
		private string _UserClass       = null;		// ����ڱ���
		private string _MediaCode       = null;		// ��ü
		private string _RapCode       = null;		// ����
		private string _AgencyCode       = null;		// �����
		private string _UserLevelName   = null;		// ����� ������
		private string _UserDept        = null;		// ����� �ҼӺμ�
		private string _UserTitle       = null;		// ����� ��å����
		private string _UserTell        = null;		// ����� ��ȭ��ȣ
		private string _UserMobile		= null;		// ����� �̵���ȭ
		private string _UserEMail		= null;		// ����� �̸���
		private string _LastLogin		= null;		// �ֱ�����
		private string _RegDt			= null;		// �������
		private string _UserComment     = null;		// ����� ���
		private string _UseYn     = null;		// ��뿩��

		// �����ȸ��
		private DataSet  _UserDataSet;
	
		public UserInfoModel() : base () 
		{
			Init();
		}

		#region Public �޼ҵ�
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

		#region  ������Ƽ 

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
