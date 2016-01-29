using System;
using System.Data;

using WinFramework.Base;

namespace AdManagerModel
{
    /// <summary>
    /// AdHitStatusModel�� ���� ��� �����Դϴ�.
    /// 2012/02/09 ��ȸ���� ����Ÿ�� �߰���
    /// </summary>
    public class AdHitStatusModel : BaseModel
    {
        // ��ȸ��
		private string _SearchMediaCode	    = null;		// �˻� ��ü
		private string _SearchRapCode	    = null;		// �˻� ��ü
		private string _SearchAgencyCode    = null;		// �˻� ��ü
		private string _SearchDay           = null;		// ��ȸ ��������
		private string _SearchKey           = null;		// ��ȸ �˻���
        private string _SearchAdType        = null;     // �˻� ��������

        // �����ȸ��
        private DataSet  _DataSet;

        public AdHitStatusModel() : base () 
        {
            Init();
        }

        #region Public �޼ҵ�
        public void Init()
        {
			_SearchMediaCode    = "";
			_SearchRapCode      = "";
			_SearchAgencyCode   = "";
			_SearchDay          = "";
			_SearchKey          = "";
            _SearchAdType       = "";
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

		public string SearchRapCode 
		{
			get { return _SearchRapCode;	}
			set { _SearchRapCode = value;	}
		}

		public string SearchAgencyCode 
		{
			get { return _SearchAgencyCode;	}
			set { _SearchAgencyCode = value;	}
		}

		public string SearchDay 
		{
			get { return _SearchDay;	}
			set { _SearchDay = value;	}
		}

		public string SearchKey
		{
			get { return _SearchKey;	}
			set { _SearchKey = value;	}
		}

        public string SearchAdType
        {
            get { return _SearchAdType;     }
            set { _SearchAdType = value;    }
        }
		#endregion
    }
}