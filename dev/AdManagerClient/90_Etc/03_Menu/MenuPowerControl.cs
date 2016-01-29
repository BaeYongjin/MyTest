// ===============================================================================
// MenuPowerControl for Charites Project
//
// MenuPowerControl.cs
//
// 채널정보관리 컨드롤을 정의합니다. 
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
    /// 채널/메뉴별 편성현황 컨트롤
    /// </summary>
    public class MenuPowerControl : System.Windows.Forms.UserControl, IUserControl
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
        MenuModel menuModel  = new MenuModel();	// 채널/메뉴 편성현황 모델
        SchChoiceAdModel schChoiceAdModel  = new SchChoiceAdModel();	// 지정광고편성모델
		
        // 화면처리용 변수
        CurrencyManager cmUserClass        = null;					// 데이터 그리드의 변경에 따른 데이터셋 관리를 위하여			
        DataTable       dtUserClass        = null;

        bool IsAdding             = false;
        
        // 사용권한
        bool IsSearching = false; // 조회중 상세화면이 업데이트 되는 것을 방지 하기위함 2011.11.29 JH.Park
        bool canCreate			  = false;
        bool canRead			  = false;
        bool canUpdate			  = false;
        // Key
        bool IsNotLoading		       = true;					// 상세조회중이 아님
        public string keyUserClassCode     = "";
        public string keyUserClassName  = "";
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
        private Janus.Windows.UI.Dock.UIPanelGroup uiPanelMenuPower;
        private Janus.Windows.UI.Dock.UIPanel uiPanel1;
        private Janus.Windows.UI.Dock.UIPanelInnerContainer uiPanel1Container;
        private Janus.Windows.UI.Dock.UIPanel uiPanel2;
        private Janus.Windows.UI.Dock.UIPanelInnerContainer uiPanel2Container;
        private Janus.Windows.GridEX.GridEX gridEX1;
        private AdManagerClient.MenuPowerDs menuPowerDs;
        private System.Data.DataView dvUserClass;
        private System.Data.DataView dvMenuPower;
        private Janus.Windows.GridEX.GridEX grdExUserClassList;
        private Janus.Windows.UI.Dock.UIPanel uiPanelMenuPowerDetail;
        private Janus.Windows.UI.Dock.UIPanelInnerContainer uiPanelMenuPowerDetailContainer;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label lbChannelNo;
        private Janus.Windows.EditControls.UIButton btnSave;
        private System.Windows.Forms.Label lbTitle;
        private Janus.Windows.EditControls.UIButton btnAdd;
        private Janus.Windows.GridEX.EditControls.EditBox ebUserClassName;
        private Janus.Windows.GridEX.EditControls.EditBox ebUserClassCode;
        private Janus.Windows.GridEX.GridEX grdExMenuPowerList;		
        private System.ComponentModel.IContainer components;

        public MenuPowerControl()
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
            Janus.Windows.GridEX.GridEXLayout grdExUserClassList_DesignTimeLayout = new Janus.Windows.GridEX.GridEXLayout();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MenuPowerControl));
            Janus.Windows.GridEX.GridEXLayout grdExMenuPowerList_DesignTimeLayout = new Janus.Windows.GridEX.GridEXLayout();
            this.uiPM = new Janus.Windows.UI.Dock.UIPanelManager(this.components);
            this.uiPanelUsers = new Janus.Windows.UI.Dock.UIPanelGroup();
            this.uiPanelMenuPower = new Janus.Windows.UI.Dock.UIPanelGroup();
            this.uiPanel1 = new Janus.Windows.UI.Dock.UIPanel();
            this.uiPanel1Container = new Janus.Windows.UI.Dock.UIPanelInnerContainer();
            this.grdExUserClassList = new Janus.Windows.GridEX.GridEX();
            this.dvUserClass = new System.Data.DataView();
            this.menuPowerDs = new AdManagerClient.MenuPowerDs();
            this.uiPanel2 = new Janus.Windows.UI.Dock.UIPanel();
            this.uiPanel2Container = new Janus.Windows.UI.Dock.UIPanelInnerContainer();
            this.grdExMenuPowerList = new Janus.Windows.GridEX.GridEX();
            this.dvMenuPower = new System.Data.DataView();
            this.uiPanelMenuPowerDetail = new Janus.Windows.UI.Dock.UIPanel();
            this.uiPanelMenuPowerDetailContainer = new Janus.Windows.UI.Dock.UIPanelInnerContainer();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnAdd = new Janus.Windows.EditControls.UIButton();
            this.ebUserClassName = new Janus.Windows.GridEX.EditControls.EditBox();
            this.ebUserClassCode = new Janus.Windows.GridEX.EditControls.EditBox();
            this.lbChannelNo = new System.Windows.Forms.Label();
            this.btnSave = new Janus.Windows.EditControls.UIButton();
            this.lbTitle = new System.Windows.Forms.Label();
            this.gridEX1 = new Janus.Windows.GridEX.GridEX();
            this.uiButton1 = new Janus.Windows.EditControls.UIButton();
            this.uiButton2 = new Janus.Windows.EditControls.UIButton();
            ((System.ComponentModel.ISupportInitialize)(this.uiPM)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelUsers)).BeginInit();
            this.uiPanelUsers.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelMenuPower)).BeginInit();
            this.uiPanelMenuPower.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanel1)).BeginInit();
            this.uiPanel1.SuspendLayout();
            this.uiPanel1Container.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdExUserClassList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dvUserClass)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.menuPowerDs)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanel2)).BeginInit();
            this.uiPanel2.SuspendLayout();
            this.uiPanel2Container.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdExMenuPowerList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dvMenuPower)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelMenuPowerDetail)).BeginInit();
            this.uiPanelMenuPowerDetail.SuspendLayout();
            this.uiPanelMenuPowerDetailContainer.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridEX1)).BeginInit();
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
            this.uiPanelMenuPower.Id = new System.Guid("88f86479-98e4-43d4-bf03-12fd789fcfc5");
            this.uiPanelMenuPower.StaticGroup = true;
            this.uiPanel1.Id = new System.Guid("91eeb7fc-856c-4a9c-b37e-db742cfa9b2e");
            this.uiPanelMenuPower.Panels.Add(this.uiPanel1);
            this.uiPanel2.Id = new System.Guid("3d64f731-de32-42ba-a10f-ac3b5ac56703");
            this.uiPanelMenuPower.Panels.Add(this.uiPanel2);
            this.uiPanelUsers.Panels.Add(this.uiPanelMenuPower);
            this.uiPanelMenuPowerDetail.Id = new System.Guid("0ae65184-cac1-4b98-924d-9e802237e79a");
            this.uiPanelUsers.Panels.Add(this.uiPanelMenuPowerDetail);
            this.uiPM.Panels.Add(this.uiPanelUsers);
            // 
            // Design Time Panel Info:
            // 
            this.uiPM.BeginPanelInfo();
            this.uiPM.AddDockPanelInfo(new System.Guid("b162c53a-7940-47cd-a0ed-9762c59c50d8"), Janus.Windows.UI.Dock.PanelGroupStyle.HorizontalTiles, Janus.Windows.UI.Dock.PanelDockStyle.Fill, true, new System.Drawing.Size(1010, 677), true);
            this.uiPM.AddDockPanelInfo(new System.Guid("88f86479-98e4-43d4-bf03-12fd789fcfc5"), new System.Guid("b162c53a-7940-47cd-a0ed-9762c59c50d8"), Janus.Windows.UI.Dock.PanelGroupStyle.VerticalTiles, true, 527, true);
            this.uiPM.AddDockPanelInfo(new System.Guid("91eeb7fc-856c-4a9c-b37e-db742cfa9b2e"), new System.Guid("88f86479-98e4-43d4-bf03-12fd789fcfc5"), 289, true);
            this.uiPM.AddDockPanelInfo(new System.Guid("3d64f731-de32-42ba-a10f-ac3b5ac56703"), new System.Guid("88f86479-98e4-43d4-bf03-12fd789fcfc5"), 717, true);
            this.uiPM.AddDockPanelInfo(new System.Guid("0ae65184-cac1-4b98-924d-9e802237e79a"), new System.Guid("b162c53a-7940-47cd-a0ed-9762c59c50d8"), 104, true);
            this.uiPM.AddFloatingPanelInfo(new System.Guid("b162c53a-7940-47cd-a0ed-9762c59c50d8"), Janus.Windows.UI.Dock.PanelGroupStyle.HorizontalTiles, true, new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
            this.uiPM.AddFloatingPanelInfo(new System.Guid("8e3140b9-bfcb-43af-b84e-2ee6cddf51dc"), new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
            this.uiPM.AddFloatingPanelInfo(new System.Guid("b2973a57-d3e1-4d80-9387-7a6dc3332542"), new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
            this.uiPM.AddFloatingPanelInfo(new System.Guid("afb3b7d0-aa79-443c-a1e8-3baba38f7a4d"), new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
            this.uiPM.AddFloatingPanelInfo(new System.Guid("88f86479-98e4-43d4-bf03-12fd789fcfc5"), Janus.Windows.UI.Dock.PanelGroupStyle.VerticalTiles, true, new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
            this.uiPM.AddFloatingPanelInfo(new System.Guid("91eeb7fc-856c-4a9c-b37e-db742cfa9b2e"), new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
            this.uiPM.AddFloatingPanelInfo(new System.Guid("3d64f731-de32-42ba-a10f-ac3b5ac56703"), new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
            this.uiPM.AddFloatingPanelInfo(new System.Guid("609188ab-b98f-4466-8472-b8b36f1af6d5"), Janus.Windows.UI.Dock.PanelGroupStyle.HorizontalTiles, true, new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
            this.uiPM.AddFloatingPanelInfo(new System.Guid("c1ac152b-71d8-497e-a36e-0758f080f6ec"), new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
            this.uiPM.AddFloatingPanelInfo(new System.Guid("8e7f9052-77ad-49de-84a8-26a099999d9e"), new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
            this.uiPM.AddFloatingPanelInfo(new System.Guid("0ae65184-cac1-4b98-924d-9e802237e79a"), new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
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
            this.uiPanelUsers.TabStop = false;
            this.uiPanelUsers.Text = "메뉴권한관리";
            // 
            // uiPanelMenuPower
            // 
            this.uiPanelMenuPower.AllowResize = Janus.Windows.UI.InheritableBoolean.True;
            this.uiPanelMenuPower.CaptionStyle = Janus.Windows.UI.Dock.PanelCaptionStyle.Dark;
            this.uiPanelMenuPower.CaptionVisible = Janus.Windows.UI.InheritableBoolean.False;
            this.uiPanelMenuPower.GroupStyle = Janus.Windows.UI.Dock.PanelGroupStyle.VerticalTiles;
            this.uiPanelMenuPower.Location = new System.Drawing.Point(0, 22);
            this.uiPanelMenuPower.Name = "uiPanelMenuPower";
            this.uiPanelMenuPower.Size = new System.Drawing.Size(1010, 544);
            this.uiPanelMenuPower.TabIndex = 0;
            // 
            // uiPanel1
            // 
            this.uiPanel1.InnerContainer = this.uiPanel1Container;
            this.uiPanel1.Location = new System.Drawing.Point(0, 0);
            this.uiPanel1.Name = "uiPanel1";
            this.uiPanel1.Size = new System.Drawing.Size(289, 544);
            this.uiPanel1.TabIndex = 0;
            this.uiPanel1.TabStop = false;
            this.uiPanel1.Text = "사용자정보";
            // 
            // uiPanel1Container
            // 
            this.uiPanel1Container.Controls.Add(this.grdExUserClassList);
            this.uiPanel1Container.Location = new System.Drawing.Point(1, 23);
            this.uiPanel1Container.Name = "uiPanel1Container";
            this.uiPanel1Container.Size = new System.Drawing.Size(287, 520);
            this.uiPanel1Container.TabIndex = 0;
            // 
            // grdExUserClassList
            // 
            this.grdExUserClassList.AllowEdit = Janus.Windows.GridEX.InheritableBoolean.False;
            this.grdExUserClassList.AlternatingColors = true;
            this.grdExUserClassList.BorderStyle = Janus.Windows.GridEX.BorderStyle.None;
            this.grdExUserClassList.DataSource = this.dvUserClass;
            grdExUserClassList_DesignTimeLayout.LayoutString = resources.GetString("grdExUserClassList_DesignTimeLayout.LayoutString");
            this.grdExUserClassList.DesignTimeLayout = grdExUserClassList_DesignTimeLayout;
            this.grdExUserClassList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdExUserClassList.EditorsControlStyle.ButtonAppearance = Janus.Windows.GridEX.ButtonAppearance.Regular;
            this.grdExUserClassList.EmptyRows = true;
            this.grdExUserClassList.FocusCellFormatStyle.BackColor = System.Drawing.SystemColors.Highlight;
            this.grdExUserClassList.FocusCellFormatStyle.ForeColor = System.Drawing.Color.White;
            this.grdExUserClassList.FocusStyle = Janus.Windows.GridEX.FocusStyle.None;
            this.grdExUserClassList.Font = new System.Drawing.Font("굴림체", 9F);
            this.grdExUserClassList.FrozenColumns = 2;
            this.grdExUserClassList.GridLineColor = System.Drawing.Color.Silver;
            this.grdExUserClassList.GridLines = Janus.Windows.GridEX.GridLines.Vertical;
            this.grdExUserClassList.GridLineStyle = Janus.Windows.GridEX.GridLineStyle.Solid;
            this.grdExUserClassList.GroupByBoxVisible = false;
            this.grdExUserClassList.HeaderFormatStyle.TextAlignment = Janus.Windows.GridEX.TextAlignment.Center;
            this.grdExUserClassList.HideSelection = Janus.Windows.GridEX.HideSelection.HighlightInactive;
            this.grdExUserClassList.Location = new System.Drawing.Point(0, 0);
            this.grdExUserClassList.Name = "grdExUserClassList";
            this.grdExUserClassList.ScrollBars = Janus.Windows.GridEX.ScrollBars.Vertical;
            this.grdExUserClassList.SelectedInactiveFormatStyle.BackColor = System.Drawing.Color.Gold;
            this.grdExUserClassList.Size = new System.Drawing.Size(287, 520);
            this.grdExUserClassList.TabIndex = 1;
            this.grdExUserClassList.TabKeyBehavior = Janus.Windows.GridEX.TabKeyBehavior.ControlNavigation;
            this.grdExUserClassList.ThemedAreas = ((Janus.Windows.GridEX.ThemedArea)(((((((((Janus.Windows.GridEX.ThemedArea.ScrollBars | Janus.Windows.GridEX.ThemedArea.EditControls) 
            | Janus.Windows.GridEX.ThemedArea.Headers) 
            | Janus.Windows.GridEX.ThemedArea.GroupByBox) 
            | Janus.Windows.GridEX.ThemedArea.GroupRows) 
            | Janus.Windows.GridEX.ThemedArea.ControlBorder) 
            | Janus.Windows.GridEX.ThemedArea.Cards) 
            | Janus.Windows.GridEX.ThemedArea.Gridlines) 
            | Janus.Windows.GridEX.ThemedArea.CheckBoxes)));
            this.grdExUserClassList.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            this.grdExUserClassList.SelectionChanged += new System.EventHandler(this.OnGrdRowChangedUserClass);
            this.grdExUserClassList.Enter += new System.EventHandler(this.OnGrdRowChangedUserClass);
            // 
            // dvUserClass
            // 
            this.dvUserClass.Table = this.menuPowerDs.UserClass;
            // 
            // menuPowerDs
            // 
            this.menuPowerDs.DataSetName = "MenuPowerDs";
            this.menuPowerDs.Locale = new System.Globalization.CultureInfo("en-US");
            this.menuPowerDs.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // uiPanel2
            // 
            this.uiPanel2.InnerContainer = this.uiPanel2Container;
            this.uiPanel2.Location = new System.Drawing.Point(293, 0);
            this.uiPanel2.Name = "uiPanel2";
            this.uiPanel2.Size = new System.Drawing.Size(717, 544);
            this.uiPanel2.TabIndex = 0;
            this.uiPanel2.Text = "메뉴권한정보";
            // 
            // uiPanel2Container
            // 
            this.uiPanel2Container.Controls.Add(this.grdExMenuPowerList);
            this.uiPanel2Container.Location = new System.Drawing.Point(1, 23);
            this.uiPanel2Container.Name = "uiPanel2Container";
            this.uiPanel2Container.Size = new System.Drawing.Size(715, 520);
            this.uiPanel2Container.TabIndex = 0;
            // 
            // grdExMenuPowerList
            // 
            this.grdExMenuPowerList.AlternatingColors = true;
            this.grdExMenuPowerList.AutoEdit = true;
            this.grdExMenuPowerList.BorderStyle = Janus.Windows.GridEX.BorderStyle.None;
            this.grdExMenuPowerList.DataSource = this.dvMenuPower;
            grdExMenuPowerList_DesignTimeLayout.LayoutString = resources.GetString("grdExMenuPowerList_DesignTimeLayout.LayoutString");
            this.grdExMenuPowerList.DesignTimeLayout = grdExMenuPowerList_DesignTimeLayout;
            this.grdExMenuPowerList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdExMenuPowerList.EditorsControlStyle.ButtonAppearance = Janus.Windows.GridEX.ButtonAppearance.Regular;
            this.grdExMenuPowerList.EmptyRows = true;
            this.grdExMenuPowerList.FocusCellFormatStyle.BackColor = System.Drawing.SystemColors.Highlight;
            this.grdExMenuPowerList.FocusStyle = Janus.Windows.GridEX.FocusStyle.None;
            this.grdExMenuPowerList.Font = new System.Drawing.Font("굴림체", 9F);
            this.grdExMenuPowerList.FrozenColumns = 2;
            this.grdExMenuPowerList.GridLineColor = System.Drawing.Color.Silver;
            this.grdExMenuPowerList.GridLines = Janus.Windows.GridEX.GridLines.Vertical;
            this.grdExMenuPowerList.GridLineStyle = Janus.Windows.GridEX.GridLineStyle.Solid;
            this.grdExMenuPowerList.GroupByBoxVisible = false;
            this.grdExMenuPowerList.HideSelection = Janus.Windows.GridEX.HideSelection.HighlightInactive;
            this.grdExMenuPowerList.Location = new System.Drawing.Point(0, 0);
            this.grdExMenuPowerList.Name = "grdExMenuPowerList";
            this.grdExMenuPowerList.ScrollBars = Janus.Windows.GridEX.ScrollBars.Vertical;
            this.grdExMenuPowerList.SelectedInactiveFormatStyle.BackColor = System.Drawing.Color.Gold;
            this.grdExMenuPowerList.Size = new System.Drawing.Size(715, 520);
            this.grdExMenuPowerList.TabIndex = 2;
            this.grdExMenuPowerList.TabKeyBehavior = Janus.Windows.GridEX.TabKeyBehavior.ControlNavigation;
            this.grdExMenuPowerList.ThemedAreas = ((Janus.Windows.GridEX.ThemedArea)(((((((((Janus.Windows.GridEX.ThemedArea.ScrollBars | Janus.Windows.GridEX.ThemedArea.EditControls) 
            | Janus.Windows.GridEX.ThemedArea.Headers) 
            | Janus.Windows.GridEX.ThemedArea.GroupByBox) 
            | Janus.Windows.GridEX.ThemedArea.GroupRows) 
            | Janus.Windows.GridEX.ThemedArea.ControlBorder) 
            | Janus.Windows.GridEX.ThemedArea.Cards) 
            | Janus.Windows.GridEX.ThemedArea.Gridlines) 
            | Janus.Windows.GridEX.ThemedArea.CheckBoxes)));
            this.grdExMenuPowerList.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            // 
            // dvMenuPower
            // 
            this.dvMenuPower.Table = this.menuPowerDs.MenuPower;
            // 
            // uiPanelMenuPowerDetail
            // 
            this.uiPanelMenuPowerDetail.AllowResize = Janus.Windows.UI.InheritableBoolean.False;
            this.uiPanelMenuPowerDetail.InnerContainer = this.uiPanelMenuPowerDetailContainer;
            this.uiPanelMenuPowerDetail.Location = new System.Drawing.Point(0, 570);
            this.uiPanelMenuPowerDetail.Name = "uiPanelMenuPowerDetail";
            this.uiPanelMenuPowerDetail.Size = new System.Drawing.Size(1010, 107);
            this.uiPanelMenuPowerDetail.TabIndex = 0;
            this.uiPanelMenuPowerDetail.TabStop = false;
            this.uiPanelMenuPowerDetail.Text = "성세정보";
            // 
            // uiPanelMenuPowerDetailContainer
            // 
            this.uiPanelMenuPowerDetailContainer.Controls.Add(this.panel1);
            this.uiPanelMenuPowerDetailContainer.Location = new System.Drawing.Point(1, 23);
            this.uiPanelMenuPowerDetailContainer.Name = "uiPanelMenuPowerDetailContainer";
            this.uiPanelMenuPowerDetailContainer.Size = new System.Drawing.Size(1008, 83);
            this.uiPanelMenuPowerDetailContainer.TabIndex = 0;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnAdd);
            this.panel1.Controls.Add(this.ebUserClassName);
            this.panel1.Controls.Add(this.ebUserClassCode);
            this.panel1.Controls.Add(this.lbChannelNo);
            this.panel1.Controls.Add(this.btnSave);
            this.panel1.Controls.Add(this.lbTitle);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1008, 83);
            this.panel1.TabIndex = 7;
            // 
            // btnAdd
            // 
            this.btnAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnAdd.BackColor = System.Drawing.SystemColors.Window;
            this.btnAdd.Enabled = false;
            this.btnAdd.Location = new System.Drawing.Point(116, 46);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(104, 24);
            this.btnAdd.TabIndex = 6;
            this.btnAdd.Text = "추 가";
            this.btnAdd.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // ebUserClassName
            // 
            this.ebUserClassName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.ebUserClassName.Font = new System.Drawing.Font("굴림체", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.ebUserClassName.Location = new System.Drawing.Point(294, 13);
            this.ebUserClassName.MaxLength = 120;
            this.ebUserClassName.Name = "ebUserClassName";
            this.ebUserClassName.Size = new System.Drawing.Size(152, 21);
            this.ebUserClassName.TabIndex = 4;
            this.ebUserClassName.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
            this.ebUserClassName.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            // 
            // ebUserClassCode
            // 
            this.ebUserClassCode.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.ebUserClassCode.Font = new System.Drawing.Font("굴림체", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.ebUserClassCode.Location = new System.Drawing.Point(56, 13);
            this.ebUserClassCode.MaxLength = 120;
            this.ebUserClassCode.Name = "ebUserClassCode";
            this.ebUserClassCode.Size = new System.Drawing.Size(152, 21);
            this.ebUserClassCode.TabIndex = 3;
            this.ebUserClassCode.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
            this.ebUserClassCode.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            // 
            // lbChannelNo
            // 
            this.lbChannelNo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lbChannelNo.Font = new System.Drawing.Font("굴림체", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lbChannelNo.Location = new System.Drawing.Point(8, 13);
            this.lbChannelNo.Name = "lbChannelNo";
            this.lbChannelNo.Size = new System.Drawing.Size(72, 21);
            this.lbChannelNo.TabIndex = 0;
            this.lbChannelNo.Text = "코드";
            this.lbChannelNo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnSave.BackColor = System.Drawing.SystemColors.Window;
            this.btnSave.Enabled = false;
            this.btnSave.Location = new System.Drawing.Point(8, 46);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(104, 24);
            this.btnSave.TabIndex = 5;
            this.btnSave.Text = "저 장";
            this.btnSave.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // lbTitle
            // 
            this.lbTitle.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lbTitle.Font = new System.Drawing.Font("굴림체", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lbTitle.Location = new System.Drawing.Point(232, 13);
            this.lbTitle.Name = "lbTitle";
            this.lbTitle.Size = new System.Drawing.Size(72, 21);
            this.lbTitle.TabIndex = 0;
            this.lbTitle.Text = "구분명";
            this.lbTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // gridEX1
            // 
            this.gridEX1.Location = new System.Drawing.Point(0, 0);
            this.gridEX1.Name = "gridEX1";
            this.gridEX1.Size = new System.Drawing.Size(400, 376);
            this.gridEX1.TabIndex = 0;
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
            // MenuPowerControl
            // 
            this.Controls.Add(this.uiPanelUsers);
            this.Name = "MenuPowerControl";
            this.Size = new System.Drawing.Size(1010, 677);
            this.Load += new System.EventHandler(this.MenuPowerControl_Load);
            ((System.ComponentModel.ISupportInitialize)(this.uiPM)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelUsers)).EndInit();
            this.uiPanelUsers.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelMenuPower)).EndInit();
            this.uiPanelMenuPower.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.uiPanel1)).EndInit();
            this.uiPanel1.ResumeLayout(false);
            this.uiPanel1Container.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdExUserClassList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dvUserClass)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.menuPowerDs)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanel2)).EndInit();
            this.uiPanel2.ResumeLayout(false);
            this.uiPanel2Container.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdExMenuPowerList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dvMenuPower)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelMenuPowerDetail)).EndInit();
            this.uiPanelMenuPowerDetail.ResumeLayout(false);
            this.uiPanelMenuPowerDetailContainer.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridEX1)).EndInit();
            this.ResumeLayout(false);

		}
        #endregion

        #region 컨트롤 로드
        private void MenuPowerControl_Load(object sender, System.EventArgs e)
        {
            // 데이터관리용 객체생성
            dtUserClass= ((DataView)grdExUserClassList.DataSource).Table;
            cmUserClass= (CurrencyManager) this.BindingContext[grdExUserClassList.DataSource]; 
            //
            //            cmMenuPower = (CurrencyManager) this.BindingContext[grdExMenuPowerList.DataSource]; 
            //            dtMenuPower  = ((DataView)grdExMenuPowerList.DataSource).Table;
            //			
            //            // 컨트롤 초기화
            InitControl();
        }

        #endregion

        #region 컨트롤 초기화
        private void InitControl()
        {
            ProgressStart();

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

            if(canRead) SearchUserClass();
			
            InitButton();
            
            ProgressStop();
        }

	
        private void InitButton()
        {
            if(canCreate) btnAdd.Enabled = true;
            
            if(!IsAdding && ebUserClassCode.Text.Trim().Length > 0) 
            {
                if(canUpdate) btnSave.Enabled   = true;
                
            }
            if(IsAdding)
            {
                btnAdd.Enabled    = false;
                ResetTextReadonly();
            }
            else
            {
                btnAdd.Enabled    = true;
                SetTextReadonly();
            }

            Application.DoEvents();
        }

        private void DisableButton()
        {
            
            btnAdd.Enabled    = false;
            btnSave.Enabled   = false;
            
            Application.DoEvents();
        }

        #endregion

        #region 사용자 액션처리 메소드

        /// <summary>
        /// 그리드의 Row변경시
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnGrdRowChangedUserClass(object sender, System.EventArgs e) 
        {
            if (!IsSearching) // 2011.11.29 JH.Park 조회중이 아닐경우에만 동작하도록 변경
            {
                if (IsNotLoading)
                {
                    SetDetailTextUserClass();
                    InitButton();
                }
            }
        }

		
        private void OnGrdRowChangedMenuPower(object sender, System.EventArgs e) 
        {		
            //            if(IsNotLoading)
            //            {
            //                SetDetailTextChannel();
            //                InitButton();
            //            }
        }

        /// <summary>
        /// 추가버튼 클릭
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAdd_Click(object sender, System.EventArgs e)
        {
            btnAdd.Enabled    = false;
            btnSave.Enabled   = true;

            IsAdding = true;

            ResetTextReadonly();
            ResetDetailText();

            ebUserClassCode.Focus();
        }

        /// <summary>
        /// 저장버튼 클릭
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_Click(object sender, System.EventArgs e)
        {
            SaveUserClass();
        }


   
        #endregion

        #region 처리메소드

        /// <summary>
        /// 채널/메뉴 편성현황 목록 조회
        /// </summary>
        private void SearchUserClass()
        {
            StatusMessage("메뉴 정보를 조회합니다.");

            try
            {
                menuModel.Init();
                				
                // 데이터 클리어
                menuPowerDs.UserClass.Clear();
                menuPowerDs.MenuPower.Clear();   
                ResetDetailText();
                
                // 장르메뉴 조회 서비스를 호출한다.
                new MenuManager(systemModel,commonModel).GetUserClassList(menuModel);
                
                if (menuModel.ResultCD.Equals("0000"))
                {
                    Utility.SetDataTable(menuPowerDs.UserClass, menuModel.MenuDataSet);
                    StatusMessage(menuModel.ResultCnt + "건의 메뉴 정보가 조회되었습니다.");
                    if(canUpdate)
                    {
                        AddMenuPower();									
                    }	
                    grdExUserClassList.Focus();
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
        }


        private void SetDetailTextUserClass()
        {
            int curRow = cmUserClass.Position;
            
            if(curRow >= 0 )
            {
                IsNotLoading = false;	// 조회중 다시 조회되는 것을 방지함.
                try
                {
                    keyUserClassCode    = dtUserClass.Rows[curRow]["UserClassCode"].ToString();
                    keyUserClassName = dtUserClass.Rows[curRow]["UserClassName"].ToString();
                    
                    ebUserClassCode.Text = keyUserClassCode;
                    ebUserClassName.Text = keyUserClassName;

                    menuPowerDs.MenuPower.Clear();        
            
                    SearchMenuPower();
                    IsAdding = false;
                }
                finally
                {
                    IsNotLoading = true;
                }
            }
            
            StatusMessage("준비");
        }

        /// <summary>
        /// 채널목록 조회
        /// </summary>
        private void SearchMenuPower()
        {
            StatusMessage("채널목록을 조회합니다.");

            try
            {				
                menuModel.Init();
                menuModel.UserClassCode    = keyUserClassCode;

                // 채널목록조회 서비스를 호출한다.
                new MenuManager(systemModel,commonModel).GetMenuPowerList(menuModel);

                if (menuModel.ResultCD.Equals("0000"))
                {
                    Utility.SetDataTable(menuPowerDs.MenuPower, menuModel.MenuDataSet);				
                    StatusMessage(menuModel.ResultCnt + "건의 채널 정보가 조회되었습니다.");
                }
                
            }
            catch(FrameException fe)
            {
                FrameSystem.showMsgForm("채널조회오류", new string[] {fe.ErrCode, fe.ResultMsg});
            }
            catch(Exception ex)
            {
                FrameSystem.showMsgForm("채널조회오류",new string[] {"",ex.Message});
            }
        }
    
        /// <summary>
        /// 사용자정보 저장
        /// </summary>
        private void SaveUserClass()
        {
            StatusMessage("사용자정보를 저장합니다.");

            if(ebUserClassCode.Text.Trim().Length == 0) 
            {
                MessageBox.Show("사용자 코드가  입력되지 않았습니다.","사용자정보 저장", 
                    MessageBoxButtons.OK, MessageBoxIcon.Information );
                ebUserClassCode.Focus();
                return;							
            }
            if(ebUserClassCode.Text.Trim().Length > 2) 
            {
                MessageBox.Show("사용자 코드는 2자리까지 입력됩니다.다시 입력해주세요.","사용자정보 저장", 
                    MessageBoxButtons.OK, MessageBoxIcon.Information );
                ebUserClassCode.Text = "";
                ebUserClassCode.Focus();
                return;							
            }
            if(ebUserClassName.Text.Trim().Length == 0) 
            {
                MessageBox.Show("구분명이  입력되지 않았습니다.","사용자정보 저장", 
                    MessageBoxButtons.OK, MessageBoxIcon.Information );
                ebUserClassName.Focus();
                return;							
            }

            try
            {

                menuModel.UserClassCode            = ebUserClassCode.Text.Trim();
                menuModel.UserClassName         = ebUserClassName.Text.Trim();

                // 사용자정보 저장 서비스를 호출한다.
                if (IsAdding)
                {
                    new MenuManager(systemModel,commonModel).SetUserClassAdd(menuModel);
                    StatusMessage("사용자정보가 추가되었습니다.");
                    
                    ResetDetailText();
                }
                else
                {
                    //그리드의 체크박스 데이터를 데이터셋 통째로 넘긴다.
                    menuModel.MenuDataSet = menuPowerDs.Copy();
                    
                    new MenuManager(systemModel,commonModel).SetUserClassUpdate(menuModel);
                    StatusMessage("사용자정보가 저장되었습니다.");
                }
                    
                DisableButton();
                SearchUserClass();
                IsAdding = false;
                InitButton();
					
            }
            catch(FrameException fe)
            {
                FrameSystem.showMsgForm("사용자정보 저장오류", new string[] {fe.ErrCode, fe.ResultMsg});
            }
            catch(Exception ex)
            {
                FrameSystem.showMsgForm("사용자정보 저장오류",new string[] {"",ex.Message});
            }			
        }

        
        private void ResetDetailText()
        {	
            ebUserClassCode.Text = "";
            ebUserClassName.Text = "";
        }

        /// <summary>
        /// 상세정보 ReadOnly
        /// </summary>
        private void SetTextReadonly()
        {
            ebUserClassCode.ReadOnly        = true;
            ebUserClassCode.BackColor       = Color.WhiteSmoke;
        }

        /// <summary>
        /// 상세정보 수정가능케
        /// </summary>
        private void ResetTextReadonly()
        {
            ebUserClassCode.ReadOnly        = false;
            ebUserClassCode.BackColor       = Color.White;
        }


        /// <summary>
        /// 키캆을찾아 그리드 키에 해당되는로우로..
        /// </summary>
        private void AddMenuPower()
        {
            StatusMessage("키캆");		

            try
            {
                int rowIndex = 0;
                if ( menuPowerDs.Tables["UserClass"].Rows.Count < 1 ) return;
              
                foreach (DataRow row in menuPowerDs.Tables["UserClass"].Rows)
                {					
                    if(IsAdding)
                    {
                        cmUserClass.Position = 0;
                        keyUserClassCode = null;						
                    }
                    else
                    {						
                        if(row["UserClassCode"].ToString().Equals(keyUserClassCode))
                        {					
                            cmUserClass.Position = rowIndex;
                            break;								
                        }
                    }

                    rowIndex++;
                    grdExUserClassList.EnsureVisible();
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