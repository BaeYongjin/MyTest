using System;
using System.Data;

using WinFramework.Base;

namespace AdManagerModel
{
	/// <summary>
	/// SchPublishModel�� ���� ��� �����Դϴ�.
	/// </summary>
	public class SchPublishModel : BaseModel
	{

		// ��ȸ��
		private string _SearchKey              = null;		// �˻���
		private string _SearchMediaCode	       = null;		// �˻� ��ü		

		// ��������
		private string _AckNo	            = null;		// ������ι�ȣ
		private string _State	            = null;		// ������λ��� 10:����(��) 20:������(����) 30:��������(����)
		private string _AckDesc             = null;
		private string _ChkDesc             = null;
		private string _PublishDesc         = null;

 
		// �����ȸ��
		private DataSet  _ScheduleDataSet;
		private DataSet  _PublishDataSet;

		public SchPublishModel() : base () 
		{
			Init();
		}

		#region Public �޼ҵ�
		public void Init()
		{
			_SearchKey              = "";
			_SearchMediaCode	    = "";			

			_AckNo              = "";
			_State              = "";
			_AckDesc            = "";
			_ChkDesc            = "";
			_PublishDesc        = "";

			            
			_ScheduleDataSet = new DataSet();
			_PublishDataSet  = new DataSet();
		}

		#endregion

		#region  ������Ƽ 

		public DataSet PublishDataSet
		{
			get { return _PublishDataSet;	}
			set { _PublishDataSet = value;	}
		}

		public DataSet ScheduleDataSet
		{
			get { return _ScheduleDataSet;	}
			set { _ScheduleDataSet = value;	}
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
		
		public string AckNo 
		{
			get { return _AckNo;		}
			set { _AckNo = value;		}
		}
		
		public string State 
		{
			get { return _State;		}
			set { _State = value;		}
		}
		
		public string AckDesc 
		{
			get { return _AckDesc;		}
			set { _AckDesc = value;		}
		}

		public string ChkDesc 
		{
			get { return _ChkDesc;		}
			set { _ChkDesc = value;		}
		}
		
		public string PublishDesc 
		{
			get { return _PublishDesc;		}
			set { _PublishDesc = value;		}
		}


		#endregion

	}
}