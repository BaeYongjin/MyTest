// ===============================================================================
// GroupControl for Charites Project
//
// GroupControl.cs
//
// 그룹정보관리 컨드롤을 정의합니다. 
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
 * Class Name: GroupControl
 * 주요기능  : 그룹정보관리 컨트롤
 * 작성자    : 모름
 * 작성일    : 모름
 * -------------------------------------------------------
 * [수정사항]
 * -------------------------------------------------------
 * 수정코드  : [E_01]
 * 수정자    : 김보배
 * 수정일    : 2013.02
 * 수정내용  :        
 *            - 검색어로 조회 안함
 * 수정함수  :
 *            - ebSearchKey_TextChanged
 *            - ebSearchKey_Click
 *            - ebSearchKey_KeyDown
 *            - SearchGroupList 검색어 
 * --------------------------------------------------------
 */
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
	/// 그룹/메뉴별 편성현황 컨트롤
	/// </summary>
	public class GroupControl : System.Windows.Forms.UserControl, IUserControl
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
		GroupModel groupModel = new GroupModel();	// 그룹/메뉴 편성현황 모델
//		SchChoiceAdModel schChoiceAdModel			= new SchChoiceAdModel();		// 지정광고편성모델
//		SchPublishModel schPublishModel				= new SchPublishModel();		// 광고승인모델
		
		// 화면처리용 변수		
		bool IsNewSearchKey		  = true;					// 검색어입력 여부
		CurrencyManager cmChannel     = null;					
		CurrencyManager cmSchedule    = null;							
		DataTable       dtChannel     = null;
		DataTable       dtSchedule    = null;

		// 사용권한
        bool IsSearching = false; // 조회중 상세화면이 업데이트 되는 것을 방지 하기위함 2011.11.29 JH.Park
		bool IsAdding             = false;
		bool canCreate			  = false;
		bool canRead			  = false;
		bool canUpdate			  = false;		
		bool canDelete            = false;

		// Key
		bool IsNotLoading		        = true;					// 상세조회중이 아님
		public string keyMediaCode      = "";
        public string keyCategoryCode   = "";
//        public string keyCategoryName   = "";
        public string keyGenreCode      = "";
		public string keyChannelNo      = "";
		public string keyItemNo         = "";
		public string keyScheduleOrder  = "";

		string groupCode       = "";
        string KeyCategoryCode = "";
        public string KeyCategoryName = "";
        //string KeyCategoryCode1       = "";		
		//string KeyCategoryCode2       = "";
		//string KeyGenreCode       = "";	
		//string KeyGenreCode1       = "";	
		//string KeyChannelNo       = "";	

        bool IsAllCheck = false;

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
		private Janus.Windows.UI.Dock.UIPanelGroup uiPanelChooseAdSchedule;
		private Janus.Windows.GridEX.GridEX grdExChooseAdScheduleList;
		private Janus.Windows.UI.Dock.UIPanel uiPanelSearch;
		private Janus.Windows.UI.Dock.UIPanelInnerContainer uiPanelSearchContainer;
		private System.Windows.Forms.Panel pnlDetail;
		private Janus.Windows.EditControls.UIButton btnDelete;
        private System.Windows.Forms.Panel panel1;
		private Janus.Windows.EditControls.UIButton btnAdd1;
		private Janus.Windows.EditControls.UIButton btnDelete1;
		private Janus.Windows.EditControls.UIButton btnSave;
		private System.Windows.Forms.Label lbUseYn;
		private Janus.Windows.EditControls.UIRadioButton rbUseYn_N;
		private Janus.Windows.EditControls.UIRadioButton rbUseYn_Y;
		private System.Windows.Forms.Label lbGroupName;
		private Janus.Windows.GridEX.EditControls.EditBox ebGroupName;
		private Janus.Windows.GridEX.EditControls.EditBox ebComment;
		private System.Windows.Forms.Label lbComment;
		private Janus.Windows.EditControls.UIButton btnCategory;
		private Janus.Windows.EditControls.UIButton btnGenre;
		private Janus.Windows.EditControls.UIButton btnChannel;
		private System.Data.DataView dvGroup;
		private System.Data.DataView dvGroupDetail;
		private Janus.Windows.UI.Dock.UIPanelGroup uiPanelGroup1;
		private System.Windows.Forms.Panel panel2;
		private AdManagerClient._10_Media._10_Group.GroupDs groupDs;
        private Janus.Windows.EditControls.UIButton btnMap;
		private Janus.Windows.UI.Dock.UIPanel uiPanelGroupList;
		private Janus.Windows.UI.Dock.UIPanelInnerContainer uiPanelGroupListContainer;
		private Janus.Windows.UI.Dock.UIPanel uiPanelGroupDetail;
		private Janus.Windows.UI.Dock.UIPanelInnerContainer uiPanelGroupDetailContainer;
		private Janus.Windows.UI.Dock.UIPanel uiPanelGroupSchedule;
		private Janus.Windows.UI.Dock.UIPanelInnerContainer uiPanelGroupScheduleContainer;
		private Janus.Windows.UI.Dock.UIPanel uiPanelCommand;
		private Janus.Windows.UI.Dock.UIPanelInnerContainer uiPanelCommandContainer;
		private Janus.Windows.GridEX.GridEX grdExScheduleList;
		private Janus.Windows.EditControls.UIButton btnSeries;		
		private System.ComponentModel.IContainer components;

		public GroupControl()
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
            Janus.Windows.GridEX.GridEXLayout grdExChooseAdScheduleList_DesignTimeLayout = new Janus.Windows.GridEX.GridEXLayout();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GroupControl));
            Janus.Windows.GridEX.GridEXLayout grdExScheduleList_DesignTimeLayout = new Janus.Windows.GridEX.GridEXLayout();
            this.uiPM = new Janus.Windows.UI.Dock.UIPanelManager(this.components);
            this.uiPanelUsers = new Janus.Windows.UI.Dock.UIPanelGroup();
            this.uiPanelSearch = new Janus.Windows.UI.Dock.UIPanel();
            this.uiPanelSearchContainer = new Janus.Windows.UI.Dock.UIPanelInnerContainer();
            this.pnlSearch = new System.Windows.Forms.Panel();
            this.cbSearchMedia = new Janus.Windows.EditControls.UIComboBox();
            this.pnlSearchBtn = new System.Windows.Forms.Panel();
            this.btnSearch = new Janus.Windows.EditControls.UIButton();
            this.uiPanelChooseAdSchedule = new Janus.Windows.UI.Dock.UIPanelGroup();
            this.uiPanelGroup1 = new Janus.Windows.UI.Dock.UIPanelGroup();
            this.uiPanelGroupList = new Janus.Windows.UI.Dock.UIPanel();
            this.uiPanelGroupListContainer = new Janus.Windows.UI.Dock.UIPanelInnerContainer();
            this.grdExChooseAdScheduleList = new Janus.Windows.GridEX.GridEX();
            this.dvGroup = new System.Data.DataView();
            this.groupDs = new AdManagerClient._10_Media._10_Group.GroupDs();
            this.uiPanelGroupDetail = new Janus.Windows.UI.Dock.UIPanel();
            this.uiPanelGroupDetailContainer = new Janus.Windows.UI.Dock.UIPanelInnerContainer();
            this.panel2 = new System.Windows.Forms.Panel();
            this.ebComment = new Janus.Windows.GridEX.EditControls.EditBox();
            this.lbComment = new System.Windows.Forms.Label();
            this.ebGroupName = new Janus.Windows.GridEX.EditControls.EditBox();
            this.lbGroupName = new System.Windows.Forms.Label();
            this.lbUseYn = new System.Windows.Forms.Label();
            this.rbUseYn_N = new Janus.Windows.EditControls.UIRadioButton();
            this.rbUseYn_Y = new Janus.Windows.EditControls.UIRadioButton();
            this.btnDelete1 = new Janus.Windows.EditControls.UIButton();
            this.btnSave = new Janus.Windows.EditControls.UIButton();
            this.btnAdd1 = new Janus.Windows.EditControls.UIButton();
            this.uiPanelGroupSchedule = new Janus.Windows.UI.Dock.UIPanel();
            this.uiPanelGroupScheduleContainer = new Janus.Windows.UI.Dock.UIPanelInnerContainer();
            this.grdExScheduleList = new Janus.Windows.GridEX.GridEX();
            this.dvGroupDetail = new System.Data.DataView();
            this.uiPanelCommand = new Janus.Windows.UI.Dock.UIPanel();
            this.uiPanelCommandContainer = new Janus.Windows.UI.Dock.UIPanelInnerContainer();
            this.pnlDetail = new System.Windows.Forms.Panel();
            this.btnSeries = new Janus.Windows.EditControls.UIButton();
            this.btnMap = new Janus.Windows.EditControls.UIButton();
            this.btnDelete = new Janus.Windows.EditControls.UIButton();
            this.btnGenre = new Janus.Windows.EditControls.UIButton();
            this.btnChannel = new Janus.Windows.EditControls.UIButton();
            this.btnCategory = new Janus.Windows.EditControls.UIButton();
            this.panel1 = new System.Windows.Forms.Panel();
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
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelChooseAdSchedule)).BeginInit();
            this.uiPanelChooseAdSchedule.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelGroup1)).BeginInit();
            this.uiPanelGroup1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelGroupList)).BeginInit();
            this.uiPanelGroupList.SuspendLayout();
            this.uiPanelGroupListContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdExChooseAdScheduleList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dvGroup)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupDs)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelGroupDetail)).BeginInit();
            this.uiPanelGroupDetail.SuspendLayout();
            this.uiPanelGroupDetailContainer.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelGroupSchedule)).BeginInit();
            this.uiPanelGroupSchedule.SuspendLayout();
            this.uiPanelGroupScheduleContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdExScheduleList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dvGroupDetail)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelCommand)).BeginInit();
            this.uiPanelCommand.SuspendLayout();
            this.uiPanelCommandContainer.SuspendLayout();
            this.pnlDetail.SuspendLayout();
            this.SuspendLayout();
            // 
            // uiPM
            // 
            this.uiPM.BackColorGradientAutoHideStrip = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(218)))), ((int)(((byte)(250)))));
            this.uiPM.ContainerControl = this;
            this.uiPM.Office2007ColorScheme = Janus.Windows.UI.Office2007ColorScheme.Blue;
            this.uiPM.PanelPadding.Bottom = 2;
            this.uiPM.PanelPadding.Left = 2;
            this.uiPM.PanelPadding.Right = 2;
            this.uiPM.PanelPadding.Top = 2;
            this.uiPM.SplitterSize = 2;
            this.uiPM.VisualStyle = Janus.Windows.UI.Dock.PanelVisualStyle.Office2007;
            this.uiPanelUsers.Id = new System.Guid("b162c53a-7940-47cd-a0ed-9762c59c50d8");
            this.uiPanelUsers.StaticGroup = true;
            this.uiPanelSearch.Id = new System.Guid("afb3b7d0-aa79-443c-a1e8-3baba38f7a4d");
            this.uiPanelUsers.Panels.Add(this.uiPanelSearch);
            this.uiPanelChooseAdSchedule.Id = new System.Guid("88f86479-98e4-43d4-bf03-12fd789fcfc5");
            this.uiPanelChooseAdSchedule.StaticGroup = true;
            this.uiPanelGroup1.Id = new System.Guid("634546b1-9889-4a14-8305-980f7d83400f");
            this.uiPanelGroup1.StaticGroup = true;
            this.uiPanelGroupList.Id = new System.Guid("3d64f731-de32-42ba-a10f-ac3b5ac56703");
            this.uiPanelGroup1.Panels.Add(this.uiPanelGroupList);
            this.uiPanelGroupDetail.Id = new System.Guid("24b67d40-680b-4abe-9636-b08df2bd0a00");
            this.uiPanelGroup1.Panels.Add(this.uiPanelGroupDetail);
            this.uiPanelChooseAdSchedule.Panels.Add(this.uiPanelGroup1);
            this.uiPanelUsers.Panels.Add(this.uiPanelChooseAdSchedule);
            this.uiPanelGroupSchedule.Id = new System.Guid("b2973a57-d3e1-4d80-9387-7a6dc3332542");
            this.uiPanelUsers.Panels.Add(this.uiPanelGroupSchedule);
            this.uiPanelCommand.Id = new System.Guid("c1ac152b-71d8-497e-a36e-0758f080f6ec");
            this.uiPanelUsers.Panels.Add(this.uiPanelCommand);
            this.uiPM.Panels.Add(this.uiPanelUsers);
            // 
            // Design Time Panel Info:
            // 
            this.uiPM.BeginPanelInfo();
            this.uiPM.AddDockPanelInfo(new System.Guid("b162c53a-7940-47cd-a0ed-9762c59c50d8"), Janus.Windows.UI.Dock.PanelGroupStyle.HorizontalTiles, Janus.Windows.UI.Dock.PanelDockStyle.Fill, true, new System.Drawing.Size(1006, 673), true);
            this.uiPM.AddDockPanelInfo(new System.Guid("afb3b7d0-aa79-443c-a1e8-3baba38f7a4d"), new System.Guid("b162c53a-7940-47cd-a0ed-9762c59c50d8"), 42, true);
            this.uiPM.AddDockPanelInfo(new System.Guid("88f86479-98e4-43d4-bf03-12fd789fcfc5"), new System.Guid("b162c53a-7940-47cd-a0ed-9762c59c50d8"), Janus.Windows.UI.Dock.PanelGroupStyle.VerticalTiles, true, 171, true);
            this.uiPM.AddDockPanelInfo(new System.Guid("634546b1-9889-4a14-8305-980f7d83400f"), new System.Guid("88f86479-98e4-43d4-bf03-12fd789fcfc5"), Janus.Windows.UI.Dock.PanelGroupStyle.VerticalTiles, true, 1010, true);
            this.uiPM.AddDockPanelInfo(new System.Guid("3d64f731-de32-42ba-a10f-ac3b5ac56703"), new System.Guid("634546b1-9889-4a14-8305-980f7d83400f"), 447, true);
            this.uiPM.AddDockPanelInfo(new System.Guid("24b67d40-680b-4abe-9636-b08df2bd0a00"), new System.Guid("634546b1-9889-4a14-8305-980f7d83400f"), 400, true);
            this.uiPM.AddDockPanelInfo(new System.Guid("b2973a57-d3e1-4d80-9387-7a6dc3332542"), new System.Guid("b162c53a-7940-47cd-a0ed-9762c59c50d8"), 379, true);
            this.uiPM.AddDockPanelInfo(new System.Guid("c1ac152b-71d8-497e-a36e-0758f080f6ec"), new System.Guid("b162c53a-7940-47cd-a0ed-9762c59c50d8"), 51, true);
            this.uiPM.AddFloatingPanelInfo(new System.Guid("b162c53a-7940-47cd-a0ed-9762c59c50d8"), Janus.Windows.UI.Dock.PanelGroupStyle.HorizontalTiles, true, new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
            this.uiPM.AddFloatingPanelInfo(new System.Guid("8e3140b9-bfcb-43af-b84e-2ee6cddf51dc"), new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
            this.uiPM.AddFloatingPanelInfo(new System.Guid("b2973a57-d3e1-4d80-9387-7a6dc3332542"), new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
            this.uiPM.AddFloatingPanelInfo(new System.Guid("afb3b7d0-aa79-443c-a1e8-3baba38f7a4d"), new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
            this.uiPM.AddFloatingPanelInfo(new System.Guid("88f86479-98e4-43d4-bf03-12fd789fcfc5"), Janus.Windows.UI.Dock.PanelGroupStyle.VerticalTiles, true, new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
            this.uiPM.AddFloatingPanelInfo(new System.Guid("91eeb7fc-856c-4a9c-b37e-db742cfa9b2e"), new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
            this.uiPM.AddFloatingPanelInfo(new System.Guid("3d64f731-de32-42ba-a10f-ac3b5ac56703"), new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
            this.uiPM.AddFloatingPanelInfo(new System.Guid("609188ab-b98f-4466-8472-b8b36f1af6d5"), Janus.Windows.UI.Dock.PanelGroupStyle.HorizontalTiles, true, new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
            this.uiPM.AddFloatingPanelInfo(new System.Guid("c1ac152b-71d8-497e-a36e-0758f080f6ec"), new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
            this.uiPM.AddFloatingPanelInfo(new System.Guid("29ced0ac-906d-4249-8861-8957fec84e41"), new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
            this.uiPM.AddFloatingPanelInfo(new System.Guid("634546b1-9889-4a14-8305-980f7d83400f"), Janus.Windows.UI.Dock.PanelGroupStyle.VerticalTiles, true, new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
            this.uiPM.AddFloatingPanelInfo(new System.Guid("24b67d40-680b-4abe-9636-b08df2bd0a00"), new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
            this.uiPM.EndPanelInfo();
            // 
            // uiPanelUsers
            // 
            this.uiPanelUsers.ActiveCaptionMode = Janus.Windows.UI.Dock.ActiveCaptionMode.Never;
            this.uiPanelUsers.AllowResize = Janus.Windows.UI.InheritableBoolean.True;
            this.uiPanelUsers.BackColor = System.Drawing.SystemColors.Window;
            this.uiPanelUsers.BorderPanel = Janus.Windows.UI.InheritableBoolean.True;
            this.uiPanelUsers.CaptionStyle = Janus.Windows.UI.Dock.PanelCaptionStyle.Dark;
            this.uiPanelUsers.CloseButtonVisible = Janus.Windows.UI.InheritableBoolean.False;
            this.uiPanelUsers.Location = new System.Drawing.Point(2, 2);
            this.uiPanelUsers.Name = "uiPanelUsers";
            this.uiPanelUsers.Size = new System.Drawing.Size(1006, 673);
            this.uiPanelUsers.TabIndex = 4;
            this.uiPanelUsers.Text = "편성그룹 관리";
            // 
            // uiPanelSearch
            // 
            this.uiPanelSearch.AllowResize = Janus.Windows.UI.InheritableBoolean.False;
            this.uiPanelSearch.CaptionVisible = Janus.Windows.UI.InheritableBoolean.False;
            this.uiPanelSearch.CloseButtonVisible = Janus.Windows.UI.InheritableBoolean.False;
            this.uiPanelSearch.InnerContainer = this.uiPanelSearchContainer;
            this.uiPanelSearch.Location = new System.Drawing.Point(1, 23);
            this.uiPanelSearch.Name = "uiPanelSearch";
            this.uiPanelSearch.Size = new System.Drawing.Size(1004, 42);
            this.uiPanelSearch.TabIndex = 4;
            this.uiPanelSearch.Text = "검색";
            // 
            // uiPanelSearchContainer
            // 
            this.uiPanelSearchContainer.Controls.Add(this.pnlSearch);
            this.uiPanelSearchContainer.Location = new System.Drawing.Point(0, 1);
            this.uiPanelSearchContainer.Name = "uiPanelSearchContainer";
            this.uiPanelSearchContainer.Size = new System.Drawing.Size(1004, 41);
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
            this.pnlSearch.Size = new System.Drawing.Size(1004, 41);
            this.pnlSearch.TabIndex = 3;
            // 
            // cbSearchMedia
            // 
            this.cbSearchMedia.ComboStyle = Janus.Windows.EditControls.ComboStyle.DropDownList;
            this.cbSearchMedia.Location = new System.Drawing.Point(8, 8);
            this.cbSearchMedia.Name = "cbSearchMedia";
            this.cbSearchMedia.Size = new System.Drawing.Size(152, 21);
            this.cbSearchMedia.TabIndex = 34;
            this.cbSearchMedia.Text = "매체선택";
            this.cbSearchMedia.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            // 
            // pnlSearchBtn
            // 
            this.pnlSearchBtn.BackColor = System.Drawing.SystemColors.Window;
            this.pnlSearchBtn.Controls.Add(this.btnSearch);
            this.pnlSearchBtn.Dock = System.Windows.Forms.DockStyle.Right;
            this.pnlSearchBtn.Location = new System.Drawing.Point(884, 0);
            this.pnlSearchBtn.Name = "pnlSearchBtn";
            this.pnlSearchBtn.Size = new System.Drawing.Size(120, 41);
            this.pnlSearchBtn.TabIndex = 5;
            // 
            // btnSearch
            // 
            this.btnSearch.Enabled = false;
            this.btnSearch.Location = new System.Drawing.Point(16, 8);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(90, 24);
            this.btnSearch.TabIndex = 3;
            this.btnSearch.Text = "조 회";
            this.btnSearch.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // uiPanelChooseAdSchedule
            // 
            this.uiPanelChooseAdSchedule.AllowResize = Janus.Windows.UI.InheritableBoolean.True;
            this.uiPanelChooseAdSchedule.BorderCaption = Janus.Windows.UI.InheritableBoolean.False;
            this.uiPanelChooseAdSchedule.BorderPanel = Janus.Windows.UI.InheritableBoolean.True;
            this.uiPanelChooseAdSchedule.CaptionHeight = 20;
            this.uiPanelChooseAdSchedule.CaptionStyle = Janus.Windows.UI.Dock.PanelCaptionStyle.Dark;
            this.uiPanelChooseAdSchedule.CaptionVisible = Janus.Windows.UI.InheritableBoolean.False;
            this.uiPanelChooseAdSchedule.GroupStyle = Janus.Windows.UI.Dock.PanelGroupStyle.VerticalTiles;
            this.uiPanelChooseAdSchedule.Location = new System.Drawing.Point(1, 67);
            this.uiPanelChooseAdSchedule.Name = "uiPanelChooseAdSchedule";
            this.uiPanelChooseAdSchedule.Size = new System.Drawing.Size(1004, 171);
            this.uiPanelChooseAdSchedule.TabIndex = 4;
            this.uiPanelChooseAdSchedule.Text = "그룹목록";
            // 
            // uiPanelGroup1
            // 
            this.uiPanelGroup1.CaptionHeight = 0;
            this.uiPanelGroup1.GroupStyle = Janus.Windows.UI.Dock.PanelGroupStyle.VerticalTiles;
            this.uiPanelGroup1.Location = new System.Drawing.Point(1, 1);
            this.uiPanelGroup1.Name = "uiPanelGroup1";
            this.uiPanelGroup1.Size = new System.Drawing.Size(1002, 169);
            this.uiPanelGroup1.TabIndex = 0;
            this.uiPanelGroup1.Visible = false;
            // 
            // uiPanelGroupList
            // 
            this.uiPanelGroupList.CaptionStyle = Janus.Windows.UI.Dock.PanelCaptionStyle.Light;
            this.uiPanelGroupList.CaptionVisible = Janus.Windows.UI.InheritableBoolean.False;
            this.uiPanelGroupList.InnerContainer = this.uiPanelGroupListContainer;
            this.uiPanelGroupList.Location = new System.Drawing.Point(0, 0);
            this.uiPanelGroupList.Name = "uiPanelGroupList";
            this.uiPanelGroupList.Size = new System.Drawing.Size(528, 169);
            this.uiPanelGroupList.TabIndex = 4;
            this.uiPanelGroupList.Text = "편성그룹 목록";
            // 
            // uiPanelGroupListContainer
            // 
            this.uiPanelGroupListContainer.Controls.Add(this.grdExChooseAdScheduleList);
            this.uiPanelGroupListContainer.Location = new System.Drawing.Point(1, 1);
            this.uiPanelGroupListContainer.Name = "uiPanelGroupListContainer";
            this.uiPanelGroupListContainer.Size = new System.Drawing.Size(526, 167);
            this.uiPanelGroupListContainer.TabIndex = 0;
            // 
            // grdExChooseAdScheduleList
            // 
            this.grdExChooseAdScheduleList.AllowEdit = Janus.Windows.GridEX.InheritableBoolean.False;
            this.grdExChooseAdScheduleList.AlternatingColors = true;
            this.grdExChooseAdScheduleList.BorderStyle = Janus.Windows.GridEX.BorderStyle.None;
            this.grdExChooseAdScheduleList.DataSource = this.dvGroup;
            grdExChooseAdScheduleList_DesignTimeLayout.LayoutString = resources.GetString("grdExChooseAdScheduleList_DesignTimeLayout.LayoutString");
            this.grdExChooseAdScheduleList.DesignTimeLayout = grdExChooseAdScheduleList_DesignTimeLayout;
            this.grdExChooseAdScheduleList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdExChooseAdScheduleList.EditorsControlStyle.ButtonAppearance = Janus.Windows.GridEX.ButtonAppearance.Regular;
            this.grdExChooseAdScheduleList.EmptyRows = true;
            this.grdExChooseAdScheduleList.FocusCellFormatStyle.BackColor = System.Drawing.SystemColors.Highlight;
            this.grdExChooseAdScheduleList.FocusCellFormatStyle.ForeColor = System.Drawing.Color.White;
            this.grdExChooseAdScheduleList.FocusStyle = Janus.Windows.GridEX.FocusStyle.None;
            this.grdExChooseAdScheduleList.GridLineColor = System.Drawing.Color.Silver;
            this.grdExChooseAdScheduleList.GridLines = Janus.Windows.GridEX.GridLines.Vertical;
            this.grdExChooseAdScheduleList.GridLineStyle = Janus.Windows.GridEX.GridLineStyle.Solid;
            this.grdExChooseAdScheduleList.GroupByBoxVisible = false;
            this.grdExChooseAdScheduleList.HeaderFormatStyle.TextAlignment = Janus.Windows.GridEX.TextAlignment.Center;
            this.grdExChooseAdScheduleList.Location = new System.Drawing.Point(0, 0);
            this.grdExChooseAdScheduleList.Name = "grdExChooseAdScheduleList";
            this.grdExChooseAdScheduleList.ScrollBars = Janus.Windows.GridEX.ScrollBars.Vertical;
            this.grdExChooseAdScheduleList.SelectedInactiveFormatStyle.BackColor = System.Drawing.Color.Gold;
            this.grdExChooseAdScheduleList.Size = new System.Drawing.Size(526, 167);
            this.grdExChooseAdScheduleList.TabIndex = 8;
            this.grdExChooseAdScheduleList.ThemedAreas = ((Janus.Windows.GridEX.ThemedArea)(((((((((Janus.Windows.GridEX.ThemedArea.ScrollBars | Janus.Windows.GridEX.ThemedArea.EditControls) 
            | Janus.Windows.GridEX.ThemedArea.Headers) 
            | Janus.Windows.GridEX.ThemedArea.GroupByBox) 
            | Janus.Windows.GridEX.ThemedArea.GroupRows) 
            | Janus.Windows.GridEX.ThemedArea.ControlBorder) 
            | Janus.Windows.GridEX.ThemedArea.Cards) 
            | Janus.Windows.GridEX.ThemedArea.Gridlines) 
            | Janus.Windows.GridEX.ThemedArea.CheckBoxes)));
            this.grdExChooseAdScheduleList.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            this.grdExChooseAdScheduleList.SelectionChanged += new System.EventHandler(this.OnGrdRowChangedChannel);
            this.grdExChooseAdScheduleList.Enter += new System.EventHandler(this.OnGrdRowChangedChannel);
            // 
            // dvGroup
            // 
            this.dvGroup.Table = this.groupDs.Group;
            // 
            // groupDs
            // 
            this.groupDs.DataSetName = "GroupDs";
            this.groupDs.Locale = new System.Globalization.CultureInfo("en-US");
            this.groupDs.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // uiPanelGroupDetail
            // 
            this.uiPanelGroupDetail.CaptionStyle = Janus.Windows.UI.Dock.PanelCaptionStyle.Light;
            this.uiPanelGroupDetail.CaptionVisible = Janus.Windows.UI.InheritableBoolean.False;
            this.uiPanelGroupDetail.InnerContainer = this.uiPanelGroupDetailContainer;
            this.uiPanelGroupDetail.Location = new System.Drawing.Point(530, 0);
            this.uiPanelGroupDetail.Name = "uiPanelGroupDetail";
            this.uiPanelGroupDetail.Size = new System.Drawing.Size(472, 169);
            this.uiPanelGroupDetail.TabIndex = 4;
            this.uiPanelGroupDetail.Text = "편성그룹 내역";
            // 
            // uiPanelGroupDetailContainer
            // 
            this.uiPanelGroupDetailContainer.Controls.Add(this.panel2);
            this.uiPanelGroupDetailContainer.Location = new System.Drawing.Point(1, 1);
            this.uiPanelGroupDetailContainer.Name = "uiPanelGroupDetailContainer";
            this.uiPanelGroupDetailContainer.Size = new System.Drawing.Size(470, 167);
            this.uiPanelGroupDetailContainer.TabIndex = 0;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.ebComment);
            this.panel2.Controls.Add(this.lbComment);
            this.panel2.Controls.Add(this.ebGroupName);
            this.panel2.Controls.Add(this.lbGroupName);
            this.panel2.Controls.Add(this.lbUseYn);
            this.panel2.Controls.Add(this.rbUseYn_N);
            this.panel2.Controls.Add(this.rbUseYn_Y);
            this.panel2.Controls.Add(this.btnDelete1);
            this.panel2.Controls.Add(this.btnSave);
            this.panel2.Controls.Add(this.btnAdd1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(470, 167);
            this.panel2.TabIndex = 0;
            // 
            // ebComment
            // 
            this.ebComment.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.ebComment.Location = new System.Drawing.Point(56, 54);
            this.ebComment.Multiline = true;
            this.ebComment.Name = "ebComment";
            this.ebComment.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.ebComment.Size = new System.Drawing.Size(295, 56);
            this.ebComment.TabIndex = 22;
            this.ebComment.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
            this.ebComment.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2003;
            // 
            // lbComment
            // 
            this.lbComment.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lbComment.Location = new System.Drawing.Point(8, 54);
            this.lbComment.Name = "lbComment";
            this.lbComment.Size = new System.Drawing.Size(40, 21);
            this.lbComment.TabIndex = 23;
            this.lbComment.Text = "비고";
            this.lbComment.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ebGroupName
            // 
            this.ebGroupName.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.ebGroupName.Location = new System.Drawing.Point(56, 30);
            this.ebGroupName.MaxLength = 200;
            this.ebGroupName.Name = "ebGroupName";
            this.ebGroupName.Size = new System.Drawing.Size(295, 21);
            this.ebGroupName.TabIndex = 12;
            this.ebGroupName.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
            this.ebGroupName.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2003;
            // 
            // lbGroupName
            // 
            this.lbGroupName.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lbGroupName.BackColor = System.Drawing.SystemColors.Window;
            this.lbGroupName.Location = new System.Drawing.Point(8, 30);
            this.lbGroupName.Name = "lbGroupName";
            this.lbGroupName.Size = new System.Drawing.Size(48, 21);
            this.lbGroupName.TabIndex = 13;
            this.lbGroupName.Text = "그룹명";
            this.lbGroupName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lbUseYn
            // 
            this.lbUseYn.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lbUseYn.Location = new System.Drawing.Point(8, 118);
            this.lbUseYn.Name = "lbUseYn";
            this.lbUseYn.Size = new System.Drawing.Size(72, 21);
            this.lbUseYn.TabIndex = 43;
            this.lbUseYn.Text = "사용여부";
            this.lbUseYn.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // rbUseYn_N
            // 
            this.rbUseYn_N.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.rbUseYn_N.BackColor = System.Drawing.Color.Transparent;
            this.rbUseYn_N.ForeColor = System.Drawing.Color.OrangeRed;
            this.rbUseYn_N.Location = new System.Drawing.Point(160, 118);
            this.rbUseYn_N.Name = "rbUseYn_N";
            this.rbUseYn_N.Size = new System.Drawing.Size(72, 23);
            this.rbUseYn_N.TabIndex = 42;
            this.rbUseYn_N.Text = "사용안함";
            // 
            // rbUseYn_Y
            // 
            this.rbUseYn_Y.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.rbUseYn_Y.BackColor = System.Drawing.Color.Transparent;
            this.rbUseYn_Y.Location = new System.Drawing.Point(80, 118);
            this.rbUseYn_Y.Name = "rbUseYn_Y";
            this.rbUseYn_Y.Size = new System.Drawing.Size(72, 23);
            this.rbUseYn_Y.TabIndex = 41;
            this.rbUseYn_Y.Text = "사용함";
            // 
            // btnDelete1
            // 
            this.btnDelete1.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.btnDelete1.Enabled = false;
            this.btnDelete1.Location = new System.Drawing.Point(368, 57);
            this.btnDelete1.Name = "btnDelete1";
            this.btnDelete1.Size = new System.Drawing.Size(90, 24);
            this.btnDelete1.TabIndex = 20;
            this.btnDelete1.Text = "삭 제";
            this.btnDelete1.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.btnDelete1.Click += new System.EventHandler(this.btnDelete1_Click);
            // 
            // btnSave
            // 
            this.btnSave.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.btnSave.Enabled = false;
            this.btnSave.Location = new System.Drawing.Point(368, 89);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(90, 24);
            this.btnSave.TabIndex = 21;
            this.btnSave.Text = "저 장";
            this.btnSave.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnAdd1
            // 
            this.btnAdd1.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.btnAdd1.Enabled = false;
            this.btnAdd1.Location = new System.Drawing.Point(368, 25);
            this.btnAdd1.Name = "btnAdd1";
            this.btnAdd1.Size = new System.Drawing.Size(90, 24);
            this.btnAdd1.TabIndex = 19;
            this.btnAdd1.Text = "추 가";
            this.btnAdd1.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.btnAdd1.Click += new System.EventHandler(this.btnAdd1_Click);
            // 
            // uiPanelGroupSchedule
            // 
            this.uiPanelGroupSchedule.ActiveCaptionMode = Janus.Windows.UI.Dock.ActiveCaptionMode.Never;
            this.uiPanelGroupSchedule.AllowResize = Janus.Windows.UI.InheritableBoolean.True;
            this.uiPanelGroupSchedule.BackColor = System.Drawing.SystemColors.Window;
            this.uiPanelGroupSchedule.CaptionHeight = 20;
            this.uiPanelGroupSchedule.CaptionStyle = Janus.Windows.UI.Dock.PanelCaptionStyle.Dark;
            this.uiPanelGroupSchedule.CaptionVisible = Janus.Windows.UI.InheritableBoolean.False;
            this.uiPanelGroupSchedule.InnerContainer = this.uiPanelGroupScheduleContainer;
            this.uiPanelGroupSchedule.Location = new System.Drawing.Point(1, 240);
            this.uiPanelGroupSchedule.Name = "uiPanelGroupSchedule";
            this.uiPanelGroupSchedule.Size = new System.Drawing.Size(1004, 379);
            this.uiPanelGroupSchedule.TabIndex = 4;
            this.uiPanelGroupSchedule.Text = "그룹상세목록";
            // 
            // uiPanelGroupScheduleContainer
            // 
            this.uiPanelGroupScheduleContainer.Controls.Add(this.grdExScheduleList);
            this.uiPanelGroupScheduleContainer.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.uiPanelGroupScheduleContainer.Location = new System.Drawing.Point(1, 0);
            this.uiPanelGroupScheduleContainer.Name = "uiPanelGroupScheduleContainer";
            this.uiPanelGroupScheduleContainer.Size = new System.Drawing.Size(1002, 378);
            this.uiPanelGroupScheduleContainer.TabIndex = 0;
            // 
            // grdExScheduleList
            // 
            this.grdExScheduleList.AlternatingColors = true;
            this.grdExScheduleList.BorderStyle = Janus.Windows.GridEX.BorderStyle.None;
            this.grdExScheduleList.CellSelectionMode = Janus.Windows.GridEX.CellSelectionMode.SingleCell;
            this.grdExScheduleList.DataSource = this.dvGroupDetail;
            grdExScheduleList_DesignTimeLayout.LayoutString = resources.GetString("grdExScheduleList_DesignTimeLayout.LayoutString");
            this.grdExScheduleList.DesignTimeLayout = grdExScheduleList_DesignTimeLayout;
            this.grdExScheduleList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdExScheduleList.EmptyRows = true;
            this.grdExScheduleList.FocusCellFormatStyle.BackColor = System.Drawing.SystemColors.Highlight;
            this.grdExScheduleList.FocusStyle = Janus.Windows.GridEX.FocusStyle.None;
            this.grdExScheduleList.Font = new System.Drawing.Font("나눔고딕", 8.5F);
            this.grdExScheduleList.FrozenColumns = 2;
            this.grdExScheduleList.GridLineColor = System.Drawing.Color.Silver;
            this.grdExScheduleList.GridLines = Janus.Windows.GridEX.GridLines.Vertical;
            this.grdExScheduleList.GridLineStyle = Janus.Windows.GridEX.GridLineStyle.Solid;
            this.grdExScheduleList.GroupByBoxVisible = false;
            this.grdExScheduleList.Location = new System.Drawing.Point(0, 0);
            this.grdExScheduleList.Name = "grdExScheduleList";
            this.grdExScheduleList.ScrollBars = Janus.Windows.GridEX.ScrollBars.Vertical;
            this.grdExScheduleList.SelectedInactiveFormatStyle.BackColor = System.Drawing.Color.DarkOrange;
            this.grdExScheduleList.Size = new System.Drawing.Size(1002, 378);
            this.grdExScheduleList.TabIndex = 10;
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
            this.grdExScheduleList.ColumnHeaderClick += new Janus.Windows.GridEX.ColumnActionEventHandler(this.grdExScheduleList_ColumnHeaderClick_1);
            // 
            // dvGroupDetail
            // 
            this.dvGroupDetail.Table = this.groupDs.AdSchedule;
            // 
            // uiPanelCommand
            // 
            this.uiPanelCommand.AllowResize = Janus.Windows.UI.InheritableBoolean.False;
            this.uiPanelCommand.CaptionStyle = Janus.Windows.UI.Dock.PanelCaptionStyle.Dark;
            this.uiPanelCommand.CaptionVisible = Janus.Windows.UI.InheritableBoolean.False;
            this.uiPanelCommand.InnerContainer = this.uiPanelCommandContainer;
            this.uiPanelCommand.Location = new System.Drawing.Point(1, 621);
            this.uiPanelCommand.Name = "uiPanelCommand";
            this.uiPanelCommand.Size = new System.Drawing.Size(1004, 51);
            this.uiPanelCommand.TabIndex = 4;
            this.uiPanelCommand.Text = "명령창";
            // 
            // uiPanelCommandContainer
            // 
            this.uiPanelCommandContainer.Controls.Add(this.pnlDetail);
            this.uiPanelCommandContainer.Location = new System.Drawing.Point(0, 1);
            this.uiPanelCommandContainer.Name = "uiPanelCommandContainer";
            this.uiPanelCommandContainer.Size = new System.Drawing.Size(1004, 50);
            this.uiPanelCommandContainer.TabIndex = 0;
            // 
            // pnlDetail
            // 
            this.pnlDetail.BackColor = System.Drawing.SystemColors.Window;
            this.pnlDetail.Controls.Add(this.btnSeries);
            this.pnlDetail.Controls.Add(this.btnMap);
            this.pnlDetail.Controls.Add(this.btnDelete);
            this.pnlDetail.Controls.Add(this.btnGenre);
            this.pnlDetail.Controls.Add(this.btnChannel);
            this.pnlDetail.Controls.Add(this.btnCategory);
            this.pnlDetail.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlDetail.Location = new System.Drawing.Point(0, 0);
            this.pnlDetail.Name = "pnlDetail";
            this.pnlDetail.Size = new System.Drawing.Size(1004, 50);
            this.pnlDetail.TabIndex = 4;
            // 
            // btnSeries
            // 
            this.btnSeries.Enabled = false;
            this.btnSeries.Location = new System.Drawing.Point(308, 14);
            this.btnSeries.Name = "btnSeries";
            this.btnSeries.Size = new System.Drawing.Size(90, 24);
            this.btnSeries.TabIndex = 44;
            this.btnSeries.Text = "회차";
            this.btnSeries.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.btnSeries.Click += new System.EventHandler(this.btnSeries_Click);
            // 
            // btnMap
            // 
            this.btnMap.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnMap.Location = new System.Drawing.Point(776, 15);
            this.btnMap.Name = "btnMap";
            this.btnMap.Size = new System.Drawing.Size(104, 24);
            this.btnMap.TabIndex = 43;
            this.btnMap.Text = "설정현황";
            this.btnMap.Visible = false;
            this.btnMap.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.btnMap.Click += new System.EventHandler(this.btnMap_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnDelete.Location = new System.Drawing.Point(888, 15);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(104, 24);
            this.btnDelete.TabIndex = 20;
            this.btnDelete.Text = "삭 제";
            this.btnDelete.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnGenre
            // 
            this.btnGenre.Enabled = false;
            this.btnGenre.Location = new System.Drawing.Point(112, 14);
            this.btnGenre.Name = "btnGenre";
            this.btnGenre.Size = new System.Drawing.Size(90, 24);
            this.btnGenre.TabIndex = 16;
            this.btnGenre.Text = "2차메뉴";
            this.btnGenre.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.btnGenre.Click += new System.EventHandler(this.btnGenre_Click);
            // 
            // btnChannel
            // 
            this.btnChannel.Enabled = false;
            this.btnChannel.Location = new System.Drawing.Point(210, 14);
            this.btnChannel.Name = "btnChannel";
            this.btnChannel.Size = new System.Drawing.Size(90, 24);
            this.btnChannel.TabIndex = 16;
            this.btnChannel.Text = "프로그램";
            this.btnChannel.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.btnChannel.Click += new System.EventHandler(this.btnChannel_Click);
            // 
            // btnCategory
            // 
            this.btnCategory.Enabled = false;
            this.btnCategory.Location = new System.Drawing.Point(14, 14);
            this.btnCategory.Name = "btnCategory";
            this.btnCategory.Size = new System.Drawing.Size(90, 24);
            this.btnCategory.TabIndex = 16;
            this.btnCategory.Text = "1차메뉴";
            this.btnCategory.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.btnCategory.Click += new System.EventHandler(this.btnCategory_Click);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.Window;
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(851, 118);
            this.panel1.TabIndex = 5;
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
            // GroupControl
            // 
            this.Controls.Add(this.uiPanelUsers);
            this.Font = global::AdManagerClient.Properties.Settings.Default.Font1;
            this.Name = "GroupControl";
            this.Size = new System.Drawing.Size(1010, 677);
            this.Load += new System.EventHandler(this.GroupControl_Load);
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
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelGroup1)).EndInit();
            this.uiPanelGroup1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelGroupList)).EndInit();
            this.uiPanelGroupList.ResumeLayout(false);
            this.uiPanelGroupListContainer.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdExChooseAdScheduleList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dvGroup)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupDs)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelGroupDetail)).EndInit();
            this.uiPanelGroupDetail.ResumeLayout(false);
            this.uiPanelGroupDetailContainer.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelGroupSchedule)).EndInit();
            this.uiPanelGroupSchedule.ResumeLayout(false);
            this.uiPanelGroupScheduleContainer.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdExScheduleList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dvGroupDetail)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelCommand)).EndInit();
            this.uiPanelCommand.ResumeLayout(false);
            this.uiPanelCommandContainer.ResumeLayout(false);
            this.pnlDetail.ResumeLayout(false);
            this.ResumeLayout(false);

        }
		#endregion

		#region 컨트롤 로드
		private void GroupControl_Load(object sender, System.EventArgs e)
		{
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
				ResetTextReadonly();
				canUpdate = true;
			}

			// 삭제권한 검사
			if(menu.CanDelete(MenuCode))
			{
				canDelete = true;
			}

			InitButton();

			ProgressStop();
									
			if(canRead) SearchGroupList();
		}	

		private void InitCombo_Media()
		{			
			MediaCodeModel mediacodeModel = new MediaCodeModel();		
			new MediaCodeManager(systemModel, commonModel).GetMediaCodeList(mediacodeModel);
			
			if (mediacodeModel.ResultCD.Equals("0000"))
			{
				// 데이터셋에 셋팅
				Utility.SetDataTable(groupDs.Medias, mediacodeModel.MediaCodeDataSet);				
			}

			// 상세조회 콤보
			// 상세정보의 콤보는 Dataset을 데이터소스로 가진다.

			// 검색조건의 콤보
			this.cbSearchMedia.Items.Clear();
			
			// 콤보박스에 셋트할 코드목록을 담을 Item배열을 선언
			Janus.Windows.EditControls.UIComboBoxItem[] comboItems = new Janus.Windows.EditControls.UIComboBoxItem[mediacodeModel.ResultCnt + 1];

			comboItems[0] = new Janus.Windows.EditControls.UIComboBoxItem("매체선택","00");
			
			for(int i=0;i<mediacodeModel.ResultCnt;i++)
			{
				DataRow row = groupDs.Medias.Rows[i];

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
				for(int i=0;i < groupDs.Medias.Rows.Count;i++)
				{
					DataRow row = groupDs.Medias.Rows[i];					
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
			if(canRead)   btnSearch.Enabled = true;
			if(canCreate) btnAdd1.Enabled    = true;

			if(ebGroupName.Text.Trim().Length > 0) 
			{
				if(canDelete) btnDelete1.Enabled = true;
				if(canUpdate) btnSave.Enabled   = true;
			}
			btnCategory.Enabled = true;
			btnGenre.Enabled	= true;
			btnChannel.Enabled	= true;
			btnSeries.Enabled	= true;

			int curRow = cmSchedule.Position;

			if(curRow >= 0 )
			{
				btnDelete.Enabled = true;
			}
			Application.DoEvents();
		}

		private void DisableButton()
		{
			btnSearch.Enabled	= false;
			btnDelete.Enabled	= false;
			btnSave.Enabled		= false;
			btnAdd1.Enabled		= false;
			btnDelete1.Enabled	= false;
			Application.DoEvents();
		}

		#endregion

		#region 사용자 액션처리 메소드

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
                    InitButton();
                }
            }
		}

		
		private void OnGrdRowChangedChannel(object sender, System.EventArgs e) 
		{
            if (!IsSearching) // 2011.11.29 JH.Park 조회중이 아닐경우에만 동작하도록 변경
            {
                if (IsNotLoading)
                {
                    SetGroupDetailText();
                    InitButton();
                }
            }
		}

		private void OnGrdRowChangedSchedule(object sender, System.EventArgs e) 
		{		
			//SetGroupDetailButton();
		}

		/// <summary>
        /// [E_01]검색어 변경
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
        //private void ebSearchKey_TextChanged(object sender, System.EventArgs e)
        //{
        //    IsNewSearchKey = false;
        //} 

		/// <summary>
        /// [E_01]검색어 클릭 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
        //private void ebSearchKey_Click(object sender, System.EventArgs e)
        //{
        //    if(IsNewSearchKey)
        //    {
        //        ebSearchKey.Text = "";
        //    }
        //    else
        //    {
        //        ebSearchKey.SelectAll();
        //    }
        //}

        //[E_01]
        //private void ebSearchKey_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        //{
        //    if(e.KeyCode == Keys.Enter)
        //    {
        //        SearchGroupList();
        //    }
        //}

		/// <summary>
		/// 조회버튼 클릭
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnSearch_Click(object sender, System.EventArgs e)
		{			
			ResetDetail();
			DisableButton();
			SearchGroupList();			
			InitButton();
		}

		private void btnAdd1_Click(object sender, System.EventArgs e)
		{
			btnAdd1.Enabled    = false;
			btnDelete.Enabled = false;
			btnSave.Enabled   = true;

			IsAdding = true;	
			ResetTextReadonly();
			ResetGroupText();

			ebGroupName.Focus();
		}

		private void btnSave_Click(object sender, System.EventArgs e)
		{
			//			그룹 비고 저장
			SaveGroup();			
		}

		private void btnDelete_Click(object sender, System.EventArgs e)
		{			
			DeleteGroupDetail();			
		}

		private void btnDelete1_Click(object sender, System.EventArgs e)
		{		
			DeleteGroup();				
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
		/// 그룹목록 조회
		/// </summary>
		private void SearchGroupList()
		{
            IsSearching = true;

			StatusMessage("그룹목록을 조회합니다.");

			try
			{
                // [E_01] 수정
                //if(IsNewSearchKey)
                //{
                //    groupModel.SearchKey = "";
                //}
                //else
                //{
                //    groupModel.SearchKey  = ebSearchKey.Text;
                //}

				groupModel.Init();
				keyMediaCode			  = cbSearchMedia.SelectedValue.ToString();
				groupModel.SearchMedia    = cbSearchMedia.SelectedValue.ToString();
				
				// 그룹목록조회 서비스를 호출한다.
				new GroupManager(systemModel,commonModel).GetGroupList(groupModel);

				if (groupModel.ResultCD.Equals("0000"))
				{
					Utility.SetDataTable(groupDs.Group, groupModel.GroupDataSet);				
					StatusMessage(groupModel.ResultCnt + "건의 그룹 정보가 조회되었습니다.");
				}     
				SetGroupDetailText();
				int curRow = cmChannel.Position;
				if(canCreate) btnAdd1.Enabled = true;
				if(curRow >= 0 )
				{
					if(canCreate) btnAdd1.Enabled = true;
					if(canUpdate) btnSave.Enabled = true;
					if(canDelete) btnDelete1.Enabled = true;										
				}
			}
			catch(FrameException fe)
			{
				FrameSystem.showMsgForm("그룹조회오류", new string[] {fe.ErrCode, fe.ResultMsg});
			}
			catch(Exception ex)
			{
				FrameSystem.showMsgForm("그룹조회오류",new string[] {"",ex.Message});
			}
            finally
            {
                IsSearching = false; // 조회중 Flag 리셋
            }
		}
       
		//그룹상세조회
		private void SearchGroupDetail()
		{
			StatusMessage("그룹상세 조회합니다.");

			try
			{
				grdExScheduleList.UnCheckAllRecords();
				groupDs.AdSchedule.Clear();

				// 데이터모델 초기화
				groupModel.Init();

				// 데이터모델에 전송할 내용을 셋트한다.
     
				groupModel.SearchGroup		=  groupCode;   

				// 광고파일목록조회 서비스를 호출한다.
				new GroupManager(systemModel,commonModel).GetGroupDetailList(groupModel);

				if (groupModel.ResultCD.Equals("0000"))
				{
					Utility.SetDataTable(groupDs.AdSchedule, groupModel.GroupDetailDataSet);		
					StatusMessage(groupModel.ResultCnt + "건의 광고파일 정보가 조회되었습니다.");
					//SetCurrentRowSchedule();
				}
			}
			catch(FrameException fe)
			{
				FrameSystem.showMsgForm("그룹상세 조회오류", new string[] {fe.ErrCode, fe.ResultMsg});
			}
			catch(Exception ex)
			{
				FrameSystem.showMsgForm("그룹상세 조회오류",new string[] {"",ex.Message});
			}

		}        
		

		/// <summary>
		///  그룹비고저장
		/// </summary>
		private void SaveGroup()
		{
			StatusMessage("그룹 정보를 저장합니다.");
			
			if(ebGroupName.Text.Trim().Length == 0) 
			{
				MessageBox.Show("그룹명이 입력되지 않았습니다.","그룹 저장", 
					MessageBoxButtons.OK, MessageBoxIcon.Information );
				ebComment.Focus();
				return;				
			}

			try
			{
				groupModel.MediaCode = keyMediaCode;
				int curRow = cmChannel.Position;
				if(curRow >= 0)
				{					
					groupModel.GroupCode = dtChannel.Rows[curRow]["GroupCode"].ToString();
				}
					groupModel.GroupName  =  ebGroupName.Text;
					groupModel.Comment  =  ebComment.Text;
					//사용여부
					if(rbUseYn_Y.Checked)
					{
						groupModel.UseYn       = "Y";
					}
					else
					{
						groupModel.UseYn       = "N";
					}

				if (IsAdding)
				{
					new GroupManager(systemModel,commonModel).SetGroupAdd(groupModel);
					StatusMessage("그룹 정보가 추가되었습니다.");
					IsAdding = false;
					ResetGroupText();
				}
				else
				{
					new GroupManager(systemModel,commonModel).SetGroupUpdate(groupModel);			
				}				

				DisableButton();
				SearchGroupList();
				InitButton();

			}
			catch(FrameException fe)
			{
				FrameSystem.showMsgForm("그룹 비고 저장오류", new string[] {fe.ErrCode, fe.ResultMsg});
			}
			catch(Exception ex)
			{
				FrameSystem.showMsgForm("그룹 비고 저장오류",new string[] {"",ex.Message});
			}			
		}

		/// <summary>
		/// 선택된 그룹을 삭제
		/// </summary>
		private void DeleteGroup()
		{
			StatusMessage("그룹을 삭제합니다.");

			DialogResult result = MessageBox.Show("그룹을 삭제 하시겠습니까?","그룹 삭제",
				MessageBoxButtons.YesNo, MessageBoxIcon.Question,
				MessageBoxDefaultButton.Button2);

			if (result == DialogResult.No) return;

			// 그리드에 변경된 데이터를 Datasource에 업데이트 한다.
			grdExChooseAdScheduleList.UpdateData();

			try
			{
				int SetCount = 0;

				// 삭제 시킴				
								
				groupModel.Init();		

				groupModel.MediaCode = keyMediaCode;			
				int curRow = cmChannel.Position;
				if(curRow >= 0)
				{					
					groupModel.GroupCode = dtChannel.Rows[curRow]["GroupCode"].ToString();
					
					new GroupManager(systemModel,commonModel).SetGroupDelete(groupModel);
				
					if (groupModel.ResultCD.Equals("0000"))
					{
						SetCount++;
					}	
				}			

				if(SetCount > 0)
				{
					ReloadList();
					StatusMessage("그룹이 삭제되었습니다.");			
					SearchGroupList();
				}	
				else
				{
					MessageBox.Show("선택된 그룹이 없습니다.", "그룹 삭제",
						MessageBoxButtons.OK, MessageBoxIcon.Information);
				}	
			}
			catch(FrameException fe)
			{
				FrameSystem.showMsgForm("그룹 삭제오류", new string[] {fe.ErrCode, fe.ResultMsg});
			}
			catch(Exception ex)
			{
				FrameSystem.showMsgForm("그룹 삭제오류",new string[] {"",ex.Message});
			}			
		}

		/// <summary>
		/// 선택된 지정그룹광고 상세내역을 삭제
		/// </summary>
		private void DeleteGroupDetail()
		{
			int i = 0;
			StatusMessage("선택된 그룹의 상세내역을 삭제합니다.");
			grdExScheduleList.UpdateData();

			DialogResult result = MessageBox.Show("선택하신 상세내역을 삭제 하시겠습니까?","편성그룹",MessageBoxButtons.YesNo, MessageBoxIcon.Question,MessageBoxDefaultButton.Button2);
			if (result == DialogResult.No) return;

			try
			{
				foreach( DataRow row in groupDs.AdSchedule.Rows )
				{
					if( row["CheckYn"].ToString().Equals("True") )
					{
						groupModel.Init();
						groupModel.GroupCode	=	groupCode;
						groupModel.CategoryCode =	row["Category"].ToString();
						groupModel.GenreCode	=	row["Genre"].ToString();
						groupModel.ChannelNo	=	row["Channel"].ToString();
						groupModel.SeriesNo		=	row["Series"].ToString();
					
						new GroupManager(systemModel,commonModel).SetGroupDetailDelete(groupModel);
					
						if ( !groupModel.ResultCD.Equals("0000"))
						{
							FrameSystem.showMsgForm("편성그룹 삭제오류", new string[] {groupModel.ResultCD, groupModel.ResultDesc});
							return;
						}
						else
						{
							i++;
						}
					}
					Application.DoEvents();
				}

				ReloadDetailList();
			}
			catch(FrameException fe)
			{
				FrameSystem.showMsgForm("그룹상세내역이 삭제오류", new string[] {fe.ErrCode, fe.ResultMsg});
			}
			catch(Exception ex)
			{
				FrameSystem.showMsgForm("그룹상세내역이 삭제오류",new string[] {"",ex.Message});
			}			
			finally
			{
				if( i > 0 )	StatusMessage( i.ToString() + "건의 편성그룹 상세내역 삭제완료.");
				else		StatusMessage( "" );
			}
		}
        
		private void ResetDetail()
		{				
			btnDelete.Enabled	= false;
			btnCategory.Enabled = false;
			btnGenre.Enabled	= false;
			btnChannel.Enabled	= false;
			btnSeries.Enabled	= false;
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
				if(row["GroupCode"].ToString().Equals(groupCode))
				{					
					cmSchedule.Position = rowIndex;
					break;								
				}
				rowIndex++;
			}
			grdExScheduleList.EnsureVisible();
		}

		private void ResetGroupText()
		{			
			ebGroupName.Text           = "";			
			ebComment.Text        = "";			
			rbUseYn_Y.Checked         = true;
			rbUseYn_N.Checked         = false;
			if(!IsAdding)
			{
				ebGroupName.ReadOnly         = true;
				ebComment.ReadOnly         = true;
				ebGroupName.BackColor        = Color.WhiteSmoke;
				ebComment.BackColor        = Color.WhiteSmoke;
			}
		}

		/// <summary>
		/// 그룹 상세정보의 셋트
		/// </summary>
		private void SetGroupDetailText()
		{
			int curRow = cmChannel.Position;
			if(curRow >= 0)
			{
				groupCode				   = dtChannel.Rows[curRow]["GroupCode"].ToString();			
				ebGroupName.Text           = dtChannel.Rows[curRow]["GroupName"].ToString();				
				ebComment.Text			   = dtChannel.Rows[curRow]["Comment"].ToString();							
				string UseYn               = dtChannel.Rows[curRow]["UseYn"].ToString();

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
				SearchGroupDetail();				
				IsAdding = false;
				ResetTextReadonly();
			}
			StatusMessage("준비");
		}

		
		/// <summary>
		/// 그룹 상세정보의 셋트
		/// </summary>
//		private void SetGroupDetailButton()
//		{
//			int curRow = cmSchedule.Position;
//			if(curRow >= 0)
//			{
//				if(dtSchedule.Rows[curRow]["CategoryCode"].ToString()!=null && dtSchedule.Rows[curRow]["CategoryCode"].ToString()!="0")
//				{
//					btnCategory.Enabled = false;
//					btnGenre.Enabled = true;
//					btnChannel.Enabled = true;
//				}
//				if(dtSchedule.Rows[curRow]["GenreCode"].ToString()!=null && dtSchedule.Rows[curRow]["GenreCode"].ToString()!="0")
//				{
//					btnCategory.Enabled = false;
//					btnGenre.Enabled = false;
//					btnChannel.Enabled = true;
//				}
//				if(dtSchedule.Rows[curRow]["ChannelNo"].ToString()!=null && dtSchedule.Rows[curRow]["ChannelNo"].ToString()!="0")
//				{
//					btnCategory.Enabled = false;
//					btnGenre.Enabled = false;
//					btnChannel.Enabled = false;
//				}
//			}			
//		}

		/// <summary>
		/// 상세정보 수정가능케
		/// </summary>
		private void ResetTextReadonly()
		{			
			ebGroupName.ReadOnly       = false;
			ebComment.ReadOnly      = false;			
			// 사용자구분이 어드민 또는 수퍼유저인 경우만 사용레벨, 사용자구분, 사용여부를 수정할 수 있다.
			if(commonModel.UserClass.Equals("10") || commonModel.UserClass.Equals("20"))
			{
				rbUseYn_Y.Enabled         = true;
				rbUseYn_N.Enabled         = true;
			}
			
			ebGroupName.BackColor      = Color.White;
			ebComment.BackColor     = Color.White;						
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
			SearchGroupList();					
		}
			
		public void ReloadDetailList()
		{
			SearchGroupDetail();
		}

		private void btnCategory_Click(object sender, System.EventArgs e)
		{
			// 채널 목록 검색 팝업 띠우기
			CategoryPopForm CategoryPopForm = new CategoryPopForm(this);
			CategoryPopForm.ShowDialog();
			CategoryPopForm.Dispose();
			CategoryPopForm = null;
		}		

        
		/// <summary>
		/// 그룹에 카테고리를 등록한다
		/// </summary>
        public string CategoryName
        {
            get { return KeyCategoryName; }
            set { KeyCategoryName = value; }
        }
		
		/// <summary>
		/// 그룹에 카테고리를 등록한다
		/// </summary>
		public string CategoryCode
		{
			set
			{
				this.KeyCategoryCode = value;
				groupModel.GroupCode = groupCode;
				groupModel.CategoryCode = KeyCategoryCode;

				if(groupDs.AdSchedule.Rows.Count<=0)
				{
					new GroupManager(systemModel,commonModel).SetGroupDetailAdd(groupModel);
					ReloadDetailList();
                    MessageBox.Show("선택하신 카테고리[" + KeyCategoryCode + "]" + KeyCategoryName + "가 등록 되었습니다.", "그룹상세", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    KeyCategoryCode = "";					
				}
				else
				{
                    for( int j = 0 ; j < groupDs.AdSchedule.Rows.Count ; j++)
					{
						DataRow row = groupDs.AdSchedule.Rows[j];
						DataRow[] CategoryRows = groupDs.AdSchedule.Select("Category = '"+KeyCategoryCode+"'");

						if(CategoryRows.Length!=0)
						{
                            MessageBox.Show("선택하신 카테고리[" + KeyCategoryCode + "]" + KeyCategoryName + "가 기존 내역에 있습니다.\n새그룹에 등록하여 주세요", "그룹상세", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            break;
						}
						else
						{
							new GroupManager(systemModel,commonModel).SetGroupDetailAdd(groupModel);
							ReloadDetailList();
                            MessageBox.Show("선택하신 카테고리[" + KeyCategoryCode + "]" + KeyCategoryName + "가 등록 되었습니다.", "그룹상세", MessageBoxButtons.OK, MessageBoxIcon.Information);
							KeyCategoryCode = "";
                            break;
						}
					}
				}
			}
		}


		private void btnGenre_Click(object sender, System.EventArgs e)
		{
			// 채널 목록 검색 팝업 띠우기
			GenrePopForm1 GenrePopForm1 = new GenrePopForm1(this);
			int curRow = cmSchedule.Position;

//			if(curRow >= 0)
//			{			
//				GenrePopForm1.KeyCategoryCode1 = dtSchedule.Rows[curRow]["Category"].ToString();						
//			}

			GenrePopForm1.ShowDialog();
			GenrePopForm1.Dispose();
			GenrePopForm1 = null;
		}
		
		public bool SetGenre( string category, string genre )
		{
			groupModel.GroupCode	=	groupCode;
			groupModel.CategoryCode	=	category;
			groupModel.GenreCode	=	genre;

            if (groupDs.AdSchedule.Rows.Count > 0)
            {
                for (int i = 0; i < groupDs.AdSchedule.Rows.Count; i++)
                {
                    DataRow row = groupDs.AdSchedule.Rows[i];
                    DataRow[] CategoryRows = groupDs.AdSchedule.Select("Category = " + category + " and Genre = 0 and Channel=0 and Series=0");

                    if (CategoryRows.Length == 0)
                    {
                        DataRow[] FoundRows = groupDs.AdSchedule.Select("Category = " + category + " AND Genre =" + genre);
                        //FoundRows가 0이면 무조건 무조건 추가
                        if (FoundRows.Length != 0)
                        {
                            MessageBox.Show("선택하신 장르["+genre+"]가 기존 내역에 있습니다", "그룹상세", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return false;
                        }
                        else
                        {
                            new GroupManager(systemModel, commonModel).SetGroupGenreUpdate(groupModel);
                            ReloadDetailList();
                            break;
                        }
                    }
                    else
                    {
                        MessageBox.Show("상위 카테고리가 선택되어 있습니다.\n\n장르선택을 하실경우엔 상위 카테고리를 삭제해야 합니다.", "그룹상세", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return false;
                    }
                }
            }
            else
            {
                // 처음 등록하는 경우
                new GroupManager(systemModel, commonModel).SetGroupGenreUpdate(groupModel);
                ReloadDetailList();
                
            }
            return true;
		}


		/// <summary>
		/// 채널편성 버튼 클릭
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnChannel_Click(object sender, System.EventArgs e)
		{
			// 채널 목록 검색 팝업 띠우기
			ChannelNoPopForm1 ChannelNoPopForm1 = new ChannelNoPopForm1(this);
			ChannelNoPopForm1.ShowDialog();
			ChannelNoPopForm1.Dispose();
			ChannelNoPopForm1 = null;
		}

		private void btnSeries_Click(object sender, System.EventArgs e)
		{
			SeriesPopForm	pForm = new SeriesPopForm(this);
			pForm.ShowDialog(this);
			pForm.Dispose();
			pForm = null;
		}

        private void btnMap_Click(object sender, System.EventArgs e)
        {
            AdManagerClient._10_Media._10_Group.GroupMappingInfo    form = new AdManagerClient._10_Media._10_Group.GroupMappingInfo();
            form.ShowDialog(this);
            form.Dispose();
            form = null;
        }
				
		public bool ChannelNo( string category, string genre, string channel)
		{
			bool		rtnValue= false;
			DataRow[]	rows	= null;
			try
			{
				groupModel.Init();
				groupModel.GroupCode	= groupCode;
				groupModel.CategoryCode = category;
				groupModel.GenreCode	= genre;
				groupModel.ChannelNo	= channel;

				rows = groupDs.AdSchedule.Select("Category = "+category+" and Genre = 0 and Channel=0 and Series=0");
				if ( rows.Length > 0 )
				{
					MessageBox.Show("상위 카테고리가 선택되어 있습니다.\n\n채널선택을 하실경우엔 상위 카테고리를 삭제해야 합니다.", "그룹상세",MessageBoxButtons.OK, MessageBoxIcon.Information);
					return false;
				}

				rows = groupDs.AdSchedule.Select("Category=" + category + " and Genre = " + genre + " and Channel=0 and Series=0");
				if ( rows.Length > 0 )
				{
					MessageBox.Show("상위 장르가 선택되어 있습니다.\n\n채널선택을 하실경우엔 상위 장르를 삭제해야 합니다.", "그룹상세",MessageBoxButtons.OK, MessageBoxIcon.Information);
					return false;
				}

				rows = groupDs.AdSchedule.Select("Category = "+category+ " and Genre = " + genre + " and Channel=" + channel );
				if ( rows.Length > 0 )
				{
					MessageBox.Show("선택한 채널이 이미 등록되어 있습니다.\n\n다른채널을 선택하십시요", "그룹상세",MessageBoxButtons.OK, MessageBoxIcon.Information);
					return false;
				}

				new GroupManager(systemModel,commonModel).SetGroupChannelUpdate(groupModel);
				rtnValue = true;
				ReloadDetailList();
			}
			catch(Exception ex)
			{
				rtnValue = false;
				MessageBox.Show("오류발생\n\n" + ex.Message, "그룹상세",MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
			return rtnValue;
		}


		/// <summary>
		/// 편성선택 팝업창에서 해당 Row를 더블클릭했을때 호출한다
		/// 해당 회차를 선택된 그룹에 추가한다
		/// </summary>
		/// <param name="category"></param>
		/// <param name="genre"></param>
		/// <param name="channel"></param>
		/// <param name="series"></param>
		/// <returns></returns>
		public bool SeriesNo( string category, string genre, string channel, string series)
		{
			bool		rtnValue= false;
			DataRow[]	rows	= null;
			try
			{
				groupModel.Init();
				groupModel.GroupCode	= groupCode;
				groupModel.CategoryCode = category;
				groupModel.GenreCode	= genre;
				groupModel.ChannelNo	= channel;
				groupModel.SeriesNo		= series;

				rows = groupDs.AdSchedule.Select("Category = "+category+" and Genre = 0 and Channel=0 and Series=0");
				if ( rows.Length > 0 )
				{
					MessageBox.Show("상위 카테고리가 선택되어 있습니다.\n\n회차선택을 하실경우엔 상위 카테고리를 삭제해야 합니다.", "그룹상세",MessageBoxButtons.OK, MessageBoxIcon.Information);
					return false;
				}

				rows = groupDs.AdSchedule.Select("Category=" + category + " and Genre = " + genre + " and Channel=0 and Series=0");
				if ( rows.Length > 0 )
				{
					MessageBox.Show("상위 장르가 선택되어 있습니다.\n\n회차선택을 하실경우엔 상위 장르를 삭제해야 합니다.", "그룹상세",MessageBoxButtons.OK, MessageBoxIcon.Information);
					return false;
				}

				rows = groupDs.AdSchedule.Select("Category = "+category+ " and Genre = " + genre + " and Channel=" + channel + " and Series=0" );
				if ( rows.Length > 0 )
				{
					MessageBox.Show("상위 채널이 선택되어 있습니다.\n\n회차선택을 하실경우엔 상위 채널를 삭제해야 합니다.", "그룹상세",MessageBoxButtons.OK, MessageBoxIcon.Information);
					return false;
				}

				rows = groupDs.AdSchedule.Select("Category = "+category+ " and Genre = " + genre + " and Channel=" + channel + " and Series=" + series );
				if ( rows.Length > 0 )
				{
					MessageBox.Show("선택한 회차가 이미 등록되어 있습니다.\n\n다른회차을 선택하십시요", "그룹상세",MessageBoxButtons.OK, MessageBoxIcon.Information);
					return false;
				}

				new GroupManager(systemModel,commonModel).SetGroupSerieslUpdate(groupModel);
				rtnValue = true;
				ReloadDetailList();
			}
			catch(Exception ex)
			{
				rtnValue = false;
				MessageBox.Show("오류발생\n\n" + ex.Message, "그룹상세",MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
			return rtnValue;
		}

		
		#endregion

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

        private void grdExScheduleList_ColumnHeaderClick_1(object sender, Janus.Windows.GridEX.ColumnActionEventArgs e)
        {

            //컬럼Index 0(체크박스컬럼이)이 아니면 빠져나가게 처리.
            if (e.Column.Index != 0)
            {
                return;
            }

            if (IsAllCheck)
            {
                grdExScheduleList.UnCheckAllRecords();
                for (int i = 0; i < dtSchedule.Rows.Count; i++)
                {
                    dtSchedule.Rows[i].BeginEdit();
                    dtSchedule.Rows[i]["CheckYn"] = "False";
                    dtSchedule.Rows[i].EndEdit();
                }
                IsAllCheck = false;
            }
            else
            {
                grdExScheduleList.CheckAllRecords();
                for (int i = 0; i < dtSchedule.Rows.Count; i++)
                {
                    dtSchedule.Rows[i].BeginEdit();
                    dtSchedule.Rows[i]["CheckYn"] = "True";
                    dtSchedule.Rows[i].EndEdit();
                }
                IsAllCheck = true;
            }	
        }


 
	}
}