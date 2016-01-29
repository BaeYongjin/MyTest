// ===============================================================================
//
// SummaryAdControl.cs
//
// 총괄 광고 집계 컨트롤을 정의합니다. 
//
// ===============================================================================
// Release history
// 2007.10.08 RH.Jung 엑셀출력시 이용자수 및 Reach는 표시안함
// ===============================================================================
// Copyright (C) 2007 G-Inno Systems Inc.
// All rights reserved.
// 
using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using System.Threading;
using System.Text;

using WinFramework.Misc;
using WinFramework.Base;
using WinFramework.Data;

using AdManagerModel;
using Excel = Microsoft.Office.Interop.Excel; // 엑셀 참조
using System.Reflection;

namespace AdManagerClient
{
    /// <summary>
    /// 광고 일별시청률 관리 컨트롤
    /// </summary>
    public class SummaryAdControl : System.Windows.Forms.UserControl, IUserControl
    {
        #region 이벤트핸들러
        public event StatusEventHandler 			StatusEvent;			// 상태이벤트 핸들러
        public event ProgressEventHandler 			ProgressEvent;			// 처리중이벤트 핸들러
        #endregion
			
        #region 사용자정의 객체 및 변수

        // 시스템 정보 : 화면공통
        private SystemModel   systemModel   = FrameSystem.oSysModel;
        private CommonModel   commonModel   = FrameSystem.oComModel;
        private Logger        log           = FrameSystem.oLog;
        private MenuPower     menu          = FrameSystem.oMenu;

        // 메뉴코드 : 보안이 필요한 화면에 필요함
        public string        menuCode		= "";

        // 사용할 정보모델
        SummaryAdModel summaryAdModel  = new SummaryAdModel();	// 프로그램별 광고시청집계모델

        // 화면처리용 변수
        bool canRead			  = false;

        // Key 데이터
		string keyMediaName    = "";
		string keyContractName = "";
        string keyCampaignCode = "";
        string keyCampaignName = "";
        string keyAgencyName   = "";
		string keyAdvertiserName = "";
		string keyItemNo       = "";
		string keyItemName     = "";
		string keyReportBgnDay = "";
		string keyReportEndDay = "";
		string keyTotalUser    = "";
		int		keyPrice		= 0;
		string keyReportType   = "";

        string itemStartDay     = "";
        string itemEndDay       = "";
        string contractStartDay = "";
        string contractEndDay   = "";
        private Janus.Windows.EditControls.UICheckBox chkGenre;
		private Janus.Windows.EditControls.UICheckBox chkProfile;

		bool canPrint  = false;

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

        #region 화면 컴포넌트, 생성자, 소멸자

        private Janus.Windows.UI.Dock.UIPanelManager uiPM;
        private System.Windows.Forms.Panel pnlSearch;
        private Janus.Windows.EditControls.UIButton btnSearch;
        private Janus.Windows.UI.Dock.UIPanel uiPanelSearch;
        private Janus.Windows.UI.Dock.UIPanelInnerContainer uiPanelSearchContainer;
        private Janus.Windows.UI.Dock.UIPanel uiPanelList;
        private Janus.Windows.UI.Dock.UIPanelInnerContainer uiPanelListContainer;
        private Janus.Windows.UI.Dock.UIPanelGroup uiPanelChoiceAdSchedule;
        private Janus.Windows.EditControls.UIComboBox cbSearchMedia;
        private System.Windows.Forms.Label label1;
        private Janus.Windows.GridEX.EditControls.EditBox editBox1;
        private System.Windows.Forms.Panel panMenuSchedule;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel3;
		private System.Windows.Forms.Label lbSearchDate;
		private Janus.Windows.EditControls.UIButton btnPrint;
		private Janus.Windows.EditControls.UIButton btnExcel;
		private Janus.Windows.EditControls.UIGroupBox uiGroupBox1;
		private Janus.Windows.GridEX.GridEX grdExSummary;
		private AdManagerClient.SummaryAdDs summaryAdDs;
		private Janus.Windows.EditControls.UIComboBox cbSearchContract;
		private System.Windows.Forms.Label label6;
		private Janus.Windows.EditControls.UIComboBox cbSearchItem;
		private Janus.Windows.EditControls.UIRadioButton rbSearchType_W;
		private Janus.Windows.EditControls.UIRadioButton rbSearchType_C;
		private Janus.Windows.EditControls.UIRadioButton rbSearchType_M;
		private Janus.Windows.EditControls.UIRadioButton rbSearchType_D;
		private Janus.Windows.GridEX.EditControls.NumericEditBox ebTotalUser;
		private System.Windows.Forms.Label label9;
		private System.Windows.Forms.Label label10;
		private Janus.Windows.CalendarCombo.CalendarCombo cbSearchStartDay;
		private Janus.Windows.CalendarCombo.CalendarCombo cbSearchEndDay;
        private Janus.Windows.EditControls.UIComboBox cbCampaign;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label2;
        private System.ComponentModel.IContainer components;

        public SummaryAdControl()
        {
            // 이 호출은 Windows.Forms Form 디자이너에 필요합니다.
            InitializeComponent();

            

        }

        /// <summary> 
        /// 사용 중인 모든 리소스를 정리합니다.
        /// </summary>
        protected override void Dispose( bool disposing )
        {
            if( disposing )
            {
                if(components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose( disposing );
        }
        #endregion

        #region 구성 요소 디자이너에서 생성한 코드
        /// <summary> 
        /// 디자이너 지원에 필요한 메서드입니다. 
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마십시오.
        /// </summary>
        private void InitializeComponent()
        {
			this.components = new System.ComponentModel.Container();
			Janus.Windows.GridEX.GridEXLayout grdExSummary_DesignTimeLayout = new Janus.Windows.GridEX.GridEXLayout();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SummaryAdControl));
			this.uiPM = new Janus.Windows.UI.Dock.UIPanelManager(this.components);
			this.uiPanelChoiceAdSchedule = new Janus.Windows.UI.Dock.UIPanelGroup();
			this.uiPanelSearch = new Janus.Windows.UI.Dock.UIPanel();
			this.uiPanelSearchContainer = new Janus.Windows.UI.Dock.UIPanelInnerContainer();
			this.pnlSearch = new System.Windows.Forms.Panel();
			this.chkProfile = new Janus.Windows.EditControls.UICheckBox();
			this.chkGenre = new Janus.Windows.EditControls.UICheckBox();
			this.label4 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.cbCampaign = new Janus.Windows.EditControls.UIComboBox();
			this.ebTotalUser = new Janus.Windows.GridEX.EditControls.NumericEditBox();
			this.uiGroupBox1 = new Janus.Windows.EditControls.UIGroupBox();
			this.rbSearchType_W = new Janus.Windows.EditControls.UIRadioButton();
			this.rbSearchType_C = new Janus.Windows.EditControls.UIRadioButton();
			this.rbSearchType_M = new Janus.Windows.EditControls.UIRadioButton();
			this.rbSearchType_D = new Janus.Windows.EditControls.UIRadioButton();
			this.cbSearchStartDay = new Janus.Windows.CalendarCombo.CalendarCombo();
			this.lbSearchDate = new System.Windows.Forms.Label();
			this.cbSearchMedia = new Janus.Windows.EditControls.UIComboBox();
			this.cbSearchContract = new Janus.Windows.EditControls.UIComboBox();
			this.btnSearch = new Janus.Windows.EditControls.UIButton();
			this.btnExcel = new Janus.Windows.EditControls.UIButton();
			this.btnPrint = new Janus.Windows.EditControls.UIButton();
			this.label6 = new System.Windows.Forms.Label();
			this.cbSearchItem = new Janus.Windows.EditControls.UIComboBox();
			this.label9 = new System.Windows.Forms.Label();
			this.label10 = new System.Windows.Forms.Label();
			this.cbSearchEndDay = new Janus.Windows.CalendarCombo.CalendarCombo();
			this.uiPanelList = new Janus.Windows.UI.Dock.UIPanel();
			this.uiPanelListContainer = new Janus.Windows.UI.Dock.UIPanelInnerContainer();
			this.grdExSummary = new Janus.Windows.GridEX.GridEX();
			this.summaryAdDs = new AdManagerClient.SummaryAdDs();
			this.panel3 = new System.Windows.Forms.Panel();
			this.panMenuSchedule = new System.Windows.Forms.Panel();
			this.panel2 = new System.Windows.Forms.Panel();
			this.label1 = new System.Windows.Forms.Label();
			this.editBox1 = new Janus.Windows.GridEX.EditControls.EditBox();
			((System.ComponentModel.ISupportInitialize)(this.uiPM)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.uiPanelChoiceAdSchedule)).BeginInit();
			this.uiPanelChoiceAdSchedule.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.uiPanelSearch)).BeginInit();
			this.uiPanelSearch.SuspendLayout();
			this.uiPanelSearchContainer.SuspendLayout();
			this.pnlSearch.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.uiGroupBox1)).BeginInit();
			this.uiGroupBox1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.uiPanelList)).BeginInit();
			this.uiPanelList.SuspendLayout();
			this.uiPanelListContainer.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.grdExSummary)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.summaryAdDs)).BeginInit();
			this.panMenuSchedule.SuspendLayout();
			this.SuspendLayout();
			// 
			// uiPM
			// 
			this.uiPM.BackColorGradientAutoHideStrip = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(218)))), ((int)(((byte)(250)))));
			this.uiPM.ContainerControl = this;
			this.uiPM.PanelPadding.Bottom = 0;
			this.uiPM.PanelPadding.Left = 0;
			this.uiPM.PanelPadding.Right = 0;
			this.uiPM.PanelPadding.Top = 0;
			this.uiPM.VisualStyle = Janus.Windows.UI.Dock.PanelVisualStyle.Office2007;
			this.uiPanelChoiceAdSchedule.Id = new System.Guid("b162c53a-7940-47cd-a0ed-9762c59c50d8");
			this.uiPanelChoiceAdSchedule.StaticGroup = true;
			this.uiPanelSearch.Id = new System.Guid("afb3b7d0-aa79-443c-a1e8-3baba38f7a4d");
			this.uiPanelChoiceAdSchedule.Panels.Add(this.uiPanelSearch);
			this.uiPanelList.Id = new System.Guid("8e3140b9-bfcb-43af-b84e-2ee6cddf51dc");
			this.uiPanelChoiceAdSchedule.Panels.Add(this.uiPanelList);
			this.uiPM.Panels.Add(this.uiPanelChoiceAdSchedule);
			// 
			// Design Time Panel Info:
			// 
			this.uiPM.BeginPanelInfo();
			this.uiPM.AddDockPanelInfo(new System.Guid("b162c53a-7940-47cd-a0ed-9762c59c50d8"), Janus.Windows.UI.Dock.PanelGroupStyle.HorizontalTiles, Janus.Windows.UI.Dock.PanelDockStyle.Fill, true, new System.Drawing.Size(1010, 677), true);
			this.uiPM.AddDockPanelInfo(new System.Guid("afb3b7d0-aa79-443c-a1e8-3baba38f7a4d"), new System.Guid("b162c53a-7940-47cd-a0ed-9762c59c50d8"), 86, true);
			this.uiPM.AddDockPanelInfo(new System.Guid("8e3140b9-bfcb-43af-b84e-2ee6cddf51dc"), new System.Guid("b162c53a-7940-47cd-a0ed-9762c59c50d8"), 545, true);
			this.uiPM.AddFloatingPanelInfo(new System.Guid("b162c53a-7940-47cd-a0ed-9762c59c50d8"), Janus.Windows.UI.Dock.PanelGroupStyle.HorizontalTiles, true, new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
			this.uiPM.AddFloatingPanelInfo(new System.Guid("8e3140b9-bfcb-43af-b84e-2ee6cddf51dc"), new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
			this.uiPM.AddFloatingPanelInfo(new System.Guid("b2973a57-d3e1-4d80-9387-7a6dc3332542"), new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
			this.uiPM.AddFloatingPanelInfo(new System.Guid("afb3b7d0-aa79-443c-a1e8-3baba38f7a4d"), new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
			this.uiPM.AddFloatingPanelInfo(new System.Guid("18e78aea-bf80-4094-94f6-0d7d6006bb3d"), Janus.Windows.UI.Dock.PanelGroupStyle.VerticalTiles, true, new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
			this.uiPM.AddFloatingPanelInfo(new System.Guid("02c920f2-57c5-4aa8-bf42-309880d3314e"), new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
			this.uiPM.AddFloatingPanelInfo(new System.Guid("f40b18e5-7e83-4164-be4f-97c23451198d"), new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
			this.uiPM.AddFloatingPanelInfo(new System.Guid("f94be314-c212-42b8-b676-497c4d5f5485"), new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
			this.uiPM.EndPanelInfo();
			// 
			// uiPanelChoiceAdSchedule
			// 
			this.uiPanelChoiceAdSchedule.ActiveCaptionMode = Janus.Windows.UI.Dock.ActiveCaptionMode.Never;
			this.uiPanelChoiceAdSchedule.BackColor = System.Drawing.SystemColors.Window;
			this.uiPanelChoiceAdSchedule.CaptionVisible = Janus.Windows.UI.InheritableBoolean.True;
			this.uiPanelChoiceAdSchedule.CloseButtonVisible = Janus.Windows.UI.InheritableBoolean.False;
			this.uiPanelChoiceAdSchedule.Location = new System.Drawing.Point(0, 0);
			this.uiPanelChoiceAdSchedule.Name = "uiPanelChoiceAdSchedule";
			this.uiPanelChoiceAdSchedule.Size = new System.Drawing.Size(1010, 677);
			this.uiPanelChoiceAdSchedule.TabIndex = 4;
			this.uiPanelChoiceAdSchedule.Text = "광고총괄집계";
			// 
			// uiPanelSearch
			// 
			this.uiPanelSearch.AllowResize = Janus.Windows.UI.InheritableBoolean.False;
			this.uiPanelSearch.CaptionVisible = Janus.Windows.UI.InheritableBoolean.False;
			this.uiPanelSearch.CloseButtonVisible = Janus.Windows.UI.InheritableBoolean.False;
			this.uiPanelSearch.InnerContainer = this.uiPanelSearchContainer;
			this.uiPanelSearch.Location = new System.Drawing.Point(0, 22);
			this.uiPanelSearch.Name = "uiPanelSearch";
			this.uiPanelSearch.Size = new System.Drawing.Size(1010, 88);
			this.uiPanelSearch.TabIndex = 4;
			this.uiPanelSearch.Text = "검색";
			// 
			// uiPanelSearchContainer
			// 
			this.uiPanelSearchContainer.Controls.Add(this.pnlSearch);
			this.uiPanelSearchContainer.Location = new System.Drawing.Point(1, 1);
			this.uiPanelSearchContainer.Name = "uiPanelSearchContainer";
			this.uiPanelSearchContainer.Size = new System.Drawing.Size(1008, 86);
			this.uiPanelSearchContainer.TabIndex = 0;
			// 
			// pnlSearch
			// 
			this.pnlSearch.BackColor = System.Drawing.SystemColors.Window;
			this.pnlSearch.Controls.Add(this.chkProfile);
			this.pnlSearch.Controls.Add(this.chkGenre);
			this.pnlSearch.Controls.Add(this.label4);
			this.pnlSearch.Controls.Add(this.label2);
			this.pnlSearch.Controls.Add(this.cbCampaign);
			this.pnlSearch.Controls.Add(this.ebTotalUser);
			this.pnlSearch.Controls.Add(this.uiGroupBox1);
			this.pnlSearch.Controls.Add(this.cbSearchStartDay);
			this.pnlSearch.Controls.Add(this.lbSearchDate);
			this.pnlSearch.Controls.Add(this.cbSearchMedia);
			this.pnlSearch.Controls.Add(this.cbSearchContract);
			this.pnlSearch.Controls.Add(this.btnSearch);
			this.pnlSearch.Controls.Add(this.btnExcel);
			this.pnlSearch.Controls.Add(this.btnPrint);
			this.pnlSearch.Controls.Add(this.label6);
			this.pnlSearch.Controls.Add(this.cbSearchItem);
			this.pnlSearch.Controls.Add(this.label9);
			this.pnlSearch.Controls.Add(this.label10);
			this.pnlSearch.Controls.Add(this.cbSearchEndDay);
			this.pnlSearch.Dock = System.Windows.Forms.DockStyle.Fill;
			this.pnlSearch.Location = new System.Drawing.Point(0, 0);
			this.pnlSearch.Name = "pnlSearch";
			this.pnlSearch.Size = new System.Drawing.Size(1008, 86);
			this.pnlSearch.TabIndex = 3;
			// 
			// chkProfile
			// 
			this.chkProfile.Location = new System.Drawing.Point(788, 42);
			this.chkProfile.Name = "chkProfile";
			this.chkProfile.Size = new System.Drawing.Size(104, 23);
			this.chkProfile.TabIndex = 186;
			this.chkProfile.Text = "프로파일 포함";
			this.chkProfile.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
			// 
			// chkGenre
			// 
			this.chkGenre.Location = new System.Drawing.Point(788, 17);
			this.chkGenre.Name = "chkGenre";
			this.chkGenre.Size = new System.Drawing.Size(104, 23);
			this.chkGenre.TabIndex = 185;
			this.chkGenre.Text = "장르단위 포함";
			this.chkGenre.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(63, 54);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(60, 21);
			this.label4.TabIndex = 184;
			this.label4.Text = "개별광고";
			this.label4.TextAlign = System.Drawing.ContentAlignment.BottomRight;
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(80, 31);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(44, 21);
			this.label2.TabIndex = 183;
			this.label2.Text = "캠페인";
			this.label2.TextAlign = System.Drawing.ContentAlignment.BottomRight;
			// 
			// cbCampaign
			// 
			this.cbCampaign.BackColor = System.Drawing.Color.White;
			this.cbCampaign.ComboStyle = Janus.Windows.EditControls.ComboStyle.DropDownList;
			this.cbCampaign.Location = new System.Drawing.Point(124, 32);
			this.cbCampaign.Name = "cbCampaign";
			this.cbCampaign.Size = new System.Drawing.Size(244, 21);
			this.cbCampaign.TabIndex = 2;
			this.cbCampaign.Text = "캠페인선택";
			this.cbCampaign.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
			this.cbCampaign.SelectedIndexChanged += new System.EventHandler(this.cbCampaign_SelectedIndexChanged);
			// 
			// ebTotalUser
			// 
			this.ebTotalUser.BackColor = System.Drawing.Color.GhostWhite;
			this.ebTotalUser.DecimalDigits = 0;
			this.ebTotalUser.FormatString = "#,##0";
			this.ebTotalUser.Location = new System.Drawing.Point(616, 56);
			this.ebTotalUser.Name = "ebTotalUser";
			this.ebTotalUser.ReadOnly = true;
			this.ebTotalUser.Size = new System.Drawing.Size(104, 21);
			this.ebTotalUser.TabIndex = 181;
			this.ebTotalUser.TabStop = false;
			this.ebTotalUser.Text = "0";
			this.ebTotalUser.TextAlignment = Janus.Windows.GridEX.TextAlignment.Far;
			this.ebTotalUser.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
			this.ebTotalUser.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
			// 
			// uiGroupBox1
			// 
			this.uiGroupBox1.BorderColor = System.Drawing.Color.DarkGray;
			this.uiGroupBox1.Controls.Add(this.rbSearchType_W);
			this.uiGroupBox1.Controls.Add(this.rbSearchType_C);
			this.uiGroupBox1.Controls.Add(this.rbSearchType_M);
			this.uiGroupBox1.Controls.Add(this.rbSearchType_D);
			this.uiGroupBox1.FrameStyle = Janus.Windows.EditControls.FrameStyle.None;
			this.uiGroupBox1.Location = new System.Drawing.Point(472, 7);
			this.uiGroupBox1.Name = "uiGroupBox1";
			this.uiGroupBox1.Size = new System.Drawing.Size(310, 24);
			this.uiGroupBox1.TabIndex = 20;
			this.uiGroupBox1.VisualStyle = Janus.Windows.UI.Dock.PanelVisualStyle.Office2003;
			// 
			// rbSearchType_W
			// 
			this.rbSearchType_W.AutoSize = true;
			this.rbSearchType_W.Location = new System.Drawing.Point(134, 4);
			this.rbSearchType_W.Name = "rbSearchType_W";
			this.rbSearchType_W.ShowFocusRectangle = false;
			this.rbSearchType_W.Size = new System.Drawing.Size(43, 18);
			this.rbSearchType_W.TabIndex = 2;
			this.rbSearchType_W.Text = "주간";
			this.rbSearchType_W.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
			this.rbSearchType_W.CheckedChanged += new System.EventHandler(this.rbSearchType_W_CheckedChanged);
			// 
			// rbSearchType_C
			// 
			this.rbSearchType_C.AutoSize = true;
			this.rbSearchType_C.BackColor = System.Drawing.SystemColors.Window;
			this.rbSearchType_C.Location = new System.Drawing.Point(4, 4);
			this.rbSearchType_C.Name = "rbSearchType_C";
			this.rbSearchType_C.ShowFocusRectangle = false;
			this.rbSearchType_C.Size = new System.Drawing.Size(43, 18);
			this.rbSearchType_C.TabIndex = 0;
			this.rbSearchType_C.Text = "기간";
			this.rbSearchType_C.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
			this.rbSearchType_C.CheckedChanged += new System.EventHandler(this.rbSearchType_C_CheckedChanged);
			// 
			// rbSearchType_M
			// 
			this.rbSearchType_M.AutoSize = true;
			this.rbSearchType_M.Location = new System.Drawing.Point(199, 4);
			this.rbSearchType_M.Name = "rbSearchType_M";
			this.rbSearchType_M.ShowFocusRectangle = false;
			this.rbSearchType_M.Size = new System.Drawing.Size(43, 18);
			this.rbSearchType_M.TabIndex = 3;
			this.rbSearchType_M.Text = "월간";
			this.rbSearchType_M.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
			this.rbSearchType_M.CheckedChanged += new System.EventHandler(this.rbSearchType_M_CheckedChanged);
			// 
			// rbSearchType_D
			// 
			this.rbSearchType_D.AutoSize = true;
			this.rbSearchType_D.Checked = true;
			this.rbSearchType_D.Location = new System.Drawing.Point(69, 4);
			this.rbSearchType_D.Name = "rbSearchType_D";
			this.rbSearchType_D.ShowFocusRectangle = false;
			this.rbSearchType_D.Size = new System.Drawing.Size(43, 18);
			this.rbSearchType_D.TabIndex = 1;
			this.rbSearchType_D.TabStop = true;
			this.rbSearchType_D.Text = "일간";
			this.rbSearchType_D.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
			this.rbSearchType_D.CheckedChanged += new System.EventHandler(this.rbSearchType_D_CheckedChanged);
			// 
			// cbSearchStartDay
			// 
			// 
			// 
			// 
			this.cbSearchStartDay.DropDownCalendar.FirstMonth = new System.DateTime(2007, 8, 1, 0, 0, 0, 0);
			this.cbSearchStartDay.DropDownCalendar.Name = "";
			this.cbSearchStartDay.DropDownCalendar.VisualStyle = Janus.Windows.CalendarCombo.VisualStyle.Office2007;
			this.cbSearchStartDay.Location = new System.Drawing.Point(476, 32);
			this.cbSearchStartDay.Name = "cbSearchStartDay";
			this.cbSearchStartDay.Size = new System.Drawing.Size(104, 21);
			this.cbSearchStartDay.TabIndex = 4;
			this.cbSearchStartDay.Value = new System.DateTime(2007, 9, 14, 0, 0, 0, 0);
			this.cbSearchStartDay.VisualStyle = Janus.Windows.CalendarCombo.VisualStyle.Office2007;
			this.cbSearchStartDay.TextChanged += new System.EventHandler(this.cbSearchStartDay_TextChanged);
			// 
			// lbSearchDate
			// 
			this.lbSearchDate.Location = new System.Drawing.Point(392, 34);
			this.lbSearchDate.Name = "lbSearchDate";
			this.lbSearchDate.Size = new System.Drawing.Size(72, 21);
			this.lbSearchDate.TabIndex = 14;
			this.lbSearchDate.Text = "집계기준일";
			this.lbSearchDate.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// cbSearchMedia
			// 
			this.cbSearchMedia.BackColor = System.Drawing.Color.White;
			this.cbSearchMedia.ComboStyle = Janus.Windows.EditControls.ComboStyle.DropDownList;
			this.cbSearchMedia.Location = new System.Drawing.Point(8, 8);
			this.cbSearchMedia.Name = "cbSearchMedia";
			this.cbSearchMedia.Size = new System.Drawing.Size(112, 21);
			this.cbSearchMedia.TabIndex = 0;
			this.cbSearchMedia.Text = "매체선택";
			this.cbSearchMedia.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
			this.cbSearchMedia.SelectedIndexChanged += new System.EventHandler(this.cbSearchMedia_SelectedIndexChanged);
			// 
			// cbSearchContract
			// 
			this.cbSearchContract.BackColor = System.Drawing.Color.White;
			this.cbSearchContract.ComboStyle = Janus.Windows.EditControls.ComboStyle.DropDownList;
			this.cbSearchContract.Location = new System.Drawing.Point(124, 8);
			this.cbSearchContract.Name = "cbSearchContract";
			this.cbSearchContract.Size = new System.Drawing.Size(244, 21);
			this.cbSearchContract.TabIndex = 1;
			this.cbSearchContract.Text = "광고계약선택";
			this.cbSearchContract.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
			this.cbSearchContract.Click += new System.EventHandler(this.cbSearchContract_Click);
			// 
			// btnSearch
			// 
			this.btnSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnSearch.Enabled = false;
			this.btnSearch.Location = new System.Drawing.Point(895, 14);
			this.btnSearch.Name = "btnSearch";
			this.btnSearch.Size = new System.Drawing.Size(104, 24);
			this.btnSearch.TabIndex = 6;
			this.btnSearch.Text = "조 회";
			this.btnSearch.VisualStyle = Janus.Windows.UI.VisualStyle.Office2003;
			this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
			// 
			// btnExcel
			// 
			this.btnExcel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnExcel.Enabled = false;
			this.btnExcel.Location = new System.Drawing.Point(895, 42);
			this.btnExcel.Name = "btnExcel";
			this.btnExcel.Size = new System.Drawing.Size(104, 24);
			this.btnExcel.TabIndex = 7;
			this.btnExcel.Text = "EXCEL 출력";
			this.btnExcel.VisualStyle = Janus.Windows.UI.VisualStyle.Office2003;
			this.btnExcel.Click += new System.EventHandler(this.btnExcel_Click);
			// 
			// btnPrint
			// 
			this.btnPrint.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnPrint.Enabled = false;
			this.btnPrint.Location = new System.Drawing.Point(895, 88);
			this.btnPrint.Name = "btnPrint";
			this.btnPrint.Size = new System.Drawing.Size(104, 24);
			this.btnPrint.TabIndex = 11;
			this.btnPrint.Text = "리포트";
			this.btnPrint.Visible = false;
			this.btnPrint.VisualStyle = Janus.Windows.UI.VisualStyle.Office2003;
			this.btnPrint.Click += new System.EventHandler(this.btnPrint_Click);
			// 
			// label6
			// 
			this.label6.Location = new System.Drawing.Point(392, 9);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(68, 21);
			this.label6.TabIndex = 14;
			this.label6.Text = "기간구분";
			this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// cbSearchItem
			// 
			this.cbSearchItem.BackColor = System.Drawing.Color.White;
			this.cbSearchItem.ComboStyle = Janus.Windows.EditControls.ComboStyle.DropDownList;
			this.cbSearchItem.Location = new System.Drawing.Point(124, 56);
			this.cbSearchItem.Name = "cbSearchItem";
			this.cbSearchItem.Size = new System.Drawing.Size(244, 21);
			this.cbSearchItem.TabIndex = 3;
			this.cbSearchItem.Text = "광고선택";
			this.cbSearchItem.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
			this.cbSearchItem.SelectedItemChanged += new System.EventHandler(this.cbSearchItem_SelectedItemChanged);
			// 
			// label9
			// 
			this.label9.Location = new System.Drawing.Point(392, 57);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(128, 21);
			this.label9.TabIndex = 14;
			this.label9.Text = "집계일 기준 이용가구";
			this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label10
			// 
			this.label10.Location = new System.Drawing.Point(588, 36);
			this.label10.Name = "label10";
			this.label10.Size = new System.Drawing.Size(20, 16);
			this.label10.TabIndex = 4;
			this.label10.Text = "~";
			this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// cbSearchEndDay
			// 
			// 
			// 
			// 
			this.cbSearchEndDay.DropDownCalendar.FirstMonth = new System.DateTime(2007, 8, 1, 0, 0, 0, 0);
			this.cbSearchEndDay.DropDownCalendar.Name = "";
			this.cbSearchEndDay.DropDownCalendar.VisualStyle = Janus.Windows.CalendarCombo.VisualStyle.Office2007;
			this.cbSearchEndDay.Enabled = false;
			this.cbSearchEndDay.Location = new System.Drawing.Point(616, 32);
			this.cbSearchEndDay.Name = "cbSearchEndDay";
			this.cbSearchEndDay.Size = new System.Drawing.Size(104, 21);
			this.cbSearchEndDay.TabIndex = 5;
			this.cbSearchEndDay.Value = new System.DateTime(2007, 9, 14, 0, 0, 0, 0);
			this.cbSearchEndDay.VisualStyle = Janus.Windows.CalendarCombo.VisualStyle.Office2007;
			// 
			// uiPanelList
			// 
			this.uiPanelList.BackColor = System.Drawing.SystemColors.Window;
			this.uiPanelList.CaptionStyle = Janus.Windows.UI.Dock.PanelCaptionStyle.Dark;
			this.uiPanelList.InnerContainer = this.uiPanelListContainer;
			this.uiPanelList.Location = new System.Drawing.Point(0, 114);
			this.uiPanelList.MinimumSize = new System.Drawing.Size(-1, 100);
			this.uiPanelList.Name = "uiPanelList";
			this.uiPanelList.Size = new System.Drawing.Size(1010, 563);
			this.uiPanelList.TabIndex = 4;
			this.uiPanelList.Text = "가구별 시청조사";
			// 
			// uiPanelListContainer
			// 
			this.uiPanelListContainer.BackColor = System.Drawing.SystemColors.Window;
			this.uiPanelListContainer.Controls.Add(this.grdExSummary);
			this.uiPanelListContainer.Location = new System.Drawing.Point(1, 23);
			this.uiPanelListContainer.Name = "uiPanelListContainer";
			this.uiPanelListContainer.Size = new System.Drawing.Size(1008, 539);
			this.uiPanelListContainer.TabIndex = 0;
			// 
			// grdExSummary
			// 
			this.grdExSummary.AllowEdit = Janus.Windows.GridEX.InheritableBoolean.False;
			this.grdExSummary.AlternatingColors = true;
			this.grdExSummary.BorderStyle = Janus.Windows.GridEX.BorderStyle.None;
			this.grdExSummary.DataSource = this.summaryAdDs.Report;
			grdExSummary_DesignTimeLayout.LayoutString = resources.GetString("grdExSummary_DesignTimeLayout.LayoutString");
			this.grdExSummary.DesignTimeLayout = grdExSummary_DesignTimeLayout;
			this.grdExSummary.Dock = System.Windows.Forms.DockStyle.Fill;
			this.grdExSummary.EditorsControlStyle.ButtonAppearance = Janus.Windows.GridEX.ButtonAppearance.Regular;
			this.grdExSummary.EmptyRows = true;
			this.grdExSummary.FocusCellFormatStyle.BackColor = System.Drawing.SystemColors.Highlight;
			this.grdExSummary.FocusCellFormatStyle.ForeColor = System.Drawing.Color.White;
			this.grdExSummary.FocusStyle = Janus.Windows.GridEX.FocusStyle.None;
			this.grdExSummary.GridLineColor = System.Drawing.Color.Silver;
			this.grdExSummary.GridLines = Janus.Windows.GridEX.GridLines.Vertical;
			this.grdExSummary.GridLineStyle = Janus.Windows.GridEX.GridLineStyle.Solid;
			this.grdExSummary.GroupByBoxInfoFormatStyle.BackColor = System.Drawing.Color.Empty;
			this.grdExSummary.GroupByBoxVisible = false;
			this.grdExSummary.GroupRowFormatStyle.BackColor = System.Drawing.Color.White;
			this.grdExSummary.GroupRowFormatStyle.BackColorGradient = System.Drawing.Color.FromArgb(((int)(((byte)(218)))), ((int)(((byte)(228)))), ((int)(((byte)(238)))));
			this.grdExSummary.GroupRowFormatStyle.FontBold = Janus.Windows.GridEX.TriState.True;
			this.grdExSummary.GroupRowVisualStyle = Janus.Windows.GridEX.GroupRowVisualStyle.Outlook2003;
			this.grdExSummary.GroupTotalRowFormatStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
			this.grdExSummary.GroupTotalRowFormatStyle.FontBold = Janus.Windows.GridEX.TriState.True;
			this.grdExSummary.GroupTotalRowFormatStyle.ForeColor = System.Drawing.SystemColors.ControlText;
			this.grdExSummary.HeaderFormatStyle.TextAlignment = Janus.Windows.GridEX.TextAlignment.Center;
			this.grdExSummary.HideSelection = Janus.Windows.GridEX.HideSelection.Highlight;
			this.grdExSummary.Location = new System.Drawing.Point(0, 0);
			this.grdExSummary.Name = "grdExSummary";
			this.grdExSummary.ScrollBars = Janus.Windows.GridEX.ScrollBars.Vertical;
			this.grdExSummary.SelectedInactiveFormatStyle.BackColor = System.Drawing.Color.Gold;
			this.grdExSummary.Size = new System.Drawing.Size(1008, 539);
			this.grdExSummary.TabIndex = 12;
			this.grdExSummary.TabKeyBehavior = Janus.Windows.GridEX.TabKeyBehavior.ControlNavigation;
			this.grdExSummary.ThemedAreas = ((Janus.Windows.GridEX.ThemedArea)(((((((((Janus.Windows.GridEX.ThemedArea.ScrollBars | Janus.Windows.GridEX.ThemedArea.EditControls) 
            | Janus.Windows.GridEX.ThemedArea.Headers) 
            | Janus.Windows.GridEX.ThemedArea.GroupByBox) 
            | Janus.Windows.GridEX.ThemedArea.GroupRows) 
            | Janus.Windows.GridEX.ThemedArea.ControlBorder) 
            | Janus.Windows.GridEX.ThemedArea.Cards) 
            | Janus.Windows.GridEX.ThemedArea.Gridlines) 
            | Janus.Windows.GridEX.ThemedArea.CheckBoxes)));
			this.grdExSummary.TotalRowFormatStyle.BackColor = System.Drawing.Color.Gold;
			this.grdExSummary.TotalRowFormatStyle.FontBold = Janus.Windows.GridEX.TriState.True;
			this.grdExSummary.TotalRowFormatStyle.ForeColor = System.Drawing.SystemColors.ControlText;
			this.grdExSummary.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
			this.grdExSummary.FormattingRow += new Janus.Windows.GridEX.RowLoadEventHandler(this.grdExSummary_FormattingRow);
			// 
			// summaryAdDs
			// 
			this.summaryAdDs.DataSetName = "SummaryAdDs";
			this.summaryAdDs.Locale = new System.Globalization.CultureInfo("en-US");
			this.summaryAdDs.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
			// 
			// panel3
			// 
			this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel3.Location = new System.Drawing.Point(0, 0);
			this.panel3.Name = "panel3";
			this.panel3.Size = new System.Drawing.Size(851, 322);
			this.panel3.TabIndex = 25;
			// 
			// panMenuSchedule
			// 
			this.panMenuSchedule.BackColor = System.Drawing.SystemColors.Window;
			this.panMenuSchedule.Controls.Add(this.panel2);
			this.panMenuSchedule.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panMenuSchedule.Location = new System.Drawing.Point(0, 0);
			this.panMenuSchedule.Name = "panMenuSchedule";
			this.panMenuSchedule.Size = new System.Drawing.Size(851, 81);
			this.panMenuSchedule.TabIndex = 6;
			// 
			// panel2
			// 
			this.panel2.BackColor = System.Drawing.SystemColors.Window;
			this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel2.Location = new System.Drawing.Point(0, 0);
			this.panel2.Name = "panel2";
			this.panel2.Size = new System.Drawing.Size(851, 81);
			this.panel2.TabIndex = 4;
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(0, 0);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(100, 23);
			this.label1.TabIndex = 0;
			// 
			// editBox1
			// 
			this.editBox1.Location = new System.Drawing.Point(0, 0);
			this.editBox1.Name = "editBox1";
			this.editBox1.Size = new System.Drawing.Size(100, 21);
			this.editBox1.TabIndex = 0;
			this.editBox1.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
			// 
			// SummaryAdControl
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.Controls.Add(this.uiPanelChoiceAdSchedule);
			this.Font = global::AdManagerClient.Properties.Settings.Default.Font1;
			this.Name = "SummaryAdControl";
			this.Size = new System.Drawing.Size(1010, 677);
			this.Load += new System.EventHandler(this.UserControl_Load);
			((System.ComponentModel.ISupportInitialize)(this.uiPM)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.uiPanelChoiceAdSchedule)).EndInit();
			this.uiPanelChoiceAdSchedule.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.uiPanelSearch)).EndInit();
			this.uiPanelSearch.ResumeLayout(false);
			this.uiPanelSearchContainer.ResumeLayout(false);
			this.pnlSearch.ResumeLayout(false);
			this.pnlSearch.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.uiGroupBox1)).EndInit();
			this.uiGroupBox1.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.uiPanelList)).EndInit();
			this.uiPanelList.ResumeLayout(false);
			this.uiPanelListContainer.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.grdExSummary)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.summaryAdDs)).EndInit();
			this.panMenuSchedule.ResumeLayout(false);
			this.ResumeLayout(false);

		}
        #endregion

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
            if(menu.CanRead(MenuCode))
            {
                canRead = true;               
            }

            InitButton();

			ProgressStop();
		}


        private void InitCombo()
        {
            Init_MediaCode();
            InitCombo_Level();

            Init_cbSearchContract();
            Init_cbCampaignCode();
            Init_cbSearchItem();

			// 기간시작일 및 종료일을 금일로 셋트
			cbSearchStartDay.Value = DateTime.Now.AddDays(-1);
            //cbSearchStartDay.Value = DateTime.Now.AddDays(-1);
			
			rbSearchType_D.Checked = true;

			// 어드민&슈퍼유저 등급만 프로파일리포팅 옵션을 활성화 시킨다.
			if (commonModel.UserClass.Equals("10") || commonModel.UserClass.Equals("20"))
			{
				chkProfile.Visible = true;
			}
			else
			{
				chkProfile.Visible = false;
				chkProfile.Checked = false;
			}
		}

        private void Init_MediaCode()
        {
            // 매체를 조회한다.
            MediaCodeModel mediacodeModel = new MediaCodeModel();
            new MediaCodeManager(systemModel, commonModel).GetMediaCodeList(mediacodeModel);
			
            if (mediacodeModel.ResultCD.Equals("0000"))
            {
                // 데이터셋에 셋팅
                Utility.SetDataTable(summaryAdDs.Media, mediacodeModel.MediaCodeDataSet);				
            }

            // 검색조건의 콤보
            this.cbSearchMedia.Items.Clear();
			
            // 콤보박스에 셋트할 코드목록을 담을 Item배열을 선언
            Janus.Windows.EditControls.UIComboBoxItem[] comboItems = new Janus.Windows.EditControls.UIComboBoxItem[mediacodeModel.ResultCnt + 1];

            comboItems[0] = new Janus.Windows.EditControls.UIComboBoxItem("매체선택","00");
			
            for(int i=0;i<mediacodeModel.ResultCnt;i++)
            {
                DataRow row = summaryAdDs.Media.Rows[i];

                string val = row["MediaCode"].ToString();
                string txt = row["MediaName"].ToString();
                comboItems[i+1] = new Janus.Windows.EditControls.UIComboBoxItem(txt,val);
            }
            // 콤보에 셋트
            this.cbSearchMedia.Items.AddRange(comboItems);
            this.cbSearchMedia.SelectedIndex = 0;

            Application.DoEvents();
        }


        /// <summary>
        /// 계약데이터 읽어오기
        /// </summary>
        private void Init_cbSearchContract()
        {

            // 검색조건의 콤보
            this.cbSearchContract.Items.Clear();
			
            // 콤보에 셋트
            this.cbSearchContract.Items.Add(new Janus.Windows.EditControls.UIComboBoxItem("계약 선택","00"));
            this.cbSearchContract.SelectedIndex = 0;

            Application.DoEvents();
        }


        /// <summary>
        /// 캠페인콤보 초기화
        /// </summary>
        private void Init_cbCampaignCode()
        {
            // 검색조건의 콤보
            this.cbCampaign.Items.Clear();

            // 콤보에 셋팅
            this.cbCampaign.Items.Add(new Janus.Windows.EditControls.UIComboBoxItem("캠페인 선택","00"));
            this.cbCampaign.SelectedIndex = 0;
            Application.DoEvents();
        }


        /// <summary>
        /// 내역콤보 초기화
        /// </summary>
        private void Init_cbSearchItem()
        {
            // 검색조건의 콤보
            this.cbSearchItem.Items.Clear();
            this.cbSearchItem.Items.Add(new Janus.Windows.EditControls.UIComboBoxItem("광고 선택","00"));
            this.cbSearchItem.SelectedIndex = 0;

            Application.DoEvents();
        }


        private void InitCombo_Level()
        {

            if(commonModel.UserLevel=="20")
            {
                cbSearchMedia.SelectedValue = commonModel.MediaCode;			
                cbSearchMedia.ReadOnly = true;					
            }
            else
            {
				for(int i=0;i < summaryAdDs.Media.Rows.Count;i++)
				{
					DataRow row = summaryAdDs.Media.Rows[i];					
					if(row["MediaCode"].ToString().Equals(FrameSystem._HANATV.ToString()))
					{
						cbSearchMedia.SelectedValue = FrameSystem._HANATV; // 하나TV를 기본값으로 한다.	 		
						break;															
					}
					else
					{
						cbSearchMedia.SelectedValue="00";
					}
				}	
            }
		
            Application.DoEvents();
        }


        private void InitButton()
        {
			if(canRead)
			{
				btnSearch.Enabled = true;
			}
			
			if(canPrint)
			{
				btnPrint.Enabled = true;
				btnExcel.Enabled = true;
			}

            grdExSummary.Focus();

            Application.DoEvents();
        }



       
        private void DisableButton()
        {
            btnSearch.Enabled	= false;
			btnPrint.Enabled    = false;
			btnExcel.Enabled    = false;

            Application.DoEvents();
        }



        #endregion

        #region 액션처리 메소드


		private void cbSearchMedia_SelectedIndexChanged(object sender, System.EventArgs e)
		{
            Init_cbSearchContract();
            Init_cbCampaignCode();
			Init_cbSearchItem();
		}

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

		private void cbSearchContract_Click(object sender, System.EventArgs e)
		{
			if(cbSearchMedia.SelectedValue.Equals("00"))
			{
				MessageBox.Show("매체를 선택해 주십시오.", "총괄 광고 집계",MessageBoxButtons.OK, MessageBoxIcon.Information);
				return;
			}

			DisableButton();	
			//  지정광고 대상목록 검색창 
			SummaryAd_SearchContractForm pForm = new SummaryAd_SearchContractForm(this);

			// 매체코드셋트
			pForm.keyMediaCode = cbSearchMedia.SelectedValue.ToString();

			pForm.ShowDialog();            
			pForm.Dispose();
			pForm = null;
		
			InitButton();				
		}

		private void btnPrint_Click(object sender, System.EventArgs e)
		{
/*
			//  지정광고 대상목록 검색창 
			SummaryAdReportForm rptForm = new SummaryAdReportForm(this);

			rptForm.ds = summaryAd;
			rptForm.ReportTitle = "프로그램별 광고시청집계"; 
			rptForm.ReportCondition = "매체 : [" + keyMediaName + "]  조회일자 : [" + keyReportDate + "]";

			rptForm.ShowDialog();            
			rptForm.Dispose();
			rptForm = null;
*/
		}


		private void cbSearchItem_SelectedItemChanged(object sender, System.EventArgs e)
		{
			string ItemNo = cbSearchItem.SelectedItem.Value.ToString();

            if(!ItemNo.Equals("00"))
            {
                DataRow[] rows = summaryAdDs.Items.Select("ItemNo = '" + ItemNo + "' ");

                if(rows.Length > 0)
                {
                    itemEndDay      = Utility.reConvertDate(rows[0]["RealEndDay"].ToString());    // 이벤트 처리를 위해 EndDay를 먼저셋트
                    itemStartDay    = Utility.reConvertDate(rows[0]["ExcuteStartDay"].ToString());

                    if(rbSearchType_C.Checked)
                    {
                        if( !cbSearchStartDay.Enabled   )   cbSearchStartDay.Enabled = true;
                        if( !cbSearchEndDay.Enabled     )   cbSearchEndDay.Enabled   = true;

                        if( itemStartDay.Length == 10 && itemEndDay.Length == 10 )
                        {
                            cbSearchStartDay.Value = FrameSystem.ConverStrTotDate( itemStartDay );
                            cbSearchEndDay.Text = itemEndDay;
                        }
                        else
                        {
                            cbSearchStartDay.Value = DateTime.Now.AddDays(-1);
                            cbSearchEndDay.Value  = cbSearchStartDay.Value;
                        }
                    }	
                }
            }
            else
            {
                if(rbSearchType_C.Checked)
                {
                    if( !cbSearchStartDay.Enabled   )   cbSearchStartDay.Enabled = true;
                    if( !cbSearchEndDay.Enabled     )   cbSearchEndDay.Enabled   = true;
                
                    // 계약일때
                    if( contractStartDay.Length == 10 && contractEndDay.Length == 10 )
                    {
                        //cbSearchStartDay.Text = FrameSystem.ConverStrTotDate( contractStartDay );
                        cbSearchStartDay.Text = contractStartDay;
                        cbSearchEndDay.Text = contractEndDay;
                    }
                    else
                    {
                        cbSearchStartDay.Value = DateTime.Now.AddDays(-1);
                        cbSearchEndDay.Value  = cbSearchStartDay.Value;
                    }
                }
            }
		}


        /// <summary>
        /// 집행실적을 기간으로 선택했을때
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
		private void rbSearchType_C_CheckedChanged(object sender, System.EventArgs e)
		{
			if(rbSearchType_C.Checked)
			{
                if( !cbSearchStartDay.Enabled   )   cbSearchStartDay.Enabled = true;
				if( !cbSearchEndDay.Enabled     )   cbSearchEndDay.Enabled   = true;

                if( cbSearchItem.SelectedValue.ToString().Equals("00") )
                {
                    // 계약일때
                    if( contractStartDay.Length == 10 && contractEndDay.Length == 10 )
                    {
                        //cbSearchStartDay.Text = FrameSystem.ConverStrTotDate( contractStartDay );
                        cbSearchStartDay.Text = contractStartDay;
                        cbSearchEndDay.Text = contractEndDay;
                    }
                    else
                    {
                        cbSearchStartDay.Value = DateTime.Now.AddDays(-1);
                        cbSearchEndDay.Value  = cbSearchStartDay.Value;
                    }
                }
                else
                {

                    if( itemStartDay.Length == 10 && itemEndDay.Length == 10 )
                    {
                        cbSearchStartDay.Value = FrameSystem.ConverStrTotDate( itemStartDay );
                        cbSearchEndDay.Text = itemEndDay;
                    }
                    else
                    {
                        cbSearchStartDay.Value = DateTime.Now.AddDays(-1);
                        cbSearchEndDay.Value  = cbSearchStartDay.Value;
                    }
                }

				//if( ebStartDay.Text.Length > 0) cbSearchStartDay.Text = ebStartDay.Text;
			}	
		}


        /// <summary>
        /// 일간을 선택했을때
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
		private void rbSearchType_D_CheckedChanged(object sender, System.EventArgs e)
		{
			if(rbSearchType_D.Checked)
			{
                cbSearchStartDay.Value = DateTime.Now.AddDays(-1);
				cbSearchEndDay.Value  = cbSearchStartDay.Value;
                cbSearchStartDay.Enabled = true;
                cbSearchEndDay.Enabled   = false;
			}			
		}

		private void rbSearchType_W_CheckedChanged(object sender, System.EventArgs e)
		{
			if(rbSearchType_W.Checked)
			{
				cbSearchStartDay.Enabled = true;
				cbSearchEndDay.Enabled   = false;

				DateTime dt = cbSearchStartDay.Value;

				for(int i=0;i<7;i++)
				{
					if(dt.DayOfWeek == System.DayOfWeek.Monday)
					{
						break;
					}
					dt = dt.AddDays(-1);
				}
				cbSearchStartDay.Value = dt;
				cbSearchEndDay.Value   = cbSearchStartDay.Value.AddDays(6);
			}			
		}


		private void rbSearchType_M_CheckedChanged(object sender, System.EventArgs e)
		{
			if(rbSearchType_M.Checked)
			{
				cbSearchStartDay.Enabled = true;
				cbSearchEndDay.Enabled   = false;

				cbSearchStartDay.Text = cbSearchStartDay.Value.ToString("yyyy-MM-01");
				cbSearchEndDay.Value  = cbSearchStartDay.Value.AddMonths(1).AddDays(-1);
			}			
		}


		private void cbSearchStartDay_TextChanged(object sender, System.EventArgs e)
		{
            if(rbSearchType_D.Checked)
			{
				cbSearchEndDay.Value  = cbSearchStartDay.Value;
			}
			else if(rbSearchType_W.Checked)
			{
				cbSearchEndDay.Value  = cbSearchStartDay.Value.AddDays(6);

			}
			else if(rbSearchType_M.Checked)
			{
				cbSearchEndDay.Value  = cbSearchStartDay.Value.AddMonths(1).AddDays(-1);
			}
		}
		#endregion

        #region 처리메소드

        /// <summary>
        /// 일별시청률 조회
        /// </summary>
        private void SearchReport()
        {
            StatusMessage("총괄 광고시청 집계를 조회합니다.");

			if(cbSearchMedia.SelectedValue.Equals("00"))
			{
				MessageBox.Show("매체를 선택해 주십시오.", "총괄 광고노출 집계",	MessageBoxButtons.OK, MessageBoxIcon.Information);
				return;
			}

			if(cbSearchContract.SelectedValue.Equals("00"))
			{
				MessageBox.Show("광고계약을 선택해 주십시오.", "총괄 광고노출 집계",MessageBoxButtons.OK, MessageBoxIcon.Information);
				return;
			}

			ProgressStart();

			try
			{
				// 데이터모델 초기화
				summaryAdModel.Init();

				summaryAdModel.SearchMediaCode	=  cbSearchMedia.SelectedValue.ToString(); 
				summaryAdModel.SearchContractSeq=  cbSearchContract.SelectedValue.ToString(); 
                summaryAdModel.CampaignCode		=  cbCampaign.SelectedValue.ToString(); 
				summaryAdModel.SearchItemNo		=  cbSearchItem.SelectedValue.ToString(); 
				summaryAdModel.SearchStartDay  	=  cbSearchStartDay.Value.ToString("yyMMdd");
				summaryAdModel.SearchEndDay  	=  cbSearchEndDay.Value.ToString("yyMMdd");

                if (chkGenre.Checked)
                    summaryAdModel.MenuLevel        = 2;
                else
                    summaryAdModel.MenuLevel        = 1;

				if(rbSearchType_C.Checked)
				{
					summaryAdModel.SearchType	  	=  "C"; // -- C:계약기간
				}
				else if(rbSearchType_D.Checked)
				{
					summaryAdModel.SearchType	  	=  "D"; // -- D:일간
				}
				else if(rbSearchType_W.Checked)
				{
					summaryAdModel.SearchType	  	=  "W"; // -- W:주간
				}
				else if(rbSearchType_M.Checked)
				{
					summaryAdModel.SearchType	  	=  "M"; // -- M:월간
				}

				// 임시로 항목전용(프로파일링 리포팅 추가)
				// 정식 고도화시 모델을 추가해서 작업하시요.
				// 해당 항목은 결과값으로 사용되는 항목이기에 임시로 전용함
				if (chkProfile.Checked)
				{
					summaryAdModel.TotalUser = 1;
				}
				else
				{
					summaryAdModel.TotalUser = 0;
				}

				keyMediaName      = "";
				keyContractName   = "";
				keyCampaignName   = "";
				keyAgencyName     = "";
				keyAdvertiserName = "";
				keyItemNo         = "";
				keyItemName       = "";
				keyReportBgnDay   = "";
				keyReportEndDay   = "";

				uiPanelList.Text = "시청횟수"; 

				//  일별 광고시청집계조회 서비스를 호출한다.
				new SummaryAdManager(systemModel,commonModel).GetSummaryAdReport(summaryAdModel);

				if (summaryAdModel.ResultCD.Equals("0000"))
				{
					Utility.SetDataTable(summaryAdDs.Report, summaryAdModel.ReportDataSet);		
					StatusMessage(summaryAdModel.ResultCnt + "건이 조회되었습니다.");

					keyTotalUser      = summaryAdModel.TotalUser.ToString();
										
					keyMediaName      = cbSearchMedia.SelectedItem.Text;
					keyContractName   = cbSearchContract.SelectedItem.Text;
					//keyAgencyName     = ebAgency.Text;
					//keyAdvertiserName = ebAdvertiser.Text;
					keyCampaignCode     = cbCampaign.SelectedValue.ToString();
					keyCampaignName     = cbCampaign.SelectedItem.Text;
					keyItemNo         = cbSearchItem.SelectedValue.ToString();
					keyItemName       = cbSearchItem.SelectedItem.Text;
					keyReportBgnDay   = cbSearchStartDay.Value.ToString("yyyy-MM-dd");
					keyReportEndDay   = cbSearchEndDay.Value.ToString("yyyy-MM-dd");
					keyReportType     = summaryAdModel.SearchType;

					ebTotalUser.Text  = keyTotalUser;

					uiPanelList.Text = keyMediaName + " | " + keyContractName + " | " + keyCampaignName + " | " + keyItemName;
                    uiPanelList.Text += " | " + keyReportBgnDay + " ~ " + keyReportEndDay;	
					
					canPrint = true;

				}
			}
			catch(FrameException fe)
			{
				FrameSystem.showMsgForm("총괄 광고노출 집계 조회오류", new string[] {fe.ErrCode, fe.ResultMsg});
			}
			catch(Exception ex)
			{
				FrameSystem.showMsgForm("총괄 광고노출 집계 조회오류",new string[] {"",ex.Message});
			}
			finally
			{
				ProgressStop();
			}
        }


        private void SetcbSearchContract(string ContractSeq, string ContractName)
        {

            // 검색조건의 콤보
            this.cbSearchContract.Items.Clear();
		
            // 콤보에 셋트
            this.cbSearchContract.Items.Add(new Janus.Windows.EditControls.UIComboBoxItem(ContractName,ContractSeq));
            this.cbSearchContract.SelectedIndex = 0;

            Application.DoEvents();
        }

        private void SetcbSearchCampaign()
        {
            // 매체를 조회한다.
            CampaignCodeModel campaigncodeModel = new CampaignCodeModel();
            campaigncodeModel.MediaCode = cbSearchMedia.SelectedValue.ToString();
            campaigncodeModel.ContractSeq = cbSearchContract.SelectedValue.ToString();
            new CampaignCodeManager(systemModel, commonModel).GetCampaignCodeList(campaigncodeModel);
			
            if (campaigncodeModel.ResultCD.Equals("0000"))
            {
                // 데이터셋에 셋팅
                Utility.SetDataTable(summaryAdDs.Campaign, campaigncodeModel.CampaignCodeDataSet);				
            }

            // 검색조건의 콤보
            this.cbCampaign.Items.Clear();
			
            // 콤보박스에 셋트할 코드목록을 담을 Item배열을 선언
            Janus.Windows.EditControls.UIComboBoxItem[] comboItems = new Janus.Windows.EditControls.UIComboBoxItem[campaigncodeModel.ResultCnt + 1];

            if( campaigncodeModel.ResultCnt == 0 )
            {
                comboItems[0] = new Janus.Windows.EditControls.UIComboBoxItem("캠페인 선택","00");
            }
            else
            {			
                comboItems[0] = new Janus.Windows.EditControls.UIComboBoxItem("[전체]","00");

                for(int i=0;i<campaigncodeModel.ResultCnt;i++)
                {
                    DataRow row = summaryAdDs.Campaign.Rows[i];

                    string val = row["CampaignCode"].ToString();
                    string txt = row["CampaignName"].ToString();
                    comboItems[i+1] = new Janus.Windows.EditControls.UIComboBoxItem(txt,val);
                }
            }
            // 콤보에 셋트
            this.cbCampaign.Items.AddRange(comboItems);
            this.cbCampaign.SelectedIndex = 0;

            Application.DoEvents();
        }

        private void SetcbSearchItem(string ContractSeq)
        {
            // 데이터모델 초기화
            summaryAdModel.Init();
            summaryAdModel.SearchContractSeq =  cbSearchContract.SelectedValue.ToString(); 
            summaryAdModel.CampaignCode =  cbCampaign.SelectedValue.ToString(); 

            //  광고계약에 속한 광고내역 리스트 조회 서비스를 호출한다.
            new SummaryAdManager(systemModel,commonModel).GetContractItemList(summaryAdModel);

            if (summaryAdModel.ResultCD.Equals("0000"))
            {
                // 데이터셋에 셋팅
                Utility.SetDataTable(summaryAdDs.Items, summaryAdModel.ItemDataSet);				
            }
            // 검색조건의 콤보
            this.cbSearchItem.Items.Clear();
			
            // 콤보박스에 셋트할 코드목록을 담을 Item배열을 선언
            Janus.Windows.EditControls.UIComboBoxItem[] comboItems = new Janus.Windows.EditControls.UIComboBoxItem[summaryAdModel.ResultCnt + 1];

            comboItems[0] = new Janus.Windows.EditControls.UIComboBoxItem("[전체]","00");
			
            for(int i=0;i<summaryAdModel.ResultCnt;i++)
            {
                DataRow row = summaryAdDs.Items.Rows[i];

                string val = row["ItemNo"].ToString();
                string txt = row["ItemName"].ToString();
                comboItems[i+1] = new Janus.Windows.EditControls.UIComboBoxItem(txt,val);
            }
            // 콤보에 셋트
            this.cbSearchItem.Items.AddRange(comboItems);
            if(summaryAdModel.ResultCnt > 0)
            {
                this.cbSearchItem.SelectedIndex = 1;
            }
            else
            {
                this.cbSearchItem.SelectedIndex = 0;
            }

            Application.DoEvents();
        }

        #endregion

        #region 이벤트함수

        private void StatusMessage(string strMessage)
        {
            if (StatusEvent != null) 
            {
                StatusEventArgs ea = new StatusEventArgs();
                ea.Message   = strMessage;
                StatusEvent(this,ea);
            }
        }

        private void ProgressStart()
        {
            if (ProgressEvent != null) 
            {
                ProgressEventArgs ea = new ProgressEventArgs();
                ea.Type   = ProgressEventArgs.Start;
                ProgressEvent(this,ea);
            }
        }

        private void ProgressStop()
        {
            if (ProgressEvent != null) 
            {
                ProgressEventArgs ea = new ProgressEventArgs();
                ea.Type   = ProgressEventArgs.Stop;
                ProgressEvent(this,ea);
            }
        }
        #endregion

		#region 엑셀 출력
		/// <summary>
		/// 엑셀 생성
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnExcel_Click(object sender, System.EventArgs e)
		{	

			Excel.Application xlApp= null;
			Excel._Workbook   oWB = null;
			Excel._Worksheet  oSheet = null;
			Excel.Range       oRng = null;
			
			try
			{	

				int ColMax  = 11; // 컬럼수   				

				int TitleRow		= 1;
				int ConditionRow	= 2;
				int HeaderRow		= 8;
				int DataRow			= 10;
				string StartCol		= "A";
				string EndCol		= "";
				string TitleCol		= "H";
				int DataCount		= 0;
				int CondCount		= 0;
				int HeaderCount		= 0;
				int TotalRow		= 0;
				int MergeRow		= 0;

				// 마지막 컬럼의 인덱스문자
				EndCol = GetColumnIndex(ColMax);

				xlApp = new Excel.Application();
				oWB = (Excel._Workbook)(xlApp.Workbooks.Add(Missing.Value));
				oSheet = (Excel._Worksheet)oWB.ActiveSheet;


				// 타이틀 작성
				oSheet.Cells[TitleRow,1] = "총괄 광고집행 집계";

				oRng = oSheet.get_Range(StartCol+Convert.ToString(TitleRow), TitleCol+Convert.ToString(TitleRow));
				oRng.Merge(true);
				oRng.Font.Bold = true;
				oRng.Font.Size = 16;
				oRng.VerticalAlignment   = Excel.XlVAlign.xlVAlignCenter;			
				oRng.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;		

				// 조건정보 작성
				oSheet.Cells[ConditionRow+CondCount,1] = "계약명";
				oRng = oSheet.get_Range("B"+Convert.ToString(ConditionRow+CondCount), TitleCol+Convert.ToString(ConditionRow+CondCount));
				oRng.Merge(true);
				oSheet.Cells[ConditionRow+CondCount,2] = keyContractName;
				CondCount++;

				oSheet.Cells[ConditionRow+CondCount,1] = "캠페인";
				oRng = oSheet.get_Range("B"+Convert.ToString(ConditionRow+CondCount), TitleCol+Convert.ToString(ConditionRow+CondCount));
				oRng.Merge(true);
				oSheet.Cells[ConditionRow+CondCount,2] = keyCampaignName;
				CondCount++;

				oSheet.Cells[ConditionRow+CondCount,1] = "광고명";
				oRng = oSheet.get_Range("B"+Convert.ToString(ConditionRow+CondCount), TitleCol+Convert.ToString(ConditionRow+CondCount));
				oRng.Merge(true);
				oSheet.Cells[ConditionRow+CondCount,2] =  "[" + keyItemNo + "] " + keyItemName;
				CondCount++;

				oSheet.Cells[ConditionRow+CondCount,1] = "기간";
				oRng = oSheet.get_Range("B"+Convert.ToString(ConditionRow+CondCount), TitleCol+Convert.ToString(ConditionRow+CondCount));
				oRng.Merge(true);
				string ReportType =  keyReportBgnDay + " ~ " + keyReportEndDay;

				if(keyReportType.Equals("C"))		ReportType += " (기간)";
				else if(keyReportType.Equals("D"))	ReportType += " (일간)";
				else if(keyReportType.Equals("W"))	ReportType += " (주간)";
				else if(keyReportType.Equals("M"))	ReportType += " (월간)";

				oSheet.Cells[ConditionRow+CondCount,2] = ReportType;
				CondCount++;

				oSheet.Cells[ConditionRow+CondCount,1] = "단가";
				oRng = oSheet.get_Range("B"+Convert.ToString(ConditionRow+CondCount), TitleCol+Convert.ToString(ConditionRow+CondCount));
				oRng.Merge(true);
				oSheet.Cells[ConditionRow+CondCount,2] = "";
				CondCount++;

/* 2007.10.08 RH.Jung 엑셀에서는 뺀다
				oSheet.Cells[ConditionRow+CondCount,1] = "하나TV 이용가구수";
				oRng = oSheet.get_Range("B"+Convert.ToString(ConditionRow+CondCount), TitleCol+Convert.ToString(ConditionRow+CondCount));
				oRng.Merge(true);
				oRng.NumberFormatLocal = "#,##0";
				oSheet.Cells[ConditionRow+CondCount,2] = Convert.ToInt32(keyTotalUser);
				CondCount++;
*/

				// 조건부 테두리
				oRng = oSheet.get_Range(StartCol+Convert.ToString(ConditionRow), TitleCol+Convert.ToString(ConditionRow+(CondCount-1)));
				oRng.VerticalAlignment   = Excel.XlVAlign.xlVAlignCenter;			
				oRng.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;		
				oRng.Borders.LineStyle =  Excel.XlLineStyle.xlContinuous;	// 테두리속성 아래에 실선
				oRng.Borders.Weight    = Excel.XlBorderWeight.xlThin;		// 테두리속성 아래에 가는선
                oRng.Font.Size          = 10;


				// 헤더 정보 작성
				oSheet.Cells[HeaderRow,3] = "가구별 광고 시청 조사";
				oRng = oSheet.get_Range(GetColumnIndex(3)+Convert.ToString(HeaderRow), GetColumnIndex(8)+Convert.ToString(HeaderRow));
				oRng.Merge(true);
				oSheet.Cells[HeaderRow,9] = "가구별 프로그램 시청 조사";
				oRng = oSheet.get_Range(GetColumnIndex(9)+Convert.ToString(HeaderRow), GetColumnIndex(11)+Convert.ToString(HeaderRow));
				oRng.Merge(true);

				HeaderCount = 1;
				oSheet.Cells[HeaderRow+1,HeaderCount++] = "항목";
				oSheet.Cells[HeaderRow+1,HeaderCount++] = "세부";
				oSheet.Cells[HeaderRow+1,HeaderCount++] = "광고Hit";
				oSheet.Cells[HeaderRow+1,HeaderCount++] = "비율";
				oSheet.Cells[HeaderRow+1,HeaderCount++] = "시청가구";
// 2007.10.08 RH.Jung 엑셀에서는 뺀다
//				oSheet.Cells[HeaderRow+1,HeaderCount++] = "Reach";
				oSheet.Cells[HeaderRow+1,HeaderCount++] = "Frequency";
				oSheet.Cells[HeaderRow+1,HeaderCount++] = "GRP";
				oSheet.Cells[HeaderRow+1,HeaderCount++] = "CPM";
				oSheet.Cells[HeaderRow+1,HeaderCount++] = "프로그램수";
				oSheet.Cells[HeaderRow+1,HeaderCount++] = "프로그램Hit";
				oSheet.Cells[HeaderRow+1,HeaderCount++] = "비율";


				oRng = oSheet.get_Range(StartCol+Convert.ToString(HeaderRow), EndCol+Convert.ToString(HeaderRow+1)); // 헤더의 범위
				oRng.Font.Bold           = true;							// 폰트 굵게
				oRng.VerticalAlignment   = Excel.XlVAlign.xlVAlignCenter;	// 세로중앙정렬
				oRng.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;	// 가로중앙정렬	 
				oRng.Interior.Color      = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.CornflowerBlue);   //셀 배경색 
				oRng.Font.Color          = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.White);	  //텍스트색			
				

				DataCount = 0;
				// 데이터 추출
				string OldTypeCode = "";
				int TypeMergyStart = 0;
				int MergeCount = 0;
				for (int inx =0; inx < summaryAdDs.Report.Rows.Count; inx++)
				{

					DataRow Row = summaryAdDs.Report.Rows[inx];			
					
					string TypeCode = Row["TypeCode"].ToString(); // 항목구분코드

					 // 합계 Row를 위함
					if(TypeCode.Equals("2")) // 2:합계
					{
						TotalRow = DataCount;
					}

					// 빈영역 머지를 위함
					if(!TypeCode.Equals("1") && !TypeCode.Equals("2") && !TypeCode.Equals("3") && MergeRow == 0) // 4:초수 부터 머지
					{
						MergeRow = DataCount;
					}

					// 항목 머지를 위함
					if(!OldTypeCode.Equals("") && !OldTypeCode.Equals(TypeCode))
					{
						if(MergeCount > 1)
						{
							oRng = oSheet.get_Range("A"+Convert.ToString(DataRow + TypeMergyStart), "A"+Convert.ToString(DataRow + DataCount-1));
							oRng.Merge(false);
						}
						TypeMergyStart = DataCount;
						MergeCount = 0;
					}
					OldTypeCode = TypeCode;
					MergeCount++;

					int ColCnt = 1;
					if(MergeCount == 1)
					{
						oSheet.Cells[DataRow+DataCount,ColCnt++] = Row["SumType"].ToString();	// 항목
					}
					else
					{
						ColCnt++;
					}
					oSheet.Cells[DataRow+DataCount,ColCnt++] = Row["SumName"].ToString();	// 세부

					if(Row["AdCnt"].ToString().Length > 0)
					{
						oSheet.Cells[DataRow+DataCount,ColCnt++] = Convert.ToInt32(Row["AdCnt"].ToString());	// 광고Hit
					}
					else
					{
						ColCnt++;
					}
					if(Row["AdRate"].ToString().Length > 0)
					{
						oSheet.Cells[DataRow+DataCount,ColCnt++] = Convert.ToDecimal(Row["AdRate"].ToString());	// 비율
					}
					else
					{	
						ColCnt++;
					}
					if(Row["HsCnt"].ToString().Length > 0)
					{
						oSheet.Cells[DataRow+DataCount,ColCnt++] = Convert.ToInt32(Row["HsCnt"].ToString());	// 광고시청가구
					}
					else
					{
						ColCnt++;
					}
/* 2007.10.08 RH.Jung Reach는 엑셀파일에 표시하지 않는다.
 					if(Row["Reach"].ToString().Length > 0)
					{
						oSheet.Cells[DataRow+DataCount,ColCnt++] = Convert.ToDecimal(Row["Reach"].ToString());	// Reach
					}
					else
					{
						ColCnt++;
					}					
*/
					if(Row["Freq"].ToString().Length > 0)
					{
						oSheet.Cells[DataRow+DataCount,ColCnt++] = Convert.ToDecimal(Row["Freq"].ToString());	// Frequency
					}
					else
					{
						ColCnt++;
					}
					if(Row["GRP"].ToString().Length > 0)
					{
						oSheet.Cells[DataRow+DataCount,ColCnt++] = Convert.ToDecimal(Row["GRP"].ToString());	// GRP
					}
					else
					{
						ColCnt++;
					}

					if(Row["CPM"].ToString().Length > 0)
						oSheet.Cells[DataRow+DataCount,ColCnt++] = Convert.ToDecimal(Row["CPM"].ToString());	// GRP
					else
						ColCnt++;

					if(Row["PgCnt"].ToString().Length > 0)
					{
						oSheet.Cells[DataRow+DataCount,ColCnt++] = Convert.ToInt32(Row["PgCnt"].ToString());	// 프로그램수
					}
					else
					{
						ColCnt++;
					}
					if(Row["HitCnt"].ToString().Length > 0)
					{
						oSheet.Cells[DataRow+DataCount,ColCnt++] = Convert.ToInt32(Row["HitCnt"].ToString());	// 프로그램Hit
					}
					else
					{
						ColCnt++;
					}
					if(Row["PgRate"].ToString().Length > 0)
					{
						oSheet.Cells[DataRow+DataCount,ColCnt++] = Convert.ToDecimal(Row["PgRate"].ToString());	// 비율
					}
					else
					{
						ColCnt++;
					}

					DataCount++;
				}

				if(MergeCount > 1)
				{
					oRng = oSheet.get_Range("A"+Convert.ToString(DataRow + TypeMergyStart), "A"+Convert.ToString(DataRow + DataCount-1));
					oRng.Merge(false);
				}

				DataCount--;

				//합계
				oRng = oSheet.get_Range(StartCol+Convert.ToString(DataRow+TotalRow), EndCol+Convert.ToString(DataRow+TotalRow)); // 합계 범위
				oRng.Font.Bold           = true;							// 폰트 굵게
				oRng.Interior.Color      = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.LightSteelBlue);   //셀 배경색 

				//합계 머지
				oRng = oSheet.get_Range("A"+Convert.ToString(DataRow + TotalRow), "B"+Convert.ToString(DataRow + TotalRow));
				oRng.Merge(true);

				//빈영역 색상
				oRng = oSheet.get_Range("E"+Convert.ToString(DataRow + MergeRow), "G"+Convert.ToString(DataRow + DataCount));
				oRng.Interior.Color      = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.WhiteSmoke);   //셀 배경색 
//				oRng.Merge(false);


				//항목 중앙정렬
				oRng = oSheet.get_Range("A"+Convert.ToString(DataRow), "A"+Convert.ToString(DataRow+DataCount)); // 일자
				oRng.VerticalAlignment   = Excel.XlVAlign.xlVAlignCenter;	// 세로중앙정렬
				oRng.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;	// 가로중앙정렬	 

				// 데이터 작성

                oRng = oSheet.get_Range(StartCol+Convert.ToString(HeaderRow), EndCol+Convert.ToString(DataRow+DataCount));
                oRng.Font.Size = 9;
                oRng.RowHeight = 14;

				oRng = oSheet.get_Range(StartCol+Convert.ToString(HeaderRow), EndCol+Convert.ToString(DataRow+DataCount));	// 데이터의 범위
				oRng.EntireColumn.AutoFit();					// 데이터의 크기에 셀의 가로크기 맞춤
				oRng.Borders.LineStyle =  Excel.XlLineStyle.xlContinuous;	// 테두리속성 아래에 실선
				oRng.Borders.Weight    = Excel.XlBorderWeight.xlThin;		// 테두리속성 아래에 가는선

				// 광고Hit 데이터에 셀타입 설정
				oRng = oSheet.get_Range("C"+Convert.ToString(DataRow), "C"+Convert.ToString(DataRow+DataCount));	// 데이터의 범위
				oRng.NumberFormatLocal = "#,##0";

				// 비율 데이터에 셀타입 설정
				oRng = oSheet.get_Range("D"+Convert.ToString(DataRow), "D"+Convert.ToString(DataRow+DataCount));	// 데이터의 범위
				oRng.NumberFormatLocal = "#,##0.00";

				// 광고시청가구 데이터에 셀타입 설정
				oRng = oSheet.get_Range("E"+Convert.ToString(DataRow), "E"+Convert.ToString(DataRow+DataCount));	// 데이터의 범위
				oRng.NumberFormatLocal = "#,##0";

				// Frequency & GRP 데이터에 셀타입 설정
				oRng = oSheet.get_Range("F"+Convert.ToString(DataRow), "G"+Convert.ToString(DataRow+DataCount));	// 데이터의 범위
				oRng.NumberFormatLocal = "#,##0.00";

				// 프로그램수 & 프로그램Hit 데이터에 셀타입 설정
				oRng = oSheet.get_Range("H"+Convert.ToString(DataRow), "J"+Convert.ToString(DataRow+DataCount));	// 데이터의 범위
				oRng.NumberFormatLocal = "#,##0";

				// 비율 데이터에 셀타입 설정
				oRng = oSheet.get_Range("K"+Convert.ToString(DataRow), "K"+Convert.ToString(DataRow+DataCount));	// 데이터의 범위
				oRng.NumberFormatLocal = "#,##0.00";
				
				xlApp.Visible = true;
				xlApp.UserControl = true;
			}
			catch(Exception ex)
			{
				MessageBox.Show(ex.Message);
			}

		}

		private string GetColumnIndex(int ColCount)
		{
			string[] ColName = {"Z","A","B","C","D","E","F","G","H","I","J","K","L","M","N","O","P","Q","R","S","T","U","V","W","X","Y"};

			string ColumnIndex;

			// 26보다 크면
			if(ColCount > ColName.Length)
			{
				// 2자리 인덱스문자 26 => Z;  27->AA
				ColumnIndex = ColName[(int)(Math.Floor((float)(ColCount/ColName.Length)))] + ColName[(int)(Math.Floor((float)(ColCount%ColName.Length)))];
			}
			else
			{
				ColumnIndex = ColName[(int)(Math.Floor((float)(ColCount%ColName.Length)))];
			}

			return ColumnIndex;
		}

		#endregion

		#region 팝업창을 위한 메소드
			
		public void SetContract(string ContractSeq, string ContractName, string StartDay, string EndDay, string AgencyName, string AdvertiserName)
		{
			SetcbSearchContract(ContractSeq, ContractName);
			SetcbSearchCampaign();

			contractEndDay   = Utility.reConvertDate(EndDay);	// 이벤트 처리를 위해 EndDay를 먼저 셋트한다.
			contractStartDay = Utility.reConvertDate(StartDay);
			//ebAgency.Text     = AgencyName;
			//ebAdvertiser.Text = AdvertiserName; 

			summaryAdDs.Report.Clear();

			canPrint = false;
			InitButton();
		}

		#endregion

		private void cbCampaign_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			SetcbSearchItem("");
		}

		private void grdExSummary_FormattingRow(object sender, Janus.Windows.GridEX.RowLoadEventArgs e)
		{
		
		}
	}
}
