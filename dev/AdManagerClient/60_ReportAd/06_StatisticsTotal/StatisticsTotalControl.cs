// ===============================================================================
//
// StatisticsTotalControl.cs
//
// 광고별 시청현황 집계 컨트롤을 정의합니다. 
//
// ===============================================================================
// Release history
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
using System.Drawing.Drawing2D;

using WinFramework.Misc;
using WinFramework.Base;
using WinFramework.Data;

using AdManagerModel;
using Excel = Microsoft.Office.Interop.Excel; // 엑셀 참조
using System.Reflection;

namespace AdManagerClient
{
    /// <summary>
    /// 광고별 시청현황 집계 관리 컨트롤
    /// </summary>
    public class StatisticsTotalControl : System.Windows.Forms.UserControl, IUserControl
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
        StatisticsTotalModel statisticsTotalModel  = new StatisticsTotalModel();	// 프로그램별 광고시청집계모델

        // 화면처리용 변수
		bool IsNewSearchKey		  = true;					// 검색어입력 여부
		bool canRead			  = false;

        // Key 데이터
		string keyMediaName    = "";
		string keyReportDay = "";

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
		private Janus.Windows.EditControls.UIButton btnPrint;
		private Janus.Windows.EditControls.UIButton btnExcel;
		private System.Data.DataView dvReport;
		private Janus.Windows.GridEX.EditControls.EditBox ebSearchKey;
		private Janus.Windows.GridEX.GridEX grdExRepoert;
		private Janus.Windows.GridEX.GridEX grdExReport;
		private Janus.Windows.EditControls.UIComboBox cbSearchRap;
		private Janus.Windows.EditControls.UIComboBox cbSearchAgency;
		private Janus.Windows.UI.Dock.UIPanelManager uiPM;
		private AdManagerClient.StatisticsTotal statisticsTotalDs;
        private System.ComponentModel.IContainer components;

        public StatisticsTotalControl()
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
			Janus.Windows.GridEX.GridEXLayout grdExReport_DesignTimeLayout = new Janus.Windows.GridEX.GridEXLayout();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(StatisticsTotalControl));
			this.uiPM = new Janus.Windows.UI.Dock.UIPanelManager(this.components);
			this.uiPanelChoiceAdSchedule = new Janus.Windows.UI.Dock.UIPanelGroup();
			this.uiPanelSearch = new Janus.Windows.UI.Dock.UIPanel();
			this.uiPanelSearchContainer = new Janus.Windows.UI.Dock.UIPanelInnerContainer();
			this.pnlSearch = new System.Windows.Forms.Panel();
			this.cbSearchAgency = new Janus.Windows.EditControls.UIComboBox();
			this.cbSearchRap = new Janus.Windows.EditControls.UIComboBox();
			this.ebSearchKey = new Janus.Windows.GridEX.EditControls.EditBox();
			this.cbSearchMedia = new Janus.Windows.EditControls.UIComboBox();
			this.btnSearch = new Janus.Windows.EditControls.UIButton();
			this.btnExcel = new Janus.Windows.EditControls.UIButton();
			this.btnPrint = new Janus.Windows.EditControls.UIButton();
			this.uiPanelList = new Janus.Windows.UI.Dock.UIPanel();
			this.uiPanelListContainer = new Janus.Windows.UI.Dock.UIPanelInnerContainer();
			this.grdExReport = new Janus.Windows.GridEX.GridEX();
			this.dvReport = new System.Data.DataView();
			this.statisticsTotalDs = new AdManagerClient.StatisticsTotal();
			this.grdExRepoert = new Janus.Windows.GridEX.GridEX();
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
			((System.ComponentModel.ISupportInitialize)(this.uiPanelList)).BeginInit();
			this.uiPanelList.SuspendLayout();
			this.uiPanelListContainer.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.grdExReport)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.dvReport)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.statisticsTotalDs)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.grdExRepoert)).BeginInit();
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
			this.uiPM.AddDockPanelInfo(new System.Guid("afb3b7d0-aa79-443c-a1e8-3baba38f7a4d"), new System.Guid("b162c53a-7940-47cd-a0ed-9762c59c50d8"), 42, true);
			this.uiPM.AddDockPanelInfo(new System.Guid("8e3140b9-bfcb-43af-b84e-2ee6cddf51dc"), new System.Guid("b162c53a-7940-47cd-a0ed-9762c59c50d8"), 589, true);
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
			this.uiPanelChoiceAdSchedule.Text = "전체통계";
			// 
			// uiPanelSearch
			// 
			this.uiPanelSearch.AllowResize = Janus.Windows.UI.InheritableBoolean.False;
			this.uiPanelSearch.CaptionVisible = Janus.Windows.UI.InheritableBoolean.False;
			this.uiPanelSearch.CloseButtonVisible = Janus.Windows.UI.InheritableBoolean.False;
			this.uiPanelSearch.InnerContainer = this.uiPanelSearchContainer;
			this.uiPanelSearch.Location = new System.Drawing.Point(0, 22);
			this.uiPanelSearch.Name = "uiPanelSearch";
			this.uiPanelSearch.Size = new System.Drawing.Size(1010, 43);
			this.uiPanelSearch.TabIndex = 4;
			this.uiPanelSearch.Text = "검색";
			// 
			// uiPanelSearchContainer
			// 
			this.uiPanelSearchContainer.Controls.Add(this.pnlSearch);
			this.uiPanelSearchContainer.Location = new System.Drawing.Point(1, 1);
			this.uiPanelSearchContainer.Name = "uiPanelSearchContainer";
			this.uiPanelSearchContainer.Size = new System.Drawing.Size(1008, 41);
			this.uiPanelSearchContainer.TabIndex = 0;
			// 
			// pnlSearch
			// 
			this.pnlSearch.BackColor = System.Drawing.SystemColors.Window;
			this.pnlSearch.Controls.Add(this.cbSearchAgency);
			this.pnlSearch.Controls.Add(this.cbSearchRap);
			this.pnlSearch.Controls.Add(this.ebSearchKey);
			this.pnlSearch.Controls.Add(this.cbSearchMedia);
			this.pnlSearch.Controls.Add(this.btnSearch);
			this.pnlSearch.Controls.Add(this.btnExcel);
			this.pnlSearch.Controls.Add(this.btnPrint);
			this.pnlSearch.Dock = System.Windows.Forms.DockStyle.Fill;
			this.pnlSearch.Location = new System.Drawing.Point(0, 0);
			this.pnlSearch.Name = "pnlSearch";
			this.pnlSearch.Size = new System.Drawing.Size(1008, 41);
			this.pnlSearch.TabIndex = 3;
			// 
			// cbSearchAgency
			// 
			this.cbSearchAgency.ComboStyle = Janus.Windows.EditControls.ComboStyle.DropDownList;
			this.cbSearchAgency.Location = new System.Drawing.Point(256, 8);
			this.cbSearchAgency.Name = "cbSearchAgency";
			this.cbSearchAgency.Size = new System.Drawing.Size(120, 21);
			this.cbSearchAgency.TabIndex = 3;
			this.cbSearchAgency.Text = "대행사선택";
			this.cbSearchAgency.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
			// 
			// cbSearchRap
			// 
			this.cbSearchRap.ComboStyle = Janus.Windows.EditControls.ComboStyle.DropDownList;
			this.cbSearchRap.Location = new System.Drawing.Point(128, 8);
			this.cbSearchRap.Name = "cbSearchRap";
			this.cbSearchRap.Size = new System.Drawing.Size(120, 21);
			this.cbSearchRap.TabIndex = 2;
			this.cbSearchRap.Text = "미디어렙선택";
			this.cbSearchRap.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
			// 
			// ebSearchKey
			// 
			this.ebSearchKey.Location = new System.Drawing.Point(384, 8);
			this.ebSearchKey.Name = "ebSearchKey";
			this.ebSearchKey.Size = new System.Drawing.Size(120, 21);
			this.ebSearchKey.TabIndex = 4;
			this.ebSearchKey.Text = "검색어를 입력하세요";
			this.ebSearchKey.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
			this.ebSearchKey.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
			this.ebSearchKey.TextChanged += new System.EventHandler(this.ebSearchKey_TextChanged);
			this.ebSearchKey.Click += new System.EventHandler(this.ebSearchKey_Click);
			this.ebSearchKey.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ebSearchKey_KeyDown);
			// 
			// cbSearchMedia
			// 
			this.cbSearchMedia.BackColor = System.Drawing.Color.White;
			this.cbSearchMedia.ComboStyle = Janus.Windows.EditControls.ComboStyle.DropDownList;
			this.cbSearchMedia.Location = new System.Drawing.Point(8, 8);
			this.cbSearchMedia.Name = "cbSearchMedia";
			this.cbSearchMedia.Size = new System.Drawing.Size(112, 21);
			this.cbSearchMedia.TabIndex = 1;
			this.cbSearchMedia.Text = "매체선택";
			this.cbSearchMedia.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
			// 
			// btnSearch
			// 
			this.btnSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnSearch.Enabled = false;
			this.btnSearch.Location = new System.Drawing.Point(783, 8);
			this.btnSearch.Name = "btnSearch";
			this.btnSearch.Size = new System.Drawing.Size(104, 24);
			this.btnSearch.TabIndex = 6;
			this.btnSearch.Text = "조 회";
			this.btnSearch.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
			this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
			// 
			// btnExcel
			// 
			this.btnExcel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnExcel.Enabled = false;
			this.btnExcel.Location = new System.Drawing.Point(895, 8);
			this.btnExcel.Name = "btnExcel";
			this.btnExcel.Size = new System.Drawing.Size(104, 24);
			this.btnExcel.TabIndex = 7;
			this.btnExcel.Text = "EXCEL 출력";
			this.btnExcel.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
			this.btnExcel.Click += new System.EventHandler(this.btnExcel_Click);
			// 
			// btnPrint
			// 
			this.btnPrint.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnPrint.Enabled = false;
			this.btnPrint.Location = new System.Drawing.Point(671, 8);
			this.btnPrint.Name = "btnPrint";
			this.btnPrint.Size = new System.Drawing.Size(104, 24);
			this.btnPrint.TabIndex = 5;
			this.btnPrint.Text = "리포트";
			this.btnPrint.Visible = false;
			this.btnPrint.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
			this.btnPrint.Click += new System.EventHandler(this.btnPrint_Click);
			// 
			// uiPanelList
			// 
			this.uiPanelList.BackColor = System.Drawing.SystemColors.Window;
			this.uiPanelList.CaptionStyle = Janus.Windows.UI.Dock.PanelCaptionStyle.Dark;
			this.uiPanelList.InnerContainer = this.uiPanelListContainer;
			this.uiPanelList.Location = new System.Drawing.Point(0, 69);
			this.uiPanelList.MinimumSize = new System.Drawing.Size(-1, 100);
			this.uiPanelList.Name = "uiPanelList";
			this.uiPanelList.Size = new System.Drawing.Size(1010, 608);
			this.uiPanelList.TabIndex = 4;
			this.uiPanelList.Text = "통계";
			// 
			// uiPanelListContainer
			// 
			this.uiPanelListContainer.BackColor = System.Drawing.SystemColors.Window;
			this.uiPanelListContainer.Controls.Add(this.grdExReport);
			this.uiPanelListContainer.Location = new System.Drawing.Point(1, 23);
			this.uiPanelListContainer.Name = "uiPanelListContainer";
			this.uiPanelListContainer.Size = new System.Drawing.Size(1008, 584);
			this.uiPanelListContainer.TabIndex = 0;
			// 
			// grdExReport
			// 
			this.grdExReport.AllowEdit = Janus.Windows.GridEX.InheritableBoolean.False;
			this.grdExReport.AlternatingColors = true;
			this.grdExReport.BorderStyle = Janus.Windows.GridEX.BorderStyle.None;
			this.grdExReport.DataSource = this.dvReport;
			grdExReport_DesignTimeLayout.LayoutString = resources.GetString("grdExReport_DesignTimeLayout.LayoutString");
			this.grdExReport.DesignTimeLayout = grdExReport_DesignTimeLayout;
			this.grdExReport.Dock = System.Windows.Forms.DockStyle.Fill;
			this.grdExReport.EditorsControlStyle.ButtonAppearance = Janus.Windows.GridEX.ButtonAppearance.Regular;
			this.grdExReport.EmptyRows = true;
			this.grdExReport.FocusCellFormatStyle.BackColor = System.Drawing.SystemColors.Highlight;
			this.grdExReport.FocusCellFormatStyle.ForeColor = System.Drawing.Color.White;
			this.grdExReport.FocusStyle = Janus.Windows.GridEX.FocusStyle.None;
			this.grdExReport.FrozenColumns = 2;
			this.grdExReport.GridLineColor = System.Drawing.Color.Silver;
			this.grdExReport.GridLines = Janus.Windows.GridEX.GridLines.Vertical;
			this.grdExReport.GridLineStyle = Janus.Windows.GridEX.GridLineStyle.Solid;
			this.grdExReport.GroupByBoxInfoFormatStyle.BackColor = System.Drawing.Color.Empty;
			this.grdExReport.GroupByBoxVisible = false;
			this.grdExReport.GroupRowFormatStyle.BackColor = System.Drawing.Color.White;
			this.grdExReport.GroupRowFormatStyle.BackColorGradient = System.Drawing.Color.FromArgb(((int)(((byte)(218)))), ((int)(((byte)(228)))), ((int)(((byte)(238)))));
			this.grdExReport.GroupRowFormatStyle.FontBold = Janus.Windows.GridEX.TriState.True;
			this.grdExReport.GroupRowVisualStyle = Janus.Windows.GridEX.GroupRowVisualStyle.Outlook2003;
			this.grdExReport.GroupTotalRowFormatStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
			this.grdExReport.GroupTotalRowFormatStyle.FontBold = Janus.Windows.GridEX.TriState.True;
			this.grdExReport.GroupTotalRowFormatStyle.ForeColor = System.Drawing.SystemColors.ControlText;
			this.grdExReport.HeaderFormatStyle.TextAlignment = Janus.Windows.GridEX.TextAlignment.Center;
			this.grdExReport.HideSelection = Janus.Windows.GridEX.HideSelection.Highlight;
			this.grdExReport.Location = new System.Drawing.Point(0, 0);
			this.grdExReport.Name = "grdExReport";
			this.grdExReport.ScrollBars = Janus.Windows.GridEX.ScrollBars.Vertical;
			this.grdExReport.SelectedInactiveFormatStyle.BackColor = System.Drawing.Color.Gold;
			this.grdExReport.Size = new System.Drawing.Size(1008, 584);
			this.grdExReport.TabIndex = 8;
			this.grdExReport.TabKeyBehavior = Janus.Windows.GridEX.TabKeyBehavior.ControlNavigation;
			this.grdExReport.ThemedAreas = ((Janus.Windows.GridEX.ThemedArea)(((((((((Janus.Windows.GridEX.ThemedArea.ScrollBars | Janus.Windows.GridEX.ThemedArea.EditControls) 
            | Janus.Windows.GridEX.ThemedArea.Headers) 
            | Janus.Windows.GridEX.ThemedArea.GroupByBox) 
            | Janus.Windows.GridEX.ThemedArea.GroupRows) 
            | Janus.Windows.GridEX.ThemedArea.ControlBorder) 
            | Janus.Windows.GridEX.ThemedArea.Cards) 
            | Janus.Windows.GridEX.ThemedArea.Gridlines) 
            | Janus.Windows.GridEX.ThemedArea.CheckBoxes)));
			this.grdExReport.TotalRowFormatStyle.BackColor = System.Drawing.Color.Gold;
			this.grdExReport.TotalRowFormatStyle.FontBold = Janus.Windows.GridEX.TriState.True;
			this.grdExReport.TotalRowFormatStyle.ForeColor = System.Drawing.SystemColors.ControlText;
			this.grdExReport.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
			this.grdExReport.DrawGridArea += new Janus.Windows.GridEX.DrawGridAreaEventHandler(this.grdExReport_DrawGridArea);
			// 
			// dvReport
			// 
			this.dvReport.Table = this.statisticsTotalDs.Report;
			// 
			// statisticsTotalDs
			// 
			this.statisticsTotalDs.DataSetName = "StatisticsTotalDs";
			this.statisticsTotalDs.Locale = new System.Globalization.CultureInfo("en-US");
			this.statisticsTotalDs.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
			// 
			// grdExRepoert
			// 
			this.grdExRepoert.Location = new System.Drawing.Point(0, 0);
			this.grdExRepoert.Name = "grdExRepoert";
			this.grdExRepoert.Size = new System.Drawing.Size(400, 376);
			this.grdExRepoert.TabIndex = 0;
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
			this.editBox1.Size = new System.Drawing.Size(100, 20);
			this.editBox1.TabIndex = 0;
			this.editBox1.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
			// 
			// StatisticsTotalControl
			// 
			this.Controls.Add(this.uiPanelChoiceAdSchedule);
			this.Font = global::AdManagerClient.Properties.Settings.Default.Font1;
			this.Name = "StatisticsTotalControl";
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
			((System.ComponentModel.ISupportInitialize)(this.uiPanelList)).EndInit();
			this.uiPanelList.ResumeLayout(false);
			this.uiPanelListContainer.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.grdExReport)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.dvReport)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.statisticsTotalDs)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.grdExRepoert)).EndInit();
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
			Init_RapCode();
			Init_AgencyCode();
            InitCombo_Level();

		}

        private void Init_MediaCode()
        {
            // 매체를 조회한다.
            MediaCodeModel mediacodeModel = new MediaCodeModel();
            new MediaCodeManager(systemModel, commonModel).GetMediaCodeList(mediacodeModel);
			
            if (mediacodeModel.ResultCD.Equals("0000"))
            {
                // 데이터셋에 셋팅
                Utility.SetDataTable(statisticsTotalDs.Media, mediacodeModel.MediaCodeDataSet);				
            }

            // 검색조건의 콤보
            this.cbSearchMedia.Items.Clear();
			
            // 콤보박스에 셋트할 코드목록을 담을 Item배열을 선언
            Janus.Windows.EditControls.UIComboBoxItem[] comboItems = new Janus.Windows.EditControls.UIComboBoxItem[mediacodeModel.ResultCnt + 1];

            comboItems[0] = new Janus.Windows.EditControls.UIComboBoxItem("매체선택","00");
			
            for(int i=0;i<mediacodeModel.ResultCnt;i++)
            {
                DataRow row = statisticsTotalDs.Media.Rows[i];

                string val = row["MediaCode"].ToString();
                string txt = row["MediaName"].ToString();
                comboItems[i+1] = new Janus.Windows.EditControls.UIComboBoxItem(txt,val);
            }
            // 콤보에 셋트
            this.cbSearchMedia.Items.AddRange(comboItems);
            this.cbSearchMedia.SelectedIndex = 0;

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
				Utility.SetDataTable(statisticsTotalDs.MediaRaps, mediaRapCodeModel.MediaRapCodeDataSet);				
			}

			// 검색조건의 콤보
			this.cbSearchRap.Items.Clear();
           
			// 콤보박스에 셋트할 코드목록을 담을 Item배열을 선언
			Janus.Windows.EditControls.UIComboBoxItem[] comboItems = new Janus.Windows.EditControls.UIComboBoxItem[mediaRapCodeModel.ResultCnt + 1];

			comboItems[0] = new Janus.Windows.EditControls.UIComboBoxItem("미디어렙선택","00");
			
			for(int i=0;i<mediaRapCodeModel.ResultCnt;i++)
			{
				DataRow row = statisticsTotalDs.MediaRaps.Rows[i];

				string val = row["RapCode"].ToString();
				string txt = row["RapName"].ToString();
				comboItems[i+1] = new Janus.Windows.EditControls.UIComboBoxItem(txt,val);
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
				Utility.SetDataTable(statisticsTotalDs.Agencys, agencyCodeModel.AgencyCodeDataSet);				
			}

			// 검색조건의 콤보
			this.cbSearchAgency.Items.Clear();
			
			// 콤보박스에 셋트할 코드목록을 담을 Item배열을 선언
			Janus.Windows.EditControls.UIComboBoxItem[] comboItems = new Janus.Windows.EditControls.UIComboBoxItem[agencyCodeModel.ResultCnt + 1];

			comboItems[0] = new Janus.Windows.EditControls.UIComboBoxItem("대행사선택","00");
			
			for(int i=0;i<agencyCodeModel.ResultCnt;i++)
			{
				DataRow row = statisticsTotalDs.Agencys.Rows[i];

				string val = row["AgencyCode"].ToString();
				string txt = row["AgencyName"].ToString();
				comboItems[i+1] = new Janus.Windows.EditControls.UIComboBoxItem(txt,val);
			}
			
			// 콤보에 셋트
			this.cbSearchAgency.Items.AddRange(comboItems);
			this.cbSearchAgency.SelectedIndex = 0;

			Application.DoEvents();
		}

		private void InitCombo_Level()
		{
			if(commonModel.UserLevel == "20")
			{
				// 콤보픽스						
				cbSearchMedia.SelectedValue = commonModel.MediaCode;			
				cbSearchMedia.ReadOnly = true;				            
			}
			else
			{
				for(int i=0;i < statisticsTotalDs.Media.Rows.Count;i++)
				{
					DataRow row = statisticsTotalDs.Media.Rows[i];					
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
			if (commonModel.UserLevel == "30")
			{
				cbSearchRap.SelectedValue = commonModel.RapCode;
				cbSearchRap.ReadOnly = true;
			}
			if (commonModel.UserLevel == "40")
			{
				cbSearchAgency.SelectedValue = commonModel.AgencyCode;
				cbSearchAgency.ReadOnly = true;
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

            grdExReport.Focus();

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

		/// <summary>
		/// 검색어 변경
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void ebSearchKey_TextChanged(object sender, System.EventArgs e)
		{
			IsNewSearchKey = false;
		}

		/// <summary>
		/// 검색어 클릭 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void ebSearchKey_Click(object sender, System.EventArgs e)
		{
			if(IsNewSearchKey)
			{
				ebSearchKey.Text = "";
			}
			else
			{
				ebSearchKey.SelectAll();
			}
		}

		private void ebSearchKey_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
		{
			if(e.KeyCode == Keys.Enter)
			{
				SearchReport();
			}
		}

		private void btnPrint_Click(object sender, System.EventArgs e)
		{
/*
			//  지정광고 대상목록 검색창 
			StatisticsTotalReportForm rptForm = new StatisticsTotalReportForm(this);

			rptForm.ds = statisticsTotal;
			rptForm.ReportTitle = "프로그램별 광고시청집계"; 
			rptForm.ReportCondition = "매체 : [" + keyMediaName + "]  조회일자 : [" + keyReportDate + "]";

			rptForm.ShowDialog();            
			rptForm.Dispose();
			rptForm = null;
*/
		}


		#endregion

        #region 처리메소드

        /// <summary>
        /// 전체통계 조회
        /// </summary>
        private void SearchReport()
        {
            StatusMessage("전체통계을 조회합니다.");

			if(cbSearchMedia.SelectedValue.Equals("00"))
			{
				MessageBox.Show("매체를 선택해 주십시오.", "전체통계",
					MessageBoxButtons.OK, MessageBoxIcon.Information);
				return;
			}

			ProgressStart();

			try
			{
				// 데이터모델 초기화
				statisticsTotalModel.Init();

				if(IsNewSearchKey)
				{
					statisticsTotalModel.SearchKey = "";
				}
				else
				{
					statisticsTotalModel.SearchKey  = ebSearchKey.Text;
				}
				statisticsTotalModel.SearchMediaCode  =  cbSearchMedia.SelectedValue.ToString(); 
				statisticsTotalModel.SearchRapCode    =  cbSearchRap.SelectedValue.ToString(); 
				statisticsTotalModel.SearchAgencyCode =  cbSearchAgency.SelectedValue.ToString(); 


				keyMediaName      = "";
				keyReportDay   = "";

				uiPanelList.Text = "전체통계"; 

				//  전체통계 서비스를 호출한다.
				new StatisticsTotalManager(systemModel,commonModel).GetStatisticsTotal(statisticsTotalModel);

				if (statisticsTotalModel.ResultCD.Equals("0000"))
				{
					Utility.SetDataTable(statisticsTotalDs.Report, statisticsTotalModel.ReportDataSet);		
					StatusMessage(statisticsTotalModel.ResultCnt + "건이 조회되었습니다.");
				
					keyMediaName      = cbSearchMedia.SelectedItem.Text;
					keyReportDay      = DateTime.Now.ToString("yyyy-MM-dd");

					uiPanelList.Text = "전체통계 : " + keyMediaName;
										
					canPrint = true;

				}
			}
			catch(FrameException fe)
			{
				FrameSystem.showMsgForm("전체통계 조회오류", new string[] {fe.ErrCode, fe.ResultMsg});
			}
			catch(Exception ex)
			{
				FrameSystem.showMsgForm("전체통계 조회오류",new string[] {"",ex.Message});
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

				int ColMax  = 8; // 컬럼수   				

				int TitleRow  = 1;
				int ConditionRow = 2;
				int HeaderRow = 4;
				int DataRow   = 6;
				string StartCol = "A";
				string EndCol   = "";
				string TitleCol = "E";
				int DataCount = 0;
				int CondCount = 0;
				int HeaderCount = 0;

				// 마지막 컬럼의 인덱스문자
				EndCol = GetColumnIndex(ColMax);

				xlApp = new Excel.Application();
				oWB = (Excel._Workbook)(xlApp.Workbooks.Add(Missing.Value));
				oSheet = (Excel._Worksheet)oWB.ActiveSheet;


				// 타이틀 작성
				oSheet.Cells[TitleRow,1] = "전체통계";
				oRng = oSheet.get_Range(StartCol+Convert.ToString(TitleRow), TitleCol+Convert.ToString(TitleRow));
				oRng.Merge(true);
				oRng.Font.Bold = true;
				oRng.Font.Size = 16;
				oRng.VerticalAlignment   = Excel.XlVAlign.xlVAlignCenter;			
				oRng.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;		

				// 조건정보 작성
				oSheet.Cells[ConditionRow+CondCount,1] = "집계기준일";
				oRng = oSheet.get_Range("B"+Convert.ToString(ConditionRow+CondCount), TitleCol+Convert.ToString(ConditionRow+CondCount));
				oRng.Merge(true);
				oSheet.Cells[ConditionRow+CondCount,2] = keyReportDay;
				CondCount++;

				// 조건부 테두리
				oRng = oSheet.get_Range(StartCol+Convert.ToString(ConditionRow), TitleCol+Convert.ToString(ConditionRow+(CondCount-1)));
				oRng.VerticalAlignment   = Excel.XlVAlign.xlVAlignCenter;			
				oRng.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;		
				oRng.Borders.LineStyle =  Excel.XlLineStyle.xlContinuous;	// 테두리속성 아래에 실선
				oRng.Borders.Weight    = Excel.XlBorderWeight.xlThin;		// 테두리속성 아래에 가는선


				// 헤더 정보 작성
				oSheet.Cells[HeaderRow,1] = "광고";
				oRng = oSheet.get_Range(GetColumnIndex(1)+Convert.ToString(HeaderRow), GetColumnIndex(4)+Convert.ToString(HeaderRow));
				oRng.Merge(true);
				oSheet.Cells[HeaderRow,5] = "누적노출집계";
				oRng = oSheet.get_Range(GetColumnIndex(5)+Convert.ToString(HeaderRow), GetColumnIndex(8)+Convert.ToString(HeaderRow));
				oRng.Merge(true);

				HeaderCount = 1;
				oSheet.Cells[HeaderRow+1,HeaderCount++] = "광고번호";
				oSheet.Cells[HeaderRow+1,HeaderCount++] = "광고명";
				oSheet.Cells[HeaderRow+1,HeaderCount++] = "실제종료일";
				oSheet.Cells[HeaderRow+1,HeaderCount++] = "보장노출";
				oSheet.Cells[HeaderRow+1,HeaderCount++] = "누적노출";
				oSheet.Cells[HeaderRow+1,HeaderCount++] = "%";
				oSheet.Cells[HeaderRow+1,HeaderCount++] = "";
				oSheet.Cells[HeaderRow+1,HeaderCount++] = "예상노출";


				oRng = oSheet.get_Range(StartCol+Convert.ToString(HeaderRow), EndCol+Convert.ToString(HeaderRow+1)); // 헤더의 범위
				oRng.Font.Bold           = true;							// 폰트 굵게
				oRng.VerticalAlignment   = Excel.XlVAlign.xlVAlignCenter;	// 세로중앙정렬
				oRng.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;	// 가로중앙정렬	 
				oRng.Interior.Color      = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.CornflowerBlue);   //셀 배경색 
				oRng.Font.Color          = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.White);	  //텍스트색			
				

				DataCount = 0;
				// 데이터 추출
				for (int inx =0; inx < statisticsTotalDs.Report.Rows.Count; inx++)
				{

					DataRow Row = statisticsTotalDs.Report.Rows[inx];			

					int ColCnt = 1;
					oSheet.Cells[DataRow+DataCount,ColCnt++] = Convert.ToInt32(Row["ItemNo"].ToString());		// 1  광고번호
					oSheet.Cells[DataRow+DataCount,ColCnt++] = Row["ItemName"].ToString();						// 2  광고명
					oSheet.Cells[DataRow+DataCount,ColCnt++] = Utility.reConvertDate(Row["RealEndDay"].ToString());	// 3 실제종료일
					oSheet.Cells[DataRow+DataCount,ColCnt++] = Convert.ToInt32(Row["ContractAmt"].ToString());	// 4  보장노출
					oSheet.Cells[DataRow+DataCount,ColCnt++] = Convert.ToInt32(Row["TotAdCnt"].ToString());		// 5  힛트수
					oSheet.Cells[DataRow+DataCount,ColCnt++] = Convert.ToDecimal(Row["ExcRate"].ToString());	// 6  도달율
					oSheet.Cells[DataRow+DataCount,ColCnt++] = Row["ExcRateBar"].ToString();		            // 7  그래프
					oSheet.Cells[DataRow+DataCount,ColCnt++] = Convert.ToInt32(Row["AvgExpAmt"].ToString());	// 8  평균노출량(예상누적노출수)

					DataCount++;
				}

				DataCount--;


				// 데이터 작성
				oRng = oSheet.get_Range(StartCol+Convert.ToString(HeaderRow), EndCol+Convert.ToString(DataRow+DataCount));	// 데이터의 범위
				oRng.EntireColumn.AutoFit();					// 데이터의 크기에 셀의 가로크기 맞춤
				oRng.Borders.LineStyle =  Excel.XlLineStyle.xlContinuous;	// 테두리속성 아래에 실선
				oRng.Borders.Weight    = Excel.XlBorderWeight.xlThin;		// 테두리속성 아래에 가는선

				// 실제종료일 중앙정렬
				oRng = oSheet.get_Range(GetColumnIndex(3)+Convert.ToString(DataRow), GetColumnIndex(3)+Convert.ToString(DataRow+DataCount)); // 일자
				oRng.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;	// 가로중앙정렬	 

				// 보장노출 외 데이터에 셀타입 설정
				oRng = oSheet.get_Range(GetColumnIndex(4)+Convert.ToString(DataRow), GetColumnIndex(5)+Convert.ToString(DataRow+DataCount));	// 데이터의 범위
				oRng.NumberFormatLocal = "#,##0";

				// 도달율 셀타입 설정
				oRng = oSheet.get_Range(GetColumnIndex(6)+Convert.ToString(DataRow), GetColumnIndex(6)+Convert.ToString(DataRow+DataCount));	// 데이터의 범위
				oRng.NumberFormatLocal = "#,##0.00";

				// 평균노출량 외 데이터에 셀타입 설정
				oRng = oSheet.get_Range(GetColumnIndex(8)+Convert.ToString(DataRow), GetColumnIndex(8)+Convert.ToString(DataRow+DataCount));	// 데이터의 범위
				oRng.NumberFormatLocal = "#,##0";
				
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

		private void grdExReport_DrawGridArea(object sender, Janus.Windows.GridEX.DrawGridAreaEventArgs e)
		{
			if(e.Column.Key=="ExcRateBar")
			{

				//first, draw the background:
				e.Graphics.FillRectangle(e.BackBrush,e.Bounds);

				//Now, draw the percent bar:
				if(e.Row.Cells["ExcRate"].Value != null)
				{
					// 데이터를 가져오고
					float percentValue = (float) e.Row.Cells["ExcRate"].Value;

					//calculate the rect to draw:
					Rectangle rect = Rectangle.Inflate(e.Bounds,-1,-3);
					rect.Width = (int)(rect.Width*(percentValue/100));

					//now draw the rectangle:
					if(rect.Width > 0 && rect.Height > 0) 
					{
						LinearGradientBrush br = new LinearGradientBrush(rect,Color.GreenYellow, Color.Green, LinearGradientMode.BackwardDiagonal);
			
						e.Graphics.FillRectangle(br,rect);

						br.Dispose();
					}

					e.Handled = true;
				}
			}		
		}


    }
}
