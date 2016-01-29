// ===============================================================================
//
// RptAdCategorySummaryControl.cs
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
    public class RptAdCategorySummaryControl : System.Windows.Forms.UserControl, IUserControl
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
		RptAdCategorySummaryModel rptAdCategorySummaryModel  = new RptAdCategorySummaryModel();	// 프로그램별 광고시청집계모델
		SummaryAdModel summaryAdModel  = new SummaryAdModel();	// 프로그램별 광고시청집계모델

		// 화면처리용 변수
		bool canRead			  = false;

		// Key 데이터
		public string keyContractName = "";
		public string keyItemName     = "";

		public string keyItemNo1       = "";
		public string keyItemNo2       = "";
		public string keyItemNo3       = "";
		public string keyItemNo4       = "";
		public string keyItemNo5       = "";
		public string keyItemNo6       = "";
		public string keyItemNo7       = "";
		public string keyItemNo8       = "";
		public string keyItemNo9       = "";
		public string keyItemNo10       = "";
		public string StartDay = "";
		public string EndDay   = "";

		string  keyStartDay = "";
		string  keyEndDay = "";
		string  keyReportDay = "";
        string  keyAdTypeNm = "";

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
		private Janus.Windows.GridEX.GridEX grdExRepoert;
		private Janus.Windows.UI.Dock.UIPanelManager uiPM;
		private Janus.Windows.CalendarCombo.CalendarCombo cbSearchStartDay;
		private System.Windows.Forms.Label lbSearchDate;
		private System.Windows.Forms.Label label10;
		private Janus.Windows.CalendarCombo.CalendarCombo cbSearchEndDay;
		private System.Windows.Forms.Label label11;
		private Janus.Windows.GridEX.GridEX grdExReport;
		private Janus.Windows.GridEX.GridEX gridEX1;
		private Janus.Windows.EditControls.UIComboBox cbSearchAdType;
		private System.Windows.Forms.Label label2;
		private AdManagerClient._51_ReportSummaryAd._05_RptAdCategorySummary.RptAdCategorySummaryDs rptAdCategorySummaryDs;
		private System.ComponentModel.IContainer components;

		public RptAdCategorySummaryControl()
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
            Janus.Windows.GridEX.GridEXLayout gridEX1_DesignTimeLayout = new Janus.Windows.GridEX.GridEXLayout();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RptAdCategorySummaryControl));
            Janus.Windows.GridEX.GridEXLayout grdExReport_DesignTimeLayout = new Janus.Windows.GridEX.GridEXLayout();
            this.uiPM = new Janus.Windows.UI.Dock.UIPanelManager(this.components);
            this.uiPanelChoiceAdSchedule = new Janus.Windows.UI.Dock.UIPanelGroup();
            this.uiPanelSearch = new Janus.Windows.UI.Dock.UIPanel();
            this.uiPanelSearchContainer = new Janus.Windows.UI.Dock.UIPanelInnerContainer();
            this.pnlSearch = new System.Windows.Forms.Panel();
            this.cbSearchAdType = new Janus.Windows.EditControls.UIComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.cbSearchStartDay = new Janus.Windows.CalendarCombo.CalendarCombo();
            this.lbSearchDate = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.cbSearchEndDay = new Janus.Windows.CalendarCombo.CalendarCombo();
            this.label11 = new System.Windows.Forms.Label();
            this.btnSearch = new Janus.Windows.EditControls.UIButton();
            this.btnExcel = new Janus.Windows.EditControls.UIButton();
            this.uiPanelList = new Janus.Windows.UI.Dock.UIPanel();
            this.uiPanelListContainer = new Janus.Windows.UI.Dock.UIPanelInnerContainer();
            this.gridEX1 = new Janus.Windows.GridEX.GridEX();
            this.dvReport = new System.Data.DataView();
            this.rptAdCategorySummaryDs = new AdManagerClient._51_ReportSummaryAd._05_RptAdCategorySummary.RptAdCategorySummaryDs();
            this.grdExReport = new Janus.Windows.GridEX.GridEX();
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
            ((System.ComponentModel.ISupportInitialize)(this.gridEX1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dvReport)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rptAdCategorySummaryDs)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdExReport)).BeginInit();
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
            this.uiPanelChoiceAdSchedule.Location = new System.Drawing.Point(0, 0);
            this.uiPanelChoiceAdSchedule.Name = "uiPanelChoiceAdSchedule";
            this.uiPanelChoiceAdSchedule.Size = new System.Drawing.Size(1010, 677);
            this.uiPanelChoiceAdSchedule.TabIndex = 4;
            this.uiPanelChoiceAdSchedule.Text = "카테고리별 통계";
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
            this.pnlSearch.Controls.Add(this.cbSearchAdType);
            this.pnlSearch.Controls.Add(this.label2);
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
            // cbSearchAdType
            // 
            this.cbSearchAdType.BackColor = System.Drawing.Color.White;
            this.cbSearchAdType.ComboStyle = Janus.Windows.EditControls.ComboStyle.DropDownList;
            this.cbSearchAdType.Location = new System.Drawing.Point(440, 7);
            this.cbSearchAdType.Name = "cbSearchAdType";
            this.cbSearchAdType.Size = new System.Drawing.Size(120, 23);
            this.cbSearchAdType.TabIndex = 33;
            this.cbSearchAdType.Text = "광고종류";
            this.cbSearchAdType.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(376, 8);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(72, 21);
            this.label2.TabIndex = 34;
            this.label2.Text = "광고종류";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cbSearchStartDay
            // 
            // 
            // 
            // 
            this.cbSearchStartDay.DropDownCalendar.FirstMonth = new System.DateTime(2007, 8, 1, 0, 0, 0, 0);
            this.cbSearchStartDay.DropDownCalendar.Name = "";
            this.cbSearchStartDay.DropDownCalendar.VisualStyle = Janus.Windows.CalendarCombo.VisualStyle.Office2007;
            this.cbSearchStartDay.Location = new System.Drawing.Point(80, 7);
            this.cbSearchStartDay.Name = "cbSearchStartDay";
            this.cbSearchStartDay.Size = new System.Drawing.Size(104, 23);
            this.cbSearchStartDay.TabIndex = 5;
            this.cbSearchStartDay.Value = new System.DateTime(2007, 9, 14, 0, 0, 0, 0);
            this.cbSearchStartDay.VisualStyle = Janus.Windows.CalendarCombo.VisualStyle.Office2007;
            this.cbSearchStartDay.TextChanged += new System.EventHandler(this.cbSearchStartDay_TextChanged);
            // 
            // lbSearchDate
            // 
            this.lbSearchDate.Location = new System.Drawing.Point(8, 8);
            this.lbSearchDate.Name = "lbSearchDate";
            this.lbSearchDate.Size = new System.Drawing.Size(72, 21);
            this.lbSearchDate.TabIndex = 32;
            this.lbSearchDate.Text = "집계기준일";
            this.lbSearchDate.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label10
            // 
            this.label10.Location = new System.Drawing.Point(184, 8);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(32, 21);
            this.label10.TabIndex = 29;
            this.label10.Text = "부터";
            this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cbSearchEndDay
            // 
            // 
            // 
            // 
            this.cbSearchEndDay.DropDownCalendar.FirstMonth = new System.DateTime(2007, 8, 1, 0, 0, 0, 0);
            this.cbSearchEndDay.DropDownCalendar.Name = "";
            this.cbSearchEndDay.DropDownCalendar.VisualStyle = Janus.Windows.CalendarCombo.VisualStyle.Office2007;
            this.cbSearchEndDay.Location = new System.Drawing.Point(216, 7);
            this.cbSearchEndDay.Name = "cbSearchEndDay";
            this.cbSearchEndDay.Size = new System.Drawing.Size(104, 23);
            this.cbSearchEndDay.TabIndex = 6;
            this.cbSearchEndDay.Value = new System.DateTime(2007, 9, 14, 0, 0, 0, 0);
            this.cbSearchEndDay.VisualStyle = Janus.Windows.CalendarCombo.VisualStyle.Office2007;
            this.cbSearchEndDay.TextChanged += new System.EventHandler(this.cbSearchEndDay_TextChanged);
            // 
            // label11
            // 
            this.label11.Location = new System.Drawing.Point(320, 8);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(32, 21);
            this.label11.TabIndex = 26;
            this.label11.Text = "까지";
            this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
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
            this.uiPanelListContainer.Controls.Add(this.gridEX1);
            this.uiPanelListContainer.Location = new System.Drawing.Point(1, 23);
            this.uiPanelListContainer.Name = "uiPanelListContainer";
            this.uiPanelListContainer.Size = new System.Drawing.Size(1008, 586);
            this.uiPanelListContainer.TabIndex = 0;
            // 
            // gridEX1
            // 
            this.gridEX1.AllowEdit = Janus.Windows.GridEX.InheritableBoolean.False;
            this.gridEX1.AlternatingColors = true;
            this.gridEX1.BorderStyle = Janus.Windows.GridEX.BorderStyle.None;
            this.gridEX1.ColumnHeaders = Janus.Windows.GridEX.InheritableBoolean.False;
            this.gridEX1.DataSource = this.dvReport;
            gridEX1_DesignTimeLayout.LayoutString = resources.GetString("gridEX1_DesignTimeLayout.LayoutString");
            this.gridEX1.DesignTimeLayout = gridEX1_DesignTimeLayout;
            this.gridEX1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridEX1.EditorsControlStyle.ButtonAppearance = Janus.Windows.GridEX.ButtonAppearance.Regular;
            this.gridEX1.EmptyRows = true;
            this.gridEX1.FocusCellFormatStyle.BackColor = System.Drawing.SystemColors.Highlight;
            this.gridEX1.FocusCellFormatStyle.ForeColor = System.Drawing.Color.White;
            this.gridEX1.FocusStyle = Janus.Windows.GridEX.FocusStyle.None;
            this.gridEX1.Font = new System.Drawing.Font("맑은 고딕", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.gridEX1.FrozenColumns = 2;
            this.gridEX1.GridLineColor = System.Drawing.Color.Silver;
            this.gridEX1.GridLines = Janus.Windows.GridEX.GridLines.Vertical;
            this.gridEX1.GridLineStyle = Janus.Windows.GridEX.GridLineStyle.Solid;
            this.gridEX1.GroupByBoxVisible = false;
            this.gridEX1.GroupTotals = Janus.Windows.GridEX.GroupTotals.Always;
            this.gridEX1.HeaderFormatStyle.TextAlignment = Janus.Windows.GridEX.TextAlignment.Center;
            this.gridEX1.HideSelection = Janus.Windows.GridEX.HideSelection.Highlight;
            this.gridEX1.Location = new System.Drawing.Point(0, 0);
            this.gridEX1.Name = "gridEX1";
            this.gridEX1.SelectedInactiveFormatStyle.BackColor = System.Drawing.Color.Gold;
            this.gridEX1.Size = new System.Drawing.Size(1008, 586);
            this.gridEX1.TabIndex = 13;
            this.gridEX1.TabKeyBehavior = Janus.Windows.GridEX.TabKeyBehavior.ControlNavigation;
            this.gridEX1.ThemedAreas = ((Janus.Windows.GridEX.ThemedArea)(((((((((Janus.Windows.GridEX.ThemedArea.ScrollBars | Janus.Windows.GridEX.ThemedArea.EditControls) 
            | Janus.Windows.GridEX.ThemedArea.Headers) 
            | Janus.Windows.GridEX.ThemedArea.GroupByBox) 
            | Janus.Windows.GridEX.ThemedArea.GroupRows) 
            | Janus.Windows.GridEX.ThemedArea.ControlBorder) 
            | Janus.Windows.GridEX.ThemedArea.Cards) 
            | Janus.Windows.GridEX.ThemedArea.Gridlines) 
            | Janus.Windows.GridEX.ThemedArea.CheckBoxes)));
            this.gridEX1.TotalRowFormatStyle.BackColor = System.Drawing.Color.Yellow;
            this.gridEX1.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            this.gridEX1.LoadingRow += new Janus.Windows.GridEX.RowLoadEventHandler(this.gridEX1_LoadingRow);
            // 
            // dvReport
            // 
            this.dvReport.Table = this.rptAdCategorySummaryDs.RptAdCategorySummary;
            // 
            // rptAdCategorySummaryDs
            // 
            this.rptAdCategorySummaryDs.DataSetName = "RptAdCategorySummaryDs";
            this.rptAdCategorySummaryDs.Locale = new System.Globalization.CultureInfo("en-US");
            this.rptAdCategorySummaryDs.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
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
            this.grdExReport.Font = new System.Drawing.Font("굴림체", 9F);
            this.grdExReport.GridLineColor = System.Drawing.Color.Silver;
            this.grdExReport.GridLines = Janus.Windows.GridEX.GridLines.Vertical;
            this.grdExReport.GridLineStyle = Janus.Windows.GridEX.GridLineStyle.Solid;
            this.grdExReport.GroupByBoxVisible = false;
            this.grdExReport.HideSelection = Janus.Windows.GridEX.HideSelection.Highlight;
            this.grdExReport.Location = new System.Drawing.Point(0, 0);
            this.grdExReport.Name = "grdExReport";
            this.grdExReport.SelectedInactiveFormatStyle.BackColor = System.Drawing.Color.Gold;
            this.grdExReport.Size = new System.Drawing.Size(849, 537);
            this.grdExReport.TabIndex = 12;
            this.grdExReport.TabKeyBehavior = Janus.Windows.GridEX.TabKeyBehavior.ControlNavigation;
            this.grdExReport.TotalRowFormatStyle.BackColor = System.Drawing.Color.Yellow;
            this.grdExReport.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2003;
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
            // RptAdCategorySummaryControl
            // 
            this.Controls.Add(this.uiPanelChoiceAdSchedule);
            this.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.Name = "RptAdCategorySummaryControl";
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
            ((System.ComponentModel.ISupportInitialize)(this.gridEX1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dvReport)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rptAdCategorySummaryDs)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdExReport)).EndInit();
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
			InitCombo_Start();
			InitCombo_End();	
			Init_AdType();
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

			grdExReport.Focus();

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

		private void Init_AdType()
		{
            Common.CommonCode.Init_AdType(systemModel, commonModel, cbSearchAdType);
            //// 코드에서 내역상태를 조회한다.
            //CodeModel codeModel = new CodeModel();
            //codeModel.Section = "26";				// '26':광고종류  TODO: 코드분류는 추후 XML로 관리되어야...
            //new CodeManager(systemModel, commonModel).GetCodeList(codeModel);
			
            //if (codeModel.ResultCD.Equals("0000"))
            //{
            //    // 데이터셋에 셋팅
            //    Utility.SetDataTable(rptAdCategorySummaryDs.AdType, codeModel.CodeDataSet);				
            //}
 
            //// 검색조건의 콤보
            //this.cbSearchAdType.Items.Clear();
			
            //// 콤보박스에 셋트할 코드목록을 담을 Item배열을 선언
            //Janus.Windows.EditControls.UIComboBoxItem[] comboItems = new Janus.Windows.EditControls.UIComboBoxItem[codeModel.ResultCnt + 1];

            //comboItems[0] = new Janus.Windows.EditControls.UIComboBoxItem("[광고종류]","00");
			
            //for(int i=0;i<codeModel.ResultCnt;i++)
            //{
            //    DataRow row = rptAdCategorySummaryDs.AdType.Rows[i];

            //    string val = row["Code"].ToString();
            //    string txt = row["CodeName"].ToString();
            //    comboItems[i+1] = new Janus.Windows.EditControls.UIComboBoxItem(txt,val);
            //}
            //// 콤보에 셋트
            //this.cbSearchAdType.Items.AddRange(comboItems);
            //this.cbSearchAdType.SelectedIndex = 0;

			Application.DoEvents();
		}

		#endregion

		#region 처리메소드

		/// <summary>
		/// 카테고리통계 조회
		/// </summary>
		private void SearchReport()
		{
			StatusMessage("카테고리별 조회합니다.");
			
			if(cbSearchAdType.SelectedValue.Equals("00"))
			{
			    MessageBox.Show("광고종류를 선택해 주십시요.", "카테고리통계",MessageBoxButtons.OK, MessageBoxIcon.Information);
			    return;
			}

			ProgressStart();
			try
			{
				// 데이터모델 초기화
				rptAdCategorySummaryModel.Init();
				rptAdCategorySummaryModel.LogDay  	=   cbSearchStartDay.Value.ToString("yyMMdd");
				rptAdCategorySummaryModel.LogDayEnd =   cbSearchEndDay.Value.ToString("yyMMdd");
				rptAdCategorySummaryModel.AdType    =   cbSearchAdType.SelectedValue.ToString();
				
				uiPanelList.Text = "카테고리별 광고집행 실적"; 

				//  전체통계 서비스를 호출한다.
				new RptAdCategorySummaryManager(systemModel,commonModel).GetRptAdCategorySummary(rptAdCategorySummaryModel);

				if (rptAdCategorySummaryModel.ResultCD.Equals("0000"))
				{
					Utility.SetDataTable(rptAdCategorySummaryDs.RptAdCategorySummary, rptAdCategorySummaryModel.ReportDataSet);													
					StatusMessage(rptAdCategorySummaryModel.ResultCnt + "건이 조회되었습니다.");		
		
					keyStartDay =   cbSearchStartDay.Value.ToString("yyyy-MM-dd");
					keyEndDay   =   cbSearchEndDay.Value.ToString("yyyy-MM-dd");
                    keyAdTypeNm =   "[" + cbSearchAdType.SelectedValue.ToString() + "]" + cbSearchAdType.SelectedItem.Text.ToString();

					keyReportDay =  keyStartDay + " ~ " + keyEndDay;
					
					uiPanelList.Text += " [ " + keyReportDay + " ]";
					canPrint = true;
                    DataSetChange();


				}
			}
			catch(FrameException fe)
			{
				FrameSystem.showMsgForm("카테고리통계 조회오류", new string[] {fe.ErrCode, fe.ResultMsg});
			}
			catch(Exception ex)
			{
				FrameSystem.showMsgForm("카테고리통계 조회오류",new string[] {"",ex.Message});
			}
			finally
			{
				ProgressStop();
			}
		}

        private void DataSetChange()
        {
            if( rptAdCategorySummaryDs.RptAdCategorySummary.Rows.Count > 0 )
            {
                for (int inx =0; inx < rptAdCategorySummaryDs.RptAdCategorySummary.Rows.Count; inx++)
                {		
                    DataRow Row = rptAdCategorySummaryDs.RptAdCategorySummary.Rows[inx];
				
                    if(Row["ItemNo"].ToString().Trim().Equals("광고번호"))
                    {				
                        int HeaderCount = 0;
                        gridEX1.RootTable.Columns[HeaderCount].Caption = Row[HeaderCount++].ToString();
                        gridEX1.RootTable.Columns[HeaderCount].Caption = Row[HeaderCount++].ToString();
                        gridEX1.RootTable.Columns[HeaderCount].Caption = Row[HeaderCount++].ToString();
                        gridEX1.RootTable.Columns[HeaderCount].Caption = Row[HeaderCount++].ToString();
                        gridEX1.RootTable.Columns[HeaderCount].Caption = Row[HeaderCount++].ToString();
                        gridEX1.RootTable.Columns[HeaderCount].Caption = Row[HeaderCount++].ToString();
                        gridEX1.RootTable.Columns[HeaderCount].Caption = Row[HeaderCount++].ToString();
                        gridEX1.RootTable.Columns[HeaderCount].Caption = Row[HeaderCount++].ToString();
                        gridEX1.RootTable.Columns[HeaderCount].Caption = Row[HeaderCount++].ToString();
                        gridEX1.RootTable.Columns[HeaderCount].Caption = Row[HeaderCount++].ToString();
                        gridEX1.RootTable.Columns[HeaderCount].Caption = Row[HeaderCount++].ToString();
                        gridEX1.RootTable.Columns[HeaderCount].Caption = Row[HeaderCount++].ToString();
                        gridEX1.RootTable.Columns[HeaderCount].Caption = Row[HeaderCount++].ToString();
                        gridEX1.RootTable.Columns[HeaderCount].Caption = Row[HeaderCount++].ToString();
                        gridEX1.RootTable.Columns[HeaderCount].Caption = Row[HeaderCount++].ToString();
                        gridEX1.RootTable.Columns[HeaderCount].Caption = Row[HeaderCount++].ToString();
                        gridEX1.RootTable.Columns[HeaderCount].Caption = Row[HeaderCount++].ToString();
                        gridEX1.RootTable.Columns[HeaderCount].Caption = Row[HeaderCount++].ToString();
                        gridEX1.RootTable.Columns[HeaderCount].Caption = Row[HeaderCount++].ToString();
                        gridEX1.RootTable.Columns[HeaderCount].Caption = Row[HeaderCount++].ToString();
                        gridEX1.RootTable.Columns[HeaderCount].Caption = Row[HeaderCount++].ToString();
                        gridEX1.RootTable.Columns[HeaderCount].Caption = Row[HeaderCount++].ToString();
                        gridEX1.RootTable.Columns[HeaderCount].Caption = Row[HeaderCount++].ToString();
                        gridEX1.RootTable.Columns[HeaderCount].Caption = Row[HeaderCount++].ToString();
                        gridEX1.RootTable.Columns[HeaderCount].Caption = Row[HeaderCount++].ToString();
                        gridEX1.RootTable.Columns[HeaderCount].Caption = Row[HeaderCount++].ToString();
                        gridEX1.RootTable.Columns[HeaderCount].Caption = Row[HeaderCount++].ToString();
                        gridEX1.RootTable.Columns[HeaderCount].Caption = Row[HeaderCount++].ToString();
                        gridEX1.RootTable.Columns[HeaderCount].Caption = Row[HeaderCount++].ToString();
                        gridEX1.RootTable.Columns[HeaderCount].Caption = Row[HeaderCount++].ToString();
                        gridEX1.RootTable.Columns[HeaderCount].Caption = Row[HeaderCount++].ToString();
                        gridEX1.RootTable.Columns[HeaderCount].Caption = Row[HeaderCount++].ToString();
                        gridEX1.RootTable.Columns[HeaderCount].Caption = Row[HeaderCount++].ToString();
                        gridEX1.RootTable.Columns[HeaderCount].Caption = Row[HeaderCount++].ToString();
                        gridEX1.RootTable.Columns[HeaderCount].Caption = Row[HeaderCount++].ToString();

                        Row.BeginEdit();
                        Row.Delete();
                        Row.EndEdit();

                        foreach( Janus.Windows.GridEX.GridEXColumn Col in gridEX1.RootTable.Columns )
                        {
                            if( Col.Caption.Equals("") || Col.Caption.Length == 0 )
                            {
                                Col.Visible = false;
                            }
                        }
                    }
                }
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
				int ColMax  = 35; // 컬럼수   				

				int TitleRow  = 1;
				int ConditionRow = 2;
				int HeaderRow = 5;
				int DataRow   = 6;
				string StartCol = "A";
				string EndCol   = "";
				string TitleCol = "C";
				int DataCount = 0;
				int CondCount = 0;
				int HeaderCount = 0;
				
				// 마지막 컬럼의 인덱스문자
				EndCol = GetColumnIndex(ColMax);

				xlApp = new Excel.Application();
				oWB = (Excel._Workbook)(xlApp.Workbooks.Add(Missing.Value));
				oSheet = (Excel._Worksheet)oWB.ActiveSheet;


				// 타이틀 작성
				oSheet.Cells[TitleRow,1] = "카테고리별 광고집행 실적";
				oRng = oSheet.get_Range(StartCol+Convert.ToString(TitleRow), TitleCol+Convert.ToString(TitleRow));
				oRng.Merge(true);
				oRng.Font.Bold = true;
				oRng.Font.Size = 16;
				oRng.VerticalAlignment   = Excel.XlVAlign.xlVAlignCenter;			
				oRng.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;		

				// 조건정보 작성
				oSheet.Cells[ConditionRow+CondCount,1] = "기준일";
				oRng = oSheet.get_Range("B"+Convert.ToString(ConditionRow+CondCount), TitleCol+Convert.ToString(ConditionRow+CondCount));
				oRng.Merge(true);
				oSheet.Cells[ConditionRow+CondCount,2] = keyReportDay;
				CondCount++;

                oSheet.Cells[ConditionRow+CondCount,1] = "광고종류";
                oRng = oSheet.get_Range("B"+Convert.ToString(ConditionRow+CondCount), TitleCol+Convert.ToString(ConditionRow+CondCount));
                oRng.Merge(true);
                oSheet.Cells[ConditionRow+CondCount,2] = keyAdTypeNm;
                CondCount++;

				// 조건부 테두리
				oRng = oSheet.get_Range(StartCol+Convert.ToString(ConditionRow), TitleCol+Convert.ToString(ConditionRow+(CondCount-1)));
                oRng.Font.Bold = true;
                oRng.Font.Size = 10;
				oRng.VerticalAlignment   = Excel.XlVAlign.xlVAlignCenter;			
				oRng.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;		
				oRng.Borders.LineStyle =  Excel.XlLineStyle.xlContinuous;	// 테두리속성 아래에 실선
				oRng.Borders.Weight    = Excel.XlBorderWeight.xlThin;		// 테두리속성 아래에 가는선


				// 헤더 정보 작성
//				oSheet.Cells[HeaderRow,1] = uiPanelList.Text;
//				oRng = oSheet.get_Range(GetColumnIndex(1)+Convert.ToString(HeaderRow), GetColumnIndex(35)+Convert.ToString(HeaderRow));
//				oRng.Merge(true);
				// 데이터 추출
				HeaderCount = 1;						
				oSheet.Cells[HeaderRow,HeaderCount++] = gridEX1.RootTable.Columns["ItemNo"].Caption.ToString();
				oSheet.Cells[HeaderRow,HeaderCount++] = gridEX1.RootTable.Columns["ItemNm"].Caption.ToString();
				oSheet.Cells[HeaderRow,HeaderCount++] = gridEX1.RootTable.Columns["ContSeq"].Caption.ToString();
				oSheet.Cells[HeaderRow,HeaderCount++] = gridEX1.RootTable.Columns["Tot"].Caption.ToString();
				oSheet.Cells[HeaderRow,HeaderCount++] = gridEX1.RootTable.Columns["C99"].Caption.ToString();
				oSheet.Cells[HeaderRow,HeaderCount++] = gridEX1.RootTable.Columns["C1"].Caption.ToString();
				oSheet.Cells[HeaderRow,HeaderCount++] = gridEX1.RootTable.Columns["C2"].Caption.ToString();
				oSheet.Cells[HeaderRow,HeaderCount++] = gridEX1.RootTable.Columns["C3"].Caption.ToString();
				oSheet.Cells[HeaderRow,HeaderCount++] = gridEX1.RootTable.Columns["C4"].Caption.ToString();
				oSheet.Cells[HeaderRow,HeaderCount++] = gridEX1.RootTable.Columns["C5"].Caption.ToString();
				oSheet.Cells[HeaderRow,HeaderCount++] = gridEX1.RootTable.Columns["C6"].Caption.ToString();
				oSheet.Cells[HeaderRow,HeaderCount++] = gridEX1.RootTable.Columns["C7"].Caption.ToString();
				oSheet.Cells[HeaderRow,HeaderCount++] = gridEX1.RootTable.Columns["C8"].Caption.ToString();
				oSheet.Cells[HeaderRow,HeaderCount++] = gridEX1.RootTable.Columns["C9"].Caption.ToString();
				oSheet.Cells[HeaderRow,HeaderCount++] = gridEX1.RootTable.Columns["C10"].Caption.ToString();
				oSheet.Cells[HeaderRow,HeaderCount++] = gridEX1.RootTable.Columns["C11"].Caption.ToString();
				oSheet.Cells[HeaderRow,HeaderCount++] = gridEX1.RootTable.Columns["C12"].Caption.ToString();
				oSheet.Cells[HeaderRow,HeaderCount++] = gridEX1.RootTable.Columns["C13"].Caption.ToString();
				oSheet.Cells[HeaderRow,HeaderCount++] = gridEX1.RootTable.Columns["C14"].Caption.ToString();
				oSheet.Cells[HeaderRow,HeaderCount++] = gridEX1.RootTable.Columns["C15"].Caption.ToString();
				oSheet.Cells[HeaderRow,HeaderCount++] = gridEX1.RootTable.Columns["C16"].Caption.ToString();
				oSheet.Cells[HeaderRow,HeaderCount++] = gridEX1.RootTable.Columns["C17"].Caption.ToString();
				oSheet.Cells[HeaderRow,HeaderCount++] = gridEX1.RootTable.Columns["C18"].Caption.ToString();
				oSheet.Cells[HeaderRow,HeaderCount++] = gridEX1.RootTable.Columns["C19"].Caption.ToString();
				oSheet.Cells[HeaderRow,HeaderCount++] = gridEX1.RootTable.Columns["C20"].Caption.ToString();
				oSheet.Cells[HeaderRow,HeaderCount++] = gridEX1.RootTable.Columns["C21"].Caption.ToString();
				oSheet.Cells[HeaderRow,HeaderCount++] = gridEX1.RootTable.Columns["C22"].Caption.ToString();
				oSheet.Cells[HeaderRow,HeaderCount++] = gridEX1.RootTable.Columns["C23"].Caption.ToString();
				oSheet.Cells[HeaderRow,HeaderCount++] = gridEX1.RootTable.Columns["C24"].Caption.ToString();
				oSheet.Cells[HeaderRow,HeaderCount++] = gridEX1.RootTable.Columns["C25"].Caption.ToString();
				oSheet.Cells[HeaderRow,HeaderCount++] = gridEX1.RootTable.Columns["C26"].Caption.ToString();
				oSheet.Cells[HeaderRow,HeaderCount++] = gridEX1.RootTable.Columns["C27"].Caption.ToString();
				oSheet.Cells[HeaderRow,HeaderCount++] = gridEX1.RootTable.Columns["C28"].Caption.ToString();
				oSheet.Cells[HeaderRow,HeaderCount++] = gridEX1.RootTable.Columns["C29"].Caption.ToString();
				oSheet.Cells[HeaderRow,HeaderCount++] = gridEX1.RootTable.Columns["C30"].Caption.ToString();						
				
				oRng = oSheet.get_Range(StartCol+Convert.ToString(HeaderRow), EndCol+Convert.ToString(HeaderRow));	// 헤더의 범위
				oRng.Font.Bold           = true;							// 폰트 굵게
                oRng.Font.Size          = 10;
				oRng.VerticalAlignment   = Excel.XlVAlign.xlVAlignCenter;	// 세로중앙정렬
				oRng.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;	// 가로중앙정렬	 
				oRng.Interior.Color      = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.CornflowerBlue);   //셀 배경색 
				oRng.Font.Color          = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.White);			//텍스트색			
				
				DataCount = 0;
				// 데이터 추출
				for (int inx =0; inx < rptAdCategorySummaryDs.RptAdCategorySummary.Rows.Count; inx++)
				{		
					DataRow Row = rptAdCategorySummaryDs.RptAdCategorySummary.Rows[inx];

					int ColCnt = 1;
					if(!Row["ItemNo"].ToString().Trim().Equals("광고번호"))
					{							
						oSheet.Cells[DataRow+DataCount,ColCnt++] = Row["ItemNo"].ToString();
						oSheet.Cells[DataRow+DataCount,ColCnt++] = Row["ItemNm"].ToString();
						oSheet.Cells[DataRow+DataCount,ColCnt++] = Row["ContSeq"].ToString();
						oSheet.Cells[DataRow+DataCount,ColCnt++] = Row["Tot"].ToString();
						oSheet.Cells[DataRow+DataCount,ColCnt++] = Row["C99"].ToString();
						oSheet.Cells[DataRow+DataCount,ColCnt++] = Row["C1"].ToString();
						oSheet.Cells[DataRow+DataCount,ColCnt++] = Row["C2"].ToString();
						oSheet.Cells[DataRow+DataCount,ColCnt++] = Row["C3"].ToString();
						oSheet.Cells[DataRow+DataCount,ColCnt++] = Row["C4"].ToString();
						oSheet.Cells[DataRow+DataCount,ColCnt++] = Row["C5"].ToString();
						oSheet.Cells[DataRow+DataCount,ColCnt++] = Row["C6"].ToString();
						oSheet.Cells[DataRow+DataCount,ColCnt++] = Row["C7"].ToString();
						oSheet.Cells[DataRow+DataCount,ColCnt++] = Row["C8"].ToString();
						oSheet.Cells[DataRow+DataCount,ColCnt++] = Row["C9"].ToString();
						oSheet.Cells[DataRow+DataCount,ColCnt++] = Row["C10"].ToString();
						oSheet.Cells[DataRow+DataCount,ColCnt++] = Row["C11"].ToString();
						oSheet.Cells[DataRow+DataCount,ColCnt++] = Row["C12"].ToString();
						oSheet.Cells[DataRow+DataCount,ColCnt++] = Row["C13"].ToString();
						oSheet.Cells[DataRow+DataCount,ColCnt++] = Row["C14"].ToString();
						oSheet.Cells[DataRow+DataCount,ColCnt++] = Row["C15"].ToString();
						oSheet.Cells[DataRow+DataCount,ColCnt++] = Row["C16"].ToString();
						oSheet.Cells[DataRow+DataCount,ColCnt++] = Row["C17"].ToString();
						oSheet.Cells[DataRow+DataCount,ColCnt++] = Row["C18"].ToString();
						oSheet.Cells[DataRow+DataCount,ColCnt++] = Row["C19"].ToString();
						oSheet.Cells[DataRow+DataCount,ColCnt++] = Row["C20"].ToString();
						oSheet.Cells[DataRow+DataCount,ColCnt++] = Row["C21"].ToString();
						oSheet.Cells[DataRow+DataCount,ColCnt++] = Row["C22"].ToString();
						oSheet.Cells[DataRow+DataCount,ColCnt++] = Row["C23"].ToString();
						oSheet.Cells[DataRow+DataCount,ColCnt++] = Row["C24"].ToString();
						oSheet.Cells[DataRow+DataCount,ColCnt++] = Row["C25"].ToString();
						oSheet.Cells[DataRow+DataCount,ColCnt++] = Row["C26"].ToString();
						oSheet.Cells[DataRow+DataCount,ColCnt++] = Row["C27"].ToString();
						oSheet.Cells[DataRow+DataCount,ColCnt++] = Row["C28"].ToString();
						oSheet.Cells[DataRow+DataCount,ColCnt++] = Row["C29"].ToString();
						oSheet.Cells[DataRow+DataCount,ColCnt++] = Row["C30"].ToString();	
					}

                    if( Row["ContSeq"].ToString().Equals("0") )
                    {
                        oRng = oSheet.get_Range(StartCol+Convert.ToString(DataRow+DataCount), EndCol+Convert.ToString(DataRow+DataCount));	// 헤더의 범위
                        oRng.Font.Bold           = true;							// 폰트 굵게
                        oRng.Font.Size          = 9;
                        oRng.Interior.Color      = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.LightGray);   //셀 배경색 
                        oRng.Font.Color          = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Black);			//텍스트색			
                    }
					DataCount++;
				}
				DataCount--;

				// 데이터 작성
				oRng = oSheet.get_Range(StartCol+Convert.ToString(HeaderRow), EndCol+Convert.ToString(DataRow+DataCount));	// 데이터의 범위
                oRng.Font.Size = 8;
				oRng.EntireColumn.AutoFit();					// 데이터의 크기에 셀의 가로크기 맞춤
				oRng.Borders.LineStyle =  Excel.XlLineStyle.xlContinuous;	// 테두리속성 아래에 실선
				oRng.Borders.Weight    = Excel.XlBorderWeight.xlThin;		// 테두리속성 아래에 가는선
				//광고번호 중앙정렬
				oRng = oSheet.get_Range(GetColumnIndex(1)+Convert.ToString(DataRow), GetColumnIndex(1)+Convert.ToString(DataRow+DataCount)); 
				oRng.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
				
				//광고번호 중앙정렬
				oRng = oSheet.get_Range(GetColumnIndex(2)+Convert.ToString(DataRow), GetColumnIndex(2)+Convert.ToString(DataRow+DataCount)); 
				oRng.HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft;
				
								
				//시청횟수 right정렬
				oRng = oSheet.get_Range(GetColumnIndex(3)+Convert.ToString(DataRow), GetColumnIndex(3)+Convert.ToString(DataRow+DataCount)); 
				oRng.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
				
				//시청가구 right정렬
				oRng = oSheet.get_Range(GetColumnIndex(4)+Convert.ToString(DataRow), GetColumnIndex(4)+Convert.ToString(DataRow+DataCount)); 
				oRng.HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;
				oRng.NumberFormatLocal = "#,##0";

				//rate right정렬
				oRng = oSheet.get_Range(GetColumnIndex(5)+Convert.ToString(DataRow), GetColumnIndex(5)+Convert.ToString(DataRow+DataCount)); 
				oRng.HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;
				oRng.NumberFormatLocal = "#,##0";

				//Cove right정렬
				oRng = oSheet.get_Range(GetColumnIndex(6)+Convert.ToString(DataRow), GetColumnIndex(6)+Convert.ToString(DataRow+DataCount)); 
				oRng.HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;
				oRng.NumberFormatLocal = "#,##0";

				//Freq right정렬
				oRng = oSheet.get_Range(GetColumnIndex(7)+Convert.ToString(DataRow), GetColumnIndex(7)+Convert.ToString(DataRow+DataCount)); 
				oRng.HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;
				oRng.NumberFormatLocal = "#,##0";

				oRng = oSheet.get_Range(GetColumnIndex(8)+Convert.ToString(DataRow), GetColumnIndex(8)+Convert.ToString(DataRow+DataCount)); 
				oRng.HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;
				oRng.NumberFormatLocal = "#,##0";

				oRng = oSheet.get_Range(GetColumnIndex(9)+Convert.ToString(DataRow), GetColumnIndex(9)+Convert.ToString(DataRow+DataCount)); 
				oRng.HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;
				oRng.NumberFormatLocal = "#,##0";

				oRng = oSheet.get_Range(GetColumnIndex(10)+Convert.ToString(DataRow), GetColumnIndex(10)+Convert.ToString(DataRow+DataCount)); 
				oRng.HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;
				oRng.NumberFormatLocal = "#,##0";

				oRng = oSheet.get_Range(GetColumnIndex(11)+Convert.ToString(DataRow), GetColumnIndex(11)+Convert.ToString(DataRow+DataCount)); 
				oRng.HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;
				oRng.NumberFormatLocal = "#,##0";

				oRng = oSheet.get_Range(GetColumnIndex(12)+Convert.ToString(DataRow), GetColumnIndex(12)+Convert.ToString(DataRow+DataCount)); 
				oRng.HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;
				oRng.NumberFormatLocal = "#,##0";

				oRng = oSheet.get_Range(GetColumnIndex(13)+Convert.ToString(DataRow), GetColumnIndex(13)+Convert.ToString(DataRow+DataCount)); 
				oRng.HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;
				oRng.NumberFormatLocal = "#,##0";

				oRng = oSheet.get_Range(GetColumnIndex(14)+Convert.ToString(DataRow), GetColumnIndex(14)+Convert.ToString(DataRow+DataCount)); 
				oRng.HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;
				oRng.NumberFormatLocal = "#,##0";

				oRng = oSheet.get_Range(GetColumnIndex(15)+Convert.ToString(DataRow), GetColumnIndex(15)+Convert.ToString(DataRow+DataCount)); 
				oRng.HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;
				oRng.NumberFormatLocal = "#,##0";

				oRng = oSheet.get_Range(GetColumnIndex(16)+Convert.ToString(DataRow), GetColumnIndex(16)+Convert.ToString(DataRow+DataCount)); 
				oRng.HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;
				oRng.NumberFormatLocal = "#,##0";

				oRng = oSheet.get_Range(GetColumnIndex(17)+Convert.ToString(DataRow), GetColumnIndex(17)+Convert.ToString(DataRow+DataCount)); 
				oRng.HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;
				oRng.NumberFormatLocal = "#,##0";

				oRng = oSheet.get_Range(GetColumnIndex(18)+Convert.ToString(DataRow), GetColumnIndex(18)+Convert.ToString(DataRow+DataCount)); 
				oRng.HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;
				oRng.NumberFormatLocal = "#,##0";

				oRng = oSheet.get_Range(GetColumnIndex(19)+Convert.ToString(DataRow), GetColumnIndex(19)+Convert.ToString(DataRow+DataCount)); 
				oRng.HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;
				oRng.NumberFormatLocal = "#,##0";

				oRng = oSheet.get_Range(GetColumnIndex(20)+Convert.ToString(DataRow), GetColumnIndex(20)+Convert.ToString(DataRow+DataCount)); 
				oRng.HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;
				oRng.NumberFormatLocal = "#,##0";

				oRng = oSheet.get_Range(GetColumnIndex(21)+Convert.ToString(DataRow), GetColumnIndex(21)+Convert.ToString(DataRow+DataCount)); 
				oRng.HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;
				oRng.NumberFormatLocal = "#,##0";

				oRng = oSheet.get_Range(GetColumnIndex(22)+Convert.ToString(DataRow), GetColumnIndex(22)+Convert.ToString(DataRow+DataCount)); 
				oRng.HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;
				oRng.NumberFormatLocal = "#,##0";

				oRng = oSheet.get_Range(GetColumnIndex(23)+Convert.ToString(DataRow), GetColumnIndex(23)+Convert.ToString(DataRow+DataCount)); 
				oRng.HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;
				oRng.NumberFormatLocal = "#,##0";

				oRng = oSheet.get_Range(GetColumnIndex(24)+Convert.ToString(DataRow), GetColumnIndex(24)+Convert.ToString(DataRow+DataCount)); 
				oRng.HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;
				oRng.NumberFormatLocal = "#,##0";

				oRng = oSheet.get_Range(GetColumnIndex(25)+Convert.ToString(DataRow), GetColumnIndex(25)+Convert.ToString(DataRow+DataCount)); 
				oRng.HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;
				oRng.NumberFormatLocal = "#,##0";

				oRng = oSheet.get_Range(GetColumnIndex(26)+Convert.ToString(DataRow), GetColumnIndex(26)+Convert.ToString(DataRow+DataCount)); 
				oRng.HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;
				oRng.NumberFormatLocal = "#,##0";

				oRng = oSheet.get_Range(GetColumnIndex(27)+Convert.ToString(DataRow), GetColumnIndex(27)+Convert.ToString(DataRow+DataCount)); 
				oRng.HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;
				oRng.NumberFormatLocal = "#,##0";

				oRng = oSheet.get_Range(GetColumnIndex(28)+Convert.ToString(DataRow), GetColumnIndex(28)+Convert.ToString(DataRow+DataCount)); 
				oRng.HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;
				oRng.NumberFormatLocal = "#,##0";

				oRng = oSheet.get_Range(GetColumnIndex(29)+Convert.ToString(DataRow), GetColumnIndex(29)+Convert.ToString(DataRow+DataCount)); 
				oRng.HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;
				oRng.NumberFormatLocal = "#,##0";

				oRng = oSheet.get_Range(GetColumnIndex(30)+Convert.ToString(DataRow), GetColumnIndex(30)+Convert.ToString(DataRow+DataCount)); 
				oRng.HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;
				oRng.NumberFormatLocal = "#,##0";

				oRng = oSheet.get_Range(GetColumnIndex(31)+Convert.ToString(DataRow), GetColumnIndex(31)+Convert.ToString(DataRow+DataCount)); 
				oRng.HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;
				oRng.NumberFormatLocal = "#,##0";

				oRng = oSheet.get_Range(GetColumnIndex(32)+Convert.ToString(DataRow), GetColumnIndex(32)+Convert.ToString(DataRow+DataCount)); 
				oRng.HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;
				oRng.NumberFormatLocal = "#,##0";

				oRng = oSheet.get_Range(GetColumnIndex(33)+Convert.ToString(DataRow), GetColumnIndex(33)+Convert.ToString(DataRow+DataCount)); 
				oRng.HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;
				oRng.NumberFormatLocal = "#,##0";

				oRng = oSheet.get_Range(GetColumnIndex(34)+Convert.ToString(DataRow), GetColumnIndex(34)+Convert.ToString(DataRow+DataCount)); 
				oRng.HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;
				oRng.NumberFormatLocal = "#,##0";

				oRng = oSheet.get_Range(GetColumnIndex(35)+Convert.ToString(DataRow), GetColumnIndex(35)+Convert.ToString(DataRow+DataCount)); 
				oRng.HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;
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


		private void cbSearchStartDay_TextChanged(object sender, System.EventArgs e)
		{
			cbSearchEndDay.Value  = cbSearchStartDay.Value.AddMonths(1).AddDays(-1);
		}

		private void cbSearchEndDay_TextChanged(object sender, System.EventArgs e)
		{
			if(cbSearchEndDay.Value > cbSearchStartDay.Value.AddMonths(1).AddDays(-1))
			{
				MessageBox.Show("집계종료일은 시작일로부터 한달을 넘을수 없습니다.\n확인 하시기 바랍니다.", "카테고리별 통계",
					MessageBoxButtons.OK, MessageBoxIcon.Information);			
				cbSearchEndDay.Value  = cbSearchStartDay.Value.AddMonths(1).AddDays(-1);
			}			
		}

        private void gridEX1_LoadingRow(object sender, Janus.Windows.GridEX.RowLoadEventArgs e)
        {
            
        }		
	}
}
