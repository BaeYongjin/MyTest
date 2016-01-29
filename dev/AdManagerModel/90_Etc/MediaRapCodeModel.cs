using System;
using System.Data;

using WinFramework.Base;

namespace AdManagerModel
{
	/// <summary>
	/// MediaRapCodeModel�� ���� ��� �����Դϴ�.
	/// </summary>
	public class MediaRapCodeModel : BaseModel
	{
		private string _SearchKey       = null;		// �˻���
		
		// �����
		private string _RapCode = null;				// �ڵ�
		private string _RapName  = null;			// �ڵ��


		// �����ȸ��
		private DataSet  _MediaRapCodeDataSet;
	
		public MediaRapCodeModel() : base()
		{
			Init();
		}

		#region Public �޼ҵ�
		public void Init()
		{			
			_SearchKey			= "";
			_RapCode		= "";
			_RapName	= "";

			_MediaRapCodeDataSet = null;
		}

		#endregion

		#region  ������Ƽ 	

		public string SearchKey 
		{
			get { return _SearchKey;	}
			set { _SearchKey = value;	}
		}

		public string RapCode 
		{
			get { return _RapCode;  }
			set { _RapCode = value; }
		}

		public string RapName 
		{
			get { return _RapName;  }
			set { _RapName = value; }
		}

		public DataSet MediaRapCodeDataSet
		{
			get { return _MediaRapCodeDataSet;	}
			set { _MediaRapCodeDataSet = value;	}
		}


		#endregion

	}
}
