// ===============================================================================
// UserInfo Data Model for Charites Project
//
// UserInfoModel.cs
//
// 사용자정보 클래스를 정의합니다. 
//
// ===============================================================================
// Release history
// 2010/07/12 장용석 카테고리정보 추가(Inventory관련)
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
	public class AnalysisItemGroupModel : BaseModel
	{
        // 조회 용
        private string _SearchKey = null;		// 검색어
        private string _SearchMonth = null;
        private string _RapCode = null;
        private string _AgencyCode = null;
        private string _AdvertiserCode = null;
        private int _AnalysisItemGroupNo = 0;

        // 상세정보용
        private string _AnalysisItemGroupName = null;
        private string _AnalysisItemGroupMonth = null;
        private string _AnalysisItemGroupType = null;
        private string _Comment = null;
        private string _UserID = null;
        private int _ItemNo = 0;
	
		// 목록조회용
        private DataSet _AnalysisMonthsDataSet;
        private DataSet _AnalysisItemGroupDataSet;
        private DataSet _AnalysisItemGroupDetailDataSet;
        private DataSet _ContractItemDataSet;

        public AnalysisItemGroupModel()
            : base() 
		{
			Init();
		}

		#region Public 메소드
		public void Init()
		{
            _SearchKey = null;		// 검색어
            _SearchMonth = null;
            _RapCode = null;
            _AgencyCode = null;
            _AdvertiserCode = null;
            _AnalysisItemGroupNo = 0;

            // 상세정보용

            _AnalysisItemGroupName = null;
            _AnalysisItemGroupMonth = null;
            _AnalysisItemGroupType = null;
            _Comment = null;
            _UserID = null;
            _ItemNo = 0;
			            
			_AnalysisMonthsDataSet = new DataSet();
            _AnalysisItemGroupDataSet = new DataSet();
            _AnalysisItemGroupDetailDataSet = new DataSet();
            _ContractItemDataSet = new DataSet();

		}

		#endregion

		#region  프로퍼티 

        public DataSet AnalysisMonthsDataSet
		{
            get { return _AnalysisMonthsDataSet; }
            set { _AnalysisMonthsDataSet = value; }
		}

        public DataSet AnalysisItemGroupDataSet
        {
            get { return _AnalysisItemGroupDataSet; }
            set { _AnalysisItemGroupDataSet = value; }
        }

        public DataSet AnalysisItemGroupDetailDataSet
        {
            get { return _AnalysisItemGroupDetailDataSet; }
            set { _AnalysisItemGroupDetailDataSet = value; }
        }

        public DataSet ContractItemDataSet
        {
            get { return _ContractItemDataSet; }
            set { _ContractItemDataSet = value; }
        }

		public string SearchKey 
		{
			get { return _SearchKey;	}
			set { _SearchKey = value;	}
		}

        public string SearchMonth
        {
            get { return _SearchMonth; }
            set { _SearchMonth = value; }
        }

        public string RapCode
        {
            get { return _RapCode; }
            set { _RapCode = value; }
        }

        public string AgencyCode
        {
            get { return _AgencyCode; }
            set { _AgencyCode = value; }
        }

        public string AdvertiserCode
        {
            get { return _AdvertiserCode; }
            set { _AdvertiserCode = value; }
        }

        public int AnalysisItemGroupNo
        {
            get { return _AnalysisItemGroupNo; }
            set { _AnalysisItemGroupNo = value; }
        }



        // 상세정보용

        public string AnalysisItemGroupName
        {
            get { return _AnalysisItemGroupName; }
            set { _AnalysisItemGroupName = value; }
        }

        public string AnalysisItemGroupMonth
        {
            get { return _AnalysisItemGroupMonth; }
            set { _AnalysisItemGroupMonth = value; }
        }

        public string AnalysisItemGroupType
        {
            get { return _AnalysisItemGroupType; }
            set { _AnalysisItemGroupType = value; }
        }

        public string Comment
        {
            get { return _Comment; }
            set { _Comment = value; }
        }

        public string UserID
        {
            get { return _UserID; }
            set { _UserID = value; }
        }

        public int ItemNo
        {
            get { return _ItemNo; }
            set { _ItemNo = value; }
        }

		#endregion

	}
}