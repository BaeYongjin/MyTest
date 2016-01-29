// ===============================================================================
// UserInfo Data Model for Charites Project
//
// UserInfoModel.cs
//
// ��������� Ŭ������ �����մϴ�. 
//
// ===============================================================================
// Release history
// 2010/07/12 ��뼮 ī�װ����� �߰�(Inventory����)
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
	/// ����������� Ŭ���� ��.
	/// </summary>
	public class AnalysisItemGroupModel : BaseModel
	{
        // ��ȸ ��
        private string _SearchKey = null;		// �˻���
        private string _SearchMonth = null;
        private string _RapCode = null;
        private string _AgencyCode = null;
        private string _AdvertiserCode = null;
        private int _AnalysisItemGroupNo = 0;

        // ��������
        private string _AnalysisItemGroupName = null;
        private string _AnalysisItemGroupMonth = null;
        private string _AnalysisItemGroupType = null;
        private string _Comment = null;
        private string _UserID = null;
        private int _ItemNo = 0;
	
		// �����ȸ��
        private DataSet _AnalysisMonthsDataSet;
        private DataSet _AnalysisItemGroupDataSet;
        private DataSet _AnalysisItemGroupDetailDataSet;
        private DataSet _ContractItemDataSet;

        public AnalysisItemGroupModel()
            : base() 
		{
			Init();
		}

		#region Public �޼ҵ�
		public void Init()
		{
            _SearchKey = null;		// �˻���
            _SearchMonth = null;
            _RapCode = null;
            _AgencyCode = null;
            _AdvertiserCode = null;
            _AnalysisItemGroupNo = 0;

            // ��������

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

		#region  ������Ƽ 

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



        // ��������

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