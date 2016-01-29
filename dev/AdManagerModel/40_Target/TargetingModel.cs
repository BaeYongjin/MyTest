/*
 * -------------------------------------------------------
 * Class Name: TargetingModel
 * 주요기능  : 상업광고 타겟팅 data Model
 * 작성자    : 모름
 * 작성일    : 모름
 * 특이사항  : 
 * -------------------------------------------------------
 * [수정사항]
 * -------------------------------------------------------
 * 수정코드  : [E_01]
 * 수정자    : bae
 * 수정일    : 2010.10.04
 * 수정부분  :
 *			  - _SlotExt 변수 추가
 *            - SlotExt 속성 추가
 * 수정내용  : 
 *            - 2Slot 지원여부 및 지원한 광고의 slot위치구분
 *              정보를 멤버변수및 속성추가           
 * --------------------------------------------------------
 * 수정코드  : [E_02]
 * 수정자    : 김보배
 * 수정일    : 2013.04.01
 * 수정부분  :
 *			  - 셋탑모델 사용여부, 셋탑모델정보 Model 추가
 * --------------------------------------------------------
 * 수정코드  : [E_03]
 * 수정자    : 김보배
 * 수정일    : 2013.07.09
 * 수정부분  :
 *			  - 선호도조사팝업 사용여부, 송출비율, 응답자미송출 추가
 * --------------------------------------------------------
 * 수정코드  : [E_04]
 * 수정자    : 김보배
 * 수정일    : 2013.10.16
 * 수정부분  :
 *			  - 프로파일타겟팅 사용여부, 연령대구분, 신뢰도 추가
 * --------------------------------------------------------
 * 수정코드  : [E_05]
 * 수정자    : HJ
 * 수정일    : 2015.06.02
 * 수정부분  :
 *			  - 셋탑모델별, POC별 타겟팅
 * --------------------------------------------------------
 */

using System;
using System.Data;

using WinFramework.Base;

namespace AdManagerModel
{
    /// <summary>
    /// TargetingModel에 대한 요약 설명입니다.
    /// </summary>
    public class TargetingModel : BaseModel
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
		private string _TgtTimeYn           = null;     // 노출시간대 사용여부
		private string _TgtTime             = null;     // 노출시간대
		private string _TgtAgeYn            = null;		// 노출연령대 사용여부
		private string _TgtAge              = null;		// 노출연령대
		private string _TgtAgeBtnYn         = null;		// 노출연령구간 사용여부
		private string _TgtAgeBtnBegin      = null;		// 노출연령구간 시작
		private string _TgtAgeBtnEnd        = null;		// 노출연령구간 끝
		private string _TgtSexYn            = null;		// 노출성별 사용여부
		private string _TgtSexMan           = null;		// 노출성별 남자
		private string _TgtSexWoman         = null;		// 노출성별 여자
        private string _TgtStbModelYn       = null;     // [E_02] 셋탑모델 사용여부
        private string _TgtStbModel         = null;     // [E_02]셋탑모델정보
		private string _TgtRateYn           = null;     // 노출등급
		private string _TgtRate             = null;     // 노출등급
		private string _TgtWeekYn           = null;     // 요일별 여부
		private string _TgtWeek             = null;     // 요일별
		private string _TgtCollectionYn     = null;     // 요일별 여부
		private string _TgtCollection       = null;     // 요일별
		private	string	_TgtZipYn			= null;
		private string  _TgtZip				= null;
		private	string	_TgtPPxYn			= null;		// 유료채널 집행여부
		private string	_TgtFreqYn			= null;
		private int		_TgtFreqDay			= 0;
		private int		_TgtFreqPeriod		= 0;	
		private	string	_TgtPVSYn			= null;
        private string _TgtPrefYn           = null;     // [E_03] 선호도조사팝업 사용여부
        private int    _TgtPrefRate         = 0;        // [E_03] 선호도조사팝업 송출비율
        private string _TgtPrefNosend       = null;     // [E_03] 선호도조사팝업 응답자미송출
        private string _TgtProfileYn        = null;     // [E_04] 프로파일 타겟팅 사용여부
        private string _TgtProfile          = null;     // [E_04] 프로파일 타겟팅 연령대 구분
        private int    _TgtReliablilty      = 0;        // [E_04] 프로파일 타겟팅 신뢰도
        private string _TgtPocYn            = null;     // [E_05] POC 여부
        private string _TgtPoc              = null;     // [E_05] POC

		private int     _SlotExt            = 0;        // 2Slot 및 위치(0:무시,3:2SLOT앞, 6:2Slot 뒤)[E_01]

		//비율용
		private string _Type              = null;     // 타입
		private string _Rate1             = null;     // 타입
		private string _Rate2             = null;     // 타입
		private string _Rate3             = null;     // 타입
		private string _Rate4             = null;     // 타입
		private string _Rate5             = null;     // 타입
		private string _Rate6             = null;     // 타입
		private string _Rate7             = null;     // 타입
		private string _Rate8             = null;     // 타입
		private string _Rate9             = null;     // 타입
		private string _Rate10             = null;     // 타입
		private string _Rate11             = null;     // 타입
		private string _Rate12             = null;     // 타입
		private string _Rate13             = null;     // 타입
		private string _Rate14             = null;     // 타입
		private string _Rate15             = null;     // 타입
		private string _Rate16             = null;     // 타입
		private string _Rate17             = null;     // 타입
		private string _Rate18             = null;     // 타입
		private string _Rate19             = null;     // 타입
		private string _Rate20             = null;     // 타입
		private string _Rate21             = null;     // 타입
		private string _Rate22             = null;     // 타입
		private string _Rate23             = null;     // 타입
		private string _Rate24             = null;     // 타입

        // 목록조회용
		private DataSet  _DataSet;
		private DataSet  _DataSet1;
		private DataSet  _DataSet2;
		private DataSet  _DataSet3;
		private DataSet  _DataSet4;
        private DataSet _CollectionsDataSet;
        private DataSet _TargetingCollectionDataSet;

        // 고객군타겟팅 저장/삭제용
        private string _CollectionCode = null; 

        public TargetingModel() : base () 
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
			_TgtTimeYn             = "";
			_TgtTime               = "";
			_TgtAgeYn              = "";
			_TgtAge                = "";
			_TgtAgeBtnYn           = "";
			_TgtAgeBtnBegin        = "";
			_TgtAgeBtnEnd          = "";
			_TgtSexYn              = "";
			_TgtSexMan             = "";
			_TgtSexWoman           = "";
			_TgtRateYn             = "";
			_TgtRate               = "";
			_TgtWeekYn             = "";
			_TgtWeek               = "";
			_TgtCollectionYn	= "";
			_TgtCollection      = "";
			_TgtZipYn			= "";
			_TgtZip				= "";
			_TgtPPxYn			= "";
			_TgtFreqYn			= "";
			_TgtFreqDay			= 0;
			_TgtFreqPeriod		= 0;
			_TgtPVSYn			= "";
            _TgtPocYn           = "";
            _TgtPoc             = "";

			_Type                = "";
			_Rate1               = "";
			_Rate2               = "";
			_Rate3               = "";
			_Rate4               = "";
			_Rate5               = "";
			_Rate6               = "";
			_Rate7               = "";
			_Rate8               = "";
			_Rate9               = "";
			_Rate10               = "";
			_Rate11               = "";
			_Rate12               = "";
			_Rate13               = "";
			_Rate14               = "";
			_Rate15               = "";
			_Rate16               = "";
			_Rate17               = "";
			_Rate18               = "";
			_Rate19               = "";
			_Rate20               = "";
			_Rate21               = "";
			_Rate22               = "";
			_Rate23               = "";
			_Rate24               = "";
               		
			_DataSet = new DataSet();
			_DataSet1 = new DataSet();
			_DataSet2 = new DataSet();
			_DataSet3 = new DataSet();
			_DataSet4 = new DataSet();
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

		public DataSet RateDataSet
		{
			get { return _DataSet4;	}
			set { _DataSet4 = value;	}
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

		public string TgtZipYn
		{
			get { return _TgtZipYn;	}
			set { _TgtZipYn = value;	}
		}

		public string TgtZip
		{
			get { return _TgtZip;	}
			set { _TgtZip = value;	}
		}

        /// <summary>
        /// [E_02] 셋탑정보 사용여부
        /// </summary>
        public string TgtStbModelYn
        {
            get { return _TgtStbModelYn; }
            set { _TgtStbModelYn = value; }
        }

        /// <summary>
        /// [E_02] 셋탑정보
        /// </summary>
        public string TgtStbModel
        {
            get { return _TgtStbModel;  }
            set { _TgtStbModel = value; }
        }

		/// <summary>
		/// 유료채널 광고집행여부 설정
		/// </summary>
		public string TgtPPxYn
		{
			get { return _TgtPPxYn;	}
			set { _TgtPPxYn = value;	}
		}

		public string TgtFreqYn
		{
			get { return _TgtFreqYn;	}
			set { _TgtFreqYn = value;	}
		}

		public int TgtFreqDay
		{
			get { return _TgtFreqDay;	}
			set { _TgtFreqDay = value;	}
		}

		public int	TgtFreqFeriod
		{
			get { return _TgtFreqPeriod;	}
			set { _TgtFreqPeriod = value;	}
		}

		/// <summary>
		/// 개인시청DB타겟팅 사용여부
		/// </summary>
		public string TgtPVSYn
		{
			get { return _TgtPVSYn;	}
			set { _TgtPVSYn = value;	}
		}

        /// <summary>
        /// [E_03] 선호도조사팝업 사용여부
        /// </summary>
        public string TgtPrefYn
        {
            get { return _TgtPrefYn; }
            set { _TgtPrefYn = value; }
        }

        /// <summary>
        /// [E_03] 선호도조사팝업 송출비율
        /// </summary>
        public int TgtPrefRate
        {
            get { return _TgtPrefRate; }
            set { _TgtPrefRate = value; }
        }

        /// <summary>
        /// [E_03] 선호도조사팝업 응답자미송출
        /// </summary>
        public string TgtPrefNosend
        {
            get { return _TgtPrefNosend; }
            set { _TgtPrefNosend = value; }
        }

        /// <summary>
        /// [E_04] 프로파일 타겟팅 사용여부
        /// </summary>
        public string TgtProfileYn
        {
            get { return _TgtProfileYn; }
            set { _TgtProfileYn = value; }
        }

        /// <summary>
        /// [E_04] 프로파일 타겟팅 연령대 구분
        /// </summary>
        public string TgtProfile
        {
            get { return _TgtProfile; }
            set { _TgtProfile = value; }
        }

        /// <summary>
        /// [E_04] 프로파일 타겟팅 신뢰도
        /// </summary>
        public int TgtReliablilty
        {
            get { return _TgtReliablilty; }
            set {_TgtReliablilty = value;}
        }

		public string Type
		{
			get { return _Type;	}
			set { _Type = value;	}
		}

		public string Rate1
		{
			get { return _Rate1;	}
			set { _Rate1 = value;	}
		}

		public string Rate2
		{
			get { return _Rate2;	}
			set { _Rate2 = value;	}
		}

		public string Rate3
		{
			get { return _Rate3;	}
			set { _Rate3 = value;	}
		}

		public string Rate4
		{
			get { return _Rate4;	}
			set { _Rate4 = value;	}
		}

		public string Rate5
		{
			get { return _Rate5;	}
			set { _Rate5 = value;	}
		}

		public string Rate6
		{
			get { return _Rate6;	}
			set { _Rate6 = value;	}
		}

		public string Rate7
		{
			get { return _Rate7;	}
			set { _Rate7 = value;	}
		}

		public string Rate8
		{
			get { return _Rate8;	}
			set { _Rate8 = value;	}
		}

		public string Rate9
		{
			get { return _Rate9;	}
			set { _Rate9 = value;	}
		}

		public string Rate10
		{
			get { return _Rate10;	}
			set { _Rate10 = value;	}
		}

		public string Rate11
		{
			get { return _Rate11;	}
			set { _Rate11 = value;	}
		}

		public string Rate12
		{
			get { return _Rate12;	}
			set { _Rate12 = value;	}
		}

		public string Rate13
		{
			get { return _Rate13;	}
			set { _Rate13 = value;	}
		}

		public string Rate14
		{
			get { return _Rate14;	}
			set { _Rate14 = value;	}
		}

		public string Rate15
		{
			get { return _Rate15;	}
			set { _Rate15 = value;	}
		}

		public string Rate16
		{
			get { return _Rate16;	}
			set { _Rate16 = value;	}
		}

		public string Rate17
		{
			get { return _Rate17;	}
			set { _Rate17 = value;	}
		}

		public string Rate18
		{
			get { return _Rate18;	}
			set { _Rate18 = value;	}
		}

		public string Rate19
		{
			get { return _Rate19;	}
			set { _Rate19 = value;	}
		}

		public string Rate20
		{
			get { return _Rate20;	}
			set { _Rate20 = value;	}
		}

		public string Rate21
		{
			get { return _Rate21;	}
			set { _Rate21 = value;	}
		}

		public string Rate22
		{
			get { return _Rate22;	}
			set { _Rate22 = value;	}
		}

		public string Rate23
		{
			get { return _Rate23;	}
			set { _Rate23 = value;	}
		}

		public string Rate24
		{
			get { return _Rate24;	}
			set { _Rate24 = value;	}
		}		

        #endregion

		/// <summary>
		/// 2Slot 지원여부및 그 위치(0:무시,3:2slot 앞, 6:2Slot 뒤)[E_01]
		/// </summary>
		public int SlotExt
		{
			set{_SlotExt = value;}
			get{return _SlotExt;}
		}

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

    }
}