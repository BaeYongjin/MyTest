using System;
using System.Data;

using WinFramework.Base;

namespace AdManagerModel
{
	/// <summary>
	/// CampaignCodeModel에 대한 요약 설명입니다.
	/// </summary>
	public class CampaignCodeModel : BaseModel
	{		
		private string _MediaCode       = null;		// 검색 미디어렙
		
		// 결과용
		private string _ContractSeq = null;				// 코드
		private string _CampaignCode = null;				// 코드
		private string _CampaignName  = null;			// 코드명


		// 목록조회용
		private DataSet  _CampaignCodeDataSet;
	
		public CampaignCodeModel() : base()
		{
			Init();
		}

		#region Public 메소드
		public void Init()
		{			
			_MediaCode			= "";
			_ContractSeq			= "";
			_CampaignCode		= "";
			_CampaignName	= "";

			_CampaignCodeDataSet = null;
		}

		#endregion

		#region  프로퍼티 	
		

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
