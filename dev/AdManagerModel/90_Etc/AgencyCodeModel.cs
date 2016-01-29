using System;
using System.Data;

using WinFramework.Base;

namespace AdManagerModel
{
	/// <summary>
	/// AgencyCodeModel�� ���� ��� �����Դϴ�.
	/// </summary>
	public class AgencyCodeModel : BaseModel
	{

		private string _SearchKey       = null;		// �˻���
		private string _SearchRap       = null;		// �˻� �̵�
		
		// �����
		private string _AgencyCode = null;				// �ڵ�
		private string _AgencyName  = null;			// �ڵ��


		// �����ȸ��
		private DataSet  _AgencyCodeDataSet;
	
		public AgencyCodeModel() : base()
		{
			Init();
		}

		#region Public �޼ҵ�
		public void Init()
		{			
			_SearchKey			= "";
			_SearchRap			= "";
			_AgencyCode		= "";
			_AgencyName	= "";

			_AgencyCodeDataSet = null;
		}

		#endregion

		#region  ������Ƽ 	

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

		public string AgencyCode 
		{
			get { return _AgencyCode;  }
			set { _AgencyCode = value; }
		}

		public string AgencyName 
		{
			get { return _AgencyName;  }
			set { _AgencyName = value; }
		}

		public DataSet AgencyCodeDataSet
		{
			get { return _AgencyCodeDataSet;	}
			set { _AgencyCodeDataSet = value;	}
		}


		#endregion

	}
}
