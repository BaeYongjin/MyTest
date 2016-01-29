// ===============================================================================
// Agency Data Model for Charites Project
//
// AgencyModel.cs
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
	public class AgencyModel : BaseModel
	{

		// ��ȸ��
		private string _SearchKey       = null;		// �˻���
		private string _SearchAgencyType = null;		// �˻�����
		private string _SearchchkAdState_10    = null;		// �˻� ��뿩�λ���
		private string _SearchRap       = null;		// �˻� �̵�

		// ��������
		private string _AgencyCode		= null;		// ������ڵ�
		private string _AgencyName       = null;	// ������
		private string _RapCode	 		= null;		// ����籸��
		private string _RapName			= null;		// ����籸��
		private string _AgencyType		= null;		// ����籸��
		private string _AgencyTypeName	= null;		// ����籸�и�
		private string _Address			= null;		// �ּ�
		private string _Tell			= null;		// ��ȭ
		private string _Comment			= null;		// ���
		private string _UseYn			= null;		// ��뿩��
		private string _RegDt			= null;		// ����Ͻ�
		private string _ModDt			= null;		// �����Ͻ�
		private string _RegID			= null;		// �����(����������)

		// �����ȸ��
		private DataSet  _DataSet;
	
		public AgencyModel() : base () 
		{
			Init();
		}

		#region Public �޼ҵ�
		public void Init()
		{
			_SearchKey			= "";
			_SearchAgencyType	= "";
			_SearchchkAdState_10	= "";
			_SearchRap	= "";

			_AgencyCode			= "";
			_AgencyName			= "";
			_RapCode			= "";
			_RapName			= "";
			_AgencyType			= "";
			_AgencyTypeName		= "";
			_Address			= "";
			_Tell				= "";
			_Comment			= "";
			_UseYn				= "";
			_RegDt				= "";
			_ModDt				= "";
			_RegID				= "";
				
			_DataSet = new DataSet();
		}

		#endregion

		#region  ������Ƽ 

		public DataSet AgencyDataSet
		{
			get { return _DataSet;	}
			set { _DataSet = value;	}
		}

		public string SearchKey 
		{
			get { return _SearchKey;	}
			set { _SearchKey = value;	}
		}
	
		public string SearchAgencyType
		{
			get { return _SearchAgencyType;	}
			set { _SearchAgencyType = value;	}
		}

		public string SearchchkAdState_10 
		{
			get { return _SearchchkAdState_10;	}
			set { _SearchchkAdState_10 = value;	}
		}

		public string SearchRap 
		{
			get { return _SearchRap;	}
			set { _SearchRap = value;	}
		}

		public string AgencyCode
		{
			get { return _AgencyCode;	}
			set { _AgencyCode = value;	}
		}

		public string AgencyName
		{
			get { return _AgencyName;	}
			set { _AgencyName = value;	}
		}

		public string RapCode
		{
			get { return _RapCode;	}
			set { _RapCode = value;	}
		}

		public string RapName
		{
			get { return _RapName;	}
			set { _RapName = value;	}
		}

		public string AgencyType
		{
			get { return _AgencyType;	}
			set { _AgencyType = value;	}
		}

		public string AgencyTypeName
		{
			get { return _AgencyTypeName;	}
			set { _AgencyTypeName = value;	}
		}

		public string Address
		{
			get { return _Address;	}
			set { _Address = value;	}
		}

		public string Tell
		{
			get { return _Tell;	}
			set { _Tell = value;	}
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

		public string RegDt
		{
			get { return _RegDt;	}
			set { _RegDt = value;	}
		}

		public string ModDt
		{
			get { return _ModDt;	}
			set { _ModDt = value;	}
		}

		public string RegID
		{
			get { return _RegID;	}
			set { _RegID = value;	}
		}

		#endregion

	}
}
