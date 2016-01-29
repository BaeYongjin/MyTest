// ===============================================================================
// UserInfo Data Model for Charites Project
//
// UserInfoModel.cs
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
	public class MediaInfoModel : BaseModel
	{

		// ��ȸ��
		private string _SearchKey       = null;		// �˻���
		private string _SearchUserLevel = null;		// �˻�����
		private string _SearchchkAdState_10    = null;		// �˻� ��뿩�λ���

		// ��������
		private string _MediaCode          = null;		// ����� ���̵�
		private string _MediaName        = null;		// ����� ��
		private string _Charger    = null;		// ����� ��й�ȣ
		private string _Tell       = null;		// ����� ����
		private string _Email   = null;		// ����� ������
		private string _UseYn   = null;		// ��뿩��
		private string _RegDt        = null;		// ����� �ҼӺμ�
		private string _ModDt       = null;		// ����� ��å����
		

		// �����ȸ��
		private DataSet  _UserDataSet;
	
		public MediaInfoModel() : base () 
		{
			Init();
		}

		#region Public �޼ҵ�
		public void Init()
		{
			_SearchKey		= "";
			_SearchUserLevel = "";
			_SearchchkAdState_10 = "";

			_MediaCode			= "";
			_MediaName		= "";
			_Charger	= "";
			_Tell		= "";
			_Email	= "";
			_UseYn	= "";
			_RegDt   	= "";
			_ModDt		= "";
			            
			_UserDataSet = new DataSet();
		}

		#endregion

		#region  ������Ƽ 

		public DataSet UserDataSet
		{
			get { return _UserDataSet;	}
			set { _UserDataSet = value;	}
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
	
		public string SearchUserLevel 
		{
			get { return _SearchUserLevel;	}
			set { _SearchUserLevel = value;	}
		}

		public string MediaCode 
		{
			get { return _MediaCode;		}
			set { _MediaCode = value;		}
		}

		public string MediaName 
		{
			get { return _MediaName;		}
			set { _MediaName = value;	}
		}

		public string Charger 
		{
			get { return _Charger; }
			set { _Charger = value;}
		}

		public string Tell 
		{
			get { return _Tell;	}
			set { _Tell = value;	}
		}

		public string Email
		{
			get { return _Email;	}
			set { _Email = value;	}
		}

		public string UseYn
		{
			get { return _UseYn;	}
			set { _UseYn = value;	}
		}

		public string RegDt
		{
			get { return _RegDt;		}
			set { _RegDt = value;	}
		}

		public string ModDt 
		{
			get { return _ModDt;	}
			set { _ModDt = value;	}
		}

		

		#endregion

	}
}
