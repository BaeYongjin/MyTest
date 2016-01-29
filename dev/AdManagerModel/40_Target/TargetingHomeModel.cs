using System;
using System.Data;

using WinFramework.Base;

namespace AdManagerModel
{
	/// <summary>
	/// TargetingModel�� ���� ��� �����Դϴ�.
	/// </summary>
	public class TargetingHomeModel : BaseModel
	{

		// ��ȸ��
		private string _SearchKey            = null;		// �˻���
		private string _SearchMediaCode	     = null;		// �˻� ��ü
		private string _SearchRapCode        = null;		// �˻� ��
		private string _SearchAgencyCode     = null;		// �˻� �����
		private string _SearchAdvertiserCode = null;		// �˻� ������
		private string _SearchContractState  = null;		// �˻� ������
		private string _SearchAdClass        = null;		// �˻� ����뵵
		private string _SearchchkAdState_20  = null;		// �˻� ������� : ��
		private string _SearchchkAdState_30  = null;		// �˻� ������� : ����
		private string _SearchchkAdState_40  = null;		// �˻� ������� : ����
		private string _SearchchkTimeY	     = null;		// �˻� Ÿ���ÿ���Y
		private string _SearchchkTimeN	     = null;		// �˻� Ÿ���ÿ���N
		private string _SearchAdType         = null;		// �˻� ����뵵

		// ��������		
		private string _ItemNo				= null;		// ��೻��Key
		private string _ItemName			= null;		// �����
		private string _MediaCode           = null;		// ��ü�ڵ�
		private string _ContractAmt 		= null;		// �����๰��
		private string _PriorityCd 		    = null;		// ����켱�������
		private string _AmtControlYn        = null;		// �������� ��뿩��
		private string _AmtControlRate      = null;		// �������� ����
		private string _TgtRegion1Yn        = null;     // ��������_���� ��뿩��
		private string _TgtRegion1          = null;     // ��������_����
		private string _TgtSexYn            = null;     // ���⼺�� ��뿩��		
		private string _TgtTime             = null;     // ����ð���
		private string _TgtAgeYn            = null;		// ���⿬�ɴ� ��뿩��
		private string _TgtAge              = null;		// ���⿬�ɴ�
		private string _TgtAgeBtnYn         = null;		// ���⿬�ɱ��� ��뿩��
		private string _TgtAgeBtnBegin      = null;		// ���⿬�ɱ��� ����
		private string _TgtAgeBtnEnd        = null;		// ���⿬�ɱ��� ��
		private string _TgtTimeYn           = null;	    // ����ð��� ��뿩��
		private string _TgtSexMan           = null;		// ���⼺�� ����
		private string _TgtSexWoman         = null;		// ���⼺�� ����
		private string _TgtRateYn           = null;     // ������
		private string _TgtRate             = null;     // ������
		private string _TgtWeekYn           = null;     // ���Ϻ� ����
		private string _TgtWeek             = null;     // ���Ϻ�
		private string _TgtCollectionYn     = null;     // ���Ϻ� ����
		private string _TgtCollection       = null;     // ���Ϻ�
        private string _TgtStbModelYn            = null;     // ��ž�𵨺� ����
        private string _TgtStbModel              = null;     // ��ž�𵨺�
        private string _TgtPocYn            = null;     // POC ����
        private string _TgtPoc              = null;     // POC


		// �����ȸ��
		private DataSet  _DataSet;
		private DataSet  _DataSet1;
		private DataSet  _DataSet2;
		private DataSet  _DataSet3;


        // ����Ÿ���ÿ�
        private DataSet _CollectionsDataSet;
        private DataSet _TargetingCollectionDataSet;

        private string _CollectionCode = null; 

		public TargetingHomeModel() : base () 
		{
			Init();
		}

		#region Public �޼ҵ�
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

		#region  ������Ƽ 

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

        #region ����Ÿ����
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