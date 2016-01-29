using System;
using System.Data;

using WinFramework.Base;

namespace AdManagerModel
{
    /// <summary>
    /// DailyAdHitModel�� ���� ��� �����Դϴ�.
    /// </summary>
    public class DailyAdHitModel : BaseModel
    {

        // ��ȸ��
        private string _SearchMediaCode	     = null;		// �˻� ��ü
		private string _SearchContractSeq    = null;		// ��ȸ ����ȣ
		private string _SearchItemNo         = null;		// ��ȸ �����ȣ
		private string _SearchType           = null;		// ��ȸ ���� D:���ñⰣ C:���Ⱓ
		private string _SearchBgnDay         = null;		// ��ȸ ���� ��������
		private string _SearchEndDay         = null;		// ��ȸ ���� ��������	
		private string _CampaignCode         = null;

        // �����ȸ��
        private DataSet  _DataSet;
		private DataSet _DataSet2;

        public DailyAdHitModel() : base () 
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
			_SearchBgnDay          = "";
			_SearchEndDay          = "";
			_CampaignCode          = "";
			_DataSet = new DataSet();
			_DataSet2 = new DataSet();
		}

        #endregion

        #region  ������Ƽ 

        public DataSet ReportDataSet
        {
            get { return _DataSet;	}
            set { _DataSet = value;	}
        }

		public DataSet HeaderDataSet
		{
			get { return _DataSet2;	}
			set { _DataSet2 = value;	}
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

		public string SearchBgnDay 
		{
			get { return _SearchBgnDay;	}
			set { _SearchBgnDay = value;	}
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