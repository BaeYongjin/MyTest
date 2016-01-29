// ===============================================================================
// Contract Data Model for Charites Project
//
// AdPopModel.cs
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
	public class AdPopModel : BaseModel
	{		
		// ��������
		private string _ItemNo          = null;		// �����ȣ		
		private string _MediaCode       = null;		// ��ü�ڵ�
		private string _JumpType		= null;		// ��������
		private string _GenreCode1      = null;		// �帣�ڵ�
		private string _GenreCode2      = null;		// �帣�ڵ�
		private string _GenreCode3      = null;		// �帣�ڵ�
		private string _GenreCode4      = null;		// �帣�ڵ�
		private string _GenreCode5      = null;		// �帣�ڵ�
		private string _Flag	        = null;		// �帣��
		private string _ChannelNo       = null;		// ä�ι�ȣ
		private string _Title           = null;		// ���α׷���
		private string _ContentID      = null;		// ������ID
		private string _ContentID1      = null;		// ������ID
		private string _ContentID2      = null;		// ������ID
		private string _ContentID3      = null;		// ������ID
		private string _ContentID4      = null;		// ������ID
		private string _ContentID5      = null;		// ������ID
		private string _AdPopID         = null;		// ����ID
		private string _PopupTitle      = null;		// ��������
		private string _HomeYn          = null;		// Ȩ ���⿩��
		private string _ChannelYn       = null;		// ä�� ���⿩��		


		// �����ȸ��
		private DataSet  _DataSet;
		private DataSet  _DataSet2;	// ����˻���
		private DataSet  _DataSet3;	// ä�ΰ˻���
		private DataSet  _DataSet4;	// �������˻���
		
		private DataSet  _DataSet5; // �����ý��� �˾���������Ʈ��ȸ��


		public AdPopModel() : base () 
		{
			Init();
		}

		#region Public �޼ҵ�
		public void Init()
		{			
			_ItemNo      	= "";		
			_MediaCode   	= "";		
			_JumpType		= "";		
			_GenreCode1  	= "";		
			_GenreCode2  	= "";		
			_GenreCode3  	= "";		
			_GenreCode4  	= "";		
			_GenreCode5  	= "";		
			_Flag	    	= "";      
			_ChannelNo       = "";
			_Title           = "";
			_ContentID  	= "";      
			_ContentID1  	= "";      
			_ContentID2  	= "";
			_ContentID3		= "";
			_ContentID4		= "";
			_ContentID5		= "";
			_AdPopID		= "";
			_PopupTitle		= "";
			_HomeYn			= "";
			_ChannelYn		= "";

			
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
		
		public string ItemNo           
		{
			get { return _ItemNo          ; }
			set { _ItemNo           = value;}
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
		public string GenreCode1 
		{
			get { return _GenreCode1; }
			set { _GenreCode1 = value;}
		}

		public string GenreCode2 
		{
			get { return _GenreCode2; }
			set { _GenreCode2 = value;}
		}

		public string GenreCode3 
		{
			get { return _GenreCode3; }
			set { _GenreCode3 = value;}
		}

		public string GenreCode4 
		{
			get { return _GenreCode4; }
			set { _GenreCode4 = value;}
		}
		
		public string GenreCode5 
		{
			get { return _GenreCode5; }
			set { _GenreCode5 = value;}
		}

		public string Flag 
		{
			get { return _Flag; }
			set { _Flag = value;}
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

		public string ContentID1 
		{
			get { return _ContentID1; }
			set { _ContentID1 = value;}
		}

		public string ContentID2 
		{
			get { return _ContentID2; }
			set { _ContentID2 = value;}
		}

		public string ContentID3 
		{
			get { return _ContentID3; }
			set { _ContentID3 = value;}
		}

		public string ContentID4 
		{
			get { return _ContentID4; }
			set { _ContentID4 = value;}
		}

		public string ContentID5 
		{
			get { return _ContentID5; }
			set { _ContentID5 = value;}
		}

		public string AdPopID 
		{
			get { return _AdPopID; }
			set { _AdPopID = value;}
		}
		public string PopupTitle
		{
			get { return _PopupTitle; }
			set { _PopupTitle = value;}
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
		 
		#endregion

	}
}
