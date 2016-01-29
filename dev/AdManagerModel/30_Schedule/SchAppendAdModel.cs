using System;
using System.Data;

using WinFramework.Base;

namespace AdManagerModel
{
	/// <summary>
	/// SchAppendAdModel�� ���� ��� �����Դϴ�.
	/// </summary>
	public class SchAppendAdModel : BaseModel
	{

		// ��ȸ��
		private string _SearchKey            = null;		// �˻���
		private string _SearchMediaCode	     = null;		// �˻� ��ü
		private string _SearchRapCode        = null;		// �˻� ��
		private string _SearchAgencyCode     = null;		// �˻� �����
		private string _SearchAdvertiserCode = null;		// �˻� ������
		private string _SearchContractState  = null;		// �˻� ������
		private string _SearchAdClass        = null;		// �˻� ����뵵
		private string _SearchchkAdState_10    = null;		// �˻� ������� : �غ�
		private string _SearchchkAdState_20    = null;		// �˻� ������� : ��
		private string _SearchchkAdState_30    = null;		// �˻� ������� : ����
		private string _SearchchkAdState_40    = null;		// �˻� ������� : ����


		// ��������		
		private string _MediaCode           = null;		// ��ü�ڵ�
		private string _ScheduleOrder		= null;		// ������
		private string _ItemNo				= null;		// ��೻��Key
		private string _ItemName			= null;		// �����

		// �����ȸ��
		private DataSet  _DataSet;

		// ������ ����
		private string _LastOrder     = null;

		// ���ϰǼ��˻��
		private int _FileListCount = 0;

		public SchAppendAdModel() : base () 
		{
			Init();
		}

		#region Public �޼ҵ�
		public void Init()
		{
			_SearchKey		       = "";
			_SearchMediaCode 	   = "";
			_SearchRapCode         = "";
			_SearchAgencyCode      = "";
			_SearchAdvertiserCode  = "";
			_SearchContractState   = "";
			_SearchAdClass         = "";
			_SearchchkAdState_20   = "";
			_SearchchkAdState_30   = "";
			_SearchchkAdState_40   = "";
			
			_MediaCode   		   = "";
			_ScheduleOrder		   = "";
			_ItemNo				   = "";
			_ItemName              = "";

			_LastOrder		   = "";

			_DataSet = new DataSet();

			_FileListCount = 0;
		}

		#endregion

		#region  ������Ƽ 

		public int FileListCount
		{
			get { return _FileListCount;	}
			set { _FileListCount = value;	}
		}

		public DataSet ScheduleDataSet
		{
			get { return _DataSet;	}
			set { _DataSet = value;	}
		}

		public string SearchKey 
		{
			get { return _SearchKey;	}
			set { _SearchKey = value;	}
		}
	
		public string SearchMediaCode 
		{
			get { return _SearchMediaCode;	}
			set { _SearchMediaCode = value;	}
		}

		public string SearchRapCode 
		{
			get { return _SearchRapCode;		}
			set { _SearchRapCode = value;		}
		}

		public string SearchAgencyCode 
		{
			get { return _SearchAgencyCode;		}
			set { _SearchAgencyCode = value;	}
		}

		public string SearchAdvertiserCode 
		{
			get { return _SearchAdvertiserCode;		}
			set { _SearchAdvertiserCode = value;	}
		}

		public string SearchContractState 
		{
			get { return _SearchContractState;		}
			set { _SearchContractState = value;		}
		}

		public string SearchAdClass 
		{
			get { return _SearchAdClass;		}
			set { _SearchAdClass = value;		}
		}
		
		public string SearchchkAdState_10 
		{
			get { return _SearchchkAdState_10;		}
			set { _SearchchkAdState_10 = value;		}
		}

		public string SearchchkAdState_20 
		{
			get { return _SearchchkAdState_20;		}
			set { _SearchchkAdState_20 = value;		}
		}
		public string SearchchkAdState_30 
		{
			get { return _SearchchkAdState_30;		}
			set { _SearchchkAdState_30 = value;		}
		}
		public string SearchchkAdState_40 
		{
			get { return _SearchchkAdState_40;		}
			set { _SearchchkAdState_40 = value;		}
		}

		public string MediaCode
		{
			get { return _MediaCode;		}
			set { _MediaCode = value;		}
		}

		public string ScheduleOrder
		{
			get { return _ScheduleOrder;		}
			set { _ScheduleOrder = value;		}
		}

		public string ItemNo
		{
			get { return _ItemNo;	}
			set { _ItemNo = value;	}
		}

		public string ItemName
		{
			get { return _ItemName;	}
			set { _ItemName = value;	}
		}

		public string LastOrder
		{
			get { return _LastOrder;		}
			set { _LastOrder = value;		}
		}

		#endregion

	}
}