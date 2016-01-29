// ===============================================================================
//
// StatisticsRegionControl.cs
//
// ���� ��û��Ȳ ���� ��Ʈ���� �����մϴ�. 
//
// ===============================================================================
// Release history
// ===============================================================================
// Copyright (C) 2007 G-Inno Systems Inc.
// All rights reserved.
// 

/* 
 * -------------------------------------------------------
 * Class Name: StatisticsRegionControl
 * �ֿ���  : ������ ���
 * �ۼ���    : ��
 * �ۼ���    : ��
 * Ư�̻���  : �������� Ȯ���� ���� ��� �߰� -bae
 * -------------------------------------------------------
 * [��������]
 * -------------------------------------------------------
 * �����ڵ�  : [E_01]
 * ������    : bae
 * ������    : 2010.08.03
 * �����κ�  : 
 *            - MakeExcel(..)
 * 
 *            - statisticsRegionDs -Report(table)��
 *                Orders, ParentCode, SummaryDesc �÷� �߰�
 * 
 *            - GridEx �� Groups�� SummaryDesc �߰�
 *                        Column�� Orders, ParentCode, SummaryDesc �߰�
 * 
 * ��������  : 
 *            - �������� 3�ܰ� ���� Ȯ������ ������ �׸��� ����
              
 * --------------------------------------------------------
 * 
 * --------------------------------------------------------
 * 
 */

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
    public class StatisticsRegionControl : System.Windows.Forms.UserControl, IUserControl
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
        StatisticsRegionModel mainModel  = new StatisticsRegionModel();	// ���α׷��� �����û�����
		//SummaryAdModel summaryAdModel  = new SummaryAdModel();	// ���α׷��� �����û�����

		// Key ������
		string keyMediaName    = "";
		string keyContractName = "";
        string keyCampaignCode = "";
        string keyCampaignName = "";
		string keyAgencyName   = "";
		string keyAdvertiserName = "";
		string keyItemNo       = "";
		string keyItemName     = "";
		string keyReportBgnDay = "";
		string keyReportEndDay = "";
		string keyReportType   = "";
		string keyTotalAdCnt    = "";
		string keyContractAmt   = "";
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
		private System.Data.DataView dvReport;
		private Janus.Windows.GridEX.GridEX grdExRepoert;
		private Janus.Windows.UI.Dock.UIPanelManager uiPM;
		private Janus.Windows.GridEX.GridEX grdExReport;
		private AdManagerClient.StatisticsRegionDs statisticsRegionDs;
        private AdManagerClient.ReportHeaderControl rptHeader;
        private Janus.Windows.GridEX.EditControls.NumericEditBox ebContractAmt;
        private System.Windows.Forms.Label label9;
        private Janus.Windows.GridEX.EditControls.NumericEditBox ebTotalAdCnt;
        private System.ComponentModel.IContainer components;

        public StatisticsRegionControl()
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(StatisticsRegionControl));
            this.uiPM = new Janus.Windows.UI.Dock.UIPanelManager(this.components);
            this.uiPanelChoiceAdSchedule = new Janus.Windows.UI.Dock.UIPanelGroup();
            this.uiPanelSearch = new Janus.Windows.UI.Dock.UIPanel();
            this.uiPanelSearchContainer = new Janus.Windows.UI.Dock.UIPanelInnerContainer();
            this.pnlSearch = new System.Windows.Forms.Panel();
            this.ebContractAmt = new Janus.Windows.GridEX.EditControls.NumericEditBox();
            this.label9 = new System.Windows.Forms.Label();
            this.ebTotalAdCnt = new Janus.Windows.GridEX.EditControls.NumericEditBox();
            this.rptHeader = new AdManagerClient.ReportHeaderControl();
            this.uiPanelList = new Janus.Windows.UI.Dock.UIPanel();
            this.uiPanelListContainer = new Janus.Windows.UI.Dock.UIPanelInnerContainer();
            this.grdExReport = new Janus.Windows.GridEX.GridEX();
            this.dvReport = new System.Data.DataView();
            this.statisticsRegionDs = new AdManagerClient.StatisticsRegionDs();
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
            ((System.ComponentModel.ISupportInitialize)(this.statisticsRegionDs)).BeginInit();
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
            this.uiPanelChoiceAdSchedule.Text = "���������";
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
            this.pnlSearch.Controls.Add(this.ebContractAmt);
            this.pnlSearch.Controls.Add(this.label9);
            this.pnlSearch.Controls.Add(this.ebTotalAdCnt);
            this.pnlSearch.Controls.Add(this.rptHeader);
            this.pnlSearch.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlSearch.Location = new System.Drawing.Point(0, 0);
            this.pnlSearch.Name = "pnlSearch";
            this.pnlSearch.Size = new System.Drawing.Size(1008, 109);
            this.pnlSearch.TabIndex = 3;
            // 
            // ebContractAmt
            // 
            this.ebContractAmt.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ebContractAmt.DecimalDigits = 0;
            this.ebContractAmt.FormatString = "#,##0";
            this.ebContractAmt.Location = new System.Drawing.Point(616, 82);
            this.ebContractAmt.Name = "ebContractAmt";
            this.ebContractAmt.ReadOnly = true;
            this.ebContractAmt.Size = new System.Drawing.Size(104, 23);
            this.ebContractAmt.TabIndex = 189;
            this.ebContractAmt.TabStop = false;
            this.ebContractAmt.Text = "0";
            this.ebContractAmt.TextAlignment = Janus.Windows.GridEX.TextAlignment.Far;
            this.ebContractAmt.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.ebContractAmt.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2003;
            // 
            // label9
            // 
            this.label9.Location = new System.Drawing.Point(489, 82);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(104, 21);
            this.label9.TabIndex = 187;
            this.label9.Text = "�������/�ѳ���";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ebTotalAdCnt
            // 
            this.ebTotalAdCnt.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ebTotalAdCnt.DecimalDigits = 0;
            this.ebTotalAdCnt.FormatString = "#,##0";
            this.ebTotalAdCnt.Location = new System.Drawing.Point(723, 82);
            this.ebTotalAdCnt.Name = "ebTotalAdCnt";
            this.ebTotalAdCnt.ReadOnly = true;
            this.ebTotalAdCnt.Size = new System.Drawing.Size(104, 23);
            this.ebTotalAdCnt.TabIndex = 188;
            this.ebTotalAdCnt.Text = "0";
            this.ebTotalAdCnt.TextAlignment = Janus.Windows.GridEX.TextAlignment.Far;
            this.ebTotalAdCnt.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.ebTotalAdCnt.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2003;
            // 
            // rptHeader
            // 
            this.rptHeader.BackColor = System.Drawing.Color.DeepSkyBlue;
            this.rptHeader.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rptHeader.Location = new System.Drawing.Point(0, 0);
            this.rptHeader.Name = "rptHeader";
            this.rptHeader.Size = new System.Drawing.Size(1008, 109);
            this.rptHeader.TabIndex = 184;
            this.rptHeader.u_IsPrint = false;
            this.rptHeader.u_MenuName = "";
            this.rptHeader.Load += new System.EventHandler(this.rptHeader_Load);
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
            this.uiPanelList.Text = "���";
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
            this.grdExReport.GroupRowFormatStyle.BackColor = System.Drawing.Color.White;
            this.grdExReport.GroupRowFormatStyle.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.grdExReport.GroupRowVisualStyle = Janus.Windows.GridEX.GroupRowVisualStyle.Outlook2003;
            this.grdExReport.HeaderFormatStyle.TextAlignment = Janus.Windows.GridEX.TextAlignment.Center;
            this.grdExReport.HideSelection = Janus.Windows.GridEX.HideSelection.Highlight;
            this.grdExReport.Location = new System.Drawing.Point(0, 0);
            this.grdExReport.Name = "grdExReport";
            this.grdExReport.ScrollBars = Janus.Windows.GridEX.ScrollBars.Vertical;
            this.grdExReport.SelectedInactiveFormatStyle.BackColor = System.Drawing.Color.Gold;
            this.grdExReport.Size = new System.Drawing.Size(1008, 516);
            this.grdExReport.TabIndex = 12;
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
            this.dvReport.Table = this.statisticsRegionDs.Report;
            // 
            // statisticsRegionDs
            // 
            this.statisticsRegionDs.DataSetName = "StatisticsRegionDs";
            this.statisticsRegionDs.Locale = new System.Globalization.CultureInfo("en-US");
            this.statisticsRegionDs.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
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
            // StatisticsRegionControl
            // 
            this.Controls.Add(this.uiPanelChoiceAdSchedule);
            this.Font = new System.Drawing.Font("���� ���", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.Name = "StatisticsRegionControl";
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
            ((System.ComponentModel.ISupportInitialize)(this.statisticsRegionDs)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdExRepoert)).EndInit();
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

        private void MakeExcel()
        {	
            Excel.Application xlApp= null;
            Excel._Workbook   oWB = null;
            Excel._Worksheet  oSheet = null;
            Excel.Range       oRng = null;
			
            try
            {	


                int ColMax  = 5; // �÷���   				

                int TitleRow  = 1;		
                int ConditionRow = 2;   
                int HeaderRow = 10;
                int DataRow   = 11;
                string StartCol = "A";
                string EndCol   = "";
                string TitleCol = "E";
                int DataCount = 0;
                int CondCount = 0;
                int HeaderCount = 0;

                // ������ �÷��� �ε�������
                EndCol = GetColumnIndex(ColMax);

                xlApp = new Excel.Application();
                oWB = (Excel._Workbook)(xlApp.Workbooks.Add(Missing.Value));
                oSheet = (Excel._Worksheet)oWB.ActiveSheet;
				xlApp.Visible = true;


                // Ÿ��Ʋ �ۼ�
                oSheet.Cells[TitleRow,1] = "���������";
                oRng = oSheet.get_Range(StartCol+Convert.ToString(TitleRow), TitleCol+Convert.ToString(TitleRow));
                oRng.Merge(true);
                oRng.Font.Bold = true;
                oRng.Font.Size = 16;
                oRng.VerticalAlignment   = Excel.XlVAlign.xlVAlignCenter;			
                oRng.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;		

                // �������� �ۼ�
                oSheet.Cells[ConditionRow+CondCount,1] = "����";
                oRng = oSheet.get_Range("B"+Convert.ToString(ConditionRow+CondCount), TitleCol+Convert.ToString(ConditionRow+CondCount));
                oRng.Merge(true);
                oSheet.Cells[ConditionRow+CondCount,2] = keyContractName;
                CondCount++;

                oSheet.Cells[ConditionRow+CondCount,1] = "ķ����";
                oRng = oSheet.get_Range("B"+Convert.ToString(ConditionRow+CondCount), TitleCol+Convert.ToString(ConditionRow+CondCount));
                oRng.Merge(true);
                oSheet.Cells[ConditionRow+CondCount,2] = keyCampaignName;
                CondCount++;

                oSheet.Cells[ConditionRow+CondCount,1] = "�����";
                oRng = oSheet.get_Range("B"+Convert.ToString(ConditionRow+CondCount), TitleCol+Convert.ToString(ConditionRow+CondCount));
                oRng.Merge(true);
                oSheet.Cells[ConditionRow+CondCount,2] = "[" + keyItemNo + "] " + keyItemName;
                CondCount++;

                oSheet.Cells[ConditionRow+CondCount,1] = "�����";
                oRng = oSheet.get_Range("B"+Convert.ToString(ConditionRow+CondCount), TitleCol+Convert.ToString(ConditionRow+CondCount));
                oRng.Merge(true);
                oSheet.Cells[ConditionRow+CondCount,2] = keyAgencyName;
                CondCount++;

                oSheet.Cells[ConditionRow+CondCount,1] = "�Ⱓ";
                oRng = oSheet.get_Range("B"+Convert.ToString(ConditionRow+CondCount), TitleCol+Convert.ToString(ConditionRow+CondCount));
                oRng.Merge(true);
                string ReportType =  keyReportBgnDay + " ~ " + keyReportEndDay;
                ReportType += " (" + keyReportType + ")";
                oSheet.Cells[ConditionRow+CondCount,2] = ReportType;
                CondCount++;

                oSheet.Cells[ConditionRow+CondCount,1] = "�������";
                oRng = oSheet.get_Range("B"+Convert.ToString(ConditionRow+CondCount), TitleCol+Convert.ToString(ConditionRow+CondCount));
                oRng.NumberFormatLocal = "#,##0";
                oRng.Merge(true);
                oSheet.Cells[ConditionRow+CondCount,2] = Convert.ToInt32(keyContractAmt);
                CondCount++;

                oSheet.Cells[ConditionRow+CondCount,1] = "�ѳ���";
                oRng = oSheet.get_Range("B"+Convert.ToString(ConditionRow+CondCount), TitleCol+Convert.ToString(ConditionRow+CondCount));
                oRng.NumberFormatLocal = "#,##0";
                oRng.Merge(true);
                oSheet.Cells[ConditionRow+CondCount,2] = Convert.ToInt32(keyTotalAdCnt);
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
                oRng = oSheet.get_Range(GetColumnIndex(1)+Convert.ToString(HeaderRow), GetColumnIndex(3)+Convert.ToString(HeaderRow));
                oRng.Merge(true);
                HeaderCount++;
				HeaderCount++;

                oSheet.Cells[HeaderRow,HeaderCount++] = "�����";
                oSheet.Cells[HeaderRow,HeaderCount++] = "%";

                oRng = oSheet.get_Range(StartCol+Convert.ToString(HeaderRow), EndCol+Convert.ToString(HeaderRow)); // ����� ����
                oRng.Font.Bold           = true;							// ��Ʈ ����
                oRng.VerticalAlignment   = Excel.XlVAlign.xlVAlignCenter;	// �����߾�����
                oRng.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;	// �����߾�����	 
                oRng.Interior.Color      = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.CornflowerBlue);   //�� ���� 
                oRng.Font.Color          = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.White);	  //�ؽ�Ʈ��			
				

                DataCount = 0;
                string OldORDCode = "";
				string PrivORDCode= "";
                int TypeMergyStart = 0;
                int MergeCount = 0;

				int	TypeMergyStart2 = 0;
				int MergeCount2 = 0;
                // ������ ����
                for (int inx =0; inx < statisticsRegionDs.Report.Rows.Count; inx++)
                {
                    DataRow Row = statisticsRegionDs.Report.Rows[inx];			

                    int ColCnt = 1;
					
                    string ORDCode = Row["ORD"].ToString();
                    int SubCount = Convert.ToInt32(Row["SubCount"].ToString());

                    // �׸� ������ ����
                    if(!OldORDCode.Equals("") && !OldORDCode.Equals(ORDCode))
                    {
						/*
                        if(MergeCount > 1)
                        {
                            oRng = oSheet.get_Range("A"+Convert.ToString(DataRow + TypeMergyStart), "A"+Convert.ToString(DataRow + DataCount-1));
                            oRng.Merge(false);
                        }
                        TypeMergyStart = DataCount;
                        MergeCount = 0;
						*/
						
						// �����ڵ�:[E_01] �߰��� �κ�
						//if (!ORDCode.Equals("2") && !ORDCode.Equals("3"))
						if ( ORDCode.Equals("1") )
						{ 
							// Merge�� �ֻ��� levle=1 �� �����ؼ� �Ѵ�.
							if(MergeCount > 1)
							{									
								//oSheet.Cells[DataRow+TypeMergyStart,1] = "1";
								oRng = oSheet.get_Range("A"+Convert.ToString(DataRow + TypeMergyStart), "A"+Convert.ToString(DataRow + TypeMergyStart + MergeCount - 1));
								oRng.Merge(true);												
							}
						}
						else if ( ORDCode.Equals("2") )
						{ 
							// Merge�� �ֻ��� levle=1 �� �����ؼ� �Ѵ�.
							if(MergeCount2 > 1)
							{	
								//oSheet.Cells[DataRow+TypeMergyStart2,2] = "2";							
								oRng = oSheet.get_Range(GetColumnIndex(2)+Convert.ToString(DataRow + TypeMergyStart2), GetColumnIndex(2)+Convert.ToString(DataRow + TypeMergyStart2 + MergeCount2 - 1));
								oRng.Merge(true);													
							}
						}

						// Merge ���� row index �� 
						if(OldORDCode.Equals("1") || ORDCode.Equals("1"))
						{
							TypeMergyStart = DataCount;	
							MergeCount = 0;
						}						
						else if(OldORDCode.Equals("2") || ORDCode.Equals("2"))
						{
							TypeMergyStart2 = DataCount;	
							MergeCount2 = 0;
						}						
						////////////////////////////////////
						
                    }

                    OldORDCode = ORDCode;
					PrivORDCode= ORDCode;
                    if(ORDCode.Equals("2")) MergeCount++; // �Һз��϶��� �����ϱ�����

					//[E_01] �߰�
					if(ORDCode.Equals("3"))
					{
						MergeCount++; // �Һз��϶��� �����ϱ�����
						MergeCount2++; // �Һз��϶��� �����ϱ�����

					}

                    if(ORDCode.Equals("1")) // ��з� ������
                    {
                        oSheet.Cells[DataRow+DataCount,1] = Row["SumName"].ToString();					// 1  ��з� ������
                        oRng = oSheet.get_Range(GetColumnIndex(1)+Convert.ToString(DataRow+DataCount), GetColumnIndex(3)+Convert.ToString(DataRow+DataCount));
                        oRng.Merge(true);
                        ColCnt++;	// �Һз� �������� �ǳʶڴ�

                    }
					else if(ORDCode.Equals("2")) // ��з� ������
					{
						oSheet.Cells[DataRow+DataCount,2] = Row["subSumName"].ToString();					// 1  ��з� ������
						oRng = oSheet.get_Range(GetColumnIndex(2)+Convert.ToString(DataRow+DataCount), GetColumnIndex(3)+Convert.ToString(DataRow+DataCount));
						oRng.Merge(true);
						ColCnt++;	// �Һз� �������� �ǳʶڴ�

					}
                    else
                    {
                        oSheet.Cells[DataRow+DataCount,3] = Row["subSumName"].ToString();							// 2  �Һз� ������
                    }

                    oSheet.Cells[DataRow+DataCount,4] = Convert.ToInt32(Row["AdCnt"].ToString());		// 3  �����
                    oSheet.Cells[DataRow+DataCount,5] = Convert.ToDecimal(Row["AdRate"].ToString());		// 4  %
                    

                    if(ORDCode.Equals("1") && (SubCount > 0)) // ��з� ������ �̰� �Һз��� ������
                    {
                        oRng = oSheet.get_Range(GetColumnIndex(1)+Convert.ToString(DataRow+DataCount), GetColumnIndex(5)+Convert.ToString(DataRow+DataCount));
                        oRng.Interior.Color      = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Yellow);   //�� ���� 
                    }
					else if (ORDCode.Equals("2") && (SubCount > 0)) // �Һз�2�� �̸鼭 �������� �Һз��� ������
					{
						// �����ڵ� : [E_01] �� ���� �߰� ��
						oRng = oSheet.get_Range(GetColumnIndex(2)+Convert.ToString(DataRow+DataCount), GetColumnIndex(5)+Convert.ToString(DataRow+DataCount));
						oRng.Interior.Color      = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.GreenYellow);   //�� ���� 
						//////////////////
					}
					else if (ORDCode.Equals("3") && (SubCount == 0))
					{
						// �����ڵ� : [E_01] �� ���� �߰� ��
						// 3 ������ 2���� �����̹Ƿ� ȸ������ ó�� �� ��.
						oRng = oSheet.get_Range(GetColumnIndex(2)+Convert.ToString(DataRow+DataCount), GetColumnIndex(5)+Convert.ToString(DataRow+DataCount));
						oRng.Interior.Color      = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.WhiteSmoke);   //�� ���� 						
					}


                    DataCount++;
					Application.DoEvents();
                }

                if(MergeCount > 1)
                {
                    oRng = oSheet.get_Range("A"+Convert.ToString(DataRow + TypeMergyStart), "A"+Convert.ToString(DataRow + DataCount-1));
                    oRng.Merge(false);
                }

                DataCount--;


                // ������ �ۼ�
                oRng = oSheet.get_Range(StartCol+Convert.ToString(HeaderRow), EndCol+Convert.ToString(DataRow+DataCount));	// �������� ����
                oRng.EntireColumn.AutoFit();					// �������� ũ�⿡ ���� ����ũ�� ����
                oRng.Borders.LineStyle =  Excel.XlLineStyle.xlContinuous;	// �׵θ��Ӽ� �Ʒ��� �Ǽ�
                oRng.Borders.Weight    = Excel.XlBorderWeight.xlThin;		// �׵θ��Ӽ� �Ʒ��� ���¼�

                // ����� �����Ϳ� ��Ÿ�� ����
                oRng = oSheet.get_Range(GetColumnIndex(3)+Convert.ToString(DataRow), GetColumnIndex(3)+Convert.ToString(DataRow+DataCount));	// �������� ����
                oRng.NumberFormatLocal = "#,##0";

                // % ��Ÿ�� ����
                oRng = oSheet.get_Range(GetColumnIndex(4)+Convert.ToString(DataRow), GetColumnIndex(4)+Convert.ToString(DataRow+DataCount));	// �������� ����
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

        private void OnSearch(object sender, SearchReportData e)
        {
            StatusMessage("������ ����� ��ȸ�մϴ�.");
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
			 
                uiPanelList.Text = "���������"; 
			 
                //  ��ü��� ���񽺸� ȣ���Ѵ�.
                new StatisticsRegionManager(systemModel,commonModel).GetStatisticsRegionReport(mainModel);
	
                if (mainModel.ResultCD.Equals("0000"))
                {
                    Utility.SetDataTable(statisticsRegionDs.Report, mainModel.ReportDataSet);		
                    StatusMessage(mainModel.ResultCnt + "���� ��ȸ�Ǿ����ϴ�.");
			 	
                    keyTotalAdCnt     = mainModel.TotalAdCnt.ToString();
                    keyContractAmt    = mainModel.ContractAmt.ToString();
			 	
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

                    ebTotalAdCnt.Text   = keyTotalAdCnt;
                    ebContractAmt.Text  = keyContractAmt;

                    uiPanelList.Text = keyMediaName + " | " + keyContractName + " | " + keyCampaignName + " | " + keyItemName;
                    uiPanelList.Text += " | " + keyReportBgnDay + " ~ " + keyReportEndDay;	
	
                    rptHeader.u_IsPrint = true;
                }
                else
                {
                    rptHeader.u_IsPrint = false;
                }
            }
            catch(FrameException fe)
            {
                rptHeader.u_IsPrint = false;
                FrameSystem.showMsgForm("ä�κ���� ��ȸ����", new string[] {fe.ErrCode, fe.ResultMsg});
            }
            catch(Exception ex)
            {
                rptHeader.u_IsPrint = false;
                FrameSystem.showMsgForm("ä�κ���� ��ȸ����",new string[] {"",ex.Message});
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

		private void grdExReport_DrawGridArea(object sender, Janus.Windows.GridEX.DrawGridAreaEventArgs e)
		{
			if(e.Column.Key=="RateBar")
			{

				//first, draw the background:
				e.Graphics.FillRectangle(e.BackBrush,e.Bounds);

				//Now, draw the percent bar:
				if(e.Row.Cells["AdRate"].Value != null)
				{
					// �����͸� ��������
					float percentValue = (float) e.Row.Cells["AdRate"].Value;

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

		private void rptHeader_Load(object sender, System.EventArgs e)
		{
		
		}		
    }
}
