// ===============================================================================
// Contract Data Model for Charites Project
//
// ContractModel.cs
//
// ���������� Ŭ������ �����մϴ�. 
//
// ===============================================================================
// Release history
// 2007.06.06 �۸�ȯ v1.0
// 2007.10.03 RH.Jung �����º� üũ ��ȸ���� �߰�
// ===============================================================================
// Copyright (C) 2007 G-Inno Systems Inc.
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
    public class ContractModel : BaseModel
    {

        // ��ȸ��
        private string _SearchKey       = null;		// �˻���
		private string _SearchState_10  = null;     // �����º� üũ ��ȸ���� 10:��� 2007.10.03 RH.Jung
		private string _SearchState_20  = null;     // �����º� üũ ��ȸ���� 20:����
		private string _SearchRap       = null;		// �˻� �̵�
		private string _SearchUse       = null;		// �˻� ��뿩��


        // ��������
        private string _MediaCode	    = null;		//��üID
        private string _RapCode	        = null;		//�̵�ID
        private string _AgencyCode		= null;		//�����
        private string _AdvertiserCode	= null;		//������ 
        private string _MediaName	= null;		//��üID��
        private string _RapName	    = null;		//�̵�ID��
        private string _AgencyName	= null;		//������
        private string _AdvertiserName	= null;		//�����ָ� 
        private string _ContractSeq		= null;		//������
        private string _ContractName	= null;		//����
        private string _State	        = null;		//������
        private string _ContStartDay	= null;		//��������
        private string _ContEndDay      = null;     //���������
		private string _PackageNo	    = null;		// ��Ű����ȣ
		private string _PackageName     = null;		// ��Ű����
		private string _ContractAmt      = null;     //��๰��
		private string _BonusRate       = null;		// ���ʽ���
		private string _LongBonus       = null;		// ��⺸�ʽ���
		private string _SpecialBonus       = null;		// Ư�����ʽ���
		private string _TotalBonus       = null;		// �Ѻ��ʽ���
		private string _SecurityTgt       = null;		// �������
		private string _packageName       = null;		// ��ǰ��		
		private string _Price           = null;		// �ܰ�
		private string _AdTime          = null;		// �����ʼ�
        private string _Comment         = null;     //�޸�
		private string _JobCode         = null;     //�����ڵ�
		private string _JobName         = null;     //������
		private string _JobName2         = null;     //������
		private string _JobName3         = null;     //������
		private string _Level1Code      = null;     //��з��ڵ�
		private string _Level2Code      = null;     //�ߺз��ڵ�
		private string _Level	        = null;     //����
		private string _JobClass        = null;     //����
		private string _UseYn           = null;		// ��뿩��

        // �����ȸ��
        private DataSet  _ContractDataSet;

        public ContractModel() : base () 
        {
            Init();
        }

        #region Public �޼ҵ�
        public void Init()
        {
            _SearchKey		= "";
			_SearchState_10 = "";
			_SearchState_20 = "";
			_SearchRap      = "";
			_SearchUse      = "";

            _MediaCode	    = "";		//��üID
            _RapCode	    = "";		//�̵�ID
            _AgencyCode		= "";		//�����
            _AdvertiserCode	= "";		//������
            _MediaName	= "";       //��üID��
            _RapName	= "";       //�̵�ID��
            _AgencyName	= "";       //������
            _AdvertiserName = "";   //�����ָ�
 
            _ContractSeq	= "";		//������
            _ContractName	= "";		//����
            _State	        = "";		//������
            _ContStartDay	= "";		//��������
            _ContEndDay     = "";       //���������
			_PackageNo      = "";
			_PackageName    = "";
			_ContractAmt     = "";       //��๰��
			_BonusRate      = "";
			_LongBonus      = "";
			_SpecialBonus      = "";
			_TotalBonus      = "";
			_SecurityTgt      = "";
			_packageName      = "";
			_Price          = "";
			_AdTime         = "";
            _Comment        = "";       //�޸�
			_JobCode        = "";       //�����ڵ�
			_JobName        = "";       //������
			_JobName2        = "";       //������
			_JobName3        = "";       //������
			_Level1Code     = "";       //��з��ڵ�
			_Level2Code     = "";       //�ߺз��ڵ�
			_Level	        = "";       //����
			_JobClass	        = "";       //����
			_UseYn          = "";

	

            _ContractDataSet = new DataSet();
        }

        #endregion

        #region  ������Ƽ 

        public DataSet ContractDataSet
        {
            get { return _ContractDataSet;	}
            set { _ContractDataSet = value;	}
        }

        public string SearchKey 
        {
            get { return _SearchKey;	}
            set { _SearchKey = value;	}
        }

		// 2007.10.03
		//
		public string SearchState_10 
		{
			get { return _SearchState_10;	}
			set { _SearchState_10 = value;	}
		}
	
		public string SearchState_20 
		{
			get { return _SearchState_20;	}
			set { _SearchState_20 = value;	}
		}

		public string SearchRap 
		{
			get { return _SearchRap;	}
			set { _SearchRap = value;	}
		}

		public string SearchUse  
		{
			get { return _SearchUse;	}
			set { _SearchUse = value;	}
		}
		//
		// 2007.10.03 
	
        public string MediaCode
        {
            get { return _MediaCode; }
            set { _MediaCode = value;}
        }
        public string RapCode
        {
            get { return _RapCode; }
            set { _RapCode = value;}
        }

        public string AgencyCode 
        {
            get { return _AgencyCode; }
            set { _AgencyCode = value;}
        }

        public string AdvertiserCode
        {
            get { return _AdvertiserCode;}
            set { _AdvertiserCode= value;}
        }
        public string MediaName
        {
            get { return _MediaName; }
            set { _MediaName = value;}
        }
        public string RapName
        {
            get { return _RapName; }
            set { _RapName = value;}
        }

        public string AgencyName 
        {
            get { return _AgencyName; }
            set { _AgencyName = value;}
        }

        public string AdvertiserName
        {
            get { return _AdvertiserName;}
            set { _AdvertiserName= value;}
        }

        public string ContractSeq 
        {
            get { return _ContractSeq; }
            set { _ContractSeq = value;}
        }
        public string ContractName 
        {
            get { return _ContractName; }
            set { _ContractName = value;}
        }
        public string State 
        {
            get { return _State;	}
            set { _State = value;}
        }
        public string ContStartDay 
        {
            get { return _ContStartDay; }
            set { _ContStartDay = value;}
        }
        public string ContEndDay     
        {
            get { return _ContEndDay;  }
            set { _ContEndDay = value;}
        }
		public string PackageNo 
		{
			get { return _PackageNo;	}
			set { _PackageNo = value;	}
		}

		public string PackageName 
		{
			get { return _PackageName;	}
			set { _PackageName = value;	}
		}

		public string ContractAmt     
		{
			get { return _ContractAmt;  }
			set { _ContractAmt = value;}
		}
        public string Comment        
        {
            get { return _Comment;}
            set { _Comment = value;	}
        }

		public string JobCode        
		{
			get { return _JobCode;}
			set { _JobCode = value;	}
		}

		public string JobName        
		{
			get { return _JobName;}
			set { _JobName = value;	}
		}

		public string JobName2        
		{
			get { return _JobName2;}
			set { _JobName2 = value;	}
		}

		public string JobName3        
		{
			get { return _JobName3;}
			set { _JobName3 = value;	}
		}

		public string Level1Code        
		{
			get { return _Level1Code;}
			set { _Level1Code = value;	}
		}
		
		public string Level2Code        
		{
			get { return _Level2Code;}
			set { _Level2Code = value;	}
		}

		public string Level        
		{
			get { return _Level;}
			set { _Level = value;	}
		}

		public string JobClass        
		{
			get { return _JobClass;}
			set { _JobClass = value;	}
		}

		public string UseYn 
		{
			get { return _UseYn;	}
			set { _UseYn = value;	}
		}

		public string BonusRate 
		{
			get { return _BonusRate;	}
			set { _BonusRate = value;	}
		}

		public string LongBonus 
		{
			get { return _LongBonus;	}
			set { _LongBonus = value;	}
		}

		public string SpecialBonus 
		{
			get { return _SpecialBonus;	}
			set { _SpecialBonus = value;	}
		}

		public string TotalBonus 
		{
			get { return _TotalBonus;	}
			set { _TotalBonus = value;	}
		}

		public string SecurityTgt 
		{
			get { return _SecurityTgt;	}
			set { _SecurityTgt = value;	}
		}

		public string packageName 
		{
			get { return _packageName;	}
			set { _packageName = value;	}
		}

		public string Price 
		{
			get { return _Price;	}
			set { _Price = value;	}
		}

		public string AdTime 
		{
			get { return _AdTime;	}
			set { _AdTime = value;	}
		}
        
        #endregion

    }
}
