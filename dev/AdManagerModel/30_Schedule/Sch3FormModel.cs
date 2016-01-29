using System;
using System.Data;
using WinFramework.Base;

namespace AdManagerModel
{
	/// <summary>
	/// Sch3FormModel에 대한 요약 설명입니다.
	/// </summary>
	public class Sch3FormModel : BaseModel
	{
		private	int		_SearchMedia;
		private	int		_SearchCategory;
		private	int		_SearchGenre;
		private	int		_SearchChannel;
		private	int		_SearchItemNo;
		private	int		_SearchType;

		private	DataSet	_dsGenre;
		private	DataSet	_dsChannel;
		private	DataSet	_dsSeries;

		public Sch3FormModel() : base()
		{
			Init();
		}

		#region Public 메소드
		public void Init()
		{
			_dsGenre	= new DataSet();
			_dsChannel	= new DataSet();
			_dsSeries	= new DataSet();
		}

		#endregion

		#region  프로퍼티 

		/// <summary>
		/// 광고코드를 가져오거나 설정합니다
		/// </summary>
		public	int	ItemNo
		{
			get	{	return	_SearchItemNo;		}
			set	{	_SearchItemNo	= value;	}
		}

		/// <summary>
		/// 미디어코드를 가져오거나 설정합니다
		/// </summary>
		public	int	Media
		{
			get	{	return	_SearchMedia;		}
			set	{	_SearchMedia	= value;	}
		}

		/// <summary>
		/// 카테고리코드를 가져오거나 설정합니다
		/// </summary>
		public	int	Category
		{
			get	{	return	_SearchCategory;	}
			set	{	_SearchCategory	= value;	}
		}

		/// <summary>
		/// 장르코드를 가져오거나 설정합니다
		/// </summary>
		public	int	Genre
		{
			get	{	return	_SearchGenre;		}
			set	{	_SearchGenre	= value;	}
		}

		/// <summary>
		/// 채널코드를 가져오거나 설정합니다
		/// </summary>
		public	int	Channel
		{
			get	{	return	_SearchChannel;		}
			set	{	_SearchChannel	= value;	}
		}

		/// <summary>
		/// 조회타입을 가져오거나 설정합니다.
		/// 00 : 전체
		/// 10 : 상업광고만
		/// 20 : 매체광고만
		/// 13 : CSS
		/// 77 : 지정광고
		/// </summary>
		public	int	DataType
		{
			get	{	return _SearchType; }
			set	{	_SearchType = value;}
		}

		/// <summary>
		/// 메뉴(장르) 데이터셋를 가져오거나 설정합니다
		/// </summary>
		public DataSet	DsGenre
		{
			get
			{
				return _dsGenre;
			}
			set
			{
				_dsGenre	= value;
			}
		}


		/// <summary>
		/// 채널 데이터셋를 가져오거나 설정합니다
		/// </summary>
		public DataSet	DsChannel
		{
			get
			{
				return _dsChannel;
			}
			set
			{
				_dsChannel	= value;
			}
		}


		/// <summary>
		/// 시리즈정보 데이터셋를 가져오거나 설정합니다
		/// </summary>
		public DataSet	DsSeries
		{
			get
			{
				return _dsSeries;
			}
			set
			{
				_dsSeries	= value;
			}
		}
		#endregion

	}
}
