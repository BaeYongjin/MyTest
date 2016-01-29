// ===============================================================================
// MediaInfoControl.cs
// ��ü�������� ������� �����մϴ�. 
// ===============================================================================
// Release history
// ===============================================================================
// Copyright (C) 2006 G-Inno Systems Inc.
// All rights reserved.
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
    /// ��ü���� ��Ʈ��
    /// </summary>
    public class MediaInfoControl : System.Windows.Forms.UserControl, IUserControl
    {
        #region �̺�Ʈ�ڵ鷯
        public event StatusEventHandler StatusEvent;			// �����̺�Ʈ �ڵ鷯
        public event ProgressEventHandler ProgressEvent;			// ó�����̺�Ʈ �ڵ鷯
        #endregion

        #region ��ü���� ��ü �� ����

        // �ý��� ���� : ȭ�����
        private SystemModel systemModel = FrameSystem.oSysModel;
        private CommonModel commonModel = FrameSystem.oComModel;
        private Logger log = FrameSystem.oLog;
        private MenuPower menu = FrameSystem.oMenu;

        // �޴��ڵ� : ������ �ʿ��� ȭ�鿡 �ʿ���
        private string menuCode = "";

        // ����� ������
        MediaInfoModel mediasModel = new MediaInfoModel();	// ��ü������

        // ȭ��ó���� ����
        bool IsNewSearchKey = true;					// �˻����Է� ����
        CurrencyManager cm = null;					// ������ �׸����� ���濡 ���� �����ͼ� ������ ���Ͽ�					
        DataTable dt = null;



        bool IsSearching = false; // ��ȸ�� ó�� : ��ȸ�� ��ȭ���� ������Ʈ �Ǵ� ���� ���� �ϱ����� 2011.11.29 JH.Park
        bool IsAdding = false;
        bool canRead = false;
        bool canUpdate = false;
        bool canCreate = false;
        bool canDelete = false;
        private Janus.Windows.EditControls.UICheckBox uiCheckBox3;
        private string mediaInfoCode = null;

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
        private Janus.Windows.UI.Dock.UIPanel uiPanelUserList;
        private Janus.Windows.UI.Dock.UIPanelInnerContainer uiPanelUserListContainer;
        private Janus.Windows.UI.Dock.UIPanel uiPanelUserDetail;
        private Janus.Windows.UI.Dock.UIPanelInnerContainer uiPanelUserDetailContainer;
        private Janus.Windows.UI.Dock.UIPanel uiPanelUsersSearch;
        private Janus.Windows.UI.Dock.UIPanelInnerContainer uiPanelUsersSearchContainer;
        private Janus.Windows.EditControls.UIButton uiButton2;
        private Janus.Windows.EditControls.UIButton uiButton1;
        private Janus.Windows.UI.Dock.UIPanelGroup uiPanelUsers;
        private System.Windows.Forms.Panel pnlSearch;
        private Janus.Windows.GridEX.EditControls.EditBox ebSearchKey;
        private Janus.Windows.EditControls.UIButton btnSearch;
        private System.Windows.Forms.Panel pnlUserDetail;
        private System.Windows.Forms.Label lbUserDept;
        private System.Windows.Forms.Label lbUserName;
        private System.Windows.Forms.Label lbRegDt;
        private Janus.Windows.EditControls.UIButton btnAdd;
        private Janus.Windows.EditControls.UIButton btnSave;
        private Janus.Windows.GridEX.EditControls.EditBox ebRegDt;
        private Janus.Windows.EditControls.UIButton btnDelete;
        private System.Data.DataView dvMediaInfo;
        private System.Windows.Forms.Label lbUserTell;
        private System.Windows.Forms.Label lbUserID;
        private Janus.Windows.GridEX.EditControls.EditBox ebTell;
        private Janus.Windows.GridEX.EditControls.EditBox ebMediaName;
        private Janus.Windows.GridEX.EditControls.EditBox ebCharger;
        private Janus.Windows.GridEX.EditControls.EditBox ebEmail;
        private AdManagerClient.MediaInfoDs mediaInfoDs;
        private System.Windows.Forms.Label label1;
        private Janus.Windows.GridEX.EditControls.EditBox editBox1;
        private Janus.Windows.GridEX.EditControls.EditBox ebModDt;
        private Janus.Windows.GridEX.GridEX grdExMediaList;
        private Janus.Windows.EditControls.UIRadioButton rbUseYn_N;
        private Janus.Windows.EditControls.UIRadioButton rbUseYn_Y;
        private System.Windows.Forms.Label lbUseYn;
        private System.Windows.Forms.Label label2;
        private System.ComponentModel.IContainer components;

        public MediaInfoControl()
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
			Janus.Windows.GridEX.GridEXLayout grdExMediaList_DesignTimeLayout = new Janus.Windows.GridEX.GridEXLayout();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MediaInfoControl));
			this.uiPM = new Janus.Windows.UI.Dock.UIPanelManager(this.components);
			this.uiPanelUsers = new Janus.Windows.UI.Dock.UIPanelGroup();
			this.uiPanelUsersSearch = new Janus.Windows.UI.Dock.UIPanel();
			this.uiPanelUsersSearchContainer = new Janus.Windows.UI.Dock.UIPanelInnerContainer();
			this.pnlSearch = new System.Windows.Forms.Panel();
			this.uiCheckBox3 = new Janus.Windows.EditControls.UICheckBox();
			this.ebSearchKey = new Janus.Windows.GridEX.EditControls.EditBox();
			this.btnSearch = new Janus.Windows.EditControls.UIButton();
			this.uiPanelUserList = new Janus.Windows.UI.Dock.UIPanel();
			this.uiPanelUserListContainer = new Janus.Windows.UI.Dock.UIPanelInnerContainer();
			this.grdExMediaList = new Janus.Windows.GridEX.GridEX();
			this.dvMediaInfo = new System.Data.DataView();
			this.mediaInfoDs = new AdManagerClient.MediaInfoDs();
			this.uiPanelUserDetail = new Janus.Windows.UI.Dock.UIPanel();
			this.uiPanelUserDetailContainer = new Janus.Windows.UI.Dock.UIPanelInnerContainer();
			this.pnlUserDetail = new System.Windows.Forms.Panel();
			this.lbUserTell = new System.Windows.Forms.Label();
			this.ebTell = new Janus.Windows.GridEX.EditControls.EditBox();
			this.lbUserDept = new System.Windows.Forms.Label();
			this.ebMediaName = new Janus.Windows.GridEX.EditControls.EditBox();
			this.lbUserID = new System.Windows.Forms.Label();
			this.ebCharger = new Janus.Windows.GridEX.EditControls.EditBox();
			this.lbUserName = new System.Windows.Forms.Label();
			this.lbRegDt = new System.Windows.Forms.Label();
			this.ebRegDt = new Janus.Windows.GridEX.EditControls.EditBox();
			this.ebEmail = new Janus.Windows.GridEX.EditControls.EditBox();
			this.label1 = new System.Windows.Forms.Label();
			this.ebModDt = new Janus.Windows.GridEX.EditControls.EditBox();
			this.btnAdd = new Janus.Windows.EditControls.UIButton();
			this.btnDelete = new Janus.Windows.EditControls.UIButton();
			this.btnSave = new Janus.Windows.EditControls.UIButton();
			this.lbUseYn = new System.Windows.Forms.Label();
			this.rbUseYn_N = new Janus.Windows.EditControls.UIRadioButton();
			this.rbUseYn_Y = new Janus.Windows.EditControls.UIRadioButton();
			this.label2 = new System.Windows.Forms.Label();
			this.editBox1 = new Janus.Windows.GridEX.EditControls.EditBox();
			this.uiButton1 = new Janus.Windows.EditControls.UIButton();
			this.uiButton2 = new Janus.Windows.EditControls.UIButton();
			((System.ComponentModel.ISupportInitialize)(this.uiPM)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.uiPanelUsers)).BeginInit();
			this.uiPanelUsers.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.uiPanelUsersSearch)).BeginInit();
			this.uiPanelUsersSearch.SuspendLayout();
			this.uiPanelUsersSearchContainer.SuspendLayout();
			this.pnlSearch.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.uiPanelUserList)).BeginInit();
			this.uiPanelUserList.SuspendLayout();
			this.uiPanelUserListContainer.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.grdExMediaList)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.dvMediaInfo)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.mediaInfoDs)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.uiPanelUserDetail)).BeginInit();
			this.uiPanelUserDetail.SuspendLayout();
			this.uiPanelUserDetailContainer.SuspendLayout();
			this.pnlUserDetail.SuspendLayout();
			this.SuspendLayout();
			// 
			// uiPM
			// 
			this.uiPM.BackColorGradientAutoHideStrip = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(245)))), ((int)(((byte)(243)))));
			this.uiPM.ContainerControl = this;
			this.uiPM.VisualStyle = Janus.Windows.UI.Dock.PanelVisualStyle.Office2007;
			this.uiPanelUsers.Id = new System.Guid("b162c53a-7940-47cd-a0ed-9762c59c50d8");
			this.uiPanelUsers.StaticGroup = true;
			this.uiPanelUsersSearch.Id = new System.Guid("afb3b7d0-aa79-443c-a1e8-3baba38f7a4d");
			this.uiPanelUsers.Panels.Add(this.uiPanelUsersSearch);
			this.uiPanelUserList.Id = new System.Guid("8e3140b9-bfcb-43af-b84e-2ee6cddf51dc");
			this.uiPanelUsers.Panels.Add(this.uiPanelUserList);
			this.uiPanelUserDetail.Id = new System.Guid("b2973a57-d3e1-4d80-9387-7a6dc3332542");
			this.uiPanelUsers.Panels.Add(this.uiPanelUserDetail);
			this.uiPM.Panels.Add(this.uiPanelUsers);
			// 
			// Design Time Panel Info:
			// 
			this.uiPM.BeginPanelInfo();
			this.uiPM.AddDockPanelInfo(new System.Guid("b162c53a-7940-47cd-a0ed-9762c59c50d8"), Janus.Windows.UI.Dock.PanelGroupStyle.HorizontalTiles, Janus.Windows.UI.Dock.PanelDockStyle.Fill, true, new System.Drawing.Size(1004, 671), true);
			this.uiPM.AddDockPanelInfo(new System.Guid("afb3b7d0-aa79-443c-a1e8-3baba38f7a4d"), new System.Guid("b162c53a-7940-47cd-a0ed-9762c59c50d8"), 42, true);
			this.uiPM.AddDockPanelInfo(new System.Guid("8e3140b9-bfcb-43af-b84e-2ee6cddf51dc"), new System.Guid("b162c53a-7940-47cd-a0ed-9762c59c50d8"), 399, true);
			this.uiPM.AddDockPanelInfo(new System.Guid("b2973a57-d3e1-4d80-9387-7a6dc3332542"), new System.Guid("b162c53a-7940-47cd-a0ed-9762c59c50d8"), 180, true);
			this.uiPM.AddFloatingPanelInfo(new System.Guid("b162c53a-7940-47cd-a0ed-9762c59c50d8"), Janus.Windows.UI.Dock.PanelGroupStyle.HorizontalTiles, true, new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
			this.uiPM.AddFloatingPanelInfo(new System.Guid("8e3140b9-bfcb-43af-b84e-2ee6cddf51dc"), new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
			this.uiPM.AddFloatingPanelInfo(new System.Guid("b2973a57-d3e1-4d80-9387-7a6dc3332542"), new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
			this.uiPM.AddFloatingPanelInfo(new System.Guid("afb3b7d0-aa79-443c-a1e8-3baba38f7a4d"), new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
			this.uiPM.EndPanelInfo();
			// 
			// uiPanelUsers
			// 
			this.uiPanelUsers.ActiveCaptionMode = Janus.Windows.UI.Dock.ActiveCaptionMode.Never;
			this.uiPanelUsers.BackColor = System.Drawing.SystemColors.Window;
			this.uiPanelUsers.CaptionVisible = Janus.Windows.UI.InheritableBoolean.True;
			this.uiPanelUsers.CloseButtonVisible = Janus.Windows.UI.InheritableBoolean.False;
			this.uiPanelUsers.Location = new System.Drawing.Point(3, 3);
			this.uiPanelUsers.Name = "uiPanelUsers";
			this.uiPanelUsers.Size = new System.Drawing.Size(1004, 671);
			this.uiPanelUsers.TabIndex = 0;
			this.uiPanelUsers.TabStop = false;
			this.uiPanelUsers.Text = "��ü��������";
			// 
			// uiPanelUsersSearch
			// 
			this.uiPanelUsersSearch.AllowResize = Janus.Windows.UI.InheritableBoolean.False;
			this.uiPanelUsersSearch.CaptionVisible = Janus.Windows.UI.InheritableBoolean.False;
			this.uiPanelUsersSearch.CloseButtonVisible = Janus.Windows.UI.InheritableBoolean.False;
			this.uiPanelUsersSearch.InnerContainer = this.uiPanelUsersSearchContainer;
			this.uiPanelUsersSearch.Location = new System.Drawing.Point(0, 22);
			this.uiPanelUsersSearch.Name = "uiPanelUsersSearch";
			this.uiPanelUsersSearch.Size = new System.Drawing.Size(1004, 43);
			this.uiPanelUsersSearch.TabIndex = 0;
			this.uiPanelUsersSearch.Text = "�˻�";
			// 
			// uiPanelUsersSearchContainer
			// 
			this.uiPanelUsersSearchContainer.Controls.Add(this.pnlSearch);
			this.uiPanelUsersSearchContainer.Location = new System.Drawing.Point(1, 1);
			this.uiPanelUsersSearchContainer.Name = "uiPanelUsersSearchContainer";
			this.uiPanelUsersSearchContainer.Size = new System.Drawing.Size(1002, 41);
			this.uiPanelUsersSearchContainer.TabIndex = 0;
			// 
			// pnlSearch
			// 
			this.pnlSearch.BackColor = System.Drawing.SystemColors.Window;
			this.pnlSearch.Controls.Add(this.uiCheckBox3);
			this.pnlSearch.Controls.Add(this.ebSearchKey);
			this.pnlSearch.Controls.Add(this.btnSearch);
			this.pnlSearch.Dock = System.Windows.Forms.DockStyle.Fill;
			this.pnlSearch.Location = new System.Drawing.Point(0, 0);
			this.pnlSearch.Name = "pnlSearch";
			this.pnlSearch.Size = new System.Drawing.Size(1002, 41);
			this.pnlSearch.TabIndex = 0;
			// 
			// uiCheckBox3
			// 
			this.uiCheckBox3.Location = new System.Drawing.Point(232, 11);
			this.uiCheckBox3.Name = "uiCheckBox3";
			this.uiCheckBox3.Size = new System.Drawing.Size(104, 23);
			this.uiCheckBox3.TabIndex = 52;
			this.uiCheckBox3.Text = "������ ����";
			// 
			// ebSearchKey
			// 
			this.ebSearchKey.Location = new System.Drawing.Point(8, 11);
			this.ebSearchKey.Name = "ebSearchKey";
			this.ebSearchKey.Size = new System.Drawing.Size(208, 21);
			this.ebSearchKey.TabIndex = 1;
			this.ebSearchKey.Text = "�˻�� �Է��ϼ���";
			this.ebSearchKey.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
			this.ebSearchKey.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
			this.ebSearchKey.TextChanged += new System.EventHandler(this.ebSearchKey_TextChanged);
			this.ebSearchKey.Click += new System.EventHandler(this.ebSearchKey_Click);
			this.ebSearchKey.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ebSearchKey_KeyDown);
			// 
			// btnSearch
			// 
			this.btnSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnSearch.Enabled = false;
			this.btnSearch.Location = new System.Drawing.Point(887, 8);
			this.btnSearch.Name = "btnSearch";
			this.btnSearch.Size = new System.Drawing.Size(104, 24);
			this.btnSearch.TabIndex = 3;
			this.btnSearch.Text = "�� ȸ";
			this.btnSearch.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
			this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
			// 
			// uiPanelUserList
			// 
			this.uiPanelUserList.BackColor = System.Drawing.SystemColors.InactiveCaptionText;
			this.uiPanelUserList.CaptionStyle = Janus.Windows.UI.Dock.PanelCaptionStyle.Dark;
			this.uiPanelUserList.CaptionVisible = Janus.Windows.UI.InheritableBoolean.True;
			this.uiPanelUserList.ImeMode = System.Windows.Forms.ImeMode.AlphaFull;
			this.uiPanelUserList.InnerContainer = this.uiPanelUserListContainer;
			this.uiPanelUserList.Location = new System.Drawing.Point(0, 69);
			this.uiPanelUserList.MinimumSize = new System.Drawing.Size(-1, 100);
			this.uiPanelUserList.Name = "uiPanelUserList";
			this.uiPanelUserList.Size = new System.Drawing.Size(1004, 413);
			this.uiPanelUserList.TabIndex = 3;
			this.uiPanelUserList.TabStop = false;
			this.uiPanelUserList.Text = "��ü���";
			// 
			// uiPanelUserListContainer
			// 
			this.uiPanelUserListContainer.BackColor = System.Drawing.SystemColors.Window;
			this.uiPanelUserListContainer.Controls.Add(this.grdExMediaList);
			this.uiPanelUserListContainer.Location = new System.Drawing.Point(1, 23);
			this.uiPanelUserListContainer.Name = "uiPanelUserListContainer";
			this.uiPanelUserListContainer.Size = new System.Drawing.Size(1002, 389);
			this.uiPanelUserListContainer.TabIndex = 0;
			// 
			// grdExMediaList
			// 
			this.grdExMediaList.AllowEdit = Janus.Windows.GridEX.InheritableBoolean.False;
			this.grdExMediaList.AlternatingColors = true;
			this.grdExMediaList.AlternatingRowFormatStyle.BackgroundImageDrawMode = Janus.Windows.GridEX.BackgroundImageDrawMode.Tile;
			this.grdExMediaList.BorderStyle = Janus.Windows.GridEX.BorderStyle.None;
			this.grdExMediaList.DataSource = this.dvMediaInfo;
			grdExMediaList_DesignTimeLayout.LayoutString = resources.GetString("grdExMediaList_DesignTimeLayout.LayoutString");
			this.grdExMediaList.DesignTimeLayout = grdExMediaList_DesignTimeLayout;
			this.grdExMediaList.Dock = System.Windows.Forms.DockStyle.Fill;
			this.grdExMediaList.EditorsControlStyle.ButtonAppearance = Janus.Windows.GridEX.ButtonAppearance.Regular;
			this.grdExMediaList.EmptyRows = true;
			this.grdExMediaList.FocusCellFormatStyle.BackColor = System.Drawing.SystemColors.Highlight;
			this.grdExMediaList.FocusCellFormatStyle.ForeColor = System.Drawing.Color.White;
			this.grdExMediaList.FocusStyle = Janus.Windows.GridEX.FocusStyle.None;
			this.grdExMediaList.FrozenColumns = 2;
			this.grdExMediaList.GridLineColor = System.Drawing.Color.Silver;
			this.grdExMediaList.GridLines = Janus.Windows.GridEX.GridLines.Vertical;
			this.grdExMediaList.GridLineStyle = Janus.Windows.GridEX.GridLineStyle.Solid;
			this.grdExMediaList.GroupByBoxVisible = false;
			this.grdExMediaList.HeaderFormatStyle.TextAlignment = Janus.Windows.GridEX.TextAlignment.Center;
			this.grdExMediaList.HideSelection = Janus.Windows.GridEX.HideSelection.HighlightInactive;
			this.grdExMediaList.Location = new System.Drawing.Point(0, 0);
			this.grdExMediaList.Name = "grdExMediaList";
			this.grdExMediaList.ScrollBars = Janus.Windows.GridEX.ScrollBars.Vertical;
			this.grdExMediaList.SelectedInactiveFormatStyle.BackColor = System.Drawing.Color.Gold;
			this.grdExMediaList.Size = new System.Drawing.Size(1002, 389);
			this.grdExMediaList.TabIndex = 4;
			this.grdExMediaList.TabKeyBehavior = Janus.Windows.GridEX.TabKeyBehavior.ControlNavigation;
			this.grdExMediaList.ThemedAreas = ((Janus.Windows.GridEX.ThemedArea)(((((((((Janus.Windows.GridEX.ThemedArea.ScrollBars | Janus.Windows.GridEX.ThemedArea.EditControls) 
            | Janus.Windows.GridEX.ThemedArea.Headers) 
            | Janus.Windows.GridEX.ThemedArea.GroupByBox) 
            | Janus.Windows.GridEX.ThemedArea.GroupRows) 
            | Janus.Windows.GridEX.ThemedArea.ControlBorder) 
            | Janus.Windows.GridEX.ThemedArea.Cards) 
            | Janus.Windows.GridEX.ThemedArea.Gridlines) 
            | Janus.Windows.GridEX.ThemedArea.CheckBoxes)));
			this.grdExMediaList.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
			this.grdExMediaList.Enter += new System.EventHandler(this.OnGrdRowChanged);
			// 
			// dvMediaInfo
			// 
			this.dvMediaInfo.Table = this.mediaInfoDs.Medias;
			// 
			// mediaInfoDs
			// 
			this.mediaInfoDs.DataSetName = "MediaInfoDs";
			this.mediaInfoDs.Locale = new System.Globalization.CultureInfo("en-US");
			this.mediaInfoDs.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
			// 
			// uiPanelUserDetail
			// 
			this.uiPanelUserDetail.AllowResize = Janus.Windows.UI.InheritableBoolean.False;
			this.uiPanelUserDetail.BackColor = System.Drawing.SystemColors.Window;
			this.uiPanelUserDetail.CaptionStyle = Janus.Windows.UI.Dock.PanelCaptionStyle.Dark;
			this.uiPanelUserDetail.InnerContainer = this.uiPanelUserDetailContainer;
			this.uiPanelUserDetail.Location = new System.Drawing.Point(0, 486);
			this.uiPanelUserDetail.Name = "uiPanelUserDetail";
			this.uiPanelUserDetail.Size = new System.Drawing.Size(1004, 185);
			this.uiPanelUserDetail.TabIndex = 5;
			this.uiPanelUserDetail.TabStop = false;
			this.uiPanelUserDetail.Text = "������";
			// 
			// uiPanelUserDetailContainer
			// 
			this.uiPanelUserDetailContainer.Controls.Add(this.pnlUserDetail);
			this.uiPanelUserDetailContainer.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.uiPanelUserDetailContainer.Location = new System.Drawing.Point(1, 23);
			this.uiPanelUserDetailContainer.Name = "uiPanelUserDetailContainer";
			this.uiPanelUserDetailContainer.Size = new System.Drawing.Size(1002, 161);
			this.uiPanelUserDetailContainer.TabIndex = 0;
			// 
			// pnlUserDetail
			// 
			this.pnlUserDetail.BackColor = System.Drawing.SystemColors.Window;
			this.pnlUserDetail.Controls.Add(this.lbUserTell);
			this.pnlUserDetail.Controls.Add(this.ebTell);
			this.pnlUserDetail.Controls.Add(this.lbUserDept);
			this.pnlUserDetail.Controls.Add(this.ebMediaName);
			this.pnlUserDetail.Controls.Add(this.lbUserID);
			this.pnlUserDetail.Controls.Add(this.ebCharger);
			this.pnlUserDetail.Controls.Add(this.lbUserName);
			this.pnlUserDetail.Controls.Add(this.lbRegDt);
			this.pnlUserDetail.Controls.Add(this.ebRegDt);
			this.pnlUserDetail.Controls.Add(this.ebEmail);
			this.pnlUserDetail.Controls.Add(this.label1);
			this.pnlUserDetail.Controls.Add(this.ebModDt);
			this.pnlUserDetail.Controls.Add(this.btnAdd);
			this.pnlUserDetail.Controls.Add(this.btnDelete);
			this.pnlUserDetail.Controls.Add(this.btnSave);
			this.pnlUserDetail.Controls.Add(this.lbUseYn);
			this.pnlUserDetail.Controls.Add(this.rbUseYn_N);
			this.pnlUserDetail.Controls.Add(this.rbUseYn_Y);
			this.pnlUserDetail.Controls.Add(this.label2);
			this.pnlUserDetail.Dock = System.Windows.Forms.DockStyle.Fill;
			this.pnlUserDetail.Location = new System.Drawing.Point(0, 0);
			this.pnlUserDetail.Name = "pnlUserDetail";
			this.pnlUserDetail.Size = new System.Drawing.Size(1002, 161);
			this.pnlUserDetail.TabIndex = 0;
			// 
			// lbUserTell
			// 
			this.lbUserTell.BackColor = System.Drawing.SystemColors.Window;
			this.lbUserTell.Location = new System.Drawing.Point(376, 32);
			this.lbUserTell.Name = "lbUserTell";
			this.lbUserTell.Size = new System.Drawing.Size(72, 16);
			this.lbUserTell.TabIndex = 30;
			this.lbUserTell.Text = "��ȭ��ȣ";
			// 
			// ebTell
			// 
			this.ebTell.Location = new System.Drawing.Point(448, 32);
			this.ebTell.MaxLength = 15;
			this.ebTell.Name = "ebTell";
			this.ebTell.Size = new System.Drawing.Size(280, 21);
			this.ebTell.TabIndex = 9;
			this.ebTell.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
			this.ebTell.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
			// 
			// lbUserDept
			// 
			this.lbUserDept.Location = new System.Drawing.Point(376, 12);
			this.lbUserDept.Name = "lbUserDept";
			this.lbUserDept.Size = new System.Drawing.Size(72, 16);
			this.lbUserDept.TabIndex = 24;
			this.lbUserDept.Text = "E-mail";
			// 
			// ebMediaName
			// 
			this.ebMediaName.BackColor = System.Drawing.Color.WhiteSmoke;
			this.ebMediaName.Location = new System.Drawing.Point(80, 8);
			this.ebMediaName.MaxLength = 20;
			this.ebMediaName.Name = "ebMediaName";
			this.ebMediaName.Size = new System.Drawing.Size(280, 21);
			this.ebMediaName.TabIndex = 6;
			this.ebMediaName.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
			this.ebMediaName.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
			// 
			// lbUserID
			// 
			this.lbUserID.BackColor = System.Drawing.SystemColors.Window;
			this.lbUserID.Location = new System.Drawing.Point(8, 12);
			this.lbUserID.Name = "lbUserID";
			this.lbUserID.Size = new System.Drawing.Size(72, 16);
			this.lbUserID.TabIndex = 18;
			this.lbUserID.Text = "��ü��Ī";
			// 
			// ebCharger
			// 
			this.ebCharger.Location = new System.Drawing.Point(80, 32);
			this.ebCharger.MaxLength = 10;
			this.ebCharger.Name = "ebCharger";
			this.ebCharger.Size = new System.Drawing.Size(280, 21);
			this.ebCharger.TabIndex = 8;
			this.ebCharger.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
			this.ebCharger.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
			// 
			// lbUserName
			// 
			this.lbUserName.Location = new System.Drawing.Point(8, 34);
			this.lbUserName.Name = "lbUserName";
			this.lbUserName.Size = new System.Drawing.Size(72, 16);
			this.lbUserName.TabIndex = 19;
			this.lbUserName.Text = "����ڸ�";
			// 
			// lbRegDt
			// 
			this.lbRegDt.Location = new System.Drawing.Point(8, 56);
			this.lbRegDt.Name = "lbRegDt";
			this.lbRegDt.Size = new System.Drawing.Size(72, 16);
			this.lbRegDt.TabIndex = 29;
			this.lbRegDt.Text = "����Ͻ�";
			// 
			// ebRegDt
			// 
			this.ebRegDt.BackColor = System.Drawing.Color.WhiteSmoke;
			this.ebRegDt.Location = new System.Drawing.Point(80, 56);
			this.ebRegDt.Name = "ebRegDt";
			this.ebRegDt.ReadOnly = true;
			this.ebRegDt.Size = new System.Drawing.Size(280, 21);
			this.ebRegDt.TabIndex = 0;
			this.ebRegDt.TabStop = false;
			this.ebRegDt.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
			this.ebRegDt.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
			// 
			// ebEmail
			// 
			this.ebEmail.Location = new System.Drawing.Point(448, 8);
			this.ebEmail.MaxLength = 40;
			this.ebEmail.Name = "ebEmail";
			this.ebEmail.Size = new System.Drawing.Size(280, 21);
			this.ebEmail.TabIndex = 7;
			this.ebEmail.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
			this.ebEmail.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(8, 80);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(72, 16);
			this.label1.TabIndex = 29;
			this.label1.Text = "�����Ͻ�";
			// 
			// ebModDt
			// 
			this.ebModDt.BackColor = System.Drawing.Color.WhiteSmoke;
			this.ebModDt.Location = new System.Drawing.Point(80, 80);
			this.ebModDt.Name = "ebModDt";
			this.ebModDt.ReadOnly = true;
			this.ebModDt.Size = new System.Drawing.Size(280, 21);
			this.ebModDt.TabIndex = 0;
			this.ebModDt.TabStop = false;
			this.ebModDt.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
			this.ebModDt.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
			// 
			// btnAdd
			// 
			this.btnAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.btnAdd.Enabled = false;
			this.btnAdd.Location = new System.Drawing.Point(232, 123);
			this.btnAdd.Name = "btnAdd";
			this.btnAdd.Size = new System.Drawing.Size(104, 24);
			this.btnAdd.TabIndex = 12;
			this.btnAdd.Text = "�� ��";
			this.btnAdd.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
			this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
			// 
			// btnDelete
			// 
			this.btnDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.btnDelete.Enabled = false;
			this.btnDelete.Location = new System.Drawing.Point(120, 123);
			this.btnDelete.Name = "btnDelete";
			this.btnDelete.Size = new System.Drawing.Size(104, 24);
			this.btnDelete.TabIndex = 11;
			this.btnDelete.Text = "�� ��";
			this.btnDelete.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
			this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
			// 
			// btnSave
			// 
			this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.btnSave.Enabled = false;
			this.btnSave.Location = new System.Drawing.Point(8, 123);
			this.btnSave.Name = "btnSave";
			this.btnSave.Size = new System.Drawing.Size(104, 24);
			this.btnSave.TabIndex = 10;
			this.btnSave.Text = "�� ��";
			this.btnSave.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
			this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
			// 
			// lbUseYn
			// 
			this.lbUseYn.Location = new System.Drawing.Point(376, 56);
			this.lbUseYn.Name = "lbUseYn";
			this.lbUseYn.Size = new System.Drawing.Size(72, 21);
			this.lbUseYn.TabIndex = 37;
			this.lbUseYn.Text = "��뿩��";
			this.lbUseYn.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// rbUseYn_N
			// 
			this.rbUseYn_N.BackColor = System.Drawing.Color.Transparent;
			this.rbUseYn_N.Location = new System.Drawing.Point(528, 56);
			this.rbUseYn_N.Name = "rbUseYn_N";
			this.rbUseYn_N.Size = new System.Drawing.Size(72, 23);
			this.rbUseYn_N.TabIndex = 9;
			this.rbUseYn_N.Text = "������";
			// 
			// rbUseYn_Y
			// 
			this.rbUseYn_Y.BackColor = System.Drawing.Color.Transparent;
			this.rbUseYn_Y.Location = new System.Drawing.Point(448, 56);
			this.rbUseYn_Y.Name = "rbUseYn_Y";
			this.rbUseYn_Y.Size = new System.Drawing.Size(72, 23);
			this.rbUseYn_Y.TabIndex = 9;
			this.rbUseYn_Y.Text = "�����";
			// 
			// label2
			// 
			this.label2.ForeColor = System.Drawing.Color.Red;
			this.label2.Location = new System.Drawing.Point(376, 80);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(352, 23);
			this.label2.TabIndex = 4;
			this.label2.Text = "�ظ�ü�ڵ尡 \'1\'�� ��쿡�� ��뿩�θ� ���� �� �� �����ϴ�.";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// editBox1
			// 
			this.editBox1.Location = new System.Drawing.Point(0, 0);
			this.editBox1.Name = "editBox1";
			this.editBox1.Size = new System.Drawing.Size(100, 21);
			this.editBox1.TabIndex = 0;
			this.editBox1.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
			// 
			// uiButton1
			// 
			this.uiButton1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.uiButton1.Location = new System.Drawing.Point(136, 8);
			this.uiButton1.Name = "uiButton1";
			this.uiButton1.Size = new System.Drawing.Size(112, 24);
			this.uiButton1.TabIndex = 5;
			this.uiButton1.Text = "�� ��";
			this.uiButton1.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
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
			// MediaInfoControl
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.Controls.Add(this.uiPanelUsers);
			this.Font = global::AdManagerClient.Properties.Settings.Default.Font1;
			this.Name = "MediaInfoControl";
			this.Size = new System.Drawing.Size(1010, 677);
			this.Load += new System.EventHandler(this.UserControl_Load);
			((System.ComponentModel.ISupportInitialize)(this.uiPM)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.uiPanelUsers)).EndInit();
			this.uiPanelUsers.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.uiPanelUsersSearch)).EndInit();
			this.uiPanelUsersSearch.ResumeLayout(false);
			this.uiPanelUsersSearchContainer.ResumeLayout(false);
			this.pnlSearch.ResumeLayout(false);
			this.pnlSearch.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.uiPanelUserList)).EndInit();
			this.uiPanelUserList.ResumeLayout(false);
			this.uiPanelUserListContainer.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.grdExMediaList)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.dvMediaInfo)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.mediaInfoDs)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.uiPanelUserDetail)).EndInit();
			this.uiPanelUserDetail.ResumeLayout(false);
			this.uiPanelUserDetailContainer.ResumeLayout(false);
			this.pnlUserDetail.ResumeLayout(false);
			this.pnlUserDetail.PerformLayout();
			this.ResumeLayout(false);

        }
        #endregion

        #region ��Ʈ�� �ε�
        private void UserControl_Load(object sender, System.EventArgs e)
        {
            // �����Ͱ����� ��ü����
            dt = ((DataView)grdExMediaList.DataSource).Table;
            cm = (CurrencyManager)this.BindingContext[grdExMediaList.DataSource];
            cm.PositionChanged += new System.EventHandler(OnGrdRowChanged);

            // ��Ʈ�� �ʱ�ȭ
            InitControl();
        }

        #endregion

        #region ��Ʈ�� �ʱ�ȭ
        private void InitControl()
        {
            ProgressStart();

            // ��ȸ���� �˻�
            if (menu.CanRead(MenuCode))
            {
                canRead = true;
                SearchUsers();
            }

            // �߰���ư Ȱ��ȭ
            if (menu.CanCreate(MenuCode))
            {
                canCreate = true;
            }

            // ������ư Ȱ��ȭ
            if (menu.CanDelete(MenuCode))
            {
                canDelete = true;
            }

            // �����ư Ȱ��ȭ
            if (menu.CanUpdate(MenuCode))
            {
                ResetTextReadonly();
                canUpdate = true;
            }
            else
            {
                SetTextReadonly();
            }

            InitButton();
            SetTextReadonly();

            ProgressStop();
        }

        private void InitButton()
        {
            if (canRead) btnSearch.Enabled = true;
            if (canCreate) btnAdd.Enabled = true;

            if (ebMediaName.Text.Trim().Length > 0)
            {
                if (canDelete) btnDelete.Enabled = true;
                if (canUpdate) btnSave.Enabled = true;
            }
        }

        private void DisableButton()
        {
            btnSearch.Enabled = false;
            btnAdd.Enabled = false;
            btnSave.Enabled = false;
            btnDelete.Enabled = false;
        }

        #endregion

        #region ��ü �׼�ó�� �޼ҵ�

        /// <summary>
        /// �׸����� Row�����
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnGrdRowChanged(object sender, System.EventArgs e)
        {
            if (!IsSearching) // 2011.11.29 RH.Park ��ȸ���� �ƴҰ�쿡�� �����ϵ��� ����
            {
                if (grdExMediaList.RecordCount > 0)
                {
                    SetUserDetailText();
                }
                InitButton();
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

            SearchUsers();

            InitButton();
            ProgressStop();
        }

        /// <summary>
        /// �߰���ư Ŭ��
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAdd_Click(object sender, System.EventArgs e)
        {
            btnAdd.Enabled = false;
            btnDelete.Enabled = false;
            btnSave.Enabled = true;

            IsAdding = true;

            ResetTextReadonly();
            ResetUserDetailText();

            ebMediaName.Focus();
        }

        /// <summary>
        /// �����ư Ŭ��
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_Click(object sender, System.EventArgs e)
        {
            SaveMediaInfoDetail();
        }

        /// <summary>
        /// ������ư Ŭ��
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDelete_Click(object sender, System.EventArgs e)
        {
            DeleteUser();
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
            if (IsNewSearchKey)
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
            if (e.KeyCode == Keys.Enter)
            {
                SearchUsers();
            }
        }

        #endregion

        #region ó���޼ҵ�

        /// <summary>
        /// ��ü��� ��ȸ
        /// </summary>
        private void SearchUsers()
        {
            IsSearching = true;

            StatusMessage("��ü ������ ��ȸ�մϴ�.");

            try
            {
                mediasModel.Init();
                // �����͸𵨿� ������ ������ ��Ʈ�Ѵ�.
                if (IsNewSearchKey)
                {
                    mediasModel.SearchKey = "";
                }
                else
                {
                    mediasModel.SearchKey = ebSearchKey.Text;
                }


                ResetUserDetailText();

                if (uiCheckBox3.Checked)
                {
                    mediasModel.SearchchkAdState_10 = "Y";
                }
                else
                {
                    mediasModel.SearchchkAdState_10 = "N";
                }
                // ��ü�����ȸ ���񽺸� ȣ���Ѵ�.
                new MediaInfoManager(systemModel, commonModel).GetUserList(mediasModel);

                if (mediasModel.ResultCD.Equals("0000"))
                {
                    Utility.SetDataTable(mediaInfoDs.Medias, mediasModel.UserDataSet);
                    StatusMessage(mediasModel.ResultCnt + "���� ��ü ������ ��ȸ�Ǿ����ϴ�.");
                    if (canUpdate)
                    {
                        AddSchChoice();
                    }
                    SetUserDetailText();
                }
            }
            catch (FrameException fe)
            {
                FrameSystem.showMsgForm("��ü��ȸ����", new string[] { fe.ErrCode, fe.ResultMsg });
            }
            catch (Exception ex)
            {
                FrameSystem.showMsgForm("��ü��ȸ����", new string[] { "", ex.Message });
            }
            finally
            {
                IsSearching = false;
            }
        }

        /// <summary>
        /// Ű����ã�� �׸��� Ű�� �ش�Ǵ·ο��..
        /// </summary>
        private void AddSchChoice()
        {
            StatusMessage("Ű��");

            try
            {
                int rowIndex = 0;
                if (mediaInfoDs.Tables["Medias"].Rows.Count < 1) return;

                foreach (DataRow row in mediaInfoDs.Tables["Medias"].Rows)
                {
                    if (IsAdding)
                    {
                        cm.Position = 0;
                        mediaInfoCode = null;
                    }
                    else
                    {
                        if (row["MediaCode"].ToString().Equals(mediaInfoCode))
                        {
                            cm.Position = rowIndex;
                            break;
                        }
                    }

                    rowIndex++;
                    grdExMediaList.EnsureVisible();
                }
            }
            catch (FrameException fe)
            {
                FrameSystem.showMsgForm("Ű������", new string[] { fe.ErrCode, fe.ResultMsg });
            }
            catch (Exception ex)
            {
                FrameSystem.showMsgForm("Ű������", new string[] { "", ex.Message });
            }
        }


        /// <summary>
        /// ��ü������ ����
        /// </summary>
        private void SaveMediaInfoDetail()
        {
            StatusMessage("��ü ������ �����մϴ�.");

            if (ebMediaName.Text.Trim().Length == 0)
            {
                MessageBox.Show("��ü�̸��� �Էµ��� �ʾҽ��ϴ�.", "��ü ����",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                ebMediaName.Focus();
                return;
            }

            if (ebCharger.Text.Trim().Length == 0)
            {
                MessageBox.Show("����ڸ��� �Էµ��� �ʾҽ��ϴ�.", "��ü ����",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                ebCharger.Focus();
                return;
            }

            try
            {
                // �����͸𵨿� ������ ������ ��Ʈ�Ѵ�.
                mediasModel.MediaCode = mediaInfoCode;
                mediasModel.MediaName = ebMediaName.Text.Trim();
                mediasModel.Charger = ebCharger.Text;
                mediasModel.Tell = ebTell.Text;
                mediasModel.Email = ebEmail.Text;
                //��뿩��
                if (rbUseYn_Y.Checked)
                {
                    mediasModel.UseYn = "Y";
                }
                else
                {
                    mediasModel.UseYn = "N";
                }

                // ��ü ������ ���� ���񽺸� ȣ���Ѵ�.
                if (IsAdding)
                {
                    new MediaInfoManager(systemModel, commonModel).SetUserAdd(mediasModel);
                    StatusMessage("��ü ������ �߰��Ǿ����ϴ�.");
                    IsAdding = false;
                    ResetUserDetailText();
                }
                else
                {
                    new MediaInfoManager(systemModel, commonModel).SetUserUpdate(mediasModel);
                    StatusMessage("��ü ������ ����Ǿ����ϴ�.");
                }

                DisableButton();
                SearchUsers();
                InitButton();
            }
            catch (FrameException fe)
            {
                FrameSystem.showMsgForm("��ü���� �������", new string[] { fe.ErrCode, fe.ResultMsg });
            }
            catch (Exception ex)
            {
                FrameSystem.showMsgForm("��ü���� �������", new string[] { "", ex.Message });
            }
        }

        /// <summary>
        /// ��ü���� ����
        /// </summary>
        private void DeleteUser()
        {
            StatusMessage("��ü ������ �����մϴ�.");

            if (ebMediaName.Text.Trim().Length == 0)
            {
                MessageBox.Show("������ ��ü ������ �����ϴ�.", "��ü ����",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            DialogResult result = MessageBox.Show("�ش� ��ü ������ ���� �Ͻðڽ��ϱ�?", "��ü ����",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question,
                MessageBoxDefaultButton.Button2);

            if (result == DialogResult.No) return;

            try
            {
                // �����͸𵨿� ������ ������ ��Ʈ�Ѵ�.
                mediasModel.MediaCode = mediaInfoCode.Trim();

                // ��ü ������ ���� ���񽺸� ȣ���Ѵ�.
                new MediaInfoManager(systemModel, commonModel).SetUserDelete(mediasModel);

                ResetUserDetailText();
                StatusMessage("��ü ������ �����Ǿ����ϴ�.");

                DisableButton();
                SearchUsers();
                InitButton();
            }
            catch (FrameException fe)
            {
                FrameSystem.showMsgForm("��ü���� ��������", new string[] { fe.ErrCode, fe.ResultMsg });
            }
            catch (Exception ex)
            {
                FrameSystem.showMsgForm("��ü���� ��������", new string[] { "", ex.Message });
            }
        }

        /// <summary>
        /// ��ü �������� ��Ʈ
        /// </summary>
        private void SetUserDetailText()
        {
            int curRow = cm.Position;

            if (curRow >= 0)
            {
                mediaInfoCode = dt.Rows[curRow]["MediaCode"].ToString();
                ebMediaName.Text = dt.Rows[curRow]["MediaName"].ToString();
                ebCharger.Text = dt.Rows[curRow]["Charger"].ToString();
                ebEmail.Text = dt.Rows[curRow]["Email"].ToString();
                ebTell.Text = dt.Rows[curRow]["Tell"].ToString();
                ebRegDt.Text = dt.Rows[curRow]["RegDt"].ToString();
                ebModDt.Text = dt.Rows[curRow]["ModDt"].ToString();
                string UseYn = dt.Rows[curRow]["UseYn"].ToString();

                if (UseYn.Equals("Y"))
                {
                    rbUseYn_Y.Checked = true;
                    rbUseYn_N.Checked = false;
                }
                else
                {
                    rbUseYn_Y.Checked = false;
                    rbUseYn_N.Checked = true;
                }

                IsAdding = false;
                ResetTextReadonly();
            }

            StatusMessage("�غ�");
        }

        private void ResetUserDetailText()
        {
            ebMediaName.Text = "";
            ebCharger.Text = "";
            ebEmail.Text = "";
            ebTell.Text = "";
            rbUseYn_Y.Checked = true;
            rbUseYn_N.Checked = false;
            ebRegDt.Text = "";
            ebModDt.Text = "";
        }

        /// <summary>
        /// ������ ReadOnly
        /// </summary>
        private void SetTextReadonly()
        {
            ebMediaName.ReadOnly = true;
            ebCharger.ReadOnly = true;
            ebEmail.ReadOnly = true;
            ebTell.ReadOnly = true;
            rbUseYn_Y.Enabled = false;
            rbUseYn_N.Enabled = false;

            ebMediaName.BackColor = Color.WhiteSmoke;
            ebCharger.BackColor = Color.WhiteSmoke;
            ebEmail.BackColor = Color.WhiteSmoke;
            ebTell.BackColor = Color.WhiteSmoke;

        }

        /// <summary>
        /// ������ ����������
        /// </summary>
        private void ResetTextReadonly()
        {
            ebMediaName.ReadOnly = false;
            ebCharger.ReadOnly = false;
            ebEmail.ReadOnly = false;
            ebTell.ReadOnly = false;

            ebMediaName.BackColor = Color.White;
            ebCharger.BackColor = Color.White;
            ebEmail.BackColor = Color.White;
            ebTell.BackColor = Color.White;

            // ����ڱ����� ���� �Ǵ� ���������� ��츸 ��뷹��, ����ڱ���, ��뿩�θ� ������ �� �ִ�.
            if (commonModel.UserClass.Equals("10") || commonModel.UserClass.Equals("20"))
            {
                rbUseYn_Y.Enabled = true;
                rbUseYn_N.Enabled = true;
            }
            int curRow = cm.Position;
            if (curRow >= 0)
            {//��ü�ڵ尡 '1'�� ��쿡�� ��뿩�θ� ���� �� �� ����.
                if (dt.Rows[curRow]["MediaCode"].ToString().Equals("1"))
                {
                    rbUseYn_Y.Enabled = false;
                    rbUseYn_N.Enabled = false;
                }
            }

            // �ű��ۼ��̸� ���̵���� ���Ⱑ��
            if (IsAdding)
            {
                ebMediaName.ReadOnly = false;
                ebMediaName.BackColor = Color.White;
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
    }
}