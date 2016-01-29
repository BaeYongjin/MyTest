using System;
using System.Data;

using WinFramework.Base;

namespace AdManagerModel
{
	/// <summary>
	/// StatisticsRegionModel�� ���� ��� �����Դϴ�.
	/// </summary>
	public class StatisticsRegionModel : BaseModel
	{

		// ��ȸ��
		private string _SearchMediaCode	     = null;		// �˻� ��ü
		private string _SearchContractSeq    = null;		// ��ȸ ����ȣ
		private string _SearchItemNo         = null;		// ��ȸ �����ȣ ''�̸� ���� ��ü
		private string _SearchType           = null;		// ��ȸ ���� T:�ѱⰣ B:���ñⰣ D:�ϰ� W:�ְ� M:����
		private string _SearchStartDay       = null;		// ��ȸ ������� ����
		private string _SearchEndDay         = null;		// ��ȸ ������� ����
		private string _CampaignCode         = null;

		// �����ȸ��
		private DataSet  _DataSet;

		
		// �߰�������
		private int		_ContractAmt	= 0;		// ������(�������)
		private int		_TotalAdCnt     = 0;		// �ѳ���

		public StatisticsRegionModel() : base () 
		{
			Init();
		}

		#region Public �޼ҵ�
		public void Init()
		{
			_SearchMediaCode 	   = "";
			_SearchContractSeq     = "";
			_SearchItemNo          = "";
			_SearchType            = "";
			_SearchStartDay          = "";
			_SearchEndDay          = "";
			_CampaignCode          = "";
			_DataSet = new DataSet();
		}

		#endregion

		#region  ������Ƽ 

		public int ContractAmt
		{
			get { return _ContractAmt;	}
			set { _ContractAmt = value;	}
		}

		public int TotalAdCnt
		{
			get { return _TotalAdCnt;	}
			set { _TotalAdCnt = value;	}
		}

		public DataSet ReportDataSet
		{
			get { return _DataSet;	}
			set { _DataSet = value;	}
		}

		public string SearchMediaCode 
		{
			get { return _SearchMediaCode;	}
			set { _SearchMediaCode = value;	}
		}

		public string SearchContractSeq
		{
			get { return _SearchContractSeq;	}
			set { _SearchContractSeq = value;	}
		}

		public string SearchItemNo 
		{
			get { return _SearchItemNo;	}
			set { _SearchItemNo = value;	}
		}

		public string SearchType 
		{
			get { return _SearchType;	}
			set { _SearchType = value;	}
		}

		public string SearchStartDay 
		{
			get { return _SearchStartDay;	}
			set { _SearchStartDay = value;	}
		}

		public string SearchEndDay 
		{
			get { return _SearchEndDay;	}
			set { _SearchEndDay = value;	}
		}

		public string CampaignCode 
		{
			get { return _CampaignCode;	}
			set { _CampaignCode = value;	}
		}

		#endregion

	}
}