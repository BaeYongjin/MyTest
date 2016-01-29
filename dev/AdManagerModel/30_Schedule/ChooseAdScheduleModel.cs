
using System;
using System.Data;

using WinFramework.Base;

namespace AdManagerModel
{
    /// <summary>
    /// ����������� Ŭ���� ��.
    /// </summary>
    public class ChooseAdScheduleModel : BaseModel
    {

        // ��ȸ��
        private string _SearchMediaCode    = null;		// �˻���ü

		// ����ȸ��
		private string _MediaCode     = null;
		private string _CategoryCode  = null;
		private string _GenreCode     = null;
		private string _ChannelNo     = null;
		private	string _Series		  = null;
		private string _ItemNo        = null;
		private string _ScheduleOrder = null;
		private string _ItemName      = null;
        private string _SearchchkAdState_10    = null;		// �˻� ������� : �غ�
        private string _SearchchkAdState_20    = null;		// �˻� ������� : ��
        private string _SearchchkAdState_30    = null;		// �˻� ������� : ����
        private string _SearchchkAdState_40    = null;		// �˻� ������� : ����
        
		
        // �����ȸ��
        private DataSet  _ChooseAdScheduleDataSet;    

		private string _LastOrder     = null;

        // �̵�� �޺� ��ȸ��
			
        public ChooseAdScheduleModel() : base () 
        {
            Init();
        }

        #region Public �޼ҵ�
        public void Init()
        {			
            _SearchMediaCode = "";

			_MediaCode      = "";
			_CategoryCode   = "";
			_GenreCode      = "";
			_ChannelNo      = "";
			_Series			= "";
			_ItemNo         = "";
			_ScheduleOrder  = "";
			_ItemName       = "";
            _SearchchkAdState_10   = "";
            _SearchchkAdState_20   = "";
            _SearchchkAdState_30   = "";
            _SearchchkAdState_40   = "";

            _ChooseAdScheduleDataSet = new DataSet();
			_LastOrder       = "";
        }

        #endregion

        #region  ������Ƽ 

        public DataSet ChooseAdScheduleDataSet
        {
            get { return _ChooseAdScheduleDataSet;	}
            set { _ChooseAdScheduleDataSet = value;	}
        }

        public string SearchMediaCode
        {
            get { return _SearchMediaCode;	}
            set { _SearchMediaCode = value;	}
        }

		public string ItemNo
		{
			get { return _ItemNo;	}
			set { _ItemNo = value;	}
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

		public string GenreCode
		{
			get { return _GenreCode;	}
			set { _GenreCode = value;	}
		}

		public string ChannelNo
		{
			get { return _ChannelNo;	}
			set { _ChannelNo = value;	}
		}

		/// <summary>
		/// ä�ι�ȣ�� �������ų� �����մϴ�.
		/// </summary>
		public string SeriesNo
		{
			get { return _Series;	}
			set { _Series = value;	}
		}

		public string ScheduleOrder
		{
			get { return _ScheduleOrder;	}
			set { _ScheduleOrder = value;	}
		}

        public string SearchchkAdState_10 
        {
            get { return _SearchchkAdState_10;		}
            set { _SearchchkAdState_10 = value;		}
        }

        public string SearchchkAdState_20 
        {
            get { return _SearchchkAdState_20;		}
            set { _SearchchkAdState_20 = value;		}
        }

        public string SearchchkAdState_30 
        {
            get { return _SearchchkAdState_30;		}
            set { _SearchchkAdState_30 = value;		}
        }

        public string SearchchkAdState_40 
        {
            get { return _SearchchkAdState_40;		}
            set { _SearchchkAdState_40 = value;		}
        }

		public string ItemName
		{
			get { return _ItemName;	}
			set { _ItemName = value;	}
		}

		public string LastOrder
		{
			get { return _LastOrder;	}
			set { _LastOrder = value;	}
		}

		#endregion

    }
}
