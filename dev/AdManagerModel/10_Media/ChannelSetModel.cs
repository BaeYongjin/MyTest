
using System;
using System.Data;

using WinFramework.Base;

namespace AdManagerModel
{
	/// <summary>
	/// ����������� Ŭ���� ��.
	/// </summary>
	public class ChannelSetModel : BaseModel
	{

		// ��ȸ��
		private string _SearchKey       = null;		// �˻���
		private string _SearchMediaName = null;		// �˻�����
		private string _SearchCategoryName = null;		// �˻�����
		private string _SearchGenreName = null;		// �˻�����
		
		// ��������
		private string _MediaCode       = null;		// ��ü�ڵ�		
		private string _MediaCode_P       = null;		// ��ü�ڵ�		
		private string _MediaCode_old       = null;		// ��ü�ڵ�		
		private string _MediaName       = null;		// ��ü��Ī
		private string _CategoryCode       = null;		// ��ü�ڵ�
		private string _CategoryCode_P       = null;		// ��ü�ڵ�
		private string _CategoryCode_old       = null;		// ��ü�ڵ�
		private string _CategoryName       = null;		// ��ü�ڵ�
		private string _ChannelNo       = null;		// ������ ���
		private string _ChannelNo_old       = null;		// ������ ���
		private string _Title           = null;		// ��������
		private string _SeriesNo           = null;		// ��������
		private string _ContentId           = null;		// ��������
		private string _TotalSeries           = null;		// ��������		
		private string _GenreCode       = null;		// ��ü�ڵ�		
		private string _GenreCode_P       = null;		// ��ü�ڵ�		
		private string _GenreCode_old       = null;		// ��ü�ڵ�		
		private string _GenreName       = null;		// ��ü�ڵ�		
		private string _ModDt           = null;		// ���������Ͻ�
		private string _CheckYn           = null;		// ���������Ͻ�
			
		// �����ȸ��
		private DataSet  _ChannelSetDataSet;
		private DataSet  _CategoryDataSet;
		private DataSet  _GenreDataSet;

		// �̵�� �޺� ��ȸ��
			
		public ChannelSetModel() : base () 
		{
			Init();
		}

		#region Public �޼ҵ�
		public void Init()
		{			
			_SearchKey		= "";
			_SearchMediaName		= "";
			_SearchCategoryName		= "";
			_SearchGenreName		= "";

			_MediaCode       = "";		// ��ü�ڵ�
			_MediaCode_P       = "";		// ��ü�ڵ�
			_MediaCode_old       = "";		// ��ü�ڵ�
			_MediaName       = "";
			_CategoryCode       = "";		// ��ü�ڵ�
			_CategoryCode_P       = "";		// ��ü�ڵ�
			_CategoryCode_old       = "";		// ��ü�ڵ�
			_CategoryName       = "";		// ��ü�ڵ�
			_ChannelNo       = "";		// ä���ڵ�
			_ChannelNo_old       = "";		// ä���ڵ�
			_Title           = "";      // ��������
			_SeriesNo        = "";		// �ø��� ��ȣ
			_ContentId       = "";		// ������ ���̵�
			_TotalSeries	 = "";		// �ø��� ���
			_GenreCode       = "";		// ��ü�ڵ�
			_GenreCode_P       = "";		// ��ü�ڵ�
			_GenreCode_old       = "";		// ��ü�ڵ�
			_GenreName       = "";		// ��ü�ڵ�
			_ModDt           = "";		// �������
			_CheckYn         = "";
	

			_ChannelSetDataSet = new DataSet();
			_CategoryDataSet   = new DataSet();
			_GenreDataSet   = new DataSet();
		}

		#endregion

		#region  ������Ƽ 

		public DataSet ChannelSetDataSet
		{
			get { return _ChannelSetDataSet;	}
			set { _ChannelSetDataSet = value;	}
		}
		public DataSet CategoryDataSet
		{
			get { return _CategoryDataSet;	}
			set { _CategoryDataSet = value;	}
		}

		public DataSet GenreDataSet
		{
			get { return _GenreDataSet;	}
			set { _GenreDataSet = value;	}
		}

		public string SearchKey 
		{
			get { return _SearchKey;	}
			set { _SearchKey = value;	}
		}

		public string SearchMediaName 
		{
			get { return _SearchMediaName;	}
			set { _SearchMediaName = value;	}
		}

		public string SearchCategoryName 
		{
			get { return _SearchCategoryName;	}
			set { _SearchCategoryName = value;	}
		}

		public string SearchGenreName 
		{
			get { return _SearchGenreName;	}
			set { _SearchGenreName = value;	}
		}
	

		public string ContentId 
		{
			get { return _ContentId;		}
			set { _ContentId = value;		}
		}

		public string MediaCode
		{
			get { return _MediaCode;		}
			set { _MediaCode= value;	}
		}

		public string MediaCode_P
		{
			get { return _MediaCode_P;		}
			set { _MediaCode_P= value;	}
		}

		public string MediaCode_old
		{
			get { return _MediaCode_old;		}
			set { _MediaCode_old= value;	}
		}

		public string MediaName
		{
			get { return _MediaName;		}
			set { _MediaName= value;	}
		}

		public string CategoryCode
		{
			get { return _CategoryCode;		}
			set { _CategoryCode= value;	}
		}

		public string CategoryCode_P
		{
			get { return _CategoryCode_P;		}
			set { _CategoryCode_P= value;	}
		}

		public string CategoryCode_old
		{
			get { return _CategoryCode_old;		}
			set { _CategoryCode_old= value;	}
		}

		public string CategoryName
		{
			get { return _CategoryName;		}
			set { _CategoryName= value;	}
		}

		public string ChannelNo
		{
			get { return _ChannelNo; }
			set { _ChannelNo = value;}
		}
        
		public string ChannelNo_old
		{
			get { return _ChannelNo_old; }
			set { _ChannelNo_old = value;}
		}

		public string Title
		{
			get { return _Title; }
			set { _Title = value;}
		}

		public string SeriesNo 
		{
			get { return _SeriesNo;	}
			set { _SeriesNo = value;	}
		}

		public string GenreCode
		{
			get { return _GenreCode;		}
			set { _GenreCode= value;		}
		}

		public string GenreCode_P
		{
			get { return _GenreCode_P;		}
			set { _GenreCode_P= value;		}
		}

		public string GenreCode_old
		{
			get { return _GenreCode_old;		}
			set { _GenreCode_old= value;		}
		}

		public string GenreName
		{
			get { return _GenreName;		}
			set { _GenreName= value;		}
		}

		public string ModDt
		{
			get { return _ModDt;		}
			set { _ModDt= value;		}
		}

		public string TotalSeries 
		{
			get { return _TotalSeries;		}
			set { _TotalSeries = value;		}
		}

		public string CheckYn 
		{
			get { return _CheckYn;		}
			set { _CheckYn = value;		}
		}

        

		#endregion

	}
}
