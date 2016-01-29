using System;
using System.Data;

using AdManagerModel;
using System.Collections;

namespace AdManagerClient.Common.Args
{
    class ItemEventArgs : EventArgs
    {
        public ItemModel itemModel = new ItemModel();
        public DataSet dataSet;

        // 매체
        public string keyMediaCode = "";			// 팝업호출시 매체셋트
        public string keyOrder = "";
        //선택된 광고번호들
        ArrayList _list = new ArrayList();
        //선택된 광고들의 계약명(다중계약의 경우엔 마지막선택건
        string _contNm = "";
        //선택된 광고중 마지막광고의 광고명
        string _itemNm = "";

        public int keyItemNo = 0;
        public string keyItemName = "";
        public string AdType = "";

        public string KeyCugCode = ""; // 추가
        public string CheckSchResult = "check";

        /// <summary>
        /// 선택된 광고번호를 담고 있는 ArrayList를 가져온다
        /// </summary>
        public ArrayList Items
        {
            get
            {
                return _list;
            }
        }
        /// <summary>
        /// 선택된 광고들의 계약명을 가져온다
        /// </summary>
        public string ContractName
        {
            get
            {
                return _contNm;
            }
        }
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
    }


    #region [ 다중선택 광고내역 매개변수 ]
    /// <summary>
    /// 다중선택 이벤트 발생시 사용되는 핸들러
    /// </summary>
    public delegate void AdItemSelectEventHandler(object sender, AdItemsEventArgs e);

    #endregion

    #region [ 계약관련 대리자 정의 ]
    public delegate void ContractEventHandler(object sender, ContractEventArgs e);
    #endregion

    #region [ 레포트 조회용 기본 조건 데이터스트럭쳐 ]
    /// <summary>
    /// 레포트조회용 기본조건 데이터Struct
    /// </summary>
    public struct SearchReportData
    {
        public string MediaCode;
        public string MediaName;
        public string ContractSeq;
        public string ContractName;
        public string CampaignNo;
        public string CampaignName;
        public string ItemNo;
        public string ItemName;
        public string ItemBeginDay;
        public string ItemEndDay;
        public string ContBeginDay;
        public string ContEndDay;
        public string AgencyName;
        public string AdvertiserName;
    }
    #endregion

    /// <summary>
    /// 조회버튼 클릭 이벤트용
    /// </summary>
    public delegate void SearchClickEventHandler(object sender, SearchReportData e);

    /// <summary>
    /// 엑셀버튼 클릭 이벤트용
    /// </summary>
    public delegate void ExcelClickEventHandler(object sender, EventArgs e);
}
