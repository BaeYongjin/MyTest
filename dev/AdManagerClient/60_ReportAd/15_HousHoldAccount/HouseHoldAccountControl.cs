// ===============================================================================
//
// DailyAdHitControl.cs
//
// �Ϻ� ���� ��û���� ��Ʈ���� �����մϴ�. 
//
// ===============================================================================
// Release history
//
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
    /// �Ͽ콺Ȧ�� ���� ���� ��Ʈ��
    /// </summary>
    public class HouseHoldAccountControl : System.Windows.Forms.UserControl, IUserControl
    {
        #region ��������� ��ü �� ����

      // �ý��� ���� : ȭ�����
      private SystemModel   systemModel   = FrameSystem.oSysModel;
      private CommonModel   commonModel   = FrameSystem.oComModel;
      private Logger        log           = FrameSystem.oLog;
      private MenuPower     menu          = FrameSystem.oMenu;

        // �޴��ڵ� : ������ �ʿ��� ȭ�鿡 �ʿ���
        public string        menuCode		= "";

        // ����� ������
        HouseHoldAccountModel mainModel  = new HouseHoldAccountModel();	// ���α׷��� �����û�����
        string keyMediaName         = "";
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
        private AdManagerClient.ReportHeaderControl rptHeader;
		private AdManagerClient._60_ReportAd._15_HousHoldAccount.HouseHoldAccountDs houseHoldAccountDs;
		private System.Data.DataView dsView;
		private Janus.Windows.GridEX.GridEX grdExReport;
        private System.ComponentModel.IContainer components;

        public HouseHoldAccountControl()
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(HouseHoldAccountControl));
            this.uiPM = new Janus.Windows.UI.Dock.UIPanelManager(this.components);
            this.uiPanelChoiceAdSchedule = new Janus.Windows.UI.Dock.UIPanelGroup();
            this.uiPanelSearch = new Janus.Windows.UI.Dock.UIPanel();
            this.uiPanelSearchContainer = new Janus.Windows.UI.Dock.UIPanelInnerContainer();
            this.pnlSearch = new System.Windows.Forms.Panel();
            this.rptHeader = new AdManagerClient.ReportHeaderControl();
            this.uiPanelList = new Janus.Windows.UI.Dock.UIPanel();
            this.uiPanelListContainer = new Janus.Windows.UI.Dock.UIPanelInnerContainer();
            this.grdExReport = new Janus.Windows.GridEX.GridEX();
            this.dsView = new System.Data.DataView();
            this.houseHoldAccountDs = new AdManagerClient._60_ReportAd._15_HousHoldAccount.HouseHoldAccountDs();
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
            ((System.ComponentModel.ISupportInitialize)(this.dsView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.houseHoldAccountDs)).BeginInit();
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
            this.uiPM.AddDockPanelInfo(new System.Guid("afb3b7d0-aa79-443c-a1e8-3baba38f7a4d"), new System.Guid("b162c53a-7940-47cd-a0ed-9762c59c50d8"), 108, true);
            this.uiPM.AddDockPanelInfo(new System.Guid("8e3140b9-bfcb-43af-b84e-2ee6cddf51dc"), new System.Guid("b162c53a-7940-47cd-a0ed-9762c59c50d8"), 523, true);
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
            this.uiPanelChoiceAdSchedule.Text = "�Ͽ콺Ȧ�� ����";
            // 
            // uiPanelSearch
            // 
            this.uiPanelSearch.AllowResize = Janus.Windows.UI.InheritableBoolean.False;
            this.uiPanelSearch.CaptionVisible = Janus.Windows.UI.InheritableBoolean.False;
            this.uiPanelSearch.CloseButtonVisible = Janus.Windows.UI.InheritableBoolean.False;
            this.uiPanelSearch.InnerContainer = this.uiPanelSearchContainer;
            this.uiPanelSearch.Location = new System.Drawing.Point(0, 22);
            this.uiPanelSearch.Name = "uiPanelSearch";
            this.uiPanelSearch.Size = new System.Drawing.Size(1010, 111);
            this.uiPanelSearch.TabIndex = 4;
            this.uiPanelSearch.Text = "�˻�";
            // 
            // uiPanelSearchContainer
            // 
            this.uiPanelSearchContainer.Controls.Add(this.pnlSearch);
            this.uiPanelSearchContainer.Location = new System.Drawing.Point(1, 1);
            this.uiPanelSearchContainer.Name = "uiPanelSearchContainer";
            this.uiPanelSearchContainer.Size = new System.Drawing.Size(1008, 109);
            this.uiPanelSearchContainer.TabIndex = 0;
            // 
            // pnlSearch
            // 
            this.pnlSearch.BackColor = System.Drawing.SystemColors.Window;
            this.pnlSearch.Controls.Add(this.rptHeader);
            this.pnlSearch.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlSearch.ImeMode = System.Windows.Forms.ImeMode.Off;
            this.pnlSearch.Location = new System.Drawing.Point(0, 0);
            this.pnlSearch.Name = "pnlSearch";
            this.pnlSearch.Size = new System.Drawing.Size(1008, 109);
            this.pnlSearch.TabIndex = 3;
            // 
            // rptHeader
            // 
            this.rptHeader.BackColor = System.Drawing.Color.DeepSkyBlue;
            this.rptHeader.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rptHeader.Location = new System.Drawing.Point(0, 0);
            this.rptHeader.Name = "rptHeader";
            this.rptHeader.Size = new System.Drawing.Size(1008, 109);
            this.rptHeader.TabIndex = 0;
            this.rptHeader.u_IsPrint = false;
            this.rptHeader.u_MenuName = "";
            // 
            // uiPanelList
            // 
            this.uiPanelList.BackColor = System.Drawing.SystemColors.Window;
            this.uiPanelList.CaptionStyle = Janus.Windows.UI.Dock.PanelCaptionStyle.Dark;
            this.uiPanelList.InnerContainer = this.uiPanelListContainer;
            this.uiPanelList.Location = new System.Drawing.Point(0, 137);
            this.uiPanelList.MinimumSize = new System.Drawing.Size(-1, 100);
            this.uiPanelList.Name = "uiPanelList";
            this.uiPanelList.Size = new System.Drawing.Size(1010, 540);
            this.uiPanelList.TabIndex = 4;
            this.uiPanelList.Text = "�Ϻ� �帣�� ������� ��Ȳ";
            // 
            // uiPanelListContainer
            // 
            this.uiPanelListContainer.BackColor = System.Drawing.SystemColors.Window;
            this.uiPanelListContainer.Controls.Add(this.grdExReport);
            this.uiPanelListContainer.Location = new System.Drawing.Point(1, 23);
            this.uiPanelListContainer.Name = "uiPanelListContainer";
            this.uiPanelListContainer.Size = new System.Drawing.Size(1008, 516);
            this.uiPanelListContainer.TabIndex = 0;
            // 
            // grdExReport
            // 
            this.grdExReport.AllowEdit = Janus.Windows.GridEX.InheritableBoolean.False;
            this.grdExReport.AlternatingColors = true;
            this.grdExReport.BorderStyle = Janus.Windows.GridEX.BorderStyle.None;
            this.grdExReport.DataSource = this.dsView;
            grdExReport_DesignTimeLayout.LayoutString = resources.GetString("grdExReport_DesignTimeLayout.LayoutString");
            this.grdExReport.DesignTimeLayout = grdExReport_DesignTimeLayout;
            this.grdExReport.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdExReport.EditorsControlStyle.ButtonAppearance = Janus.Windows.GridEX.ButtonAppearance.Regular;
            this.grdExReport.EmptyRows = true;
            this.grdExReport.FocusCellFormatStyle.BackColor = System.Drawing.SystemColors.Highlight;
            this.grdExReport.FocusCellFormatStyle.ForeColor = System.Drawing.Color.White;
            this.grdExReport.FocusStyle = Janus.Windows.GridEX.FocusStyle.None;
            this.grdExReport.Font = new System.Drawing.Font("���� ���", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.grdExReport.FrozenColumns = 1;
            this.grdExReport.GridLineColor = System.Drawing.Color.Silver;
            this.grdExReport.GridLines = Janus.Windows.GridEX.GridLines.Vertical;
            this.grdExReport.GridLineStyle = Janus.Windows.GridEX.GridLineStyle.Solid;
            this.grdExReport.GroupByBoxVisible = false;
            this.grdExReport.HeaderFormatStyle.TextAlignment = Janus.Windows.GridEX.TextAlignment.Center;
            this.grdExReport.HideSelection = Janus.Windows.GridEX.HideSelection.Highlight;
            this.grdExReport.Location = new System.Drawing.Point(0, 0);
            this.grdExReport.Name = "grdExReport";
            this.grdExReport.SelectedInactiveFormatStyle.BackColor = System.Drawing.Color.Gold;
            this.grdExReport.Size = new System.Drawing.Size(1008, 516);
            this.grdExReport.TabIndex = 13;
            this.grdExReport.TabKeyBehavior = Janus.Windows.GridEX.TabKeyBehavior.ControlNavigation;
            this.grdExReport.TotalRowFormatStyle.BackColor = System.Drawing.Color.Yellow;
            this.grdExReport.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            // 
            // dsView
            // 
            this.dsView.Table = this.houseHoldAccountDs.dsHouseHoldAccount;
            // 
            // houseHoldAccountDs
            // 
            this.houseHoldAccountDs.DataSetName = "HouseHoldAccountDs";
            this.houseHoldAccountDs.Locale = new System.Globalization.CultureInfo("en-US");
            this.houseHoldAccountDs.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
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
            // HouseHoldAccountControl
            // 
            this.Controls.Add(this.uiPanelChoiceAdSchedule);
            this.Name = "HouseHoldAccountControl";
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
            ((System.ComponentModel.ISupportInitialize)(this.grdExReport)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dsView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.houseHoldAccountDs)).EndInit();
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
        #endregion

        #region �̺�Ʈ�Լ�

        public event StatusEventHandler 			StatusEvent;			// �����̺�Ʈ �ڵ鷯
        public event ProgressEventHandler 			ProgressEvent;			// ó�����̺�Ʈ �ڵ鷯
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
				int ColCount  = 3; // �÷���
				int TitleRow  = 1;
				int ConditionRow = 2;
				int HeaderRow = 8;
				int DataRow   = 9;
				string StartCol = "A";
				string EndCol   = "";
				string TitleCol = "H";
				int DataCount = 0;
				int GenreCount = 0;
				
				// ����� �帣�� ����
				GenreCount = houseHoldAccountDs.dsHouseHoldAccount.Rows.Count;

				// ������ �÷��� �ε�������
				EndCol = GetColumnIndex(ColCount);

				xlApp = new Excel.Application();
				oWB = (Excel._Workbook)(xlApp.Workbooks.Add(Missing.Value));
				oSheet = (Excel._Worksheet)oWB.ActiveSheet;


				// Ÿ��Ʋ �ۼ�
				oSheet.Cells[TitleRow,1] = "�Ϻ����� UH����";
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
				oSheet.Cells[HeaderRow,1] = "����";
				oSheet.Cells[HeaderRow,2] = "����UH";
				oSheet.Cells[HeaderRow,3] = "����UH";

				oRng = oSheet.get_Range(StartCol+Convert.ToString(HeaderRow), EndCol+Convert.ToString(HeaderRow));	// ����� ����
				oRng.Font.Bold  = true;							// ��Ʈ ����
				oRng.Font.Size  = 8;							// ��Ʈ ����
				oRng.VerticalAlignment   = Excel.XlVAlign.xlVAlignCenter;	// �����߾�����
				oRng.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;	// �����߾�����	 
				oRng.Interior.Color      = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.CornflowerBlue);   //�� ���� 
				oRng.Font.Color          = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.White);			//�ؽ�Ʈ��			
				
				DataCount = 0;
				// ������ ����
				for (int inx =0; inx < houseHoldAccountDs.Tables[0].Rows.Count; inx++)
				{		
					DataRow Row = houseHoldAccountDs.Tables[0].Rows[inx];
					int ColCnt = 1;

					oSheet.Cells[DataRow+inx, ColCnt++] = Row["LogDay"];	// 1  ����
					oSheet.Cells[DataRow+inx, ColCnt++] = Row["Cnt"];		// 2  �Ͽ콺Ȧ��
					oSheet.Cells[DataRow+inx, ColCnt++] = Row["Inc"];		// 3  �����Ͽ콺Ȧ��
					DataCount++;
				}

				DataCount--;

				//���� �߾�����
				oRng = oSheet.get_Range("A"+Convert.ToString(DataRow), "A"+Convert.ToString(DataRow+DataCount)); // ����
				oRng.VerticalAlignment   = Excel.XlVAlign.xlVAlignCenter;	// �����߾�����
				oRng.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;	// �����߾�����	 

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
			if(e.ContractName == null || e.ContractName.Equals(""))
			{
				MessageBox.Show("����� ������ �ּ���.");
				return;
			}
			//if(cbSearchEndDay.Value > cbSearchStartDay.Value.AddMonths(1).AddDays(-1))

			DateTime sdt = DateTime.ParseExact(e.ItemBeginDay, "yyMMdd", System.Globalization.CultureInfo.CurrentCulture);
			DateTime edt = DateTime.ParseExact(e.ItemEndDay, "yyMMdd", System.Globalization.CultureInfo.CurrentCulture);
			if(sdt > edt)
			{
				MessageBox.Show("�˻��������� ������ ���� Ŭ ���� �����ϴ�.");
			}
			if(sdt.AddMonths(2) < edt)
			{
				MessageBox.Show("�˻��Ⱓ�� 2������ ���� �� �����ϴ�.");
			}

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
                mainModel.SearchStartDay    =  e.ItemBeginDay;
                mainModel.SearchEndDay  	=  e.ItemEndDay;
			 
		 
                keyMediaName      = "";
                keyContractName   = "";
                keyAgencyName     = "";
                keyAdvertiserName = "";
                keyItemNo         = "";
                keyItemName       = "";
                keyReportBgnDay   = "";
                keyReportEndDay   = "";
			 
                uiPanelList.Text = "�Ͽ콺Ȧ��������ȸ"; 
			 
                //  ��ü��� ���񽺸� ȣ���Ѵ�.
                new HouseHoldAccountManager(systemModel,commonModel).GetHouseHoldAccount(mainModel);
	
                if (mainModel.ResultCD.Equals("0000"))
                {
                    Utility.SetDataTable(houseHoldAccountDs.dsHouseHoldAccount, mainModel.ReportDataSet);		
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

                    //Row�����͸� ����Ʈ�� �����ͷ� �籸���Ѵ�.
                }
                else
                {
                    rptHeader.u_IsPrint = false;
                }
            }
            catch(FrameException fe)
            {
                rptHeader.u_IsPrint = false;
                FrameSystem.showMsgForm("�Ϻ� �����û ��ȸ����", new string[] {fe.ErrCode, fe.ResultMsg});
            }
            catch(Exception ex)
            {
                rptHeader.u_IsPrint = false;
                FrameSystem.showMsgForm("�Ϻ� �����û ��ȸ����",new string[] {"",ex.Message});
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
