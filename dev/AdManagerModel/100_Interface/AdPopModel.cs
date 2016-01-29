// ===============================================================================
// Contract Data Model for Charites Project
//
// AdPopModel.cs
//
// 채널점핑 클래스를 정의합니다. 
//
// ===============================================================================
// Release history
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
	public class AdPopModel : BaseModel
	{		
		// 상세정보용
		private string _ItemNo          = null;		// 광고번호		
		private string _MediaCode       = null;		// 매체코드
		private string _JumpType		= null;		// 점프구분
		private string _GenreCode1      = null;		// 장르코드
		private string _GenreCode2      = null;		// 장르코드
		private string _GenreCode3      = null;		// 장르코드
		private string _GenreCode4      = null;		// 장르코드
		private string _GenreCode5      = null;		// 장르코드
		private string _Flag	        = null;		// 장르명
		private string _ChannelNo       = null;		// 채널번호
		private string _Title           = null;		// 프로그램명
		private string _ContentID      = null;		// 컨텐츠ID
		private string _ContentID1      = null;		// 컨텐츠ID
		private string _ContentID2      = null;		// 컨텐츠ID
		private string _ContentID3      = null;		// 컨텐츠ID
		private string _ContentID4      = null;		// 컨텐츠ID
		private string _ContentID5      = null;		// 컨텐츠ID
		private string _AdPopID         = null;		// 공지ID
		private string _PopupTitle      = null;		// 공지제목
		private string _HomeYn          = null;		// 홈 노출여부
		private string _ChannelYn       = null;		// 채널 노출여부		


		// 목록조회용
		private DataSet  _DataSet;
		private DataSet  _DataSet2;	// 광고검색용
		private DataSet  _DataSet3;	// 채널검색용
		private DataSet  _DataSet4;	// 컨텐츠검색용
		
		private DataSet  _DataSet5; // 공지시스템 팝업공지리스트조회용


		public AdPopModel() : base () 
		{
			Init();
		}

		#region Public 메소드
		public void Init()
		{			
			_ItemNo      	= "";		
			_MediaCode   	= "";		
			_JumpType		= "";		
			_GenreCode1  	= "";		
			_GenreCode2  	= "";		
			_GenreCode3  	= "";		
			_GenreCode4  	= "";		
			_GenreCode5  	= "";		
			_Flag	    	= "";      
			_ChannelNo       = "";
			_Title           = "";
			_ContentID  	= "";      
			_ContentID1  	= "";      
			_ContentID2  	= "";
			_ContentID3		= "";
			_ContentID4		= "";
			_ContentID5		= "";
			_AdPopID		= "";
			_PopupTitle		= "";
			_HomeYn			= "";
			_ChannelYn		= "";

			
			_DataSet = new DataSet();
			_DataSet2 = new DataSet();
			_DataSet3 = new DataSet();
			_DataSet4 = new DataSet();
			_DataSet5 = new DataSet();
		}

		#endregion

		#region  프로퍼티 

		public DataSet ChannelJumpDataSet
		{
			get { return _DataSet;	}
			set { _DataSet = value;	}
		}

		public DataSet ContractItemDataSet
		{
			get { return _DataSet2;	}
			set { _DataSet2 = value;	}
		}

		public DataSet ChannelListDataSet
		{
			get { return _DataSet3;	}
			set { _DataSet3 = value;	}
		}

		public DataSet ContentListDataSet
		{
			get { return _DataSet4;	}
			set { _DataSet4 = value;	}
		}

		public DataSet AdPopListDataSet
		{
			get { return _DataSet5;	}
			set { _DataSet5 = value;	}
		}
		
		public string ItemNo           
		{
			get { return _ItemNo          ; }
			set { _ItemNo           = value;}
		}
		
		public string MediaCode 
		{
			get { return _MediaCode; }
			set { _MediaCode = value;}
		}
		public string JumpType 
		{
			get { return _JumpType; }
			set { _JumpType = value;}
		}
		public string GenreCode1 
		{
			get { return _GenreCode1; }
			set { _GenreCode1 = value;}
		}

		public string GenreCode2 
		{
			get { return _GenreCode2; }
			set { _GenreCode2 = value;}
		}

		public string GenreCode3 
		{
			get { return _GenreCode3; }
			set { _GenreCode3 = value;}
		}

		public string GenreCode4 
		{
			get { return _GenreCode4; }
			set { _GenreCode4 = value;}
		}
		
		public string GenreCode5 
		{
			get { return _GenreCode5; }
			set { _GenreCode5 = value;}
		}

		public string Flag 
		{
			get { return _Flag; }
			set { _Flag = value;}
		}
		
		public string ChannelNo 
		{
			get { return _ChannelNo; }
			set { _ChannelNo = value;}
		}
		public string Title 
		{
			get { return _Title; }
			set { _Title = value;}
		}

		public string ContentID
		{
			get { return _ContentID; }
			set { _ContentID = value;}
		}

		public string ContentID1 
		{
			get { return _ContentID1; }
			set { _ContentID1 = value;}
		}

		public string ContentID2 
		{
			get { return _ContentID2; }
			set { _ContentID2 = value;}
		}

		public string ContentID3 
		{
			get { return _ContentID3; }
			set { _ContentID3 = value;}
		}

		public string ContentID4 
		{
			get { return _ContentID4; }
			set { _ContentID4 = value;}
		}

		public string ContentID5 
		{
			get { return _ContentID5; }
			set { _ContentID5 = value;}
		}

		public string AdPopID 
		{
			get { return _AdPopID; }
			set { _AdPopID = value;}
		}
		public string PopupTitle
		{
			get { return _PopupTitle; }
			set { _PopupTitle = value;}
		}
		public string HomeYn 
		{
			get { return _HomeYn; }
			set { _HomeYn = value;}
		}
		public string ChannelYn 
		{
			get { return _ChannelYn; }
			set { _ChannelYn = value;}
		}
		 
		#endregion

	}
}
