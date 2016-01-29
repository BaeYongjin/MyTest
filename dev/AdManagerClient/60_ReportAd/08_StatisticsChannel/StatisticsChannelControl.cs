// ===============================================================================
//
// StatisticsChannelControl.cs
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
    public class StatisticsChannelControl : System.Windows.Forms.UserControl, IUserControl
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
        StatisticsChannelModel mainModel  = new StatisticsChannelModel();	// 프로그램별 광고시청집계모델
		SummaryAdModel summaryAdModel  = new SummaryAdModel();	// 프로그램별 광고시청집계모델

		// Key 데이터
		string keyMediaName    = "";
		string keyContractName = "";
		string keyAgencyName   = "";
		string keyAdvertiserName = "";
        string keyCampaignCode = "";
        string keyCampaignName = "";
		string keyItemNo       = "";
		string keyItemName     = "";
		string keyReportBgnDay = "";
		string keyReportEndDay = "";
		string keyReportType   = "";

		string keyTotalAdCnt    = "";
		string keyContractAmt   = "";
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
        private Janus.Windows.UI.Dock.UIPanel uiPanelSearch;
        private Janus.Windows.UI.Dock.UIPanelInnerContainer uiPanelSearchContainer;
        private Janus.Windows.UI.Dock.UIPanel uiPanelList;
        private Janus.Windows.UI.Dock.UIPanelInnerContainer uiPanelListContainer;
        private Janus.Windows.UI.Dock.UIPanelGroup uiPanelChoiceAdSchedule;
        private System.Windows.Forms.Label label1;
        private Janus.Windows.GridEX.EditControls.EditBox editBox1;
        private System.Windows.Forms.Panel panMenuSchedule;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel3;
		private System.Data.DataView dvReport;
		private Janus.Windows.GridEX.GridEX grdExRepoert;
		private Janus.Windows.UI.Dock.UIPanelManager uiPM;
		private AdManagerClient.StatisticsChannelDs statisticsChannelDs;
		private Janus.Windows.GridEX.GridEX grdExReport;
        private Janus.Windows.GridEX.EditControls.NumericEditBox ebContractAmt;
        private System.Windows.Forms.Label label9;
        private Janus.Windows.GridEX.EditControls.NumericEditBox ebTotalAdCnt;
        private AdManagerClient.ReportHeaderControl rptHeader;
        private System.ComponentModel.IContainer components;

        public StatisticsChannelControl()
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(StatisticsChannelControl));
			this.uiPM = new Janus.Windows.UI.Dock.UIPanelManager(this.components);
			this.uiPanelChoiceAdSchedule = new Janus.Windows.UI.Dock.UIPanelGroup();
			this.uiPanelSearch = new Janus.Windows.UI.Dock.UIPanel();
			this.uiPanelSearchContainer = new Janus.Windows.UI.Dock.UIPanelInnerContainer();
			this.pnlSearch = new System.Windows.Forms.Panel();
			this.ebContractAmt = new Janus.Windows.GridEX.EditControls.NumericEditBox();
			this.label9 = new System.Windows.Forms.Label();
			this.ebTotalAdCnt = new Janus.Windows.GridEX.EditControls.NumericEditBox();
			this.rptHeader = new AdManagerClient.ReportHeaderControl();
			this.uiPanelList = new Janus.Windows.UI.Dock.UIPanel();
			this.uiPanelListContainer = new Janus.Windows.UI.Dock.UIPanelInnerContainer();
			this.grdExReport = new Janus.Windows.GridEX.GridEX();
			this.dvReport = new System.Data.DataView();
			this.statisticsChannelDs = new AdManagerClient.StatisticsChannelDs();
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
			((System.ComponentModel.ISupportInitialize)(this.statisticsChannelDs)).BeginInit();
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
			this.uiPM.AddDockPanelInfo(new System.Guid("afb3b7d0-aa79-443c-a1e8-3baba38f7a4d"), new System.Guid("b162c53a-7940-47cd-a0ed-9762c59c50d8"), 106, true);
			this.uiPM.AddDockPanelInfo(new System.Guid("8e3140b9-bfcb-43af-b84e-2ee6cddf51dc"), new System.Guid("b162c53a-7940-47cd-a0ed-9762c59c50d8"), 525, true);
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
			this.uiPanelChoiceAdSchedule.Text = "채널별통계";
			// 
			// uiPanelSearch
			// 
			this.uiPanelSearch.AllowResize = Janus.Windows.UI.InheritableBoolean.False;
			this.uiPanelSearch.CaptionVisible = Janus.Windows.UI.InheritableBoolean.False;
			this.uiPanelSearch.CloseButtonVisible = Janus.Windows.UI.InheritableBoolean.False;
			this.uiPanelSearch.InnerContainer = this.uiPanelSearchContainer;
			this.uiPanelSearch.Location = new System.Drawing.Point(0, 22);
			this.uiPanelSearch.Name = "uiPanelSearch";
			this.uiPanelSearch.Size = new System.Drawing.Size(1010, 109);
			this.uiPanelSearch.TabIndex = 4;
			this.uiPanelSearch.Text = "검색";
			// 
			// uiPanelSearchContainer
			// 
			this.uiPanelSearchContainer.Controls.Add(this.pnlSearch);
			this.uiPanelSearchContainer.Location = new System.Drawing.Point(1, 1);
			this.uiPanelSearchContainer.Name = "uiPanelSearchContainer";
			this.uiPanelSearchContainer.Size = new System.Drawing.Size(1008, 107);
			this.uiPanelSearchContainer.TabIndex = 0;
			// 
			// pnlSearch
			// 
			this.pnlSearch.BackColor = System.Drawing.SystemColors.Window;
			this.pnlSearch.Controls.Add(this.ebContractAmt);
			this.pnlSearch.Controls.Add(this.label9);
			this.pnlSearch.Controls.Add(this.ebTotalAdCnt);
			this.pnlSearch.Controls.Add(this.rptHeader);
			this.pnlSearch.Dock = System.Windows.Forms.DockStyle.Fill;
			this.pnlSearch.Location = new System.Drawing.Point(0, 0);
			this.pnlSearch.Name = "pnlSearch";
			this.pnlSearch.Size = new System.Drawing.Size(1008, 107);
			this.pnlSearch.TabIndex = 3;
			// 
			// ebContractAmt
			// 
			this.ebContractAmt.BackColor = System.Drawing.Color.WhiteSmoke;
			this.ebContractAmt.DecimalDigits = 0;
			this.ebContractAmt.FormatString = "#,##0";
			this.ebContractAmt.Location = new System.Drawing.Point(624, 80);
			this.ebContractAmt.Name = "ebContractAmt";
			this.ebContractAmt.ReadOnly = true;
			this.ebContractAmt.Size = new System.Drawing.Size(104, 21);
			this.ebContractAmt.TabIndex = 186;
			this.ebContractAmt.TabStop = false;
			this.ebContractAmt.Text = "0";
			this.ebContractAmt.TextAlignment = Janus.Windows.GridEX.TextAlignment.Far;
			this.ebContractAmt.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
			this.ebContractAmt.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2003;
			// 
			// label9
			// 
			this.label9.Location = new System.Drawing.Point(504, 80);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(104, 21);
			this.label9.TabIndex = 184;
			this.label9.Text = "보장노출/총노출";
			this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// ebTotalAdCnt
			// 
			this.ebTotalAdCnt.BackColor = System.Drawing.Color.WhiteSmoke;
			this.ebTotalAdCnt.DecimalDigits = 0;
			this.ebTotalAdCnt.FormatString = "#,##0";
			this.ebTotalAdCnt.Location = new System.Drawing.Point(736, 80);
			this.ebTotalAdCnt.Name = "ebTotalAdCnt";
			this.ebTotalAdCnt.ReadOnly = true;
			this.ebTotalAdCnt.Size = new System.Drawing.Size(104, 21);
			this.ebTotalAdCnt.TabIndex = 185;
			this.ebTotalAdCnt.Text = "0";
			this.ebTotalAdCnt.TextAlignment = Janus.Windows.GridEX.TextAlignment.Far;
			this.ebTotalAdCnt.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
			this.ebTotalAdCnt.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2003;
			// 
			// rptHeader
			// 
			this.rptHeader.BackColor = System.Drawing.Color.DeepSkyBlue;
			this.rptHeader.Dock = System.Windows.Forms.DockStyle.Fill;
			this.rptHeader.Location = new System.Drawing.Point(0, 0);
			this.rptHeader.Name = "rptHeader";
			this.rptHeader.Size = new System.Drawing.Size(1008, 107);
			this.rptHeader.TabIndex = 187;
			this.rptHeader.u_IsPrint = false;
			this.rptHeader.u_MenuName = "";
			// 
			// uiPanelList
			// 
			this.uiPanelList.BackColor = System.Drawing.SystemColors.Window;
			this.uiPanelList.CaptionStyle = Janus.Windows.UI.Dock.PanelCaptionStyle.Dark;
			this.uiPanelList.InnerContainer = this.uiPanelListContainer;
			this.uiPanelList.Location = new System.Drawing.Point(0, 135);
			this.uiPanelList.MinimumSize = new System.Drawing.Size(-1, 100);
			this.uiPanelList.Name = "uiPanelList";
			this.uiPanelList.Size = new System.Drawing.Size(1010, 542);
			this.uiPanelList.TabIndex = 4;
			this.uiPanelList.Text = "통계";
			// 
			// uiPanelListContainer
			// 
			this.uiPanelListContainer.BackColor = System.Drawing.SystemColors.Window;
			this.uiPanelListContainer.Controls.Add(this.grdExReport);
			this.uiPanelListContainer.Location = new System.Drawing.Point(1, 23);
			this.uiPanelListContainer.Name = "uiPanelListContainer";
			this.uiPanelListContainer.Size = new System.Drawing.Size(1008, 518);
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
			this.grdExReport.GridLineColor = System.Drawing.Color.Silver;
			this.grdExReport.GridLines = Janus.Windows.GridEX.GridLines.Vertical;
			this.grdExReport.GridLineStyle = Janus.Windows.GridEX.GridLineStyle.Solid;
			this.grdExReport.GroupByBoxVisible = false;
			this.grdExReport.GroupRowFormatStyle.BackColor = System.Drawing.Color.White;
			this.grdExReport.GroupRowFormatStyle.ForeColor = System.Drawing.SystemColors.ActiveCaption;
			this.grdExReport.GroupRowVisualStyle = Janus.Windows.GridEX.GroupRowVisualStyle.Outlook2003;
			this.grdExReport.HeaderFormatStyle.TextAlignment = Janus.Windows.GridEX.TextAlignment.Center;
			this.grdExReport.HideSelection = Janus.Windows.GridEX.HideSelection.Highlight;
			this.grdExReport.Location = new System.Drawing.Point(0, 0);
			this.grdExReport.Name = "grdExReport";
			this.grdExReport.ScrollBars = Janus.Windows.GridEX.ScrollBars.Vertical;
			this.grdExReport.SelectedInactiveFormatStyle.BackColor = System.Drawing.Color.Gold;
			this.grdExReport.Size = new System.Drawing.Size(1008, 518);
			this.grdExReport.TabIndex = 12;
			this.grdExReport.TabKeyBehavior = Janus.Windows.GridEX.TabKeyBehavior.ControlNavigation;
			this.grdExReport.ThemedAreas = ((Janus.Windows.GridEX.ThemedArea)(((((((((Janus.Windows.GridEX.ThemedArea.ScrollBars | Janus.Windows.GridEX.ThemedArea.EditControls) 
            | Janus.Windows.GridEX.ThemedArea.Headers) 
            | Janus.Windows.GridEX.ThemedArea.GroupByBox) 
            | Janus.Windows.GridEX.ThemedArea.GroupRows) 
            | Janus.Windows.GridEX.ThemedArea.ControlBorder) 
            | Janus.Windows.GridEX.ThemedArea.Cards) 
            | Janus.Windows.GridEX.ThemedArea.Gridlines) 
            | Janus.Windows.GridEX.ThemedArea.CheckBoxes)));
			this.grdExReport.TotalRowFormatStyle.BackColor = System.Drawing.Color.Yellow;
			this.grdExReport.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
			this.grdExReport.DrawGridArea += new Janus.Windows.GridEX.DrawGridAreaEventHandler(this.grdExReport_DrawGridArea);
			// 
			// dvReport
			// 
			this.dvReport.Table = this.statisticsChannelDs.Report;
			// 
			// statisticsChannelDs
			// 
			this.statisticsChannelDs.DataSetName = "StatisticsChannelDs";
			this.statisticsChannelDs.Locale = new System.Globalization.CultureInfo("en-US");
			this.statisticsChannelDs.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
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
			// StatisticsChannelControl
			// 
			this.Controls.Add(this.uiPanelChoiceAdSchedule);
			this.Font = global::AdManagerClient.Properties.Settings.Default.Font1;
			this.Name = "StatisticsChannelControl";
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
			((System.ComponentModel.ISupportInitialize)(this.statisticsChannelDs)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.grdExRepoert)).EndInit();
			this.panMenuSchedule.ResumeLayout(false);
			this.ResumeLayout(false);

        }
        #endregion

        #region 컨트롤 로드
        private void UserControl_Load(object sender, System.EventArgs e)
        {
            rptHeader.SearchClicked += new AdManagerClient.SearchClickEventHandler(OnSearch);
            rptHeader.ExcelClicked	+= new AdManagerClient.ExcelClickEventHandler(OnExcel);
            rptHeader.u_MenuName = MenuCode;
            rptHeader.u_InitControl();
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
		private void MakeExcel()
		{	
			Excel.Application xlApp= null;
			Excel._Workbook   oWB = null;
			Excel._Worksheet  oSheet = null;
			Excel.Range       oRng = null;
			
			try
			{	
				int ColMax          = 5;
				int TitleRow        = 1;		
				int ConditionRow    = 2;   
				int HeaderRow       = 10;
				int DataRow         = 11;
				string StartCol     = "A";
				string EndCol       = "";
				string TitleCol     = "E";
				int DataCount       = 0;
				int CondCount       = 0;
				int HeaderCount     = 0;

				// 마지막 컬럼의 인덱스문자
				EndCol = GetColumnIndex(ColMax);

				xlApp = new Excel.Application();
				oWB = (Excel._Workbook)(xlApp.Workbooks.Add(Missing.Value));
				oSheet = (Excel._Worksheet)oWB.ActiveSheet;


				// 타이틀 작성
				oSheet.Cells[TitleRow,1] = "채널통계";
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
				oSheet.Cells[ConditionRow+CondCount,2] = "[" + keyItemNo + "] " + keyItemName;
				CondCount++;

				oSheet.Cells[ConditionRow+CondCount,1] = "대행사";
				oRng = oSheet.get_Range("B"+Convert.ToString(ConditionRow+CondCount), TitleCol+Convert.ToString(ConditionRow+CondCount));
				oRng.Merge(true);
				oSheet.Cells[ConditionRow+CondCount,2] = keyAgencyName;
				CondCount++;

				oSheet.Cells[ConditionRow+CondCount,1] = "기간";
				oRng = oSheet.get_Range("B"+Convert.ToString(ConditionRow+CondCount), TitleCol+Convert.ToString(ConditionRow+CondCount));
				oRng.Merge(true);
				string ReportType =  keyReportBgnDay + " ~ " + keyReportEndDay;
				ReportType += " (" + keyReportType + ") ";

				oSheet.Cells[ConditionRow+CondCount,2] = ReportType;
				CondCount++;

				oSheet.Cells[ConditionRow+CondCount,1] = "보장노출";
				oRng = oSheet.get_Range("B"+Convert.ToString(ConditionRow+CondCount), TitleCol+Convert.ToString(ConditionRow+CondCount));
				oRng.NumberFormatLocal = "#,##0";
				oRng.Merge(true);
				oSheet.Cells[ConditionRow+CondCount,2] = Convert.ToInt32(keyContractAmt);
				CondCount++;

				oSheet.Cells[ConditionRow+CondCount,1] = "총노출";
				oRng = oSheet.get_Range("B"+Convert.ToString(ConditionRow+CondCount), TitleCol+Convert.ToString(ConditionRow+CondCount));
				oRng.NumberFormatLocal = "#,##0";
				oRng.Merge(true);
				oSheet.Cells[ConditionRow+CondCount,2] = Convert.ToInt32(keyTotalAdCnt);
				CondCount++;


				// 조건부 테두리
				oRng = oSheet.get_Range(StartCol+Convert.ToString(ConditionRow), TitleCol+Convert.ToString(ConditionRow+(CondCount-1)));
				oRng.VerticalAlignment   = Excel.XlVAlign.xlVAlignCenter;			
				oRng.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;		
				oRng.Borders.LineStyle =  Excel.XlLineStyle.xlContinuous;	// 테두리속성 아래에 실선
				oRng.Borders.Weight    = Excel.XlBorderWeight.xlThin;		// 테두리속성 아래에 가는선


				// 헤더 정보 작성
				HeaderCount = 1;
				oSheet.Cells[HeaderRow,HeaderCount++] = "구분"; 
				oRng = oSheet.get_Range(GetColumnIndex(1)+Convert.ToString(HeaderRow), GetColumnIndex(2)+Convert.ToString(HeaderRow));
				oRng.Merge(true);
				HeaderCount++;

				oSheet.Cells[HeaderRow,HeaderCount++] = "노출수";
				oSheet.Cells[HeaderRow,HeaderCount++] = "%";
				oSheet.Cells[HeaderRow,HeaderCount++] = "";

				oRng = oSheet.get_Range(StartCol+Convert.ToString(HeaderRow), EndCol+Convert.ToString(HeaderRow)); // 헤더의 범위
				oRng.Font.Bold           = true;							// 폰트 굵게
				oRng.VerticalAlignment   = Excel.XlVAlign.xlVAlignCenter;	// 세로중앙정렬
				oRng.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;	// 가로중앙정렬	 
				oRng.Interior.Color      = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.CornflowerBlue);   //셀 배경색 
				oRng.Font.Color          = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.White);	  //텍스트색			
				

				DataCount = 0;
				string OldORDCode = "";
				int TypeMergyStart = 0;
				int MergeCount = 0;

				// 데이터 추출
				for (int inx =0; inx < statisticsChannelDs.Report.Rows.Count; inx++)
				{

					DataRow Row = statisticsChannelDs.Report.Rows[inx];			

					int ColCnt = 1;
					
					string ORDCode = Row["ORDCode"].ToString();

					// 항목 머지를 위함
					if(!OldORDCode.Equals("") && !OldORDCode.Equals(ORDCode))
					{
						if(MergeCount > 1)
						{
							oRng = oSheet.get_Range("A"+Convert.ToString(DataRow + TypeMergyStart), "A"+Convert.ToString(DataRow + DataCount-1));
							oRng.Merge(false);
						}
						TypeMergyStart = DataCount;
						MergeCount = 0;
					}
					OldORDCode = ORDCode;
					MergeCount++;


					if(ORDCode.Equals("1")) // 카테고리합계이면
					{
						oSheet.Cells[DataRow+DataCount,ColCnt++] = Row["CategoryName"].ToString();					// 1  카테고리명
						oRng = oSheet.get_Range(GetColumnIndex(1)+Convert.ToString(DataRow+DataCount), GetColumnIndex(2)+Convert.ToString(DataRow+DataCount));
						oRng.Merge(true);
						ColCnt++;

					}
					else
					{
						if(MergeCount == 1)
						{
							oSheet.Cells[DataRow+DataCount,ColCnt++] = Row["ORDName"].ToString();						// 1  구분명
						}
						else
						{
							ColCnt++;
						}
						oSheet.Cells[DataRow+DataCount,ColCnt++] = Row["Title"].ToString();							// 2  제목(장르명/프로그램명)
					}
					oSheet.Cells[DataRow+DataCount,ColCnt++] = Convert.ToInt32(Row["AdCnt"].ToString());		// 3  노출수
					oSheet.Cells[DataRow+DataCount,ColCnt++] = Convert.ToDecimal(Row["AdRate"].ToString());		// 4  %
					oSheet.Cells[DataRow+DataCount,ColCnt++] = Row["RateBar"].ToString();						// 5  그래프

					if(ORDCode.Equals("1")) // 카테고리합계이면
					{
						oRng = oSheet.get_Range(GetColumnIndex(1)+Convert.ToString(DataRow+DataCount), GetColumnIndex(5)+Convert.ToString(DataRow+DataCount));
						oRng.Interior.Color      = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Yellow);   //셀 배경색 
					}

					DataCount++;
				}

				if(MergeCount > 1)
				{
					oRng = oSheet.get_Range("A"+Convert.ToString(DataRow + TypeMergyStart), "A"+Convert.ToString(DataRow + DataCount-1));
					oRng.Merge(false);
				}

				DataCount--;


				// 데이터 작성
				oRng = oSheet.get_Range(StartCol+Convert.ToString(HeaderRow), EndCol+Convert.ToString(DataRow+DataCount));	// 데이터의 범위
				oRng.EntireColumn.AutoFit();					// 데이터의 크기에 셀의 가로크기 맞춤
				oRng.Borders.LineStyle =  Excel.XlLineStyle.xlContinuous;	// 테두리속성 아래에 실선
				oRng.Borders.Weight    = Excel.XlBorderWeight.xlThin;		// 테두리속성 아래에 가는선

				// 노출수 데이터에 셀타입 설정
				oRng = oSheet.get_Range(GetColumnIndex(3)+Convert.ToString(DataRow), GetColumnIndex(3)+Convert.ToString(DataRow+DataCount));	// 데이터의 범위
				oRng.NumberFormatLocal = "#,##0";

				// % 셀타입 설정
				oRng = oSheet.get_Range(GetColumnIndex(4)+Convert.ToString(DataRow), GetColumnIndex(4)+Convert.ToString(DataRow+DataCount));	// 데이터의 범위
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

        private void OnSearch(object sender, SearchReportData e)
        {

            StatusMessage("일별통계을 조회합니다.");
            ProgressStart();
			 
            try
            {
                // 데이터모델 초기화
                mainModel.Init();
			 
                mainModel.SearchMediaCode   =  e.MediaCode;
                mainModel.SearchContractSeq =  e.ContractSeq;
                mainModel.CampaignCode		=  e.CampaignNo;
                mainModel.SearchItemNo      =  e.ItemNo;
                mainModel.SearchStartDay    =  e.ItemBeginDay;
                mainModel.SearchEndDay  	=  e.ItemEndDay;
			 
		 
                keyMediaName      = "";
                keyContractName   = "";
                keyAgencyName     = "";
                keyAdvertiserName = "";
                keyItemNo         = "";
                keyItemName       = "";
                keyReportBgnDay   = "";
                keyReportEndDay   = "";
			 
                uiPanelList.Text = "일별통계"; 
			 
                //  전체통계 서비스를 호출한다.
                new StatisticsChannelManager(systemModel,commonModel).GetStatisticsChannelReport(mainModel);
	
                if (mainModel.ResultCD.Equals("0000"))
                {
                    Utility.SetDataTableFast(statisticsChannelDs.Report, mainModel.ReportDataSet);		
                    StatusMessage(mainModel.ResultCnt + "건이 조회되었습니다.");
			 	
                    keyTotalAdCnt     = mainModel.TotalAdCnt.ToString();
                    keyContractAmt    = mainModel.ContractAmt.ToString();
			 	
                    keyMediaName      = e.MediaName;
                    keyContractName   = e.ContractName;
                    keyAgencyName     = e.AgencyName;
                    keyAdvertiserName = e.AdvertiserName;
                    keyCampaignCode   = e.CampaignNo;
                    keyCampaignName   = e.CampaignName;
                    keyItemNo         = e.ItemNo;
                    keyItemName       = e.ItemName;
                    keyReportBgnDay   = e.ItemBeginDay;
                    keyReportEndDay   = e.ItemEndDay;
                    keyReportType     = rptHeader.u_DayType;

                    ebTotalAdCnt.Text   = keyTotalAdCnt;
                    ebContractAmt.Text  = keyContractAmt;

                    uiPanelList.Text = keyMediaName + " | " + keyContractName + " | " + keyCampaignName + " | " + keyItemName;
                    uiPanelList.Text += " | " + keyReportBgnDay + " ~ " + keyReportEndDay;	
	
                    rptHeader.u_IsPrint = true;
                }
                else
                {
                    rptHeader.u_IsPrint = false;
                }
            }
            catch(FrameException fe)
            {
                rptHeader.u_IsPrint = false;
                FrameSystem.showMsgForm("채널별통계 조회오류", new string[] {fe.ErrCode, fe.ResultMsg});
            }
            catch(Exception ex)
            {
                rptHeader.u_IsPrint = false;
                FrameSystem.showMsgForm("채널별통계 조회오류",new string[] {"",ex.Message});
            }
            finally
            {
                ProgressStop();
            }

        }


        private void OnExcel(object sender, EventArgs e)
        {
            MakeExcel();
        }

		private void grdExReport_DrawGridArea(object sender, Janus.Windows.GridEX.DrawGridAreaEventArgs e)
		{
			if(e.Column.Key=="RateBar")
			{

				//first, draw the background:
				e.Graphics.FillRectangle(e.BackBrush,e.Bounds);

				//Now, draw the percent bar:
				if(e.Row.Cells["AdRate"].Value != null)
				{
					// 데이터를 가져오고
					float percentValue = (float) e.Row.Cells["AdRate"].Value;

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
