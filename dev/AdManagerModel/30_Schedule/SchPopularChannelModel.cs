
using System;
using System.Data;

using WinFramework.Base;

namespace AdManagerModel
{
	/// <summary>
	/// ����������� Ŭ���� ��.
	/// </summary>
	public class SchPopularChannelModel : BaseModel
	{

		// ��ȸ��
		private string _SearchMediaCode    = null;		// �˻���ü
		private string _SearchKey       = null;		// �˻���		
		private string _SearchType           = null;		// ��ȸ ���� B:���ñⰣ D:�ϰ� W:�ְ� M:����
		private string _SearchStartDay       = null;		// ��ȸ ������� ����
		private string _SearchEndDay         = null;		// ��ȸ ������� ����

		// ����ȸ��
		private string _MediaCode     = null;
		private string _CategoryCode  = null;
		private string _GenreCode     = null;
		private string _Channel     = null;
		private string _ChannelNo     = null;
		private string _ItemNo        = null;
		private string _ScheduleOrder = null;
		private string _ItemName      = null;
		private string _Comment      = null;
		private string _GenreName		= null;		//�帣��
		
		// �����ȸ��
		private DataSet  _SchPopularChannelDataSet;    

		private string _LastOrder     = null;

		// �̵�� �޺� ��ȸ��
			
		public SchPopularChannelModel() : base () 
		{
			Init();
		}

		#region Public �޼ҵ�
		public void Init()
		{			
			_SearchMediaCode = "";
			_SearchKey = "";			
			_SearchType            = "";
			_SearchStartDay          = "";
			_SearchEndDay          = "";

			_MediaCode       = "";
			_CategoryCode    = "";
			_GenreCode       = "";
			_Channel       = "";
			_ChannelNo       = "";
			_ItemNo          = "";
			_ScheduleOrder   = "";
			_ItemName        = "";
			_Comment        = "";
			_GenreName        = "";

			_SchPopularChannelDataSet = new DataSet();
			_LastOrder       = "";
		}

		#endregion

		#region  ������Ƽ 

		public DataSet SchPopularChannelDataSet
		{
			get { return _SchPopularChannelDataSet;	}
			set { _SchPopularChannelDataSet = value;	}
		}

		public string SearchMediaCode
		{
			get { return _SearchMediaCode;	}
			set { _SearchMediaCode = value;	}
		}

		public string SearchKey
		{
			get { return _SearchKey;	}
			set { _SearchKey = value;	}
		}
		
		public string SearchType 
		{
			get { return _SearchType;	}
			set { _SearchType = value;	}
		}

		public string SearchStartDay 
		{
			get { return _SearchStartDay;	}
			set { _SearchStartDay = value;	}
		}

		public string SearchEndDay 
		{
			get { return _SearchEndDay;	}
			set { _SearchEndDay = value;	}
		}

		public string ItemNo
		{
			get { return _ItemNo;	}
			set { _ItemNo = value;	}
		}

		public string MediaCode
		{
			get { return _MediaCode;	}
			set { _MediaCode = value;	}
		}

		public string CategoryCode
		{
			get { return _CategoryCode;	}
			set { _CategoryCode = value;	}
		}

		public string GenreCode
		{
			get { return _GenreCode;	}
			set { _GenreCode = value;	}
		}

		public string Channel
		{
			get { return _Channel;	}
			set { _Channel = value;	}
		}

		public string ChannelNo
		{
			get { return _ChannelNo;	}
			set { _ChannelNo = value;	}
		}

		public string ScheduleOrder
		{
			get { return _ScheduleOrder;	}
			set { _ScheduleOrder = value;	}
		}

		public string ItemName
		{
			get { return _ItemName;	}
			set { _ItemName = value;	}
		}

		public string Comment
		{
			get { return _Comment;	}
			set { _Comment = value;	}
		}

		public string GenreName
		{
			get { return _GenreName;	}
			set { _GenreName = value;	}
		}


		public string LastOrder
		{
			get { return _LastOrder;	}
			set { _LastOrder = value;	}
		}

		#endregion

	}
}
