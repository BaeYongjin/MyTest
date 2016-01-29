using System;
using System.Data;

using WinFramework.Base;

namespace AdManagerModel
{
	/// <summary>
	/// MediaCodeModel에 대한 요약 설명입니다.
	/// </summary>
	public class MediaCodeModel : BaseModel
	{
		private string _SearchKey       = null;		// 검색어

		// 결과용
		private string _MediaCode = null;				// 코드
		private string _MediaCodeName  = null;			// 코드명


		// 목록조회용
		private DataSet  _MediaCodeDataSet;
	
		public MediaCodeModel() : base()
		{
			Init();
		}

		#region Public 메소드
		public void Init()
		{			
			_SearchKey			= "";
			_MediaCode		= "";
			_MediaCodeName	= "";

			_MediaCodeDataSet = null;
		}

		#endregion

		#region  프로퍼티 	

		public string SearchKey 
		{
			get { return _SearchKey;	}
			set { _SearchKey = value;	}
		}

		public string MediaCode 
		{
			get { return _MediaCode;  }
			set { _MediaCode = value; }
		}

		public string MediaCodeName 
		{
			get { return _MediaCodeName;  }
			set { _MediaCodeName = value; }
		}

		public DataSet MediaCodeDataSet
		{
			get { return _MediaCodeDataSet;	}
			set { _MediaCodeDataSet = value;	}
		}


		#endregion

	}
}
