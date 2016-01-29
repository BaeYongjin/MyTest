// ===============================================================================
//
// OAPWeekChannelJumpControl.cs
//
// ���� ��û��Ȳ ���� ��Ʈ���� �����մϴ�. 
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
using Excel = Microsoft.Office.Interop.Excel; // ���� ����
using System.Reflection;

namespace AdManagerClient
{
	/// <summary>
	/// ���� ��û��Ȳ ���� ���� ��Ʈ��
	/// </summary>
    public class OAPWeekChannelJumpControl : System.Windows.Forms.UserControl, IUserControl
	{
		#region �̺�Ʈ�ڵ鷯
		public event StatusEventHandler 			StatusEvent;			// �����̺�Ʈ �ڵ鷯
		public event ProgressEventHandler 			ProgressEvent;			// ó�����̺�Ʈ �ڵ鷯
		#endregion
			
		#region ��������� ��ü �� ����

		// �ý��� ���� : ȭ�����
		private SystemModel   systemModel   = FrameSystem.oSysModel;
		private CommonModel   commonModel   = FrameSystem.oComModel;
		private Logger        log           = FrameSystem.oLog;
		private MenuPower     menu          = FrameSystem.oMenu;

		// �޴��ڵ� : ������ �ʿ��� ȭ�鿡 �ʿ���
		public string        menuCode		= "";

		// ����� ������
		OAPWeekSummaryRptModel oAPWeekSummaryRptModel  = new OAPWeekSummaryRptModel();	// ���α׷��� �����û�����
		SummaryAdModel summaryAdModel  = new SummaryAdModel();	// ���α׷��� �����û�����

		// ȭ��ó���� ����
		bool canRead			  = false;

		// Key ������
		public string keyContractName = "";		
		public string keyItemName     = "";		

		public string StartDay = "";
		public string EndDay   = "";

		string keyStartDay = "";
		string keyEndDay = "";
		string keyReportDay = "";	

		bool canPrint  = false;

		#endregion

        #region IUserControl ����
        /// <summary>
        /// �޴� �ڵ�-������ �ʿ��� ȭ�鿡 �ʿ���
        /// </summary>
        public string MenuCode
        {
            set { this.menuCode = value; }
            get { return this.menuCode; }
        }

        /// <summary>
        /// �θ���Ʈ�� ����
        /// </summary>
        /// <param name="control"></param>
        public void SetParent(Control control)
        {
            this.Parent = control;
        }
        /// <summary>
        /// DockStype����
        /// </summary>
        /// <param name="style"></param>
        public void SetDockStyle(DockStyle style)
        {
            this.Dock = style;
        }
        #endregion

		#region ȭ�� ������Ʈ, ������, �Ҹ���

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

		public OAPWeekChannelJumpControl()
		{
			// �� ȣ���� Windows.Forms Form �����̳ʿ� �ʿ��մϴ�.
			InitializeComponent();
		}

		/// <summary> 
		/// ��� ���� ��� ���ҽ��� �����մϴ�.
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

		#region ���� ��� �����̳ʿ��� ������ �ڵ�
		/// <summary> 
		/// �����̳� ������ �ʿ��� �޼����Դϴ�. 
		/// �� �޼����� ������ �ڵ� ������� �������� ���ʽÿ�.
		/// </summary>
		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            Janus.Windows.GridEX.GridEXLayout gridEX2_DesignTimeLayout = new Janus.Windows.GridEX.GridEXLayout();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(OAPWeekChannelJumpControl));
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
            this.uiPanelChoiceAdSchedule.Location = new System.Drawing.Point(0, 0);
            this.uiPanelChoiceAdSchedule.Name = "uiPanelChoiceAdSchedule";
            this.uiPanelChoiceAdSchedule.Size = new System.Drawing.Size(1010, 677);
            this.uiPanelChoiceAdSchedule.TabIndex = 4;
            this.uiPanelChoiceAdSchedule.Text = "�ְ�ä������";
            // 
            // uiPanelSearch
            // 
            this.uiPanelSearch.AllowResize = Janus.Windows.UI.InheritableBoolean.False;
            this.uiPanelSearch.CaptionVisible = Janus.Windows.UI.InheritableBoolean.False;
            this.uiPanelSearch.CloseButtonVisible = Janus.Windows.UI.InheritableBoolean.False;
            this.uiPanelSearch.InnerContainer = this.uiPanelSearchContainer;
            this.uiPanelSearch.Location = new System.Drawing.Point(0, 22);
            this.uiPanelSearch.Name = "uiPanelSearch";
            this.uiPanelSearch.Size = new System.Drawing.Size(1010, 40);
            this.uiPanelSearch.TabIndex = 4;
            this.uiPanelSearch.Text = "�˻�";
            // 
            // uiPanelSearchContainer
            // 
            this.uiPanelSearchContainer.Controls.Add(this.pnlSearch);
            this.uiPanelSearchContainer.Location = new System.Drawing.Point(1, 1);
            this.uiPanelSearchContainer.Name = "uiPanelSearchContainer";
            this.uiPanelSearchContainer.Size = new System.Drawing.Size(1008, 38);
            this.uiPanelSearchContainer.TabIndex = 0;
            // 
            // pnlSearch
            // 
            this.pnlSearch.BackColor = System.Drawing.SystemColors.Window;
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
            this.pnlSearch.Size = new System.Drawing.Size(1008, 38);
            this.pnlSearch.TabIndex = 3;
            // 
            // cbSearchStartDay
            // 
            // 
            // 
            // 
            this.cbSearchStartDay.DropDownCalendar.FirstMonth = new System.DateTime(2007, 8, 1, 0, 0, 0, 0);
            this.cbSearchStartDay.DropDownCalendar.Name = "";
            this.cbSearchStartDay.DropDownCalendar.VisualStyle = Janus.Windows.CalendarCombo.VisualStyle.Office2007;
            this.cbSearchStartDay.Location = new System.Drawing.Point(80, 8);
            this.cbSearchStartDay.Name = "cbSearchStartDay";
            this.cbSearchStartDay.Size = new System.Drawing.Size(104, 21);
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
            this.lbSearchDate.Text = "���������";
            this.lbSearchDate.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label10
            // 
            this.label10.Location = new System.Drawing.Point(184, 8);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(32, 21);
            this.label10.TabIndex = 29;
            this.label10.Text = "����";
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
            this.cbSearchEndDay.Location = new System.Drawing.Point(216, 8);
            this.cbSearchEndDay.Name = "cbSearchEndDay";
            this.cbSearchEndDay.Size = new System.Drawing.Size(104, 21);
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
            this.label11.Text = "����";
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
            this.btnSearch.Text = "�� ȸ";
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
            this.btnExcel.Text = "EXCEL ���";
            this.btnExcel.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.btnExcel.Click += new System.EventHandler(this.btnExcel_Click);
            // 
            // uiPanelList
            // 
            this.uiPanelList.BackColor = System.Drawing.SystemColors.Window;
            this.uiPanelList.CaptionStyle = Janus.Windows.UI.Dock.PanelCaptionStyle.Dark;
            this.uiPanelList.InnerContainer = this.uiPanelListContainer;
            this.uiPanelList.Location = new System.Drawing.Point(0, 66);
            this.uiPanelList.MinimumSize = new System.Drawing.Size(-1, 100);
            this.uiPanelList.Name = "uiPanelList";
            this.uiPanelList.Size = new System.Drawing.Size(1010, 611);
            this.uiPanelList.TabIndex = 4;
            this.uiPanelList.Text = "���";
            // 
            // uiPanelListContainer
            // 
            this.uiPanelListContainer.BackColor = System.Drawing.SystemColors.Window;
            this.uiPanelListContainer.Controls.Add(this.gridEX2);
            this.uiPanelListContainer.Location = new System.Drawing.Point(1, 23);
            this.uiPanelListContainer.Name = "uiPanelListContainer";
            this.uiPanelListContainer.Size = new System.Drawing.Size(1008, 587);
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
            this.gridEX2.Font = new System.Drawing.Font("���� ���", 8.25F);
            this.gridEX2.FrozenColumns = 2;
            this.gridEX2.GridLineColor = System.Drawing.Color.Silver;
            this.gridEX2.GridLines = Janus.Windows.GridEX.GridLines.Vertical;
            this.gridEX2.GridLineStyle = Janus.Windows.GridEX.GridLineStyle.Solid;
            this.gridEX2.GroupByBoxVisible = false;
            this.gridEX2.GroupTotals = Janus.Windows.GridEX.GroupTotals.Always;
            this.gridEX2.HeaderFormatStyle.TextAlignment = Janus.Windows.GridEX.TextAlignment.Center;
            this.gridEX2.HideSelection = Janus.Windows.GridEX.HideSelection.Highlight;
            this.gridEX2.Location = new System.Drawing.Point(0, 0);
            this.gridEX2.Name = "gridEX2";
            this.gridEX2.SelectedInactiveFormatStyle.BackColor = System.Drawing.Color.Gold;
            this.gridEX2.Size = new System.Drawing.Size(1008, 587);
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
            this.gridEX2.LoadingRow += new Janus.Windows.GridEX.RowLoadEventHandler(this.gridEX2_LoadingRow);
            // 
            // dvReport
            // 
            this.dvReport.Table = this.oapWeekSummaryRptDs.OAPWeekChannelJump;
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
            // OAPWeekChannelJumpControl
            // 
            this.Controls.Add(this.uiPanelChoiceAdSchedule);
            this.Name = "OAPWeekChannelJumpControl";
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
            ((System.ComponentModel.ISupportInitialize)(this.gridEX2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dvReport)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.oapWeekSummaryRptDs)).EndInit();
            this.panMenuSchedule.ResumeLayout(false);
            this.ResumeLayout(false);

        }
		#endregion

		#region ��Ʈ�� �ε�
		private void UserControl_Load(object sender, System.EventArgs e)
		{
			// ��Ʈ�� �ʱ�ȭ
			InitControl();	
		}

		#endregion

		#region ��Ʈ�� �ʱ�ȭ
		private void InitControl()
		{
			ProgressStart();
			
			InitCombo();

			// ��ȸ���� �˻�
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

		#region �׼�ó�� �޼ҵ�

		/// <summary>
		/// <summary>
		/// ��ȸ��ư Ŭ��
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
			// �Ⱓ������ �� �������� ���Ϸ� ��Ʈ
			cbSearchStartDay.Value = DateTime.Now.AddDays(-1);	
		}

		private void InitCombo_End()
		{
			// �Ⱓ������ �� �������� ���Ϸ� ��Ʈ
			cbSearchEndDay.Value = DateTime.Now.AddDays(-1);	
		}
		
		#endregion

		#region ó���޼ҵ�

		/// <summary>
		/// ī�װ���� ��ȸ
		/// </summary>
		private void SearchReport()
		{
			StatusMessage("ī�װ��� ��ȸ�մϴ�.");
			
			//			if(cbSearchContract.SelectedValue.Equals("00"))
			//			{
			//				MessageBox.Show("�������� ������ �ֽʽÿ�.", "ī�װ����",
			//					MessageBoxButtons.OK, MessageBoxIcon.Information);
			//				return;
			//			}

			//			if(cbSearchItem.SelectedValue.Equals("00"))
			//			{
			//				MessageBox.Show("���� ������ �ֽʽÿ�.", "ī�װ����",
			//					MessageBoxButtons.OK, MessageBoxIcon.Information);
			//				return;
			//			}

			ProgressStart();

			try
			{
				// �����͸� �ʱ�ȭ
				oAPWeekSummaryRptModel.Init();
				
				oAPWeekSummaryRptModel.LogDay1  	 =  cbSearchStartDay.Value.ToString("yyMMdd");
				oAPWeekSummaryRptModel.LogDay2  	 =  cbSearchEndDay.Value.ToString("yyMMdd");
								
				uiPanelList.Text = "�ְ� ä������ ����"; 

				//  ��ü��� ���񽺸� ȣ���Ѵ�.
				new OAPWeekSummaryRptManager(systemModel,commonModel).GetOAPWeekChannelJump(oAPWeekSummaryRptModel);

				if (oAPWeekSummaryRptModel.ResultCD.Equals("0000"))
				{
					Utility.SetDataTable(oapWeekSummaryRptDs.OAPWeekChannelJump, oAPWeekSummaryRptModel.ReportDataSet);													
					StatusMessage(oAPWeekSummaryRptModel.ResultCnt + "���� ��ȸ�Ǿ����ϴ�.");		
		
					keyStartDay   = cbSearchStartDay.Value.ToString("yyyy-MM-dd");
					keyEndDay   = cbSearchEndDay.Value.ToString("yyyy-MM-dd");

					keyReportDay =  keyStartDay + " ~ " + keyEndDay;
					
					//uiPanelList.Text += " / [" + keyItemNo1    + "]" + keyItemName;
					//uiPanelList.Text += " / " + keyContractName + "";
									
					canPrint = true;
				
				}
			}
			catch(FrameException fe)
			{
				FrameSystem.showMsgForm("�ְ� ä������ ��ȸ����", new string[] {fe.ErrCode, fe.ResultMsg});
			}
			catch(Exception ex)
			{
				FrameSystem.showMsgForm("�ְ� ä������ ��ȸ����",new string[] {"",ex.Message});
			}
			finally
			{
				ProgressStop();
			}
		}
			

		#endregion

		#region �̺�Ʈ�Լ�

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

		#region ���� ���
		/// <summary>
		/// ���� ����
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
				int ColMax  = 35; // �÷���   				

				//int TitleRow  = 1;
				//int ConditionRow = 2;
				int HeaderRow = 1;
				int DataRow   = 4;
				string StartCol = "A";
				string EndCol   = "";
				//string TitleCol = "AI";
				int DataCount = 0;
				//int CondCount = 0;
				int HeaderCount = 0;
				
				// ������ �÷��� �ε�������
				EndCol = GetColumnIndex(ColMax);

				xlApp = new Excel.Application();
				oWB = (Excel._Workbook)(xlApp.Workbooks.Add(Missing.Value));
				oSheet = (Excel._Worksheet)oWB.ActiveSheet;

                oRng = oSheet.get_Range(GetColumnIndex(1)+Convert.ToString(HeaderRow), GetColumnIndex(3)+Convert.ToString(HeaderRow));
                oRng.Merge(true);
                oSheet.Cells[HeaderRow,1] = "�ְ�ä����������["  + keyReportDay + "]";

                oRng = oSheet.get_Range(GetColumnIndex(4)+Convert.ToString(HeaderRow), GetColumnIndex(7)+Convert.ToString(HeaderRow));
                oRng.Merge(true);
                oSheet.Cells[HeaderRow,4] = "�ְ�";

                oRng = oSheet.get_Range(GetColumnIndex(8)+Convert.ToString(HeaderRow), GetColumnIndex(11)+Convert.ToString(HeaderRow));
                oRng.Merge(true);
				oSheet.Cells[HeaderRow, 8] = "��";

                oRng = oSheet.get_Range(GetColumnIndex(12)+Convert.ToString(HeaderRow), GetColumnIndex(15)+Convert.ToString(HeaderRow));
                oRng.Merge(true);
                oSheet.Cells[HeaderRow,12] = "ȭ";

                oRng = oSheet.get_Range(GetColumnIndex(16)+Convert.ToString(HeaderRow), GetColumnIndex(19)+Convert.ToString(HeaderRow));
                oRng.Merge(true);
                oSheet.Cells[HeaderRow,16] = "��";

                oRng = oSheet.get_Range(GetColumnIndex(20)+Convert.ToString(HeaderRow), GetColumnIndex(23)+Convert.ToString(HeaderRow));
                oRng.Merge(true);
                oSheet.Cells[HeaderRow,20] = "��";

                oRng = oSheet.get_Range(GetColumnIndex(24)+Convert.ToString(HeaderRow), GetColumnIndex(27)+Convert.ToString(HeaderRow));
                oRng.Merge(true);
                oSheet.Cells[HeaderRow,24] = "��";

                oRng = oSheet.get_Range(GetColumnIndex(28)+Convert.ToString(HeaderRow), GetColumnIndex(31)+Convert.ToString(HeaderRow));
                oRng.Merge(true);
                oSheet.Cells[HeaderRow,28] = "��";

                oRng = oSheet.get_Range(GetColumnIndex(32)+Convert.ToString(HeaderRow), GetColumnIndex(35)+Convert.ToString(HeaderRow));
                oRng.Merge(true);
                oSheet.Cells[HeaderRow,32] = "��";
		
				HeaderCount = 1;						
				oSheet.Cells[HeaderRow + 1, HeaderCount++] = "��ȣ";
				oSheet.Cells[HeaderRow + 1, HeaderCount++] = "�����";
				oSheet.Cells[HeaderRow + 1, HeaderCount++] = "���α׷�";
				oSheet.Cells[HeaderRow + 1, HeaderCount++] = "View";
				oSheet.Cells[HeaderRow + 1, HeaderCount++] = "Hit";
				oSheet.Cells[HeaderRow + 1, HeaderCount++] = "Jump";
				oSheet.Cells[HeaderRow + 1, HeaderCount++] = "Rate";
				oSheet.Cells[HeaderRow + 1, HeaderCount++] = "View";
				oSheet.Cells[HeaderRow + 1, HeaderCount++] = "Hit";
				oSheet.Cells[HeaderRow + 1, HeaderCount++] = "Jump";
				oSheet.Cells[HeaderRow + 1, HeaderCount++] = "Rate";
				oSheet.Cells[HeaderRow + 1, HeaderCount++] = "View";
				oSheet.Cells[HeaderRow + 1, HeaderCount++] = "Hit";
				oSheet.Cells[HeaderRow + 1, HeaderCount++] = "Jump";
				oSheet.Cells[HeaderRow + 1, HeaderCount++] = "Rate";
				oSheet.Cells[HeaderRow + 1, HeaderCount++] = "View";
				oSheet.Cells[HeaderRow + 1, HeaderCount++] = "Hit";
				oSheet.Cells[HeaderRow + 1, HeaderCount++] = "Jump";
				oSheet.Cells[HeaderRow + 1, HeaderCount++] = "Rate";
				oSheet.Cells[HeaderRow + 1, HeaderCount++] = "View";
				oSheet.Cells[HeaderRow + 1, HeaderCount++] = "Hit";
				oSheet.Cells[HeaderRow + 1, HeaderCount++] = "Jump";
				oSheet.Cells[HeaderRow + 1, HeaderCount++] = "Rate";
				oSheet.Cells[HeaderRow + 1, HeaderCount++] = "View";
				oSheet.Cells[HeaderRow + 1, HeaderCount++] = "Hit";
				oSheet.Cells[HeaderRow + 1, HeaderCount++] = "Jump";
				oSheet.Cells[HeaderRow + 1, HeaderCount++] = "Rate";
				oSheet.Cells[HeaderRow + 1, HeaderCount++] = "View";
				oSheet.Cells[HeaderRow + 1, HeaderCount++] = "Hit";
				oSheet.Cells[HeaderRow + 1, HeaderCount++] = "Jump";
				oSheet.Cells[HeaderRow + 1, HeaderCount++] = "Rate";
				oSheet.Cells[HeaderRow + 1, HeaderCount++] = "View";
				oSheet.Cells[HeaderRow + 1, HeaderCount++] = "Hit";
				oSheet.Cells[HeaderRow + 1, HeaderCount++] = "Jump";
				oSheet.Cells[HeaderRow + 1, HeaderCount++] = "Rate";
								
				oRng = oSheet.get_Range(StartCol+Convert.ToString(HeaderRow), EndCol+Convert.ToString(HeaderRow+1));	// ����� ����
				oRng.Font.Bold           = true;							// ��Ʈ ����
                oRng.Font.Size          = 10;
				oRng.VerticalAlignment   = Excel.XlVAlign.xlVAlignCenter;	// �����߾�����
				oRng.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;	// �����߾�����	 
				oRng.Interior.Color      = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.CornflowerBlue);   //�� ���� 
				oRng.Font.Color          = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.White);			//�ؽ�Ʈ��			
				
				DataCount = 0;
				// ������ ����
				for (int inx =0; inx < oapWeekSummaryRptDs.OAPWeekChannelJump.Rows.Count; inx++)
				{		
					DataRow Row = oapWeekSummaryRptDs.OAPWeekChannelJump.Rows[inx];

					int ColCnt = 1;

                    if( Row["ProgName"].ToString() == "�հ�" )
                    {
                        oRng = oSheet.get_Range(GetColumnIndex(1)+Convert.ToString(DataRow+DataCount), GetColumnIndex(35)+Convert.ToString(DataRow+DataCount));
                        oRng.Interior.Color      = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Chartreuse);   //�� ���� 
                        
                        // ����κ� ����
                        oRng = oSheet.get_Range(GetColumnIndex(1)+Convert.ToString(DataRow+DataCount), GetColumnIndex(3)+Convert.ToString(DataRow+DataCount));
                        oRng.Merge(true);

                        oRng = oSheet.get_Range(GetColumnIndex(1)+Convert.ToString(DataRow+DataCount), GetColumnIndex(35)+Convert.ToString(DataRow+DataCount));
                        oRng.Font.Bold  = true;							// ��Ʈ ����
                        oRng.Font.Size  = 9;
											
                        oSheet.Cells[DataRow+DataCount,1] = "�հ�";

                        ColCnt = 4;
                        oSheet.Cells[DataRow + DataCount, ColCnt++] = Row["d0"].ToString();
						oSheet.Cells[DataRow + DataCount, ColCnt++] = Row["t0"].ToString();
                        oSheet.Cells[DataRow + DataCount, ColCnt++] = Row["w0"].ToString();
                        oSheet.Cells[DataRow + DataCount, ColCnt++] = Convert.ToDecimal(Row["r0"].ToString());
                        oSheet.Cells[DataRow + DataCount, ColCnt++] = Row["d2"].ToString();
						oSheet.Cells[DataRow + DataCount, ColCnt++] = Row["t2"].ToString();
                        oSheet.Cells[DataRow + DataCount, ColCnt++] = Row["w2"].ToString();
                        oSheet.Cells[DataRow + DataCount, ColCnt++] = Convert.ToDecimal(Row["r2"].ToString());
                        oSheet.Cells[DataRow + DataCount, ColCnt++] = Row["d3"].ToString();
						oSheet.Cells[DataRow + DataCount, ColCnt++] = Row["t3"].ToString();
                        oSheet.Cells[DataRow + DataCount, ColCnt++] = Row["w3"].ToString();
                        oSheet.Cells[DataRow + DataCount, ColCnt++] = Convert.ToDecimal(Row["r3"].ToString());
                        oSheet.Cells[DataRow + DataCount, ColCnt++] = Row["d4"].ToString();
						oSheet.Cells[DataRow + DataCount, ColCnt++] = Row["t4"].ToString();
                        oSheet.Cells[DataRow + DataCount, ColCnt++] = Row["w4"].ToString();
                        oSheet.Cells[DataRow + DataCount, ColCnt++] = Convert.ToDecimal(Row["r4"].ToString());
                        oSheet.Cells[DataRow + DataCount, ColCnt++] = Row["d5"].ToString();
						oSheet.Cells[DataRow + DataCount, ColCnt++] = Row["t5"].ToString();
                        oSheet.Cells[DataRow + DataCount, ColCnt++] = Row["w5"].ToString();
                        oSheet.Cells[DataRow + DataCount, ColCnt++] = Convert.ToDecimal(Row["r5"].ToString());
                        oSheet.Cells[DataRow + DataCount, ColCnt++] = Row["d6"].ToString();
						oSheet.Cells[DataRow + DataCount, ColCnt++] = Row["t6"].ToString();
                        oSheet.Cells[DataRow + DataCount, ColCnt++] = Row["w6"].ToString();
                        oSheet.Cells[DataRow + DataCount, ColCnt++] = Convert.ToDecimal(Row["r6"].ToString());
                        oSheet.Cells[DataRow + DataCount, ColCnt++] = Row["d7"].ToString();
						oSheet.Cells[DataRow + DataCount, ColCnt++] = Row["t7"].ToString();
                        oSheet.Cells[DataRow + DataCount, ColCnt++] = Row["w7"].ToString();
                        oSheet.Cells[DataRow + DataCount, ColCnt++] = Convert.ToDecimal(Row["r7"].ToString());
                        oSheet.Cells[DataRow + DataCount, ColCnt++] = Row["d1"].ToString();
						oSheet.Cells[DataRow + DataCount, ColCnt++] = Row["t1"].ToString();
                        oSheet.Cells[DataRow + DataCount, ColCnt++] = Row["w1"].ToString();
                        oSheet.Cells[DataRow + DataCount, ColCnt++] = Convert.ToDecimal(Row["r1"].ToString());
                    }
                    else
                    {
                        oSheet.Cells[DataRow + DataCount, ColCnt++] = Row["ItemNo"].ToString();
                        oSheet.Cells[DataRow + DataCount, ColCnt++] = Row["AdName"].ToString();
                        oSheet.Cells[DataRow + DataCount, ColCnt++] = Row["ProgName"].ToString();
                        oSheet.Cells[DataRow + DataCount, ColCnt++] = Row["d0"].ToString();
						oSheet.Cells[DataRow + DataCount, ColCnt++] = Row["t0"].ToString();
                        oSheet.Cells[DataRow + DataCount, ColCnt++] = Row["w0"].ToString();
                        oSheet.Cells[DataRow + DataCount, ColCnt++] = Convert.ToDecimal(Row["r0"].ToString());
                        oSheet.Cells[DataRow + DataCount, ColCnt++] = Row["d2"].ToString();
						oSheet.Cells[DataRow + DataCount, ColCnt++] = Row["t2"].ToString();
                        oSheet.Cells[DataRow + DataCount, ColCnt++] = Row["w2"].ToString();
                        oSheet.Cells[DataRow + DataCount, ColCnt++] = Convert.ToDecimal(Row["r2"].ToString());
                        oSheet.Cells[DataRow + DataCount, ColCnt++] = Row["d3"].ToString();
						oSheet.Cells[DataRow + DataCount, ColCnt++] = Row["t3"].ToString();
                        oSheet.Cells[DataRow + DataCount, ColCnt++] = Row["w3"].ToString();
                        oSheet.Cells[DataRow + DataCount, ColCnt++] = Convert.ToDecimal(Row["r3"].ToString());
                        oSheet.Cells[DataRow + DataCount, ColCnt++] = Row["d4"].ToString();
						oSheet.Cells[DataRow + DataCount, ColCnt++] = Row["t4"].ToString();
                        oSheet.Cells[DataRow + DataCount, ColCnt++] = Row["w4"].ToString();
                        oSheet.Cells[DataRow + DataCount, ColCnt++] = Convert.ToDecimal(Row["r4"].ToString());
                        oSheet.Cells[DataRow + DataCount, ColCnt++] = Row["d5"].ToString();
						oSheet.Cells[DataRow + DataCount, ColCnt++] = Row["t5"].ToString();
                        oSheet.Cells[DataRow + DataCount, ColCnt++] = Row["w5"].ToString();
                        oSheet.Cells[DataRow + DataCount, ColCnt++] = Convert.ToDecimal(Row["r5"].ToString());
                        oSheet.Cells[DataRow + DataCount, ColCnt++] = Row["d6"].ToString();
						oSheet.Cells[DataRow + DataCount, ColCnt++] = Row["t6"].ToString();
                        oSheet.Cells[DataRow + DataCount, ColCnt++] = Row["w6"].ToString();
                        oSheet.Cells[DataRow + DataCount, ColCnt++] = Convert.ToDecimal(Row["r6"].ToString());
                        oSheet.Cells[DataRow + DataCount, ColCnt++] = Row["d7"].ToString();
						oSheet.Cells[DataRow + DataCount, ColCnt++] = Row["t7"].ToString();
                        oSheet.Cells[DataRow + DataCount, ColCnt++] = Row["w7"].ToString();
                        oSheet.Cells[DataRow + DataCount, ColCnt++] = Convert.ToDecimal(Row["r7"].ToString());
                        oSheet.Cells[DataRow + DataCount, ColCnt++] = Row["d1"].ToString();
						oSheet.Cells[DataRow + DataCount, ColCnt++] = Row["t1"].ToString();
                        oSheet.Cells[DataRow + DataCount, ColCnt++] = Row["w1"].ToString();
                        oSheet.Cells[DataRow + DataCount, ColCnt++] = Convert.ToDecimal(Row["r1"].ToString());
                    }
					                    									
					DataCount++;
				}

				DataCount--;

				// ������ �ۼ�
				oRng = oSheet.get_Range(StartCol+Convert.ToString(HeaderRow), EndCol+Convert.ToString(DataRow+DataCount));	// �������� ����
                oRng.Font.Size = 8;
				oRng.EntireColumn.AutoFit();					// �������� ũ�⿡ ���� ����ũ�� ����
				oRng.Borders.LineStyle =  Excel.XlLineStyle.xlContinuous;	// �׵θ��Ӽ� �Ʒ��� �Ǽ�
				oRng.Borders.Weight    = Excel.XlBorderWeight.xlThin;		// �׵θ��Ӽ� �Ʒ��� ���¼�
				//�����ȣ �߾�����
				oRng = oSheet.get_Range(GetColumnIndex(1)+Convert.ToString(DataRow), GetColumnIndex(1)+Convert.ToString(DataRow+DataCount)); 
				oRng.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
				
				//�����ȣ �߾�����
				oRng = oSheet.get_Range(GetColumnIndex(2)+Convert.ToString(DataRow), GetColumnIndex(2)+Convert.ToString(DataRow+DataCount)); 
				oRng.HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft;
				
								
				//��ûȽ�� right����
				oRng = oSheet.get_Range(GetColumnIndex(3)+Convert.ToString(DataRow), GetColumnIndex(3)+Convert.ToString(DataRow+DataCount)); 
				oRng.HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft;
				
				//��û���� right����
				oRng = oSheet.get_Range(GetColumnIndex(4)+Convert.ToString(DataRow), GetColumnIndex(4)+Convert.ToString(DataRow+DataCount)); 
				oRng.HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;
				oRng.NumberFormatLocal = "#,##0";

				//rate right����
				oRng = oSheet.get_Range(GetColumnIndex(5)+Convert.ToString(DataRow), GetColumnIndex(5)+Convert.ToString(DataRow+DataCount)); 
				oRng.HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;
				oRng.NumberFormatLocal = "#,##0";

				//Cove right����
				oRng = oSheet.get_Range(GetColumnIndex(6)+Convert.ToString(DataRow), GetColumnIndex(6)+Convert.ToString(DataRow+DataCount)); 
				oRng.HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;
				oRng.NumberFormatLocal = "#,##0";

				//Freq right����
				oRng = oSheet.get_Range(GetColumnIndex(7)+Convert.ToString(DataRow), GetColumnIndex(7)+Convert.ToString(DataRow+DataCount)); 
				oRng.HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;
				oRng.NumberFormatLocal = "#,##0.00";

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
				oRng.NumberFormatLocal = "#,##0.00";

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
				oRng.NumberFormatLocal = "#,##0.00";

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
				oRng.NumberFormatLocal = "#,##0.00";

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
				oRng.NumberFormatLocal = "#,##0.00";

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
				oRng.NumberFormatLocal = "#,##0.00";

				oRng = oSheet.get_Range(GetColumnIndex(28) + Convert.ToString(DataRow), GetColumnIndex(28) + Convert.ToString(DataRow + DataCount));
				oRng.HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;
				oRng.NumberFormatLocal = "#,##0";

				oRng = oSheet.get_Range(GetColumnIndex(29) + Convert.ToString(DataRow), GetColumnIndex(29) + Convert.ToString(DataRow + DataCount));
				oRng.HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;
				oRng.NumberFormatLocal = "#,##0";

				oRng = oSheet.get_Range(GetColumnIndex(30) + Convert.ToString(DataRow), GetColumnIndex(30) + Convert.ToString(DataRow + DataCount));
				oRng.HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;
				oRng.NumberFormatLocal = "#,##0";

				oRng = oSheet.get_Range(GetColumnIndex(31) + Convert.ToString(DataRow), GetColumnIndex(31) + Convert.ToString(DataRow + DataCount));
				oRng.HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;
				oRng.NumberFormatLocal = "#,##0.00";

				oRng = oSheet.get_Range(GetColumnIndex(32) + Convert.ToString(DataRow), GetColumnIndex(32) + Convert.ToString(DataRow + DataCount));
				oRng.HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;
				oRng.NumberFormatLocal = "#,##0";

				oRng = oSheet.get_Range(GetColumnIndex(33) + Convert.ToString(DataRow), GetColumnIndex(33) + Convert.ToString(DataRow + DataCount));
				oRng.HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;
				oRng.NumberFormatLocal = "#,##0";

				oRng = oSheet.get_Range(GetColumnIndex(34) + Convert.ToString(DataRow), GetColumnIndex(34) + Convert.ToString(DataRow + DataCount));
				oRng.HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;
				oRng.NumberFormatLocal = "#,##0";

				oRng = oSheet.get_Range(GetColumnIndex(35) + Convert.ToString(DataRow), GetColumnIndex(35) + Convert.ToString(DataRow + DataCount));
				oRng.HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;
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

			// 26���� ũ��
			if(ColCount > ColName.Length)
			{
				// 2�ڸ� �ε������� 26 => Z;  27->AA
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
			cbSearchEndDay.Value  = cbSearchStartDay.Value.AddDays(6);
		}

		private void cbSearchEndDay_TextChanged(object sender, System.EventArgs e)
		{
			if(cbSearchEndDay.Value > cbSearchStartDay.Value.AddDays(6))
			{
				MessageBox.Show("������������ �����Ϸκ��� �������� ������ �����ϴ�.\nȮ�� �Ͻñ� �ٶ��ϴ�.", "OAP�ְ� ä������ ���",MessageBoxButtons.OK, MessageBoxIcon.Information);
				cbSearchEndDay.Value  = cbSearchStartDay.Value.AddDays(6);
			}			
		}

        private void gridEX2_LoadingRow(object sender, Janus.Windows.GridEX.RowLoadEventArgs e)
        {
            if( e.Row.Cells["ItemNo"].Value.Equals("999999") )
            {
                // �ش� ���� ����Ǹ� Row���� ������ �۵����� ����
                e.Row.BeginEdit();
                e.Row.Cells["ItemNo"].Value = "";
                e.Row.Cells["ProgName"].Value = "�հ�";
                e.Row.EndEdit();
            }
        }
	}
}
