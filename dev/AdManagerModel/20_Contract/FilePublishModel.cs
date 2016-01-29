using System;
using System.Data;

using WinFramework.Base;

namespace AdManagerModel
{
	/// <summary>
	/// FilePublishModel에 대한 요약 설명입니다.
	/// </summary>
	public class FilePublishModel : BaseModel
	{

		// 조회용
		private string _SearchKey       = null;	// 검색어
		private string _SearchMediaCode	= null;	// 검색 매체		
		private string _SearchReserveKey= null; // 예약작업번호

		// 상세정보용
		private string _MediaCode       = null;	// 매체코드
		private string _AckNo	        = null;	// 파일배포승인번호
		
		private string _ReserveDt		= null;	// 예약일시
		private string _State	        = null;	// 파일배포승인상태 10:승인대기  30:배포승인(배포)
		private string _PublishDesc     = null;	

		private string _ReserveUserNm   = null;
		private string _ModUserNm		= null;
		private string _ModDt			= null;

		private int	   _ItemNo			= 0;	// 작업대상 광고번호
		private string _ReserveJob		= null;	// 작업대상 광고 처리구분(+추가,-삭제)
		private	int    _TotalCount		= 0;	// 전체   파일수
		private int	   _NotCount		= 0;	// 미설정 파일수
 
		// 목록조회용
		private DataSet  _DataSet1;
		private DataSet  _DataSet2;
		private DataSet  _DataSet3;

		public FilePublishModel() : base () 
		{
			Init();
		}

		#region Public 메소드
		public void Init()
		{
			_SearchKey          = "";
			_SearchMediaCode	= "";		
			_SearchReserveKey	= "";

			_MediaCode          = "";
			_AckNo              = "";
			_ReserveDt			= "";
			_State              = "";
			_PublishDesc        = "";

			_ReserveUserNm		= "";
			_ModUserNm			= "";
			_ModDt				= "";
			_ItemNo				= 0;
			_ReserveJob			= "";
			_TotalCount			= 0;
			_NotCount			= 0;
			            
			_DataSet1  = new DataSet();
			_DataSet2  = new DataSet();
			_DataSet3  = new DataSet();
		}

		#endregion

		#region  프로퍼티 
		/// <summary>
		/// 예약작업코드
		/// </summary>
		public string SearchReserveKey
		{
			get { return _SearchReserveKey;	}
			set { _SearchReserveKey = value;	}
		}

		/// <summary>
		/// 미예약된 파일수
		/// </summary>
		public int NotCount
		{
			get { return _NotCount;	}
			set { _NotCount = value;	}
		}

		/// <summary>
		/// 수정일시
		/// </summary>
		public string ModDt
		{
			get { return _ModDt;	}
			set { _ModDt = value;	}
		}


		/// <summary>
		/// 전체 대상 파일수
		/// </summary>
		public int TotalCount
		{
			get { return _TotalCount;	}
			set { _TotalCount = value;	}
		}

		/// <summary>
		/// 예약작업 종류(+,-)
		/// </summary>
		public string JobType
		{
			get { return _ReserveJob;	}
			set { _ReserveJob = value;	}
		}

		/// <summary>
		/// 예약 광고번호
		/// </summary>
		public int ItemNo
		{
			get { return _ItemNo;	}
			set { _ItemNo = value;	}
		}

		/// <summary>
		/// 예약작업자 이름
		/// </summary>
		public string ReserveUserName
		{
			get { return _ReserveUserNm;	}
			set { _ReserveUserNm = value;	}
		}


		/// <summary>
		/// 예약작업 변경자 이름
		/// </summary>
		public string ModifyUserName
		{
			get { return _ModUserNm;	}
			set { _ModUserNm = value;	}
		}


		public DataSet PublishDataSet
		{
			get { return _DataSet1;	}
			set { _DataSet1 = value;	}
		}

		public DataSet HistoryDataSet
		{
			get { return _DataSet2;	}
			set { _DataSet2 = value;	}
		}
		
		public DataSet FileListDataSet
		{
			get { return _DataSet3;	}
			set { _DataSet3 = value;	}
		}

		public string SearchKey 
		{
			get { return _SearchKey;	}
			set { _SearchKey = value;	}
		}
	
		public string SearchMediaCode 
		{
			get { return _SearchMediaCode;	}
			set { _SearchMediaCode = value;	}
		}
		
		public string MediaCode 
		{
			get { return _MediaCode;		}
			set { _MediaCode = value;		}
		}

		/// <summary>
		/// 파일배포승인번호
		/// </summary>
		public string AckNo 
		{
			get { return _AckNo;		}
			set { _AckNo = value;		}
		}

		/// <summary>
		/// 예약작업 일시를 가져오거나 설정한다.
		/// </summary>
		public string ReserveDt
		{
			get { return _ReserveDt;		}
			set { _ReserveDt = value;		}
		}
		
		/// <summary>
		/// 파일배포예약상태
		/// </summary>
		public string State 
		{
			get { return _State;		}
			set { _State = value;		}
		}

		/// <summary>
		/// 예약작업 메세지
		/// </summary>
		public string PublishDesc 
		{
			get { return _PublishDesc;		}
			set { _PublishDesc = value;		}
		}


		#endregion

	}
}