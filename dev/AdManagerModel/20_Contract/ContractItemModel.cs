// ===============================================================================
// Contract Data Model for Charites Project
//
// ContractItemModel.cs
//
// 컨텐츠정보 클래스를 정의합니다. 
//
// ===============================================================================
// Release history
// 2007.07.16 송명환 v1.0
// ===============================================================================
// Copyright (C) 2007 G-Inno Systems Inc.
// All rights reserved.
// ==============================================================================

using System;
using System.Data;

using WinFramework.Base;

namespace AdManagerModel
{
    /// <summary>
    /// 사용자정보의 클래스 모델.
    /// </summary>
    public class ContractItemModel : BaseModel
    {

        // 조회용
        private string _SearchKey       = null;		// 검색어

        // 상세정보용
        private string _ItemNo          = null;		
        private string _MediaCode       = null;	
        private string _RapCode         = null;		
        private string _AgencyCode	    = null;	
        private string _AdvertiserCode  = null;
        private string _ContractSeq     = null;	
        private string _ItemName        = null;	
        private string _ExcuteStartDay  = null;
        private string _ExcuteEndDay    = null;
        private string _RealEndDay      = null; 
        private string _AdTime          = null;
        private string _AdState         = null;
        private string _AdClass         = null;
        private string _AdType          = null;
        private string _ScheduleType    = null;
        private string _AdRate          = null;
        private string _FileState       = null;
        private string _FileType        = null;
        private string _FileLength      = null;
        private string _FilePath        = null;
        private string _FileName        = null;
        private string _DownLevel       = null;
        private string _HistoryType       = null;
        private string _SearchchkAdState_10    = null;		// 검색 광고상태 : 준비
        private string _SearchchkAdState_20    = null;		// 검색 광고상태 : 편성
        private string _SearchchkAdState_30    = null;		// 검색 광고상태 : 중지
        private string _SearchchkAdState_40    = null;		// 검색 광고상태 : 종료
		private string _LinkChannel       = null;		
		private string _Mgrade       = null;
		private string _HomeYn       = null;
		private string _ChannelYn       = null;
		private string _CugYn           = null;
		private int _STBType = 0;

		private string _SearchMediaCode			= null;	
		private string _SearchChkSch_YN	 = null;		// 검색 편성여부

		private string _AdTypeChangeType = null;	// 광고종류변경 구분 0:변경없음 1:필수->옵션 2:옵션->필수

		private string _FileNo	 = null;		// 파일번호
		private string _FileTitle	 = null;		// 제목

		private string _Flag	 = null;		// 파일번호

        private string _ScheduleTypeName = null; //편성구분명 추가 

        private int _LinkType = 0;

        // 목록조회용
        private DataSet _ContractItemDataSet;
        private DataSet _LinkChannelDataSet;
        private DataSet _GradeDataSet;

        public ContractItemModel() : base () 
        {
            Init();
        }

        #region Public 메소드
        public void Init()
        {
            _SearchKey		 = "";

            _ItemNo          = "";		
            _MediaCode       = "";		
            _RapCode         = "";		
            _AgencyCode	   	 = "";		
            _AdvertiserCode  = "";		
            _ContractSeq     = "";		
            _ItemName        = "";		
            _ExcuteStartDay  = "";		
            _ExcuteEndDay    = "";      
            _RealEndDay      = "";      
            _AdTime         = "";
            _AdState         = "";
            _AdClass         = "";
            _AdType          = "";
            _ScheduleType    = "";
            _AdRate          = "";
            _FileState       = "";
            _FileType        = "";
            _FileLength      = "";
            _FilePath        = "";
            _FileName        = "";
            _DownLevel       = "";
            _SearchchkAdState_10 = "";
            _SearchchkAdState_20 = "";
            _SearchchkAdState_30 = "";
            _SearchchkAdState_40 = "";
			_LinkChannel = "";			
			_Mgrade = "";
			_HomeYn = "";
			_ChannelYn = "";

			_SearchMediaCode		= "";
			_SearchChkSch_YN	 = "";

			_FileNo	 = "";
			_FileTitle	 = "";

			_Flag	 = "";

			_ContractItemDataSet = new DataSet();
            _LinkChannelDataSet = new DataSet();
            _GradeDataSet = new DataSet();

			_AdTypeChangeType = "0";

            _ScheduleTypeName = ""; //추가 
        }

        #endregion

        #region  프로퍼티 

		public string AdTypeChangeType
		{
			get { return _AdTypeChangeType;	}
			set { _AdTypeChangeType = value;	}
		}

        public DataSet ContractItemDataSet
        {
            get { return _ContractItemDataSet;	}
            set { _ContractItemDataSet = value;	}
        }

        public DataSet LinkChannelDataSet
        {
            get { return _LinkChannelDataSet; }
            set { _LinkChannelDataSet = value; }
        }


		public DataSet GradeDataSet
		{
			get { return _GradeDataSet;	}
			set { _GradeDataSet = value;	}
		}

        public string SearchKey 
        {
            get { return _SearchKey;	}
            set { _SearchKey = value;	}
        }
	
        public string MediaCode
        {
            get { return _MediaCode; }
            set { _MediaCode = value;}
        }
        public string RapCode
        {
            get { return _RapCode; }
            set { _RapCode = value;}
        }

        public string AgencyCode 
        {
            get { return _AgencyCode; }
            set { _AgencyCode = value;}
        }

        public string AdvertiserCode
        {
            get { return _AdvertiserCode;}
            set { _AdvertiserCode= value;}
        }
        public string ContractSeq 
        {
            get { return _ContractSeq; }
            set { _ContractSeq = value;}
        }
        public string ItemNo           
        {
            get { return _ItemNo          ; }
            set { _ItemNo           = value;}
        }
        public string ItemName 
        {
            get { return _ItemName; }
            set { _ItemName = value;}
        }
        public string ExcuteStartDay 
        {
            get { return _ExcuteStartDay; }
            set { _ExcuteStartDay = value;}
        }
        public string ExcuteEndDay 
        {
            get { return _ExcuteEndDay; }
            set { _ExcuteEndDay = value;}
        }
        public string RealEndDay 
        {
            get { return _RealEndDay; }
            set { _RealEndDay = value;}
        }
        public string AdTime 
        {
            get { return _AdTime; }
            set { _AdTime = value;}
        }
        public string AdState 
        {
            get { return _AdState; }
            set { _AdState = value;}
        }
        public string AdClass 
        {
            get { return _AdClass; }
            set { _AdClass = value;}
        }
        public string AdType 
        {
            get { return _AdType; }
            set { _AdType = value;}
        }
        public string ScheduleType 
        {
            get { return _ScheduleType; }
            set { _ScheduleType = value;}
        }
        public string AdRate 
        {
            get { return _AdRate; }
            set { _AdRate = value;}
        }
        public string FileState 
        {
            get { return _FileState; }
            set { _FileState = value;}
        }
        public string FileType 
        {
            get { return _FileType; }
            set { _FileType = value;}
        }
        public string FileLength     
        {
            get { return _FileLength; }
            set { _FileLength = value;}
        }
        public string FilePath 
        {
            get { return _FilePath; }
            set { _FilePath = value;}
        }
        public string FileName 
        {
            get { return _FileName; }
            set { _FileName = value;}
        }
        public string DownLevel 
        {
            get { return _DownLevel; }
            set { _DownLevel = value;}
        }
        public string HistoryType 
        {
            get { return _HistoryType; }
            set { _HistoryType = value;}
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

		public string LinkChannel 
		{
			get { return _LinkChannel;		}
			set { _LinkChannel = value;		}
		}

		public string Mgrade 
		{
			get { return _Mgrade;		}
			set { _Mgrade = value;		}
		}

		public string HomeYn
		{
			get { return _HomeYn;		}
			set { _HomeYn = value;		}
		}

		public string ChannelYn 
		{
			get { return _ChannelYn;		}
			set { _ChannelYn = value;		}
		}
		/// <summary>
		/// CUG여부(Y:cug전용,N:일반공통)
		/// </summary>
		public string CugYn
		{
			get{ return _CugYn;}
			set{ _CugYn = value;}
		}

		/// <summary>
		/// 셋탑타입(0:공용,1:기본전용,2:삼성전용)
		/// </summary>
		public int STBType
		{
			get { return _STBType; }
			set { _STBType = value; }
		}

		public string SearchMediaCode
		{
			get { return _SearchMediaCode; }
			set { _SearchMediaCode = value;}
		}

		public string SearchChkSch_YN
		{
			get { return _SearchChkSch_YN;		}
			set { _SearchChkSch_YN = value;		}
		}

		public string FileNo
		{
			get { return _FileNo;		}
			set { _FileNo = value;		}
		}

		public string FileTitle
		{
			get { return _FileTitle;		}
			set { _FileTitle = value;		}
		}

		public string Flag
		{
			get { return _Flag;		}
			set { _Flag = value;		}
		}

        public string ScheduleTypeName
        {
            get { return _ScheduleTypeName; }
            set { _ScheduleTypeName = value; }
        }


        /// <summary>
        /// 연결채널타입(1:광고안보기연결채널,2:광고보기연결채널)
        /// </summary>
        public int LinkType
        {
            get { return _LinkType; }
            set { _LinkType = value; }
        }

        #endregion

    }
}
