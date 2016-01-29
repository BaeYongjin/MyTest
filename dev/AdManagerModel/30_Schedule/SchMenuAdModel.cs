
using System;
using System.Data;

using WinFramework.Base;

namespace AdManagerModel
{
    /// <summary>
    /// ����������� Ŭ���� ��.
    /// </summary>
    public class SchMenuAdModel : BaseModel
    {

        // ��ȸ��
        private string _SearchMediaCode    = null;		// �˻���ü

		private string _SearchKey            = null;		// �˻���		
		private string _SearchRapCode        = null;		// �˻� ��
		private string _SearchAgencyCode     = null;		// �˻� �����
		private string _SearchAdvertiserCode = null;		// �˻� ������
		private string _SearchContractState  = null;		// �˻� ������
		private string _SearchAdClass        = null;		// �˻� ����뵵
		private string _SearchchkAdState_10    = null;		// �˻� ������� : �غ�
		private string _SearchchkAdState_20    = null;		// �˻� ������� : ��
		private string _SearchchkAdState_30    = null;		// �˻� ������� : ����
		private string _SearchchkAdState_40    = null;		// �˻� ������� : ����

		// ����ȸ��
		private string _MediaCode     = null;
		private string _CategoryCode  = null;
		private string _GenreCode     = null;
		private string _ChannelNo     = null;
		private string _ItemNo        = null;
		private string _ContractSeq        = null;
		private string _ScheduleOrder = null;
		private string _ItemName      = null;
		private string _LastOrder     = null;
		
        // �����ȸ��
        private DataSet  _MenuAdDataSet;    

		// �����ȸ��
		private DataSet  _MenuDataSet;  
  
		// �����ȸ��
		private DataSet  _MenuContractDataSet;  

		// �����ȸ��
		private DataSet  _ChoiceMenuContractDataSet;  

		// �����ȸ�� ���� CSS�����
		private	DataSet	_ItemScheduleDataSet;
		

        // �̵�� �޺� ��ȸ��
			
        public SchMenuAdModel() : base () 
        {
            Init();
        }

        #region Public �޼ҵ�
        public void Init()
        {			
			_SearchKey		       = "";			
			_SearchRapCode         = "";
			_SearchAgencyCode      = "";
			_SearchAdvertiserCode  = "";
			_SearchContractState   = "";
			_SearchAdClass         = "";
			_SearchchkAdState_10   = "";
			_SearchchkAdState_20   = "";
			_SearchchkAdState_30   = "";
			_SearchchkAdState_40   = "";

            _SearchMediaCode = "";

			_MediaCode       = "";
			_CategoryCode    = "";
			_GenreCode       = "";
			_ChannelNo       = "";
			_ItemNo          = "";
			_ContractSeq          = "";
			_ScheduleOrder   = "";
			_ItemName        = "";
			_LastOrder       = "";

            _MenuAdDataSet = new DataSet();
			_MenuDataSet = new DataSet();
			_MenuContractDataSet = new DataSet();
			_ChoiceMenuContractDataSet = new DataSet();
			_ItemScheduleDataSet	= new DataSet();
        }

        #endregion

        #region  ������Ƽ 

		/// <summary>
		/// Ư������(CSS) ����� DS
		/// </summary>
		public DataSet ItemScheduleDataSet
		{
			get { return _ItemScheduleDataSet;	}
			set { _ItemScheduleDataSet = value;	}
		}

        public DataSet MenuAdDataSet
        {
            get { return _MenuAdDataSet;	}
            set { _MenuAdDataSet = value;	}
        }

		public DataSet MenuDataSet
		{
			get { return _MenuDataSet;	}
			set { _MenuDataSet = value;	}
		}

		public DataSet MenuContractDataSet
		{
			get { return _MenuContractDataSet;	}
			set { _MenuContractDataSet = value;	}
		}

		public DataSet ChoiceMenuContractDataSet
		{
			get { return _ChoiceMenuContractDataSet;	}
			set { _ChoiceMenuContractDataSet = value;	}
		}


		public string SearchKey 
		{
			get { return _SearchKey;	}
			set { _SearchKey = value;	}
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

		public string SearchchkAdState_10 
		{
			get { return _SearchchkAdState_10;		}
			set { _SearchchkAdState_10 = value;		}
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


        public string SearchMediaCode
        {
            get { return _SearchMediaCode;	}
            set { _SearchMediaCode = value;	}
        }

		public string ItemNo
		{
			get { return _ItemNo;	}
			set { _ItemNo = value;	}
		}

		public string ContractSeq
		{
			get { return _ContractSeq;	}
			set { _ContractSeq = value;	}
		}

		public string MediaCode
		{
			get { return _MediaCode;	}
			set { _MediaCode = value;	}
		}

		public string CategoryCode
		{
			get { return _CategoryCode;	}
			set { _CategoryCode = value;	}
		}

		public string GenreCode
		{
			get { return _GenreCode;	}
			set { _GenreCode = value;	}
		}

		public string ChannelNo
		{
			get { return _ChannelNo;	}
			set { _ChannelNo = value;	}
		}

		public string ScheduleOrder
		{
			get { return _ScheduleOrder;	}
			set { _ScheduleOrder = value;	}
		}

		public string ItemName
		{
			get { return _ItemName;	}
			set { _ItemName = value;	}
		}

		public string LastOrder
		{
			get { return _LastOrder;	}
			set { _LastOrder = value;	}
		}

		#endregion

    }
}
