// ===============================================================================
//
// DateAdTypeSummaryRptControl.cs
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
using AdManagerClient.Common;

using AdManagerModel;
using Excel = Microsoft.Office.Interop.Excel; // ���� ����
using System.Reflection;

namespace AdManagerClient
{
	/// <summary>
	/// ���� ��û��Ȳ ���� ���� ��Ʈ��
	/// </summary>
    public class DateAdTypeSummaryRptControl : System.Windows.Forms.UserControl, IUserControl
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
		DateAdTypeSummaryRptModel dateAdTypeSummaryRptModel  = new DateAdTypeSummaryRptModel();	// ���α׷��� �����û�����
		SummaryAdModel summaryAdModel  = new SummaryAdModel();	// ���α׷��� �����û�����

		// ȭ��ó���� ����
		bool canRead			  = false;

		// Key ������		
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

		string keyStartDay = "";
		string keyEndDay = "";
		string keyReportDay = "";
		private int searchType = 0;	//�˻������� �����ϱ� ���� ���� 1-�Ϻ�, 2-�ֺ�, 3-���� (�������� ����� �Ѵ�)

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
		private Janus.Windows.UI.Dock.UIPanelManager uiPM;
		private Janus.Windows.CalendarCombo.CalendarCombo cbSearchStartDay;
		private System.Windows.Forms.Label lbSearchDate;
		private System.Windows.Forms.Label label10;
		private Janus.Windows.CalendarCombo.CalendarCombo cbSearchEndDay;
		private System.Windows.Forms.Label label11;
		private Janus.Windows.GridEX.GridEX gridEX1;
		private Janus.Windows.EditControls.UIRadioButton rbSearchType_W;
		private Janus.Windows.EditControls.UIRadioButton rbSearchType_M;
		private Janus.Windows.EditControls.UIRadioButton rbSearchType_D;
		private System.Data.DataView dvReport;
		private AdManagerClient._51_ReportSummaryAd._07_DateAdTypeSummaryRpt.DateAdTypeSummaryRptDs dateAdTypeSummaryRptDs;
		private System.ComponentModel.IContainer components;

		public DateAdTypeSummaryRptControl()
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
            Janus.Windows.GridEX.GridEXLayout gridEX1_DesignTimeLayout = new Janus.Windows.GridEX.GridEXLayout();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DateAdTypeSummaryRptControl));
            this.uiPM = new Janus.Windows.UI.Dock.UIPanelManager(this.components);
            this.uiPanelChoiceAdSchedule = new Janus.Windows.UI.Dock.UIPanelGroup();
            this.uiPanelSearch = new Janus.Windows.UI.Dock.UIPanel();
            this.uiPanelSearchContainer = new Janus.Windows.UI.Dock.UIPanelInnerContainer();
            this.pnlSearch = new System.Windows.Forms.Panel();
            this.rbSearchType_W = new Janus.Windows.EditControls.UIRadioButton();
            this.rbSearchType_M = new Janus.Windows.EditControls.UIRadioButton();
            this.rbSearchType_D = new Janus.Windows.EditControls.UIRadioButton();
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
            this.dateAdTypeSummaryRptDs = new AdManagerClient._51_ReportSummaryAd._07_DateAdTypeSummaryRpt.DateAdTypeSummaryRptDs();
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
            ((System.ComponentModel.ISupportInitialize)(this.dateAdTypeSummaryRptDs)).BeginInit();
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
            this.uiPM.AddDockPanelInfo(new System.Guid("afb3b7d0-aa79-443c-a1e8-3baba38f7a4d"), new System.Guid("b162c53a-7940-47cd-a0ed-9762c59c50d8"), 30, true);
            this.uiPM.AddDockPanelInfo(new System.Guid("8e3140b9-bfcb-43af-b84e-2ee6cddf51dc"), new System.Guid("b162c53a-7940-47cd-a0ed-9762c59c50d8"), 601, true);
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
            this.uiPanelChoiceAdSchedule.Text = "���������� ���";
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
            this.pnlSearch.Controls.Add(this.rbSearchType_W);
            this.pnlSearch.Controls.Add(this.rbSearchType_M);
            this.pnlSearch.Controls.Add(this.rbSearchType_D);
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
            // rbSearchType_W
            // 
            this.rbSearchType_W.Location = new System.Drawing.Point(72, 8);
            this.rbSearchType_W.Name = "rbSearchType_W";
            this.rbSearchType_W.ShowFocusRectangle = false;
            this.rbSearchType_W.Size = new System.Drawing.Size(48, 23);
            this.rbSearchType_W.TabIndex = 34;
            this.rbSearchType_W.Text = "�ֺ�";
            this.rbSearchType_W.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.rbSearchType_W.CheckedChanged += new System.EventHandler(this.rbSearchType_CheckedChanged);
            // 
            // rbSearchType_M
            // 
            this.rbSearchType_M.Location = new System.Drawing.Point(136, 8);
            this.rbSearchType_M.Name = "rbSearchType_M";
            this.rbSearchType_M.ShowFocusRectangle = false;
            this.rbSearchType_M.Size = new System.Drawing.Size(48, 23);
            this.rbSearchType_M.TabIndex = 35;
            this.rbSearchType_M.Text = "����";
            this.rbSearchType_M.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.rbSearchType_M.CheckedChanged += new System.EventHandler(this.rbSearchType_CheckedChanged);
            // 
            // rbSearchType_D
            // 
            this.rbSearchType_D.Checked = true;
            this.rbSearchType_D.Location = new System.Drawing.Point(8, 8);
            this.rbSearchType_D.Name = "rbSearchType_D";
            this.rbSearchType_D.ShowFocusRectangle = false;
            this.rbSearchType_D.Size = new System.Drawing.Size(56, 23);
            this.rbSearchType_D.TabIndex = 33;
            this.rbSearchType_D.TabStop = true;
            this.rbSearchType_D.Text = "�Ϻ�";
            this.rbSearchType_D.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.rbSearchType_D.CheckedChanged += new System.EventHandler(this.rbSearchType_CheckedChanged);
            // 
            // cbSearchStartDay
            // 
            // 
            // 
            // 
            this.cbSearchStartDay.DropDownCalendar.FirstMonth = new System.DateTime(2007, 8, 1, 0, 0, 0, 0);
            this.cbSearchStartDay.DropDownCalendar.Name = "";
            this.cbSearchStartDay.DropDownCalendar.VisualStyle = Janus.Windows.CalendarCombo.VisualStyle.Office2007;
            this.cbSearchStartDay.Location = new System.Drawing.Point(288, 8);
            this.cbSearchStartDay.Name = "cbSearchStartDay";
            this.cbSearchStartDay.Size = new System.Drawing.Size(104, 23);
            this.cbSearchStartDay.TabIndex = 5;
            this.cbSearchStartDay.Value = new System.DateTime(2007, 9, 14, 0, 0, 0, 0);
            this.cbSearchStartDay.VisualStyle = Janus.Windows.CalendarCombo.VisualStyle.Office2007;
            this.cbSearchStartDay.TextChanged += new System.EventHandler(this.rbSearchType_CheckedChanged);
            // 
            // lbSearchDate
            // 
            this.lbSearchDate.Location = new System.Drawing.Point(216, 10);
            this.lbSearchDate.Name = "lbSearchDate";
            this.lbSearchDate.Size = new System.Drawing.Size(72, 21);
            this.lbSearchDate.TabIndex = 32;
            this.lbSearchDate.Text = "���������";
            this.lbSearchDate.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label10
            // 
            this.label10.Location = new System.Drawing.Point(392, 10);
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
            this.cbSearchEndDay.Location = new System.Drawing.Point(424, 8);
            this.cbSearchEndDay.Name = "cbSearchEndDay";
            this.cbSearchEndDay.Size = new System.Drawing.Size(104, 23);
            this.cbSearchEndDay.TabIndex = 6;
            this.cbSearchEndDay.Value = new System.DateTime(2007, 9, 14, 0, 0, 0, 0);
            this.cbSearchEndDay.VisualStyle = Janus.Windows.CalendarCombo.VisualStyle.Office2007;
            this.cbSearchEndDay.TextChanged += new System.EventHandler(this.rbSearchType_CheckedChanged);
            // 
            // label11
            // 
            this.label11.Location = new System.Drawing.Point(528, 10);
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
            this.uiPanelListContainer.Controls.Add(this.gridEX1);
            this.uiPanelListContainer.Location = new System.Drawing.Point(1, 23);
            this.uiPanelListContainer.Name = "uiPanelListContainer";
            this.uiPanelListContainer.Size = new System.Drawing.Size(1008, 587);
            this.uiPanelListContainer.TabIndex = 0;
            // 
            // gridEX1
            // 
            this.gridEX1.AlternatingColors = true;
            this.gridEX1.BorderStyle = Janus.Windows.GridEX.BorderStyle.None;
            this.gridEX1.DataSource = this.dvReport;
            gridEX1_DesignTimeLayout.LayoutString = resources.GetString("gridEX1_DesignTimeLayout.LayoutString");
            this.gridEX1.DesignTimeLayout = gridEX1_DesignTimeLayout;
            this.gridEX1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridEX1.EditorsControlStyle.ButtonAppearance = Janus.Windows.GridEX.ButtonAppearance.Regular;
            this.gridEX1.EmptyRows = true;
            this.gridEX1.FocusCellFormatStyle.BackColor = System.Drawing.SystemColors.Highlight;
            this.gridEX1.FocusCellFormatStyle.ForeColor = System.Drawing.Color.White;
            this.gridEX1.FocusStyle = Janus.Windows.GridEX.FocusStyle.None;
            this.gridEX1.Font = new System.Drawing.Font("���� ���", 9F);
            this.gridEX1.FrozenColumns = 3;
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
            this.gridEX1.Size = new System.Drawing.Size(1008, 587);
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
            // 
            // dvReport
            // 
            this.dvReport.Table = this.dateAdTypeSummaryRptDs.DateAdTypeSummaryRpt;
            // 
            // dateAdTypeSummaryRptDs
            // 
            this.dateAdTypeSummaryRptDs.DataSetName = "DateAdTypeSummaryRptDs";
            this.dateAdTypeSummaryRptDs.Locale = new System.Globalization.CultureInfo("en-US");
            this.dateAdTypeSummaryRptDs.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
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
            // DateAdTypeSummaryRptControl
            // 
            this.Controls.Add(this.uiPanelChoiceAdSchedule);
            this.Font = new System.Drawing.Font("���� ���", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.Name = "DateAdTypeSummaryRptControl";
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
            ((System.ComponentModel.ISupportInitialize)(this.dateAdTypeSummaryRptDs)).EndInit();
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

			gridEX1.Focus();

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
			DateTime Now = DateTime.Today;
			if(cbSearchStartDay.Value > cbSearchEndDay.Value)
			{
				MessageBox.Show("����������� ���������� ���� Ů�ϴ�.", "�Ϻ����������� ����",
					MessageBoxButtons.OK, MessageBoxIcon.Information);
				return;
			}
			else if(cbSearchEndDay.Value > Now)
			{
				MessageBox.Show("������������ �����Ϻ��� Ŭ���� �����ϴ�.", "�Ϻ����������� ����",
					MessageBoxButtons.OK, MessageBoxIcon.Information);
				return;
			}
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

		/// <summary>
		/// ����� ��¥���� ��ȿ���� üũ�Ѵ�.
		/// </summary>
		public void checkValue()
		{
			if(rbSearchType_D.Checked)
			{
				//�Ϻ��� ���� �Ѵ��� ������ �ȵȴ�.
				if(cbSearchEndDay.Value > cbSearchStartDay.Value.AddMonths(1).AddDays(-1))
				{
					MessageBox.Show("������������ �����Ϸκ��� �Ѵ��� ������ �����ϴ�.\nȮ�� �Ͻñ� �ٶ��ϴ�.", "�Ϻ����������� ����",
						MessageBoxButtons.OK, MessageBoxIcon.Information);			
					cbSearchEndDay.Value  = cbSearchStartDay.Value.AddMonths(1).AddDays(-1);
				}
			}
			else if(rbSearchType_M.Checked)
			{
				//������ ���� �ϳ��� �ѱ�� �ȵȴ�.
				if(cbSearchEndDay.Value > cbSearchStartDay.Value.AddYears(1).AddDays(-1))
				{
					MessageBox.Show("������������ �����Ϸκ��� 1���� ������ �����ϴ�.\nȮ�� �Ͻñ� �ٶ��ϴ�.", "�������������� ����",
						MessageBoxButtons.OK, MessageBoxIcon.Information);
					cbSearchEndDay.Value  = cbSearchStartDay.Value.AddYears(1).AddDays(-1);
				}
			}
			else if(rbSearchType_W.Checked)
			{
				//�ֺ��� ���� �ϳ��� �ѱ�� �ȵȴ�.
				if(cbSearchEndDay.Value > cbSearchStartDay.Value.AddYears(1).AddDays(-1))
				{
					MessageBox.Show("������������ �����Ϸκ��� 1���� ������ �����ϴ�.\nȮ�� �Ͻñ� �ٶ��ϴ�.", "�ֺ����������� ����",
						MessageBoxButtons.OK, MessageBoxIcon.Information);
					cbSearchEndDay.Value  = DateUtil.getLastDayOfWeekForStartMonday(cbSearchStartDay.Value.AddYears(1).AddDays(-7));
				}
			}
		}

		#endregion

		#region ó���޼ҵ�

		/// <summary>
		/// ��ü��� ��ȸ
		/// </summary>
		private void SearchReport()
		{
			StatusMessage("�Ϻ������ ��ȸ�մϴ�.");
			
			//			if(cbSearchContract.SelectedValue.Equals("00"))
			//			{
			//				MessageBox.Show("�������� ������ �ֽʽÿ�.", "�Ϻ����",
			//					MessageBoxButtons.OK, MessageBoxIcon.Information);
			//				return;
			//			}

			//			if(cbSearchItem.SelectedValue.Equals("00"))
			//			{
			//				MessageBox.Show("���� ������ �ֽʽÿ�.", "�Ϻ����",
			//					MessageBoxButtons.OK, MessageBoxIcon.Information);
			//				return;
			//			}

			ProgressStart();

			try
			{
				// �����͸� �ʱ�ȭ
				dateAdTypeSummaryRptModel.Init();
				
				dateAdTypeSummaryRptModel.LogDay1  	 =  cbSearchStartDay.Value.ToString("yyMMdd");
				dateAdTypeSummaryRptModel.LogDay2  	 =  cbSearchEndDay.Value.ToString("yyMMdd");
				
				// ���õ� ������ư�� ���� �ٸ� ���񽺸� ȣ���Ѵ�.
				if(rbSearchType_D.Checked) 
				{
					// �Ϻ��˻�
					new DateAdTypeSummaryRptManager(systemModel,commonModel).GetDateAdTypeSummaryRpt(dateAdTypeSummaryRptModel);
					searchType = 1;
				}
				else if(rbSearchType_W.Checked)
				{
					// �ֺ��˻�
					new DateAdTypeSummaryRptManager(systemModel,commonModel).GetWeeklyAdTypeSummaryRpt(dateAdTypeSummaryRptModel);
					searchType = 2;
				}
				else if(rbSearchType_M.Checked)
				{
					// �����˻�
					new DateAdTypeSummaryRptManager(systemModel,commonModel).GetMonthlyAdTypeSummaryRpt(dateAdTypeSummaryRptModel);
					searchType = 3;
				}

				if (dateAdTypeSummaryRptModel.ResultCD.Equals("0000"))
				{
					Utility.SetDataTable(dateAdTypeSummaryRptDs.DateAdTypeSummaryRpt, dateAdTypeSummaryRptModel.ReportDataSet);													
					StatusMessage(dateAdTypeSummaryRptModel.ResultCnt + "���� ��ȸ�Ǿ����ϴ�.");		
		
					keyStartDay   = cbSearchStartDay.Value.ToString("yyyy-MM-dd");
					keyEndDay   = cbSearchEndDay.Value.ToString("yyyy-MM-dd");

					keyReportDay =  keyStartDay + " ~ " + keyEndDay;
					
					canPrint = true;

					if( dateAdTypeSummaryRptDs.DateAdTypeSummaryRpt.Rows.Count > 0)
					{
						for (int inx =0; inx < dateAdTypeSummaryRptDs.DateAdTypeSummaryRpt.Rows.Count; inx++)
						{		
							DataRow Row = dateAdTypeSummaryRptDs.Tables[0].Rows[inx];

							if( Row != null && (Row["adTypeName"].ToString().Trim().Equals("����") || ((string)Row["adTypeName"])==""))
							{				
								#region ù��° �����͸� ����� �����ϰ�, ����
								//�ϴ� �� ���̰� �Ѵ�.
								foreach( Janus.Windows.GridEX.GridEXColumn Col in gridEX1.RootTable.Columns )
								{
									if( Col.Caption.Equals("") || Col.Caption.Length == 0 )
									{
										Col.Visible = true;
									}
								}

								int MaxColumnCnt = 3;
								if(rbSearchType_D.Checked)
								{
									MaxColumnCnt = 31;
								}
								else if(rbSearchType_W.Checked)
								{
									MaxColumnCnt = 53;
								}
								else if(rbSearchType_M.Checked)
								{
									MaxColumnCnt = 12;
								}

								int HeaderCount = 3;
								for(int i=0; i<MaxColumnCnt; i++)
								{
									gridEX1.RootTable.Columns[HeaderCount+i].Caption = Row[HeaderCount+i].ToString();
								}
								//�����͸� �����Ѵ�.
								try
								{
									Row.BeginEdit();
									Row.Delete();
									Row.AcceptChanges();
								}
								catch(Exception exx)
								{
									Console.WriteLine(exx.StackTrace);
									Console.WriteLine(exx.ToString());
								}
								//����� ���ٸ� �ش� �÷��� ������ �ʰ� �ٲ۴�.
								foreach( Janus.Windows.GridEX.GridEXColumn Col in gridEX1.RootTable.Columns )
								{
									if( Col.Caption.Equals("") || Col.Caption.Length == 0 )
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
			}
			catch(FrameException fe)
			{
				FrameSystem.showMsgForm("�Ϻ���� ��ȸ����", new string[] {fe.ErrCode, fe.ResultMsg});
			}
			catch(Exception ex)
			{
				FrameSystem.showMsgForm("�Ϻ���� ��ȸ����",new string[] {"",ex.Message});
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

		private void cbSearchStartDay_TextChanged(object sender, System.EventArgs e)
		{
			checkValue();
		}

		private void cbSearchEndDay_TextChanged(object sender, System.EventArgs e)
		{
			checkValue();
		}

		private void rbSearchType_CheckedChanged(object sender, System.EventArgs e)
		{
			if(rbSearchType_W.Checked)
			{
				cbSearchStartDay.Value = DateUtil.getFirstDayOfWeekForStartMonday(cbSearchStartDay.Value);
				cbSearchEndDay.Value = DateUtil.getLastDayOfWeekForStartMonday(cbSearchEndDay.Value);
				checkValue();
			}
			else if(rbSearchType_M.Checked)
			{
				cbSearchStartDay.Value = DateUtil.getFirstDayOfMonth(cbSearchStartDay.Value);
				cbSearchEndDay.Value = DateUtil.getLastDayOfMonth(cbSearchEndDay.Value);
				checkValue();
			}
			else if(rbSearchType_D.Checked)
			{
				checkValue();
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

			if(searchType == 0)
			{
				MessageBox.Show("�˻� �� ������ ������ �� �ֽ��ϴ�.");
				return;
			}
			Excel.Application xlApp= null;
			Excel._Workbook   oWB = null;
			Excel._Worksheet  oSheet = null;
			Excel.Range       oRng = null;		
			
			try
			{	
				int ColMax  = 0;
				// �÷����������̹Ƿ� �÷����� ����Ѵ�.(�ִ� 52��)
				for(int i=0; i<52; i++)
				{
					if(!"".Equals(gridEX1.RootTable.Columns["c"+i].Caption.ToString()))
					{
						ColMax++;
					}
				}

				int TitleRow  = 1;
				int ConditionRow = 2;
				int HeaderRow = 4;
				int DataRow   = 5;
				string StartCol = "A";
				string EndCol   = "";
				string TitleCol = "D";
				int DataCount = 0;
				int CondCount = 0;
				int HeaderCount = 0;
				
				// ������ �÷��� �ε�������
				EndCol = GetColumnIndex(ColMax+1);

				xlApp = new Excel.Application();
				oWB = (Excel._Workbook)(xlApp.Workbooks.Add(Missing.Value));
				oSheet = (Excel._Worksheet)oWB.ActiveSheet;


				// Ÿ��Ʋ �ۼ�
				if(searchType == 1)
				{
					oSheet.Cells[TitleRow,1] = "�Ϻ� ���������� ����";
				}
				else if(searchType == 2)
				{
					oSheet.Cells[TitleRow,1] = "�ֺ� ���������� ����";
				}
				else if(searchType == 3)
				{
					oSheet.Cells[TitleRow,1] = "���� ���������� ����";
				}

				oRng = oSheet.get_Range(StartCol+Convert.ToString(TitleRow), TitleCol+Convert.ToString(TitleRow));
				oRng.Merge(true);
				oRng.Font.Bold = true;
				oRng.Font.Size = 16;
				oRng.VerticalAlignment   = Excel.XlVAlign.xlVAlignCenter;			
				oRng.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;		

				// �������� �ۼ�
				oSheet.Cells[ConditionRow+CondCount,1] = "���������";
				oRng = oSheet.get_Range("B"+Convert.ToString(ConditionRow+CondCount), TitleCol+Convert.ToString(ConditionRow+CondCount));
				oRng.Merge(true);
				oSheet.Cells[ConditionRow+CondCount,2] = keyReportDay;
				CondCount++;

				// ���Ǻ� �׵θ�
				oRng = oSheet.get_Range(StartCol+Convert.ToString(ConditionRow), TitleCol+Convert.ToString(ConditionRow+(CondCount-1)));
				oRng.Font.Size = 10;
				oRng.Font.Bold = true;
				oRng.VerticalAlignment   = Excel.XlVAlign.xlVAlignCenter;			
				oRng.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;		
				oRng.Borders.LineStyle =  Excel.XlLineStyle.xlContinuous;	// �׵θ��Ӽ� �Ʒ��� �Ǽ�
				oRng.Borders.Weight    = Excel.XlBorderWeight.xlThin;		// �׵θ��Ӽ� �Ʒ��� ���¼�


				// ��� ����
				HeaderCount = 1;						
				oSheet.Cells[HeaderRow,HeaderCount++] = gridEX1.RootTable.Columns["adTypeName"].Caption.ToString();
				for(int i=0; i<ColMax; i++)
				{
					if(!"".Equals(gridEX1.RootTable.Columns["c"+i].Caption.ToString()))
					{
						oSheet.Cells[HeaderRow,HeaderCount++] = gridEX1.RootTable.Columns["c"+i].Caption.ToString();
					}
				}

				
				oRng = oSheet.get_Range(StartCol+Convert.ToString(HeaderRow), EndCol+Convert.ToString(HeaderRow));	// ����� ����
				oRng.Font.Bold           = true;							// ��Ʈ ����
				oRng.Font.Size          = 10;
				oRng.VerticalAlignment   = Excel.XlVAlign.xlVAlignCenter;	// �����߾�����
				oRng.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;	// �����߾�����	 
				oRng.Interior.Color      = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.CornflowerBlue);   //�� ���� 
				oRng.Font.Color          = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.White);			//�ؽ�Ʈ��			
				
				DataCount = 0;
				// ������ ����
				for (int inx =0; inx < dateAdTypeSummaryRptDs.DateAdTypeSummaryRpt.Rows.Count; inx++)
				{		
					DataRow Row = dateAdTypeSummaryRptDs.DateAdTypeSummaryRpt.Rows[inx];

					int ColCnt = 1;
					if(!Row["adTypeCode"].ToString().Equals("0"))
					{								
						//oSheet.Cells[DataRow+DataCount,ColCnt++] = Row["gubun"].ToString();
						oSheet.Cells[DataRow+DataCount,ColCnt++] = Row["adTypeName"].ToString();
						for(int i=0; i<ColMax; i++)
						{
							if(!"".Equals(gridEX1.RootTable.Columns["c"+i].Caption.ToString()))
							{
								oSheet.Cells[DataRow+DataCount,ColCnt++] = Row["c"+i].ToString();
							}
						}
					}
									
					DataCount++;
				}

				DataCount--;

				// ������ �ۼ�
				oRng = oSheet.get_Range(StartCol+Convert.ToString(HeaderRow), EndCol+Convert.ToString(DataRow+DataCount));	// �������� ����
				oRng.Font.Size = 10;
				oRng.EntireColumn.AutoFit();					// �������� ũ�⿡ ���� ����ũ�� ����
				oRng.Borders.LineStyle =  Excel.XlLineStyle.xlContinuous;	// �׵θ��Ӽ� �Ʒ��� �Ǽ�
				oRng.Borders.Weight    = Excel.XlBorderWeight.xlThin;		// �׵θ��Ӽ� �Ʒ��� ���¼�
				//�����ȣ �߾�����
				oRng = oSheet.get_Range(GetColumnIndex(1)+Convert.ToString(DataRow), GetColumnIndex(1)+Convert.ToString(DataRow+DataCount)); 
				oRng.HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft;
			
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

	}

}
