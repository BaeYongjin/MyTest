// ===============================================================================
// SchChoiceAdControl for Charites Project
//
// SchChoiceAdControl.cs
//
// ���������� ��Ʈ���� �����մϴ�. 
//
// ===============================================================================
// Release history
//
// ===============================================================================
// Copyright (C) 2006 G-Inno Systems Inc.
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

using Janus.Windows.GridEX;

using WinFramework.Misc;
using WinFramework.Base;
using WinFramework.Data;

using AdManagerModel;

namespace AdManagerClient
{
	/// <summary>
	/// �������ϰ��� ��Ʈ��
	/// </summary>
    public class SchDesignateAdControl : System.Windows.Forms.UserControl, IUserControl
	{
		#region [ ��뺯���� ������Ʈ ���� ]
		SchDesignateModel	mainModel		= new SchDesignateModel();
		SchPublishModel		schPublishModel	= new SchPublishModel();

		CurrencyManager cm        = null;
		DataTable       dt        = null;

		// ������ ���λ��� ó����
		private string keyAckNoCm    = "";
		private string keyAckStateCm = "";

		private	int		keyMedia	= 0;
		private	int		keyItemNo	= 0;
		private	string	keyItemName	= "";

        bool IsSearching = false; // ��ȸ�� ��ȭ���� ������Ʈ �Ǵ� ���� ���� �ϱ����� 2011.11.29 JH.Park

		#endregion

        #region IUserControl ����

        // �޴��ڵ� : ������ �ʿ��� ȭ�鿡 �ʿ���
        private string menuCode = "";

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
		private System.Windows.Forms.Panel pnlSearchBtn;
		private Janus.Windows.EditControls.UIButton btnSearch;
		private Janus.Windows.UI.Dock.UIPanel uiPanelSearch;
		private Janus.Windows.UI.Dock.UIPanelInnerContainer uiPanelSearchContainer;
		private Janus.Windows.UI.Dock.UIPanel uiPanelList;
		private Janus.Windows.UI.Dock.UIPanelInnerContainer uiPanelListContainer;
		private System.Data.DataView dvSchedule;
		private Janus.Windows.EditControls.UICheckBox chkAdState_20;
		private System.Windows.Forms.Label lbAdState;
		private Janus.Windows.EditControls.UIComboBox cbSearchMedia;
		private Janus.Windows.EditControls.UIComboBox cbSearchRap;
		private Janus.Windows.EditControls.UICheckBox chkAdState_40;
		private Janus.Windows.GridEX.GridEX grdExScheduleList;
		private Janus.Windows.EditControls.UICheckBox chkAdState_30;
		private Janus.Windows.EditControls.UIButton btnAdd;
		private Janus.Windows.EditControls.UIButton btnDelete;
		private System.Windows.Forms.Label label1;
		private Janus.Windows.GridEX.EditControls.EditBox editBox1;
		private System.Windows.Forms.Panel pnlDetail;
		private System.Windows.Forms.Label lbMsg;
		private Janus.Windows.UI.Dock.UIPanelGroup uiPanel;
		private Janus.Windows.UI.Dock.UIPanel uiPanelCommand;
		private Janus.Windows.UI.Dock.UIPanelInnerContainer uiPanelCommandContainer;
		private AdManagerClient._30_Schedule._16_SchDesignatedAd.SchDesignateAdDs schDesignateAdDs;
		private System.ComponentModel.IContainer components;

		public SchDesignateAdControl()
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
            Janus.Windows.GridEX.GridEXLayout grdExScheduleList_DesignTimeLayout = new Janus.Windows.GridEX.GridEXLayout();
            Janus.Windows.Common.Layouts.JanusLayoutReference grdExScheduleList_DesignTimeLayout_Reference_0 = new Janus.Windows.Common.Layouts.JanusLayoutReference("GridEXLayoutData.RootTable.FormatConditions.Condition0.FormatStyle.BackgroundImag" +
        "e");
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SchDesignateAdControl));
            Janus.Windows.Common.Layouts.JanusLayoutReference grdExScheduleList_DesignTimeLayout_Reference_1 = new Janus.Windows.Common.Layouts.JanusLayoutReference("GridEXLayoutData.RootTable.FormatConditions.Condition1.FormatStyle.BackgroundImag" +
        "e");
            Janus.Windows.Common.Layouts.JanusLayoutReference grdExScheduleList_DesignTimeLayout_Reference_2 = new Janus.Windows.Common.Layouts.JanusLayoutReference("GridEXLayoutData.RootTable.FormatConditions.Condition2.FormatStyle.BackgroundImag" +
        "e");
            Janus.Windows.Common.Layouts.JanusLayoutReference grdExScheduleList_DesignTimeLayout_Reference_3 = new Janus.Windows.Common.Layouts.JanusLayoutReference("GridEXLayoutData.RootTable.FormatConditions.Condition3.FormatStyle.BackgroundImag" +
        "e");
            Janus.Windows.Common.Layouts.JanusLayoutReference grdExScheduleList_DesignTimeLayout_Reference_4 = new Janus.Windows.Common.Layouts.JanusLayoutReference("GridEXLayoutData.RootTable.FormatConditions.Condition4.FormatStyle.BackgroundImag" +
        "e");
            this.uiPM = new Janus.Windows.UI.Dock.UIPanelManager(this.components);
            this.uiPanel = new Janus.Windows.UI.Dock.UIPanelGroup();
            this.uiPanelSearch = new Janus.Windows.UI.Dock.UIPanel();
            this.uiPanelSearchContainer = new Janus.Windows.UI.Dock.UIPanelInnerContainer();
            this.pnlSearch = new System.Windows.Forms.Panel();
            this.chkAdState_20 = new Janus.Windows.EditControls.UICheckBox();
            this.lbAdState = new System.Windows.Forms.Label();
            this.cbSearchMedia = new Janus.Windows.EditControls.UIComboBox();
            this.cbSearchRap = new Janus.Windows.EditControls.UIComboBox();
            this.pnlSearchBtn = new System.Windows.Forms.Panel();
            this.btnSearch = new Janus.Windows.EditControls.UIButton();
            this.chkAdState_30 = new Janus.Windows.EditControls.UICheckBox();
            this.chkAdState_40 = new Janus.Windows.EditControls.UICheckBox();
            this.uiPanelList = new Janus.Windows.UI.Dock.UIPanel();
            this.uiPanelListContainer = new Janus.Windows.UI.Dock.UIPanelInnerContainer();
            this.grdExScheduleList = new Janus.Windows.GridEX.GridEX();
            this.dvSchedule = new System.Data.DataView();
            this.schDesignateAdDs = new AdManagerClient._30_Schedule._16_SchDesignatedAd.SchDesignateAdDs();
            this.uiPanelCommand = new Janus.Windows.UI.Dock.UIPanel();
            this.uiPanelCommandContainer = new Janus.Windows.UI.Dock.UIPanelInnerContainer();
            this.pnlDetail = new System.Windows.Forms.Panel();
            this.lbMsg = new System.Windows.Forms.Label();
            this.btnDelete = new Janus.Windows.EditControls.UIButton();
            this.btnAdd = new Janus.Windows.EditControls.UIButton();
            this.label1 = new System.Windows.Forms.Label();
            this.editBox1 = new Janus.Windows.GridEX.EditControls.EditBox();
            ((System.ComponentModel.ISupportInitialize)(this.uiPM)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanel)).BeginInit();
            this.uiPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelSearch)).BeginInit();
            this.uiPanelSearch.SuspendLayout();
            this.uiPanelSearchContainer.SuspendLayout();
            this.pnlSearch.SuspendLayout();
            this.pnlSearchBtn.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelList)).BeginInit();
            this.uiPanelList.SuspendLayout();
            this.uiPanelListContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdExScheduleList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dvSchedule)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.schDesignateAdDs)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelCommand)).BeginInit();
            this.uiPanelCommand.SuspendLayout();
            this.uiPanelCommandContainer.SuspendLayout();
            this.pnlDetail.SuspendLayout();
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
            this.uiPanel.Id = new System.Guid("b162c53a-7940-47cd-a0ed-9762c59c50d8");
            this.uiPanel.StaticGroup = true;
            this.uiPanelSearch.Id = new System.Guid("afb3b7d0-aa79-443c-a1e8-3baba38f7a4d");
            this.uiPanel.Panels.Add(this.uiPanelSearch);
            this.uiPanelList.Id = new System.Guid("8e3140b9-bfcb-43af-b84e-2ee6cddf51dc");
            this.uiPanel.Panels.Add(this.uiPanelList);
            this.uiPanelCommand.Id = new System.Guid("b2973a57-d3e1-4d80-9387-7a6dc3332542");
            this.uiPanel.Panels.Add(this.uiPanelCommand);
            this.uiPM.Panels.Add(this.uiPanel);
            // 
            // Design Time Panel Info:
            // 
            this.uiPM.BeginPanelInfo();
            this.uiPM.AddDockPanelInfo(new System.Guid("b162c53a-7940-47cd-a0ed-9762c59c50d8"), Janus.Windows.UI.Dock.PanelGroupStyle.HorizontalTiles, Janus.Windows.UI.Dock.PanelDockStyle.Fill, true, new System.Drawing.Size(1010, 677), true);
            this.uiPM.AddDockPanelInfo(new System.Guid("afb3b7d0-aa79-443c-a1e8-3baba38f7a4d"), new System.Guid("b162c53a-7940-47cd-a0ed-9762c59c50d8"), 41, true);
            this.uiPM.AddDockPanelInfo(new System.Guid("8e3140b9-bfcb-43af-b84e-2ee6cddf51dc"), new System.Guid("b162c53a-7940-47cd-a0ed-9762c59c50d8"), 546, true);
            this.uiPM.AddDockPanelInfo(new System.Guid("b2973a57-d3e1-4d80-9387-7a6dc3332542"), new System.Guid("b162c53a-7940-47cd-a0ed-9762c59c50d8"), 40, true);
            this.uiPM.AddFloatingPanelInfo(new System.Guid("b162c53a-7940-47cd-a0ed-9762c59c50d8"), Janus.Windows.UI.Dock.PanelGroupStyle.HorizontalTiles, true, new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
            this.uiPM.AddFloatingPanelInfo(new System.Guid("8e3140b9-bfcb-43af-b84e-2ee6cddf51dc"), new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
            this.uiPM.AddFloatingPanelInfo(new System.Guid("b2973a57-d3e1-4d80-9387-7a6dc3332542"), new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
            this.uiPM.AddFloatingPanelInfo(new System.Guid("afb3b7d0-aa79-443c-a1e8-3baba38f7a4d"), new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
            this.uiPM.AddFloatingPanelInfo(new System.Guid("18e78aea-bf80-4094-94f6-0d7d6006bb3d"), Janus.Windows.UI.Dock.PanelGroupStyle.VerticalTiles, true, new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
            this.uiPM.AddFloatingPanelInfo(new System.Guid("02c920f2-57c5-4aa8-bf42-309880d3314e"), new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
            this.uiPM.AddFloatingPanelInfo(new System.Guid("f40b18e5-7e83-4164-be4f-97c23451198d"), new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
            this.uiPM.EndPanelInfo();
            // 
            // uiPanel
            // 
            this.uiPanel.ActiveCaptionMode = Janus.Windows.UI.Dock.ActiveCaptionMode.Never;
            this.uiPanel.BackColor = System.Drawing.SystemColors.Window;
            this.uiPanel.CloseButtonVisible = Janus.Windows.UI.InheritableBoolean.False;
            this.uiPanel.Location = new System.Drawing.Point(0, 0);
            this.uiPanel.Name = "uiPanel";
            this.uiPanel.Size = new System.Drawing.Size(1010, 677);
            this.uiPanel.TabIndex = 4;
            this.uiPanel.Text = "ä�κ� ������";
            // 
            // uiPanelSearch
            // 
            this.uiPanelSearch.AllowResize = Janus.Windows.UI.InheritableBoolean.False;
            this.uiPanelSearch.CaptionVisible = Janus.Windows.UI.InheritableBoolean.False;
            this.uiPanelSearch.CloseButtonVisible = Janus.Windows.UI.InheritableBoolean.False;
            this.uiPanelSearch.InnerContainer = this.uiPanelSearchContainer;
            this.uiPanelSearch.Location = new System.Drawing.Point(0, 22);
            this.uiPanelSearch.Name = "uiPanelSearch";
            this.uiPanelSearch.Size = new System.Drawing.Size(1010, 42);
            this.uiPanelSearch.TabIndex = 4;
            this.uiPanelSearch.Text = "��ȸ����";
            // 
            // uiPanelSearchContainer
            // 
            this.uiPanelSearchContainer.Controls.Add(this.pnlSearch);
            this.uiPanelSearchContainer.Location = new System.Drawing.Point(1, 1);
            this.uiPanelSearchContainer.Name = "uiPanelSearchContainer";
            this.uiPanelSearchContainer.Size = new System.Drawing.Size(1008, 40);
            this.uiPanelSearchContainer.TabIndex = 0;
            // 
            // pnlSearch
            // 
            this.pnlSearch.BackColor = System.Drawing.SystemColors.Window;
            this.pnlSearch.Controls.Add(this.chkAdState_20);
            this.pnlSearch.Controls.Add(this.lbAdState);
            this.pnlSearch.Controls.Add(this.cbSearchMedia);
            this.pnlSearch.Controls.Add(this.cbSearchRap);
            this.pnlSearch.Controls.Add(this.pnlSearchBtn);
            this.pnlSearch.Controls.Add(this.chkAdState_30);
            this.pnlSearch.Controls.Add(this.chkAdState_40);
            this.pnlSearch.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlSearch.Location = new System.Drawing.Point(0, 0);
            this.pnlSearch.Name = "pnlSearch";
            this.pnlSearch.Size = new System.Drawing.Size(1008, 40);
            this.pnlSearch.TabIndex = 0;
            // 
            // chkAdState_20
            // 
            this.chkAdState_20.Checked = true;
            this.chkAdState_20.CheckedValue = "";
            this.chkAdState_20.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkAdState_20.ForeColor = System.Drawing.SystemColors.ControlText;
            this.chkAdState_20.Location = new System.Drawing.Point(350, 8);
            this.chkAdState_20.Name = "chkAdState_20";
            this.chkAdState_20.Size = new System.Drawing.Size(44, 22);
            this.chkAdState_20.TabIndex = 8;
            this.chkAdState_20.Text = "��";
            this.chkAdState_20.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            // 
            // lbAdState
            // 
            this.lbAdState.Location = new System.Drawing.Point(286, 8);
            this.lbAdState.Name = "lbAdState";
            this.lbAdState.Size = new System.Drawing.Size(57, 22);
            this.lbAdState.TabIndex = 13;
            this.lbAdState.Text = "�������";
            this.lbAdState.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cbSearchMedia
            // 
            this.cbSearchMedia.AutoSize = false;
            this.cbSearchMedia.BackColor = System.Drawing.Color.White;
            this.cbSearchMedia.ComboStyle = Janus.Windows.EditControls.ComboStyle.DropDownList;
            this.cbSearchMedia.Location = new System.Drawing.Point(8, 8);
            this.cbSearchMedia.Name = "cbSearchMedia";
            this.cbSearchMedia.Size = new System.Drawing.Size(120, 22);
            this.cbSearchMedia.TabIndex = 1;
            this.cbSearchMedia.Text = "��ü����";
            this.cbSearchMedia.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            // 
            // cbSearchRap
            // 
            this.cbSearchRap.AutoSize = false;
            this.cbSearchRap.ComboStyle = Janus.Windows.EditControls.ComboStyle.DropDownList;
            this.cbSearchRap.Location = new System.Drawing.Point(136, 8);
            this.cbSearchRap.Name = "cbSearchRap";
            this.cbSearchRap.Size = new System.Drawing.Size(120, 22);
            this.cbSearchRap.TabIndex = 2;
            this.cbSearchRap.Text = "�̵�����";
            this.cbSearchRap.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            // 
            // pnlSearchBtn
            // 
            this.pnlSearchBtn.BackColor = System.Drawing.SystemColors.Window;
            this.pnlSearchBtn.Controls.Add(this.btnSearch);
            this.pnlSearchBtn.Dock = System.Windows.Forms.DockStyle.Right;
            this.pnlSearchBtn.Location = new System.Drawing.Point(888, 0);
            this.pnlSearchBtn.Name = "pnlSearchBtn";
            this.pnlSearchBtn.Size = new System.Drawing.Size(120, 40);
            this.pnlSearchBtn.TabIndex = 11;
            // 
            // btnSearch
            // 
            this.btnSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSearch.ButtonStyle = Janus.Windows.EditControls.ButtonStyle.Button;
            this.btnSearch.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSearch.Enabled = false;
            this.btnSearch.Location = new System.Drawing.Point(8, 8);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.ShowFocusRectangle = false;
            this.btnSearch.Size = new System.Drawing.Size(104, 24);
            this.btnSearch.TabIndex = 11;
            this.btnSearch.Text = "�� ȸ";
            this.btnSearch.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // chkAdState_30
            // 
            this.chkAdState_30.BackColor = System.Drawing.SystemColors.Window;
            this.chkAdState_30.Checked = true;
            this.chkAdState_30.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkAdState_30.Location = new System.Drawing.Point(414, 8);
            this.chkAdState_30.Name = "chkAdState_30";
            this.chkAdState_30.Size = new System.Drawing.Size(44, 22);
            this.chkAdState_30.TabIndex = 9;
            this.chkAdState_30.Text = "����";
            this.chkAdState_30.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            // 
            // chkAdState_40
            // 
            this.chkAdState_40.Location = new System.Drawing.Point(478, 8);
            this.chkAdState_40.Name = "chkAdState_40";
            this.chkAdState_40.Size = new System.Drawing.Size(44, 22);
            this.chkAdState_40.TabIndex = 10;
            this.chkAdState_40.Text = "����";
            this.chkAdState_40.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            // 
            // uiPanelList
            // 
            this.uiPanelList.AllowResize = Janus.Windows.UI.InheritableBoolean.True;
            this.uiPanelList.BackColor = System.Drawing.SystemColors.Window;
            this.uiPanelList.CaptionStyle = Janus.Windows.UI.Dock.PanelCaptionStyle.Dark;
            this.uiPanelList.InnerContainer = this.uiPanelListContainer;
            this.uiPanelList.Location = new System.Drawing.Point(0, 68);
            this.uiPanelList.MinimumSize = new System.Drawing.Size(-1, 100);
            this.uiPanelList.Name = "uiPanelList";
            this.uiPanelList.Size = new System.Drawing.Size(1010, 564);
            this.uiPanelList.TabIndex = 4;
            this.uiPanelList.Text = "����Ȳ";
            // 
            // uiPanelListContainer
            // 
            this.uiPanelListContainer.BackColor = System.Drawing.SystemColors.Window;
            this.uiPanelListContainer.Controls.Add(this.grdExScheduleList);
            this.uiPanelListContainer.Location = new System.Drawing.Point(1, 23);
            this.uiPanelListContainer.Name = "uiPanelListContainer";
            this.uiPanelListContainer.Size = new System.Drawing.Size(1008, 540);
            this.uiPanelListContainer.TabIndex = 0;
            // 
            // grdExScheduleList
            // 
            this.grdExScheduleList.AlternatingColors = true;
            this.grdExScheduleList.BorderStyle = Janus.Windows.GridEX.BorderStyle.None;
            this.grdExScheduleList.DataSource = this.dvSchedule;
            grdExScheduleList_DesignTimeLayout_Reference_0.Instance = ((object)(resources.GetObject("grdExScheduleList_DesignTimeLayout_Reference_0.Instance")));
            grdExScheduleList_DesignTimeLayout_Reference_1.Instance = ((object)(resources.GetObject("grdExScheduleList_DesignTimeLayout_Reference_1.Instance")));
            grdExScheduleList_DesignTimeLayout_Reference_2.Instance = ((object)(resources.GetObject("grdExScheduleList_DesignTimeLayout_Reference_2.Instance")));
            grdExScheduleList_DesignTimeLayout_Reference_3.Instance = ((object)(resources.GetObject("grdExScheduleList_DesignTimeLayout_Reference_3.Instance")));
            grdExScheduleList_DesignTimeLayout_Reference_4.Instance = ((object)(resources.GetObject("grdExScheduleList_DesignTimeLayout_Reference_4.Instance")));
            grdExScheduleList_DesignTimeLayout.LayoutReferences.AddRange(new Janus.Windows.Common.Layouts.JanusLayoutReference[] {
            grdExScheduleList_DesignTimeLayout_Reference_0,
            grdExScheduleList_DesignTimeLayout_Reference_1,
            grdExScheduleList_DesignTimeLayout_Reference_2,
            grdExScheduleList_DesignTimeLayout_Reference_3,
            grdExScheduleList_DesignTimeLayout_Reference_4});
            grdExScheduleList_DesignTimeLayout.LayoutString = resources.GetString("grdExScheduleList_DesignTimeLayout.LayoutString");
            this.grdExScheduleList.DesignTimeLayout = grdExScheduleList_DesignTimeLayout;
            this.grdExScheduleList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdExScheduleList.EmptyRows = true;
            this.grdExScheduleList.FocusCellFormatStyle.BackColor = System.Drawing.Color.DodgerBlue;
            this.grdExScheduleList.FocusCellFormatStyle.FontBold = Janus.Windows.GridEX.TriState.True;
            this.grdExScheduleList.FocusCellFormatStyle.ForeColor = System.Drawing.Color.Gold;
            this.grdExScheduleList.FocusStyle = Janus.Windows.GridEX.FocusStyle.None;
            this.grdExScheduleList.Font = new System.Drawing.Font("���� ���", 8.25F);
            this.grdExScheduleList.FrozenColumns = 1;
            this.grdExScheduleList.GridLineColor = System.Drawing.Color.Silver;
            this.grdExScheduleList.GridLines = Janus.Windows.GridEX.GridLines.Vertical;
            this.grdExScheduleList.GridLineStyle = Janus.Windows.GridEX.GridLineStyle.Solid;
            this.grdExScheduleList.GroupByBoxVisible = false;
            this.grdExScheduleList.HeaderFormatStyle.TextAlignment = Janus.Windows.GridEX.TextAlignment.Center;
            this.grdExScheduleList.HideSelection = Janus.Windows.GridEX.HideSelection.Highlight;
            this.grdExScheduleList.Location = new System.Drawing.Point(0, 0);
            this.grdExScheduleList.Name = "grdExScheduleList";
            this.grdExScheduleList.ScrollBars = Janus.Windows.GridEX.ScrollBars.Vertical;
            this.grdExScheduleList.SelectedFormatStyle.BackColor = System.Drawing.Color.DodgerBlue;
            this.grdExScheduleList.SelectedFormatStyle.FontBold = Janus.Windows.GridEX.TriState.True;
            this.grdExScheduleList.SelectedFormatStyle.ForeColor = System.Drawing.Color.Gold;
            this.grdExScheduleList.SelectedInactiveFormatStyle.BackColor = System.Drawing.Color.Gold;
            this.grdExScheduleList.Size = new System.Drawing.Size(1008, 540);
            this.grdExScheduleList.TabIndex = 12;
            this.grdExScheduleList.TabKeyBehavior = Janus.Windows.GridEX.TabKeyBehavior.ControlNavigation;
            this.grdExScheduleList.ThemedAreas = ((Janus.Windows.GridEX.ThemedArea)(((((((((Janus.Windows.GridEX.ThemedArea.ScrollBars | Janus.Windows.GridEX.ThemedArea.EditControls) 
            | Janus.Windows.GridEX.ThemedArea.Headers) 
            | Janus.Windows.GridEX.ThemedArea.GroupByBox) 
            | Janus.Windows.GridEX.ThemedArea.GroupRows) 
            | Janus.Windows.GridEX.ThemedArea.ControlBorder) 
            | Janus.Windows.GridEX.ThemedArea.Cards) 
            | Janus.Windows.GridEX.ThemedArea.Gridlines) 
            | Janus.Windows.GridEX.ThemedArea.CheckBoxes)));
            this.grdExScheduleList.UpdateMode = Janus.Windows.GridEX.UpdateMode.CellUpdate;
            this.grdExScheduleList.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            this.grdExScheduleList.SelectionChanged += new System.EventHandler(this.OnGrdRowChanged);
            // 
            // dvSchedule
            // 
            this.dvSchedule.Table = this.schDesignateAdDs.Schedule;
            // 
            // schDesignateAdDs
            // 
            this.schDesignateAdDs.DataSetName = "SchDesignateAdDs";
            this.schDesignateAdDs.Locale = new System.Globalization.CultureInfo("en-US");
            this.schDesignateAdDs.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // uiPanelCommand
            // 
            this.uiPanelCommand.ActiveCaptionMode = Janus.Windows.UI.Dock.ActiveCaptionMode.Never;
            this.uiPanelCommand.AllowResize = Janus.Windows.UI.InheritableBoolean.True;
            this.uiPanelCommand.BackColor = System.Drawing.SystemColors.Window;
            this.uiPanelCommand.CaptionStyle = Janus.Windows.UI.Dock.PanelCaptionStyle.Dark;
            this.uiPanelCommand.CaptionVisible = Janus.Windows.UI.InheritableBoolean.False;
            this.uiPanelCommand.InnerContainer = this.uiPanelCommandContainer;
            this.uiPanelCommand.Location = new System.Drawing.Point(0, 636);
            this.uiPanelCommand.Name = "uiPanelCommand";
            this.uiPanelCommand.Size = new System.Drawing.Size(1010, 41);
            this.uiPanelCommand.TabIndex = 4;
            // 
            // uiPanelCommandContainer
            // 
            this.uiPanelCommandContainer.Controls.Add(this.pnlDetail);
            this.uiPanelCommandContainer.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.uiPanelCommandContainer.Location = new System.Drawing.Point(1, 1);
            this.uiPanelCommandContainer.Name = "uiPanelCommandContainer";
            this.uiPanelCommandContainer.Size = new System.Drawing.Size(1008, 39);
            this.uiPanelCommandContainer.TabIndex = 0;
            // 
            // pnlDetail
            // 
            this.pnlDetail.BackColor = System.Drawing.SystemColors.Window;
            this.pnlDetail.Controls.Add(this.lbMsg);
            this.pnlDetail.Controls.Add(this.btnDelete);
            this.pnlDetail.Controls.Add(this.btnAdd);
            this.pnlDetail.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlDetail.Location = new System.Drawing.Point(0, 0);
            this.pnlDetail.Name = "pnlDetail";
            this.pnlDetail.Size = new System.Drawing.Size(1008, 39);
            this.pnlDetail.TabIndex = 13;
            // 
            // lbMsg
            // 
            this.lbMsg.Location = new System.Drawing.Point(232, 8);
            this.lbMsg.Name = "lbMsg";
            this.lbMsg.Size = new System.Drawing.Size(566, 21);
            this.lbMsg.TabIndex = 43;
            this.lbMsg.Text = "��";
            this.lbMsg.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnDelete
            // 
            this.btnDelete.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnDelete.Enabled = false;
            this.btnDelete.Location = new System.Drawing.Point(120, 6);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(104, 24);
            this.btnDelete.TabIndex = 14;
            this.btnDelete.Text = "�� ��";
            this.btnDelete.ToolTipText = "���Ǿ� �ִ� ������ ��� ������ �����մϴ�";
            this.btnDelete.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnAdd
            // 
            this.btnAdd.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnAdd.Enabled = false;
            this.btnAdd.Location = new System.Drawing.Point(8, 6);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(104, 24);
            this.btnAdd.TabIndex = 13;
            this.btnAdd.Text = "�� ��";
            this.btnAdd.ToolTipText = "�������� ���� �߰� �մϴ�";
            this.btnAdd.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
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
            this.editBox1.Size = new System.Drawing.Size(0, 21);
            this.editBox1.TabIndex = 0;
            this.editBox1.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
            // 
            // SchDesignateAdControl
            // 
            this.Controls.Add(this.uiPanel);
            this.Font = global::AdManagerClient.Properties.Settings.Default.Font1;
            this.Name = "SchDesignateAdControl";
            this.Size = new System.Drawing.Size(1010, 677);
            this.Load += new System.EventHandler(this.UserControl_Load);
            ((System.ComponentModel.ISupportInitialize)(this.uiPM)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanel)).EndInit();
            this.uiPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelSearch)).EndInit();
            this.uiPanelSearch.ResumeLayout(false);
            this.uiPanelSearchContainer.ResumeLayout(false);
            this.pnlSearch.ResumeLayout(false);
            this.pnlSearchBtn.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelList)).EndInit();
            this.uiPanelList.ResumeLayout(false);
            this.uiPanelListContainer.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdExScheduleList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dvSchedule)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.schDesignateAdDs)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelCommand)).EndInit();
            this.uiPanelCommand.ResumeLayout(false);
            this.uiPanelCommandContainer.ResumeLayout(false);
            this.pnlDetail.ResumeLayout(false);
            this.ResumeLayout(false);

		}
		#endregion

		#region ��Ʈ�� �ε�
		private void UserControl_Load(object sender, System.EventArgs e)
		{
			// �����Ͱ����� ��ü����
			dt = ((DataView)grdExScheduleList.DataSource).Table;  
			cm = (CurrencyManager) this.BindingContext[grdExScheduleList.DataSource]; 

			// ��Ʈ�� �ʱ�ȭ
			InitControl();	
		}

		#endregion

		#region ��Ʈ�� �ʱ�ȭ

		private void InitControl()
		{
			ProgressStart();

			lbMsg.Text = "";

			InitCombo();

            if(menu.CanCreate(MenuCode))	canCreate = true;
            if(menu.CanDelete(MenuCode))	canDelete = true;
			if(menu.CanRead(MenuCode))		canRead = true;
			
    		InitButton();
            ProgressStop();

			if(canRead) SearchSchedule();
		}

		private void InitCombo()
		{
			Init_MediaCode();
			Init_RapCode();
			InitCombo_Level();
		}

		private void Init_MediaCode()
		{
			// ��ü�� ��ȸ�Ѵ�.
			MediaCodeModel mediacodeModel = new MediaCodeModel();
			new MediaCodeManager(systemModel, commonModel).GetMediaCodeList(mediacodeModel);
			
			if (mediacodeModel.ResultCD.Equals("0000"))
			{
				// �����ͼ¿� ����
				Utility.SetDataTable(schDesignateAdDs.Medias, mediacodeModel.MediaCodeDataSet);				
			}

			// �˻������� �޺�
			this.cbSearchMedia.Items.Clear();
			
			// �޺��ڽ��� ��Ʈ�� �ڵ����� ���� Item�迭�� ����
			Janus.Windows.EditControls.UIComboBoxItem[] comboItems = new Janus.Windows.EditControls.UIComboBoxItem[mediacodeModel.ResultCnt + 1];

			comboItems[0] = new Janus.Windows.EditControls.UIComboBoxItem("��ü����","00");
			
			for(int i=0;i<mediacodeModel.ResultCnt;i++)
			{
				DataRow row = schDesignateAdDs.Medias.Rows[i];

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
			MediaRapCodeModel mediarapcodeModel = new MediaRapCodeModel();
			new MediaRapCodeManager(systemModel, commonModel).GetMediaRapCodeList(mediarapcodeModel);
			
			if (mediarapcodeModel.ResultCD.Equals("0000"))
			{
				// �����ͼ¿� ����
				Utility.SetDataTable(schDesignateAdDs.MediaRaps, mediarapcodeModel.MediaRapCodeDataSet);				
			}

			// �˻������� �޺�
			this.cbSearchRap.Items.Clear();
			
			// �޺��ڽ��� ��Ʈ�� �ڵ����� ���� Item�迭�� ����
			Janus.Windows.EditControls.UIComboBoxItem[] comboItems = new Janus.Windows.EditControls.UIComboBoxItem[mediarapcodeModel.ResultCnt + 1];

			comboItems[0] = new Janus.Windows.EditControls.UIComboBoxItem("�̵�����","00");
			
			for(int i=0;i<mediarapcodeModel.ResultCnt;i++)
			{
				DataRow row = schDesignateAdDs.MediaRaps.Rows[i];

				string val = row["RapCode"].ToString();
				string txt = row["RapName"].ToString();
				comboItems[i+1] = new Janus.Windows.EditControls.UIComboBoxItem(txt,val);
			}
			// �޺��� ��Ʈ
			this.cbSearchRap.Items.AddRange(comboItems);
			this.cbSearchRap.SelectedIndex = 0;

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
				for(int i=0;i < schDesignateAdDs.Medias.Rows.Count;i++)
				{
					DataRow row = schDesignateAdDs.Medias.Rows[i];					
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
			if(commonModel.UserLevel=="30")
			{
				cbSearchRap.SelectedValue = commonModel.RapCode;			
				cbSearchRap.ReadOnly = true;				
			}
	

			Application.DoEvents();
		}

		private void InitButton()
		{
			if(canRead)   btnSearch.Enabled = true;
			if(canCreate) btnAdd.Enabled    = true;
			grdExScheduleList.Focus();

			Application.DoEvents();
		}


		private void DisableButton()
		{
			btnSearch.Enabled	= false;
			btnAdd.Enabled		= false;
			btnDelete.Enabled   = false;
			Application.DoEvents();
		}

		#endregion

		#region �������� �׼�ó�� �޼ҵ�

		/// <summary>
		/// �׸����� Row�����
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void OnGrdRowChanged(object sender, System.EventArgs e) 
		{
            if (!IsSearching) // 2011.11.29 JH.Park ��ȸ���� �ƴҰ�쿡�� �����ϵ��� ����
            {
                //if(IsNotLoading)
                //{
                InitButton();
                //}
            }
		}

		/// <summary>
		/// ��ȸ��ư Ŭ��
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnSearch_Click(object sender, System.EventArgs e)
		{
            ProgressStart();
			DisableButton();
			SearchSchedule();
			InitButton();
            ProgressStop();
		}

		/// <summary>
		/// �߰���ư Ŭ��
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		/// 

		private void btnAdd_Click(object sender, System.EventArgs e)
		{
			DisableButton();	

			int curRow = cm.Position;
			if(curRow >= 0)
			{					
				keyItemNo = Convert.ToInt32( dt.Rows[curRow]["ItemNo"].ToString() );
			}			

			Schedule.SchDesignatedAdForm pForm = new AdManagerClient.Schedule.SchDesignatedAdForm();
			_30_Schedule.Sch3Form pForm2 = new AdManagerClient._30_Schedule.Sch3Form();
			pForm.ItemNo = keyItemNo;
			pForm.ShowDialog(this);


			if ( pForm.DialogResult == DialogResult.OK )
			{
				keyItemNo	=	pForm.ItemNo;
				keyItemName	=	pForm.ItemName;
			}
			else
			{
				keyItemNo	=	0;
				keyItemName	=	"";
			}
			
			pForm.Dispose();
			pForm = null;

			if( keyItemNo > 0 )
			{
				pForm2.ScheduleSelected += new AdManagerClient.Schedule.SchedulePerItemInsertEventHandler(OnScheduleSelected);
				pForm2.ItemNo	=	keyItemNo;
				pForm2.ItemName	=	keyItemName;
				pForm2.DataType	=	77;	// ������
				pForm2.ShowDialog(this);
			}
			InitButton();			
		}

		/// <summary>
		/// ������ư Ŭ��
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnDelete_Click(object sender, System.EventArgs e)
		{
			DeleteSchedule();
			InitButton();
		}
		#endregion

		#region ó���޼ҵ�


		/// <summary>
		/// ������ ����Ʈ�� �����´�
		/// </summary>
		private void SearchSchedule()
		{

			ProgressStart();

            IsSearching = true;

			try
			{
				#region [ ������ ��ȸ ]
				// ��� üũ�ڽ��� üũ�� Ǭ��.
				grdExScheduleList.UnCheckAllRecords(); 

				// �����͸𵨿� ������ ������ ��Ʈ�Ѵ�.
				mainModel.Init();
				mainModel.Media		=	int.Parse(cbSearchMedia.SelectedItem.Value.ToString());
				mainModel.MediaRep	=	int.Parse(cbSearchRap.SelectedItem.Value.ToString());
				mainModel.AdState10	=	true;
				mainModel.AdState20	=	chkAdState_20.Checked;
				mainModel.AdState30	=	chkAdState_30.Checked;
				mainModel.AdState40	=	chkAdState_40.Checked;

				new SchDesignatedAdManager(systemModel, commonModel).GetDataList( mainModel );

				schDesignateAdDs.Schedule.Clear();
				if( mainModel.ResultCD.Equals("0000") )
				{
					keyMedia	= mainModel.Media;

					Utility.SetDataTable( schDesignateAdDs.Schedule, mainModel.DsSchedule );
					StatusMessage(mainModel.ResultCnt + "���� ������ ��ȸ�Ǿ����ϴ�.");

					if(canDelete && schDesignateAdDs.Schedule.Rows.Count > 0 )	btnDelete.Enabled = true;
					else														btnDelete.Enabled = false;
				}
				#endregion

				#region [ ���۾� ���� ]
				keyAckNoCm		= "";
				keyAckStateCm	= "";
				lbMsg.Text		= "";

				schPublishModel.Init();
				schPublishModel.SearchMediaCode		 =  cbSearchMedia.SelectedItem.Value.ToString(); 

				// ���� ���λ�����ȸ ���񽺸� ȣ���Ѵ�.
				new SchPublishManager(systemModel,commonModel).GetPublishState(schPublishModel,10);
                if (schPublishModel.ResultCD.Equals("0000"))
                {
                    keyAckNoCm		= schPublishModel.AckNo;
                    keyAckStateCm	= schPublishModel.State;

                    if(keyAckNoCm.Equals("10"))	// ���λ��°� 10:�����̸�
                    {
                        lbMsg.Text = "�� �۾����Դϴ�.";
                    }
                    else if(keyAckNoCm.Equals("20")) // ���λ��°� 20:������ �����̸� ���� �Ұ��ϴ�.
                    {
                        lbMsg.Text = "�˼����� ������Դϴ�.";
                        canCreate = false;
                        canDelete = false;
                    }
                    else if(keyAckNoCm.Equals("25")) // ���λ��°� 25:���������� �����̸� ���� �Ұ��ϴ�.
                    {
                        lbMsg.Text = "�������� ������Դϴ�.";
                        canCreate = false;
                        canDelete = false;
                    }
                    else if(keyAckNoCm.Equals("30")) // ���λ��°� 30:�������� �����̸� �ű����� �����ϴ�.
                    {
                        lbMsg.Text = "";
                    }
                }
				#endregion
			}
			catch(FrameException fe)
			{
				FrameSystem.showMsgForm("�������� ����Ȳ ��ȸ����", new string[] {fe.ErrCode, fe.ResultMsg});
			}
			catch(Exception ex)
			{
				FrameSystem.showMsgForm("�������� ����Ȳ ��ȸ����",new string[] {"",ex.Message});
			}
			finally
			{
                IsSearching = false; // ��ȸ�� Flag ����
				ProgressStop();
			}
		}

		/// <summary>
		/// �������� ������ ����
		/// </summary>
		private void DeleteSchedule()
		{
            if (grdExScheduleList.GetCheckedRows().Length > 0)
            {
                StatusMessage("�������� �������� �����մϴ�.");
                DialogResult result = MessageBox.Show("�ش� ���� ������������ ���� �Ͻðڽ��ϱ�?", "���������� ����", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
                if (result == DialogResult.No) return;
            }
            else
            {
                MessageBox.Show("���õ� �������� �����ϴ�.", "���������� ����", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }


			DisableButton();
            grdExScheduleList.UpdateData();
            
			try
			{
                int deleteRows = 0;
                int totalRows = 0;
                int currentRow = cm.Position;

                totalRows = grdExScheduleList.GetCheckedRows().Length;

                foreach (GridEXRow gr in grdExScheduleList.GetCheckedRows())
                {
                    mainModel.Init();
                    mainModel.Media     = keyMedia;
                    mainModel.ItemNo = Convert.ToInt32(gr.Cells["ItemNo"].Value.ToString());
                    mainModel.Category = Convert.ToInt32(gr.Cells["Category"].Value.ToString());
                    mainModel.Genre = Convert.ToInt32(gr.Cells["Genre"].Value.ToString());
                    mainModel.Channel = Convert.ToInt32(gr.Cells["Channel"].Value.ToString());
                    mainModel.Series = Convert.ToInt32(gr.Cells["Series"].Value.ToString());

                    new SchDesignatedAdManager(systemModel, commonModel).DeleteRow(mainModel);

                    if (mainModel.ResultCD.Equals("0000"))
                    {
                        deleteRows += 1;
                    }
                }

				SearchSchedule();

                if (currentRow > cm.Count) cm.Position = cm.Count;
				else					cm.Position = currentRow;
			}
			catch(FrameException fe)
			{
				FrameSystem.showMsgForm("�������� ��������������", new string[] {fe.ErrCode, fe.ResultMsg});
			}
			catch(Exception ex)
			{
				FrameSystem.showMsgForm("�������� ������ ��������",new string[] {"",ex.Message});
			}
		}
		#endregion

		#region [����] �ý��������� �޴����� ����

		// �ý��� ���� : ȭ�����
		private SystemModel   systemModel   = FrameSystem.oSysModel;
		private CommonModel   commonModel   = FrameSystem.oComModel;
		private Logger        log           = FrameSystem.oLog;
		private MenuPower     menu          = FrameSystem.oMenu;

		private	bool	canRead		= false;
		private	bool	canCreate   = false;
		private	bool	canDelete	= false;

		#endregion

		#region [����] �̺�Ʈ �ڵ鷯�� �Լ�

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

		/// <summary>
		/// ����� �����쿡�� ���� Ŭ�������� ó���ϴ� �̺�Ʈ ó����
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void OnScheduleSelected(object sender, AdManagerClient.Schedule.SchedulePerItemEventArgs e)
		{
			StatusMessage("���õ� ������ ������ �߰��մϴ�.");

			try
			{
				for( int i = 0; i< e.ScheduleData.Count ; i++)
				{
					mainModel.Init();
					mainModel.ItemNo	=	keyItemNo;
					mainModel.Media		=	keyMedia;
					mainModel.Category	=	((SchedulePerItemModel)e.ScheduleData[i]).Category;
					mainModel.Genre		=	((SchedulePerItemModel)e.ScheduleData[i]).Genre;
					mainModel.Channel	=	((SchedulePerItemModel)e.ScheduleData[i]).Channel;
					mainModel.Series	=	((SchedulePerItemModel)e.ScheduleData[i]).Series;
			
					new SchDesignatedAdManager( systemModel, commonModel).InsertRow( mainModel );
					if( !mainModel.ResultCD.Equals("0000") )
					{
						throw new Exception(mainModel.ResultDesc);
					}
					Application.DoEvents();
				}

                SearchSchedule();
				StatusMessage("���õ� �������� �߰��Ǿ����ϴ�...");
			}
			catch(FrameException fe)
			{
				FrameSystem.showMsgForm("������ ��������", new string[] {fe.ErrCode, fe.ResultMsg});
			}
			catch(Exception ex)
			{
				FrameSystem.showMsgForm("������ ��������",new string[] {"",ex.Message});
			}			
		}
	}
} 
