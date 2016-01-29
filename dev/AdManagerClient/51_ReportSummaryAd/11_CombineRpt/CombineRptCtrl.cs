using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using WinFramework.Misc;
using WinFramework.Base;
using WinFramework.Data;

using AdManagerModel;
using Excel = Microsoft.Office.Interop.Excel; // 엑셀 참조
using System.Reflection;
using AdManagerClient._51_ReportSummaryAd._11_CombineRpt;
using AdManagerClient.ReportSummAd;

using DevExpress.XtraPrinting;
using DevExpress.XtraPrintingLinks;
using DevExpress.XtraGrid;

namespace AdManagerClient
{
    public partial class CombineRptCtrl : UserControl, IUserControl
    {

        #region [공통1] IUserControl 구현
        // 메뉴코드 : 보안이 필요한 화면에 필요함
        public string menuCode = "";
        
        /// <summary>
        /// 메뉴 코드-보안이 필요한 화면에 필요함
        /// </summary>
        public string MenuCode
        {
            set { this.menuCode = value; }
            get { return this.menuCode; }
        }

        /// <summary>
        /// 부모컨트롤 지정
        /// </summary>
        /// <param name="control"></param>
        public void SetParent(Control control)
        {
            this.Parent = control;
        }
        /// <summary>
        /// DockStype지정
        /// </summary>
        /// <param name="style"></param>
        public void SetDockStyle(DockStyle style)
        {
            this.Dock = style;
        }
        #endregion

        #region [공통2] 이벤트함수

        public event StatusEventHandler StatusEvent;			// 상태이벤트 핸들러
        public event ProgressEventHandler ProgressEvent;			// 처리중이벤트 핸들러

        private void StatusMessage(string strMessage)
        {
            if (StatusEvent != null)
            {
                StatusEventArgs ea = new StatusEventArgs();
                ea.Message = strMessage;
                StatusEvent(this, ea);
            }
        }

        private void ProgressStart()
        {
            if (ProgressEvent != null)
            {
                WinFramework.Base.ProgressEventArgs ea = new WinFramework.Base.ProgressEventArgs();
                ea.Type = WinFramework.Base.ProgressEventArgs.Start;
                ProgressEvent(this, ea);
            }
        }

        private void ProgressStop()
        {
            if (ProgressEvent != null)
            {
                WinFramework.Base.ProgressEventArgs ea = new WinFramework.Base.ProgressEventArgs();
                ea.Type = WinFramework.Base.ProgressEventArgs.Stop;
                ProgressEvent(this, ea);
            }
        }
        #endregion

        #region [공통3] 사용자정의 객체 및 변수
        // 시스템 정보 : 화면공통
        private SystemModel systemModel = FrameSystem.oSysModel;
        private CommonModel commonModel = FrameSystem.oComModel;
        private Logger log = FrameSystem.oLog;
        private MenuPower menu = FrameSystem.oMenu;
        #endregion

        #region [개별] 사용자정의 객체 및 변수
        // 사용할 정보모델
        AdManagerModel.ReportAd.RptAdBaseModel mainModel = new AdManagerModel.ReportAd.RptAdBaseModel();
        
        string keyContractName = "";
        string keyCampaignCode = "";
        string keyCampaignName = "";
        string keyReportBgnDay = "";
        string keyReportEndDay = "";
        string keyAgencyName = "";
        string keyAdvertiserName = "";
        #endregion

        public CombineRptCtrl()
        {
            InitializeComponent();
        }

        private void CombineRptCtrl_Load(object sender, EventArgs e)
        {
            rptHeader.SearchClicked += new SearchClickEventHandler(OnSearch);
            rptHeader.ExcelClicked += new ExcelClickEventHandler(OnExcel);
            rptHeader.u_MenuName = MenuCode;
            rptHeader.u_InitControl();
        }

        private void OnSearch(object sender, SearchReportData e)
        {
            StatusMessage("통합리포팅 조회합니다.");
            ProgressStart();

            try
            {
                // 데이터모델 초기화
                mainModel.Init();
                mainModel.SearchContractSeq = e.ContractSeq;
                mainModel.CampaignCode = e.CampaignNo;
                mainModel.SearchBgnDay = e.ItemBeginDay;
                mainModel.SearchEndDay = e.ItemEndDay;

                keyContractName = "";
                keyAgencyName = "";
                keyAdvertiserName = "";
                keyReportBgnDay = "";
                keyReportEndDay = "";

                uiPanelList.Text = "통합 리포팅";
                
                new ReportSummAdManager(systemModel, commonModel).GetListCombine(mainModel);
                
                if (mainModel.ResultCD.Equals("0000"))
                {
                    Utility.SetDataTable(combineRptDataSet.ListTable, mainModel.ReportDataSet);
                    StatusMessage(mainModel.ResultCnt + "건이 조회되었습니다.");

                    keyContractName = e.ContractName;
                    keyAgencyName = e.AgencyName;
                    keyAdvertiserName = e.AdvertiserName;
                    keyCampaignCode = e.CampaignNo;
                    keyCampaignName = e.CampaignName;
                    keyReportBgnDay = e.ItemBeginDay;
                    keyReportEndDay = e.ItemEndDay;
                    
                    uiPanelList.Text = keyContractName + " | " + keyCampaignName + " | "  + keyReportBgnDay + " ~ " + keyReportEndDay;

                    rptHeader.u_IsPrint = true;
                }
                else
                {
                    rptHeader.u_IsPrint = false;
                }
            }
            catch (FrameException fe)
            {
                rptHeader.u_IsPrint = false;
                FrameSystem.showMsgForm("통합리포팅 조회오류", new string[] { fe.ErrCode, fe.ResultMsg });
            }
            catch (Exception ex)
            {
                rptHeader.u_IsPrint = false;
                FrameSystem.showMsgForm("통합리포팅 조회오류", new string[] { "", ex.Message });
            }
            finally
            {
                ProgressStop();
            }
        }

        private void OnExcel(object sender, EventArgs e)
        {
            //DevExpress.XtraPrinting.XlsExportOptions opt = new DevExpress.XtraPrinting.XlsExportOptions();
            //opt.ExportMode = DevExpress.XtraPrinting.XlsExportMode.SingleFile;
            //gridControl1.ExportToXlsx(Application.StartupPath.ToString() + "/text.Xlsx");

            using (var saveDialog = new SaveFileDialog())
            {
                saveDialog.Filter = "Excel (.xlsx)|*.xlsx";

                if (saveDialog.ShowDialog() == DialogResult.OK)
                {
                    var options = new XlsxExportOptions();
                    options.ExportMode = XlsxExportMode.SingleFile;
                    options.SheetName = "통합리포팅";

                    gridControl1.ExportToXlsx(saveDialog.FileName, options);
                }
            }
        }
    }
}
