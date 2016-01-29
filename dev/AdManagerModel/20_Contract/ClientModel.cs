// ===============================================================================
// Client Data Model for Charites Project
//
// ClientModel.cs
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
	public class ClientModel : BaseModel
	{

		// ��ȸ��
		private string _SearchKey       = null;		// �˻���
		private string _SearchMediaName = null;		// �˻�����
		private string _SearchRapName = null;		// �˻�����
		private string _SearchMediaAgency = null;		// �˻�����
		private string _SearchAdvertiserName = null;		// �˻�����
		private string _SearchchkAdState_10    = null;		// �˻� ��뿩�λ���
		private string _SearchRap       = null;		// �˻� �̵�

		// ��������
		private string _MediaCode         = null;		// �̵���ڵ�
		private string _MediaCode_C       = null;		// �̵���ڵ�
		private string _RapCode           = null;		// �� �ڵ�
		private string _RapCode_C         = null;		// �� �ڵ�
		private string _AgencyCode        = null;		// ������ڵ�		
		private string _AgencyCode_C      = null;		// ������ڵ�		
		private string _AdvertiserCode    = null;		// �������ڵ�				
		private string _AdvertiserCode_C  = null;		// �������ڵ�				
		private string _Comment           = null;		// �̸���
		private string _UseYn             = null;		// ��뿩��
		private string _RegDt			  = null;		// �������
		private string _ModDt			  = null;		// ��������
		private string _RegID			  = null;		// �����
		private string _MediaName         = null;
		private string _RapName           = null;
		private string _AgencyName        = null;

		
		
		// �����ȸ��
		private DataSet  _ClientDataSet;
	
		public ClientModel() : base () 
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
			_SearchAdvertiserName = "";
			_SearchchkAdState_10 = "";
			_SearchRap = "";

			_MediaCode			= "";			
			_MediaCode_C			= "";			
			_RapCode		= "";			
			_RapCode_C		= "";			
			_AgencyCode		= "";		
			_AgencyCode_C		= "";		
			_AdvertiserCode		= "";		
			_AdvertiserCode_C		= "";		
			_Comment		= "";			
			_UseYn		= "";		
			_RegDt			= "";
			_ModDt 	= "";
			_RegID 	= "";

			_MediaName  = "";
			_RapName    = "";
			_AgencyName = "";
			
			_ClientDataSet = new DataSet();
		}

		#endregion

		#region  ������Ƽ 

		public DataSet ClientDataSet
		{
			get { return _ClientDataSet;	}
			set { _ClientDataSet = value;	}
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

		public string SearchAdvertiserName
		{
			get { return _SearchAdvertiserName;	}
			set { _SearchAdvertiserName = value;	}
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

		public string MediaCode 
		{
			get { return _MediaCode;		}
			set { _MediaCode = value;		}
		}

		public string MediaCode_C
		{
			get { return _MediaCode_C;		}
			set { _MediaCode_C = value;		}
		}
		
		public string RapCode 
		{
			get { return _RapCode;		}
			set { _RapCode = value;	}
		}

		public string RapCode_C 
		{
			get { return _RapCode_C;		}
			set { _RapCode_C = value;	}
		}

		public string AgencyCode 
		{
			get { return _AgencyCode;		}
			set { _AgencyCode = value;	}
		}

		public string AgencyCode_C 
		{
			get { return _AgencyCode_C;		}
			set { _AgencyCode_C = value;	}
		}
	
		public string AdvertiserCode 
		{
			get { return _AdvertiserCode;		}
			set { _AdvertiserCode = value;	}
		}

		public string AdvertiserCode_C 
		{
			get { return _AdvertiserCode_C;		}
			set { _AdvertiserCode_C = value;	}
		}
		
		public string Comment 
		{
			get { return _Comment;  }
			set { _Comment = value; }
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

		public string MediaName 
		{
			get { return _MediaName;  }
			set { _MediaName = value; }
		}
		
		public string RapName 
		{
			get { return _RapName;  }
			set { _RapName = value; }
		}

		public string AgencyName 
		{
			get { return _AgencyName;  }
			set { _AgencyName = value; }
		}

		#endregion

	}
}
