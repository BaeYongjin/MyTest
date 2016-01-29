// ===============================================================================
// ContentsControl for Charites Project
//
// ContentsControl.cs
//
// 컨텐츠정보관리 컨드롤을 정의합니다. 
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
    /// 컨텐츠관리 컨트롤
    /// </summary>
    public class ContentsControl : System.Windows.Forms.UserControl, IUserControl
    {
        #region 이벤트핸들러
        public event StatusEventHandler 			StatusEvent;			// 상태이벤트 핸들러
		public event ProgressEventHandler 			ProgressEvent;			// 처리중이벤트 핸들러
        #endregion
			
        #region 사용자정의 객체 및 변수

        // 시스템 정보 : 화면공통
        private SystemModel   systemModel   = FrameSystem.oSysModel;
        private CommonModel   commonModel   = FrameSystem.oComModel;
        private Logger        log           = FrameSystem.oLog;
        private MenuPower     menu          = FrameSystem.oMenu;
       
        // 메뉴코드 : 보안이 필요한 화면에 필요함
        public string        menuCode		= "";


        // 사용할 정보모델
        ContentsModel contentsModel  = new ContentsModel();	// 컨텐츠정보모델

        // 화면처리용 변수
        bool IsNewSearchKey		  = true;					// 검색어입력 여부
        CurrencyManager cm        = null;					// 데이터 그리드의 변경에 따른 데이터셋 관리를 위하여			
        DataTable       dt        = null;

        bool IsSearching = false; // 조회중 상세화면이 업데이트 되는 것을 방지 하기위함 2011.11.29 JH.Park
        bool IsAdding             = false;
        bool canRead			  = false;
        bool canUpdate			  = false;
        bool canCreate            = false;
        bool canDelete            = false;

		private string        contentId = null;		

        #endregion

        #region IUserControl 구현
        /// <summary>
        /// 메뉴 코드-보안이 필요한 화면에 필요함
        /// </summary>
        public string MenuCode
        {
            set { this.menuCode = value; }
            get { return this.menuCode; }
        }

        /// <summary>
        /// 부모컨트롤 지정
        /// </summary>
        /// <param name="control"></param>
        public void SetParent(Control control)
        {
            this.Parent = control;
        }
        /// <summary>
        /// DockStype지정
        /// </summary>
        /// <param name="style"></param>
        public void SetDockStyle(DockStyle style)
        {
            this.Dock = style;
        }
        #endregion


        #region 화면 컴포넌트, 생성자, 소멸자
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
        private Janus.Windows.GridEX.EditControls.EditBox ebContentId;
        private Janus.Windows.EditControls.UIButton btnAdd;
        private Janus.Windows.EditControls.UIButton btnSave;
        private Janus.Windows.EditControls.UIButton btnDelete;
        private System.Data.DataView dvContents;
        private Janus.Windows.GridEX.EditControls.EditBox ebTitle;
        private Janus.Windows.GridEX.EditControls.EditBox ebContentState;
        private Janus.Windows.GridEX.EditControls.EditBox ebRate;
        private Janus.Windows.GridEX.EditControls.EditBox ebSubTitle;
        private System.Windows.Forms.Label lbOrgTitle;
        private System.Windows.Forms.Label lbSubTitle;
        private System.Windows.Forms.Label lbTitle;
        private System.Windows.Forms.Label lbContentId;
        private System.Windows.Forms.Label lbContentState;
        private System.Windows.Forms.Label lbRate;
        private Janus.Windows.GridEX.EditControls.EditBox ebOrgTitle;
        private AdManagerClient.ContentsDs contentsDs;
		private Janus.Windows.GridEX.GridEX grdExContentsList;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
        private Janus.Windows.GridEX.EditControls.EditBox ebRegDate;
        private Janus.Windows.GridEX.EditControls.EditBox ebModDt;
        private System.ComponentModel.IContainer components;

        public ContentsControl()
        {
            // 이 호출은 Windows.Forms Form 디자이너에 필요합니다.
            InitializeComponent();
        }

        /// <summary> 
        /// 사용 중인 모든 리소스를 정리합니다.
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

        #region 구성 요소 디자이너에서 생성한 코드
        /// <summary> 
        /// 디자이너 지원에 필요한 메서드입니다. 
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마십시오.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            Janus.Windows.GridEX.GridEXLayout grdExContentsList_DesignTimeLayout = new Janus.Windows.GridEX.GridEXLayout();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ContentsControl));
            this.uiPM = new Janus.Windows.UI.Dock.UIPanelManager(this.components);
            this.uiPanelUsers = new Janus.Windows.UI.Dock.UIPanelGroup();
            this.uiPanelUsersSearch = new Janus.Windows.UI.Dock.UIPanel();
            this.uiPanelUsersSearchContainer = new Janus.Windows.UI.Dock.UIPanelInnerContainer();
            this.pnlSearch = new System.Windows.Forms.Panel();
            this.ebSearchKey = new Janus.Windows.GridEX.EditControls.EditBox();
            this.btnSearch = new Janus.Windows.EditControls.UIButton();
            this.uiPanelUserList = new Janus.Windows.UI.Dock.UIPanel();
            this.uiPanelUserListContainer = new Janus.Windows.UI.Dock.UIPanelInnerContainer();
            this.grdExContentsList = new Janus.Windows.GridEX.GridEX();
            this.dvContents = new System.Data.DataView();
            this.contentsDs = new AdManagerClient.ContentsDs();
            this.uiPanelUserDetail = new Janus.Windows.UI.Dock.UIPanel();
            this.uiPanelUserDetailContainer = new Janus.Windows.UI.Dock.UIPanelInnerContainer();
            this.pnlUserDetail = new System.Windows.Forms.Panel();
            this.ebSubTitle = new Janus.Windows.GridEX.EditControls.EditBox();
            this.ebContentState = new Janus.Windows.GridEX.EditControls.EditBox();
            this.lbOrgTitle = new System.Windows.Forms.Label();
            this.lbSubTitle = new System.Windows.Forms.Label();
            this.ebTitle = new Janus.Windows.GridEX.EditControls.EditBox();
            this.lbTitle = new System.Windows.Forms.Label();
            this.ebContentId = new Janus.Windows.GridEX.EditControls.EditBox();
            this.lbContentId = new System.Windows.Forms.Label();
            this.lbContentState = new System.Windows.Forms.Label();
            this.lbRate = new System.Windows.Forms.Label();
            this.ebRate = new Janus.Windows.GridEX.EditControls.EditBox();
            this.ebOrgTitle = new Janus.Windows.GridEX.EditControls.EditBox();
            this.btnAdd = new Janus.Windows.EditControls.UIButton();
            this.btnSave = new Janus.Windows.EditControls.UIButton();
            this.btnDelete = new Janus.Windows.EditControls.UIButton();
            this.ebRegDate = new Janus.Windows.GridEX.EditControls.EditBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.ebModDt = new Janus.Windows.GridEX.EditControls.EditBox();
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
            ((System.ComponentModel.ISupportInitialize)(this.grdExContentsList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dvContents)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.contentsDs)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelUserDetail)).BeginInit();
            this.uiPanelUserDetail.SuspendLayout();
            this.uiPanelUserDetailContainer.SuspendLayout();
            this.pnlUserDetail.SuspendLayout();
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
            this.uiPM.AddDockPanelInfo(new System.Guid("8e3140b9-bfcb-43af-b84e-2ee6cddf51dc"), new System.Guid("b162c53a-7940-47cd-a0ed-9762c59c50d8"), 415, true);
            this.uiPM.AddDockPanelInfo(new System.Guid("b2973a57-d3e1-4d80-9387-7a6dc3332542"), new System.Guid("b162c53a-7940-47cd-a0ed-9762c59c50d8"), 170, true);
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
            this.uiPanelUsers.Text = "컨텐츠관리";
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
            this.uiPanelUsersSearch.Text = "검색";
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
            this.pnlSearch.Controls.Add(this.ebSearchKey);
            this.pnlSearch.Controls.Add(this.btnSearch);
            this.pnlSearch.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlSearch.Location = new System.Drawing.Point(0, 0);
            this.pnlSearch.Name = "pnlSearch";
            this.pnlSearch.Size = new System.Drawing.Size(1008, 41);
            this.pnlSearch.TabIndex = 0;
            // 
            // ebSearchKey
            // 
            this.ebSearchKey.Location = new System.Drawing.Point(8, 8);
            this.ebSearchKey.Name = "ebSearchKey";
            this.ebSearchKey.Size = new System.Drawing.Size(208, 21);
            this.ebSearchKey.TabIndex = 1;
            this.ebSearchKey.Text = "검색어를 입력하세요";
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
            this.btnSearch.Location = new System.Drawing.Point(895, 8);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(104, 24);
            this.btnSearch.TabIndex = 2;
            this.btnSearch.Text = "조 회";
            this.btnSearch.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // uiPanelUserList
            // 
            this.uiPanelUserList.BackColor = System.Drawing.SystemColors.Window;
            this.uiPanelUserList.CaptionStyle = Janus.Windows.UI.Dock.PanelCaptionStyle.Dark;
            this.uiPanelUserList.InnerContainer = this.uiPanelUserListContainer;
            this.uiPanelUserList.Location = new System.Drawing.Point(0, 69);
            this.uiPanelUserList.MinimumSize = new System.Drawing.Size(-1, 100);
            this.uiPanelUserList.Name = "uiPanelUserList";
            this.uiPanelUserList.Size = new System.Drawing.Size(1010, 429);
            this.uiPanelUserList.TabIndex = 3;
            this.uiPanelUserList.TabStop = false;
            this.uiPanelUserList.Text = "컨텐츠목록";
            // 
            // uiPanelUserListContainer
            // 
            this.uiPanelUserListContainer.BackColor = System.Drawing.SystemColors.Window;
            this.uiPanelUserListContainer.Controls.Add(this.grdExContentsList);
            this.uiPanelUserListContainer.Location = new System.Drawing.Point(1, 23);
            this.uiPanelUserListContainer.Name = "uiPanelUserListContainer";
            this.uiPanelUserListContainer.Size = new System.Drawing.Size(1008, 405);
            this.uiPanelUserListContainer.TabIndex = 0;
            // 
            // grdExContentsList
            // 
            this.grdExContentsList.AllowEdit = Janus.Windows.GridEX.InheritableBoolean.False;
            this.grdExContentsList.AlternatingColors = true;
            this.grdExContentsList.AlternatingRowFormatStyle.BackgroundImageDrawMode = Janus.Windows.GridEX.BackgroundImageDrawMode.Tile;
            this.grdExContentsList.BorderStyle = Janus.Windows.GridEX.BorderStyle.None;
            this.grdExContentsList.DataSource = this.dvContents;
            grdExContentsList_DesignTimeLayout.LayoutString = resources.GetString("grdExContentsList_DesignTimeLayout.LayoutString");
            this.grdExContentsList.DesignTimeLayout = grdExContentsList_DesignTimeLayout;
            this.grdExContentsList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdExContentsList.EditorsControlStyle.ButtonAppearance = Janus.Windows.GridEX.ButtonAppearance.Regular;
            this.grdExContentsList.EmptyRows = true;
            this.grdExContentsList.FocusCellFormatStyle.BackColor = System.Drawing.SystemColors.Highlight;
            this.grdExContentsList.FocusCellFormatStyle.ForeColor = System.Drawing.Color.White;
            this.grdExContentsList.FocusStyle = Janus.Windows.GridEX.FocusStyle.None;
            this.grdExContentsList.Font = new System.Drawing.Font("굴림체", 9F);
            this.grdExContentsList.FrozenColumns = 2;
            this.grdExContentsList.GridLineColor = System.Drawing.Color.Silver;
            this.grdExContentsList.GridLines = Janus.Windows.GridEX.GridLines.Vertical;
            this.grdExContentsList.GridLineStyle = Janus.Windows.GridEX.GridLineStyle.Solid;
            this.grdExContentsList.GroupByBoxVisible = false;
            this.grdExContentsList.HeaderFormatStyle.TextAlignment = Janus.Windows.GridEX.TextAlignment.Center;
            this.grdExContentsList.HideSelection = Janus.Windows.GridEX.HideSelection.HighlightInactive;
            this.grdExContentsList.Location = new System.Drawing.Point(0, 0);
            this.grdExContentsList.Name = "grdExContentsList";
            this.grdExContentsList.ScrollBars = Janus.Windows.GridEX.ScrollBars.Vertical;
            this.grdExContentsList.SelectedInactiveFormatStyle.BackColor = System.Drawing.Color.Gold;
            this.grdExContentsList.Size = new System.Drawing.Size(1008, 405);
            this.grdExContentsList.TabIndex = 4;
            this.grdExContentsList.TabKeyBehavior = Janus.Windows.GridEX.TabKeyBehavior.ControlNavigation;
            this.grdExContentsList.ThemedAreas = ((Janus.Windows.GridEX.ThemedArea)(((((((((Janus.Windows.GridEX.ThemedArea.ScrollBars | Janus.Windows.GridEX.ThemedArea.EditControls) 
            | Janus.Windows.GridEX.ThemedArea.Headers) 
            | Janus.Windows.GridEX.ThemedArea.GroupByBox) 
            | Janus.Windows.GridEX.ThemedArea.GroupRows) 
            | Janus.Windows.GridEX.ThemedArea.ControlBorder) 
            | Janus.Windows.GridEX.ThemedArea.Cards) 
            | Janus.Windows.GridEX.ThemedArea.Gridlines) 
            | Janus.Windows.GridEX.ThemedArea.CheckBoxes)));
            this.grdExContentsList.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            this.grdExContentsList.Enter += new System.EventHandler(this.OnGrdRowChanged);
            // 
            // dvContents
            // 
            this.dvContents.Table = this.contentsDs.Contents;
            // 
            // contentsDs
            // 
            this.contentsDs.DataSetName = "ContentsDs";
            this.contentsDs.Locale = new System.Globalization.CultureInfo("en-US");
            this.contentsDs.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // uiPanelUserDetail
            // 
            this.uiPanelUserDetail.AllowResize = Janus.Windows.UI.InheritableBoolean.False;
            this.uiPanelUserDetail.BackColor = System.Drawing.SystemColors.Window;
            this.uiPanelUserDetail.CaptionStyle = Janus.Windows.UI.Dock.PanelCaptionStyle.Dark;
            this.uiPanelUserDetail.InnerContainer = this.uiPanelUserDetailContainer;
            this.uiPanelUserDetail.Location = new System.Drawing.Point(0, 502);
            this.uiPanelUserDetail.Name = "uiPanelUserDetail";
            this.uiPanelUserDetail.Size = new System.Drawing.Size(1010, 175);
            this.uiPanelUserDetail.TabIndex = 5;
            this.uiPanelUserDetail.TabStop = false;
            this.uiPanelUserDetail.Text = "상세정보";
            // 
            // uiPanelUserDetailContainer
            // 
            this.uiPanelUserDetailContainer.Controls.Add(this.pnlUserDetail);
            this.uiPanelUserDetailContainer.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.uiPanelUserDetailContainer.Location = new System.Drawing.Point(1, 23);
            this.uiPanelUserDetailContainer.Name = "uiPanelUserDetailContainer";
            this.uiPanelUserDetailContainer.Size = new System.Drawing.Size(1008, 151);
            this.uiPanelUserDetailContainer.TabIndex = 0;
            // 
            // pnlUserDetail
            // 
            this.pnlUserDetail.BackColor = System.Drawing.SystemColors.Window;
            this.pnlUserDetail.Controls.Add(this.ebSubTitle);
            this.pnlUserDetail.Controls.Add(this.ebContentState);
            this.pnlUserDetail.Controls.Add(this.lbOrgTitle);
            this.pnlUserDetail.Controls.Add(this.lbSubTitle);
            this.pnlUserDetail.Controls.Add(this.ebTitle);
            this.pnlUserDetail.Controls.Add(this.lbTitle);
            this.pnlUserDetail.Controls.Add(this.ebContentId);
            this.pnlUserDetail.Controls.Add(this.lbContentId);
            this.pnlUserDetail.Controls.Add(this.lbContentState);
            this.pnlUserDetail.Controls.Add(this.lbRate);
            this.pnlUserDetail.Controls.Add(this.ebRate);
            this.pnlUserDetail.Controls.Add(this.ebOrgTitle);
            this.pnlUserDetail.Controls.Add(this.btnAdd);
            this.pnlUserDetail.Controls.Add(this.btnSave);
            this.pnlUserDetail.Controls.Add(this.btnDelete);
            this.pnlUserDetail.Controls.Add(this.ebRegDate);
            this.pnlUserDetail.Controls.Add(this.label1);
            this.pnlUserDetail.Controls.Add(this.label2);
            this.pnlUserDetail.Controls.Add(this.ebModDt);
            this.pnlUserDetail.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlUserDetail.Location = new System.Drawing.Point(0, 0);
            this.pnlUserDetail.Name = "pnlUserDetail";
            this.pnlUserDetail.Size = new System.Drawing.Size(1008, 151);
            this.pnlUserDetail.TabIndex = 0;
            // 
            // ebSubTitle
            // 
            this.ebSubTitle.Font = new System.Drawing.Font("굴림체", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.ebSubTitle.Location = new System.Drawing.Point(80, 56);
            this.ebSubTitle.MaxLength = 40;
            this.ebSubTitle.Name = "ebSubTitle";
            this.ebSubTitle.Size = new System.Drawing.Size(488, 21);
            this.ebSubTitle.TabIndex = 8;
            this.ebSubTitle.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
            this.ebSubTitle.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            // 
            // ebContentState
            // 
            this.ebContentState.Font = new System.Drawing.Font("굴림체", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.ebContentState.Location = new System.Drawing.Point(656, 8);
            this.ebContentState.MaxLength = 3;
            this.ebContentState.Name = "ebContentState";
            this.ebContentState.Size = new System.Drawing.Size(176, 21);
            this.ebContentState.TabIndex = 10;
            this.ebContentState.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
            this.ebContentState.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            // 
            // lbOrgTitle
            // 
            this.lbOrgTitle.Font = new System.Drawing.Font("굴림체", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lbOrgTitle.Location = new System.Drawing.Point(8, 80);
            this.lbOrgTitle.Name = "lbOrgTitle";
            this.lbOrgTitle.Size = new System.Drawing.Size(72, 21);
            this.lbOrgTitle.TabIndex = 0;
            this.lbOrgTitle.Text = "원제목";
            this.lbOrgTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lbSubTitle
            // 
            this.lbSubTitle.Font = new System.Drawing.Font("굴림체", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lbSubTitle.Location = new System.Drawing.Point(8, 56);
            this.lbSubTitle.Name = "lbSubTitle";
            this.lbSubTitle.Size = new System.Drawing.Size(72, 21);
            this.lbSubTitle.TabIndex = 0;
            this.lbSubTitle.Text = "부제목";
            this.lbSubTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ebTitle
            // 
            this.ebTitle.Font = new System.Drawing.Font("굴림체", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.ebTitle.Location = new System.Drawing.Point(80, 8);
            this.ebTitle.MaxLength = 120;
            this.ebTitle.Name = "ebTitle";
            this.ebTitle.Size = new System.Drawing.Size(488, 21);
            this.ebTitle.TabIndex = 6;
            this.ebTitle.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
            this.ebTitle.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            // 
            // lbTitle
            // 
            this.lbTitle.Font = new System.Drawing.Font("굴림체", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lbTitle.Location = new System.Drawing.Point(8, 8);
            this.lbTitle.Name = "lbTitle";
            this.lbTitle.Size = new System.Drawing.Size(72, 21);
            this.lbTitle.TabIndex = 0;
            this.lbTitle.Text = "제목";
            this.lbTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ebContentId
            // 
            this.ebContentId.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ebContentId.Font = new System.Drawing.Font("굴림체", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.ebContentId.Location = new System.Drawing.Point(80, 32);
            this.ebContentId.MaxLength = 36;
            this.ebContentId.Name = "ebContentId";
            this.ebContentId.ReadOnly = true;
            this.ebContentId.Size = new System.Drawing.Size(488, 21);
            this.ebContentId.TabIndex = 0;
            this.ebContentId.TabStop = false;
            this.ebContentId.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
            this.ebContentId.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            // 
            // lbContentId
            // 
            this.lbContentId.Font = new System.Drawing.Font("굴림체", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lbContentId.Location = new System.Drawing.Point(8, 32);
            this.lbContentId.Name = "lbContentId";
            this.lbContentId.Size = new System.Drawing.Size(72, 21);
            this.lbContentId.TabIndex = 0;
            this.lbContentId.Text = "컨텐츠ID ";
            this.lbContentId.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lbContentState
            // 
            this.lbContentState.Font = new System.Drawing.Font("굴림체", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lbContentState.Location = new System.Drawing.Point(584, 8);
            this.lbContentState.Name = "lbContentState";
            this.lbContentState.Size = new System.Drawing.Size(72, 21);
            this.lbContentState.TabIndex = 0;
            this.lbContentState.Text = "상태 ";
            this.lbContentState.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lbRate
            // 
            this.lbRate.Font = new System.Drawing.Font("굴림체", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lbRate.Location = new System.Drawing.Point(584, 32);
            this.lbRate.Name = "lbRate";
            this.lbRate.Size = new System.Drawing.Size(72, 21);
            this.lbRate.TabIndex = 0;
            this.lbRate.Text = "등급 ";
            this.lbRate.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ebRate
            // 
            this.ebRate.Font = new System.Drawing.Font("굴림체", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.ebRate.Location = new System.Drawing.Point(656, 32);
            this.ebRate.MaxLength = 3;
            this.ebRate.Name = "ebRate";
            this.ebRate.Size = new System.Drawing.Size(88, 21);
            this.ebRate.TabIndex = 11;
            this.ebRate.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
            this.ebRate.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            // 
            // ebOrgTitle
            // 
            this.ebOrgTitle.Font = new System.Drawing.Font("굴림체", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.ebOrgTitle.Location = new System.Drawing.Point(80, 80);
            this.ebOrgTitle.MaxLength = 40;
            this.ebOrgTitle.Name = "ebOrgTitle";
            this.ebOrgTitle.Size = new System.Drawing.Size(488, 21);
            this.ebOrgTitle.TabIndex = 9;
            this.ebOrgTitle.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
            this.ebOrgTitle.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            // 
            // btnAdd
            // 
            this.btnAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnAdd.Enabled = false;
            this.btnAdd.Location = new System.Drawing.Point(232, 117);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(104, 24);
            this.btnAdd.TabIndex = 16;
            this.btnAdd.Text = "추 가";
            this.btnAdd.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnSave.Enabled = false;
            this.btnSave.Location = new System.Drawing.Point(8, 117);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(104, 24);
            this.btnSave.TabIndex = 14;
            this.btnSave.Text = "저 장";
            this.btnSave.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnDelete.Enabled = false;
            this.btnDelete.Location = new System.Drawing.Point(120, 117);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(104, 24);
            this.btnDelete.TabIndex = 15;
            this.btnDelete.Text = "삭 제";
            this.btnDelete.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // ebRegDate
            // 
            this.ebRegDate.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ebRegDate.Font = new System.Drawing.Font("굴림체", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.ebRegDate.Location = new System.Drawing.Point(656, 56);
            this.ebRegDate.MaxLength = 3;
            this.ebRegDate.Name = "ebRegDate";
            this.ebRegDate.ReadOnly = true;
            this.ebRegDate.Size = new System.Drawing.Size(176, 21);
            this.ebRegDate.TabIndex = 12;
            this.ebRegDate.TabStop = false;
            this.ebRegDate.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
            this.ebRegDate.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("굴림체", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label1.Location = new System.Drawing.Point(584, 56);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(72, 21);
            this.label1.TabIndex = 0;
            this.label1.Text = "등록일시 ";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("굴림체", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label2.Location = new System.Drawing.Point(584, 80);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(72, 21);
            this.label2.TabIndex = 0;
            this.label2.Text = "최종수정 ";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ebModDt
            // 
            this.ebModDt.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ebModDt.Font = new System.Drawing.Font("굴림체", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.ebModDt.Location = new System.Drawing.Point(656, 80);
            this.ebModDt.MaxLength = 3;
            this.ebModDt.Name = "ebModDt";
            this.ebModDt.ReadOnly = true;
            this.ebModDt.Size = new System.Drawing.Size(176, 21);
            this.ebModDt.TabIndex = 13;
            this.ebModDt.TabStop = false;
            this.ebModDt.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
            this.ebModDt.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            // 
            // uiButton1
            // 
            this.uiButton1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.uiButton1.Location = new System.Drawing.Point(136, 8);
            this.uiButton1.Name = "uiButton1";
            this.uiButton1.Size = new System.Drawing.Size(112, 24);
            this.uiButton1.TabIndex = 0;
            this.uiButton1.Text = "저 장";
            this.uiButton1.VisualStyle = Janus.Windows.UI.VisualStyle.Office2003;
            // 
            // uiButton2
            // 
            this.uiButton2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.uiButton2.Location = new System.Drawing.Point(8, 8);
            this.uiButton2.Name = "uiButton2";
            this.uiButton2.Size = new System.Drawing.Size(120, 24);
            this.uiButton2.TabIndex = 0;
            this.uiButton2.Text = "추 가";
            this.uiButton2.VisualStyle = Janus.Windows.UI.VisualStyle.Office2003;
            // 
            // ContentsControl
            // 
            this.Controls.Add(this.uiPanelUsers);
            this.Name = "ContentsControl";
            this.Size = new System.Drawing.Size(1010, 677);
            this.Load += new System.EventHandler(this.ContentsControl_Load);
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
            ((System.ComponentModel.ISupportInitialize)(this.grdExContentsList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dvContents)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.contentsDs)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelUserDetail)).EndInit();
            this.uiPanelUserDetail.ResumeLayout(false);
            this.uiPanelUserDetailContainer.ResumeLayout(false);
            this.pnlUserDetail.ResumeLayout(false);
            this.pnlUserDetail.PerformLayout();
            this.ResumeLayout(false);

		}
        #endregion

        #region 컨트롤 로드
        private void ContentsControl_Load(object sender, System.EventArgs e)
        {

            
            // 데이터관리용 객체생성
            dt = ((DataView)grdExContentsList.DataSource).Table;  
            cm = (CurrencyManager) this.BindingContext[grdExContentsList.DataSource]; 
            cm.PositionChanged += new System.EventHandler(OnGrdRowChanged); 

            // 컨트롤 초기화
            InitControl();
        }

        #endregion

        #region 컨트롤 초기화
        private void InitControl()
        {
			ProgressStart();
            // 조회권한 검사
            if(menu.CanRead(MenuCode))
            {
                canRead = true;
            }
			
            // 추가버튼 활성화
            if(menu.CanCreate(MenuCode))
            {
                canCreate = true;
            }

            // 삭제버튼 활성화
            if(menu.CanDelete(MenuCode))
            {
                canDelete = true;
            }

            // 저장버튼 활성화
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
			ProgressStop();
        }

        private void InitButton()
        {
            if(canRead)   btnSearch.Enabled = true;
            if(canCreate) btnAdd.Enabled    = true;

            if(ebContentId.Text.Trim().Length > 0) 
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

        #region 사용자 액션처리 메소드

        /// <summary>
        /// 그리드의 Row변경시
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnGrdRowChanged(object sender, System.EventArgs e) 
        {
            if (!IsSearching) // 2011.11.29 JH.Park 조회중이 아닐경우에만 동작하도록 변경
            {
                if (grdExContentsList.RowCount > 1)
                {
                    SetContentsDetailText();
                    InitButton();
                }
            }
        }

        /// <summary>
        /// 조회버튼 클릭
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSearch_Click(object sender, System.EventArgs e)
        {
			ProgressStop();
            DisableButton();
            SearchContents();
            InitButton();
			ProgressStop();
        }

        /// <summary>
        /// 추가버튼 클릭
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAdd_Click(object sender, System.EventArgs e)
        {
            
           btnAdd.Enabled    = false;
           btnDelete.Enabled = false;
           if(canCreate) btnSave.Enabled   = true;

            IsAdding = true;

            ResetTextReadonly();
            ReSetContentsDetailText();

            //ebContentId.Focus();
        }

        /// <summary>
        /// 저장버튼 클릭
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_Click(object sender, System.EventArgs e)
        {           
            SaveContentDetail();
        }
        /// <summary>
        /// 삭제버튼 클릭
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDelete_Click(object sender, System.EventArgs e)
        {            
            DeleteContents();
            if(contentsModel.ResultCD.Equals("0000"))
            {
                DisableButton();
                ReSetContentsDetailText();
                InitButton();
                //SearchContents();
                                 
            }
        }


        /// <summary>
        /// 검색어 변경
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ebSearchKey_TextChanged(object sender, System.EventArgs e)
        {
            IsNewSearchKey = false;
        }

        /// <summary>
        /// 검색어 클릭 
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
                SearchContents();
            }
        }

        #endregion

        #region 처리메소드

        /// <summary>
        /// 컨텐츠목록 조회
        /// </summary>
        private void SearchContents()
        {
            IsSearching = true;

            StatusMessage("컨텐츠 정보를 조회합니다.");
			
            try
            {
                contentsModel.Init();
                // 데이터모델에 전송할 내용을 셋트한다.
                if(IsNewSearchKey)
                {
                    contentsModel.SearchKey = "";
                }
                else
                {
                    contentsModel.SearchKey  = ebSearchKey.Text;
                }

				if(contentsModel.SearchKey.Trim().Length == 0) 
				{
					FrameSystem.showMsgForm("컨텐츠조회오류",new string[] {"", "검색어를 입력해 주십시오.", "" });
					return;
				}				

				ReSetContentsDetailText();

                // 컨텐츠목록조회 서비스를 호출한다.
                new ContentsManager(systemModel,commonModel).GetContentsList(contentsModel);

                if (contentsModel.ResultCD.Equals("0000"))
                {
                    Utility.SetDataTable(contentsDs.Contents, contentsModel.ContentsDataSet);				
					SetContentsDetailText();
                    StatusMessage(contentsModel.ResultCnt + "건의 컨텐츠 정보가 조회되었습니다.");
					if(canUpdate)
					{
						AddSchChoice();									
					}			
					SetContentsDetailText();
                }
            }
            catch(FrameException fe)
            {
                FrameSystem.showMsgForm("컨텐츠조회오류", new string[] {fe.ErrCode, fe.ResultMsg});
            }
            catch(Exception ex)
            {
                FrameSystem.showMsgForm("컨텐츠조회오류",new string[] {"",ex.Message});
            }
            finally
            {
                IsSearching = false; // 조회중 Flag 리셋
            }
        }

		/// <summary>
		/// 키캆을찾아 그리드 키에 해당되는로우로..
		/// </summary>
		private void AddSchChoice()
		{
			StatusMessage("키캆");		

			try
			{
				int rowIndex = 0;
				if ( contentsDs.Tables["Contents"].Rows.Count < 1 ) return;
              
				foreach (DataRow row in contentsDs.Tables["Contents"].Rows)
				{					
					if(IsAdding)
					{
						cm.Position = 0;
						contentId = null;						
					}
					else
					{						
						if(row["ContentId"].ToString().Equals(contentId))
						{					
							cm.Position = rowIndex;
							break;								
						}
					}

					rowIndex++;
					grdExContentsList.EnsureVisible();
				}
			}
			catch(FrameException fe)
			{
				FrameSystem.showMsgForm("키캆오류", new string[] {fe.ErrCode, fe.ResultMsg});
			}
			catch(Exception ex)
			{
				FrameSystem.showMsgForm("키캆오류",new string[] {"",ex.Message});
			}			
		}

        /// <summary>
        /// 컨텐츠상세정보 저장
        /// </summary>
        private void SaveContentDetail()
        {
            StatusMessage("컨텐츠 정보를 저장합니다.");
            
          
            if(ebTitle.Text.Trim().Length == 0) 
            {
				MessageBox.Show("제목이 입력되지 않았습니다.","컨텐츠 저장", 
					MessageBoxButtons.OK, MessageBoxIcon.Information );
				return;	               
            }
            
            try
            {
                //저장 전에 모델을 초기화 해준다.
                contentsModel.Init();
                // 데이터모델에 전송할 내용을 셋트한다.
                contentsModel.Title          = ebTitle.Text;
                contentsModel.ContentsState  = ebContentState.Text;
                contentsModel.Rate          = ebRate.Text;
                contentsModel.SubTitle  = ebSubTitle.Text;
                contentsModel.OrgTitle  = ebOrgTitle.Text;

                // 컨텐츠 상세정보 저장 서비스를 호출한다.
                if (IsAdding)
                {
                    new ContentsManager(systemModel,commonModel).SetContentsAdd(contentsModel);
					StatusMessage("컨텐츠 정보가 추가되었습니다.");
					IsAdding = false;
					ReSetContentsDetailText();
                }
                else
                {
                    int curRow = cm.Position;
                    //업데이트 일경우에는 키값을 설정을 해준다.
                    contentsModel.ContentKey = dt.Rows[curRow]["ContentKey"].ToString();
                    new ContentsManager(systemModel,commonModel).SetContentsUpdate(contentsModel);
                }
            
				DisableButton();				
				InitButton();

            }
            catch(FrameException fe)
            {
                FrameSystem.showMsgForm("컨텐츠정보 저장오류", new string[] {fe.ErrCode, fe.ResultMsg});
            }
            catch(Exception ex)
            {
                FrameSystem.showMsgForm("컨텐츠정보 저장오류",new string[] {"",ex.Message});
            }		
	       
        }

        /// <summary>
        /// 컨텐츠정보 삭제
        /// </summary>
        private void DeleteContents()
        {
            StatusMessage("컨텐츠 정보를 삭제합니다.");
            
            if(ebContentId.Text.Trim().Length == 0) 
            {
                MessageBox.Show("삭제할 컨텐츠 정보가 없습니다.","컨텐츠 삭제", 
                    MessageBoxButtons.OK, MessageBoxIcon.Information );
                return;
            }
            
            DialogResult result = MessageBox.Show("해당 컨텐츠 정보를 삭제 하시겠습니까?","컨텐츠 삭제",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question,
                MessageBoxDefaultButton.Button2);
            
            if (result == DialogResult.No) return;
            
            try
            {
                //저장 전에 모델을 초기화 해준다.
                contentsModel.Init();
                // 데이터모델에 전송할 내용을 셋트한다.
                //Delete 키값 생성
                int curRow = cm.Position;
                contentsModel.ContentKey = dt.Rows[curRow]["ContentKey"].ToString();
            
                // 컨텐츠 상세정보 저장 서비스를 호출한다.
                new ContentsManager(systemModel,commonModel).SetContentsDelete(contentsModel);
                           
                ReSetContentsDetailText();            
                StatusMessage("컨텐츠 정보가 삭제되었습니다.");			
            
				DisableButton();				
				InitButton();
            }
            catch(FrameException fe)
            {
                FrameSystem.showMsgForm("컨텐츠정보 삭제오류", new string[] {fe.ErrCode, fe.ResultMsg});
            }
            catch(Exception ex)
            {
                FrameSystem.showMsgForm("컨텐츠정보 삭제오류",new string[] {"",ex.Message});
            }			

        
        }
		
        /// <summary>
        /// 컨텐츠 상세정보의 셋트
        /// </summary>
        private void SetContentsDetailText()
        {
            int curRow = cm.Position;

			if(curRow >= 0)
			{
				ebContentId.Text            = dt.Rows[curRow]["ContentId"].ToString();
				contentId            = dt.Rows[curRow]["ContentId"].ToString();
				ebTitle.Text                = dt.Rows[curRow]["Title"].ToString();
				ebRate.Text                 = dt.Rows[curRow]["Rate"].ToString();
				ebContentState.Text         = dt.Rows[curRow]["ContentsState"].ToString();
				ebSubTitle.Text             = dt.Rows[curRow]["SubTitle"].ToString();
				ebOrgTitle.Text             = dt.Rows[curRow]["OrgTitle"].ToString();
				ebRegDate.Text              = dt.Rows[curRow]["RegDate"].ToString();
				ebModDt.Text                = dt.Rows[curRow]["ModDt"].ToString();
				ebContentId.ReadOnly        = true;
				ebContentId.BackColor       = Color.WhiteSmoke;
	            
				IsAdding = false;
			}
            StatusMessage("준비");
        }

        private void ReSetContentsDetailText()
        {
            ebContentId.Text             = "";
            ebTitle.Text                 = "";
            ebContentState.Text          = "";
            ebRate.Text                  = "";
            ebSubTitle.Text              = "";
            ebOrgTitle.Text              = "";
            ebRegDate.Text               = "";
            ebModDt.Text                 = "";

        }
		
        /// <summary>
        /// 상세정보 ReadOnly
        /// </summary>
        private void SetTextReadonly()
        {
        }

        /// <summary>
        /// 상세정보 수정가능케
        /// </summary>
        private void ResetTextReadonly()
        {

        }
        #endregion

        #region 이벤트함수

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
