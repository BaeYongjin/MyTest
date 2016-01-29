// ===============================================================================
// Contract Data Model for Charites Project
//
// ChannelJumpModel.cs
//
// ä������ Ŭ������ �����մϴ�. 
//
// ===============================================================================
// Release history
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
    public class ChannelJumpModel : BaseModel
    {
        #region ��������
        // ��ȸ��
        private string _SearchKey				= null;		// �˻���
		private string _SearchMediaCode			= null;	
		private string _SearchRapCode			= null;		
		private string _SearchchkAdState_10		= null;		// �˻� ������� : �غ�
		private string _SearchchkAdState_20		= null;		// �˻� ������� : ��
		private string _SearchchkAdState_30		= null;		// �˻� ������� : ����
		private string _SearchchkAdState_40		= null;		// �˻� ������� : ����
		private string _SearchAdType			= null;		// �˻� ��������
		private string _SearchJumpType			= null;		// �˻� ���α���

        // ��������
        private string _ItemNo          = null;		// �����ȣ
        private string _ItemName        = null;		// �����
		private string _MediaCode       = null;		// ��ü�ڵ�
		private string _JumpType		= null;		// ��������
        private string _GenreCode       = null;		// �帣�ڵ�
        private string _GenreName       = null;		// �帣��
        private string _ChannelNo       = null;		// ä�ι�ȣ
        private string _Title           = null;		// ���α׷���
        private string _ContentID       = null;		// ������ID
		private string _PopupID         = null;		// ����ID
		private string _PopupTitle      = null;		// ��������
        private string  _ChannelManager = null;		// ä�θŴ���
        private string _HomeYn          = null;		// Ȩ ���⿩��
        private string _ChannelYn       = null;		// ä�� ���⿩��
		private string _Type			= null;		// ä�� ���⿩��

		// 2012/06/01 �߰�
		private string _StbTypeYn = null;			// ��ž�� Ÿ���� ����
		private string _StbTypeStr = null;			// ��ž�� Ÿ���� ��

        // �����ȸ��
		private DataSet  _DataSet;
		private DataSet  _DataSet2;	// ����˻���
		private DataSet  _DataSet3;	// ä�ΰ˻���
		private DataSet  _DataSet4;	// �������˻���
		
		private DataSet  _DataSet5; // �����ý��� �˾���������Ʈ��ȸ��
        #endregion

        public ChannelJumpModel() : base () {   Init(); }

        #region Public �޼ҵ�
        public void Init()
        {
			_SearchKey				= "";
			_SearchMediaCode		= "";
			_SearchRapCode			= "";
			_SearchchkAdState_10	= "";
			_SearchchkAdState_20	= "";
			_SearchchkAdState_30	= "";
			_SearchchkAdState_40	= "";
			_SearchAdType			= "";
			_SearchJumpType			= "";

			_ItemNo			= "";		
			_ItemName		= "";		
			_MediaCode		= "";		
			_JumpType		= "";		
			_GenreCode		= "";		
			_GenreName		= "";		
			_ChannelNo		= "";		
			_Title			= "";		
			_ContentID		= "";      
			_PopupID        = "";
			_PopupTitle     = "";
            _ChannelManager = "";
			_HomeYn			= "";      
			_ChannelYn		= "";
			_Type		= "";

			_DataSet = new DataSet();
			_DataSet2 = new DataSet();
			_DataSet3 = new DataSet();
			_DataSet4 = new DataSet();
			_DataSet5 = new DataSet();
		}

        #endregion

        #region  ������Ƽ 

        public DataSet ChannelJumpDataSet
        {
            get { return _DataSet;	}
            set { _DataSet = value;	}
        }

		public DataSet ContractItemDataSet
		{
			get { return _DataSet2;	}
			set { _DataSet2 = value;	}
		}

		public DataSet ChannelListDataSet
		{
			get { return _DataSet3;	}
			set { _DataSet3 = value;	}
		}

		public DataSet ContentListDataSet
		{
			get { return _DataSet4;	}
			set { _DataSet4 = value;	}
		}

		public DataSet AdPopListDataSet
		{
			get { return _DataSet5;	}
			set { _DataSet5 = value;	}
		}

        public string SearchKey 
        {
            get { return _SearchKey;	}
            set { _SearchKey = value;	}
        }
	
        public string SearchMediaCode
        {
            get { return _SearchMediaCode; }
            set { _SearchMediaCode = value;}
        }

        public string SearchRapCode
        {
            get { return _SearchRapCode; }
            set { _SearchRapCode = value;}
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

		public string SearchAdType 
		{
			get { return _SearchAdType;		}
			set { _SearchAdType = value;		}
		}

		public string SearchJumpType 
		{
			get { return _SearchJumpType;		}
			set { _SearchJumpType = value;		}
		}

		public string ItemNo           
        {
            get { return _ItemNo          ; }
            set { _ItemNo           = value;}
        }

        public string ItemName 
        {
            get { return _ItemName; }
            set { _ItemName = value;}
        }

        public string MediaCode 
        {
            get { return _MediaCode; }
            set { _MediaCode = value;}
        }

        public string JumpType 
        {
            get { return _JumpType; }
            set { _JumpType = value;}
        }

        public string GenreCode 
        {
            get { return _GenreCode; }
            set { _GenreCode = value;}
        }

        public string GenreName 
        {
            get { return _GenreName; }
            set { _GenreName = value;}
        }

        public string ChannelNo 
        {
            get { return _ChannelNo; }
            set { _ChannelNo = value;}
        }

        public string Title 
        {
            get { return _Title; }
            set { _Title = value;}
        }

		public string ContentID 
		{
			get { return _ContentID; }
			set { _ContentID = value;}
		}

		public string PopupID 
		{
			get { return _PopupID; }
			set { _PopupID = value;}
		}

		public string PopupTitle
		{
			get { return _PopupTitle; }
			set { _PopupTitle = value;}
		}

        public string ChannelManager
        {
            get { return _ChannelManager; }
            set { _ChannelManager = value;}
        }

		public string HomeYn 
        {
            get { return _HomeYn; }
            set { _HomeYn = value;}
        }

        public string ChannelYn 
        {
            get { return _ChannelYn; }
            set { _ChannelYn = value;}
        }

		public string Type 
		{
			get { return _Type; }
			set { _Type = value;}
		}

		public string StbTypeYn
		{
			get { return _StbTypeYn; }
			set { _StbTypeYn = value; }
		}

		public string StbTypeString
		{
			get { return _StbTypeStr; }
			set { _StbTypeStr = value; }
		}
 
        #endregion
    }
}