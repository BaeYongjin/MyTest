// ===============================================================================
// MediaRap Data Model for Charites Project
//
// MediaRapModel.cs
//
// ��������� Ŭ������ �����մϴ�. 
//
// ===============================================================================
// Release history
//
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
	public class MediaRapModel : BaseModel
	{

		// ��ȸ��
		private string _SearchKey       = null;		// �˻���
		private string _SearchMediaRap = null;		// �˻�����
		private string _SearchchkAdState_10    = null;		// �˻� ��뿩�λ���

		// ��������
		private string _RapCode          = null;		// �� ���̵�
		private string _RapName        = null;		// �� ��		
		private string _RapType        = null;		// �� Ÿ��		
		private string _Tell        = null;		// ��ȭ��ȣ		
		private string _RegDt			= null;		// �������
		private string _Comment     = null;		// ���
		private string _UseYn     = null;		// ��뿩��
		
		// �����ȸ��
		private DataSet  _MediaRapDataSet;
	
		public MediaRapModel() : base () 
		{
			Init();
		}

		#region Public �޼ҵ�
		public void Init()
		{
			_SearchKey		= "";
			_SearchMediaRap = "";
			_SearchchkAdState_10 = "";

			_RapCode			= "";
			_RapName		= "";			
			_RapType		= "";			
			_Tell		= "";		
			_RegDt			= "";
			_Comment 	= "";
			_UseYn 	= "";
			
			_MediaRapDataSet = new DataSet();
		}

		#endregion

		#region  ������Ƽ 

		public DataSet MediaRapDataSet
		{
			get { return _MediaRapDataSet;	}
			set { _MediaRapDataSet = value;	}
		}

		public string SearchKey 
		{
			get { return _SearchKey;	}
			set { _SearchKey = value;	}
		}
	
		public string SearchMediaRap
		{
			get { return _SearchMediaRap;	}
			set { _SearchMediaRap = value;	}
		}

		public string SearchchkAdState_10 
		{
			get { return _SearchchkAdState_10;	}
			set { _SearchchkAdState_10 = value;	}
		}

		public string RapCode 
		{
			get { return _RapCode;		}
			set { _RapCode = value;		}
		}

		public string RapName 
		{
			get { return _RapName;		}
			set { _RapName = value;	}
		}

		public string RapType 
		{
			get { return _RapType;		}
			set { _RapType = value;	}
		}

		public string Tell 
		{
			get { return _Tell;		}
			set { _Tell = value;	}
		}


		public string RegDt 
		{
			get { return _RegDt;		}
			set { _RegDt = value;		}
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


		#endregion

	}
}
