/*
 * -------------------------------------------------------
 * [수정사항]
 * -------------------------------------------------------
 * 수정코드  : [A_01]
 * 수정자    : JH.Kim
 * 수정일    : 2015.11.
 * 수정내용  : 광고업종 추가
 * --------------------------------------------------------
 */
using System;
using System.Data;

using WinFramework.Base;

namespace AdManagerModel
{
	/// <summary>
	/// AdvertiserModelModel에 대한 요약 설명입니다.
	/// </summary>
	public class AdvertiserModel : BaseModel
	{

		// 조회용
		private string _SearchKey               = null;		// 검색어
		private string _SearchAdvertiserLevel   = null;		// 검색레벨
		private string _SearchchkAdState_10     = null;		// 검색 사용여부상태
		private string _SearchRap               = null;		// 검색 미디어렙

		// 상세정보용
		private string _AdvertiserCode          = null;		// 광고주 아이디		
		private string _AdvertiserName          = null;		// 광고주명	
		private string _RapCode	 		        = null;		// 대행사구분
		private string _RapName			        = null;		// 대행사구분
		private string _Comment                 = null;		// 비고		
		private string _UseYn                   = null;		// 사용여부		
		private string _RegDt                   = null;		// 최초등록일		
		private string _ModDt                   = null;		// 최종수정일
		private string _RegID                   = null;		// 최종수정일
        private string _JobCode                 = null;     // 업종코드        [A_01]
        private string _JobNameLevel1           = null;     // Level1 업종명   [A_01]
        private string _JobNameLevel2           = null;     // Level2 업종명   [A_01]
        private string _JobLevel                = null;     // 업종 레벨       [A_01]
        private string _JobUpperCode            = null;     // 업종상위코드    [A_01]

		// 목록조회용
		private DataSet  _AdvertiserDataSet;

		public AdvertiserModel() : base () 
		{
			Init();
		}

		#region Public 메소드
		public void Init()
		{
			_SearchKey		        = "";
			_SearchAdvertiserLevel  = "";
			_SearchchkAdState_10    = "";
			_SearchRap              = "";

			_AdvertiserCode	= "";			
			_AdvertiserName	= "";		
			_RapCode		= "";		
			_RapName		= "";		
			_Comment		= "";
			_UseYn		    = "";
			_RegDt		    = "";
			_ModDt		    = "";
			_RegID		    = "";
			_JobCode        = "";
            _JobNameLevel1  = "";
            _JobNameLevel2  = "";
            _JobLevel       = "";
            _JobUpperCode   = "";
       
			_AdvertiserDataSet = new DataSet();
		}

		#endregion

		#region  프로퍼티 

		public DataSet AdvertiserDataSet
		{
			get { return _AdvertiserDataSet;	}
			set { _AdvertiserDataSet = value;	}
		}

		public string SearchKey 
		{
			get { return _SearchKey;	}
			set { _SearchKey = value;	}
		}
	
		public string SearchAdvertiserLevel 
		{
			get { return _SearchAdvertiserLevel;	}
			set { _SearchAdvertiserLevel = value;	}
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

		public string AdvertiserCode 
		{
			get { return _AdvertiserCode;	}
			set { _AdvertiserCode = value;	}
		}

		public string AdvertiserName 
		{
			get { return _AdvertiserName;	}
			set { _AdvertiserName = value;	}
		}

		public string RapCode 
		{
			get { return _RapCode;	}
			set { _RapCode = value;	}
		}

		public string RapName 
		{
			get { return _RapName;	}
			set { _RapName = value;	}
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

		public string RegDt 
		{
			get { return _RegDt;	}
			set { _RegDt = value;	}
		}
		
		public string ModDt 
		{
			get { return _ModDt;	}
			set { _ModDt = value;	}
		}

		public string RegID 
		{
			get { return _RegID;	}
			set { _RegID = value;	}
		}

        /// <summary>
        /// 업종코드
        /// </summary>
        public string JobCode
        {
            get { return _JobCode;  }
            set { _JobCode = value; }
        }
        
        /// <summary>
        /// Level1 업종명
        /// </summary>
        public string JobNameLevel1  
        {
            get { return _JobNameLevel1;  }
            set { _JobNameLevel1 = value; }
        }

        /// <summary>
        /// Level2 업종명
        /// </summary>
        public string JobNameLevel2
        {
            get { return _JobNameLevel2; }
            set { _JobNameLevel2 = value; }
        }
        
        /// <summary>
        /// 업종코드 레벨
        /// </summary>
        public string JobLevel 
        {
            get { return _JobLevel;   }
            set { _JobLevel = value;  }
        }

        /// <summary>
        /// 상위업종코드
        /// </summary>
        public string JobUpperCode
        {
            get { return _JobUpperCode; }
            set { _JobUpperCode = value; }
        }
		#endregion

	}
}