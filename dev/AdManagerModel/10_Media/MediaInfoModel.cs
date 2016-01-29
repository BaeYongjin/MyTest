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
	public class MediaInfoModel : BaseModel
	{

		// 조회용
		private string _SearchKey       = null;		// 검색어
		private string _SearchUserLevel = null;		// 검색레벨
		private string _SearchchkAdState_10    = null;		// 검색 사용여부상태

		// 상세정보용
		private string _MediaCode          = null;		// 사용자 아이디
		private string _MediaName        = null;		// 사용자 명
		private string _Charger    = null;		// 사용자 비밀번호
		private string _Tell       = null;		// 사용자 레벨
		private string _Email   = null;		// 사용자 레벨명
		private string _UseYn   = null;		// 사용여부
		private string _RegDt        = null;		// 사용자 소속부서
		private string _ModDt       = null;		// 사용자 직책직함
		

		// 목록조회용
		private DataSet  _UserDataSet;
	
		public MediaInfoModel() : base () 
		{
			Init();
		}

		#region Public 메소드
		public void Init()
		{
			_SearchKey		= "";
			_SearchUserLevel = "";
			_SearchchkAdState_10 = "";

			_MediaCode			= "";
			_MediaName		= "";
			_Charger	= "";
			_Tell		= "";
			_Email	= "";
			_UseYn	= "";
			_RegDt   	= "";
			_ModDt		= "";
			            
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

		public string SearchchkAdState_10 
		{
			get { return _SearchchkAdState_10;	}
			set { _SearchchkAdState_10 = value;	}
		}
	
		public string SearchUserLevel 
		{
			get { return _SearchUserLevel;	}
			set { _SearchUserLevel = value;	}
		}

		public string MediaCode 
		{
			get { return _MediaCode;		}
			set { _MediaCode = value;		}
		}

		public string MediaName 
		{
			get { return _MediaName;		}
			set { _MediaName = value;	}
		}

		public string Charger 
		{
			get { return _Charger; }
			set { _Charger = value;}
		}

		public string Tell 
		{
			get { return _Tell;	}
			set { _Tell = value;	}
		}

		public string Email
		{
			get { return _Email;	}
			set { _Email = value;	}
		}

		public string UseYn
		{
			get { return _UseYn;	}
			set { _UseYn = value;	}
		}

		public string RegDt
		{
			get { return _RegDt;		}
			set { _RegDt = value;	}
		}

		public string ModDt 
		{
			get { return _ModDt;	}
			set { _ModDt = value;	}
		}

		

		#endregion

	}
}
