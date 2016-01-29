using System;
using System.Data;

using WinFramework.Base;

namespace AdManagerModel
{
	/// <summary>
	/// ZipCodeModel에 대한 요약 설명입니다.
	/// </summary>
	public class ZipCodeModel : BaseModel
	{
        private string	_SearchKey		= null;		// 사용자구분코드
		private string  _SearchZip     = null;

		private DataSet _DataSet;
	
		public ZipCodeModel() : base()
		{
			Init();
		}

		#region Public 메소드
		public void Init()
		{
			_SearchKey		= "";
			_DataSet		= new DataSet();
		}

		#endregion

		#region  프로퍼티 	

		/// <summary>
		/// 우편번호 검색명을 가져오거나 설정합니다
		/// </summary>
		public string SearchKey
		{
			get { return _SearchKey;  }
			set { _SearchKey = value; }
		}
		/// <summary>
		/// 우편번호 검색
		/// </summary>
		public string SearchZip
		{
			get{ return _SearchZip;}
			set{ _SearchZip = value;}
		}

		/// <summary>
		/// 검색결과 DataSet를 가져오거나 설정합니다
		/// </summary>
        public DataSet DsAddr
        {
            get { return _DataSet;	}
            set { _DataSet = value;	}
        }

		#endregion

	}
}