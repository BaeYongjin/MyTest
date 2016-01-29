/*
 * -------------------------------------------------------
 * 클래스명  :  CollectionHomeModel
 * 주요기능  :  홈광고 타겟고객군 관리
 * 작성자    :  HJ
 * 작성일    :  2015.05.12
 * 특이사항  :  [OAP고도화] 홈광고전용 타겟고객군을 관리
 * -------------------------------------------------------
 */

using System;
using System.Data;

using WinFramework.Base;

namespace AdManagerModel
{
    /// <summary>
    /// 사용자정보의 클래스 모델.
    /// </summary>
    public class CollectionHomeModel : BaseModel
    {
        // 조회용
        private string _SearchKey = null;		// 검색어		
        private string _SearchchkAdState_10 = null;		// 검색 사용여부상태
        private string _SearchLevel = null;		// 검색 사용여부상태
        private bool _IsStbColl = false;    // 찾기 대상이 등록된 셋탑군인지

        // 상세정보용
        private string _CollectionCode = null;		// 랩 아이디
        private string _CollectionName = null;		// 랩 명				
        private string _RegDt = null;		// 등록일자
        private string _ModDt = null;		// 등록일자
        private string _RegID = null;		// 등록일자
        private string _Comment = null;		// 비고
        private string _UseYn = null;		// 사용여부
        private string _StartDay = null;    // 고객군시작일
        private string _ExpiryDay = null;   // 고객군만료일

        // PVS동기화부분
        private int _SeqNo = 0;		// PVS그룹코드

        // 고객리스트
        private string _UserId = null;		// 원래는 광고고객ID이나 가입자번호로 차용한다
        private string _StbId = null;
        private string _PostNo = null;
        private string _ServiceCode = null;
        private string _ResidentNo = null;
        private string _Sex = null;

        private string _PageSize = null;		// 등록일자
        private string _Page = null;		// 비고

        // 목록조회용
        private DataSet _CollectionHomeDataSet;
        private DataSet _ClientListDataSet;
        private DataSet _StbListDataSet;


        // 고객리스트 관리용
        private string _FromCode = null;
        private string _ToCode = null;
        private string _CopyMove = null;

        public CollectionHomeModel()
            : base()
        {
            Init();
        }

        #region Public 메소드
        public void Init()
        {
            _SearchKey = "";
            _SearchchkAdState_10 = "";
            _SearchLevel = "";

            _CollectionCode = "";
            _CollectionName = "";
            _RegDt = "";
            _ModDt = "";
            _RegDt = "";
            _RegID = "";
            _Comment = "";
            _UseYn = "";
            _StartDay = "";
            _ExpiryDay = "";

            _UserId = "";
            _StbId = "";
            _PostNo = "";
            _ServiceCode = "";
            _ResidentNo = "";
            _Sex = "";

            _PageSize = "";
            _Page = "";
            _IsStbColl = false;

            _CollectionHomeDataSet = new DataSet();
            _ClientListDataSet = new DataSet();
            _StbListDataSet = new DataSet();

            _FromCode = "";
            _ToCode = "";
            _CopyMove = "C";
        }

        #endregion

        #region  프로퍼티

        public DataSet CollectionHomeDataSet
        {
            get { return _CollectionHomeDataSet; }
            set { _CollectionHomeDataSet = value; }
        }

        public DataSet ClientListDataSet
        {
            get { return _ClientListDataSet; }
            set { _ClientListDataSet = value; }
        }

        public DataSet StbListDataSet
        {
            get { return _StbListDataSet; }
            set { _StbListDataSet = value; }
        }

        public string SearchKey
        {
            get { return _SearchKey; }
            set { _SearchKey = value; }
        }

        public string SearchchkAdState_10
        {
            get { return _SearchchkAdState_10; }
            set { _SearchchkAdState_10 = value; }
        }

        public string SearchLevel
        {
            get { return _SearchLevel; }
            set { _SearchLevel = value; }
        }

        public string CollectionCode
        {
            get { return _CollectionCode; }
            set { _CollectionCode = value; }
        }

        public string CollectionName
        {
            get { return _CollectionName; }
            set { _CollectionName = value; }
        }

        public string RegDt
        {
            get { return _RegDt; }
            set { _RegDt = value; }
        }

        public string ModDt
        {
            get { return _ModDt; }
            set { _ModDt = value; }
        }


        public string RegID
        {
            get { return _RegID; }
            set { _RegID = value; }
        }

        public string Comment
        {
            get { return _Comment; }
            set { _Comment = value; }
        }

        public string UseYn
        {
            get { return _UseYn; }
            set { _UseYn = value; }
        }

        public string StartDay
        {
            get { return _StartDay; }
            set { _StartDay = value; }
        }

        public string ExpiryDay
        {
            get { return _ExpiryDay; }
            set { _ExpiryDay = value; }
        }

        public int SeqNo
        {
            get { return _SeqNo; }
            set { _SeqNo = value; }
        }

        public string UserId
        {
            get { return _UserId; }
            set { _UserId = value; }
        }

        public string StbId
        {
            get { return _StbId; }
            set { _StbId = value; }
        }

        public string PostNo
        {
            get { return _PostNo; }
            set { _PostNo = value; }
        }

        public string ServiceCode
        {
            get { return _ServiceCode; }
            set { _ServiceCode = value; }
        }


        public string ResidentNo
        {
            get { return _ResidentNo; }
            set { _ResidentNo = value; }
        }

        public string Sex
        {
            get { return _Sex; }
            set { _Sex = value; }
        }

        public string PageSize
        {
            get { return _PageSize; }
            set { _PageSize = value; }
        }

        public string Page
        {
            get { return _Page; }
            set { _Page = value; }
        }

        public bool IsStbColl
        {
            get { return _IsStbColl; }
            set { _IsStbColl = value; }
        }


        // 고객리스트 관리용
        public string FromCode
        {
            get { return _FromCode; }
            set { _FromCode = value; }
        }

        public string ToCode
        {
            get { return _ToCode; }
            set { _ToCode = value; }
        }

        public string CopyMove
        {
            get { return _CopyMove; }
            set { _CopyMove = value; }
        }

        #endregion

    }
}
