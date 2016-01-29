
using System;
using System.Data;

using WinFramework.Base;

namespace AdManagerModel
{
    /// <summary>
    /// ����������� Ŭ���� ��.
    /// </summary>
    public class SlotAdInfoModel : BaseModel
    {
        #region ���� ����

        // ��ȸ��
        private string _SearchMediaCode    = null;		// �˻���ü

		// ����ȸ��
		private string _MediaCode     = null;
		private string _CategoryCode  = null;
		private string _MenuCode     = null;
        private int     _MaxCount = 0;		        // ���ᱤ���� �ִ밹��
        private int     _MaxTime = 0;		        // ���ᱤ���� �ִ�ð�
        private int     _MaxCountPay = 0;		    // ���ᱤ���� �ִ밹��
        private int     _MaxTimePay = 0;		    // ���ᱤ���� �ִ�ð�
        private string _UseDate = null;		        // ��������Ͻ�
        private string _UseYn = null;               // ��뿩��
        private string _PromotionYn = null;         // ���θ�� ���� ����

        private bool _IsSetDataOnly = true;
		
        // �����ȸ��
        private DataSet  _SlotAdInfoDataSet;

        #endregion 

        #region ������
        public SlotAdInfoModel() : base () 
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

            _UseYn = "";
            _PromotionYn = "";

            _IsSetDataOnly = true;

            _SlotAdInfoDataSet = new DataSet();
        }

        #endregion

        #region  ������Ƽ 

        public DataSet SlotAdInfoDataSet
        {
            get { return _SlotAdInfoDataSet;	}
            set { _SlotAdInfoDataSet = value;	}
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

        public string PromotionYn
        {
            get { return _PromotionYn; }
            set { _PromotionYn = value; }
        }

        public string UseYn
        {
            get { return _UseYn; }
            set { _UseYn = value; }
        }

        public bool IsSetDataOnly
        {
            get { return _IsSetDataOnly; }
            set { _IsSetDataOnly = value; }
        }

        #endregion

    }
}
