// ===============================================================================
//
// AdHitStatusControl.cs
//
// ���� ��û��Ȳ ���� ��Ʈ���� �����մϴ�. 
//
// ===============================================================================
// Release history
// 2007.10.08 RH.Jung ������½� �̿��ڼ� �� Reach�� ǥ�þ���
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
using Excel = Microsoft.Office.Interop.Excel; // ���� ����
using System.Reflection;

namespace AdManagerClient
{
    /// <summary>
    /// ���� ��û��Ȳ ���� ���� ��Ʈ��
    /// </summary>
    public class AdHitStatusControl : System.Windows.Forms.UserControl, IUserControl
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
        AdHitStatusModel adHitStatusModel  = new AdHitStatusModel();	// ���α׷��� �����û�����

        // ȭ��ó���� ����
		bool IsNewSearchKey		  = true;					// �˻����Է� ����
		bool canRead			  = false;

        // Key ������
		string keyMediaName    = "";
		string keyReportDay = "";
        private Janus.Windows.EditControls.UIComboBox cbSearchAdType;

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
        private Janus.Windows.EditControls.UIComboBox cbSearchMedia;
        private System.Windows.Forms.Label label1;
        private Janus.Windows.GridEX.EditControls.EditBox editBox1;
        private System.Windows.Forms.Panel panMenuSchedule;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel3;
		private Janus.Windows.EditControls.UIButton btnExcel;
		private AdManagerClient.AdHitStatusDs adHitStatusDs;
		private System.Data.DataView dvReport;
		private Janus.Windows.CalendarCombo.CalendarCombo cbSearchDay;
		private Janus.Windows.GridEX.EditControls.EditBox ebSearchKey;
		private Janus.Windows.GridEX.GridEX grdExRepoert;
		private Janus.Windows.GridEX.GridEX grdExReport;
		private Janus.Windows.EditControls.UIComboBox cbSearchRap;
		private Janus.Windows.EditControls.UIComboBox cbSearchAgency;
		private Janus.Windows.UI.Dock.UIPanelManager uiPM;
        private System.ComponentModel.IContainer components;

        public AdHitStatusControl()
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AdHitStatusControl));
			this.uiPM = new Janus.Windows.UI.Dock.UIPanelManager(this.components);
			this.uiPanelChoiceAdSchedule = new Janus.Windows.UI.Dock.UIPanelGroup();
			this.uiPanelSearch = new Janus.Windows.UI.Dock.UIPanel();
			this.uiPanelSearchContainer = new Janus.Windows.UI.Dock.UIPanelInnerContainer();
			this.pnlSearch = new System.Windows.Forms.Panel();
			this.cbSearchAdType = new Janus.Windows.EditControls.UIComboBox();
			this.cbSearchAgency = new Janus.Windows.EditControls.UIComboBox();
			this.cbSearchRap = new Janus.Windows.EditControls.UIComboBox();
			this.ebSearchKey = new Janus.Windows.GridEX.EditControls.EditBox();
			this.cbSearchDay = new Janus.Windows.CalendarCombo.CalendarCombo();
			this.cbSearchMedia = new Janus.Windows.EditControls.UIComboBox();
			this.btnSearch = new Janus.Windows.EditControls.UIButton();
			this.btnExcel = new Janus.Windows.EditControls.UIButton();
			this.uiPanelList = new Janus.Windows.UI.Dock.UIPanel();
			this.uiPanelListContainer = new Janus.Windows.UI.Dock.UIPanelInnerContainer();
			this.grdExReport = new Janus.Windows.GridEX.GridEX();
			this.dvReport = new System.Data.DataView();
			this.adHitStatusDs = new AdManagerClient.AdHitStatusDs();
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
			((System.ComponentModel.ISupportInitialize)(this.grdExReport)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.dvReport)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.adHitStatusDs)).BeginInit();
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
			this.uiPM.AddDockPanelInfo(new System.Guid("8e3140b9-bfcb-43af-b84e-2ee6cddf51dc"), new System.Guid("b162c53a-7940-47cd-a0ed-9762c59c50d8"), 611, true);
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
			this.uiPanelChoiceAdSchedule.Text = "���� ��û��Ȳ";
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
			this.pnlSearch.Controls.Add(this.cbSearchAdType);
			this.pnlSearch.Controls.Add(this.cbSearchAgency);
			this.pnlSearch.Controls.Add(this.cbSearchRap);
			this.pnlSearch.Controls.Add(this.ebSearchKey);
			this.pnlSearch.Controls.Add(this.cbSearchDay);
			this.pnlSearch.Controls.Add(this.cbSearchMedia);
			this.pnlSearch.Controls.Add(this.btnSearch);
			this.pnlSearch.Controls.Add(this.btnExcel);
			this.pnlSearch.Dock = System.Windows.Forms.DockStyle.Fill;
			this.pnlSearch.Location = new System.Drawing.Point(0, 0);
			this.pnlSearch.Name = "pnlSearch";
			this.pnlSearch.Size = new System.Drawing.Size(1008, 38);
			this.pnlSearch.TabIndex = 3;
			this.pnlSearch.Paint += new System.Windows.Forms.PaintEventHandler(this.pnlSearch_Paint);
			// 
			// cbSearchAdType
			// 
			this.cbSearchAdType.ComboStyle = Janus.Windows.EditControls.ComboStyle.DropDownList;
			this.cbSearchAdType.Location = new System.Drawing.Point(381, 7);
			this.cbSearchAdType.Name = "cbSearchAdType";
			this.cbSearchAdType.Size = new System.Drawing.Size(89, 21);
			this.cbSearchAdType.TabIndex = 15;
			this.cbSearchAdType.Text = "��������";
			this.cbSearchAdType.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
			// 
			// cbSearchAgency
			// 
			this.cbSearchAgency.ComboStyle = Janus.Windows.EditControls.ComboStyle.DropDownList;
			this.cbSearchAgency.Location = new System.Drawing.Point(254, 7);
			this.cbSearchAgency.Name = "cbSearchAgency";
			this.cbSearchAgency.Size = new System.Drawing.Size(120, 21);
			this.cbSearchAgency.TabIndex = 3;
			this.cbSearchAgency.Text = "����缱��";
			this.cbSearchAgency.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
			// 
			// cbSearchRap
			// 
			this.cbSearchRap.ComboStyle = Janus.Windows.EditControls.ComboStyle.DropDownList;
			this.cbSearchRap.Location = new System.Drawing.Point(127, 7);
			this.cbSearchRap.Name = "cbSearchRap";
			this.cbSearchRap.Size = new System.Drawing.Size(120, 21);
			this.cbSearchRap.TabIndex = 2;
			this.cbSearchRap.Text = "�̵�����";
			this.cbSearchRap.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
			// 
			// ebSearchKey
			// 
			this.ebSearchKey.Location = new System.Drawing.Point(616, 8);
			this.ebSearchKey.Name = "ebSearchKey";
			this.ebSearchKey.Size = new System.Drawing.Size(192, 21);
			this.ebSearchKey.TabIndex = 5;
			this.ebSearchKey.Text = "�˻�� �Է��ϼ���";
			this.ebSearchKey.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
			this.ebSearchKey.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
			this.ebSearchKey.TextChanged += new System.EventHandler(this.ebSearchKey_TextChanged);
			this.ebSearchKey.Click += new System.EventHandler(this.ebSearchKey_Click);
			this.ebSearchKey.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ebSearchKey_KeyDown);
			// 
			// cbSearchDay
			// 
			// 
			// 
			// 
			this.cbSearchDay.DropDownCalendar.FirstMonth = new System.DateTime(2007, 8, 1, 0, 0, 0, 0);
			this.cbSearchDay.DropDownCalendar.Name = "";
			this.cbSearchDay.DropDownCalendar.VisualStyle = Janus.Windows.CalendarCombo.VisualStyle.Office2007;
			this.cbSearchDay.Location = new System.Drawing.Point(478, 8);
			this.cbSearchDay.Name = "cbSearchDay";
			this.cbSearchDay.Size = new System.Drawing.Size(126, 21);
			this.cbSearchDay.TabIndex = 4;
			this.cbSearchDay.Value = new System.DateTime(2007, 9, 14, 0, 0, 0, 0);
			this.cbSearchDay.VisualStyle = Janus.Windows.CalendarCombo.VisualStyle.Office2007;
			// 
			// cbSearchMedia
			// 
			this.cbSearchMedia.BackColor = System.Drawing.Color.White;
			this.cbSearchMedia.ComboStyle = Janus.Windows.EditControls.ComboStyle.DropDownList;
			this.cbSearchMedia.Location = new System.Drawing.Point(8, 7);
			this.cbSearchMedia.Name = "cbSearchMedia";
			this.cbSearchMedia.Size = new System.Drawing.Size(112, 21);
			this.cbSearchMedia.TabIndex = 1;
			this.cbSearchMedia.Text = "��ü����";
			this.cbSearchMedia.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
			// 
			// btnSearch
			// 
			this.btnSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnSearch.Enabled = false;
			this.btnSearch.Location = new System.Drawing.Point(832, 6);
			this.btnSearch.Name = "btnSearch";
			this.btnSearch.Size = new System.Drawing.Size(80, 24);
			this.btnSearch.TabIndex = 6;
			this.btnSearch.Text = "�� ȸ";
			this.btnSearch.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
			this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
			// 
			// btnExcel
			// 
			this.btnExcel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnExcel.Enabled = false;
			this.btnExcel.Location = new System.Drawing.Point(917, 6);
			this.btnExcel.Name = "btnExcel";
			this.btnExcel.Size = new System.Drawing.Size(80, 24);
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
			this.uiPanelList.Location = new System.Drawing.Point(0, 66);
			this.uiPanelList.MinimumSize = new System.Drawing.Size(-1, 100);
			this.uiPanelList.Name = "uiPanelList";
			this.uiPanelList.Size = new System.Drawing.Size(1010, 611);
			this.uiPanelList.TabIndex = 4;
			this.uiPanelList.Text = "��û��Ȳ";
			// 
			// uiPanelListContainer
			// 
			this.uiPanelListContainer.BackColor = System.Drawing.SystemColors.Window;
			this.uiPanelListContainer.Controls.Add(this.grdExReport);
			this.uiPanelListContainer.Location = new System.Drawing.Point(1, 23);
			this.uiPanelListContainer.Name = "uiPanelListContainer";
			this.uiPanelListContainer.Size = new System.Drawing.Size(1008, 587);
			this.uiPanelListContainer.TabIndex = 0;
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
			this.grdExReport.FrozenColumns = 2;
			this.grdExReport.GridLineColor = System.Drawing.Color.Silver;
			this.grdExReport.GridLines = Janus.Windows.GridEX.GridLines.Vertical;
			this.grdExReport.GridLineStyle = Janus.Windows.GridEX.GridLineStyle.Solid;
			this.grdExReport.GroupByBoxInfoFormatStyle.BackColor = System.Drawing.Color.Empty;
			this.grdExReport.GroupByBoxVisible = false;
			this.grdExReport.GroupRowFormatStyle.BackColor = System.Drawing.Color.White;
			this.grdExReport.GroupRowFormatStyle.BackColorGradient = System.Drawing.Color.FromArgb(((int)(((byte)(218)))), ((int)(((byte)(228)))), ((int)(((byte)(238)))));
			this.grdExReport.GroupRowFormatStyle.FontBold = Janus.Windows.GridEX.TriState.True;
			this.grdExReport.GroupRowVisualStyle = Janus.Windows.GridEX.GroupRowVisualStyle.Outlook2003;
			this.grdExReport.GroupTotalRowFormatStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
			this.grdExReport.GroupTotalRowFormatStyle.FontBold = Janus.Windows.GridEX.TriState.True;
			this.grdExReport.GroupTotalRowFormatStyle.ForeColor = System.Drawing.SystemColors.ControlText;
			this.grdExReport.HeaderFormatStyle.TextAlignment = Janus.Windows.GridEX.TextAlignment.Center;
			this.grdExReport.HideSelection = Janus.Windows.GridEX.HideSelection.Highlight;
			this.grdExReport.Location = new System.Drawing.Point(0, 0);
			this.grdExReport.Name = "grdExReport";
			this.grdExReport.SelectedInactiveFormatStyle.BackColor = System.Drawing.Color.Gold;
			this.grdExReport.Size = new System.Drawing.Size(1008, 587);
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
			this.grdExReport.FormattingRow += new Janus.Windows.GridEX.RowLoadEventHandler(this.grdExReport_FormattingRow);
			// 
			// dvReport
			// 
			this.dvReport.Table = this.adHitStatusDs.Report;
			// 
			// adHitStatusDs
			// 
			this.adHitStatusDs.DataSetName = "AdHitStatusDs";
			this.adHitStatusDs.Locale = new System.Globalization.CultureInfo("en-US");
			this.adHitStatusDs.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
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
			// AdHitStatusControl
			// 
			this.Controls.Add(this.uiPanelChoiceAdSchedule);
			this.Font = global::AdManagerClient.Properties.Settings.Default.Font1;
			this.Name = "AdHitStatusControl";
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
			((System.ComponentModel.ISupportInitialize)(this.adHitStatusDs)).EndInit();
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

            InitButton();

			ProgressStop();
		}

        private void InitCombo()
        {
            Init_MediaCode();
			Init_RapCode();
			Init_AgencyCode();
            Common.CommonCode.Init_AdType(systemModel, commonModel, cbSearchAdType);

            InitCombo_Level();

			// �Ⱓ������ �� �������� ���Ϸ� ��Ʈ
			cbSearchDay.Value = DateTime.Now.AddDays(-1);	
		}

        private void Init_MediaCode()
        {
            // ��ü�� ��ȸ�Ѵ�.
            MediaCodeModel mediacodeModel = new MediaCodeModel();
            new MediaCodeManager(systemModel, commonModel).GetMediaCodeList(mediacodeModel);
			
            if (mediacodeModel.ResultCD.Equals("0000"))
            {
                // �����ͼ¿� ����
                Utility.SetDataTable(adHitStatusDs.Media, mediacodeModel.MediaCodeDataSet);				
            }

            // �˻������� �޺�
            this.cbSearchMedia.Items.Clear();
			
            // �޺��ڽ��� ��Ʈ�� �ڵ����� ���� Item�迭�� ����
            Janus.Windows.EditControls.UIComboBoxItem[] comboItems = new Janus.Windows.EditControls.UIComboBoxItem[mediacodeModel.ResultCnt + 1];

            comboItems[0] = new Janus.Windows.EditControls.UIComboBoxItem("��ü����","00");
			
            for(int i=0;i<mediacodeModel.ResultCnt;i++)
            {
                DataRow row = adHitStatusDs.Media.Rows[i];

                string val = row["MediaCode"].ToString();
                string txt = row["MediaName"].ToString();
                comboItems[i+1] = new Janus.Windows.EditControls.UIComboBoxItem(txt,val);
            }
            // �޺��� ��Ʈ
            this.cbSearchMedia.Items.AddRange(comboItems);
            this.cbSearchMedia.SelectedIndex = 0;

            Application.DoEvents();
        }
        
		private void Init_RapCode()
		{
			// ���� ��ȸ�Ѵ�.
			MediaRapCodeModel mediaRapCodeModel = new MediaRapCodeModel();
			new MediaRapCodeManager(systemModel, commonModel).GetMediaRapCodeList(mediaRapCodeModel);
			
			if (mediaRapCodeModel.ResultCD.Equals("0000"))
			{
				// �����ͼ¿� ����
				Utility.SetDataTable(adHitStatusDs.MediaRaps, mediaRapCodeModel.MediaRapCodeDataSet);				
			}

			// �˻������� �޺�
			this.cbSearchRap.Items.Clear();
           
			// �޺��ڽ��� ��Ʈ�� �ڵ����� ���� Item�迭�� ����
			Janus.Windows.EditControls.UIComboBoxItem[] comboItems = new Janus.Windows.EditControls.UIComboBoxItem[mediaRapCodeModel.ResultCnt + 1];

			comboItems[0] = new Janus.Windows.EditControls.UIComboBoxItem("�̵�����","00");
			
			for(int i=0;i<mediaRapCodeModel.ResultCnt;i++)
			{
				DataRow row = adHitStatusDs.MediaRaps.Rows[i];

				string val = row["RapCode"].ToString();
				string txt = row["RapName"].ToString();
				comboItems[i+1] = new Janus.Windows.EditControls.UIComboBoxItem(txt,val);
			}
			// �޺��� ��Ʈ
			this.cbSearchRap.Items.AddRange(comboItems);
			this.cbSearchRap.SelectedIndex = 0;

			Application.DoEvents();

		}

		private void Init_AgencyCode()
		{
			// ����縦 ��ȸ�Ѵ�.
			AgencyCodeModel agencyCodeModel = new AgencyCodeModel();
			new AgencyCodeManager(systemModel, commonModel).GetAgencyCodeList(agencyCodeModel);
			
			if (agencyCodeModel.ResultCD.Equals("0000"))
			{
				// �����ͼ¿� ����
				Utility.SetDataTable(adHitStatusDs.Agencys, agencyCodeModel.AgencyCodeDataSet);				
			}

			// �˻������� �޺�
			this.cbSearchAgency.Items.Clear();
			
			// �޺��ڽ��� ��Ʈ�� �ڵ����� ���� Item�迭�� ����
			Janus.Windows.EditControls.UIComboBoxItem[] comboItems = new Janus.Windows.EditControls.UIComboBoxItem[agencyCodeModel.ResultCnt + 1];

			comboItems[0] = new Janus.Windows.EditControls.UIComboBoxItem("����缱��","00");
			
			for(int i=0;i<agencyCodeModel.ResultCnt;i++)
			{
				DataRow row = adHitStatusDs.Agencys.Rows[i];

				string val = row["AgencyCode"].ToString();
				string txt = row["AgencyName"].ToString();
				comboItems[i+1] = new Janus.Windows.EditControls.UIComboBoxItem(txt,val);
			}
			
			// �޺��� ��Ʈ
			this.cbSearchAgency.Items.AddRange(comboItems);
			this.cbSearchAgency.SelectedIndex = 0;

			Application.DoEvents();
		}

		private void InitCombo_Level()
		{
			if(commonModel.UserLevel == "20")
			{
				// �޺��Ƚ�						
				cbSearchMedia.SelectedValue = commonModel.MediaCode;			
				cbSearchMedia.ReadOnly = true;				            
			}
			else
			{
				for(int i=0;i < adHitStatusDs.Media.Rows.Count;i++)
				{
					DataRow row = adHitStatusDs.Media.Rows[i];					
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
			if (commonModel.UserLevel == "30")
			{
				cbSearchRap.SelectedValue = commonModel.RapCode;
				cbSearchRap.ReadOnly = true;
			}
			if (commonModel.UserLevel == "40")
			{
				cbSearchAgency.SelectedValue = commonModel.AgencyCode;
				cbSearchAgency.ReadOnly = true;
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

		#endregion

        #region ó���޼ҵ�

        /// <summary>
        /// �Ϻ���û�� ��ȸ
        /// </summary>
		private void SearchReport()
      {
			StatusMessage("���� ��û��Ȳ�� ��ȸ�մϴ�.");

			if(cbSearchMedia.SelectedValue.Equals("00"))
			{
				MessageBox.Show("��ü�� ������ �ֽʽÿ�.", "���� ��û��Ȳ",MessageBoxButtons.OK, MessageBoxIcon.Information);
				return;
			}

			ProgressStart();

			try
			{
				// �����͸� �ʱ�ȭ
				adHitStatusModel.Init();

				if(IsNewSearchKey)
				{
					adHitStatusModel.SearchKey = "";
				}
				else
				{
					adHitStatusModel.SearchKey  = ebSearchKey.Text;
				}

				adHitStatusModel.SearchMediaCode    =  cbSearchMedia.SelectedValue.ToString(); 
				adHitStatusModel.SearchRapCode      =  cbSearchRap.SelectedValue.ToString(); 
				adHitStatusModel.SearchAgencyCode   =  cbSearchAgency.SelectedValue.ToString(); 
				adHitStatusModel.SearchDay  	    =  cbSearchDay.Value.ToString("yyMMdd");
                adHitStatusModel.SearchAdType       =  cbSearchAdType.SelectedValue.ToString(); 


				keyMediaName   = "";
				keyReportDay   = "";

				uiPanelList.Text = "��û��Ȳ"; 

				//  �Ϻ� �����û������ȸ ���񽺸� ȣ���Ѵ�.
				new AdHitStatusManager(systemModel,commonModel).GetAdHitStatus(adHitStatusModel);

				if (adHitStatusModel.ResultCD.Equals("0000"))
				{
					Utility.SetDataTable(adHitStatusDs.Report, adHitStatusModel.ReportDataSet);		
					StatusMessage(adHitStatusModel.ResultCnt + "���� ��ȸ�Ǿ����ϴ�.");
				
					keyMediaName      = cbSearchMedia.SelectedItem.Text;
					keyReportDay   = cbSearchDay.Value.ToString("yyyy-MM-dd");

					uiPanelList.Text = "���� ��û��Ȳ : " + keyMediaName + " / " + keyReportDay;
										
					canPrint = true;

				}
			}
			catch(FrameException fe)
			{
				FrameSystem.showMsgForm("���� ��û��Ȳ ��ȸ����", new string[] {fe.ErrCode, fe.ResultMsg});
			}
			catch(Exception ex)
			{
				FrameSystem.showMsgForm("���� ��û��Ȳ ��ȸ����",new string[] {"",ex.Message});
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

				int ColMax  = 17; // �÷���   				

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
				oSheet.Cells[TitleRow,1] = "���� ��û��Ȳ";
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
				oRng.VerticalAlignment   = Excel.XlVAlign.xlVAlignCenter;			
				oRng.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;		
				oRng.Borders.LineStyle =  Excel.XlLineStyle.xlContinuous;	// �׵θ��Ӽ� �Ʒ��� �Ǽ�
				oRng.Borders.Weight    = Excel.XlBorderWeight.xlThin;		// �׵θ��Ӽ� �Ʒ��� ���¼�


				// ��� ���� �ۼ�
				HeaderCount = 1;
				oSheet.Cells[HeaderRow,HeaderCount++] = "�����ȣ";
				oSheet.Cells[HeaderRow,HeaderCount++] = "�����";
				oSheet.Cells[HeaderRow,HeaderCount++] = "���Ͻ�û��";
				oSheet.Cells[HeaderRow,HeaderCount++] = "������û��";
				oSheet.Cells[HeaderRow,HeaderCount++] = "�����û��(���)";
				oSheet.Cells[HeaderRow,HeaderCount++] = "�����û��(����)";
				oSheet.Cells[HeaderRow,HeaderCount++] = "�����û��(����)";
				oSheet.Cells[HeaderRow,HeaderCount++] = "���(�������)����";
				oSheet.Cells[HeaderRow,HeaderCount++] = "���������";
				oSheet.Cells[HeaderRow,HeaderCount++] = "����������";
				oSheet.Cells[HeaderRow,HeaderCount++] = "����������";
				oSheet.Cells[HeaderRow,HeaderCount++] = "�����ϼ�";
				oSheet.Cells[HeaderRow,HeaderCount++] = "����ϼ�";
				oSheet.Cells[HeaderRow,HeaderCount++] = "�����ϼ�";
				oSheet.Cells[HeaderRow,HeaderCount++] = "�������";
				oSheet.Cells[HeaderRow,HeaderCount++] = "�����";
				oSheet.Cells[HeaderRow,HeaderCount++] = "������";


				oRng = oSheet.get_Range(StartCol+Convert.ToString(HeaderRow), EndCol+Convert.ToString(HeaderRow)); // ����� ����
				oRng.Font.Bold           = true;							// ��Ʈ ����
				oRng.VerticalAlignment   = Excel.XlVAlign.xlVAlignCenter;	// �����߾�����
				oRng.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;	// �����߾�����	 
				oRng.Interior.Color      = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.CornflowerBlue);   //�� ���� 
				oRng.Font.Color          = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.White);	  //�ؽ�Ʈ��			
				

				DataCount = 0;
				// ������ ����
				for (int inx =0; inx < adHitStatusDs.Report.Rows.Count; inx++)
				{

					DataRow Row = adHitStatusDs.Report.Rows[inx];			

					int ColCnt = 1;
					oSheet.Cells[DataRow+DataCount,ColCnt++] = Convert.ToInt32(Row["ItemNo"].ToString());		// 1  �����ȣ
					oSheet.Cells[DataRow+DataCount,ColCnt++] = Row["ItemName"].ToString();							// 2  �����
					oSheet.Cells[DataRow+DataCount,ColCnt++] = Convert.ToInt32(Row["AdCnt"].ToString());		// 3  ���Ͻ�û��
					oSheet.Cells[DataRow+DataCount,ColCnt++] = Convert.ToInt32(Row["TotAdCnt"].ToString());	// 4  ������û��
					oSheet.Cells[DataRow+DataCount,ColCnt++] = Convert.ToInt32(Row["ExpCntExcu"].ToString());	// 5  �����û��(���)
					oSheet.Cells[DataRow+DataCount,ColCnt++] = Convert.ToInt32(Row["ExpCntReal"].ToString());	// 6  �����û��(����)
					oSheet.Cells[DataRow+DataCount,ColCnt++] = Convert.ToInt32(Row["ExpCntDay"].ToString());	// 7  �����û��(����)
					oSheet.Cells[DataRow+DataCount,ColCnt++] = Convert.ToInt32(Row["ContractAmt"].ToString());	// 8  ����û��
					oSheet.Cells[DataRow+DataCount,ColCnt++] = Utility.reConvertDate(Row["ExcuteStartDay"].ToString());		// 9  ���������
					oSheet.Cells[DataRow+DataCount,ColCnt++] = Utility.reConvertDate(Row["ExcuteEndDay"].ToString());		// 10  ����������
					oSheet.Cells[DataRow+DataCount,ColCnt++] = Utility.reConvertDate(Row["RealEndDay"].ToString());			// 11 ����������
					oSheet.Cells[DataRow+DataCount,ColCnt++] = Convert.ToInt32(Row["ExDays"].ToString());		// 12 �����ϼ�
					oSheet.Cells[DataRow+DataCount,ColCnt++] = Convert.ToInt32(Row["TotDaysExcu"].ToString());	// 13 ����ϼ�
					oSheet.Cells[DataRow+DataCount,ColCnt++] = Convert.ToInt32(Row["TotDaysReal"].ToString());	// 14 �����ϼ�
					oSheet.Cells[DataRow+DataCount,ColCnt++] = Row["ContractName"].ToString();					// 15 �������
					oSheet.Cells[DataRow+DataCount,ColCnt++] = Row["AgencyName"].ToString();					// 16 �����
					oSheet.Cells[DataRow+DataCount,ColCnt++] = Row["AdvertiserName"].ToString();				// 17 ������

					DataCount++;
				}

				DataCount--;


				// ��������� �� �߾�����
				oRng = oSheet.get_Range(GetColumnIndex(9)+Convert.ToString(DataRow), GetColumnIndex(11)+Convert.ToString(DataRow+DataCount)); // ����
				oRng.VerticalAlignment   = Excel.XlVAlign.xlVAlignCenter;	// �����߾�����
				oRng.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;	// �����߾�����	 

				// ������ �ۼ�
				oRng = oSheet.get_Range(StartCol+Convert.ToString(HeaderRow), EndCol+Convert.ToString(DataRow+DataCount));	// �������� ����
				//				oRng.set_Value(Missing.Value, items);			// �������� ��Ʈ
				oRng.EntireColumn.AutoFit();					// �������� ũ�⿡ ���� ����ũ�� ����
				oRng.Borders.LineStyle =  Excel.XlLineStyle.xlContinuous;	// �׵θ��Ӽ� �Ʒ��� �Ǽ�
				oRng.Borders.Weight    = Excel.XlBorderWeight.xlThin;		// �׵θ��Ӽ� �Ʒ��� ���¼�

				// ���Ͻ�û�� �� �����Ϳ� ��Ÿ�� ����
				oRng = oSheet.get_Range(GetColumnIndex(3)+Convert.ToString(DataRow), GetColumnIndex(8)+Convert.ToString(DataRow+DataCount));	// �������� ����
				oRng.NumberFormatLocal = "#,##0";

				// �����ϼ� �� �����Ϳ� ��Ÿ�� ����
				oRng = oSheet.get_Range(GetColumnIndex(12)+Convert.ToString(DataRow), GetColumnIndex(14)+Convert.ToString(DataRow+DataCount));	// �������� ����
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

		private void grdExReport_FormattingRow(object sender, Janus.Windows.GridEX.RowLoadEventArgs e)
		{
		
		}

        private void pnlSearch_Paint(object sender, PaintEventArgs e)
        {

        }

    }
}
