// ===============================================================================
//
// BizManageTgtControl.cs
//
// 영업관리대상 광고주의 광고 판매 내역을 조회합니다.
//
// ===============================================================================
// Release history
// ===============================================================================
// 
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

namespace AdManagerClient
{
    public partial class BizManageTgtControl : System.Windows.Forms.UserControl, IUserControl
    {
        #region 이벤트핸들러
        public event StatusEventHandler StatusEvent;			// 상태이벤트 핸들러
        public event ProgressEventHandler ProgressEvent;		// 처리중이벤트 핸들러
        #endregion

        #region 사용자정의 객체 및 변수

        // 시스템 정보 : 화면공통
        private SystemModel systemModel = FrameSystem.oSysModel;
        private CommonModel commonModel = FrameSystem.oComModel;
        private Logger log = FrameSystem.oLog;
        private MenuPower menu = FrameSystem.oMenu;

        // 메뉴코드 : 보안이 필요한 화면에 필요함
        public string menuCode = "";

        // 사용할 정보모델
        BizManageModel model = new BizManageModel();	// 광고일간레포트 모델

        // 화면처리용 변수
        bool canRead = false;

        #endregion

        #region IUserControl 구현
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

        public BizManageTgtControl()
        {
            InitializeComponent();
        }

        #region 컨트롤 로드
        private void UserControl_Load(object sender, System.EventArgs e)
        {
            // 컨트롤 초기화
            InitControl();
        }

        #endregion

        #region 컨트롤 초기화
        private void InitControl()
        {
            ProgressStart();

            InitCombo();

            // 조회권한 검사
            if (menu.CanRead(MenuCode))
            {
                canRead = true;
            }

            InitButton();

            ProgressStop();
        }

        private void InitCombo()
        {
            // 기간시작일 및 종료일을 금일로 셋트
            cbSearchBgnDay.Value = DateTime.Now;
            cbSearchEndDay.Value = DateTime.Now;

            Init_RapCode();
            Init_AgencyCode();
            Init_AdvertiserCode();
            Init_AdType();
        }

        private void InitButton()
        {
            if (canRead)
            {
                btnSearch.Enabled = true;
            }

            Application.DoEvents();
        }

        private void Init_RapCode()
        {
            // 랩을 조회한다.
            MediaRapCodeModel mediaRapCodeModel = new MediaRapCodeModel();
            new MediaRapCodeManager(systemModel, commonModel).GetMediaRapCodeList(mediaRapCodeModel);

            if (mediaRapCodeModel.ResultCD.Equals("0000"))
            {
                // 데이터셋에 셋팅
                Utility.SetDataTable(bizManageTgtDs.MediaRaps, mediaRapCodeModel.MediaRapCodeDataSet);
            }

            // 검색조건의 콤보
            this.cbSearchRap.Items.Clear();

            // 콤보박스에 셋트할 코드목록을 담을 Item배열을 선언
            Janus.Windows.EditControls.UIComboBoxItem[] comboItems = new Janus.Windows.EditControls.UIComboBoxItem[mediaRapCodeModel.ResultCnt + 1];

            comboItems[0] = new Janus.Windows.EditControls.UIComboBoxItem("미디어렙선택", "00");

            for (int i = 0; i < mediaRapCodeModel.ResultCnt; i++)
            {
                DataRow row = bizManageTgtDs.MediaRaps.Rows[i];

                string val = row["RapCode"].ToString();
                string txt = row["RapName"].ToString();
                comboItems[i + 1] = new Janus.Windows.EditControls.UIComboBoxItem(txt, val);
            }
            // 콤보에 셋트
            this.cbSearchRap.Items.AddRange(comboItems);
            this.cbSearchRap.SelectedIndex = 0;

            Application.DoEvents();
        }

        private void Init_AgencyCode()
        {
            // 대행사를 조회한다.
            AgencyCodeModel agencyCodeModel = new AgencyCodeModel();
            new AgencyCodeManager(systemModel, commonModel).GetAgencyCodeList(agencyCodeModel);

            if (agencyCodeModel.ResultCD.Equals("0000"))
            {
                // 데이터셋에 셋팅
                Utility.SetDataTable(bizManageTgtDs.Agencys, agencyCodeModel.AgencyCodeDataSet);
            }

            // 검색조건의 콤보
            this.cbSearchAgency.Items.Clear();

            // 콤보박스에 셋트할 코드목록을 담을 Item배열을 선언
            Janus.Windows.EditControls.UIComboBoxItem[] comboItems = new Janus.Windows.EditControls.UIComboBoxItem[agencyCodeModel.ResultCnt + 1];

            comboItems[0] = new Janus.Windows.EditControls.UIComboBoxItem("대행사선택", "00");

            for (int i = 0; i < agencyCodeModel.ResultCnt; i++)
            {
                DataRow row = bizManageTgtDs.Agencys.Rows[i];

                string val = row["AgencyCode"].ToString();
                string txt = row["AgencyName"].ToString();
                comboItems[i + 1] = new Janus.Windows.EditControls.UIComboBoxItem(txt, val);
            }

            // 콤보에 셋트
            this.cbSearchAgency.Items.AddRange(comboItems);
            this.cbSearchAgency.SelectedIndex = 0;

            Application.DoEvents();
        }

        private void Init_AdvertiserCode()
        {
            // 광고주를 조회한다.
            ClientModel clientModel = new ClientModel();
            new ClientManager(systemModel, commonModel).GetAdvertiserList(clientModel);

            if (clientModel.ResultCD.Equals("0000"))
            {
                // 데이터셋에 셋팅
                Utility.SetDataTable(bizManageTgtDs.Advertisers, clientModel.ClientDataSet);
            }

            // 검색조건의 콤보
            this.cbSearchAdvertiser.Items.Clear();

            // 콤보박스에 셋트할 코드목록을 담을 Item배열을 선언
            Janus.Windows.EditControls.UIComboBoxItem[] comboItems = new Janus.Windows.EditControls.UIComboBoxItem[clientModel.ResultCnt + 1];

            comboItems[0] = new Janus.Windows.EditControls.UIComboBoxItem("광고주선택", "00");

            for (int i = 0; i < clientModel.ResultCnt; i++)
            {
                DataRow row = bizManageTgtDs.Advertisers.Rows[i];

                string val = row["AdvertiserCode"].ToString();
                string txt = row["AdvertiserName"].ToString();
                comboItems[i + 1] = new Janus.Windows.EditControls.UIComboBoxItem(txt, val);
            }

            // 콤보에 셋트
            this.cbSearchAdvertiser.Items.AddRange(comboItems);
            this.cbSearchAdvertiser.SelectedIndex = 0;

            Application.DoEvents();
        }

        private void Init_AdType()
        {
            // 코드에서 내역상태를 조회한다.
            CodeModel codeModel = new CodeModel();
            codeModel.Section = "26";				// '26':광고종류  TODO: 코드분류는 추후 XML로 관리되어야...
            new CodeManager(systemModel, commonModel).GetCodeList(codeModel);

            if (codeModel.ResultCD.Equals("0000"))
            {
                // 데이터셋에 셋팅
                Utility.SetDataTable(bizManageTgtDs.AdType, codeModel.CodeDataSet);
            }

            // 검색조건의 콤보
            this.cbSearchAdType.Items.Clear();

            // 콤보박스에 셋트할 코드목록을 담을 Item배열을 선언
            Janus.Windows.EditControls.UIComboBoxItem[] comboItems = new Janus.Windows.EditControls.UIComboBoxItem[codeModel.ResultCnt + 1];

            comboItems[0] = new Janus.Windows.EditControls.UIComboBoxItem("광고종류", "00");

            for (int i = 0; i < codeModel.ResultCnt; i++)
            {
                DataRow row = bizManageTgtDs.AdType.Rows[i];

                string val = row["Code"].ToString();
                string txt = row["CodeName"].ToString();
                comboItems[i + 1] = new Janus.Windows.EditControls.UIComboBoxItem(txt, val);
            }
            // 콤보에 셋트
            this.cbSearchAdType.Items.AddRange(comboItems);
            this.cbSearchAdType.SelectedIndex = 0;

            Application.DoEvents();
        }

        private void DisableButton()
        {
            btnSearch.Enabled = false;
            //btnExcel.Enabled = false;

            Application.DoEvents();
        }

        #endregion

        #region 액션처리 메소드

        /// <summary>
        /// <summary>
        /// 조회버튼 클릭
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSearch_Click(object sender, System.EventArgs e)
        {
            DisableButton();
            SearchReport();
            InitButton();
        }

        #endregion

        #region 처리메소드
        /// <summary>
        /// 일별시청률 조회
        /// </summary>
        private void SearchReport()
        {
            StatusMessage("영업관리대상 광고 판매 내역을 조회합니다.");

            ProgressStart();

            try
            {
                // 데이터모델 초기화
                model.Init();

                model.SearchRapCode        = cbSearchRap.SelectedValue.ToString();
                model.SearchAgencyCode     = cbSearchAgency.SelectedValue.ToString();
                model.SearchAdvertiserCode = cbSearchAdvertiser.SelectedValue.ToString();
                model.SearchAdType         = cbSearchAdType.SelectedValue.ToString();

                model.SearchStartDay = cbSearchBgnDay.Value.ToString("yyyyMMdd");
                model.SearchEndDay   = cbSearchEndDay.Value.ToString("yyyyMMdd");

                //  영업관리 대상 광고 판매 내역 목록 조회 서비스를 호출한다.
                new BizManageTgtManager(systemModel, commonModel).GetBizManageTargetList(model);

                if (model.ResultCD.Equals("0000"))
                {
                    // DataView가 정상적으로 적용되지 않아 직접 테이블로 바인딩
                    //Utility.SetDataTable(bizManageTgtDs.BizManageTgtList, model.BizManageDataSet);
                    pgrdBizManageTgt.DataSource = model.BizManageDataSet.Tables[0];
                    StatusMessage(model.ResultCnt + "건이 조회되었습니다.");
                }
            }
            catch (FrameException fe)
            {
                FrameSystem.showMsgForm("영업관리대상 광고 판매 내역 조회오류", new string[] { fe.ErrCode, fe.ResultMsg });
            }
            catch (Exception ex)
            {
                FrameSystem.showMsgForm("영업관리대상 광고 판매 내역 조회오류", new string[] { "", ex.Message });
            }
            finally
            {
                ProgressStop();
            }
        }
        #endregion

        #region 이벤트함수

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
                ProgressEventArgs ea = new ProgressEventArgs();
                ea.Type = ProgressEventArgs.Start;
                ProgressEvent(this, ea);
            }
        }

        private void ProgressStop()
        {
            if (ProgressEvent != null)
            {
                ProgressEventArgs ea = new ProgressEventArgs();
                ea.Type = ProgressEventArgs.Stop;
                ProgressEvent(this, ea);
            }
        }
        #endregion

        #region 화면이벤트
        private void pgrdBizManageTgt_CustomCellDisplayText(object sender, DevExpress.XtraPivotGrid.PivotCellDisplayTextEventArgs e)
        {
            if ((e.DataField == fieldStartDay || e.DataField == fieldEndDay || e.DataField == fieldAdTime || e.DataField == fieldTgtCategoryName) 
                && e.RowValueType == DevExpress.XtraPivotGrid.PivotGridValueType.Total)
                e.DisplayText = string.Empty;
        }

        private void cbSearchEndDay_ValueChanged(object sender, EventArgs e)
        {
            DateTime bgnDay = cbSearchBgnDay.Value;
            DateTime endDay = cbSearchEndDay.Value;

            TimeSpan ts = endDay - bgnDay;
            // Difference in days.
            int differenceInDays = ts.Days;

            if (differenceInDays > 364)
            {
                cbSearchEndDay.Value = bgnDay.AddDays(364);
                MessageBox.Show("집계기준일은 최대 1년까지만 선택가능합니다.", "기간 선택 경고", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (differenceInDays < 0)
            {
                cbSearchEndDay.Value = bgnDay;
                MessageBox.Show("집계기준 종료일은 시작일보다 작을 수 없습니다.", "기간 선택 경고", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void cbSearchBgnDay_ValueChanged(object sender, EventArgs e)
        {
            DateTime bgnDay = cbSearchBgnDay.Value;
            DateTime endDay = cbSearchEndDay.Value;

            TimeSpan ts = endDay - bgnDay;
            // Difference in days.
            int differenceInDays = ts.Days;

            if (differenceInDays > 364)
            {
                cbSearchEndDay.Value = bgnDay.AddDays(364);
            }
            else if (differenceInDays < 0)
            {
                cbSearchEndDay.Value = bgnDay;
            }
        }
        #endregion
    }
}
