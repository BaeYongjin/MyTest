using System;
using System.Data;

using WinFramework.Base;

namespace AdManagerModel
{
    /// <summary>
    /// SummaryAdModel�� ���� ��� �����Դϴ�.
    /// </summary>
    public class SummaryAdModel : BaseModel
    {

        // ��ȸ��
        private string _SearchMediaCode	     = null;		// �˻� ��ü
		private string _SearchContractSeq    = null;		// ��ȸ ����ȣ
		private string _SearchItemNo         = null;		// ��ȸ �����ȣ ''�̸� ���� ��ü
		private string _SearchType           = null;		// ��ȸ ���� T:�ѱⰣ D:�ϰ� W:�ְ� M:����
		private string _SearchStartDay       = null;		// ��ȸ ������� ����
		private string _SearchEndDay         = null;		// ��ȸ ������� ����
		private string _CampaignCode         = null;
        private int _MenuLevel          = 0;

        // �����ȸ��
        private DataSet  _DataSet;
		
		// ��������
		private DataSet _DataSet2;

		// ��ȸ �̿��ڼ�
		private int		_TotalUser	= 0;

        public SummaryAdModel() : base () 
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
			_DataSet  = new DataSet();
			_DataSet2 = new DataSet();

			_TotalUser = 0;
            _MenuLevel = 0;
		}

        #endregion

        #region  ������Ƽ 

		public int TotalUser
		{
			get { return _TotalUser;	}
			set { _TotalUser = value;	}
		}

        public DataSet ReportDataSet
        {
            get { return _DataSet;	}
            set { _DataSet = value;	}
        }

		public DataSet ItemDataSet
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

        /// <summary>
        /// ������ ī�װ�[1]/ī�װ�+�帣[2]�� ������
        /// 2011�� ��ȭ���� �帣������ ������
        /// </summary>
        public int MenuLevel
        {
            get { return _MenuLevel; }
            set { _MenuLevel = value; }
        }
		#endregion

    }
}