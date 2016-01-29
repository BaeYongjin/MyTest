using System;
using System.Data;
using WinFramework.Base;

namespace AdManagerModel
{
	/// <summary>
	/// [S1] ����� ����������� ��
	/// 1. �ش籤���� ������ �������� ����
	/// </summary>
	public class SchedulePerItemModel : BaseModel
	{
		#region [ �������� ]
		private	int		_SelectItemNo;
		private	int		_SelectMedia;
		private	int		_SelectCategory;
		private	int		_SelectGenre;
		private	int		_SelectChannel;
		private	int		_SelectSeries;
		private	TYPE_ScheduleDelete	_SelectTypeDeleteJob;
		#endregion

		#region [ ������ ]
		public SchedulePerItemModel() : base()	{	Init();	}
		#endregion

		#region Public �޼ҵ�
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

		#region  ������Ƽ 

		/// <summary>
		/// �����ȣ�� �������ų� �����մϴ�( �۾���� ������ )
		/// </summary>
		public	int	ItemNo
		{
			get	{	return	_SelectItemNo;		}
			set	{	_SelectItemNo	= value;	}
		}

		/// <summary>
		/// �̵���ڵ带 �������ų� �����մϴ�
		/// </summary>
		public	int	Media
		{
			get	{	return	_SelectMedia;		}
			set	{	_SelectMedia	= value;	}
		}

		/// <summary>
		/// ī�װ��ڵ带 �������ų� �����մϴ�
		/// </summary>
		public	int	Category
		{
			get	{	return	_SelectCategory;	}
			set	{	_SelectCategory	= value;	}
		}

		/// <summary>
		/// �帣�ڵ带 �������ų� �����մϴ�
		/// </summary>
		public	int	Genre
		{
			get	{	return	_SelectGenre;		}
			set	{	_SelectGenre	= value;	}
		}

		/// <summary>
		/// ä���ڵ带 �������ų� �����մϴ�
		/// </summary>
		public	int	Channel
		{
			get	{	return	_SelectChannel;		}
			set	{	_SelectChannel	= value;	}
		}

		/// <summary>
		/// ȸ���� �������ų� �����մϴ�
		/// </summary>
		public	int	Series
		{
			get	{	return	_SelectSeries;		}
			set	{	_SelectSeries	= value;	}
		}

		/// <summary>
		/// �����۾� ������ �������ų� �����մϴ�
		/// </summary>
		public	TYPE_ScheduleDelete DeleteJobType
		{
			get	{	return _SelectTypeDeleteJob;		}
			set	{	_SelectTypeDeleteJob	= value;	}
		}

		#endregion
	}
}
