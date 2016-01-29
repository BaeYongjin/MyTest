using System;
using System.Data;

using WinFramework.Base;

namespace AdManagerModel
{
    /// <summary>
    /// CategoryRating�� ���� ��� �����Դϴ�.
    /// </summary>
    public class CategoryRatingModel : BaseModel
    {

        // ��ȸ��
        private string _SearchMediaCode	     = null;		// �˻� ��ü
		private string _SearchType           = null;		// ��ȸ ���� D:�ϰ� W:�ְ� M:����
		private string _SearchBgnDay         = null;		// ��ȸ ���� ��������
		private string _SearchEndDay         = null;		// ��ȸ ���� ��������	

        // �����ȸ��
        private DataSet  _DataSet;

        public CategoryRatingModel() : base () 
        {
            Init();
        }

        #region Public �޼ҵ�
        public void Init()
        {
            _SearchMediaCode 	   = "";
			_SearchType            = "";
			_SearchBgnDay          = "";
			_SearchEndDay          = "";
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

		#endregion

    }
}