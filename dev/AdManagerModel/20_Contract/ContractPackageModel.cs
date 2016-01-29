// ===============================================================================
// ContractPackage Data Model for Charites Project
//
// ContractPackageModel.cs
//
// �����ǰ ��Ű�� Ŭ������ �����մϴ�. 
//
// ===============================================================================
// Release history
// 2007.10.22 ������ ó��
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
    public class ContractPackageModel : BaseModel
    {

        // ��ȸ��
        private string _SearchKey       = null;		// �˻���
		private string _SearchRap       = null;		// �˻� �̵�
		private string _SearchUse       = null;		// �˻� ��뿩��
    
        // ��������
        private string _PackageNo	    = null;		// ��Ű����ȣ
		private string _PackageName     = null;		// ��Ű����
		private string _RapCode         = null;		// �̵��ڵ�
		private string _AdTime          = null;		// �����ʼ�
		private string _ContractAmt     = null;     // ������ⷮ
		private string _BonusRate       = null;		// ���ʽ���
		private string _Price           = null;		// �ܰ�
		private string _Comment         = null;		// ���
		private string _UseYn           = null;		// ��뿩��


        // �����ȸ��
        private DataSet  _DataSet;

        public ContractPackageModel() : base () 
        {
            Init();
        }

        #region Public �޼ҵ�
        public void Init()
        {
            _SearchKey		= "";
			_SearchRap      = "";
			_SearchUse      = "";

			_PackageNo      = "";
			_PackageName    = "";
			_AdTime         = "";
			_RapCode        = "";
			_ContractAmt    = "";
			_BonusRate      = "";
			_Price          = "";
			_Comment        = "";
			_UseYn          = "";

            _DataSet = new DataSet();
        }

        #endregion

        #region  ������Ƽ 

        public DataSet PackageDataSet
        {
            get { return _DataSet;	}
            set { _DataSet = value;	}
        }

        public string SearchKey 
        {
            get { return _SearchKey;	}
            set { _SearchKey = value;	}
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

		public string RapCode 
		{
			get { return _RapCode;	}
			set { _RapCode = value;	}
		}

		public string AdTime 
		{
			get { return _AdTime;	}
			set { _AdTime = value;	}
		}

		public string ContractAmt 
		{
			get { return _ContractAmt;	}
			set { _ContractAmt = value;	}
		}

		public string BonusRate 
		{
			get { return _BonusRate;	}
			set { _BonusRate = value;	}
		}

		public string Price 
		{
			get { return _Price;	}
			set { _Price = value;	}
		}

		public string Comment 
		{
			get { return _Comment;	}
			set { _Comment = value;	}
		}

		public string UseYn 
		{
			get { return _UseYn;	}
			set { _UseYn = value;	}
		}

		#endregion

    }
}
