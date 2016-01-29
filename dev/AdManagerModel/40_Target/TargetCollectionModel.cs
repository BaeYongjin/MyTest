// ===============================================================================
// MediaRap Data Model for Charites Project
//
// TargetCollectionModel.cs
//
// ��������� Ŭ������ �����մϴ�. 
//
// ===============================================================================
// Release history
// 2011/01/04   : isStbColl �Ӽ� �߰�
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
	public class TargetCollectionModel : BaseModel
	{
		// ��ȸ��
		private string  _SearchKey			    = null;		// �˻���		
		private string  _SearchchkAdState_10    = null;		// �˻� ��뿩�λ���
		private string  _SearchLevel            = null;		// �˻� ��뿩�λ���
        private bool    _IsStbColl              = false;    // ã�� ����� ��ϵ� ��ž������

		// ��������
		private string _CollectionCode          = null;		// �� ���̵�
		private string _CollectionName          = null;		// �� ��				
		private string _RegDt					= null;		// �������
		private string _ModDt					= null;		// �������
		private string _RegID					= null;		// �������
		private string _Comment					= null;		// ���
		private string _UseYn					= null;		// ��뿩��
		private	string _PvsYn					= null;

		// PVS����ȭ�κ�
		private	int		_SeqNo					= 0;		// PVS�׷��ڵ�

		// ������Ʈ
		private string _UserId					= null;		// ������ �����ID�̳� �����ڹ�ȣ�� �����Ѵ�
		private string _StbId					= null;		
		private string _PostNo					= null;		
		private string _ServiceCode				= null;		
		private string _ResidentNo				= null;		
		private string _Sex						= null;		

		private string _PageSize				= null;		// �������
		private string _Page					= null;		// ���
						
		// �����ȸ��
		private DataSet  _TargetCollectionDataSet;
		private DataSet  _ClientListDataSet;
		private DataSet  _StbListDataSet;


        // ������Ʈ ������
        private string _FromCode = null;
        private string _ToCode = null;
        private string _CopyMove = null;
	
		public TargetCollectionModel() : base () 
		{
			Init();
		}

		#region Public �޼ҵ�
		public void Init()
		{
			_SearchKey		= "";			
			_SearchchkAdState_10 = "";
			_SearchLevel = "";

			_CollectionCode	= "";
			_CollectionName	= "";			
			_RegDt			= "";			
			_ModDt			= "";		
			_RegDt			= "";
			_RegID 			= "";
			_Comment 		= "";
			_UseYn 			= "";
			_PvsYn			= "";

			_UserId			= "";
			_StbId			= "";			
			_PostNo			= "";			
			_ServiceCode	= "";		
			_ResidentNo		= "";
			_Sex 			= "";

			_PageSize		= "";
			_Page 			= "";
            _IsStbColl      = false;
						
			_TargetCollectionDataSet = new DataSet();
			_ClientListDataSet = new DataSet();
			_StbListDataSet = new DataSet();

            _FromCode = "";
            _ToCode = "";
            _CopyMove = "C";
		}

		#endregion

		#region  ������Ƽ 

		public DataSet TargetCollectionDataSet
		{
			get { return _TargetCollectionDataSet;	}
			set { _TargetCollectionDataSet = value;	}
		}

		public DataSet ClientListDataSet
		{
			get { return _ClientListDataSet;	}
			set { _ClientListDataSet = value;	}
		}

		public DataSet StbListDataSet
		{
			get { return _StbListDataSet;	}
			set { _StbListDataSet = value;	}
		}

		public string SearchKey 
		{
			get { return _SearchKey;	}
			set { _SearchKey = value;	}
		}
			
		public string SearchchkAdState_10 
		{
			get { return _SearchchkAdState_10;	}
			set { _SearchchkAdState_10 = value;	}
		}

		public string SearchLevel 
		{
			get { return _SearchLevel;	}
			set { _SearchLevel = value;	}
		}

		public string CollectionCode 
		{
			get { return _CollectionCode;		}
			set { _CollectionCode = value;		}
		}

		public string CollectionName 
		{
			get { return _CollectionName;		}
			set { _CollectionName = value;	}
		}

		public string RegDt 
		{
			get { return _RegDt;		}
			set { _RegDt = value;	}
		}

		public string ModDt 
		{
			get { return _ModDt;		}
			set { _ModDt = value;	}
		}


		public string RegID 
		{
			get { return _RegID;		}
			set { _RegID = value;		}
		}

		public string Comment 
		{
			get { return _Comment;  }
			set { _Comment = value; }
		}

		public string UseYn
		{
			get { return _UseYn;	}
			set { _UseYn = value;	}
		}

		public string PvsYn
		{
			get { return _PvsYn;	}
			set { _PvsYn = value;	}
		}

		/// <summary>
		/// PVS �׷��ڵ�
		/// </summary>
		public int	SeqNo
		{
			get { return _SeqNo;	}
			set { _SeqNo = value;	}
		}

		public string UserId 
		{
			get { return _UserId;		}
			set { _UserId = value;		}
		}

		public string StbId 
		{
			get { return _StbId;		}
			set { _StbId = value;	}
		}

		public string PostNo 
		{
			get { return _PostNo;		}
			set { _PostNo = value;	}
		}

		public string ServiceCode 
		{
			get { return _ServiceCode;		}
			set { _ServiceCode = value;	}
		}


		public string ResidentNo 
		{
			get { return _ResidentNo;		}
			set { _ResidentNo = value;		}
		}

		public string Sex 
		{
			get { return _Sex;  }
			set { _Sex = value; }
		}

		public string PageSize 
		{
			get { return _PageSize;		}
			set { _PageSize = value;		}
		}

		public string Page 
		{
			get { return _Page;  }
			set { _Page = value; }
		}

        public bool IsStbColl
        {
            get { return _IsStbColl;    }
            set { _IsStbColl = value;   }
        }


        // ������Ʈ ������
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
