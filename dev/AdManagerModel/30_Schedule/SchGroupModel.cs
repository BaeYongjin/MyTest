
using System;
using System.Data;

using WinFramework.Base;

namespace AdManagerModel
{
	/// <summary>
	/// 사용자정보의 클래스 모델.
	/// </summary>
	public class SchGroupModel : BaseModel
	{

		// 조회용
		private string _SearchMediaCode    = null;		// 검색매체

		// 상세조회용
		private string _MediaCode     = null;
		private string _CategoryCode  = null;
		private string _GroupCode     = null;
		private string _ChannelNo     = null;
		private string _ItemNo        = null;
		private string _ScheduleOrder = null;
		private string _ItemName      = null;
		private string _AdType              = null;		// 광고종류

		
		// 목록조회용
		private DataSet  _SchGroupDataSet;
		private DataSet  _ChooseAdScheduleDataSet;
        /// <summary>
        /// 편성현황 목록용
        /// </summary>
        private DataSet  _SchGroupList;                 

		private string _LastOrder     = null;

		// 미디어 콤보 조회용
			
		public SchGroupModel() : base () 
		{
			Init();
		}

		#region Public 메소드
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

		#region  프로퍼티 


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
