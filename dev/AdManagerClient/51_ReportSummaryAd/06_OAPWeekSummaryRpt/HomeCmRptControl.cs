// ===============================================================================
//
// OAPWeekChannelJumpControl.cs
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
    public class HomeCmRptControl : System.Windows.Forms.UserControl, IUserControl
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
		OAPWeekSummaryRptModel oAPWeekSummaryRptModel  = new OAPWeekSummaryRptModel();	// 프로그램별 광고시청집계모델
		SummaryAdModel summaryAdModel  = new SummaryAdModel();	// 프로그램별 광고시청집계모델

		// 화면처리용 변수
		bool canRead			  = false;

		// Key 데이터
		public string keyContractName = "";		
		public string keyItemName     = "";		

		public string StartDay = "";
		public string EndDay   = "";

		string keyStartDay = "";
		string keyEndDay = "";
		string keyReportDay = "";
        private Label lblMediaRep;
        private Janus.Windows.EditControls.UIComboBox cbSearchRep;	

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
		private System.Windows.Forms.Label label1;
		private Janus.Windows.GridEX.EditControls.EditBox editBox1;
		private System.Windows.Forms.Panel panMenuSchedule;
		private System.Windows.Forms.Panel panel2;
		private System.Windows.Forms.Panel panel3;
		private Janus.Windows.EditControls.UIButton btnExcel;
		private System.Data.DataView dvReport;
		private Janus.Windows.UI.Dock.UIPanelManager uiPM;
		private Janus.Windows.CalendarCombo.CalendarCombo cbSearchStartDay;
		private System.Windows.Forms.Label lbSearchDate;
		private System.Windows.Forms.Label label10;
		private Janus.Windows.CalendarCombo.CalendarCombo cbSearchEndDay;
		private System.Windows.Forms.Label label11;
		private AdManagerClient._51_ReportSummaryAd._06_OAPWeekSummaryRpt.OAPWeekSummaryRptDs oapWeekSummaryRptDs;
		private Janus.Windows.GridEX.GridEX gridEX2;
		private System.ComponentModel.IContainer components;

		public HomeCmRptControl()
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
            Janus.Windows.GridEX.GridEXLayout gridEX2_DesignTimeLayout = new Janus.Windows.GridEX.GridEXLayout();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(HomeCmRptControl));
            this.uiPM = new Janus.Windows.UI.Dock.UIPanelManager(this.components);
            this.uiPanelChoiceAdSchedule = new Janus.Windows.UI.Dock.UIPanelGroup();
            this.uiPanelSearch = new Janus.Windows.UI.Dock.UIPanel();
            this.uiPanelSearchContainer = new Janus.Windows.UI.Dock.UIPanelInnerContainer();
            this.pnlSearch = new System.Windows.Forms.Panel();
            this.cbSearchStartDay = new Janus.Windows.CalendarCombo.CalendarCombo();
            this.lbSearchDate = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.cbSearchEndDay = new Janus.Windows.CalendarCombo.CalendarCombo();
            this.label11 = new System.Windows.Forms.Label();
            this.btnSearch = new Janus.Windows.EditControls.UIButton();
            this.btnExcel = new Janus.Windows.EditControls.UIButton();
            this.uiPanelList = new Janus.Windows.UI.Dock.UIPanel();
            this.uiPanelListContainer = new Janus.Windows.UI.Dock.UIPanelInnerContainer();
            this.gridEX2 = new Janus.Windows.GridEX.GridEX();
            this.dvReport = new System.Data.DataView();
            this.oapWeekSummaryRptDs = new AdManagerClient._51_ReportSummaryAd._06_OAPWeekSummaryRpt.OAPWeekSummaryRptDs();
            this.panel3 = new System.Windows.Forms.Panel();
            this.panMenuSchedule = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.editBox1 = new Janus.Windows.GridEX.EditControls.EditBox();
            this.cbSearchRep = new Janus.Windows.EditControls.UIComboBox();
            this.lblMediaRep = new System.Windows.Forms.Label();
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
            ((System.ComponentModel.ISupportInitialize)(this.gridEX2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dvReport)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.oapWeekSummaryRptDs)).BeginInit();
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
            this.uiPM.AddDockPanelInfo(new System.Guid("afb3b7d0-aa79-443c-a1e8-3baba38f7a4d"), new System.Guid("b162c53a-7940-47cd-a0ed-9762c59c50d8"), 40, true);
            this.uiPM.AddDockPanelInfo(new System.Guid("8e3140b9-bfcb-43af-b84e-2ee6cddf51dc"), new System.Guid("b162c53a-7940-47cd-a0ed-9762c59c50d8"), 591, true);
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
            this.uiPanelChoiceAdSchedule.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.uiPanelChoiceAdSchedule.Location = new System.Drawing.Point(0, 0);
            this.uiPanelChoiceAdSchedule.Name = "uiPanelChoiceAdSchedule";
            this.uiPanelChoiceAdSchedule.Size = new System.Drawing.Size(1010, 677);
            this.uiPanelChoiceAdSchedule.TabIndex = 4;
            this.uiPanelChoiceAdSchedule.Text = "홈상업광고 집계";
            // 
            // uiPanelSearch
            // 
            this.uiPanelSearch.AllowResize = Janus.Windows.UI.InheritableBoolean.False;
            this.uiPanelSearch.CaptionVisible = Janus.Windows.UI.InheritableBoolean.False;
            this.uiPanelSearch.CloseButtonVisible = Janus.Windows.UI.InheritableBoolean.False;
            this.uiPanelSearch.InnerContainer = this.uiPanelSearchContainer;
            this.uiPanelSearch.Location = new System.Drawing.Point(0, 22);
            this.uiPanelSearch.Name = "uiPanelSearch";
            this.uiPanelSearch.Size = new System.Drawing.Size(1010, 41);
            this.uiPanelSearch.TabIndex = 4;
            this.uiPanelSearch.Text = "검색";
            // 
            // uiPanelSearchContainer
            // 
            this.uiPanelSearchContainer.Controls.Add(this.pnlSearch);
            this.uiPanelSearchContainer.Location = new System.Drawing.Point(1, 1);
            this.uiPanelSearchContainer.Name = "uiPanelSearchContainer";
            this.uiPanelSearchContainer.Size = new System.Drawing.Size(1008, 39);
            this.uiPanelSearchContainer.TabIndex = 0;
            // 
            // pnlSearch
            // 
            this.pnlSearch.BackColor = System.Drawing.SystemColors.Window;
            this.pnlSearch.Controls.Add(this.lblMediaRep);
            this.pnlSearch.Controls.Add(this.cbSearchRep);
            this.pnlSearch.Controls.Add(this.cbSearchStartDay);
            this.pnlSearch.Controls.Add(this.lbSearchDate);
            this.pnlSearch.Controls.Add(this.label10);
            this.pnlSearch.Controls.Add(this.cbSearchEndDay);
            this.pnlSearch.Controls.Add(this.label11);
            this.pnlSearch.Controls.Add(this.btnSearch);
            this.pnlSearch.Controls.Add(this.btnExcel);
            this.pnlSearch.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlSearch.Location = new System.Drawing.Point(0, 0);
            this.pnlSearch.Name = "pnlSearch";
            this.pnlSearch.Size = new System.Drawing.Size(1008, 39);
            this.pnlSearch.TabIndex = 3;
            // 
            // cbSearchStartDay
            // 
            this.cbSearchStartDay.AutoSize = false;
            // 
            // 
            // 
            this.cbSearchStartDay.DropDownCalendar.FirstMonth = new System.DateTime(2007, 8, 1, 0, 0, 0, 0);
            this.cbSearchStartDay.DropDownCalendar.VisualStyle = Janus.Windows.CalendarCombo.VisualStyle.Office2007;
            this.cbSearchStartDay.Location = new System.Drawing.Point(283, 9);
            this.cbSearchStartDay.Name = "cbSearchStartDay";
            this.cbSearchStartDay.Size = new System.Drawing.Size(104, 23);
            this.cbSearchStartDay.TabIndex = 5;
            this.cbSearchStartDay.Value = new System.DateTime(2007, 9, 14, 0, 0, 0, 0);
            this.cbSearchStartDay.VisualStyle = Janus.Windows.CalendarCombo.VisualStyle.Office2007;
            this.cbSearchStartDay.TextChanged += new System.EventHandler(this.cbSearchStartDay_TextChanged);
            // 
            // lbSearchDate
            // 
            this.lbSearchDate.Location = new System.Drawing.Point(211, 10);
            this.lbSearchDate.Name = "lbSearchDate";
            this.lbSearchDate.Size = new System.Drawing.Size(72, 21);
            this.lbSearchDate.TabIndex = 32;
            this.lbSearchDate.Text = "집계기간";
            this.lbSearchDate.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label10
            // 
            this.label10.Location = new System.Drawing.Point(387, 10);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(32, 21);
            this.label10.TabIndex = 29;
            this.label10.Text = "부터";
            this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // cbSearchEndDay
            // 
            this.cbSearchEndDay.AutoSize = false;
            // 
            // 
            // 
            this.cbSearchEndDay.DropDownCalendar.FirstMonth = new System.DateTime(2007, 8, 1, 0, 0, 0, 0);
            this.cbSearchEndDay.DropDownCalendar.Office2007ColorScheme = Janus.Windows.CalendarCombo.Office2007ColorScheme.Black;
            this.cbSearchEndDay.DropDownCalendar.VisualStyle = Janus.Windows.CalendarCombo.VisualStyle.Office2007;
            this.cbSearchEndDay.Location = new System.Drawing.Point(419, 9);
            this.cbSearchEndDay.Name = "cbSearchEndDay";
            this.cbSearchEndDay.Office2007ColorScheme = Janus.Windows.CalendarCombo.Office2007ColorScheme.Black;
            this.cbSearchEndDay.Size = new System.Drawing.Size(104, 23);
            this.cbSearchEndDay.TabIndex = 6;
            this.cbSearchEndDay.Value = new System.DateTime(2007, 9, 14, 0, 0, 0, 0);
            this.cbSearchEndDay.VisualStyle = Janus.Windows.CalendarCombo.VisualStyle.Office2007;
            this.cbSearchEndDay.TextChanged += new System.EventHandler(this.cbSearchEndDay_TextChanged);
            // 
            // label11
            // 
            this.label11.Location = new System.Drawing.Point(523, 10);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(32, 21);
            this.label11.TabIndex = 26;
            this.label11.Text = "까지";
            this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnSearch
            // 
            this.btnSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSearch.Enabled = false;
            this.btnSearch.Location = new System.Drawing.Point(783, 8);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(104, 24);
            this.btnSearch.TabIndex = 9;
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
            this.btnExcel.TabIndex = 10;
            this.btnExcel.Text = "EXCEL 출력";
            this.btnExcel.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.btnExcel.Click += new System.EventHandler(this.btnExcel_Click);
            // 
            // uiPanelList
            // 
            this.uiPanelList.BackColor = System.Drawing.SystemColors.Window;
            this.uiPanelList.CaptionStyle = Janus.Windows.UI.Dock.PanelCaptionStyle.Dark;
            this.uiPanelList.InnerContainer = this.uiPanelListContainer;
            this.uiPanelList.Location = new System.Drawing.Point(0, 67);
            this.uiPanelList.MinimumSize = new System.Drawing.Size(-1, 100);
            this.uiPanelList.Name = "uiPanelList";
            this.uiPanelList.Size = new System.Drawing.Size(1010, 610);
            this.uiPanelList.TabIndex = 4;
            this.uiPanelList.Text = "통계";
            // 
            // uiPanelListContainer
            // 
            this.uiPanelListContainer.BackColor = System.Drawing.SystemColors.Window;
            this.uiPanelListContainer.Controls.Add(this.gridEX2);
            this.uiPanelListContainer.Location = new System.Drawing.Point(1, 23);
            this.uiPanelListContainer.Name = "uiPanelListContainer";
            this.uiPanelListContainer.Size = new System.Drawing.Size(1008, 586);
            this.uiPanelListContainer.TabIndex = 0;
            // 
            // gridEX2
            // 
            this.gridEX2.AllowEdit = Janus.Windows.GridEX.InheritableBoolean.False;
            this.gridEX2.AlternatingColors = true;
            this.gridEX2.AlternatingRowFormatStyle.BackColor = System.Drawing.Color.WhiteSmoke;
            this.gridEX2.AlternatingRowFormatStyle.BackgroundGradientMode = Janus.Windows.GridEX.BackgroundGradientMode.Solid;
            this.gridEX2.BorderStyle = Janus.Windows.GridEX.BorderStyle.None;
            this.gridEX2.DataSource = this.dvReport;
            gridEX2_DesignTimeLayout.LayoutString = resources.GetString("gridEX2_DesignTimeLayout.LayoutString");
            this.gridEX2.DesignTimeLayout = gridEX2_DesignTimeLayout;
            this.gridEX2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridEX2.EmptyRows = true;
            this.gridEX2.FocusCellFormatStyle.BackColor = System.Drawing.SystemColors.Highlight;
            this.gridEX2.FocusCellFormatStyle.ForeColor = System.Drawing.Color.White;
            this.gridEX2.FocusStyle = Janus.Windows.GridEX.FocusStyle.None;
            this.gridEX2.Font = new System.Drawing.Font("맑은 고딕", 8.25F);
            this.gridEX2.FrozenColumns = 3;
            this.gridEX2.GridLineColor = System.Drawing.Color.Silver;
            this.gridEX2.GridLines = Janus.Windows.GridEX.GridLines.Vertical;
            this.gridEX2.GridLineStyle = Janus.Windows.GridEX.GridLineStyle.Solid;
            this.gridEX2.GroupByBoxVisible = false;
            this.gridEX2.GroupTotals = Janus.Windows.GridEX.GroupTotals.Always;
            this.gridEX2.HeaderFormatStyle.TextAlignment = Janus.Windows.GridEX.TextAlignment.Center;
            this.gridEX2.HideSelection = Janus.Windows.GridEX.HideSelection.Highlight;
            this.gridEX2.Location = new System.Drawing.Point(0, 0);
            this.gridEX2.Name = "gridEX2";
            this.gridEX2.ScrollBars = Janus.Windows.GridEX.ScrollBars.Horizontal;
            this.gridEX2.SelectedInactiveFormatStyle.BackColor = System.Drawing.Color.Gold;
            this.gridEX2.Size = new System.Drawing.Size(1008, 586);
            this.gridEX2.TabIndex = 13;
            this.gridEX2.TabKeyBehavior = Janus.Windows.GridEX.TabKeyBehavior.ControlNavigation;
            this.gridEX2.ThemedAreas = ((Janus.Windows.GridEX.ThemedArea)(((((((((Janus.Windows.GridEX.ThemedArea.ScrollBars | Janus.Windows.GridEX.ThemedArea.EditControls) 
            | Janus.Windows.GridEX.ThemedArea.Headers) 
            | Janus.Windows.GridEX.ThemedArea.GroupByBox) 
            | Janus.Windows.GridEX.ThemedArea.GroupRows) 
            | Janus.Windows.GridEX.ThemedArea.ControlBorder) 
            | Janus.Windows.GridEX.ThemedArea.Cards) 
            | Janus.Windows.GridEX.ThemedArea.Gridlines) 
            | Janus.Windows.GridEX.ThemedArea.CheckBoxes)));
            this.gridEX2.TotalRowFormatStyle.BackColor = System.Drawing.Color.Yellow;
            this.gridEX2.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            // 
            // dvReport
            // 
            this.dvReport.Table = this.oapWeekSummaryRptDs.HomeCmRpt;
            // 
            // oapWeekSummaryRptDs
            // 
            this.oapWeekSummaryRptDs.DataSetName = "OAPWeekSummaryRptDs";
            this.oapWeekSummaryRptDs.Locale = new System.Globalization.CultureInfo("en-US");
            this.oapWeekSummaryRptDs.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
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
            // cbSearchRep
            // 
            this.cbSearchRep.AutoSize = false;
            this.cbSearchRep.ComboStyle = Janus.Windows.EditControls.ComboStyle.DropDownList;
            this.cbSearchRep.DisplayMember = "RapName";
            this.cbSearchRep.Location = new System.Drawing.Point(79, 9);
            this.cbSearchRep.Name = "cbSearchRep";
            this.cbSearchRep.Size = new System.Drawing.Size(136, 23);
            this.cbSearchRep.TabIndex = 33;
            this.cbSearchRep.ValueMember = "RapCode";
            this.cbSearchRep.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            // 
            // lblMediaRep
            // 
            this.lblMediaRep.Location = new System.Drawing.Point(10, 10);
            this.lblMediaRep.Name = "lblMediaRep";
            this.lblMediaRep.Size = new System.Drawing.Size(62, 21);
            this.lblMediaRep.TabIndex = 34;
            this.lblMediaRep.Text = "미디어렙";
            this.lblMediaRep.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // HomeCmRptControl
            // 
            this.Controls.Add(this.uiPanelChoiceAdSchedule);
            this.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.Name = "HomeCmRptControl";
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
            ((System.ComponentModel.ISupportInitialize)(this.gridEX2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dvReport)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.oapWeekSummaryRptDs)).EndInit();
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
            // 공툥모듈에서 콤보박스를 채워온다.
            Common.CommonCode.Init_RepCode(systemModel, commonModel, cbSearchRep);

			InitCombo_Start();
			InitCombo_End();				
		}

		private void InitButton()
		{
			if(canRead)
			{
				btnSearch.Enabled = true;				
			}
			
			if(canPrint)
			{				
				btnExcel.Enabled = true;
			}

			gridEX2.Focus();

			Application.DoEvents();
		}

		private void DisableButton()
		{
			btnSearch.Enabled	= false;			
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

		private void InitCombo_Start()
		{
			// 기간시작일 및 종료일을 금일로 셋트
			cbSearchStartDay.Value = DateTime.Now.AddDays(-1);	
		}

		private void InitCombo_End()
		{
			// 기간시작일 및 종료일을 금일로 셋트
			cbSearchEndDay.Value = DateTime.Now.AddDays(-1);	
		}
		
		#endregion

		#region 처리메소드

		/// <summary>
		/// 카테고리통계 조회
		/// </summary>
		private void SearchReport()
		{
			StatusMessage("카테고리별 조회합니다.");
			ProgressStart();

			try
			{
                string day1 =   cbSearchStartDay.Value.ToString("yyMMdd");
                string day2 =   cbSearchEndDay.Value.ToString("yyMMdd");
                string mediaRep = cbSearchRep.SelectedValue.ToString();
								
				//  전체통계 서비스를 호출한다.
				DataSet ds = new OAPWeekSummaryRptManager(systemModel,commonModel).mngGetHomeCmReport( day1,day2,mediaRep);

				Utility.SetDataTable(oapWeekSummaryRptDs.HomeCmRpt, ds);
				StatusMessage(ds.Tables[0].Rows.Count.ToString() + "건이 조회되었습니다.");		
		
				keyStartDay = cbSearchStartDay.Value.ToString("yyyy-MM-dd");
				keyEndDay   = cbSearchEndDay.Value.ToString("yyyy-MM-dd");

				keyReportDay =  keyStartDay + " ~ " + keyEndDay;
				canPrint = true;


                if (oapWeekSummaryRptDs.HomeCmRpt.Rows.Count > 0)
                {
                    for (int inx =0; inx < oapWeekSummaryRptDs.HomeCmRpt.Rows.Count; inx++)
                    {
                        DataRow Row = oapWeekSummaryRptDs.HomeCmRpt.Rows[inx];

                        if (Row != null && (Row["ItemNo"].ToString().Trim().Equals("0")))
                        {
                            #region 첫번째 데이터를 헤더로 설정하고, 삭제
                            //일단 다 보이게 한다.
                            foreach (Janus.Windows.GridEX.GridEXColumn Col in gridEX2.RootTable.Columns)
                            {
                                if (Col.Caption.Equals("") || Col.Caption.Length == 0)
                                {
                                    Col.Visible = true;
                                }
                            }

                            int MaxColumnCnt = 31;
                            int HeaderCount = 3;

                            for (int i=0; i<MaxColumnCnt; i++)
                            {
                                gridEX2.RootTable.Columns[HeaderCount+i].Caption = "";
                                gridEX2.RootTable.Columns[HeaderCount+i].Caption = Row[HeaderCount+i].ToString();
                            }

                            //데이터를 삭제한다.
                            try
                            {
                                Row.BeginEdit();
                                Row.Delete();
                                Row.EndEdit();
                            }
                            catch (Exception exx)
                            {
                                Console.WriteLine(exx.StackTrace);
                                Console.WriteLine(exx.ToString());
                            }
                            //헤더가 없다면 해당 컬럼을 보이지 않게 바꾼다.
                            foreach (Janus.Windows.GridEX.GridEXColumn Col in gridEX2.RootTable.Columns)
                            {
                                if (Col.Caption.Equals("") || Col.Caption.Length == 0)
                                {
                                    Col.Visible = false;
                                }
                            }
                            break;
                            #endregion
                        }
                    }
                }
			}
			catch(FrameException fe)
			{
				FrameSystem.showMsgForm("홈상업광고 조회오류", new string[] {fe.ErrCode, fe.ResultMsg});
			}
			catch(Exception ex)
			{
				FrameSystem.showMsgForm("홈상업광고 조회오류",new string[] {"",ex.Message});
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
                int ColMax  = 6; // 컬럼수
                int TitleRow        = 1;
                int ConditionRow    = 2;
                int HeaderRow       = 4;
                int DataRow         = 5;
                string StartCol     = "A";
                string EndCol       = "";
                string TitleCol     = "B";
                int DataCount = 0;
                int HeaderCount = 0;

                // 기간조회 임으로 조회한 일자만큼만 데이터가 있다.
				// 과거일자 조회할때, 데이터가 완전히 뽑히지 않아서 else문에서 임시조치함
                for (int inx=0; inx<gridEX2.Tables[0].Columns.Count; inx++)
                {
					if (gridEX2.Tables[0].Columns[inx].Caption == "")
					{
						ColMax = inx;
						break;
					}
					else
					{
						ColMax = inx+1;
					}
                }

                // 마지막 컬럼의 인덱스문자
                EndCol = GetColumnIndex(ColMax);

                xlApp = new Excel.Application();
                oWB = (Excel._Workbook)(xlApp.Workbooks.Add(Missing.Value));
                oSheet = (Excel._Worksheet)oWB.ActiveSheet;
                oSheet.Name = "홈상업광고집계";

                #region [ 타이틀및 조건 부분 ]
                // 타이틀 작성
                oSheet.Cells[TitleRow,1] = "레포트명";
                oRng = oSheet.get_Range("A1","B1");
                oRng.Merge(true);
                oRng.Font.Bold = false;
                oRng.Font.Size = 10;
                oRng.Interior.Color      = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Silver);   //셀 배경색 
                oRng.VerticalAlignment   = Excel.XlVAlign.xlVAlignCenter;
                oRng.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;

                oSheet.Cells[TitleRow,3] = "홈상업광고 집계";
                oRng = oSheet.get_Range("C1", "E1");
                oRng.Merge(true);
                oRng.Font.Bold = true;
                oRng.Font.Size = 11;
                oRng.VerticalAlignment   = Excel.XlVAlign.xlVAlignCenter;
                oRng.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;

                oSheet.Cells[ConditionRow,1] = "기준일자";
                oRng = oSheet.get_Range("A2", "B2");
                oRng.Merge(true);
                oRng.Font.Bold = false;
                oRng.Font.Size = 10;
                oRng.Interior.Color      = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Silver);   //셀 배경색 
                oRng.VerticalAlignment   = Excel.XlVAlign.xlVAlignCenter;
                oRng.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;

                oSheet.Cells[ConditionRow,3] = keyReportDay;
                oRng = oSheet.get_Range("C2", "E2");
                oRng.Merge(true);
                oRng.Font.Bold = true;
                oRng.Font.Size = 10;
                oRng.VerticalAlignment   = Excel.XlVAlign.xlVAlignCenter;
                oRng.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;

                // 조건부 테두리
                oRng = oSheet.get_Range("A1", "E2");
                oRng.VerticalAlignment  = Excel.XlVAlign.xlVAlignCenter;			
                oRng.HorizontalAlignment= Excel.XlHAlign.xlHAlignCenter;		
                oRng.Borders.LineStyle  =  Excel.XlLineStyle.xlContinuous;	// 테두리속성 아래에 실선
                oRng.Borders.Weight     = Excel.XlBorderWeight.xlThin;		// 테두리속성 아래에 가는선
                #endregion
                
                // 헤더 정보 작성
                HeaderCount = 1;
                for( int inx=0;inx<gridEX2.Tables[0].Columns.Count;inx++)
                {
                    if( gridEX2.Tables[0].Columns[inx].Caption == "" )  break;
                    oSheet.Cells[HeaderRow,inx+1] = gridEX2.Tables[0].Columns[inx].Caption;
                }

                oRng = oSheet.get_Range(StartCol+Convert.ToString(HeaderRow), EndCol+Convert.ToString(HeaderRow)); // 헤더의 범위
                oRng.Font.Bold           = true;							// 폰트 굵게
                oRng.VerticalAlignment   = Excel.XlVAlign.xlVAlignCenter;	// 세로중앙정렬
                oRng.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;	// 가로중앙정렬	 
                oRng.Interior.Color      = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.CornflowerBlue);   //셀 배경색 
                oRng.Font.Color          = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.White);	  //텍스트색			
				
                DataCount = 0;
                // 데이터 추출
                for (int inx =0; inx < oapWeekSummaryRptDs.HomeCmRpt.Rows.Count; inx++)
                {

                    DataRow Row = oapWeekSummaryRptDs.HomeCmRpt.Rows[inx];
                    
                    oSheet.Cells[DataRow+DataCount, 1] = Row[0].ToString();
                    oSheet.Cells[DataRow+DataCount, 2] = Row[1].ToString();

                    for (int i=2; i < ColMax; i++)
                    {
                        oSheet.Cells[DataRow+DataCount, i+1] = Row[i].ToString();
                    }
                    DataCount++;
                }

                DataCount--;
                
                // 광고번호는 중앙정렬
                oRng = oSheet.get_Range(GetColumnIndex(1)+Convert.ToString(DataRow), GetColumnIndex(1)+Convert.ToString(DataRow+DataCount));
                oRng.VerticalAlignment   = Excel.XlVAlign.xlVAlignCenter;	// 세로중앙정렬
                oRng.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;	// 가로중앙정렬	 

                // 데이터 작성
                oRng = oSheet.get_Range(StartCol+Convert.ToString(HeaderRow), EndCol+Convert.ToString(DataRow+DataCount));	// 데이터의 범위
                oRng.EntireColumn.AutoFit();					// 데이터의 크기에 셀의 가로크기 맞춤
                oRng.Borders.LineStyle =  Excel.XlLineStyle.xlContinuous;	// 테두리속성 아래에 실선
                oRng.Borders.Weight    = Excel.XlBorderWeight.xlThin;		// 테두리속성 아래에 가는선
                oRng.Font.Bold = false;
                oRng.Font.Size = 9;

                // 일자별 데이터 부분
                oRng = oSheet.get_Range(GetColumnIndex(3)+Convert.ToString(DataRow), GetColumnIndex(ColMax)+Convert.ToString(DataRow+DataCount));	// 데이터의 범위
                oRng.NumberFormatLocal = "#,##0";

                // 합계처리
                // DataCount;
                oSheet.Cells[DataRow+DataCount, 1] = "합계";
                oSheet.Cells[DataRow+DataCount, 2] = "";
                oRng = oSheet.get_Range(GetColumnIndex(1)+Convert.ToString(DataRow+DataCount), GetColumnIndex(2)+Convert.ToString(DataRow+DataCount)); // 일자
                oRng.Merge(true);
                oRng.Font.Bold = true;
                oRng.Font.Size = 10;
                oRng.Interior.Color      = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Silver);   //셀 배경색 
                oRng.VerticalAlignment   = Excel.XlVAlign.xlVAlignCenter;
                oRng.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                				
                xlApp.Visible = true;
                xlApp.UserControl = true;
            }
            catch(Exception ex)
            {
                if (xlApp != null)
                {
                    xlApp.Visible = true;
                    xlApp.UserControl = true;
                }
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


		private void cbSearchStartDay_TextChanged(object sender, System.EventArgs e)
		{
			cbSearchEndDay.Value  = cbSearchStartDay.Value.AddDays(0);
		}

		private void cbSearchEndDay_TextChanged(object sender, System.EventArgs e)
		{
		}
	}
}
