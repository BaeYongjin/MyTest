// ===============================================================================
// ChannelSetControl for Charites Project
//
// ChannelSetControl.cs
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
using Excel = Microsoft.Office.Interop.Excel; // 엑셀 참조
using System.Reflection;

namespace AdManagerClient
{
	/// <summary>
	/// 채널관리 컨트롤
	/// </summary>
	public class ChannelSetControl : System.Windows.Forms.UserControl, IUserControl
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
		ChannelSetModel channelSetModel  = new ChannelSetModel();	// 채널정보모델

		// 화면처리용 변수
		bool IsNewSearchKey		  = true;					// 검색어입력 여부
		CurrencyManager cm        = null;					// 데이터 그리드의 변경에 따른 데이터셋 관리를 위하여			
		CurrencyManager cmChild        = null;					// 데이터 그리드의 변경에 따른 데이터셋 관리를 위하여			
		DataTable       dt        = null;
		DataTable       dtChild        = null;
		private string        genreCode = null;

		private string        mediaCode_old = null;
		private string        categoryCode_old = null;
		private string        genreCode_old = null;
		private string        channelNo_old = null;

        bool IsSearching = false; // 조회중 상세화면이 업데이트 되는 것을 방지 하기위함 2011.11.29 JH.Park
		bool IsAdding             = false;
		bool canRead			  = false;
		bool canUpdate			  = false;
		bool canCreate            = false;
		bool canDelete            = false;

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
		private System.Data.DataView dvChannelSet;
		private Janus.Windows.EditControls.UIButton btnDelete;
		private Janus.Windows.EditControls.UIButton btnAdd;
		private Janus.Windows.EditControls.UIButton btnSave;
		private Janus.Windows.GridEX.EditControls.EditBox ebChannelNo;
		private Janus.Windows.GridEX.EditControls.EditBox ebModDt;
		private System.Data.DataView dvCateGen;
		private Janus.Windows.EditControls.UIComboBox cbMediaName;
		private Janus.Windows.EditControls.UIComboBox cbSearchMediaName;
		private Janus.Windows.EditControls.UIComboBox cbSearchCategoryName;
		private Janus.Windows.EditControls.UIComboBox cbSearchGenreName;
		private Janus.Windows.EditControls.UIComboBox cbCategoryName;
		private System.Windows.Forms.Label lbGenreName;
		private System.Windows.Forms.Label lbCategoryName;
		private System.Windows.Forms.Label lbMediaName;
		private System.Windows.Forms.Label lbModDt;
		private System.Windows.Forms.Label lbSeriesNo;
		private System.Windows.Forms.Label lbChannelNo;
		private System.Windows.Forms.ImageList imageList;
		private Janus.Windows.GridEX.EditControls.EditBox ebGenreName;
		private Janus.Windows.GridEX.EditControls.EditBox ebTotalSeries;
		private Janus.Windows.GridEX.EditControls.EditBox ebTitle;
		private System.Windows.Forms.Label lbTitle;
		private Janus.Windows.UI.Dock.UIPanelGroup uiPanelChannelSet;
		private Janus.Windows.UI.Dock.UIPanel uiPanel1;
		private Janus.Windows.UI.Dock.UIPanelInnerContainer uiPanel1Container;
		private Janus.Windows.UI.Dock.UIPanel uiPanel2;
		private Janus.Windows.UI.Dock.UIPanelInnerContainer uiPanel2Container;
		private Janus.Windows.GridEX.GridEX grdExCategenList;
		private Janus.Windows.GridEX.GridEX grdExChannelSetList;
		private Janus.Windows.EditControls.UIButton btnGenreName;
		private Janus.Windows.EditControls.UIButton btnChannelNo;
		private Janus.Windows.EditControls.UIButton btnExcel;
		private AdManagerClient._10_Media._08_ChannelSet.ChannelSetDs channelSetDs;		
		private System.ComponentModel.IContainer components;

		public ChannelSetControl()
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ChannelSetControl));
            Janus.Windows.GridEX.GridEXLayout grdExCategenList_Layout_0 = new Janus.Windows.GridEX.GridEXLayout();
            Janus.Windows.GridEX.GridEXLayout grdExChannelSetList_DesignTimeLayout = new Janus.Windows.GridEX.GridEXLayout();
            this.uiPM = new Janus.Windows.UI.Dock.UIPanelManager(this.components);
            this.uiPanelUsers = new Janus.Windows.UI.Dock.UIPanelGroup();
            this.uiPanelUsersSearch = new Janus.Windows.UI.Dock.UIPanel();
            this.uiPanelUsersSearchContainer = new Janus.Windows.UI.Dock.UIPanelInnerContainer();
            this.pnlSearch = new System.Windows.Forms.Panel();
            this.btnExcel = new Janus.Windows.EditControls.UIButton();
            this.cbSearchMediaName = new Janus.Windows.EditControls.UIComboBox();
            this.ebSearchKey = new Janus.Windows.GridEX.EditControls.EditBox();
            this.cbSearchCategoryName = new Janus.Windows.EditControls.UIComboBox();
            this.cbSearchGenreName = new Janus.Windows.EditControls.UIComboBox();
            this.btnSearch = new Janus.Windows.EditControls.UIButton();
            this.uiPanelChannelSet = new Janus.Windows.UI.Dock.UIPanelGroup();
            this.uiPanel1 = new Janus.Windows.UI.Dock.UIPanel();
            this.uiPanel1Container = new Janus.Windows.UI.Dock.UIPanelInnerContainer();
            this.grdExCategenList = new Janus.Windows.GridEX.GridEX();
            this.dvCateGen = new System.Data.DataView();
            this.channelSetDs = new AdManagerClient._10_Media._08_ChannelSet.ChannelSetDs();
            this.uiPanel2 = new Janus.Windows.UI.Dock.UIPanel();
            this.uiPanel2Container = new Janus.Windows.UI.Dock.UIPanelInnerContainer();
            this.grdExChannelSetList = new Janus.Windows.GridEX.GridEX();
            this.dvChannelSet = new System.Data.DataView();
            this.uiPanelUserDetail = new Janus.Windows.UI.Dock.UIPanel();
            this.uiPanelUserDetailContainer = new Janus.Windows.UI.Dock.UIPanelInnerContainer();
            this.pnlUserDetail = new System.Windows.Forms.Panel();
            this.btnChannelNo = new Janus.Windows.EditControls.UIButton();
            this.btnGenreName = new Janus.Windows.EditControls.UIButton();
            this.ebTotalSeries = new Janus.Windows.GridEX.EditControls.EditBox();
            this.ebTitle = new Janus.Windows.GridEX.EditControls.EditBox();
            this.ebGenreName = new Janus.Windows.GridEX.EditControls.EditBox();
            this.ebModDt = new Janus.Windows.GridEX.EditControls.EditBox();
            this.lbModDt = new System.Windows.Forms.Label();
            this.cbCategoryName = new Janus.Windows.EditControls.UIComboBox();
            this.ebChannelNo = new Janus.Windows.GridEX.EditControls.EditBox();
            this.lbChannelNo = new System.Windows.Forms.Label();
            this.btnDelete = new Janus.Windows.EditControls.UIButton();
            this.btnAdd = new Janus.Windows.EditControls.UIButton();
            this.btnSave = new Janus.Windows.EditControls.UIButton();
            this.cbMediaName = new Janus.Windows.EditControls.UIComboBox();
            this.lbMediaName = new System.Windows.Forms.Label();
            this.lbSeriesNo = new System.Windows.Forms.Label();
            this.lbCategoryName = new System.Windows.Forms.Label();
            this.lbGenreName = new System.Windows.Forms.Label();
            this.lbTitle = new System.Windows.Forms.Label();
            this.uiButton1 = new Janus.Windows.EditControls.UIButton();
            this.uiButton2 = new Janus.Windows.EditControls.UIButton();
            this.imageList = new System.Windows.Forms.ImageList(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.uiPM)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelUsers)).BeginInit();
            this.uiPanelUsers.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelUsersSearch)).BeginInit();
            this.uiPanelUsersSearch.SuspendLayout();
            this.uiPanelUsersSearchContainer.SuspendLayout();
            this.pnlSearch.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelChannelSet)).BeginInit();
            this.uiPanelChannelSet.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanel1)).BeginInit();
            this.uiPanel1.SuspendLayout();
            this.uiPanel1Container.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdExCategenList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dvCateGen)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.channelSetDs)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanel2)).BeginInit();
            this.uiPanel2.SuspendLayout();
            this.uiPanel2Container.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdExChannelSetList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dvChannelSet)).BeginInit();
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
            this.uiPanelChannelSet.Id = new System.Guid("88f86479-98e4-43d4-bf03-12fd789fcfc5");
            this.uiPanelChannelSet.StaticGroup = true;
            this.uiPanel1.Id = new System.Guid("91eeb7fc-856c-4a9c-b37e-db742cfa9b2e");
            this.uiPanelChannelSet.Panels.Add(this.uiPanel1);
            this.uiPanel2.Id = new System.Guid("3d64f731-de32-42ba-a10f-ac3b5ac56703");
            this.uiPanelChannelSet.Panels.Add(this.uiPanel2);
            this.uiPanelUsers.Panels.Add(this.uiPanelChannelSet);
            this.uiPanelUserDetail.Id = new System.Guid("b2973a57-d3e1-4d80-9387-7a6dc3332542");
            this.uiPanelUsers.Panels.Add(this.uiPanelUserDetail);
            this.uiPM.Panels.Add(this.uiPanelUsers);
            // 
            // Design Time Panel Info:
            // 
            this.uiPM.BeginPanelInfo();
            this.uiPM.AddDockPanelInfo(new System.Guid("b162c53a-7940-47cd-a0ed-9762c59c50d8"), Janus.Windows.UI.Dock.PanelGroupStyle.HorizontalTiles, Janus.Windows.UI.Dock.PanelDockStyle.Fill, true, new System.Drawing.Size(1010, 677), true);
            this.uiPM.AddDockPanelInfo(new System.Guid("afb3b7d0-aa79-443c-a1e8-3baba38f7a4d"), new System.Guid("b162c53a-7940-47cd-a0ed-9762c59c50d8"), 42, true);
            this.uiPM.AddDockPanelInfo(new System.Guid("88f86479-98e4-43d4-bf03-12fd789fcfc5"), new System.Guid("b162c53a-7940-47cd-a0ed-9762c59c50d8"), Janus.Windows.UI.Dock.PanelGroupStyle.VerticalTiles, true, 435, true);
            this.uiPM.AddDockPanelInfo(new System.Guid("91eeb7fc-856c-4a9c-b37e-db742cfa9b2e"), new System.Guid("88f86479-98e4-43d4-bf03-12fd789fcfc5"), 425, true);
            this.uiPM.AddDockPanelInfo(new System.Guid("3d64f731-de32-42ba-a10f-ac3b5ac56703"), new System.Guid("88f86479-98e4-43d4-bf03-12fd789fcfc5"), 422, true);
            this.uiPM.AddDockPanelInfo(new System.Guid("b2973a57-d3e1-4d80-9387-7a6dc3332542"), new System.Guid("b162c53a-7940-47cd-a0ed-9762c59c50d8"), 150, true);
            this.uiPM.AddFloatingPanelInfo(new System.Guid("b162c53a-7940-47cd-a0ed-9762c59c50d8"), Janus.Windows.UI.Dock.PanelGroupStyle.HorizontalTiles, true, new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
            this.uiPM.AddFloatingPanelInfo(new System.Guid("8e3140b9-bfcb-43af-b84e-2ee6cddf51dc"), new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
            this.uiPM.AddFloatingPanelInfo(new System.Guid("b2973a57-d3e1-4d80-9387-7a6dc3332542"), new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
            this.uiPM.AddFloatingPanelInfo(new System.Guid("afb3b7d0-aa79-443c-a1e8-3baba38f7a4d"), new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
            this.uiPM.AddFloatingPanelInfo(new System.Guid("88f86479-98e4-43d4-bf03-12fd789fcfc5"), Janus.Windows.UI.Dock.PanelGroupStyle.VerticalTiles, true, new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
            this.uiPM.AddFloatingPanelInfo(new System.Guid("91eeb7fc-856c-4a9c-b37e-db742cfa9b2e"), new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
            this.uiPM.AddFloatingPanelInfo(new System.Guid("3d64f731-de32-42ba-a10f-ac3b5ac56703"), new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
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
            this.uiPanelUsers.Text = "채널구성관리";
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
            this.pnlSearch.Controls.Add(this.btnExcel);
            this.pnlSearch.Controls.Add(this.cbSearchMediaName);
            this.pnlSearch.Controls.Add(this.ebSearchKey);
            this.pnlSearch.Controls.Add(this.cbSearchCategoryName);
            this.pnlSearch.Controls.Add(this.cbSearchGenreName);
            this.pnlSearch.Controls.Add(this.btnSearch);
            this.pnlSearch.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlSearch.Location = new System.Drawing.Point(0, 0);
            this.pnlSearch.Name = "pnlSearch";
            this.pnlSearch.Size = new System.Drawing.Size(1008, 41);
            this.pnlSearch.TabIndex = 0;
            // 
            // btnExcel
            // 
            this.btnExcel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExcel.Enabled = false;
            this.btnExcel.Location = new System.Drawing.Point(903, 8);
            this.btnExcel.Name = "btnExcel";
            this.btnExcel.Size = new System.Drawing.Size(96, 24);
            this.btnExcel.TabIndex = 6;
            this.btnExcel.Text = "EXCEL출력";
            this.btnExcel.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.btnExcel.Click += new System.EventHandler(this.btnExcel_Click);
            // 
            // cbSearchMediaName
            // 
            this.cbSearchMediaName.ComboStyle = Janus.Windows.EditControls.ComboStyle.DropDownList;
            this.cbSearchMediaName.Font = new System.Drawing.Font("굴림체", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.cbSearchMediaName.Location = new System.Drawing.Point(8, 11);
            this.cbSearchMediaName.Name = "cbSearchMediaName";
            this.cbSearchMediaName.Size = new System.Drawing.Size(152, 21);
            this.cbSearchMediaName.TabIndex = 1;
            this.cbSearchMediaName.Text = "매체선택";
            this.cbSearchMediaName.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            // 
            // ebSearchKey
            // 
            this.ebSearchKey.Location = new System.Drawing.Point(488, 11);
            this.ebSearchKey.Name = "ebSearchKey";
            this.ebSearchKey.Size = new System.Drawing.Size(144, 21);
            this.ebSearchKey.TabIndex = 4;
            this.ebSearchKey.Text = "검색어를 입력하세요";
            this.ebSearchKey.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
            this.ebSearchKey.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            this.ebSearchKey.TextChanged += new System.EventHandler(this.ebSearchKey_TextChanged);
            this.ebSearchKey.Click += new System.EventHandler(this.ebSearchKey_Click);
            this.ebSearchKey.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ebSearchKey_KeyDown);
            // 
            // cbSearchCategoryName
            // 
            this.cbSearchCategoryName.ComboStyle = Janus.Windows.EditControls.ComboStyle.DropDownList;
            this.cbSearchCategoryName.Font = new System.Drawing.Font("굴림체", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.cbSearchCategoryName.Location = new System.Drawing.Point(168, 11);
            this.cbSearchCategoryName.Name = "cbSearchCategoryName";
            this.cbSearchCategoryName.Size = new System.Drawing.Size(152, 21);
            this.cbSearchCategoryName.TabIndex = 2;
            this.cbSearchCategoryName.Text = "카테고리선택";
            this.cbSearchCategoryName.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            // 
            // cbSearchGenreName
            // 
            this.cbSearchGenreName.ComboStyle = Janus.Windows.EditControls.ComboStyle.DropDownList;
            this.cbSearchGenreName.Font = new System.Drawing.Font("굴림체", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.cbSearchGenreName.Location = new System.Drawing.Point(328, 11);
            this.cbSearchGenreName.Name = "cbSearchGenreName";
            this.cbSearchGenreName.Size = new System.Drawing.Size(152, 21);
            this.cbSearchGenreName.TabIndex = 3;
            this.cbSearchGenreName.Text = "장르선택";
            this.cbSearchGenreName.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            // 
            // btnSearch
            // 
            this.btnSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSearch.Enabled = false;
            this.btnSearch.Location = new System.Drawing.Point(799, 8);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(96, 24);
            this.btnSearch.TabIndex = 5;
            this.btnSearch.Text = "조 회";
            this.btnSearch.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // uiPanelChannelSet
            // 
            this.uiPanelChannelSet.CaptionStyle = Janus.Windows.UI.Dock.PanelCaptionStyle.Dark;
            this.uiPanelChannelSet.GroupStyle = Janus.Windows.UI.Dock.PanelGroupStyle.VerticalTiles;
            this.uiPanelChannelSet.Location = new System.Drawing.Point(0, 69);
            this.uiPanelChannelSet.Name = "uiPanelChannelSet";
            this.uiPanelChannelSet.Size = new System.Drawing.Size(1010, 449);
            this.uiPanelChannelSet.TabIndex = 0;
            this.uiPanelChannelSet.Text = "채널구성";
            // 
            // uiPanel1
            // 
            this.uiPanel1.InnerContainer = this.uiPanel1Container;
            this.uiPanel1.Location = new System.Drawing.Point(0, 22);
            this.uiPanel1.Name = "uiPanel1";
            this.uiPanel1.Size = new System.Drawing.Size(505, 427);
            this.uiPanel1.TabIndex = 0;
            this.uiPanel1.Text = "메뉴";
            // 
            // uiPanel1Container
            // 
            this.uiPanel1Container.Controls.Add(this.grdExCategenList);
            this.uiPanel1Container.Location = new System.Drawing.Point(1, 23);
            this.uiPanel1Container.Name = "uiPanel1Container";
            this.uiPanel1Container.Size = new System.Drawing.Size(503, 403);
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
            this.grdExCategenList.EditorsControlStyle.ButtonAppearance = Janus.Windows.GridEX.ButtonAppearance.Regular;
            this.grdExCategenList.EmptyRows = true;
            this.grdExCategenList.FocusCellFormatStyle.BackColor = System.Drawing.SystemColors.Highlight;
            this.grdExCategenList.FocusCellFormatStyle.ForeColor = System.Drawing.Color.White;
            this.grdExCategenList.FocusStyle = Janus.Windows.GridEX.FocusStyle.None;
            this.grdExCategenList.Font = new System.Drawing.Font("굴림체", 9F);
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
            this.grdExCategenList.Size = new System.Drawing.Size(503, 403);
            this.grdExCategenList.TabIndex = 7;
            this.grdExCategenList.TabKeyBehavior = Janus.Windows.GridEX.TabKeyBehavior.ControlNavigation;
            this.grdExCategenList.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            this.grdExCategenList.Enter += new System.EventHandler(this.OnGrdRowChanged);
            // 
            // dvCateGen
            // 
            this.dvCateGen.Table = this.channelSetDs.Categens;
            // 
            // channelSetDs
            // 
            this.channelSetDs.DataSetName = "ChannelSetDs";
            this.channelSetDs.Locale = new System.Globalization.CultureInfo("en-US");
            this.channelSetDs.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // uiPanel2
            // 
            this.uiPanel2.InnerContainer = this.uiPanel2Container;
            this.uiPanel2.Location = new System.Drawing.Point(509, 22);
            this.uiPanel2.Name = "uiPanel2";
            this.uiPanel2.Size = new System.Drawing.Size(501, 427);
            this.uiPanel2.TabIndex = 0;
            this.uiPanel2.Text = "채널";
            // 
            // uiPanel2Container
            // 
            this.uiPanel2Container.Controls.Add(this.grdExChannelSetList);
            this.uiPanel2Container.Location = new System.Drawing.Point(1, 23);
            this.uiPanel2Container.Name = "uiPanel2Container";
            this.uiPanel2Container.Size = new System.Drawing.Size(499, 403);
            this.uiPanel2Container.TabIndex = 0;
            // 
            // grdExChannelSetList
            // 
            this.grdExChannelSetList.AllowEdit = Janus.Windows.GridEX.InheritableBoolean.False;
            this.grdExChannelSetList.AlternatingColors = true;
            this.grdExChannelSetList.AutomaticSort = false;
            this.grdExChannelSetList.BorderStyle = Janus.Windows.GridEX.BorderStyle.None;
            this.grdExChannelSetList.DataSource = this.dvChannelSet;
            grdExChannelSetList_DesignTimeLayout.LayoutString = resources.GetString("grdExChannelSetList_DesignTimeLayout.LayoutString");
            this.grdExChannelSetList.DesignTimeLayout = grdExChannelSetList_DesignTimeLayout;
            this.grdExChannelSetList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdExChannelSetList.EditorsControlStyle.ButtonAppearance = Janus.Windows.GridEX.ButtonAppearance.Regular;
            this.grdExChannelSetList.EmptyRows = true;
            this.grdExChannelSetList.FocusCellFormatStyle.BackColor = System.Drawing.SystemColors.Highlight;
            this.grdExChannelSetList.FocusCellFormatStyle.ForeColor = System.Drawing.Color.White;
            this.grdExChannelSetList.FocusStyle = Janus.Windows.GridEX.FocusStyle.None;
            this.grdExChannelSetList.Font = new System.Drawing.Font("굴림체", 9F);
            this.grdExChannelSetList.FrozenColumns = 2;
            this.grdExChannelSetList.GridLineColor = System.Drawing.Color.Silver;
            this.grdExChannelSetList.GridLines = Janus.Windows.GridEX.GridLines.Vertical;
            this.grdExChannelSetList.GridLineStyle = Janus.Windows.GridEX.GridLineStyle.Solid;
            this.grdExChannelSetList.GroupByBoxVisible = false;
            this.grdExChannelSetList.HeaderFormatStyle.TextAlignment = Janus.Windows.GridEX.TextAlignment.Center;
            this.grdExChannelSetList.HideSelection = Janus.Windows.GridEX.HideSelection.HighlightInactive;
            this.grdExChannelSetList.Location = new System.Drawing.Point(0, 0);
            this.grdExChannelSetList.Name = "grdExChannelSetList";
            this.grdExChannelSetList.ScrollBars = Janus.Windows.GridEX.ScrollBars.Vertical;
            this.grdExChannelSetList.SelectedInactiveFormatStyle.BackColor = System.Drawing.Color.Gold;
            this.grdExChannelSetList.Size = new System.Drawing.Size(499, 403);
            this.grdExChannelSetList.TabIndex = 8;
            this.grdExChannelSetList.TabKeyBehavior = Janus.Windows.GridEX.TabKeyBehavior.ControlNavigation;
            this.grdExChannelSetList.ThemedAreas = ((Janus.Windows.GridEX.ThemedArea)(((((((((Janus.Windows.GridEX.ThemedArea.ScrollBars | Janus.Windows.GridEX.ThemedArea.EditControls) 
            | Janus.Windows.GridEX.ThemedArea.Headers) 
            | Janus.Windows.GridEX.ThemedArea.GroupByBox) 
            | Janus.Windows.GridEX.ThemedArea.GroupRows) 
            | Janus.Windows.GridEX.ThemedArea.ControlBorder) 
            | Janus.Windows.GridEX.ThemedArea.Cards) 
            | Janus.Windows.GridEX.ThemedArea.Gridlines) 
            | Janus.Windows.GridEX.ThemedArea.CheckBoxes)));
            this.grdExChannelSetList.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            this.grdExChannelSetList.Enter += new System.EventHandler(this.OnGrdRowDetailChanged);
            // 
            // dvChannelSet
            // 
            this.dvChannelSet.Table = this.channelSetDs.ChannelSets;
            // 
            // uiPanelUserDetail
            // 
            this.uiPanelUserDetail.BackColor = System.Drawing.SystemColors.Window;
            this.uiPanelUserDetail.CaptionStyle = Janus.Windows.UI.Dock.PanelCaptionStyle.Dark;
            this.uiPanelUserDetail.InnerContainer = this.uiPanelUserDetailContainer;
            this.uiPanelUserDetail.Location = new System.Drawing.Point(0, 522);
            this.uiPanelUserDetail.Name = "uiPanelUserDetail";
            this.uiPanelUserDetail.Size = new System.Drawing.Size(1010, 155);
            this.uiPanelUserDetail.TabIndex = 0;
            this.uiPanelUserDetail.Text = "상세정보";
            // 
            // uiPanelUserDetailContainer
            // 
            this.uiPanelUserDetailContainer.Controls.Add(this.pnlUserDetail);
            this.uiPanelUserDetailContainer.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.uiPanelUserDetailContainer.Location = new System.Drawing.Point(1, 23);
            this.uiPanelUserDetailContainer.Name = "uiPanelUserDetailContainer";
            this.uiPanelUserDetailContainer.Size = new System.Drawing.Size(1008, 131);
            this.uiPanelUserDetailContainer.TabIndex = 0;
            // 
            // pnlUserDetail
            // 
            this.pnlUserDetail.BackColor = System.Drawing.SystemColors.Window;
            this.pnlUserDetail.Controls.Add(this.btnChannelNo);
            this.pnlUserDetail.Controls.Add(this.btnGenreName);
            this.pnlUserDetail.Controls.Add(this.ebTotalSeries);
            this.pnlUserDetail.Controls.Add(this.ebTitle);
            this.pnlUserDetail.Controls.Add(this.ebGenreName);
            this.pnlUserDetail.Controls.Add(this.ebModDt);
            this.pnlUserDetail.Controls.Add(this.lbModDt);
            this.pnlUserDetail.Controls.Add(this.cbCategoryName);
            this.pnlUserDetail.Controls.Add(this.ebChannelNo);
            this.pnlUserDetail.Controls.Add(this.lbChannelNo);
            this.pnlUserDetail.Controls.Add(this.btnDelete);
            this.pnlUserDetail.Controls.Add(this.btnAdd);
            this.pnlUserDetail.Controls.Add(this.btnSave);
            this.pnlUserDetail.Controls.Add(this.cbMediaName);
            this.pnlUserDetail.Controls.Add(this.lbMediaName);
            this.pnlUserDetail.Controls.Add(this.lbSeriesNo);
            this.pnlUserDetail.Controls.Add(this.lbCategoryName);
            this.pnlUserDetail.Controls.Add(this.lbGenreName);
            this.pnlUserDetail.Controls.Add(this.lbTitle);
            this.pnlUserDetail.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlUserDetail.Location = new System.Drawing.Point(0, 0);
            this.pnlUserDetail.Name = "pnlUserDetail";
            this.pnlUserDetail.Size = new System.Drawing.Size(1008, 131);
            this.pnlUserDetail.TabIndex = 0;
            // 
            // btnChannelNo
            // 
            this.btnChannelNo.Location = new System.Drawing.Point(352, 32);
            this.btnChannelNo.Name = "btnChannelNo";
            this.btnChannelNo.Size = new System.Drawing.Size(104, 24);
            this.btnChannelNo.TabIndex = 12;
            this.btnChannelNo.Text = "채널선택";
            this.btnChannelNo.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.btnChannelNo.Click += new System.EventHandler(this.btnChannelNo_Click);
            // 
            // btnGenreName
            // 
            this.btnGenreName.Location = new System.Drawing.Point(560, 32);
            this.btnGenreName.Name = "btnGenreName";
            this.btnGenreName.Size = new System.Drawing.Size(104, 22);
            this.btnGenreName.TabIndex = 13;
            this.btnGenreName.Text = "장르선택";
            this.btnGenreName.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.btnGenreName.Click += new System.EventHandler(this.btnGenreName_Click);
            // 
            // ebTotalSeries
            // 
            this.ebTotalSeries.Font = new System.Drawing.Font("굴림체", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.ebTotalSeries.Location = new System.Drawing.Point(560, 64);
            this.ebTotalSeries.MaxLength = 120;
            this.ebTotalSeries.Name = "ebTotalSeries";
            this.ebTotalSeries.Size = new System.Drawing.Size(56, 21);
            this.ebTotalSeries.TabIndex = 15;
            this.ebTotalSeries.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
            this.ebTotalSeries.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            this.ebTotalSeries.Click += new System.EventHandler(this.ebTotalSeries_Click);
            // 
            // ebTitle
            // 
            this.ebTitle.Font = new System.Drawing.Font("굴림체", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.ebTitle.Location = new System.Drawing.Point(80, 64);
            this.ebTitle.MaxLength = 120;
            this.ebTitle.Name = "ebTitle";
            this.ebTitle.Size = new System.Drawing.Size(376, 21);
            this.ebTitle.TabIndex = 14;
            this.ebTitle.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
            this.ebTitle.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            // 
            // ebGenreName
            // 
            this.ebGenreName.Font = new System.Drawing.Font("굴림체", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.ebGenreName.Location = new System.Drawing.Point(560, 8);
            this.ebGenreName.MaxLength = 120;
            this.ebGenreName.Name = "ebGenreName";
            this.ebGenreName.Size = new System.Drawing.Size(280, 21);
            this.ebGenreName.TabIndex = 10;
            this.ebGenreName.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
            this.ebGenreName.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            // 
            // ebModDt
            // 
            this.ebModDt.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ebModDt.Font = new System.Drawing.Font("굴림체", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.ebModDt.Location = new System.Drawing.Point(704, 64);
            this.ebModDt.Name = "ebModDt";
            this.ebModDt.ReadOnly = true;
            this.ebModDt.Size = new System.Drawing.Size(136, 21);
            this.ebModDt.TabIndex = 16;
            this.ebModDt.TabStop = false;
            this.ebModDt.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
            this.ebModDt.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            // 
            // lbModDt
            // 
            this.lbModDt.Font = new System.Drawing.Font("굴림체", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lbModDt.Location = new System.Drawing.Point(632, 64);
            this.lbModDt.Name = "lbModDt";
            this.lbModDt.Size = new System.Drawing.Size(72, 21);
            this.lbModDt.TabIndex = 0;
            this.lbModDt.Text = "수정일자";
            this.lbModDt.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cbCategoryName
            // 
            this.cbCategoryName.ComboStyle = Janus.Windows.EditControls.ComboStyle.DropDownList;
            this.cbCategoryName.DisplayMember = "CategoryName";
            this.cbCategoryName.Font = new System.Drawing.Font("굴림체", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.cbCategoryName.Location = new System.Drawing.Point(80, 32);
            this.cbCategoryName.Name = "cbCategoryName";
            this.cbCategoryName.Size = new System.Drawing.Size(152, 21);
            this.cbCategoryName.TabIndex = 11;
            this.cbCategoryName.ValueMember = "CategoryCode";
            this.cbCategoryName.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            // 
            // ebChannelNo
            // 
            this.ebChannelNo.Font = new System.Drawing.Font("굴림체", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.ebChannelNo.Location = new System.Drawing.Point(352, 8);
            this.ebChannelNo.MaxLength = 120;
            this.ebChannelNo.Name = "ebChannelNo";
            this.ebChannelNo.Size = new System.Drawing.Size(104, 21);
            this.ebChannelNo.TabIndex = 9;
            this.ebChannelNo.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
            this.ebChannelNo.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            // 
            // lbChannelNo
            // 
            this.lbChannelNo.Font = new System.Drawing.Font("굴림체", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lbChannelNo.Location = new System.Drawing.Point(280, 8);
            this.lbChannelNo.Name = "lbChannelNo";
            this.lbChannelNo.Size = new System.Drawing.Size(72, 21);
            this.lbChannelNo.TabIndex = 0;
            this.lbChannelNo.Text = "채널번호 ";
            this.lbChannelNo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnDelete
            // 
            this.btnDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnDelete.BackColor = System.Drawing.SystemColors.Window;
            this.btnDelete.Enabled = false;
            this.btnDelete.Location = new System.Drawing.Point(128, 95);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(104, 24);
            this.btnDelete.TabIndex = 18;
            this.btnDelete.Text = "삭 제";
            this.btnDelete.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnAdd
            // 
            this.btnAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnAdd.BackColor = System.Drawing.SystemColors.Window;
            this.btnAdd.Enabled = false;
            this.btnAdd.Location = new System.Drawing.Point(240, 95);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(104, 24);
            this.btnAdd.TabIndex = 19;
            this.btnAdd.Text = "추 가";
            this.btnAdd.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnSave.BackColor = System.Drawing.SystemColors.Window;
            this.btnSave.Enabled = false;
            this.btnSave.Location = new System.Drawing.Point(16, 95);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(104, 24);
            this.btnSave.TabIndex = 17;
            this.btnSave.Text = "저 장";
            this.btnSave.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // cbMediaName
            // 
            this.cbMediaName.ComboStyle = Janus.Windows.EditControls.ComboStyle.DropDownList;
            this.cbMediaName.DisplayMember = "MediaName";
            this.cbMediaName.Font = new System.Drawing.Font("굴림체", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.cbMediaName.Location = new System.Drawing.Point(80, 8);
            this.cbMediaName.Name = "cbMediaName";
            this.cbMediaName.Size = new System.Drawing.Size(152, 21);
            this.cbMediaName.TabIndex = 8;
            this.cbMediaName.ValueMember = "MediaCode";
            this.cbMediaName.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            // 
            // lbMediaName
            // 
            this.lbMediaName.Font = new System.Drawing.Font("굴림체", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lbMediaName.Location = new System.Drawing.Point(8, 8);
            this.lbMediaName.Name = "lbMediaName";
            this.lbMediaName.Size = new System.Drawing.Size(72, 21);
            this.lbMediaName.TabIndex = 12;
            this.lbMediaName.Text = "매체";
            this.lbMediaName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lbSeriesNo
            // 
            this.lbSeriesNo.Font = new System.Drawing.Font("굴림체", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lbSeriesNo.Location = new System.Drawing.Point(488, 64);
            this.lbSeriesNo.Name = "lbSeriesNo";
            this.lbSeriesNo.Size = new System.Drawing.Size(72, 21);
            this.lbSeriesNo.TabIndex = 13;
            this.lbSeriesNo.Text = "시리즈편수 ";
            this.lbSeriesNo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lbCategoryName
            // 
            this.lbCategoryName.Font = new System.Drawing.Font("굴림체", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lbCategoryName.Location = new System.Drawing.Point(8, 32);
            this.lbCategoryName.Name = "lbCategoryName";
            this.lbCategoryName.Size = new System.Drawing.Size(72, 20);
            this.lbCategoryName.TabIndex = 14;
            this.lbCategoryName.Text = "카테고리";
            this.lbCategoryName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lbGenreName
            // 
            this.lbGenreName.Font = new System.Drawing.Font("굴림체", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lbGenreName.Location = new System.Drawing.Point(488, 8);
            this.lbGenreName.Name = "lbGenreName";
            this.lbGenreName.Size = new System.Drawing.Size(72, 21);
            this.lbGenreName.TabIndex = 15;
            this.lbGenreName.Text = "장르";
            this.lbGenreName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lbTitle
            // 
            this.lbTitle.Font = new System.Drawing.Font("굴림체", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lbTitle.Location = new System.Drawing.Point(8, 64);
            this.lbTitle.Name = "lbTitle";
            this.lbTitle.Size = new System.Drawing.Size(72, 21);
            this.lbTitle.TabIndex = 18;
            this.lbTitle.Text = "컨텐츠명";
            this.lbTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
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
            // imageList
            // 
            this.imageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList.ImageStream")));
            this.imageList.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList.Images.SetKeyName(0, "");
            this.imageList.Images.SetKeyName(1, "");
            this.imageList.Images.SetKeyName(2, "");
            // 
            // ChannelSetControl
            // 
            this.Controls.Add(this.uiPanelUsers);
            this.Name = "ChannelSetControl";
            this.Size = new System.Drawing.Size(1010, 677);
            this.Load += new System.EventHandler(this.ChannelSetControl_Load);
            ((System.ComponentModel.ISupportInitialize)(this.uiPM)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelUsers)).EndInit();
            this.uiPanelUsers.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelUsersSearch)).EndInit();
            this.uiPanelUsersSearch.ResumeLayout(false);
            this.uiPanelUsersSearchContainer.ResumeLayout(false);
            this.pnlSearch.ResumeLayout(false);
            this.pnlSearch.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelChannelSet)).EndInit();
            this.uiPanelChannelSet.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.uiPanel1)).EndInit();
            this.uiPanel1.ResumeLayout(false);
            this.uiPanel1Container.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdExCategenList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dvCateGen)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.channelSetDs)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanel2)).EndInit();
            this.uiPanel2.ResumeLayout(false);
            this.uiPanel2Container.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdExChannelSetList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dvChannelSet)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelUserDetail)).EndInit();
            this.uiPanelUserDetail.ResumeLayout(false);
            this.uiPanelUserDetailContainer.ResumeLayout(false);
            this.pnlUserDetail.ResumeLayout(false);
            this.pnlUserDetail.PerformLayout();
            this.ResumeLayout(false);

		}
		#endregion

		#region 컨트롤 로드
		private void ChannelSetControl_Load(object sender, System.EventArgs e)
		{		
			// 데이터관리용 객체생성
			dt = ((DataView)grdExCategenList.DataSource).Table;
			dtChild  = ((DataView)grdExChannelSetList.DataSource).Table;

			cm = (CurrencyManager) this.BindingContext[grdExCategenList.DataSource]; 
			cm.PositionChanged += new System.EventHandler(OnGrdRowChanged); 

			cmChild = (CurrencyManager) this.BindingContext[grdExChannelSetList.DataSource]; 
			cmChild.PositionChanged += new System.EventHandler(OnGrdRowDetailChanged); 		

			// 컨트롤 초기화
			InitControl();			
		}

		#endregion

		#region 컨트롤 초기화
		private void InitControl()
		{
			ProgressStart();
			InitCombo();
			InitCombo_Category();
			InitCombo_Genre();
			InitCombo_Level();

			// 조회권한 검사
			if(menu.CanRead(MenuCode))
			{
				canRead = true;
				SearchChannelSet();
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
			SetTextReadonly();
			ProgressStop();
		}

		private void InitCombo()
		{			
			MediaCodeModel mediacodeModel = new MediaCodeModel();		
			new MediaCodeManager(systemModel, commonModel).GetMediaCodeList(mediacodeModel);
			
			if (mediacodeModel.ResultCD.Equals("0000"))
			{
				// 데이터셋에 셋팅
				Utility.SetDataTable(channelSetDs.Medias, mediacodeModel.MediaCodeDataSet);				
			}

			// 상세조회 콤보
			// 상세정보의 콤보는 Dataset을 데이터소스로 가진다.

			// 검색조건의 콤보
			this.cbSearchMediaName.Items.Clear();
			
			// 콤보박스에 셋트할 코드목록을 담을 Item배열을 선언
			Janus.Windows.EditControls.UIComboBoxItem[] comboItems = new Janus.Windows.EditControls.UIComboBoxItem[mediacodeModel.ResultCnt + 1];

			comboItems[0] = new Janus.Windows.EditControls.UIComboBoxItem("매체선택","00");
			
			for(int i=0;i<mediacodeModel.ResultCnt;i++)
			{
				DataRow row = channelSetDs.Medias.Rows[i];

				string val = row["MediaCode"].ToString();
				string txt = row["MediaName"].ToString();
				comboItems[i+1] = new Janus.Windows.EditControls.UIComboBoxItem(txt,val);
			}
			// 콤보에 셋트
			this.cbSearchMediaName.Items.AddRange(comboItems);
			this.cbSearchMediaName.SelectedIndex = 0;

			Application.DoEvents();
		}
		
		public void InitCombo_Category()
		{
			// 코드에서 보안레벨을 조회한다.
			ChannelSetModel channelSetModel = new ChannelSetModel();
			new ChannelSetManager(systemModel, commonModel).GetCategoryList(channelSetModel);
			
			if (channelSetModel.ResultCD.Equals("0000"))
			{
				// 데이터셋에 셋팅
				Utility.SetDataTable(channelSetDs.Categorys, channelSetModel.CategoryDataSet);	
			}

			// 하단 매체 콤보의 값 초기화. 
			this.cbSearchCategoryName.Items.Clear();
            
			// 콤보박스에 셋트할 코드목록을 담을 Item배열을 선언
			Janus.Windows.EditControls.UIComboBoxItem[] comboItems = new Janus.Windows.EditControls.UIComboBoxItem[channelSetModel.ResultCnt + 1];
            
			comboItems[0] = new Janus.Windows.EditControls.UIComboBoxItem("카테고리선택","00");
			
			for(int i=0;i<channelSetModel.ResultCnt;i++)
			{
				DataRow row = channelSetDs.Categorys.Rows[i];

				string val = row["CategoryCode"].ToString();
				string txt = row["CategoryName"].ToString();
				comboItems[i+1] = new Janus.Windows.EditControls.UIComboBoxItem(txt,val);
			}


			// 하단 콤보에 셋트
			this.cbSearchCategoryName.Items.AddRange(comboItems);
			this.cbSearchCategoryName.SelectedIndex = 0;

			Application.DoEvents();
		}

		public void InitCombo_Genre()
		{
			// 코드에서 보안레벨을 조회한다.
			ChannelSetModel channelSetModel = new ChannelSetModel();
			new ChannelSetManager(systemModel, commonModel).GetGenreList(channelSetModel);
			
			if (channelSetModel.ResultCD.Equals("0000"))
			{
				// 데이터셋에 셋팅
				Utility.SetDataTable(channelSetDs.Genres, channelSetModel.GenreDataSet);	
			}

			// 하단 매체 콤보의 값 초기화. 
			this.cbSearchGenreName.Items.Clear();
            
			// 콤보박스에 셋트할 코드목록을 담을 Item배열을 선언
			Janus.Windows.EditControls.UIComboBoxItem[] comboItems = new Janus.Windows.EditControls.UIComboBoxItem[channelSetModel.ResultCnt + 1];
            
			comboItems[0] = new Janus.Windows.EditControls.UIComboBoxItem("장르선택","00");
			
			for(int i=0;i<channelSetModel.ResultCnt;i++)
			{
				DataRow row = channelSetDs.Genres.Rows[i];

				string val = row["GenreCode"].ToString();
				string txt = row["GenreName"].ToString();
				comboItems[i+1] = new Janus.Windows.EditControls.UIComboBoxItem(txt,val);
			}


			// 하단 콤보에 셋트
			this.cbSearchGenreName.Items.AddRange(comboItems);
			this.cbSearchGenreName.SelectedIndex = 0;

			Application.DoEvents();

		}        

		private void InitCombo_Level()
		{			
			if(commonModel.UserLevel=="20")
			{
				// 콤보픽스						
				cbSearchMediaName.SelectedValue = commonModel.MediaCode;			
				cbSearchMediaName.ReadOnly = true;										
			}
			else
			{
				for(int i=0;i < channelSetDs.Medias.Rows.Count;i++)
				{
					DataRow row = channelSetDs.Medias.Rows[i];					
					if(row["MediaCode"].ToString().Equals(FrameSystem._HANATV.ToString()))
					{
						cbSearchMediaName.SelectedValue = FrameSystem._HANATV; // 하나TV를 기본값으로 한다.	 		
						break;															
					}
					else
					{
						cbSearchMediaName.SelectedValue="00";
					}
				}	
			}
			Application.DoEvents();
		}

		private void InitButton()
		{
			if(canRead)  
			{
				btnSearch.Enabled = true;
				btnExcel.Enabled = true;
			}
			if(canCreate) btnAdd.Enabled    = true;
			
			if(ebChannelNo.Text.Trim().Length > 0) 
			{
				if(canDelete) btnDelete.Enabled = true;
				if(canUpdate) btnSave.Enabled   = true;
			}
			if(IsAdding)
			{
				if(canCreate) cbMediaName.Enabled    = true;
			}
			Application.DoEvents();
		}

		private void DisableButton()
		{
			btnSearch.Enabled = false;
			btnExcel.Enabled = false;
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
                if (grdExCategenList.RecordCount > 0)
                {
                    SetCategenDetailText();
                }
                SetTextReadonly();
                InitButton();
            }
		}

		
		private void OnGrdRowDetailChanged(object sender, System.EventArgs e) 
		{		
			if(grdExChannelSetList.RecordCount > 0 )
			{
				SetChannelSetDetailText();

                try
                {

                    int curRow = cmChild.Position;
                    //int curRow1 = cm.Position;		

                    if (curRow >= 0)
                    {
                        mediaCode_old = dtChild.Rows[curRow]["MediaCode"].ToString();
                        categoryCode_old = dtChild.Rows[curRow]["CategoryCode"].ToString();
                        genreCode_old = dtChild.Rows[curRow]["GenreCode"].ToString();
                        channelNo_old = dtChild.Rows[curRow]["ChannelNo"].ToString();
                    }		


                }
                catch
                {
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
			ProgressStart();
			if(cbSearchMediaName.SelectedValue.ToString() == "00") 
			{
				FrameSystem.showMsgForm("채널정보검색 오류",new string[] {"", "매체을 선택하여 주세요.", "" });
				return;
			}
			ReSetChannelSetDetailText();
			ReSetGridData();
			DisableButton();
			SetTextReadonly();			
			SearchChannelSet();			
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

			//btnDetailAdd.Enabled = true;
			IsAdding = true;

			ResetTextReadonly();
			ReSetChannelSetDetailText();
			//ReSetGridData();

			ebChannelNo.Focus();
		}

		/// <summary>
		/// 저장버튼 클릭
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnSave_Click(object sender, System.EventArgs e)
		{			
			SaveChannelSetDetail();		
			//ReSetGridData();
			//SearchChannelSetDetail();			
		}
		/// <summary>
		/// 삭제버튼 클릭
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnDelete_Click(object sender, System.EventArgs e)
		{			
			DeleteChannelSet();		
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
				SearchChannelSet();
			}
		}

		#endregion

		#region 처리메소드

		/// <summary>
		/// 채널목록 조회
		/// </summary>
		private void SearchChannelSet()
		{
            IsSearching = true;

			StatusMessage("채널 정보를 조회합니다.");
//			if(cbSearchMediaName.SelectedItem.Value.Equals("00")) 
//			{
//				MessageBox.Show("매체를 선택하여 주시기 바랍니다.","채널구성 조회", 
//					MessageBoxButtons.OK, MessageBoxIcon.Information );
//				return;
//			}

			try
			{
				channelSetModel.Init();
				// 데이터모델에 전송할 내용을 셋트한다.				
				if(IsNewSearchKey)
				{
					channelSetModel.SearchKey = "";
				}
				else
				{
					channelSetModel.SearchKey  = ebSearchKey.Text;
				}

				channelSetModel.SearchMediaName = cbSearchMediaName.SelectedItem.Value.ToString();
				channelSetModel.SearchCategoryName = cbSearchCategoryName.SelectedItem.Value.ToString();
				channelSetModel.SearchGenreName = cbSearchGenreName.SelectedItem.Value.ToString();

				ReSetChannelSetDetailText();

				// 채널목록조회 서비스를 호출한다.
				new ChannelSetManager(systemModel,commonModel).GetCategenList(channelSetModel);

				if (channelSetModel.ResultCD.Equals("0000"))
				{
					Utility.SetDataTable(channelSetDs.Categens, channelSetModel.ChannelSetDataSet);
					StatusMessage(channelSetModel.ResultCnt + "건의 채널 정보가 조회되었습니다.");
					//SearchCategenDetail();
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
            finally
            {
                IsSearching = false; // 조회중 Flag 리셋
            }
		}
     
		/// <summary>
		/// 채널목록 조회
		/// </summary>
		private void SearchCategenDetail()
		{
			StatusMessage("채널 디테일 정보를 조회합니다.");

			try
			{				
                channelSetModel.Init();
				int curRow = cm.Position;          

				if(curRow < 0) return;
							
				//channelSetModel.ChannelNo = dt.Rows[curRow]["ChannelNo"].ToString();
			
				channelSetModel.MediaCode = dt.Rows[curRow]["MediaCode"].ToString();
				channelSetModel.CategoryCode = dt.Rows[curRow]["CategoryCode"].ToString();
				channelSetModel.GenreCode = dt.Rows[curRow]["GenreCode"].ToString();
			
				// 채널목록조회 서비스를 호출한다.
				new ChannelSetManager(systemModel,commonModel).GetChannelSetList(channelSetModel);

				if (channelSetModel.ResultCD.Equals("0000"))
				{
					Utility.SetDataTable(channelSetDs.ChannelSets, channelSetModel.ChannelSetDataSet);				
					StatusMessage(channelSetModel.ResultCnt + "건의 채널 정보가 조회되었습니다.");
					SetChannelSetDetailText();
				}
				//btnDetailAdd.Enabled = true;
            
				if(channelSetModel.ResultCnt > 0)
				{
					//btnDetailDelete.Enabled = true;   
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

		private void SearchChannelSetDetail()
		{
			StatusMessage("채널 디테일 정보를 조회합니다.");

			try
			{
               
				// 데이터모델에 전송할 내용을 셋트한다.
                
				int curRow = cmChild.Position;
             
				ebChannelNo.Text  = dtChild.Rows[curRow]["ChannelNo"].ToString();
                ebTotalSeries.Text = dtChild.Rows[curRow]["TotalSeries"].ToString();
				
				// 채널목록조회 서비스를 호출한다.
				new ChannelSetManager(systemModel,commonModel).GetChannelSetList(channelSetModel);

				if (channelSetModel.ResultCD.Equals("0000"))
				{
					Utility.SetDataTable(channelSetDs.ChannelSets, channelSetModel.ChannelSetDataSet);
					StatusMessage(channelSetModel.ResultCnt + "건의 채널 정보가 조회되었습니다.");					
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
		/// 채널상세정보 저장
		/// </summary>
		private void SaveChannelSetDetail()
		{
			//IsAdding = true;

			StatusMessage("채널 정보를 저장합니다.");                        
                      
			if(cbMediaName.SelectedValue.ToString().Length == 0) 
			{
				MessageBox.Show("매체명이 입력되지 않았습니다.","채널구성 저장", 
					MessageBoxButtons.OK, MessageBoxIcon.Information );
				return;								
			}
			if(cbCategoryName.SelectedValue.ToString().Length == 0) 
			{
				MessageBox.Show("카테고리가 입력되지 않았습니다.","채널구성 저장", 
					MessageBoxButtons.OK, MessageBoxIcon.Information );
				return;					
			}
			if(ebGenreName.Text.ToString().Length == 0) 
			{
				MessageBox.Show("장르가 입력되지 않았습니다.","채널구성 저장", 
					MessageBoxButtons.OK, MessageBoxIcon.Information );
				return;					
			}
			if(ebChannelNo.Text.Trim().Length == 0) 
			{
				MessageBox.Show("채널이 입력되지 않았습니다.","채널구성 저장", 
					MessageBoxButtons.OK, MessageBoxIcon.Information );
					ResetPop();
				return;					
			}
			
                        
			try
			{
				//저장 전에 모델을 초기화 해준다.
				channelSetModel.Init();
				// 데이터모델에 전송할 내용을 셋트한다.
				channelSetModel.MediaCode      = cbMediaName.SelectedValue.ToString();
				channelSetModel.CategoryCode      = cbCategoryName.SelectedValue.ToString();
				channelSetModel.GenreCode      = genreCode;
				channelSetModel.ChannelNo      = ebChannelNo.Text;		

				channelSetModel.MediaCode_old = mediaCode_old;
				channelSetModel.CategoryCode_old = categoryCode_old;
				channelSetModel.GenreCode_old = genreCode_old;
				channelSetModel.ChannelNo_old = channelNo_old;
					
				//channelSetModel.ChannelSetDataSet = channelSetDs.Copy();                         

				// 채널 상세정보 저장 서비스를 호출한다.
				if (IsAdding)
				{
					new ChannelSetManager(systemModel,commonModel).SetChannelSetAdd(channelSetModel);
					StatusMessage("채널구성 정보가 추가되었습니다.");
					 IsAdding = false;
					ReSetChannelSetDetailText();
				}
				else
				{
					new ChannelSetManager(systemModel,commonModel).SetChannelSetUpdate(channelSetModel);
					StatusMessage("채널구성 정보가 수정되었습니다.");
				}
				
				 DisableButton();
				SearchChannelSetDetail();
				InitButton();
                        
			}
			catch(FrameException fe)
			{
				FrameSystem.showMsgForm("채널정보 저장오류", new string[] {fe.ErrCode, fe.ResultMsg});
			}
			catch(Exception ex)
			{
				FrameSystem.showMsgForm("채널정보 저장오류",new string[] {"",ex.Message});
			}			
		}

		/// <summary>
		/// 채널정보 삭제
		/// </summary>
		private void DeleteChannelSet()
		{
			StatusMessage("채널 정보를 삭제합니다.");
                        
			if(ebChannelNo.Text.Trim().Length == 0) 
			{
				MessageBox.Show("삭제할 채널 정보가 없습니다.","채널 삭제", 
					MessageBoxButtons.OK, MessageBoxIcon.Information );
				return;
			}
                        
			DialogResult result = MessageBox.Show("해당 채널 정보를 삭제 하시겠습니까?","채널 삭제",
				MessageBoxButtons.YesNo, MessageBoxIcon.Question,
				MessageBoxDefaultButton.Button2);
                        
			if (result == DialogResult.No) return;
                        
			try
			{
				// 데이터모델에 전송할 내용을 셋트한다.				
				channelSetModel.MediaCode       = cbMediaName.SelectedValue.ToString();
				channelSetModel.CategoryCode       = cbCategoryName.SelectedValue.ToString();
				channelSetModel.GenreCode       = genreCode;
				channelSetModel.ChannelNo       = ebChannelNo.Text;
                        
				// 채널 상세정보 저장 서비스를 호출한다.
				new ChannelSetManager(systemModel,commonModel).SetChannelSetDelete(channelSetModel);
                        			
				ReSetChannelSetDetailText();			
				SearchChannelSetDetail();
				StatusMessage("채널 정보가 삭제되었습니다.");			
				//ReSetGridData();
					
				DisableButton();
				SearchChannelSetDetail();
				InitButton();
			}
			catch(FrameException fe)
			{
				FrameSystem.showMsgForm("채널정보 삭제오류", new string[] {fe.ErrCode, fe.ResultMsg});
			}
			catch(Exception ex)
			{
				FrameSystem.showMsgForm("채널정보 삭제오류",new string[] {"",ex.Message});
			}			

		}
		
		/// <summary>
		/// 채널 상세정보의 셋트
		/// </summary>
		private void SetCategenDetailText()
		{
			int curRow = cm.Position;
            
			cbMediaName.ReadOnly         = false;			
			cbCategoryName.ReadOnly         = false;			
			ebGenreName.ReadOnly         = false;											
						
			cbMediaName.BackColor        = Color.White;			
			cbCategoryName.BackColor        = Color.White;			
			ebGenreName.BackColor        = Color.White;						

			SearchCategenDetail();
            
			IsAdding = false;
            
			StatusMessage("준비");
		}

		private void SetChannelSetDetailText()
		{		
			int curRow = cmChild.Position;
			//int curRow1 = cm.Position;		
		
			if(curRow >= 0)
			{
				cbMediaName.SelectedValue = dtChild.Rows[curRow]["MediaCode"].ToString();
				cbCategoryName.SelectedValue = dtChild.Rows[curRow]["CategoryCode"].ToString();
				genreCode = dtChild.Rows[curRow]["GenreCode"].ToString();
				ebGenreName.Text = dtChild.Rows[curRow]["GenreName"].ToString();
				ebChannelNo.Text          = dtChild.Rows[curRow]["ChannelNo"].ToString();
				ebTotalSeries.Text          = dtChild.Rows[curRow]["TotalSeries"].ToString();
				ebTitle.Text          = dtChild.Rows[curRow]["Title"].ToString();
				ebModDt.Text          = dtChild.Rows[curRow]["ModDt"].ToString();

				IsAdding = false;
				ResetTextReadonly();
			}					
				
			StatusMessage("준비");
		}

		private void ReSetChannelSetDetailText()
		{			
			ebChannelNo.Text                 = "";			
			ebTotalSeries.Text                 = "";
			ebTitle.Text                 = "";
			ebModDt.Text                 = "";
		}
        
		private void ReSetGridData()
		{			
			//추가를 하면 채널셋그리드를 리셋한다.
			channelSetDs.ChannelSets.Clear();        
		}
        
		
		/// <summary>
		/// 상세정보 ReadOnly
		/// </summary>
		private void SetTextReadonly()
		{
			cbMediaName.ReadOnly = true;
			cbCategoryName.ReadOnly = true;
			ebGenreName.ReadOnly = true;
			ebTotalSeries.ReadOnly         = true;						
			ebTitle.ReadOnly         = true;
			ebChannelNo.ReadOnly       = true;

			ebChannelNo.BackColor = Color.WhiteSmoke;
			cbMediaName.BackColor = Color.White;
			cbCategoryName.BackColor = Color.White;
			ebGenreName.BackColor = Color.WhiteSmoke;
			ebTotalSeries.BackColor = Color.WhiteSmoke;
			ebTitle.BackColor = Color.WhiteSmoke;
   		}

		/// <summary>
		/// 상세정보 수정가능케
		/// </summary>
		private void ResetTextReadonly()
		{	
			cbMediaName.ReadOnly = false; 
			cbCategoryName.ReadOnly = false; 
			ebGenreName.ReadOnly = false;
			ebTotalSeries.ReadOnly         = false;						
			//ebTitle.ReadOnly         = false;
			ebChannelNo.ReadOnly       = false;

			cbMediaName.BackColor      = Color.White;
			cbCategoryName.BackColor     = Color.White;			
			ebGenreName.BackColor      = Color.White;			
			ebTotalSeries.BackColor   = Color.White;
			//ebTitle.BackColor   = Color.White;					
			ebChannelNo.BackColor   = Color.White;					
		}

		/// <summary>
		/// 상세정보 수정가능케
		/// </summary>
		private void setSeriesNo()
		{
			int i=1;
			Debug.WriteLine("setSeriesNo1:" + channelSetDs.ChannelSets.Rows.Count);
			if(channelSetDs.Tables["ChannelSets"].Rows.Count == 1)
			{
				i--;
			}

			//컨텐츠 목록 갯수가 1개이면 0번으로 입력을 하고 w
			//n개 이면 1부터 입력한다.
			Debug.WriteLine("setSeriesNo21:" + channelSetDs.ChannelSets.Rows.Count);
			Debug.WriteLine("setSeriesNo22:" + channelSetDs.Tables["ChannelSets"].Rows.Count);

			foreach (DataRow row in channelSetDs.Tables["ChannelSets"].Rows)
			{
             
				row["SeriesNo"] = i;
				i++;
			}

			//전체 체크박스 표시 했을때 0으로 바뀌지 않아서 강제적 셋팅
			//추후 방법이 해결방안 찾으면 수정
			Debug.WriteLine("setSeriesNo3:" + channelSetDs.ChannelSets.Rows.Count);
            
			if(channelSetDs.Tables["ChannelSets"].Rows.Count ==1)
			{
				DataRow row = channelSetDs.Tables["ChannelSets"].Rows[0];
				row["SeriesNo"] = 0;
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

		#region 엑셀 출력
		/// <summary>
		/// 엑셀 생성
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnExcel_Click(object sender, System.EventArgs e)
		{	

			Excel.Application xlApp= null;
			Excel._Workbook   oWB = null;
			Excel._Worksheet  oSheet = null;
			Excel.Range       oRng = null;
			
			try
			{	
				int ColMax  = 4; // 컬럼수   				

				int TitleRow  = 1;						
				int HeaderRow = 2;				
				string StartCol = "A";
				string EndCol   = "";
				string TitleCol = "D";				
				int CondCount = 0;
				int HeaderCount = 0;

				// 마지막 컬럼의 인덱스문자
				EndCol = GetColumnIndex(ColMax);

				xlApp = new Excel.Application();
				oWB = (Excel._Workbook)(xlApp.Workbooks.Add(Missing.Value));
				oSheet = (Excel._Worksheet)oWB.ActiveSheet;


				// 타이틀 작성
				oSheet.Cells[TitleRow,1] = "광고내역목록";
				oRng = oSheet.get_Range(StartCol+Convert.ToString(TitleRow), TitleCol+Convert.ToString(TitleRow));
				oRng.Merge(true);
				oRng.Font.Bold = true;
				oRng.Font.Size = 16;
				oRng.VerticalAlignment   = Excel.XlVAlign.xlVAlignCenter;			
				oRng.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;		
												
				CondCount++;
								
				// 헤더 정보 작성
				HeaderCount = 1;				
				//				oRng = oSheet.get_Range(GetColumnIndex(1)+Convert.ToString(HeaderRow), GetColumnIndex(1)+Convert.ToString(HeaderRow));
				//				oRng.Merge(true);
				//				HeaderCount++;

				oSheet.Cells[HeaderRow,HeaderCount++] = "카테고리명";
				oSheet.Cells[HeaderRow,HeaderCount++] = "장르명";
				oSheet.Cells[HeaderRow,HeaderCount++] = "채널번호";
				oSheet.Cells[HeaderRow,HeaderCount++] = "컨텐츠명";
																
				oRng = oSheet.get_Range(StartCol+Convert.ToString(HeaderRow), EndCol+Convert.ToString(HeaderRow)); // 헤더의 범위
				oRng.Font.Bold           = true;							// 폰트 굵게
				oRng.VerticalAlignment   = Excel.XlVAlign.xlVAlignCenter;	// 세로중앙정렬
				oRng.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;	// 가로중앙정렬	 
				oRng.Interior.Color      = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.CornflowerBlue);   //셀 배경색 
				oRng.Font.Color          = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.White);	  //텍스트색			

				string[,] items = new string[channelSetDs.Tables[0].Rows.Count, 4];
				// 데이터 추출
				for (int inx =0; inx < channelSetDs.Tables[0].Rows.Count; inx++)
				{
					items[inx, 0] = channelSetDs.Tables[0].Rows[inx]["CategoryName"].ToString();					
					items[inx, 1] = channelSetDs.Tables[0].Rows[inx]["genreName"].ToString();					
					items[inx, 2] = channelSetDs.Tables[0].Rows[inx]["ChannelNo"].ToString();					
					items[inx, 3] = channelSetDs.Tables[0].Rows[inx]["Title"].ToString();															
										
				}
				oSheet.get_Range("A3", "D"+Convert.ToString((items.Length/4)+2)).set_Value(Missing.Value, items);

				oRng = oSheet.get_Range(StartCol+Convert.ToString(HeaderRow), EndCol+Convert.ToString(channelSetDs.Tables[0].Rows.Count));	// 데이터의 범위
				oRng.EntireColumn.AutoFit();					// 데이터의 크기에 셀의 가로크기 맞춤

				oRng.Borders.LineStyle =  Excel.XlLineStyle.xlContinuous;	// 테두리속성 아래에 실선
				oRng.Borders.Weight    = Excel.XlBorderWeight.xlThin;		// 테두리속성 아래에 가는선
				
				oRng = oSheet.get_Range("B2", "Q2");
				oRng.EntireColumn.AutoFit();
				
				xlApp.Visible = true;
				xlApp.UserControl = true;				
			}
			catch(Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
		}

		private string GetColumnIndex(int ColCount)
		{
			string[] ColName = {"Z","A","B","C","D","E","F","G","H","I","J","K","L","M","N","O","P","Q","R","S","T","U","V","W","X","Y"};

			string ColumnIndex;

			// 26보다 크면
			if(ColCount > ColName.Length)
			{
				// 2자리 인덱스문자 26 => Z;  27->AA
				ColumnIndex = ColName[(int)(Math.Floor((float)(ColCount/ColName.Length)))] + ColName[(int)(Math.Floor((float)(ColCount%ColName.Length)))];
			}
			else
			{
				ColumnIndex = ColName[(int)(Math.Floor((float)(ColCount%ColName.Length)))];
			}

			return ColumnIndex;
		}

		#endregion


		private void btnDetailDelete_Click(object sender, System.EventArgs e)
		{
			try
			{
				int i=0;
              
				//grid의 체크 플래그를 dtChild에 적용해준다.
				foreach (Janus.Windows.GridEX.GridEXRow gr in grdExChannelSetList.GetRows())
				{
					if(gr.Cells["CheckYn"].Value.ToString().Equals("True"))
					{
						dtChild.Rows[i]["CheckYn"]="True";
					}
					i++;
				} 
				Debug.WriteLine("----------------------------------------------");
				//Debug.WriteLine("Delect_Click1:" + channelSetDs.ChannelSets.Rows.Count);

				//grid의 체크 플래그가 "True"인것만 삭제. 
				DataRow[] deleteRows = channelSetDs.ChannelSets.Select("CheckYn='True'");
                

				Debug.WriteLine("Delect_Click2:" + channelSetDs.ChannelSets.Rows.Count);
				for( int j = 0; j < deleteRows.Length;j++)
				{
					deleteRows[j].Delete();
				}
				Debug.WriteLine("Delect_Click3:" + channelSetDs.ChannelSets.Rows.Count);
				setSeriesNo();
				Debug.WriteLine("Delect_Click4:" + channelSetDs.ChannelSets.Rows.Count);
			}
			catch(Exception ex)
			{
				Debug.WriteLine(ex.Message);
			}
		}


		#region 외부노출 메소드
		/// <summary>
		/// 선택된 Row들을 입력시킴
		/// </summary>
		/// <param name="dtc"></param>
		public void adOn_AddContent(DataSet contentsDs)
		{
            
			//모달창에서 받아온 DataSet을 Loop만큼 돌려서 체크된 값을
			//인서트 시킴
			for(int i=0;i< contentsDs.Tables["ChannelSets"].Rows.Count;i++)
			{

				DataRow row = contentsDs.Tables["ChannelSets"].Rows[i];

				if(row["CheckYn"].ToString().Equals("True"))
				{

                
					DataRow[] foundRows = dtChild.Select("ContentId = '" + row["ContentId"].ToString() + "'");
                    
					if( foundRows.Length == 0 )
					{
						DataRow newRow = dtChild.NewRow();

						newRow[0] = "False";
						newRow[1] = row["ContentId"].ToString();
						newRow[2] = row["Title"].ToString();
             
						dtChild.Rows.Add(newRow);
						dtChild.Rows.Add(new Object[] {1, "False",2,row["ContentId"].ToString(),3,row["Title"].ToString()});

						newRow = null;
					}
					if( null != foundRows ) foundRows = null;
				}
				if( null != row ) row = null;


			}
			//컨텐츠리스트가 0보다 클경우에 삭제 버튼 활성화
			if(contentsDs.Tables["ChannelSets"].Rows.Count >0)
			{
				//btnDetailDelete.Enabled = true;
			}
			setSeriesNo();
		}

		#endregion

		//이 메소드가 실행이 되면 해당 필드의 팝업이 호출된다.
		private void ResetPop()
		{			
			ChannelNoPopForm channelNoPopForm = new ChannelNoPopForm(this);
			channelNoPopForm.ShowDialog();
			channelNoPopForm.Dispose();
			channelNoPopForm = null;
		}

		private void btnChannelNo_Click(object sender, System.EventArgs e)
		{
			// 채널 목록 검색 팝업 띠우기
			ChannelNoPopForm channelNoPopForm = new ChannelNoPopForm(this);
			channelNoPopForm.ShowDialog();
			channelNoPopForm.Dispose();
			channelNoPopForm = null;
		}	
		
		public string ChannelNo
		{				
			set
			{
				this.ebChannelNo.Text = value;
			}			
		}
		
		private void btnGenreName_Click(object sender, System.EventArgs e)
		{
			// 채널 목록 검색 팝업 띠우기
			GenrePopForm GenrePopForm = new GenrePopForm(this);
			GenrePopForm.ShowDialog();
			GenrePopForm.Dispose();
			GenrePopForm = null;
		}

		private void ebTotalSeries_Click(object sender, System.EventArgs e)
		{
		
		}

		public string GenreCode
		{
			set
			{
				this.genreCode = value;
			}
		}

		public string GenreName
		{				
			set
			{
				this.ebGenreName.Text = value;
			}			
		}
	}
}