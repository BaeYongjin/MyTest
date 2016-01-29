using System;
using System.Data;

using WinFramework.Base;

namespace AdManagerModel
{
    /// <summary>
    /// PgmDailyRatingModel�� ���� ��� �����Դϴ�.
    /// </summary>
    public class PgmDailyRatingModel : BaseModel
    {

        // ��ȸ��
        private string _SearchMediaCode	     = null;		// �˻� ��ü
		private string _SearchDate           = null;		// ��ȸ ����

        // �����ȸ��
        private DataSet  _DataSet;

        public PgmDailyRatingModel() : base () 
        {
            Init();
        }

        #region Public �޼ҵ�
        public void Init()
        {
            _SearchMediaCode 	   = "";
			_SearchDate            = "";
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

		public string SearchDate 
		{
			get { return _SearchDate;	}
			set { _SearchDate = value;	}
		}

        #endregion

    }
}