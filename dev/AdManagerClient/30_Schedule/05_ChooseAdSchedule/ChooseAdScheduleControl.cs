// ===============================================================================
// ChooseAdScheduleControl for Charites Project
//
// ChooseAdScheduleControl.cs
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
using AdManagerClient.Common.Args;
namespace AdManagerClient
{
    /// <summary>
    /// 채널/메뉴별 편성현황 컨트롤
    /// </summary>
    public class ChooseAdScheduleControl : System.Windows.Forms.UserControl, IUserControl
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
        ChooseAdScheduleModel chooseAdScheduleModel = new ChooseAdScheduleModel();	// 채널/메뉴 편성현황 모델
		SchChoiceAdModel schChoiceAdModel			= new SchChoiceAdModel();		// 지정광고편성모델
		SchPublishModel schPublishModel				= new SchPublishModel();		// 광고승인모델
		
		// 화면처리용 변수
        CurrencyManager cmMenu      = null;					// 데이터 그리드의 변경에 따른 데이터셋 관리를 위하여			
        CurrencyManager cmChannel   = null;		
		CurrencyManager cmSchedule  = null;					
		DataTable       dtMenu      = null;
        DataTable       dtChannel   = null;
		DataTable       dtSchedule  = null;

		// 사용권한
        bool IsSearching = false; // 조회중 상세화면이 업데이트 되는 것을 방지 하기위함 2011.11.29 JH.Park
		bool canCreate			  = false;
		bool canRead			  = false;
		bool canUpdate			  = false;
		bool canDelete            = false;

		// Key
		bool IsNotLoading		       = true;					// 상세조회중이 아님
		public string keyMediaCode     = "";
		public string keyCategoryCode  = "";
		public string keyGenreCode     = "";
        public string keyGenreParCode = "";
		public string keyChannelNo     = "";
		public string keySeriesNo		= "";
		public string keyItemNo        = "";
		public string keyScheduleOrder = "";

		private	TYPE_Schedule	keyScheduleType;

		string keyItemName     = "";
		string keyLastOrder    = "";
		
		// 순위변경구분
		const int ORDER_FIRST = 1;
		const int ORDER_LAST  = 2;
		const int ORDER_UP    = 3;
		const int ORDER_DOWN  = 4;

		// 편성배포 승인상태 처리용
		private string keyAckNoCm    = "";
		private string keyAckStateCm = "";
		private string keyAckNoOap    = "";
        private Janus.Windows.EditControls.UICheckBox chkAdState_20;
        private Janus.Windows.EditControls.UICheckBox chkAdState_30;
        private Janus.Windows.EditControls.UICheckBox chkAdState_40;
        private Janus.Windows.EditControls.UIComboBox cbSearchMedia;
        private Janus.Windows.GridEX.GridEX gridEX1;
		private string keyAckStateOap = "";

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
        private System.Data.DataView dvChooseAdSchedule;
        private System.Data.DataView dvCateGen;
		private AdManagerClient.ChooseAdScheduleDs chooseAdScheduleDs;
        private Janus.Windows.UI.Dock.UIPanelGroup uiPanelChooseAdSchedule;
        private Janus.Windows.UI.Dock.UIPanel uiPanel1;
        private Janus.Windows.UI.Dock.UIPanelInnerContainer uiPanel1Container;
        private Janus.Windows.UI.Dock.UIPanel uiPanel2;
        private Janus.Windows.UI.Dock.UIPanelInnerContainer uiPanel2Container;
        private Janus.Windows.GridEX.GridEX grdExCategenList;
        private Janus.Windows.GridEX.GridEX grdExChooseAdScheduleList;
        private System.Windows.Forms.Button btnGenreName;
        private System.Windows.Forms.Panel pnlUserDetail;
        private System.Data.DataView dvChannelSet;
        private Janus.Windows.GridEX.GridEX grdExScheduleList;
		private Janus.Windows.UI.Dock.UIPanel uiPanelSearch;
		private Janus.Windows.UI.Dock.UIPanelInnerContainer uiPanelSearchContainer;
		private Janus.Windows.UI.Dock.UIPanel uiPanelList;
		private Janus.Windows.UI.Dock.UIPanelInnerContainer uiPanelListContainer;
		private System.Windows.Forms.Panel pnlDetail;
		private Janus.Windows.EditControls.UIButton btnDelete;
		private Janus.Windows.UI.Dock.UIPanel uiPanelDetail;
		private Janus.Windows.UI.Dock.UIPanelInnerContainer uiPanelDetailContainer;
		private Janus.Windows.EditControls.UIGroupBox gbScheduling;
		private Janus.Windows.EditControls.UIButton btnOrderUp;
		private Janus.Windows.EditControls.UIButton btnOrderDown;
		private Janus.Windows.EditControls.UIButton btnOrderFirst;
		private Janus.Windows.EditControls.UIButton btnOrderLast;
        private System.Windows.Forms.Label lbMsg;
        private System.Windows.Forms.Label lbAdState;
        private System.Windows.Forms.Label label1;
        private Janus.Windows.EditControls.UIGroupBox uiGroupBox1;
        private Janus.Windows.EditControls.UIGroupBox uiGroupBox2;
        private Janus.Windows.EditControls.UIButton btnAdd_12;
        private Janus.Windows.EditControls.UIButton btnAdd_11;
        private Janus.Windows.EditControls.UIButton btnAdd_20;
        private Janus.Windows.EditControls.UIButton btnAdd_16;
        private Janus.Windows.EditControls.UIButton btnAdd_10;
		private System.Data.DataView dvSeries;
        private Janus.Windows.EditControls.UIButton btnAdd_19;		
        private System.ComponentModel.IContainer components;

        public ChooseAdScheduleControl()
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ChooseAdScheduleControl));
            Janus.Windows.GridEX.GridEXLayout grdExCategenList_Layout_0 = new Janus.Windows.GridEX.GridEXLayout();
            Janus.Windows.GridEX.GridEXLayout gridEX1_DesignTimeLayout = new Janus.Windows.GridEX.GridEXLayout();
            Janus.Windows.Common.Layouts.JanusLayoutReference gridEX1_DesignTimeLayout_Reference_0 = new Janus.Windows.Common.Layouts.JanusLayoutReference("GridEXLayoutData.RootTable.FormatConditions.Condition0.FormatStyle.BackgroundImag" +
        "e");
            Janus.Windows.Common.Layouts.JanusLayoutReference gridEX1_DesignTimeLayout_Reference_1 = new Janus.Windows.Common.Layouts.JanusLayoutReference("GridEXLayoutData.RootTable.FormatConditions.Condition1.FormatStyle.BackgroundImag" +
        "e");
            Janus.Windows.Common.Layouts.JanusLayoutReference gridEX1_DesignTimeLayout_Reference_2 = new Janus.Windows.Common.Layouts.JanusLayoutReference("GridEXLayoutData.RootTable.FormatConditions.Condition2.FormatStyle.BackgroundImag" +
        "e");
            Janus.Windows.GridEX.GridEXLayout grdExChooseAdScheduleList_DesignTimeLayout = new Janus.Windows.GridEX.GridEXLayout();
            Janus.Windows.Common.Layouts.JanusLayoutReference grdExChooseAdScheduleList_DesignTimeLayout_Reference_0 = new Janus.Windows.Common.Layouts.JanusLayoutReference("GridEXLayoutData.RootTable.FormatConditions.Condition0.FormatStyle.BackgroundImag" +
        "e");
            Janus.Windows.Common.Layouts.JanusLayoutReference grdExChooseAdScheduleList_DesignTimeLayout_Reference_1 = new Janus.Windows.Common.Layouts.JanusLayoutReference("GridEXLayoutData.RootTable.FormatConditions.Condition1.FormatStyle.BackgroundImag" +
        "e");
            Janus.Windows.Common.Layouts.JanusLayoutReference grdExChooseAdScheduleList_DesignTimeLayout_Reference_2 = new Janus.Windows.Common.Layouts.JanusLayoutReference("GridEXLayoutData.RootTable.FormatConditions.Condition2.FormatStyle.BackgroundImag" +
        "e");
            Janus.Windows.GridEX.GridEXLayout grdExScheduleList_DesignTimeLayout = new Janus.Windows.GridEX.GridEXLayout();
            this.uiPM = new Janus.Windows.UI.Dock.UIPanelManager(this.components);
            this.uiPanelUsers = new Janus.Windows.UI.Dock.UIPanelGroup();
            this.uiPanelSearch = new Janus.Windows.UI.Dock.UIPanel();
            this.uiPanelSearchContainer = new Janus.Windows.UI.Dock.UIPanelInnerContainer();
            this.pnlSearch = new System.Windows.Forms.Panel();
            this.cbSearchMedia = new Janus.Windows.EditControls.UIComboBox();
            this.chkAdState_40 = new Janus.Windows.EditControls.UICheckBox();
            this.chkAdState_30 = new Janus.Windows.EditControls.UICheckBox();
            this.chkAdState_20 = new Janus.Windows.EditControls.UICheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.lbAdState = new System.Windows.Forms.Label();
            this.pnlSearchBtn = new System.Windows.Forms.Panel();
            this.btnSearch = new Janus.Windows.EditControls.UIButton();
            this.lbMsg = new System.Windows.Forms.Label();
            this.uiPanelChooseAdSchedule = new Janus.Windows.UI.Dock.UIPanelGroup();
            this.uiPanel1 = new Janus.Windows.UI.Dock.UIPanel();
            this.uiPanel1Container = new Janus.Windows.UI.Dock.UIPanelInnerContainer();
            this.grdExCategenList = new Janus.Windows.GridEX.GridEX();
            this.dvCateGen = new System.Data.DataView();
            this.chooseAdScheduleDs = new AdManagerClient.ChooseAdScheduleDs();
            this.uiPanel2 = new Janus.Windows.UI.Dock.UIPanel();
            this.uiPanel2Container = new Janus.Windows.UI.Dock.UIPanelInnerContainer();
            this.gridEX1 = new Janus.Windows.GridEX.GridEX();
            this.grdExChooseAdScheduleList = new Janus.Windows.GridEX.GridEX();
            this.dvChannelSet = new System.Data.DataView();
            this.uiPanelList = new Janus.Windows.UI.Dock.UIPanel();
            this.uiPanelListContainer = new Janus.Windows.UI.Dock.UIPanelInnerContainer();
            this.pnlUserDetail = new System.Windows.Forms.Panel();
            this.grdExScheduleList = new Janus.Windows.GridEX.GridEX();
            this.dvChooseAdSchedule = new System.Data.DataView();
            this.btnGenreName = new System.Windows.Forms.Button();
            this.uiPanelDetail = new Janus.Windows.UI.Dock.UIPanel();
            this.uiPanelDetailContainer = new Janus.Windows.UI.Dock.UIPanelInnerContainer();
            this.pnlDetail = new System.Windows.Forms.Panel();
            this.uiGroupBox2 = new Janus.Windows.EditControls.UIGroupBox();
            this.btnAdd_12 = new Janus.Windows.EditControls.UIButton();
            this.btnAdd_11 = new Janus.Windows.EditControls.UIButton();
            this.btnAdd_20 = new Janus.Windows.EditControls.UIButton();
            this.uiGroupBox1 = new Janus.Windows.EditControls.UIGroupBox();
            this.btnAdd_19 = new Janus.Windows.EditControls.UIButton();
            this.btnAdd_16 = new Janus.Windows.EditControls.UIButton();
            this.btnAdd_10 = new Janus.Windows.EditControls.UIButton();
            this.gbScheduling = new Janus.Windows.EditControls.UIGroupBox();
            this.btnOrderUp = new Janus.Windows.EditControls.UIButton();
            this.btnOrderDown = new Janus.Windows.EditControls.UIButton();
            this.btnOrderFirst = new Janus.Windows.EditControls.UIButton();
            this.btnOrderLast = new Janus.Windows.EditControls.UIButton();
            this.btnDelete = new Janus.Windows.EditControls.UIButton();
            this.uiButton1 = new Janus.Windows.EditControls.UIButton();
            this.uiButton2 = new Janus.Windows.EditControls.UIButton();
            this.dvSeries = new System.Data.DataView();
            ((System.ComponentModel.ISupportInitialize)(this.uiPM)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelUsers)).BeginInit();
            this.uiPanelUsers.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelSearch)).BeginInit();
            this.uiPanelSearch.SuspendLayout();
            this.uiPanelSearchContainer.SuspendLayout();
            this.pnlSearch.SuspendLayout();
            this.pnlSearchBtn.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelChooseAdSchedule)).BeginInit();
            this.uiPanelChooseAdSchedule.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanel1)).BeginInit();
            this.uiPanel1.SuspendLayout();
            this.uiPanel1Container.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdExCategenList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dvCateGen)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chooseAdScheduleDs)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanel2)).BeginInit();
            this.uiPanel2.SuspendLayout();
            this.uiPanel2Container.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridEX1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdExChooseAdScheduleList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dvChannelSet)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelList)).BeginInit();
            this.uiPanelList.SuspendLayout();
            this.uiPanelListContainer.SuspendLayout();
            this.pnlUserDetail.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdExScheduleList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dvChooseAdSchedule)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelDetail)).BeginInit();
            this.uiPanelDetail.SuspendLayout();
            this.uiPanelDetailContainer.SuspendLayout();
            this.pnlDetail.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uiGroupBox2)).BeginInit();
            this.uiGroupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uiGroupBox1)).BeginInit();
            this.uiGroupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gbScheduling)).BeginInit();
            this.gbScheduling.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dvSeries)).BeginInit();
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
            this.uiPanelChooseAdSchedule.Id = new System.Guid("88f86479-98e4-43d4-bf03-12fd789fcfc5");
            this.uiPanelChooseAdSchedule.StaticGroup = true;
            this.uiPanel1.Id = new System.Guid("91eeb7fc-856c-4a9c-b37e-db742cfa9b2e");
            this.uiPanelChooseAdSchedule.Panels.Add(this.uiPanel1);
            this.uiPanel2.Id = new System.Guid("3d64f731-de32-42ba-a10f-ac3b5ac56703");
            this.uiPanelChooseAdSchedule.Panels.Add(this.uiPanel2);
            this.uiPanelUsers.Panels.Add(this.uiPanelChooseAdSchedule);
            this.uiPanelList.Id = new System.Guid("b2973a57-d3e1-4d80-9387-7a6dc3332542");
            this.uiPanelUsers.Panels.Add(this.uiPanelList);
            this.uiPanelDetail.Id = new System.Guid("c1ac152b-71d8-497e-a36e-0758f080f6ec");
            this.uiPanelUsers.Panels.Add(this.uiPanelDetail);
            this.uiPM.Panels.Add(this.uiPanelUsers);
            // 
            // Design Time Panel Info:
            // 
            this.uiPM.BeginPanelInfo();
            this.uiPM.AddDockPanelInfo(new System.Guid("b162c53a-7940-47cd-a0ed-9762c59c50d8"), Janus.Windows.UI.Dock.PanelGroupStyle.HorizontalTiles, Janus.Windows.UI.Dock.PanelDockStyle.Fill, true, new System.Drawing.Size(1010, 677), true);
            this.uiPM.AddDockPanelInfo(new System.Guid("afb3b7d0-aa79-443c-a1e8-3baba38f7a4d"), new System.Guid("b162c53a-7940-47cd-a0ed-9762c59c50d8"), 43, true);
            this.uiPM.AddDockPanelInfo(new System.Guid("88f86479-98e4-43d4-bf03-12fd789fcfc5"), new System.Guid("b162c53a-7940-47cd-a0ed-9762c59c50d8"), Janus.Windows.UI.Dock.PanelGroupStyle.VerticalTiles, true, 267, true);
            this.uiPM.AddDockPanelInfo(new System.Guid("91eeb7fc-856c-4a9c-b37e-db742cfa9b2e"), new System.Guid("88f86479-98e4-43d4-bf03-12fd789fcfc5"), 352, true);
            this.uiPM.AddDockPanelInfo(new System.Guid("3d64f731-de32-42ba-a10f-ac3b5ac56703"), new System.Guid("88f86479-98e4-43d4-bf03-12fd789fcfc5"), 495, true);
            this.uiPM.AddDockPanelInfo(new System.Guid("b2973a57-d3e1-4d80-9387-7a6dc3332542"), new System.Guid("b162c53a-7940-47cd-a0ed-9762c59c50d8"), 253, true);
            this.uiPM.AddDockPanelInfo(new System.Guid("c1ac152b-71d8-497e-a36e-0758f080f6ec"), new System.Guid("b162c53a-7940-47cd-a0ed-9762c59c50d8"), 80, true);
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
            this.uiPanelUsers.Text = "메뉴/채널별 편성현황";
            // 
            // uiPanelSearch
            // 
            this.uiPanelSearch.AllowResize = Janus.Windows.UI.InheritableBoolean.False;
            this.uiPanelSearch.CaptionVisible = Janus.Windows.UI.InheritableBoolean.False;
            this.uiPanelSearch.CloseButtonVisible = Janus.Windows.UI.InheritableBoolean.False;
            this.uiPanelSearch.InnerContainer = this.uiPanelSearchContainer;
            this.uiPanelSearch.Location = new System.Drawing.Point(0, 22);
            this.uiPanelSearch.Name = "uiPanelSearch";
            this.uiPanelSearch.Size = new System.Drawing.Size(1010, 43);
            this.uiPanelSearch.TabIndex = 4;
            this.uiPanelSearch.Text = "검색";
            // 
            // uiPanelSearchContainer
            // 
            this.uiPanelSearchContainer.Controls.Add(this.pnlSearch);
            this.uiPanelSearchContainer.Location = new System.Drawing.Point(1, 1);
            this.uiPanelSearchContainer.Name = "uiPanelSearchContainer";
            this.uiPanelSearchContainer.Size = new System.Drawing.Size(1008, 41);
            this.uiPanelSearchContainer.TabIndex = 0;
            // 
            // pnlSearch
            // 
            this.pnlSearch.BackColor = System.Drawing.SystemColors.Window;
            this.pnlSearch.Controls.Add(this.cbSearchMedia);
            this.pnlSearch.Controls.Add(this.chkAdState_40);
            this.pnlSearch.Controls.Add(this.chkAdState_30);
            this.pnlSearch.Controls.Add(this.chkAdState_20);
            this.pnlSearch.Controls.Add(this.label1);
            this.pnlSearch.Controls.Add(this.lbAdState);
            this.pnlSearch.Controls.Add(this.pnlSearchBtn);
            this.pnlSearch.Controls.Add(this.lbMsg);
            this.pnlSearch.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlSearch.Location = new System.Drawing.Point(0, 0);
            this.pnlSearch.Name = "pnlSearch";
            this.pnlSearch.Size = new System.Drawing.Size(1008, 41);
            this.pnlSearch.TabIndex = 3;
            this.pnlSearch.Paint += new System.Windows.Forms.PaintEventHandler(this.pnlSearch_Paint);
            // 
            // cbSearchMedia
            // 
            this.cbSearchMedia.BackColor = System.Drawing.Color.White;
            this.cbSearchMedia.ComboStyle = Janus.Windows.EditControls.ComboStyle.DropDownList;
            this.cbSearchMedia.Location = new System.Drawing.Point(8, 11);
            this.cbSearchMedia.Name = "cbSearchMedia";
            this.cbSearchMedia.Size = new System.Drawing.Size(120, 21);
            this.cbSearchMedia.TabIndex = 46;
            this.cbSearchMedia.Text = "매체선택";
            this.cbSearchMedia.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            // 
            // chkAdState_40
            // 
            this.chkAdState_40.Location = new System.Drawing.Point(666, 10);
            this.chkAdState_40.Name = "chkAdState_40";
            this.chkAdState_40.Size = new System.Drawing.Size(50, 23);
            this.chkAdState_40.TabIndex = 45;
            this.chkAdState_40.Text = "종료";
            // 
            // chkAdState_30
            // 
            this.chkAdState_30.Checked = true;
            this.chkAdState_30.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkAdState_30.Location = new System.Drawing.Point(610, 10);
            this.chkAdState_30.Name = "chkAdState_30";
            this.chkAdState_30.Size = new System.Drawing.Size(50, 23);
            this.chkAdState_30.TabIndex = 45;
            this.chkAdState_30.Text = "중지";
            // 
            // chkAdState_20
            // 
            this.chkAdState_20.Checked = true;
            this.chkAdState_20.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkAdState_20.Location = new System.Drawing.Point(554, 10);
            this.chkAdState_20.Name = "chkAdState_20";
            this.chkAdState_20.Size = new System.Drawing.Size(50, 23);
            this.chkAdState_20.TabIndex = 45;
            this.chkAdState_20.Text = "편성";
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(199, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(57, 16);
            this.label1.TabIndex = 44;
            this.label1.Text = "편성상태";
            // 
            // lbAdState
            // 
            this.lbAdState.Location = new System.Drawing.Point(491, 13);
            this.lbAdState.Name = "lbAdState";
            this.lbAdState.Size = new System.Drawing.Size(57, 16);
            this.lbAdState.TabIndex = 17;
            this.lbAdState.Text = "광고상태";
            // 
            // pnlSearchBtn
            // 
            this.pnlSearchBtn.BackColor = System.Drawing.SystemColors.Window;
            this.pnlSearchBtn.Controls.Add(this.btnSearch);
            this.pnlSearchBtn.Dock = System.Windows.Forms.DockStyle.Right;
            this.pnlSearchBtn.Location = new System.Drawing.Point(888, 0);
            this.pnlSearchBtn.Name = "pnlSearchBtn";
            this.pnlSearchBtn.Size = new System.Drawing.Size(120, 41);
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
            // lbMsg
            // 
            this.lbMsg.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lbMsg.Location = new System.Drawing.Point(262, 10);
            this.lbMsg.Name = "lbMsg";
            this.lbMsg.Size = new System.Drawing.Size(192, 22);
            this.lbMsg.TabIndex = 43;
            this.lbMsg.Text = "편성";
            this.lbMsg.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // uiPanelChooseAdSchedule
            // 
            this.uiPanelChooseAdSchedule.AllowResize = Janus.Windows.UI.InheritableBoolean.True;
            this.uiPanelChooseAdSchedule.CaptionStyle = Janus.Windows.UI.Dock.PanelCaptionStyle.Dark;
            this.uiPanelChooseAdSchedule.CaptionVisible = Janus.Windows.UI.InheritableBoolean.False;
            this.uiPanelChooseAdSchedule.GroupStyle = Janus.Windows.UI.Dock.PanelGroupStyle.VerticalTiles;
            this.uiPanelChooseAdSchedule.Location = new System.Drawing.Point(0, 69);
            this.uiPanelChooseAdSchedule.Name = "uiPanelChooseAdSchedule";
            this.uiPanelChooseAdSchedule.Size = new System.Drawing.Size(1010, 267);
            this.uiPanelChooseAdSchedule.TabIndex = 4;
            this.uiPanelChooseAdSchedule.Text = "메뉴/채널 목록";
            // 
            // uiPanel1
            // 
            this.uiPanel1.CaptionStyle = Janus.Windows.UI.Dock.PanelCaptionStyle.Light;
            this.uiPanel1.CaptionVisible = Janus.Windows.UI.InheritableBoolean.False;
            this.uiPanel1.InnerContainer = this.uiPanel1Container;
            this.uiPanel1.Location = new System.Drawing.Point(0, 0);
            this.uiPanel1.Name = "uiPanel1";
            this.uiPanel1.Size = new System.Drawing.Size(419, 267);
            this.uiPanel1.TabIndex = 4;
            this.uiPanel1.Text = "메뉴";
            // 
            // uiPanel1Container
            // 
            this.uiPanel1Container.Controls.Add(this.grdExCategenList);
            this.uiPanel1Container.Location = new System.Drawing.Point(1, 1);
            this.uiPanel1Container.Name = "uiPanel1Container";
            this.uiPanel1Container.Size = new System.Drawing.Size(417, 265);
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
            grdExCategenList_DesignTimeLayout.LayoutString = resources.GetString("grdExCategenList_DesignTimeLayout.LayoutString");
            this.grdExCategenList.DesignTimeLayout = grdExCategenList_DesignTimeLayout;
            this.grdExCategenList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdExCategenList.EmptyRows = true;
            this.grdExCategenList.FocusCellFormatStyle.BackColor = System.Drawing.SystemColors.Highlight;
            this.grdExCategenList.FocusCellFormatStyle.ForeColor = System.Drawing.Color.White;
            this.grdExCategenList.FocusStyle = Janus.Windows.GridEX.FocusStyle.None;
            this.grdExCategenList.FrozenColumns = 2;
            this.grdExCategenList.GridLineColor = System.Drawing.Color.Silver;
            this.grdExCategenList.GridLines = Janus.Windows.GridEX.GridLines.Vertical;
            this.grdExCategenList.GridLineStyle = Janus.Windows.GridEX.GridLineStyle.Solid;
            this.grdExCategenList.GroupByBoxVisible = false;
            this.grdExCategenList.GroupMode = Janus.Windows.GridEX.GroupMode.Collapsed;
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
            this.grdExCategenList.Size = new System.Drawing.Size(417, 265);
            this.grdExCategenList.TabIndex = 3;
            this.grdExCategenList.TabKeyBehavior = Janus.Windows.GridEX.TabKeyBehavior.ControlNavigation;
            this.grdExCategenList.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            this.grdExCategenList.SelectionChanged += new System.EventHandler(this.OnGrdRowChangedGanre);
            this.grdExCategenList.Enter += new System.EventHandler(this.OnGrdRowChangedGanre);
            // 
            // dvCateGen
            // 
            this.dvCateGen.Table = this.chooseAdScheduleDs.Categens;
            // 
            // chooseAdScheduleDs
            // 
            this.chooseAdScheduleDs.DataSetName = "ChooseAdScheduleDs";
            this.chooseAdScheduleDs.Locale = new System.Globalization.CultureInfo("");
            this.chooseAdScheduleDs.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // uiPanel2
            // 
            this.uiPanel2.CaptionStyle = Janus.Windows.UI.Dock.PanelCaptionStyle.Light;
            this.uiPanel2.CaptionVisible = Janus.Windows.UI.InheritableBoolean.False;
            this.uiPanel2.InnerContainer = this.uiPanel2Container;
            this.uiPanel2.Location = new System.Drawing.Point(423, 0);
            this.uiPanel2.Name = "uiPanel2";
            this.uiPanel2.Size = new System.Drawing.Size(587, 267);
            this.uiPanel2.TabIndex = 4;
            this.uiPanel2.Text = "채널";
            // 
            // uiPanel2Container
            // 
            this.uiPanel2Container.Controls.Add(this.gridEX1);
            this.uiPanel2Container.Controls.Add(this.grdExChooseAdScheduleList);
            this.uiPanel2Container.Location = new System.Drawing.Point(1, 1);
            this.uiPanel2Container.Name = "uiPanel2Container";
            this.uiPanel2Container.Size = new System.Drawing.Size(585, 265);
            this.uiPanel2Container.TabIndex = 0;
            // 
            // gridEX1
            // 
            this.gridEX1.AllowEdit = Janus.Windows.GridEX.InheritableBoolean.False;
            this.gridEX1.AlternatingColors = true;
            this.gridEX1.BorderStyle = Janus.Windows.GridEX.BorderStyle.None;
            gridEX1_DesignTimeLayout_Reference_0.Instance = ((object)(resources.GetObject("gridEX1_DesignTimeLayout_Reference_0.Instance")));
            gridEX1_DesignTimeLayout_Reference_1.Instance = ((object)(resources.GetObject("gridEX1_DesignTimeLayout_Reference_1.Instance")));
            gridEX1_DesignTimeLayout_Reference_2.Instance = ((object)(resources.GetObject("gridEX1_DesignTimeLayout_Reference_2.Instance")));
            gridEX1_DesignTimeLayout.LayoutReferences.AddRange(new Janus.Windows.Common.Layouts.JanusLayoutReference[] {
            gridEX1_DesignTimeLayout_Reference_0,
            gridEX1_DesignTimeLayout_Reference_1,
            gridEX1_DesignTimeLayout_Reference_2});
            gridEX1_DesignTimeLayout.LayoutString = resources.GetString("gridEX1_DesignTimeLayout.LayoutString");
            this.gridEX1.DesignTimeLayout = gridEX1_DesignTimeLayout;
            this.gridEX1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridEX1.EmptyRows = true;
            this.gridEX1.FocusCellFormatStyle.BackColor = System.Drawing.SystemColors.Highlight;
            this.gridEX1.FocusCellFormatStyle.ForeColor = System.Drawing.Color.White;
            this.gridEX1.FocusStyle = Janus.Windows.GridEX.FocusStyle.None;
            this.gridEX1.GridLineColor = System.Drawing.Color.Silver;
            this.gridEX1.GridLines = Janus.Windows.GridEX.GridLines.Vertical;
            this.gridEX1.GridLineStyle = Janus.Windows.GridEX.GridLineStyle.Solid;
            this.gridEX1.GroupByBoxVisible = false;
            this.gridEX1.HeaderFormatStyle.TextAlignment = Janus.Windows.GridEX.TextAlignment.Center;
            this.gridEX1.HideSelection = Janus.Windows.GridEX.HideSelection.HighlightInactive;
            this.gridEX1.Hierarchical = true;
            this.gridEX1.Location = new System.Drawing.Point(0, 0);
            this.gridEX1.Name = "gridEX1";
            this.gridEX1.ScrollBars = Janus.Windows.GridEX.ScrollBars.Vertical;
            this.gridEX1.SelectedInactiveFormatStyle.BackColor = System.Drawing.Color.Gold;
            this.gridEX1.Size = new System.Drawing.Size(585, 265);
            this.gridEX1.TabIndex = 5;
            this.gridEX1.TabKeyBehavior = Janus.Windows.GridEX.TabKeyBehavior.ControlNavigation;
            this.gridEX1.ThemedAreas = ((Janus.Windows.GridEX.ThemedArea)(((((((((Janus.Windows.GridEX.ThemedArea.ScrollBars | Janus.Windows.GridEX.ThemedArea.EditControls) 
            | Janus.Windows.GridEX.ThemedArea.Headers) 
            | Janus.Windows.GridEX.ThemedArea.GroupByBox) 
            | Janus.Windows.GridEX.ThemedArea.GroupRows) 
            | Janus.Windows.GridEX.ThemedArea.ControlBorder) 
            | Janus.Windows.GridEX.ThemedArea.Cards) 
            | Janus.Windows.GridEX.ThemedArea.Gridlines) 
            | Janus.Windows.GridEX.ThemedArea.CheckBoxes)));
            this.gridEX1.TreeLineColor = System.Drawing.Color.Silver;
            this.gridEX1.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            // 
            // grdExChooseAdScheduleList
            // 
            this.grdExChooseAdScheduleList.AllowEdit = Janus.Windows.GridEX.InheritableBoolean.False;
            this.grdExChooseAdScheduleList.AlternatingColors = true;
            this.grdExChooseAdScheduleList.BorderStyle = Janus.Windows.GridEX.BorderStyle.None;
            this.grdExChooseAdScheduleList.DataSource = this.dvChannelSet;
            grdExChooseAdScheduleList_DesignTimeLayout_Reference_0.Instance = ((object)(resources.GetObject("grdExChooseAdScheduleList_DesignTimeLayout_Reference_0.Instance")));
            grdExChooseAdScheduleList_DesignTimeLayout_Reference_1.Instance = ((object)(resources.GetObject("grdExChooseAdScheduleList_DesignTimeLayout_Reference_1.Instance")));
            grdExChooseAdScheduleList_DesignTimeLayout_Reference_2.Instance = ((object)(resources.GetObject("grdExChooseAdScheduleList_DesignTimeLayout_Reference_2.Instance")));
            grdExChooseAdScheduleList_DesignTimeLayout.LayoutReferences.AddRange(new Janus.Windows.Common.Layouts.JanusLayoutReference[] {
            grdExChooseAdScheduleList_DesignTimeLayout_Reference_0,
            grdExChooseAdScheduleList_DesignTimeLayout_Reference_1,
            grdExChooseAdScheduleList_DesignTimeLayout_Reference_2});
            grdExChooseAdScheduleList_DesignTimeLayout.LayoutString = resources.GetString("grdExChooseAdScheduleList_DesignTimeLayout.LayoutString");
            this.grdExChooseAdScheduleList.DesignTimeLayout = grdExChooseAdScheduleList_DesignTimeLayout;
            this.grdExChooseAdScheduleList.EmptyRows = true;
            this.grdExChooseAdScheduleList.FocusCellFormatStyle.BackColor = System.Drawing.SystemColors.Highlight;
            this.grdExChooseAdScheduleList.FocusCellFormatStyle.ForeColor = System.Drawing.Color.White;
            this.grdExChooseAdScheduleList.FocusStyle = Janus.Windows.GridEX.FocusStyle.None;
            this.grdExChooseAdScheduleList.GridLineColor = System.Drawing.Color.Silver;
            this.grdExChooseAdScheduleList.GridLines = Janus.Windows.GridEX.GridLines.Vertical;
            this.grdExChooseAdScheduleList.GridLineStyle = Janus.Windows.GridEX.GridLineStyle.Solid;
            this.grdExChooseAdScheduleList.GroupByBoxVisible = false;
            this.grdExChooseAdScheduleList.HeaderFormatStyle.TextAlignment = Janus.Windows.GridEX.TextAlignment.Center;
            this.grdExChooseAdScheduleList.HideSelection = Janus.Windows.GridEX.HideSelection.HighlightInactive;
            this.grdExChooseAdScheduleList.Hierarchical = true;
            this.grdExChooseAdScheduleList.Location = new System.Drawing.Point(33, 52);
            this.grdExChooseAdScheduleList.Name = "grdExChooseAdScheduleList";
            this.grdExChooseAdScheduleList.ScrollBars = Janus.Windows.GridEX.ScrollBars.Vertical;
            this.grdExChooseAdScheduleList.SelectedInactiveFormatStyle.BackColor = System.Drawing.Color.Gold;
            this.grdExChooseAdScheduleList.Size = new System.Drawing.Size(248, 111);
            this.grdExChooseAdScheduleList.TabIndex = 4;
            this.grdExChooseAdScheduleList.TabKeyBehavior = Janus.Windows.GridEX.TabKeyBehavior.ControlNavigation;
            this.grdExChooseAdScheduleList.ThemedAreas = ((Janus.Windows.GridEX.ThemedArea)(((((((((Janus.Windows.GridEX.ThemedArea.ScrollBars | Janus.Windows.GridEX.ThemedArea.EditControls) 
            | Janus.Windows.GridEX.ThemedArea.Headers) 
            | Janus.Windows.GridEX.ThemedArea.GroupByBox) 
            | Janus.Windows.GridEX.ThemedArea.GroupRows) 
            | Janus.Windows.GridEX.ThemedArea.ControlBorder) 
            | Janus.Windows.GridEX.ThemedArea.Cards) 
            | Janus.Windows.GridEX.ThemedArea.Gridlines) 
            | Janus.Windows.GridEX.ThemedArea.CheckBoxes)));
            this.grdExChooseAdScheduleList.TreeLineColor = System.Drawing.Color.Silver;
            this.grdExChooseAdScheduleList.Visible = false;
            this.grdExChooseAdScheduleList.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            this.grdExChooseAdScheduleList.SelectionChanged += new System.EventHandler(this.OnGrdRowChangedChannel);
            this.grdExChooseAdScheduleList.Enter += new System.EventHandler(this.OnGrdRowChangedChannel);
            // 
            // dvChannelSet
            // 
            this.dvChannelSet.Table = this.chooseAdScheduleDs.ChannelSets;
            // 
            // uiPanelList
            // 
            this.uiPanelList.ActiveCaptionMode = Janus.Windows.UI.Dock.ActiveCaptionMode.Never;
            this.uiPanelList.AllowResize = Janus.Windows.UI.InheritableBoolean.True;
            this.uiPanelList.BackColor = System.Drawing.SystemColors.Window;
            this.uiPanelList.CaptionFormatStyle.FontBold = Janus.Windows.UI.TriState.True;
            this.uiPanelList.CaptionFormatStyle.FontSize = 9F;
            this.uiPanelList.CaptionHeight = 26;
            this.uiPanelList.CaptionStyle = Janus.Windows.UI.Dock.PanelCaptionStyle.Light;
            this.uiPanelList.InnerAreaStyle = Janus.Windows.UI.Dock.PanelInnerAreaStyle.AppWorkspace;
            this.uiPanelList.InnerContainer = this.uiPanelListContainer;
            this.uiPanelList.Location = new System.Drawing.Point(0, 340);
            this.uiPanelList.Name = "uiPanelList";
            this.uiPanelList.Size = new System.Drawing.Size(1010, 253);
            this.uiPanelList.TabIndex = 4;
            this.uiPanelList.Text = "메뉴별편성현황";
            // 
            // uiPanelListContainer
            // 
            this.uiPanelListContainer.Controls.Add(this.pnlUserDetail);
            this.uiPanelListContainer.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.uiPanelListContainer.Location = new System.Drawing.Point(1, 26);
            this.uiPanelListContainer.Name = "uiPanelListContainer";
            this.uiPanelListContainer.Size = new System.Drawing.Size(1008, 226);
            this.uiPanelListContainer.TabIndex = 0;
            // 
            // pnlUserDetail
            // 
            this.pnlUserDetail.BackColor = System.Drawing.SystemColors.Window;
            this.pnlUserDetail.Controls.Add(this.grdExScheduleList);
            this.pnlUserDetail.Controls.Add(this.btnGenreName);
            this.pnlUserDetail.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlUserDetail.Location = new System.Drawing.Point(0, 0);
            this.pnlUserDetail.Name = "pnlUserDetail";
            this.pnlUserDetail.Size = new System.Drawing.Size(1008, 226);
            this.pnlUserDetail.TabIndex = 3;
            // 
            // grdExScheduleList
            // 
            this.grdExScheduleList.AutomaticSort = false;
            this.grdExScheduleList.BorderStyle = Janus.Windows.GridEX.BorderStyle.None;
            this.grdExScheduleList.DataSource = this.dvChooseAdSchedule;
            grdExScheduleList_DesignTimeLayout.LayoutString = resources.GetString("grdExScheduleList_DesignTimeLayout.LayoutString");
            this.grdExScheduleList.DesignTimeLayout = grdExScheduleList_DesignTimeLayout;
            this.grdExScheduleList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdExScheduleList.EditorsControlStyle.ButtonAppearance = Janus.Windows.GridEX.ButtonAppearance.Regular;
            this.grdExScheduleList.EmptyRows = true;
            this.grdExScheduleList.FocusCellFormatStyle.BackColor = System.Drawing.SystemColors.Highlight;
            this.grdExScheduleList.FocusCellFormatStyle.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.grdExScheduleList.FocusStyle = Janus.Windows.GridEX.FocusStyle.None;
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
            this.grdExScheduleList.SelectedInactiveFormatStyle.BackColor = System.Drawing.Color.Gold;
            this.grdExScheduleList.Size = new System.Drawing.Size(1008, 226);
            this.grdExScheduleList.TabIndex = 5;
            this.grdExScheduleList.TabKeyBehavior = Janus.Windows.GridEX.TabKeyBehavior.ControlNavigation;
            this.grdExScheduleList.ThemedAreas = ((Janus.Windows.GridEX.ThemedArea)(((((((((Janus.Windows.GridEX.ThemedArea.ScrollBars | Janus.Windows.GridEX.ThemedArea.EditControls) 
            | Janus.Windows.GridEX.ThemedArea.Headers) 
            | Janus.Windows.GridEX.ThemedArea.GroupByBox) 
            | Janus.Windows.GridEX.ThemedArea.GroupRows) 
            | Janus.Windows.GridEX.ThemedArea.ControlBorder) 
            | Janus.Windows.GridEX.ThemedArea.Cards) 
            | Janus.Windows.GridEX.ThemedArea.Gridlines) 
            | Janus.Windows.GridEX.ThemedArea.CheckBoxes)));
            this.grdExScheduleList.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            this.grdExScheduleList.CellValueChanged += new Janus.Windows.GridEX.ColumnActionEventHandler(this.grdExScheduleList_CellValueChanged);
            this.grdExScheduleList.ColumnHeaderClick += new Janus.Windows.GridEX.ColumnActionEventHandler(this.grdExScheduleList_ColumnHeaderClick);
            this.grdExScheduleList.SelectionChanged += new System.EventHandler(this.OnGrdRowChangedSchedule);
            // 
            // dvChooseAdSchedule
            // 
            this.dvChooseAdSchedule.Table = this.chooseAdScheduleDs.ChooseAdSchedule;
            // 
            // btnGenreName
            // 
            this.btnGenreName.BackColor = System.Drawing.SystemColors.Window;
            this.btnGenreName.Location = new System.Drawing.Point(808, 8);
            this.btnGenreName.Name = "btnGenreName";
            this.btnGenreName.Size = new System.Drawing.Size(26, 21);
            this.btnGenreName.TabIndex = 6;
            this.btnGenreName.UseVisualStyleBackColor = false;
            // 
            // uiPanelDetail
            // 
            this.uiPanelDetail.AllowResize = Janus.Windows.UI.InheritableBoolean.False;
            this.uiPanelDetail.CaptionStyle = Janus.Windows.UI.Dock.PanelCaptionStyle.Dark;
            this.uiPanelDetail.InnerContainer = this.uiPanelDetailContainer;
            this.uiPanelDetail.Location = new System.Drawing.Point(0, 597);
            this.uiPanelDetail.Name = "uiPanelDetail";
            this.uiPanelDetail.Size = new System.Drawing.Size(1010, 80);
            this.uiPanelDetail.TabIndex = 4;
            this.uiPanelDetail.Text = "편성";
            // 
            // uiPanelDetailContainer
            // 
            this.uiPanelDetailContainer.Controls.Add(this.pnlDetail);
            this.uiPanelDetailContainer.Location = new System.Drawing.Point(1, 23);
            this.uiPanelDetailContainer.Name = "uiPanelDetailContainer";
            this.uiPanelDetailContainer.Size = new System.Drawing.Size(1008, 56);
            this.uiPanelDetailContainer.TabIndex = 0;
            // 
            // pnlDetail
            // 
            this.pnlDetail.BackColor = System.Drawing.SystemColors.Window;
            this.pnlDetail.Controls.Add(this.uiGroupBox2);
            this.pnlDetail.Controls.Add(this.uiGroupBox1);
            this.pnlDetail.Controls.Add(this.gbScheduling);
            this.pnlDetail.Controls.Add(this.btnDelete);
            this.pnlDetail.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlDetail.Location = new System.Drawing.Point(0, 0);
            this.pnlDetail.Name = "pnlDetail";
            this.pnlDetail.Size = new System.Drawing.Size(1008, 56);
            this.pnlDetail.TabIndex = 6;
            // 
            // uiGroupBox2
            // 
            this.uiGroupBox2.Controls.Add(this.btnAdd_12);
            this.uiGroupBox2.Controls.Add(this.btnAdd_11);
            this.uiGroupBox2.Controls.Add(this.btnAdd_20);
            this.uiGroupBox2.Font = new System.Drawing.Font("맑은 고딕", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.uiGroupBox2.Location = new System.Drawing.Point(220, 4);
            this.uiGroupBox2.Name = "uiGroupBox2";
            this.uiGroupBox2.Size = new System.Drawing.Size(204, 44);
            this.uiGroupBox2.TabIndex = 13;
            this.uiGroupBox2.Text = "매체광고 편성추가";
            this.uiGroupBox2.Visible = false;
            this.uiGroupBox2.VisualStyle = Janus.Windows.UI.Dock.PanelVisualStyle.Office2003;
            // 
            // btnAdd_12
            // 
            this.btnAdd_12.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnAdd_12.Location = new System.Drawing.Point(136, 17);
            this.btnAdd_12.Name = "btnAdd_12";
            this.btnAdd_12.Size = new System.Drawing.Size(60, 22);
            this.btnAdd_12.TabIndex = 11;
            this.btnAdd_12.Text = "SCM";
            this.btnAdd_12.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.btnAdd_12.Click += new System.EventHandler(this.btnAdd_12_Click);
            // 
            // btnAdd_11
            // 
            this.btnAdd_11.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnAdd_11.Location = new System.Drawing.Point(72, 17);
            this.btnAdd_11.Name = "btnAdd_11";
            this.btnAdd_11.Size = new System.Drawing.Size(60, 22);
            this.btnAdd_11.TabIndex = 10;
            this.btnAdd_11.Text = "EAP";
            this.btnAdd_11.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.btnAdd_11.Click += new System.EventHandler(this.btnAdd_11_Click);
            // 
            // btnAdd_20
            // 
            this.btnAdd_20.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnAdd_20.Location = new System.Drawing.Point(8, 17);
            this.btnAdd_20.Name = "btnAdd_20";
            this.btnAdd_20.Size = new System.Drawing.Size(60, 22);
            this.btnAdd_20.TabIndex = 9;
            this.btnAdd_20.Text = "OAP";
            this.btnAdd_20.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.btnAdd_20.Click += new System.EventHandler(this.btnAdd_20_Click);
            // 
            // uiGroupBox1
            // 
            this.uiGroupBox1.Controls.Add(this.btnAdd_19);
            this.uiGroupBox1.Controls.Add(this.btnAdd_16);
            this.uiGroupBox1.Controls.Add(this.btnAdd_10);
            this.uiGroupBox1.Font = new System.Drawing.Font("맑은 고딕", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.uiGroupBox1.Location = new System.Drawing.Point(8, 4);
            this.uiGroupBox1.Name = "uiGroupBox1";
            this.uiGroupBox1.Size = new System.Drawing.Size(204, 44);
            this.uiGroupBox1.TabIndex = 9;
            this.uiGroupBox1.Text = "상업광고 편성추가";
            this.uiGroupBox1.VisualStyle = Janus.Windows.UI.Dock.PanelVisualStyle.Office2003;
            // 
            // btnAdd_19
            // 
            this.btnAdd_19.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnAdd_19.Location = new System.Drawing.Point(136, 17);
            this.btnAdd_19.Name = "btnAdd_19";
            this.btnAdd_19.Size = new System.Drawing.Size(60, 22);
            this.btnAdd_19.TabIndex = 11;
            this.btnAdd_19.Text = "Post";
            this.btnAdd_19.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.btnAdd_19.Click += new System.EventHandler(this.btnAdd_19_Click);
            // 
            // btnAdd_16
            // 
            this.btnAdd_16.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnAdd_16.Location = new System.Drawing.Point(72, 17);
            this.btnAdd_16.Name = "btnAdd_16";
            this.btnAdd_16.Size = new System.Drawing.Size(60, 22);
            this.btnAdd_16.TabIndex = 10;
            this.btnAdd_16.Text = "Mid";
            this.btnAdd_16.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.btnAdd_16.Click += new System.EventHandler(this.btnAdd_16_Click);
            // 
            // btnAdd_10
            // 
            this.btnAdd_10.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnAdd_10.Location = new System.Drawing.Point(8, 17);
            this.btnAdd_10.Name = "btnAdd_10";
            this.btnAdd_10.Size = new System.Drawing.Size(60, 22);
            this.btnAdd_10.TabIndex = 9;
            this.btnAdd_10.Text = "Pre";
            this.btnAdd_10.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.btnAdd_10.Click += new System.EventHandler(this.btnAdd_10_Click);
            // 
            // gbScheduling
            // 
            this.gbScheduling.Controls.Add(this.btnOrderUp);
            this.gbScheduling.Controls.Add(this.btnOrderDown);
            this.gbScheduling.Controls.Add(this.btnOrderFirst);
            this.gbScheduling.Controls.Add(this.btnOrderLast);
            this.gbScheduling.Font = new System.Drawing.Font("맑은 고딕", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.gbScheduling.Location = new System.Drawing.Point(672, 3);
            this.gbScheduling.Name = "gbScheduling";
            this.gbScheduling.Size = new System.Drawing.Size(328, 44);
            this.gbScheduling.TabIndex = 8;
            this.gbScheduling.Text = "편성순서변경 : 편성순서의 변경은 OAP만 가능합니다. ";
            this.gbScheduling.Visible = false;
            this.gbScheduling.VisualStyle = Janus.Windows.UI.Dock.PanelVisualStyle.Office2003;
            // 
            // btnOrderUp
            // 
            this.btnOrderUp.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnOrderUp.Enabled = false;
            this.btnOrderUp.Location = new System.Drawing.Point(88, 17);
            this.btnOrderUp.Name = "btnOrderUp";
            this.btnOrderUp.Size = new System.Drawing.Size(72, 22);
            this.btnOrderUp.TabIndex = 10;
            this.btnOrderUp.Text = "순서올림(▲)";
            this.btnOrderUp.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.btnOrderUp.Click += new System.EventHandler(this.btnOrderUp_Click);
            // 
            // btnOrderDown
            // 
            this.btnOrderDown.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnOrderDown.Enabled = false;
            this.btnOrderDown.Location = new System.Drawing.Point(168, 17);
            this.btnOrderDown.Name = "btnOrderDown";
            this.btnOrderDown.Size = new System.Drawing.Size(72, 22);
            this.btnOrderDown.TabIndex = 11;
            this.btnOrderDown.Text = "순서내림(▼)";
            this.btnOrderDown.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.btnOrderDown.Click += new System.EventHandler(this.btnOrderDown_Click);
            // 
            // btnOrderFirst
            // 
            this.btnOrderFirst.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnOrderFirst.Enabled = false;
            this.btnOrderFirst.Location = new System.Drawing.Point(8, 17);
            this.btnOrderFirst.Name = "btnOrderFirst";
            this.btnOrderFirst.Size = new System.Drawing.Size(72, 22);
            this.btnOrderFirst.TabIndex = 9;
            this.btnOrderFirst.Text = "첫순서로";
            this.btnOrderFirst.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.btnOrderFirst.Click += new System.EventHandler(this.btnOrderFirst_Click);
            // 
            // btnOrderLast
            // 
            this.btnOrderLast.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnOrderLast.Enabled = false;
            this.btnOrderLast.Location = new System.Drawing.Point(248, 17);
            this.btnOrderLast.Name = "btnOrderLast";
            this.btnOrderLast.Size = new System.Drawing.Size(72, 22);
            this.btnOrderLast.TabIndex = 12;
            this.btnOrderLast.Text = "끝순서로";
            this.btnOrderLast.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.btnOrderLast.Click += new System.EventHandler(this.btnOrderLast_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnDelete.Enabled = false;
            this.btnDelete.FlatBorderColor = System.Drawing.Color.DimGray;
            this.btnDelete.Location = new System.Drawing.Point(442, 18);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(65, 24);
            this.btnDelete.StateStyles.FormatStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(111)))), ((int)(((byte)(169)))));
            this.btnDelete.StateStyles.FormatStyle.BackColorAlphaMode = Janus.Windows.UI.AlphaMode.UseAlpha;
            this.btnDelete.StateStyles.FormatStyle.BackColorGradient = System.Drawing.Color.FromArgb(((int)(((byte)(205)))), ((int)(((byte)(6)))), ((int)(((byte)(48)))));
            this.btnDelete.StateStyles.FormatStyle.BackgroundGradientMode = Janus.Windows.UI.BackgroundGradientMode.Vertical;
            this.btnDelete.TabIndex = 8;
            this.btnDelete.Text = "삭 제";
            this.btnDelete.UseThemes = false;
            this.btnDelete.VisualStyle = Janus.Windows.UI.VisualStyle.Office2003;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
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
            // dvSeries
            // 
            this.dvSeries.Table = this.chooseAdScheduleDs.Series;
            // 
            // ChooseAdScheduleControl
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.uiPanelUsers);
            this.Font = global::AdManagerClient.Properties.Settings.Default.Font1;
            this.Name = "ChooseAdScheduleControl";
            this.Size = new System.Drawing.Size(1010, 677);
            this.Load += new System.EventHandler(this.ChooseAdScheduleControl_Load);
            ((System.ComponentModel.ISupportInitialize)(this.uiPM)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelUsers)).EndInit();
            this.uiPanelUsers.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelSearch)).EndInit();
            this.uiPanelSearch.ResumeLayout(false);
            this.uiPanelSearchContainer.ResumeLayout(false);
            this.pnlSearch.ResumeLayout(false);
            this.pnlSearchBtn.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelChooseAdSchedule)).EndInit();
            this.uiPanelChooseAdSchedule.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.uiPanel1)).EndInit();
            this.uiPanel1.ResumeLayout(false);
            this.uiPanel1Container.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdExCategenList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dvCateGen)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chooseAdScheduleDs)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanel2)).EndInit();
            this.uiPanel2.ResumeLayout(false);
            this.uiPanel2Container.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridEX1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdExChooseAdScheduleList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dvChannelSet)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelList)).EndInit();
            this.uiPanelList.ResumeLayout(false);
            this.uiPanelListContainer.ResumeLayout(false);
            this.pnlUserDetail.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdExScheduleList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dvChooseAdSchedule)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelDetail)).EndInit();
            this.uiPanelDetail.ResumeLayout(false);
            this.uiPanelDetailContainer.ResumeLayout(false);
            this.pnlDetail.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.uiGroupBox2)).EndInit();
            this.uiGroupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.uiGroupBox1)).EndInit();
            this.uiGroupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gbScheduling)).EndInit();
            this.gbScheduling.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dvSeries)).EndInit();
            this.ResumeLayout(false);

        }
        #endregion

        #region 컨트롤 로드
        private void ChooseAdScheduleControl_Load(object sender, System.EventArgs e)
        {
            // 데이터관리용 객체생성
            dtMenu= ((DataView)grdExCategenList.DataSource).Table;
            cmMenu= (CurrencyManager) this.BindingContext[grdExCategenList.DataSource]; 

			cmChannel = (CurrencyManager) this.BindingContext[grdExChooseAdScheduleList.DataSource]; 
			dtChannel  = ((DataView)grdExChooseAdScheduleList.DataSource).Table;
						
			cmSchedule = (CurrencyManager) this.BindingContext[grdExScheduleList.DataSource]; 
			dtSchedule = ((DataView)grdExScheduleList.DataSource).Table;

			// 컨트롤 초기화
			InitControl();
		}

        #endregion

        #region 컨트롤 초기화
        private void InitControl()
        {
            ProgressStart();

			lbMsg.Text = "";
            InitCombo();
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
			
			if(canRead) SearchMenu();
        }
        /// <summary>
        /// 매체선택 초기화
        /// </summary>
        private void InitCombo()
        {
            // 코드에서 보안레벨을 조회한다.
            MediaCodeModel mediacodeModel = new MediaCodeModel();
            new MediaCodeManager(systemModel, commonModel).GetMediaCodeList(mediacodeModel);

            if (mediacodeModel.ResultCD.Equals("0000"))
            {
                // 데이터셋에 셋팅
                Utility.SetDataTable(chooseAdScheduleDs.Medias, mediacodeModel.MediaCodeDataSet);
            }

            // 상세조회 콤보
            // 상세정보의 콤보는 Dataset을 데이터소스로 가진다.

            // 검색조건의 콤보
            this.cbSearchMedia.Items.Clear();

            // 콤보박스에 셋트할 코드목록을 담을 Item배열을 선언
            Janus.Windows.EditControls.UIComboBoxItem[] comboItems = new Janus.Windows.EditControls.UIComboBoxItem[mediacodeModel.ResultCnt + 1];

            comboItems[0] = new Janus.Windows.EditControls.UIComboBoxItem("매체선택", "00");

            for (int i = 0; i < mediacodeModel.ResultCnt; i++)
            {
                DataRow row = chooseAdScheduleDs.Medias.Rows[i];

                string val = row["MediaCode"].ToString();
                string txt = row["MediaName"].ToString();
                comboItems[i + 1] = new Janus.Windows.EditControls.UIComboBoxItem(txt, val);
            }
            // 콤보에 셋트
            this.cbSearchMedia.Items.AddRange(comboItems);
            if (mediacodeModel.ResultCnt > 1)
                this.cbSearchMedia.SelectedIndex = 1;
            else this.cbSearchMedia.SelectedIndex = 0;

            Application.DoEvents();
        }
		private void InitCombo_Level()
        {
            Application.DoEvents();
        }

        private void InitButton()
        {
			if(canRead)   btnSearch.Enabled = true;

            Application.DoEvents();
        }

        private void DisableButton()
        {
            btnSearch.Enabled = false;
            btnAdd_10.Enabled = false;
            btnAdd_11.Enabled = false;
            btnAdd_12.Enabled = false;
            btnAdd_16.Enabled = false;
            btnAdd_20.Enabled = false;
			btnDelete.Enabled = false;
			Application.DoEvents();
        }

        #endregion

        #region 사용자 액션처리 메소드

        /// <summary>
        /// 왼쪽 카테고리/장르 그리드의 SelectedRow가 변경될때 처리함수
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnGrdRowChangedGanre(object sender, System.EventArgs e) 
        {
            if (!IsSearching) // 2011.11.29 JH.Park 조회중이 아닐경우에만 동작하도록 변경
            {
                if (IsNotLoading)
                {
                    SetDetailTextMenu();
                    InitButton();
                }
            }
        }
		
		/// <summary>
		/// 오른쪽 채널/회차별 그리드의 SelectedRow가 변경될때 처리함수
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
        private void OnGrdRowChangedChannel(object sender, System.EventArgs e) 
        {
            if (!IsSearching) // 2011.11.29 JH.Park 조회중이 아닐경우에만 동작하도록 변경
            {
                if (IsNotLoading)
                {
                    SetDetailTextChannel();
                    InitButton();
                }
            }
		}

		private void OnGrdRowChangedSchedule(object sender, System.EventArgs e) 
		{
            if (!IsSearching) // 2011.11.29 JH.Park 조회중이 아닐경우에만 동작하도록 변경
            {
                SetDetailTextSchedule();
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
            InitButton();
        }

		private void btnAdd_Click(object sender, System.EventArgs e)
		{
			//  지정광고 대상목록 검색창 
			//ChooseAdSearch_pForm pForm = new ChooseAdSearch_pForm(this);
            pForm = new ItemMultiChoiceForm(this);
			
            /*
			switch(keyScheduleType)
			{
				case TYPE_Schedule.Genre:
					pForm.ScheduleType	= keyScheduleType;
					pForm.keyMediaCode = keyMediaCode;
					pForm.keyGenreCode = keyGenreCode;
					break;
				case TYPE_Schedule.Channel:
					pForm.ScheduleType	= keyScheduleType;
					pForm.keyMediaCode = keyMediaCode;
					pForm.keyChannelNo = keyChannelNo;
					break;
			}
             * */
            pForm.callType = "ChooseAdScheduleControl";
            pForm.ReturnDate += new ItemMultiChoiceForm.PopupService(ItemMultiChoiceForm_Return);
			pForm.ShowDialog();            
			pForm.Dispose();
			pForm = null;
		}


		private void btnDelete_Click(object sender, System.EventArgs e)
		{
			switch(keyScheduleType)
			{
				case TYPE_Schedule.Genre:
					DeleteChoiceAdMenuDetail();		
					break;
				case TYPE_Schedule.Channel:
					DeleteChoiceAdChannelDetail();		
					break;
			}
		}


		private void btnOrderFirst_Click(object sender, System.EventArgs e)
		{
			switch(keyScheduleType)
			{
				case TYPE_Schedule.Genre:
					OrderSetScheduleMenuAd(ORDER_FIRST);
					break;
				case TYPE_Schedule.Channel:
					OrderSetScheduleChannelAd(ORDER_FIRST);
					break;
			}		
		}


		private void btnOrderUp_Click(object sender, System.EventArgs e)
		{
			switch(keyScheduleType)
			{
				case TYPE_Schedule.Genre:
					OrderSetScheduleMenuAd(ORDER_UP);
					break;
				case TYPE_Schedule.Channel:
					OrderSetScheduleChannelAd(ORDER_UP);
					break;
			}				
		}

		private void btnOrderDown_Click(object sender, System.EventArgs e)
		{
			switch(keyScheduleType)
			{
				case TYPE_Schedule.Genre:
					OrderSetScheduleMenuAd(ORDER_DOWN);
					break;
				case TYPE_Schedule.Channel:
					OrderSetScheduleChannelAd(ORDER_DOWN);
					break;
			}				
		}

		private void btnOrderLast_Click(object sender, System.EventArgs e)
		{
			switch(keyScheduleType)
			{
				case TYPE_Schedule.Genre:
					OrderSetScheduleMenuAd(ORDER_LAST);
					break;
				case TYPE_Schedule.Channel:
					OrderSetScheduleChannelAd(ORDER_LAST);
					break;
			}				
		}

		private void grdExScheduleList_ColumnHeaderClick(object sender, Janus.Windows.GridEX.ColumnActionEventArgs e)
		{

			//컬럼Index 0(체크박스컬럼이)이 아니면 빠져나가게 처리.
			if(e.Column.Index != 0)
			{
				return;
			}
            
			//ColumnHeader Click시에 dt Setting 
			DataRow[] foundRows = dtSchedule.Select("CheckYn = 'False'");
         
			// if(grdExItemList.CurrentColumn.Position == 0){
			if( foundRows.Length == 0 )
			{
				for(int i=0;i < dtSchedule.Rows.Count;i++)
				{
					dtSchedule.Rows[i].BeginEdit();
					dtSchedule.Rows[i]["CheckYn"]="False";
					dtSchedule.Rows[i].EndEdit();
				}
			}
			else
			{
				for(int i=0;i < dtSchedule.Rows.Count;i++)
				{
					dtSchedule.Rows[i].BeginEdit();
					dtSchedule.Rows[i]["CheckYn"]="True";
					dtSchedule.Rows[i].EndEdit();
				}
			}
		}

   
        #endregion

        #region 처리메소드

        /// <summary>
        /// 채널/메뉴 편성현황 목록 조회
        /// </summary>
        private void SearchMenu()
        {
            IsSearching = true;

            StatusMessage("메뉴 정보를 조회합니다.");

            try
            {
                ProgressStart();
				// 편성배포승인 처리상태를 조회한다.
				keyAckNoCm    = "";
				keyAckStateCm = "";

				keyAckNoOap    = "";
				keyAckStateOap = "";

				schPublishModel.Init();
				schPublishModel.SearchMediaCode = "1";

				// 현재 승인상태조회 서비스를 호출한다.
				new SchPublishManager(systemModel,commonModel).GetPublishState(schPublishModel,10);

				if (schPublishModel.ResultCD.Equals("0000"))
				{
					keyAckNoCm    = schPublishModel.AckNo;
					keyAckStateCm = schPublishModel.State;
				}

				new SchPublishManager(systemModel,commonModel).GetPublishState(schPublishModel,20);

				if (schPublishModel.ResultCD.Equals("0000"))
				{
					keyAckNoOap    = schPublishModel.AckNo;
					keyAckStateOap = schPublishModel.State;
				}


                chooseAdScheduleModel.Init();
				
				// 데이터 클리어
				chooseAdScheduleDs.Series.Clear();
				chooseAdScheduleDs.Categens.Clear();
				chooseAdScheduleDs.ChannelSets.Clear();   
				chooseAdScheduleDs.ChooseAdSchedule.Clear();  
				ResetDetail();

                // 데이터모델에 전송할 내용을 셋트한다.				
				chooseAdScheduleModel.SearchMediaCode = "1";

                // 장르메뉴 조회 서비스를 호출한다.
                new ChooseAdScheduleManager(systemModel,commonModel).GetMenuList(chooseAdScheduleModel);

                if (chooseAdScheduleModel.ResultCD.Equals("0000"))
                {
                    Utility.SetDataTable(chooseAdScheduleDs.Categens, chooseAdScheduleModel.ChooseAdScheduleDataSet);
                    StatusMessage(chooseAdScheduleModel.ResultCnt + "건의 메뉴 정보가 조회되었습니다.");
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
		/// 장르선택변경시 처리, 채널리스트및 장르편성분 조회
		/// </summary>
		private void SetDetailTextMenu()
		{
			int curRow = cmMenu.Position;
			if(curRow >= 0 )
			{
				IsNotLoading = false;	// 조회중 다시 조회되는 것을 방지함.
				try
				{
					keyScheduleType	= TYPE_Schedule.Genre;
					uiPanelList.Text = ""
						+ "[메뉴편성] " 
						+ dtMenu.Rows[curRow]["MediaName"].ToString().Trim() + "||" 
						+ dtMenu.Rows[curRow]["CategoryName"].ToString().Trim() + "||" 
						+ dtMenu.Rows[curRow]["GenreName"].ToString().Trim();		
 
					keyMediaCode    = dtMenu.Rows[curRow]["MediaCode"].ToString();
					keyCategoryCode = dtMenu.Rows[curRow]["CategoryCode"].ToString();
					keyGenreCode    = dtMenu.Rows[curRow]["GenreCode"].ToString();
				
					// 해당장르의 채널및 회차 목록을 읽어온다.
					chooseAdScheduleDs.Series.Clear();
					chooseAdScheduleDs.ChannelSets.Clear();   
					SearchChannelList();

					// 해당장르의 편성목록을 읽어온다.
					chooseAdScheduleDs.ChooseAdSchedule.Clear();
                    SearcheMenuSchedul();
				}
				finally
				{
					IsNotLoading = true;
				}
			}

			StatusMessage("준비");
		}

		/// <summary>
		/// Channel/Series변경시 처리하는 함수
		/// </summary>
		private void SetDetailTextChannel()
		{
			int		curRow		= cmChannel.Position;
			string	keyTable	= grdExChooseAdScheduleList.SelectedItems[0].Table.Key.ToString();
			string	title		= "";

			if( curRow >= 0 )
			{
				if( keyTable.Equals("ChannelSetsSeries") )
				{
					keyScheduleType	= TYPE_Schedule.Series;
					IsNotLoading = false;

					try
					{
						keyCategoryCode = grdExChooseAdScheduleList.SelectedItems[0].GetRow().Cells["CategoryCode"].Value.ToString();
						keyGenreCode	= grdExChooseAdScheduleList.SelectedItems[0].GetRow().Cells["GenreCode"].Value.ToString();
						keyChannelNo	= grdExChooseAdScheduleList.SelectedItems[0].GetRow().Cells["ChannelNo"].Value.ToString();
						keySeriesNo		= grdExChooseAdScheduleList.SelectedItems[0].GetRow().Cells["SeriesNo"].Value.ToString();
						title			= grdExChooseAdScheduleList.SelectedItems[0].GetRow().Cells["SubTitle"].Value.ToString();

						uiPanelList.Text = string.Format("[회차편성] {0}||{1}||{2}-{3}||{4}",keyCategoryCode, keyGenreCode,keyChannelNo,keySeriesNo,title);
						chooseAdScheduleDs.ChooseAdSchedule.Clear();        
						SearchAdScheduleSeries();
					}
					finally
					{
						IsNotLoading = true;
					}
				}
				else
				{
					keyScheduleType	= TYPE_Schedule.Channel;
					IsNotLoading = false;
					try
					{
						keyMediaCode    = dtChannel.Rows[curRow]["MediaCode"].ToString();
						keyCategoryCode = dtChannel.Rows[curRow]["CategoryCode"].ToString();
						keyGenreCode    = dtChannel.Rows[curRow]["GenreCode"].ToString();
						keyChannelNo    = dtChannel.Rows[curRow]["ChannelNo"].ToString();
						keySeriesNo		= "0";
						title			= grdExChooseAdScheduleList.SelectedItems[0].GetRow().Cells["Title"].Value.ToString();

						uiPanelList.Text = string.Format("[채널편성] {0}||{1}||{2}-{3}||{4}",keyCategoryCode, keyGenreCode,keyChannelNo,keySeriesNo,title);

						chooseAdScheduleDs.ChooseAdSchedule.Clear();        
						SearchChannelSchedule();
					}
					finally
					{
						IsNotLoading = true;
					}
				}
			}
		}

        /// <summary>
        /// 편성광고 선택변경시
        /// </summary>
		private void SetDetailTextSchedule()
		{
			ResetDetail();

			int curRow = cmSchedule.Position;

			if(curRow >= 0 )
			{
				uiPanelDetail.Text = ""
					+ "편성광고 : " 
					+ dtSchedule.Rows[curRow]["CmName"].ToString().Trim() + "편성 / " 
					+ dtSchedule.Rows[curRow]["AdTypeName"].ToString().Trim() + " / [" 
					+ dtSchedule.Rows[curRow]["ItemNo"].ToString() + "]"
					+ dtSchedule.Rows[curRow]["ItemName"].ToString().Trim()
					;

				keyItemNo        = dtSchedule.Rows[curRow]["ItemNo"].ToString();
				keyItemName      = dtSchedule.Rows[curRow]["ItemName"].ToString();
				keyScheduleOrder = dtSchedule.Rows[curRow]["ScheduleOrder"].ToString(); 
				
				string AdType	=  dtSchedule.Rows[curRow]["AdType"].ToString();
				TYPE_Schedule schType =  (TYPE_Schedule)Convert.ToInt32(dtSchedule.Rows[curRow]["CmType"].ToString());

				if(canDelete)
				{	
					if( keyScheduleType == schType)
					{
                        if (AdType.Equals("10") || AdType.Equals("16") || AdType.Equals("19") )
						{
							if( keyAckStateCm.Equals("") || keyAckStateCm.Equals("10") || keyAckStateCm.Equals("30") )	btnDelete.Enabled = true;
						}
						else
						{
							if( keyAckStateOap.Equals("") || keyAckStateOap.Equals("10") || keyAckStateOap.Equals("30") )	btnDelete.Enabled = true;
						}
					}
				}

				if(canUpdate)
				{
					// 광고종류가 OAP인 경우만 순서변경이 가능하다.
					if(AdType.Equals("20"))
					{
						if( keyAckStateOap.Equals("") || keyAckStateOap.Equals("10") || keyAckStateOap.Equals("30") )
						{
							btnOrderUp.Enabled			= true;
							btnOrderDown.Enabled		= true;
							btnOrderFirst.Enabled		= true;
							btnOrderLast.Enabled		= true;
						}
					}
				}
			}
		}
    
        /// <summary>
        /// 해당 장르의 채널/회차 목록을 가져온다
        /// </summary>
        private void SearchChannelList()
        {
            StatusMessage("채널/회차 목록을 조회합니다.");
            try
            {				
                chooseAdScheduleModel.Init();
                chooseAdScheduleModel.MediaCode    = keyMediaCode;
                chooseAdScheduleModel.CategoryCode = keyCategoryCode;
                chooseAdScheduleModel.GenreCode    = keyGenreCode;

                /*
				// 채널목록조회 서비스를 호출한다.
                new ChooseAdScheduleManager(systemModel,commonModel).GetChannelList(chooseAdScheduleModel);

                if (chooseAdScheduleModel.ResultCD.Equals("0000"))
                {
                    Utility.SetDataTableFast(chooseAdScheduleDs.ChannelSets, chooseAdScheduleModel.ChooseAdScheduleDataSet);				
                    StatusMessage(chooseAdScheduleModel.ResultCnt + "건의 채널 정보가 조회되었습니다.");
                }

				chooseAdScheduleModel.Init();
				chooseAdScheduleModel.MediaCode    = keyMediaCode;
				chooseAdScheduleModel.CategoryCode = keyCategoryCode;
				chooseAdScheduleModel.GenreCode    = keyGenreCode;

				// 채널목록조회 서비스를 호출한다.
				new ChooseAdScheduleManager(systemModel,commonModel).GetChannelSerList(chooseAdScheduleModel);

				if (chooseAdScheduleModel.ResultCD.Equals("0000"))
				{
					Utility.SetDataTableFast(chooseAdScheduleDs.Series, chooseAdScheduleModel.ChooseAdScheduleDataSet);				
					StatusMessage(chooseAdScheduleModel.ResultCnt + "건의 회차 정보가 조회되었습니다.");
				}
                */
                
                new ChooseAdScheduleManager(systemModel, commonModel).GetChannelList(chooseAdScheduleModel);

                if (chooseAdScheduleModel.ResultCD.Equals("0000"))
                {
                    //Utility.SetDataTableFast(chooseAdScheduleDs.ChannelSets, chooseAdScheduleModel.ChooseAdScheduleDataSet);
                    gridEX1.DataSource = chooseAdScheduleModel.ChooseAdScheduleDataSet.Tables[0];

                    StatusMessage(chooseAdScheduleModel.ResultCnt + "건의 채널 정보가 조회되었습니다.");
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
		/// 장르 편성현황 조회하기
		/// </summary>
		private void SearcheMenuSchedul()
		{
			StatusMessage("메뉴 편성현황을 조회합니다.");

			try
			{
				grdExScheduleList.UnCheckAllRecords();

				// 데이터모델 초기화
				chooseAdScheduleModel.Init();

				// 데이터모델에 전송할 내용을 셋트한다.
     
				chooseAdScheduleModel.MediaCode		=  keyMediaCode;   
				chooseAdScheduleModel.GenreCode     =  keyGenreCode;

                if(chkAdState_20.Checked) chooseAdScheduleModel.SearchchkAdState_20 = "Y";
                if(chkAdState_30.Checked) chooseAdScheduleModel.SearchchkAdState_30 = "Y";
                if(chkAdState_40.Checked) chooseAdScheduleModel.SearchchkAdState_40 = "Y";

				// 광고파일목록조회 서비스를 호출한다.
				new ChooseAdScheduleManager(systemModel,commonModel).GetChooseAdScheduleListMenu(chooseAdScheduleModel);

				if (chooseAdScheduleModel.ResultCD.Equals("0000"))
				{
					Utility.SetDataTableFast(chooseAdScheduleDs.ChooseAdSchedule, chooseAdScheduleModel.ChooseAdScheduleDataSet);		
					StatusMessage(chooseAdScheduleModel.ResultCnt + "건의 광고파일 정보가 조회되었습니다.");

					keyLastOrder = chooseAdScheduleModel.LastOrder;
					SetCurrentRowSchedule();

					if(canCreate)
					{
						if( keyAckStateCm.Equals("") || keyAckStateCm.Equals("10") || keyAckStateCm.Equals("30") )
						{
							btnAdd_10.Enabled = true;
							btnAdd_16.Enabled = true;
						
							if( keyAckStateOap.Equals("") || keyAckStateOap.Equals("10") || keyAckStateOap.Equals("30") )
							{
								btnAdd_11.Enabled = true;
								btnAdd_12.Enabled = true;
								btnAdd_20.Enabled = true;
							}
							else
							{
								btnAdd_11.Enabled = false;
								btnAdd_12.Enabled = false;
								btnAdd_20.Enabled = false;
							}
						}
						else if( keyAckStateOap.Equals("") || keyAckStateOap.Equals("10") || keyAckStateOap.Equals("30") )
						{
							btnAdd_10.Enabled = false;
							btnAdd_16.Enabled = false;

							btnAdd_11.Enabled = true;
							btnAdd_12.Enabled = true;
							btnAdd_20.Enabled = true;
						}
						else
						{
							btnAdd_10.Enabled = false;
							btnAdd_16.Enabled = false;

							btnAdd_11.Enabled = false;
							btnAdd_12.Enabled = false;
							btnAdd_20.Enabled = false;
						}
					}
				}

				SetDetailTextSchedule();

			}
			catch(FrameException fe)
			{
				FrameSystem.showMsgForm("메뉴 편성현황 조회오류", new string[] {fe.ErrCode, fe.ResultMsg});
			}
			catch(Exception ex)
			{
				FrameSystem.showMsgForm("메뉴 편성현황 조회오류",new string[] {"",ex.Message});
			}
		}
        
		/// <summary>
		/// 채널별 광고편성현황 조회
		/// </summary>
		private void SearchChannelSchedule()
		{
			StatusMessage("채널 편성현황을 조회합니다.");

			try
			{
				grdExScheduleList.UnCheckAllRecords();

				// 데이터모델 초기화
				chooseAdScheduleModel.Init();

				// 데이터모델에 전송할 내용을 셋트한다.
     
				chooseAdScheduleModel.MediaCode		=  keyMediaCode;   
				chooseAdScheduleModel.GenreCode     =  keyGenreCode;
				chooseAdScheduleModel.ChannelNo     =  keyChannelNo;

                if(chkAdState_20.Checked) chooseAdScheduleModel.SearchchkAdState_20 = "Y";
                if(chkAdState_30.Checked) chooseAdScheduleModel.SearchchkAdState_30 = "Y";
                if(chkAdState_40.Checked) chooseAdScheduleModel.SearchchkAdState_40 = "Y";

				// 광고파일목록조회 서비스를 호출한다.
				new ChooseAdScheduleManager(systemModel,commonModel).GetChooseAdScheduleListChannel(chooseAdScheduleModel);
				
				if (chooseAdScheduleModel.ResultCD.Equals("0000"))
				{
					Utility.SetDataTable(chooseAdScheduleDs.ChooseAdSchedule, chooseAdScheduleModel.ChooseAdScheduleDataSet);		
					StatusMessage(chooseAdScheduleModel.ResultCnt + "건의 광고파일 정보가 조회되었습니다.");

					keyLastOrder = chooseAdScheduleModel.LastOrder;
					SetCurrentRowSchedule();
				}

				SetDetailTextSchedule();

			}
			catch(FrameException fe)
			{
				FrameSystem.showMsgForm("채널 편성현황 조회오류", new string[] {fe.ErrCode, fe.ResultMsg});
			}
			catch(Exception ex)
			{
				FrameSystem.showMsgForm("채널 편성현황 조회오류",new string[] {"",ex.Message});
			}

		}


		/// <summary>
		/// 채널/회차별 광고편성현황을 조회
		/// </summary>
		private void SearchAdScheduleSeries()
		{
			StatusMessage("채널회차 편성현황을 조회합니다.");

			try
			{
				grdExScheduleList.UnCheckAllRecords();

				// 데이터모델 초기화
				chooseAdScheduleModel.Init();

				// 데이터모델에 전송할 내용을 셋트한다.
     
				chooseAdScheduleModel.MediaCode		=  keyMediaCode;   
				chooseAdScheduleModel.GenreCode     =  keyGenreCode;
				chooseAdScheduleModel.ChannelNo     =  keyChannelNo;
				chooseAdScheduleModel.SeriesNo		=  keySeriesNo;

				if(chkAdState_20.Checked) chooseAdScheduleModel.SearchchkAdState_20 = "Y";
				if(chkAdState_30.Checked) chooseAdScheduleModel.SearchchkAdState_30 = "Y";
				if(chkAdState_40.Checked) chooseAdScheduleModel.SearchchkAdState_40 = "Y";

				// 광고파일목록조회 서비스를 호출한다.
				new ChooseAdScheduleManager(systemModel,commonModel).GetChooseAdScheduleListSeries(chooseAdScheduleModel);
				
				if (chooseAdScheduleModel.ResultCD.Equals("0000"))
				{
					Utility.SetDataTable(chooseAdScheduleDs.ChooseAdSchedule, chooseAdScheduleModel.ChooseAdScheduleDataSet);		
					StatusMessage(chooseAdScheduleModel.ResultCnt + "건의 광고파일 정보가 조회되었습니다.");

					keyLastOrder = chooseAdScheduleModel.LastOrder;
					SetCurrentRowSchedule();
				}

				SetDetailTextSchedule();

			}
			catch(FrameException fe)
			{
				FrameSystem.showMsgForm("채널회차 편성현황 조회오류", new string[] {fe.ErrCode, fe.ResultMsg});
			}
			catch(Exception ex)
			{
				FrameSystem.showMsgForm("채널회차 편성현황 조회오류",new string[] {"",ex.Message});
			}

		}


		/// <summary>
		/// 선택된 장르를 지정메뉴광고 상세내역을 삭제
		/// </summary>
		private void DeleteChoiceAdMenuDetail()
		{
			StatusMessage("지정메뉴광고 편성내역을 삭제합니다.");


			DialogResult result = MessageBox.Show("해당 지정메뉴광고 편성내역을 삭제 하시겠습니까?","지정메뉴광고 편성내역 삭제",
				MessageBoxButtons.YesNo, MessageBoxIcon.Question,
				MessageBoxDefaultButton.Button2);

			if (result == DialogResult.No) return;

			// 그리드에 변경된 데이터를 Datasource에 업데이트 한다.
			grdExScheduleList.UpdateData();

			try
			{
				int SetCount = 0;

				// 삭제 시킴
				for(int i = 0;i < chooseAdScheduleDs.ChooseAdSchedule.Rows.Count;i++)
				{
					DataRow row = chooseAdScheduleDs.ChooseAdSchedule.Rows[i];

					Debug.WriteLine( i.ToString() + ":" + row["CheckYn"].ToString() + "|" + row["ItemName"].ToString() );

					if(row["CheckYn"].ToString().Equals("True"))
					{
						schChoiceAdModel.Init();

						schChoiceAdModel.ItemNo    =  row["ItemNo"].ToString();
						schChoiceAdModel.MediaCode =  row["MediaCode"].ToString();
						schChoiceAdModel.GenreCode =  row["GenreCode"].ToString();

						new SchChoiceAdManager(systemModel,commonModel).SetSchChoiceMenuDetailDelete(schChoiceAdModel);

						if (schChoiceAdModel.ResultCD.Equals("0000"))
						{
							SetCount++;
						}	
					}
				}

				if(SetCount > 0)
				{
					ReloadList();
					StatusMessage("지정메뉴광고 편성상세내역이 삭제되었습니다.");			
				}	
				else
				{
					MessageBox.Show("선택된 지정메뉴광고 편성내역이 없습니다.", "메뉴광고편성",
						MessageBoxButtons.OK, MessageBoxIcon.Information);
				}	
			}
			catch(FrameException fe)
			{
				FrameSystem.showMsgForm("지정메뉴광고 상세편성 삭제오류", new string[] {fe.ErrCode, fe.ResultMsg});
			}
			catch(Exception ex)
			{
				FrameSystem.showMsgForm("지정메뉴광고 상세편성 삭제오류",new string[] {"",ex.Message});
			}			
		}


		/// <summary>
		/// 선택된 지정채널광고 상세내역을 삭제
		/// </summary>
		private void DeleteChoiceAdChannelDetail()
		{
			StatusMessage("지정채널광고 편성상세내역을 삭제합니다.");

			DialogResult result = MessageBox.Show("해당 지정채널광고 편성내역을 삭제 하시겠습니까?","지정채널광고 편성내역 삭제",
				MessageBoxButtons.YesNo, MessageBoxIcon.Question,
				MessageBoxDefaultButton.Button2);

			if (result == DialogResult.No) return;

			// 그리드에 변경된 데이터를 Datasource에 업데이트 한다.
			grdExScheduleList.UpdateData();

			try
			{
				int SetCount = 0;

				// 삭제 시킴
				for(int i = 0;i < chooseAdScheduleDs.ChooseAdSchedule.Rows.Count;i++)
				{
					DataRow row = chooseAdScheduleDs.ChooseAdSchedule.Rows[i];

					Debug.WriteLine( i.ToString() + ":" + row["CheckYn"].ToString() + "|" + row["ItemName"].ToString() );

					if(row["CheckYn"].ToString().Equals("True"))
					{

						schChoiceAdModel.Init();

						schChoiceAdModel.ItemNo    = row["ItemNo"].ToString();
						schChoiceAdModel.MediaCode = row["MediaCode"].ToString();
						schChoiceAdModel.ChannelNo = row["ChannelNo"].ToString();

						new SchChoiceAdManager(systemModel,commonModel).SetSchChoiceChannelDetailDelete(schChoiceAdModel);

						if (schChoiceAdModel.ResultCD.Equals("0000"))
						{
							SetCount++;
						}	
					}
				}

				if(SetCount > 0)
				{
					ReloadList();
					StatusMessage("지정채널광고 편성상세내역이 삭제되었습니다.");			
				}	
				else
				{
					MessageBox.Show("선택된 지정채널광고 편성내역이 없습니다.", "채널광고편성",
						MessageBoxButtons.OK, MessageBoxIcon.Information);
				}	
			}
			catch(FrameException fe)
			{
				FrameSystem.showMsgForm("지정채널광고 상세편성 삭제오류", new string[] {fe.ErrCode, fe.ResultMsg});
			}
			catch(Exception ex)
			{
				FrameSystem.showMsgForm("지정채널광고 상세편성 삭제오류",new string[] {"",ex.Message});
			}			
		}
        
        private void ResetDetail()
        {	
			uiPanelDetail.Text = "편성광고";	

			btnDelete.Enabled			= false;
			btnOrderUp.Enabled			= false;
			btnOrderDown.Enabled		= false;
			btnOrderFirst.Enabled		= false;
			btnOrderLast.Enabled		= false;
        }



		/// <summary>
		/// 지정메뉴편성내역 순위변경
		/// </summary>
		private void OrderSetScheduleMenuAd(int OrderSet)
		{
			StatusMessage("지정메뉴광고 편성내역의 편성순서를 변경합니다.");

			if(keyItemNo.Length == 0) 
			{
				MessageBox.Show("변경할 지정메뉴광고 편성내역이 선택되지 않았습니다.","지정메뉴광고 편성내역 순서변경", 
					MessageBoxButtons.OK, MessageBoxIcon.Information );
				return;
			}

			try
			{
				chooseAdScheduleModel.Init();

				// 데이터모델에 전송할 내용을 셋트한다.
				chooseAdScheduleModel.MediaCode     = keyMediaCode;
				chooseAdScheduleModel.GenreCode     = keyGenreCode;
				chooseAdScheduleModel.ItemNo        = keyItemNo;
				chooseAdScheduleModel.ScheduleOrder = keyScheduleOrder;
				chooseAdScheduleModel.ItemName      = keyItemName;

				int NowOrder  = Convert.ToInt32(keyScheduleOrder);
				int LastOrder = Convert.ToInt32(keyLastOrder); 

				if(NowOrder == 0)
				{
					MessageBox.Show("해당 지정메뉴광고의 순서는 변경될 수 없습니다.","지정메뉴광고 편성내역 순서변경", 
						MessageBoxButtons.OK, MessageBoxIcon.Information );							
					return;
				}

				switch(OrderSet)
				{
					case ORDER_FIRST:
						if(NowOrder == 1) 
						{
							MessageBox.Show("해당 지정메뉴광고 편성내역이 첫번째 순서입니다.","지정메뉴광고 편성내역 순서변경", 
								MessageBoxButtons.OK, MessageBoxIcon.Information );							
							return;
						}
						break;
					case ORDER_UP:
						if(NowOrder == 1) 
						{
							MessageBox.Show("해당 지정메뉴광고 편성내역이 첫번째 순서입니다.","지정메뉴광고 편성내역 순서변경", 
								MessageBoxButtons.OK, MessageBoxIcon.Information );							
							return;
						}
						break;
					case ORDER_DOWN:
						if(NowOrder == LastOrder) 
						{
							MessageBox.Show("해당 지정메뉴광고 편성내역이 마지막 순서입니다.","지정메뉴광고 편성내역 순서변경", 
								MessageBoxButtons.OK, MessageBoxIcon.Information );							
							return;
						}
						break;
					case ORDER_LAST:
						if(NowOrder == LastOrder) 
						{
							MessageBox.Show("해당 지정메뉴광고 편성내역이 마지막 순서입니다.","지정메뉴광고 편성내역 순서변경", 
								MessageBoxButtons.OK, MessageBoxIcon.Information );							
							return;
						}
						break;
				}

				// 지정메뉴광고 편성내역 순위 옮기는 서비스를 호출한다.
				new ChooseAdScheduleManager(systemModel,commonModel).SetSchMenuAdOrderSet(chooseAdScheduleModel, OrderSet);
				keyScheduleOrder = chooseAdScheduleModel.ScheduleOrder;
				
				if (chooseAdScheduleModel.ResultCD.Equals("0000"))
				{
					ReloadList();
					StatusMessage("지정메뉴광고 편성내역의 순위가 변경되었습니다.");			
				}		
		
			}
			catch(FrameException fe)
			{
				FrameSystem.showMsgForm("지정메뉴광고 편성내역 순서변경 오류", new string[] {fe.ErrCode, fe.ResultMsg});
			}
			catch(Exception ex)
			{
				FrameSystem.showMsgForm("지정메뉴광고 편성내역 순서변경 오류",new string[] {"",ex.Message});
			}		

		}
		
		/// <summary>
		/// 지정채널편성내역 순서변경
		/// </summary>
		private void OrderSetScheduleChannelAd(int OrderSet)
		{
			StatusMessage("지정채널광고 편성내역의 편성순위를 변경합니다.");

			if(keyItemNo.Length == 0) 
			{
				MessageBox.Show("변경할 지정채널광고 편성내역이 선택되지 않았습니다.","지정채널광고 편성내역 순위변경", 
					MessageBoxButtons.OK, MessageBoxIcon.Information );
				return;
			}

			try
			{
				chooseAdScheduleModel.Init();

				// 데이터모델에 전송할 내용을 셋트한다.
				chooseAdScheduleModel.MediaCode     = keyMediaCode;
				chooseAdScheduleModel.ChannelNo     = keyChannelNo;
				chooseAdScheduleModel.ItemNo        = keyItemNo;
				chooseAdScheduleModel.ScheduleOrder = keyScheduleOrder;
				chooseAdScheduleModel.ItemName      = keyItemName;

				int NowOrder  = Convert.ToInt32(keyScheduleOrder);
				int LastOrder = Convert.ToInt32(keyLastOrder); 

				if(NowOrder == 0)
				{
					MessageBox.Show("해당 지정채널광고의 순서는 변경될 수 없습니다.","지정채널광고 편성내역 순서변경", 
						MessageBoxButtons.OK, MessageBoxIcon.Information );							
					return;
				}

				switch(OrderSet)
				{
					case ORDER_FIRST:
						if(NowOrder <= 1) 
						{
							MessageBox.Show("해당 지정채널광고 편성내역이 첫번째 순서입니다.","지정채널광고 편성내역 순서변경", 
								MessageBoxButtons.OK, MessageBoxIcon.Information );							
							return;
						}
						break;
					case ORDER_UP:
						if(NowOrder <= 1) 
						{
							MessageBox.Show("해당 지정채널광고 편성내역이 첫번째 순서입니다.","지정채널광고 편성내역 순서변경", 
								MessageBoxButtons.OK, MessageBoxIcon.Information );							
							return;
						}
						break;
					case ORDER_DOWN:
						if(NowOrder >= LastOrder) 
						{
							MessageBox.Show("해당 지정채널광고 편성내역이 마지막 순서입니다.","지정채널광고 편성내역 순서변경", 
								MessageBoxButtons.OK, MessageBoxIcon.Information );							
							return;
						}
						break;
					case ORDER_LAST:
						if(NowOrder >= LastOrder) 
						{
							MessageBox.Show("해당 지정채널광고 편성내역이 마지막 순서입니다.","지정채널광고 편성내역 순서변경", 
								MessageBoxButtons.OK, MessageBoxIcon.Information );							
							return;
						}
						break;
				}

				// 지정채널광고 편성내역 순위 옮기는 서비스를 호출한다.
				new ChooseAdScheduleManager(systemModel,commonModel).SetSchChannelAdOrderSet(chooseAdScheduleModel, OrderSet);
				keyScheduleOrder = chooseAdScheduleModel.ScheduleOrder;
				
				if (chooseAdScheduleModel.ResultCD.Equals("0000"))
				{
					ReloadSchedule();
					StatusMessage("지정채널광고 편성내역의 순서가 변경되었습니다.");			
				}		
		
			}
			catch(FrameException fe)
			{
				FrameSystem.showMsgForm("지정채널광고 편성내역 순서변경 오류", new string[] {fe.ErrCode, fe.ResultMsg});
			}
			catch(Exception ex)
			{
				FrameSystem.showMsgForm("지정채널광고 편성내역 순서변경 오류",new string[] {"",ex.Message});
			}		

		}

		/// <summary>
		/// 키캆을찾아 그리드 키에 해당되는로우로..
		/// </summary>
		private void SetCurrentRowSchedule()
		{

			int rowIndex = 0;
			if ( dtSchedule.Rows.Count < 1 ) return;              
			foreach (DataRow row in dtSchedule.Rows)
			{					
				if(row["ScheduleOrder"].ToString().Equals(keyScheduleOrder))
				{					
					cmSchedule.Position = rowIndex;
					break;								
				}
				rowIndex++;
			}
			grdExScheduleList.EnsureVisible();
		}


		private void ReloadSchedule()
		{
			switch(keyScheduleType)
			{
				case TYPE_Schedule.Genre:
					SearcheMenuSchedul();
					break;
				case TYPE_Schedule.Channel:
					SearchChannelSchedule();
					break;
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
        #endregion

		#region 팝업창을 위한 메소드
			
		public void ReloadList()
		{
			switch(keyScheduleType)
			{
				case TYPE_Schedule.Genre:
					SetDetailTextMenu();
					break;
				case TYPE_Schedule.Channel:
					SetDetailTextChannel();
					break;
			}
		}

		#endregion

        /// <summary>
        /// 편성추가할 광고 팝업윈도우를 오픈하는 함수
        /// 각 추가버튼에서 클릭에서 호출한다
        /// </summary>
        /// <param name="adType"></param>
        ItemMultiChoiceForm pForm = null;
        private void InsertOpenPopupWin(int adType)
        {
            //ChooseAdSearch_pForm pForm = new ChooseAdSearch_pForm(this);
            pForm = new ItemMultiChoiceForm(this);

            switch(keyScheduleType)
            {
                case TYPE_Schedule.Genre:
                    pForm.keySchType = adType.ToString();
                    /*
                    pForm.ScheduleType = TYPE_Schedule.Genre;
                    pForm.keyMediaCode = keyMediaCode;
                    pForm.keyGenreCode = keyGenreCode;
					pForm.keyChannelNo = "0";
					pForm.keySeriesNo	= "0";
                    pForm.AdType        = adType;
                    */
                    break;
                case TYPE_Schedule.Channel:
                    pForm.keySchType = adType.ToString();
                    /*
                    pForm.ScheduleType	= TYPE_Schedule.Channel;
                    pForm.keyMediaCode	= keyMediaCode;
                    pForm.keyChannelNo	= keyChannelNo;
					pForm.keySeriesNo	= "0";
                    pForm.AdType        = adType;
                     * */
                    break;
				case TYPE_Schedule.Series:
                    pForm.keySchType = adType.ToString();
                    /*
					pForm.ScheduleType = TYPE_Schedule.Series;
					pForm.keyMediaCode = keyMediaCode;
					pForm.keyChannelNo = keyChannelNo;
					pForm.keySeriesNo  = keySeriesNo;
					pForm.AdType       = adType;
                     * */
					break;
            }
            pForm.callType = "ChooseAdScheduleControl";
            pForm.ReturnDate += new ItemMultiChoiceForm.PopupService(ItemMultiChoiceForm_Return);
            pForm.ShowDialog();            
            pForm.Dispose();
            pForm = null;
        }

        /// <summary>
        /// SchChoiceAdSearch_pForm 폼에서 광고 선택
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        public void ItemMultiChoiceForm_Return(object sender, EventArgs args)
        {
            ItemEventArgs itemEventArgs = (ItemEventArgs)args;
            ItemMultiChoice_pDs itemMultiChoice_pDs = (ItemMultiChoice_pDs)itemEventArgs.dataSet;
            SchChoiceAdModel schChoiceAdModel = new SchChoiceAdModel();
            try
            {
                int SetCount = 0;

                for (int i = 0; i < itemMultiChoice_pDs.ChoiceAdItems.Rows.Count; i++)
                {
                    DataRow row = itemMultiChoice_pDs.ChoiceAdItems.Rows[i];

                    if (row["CheckYn"].ToString().Equals("True"))
                    {
                        schChoiceAdModel.Init();

                        //switch (_schType)
                        switch (keyScheduleType)
                        {
                            case TYPE_Schedule.Genre:
                                schChoiceAdModel.ItemNo = row["ItemNo"].ToString();
                                schChoiceAdModel.ItemName = row["ItemName"].ToString();
                                schChoiceAdModel.AdType = row["AdType"].ToString();
                                
                                schChoiceAdModel.MediaCode = keyMediaCode;
                                schChoiceAdModel.CategoryCode = keyCategoryCode;
                                schChoiceAdModel.GenreCode = keyGenreCode;
                                schChoiceAdModel.ChannelNo = keyChannelNo;
                                schChoiceAdModel.SeriesNo = keySeriesNo;
                                //schChoiceAdModel.ScheduleType = _schType;
                                schChoiceAdModel.ScheduleType = keyScheduleType;

                                new SchChoiceAdManager(systemModel, commonModel).SetSchChoiceMenuDetailAdd(schChoiceAdModel);

                                keyItemNo = row["ItemNo"].ToString();

                                break;
                            case TYPE_Schedule.Channel:
                                schChoiceAdModel.ItemNo = row["ItemNo"].ToString();
                                schChoiceAdModel.ItemName = row["ItemName"].ToString();
                                schChoiceAdModel.AdType = row["AdType"].ToString();

                                schChoiceAdModel.MediaCode = keyMediaCode;
                                schChoiceAdModel.GenreCode = keyGenreCode;
                                schChoiceAdModel.ChannelNo = keyChannelNo;
                                schChoiceAdModel.SeriesNo = keySeriesNo;
                                //schChoiceAdModel.ScheduleType = _schType;
                                schChoiceAdModel.ScheduleType = keyScheduleType;

                                new SchChoiceAdManager(systemModel, commonModel).SetSchChoiceChannelDetailAdd(schChoiceAdModel);

                                keyItemNo = row["ItemNo"].ToString();

                                break;
                            case TYPE_Schedule.Series:
                                schChoiceAdModel.ItemNo = row["ItemNo"].ToString();
                                schChoiceAdModel.ItemName = row["ItemName"].ToString();
                                schChoiceAdModel.AdType = row["AdType"].ToString();

                                schChoiceAdModel.MediaCode = keyMediaCode;
                                schChoiceAdModel.GenreCode = keyGenreCode;
                                schChoiceAdModel.ChannelNo = keyChannelNo;
                                schChoiceAdModel.SeriesNo = keySeriesNo;
                                //schChoiceAdModel.ScheduleType = _schType;
                                schChoiceAdModel.ScheduleType = keyScheduleType;

                                new SchChoiceAdManager(systemModel, commonModel).SetSchChoiceSeriesDetailAdd(schChoiceAdModel);

                                keyItemNo = row["ItemNo"].ToString();

                                break;
                        }

                        if (schChoiceAdModel.ResultCD.Equals("0000"))
                        {
                            SetCount++;
                        }
                    }
                }

                // 체크된 모든 항목을 클리어
                pForm.ClearListCheck();

                if (SetCount > 0)
                {
                    ReloadList();
                }
                else
                {
                    MessageBox.Show("선택한 광고가 없습니다.", "지정광고편성", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (FrameException fe)
            {
                FrameSystem.showMsgForm("지정광고 편성오류", new string[] { fe.ErrCode, fe.ResultMsg });
            }
            catch (Exception ex)
            {
                FrameSystem.showMsgForm("지정광고 편성오류", new string[] { "", ex.Message });
            }
        }

        /// <summary>
        /// 상업광고 추가팝업을 연다
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAdd_10_Click(object sender, System.EventArgs e)
        {
            this.InsertOpenPopupWin( 10 );
        }

        private void btnAdd_16_Click(object sender, System.EventArgs e)
        {
            this.InsertOpenPopupWin( 16 );
        }

        private void btnAdd_19_Click(object sender, System.EventArgs e)
        {
            this.InsertOpenPopupWin( 19 );
        }

        private void btnAdd_20_Click(object sender, System.EventArgs e)
        {
            this.InsertOpenPopupWin( 20 );
        }

        private void btnAdd_11_Click(object sender, System.EventArgs e)
        {
            this.InsertOpenPopupWin( 11 );        
        }

        private void btnAdd_12_Click(object sender, System.EventArgs e)
        {
            this.InsertOpenPopupWin( 12 );        
        }

        private void grdExScheduleList_CellValueChanged(object sender, Janus.Windows.GridEX.ColumnActionEventArgs e)
        {
            if (e.Column.EditType == Janus.Windows.GridEX.EditType.CheckBox)
            {
                int curRow = cmSchedule.Position;
                if (curRow >= 0)
                {
                    dtSchedule.Rows[curRow].BeginEdit();
                    dtSchedule.Rows[curRow]["CheckYn"] = grdExScheduleList.GetValue(e.Column).ToString();
                    dtSchedule.Rows[curRow].EndEdit();
                }
            }
        }

        private void pnlSearch_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}