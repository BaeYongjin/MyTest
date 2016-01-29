using System;
using System.Data;

using WinFramework.Base;

namespace AdManagerModel
{
	/// <summary>
	/// CampaignCodeModel�� ���� ��� �����Դϴ�.
	/// </summary>
	public class CampaignCodeModel : BaseModel
	{		
		private string _MediaCode       = null;		// �˻� �̵�
		
		// �����
		private string _ContractSeq = null;				// �ڵ�
		private string _CampaignCode = null;				// �ڵ�
		private string _CampaignName  = null;			// �ڵ��


		// �����ȸ��
		private DataSet  _CampaignCodeDataSet;
	
		public CampaignCodeModel() : base()
		{
			Init();
		}

		#region Public �޼ҵ�
		public void Init()
		{			
			_MediaCode			= "";
			_ContractSeq			= "";
			_CampaignCode		= "";
			_CampaignName	= "";

			_CampaignCodeDataSet = null;
		}

		#endregion

		#region  ������Ƽ 	
		

		public string MediaCode 
		{
			get { return _MediaCode;	}
			set { _MediaCode = value;	}
		}

		public string ContractSeq 
		{
			get { return _ContractSeq;	}
			set { _ContractSeq = value;	}
		}

		public string CampaignCode 
		{
			get { return _CampaignCode;  }
			set { _CampaignCode = value; }
		}

		public string CampaignName 
		{
			get { return _CampaignName;  }
			set { _CampaignName = value; }
		}

		public DataSet CampaignCodeDataSet
		{
			get { return _CampaignCodeDataSet;	}
			set { _CampaignCodeDataSet = value;	}
		}


		#endregion

	}
}
