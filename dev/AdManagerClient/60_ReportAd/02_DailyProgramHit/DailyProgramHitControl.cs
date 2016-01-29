// ===============================================================================
//
// DailyProgramHitControl.cs
//
// �Ϻ� ���α׷� ��û���� ��Ʈ���� �����մϴ�. 
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
using Excel = Microsoft.Office.Interop.Excel; // ���� ����
using System.Reflection;

namespace AdManagerClient
{
    /// <summary>
    /// ���� �Ϻ���û�� ���� ��Ʈ��
    /// </summary>
    public class DailyProgramHitControl : System.Windows.Forms.UserControl, IUserControl
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
        DailyProgramHitModel mainModel  = new DailyProgramHitModel();	// ���α׷��� �����û�����
		//SummaryAdModel summaryAdModel        = new SummaryAdModel();	// �Ѱ������

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
			this.uiPanelChoiceAdSchedule.Text = "�Ϻ� ���α׷� ��û����";
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
			this.uiPanelSearch.Text = "�˻�";
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
			this.uiPanelList.Text = "�Ϻ���ûȽ��";
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

        #region ��Ʈ�� �ε�
        private void UserControl_Load(object sender, System.EventArgs e)
        {
            rptHeader.SearchClicked += new AdManagerClient.SearchClickEventHandler(OnSearch);
            rptHeader.ExcelClicked	+= new AdManagerClient.ExcelClickEventHandler(OnExcel);
            rptHeader.u_MenuName = MenuCode;
            rptHeader.u_InitControl();
        }

        #endregion

        #region ó���޼ҵ�

        /// <summary>
        /// �Ϻ���û�� ��ȸ
        /// </summary>
        private void SearchReport()
        {
            StatusMessage("�Ϻ� ���α׷� ��û���踦 ��ȸ�մϴ�.");

//			try
//			{
//				// �����͸� �ʱ�ȭ
//				dailyProgramHitModel.Init();
//
//				dailyProgramHitModel.SearchMediaCode	=  cbSearchMedia.SelectedValue.ToString(); 
//				dailyProgramHitModel.SearchContractSeq	=  cbSearchContract.SelectedValue.ToString(); 
//				dailyProgramHitModel.SearchItemNo		=  cbSearchItem.SelectedValue.ToString(); 
//				dailyProgramHitModel.CampaignCode		=  cbCampaign.SelectedValue.ToString(); 
//
//				if(rbSearchType_D.Checked)
//				{
//					dailyProgramHitModel.SearchType	  	=  "D"; // -- D:���ñⰣ C:���Ⱓ
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
//				uiPanelList.Text = "��ûȽ��"; 
//
//				//  �Ϻ� ���α׷���û������ȸ ���񽺸� ȣ���Ѵ�.
//				new DailyProgramHitManager(systemModel,commonModel).GetDailyProgramHitReport(dailyProgramHitModel);
//
//				if (dailyProgramHitModel.ResultCD.Equals("0000"))
//				{
//					Utility.SetDataTable(dailyProgramHitDs.RowData, dailyProgramHitModel.ReportDataSet);		
//					Utility.SetDataTable(dailyProgramHitDs.Header,  dailyProgramHitModel.HeaderDataSet);		
//					StatusMessage(dailyProgramHitModel.ResultCnt + "���� ��ȸ�Ǿ����ϴ�.");
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
//					uiPanelList.Text = "�Ϻ���ûȽ�� : " + keyMediaName + " / " + keyContractName ;
//
//					if(!keyItemNo.Equals("") && !keyItemNo.Equals("00"))
//					{
//						uiPanelList.Text += " / [" + keyItemNo    + "]" + keyItemName;
//					}
//					else
//					{
//						uiPanelList.Text += " / ��ü����";
//					}
//					
//					uiPanelList.Text += " / " + keyReportBgnDay + " ~ " + keyReportEndDay;	
//			
//					canPrint = true;
//
//					// �׸����� ����� �ٽ� ��Ʈ�Ѵ�.
//					SetListHeader();
//
//					//Row�����͸� ����Ʈ�� �����ͷ� �籸���Ѵ�.
//					SetListData();
//				}
//			}
//			catch(FrameException fe)
//			{
//				FrameSystem.showMsgForm("�Ϻ� ���α׷� ��ûȽ�� ��ȸ����", new string[] {fe.ErrCode, fe.ResultMsg});
//			}
//			catch(Exception ex)
//			{
//				FrameSystem.showMsgForm("�Ϻ� ���α׷� ��ûȽ�� ��ȸ����",new string[] {"",ex.Message});
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
			

			dailyProgramHitDs.Report.Clear();		// ������ Ŭ����

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
						reportRow["SumDay"] = SumDay;				// �ϰ��Ʈ
						SumDay = 0;

						dailyProgramHitDs.Report.Rows.Add(reportRow); // ����Ʈ�����Ϳ� Row��Ʈ
					}

					reportRow = dailyProgramHitDs.Report.NewRow();
					OldDay = LogDay;
					GenreIndex = 0;		// �÷���ȣ �ʱ�ȭ
				}		

				reportRow["Genre"+GenreIndex.ToString()] =  Convert.ToInt32(dataRow["HitCnt"].ToString());  // �÷����� ��Ʈ
				SumDay += Convert.ToInt32(dataRow["HitCnt"].ToString());
				GenreIndex++;
			}

			// ������ Row
			if(!OldDay.Equals(""))
			{
				reportRow["SumDay"] = SumDay;
				reportRow["LogDay"] = Utility.reConvertDate("20" + OldDay);
				dailyProgramHitDs.Report.Rows.Add(reportRow); // ����Ʈ�����Ϳ� Row��Ʈ
			}	
		}

		private void SetListHeader()
		{
			StringBuilder LayoutXml = null;

            // ��������
			LayoutXml = new StringBuilder();

			LayoutXml.Append("\n"
				+ "<GridEXLayoutData>                                                  \n"
				+ "  <RootTable>                                                       \n"
				+ "    <Caption>Report</Caption>                                       \n"
				+ "    <CellLayoutMode>UseColumnSets</CellLayoutMode>                  \n"
				+ "    <ColumnHeaders>True</ColumnHeaders>                             \n"
				+ "    <ColumnSets Collection=\"true\">                                \n"
				

			// �÷���������Ʈ

				// ���ڿ� �հ� �÷�
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

			// ī�װ���Ʈ
			// 
			string OldCategory = "";
			string NowCategory = "";
			string NowGenre    = "";
			int    ColIndex      = 2;  // ����, �հ� �������� �̹Ƿ�
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
						// ī�װ��� ���κ�
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
					// ī�װ� ���ۺκ�
					LayoutXml.Append("<ColumnSet" + ColumnSet + " ID=\"ColumnSet" + ColumnSet + "\">\n");
					LayoutXml.Append("  <Key>ColumnSet" + ColumnSet + "</Key>\n");
					LayoutXml.Append("  <Caption>"+NowCategory+"</Caption>\n");

					OldCategory = NowCategory;
				}

				// �帣���� �κ�
				LayoutXml.Append("  <ColIndex"+ColumnCount.ToString() + ">"+ColIndex.ToString()+"</ColIndex"+ColumnCount.ToString() + ">\n");
				LayoutXml.Append("  <Row"+ColumnCount.ToString() + ">0</Row"+ColumnCount.ToString() + ">\n");
				LayoutXml.Append("  <Col"+ColumnCount.ToString() + ">"+ColumnCount.ToString()+"</Col"+ColumnCount.ToString() + ">\n");
				LayoutXml.Append("  <RowSpan"+ColumnCount.ToString() + ">1</RowSpan"+ColumnCount.ToString() + ">\n");
				LayoutXml.Append("  <ColSpan"+ColumnCount.ToString() + ">1</ColSpan"+ColumnCount.ToString() + ">\n");
				ColumnCount++;
				ColIndex++;			
			}

			// ������ ī�װ����� ���κ�
			if(!OldCategory.Equals("")) 
			{ 
				// ī�װ��� ���κ�
				LayoutXml.Append("  <ColumnCount>" + ColumnCount.ToString() + "</ColumnCount>\n");
				LayoutXml.Append("  <Position>" + Position.ToString() + "</Position>\n");
				for(int j=0;j<ColumnCount;j++)
				{
					LayoutXml.Append("  <ColumnWidth" + j.ToString() + ">100</ColumnWidth" + j.ToString() + ">\n");
				}
				LayoutXml.Append("  <EntriesCount>" + ColumnCount.ToString() + "</EntriesCount>\n");							
				LayoutXml.Append("</ColumnSet" + ColumnSet + ">\n");

			}

			// �÷�������Ʈ
			LayoutXml.Append(""
				+ "    </ColumnSets>                                                   \n"
				+ "    <Columns Collection=\"true\">                                   \n"
				+ "      <Column0 ID=\"LogDay\">                                       \n"
				+ "        <Bound>True</Bound>                                         \n"
				+ "        <Caption>����</Caption>                                     \n"
				+ "        <DataMember>LogDay</DataMember>                             \n"
				+ "        <Key>LogDay</Key>                                           \n"
				+ "        <Position>0</Position>                                      \n"
				+ "        <TextAlignment>Center</TextAlignment>                       \n"
				+ "      </Column0>                                                    \n"
				+ "      <Column1 ID=\"SumDay\">                                       \n"
				+ "        <Bound>True</Bound>                                         \n"
				+ "        <Caption>�հ�</Caption>                                     \n"
				+ "        <DataMember>SumDay</DataMember>                             \n"
				+ "        <FormatString>#,##0</FormatString>                          \n"
				+ "        <Key>SumDay</Key>                                           \n"
				+ "        <Position>1</Position>                                      \n"
				+ "        <TextAlignment>Far</TextAlignment>                          \n"
				+ "        <AggregateFunction>Sum</AggregateFunction>                  \n"
				+ "        <TotalFormatString>#,##0</TotalFormatString>                \n"
				+ "      </Column1>                                                    \n"
				);

			ColIndex      = 2;  // ����, �հ� �������� �ٽ�

			for(int i=0;i<dailyProgramHitDs.Header.Rows.Count;i++)
			{
				DataRow Row = dailyProgramHitDs.Header.Rows[i];

				NowCategory = Row["CategoryName"].ToString().Replace("&","/");
				NowGenre    = Row["GenreName"].ToString().Replace("&","/");

				// �帣���� �κ�
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


			maxColumn = ColIndex;  // ��ü�÷��� ����. �������� ���

			FrameSystem.oLog.Debug(LayoutXml.ToString());

			// �ۼ��� ���̾ƿ� XML�� �׸��忡 �ε��Ѵ�.
			Janus.Windows.GridEX.GridEXLayout layout = new Janus.Windows.GridEX.GridEXLayout();
			layout.LayoutString = LayoutXml.ToString();
			this.grdExHitList.LoadLayout(layout);

			// �׸��带 �ʱ�ȭ�Ѵ�.

			#region �׸��� �ʱ�ȭ
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
			this.grdExHitList.Font = new System.Drawing.Font("����ü", 9F);
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
		private void MakeExcel()
		{	

			Excel.Application xlApp= null;
			Excel._Workbook   oWB = null;
			Excel._Worksheet  oSheet = null;
			Excel.Range       oRng = null;
			
			try
			{	

				int ColCount  = maxColumn; // �÷���				

				int TitleRow  = 1;
				int ConditionRow = 2;
				int HeaderRow = 8;
				int DataRow   = 10;
				string StartCol = "A";
				string EndCol   = "";
				string TitleCol = "H";
				int DataCount = 0;
				int GenreCount = 0;

				// ����� �帣�� ����
				GenreCount = dailyProgramHitDs.Header.Rows.Count;

				// ������ �÷��� �ε�������
				EndCol = GetColumnIndex(ColCount);

				xlApp = new Excel.Application();
				oWB = (Excel._Workbook)(xlApp.Workbooks.Add(Missing.Value));
				oSheet = (Excel._Worksheet)oWB.ActiveSheet;


				// Ÿ��Ʋ �ۼ�
				oSheet.Cells[TitleRow,1] = "�Ϻ� ���α׷���û ����";
				oRng = oSheet.get_Range(StartCol+Convert.ToString(TitleRow), TitleCol+Convert.ToString(TitleRow));
				oRng.Merge(true);
				oRng.Font.Bold = true;
				oRng.Font.Size = 16;
				oRng.VerticalAlignment   = Excel.XlVAlign.xlVAlignCenter;			
				oRng.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;		

				// �������� �ۼ�
				oSheet.Cells[ConditionRow,1] = "��ü";
				oRng = oSheet.get_Range("B"+Convert.ToString(ConditionRow), TitleCol+Convert.ToString(ConditionRow));
				oRng.Merge(true);
				oSheet.Cells[ConditionRow,2] = keyMediaName;

				oSheet.Cells[ConditionRow+1,1] = "����";
				oRng = oSheet.get_Range("B"+Convert.ToString(ConditionRow+1), TitleCol+Convert.ToString(ConditionRow+1));
				oRng.Merge(true);
				oSheet.Cells[ConditionRow+1,2] = keyContractName;

                oSheet.Cells[ConditionRow+2,1] = "ķ����";
                oRng = oSheet.get_Range("B"+Convert.ToString(ConditionRow+2), TitleCol+Convert.ToString(ConditionRow+2));
                oRng.Merge(true);
                oSheet.Cells[ConditionRow+2,2] = keyCampaignName;
					
				oSheet.Cells[ConditionRow+3,1] = "�����";
				oRng = oSheet.get_Range("B"+Convert.ToString(ConditionRow+3), TitleCol+Convert.ToString(ConditionRow+3));
				oRng.Merge(true);
				oSheet.Cells[ConditionRow+3,2] = "[" + keyItemNo + "] " + keyItemName;

				oSheet.Cells[ConditionRow+4,1] = "����Ⱓ";
				oRng = oSheet.get_Range("B"+Convert.ToString(ConditionRow+4), TitleCol+Convert.ToString(ConditionRow+4));
				oRng.Merge(true);
                string ReportType =  Utility.reConvertDate("20" + keyReportBgnDay) + " ~ " + Utility.reConvertDate("20" + keyReportEndDay);
                ReportType += " (" + keyReportType + ")";
                oSheet.Cells[ConditionRow+4,2] = ReportType;

				// ���Ǻ� �׵θ�
				oRng = oSheet.get_Range(StartCol+Convert.ToString(ConditionRow), TitleCol+Convert.ToString(ConditionRow+4));
				oRng.VerticalAlignment   = Excel.XlVAlign.xlVAlignCenter;			
				oRng.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;		
				oRng.Borders.LineStyle =  Excel.XlLineStyle.xlContinuous;	// �׵θ��Ӽ� �Ʒ��� �Ǽ�
				oRng.Borders.Weight    = Excel.XlBorderWeight.xlThin;		// �׵θ��Ӽ� �Ʒ��� ���¼�
                oRng.Font.Size          = 10;


				// ��� ���� �ۼ�
				oSheet.Cells[HeaderRow+1,1] = "����";
				oSheet.Cells[HeaderRow+1,2] = "�հ�";


				string OldCategory = "";
				string NowCategory = "";
				int bgncol  = 3;	// ó�� �帣�� ���۵Ǵ� �÷���ġ 3��°����
				int colcnt  = 0;   

				for(int i=0;i<GenreCount;i++)
				{
					DataRow Row = dailyProgramHitDs.Header.Rows[i];				// ��������ͷκ��� �帣�� ����� ��Ʈ�Ѵ�.

					NowCategory =  Row["CategoryName"].ToString();	// ī�װ���
					if(!OldCategory.Equals(NowCategory))
					{
						if(!OldCategory.Equals(""))			// ī�װ��� �ٲ������ �帣����ŭ �����ϰ� ī�װ��� ��Ʈ
						{ 
							oRng = oSheet.get_Range(GetColumnIndex(bgncol)+Convert.ToString(HeaderRow), GetColumnIndex(bgncol+colcnt-1)+Convert.ToString(HeaderRow));
							oRng.Merge(true);

							oSheet.Cells[HeaderRow,bgncol] = OldCategory;
							bgncol += colcnt;
							colcnt  = 0;
						}
					}

					OldCategory = NowCategory;

					oSheet.Cells[HeaderRow+1,3+i] = Row["GenreName"].ToString(); //�帣�� . ó�� �帣�� ���۵Ǵ� �÷���ġ 3��°����

					colcnt++;
				}	
				if(!OldCategory.Equals("")) 
				{ 
					oRng = oSheet.get_Range(GetColumnIndex(bgncol)+Convert.ToString(HeaderRow), GetColumnIndex(ColCount)+Convert.ToString(HeaderRow));
					oRng.Merge(true);
					oSheet.Cells[HeaderRow,bgncol] = OldCategory;
				}		


				oRng = oSheet.get_Range(StartCol+Convert.ToString(HeaderRow), EndCol+Convert.ToString(HeaderRow+1)); // ����� ����
				oRng.Font.Bold           = true;							// ��Ʈ ����
				oRng.VerticalAlignment   = Excel.XlVAlign.xlVAlignCenter;	// �����߾�����
				oRng.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;	// �����߾�����	 
				oRng.Interior.Color      = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.CornflowerBlue);   //�� ���� 
				oRng.Font.Color          = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.White);	  //�ؽ�Ʈ��			
				
				//�հ踦 ����
				int TotalSumDay = 0;
				int[] TotalHit = new int[GenreCount];

				for(int i=0;i<GenreCount;i++)
				{
					TotalHit[i] = 0; // �ʱ�ȭ
				}

				DataCount = 1;
				// ������ ����
				for (int inx =0; inx < dailyProgramHitDs.Report.Rows.Count; inx++)
				{

					oSheet.Cells[DataRow+DataCount,1] = dailyProgramHitDs.Report.Rows[inx]["LogDay"].ToString();
					int SumDay = Convert.ToInt32(dailyProgramHitDs.Report.Rows[inx]["SumDay"].ToString());	
					oSheet.Cells[DataRow+DataCount,2] = SumDay;	
					TotalSumDay       += SumDay;
			
					for(int i=0;i<GenreCount;i++)
					{
						DataRow Row = dailyProgramHitDs.Header.Rows[i];				// ��������ͷκ��� �帣�� ����� ��Ʈ�Ѵ�.

						int Hit = Convert.ToInt32(dailyProgramHitDs.Report.Rows[inx]["Genre"+i.ToString()].ToString());
						oSheet.Cells[DataRow+DataCount,3+i] = Hit;
						TotalHit[i] += Hit;
					}					
					DataCount++;
				}

				// �Ѱ� : �����ͺ��� ù���ο� �ִ´�.			
				oSheet.Cells[DataRow,1] = "�հ�";
				oSheet.Cells[DataRow,2] = TotalSumDay;	
				for(int i=0;i<GenreCount;i++)
				{
					oSheet.Cells[DataRow,3+i] = TotalHit[i];
				}	
				
				oRng = oSheet.get_Range(StartCol+Convert.ToString(DataRow), EndCol+Convert.ToString(DataRow)); // �հ� ����
				oRng.Font.Bold           = true;							// ��Ʈ ����
				oRng.Interior.Color      = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.LightSteelBlue);   //�� ���� 

				//���� �߾�����
				oRng = oSheet.get_Range("A"+Convert.ToString(DataRow), "A"+Convert.ToString(DataRow+DataCount)); // ����
				oRng.VerticalAlignment   = Excel.XlVAlign.xlVAlignCenter;	// �����߾�����
				oRng.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;	// �����߾�����	 

                DataCount--;

                // ������ �κ� ��Ʈ ����
                oRng = oSheet.get_Range(StartCol+Convert.ToString(HeaderRow), EndCol+Convert.ToString(DataRow+DataCount));
                oRng.Font.Size = 9;
                oRng.RowHeight = 14;

				// ������ �ۼ�
				oRng = oSheet.get_Range(StartCol+Convert.ToString(HeaderRow), EndCol+Convert.ToString(DataRow+DataCount));	// �������� ����
				oRng.EntireColumn.AutoFit();					// �������� ũ�⿡ ���� ����ũ�� ����
				oRng.Borders.LineStyle =  Excel.XlLineStyle.xlContinuous;	// �׵θ��Ӽ� �Ʒ��� �Ǽ�
				oRng.Borders.Weight    = Excel.XlBorderWeight.xlThin;		// �׵θ��Ӽ� �Ʒ��� ���¼�

				// ������ �����Ϳ� ��Ÿ�� ����
				oRng = oSheet.get_Range("B"+Convert.ToString(DataRow), EndCol+Convert.ToString(DataRow+DataCount));	// �������� ����
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

        private void OnSearch(object sender, SearchReportData e)
        {
            StatusMessage("�Ϻ� ���α׷� ��û����� ��ȸ�մϴ�.");
            ProgressStart();
			 
            try
            {
                // �����͸� �ʱ�ȭ
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
			 
                uiPanelList.Text = "�Ϻ� ���α׷� ��û"; 
			 
                //  ��ü��� ���񽺸� ȣ���Ѵ�.
                new DailyProgramHitManager(systemModel,commonModel).GetDailyProgramHitReport(mainModel);
	
                if (mainModel.ResultCD.Equals("0000"))
                {
                    Utility.SetDataTable(dailyProgramHitDs.RowData, mainModel.ReportDataSet);		
                    Utility.SetDataTable(dailyProgramHitDs.Header,  mainModel.HeaderDataSet);		
                    StatusMessage(mainModel.ResultCnt + "���� ��ȸ�Ǿ����ϴ�.");
			 	
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
                    //Row�����͸� ����Ʈ�� �����ͷ� �籸���Ѵ�.
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
                FrameSystem.showMsgForm("�Ϻ� ���α׷� ��û��� ��ȸ����", new string[] {fe.ErrCode, fe.ResultMsg});
            }
            catch(Exception ex)
            {
                rptHeader.u_IsPrint = false;
                FrameSystem.showMsgForm("�Ϻ� ���α׷� ��û��� ��ȸ����",new string[] {"",ex.Message});
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
