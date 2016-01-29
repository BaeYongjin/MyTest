// ===============================================================================
// SlotAdInfoControl 
//
// SlotAdInfoControl.cs
//
// 광고 슬롯 정보 관리
//
// ===============================================================================
// Release history
//
// ===============================================================================
// Copyright (C) 2014 Dartmedia co..
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
    /// 광고 슬롯 정보 관리 컨트롤
    /// </summary>
    public class SlotAdInfoControl : System.Windows.Forms.UserControl, IUserControl
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
        SlotAdInfoModel slotAdInfoModel = new SlotAdInfoModel();	                // 채널/메뉴 편성현황 모델
		SchChoiceAdModel schChoiceAdModel			= new SchChoiceAdModel();		// 지정광고편성모델
		SchPublishModel schPublishModel				= new SchPublishModel();		// 광고승인모델
		
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

        //광고 슬롯 정보 기본값
        public int defaultMaxCount = 3;
        public int defaultMaxTime = 60;
        public int defaultMaxCountPay = 2;
        public int defaultMaxTimePay = 30;
        public string defaultUseYn = "Y";
        public string defaultPromotionYn = "Y";
		
		
		// 편성배포 승인상태 처리용
        private Label label58;
        private Label label2;
        private Janus.Windows.GridEX.EditControls.EditBox ebMenuName;
        private Label lbMenuName;
        private Janus.Windows.GridEX.EditControls.EditBox ebCategoryName;
        private Label lbCategoryName;
        private Janus.Windows.EditControls.UIGroupBox uiGroupBox1;
        private Label label3;
        private Janus.Windows.EditControls.UIGroupBox uiGroupBox2;
        private Janus.Windows.GridEX.EditControls.IntegerUpDown udMaxTimePay;
        private Janus.Windows.GridEX.EditControls.IntegerUpDown udMaxCountPay;
        private Label label4;
        private Label label5;
        private Janus.Windows.GridEX.EditControls.IntegerUpDown udMaxTime;
        private Janus.Windows.GridEX.EditControls.IntegerUpDown udMaxCount;
        private Janus.Windows.EditControls.UIButton btnSave;
        private Janus.Windows.EditControls.UIButton btnCancel;
        private Janus.Windows.EditControls.UIButton btnUpdate;
        private AdManagerClient._10_Media._15_SlotAdInfo.SlotAdInfoDs slotAdInfoDs;
        private Label label1;
        private Janus.Windows.EditControls.UIButton btnDelete;
        private Janus.Windows.EditControls.UICheckBox chkSetDataOnly;
        private Label lbMsg;
        private Janus.Windows.EditControls.UICheckBox chkUseYn;
        private Janus.Windows.EditControls.UICheckBox chkPromotionYn;
        private Label label6;
        private DataView dvCateGen;

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
        private Janus.Windows.UI.Dock.UIPanelGroup uiPanelSlotAdInfo;
        private Janus.Windows.UI.Dock.UIPanel uiPanel1;
        private Janus.Windows.UI.Dock.UIPanelInnerContainer uiPanel1Container;
        private Janus.Windows.GridEX.GridEX grdExCategenList;
		private Janus.Windows.UI.Dock.UIPanel uiPanelSearch;
        private Janus.Windows.UI.Dock.UIPanelInnerContainer uiPanelSearchContainer;
        private System.Windows.Forms.Panel pnlDetail;
		private Janus.Windows.UI.Dock.UIPanel uiPanelDetail;
        private Janus.Windows.UI.Dock.UIPanelInnerContainer uiPanelDetailContainer;		
        private System.ComponentModel.IContainer components;

        public SlotAdInfoControl()
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
            Janus.Windows.Common.Layouts.JanusLayoutReference grdExCategenList_DesignTimeLayout_Reference_0 = new Janus.Windows.Common.Layouts.JanusLayoutReference("GridEXLayoutData.RootTable.FormatConditions.Condition1.FormatStyle.BackgroundImag" +
        "e");
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SlotAdInfoControl));
            Janus.Windows.Common.Layouts.JanusLayoutReference grdExCategenList_DesignTimeLayout_Reference_1 = new Janus.Windows.Common.Layouts.JanusLayoutReference("GridEXLayoutData.RootTable.FormatConditions.Condition2.FormatStyle.BackgroundImag" +
        "e");
            Janus.Windows.Common.Layouts.JanusLayoutReference grdExCategenList_DesignTimeLayout_Reference_2 = new Janus.Windows.Common.Layouts.JanusLayoutReference("GridEXLayoutData.RootTable.FormatConditions.Condition3.FormatStyle.BackgroundImag" +
        "e");
            Janus.Windows.Common.Layouts.JanusLayoutReference grdExCategenList_DesignTimeLayout_Reference_3 = new Janus.Windows.Common.Layouts.JanusLayoutReference("GridEXLayoutData.RootTable.FormatConditions.Condition4.FormatStyle.BackgroundImag" +
        "e");
            Janus.Windows.GridEX.GridEXLayout grdExCategenList_Layout_0 = new Janus.Windows.GridEX.GridEXLayout();
            this.uiPM = new Janus.Windows.UI.Dock.UIPanelManager(this.components);
            this.uiPanelUsers = new Janus.Windows.UI.Dock.UIPanelGroup();
            this.uiPanelSearch = new Janus.Windows.UI.Dock.UIPanel();
            this.uiPanelSearchContainer = new Janus.Windows.UI.Dock.UIPanelInnerContainer();
            this.pnlSearch = new System.Windows.Forms.Panel();
            this.lbMsg = new System.Windows.Forms.Label();
            this.chkSetDataOnly = new Janus.Windows.EditControls.UICheckBox();
            this.cbSearchMedia = new Janus.Windows.EditControls.UIComboBox();
            this.pnlSearchBtn = new System.Windows.Forms.Panel();
            this.btnSearch = new Janus.Windows.EditControls.UIButton();
            this.uiPanelSlotAdInfo = new Janus.Windows.UI.Dock.UIPanelGroup();
            this.uiPanel1 = new Janus.Windows.UI.Dock.UIPanel();
            this.uiPanel1Container = new Janus.Windows.UI.Dock.UIPanelInnerContainer();
            this.grdExCategenList = new Janus.Windows.GridEX.GridEX();
            this.dvCateGen = new System.Data.DataView();
            this.slotAdInfoDs = new AdManagerClient._10_Media._15_SlotAdInfo.SlotAdInfoDs();
            this.uiPanelDetail = new Janus.Windows.UI.Dock.UIPanel();
            this.uiPanelDetailContainer = new Janus.Windows.UI.Dock.UIPanelInnerContainer();
            this.pnlDetail = new System.Windows.Forms.Panel();
            this.chkUseYn = new Janus.Windows.EditControls.UICheckBox();
            this.btnDelete = new Janus.Windows.EditControls.UIButton();
            this.label1 = new System.Windows.Forms.Label();
            this.btnSave = new Janus.Windows.EditControls.UIButton();
            this.uiGroupBox2 = new Janus.Windows.EditControls.UIGroupBox();
            this.udMaxTimePay = new Janus.Windows.GridEX.EditControls.IntegerUpDown();
            this.udMaxCountPay = new Janus.Windows.GridEX.EditControls.IntegerUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.btnCancel = new Janus.Windows.EditControls.UIButton();
            this.uiGroupBox1 = new Janus.Windows.EditControls.UIGroupBox();
            this.udMaxTime = new Janus.Windows.GridEX.EditControls.IntegerUpDown();
            this.udMaxCount = new Janus.Windows.GridEX.EditControls.IntegerUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.btnUpdate = new Janus.Windows.EditControls.UIButton();
            this.ebMenuName = new Janus.Windows.GridEX.EditControls.EditBox();
            this.lbMenuName = new System.Windows.Forms.Label();
            this.ebCategoryName = new Janus.Windows.GridEX.EditControls.EditBox();
            this.lbCategoryName = new System.Windows.Forms.Label();
            this.label58 = new System.Windows.Forms.Label();
            this.uiButton1 = new Janus.Windows.EditControls.UIButton();
            this.uiButton2 = new Janus.Windows.EditControls.UIButton();
            this.label6 = new System.Windows.Forms.Label();
            this.chkPromotionYn = new Janus.Windows.EditControls.UICheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.uiPM)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelUsers)).BeginInit();
            this.uiPanelUsers.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelSearch)).BeginInit();
            this.uiPanelSearch.SuspendLayout();
            this.uiPanelSearchContainer.SuspendLayout();
            this.pnlSearch.SuspendLayout();
            this.pnlSearchBtn.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelSlotAdInfo)).BeginInit();
            this.uiPanelSlotAdInfo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanel1)).BeginInit();
            this.uiPanel1.SuspendLayout();
            this.uiPanel1Container.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdExCategenList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dvCateGen)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.slotAdInfoDs)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelDetail)).BeginInit();
            this.uiPanelDetail.SuspendLayout();
            this.uiPanelDetailContainer.SuspendLayout();
            this.pnlDetail.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uiGroupBox2)).BeginInit();
            this.uiGroupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uiGroupBox1)).BeginInit();
            this.uiGroupBox1.SuspendLayout();
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
            this.uiPanelSlotAdInfo.Id = new System.Guid("88f86479-98e4-43d4-bf03-12fd789fcfc5");
            this.uiPanelSlotAdInfo.StaticGroup = true;
            this.uiPanel1.Id = new System.Guid("91eeb7fc-856c-4a9c-b37e-db742cfa9b2e");
            this.uiPanelSlotAdInfo.Panels.Add(this.uiPanel1);
            this.uiPanelUsers.Panels.Add(this.uiPanelSlotAdInfo);
            this.uiPanelDetail.Id = new System.Guid("c1ac152b-71d8-497e-a36e-0758f080f6ec");
            this.uiPanelUsers.Panels.Add(this.uiPanelDetail);
            this.uiPM.Panels.Add(this.uiPanelUsers);
            // 
            // Design Time Panel Info:
            // 
            this.uiPM.BeginPanelInfo();
            this.uiPM.AddDockPanelInfo(new System.Guid("b162c53a-7940-47cd-a0ed-9762c59c50d8"), Janus.Windows.UI.Dock.PanelGroupStyle.HorizontalTiles, Janus.Windows.UI.Dock.PanelDockStyle.Fill, true, new System.Drawing.Size(1010, 677), true);
            this.uiPM.AddDockPanelInfo(new System.Guid("afb3b7d0-aa79-443c-a1e8-3baba38f7a4d"), new System.Guid("b162c53a-7940-47cd-a0ed-9762c59c50d8"), 31, true);
            this.uiPM.AddDockPanelInfo(new System.Guid("88f86479-98e4-43d4-bf03-12fd789fcfc5"), new System.Guid("b162c53a-7940-47cd-a0ed-9762c59c50d8"), Janus.Windows.UI.Dock.PanelGroupStyle.VerticalTiles, true, 434, true);
            this.uiPM.AddDockPanelInfo(new System.Guid("91eeb7fc-856c-4a9c-b37e-db742cfa9b2e"), new System.Guid("88f86479-98e4-43d4-bf03-12fd789fcfc5"), 419, true);
            this.uiPM.AddDockPanelInfo(new System.Guid("c1ac152b-71d8-497e-a36e-0758f080f6ec"), new System.Guid("b162c53a-7940-47cd-a0ed-9762c59c50d8"), 182, true);
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
            this.uiPanelUsers.Text = "광고슬롯관리";
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
            this.pnlSearch.Controls.Add(this.lbMsg);
            this.pnlSearch.Controls.Add(this.chkSetDataOnly);
            this.pnlSearch.Controls.Add(this.cbSearchMedia);
            this.pnlSearch.Controls.Add(this.pnlSearchBtn);
            this.pnlSearch.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlSearch.Location = new System.Drawing.Point(0, 0);
            this.pnlSearch.Name = "pnlSearch";
            this.pnlSearch.Size = new System.Drawing.Size(1008, 38);
            this.pnlSearch.TabIndex = 3;
            // 
            // lbMsg
            // 
            this.lbMsg.ForeColor = System.Drawing.Color.Blue;
            this.lbMsg.Location = new System.Drawing.Point(166, 10);
            this.lbMsg.Name = "lbMsg";
            this.lbMsg.Size = new System.Drawing.Size(494, 18);
            this.lbMsg.TabIndex = 48;
            this.lbMsg.Text = "광고슬롯정보";
            this.lbMsg.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // chkSetDataOnly
            // 
            this.chkSetDataOnly.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.chkSetDataOnly.Checked = true;
            this.chkSetDataOnly.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkSetDataOnly.Location = new System.Drawing.Point(665, 9);
            this.chkSetDataOnly.Name = "chkSetDataOnly";
            this.chkSetDataOnly.Size = new System.Drawing.Size(207, 23);
            this.chkSetDataOnly.TabIndex = 47;
            this.chkSetDataOnly.Text = "슬롯 정보가 설정된 메뉴만 보기";
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
            this.pnlSearchBtn.Controls.Add(this.btnSearch);
            this.pnlSearchBtn.Dock = System.Windows.Forms.DockStyle.Right;
            this.pnlSearchBtn.Location = new System.Drawing.Point(888, 0);
            this.pnlSearchBtn.Name = "pnlSearchBtn";
            this.pnlSearchBtn.Size = new System.Drawing.Size(120, 38);
            this.pnlSearchBtn.TabIndex = 3;
            // 
            // btnSearch
            // 
            this.btnSearch.Enabled = false;
            this.btnSearch.Location = new System.Drawing.Point(8, 8);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(104, 24);
            this.btnSearch.TabIndex = 2;
            this.btnSearch.Text = "조 회";
            this.btnSearch.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // uiPanelSlotAdInfo
            // 
            this.uiPanelSlotAdInfo.AllowResize = Janus.Windows.UI.InheritableBoolean.True;
            this.uiPanelSlotAdInfo.CaptionStyle = Janus.Windows.UI.Dock.PanelCaptionStyle.Dark;
            this.uiPanelSlotAdInfo.CaptionVisible = Janus.Windows.UI.InheritableBoolean.False;
            this.uiPanelSlotAdInfo.GroupStyle = Janus.Windows.UI.Dock.PanelGroupStyle.VerticalTiles;
            this.uiPanelSlotAdInfo.Location = new System.Drawing.Point(0, 66);
            this.uiPanelSlotAdInfo.Name = "uiPanelSlotAdInfo";
            this.uiPanelSlotAdInfo.Size = new System.Drawing.Size(1010, 426);
            this.uiPanelSlotAdInfo.TabIndex = 4;
            this.uiPanelSlotAdInfo.Text = "메뉴/채널 목록";
            // 
            // uiPanel1
            // 
            this.uiPanel1.CaptionStyle = Janus.Windows.UI.Dock.PanelCaptionStyle.Light;
            this.uiPanel1.CaptionVisible = Janus.Windows.UI.InheritableBoolean.False;
            this.uiPanel1.InnerContainer = this.uiPanel1Container;
            this.uiPanel1.Location = new System.Drawing.Point(0, 0);
            this.uiPanel1.Name = "uiPanel1";
            this.uiPanel1.Size = new System.Drawing.Size(1010, 426);
            this.uiPanel1.TabIndex = 4;
            this.uiPanel1.Text = "메뉴";
            // 
            // uiPanel1Container
            // 
            this.uiPanel1Container.Controls.Add(this.grdExCategenList);
            this.uiPanel1Container.Location = new System.Drawing.Point(1, 1);
            this.uiPanel1Container.Name = "uiPanel1Container";
            this.uiPanel1Container.Size = new System.Drawing.Size(1008, 424);
            this.uiPanel1Container.TabIndex = 0;
            // 
            // grdExCategenList
            // 
            this.grdExCategenList.AllowEdit = Janus.Windows.GridEX.InheritableBoolean.False;
            this.grdExCategenList.AlternatingColors = true;
            this.grdExCategenList.AlternatingRowFormatStyle.BackgroundImageDrawMode = Janus.Windows.GridEX.BackgroundImageDrawMode.Tile;
            this.grdExCategenList.AutomaticSort = false;
            this.grdExCategenList.BorderStyle = Janus.Windows.GridEX.BorderStyle.None;
            this.grdExCategenList.DataSource = this.dvCateGen;
            grdExCategenList_DesignTimeLayout_Reference_0.Instance = ((object)(resources.GetObject("grdExCategenList_DesignTimeLayout_Reference_0.Instance")));
            grdExCategenList_DesignTimeLayout_Reference_1.Instance = ((object)(resources.GetObject("grdExCategenList_DesignTimeLayout_Reference_1.Instance")));
            grdExCategenList_DesignTimeLayout_Reference_2.Instance = ((object)(resources.GetObject("grdExCategenList_DesignTimeLayout_Reference_2.Instance")));
            grdExCategenList_DesignTimeLayout_Reference_3.Instance = ((object)(resources.GetObject("grdExCategenList_DesignTimeLayout_Reference_3.Instance")));
            grdExCategenList_DesignTimeLayout.LayoutReferences.AddRange(new Janus.Windows.Common.Layouts.JanusLayoutReference[] {
            grdExCategenList_DesignTimeLayout_Reference_0,
            grdExCategenList_DesignTimeLayout_Reference_1,
            grdExCategenList_DesignTimeLayout_Reference_2,
            grdExCategenList_DesignTimeLayout_Reference_3});
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
            this.grdExCategenList.Size = new System.Drawing.Size(1008, 424);
            this.grdExCategenList.TabIndex = 3;
            this.grdExCategenList.TabKeyBehavior = Janus.Windows.GridEX.TabKeyBehavior.ControlNavigation;
            this.grdExCategenList.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            this.grdExCategenList.SelectionChanged += new System.EventHandler(this.OnGrdRowChangedMenu);
            this.grdExCategenList.Click += new System.EventHandler(this.OnGrdRowChangedMenu);
            // 
            // dvCateGen
            // 
            this.dvCateGen.Table = this.slotAdInfoDs.Categens;
            // 
            // slotAdInfoDs
            // 
            this.slotAdInfoDs.DataSetName = "SlotAdInfoDs";
            this.slotAdInfoDs.Locale = new System.Globalization.CultureInfo("en-US");
            this.slotAdInfoDs.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // uiPanelDetail
            // 
            this.uiPanelDetail.AllowResize = Janus.Windows.UI.InheritableBoolean.False;
            this.uiPanelDetail.CaptionStyle = Janus.Windows.UI.Dock.PanelCaptionStyle.Dark;
            this.uiPanelDetail.InnerContainer = this.uiPanelDetailContainer;
            this.uiPanelDetail.Location = new System.Drawing.Point(0, 496);
            this.uiPanelDetail.Name = "uiPanelDetail";
            this.uiPanelDetail.Size = new System.Drawing.Size(1010, 181);
            this.uiPanelDetail.TabIndex = 4;
            this.uiPanelDetail.Text = "상세정보";
            // 
            // uiPanelDetailContainer
            // 
            this.uiPanelDetailContainer.Controls.Add(this.pnlDetail);
            this.uiPanelDetailContainer.Location = new System.Drawing.Point(1, 23);
            this.uiPanelDetailContainer.Name = "uiPanelDetailContainer";
            this.uiPanelDetailContainer.Size = new System.Drawing.Size(1008, 157);
            this.uiPanelDetailContainer.TabIndex = 0;
            // 
            // pnlDetail
            // 
            this.pnlDetail.BackColor = System.Drawing.SystemColors.Window;
            this.pnlDetail.Controls.Add(this.chkPromotionYn);
            this.pnlDetail.Controls.Add(this.label6);
            this.pnlDetail.Controls.Add(this.chkUseYn);
            this.pnlDetail.Controls.Add(this.btnDelete);
            this.pnlDetail.Controls.Add(this.label1);
            this.pnlDetail.Controls.Add(this.btnSave);
            this.pnlDetail.Controls.Add(this.uiGroupBox2);
            this.pnlDetail.Controls.Add(this.btnCancel);
            this.pnlDetail.Controls.Add(this.uiGroupBox1);
            this.pnlDetail.Controls.Add(this.btnUpdate);
            this.pnlDetail.Controls.Add(this.ebMenuName);
            this.pnlDetail.Controls.Add(this.lbMenuName);
            this.pnlDetail.Controls.Add(this.ebCategoryName);
            this.pnlDetail.Controls.Add(this.lbCategoryName);
            this.pnlDetail.Controls.Add(this.label58);
            this.pnlDetail.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlDetail.Location = new System.Drawing.Point(0, 0);
            this.pnlDetail.Name = "pnlDetail";
            this.pnlDetail.Size = new System.Drawing.Size(1008, 157);
            this.pnlDetail.TabIndex = 6;
            // 
            // chkUseYn
            // 
            this.chkUseYn.Location = new System.Drawing.Point(809, 95);
            this.chkUseYn.Name = "chkUseYn";
            this.chkUseYn.Size = new System.Drawing.Size(14, 18);
            this.chkUseYn.TabIndex = 269;
            this.chkUseYn.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            // 
            // btnDelete
            // 
            this.btnDelete.Enabled = false;
            this.btnDelete.Location = new System.Drawing.Point(888, 16);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(104, 24);
            this.btnDelete.TabIndex = 268;
            this.btnDelete.Text = "삭 제";
            this.btnDelete.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(751, 93);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(72, 21);
            this.label1.TabIndex = 267;
            this.label1.Text = "사용여부";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnSave
            // 
            this.btnSave.Enabled = false;
            this.btnSave.Location = new System.Drawing.Point(665, 15);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(104, 24);
            this.btnSave.TabIndex = 260;
            this.btnSave.Text = "저 장";
            this.btnSave.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // uiGroupBox2
            // 
            this.uiGroupBox2.Controls.Add(this.udMaxTimePay);
            this.uiGroupBox2.Controls.Add(this.udMaxCountPay);
            this.uiGroupBox2.Controls.Add(this.label4);
            this.uiGroupBox2.Controls.Add(this.label5);
            this.uiGroupBox2.Font = new System.Drawing.Font("맑은 고딕", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.uiGroupBox2.Location = new System.Drawing.Point(385, 72);
            this.uiGroupBox2.Name = "uiGroupBox2";
            this.uiGroupBox2.Size = new System.Drawing.Size(360, 60);
            this.uiGroupBox2.TabIndex = 259;
            this.uiGroupBox2.Text = "유료채널";
            this.uiGroupBox2.VisualStyle = Janus.Windows.UI.Dock.PanelVisualStyle.Office2003;
            // 
            // udMaxTimePay
            // 
            this.udMaxTimePay.Enabled = false;
            this.udMaxTimePay.Location = new System.Drawing.Point(294, 23);
            this.udMaxTimePay.MaxLength = 3;
            this.udMaxTimePay.Name = "udMaxTimePay";
            this.udMaxTimePay.Size = new System.Drawing.Size(40, 22);
            this.udMaxTimePay.TabIndex = 258;
            this.udMaxTimePay.TextAlignment = Janus.Windows.GridEX.TextAlignment.Far;
            this.udMaxTimePay.Value = 30;
            this.udMaxTimePay.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            // 
            // udMaxCountPay
            // 
            this.udMaxCountPay.Enabled = false;
            this.udMaxCountPay.Location = new System.Drawing.Point(132, 23);
            this.udMaxCountPay.Maximum = 4;
            this.udMaxCountPay.MaxLength = 3;
            this.udMaxCountPay.Name = "udMaxCountPay";
            this.udMaxCountPay.Size = new System.Drawing.Size(40, 22);
            this.udMaxCountPay.TabIndex = 257;
            this.udMaxCountPay.TextAlignment = Janus.Windows.GridEX.TextAlignment.Far;
            this.udMaxCountPay.Value = 2;
            this.udMaxCountPay.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(194, 26);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(96, 16);
            this.label4.TabIndex = 256;
            this.label4.Text = "최대광고타임(초)";
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(29, 26);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(106, 16);
            this.label5.TabIndex = 254;
            this.label5.Text = "최대광고갯수";
            // 
            // btnCancel
            // 
            this.btnCancel.Enabled = false;
            this.btnCancel.Location = new System.Drawing.Point(778, 15);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(104, 24);
            this.btnCancel.TabIndex = 261;
            this.btnCancel.Text = "취 소";
            this.btnCancel.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // uiGroupBox1
            // 
            this.uiGroupBox1.Controls.Add(this.udMaxTime);
            this.uiGroupBox1.Controls.Add(this.udMaxCount);
            this.uiGroupBox1.Controls.Add(this.label3);
            this.uiGroupBox1.Controls.Add(this.label2);
            this.uiGroupBox1.Font = new System.Drawing.Font("맑은 고딕", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.uiGroupBox1.Location = new System.Drawing.Point(15, 72);
            this.uiGroupBox1.Name = "uiGroupBox1";
            this.uiGroupBox1.Size = new System.Drawing.Size(360, 60);
            this.uiGroupBox1.TabIndex = 255;
            this.uiGroupBox1.Text = "무료채널";
            this.uiGroupBox1.VisualStyle = Janus.Windows.UI.Dock.PanelVisualStyle.Office2003;
            // 
            // udMaxTime
            // 
            this.udMaxTime.Enabled = false;
            this.udMaxTime.Location = new System.Drawing.Point(297, 23);
            this.udMaxTime.MaxLength = 3;
            this.udMaxTime.Name = "udMaxTime";
            this.udMaxTime.Size = new System.Drawing.Size(40, 22);
            this.udMaxTime.TabIndex = 258;
            this.udMaxTime.TextAlignment = Janus.Windows.GridEX.TextAlignment.Far;
            this.udMaxTime.Value = 60;
            this.udMaxTime.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            // 
            // udMaxCount
            // 
            this.udMaxCount.Enabled = false;
            this.udMaxCount.Location = new System.Drawing.Point(123, 23);
            this.udMaxCount.Maximum = 4;
            this.udMaxCount.MaxLength = 3;
            this.udMaxCount.Name = "udMaxCount";
            this.udMaxCount.Size = new System.Drawing.Size(40, 22);
            this.udMaxCount.TabIndex = 257;
            this.udMaxCount.TextAlignment = Janus.Windows.GridEX.TextAlignment.Far;
            this.udMaxCount.Value = 3;
            this.udMaxCount.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(197, 26);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(96, 16);
            this.label3.TabIndex = 256;
            this.label3.Text = "최대광고타임(초)";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(22, 26);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(101, 16);
            this.label2.TabIndex = 254;
            this.label2.Text = "최대광고갯수";
            // 
            // btnUpdate
            // 
            this.btnUpdate.Enabled = false;
            this.btnUpdate.Location = new System.Drawing.Point(553, 15);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(104, 24);
            this.btnUpdate.TabIndex = 259;
            this.btnUpdate.Text = "수 정";
            this.btnUpdate.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
            // 
            // ebMenuName
            // 
            this.ebMenuName.Location = new System.Drawing.Point(325, 17);
            this.ebMenuName.MaxLength = 40;
            this.ebMenuName.Name = "ebMenuName";
            this.ebMenuName.ReadOnly = true;
            this.ebMenuName.Size = new System.Drawing.Size(214, 21);
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
            this.ebCategoryName.Location = new System.Drawing.Point(78, 17);
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
            // label58
            // 
            this.label58.BackColor = System.Drawing.Color.Gray;
            this.label58.Location = new System.Drawing.Point(8, 50);
            this.label58.Name = "label58";
            this.label58.Size = new System.Drawing.Size(980, 1);
            this.label58.TabIndex = 246;
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
            // label6
            // 
            this.label6.Location = new System.Drawing.Point(751, 72);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(112, 21);
            this.label6.TabIndex = 270;
            this.label6.Text = "프로모션 송출 여부";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label6.Visible = false;
            // 
            // chkPromotionYn
            // 
            this.chkPromotionYn.Location = new System.Drawing.Point(858, 74);
            this.chkPromotionYn.Name = "chkPromotionYn";
            this.chkPromotionYn.Size = new System.Drawing.Size(14, 18);
            this.chkPromotionYn.TabIndex = 271;
            this.chkPromotionYn.Visible = false;
            this.chkPromotionYn.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            // 
            // SlotAdInfoControl
            // 
            this.Controls.Add(this.uiPanelUsers);
            this.Font = global::AdManagerClient.Properties.Settings.Default.Font1;
            this.Name = "SlotAdInfoControl";
            this.Size = new System.Drawing.Size(1010, 677);
            this.Load += new System.EventHandler(this.SlotAdInfoControl_Load);
            ((System.ComponentModel.ISupportInitialize)(this.uiPM)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelUsers)).EndInit();
            this.uiPanelUsers.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelSearch)).EndInit();
            this.uiPanelSearch.ResumeLayout(false);
            this.uiPanelSearchContainer.ResumeLayout(false);
            this.pnlSearch.ResumeLayout(false);
            this.pnlSearchBtn.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelSlotAdInfo)).EndInit();
            this.uiPanelSlotAdInfo.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.uiPanel1)).EndInit();
            this.uiPanel1.ResumeLayout(false);
            this.uiPanel1Container.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdExCategenList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dvCateGen)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.slotAdInfoDs)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelDetail)).EndInit();
            this.uiPanelDetail.ResumeLayout(false);
            this.uiPanelDetailContainer.ResumeLayout(false);
            this.pnlDetail.ResumeLayout(false);
            this.pnlDetail.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uiGroupBox2)).EndInit();
            this.uiGroupBox2.ResumeLayout(false);
            this.uiGroupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uiGroupBox1)).EndInit();
            this.uiGroupBox1.ResumeLayout(false);
            this.uiGroupBox1.PerformLayout();
            this.ResumeLayout(false);

        }
        #endregion

        #region 컨트롤 로드
        private void SlotAdInfoControl_Load(object sender, System.EventArgs e)
        {
            // 데이터관리용 객체생성
            dtMenu= ((DataView)grdExCategenList.DataSource).Table;
            cmMenu= (CurrencyManager) this.BindingContext[grdExCategenList.DataSource];
            lbMsg.Text = "";
            chkUseYn.CheckState = CheckState.Unchecked;
            chkPromotionYn.CheckState = CheckState.Unchecked;

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

            if (canRead) GetDefaultSlotAdInfo();

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
                Utility.SetDataTable(slotAdInfoDs.Medias, mediacodeModel.MediaCodeDataSet);				
            }

            // 검색조건의 콤보
            this.cbSearchMedia.Items.Clear();
			
            // 콤보박스에 셋트할 코드목록을 담을 Item배열을 선언
            Janus.Windows.EditControls.UIComboBoxItem[] comboItems = new Janus.Windows.EditControls.UIComboBoxItem[mediacodeModel.ResultCnt + 1];

            comboItems[0] = new Janus.Windows.EditControls.UIComboBoxItem("매체선택","00");
			
            for(int i=0;i<mediacodeModel.ResultCnt;i++)
            {
                DataRow row = slotAdInfoDs.Medias.Rows[i];

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
				for(int i=0;i < slotAdInfoDs.Medias.Rows.Count;i++)
				{
					DataRow row = slotAdInfoDs.Medias.Rows[i];					
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
        /// 광고 슬롯 정보 기본값 조회
        /// </summary>
        private void GetDefaultSlotAdInfo()
        {
            IsSearching = true;

            StatusMessage("광고 슬롯 정보 기본값을 조회합니다.");

            if (cbSearchMedia.SelectedItem.Value.Equals("00"))
            {
                MessageBox.Show("매체를 선택하여 주시기 바랍니다.", "광고 슬롯 정보 조회", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            try
            {
                ProgressStart();

                slotAdInfoModel.Init();

                // 데이터모델에 전송할 내용을 셋트한다.				
                slotAdInfoModel.SearchMediaCode = cbSearchMedia.SelectedValue.ToString();

                // 광고 슬롯 정보 기본값 조회 서비스를 호출한다.
                new SlotAdInfoManager(systemModel, commonModel).GetDefaultSlotAdInfo(slotAdInfoModel);

                if (slotAdInfoModel.ResultCD.Equals("0000"))
                {

                    defaultMaxCount = slotAdInfoModel.MaxCount;
                    defaultMaxTime = slotAdInfoModel.MaxTime;
                    defaultMaxCountPay = slotAdInfoModel.MaxCountPay;
                    defaultMaxTimePay = slotAdInfoModel.MaxTimePay;
                    defaultUseYn = slotAdInfoModel.UseYn;
                    defaultPromotionYn = slotAdInfoModel.PromotionYn;

                    lbMsg.Text = "기본값:무료채널-광고"+defaultMaxCount+"개,시간"+defaultMaxTime+"초/유료채널:광고"+defaultMaxCountPay+"개,시간"+defaultMaxTimePay+"초";

                }
            }
            catch (FrameException fe)
            {
                FrameSystem.showMsgForm("광고 슬롯 정보 기본값 조회", new string[] { fe.ErrCode, fe.ResultMsg });
            }
            catch (Exception ex)
            {
                FrameSystem.showMsgForm("광고 슬롯 정보 기본값 조회", new string[] { "", ex.Message });
            }
            finally
            {
                IsSearching = false; // 조회중 Flag 리셋
                ProgressStop();
            }

        }

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
			
                slotAdInfoModel.Init();
				
				// 데이터 클리어
				slotAdInfoDs.Categens.Clear();
				//slotAdInfoDs.SlotAdInfo.Clear();  
				ResetDetail();

                // 데이터모델에 전송할 내용을 셋트한다.				
                slotAdInfoModel.SearchMediaCode = cbSearchMedia.SelectedValue.ToString();


                //세팅된 메뉴만 보기 체크판단
                slotAdInfoModel.IsSetDataOnly = chkSetDataOnly.Checked;
                
                // 슬롯 세팅 현황 목록 조회 서비스를 호출한다.
                new SlotAdInfoManager(systemModel,commonModel).GetMenuList(slotAdInfoModel);

                if (slotAdInfoModel.ResultCD.Equals("0000"))
                {
                    Utility.SetDataTable(slotAdInfoDs.Categens, slotAdInfoModel.SlotAdInfoDataSet);
                    StatusMessage(slotAdInfoModel.ResultCnt + "건의 메뉴 정보가 조회되었습니다.");
//                    if (slotAdInfoModel.ResultCnt > 0) cmMenu.Position = 0;

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
                        + "[광고슬롯] "
                        + dtMenu.Rows[curRow]["CategoryName"].ToString().Trim() + "||"
                        + dtMenu.Rows[curRow]["MenuName"].ToString().Trim();

                    keyCategoryCode = dtMenu.Rows[curRow]["CategoryCode"].ToString();
                    keyMenuCode = dtMenu.Rows[curRow]["MenuCode"].ToString();

                    ebCategoryName.Text = dtMenu.Rows[curRow]["CategoryName"].ToString();
                    ebMenuName.Text = dtMenu.Rows[curRow]["MenuName"].ToString();

                    //상세 정보 세팅
                    string sMaxCount = dtMenu.Rows[curRow]["MaxCount"].ToString();
                    if (sMaxCount.Equals("0"))
                        udMaxCount.Value = 0;
                    else
                        udMaxCount.Value = Convert.ToInt32(sMaxCount);

                    string sMaxTime = dtMenu.Rows[curRow]["MaxTime"].ToString();
                    if (sMaxTime.Equals("0"))
                        udMaxTime.Value = 0;
                    else
                        udMaxTime.Value = Convert.ToInt32(sMaxTime);

                    string sMaxCountPay = dtMenu.Rows[curRow]["MaxCountPay"].ToString();
                    if (sMaxCountPay.Equals("0"))
                        udMaxCountPay.Value = 0;
                    else
                        udMaxCountPay.Value = Convert.ToInt32(sMaxCountPay);

                    string sMaxTimePay = dtMenu.Rows[curRow]["MaxTimePay"].ToString();
                    if (sMaxTimePay.Equals("0"))
                        udMaxTimePay.Value = 0;
                    else
                        udMaxTimePay.Value = Convert.ToInt32(sMaxTimePay);

                    string useYn = dtMenu.Rows[curRow]["UseYn"].ToString();
                    if (useYn.Equals("Y"))
                    {
                        chkUseYn.CheckState = CheckState.Checked;
                    }
                    else
                    {
                        chkUseYn.CheckState = CheckState.Unchecked;
                    }

                    string promotionYn = dtMenu.Rows[curRow]["PromotionYn"].ToString();
                    if (promotionYn.Equals("Y"))
                    {
                        chkPromotionYn.CheckState = CheckState.Checked;
                    }
                    else
                    {
                        chkPromotionYn.CheckState = CheckState.Unchecked;
                    }
                    
                    if ((udMaxCount.Value + udMaxTime.Value + udMaxCountPay.Value + udMaxTimePay.Value) == 0)
                        IsInsert = true;
                    else
                        IsInsert = false;

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

                udMaxCount.Value = 0;
                udMaxTime.Value = 0;
                udMaxCountPay.Value = 0;
                udMaxTimePay.Value = 0;
                chkUseYn.CheckState = CheckState.Unchecked;

                udMaxCount.Enabled = false;
                udMaxTime.Enabled = false;
                udMaxCountPay.Enabled = false;
                udMaxTimePay.Enabled = false;
                chkUseYn.Enabled = false;
                chkPromotionYn.Enabled = false;

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


            udMaxCount.Enabled = false;
            udMaxTime.Enabled = false;
            udMaxCountPay.Enabled = false;
            udMaxTimePay.Enabled = false;
            chkUseYn.Enabled = false;
            chkPromotionYn.Enabled = false;
        }

        public void ReloadList()
        {
            SetDetailTextMenu();
        }

        /// <summary>
        /// 광고 슬롯 정보 저장
        /// </summary>
        private void SaveFlexSlotInfo()
        {
            StatusMessage("광고 슬롯 정보를 저장합니다.");

            const int MIN_COUNT = 0;
            const int MAX_COUNT = 4;
            const int MIN_TIME = 10;
            const int MAX_TIME = 100;

            if (udMaxCount.Value < 0 || udMaxCount.Value > 4 )
            {
                MessageBox.Show("무료 광고의 최대 광고 갯수는 " + MIN_COUNT + " ~ " + MAX_COUNT + "개 이내로 입력해주시기 바랍니다.", "광고 슬롯 정보 저장", MessageBoxButtons.OK, MessageBoxIcon.Information);
                udMaxCount.Focus();
                return;
            }
            if (udMaxTime.Value < 10 || udMaxTime.Value > 100)
            {
                MessageBox.Show("무료 광고의 최대 광고 시간은 " + MIN_TIME + " ~ " + MAX_TIME + "초 이내로 입력해주시기 바랍니다.", "광고 슬롯 정보 저장", MessageBoxButtons.OK, MessageBoxIcon.Information);
                udMaxTime.Focus();
                return;
            }
            if (udMaxCountPay.Value < 0 || udMaxCountPay.Value > 4)
            {
                MessageBox.Show("유료 광고의 최대 광고 갯수는 " + MIN_COUNT + " ~ " + MAX_COUNT + "개 이내로 입력해주시기 바랍니다..", "광고 슬롯 정보 저장", MessageBoxButtons.OK, MessageBoxIcon.Information);
                udMaxCountPay.Focus();
                return;
            }
            if (udMaxTimePay.Value < 10 || udMaxTimePay.Value > 100)
            {
                MessageBox.Show("유료 광고의 최대 광고 시간은 " + MIN_TIME + " ~ " + MAX_TIME + "초이내로 입력해주시기 바랍니다.", "광고 슬롯 정보 저장", MessageBoxButtons.OK, MessageBoxIcon.Information);
                udMaxTimePay.Focus();
                return;
            }

            try
            {
                
                slotAdInfoModel.Init();
                slotAdInfoModel.CategoryCode = keyCategoryCode;
                slotAdInfoModel.MenuCode = keyMenuCode;
                slotAdInfoModel.MaxCount = udMaxCount.Value;
                slotAdInfoModel.MaxTime = udMaxTime.Value;
                slotAdInfoModel.MaxCountPay = udMaxCountPay.Value;
                slotAdInfoModel.MaxTimePay = udMaxTimePay.Value;

                if (chkUseYn.CheckState == CheckState.Checked)
                {
                    slotAdInfoModel.UseYn = "Y";
                }
                else 
                {
                    slotAdInfoModel.UseYn = "N";
                }

                if (chkPromotionYn.CheckState == CheckState.Checked)
                {
                    slotAdInfoModel.PromotionYn = "Y";
                }
                else
                {
                    slotAdInfoModel.PromotionYn = "N";
                }
                
                //상세정보 저장 서비스 호출
                if (IsInsert)
                {
                    new SlotAdInfoManager(systemModel, commonModel).InsertSlotAdInfo(slotAdInfoModel);
                }
                else
                {
                    new SlotAdInfoManager(systemModel, commonModel).UpdateSlotAdInfo(slotAdInfoModel);
                }

                if (slotAdInfoModel.ResultCD.Equals("0000"))
                {

                    StatusMessage("광고 슬롯 정보가 저장되었습니다.");
                    SearchMenu();
                    InitButton();
                    ScrollToCurrent();
                    OnGrdRowChangedMenu(null, null);
                }
            }
            catch (FrameException fe)
            {
                FrameSystem.showMsgForm("광고 슬롯 정보 저장 오류", new string[] { fe.ErrCode, fe.ResultMsg });
            }
            catch (Exception ex)
            {
                FrameSystem.showMsgForm("광고 슬롯 정보 저장 오류", new string[] { "", ex.Message });
            }
        }

        /// <summary>
        /// 광고 슬롯 정보 삭제
        /// </summary>
        private void DeleteFlexSlotInfo()
        {
            StatusMessage("광고 슬롯 정보를 삭제합니다.");

            try
            {

                slotAdInfoModel.Init();
                slotAdInfoModel.CategoryCode = keyCategoryCode;
                slotAdInfoModel.MenuCode = keyMenuCode;

                //상세정보 삭제 서비스 호출
                new SlotAdInfoManager(systemModel, commonModel).DeleteSlotAdInfo(slotAdInfoModel);
                
                if (slotAdInfoModel.ResultCD.Equals("0000"))
                {

                    StatusMessage("광고 슬롯 정보가 삭제되었습니다.");
                    SearchMenu();
                    InitButton();
                    ScrollToCurrent();
                    grdExCategenList.Focus();
                    OnGrdRowChangedMenu(null, null);
                }

            }
            catch (FrameException fe)
            {
                FrameSystem.showMsgForm("광고 슬롯 정보 저장 오류", new string[] { fe.ErrCode, fe.ResultMsg });
            }
            catch (Exception ex)
            {
                FrameSystem.showMsgForm("광고 슬롯 정보 저장 오류", new string[] { "", ex.Message });
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

            udMaxCount.Enabled = true;
            udMaxTime.Enabled = true;
            udMaxCountPay.Enabled = true;
            udMaxTimePay.Enabled = true;
            chkUseYn.Enabled = true;
            chkPromotionYn.Enabled = true;

            btnUpdate.Enabled = false;
            btnDelete.Enabled = false;
            btnSave.Enabled = true;
            btnCancel.Enabled = true;


            //값이 이미 설정되어 있는지 여부
            if (IsInsert)
            {
                //값이 처음 설정되는 경우 기본값을 설정해준다.
                udMaxCount.Value = defaultMaxCount;
                udMaxTime.Value = defaultMaxTime;
                udMaxCountPay.Value = defaultMaxCountPay;
                udMaxTimePay.Value = defaultMaxTimePay;

                if (defaultUseYn.Equals("Y"))
                {
                    chkUseYn.CheckState = CheckState.Checked;
                }
                else
                {
                    chkUseYn.CheckState = CheckState.Unchecked;
                }

                if (defaultPromotionYn.Equals("Y"))
                {
                    chkPromotionYn.CheckState = CheckState.Checked;
                }
                else
                {
                    chkPromotionYn.CheckState = CheckState.Unchecked;
                }
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
                MessageBox.Show("삭제할 광고 슬롯 정보가 없습니다.", "광고 슬롯 정보 삭제",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            DialogResult result = MessageBox.Show("해당 메뉴의 광고 슬롯 정보를 삭제 하시겠습니까?", "광고 슬롯 정보 삭제",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question,
                MessageBoxDefaultButton.Button2);

            if (result == DialogResult.No) return;

            
            DeleteFlexSlotInfo();
        }
       #endregion
                
    }
}