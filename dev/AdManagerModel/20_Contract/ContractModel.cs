// ===============================================================================
// Contract Data Model for Charites Project
//
// ContractModel.cs
//
// 컨텐츠정보 클래스를 정의합니다. 
//
// ===============================================================================
// Release history
// 2007.06.06 송명환 v1.0
// 2007.10.03 RH.Jung 계약상태별 체크 조회조건 추가
// ===============================================================================
// Copyright (C) 2007 G-Inno Systems Inc.
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
    public class ContractModel : BaseModel
    {

        // 조회용
        private string _SearchKey       = null;		// 검색어
		private string _SearchState_10  = null;     // 계약상태별 체크 조회조건 10:운영중 2007.10.03 RH.Jung
		private string _SearchState_20  = null;     // 계약상태별 체크 조회조건 20:종료
		private string _SearchRap       = null;		// 검색 미디어렙
		private string _SearchUse       = null;		// 검색 사용여부


        // 상세정보용
        private string _MediaCode	    = null;		//매체ID
        private string _RapCode	        = null;		//미디어랩ID
        private string _AgencyCode		= null;		//대행사
        private string _AdvertiserCode	= null;		//광고주 
        private string _MediaName	= null;		//매체ID명
        private string _RapName	    = null;		//미디어랩ID명
        private string _AgencyName	= null;		//대행사명
        private string _AdvertiserName	= null;		//광고주명 
        private string _ContractSeq		= null;		//계약순번
        private string _ContractName	= null;		//계약명
        private string _State	        = null;		//계약상태
        private string _ContStartDay	= null;		//계약시작일
        private string _ContEndDay      = null;     //계약종료일
		private string _PackageNo	    = null;		// 팩키지번호
		private string _PackageName     = null;		// 팩키지명
		private string _ContractAmt      = null;     //계약물량
		private string _BonusRate       = null;		// 보너스율
		private string _LongBonus       = null;		// 장기보너스율
		private string _SpecialBonus       = null;		// 특별보너스율
		private string _TotalBonus       = null;		// 총보너스율
		private string _SecurityTgt       = null;		// 보장노출
		private string _packageName       = null;		// 상품명		
		private string _Price           = null;		// 단가
		private string _AdTime          = null;		// 운행초수
        private string _Comment         = null;     //메모
		private string _JobCode         = null;     //업종코드
		private string _JobName         = null;     //업종명
		private string _JobName2         = null;     //업종명
		private string _JobName3         = null;     //업종명
		private string _Level1Code      = null;     //대분류코드
		private string _Level2Code      = null;     //중분류코드
		private string _Level	        = null;     //레벨
		private string _JobClass        = null;     //레벨
		private string _UseYn           = null;		// 사용여부

        // 목록조회용
        private DataSet  _ContractDataSet;

        public ContractModel() : base () 
        {
            Init();
        }

        #region Public 메소드
        public void Init()
        {
            _SearchKey		= "";
			_SearchState_10 = "";
			_SearchState_20 = "";
			_SearchRap      = "";
			_SearchUse      = "";

            _MediaCode	    = "";		//매체ID
            _RapCode	    = "";		//미디어랩ID
            _AgencyCode		= "";		//대행사
            _AdvertiserCode	= "";		//광고주
            _MediaName	= "";       //매체ID명
            _RapName	= "";       //미디어랩ID명
            _AgencyName	= "";       //대행사명
            _AdvertiserName = "";   //광고주명
 
            _ContractSeq	= "";		//계약순번
            _ContractName	= "";		//계약명
            _State	        = "";		//계약상태
            _ContStartDay	= "";		//계약시작일
            _ContEndDay     = "";       //계약종료일
			_PackageNo      = "";
			_PackageName    = "";
			_ContractAmt     = "";       //계약물량
			_BonusRate      = "";
			_LongBonus      = "";
			_SpecialBonus      = "";
			_TotalBonus      = "";
			_SecurityTgt      = "";
			_packageName      = "";
			_Price          = "";
			_AdTime         = "";
            _Comment        = "";       //메모
			_JobCode        = "";       //업종코드
			_JobName        = "";       //업종명
			_JobName2        = "";       //업종명
			_JobName3        = "";       //업종명
			_Level1Code     = "";       //대분류코드
			_Level2Code     = "";       //중분류코드
			_Level	        = "";       //레벨
			_JobClass	        = "";       //레벨
			_UseYn          = "";

	

            _ContractDataSet = new DataSet();
        }

        #endregion

        #region  프로퍼티 

        public DataSet ContractDataSet
        {
            get { return _ContractDataSet;	}
            set { _ContractDataSet = value;	}
        }

        public string SearchKey 
        {
            get { return _SearchKey;	}
            set { _SearchKey = value;	}
        }

		// 2007.10.03
		//
		public string SearchState_10 
		{
			get { return _SearchState_10;	}
			set { _SearchState_10 = value;	}
		}
	
		public string SearchState_20 
		{
			get { return _SearchState_20;	}
			set { _SearchState_20 = value;	}
		}

		public string SearchRap 
		{
			get { return _SearchRap;	}
			set { _SearchRap = value;	}
		}

		public string SearchUse  
		{
			get { return _SearchUse;	}
			set { _SearchUse = value;	}
		}
		//
		// 2007.10.03 
	
        public string MediaCode
        {
            get { return _MediaCode; }
            set { _MediaCode = value;}
        }
        public string RapCode
        {
            get { return _RapCode; }
            set { _RapCode = value;}
        }

        public string AgencyCode 
        {
            get { return _AgencyCode; }
            set { _AgencyCode = value;}
        }

        public string AdvertiserCode
        {
            get { return _AdvertiserCode;}
            set { _AdvertiserCode= value;}
        }
        public string MediaName
        {
            get { return _MediaName; }
            set { _MediaName = value;}
        }
        public string RapName
        {
            get { return _RapName; }
            set { _RapName = value;}
        }

        public string AgencyName 
        {
            get { return _AgencyName; }
            set { _AgencyName = value;}
        }

        public string AdvertiserName
        {
            get { return _AdvertiserName;}
            set { _AdvertiserName= value;}
        }

        public string ContractSeq 
        {
            get { return _ContractSeq; }
            set { _ContractSeq = value;}
        }
        public string ContractName 
        {
            get { return _ContractName; }
            set { _ContractName = value;}
        }
        public string State 
        {
            get { return _State;	}
            set { _State = value;}
        }
        public string ContStartDay 
        {
            get { return _ContStartDay; }
            set { _ContStartDay = value;}
        }
        public string ContEndDay     
        {
            get { return _ContEndDay;  }
            set { _ContEndDay = value;}
        }
		public string PackageNo 
		{
			get { return _PackageNo;	}
			set { _PackageNo = value;	}
		}

		public string PackageName 
		{
			get { return _PackageName;	}
			set { _PackageName = value;	}
		}

		public string ContractAmt     
		{
			get { return _ContractAmt;  }
			set { _ContractAmt = value;}
		}
        public string Comment        
        {
            get { return _Comment;}
            set { _Comment = value;	}
        }

		public string JobCode        
		{
			get { return _JobCode;}
			set { _JobCode = value;	}
		}

		public string JobName        
		{
			get { return _JobName;}
			set { _JobName = value;	}
		}

		public string JobName2        
		{
			get { return _JobName2;}
			set { _JobName2 = value;	}
		}

		public string JobName3        
		{
			get { return _JobName3;}
			set { _JobName3 = value;	}
		}

		public string Level1Code        
		{
			get { return _Level1Code;}
			set { _Level1Code = value;	}
		}
		
		public string Level2Code        
		{
			get { return _Level2Code;}
			set { _Level2Code = value;	}
		}

		public string Level        
		{
			get { return _Level;}
			set { _Level = value;	}
		}

		public string JobClass        
		{
			get { return _JobClass;}
			set { _JobClass = value;	}
		}

		public string UseYn 
		{
			get { return _UseYn;	}
			set { _UseYn = value;	}
		}

		public string BonusRate 
		{
			get { return _BonusRate;	}
			set { _BonusRate = value;	}
		}

		public string LongBonus 
		{
			get { return _LongBonus;	}
			set { _LongBonus = value;	}
		}

		public string SpecialBonus 
		{
			get { return _SpecialBonus;	}
			set { _SpecialBonus = value;	}
		}

		public string TotalBonus 
		{
			get { return _TotalBonus;	}
			set { _TotalBonus = value;	}
		}

		public string SecurityTgt 
		{
			get { return _SecurityTgt;	}
			set { _SecurityTgt = value;	}
		}

		public string packageName 
		{
			get { return _packageName;	}
			set { _packageName = value;	}
		}

		public string Price 
		{
			get { return _Price;	}
			set { _Price = value;	}
		}

		public string AdTime 
		{
			get { return _AdTime;	}
			set { _AdTime = value;	}
		}
        
        #endregion

    }
}
