// ===============================================================================
//
// ChannelDayControl.cs
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
using AdManagerClient.Common.Args;

namespace AdManagerClient
{
	/// <summary>
	/// ���� ��û��Ȳ ���� ���� ��Ʈ��
	/// </summary>
    public class ChannelDayControl : System.Windows.Forms.UserControl, IUserControl
	{
		#region �̺�Ʈ�ڵ鷯
		public event StatusEventHandler 			StatusEvent;			// �����̺�Ʈ �ڵ鷯
		public event ProgressEventHandler 			ProgressEvent;			// ó�����̺�Ʈ �ڵ鷯
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
			
		#region ��������� ��ü �� ����

		// �ý��� ���� : ȭ�����
		private SystemModel   systemModel   = FrameSystem.oSysModel;
		private CommonModel   commonModel   = FrameSystem.oComModel;
		private Logger        log           = FrameSystem.oLog;
		private MenuPower     menu          = FrameSystem.oMenu;

		// �޴��ڵ� : ������ �ʿ��� ȭ�鿡 �ʿ���
		public string        menuCode		= "";

		// ����� ������
		AreaDayModel areaDayModel  = new AreaDayModel();	// ���α׷��� �����û�����
		SummaryAdModel summaryAdModel  = new SummaryAdModel();	// ���α׷��� �����û�����

		// ȭ��ó���� ����
		private bool    canRead		= false;
		private bool    canPrint    = false;
        private string  winName     = "ä��-���Ϻ� ����";

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
		private AdManagerClient._60_ReportAd._11_AreaDay.AreaDayDs areaDayDs;
        private Janus.Windows.GridEX.EditControls.EditBox ebContractName;
        private Janus.Windows.CalendarCombo.CalendarCombo cbSearchStartDay;
        private System.Windows.Forms.Label lbSearchDate;
        private System.Windows.Forms.Label label5;
        private Janus.Windows.GridEX.EditControls.EditBox ebItemName;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label10;
        private Janus.Windows.CalendarCombo.CalendarCombo cbSearchEndDay;
        private System.Windows.Forms.Label label11;
        private Janus.Windows.EditControls.UIButton btnSearch;
        private Janus.Windows.EditControls.UIButton btnExcel;
		private System.ComponentModel.IContainer components;

		public ChannelDayControl()
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ChannelDayControl));
            this.uiPM = new Janus.Windows.UI.Dock.UIPanelManager(this.components);
            this.uiPanelChoiceAdSchedule = new Janus.Windows.UI.Dock.UIPanelGroup();
            this.uiPanelSearch = new Janus.Windows.UI.Dock.UIPanel();
            this.uiPanelSearchContainer = new Janus.Windows.UI.Dock.UIPanelInnerContainer();
            this.pnlSearch = new System.Windows.Forms.Panel();
            this.ebContractName = new Janus.Windows.GridEX.EditControls.EditBox();
            this.cbSearchStartDay = new Janus.Windows.CalendarCombo.CalendarCombo();
            this.lbSearchDate = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.ebItemName = new Janus.Windows.GridEX.EditControls.EditBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.cbSearchEndDay = new Janus.Windows.CalendarCombo.CalendarCombo();
            this.label11 = new System.Windows.Forms.Label();
            this.btnSearch = new Janus.Windows.EditControls.UIButton();
            this.btnExcel = new Janus.Windows.EditControls.UIButton();
            this.uiPanelList = new Janus.Windows.UI.Dock.UIPanel();
            this.uiPanelListContainer = new Janus.Windows.UI.Dock.UIPanelInnerContainer();
            this.grdExReport = new Janus.Windows.GridEX.GridEX();
            this.dvReport = new System.Data.DataView();
            this.areaDayDs = new AdManagerClient._60_ReportAd._11_AreaDay.AreaDayDs();
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
            ((System.ComponentModel.ISupportInitialize)(this.areaDayDs)).BeginInit();
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
            this.uiPanelChoiceAdSchedule.Text = "ä��-���Ϻ� ����";
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
            this.pnlSearch.Controls.Add(this.ebContractName);
            this.pnlSearch.Controls.Add(this.cbSearchStartDay);
            this.pnlSearch.Controls.Add(this.lbSearchDate);
            this.pnlSearch.Controls.Add(this.label5);
            this.pnlSearch.Controls.Add(this.ebItemName);
            this.pnlSearch.Controls.Add(this.label7);
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
            // ebContractName
            // 
            this.ebContractName.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ebContractName.Location = new System.Drawing.Point(512, 32);
            this.ebContractName.Name = "ebContractName";
            this.ebContractName.Size = new System.Drawing.Size(256, 21);
            this.ebContractName.TabIndex = 72;
            this.ebContractName.TabStop = false;
            this.ebContractName.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
            this.ebContractName.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
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
            this.cbSearchStartDay.TabIndex = 62;
            this.cbSearchStartDay.Value = new System.DateTime(2007, 9, 14, 0, 0, 0, 0);
            this.cbSearchStartDay.VisualStyle = Janus.Windows.CalendarCombo.VisualStyle.Office2007;
            this.cbSearchStartDay.ValueChanged += new System.EventHandler(this.cbSearchStartDay_ValueChanged);
            // 
            // lbSearchDate
            // 
            this.lbSearchDate.Font = new System.Drawing.Font("����", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lbSearchDate.Location = new System.Drawing.Point(8, 8);
            this.lbSearchDate.Name = "lbSearchDate";
            this.lbSearchDate.Size = new System.Drawing.Size(72, 21);
            this.lbSearchDate.TabIndex = 70;
            this.lbSearchDate.Text = "���������";
            this.lbSearchDate.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label5
            // 
            this.label5.Font = new System.Drawing.Font("����", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label5.Location = new System.Drawing.Point(464, 32);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(56, 21);
            this.label5.TabIndex = 66;
            this.label5.Text = "����";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ebItemName
            // 
            this.ebItemName.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ebItemName.ButtonStyle = Janus.Windows.GridEX.EditControls.EditButtonStyle.Ellipsis;
            this.ebItemName.Cursor = System.Windows.Forms.Cursors.Hand;
            this.ebItemName.Location = new System.Drawing.Point(80, 32);
            this.ebItemName.MaxLength = 1000;
            this.ebItemName.Name = "ebItemName";
            this.ebItemName.Size = new System.Drawing.Size(368, 21);
            this.ebItemName.TabIndex = 71;
            this.ebItemName.TabStop = false;
            this.ebItemName.Text = "��ȸ�� ���� �����Ͻʽÿ�";
            this.ebItemName.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
            this.ebItemName.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            this.ebItemName.ButtonClick += new System.EventHandler(this.ebItemName_ButtonClick);
            // 
            // label7
            // 
            this.label7.Font = new System.Drawing.Font("����", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label7.Location = new System.Drawing.Point(8, 32);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(48, 21);
            this.label7.TabIndex = 68;
            this.label7.Text = "����";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label10
            // 
            this.label10.Location = new System.Drawing.Point(184, 8);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(32, 21);
            this.label10.TabIndex = 69;
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
            this.cbSearchEndDay.TabIndex = 63;
            this.cbSearchEndDay.Value = new System.DateTime(2007, 9, 14, 0, 0, 0, 0);
            this.cbSearchEndDay.VisualStyle = Janus.Windows.CalendarCombo.VisualStyle.Office2007;
            // 
            // label11
            // 
            this.label11.Location = new System.Drawing.Point(320, 8);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(32, 21);
            this.label11.TabIndex = 67;
            this.label11.Text = "����";
            this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnSearch
            // 
            this.btnSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSearch.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSearch.Enabled = false;
            this.btnSearch.Location = new System.Drawing.Point(783, 4);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(104, 24);
            this.btnSearch.TabIndex = 64;
            this.btnSearch.Text = "�� ȸ";
            this.btnSearch.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // btnExcel
            // 
            this.btnExcel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExcel.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnExcel.Enabled = false;
            this.btnExcel.Location = new System.Drawing.Point(895, 4);
            this.btnExcel.Name = "btnExcel";
            this.btnExcel.Size = new System.Drawing.Size(104, 24);
            this.btnExcel.TabIndex = 65;
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
            this.grdExReport.DataSource = this.dvReport;
            grdExReport_DesignTimeLayout.LayoutString = resources.GetString("grdExReport_DesignTimeLayout.LayoutString");
            this.grdExReport.DesignTimeLayout = grdExReport_DesignTimeLayout;
            this.grdExReport.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdExReport.EditorsControlStyle.ButtonAppearance = Janus.Windows.GridEX.ButtonAppearance.Regular;
            this.grdExReport.EmptyRows = true;
            this.grdExReport.FocusCellFormatStyle.BackColor = System.Drawing.SystemColors.Highlight;
            this.grdExReport.FocusCellFormatStyle.ForeColor = System.Drawing.Color.White;
            this.grdExReport.FocusStyle = Janus.Windows.GridEX.FocusStyle.None;
            this.grdExReport.Font = new System.Drawing.Font("����ü", 9F);
            this.grdExReport.GridLineColor = System.Drawing.Color.Silver;
            this.grdExReport.GridLines = Janus.Windows.GridEX.GridLines.Vertical;
            this.grdExReport.GridLineStyle = Janus.Windows.GridEX.GridLineStyle.Solid;
            this.grdExReport.GroupByBoxVisible = false;
            this.grdExReport.HeaderFormatStyle.TextAlignment = Janus.Windows.GridEX.TextAlignment.Center;
            this.grdExReport.HideSelection = Janus.Windows.GridEX.HideSelection.Highlight;
            this.grdExReport.Location = new System.Drawing.Point(0, 0);
            this.grdExReport.Name = "grdExReport";
            this.grdExReport.ScrollBars = Janus.Windows.GridEX.ScrollBars.Vertical;
            this.grdExReport.SelectedInactiveFormatStyle.BackColor = System.Drawing.Color.Gold;
            this.grdExReport.Size = new System.Drawing.Size(1008, 564);
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
            this.grdExReport.TotalRowFormatStyle.BackColor = System.Drawing.Color.Yellow;
            this.grdExReport.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            // 
            // dvReport
            // 
            this.dvReport.Table = this.areaDayDs.ChannelDay;
            // 
            // areaDayDs
            // 
            this.areaDayDs.DataSetName = "AreaDayDs";
            this.areaDayDs.Locale = new System.Globalization.CultureInfo("en-US");
            this.areaDayDs.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
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
            // ChannelDayControl
            // 
            this.Controls.Add(this.uiPanelChoiceAdSchedule);
            this.Name = "ChannelDayControl";
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
            ((System.ComponentModel.ISupportInitialize)(this.areaDayDs)).EndInit();
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
            if(menu.CanRead(MenuCode))  canRead = true;               
            InitButton();
            ProgressStop();
        }

        private void InitCombo()
        {
            InitCombo_Start();
            InitCombo_End();			
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
        
        #region [�޼ҵ�] ����� ����ó��
        /// <summary>
        /// ��ȸ��ư
        /// </summary>
        private void btnSearch_Click(object sender, System.EventArgs e)
        {
            DisableButton();
            SearchReport();
            InitButton();
        }

        /// <summary>
        /// ������ư
        /// </summary>
        private void btnExcel_Click(object sender, System.EventArgs e)
        {
            this.MakeExcel();
        }

        /// <summary>
        /// ������ ��ư
        /// </summary>
        private void ebItemName_ButtonClick(object sender, System.EventArgs e)
        {
            DisableButton();

            ItemMultiChoiceForm pForm = new ItemMultiChoiceForm();
            //Searchitem pForm = new Searchitem();
            //pForm.ItemSelected += new AdItemSelectEventHandler(OnItemSelected);
            pForm.callType = "ChannelDayControl";
            pForm.ReturnDate += new ItemMultiChoiceForm.PopupService(ItemMultiChoiceForm_Return);
            pForm.ShowDialog();
            pForm.Dispose();
            pForm = null;

            InitButton();
        }


        #region ������ �̺�Ʈ ó����

        /// <summary>
        /// ���õ� �����ȣ�� ��� �迭
        /// </summary>
        private string adItemName = "";
        private string adContractName = "";
        private ArrayList _itemList = new ArrayList();
        private void ItemMultiChoiceForm_Return(object sender, EventArgs args)
        {
            ItemEventArgs itemEventArgs = (ItemEventArgs)args;
            ItemMultiChoice_pDs itemMultiChoice_pDs = (ItemMultiChoice_pDs)itemEventArgs.dataSet;
            try
            {
                // ���� ������ ����
                if (_itemList != null && _itemList.Count > 0) _itemList.Clear();

                //�μ�Ʈ ��Ŵ
                for (int i = 0; i < itemMultiChoice_pDs.ChoiceAdItems.Rows.Count; i++)
                {
                    DataRow row = itemMultiChoice_pDs.ChoiceAdItems.Rows[i];

                    if (row["CheckYn"].ToString().Equals("True"))
                    {
                        adItemName = row["ItemName"].ToString();
                        adContractName = row["ContractName"].ToString();
                        _itemList.Add(row["ItemNo"].ToString());
                    }
                }

                // ���õ� ���� �ִ� ���
                if (_itemList.Count > 0)
                {
                    int itemCnt = _itemList.Count - 1;
                    ebContractName.Text = adContractName;
                    ebItemName.Text = "'" + adItemName + "' �� " + itemCnt.ToString() + "��";
                }
            }
            catch (FrameException fe)
            {
                FrameSystem.showMsgForm("������ ����", new string[] { fe.ErrCode, fe.ResultMsg });
            }
            catch (Exception ex)
            {
                FrameSystem.showMsgForm("������ ����", new string[] { "", ex.Message });
            }
        }
        #endregion


        /// <summary>
        /// ��������� ����
        /// </summary>
        private void cbSearchStartDay_ValueChanged(object sender, System.EventArgs e)
        {
            if( cbSearchStartDay.Value > cbSearchEndDay.Value )
            {
                cbSearchEndDay.Value = cbSearchStartDay.Value;
            }
        }

        #endregion

        #region [�Լ�] ��ȸó��

        /// <summary>
        /// ��ü��� ��ȸ
        /// </summary>
        private void SearchReport()
        {

            StatusMessage( winName + "�� ��ȸ�մϴ�.");
			
            if( _itemList.Count == 0 )
            {
                MessageBox.Show("���� ������ �ֽʽÿ�.", winName,MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            ProgressStart();

            try
            {
                // �����͸� �ʱ�ȭ
                areaDayModel.Init();
                areaDayModel.StartDay  	 =  cbSearchStartDay.Value.ToString("yyMMdd");
                areaDayModel.EndDay  	 =  cbSearchEndDay.Value.ToString("yyMMdd");
                areaDayModel.AdList      =  _itemList;
				
                uiPanelList.Text = winName;

                //  ��ü��� ���񽺸� ȣ���Ѵ�.
                new AreaDayManager(systemModel,commonModel).GetChannelDay(areaDayModel);

                if (areaDayModel.ResultCD.Equals("0000"))
                {
                    Utility.SetDataTable(areaDayDs.ChannelDay, areaDayModel.ReportDataSet);		
                    StatusMessage(areaDayModel.ResultCnt + "���� ��ȸ�Ǿ����ϴ�.");		
		
                    uiPanelList.Text += " || " + ebContractName.Text;
                    uiPanelList.Text += " || " + ebItemName.Text;
                    uiPanelList.Text += " || " + cbSearchStartDay.Value.ToString("yyyy-MM-dd") + " ~ " + cbSearchEndDay.Value.ToString("yyyy-MM-dd");
                    canPrint = true;
                }
            }
            catch(FrameException fe)
            {
                FrameSystem.showMsgForm(winName + " ��ȸ����", new string[] {fe.ErrCode, fe.ResultMsg});
            }
            catch(Exception ex)
            {
                FrameSystem.showMsgForm(winName + " ��ȸ����",new string[] {"",ex.Message});
            }
            finally
            {
                ProgressStop();
            }
        }
        #endregion
		
        #region [�Լ�] ��������
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
                int ColMax  = 10; // �÷���   				

                int TitleRow  = 1;
                int ConditionRow = 2;
                int HeaderRow = 6;
                int DataRow   = 7;
                string StartCol = "A";
                string EndCol   = "";
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
                oSheet.Cells[TitleRow,1] = "ä��-���Ϻ� ����";
                oRng = oSheet.get_Range(StartCol+Convert.ToString(TitleRow), TitleCol+Convert.ToString(TitleRow));
                oRng.Merge(true);
                oRng.Font.Bold = true;
                oRng.Font.Size = 16;
                oRng.VerticalAlignment   = Excel.XlVAlign.xlVAlignCenter;			
                oRng.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;		

                oSheet.Cells[ConditionRow+CondCount,1] = "���������";
                oRng = oSheet.get_Range("B"+Convert.ToString(ConditionRow+CondCount), TitleCol+Convert.ToString(ConditionRow+CondCount));
                oRng.Merge(true);
                oSheet.Cells[ConditionRow+CondCount,2] = cbSearchStartDay.Value.ToString("yyyy-MM-dd") + " ~ " + cbSearchEndDay.Value.ToString("yyyy-MM-dd");
                CondCount++;

                oSheet.Cells[ConditionRow+CondCount,1] = "����";
                oRng = oSheet.get_Range("B"+Convert.ToString(ConditionRow+CondCount), TitleCol+Convert.ToString(ConditionRow+CondCount));
                oRng.Merge(true);
                oSheet.Cells[ConditionRow+CondCount,2] = ebContractName.Text;
                CondCount++;

                oSheet.Cells[ConditionRow+CondCount,1] = "�����";
                oRng = oSheet.get_Range("B"+Convert.ToString(ConditionRow+CondCount), TitleCol+Convert.ToString(ConditionRow+CondCount));
                oRng.Merge(true);
                oSheet.Cells[ConditionRow+CondCount,2] = ebItemName.Text;
                CondCount++;

                // ���Ǻ� �׵θ�
                oRng = oSheet.get_Range(StartCol+Convert.ToString(ConditionRow), TitleCol+Convert.ToString(ConditionRow+(CondCount-1)));
                oRng.VerticalAlignment   = Excel.XlVAlign.xlVAlignCenter;			
                oRng.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;		
                oRng.Borders.LineStyle =  Excel.XlLineStyle.xlContinuous;	// �׵θ��Ӽ� �Ʒ��� �Ǽ�
                oRng.Borders.Weight    = Excel.XlBorderWeight.xlThin;		// �׵θ��Ӽ� �Ʒ��� ���¼�
	
                HeaderCount = 1;
                oSheet.Cells[HeaderRow,HeaderCount++] = "ī�װ�";
                oSheet.Cells[HeaderRow,HeaderCount++] = "�帣";
                oSheet.Cells[HeaderRow,HeaderCount++] = "ä��";
                oSheet.Cells[HeaderRow,HeaderCount++] = "��";
                oSheet.Cells[HeaderRow,HeaderCount++] = "ȭ";
                oSheet.Cells[HeaderRow,HeaderCount++] = "��";
                oSheet.Cells[HeaderRow,HeaderCount++] = "��";
                oSheet.Cells[HeaderRow,HeaderCount++] = "��";
                oSheet.Cells[HeaderRow,HeaderCount++] = "��";
                oSheet.Cells[HeaderRow,HeaderCount++] = "��";
				
                oRng = oSheet.get_Range(StartCol+Convert.ToString(HeaderRow), EndCol+Convert.ToString(HeaderRow));	// ����� ����
                oRng.Font.Bold           = true;							// ��Ʈ ����
                oRng.VerticalAlignment   = Excel.XlVAlign.xlVAlignCenter;	// �����߾�����
                oRng.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;	// �����߾�����	 
                oRng.Interior.Color      = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.CornflowerBlue);   //�� ���� 
                oRng.Font.Color          = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.White);			//�ؽ�Ʈ��			
				
                DataCount = 0;
                // ������ ����
                for (int inx =0; inx < areaDayDs.ChannelDay.Rows.Count; inx++)
                {		
                    DataRow Row = areaDayDs.ChannelDay.Rows[inx];

                    int ColCnt = 1;

                    oSheet.Cells[DataRow+DataCount,ColCnt++] = Row["CategoryNm"].ToString();								// 1  �����ȣ
                    oSheet.Cells[DataRow+DataCount,ColCnt++] = Row["GenreNm"].ToString();								// 1  �����ȣ
                    oSheet.Cells[DataRow+DataCount,ColCnt++] = Row["ChannelNm"].ToString();								// 1  �����ȣ
                    oSheet.Cells[DataRow+DataCount,ColCnt++] = Convert.ToInt64(Row["Mon"].ToString());								// 2  �����
                    oSheet.Cells[DataRow+DataCount,ColCnt++] = Convert.ToInt64(Row["Tue"].ToString());				// 3  ��ûȽ��
                    oSheet.Cells[DataRow+DataCount,ColCnt++] = Convert.ToDecimal(Row["Wed"].ToString());			// 4  ��û����
                    oSheet.Cells[DataRow+DataCount,ColCnt++] = Convert.ToInt64(Row["Thu"].ToString());			// 5  Rate
                    oSheet.Cells[DataRow+DataCount,ColCnt++] = Convert.ToInt64(Row["Fri"].ToString());			// 6  Cove
                    oSheet.Cells[DataRow+DataCount,ColCnt++] = Convert.ToDecimal(Row["Sat"].ToString());			// 7  Freq
                    oSheet.Cells[DataRow+DataCount,ColCnt++] = Convert.ToDecimal(Row["Sun"].ToString());			// 7  Freq

                    DataCount++;
                }

                DataCount--;

                // ������ �ۼ�
                oRng = oSheet.get_Range(StartCol+Convert.ToString(HeaderRow), EndCol+Convert.ToString(DataRow+DataCount));	// �������� ����
                oRng.EntireColumn.AutoFit();					// �������� ũ�⿡ ���� ����ũ�� ����
                oRng.Borders.LineStyle =  Excel.XlLineStyle.xlContinuous;	// �׵θ��Ӽ� �Ʒ��� �Ǽ�
                oRng.Borders.Weight    = Excel.XlBorderWeight.xlThin;		// �׵θ��Ӽ� �Ʒ��� ���¼�
                //�����ȣ �߾�����
                oRng = oSheet.get_Range(GetColumnIndex(1)+Convert.ToString(DataRow), GetColumnIndex(1)+Convert.ToString(DataRow+DataCount)); 
                oRng.HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft;
				
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
                oRng.NumberFormatLocal = "#,##0";

                //Freq right����
                oRng = oSheet.get_Range(GetColumnIndex(8)+Convert.ToString(DataRow), GetColumnIndex(8)+Convert.ToString(DataRow+DataCount)); 
                oRng.HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;
                oRng.NumberFormatLocal = "#,##0";
			
                //Freq right����
                oRng = oSheet.get_Range(GetColumnIndex(9)+Convert.ToString(DataRow), GetColumnIndex(9)+Convert.ToString(DataRow+DataCount)); 
                oRng.HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;
                oRng.NumberFormatLocal = "#,##0";

                //Freq right����
                oRng = oSheet.get_Range(GetColumnIndex(10)+Convert.ToString(DataRow), GetColumnIndex(10)+Convert.ToString(DataRow+DataCount)); 
                oRng.HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;
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

	}
}
