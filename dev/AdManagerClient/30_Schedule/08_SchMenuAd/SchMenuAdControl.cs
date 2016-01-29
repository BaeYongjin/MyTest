// ===============================================================================
// CSS광고 편성관리 프로그램
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
    /// 채널/메뉴별 편성현황 컨트롤
    /// </summary>
    public class SchMenuAdControl : System.Windows.Forms.UserControl, IUserControl
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
		SchMenuAdModel			schMenuAdModel			= new SchMenuAdModel();			// 메뉴별 편성모델
        ChooseAdScheduleModel	chooseAdScheduleModel	= new ChooseAdScheduleModel();	// 채널/메뉴 편성현황 모델
		SchChoiceAdModel		schChoiceAdModel		= new SchChoiceAdModel();		// 지정광고편성모델
		SchPublishModel			schPublishModel			= new SchPublishModel();		// 광고승인모델
		SchedulePerItemModel	schPerItemModel			= new SchedulePerItemModel();	//	광고당 편성관리 모델[S1]
		
		// 화면처리용 변수
        CurrencyManager cmMenu        = null;					// 데이터 그리드의 변경에 따른 데이터셋 관리를 위하여			
		CurrencyManager cmSchedule    = null;					
		CurrencyManager cmItem        = null;					
		DataTable       dtMenu        = null;
		DataTable       dtSchedule    = null;
		DataTable       dtItem        = null;

		bool IsNewSearchKey		  = true;					// 검색어입력 여부

		// 사용권한
        bool IsSearching = false; // 조회중 상세화면이 업데이트 되는 것을 방지 하기위함 2011.11.29 JH.Park
		bool canCreate			  = false;
		bool canRead			  = false;
		//bool canUpdate			  = false;
		bool canDelete            = false;

		// Key
		bool IsNotLoading		       = true;					// 상세조회중이 아님
		public string keyMediaCode     = "";
		public string keyCategoryCode  = "";
		public string keyGenreCode     = "";
		public string keyChannelNo     = "";
		public string keyItemNo        = "";
		public string keyScheduleOrder = "";

		//편성하기전 CSS중복을 막기위한 체크변수
		public string schCategoryCode  = "";
		public string schGenreCode     = "";
		public string schCategoryName  = "";
		public string genre_rCategoryName  = "";
		public string schGenreName     = "";
		public string schItemNo        = "";
		public string schItemName      = "";
		public string schAdType        = "";
		public string schContractSeq        = "";
		public string schContractName        = "";
		public string schExcuteStartDay        = "";
		public string schCategoryCode_1		="";
		//string keyCmType     = "";
		string keyItemName     = "";
		//string keyLastOrder    = "";
		
		// 메뉴/채널 구분
		const int AD_MENU     = 1;
		const int AD_CHANNEL  = 2;

		// 순위변경구분
		const int ORDER_FIRST = 1;
		const int ORDER_LAST  = 2;
		const int ORDER_UP    = 3;
		const int ORDER_DOWN  = 4;

		// 편성배포 승인상태 처리용
		private int keyAckNo    = 1;
		private string keyAckState = "";

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
        private Janus.Windows.UI.Dock.UIPanelGroup uiPanelChooseAdSchedule;
		private Janus.Windows.UI.Dock.UIPanel uiPanelSearch;
		private Janus.Windows.UI.Dock.UIPanelInnerContainer uiPanelSearchContainer;
		private System.Windows.Forms.Panel pnlDetail;
		private Janus.Windows.EditControls.UIButton btnDelete;
		private System.Windows.Forms.Label lbMsg;
		private System.Data.DataView dvSchedule;
		private Janus.Windows.UI.Dock.UIPanel uiPanelMenuList;
		private Janus.Windows.UI.Dock.UIPanelInnerContainer uiPanelMenuListContainer;
		private System.Data.DataView dvItem;
		private Janus.Windows.UI.Dock.UIPanelGroup uiPanelGroup1;
		private Janus.Windows.GridEX.GridEX grdExItemList;
		private Janus.Windows.UI.Dock.UIPanelGroup uiPanel0;
		private Janus.Windows.EditControls.UIButton btnAddSchedule;
		private Janus.Windows.EditControls.UICheckBox chkAdState_10;
		private Janus.Windows.EditControls.UICheckBox chkAdState_40;
		private Janus.Windows.EditControls.UICheckBox chkAdState_30;
		private Janus.Windows.EditControls.UICheckBox chkAdState_20;
		private System.Windows.Forms.Label label3;
		private Janus.Windows.UI.Dock.UIPanelGroup uiPanelGroup3;
		private AdManagerClient.SchMenuAdDs schMenuAdDs;
		private Janus.Windows.GridEX.GridEX grdExSchList;
		private System.Windows.Forms.ImageList ilSchType;
		private Janus.Windows.UI.Dock.UIPanel uiPanelItemSch;
		private Janus.Windows.UI.Dock.UIPanelInnerContainer uiPanelItemSchContainer;
		private Janus.Windows.UI.Dock.UIPanel uiPanelCommand;
		private Janus.Windows.UI.Dock.UIPanelInnerContainer uiPanelCommandContainer;		
        private System.ComponentModel.IContainer components;

        public SchMenuAdControl()
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
			Janus.Windows.GridEX.GridEXLayout grdExItemList_Layout_0 = new Janus.Windows.GridEX.GridEXLayout();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SchMenuAdControl));
			Janus.Windows.GridEX.GridEXLayout grdExSchList_DesignTimeLayout = new Janus.Windows.GridEX.GridEXLayout();
			Janus.Windows.Common.Layouts.JanusLayoutReference grdExSchList_DesignTimeLayout_Reference_0 = new Janus.Windows.Common.Layouts.JanusLayoutReference("GridEXLayoutData.RootTable.Columns.Column2.ValueList.Item0.Image");
			Janus.Windows.Common.Layouts.JanusLayoutReference grdExSchList_DesignTimeLayout_Reference_1 = new Janus.Windows.Common.Layouts.JanusLayoutReference("GridEXLayoutData.RootTable.Columns.Column2.ValueList.Item1.Image");
			Janus.Windows.Common.Layouts.JanusLayoutReference grdExSchList_DesignTimeLayout_Reference_2 = new Janus.Windows.Common.Layouts.JanusLayoutReference("GridEXLayoutData.RootTable.Columns.Column2.ValueList.Item2.Image");
			Janus.Windows.Common.Layouts.JanusLayoutReference grdExSchList_DesignTimeLayout_Reference_3 = new Janus.Windows.Common.Layouts.JanusLayoutReference("GridEXLayoutData.RootTable.FormatConditions.Condition0.FormatStyle.BackgroundImag" +
        "e");
			this.dvItem = new System.Data.DataView();
			this.schMenuAdDs = new AdManagerClient.SchMenuAdDs();
			this.uiPM = new Janus.Windows.UI.Dock.UIPanelManager(this.components);
			this.uiPanelUsers = new Janus.Windows.UI.Dock.UIPanelGroup();
			this.uiPanelSearch = new Janus.Windows.UI.Dock.UIPanel();
			this.uiPanelSearchContainer = new Janus.Windows.UI.Dock.UIPanelInnerContainer();
			this.pnlSearch = new System.Windows.Forms.Panel();
			this.chkAdState_10 = new Janus.Windows.EditControls.UICheckBox();
			this.chkAdState_40 = new Janus.Windows.EditControls.UICheckBox();
			this.chkAdState_30 = new Janus.Windows.EditControls.UICheckBox();
			this.chkAdState_20 = new Janus.Windows.EditControls.UICheckBox();
			this.label3 = new System.Windows.Forms.Label();
			this.pnlSearchBtn = new System.Windows.Forms.Panel();
			this.btnSearch = new Janus.Windows.EditControls.UIButton();
			this.uiPanelChooseAdSchedule = new Janus.Windows.UI.Dock.UIPanelGroup();
			this.uiPanel0 = new Janus.Windows.UI.Dock.UIPanelGroup();
			this.uiPanelGroup3 = new Janus.Windows.UI.Dock.UIPanelGroup();
			this.uiPanelGroup1 = new Janus.Windows.UI.Dock.UIPanelGroup();
			this.uiPanelMenuList = new Janus.Windows.UI.Dock.UIPanel();
			this.uiPanelMenuListContainer = new Janus.Windows.UI.Dock.UIPanelInnerContainer();
			this.grdExItemList = new Janus.Windows.GridEX.GridEX();
			this.uiPanelItemSch = new Janus.Windows.UI.Dock.UIPanel();
			this.uiPanelItemSchContainer = new Janus.Windows.UI.Dock.UIPanelInnerContainer();
			this.grdExSchList = new Janus.Windows.GridEX.GridEX();
			this.dvSchedule = new System.Data.DataView();
			this.ilSchType = new System.Windows.Forms.ImageList(this.components);
			this.uiPanelCommand = new Janus.Windows.UI.Dock.UIPanel();
			this.uiPanelCommandContainer = new Janus.Windows.UI.Dock.UIPanelInnerContainer();
			this.pnlDetail = new System.Windows.Forms.Panel();
			this.lbMsg = new System.Windows.Forms.Label();
			this.btnDelete = new Janus.Windows.EditControls.UIButton();
			this.btnAddSchedule = new Janus.Windows.EditControls.UIButton();
			this.uiButton1 = new Janus.Windows.EditControls.UIButton();
			this.uiButton2 = new Janus.Windows.EditControls.UIButton();
			((System.ComponentModel.ISupportInitialize)(this.dvItem)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.schMenuAdDs)).BeginInit();
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
			((System.ComponentModel.ISupportInitialize)(this.uiPanel0)).BeginInit();
			this.uiPanel0.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.uiPanelGroup3)).BeginInit();
			this.uiPanelGroup3.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.uiPanelGroup1)).BeginInit();
			this.uiPanelGroup1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.uiPanelMenuList)).BeginInit();
			this.uiPanelMenuList.SuspendLayout();
			this.uiPanelMenuListContainer.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.grdExItemList)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.uiPanelItemSch)).BeginInit();
			this.uiPanelItemSch.SuspendLayout();
			this.uiPanelItemSchContainer.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.grdExSchList)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.dvSchedule)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.uiPanelCommand)).BeginInit();
			this.uiPanelCommand.SuspendLayout();
			this.uiPanelCommandContainer.SuspendLayout();
			this.pnlDetail.SuspendLayout();
			this.SuspendLayout();
			// 
			// dvItem
			// 
			this.dvItem.Table = this.schMenuAdDs.AdItems;
			// 
			// schMenuAdDs
			// 
			this.schMenuAdDs.DataSetName = "SchMenuAdDs";
			this.schMenuAdDs.Locale = new System.Globalization.CultureInfo("en-US");
			this.schMenuAdDs.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
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
			this.uiPanel0.Id = new System.Guid("2806d5be-9313-4880-9375-8ddea70da2d9");
			this.uiPanel0.StaticGroup = true;
			this.uiPanelGroup3.Id = new System.Guid("6cb04851-b145-4e62-96cc-1ce0862ebd87");
			this.uiPanelGroup3.StaticGroup = true;
			this.uiPanelGroup1.Id = new System.Guid("7aed39b4-c8d5-4c36-b797-cee3dbec5dfd");
			this.uiPanelGroup1.StaticGroup = true;
			this.uiPanelMenuList.Id = new System.Guid("0b131b73-7292-40ae-a0eb-eec70c0194b3");
			this.uiPanelGroup1.Panels.Add(this.uiPanelMenuList);
			this.uiPanelGroup3.Panels.Add(this.uiPanelGroup1);
			this.uiPanelItemSch.Id = new System.Guid("bbbf6752-952b-4c84-ab4e-c38ba43740bc");
			this.uiPanelGroup3.Panels.Add(this.uiPanelItemSch);
			this.uiPanel0.Panels.Add(this.uiPanelGroup3);
			this.uiPanelChooseAdSchedule.Panels.Add(this.uiPanel0);
			this.uiPanelUsers.Panels.Add(this.uiPanelChooseAdSchedule);
			this.uiPanelCommand.Id = new System.Guid("c1ac152b-71d8-497e-a36e-0758f080f6ec");
			this.uiPanelUsers.Panels.Add(this.uiPanelCommand);
			this.uiPM.Panels.Add(this.uiPanelUsers);
			// 
			// Design Time Panel Info:
			// 
			this.uiPM.BeginPanelInfo();
			this.uiPM.AddDockPanelInfo(new System.Guid("b162c53a-7940-47cd-a0ed-9762c59c50d8"), Janus.Windows.UI.Dock.PanelGroupStyle.HorizontalTiles, Janus.Windows.UI.Dock.PanelDockStyle.Fill, true, new System.Drawing.Size(1010, 677), true);
			this.uiPM.AddDockPanelInfo(new System.Guid("afb3b7d0-aa79-443c-a1e8-3baba38f7a4d"), new System.Guid("b162c53a-7940-47cd-a0ed-9762c59c50d8"), 64, true);
			this.uiPM.AddDockPanelInfo(new System.Guid("88f86479-98e4-43d4-bf03-12fd789fcfc5"), new System.Guid("b162c53a-7940-47cd-a0ed-9762c59c50d8"), Janus.Windows.UI.Dock.PanelGroupStyle.VerticalTiles, true, 544, true);
			this.uiPM.AddDockPanelInfo(new System.Guid("2806d5be-9313-4880-9375-8ddea70da2d9"), new System.Guid("88f86479-98e4-43d4-bf03-12fd789fcfc5"), Janus.Windows.UI.Dock.PanelGroupStyle.HorizontalTiles, true, 847, true);
			this.uiPM.AddDockPanelInfo(new System.Guid("6cb04851-b145-4e62-96cc-1ce0862ebd87"), new System.Guid("2806d5be-9313-4880-9375-8ddea70da2d9"), Janus.Windows.UI.Dock.PanelGroupStyle.HorizontalTiles, true, 237, true);
			this.uiPM.AddDockPanelInfo(new System.Guid("7aed39b4-c8d5-4c36-b797-cee3dbec5dfd"), new System.Guid("6cb04851-b145-4e62-96cc-1ce0862ebd87"), Janus.Windows.UI.Dock.PanelGroupStyle.HorizontalTiles, true, 193, true);
			this.uiPM.AddDockPanelInfo(new System.Guid("0b131b73-7292-40ae-a0eb-eec70c0194b3"), new System.Guid("7aed39b4-c8d5-4c36-b797-cee3dbec5dfd"), 237, true);
			this.uiPM.AddDockPanelInfo(new System.Guid("bbbf6752-952b-4c84-ab4e-c38ba43740bc"), new System.Guid("6cb04851-b145-4e62-96cc-1ce0862ebd87"), 297, true);
			this.uiPM.AddDockPanelInfo(new System.Guid("c1ac152b-71d8-497e-a36e-0758f080f6ec"), new System.Guid("b162c53a-7940-47cd-a0ed-9762c59c50d8"), 41, true);
			this.uiPM.AddFloatingPanelInfo(new System.Guid("b162c53a-7940-47cd-a0ed-9762c59c50d8"), Janus.Windows.UI.Dock.PanelGroupStyle.HorizontalTiles, true, new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
			this.uiPM.AddFloatingPanelInfo(new System.Guid("8e3140b9-bfcb-43af-b84e-2ee6cddf51dc"), new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
			this.uiPM.AddFloatingPanelInfo(new System.Guid("b2973a57-d3e1-4d80-9387-7a6dc3332542"), new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
			this.uiPM.AddFloatingPanelInfo(new System.Guid("afb3b7d0-aa79-443c-a1e8-3baba38f7a4d"), new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
			this.uiPM.AddFloatingPanelInfo(new System.Guid("88f86479-98e4-43d4-bf03-12fd789fcfc5"), Janus.Windows.UI.Dock.PanelGroupStyle.VerticalTiles, true, new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
			this.uiPM.AddFloatingPanelInfo(new System.Guid("91eeb7fc-856c-4a9c-b37e-db742cfa9b2e"), new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
			this.uiPM.AddFloatingPanelInfo(new System.Guid("3d64f731-de32-42ba-a10f-ac3b5ac56703"), new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
			this.uiPM.AddFloatingPanelInfo(new System.Guid("609188ab-b98f-4466-8472-b8b36f1af6d5"), Janus.Windows.UI.Dock.PanelGroupStyle.HorizontalTiles, true, new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
			this.uiPM.AddFloatingPanelInfo(new System.Guid("c1ac152b-71d8-497e-a36e-0758f080f6ec"), new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
			this.uiPM.AddFloatingPanelInfo(new System.Guid("2806d5be-9313-4880-9375-8ddea70da2d9"), Janus.Windows.UI.Dock.PanelGroupStyle.HorizontalTiles, true, new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
			this.uiPM.AddFloatingPanelInfo(new System.Guid("f04e3a8a-bee1-4b6d-8d1f-a73ceeb294a2"), Janus.Windows.UI.Dock.PanelGroupStyle.HorizontalTiles, true, new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
			this.uiPM.AddFloatingPanelInfo(new System.Guid("0b131b73-7292-40ae-a0eb-eec70c0194b3"), new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
			this.uiPM.AddFloatingPanelInfo(new System.Guid("51a04026-a9b8-4a3b-a03f-d44286c91b63"), new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
			this.uiPM.AddFloatingPanelInfo(new System.Guid("a218bd6e-bac7-4d69-93f7-1ec2a85cd1cd"), new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
			this.uiPM.AddFloatingPanelInfo(new System.Guid("89b897d8-ab06-4b0b-88d2-23baaa7df778"), new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
			this.uiPM.AddFloatingPanelInfo(new System.Guid("2d341546-49ef-4382-af68-ea351671cf39"), new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
			this.uiPM.AddFloatingPanelInfo(new System.Guid("7aed39b4-c8d5-4c36-b797-cee3dbec5dfd"), Janus.Windows.UI.Dock.PanelGroupStyle.HorizontalTiles, true, new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
			this.uiPM.AddFloatingPanelInfo(new System.Guid("eda9799c-1c5f-43f5-b22a-5ed1cb9e55a5"), Janus.Windows.UI.Dock.PanelGroupStyle.HorizontalTiles, true, new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
			this.uiPM.AddFloatingPanelInfo(new System.Guid("5b747dc2-aafb-44a5-a251-26985fcd98c2"), Janus.Windows.UI.Dock.PanelGroupStyle.VerticalTiles, true, new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
			this.uiPM.AddFloatingPanelInfo(new System.Guid("f32d7dd7-16c4-4660-af64-5b2e10a33778"), Janus.Windows.UI.Dock.PanelGroupStyle.HorizontalTiles, true, new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
			this.uiPM.AddFloatingPanelInfo(new System.Guid("20e12e36-c369-4e66-8a1d-dcd69a9bf825"), Janus.Windows.UI.Dock.PanelGroupStyle.HorizontalTiles, true, new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
			this.uiPM.AddFloatingPanelInfo(new System.Guid("65a7b818-9a0f-4743-a848-93c8fa3dd46a"), Janus.Windows.UI.Dock.PanelGroupStyle.HorizontalTiles, true, new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
			this.uiPM.AddFloatingPanelInfo(new System.Guid("c94b5d77-bec3-4ba0-9ced-062dc2a5b910"), Janus.Windows.UI.Dock.PanelGroupStyle.HorizontalTiles, true, new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
			this.uiPM.AddFloatingPanelInfo(new System.Guid("4689877a-bf9c-4573-9b99-2d59d6bb7df4"), Janus.Windows.UI.Dock.PanelGroupStyle.HorizontalTiles, true, new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
			this.uiPM.AddFloatingPanelInfo(new System.Guid("f59601e1-4331-435a-bf00-7f9263966efd"), new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
			this.uiPM.AddFloatingPanelInfo(new System.Guid("bbbf6752-952b-4c84-ab4e-c38ba43740bc"), new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
			this.uiPM.AddFloatingPanelInfo(new System.Guid("6cb04851-b145-4e62-96cc-1ce0862ebd87"), Janus.Windows.UI.Dock.PanelGroupStyle.HorizontalTiles, true, new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
			this.uiPM.EndPanelInfo();
			// 
			// uiPanelUsers
			// 
			this.uiPanelUsers.ActiveCaptionMode = Janus.Windows.UI.Dock.ActiveCaptionMode.Never;
			this.uiPanelUsers.BackColor = System.Drawing.SystemColors.Window;
			this.uiPanelUsers.CaptionFormatStyle.FontBold = Janus.Windows.UI.TriState.True;
			this.uiPanelUsers.CaptionStyle = Janus.Windows.UI.Dock.PanelCaptionStyle.Dark;
			this.uiPanelUsers.CaptionVisible = Janus.Windows.UI.InheritableBoolean.False;
			this.uiPanelUsers.CloseButtonVisible = Janus.Windows.UI.InheritableBoolean.False;
			this.uiPanelUsers.Location = new System.Drawing.Point(0, 0);
			this.uiPanelUsers.Name = "uiPanelUsers";
			this.uiPanelUsers.Size = new System.Drawing.Size(1010, 677);
			this.uiPanelUsers.TabIndex = 4;
			// 
			// uiPanelSearch
			// 
			this.uiPanelSearch.AllowResize = Janus.Windows.UI.InheritableBoolean.False;
			this.uiPanelSearch.CaptionVisible = Janus.Windows.UI.InheritableBoolean.True;
			this.uiPanelSearch.CloseButtonVisible = Janus.Windows.UI.InheritableBoolean.False;
			this.uiPanelSearch.InnerContainer = this.uiPanelSearchContainer;
			this.uiPanelSearch.Location = new System.Drawing.Point(0, 0);
			this.uiPanelSearch.Name = "uiPanelSearch";
			this.uiPanelSearch.Size = new System.Drawing.Size(1010, 65);
			this.uiPanelSearch.TabIndex = 4;
			this.uiPanelSearch.Text = "재핑광고 편성";
			// 
			// uiPanelSearchContainer
			// 
			this.uiPanelSearchContainer.Controls.Add(this.pnlSearch);
			this.uiPanelSearchContainer.Location = new System.Drawing.Point(1, 23);
			this.uiPanelSearchContainer.Name = "uiPanelSearchContainer";
			this.uiPanelSearchContainer.Size = new System.Drawing.Size(1008, 41);
			this.uiPanelSearchContainer.TabIndex = 0;
			// 
			// pnlSearch
			// 
			this.pnlSearch.BackColor = System.Drawing.SystemColors.Window;
			this.pnlSearch.Controls.Add(this.chkAdState_10);
			this.pnlSearch.Controls.Add(this.chkAdState_40);
			this.pnlSearch.Controls.Add(this.chkAdState_30);
			this.pnlSearch.Controls.Add(this.chkAdState_20);
			this.pnlSearch.Controls.Add(this.label3);
			this.pnlSearch.Controls.Add(this.pnlSearchBtn);
			this.pnlSearch.Dock = System.Windows.Forms.DockStyle.Fill;
			this.pnlSearch.Location = new System.Drawing.Point(0, 0);
			this.pnlSearch.Name = "pnlSearch";
			this.pnlSearch.Size = new System.Drawing.Size(1008, 41);
			this.pnlSearch.TabIndex = 0;
			// 
			// chkAdState_10
			// 
			this.chkAdState_10.BackColor = System.Drawing.SystemColors.Window;
			this.chkAdState_10.Checked = true;
			this.chkAdState_10.CheckedValue = "";
			this.chkAdState_10.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkAdState_10.ForeColor = System.Drawing.SystemColors.ControlText;
			this.chkAdState_10.Location = new System.Drawing.Point(70, 10);
			this.chkAdState_10.Name = "chkAdState_10";
			this.chkAdState_10.Size = new System.Drawing.Size(44, 21);
			this.chkAdState_10.TabIndex = 2;
			this.chkAdState_10.Text = "대기";
			this.chkAdState_10.VisualStyle = Janus.Windows.UI.VisualStyle.VS2005;
			// 
			// chkAdState_40
			// 
			this.chkAdState_40.BackColor = System.Drawing.SystemColors.Window;
			this.chkAdState_40.Location = new System.Drawing.Point(268, 10);
			this.chkAdState_40.Name = "chkAdState_40";
			this.chkAdState_40.Size = new System.Drawing.Size(44, 21);
			this.chkAdState_40.TabIndex = 5;
			this.chkAdState_40.Text = "종료";
			this.chkAdState_40.VisualStyle = Janus.Windows.UI.VisualStyle.VS2005;
			// 
			// chkAdState_30
			// 
			this.chkAdState_30.BackColor = System.Drawing.SystemColors.Window;
			this.chkAdState_30.Location = new System.Drawing.Point(202, 10);
			this.chkAdState_30.Name = "chkAdState_30";
			this.chkAdState_30.Size = new System.Drawing.Size(44, 21);
			this.chkAdState_30.TabIndex = 4;
			this.chkAdState_30.Text = "중지";
			this.chkAdState_30.VisualStyle = Janus.Windows.UI.VisualStyle.VS2005;
			// 
			// chkAdState_20
			// 
			this.chkAdState_20.BackColor = System.Drawing.SystemColors.Window;
			this.chkAdState_20.Checked = true;
			this.chkAdState_20.CheckedValue = "";
			this.chkAdState_20.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkAdState_20.ForeColor = System.Drawing.SystemColors.ControlText;
			this.chkAdState_20.Location = new System.Drawing.Point(136, 10);
			this.chkAdState_20.Name = "chkAdState_20";
			this.chkAdState_20.Size = new System.Drawing.Size(44, 21);
			this.chkAdState_20.TabIndex = 3;
			this.chkAdState_20.Text = "편성";
			this.chkAdState_20.VisualStyle = Janus.Windows.UI.VisualStyle.VS2005;
			// 
			// label3
			// 
			this.label3.BackColor = System.Drawing.SystemColors.Window;
			this.label3.Location = new System.Drawing.Point(6, 10);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(56, 21);
			this.label3.TabIndex = 49;
			this.label3.Text = "광고상태";
			this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// pnlSearchBtn
			// 
			this.pnlSearchBtn.BackColor = System.Drawing.SystemColors.Window;
			this.pnlSearchBtn.Controls.Add(this.btnSearch);
			this.pnlSearchBtn.Dock = System.Windows.Forms.DockStyle.Right;
			this.pnlSearchBtn.Location = new System.Drawing.Point(888, 0);
			this.pnlSearchBtn.Name = "pnlSearchBtn";
			this.pnlSearchBtn.Size = new System.Drawing.Size(120, 41);
			this.pnlSearchBtn.TabIndex = 5;
			// 
			// btnSearch
			// 
			this.btnSearch.Cursor = System.Windows.Forms.Cursors.Hand;
			this.btnSearch.Enabled = false;
			this.btnSearch.Location = new System.Drawing.Point(8, 7);
			this.btnSearch.Name = "btnSearch";
			this.btnSearch.Size = new System.Drawing.Size(104, 24);
			this.btnSearch.TabIndex = 6;
			this.btnSearch.Text = "조 회";
			this.btnSearch.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
			this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
			// 
			// uiPanelChooseAdSchedule
			// 
			this.uiPanelChooseAdSchedule.AllowResize = Janus.Windows.UI.InheritableBoolean.True;
			this.uiPanelChooseAdSchedule.CaptionVisible = Janus.Windows.UI.InheritableBoolean.False;
			this.uiPanelChooseAdSchedule.GroupStyle = Janus.Windows.UI.Dock.PanelGroupStyle.VerticalTiles;
			this.uiPanelChooseAdSchedule.Location = new System.Drawing.Point(0, 69);
			this.uiPanelChooseAdSchedule.Name = "uiPanelChooseAdSchedule";
			this.uiPanelChooseAdSchedule.Size = new System.Drawing.Size(1010, 562);
			this.uiPanelChooseAdSchedule.TabIndex = 4;
			this.uiPanelChooseAdSchedule.Text = "메뉴/채널 목록";
			// 
			// uiPanel0
			// 
			this.uiPanel0.CaptionVisible = Janus.Windows.UI.InheritableBoolean.False;
			this.uiPanel0.Location = new System.Drawing.Point(0, 0);
			this.uiPanel0.Name = "uiPanel0";
			this.uiPanel0.Size = new System.Drawing.Size(1010, 562);
			this.uiPanel0.TabIndex = 4;
			this.uiPanel0.Text = "Panel 0";
			// 
			// uiPanelGroup3
			// 
			this.uiPanelGroup3.CaptionVisible = Janus.Windows.UI.InheritableBoolean.False;
			this.uiPanelGroup3.Location = new System.Drawing.Point(0, 0);
			this.uiPanelGroup3.Name = "uiPanelGroup3";
			this.uiPanelGroup3.Size = new System.Drawing.Size(1010, 562);
			this.uiPanelGroup3.TabIndex = 0;
			// 
			// uiPanelGroup1
			// 
			this.uiPanelGroup1.CaptionVisible = Janus.Windows.UI.InheritableBoolean.False;
			this.uiPanelGroup1.Location = new System.Drawing.Point(0, 0);
			this.uiPanelGroup1.Name = "uiPanelGroup1";
			this.uiPanelGroup1.Size = new System.Drawing.Size(1010, 220);
			this.uiPanelGroup1.TabIndex = 1;
			this.uiPanelGroup1.Text = "PanelGroup1";
			// 
			// uiPanelMenuList
			// 
			this.uiPanelMenuList.CaptionVisible = Janus.Windows.UI.InheritableBoolean.True;
			this.uiPanelMenuList.InnerContainer = this.uiPanelMenuListContainer;
			this.uiPanelMenuList.Location = new System.Drawing.Point(0, 0);
			this.uiPanelMenuList.Name = "uiPanelMenuList";
			this.uiPanelMenuList.Size = new System.Drawing.Size(1010, 220);
			this.uiPanelMenuList.TabIndex = 4;
			this.uiPanelMenuList.Text = "광고목록";
			// 
			// uiPanelMenuListContainer
			// 
			this.uiPanelMenuListContainer.Controls.Add(this.grdExItemList);
			this.uiPanelMenuListContainer.Location = new System.Drawing.Point(1, 23);
			this.uiPanelMenuListContainer.Name = "uiPanelMenuListContainer";
			this.uiPanelMenuListContainer.Size = new System.Drawing.Size(1008, 196);
			this.uiPanelMenuListContainer.TabIndex = 0;
			// 
			// grdExItemList
			// 
			this.grdExItemList.AlternatingColors = true;
			this.grdExItemList.BorderStyle = Janus.Windows.GridEX.BorderStyle.None;
			this.grdExItemList.DataSource = this.dvItem;
			this.grdExItemList.Dock = System.Windows.Forms.DockStyle.Fill;
			this.grdExItemList.EmptyRows = true;
			this.grdExItemList.FocusCellFormatStyle.BackColor = System.Drawing.SystemColors.Highlight;
			this.grdExItemList.FocusCellFormatStyle.ForeColor = System.Drawing.Color.White;
			this.grdExItemList.FocusStyle = Janus.Windows.GridEX.FocusStyle.None;
			this.grdExItemList.FrozenColumns = 3;
			this.grdExItemList.GridLines = Janus.Windows.GridEX.GridLines.Vertical;
			this.grdExItemList.GridLineStyle = Janus.Windows.GridEX.GridLineStyle.Solid;
			this.grdExItemList.GroupByBoxVisible = false;
			this.grdExItemList.GroupMode = Janus.Windows.GridEX.GroupMode.Collapsed;
			this.grdExItemList.HeaderFormatStyle.TextAlignment = Janus.Windows.GridEX.TextAlignment.Center;
			this.grdExItemList.HideSelection = Janus.Windows.GridEX.HideSelection.HighlightInactive;
			grdExItemList_Layout_0.DataSource = this.dvItem;
			grdExItemList_Layout_0.IsCurrentLayout = true;
			grdExItemList_Layout_0.Key = "bae";
			grdExItemList_Layout_0.LayoutString = resources.GetString("grdExItemList_Layout_0.LayoutString");
			this.grdExItemList.Layouts.AddRange(new Janus.Windows.GridEX.GridEXLayout[] {
            grdExItemList_Layout_0});
			this.grdExItemList.Location = new System.Drawing.Point(0, 0);
			this.grdExItemList.Name = "grdExItemList";
			this.grdExItemList.ScrollBars = Janus.Windows.GridEX.ScrollBars.Vertical;
			this.grdExItemList.SelectedInactiveFormatStyle.BackColor = System.Drawing.Color.Gold;
			this.grdExItemList.SelectedInactiveFormatStyle.ForeColor = System.Drawing.Color.Empty;
			this.grdExItemList.Size = new System.Drawing.Size(1008, 196);
			this.grdExItemList.TabIndex = 7;
			this.grdExItemList.TabKeyBehavior = Janus.Windows.GridEX.TabKeyBehavior.ControlNavigation;
			this.grdExItemList.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
			this.grdExItemList.SelectionChanged += new System.EventHandler(this.grdExItemList_SelectionChanged);
			// 
			// uiPanelItemSch
			// 
			this.uiPanelItemSch.InnerContainer = this.uiPanelItemSchContainer;
			this.uiPanelItemSch.Location = new System.Drawing.Point(0, 224);
			this.uiPanelItemSch.Name = "uiPanelItemSch";
			this.uiPanelItemSch.Size = new System.Drawing.Size(1010, 338);
			this.uiPanelItemSch.TabIndex = 4;
			this.uiPanelItemSch.Text = "편성현황";
			// 
			// uiPanelItemSchContainer
			// 
			this.uiPanelItemSchContainer.Controls.Add(this.grdExSchList);
			this.uiPanelItemSchContainer.Location = new System.Drawing.Point(1, 23);
			this.uiPanelItemSchContainer.Name = "uiPanelItemSchContainer";
			this.uiPanelItemSchContainer.Size = new System.Drawing.Size(1008, 314);
			this.uiPanelItemSchContainer.TabIndex = 0;
			// 
			// grdExSchList
			// 
			this.grdExSchList.AlternatingColors = true;
			this.grdExSchList.BorderStyle = Janus.Windows.GridEX.BorderStyle.None;
			this.grdExSchList.CellSelectionMode = Janus.Windows.GridEX.CellSelectionMode.SingleCell;
			this.grdExSchList.DataSource = this.dvSchedule;
			grdExSchList_DesignTimeLayout_Reference_0.Instance = ((object)(resources.GetObject("grdExSchList_DesignTimeLayout_Reference_0.Instance")));
			grdExSchList_DesignTimeLayout_Reference_1.Instance = ((object)(resources.GetObject("grdExSchList_DesignTimeLayout_Reference_1.Instance")));
			grdExSchList_DesignTimeLayout_Reference_2.Instance = ((object)(resources.GetObject("grdExSchList_DesignTimeLayout_Reference_2.Instance")));
			grdExSchList_DesignTimeLayout_Reference_3.Instance = ((object)(resources.GetObject("grdExSchList_DesignTimeLayout_Reference_3.Instance")));
			grdExSchList_DesignTimeLayout.LayoutReferences.AddRange(new Janus.Windows.Common.Layouts.JanusLayoutReference[] {
            grdExSchList_DesignTimeLayout_Reference_0,
            grdExSchList_DesignTimeLayout_Reference_1,
            grdExSchList_DesignTimeLayout_Reference_2,
            grdExSchList_DesignTimeLayout_Reference_3});
			grdExSchList_DesignTimeLayout.LayoutString = resources.GetString("grdExSchList_DesignTimeLayout.LayoutString");
			this.grdExSchList.DesignTimeLayout = grdExSchList_DesignTimeLayout;
			this.grdExSchList.Dock = System.Windows.Forms.DockStyle.Fill;
			this.grdExSchList.EmptyRows = true;
			this.grdExSchList.FocusCellFormatStyle.BackColor = System.Drawing.SystemColors.Highlight;
			this.grdExSchList.FocusStyle = Janus.Windows.GridEX.FocusStyle.None;
			this.grdExSchList.FrozenColumns = 2;
			this.grdExSchList.GridLineColor = System.Drawing.Color.Silver;
			this.grdExSchList.GridLines = Janus.Windows.GridEX.GridLines.Vertical;
			this.grdExSchList.GridLineStyle = Janus.Windows.GridEX.GridLineStyle.Solid;
			this.grdExSchList.GroupByBoxVisible = false;
			this.grdExSchList.ImageList = this.ilSchType;
			this.grdExSchList.Location = new System.Drawing.Point(0, 0);
			this.grdExSchList.Name = "grdExSchList";
			this.grdExSchList.ScrollBars = Janus.Windows.GridEX.ScrollBars.Vertical;
			this.grdExSchList.SelectedInactiveFormatStyle.BackColor = System.Drawing.Color.DarkOrange;
			this.grdExSchList.Size = new System.Drawing.Size(1008, 314);
			this.grdExSchList.TabIndex = 9;
			this.grdExSchList.TabKeyBehavior = Janus.Windows.GridEX.TabKeyBehavior.ControlNavigation;
			this.grdExSchList.ThemedAreas = ((Janus.Windows.GridEX.ThemedArea)(((((((((Janus.Windows.GridEX.ThemedArea.ScrollBars | Janus.Windows.GridEX.ThemedArea.EditControls) 
            | Janus.Windows.GridEX.ThemedArea.Headers) 
            | Janus.Windows.GridEX.ThemedArea.GroupByBox) 
            | Janus.Windows.GridEX.ThemedArea.GroupRows) 
            | Janus.Windows.GridEX.ThemedArea.ControlBorder) 
            | Janus.Windows.GridEX.ThemedArea.Cards) 
            | Janus.Windows.GridEX.ThemedArea.Gridlines) 
            | Janus.Windows.GridEX.ThemedArea.CheckBoxes)));
			this.grdExSchList.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2003;
			// 
			// dvSchedule
			// 
			this.dvSchedule.Table = this.schMenuAdDs.AdSchedule;
			// 
			// ilSchType
			// 
			this.ilSchType.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("ilSchType.ImageStream")));
			this.ilSchType.TransparentColor = System.Drawing.Color.Transparent;
			this.ilSchType.Images.SetKeyName(0, "");
			this.ilSchType.Images.SetKeyName(1, "");
			this.ilSchType.Images.SetKeyName(2, "");
			this.ilSchType.Images.SetKeyName(3, "");
			this.ilSchType.Images.SetKeyName(4, "");
			this.ilSchType.Images.SetKeyName(5, "");
			this.ilSchType.Images.SetKeyName(6, "");
			this.ilSchType.Images.SetKeyName(7, "");
			// 
			// uiPanelCommand
			// 
			this.uiPanelCommand.AllowResize = Janus.Windows.UI.InheritableBoolean.False;
			this.uiPanelCommand.CaptionVisible = Janus.Windows.UI.InheritableBoolean.False;
			this.uiPanelCommand.InnerContainer = this.uiPanelCommandContainer;
			this.uiPanelCommand.Location = new System.Drawing.Point(0, 635);
			this.uiPanelCommand.Name = "uiPanelCommand";
			this.uiPanelCommand.Size = new System.Drawing.Size(1010, 42);
			this.uiPanelCommand.TabIndex = 4;
			this.uiPanelCommand.Text = "편성";
			// 
			// uiPanelCommandContainer
			// 
			this.uiPanelCommandContainer.Controls.Add(this.pnlDetail);
			this.uiPanelCommandContainer.Location = new System.Drawing.Point(1, 1);
			this.uiPanelCommandContainer.Name = "uiPanelCommandContainer";
			this.uiPanelCommandContainer.Size = new System.Drawing.Size(1008, 40);
			this.uiPanelCommandContainer.TabIndex = 0;
			// 
			// pnlDetail
			// 
			this.pnlDetail.BackColor = System.Drawing.SystemColors.Window;
			this.pnlDetail.Controls.Add(this.lbMsg);
			this.pnlDetail.Controls.Add(this.btnDelete);
			this.pnlDetail.Controls.Add(this.btnAddSchedule);
			this.pnlDetail.Dock = System.Windows.Forms.DockStyle.Fill;
			this.pnlDetail.Location = new System.Drawing.Point(0, 0);
			this.pnlDetail.Name = "pnlDetail";
			this.pnlDetail.Size = new System.Drawing.Size(1008, 40);
			this.pnlDetail.TabIndex = 10;
			// 
			// lbMsg
			// 
			this.lbMsg.Location = new System.Drawing.Point(232, 10);
			this.lbMsg.Name = "lbMsg";
			this.lbMsg.Size = new System.Drawing.Size(592, 21);
			this.lbMsg.TabIndex = 43;
			this.lbMsg.Text = "편성";
			this.lbMsg.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// btnDelete
			// 
			this.btnDelete.Cursor = System.Windows.Forms.Cursors.Hand;
			this.btnDelete.Enabled = false;
			this.btnDelete.Location = new System.Drawing.Point(118, 8);
			this.btnDelete.Name = "btnDelete";
			this.btnDelete.Size = new System.Drawing.Size(104, 24);
			this.btnDelete.TabIndex = 10;
			this.btnDelete.Text = "삭 제";
			this.btnDelete.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
			this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
			// 
			// btnAddSchedule
			// 
			this.btnAddSchedule.BackColor = System.Drawing.Color.White;
			this.btnAddSchedule.Cursor = System.Windows.Forms.Cursors.Hand;
			this.btnAddSchedule.Location = new System.Drawing.Point(6, 8);
			this.btnAddSchedule.Name = "btnAddSchedule";
			this.btnAddSchedule.Size = new System.Drawing.Size(104, 24);
			this.btnAddSchedule.TabIndex = 12;
			this.btnAddSchedule.Text = "편성";
			this.btnAddSchedule.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
			this.btnAddSchedule.Click += new System.EventHandler(this.btnAddSchedule_Click);
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
			// SchMenuAdControl
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.Controls.Add(this.uiPanelUsers);
			this.Font = global::AdManagerClient.Properties.Settings.Default.Font1;
			this.Name = "SchMenuAdControl";
			this.Size = new System.Drawing.Size(1010, 677);
			this.Load += new System.EventHandler(this.SchMenuAdControl_Load);
			((System.ComponentModel.ISupportInitialize)(this.dvItem)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.schMenuAdDs)).EndInit();
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
			((System.ComponentModel.ISupportInitialize)(this.uiPanel0)).EndInit();
			this.uiPanel0.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.uiPanelGroup3)).EndInit();
			this.uiPanelGroup3.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.uiPanelGroup1)).EndInit();
			this.uiPanelGroup1.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.uiPanelMenuList)).EndInit();
			this.uiPanelMenuList.ResumeLayout(false);
			this.uiPanelMenuListContainer.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.grdExItemList)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.uiPanelItemSch)).EndInit();
			this.uiPanelItemSch.ResumeLayout(false);
			this.uiPanelItemSchContainer.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.grdExSchList)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.dvSchedule)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.uiPanelCommand)).EndInit();
			this.uiPanelCommand.ResumeLayout(false);
			this.uiPanelCommandContainer.ResumeLayout(false);
			this.pnlDetail.ResumeLayout(false);
			this.ResumeLayout(false);

		}
        #endregion

        #region 컨트롤 로드
        private void SchMenuAdControl_Load(object sender, System.EventArgs e)
        {
			// 편성현황
			cmSchedule = (CurrencyManager) this.BindingContext[grdExSchList.DataSource]; 
			dtSchedule = ((DataView)grdExSchList.DataSource).Table;

			// 광고
			dtItem = ((DataView)grdExItemList.DataSource).Table;  
			cmItem = (CurrencyManager) this.BindingContext[grdExItemList.DataSource]; 

			// 컨트롤 초기화
			InitControl();
		}

        #endregion

        #region 컨트롤 초기화
		/// <summary>
		/// [S1] 컨트롤 초기화
		/// </summary>
        private void InitControl()
        {
            ProgressStart();
			lbMsg.Text = "";

			// 권한 검사
			if(menu.CanCreate(MenuCode))	canCreate = true;
            if(menu.CanRead(MenuCode))		canRead = true;
			if(menu.CanDelete(MenuCode))	canDelete = true;

			InitButton();
			ProgressStop();
			
			if(canRead) SearchItem();
        }

        private void InitButton()
        {
			GetAckState();
			if(canRead)   btnSearch.Enabled = true;
            Application.DoEvents();
        }

        private void DisableButton()
        {
            btnSearch.Enabled = false;
			btnDelete.Enabled = false;
			Application.DoEvents();
        }

        #endregion

		public void ReloadMenuList()
		{
			LoadItemSchedule();
		}

        #region 사용자 액션처리 메소드

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
//				ebSearchKey.Text = "";
			}
			else
			{
//				ebSearchKey.SelectAll();
			}
		}

		private void ebSearchKey_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
		{
			if(e.KeyCode == Keys.Enter && !(IsNewSearchKey))
			{
				SearchItem();
			}
		}

        /// <summary>
        /// 그리드의 Row변경시
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

		private void OnGrdRowChangedSchedule(object sender, System.EventArgs e) 
		{
            if (!IsSearching) // 2011.11.29 JH.Park 조회중이 아닐경우에만 동작하도록 변경
            {
                SetDetailTextSchedule();
            }
		}

		private void grdExItemList_SelectionChanged(object sender, System.EventArgs e)
		{
			if(IsNotLoading)
			{
				SetDetailTextSchedule();
				InitButton();
			}
		}
 

        /// <summary>
        /// 조회버튼 클릭시 처리
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSearch_Click(object sender, System.EventArgs e)
        {			
            ResetDetail();
            DisableButton();
			SearchItem();
            InitButton();
        }

		/// <summary>
		/// [S1] 삭제버튼 클릭시 처리
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnDelete_Click(object sender, System.EventArgs e)
		{
			DeleteItemSchedule();
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

		private void ebGenreCode_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
		{
			// 장르코드입력후 엔터를 치면
			if(e.KeyCode == Keys.Enter)
			{
//				if( ebGenreCode.Text.Length > 0)
//				{
//
//					//ColumnHeader Click시에 dt Setting 
//					DataRow[] foundRows = dtMenu.Select("GenreCode = '" + ebGenreCode.Text + "'");
//         
//					// if(grdExItemList.CurrentColumn.Position == 0){
//					if( foundRows.Length > 0 )
//					{
//
//						keyGenreCode     = foundRows[0]["GenreCode"].ToString();
//						ebGenreName.Text = foundRows[0]["GenreName"].ToString(); 
//						SetCurrentRowMenu();
//						ebGenreCode.SelectAll();
//					}
//					else
//					{
//						DialogResult result = MessageBox.Show("입력한 장르코드로 신규편성을 시작합니다.","메뉴별 지정광고 편성",
//							MessageBoxButtons.YesNo, MessageBoxIcon.Question,
//							MessageBoxDefaultButton.Button2);
//
//						if (result == DialogResult.No) return;					
//					}
//				}		
			}

		}

		private void grdExItemList_ColumnHeaderClick(object sender, Janus.Windows.GridEX.ColumnActionEventArgs e)
		{

			//컬럼Index 0(체크박스컬럼이)이 아니면 빠져나가게 처리.
			if(e.Column.Index != 0)
			{
				return;
			}
            
			//ColumnHeader Click시에 dt Setting 
			DataRow[] foundRows = dtItem.Select("CheckYn = 'False'");
         
			// if(grdExItemList.CurrentColumn.Position == 0){
			if( foundRows.Length == 0 )
			{
				for(int i=0;i < dtItem.Rows.Count;i++)
				{
					dtItem.Rows[i].BeginEdit();
					dtItem.Rows[i]["CheckYn"]="False";
					dtItem.Rows[i].EndEdit();
				}
			}
			else
			{
				for(int i=0;i < dtItem.Rows.Count;i++)
				{
					dtItem.Rows[i].BeginEdit();
					dtItem.Rows[i]["CheckYn"]="True";
					dtItem.Rows[i].EndEdit();
				}
			}
		}


		private void ebItemNo_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
		{
			// 장르코드입력후 엔터를 치면
			if(e.KeyCode == Keys.Enter)
			{
				// 입력한 내용이 있을 때 
//				if( ebItemNo.Text.Length > 0)
//				{
//					//해당광고 편성
//					ebItemNo.SelectAll();
//				}						
			}

		}

		private void ebGenreCode_Click(object sender, System.EventArgs e)
		{
//			ebGenreCode.SelectAll();
		}

		private void ebItemNo_Click(object sender, System.EventArgs e)
		{
//			ebItemNo.SelectAll();		
		}
   
		private void btnAddSchedule_Click(object sender, System.EventArgs e)
		{
			//_30_Schedule.Sch3Form pForm = new AdManagerClient._30_Schedule.Sch3Form();
			//pForm.ScheduleSelected += new AdManagerClient.Schedule.SchedulePerItemInsertEventHandler(OnScheduleSelected);
			//pForm.ItemNo	=	int.Parse(keyItemNo);
			//pForm.ItemName	=	keyItemName;
			//pForm.DataType	=	13;
			//pForm.ShowDialog(this);

			SchMenuAd_pForm pForm = new SchMenuAd_pForm(this);

			pForm.ShowDialog();
			pForm.Dispose();
			pForm = null;		


//			AddSchChoiceAdDeailMenu();		
//			// 체크된 모든 항목을 클리어
//			ClearListCheck_Menu();
//			ReloadSchedule();
		}


        #endregion

        #region 처리메소드

		
		/// <summary>
		/// 키캆을찾아 그리드 키에 해당되는로우로..
		/// </summary>
//		private void SetCurrentRowMenu()
//		{
//
//			int rowIndex = 0;
//			if ( dtMenu.Rows.Count < 1 ) return;              
//			foreach (DataRow row in dtMenu.Rows)
//			{					
//				if(row["GenreCode"].ToString().Equals(keyGenreCode))
//				{					
//					cmMenu.Position = rowIndex;
//					break;								
//				}
//				rowIndex++;
//			}
//			grdExMenuList.EnsureVisible();
//		}

		/// <summary>
		/// [S1] 편성승인 상태를 읽어온후, 신규편성이 가능한 상태인지를 첵크한다
		/// </summary>
		private	void GetAckState()
		{
			try
			{
				keyAckNo    = 0;
				keyAckState = "";

				schPublishModel.Init();
				schPublishModel.SearchMediaCode = "1";

				// 현재 승인상태조회 서비스를 호출한다, CSS광고임으로 상업광고타입으로 조회한다
				new SchPublishManager(systemModel,commonModel).GetPublishState(schPublishModel,10);

				if (schPublishModel.ResultCD.Equals("0000"))
				{
					keyAckNo    = int.Parse(schPublishModel.AckNo);
					keyAckState = schPublishModel.State;

					if(keyAckState.Equals("10"))	// 승인상태가 10:편성중이면
					{
						lbMsg.Text = "편성 진행중입니다.";
					}
					else if(keyAckState.Equals("20")) // 승인상태가 20:편성승인 상태이면 편성이 불가하다.
					{
						lbMsg.Text = "검수승인 대기중입니다.";
						canCreate = false;
						canDelete = false;
					}
					else if(keyAckState.Equals("25")) // 승인상태가 25:배포대기 상태이면 편성이 불가하다.
					{
						lbMsg.Text = "배포승인 대기중입니다.";
						canCreate = false;
						canDelete = false;
					}
					else if(keyAckState.Equals("30")) // 승인상태가 30:배포승인 상태이면 신규편성이 가능하다.
					{
						lbMsg.Text = "신규편성이 가능한 상태 입니다";
					}
				}
			}
			catch(FrameException fe)
			{
			    FrameSystem.showMsgForm("편성승인 상태 조회오류", new string[] {fe.ErrCode, fe.ResultMsg});
			}
			catch(Exception ex)
			{
			    FrameSystem.showMsgForm("편성승인 상태 조회오류",new string[] {"",ex.Message});
			}
		}


		private void SetDetailTextMenu()
		{
			int curRow = cmMenu.Position;

			if(curRow >= 0 )
			{
				IsNotLoading = false;	// 조회중 다시 조회되는 것을 방지함.
				try
				{
					//keyCmType       = "M";

//					uiPanelList.Text = ""
//						+ "메뉴별 편성현황 : " 
//						+ dtMenu.Rows[curRow]["MediaName"].ToString().Trim() + " > " 
//						+ dtMenu.Rows[curRow]["CategoryCode"].ToString().Trim() + " " 
//						+ dtMenu.Rows[curRow]["CategoryName"].ToString().Trim() + " > " 
//						+ dtMenu.Rows[curRow]["GenreCode"].ToString().Trim() + " " 
//						+ dtMenu.Rows[curRow]["GenreName"].ToString().Trim()
//						;		
 
					keyMediaCode    = dtMenu.Rows[curRow]["MediaCode"].ToString();
					keyCategoryCode = dtMenu.Rows[curRow]["CategoryCode"].ToString();
					keyGenreCode    = dtMenu.Rows[curRow]["GenreCode"].ToString();

					schMenuAdDs.AdSchedule.Clear();        
//					SearchChooseAdScheduleMenu();
				}
				finally
				{
					IsNotLoading = true;
				}
			}

			StatusMessage("준비");
		}

		private void SetDetailTextSchedule()
		{
			ResetDetail();

			int curRow = cmItem.Position;

			if(curRow >= 0 )
			{
				uiPanelItemSch.Text = " 편성광고 : " + dtItem.Rows[curRow]["AdTypeName"].ToString().Trim() + " [" + dtItem.Rows[curRow]["ItemNo"].ToString() + "]" + dtItem.Rows[curRow]["ItemName"].ToString().Trim();
				keyItemNo        = dtItem.Rows[curRow]["ItemNo"].ToString();
				keyItemName      = dtItem.Rows[curRow]["ItemName"].ToString();
				schMenuAdDs.AdSchedule.Clear();

				// 해당광고의 편성내역을 읽어온다
				LoadItemSchedule();
			}
		}

    	/// <summary>
    	/// [S1] 선택된 편성정보를 삭제합니다
    	/// </summary>
		private void DeleteItemSchedule()
		{
			StatusMessage("선택된 편성정보 내역을 삭제합니다.");

			//그리드에 변경된 데이터를 Datasource에 업데이트 한다.
			grdExSchList.UpdateData();

			DialogResult result = MessageBox.Show("선택하신 편성정보 내역을 삭제 하시겠습니까?","재핑광고 편성",MessageBoxButtons.YesNo, MessageBoxIcon.Question,MessageBoxDefaultButton.Button2);
			if (result == DialogResult.No) return;

			try
			{
				foreach( DataRow row in schMenuAdDs.AdSchedule.Rows )
				{
                    if( row["CheckYn"].ToString().Equals("True") )
					{
						schChoiceAdModel.Init();

						schChoiceAdModel.ItemNo = keyItemNo;
						schChoiceAdModel.GenreCode = row["Genre"].ToString();
						schChoiceAdModel.AdType = "31";

						new SchChoiceAdManager(systemModel, commonModel).SetSchChoiceRealChDetailDelete(schChoiceAdModel);

						if (!schChoiceAdModel.ResultCD.Equals("0000"))
						{
							FrameSystem.showMsgForm("편성정보 삭제오류", new string[] { schChoiceAdModel.ResultCD, schChoiceAdModel.ResultDesc });
							return;
						}
					}
					Application.DoEvents();
				}

				ReloadSchedule();
				StatusMessage("선택된 편성정보가 삭제되었습니다...");
			}
			catch(FrameException fe)
			{
				FrameSystem.showMsgForm("편성정보 삭제오류", new string[] {fe.ErrCode, fe.ResultMsg});
			}
			catch(Exception ex)
			{
				FrameSystem.showMsgForm("편성정보 삭제오류",new string[] {"",ex.Message});
			}			
		}


		private void InsertItemSchedule( ArrayList e )
		{
			StatusMessage("선택된 편성정보 내역을 추가합니다.");

			try
			{
				for( int i = 0; i< e.Count ; i++)
				{
					schPerItemModel.Init();
					schPerItemModel.ItemNo	=	Convert.ToInt32( keyItemNo );
					schPerItemModel.Media	=	((SchedulePerItemModel)e[i]).Media;
					schPerItemModel.Category=	((SchedulePerItemModel)e[i]).Category;
					schPerItemModel.Genre	=	((SchedulePerItemModel)e[i]).Genre;
					schPerItemModel.Channel	=	((SchedulePerItemModel)e[i]).Channel;
					schPerItemModel.Series	=	((SchedulePerItemModel)e[i]).Series;
					schPerItemModel.DeleteJobType =	((SchedulePerItemModel)e[i]).DeleteJobType;

					new SchMenuAdManager( systemModel, commonModel).InsertItemSchedule( schPerItemModel );
					if( !schPerItemModel.ResultCD.Equals("0000") )
					{
						throw new Exception(schPerItemModel.ResultDesc);
					}
					Application.DoEvents();
				}

				ReloadSchedule();
				StatusMessage("선택된 편성정보가 추가되었습니다...");
			}
			catch(FrameException fe)
			{
				FrameSystem.showMsgForm("편성정보 삭제오류", new string[] {fe.ErrCode, fe.ResultMsg});
			}
			catch(Exception ex)
			{
				FrameSystem.showMsgForm("편성정보 삭제오류",new string[] {"",ex.Message});
			}			
		}


        private void ResetDetail()
        {	
			btnDelete.Enabled			= false;			
        }


		private void ReloadSchedule()
		{
			SearchItem();
		}


		/// <summary>
		/// 재핑광고 편성대상 광고내역 조회
		/// </summary>
		private void SearchItem()
		{
            IsSearching = true;

			try
			{
				schMenuAdModel.Init();
				schMenuAdDs.AdItems.Clear();
				keyMediaCode = "1";

				if(chkAdState_10.Checked)   schMenuAdModel.SearchchkAdState_10   = "Y";
				if(chkAdState_20.Checked)   schMenuAdModel.SearchchkAdState_20   = "Y";
				if(chkAdState_30.Checked)   schMenuAdModel.SearchchkAdState_30   = "Y";
				if(chkAdState_40.Checked)   schMenuAdModel.SearchchkAdState_40   = "Y";


				// 목록조회 서비스를 호출한다.
				new SchMenuAdManager(systemModel,commonModel).GetContractItemList(schMenuAdModel);

				if (schMenuAdModel.ResultCD.Equals("0000"))
				{
					Utility.SetDataTable(schMenuAdDs.AdItems, schMenuAdModel.MenuAdDataSet);
					SetCurrentRowItem();
				}

				SetDetailTextSchedule();
			}
			catch(FrameException fe)
			{
				FrameSystem.showMsgForm("광고목록 조회오류", new string[] {fe.ErrCode, fe.ResultMsg});
			}
			catch(Exception ex)
			{
				FrameSystem.showMsgForm("광고목록 조회오류",new string[] {"",ex.Message});
			}
            finally
            {
                IsSearching = false; // 조회중 Flag 리셋
            }
		}


		/// <summary>
		/// [S1] 키캆을찾아 그리드 키에 해당되는로우로
		/// </summary>
		private void SetCurrentRowItem()
		{

			int rowIndex = 0;
			if ( dtItem.Rows.Count < 1 ) return;              
			foreach (DataRow row in dtItem.Rows)
			{					
				if(row["ItemNo"].ToString().Equals(keyItemNo))
				{					
					cmItem.Position = rowIndex;
					break;								
				}
				rowIndex++;
			}
			grdExItemList.EnsureVisible();
		}
	

		private void ClearListCheck()
		{

			// 체크된 모든 항목을 클리어
			grdExItemList.UnCheckAllRecords();
			grdExItemList.UpdateData();
				   
			// 데이터 클리어
			for(int i=0;i < dtItem.Rows.Count;i++)
			{
				dtItem.Rows[i].BeginEdit();
				dtItem.Rows[i]["CheckYn"]="False";
				dtItem.Rows[i].EndEdit();
			}
		}

		private void ClearListCheck_Menu()
		{
			// 데이터 클리어
			for(int i=0;i < dtMenu.Rows.Count;i++)
			{
				dtMenu.Rows[i].BeginEdit();
				dtMenu.Rows[i]["CheckYn"]="False";
				dtMenu.Rows[i].EndEdit();
			}
		}

		/// <summary>
		/// [S1] 해당광고의 편성현황을 읽어온다
		/// </summary>
		private void LoadItemSchedule()
		{
			try
			{
				grdExSchList.UnCheckAllRecords();

				schMenuAdModel.Init();
				schMenuAdDs.AdSchedule.Clear();

				schMenuAdModel.ItemNo = keyItemNo;

				// 지정메뉴광고 상세편성내역 조회 서비스를 호출한다.
				new SchMenuAdManager(systemModel,commonModel).GetItemScheduleList(schMenuAdModel);

				if (schMenuAdModel.ResultCD.Equals("0000"))
				{
					Utility.SetDataTable(schMenuAdDs.AdSchedule, schMenuAdModel.ItemScheduleDataSet);
					
					// 삭제버튼 활성화
					if(canDelete && schMenuAdDs.AdSchedule.Rows.Count > 0 )	btnDelete.Enabled = true;
					else													btnDelete.Enabled = false;

					SetCurrentRowGenre();
				}

				// 추가버튼 활성화
				//if(canCreate) btnAddMenu.Enabled    = true;			    			
			}
			catch(FrameException fe)
			{
				FrameSystem.showMsgForm("지정광고 상세편성내역 조회오류", new string[] {fe.ErrCode, fe.ResultMsg});
			}
			catch(Exception ex)
			{
				FrameSystem.showMsgForm("지정광고 상세편성내역 조회오류",new string[] {"",ex.Message});
			}
			
		}

		/// <summary>
		/// 키캆을찾아 그리드 키에 해당되는로우로..
		/// </summary>
		private void SetCurrentRowGenre()		//08.08.01추가
		{

//			int rowIndex = 0;
//			if ( dtSchedule.Rows.Count < 1 ) return;              
//			foreach (DataRow row in dtSchedule.Rows)
//			{					
//				if(row["GenreCode"].ToString().Equals(schGenreCode))
//				{					
//					cmSchedule.Position = rowIndex;
//					break;								
//				}
//				rowIndex++;
//			}
//			grdExGenreList.EnsureVisible();
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

		#region 팝업창을 위한 메소드
			
		public void ReloadList()
		{
			SetDetailTextMenu();
		}

		#endregion

		//private void button1_Click(object sender, System.EventArgs e)
		//{
		//    _30_Schedule.Sch3Form pForm = new AdManagerClient._30_Schedule.Sch3Form();
		//    pForm.ScheduleSelected += new AdManagerClient.Schedule.SchedulePerItemInsertEventHandler(OnScheduleSelected);
		//    pForm.ShowDialog(this);
		//}

		private void OnScheduleSelected(object sender, AdManagerClient.Schedule.SchedulePerItemEventArgs e)
		{
			try
			{
				this.InsertItemSchedule( e.ScheduleData );
			}
			catch(Exception ex)
			{
				throw ex;
			}
		}
	}
}