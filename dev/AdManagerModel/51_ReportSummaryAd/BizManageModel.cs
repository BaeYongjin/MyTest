// ===============================================================================
// Contract Data Model for Charites Project
//
// BizManageModel.cs
//
// 컨텐츠정보 클래스를 정의합니다. 
//
// ===============================================================================
// Release history
// 2015.11.10 김진호 v1.0
// ===============================================================================
// Copyright (C) 2015 Dartmedia Inc.
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
    public class BizManageModel : BaseModel
    {

        // 조회용
        private string _SearchKey      = null;	    // 검색어
        private string _SearchStartDay = null;	    // 검색 범위 시작 날짜
        private string _SearchEndDay   = null;	    // 검색 범위 종료 날짜

        private string _SearchRapCode        = null;      // 검색용 미디어렙 코드
        private string _SearchAgencyCode     = null;      // 검색용 대행사 코드
        private string _SearchAdvertiserCode = null;      // 검색용 광고주 코드
        private string _SearchAdType         = null;      // 검색용 광고종류 코드

        // 상세정보용
        private string _AdTypeName	    = null;     // 구분1
        private string _AdvertiserName  = null;     // 광고주 법인명
        private string _BrandName       = null;     // 브랜드명
        private string _JobClassName    = null;     // 업종분류기타
        private string _RapName         = null;     // 미디어렙
        private string _AgencyName      = null;     // 광고대행사명
        private string _StartDay        = null;     // 집행시작일
        private string _EndDay          = null;     // 집행종료일
        private string _EndMonth        = null;     // 집행종료일 기준 월
        private string _Pirce           = null;     // 매출금액
        private string _AdTime          = null;     // 초수
        private string _OfferExpCnt     = null;     // 보장노출수
        private string _RealExpCnt      = null;     // 실집행노출수
        private string _OfferCPM        = null;     // 제안 CPM
        private string _RealCPM         = null;     // 결과 CPM

        // 목록조회용
        private DataSet _BizManageDataSet;

        public BizManageModel() : base() 
        {
            Init();
        }

        #region Public 메소드
        public void Init()
        {
            _SearchKey      = "";
            _SearchStartDay = "";
            _SearchEndDay   = "";

            _SearchRapCode        = "";
            _SearchAgencyCode     = "";
            _SearchAdvertiserCode = "";
            _SearchAdType         = "";

            _AdTypeName     = "";
            _AdvertiserName = "";
            _BrandName      = "";
            _JobClassName   = "";
            _RapName        = "";
            _AgencyName     = "";
            _StartDay       = "";
            _EndDay         = "";
            _EndMonth       = "";
            _Pirce          = "";
            _AdTime         = "";
            _OfferExpCnt    = "";
            _RealExpCnt     = "";
            _OfferCPM       = "";
            _RealCPM        = "";

            _BizManageDataSet = new DataSet();
        }

        #endregion

        #region  프로퍼티 

        public string SearchKey
		{
            get { return _SearchKey; }
            set { _SearchKey = value; }
		}

        public string SearchStartDay
        {
            get { return _SearchStartDay; }
            set { _SearchStartDay = value; }
        }

        public string SearchEndDay
        {
            get { return _SearchEndDay; }
            set { _SearchEndDay = value; }
        }

        public string SearchRapCode
        {
            get { return _SearchRapCode; }
            set { _SearchRapCode = value; }
        }

        public string SearchAgencyCode
        {
            get { return _SearchAgencyCode; }
            set { _SearchAgencyCode = value; }
        }

        public string SearchAdvertiserCode
        {
            get { return _SearchAdvertiserCode; }
            set { _SearchAdvertiserCode = value; }
        }

        public string SearchAdType
        {
            get { return _SearchAdType; }
            set { _SearchAdType = value; }
        }     

        public DataSet BizManageDataSet
        {
            get { return _BizManageDataSet; }
            set { _BizManageDataSet = value; }
        }

        public string AdTypeName 
        {
            get { return _AdTypeName; }
            set { _AdTypeName = value; }
        }

        public string AdvertiserName
        {
            get { return _AdvertiserName; }
            set { _AdvertiserName = value; }
        }
        public string BrandName
        {
            get { return _BrandName; }
            set { _BrandName = value; }
        }

        public string JobClassName 
        {
            get { return _JobClassName; }
            set { _JobClassName = value; }
        }

        public string RapName
        {
            get { return _RapName; }
            set { _RapName = value; }
        }

        public string AgencyName 
        {
            get { return _AgencyName; }
            set { _AgencyName = value; }
        }

        public string StartDay           
        {
            get { return _StartDay; }
            set { _StartDay = value; }
        }

        public string EndDay 
        {
            get { return _EndDay; }
            set { _EndDay = value; }
        }

        public string EndMonth 
        {
            get { return _EndMonth; }
            set { _EndMonth = value; }
        }

        public string Pirce 
        {
            get { return _Pirce; }
            set { _Pirce = value; }
        }

        public string AdTime 
        {
            get { return _AdTime; }
            set { _AdTime = value; }
        }

        public string OfferExpCnt 
        {
            get { return _OfferExpCnt; }
            set { _OfferExpCnt = value; }
        }

        public string RealExpCnt 
        {
            get { return _RealExpCnt; }
            set { _RealExpCnt = value; }
        }

        public string OfferCPM 
        {
            get { return _OfferCPM; }
            set { _OfferCPM = value; }
        }

        public string RealCPM 
        {
            get { return _RealCPM; }
            set { _RealCPM = value; }
        }
        #endregion

    }
}
