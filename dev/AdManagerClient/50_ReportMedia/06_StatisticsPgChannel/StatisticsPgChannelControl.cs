// ===============================================================================
//
// StatisticsPgChannelControl.cs
//
// ����������Ʈ ä����� ��Ʈ���� �����մϴ�. 
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
    /// ��������� ä����� ���� ��Ʈ��
    /// </summary>
    public class StatisticsPgChannelControl : System.Windows.Forms.UserControl, IUserControl
    {
        #region �̺�Ʈ�ڵ鷯
        public event StatusEventHandler StatusEvent;			// �����̺�Ʈ �ڵ鷯
        public event ProgressEventHandler ProgressEvent;			// ó�����̺�Ʈ �ڵ鷯
        #endregion

        #region ��������� ��ü �� ����

        // �ý��� ���� : ȭ�����
        private SystemModel systemModel = FrameSystem.oSysModel;
        private CommonModel commonModel = FrameSystem.oComModel;
        private Logger log = FrameSystem.oLog;
        private MenuPower menu = FrameSystem.oMenu;

        // �޴��ڵ� : ������ �ʿ��� ȭ�鿡 �ʿ���
        public string menuCode = "";

        // ����� ������
        StatisticsPgChannelModel statisticsPgChannelModel = new StatisticsPgChannelModel();	// ������ ī�װ������ ��

        // ȭ��ó���� ����
        bool canRead = false;

        // Key ������
        string keyMediaName = "";
        string keyReportBgnDay = "";
        string keyReportEndDay = "";
        string keyReportType = "";

        bool canPrint = false;

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
        private AdManagerClient._50_ReportMedia.ReportMediaDs statisticsPgChannelDs;
        private System.ComponentModel.IContainer components;

        public StatisticsPgChannelControl()
        {
            // �� ȣ���� Windows.Forms Form �����̳ʿ� �ʿ��մϴ�.
            InitializeComponent();



        }

        /// <summary> 
        /// ��� ���� ��� ���ҽ��� �����մϴ�.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(StatisticsPgChannelControl));
            this.uiPM = new Janus.Windows.UI.Dock.UIPanelManager(this.components);
            this.uiPanelChoiceAdSchedule = new Janus.Windows.UI.Dock.UIPanelGroup();
            this.uiPanelSearch = new Janus.Windows.UI.Dock.UIPanel();
            this.uiPanelSearchContainer = new Janus.Windows.UI.Dock.UIPanelInnerContainer();
            this.pnlSearch = new System.Windows.Forms.Panel();
            this.uiGroupBox1 = new Janus.Windows.EditControls.UIGroupBox();
            this.rbSearchType_W = new Janus.Windows.EditControls.UIRadioButton();
            this.rbSearchType_C = new Janus.Windows.EditControls.UIRadioButton();
            this.rbSearchType_M = new Janus.Windows.EditControls.UIRadioButton();
            this.rbSearchType_D = new Janus.Windows.EditControls.UIRadioButton();
            this.cbSearchStartDay = new Janus.Windows.CalendarCombo.CalendarCombo();
            this.lbSearchDate = new System.Windows.Forms.Label();
            this.cbSearchMedia = new Janus.Windows.EditControls.UIComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.cbSearchEndDay = new Janus.Windows.CalendarCombo.CalendarCombo();
            this.label11 = new System.Windows.Forms.Label();
            this.btnSearch = new Janus.Windows.EditControls.UIButton();
            this.btnExcel = new Janus.Windows.EditControls.UIButton();
            this.uiPanelList = new Janus.Windows.UI.Dock.UIPanel();
            this.uiPanelListContainer = new Janus.Windows.UI.Dock.UIPanelInnerContainer();
            this.grdExReport = new Janus.Windows.GridEX.GridEX();
            this.dvReport = new System.Data.DataView();
            this.statisticsPgChannelDs = new AdManagerClient._50_ReportMedia.ReportMediaDs();
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
            ((System.ComponentModel.ISupportInitialize)(this.uiGroupBox1)).BeginInit();
            this.uiGroupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelList)).BeginInit();
            this.uiPanelList.SuspendLayout();
            this.uiPanelListContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdExReport)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dvReport)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.statisticsPgChannelDs)).BeginInit();
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
            this.uiPM.AddDockPanelInfo(new System.Guid("afb3b7d0-aa79-443c-a1e8-3baba38f7a4d"), new System.Guid("b162c53a-7940-47cd-a0ed-9762c59c50d8"), 62, true);
            this.uiPM.AddDockPanelInfo(new System.Guid("8e3140b9-bfcb-43af-b84e-2ee6cddf51dc"), new System.Guid("b162c53a-7940-47cd-a0ed-9762c59c50d8"), 569, true);
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
            this.uiPanelChoiceAdSchedule.Text = "ä�κ� ������ ���";
            // 
            // uiPanelSearch
            // 
            this.uiPanelSearch.AllowResize = Janus.Windows.UI.InheritableBoolean.False;
            this.uiPanelSearch.CaptionVisible = Janus.Windows.UI.InheritableBoolean.False;
            this.uiPanelSearch.CloseButtonVisible = Janus.Windows.UI.InheritableBoolean.False;
            this.uiPanelSearch.InnerContainer = this.uiPanelSearchContainer;
            this.uiPanelSearch.Location = new System.Drawing.Point(0, 22);
            this.uiPanelSearch.Name = "uiPanelSearch";
            this.uiPanelSearch.Size = new System.Drawing.Size(1010, 63);
            this.uiPanelSearch.TabIndex = 4;
            this.uiPanelSearch.Text = "�˻�";
            // 
            // uiPanelSearchContainer
            // 
            this.uiPanelSearchContainer.Controls.Add(this.pnlSearch);
            this.uiPanelSearchContainer.Location = new System.Drawing.Point(1, 1);
            this.uiPanelSearchContainer.Name = "uiPanelSearchContainer";
            this.uiPanelSearchContainer.Size = new System.Drawing.Size(1008, 61);
            this.uiPanelSearchContainer.TabIndex = 0;
            // 
            // pnlSearch
            // 
            this.pnlSearch.BackColor = System.Drawing.SystemColors.Window;
            this.pnlSearch.Controls.Add(this.uiGroupBox1);
            this.pnlSearch.Controls.Add(this.cbSearchStartDay);
            this.pnlSearch.Controls.Add(this.lbSearchDate);
            this.pnlSearch.Controls.Add(this.cbSearchMedia);
            this.pnlSearch.Controls.Add(this.label6);
            this.pnlSearch.Controls.Add(this.label10);
            this.pnlSearch.Controls.Add(this.cbSearchEndDay);
            this.pnlSearch.Controls.Add(this.label11);
            this.pnlSearch.Controls.Add(this.btnSearch);
            this.pnlSearch.Controls.Add(this.btnExcel);
            this.pnlSearch.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlSearch.Location = new System.Drawing.Point(0, 0);
            this.pnlSearch.Name = "pnlSearch";
            this.pnlSearch.Size = new System.Drawing.Size(1008, 61);
            this.pnlSearch.TabIndex = 3;
            // 
            // uiGroupBox1
            // 
            this.uiGroupBox1.BorderColor = System.Drawing.Color.Transparent;
            this.uiGroupBox1.Controls.Add(this.rbSearchType_W);
            this.uiGroupBox1.Controls.Add(this.rbSearchType_C);
            this.uiGroupBox1.Controls.Add(this.rbSearchType_M);
            this.uiGroupBox1.Controls.Add(this.rbSearchType_D);
            this.uiGroupBox1.Location = new System.Drawing.Point(216, 7);
            this.uiGroupBox1.Name = "uiGroupBox1";
            this.uiGroupBox1.Size = new System.Drawing.Size(272, 24);
            this.uiGroupBox1.TabIndex = 2;
            // 
            // rbSearchType_W
            // 
            this.rbSearchType_W.Checked = true;
            this.rbSearchType_W.Location = new System.Drawing.Point(160, 0);
            this.rbSearchType_W.Name = "rbSearchType_W";
            this.rbSearchType_W.Size = new System.Drawing.Size(48, 23);
            this.rbSearchType_W.TabIndex = 2;
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
            this.rbSearchType_C.Size = new System.Drawing.Size(72, 23);
            this.rbSearchType_C.TabIndex = 2;
            this.rbSearchType_C.Text = "���ñⰣ";
            this.rbSearchType_C.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.rbSearchType_C.CheckedChanged += new System.EventHandler(this.rbSearchType_C_CheckedChanged);
            // 
            // rbSearchType_M
            // 
            this.rbSearchType_M.Location = new System.Drawing.Point(224, 0);
            this.rbSearchType_M.Name = "rbSearchType_M";
            this.rbSearchType_M.Size = new System.Drawing.Size(48, 23);
            this.rbSearchType_M.TabIndex = 2;
            this.rbSearchType_M.Text = "����";
            this.rbSearchType_M.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.rbSearchType_M.CheckedChanged += new System.EventHandler(this.rbSearchType_M_CheckedChanged);
            // 
            // rbSearchType_D
            // 
            this.rbSearchType_D.Location = new System.Drawing.Point(88, 0);
            this.rbSearchType_D.Name = "rbSearchType_D";
            this.rbSearchType_D.Size = new System.Drawing.Size(56, 23);
            this.rbSearchType_D.TabIndex = 2;
            this.rbSearchType_D.Text = "�ϰ�";
            this.rbSearchType_D.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.rbSearchType_D.CheckedChanged += new System.EventHandler(this.rbSearchType_D_CheckedChanged);
            // 
            // cbSearchStartDay
            // 
            // 
            // 
            // 
            this.cbSearchStartDay.DropDownCalendar.FirstMonth = new System.DateTime(2007, 8, 1, 0, 0, 0, 0);
            this.cbSearchStartDay.DropDownCalendar.Name = "";
            this.cbSearchStartDay.DropDownCalendar.VisualStyle = Janus.Windows.CalendarCombo.VisualStyle.Office2007;
            this.cbSearchStartDay.Location = new System.Drawing.Point(216, 31);
            this.cbSearchStartDay.Name = "cbSearchStartDay";
            this.cbSearchStartDay.Size = new System.Drawing.Size(104, 23);
            this.cbSearchStartDay.TabIndex = 3;
            this.cbSearchStartDay.Value = new System.DateTime(2007, 9, 14, 0, 0, 0, 0);
            this.cbSearchStartDay.VisualStyle = Janus.Windows.CalendarCombo.VisualStyle.Office2007;
            this.cbSearchStartDay.TextChanged += new System.EventHandler(this.cbSearchStartDay_TextChanged);
            // 
            // lbSearchDate
            // 
            this.lbSearchDate.Location = new System.Drawing.Point(128, 31);
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
            // 
            // label6
            // 
            this.label6.Location = new System.Drawing.Point(128, 7);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(80, 21);
            this.label6.TabIndex = 24;
            this.label6.Text = "����Ⱓ����";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label10
            // 
            this.label10.Location = new System.Drawing.Point(320, 31);
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
            this.cbSearchEndDay.Location = new System.Drawing.Point(352, 31);
            this.cbSearchEndDay.Name = "cbSearchEndDay";
            this.cbSearchEndDay.Size = new System.Drawing.Size(104, 23);
            this.cbSearchEndDay.TabIndex = 4;
            this.cbSearchEndDay.Value = new System.DateTime(2007, 9, 14, 0, 0, 0, 0);
            this.cbSearchEndDay.VisualStyle = Janus.Windows.CalendarCombo.VisualStyle.Office2007;
            // 
            // label11
            // 
            this.label11.Location = new System.Drawing.Point(456, 31);
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
            this.btnSearch.TabIndex = 6;
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
            this.btnExcel.TabIndex = 7;
            this.btnExcel.Text = "EXCEL ���";
            this.btnExcel.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.btnExcel.Click += new System.EventHandler(this.btnExcel_Click);
            // 
            // uiPanelList
            // 
            this.uiPanelList.BackColor = System.Drawing.SystemColors.Window;
            this.uiPanelList.CaptionStyle = Janus.Windows.UI.Dock.PanelCaptionStyle.Dark;
            this.uiPanelList.InnerContainer = this.uiPanelListContainer;
            this.uiPanelList.Location = new System.Drawing.Point(0, 89);
            this.uiPanelList.MinimumSize = new System.Drawing.Size(-1, 100);
            this.uiPanelList.Name = "uiPanelList";
            this.uiPanelList.Size = new System.Drawing.Size(1010, 588);
            this.uiPanelList.TabIndex = 4;
            this.uiPanelList.Text = "���";
            // 
            // uiPanelListContainer
            // 
            this.uiPanelListContainer.BackColor = System.Drawing.SystemColors.Window;
            this.uiPanelListContainer.Controls.Add(this.grdExReport);
            this.uiPanelListContainer.Location = new System.Drawing.Point(1, 23);
            this.uiPanelListContainer.Name = "uiPanelListContainer";
            this.uiPanelListContainer.Size = new System.Drawing.Size(1008, 564);
            this.uiPanelListContainer.TabIndex = 0;
            // 
            // grdExReport
            // 
            this.grdExReport.AllowEdit = Janus.Windows.GridEX.InheritableBoolean.False;
            this.grdExReport.AlternatingColors = true;
            this.grdExReport.BorderStyle = Janus.Windows.GridEX.BorderStyle.None;
            this.grdExReport.BuiltInTextsData = "<LocalizableData ID=\"LocalizableStrings\" Collection=\"true\"><GroupByBoxInfo>�׷�ȭ�� ��" +
    "���� �巹���ؼ� �ű�ø� �˴ϴ�</GroupByBoxInfo></LocalizableData>";
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
            this.grdExReport.GroupRowFormatStyle.BackColor = System.Drawing.Color.White;
            this.grdExReport.GroupRowFormatStyle.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.grdExReport.GroupRowVisualStyle = Janus.Windows.GridEX.GroupRowVisualStyle.Outlook2003;
            this.grdExReport.HeaderFormatStyle.TextAlignment = Janus.Windows.GridEX.TextAlignment.Center;
            this.grdExReport.HideSelection = Janus.Windows.GridEX.HideSelection.Highlight;
            this.grdExReport.Location = new System.Drawing.Point(0, 0);
            this.grdExReport.Name = "grdExReport";
            this.grdExReport.ScrollBars = Janus.Windows.GridEX.ScrollBars.Vertical;
            this.grdExReport.SelectedInactiveFormatStyle.BackColor = System.Drawing.Color.Gold;
            this.grdExReport.Size = new System.Drawing.Size(1008, 564);
            this.grdExReport.TabIndex = 8;
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
            this.dvReport.Table = this.statisticsPgChannelDs.ReportChannel;
            // 
            // statisticsPgChannelDs
            // 
            this.statisticsPgChannelDs.DataSetName = "ReportMediaDs";
            this.statisticsPgChannelDs.Locale = new System.Globalization.CultureInfo("en-US");
            this.statisticsPgChannelDs.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
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
            this.editBox1.Size = new System.Drawing.Size(100, 21);
            this.editBox1.TabIndex = 0;
            this.editBox1.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
            // 
            // StatisticsPgChannelControl
            // 
            this.Controls.Add(this.uiPanelChoiceAdSchedule);
            this.Font = new System.Drawing.Font("���� ���", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.Name = "StatisticsPgChannelControl";
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
            ((System.ComponentModel.ISupportInitialize)(this.statisticsPgChannelDs)).EndInit();
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
            if (menu.CanRead(MenuCode))
            {
                canRead = true;
            }

            ProgressStop();

            //			if(canRead)
            //			{
            //				SearchReport();
            //			}

            InitButton();
        }

        private void InitCombo()
        {
            Init_MediaCode();
            InitCombo_Level();

            // �Ⱓ�������� ������ �����Ϸ� ��Ʈ
            DateTime dt = DateTime.Now.AddDays(-7); //���� ���ϱ��� 7�������� ��Ʈ

            for (int i = 0; i < 7; i++)
            {
                if (dt.DayOfWeek == System.DayOfWeek.Monday)
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
                Utility.SetDataTable(statisticsPgChannelDs.Media, mediacodeModel.MediaCodeDataSet);
            }

            // �˻������� �޺�
            this.cbSearchMedia.Items.Clear();

            // �޺��ڽ��� ��Ʈ�� �ڵ����� ���� Item�迭�� ����
            Janus.Windows.EditControls.UIComboBoxItem[] comboItems = new Janus.Windows.EditControls.UIComboBoxItem[mediacodeModel.ResultCnt + 1];

            comboItems[0] = new Janus.Windows.EditControls.UIComboBoxItem("��ü����", "00");

            for (int i = 0; i < mediacodeModel.ResultCnt; i++)
            {
                DataRow row = statisticsPgChannelDs.Media.Rows[i];

                string val = row["MediaCode"].ToString();
                string txt = row["MediaName"].ToString();
                comboItems[i + 1] = new Janus.Windows.EditControls.UIComboBoxItem(txt, val);
            }
            // �޺��� ��Ʈ
            this.cbSearchMedia.Items.AddRange(comboItems);
            this.cbSearchMedia.SelectedIndex = 0;

            Application.DoEvents();
        }


        private void InitCombo_Level()
        {

            if (commonModel.UserLevel == "20")
            {
                cbSearchMedia.SelectedValue = commonModel.MediaCode;
                cbSearchMedia.ReadOnly = true;
            }
            else
            {
                for (int i = 0; i < statisticsPgChannelDs.Media.Rows.Count; i++)
                {
                    DataRow row = statisticsPgChannelDs.Media.Rows[i];
                    if (row["MediaCode"].ToString().Equals(FrameSystem._HANATV.ToString()))
                    {
                        cbSearchMedia.SelectedValue = FrameSystem._HANATV; // �ϳ�TV�� �⺻������ �Ѵ�.	 		
                        break;
                    }
                    else
                    {
                        cbSearchMedia.SelectedValue = "00";
                    }
                }
            }

            Application.DoEvents();
        }

        private void InitButton()
        {
            if (canRead)
            {
                btnSearch.Enabled = true;
            }

            if (canPrint)
            {
                btnExcel.Enabled = true;
            }

            grdExReport.Focus();

            Application.DoEvents();
        }

        private void DisableButton()
        {
            btnSearch.Enabled = false;
            btnExcel.Enabled = false;

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

        private void rbSearchType_C_CheckedChanged(object sender, System.EventArgs e)
        {
            if (rbSearchType_C.Checked)
            {
                cbSearchStartDay.Enabled = true;
                cbSearchEndDay.Enabled = true;
            }
        }


        private void rbSearchType_D_CheckedChanged(object sender, System.EventArgs e)
        {
            if (rbSearchType_D.Checked)
            {
                cbSearchStartDay.Enabled = true;
                cbSearchEndDay.Enabled = false;
                cbSearchEndDay.Value = cbSearchStartDay.Value;
            }
        }

        private void rbSearchType_W_CheckedChanged(object sender, System.EventArgs e)
        {
            if (rbSearchType_W.Checked)
            {
                cbSearchStartDay.Enabled = true;
                cbSearchEndDay.Enabled = false;

                DateTime dt = cbSearchStartDay.Value;

                for (int i = 0; i < 7; i++)
                {
                    if (dt.DayOfWeek == System.DayOfWeek.Monday)
                    {
                        break;
                    }
                    dt = dt.AddDays(-1);
                }
                cbSearchStartDay.Value = dt;
                cbSearchEndDay.Value = cbSearchStartDay.Value.AddDays(6);
            }
        }


        private void rbSearchType_M_CheckedChanged(object sender, System.EventArgs e)
        {
            if (rbSearchType_M.Checked)
            {
                cbSearchStartDay.Enabled = true;
                cbSearchEndDay.Enabled = false;

                cbSearchStartDay.Text = cbSearchStartDay.Value.ToString("yyyy-MM-01");
                cbSearchEndDay.Value = cbSearchStartDay.Value.AddMonths(1).AddDays(-1);
            }
        }


        private void cbSearchStartDay_TextChanged(object sender, System.EventArgs e)
        {
            if (rbSearchType_D.Checked)
            {
                cbSearchEndDay.Value = cbSearchStartDay.Value;
            }
            else if (rbSearchType_W.Checked)
            {
                if (cbSearchStartDay.Value.DayOfWeek != System.DayOfWeek.Monday)
                {
                    MessageBox.Show("�ش����ڴ� �������� �ƴմϴ�.\n�ϰ���ȸ�� �����մϴ�.", "ä�����",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);

                    rbSearchType_D.Checked = true;
                    return;
                }
                cbSearchEndDay.Value = cbSearchStartDay.Value.AddDays(6);

            }
            else if (rbSearchType_M.Checked)
            {
                if (!cbSearchStartDay.Value.ToString("dd").Equals("01"))
                {
                    MessageBox.Show("�ش����ڴ� �ش���� 1���� �ƴմϴ�.\n�ϰ���ȸ�� �����մϴ�.", "ä�����",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);

                    rbSearchType_D.Checked = true;
                    return;
                }
                cbSearchEndDay.Value = cbSearchStartDay.Value.AddMonths(1).AddDays(-1);
            }

        }

        #endregion

        #region ó���޼ҵ�

        /// <summary>
        /// ��ü��� ��ȸ
        /// </summary>
        private void SearchReport()
        {
            StatusMessage("ä������� ��ȸ�մϴ�.");

            if (cbSearchMedia.SelectedValue.Equals("00"))
            {
                MessageBox.Show("��ü�� ������ �ֽʽÿ�.", "ä�����", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            ProgressStart();

            try
            {
                // �����͸� �ʱ�ȭ
                statisticsPgChannelModel.Init();
                statisticsPgChannelDs.ReportChannel.Clear();

                statisticsPgChannelModel.SearchMediaCode = cbSearchMedia.SelectedValue.ToString();
                statisticsPgChannelModel.SearchStartDay = cbSearchStartDay.Value.ToString("yyMMdd");
                statisticsPgChannelModel.SearchEndDay = cbSearchEndDay.Value.ToString("yyMMdd");

                if (rbSearchType_C.Checked)
                {
                    statisticsPgChannelModel.SearchType = "B"; // -- B:���ñⰣ
                }
                else if (rbSearchType_D.Checked)
                {
                    statisticsPgChannelModel.SearchType = "D"; // -- D:�ϰ�
                }
                else if (rbSearchType_W.Checked)
                {
                    statisticsPgChannelModel.SearchType = "W"; // -- W:�ְ�
                }
                else if (rbSearchType_M.Checked)
                {
                    statisticsPgChannelModel.SearchType = "M"; // -- M:����
                }

                keyMediaName = "";
                keyReportBgnDay = "";
                keyReportEndDay = "";

                uiPanelList.Text = "ä�����";

                //  ��ü��� ���񽺸� ȣ���Ѵ�.
                new StatisticsPgChannelManager(systemModel, commonModel).GetStatisticsPgChannelReport(statisticsPgChannelModel);

                if (statisticsPgChannelModel.ResultCD.Equals("0000"))
                {
                    Utility.SetDataTableFast(statisticsPgChannelDs.ReportChannel, statisticsPgChannelModel.ReportDataSet);
                    StatusMessage(statisticsPgChannelModel.ResultCnt + "���� ��ȸ�Ǿ����ϴ�.");

                    keyMediaName = cbSearchMedia.SelectedItem.Text;
                    keyReportBgnDay = cbSearchStartDay.Value.ToString("yyyy-MM-dd");
                    keyReportEndDay = cbSearchEndDay.Value.ToString("yyyy-MM-dd");
                    keyReportType = statisticsPgChannelModel.SearchType;

                    uiPanelList.Text = "ä����� : " + keyMediaName + " / " + keyReportBgnDay + " ~ " + keyReportEndDay;

                    canPrint = true;

                }
            }
            catch (FrameException fe)
            {
                FrameSystem.showMsgForm("ä����� ��ȸ����", new string[] { fe.ErrCode, fe.ResultMsg });
            }
            catch (Exception ex)
            {
                FrameSystem.showMsgForm("ä����� ��ȸ����", new string[] { "", ex.Message });
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
                ea.Message = strMessage;
                StatusEvent(this, ea);
            }
        }

        private void ProgressStart()
        {
            if (ProgressEvent != null)
            {
                ProgressEventArgs ea = new ProgressEventArgs();
                ea.Type = ProgressEventArgs.Start;
                ProgressEvent(this, ea);
            }
        }

        private void ProgressStop()
        {
            if (ProgressEvent != null)
            {
                ProgressEventArgs ea = new ProgressEventArgs();
                ea.Type = ProgressEventArgs.Stop;
                ProgressEvent(this, ea);
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
            Excel.Application xlApp = null;
            Excel._Workbook oWB = null;
            Excel._Worksheet oSheet = null;
            Excel.Range oRng = null;

            try
            {

                ProgressStart();

                int ColMax = 10; // �÷���   				

                int TitleRow = 1;
                int ConditionRow = 2;
                int HeaderRow = 4;
                int DataRow = 5;
                string StartCol = "A";
                string EndCol = "";
                string TitleCol = "J";
                int DataCount = 0;
                int CondCount = 0;
                int HeaderCount = 0;

                // ������ �÷��� �ε�������
                EndCol = GetColumnIndex(ColMax);

                xlApp = new Excel.Application();
                oWB = (Excel._Workbook)(xlApp.Workbooks.Add(Missing.Value));
                oSheet = (Excel._Worksheet)oWB.ActiveSheet;


                // Ÿ��Ʋ �ۼ�
                oSheet.Cells[TitleRow, 1] = "ä�����";
                oRng = oSheet.get_Range(StartCol + Convert.ToString(TitleRow), TitleCol + Convert.ToString(TitleRow));
                oRng.Merge(true);
                oRng.Font.Bold = true;
                oRng.Font.Size = 16;
                oRng.VerticalAlignment = Excel.XlVAlign.xlVAlignCenter;
                oRng.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;

                // �������� �ۼ�
                oSheet.Cells[ConditionRow + CondCount, 1] = "�Ⱓ";
                oRng = oSheet.get_Range("B" + Convert.ToString(ConditionRow + CondCount), TitleCol + Convert.ToString(ConditionRow + CondCount));
                oRng.Merge(true);

                string ReportType = keyReportBgnDay + " ~ " + keyReportEndDay;
                if (keyReportType.Equals("B")) ReportType += " (���ñⰣ)";
                else if (keyReportType.Equals("D")) ReportType += " (�ϰ�)";
                else if (keyReportType.Equals("W")) ReportType += " (�ְ�)";
                else if (keyReportType.Equals("M")) ReportType += " (����)";

                oSheet.Cells[ConditionRow + CondCount, 2] = ReportType;
                CondCount++;

                // ���Ǻ� �׵θ�
                oRng = oSheet.get_Range(StartCol + Convert.ToString(ConditionRow), TitleCol + Convert.ToString(ConditionRow + (CondCount - 1)));
                oRng.VerticalAlignment = Excel.XlVAlign.xlVAlignCenter;
                oRng.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                oRng.Borders.LineStyle = Excel.XlLineStyle.xlContinuous;	// �׵θ��Ӽ� �Ʒ��� �Ǽ�
                oRng.Borders.Weight = Excel.XlBorderWeight.xlThin;		// �׵θ��Ӽ� �Ʒ��� ���¼�


                // ��� ���� �ۼ�
                HeaderCount = 1;
                oSheet.Cells[HeaderRow, HeaderCount++] = "����";
                //oRng = oSheet.get_Range(GetColumnIndex(1)+Convert.ToString(HeaderRow), GetColumnIndex(2)+Convert.ToString(HeaderRow));
                //oRng.Merge(true);
                //HeaderCount++;

                oSheet.Cells[HeaderRow, HeaderCount++] = "���α׷�";
                oSheet.Cells[HeaderRow, HeaderCount++] = "ī�װ�";
                oSheet.Cells[HeaderRow, HeaderCount++] = "�޴�";
                oSheet.Cells[HeaderRow, HeaderCount++] = "Hit";
                oSheet.Cells[HeaderRow, HeaderCount++] = "%";
                oSheet.Cells[HeaderRow, HeaderCount++] = "Hit(�̾��)";
                oSheet.Cells[HeaderRow, HeaderCount++] = "%";
                oSheet.Cells[HeaderRow, HeaderCount++] = "Hit(����)";
                oSheet.Cells[HeaderRow, HeaderCount++] = "%";

                oRng = oSheet.get_Range(StartCol + Convert.ToString(HeaderRow), EndCol + Convert.ToString(HeaderRow)); // ����� ����
                oRng.Font.Bold = true;							// ��Ʈ ����
                oRng.VerticalAlignment = Excel.XlVAlign.xlVAlignCenter;	// �����߾�����
                oRng.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;	// �����߾�����	 
                oRng.Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.CornflowerBlue);   //�� ���� 
                oRng.Font.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.White);	  //�ؽ�Ʈ��			


                DataCount = 0;

                // ������ ����
                for (int inx = 0; inx < statisticsPgChannelDs.ReportChannel.Rows.Count; inx++)
                {
                    DataRow Row = statisticsPgChannelDs.ReportChannel.Rows[inx];

                    int ColCnt = 1;

                    oSheet.Cells[DataRow + DataCount, ColCnt++] = Convert.ToInt32(Row["Rank"].ToString());			// 1  ����
                    oSheet.Cells[DataRow + DataCount, ColCnt++] = Row["ProgramNm"].ToString();						// 2  ���α׷���
                    oSheet.Cells[DataRow + DataCount, ColCnt++] = Row["CategoryNm"].ToString();					// 3  ī�װ�
                    oSheet.Cells[DataRow + DataCount, ColCnt++] = Row["GenreNm"].ToString();						// 4  �޴�
                    oSheet.Cells[DataRow + DataCount, ColCnt++] = Convert.ToInt32(Row["PgCnt"].ToString());		// 5  �����
                    oSheet.Cells[DataRow + DataCount, ColCnt++] = Convert.ToDecimal(Row["PgRate"].ToString());		// 6  %
                    oSheet.Cells[DataRow + DataCount, ColCnt++] = Convert.ToInt32(Row["RePlayCnt"].ToString());	// 7  �̾������
                    oSheet.Cells[DataRow + DataCount, ColCnt++] = Convert.ToDecimal(Row["RePlayRate"].ToString());	// 8  %
                    oSheet.Cells[DataRow + DataCount, ColCnt++] = Convert.ToInt32(Row["PPxCnt"].ToString());		// 9  ��������
                    oSheet.Cells[DataRow + DataCount, ColCnt++] = Convert.ToDecimal(Row["PPxRate"].ToString());	// 10  %

                    DataCount++;
                    Application.DoEvents();
                }

                DataCount--;


                // ������ �ۼ�
                oRng = oSheet.get_Range(StartCol + Convert.ToString(HeaderRow), EndCol + Convert.ToString(DataRow + DataCount));	// �������� ����
                oRng.Font.Size = 9;
                oRng.EntireColumn.AutoFit();					// �������� ũ�⿡ ���� ����ũ�� ����
                oRng.Borders.LineStyle = Excel.XlLineStyle.xlContinuous;	// �׵θ��Ӽ� �Ʒ��� �Ǽ�
                oRng.Borders.Weight = Excel.XlBorderWeight.xlThin;		// �׵θ��Ӽ� �Ʒ��� ���¼�

                oRng = oSheet.get_Range(GetColumnIndex(1) + Convert.ToString(DataRow), GetColumnIndex(1) + Convert.ToString(DataRow + DataCount));	// �������� ����
                oRng.NumberFormatLocal = "#,##0";

                // ����� �����Ϳ� ��Ÿ�� ����
                oRng = oSheet.get_Range(GetColumnIndex(5) + Convert.ToString(DataRow), GetColumnIndex(5) + Convert.ToString(DataRow + DataCount));	// �������� ����
                oRng.NumberFormatLocal = "#,##0";

                // % ��Ÿ�� ����
                oRng = oSheet.get_Range(GetColumnIndex(6) + Convert.ToString(DataRow), GetColumnIndex(6) + Convert.ToString(DataRow + DataCount));	// �������� ����
                oRng.NumberFormatLocal = "#,##0.00";

                oRng = oSheet.get_Range(GetColumnIndex(7) + Convert.ToString(DataRow), GetColumnIndex(7) + Convert.ToString(DataRow + DataCount));	// �������� ����
                oRng.NumberFormatLocal = "#,##0";

                // % ��Ÿ�� ����
                oRng = oSheet.get_Range(GetColumnIndex(8) + Convert.ToString(DataRow), GetColumnIndex(8) + Convert.ToString(DataRow + DataCount));	// �������� ����
                oRng.NumberFormatLocal = "#,##0.00";

                oRng = oSheet.get_Range(GetColumnIndex(9) + Convert.ToString(DataRow), GetColumnIndex(7) + Convert.ToString(DataRow + DataCount));	// �������� ����
                oRng.NumberFormatLocal = "#,##0";

                // % ��Ÿ�� ����
                oRng = oSheet.get_Range(GetColumnIndex(10) + Convert.ToString(DataRow), GetColumnIndex(8) + Convert.ToString(DataRow + DataCount));	// �������� ����
                oRng.NumberFormatLocal = "#,##0.00";

                xlApp.Visible = true;
                xlApp.UserControl = true;


            }
            catch (Exception ex)
            {
                if (xlApp != null)
                {
                    xlApp.Visible = true;
                    xlApp.UserControl = true;
                }
                MessageBox.Show(ex.Message);
            }
            finally
            {
                ProgressStop();
            }
        }

        private string GetColumnIndex(int ColCount)
        {
            string[] ColName = { "Z", "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y" };

            string ColumnIndex;

            // 26���� ũ��
            if (ColCount > ColName.Length)
            {
                // 2�ڸ� �ε������� 26 => Z;  27->AA
                ColumnIndex = ColName[(int)(Math.Floor((float)(ColCount / ColName.Length)))] + ColName[(int)(Math.Floor((float)(ColCount % ColName.Length)))];
            }
            else
            {
                ColumnIndex = ColName[(int)(Math.Floor((float)(ColCount % ColName.Length)))];
            }

            return ColumnIndex;
        }

        #endregion

        #region ����׷���
        private void grdExReport_DrawGridArea(object sender, Janus.Windows.GridEX.DrawGridAreaEventArgs e)
        {
            if (e.Column.Key == "RateBar")
            {

                //first, draw the background:
                e.Graphics.FillRectangle(e.BackBrush, e.Bounds);

                //Now, draw the percent bar:
                if (e.Row.Cells["PgRate"].Value != null)
                {
                    // �����͸� ��������
                    float percentValue = (float)e.Row.Cells["PgRate"].Value;

                    //calculate the rect to draw:
                    Rectangle rect = Rectangle.Inflate(e.Bounds, -1, -3);
                    rect.Width = (int)(rect.Width * (percentValue / 10));  // ��ȸ�����Ͱ� �� �����Ƿ� 10�� Ȯ���ؼ� �����ش�. 100���� �������� 10���� ����

                    //now draw the rectangle:
                    if (rect.Width > 0 && rect.Height > 0)
                    {
                        LinearGradientBrush br = new LinearGradientBrush(rect, Color.GreenYellow, Color.Green, LinearGradientMode.BackwardDiagonal);

                        e.Graphics.FillRectangle(br, rect);

                        br.Dispose();
                    }

                    e.Handled = true;
                }
            }
        }
        #endregion
    }
}
