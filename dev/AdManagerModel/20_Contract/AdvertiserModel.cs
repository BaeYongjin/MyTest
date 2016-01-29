/*
 * -------------------------------------------------------
 * [��������]
 * -------------------------------------------------------
 * �����ڵ�  : [A_01]
 * ������    : JH.Kim
 * ������    : 2015.11.
 * ��������  : ������� �߰�
 * --------------------------------------------------------
 */
using System;
using System.Data;

using WinFramework.Base;

namespace AdManagerModel
{
	/// <summary>
	/// AdvertiserModelModel�� ���� ��� �����Դϴ�.
	/// </summary>
	public class AdvertiserModel : BaseModel
	{

		// ��ȸ��
		private string _SearchKey               = null;		// �˻���
		private string _SearchAdvertiserLevel   = null;		// �˻�����
		private string _SearchchkAdState_10     = null;		// �˻� ��뿩�λ���
		private string _SearchRap               = null;		// �˻� �̵�

		// ��������
		private string _AdvertiserCode          = null;		// ������ ���̵�		
		private string _AdvertiserName          = null;		// �����ָ�	
		private string _RapCode	 		        = null;		// ����籸��
		private string _RapName			        = null;		// ����籸��
		private string _Comment                 = null;		// ���		
		private string _UseYn                   = null;		// ��뿩��		
		private string _RegDt                   = null;		// ���ʵ����		
		private string _ModDt                   = null;		// ����������
		private string _RegID                   = null;		// ����������
        private string _JobCode                 = null;     // �����ڵ�        [A_01]
        private string _JobNameLevel1           = null;     // Level1 ������   [A_01]
        private string _JobNameLevel2           = null;     // Level2 ������   [A_01]
        private string _JobLevel                = null;     // ���� ����       [A_01]
        private string _JobUpperCode            = null;     // ���������ڵ�    [A_01]

		// �����ȸ��
		private DataSet  _AdvertiserDataSet;

		public AdvertiserModel() : base () 
		{
			Init();
		}

		#region Public �޼ҵ�
		public void Init()
		{
			_SearchKey		        = "";
			_SearchAdvertiserLevel  = "";
			_SearchchkAdState_10    = "";
			_SearchRap              = "";

			_AdvertiserCode	= "";			
			_AdvertiserName	= "";		
			_RapCode		= "";		
			_RapName		= "";		
			_Comment		= "";
			_UseYn		    = "";
			_RegDt		    = "";
			_ModDt		    = "";
			_RegID		    = "";
			_JobCode        = "";
            _JobNameLevel1  = "";
            _JobNameLevel2  = "";
            _JobLevel       = "";
            _JobUpperCode   = "";
       
			_AdvertiserDataSet = new DataSet();
		}

		#endregion

		#region  ������Ƽ 

		public DataSet AdvertiserDataSet
		{
			get { return _AdvertiserDataSet;	}
			set { _AdvertiserDataSet = value;	}
		}

		public string SearchKey 
		{
			get { return _SearchKey;	}
			set { _SearchKey = value;	}
		}
	
		public string SearchAdvertiserLevel 
		{
			get { return _SearchAdvertiserLevel;	}
			set { _SearchAdvertiserLevel = value;	}
		}

		public string SearchchkAdState_10 
		{
			get { return _SearchchkAdState_10;	}
			set { _SearchchkAdState_10 = value;	}
		}

		public string SearchRap 
		{
			get { return _SearchRap;	}
			set { _SearchRap = value;	}
		}

		public string AdvertiserCode 
		{
			get { return _AdvertiserCode;	}
			set { _AdvertiserCode = value;	}
		}

		public string AdvertiserName 
		{
			get { return _AdvertiserName;	}
			set { _AdvertiserName = value;	}
		}

		public string RapCode 
		{
			get { return _RapCode;	}
			set { _RapCode = value;	}
		}

		public string RapName 
		{
			get { return _RapName;	}
			set { _RapName = value;	}
		}

		public string Comment 
		{
			get { return _Comment;	}
			set { _Comment = value;	}
		}

		public string UseYn
		{
			get { return _UseYn;	}
			set { _UseYn = value;	}
		}

		public string RegDt 
		{
			get { return _RegDt;	}
			set { _RegDt = value;	}
		}
		
		public string ModDt 
		{
			get { return _ModDt;	}
			set { _ModDt = value;	}
		}

		public string RegID 
		{
			get { return _RegID;	}
			set { _RegID = value;	}
		}

        /// <summary>
        /// �����ڵ�
        /// </summary>
        public string JobCode
        {
            get { return _JobCode;  }
            set { _JobCode = value; }
        }
        
        /// <summary>
        /// Level1 ������
        /// </summary>
        public string JobNameLevel1  
        {
            get { return _JobNameLevel1;  }
            set { _JobNameLevel1 = value; }
        }

        /// <summary>
        /// Level2 ������
        /// </summary>
        public string JobNameLevel2
        {
            get { return _JobNameLevel2; }
            set { _JobNameLevel2 = value; }
        }
        
        /// <summary>
        /// �����ڵ� ����
        /// </summary>
        public string JobLevel 
        {
            get { return _JobLevel;   }
            set { _JobLevel = value;  }
        }

        /// <summary>
        /// ���������ڵ�
        /// </summary>
        public string JobUpperCode
        {
            get { return _JobUpperCode; }
            set { _JobUpperCode = value; }
        }
		#endregion

	}
}