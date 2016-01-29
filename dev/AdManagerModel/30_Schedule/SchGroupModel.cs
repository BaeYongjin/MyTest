
using System;
using System.Data;

using WinFramework.Base;

namespace AdManagerModel
{
	/// <summary>
	/// ����������� Ŭ���� ��.
	/// </summary>
	public class SchGroupModel : BaseModel
	{

		// ��ȸ��
		private string _SearchMediaCode    = null;		// �˻���ü

		// ����ȸ��
		private string _MediaCode     = null;
		private string _CategoryCode  = null;
		private string _GroupCode     = null;
		private string _ChannelNo     = null;
		private string _ItemNo        = null;
		private string _ScheduleOrder = null;
		private string _ItemName      = null;
		private string _AdType              = null;		// ��������

		
		// �����ȸ��
		private DataSet  _SchGroupDataSet;
		private DataSet  _ChooseAdScheduleDataSet;
        /// <summary>
        /// ����Ȳ ��Ͽ�
        /// </summary>
        private DataSet  _SchGroupList;                 

		private string _LastOrder     = null;

		// �̵�� �޺� ��ȸ��
			
		public SchGroupModel() : base () 
		{
			Init();
		}

		#region Public �޼ҵ�
		public void Init()
		{			
			_SearchMediaCode = "";

			_MediaCode       = "";
			_CategoryCode    = "";
			_GroupCode       = "";
			_ChannelNo       = "";
			_ItemNo          = "";
			_ScheduleOrder   = "";
			_ItemName        = "";
			_AdType                = "";

			_SchGroupDataSet            = new DataSet();
			_ChooseAdScheduleDataSet    = new DataSet();
            _SchGroupList               = new DataSet();
			_LastOrder       = "";
		}

		#endregion

		#region  ������Ƽ 


        public DataSet SchGroupList
        {
            get { return _SchGroupList;	}
            set { _SchGroupList = value;	}
        }

		public DataSet SchGroupDataSet
		{
			get { return _SchGroupDataSet;	}
			set { _SchGroupDataSet = value;	}
		}

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

		public string GroupCode
		{
			get { return _GroupCode;	}
			set { _GroupCode = value;	}
		}

		public string ChannelNo
		{
			get { return _ChannelNo;	}
			set { _ChannelNo = value;	}
		}

		public string ScheduleOrder
		{
			get { return _ScheduleOrder;	}
			set { _ScheduleOrder = value;	}
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

		public string AdType
		{
			get { return _AdType;	}
			set { _AdType = value;	}
		}

		#endregion

	}
}
