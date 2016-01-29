using System;
using System.Data;
using WinFramework.Base;

namespace AdManagerModel
{
	/// <summary>
	/// Ư���������� ������ ��
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
		/// Ư�� ������ ������ ��
		/// </summary>
		public SchDesignateModel() : base()
		{
			Init();
		}

		#region Public �޼ҵ�
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

		#region  ������Ƽ 

		/// <summary>
		/// �̵���ڵ带 �������ų� �����մϴ�
		/// </summary>
		public	int	Media
		{
			get	{	return	_Media;		}
			set	{	_Media	= value;	}
		}


		/// <summary>
		/// �̵��ڵ带 �������ų� �����մϴ�
		/// </summary>
		public	int	MediaRep
		{
			get	{	return	_Rep;		}
			set	{	_Rep	= value;	}
		}


		/// <summary>
		/// �����ڵ带 �������ų� �����մϴ�
		/// </summary>
		public	int	ItemNo
		{
			get	{	return	_ItemNo;		}
			set	{	_ItemNo	= value;	}
		}

		/// <summary>
		/// ī�װ��ڵ带 �������ų� �����մϴ�
		/// </summary>
		public	int	Category
		{
			get	{	return	_Category;	}
			set	{	_Category	= value;	}
		}

		/// <summary>
		/// �帣�ڵ带 �������ų� �����մϴ�
		/// </summary>
		public	int	Genre
		{
			get	{	return	_Genre;		}
			set	{	_Genre	= value;	}
		}

		/// <summary>
		/// ä���ڵ带 �������ų� �����մϴ�
		/// </summary>
		public	int	Channel
		{
			get	{	return	_Channel;		}
			set	{	_Channel	= value;	}
		}

		/// <summary>
		/// �ø����ȣ�� �������ų� �����մϴ�
		/// </summary>
		public	int	Series
		{
			get	{	return	_Series;		}
			set	{	_Series	= value;	}
		}

		/// <summary>
		/// ������� ������(10)�� ��ȸ���ǿ� ������ �������� �����ϰų� �����ɴϴ�
		/// </summary>
		public	bool AdState10
		{
			get	{	return	_AdStat10;		}
			set	{	_AdStat10	= value;	}
		}

		/// <summary>
		/// ������� ������(20)�� ��ȸ���ǿ� ������ �������� �����ϰų� �����ɴϴ�
		/// </summary>
		public	bool AdState20
		{
			get	{	return	_AdStat20;		}
			set	{	_AdStat20	= value;	}
		}

		/// <summary>
		/// ������� ��������(30)�� ��ȸ���ǿ� ������ �������� �����ϰų� �����ɴϴ�
		/// </summary>
		public	bool AdState30
		{
			get	{	return	_AdStat30;		}
			set	{	_AdStat30	= value;	}
		}

		/// <summary>
		/// ������� �������(40)�� ��ȸ���ǿ� ������ �������� �����ϰų� �����ɴϴ�
		/// </summary>
		public	bool AdState40
		{
			get	{	return	_AdStat40;		}
			set	{	_AdStat40	= value;	}
		}

		/// <summary>
		/// �޴�(�帣) �����ͼ¸� �������ų� �����մϴ�
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
