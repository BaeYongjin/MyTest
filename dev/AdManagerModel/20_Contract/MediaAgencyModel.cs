// ===============================================================================
// MediaAgency Data Model for Charites Project
//
// MediaAgencyModel.cs
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
	public class MediaAgencyModel : BaseModel
	{

		// 조회용
		private string _SearchKey       = null;		// 검색어
		private string _SearchMediaName = null;		// 검색레벨
		private string _SearchRapName = null;		// 검색레벨
		private string _SearchMediaAgency = null;		// 검색레벨
		private string _SearchchkAdState_10    = null;		// 검색 사용여부상태

		// 상세정보용
		private string _MediaCode          = null;		// 미디어코드
		private string _RapCode          = null;		// 랩 코드
		private string _AgencyCode        = null;		// 대행사코드		
		private string _ContStartDay        = null;		// 시작일		
		private string _ContEndDay        = null;		// 종료일
		private string _Charger        = null;		// 담당자
		private string _Tell        = null;		// 전화번호		
		private string _Email        = null;		// 이메일
		private string _UseYn        = null;		// 사용여부
		private string _RegDt			= null;		// 등록일자
		private string _ModDt			= null;		// 수정일자
		private string _RegID			= null;		// 등록자
		
		
		// 목록조회용
		private DataSet  _MediaAgencyDataSet;
	
		public MediaAgencyModel() : base () 
		{
			Init();
		}

		#region Public 메소드
		public void Init()
		{
			_SearchKey		= "";
			_SearchMediaName = "";
			_SearchRapName = "";
			_SearchMediaAgency = "";
			_SearchchkAdState_10 = "";

			_MediaCode			= "";
			_RapCode		= "";			
			_AgencyCode		= "";			
			_ContStartDay		= "";		
			_ContEndDay			= "";
			_Charger			= "";
			_Tell 	= "";
			_Email		= "";			
			_UseYn		= "";		
			_RegDt			= "";
			_ModDt 	= "";
			_RegID 	= "";


			
			_MediaAgencyDataSet = new DataSet();
		}

		#endregion

		#region  프로퍼티 

		public DataSet MediaAgencyDataSet
		{
			get { return _MediaAgencyDataSet;	}
			set { _MediaAgencyDataSet = value;	}
		}

		public string SearchKey 
		{
			get { return _SearchKey;	}
			set { _SearchKey = value;	}
		}

		public string SearchMediaName
		{
			get { return _SearchMediaName;	}
			set { _SearchMediaName = value;	}
		}

		public string SearchRapName
		{
			get { return _SearchRapName;	}
			set { _SearchRapName = value;	}
		}
	
		public string SearchMediaAgency
		{
			get { return _SearchMediaAgency;	}
			set { _SearchMediaAgency = value;	}
		}

		public string SearchchkAdState_10 
		{
			get { return _SearchchkAdState_10;	}
			set { _SearchchkAdState_10 = value;	}
		}

		public string MediaCode 
		{
			get { return _MediaCode;		}
			set { _MediaCode = value;		}
		}

		public string RapCode 
		{
			get { return _RapCode;		}
			set { _RapCode = value;	}
		}

		public string AgencyCode 
		{
			get { return _AgencyCode;		}
			set { _AgencyCode = value;	}
		}

		public string ContStartDay 
		{
			get { return _ContStartDay;		}
			set { _ContStartDay = value;	}
		}


		public string ContEndDay 
		{
			get { return _ContEndDay;		}
			set { _ContEndDay = value;		}
		}

		public string Charger 
		{
			get { return _Charger;		}
			set { _Charger = value;		}
		}

		public string Tell 
		{
			get { return _Tell;  }
			set { _Tell = value; }
		}

		public string Email 
		{
			get { return _Email;  }
			set { _Email = value; }
		}

		public string UseYn 
		{
			get { return _UseYn;  }
			set { _UseYn = value; }
		}

		public string RegDt 
		{
			get { return _RegDt;  }
			set { _RegDt = value; }
		}

		public string ModDt 
		{
			get { return _ModDt;  }
			set { _ModDt = value; }
		}

		public string RegID 
		{
			get { return _RegID;  }
			set { _RegID = value; }
		}
		
		#endregion

	}
}
