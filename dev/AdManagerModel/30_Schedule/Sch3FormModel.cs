using System;
using System.Data;
using WinFramework.Base;

namespace AdManagerModel
{
	/// <summary>
	/// Sch3FormModel�� ���� ��� �����Դϴ�.
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

		#region Public �޼ҵ�
		public void Init()
		{
			_dsGenre	= new DataSet();
			_dsChannel	= new DataSet();
			_dsSeries	= new DataSet();
		}

		#endregion

		#region  ������Ƽ 

		/// <summary>
		/// �����ڵ带 �������ų� �����մϴ�
		/// </summary>
		public	int	ItemNo
		{
			get	{	return	_SearchItemNo;		}
			set	{	_SearchItemNo	= value;	}
		}

		/// <summary>
		/// �̵���ڵ带 �������ų� �����մϴ�
		/// </summary>
		public	int	Media
		{
			get	{	return	_SearchMedia;		}
			set	{	_SearchMedia	= value;	}
		}

		/// <summary>
		/// ī�װ��ڵ带 �������ų� �����մϴ�
		/// </summary>
		public	int	Category
		{
			get	{	return	_SearchCategory;	}
			set	{	_SearchCategory	= value;	}
		}

		/// <summary>
		/// �帣�ڵ带 �������ų� �����մϴ�
		/// </summary>
		public	int	Genre
		{
			get	{	return	_SearchGenre;		}
			set	{	_SearchGenre	= value;	}
		}

		/// <summary>
		/// ä���ڵ带 �������ų� �����մϴ�
		/// </summary>
		public	int	Channel
		{
			get	{	return	_SearchChannel;		}
			set	{	_SearchChannel	= value;	}
		}

		/// <summary>
		/// ��ȸŸ���� �������ų� �����մϴ�.
		/// 00 : ��ü
		/// 10 : �������
		/// 20 : ��ü����
		/// 13 : CSS
		/// 77 : ��������
		/// </summary>
		public	int	DataType
		{
			get	{	return _SearchType; }
			set	{	_SearchType = value;}
		}

		/// <summary>
		/// �޴�(�帣) �����ͼ¸� �������ų� �����մϴ�
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
		/// ä�� �����ͼ¸� �������ų� �����մϴ�
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
		/// �ø������� �����ͼ¸� �������ų� �����մϴ�
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
