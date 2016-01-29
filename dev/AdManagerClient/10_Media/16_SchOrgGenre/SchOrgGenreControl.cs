// ===============================================================================
// SchOrgGenreControl 
//
// SchOrgGenreControl.cs
//
// 원장르 기반 OAP 편성 관리
//
// ===============================================================================
// Release history
// 최초 작성 2015.05.28 Youngil.Yi
// ===============================================================================
// Copyright (C) 2054 Dartmedia co..
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
using AdManagerClient.Common.Args;
namespace AdManagerClient
{
    /// <summary>
    /// 원장르 기반 OAP 편성
    /// </summary>
    public class SchOrgGenreControl : System.Windows.Forms.UserControl, IUserControl
    {
        #region 사용자정의 객체 및 변수

        // 시스템 정보 : 화면공통
        private SystemModel   systemModel   = FrameSystem.oSysModel;
        private CommonModel   commonModel   = FrameSystem.oComModel;
        private Logger        log           = FrameSystem.oLog;
        private MenuPower     menu          = FrameSystem.oMenu;
        // 메뉴코드 : 보안이 필요한 화면에 필요함
        public string        menuCode		= "";

        // 사용할 정보모델
        SchOrgGenreModel schOrgGenreModel = new SchOrgGenreModel();	                // 채널/메뉴 편성현황 모델
		
		// 화면처리용 변수
        CurrencyManager cmMenu      = null;					// 데이터 그리드의 변경에 따른 데이터셋 관리를 위하여			
        DataTable       dtMenu      = null;
        
		// 사용권한
        bool IsSearching = false;       // 조회중 상세화면이 업데이트 되는 것을 방지 하기위함 
        bool IsInsert = false;          //DB저장시 Insert/Update Flag
        bool canCreate            = false;
		bool canRead			  = false;
		bool canUpdate			  = false;
		bool canDelete            = false;

		// Key
		bool IsNotLoading		       = true;					// 상세조회중이 아님
		public string keyMediaCode     = "";
		public string keyCategoryCode  = "";
		public string keyMenuCode     = "";
        bool IsNewSearchKey = true;					// 검색어입력 여부


		// 편성배포 승인상태 처리용
        private Janus.Windows.GridEX.EditControls.EditBox ebMenuName;
        private Label lbMenuName;
        private Janus.Windows.GridEX.EditControls.EditBox ebCategoryName;
        private Label lbCategoryName;
        private Janus.Windows.EditControls.UIButton btnSave;
        private Janus.Windows.EditControls.UIButton btnCancel;
        private Janus.Windows.EditControls.UIButton btnUpdate;
        private DevExpress.XtraEditors.TimeEdit teUseTime;
        private Label lbContStarDay;
        private Janus.Windows.CalendarCombo.CalendarCombo ebUseDate;
        private AdManagerClient.SchOrgGenreDs schOrgGenreDs;
        private Label label1;
        private Janus.Windows.EditControls.UIButton btnDelete;
        private Janus.Windows.GridEX.EditControls.EditBox ebSearchKey;
        private Janus.Windows.EditControls.UICheckBox chkSetDataOnly;
        private DataView dvSchOrgGenre;

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
        private Janus.Windows.EditControls.UIButton uiButton2;
        private Janus.Windows.EditControls.UIButton uiButton1;
        private Janus.Windows.UI.Dock.UIPanelGroup uiPanelUsers;
        private System.Windows.Forms.Panel pnlSearch;
        private System.Windows.Forms.Panel pnlSearchBtn;
        private Janus.Windows.EditControls.UIButton btnSearch;
        private Janus.Windows.EditControls.UIComboBox cbSearchMedia;
        private Janus.Windows.UI.Dock.UIPanelGroup uiPanelSchOrgGenre;
        private Janus.Windows.UI.Dock.UIPanel uiPanel1;
        private Janus.Windows.UI.Dock.UIPanelInnerContainer uiPanel1Container;
        private Janus.Windows.GridEX.GridEX grdExCategenList;
		private Janus.Windows.UI.Dock.UIPanel uiPanelSearch;
        private Janus.Windows.UI.Dock.UIPanelInnerContainer uiPanelSearchContainer;
        private System.Windows.Forms.Panel pnlDetail;
		private Janus.Windows.UI.Dock.UIPanel uiPanelDetail;
        private Janus.Windows.UI.Dock.UIPanelInnerContainer uiPanelDetailContainer;		
        private System.ComponentModel.IContainer components;

        public SchOrgGenreControl()
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
			Janus.Windows.GridEX.GridEXLayout grdExCategenList_DesignTimeLayout = new Janus.Windows.GridEX.GridEXLayout();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SchOrgGenreControl));
			Janus.Windows.GridEX.GridEXLayout grdExCategenList_Layout_0 = new Janus.Windows.GridEX.GridEXLayout();
			this.uiPM = new Janus.Windows.UI.Dock.UIPanelManager(this.components);
			this.uiPanelUsers = new Janus.Windows.UI.Dock.UIPanelGroup();
			this.uiPanelSearch = new Janus.Windows.UI.Dock.UIPanel();
			this.uiPanelSearchContainer = new Janus.Windows.UI.Dock.UIPanelInnerContainer();
			this.pnlSearch = new System.Windows.Forms.Panel();
			this.cbSearchMedia = new Janus.Windows.EditControls.UIComboBox();
			this.pnlSearchBtn = new System.Windows.Forms.Panel();
			this.chkSetDataOnly = new Janus.Windows.EditControls.UICheckBox();
			this.ebSearchKey = new Janus.Windows.GridEX.EditControls.EditBox();
			this.btnSearch = new Janus.Windows.EditControls.UIButton();
			this.uiPanelSchOrgGenre = new Janus.Windows.UI.Dock.UIPanelGroup();
			this.uiPanel1 = new Janus.Windows.UI.Dock.UIPanel();
			this.uiPanel1Container = new Janus.Windows.UI.Dock.UIPanelInnerContainer();
			this.grdExCategenList = new Janus.Windows.GridEX.GridEX();
			this.dvSchOrgGenre = new System.Data.DataView();
			this.schOrgGenreDs = new AdManagerClient.SchOrgGenreDs();
			this.uiPanelDetail = new Janus.Windows.UI.Dock.UIPanel();
			this.uiPanelDetailContainer = new Janus.Windows.UI.Dock.UIPanelInnerContainer();
			this.pnlDetail = new System.Windows.Forms.Panel();
			this.btnDelete = new Janus.Windows.EditControls.UIButton();
			this.teUseTime = new DevExpress.XtraEditors.TimeEdit();
			this.ebUseDate = new Janus.Windows.CalendarCombo.CalendarCombo();
			this.btnSave = new Janus.Windows.EditControls.UIButton();
			this.btnCancel = new Janus.Windows.EditControls.UIButton();
			this.btnUpdate = new Janus.Windows.EditControls.UIButton();
			this.ebMenuName = new Janus.Windows.GridEX.EditControls.EditBox();
			this.lbMenuName = new System.Windows.Forms.Label();
			this.ebCategoryName = new Janus.Windows.GridEX.EditControls.EditBox();
			this.lbCategoryName = new System.Windows.Forms.Label();
			this.lbContStarDay = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.uiButton1 = new Janus.Windows.EditControls.UIButton();
			this.uiButton2 = new Janus.Windows.EditControls.UIButton();
			((System.ComponentModel.ISupportInitialize)(this.uiPM)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.uiPanelUsers)).BeginInit();
			this.uiPanelUsers.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.uiPanelSearch)).BeginInit();
			this.uiPanelSearch.SuspendLayout();
			this.uiPanelSearchContainer.SuspendLayout();
			this.pnlSearch.SuspendLayout();
			this.pnlSearchBtn.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.uiPanelSchOrgGenre)).BeginInit();
			this.uiPanelSchOrgGenre.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.uiPanel1)).BeginInit();
			this.uiPanel1.SuspendLayout();
			this.uiPanel1Container.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.grdExCategenList)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.dvSchOrgGenre)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.schOrgGenreDs)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.uiPanelDetail)).BeginInit();
			this.uiPanelDetail.SuspendLayout();
			this.uiPanelDetailContainer.SuspendLayout();
			this.pnlDetail.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.teUseTime.Properties)).BeginInit();
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
			this.uiPanelSearch.Id = new System.Guid("afb3b7d0-aa79-443c-a1e8-3baba38f7a4d");
			this.uiPanelUsers.Panels.Add(this.uiPanelSearch);
			this.uiPanelSchOrgGenre.Id = new System.Guid("88f86479-98e4-43d4-bf03-12fd789fcfc5");
			this.uiPanelSchOrgGenre.StaticGroup = true;
			this.uiPanel1.Id = new System.Guid("91eeb7fc-856c-4a9c-b37e-db742cfa9b2e");
			this.uiPanelSchOrgGenre.Panels.Add(this.uiPanel1);
			this.uiPanelUsers.Panels.Add(this.uiPanelSchOrgGenre);
			this.uiPanelDetail.Id = new System.Guid("c1ac152b-71d8-497e-a36e-0758f080f6ec");
			this.uiPanelUsers.Panels.Add(this.uiPanelDetail);
			this.uiPM.Panels.Add(this.uiPanelUsers);
			// 
			// Design Time Panel Info:
			// 
			this.uiPM.BeginPanelInfo();
			this.uiPM.AddDockPanelInfo(new System.Guid("b162c53a-7940-47cd-a0ed-9762c59c50d8"), Janus.Windows.UI.Dock.PanelGroupStyle.HorizontalTiles, Janus.Windows.UI.Dock.PanelDockStyle.Fill, true, new System.Drawing.Size(1010, 677), true);
			this.uiPM.AddDockPanelInfo(new System.Guid("afb3b7d0-aa79-443c-a1e8-3baba38f7a4d"), new System.Guid("b162c53a-7940-47cd-a0ed-9762c59c50d8"), 31, true);
			this.uiPM.AddDockPanelInfo(new System.Guid("88f86479-98e4-43d4-bf03-12fd789fcfc5"), new System.Guid("b162c53a-7940-47cd-a0ed-9762c59c50d8"), Janus.Windows.UI.Dock.PanelGroupStyle.VerticalTiles, true, 496, true);
			this.uiPM.AddDockPanelInfo(new System.Guid("91eeb7fc-856c-4a9c-b37e-db742cfa9b2e"), new System.Guid("88f86479-98e4-43d4-bf03-12fd789fcfc5"), 419, true);
			this.uiPM.AddDockPanelInfo(new System.Guid("c1ac152b-71d8-497e-a36e-0758f080f6ec"), new System.Guid("b162c53a-7940-47cd-a0ed-9762c59c50d8"), 120, true);
			this.uiPM.AddFloatingPanelInfo(new System.Guid("b162c53a-7940-47cd-a0ed-9762c59c50d8"), Janus.Windows.UI.Dock.PanelGroupStyle.HorizontalTiles, true, new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
			this.uiPM.AddFloatingPanelInfo(new System.Guid("8e3140b9-bfcb-43af-b84e-2ee6cddf51dc"), new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
			this.uiPM.AddFloatingPanelInfo(new System.Guid("b2973a57-d3e1-4d80-9387-7a6dc3332542"), new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
			this.uiPM.AddFloatingPanelInfo(new System.Guid("afb3b7d0-aa79-443c-a1e8-3baba38f7a4d"), new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
			this.uiPM.AddFloatingPanelInfo(new System.Guid("88f86479-98e4-43d4-bf03-12fd789fcfc5"), Janus.Windows.UI.Dock.PanelGroupStyle.VerticalTiles, true, new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
			this.uiPM.AddFloatingPanelInfo(new System.Guid("91eeb7fc-856c-4a9c-b37e-db742cfa9b2e"), new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
			this.uiPM.AddFloatingPanelInfo(new System.Guid("3d64f731-de32-42ba-a10f-ac3b5ac56703"), new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
			this.uiPM.AddFloatingPanelInfo(new System.Guid("609188ab-b98f-4466-8472-b8b36f1af6d5"), Janus.Windows.UI.Dock.PanelGroupStyle.HorizontalTiles, true, new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
			this.uiPM.AddFloatingPanelInfo(new System.Guid("c1ac152b-71d8-497e-a36e-0758f080f6ec"), new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
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
			this.uiPanelUsers.TabIndex = 4;
			this.uiPanelUsers.Text = "원장르기반OAP편성";
			// 
			// uiPanelSearch
			// 
			this.uiPanelSearch.AllowResize = Janus.Windows.UI.InheritableBoolean.False;
			this.uiPanelSearch.CaptionVisible = Janus.Windows.UI.InheritableBoolean.False;
			this.uiPanelSearch.CloseButtonVisible = Janus.Windows.UI.InheritableBoolean.False;
			this.uiPanelSearch.InnerContainer = this.uiPanelSearchContainer;
			this.uiPanelSearch.Location = new System.Drawing.Point(0, 22);
			this.uiPanelSearch.Name = "uiPanelSearch";
			this.uiPanelSearch.Size = new System.Drawing.Size(1010, 40);
			this.uiPanelSearch.TabIndex = 4;
			this.uiPanelSearch.Text = "검색";
			// 
			// uiPanelSearchContainer
			// 
			this.uiPanelSearchContainer.Controls.Add(this.pnlSearch);
			this.uiPanelSearchContainer.Location = new System.Drawing.Point(1, 1);
			this.uiPanelSearchContainer.Name = "uiPanelSearchContainer";
			this.uiPanelSearchContainer.Size = new System.Drawing.Size(1008, 38);
			this.uiPanelSearchContainer.TabIndex = 0;
			// 
			// pnlSearch
			// 
			this.pnlSearch.BackColor = System.Drawing.SystemColors.Window;
			this.pnlSearch.Controls.Add(this.cbSearchMedia);
			this.pnlSearch.Controls.Add(this.pnlSearchBtn);
			this.pnlSearch.Dock = System.Windows.Forms.DockStyle.Fill;
			this.pnlSearch.Location = new System.Drawing.Point(0, 0);
			this.pnlSearch.Name = "pnlSearch";
			this.pnlSearch.Size = new System.Drawing.Size(1008, 38);
			this.pnlSearch.TabIndex = 3;
			// 
			// cbSearchMedia
			// 
			this.cbSearchMedia.ComboStyle = Janus.Windows.EditControls.ComboStyle.DropDownList;
			this.cbSearchMedia.Location = new System.Drawing.Point(8, 9);
			this.cbSearchMedia.Name = "cbSearchMedia";
			this.cbSearchMedia.Size = new System.Drawing.Size(152, 21);
			this.cbSearchMedia.TabIndex = 1;
			this.cbSearchMedia.Text = "매체선택";
			this.cbSearchMedia.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
			// 
			// pnlSearchBtn
			// 
			this.pnlSearchBtn.BackColor = System.Drawing.SystemColors.Window;
			this.pnlSearchBtn.Controls.Add(this.chkSetDataOnly);
			this.pnlSearchBtn.Controls.Add(this.ebSearchKey);
			this.pnlSearchBtn.Controls.Add(this.btnSearch);
			this.pnlSearchBtn.Dock = System.Windows.Forms.DockStyle.Right;
			this.pnlSearchBtn.Location = new System.Drawing.Point(498, 0);
			this.pnlSearchBtn.Name = "pnlSearchBtn";
			this.pnlSearchBtn.Size = new System.Drawing.Size(510, 38);
			this.pnlSearchBtn.TabIndex = 3;
			// 
			// chkSetDataOnly
			// 
			this.chkSetDataOnly.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.chkSetDataOnly.Checked = true;
			this.chkSetDataOnly.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkSetDataOnly.Location = new System.Drawing.Point(15, 9);
			this.chkSetDataOnly.Name = "chkSetDataOnly";
			this.chkSetDataOnly.Size = new System.Drawing.Size(196, 23);
			this.chkSetDataOnly.TabIndex = 48;
			this.chkSetDataOnly.Text = "원장르기반으로 설정된 메뉴만 보기";
			this.chkSetDataOnly.CheckedChanged += new System.EventHandler(this.chkSetDataOnly_CheckedChanged);
			// 
			// ebSearchKey
			// 
			this.ebSearchKey.ButtonFont = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.ebSearchKey.Location = new System.Drawing.Point(232, 9);
			this.ebSearchKey.Name = "ebSearchKey";
			this.ebSearchKey.Size = new System.Drawing.Size(147, 21);
			this.ebSearchKey.TabIndex = 6;
			this.ebSearchKey.Text = "검색어를 입력하세요";
			this.ebSearchKey.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
			this.ebSearchKey.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
			this.ebSearchKey.TextChanged += new System.EventHandler(this.ebSearchKey_TextChanged);
			this.ebSearchKey.Click += new System.EventHandler(this.ebSearchKey_Click);
			this.ebSearchKey.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ebSearchKey_KeyDown);
			// 
			// btnSearch
			// 
			this.btnSearch.Enabled = false;
			this.btnSearch.Location = new System.Drawing.Point(401, 7);
			this.btnSearch.Name = "btnSearch";
			this.btnSearch.Size = new System.Drawing.Size(93, 24);
			this.btnSearch.TabIndex = 2;
			this.btnSearch.Text = "조 회";
			this.btnSearch.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
			this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
			// 
			// uiPanelSchOrgGenre
			// 
			this.uiPanelSchOrgGenre.AllowResize = Janus.Windows.UI.InheritableBoolean.True;
			this.uiPanelSchOrgGenre.CaptionStyle = Janus.Windows.UI.Dock.PanelCaptionStyle.Dark;
			this.uiPanelSchOrgGenre.CaptionVisible = Janus.Windows.UI.InheritableBoolean.False;
			this.uiPanelSchOrgGenre.GroupStyle = Janus.Windows.UI.Dock.PanelGroupStyle.VerticalTiles;
			this.uiPanelSchOrgGenre.Location = new System.Drawing.Point(0, 66);
			this.uiPanelSchOrgGenre.Name = "uiPanelSchOrgGenre";
			this.uiPanelSchOrgGenre.Size = new System.Drawing.Size(1010, 487);
			this.uiPanelSchOrgGenre.TabIndex = 4;
			this.uiPanelSchOrgGenre.Text = "메뉴/채널 목록";
			// 
			// uiPanel1
			// 
			this.uiPanel1.CaptionStyle = Janus.Windows.UI.Dock.PanelCaptionStyle.Light;
			this.uiPanel1.CaptionVisible = Janus.Windows.UI.InheritableBoolean.False;
			this.uiPanel1.InnerContainer = this.uiPanel1Container;
			this.uiPanel1.Location = new System.Drawing.Point(0, 0);
			this.uiPanel1.Name = "uiPanel1";
			this.uiPanel1.Size = new System.Drawing.Size(1010, 487);
			this.uiPanel1.TabIndex = 4;
			this.uiPanel1.Text = "메뉴";
			// 
			// uiPanel1Container
			// 
			this.uiPanel1Container.Controls.Add(this.grdExCategenList);
			this.uiPanel1Container.Location = new System.Drawing.Point(1, 1);
			this.uiPanel1Container.Name = "uiPanel1Container";
			this.uiPanel1Container.Size = new System.Drawing.Size(1008, 485);
			this.uiPanel1Container.TabIndex = 0;
			// 
			// grdExCategenList
			// 
			this.grdExCategenList.AllowEdit = Janus.Windows.GridEX.InheritableBoolean.False;
			this.grdExCategenList.AlternatingColors = true;
			this.grdExCategenList.AlternatingRowFormatStyle.BackgroundImageDrawMode = Janus.Windows.GridEX.BackgroundImageDrawMode.Tile;
			this.grdExCategenList.AutomaticSort = false;
			this.grdExCategenList.BorderStyle = Janus.Windows.GridEX.BorderStyle.None;
			this.grdExCategenList.DataSource = this.dvSchOrgGenre;
			grdExCategenList_DesignTimeLayout.LayoutString = resources.GetString("grdExCategenList_DesignTimeLayout.LayoutString");
			this.grdExCategenList.DesignTimeLayout = grdExCategenList_DesignTimeLayout;
			this.grdExCategenList.Dock = System.Windows.Forms.DockStyle.Fill;
			this.grdExCategenList.EmptyRows = true;
			this.grdExCategenList.FocusCellFormatStyle.BackColor = System.Drawing.SystemColors.Highlight;
			this.grdExCategenList.FocusCellFormatStyle.ForeColor = System.Drawing.Color.White;
			this.grdExCategenList.FocusStyle = Janus.Windows.GridEX.FocusStyle.None;
			this.grdExCategenList.Font = new System.Drawing.Font("나눔고딕", 8.249999F);
			this.grdExCategenList.FrozenColumns = 2;
			this.grdExCategenList.GridLineColor = System.Drawing.Color.Silver;
			this.grdExCategenList.GridLines = Janus.Windows.GridEX.GridLines.Vertical;
			this.grdExCategenList.GridLineStyle = Janus.Windows.GridEX.GridLineStyle.Solid;
			this.grdExCategenList.GroupByBoxVisible = false;
			this.grdExCategenList.GroupRowVisualStyle = Janus.Windows.GridEX.GroupRowVisualStyle.Outlook2003;
			this.grdExCategenList.HeaderFormatStyle.TextAlignment = Janus.Windows.GridEX.TextAlignment.Center;
			this.grdExCategenList.HideSelection = Janus.Windows.GridEX.HideSelection.HighlightInactive;
			grdExCategenList_Layout_0.Key = "bea";
			this.grdExCategenList.Layouts.AddRange(new Janus.Windows.GridEX.GridEXLayout[] {
            grdExCategenList_Layout_0});
			this.grdExCategenList.Location = new System.Drawing.Point(0, 0);
			this.grdExCategenList.Name = "grdExCategenList";
			this.grdExCategenList.ScrollBars = Janus.Windows.GridEX.ScrollBars.Vertical;
			this.grdExCategenList.SelectedInactiveFormatStyle.BackColor = System.Drawing.Color.Gold;
			this.grdExCategenList.Size = new System.Drawing.Size(1008, 485);
			this.grdExCategenList.TabIndex = 3;
			this.grdExCategenList.TabKeyBehavior = Janus.Windows.GridEX.TabKeyBehavior.ControlNavigation;
			this.grdExCategenList.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
			this.grdExCategenList.SelectionChanged += new System.EventHandler(this.OnGrdRowChangedMenu);
			this.grdExCategenList.Click += new System.EventHandler(this.OnGrdRowChangedMenu);
			// 
			// dvSchOrgGenre
			// 
			this.dvSchOrgGenre.Table = this.schOrgGenreDs.SchOrgGenre;
			// 
			// schOrgGenreDs
			// 
			this.schOrgGenreDs.DataSetName = "SchOrgGenreDs";
			this.schOrgGenreDs.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
			// 
			// uiPanelDetail
			// 
			this.uiPanelDetail.AllowResize = Janus.Windows.UI.InheritableBoolean.False;
			this.uiPanelDetail.CaptionStyle = Janus.Windows.UI.Dock.PanelCaptionStyle.Dark;
			this.uiPanelDetail.InnerContainer = this.uiPanelDetailContainer;
			this.uiPanelDetail.Location = new System.Drawing.Point(0, 557);
			this.uiPanelDetail.Name = "uiPanelDetail";
			this.uiPanelDetail.Size = new System.Drawing.Size(1010, 120);
			this.uiPanelDetail.TabIndex = 4;
			this.uiPanelDetail.Text = "상세정보";
			// 
			// uiPanelDetailContainer
			// 
			this.uiPanelDetailContainer.Controls.Add(this.pnlDetail);
			this.uiPanelDetailContainer.Location = new System.Drawing.Point(1, 23);
			this.uiPanelDetailContainer.Name = "uiPanelDetailContainer";
			this.uiPanelDetailContainer.Size = new System.Drawing.Size(1008, 96);
			this.uiPanelDetailContainer.TabIndex = 0;
			// 
			// pnlDetail
			// 
			this.pnlDetail.BackColor = System.Drawing.SystemColors.Window;
			this.pnlDetail.Controls.Add(this.btnDelete);
			this.pnlDetail.Controls.Add(this.teUseTime);
			this.pnlDetail.Controls.Add(this.ebUseDate);
			this.pnlDetail.Controls.Add(this.btnSave);
			this.pnlDetail.Controls.Add(this.btnCancel);
			this.pnlDetail.Controls.Add(this.btnUpdate);
			this.pnlDetail.Controls.Add(this.ebMenuName);
			this.pnlDetail.Controls.Add(this.lbMenuName);
			this.pnlDetail.Controls.Add(this.ebCategoryName);
			this.pnlDetail.Controls.Add(this.lbCategoryName);
			this.pnlDetail.Controls.Add(this.lbContStarDay);
			this.pnlDetail.Controls.Add(this.label1);
			this.pnlDetail.Dock = System.Windows.Forms.DockStyle.Fill;
			this.pnlDetail.Location = new System.Drawing.Point(0, 0);
			this.pnlDetail.Name = "pnlDetail";
			this.pnlDetail.Size = new System.Drawing.Size(1008, 96);
			this.pnlDetail.TabIndex = 6;
			// 
			// btnDelete
			// 
			this.btnDelete.Enabled = false;
			this.btnDelete.Location = new System.Drawing.Point(892, 16);
			this.btnDelete.Name = "btnDelete";
			this.btnDelete.Size = new System.Drawing.Size(99, 24);
			this.btnDelete.TabIndex = 268;
			this.btnDelete.Text = "삭 제";
			this.btnDelete.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
			this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
			// 
			// teUseTime
			// 
			this.teUseTime.EditValue = new System.DateTime(2014, 9, 16, 12, 0, 0, 0);
			this.teUseTime.Enabled = false;
			this.teUseTime.Location = new System.Drawing.Point(360, 55);
			this.teUseTime.Name = "teUseTime";
			this.teUseTime.Properties.Appearance.BorderColor = System.Drawing.SystemColors.MenuHighlight;
			this.teUseTime.Properties.Appearance.Font = new System.Drawing.Font("나눔고딕", 8.249999F);
			this.teUseTime.Properties.Appearance.Options.UseBorderColor = true;
			this.teUseTime.Properties.Appearance.Options.UseFont = true;
			this.teUseTime.Properties.AppearanceFocused.BorderColor = System.Drawing.SystemColors.MenuHighlight;
			this.teUseTime.Properties.AppearanceFocused.Options.UseBorderColor = true;
			this.teUseTime.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
			this.teUseTime.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
			this.teUseTime.Properties.ButtonsStyle = DevExpress.XtraEditors.Controls.BorderStyles.Flat;
			this.teUseTime.Size = new System.Drawing.Size(195, 20);
			this.teUseTime.TabIndex = 266;
			// 
			// ebUseDate
			// 
			// 
			// 
			// 
			this.ebUseDate.DropDownCalendar.FirstMonth = new System.DateTime(2007, 8, 1, 0, 0, 0, 0);
			this.ebUseDate.DropDownCalendar.Name = "";
			this.ebUseDate.DropDownCalendar.VisualStyle = Janus.Windows.CalendarCombo.VisualStyle.Office2007;
			this.ebUseDate.Enabled = false;
			this.ebUseDate.Location = new System.Drawing.Point(80, 52);
			this.ebUseDate.Name = "ebUseDate";
			this.ebUseDate.Size = new System.Drawing.Size(183, 21);
			this.ebUseDate.TabIndex = 264;
			this.ebUseDate.Value = new System.DateTime(1, 2, 1, 0, 0, 0, 0);
			this.ebUseDate.VisualStyle = Janus.Windows.CalendarCombo.VisualStyle.Office2007;
			// 
			// btnSave
			// 
			this.btnSave.Enabled = false;
			this.btnSave.Location = new System.Drawing.Point(675, 15);
			this.btnSave.Name = "btnSave";
			this.btnSave.Size = new System.Drawing.Size(99, 24);
			this.btnSave.TabIndex = 260;
			this.btnSave.Text = "저 장";
			this.btnSave.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
			this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
			// 
			// btnCancel
			// 
			this.btnCancel.Enabled = false;
			this.btnCancel.Location = new System.Drawing.Point(785, 15);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(99, 24);
			this.btnCancel.TabIndex = 261;
			this.btnCancel.Text = "취 소";
			this.btnCancel.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
			this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
			// 
			// btnUpdate
			// 
			this.btnUpdate.Enabled = false;
			this.btnUpdate.Location = new System.Drawing.Point(566, 15);
			this.btnUpdate.Name = "btnUpdate";
			this.btnUpdate.Size = new System.Drawing.Size(99, 24);
			this.btnUpdate.TabIndex = 259;
			this.btnUpdate.Text = "수 정";
			this.btnUpdate.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
			this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
			// 
			// ebMenuName
			// 
			this.ebMenuName.Location = new System.Drawing.Point(360, 17);
			this.ebMenuName.MaxLength = 40;
			this.ebMenuName.Name = "ebMenuName";
			this.ebMenuName.ReadOnly = true;
			this.ebMenuName.Size = new System.Drawing.Size(195, 21);
			this.ebMenuName.TabIndex = 251;
			this.ebMenuName.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
			this.ebMenuName.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
			// 
			// lbMenuName
			// 
			this.lbMenuName.Location = new System.Drawing.Point(274, 20);
			this.lbMenuName.Name = "lbMenuName";
			this.lbMenuName.Size = new System.Drawing.Size(80, 16);
			this.lbMenuName.TabIndex = 252;
			this.lbMenuName.Text = "메뉴명";
			// 
			// ebCategoryName
			// 
			this.ebCategoryName.Location = new System.Drawing.Point(80, 17);
			this.ebCategoryName.MaxLength = 40;
			this.ebCategoryName.Name = "ebCategoryName";
			this.ebCategoryName.ReadOnly = true;
			this.ebCategoryName.Size = new System.Drawing.Size(183, 21);
			this.ebCategoryName.TabIndex = 248;
			this.ebCategoryName.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
			this.ebCategoryName.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
			// 
			// lbCategoryName
			// 
			this.lbCategoryName.Location = new System.Drawing.Point(8, 20);
			this.lbCategoryName.Name = "lbCategoryName";
			this.lbCategoryName.Size = new System.Drawing.Size(80, 16);
			this.lbCategoryName.TabIndex = 250;
			this.lbCategoryName.Text = "카테고리명";
			// 
			// lbContStarDay
			// 
			this.lbContStarDay.Location = new System.Drawing.Point(7, 52);
			this.lbContStarDay.Name = "lbContStarDay";
			this.lbContStarDay.Size = new System.Drawing.Size(80, 21);
			this.lbContStarDay.TabIndex = 265;
			this.lbContStarDay.Text = "적용시작일자";
			this.lbContStarDay.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(274, 52);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(80, 21);
			this.label1.TabIndex = 267;
			this.label1.Text = "적용시작시각";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// uiButton1
			// 
			this.uiButton1.Location = new System.Drawing.Point(0, 0);
			this.uiButton1.Name = "uiButton1";
			this.uiButton1.Size = new System.Drawing.Size(75, 23);
			this.uiButton1.TabIndex = 0;
			// 
			// uiButton2
			// 
			this.uiButton2.Location = new System.Drawing.Point(0, 0);
			this.uiButton2.Name = "uiButton2";
			this.uiButton2.Size = new System.Drawing.Size(75, 23);
			this.uiButton2.TabIndex = 0;
			// 
			// SchOrgGenreControl
			// 
			this.Controls.Add(this.uiPanelUsers);
			this.Font = global::AdManagerClient.Properties.Settings.Default.Font1;
			this.Name = "SchOrgGenreControl";
			this.Size = new System.Drawing.Size(1010, 677);
			this.Load += new System.EventHandler(this.SchOrgGenreControl_Load);
			((System.ComponentModel.ISupportInitialize)(this.uiPM)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.uiPanelUsers)).EndInit();
			this.uiPanelUsers.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.uiPanelSearch)).EndInit();
			this.uiPanelSearch.ResumeLayout(false);
			this.uiPanelSearchContainer.ResumeLayout(false);
			this.pnlSearch.ResumeLayout(false);
			this.pnlSearchBtn.ResumeLayout(false);
			this.pnlSearchBtn.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.uiPanelSchOrgGenre)).EndInit();
			this.uiPanelSchOrgGenre.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.uiPanel1)).EndInit();
			this.uiPanel1.ResumeLayout(false);
			this.uiPanel1Container.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.grdExCategenList)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.dvSchOrgGenre)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.schOrgGenreDs)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.uiPanelDetail)).EndInit();
			this.uiPanelDetail.ResumeLayout(false);
			this.uiPanelDetailContainer.ResumeLayout(false);
			this.pnlDetail.ResumeLayout(false);
			this.pnlDetail.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.teUseTime.Properties)).EndInit();
			this.ResumeLayout(false);

        }
        #endregion

        #region 컨트롤 로드
        private void SchOrgGenreControl_Load(object sender, System.EventArgs e)
        {
            // 데이터관리용 객체생성
            dtMenu= ((DataView)grdExCategenList.DataSource).Table;
            cmMenu= (CurrencyManager) this.BindingContext[grdExCategenList.DataSource];
            ebUseDate.Text = "";
            teUseTime.Text = "";
            
			// 컨트롤 초기화
			InitControl();
		}

        #endregion

        #region 컨트롤 초기화
        private void InitControl()
        {
            ProgressStart();

            InitCombo_Media();
            InitCombo_Level();

			// 추가권한 검사
			if(menu.CanCreate(MenuCode))
			{
				canCreate = true;
			}

			// 조회권한 검사
            if(menu.CanRead(MenuCode))
            {
                canRead = true;
            }

			// 수정권한 검사
			if(menu.CanUpdate(MenuCode))
			{
				canUpdate = true;
			}

			// 삭제권한 검사
			if(menu.CanDelete(MenuCode))
			{
				canDelete = true;
			}

			InitButton();

			ProgressStop();

   
            if (canRead)
            {
                SearchMenu();
                OnGrdRowChangedMenu(null, null);
            }


        }

        private void InitCombo_Media()
        {			
            MediaCodeModel mediacodeModel = new MediaCodeModel();		
            new MediaCodeManager(systemModel, commonModel).GetMediaCodeList(mediacodeModel);
			
            if (mediacodeModel.ResultCD.Equals("0000"))
            {
                // 데이터셋에 셋팅
                Utility.SetDataTable(schOrgGenreDs.Medias, mediacodeModel.MediaCodeDataSet);				
            }

            // 검색조건의 콤보
            this.cbSearchMedia.Items.Clear();
			
            // 콤보박스에 셋트할 코드목록을 담을 Item배열을 선언
            Janus.Windows.EditControls.UIComboBoxItem[] comboItems = new Janus.Windows.EditControls.UIComboBoxItem[mediacodeModel.ResultCnt + 1];

            comboItems[0] = new Janus.Windows.EditControls.UIComboBoxItem("매체선택","00");
			
            for(int i=0;i<mediacodeModel.ResultCnt;i++)
            {
                DataRow row = schOrgGenreDs.Medias.Rows[i];

                string val = row["MediaCode"].ToString();
                string txt = row["MediaName"].ToString();
                comboItems[i+1] = new Janus.Windows.EditControls.UIComboBoxItem(txt,val);
            }
            // 콤보에 셋트
            this.cbSearchMedia.Items.AddRange(comboItems);
            this.cbSearchMedia.SelectedIndex = 0;

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
				for(int i=0;i < schOrgGenreDs.Medias.Rows.Count;i++)
				{
					DataRow row = schOrgGenreDs.Medias.Rows[i];					
					if(row["MediaCode"].ToString().Equals(FrameSystem._HANATV.ToString()))
					{
						cbSearchMedia.SelectedValue = FrameSystem._HANATV; // 하나TV를 기본값으로 한다.	 		
						break;															
					}
					else
					{
						cbSearchMedia.SelectedValue="00";
					}
				}	
			}
            	
		
            Application.DoEvents();
        }

        private void InitButton()
        {
            if (canRead) btnSearch.Enabled = true;

            Application.DoEvents();
        }

        private void DisableButton()
        {
            btnSearch.Enabled = false;
            btnUpdate.Enabled = false;
    		btnSave.Enabled = false;
            btnCancel.Enabled = false;
            btnDelete.Enabled = false;
            Application.DoEvents();
        }

        #endregion

        #region 사용자 액션처리 메소드

        /// <summary>
        /// 메뉴 SelectedRow가 변경될때 처리함수
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnGrdRowChangedMenu(object sender, System.EventArgs e) 
        {
            if (!IsSearching) 
            {
                if (IsNotLoading)
                {
                    InitButton();
                    SetDetailTextMenu();
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
            ResetDetail();
            DisableButton();
            SearchMenu();			
            OnGrdRowChangedMenu(sender, e);
        }

        #endregion

        #region 처리메소드


        /// <summary>
        /// 슬롯 세팅 현황 목록 조회
        /// </summary>
        private void SearchMenu()
        {
            IsSearching = true;

            StatusMessage("메뉴 정보를 조회합니다.");

			if(cbSearchMedia.SelectedItem.Value.Equals("00")) 
			{
				MessageBox.Show("매체를 선택하여 주시기 바랍니다.","광고 슬롯 현황 조회",MessageBoxButtons.OK, MessageBoxIcon.Information );
				return;
			}

            try
            {
                ProgressStart();
			
                schOrgGenreModel.Init();
				
				// 데이터 클리어
				schOrgGenreDs.SchOrgGenre.Clear();
				//schOrgGenreDs.SchOrgGenre.Clear();  
				ResetDetail();

                // 데이터모델에 전송할 내용을 셋트한다.				
                schOrgGenreModel.SearchMediaCode = cbSearchMedia.SelectedValue.ToString();

                if (IsNewSearchKey) schOrgGenreModel.SearchKey = "";
                else schOrgGenreModel.SearchKey = ebSearchKey.Text.Trim();

                //세팅된 메뉴만 보기 체크판단
                schOrgGenreModel.IsSetDataOnly = chkSetDataOnly.Checked;
                
                // 슬롯 세팅 현황 목록 조회 서비스를 호출한다.
                new SchOrgGenreManager(systemModel,commonModel).GetMenuList(schOrgGenreModel);

                if (schOrgGenreModel.ResultCD.Equals("0000"))
                {
                    Utility.SetDataTable(schOrgGenreDs.SchOrgGenre, schOrgGenreModel.SchOrgGenreDataSet);
                    StatusMessage(schOrgGenreModel.ResultCnt + "건의 메뉴 정보가 조회되었습니다.");
//                    if (schOrgGenreModel.ResultCnt > 0) cmMenu.Position = 0;

					grdExCategenList.Focus();
                     
				}
            }
            catch(FrameException fe)
            {
                FrameSystem.showMsgForm("메뉴 조회오류", new string[] {fe.ErrCode, fe.ResultMsg});
            }
            catch(Exception ex)
            {
                FrameSystem.showMsgForm("메뉴 조회오류",new string[] {"",ex.Message});
            }
			finally
			{
                IsSearching = false; // 조회중 Flag 리셋
				ProgressStop();
			}
 
        }

		/// <summary>
		/// 메뉴선택변경시 처리, 채널리스트및 메뉴편성분 조회
		/// </summary>
		private void SetDetailTextMenu()
		{

			int curRow = cmMenu.Position;
            if (curRow >= 0)
            {
                IsNotLoading = false;	// 조회중 다시 조회되는 것을 방지함.
                try
                {
                    btnSave.Enabled = false;
                    btnCancel.Enabled = false;

                    uiPanelDetail.Text = ""
                        + "[원장르기반OAP편성] "
                        + dtMenu.Rows[curRow]["CategoryName"].ToString().Trim() + "||"
                        + dtMenu.Rows[curRow]["MenuName"].ToString().Trim();

                    keyCategoryCode = dtMenu.Rows[curRow]["CategoryCode"].ToString();
                    keyMenuCode = dtMenu.Rows[curRow]["MenuCode"].ToString();

                    ebCategoryName.Text = dtMenu.Rows[curRow]["CategoryName"].ToString();
                    ebMenuName.Text = dtMenu.Rows[curRow]["MenuName"].ToString();


                    DateTime useDate = new DateTime(0);
                    if (dtMenu.Rows[curRow]["UseDate"].ToString().Length > 0)
                    {
                        useDate = Convert.ToDateTime(dtMenu.Rows[curRow]["UseDate"].ToString());
                        IsInsert = false;
                    }
                    else
                    {
                        IsInsert = true;
                    }

                    ebUseDate.Value = useDate;
                    teUseTime.EditValue = useDate.ToLongTimeString();
                    
                    

                    if (canUpdate) btnUpdate.Enabled = true;

                    if (canDelete && !IsInsert)  //삭제권한이 있고 유동광고 슬룻 정보가 세팅 되어 있는경우 삭제 버튼 활성화 
                        btnDelete.Enabled = true;
                    else                            //그외는 삭제 버튼 비활성화
                        btnDelete.Enabled = false;

                }
                catch (FormatException fe)
                {

                    FrameSystem.showMsgForm("데이터변환 오류", new string[] { fe.Message });
                }
                finally
                {
                    IsNotLoading = true;
                }
            }
            else
            {
                uiPanelDetail.Text = "상세정보";

                ebCategoryName.Text = "";
                ebMenuName.Text = "";

                ebUseDate.Text = "";
                teUseTime.Text = "";

                ebUseDate.Enabled = false;
                teUseTime.Enabled = false;

                btnUpdate.Enabled = false;
                btnDelete.Enabled = false;
                btnSave.Enabled = false;
                btnCancel.Enabled = false;
            }


			StatusMessage("준비");
		}
    
        private void ResetDetail()
        {	
//			uiPanelDetail.Text = "광고슬롯";

            btnUpdate.Enabled = false;
            btnSave.Enabled = false;
            btnCancel.Enabled = false;
            btnDelete.Enabled = false;

            ebUseDate.Enabled = false;
            teUseTime.Enabled = false;

        }

        public void ReloadList()
        {
            SetDetailTextMenu();
        }

        /// <summary>
        /// 원장르기반 OAP 편성 저장
        /// </summary>
        private void SaveFlexSlotInfo()
        {
            StatusMessage("원장르기반 OAP 편성를 저장합니다.");

            if (ebUseDate.Value < DateTime.Today)
            {
                MessageBox.Show("적용 시작 일자는 현재일 이후로 입력해주시기 바랍니다.", "원장르기반 OAP 편성 저장", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ebUseDate.Focus();
                return;
            }

            try
            {
                
                schOrgGenreModel.Init();
                schOrgGenreModel.CategoryCode = keyCategoryCode;
                schOrgGenreModel.MenuCode = keyMenuCode;
                
                schOrgGenreModel.UseDate = ebUseDate.Text + ' ' + teUseTime.Text;

                
                //상세정보 저장 서비스 호출
                if (IsInsert)
                {
                    new SchOrgGenreManager(systemModel, commonModel).InsertSchOrgGenre(schOrgGenreModel);
                }
                else
                {
                    new SchOrgGenreManager(systemModel, commonModel).UpdateSchOrgGenre(schOrgGenreModel);
                }

                if (schOrgGenreModel.ResultCD.Equals("0000"))
                {

                    StatusMessage("원장르기반 OAP 편성가 저장되었습니다.");
                    SearchMenu();
                    InitButton();
                    ScrollToCurrent();
                    OnGrdRowChangedMenu(null, null);

                }
                 
            }
            catch (FrameException fe)
            {
                FrameSystem.showMsgForm("원장르기반 OAP 편성 저장 오류", new string[] { fe.ErrCode, fe.ResultMsg });
            }
            catch (Exception ex)
            {
                FrameSystem.showMsgForm("원장르기반 OAP 편성 저장 오류", new string[] { "", ex.Message });
            }
        }

        /// <summary>
        /// 원장르기반 OAP 편성 삭제
        /// </summary>
        private void DeleteFlexSlotInfo()
        {
            StatusMessage("원장르기반 OAP 편성를 삭제합니다.");

            try
            {

                schOrgGenreModel.Init();
                schOrgGenreModel.CategoryCode = keyCategoryCode;
                schOrgGenreModel.MenuCode = keyMenuCode;
                

                //상세정보 삭제 서비스 호출
                new SchOrgGenreManager(systemModel, commonModel).DeleteSchOrgGenre(schOrgGenreModel);
                
                if (schOrgGenreModel.ResultCD.Equals("0000"))
                {

                    StatusMessage("원장르기반 OAP 편성가 삭제되었습니다.");
                    SearchMenu();
                    InitButton();
                    ScrollToCurrent();
                    grdExCategenList.Focus();
                    OnGrdRowChangedMenu(null, null);
                }

            }
            catch (FrameException fe)
            {
                FrameSystem.showMsgForm("원장르기반 OAP 편성 저장 오류", new string[] { fe.ErrCode, fe.ResultMsg });
            }
            catch (Exception ex)
            {
                FrameSystem.showMsgForm("원장르기반 OAP 편성 저장 오류", new string[] { "", ex.Message });
            }
        }

        /// <summary>
        /// 키캆을찾아 그리드 키에 해당되는로우로..
        /// </summary>
        private void ScrollToCurrent()
        {
            try
            {
                if (dtMenu.Rows.Count < 1) return;
                if (keyCategoryCode.Length == 0 || keyMenuCode == "") return;

                int rowIndex = 0;

                foreach (DataRow row in dtMenu.Rows)
                {
                    if (row["CategoryCode"].ToString().Equals(keyCategoryCode))
                    {
                        if (row["MenuCode"].ToString().Equals(keyMenuCode))
                        {
                            cmMenu.Position = rowIndex;
                            break;
                        }
                    }
                    rowIndex++;
                }
                grdExCategenList.EnsureVisible();
            }
            catch (FrameException fe)
            {
                FrameSystem.showMsgForm("키값오류", new string[] { fe.ErrCode, fe.ResultMsg });
            }
            catch (Exception ex)
            {
                FrameSystem.showMsgForm("키값오류", new string[] { "", ex.Message });
            }
        }

        #endregion

        #region 이벤트함수

		public event StatusEventHandler 			StatusEvent;			// 상태이벤트 핸들러
		public event ProgressEventHandler 			ProgressEvent;			// 처리중이벤트 핸들러

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

        /// <summary>
        /// 저장버튼 클릭
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_Click(object sender, EventArgs e)
        {
             SaveFlexSlotInfo();
            
             
        }

        /// <summary>
        /// 수정버튼 클릭
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            ebUseDate.Enabled = true;
            teUseTime.Enabled = true;

            btnUpdate.Enabled = false;
            btnDelete.Enabled = false;
            btnSave.Enabled = true;
            btnCancel.Enabled = true;


            //값이 이미 설정되어 있는지 여부
            if (IsInsert)
            {
                //값이 처음 설정되는 경우 기본값을 설정해준다.
                ebUseDate.Text     = DateTime.Now.ToLocalTime().ToLongDateString();
                teUseTime.EditValue = DateTime.Now.ToLocalTime().ToLongTimeString();   
            }
        }

        /// <summary>
        /// 취소버튼 클릭
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            ResetDetail();
            OnGrdRowChangedMenu(sender, e);
        }

        /// <summary>
        /// 삭제버튼 클릭
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (keyMenuCode.Trim().Length == 0)
            {
                MessageBox.Show("삭제할 원장르기반 OAP 편성가 없습니다.", "원장르기반 OAP 편성 삭제",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            DialogResult result = MessageBox.Show("해당 메뉴의 원장르기반 OAP 편성를 삭제 하시겠습니까?", "원장르기반 OAP 편성 삭제",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question,
                MessageBoxDefaultButton.Button2);

            if (result == DialogResult.No) return;

            
            DeleteFlexSlotInfo();

            
        }



        /// <summary>
        /// 검색어 변경
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// 
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

        /// <summary>
        /// 검색어 클릭 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ebSearchKey_TextChanged(object sender, System.EventArgs e)
        {
            IsNewSearchKey = false;
        }

        private void ebSearchKey_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnSearch_Click(null, null);
            }
        }
       #endregion

        private void chkSetDataOnly_CheckedChanged(object sender, EventArgs e)
        {
            btnSearch_Click(null, null);
        }

                
    }
}