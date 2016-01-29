// ===============================================================================
//
// StatisticsPgWeekControl.cs
//
// ����������Ʈ ���Ϻ���� ��Ʈ���� �����մϴ�. 
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
    /// ����������Ʈ ���Ϻ���� ���� ��Ʈ��
    /// </summary>
    public class StatisticsPgWeekControl : System.Windows.Forms.UserControl, IUserControl
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
        StatisticsPgWeekModel statisticsPgWeekModel  = new StatisticsPgWeekModel();	// ���α׷��� �����û�����

        // ȭ��ó���� ����
		bool canRead			  = false;
		bool IsNewSearchKey		  = true;					// �˻����Է� ����

		// Key ������
		string keyMediaName    = "";
		string keyCategoryName = "";
		string keyGenreName   = "";
		string keyReportBgnDay = "";
		string keyReportEndDay = "";
		string keyReportType   = "";
        private Label label2;

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
		private Janus.Windows.GridEX.GridEX grdExRepoert;
		private Janus.Windows.UI.Dock.UIPanelManager uiPM;
		private Janus.Windows.EditControls.UIGroupBox uiGroupBox1;
		private Janus.Windows.EditControls.UIRadioButton rbSearchType_W;
		private Janus.Windows.EditControls.UIRadioButton rbSearchType_C;
		private Janus.Windows.EditControls.UIRadioButton rbSearchType_M;
		private Janus.Windows.EditControls.UIRadioButton rbSearchType_D;
		private Janus.Windows.CalendarCombo.CalendarCombo cbSearchStartDay;
		private System.Windows.Forms.Label lbSearchDate;
		private Janus.Windows.EditControls.UIComboBox cbSearchMedia;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Label label10;
		private Janus.Windows.CalendarCombo.CalendarCombo cbSearchEndDay;
		private System.Windows.Forms.Label label11;
		private Janus.Windows.GridEX.GridEX grdExReport;
		private Janus.Windows.EditControls.UIComboBox cbSearchCategory;
		private Janus.Windows.EditControls.UIComboBox cbSearchGenre;
		private Janus.Windows.GridEX.EditControls.EditBox ebSearchKey;
		private Janus.Windows.EditControls.UIRadioButton rbSearchType_1WA;
		private Janus.Windows.EditControls.UIRadioButton rbSearchType_1MA;
		private Janus.Windows.EditControls.UIRadioButton rbSearchType_2MA;
		private Janus.Windows.EditControls.UIRadioButton rbSearchType_3MA;
		private AdManagerClient._50_ReportMedia.ReportMediaDs statisticsPgWeekDs;
        private System.ComponentModel.IContainer components;

        public StatisticsPgWeekControl()
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
            Janus.Windows.GridEX.GridEXLayout grdExReport_DesignTimeLayout = new Janus.Windows.GridEX.GridEXLayout();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(StatisticsPgWeekControl));
            this.uiPM = new Janus.Windows.UI.Dock.UIPanelManager(this.components);
            this.uiPanelChoiceAdSchedule = new Janus.Windows.UI.Dock.UIPanelGroup();
            this.uiPanelSearch = new Janus.Windows.UI.Dock.UIPanel();
            this.uiPanelSearchContainer = new Janus.Windows.UI.Dock.UIPanelInnerContainer();
            this.pnlSearch = new System.Windows.Forms.Panel();
            this.ebSearchKey = new Janus.Windows.GridEX.EditControls.EditBox();
            this.uiGroupBox1 = new Janus.Windows.EditControls.UIGroupBox();
            this.rbSearchType_W = new Janus.Windows.EditControls.UIRadioButton();
            this.rbSearchType_C = new Janus.Windows.EditControls.UIRadioButton();
            this.rbSearchType_M = new Janus.Windows.EditControls.UIRadioButton();
            this.rbSearchType_D = new Janus.Windows.EditControls.UIRadioButton();
            this.rbSearchType_1WA = new Janus.Windows.EditControls.UIRadioButton();
            this.rbSearchType_1MA = new Janus.Windows.EditControls.UIRadioButton();
            this.rbSearchType_2MA = new Janus.Windows.EditControls.UIRadioButton();
            this.rbSearchType_3MA = new Janus.Windows.EditControls.UIRadioButton();
            this.cbSearchStartDay = new Janus.Windows.CalendarCombo.CalendarCombo();
            this.lbSearchDate = new System.Windows.Forms.Label();
            this.cbSearchMedia = new Janus.Windows.EditControls.UIComboBox();
            this.cbSearchCategory = new Janus.Windows.EditControls.UIComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.cbSearchGenre = new Janus.Windows.EditControls.UIComboBox();
            this.label10 = new System.Windows.Forms.Label();
            this.cbSearchEndDay = new Janus.Windows.CalendarCombo.CalendarCombo();
            this.label11 = new System.Windows.Forms.Label();
            this.btnSearch = new Janus.Windows.EditControls.UIButton();
            this.btnExcel = new Janus.Windows.EditControls.UIButton();
            this.uiPanelList = new Janus.Windows.UI.Dock.UIPanel();
            this.uiPanelListContainer = new Janus.Windows.UI.Dock.UIPanelInnerContainer();
            this.grdExReport = new Janus.Windows.GridEX.GridEX();
            this.dvReport = new System.Data.DataView();
            this.statisticsPgWeekDs = new AdManagerClient._50_ReportMedia.ReportMediaDs();
            this.grdExRepoert = new Janus.Windows.GridEX.GridEX();
            this.panel3 = new System.Windows.Forms.Panel();
            this.panMenuSchedule = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.editBox1 = new Janus.Windows.GridEX.EditControls.EditBox();
            this.label2 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.uiPM)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelChoiceAdSchedule)).BeginInit();
            this.uiPanelChoiceAdSchedule.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelSearch)).BeginInit();
            this.uiPanelSearch.SuspendLayout();
            this.uiPanelSearchContainer.SuspendLayout();
            this.pnlSearch.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uiGroupBox1)).BeginInit();
            this.uiGroupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelList)).BeginInit();
            this.uiPanelList.SuspendLayout();
            this.uiPanelListContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdExReport)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dvReport)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.statisticsPgWeekDs)).BeginInit();
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
            this.uiPM.AddDockPanelInfo(new System.Guid("afb3b7d0-aa79-443c-a1e8-3baba38f7a4d"), new System.Guid("b162c53a-7940-47cd-a0ed-9762c59c50d8"), 86, true);
            this.uiPM.AddDockPanelInfo(new System.Guid("8e3140b9-bfcb-43af-b84e-2ee6cddf51dc"), new System.Guid("b162c53a-7940-47cd-a0ed-9762c59c50d8"), 545, true);
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
            this.uiPanelChoiceAdSchedule.Text = "���Ϻ����";
            // 
            // uiPanelSearch
            // 
            this.uiPanelSearch.AllowResize = Janus.Windows.UI.InheritableBoolean.False;
            this.uiPanelSearch.CaptionVisible = Janus.Windows.UI.InheritableBoolean.False;
            this.uiPanelSearch.CloseButtonVisible = Janus.Windows.UI.InheritableBoolean.False;
            this.uiPanelSearch.InnerContainer = this.uiPanelSearchContainer;
            this.uiPanelSearch.Location = new System.Drawing.Point(0, 22);
            this.uiPanelSearch.Name = "uiPanelSearch";
            this.uiPanelSearch.Size = new System.Drawing.Size(1010, 88);
            this.uiPanelSearch.TabIndex = 4;
            this.uiPanelSearch.Text = "�˻�";
            // 
            // uiPanelSearchContainer
            // 
            this.uiPanelSearchContainer.Controls.Add(this.pnlSearch);
            this.uiPanelSearchContainer.Location = new System.Drawing.Point(1, 1);
            this.uiPanelSearchContainer.Name = "uiPanelSearchContainer";
            this.uiPanelSearchContainer.Size = new System.Drawing.Size(1008, 86);
            this.uiPanelSearchContainer.TabIndex = 0;
            // 
            // pnlSearch
            // 
            this.pnlSearch.BackColor = System.Drawing.SystemColors.Window;
            this.pnlSearch.Controls.Add(this.label2);
            this.pnlSearch.Controls.Add(this.ebSearchKey);
            this.pnlSearch.Controls.Add(this.uiGroupBox1);
            this.pnlSearch.Controls.Add(this.cbSearchStartDay);
            this.pnlSearch.Controls.Add(this.lbSearchDate);
            this.pnlSearch.Controls.Add(this.cbSearchMedia);
            this.pnlSearch.Controls.Add(this.cbSearchCategory);
            this.pnlSearch.Controls.Add(this.label6);
            this.pnlSearch.Controls.Add(this.cbSearchGenre);
            this.pnlSearch.Controls.Add(this.label10);
            this.pnlSearch.Controls.Add(this.cbSearchEndDay);
            this.pnlSearch.Controls.Add(this.label11);
            this.pnlSearch.Controls.Add(this.btnSearch);
            this.pnlSearch.Controls.Add(this.btnExcel);
            this.pnlSearch.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlSearch.Font = new System.Drawing.Font("���� ���", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.pnlSearch.Location = new System.Drawing.Point(0, 0);
            this.pnlSearch.Name = "pnlSearch";
            this.pnlSearch.Size = new System.Drawing.Size(1008, 86);
            this.pnlSearch.TabIndex = 3;
            // 
            // ebSearchKey
            // 
            this.ebSearchKey.Location = new System.Drawing.Point(496, 7);
            this.ebSearchKey.Name = "ebSearchKey";
            this.ebSearchKey.Size = new System.Drawing.Size(184, 23);
            this.ebSearchKey.TabIndex = 4;
            this.ebSearchKey.Text = "���α׷����� �Է��ϼ���";
            this.ebSearchKey.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
            this.ebSearchKey.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            this.ebSearchKey.TextChanged += new System.EventHandler(this.ebSearchKey_TextChanged);
            this.ebSearchKey.Click += new System.EventHandler(this.ebSearchKey_Click);
            this.ebSearchKey.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ebSearchKey_KeyDown);
            // 
            // uiGroupBox1
            // 
            this.uiGroupBox1.BorderColor = System.Drawing.Color.Transparent;
            this.uiGroupBox1.Controls.Add(this.rbSearchType_W);
            this.uiGroupBox1.Controls.Add(this.rbSearchType_C);
            this.uiGroupBox1.Controls.Add(this.rbSearchType_M);
            this.uiGroupBox1.Controls.Add(this.rbSearchType_D);
            this.uiGroupBox1.Controls.Add(this.rbSearchType_1WA);
            this.uiGroupBox1.Controls.Add(this.rbSearchType_1MA);
            this.uiGroupBox1.Controls.Add(this.rbSearchType_2MA);
            this.uiGroupBox1.Controls.Add(this.rbSearchType_3MA);
            this.uiGroupBox1.Location = new System.Drawing.Point(96, 32);
            this.uiGroupBox1.Name = "uiGroupBox1";
            this.uiGroupBox1.Size = new System.Drawing.Size(624, 24);
            this.uiGroupBox1.TabIndex = 5;
            // 
            // rbSearchType_W
            // 
            this.rbSearchType_W.Checked = true;
            this.rbSearchType_W.Location = new System.Drawing.Point(112, 0);
            this.rbSearchType_W.Name = "rbSearchType_W";
            this.rbSearchType_W.Size = new System.Drawing.Size(48, 23);
            this.rbSearchType_W.TabIndex = 5;
            this.rbSearchType_W.TabStop = true;
            this.rbSearchType_W.Text = "�ְ�";
            this.rbSearchType_W.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.rbSearchType_W.CheckedChanged += new System.EventHandler(this.rbSearchType_W_CheckedChanged);
            // 
            // rbSearchType_C
            // 
            this.rbSearchType_C.BackColor = System.Drawing.SystemColors.Window;
            this.rbSearchType_C.Location = new System.Drawing.Point(0, 0);
            this.rbSearchType_C.Name = "rbSearchType_C";
            this.rbSearchType_C.Size = new System.Drawing.Size(48, 23);
            this.rbSearchType_C.TabIndex = 5;
            this.rbSearchType_C.Text = "�Ⱓ";
            this.rbSearchType_C.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.rbSearchType_C.CheckedChanged += new System.EventHandler(this.rbSearchType_C_CheckedChanged);
            // 
            // rbSearchType_M
            // 
            this.rbSearchType_M.Location = new System.Drawing.Point(168, 0);
            this.rbSearchType_M.Name = "rbSearchType_M";
            this.rbSearchType_M.Size = new System.Drawing.Size(48, 23);
            this.rbSearchType_M.TabIndex = 5;
            this.rbSearchType_M.Text = "����";
            this.rbSearchType_M.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.rbSearchType_M.CheckedChanged += new System.EventHandler(this.rbSearchType_M_CheckedChanged);
            // 
            // rbSearchType_D
            // 
            this.rbSearchType_D.Location = new System.Drawing.Point(56, 0);
            this.rbSearchType_D.Name = "rbSearchType_D";
            this.rbSearchType_D.Size = new System.Drawing.Size(56, 23);
            this.rbSearchType_D.TabIndex = 5;
            this.rbSearchType_D.Text = "�ϰ�";
            this.rbSearchType_D.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.rbSearchType_D.CheckedChanged += new System.EventHandler(this.rbSearchType_D_CheckedChanged);
            // 
            // rbSearchType_1WA
            // 
            this.rbSearchType_1WA.Location = new System.Drawing.Point(224, 0);
            this.rbSearchType_1WA.Name = "rbSearchType_1WA";
            this.rbSearchType_1WA.Size = new System.Drawing.Size(80, 23);
            this.rbSearchType_1WA.TabIndex = 6;
            this.rbSearchType_1WA.Text = "1�������";
            this.rbSearchType_1WA.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.rbSearchType_1WA.CheckedChanged += new System.EventHandler(this.rbSearchType_1WA_CheckedChanged);
            // 
            // rbSearchType_1MA
            // 
            this.rbSearchType_1MA.Location = new System.Drawing.Point(312, 0);
            this.rbSearchType_1MA.Name = "rbSearchType_1MA";
            this.rbSearchType_1MA.Size = new System.Drawing.Size(80, 23);
            this.rbSearchType_1MA.TabIndex = 6;
            this.rbSearchType_1MA.Text = "1�������";
            this.rbSearchType_1MA.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.rbSearchType_1MA.CheckedChanged += new System.EventHandler(this.rbSearchType_1MA_CheckedChanged);
            // 
            // rbSearchType_2MA
            // 
            this.rbSearchType_2MA.Location = new System.Drawing.Point(400, 0);
            this.rbSearchType_2MA.Name = "rbSearchType_2MA";
            this.rbSearchType_2MA.Size = new System.Drawing.Size(80, 23);
            this.rbSearchType_2MA.TabIndex = 6;
            this.rbSearchType_2MA.Text = "2�������";
            this.rbSearchType_2MA.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.rbSearchType_2MA.CheckedChanged += new System.EventHandler(this.rbSearchType_2MA_CheckedChanged);
            // 
            // rbSearchType_3MA
            // 
            this.rbSearchType_3MA.Location = new System.Drawing.Point(488, 0);
            this.rbSearchType_3MA.Name = "rbSearchType_3MA";
            this.rbSearchType_3MA.Size = new System.Drawing.Size(80, 23);
            this.rbSearchType_3MA.TabIndex = 6;
            this.rbSearchType_3MA.Text = "3�������";
            this.rbSearchType_3MA.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.rbSearchType_3MA.CheckedChanged += new System.EventHandler(this.rbSearchType_3MA_CheckedChanged);
            // 
            // cbSearchStartDay
            // 
            // 
            // 
            // 
            this.cbSearchStartDay.DropDownCalendar.FirstMonth = new System.DateTime(2007, 8, 1, 0, 0, 0, 0);
            this.cbSearchStartDay.DropDownCalendar.Name = "";
            this.cbSearchStartDay.DropDownCalendar.VisualStyle = Janus.Windows.CalendarCombo.VisualStyle.Office2007;
            this.cbSearchStartDay.Location = new System.Drawing.Point(96, 56);
            this.cbSearchStartDay.Name = "cbSearchStartDay";
            this.cbSearchStartDay.Size = new System.Drawing.Size(104, 23);
            this.cbSearchStartDay.TabIndex = 7;
            this.cbSearchStartDay.Value = new System.DateTime(2007, 9, 14, 0, 0, 0, 0);
            this.cbSearchStartDay.VisualStyle = Janus.Windows.CalendarCombo.VisualStyle.Office2007;
            this.cbSearchStartDay.TextChanged += new System.EventHandler(this.cbSearchStartDay_TextChanged);
            // 
            // lbSearchDate
            // 
            this.lbSearchDate.Location = new System.Drawing.Point(8, 56);
            this.lbSearchDate.Name = "lbSearchDate";
            this.lbSearchDate.Size = new System.Drawing.Size(72, 21);
            this.lbSearchDate.TabIndex = 32;
            this.lbSearchDate.Text = "���������";
            this.lbSearchDate.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cbSearchMedia
            // 
            this.cbSearchMedia.BackColor = System.Drawing.Color.White;
            this.cbSearchMedia.ComboStyle = Janus.Windows.EditControls.ComboStyle.DropDownList;
            this.cbSearchMedia.Location = new System.Drawing.Point(8, 8);
            this.cbSearchMedia.Name = "cbSearchMedia";
            this.cbSearchMedia.Size = new System.Drawing.Size(112, 23);
            this.cbSearchMedia.TabIndex = 1;
            this.cbSearchMedia.Text = "��ü����";
            this.cbSearchMedia.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.cbSearchMedia.SelectedIndexChanged += new System.EventHandler(this.cbSearchMedia_SelectedIndexChanged);
            // 
            // cbSearchCategory
            // 
            this.cbSearchCategory.BackColor = System.Drawing.Color.White;
            this.cbSearchCategory.ComboStyle = Janus.Windows.EditControls.ComboStyle.DropDownList;
            this.cbSearchCategory.Location = new System.Drawing.Point(128, 8);
            this.cbSearchCategory.Name = "cbSearchCategory";
            this.cbSearchCategory.Size = new System.Drawing.Size(136, 23);
            this.cbSearchCategory.TabIndex = 2;
            this.cbSearchCategory.Text = "ī�װ�����";
            this.cbSearchCategory.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.cbSearchCategory.SelectedItemChanged += new System.EventHandler(this.cbSearchCategory_SelectedItemChanged);
            // 
            // label6
            // 
            this.label6.Location = new System.Drawing.Point(8, 32);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(80, 21);
            this.label6.TabIndex = 24;
            this.label6.Text = "����Ⱓ����";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cbSearchGenre
            // 
            this.cbSearchGenre.BackColor = System.Drawing.Color.White;
            this.cbSearchGenre.ComboStyle = Janus.Windows.EditControls.ComboStyle.DropDownList;
            this.cbSearchGenre.Location = new System.Drawing.Point(272, 8);
            this.cbSearchGenre.Name = "cbSearchGenre";
            this.cbSearchGenre.Size = new System.Drawing.Size(128, 23);
            this.cbSearchGenre.TabIndex = 3;
            this.cbSearchGenre.Text = "�帣����";
            this.cbSearchGenre.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            // 
            // label10
            // 
            this.label10.Location = new System.Drawing.Point(200, 56);
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
            this.cbSearchEndDay.Enabled = false;
            this.cbSearchEndDay.Location = new System.Drawing.Point(232, 56);
            this.cbSearchEndDay.Name = "cbSearchEndDay";
            this.cbSearchEndDay.Size = new System.Drawing.Size(104, 23);
            this.cbSearchEndDay.TabIndex = 7;
            this.cbSearchEndDay.Value = new System.DateTime(2007, 9, 14, 0, 0, 0, 0);
            this.cbSearchEndDay.VisualStyle = Janus.Windows.CalendarCombo.VisualStyle.Office2007;
            // 
            // label11
            // 
            this.label11.Location = new System.Drawing.Point(336, 56);
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
            this.btnSearch.TabIndex = 8;
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
            this.btnExcel.TabIndex = 9;
            this.btnExcel.Text = "EXCEL ���";
            this.btnExcel.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.btnExcel.Click += new System.EventHandler(this.btnExcel_Click);
            // 
            // uiPanelList
            // 
            this.uiPanelList.BackColor = System.Drawing.SystemColors.Window;
            this.uiPanelList.CaptionStyle = Janus.Windows.UI.Dock.PanelCaptionStyle.Dark;
            this.uiPanelList.InnerContainer = this.uiPanelListContainer;
            this.uiPanelList.Location = new System.Drawing.Point(0, 114);
            this.uiPanelList.MinimumSize = new System.Drawing.Size(-1, 100);
            this.uiPanelList.Name = "uiPanelList";
            this.uiPanelList.Size = new System.Drawing.Size(1010, 563);
            this.uiPanelList.TabIndex = 4;
            this.uiPanelList.Text = "���";
            // 
            // uiPanelListContainer
            // 
            this.uiPanelListContainer.BackColor = System.Drawing.SystemColors.Window;
            this.uiPanelListContainer.Controls.Add(this.grdExReport);
            this.uiPanelListContainer.Location = new System.Drawing.Point(1, 23);
            this.uiPanelListContainer.Name = "uiPanelListContainer";
            this.uiPanelListContainer.Size = new System.Drawing.Size(1008, 539);
            this.uiPanelListContainer.TabIndex = 0;
            // 
            // grdExReport
            // 
            this.grdExReport.AllowEdit = Janus.Windows.GridEX.InheritableBoolean.False;
            this.grdExReport.AlternatingColors = true;
            this.grdExReport.AutomaticSort = false;
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
            this.grdExReport.Font = new System.Drawing.Font("���� ���", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
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
            this.grdExReport.Size = new System.Drawing.Size(1008, 539);
            this.grdExReport.TabIndex = 11;
            this.grdExReport.TabKeyBehavior = Janus.Windows.GridEX.TabKeyBehavior.ControlNavigation;
            this.grdExReport.ThemedAreas = ((Janus.Windows.GridEX.ThemedArea)(((((((((Janus.Windows.GridEX.ThemedArea.ScrollBars | Janus.Windows.GridEX.ThemedArea.EditControls)
                        | Janus.Windows.GridEX.ThemedArea.Headers)
                        | Janus.Windows.GridEX.ThemedArea.GroupByBox)
                        | Janus.Windows.GridEX.ThemedArea.GroupRows)
                        | Janus.Windows.GridEX.ThemedArea.ControlBorder)
                        | Janus.Windows.GridEX.ThemedArea.Cards)
                        | Janus.Windows.GridEX.ThemedArea.Gridlines)
                        | Janus.Windows.GridEX.ThemedArea.CheckBoxes)));
            this.grdExReport.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            this.grdExReport.DrawGridArea += new Janus.Windows.GridEX.DrawGridAreaEventHandler(this.grdExReport_DrawGridArea);
            // 
            // dvReport
            // 
            this.dvReport.Table = this.statisticsPgWeekDs.ReportWeek;
            // 
            // statisticsPgWeekDs
            // 
            this.statisticsPgWeekDs.DataSetName = "ReportMediaDs";
            this.statisticsPgWeekDs.Locale = new System.Globalization.CultureInfo("en-US");
            this.statisticsPgWeekDs.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
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
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(411, 13);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(79, 15);
            this.label2.TabIndex = 34;
            this.label2.Text = "���α׷��߰�";
            // 
            // StatisticsPgWeekControl
            // 
            this.Controls.Add(this.uiPanelChoiceAdSchedule);
            this.Name = "StatisticsPgWeekControl";
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
            ((System.ComponentModel.ISupportInitialize)(this.uiGroupBox1)).EndInit();
            this.uiGroupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelList)).EndInit();
            this.uiPanelList.ResumeLayout(false);
            this.uiPanelListContainer.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdExReport)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dvReport)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.statisticsPgWeekDs)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdExRepoert)).EndInit();
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

			ProgressStop();
			InitButton();
		}

		private void InitCombo()
		{
			Init_MediaCode();
			InitCombo_Level();
			
			// �Ⱓ�������� ������ �����Ϸ� ��Ʈ
			DateTime dt = DateTime.Now.AddDays(-7); //���� ���ϱ��� 7�������� ��Ʈ

			for(int i=0;i<7;i++)
			{
				if(dt.DayOfWeek == System.DayOfWeek.Monday)
				{
					break;
				}
				dt = dt.AddDays(-1);
			}
			cbSearchStartDay.Value = dt;
			cbSearchEndDay.Value = cbSearchStartDay.Value.AddDays(6);

			rbSearchType_W.Checked = true;
		}

		private void Init_MediaCode()
		{
			// ��ü�� ��ȸ�Ѵ�.
			MediaCodeModel mediacodeModel = new MediaCodeModel();
			new MediaCodeManager(systemModel, commonModel).GetMediaCodeList(mediacodeModel);
			
			if (mediacodeModel.ResultCD.Equals("0000"))
			{
				// �����ͼ¿� ����
				Utility.SetDataTable(statisticsPgWeekDs.Media, mediacodeModel.MediaCodeDataSet);				
			}

			// �˻������� �޺�
			this.cbSearchMedia.Items.Clear();
			
			// �޺��ڽ��� ��Ʈ�� �ڵ����� ���� Item�迭�� ����
			Janus.Windows.EditControls.UIComboBoxItem[] comboItems = new Janus.Windows.EditControls.UIComboBoxItem[mediacodeModel.ResultCnt + 1];

			comboItems[0] = new Janus.Windows.EditControls.UIComboBoxItem("��ü����","00");
			
			for(int i=0;i<mediacodeModel.ResultCnt;i++)
			{
				DataRow row = statisticsPgWeekDs.Media.Rows[i];

				string val = row["MediaCode"].ToString();
				string txt = row["MediaName"].ToString();
				comboItems[i+1] = new Janus.Windows.EditControls.UIComboBoxItem(txt,val);
			}
			// �޺��� ��Ʈ
			this.cbSearchMedia.Items.AddRange(comboItems);
			this.cbSearchMedia.SelectedIndex = 0;

			Application.DoEvents();
		}


		private void Init_cbSearchCategory()
		{
			string MediaCode = cbSearchMedia.SelectedItem.Value.ToString();
			if(MediaCode.Equals("00")) return;

			// ī�װ��� ��ȸ�Ѵ�.
			StatisticsPgWeekModel statisticsPgWeekModel = new StatisticsPgWeekModel();

			statisticsPgWeekModel.Init();
			statisticsPgWeekModel.SearchMediaCode = MediaCode;

			new StatisticsPgWeekManager(systemModel, commonModel).GetCategoryList(statisticsPgWeekModel);
			
			if (statisticsPgWeekModel.ResultCD.Equals("0000"))
			{
				// �����ͼ¿� ����
				Utility.SetDataTable(statisticsPgWeekDs.Category, statisticsPgWeekModel.CategoryDataSet);				
			}

			int CategoryCnt = statisticsPgWeekModel.ResultCnt;

			// �帣�� ��ȸ�Ѵ�.
			statisticsPgWeekModel.Init();
			statisticsPgWeekModel.SearchMediaCode = MediaCode;

			new StatisticsPgWeekManager(systemModel, commonModel).GetGenreList(statisticsPgWeekModel);
			
			if (statisticsPgWeekModel.ResultCD.Equals("0000"))
			{
				// �����ͼ¿� ����
				Utility.SetDataTable(statisticsPgWeekDs.Genre, statisticsPgWeekModel.GenreDataSet);				
			}

			// �˻������� �޺�
			this.cbSearchCategory.Items.Clear();
			
			// �޺��� ��Ʈ
			this.cbSearchCategory.Items.Add(new Janus.Windows.EditControls.UIComboBoxItem("ī�װ���ü","00"));
			for(int i=0;i<CategoryCnt;i++)
			{
				DataRow row = statisticsPgWeekDs.Category.Rows[i];

				string val = row["CategoryCode"].ToString();
				string txt = row["CategoryName"].ToString();
				// �޺��� ��Ʈ
				this.cbSearchCategory.Items.Add(new Janus.Windows.EditControls.UIComboBoxItem(txt,val));
			}
			this.cbSearchCategory.SelectedIndex = 0;

			Init_cbSearchGenre();

			Application.DoEvents();
		}

		private void Init_cbSearchGenre()
		{
			// �˻������� �޺�
			this.cbSearchGenre.Items.Clear();
			
			// �޺��� ��Ʈ
			this.cbSearchGenre.Items.Add(new Janus.Windows.EditControls.UIComboBoxItem("�帣��ü","00"));
			this.cbSearchGenre.SelectedIndex = 0;

			Application.DoEvents();
		}

		private void SetcbSearchGenre(string CategoryCode)
		{
			Init_cbSearchGenre();
			
			if(CategoryCode.Length > 0 && !CategoryCode.Equals("00"))
			{
				// �帣Dataset���� �ش� ī�װ��� �帣�� ��ȸ
				DataRow[] genreRow =  statisticsPgWeekDs.Tables["Genre"].Select("CategoryCode = '" + CategoryCode +"'");


				// �޺��� ��Ʈ
				for(int i=0;i<genreRow.Length;i++)
				{
					DataRow row = genreRow[i];

					string val = row["GenreCode"].ToString();
					string txt = row["GenreName"].ToString();
					// �޺��� ��Ʈ
					this.cbSearchGenre.Items.Add(new Janus.Windows.EditControls.UIComboBoxItem(txt,val));
				}
			}

			Application.DoEvents();
		}


		private void InitCombo_Level()
		{

			if(commonModel.UserLevel=="20")
			{
				cbSearchMedia.SelectedValue = commonModel.MediaCode;			
				cbSearchMedia.ReadOnly = true;					
			}
			else
			{
				for(int i=0;i < statisticsPgWeekDs.Media.Rows.Count;i++)
				{
					DataRow row = statisticsPgWeekDs.Media.Rows[i];					
					if(row["MediaCode"].ToString().Equals(FrameSystem._HANATV.ToString()))
					{
						cbSearchMedia.SelectedValue = FrameSystem._HANATV; // �ϳ�TV�� �⺻������ �Ѵ�.	 		
						break;															
					}
					else
					{
						cbSearchMedia.SelectedValue="00";
					}
				}	
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

		#region �׼�ó�� �޼ҵ�


		private void cbSearchMedia_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			Init_cbSearchCategory();
		}


		private void cbSearchCategory_SelectedItemChanged(object sender, System.EventArgs e)
		{
			Init_cbSearchGenre();

			string CategoryCode = cbSearchCategory.SelectedItem.Value.ToString();
			if(CategoryCode.Equals("00")) return;

			SetcbSearchGenre(CategoryCode);
		}

		/// <summary>
		/// �˻��� ����
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void ebSearchKey_TextChanged(object sender, System.EventArgs e)
		{
			IsNewSearchKey = false;
		}

		/// <summary>
		/// �˻��� Ŭ�� 
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

		private void rbSearchType_C_CheckedChanged(object sender, System.EventArgs e)
		{
			if(rbSearchType_C.Checked)
			{
				cbSearchStartDay.Enabled = true;
				cbSearchEndDay.Enabled   = true;
			}	
		}


		private void rbSearchType_D_CheckedChanged(object sender, System.EventArgs e)
		{
			if(rbSearchType_D.Checked)
			{
				cbSearchStartDay.Enabled = true;
				cbSearchEndDay.Enabled   = false;
				cbSearchEndDay.Value  = cbSearchStartDay.Value;
			}			
		}

		private void rbSearchType_W_CheckedChanged(object sender, System.EventArgs e)
		{
			if(rbSearchType_W.Checked)
			{
				cbSearchStartDay.Enabled = true;
				cbSearchEndDay.Enabled   = false;

				DateTime dt = cbSearchStartDay.Value;

				for(int i=0;i<7;i++)
				{
					if(dt.DayOfWeek == System.DayOfWeek.Monday)
					{
						break;
					}
					dt = dt.AddDays(-1);
				}
				cbSearchStartDay.Value = dt;
				cbSearchEndDay.Value   = cbSearchStartDay.Value.AddDays(6);
			}			
		}


		private void rbSearchType_M_CheckedChanged(object sender, System.EventArgs e)
		{
			if(rbSearchType_M.Checked)
			{
				cbSearchStartDay.Enabled = true;
				cbSearchEndDay.Enabled   = false;

				cbSearchStartDay.Text = cbSearchStartDay.Value.ToString("yyyy-MM-01");
				cbSearchEndDay.Value  = cbSearchStartDay.Value.AddMonths(1).AddDays(-1);
			}			
		}

		private void rbSearchType_1WA_CheckedChanged(object sender, System.EventArgs e)
		{
			if(rbSearchType_1WA.Checked)
			{
				cbSearchStartDay.Enabled = false;
				cbSearchEndDay.Enabled   = false;

				// �Ⱓ�������� ������ �����Ϸ� ��Ʈ
				DateTime dt = DateTime.Now.AddDays(-7); //���� ���ϱ��� 7�������� ��Ʈ

				for(int i=0;i<7;i++)
				{
					if(dt.DayOfWeek == System.DayOfWeek.Monday)
					{
						break;
					}
					dt = dt.AddDays(-1);
				}
				cbSearchStartDay.Value = dt;
				cbSearchEndDay.Value = cbSearchStartDay.Value.AddDays(6);
			}			
		}

		private void rbSearchType_1MA_CheckedChanged(object sender, System.EventArgs e)
		{
			if(rbSearchType_1MA.Checked)
			{
				cbSearchStartDay.Enabled = false;
				cbSearchEndDay.Enabled   = false;

				DateTime dt = DateTime.Now;
				cbSearchStartDay.Value = new DateTime(dt.Year, dt.AddMonths(-1).Month, 1);		// ���� 1��
				cbSearchEndDay.Value = new DateTime(dt.Year, dt.Month, 1).AddDays(-1);			// �ݿ� 1�� ���� = ���� ��������
			}					
		}

		private void rbSearchType_2MA_CheckedChanged(object sender, System.EventArgs e)
		{
			if(rbSearchType_2MA.Checked)
			{
				cbSearchStartDay.Enabled = false;
				cbSearchEndDay.Enabled   = false;

				DateTime dt = DateTime.Now;
				cbSearchStartDay.Value = new DateTime(dt.Year, dt.AddMonths(-2).Month, 1);		// ������ 1��
				cbSearchEndDay.Value = new DateTime(dt.Year, dt.Month, 1).AddDays(-1);			// �ݿ� 1�� ���� = ���� ��������
			}				
		}

		private void rbSearchType_3MA_CheckedChanged(object sender, System.EventArgs e)
		{
			if(rbSearchType_3MA.Checked)
			{
				cbSearchStartDay.Enabled = false;
				cbSearchEndDay.Enabled   = false;

				DateTime dt = DateTime.Now;
				cbSearchStartDay.Value = new DateTime(dt.Year, dt.AddMonths(-3).Month, 1);		// �������� 1��
				cbSearchEndDay.Value = new DateTime(dt.Year, dt.Month, 1).AddDays(-1);			// �ݿ� 1�� ���� = ���� ��������
			}						
		}

		private void cbSearchStartDay_TextChanged(object sender, System.EventArgs e)
		{
			if(rbSearchType_D.Checked)
			{
				cbSearchEndDay.Value  = cbSearchStartDay.Value;
			}
			else if(rbSearchType_W.Checked)
			{
				if(cbSearchStartDay.Value.DayOfWeek != System.DayOfWeek.Monday)
				{
					MessageBox.Show("�ش����ڴ� �������� �ƴմϴ�.\n�ϰ���ȸ�� �����մϴ�.", "�Ѱ� �����û ����",
						MessageBoxButtons.OK, MessageBoxIcon.Information);

					rbSearchType_D.Checked = true;
					return;
				}
				cbSearchEndDay.Value  = cbSearchStartDay.Value.AddDays(6);

			}
			else if(rbSearchType_M.Checked)
			{
				if(!cbSearchStartDay.Value.ToString("dd").Equals("01"))
				{
					MessageBox.Show("�ش����ڴ� �ش���� 1���� �ƴմϴ�.\n�ϰ���ȸ�� �����մϴ�.", "�Ѱ� �����û ����",
						MessageBoxButtons.OK, MessageBoxIcon.Information);

					rbSearchType_D.Checked = true;
					return;
				}
				cbSearchEndDay.Value  = cbSearchStartDay.Value.AddMonths(1).AddDays(-1);
			}
		
		}

		#endregion

        #region ó���޼ҵ�

        /// <summary>
        /// ��ü��� ��ȸ
        /// </summary>
        private void SearchReport()
        {
            StatusMessage("���Ϻ������ ��ȸ�մϴ�.");

			if(cbSearchMedia.SelectedValue.Equals("00"))
			{
				MessageBox.Show("��ü�� ������ �ֽʽÿ�.", "���Ϻ����",
					MessageBoxButtons.OK, MessageBoxIcon.Information);
				return;
			}

			ProgressStart();

			try
			{
				// �����͸� �ʱ�ȭ
				statisticsPgWeekModel.Init();

				statisticsPgWeekModel.SearchMediaCode    =  cbSearchMedia.SelectedValue.ToString(); 
				statisticsPgWeekModel.SearchCategoryCode =  cbSearchCategory.SelectedValue.ToString(); 
				statisticsPgWeekModel.SearchGenreCode    =  cbSearchGenre.SelectedValue.ToString(); 
				statisticsPgWeekModel.SearchStartDay     =  cbSearchStartDay.Value.ToString("yyMMdd");
				statisticsPgWeekModel.SearchEndDay  	   =  cbSearchEndDay.Value.ToString("yyMMdd");

				// �����͸𵨿� ������ ������ ��Ʈ�Ѵ�.
				if(IsNewSearchKey)
				{
					statisticsPgWeekModel.SearchKey = "";
				}
				else
				{
					statisticsPgWeekModel.SearchKey  = ebSearchKey.Text;
				}

				bool IsAvg = false;

				if(rbSearchType_C.Checked)
				{
					statisticsPgWeekModel.SearchType	  	=  "C"; // -- C:���Ⱓ
					IsAvg = false;
				}
				else if(rbSearchType_D.Checked)
				{
					statisticsPgWeekModel.SearchType	  	=  "D"; // -- D:�ϰ�
					IsAvg = false;
				}
				else if(rbSearchType_W.Checked)
				{
					statisticsPgWeekModel.SearchType	  	=  "W"; // -- W:�ְ�
					IsAvg = false;
				}
				else if(rbSearchType_M.Checked)
				{
					statisticsPgWeekModel.SearchType	  	=  "M"; // -- M:����
					IsAvg = false;
				}
				else if(rbSearchType_1WA.Checked)
				{
					statisticsPgWeekModel.SearchType	  	=  "1WA"; // --1WA:���������
					IsAvg = true;
				}
				else if(rbSearchType_1MA.Checked)
				{
					statisticsPgWeekModel.SearchType	  	=  "1MA"; // --1MA:1�������
					IsAvg = true;
				}
				else if(rbSearchType_2MA.Checked)
				{
					statisticsPgWeekModel.SearchType	  	=  "2MA"; // --2MA:2�������
					IsAvg = true;
				}
				else if(rbSearchType_3MA.Checked)
				{
					statisticsPgWeekModel.SearchType	  	=  "3MA"; // --3MA:2�������
					IsAvg = true;
				}

				keyMediaName      = "";
				keyReportEndDay   = "";

				uiPanelList.Text = "���Ϻ����"; 

				//  ��ü��� ���񽺸� ȣ���Ѵ�.
				if(IsAvg)
				{
					new StatisticsPgWeekManager(systemModel,commonModel).GetStatisticsPgWeekReportAVG(statisticsPgWeekModel);
				}
				else
				{
					new StatisticsPgWeekManager(systemModel,commonModel).GetStatisticsPgWeekReport(statisticsPgWeekModel);
				}

				if (statisticsPgWeekModel.ResultCD.Equals("0000"))
				{
					Utility.SetDataTable(statisticsPgWeekDs.ReportWeek, statisticsPgWeekModel.ReportDataSet);		
					StatusMessage(statisticsPgWeekModel.ResultCnt + "���� ��ȸ�Ǿ����ϴ�.");
				
				
					keyMediaName      = cbSearchMedia.SelectedItem.Text;
					keyCategoryName   = cbSearchCategory.SelectedItem.Text;
					keyGenreName      = cbSearchGenre.SelectedItem.Text;
					keyReportBgnDay   = cbSearchStartDay.Value.ToString("yyyy-MM-dd");
					keyReportEndDay   = cbSearchEndDay.Value.ToString("yyyy-MM-dd");
					keyReportType     = statisticsPgWeekModel.SearchType;


					uiPanelList.Text = "���Ϻ���� : " + keyMediaName;

					if(cbSearchCategory.SelectedItem.Value.ToString().Equals("00"))
					{
						uiPanelList.Text += " / ���Ϻ� ��ü" ;
					}
					else
					{
						uiPanelList.Text += " / " + keyCategoryName ;

						if(cbSearchGenre.SelectedItem.Value.ToString().Equals("00"))
						{
							uiPanelList.Text += " ��ü" ;
						}
						else
						{
							uiPanelList.Text += " " + keyGenreName;
						}
					}

					uiPanelList.Text += " / " + keyReportBgnDay + " ~ " + keyReportEndDay;
									
					canPrint = true;

				}
			}
			catch(FrameException fe)
			{
				FrameSystem.showMsgForm("���Ϻ���� ��ȸ����", new string[] {fe.ErrCode, fe.ResultMsg});
			}
			catch(Exception ex)
			{
				FrameSystem.showMsgForm("���Ϻ���� ��ȸ����",new string[] {"",ex.Message});
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

				int ColMax  = 4; // �÷���   				

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
				EndCol = GetColumnIndex(ColMax);

				xlApp = new Excel.Application();
				oWB = (Excel._Workbook)(xlApp.Workbooks.Add(Missing.Value));
				oSheet = (Excel._Worksheet)oWB.ActiveSheet;


				// Ÿ��Ʋ �ۼ�
				string Title = "";
				if(cbSearchCategory.SelectedItem.Value.ToString().Equals("00"))
				{
					Title += "���Ϻ� ��ü" ;
				}
				else
				{
					Title += keyCategoryName ;

					if(cbSearchGenre.SelectedItem.Value.ToString().Equals("00"))
					{
						Title += " ��ü" ;
					}
					else
					{
						Title += " " + keyGenreName;
					}
				}

				Title += " ���" ;
			
				oSheet.Cells[TitleRow,1] = Title;
				oRng = oSheet.get_Range(StartCol+Convert.ToString(TitleRow), TitleCol+Convert.ToString(TitleRow));
				oRng.Merge(true);
				oRng.Font.Bold = true;
				oRng.Font.Size = 16;
				oRng.VerticalAlignment   = Excel.XlVAlign.xlVAlignCenter;			
				oRng.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;		

				oSheet.Cells[ConditionRow+CondCount,1] = "�Ⱓ";
				oRng = oSheet.get_Range("B"+Convert.ToString(ConditionRow+CondCount), TitleCol+Convert.ToString(ConditionRow+CondCount));
				oRng.Merge(true);
				string ReportType =  keyReportBgnDay + " ~ " + keyReportEndDay;
				if(keyReportType.Equals("C"))
				{
					ReportType += " (�ѱⰣ)";
				}
				else if(keyReportType.Equals("D"))
				{
					ReportType += " (�ϰ�)";
				}
				else if(keyReportType.Equals("W"))
				{
					ReportType += " (�ְ�)";
				}
				else if(keyReportType.Equals("M"))
				{
					ReportType += " (����)";
				}
				else if(rbSearchType_1WA.Checked)
				{
					ReportType += " (���������)";
				}
				else if(rbSearchType_1MA.Checked)
				{
					ReportType += " (1�������)";
				}
				else if(rbSearchType_2MA.Checked)
				{
					ReportType += " (2�������)";
				}
				else if(rbSearchType_3MA.Checked)
				{
					ReportType += " (3�������)";
				}

				oSheet.Cells[ConditionRow+CondCount,2] = ReportType;
				CondCount++;

				// ���Ǻ� �׵θ�
				oRng = oSheet.get_Range(StartCol+Convert.ToString(ConditionRow), TitleCol+Convert.ToString(ConditionRow+(CondCount-1)));
				oRng.VerticalAlignment   = Excel.XlVAlign.xlVAlignCenter;			
				oRng.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;		
				oRng.Borders.LineStyle =  Excel.XlLineStyle.xlContinuous;	// �׵θ��Ӽ� �Ʒ��� �Ǽ�
				oRng.Borders.Weight    = Excel.XlBorderWeight.xlThin;		// �׵θ��Ӽ� �Ʒ��� ���¼�


				// ��� ���� �ۼ�
				HeaderCount = 1;
				oSheet.Cells[HeaderRow,HeaderCount++] = "����"; 
				oRng = oSheet.get_Range(GetColumnIndex(1)+Convert.ToString(HeaderRow), GetColumnIndex(2)+Convert.ToString(HeaderRow));
				oRng.Merge(true);
				HeaderCount++;

				oSheet.Cells[HeaderRow,HeaderCount++] = "�����";
				oSheet.Cells[HeaderRow,HeaderCount++] = "%";
				oSheet.Cells[HeaderRow,HeaderCount++] = "";

				oRng = oSheet.get_Range(StartCol+Convert.ToString(HeaderRow), EndCol+Convert.ToString(HeaderRow)); // ����� ����
				oRng.Font.Bold           = true;							// ��Ʈ ����
				oRng.VerticalAlignment   = Excel.XlVAlign.xlVAlignCenter;	// �����߾�����
				oRng.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;	// �����߾�����	 
				oRng.Interior.Color      = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.CornflowerBlue);   //�� ���� 
				oRng.Font.Color          = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.White);	  //�ؽ�Ʈ��			
				

				DataCount = 0;

				// ������ ����
				for (int inx =0; inx < statisticsPgWeekDs.ReportWeek.Rows.Count; inx++)
				{

					DataRow Row = statisticsPgWeekDs.ReportWeek.Rows[inx];			

					int ColCnt = 1;
					
					oSheet.Cells[DataRow+DataCount,ColCnt++] = Row["WeekName"].ToString();						// 1  ����
					oSheet.Cells[DataRow+DataCount,ColCnt++] = Convert.ToInt32(Row["PgCnt"].ToString());		// 2  �����
					oSheet.Cells[DataRow+DataCount,ColCnt++] = Convert.ToDecimal(Row["PgRate"].ToString());		// 3  %
					oSheet.Cells[DataRow+DataCount,ColCnt++] = Row["RateBar"].ToString();						// 4  �׷���

					DataCount++;
				}

				DataCount--;


				// ������ �ۼ�
				oRng = oSheet.get_Range(StartCol+Convert.ToString(HeaderRow), EndCol+Convert.ToString(DataRow+DataCount));	// �������� ����
				oRng.EntireColumn.AutoFit();					// �������� ũ�⿡ ���� ����ũ�� ����
				oRng.Borders.LineStyle =  Excel.XlLineStyle.xlContinuous;	// �׵θ��Ӽ� �Ʒ��� �Ǽ�
				oRng.Borders.Weight    = Excel.XlBorderWeight.xlThin;		// �׵θ��Ӽ� �Ʒ��� ���¼�

				// ����� �����Ϳ� ��Ÿ�� ����
				oRng = oSheet.get_Range(GetColumnIndex(2)+Convert.ToString(DataRow), GetColumnIndex(2)+Convert.ToString(DataRow+DataCount));	// �������� ����
				oRng.NumberFormatLocal = "#,##0";

				// % ��Ÿ�� ����
				oRng = oSheet.get_Range(GetColumnIndex(3)+Convert.ToString(DataRow), GetColumnIndex(3)+Convert.ToString(DataRow+DataCount));	// �������� ����
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

		private void grdExReport_DrawGridArea(object sender, Janus.Windows.GridEX.DrawGridAreaEventArgs e)
		{
			if(e.Column.Key=="RateBar")
			{

				//first, draw the background:
				e.Graphics.FillRectangle(e.BackBrush,e.Bounds);

				//Now, draw the percent bar:
				if(e.Row.Cells["PgRate"].Value != null)
				{
					// �����͸� ��������
					float percentValue = (float) e.Row.Cells["PgRate"].Value;

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
