using System;
using System.Data;

using WinFramework.Base;

namespace AdManagerModel
{
	/// <summary>
	/// TargetingModel에 대한 요약 설명입니다.
	/// </summary>
	public class TargetingHomeModel : BaseModel
	{

		// 조회용
		private string _SearchKey            = null;		// 검색어
		private string _SearchMediaCode	     = null;		// 검색 매체
		private string _SearchRapCode        = null;		// 검색 랩
		private string _SearchAgencyCode     = null;		// 검색 대행사
		private string _SearchAdvertiserCode = null;		// 검색 광고주
		private string _SearchContractState  = null;		// 검색 계약상태
		private string _SearchAdClass        = null;		// 검색 광고용도
		private string _SearchchkAdState_20  = null;		// 검색 광고상태 : 편성
		private string _SearchchkAdState_30  = null;		// 검색 광고상태 : 중지
		private string _SearchchkAdState_40  = null;		// 검색 광고상태 : 종료
		private string _SearchchkTimeY	     = null;		// 검색 타겟팅여부Y
		private string _SearchchkTimeN	     = null;		// 검색 타겟팅여부N
		private string _SearchAdType         = null;		// 검색 광고용도

		// 상세정보용		
		private string _ItemNo				= null;		// 계약내역Key
		private string _ItemName			= null;		// 광고명
		private string _MediaCode           = null;		// 매체코드
		private string _ContractAmt 		= null;		// 노출계약물량
		private string _PriorityCd 		    = null;		// 노출우선순위등급
		private string _AmtControlYn        = null;		// 노출제어 사용여부
		private string _AmtControlRate      = null;		// 노출제어 비율
		private string _TgtRegion1Yn        = null;     // 노출지역_행정 사용여부
		private string _TgtRegion1          = null;     // 노출지역_행정
		private string _TgtSexYn            = null;     // 노출성별 사용여부		
		private string _TgtTime             = null;     // 노출시간대
		private string _TgtAgeYn            = null;		// 노출연령대 사용여부
		private string _TgtAge              = null;		// 노출연령대
		private string _TgtAgeBtnYn         = null;		// 노출연령구간 사용여부
		private string _TgtAgeBtnBegin      = null;		// 노출연령구간 시작
		private string _TgtAgeBtnEnd        = null;		// 노출연령구간 끝
		private string _TgtTimeYn           = null;	    // 노출시간대 사용여부
		private string _TgtSexMan           = null;		// 노출성별 남자
		private string _TgtSexWoman         = null;		// 노출성별 여자
		private string _TgtRateYn           = null;     // 노출등급
		private string _TgtRate             = null;     // 노출등급
		private string _TgtWeekYn           = null;     // 요일별 여부
		private string _TgtWeek             = null;     // 요일별
		private string _TgtCollectionYn     = null;     // 요일별 여부
		private string _TgtCollection       = null;     // 요일별
        private string _TgtStbModelYn            = null;     // 셋탑모델별 여부
        private string _TgtStbModel              = null;     // 셋탑모델별
        private string _TgtPocYn            = null;     // POC 여부
        private string _TgtPoc              = null;     // POC


		// 목록조회용
		private DataSet  _DataSet;
		private DataSet  _DataSet1;
		private DataSet  _DataSet2;
		private DataSet  _DataSet3;


        // 고객군타겟팅용
        private DataSet _CollectionsDataSet;
        private DataSet _TargetingCollectionDataSet;

        private string _CollectionCode = null; 

		public TargetingHomeModel() : base () 
		{
			Init();
		}

		#region Public 메소드
		public void Init()
		{
			_SearchKey		       = "";
			_SearchMediaCode 	   = "";
			_SearchRapCode         = "";
			_SearchAgencyCode      = "";
			_SearchAdvertiserCode  = "";
			_SearchContractState   = "";
			_SearchAdClass         = "";
			_SearchchkAdState_20   = "";
			_SearchchkAdState_30   = "";
			_SearchchkAdState_40   = "";
			_SearchchkTimeY		   = "";
			_SearchchkTimeN		   = "";
			_SearchAdType          = "";
			
			_ItemNo				   = "";
			_ItemName              = "";
			_MediaCode             = "";
			            
			_ContractAmt 	       = "";
			_PriorityCd 		   = "";
			_AmtControlYn          = "";
			_AmtControlRate        = "";
			_TgtRegion1Yn          = "";
			_TgtRegion1            = "";
			_TgtSexYn             = "";			
			_TgtTime               = "";
			_TgtAgeYn              = "";
			_TgtAge                = "";
			_TgtAgeBtnYn           = "";
			_TgtAgeBtnBegin        = "";
			_TgtAgeBtnEnd          = "";
			_TgtTimeYn             = "";
			_TgtSexMan             = "";
			_TgtSexWoman           = "";
			_TgtRateYn             = "";
			_TgtRate               = "";
			_TgtWeekYn             = "";
			_TgtWeek               = "";
			_TgtCollectionYn       = "";
			_TgtCollection         = "";
            _TgtStbModelYn         = "";
            _TgtStbModel           = "";
            _TgtPocYn              = "";
            _TgtPoc                = "";

			_DataSet = new DataSet();
			_DataSet1 = new DataSet();
			_DataSet2 = new DataSet();
			_DataSet3 = new DataSet();

            _CollectionsDataSet = new DataSet();
            _TargetingCollectionDataSet = new DataSet();

            _CollectionCode = "";

        }

		#endregion

		#region  프로퍼티 

		public DataSet TargetingDataSet
		{
			get { return _DataSet;	}
			set { _DataSet = value;	}
		}
		public DataSet DetailDataSet
		{
			get { return _DataSet1;	}
			set { _DataSet1 = value;	}
		}

		public DataSet RegionDataSet
		{
			get { return _DataSet2;	}
			set { _DataSet2 = value;	}
		}

		public DataSet AgeDataSet
		{
			get { return _DataSet3;	}
			set { _DataSet3 = value;	}
		}

		public string SearchKey 
		{
			get { return _SearchKey;	}
			set { _SearchKey = value;	}
		}
	
		public string SearchMediaCode 
		{
			get { return _SearchMediaCode;	}
			set { _SearchMediaCode = value;	}
		}

		public string SearchRapCode 
		{
			get { return _SearchRapCode;		}
			set { _SearchRapCode = value;		}
		}

		public string SearchAgencyCode 
		{
			get { return _SearchAgencyCode;		}
			set { _SearchAgencyCode = value;	}
		}

		public string SearchAdvertiserCode 
		{
			get { return _SearchAdvertiserCode;		}
			set { _SearchAdvertiserCode = value;	}
		}

		public string SearchContractState 
		{
			get { return _SearchContractState;		}
			set { _SearchContractState = value;		}
		}

		public string SearchAdClass 
		{
			get { return _SearchAdClass;		}
			set { _SearchAdClass = value;		}
		}

		public string SearchAdType 
		{
			get { return _SearchAdType;		}
			set { _SearchAdType = value;		}
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
		public string SearchchkTimeY 
		{
			get { return _SearchchkTimeY;		}
			set { _SearchchkTimeY = value;		}
		}
		public string SearchchkTimeN 
		{
			get { return _SearchchkTimeN;		}
			set { _SearchchkTimeN = value;		}
		}

		public string ItemNo
		{
			get { return _ItemNo;	}
			set { _ItemNo = value;	}
		}

		public string ItemName
		{
			get { return _ItemName;	}
			set { _ItemName = value;	}
		}

		public string MediaCode
		{
			get { return _MediaCode;	}
			set { _MediaCode = value;	}
		}

		public string ContractAmt
		{
			get { return _ContractAmt;	}
			set { _ContractAmt = value;	}
		}

		public string PriorityCd
		{
			get { return _PriorityCd;	}
			set { _PriorityCd = value;	}
		}

		public string AmtControlYn
		{
			get { return _AmtControlYn;	}
			set { _AmtControlYn = value;	}
		}

		public string AmtControlRate
		{
			get { return _AmtControlRate;	}
			set { _AmtControlRate = value;	}
		}

		public string TgtRegion1Yn
		{
			get { return _TgtRegion1Yn;	}
			set { _TgtRegion1Yn = value;	}
		}

		public string TgtRegion1
		{
			get { return _TgtRegion1;	}
			set { _TgtRegion1 = value;	}
		}

		public string TgtTimeYn
		{
			get { return _TgtTimeYn;	}
			set { _TgtTimeYn = value;	}
		}
		
		public string TgtTime
		{
			get { return _TgtTime;	}
			set { _TgtTime = value;	}
		}

		public string TgtAgeYn
		{
			get { return _TgtAgeYn;	}
			set { _TgtAgeYn = value;	}
		}

		public string TgtAge
		{
			get { return _TgtAge;	}
			set { _TgtAge = value;	}
		}

		public string TgtAgeBtnYn
		{
			get { return _TgtAgeBtnYn;	}
			set { _TgtAgeBtnYn = value;	}
		}

		public string TgtAgeBtnBegin
		{
			get { return _TgtAgeBtnBegin;	}
			set { _TgtAgeBtnBegin = value;	}
		}

		public string TgtAgeBtnEnd
		{
			get { return _TgtAgeBtnEnd;	}
			set { _TgtAgeBtnEnd = value;	}
		}

		public string TgtSexYn
		{
			get { return _TgtSexYn;	}
			set { _TgtSexYn = value;	}
		}

		public string TgtSexMan
		{
			get { return _TgtSexMan;	}
			set { _TgtSexMan = value;	}
		}

		public string TgtSexWoman
		{
			get { return _TgtSexWoman;	}
			set { _TgtSexWoman = value;	}
		}

		public string TgtRateYn
		{
			get { return _TgtRateYn;	}
			set { _TgtRateYn = value;	}
		}

		public string TgtRate
		{
			get { return _TgtRate;	}
			set { _TgtRate = value;	}
		}

		public string TgtWeekYn
		{
			get { return _TgtWeekYn;	}
			set { _TgtWeekYn = value;	}
		}

		public string TgtWeek
		{
			get { return _TgtWeek;	}
			set { _TgtWeek = value;	}
		}

		public string TgtCollectionYn
		{
			get { return _TgtCollectionYn;	}
			set { _TgtCollectionYn = value;	}
		}

		public string TgtCollection
		{
			get { return _TgtCollection;	}
			set { _TgtCollection = value;	}
		}

        public string TgtStbModelYn
        {
            get { return _TgtStbModelYn; }
            set { _TgtStbModelYn = value; }
        }
        public string TgtStbModel
        {
            get { return _TgtStbModel; }
            set { _TgtStbModel = value; }
        }
        public string TgtPocYn
        {
            get { return _TgtPocYn; }
            set { _TgtPocYn = value; }
        }
        public string TgtPoc
        {
            get { return _TgtPoc; }
            set { _TgtPoc = value; }
        }

		#endregion

        #region 고객군타겟팅
        public DataSet CollectionsDataSet
        {
            get { return _CollectionsDataSet; }
            set { _CollectionsDataSet = value; }
        }

        public DataSet TargetingCollectionDataSet
        {
            get { return _TargetingCollectionDataSet; }
            set { _TargetingCollectionDataSet = value; }
        }

        public string CollectionCode
        {
            set { _CollectionCode = value; }
            get { return _CollectionCode; }
        }
        #endregion

    }
}