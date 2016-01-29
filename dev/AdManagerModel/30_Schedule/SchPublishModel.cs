using System;
using System.Data;

using WinFramework.Base;

namespace AdManagerModel
{
	/// <summary>
	/// SchPublishModel에 대한 요약 설명입니다.
	/// </summary>
	public class SchPublishModel : BaseModel
	{

		// 조회용
		private string _SearchKey              = null;		// 검색어
		private string _SearchMediaCode	       = null;		// 검색 매체		

		// 상세정보용
		private string _AckNo	            = null;		// 광고승인번호
		private string _State	            = null;		// 광고승인상태 10:편성중(편성) 20:편성승인(승인) 30:배포승인(배포)
		private string _AckDesc             = null;
		private string _ChkDesc             = null;
		private string _PublishDesc         = null;

 
		// 목록조회용
		private DataSet  _ScheduleDataSet;
		private DataSet  _PublishDataSet;

		public SchPublishModel() : base () 
		{
			Init();
		}

		#region Public 메소드
		public void Init()
		{
			_SearchKey              = "";
			_SearchMediaCode	    = "";			

			_AckNo              = "";
			_State              = "";
			_AckDesc            = "";
			_ChkDesc            = "";
			_PublishDesc        = "";

			            
			_ScheduleDataSet = new DataSet();
			_PublishDataSet  = new DataSet();
		}

		#endregion

		#region  프로퍼티 

		public DataSet PublishDataSet
		{
			get { return _PublishDataSet;	}
			set { _PublishDataSet = value;	}
		}

		public DataSet ScheduleDataSet
		{
			get { return _ScheduleDataSet;	}
			set { _ScheduleDataSet = value;	}
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
		
		public string AckNo 
		{
			get { return _AckNo;		}
			set { _AckNo = value;		}
		}
		
		public string State 
		{
			get { return _State;		}
			set { _State = value;		}
		}
		
		public string AckDesc 
		{
			get { return _AckDesc;		}
			set { _AckDesc = value;		}
		}

		public string ChkDesc 
		{
			get { return _ChkDesc;		}
			set { _ChkDesc = value;		}
		}
		
		public string PublishDesc 
		{
			get { return _PublishDesc;		}
			set { _PublishDesc = value;		}
		}


		#endregion

	}
}