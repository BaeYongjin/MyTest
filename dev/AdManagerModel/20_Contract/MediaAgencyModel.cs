// ===============================================================================
// MediaAgency Data Model for Charites Project
//
// MediaAgencyModel.cs
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
	public class MediaAgencyModel : BaseModel
	{

		// ��ȸ��
		private string _SearchKey       = null;		// �˻���
		private string _SearchMediaName = null;		// �˻�����
		private string _SearchRapName = null;		// �˻�����
		private string _SearchMediaAgency = null;		// �˻�����
		private string _SearchchkAdState_10    = null;		// �˻� ��뿩�λ���

		// ��������
		private string _MediaCode          = null;		// �̵���ڵ�
		private string _RapCode          = null;		// �� �ڵ�
		private string _AgencyCode        = null;		// ������ڵ�		
		private string _ContStartDay        = null;		// ������		
		private string _ContEndDay        = null;		// ������
		private string _Charger        = null;		// �����
		private string _Tell        = null;		// ��ȭ��ȣ		
		private string _Email        = null;		// �̸���
		private string _UseYn        = null;		// ��뿩��
		private string _RegDt			= null;		// �������
		private string _ModDt			= null;		// ��������
		private string _RegID			= null;		// �����
		
		
		// �����ȸ��
		private DataSet  _MediaAgencyDataSet;
	
		public MediaAgencyModel() : base () 
		{
			Init();
		}

		#region Public �޼ҵ�
		public void Init()
		{
			_SearchKey		= "";
			_SearchMediaName = "";
			_SearchRapName = "";
			_SearchMediaAgency = "";
			_SearchchkAdState_10 = "";

			_MediaCode			= "";
			_RapCode		= "";			
			_AgencyCode		= "";			
			_ContStartDay		= "";		
			_ContEndDay			= "";
			_Charger			= "";
			_Tell 	= "";
			_Email		= "";			
			_UseYn		= "";		
			_RegDt			= "";
			_ModDt 	= "";
			_RegID 	= "";


			
			_MediaAgencyDataSet = new DataSet();
		}

		#endregion

		#region  ������Ƽ 

		public DataSet MediaAgencyDataSet
		{
			get { return _MediaAgencyDataSet;	}
			set { _MediaAgencyDataSet = value;	}
		}

		public string SearchKey 
		{
			get { return _SearchKey;	}
			set { _SearchKey = value;	}
		}

		public string SearchMediaName
		{
			get { return _SearchMediaName;	}
			set { _SearchMediaName = value;	}
		}

		public string SearchRapName
		{
			get { return _SearchRapName;	}
			set { _SearchRapName = value;	}
		}
	
		public string SearchMediaAgency
		{
			get { return _SearchMediaAgency;	}
			set { _SearchMediaAgency = value;	}
		}

		public string SearchchkAdState_10 
		{
			get { return _SearchchkAdState_10;	}
			set { _SearchchkAdState_10 = value;	}
		}

		public string MediaCode 
		{
			get { return _MediaCode;		}
			set { _MediaCode = value;		}
		}

		public string RapCode 
		{
			get { return _RapCode;		}
			set { _RapCode = value;	}
		}

		public string AgencyCode 
		{
			get { return _AgencyCode;		}
			set { _AgencyCode = value;	}
		}

		public string ContStartDay 
		{
			get { return _ContStartDay;		}
			set { _ContStartDay = value;	}
		}


		public string ContEndDay 
		{
			get { return _ContEndDay;		}
			set { _ContEndDay = value;		}
		}

		public string Charger 
		{
			get { return _Charger;		}
			set { _Charger = value;		}
		}

		public string Tell 
		{
			get { return _Tell;  }
			set { _Tell = value; }
		}

		public string Email 
		{
			get { return _Email;  }
			set { _Email = value; }
		}

		public string UseYn 
		{
			get { return _UseYn;  }
			set { _UseYn = value; }
		}

		public string RegDt 
		{
			get { return _RegDt;  }
			set { _RegDt = value; }
		}

		public string ModDt 
		{
			get { return _ModDt;  }
			set { _ModDt = value; }
		}

		public string RegID 
		{
			get { return _RegID;  }
			set { _RegID = value; }
		}
		
		#endregion

	}
}
