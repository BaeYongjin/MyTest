/*
 * -------------------------------------------------------
 * Class Name: TargetingModel
 * �ֿ���  : ������� Ÿ���� data Model
 * �ۼ���    : ��
 * �ۼ���    : ��
 * Ư�̻���  : 
 * -------------------------------------------------------
 * [��������]
 * -------------------------------------------------------
 * �����ڵ�  : [E_01]
 * ������    : bae
 * ������    : 2010.10.04
 * �����κ�  :
 *			  - _SlotExt ���� �߰�
 *            - SlotExt �Ӽ� �߰�
 * ��������  : 
 *            - 2Slot �������� �� ������ ������ slot��ġ����
 *              ������ ��������� �Ӽ��߰�           
 * --------------------------------------------------------
 * �����ڵ�  : [E_02]
 * ������    : �躸��
 * ������    : 2013.04.01
 * �����κ�  :
 *			  - ��ž�� ��뿩��, ��ž������ Model �߰�
 * --------------------------------------------------------
 * �����ڵ�  : [E_03]
 * ������    : �躸��
 * ������    : 2013.07.09
 * �����κ�  :
 *			  - ��ȣ�������˾� ��뿩��, �������, �����ڹ̼��� �߰�
 * --------------------------------------------------------
 * �����ڵ�  : [E_04]
 * ������    : �躸��
 * ������    : 2013.10.16
 * �����κ�  :
 *			  - ��������Ÿ���� ��뿩��, ���ɴ뱸��, �ŷڵ� �߰�
 * --------------------------------------------------------
 * �����ڵ�  : [E_05]
 * ������    : HJ
 * ������    : 2015.06.02
 * �����κ�  :
 *			  - ��ž�𵨺�, POC�� Ÿ����
 * --------------------------------------------------------
 */

using System;
using System.Data;

using WinFramework.Base;

namespace AdManagerModel
{
    /// <summary>
    /// TargetingModel�� ���� ��� �����Դϴ�.
    /// </summary>
    public class TargetingModel : BaseModel
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
		private string _TgtTimeYn           = null;     // ����ð��� ��뿩��
		private string _TgtTime             = null;     // ����ð���
		private string _TgtAgeYn            = null;		// ���⿬�ɴ� ��뿩��
		private string _TgtAge              = null;		// ���⿬�ɴ�
		private string _TgtAgeBtnYn         = null;		// ���⿬�ɱ��� ��뿩��
		private string _TgtAgeBtnBegin      = null;		// ���⿬�ɱ��� ����
		private string _TgtAgeBtnEnd        = null;		// ���⿬�ɱ��� ��
		private string _TgtSexYn            = null;		// ���⼺�� ��뿩��
		private string _TgtSexMan           = null;		// ���⼺�� ����
		private string _TgtSexWoman         = null;		// ���⼺�� ����
        private string _TgtStbModelYn       = null;     // [E_02] ��ž�� ��뿩��
        private string _TgtStbModel         = null;     // [E_02]��ž������
		private string _TgtRateYn           = null;     // ������
		private string _TgtRate             = null;     // ������
		private string _TgtWeekYn           = null;     // ���Ϻ� ����
		private string _TgtWeek             = null;     // ���Ϻ�
		private string _TgtCollectionYn     = null;     // ���Ϻ� ����
		private string _TgtCollection       = null;     // ���Ϻ�
		private	string	_TgtZipYn			= null;
		private string  _TgtZip				= null;
		private	string	_TgtPPxYn			= null;		// ����ä�� ���࿩��
		private string	_TgtFreqYn			= null;
		private int		_TgtFreqDay			= 0;
		private int		_TgtFreqPeriod		= 0;	
		private	string	_TgtPVSYn			= null;
        private string _TgtPrefYn           = null;     // [E_03] ��ȣ�������˾� ��뿩��
        private int    _TgtPrefRate         = 0;        // [E_03] ��ȣ�������˾� �������
        private string _TgtPrefNosend       = null;     // [E_03] ��ȣ�������˾� �����ڹ̼���
        private string _TgtProfileYn        = null;     // [E_04] �������� Ÿ���� ��뿩��
        private string _TgtProfile          = null;     // [E_04] �������� Ÿ���� ���ɴ� ����
        private int    _TgtReliablilty      = 0;        // [E_04] �������� Ÿ���� �ŷڵ�
        private string _TgtPocYn            = null;     // [E_05] POC ����
        private string _TgtPoc              = null;     // [E_05] POC

		private int     _SlotExt            = 0;        // 2Slot �� ��ġ(0:����,3:2SLOT��, 6:2Slot ��)[E_01]

		//������
		private string _Type              = null;     // Ÿ��
		private string _Rate1             = null;     // Ÿ��
		private string _Rate2             = null;     // Ÿ��
		private string _Rate3             = null;     // Ÿ��
		private string _Rate4             = null;     // Ÿ��
		private string _Rate5             = null;     // Ÿ��
		private string _Rate6             = null;     // Ÿ��
		private string _Rate7             = null;     // Ÿ��
		private string _Rate8             = null;     // Ÿ��
		private string _Rate9             = null;     // Ÿ��
		private string _Rate10             = null;     // Ÿ��
		private string _Rate11             = null;     // Ÿ��
		private string _Rate12             = null;     // Ÿ��
		private string _Rate13             = null;     // Ÿ��
		private string _Rate14             = null;     // Ÿ��
		private string _Rate15             = null;     // Ÿ��
		private string _Rate16             = null;     // Ÿ��
		private string _Rate17             = null;     // Ÿ��
		private string _Rate18             = null;     // Ÿ��
		private string _Rate19             = null;     // Ÿ��
		private string _Rate20             = null;     // Ÿ��
		private string _Rate21             = null;     // Ÿ��
		private string _Rate22             = null;     // Ÿ��
		private string _Rate23             = null;     // Ÿ��
		private string _Rate24             = null;     // Ÿ��

        // �����ȸ��
		private DataSet  _DataSet;
		private DataSet  _DataSet1;
		private DataSet  _DataSet2;
		private DataSet  _DataSet3;
		private DataSet  _DataSet4;
        private DataSet _CollectionsDataSet;
        private DataSet _TargetingCollectionDataSet;

        // ����Ÿ���� ����/������
        private string _CollectionCode = null; 

        public TargetingModel() : base () 
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
        /// [E_02] ��ž���� ��뿩��
        /// </summary>
        public string TgtStbModelYn
        {
            get { return _TgtStbModelYn; }
            set { _TgtStbModelYn = value; }
        }

        /// <summary>
        /// [E_02] ��ž����
        /// </summary>
        public string TgtStbModel
        {
            get { return _TgtStbModel;  }
            set { _TgtStbModel = value; }
        }

		/// <summary>
		/// ����ä�� �������࿩�� ����
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
		/// ���ν�ûDBŸ���� ��뿩��
		/// </summary>
		public string TgtPVSYn
		{
			get { return _TgtPVSYn;	}
			set { _TgtPVSYn = value;	}
		}

        /// <summary>
        /// [E_03] ��ȣ�������˾� ��뿩��
        /// </summary>
        public string TgtPrefYn
        {
            get { return _TgtPrefYn; }
            set { _TgtPrefYn = value; }
        }

        /// <summary>
        /// [E_03] ��ȣ�������˾� �������
        /// </summary>
        public int TgtPrefRate
        {
            get { return _TgtPrefRate; }
            set { _TgtPrefRate = value; }
        }

        /// <summary>
        /// [E_03] ��ȣ�������˾� �����ڹ̼���
        /// </summary>
        public string TgtPrefNosend
        {
            get { return _TgtPrefNosend; }
            set { _TgtPrefNosend = value; }
        }

        /// <summary>
        /// [E_04] �������� Ÿ���� ��뿩��
        /// </summary>
        public string TgtProfileYn
        {
            get { return _TgtProfileYn; }
            set { _TgtProfileYn = value; }
        }

        /// <summary>
        /// [E_04] �������� Ÿ���� ���ɴ� ����
        /// </summary>
        public string TgtProfile
        {
            get { return _TgtProfile; }
            set { _TgtProfile = value; }
        }

        /// <summary>
        /// [E_04] �������� Ÿ���� �ŷڵ�
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
		/// 2Slot �������ι� �� ��ġ(0:����,3:2slot ��, 6:2Slot ��)[E_01]
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