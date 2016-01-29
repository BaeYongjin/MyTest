// ===============================================================================
// ChannelControl for Charites Project
//
// ChannelControl.cs
//
// ä���������� ������� �����մϴ�. 
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
using System.Windows.Forms;
using System.Diagnostics;

using WinFramework.Misc;
using WinFramework.Base;
using WinFramework.Data;

using AdManagerModel;
namespace AdManagerClient
{
	/// <summary>
	/// ä�ΰ��� ��Ʈ��
	/// </summary>
	public class ChannelControl : System.Windows.Forms.UserControl, IUserControl
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
		ChannelModel channelModel  = new ChannelModel();	// ä��������

		// ȭ��ó���� ����
		bool IsNewSearchKey		  = true;					// �˻����Է� ����
		CurrencyManager cm        = null;					// ������ �׸����� ���濡 ���� �����ͼ� ������ ���Ͽ�			
        CurrencyManager cmChild = null;

		DataTable       dt        = null;
		DataTable       dtChild        = null;

        bool IsSearching = false; // ��ȸ�� ��ȭ���� ������Ʈ �Ǵ� ���� ���� �ϱ����� 2011.11.29 JH.Park
		bool IsAdding             = true;
		bool canRead			  = false;
		bool canUpdate			  = false;
		bool canCreate            = false;
		bool canDelete            = false;

		// Key ������
		bool IsNotLoading		  = true;					// ����ȸ���� �ƴ�
        string keyGenreCode = "";
        string keyServiceId = ""; // ä�λ� row ���ε��� Ű
        private Janus.Windows.UI.Dock.UIPanelManager uiPM;
        
        private Janus.Windows.UI.Dock.UIPanelGroup uiPanel6;
        private Janus.Windows.UI.Dock.UIPanel uiPanel7;
        private Janus.Windows.UI.Dock.UIPanelInnerContainer uiPanel7Container;
        private Janus.Windows.UI.Dock.UIPanelGroup uiPanel8;
        private Janus.Windows.UI.Dock.UIPanel uiPanel9;
        private Janus.Windows.UI.Dock.UIPanelInnerContainer uiPanel9Container;
        private Janus.Windows.UI.Dock.UIPanel uiPanel10;
        private Janus.Windows.UI.Dock.UIPanelInnerContainer uiPanel10Container;
        private Janus.Windows.UI.Dock.UIPanel uiPanel11;
        private Janus.Windows.UI.Dock.UIPanelInnerContainer uiPanel11Container;
        private Panel pnlSearch;
        private Janus.Windows.EditControls.UICheckBox chkSearchYn;
        private Janus.Windows.EditControls.UIButton btnSearch;
        private Panel panel1;
        private Label label9;
        private Label label6;
        private Label label7;
        private Janus.Windows.GridEX.EditControls.IntegerUpDown ud_cAdNRate;
        private Label label2;
        private Janus.Windows.GridEX.EditControls.IntegerUpDown ud_cAdRate;
        private Janus.Windows.GridEX.EditControls.EditBox ebChannelNum;
        private Janus.Windows.GridEX.EditControls.EditBox ebServiceId;
        private Janus.Windows.EditControls.UICheckBox chkUseYn;
        private Janus.Windows.EditControls.UICheckBox chkAdYn;
        private Label lblChannelNum;
        private Janus.Windows.EditControls.UIButton btnSave;
        private Label lblServiceId;
        private Janus.Windows.GridEX.GridEX grdGenre;
        private Janus.Windows.GridEX.GridEX grdChannel;
        private Janus.Windows.GridEX.EditControls.EditBox ebChannelNm;
        private Label lblChannelNm;
        private Label label1;
        private Janus.Windows.GridEX.EditControls.EditBox ebChannelRank;
        private ChannelDs channelDs1;
        private DataView dvStmCode;
        private DataView dvChannel;
		string keyChannelNo       = "";

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

        private Janus.Windows.EditControls.UIButton uiButton2;
        private Janus.Windows.EditControls.UIButton uiButton1;
        private System.Windows.Forms.Panel pnlUserDetail;
		private System.ComponentModel.IContainer components;

		public ChannelControl()
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
            Janus.Windows.GridEX.GridEXLayout grdGenre_DesignTimeLayout = new Janus.Windows.GridEX.GridEXLayout();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ChannelControl));
            Janus.Windows.GridEX.GridEXLayout grdChannel_DesignTimeLayout = new Janus.Windows.GridEX.GridEXLayout();
            this.pnlUserDetail = new System.Windows.Forms.Panel();
            this.uiButton1 = new Janus.Windows.EditControls.UIButton();
            this.uiButton2 = new Janus.Windows.EditControls.UIButton();
            this.uiPM = new Janus.Windows.UI.Dock.UIPanelManager(this.components);
            this.uiPanel6 = new Janus.Windows.UI.Dock.UIPanelGroup();
            this.uiPanel7 = new Janus.Windows.UI.Dock.UIPanel();
            this.uiPanel7Container = new Janus.Windows.UI.Dock.UIPanelInnerContainer();
            this.pnlSearch = new System.Windows.Forms.Panel();
            this.chkSearchYn = new Janus.Windows.EditControls.UICheckBox();
            this.btnSearch = new Janus.Windows.EditControls.UIButton();
            this.uiPanel8 = new Janus.Windows.UI.Dock.UIPanelGroup();
            this.uiPanel10 = new Janus.Windows.UI.Dock.UIPanel();
            this.uiPanel10Container = new Janus.Windows.UI.Dock.UIPanelInnerContainer();
            this.grdGenre = new Janus.Windows.GridEX.GridEX();
            this.dvStmCode = new System.Data.DataView();
            this.channelDs1 = new AdManagerClient.ChannelDs();
            this.uiPanel11 = new Janus.Windows.UI.Dock.UIPanel();
            this.uiPanel11Container = new Janus.Windows.UI.Dock.UIPanelInnerContainer();
            this.grdChannel = new Janus.Windows.GridEX.GridEX();
            this.dvChannel = new System.Data.DataView();
            this.uiPanel9 = new Janus.Windows.UI.Dock.UIPanel();
            this.uiPanel9Container = new Janus.Windows.UI.Dock.UIPanelInnerContainer();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.ebChannelRank = new Janus.Windows.GridEX.EditControls.EditBox();
            this.lblChannelNm = new System.Windows.Forms.Label();
            this.ebChannelNm = new Janus.Windows.GridEX.EditControls.EditBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.ud_cAdNRate = new Janus.Windows.GridEX.EditControls.IntegerUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.ud_cAdRate = new Janus.Windows.GridEX.EditControls.IntegerUpDown();
            this.ebChannelNum = new Janus.Windows.GridEX.EditControls.EditBox();
            this.ebServiceId = new Janus.Windows.GridEX.EditControls.EditBox();
            this.chkUseYn = new Janus.Windows.EditControls.UICheckBox();
            this.chkAdYn = new Janus.Windows.EditControls.UICheckBox();
            this.lblChannelNum = new System.Windows.Forms.Label();
            this.btnSave = new Janus.Windows.EditControls.UIButton();
            this.lblServiceId = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.uiPM)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanel6)).BeginInit();
            this.uiPanel6.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanel7)).BeginInit();
            this.uiPanel7.SuspendLayout();
            this.uiPanel7Container.SuspendLayout();
            this.pnlSearch.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanel8)).BeginInit();
            this.uiPanel8.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanel10)).BeginInit();
            this.uiPanel10.SuspendLayout();
            this.uiPanel10Container.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdGenre)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dvStmCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.channelDs1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanel11)).BeginInit();
            this.uiPanel11.SuspendLayout();
            this.uiPanel11Container.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdChannel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dvChannel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanel9)).BeginInit();
            this.uiPanel9.SuspendLayout();
            this.uiPanel9Container.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlUserDetail
            // 
            this.pnlUserDetail.BackColor = System.Drawing.SystemColors.Window;
            this.pnlUserDetail.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlUserDetail.Location = new System.Drawing.Point(0, 0);
            this.pnlUserDetail.Name = "pnlUserDetail";
            this.pnlUserDetail.Size = new System.Drawing.Size(849, 205);
            this.pnlUserDetail.TabIndex = 3;
            // 
            // uiButton1
            // 
            this.uiButton1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.uiButton1.Location = new System.Drawing.Point(136, 8);
            this.uiButton1.Name = "uiButton1";
            this.uiButton1.Size = new System.Drawing.Size(112, 24);
            this.uiButton1.TabIndex = 5;
            this.uiButton1.Text = "�� ��";
            this.uiButton1.VisualStyle = Janus.Windows.UI.VisualStyle.Office2003;
            // 
            // uiButton2
            // 
            this.uiButton2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.uiButton2.Location = new System.Drawing.Point(8, 8);
            this.uiButton2.Name = "uiButton2";
            this.uiButton2.Size = new System.Drawing.Size(120, 24);
            this.uiButton2.TabIndex = 6;
            this.uiButton2.Text = "�� ��";
            this.uiButton2.VisualStyle = Janus.Windows.UI.VisualStyle.Office2003;
            // 
            // uiPM
            // 
            this.uiPM.BackColorGradientAutoHideStrip = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(218)))), ((int)(((byte)(250)))));
            this.uiPM.ContainerControl = this;
            this.uiPM.VisualStyle = Janus.Windows.UI.Dock.PanelVisualStyle.Office2007;
            this.uiPanel6.Id = new System.Guid("56251b73-d925-4257-8a28-6338273a7fd4");
            this.uiPanel6.StaticGroup = true;
            this.uiPanel7.Id = new System.Guid("c8adb4fd-b8d9-4294-acee-b3e858105ad9");
            this.uiPanel6.Panels.Add(this.uiPanel7);
            this.uiPanel8.Id = new System.Guid("2e246c1b-b409-48c0-b250-26e0ef96c593");
            this.uiPanel8.StaticGroup = true;
            this.uiPanel10.Id = new System.Guid("b078c821-d135-4daa-8f4f-01b7f542b35e");
            this.uiPanel8.Panels.Add(this.uiPanel10);
            this.uiPanel11.Id = new System.Guid("e5dad027-96d3-4471-bd27-f3cb6389df38");
            this.uiPanel8.Panels.Add(this.uiPanel11);
            this.uiPanel6.Panels.Add(this.uiPanel8);
            this.uiPanel9.Id = new System.Guid("4602dd0f-1e3c-4d56-8155-dd5b6f6045a2");
            this.uiPanel6.Panels.Add(this.uiPanel9);
            this.uiPM.Panels.Add(this.uiPanel6);
            // 
            // Design Time Panel Info:
            // 
            this.uiPM.BeginPanelInfo();
            this.uiPM.AddDockPanelInfo(new System.Guid("56251b73-d925-4257-8a28-6338273a7fd4"), Janus.Windows.UI.Dock.PanelGroupStyle.HorizontalTiles, Janus.Windows.UI.Dock.PanelDockStyle.Fill, true, new System.Drawing.Size(1004, 671), true);
            this.uiPM.AddDockPanelInfo(new System.Guid("c8adb4fd-b8d9-4294-acee-b3e858105ad9"), new System.Guid("56251b73-d925-4257-8a28-6338273a7fd4"), 74, true);
            this.uiPM.AddDockPanelInfo(new System.Guid("2e246c1b-b409-48c0-b250-26e0ef96c593"), new System.Guid("56251b73-d925-4257-8a28-6338273a7fd4"), Janus.Windows.UI.Dock.PanelGroupStyle.VerticalTiles, true, 431, true);
            this.uiPM.AddDockPanelInfo(new System.Guid("b078c821-d135-4daa-8f4f-01b7f542b35e"), new System.Guid("2e246c1b-b409-48c0-b250-26e0ef96c593"), 396, true);
            this.uiPM.AddDockPanelInfo(new System.Guid("e5dad027-96d3-4471-bd27-f3cb6389df38"), new System.Guid("2e246c1b-b409-48c0-b250-26e0ef96c593"), 604, true);
            this.uiPM.AddDockPanelInfo(new System.Guid("4602dd0f-1e3c-4d56-8155-dd5b6f6045a2"), new System.Guid("56251b73-d925-4257-8a28-6338273a7fd4"), 136, true);
            this.uiPM.AddFloatingPanelInfo(new System.Guid("22904c6a-5df3-4566-9005-5ec5533171d7"), Janus.Windows.UI.Dock.PanelGroupStyle.HorizontalTiles, true, new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
            this.uiPM.AddFloatingPanelInfo(new System.Guid("a5e5700a-e28f-48aa-aa11-f2a33480bcad"), new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
            this.uiPM.AddFloatingPanelInfo(new System.Guid("4a82456d-5f7a-482b-8bdc-efc376534623"), Janus.Windows.UI.Dock.PanelGroupStyle.VerticalTiles, true, new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
            this.uiPM.AddFloatingPanelInfo(new System.Guid("8d839203-5368-4c04-9f26-9915a0db75ea"), new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
            this.uiPM.AddFloatingPanelInfo(new System.Guid("8dcb99bc-e685-4cc2-91f9-f0f8df127cf8"), new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
            this.uiPM.AddFloatingPanelInfo(new System.Guid("0ceaa1b4-6590-40d5-a32c-fa99e73cc9ab"), new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
            this.uiPM.AddFloatingPanelInfo(new System.Guid("56251b73-d925-4257-8a28-6338273a7fd4"), Janus.Windows.UI.Dock.PanelGroupStyle.HorizontalTiles, true, new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
            this.uiPM.AddFloatingPanelInfo(new System.Guid("c8adb4fd-b8d9-4294-acee-b3e858105ad9"), new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
            this.uiPM.AddFloatingPanelInfo(new System.Guid("2e246c1b-b409-48c0-b250-26e0ef96c593"), Janus.Windows.UI.Dock.PanelGroupStyle.VerticalTiles, true, new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
            this.uiPM.AddFloatingPanelInfo(new System.Guid("b078c821-d135-4daa-8f4f-01b7f542b35e"), new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
            this.uiPM.AddFloatingPanelInfo(new System.Guid("e5dad027-96d3-4471-bd27-f3cb6389df38"), new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
            this.uiPM.AddFloatingPanelInfo(new System.Guid("4602dd0f-1e3c-4d56-8155-dd5b6f6045a2"), new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
            this.uiPM.EndPanelInfo();
            // 
            // uiPanel6
            // 
            this.uiPanel6.CaptionVisible = Janus.Windows.UI.InheritableBoolean.False;
            this.uiPanel6.Location = new System.Drawing.Point(3, 3);
            this.uiPanel6.Name = "uiPanel6";
            this.uiPanel6.Size = new System.Drawing.Size(1004, 671);
            this.uiPanel6.TabIndex = 4;
            this.uiPanel6.Text = "Panel 6";
            // 
            // uiPanel7
            // 
            this.uiPanel7.InnerContainer = this.uiPanel7Container;
            this.uiPanel7.Location = new System.Drawing.Point(0, 0);
            this.uiPanel7.Name = "uiPanel7";
            this.uiPanel7.Size = new System.Drawing.Size(1004, 77);
            this.uiPanel7.TabIndex = 4;
            this.uiPanel7.Text = "�ǽð�ä������ ����";
            // 
            // uiPanel7Container
            // 
            this.uiPanel7Container.Controls.Add(this.pnlSearch);
            this.uiPanel7Container.Location = new System.Drawing.Point(1, 23);
            this.uiPanel7Container.Name = "uiPanel7Container";
            this.uiPanel7Container.Size = new System.Drawing.Size(1002, 53);
            this.uiPanel7Container.TabIndex = 0;
            // 
            // pnlSearch
            // 
            this.pnlSearch.BackColor = System.Drawing.SystemColors.Window;
            this.pnlSearch.Controls.Add(this.chkSearchYn);
            this.pnlSearch.Controls.Add(this.btnSearch);
            this.pnlSearch.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlSearch.Location = new System.Drawing.Point(0, 0);
            this.pnlSearch.Name = "pnlSearch";
            this.pnlSearch.Size = new System.Drawing.Size(1002, 53);
            this.pnlSearch.TabIndex = 5;
            // 
            // chkSearchYn
            // 
            this.chkSearchYn.Location = new System.Drawing.Point(761, 6);
            this.chkSearchYn.Name = "chkSearchYn";
            this.chkSearchYn.ShowFocusRectangle = false;
            this.chkSearchYn.Size = new System.Drawing.Size(109, 26);
            this.chkSearchYn.TabIndex = 46;
            this.chkSearchYn.Text = "ä�� ��뿩��";
            this.chkSearchYn.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            // 
            // btnSearch
            // 
            this.btnSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSearch.Enabled = false;
            this.btnSearch.Location = new System.Drawing.Point(887, 6);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(104, 24);
            this.btnSearch.TabIndex = 3;
            this.btnSearch.Text = "�� ȸ";
            this.btnSearch.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // uiPanel8
            // 
            this.uiPanel8.CaptionVisible = Janus.Windows.UI.InheritableBoolean.False;
            this.uiPanel8.GroupStyle = Janus.Windows.UI.Dock.PanelGroupStyle.VerticalTiles;
            this.uiPanel8.Location = new System.Drawing.Point(0, 81);
            this.uiPanel8.Name = "uiPanel8";
            this.uiPanel8.Size = new System.Drawing.Size(1004, 446);
            this.uiPanel8.TabIndex = 4;
            this.uiPanel8.Text = "Panel 8";
            // 
            // uiPanel10
            // 
            this.uiPanel10.InnerContainer = this.uiPanel10Container;
            this.uiPanel10.Location = new System.Drawing.Point(0, 0);
            this.uiPanel10.Name = "uiPanel10";
            this.uiPanel10.Size = new System.Drawing.Size(396, 446);
            this.uiPanel10.TabIndex = 4;
            this.uiPanel10.Text = "ä���帣";
            // 
            // uiPanel10Container
            // 
            this.uiPanel10Container.Controls.Add(this.grdGenre);
            this.uiPanel10Container.Location = new System.Drawing.Point(1, 23);
            this.uiPanel10Container.Name = "uiPanel10Container";
            this.uiPanel10Container.Size = new System.Drawing.Size(394, 422);
            this.uiPanel10Container.TabIndex = 0;
            // 
            // grdGenre
            // 
            this.grdGenre.AllowEdit = Janus.Windows.GridEX.InheritableBoolean.False;
            this.grdGenre.AlternatingColors = true;
            this.grdGenre.AlternatingRowFormatStyle.BackgroundImageDrawMode = Janus.Windows.GridEX.BackgroundImageDrawMode.Tile;
            this.grdGenre.BorderStyle = Janus.Windows.GridEX.BorderStyle.None;
            this.grdGenre.DataSource = this.dvStmCode;
            grdGenre_DesignTimeLayout.LayoutString = resources.GetString("grdGenre_DesignTimeLayout.LayoutString");
            this.grdGenre.DesignTimeLayout = grdGenre_DesignTimeLayout;
            this.grdGenre.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdGenre.EmptyRows = true;
            this.grdGenre.FocusCellFormatStyle.BackColor = System.Drawing.SystemColors.Highlight;
            this.grdGenre.FocusCellFormatStyle.ForeColor = System.Drawing.Color.White;
            this.grdGenre.FocusStyle = Janus.Windows.GridEX.FocusStyle.None;
            this.grdGenre.Font = new System.Drawing.Font("�������", 8.5F);
            this.grdGenre.FrozenColumns = 2;
            this.grdGenre.GridLineColor = System.Drawing.Color.Silver;
            this.grdGenre.GridLines = Janus.Windows.GridEX.GridLines.Vertical;
            this.grdGenre.GridLineStyle = Janus.Windows.GridEX.GridLineStyle.Solid;
            this.grdGenre.GroupByBoxVisible = false;
            this.grdGenre.HeaderFormatStyle.TextAlignment = Janus.Windows.GridEX.TextAlignment.Center;
            this.grdGenre.HideSelection = Janus.Windows.GridEX.HideSelection.HighlightInactive;
            this.grdGenre.ImeMode = System.Windows.Forms.ImeMode.AlphaFull;
            this.grdGenre.Location = new System.Drawing.Point(0, 0);
            this.grdGenre.Name = "grdGenre";
            this.grdGenre.ScrollBars = Janus.Windows.GridEX.ScrollBars.Vertical;
            this.grdGenre.SelectedInactiveFormatStyle.BackColor = System.Drawing.Color.Gold;
            this.grdGenre.Size = new System.Drawing.Size(394, 422);
            this.grdGenre.TabIndex = 7;
            this.grdGenre.TabKeyBehavior = Janus.Windows.GridEX.TabKeyBehavior.ControlNavigation;
            this.grdGenre.ThemedAreas = ((Janus.Windows.GridEX.ThemedArea)(((((((((Janus.Windows.GridEX.ThemedArea.ScrollBars | Janus.Windows.GridEX.ThemedArea.EditControls) 
            | Janus.Windows.GridEX.ThemedArea.Headers) 
            | Janus.Windows.GridEX.ThemedArea.GroupByBox) 
            | Janus.Windows.GridEX.ThemedArea.GroupRows) 
            | Janus.Windows.GridEX.ThemedArea.ControlBorder) 
            | Janus.Windows.GridEX.ThemedArea.Cards) 
            | Janus.Windows.GridEX.ThemedArea.Gridlines) 
            | Janus.Windows.GridEX.ThemedArea.CheckBoxes)));
            this.grdGenre.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            this.grdGenre.Click += new System.EventHandler(this.grdGenre_Click);
            // 
            // dvStmCode
            // 
            this.dvStmCode.Table = this.channelDs1.StmCode;
            // 
            // channelDs1
            // 
            this.channelDs1.DataSetName = "ChannelDs";
            this.channelDs1.Locale = new System.Globalization.CultureInfo("en-US");
            this.channelDs1.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // uiPanel11
            // 
            this.uiPanel11.InnerContainer = this.uiPanel11Container;
            this.uiPanel11.Location = new System.Drawing.Point(400, 0);
            this.uiPanel11.Name = "uiPanel11";
            this.uiPanel11.Size = new System.Drawing.Size(604, 446);
            this.uiPanel11.TabIndex = 4;
            this.uiPanel11.Text = "�ǽð�ä��";
            // 
            // uiPanel11Container
            // 
            this.uiPanel11Container.Controls.Add(this.grdChannel);
            this.uiPanel11Container.Location = new System.Drawing.Point(1, 23);
            this.uiPanel11Container.Name = "uiPanel11Container";
            this.uiPanel11Container.Size = new System.Drawing.Size(602, 422);
            this.uiPanel11Container.TabIndex = 0;
            // 
            // grdChannel
            // 
            this.grdChannel.AllowEdit = Janus.Windows.GridEX.InheritableBoolean.False;
            this.grdChannel.AlternatingColors = true;
            this.grdChannel.AlternatingRowFormatStyle.BackgroundImageDrawMode = Janus.Windows.GridEX.BackgroundImageDrawMode.Tile;
            this.grdChannel.BorderStyle = Janus.Windows.GridEX.BorderStyle.None;
            this.grdChannel.DataSource = this.dvChannel;
            grdChannel_DesignTimeLayout.LayoutString = resources.GetString("grdChannel_DesignTimeLayout.LayoutString");
            this.grdChannel.DesignTimeLayout = grdChannel_DesignTimeLayout;
            this.grdChannel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdChannel.EmptyRows = true;
            this.grdChannel.FocusCellFormatStyle.BackColor = System.Drawing.SystemColors.Highlight;
            this.grdChannel.FocusCellFormatStyle.ForeColor = System.Drawing.Color.White;
            this.grdChannel.FocusStyle = Janus.Windows.GridEX.FocusStyle.None;
            this.grdChannel.Font = new System.Drawing.Font("�������", 8.5F);
            this.grdChannel.FrozenColumns = 2;
            this.grdChannel.GridLineColor = System.Drawing.Color.Silver;
            this.grdChannel.GridLines = Janus.Windows.GridEX.GridLines.Vertical;
            this.grdChannel.GridLineStyle = Janus.Windows.GridEX.GridLineStyle.Solid;
            this.grdChannel.GroupByBoxVisible = false;
            this.grdChannel.HeaderFormatStyle.TextAlignment = Janus.Windows.GridEX.TextAlignment.Center;
            this.grdChannel.HideSelection = Janus.Windows.GridEX.HideSelection.HighlightInactive;
            this.grdChannel.Location = new System.Drawing.Point(0, 0);
            this.grdChannel.Name = "grdChannel";
            this.grdChannel.ScrollBars = Janus.Windows.GridEX.ScrollBars.Vertical;
            this.grdChannel.SelectedInactiveFormatStyle.BackColor = System.Drawing.Color.Gold;
            this.grdChannel.Size = new System.Drawing.Size(602, 422);
            this.grdChannel.TabIndex = 7;
            this.grdChannel.TabKeyBehavior = Janus.Windows.GridEX.TabKeyBehavior.ControlNavigation;
            this.grdChannel.ThemedAreas = ((Janus.Windows.GridEX.ThemedArea)(((((((((Janus.Windows.GridEX.ThemedArea.ScrollBars | Janus.Windows.GridEX.ThemedArea.EditControls) 
            | Janus.Windows.GridEX.ThemedArea.Headers) 
            | Janus.Windows.GridEX.ThemedArea.GroupByBox) 
            | Janus.Windows.GridEX.ThemedArea.GroupRows) 
            | Janus.Windows.GridEX.ThemedArea.ControlBorder) 
            | Janus.Windows.GridEX.ThemedArea.Cards) 
            | Janus.Windows.GridEX.ThemedArea.Gridlines) 
            | Janus.Windows.GridEX.ThemedArea.CheckBoxes)));
            this.grdChannel.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            this.grdChannel.Click += new System.EventHandler(this.grdChannel_Click);
            // 
            // dvChannel
            // 
            this.dvChannel.Table = this.channelDs1.ChannelCode;
            // 
            // uiPanel9
            // 
            this.uiPanel9.InnerContainer = this.uiPanel9Container;
            this.uiPanel9.Location = new System.Drawing.Point(0, 531);
            this.uiPanel9.Name = "uiPanel9";
            this.uiPanel9.Size = new System.Drawing.Size(1004, 140);
            this.uiPanel9.TabIndex = 4;
            this.uiPanel9.Text = "�ǽð�ä�� ��";
            // 
            // uiPanel9Container
            // 
            this.uiPanel9Container.Controls.Add(this.panel1);
            this.uiPanel9Container.Location = new System.Drawing.Point(1, 23);
            this.uiPanel9Container.Name = "uiPanel9Container";
            this.uiPanel9Container.Size = new System.Drawing.Size(1002, 116);
            this.uiPanel9Container.TabIndex = 0;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.Window;
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.ebChannelRank);
            this.panel1.Controls.Add(this.lblChannelNm);
            this.panel1.Controls.Add(this.ebChannelNm);
            this.panel1.Controls.Add(this.label9);
            this.panel1.Controls.Add(this.label6);
            this.panel1.Controls.Add(this.label7);
            this.panel1.Controls.Add(this.ud_cAdNRate);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.ud_cAdRate);
            this.panel1.Controls.Add(this.ebChannelNum);
            this.panel1.Controls.Add(this.ebServiceId);
            this.panel1.Controls.Add(this.chkUseYn);
            this.panel1.Controls.Add(this.chkAdYn);
            this.panel1.Controls.Add(this.lblChannelNum);
            this.panel1.Controls.Add(this.btnSave);
            this.panel1.Controls.Add(this.lblServiceId);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1002, 116);
            this.panel1.TabIndex = 5;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(194, 21);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(60, 21);
            this.label1.TabIndex = 273;
            this.label1.Text = "ä�ε��";
            // 
            // ebChannelRank
            // 
            this.ebChannelRank.Location = new System.Drawing.Point(257, 16);
            this.ebChannelRank.MaxLength = 40;
            this.ebChannelRank.Name = "ebChannelRank";
            this.ebChannelRank.ReadOnly = true;
            this.ebChannelRank.Size = new System.Drawing.Size(51, 21);
            this.ebChannelRank.TabIndex = 272;
            this.ebChannelRank.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
            this.ebChannelRank.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            // 
            // lblChannelNm
            // 
            this.lblChannelNm.Location = new System.Drawing.Point(194, 45);
            this.lblChannelNm.Name = "lblChannelNm";
            this.lblChannelNm.Size = new System.Drawing.Size(57, 21);
            this.lblChannelNm.TabIndex = 271;
            this.lblChannelNm.Text = "ä�θ�";
            // 
            // ebChannelNm
            // 
            this.ebChannelNm.Location = new System.Drawing.Point(257, 43);
            this.ebChannelNm.MaxLength = 40;
            this.ebChannelNm.Name = "ebChannelNm";
            this.ebChannelNm.ReadOnly = true;
            this.ebChannelNm.Size = new System.Drawing.Size(197, 21);
            this.ebChannelNm.TabIndex = 270;
            this.ebChannelNm.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
            this.ebChannelNm.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            // 
            // label9
            // 
            this.label9.Location = new System.Drawing.Point(868, 21);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(53, 21);
            this.label9.TabIndex = 269;
            this.label9.Text = "ä�λ��";
            // 
            // label6
            // 
            this.label6.Location = new System.Drawing.Point(482, 21);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(60, 21);
            this.label6.TabIndex = 268;
            this.label6.Text = "��������";
            // 
            // label7
            // 
            this.label7.Location = new System.Drawing.Point(690, 21);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(133, 21);
            this.label7.TabIndex = 263;
            this.label7.Text = "ADNetwork �������";
            // 
            // ud_cAdNRate
            // 
            this.ud_cAdNRate.Location = new System.Drawing.Point(719, 43);
            this.ud_cAdNRate.MaxLength = 3;
            this.ud_cAdNRate.Name = "ud_cAdNRate";
            this.ud_cAdNRate.Size = new System.Drawing.Size(40, 21);
            this.ud_cAdNRate.TabIndex = 262;
            this.ud_cAdNRate.TextAlignment = Janus.Windows.GridEX.TextAlignment.Far;
            this.ud_cAdNRate.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(572, 21);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(87, 21);
            this.label2.TabIndex = 259;
            this.label2.Text = "�����������";
            // 
            // ud_cAdRate
            // 
            this.ud_cAdRate.Location = new System.Drawing.Point(590, 43);
            this.ud_cAdRate.MaxLength = 3;
            this.ud_cAdRate.Name = "ud_cAdRate";
            this.ud_cAdRate.Size = new System.Drawing.Size(40, 21);
            this.ud_cAdRate.TabIndex = 258;
            this.ud_cAdRate.TextAlignment = Janus.Windows.GridEX.TextAlignment.Far;
            this.ud_cAdRate.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            // 
            // ebChannelNum
            // 
            this.ebChannelNum.Location = new System.Drawing.Point(78, 43);
            this.ebChannelNum.MaxLength = 40;
            this.ebChannelNum.Name = "ebChannelNum";
            this.ebChannelNum.ReadOnly = true;
            this.ebChannelNum.Size = new System.Drawing.Size(83, 21);
            this.ebChannelNum.TabIndex = 51;
            this.ebChannelNum.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
            this.ebChannelNum.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            // 
            // ebServiceId
            // 
            this.ebServiceId.Location = new System.Drawing.Point(78, 16);
            this.ebServiceId.MaxLength = 40;
            this.ebServiceId.Name = "ebServiceId";
            this.ebServiceId.ReadOnly = true;
            this.ebServiceId.Size = new System.Drawing.Size(83, 21);
            this.ebServiceId.TabIndex = 50;
            this.ebServiceId.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
            this.ebServiceId.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            // 
            // chkUseYn
            // 
            this.chkUseYn.Checked = true;
            this.chkUseYn.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkUseYn.Location = new System.Drawing.Point(887, 43);
            this.chkUseYn.Name = "chkUseYn";
            this.chkUseYn.Size = new System.Drawing.Size(14, 18);
            this.chkUseYn.TabIndex = 45;
            this.chkUseYn.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            // 
            // chkAdYn
            // 
            this.chkAdYn.Location = new System.Drawing.Point(503, 43);
            this.chkAdYn.Name = "chkAdYn";
            this.chkAdYn.Size = new System.Drawing.Size(14, 18);
            this.chkAdYn.TabIndex = 44;
            this.chkAdYn.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            // 
            // lblChannelNum
            // 
            this.lblChannelNum.Location = new System.Drawing.Point(15, 45);
            this.lblChannelNum.Name = "lblChannelNum";
            this.lblChannelNum.Size = new System.Drawing.Size(57, 21);
            this.lblChannelNum.TabIndex = 39;
            this.lblChannelNum.Text = "ä�ι�ȣ";
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnSave.Location = new System.Drawing.Point(887, 84);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(104, 24);
            this.btnSave.TabIndex = 10;
            this.btnSave.Text = "�� ��";
            this.btnSave.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // lblServiceId
            // 
            this.lblServiceId.Location = new System.Drawing.Point(15, 21);
            this.lblServiceId.Name = "lblServiceId";
            this.lblServiceId.Size = new System.Drawing.Size(57, 21);
            this.lblServiceId.TabIndex = 24;
            this.lblServiceId.Text = "����ID";
            // 
            // ChannelControl
            // 
            this.Controls.Add(this.uiPanel6);
            this.Font = new System.Drawing.Font("�������", 8.999999F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.Name = "ChannelControl";
            this.Size = new System.Drawing.Size(1010, 677);
            this.Load += new System.EventHandler(this.ChannelControl_Load);
            ((System.ComponentModel.ISupportInitialize)(this.uiPM)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanel6)).EndInit();
            this.uiPanel6.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.uiPanel7)).EndInit();
            this.uiPanel7.ResumeLayout(false);
            this.uiPanel7Container.ResumeLayout(false);
            this.pnlSearch.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.uiPanel8)).EndInit();
            this.uiPanel8.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.uiPanel10)).EndInit();
            this.uiPanel10.ResumeLayout(false);
            this.uiPanel10Container.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdGenre)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dvStmCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.channelDs1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanel11)).EndInit();
            this.uiPanel11.ResumeLayout(false);
            this.uiPanel11Container.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdChannel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dvChannel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanel9)).EndInit();
            this.uiPanel9.ResumeLayout(false);
            this.uiPanel9Container.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

		}
		#endregion

        #region ��Ʈ�� �ε�
        private void ChannelControl_Load(object sender, System.EventArgs e)
        {

            
            // �����Ͱ����� ��ü����
            dt = ((DataView)grdGenre.DataSource).Table;  
            dtChild  = ((DataView)grdChannel.DataSource).Table;

            cm = (CurrencyManager)this.BindingContext[grdGenre.DataSource];
            cmChild = (CurrencyManager)this.BindingContext[grdChannel.DataSource];
            // ��Ʈ�� �ʱ�ȭ
            InitControl();
        }

        #endregion

        #region ��Ʈ�� �ʱ�ȭ
        private void InitControl()
        {
            ProgressStart();          

            // ��ȸ���� �˻�
            if(menu.CanRead(MenuCode))
            {
                canRead = true;
                SearchGenre(); // ä���帣 �˻�
            }
			
            // �߰���ư Ȱ��ȭ
            if(menu.CanCreate(MenuCode))
            {
                canCreate = true;
            }

            // ������ư Ȱ��ȭ
            if(menu.CanDelete(MenuCode))
            {
                canDelete = true;
            }

            // �����ư Ȱ��ȭ
            if(menu.CanUpdate(MenuCode))
            {
                ResetTextReadonly();
                canUpdate = true;
            }
            else
            {
               
            }

            InitButton();
            ProgressStop();
        }

        


        private void InitButton()
        {
            if(canRead)   btnSearch.Enabled = true;
          
            if(ebServiceId.Text.Trim().Length > 0) 
            {               
                if(canUpdate) btnSave.Enabled   = true;
            }
            

            Application.DoEvents();
        }

        private void DisableButton()
        {
            //btnSearch.Enabled = false;          
            //btnSave.Enabled   = false;
            
            Application.DoEvents();
        }

        #endregion

        #region ����� �׼�ó�� �޼ҵ�

        /// <summary>
        /// �׸����� Row�����
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnGrdRowChanged(object sender, System.EventArgs e) 
        {
            if (!IsSearching)
            {
                if (IsNotLoading)
                {
                    SetGenreDetailText();
                    
                    InitButton();

                }
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
            keyGenreCode = "";
            keyServiceId = "";
            ReSetChannelDetailText();
            ReSetGridData();
            DisableButton();
            
            SearchGenre();
            InitButton();
            ProgressStop();
        }
               

        /// <summary>
        /// �����ư Ŭ��
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_Click(object sender, System.EventArgs e)
        {            
            SaveChannel();
            autoBinding();
        }
        

        #endregion

        #region ó���޼ҵ�

        /// <summary>
        /// ä���帣��� ��ȸ
        /// </summary>
        private void SearchGenre()
        {
            IsSearching = true;

            StatusMessage("ä�� ������ ��ȸ�մϴ�.");           

            try
            {
                channelModel.Init();
                channelDs1.StmCode.Clear();
                
                // ä�θ����ȸ ���񽺸� ȣ���Ѵ�.
                new ChannelManager(systemModel,commonModel).GetChannelList(channelModel);

                if (channelModel.ResultCD.Equals("0000"))
                {
                    Utility.SetDataTableFast(channelDs1.StmCode, channelModel.ChannelDataSet);				
                    StatusMessage(channelModel.ResultCnt + "���� ä���帣 ������ ��ȸ�Ǿ����ϴ�.");
                   
                    SetGenreDetailText();
                }
            }
            catch(FrameException fe)
            {
                FrameSystem.showMsgForm("ä����ȸ����", new string[] {fe.ErrCode, fe.ResultMsg});
            }
            catch(Exception ex)
            {
                FrameSystem.showMsgForm("ä����ȸ����",new string[] {"",ex.Message});
            }
            finally
            {
                IsSearching = false; // ��ȸ�� Flag ����
            }
        }
     
        /// <summary>
        /// Ű����ã�� �׸��� Ű�� �ش�Ǵ·ο��..
        /// </summary>
        private void AddSchChoice()
        {
            //StatusMessage("Ű��");		

            //try
            //{
            //    int rowIndex = 0;
            //    if ( channelDs1.Tables["StmCod"].Rows.Count < 1 ) return;

            //    foreach (DataRow row in channelDs1.Tables["StmCod"].Rows)
            //    {					
            //        if(IsAdding)
            //        {
            //            cm.Position = 0;
            //            keyGenreCode = null;
            //            keyChannelNo = null;
            //        }
            //        else
            //        {						
            //            if(row["stm_cod"].ToString().Equals(keyGenreCode) && row["ChannelNo"].ToString().Equals(keyChannelNo))
            //            {					
            //                cm.Position = rowIndex;
            //                break;								
            //            }
            //        }

            //        rowIndex++;
            //        grdExChannelList.EnsureVisible();
            //    }
            //}
            //catch(FrameException fe)
            //{
            //    FrameSystem.showMsgForm("Ű������", new string[] {fe.ErrCode, fe.ResultMsg});
            //}
            //catch(Exception ex)
            //{
            //    FrameSystem.showMsgForm("Ű������",new string[] {"",ex.Message});
            //}			
        }
        /// <summary>
        /// ä�θ�� ��ȸ
        /// </summary>
        private void SearchChannel()
        {
            StatusMessage("ä�� ������ ������ ��ȸ�մϴ�.");

            try
            {                                           
                //���� ���� ���� �ʱ�ȭ ���ش�.
                channelModel.Init();

                // �����͸𵨿� ������ ������ ��Ʈ�Ѵ�.
                channelModel.GenreCode = keyGenreCode;
                if (chkSearchYn.Checked)
                    channelModel.CheckYn = "Y";
                else channelModel.CheckYn = "N";

                // ä�θ����ȸ ���񽺸� ȣ���Ѵ�.
                new ChannelManager(systemModel,commonModel).GetChannelDetailList(channelModel);


                Utility.SetDataTable(channelDs1.ChannelCode, channelModel.ChannelDataSet);				
                StatusMessage(channelModel.ResultCnt + "���� ä�� ������ ��ȸ�Ǿ����ϴ�.");
            }
            catch(FrameException fe)
            {
                FrameSystem.showMsgForm("ä����ȸ����", new string[] {fe.ErrCode, fe.ResultMsg});
            }
            catch(Exception ex)
            {
                FrameSystem.showMsgForm("ä����ȸ����",new string[] {"",ex.Message});
            }
        }
       

        /// <summary>
        /// ä�λ����� ����
        /// </summary>
        private void SaveChannel()
        {

            IsAdding = true;

            StatusMessage("ä�� ������ �����մϴ�.");
            
            if(ebServiceId.Text.Trim().Length == 0) 
            {
                MessageBox.Show("�ǽð�ä�� ������ ���õ��� �ʾҽ��ϴ�.","ä�� ����", 
                    MessageBoxButtons.OK, MessageBoxIcon.Information );
                grdChannel.Focus();
                return;								
            }
            
            
                        
            try
            {
                //���� ���� ���� �ʱ�ȭ ���ش�.
                channelModel.Init();
                // �����͸𵨿� ������ ������ ��Ʈ�Ѵ�.
                channelModel.ServiceID = ebServiceId.Text;
                channelModel.ChannelNumber = ebChannelNum.Text;
                if (chkUseYn.Checked)
                    channelModel.UseYn = "Y";
                else channelModel.UseYn = "N";

                if (chkAdYn.Checked)
                    channelModel.AdYn = "Y";
                else channelModel.AdYn = "N";

                channelModel.AdRate = ud_cAdRate.Value.ToString();
                channelModel.AdnRate = ud_cAdNRate.Value.ToString();

              
                // ä�� ������ ���� ���񽺸� ȣ���Ѵ�.
                if (IsAdding)
                {
                    new ChannelManager(systemModel,commonModel).SetChannelAdd(channelModel);
					
                    StatusMessage("ä�� ������ �߰��Ǿ����ϴ�.");
                    //IsAdding = false;
                    ReSetChannelDetailText();
                }
            			

                DisableButton();
                SearchChannel(); // ������Ʈ�� ä�������˻�
                InitButton();
                        
            }
            catch(FrameException fe)
            {
                FrameSystem.showMsgForm("ä������ �������", new string[] {fe.ErrCode, fe.ResultMsg});
            }
            catch(Exception ex)
            {
                FrameSystem.showMsgForm("ä������ �������",new string[] {"",ex.Message});
            }			
        }

      
        /// <summary>
        /// �ǽð�ä�� �帣������ ��Ʈ
        /// </summary>
        private void SetGenreDetailText()
        {
            int curRow = cm.Position;

            if(curRow >= 0 )
            {
                IsNotLoading = false;	// ��ȸ�� �ٽ� ��ȸ�Ǵ� ���� ������.
                try
                {   
                    // ä���帣 �ڵ� �� 
                    keyGenreCode = dt.Rows[curRow]["stm_cod"].ToString();                    
                              
                    //ä�� ��� ��ȸ
                    SearchChannel();

                    //IsAdding = false;
                }
                finally
                {
                    IsNotLoading = true;
                }
            }            
			
            StatusMessage("�غ�");
            InitButton();
        }

        /// <summary>
        /// ���� �� �ǽð�ä������ ��Ʈ�ѿ� ���ε�
        /// </summary>
        private void SetChannelDetailText()
        {
            int curRow = cmChild.Position;
            string nAdRate = "";
            string nAdnRae = "";

            if (curRow >= 0)
            {
                ReSetChannelDetailText();

                ebServiceId.Text = dtChild.Rows[curRow]["svc_id"].ToString();
                keyServiceId = ebServiceId.Text.Trim();
                ebChannelNm.Text = dtChild.Rows[curRow]["ch_nm"].ToString();
                ebChannelNum.Text = dtChild.Rows[curRow]["ch_no"].ToString();
                ebChannelRank.Text = dtChild.Rows[curRow]["ch_rank"].ToString();

                if (dtChild.Rows[curRow]["ad_yn"].ToString().Equals("Y"))                
                    chkAdYn.Checked = true;                
                else                
                    chkAdYn.Checked = false;

                if (dtChild.Rows[curRow]["use_yn"].ToString().Equals("Y"))
                    chkUseYn.Checked = true;
                else
                    chkUseYn.Checked = false;

                nAdRate = dtChild.Rows[curRow]["ad_rate"].ToString();
                if (!nAdRate.Equals(string.Empty))
                {
                    ud_cAdRate.Value = Convert.ToInt32(nAdRate);
                }
                nAdnRae = dtChild.Rows[curRow]["adn_rate"].ToString();
                if (!nAdnRae.Equals(string.Empty))
                {
                    ud_cAdNRate.Value = Convert.ToInt32(nAdnRae);
                }
            }
        }

        /// <summary>
        /// �ǽð�ä�� ���� �� ���� �� ���ε� �ڵ�ó��
        /// </summary>
        private void autoBinding()
        {
            try
            {
                int rowIndex = 0;
                if (channelDs1.Tables["ChannelCode"].Rows.Count < 1) return;
                foreach (DataRow row in channelDs1.Tables["ChannelCode"].Rows)
                {
                    if (row["svc_id"].ToString().Equals(keyServiceId))
                    {
                        cmChild.Position = rowIndex;
                        break;
                    }
                    rowIndex++;
                }
                grdChannel_Click(this, null);
            }
            catch (Exception ex)
            {
                FrameSystem.showMsgForm("Ű������", new string[] { "", ex.Message });
            }
        }

        /// <summary>
        /// �ǽð�ä�� ���ε� ��Ʈ�� �ʱ�ȭ
        /// </summary>
        private void ReSetChannelDetailText()
        {
                      
            ebServiceId.Text  = "";
            ebChannelNum.Text = "";
            ebChannelNm.Text  = "";
            ebChannelRank.Text = "";

            chkAdYn.Checked = false;
            chkUseYn.Checked = true;
            ud_cAdRate.Value = 0;
            ud_cAdNRate.Value = 0;

        }
        
        private void ReSetGridData()
        {
            //gridContentsList.EmptyRows(;
            channelDs1.Contents.Clear();
            //gridContentsList.Dispose
        
        }
        
		
      
        /// <summary>
        /// ������ ����������
        /// </summary>
        private void ResetTextReadonly()
        {

            // �ű��ۼ��̸� ���̵���� ���Ⱑ��
            if (IsAdding)
            {                
            
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

        private void grdChannel_Click(object sender, EventArgs e)
        {
            ReSetChannelDetailText();
            int curRow = cmChild.Position;
            if (curRow < 0)
            {
                ReSetChannelDetailText();
            }
            else
            {
                keyServiceId = "";
                SetChannelDetailText();
            }
        }

        private void grdGenre_Click(object sender, EventArgs e)
        {
            if (grdGenre.RecordCount > 0)
            {
                ReSetChannelDetailText();
                SetGenreDetailText();
                SearchChannel();
            }
        }







    }
}   