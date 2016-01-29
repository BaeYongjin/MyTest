using System;
using System.Data;

using WinFramework.Base;

namespace AdManagerModel
{
	/// <summary>
	/// GenreModel에 대한 요약 설명입니다.
	/// </summary>
	public class GenreModel : BaseModel
	{

		// 조회용
		private string _SearchKey       = null;		// 검색어
		private string _SearchGenreLevel = null;		// 검색레벨

		// 상세정보용
		private string _MediaCode       = null;		// 사용자 아이디
        private string _CategoryCode    = null;
		private string _GenreCode       = null;		// 사용자 아이디
		private string _GenreName       = null;		// 사용자 명		
		private string _ModDt           = null;		// 사용자 직책직함

		// 목록조회용
		private DataSet  _UserDataSet;

		public GenreModel() : base () 
		{
			Init();
		}

		#region Public 메소드
		public void Init()
		{
			_SearchKey		= "";
			_SearchGenreLevel = "";

			_MediaCode			= "";
			_GenreCode			= "";
			_GenreName		= "";			
			_ModDt		= "";
			            
			_UserDataSet = new DataSet();
		}

		#endregion

		#region  프로퍼티 

		public DataSet UserDataSet
		{
			get { return _UserDataSet;	}
			set { _UserDataSet = value;	}
		}

		public string SearchKey 
		{
			get { return _SearchKey;	}
			set { _SearchKey = value;	}
		}
	
		public string SearchGenreLevel 
		{
			get { return _SearchGenreLevel;	}
			set { _SearchGenreLevel = value;	}
		}

		public string MediaCode 
		{
			get { return _MediaCode;		}
			set { _MediaCode = value;		}
		}

        public string CategoryCode
        {
            get
            {
                return _CategoryCode;
            }
            set
            {
                _CategoryCode = value;
            }
        }

		public string GenreCode 
		{
			get { return _GenreCode;		}
			set { _GenreCode = value;		}
		}

		public string GenreName 
		{
			get { return _GenreName;		}
			set { _GenreName = value;	}
		}
		
		public string ModDt 
		{
			get { return _ModDt;	}
			set { _ModDt = value;	}
		}
		
		#endregion

	}
}
