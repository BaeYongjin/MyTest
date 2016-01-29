using System;
using System.Data;
using WinFramework.Base;

namespace AdManagerModel
{
	/// <summary>
	/// 특정지정광고 데이터 모델
	/// </summary>
	public class SchDesignateModel : BaseModel
	{
		private	int		_Media;
		private	int		_Rep;
		private	int		_Category;
		private	int		_Genre;
		private	int		_Channel;
		private	int		_Series;
		private	int		_ItemNo;
		private	bool	_AdStat10;
		private	bool	_AdStat20;
		private	bool	_AdStat30;
		private	bool	_AdStat40;


		private	DataSet	_dsSchedule;

		private	DataSet	_dsItem;

		/// <summary>
		/// 특정 지정편성 데이터 모델
		/// </summary>
		public SchDesignateModel() : base()
		{
			Init();
		}

		#region Public 메소드
		public void Init()
		{
			_Media	= 0;
			_Rep	= 0;

			_ItemNo		= 0;
			_Category	= 0;
			_Genre		= 0;
			_Channel	= 0;
			_Series		= 0;

			_AdStat10	= false;
			_AdStat20	= false;
			_AdStat30	= false;
			_AdStat40	= false;

			_dsSchedule	= new DataSet();
			_dsItem		= new DataSet();
		}

		#endregion

		#region  프로퍼티 

		/// <summary>
		/// 미디어코드를 가져오거나 설정합니다
		/// </summary>
		public	int	Media
		{
			get	{	return	_Media;		}
			set	{	_Media	= value;	}
		}


		/// <summary>
		/// 미디어랩코드를 가져오거나 설정합니다
		/// </summary>
		public	int	MediaRep
		{
			get	{	return	_Rep;		}
			set	{	_Rep	= value;	}
		}


		/// <summary>
		/// 광고코드를 가져오거나 설정합니다
		/// </summary>
		public	int	ItemNo
		{
			get	{	return	_ItemNo;		}
			set	{	_ItemNo	= value;	}
		}

		/// <summary>
		/// 카테고리코드를 가져오거나 설정합니다
		/// </summary>
		public	int	Category
		{
			get	{	return	_Category;	}
			set	{	_Category	= value;	}
		}

		/// <summary>
		/// 장르코드를 가져오거나 설정합니다
		/// </summary>
		public	int	Genre
		{
			get	{	return	_Genre;		}
			set	{	_Genre	= value;	}
		}

		/// <summary>
		/// 채널코드를 가져오거나 설정합니다
		/// </summary>
		public	int	Channel
		{
			get	{	return	_Channel;		}
			set	{	_Channel	= value;	}
		}

		/// <summary>
		/// 시리즈번호를 가져오거나 설정합니다
		/// </summary>
		public	int	Series
		{
			get	{	return	_Series;		}
			set	{	_Series	= value;	}
		}

		/// <summary>
		/// 광고상태 대기상태(10)를 조회조건에 포함할 것인지를 설정하거나 가져옵니다
		/// </summary>
		public	bool AdState10
		{
			get	{	return	_AdStat10;		}
			set	{	_AdStat10	= value;	}
		}

		/// <summary>
		/// 광고상태 편성상태(20)를 조회조건에 포함할 것인지를 설정하거나 가져옵니다
		/// </summary>
		public	bool AdState20
		{
			get	{	return	_AdStat20;		}
			set	{	_AdStat20	= value;	}
		}

		/// <summary>
		/// 광고상태 중지상태(30)를 조회조건에 포함할 것인지를 설정하거나 가져옵니다
		/// </summary>
		public	bool AdState30
		{
			get	{	return	_AdStat30;		}
			set	{	_AdStat30	= value;	}
		}

		/// <summary>
		/// 광고상태 종료상태(40)를 조회조건에 포함할 것인지를 설정하거나 가져옵니다
		/// </summary>
		public	bool AdState40
		{
			get	{	return	_AdStat40;		}
			set	{	_AdStat40	= value;	}
		}

		/// <summary>
		/// 메뉴(장르) 데이터셋를 가져오거나 설정합니다
		/// </summary>
		public DataSet	DsSchedule
		{
			get
			{
				return _dsSchedule;
			}
			set
			{
				_dsSchedule	= value;
			}
		}

		public DataSet	DsItem
		{
			get
			{
				return _dsItem;
			}
			set
			{
				_dsItem	= value;
			}
		}
		#endregion
	}
}
