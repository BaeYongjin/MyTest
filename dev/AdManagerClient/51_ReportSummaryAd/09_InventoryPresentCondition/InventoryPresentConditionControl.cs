// ===============================================================================
//
// InventoryPresentConditionControl.cs
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
    public class InventoryPresentConditionControl : System.Windows.Forms.UserControl, IUserControl
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
		InventoryPresentConditionModel inventoryPresentConditionModel  = new InventoryPresentConditionModel();	// �κ��丮��Ȳ��

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
		private string SearchType = "";	//�˻������� �����ϱ� ���� ���� 30�� �������� �ƴ��� �̷� �͵��� üũ�Ѵ�.

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
		private System.Data.DataView dvReport;
		private AdManagerClient._51_ReportSummaryAd._09_InventoryPresentCondition.InventoryPresentConditionDs inventoryPresentConditionDs;
		private Janus.Windows.EditControls.UIButton btnMakeData;
		private System.Windows.Forms.RadioButton rb3Month;
		private System.Windows.Forms.RadioButton rbYear;
		private System.Windows.Forms.RadioButton rbPreMon;
		private System.ComponentModel.IContainer components;

		public InventoryPresentConditionControl()
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(InventoryPresentConditionControl));
            this.uiPM = new Janus.Windows.UI.Dock.UIPanelManager(this.components);
            this.uiPanelChoiceAdSchedule = new Janus.Windows.UI.Dock.UIPanelGroup();
            this.uiPanelSearch = new Janus.Windows.UI.Dock.UIPanel();
            this.uiPanelSearchContainer = new Janus.Windows.UI.Dock.UIPanelInnerContainer();
            this.pnlSearch = new System.Windows.Forms.Panel();
            this.rbPreMon = new System.Windows.Forms.RadioButton();
            this.rbYear = new System.Windows.Forms.RadioButton();
            this.rb3Month = new System.Windows.Forms.RadioButton();
            this.cbSearchStartDay = new Janus.Windows.CalendarCombo.CalendarCombo();
            this.lbSearchDate = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.cbSearchEndDay = new Janus.Windows.CalendarCombo.CalendarCombo();
            this.label11 = new System.Windows.Forms.Label();
            this.btnSearch = new Janus.Windows.EditControls.UIButton();
            this.btnExcel = new Janus.Windows.EditControls.UIButton();
            this.btnMakeData = new Janus.Windows.EditControls.UIButton();
            this.uiPanelList = new Janus.Windows.UI.Dock.UIPanel();
            this.uiPanelListContainer = new Janus.Windows.UI.Dock.UIPanelInnerContainer();
            this.gridEX1 = new Janus.Windows.GridEX.GridEX();
            this.dvReport = new System.Data.DataView();
            this.inventoryPresentConditionDs = new AdManagerClient._51_ReportSummaryAd._09_InventoryPresentCondition.InventoryPresentConditionDs();
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
            ((System.ComponentModel.ISupportInitialize)(this.inventoryPresentConditionDs)).BeginInit();
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
            this.uiPanelChoiceAdSchedule.Text = "�κ��丮��Ȳ";
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
            this.pnlSearch.Controls.Add(this.rbPreMon);
            this.pnlSearch.Controls.Add(this.rbYear);
            this.pnlSearch.Controls.Add(this.rb3Month);
            this.pnlSearch.Controls.Add(this.cbSearchStartDay);
            this.pnlSearch.Controls.Add(this.lbSearchDate);
            this.pnlSearch.Controls.Add(this.label10);
            this.pnlSearch.Controls.Add(this.cbSearchEndDay);
            this.pnlSearch.Controls.Add(this.label11);
            this.pnlSearch.Controls.Add(this.btnSearch);
            this.pnlSearch.Controls.Add(this.btnExcel);
            this.pnlSearch.Controls.Add(this.btnMakeData);
            this.pnlSearch.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlSearch.Location = new System.Drawing.Point(0, 0);
            this.pnlSearch.Name = "pnlSearch";
            this.pnlSearch.Size = new System.Drawing.Size(1008, 38);
            this.pnlSearch.TabIndex = 3;
            // 
            // rbPreMon
            // 
            this.rbPreMon.Location = new System.Drawing.Point(480, 12);
            this.rbPreMon.Name = "rbPreMon";
            this.rbPreMon.Size = new System.Drawing.Size(72, 20);
            this.rbPreMon.TabIndex = 35;
            this.rbPreMon.Text = "��������";
            // 
            // rbYear
            // 
            this.rbYear.Location = new System.Drawing.Point(404, 9);
            this.rbYear.Name = "rbYear";
            this.rbYear.Size = new System.Drawing.Size(74, 24);
            this.rbYear.TabIndex = 34;
            this.rbYear.Text = "1������";
            // 
            // rb3Month
            // 
            this.rb3Month.Checked = true;
            this.rb3Month.Location = new System.Drawing.Point(316, 8);
            this.rb3Month.Name = "rb3Month";
            this.rb3Month.Size = new System.Drawing.Size(85, 24);
            this.rb3Month.TabIndex = 33;
            this.rb3Month.TabStop = true;
            this.rb3Month.Text = "3�������";
            // 
            // cbSearchStartDay
            // 
            // 
            // 
            // 
            this.cbSearchStartDay.DropDownCalendar.FirstMonth = new System.DateTime(2007, 8, 1, 0, 0, 0, 0);
            this.cbSearchStartDay.DropDownCalendar.Name = "";
            this.cbSearchStartDay.DropDownCalendar.VisualStyle = Janus.Windows.CalendarCombo.VisualStyle.Office2007;
            this.cbSearchStartDay.Location = new System.Drawing.Point(40, 8);
            this.cbSearchStartDay.Name = "cbSearchStartDay";
            this.cbSearchStartDay.Size = new System.Drawing.Size(104, 23);
            this.cbSearchStartDay.TabIndex = 5;
            this.cbSearchStartDay.Value = new System.DateTime(2007, 9, 14, 0, 0, 0, 0);
            this.cbSearchStartDay.VisualStyle = Janus.Windows.CalendarCombo.VisualStyle.Office2007;
            this.cbSearchStartDay.TextChanged += new System.EventHandler(this.rbSearchType_CheckedChanged);
            // 
            // lbSearchDate
            // 
            this.lbSearchDate.Location = new System.Drawing.Point(8, 10);
            this.lbSearchDate.Name = "lbSearchDate";
            this.lbSearchDate.Size = new System.Drawing.Size(32, 21);
            this.lbSearchDate.TabIndex = 32;
            this.lbSearchDate.Text = "�Ⱓ";
            this.lbSearchDate.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label10
            // 
            this.label10.Location = new System.Drawing.Point(144, 10);
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
            this.cbSearchEndDay.Location = new System.Drawing.Point(176, 8);
            this.cbSearchEndDay.Name = "cbSearchEndDay";
            this.cbSearchEndDay.Size = new System.Drawing.Size(104, 23);
            this.cbSearchEndDay.TabIndex = 6;
            this.cbSearchEndDay.Value = new System.DateTime(2007, 9, 14, 0, 0, 0, 0);
            this.cbSearchEndDay.VisualStyle = Janus.Windows.CalendarCombo.VisualStyle.Office2007;
            this.cbSearchEndDay.TextChanged += new System.EventHandler(this.rbSearchType_CheckedChanged);
            // 
            // label11
            // 
            this.label11.Location = new System.Drawing.Point(280, 10);
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
            this.btnSearch.Location = new System.Drawing.Point(815, 8);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(88, 24);
            this.btnSearch.TabIndex = 9;
            this.btnSearch.Text = "�� ȸ";
            this.btnSearch.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // btnExcel
            // 
            this.btnExcel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExcel.Enabled = false;
            this.btnExcel.Location = new System.Drawing.Point(911, 8);
            this.btnExcel.Name = "btnExcel";
            this.btnExcel.Size = new System.Drawing.Size(88, 24);
            this.btnExcel.TabIndex = 10;
            this.btnExcel.Text = "EXCEL ���";
            this.btnExcel.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.btnExcel.Click += new System.EventHandler(this.btnExcel_Click);
            // 
            // btnMakeData
            // 
            this.btnMakeData.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnMakeData.Enabled = false;
            this.btnMakeData.Location = new System.Drawing.Point(711, 8);
            this.btnMakeData.Name = "btnMakeData";
            this.btnMakeData.Size = new System.Drawing.Size(96, 24);
            this.btnMakeData.TabIndex = 9;
            this.btnMakeData.Text = "���ص����ͻ���";
            this.btnMakeData.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.btnMakeData.Click += new System.EventHandler(this.btnMakeData_Click);
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
            this.gridEX1.Font = new System.Drawing.Font("���� ���", 8F);
            this.gridEX1.FrozenColumns = 11;
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
            this.gridEX1.TotalRowFormatStyle.BackColor = System.Drawing.Color.Yellow;
            this.gridEX1.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            // 
            // dvReport
            // 
            this.dvReport.Table = this.inventoryPresentConditionDs.InventoryPresentCondition;
            // 
            // inventoryPresentConditionDs
            // 
            this.inventoryPresentConditionDs.DataSetName = "InventoryPresentConditionDs";
            this.inventoryPresentConditionDs.Locale = new System.Globalization.CultureInfo("en-US");
            this.inventoryPresentConditionDs.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
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
            // InventoryPresentConditionControl
            // 
            this.Controls.Add(this.uiPanelChoiceAdSchedule);
            this.Font = new System.Drawing.Font("���� ���", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.Name = "InventoryPresentConditionControl";
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
            ((System.ComponentModel.ISupportInitialize)(this.inventoryPresentConditionDs)).EndInit();
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
				btnMakeData.Enabled = true;
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
			btnMakeData.Enabled	= false;
			
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
				MessageBox.Show("�˻��������� �˻������� ���� Ů�ϴ�.", "�κ��丮��Ȳ", MessageBoxButtons.OK, MessageBoxIcon.Information);
				return;
			}
			/*
			else if(cbSearchStartDay.Value < Now)
			{
				MessageBox.Show("�˻��������� ������ �� �����ϴ�.", "�κ��丮��Ȳ", MessageBoxButtons.OK, MessageBoxIcon.Information);
				return;
			}
			*/
			DisableButton();
			SearchReport();
			InitButton();
		}

		private void InitCombo_Start()
		{
			// �Ⱓ������ �� �������� ���Ϸ� ��Ʈ
			cbSearchStartDay.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
		}

		private void InitCombo_End()
		{
			// �Ⱓ�������� ���� ���� ������ ���� ����
			DateTime dt = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month));
			cbSearchEndDay.Value = dt;
		}

		/// <summary>
		/// ���ص����� ������ư Ŭ��ó��
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnMakeData_Click(object sender, System.EventArgs e)
		{
			DisableButton();
			MakeData();
			InitButton();
		}

		#endregion

		#region ó���޼ҵ�

		/// <summary>
		/// ��ü��� ��ȸ
		/// </summary>
		private void SearchReport()
		{
			StatusMessage("�κ��丮��Ȳ�� ��ȸ�մϴ�.");
			
			ProgressStart();

			try
			{
				// �����͸� �ʱ�ȭ
				inventoryPresentConditionModel.Init();
				
				inventoryPresentConditionModel.LogDay1	=  cbSearchStartDay.Value.ToString("yyMMdd");
				inventoryPresentConditionModel.LogDay2	=  cbSearchEndDay.Value.ToString("yyMMdd");
				if(rb3Month.Checked)
				{
					inventoryPresentConditionModel.SearchType = 1;
					SearchType = rb3Month.Text;
				}
				else if(rbYear.Checked)
				{
					inventoryPresentConditionModel.SearchType = 2;
					SearchType = rbYear.Text;
				}
				else if(rbPreMon.Checked)
				{
					inventoryPresentConditionModel.SearchType = 3;
					SearchType = rbYear.Text;
				}
				
				new InventoryPresentConditionManager(systemModel,commonModel).GetInventoryPresentCondition(inventoryPresentConditionModel);

				if (inventoryPresentConditionModel.ResultCD.Equals("0000"))
				{
					Utility.SetDataTable(inventoryPresentConditionDs.InventoryPresentCondition, inventoryPresentConditionModel.ReportDataSet);													
					StatusMessage(inventoryPresentConditionModel.ResultCnt + "���� ��ȸ�Ǿ����ϴ�.");
		
					keyStartDay   = cbSearchStartDay.Value.ToString("yyyy-MM-dd");
					keyEndDay   = cbSearchEndDay.Value.ToString("yyyy-MM-dd");

					keyReportDay =  keyStartDay + " ~ " + keyEndDay;
					
					canPrint = true;

					if( inventoryPresentConditionDs.Tables[0].Rows.Count > 0)
					{
						//DataSet�� ���ǰ� �þ�� �̺κе� �����ؾ� �Ѵ�. ���� C0~C31���� ���߾��� �ִ�.
						int MaxColumnCnt = 32;		//���� �÷��� ����
						int ChangeRowData = 0;
						int TableRowCount = inventoryPresentConditionDs.Tables[0].Rows.Count;
						int GridHeaderCount = 11;
						DataRow Row2 = null;	//�����κ��丮 Row.
						DataRow Row3 = null;	//�ʿ��κ��丮 Row.

						for (int inx =0; inx < TableRowCount; inx++)
						{
							DataRow Row = inventoryPresentConditionDs.Tables[0].Rows[inx];

							if( Row != null && (Row["Gubun"].ToString().Trim().Equals("-1") ) )
							{				
								#region ù��° �����͸� ����� �����ϰ�, ����

								//�ϴ� �� ���̰� �ϰ� ����� �����.
								foreach( Janus.Windows.GridEX.GridEXColumn Col in gridEX1.RootTable.Columns )
								{
									if( Col.Caption.Equals("") || Col.Caption.Length == 0 )
									{
										Col.Visible = true;
									}
								}

								for(int i=0; i<MaxColumnCnt; i++)
								{
									gridEX1.RootTable.Columns[GridHeaderCount+i].Caption = Row[GridHeaderCount + i].ToString();
								}
								//�����͸� �����Ѵ�.
								Row.BeginEdit();
								Row.Delete();
								//������ �ߴٸ� �迭�� �޶����Ƿ� �ٽ� ����� �ϵ��� �Ѵ�.
								TableRowCount--;
								inx--;
								Row.EndEdit();


								//����� ���ٸ� �ش� �÷��� ������ �ʰ� �ٲ۴�.
								foreach( Janus.Windows.GridEX.GridEXColumn Col in gridEX1.RootTable.Columns )
								{
									if( Col.Caption.Equals("") || Col.Caption.Length == 0 )
									{
										Col.Visible = false;
									}
								}
								#endregion

								ChangeRowData++;
							}
							else if( Row != null && (Row["Gubun"].ToString().Equals("0") || Row["Gubun"].ToString().Equals("1") || Row["Gubun"].ToString().Equals("2") ) )
							{
								#region �����κ��丮�� �ʿ��κ��丮���� �۴ٸ� ��� �˷� �־�� �Ѵ�.
								//���� �����κ��丮��inx �� �����Ѵ�.
								if(Row["Gubun"].ToString().Equals("0"))
								{
									Row2 = inventoryPresentConditionDs.Tables[0].Rows[inx];
								}
								if(Row["Gubun"].ToString().Equals("1"))
								{
									Row3 = inventoryPresentConditionDs.Tables[0].Rows[inx];
									for(int i=0; i<MaxColumnCnt; i++)
									{
										if(Row2[GridHeaderCount + i].ToString() != null && !Row2[GridHeaderCount + i].ToString().Equals("") && Convert.ToInt32(Row2[GridHeaderCount + i].ToString().Replace(",", "")) < Convert.ToInt32(Row3[GridHeaderCount + i].ToString().Replace(",", "")))
										{
											gridEX1.RootTable.Columns[GridHeaderCount + i].HeaderStyle.ForeColor = Color.Red;
										}
										else if(Row2[GridHeaderCount + i].ToString() != null && !Row2[GridHeaderCount + i].ToString().Equals(""))
										{
											gridEX1.RootTable.Columns[GridHeaderCount + i].HeaderStyle.Reset();
										}
									}
								}
								#endregion

								//�������� ���⼭ �����ϵ��� �Ѵ�. ���ڵ������ ������� �ϴ� �ȱ׷��� ������ ������ �� �����ϰ� �ȴ�.
								//���� �̰����� ����� �� �ֵ��� �Ѵ�.
								if(Row["Gubun"].ToString().Equals("2"))
								{
									Row.BeginEdit();
									for(int i=-1; i<MaxColumnCnt; i++)
									{
										if(Row2[GridHeaderCount + i].ToString() != null && !Row2[GridHeaderCount + i].ToString().Equals("") && !Row2[GridHeaderCount + i].ToString().Equals("0"))
										{
											Row[GridHeaderCount + i] = ""+ (Convert.ToInt64(Row3[GridHeaderCount + i].ToString().Replace(",", "")) * 100 / Convert.ToInt32(Row2[GridHeaderCount + i].ToString().Replace(",", ""))) +"%";
										}
										else
										{
											Row[GridHeaderCount + i] = "-";
										}
									}
									Row.EndEdit();

								}

								#region �ι�°, ����°, �׹�° �������� Rnum �÷��� ''�� ����
								Row.BeginEdit();
								Row["Rnum"] = DBNull.Value;
								Row.EndEdit();
								#endregion

								ChangeRowData++;
							}


							if(ChangeRowData == 4)
							{
								break;
							}
						}
					}
				
				}
			}
			catch(FrameException fe)
			{
				FrameSystem.showMsgForm("�κ��丮��Ȳ ��ȸ����", new string[] {fe.ErrCode, fe.ResultMsg});
			}
			catch(Exception ex)
			{
				FrameSystem.showMsgForm("�κ��丮��Ȳ ��ȸ����",new string[] {"",ex.Message});
			}
			finally
			{
				ProgressStop();
			}
		}

		/// <summary>
		/// �κ��丮��Ȳ ���ص����� ����
		/// </summary>
		private void MakeData()
		{
			StatusMessage("�κ��丮��Ȳ ���ص����͸� ���� �� �Դϴ�.");
			ProgressStart();

			try
			{
				inventoryPresentConditionModel.Init();
				
				if(rb3Month.Checked)		inventoryPresentConditionModel.SearchType = 1;		// 3������� ����
				else if(rbYear.Checked)		inventoryPresentConditionModel.SearchType = 2;		// 1����� ����
				else if(rbPreMon.Checked)	inventoryPresentConditionModel.SearchType = 3;		// ������� ����
				
				new InventoryPresentConditionManager(systemModel,commonModel).SetInventorySummuryData(inventoryPresentConditionModel);

				if (inventoryPresentConditionModel.ResultCD.Equals("0000"))
				{
					StatusMessage("�κ��丮 ���� ������ ������ �Ϸᰡ �Ǿ����ϴ�.");
				}
			}
			catch(FrameException fe)
			{
				FrameSystem.showMsgForm("�κ��丮��Ȳ ���� ������ ���� ����", new string[] {fe.ErrCode, fe.ResultMsg});
			}
			catch(Exception ex)
			{
				FrameSystem.showMsgForm("�κ��丮��Ȳ ���� ������ ���� ����",new string[] {"",ex.Message});
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

		private void rbSearchType_CheckedChanged(object sender, System.EventArgs e)
		{
			if(rb3Month.Checked)
			{
				inventoryPresentConditionModel.SearchType = 1;
			}
			else if(rbYear.Checked)
			{
				inventoryPresentConditionModel.SearchType = 2;
			}
			else if(rbPreMon.Checked)
			{
				inventoryPresentConditionModel.SearchType = 3;
			}
		}

		private void gridEX1_LoadingRow(object sender, Janus.Windows.GridEX.RowLoadEventArgs e)
		{
			if(e.Row.Cells[0].Value.ToString().Equals(0))
			{
				e.Row.Cells[0].Value = e.Row.Cells[1].Value;
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

			if(SearchType.Equals(""))
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
				// �÷����������̹Ƿ� �÷����� ����Ѵ�.(���� 32��)
				//DataSet�� ���ǵ� C0~C31���� ���� ���Ͽ� ������ ���̴�. �̰��� ������ �Ǹ� �Ʒ� for���� ����.
				for(int i=0; i<32; i++)
				{
					if(!"".Equals(gridEX1.RootTable.Columns["c"+i].Caption.ToString()))
					{
						ColMax++;
					}
				}

				int TitleRow  = 1;
				int ConditionRow = 2;
				int HeaderRow = 5;
				int DataRow   = 6;
				int DisplayHeadColCount = 7;	//������ ǥ���Ǵ� ����� �÷� ��. ���⼭ ����� CXX �������� �� ������ �ٷ� �������� �ǹ��Ѵ�.
				string StartCol = "A";
				string EndCol   = "";
				string TitleCol = "D";
				int DataCount = 0;
				int CondCount = 0;
				int HeaderCount = 0;
				
				// ������ �÷��� �ε�������
				EndCol = GetColumnIndex(ColMax+DisplayHeadColCount);

				xlApp = new Excel.Application();
				oWB = (Excel._Workbook)(xlApp.Workbooks.Add(Missing.Value));
				oSheet = (Excel._Worksheet)oWB.ActiveSheet;


				// Ÿ��Ʋ �ۼ�
				oSheet.Cells[TitleRow,1] = "�κ��丮��Ȳ";

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

				oSheet.Cells[ConditionRow+CondCount,1] = "���ص�����";
				oRng = oSheet.get_Range("B"+Convert.ToString(ConditionRow+CondCount), TitleCol+Convert.ToString(ConditionRow+CondCount));
				oRng.Merge(true);
				oSheet.Cells[ConditionRow+CondCount,2] = SearchType;
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
				oSheet.Cells[HeaderRow,HeaderCount++] = gridEX1.RootTable.Columns["Rnum"].Caption.ToString();
				oSheet.Cells[HeaderRow,HeaderCount++] = gridEX1.RootTable.Columns["ItemName"].Caption.ToString();
				oSheet.Cells[HeaderRow,HeaderCount++] = gridEX1.RootTable.Columns["ContractAmt"].Caption.ToString();
				oSheet.Cells[HeaderRow,HeaderCount++] = gridEX1.RootTable.Columns["RemaindAmt"].Caption.ToString();
				oSheet.Cells[HeaderRow,HeaderCount++] = gridEX1.RootTable.Columns["StartDate"].Caption.ToString();
				oSheet.Cells[HeaderRow,HeaderCount++] = gridEX1.RootTable.Columns["EndDate"].Caption.ToString();
				oSheet.Cells[HeaderRow,HeaderCount++] = gridEX1.RootTable.Columns["RowTotal"].Caption.ToString();

				// 8����
				for(int i=0; i<ColMax; i++)
				{
					if(!"-1".Equals(gridEX1.RootTable.Columns["c"+i].Caption.ToString()))
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

				//�ȳ����� �߰�
				oSheet.Cells[2, 6] = "���";
				oSheet.Cells[2, 7] = "����";
				oSheet.Cells[2, 8] = "����";
				oSheet.Cells[3, 6] = "���Ϲ̽���";
				oRng = oSheet.get_Range("F2", "F2");
				oRng.Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.LawnGreen);   //�� ���� 
				oRng = oSheet.get_Range("G2", "G2");
				oRng.Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Violet);   //�� ���� 
				oRng = oSheet.get_Range("H2", "H2");
				oRng.Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.OrangeRed);   //�� ���� 
				oRng = oSheet.get_Range("F3", "F3");
				oRng.Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.SeaShell);   //�� ���� 

				DataCount = 0;
				// ������ ����
				for (int inx =0; inx < inventoryPresentConditionDs.Tables[0].Rows.Count; inx++)
				{		
					DataRow Row = inventoryPresentConditionDs.Tables[0].Rows[inx];

					int ColCnt = 1;
					if("0".Equals(Row["Rnum"].ToString()))
					{
						string StartCell = GetColumnIndex(ColCnt++) + (DataRow+DataCount);
						string EndCell = GetColumnIndex(ColCnt++) +(DataRow+DataCount);
						oRng = oSheet.get_Range(StartCell, EndCell);
						oRng.Merge(true);
						oSheet.Cells[DataRow+DataCount,ColCnt-2] = Row["ItemName"].ToString();
					}
					else
					{
						oSheet.Cells[DataRow+DataCount,ColCnt++] = Row["Rnum"].ToString();
						oSheet.Cells[DataRow+DataCount,ColCnt++] = Row["ItemName"].ToString();
					}
					
					if("0".Equals(Row["Gubun"].ToString()))
					{
						DataRow Row1 = inventoryPresentConditionDs.Tables[0].Rows[inx+1];	//�ʿ��κ��丮
						if(Row != null && Row1 != null)
						{
							for(int i=0; i<ColMax; i++)
							{
								if(Convert.ToInt64(Row["c"+i].ToString().Replace(",", "")) < Convert.ToInt64(Row1["c"+i].ToString().Replace(",", "")))
								{
									oRng = oSheet.get_Range(GetColumnIndex(i+8) + Convert.ToString(DataRow+DataCount-1), GetColumnIndex(i+8) + Convert.ToString(DataRow+DataCount-1));	// ����� ����
									oRng.Interior.Color      = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Red);   //�� ���� 
								}
							}
						}
					}
					else if("0".Equals(Row["Gubun"].ToString()))
					{
						oRng = oSheet.get_Range(StartCol+Convert.ToString(DataRow+DataCount), EndCol+Convert.ToString(DataRow+DataCount));	// ����� ����
						oRng.Interior.Color      = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.LawnGreen);   //�� ���� 
					}
					else if("1".Equals(Row["Gubun"].ToString()))
					{
						oRng = oSheet.get_Range(StartCol+Convert.ToString(DataRow+DataCount), EndCol+Convert.ToString(DataRow+DataCount));	// ����� ����
						oRng.Interior.Color      = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Yellow);   //�� ���� 
					}
					else if("2".Equals(Row["Gubun"].ToString()))
					{
						oRng = oSheet.get_Range(StartCol+Convert.ToString(DataRow+DataCount), EndCol+Convert.ToString(DataRow+DataCount));	// ����� ����
						oRng.Interior.Color      = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.MistyRose);   //�� ���� 
					}
					else
					{
						oRng = oSheet.get_Range(StartCol+Convert.ToString(DataRow+DataCount), EndCol+Convert.ToString(DataRow+DataCount));	// ����� ����
						if("10".Equals(Row["AdState"].ToString()))
						{
							oRng.Interior.Color      = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.LightYellow);   //����� �� ����
						}
						else if("30".Equals(Row["AdState"].ToString()))
						{
							oRng.Interior.Color      = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Violet);   //������ �� ����
						}
						else if("40".Equals(Row["AdState"].ToString()))
						{
							oRng.Interior.Color      = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.OrangeRed);   //������ �� ����
						}
						else if(!"30".Equals(Row["FileState"].ToString()))
						{
							oRng.Interior.Color      = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.SeaShell);   //���� �̹��� �� ���� 
						}
					}
					//�÷� ��������� ����
					oSheet.Cells[DataRow+DataCount,ColCnt++] = Row["ContractAmt"].ToString();
					oSheet.Cells[DataRow+DataCount,ColCnt++] = Row["RemaindAmt"].ToString();
					oSheet.Cells[DataRow+DataCount,ColCnt++] = Row["StartDate"].ToString();
					oSheet.Cells[DataRow+DataCount,ColCnt++] = Row["EndDate"].ToString();
					oSheet.Cells[DataRow+DataCount,ColCnt++] = Row["RowTotal"].ToString();
					//���� �÷� ������(��ī�װ���) ����
					for(int i=0; i<ColMax; i++)
					{
						if(!"".Equals(gridEX1.RootTable.Columns["c"+i].Caption.ToString()))
						{
							oSheet.Cells[DataRow+DataCount,ColCnt++] = Row["c"+i].ToString();
						}
					}
					
					DataCount++;
				}

				DataCount--;

				// ������ �ۼ�
				
				oRng = oSheet.get_Range(StartCol+Convert.ToString(HeaderRow), EndCol+Convert.ToString(DataRow+DataCount));	// �������� ����
				oRng.Font.Size = 9;
				oRng.EntireColumn.AutoFit();					// �������� ũ�⿡ ���� ����ũ�� ����
				oRng.Borders.LineStyle =  Excel.XlLineStyle.xlContinuous;	// �׵θ��Ӽ� �Ʒ��� �Ǽ�
				oRng.Borders.Weight    = Excel.XlBorderWeight.xlThin;		// �׵θ��Ӽ� �Ʒ��� ���¼�


				//��ȣ �߾�����
				oRng = oSheet.get_Range(GetColumnIndex(1)+Convert.ToString(DataRow), GetColumnIndex(1)+Convert.ToString(DataRow+DataCount)); 
				oRng.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;

				// ��๰��,���๰���� �������̰� �������� ��������
				oRng = oSheet.get_Range(GetColumnIndex(3)+Convert.ToString(DataRow), GetColumnIndex(4)+Convert.ToString(DataRow+DataCount)); 
				oRng.HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;
				oRng.NumberFormatLocal = "#,##0";

				// ���ڴ� �߾�����
				oRng = oSheet.get_Range(GetColumnIndex(5)+Convert.ToString(DataRow), GetColumnIndex(6)+Convert.ToString(DataRow+DataCount)); 
				oRng.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
			
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
