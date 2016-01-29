/*
 * -------------------------------------------------------
 * �����ڵ�  : [E_01]
 * ������    : YJ.PARK
 * ������    : 2014.08.28
 * ��������  :        
 *            - Ȩ ������� �� ���� �߰�
 * -------------------------------------------------------
 */
using System;
using System.Data;

using WinFramework.Base;

namespace AdManagerModel
{
	/// <summary>
	/// Ȩ������� ������ Model
	/// </summary>
	public class SchHomeCmModel : BaseModel
	{
		#region [�Ӽ�] ��ȸ����
		private	int		_SearchMedia= 0;
		/// <summary>
		/// ��ȸ������ �̵���ڵ带 �������ų� �����մϴ�
		/// </summary>
		public int	SearchMediaCode 
		{
			get { return _SearchMedia;	}
			set { _SearchMedia = value;	}
		}
		#endregion

		#region [�Ӽ�] �����ȸ�� DataSet
		private DataSet  _DataSet;
		/// <summary>
		/// ��ȸ �����ͼ��� �������ų� �����մϴ�
		/// </summary>
		public DataSet ScheduleDataSet
		{
			get { return _DataSet;	}
			set { _DataSet = value;	}
		}
		#endregion

		#region [�Ӽ�] ������ �Է�/����/������
		#region [�Ӽ�] �����ȣ
		private	int		_ItemNo		= 0;
		/// <summary>
		/// �����ȣ�� �����ϰų� �����ɴϴ�
		/// </summary>
		public int ItemNo
		{
			get {	return _ItemNo;		}
			set {	_ItemNo	= value;	}
			
		}
		#endregion

		#region [�Ӽ�] �����
		private	string	_ItemName	= "";
		/// <summary>
		/// �����Ī�� �������ų� �����մϴ�
		/// </summary>
		public	string	ItemName
		{
			get {	return _ItemName;	}
			set {	_ItemName = value;	}
		}
		#endregion

        #region [�Ӽ�] ���Թ�ȣ [E_01]
        private int _SlotNo = 0;
        /// <summary>
        /// ���Թ�ȣ�� �����ϰų� �����ɴϴ�. [E_01]
        /// </summary>
        public int SlotNo
        {
            get { return _SlotNo;       }
            set { _SlotNo = value;      }
        }
        #endregion

        #region [�Ӽ�] Old ���Թ�ȣ [E_01]
        private int _OldSlotNo = 0;
        /// <summary>
        /// ���Թ�ȣ�� �����ϰų� �����ɴϴ�. [E_01]
        /// </summary>
        public int OldSlotNo
        {
            get { return _OldSlotNo; }
            set { _OldSlotNo = value; }
        }
        #endregion

        #region [�Ӽ�] �������
        private	int		_Order		= 0;
		/// <summary>
		/// ��������� �������ų� �����մϴ�
		/// </summary>
		public	int	Order
		{
			get { return _Order;	}
			set { _Order = value;	}
		}
		#endregion
		
		#region [�Ӽ�] �̵���ڵ�
		private	int	_Media		= 0;
		/// <summary>
		/// �̵���ڵ带 �����ϰų� �����ɴϴ�
		/// </summary>
		public int  MediaCode
		{
			get { return _Media;		}
			set { _Media = value;		}
		}
		#endregion

		#region [�Ӽ�] ���幰��
		private	int		_ImpTotal	= 0;
		/// <summary>
		/// ���幰���� �������ų� �����մϴ�
		/// </summary>
		public	int	ImpressionCount
		{
			get { return _ImpTotal;		}
			set { _ImpTotal	= value;	}
		}
		#endregion

		#region [�Ӽ�] ���幰�� �ϰ�
		private	int		_ImpDay		= 0;
		/// <summary>
		/// �ϰ����幰���� �������ų� �����մϴ�
		/// </summary>
		public int	ImpressionCntDaily
		{
			get { return _ImpDay;	}
			set { _ImpDay = value;	}
		}
		#endregion

		#region [�Ӽ�] �����������
		private	string	_BeginDay	= "";
		/// <summary>
		/// ����������ڸ� �������ų� �����մϴ�
		/// </summary>
		public  string	BeginDate	
		{
			get { return _BeginDay;		}
			set {	_BeginDay = value;	}
		}
		#endregion

		#region [�Ӽ�] ������������
		private	string	_EndDay		= "";
		/// <summary>
		/// �����������ڸ� �������ų� �����մϴ�
		/// </summary>
		public  string	EndDate	
		{
			get { return _EndDay;		}
			set {	_EndDay = value;	}
		}
		
		#endregion

		#region [�Ӽ�] ��뿩��
		private	string	_UseYN		= "";
		/// <summary>
		/// ��뿩�θ� �������ų� �����մϴ�
		/// </summary>
		public string	IsUse
		{
			get { return _UseYN;	}
			set { _UseYN = value;	}
		}
		#endregion
		
		#region [�Ӽ�] ����󵵼�
		private	int		_FreqCnt	= 0;
		/// <summary>
		/// ����󵵼��� �������ų� �����մϴ�
		/// </summary>
		public	int	FreqCount
		{
			get { return _FreqCnt;	}
			set { _FreqCnt = value;	}
		}
		#endregion

		#endregion

		#region [�Լ�] ������
	
		/// <summary>
		/// [S1] Ȩ������� ������ Model
		/// </summary>
		public SchHomeCmModel() : base () 
		{
			Init();
		}
		#endregion

		#region Public �޼ҵ�
		/// <summary>
		/// ���� �ʱ�ȭ �Ѵ�
		/// </summary>
		public void Init()
		{
			_SearchMedia	= 0;
			_ItemNo		= 0;
			_ItemName	= "";
			_Order		= 0;
			_Media		= 0;
			_ImpTotal	= 0;
			_ImpDay		= 0;
			_BeginDay	= "";
			_EndDay		= "";
			_UseYN		= "";
			_FreqCnt	= 0;
			_DataSet	= new DataSet();

			//[E_01]
            _SlotNo     = 0;     
            _OldSlotNo  = 0;    
		}

		#endregion
	}
}