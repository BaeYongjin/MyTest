using System;

namespace AdManagerClient.Common
{
    /// <summary>
    /// ī�װ� �帣 ���� �̺�Ʈ ��ť��Ʈ
    /// </summary>
    public class CategoryGenreEventArgs : EventArgs
    {
        private int _categoryCode   = 0;
        /// <summary>
        /// ī�װ��ڵ带 �����ϰų� �����ɴϴ�
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
        /// �帣�ڵ带 �����ϰų� �����ɴϴ�
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
        /// ī�װ��� �����ϰų� �����ɴϴ�
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
        /// �帣�� �����ϰų� �����ɴϴ�
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
        /// ī�װ��帣���� �ƱԸ�Ʈ ����
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
    /// ī�װ� �帣 ���� �븮��
    /// </summary>
    public  delegate void CategoryGenreEventHandler( object sender, CategoryGenreEventArgs e);

    /// <summary>
    /// ������ ���� �̺�Ʈ Args
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
    /// ���������� �븮��
    /// </summary>
    public delegate void ContentsEventHandler( object sender, ContentsEventArgs e);

	/// <summary>
	/// ������ ���� �̺�Ʈ Args
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
		/// �������� ���ʷ� �� �� �ش��ϴ� ���� ù ���ڸ� ������ �´�.
		/// </summary>
		/// <param name="date"></param>
		/// <returns></returns>
		public static DateTime getFirstDayOfWeekForStartMonday(DateTime date)
		{
			//�������� ó�� �������� ���� ������ �����ϴ� ��¥�� �̷��� ��������� ���Ѵ�.
			int dof = (1 - (int)date.DayOfWeek);
			if(dof == 1)
			{
				dof = -6;
			}
			return date.AddDays(dof);
		}

		/// <summary>
		/// �������� ���ʷ� �� �� �ش��ϴ� ���� ���������ڸ� ������ �´�.
		/// </summary>
		/// <param name="date"></param>
		/// <returns></returns>
		public static DateTime getLastDayOfWeekForStartMonday(DateTime date)
		{
			//�������� ó�� �������� ���� ������ ���ؾ��ϴ� ��¥�� �̷��� ��������� ���Ѵ�.
			return date.AddDays((7 - (int)date.DayOfWeek) % 7);
		}

		/// <summary>
		/// ������ �����Ѵ�.
		/// </summary>
		/// <param name="date"></param>
		/// <returns></returns>
		public static DateTime getLastDayOfMonth(DateTime date)
		{
			date = date.AddMonths(1);
			return date.AddDays(-1 * date.Day);
		}

		/// <summary>
		/// ���ʸ� �����Ѵ�.
		/// </summary>
		/// <param name="date"></param>
		/// <returns></returns>
		public static DateTime getFirstDayOfMonth(DateTime date)
		{
			return date.AddDays(-1 * date.Day + 1);
		}

	}

	/// <summary>
	/// ���������� �븮��
	/// </summary>
	public delegate void ZipCodeEventHandler( object sender, ZipCodeEventArgs e);
}

