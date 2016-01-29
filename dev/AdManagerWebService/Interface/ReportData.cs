using System;

namespace AdManagerWebService.Interface
{
    [Serializable]
    public class TestData
    {
        private int     _num    = 0;
        public  int Num
        {
            get
            {
                return _num;
            }
            set
            {
                _num = value;
            }
        }
        private string  _str    = "";
        public string STR
        {
            get
            {
                return _str;
            }
            set
            {
                _str = value;
            }
        }
    }
	
	[Serializable]
	public class ResultViewCntAdverEachProgram
	{
		private	bool	_success;
		public	bool	SUCCESS
		{
			get	{	return	_success;		}
			set {	_success	= value;	}
		}

		private	int		_resultCnt;
		public	int	ResultCnt
		{
			get	{	return _resultCnt;		}
			set {	_resultCnt	= value;	}
		}

		private	string	_message;
		public	string	Message
		{
			get	{	return	_message;	}
			set {	_message = value;	}
		}

		private	ViewCntAdverEachProgram[] _resultSet;
		public	ViewCntAdverEachProgram[] ResultSet
		{
			get	{	return	_resultSet;		}
			set {	_resultSet	= value;	}
		}
	}

    /// <summary>
    /// 프로그램별 광고시청 집계
    /// 데이터 구조
    /// </summary>
    [Serializable]
    public class ViewCntAdverEachProgram
    {
        private int     _categoryCode;
        public  int CategoryCode    
        {
            get {   return _categoryCode;       }
            set {   _categoryCode   = value;    }
        }

        private string  _categoryName;
        public  string  CategoryName
        {
            get {   return _categoryName;       }
            set {   _categoryName   = value;    }
        }

        private int     _genreCode;
        public  int GenreCode
        {
            get {   return  _genreCode;     }
            set {   _genreCode  = value;    }
        }

        private string  _genreName;
        public  string  GenreName
        {
            get {   return _genreName;      }
            set {   _genreName = value;     }
        }

        private int     _channelNo;
        public  int ChannelNo
        {
            get {   return  _channelNo;     }
			set {	_channelNo	= value;	}
        }

        private string  _programName;
        public  string  ProgramName
        {
            get	{	return  _programName;	}
			set {	_programName = value;	}
        }

        private long    _adCnt;
        public long AdViewCnt
        {
            get	{	return  _adCnt;			}
			set {	_adCnt	= value;		}
        }

        private long    _pgCnt;
        public  long    PgHitCnt
        {
            get	{	return _pgCnt;	}
			set {	_pgCnt = value;	}
        }

        public ViewCntAdverEachProgram(int categoryCode, string categoryName, int genreCode, string genreName, int channelNo, string programName, long adCnt, long pgCnt)
        {
            _categoryCode   = categoryCode;
            _categoryName   = categoryName;
            _genreCode      = genreCode;
            _genreName      = genreName;
            _channelNo      = channelNo;
            _programName    = programName;
            _adCnt          = adCnt;
            _pgCnt          = pgCnt;
        }

        public ViewCntAdverEachProgram()
        {
            _categoryCode   = 0;
            _categoryName   = "";
            _genreCode      = 0;
            _genreName      = "";
            _channelNo      = 0;
            _programName    = "";
            _adCnt          = 0;
            _pgCnt          = 0;
        }
    }
}
