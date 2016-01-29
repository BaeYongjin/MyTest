using System;
using System.Data;

using WinFramework.Base;

namespace AdManagerModel
{
    /// <summary>
    /// OAP 편성그룹관리 모델
    /// </summary>
    public class GroupOrganizationModel : BaseModel
    {
        public GroupOrganizationModel()
            : base()
        {
            Init();
        }

        #region Public 메소드

        public void Init()
        {
            GroupCode = "";
            GroupName = "";
            Comment = "";
            UseYn = "";
            RegDt = "";
            ModDt = "";
            RegId = "";
            ItemNo = "";
            ScheduleOrder = "";
            AckNo = "";
            MediaCode = "";
            LogYn = 0;
            //CommonYn = "";
            CommonYn = 0;
            ItemName = "";
            State = "";
            FileSize = "";
        }

        #endregion

        #region 프로퍼티

        public string GroupCode { set; get; }

        public string GroupName { set; get; }

        public string Comment { set; get; }

        public string UseYn { set; get; }

        public string RegDt { set; get; }

        public string ModDt { set; get; }

        public string RegId { set; get; }

        public string ItemNo { set; get; }

        public string ScheduleOrder { set; get; }

        public string AckNo { set; get; }

        public string MediaCode { set; get; }

        public int LogYn { set; get; }

        //public string CommonYn { set; get; }
        public int CommonYn { set; get; }

        public string ItemName { set; get; }

        public string State { set; get; }

        public string FileSize { set; get; }

        /// <summary>
        /// 마지막순서
        /// </summary>
        public string LastOrder { set; get; }

        /// <summary>
        /// 파일건수검사용
        /// </summary>
        public int FileListCount { set; get; }

        /// <summary>
        /// 데이터셋
        /// </summary>
        public DataSet GroupOrganizationDataSet { set; get; }

        #endregion
    }
}
