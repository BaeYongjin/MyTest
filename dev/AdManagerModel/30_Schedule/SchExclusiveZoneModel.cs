// ===============================================================================
// AdGroup Data Model for Charites Project
//
// AdGroupModel.cs
//
// 시간대독점편성 클래스를 정의합니다. 
//
// ===============================================================================
// Release history
// 2011.12.27 박재현 v1.0
// ===============================================================================
// Copyright (C) 2007 G-Inno Systems Inc.
// All rights reserved.
// ==============================================================================
using System;
using System.Collections.Generic;
using System.Text;

using System.Data;
using WinFramework.Base;

namespace AdManagerModel
{
    public class SchExclusiveZoneModel : BaseModel
    {
       
        //시간대독점편성뷰어부분
        private string _ItemNo = null;     //광고 ID
        private string _ItemName = null;     //광주 명
        private string _ExecutStartDay = null;     //집행시작일
        private string _RealEndDay = null;     //실제종료일 
        private string _TgtTime = null;     //설정시간

        //시간대독점
        private DataSet _SchExclusiveDataSet;


        //조회용 ( 시간대 독점 편성 추가 )
        private string _SearchKey = null;		// 검색어
        private string _SearchMediaCode = null;		// 검색 매체
        private string _SearchRapCode = null;		// 검색 랩
        private string _SearchAgencyCode = null;		// 검색 대행사
        private string _SearchchkAdState_20 = null;		// 검색 광고상태 : 편성
        private string _SearchchkAdState_30 = null;		// 검색 광고상태 : 중지
        private string _SearchchkAdState_40 = null;		// 검색 광고상태 : 종료

        private string _ContractAmt = null;             //물량수량

        //목록
        private DataSet _TargetingDataSet;

        //수정셋
        private DataSet _SchExDetailDataSet;

        //선택수정
        private string _TgtTimeYn = null;   //시간 체크
        private string _TgtWeekYn = null;   //요일 체크
        private string _TgtWeek = null;     //요일 선택값

        public SchExclusiveZoneModel() : base()
        {
            Init();
        }

        #region 변수 초기화
        public void Init()
        {
            //시간대 독점 편성 뷰어 부분
            _ItemNo             = "";
            _ItemName           = "";
            _ExecutStartDay     = "";
            _RealEndDay         = "";
            _TgtTime            = "";

            _SchExclusiveDataSet = new DataSet();

            //추가버튼 조회
             _SearchKey = "";		// 검색어
             _SearchMediaCode = "";		// 검색 매체
             _SearchRapCode = "";		// 검색 랩
             _SearchAgencyCode = "";		// 검색 대행사
             _SearchchkAdState_20 = "";		// 검색 광고상태 : 편성
             _SearchchkAdState_30 = "";		// 검색 광고상태 : 중지
             _SearchchkAdState_40 = "";		// 검색 광고상태 : 종료

             _ContractAmt = "";             //물량 수량

            //목록
             _TargetingDataSet = new DataSet();
            
            //수정
             _SchExDetailDataSet = new DataSet();

            //선택 수정 
            _TgtTimeYn = "";           //시간대 선택 여부 
            _TgtWeekYn = "";           //요일 선택 여부 
            _TgtWeek = "";             //요일 값
        }
        #endregion

        #region 프로퍼티(속성)
            #region 시간대 독점 뷰어 부분 
            public DataSet SchExclusiveDataSet
            {
                get { return _SchExclusiveDataSet; }
                set { _SchExclusiveDataSet = value; }
            }

            //광고ID
            public string ItemNo
            {
                get { return _ItemNo; }
                set { _ItemNo = value; }
            }
        
            //광고명
            public string ItemName
            {
                get { return _ItemName; }
                set { _ItemName = value; }
            }

            //집행시작일
            public string ExecutStartDay
            {
                get { return _ExecutStartDay; }
                set { _ExecutStartDay = value; }
            }

            //실제종료일 
            public string RealEndDay
            {
                get { return _RealEndDay; }
                set { _RealEndDay = value; }
            }

            //시간 값
            public string TgtTime
            {
                get { return _TgtTime; }
                set { _TgtTime = value; }
            }
            #endregion

            #region 추가 버튼 조회   
            //검색어
            public string SearchKey
            {
                get { return _SearchKey; }
                set { _SearchKey = value; }
            }
            //검색매체
            public string SearchMediaCode
            {
                get { return _SearchMediaCode; }
                set { _SearchMediaCode = value; }
            }
            //검색 렙
            public string SearchRapCode
            {
                get { return _SearchRapCode; }
                set { _SearchRapCode = value; }
            }
            //검색 대행사
            public string SearchAgencyCode
            {
                get { return _SearchAgencyCode; }
                set { _SearchAgencyCode = value; }
            }
            // 검색 광고상태 : 편성
            public string SearchchkAdState_20
            {
                get { return _SearchchkAdState_20; }
                set { _SearchchkAdState_20 = value; }
            }
            // 검색 광고상태 : 중지
            public string SearchchkAdState_30
            {
                get { return _SearchchkAdState_30; }
                set { _SearchchkAdState_30 = value; }
            }
            // 검색 광고상태 : 종료
            public string SearchchkAdState_40
            {
                get { return _SearchchkAdState_40; }
                set { _SearchchkAdState_40 = value; }
            }

            //물량 수량
            public string ContractAmt
            {
                get { return _ContractAmt; }
                set { _ContractAmt = value; }
            }

            //목록
            public DataSet TargetingDataSet
            {
                get { return _TargetingDataSet; }
                set { _TargetingDataSet = value; }
            }
            #endregion

            //수정 뷰어 
            public DataSet SchExDetailDataSet
            {
                get { return _SchExDetailDataSet; }
                set { _SchExDetailDataSet = value; }
            }

            //시간체크
            public string TgtTimeYn
            {
                get { return _TgtTimeYn; }
                set { _TgtTimeYn = value; }
            }
            //요일 체크 
            public string TgtWeekYn
            {
                get { return _TgtWeekYn; }
                set { _TgtWeekYn = value; }
            }
            //요일 값
            public string TgtWeek
            {
                get { return _TgtWeek; }
                set { _TgtWeek = value; }
            }

        #endregion
    }
}
