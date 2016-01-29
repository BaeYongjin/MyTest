using System;
using System.Collections;

namespace AdManagerClient
{
	#region [ ������ �Ű����� Ŭ���� ]
	/// <summary>
	/// ������ �Ű����� Ŭ����
	/// </summary>
	public class ContractEventArgs : EventArgs
	{
		#region [ �� �� ]
		#region[ P:����ȣ ]
		private string _contractSeq    = "";
		/// <summary>
		/// ����ȣ�� �����ɴϴ�.
		/// </summary>
		public string	ContractSeq
		{
			get
			{
				return _contractSeq;
			}
		}
		#endregion

		#region[ P:���� ]
		private string _contractName   = "";
		/// <summary>
		/// ����ȣ�� �����ɴϴ�.
		/// </summary>
		public string	ContractName
		{
			get
			{
				return _contractName;
			}
		}
		#endregion

		#region[ P:�������� ]
		private string _startDay   = "";
		/// <summary>
		/// ���������ڸ� �����ɴϴ�.
		/// </summary>
		public string	BeginDay
		{
			get
			{
				return _startDay;
			}
		}
		#endregion

		#region[ P:��������� ]
		private string _endDay         = "";
		/// <summary>
		/// ����������ڸ� �����ɴϴ�.
		/// </summary>
		public string	EndDay
		{
			get
			{
				return _endDay;
			}
		}
		#endregion

		#region[ P:������ ]
		private string _agencyName     = "";
		/// <summary>
		/// �������� �����ɴϴ�
		/// </summary>
		public string	AgencyName
		{
			get
			{
				return _agencyName;
			}
		}
		#endregion

		#region[ P:������ �� ]
		private string _advertiserName     = "";
		/// <summary>
		/// �����ָ��� �����ɴϴ�
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

		#region [ M:������ ]
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

    #region [ ���߼��� ������ �Ű����� ]

    public class AdItemsEventArgs : EventArgs
    {
        #region [�Ӽ�] ���õ� �����ȣ��
        ArrayList   _list   = new ArrayList();
        /// <summary>
        /// ���õ� �����ȣ�� ��� �ִ� ArrayList�� �����´�
        /// </summary>
        public  ArrayList Items
        {
            get
            {
                return _list;
            }
        }
        #endregion

        #region [�Ӽ�] ���õ� ������� ����(���߰���� ��쿣 ���������ð�
        string      _contNm = "";
        /// <summary>
        /// ���õ� ������� ������ �����´�
        /// </summary>
        public  string  ContractName
        {
            get
            {
                return _contNm;
            }
        }
        #endregion

        #region [�Ӽ�] ���õ� ������ ������������ �����
        string      _itemNm = "";
        /// <summary>
        /// ���õ� ������ ������������ ����� �����´�
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
        /// ���߼��� ������ �̺�Ʈ �Ű�����
        /// </summary>
        /// <param name="adItems">���õ� �����ȣ�� ��� ArrayList</param>
        /// <param name="contractName">����Ī</param>
        /// <param name="itemName">�����Ī</param>
        public AdItemsEventArgs( ArrayList adItems, string contractName, string itemName)
        {
            _list   = adItems;
            _contNm = contractName;
            _itemNm = itemName;
        }
    }


    /// <summary>
    /// ���߼��� �̺�Ʈ �߻��� ���Ǵ� �ڵ鷯
    /// </summary>
    public  delegate void AdItemSelectEventHandler( object sender, AdItemsEventArgs e );

    #endregion

	#region [ ������ �븮�� ���� ]
	public delegate void ContractEventHandler( object sender, ContractEventArgs e);
	#endregion

	#region [ ����Ʈ ��ȸ�� �⺻ ���� �����ͽ�Ʈ���� ]
	/// <summary>
	/// ����Ʈ��ȸ�� �⺻���� ������Struct
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
	/// ��ȸ��ư Ŭ�� �̺�Ʈ��
	/// </summary>
	public delegate void SearchClickEventHandler( object sender, SearchReportData e );

	/// <summary>
	/// ������ư Ŭ�� �̺�Ʈ��
	/// </summary>
	public delegate void ExcelClickEventHandler( object sender, EventArgs e);
}
