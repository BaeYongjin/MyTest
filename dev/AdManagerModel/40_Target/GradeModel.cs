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
	public class GradeModel : BaseModel
	{

		// 조회용
		private string _SearchKey       = null;		// 검색어
		private string _SearchUserLevel = null;		// 검색레벨		
		private string _MediaCode         = null;		// 사용자 아이디

		private string _SearchchkAdState_10    = null;		// 검색 광고상태 : 준비
		private string _SearchchkAdState_20    = null;		// 검색 광고상태 : 편성
		private string _SearchchkAdState_30    = null;		// 검색 광고상태 : 중지
		private string _SearchchkAdState_40    = null;		// 검색 광고상태 : 종료

		// 상세정보용
		private string _Code         = null;		// 사용자 아이디
		private string _CodeName     = null;		// 사용자 명
		private string _Grade		 = null;		// 사용자 비밀번호	
		private string _RegID        = null;		// 사용자 소속부서
		private string _RegDt        = null;		// 사용자 소속부서
		private string _ModDt        = null;		// 사용자 직책직함
		private string _Code_O         = null;		// 사용자 아이디

		// 목록조회용
		private DataSet  _GradeDataSet;
		private DataSet  _ContractItemDataSet;
	
		public GradeModel() : base () 
		{
			Init();
		}

		#region Public 메소드
		public void Init()
		{
			_SearchKey		= "";
			_SearchUserLevel = "";			
			_MediaCode = "";

			_SearchchkAdState_10 = "";
			_SearchchkAdState_20 = "";
			_SearchchkAdState_30 = "";
			_SearchchkAdState_40 = "";

			_Code			= "";
			_CodeName		= "";
			_Grade	= "";	
			_RegID   	= "";
			_RegDt   	= "";
			_ModDt		= "";
			_Code_O		= "";
			            
			_GradeDataSet = new DataSet();
			_ContractItemDataSet = new DataSet();
		}

		#endregion

		#region  프로퍼티 

		public DataSet GradeDataSet
		{
			get { return _GradeDataSet;	}
			set { _GradeDataSet = value;	}
		}

		public DataSet ContractItemDataSet
		{
			get { return _ContractItemDataSet;	}
			set { _ContractItemDataSet = value;	}
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

		public string MediaCode 
		{
			get { return _MediaCode;	}
			set { _MediaCode = value;	}
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

		public string Code 
		{
			get { return _Code;		}
			set { _Code = value;		}
		}

		public string CodeName 
		{
			get { return _CodeName;		}
			set { _CodeName = value;	}
		}

		public string Grade 
		{
			get { return _Grade; }
			set { _Grade = value;}
		}	

		public string RegID
		{
			get { return _RegID;	}
			set { _RegID = value;	}
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

		public string Code_O 
		{
			get { return _Code_O;	}
			set { _Code_O = value;	}
		}

		

		#endregion

	}
}
