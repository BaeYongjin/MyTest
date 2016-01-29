
using System;
using System.Data;

using WinFramework.Base;

namespace AdManagerModel
{
	/// <summary>
	/// 사용자정보의 클래스 모델.
	/// </summary>
	public class SlotOrganizationModel : BaseModel
	{

		// 조회용
		private string _SearchKey       = null;		// 검색어
		private string _SearchMediaName = null;		// 검색레벨
		private string _SearchCategoryName = null;		// 검색레벨
		private string _SearchGenreName = null;		// 검색레벨
		private string _SearchchkUseYn = null;		// 검색레벨
		
		// 상세정보용
		private string _MediaCode       = null;		// 매체코드	
		private string _MediaCode_P       = null;		// 매체코드		
		private string _MediaCode_old       = null;		// 매체코드			
		private string _MediaName       = null;		// 매체명칭
		private string _CategoryCode       = null;		// 매체코드	
		private string _CategoryCode_P       = null;		// 매체코드
		private string _CategoryCode_old       = null;		// 매체코드
		private string _CategoryName       = null;		// 매체코드
		private string _GenreCode       = null;		// 매체코드	
		private string _GenreCode_P       = null;		// 매체코드		
		private string _GenreCode_old       = null;		// 매체코드			
		private string _GenreName       = null;		// 매체코드		
		private string _ChannelNo       = null;		// 컨텐츠 등급			
		private string _ChannelNo_old       = null;		// 컨텐츠 등급
		private string _Title           = null;		// 컨텐츠명				
		private string _Slot1           = null;		// 컨텐츠명				
		private string _Slot2           = null;		// 컨텐츠명				
		private string _Slot3           = null;		// 컨텐츠명				
		private string _Slot4           = null;		// 컨텐츠명				
		private string _Slot5           = null;		// 컨텐츠명				
		private string _Slot6           = null;		// 컨텐츠명				
		private string _Slot7           = null;		// 컨텐츠명				
		private string _Slot8           = null;		// 컨텐츠명				
		private string _Slot9           = null;		// 컨텐츠명				
		private string _Slot10           = null;		// 컨텐츠명				
		private string _Slot11           = null;		// 컨텐츠명				
		private string _Slot12           = null;		// 컨텐츠명				
		private string _Slot13           = null;		// 컨텐츠명				
		private string _Slot14           = null;		// 컨텐츠명				
		private string _Slot15           = null;		// 컨텐츠명				
		private string _UseYn           = null;		// 최종수정일시
		private string _RegDt           = null;		// 최종수정일시
		private string _ModDt           = null;		// 최종수정일시
		private string _RegID           = null;		// 최종수정일시
			
		// 목록조회용
		private DataSet  _SlotDataSet;
		private DataSet  _CategoryDataSet;
		private DataSet  _GenreDataSet;
		private DataSet  _ChannelSetDataSet;
		private DataSet  _SlotCodeDataSet;
				
		// 미디어 콤보 조회용
			
		public SlotOrganizationModel() : base () 
		{
			Init();
		}

		#region Public 메소드
		public void Init()
		{			
			_SearchKey		= "";
			_SearchMediaName		= "";
			_SearchCategoryName		= "";
			_SearchGenreName		= "";
			_SearchchkUseYn			= "";

			_MediaCode       = "";		// 매체코드		
			_MediaCode_P       = "";		// 매체코드
			_MediaCode_old       = "";		// 매체코드
			_MediaName       = "";
			_CategoryCode       = "";		// 매체코드	
			_CategoryCode_P       = "";		// 매체코드
			_CategoryCode_old       = "";		// 매체코드
			_CategoryName       = "";		// 매체코드
			_GenreCode       = "";		// 매체코드	
			_GenreCode_P       = "";		// 매체코드
			_GenreCode_old       = "";		// 매체코드
			_GenreName       = "";		// 매체코드
			_ChannelNo       = "";		// 채널코드
			_ChannelNo_old       = "";		// 채널코드
			_Title           = "";      // 컨텐츠명
			_Slot1           = "";      // 컨텐츠명
			_Slot2           = "";      // 컨텐츠명
			_Slot3           = "";      // 컨텐츠명
			_Slot4           = "";      // 컨텐츠명
			_Slot5           = "";      // 컨텐츠명
			_Slot6           = "";      // 컨텐츠명
			_Slot7           = "";      // 컨텐츠명
			_Slot8           = "";      // 컨텐츠명
			_Slot9           = "";      // 컨텐츠명
			_Slot10           = "";      // 컨텐츠명
			_Slot11           = "";      // 컨텐츠명
			_Slot12           = "";      // 컨텐츠명
			_Slot13           = "";      // 컨텐츠명
			_Slot14           = "";      // 컨텐츠명
			_Slot15           = "";      // 컨텐츠명
			_UseYn           = "";
			_RegDt           = "";		// 등록일자
			_ModDt           = "";		// 등록일자
			_RegID           = "";		// 등록일자			
	

			_SlotDataSet = new DataSet();
			_CategoryDataSet = new DataSet();
			_GenreDataSet = new DataSet();
			_ChannelSetDataSet = new DataSet();
			_SlotCodeDataSet = new DataSet();

		}

		#endregion

		#region  프로퍼티 

		public DataSet SlotDataSet
		{
			get { return _SlotDataSet;	}
			set { _SlotDataSet = value;	}
		}

		public DataSet CategoryDataSet
		{
			get { return _CategoryDataSet;	}
			set { _CategoryDataSet = value;	}
		}

		public DataSet GenreDataSet
		{
			get { return _GenreDataSet;	}
			set { _GenreDataSet = value;	}
		}

		public DataSet ChannelSetDataSet
		{
			get { return _ChannelSetDataSet;	}
			set { _ChannelSetDataSet = value;	}
		}

		public DataSet SlotCodeDataSet
		{
			get { return _SlotCodeDataSet;	}
			set { _SlotCodeDataSet = value;	}
		}
		
		public string SearchKey 
		{
			get { return _SearchKey;	}
			set { _SearchKey = value;	}
		}

		public string SearchMediaName 
		{
			get { return _SearchMediaName;	}
			set { _SearchMediaName = value;	}
		}

		public string SearchCategoryName 
		{
			get { return _SearchCategoryName;	}
			set { _SearchCategoryName = value;	}
		}

		public string SearchGenreName 
		{
			get { return _SearchGenreName;	}
			set { _SearchGenreName = value;	}
		}

		public string SearchchkUseYn 
		{
			get { return _SearchchkUseYn;	}
			set { _SearchchkUseYn = value;	}
		}

		public string MediaCode
		{
			get { return _MediaCode;		}
			set { _MediaCode= value;	}
		}

		public string MediaCode_P
		{
			get { return _MediaCode_P;		}
			set { _MediaCode_P= value;	}
		}

		public string MediaCode_old
		{
			get { return _MediaCode_old;		}
			set { _MediaCode_old= value;	}
		}

		public string MediaName
		{
			get { return _MediaName;		}
			set { _MediaName= value;	}
		}

		public string CategoryCode
		{
			get { return _CategoryCode;		}
			set { _CategoryCode= value;	}
		}

		public string CategoryCode_P
		{
			get { return _CategoryCode_P;		}
			set { _CategoryCode_P= value;	}
		}

		public string CategoryCode_old
		{
			get { return _CategoryCode_old;		}
			set { _CategoryCode_old= value;	}
		}

		public string CategoryName
		{
			get { return _CategoryName;		}
			set { _CategoryName= value;	}
		}

		public string GenreCode
		{
			get { return _GenreCode;		}
			set { _GenreCode= value;		}
		}

		public string GenreCode_P
		{
			get { return _GenreCode_P;		}
			set { _GenreCode_P= value;		}
		}

		public string GenreCode_old
		{
			get { return _GenreCode_old;		}
			set { _GenreCode_old= value;		}
		}
		
		public string GenreName
		{
			get { return _GenreName;		}
			set { _GenreName= value;		}
		}

		public string ChannelNo
		{
			get { return _ChannelNo; }
			set { _ChannelNo = value;}
		}

		public string ChannelNo_old
		{
			get { return _ChannelNo_old; }
			set { _ChannelNo_old = value;}
		}
        		
		public string Title
		{
			get { return _Title; }
			set { _Title = value;}
		}
		
		public string Slot1
		{
			get { return _Slot1; }
			set { _Slot1 = value;}
		}

		public string Slot2
		{
			get { return _Slot2; }
			set { _Slot2 = value;}
		}

		public string Slot3
		{
			get { return _Slot3; }
			set { _Slot3 = value;}
		}

		public string Slot4
		{
			get { return _Slot4; }
			set { _Slot4 = value;}
		}

		public string Slot5
		{
			get { return _Slot5; }
			set { _Slot5 = value;}
		}

		public string Slot6
		{
			get { return _Slot6; }
			set { _Slot6 = value;}
		}

		public string Slot7
		{
			get { return _Slot7; }
			set { _Slot7 = value;}
		}

		public string Slot8
		{
			get { return _Slot8; }
			set { _Slot8 = value;}
		}

		public string Slot9
		{
			get { return _Slot9; }
			set { _Slot9 = value;}
		}

		public string Slot10
		{
			get { return _Slot10; }
			set { _Slot10 = value;}
		}

		public string Slot11
		{
			get { return _Slot11; }
			set { _Slot11 = value;}
		}

		public string Slot12
		{
			get { return _Slot12; }
			set { _Slot12 = value;}
		}

		public string Slot13
		{
			get { return _Slot13; }
			set { _Slot13 = value;}
		}

		public string Slot14
		{
			get { return _Slot14; }
			set { _Slot14 = value;}
		}

		public string Slot15
		{
			get { return _Slot15; }
			set { _Slot15 = value;}
		}
		
		public string UseYn 
		{
			get { return _UseYn;		}
			set { _UseYn = value;		}
		}

		public string RegDt
		{
			get { return _RegDt;		}
			set { _RegDt= value;		}
		}

		public string ModDt
		{
			get { return _ModDt;		}
			set { _ModDt= value;		}
		}

		public string RegID
		{
			get { return _RegID;		}
			set { _RegID= value;		}
		}        

		#endregion

	}
}
