// ===============================================================================
//
// RptSummaryAdWeeklyControl.cs
//
// 광고주간레포트 컨트롤을 정의합니다. 
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
	/// 광고별 시청현황 집계 관리 컨트롤
	/// </summary>
    public class RptSummaryAdWeeklyControl : System.Windows.Forms.UserControl, IUserControl
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
		RptSummaryAdWeeklyModel rptSummaryAdWeeklyModel = new RptSummaryAdWeeklyModel();	// 광고주간레포트 모델
		
		// 화면처리용 변수
		bool canRead			  = false;

		// Key 데이터
		string keyReportStartDay = "";
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
		private System.Windows.Forms.Label label1;
		private Janus.Windows.GridEX.EditControls.EditBox editBox1;
		private System.Windows.Forms.Panel panMenuSchedule;
		private System.Windows.Forms.Panel panel2;
		private System.Windows.Forms.Panel panel3;
		private System.Windows.Forms.Label lbSearchDate;
		private Janus.Windows.EditControls.UIButton btnExcel;
		private System.Data.DataView dvReport;
		private Janus.Windows.GridEX.GridEX grdExReport;
		private Janus.Windows.UI.Dock.UIPanelManager uiPM;
		private AdManagerClient._51_ReportSummaryAd._02_RptSummaryAdWeekly.RptSummaryAdWeeklyDs rptSummaryAdWeeklyDs;
        private Janus.Windows.CalendarCombo.CalendarCombo cbSearchBgnDay;
        private Janus.Windows.CalendarCombo.CalendarCombo cbSearchEndDay;
		private System.ComponentModel.IContainer components;

		public RptSummaryAdWeeklyControl()
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RptSummaryAdWeeklyControl));
            this.uiPM = new Janus.Windows.UI.Dock.UIPanelManager(this.components);
            this.uiPanelChoiceAdSchedule = new Janus.Windows.UI.Dock.UIPanelGroup();
            this.uiPanelSearch = new Janus.Windows.UI.Dock.UIPanel();
            this.uiPanelSearchContainer = new Janus.Windows.UI.Dock.UIPanelInnerContainer();
            this.pnlSearch = new System.Windows.Forms.Panel();
            this.cbSearchEndDay = new Janus.Windows.CalendarCombo.CalendarCombo();
            this.cbSearchBgnDay = new Janus.Windows.CalendarCombo.CalendarCombo();
            this.lbSearchDate = new System.Windows.Forms.Label();
            this.btnSearch = new Janus.Windows.EditControls.UIButton();
            this.btnExcel = new Janus.Windows.EditControls.UIButton();
            this.uiPanelList = new Janus.Windows.UI.Dock.UIPanel();
            this.uiPanelListContainer = new Janus.Windows.UI.Dock.UIPanelInnerContainer();
            this.grdExReport = new Janus.Windows.GridEX.GridEX();
            this.dvReport = new System.Data.DataView();
            this.rptSummaryAdWeeklyDs = new AdManagerClient._51_ReportSummaryAd._02_RptSummaryAdWeekly.RptSummaryAdWeeklyDs();
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
            ((System.ComponentModel.ISupportInitialize)(this.rptSummaryAdWeeklyDs)).BeginInit();
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
            this.uiPanelChoiceAdSchedule.Text = "주간 광고 집행실적";
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
            this.pnlSearch.Controls.Add(this.cbSearchEndDay);
            this.pnlSearch.Controls.Add(this.cbSearchBgnDay);
            this.pnlSearch.Controls.Add(this.lbSearchDate);
            this.pnlSearch.Controls.Add(this.btnSearch);
            this.pnlSearch.Controls.Add(this.btnExcel);
            this.pnlSearch.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlSearch.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.pnlSearch.Location = new System.Drawing.Point(0, 0);
            this.pnlSearch.Name = "pnlSearch";
            this.pnlSearch.Size = new System.Drawing.Size(1008, 39);
            this.pnlSearch.TabIndex = 3;
            // 
            // cbSearchEndDay
            // 
            // 
            // 
            // 
            this.cbSearchEndDay.DropDownCalendar.FirstMonth = new System.DateTime(2007, 8, 1, 0, 0, 0, 0);
            this.cbSearchEndDay.DropDownCalendar.Name = "";
            this.cbSearchEndDay.DropDownCalendar.VisualStyle = Janus.Windows.CalendarCombo.VisualStyle.Office2007;
            this.cbSearchEndDay.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.cbSearchEndDay.Location = new System.Drawing.Point(208, 8);
            this.cbSearchEndDay.Name = "cbSearchEndDay";
            this.cbSearchEndDay.ReadOnly = true;
            this.cbSearchEndDay.Size = new System.Drawing.Size(120, 23);
            this.cbSearchEndDay.TabIndex = 15;
            this.cbSearchEndDay.TabStop = false;
            this.cbSearchEndDay.Value = new System.DateTime(2007, 9, 14, 0, 0, 0, 0);
            this.cbSearchEndDay.VisualStyle = Janus.Windows.CalendarCombo.VisualStyle.Office2007;
            // 
            // cbSearchBgnDay
            // 
            // 
            // 
            // 
            this.cbSearchBgnDay.DropDownCalendar.FirstMonth = new System.DateTime(2007, 8, 1, 0, 0, 0, 0);
            this.cbSearchBgnDay.DropDownCalendar.Name = "";
            this.cbSearchBgnDay.DropDownCalendar.VisualStyle = Janus.Windows.CalendarCombo.VisualStyle.Office2007;
            this.cbSearchBgnDay.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.cbSearchBgnDay.Location = new System.Drawing.Point(80, 8);
            this.cbSearchBgnDay.Name = "cbSearchBgnDay";
            this.cbSearchBgnDay.SecondIncrement = 1;
            this.cbSearchBgnDay.Size = new System.Drawing.Size(120, 23);
            this.cbSearchBgnDay.TabIndex = 4;
            this.cbSearchBgnDay.Value = new System.DateTime(2007, 9, 14, 0, 0, 0, 0);
            this.cbSearchBgnDay.VisualStyle = Janus.Windows.CalendarCombo.VisualStyle.Office2007;
            this.cbSearchBgnDay.ValueChanged += new System.EventHandler(this.cbSearchBgnDay_ValueChanged);
            // 
            // lbSearchDate
            // 
            this.lbSearchDate.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lbSearchDate.Location = new System.Drawing.Point(12, 8);
            this.lbSearchDate.Name = "lbSearchDate";
            this.lbSearchDate.Size = new System.Drawing.Size(72, 21);
            this.lbSearchDate.TabIndex = 14;
            this.lbSearchDate.Text = "집계기준일";
            this.lbSearchDate.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnSearch
            // 
            this.btnSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSearch.Enabled = false;
            this.btnSearch.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
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
            this.btnExcel.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnExcel.Location = new System.Drawing.Point(895, 8);
            this.btnExcel.Name = "btnExcel";
            this.btnExcel.Size = new System.Drawing.Size(104, 24);
            this.btnExcel.TabIndex = 7;
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
            // 
            // uiPanelListContainer
            // 
            this.uiPanelListContainer.BackColor = System.Drawing.SystemColors.Window;
            this.uiPanelListContainer.Controls.Add(this.grdExReport);
            this.uiPanelListContainer.Location = new System.Drawing.Point(1, 23);
            this.uiPanelListContainer.Name = "uiPanelListContainer";
            this.uiPanelListContainer.Size = new System.Drawing.Size(1008, 586);
            this.uiPanelListContainer.TabIndex = 0;
            // 
            // grdExReport
            // 
            this.grdExReport.AllowEdit = Janus.Windows.GridEX.InheritableBoolean.False;
            this.grdExReport.AlternatingColors = true;
            this.grdExReport.AlternatingRowFormatStyle.BackColorAlphaMode = Janus.Windows.GridEX.AlphaMode.UseAlpha;
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
            this.grdExReport.Font = new System.Drawing.Font("맑은 고딕", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.grdExReport.FrozenColumns = 1;
            this.grdExReport.GridLineColor = System.Drawing.Color.Silver;
            this.grdExReport.GridLines = Janus.Windows.GridEX.GridLines.None;
            this.grdExReport.GridLineStyle = Janus.Windows.GridEX.GridLineStyle.Solid;
            this.grdExReport.GroupByBoxInfoFormatStyle.BackColor = System.Drawing.Color.Empty;
            this.grdExReport.GroupByBoxVisible = false;
            this.grdExReport.GroupRowFormatStyle.BackColor = System.Drawing.Color.White;
            this.grdExReport.GroupRowFormatStyle.BackColorGradient = System.Drawing.Color.FromArgb(((int)(((byte)(218)))), ((int)(((byte)(228)))), ((int)(((byte)(238)))));
            this.grdExReport.GroupRowFormatStyle.FontBold = Janus.Windows.GridEX.TriState.True;
            this.grdExReport.GroupTotalRowFormatStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.grdExReport.GroupTotalRowFormatStyle.FontBold = Janus.Windows.GridEX.TriState.True;
            this.grdExReport.GroupTotalRowFormatStyle.ForeColor = System.Drawing.SystemColors.ControlText;
            this.grdExReport.HeaderFormatStyle.TextAlignment = Janus.Windows.GridEX.TextAlignment.Center;
            this.grdExReport.HideSelection = Janus.Windows.GridEX.HideSelection.Highlight;
            this.grdExReport.Location = new System.Drawing.Point(0, 0);
            this.grdExReport.Name = "grdExReport";
            this.grdExReport.RowFormatStyle.BackColor = System.Drawing.Color.Empty;
            this.grdExReport.SelectedInactiveFormatStyle.BackColor = System.Drawing.Color.Gold;
            this.grdExReport.Size = new System.Drawing.Size(1008, 586);
            this.grdExReport.TabIndex = 9;
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
            // 
            // dvReport
            // 
            this.dvReport.Table = this.rptSummaryAdWeeklyDs.RptWeekly;
            // 
            // rptSummaryAdWeeklyDs
            // 
            this.rptSummaryAdWeeklyDs.DataSetName = "RptSummaryAdWeeklyDs";
            this.rptSummaryAdWeeklyDs.Locale = new System.Globalization.CultureInfo("en-US");
            this.rptSummaryAdWeeklyDs.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
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
            // RptSummaryAdWeeklyControl
            // 
            this.Controls.Add(this.uiPanelChoiceAdSchedule);
            this.Name = "RptSummaryAdWeeklyControl";
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
            ((System.ComponentModel.ISupportInitialize)(this.rptSummaryAdWeeklyDs)).EndInit();
            this.panMenuSchedule.ResumeLayout(false);
            this.ResumeLayout(false);

        }
		#endregion

		#region 컨트롤 로드
		private void UserControl_Load(object sender, System.EventArgs e)
		{
			// 컨트롤 초기화
			InitControl();	
            cbSearchBgnDay.Focus();
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
			// 기간시작일 및 종료일을 금일로 셋트
			cbSearchBgnDay.Value = DateTime.Now.AddDays(-1);
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

		#endregion

		#region 처리메소드

		/// <summary>
		/// 일별시청률 조회
		/// </summary>
		private void SearchReport()
		{
			StatusMessage("주간 시청현황을 조회합니다.");
			ProgressStart();
			try
			{
				// 데이터모델 초기화
				rptSummaryAdWeeklyModel.Init();
				rptSummaryAdWeeklyModel.SearchStartDay  = cbSearchBgnDay.Value.ToString("yyMMdd");
				rptSummaryAdWeeklyModel.SearchDay       = cbSearchEndDay.Value.ToString("yyMMdd");
				uiPanelList.Text = "주간시청현황"; 

				//  주간 광고시청집계조회 서비스를 호출한다.
				new RptSummaryAdWeeklyManager(systemModel, commonModel).GetRptSummaryAdWeeklyList(rptSummaryAdWeeklyModel);
		
				if (rptSummaryAdWeeklyModel.ResultCD.Equals("0000"))
				{

					Utility.SetDataTable(rptSummaryAdWeeklyDs.RptWeekly, rptSummaryAdWeeklyModel.RptWeeklyDataSet);	
					StatusMessage(rptSummaryAdWeeklyModel.ResultCnt + "건이 조회되었습니다.");

					keyReportStartDay   = cbSearchBgnDay.Value.ToString("yyyy-MM-dd");
					keyReportDay   = cbSearchEndDay.Value.ToString("yyyy-MM-dd");

					uiPanelList.Text = "주간시청현황 : " + keyReportStartDay + " ~ " +keyReportDay;
										
					canPrint = true;

				}
			}
			catch(FrameException fe)
			{
				FrameSystem.showMsgForm("광고주간집계 현황 조회오류", new string[] {fe.ErrCode, fe.ResultMsg});
			}
			catch(Exception ex)
			{
				FrameSystem.showMsgForm("광고주간집계 현황 조회오류",new string[] {"",ex.Message});
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

            string N_ItemNo = "";
			try
			{	
				int ColMax  = 24; // 컬럼수   				
				int HeaderRow   = 1;
				int DataRow   = 3;
				string StartCol = "A";
				string EndCol   = "";
				//string TitleCol = "E";
				int DataCount = 0;
				//int CondCount = 0;
				int HeaderCount = 0;

				// 마지막 컬럼의 인덱스문자
				EndCol = GetColumnIndex(ColMax);

				xlApp = new Excel.Application();
				oWB = (Excel._Workbook)(xlApp.Workbooks.Add(Missing.Value));
				oSheet = (Excel._Worksheet)oWB.ActiveSheet;

				// 헤더 정보 작성
				oSheet.Cells[HeaderRow,1] = "주간광고집행실적["  + keyReportStartDay +" ~ "+ keyReportDay + "]";
				oRng = oSheet.get_Range(GetColumnIndex(1)+Convert.ToString(HeaderRow), GetColumnIndex(3)+Convert.ToString(HeaderRow));
				oRng.Merge(true);

				oSheet.Cells[HeaderRow,4] = "광고Hit";
				oRng = oSheet.get_Range(GetColumnIndex(4)+Convert.ToString(HeaderRow), GetColumnIndex(11)+Convert.ToString(HeaderRow));
				oRng.Merge(true);

				oSheet.Cells[HeaderRow,12] = "광고가구";
				oRng = oSheet.get_Range(GetColumnIndex(12)+Convert.ToString(HeaderRow), GetColumnIndex(19)+Convert.ToString(HeaderRow));
				oRng.Merge(true);

				oSheet.Cells[HeaderRow,20] = "주간";
				oRng = oSheet.get_Range(GetColumnIndex(20)+Convert.ToString(HeaderRow), GetColumnIndex(21)+Convert.ToString(HeaderRow));
				oRng.Merge(true);

				oSheet.Cells[HeaderRow,22] = "누적";
				oRng = oSheet.get_Range(GetColumnIndex(22)+Convert.ToString(HeaderRow), GetColumnIndex(24)+Convert.ToString(HeaderRow));
				oRng.Merge(true);

				HeaderCount = 1;
                oSheet.Cells[HeaderRow+1,HeaderCount++] = "No";
				oSheet.Cells[HeaderRow+1,HeaderCount++] = "Item";
				oSheet.Cells[HeaderRow+1,HeaderCount++] = "광고명";
				oSheet.Cells[HeaderRow+1,HeaderCount++] = "월";
				oSheet.Cells[HeaderRow+1,HeaderCount++] = "화";
				oSheet.Cells[HeaderRow+1,HeaderCount++] = "수";
				oSheet.Cells[HeaderRow+1,HeaderCount++] = "목";
				oSheet.Cells[HeaderRow+1,HeaderCount++] = "금";
				oSheet.Cells[HeaderRow+1,HeaderCount++] = "토";
				oSheet.Cells[HeaderRow+1,HeaderCount++] = "일";
				oSheet.Cells[HeaderRow+1,HeaderCount++] = "주간합";
				oSheet.Cells[HeaderRow+1,HeaderCount++] = "월";
				oSheet.Cells[HeaderRow+1,HeaderCount++] = "화";
				oSheet.Cells[HeaderRow+1,HeaderCount++] = "수";
				oSheet.Cells[HeaderRow+1,HeaderCount++] = "목";
				oSheet.Cells[HeaderRow+1,HeaderCount++] = "금";
				oSheet.Cells[HeaderRow+1,HeaderCount++] = "토";
				oSheet.Cells[HeaderRow+1,HeaderCount++] = "일";
				oSheet.Cells[HeaderRow+1,HeaderCount++] = "주간합";
				oSheet.Cells[HeaderRow+1,HeaderCount++] = "Cove";
				oSheet.Cells[HeaderRow+1,HeaderCount++] = "Freq";
				oSheet.Cells[HeaderRow+1,HeaderCount++] = "Hit";
				oSheet.Cells[HeaderRow+1,HeaderCount++] = "가구";
				oSheet.Cells[HeaderRow+1,HeaderCount++] = "일수";

				oRng = oSheet.get_Range(StartCol+Convert.ToString(HeaderRow), EndCol+Convert.ToString(HeaderRow+1));	// 헤더의 범위
				oRng.Font.Bold          = true;							// 폰트 굵게
                oRng.Font.Size          = 9;
				oRng.VerticalAlignment  = Excel.XlVAlign.xlVAlignCenter;	// 세로중앙정렬
				oRng.HorizontalAlignment= Excel.XlHAlign.xlHAlignCenter;	// 가로중앙정렬	 
				oRng.Interior.Color     = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.CornflowerBlue);   //셀 배경색 
				oRng.Font.Color         = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.White);			//텍스트색			

                oRng = oSheet.get_Range("A1","A1");	// 헤더의 범위
                oRng.Font.Bold          = true;							// 폰트 굵게
                oRng.Font.Size          = 11;
				
				DataCount = 0;
                int no = 0;
				// 데이터 추출
				for (int inx =0; inx < rptSummaryAdWeeklyDs.RptWeekly.Rows.Count; inx++)
				{		
					DataRow Row = rptSummaryAdWeeklyDs.RptWeekly.Rows[inx];
                    
					int ColCnt = 1;

					N_ItemNo = Row["ItemNo"].ToString();	
                    no++;

					if(N_ItemNo.Equals("88888888")) // 카테고리합계이면
					{
                        no = 0;
                        // 중간합계Row 색상설정
						oRng = oSheet.get_Range(GetColumnIndex(1)+Convert.ToString(DataRow+DataCount), GetColumnIndex(24)+Convert.ToString(DataRow+DataCount));
						oRng.Interior.Color      = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Yellow);   //셀 배경색 
                        
                        // 제목부분 머지
   						oRng = oSheet.get_Range(GetColumnIndex(1)+Convert.ToString(DataRow+DataCount), GetColumnIndex(3)+Convert.ToString(DataRow+DataCount));
                        oRng.Merge(true);

                        oRng = oSheet.get_Range(GetColumnIndex(1)+Convert.ToString(DataRow+DataCount), GetColumnIndex(24)+Convert.ToString(DataRow+DataCount));
                        oRng.Font.Bold  = true;							// 폰트 굵게
                        oRng.Font.Size  = 9;

                        //oSheet.Cells[DataRow+DataCount,ColCnt++] = no.ToString();
                        //oSheet.Cells[DataRow+DataCount,ColCnt++] = Row["ItemNo1"].ToString();								// 1  광고번호
                        oSheet.Cells[DataRow+DataCount,1] = Row["ItemName"].ToString();								// 2  광고명
                        ColCnt = 4;
                        oSheet.Cells[DataRow+DataCount,ColCnt++] = Convert.ToInt64(Row["DayAdCnt1"].ToString());			// 3  월
                        oSheet.Cells[DataRow+DataCount,ColCnt++] = Convert.ToInt64(Row["DayAdCnt2"].ToString());			// 4  화
                        oSheet.Cells[DataRow+DataCount,ColCnt++] = Convert.ToInt64(Row["DayAdCnt3"].ToString());			// 5  수
                        oSheet.Cells[DataRow+DataCount,ColCnt++] = Convert.ToInt64(Row["DayAdCnt4"].ToString());			// 6  목
                        oSheet.Cells[DataRow+DataCount,ColCnt++] = Convert.ToInt64(Row["DayAdCnt5"].ToString());			// 7  금
                        oSheet.Cells[DataRow+DataCount,ColCnt++] = Convert.ToInt64(Row["DayAdCnt6"].ToString());			// 8  토
                        oSheet.Cells[DataRow+DataCount,ColCnt++] = Convert.ToInt64(Row["DayAdCnt7"].ToString());			// 9  일
                        oSheet.Cells[DataRow+DataCount,ColCnt++] = Convert.ToInt64(Row["SumDayAdCnt"].ToString());			// 10  주간합
                        oSheet.Cells[DataRow+DataCount,ColCnt++] = Convert.ToInt64(Row["DayAdUsers1"].ToString());			// 11  월
                        oSheet.Cells[DataRow+DataCount,ColCnt++] = Convert.ToInt64(Row["DayAdUsers2"].ToString());			// 12  화
                        oSheet.Cells[DataRow+DataCount,ColCnt++] = Convert.ToInt64(Row["DayAdUsers3"].ToString());			// 13  수
                        oSheet.Cells[DataRow+DataCount,ColCnt++] = Convert.ToInt64(Row["DayAdUsers4"].ToString());			// 14  목
                        oSheet.Cells[DataRow+DataCount,ColCnt++] = Convert.ToInt64(Row["DayAdUsers5"].ToString());			// 15  금
                        oSheet.Cells[DataRow+DataCount,ColCnt++] = Convert.ToInt64(Row["DayAdUsers6"].ToString());			// 16  토
                        oSheet.Cells[DataRow+DataCount,ColCnt++] = Convert.ToInt64(Row["DayAdUsers7"].ToString());			// 17  일
                        oSheet.Cells[DataRow+DataCount,ColCnt++] = Convert.ToInt64(Row["SumDayUsers"].ToString());			// 18  주간
                        oSheet.Cells[DataRow+DataCount,ColCnt++] = Convert.ToDecimal(Row["DayCovr"].ToString());			// 19  Cove
                        oSheet.Cells[DataRow+DataCount,ColCnt++] = Convert.ToDecimal(Row["DayFreq"].ToString());			// 20  Freq
                        oSheet.Cells[DataRow+DataCount,ColCnt++] = Convert.ToInt64(Row["AccuAdCnt"].ToString());			// 21  Rate
                        oSheet.Cells[DataRow+DataCount,ColCnt++] = Convert.ToInt64(Row["AccuAdUsers"].ToString());			// 22  Cove
                        oSheet.Cells[DataRow+DataCount,ColCnt++] = Row["RunDay"].ToString();								// 23 Days
					}
                    else if(N_ItemNo.Equals("99999999")) // 전체합계이면
                    {
                        oSheet.Cells[DataRow+DataCount,1] = "";
                        oRng = oSheet.get_Range(GetColumnIndex(1)+Convert.ToString(DataRow+DataCount), GetColumnIndex(24)+Convert.ToString(DataRow+DataCount));
                        oRng.Interior.Color      = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Chartreuse);   //셀 배경색 

                        oRng = oSheet.get_Range(GetColumnIndex(1)+Convert.ToString(DataRow+DataCount), GetColumnIndex(3)+Convert.ToString(DataRow+DataCount));
                        oRng.Merge(true);

                        oRng = oSheet.get_Range(GetColumnIndex(1)+Convert.ToString(DataRow+DataCount), GetColumnIndex(24)+Convert.ToString(DataRow+DataCount));
                        oRng.Font.Bold  = true;							// 폰트 굵게
                        oRng.Font.Size  = 9;

                        oSheet.Cells[DataRow+DataCount,1] = Row["ItemName"].ToString();								// 2  광고명
                        ColCnt = 4;

                        oSheet.Cells[DataRow+DataCount,ColCnt++] = Convert.ToInt64(Row["DayAdCnt1"].ToString());			// 3  월
                        oSheet.Cells[DataRow+DataCount,ColCnt++] = Convert.ToInt64(Row["DayAdCnt2"].ToString());			// 4  화
                        oSheet.Cells[DataRow+DataCount,ColCnt++] = Convert.ToInt64(Row["DayAdCnt3"].ToString());			// 5  수
                        oSheet.Cells[DataRow+DataCount,ColCnt++] = Convert.ToInt64(Row["DayAdCnt4"].ToString());			// 6  목
                        oSheet.Cells[DataRow+DataCount,ColCnt++] = Convert.ToInt64(Row["DayAdCnt5"].ToString());			// 7  금
                        oSheet.Cells[DataRow+DataCount,ColCnt++] = Convert.ToInt64(Row["DayAdCnt6"].ToString());			// 8  토
                        oSheet.Cells[DataRow+DataCount,ColCnt++] = Convert.ToInt64(Row["DayAdCnt7"].ToString());			// 9  일
                        oSheet.Cells[DataRow+DataCount,ColCnt++] = Convert.ToInt64(Row["SumDayAdCnt"].ToString());			// 10  주간합
                        oSheet.Cells[DataRow+DataCount,ColCnt++] = Convert.ToInt64(Row["DayAdUsers1"].ToString());			// 11  월
                        oSheet.Cells[DataRow+DataCount,ColCnt++] = Convert.ToInt64(Row["DayAdUsers2"].ToString());			// 12  화
                        oSheet.Cells[DataRow+DataCount,ColCnt++] = Convert.ToInt64(Row["DayAdUsers3"].ToString());			// 13  수
                        oSheet.Cells[DataRow+DataCount,ColCnt++] = Convert.ToInt64(Row["DayAdUsers4"].ToString());			// 14  목
                        oSheet.Cells[DataRow+DataCount,ColCnt++] = Convert.ToInt64(Row["DayAdUsers5"].ToString());			// 15  금
                        oSheet.Cells[DataRow+DataCount,ColCnt++] = Convert.ToInt64(Row["DayAdUsers6"].ToString());			// 16  토
                        oSheet.Cells[DataRow+DataCount,ColCnt++] = Convert.ToInt64(Row["DayAdUsers7"].ToString());			// 17  일
                        oSheet.Cells[DataRow+DataCount,ColCnt++] = Convert.ToInt64(Row["SumDayUsers"].ToString());			// 18  주간
                        oSheet.Cells[DataRow+DataCount,ColCnt++] = Convert.ToDecimal(Row["DayCovr"].ToString());			// 19  Cove
                        oSheet.Cells[DataRow+DataCount,ColCnt++] = Convert.ToDecimal(Row["DayFreq"].ToString());			// 20  Freq
                        oSheet.Cells[DataRow+DataCount,ColCnt++] = Convert.ToInt64(Row["AccuAdCnt"].ToString());			// 21  Rate
                        oSheet.Cells[DataRow+DataCount,ColCnt++] = Convert.ToInt64(Row["AccuAdUsers"].ToString());			// 22  Cove
                        oSheet.Cells[DataRow+DataCount,ColCnt++] = Row["RunDay"].ToString();								// 23 Days
                    }
                    else
                    {
                        oRng = oSheet.get_Range(GetColumnIndex(1)+Convert.ToString(DataRow+DataCount), GetColumnIndex(24)+Convert.ToString(DataRow+DataCount));
                        oRng.Font.Size  = 8;

                        oSheet.Cells[DataRow+DataCount,ColCnt++] = no.ToString();
                        oSheet.Cells[DataRow+DataCount,ColCnt++] = Row["ItemNo1"].ToString();								// 1  광고번호
                        oSheet.Cells[DataRow+DataCount,ColCnt++] = Row["ItemName"].ToString();								// 2  광고명
                        oSheet.Cells[DataRow+DataCount,ColCnt++] = Convert.ToInt64(Row["DayAdCnt1"].ToString());			// 3  월
                        oSheet.Cells[DataRow+DataCount,ColCnt++] = Convert.ToInt64(Row["DayAdCnt2"].ToString());			// 4  화
                        oSheet.Cells[DataRow+DataCount,ColCnt++] = Convert.ToInt64(Row["DayAdCnt3"].ToString());			// 5  수
                        oSheet.Cells[DataRow+DataCount,ColCnt++] = Convert.ToInt64(Row["DayAdCnt4"].ToString());			// 6  목
                        oSheet.Cells[DataRow+DataCount,ColCnt++] = Convert.ToInt64(Row["DayAdCnt5"].ToString());			// 7  금
                        oSheet.Cells[DataRow+DataCount,ColCnt++] = Convert.ToInt64(Row["DayAdCnt6"].ToString());			// 8  토
                        oSheet.Cells[DataRow+DataCount,ColCnt++] = Convert.ToInt64(Row["DayAdCnt7"].ToString());			// 9  일
                        oSheet.Cells[DataRow+DataCount,ColCnt++] = Convert.ToInt64(Row["SumDayAdCnt"].ToString());			// 10  주간합
                        oSheet.Cells[DataRow+DataCount,ColCnt++] = Convert.ToInt64(Row["DayAdUsers1"].ToString());			// 11  월
                        oSheet.Cells[DataRow+DataCount,ColCnt++] = Convert.ToInt64(Row["DayAdUsers2"].ToString());			// 12  화
                        oSheet.Cells[DataRow+DataCount,ColCnt++] = Convert.ToInt64(Row["DayAdUsers3"].ToString());			// 13  수
                        oSheet.Cells[DataRow+DataCount,ColCnt++] = Convert.ToInt64(Row["DayAdUsers4"].ToString());			// 14  목
                        oSheet.Cells[DataRow+DataCount,ColCnt++] = Convert.ToInt64(Row["DayAdUsers5"].ToString());			// 15  금
                        oSheet.Cells[DataRow+DataCount,ColCnt++] = Convert.ToInt64(Row["DayAdUsers6"].ToString());			// 16  토
                        oSheet.Cells[DataRow+DataCount,ColCnt++] = Convert.ToInt64(Row["DayAdUsers7"].ToString());			// 17  일
                        oSheet.Cells[DataRow+DataCount,ColCnt++] = Convert.ToInt64(Row["SumDayUsers"].ToString());			// 18  주간
                        oSheet.Cells[DataRow+DataCount,ColCnt++] = Convert.ToDecimal(Row["DayCovr"].ToString());			// 19  Cove
                        oSheet.Cells[DataRow+DataCount,ColCnt++] = Convert.ToDecimal(Row["DayFreq"].ToString());			// 20  Freq
                        oSheet.Cells[DataRow+DataCount,ColCnt++] = Convert.ToInt64(Row["AccuAdCnt"].ToString());			// 21  Rate
                        oSheet.Cells[DataRow+DataCount,ColCnt++] = Convert.ToInt64(Row["AccuAdUsers"].ToString());			// 22  Cove
                        oSheet.Cells[DataRow+DataCount,ColCnt++] = Row["RunDay"].ToString();								// 23 Days
                    }

					DataCount++;
				}

				DataCount--;

				// 데이터 작성
				oRng = oSheet.get_Range(StartCol+Convert.ToString(HeaderRow), EndCol+Convert.ToString(DataRow+DataCount));	// 데이터의 범위
				oRng.EntireColumn.AutoFit();					// 데이터의 크기에 셀의 가로크기 맞춤
				oRng.Borders.LineStyle =  Excel.XlLineStyle.xlContinuous;	// 테두리속성 아래에 실선
				oRng.Borders.Weight    = Excel.XlBorderWeight.xlThin;		// 테두리속성 아래에 가는선

                //순번 중앙정렬
                oRng = oSheet.get_Range(GetColumnIndex(1)+Convert.ToString(DataRow), GetColumnIndex(1)+Convert.ToString(DataRow+DataCount)); 
                oRng.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;

				//광고번호 중앙정렬
				oRng = oSheet.get_Range(GetColumnIndex(2)+Convert.ToString(DataRow), GetColumnIndex(2)+Convert.ToString(DataRow+DataCount)); 
				oRng.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;

                //광고명 left정렬
                oRng = oSheet.get_Range(GetColumnIndex(3)+Convert.ToString(DataRow), GetColumnIndex(3)+Convert.ToString(DataRow+DataCount)); 
                oRng.HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft;

				//월 right정렬
				oRng = oSheet.get_Range(GetColumnIndex(4)+Convert.ToString(DataRow), GetColumnIndex(4)+Convert.ToString(DataRow+DataCount)); 
				oRng.HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;
				oRng.NumberFormatLocal = "#,##0";

				//화 right정렬
				oRng = oSheet.get_Range(GetColumnIndex(5)+Convert.ToString(DataRow), GetColumnIndex(5)+Convert.ToString(DataRow+DataCount)); 
				oRng.HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;
				oRng.NumberFormatLocal = "#,##0";

				//수 right정렬
				oRng = oSheet.get_Range(GetColumnIndex(6)+Convert.ToString(DataRow), GetColumnIndex(6)+Convert.ToString(DataRow+DataCount)); 
				oRng.HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;
				oRng.NumberFormatLocal = "#,##0";

				//목 right정렬
				oRng = oSheet.get_Range(GetColumnIndex(7)+Convert.ToString(DataRow), GetColumnIndex(7)+Convert.ToString(DataRow+DataCount)); 
				oRng.HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;
				oRng.NumberFormatLocal = "#,##0";

				//금 right정렬
				oRng = oSheet.get_Range(GetColumnIndex(8)+Convert.ToString(DataRow), GetColumnIndex(8)+Convert.ToString(DataRow+DataCount)); 
				oRng.HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;
				oRng.NumberFormatLocal = "#,##0";

				//토 right정렬
				oRng = oSheet.get_Range(GetColumnIndex(9)+Convert.ToString(DataRow), GetColumnIndex(9)+Convert.ToString(DataRow+DataCount)); 
				oRng.HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;
				oRng.NumberFormatLocal = "#,##0";
				
				//일 right정렬
				oRng = oSheet.get_Range(GetColumnIndex(10)+Convert.ToString(DataRow), GetColumnIndex(10)+Convert.ToString(DataRow+DataCount)); 
				oRng.HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;
				oRng.NumberFormatLocal = "#,##0";

				//주간합 right정렬
				oRng = oSheet.get_Range(GetColumnIndex(11)+Convert.ToString(DataRow), GetColumnIndex(11)+Convert.ToString(DataRow+DataCount)); 
				oRng.HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;
				oRng.NumberFormatLocal = "#,##0";

				//월 right정렬
				oRng = oSheet.get_Range(GetColumnIndex(12)+Convert.ToString(DataRow), GetColumnIndex(12)+Convert.ToString(DataRow+DataCount)); 
				oRng.HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;
				oRng.NumberFormatLocal = "#,##0";

				//화 right정렬
				oRng = oSheet.get_Range(GetColumnIndex(13)+Convert.ToString(DataRow), GetColumnIndex(13)+Convert.ToString(DataRow+DataCount)); 
				oRng.HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;
				oRng.NumberFormatLocal = "#,##0";

				//수 right정렬
				oRng = oSheet.get_Range(GetColumnIndex(14)+Convert.ToString(DataRow), GetColumnIndex(14)+Convert.ToString(DataRow+DataCount)); 
				oRng.HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;
				oRng.NumberFormatLocal = "#,##0";

				//목 right정렬
				oRng = oSheet.get_Range(GetColumnIndex(15)+Convert.ToString(DataRow), GetColumnIndex(15)+Convert.ToString(DataRow+DataCount)); 
				oRng.HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;
				oRng.NumberFormatLocal = "#,##0";

				//금 right정렬
				oRng = oSheet.get_Range(GetColumnIndex(16)+Convert.ToString(DataRow), GetColumnIndex(16)+Convert.ToString(DataRow+DataCount)); 
				oRng.HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;
				oRng.NumberFormatLocal = "#,##0";

				//토 right정렬
				oRng = oSheet.get_Range(GetColumnIndex(17)+Convert.ToString(DataRow), GetColumnIndex(17)+Convert.ToString(DataRow+DataCount)); 
				oRng.HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;
				oRng.NumberFormatLocal = "#,##0";
				
				//일 right정렬
				oRng = oSheet.get_Range(GetColumnIndex(18)+Convert.ToString(DataRow), GetColumnIndex(18)+Convert.ToString(DataRow+DataCount)); 
				oRng.HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;
				oRng.NumberFormatLocal = "#,##0";

				//주간 right정렬
				oRng = oSheet.get_Range(GetColumnIndex(19)+Convert.ToString(DataRow), GetColumnIndex(19)+Convert.ToString(DataRow+DataCount)); 
				oRng.HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;
				oRng.NumberFormatLocal = "#,##0";

				//Cove right정렬
				oRng = oSheet.get_Range(GetColumnIndex(20)+Convert.ToString(DataRow), GetColumnIndex(20)+Convert.ToString(DataRow+DataCount)); 
				oRng.HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;
				oRng.NumberFormatLocal = "#,##0.00";

				//Freq right정렬
				oRng = oSheet.get_Range(GetColumnIndex(21)+Convert.ToString(DataRow), GetColumnIndex(21)+Convert.ToString(DataRow+DataCount)); 
				oRng.HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;
				oRng.NumberFormatLocal = "#,##0.00";
				
				//Hit right정렬
				oRng = oSheet.get_Range(GetColumnIndex(22)+Convert.ToString(DataRow), GetColumnIndex(22)+Convert.ToString(DataRow+DataCount)); 
				oRng.HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;
				oRng.NumberFormatLocal = "#,##0";
	
				//가구 right정렬
				oRng = oSheet.get_Range(GetColumnIndex(23)+Convert.ToString(DataRow), GetColumnIndex(23)+Convert.ToString(DataRow+DataCount)); 
				oRng.HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;
				oRng.NumberFormatLocal = "#,##0";

				//일수 right정렬
				oRng = oSheet.get_Range(GetColumnIndex(24)+Convert.ToString(DataRow), GetColumnIndex(24)+Convert.ToString(DataRow+DataCount)); 
				oRng.HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;

                oRng = oSheet.get_Range(GetColumnIndex(4)+Convert.ToString(1), GetColumnIndex(11)+Convert.ToString(DataRow+DataCount)); 
                oRng.Borders[Excel.XlBordersIndex.xlEdgeRight].LineStyle = Excel.XlLineStyle.xlDouble;
                oRng.Borders[Excel.XlBordersIndex.xlEdgeLeft].LineStyle = Excel.XlLineStyle.xlDouble;

                oRng = oSheet.get_Range(GetColumnIndex(20)+Convert.ToString(1), GetColumnIndex(21)+Convert.ToString(DataRow+DataCount)); 
                oRng.Borders[Excel.XlBordersIndex.xlEdgeRight].LineStyle = Excel.XlLineStyle.xlDouble;
                oRng.Borders[Excel.XlBordersIndex.xlEdgeLeft].LineStyle = Excel.XlLineStyle.xlDouble;
                

				xlApp.Visible = true;
				xlApp.UserControl = true;
			}
			catch(Exception ex)
			{
				MessageBox.Show(" [광고번호:" + N_ItemNo.ToString() + "] " +  ex.Message);
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

        private void cbSearchBgnDay_ValueChanged(object sender, System.EventArgs e)
        {
            cbSearchEndDay.Value = cbSearchBgnDay.Value.AddDays(6);
        }

	}
}
