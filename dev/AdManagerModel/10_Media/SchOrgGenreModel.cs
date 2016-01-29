
using System;
using System.Data;

using WinFramework.Base;

namespace AdManagerModel
{
    /// <summary>
    /// ����������� Ŭ���� ��.
    /// </summary>
    public class SchOrgGenreModel : BaseModel
    {
        #region ���� ����

        // ��ȸ��
        private string _SearchMediaCode    = null;		// �˻���ü
        private string _SearchKey = null;

		// ����ȸ��
		private string _MediaCode     = null;
		private string _CategoryCode  = null;
		private string _MenuCode     = null;
        private int     _MaxCount = 0;		        // ���ᱤ���� �ִ밹��
        private int     _MaxTime = 0;		        // ���ᱤ���� �ִ�ð�
        private int     _MaxCountPay = 0;		    // ���ᱤ���� �ִ밹��
        private int     _MaxTimePay = 0;		    // ���ᱤ���� �ִ�ð�
        private string _UseDate = null;		        // ��������Ͻ�

        private bool _IsSetDataOnly = true;
		
        // �����ȸ��
        private DataSet  _SchOrgGenreDataSet;

        #endregion 

        #region ������
        public SchOrgGenreModel() : base () 
        {
            Init();
        }
        #endregion
 
        #region Public �޼ҵ�
        public void Init()
        {			
            _SearchMediaCode = "";

			_MediaCode      = "";
			_CategoryCode   = "";
			_MenuCode      = "";
            _MaxCount = 0;
            _MaxTime = 0;
            _MaxCountPay = 0;
            _MaxTimePay = 0;
            _UseDate = "";
            _IsSetDataOnly = true;
            _SearchKey = "";

            _SchOrgGenreDataSet = new DataSet();
        }

        #endregion

        #region  ������Ƽ 

        public DataSet SchOrgGenreDataSet
        {
            get { return _SchOrgGenreDataSet;	}
            set { _SchOrgGenreDataSet = value;	}
        }

        public string SearchMediaCode
        {
            get { return _SearchMediaCode;	}
            set { _SearchMediaCode = value;	}
        }

		public string MediaCode
		{
			get { return _MediaCode;	}
			set { _MediaCode = value;	}
		}

		public string CategoryCode
		{
			get { return _CategoryCode;	}
			set { _CategoryCode = value;	}
		}

		public string MenuCode
		{
			get { return _MenuCode;	}
			set { _MenuCode = value;	}
		}
        
        public int MaxCount
		{
			get { return _MaxCount;	}
			set { _MaxCount = value;	}
		}
        
        public int MaxTime
		{
			get { return _MaxTime;	}
			set { _MaxTime = value;	}
		}

        public int MaxCountPay
		{
			get { return _MaxCountPay;	}
			set { _MaxCountPay = value;	}
		}
        
        public int MaxTimePay
		{
			get { return _MaxTimePay;	}
			set { _MaxTimePay = value;	}
		}

        public string UseDate
        {
            get { return _UseDate; }
            set { _UseDate = value; }
        }

        public bool IsSetDataOnly
        {
            get { return _IsSetDataOnly; }
            set { _IsSetDataOnly = value; }
        }

        public string SearchKey
        {
            get { return _SearchKey; }
            set { _SearchKey = value; }
        }
	
        #endregion

    }
}
