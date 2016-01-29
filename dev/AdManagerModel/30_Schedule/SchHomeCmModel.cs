/*
 * -------------------------------------------------------
 * 수정코드  : [E_01]
 * 수정자    : YJ.PARK
 * 수정일    : 2014.08.28
 * 수정내용  :        
 *            - 홈 상업광고 편성 슬롯 추가
 * -------------------------------------------------------
 */
using System;
using System.Data;

using WinFramework.Base;

namespace AdManagerModel
{
	/// <summary>
	/// 홈상업광고 데이터 Model
	/// </summary>
	public class SchHomeCmModel : BaseModel
	{
		#region [속성] 조회조건
		private	int		_SearchMedia= 0;
		/// <summary>
		/// 조회조건인 미디어코드를 가져오거나 설정합니다
		/// </summary>
		public int	SearchMediaCode 
		{
			get { return _SearchMedia;	}
			set { _SearchMedia = value;	}
		}
		#endregion

		#region [속성] 목록조회용 DataSet
		private DataSet  _DataSet;
		/// <summary>
		/// 조회 데이터셋을 가져오거나 설정합니다
		/// </summary>
		public DataSet ScheduleDataSet
		{
			get { return _DataSet;	}
			set { _DataSet = value;	}
		}
		#endregion

		#region [속성] 데이터 입력/수정/삭제용
		#region [속성] 광고번호
		private	int		_ItemNo		= 0;
		/// <summary>
		/// 광고번호를 설정하거나 가져옵니다
		/// </summary>
		public int ItemNo
		{
			get {	return _ItemNo;		}
			set {	_ItemNo	= value;	}
			
		}
		#endregion

		#region [속성] 광고명
		private	string	_ItemName	= "";
		/// <summary>
		/// 광고명칭을 가져오거나 설정합니다
		/// </summary>
		public	string	ItemName
		{
			get {	return _ItemName;	}
			set {	_ItemName = value;	}
		}
		#endregion

        #region [속성] 슬롯번호 [E_01]
        private int _SlotNo = 0;
        /// <summary>
        /// 슬롯번호를 설정하거나 가져옵니다. [E_01]
        /// </summary>
        public int SlotNo
        {
            get { return _SlotNo;       }
            set { _SlotNo = value;      }
        }
        #endregion

        #region [속성] Old 슬롯번호 [E_01]
        private int _OldSlotNo = 0;
        /// <summary>
        /// 슬롯번호를 설정하거나 가져옵니다. [E_01]
        /// </summary>
        public int OldSlotNo
        {
            get { return _OldSlotNo; }
            set { _OldSlotNo = value; }
        }
        #endregion

        #region [속성] 광고순번
        private	int		_Order		= 0;
		/// <summary>
		/// 광고순번을 가져오거나 설정합니다
		/// </summary>
		public	int	Order
		{
			get { return _Order;	}
			set { _Order = value;	}
		}
		#endregion
		
		#region [속성] 미디어코드
		private	int	_Media		= 0;
		/// <summary>
		/// 미디어코드를 설정하거나 가져옵니다
		/// </summary>
		public int  MediaCode
		{
			get { return _Media;		}
			set { _Media = value;		}
		}
		#endregion

		#region [속성] 보장물량
		private	int		_ImpTotal	= 0;
		/// <summary>
		/// 보장물량을 가져오거나 설정합니다
		/// </summary>
		public	int	ImpressionCount
		{
			get { return _ImpTotal;		}
			set { _ImpTotal	= value;	}
		}
		#endregion

		#region [속성] 보장물량 일간
		private	int		_ImpDay		= 0;
		/// <summary>
		/// 일간보장물량을 가져오거나 설정합니다
		/// </summary>
		public int	ImpressionCntDaily
		{
			get { return _ImpDay;	}
			set { _ImpDay = value;	}
		}
		#endregion

		#region [속성] 노출시작일자
		private	string	_BeginDay	= "";
		/// <summary>
		/// 노출시작일자를 가져오거나 설정합니다
		/// </summary>
		public  string	BeginDate	
		{
			get { return _BeginDay;		}
			set {	_BeginDay = value;	}
		}
		#endregion

		#region [속성] 노출종료일자
		private	string	_EndDay		= "";
		/// <summary>
		/// 노출종료일자를 가져오거나 설정합니다
		/// </summary>
		public  string	EndDate	
		{
			get { return _EndDay;		}
			set {	_EndDay = value;	}
		}
		
		#endregion

		#region [속성] 사용여부
		private	string	_UseYN		= "";
		/// <summary>
		/// 사용여부를 가져오거나 설정합니다
		/// </summary>
		public string	IsUse
		{
			get { return _UseYN;	}
			set { _UseYN = value;	}
		}
		#endregion
		
		#region [속성] 노출빈도수
		private	int		_FreqCnt	= 0;
		/// <summary>
		/// 노출빈도수를 가져오거나 설정합니다
		/// </summary>
		public	int	FreqCount
		{
			get { return _FreqCnt;	}
			set { _FreqCnt = value;	}
		}
		#endregion

		#endregion

		#region [함수] 생성자
	
		/// <summary>
		/// [S1] 홈상업광고 데이터 Model
		/// </summary>
		public SchHomeCmModel() : base () 
		{
			Init();
		}
		#endregion

		#region Public 메소드
		/// <summary>
		/// 모델을 초기화 한다
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