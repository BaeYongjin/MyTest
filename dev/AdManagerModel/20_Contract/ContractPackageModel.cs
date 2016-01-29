// ===============================================================================
// ContractPackage Data Model for Charites Project
//
// ContractPackageModel.cs
//
// 광고상품 팩키지 클래스를 정의합니다. 
//
// ===============================================================================
// Release history
// 2007.10.22 정래혁 처음
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
    public class ContractPackageModel : BaseModel
    {

        // 조회용
        private string _SearchKey       = null;		// 검색어
		private string _SearchRap       = null;		// 검색 미디어렙
		private string _SearchUse       = null;		// 검색 사용여부
    
        // 상세정보용
        private string _PackageNo	    = null;		// 팩키지번호
		private string _PackageName     = null;		// 팩키지명
		private string _RapCode         = null;		// 미디어렙코드
		private string _AdTime          = null;		// 운행초수
		private string _ContractAmt     = null;     // 보장노출량
		private string _BonusRate       = null;		// 보너스율
		private string _Price           = null;		// 단가
		private string _Comment         = null;		// 비고
		private string _UseYn           = null;		// 사용여부


        // 목록조회용
        private DataSet  _DataSet;

        public ContractPackageModel() : base () 
        {
            Init();
        }

        #region Public 메소드
        public void Init()
        {
            _SearchKey		= "";
			_SearchRap      = "";
			_SearchUse      = "";

			_PackageNo      = "";
			_PackageName    = "";
			_AdTime         = "";
			_RapCode        = "";
			_ContractAmt    = "";
			_BonusRate      = "";
			_Price          = "";
			_Comment        = "";
			_UseYn          = "";

            _DataSet = new DataSet();
        }

        #endregion

        #region  프로퍼티 

        public DataSet PackageDataSet
        {
            get { return _DataSet;	}
            set { _DataSet = value;	}
        }

        public string SearchKey 
        {
            get { return _SearchKey;	}
            set { _SearchKey = value;	}
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

		public string RapCode 
		{
			get { return _RapCode;	}
			set { _RapCode = value;	}
		}

		public string AdTime 
		{
			get { return _AdTime;	}
			set { _AdTime = value;	}
		}

		public string ContractAmt 
		{
			get { return _ContractAmt;	}
			set { _ContractAmt = value;	}
		}

		public string BonusRate 
		{
			get { return _BonusRate;	}
			set { _BonusRate = value;	}
		}

		public string Price 
		{
			get { return _Price;	}
			set { _Price = value;	}
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

		#endregion

    }
}
