// ===============================================================================
// UserInfoControl for Charites Project
//
// UserInfoControl.cs
//
// 사용자정보관리 컨드롤을 정의합니다. 
//
// ===============================================================================
// Release history
//
// ===============================================================================
// Copyright (C) 2006 G-Inno Systems Inc.
// All rights reserved.
//

/*
 * -------------------------------------------------------
 * [수정사항]
 * -------------------------------------------------------
 * 수정코드  : [A_01]
 * 수정자    : JH.Kim
 * 수정일    : 2015.11.
 * 수정내용  : 광고업종 추가
 * --------------------------------------------------------
 */
 
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
	/// 사용자관리 컨트롤
	/// </summary>
    public class AdvertiserControl : System.Windows.Forms.UserControl, IUserControl
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
		public string menuCode = "";

		// 사용할 정보모델
		AdvertiserModel advertiserModel = new AdvertiserModel();	// 사용자정보모델

		// 화면처리용 변수
		bool IsNewSearchKey	= true;					// 검색어입력 여부
		CurrencyManager cm  = null;					// 데이터 그리드의 변경에 따른 데이터셋 관리를 위하여			
		DataTable       dt  = null;

        bool IsSearching = false; // 조회중 상세화면이 업데이트 되는 것을 방지 하기위함 2011.11.29 JH.Park
		bool IsAdding    = false;
		bool canRead	 = false;
		bool canUpdate	 = false;
		bool canCreate   = false;
		bool canDelete   = false;

        private string advertiserCode = null;
        private Panel pnlSearch;
        private Janus.Windows.EditControls.UICheckBox uiCheckBox1;
        private Janus.Windows.EditControls.UIComboBox cbSearchRap;
        private Janus.Windows.EditControls.UIButton btnSearch;
        private Janus.Windows.GridEX.EditControls.EditBox ebSearchKey;
        private Janus.Windows.EditControls.UICheckBox uiCheckBox2;
		private Janus.Windows.EditControls.UIButton btnJobClassSearch;
		private Janus.Windows.GridEX.EditControls.EditBox ebJobClassName;
		private Label label1;
		private string  keyRepCode = "";
        private string  keyJobCode = string.Empty; // [A_01] 광고 업종 선택에서 획득하는 업종코드
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
		private System.Windows.Forms.Panel pnlUserDetail;
		private Janus.Windows.EditControls.UIButton btnAdd;
		private Janus.Windows.EditControls.UIButton btnSave;
		private Janus.Windows.EditControls.UIButton btnDelete;
		private Janus.Windows.GridEX.EditControls.EditBox ebUserName;		
		private System.Data.DataView dvAdvertiser;
		private Janus.Windows.GridEX.EditControls.EditBox ebAdvertiserName;
		private System.Windows.Forms.Label lbAdvertiserName2;		
		private System.Windows.Forms.Label lbComment;
		private System.Windows.Forms.Label label2;
		private Janus.Windows.GridEX.EditControls.EditBox ebComment;
		private Janus.Windows.GridEX.EditControls.EditBox ebRegDt;
		private Janus.Windows.GridEX.EditControls.EditBox ebModDt;
		private Janus.Windows.EditControls.UIRadioButton rbUseYn_Y;
		private Janus.Windows.EditControls.UIRadioButton rbUseYn_N;
		private System.Windows.Forms.Label lbUseYn;
		private System.Windows.Forms.Label lbModDt;
		private AdManagerClient.AdvertiserDs advertiserDs;
		private System.Windows.Forms.Label lbRegID;
		private Janus.Windows.GridEX.EditControls.EditBox ebRegID;
        private Janus.Windows.GridEX.GridEX grdExAdvertiserList;
		private Janus.Windows.EditControls.UIButton btnRapSearch;
		private Janus.Windows.GridEX.EditControls.EditBox ebRapName;
		private System.Windows.Forms.Label label3;				
		private System.ComponentModel.IContainer components;

		public AdvertiserControl()
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
            Janus.Windows.GridEX.GridEXLayout grdExAdvertiserList_DesignTimeLayout = new Janus.Windows.GridEX.GridEXLayout();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AdvertiserControl));
            this.uiPM = new Janus.Windows.UI.Dock.UIPanelManager(this.components);
            this.uiPanelUsers = new Janus.Windows.UI.Dock.UIPanelGroup();
            this.uiPanelUsersSearch = new Janus.Windows.UI.Dock.UIPanel();
            this.uiPanelUsersSearchContainer = new Janus.Windows.UI.Dock.UIPanelInnerContainer();
            this.pnlSearch = new System.Windows.Forms.Panel();
            this.uiCheckBox1 = new Janus.Windows.EditControls.UICheckBox();
            this.cbSearchRap = new Janus.Windows.EditControls.UIComboBox();
            this.btnSearch = new Janus.Windows.EditControls.UIButton();
            this.ebSearchKey = new Janus.Windows.GridEX.EditControls.EditBox();
            this.uiPanelUserList = new Janus.Windows.UI.Dock.UIPanel();
            this.uiPanelUserListContainer = new Janus.Windows.UI.Dock.UIPanelInnerContainer();
            this.grdExAdvertiserList = new Janus.Windows.GridEX.GridEX();
            this.dvAdvertiser = new System.Data.DataView();
            this.advertiserDs = new AdManagerClient.AdvertiserDs();
            this.uiPanelUserDetail = new Janus.Windows.UI.Dock.UIPanel();
            this.uiPanelUserDetailContainer = new Janus.Windows.UI.Dock.UIPanelInnerContainer();
            this.pnlUserDetail = new System.Windows.Forms.Panel();
            this.btnJobClassSearch = new Janus.Windows.EditControls.UIButton();
            this.ebJobClassName = new Janus.Windows.GridEX.EditControls.EditBox();
            this.label1 = new System.Windows.Forms.Label();
            this.uiCheckBox2 = new Janus.Windows.EditControls.UICheckBox();
            this.btnRapSearch = new Janus.Windows.EditControls.UIButton();
            this.ebRapName = new Janus.Windows.GridEX.EditControls.EditBox();
            this.label3 = new System.Windows.Forms.Label();
            this.rbUseYn_N = new Janus.Windows.EditControls.UIRadioButton();
            this.rbUseYn_Y = new Janus.Windows.EditControls.UIRadioButton();
            this.lbUseYn = new System.Windows.Forms.Label();
            this.ebModDt = new Janus.Windows.GridEX.EditControls.EditBox();
            this.ebRegDt = new Janus.Windows.GridEX.EditControls.EditBox();
            this.ebComment = new Janus.Windows.GridEX.EditControls.EditBox();
            this.ebAdvertiserName = new Janus.Windows.GridEX.EditControls.EditBox();
            this.lbAdvertiserName2 = new System.Windows.Forms.Label();
            this.lbModDt = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.lbComment = new System.Windows.Forms.Label();
            this.ebRegID = new Janus.Windows.GridEX.EditControls.EditBox();
            this.lbRegID = new System.Windows.Forms.Label();
            this.btnDelete = new Janus.Windows.EditControls.UIButton();
            this.btnAdd = new Janus.Windows.EditControls.UIButton();
            this.btnSave = new Janus.Windows.EditControls.UIButton();
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
            ((System.ComponentModel.ISupportInitialize)(this.grdExAdvertiserList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dvAdvertiser)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.advertiserDs)).BeginInit();
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
            this.uiPM.AddDockPanelInfo(new System.Guid("8e3140b9-bfcb-43af-b84e-2ee6cddf51dc"), new System.Guid("b162c53a-7940-47cd-a0ed-9762c59c50d8"), 420, true);
            this.uiPM.AddDockPanelInfo(new System.Guid("b2973a57-d3e1-4d80-9387-7a6dc3332542"), new System.Guid("b162c53a-7940-47cd-a0ed-9762c59c50d8"), 165, true);
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
            this.uiPanelUsers.Text = "광고주관리";
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
            this.pnlSearch.Controls.Add(this.uiCheckBox1);
            this.pnlSearch.Controls.Add(this.cbSearchRap);
            this.pnlSearch.Controls.Add(this.btnSearch);
            this.pnlSearch.Controls.Add(this.ebSearchKey);
            this.pnlSearch.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlSearch.Location = new System.Drawing.Point(0, 0);
            this.pnlSearch.Name = "pnlSearch";
            this.pnlSearch.Size = new System.Drawing.Size(1008, 41);
            this.pnlSearch.TabIndex = 0;
            // 
            // uiCheckBox1
            // 
            this.uiCheckBox1.Location = new System.Drawing.Point(350, 8);
            this.uiCheckBox1.Name = "uiCheckBox1";
            this.uiCheckBox1.Size = new System.Drawing.Size(104, 23);
            this.uiCheckBox1.TabIndex = 5;
            this.uiCheckBox1.Text = "사용안함 포함";
            this.uiCheckBox1.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            // 
            // cbSearchRap
            // 
            this.cbSearchRap.ComboStyle = Janus.Windows.EditControls.ComboStyle.DropDownList;
            this.cbSearchRap.Location = new System.Drawing.Point(8, 8);
            this.cbSearchRap.Name = "cbSearchRap";
            this.cbSearchRap.Size = new System.Drawing.Size(120, 21);
            this.cbSearchRap.TabIndex = 1;
            this.cbSearchRap.Text = "미디어렙선택";
            this.cbSearchRap.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            // 
            // btnSearch
            // 
            this.btnSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSearch.Enabled = false;
            this.btnSearch.Location = new System.Drawing.Point(893, 8);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(104, 24);
            this.btnSearch.TabIndex = 4;
            this.btnSearch.Text = "조 회";
            this.btnSearch.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // ebSearchKey
            // 
            this.ebSearchKey.Location = new System.Drawing.Point(136, 8);
            this.ebSearchKey.Name = "ebSearchKey";
            this.ebSearchKey.Size = new System.Drawing.Size(208, 21);
            this.ebSearchKey.TabIndex = 2;
            this.ebSearchKey.Text = "검색어를 입력하세요";
            this.ebSearchKey.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
            this.ebSearchKey.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            this.ebSearchKey.TextChanged += new System.EventHandler(this.ebSearchKey_TextChanged);
            this.ebSearchKey.Click += new System.EventHandler(this.ebSearchKey_Click);
            this.ebSearchKey.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ebSearchKey_KeyDown);
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
            this.uiPanelUserList.Size = new System.Drawing.Size(1010, 434);
            this.uiPanelUserList.TabIndex = 0;
            this.uiPanelUserList.TabStop = false;
            this.uiPanelUserList.Text = "광고주";
            // 
            // uiPanelUserListContainer
            // 
            this.uiPanelUserListContainer.BackColor = System.Drawing.SystemColors.Window;
            this.uiPanelUserListContainer.Controls.Add(this.grdExAdvertiserList);
            this.uiPanelUserListContainer.Location = new System.Drawing.Point(1, 23);
            this.uiPanelUserListContainer.Name = "uiPanelUserListContainer";
            this.uiPanelUserListContainer.Size = new System.Drawing.Size(1008, 410);
            this.uiPanelUserListContainer.TabIndex = 0;
            // 
            // grdExAdvertiserList
            // 
            this.grdExAdvertiserList.AllowEdit = Janus.Windows.GridEX.InheritableBoolean.False;
            this.grdExAdvertiserList.AlternatingColors = true;
            this.grdExAdvertiserList.AlternatingRowFormatStyle.BackgroundImageDrawMode = Janus.Windows.GridEX.BackgroundImageDrawMode.Tile;
            this.grdExAdvertiserList.BorderStyle = Janus.Windows.GridEX.BorderStyle.None;
            this.grdExAdvertiserList.DataSource = this.dvAdvertiser;
            grdExAdvertiserList_DesignTimeLayout.LayoutString = resources.GetString("grdExAdvertiserList_DesignTimeLayout.LayoutString");
            this.grdExAdvertiserList.DesignTimeLayout = grdExAdvertiserList_DesignTimeLayout;
            this.grdExAdvertiserList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdExAdvertiserList.EmptyRows = true;
            this.grdExAdvertiserList.FocusCellFormatStyle.BackColor = System.Drawing.SystemColors.Highlight;
            this.grdExAdvertiserList.FocusCellFormatStyle.ForeColor = System.Drawing.Color.White;
            this.grdExAdvertiserList.FocusStyle = Janus.Windows.GridEX.FocusStyle.None;
            this.grdExAdvertiserList.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.5F);
            this.grdExAdvertiserList.FrozenColumns = 2;
            this.grdExAdvertiserList.GridLineColor = System.Drawing.Color.Silver;
            this.grdExAdvertiserList.GridLines = Janus.Windows.GridEX.GridLines.Vertical;
            this.grdExAdvertiserList.GridLineStyle = Janus.Windows.GridEX.GridLineStyle.Solid;
            this.grdExAdvertiserList.GroupByBoxVisible = false;
            this.grdExAdvertiserList.HeaderFormatStyle.TextAlignment = Janus.Windows.GridEX.TextAlignment.Center;
            this.grdExAdvertiserList.HideSelection = Janus.Windows.GridEX.HideSelection.HighlightInactive;
            this.grdExAdvertiserList.Location = new System.Drawing.Point(0, 0);
            this.grdExAdvertiserList.Name = "grdExAdvertiserList";
            this.grdExAdvertiserList.ScrollBars = Janus.Windows.GridEX.ScrollBars.Vertical;
            this.grdExAdvertiserList.SelectedInactiveFormatStyle.BackColor = System.Drawing.Color.Gold;
            this.grdExAdvertiserList.Size = new System.Drawing.Size(1008, 410);
            this.grdExAdvertiserList.TabIndex = 5;
            this.grdExAdvertiserList.TabKeyBehavior = Janus.Windows.GridEX.TabKeyBehavior.ControlNavigation;
            this.grdExAdvertiserList.ThemedAreas = ((Janus.Windows.GridEX.ThemedArea)(((((((((Janus.Windows.GridEX.ThemedArea.ScrollBars | Janus.Windows.GridEX.ThemedArea.EditControls) 
            | Janus.Windows.GridEX.ThemedArea.Headers) 
            | Janus.Windows.GridEX.ThemedArea.GroupByBox) 
            | Janus.Windows.GridEX.ThemedArea.GroupRows) 
            | Janus.Windows.GridEX.ThemedArea.ControlBorder) 
            | Janus.Windows.GridEX.ThemedArea.Cards) 
            | Janus.Windows.GridEX.ThemedArea.Gridlines) 
            | Janus.Windows.GridEX.ThemedArea.CheckBoxes)));
            this.grdExAdvertiserList.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            this.grdExAdvertiserList.FormattingRow += new Janus.Windows.GridEX.RowLoadEventHandler(this.grdExAdvertiserList_FormattingRow);
            // 
            // dvAdvertiser
            // 
            this.dvAdvertiser.Table = this.advertiserDs.Advertisers;
            // 
            // advertiserDs
            // 
            this.advertiserDs.DataSetName = "AdvertiserDs";
            this.advertiserDs.Locale = new System.Globalization.CultureInfo("en-US");
            this.advertiserDs.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // uiPanelUserDetail
            // 
            this.uiPanelUserDetail.AllowResize = Janus.Windows.UI.InheritableBoolean.False;
            this.uiPanelUserDetail.BackColor = System.Drawing.SystemColors.Window;
            this.uiPanelUserDetail.CaptionStyle = Janus.Windows.UI.Dock.PanelCaptionStyle.Dark;
            this.uiPanelUserDetail.InnerContainer = this.uiPanelUserDetailContainer;
            this.uiPanelUserDetail.Location = new System.Drawing.Point(0, 507);
            this.uiPanelUserDetail.Name = "uiPanelUserDetail";
            this.uiPanelUserDetail.Size = new System.Drawing.Size(1010, 170);
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
            this.uiPanelUserDetailContainer.Size = new System.Drawing.Size(1008, 146);
            this.uiPanelUserDetailContainer.TabIndex = 0;
            // 
            // pnlUserDetail
            // 
            this.pnlUserDetail.BackColor = System.Drawing.SystemColors.Window;
            this.pnlUserDetail.Controls.Add(this.btnJobClassSearch);
            this.pnlUserDetail.Controls.Add(this.ebJobClassName);
            this.pnlUserDetail.Controls.Add(this.label1);
            this.pnlUserDetail.Controls.Add(this.uiCheckBox2);
            this.pnlUserDetail.Controls.Add(this.btnRapSearch);
            this.pnlUserDetail.Controls.Add(this.ebRapName);
            this.pnlUserDetail.Controls.Add(this.label3);
            this.pnlUserDetail.Controls.Add(this.rbUseYn_N);
            this.pnlUserDetail.Controls.Add(this.rbUseYn_Y);
            this.pnlUserDetail.Controls.Add(this.lbUseYn);
            this.pnlUserDetail.Controls.Add(this.ebModDt);
            this.pnlUserDetail.Controls.Add(this.ebRegDt);
            this.pnlUserDetail.Controls.Add(this.ebComment);
            this.pnlUserDetail.Controls.Add(this.ebAdvertiserName);
            this.pnlUserDetail.Controls.Add(this.lbAdvertiserName2);
            this.pnlUserDetail.Controls.Add(this.lbModDt);
            this.pnlUserDetail.Controls.Add(this.label2);
            this.pnlUserDetail.Controls.Add(this.lbComment);
            this.pnlUserDetail.Controls.Add(this.ebRegID);
            this.pnlUserDetail.Controls.Add(this.lbRegID);
            this.pnlUserDetail.Controls.Add(this.btnDelete);
            this.pnlUserDetail.Controls.Add(this.btnAdd);
            this.pnlUserDetail.Controls.Add(this.btnSave);
            this.pnlUserDetail.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlUserDetail.Location = new System.Drawing.Point(0, 0);
            this.pnlUserDetail.Name = "pnlUserDetail";
            this.pnlUserDetail.Size = new System.Drawing.Size(1008, 146);
            this.pnlUserDetail.TabIndex = 0;
            // 
            // btnJobClassSearch
            // 
            this.btnJobClassSearch.Enabled = false;
            this.btnJobClassSearch.Location = new System.Drawing.Point(317, 59);
            this.btnJobClassSearch.Name = "btnJobClassSearch";
            this.btnJobClassSearch.Size = new System.Drawing.Size(104, 21);
            this.btnJobClassSearch.TabIndex = 202;
            this.btnJobClassSearch.Text = "광고업종선택";
            this.btnJobClassSearch.Visible = false;
            this.btnJobClassSearch.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.btnJobClassSearch.Click += new System.EventHandler(this.btnJobClassSearch_Click);
            // 
            // ebJobClassName
            // 
            this.ebJobClassName.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ebJobClassName.Location = new System.Drawing.Point(72, 59);
            this.ebJobClassName.Name = "ebJobClassName";
            this.ebJobClassName.Size = new System.Drawing.Size(239, 21);
            this.ebJobClassName.TabIndex = 201;
            this.ebJobClassName.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
            this.ebJobClassName.Visible = false;
            this.ebJobClassName.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(8, 59);
            this.label1.Name = "label1";
            this.label1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.label1.Size = new System.Drawing.Size(62, 21);
            this.label1.TabIndex = 203;
            this.label1.Text = "광고업종";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label1.Visible = false;
            // 
            // uiCheckBox2
            // 
            this.uiCheckBox2.Location = new System.Drawing.Point(427, 7);
            this.uiCheckBox2.Name = "uiCheckBox2";
            this.uiCheckBox2.Size = new System.Drawing.Size(104, 23);
            this.uiCheckBox2.TabIndex = 200;
            this.uiCheckBox2.Text = "미디어렙 공용";
            this.uiCheckBox2.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            // 
            // btnRapSearch
            // 
            this.btnRapSearch.Enabled = false;
            this.btnRapSearch.Location = new System.Drawing.Point(317, 8);
            this.btnRapSearch.Name = "btnRapSearch";
            this.btnRapSearch.Size = new System.Drawing.Size(104, 21);
            this.btnRapSearch.TabIndex = 6;
            this.btnRapSearch.Text = "미디어렙선택";
            this.btnRapSearch.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.btnRapSearch.Click += new System.EventHandler(this.btnRapSearch_Click);
            // 
            // ebRapName
            // 
            this.ebRapName.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ebRapName.Location = new System.Drawing.Point(72, 8);
            this.ebRapName.Name = "ebRapName";
            this.ebRapName.Size = new System.Drawing.Size(239, 21);
            this.ebRapName.TabIndex = 5;
            this.ebRapName.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
            this.ebRapName.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(8, 8);
            this.label3.Name = "label3";
            this.label3.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.label3.Size = new System.Drawing.Size(62, 21);
            this.label3.TabIndex = 199;
            this.label3.Text = "미디어렙";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // rbUseYn_N
            // 
            this.rbUseYn_N.BackColor = System.Drawing.Color.Transparent;
            this.rbUseYn_N.Location = new System.Drawing.Point(907, 80);
            this.rbUseYn_N.Name = "rbUseYn_N";
            this.rbUseYn_N.Size = new System.Drawing.Size(72, 23);
            this.rbUseYn_N.TabIndex = 32;
            this.rbUseYn_N.Text = "사용안함";
            this.rbUseYn_N.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            // 
            // rbUseYn_Y
            // 
            this.rbUseYn_Y.BackColor = System.Drawing.Color.Transparent;
            this.rbUseYn_Y.Location = new System.Drawing.Point(827, 80);
            this.rbUseYn_Y.Name = "rbUseYn_Y";
            this.rbUseYn_Y.Size = new System.Drawing.Size(72, 23);
            this.rbUseYn_Y.TabIndex = 10;
            this.rbUseYn_Y.Text = "사용함";
            this.rbUseYn_Y.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            // 
            // lbUseYn
            // 
            this.lbUseYn.Location = new System.Drawing.Point(746, 80);
            this.lbUseYn.Name = "lbUseYn";
            this.lbUseYn.Size = new System.Drawing.Size(72, 21);
            this.lbUseYn.TabIndex = 30;
            this.lbUseYn.Text = "사용여부";
            this.lbUseYn.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ebModDt
            // 
            this.ebModDt.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ebModDt.Location = new System.Drawing.Point(818, 32);
            this.ebModDt.Name = "ebModDt";
            this.ebModDt.ReadOnly = true;
            this.ebModDt.Size = new System.Drawing.Size(168, 21);
            this.ebModDt.TabIndex = 9;
            this.ebModDt.TabStop = false;
            this.ebModDt.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
            this.ebModDt.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            // 
            // ebRegDt
            // 
            this.ebRegDt.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ebRegDt.Location = new System.Drawing.Point(818, 8);
            this.ebRegDt.Name = "ebRegDt";
            this.ebRegDt.ReadOnly = true;
            this.ebRegDt.Size = new System.Drawing.Size(168, 21);
            this.ebRegDt.TabIndex = 7;
            this.ebRegDt.TabStop = false;
            this.ebRegDt.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
            this.ebRegDt.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            // 
            // ebComment
            // 
            this.ebComment.Location = new System.Drawing.Point(583, 8);
            this.ebComment.Multiline = true;
            this.ebComment.Name = "ebComment";
            this.ebComment.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.ebComment.Size = new System.Drawing.Size(157, 93);
            this.ebComment.TabIndex = 9;
            this.ebComment.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
            this.ebComment.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            // 
            // ebAdvertiserName
            // 
            this.ebAdvertiserName.Location = new System.Drawing.Point(72, 33);
            this.ebAdvertiserName.MaxLength = 40;
            this.ebAdvertiserName.Name = "ebAdvertiserName";
            this.ebAdvertiserName.Size = new System.Drawing.Size(239, 21);
            this.ebAdvertiserName.TabIndex = 8;
            this.ebAdvertiserName.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
            this.ebAdvertiserName.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            // 
            // lbAdvertiserName2
            // 
            this.lbAdvertiserName2.Location = new System.Drawing.Point(8, 33);
            this.lbAdvertiserName2.Name = "lbAdvertiserName2";
            this.lbAdvertiserName2.Size = new System.Drawing.Size(64, 21);
            this.lbAdvertiserName2.TabIndex = 24;
            this.lbAdvertiserName2.Text = "광고주명";
            this.lbAdvertiserName2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lbModDt
            // 
            this.lbModDt.Location = new System.Drawing.Point(746, 32);
            this.lbModDt.Name = "lbModDt";
            this.lbModDt.Size = new System.Drawing.Size(72, 21);
            this.lbModDt.TabIndex = 7;
            this.lbModDt.Text = "수정일시";
            this.lbModDt.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(746, 8);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(72, 21);
            this.label2.TabIndex = 6;
            this.label2.Text = "등록일시";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lbComment
            // 
            this.lbComment.Location = new System.Drawing.Point(537, 8);
            this.lbComment.Name = "lbComment";
            this.lbComment.Size = new System.Drawing.Size(40, 21);
            this.lbComment.TabIndex = 5;
            this.lbComment.Text = "비고";
            this.lbComment.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ebRegID
            // 
            this.ebRegID.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ebRegID.Location = new System.Drawing.Point(818, 56);
            this.ebRegID.Name = "ebRegID";
            this.ebRegID.ReadOnly = true;
            this.ebRegID.Size = new System.Drawing.Size(168, 21);
            this.ebRegID.TabIndex = 10;
            this.ebRegID.TabStop = false;
            this.ebRegID.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
            this.ebRegID.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            // 
            // lbRegID
            // 
            this.lbRegID.Location = new System.Drawing.Point(746, 56);
            this.lbRegID.Name = "lbRegID";
            this.lbRegID.Size = new System.Drawing.Size(72, 21);
            this.lbRegID.TabIndex = 7;
            this.lbRegID.Text = "등록자ID";
            this.lbRegID.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnDelete
            // 
            this.btnDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnDelete.Enabled = false;
            this.btnDelete.Location = new System.Drawing.Point(120, 114);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(104, 24);
            this.btnDelete.TabIndex = 12;
            this.btnDelete.Text = "삭 제";
            this.btnDelete.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnAdd
            // 
            this.btnAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnAdd.Enabled = false;
            this.btnAdd.Location = new System.Drawing.Point(232, 114);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(104, 24);
            this.btnAdd.TabIndex = 13;
            this.btnAdd.Text = "추 가";
            this.btnAdd.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnSave.Enabled = false;
            this.btnSave.Location = new System.Drawing.Point(8, 114);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(104, 24);
            this.btnSave.TabIndex = 11;
            this.btnSave.Text = "저 장";
            this.btnSave.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
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
            this.uiButton1.TabIndex = 5;
            this.uiButton1.Text = "저 장";
            this.uiButton1.VisualStyle = Janus.Windows.UI.VisualStyle.Office2003;
            // 
            // uiButton2
            // 
            this.uiButton2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.uiButton2.Location = new System.Drawing.Point(8, 8);
            this.uiButton2.Name = "uiButton2";
            this.uiButton2.Size = new System.Drawing.Size(120, 24);
            this.uiButton2.TabIndex = 6;
            this.uiButton2.Text = "추 가";
            this.uiButton2.VisualStyle = Janus.Windows.UI.VisualStyle.Office2003;
            // 
            // AdvertiserControl
            // 
            this.Controls.Add(this.uiPanelUsers);
            this.Font = global::AdManagerClient.Properties.Settings.Default.Font1;
            this.Name = "AdvertiserControl";
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
            ((System.ComponentModel.ISupportInitialize)(this.grdExAdvertiserList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dvAdvertiser)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.advertiserDs)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelUserDetail)).EndInit();
            this.uiPanelUserDetail.ResumeLayout(false);
            this.uiPanelUserDetailContainer.ResumeLayout(false);
            this.pnlUserDetail.ResumeLayout(false);
            this.pnlUserDetail.PerformLayout();
            this.ResumeLayout(false);

		}
		#endregion

		#region 컨트롤 로드
		private void UserControl_Load(object sender, System.EventArgs e)
		{
			// 데이터관리용 객체생성
			dt = ((DataView)grdExAdvertiserList.DataSource).Table;  
			cm = (CurrencyManager) this.BindingContext[grdExAdvertiserList.DataSource]; 
			cm.PositionChanged += new System.EventHandler(OnGrdRowChanged); 

			// 컨트롤 초기화
			InitControl();
		}

		#endregion

		#region 컨트롤 초기화
		private void InitControl()
		{
            ProgressStart();
			InitCombo();
			Application.DoEvents();

			// 조회권한 검사
			if(menu.CanRead(MenuCode))
			{
				canRead = true;
				SearchAdvertiser();
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
				//ResetTextReadonly();
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

			if(ebAdvertiserName.Text.Trim().Length > 0) 
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

		private void InitCombo()
		{
			Init_RapCode();			
		}

		private void Init_RapCode()
		{
            /* 랩사는 고정 - 모바일
			// 랩을 조회한다.
			MediaRapCodeModel mediaRapCodeModel = new MediaRapCodeModel();
			new MediaRapCodeManager(systemModel, commonModel).GetMediaRapCodeList(mediaRapCodeModel);
			
			if (mediaRapCodeModel.ResultCD.Equals("0000"))
			{
				// 데이터셋에 셋팅
				Utility.SetDataTable(advertiserDs.MediaRap, mediaRapCodeModel.MediaRapCodeDataSet);				
			}

			// 검색조건의 콤보
			this.cbSearchRap.Items.Clear();
			// 콤보박스에 셋트할 코드목록을 담을 Item배열을 선언
			Janus.Windows.EditControls.UIComboBoxItem[] comboItems = new Janus.Windows.EditControls.UIComboBoxItem[mediaRapCodeModel.ResultCnt + 1];

			comboItems[0] = new Janus.Windows.EditControls.UIComboBoxItem("미디어렙선택","00");
			
			for(int i=0;i<mediaRapCodeModel.ResultCnt;i++)
			{
				DataRow row = advertiserDs.MediaRap.Rows[i];

				string val = row["RapCode"].ToString();
				string txt = row["RapName"].ToString();
				comboItems[i+1] = new Janus.Windows.EditControls.UIComboBoxItem(txt,val);
			}
			// 콤보에 셋트
			this.cbSearchRap.Items.AddRange(comboItems);
			this.cbSearchRap.SelectedIndex = 0;

			Application.DoEvents();
            */
            this.cbSearchRap.Items.Clear();
            DataSet ds = new DataSet("rapset");
            DataTable dt = new DataTable("rap");
            dt.Columns.Add("RapCode", typeof(string));
            dt.Columns.Add("RapName", typeof(string));
            DataRow nRow = dt.NewRow();
            nRow["RapCode"] = "1";
            nRow["RapName"] = "모바일 편성팀";
            dt.Rows.Add(nRow);
            ds.Tables.Add(dt);

            Utility.SetDataTable(advertiserDs.MediaRap, ds);
            // 검색조건의 콤보
            this.cbSearchRap.Items.Clear();
            // 콤보박스에 셋트할 코드목록을 담을 Item배열을 선언
            Janus.Windows.EditControls.UIComboBoxItem[] comboItems = new Janus.Windows.EditControls.UIComboBoxItem[2];

            comboItems[0] = new Janus.Windows.EditControls.UIComboBoxItem("미디어렙선택", "00");

            for (int i = 0; i < 1; i++)
            {
                DataRow row = advertiserDs.MediaRap.Rows[i];

                string val = row["RapCode"].ToString();
                string txt = row["RapName"].ToString();
                comboItems[i + 1] = new Janus.Windows.EditControls.UIComboBoxItem(txt, val);
            }
            // 콤보에 셋트
            this.cbSearchRap.Items.AddRange(comboItems);
            this.cbSearchRap.SelectedIndex = 1;
            this.cbSearchRap.ReadOnly = true;
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
                if (grdExAdvertiserList.RecordCount > 0)
                {
                    SetAdvertiserDetailText();
                }
                InitButton();
            }

		}

		/// <summary>
		/// 조회버튼 클릭
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnSearch_Click(object sender, System.EventArgs e)
		{
            ProgressStart();

			DisableButton();
			SearchAdvertiser();
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
			btnSave.Enabled   = true;

			IsAdding = true;

			ResetTextReadonly();
			ResetAdvertiserDetailText();

			ebAdvertiserName.Focus();
		}

		/// <summary>
		/// 저장버튼 클릭
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnSave_Click(object sender, System.EventArgs e)
		{	
			SaveAdvertiserDetail();
		}

		/// <summary>
		/// 삭제버튼 클릭
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnDelete_Click(object sender, System.EventArgs e)
		{
			DeleteAdvertiser();
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
				SearchAdvertiser();
			}
		}

		#endregion

		#region 처리메소드

		/// <summary>
		/// 사용자목록 조회
		/// </summary>
		private void SearchAdvertiser()
		{
            IsSearching = true;

			StatusMessage("광고주 정보를 조회합니다.");

			try
			{
				advertiserModel.Init();
				// 데이터모델에 전송할 내용을 셋트한다.
				if(IsNewSearchKey)
				{
					advertiserModel.SearchKey = "";
				}
				else
				{
					advertiserModel.SearchKey  = ebSearchKey.Text;
				}

                if (uiCheckBox1.Checked)
				{
					advertiserModel.SearchchkAdState_10   = "Y";
				}
				else
				{
					advertiserModel.SearchchkAdState_10   = "N";
				}


				advertiserModel.SearchRap = cbSearchRap.SelectedValue.ToString();

				// 사용자목록조회 서비스를 호출한다.
				new AdvertiserManager(systemModel,commonModel).GetAdvertiserList(advertiserModel);

				if (advertiserModel.ResultCD.Equals("0000"))
				{
					Utility.SetDataTable(advertiserDs.Advertisers, advertiserModel.AdvertiserDataSet);				
					StatusMessage(advertiserModel.ResultCnt + "건의 광고주 정보가 조회되었습니다.");
					if(canUpdate)
					{
						AddSchChoice();									
					}				
					SetAdvertiserDetailText();
				}
			}
			catch(FrameException fe)
			{
				FrameSystem.showMsgForm("광고주조회오류", new string[] {fe.ErrCode, fe.ResultMsg});
			}
			catch(Exception ex)
			{
				FrameSystem.showMsgForm("광고주조회오류",new string[] {"",ex.Message});
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
				if ( advertiserDs.Tables["Advertisers"].Rows.Count < 1 ) return;
              
				foreach (DataRow row in advertiserDs.Tables["Advertisers"].Rows)
				{					
					if(IsAdding)
					{
						cm.Position = 0;
						advertiserCode = null;						
					}
					else
					{						
						if(row["AdvertiserCode"].ToString().Equals(advertiserCode))
						{					
							cm.Position = rowIndex;
							break;								
						}
					}

					rowIndex++;
					grdExAdvertiserList.EnsureVisible();
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
		/// 사용자상세정보 저장
		/// </summary>
		private void SaveAdvertiserDetail()
		{
            //데이터 모델 초기화
            advertiserModel.Init();

			StatusMessage("광고주 정보를 저장합니다.");
			
			if(ebAdvertiserName.Text.Trim().Length == 0) 
			{				
				MessageBox.Show("광고주명이 입력되지 않았습니다.","광고주 저장", 
					MessageBoxButtons.OK, MessageBoxIcon.Information );
				ebAdvertiserName.Focus();
                return;		
			}

			try
			{   
				// 데이터모델에 전송할 내용을 셋트한다.				
				advertiserModel.AdvertiserCode = advertiserCode;		
				advertiserModel.AdvertiserName = ebAdvertiserName.Text.Trim();	
				advertiserModel.RapCode        = keyRepCode;
				advertiserModel.Comment        = ebComment.Text.Trim();

				//사용여부
				if(rbUseYn_Y.Checked)
				{
					advertiserModel.UseYn       = "Y";
				}
				else
				{
					advertiserModel.UseYn       = "N";
				}

                // 업종 [A_01]
                if (!keyJobCode.Equals(string.Empty))
                {
                    advertiserModel.JobCode = keyJobCode.ToString();
                }
				
				// 사용자 상세정보 저장 서비스를 호출한다.
				if (IsAdding)
				{
					new AdvertiserManager(systemModel,commonModel).SetAdvertiserAdd(advertiserModel);
            
                    StatusMessage("광고주 정보가 추가되었습니다.");
                    IsAdding = false;
                    ResetAdvertiserDetailText();
                }
				else
				{
					new AdvertiserManager(systemModel,commonModel).SetAdvertiserUpdate(advertiserModel);
                    StatusMessage("광고주 정보가 저장되었습니다.");
                }
                
                DisableButton();
                SearchAdvertiser();
                InitButton();
			}
			catch(FrameException fe)
			{
				FrameSystem.showMsgForm("광고주정보 저장오류", new string[] {fe.ErrCode, fe.ResultMsg});
			}
			catch(Exception ex)
			{
				FrameSystem.showMsgForm("광고주정보 저장오류",new string[] {"",ex.Message});
			}			
		}

		/// <summary>
		/// 사용자정보 삭제
		/// </summary>
		private void DeleteAdvertiser()
		{
			StatusMessage("광고주 정보를 삭제합니다.");

			if(ebAdvertiserName.Text.Trim().Length == 0) 
			{
				MessageBox.Show("삭제할 광고주 정보가 없습니다.","광고주 삭제", 
					MessageBoxButtons.OK, MessageBoxIcon.Information );
				return;
			}

			DialogResult result = MessageBox.Show("해당 광고주 정보를 삭제 하시겠습니까?","광고주 삭제",
				MessageBoxButtons.YesNo, MessageBoxIcon.Question,
				MessageBoxDefaultButton.Button2);

			if (result == DialogResult.No) return;

			try
			{
				// 데이터모델에 전송할 내용을 셋트한다.
				advertiserModel.AdvertiserCode       = advertiserCode;
				
				// 사용자 상세정보 저장 서비스를 호출한다.
				new AdvertiserManager(systemModel,commonModel).SetAdvertiserDelete(advertiserModel);

				ResetAdvertiserDetailText();
				
				StatusMessage("광고주 정보가 삭제되었습니다.");			
			    DisableButton();
                SearchAdvertiser();
                InitButton();
			}
			catch(FrameException fe)
			{
				FrameSystem.showMsgForm("광고주정보 삭제오류", new string[] {fe.ErrCode, fe.ResultMsg});
			}
			catch(Exception ex)
			{
				FrameSystem.showMsgForm("광고주정보 삭제오류",new string[] {"",ex.Message});
			}			

		}
		
		/// <summary>
		/// 사용자 상세정보의 셋트
		/// </summary>
		private void SetAdvertiserDetailText()
		{
			int curRow = cm.Position;
			if(curRow >= 0)
			{
				advertiserCode        = dt.Rows[curRow]["AdvertiserCode"].ToString();			
				ebAdvertiserName.Text = dt.Rows[curRow]["AdvertiserName"].ToString();
                keyRepCode            = dt.Rows[curRow]["RapCode"].ToString();
                ebRapName.Text        = dt.Rows[curRow]["RapName"].ToString();
				ebComment.Text        = dt.Rows[curRow]["Comment"].ToString();
				ebRegDt.Text          = dt.Rows[curRow]["RegDt"].ToString();
				ebModDt.Text          = dt.Rows[curRow]["ModDt"].ToString();
				ebRegID.Text          = dt.Rows[curRow]["RegID"].ToString();
				string UseYn          = dt.Rows[curRow]["UseYn"].ToString();
                keyJobCode            = dt.Rows[curRow]["JobCode"].ToString();          // [A_01]
                string jobNamelevel1  = dt.Rows[curRow]["JobNameLevel1"].ToString();    // [A_01]
                string jobNamelevel2  = dt.Rows[curRow]["JobNameLevel2"].ToString();    // [A_01]

                // [A_01] Level1 업종 코드를 선택한 경우는 업종명이 Level1, Level2가 같다
                if (!jobNamelevel1.Equals(jobNamelevel2))
                {
                    ebJobClassName.Text = jobNamelevel1 + " - " + jobNamelevel2;
                }
                else
                {
                    ebJobClassName.Text = jobNamelevel1;
                }

				if(UseYn.Equals("Y"))
				{
					rbUseYn_Y.Checked = true;
					rbUseYn_N.Checked = false;
				}
				else
				{
					rbUseYn_Y.Checked = false;
					rbUseYn_N.Checked = true;
				}

				if(canUpdate) 
				{
					btnSave.Enabled   = true;
					ResetTextReadonly();
				}
				else
				{
					btnSave.Enabled   = false;
					SetTextReadonly();           
				}

				IsAdding = false;
			}
			StatusMessage("준비");
		}

		private void ResetAdvertiserDetailText()
		{	
			ebAdvertiserName.Text = "";		
			ebComment.Text        = "";		
			ebRegDt.Text          = "";		
			ebModDt.Text          = "";		
			ebRegID.Text          = "";		
			rbUseYn_Y.Checked	  = true;
			rbUseYn_N.Checked	  = false;
            keyJobCode            = string.Empty;   // [A_01]
            ebJobClassName.Text   = string.Empty;   // [A_01]

			if(!IsAdding)
			{
				ebAdvertiserName.ReadOnly  = false;
				ebAdvertiserName.BackColor = Color.WhiteSmoke;
			}

			if(commonModel.UserLevel=="30")
			{
				ebRapName.Text = commonModel.RapName;
				keyRepCode     = commonModel.RapCode;
			}
			else
			{
				ebRapName.Text = "공용";
				keyRepCode     = "0";
			}
		}
		
		/// <summary>
		/// 상세정보 ReadOnly
		/// </summary>
		private void SetTextReadonly()
		{			
			ebAdvertiserName.ReadOnly   = true;
			ebComment.ReadOnly          = true;
			rbUseYn_Y.Enabled			= false;
			rbUseYn_N.Enabled			= false;			
			
			ebAdvertiserName.BackColor  = Color.WhiteSmoke;
			ebComment.BackColor         = Color.WhiteSmoke;
			btnRapSearch.Enabled        = false;
            btnJobClassSearch.Enabled   = false;    // [A_01]
		}

		/// <summary>
		/// 상세정보 수정가능케
		/// </summary>
		private void ResetTextReadonly()
		{			
			ebAdvertiserName.ReadOnly   = false;		
			ebComment.ReadOnly          = false;
			rbUseYn_Y.Enabled			= true;
			rbUseYn_N.Enabled			= true;
			ebAdvertiserName.BackColor  = Color.White;
			ebComment.BackColor         = Color.White;

			if(commonModel.UserLevel=="30")
			{			
				btnRapSearch.Enabled      = false;
                btnJobClassSearch.Enabled = false; 
			}
			else
			{
                if (uiCheckBox2.Checked)
				{
					btnRapSearch.Enabled = false;
				}
				else
				{
					btnRapSearch.Enabled = true;
				}

                // 미디어렙선택 버튼과 달리 확인할 체크박스가 없으므로 항상 true
                btnJobClassSearch.Enabled = true;   
			}						

			// 신규작성이면 아이디까지 쓰기가능
			if (IsAdding)
			{
				/*ebAdvertiserCode.ReadOnly         = true;
				ebAdvertiserCode.BackColor        = Color.White;*/
			}
		}
		#endregion

		#region 팝업을 위한 메소드

		public void SetMediaRep(string RepCode, string RepName)
		{	
			keyRepCode      = RepCode;
			ebRapName.Text  = RepName;		
        }

        public void SetJobClass(string jobClassCode, string jobClassName)
        {
            this.keyJobCode = jobClassCode;
            ebJobClassName.Text = jobClassName;
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

		private void btnRapSearch_Click(object sender, System.EventArgs e)
		{
			Advertiser_RepSearchForm rapForm = new Advertiser_RepSearchForm(this);
			rapForm.ShowDialog();            
			rapForm.Dispose();
			rapForm = null;		
		}

        /// <summary>
        /// [A_01]
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnJobClassSearch_Click(object sender, System.EventArgs e)
        {
            AdvertiserJob_pForm rapForm = new AdvertiserJob_pForm(this);
            rapForm.ShowDialog();
            rapForm.Dispose();
            rapForm = null;
        }

		private void chkAllRep_CheckedChanged(object sender, System.EventArgs e)
		{
            if (uiCheckBox2.Checked)
			{
				ebRapName.Text = "공용";
				keyRepCode     = "0";
				btnRapSearch.Enabled     = false;	
			}
			else
			{
				if(commonModel.UserLevel=="30")
				{
					ebRapName.Text = commonModel.RapName;
					keyRepCode     = commonModel.RapCode;
					btnRapSearch.Enabled     = false;
				}
				else
				{
					btnRapSearch.Enabled     = true;
				}
			}
        }

        /// <summary>
        /// [A_01]
        /// Level1, Level2 업종명을 이용하여 메인 업종명(JobName) Cell 데이터 셋팅
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void grdExAdvertiserList_FormattingRow(object sender, Janus.Windows.GridEX.RowLoadEventArgs e)
        {
            /* 이 프로젝트에서는 사용안함.
            string jobNamelevel1 = e.Row.Cells["JobNameLevel1"].Value.ToString();
            string jobNamelevel2 = e.Row.Cells["JobNameLevel2"].Value.ToString();

            if (!jobNamelevel1.Equals(jobNamelevel2))
            {
                e.Row.Cells["JobName"].Text = jobNamelevel1 + " - " + jobNamelevel2;
            }
            else
            {
                e.Row.Cells["JobName"].Text = jobNamelevel1;
            }
             **/
        }
	}
}