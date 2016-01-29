using System;

namespace AdManagerClient.Common
{
    /// <summary>
    /// 카테고리 장르 선택 이벤트 아큐먼트
    /// </summary>
    public class CategoryGenreEventArgs : EventArgs
    {
        private int _categoryCode   = 0;
        /// <summary>
        /// 카테고리코드를 설정하거나 가져옵니다
        /// </summary>
        public  int Category
        {
            get
            {
                return _categoryCode;
            }
            set
            {
                _categoryCode = value;
            }
        }


        private int _genreCode      = 0;
        /// <summary>
        /// 장르코드를 설정하거나 가져옵니다
        /// </summary>
        public  int Genre
        {
            get
            {
                return _genreCode;
            }
            set
            {
                _genreCode = value;
            }
        }


        private string _categoryName = "";
        /// <summary>
        /// 카테고리명를 설정하거나 가져옵니다
        /// </summary>
        public  string CategoryName
        {
            get
            {
                return _categoryName;
            }
            set
            {
                _categoryName = value;
            }
        }


        private string _genreName = "";
        /// <summary>
        /// 장르명를 설정하거나 가져옵니다
        /// </summary>
        public  string GenreName
        {
            get
            {
                return _genreName;
            }
            set
            {
                _genreName = value;
            }
        }
    

        /// <summary>
        /// 카테고리장르선택 아규먼트 생성
        /// </summary>
        /// <param name="category"></param>
        /// <param name="genre"></param>
        public CategoryGenreEventArgs( int category, int genre , string categoryName, string genreName )
        {
            _categoryCode   = category;
            _genreCode      = genre;
            _categoryName   = categoryName;
            _genreName      = genreName;
        }

        public CategoryGenreEventArgs()
        {
            _categoryCode   = 0;
            _genreCode      = 0;
            _categoryName   = "";
            _genreName      = "";
        }
    }


    /// <summary>
    /// 카테고리 장르 선택 대리자
    /// </summary>
    public  delegate void CategoryGenreEventHandler( object sender, CategoryGenreEventArgs e);

    /// <summary>
    /// 컨텐츠 선택 이벤트 Args
    /// </summary>
    public class ContentsEventArgs : EventArgs
    {
        private int     _categoryCode   = 0;
        public  int     CategoryCode
        {
            get {   return  _categoryCode;      }
            set {   _categoryCode   = value;    }
        }

        private string  _categoryName   = "";
        public  string  CategoryName
        {
            get {   return  _categoryName;      }
            set {   _categoryName   = value;    }
        }

        private int     _genreCode      = 0;
        public  int     GenreCode
        {
            get {   return  _genreCode;     }
            set {   _genreCode  = value;    }
        }

        private string  _genreName      = "";
        public  string  GenreName
        {
            get {   return  _genreName; }
            set {   _genreName = value; }
        }
        
        private int     _channelNo      = 0;
        public  int     ChannelNo
        {
            get {   return  _channelNo; }
            set {   _channelNo = value; }
        }

        private int     _seriesNo       = 0;
        public  int     SeriesNo
        {
            get {   return  _seriesNo; }
            set {   _seriesNo = value; }
        }
        
        private string  _title          = "";
        public  string  Title
        {
            get {   return  _title; }
            set {   _title = value; }
        }

        private string  _subTitle       = "";
        public  string  SubTitle
        {
            get {   return  _subTitle; }
            set {   _subTitle = value; }
        }

        private string  _contentId      = "";
        public  string  ContentId
        {
            get {   return  _contentId; }
            set {   _contentId = value; }
        }

        public ContentsEventArgs()
        {
            _categoryCode   = 0;
            _categoryName   = "";
            _genreCode      = 0;
            _genreName      = "";
            _channelNo      = 0;
            _seriesNo       = 0;
            _title          = "";
            _subTitle       = "";
            _contentId      = "";
        }
        

        public ContentsEventArgs(       int     categoryCode
                                    ,   string  categoryName
                                    ,   int     genreCode
                                    ,   string  genreName
                                    ,   int     channelNo
                                    ,   int     seriesNo
                                    ,   string  title
                                    ,   string  subTitle
                                    ,   string  contentId)
        {
            _categoryCode   = categoryCode;
            _categoryName   = categoryName;
            _genreCode      = genreCode;
            _genreName      = genreName;
            _channelNo      = channelNo;
            _seriesNo       = seriesNo;
            _title          = title;
            _subTitle       = subTitle;
            _contentId      = contentId;
        }

    
    }

    /// <summary>
    /// 컨텐츠선택 대리자
    /// </summary>
    public delegate void ContentsEventHandler( object sender, ContentsEventArgs e);

	/// <summary>
	/// 컨텐츠 선택 이벤트 Args
	/// </summary>
	public class ZipCodeEventArgs : EventArgs
	{
		private string  _zipCode   = "";
		public  string  ZipCode
		{
			get {   return  _zipCode;      }
			set {   _zipCode   = value;    }
		}


		private string  _zipAddr      = "";
		public  string  ZipAddr
		{
			get {   return  _zipAddr; }
			set {   _zipAddr = value; }
		}
        

		public ZipCodeEventArgs()
		{
			_zipCode   = "";
			_zipAddr   = "";
		}
        

		public ZipCodeEventArgs(string  zipCode, string zipAddr )
		{
			_zipCode   = zipCode;
			_zipAddr   = zipAddr;
		}

    
	}

	public class DateUtil 
	{
		/// <summary>
		/// 월요일을 주초로 볼 때 해당하는 주의 첫 일자를 가지고 온다.
		/// </summary>
		/// <param name="date"></param>
		/// <returns></returns>
		public static DateTime getFirstDayOfWeekForStartMonday(DateTime date)
		{
			//월요일이 처음 시작으로 보기 때문에 빼야하는 날짜를 이렇게 산술적으로 구한다.
			int dof = (1 - (int)date.DayOfWeek);
			if(dof == 1)
			{
				dof = -6;
			}
			return date.AddDays(dof);
		}

		/// <summary>
		/// 월요일을 주초로 볼 때 해당하는 주의 마지막일자를 가지고 온다.
		/// </summary>
		/// <param name="date"></param>
		/// <returns></returns>
		public static DateTime getLastDayOfWeekForStartMonday(DateTime date)
		{
			//월요일이 처음 시작으로 보기 때문에 더해야하는 날짜를 이렇게 산술적으로 구한다.
			return date.AddDays((7 - (int)date.DayOfWeek) % 7);
		}

		/// <summary>
		/// 월말을 리턴한다.
		/// </summary>
		/// <param name="date"></param>
		/// <returns></returns>
		public static DateTime getLastDayOfMonth(DateTime date)
		{
			date = date.AddMonths(1);
			return date.AddDays(-1 * date.Day);
		}

		/// <summary>
		/// 월초를 리턴한다.
		/// </summary>
		/// <param name="date"></param>
		/// <returns></returns>
		public static DateTime getFirstDayOfMonth(DateTime date)
		{
			return date.AddDays(-1 * date.Day + 1);
		}

	}

	/// <summary>
	/// 컨텐츠선택 대리자
	/// </summary>
	public delegate void ZipCodeEventHandler( object sender, ZipCodeEventArgs e);
}

