/*
 * -------------------------------------------------------
 * Class Name: TargetingAdDenyModel
 * 주요기능  : 광고거부자 관리 Model
 * 작성일    : 2013.12. 16
 * 특이사항  : 
 * -------------------------------------------------------
 */
using System;
using System.Data;

using WinFramework.Base;

namespace AdManagerModel
{
    public class TargetingAdDenyModel : BaseModel
    {
        #region 프로퍼티

        public string KeySearch { set; get; }

        public string KeyPopupSearch { set; get; }

        /// <summary>
        /// 사용자ID
        /// </summary>
        public int UserID { set; get; }

        /// <summary>
        /// 셋탑ID
        /// </summary>
        public string StbId { set; get; }

        /// <summary>
        /// 거부할 광고 종류
        /// </summary>
        public string DenyCode { set; get; }

        /// <summary>
        /// 사용여부
        /// </summary>
        public string UseYn { set; get; }

        /// <summary>
        /// 비고
        /// </summary>
        public string Comment { set; get; }

        public DataSet AdDenyDataset { set; get; }

        #endregion
    }
}
