// ===============================================================================
// Client Data Model for Charites Project
//
// ClientModel.cs
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
	public class ClientModel : BaseModel
	{

		// 조회용
		private string _SearchKey       = null;		// 검색어
		private string _SearchMediaName = null;		// 검색레벨
		private string _SearchRapName = null;		// 검색레벨
		private string _SearchMediaAgency = null;		// 검색레벨
		private string _SearchAdvertiserName = null;		// 검색레벨
		private string _SearchchkAdState_10    = null;		// 검색 사용여부상태
		private string _SearchRap       = null;		// 검색 미디어렙

		// 상세정보용
		private string _MediaCode         = null;		// 미디어코드
		private string _MediaCode_C       = null;		// 미디어코드
		private string _RapCode           = null;		// 랩 코드
		private string _RapCode_C         = null;		// 랩 코드
		private string _AgencyCode        = null;		// 대행사코드		
		private string _AgencyCode_C      = null;		// 대행사코드		
		private string _AdvertiserCode    = null;		// 광고주코드				
		private string _AdvertiserCode_C  = null;		// 광고주코드				
		private string _Comment           = null;		// 이메일
		private string _UseYn             = null;		// 사용여부
		private string _RegDt			  = null;		// 등록일자
		private string _ModDt			  = null;		// 수정일자
		private string _RegID			  = null;		// 등록자
		private string _MediaName         = null;
		private string _RapName           = null;
		private string _AgencyName        = null;

		
		
		// 목록조회용
		private DataSet  _ClientDataSet;
	
		public ClientModel() : base () 
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
			_SearchAdvertiserName = "";
			_SearchchkAdState_10 = "";
			_SearchRap = "";

			_MediaCode			= "";			
			_MediaCode_C			= "";			
			_RapCode		= "";			
			_RapCode_C		= "";			
			_AgencyCode		= "";		
			_AgencyCode_C		= "";		
			_AdvertiserCode		= "";		
			_AdvertiserCode_C		= "";		
			_Comment		= "";			
			_UseYn		= "";		
			_RegDt			= "";
			_ModDt 	= "";
			_RegID 	= "";

			_MediaName  = "";
			_RapName    = "";
			_AgencyName = "";
			
			_ClientDataSet = new DataSet();
		}

		#endregion

		#region  프로퍼티 

		public DataSet ClientDataSet
		{
			get { return _ClientDataSet;	}
			set { _ClientDataSet = value;	}
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

		public string SearchAdvertiserName
		{
			get { return _SearchAdvertiserName;	}
			set { _SearchAdvertiserName = value;	}
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

		public string MediaCode 
		{
			get { return _MediaCode;		}
			set { _MediaCode = value;		}
		}

		public string MediaCode_C
		{
			get { return _MediaCode_C;		}
			set { _MediaCode_C = value;		}
		}
		
		public string RapCode 
		{
			get { return _RapCode;		}
			set { _RapCode = value;	}
		}

		public string RapCode_C 
		{
			get { return _RapCode_C;		}
			set { _RapCode_C = value;	}
		}

		public string AgencyCode 
		{
			get { return _AgencyCode;		}
			set { _AgencyCode = value;	}
		}

		public string AgencyCode_C 
		{
			get { return _AgencyCode_C;		}
			set { _AgencyCode_C = value;	}
		}
	
		public string AdvertiserCode 
		{
			get { return _AdvertiserCode;		}
			set { _AdvertiserCode = value;	}
		}

		public string AdvertiserCode_C 
		{
			get { return _AdvertiserCode_C;		}
			set { _AdvertiserCode_C = value;	}
		}
		
		public string Comment 
		{
			get { return _Comment;  }
			set { _Comment = value; }
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

		public string MediaName 
		{
			get { return _MediaName;  }
			set { _MediaName = value; }
		}
		
		public string RapName 
		{
			get { return _RapName;  }
			set { _RapName = value; }
		}

		public string AgencyName 
		{
			get { return _AgencyName;  }
			set { _AgencyName = value; }
		}

		#endregion

	}
}
