using System;
using System.Data;

using WinFramework.Base;

namespace AdManagerModel
{
	/// <summary>
	/// AdTypeMoniteringModel�� ���� ��� �����Դϴ�.
	/// </summary>
	public class AdTypeMoniteringModel : BaseModel
	{
		// ��ȸ��
		private string _LogDay	=   null;
		private string _AdType	=   null;
        private string _Rap     =   null;
		
		// �����ȸ��
		private DataSet  _DataSet;
		
		// ��������
		private DataSet _DataSet2;

		public AdTypeMoniteringModel() : base () 
		{
			Init();
		}

		#region Public �޼ҵ�
		public void Init()
		{
			_LogDay 	= "";
			_AdType 	= "";
            _Rap        = "";
			_DataSet  = new DataSet();
			_DataSet2 = new DataSet();			
		}

		#endregion

		#region  ������Ƽ 

		public DataSet ReportDataSet
		{
			get { return _DataSet;	}
			set { _DataSet = value;	}
		}

		public DataSet ItemDataSet
		{
			get { return _DataSet2;	}
			set { _DataSet2 = value;	}
		}

		public string LogDay 
		{
			get { return _LogDay;	}
			set { _LogDay = value;	}
		}

		public string AdType 
		{
			get { return _AdType;	}
			set { _AdType = value;	}
		}

        /// <summary>
        /// �̵��ڵ带 �����ϰų� �����ɴϴ�
        /// </summary>
        public string Rap
        {
            get { return _Rap;      }
            set { _Rap  = value;    }
        }
	
		#endregion

	}
}