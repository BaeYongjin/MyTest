using System;
using System.Data;

using WinFramework.Base;

namespace AdManagerModel
{
	/// <summary>
	/// MediaCodeModel�� ���� ��� �����Դϴ�.
	/// </summary>
	public class MediaCodeModel : BaseModel
	{
		private string _SearchKey       = null;		// �˻���

		// �����
		private string _MediaCode = null;				// �ڵ�
		private string _MediaCodeName  = null;			// �ڵ��


		// �����ȸ��
		private DataSet  _MediaCodeDataSet;
	
		public MediaCodeModel() : base()
		{
			Init();
		}

		#region Public �޼ҵ�
		public void Init()
		{			
			_SearchKey			= "";
			_MediaCode		= "";
			_MediaCodeName	= "";

			_MediaCodeDataSet = null;
		}

		#endregion

		#region  ������Ƽ 	

		public string SearchKey 
		{
			get { return _SearchKey;	}
			set { _SearchKey = value;	}
		}

		public string MediaCode 
		{
			get { return _MediaCode;  }
			set { _MediaCode = value; }
		}

		public string MediaCodeName 
		{
			get { return _MediaCodeName;  }
			set { _MediaCodeName = value; }
		}

		public DataSet MediaCodeDataSet
		{
			get { return _MediaCodeDataSet;	}
			set { _MediaCodeDataSet = value;	}
		}


		#endregion

	}
}
