using System;
using System.Collections;
using System.Data;

using WinFramework.Base;

namespace AdManagerModel
{
	/// <summary>
	/// AreaDayModel�� ���� ��� �����Դϴ�.
	/// </summary>
	[Serializable]
	public class AreaDayModel : BaseModel
	{	
		private string      _StartDay   = null;
		private string      _EndDay     = null;
		private string[]	_AdListStr	= null;
        private ArrayList   _AdList;
		private DataSet     _DataSet;

		public AreaDayModel() : base () 
		{
			Init();
		}

		#region Public �޼ҵ�
		public void Init()
		{
			_StartDay	=	"";
			_EndDay     =	"";
            _AdList     =	new ArrayList();
			_DataSet	=	new DataSet();
			_AdListStr	=	new string[0];
		}
		#endregion

		#region  ������Ƽ 

		public DataSet ReportDataSet
		{
			get { return _DataSet;	}
			set { _DataSet = value;	}
		}

        /// <summary>
        /// ���ñ��� ����Ʈ
        /// </summary>
        public ArrayList    AdList
        {
            get { return _AdList;	}
            set { _AdList = value;	}
        }


		public string StartDay
		{
			get { return _StartDay;	}
			set { _StartDay = value;	}
		}

		public string EndDay
		{
			get { return _EndDay;	}
			set { _EndDay = value;	}
		}

		public string[]	AdListStr
		{
			get { return _AdListStr;	}
			set { _AdListStr = value;	}
		}
		#endregion

	}
}