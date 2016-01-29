// ===============================================================================
//
// ProgramAdHitControl.cs
//
// 프로그램별 광고시청집계 컨트롤을 정의합니다. 
//
// ===============================================================================
// Release history
//
// ===============================================================================
// Copyright (C) 2006 G-Inno Systems Inc.
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
    public class ProgramAdHitControl : System.Windows.Forms.UserControl, IUserControl
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
        ProgramAdHitModel mainModel  = new ProgramAdHitModel();	// 프로그램별 광고시청집계모델

        // Key 데이터
		string keyMediaName         = "";
		string keyContractSeq       = "";
		string keyContractName      = "";
        string keyCampaignCode      = "";
        string keyCampaignName      = "";
		string keyItemNo            = "";
		string keyItemName          = "";
		string keyReportBgnDay      = "";
		string keyReportEndDay      = "";
        string keyAgencyName        = "";
        string keyAdvertiserName    = "";
        string keyReportType        = "";
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
		private Janus.Windows.GridEX.GridEX grdExRatingList;
		private System.Data.DataView dsReport;
		private AdManagerClient.ProgramAdHitDs programAdHitDs;
        private AdManagerClient.ReportHeaderControl rptHeader;
        private System.ComponentModel.IContainer components;

        public ProgramAdHitControl()
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
            Janus.Windows.GridEX.GridEXLayout grdExRatingList_DesignTimeLayout = new Janus.Windows.GridEX.GridEXLayout();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ProgramAdHitControl));
            this.uiPM = new Janus.Windows.UI.Dock.UIPanelManager(this.components);
            this.uiPanelChoiceAdSchedule = new Janus.Windows.UI.Dock.UIPanelGroup();
            this.uiPanelSearch = new Janus.Windows.UI.Dock.UIPanel();
            this.uiPanelSearchContainer = new Janus.Windows.UI.Dock.UIPanelInnerContainer();
            this.pnlSearch = new System.Windows.Forms.Panel();
            this.rptHeader = new AdManagerClient.ReportHeaderControl();
            this.uiPanelList = new Janus.Windows.UI.Dock.UIPanel();
            this.uiPanelListContainer = new Janus.Windows.UI.Dock.UIPanelInnerContainer();
            this.grdExRatingList = new Janus.Windows.GridEX.GridEX();
            this.dsReport = new System.Data.DataView();
            this.programAdHitDs = new AdManagerClient.ProgramAdHitDs();
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
            ((System.ComponentModel.ISupportInitialize)(this.grdExRatingList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dsReport)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.programAdHitDs)).BeginInit();
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
            this.uiPM.AddDockPanelInfo(new System.Guid("afb3b7d0-aa79-443c-a1e8-3baba38f7a4d"), new System.Guid("b162c53a-7940-47cd-a0ed-9762c59c50d8"), 110, true);
            this.uiPM.AddDockPanelInfo(new System.Guid("8e3140b9-bfcb-43af-b84e-2ee6cddf51dc"), new System.Guid("b162c53a-7940-47cd-a0ed-9762c59c50d8"), 521, true);
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
            this.uiPanelChoiceAdSchedule.BackColor = System.Drawing.SystemColors.Window;
            this.uiPanelChoiceAdSchedule.CaptionVisible = Janus.Windows.UI.InheritableBoolean.True;
            this.uiPanelChoiceAdSchedule.CloseButtonVisible = Janus.Windows.UI.InheritableBoolean.False;
            this.uiPanelChoiceAdSchedule.Location = new System.Drawing.Point(0, 0);
            this.uiPanelChoiceAdSchedule.Name = "uiPanelChoiceAdSchedule";
            this.uiPanelChoiceAdSchedule.Size = new System.Drawing.Size(1010, 677);
            this.uiPanelChoiceAdSchedule.TabIndex = 4;
            this.uiPanelChoiceAdSchedule.Text = "프로그램별 광고시청집계";
            // 
            // uiPanelSearch
            // 
            this.uiPanelSearch.AllowResize = Janus.Windows.UI.InheritableBoolean.False;
            this.uiPanelSearch.CaptionVisible = Janus.Windows.UI.InheritableBoolean.False;
            this.uiPanelSearch.CloseButtonVisible = Janus.Windows.UI.InheritableBoolean.False;
            this.uiPanelSearch.InnerContainer = this.uiPanelSearchContainer;
            this.uiPanelSearch.Location = new System.Drawing.Point(0, 22);
            this.uiPanelSearch.Name = "uiPanelSearch";
            this.uiPanelSearch.Size = new System.Drawing.Size(1010, 113);
            this.uiPanelSearch.TabIndex = 4;
            this.uiPanelSearch.Text = "검색";
            // 
            // uiPanelSearchContainer
            // 
            this.uiPanelSearchContainer.Controls.Add(this.pnlSearch);
            this.uiPanelSearchContainer.Location = new System.Drawing.Point(1, 1);
            this.uiPanelSearchContainer.Name = "uiPanelSearchContainer";
            this.uiPanelSearchContainer.Size = new System.Drawing.Size(1008, 111);
            this.uiPanelSearchContainer.TabIndex = 0;
            // 
            // pnlSearch
            // 
            this.pnlSearch.BackColor = System.Drawing.SystemColors.Window;
            this.pnlSearch.Controls.Add(this.rptHeader);
            this.pnlSearch.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlSearch.Location = new System.Drawing.Point(0, 0);
            this.pnlSearch.Name = "pnlSearch";
            this.pnlSearch.Size = new System.Drawing.Size(1008, 111);
            this.pnlSearch.TabIndex = 3;
            // 
            // rptHeader
            // 
            this.rptHeader.BackColor = System.Drawing.Color.DeepSkyBlue;
            this.rptHeader.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rptHeader.Font = global::AdManagerClient.Properties.Settings.Default.Font1;
            this.rptHeader.Location = new System.Drawing.Point(0, 0);
            this.rptHeader.Name = "rptHeader";
            this.rptHeader.Size = new System.Drawing.Size(1008, 111);
            this.rptHeader.TabIndex = 0;
            this.rptHeader.u_IsPrint = false;
            this.rptHeader.u_MenuName = "";
            this.rptHeader.Load += new System.EventHandler(this.rptHeader_Load);
            // 
            // uiPanelList
            // 
            this.uiPanelList.BackColor = System.Drawing.SystemColors.Window;
            this.uiPanelList.CaptionStyle = Janus.Windows.UI.Dock.PanelCaptionStyle.Dark;
            this.uiPanelList.InnerContainer = this.uiPanelListContainer;
            this.uiPanelList.Location = new System.Drawing.Point(0, 139);
            this.uiPanelList.MinimumSize = new System.Drawing.Size(-1, 100);
            this.uiPanelList.Name = "uiPanelList";
            this.uiPanelList.Size = new System.Drawing.Size(1010, 538);
            this.uiPanelList.TabIndex = 4;
            this.uiPanelList.Text = "시청률";
            // 
            // uiPanelListContainer
            // 
            this.uiPanelListContainer.BackColor = System.Drawing.SystemColors.Window;
            this.uiPanelListContainer.Controls.Add(this.grdExRatingList);
            this.uiPanelListContainer.Location = new System.Drawing.Point(1, 23);
            this.uiPanelListContainer.Name = "uiPanelListContainer";
            this.uiPanelListContainer.Size = new System.Drawing.Size(1008, 514);
            this.uiPanelListContainer.TabIndex = 0;
            // 
            // grdExRatingList
            // 
            this.grdExRatingList.AllowEdit = Janus.Windows.GridEX.InheritableBoolean.False;
            this.grdExRatingList.AlternatingColors = true;
            this.grdExRatingList.BorderStyle = Janus.Windows.GridEX.BorderStyle.None;
            this.grdExRatingList.DataSource = this.dsReport;
            grdExRatingList_DesignTimeLayout.LayoutString = resources.GetString("grdExRatingList_DesignTimeLayout.LayoutString");
            this.grdExRatingList.DesignTimeLayout = grdExRatingList_DesignTimeLayout;
            this.grdExRatingList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdExRatingList.EditorsControlStyle.ButtonAppearance = Janus.Windows.GridEX.ButtonAppearance.Regular;
            this.grdExRatingList.EmptyRows = true;
            this.grdExRatingList.FocusCellFormatStyle.BackColor = System.Drawing.SystemColors.Highlight;
            this.grdExRatingList.FocusCellFormatStyle.ForeColor = System.Drawing.Color.White;
            this.grdExRatingList.FocusStyle = Janus.Windows.GridEX.FocusStyle.None;
            this.grdExRatingList.GridLineColor = System.Drawing.Color.Silver;
            this.grdExRatingList.GridLines = Janus.Windows.GridEX.GridLines.Vertical;
            this.grdExRatingList.GridLineStyle = Janus.Windows.GridEX.GridLineStyle.Solid;
            this.grdExRatingList.GroupByBoxInfoFormatStyle.BackColor = System.Drawing.Color.Empty;
            this.grdExRatingList.GroupByBoxVisible = false;
            this.grdExRatingList.GroupRowFormatStyle.BackColor = System.Drawing.Color.White;
            this.grdExRatingList.GroupRowFormatStyle.BackColorGradient = System.Drawing.Color.FromArgb(((int)(((byte)(218)))), ((int)(((byte)(228)))), ((int)(((byte)(238)))));
            this.grdExRatingList.GroupRowVisualStyle = Janus.Windows.GridEX.GroupRowVisualStyle.Outlook2003;
            this.grdExRatingList.GroupTotalRowFormatStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.grdExRatingList.GroupTotalRowFormatStyle.FontBold = Janus.Windows.GridEX.TriState.True;
            this.grdExRatingList.GroupTotalRowFormatStyle.ForeColor = System.Drawing.SystemColors.ControlText;
            this.grdExRatingList.GroupTotals = Janus.Windows.GridEX.GroupTotals.Always;
            this.grdExRatingList.HeaderFormatStyle.TextAlignment = Janus.Windows.GridEX.TextAlignment.Center;
            this.grdExRatingList.HideSelection = Janus.Windows.GridEX.HideSelection.Highlight;
            this.grdExRatingList.Location = new System.Drawing.Point(0, 0);
            this.grdExRatingList.Name = "grdExRatingList";
            this.grdExRatingList.ScrollBars = Janus.Windows.GridEX.ScrollBars.Vertical;
            this.grdExRatingList.SelectedInactiveFormatStyle.BackColor = System.Drawing.Color.Gold;
            this.grdExRatingList.Size = new System.Drawing.Size(1008, 514);
            this.grdExRatingList.TabIndex = 13;
            this.grdExRatingList.TabKeyBehavior = Janus.Windows.GridEX.TabKeyBehavior.ControlNavigation;
            this.grdExRatingList.ThemedAreas = ((Janus.Windows.GridEX.ThemedArea)(((((((((Janus.Windows.GridEX.ThemedArea.ScrollBars | Janus.Windows.GridEX.ThemedArea.EditControls) 
            | Janus.Windows.GridEX.ThemedArea.Headers) 
            | Janus.Windows.GridEX.ThemedArea.GroupByBox) 
            | Janus.Windows.GridEX.ThemedArea.GroupRows) 
            | Janus.Windows.GridEX.ThemedArea.ControlBorder) 
            | Janus.Windows.GridEX.ThemedArea.Cards) 
            | Janus.Windows.GridEX.ThemedArea.Gridlines) 
            | Janus.Windows.GridEX.ThemedArea.CheckBoxes)));
            this.grdExRatingList.TotalRow = Janus.Windows.GridEX.InheritableBoolean.True;
            this.grdExRatingList.TotalRowFormatStyle.BackColor = System.Drawing.Color.Gold;
            this.grdExRatingList.TotalRowFormatStyle.FontBold = Janus.Windows.GridEX.TriState.True;
            this.grdExRatingList.TotalRowFormatStyle.ForeColor = System.Drawing.SystemColors.ControlText;
            this.grdExRatingList.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            // 
            // dsReport
            // 
            this.dsReport.Table = this.programAdHitDs.Report;
            // 
            // programAdHitDs
            // 
            this.programAdHitDs.DataSetName = "ProgramAdHitDs";
            this.programAdHitDs.Locale = new System.Globalization.CultureInfo("en-US");
            this.programAdHitDs.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
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
            // ProgramAdHitControl
            // 
            this.Controls.Add(this.uiPanelChoiceAdSchedule);
            this.Font = global::AdManagerClient.Properties.Settings.Default.Font1;
            this.Name = "ProgramAdHitControl";
            this.Size = new System.Drawing.Size(1010, 677);
            this.Load += new System.EventHandler(this.UserControl_Load);
            ((System.ComponentModel.ISupportInitialize)(this.uiPM)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelChoiceAdSchedule)).EndInit();
            this.uiPanelChoiceAdSchedule.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelSearch)).EndInit();
            this.uiPanelSearch.ResumeLayout(false);
            this.uiPanelSearchContainer.ResumeLayout(false);
            this.pnlSearch.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelList)).EndInit();
            this.uiPanelList.ResumeLayout(false);
            this.uiPanelListContainer.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdExRatingList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dsReport)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.programAdHitDs)).EndInit();
            this.panMenuSchedule.ResumeLayout(false);
            this.ResumeLayout(false);

        }
        #endregion

        #region 컨트롤 로드
        private void UserControl_Load(object sender, System.EventArgs e)
        {
            // 컨트롤 초기화
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
				int ColCount  = 5; // 컬럼수
				int TitleRow  = 1;
				int ConditionRow = 2;
				int HeaderRow = 8;
				int     DataRow   = 9;
				string  StartCol = "A";
				string  EndCol   = "E";
				int     DataCount = 0;

				int TotalAdHit = 0;
				int TotalPgHit = 0;
				int SumCategoryAdHit = 0;
				int SumCategoryPgHit = 0;
				int SumGenreAdHit = 0;
				int SumGenrePgHit = 0;

				string OldCategory = "";
				string OldGenre    = "";

				xlApp = new Excel.Application();
				oWB = (Excel._Workbook)(xlApp.Workbooks.Add(Missing.Value));
				oSheet = (Excel._Worksheet)oWB.ActiveSheet;


				// 타이틀 작성
				oSheet.Cells[TitleRow,1] = "프로그램별 광고시청 집계";
				oRng = oSheet.get_Range(StartCol+Convert.ToString(TitleRow), EndCol+Convert.ToString(TitleRow));
				oRng.Merge(true);
				oRng.Font.Bold = true;
				oRng.Font.Size = 16;
				oRng.VerticalAlignment   = Excel.XlVAlign.xlVAlignCenter;			
				oRng.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;		

				// 조건정보 작성
				oSheet.Cells[ConditionRow,1] = "매체";
				oRng = oSheet.get_Range("B"+Convert.ToString(ConditionRow), EndCol+Convert.ToString(ConditionRow));
				oRng.Merge(true);
				oSheet.Cells[ConditionRow,2] = keyMediaName;

				oSheet.Cells[ConditionRow+1,1] = "계약명";
				oRng = oSheet.get_Range("B"+Convert.ToString(ConditionRow+1), EndCol+Convert.ToString(ConditionRow+1));
				oRng.Merge(true);
				oSheet.Cells[ConditionRow+1,2] = keyContractName;

                oSheet.Cells[ConditionRow+2,1] = "캠페인";
                oRng = oSheet.get_Range("B"+Convert.ToString(ConditionRow+2), EndCol+Convert.ToString(ConditionRow+2));
                oRng.Merge(true);
                oSheet.Cells[ConditionRow+2,2] = keyCampaignName;
					
				oSheet.Cells[ConditionRow+3,1] = "광고명";
				oRng = oSheet.get_Range("B"+Convert.ToString(ConditionRow+3), EndCol+Convert.ToString(ConditionRow+3));
				oRng.Merge(true);
				oSheet.Cells[ConditionRow+3,2] = "[" + keyItemNo + "] " + keyItemName;

				oSheet.Cells[ConditionRow+4,1] = "집계기간";
				oRng = oSheet.get_Range("B"+Convert.ToString(ConditionRow+4), EndCol+Convert.ToString(ConditionRow+4));
				oRng.Merge(true);
                
                string ReportType =  Utility.reConvertDate("20" + keyReportBgnDay) + " ~ " + Utility.reConvertDate("20" + keyReportEndDay);
                ReportType += " (" + keyReportType + ")";
				oSheet.Cells[ConditionRow+4,2] = ReportType;

				// 조건부 테두리
				oRng = oSheet.get_Range(StartCol+Convert.ToString(ConditionRow), EndCol+Convert.ToString(ConditionRow+4));
				oRng.VerticalAlignment  = Excel.XlVAlign.xlVAlignCenter;			
				oRng.HorizontalAlignment= Excel.XlHAlign.xlHAlignCenter;		
				oRng.Borders.LineStyle  =  Excel.XlLineStyle.xlContinuous;	// 테두리속성 아래에 실선
				oRng.Borders.Weight     = Excel.XlBorderWeight.xlThin;		// 테두리속성 아래에 가는선
                oRng.Font.Size          = 10;

				// 헤더 정보 작성
				oSheet.Cells[HeaderRow,1] = "카테고리";
				oSheet.Cells[HeaderRow,2] = "장르";
				oSheet.Cells[HeaderRow,3] = "프로그램명";
				oSheet.Cells[HeaderRow,4] = "광고시청횟수";
				oSheet.Cells[HeaderRow,5] = "프로그램시청횟수";
				
				oRng = oSheet.get_Range(StartCol+Convert.ToString(HeaderRow), EndCol+Convert.ToString(HeaderRow)); // 헤더의 범위
				oRng.Font.Bold           = true;							// 폰트 굵게
				oRng.VerticalAlignment   = Excel.XlVAlign.xlVAlignCenter;	// 세로중앙정렬
				oRng.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;	// 가로중앙정렬	 
				oRng.Interior.Color      = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.CornflowerBlue);   //셀 배경색 
				oRng.Font.Color          = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.White);	  //텍스트색			
				
				//필요한 컬럼만 얻기
				object[,] items = new object[programAdHitDs.Report.Rows.Count, ColCount]; 

				DataCount = 1;
				// 데이터 추출
				for (int inx =0; inx < programAdHitDs.Report.Rows.Count; inx++)
				{

					string NowCategory = programAdHitDs.Report.Rows[inx]["CategoryName"].ToString();
					string NowGenre    = programAdHitDs.Report.Rows[inx]["GenreName"].ToString();   

					// 장르가 바뀌었을때
					if(!OldGenre.Equals(NowGenre))
					{
						if(!OldGenre.Equals(""))
						{
							oSheet.Cells[DataRow+DataCount,1] = "";
							oSheet.Cells[DataRow+DataCount,2] = "";					
							oSheet.Cells[DataRow+DataCount,3] = "소계";
							oSheet.Cells[DataRow+DataCount,4] = SumGenreAdHit;
							oSheet.Cells[DataRow+DataCount,5] = SumGenrePgHit;

							oRng = oSheet.get_Range(StartCol+Convert.ToString(DataRow+DataCount), EndCol+Convert.ToString(DataRow+DataCount)); // 헤더의 범위
							oRng.Font.Bold      = true;							// 폰트 굵게
                            oRng.Font.Size      = 9;
							oRng.Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.AliceBlue);   //셀 배경색 

							oRng = oSheet.get_Range("C"+Convert.ToString(DataRow+DataCount), "C"+Convert.ToString(DataRow+DataCount)); // 헤더의 범위
							oRng.VerticalAlignment   = Excel.XlVAlign.xlVAlignCenter;	// 세로중앙정렬
							oRng.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;	// 가로중앙정렬	 
                            
							SumGenreAdHit = 0;
							SumGenrePgHit = 0;
													
							DataCount++;
						}

					}
					
					// 카테고리가 바뀌었을때
					if(!OldCategory.Equals(NowCategory))
					{
                        // 장르가 같으면 위에서 첵크가 않되기 때문에 장르계를 따로 낸다
                        if(OldGenre.Equals(NowGenre))
                        {
                            if(!OldGenre.Equals(""))
                            {
                                oSheet.Cells[DataRow+DataCount,1] = "";
                                oSheet.Cells[DataRow+DataCount,2] = "";					
                                oSheet.Cells[DataRow+DataCount,3] = "소계";
                                oSheet.Cells[DataRow+DataCount,4] = SumGenreAdHit;
                                oSheet.Cells[DataRow+DataCount,5] = SumGenrePgHit;

                                oRng = oSheet.get_Range(StartCol+Convert.ToString(DataRow+DataCount), EndCol+Convert.ToString(DataRow+DataCount)); // 헤더의 범위
                                oRng.Font.Bold      = true;							// 폰트 굵게
                                oRng.Font.Size      = 9;
                                oRng.Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.AliceBlue);   //셀 배경색 

                                oRng = oSheet.get_Range("C"+Convert.ToString(DataRow+DataCount), "C"+Convert.ToString(DataRow+DataCount)); // 헤더의 범위
                                oRng.VerticalAlignment   = Excel.XlVAlign.xlVAlignCenter;	// 세로중앙정렬
                                oRng.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;	// 가로중앙정렬	 
                            
                                SumGenreAdHit = 0;
                                SumGenrePgHit = 0;
       							DataCount++;
                            }

                        }

						if(!OldCategory.Equals(""))
						{
							oSheet.Cells[DataRow+DataCount,1] = "";
							oSheet.Cells[DataRow+DataCount,2] = "";					
							oSheet.Cells[DataRow+DataCount,3] = "합계";
							oSheet.Cells[DataRow+DataCount,4] = SumCategoryAdHit;
							oSheet.Cells[DataRow+DataCount,5] = SumCategoryPgHit;

							oRng = oSheet.get_Range(StartCol+Convert.ToString(DataRow+DataCount), EndCol+Convert.ToString(DataRow+DataCount)); // 헤더의 범위
							oRng.Font.Bold           = true;							// 폰트 굵게
							oRng.Interior.Color      = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.LightSteelBlue);   //셀 배경색 

							oRng = oSheet.get_Range("C"+Convert.ToString(DataRow+DataCount), "C"+Convert.ToString(DataRow+DataCount)); // 헤더의 범위
							oRng.VerticalAlignment   = Excel.XlVAlign.xlVAlignCenter;	// 세로중앙정렬
							oRng.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;	// 가로중앙정렬	 

							SumCategoryAdHit = 0;
							SumCategoryPgHit = 0;

							DataCount++;
						}
					}

                    OldGenre = NowGenre;
					OldCategory = NowCategory;																		

					oSheet.Cells[DataRow+DataCount,1] = programAdHitDs.Report.Rows[inx]["CategoryName"].ToString();
					oSheet.Cells[DataRow+DataCount,2] = programAdHitDs.Report.Rows[inx]["GenreName"].ToString();					
					oSheet.Cells[DataRow+DataCount,3] = programAdHitDs.Report.Rows[inx]["ProgramNm"].ToString();
					oSheet.Cells[DataRow+DataCount,4] = Convert.ToInt32(programAdHitDs.Report.Rows[inx]["AdHitCnt"].ToString());
					oSheet.Cells[DataRow+DataCount,5] = Convert.ToInt32(programAdHitDs.Report.Rows[inx]["PgHitCnt"].ToString());
					
					DataCount++;

					int AdHit = Convert.ToInt32(programAdHitDs.Report.Rows[inx]["AdHitCnt"].ToString());
					int PgHit = Convert.ToInt32(programAdHitDs.Report.Rows[inx]["PgHitCnt"].ToString());

					// 합계를 구함
					TotalAdHit       += AdHit;
					TotalPgHit       += PgHit;
					SumCategoryAdHit += AdHit;
					SumCategoryPgHit += PgHit;
					SumGenreAdHit    += AdHit;
					SumGenrePgHit    += PgHit;


				}
				// 마지막 소계/합계
			
				oSheet.Cells[DataRow+DataCount,1] = "";
				oSheet.Cells[DataRow+DataCount,2] = "";					
				oSheet.Cells[DataRow+DataCount,3] = "소계";
				oSheet.Cells[DataRow+DataCount,4] = SumGenreAdHit;
				oSheet.Cells[DataRow+DataCount,5] = SumGenrePgHit;

				oRng = oSheet.get_Range(StartCol+Convert.ToString(DataRow+DataCount), EndCol+Convert.ToString(DataRow+DataCount)); // 헤더의 범위
				oRng.Font.Bold           = true;							// 폰트 굵게
				oRng.Interior.Color      = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.AliceBlue);   //셀 배경색 

				oRng = oSheet.get_Range("C"+Convert.ToString(DataRow+DataCount), "C"+Convert.ToString(DataRow+DataCount)); // 헤더의 범위
				oRng.VerticalAlignment   = Excel.XlVAlign.xlVAlignCenter;	// 세로중앙정렬
				oRng.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;	// 가로중앙정렬	 

				DataCount++;

				oSheet.Cells[DataRow+DataCount,1] = "";
				oSheet.Cells[DataRow+DataCount,2] = "";					
				oSheet.Cells[DataRow+DataCount,3] = "합계";
				oSheet.Cells[DataRow+DataCount,4] = SumCategoryAdHit;
				oSheet.Cells[DataRow+DataCount,5] = SumCategoryPgHit;

				oRng = oSheet.get_Range(StartCol+Convert.ToString(DataRow+DataCount), EndCol+Convert.ToString(DataRow+DataCount)); // 헤더의 범위
				oRng.Font.Bold           = true;							// 폰트 굵게
				oRng.Interior.Color      = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.LightSteelBlue);   //셀 배경색 

				oRng = oSheet.get_Range("C"+Convert.ToString(DataRow+DataCount), "C"+Convert.ToString(DataRow+DataCount)); // 헤더의 범위
				oRng.VerticalAlignment   = Excel.XlVAlign.xlVAlignCenter;	// 세로중앙정렬
				oRng.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;	// 가로중앙정렬	 


				// 총계 : 데이터부의 첫라인에 넣는다.
				oSheet.Cells[DataRow,1] = "";
				oSheet.Cells[DataRow,2] = "";					
				oSheet.Cells[DataRow,3] = "총계";
				oSheet.Cells[DataRow,4] = TotalAdHit;
				oSheet.Cells[DataRow,5] = TotalPgHit;

				oRng = oSheet.get_Range(StartCol+Convert.ToString(DataRow), EndCol+Convert.ToString(DataRow)); // 헤더의 범위
				oRng.Font.Bold           = true;							// 폰트 굵게
				oRng.Interior.Color      = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.LightSteelBlue);   //셀 배경색 

				oRng = oSheet.get_Range("C"+Convert.ToString(DataRow), "C"+Convert.ToString(DataRow)); // 헤더의 범위
				oRng.VerticalAlignment   = Excel.XlVAlign.xlVAlignCenter;	// 세로중앙정렬
				oRng.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;	// 가로중앙정렬	 


                // 데이터 부분 폰트 수정
                oRng = oSheet.get_Range(StartCol+Convert.ToString(HeaderRow), EndCol+Convert.ToString(DataRow+DataCount));
                oRng.Font.Size = 9;
                oRng.RowHeight = 14;

				// 데이터 작성
				oRng = oSheet.get_Range(StartCol+Convert.ToString(HeaderRow), EndCol+Convert.ToString(DataRow+DataCount));	// 데이터의 범위
//				oRng.set_Value(Missing.Value, items);			// 데이터의 셋트
				oRng.EntireColumn.AutoFit();					// 데이터의 크기에 셀의 가로크기 맞춤
				oRng.Borders.LineStyle =  Excel.XlLineStyle.xlContinuous;	// 테두리속성 아래에 실선
				oRng.Borders.Weight    = Excel.XlBorderWeight.xlThin;		// 테두리속성 아래에 가는선

				// 숫자형 데이터에 셀타입 설정
				oRng = oSheet.get_Range("D"+Convert.ToString(DataRow), EndCol+Convert.ToString(DataRow+DataCount));	// 데이터의 범위
				oRng.NumberFormatLocal = "#,##0";

				xlApp.Visible = true;
				xlApp.UserControl = true;


			}
			catch(Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
		}

		#endregion

        private void OnSearch(object sender, SearchReportData e)
        {
            StatusMessage("프로그램별 통계을 조회합니다.");
            ProgressStart();
			 
            try
            {
                // 데이터모델 초기화
                mainModel.Init();
                mainModel.SearchMediaCode   =  e.MediaCode;
                mainModel.SearchContractSeq =  e.ContractSeq;
                mainModel.CampaignCode		=  e.CampaignNo;
                mainModel.SearchItemNo      =  e.ItemNo;
                mainModel.SearchBgnDay    =  e.ItemBeginDay;
                mainModel.SearchEndDay  	=  e.ItemEndDay;
			 
		 
                keyMediaName      = "";
                keyContractName   = "";
                keyAgencyName     = "";
                keyAdvertiserName = "";
                keyItemNo         = "";
                keyItemName       = "";
                keyReportBgnDay   = "";
                keyReportEndDay   = "";
			 
                uiPanelList.Text = "프로그램별 통계"; 
			 
                //  전체통계 서비스를 호출한다.
                new ProgramAdHitManager(systemModel,commonModel).GetProgramAdHitReport(mainModel);
	
                if (mainModel.ResultCD.Equals("0000"))
                {
                    Utility.SetDataTable(programAdHitDs.Report, mainModel.ReportDataSet);		
                    StatusMessage(mainModel.ResultCnt + "건이 조회되었습니다.");
			 	
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
                FrameSystem.showMsgForm("프로그램별 통계 조회오류", new string[] {fe.ErrCode, fe.ResultMsg});
            }
            catch(Exception ex)
            {
                rptHeader.u_IsPrint = false;
                FrameSystem.showMsgForm("프로그램별 통계 조회오류",new string[] {"",ex.Message});
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

        private void rptHeader_Load(object sender, EventArgs e)
        {

        }

    }
}
