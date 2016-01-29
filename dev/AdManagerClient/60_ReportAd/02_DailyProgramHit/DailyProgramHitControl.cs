// ===============================================================================
//
// DailyProgramHitControl.cs
//
// 일별 프로그램 시청집계 컨트롤을 정의합니다. 
//
// ===============================================================================
// Release history
//
// ===============================================================================
// Copyright (C) 2007 G-Inno Systems Inc.
// All rights reserved.
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
    public class DailyProgramHitControl : System.Windows.Forms.UserControl, IUserControl
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
        DailyProgramHitModel mainModel  = new DailyProgramHitModel();	// 프로그램별 광고시청집계모델
		//SummaryAdModel summaryAdModel        = new SummaryAdModel();	// 총괄집계모델

        string keyMediaName         = "";
        //string keyContractSeq       = "";
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
		int maxColumn = 0;

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
		private Janus.Windows.GridEX.GridEX grdExHitList;
		private AdManagerClient.DailyProgramHitDs dailyProgramHitDs;
        private AdManagerClient.ReportHeaderControl rptHeader;
        private System.ComponentModel.IContainer components;

        public DailyProgramHitControl()
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
			Janus.Windows.GridEX.GridEXLayout grdExHitList_DesignTimeLayout = new Janus.Windows.GridEX.GridEXLayout();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DailyProgramHitControl));
			this.uiPM = new Janus.Windows.UI.Dock.UIPanelManager(this.components);
			this.uiPanelChoiceAdSchedule = new Janus.Windows.UI.Dock.UIPanelGroup();
			this.uiPanelSearch = new Janus.Windows.UI.Dock.UIPanel();
			this.uiPanelSearchContainer = new Janus.Windows.UI.Dock.UIPanelInnerContainer();
			this.pnlSearch = new System.Windows.Forms.Panel();
			this.rptHeader = new AdManagerClient.ReportHeaderControl();
			this.uiPanelList = new Janus.Windows.UI.Dock.UIPanel();
			this.uiPanelListContainer = new Janus.Windows.UI.Dock.UIPanelInnerContainer();
			this.grdExHitList = new Janus.Windows.GridEX.GridEX();
			this.dailyProgramHitDs = new AdManagerClient.DailyProgramHitDs();
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
			((System.ComponentModel.ISupportInitialize)(this.grdExHitList)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.dailyProgramHitDs)).BeginInit();
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
			this.uiPanelChoiceAdSchedule.ActiveCaptionMode = Janus.Windows.UI.Dock.ActiveCaptionMode.Never;
			this.uiPanelChoiceAdSchedule.BackColor = System.Drawing.SystemColors.Window;
			this.uiPanelChoiceAdSchedule.CaptionVisible = Janus.Windows.UI.InheritableBoolean.True;
			this.uiPanelChoiceAdSchedule.CloseButtonVisible = Janus.Windows.UI.InheritableBoolean.False;
			this.uiPanelChoiceAdSchedule.Location = new System.Drawing.Point(0, 0);
			this.uiPanelChoiceAdSchedule.Name = "uiPanelChoiceAdSchedule";
			this.uiPanelChoiceAdSchedule.Size = new System.Drawing.Size(1010, 677);
			this.uiPanelChoiceAdSchedule.TabIndex = 4;
			this.uiPanelChoiceAdSchedule.Text = "일별 프로그램 시청집계";
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
			this.rptHeader.Location = new System.Drawing.Point(0, 0);
			this.rptHeader.Name = "rptHeader";
			this.rptHeader.Size = new System.Drawing.Size(1008, 111);
			this.rptHeader.TabIndex = 0;
			this.rptHeader.u_IsPrint = false;
			this.rptHeader.u_MenuName = "";
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
			this.uiPanelList.Text = "일별시청횟수";
			// 
			// uiPanelListContainer
			// 
			this.uiPanelListContainer.BackColor = System.Drawing.SystemColors.Window;
			this.uiPanelListContainer.Controls.Add(this.grdExHitList);
			this.uiPanelListContainer.Location = new System.Drawing.Point(1, 23);
			this.uiPanelListContainer.Name = "uiPanelListContainer";
			this.uiPanelListContainer.Size = new System.Drawing.Size(1008, 514);
			this.uiPanelListContainer.TabIndex = 0;
			// 
			// grdExHitList
			// 
			this.grdExHitList.AllowEdit = Janus.Windows.GridEX.InheritableBoolean.False;
			this.grdExHitList.AlternatingColors = true;
			this.grdExHitList.BorderStyle = Janus.Windows.GridEX.BorderStyle.None;
			this.grdExHitList.DataSource = this.dailyProgramHitDs.Report;
			grdExHitList_DesignTimeLayout.LayoutString = resources.GetString("grdExHitList_DesignTimeLayout.LayoutString");
			this.grdExHitList.DesignTimeLayout = grdExHitList_DesignTimeLayout;
			this.grdExHitList.Dock = System.Windows.Forms.DockStyle.Fill;
			this.grdExHitList.EditorsControlStyle.ButtonAppearance = Janus.Windows.GridEX.ButtonAppearance.Regular;
			this.grdExHitList.EmptyRows = true;
			this.grdExHitList.FocusCellFormatStyle.BackColor = System.Drawing.SystemColors.Highlight;
			this.grdExHitList.FocusCellFormatStyle.ForeColor = System.Drawing.Color.White;
			this.grdExHitList.FocusStyle = Janus.Windows.GridEX.FocusStyle.None;
			this.grdExHitList.GridLineColor = System.Drawing.Color.Silver;
			this.grdExHitList.GridLines = Janus.Windows.GridEX.GridLines.Vertical;
			this.grdExHitList.GridLineStyle = Janus.Windows.GridEX.GridLineStyle.Solid;
			this.grdExHitList.GroupByBoxInfoFormatStyle.BackColor = System.Drawing.Color.Empty;
			this.grdExHitList.GroupByBoxVisible = false;
			this.grdExHitList.GroupRowFormatStyle.BackColor = System.Drawing.Color.White;
			this.grdExHitList.GroupRowFormatStyle.BackColorGradient = System.Drawing.Color.FromArgb(((int)(((byte)(218)))), ((int)(((byte)(228)))), ((int)(((byte)(238)))));
			this.grdExHitList.GroupRowVisualStyle = Janus.Windows.GridEX.GroupRowVisualStyle.Outlook2003;
			this.grdExHitList.GroupTotalRowFormatStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
			this.grdExHitList.GroupTotalRowFormatStyle.FontBold = Janus.Windows.GridEX.TriState.True;
			this.grdExHitList.GroupTotalRowFormatStyle.ForeColor = System.Drawing.SystemColors.ControlText;
			this.grdExHitList.GroupTotals = Janus.Windows.GridEX.GroupTotals.Always;
			this.grdExHitList.HeaderFormatStyle.TextAlignment = Janus.Windows.GridEX.TextAlignment.Center;
			this.grdExHitList.HideSelection = Janus.Windows.GridEX.HideSelection.Highlight;
			this.grdExHitList.Location = new System.Drawing.Point(0, 0);
			this.grdExHitList.Name = "grdExHitList";
			this.grdExHitList.SelectedInactiveFormatStyle.BackColor = System.Drawing.Color.Gold;
			this.grdExHitList.Size = new System.Drawing.Size(1008, 514);
			this.grdExHitList.TabIndex = 13;
			this.grdExHitList.TabKeyBehavior = Janus.Windows.GridEX.TabKeyBehavior.ControlNavigation;
			this.grdExHitList.ThemedAreas = ((Janus.Windows.GridEX.ThemedArea)(((((((((Janus.Windows.GridEX.ThemedArea.ScrollBars | Janus.Windows.GridEX.ThemedArea.EditControls) 
            | Janus.Windows.GridEX.ThemedArea.Headers) 
            | Janus.Windows.GridEX.ThemedArea.GroupByBox) 
            | Janus.Windows.GridEX.ThemedArea.GroupRows) 
            | Janus.Windows.GridEX.ThemedArea.ControlBorder) 
            | Janus.Windows.GridEX.ThemedArea.Cards) 
            | Janus.Windows.GridEX.ThemedArea.Gridlines) 
            | Janus.Windows.GridEX.ThemedArea.CheckBoxes)));
			this.grdExHitList.TotalRow = Janus.Windows.GridEX.InheritableBoolean.True;
			this.grdExHitList.TotalRowFormatStyle.BackColor = System.Drawing.Color.Gold;
			this.grdExHitList.TotalRowFormatStyle.FontBold = Janus.Windows.GridEX.TriState.True;
			this.grdExHitList.TotalRowFormatStyle.ForeColor = System.Drawing.SystemColors.ControlText;
			this.grdExHitList.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
			// 
			// dailyProgramHitDs
			// 
			this.dailyProgramHitDs.DataSetName = "DailyProgramHitDs";
			this.dailyProgramHitDs.Locale = new System.Globalization.CultureInfo("en-US");
			this.dailyProgramHitDs.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
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
			// DailyProgramHitControl
			// 
			this.Controls.Add(this.uiPanelChoiceAdSchedule);
			this.Font = global::AdManagerClient.Properties.Settings.Default.Font1;
			this.Name = "DailyProgramHitControl";
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
			((System.ComponentModel.ISupportInitialize)(this.grdExHitList)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.dailyProgramHitDs)).EndInit();
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

        #region 처리메소드

        /// <summary>
        /// 일별시청률 조회
        /// </summary>
        private void SearchReport()
        {
            StatusMessage("일별 프로그램 시청집계를 조회합니다.");

//			try
//			{
//				// 데이터모델 초기화
//				dailyProgramHitModel.Init();
//
//				dailyProgramHitModel.SearchMediaCode	=  cbSearchMedia.SelectedValue.ToString(); 
//				dailyProgramHitModel.SearchContractSeq	=  cbSearchContract.SelectedValue.ToString(); 
//				dailyProgramHitModel.SearchItemNo		=  cbSearchItem.SelectedValue.ToString(); 
//				dailyProgramHitModel.CampaignCode		=  cbCampaign.SelectedValue.ToString(); 
//
//				if(rbSearchType_D.Checked)
//				{
//					dailyProgramHitModel.SearchType	  	=  "D"; // -- D:선택기간 C:계약기간
//					dailyProgramHitModel.SearchBgnDay  	=  cbSearchBgnDay.Value.ToString("yyMMdd");
//					dailyProgramHitModel.SearchEndDay  	=  cbSearchEndDay.Value.ToString("yyMMdd");
//				}
//				else
//				{
//					dailyProgramHitModel.SearchType	  	=  "C"; 
//					dailyProgramHitModel.SearchBgnDay  	=  Utility.convertDate(ebExcuteStartDay.Text);
//					dailyProgramHitModel.SearchEndDay  	=  Utility.convertDate(ebExcuteEndDay.Text);
//				}
//				keyMediaName    = "";
//				keyContractSeq  = "";
//				keyContractName = "";
//				keyItemNo       = "";
//				keyItemName     = "";
//				keyReportBgnDay = "";
//				keyReportEndDay = "";
//
//				uiPanelList.Text = "시청횟수"; 
//
//				//  일별 프로그램시청집계조회 서비스를 호출한다.
//				new DailyProgramHitManager(systemModel,commonModel).GetDailyProgramHitReport(dailyProgramHitModel);
//
//				if (dailyProgramHitModel.ResultCD.Equals("0000"))
//				{
//					Utility.SetDataTable(dailyProgramHitDs.RowData, dailyProgramHitModel.ReportDataSet);		
//					Utility.SetDataTable(dailyProgramHitDs.Header,  dailyProgramHitModel.HeaderDataSet);		
//					StatusMessage(dailyProgramHitModel.ResultCnt + "건이 조회되었습니다.");
//				
//					keyMediaName    = cbSearchMedia.SelectedItem.Text;
//					keyContractSeq  = cbSearchContract.SelectedValue.ToString(); 
//					keyContractName = cbSearchContract.SelectedItem.Text; 
//					keyItemNo       = cbSearchItem.SelectedValue.ToString();
//					keyItemName     = cbSearchItem.SelectedItem.Text;
//					if(rbSearchType_D.Checked)
//					{
//						keyReportBgnDay = cbSearchBgnDay.Value.ToString("yyyy-MM-dd");
//						keyReportEndDay = cbSearchEndDay.Value.ToString("yyyy-MM-dd");
//					}
//					else
//					{
//						keyReportBgnDay = ebExcuteStartDay.Text;
//						keyReportEndDay = ebExcuteEndDay.Text;
//					}
//
//					uiPanelList.Text = "일별시청횟수 : " + keyMediaName + " / " + keyContractName ;
//
//					if(!keyItemNo.Equals("") && !keyItemNo.Equals("00"))
//					{
//						uiPanelList.Text += " / [" + keyItemNo    + "]" + keyItemName;
//					}
//					else
//					{
//						uiPanelList.Text += " / 전체광고";
//					}
//					
//					uiPanelList.Text += " / " + keyReportBgnDay + " ~ " + keyReportEndDay;	
//			
//					canPrint = true;
//
//					// 그리드의 헤더를 다시 셋트한다.
//					SetListHeader();
//
//					//Row데이터를 리포트용 데이터로 재구성한다.
//					SetListData();
//				}
//			}
//			catch(FrameException fe)
//			{
//				FrameSystem.showMsgForm("일별 프로그램 시청횟수 조회오류", new string[] {fe.ErrCode, fe.ResultMsg});
//			}
//			catch(Exception ex)
//			{
//				FrameSystem.showMsgForm("일별 프로그램 시청횟수 조회오류",new string[] {"",ex.Message});
//			}
//			finally
//			{
//				ProgressStop();
//			}
        }
		

		private void SetListData()
		{
			int    GenreIndex = 0;
			string OldDay   = "";
			string LogDay   = "";
			int    SumDay   = 0;
			

			dailyProgramHitDs.Report.Clear();		// 데이터 클리어

			DataRow reportRow = dailyProgramHitDs.Report.NewRow();

			for(int i=0;i<dailyProgramHitDs.RowData.Rows.Count;i++)
			{
				DataRow dataRow = dailyProgramHitDs.RowData.Rows[i];

				LogDay = dataRow["LogDay"].ToString();

				if(!OldDay.Equals(LogDay))
				{
					if(!OldDay.Equals(""))
					{
						reportRow["LogDay"] = Utility.reConvertDate("20" + OldDay);
						reportRow["SumDay"] = SumDay;				// 일계셋트
						SumDay = 0;

						dailyProgramHitDs.Report.Rows.Add(reportRow); // 리포트데이터에 Row셋트
					}

					reportRow = dailyProgramHitDs.Report.NewRow();
					OldDay = LogDay;
					GenreIndex = 0;		// 컬럼번호 초기화
				}		

				reportRow["Genre"+GenreIndex.ToString()] =  Convert.ToInt32(dataRow["HitCnt"].ToString());  // 컬럼값을 셋트
				SumDay += Convert.ToInt32(dataRow["HitCnt"].ToString());
				GenreIndex++;
			}

			// 마지막 Row
			if(!OldDay.Equals(""))
			{
				reportRow["SumDay"] = SumDay;
				reportRow["LogDay"] = Utility.reConvertDate("20" + OldDay);
				dailyProgramHitDs.Report.Rows.Add(reportRow); // 리포트데이터에 Row셋트
			}	
		}

		private void SetListHeader()
		{
			StringBuilder LayoutXml = null;

            // 쿼리생성
			LayoutXml = new StringBuilder();

			LayoutXml.Append("\n"
				+ "<GridEXLayoutData>                                                  \n"
				+ "  <RootTable>                                                       \n"
				+ "    <Caption>Report</Caption>                                       \n"
				+ "    <CellLayoutMode>UseColumnSets</CellLayoutMode>                  \n"
				+ "    <ColumnHeaders>True</ColumnHeaders>                             \n"
				+ "    <ColumnSets Collection=\"true\">                                \n"
				

			// 컬럼셋정보셋트

				// 일자와 합계 컬럼
				+ "      <ColumnSet0 ID=\"ColumnSet0\">                                \n"
				+ "        <Key>ColumnSet0</Key>                                       \n"
				+ "        <Position>0</Position>                                      \n"
				+ "        <ColumnWidth0>100</ColumnWidth0>                            \n"
				+ "        <ColumnWidth1>100</ColumnWidth1>                            \n"
				+ "        <EntriesCount>2</EntriesCount>                              \n"
				+ "        <ColIndex0>0</ColIndex0>                                    \n"
				+ "        <Row0>0</Row0>                                              \n"
				+ "        <Col0>0</Col0>                                              \n"
				+ "        <RowSpan0>1</RowSpan0>                                      \n"
				+ "        <ColSpan0>1</ColSpan0>                                      \n"
				+ "        <ColIndex1>1</ColIndex1>                                    \n"
				+ "        <Row1>0</Row1>                                              \n"
				+ "        <Col1>1</Col1>                                              \n"
				+ "        <RowSpan1>1</RowSpan1>                                      \n"
				+ "        <ColSpan1>1</ColSpan1>                                      \n"
				+ "      </ColumnSet0>                                                 \n"
				);

			// 카테고리셋트
			// 
			string OldCategory = "";
			string NowCategory = "";
			string NowGenre    = "";
			int    ColIndex      = 2;  // 일자, 합계 다음부터 이므로
			int    ColumnSet     = 0;  
			int    Position      = 1;
			int    ColumnCount   = 0;

			for(int i=0;i<dailyProgramHitDs.Header.Rows.Count;i++)
			{
				DataRow Row = dailyProgramHitDs.Header.Rows[i];

				NowCategory = Row["CategoryName"].ToString().Replace("&","_");
				NowGenre    = Row["GenreName"].ToString().Replace("&","_");

				if(!OldCategory.Equals(NowCategory))
				{
					if(!OldCategory.Equals("")) 
					{ 
						// 카테고리의 끝부분
						LayoutXml.Append("  <ColumnCount>" + ColumnCount.ToString() + "</ColumnCount>\n");
						LayoutXml.Append("  <Position>" + Position.ToString() + "</Position>\n");
						for(int j=0;j<ColumnCount;j++)
						{
							LayoutXml.Append("  <ColumnWidth" + j.ToString() + ">100</ColumnWidth" + j.ToString() + ">\n");
						}
						LayoutXml.Append("  <EntriesCount>" + ColumnCount.ToString() + "</EntriesCount>\n");							
						LayoutXml.Append("</ColumnSet" + ColumnSet + ">\n");

						ColumnCount = 0;
						Position++;
					}

					ColumnSet++;
					// 카테고리 시작부분
					LayoutXml.Append("<ColumnSet" + ColumnSet + " ID=\"ColumnSet" + ColumnSet + "\">\n");
					LayoutXml.Append("  <Key>ColumnSet" + ColumnSet + "</Key>\n");
					LayoutXml.Append("  <Caption>"+NowCategory+"</Caption>\n");

					OldCategory = NowCategory;
				}

				// 장르정보 부분
				LayoutXml.Append("  <ColIndex"+ColumnCount.ToString() + ">"+ColIndex.ToString()+"</ColIndex"+ColumnCount.ToString() + ">\n");
				LayoutXml.Append("  <Row"+ColumnCount.ToString() + ">0</Row"+ColumnCount.ToString() + ">\n");
				LayoutXml.Append("  <Col"+ColumnCount.ToString() + ">"+ColumnCount.ToString()+"</Col"+ColumnCount.ToString() + ">\n");
				LayoutXml.Append("  <RowSpan"+ColumnCount.ToString() + ">1</RowSpan"+ColumnCount.ToString() + ">\n");
				LayoutXml.Append("  <ColSpan"+ColumnCount.ToString() + ">1</ColSpan"+ColumnCount.ToString() + ">\n");
				ColumnCount++;
				ColIndex++;			
			}

			// 마지막 카테고리정보 끝부분
			if(!OldCategory.Equals("")) 
			{ 
				// 카테고리의 끝부분
				LayoutXml.Append("  <ColumnCount>" + ColumnCount.ToString() + "</ColumnCount>\n");
				LayoutXml.Append("  <Position>" + Position.ToString() + "</Position>\n");
				for(int j=0;j<ColumnCount;j++)
				{
					LayoutXml.Append("  <ColumnWidth" + j.ToString() + ">100</ColumnWidth" + j.ToString() + ">\n");
				}
				LayoutXml.Append("  <EntriesCount>" + ColumnCount.ToString() + "</EntriesCount>\n");							
				LayoutXml.Append("</ColumnSet" + ColumnSet + ">\n");

			}

			// 컬럼정보셋트
			LayoutXml.Append(""
				+ "    </ColumnSets>                                                   \n"
				+ "    <Columns Collection=\"true\">                                   \n"
				+ "      <Column0 ID=\"LogDay\">                                       \n"
				+ "        <Bound>True</Bound>                                         \n"
				+ "        <Caption>일자</Caption>                                     \n"
				+ "        <DataMember>LogDay</DataMember>                             \n"
				+ "        <Key>LogDay</Key>                                           \n"
				+ "        <Position>0</Position>                                      \n"
				+ "        <TextAlignment>Center</TextAlignment>                       \n"
				+ "      </Column0>                                                    \n"
				+ "      <Column1 ID=\"SumDay\">                                       \n"
				+ "        <Bound>True</Bound>                                         \n"
				+ "        <Caption>합계</Caption>                                     \n"
				+ "        <DataMember>SumDay</DataMember>                             \n"
				+ "        <FormatString>#,##0</FormatString>                          \n"
				+ "        <Key>SumDay</Key>                                           \n"
				+ "        <Position>1</Position>                                      \n"
				+ "        <TextAlignment>Far</TextAlignment>                          \n"
				+ "        <AggregateFunction>Sum</AggregateFunction>                  \n"
				+ "        <TotalFormatString>#,##0</TotalFormatString>                \n"
				+ "      </Column1>                                                    \n"
				);

			ColIndex      = 2;  // 일자, 합계 다음부터 다시

			for(int i=0;i<dailyProgramHitDs.Header.Rows.Count;i++)
			{
				DataRow Row = dailyProgramHitDs.Header.Rows[i];

				NowCategory = Row["CategoryName"].ToString().Replace("&","/");
				NowGenre    = Row["GenreName"].ToString().Replace("&","/");

				// 장르정보 부분
				LayoutXml.Append("    <Column"+ColIndex.ToString() + " ID=\"Genre"+i.ToString() + "\">\n");
				LayoutXml.Append("      <Bound>True</Bound>\n");
				LayoutXml.Append("      <Caption>"+NowGenre+"</Caption>\n");
				LayoutXml.Append("        <DataMember>Genre"+i.ToString() + "</DataMember>\n");
				LayoutXml.Append("        <FormatString>#,##0</FormatString>\n");
				LayoutXml.Append("        <Position>"+ColIndex.ToString() + "</Position>\n");
				LayoutXml.Append("        <TextAlignment>Far</TextAlignment>\n");
				LayoutXml.Append("        <AggregateFunction>Sum</AggregateFunction>\n");
				LayoutXml.Append("        <TotalFormatString>#,##0</TotalFormatString>\n");
				LayoutXml.Append("    </Column"+ColIndex.ToString()+">\n");
				ColumnCount++;
				ColIndex++;			
			}

			LayoutXml.Append(""
				+ "    </Columns>                                                      \n"
				+ "    <ColumnSetHeaders>True</ColumnSetHeaders>                       \n"
				+ "    <ColumnSetRowCount>1</ColumnSetRowCount>                        \n"
				+ "    <GroupCondition ID=\"\" />                                      \n"
				+ "    <Key>Report</Key>                                               \n"
				+ "  </RootTable>                                                      \n"
				+ "</GridEXLayoutData>                                                 \n"
				);


			maxColumn = ColIndex;  // 전체컬럼수 저장. 엑셀에서 사용

			FrameSystem.oLog.Debug(LayoutXml.ToString());

			// 작성된 레이아웃 XML을 그리드에 로드한다.
			Janus.Windows.GridEX.GridEXLayout layout = new Janus.Windows.GridEX.GridEXLayout();
			layout.LayoutString = LayoutXml.ToString();
			this.grdExHitList.LoadLayout(layout);

			// 그리드를 초기화한다.

			#region 그리드 초기화
			this.grdExHitList.AllowEdit = Janus.Windows.GridEX.InheritableBoolean.False;
			this.grdExHitList.AlternatingColors = true;
			this.grdExHitList.BorderStyle = Janus.Windows.GridEX.BorderStyle.None;
			this.grdExHitList.DataSource = this.dailyProgramHitDs.Report;
			this.grdExHitList.Dock = System.Windows.Forms.DockStyle.Fill;
			this.grdExHitList.EditorsControlStyle.ButtonAppearance = Janus.Windows.GridEX.ButtonAppearance.Regular;
			this.grdExHitList.EmptyRows = true;
			this.grdExHitList.FocusCellFormatStyle.BackColor = System.Drawing.SystemColors.Highlight;
			this.grdExHitList.FocusCellFormatStyle.ForeColor = System.Drawing.Color.White;
			this.grdExHitList.FocusStyle = Janus.Windows.GridEX.FocusStyle.None;
			this.grdExHitList.Font = new System.Drawing.Font("굴림체", 9F);
			this.grdExHitList.GridLineColor = System.Drawing.Color.Silver;
			this.grdExHitList.GridLines = Janus.Windows.GridEX.GridLines.Vertical;
			this.grdExHitList.GridLineStyle = Janus.Windows.GridEX.GridLineStyle.Solid;
			this.grdExHitList.GroupByBoxInfoFormatStyle.BackColor = System.Drawing.Color.Empty;
			this.grdExHitList.GroupByBoxVisible = false;
			this.grdExHitList.GroupRowFormatStyle.BackColor = System.Drawing.Color.White;
			this.grdExHitList.GroupRowFormatStyle.BackColorGradient = System.Drawing.Color.FromArgb(((System.Byte)(218)), ((System.Byte)(228)), ((System.Byte)(238)));
			this.grdExHitList.GroupRowVisualStyle = Janus.Windows.GridEX.GroupRowVisualStyle.Outlook2003;
			this.grdExHitList.GroupTotalRowFormatStyle.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(255)), ((System.Byte)(192)), ((System.Byte)(255)));
			this.grdExHitList.GroupTotalRowFormatStyle.FontBold = Janus.Windows.GridEX.TriState.True;
			this.grdExHitList.GroupTotalRowFormatStyle.ForeColor = System.Drawing.SystemColors.ControlText;
			this.grdExHitList.GroupTotals = Janus.Windows.GridEX.GroupTotals.Always;
			this.grdExHitList.HeaderFormatStyle.TextAlignment = Janus.Windows.GridEX.TextAlignment.Center;
			this.grdExHitList.HideSelection = Janus.Windows.GridEX.HideSelection.Highlight;
			this.grdExHitList.Location = new System.Drawing.Point(0, 0);
			this.grdExHitList.Name = "grdExHitList";
			this.grdExHitList.SelectedInactiveFormatStyle.BackColor = System.Drawing.Color.Gold;
			this.grdExHitList.Size = new System.Drawing.Size(849, 521);
			this.grdExHitList.TabIndex = 12;
			this.grdExHitList.ThemedAreas = ((Janus.Windows.GridEX.ThemedArea)(((((((((Janus.Windows.GridEX.ThemedArea.ScrollBars | Janus.Windows.GridEX.ThemedArea.EditControls) 
				| Janus.Windows.GridEX.ThemedArea.Headers) 
				| Janus.Windows.GridEX.ThemedArea.GroupByBox) 
				| Janus.Windows.GridEX.ThemedArea.GroupRows) 
				| Janus.Windows.GridEX.ThemedArea.ControlBorder) 
				| Janus.Windows.GridEX.ThemedArea.Cards) 
				| Janus.Windows.GridEX.ThemedArea.Gridlines) 
				| Janus.Windows.GridEX.ThemedArea.CheckBoxes)));
			this.grdExHitList.TotalRow = Janus.Windows.GridEX.InheritableBoolean.True;
			this.grdExHitList.TotalRowFormatStyle.BackColor = System.Drawing.Color.Gold;
			this.grdExHitList.TotalRowFormatStyle.FontBold = Janus.Windows.GridEX.TriState.True;
			this.grdExHitList.TotalRowFormatStyle.ForeColor = System.Drawing.SystemColors.ControlText;
			this.grdExHitList.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2003;
			
			#endregion
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

				int ColCount  = maxColumn; // 컬럼수				

				int TitleRow  = 1;
				int ConditionRow = 2;
				int HeaderRow = 8;
				int DataRow   = 10;
				string StartCol = "A";
				string EndCol   = "";
				string TitleCol = "H";
				int DataCount = 0;
				int GenreCount = 0;

				// 헤더의 장르의 갯수
				GenreCount = dailyProgramHitDs.Header.Rows.Count;

				// 마지막 컬럼의 인덱스문자
				EndCol = GetColumnIndex(ColCount);

				xlApp = new Excel.Application();
				oWB = (Excel._Workbook)(xlApp.Workbooks.Add(Missing.Value));
				oSheet = (Excel._Worksheet)oWB.ActiveSheet;


				// 타이틀 작성
				oSheet.Cells[TitleRow,1] = "일별 프로그램시청 집계";
				oRng = oSheet.get_Range(StartCol+Convert.ToString(TitleRow), TitleCol+Convert.ToString(TitleRow));
				oRng.Merge(true);
				oRng.Font.Bold = true;
				oRng.Font.Size = 16;
				oRng.VerticalAlignment   = Excel.XlVAlign.xlVAlignCenter;			
				oRng.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;		

				// 조건정보 작성
				oSheet.Cells[ConditionRow,1] = "매체";
				oRng = oSheet.get_Range("B"+Convert.ToString(ConditionRow), TitleCol+Convert.ToString(ConditionRow));
				oRng.Merge(true);
				oSheet.Cells[ConditionRow,2] = keyMediaName;

				oSheet.Cells[ConditionRow+1,1] = "계약명";
				oRng = oSheet.get_Range("B"+Convert.ToString(ConditionRow+1), TitleCol+Convert.ToString(ConditionRow+1));
				oRng.Merge(true);
				oSheet.Cells[ConditionRow+1,2] = keyContractName;

                oSheet.Cells[ConditionRow+2,1] = "캠페인";
                oRng = oSheet.get_Range("B"+Convert.ToString(ConditionRow+2), TitleCol+Convert.ToString(ConditionRow+2));
                oRng.Merge(true);
                oSheet.Cells[ConditionRow+2,2] = keyCampaignName;
					
				oSheet.Cells[ConditionRow+3,1] = "광고명";
				oRng = oSheet.get_Range("B"+Convert.ToString(ConditionRow+3), TitleCol+Convert.ToString(ConditionRow+3));
				oRng.Merge(true);
				oSheet.Cells[ConditionRow+3,2] = "[" + keyItemNo + "] " + keyItemName;

				oSheet.Cells[ConditionRow+4,1] = "집계기간";
				oRng = oSheet.get_Range("B"+Convert.ToString(ConditionRow+4), TitleCol+Convert.ToString(ConditionRow+4));
				oRng.Merge(true);
                string ReportType =  Utility.reConvertDate("20" + keyReportBgnDay) + " ~ " + Utility.reConvertDate("20" + keyReportEndDay);
                ReportType += " (" + keyReportType + ")";
                oSheet.Cells[ConditionRow+4,2] = ReportType;

				// 조건부 테두리
				oRng = oSheet.get_Range(StartCol+Convert.ToString(ConditionRow), TitleCol+Convert.ToString(ConditionRow+4));
				oRng.VerticalAlignment   = Excel.XlVAlign.xlVAlignCenter;			
				oRng.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;		
				oRng.Borders.LineStyle =  Excel.XlLineStyle.xlContinuous;	// 테두리속성 아래에 실선
				oRng.Borders.Weight    = Excel.XlBorderWeight.xlThin;		// 테두리속성 아래에 가는선
                oRng.Font.Size          = 10;


				// 헤더 정보 작성
				oSheet.Cells[HeaderRow+1,1] = "일자";
				oSheet.Cells[HeaderRow+1,2] = "합계";


				string OldCategory = "";
				string NowCategory = "";
				int bgncol  = 3;	// 처음 장르가 시작되는 컬럼위치 3번째부터
				int colcnt  = 0;   

				for(int i=0;i<GenreCount;i++)
				{
					DataRow Row = dailyProgramHitDs.Header.Rows[i];				// 헤더데이터로부터 장르를 헤더로 셋트한다.

					NowCategory =  Row["CategoryName"].ToString();	// 카테고리명
					if(!OldCategory.Equals(NowCategory))
					{
						if(!OldCategory.Equals(""))			// 카테고리가 바뀌었으면 장르수만큼 머지하고 카테고리명 셋트
						{ 
							oRng = oSheet.get_Range(GetColumnIndex(bgncol)+Convert.ToString(HeaderRow), GetColumnIndex(bgncol+colcnt-1)+Convert.ToString(HeaderRow));
							oRng.Merge(true);

							oSheet.Cells[HeaderRow,bgncol] = OldCategory;
							bgncol += colcnt;
							colcnt  = 0;
						}
					}

					OldCategory = NowCategory;

					oSheet.Cells[HeaderRow+1,3+i] = Row["GenreName"].ToString(); //장르명 . 처음 장르가 시작되는 컬럼위치 3번째부터

					colcnt++;
				}	
				if(!OldCategory.Equals("")) 
				{ 
					oRng = oSheet.get_Range(GetColumnIndex(bgncol)+Convert.ToString(HeaderRow), GetColumnIndex(ColCount)+Convert.ToString(HeaderRow));
					oRng.Merge(true);
					oSheet.Cells[HeaderRow,bgncol] = OldCategory;
				}		


				oRng = oSheet.get_Range(StartCol+Convert.ToString(HeaderRow), EndCol+Convert.ToString(HeaderRow+1)); // 헤더의 범위
				oRng.Font.Bold           = true;							// 폰트 굵게
				oRng.VerticalAlignment   = Excel.XlVAlign.xlVAlignCenter;	// 세로중앙정렬
				oRng.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;	// 가로중앙정렬	 
				oRng.Interior.Color      = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.CornflowerBlue);   //셀 배경색 
				oRng.Font.Color          = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.White);	  //텍스트색			
				
				//합계를 위한
				int TotalSumDay = 0;
				int[] TotalHit = new int[GenreCount];

				for(int i=0;i<GenreCount;i++)
				{
					TotalHit[i] = 0; // 초기화
				}

				DataCount = 1;
				// 데이터 추출
				for (int inx =0; inx < dailyProgramHitDs.Report.Rows.Count; inx++)
				{

					oSheet.Cells[DataRow+DataCount,1] = dailyProgramHitDs.Report.Rows[inx]["LogDay"].ToString();
					int SumDay = Convert.ToInt32(dailyProgramHitDs.Report.Rows[inx]["SumDay"].ToString());	
					oSheet.Cells[DataRow+DataCount,2] = SumDay;	
					TotalSumDay       += SumDay;
			
					for(int i=0;i<GenreCount;i++)
					{
						DataRow Row = dailyProgramHitDs.Header.Rows[i];				// 헤더데이터로부터 장르를 헤더로 셋트한다.

						int Hit = Convert.ToInt32(dailyProgramHitDs.Report.Rows[inx]["Genre"+i.ToString()].ToString());
						oSheet.Cells[DataRow+DataCount,3+i] = Hit;
						TotalHit[i] += Hit;
					}					
					DataCount++;
				}

				// 총계 : 데이터부의 첫라인에 넣는다.			
				oSheet.Cells[DataRow,1] = "합계";
				oSheet.Cells[DataRow,2] = TotalSumDay;	
				for(int i=0;i<GenreCount;i++)
				{
					oSheet.Cells[DataRow,3+i] = TotalHit[i];
				}	
				
				oRng = oSheet.get_Range(StartCol+Convert.ToString(DataRow), EndCol+Convert.ToString(DataRow)); // 합계 범위
				oRng.Font.Bold           = true;							// 폰트 굵게
				oRng.Interior.Color      = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.LightSteelBlue);   //셀 배경색 

				//일자 중앙정렬
				oRng = oSheet.get_Range("A"+Convert.ToString(DataRow), "A"+Convert.ToString(DataRow+DataCount)); // 일자
				oRng.VerticalAlignment   = Excel.XlVAlign.xlVAlignCenter;	// 세로중앙정렬
				oRng.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;	// 가로중앙정렬	 

                DataCount--;

                // 데이터 부분 폰트 수정
                oRng = oSheet.get_Range(StartCol+Convert.ToString(HeaderRow), EndCol+Convert.ToString(DataRow+DataCount));
                oRng.Font.Size = 9;
                oRng.RowHeight = 14;

				// 데이터 작성
				oRng = oSheet.get_Range(StartCol+Convert.ToString(HeaderRow), EndCol+Convert.ToString(DataRow+DataCount));	// 데이터의 범위
				oRng.EntireColumn.AutoFit();					// 데이터의 크기에 셀의 가로크기 맞춤
				oRng.Borders.LineStyle =  Excel.XlLineStyle.xlContinuous;	// 테두리속성 아래에 실선
				oRng.Borders.Weight    = Excel.XlBorderWeight.xlThin;		// 테두리속성 아래에 가는선

				// 숫자형 데이터에 셀타입 설정
				oRng = oSheet.get_Range("B"+Convert.ToString(DataRow), EndCol+Convert.ToString(DataRow+DataCount));	// 데이터의 범위
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

        private void OnSearch(object sender, SearchReportData e)
        {
            StatusMessage("일별 프로그램 시청통계을 조회합니다.");
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
			 
                uiPanelList.Text = "일별 프로그램 시청"; 
			 
                //  전체통계 서비스를 호출한다.
                new DailyProgramHitManager(systemModel,commonModel).GetDailyProgramHitReport(mainModel);
	
                if (mainModel.ResultCD.Equals("0000"))
                {
                    Utility.SetDataTable(dailyProgramHitDs.RowData, mainModel.ReportDataSet);		
                    Utility.SetDataTable(dailyProgramHitDs.Header,  mainModel.HeaderDataSet);		
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

                    SetListHeader();
                    //Row데이터를 리포트용 데이터로 재구성한다.
                    SetListData();
                }
                else
                {
                    rptHeader.u_IsPrint = false;
                }
            }
            catch(FrameException fe)
            {
                rptHeader.u_IsPrint = false;
                FrameSystem.showMsgForm("일별 프로그램 시청통계 조회오류", new string[] {fe.ErrCode, fe.ResultMsg});
            }
            catch(Exception ex)
            {
                rptHeader.u_IsPrint = false;
                FrameSystem.showMsgForm("일별 프로그램 시청통계 조회오류",new string[] {"",ex.Message});
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

    }
}
