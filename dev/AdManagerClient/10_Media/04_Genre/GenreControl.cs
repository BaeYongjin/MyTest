// ===============================================================================
// UserInfoControl for Charites Project
//
// UserInfoControl.cs
//
// ������������� ������� �����մϴ�. 
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

using WinFramework.Misc;
using WinFramework.Base;
using WinFramework.Data;

using AdManagerModel;

namespace AdManagerClient
{
	/// <summary>
	/// ����ڰ��� ��Ʈ��
	/// </summary>
	public class GenreControl : System.Windows.Forms.UserControl, IUserControl
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
		GenreModel genreModel  = new GenreModel();	// �����������

		// ȭ��ó���� ����
		bool IsNewSearchKey		  = true;					// �˻����Է� ����
		CurrencyManager cm        = null;					// ������ �׸����� ���濡 ���� �����ͼ� ������ ���Ͽ�			
		DataTable       dt        = null;

        bool IsSearching = false; // ��ȸ�� ��ȭ���� ������Ʈ �Ǵ� ���� ���� �ϱ����� 2011.11.29 JH.Park 
		bool IsAdding             = false;
		bool canRead			  = false;
		bool canUpdate			  = false;
		bool canCreate            = false;
		bool canDelete            = false;
		private string        mediaCode = null;
		private string        genreCode = null;

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
		private Janus.Windows.GridEX.EditControls.EditBox ebUserName;
		private System.Windows.Forms.Label lbGenreName;
		private System.Windows.Forms.Label lbMediaName;
		private Janus.Windows.GridEX.EditControls.EditBox ebMediaName;
		private Janus.Windows.EditControls.UIComboBox cbMediaLevel;
		private Janus.Windows.GridEX.EditControls.EditBox ebGenreName;
		private System.Data.DataView dvGenre;
		private AdManagerClient.GenreDs genreDs;
		private Janus.Windows.GridEX.EditControls.EditBox ebModDt;
		private System.Windows.Forms.Label lbModDt;
		private Janus.Windows.GridEX.GridEX grdExGenreList;
		private Janus.Windows.EditControls.UIButton btnDelete;
		private Janus.Windows.EditControls.UIButton btnAdd;
		private Janus.Windows.EditControls.UIButton btnSave;		
		private System.ComponentModel.IContainer components;

		public GenreControl()
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
            Janus.Windows.GridEX.GridEXLayout grdExGenreList_DesignTimeLayout = new Janus.Windows.GridEX.GridEXLayout();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GenreControl));
            this.uiPM = new Janus.Windows.UI.Dock.UIPanelManager(this.components);
            this.uiPanelUsers = new Janus.Windows.UI.Dock.UIPanelGroup();
            this.uiPanelUsersSearch = new Janus.Windows.UI.Dock.UIPanel();
            this.uiPanelUsersSearchContainer = new Janus.Windows.UI.Dock.UIPanelInnerContainer();
            this.pnlSearch = new System.Windows.Forms.Panel();
            this.cbMediaLevel = new Janus.Windows.EditControls.UIComboBox();
            this.ebSearchKey = new Janus.Windows.GridEX.EditControls.EditBox();
            this.btnSearch = new Janus.Windows.EditControls.UIButton();
            this.uiPanelUserList = new Janus.Windows.UI.Dock.UIPanel();
            this.uiPanelUserListContainer = new Janus.Windows.UI.Dock.UIPanelInnerContainer();
            this.grdExGenreList = new Janus.Windows.GridEX.GridEX();
            this.dvGenre = new System.Data.DataView();
            this.genreDs = new AdManagerClient.GenreDs();
            this.uiPanelUserDetail = new Janus.Windows.UI.Dock.UIPanel();
            this.uiPanelUserDetailContainer = new Janus.Windows.UI.Dock.UIPanelInnerContainer();
            this.pnlUserDetail = new System.Windows.Forms.Panel();
            this.btnDelete = new Janus.Windows.EditControls.UIButton();
            this.btnAdd = new Janus.Windows.EditControls.UIButton();
            this.btnSave = new Janus.Windows.EditControls.UIButton();
            this.ebModDt = new Janus.Windows.GridEX.EditControls.EditBox();
            this.ebGenreName = new Janus.Windows.GridEX.EditControls.EditBox();
            this.ebMediaName = new Janus.Windows.GridEX.EditControls.EditBox();
            this.lbGenreName = new System.Windows.Forms.Label();
            this.lbMediaName = new System.Windows.Forms.Label();
            this.lbModDt = new System.Windows.Forms.Label();
            this.ebUserName = new Janus.Windows.GridEX.EditControls.EditBox();
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
            ((System.ComponentModel.ISupportInitialize)(this.grdExGenreList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dvGenre)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.genreDs)).BeginInit();
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
            this.uiPM.PanelPadding.Bottom = 0;
            this.uiPM.PanelPadding.Left = 0;
            this.uiPM.PanelPadding.Right = 0;
            this.uiPM.PanelPadding.Top = 0;
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
            this.uiPM.AddDockPanelInfo(new System.Guid("b162c53a-7940-47cd-a0ed-9762c59c50d8"), Janus.Windows.UI.Dock.PanelGroupStyle.HorizontalTiles, Janus.Windows.UI.Dock.PanelDockStyle.Fill, true, new System.Drawing.Size(1010, 677), true);
            this.uiPM.AddDockPanelInfo(new System.Guid("afb3b7d0-aa79-443c-a1e8-3baba38f7a4d"), new System.Guid("b162c53a-7940-47cd-a0ed-9762c59c50d8"), 42, true);
            this.uiPM.AddDockPanelInfo(new System.Guid("8e3140b9-bfcb-43af-b84e-2ee6cddf51dc"), new System.Guid("b162c53a-7940-47cd-a0ed-9762c59c50d8"), 457, true);
            this.uiPM.AddDockPanelInfo(new System.Guid("b2973a57-d3e1-4d80-9387-7a6dc3332542"), new System.Guid("b162c53a-7940-47cd-a0ed-9762c59c50d8"), 122, true);
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
            this.uiPanelUsers.Location = new System.Drawing.Point(0, 0);
            this.uiPanelUsers.Name = "uiPanelUsers";
            this.uiPanelUsers.Size = new System.Drawing.Size(1010, 677);
            this.uiPanelUsers.TabIndex = 0;
            this.uiPanelUsers.Text = "�帣����";
            // 
            // uiPanelUsersSearch
            // 
            this.uiPanelUsersSearch.AllowResize = Janus.Windows.UI.InheritableBoolean.False;
            this.uiPanelUsersSearch.CaptionVisible = Janus.Windows.UI.InheritableBoolean.False;
            this.uiPanelUsersSearch.CloseButtonVisible = Janus.Windows.UI.InheritableBoolean.False;
            this.uiPanelUsersSearch.InnerContainer = this.uiPanelUsersSearchContainer;
            this.uiPanelUsersSearch.Location = new System.Drawing.Point(0, 22);
            this.uiPanelUsersSearch.Name = "uiPanelUsersSearch";
            this.uiPanelUsersSearch.Size = new System.Drawing.Size(1010, 43);
            this.uiPanelUsersSearch.TabIndex = 0;
            this.uiPanelUsersSearch.Text = "�˻�";
            // 
            // uiPanelUsersSearchContainer
            // 
            this.uiPanelUsersSearchContainer.Controls.Add(this.pnlSearch);
            this.uiPanelUsersSearchContainer.Location = new System.Drawing.Point(1, 1);
            this.uiPanelUsersSearchContainer.Name = "uiPanelUsersSearchContainer";
            this.uiPanelUsersSearchContainer.Size = new System.Drawing.Size(1008, 41);
            this.uiPanelUsersSearchContainer.TabIndex = 0;
            // 
            // pnlSearch
            // 
            this.pnlSearch.BackColor = System.Drawing.SystemColors.Window;
            this.pnlSearch.Controls.Add(this.cbMediaLevel);
            this.pnlSearch.Controls.Add(this.ebSearchKey);
            this.pnlSearch.Controls.Add(this.btnSearch);
            this.pnlSearch.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlSearch.Location = new System.Drawing.Point(0, 0);
            this.pnlSearch.Name = "pnlSearch";
            this.pnlSearch.Size = new System.Drawing.Size(1008, 41);
            this.pnlSearch.TabIndex = 0;
            // 
            // cbMediaLevel
            // 
            this.cbMediaLevel.ComboStyle = Janus.Windows.EditControls.ComboStyle.DropDownList;
            this.cbMediaLevel.Font = new System.Drawing.Font("����ü", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.cbMediaLevel.Location = new System.Drawing.Point(8, 8);
            this.cbMediaLevel.Name = "cbMediaLevel";
            this.cbMediaLevel.Size = new System.Drawing.Size(160, 21);
            this.cbMediaLevel.TabIndex = 1;
            this.cbMediaLevel.Text = "��ü����";
            this.cbMediaLevel.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            // 
            // ebSearchKey
            // 
            this.ebSearchKey.Location = new System.Drawing.Point(176, 8);
            this.ebSearchKey.Name = "ebSearchKey";
            this.ebSearchKey.Size = new System.Drawing.Size(208, 21);
            this.ebSearchKey.TabIndex = 2;
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
            this.btnSearch.Location = new System.Drawing.Point(893, 8);
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
            this.uiPanelUserList.Size = new System.Drawing.Size(1010, 477);
            this.uiPanelUserList.TabIndex = 0;
            this.uiPanelUserList.TabStop = false;
            this.uiPanelUserList.Text = "�帣";
            // 
            // uiPanelUserListContainer
            // 
            this.uiPanelUserListContainer.BackColor = System.Drawing.SystemColors.Window;
            this.uiPanelUserListContainer.Controls.Add(this.grdExGenreList);
            this.uiPanelUserListContainer.Location = new System.Drawing.Point(1, 23);
            this.uiPanelUserListContainer.Name = "uiPanelUserListContainer";
            this.uiPanelUserListContainer.Size = new System.Drawing.Size(1008, 453);
            this.uiPanelUserListContainer.TabIndex = 0;
            // 
            // grdExGenreList
            // 
            this.grdExGenreList.AllowEdit = Janus.Windows.GridEX.InheritableBoolean.False;
            this.grdExGenreList.AlternatingColors = true;
            this.grdExGenreList.AlternatingRowFormatStyle.BackgroundImageDrawMode = Janus.Windows.GridEX.BackgroundImageDrawMode.Tile;
            this.grdExGenreList.BorderStyle = Janus.Windows.GridEX.BorderStyle.None;
            this.grdExGenreList.DataSource = this.dvGenre;
            grdExGenreList_DesignTimeLayout.LayoutString = resources.GetString("grdExGenreList_DesignTimeLayout.LayoutString");
            this.grdExGenreList.DesignTimeLayout = grdExGenreList_DesignTimeLayout;
            this.grdExGenreList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdExGenreList.EditorsControlStyle.ButtonAppearance = Janus.Windows.GridEX.ButtonAppearance.Regular;
            this.grdExGenreList.EmptyRows = true;
            this.grdExGenreList.FocusCellFormatStyle.BackColor = System.Drawing.SystemColors.Highlight;
            this.grdExGenreList.FocusCellFormatStyle.ForeColor = System.Drawing.Color.White;
            this.grdExGenreList.FocusStyle = Janus.Windows.GridEX.FocusStyle.None;
            this.grdExGenreList.Font = new System.Drawing.Font("����ü", 9F);
            this.grdExGenreList.FrozenColumns = 2;
            this.grdExGenreList.GridLineColor = System.Drawing.Color.Silver;
            this.grdExGenreList.GridLines = Janus.Windows.GridEX.GridLines.Vertical;
            this.grdExGenreList.GridLineStyle = Janus.Windows.GridEX.GridLineStyle.Solid;
            this.grdExGenreList.GroupByBoxVisible = false;
            this.grdExGenreList.HeaderFormatStyle.TextAlignment = Janus.Windows.GridEX.TextAlignment.Center;
            this.grdExGenreList.HideSelection = Janus.Windows.GridEX.HideSelection.HighlightInactive;
            this.grdExGenreList.Location = new System.Drawing.Point(0, 0);
            this.grdExGenreList.Name = "grdExGenreList";
            this.grdExGenreList.ScrollBars = Janus.Windows.GridEX.ScrollBars.Vertical;
            this.grdExGenreList.SelectedInactiveFormatStyle.BackColor = System.Drawing.Color.Gold;
            this.grdExGenreList.Size = new System.Drawing.Size(1008, 453);
            this.grdExGenreList.TabIndex = 4;
            this.grdExGenreList.TabKeyBehavior = Janus.Windows.GridEX.TabKeyBehavior.ControlNavigation;
            this.grdExGenreList.ThemedAreas = ((Janus.Windows.GridEX.ThemedArea)(((((((((Janus.Windows.GridEX.ThemedArea.ScrollBars | Janus.Windows.GridEX.ThemedArea.EditControls) 
            | Janus.Windows.GridEX.ThemedArea.Headers) 
            | Janus.Windows.GridEX.ThemedArea.GroupByBox) 
            | Janus.Windows.GridEX.ThemedArea.GroupRows) 
            | Janus.Windows.GridEX.ThemedArea.ControlBorder) 
            | Janus.Windows.GridEX.ThemedArea.Cards) 
            | Janus.Windows.GridEX.ThemedArea.Gridlines) 
            | Janus.Windows.GridEX.ThemedArea.CheckBoxes)));
            this.grdExGenreList.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            this.grdExGenreList.Enter += new System.EventHandler(this.OnGrdRowChanged);
            // 
            // dvGenre
            // 
            this.dvGenre.Table = this.genreDs.Genres;
            // 
            // genreDs
            // 
            this.genreDs.DataSetName = "GenreDs";
            this.genreDs.Locale = new System.Globalization.CultureInfo("en-US");
            this.genreDs.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // uiPanelUserDetail
            // 
            this.uiPanelUserDetail.AllowResize = Janus.Windows.UI.InheritableBoolean.False;
            this.uiPanelUserDetail.BackColor = System.Drawing.SystemColors.Window;
            this.uiPanelUserDetail.CaptionStyle = Janus.Windows.UI.Dock.PanelCaptionStyle.Dark;
            this.uiPanelUserDetail.InnerContainer = this.uiPanelUserDetailContainer;
            this.uiPanelUserDetail.Location = new System.Drawing.Point(0, 550);
            this.uiPanelUserDetail.Name = "uiPanelUserDetail";
            this.uiPanelUserDetail.Size = new System.Drawing.Size(1010, 127);
            this.uiPanelUserDetail.TabIndex = 0;
            this.uiPanelUserDetail.TabStop = false;
            this.uiPanelUserDetail.Text = "������";
            // 
            // uiPanelUserDetailContainer
            // 
            this.uiPanelUserDetailContainer.Controls.Add(this.pnlUserDetail);
            this.uiPanelUserDetailContainer.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.uiPanelUserDetailContainer.Location = new System.Drawing.Point(1, 23);
            this.uiPanelUserDetailContainer.Name = "uiPanelUserDetailContainer";
            this.uiPanelUserDetailContainer.Size = new System.Drawing.Size(1008, 103);
            this.uiPanelUserDetailContainer.TabIndex = 0;
            // 
            // pnlUserDetail
            // 
            this.pnlUserDetail.BackColor = System.Drawing.SystemColors.Window;
            this.pnlUserDetail.Controls.Add(this.btnDelete);
            this.pnlUserDetail.Controls.Add(this.btnAdd);
            this.pnlUserDetail.Controls.Add(this.btnSave);
            this.pnlUserDetail.Controls.Add(this.ebModDt);
            this.pnlUserDetail.Controls.Add(this.ebGenreName);
            this.pnlUserDetail.Controls.Add(this.ebMediaName);
            this.pnlUserDetail.Controls.Add(this.lbGenreName);
            this.pnlUserDetail.Controls.Add(this.lbMediaName);
            this.pnlUserDetail.Controls.Add(this.lbModDt);
            this.pnlUserDetail.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlUserDetail.Location = new System.Drawing.Point(0, 0);
            this.pnlUserDetail.Name = "pnlUserDetail";
            this.pnlUserDetail.Size = new System.Drawing.Size(1008, 103);
            this.pnlUserDetail.TabIndex = 0;
            // 
            // btnDelete
            // 
            this.btnDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnDelete.Enabled = false;
            this.btnDelete.Location = new System.Drawing.Point(126, 61);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(104, 24);
            this.btnDelete.TabIndex = 7;
            this.btnDelete.Text = "�� ��";
            this.btnDelete.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnAdd
            // 
            this.btnAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnAdd.Enabled = false;
            this.btnAdd.Location = new System.Drawing.Point(238, 61);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(104, 24);
            this.btnAdd.TabIndex = 8;
            this.btnAdd.Text = "�� ��";
            this.btnAdd.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnSave.Enabled = false;
            this.btnSave.Location = new System.Drawing.Point(14, 61);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(104, 24);
            this.btnSave.TabIndex = 6;
            this.btnSave.Text = "�� ��";
            this.btnSave.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // ebModDt
            // 
            this.ebModDt.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ebModDt.Font = new System.Drawing.Font("����ü", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.ebModDt.Location = new System.Drawing.Point(576, 8);
            this.ebModDt.Name = "ebModDt";
            this.ebModDt.ReadOnly = true;
            this.ebModDt.Size = new System.Drawing.Size(190, 21);
            this.ebModDt.TabIndex = 0;
            this.ebModDt.TabStop = false;
            this.ebModDt.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
            this.ebModDt.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            // 
            // ebGenreName
            // 
            this.ebGenreName.Font = new System.Drawing.Font("����ü", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.ebGenreName.Location = new System.Drawing.Point(312, 8);
            this.ebGenreName.MaxLength = 40;
            this.ebGenreName.Name = "ebGenreName";
            this.ebGenreName.Size = new System.Drawing.Size(190, 21);
            this.ebGenreName.TabIndex = 5;
            this.ebGenreName.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
            this.ebGenreName.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            // 
            // ebMediaName
            // 
            this.ebMediaName.Location = new System.Drawing.Point(56, 8);
            this.ebMediaName.MaxLength = 40;
            this.ebMediaName.Name = "ebMediaName";
            this.ebMediaName.Size = new System.Drawing.Size(190, 21);
            this.ebMediaName.TabIndex = 0;
            this.ebMediaName.TabStop = false;
            this.ebMediaName.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
            this.ebMediaName.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            // 
            // lbGenreName
            // 
            this.lbGenreName.Font = new System.Drawing.Font("����ü", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lbGenreName.Location = new System.Drawing.Point(264, 12);
            this.lbGenreName.Name = "lbGenreName";
            this.lbGenreName.Size = new System.Drawing.Size(80, 16);
            this.lbGenreName.TabIndex = 0;
            this.lbGenreName.Text = "�帣��";
            // 
            // lbMediaName
            // 
            this.lbMediaName.BackColor = System.Drawing.SystemColors.Window;
            this.lbMediaName.Font = new System.Drawing.Font("����ü", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lbMediaName.Location = new System.Drawing.Point(8, 12);
            this.lbMediaName.Name = "lbMediaName";
            this.lbMediaName.Size = new System.Drawing.Size(96, 16);
            this.lbMediaName.TabIndex = 0;
            this.lbMediaName.Text = "��ü��";
            // 
            // lbModDt
            // 
            this.lbModDt.Font = new System.Drawing.Font("����ü", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lbModDt.Location = new System.Drawing.Point(520, 12);
            this.lbModDt.Name = "lbModDt";
            this.lbModDt.Size = new System.Drawing.Size(72, 16);
            this.lbModDt.TabIndex = 0;
            this.lbModDt.Text = "�����Ͻ�";
            // 
            // ebUserName
            // 
            this.ebUserName.Location = new System.Drawing.Point(0, 0);
            this.ebUserName.Name = "ebUserName";
            this.ebUserName.Size = new System.Drawing.Size(0, 21);
            this.ebUserName.TabIndex = 0;
            this.ebUserName.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
            // 
            // uiButton1
            // 
            this.uiButton1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.uiButton1.Location = new System.Drawing.Point(136, 8);
            this.uiButton1.Name = "uiButton1";
            this.uiButton1.Size = new System.Drawing.Size(112, 24);
            this.uiButton1.TabIndex = 0;
            this.uiButton1.Text = "�� ��";
            this.uiButton1.VisualStyle = Janus.Windows.UI.VisualStyle.Office2003;
            // 
            // uiButton2
            // 
            this.uiButton2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.uiButton2.Location = new System.Drawing.Point(8, 8);
            this.uiButton2.Name = "uiButton2";
            this.uiButton2.Size = new System.Drawing.Size(120, 24);
            this.uiButton2.TabIndex = 0;
            this.uiButton2.Text = "�� ��";
            this.uiButton2.VisualStyle = Janus.Windows.UI.VisualStyle.Office2003;
            // 
            // GenreControl
            // 
            this.Controls.Add(this.uiPanelUsers);
            this.Name = "GenreControl";
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
            ((System.ComponentModel.ISupportInitialize)(this.grdExGenreList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dvGenre)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.genreDs)).EndInit();
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
			dt = ((DataView)grdExGenreList.DataSource).Table;  
			cm = (CurrencyManager) this.BindingContext[grdExGenreList.DataSource]; 
			cm.PositionChanged += new System.EventHandler(OnGrdRowChanged); 

			// ��Ʈ�� �ʱ�ȭ
			InitControl();
		}

		#endregion

		#region ��Ʈ�� �ʱ�ȭ
		private void InitControl()
		{
			ProgressStart();
			InitCombo();
			InitCombo_Level();
			// ��ȸ���� �˻�
			if(menu.CanRead(MenuCode))
			{
				canRead = true;
				SearchGenre();
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
				SetTextReadonly();
			}			
			InitButton();
			SetTextReadonly();
			ProgressStop();
		}

		private void InitCombo()
		{
			// �ڵ忡�� ���ȷ����� ��ȸ�Ѵ�.
			MediaCodeModel mediacodeModel = new MediaCodeModel();
			new MediaCodeManager(systemModel, commonModel).GetMediaCodeList(mediacodeModel);
			
			if (mediacodeModel.ResultCD.Equals("0000"))
			{
				// �����ͼ¿� ����
				Utility.SetDataTable(genreDs.Medias, mediacodeModel.MediaCodeDataSet);				
			}

			// ����ȸ �޺�
			// �������� �޺��� Dataset�� �����ͼҽ��� ������.

			// �˻������� �޺�
			this.cbMediaLevel.Items.Clear();
			
			// �޺��ڽ��� ��Ʈ�� �ڵ����� ���� Item�迭�� ����
			Janus.Windows.EditControls.UIComboBoxItem[] comboItems = new Janus.Windows.EditControls.UIComboBoxItem[mediacodeModel.ResultCnt + 1];

			comboItems[0] = new Janus.Windows.EditControls.UIComboBoxItem("��ü����","00");
			
			for(int i=0;i<mediacodeModel.ResultCnt;i++)
			{
				DataRow row = genreDs.Medias.Rows[i];

				string val = row["MediaCode"].ToString();
				string txt = row["MediaName"].ToString();
				comboItems[i+1] = new Janus.Windows.EditControls.UIComboBoxItem(txt,val);
			}
			// �޺��� ��Ʈ
			this.cbMediaLevel.Items.AddRange(comboItems);
			this.cbMediaLevel.SelectedIndex = 0;

			Application.DoEvents();
		}

		private void InitCombo_Level()
		{			
			if(commonModel.UserLevel=="20")
			{
				// �޺��Ƚ�						
				cbMediaLevel.SelectedValue = commonModel.MediaCode;			
				cbMediaLevel.ReadOnly = true;				
			}
			else
			{				
				for(int i=0;i < genreDs.Medias.Rows.Count;i++)
				{
					DataRow row = genreDs.Medias.Rows[i];					
					if(row["MediaCode"].ToString().Equals(FrameSystem._HANATV.ToString()))
					{
						cbMediaLevel.SelectedValue = FrameSystem._HANATV; // �ϳ�TV�� �⺻������ �Ѵ�.	 		
						break;															
					}
					else
					{
						cbMediaLevel.SelectedValue="00";
					}
				}	
			}
			Application.DoEvents();
		}

		private void InitButton()
		{
			if(canRead)   btnSearch.Enabled = true;
			if(canCreate) btnAdd.Enabled    = true;

			if(ebMediaName.Text.Trim().Length > 0) 
			{
				if(canDelete) btnDelete.Enabled = true;
				if(canUpdate) btnSave.Enabled   = true;
			}
			Application.DoEvents();
		}

		private void DisableButton()
		{
			btnSearch.Enabled = false;
			btnAdd.Enabled    = false;
			btnSave.Enabled   = false;
			btnDelete.Enabled = false;
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
                SetGenreDetailText();
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
			SearchGenre();
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
			btnAdd.Enabled    = false;
			btnDelete.Enabled = false;
			btnSave.Enabled   = true;

			IsAdding = true;

			ResetTextReadonly();
			ResetGenreDetailText();

			ebGenreName.Focus();
		}

		/// <summary>
		/// �����ư Ŭ��
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnSave_Click(object sender, System.EventArgs e)
		{			
			SaveUserDetail();			
		}

		/// <summary>
		/// ������ư Ŭ��
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnDelete_Click(object sender, System.EventArgs e)
		{			
			DeleteGenre();			
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
				SearchGenre();
			}
		}

		#endregion

		#region ó���޼ҵ�

		/// <summary>
		/// ����ڸ�� ��ȸ
		/// </summary>
		private void SearchGenre()
		{
            IsSearching = true;
			StatusMessage("�帣 ������ ��ȸ�մϴ�.");

//			if(cbMediaLevel.SelectedItem.Value.Equals("00")) 
//			{
//				MessageBox.Show("��ü�� �����Ͽ� �ֽñ� �ٶ��ϴ�.","�帣 ��ȸ", 
//					MessageBoxButtons.OK, MessageBoxIcon.Information );
//				return;
//			}

            try
            {
                genreModel.Init();
                // �����͸𵨿� ������ ������ ��Ʈ�Ѵ�.
                if (IsNewSearchKey)
                {
                    genreModel.SearchKey = "";
                }
                else
                {
                    genreModel.SearchKey = ebSearchKey.Text;
                }

                genreModel.SearchGenreLevel = cbMediaLevel.SelectedItem.Value.ToString();

                ResetGenreDetailText();

                // ����ڸ����ȸ ���񽺸� ȣ���Ѵ�.
                new GenreManager(systemModel, commonModel).GetGenreList(genreModel);

                if (genreModel.ResultCD.Equals("0000"))
                {
                    Utility.SetDataTable(genreDs.Genres, genreModel.UserDataSet);
                    StatusMessage(genreModel.ResultCnt + "���� �帣 ������ ��ȸ�Ǿ����ϴ�.");
                    if (canUpdate)
                    {
                        AddSchChoice();
                    }
                    SetGenreDetailText();
                }
            }
            catch (FrameException fe)
            {
                FrameSystem.showMsgForm("�帣��ȸ����", new string[] { fe.ErrCode, fe.ResultMsg });
            }
            catch (Exception ex)
            {
                FrameSystem.showMsgForm("�帣��ȸ����", new string[] { "", ex.Message });
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
			StatusMessage("Ű��");		

			try
			{
				int rowIndex = 0;
				if ( genreDs.Tables["Genres"].Rows.Count < 1 ) return;
              
				foreach (DataRow row in genreDs.Tables["Genres"].Rows)
				{					
					if(IsAdding)
					{
						cm.Position = 0;
						mediaCode = null;
						genreCode = null;
					}
					else
					{						
						if(row["MediaCode"].ToString().Equals(mediaCode) && row["GenreCode"].ToString().Equals(genreCode))
						{					
							cm.Position = rowIndex;
							break;								
						}
					}

					rowIndex++;
					grdExGenreList.EnsureVisible();
				}
			}
			catch(FrameException fe)
			{
				FrameSystem.showMsgForm("Ű������", new string[] {fe.ErrCode, fe.ResultMsg});
			}
			catch(Exception ex)
			{
				FrameSystem.showMsgForm("Ű������",new string[] {"",ex.Message});
			}			
		}

		/// <summary>
		/// ����ڻ����� ����
		/// </summary>
		private void SaveUserDetail()
		{
			StatusMessage("�帣 ������ �����մϴ�.");

			if(ebGenreName.Text.Trim().Length == 0) 
			{
				MessageBox.Show("�帣���� �Էµ��� �ʾҽ��ϴ�.","�帣 ����", 
					MessageBoxButtons.OK, MessageBoxIcon.Information );
					ebGenreName.Focus();
				return;
				
			}			

			try
			{     

				// �����͸𵨿� ������ ������ ��Ʈ�Ѵ�.
				genreModel.MediaCode       = cbMediaLevel.SelectedValue.ToString();
				genreModel.GenreCode       = genreCode;
				//genreModel.MediaName = Security.Encrypt(ebUserPassword.Text);
				genreModel.GenreName = ebGenreName.Text.Trim();	
	
				
				// ����� ������ ���� ���񽺸� ȣ���Ѵ�.
				if (IsAdding)
				{
					new GenreManager(systemModel,commonModel).SetGenreAdd(genreModel);
					StatusMessage("�帣 ������ �߰��Ǿ����ϴ�.");
					IsAdding = false;
					ResetGenreDetailText();
				}
				else
				{
					new GenreManager(systemModel,commonModel).SetGenreUpdate(genreModel);
				}
				
				DisableButton();
				SearchGenre();
				InitButton();
			}
			catch(FrameException fe)
			{
				FrameSystem.showMsgForm("�帣���� �������", new string[] {fe.ErrCode, fe.ResultMsg});
			}
			catch(Exception ex)
			{
				FrameSystem.showMsgForm("�帣���� �������",new string[] {"",ex.Message});
			}			
		}

		/// <summary>
		/// ��������� ����
		/// </summary>
		private void DeleteGenre()
		{
			StatusMessage("����� ������ �����մϴ�.");

			if(ebMediaName.Text.Trim().Length == 0) 
			{
				MessageBox.Show("������ �帣 ������ �����ϴ�.","�帣 ����", 
					MessageBoxButtons.OK, MessageBoxIcon.Information );
				return;
			}

			DialogResult result = MessageBox.Show("�ش� �帣 ������ ���� �Ͻðڽ��ϱ�?","�帣 ����",
				MessageBoxButtons.YesNo, MessageBoxIcon.Question,
				MessageBoxDefaultButton.Button2);

			if (result == DialogResult.No) return;

			try
			{
				// �����͸𵨿� ������ ������ ��Ʈ�Ѵ�.
				genreModel.MediaCode       = mediaCode.Trim();
				genreModel.GenreCode       = genreCode.Trim();

				// ����� ������ ���� ���񽺸� ȣ���Ѵ�.
				new GenreManager(systemModel,commonModel).SetGenreDelete(genreModel);
				
				ResetGenreDetailText();				
				StatusMessage("ī�װ� ������ �����Ǿ����ϴ�.");			
				
				DisableButton();
				SearchGenre();
				InitButton();
			}
			catch(FrameException fe)
			{
				FrameSystem.showMsgForm("�帣���� ��������", new string[] {fe.ErrCode, fe.ResultMsg});
			}
			catch(Exception ex)
			{
				FrameSystem.showMsgForm("�帣���� ��������",new string[] {"",ex.Message});
			}			

		}
		
		/// <summary>
		/// ����� �������� ��Ʈ
		/// </summary>
		private void SetGenreDetailText()
		{
			int curRow = cm.Position;

			if(curRow >= 0)
			{
				mediaCode             = dt.Rows[curRow]["MediaCode"].ToString();
				genreCode             = dt.Rows[curRow]["GenreCode"].ToString();			
				ebMediaName.Text           = dt.Rows[curRow]["MediaName"].ToString();
				ebGenreName.Text           = dt.Rows[curRow]["GenreName"].ToString();
				ebModDt.Text           = dt.Rows[curRow]["ModDt"].ToString();

				IsAdding = false;
				ResetTextReadonly();
			}
			StatusMessage("�غ�");
		}

		private void ResetGenreDetailText()
		{
			ebMediaName.Text             = "";		
			ebGenreName.Text           = "";				
		}
		
		/// <summary>
		/// ������ ReadOnly
		/// </summary>
		private void SetTextReadonly()
		{
			ebMediaName.ReadOnly         = true;			
			ebGenreName.ReadOnly       = true;			

			ebMediaName.BackColor        = Color.WhiteSmoke;			
			ebGenreName.BackColor      = Color.WhiteSmoke;			
		}

		/// <summary>
		/// ������ ����������
		/// </summary>
		private void ResetTextReadonly()
		{			
			ebGenreName.ReadOnly       = false;	
			ebGenreName.ReadOnly       = false;			
			ebGenreName.BackColor      = Color.White;
			ebGenreName.BackColor      = Color.White;						
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
	
	}
}
