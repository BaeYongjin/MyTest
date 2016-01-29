using System;
using System.Data;
using WinFramework.Base;

namespace AdManagerModel
{
	/// <summary>
	/// [S1] 광고당 스케쥴관리용 모델
	/// 1. 해당광고의 선택한 스케쥴을 삭제
	/// </summary>
	public class SchedulePerItemModel : BaseModel
	{
		#region [ 변수선언 ]
		private	int		_SelectItemNo;
		private	int		_SelectMedia;
		private	int		_SelectCategory;
		private	int		_SelectGenre;
		private	int		_SelectChannel;
		private	int		_SelectSeries;
		private	TYPE_ScheduleDelete	_SelectTypeDeleteJob;
		#endregion

		#region [ 생성자 ]
		public SchedulePerItemModel() : base()	{	Init();	}
		#endregion

		#region Public 메소드
		public void Init()
		{
			_SelectItemNo	=	0;
			_SelectMedia	=	0;
			_SelectCategory	=	0;
			_SelectGenre	=	0;
			_SelectChannel	=	0;
			_SelectSeries	=	0;
			_SelectTypeDeleteJob	= TYPE_ScheduleDelete.Series;
		}
		#endregion

		#region  프로퍼티 

		/// <summary>
		/// 광고번호를 가져오거나 설정합니다( 작업대상 광고임 )
		/// </summary>
		public	int	ItemNo
		{
			get	{	return	_SelectItemNo;		}
			set	{	_SelectItemNo	= value;	}
		}

		/// <summary>
		/// 미디어코드를 가져오거나 설정합니다
		/// </summary>
		public	int	Media
		{
			get	{	return	_SelectMedia;		}
			set	{	_SelectMedia	= value;	}
		}

		/// <summary>
		/// 카테고리코드를 가져오거나 설정합니다
		/// </summary>
		public	int	Category
		{
			get	{	return	_SelectCategory;	}
			set	{	_SelectCategory	= value;	}
		}

		/// <summary>
		/// 장르코드를 가져오거나 설정합니다
		/// </summary>
		public	int	Genre
		{
			get	{	return	_SelectGenre;		}
			set	{	_SelectGenre	= value;	}
		}

		/// <summary>
		/// 채널코드를 가져오거나 설정합니다
		/// </summary>
		public	int	Channel
		{
			get	{	return	_SelectChannel;		}
			set	{	_SelectChannel	= value;	}
		}

		/// <summary>
		/// 회차를 가져오거나 설정합니다
		/// </summary>
		public	int	Series
		{
			get	{	return	_SelectSeries;		}
			set	{	_SelectSeries	= value;	}
		}

		/// <summary>
		/// 삭제작업 형식을 가져오거나 설정합니다
		/// </summary>
		public	TYPE_ScheduleDelete DeleteJobType
		{
			get	{	return _SelectTypeDeleteJob;		}
			set	{	_SelectTypeDeleteJob	= value;	}
		}

		#endregion
	}
}
