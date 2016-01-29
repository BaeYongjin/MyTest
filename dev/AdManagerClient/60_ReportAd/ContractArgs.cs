using System;
using System.Collections;

namespace AdManagerClient
{
	#region [ 계약관련 매개변수 클래스 ]
	/// <summary>
	/// 계약관련 매개변수 클래스
	/// </summary>
	public class ContractEventArgs : EventArgs
	{
		#region [ 속 성 ]
		#region[ P:계약번호 ]
		private string _contractSeq    = "";
		/// <summary>
		/// 계약번호를 가져옵니다.
		/// </summary>
		public string	ContractSeq
		{
			get
			{
				return _contractSeq;
			}
		}
		#endregion

		#region[ P:계약명 ]
		private string _contractName   = "";
		/// <summary>
		/// 계약번호를 가져옵니다.
		/// </summary>
		public string	ContractName
		{
			get
			{
				return _contractName;
			}
		}
		#endregion

		#region[ P:계약시작일 ]
		private string _startDay   = "";
		/// <summary>
		/// 계약시작일자를 가져옵니다.
		/// </summary>
		public string	BeginDay
		{
			get
			{
				return _startDay;
			}
		}
		#endregion

		#region[ P:계약종료일 ]
		private string _endDay         = "";
		/// <summary>
		/// 계약종료일자를 가져옵니다.
		/// </summary>
		public string	EndDay
		{
			get
			{
				return _endDay;
			}
		}
		#endregion

		#region[ P:대행사명 ]
		private string _agencyName     = "";
		/// <summary>
		/// 대행사명을 가져옵니다
		/// </summary>
		public string	AgencyName
		{
			get
			{
				return _agencyName;
			}
		}
		#endregion

		#region[ P:광고주 명 ]
		private string _advertiserName     = "";
		/// <summary>
		/// 광고주명을 가져옵니다
		/// </summary>
		public string	AdvertiserName
		{
			get
			{
				return _advertiserName;
			}
		}
		#endregion
		#endregion

		#region [ M:생성자 ]
		public ContractEventArgs( string contSeq, string contNm, string beginDay, string endDay, string agencyNm, string adverNm)
		{
			_contractSeq	= contSeq;
			_contractName	= contNm;
			_startDay		= beginDay;
			_endDay			= endDay;
			_agencyName		= agencyNm;
			_advertiserName= adverNm;
		}
		#endregion
	}
	#endregion

    #region [ 다중선택 광고내역 매개변수 ]

    public class AdItemsEventArgs : EventArgs
    {
        #region [속성] 선택된 광고번호들
        ArrayList   _list   = new ArrayList();
        /// <summary>
        /// 선택된 광고번호를 담고 있는 ArrayList를 가져온다
        /// </summary>
        public  ArrayList Items
        {
            get
            {
                return _list;
            }
        }
        #endregion

        #region [속성] 선택된 광고들의 계약명(다중계약의 경우엔 마지막선택건
        string      _contNm = "";
        /// <summary>
        /// 선택된 광고들의 계약명을 가져온다
        /// </summary>
        public  string  ContractName
        {
            get
            {
                return _contNm;
            }
        }
        #endregion

        #region [속성] 선택된 광고중 마지막광고의 광고명
        string      _itemNm = "";
        /// <summary>
        /// 선택된 광고중 마지막광고의 광고명를 가져온다
        /// </summary>
        public string ItemName
        {
            get
            {
                return _itemNm;
            }
        }
        #endregion

        /// <summary>
        /// 다중선택 광고내역 이벤트 매개변수
        /// </summary>
        /// <param name="adItems">선택된 광고번호가 담긴 ArrayList</param>
        /// <param name="contractName">계약명칭</param>
        /// <param name="itemName">광고명칭</param>
        public AdItemsEventArgs( ArrayList adItems, string contractName, string itemName)
        {
            _list   = adItems;
            _contNm = contractName;
            _itemNm = itemName;
        }
    }


    /// <summary>
    /// 다중선택 이벤트 발생시 사용되는 핸들러
    /// </summary>
    public  delegate void AdItemSelectEventHandler( object sender, AdItemsEventArgs e );

    #endregion

	#region [ 계약관련 대리자 정의 ]
	public delegate void ContractEventHandler( object sender, ContractEventArgs e);
	#endregion

	#region [ 레포트 조회용 기본 조건 데이터스트럭쳐 ]
	/// <summary>
	/// 레포트조회용 기본조건 데이터Struct
	/// </summary>
	public struct SearchReportData
	{
		public	string	MediaCode;
		public	string	MediaName;
		public	string	ContractSeq;
		public	string	ContractName;
		public	string	CampaignNo;
		public	string	CampaignName;
		public	string	ItemNo;
		public	string	ItemName;
		public	string	ItemBeginDay;
		public   string	ItemEndDay;
		public	string	ContBeginDay;
		public	string	ContEndDay;
		public	string	AgencyName;
		public	string	AdvertiserName;
	}
	#endregion



	/// <summary>
	/// 조회버튼 클릭 이벤트용
	/// </summary>
	public delegate void SearchClickEventHandler( object sender, SearchReportData e );

	/// <summary>
	/// 엑셀버튼 클릭 이벤트용
	/// </summary>
	public delegate void ExcelClickEventHandler( object sender, EventArgs e);
}
