using System;
using System.Data;

using WinFramework.Base;

namespace AdManagerModel
{
    /// <summary>
    /// ProgramHitModel�� ���� ��� �����Դϴ�.
    /// </summary>
    public class ProgramAdHitModel : BaseModel
    {

        // ��ȸ��
        private string _SearchMediaCode	     = null;		// �˻� ��ü
		private string _SearchContractSeq    = null;		// ��ȸ ����ȣ
		private string _SearchItemNo         = null;		// ��ȸ �����ȣ
		private string _SearchType           = null;		// ��ȸ ���� D:���ñⰣ C:���Ⱓ
		private string _SearchBgnDay         = null;		// ��ȸ ���� ��������
		private string _SearchEndDay         = null;		// ��ȸ ���� ��������	
		private string _CampaignCode         = null;		// ��ȸ ���� ��������	

        // �����ȸ��
        private DataSet  _DataSet;

        public ProgramAdHitModel() : base () 
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
        }

        #endregion

        #region  ������Ƽ 

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

		public string SearchItemNo 
		{
			get { return _SearchItemNo;	}
			set { _SearchItemNo = value;	}
		}

		public string SearchContractSeq
		{
			get { return _SearchContractSeq;	}
			set { _SearchContractSeq = value;	}
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