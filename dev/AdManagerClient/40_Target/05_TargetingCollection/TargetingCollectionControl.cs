// ===============================================================================
// TargetCollectionControl for Charites Project
//
// TargetCollectionControl.cs
//
// 타겟군정보관리 컨드롤을 정의합니다. 
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
using AdManagerClient.Common.Args;

using Janus.Windows.GridEX;

namespace AdManagerClient
{
	/// <summary>
	/// 타겟군관리 컨트롤
	/// </summary>
    public class TargetingCollectionControl : System.Windows.Forms.UserControl, IUserControl
	{
		#region 이벤트핸들러
		public event StatusEventHandler 			StatusEvent;			// 상태이벤트 핸들러
		public event ProgressEventHandler 			ProgressEvent;			// 처리중이벤트 핸들러
		#endregion
			
		#region 타겟군정의 객체 및 변수

		// 시스템 정보 : 화면공통
		private SystemModel   systemModel   = FrameSystem.oSysModel;
		private CommonModel   commonModel   = FrameSystem.oComModel;
		private Logger        log           = FrameSystem.oLog;
		private MenuPower     menu          = FrameSystem.oMenu;

		// 메뉴코드 : 보안이 필요한 화면에 필요함
		public string        menuCode		= "";
		public string        keyCnt		= "";

        // Key 데이터
        string keyCollectionCode = string.Empty;
        string collectionCode = string.Empty;
        string collectionName = string.Empty;

		// 사용할 정보모델
        TargetingCollectionModel model = new TargetingCollectionModel();	// 타겟군정보모델

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

        private Janus.Windows.UI.Dock.UIPanelGroup uiPanelTargeting;
        private Janus.Windows.UI.Dock.UIPanel uiPanelHome;
        private Janus.Windows.UI.Dock.UIPanelInnerContainer uiPanel1Container;
        private Janus.Windows.UI.Dock.UIPanel uiPanelOAP;
        private Janus.Windows.UI.Dock.UIPanelInnerContainer uiPanel2Container;
        private Janus.Windows.UI.Dock.UIPanel uiPanelCM;
        private Janus.Windows.UI.Dock.UIPanelInnerContainer uiPanel3Container;
        private Panel panel3;
        private Panel panel5;
        private Janus.Windows.EditControls.UIButton btnDeleteCM;
        private Janus.Windows.EditControls.UIButton btnAddCM;
        private Panel panel1;
        private Panel panel2;
        private Janus.Windows.EditControls.UIButton btnDeleteOAP;
        private Janus.Windows.EditControls.UIButton btnAddOAP;
        private Panel panel4;
        private Panel panel6;
        private Janus.Windows.EditControls.UIButton btnDeleteHome;
        private Janus.Windows.EditControls.UIButton btnAddHome;
        private TargetingCollectionDs targetingCollectionDs;
        private DataView dvCollections;
        private DataView dvTagetingCM;
        private DataView dvTargetingOAP;
        private DataView dvTargetingHome;
        private Janus.Windows.GridEX.GridEX grdExTargetOAP;
        private Janus.Windows.GridEX.GridEX grdExTargetHome;
        private Janus.Windows.GridEX.GridEX grdExTargetCM;
        private Janus.Windows.EditControls.UICheckBox chkSearchNonuseYn;
		private string        cnt = null;

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
		private Janus.Windows.UI.Dock.UIPanel uiPanelMediaRapList;
        private Janus.Windows.UI.Dock.UIPanelInnerContainer uiPanelMediaRapListContainer;
		private Janus.Windows.UI.Dock.UIPanel uiPanelMediaRapsSearch;
		private Janus.Windows.UI.Dock.UIPanelInnerContainer uiPanelMediaRapsSearchContainer;
		private Janus.Windows.EditControls.UIButton uiButton2;
		private Janus.Windows.EditControls.UIButton uiButton1;
		private Janus.Windows.UI.Dock.UIPanelGroup uiPanelMediaRaps;
		private System.Windows.Forms.Panel pnlSearch;
		private Janus.Windows.GridEX.EditControls.EditBox ebSearchKey;
		private Janus.Windows.EditControls.UIButton btnSearch;
        private System.Windows.Forms.Panel pnlMediaRapDetail;
        private Janus.Windows.GridEX.GridEX grdExCollectionList;
		private System.ComponentModel.IContainer components;

		public TargetingCollectionControl()
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
            Janus.Windows.GridEX.GridEXLayout grdExCollectionList_DesignTimeLayout = new Janus.Windows.GridEX.GridEXLayout();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TargetingCollectionControl));
            Janus.Windows.GridEX.GridEXLayout grdExTargetCM_DesignTimeLayout = new Janus.Windows.GridEX.GridEXLayout();
            Janus.Windows.GridEX.GridEXLayout grdExTargetOAP_DesignTimeLayout = new Janus.Windows.GridEX.GridEXLayout();
            Janus.Windows.GridEX.GridEXLayout grdExTargetHome_DesignTimeLayout = new Janus.Windows.GridEX.GridEXLayout();
            this.uiPM = new Janus.Windows.UI.Dock.UIPanelManager(this.components);
            this.uiPanelMediaRaps = new Janus.Windows.UI.Dock.UIPanelGroup();
            this.uiPanelMediaRapsSearch = new Janus.Windows.UI.Dock.UIPanel();
            this.uiPanelMediaRapsSearchContainer = new Janus.Windows.UI.Dock.UIPanelInnerContainer();
            this.pnlSearch = new System.Windows.Forms.Panel();
            this.chkSearchNonuseYn = new Janus.Windows.EditControls.UICheckBox();
            this.ebSearchKey = new Janus.Windows.GridEX.EditControls.EditBox();
            this.btnSearch = new Janus.Windows.EditControls.UIButton();
            this.uiPanelMediaRapList = new Janus.Windows.UI.Dock.UIPanel();
            this.uiPanelMediaRapListContainer = new Janus.Windows.UI.Dock.UIPanelInnerContainer();
            this.grdExCollectionList = new Janus.Windows.GridEX.GridEX();
            this.dvCollections = new System.Data.DataView();
            this.targetingCollectionDs = new AdManagerClient.TargetingCollectionDs();
            this.uiPanelTargeting = new Janus.Windows.UI.Dock.UIPanelGroup();
            this.uiPanelHome = new Janus.Windows.UI.Dock.UIPanel();
            this.uiPanel1Container = new Janus.Windows.UI.Dock.UIPanelInnerContainer();
            this.panel3 = new System.Windows.Forms.Panel();
            this.grdExTargetCM = new Janus.Windows.GridEX.GridEX();
            this.dvTagetingCM = new System.Data.DataView();
            this.panel5 = new System.Windows.Forms.Panel();
            this.btnDeleteCM = new Janus.Windows.EditControls.UIButton();
            this.btnAddCM = new Janus.Windows.EditControls.UIButton();
            this.uiPanelOAP = new Janus.Windows.UI.Dock.UIPanel();
            this.uiPanel2Container = new Janus.Windows.UI.Dock.UIPanelInnerContainer();
            this.panel1 = new System.Windows.Forms.Panel();
            this.grdExTargetOAP = new Janus.Windows.GridEX.GridEX();
            this.dvTargetingOAP = new System.Data.DataView();
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnDeleteOAP = new Janus.Windows.EditControls.UIButton();
            this.btnAddOAP = new Janus.Windows.EditControls.UIButton();
            this.uiPanelCM = new Janus.Windows.UI.Dock.UIPanel();
            this.uiPanel3Container = new Janus.Windows.UI.Dock.UIPanelInnerContainer();
            this.panel4 = new System.Windows.Forms.Panel();
            this.grdExTargetHome = new Janus.Windows.GridEX.GridEX();
            this.dvTargetingHome = new System.Data.DataView();
            this.panel6 = new System.Windows.Forms.Panel();
            this.btnDeleteHome = new Janus.Windows.EditControls.UIButton();
            this.btnAddHome = new Janus.Windows.EditControls.UIButton();
            this.pnlMediaRapDetail = new System.Windows.Forms.Panel();
            this.uiButton1 = new Janus.Windows.EditControls.UIButton();
            this.uiButton2 = new Janus.Windows.EditControls.UIButton();
            ((System.ComponentModel.ISupportInitialize)(this.uiPM)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelMediaRaps)).BeginInit();
            this.uiPanelMediaRaps.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelMediaRapsSearch)).BeginInit();
            this.uiPanelMediaRapsSearch.SuspendLayout();
            this.uiPanelMediaRapsSearchContainer.SuspendLayout();
            this.pnlSearch.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelMediaRapList)).BeginInit();
            this.uiPanelMediaRapList.SuspendLayout();
            this.uiPanelMediaRapListContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdExCollectionList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dvCollections)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.targetingCollectionDs)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelTargeting)).BeginInit();
            this.uiPanelTargeting.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelHome)).BeginInit();
            this.uiPanelHome.SuspendLayout();
            this.uiPanel1Container.SuspendLayout();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdExTargetCM)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dvTagetingCM)).BeginInit();
            this.panel5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelOAP)).BeginInit();
            this.uiPanelOAP.SuspendLayout();
            this.uiPanel2Container.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdExTargetOAP)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dvTargetingOAP)).BeginInit();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelCM)).BeginInit();
            this.uiPanelCM.SuspendLayout();
            this.uiPanel3Container.SuspendLayout();
            this.panel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdExTargetHome)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dvTargetingHome)).BeginInit();
            this.panel6.SuspendLayout();
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
            this.uiPanelMediaRaps.Id = new System.Guid("b162c53a-7940-47cd-a0ed-9762c59c50d8");
            this.uiPanelMediaRaps.StaticGroup = true;
            this.uiPanelMediaRapsSearch.Id = new System.Guid("afb3b7d0-aa79-443c-a1e8-3baba38f7a4d");
            this.uiPanelMediaRaps.Panels.Add(this.uiPanelMediaRapsSearch);
            this.uiPanelMediaRapList.Id = new System.Guid("8e3140b9-bfcb-43af-b84e-2ee6cddf51dc");
            this.uiPanelMediaRaps.Panels.Add(this.uiPanelMediaRapList);
            this.uiPanelTargeting.Id = new System.Guid("387920bf-83ab-49c1-a03c-e3eba1678347");
            this.uiPanelTargeting.StaticGroup = true;
            this.uiPanelHome.Id = new System.Guid("43bce69e-3cf9-4401-8359-61a93d258c6c");
            this.uiPanelTargeting.Panels.Add(this.uiPanelHome);
            this.uiPanelOAP.Id = new System.Guid("78340e62-02bb-4b85-afe3-cfaec7cf9d13");
            this.uiPanelTargeting.Panels.Add(this.uiPanelOAP);
            this.uiPanelCM.Id = new System.Guid("17591088-ffb1-4fa7-8d96-661079cca56d");
            this.uiPanelTargeting.Panels.Add(this.uiPanelCM);
            this.uiPanelMediaRaps.Panels.Add(this.uiPanelTargeting);
            this.uiPM.Panels.Add(this.uiPanelMediaRaps);
            // 
            // Design Time Panel Info:
            // 
            this.uiPM.BeginPanelInfo();
            this.uiPM.AddDockPanelInfo(new System.Guid("b162c53a-7940-47cd-a0ed-9762c59c50d8"), Janus.Windows.UI.Dock.PanelGroupStyle.HorizontalTiles, Janus.Windows.UI.Dock.PanelDockStyle.Fill, true, new System.Drawing.Size(1010, 677), true);
            this.uiPM.AddDockPanelInfo(new System.Guid("afb3b7d0-aa79-443c-a1e8-3baba38f7a4d"), new System.Guid("b162c53a-7940-47cd-a0ed-9762c59c50d8"), 40, true);
            this.uiPM.AddDockPanelInfo(new System.Guid("8e3140b9-bfcb-43af-b84e-2ee6cddf51dc"), new System.Guid("b162c53a-7940-47cd-a0ed-9762c59c50d8"), 264, true);
            this.uiPM.AddDockPanelInfo(new System.Guid("387920bf-83ab-49c1-a03c-e3eba1678347"), new System.Guid("b162c53a-7940-47cd-a0ed-9762c59c50d8"), Janus.Windows.UI.Dock.PanelGroupStyle.VerticalTiles, true, 343, true);
            this.uiPM.AddDockPanelInfo(new System.Guid("43bce69e-3cf9-4401-8359-61a93d258c6c"), new System.Guid("387920bf-83ab-49c1-a03c-e3eba1678347"), 1010, true);
            this.uiPM.AddDockPanelInfo(new System.Guid("78340e62-02bb-4b85-afe3-cfaec7cf9d13"), new System.Guid("387920bf-83ab-49c1-a03c-e3eba1678347"), 1010, true);
            this.uiPM.AddDockPanelInfo(new System.Guid("17591088-ffb1-4fa7-8d96-661079cca56d"), new System.Guid("387920bf-83ab-49c1-a03c-e3eba1678347"), 1010, true);
            this.uiPM.AddFloatingPanelInfo(new System.Guid("b162c53a-7940-47cd-a0ed-9762c59c50d8"), Janus.Windows.UI.Dock.PanelGroupStyle.HorizontalTiles, true, new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
            this.uiPM.AddFloatingPanelInfo(new System.Guid("8e3140b9-bfcb-43af-b84e-2ee6cddf51dc"), new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
            this.uiPM.AddFloatingPanelInfo(new System.Guid("b2973a57-d3e1-4d80-9387-7a6dc3332542"), new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
            this.uiPM.AddFloatingPanelInfo(new System.Guid("afb3b7d0-aa79-443c-a1e8-3baba38f7a4d"), new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
            this.uiPM.AddFloatingPanelInfo(new System.Guid("387920bf-83ab-49c1-a03c-e3eba1678347"), Janus.Windows.UI.Dock.PanelGroupStyle.VerticalTiles, true, new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
            this.uiPM.AddFloatingPanelInfo(new System.Guid("43bce69e-3cf9-4401-8359-61a93d258c6c"), new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
            this.uiPM.AddFloatingPanelInfo(new System.Guid("78340e62-02bb-4b85-afe3-cfaec7cf9d13"), new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
            this.uiPM.AddFloatingPanelInfo(new System.Guid("17591088-ffb1-4fa7-8d96-661079cca56d"), new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
            this.uiPM.EndPanelInfo();
            // 
            // uiPanelMediaRaps
            // 
            this.uiPanelMediaRaps.ActiveCaptionMode = Janus.Windows.UI.Dock.ActiveCaptionMode.Never;
            this.uiPanelMediaRaps.BackColor = System.Drawing.SystemColors.Window;
            this.uiPanelMediaRaps.CaptionVisible = Janus.Windows.UI.InheritableBoolean.True;
            this.uiPanelMediaRaps.CloseButtonVisible = Janus.Windows.UI.InheritableBoolean.False;
            this.uiPanelMediaRaps.Location = new System.Drawing.Point(0, 0);
            this.uiPanelMediaRaps.Name = "uiPanelMediaRaps";
            this.uiPanelMediaRaps.Size = new System.Drawing.Size(1010, 677);
            this.uiPanelMediaRaps.TabIndex = 4;
            this.uiPanelMediaRaps.Text = "고객군기준타겟팅";
            // 
            // uiPanelMediaRapsSearch
            // 
            this.uiPanelMediaRapsSearch.AllowResize = Janus.Windows.UI.InheritableBoolean.False;
            this.uiPanelMediaRapsSearch.CaptionVisible = Janus.Windows.UI.InheritableBoolean.False;
            this.uiPanelMediaRapsSearch.CloseButtonVisible = Janus.Windows.UI.InheritableBoolean.False;
            this.uiPanelMediaRapsSearch.InnerContainer = this.uiPanelMediaRapsSearchContainer;
            this.uiPanelMediaRapsSearch.Location = new System.Drawing.Point(0, 22);
            this.uiPanelMediaRapsSearch.Name = "uiPanelMediaRapsSearch";
            this.uiPanelMediaRapsSearch.Size = new System.Drawing.Size(1010, 40);
            this.uiPanelMediaRapsSearch.TabIndex = 0;
            this.uiPanelMediaRapsSearch.Text = "검색";
            // 
            // uiPanelMediaRapsSearchContainer
            // 
            this.uiPanelMediaRapsSearchContainer.Controls.Add(this.pnlSearch);
            this.uiPanelMediaRapsSearchContainer.Location = new System.Drawing.Point(1, 1);
            this.uiPanelMediaRapsSearchContainer.Name = "uiPanelMediaRapsSearchContainer";
            this.uiPanelMediaRapsSearchContainer.Size = new System.Drawing.Size(1008, 38);
            this.uiPanelMediaRapsSearchContainer.TabIndex = 0;
            // 
            // pnlSearch
            // 
            this.pnlSearch.BackColor = System.Drawing.SystemColors.Window;
            this.pnlSearch.Controls.Add(this.chkSearchNonuseYn);
            this.pnlSearch.Controls.Add(this.ebSearchKey);
            this.pnlSearch.Controls.Add(this.btnSearch);
            this.pnlSearch.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlSearch.Location = new System.Drawing.Point(0, 0);
            this.pnlSearch.Name = "pnlSearch";
            this.pnlSearch.Size = new System.Drawing.Size(1008, 38);
            this.pnlSearch.TabIndex = 0;
            // 
            // chkSearchNonuseYn
            // 
            this.chkSearchNonuseYn.BackColor = System.Drawing.SystemColors.Window;
            this.chkSearchNonuseYn.Font = new System.Drawing.Font("맑은 고딕", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.chkSearchNonuseYn.Location = new System.Drawing.Point(262, 11);
            this.chkSearchNonuseYn.Name = "chkSearchNonuseYn";
            this.chkSearchNonuseYn.Size = new System.Drawing.Size(95, 20);
            this.chkSearchNonuseYn.TabIndex = 34;
            this.chkSearchNonuseYn.Text = "미사용 포함";
            this.chkSearchNonuseYn.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            // 
            // ebSearchKey
            // 
            this.ebSearchKey.Location = new System.Drawing.Point(8, 9);
            this.ebSearchKey.Name = "ebSearchKey";
            this.ebSearchKey.Size = new System.Drawing.Size(248, 23);
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
            this.btnSearch.Location = new System.Drawing.Point(891, 8);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(104, 24);
            this.btnSearch.TabIndex = 3;
            this.btnSearch.Text = "조 회";
            this.btnSearch.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // uiPanelMediaRapList
            // 
            this.uiPanelMediaRapList.AllowResize = Janus.Windows.UI.InheritableBoolean.False;
            this.uiPanelMediaRapList.BackColor = System.Drawing.SystemColors.Window;
            this.uiPanelMediaRapList.CaptionStyle = Janus.Windows.UI.Dock.PanelCaptionStyle.Dark;
            this.uiPanelMediaRapList.InnerContainer = this.uiPanelMediaRapListContainer;
            this.uiPanelMediaRapList.Location = new System.Drawing.Point(0, 66);
            this.uiPanelMediaRapList.MinimumSize = new System.Drawing.Size(-1, 100);
            this.uiPanelMediaRapList.Name = "uiPanelMediaRapList";
            this.uiPanelMediaRapList.Size = new System.Drawing.Size(1010, 264);
            this.uiPanelMediaRapList.TabIndex = 1;
            this.uiPanelMediaRapList.TabStop = false;
            this.uiPanelMediaRapList.Text = "고객군 목록";
            // 
            // uiPanelMediaRapListContainer
            // 
            this.uiPanelMediaRapListContainer.BackColor = System.Drawing.SystemColors.Window;
            this.uiPanelMediaRapListContainer.Controls.Add(this.grdExCollectionList);
            this.uiPanelMediaRapListContainer.Location = new System.Drawing.Point(1, 23);
            this.uiPanelMediaRapListContainer.Name = "uiPanelMediaRapListContainer";
            this.uiPanelMediaRapListContainer.Size = new System.Drawing.Size(1008, 240);
            this.uiPanelMediaRapListContainer.TabIndex = 0;
            // 
            // grdExCollectionList
            // 
            this.grdExCollectionList.AllowEdit = Janus.Windows.GridEX.InheritableBoolean.False;
            this.grdExCollectionList.AlternatingColors = true;
            this.grdExCollectionList.AlternatingRowFormatStyle.BackgroundImageDrawMode = Janus.Windows.GridEX.BackgroundImageDrawMode.Tile;
            this.grdExCollectionList.BorderStyle = Janus.Windows.GridEX.BorderStyle.None;
            this.grdExCollectionList.DataSource = this.dvCollections;
            grdExCollectionList_DesignTimeLayout.LayoutString = resources.GetString("grdExCollectionList_DesignTimeLayout.LayoutString");
            this.grdExCollectionList.DesignTimeLayout = grdExCollectionList_DesignTimeLayout;
            this.grdExCollectionList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdExCollectionList.EmptyRows = true;
            this.grdExCollectionList.FocusCellFormatStyle.BackColor = System.Drawing.SystemColors.Highlight;
            this.grdExCollectionList.FocusCellFormatStyle.ForeColor = System.Drawing.Color.White;
            this.grdExCollectionList.FocusStyle = Janus.Windows.GridEX.FocusStyle.None;
            this.grdExCollectionList.Font = new System.Drawing.Font("맑은 고딕", 8.25F);
            this.grdExCollectionList.FrozenColumns = 2;
            this.grdExCollectionList.GridLineColor = System.Drawing.Color.Silver;
            this.grdExCollectionList.GridLines = Janus.Windows.GridEX.GridLines.Vertical;
            this.grdExCollectionList.GridLineStyle = Janus.Windows.GridEX.GridLineStyle.Solid;
            this.grdExCollectionList.GroupByBoxVisible = false;
            this.grdExCollectionList.HeaderFormatStyle.TextAlignment = Janus.Windows.GridEX.TextAlignment.Center;
            this.grdExCollectionList.HideSelection = Janus.Windows.GridEX.HideSelection.HighlightInactive;
            this.grdExCollectionList.Location = new System.Drawing.Point(0, 0);
            this.grdExCollectionList.Name = "grdExCollectionList";
            this.grdExCollectionList.ScrollBars = Janus.Windows.GridEX.ScrollBars.Vertical;
            this.grdExCollectionList.SelectedInactiveFormatStyle.BackColor = System.Drawing.Color.Gold;
            this.grdExCollectionList.Size = new System.Drawing.Size(1008, 240);
            this.grdExCollectionList.TabIndex = 4;
            this.grdExCollectionList.TabKeyBehavior = Janus.Windows.GridEX.TabKeyBehavior.ControlNavigation;
            this.grdExCollectionList.ThemedAreas = ((Janus.Windows.GridEX.ThemedArea)(((((((((Janus.Windows.GridEX.ThemedArea.ScrollBars | Janus.Windows.GridEX.ThemedArea.EditControls) 
            | Janus.Windows.GridEX.ThemedArea.Headers) 
            | Janus.Windows.GridEX.ThemedArea.GroupByBox) 
            | Janus.Windows.GridEX.ThemedArea.GroupRows) 
            | Janus.Windows.GridEX.ThemedArea.ControlBorder) 
            | Janus.Windows.GridEX.ThemedArea.Cards) 
            | Janus.Windows.GridEX.ThemedArea.Gridlines) 
            | Janus.Windows.GridEX.ThemedArea.CheckBoxes)));
            this.grdExCollectionList.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            this.grdExCollectionList.Enter += new System.EventHandler(this.OnGrdRowChanged);
            // 
            // dvCollections
            // 
            this.dvCollections.Table = this.targetingCollectionDs.Collections;
            // 
            // targetingCollectionDs
            // 
            this.targetingCollectionDs.DataSetName = "TargetingCollectionDs";
            this.targetingCollectionDs.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // uiPanelTargeting
            // 
            this.uiPanelTargeting.AllowResize = Janus.Windows.UI.InheritableBoolean.False;
            this.uiPanelTargeting.GroupStyle = Janus.Windows.UI.Dock.PanelGroupStyle.VerticalTiles;
            this.uiPanelTargeting.Location = new System.Drawing.Point(0, 334);
            this.uiPanelTargeting.Name = "uiPanelTargeting";
            this.uiPanelTargeting.Size = new System.Drawing.Size(1010, 343);
            this.uiPanelTargeting.TabIndex = 4;
            this.uiPanelTargeting.Text = "타겟팅";
            // 
            // uiPanelHome
            // 
            this.uiPanelHome.AllowResize = Janus.Windows.UI.InheritableBoolean.False;
            this.uiPanelHome.InnerContainer = this.uiPanel1Container;
            this.uiPanelHome.Location = new System.Drawing.Point(0, 22);
            this.uiPanelHome.Name = "uiPanelHome";
            this.uiPanelHome.Size = new System.Drawing.Size(334, 321);
            this.uiPanelHome.TabIndex = 4;
            this.uiPanelHome.Text = "상업광고";
            // 
            // uiPanel1Container
            // 
            this.uiPanel1Container.Controls.Add(this.panel3);
            this.uiPanel1Container.Controls.Add(this.panel5);
            this.uiPanel1Container.Location = new System.Drawing.Point(1, 23);
            this.uiPanel1Container.Name = "uiPanel1Container";
            this.uiPanel1Container.Size = new System.Drawing.Size(332, 297);
            this.uiPanel1Container.TabIndex = 0;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.grdExTargetCM);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(0, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(332, 257);
            this.panel3.TabIndex = 25;
            // 
            // grdExTargetCM
            // 
            this.grdExTargetCM.AlternatingColors = true;
            this.grdExTargetCM.BorderStyle = Janus.Windows.GridEX.BorderStyle.Flat;
            this.grdExTargetCM.DataSource = this.dvTagetingCM;
            grdExTargetCM_DesignTimeLayout.LayoutString = resources.GetString("grdExTargetCM_DesignTimeLayout.LayoutString");
            this.grdExTargetCM.DesignTimeLayout = grdExTargetCM_DesignTimeLayout;
            this.grdExTargetCM.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdExTargetCM.EmptyRows = true;
            this.grdExTargetCM.FocusCellFormatStyle.BackColor = System.Drawing.SystemColors.Highlight;
            this.grdExTargetCM.FocusStyle = Janus.Windows.GridEX.FocusStyle.None;
            this.grdExTargetCM.Font = new System.Drawing.Font("맑은 고딕", 8.25F);
            this.grdExTargetCM.FrozenColumns = 2;
            this.grdExTargetCM.GridLineColor = System.Drawing.Color.Silver;
            this.grdExTargetCM.GridLines = Janus.Windows.GridEX.GridLines.Vertical;
            this.grdExTargetCM.GridLineStyle = Janus.Windows.GridEX.GridLineStyle.Solid;
            this.grdExTargetCM.GroupByBoxVisible = false;
            this.grdExTargetCM.Location = new System.Drawing.Point(0, 0);
            this.grdExTargetCM.Name = "grdExTargetCM";
            this.grdExTargetCM.ScrollBars = Janus.Windows.GridEX.ScrollBars.Vertical;
            this.grdExTargetCM.SelectedInactiveFormatStyle.BackColor = System.Drawing.Color.DarkOrange;
            this.grdExTargetCM.Size = new System.Drawing.Size(332, 257);
            this.grdExTargetCM.TabIndex = 19;
            this.grdExTargetCM.TabKeyBehavior = Janus.Windows.GridEX.TabKeyBehavior.ControlNavigation;
            this.grdExTargetCM.ThemedAreas = ((Janus.Windows.GridEX.ThemedArea)(((((((((Janus.Windows.GridEX.ThemedArea.ScrollBars | Janus.Windows.GridEX.ThemedArea.EditControls) 
            | Janus.Windows.GridEX.ThemedArea.Headers) 
            | Janus.Windows.GridEX.ThemedArea.GroupByBox) 
            | Janus.Windows.GridEX.ThemedArea.GroupRows) 
            | Janus.Windows.GridEX.ThemedArea.ControlBorder) 
            | Janus.Windows.GridEX.ThemedArea.Cards) 
            | Janus.Windows.GridEX.ThemedArea.Gridlines) 
            | Janus.Windows.GridEX.ThemedArea.CheckBoxes)));
            this.grdExTargetCM.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            // 
            // dvTagetingCM
            // 
            this.dvTagetingCM.Table = this.targetingCollectionDs.TargetCM;
            // 
            // panel5
            // 
            this.panel5.BackColor = System.Drawing.SystemColors.Window;
            this.panel5.Controls.Add(this.btnDeleteCM);
            this.panel5.Controls.Add(this.btnAddCM);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel5.Location = new System.Drawing.Point(0, 257);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(332, 40);
            this.panel5.TabIndex = 24;
            // 
            // btnDeleteCM
            // 
            this.btnDeleteCM.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnDeleteCM.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnDeleteCM.Enabled = false;
            this.btnDeleteCM.Location = new System.Drawing.Point(120, 8);
            this.btnDeleteCM.Name = "btnDeleteCM";
            this.btnDeleteCM.Size = new System.Drawing.Size(104, 24);
            this.btnDeleteCM.TabIndex = 17;
            this.btnDeleteCM.Text = "삭 제";
            this.btnDeleteCM.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.btnDeleteCM.Click += new System.EventHandler(this.btnDeleteCM_Click);
            // 
            // btnAddCM
            // 
            this.btnAddCM.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnAddCM.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnAddCM.Enabled = false;
            this.btnAddCM.Location = new System.Drawing.Point(8, 8);
            this.btnAddCM.Name = "btnAddCM";
            this.btnAddCM.Size = new System.Drawing.Size(104, 24);
            this.btnAddCM.TabIndex = 16;
            this.btnAddCM.Text = "추 가";
            this.btnAddCM.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.btnAddCM.Click += new System.EventHandler(this.btnAddCM_Click);
            // 
            // uiPanelOAP
            // 
            this.uiPanelOAP.AllowResize = Janus.Windows.UI.InheritableBoolean.False;
            this.uiPanelOAP.InnerContainer = this.uiPanel2Container;
            this.uiPanelOAP.Location = new System.Drawing.Point(338, 22);
            this.uiPanelOAP.Name = "uiPanelOAP";
            this.uiPanelOAP.Size = new System.Drawing.Size(334, 321);
            this.uiPanelOAP.TabIndex = 4;
            this.uiPanelOAP.Text = "매체광고";
            // 
            // uiPanel2Container
            // 
            this.uiPanel2Container.Controls.Add(this.panel1);
            this.uiPanel2Container.Controls.Add(this.panel2);
            this.uiPanel2Container.Location = new System.Drawing.Point(1, 23);
            this.uiPanel2Container.Name = "uiPanel2Container";
            this.uiPanel2Container.Size = new System.Drawing.Size(332, 297);
            this.uiPanel2Container.TabIndex = 0;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.grdExTargetOAP);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(332, 257);
            this.panel1.TabIndex = 25;
            // 
            // grdExTargetOAP
            // 
            this.grdExTargetOAP.AlternatingColors = true;
            this.grdExTargetOAP.BorderStyle = Janus.Windows.GridEX.BorderStyle.None;
            this.grdExTargetOAP.DataSource = this.dvTargetingOAP;
            grdExTargetOAP_DesignTimeLayout.LayoutString = resources.GetString("grdExTargetOAP_DesignTimeLayout.LayoutString");
            this.grdExTargetOAP.DesignTimeLayout = grdExTargetOAP_DesignTimeLayout;
            this.grdExTargetOAP.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdExTargetOAP.EmptyRows = true;
            this.grdExTargetOAP.FocusCellFormatStyle.BackColor = System.Drawing.SystemColors.Highlight;
            this.grdExTargetOAP.FocusStyle = Janus.Windows.GridEX.FocusStyle.None;
            this.grdExTargetOAP.Font = new System.Drawing.Font("맑은 고딕", 8.25F);
            this.grdExTargetOAP.FrozenColumns = 2;
            this.grdExTargetOAP.GridLineColor = System.Drawing.Color.Silver;
            this.grdExTargetOAP.GridLines = Janus.Windows.GridEX.GridLines.Vertical;
            this.grdExTargetOAP.GridLineStyle = Janus.Windows.GridEX.GridLineStyle.Solid;
            this.grdExTargetOAP.GroupByBoxVisible = false;
            this.grdExTargetOAP.Location = new System.Drawing.Point(0, 0);
            this.grdExTargetOAP.Name = "grdExTargetOAP";
            this.grdExTargetOAP.ScrollBars = Janus.Windows.GridEX.ScrollBars.Vertical;
            this.grdExTargetOAP.SelectedInactiveFormatStyle.BackColor = System.Drawing.Color.DarkOrange;
            this.grdExTargetOAP.Size = new System.Drawing.Size(332, 257);
            this.grdExTargetOAP.TabIndex = 16;
            this.grdExTargetOAP.TabKeyBehavior = Janus.Windows.GridEX.TabKeyBehavior.ControlNavigation;
            this.grdExTargetOAP.ThemedAreas = ((Janus.Windows.GridEX.ThemedArea)(((((((((Janus.Windows.GridEX.ThemedArea.ScrollBars | Janus.Windows.GridEX.ThemedArea.EditControls) 
            | Janus.Windows.GridEX.ThemedArea.Headers) 
            | Janus.Windows.GridEX.ThemedArea.GroupByBox) 
            | Janus.Windows.GridEX.ThemedArea.GroupRows) 
            | Janus.Windows.GridEX.ThemedArea.ControlBorder) 
            | Janus.Windows.GridEX.ThemedArea.Cards) 
            | Janus.Windows.GridEX.ThemedArea.Gridlines) 
            | Janus.Windows.GridEX.ThemedArea.CheckBoxes)));
            this.grdExTargetOAP.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            // 
            // dvTargetingOAP
            // 
            this.dvTargetingOAP.Table = this.targetingCollectionDs.TargetOAP;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.SystemColors.Window;
            this.panel2.Controls.Add(this.btnDeleteOAP);
            this.panel2.Controls.Add(this.btnAddOAP);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 257);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(332, 40);
            this.panel2.TabIndex = 24;
            // 
            // btnDeleteOAP
            // 
            this.btnDeleteOAP.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnDeleteOAP.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnDeleteOAP.Enabled = false;
            this.btnDeleteOAP.Location = new System.Drawing.Point(120, 8);
            this.btnDeleteOAP.Name = "btnDeleteOAP";
            this.btnDeleteOAP.Size = new System.Drawing.Size(104, 24);
            this.btnDeleteOAP.TabIndex = 17;
            this.btnDeleteOAP.Text = "삭 제";
            this.btnDeleteOAP.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.btnDeleteOAP.Click += new System.EventHandler(this.btnDeleteOAP_Click);
            // 
            // btnAddOAP
            // 
            this.btnAddOAP.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnAddOAP.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnAddOAP.Enabled = false;
            this.btnAddOAP.Location = new System.Drawing.Point(8, 8);
            this.btnAddOAP.Name = "btnAddOAP";
            this.btnAddOAP.Size = new System.Drawing.Size(104, 24);
            this.btnAddOAP.TabIndex = 16;
            this.btnAddOAP.Text = "추 가";
            this.btnAddOAP.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.btnAddOAP.Click += new System.EventHandler(this.btnAddOAP_Click);
            // 
            // uiPanelCM
            // 
            this.uiPanelCM.AllowResize = Janus.Windows.UI.InheritableBoolean.False;
            this.uiPanelCM.InnerContainer = this.uiPanel3Container;
            this.uiPanelCM.Location = new System.Drawing.Point(676, 22);
            this.uiPanelCM.Name = "uiPanelCM";
            this.uiPanelCM.Size = new System.Drawing.Size(334, 321);
            this.uiPanelCM.TabIndex = 4;
            this.uiPanelCM.Text = "홈광고";
            // 
            // uiPanel3Container
            // 
            this.uiPanel3Container.Controls.Add(this.panel4);
            this.uiPanel3Container.Controls.Add(this.panel6);
            this.uiPanel3Container.Location = new System.Drawing.Point(1, 23);
            this.uiPanel3Container.Name = "uiPanel3Container";
            this.uiPanel3Container.Size = new System.Drawing.Size(332, 297);
            this.uiPanel3Container.TabIndex = 0;
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.grdExTargetHome);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel4.Location = new System.Drawing.Point(0, 0);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(332, 257);
            this.panel4.TabIndex = 25;
            // 
            // grdExTargetHome
            // 
            this.grdExTargetHome.AlternatingColors = true;
            this.grdExTargetHome.BorderStyle = Janus.Windows.GridEX.BorderStyle.None;
            this.grdExTargetHome.DataSource = this.dvTargetingHome;
            grdExTargetHome_DesignTimeLayout.LayoutString = resources.GetString("grdExTargetHome_DesignTimeLayout.LayoutString");
            this.grdExTargetHome.DesignTimeLayout = grdExTargetHome_DesignTimeLayout;
            this.grdExTargetHome.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdExTargetHome.EmptyRows = true;
            this.grdExTargetHome.FocusCellFormatStyle.BackColor = System.Drawing.SystemColors.Highlight;
            this.grdExTargetHome.FocusStyle = Janus.Windows.GridEX.FocusStyle.None;
            this.grdExTargetHome.Font = new System.Drawing.Font("맑은 고딕", 8.25F);
            this.grdExTargetHome.FrozenColumns = 2;
            this.grdExTargetHome.GridLineColor = System.Drawing.Color.Silver;
            this.grdExTargetHome.GridLines = Janus.Windows.GridEX.GridLines.Vertical;
            this.grdExTargetHome.GridLineStyle = Janus.Windows.GridEX.GridLineStyle.Solid;
            this.grdExTargetHome.GroupByBoxVisible = false;
            this.grdExTargetHome.Location = new System.Drawing.Point(0, 0);
            this.grdExTargetHome.Name = "grdExTargetHome";
            this.grdExTargetHome.ScrollBars = Janus.Windows.GridEX.ScrollBars.Vertical;
            this.grdExTargetHome.SelectedInactiveFormatStyle.BackColor = System.Drawing.Color.DarkOrange;
            this.grdExTargetHome.Size = new System.Drawing.Size(332, 257);
            this.grdExTargetHome.TabIndex = 16;
            this.grdExTargetHome.TabKeyBehavior = Janus.Windows.GridEX.TabKeyBehavior.ControlNavigation;
            this.grdExTargetHome.ThemedAreas = ((Janus.Windows.GridEX.ThemedArea)(((((((((Janus.Windows.GridEX.ThemedArea.ScrollBars | Janus.Windows.GridEX.ThemedArea.EditControls) 
            | Janus.Windows.GridEX.ThemedArea.Headers) 
            | Janus.Windows.GridEX.ThemedArea.GroupByBox) 
            | Janus.Windows.GridEX.ThemedArea.GroupRows) 
            | Janus.Windows.GridEX.ThemedArea.ControlBorder) 
            | Janus.Windows.GridEX.ThemedArea.Cards) 
            | Janus.Windows.GridEX.ThemedArea.Gridlines) 
            | Janus.Windows.GridEX.ThemedArea.CheckBoxes)));
            this.grdExTargetHome.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            // 
            // dvTargetingHome
            // 
            this.dvTargetingHome.Table = this.targetingCollectionDs.TargetHome;
            // 
            // panel6
            // 
            this.panel6.BackColor = System.Drawing.SystemColors.Window;
            this.panel6.Controls.Add(this.btnDeleteHome);
            this.panel6.Controls.Add(this.btnAddHome);
            this.panel6.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel6.Location = new System.Drawing.Point(0, 257);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(332, 40);
            this.panel6.TabIndex = 24;
            // 
            // btnDeleteHome
            // 
            this.btnDeleteHome.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnDeleteHome.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnDeleteHome.Enabled = false;
            this.btnDeleteHome.Location = new System.Drawing.Point(120, 8);
            this.btnDeleteHome.Name = "btnDeleteHome";
            this.btnDeleteHome.Size = new System.Drawing.Size(104, 24);
            this.btnDeleteHome.TabIndex = 17;
            this.btnDeleteHome.Text = "삭 제";
            this.btnDeleteHome.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.btnDeleteHome.Click += new System.EventHandler(this.btnDeleteHome_Click);
            // 
            // btnAddHome
            // 
            this.btnAddHome.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnAddHome.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnAddHome.Enabled = false;
            this.btnAddHome.Location = new System.Drawing.Point(8, 8);
            this.btnAddHome.Name = "btnAddHome";
            this.btnAddHome.Size = new System.Drawing.Size(104, 24);
            this.btnAddHome.TabIndex = 16;
            this.btnAddHome.Text = "추 가";
            this.btnAddHome.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.btnAddHome.Click += new System.EventHandler(this.btnAddHome_Click);
            // 
            // pnlMediaRapDetail
            // 
            this.pnlMediaRapDetail.BackColor = System.Drawing.SystemColors.Window;
            this.pnlMediaRapDetail.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlMediaRapDetail.Font = new System.Drawing.Font("맑은 고딕", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.pnlMediaRapDetail.Location = new System.Drawing.Point(0, 0);
            this.pnlMediaRapDetail.Name = "pnlMediaRapDetail";
            this.pnlMediaRapDetail.Size = new System.Drawing.Size(1008, 264);
            this.pnlMediaRapDetail.TabIndex = 0;
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
            // TargetingCollectionControl
            // 
            this.Controls.Add(this.uiPanelMediaRaps);
            this.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "TargetingCollectionControl";
            this.Size = new System.Drawing.Size(1010, 677);
            this.Load += new System.EventHandler(this.TargetCollectionControl_Load);
            ((System.ComponentModel.ISupportInitialize)(this.uiPM)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelMediaRaps)).EndInit();
            this.uiPanelMediaRaps.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelMediaRapsSearch)).EndInit();
            this.uiPanelMediaRapsSearch.ResumeLayout(false);
            this.uiPanelMediaRapsSearchContainer.ResumeLayout(false);
            this.pnlSearch.ResumeLayout(false);
            this.pnlSearch.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelMediaRapList)).EndInit();
            this.uiPanelMediaRapList.ResumeLayout(false);
            this.uiPanelMediaRapListContainer.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdExCollectionList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dvCollections)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.targetingCollectionDs)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelTargeting)).EndInit();
            this.uiPanelTargeting.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelHome)).EndInit();
            this.uiPanelHome.ResumeLayout(false);
            this.uiPanel1Container.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdExTargetCM)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dvTagetingCM)).EndInit();
            this.panel5.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelOAP)).EndInit();
            this.uiPanelOAP.ResumeLayout(false);
            this.uiPanel2Container.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdExTargetOAP)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dvTargetingOAP)).EndInit();
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelCM)).EndInit();
            this.uiPanelCM.ResumeLayout(false);
            this.uiPanel3Container.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdExTargetHome)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dvTargetingHome)).EndInit();
            this.panel6.ResumeLayout(false);
            this.ResumeLayout(false);

		}
		#endregion

		#region 컨트롤 로드
		private void TargetCollectionControl_Load(object sender, System.EventArgs e)
		{
			// 데이터관리용 객체생성
			dt = ((DataView)grdExCollectionList.DataSource).Table;  
			cm = (CurrencyManager) this.BindingContext[grdExCollectionList.DataSource]; 
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
				SearchCollections();
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
				canUpdate = true;
			}

			InitButton();
			ProgressStop();
		}
		
		private void InitButton()
		{
			if(canRead)   btnSearch.Enabled = true;
            if (canUpdate)
            {
                btnAddCM.Enabled = true;
                btnAddOAP.Enabled = true;
                btnAddHome.Enabled = true;
                btnDeleteCM.Enabled = true;
                btnDeleteOAP.Enabled = true;
                btnDeleteHome.Enabled = true;
            }
            else
            {
                if (canCreate)
                {
                    btnAddCM.Enabled = true;
                    btnAddOAP.Enabled = true;
                    btnAddHome.Enabled = true;
                }

                if (canDelete)
                {
                    btnDeleteCM.Enabled = true;
                    btnDeleteOAP.Enabled = true;
                    btnDeleteHome.Enabled = true;
                }
            }

			Application.DoEvents();
		}

		private void DisableButton()
		{
			btnSearch.Enabled	= true;

            Application.DoEvents();
		}

		#endregion

		#region 타겟군 액션처리 메소드

		/// <summary>
		/// 그리드의 Row변경시
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void OnGrdRowChanged(object sender, System.EventArgs e) 
		{
            if (!IsSearching) // 2011.11.29 JH.Park 조회중이 아닐경우에만 동작하도록 변경
            {
                if (grdExCollectionList.RecordCount > 0)
                {
                    SetTargetingLists();
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
			DisableButton();
			SearchCollections();
			InitButton();
			ProgressStop();
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
				SearchCollections();
			}
		}

		#endregion

		#region 처리메소드

		/// <summary>
		/// 타겟군목록 조회
		/// </summary>
		private void SearchCollections()
		{
            IsSearching = true;

            StatusMessage("고객군 정보를 조회합니다.");		

			try
			{
                // 데이터모델 초기화
                model.Init();

                // 데이터모델에 전송할 내용을 셋트한다.
                if (IsNewSearchKey) model.SearchKey = "";
                else model.SearchKey = ebSearchKey.Text;

                // 미사용 포함 여부 
                if (chkSearchNonuseYn.Checked) model.SearchNonuseYn = "Y";
                else model.SearchNonuseYn = "N";

                // 광고 타겟팅 목록조회 서비스를 호출한다.
                new TargetingCollectionManager(systemModel, commonModel).GetCollectionList(model);

                if (model.ResultCD.Equals("0000"))
                {
                    Utility.SetDataTableNoEvent(targetingCollectionDs.Collections, model.CollectionsDataSet);
                    StatusMessage(model.ResultCnt + "건의 고객군 정보가 조회되었습니다.");
                    SetTargetingLists();  // 
                }
			}
			catch(FrameException fe)
			{
                FrameSystem.showMsgForm("고객군조회오류", new string[] { fe.ErrCode, fe.ResultMsg });
			}
			catch(Exception ex)
			{
                FrameSystem.showMsgForm("고객군조회오류", new string[] { "", ex.Message });
			}
            finally
            {
                IsSearching = false; // 조회중 Flag 리셋
            }
		}

		/// <summary>
		/// 키캆을찾아 그리드 키에 해당되는로우로..
		/// </summary>
		private void ScrollToLastRow()
		{
            
			try
			{
				int rowIndex = 0;
				if ( targetingCollectionDs.Tables["Collections"].Rows.Count < 1 ) return;

                foreach (DataRow row in targetingCollectionDs.Tables["Collections"].Rows)
				{					
					if(IsAdding)
					{
						cm.Position = 0;
						keyCollectionCode = null;						
					}
					else
					{
                        if (row["CollectionCode"].ToString().Equals(keyCollectionCode))
						{					
							cm.Position = rowIndex;
							break;								
						}
					}

					rowIndex++;
					grdExCollectionList.EnsureVisible();
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
		/// 고객군 타겟팅정보의 셋트
		/// </summary>
		private void SetTargetingLists()
		{

			int curRow = cm.Position;
			if(curRow >= 0)
			{
                collectionCode = dt.Rows[curRow]["CollectionCode"].ToString();
                collectionName = dt.Rows[curRow]["CollectionName"].ToString();

                uiPanelTargeting.Text = "타겟팅 정보 설정 : [" + collectionCode + "] " + collectionName;

				model.Init();
                model.CollectionCode = collectionCode; 

				// 상업광고 타겟팅 리스트 조회 서비스를 호출한다.
				new TargetingCollectionManager(systemModel,commonModel).GetTargetingCMList(model);

                if (model.ResultCD.Equals("0000"))
                {
                    Utility.SetDataTable(targetingCollectionDs.TargetCM, model.CMDataSet);
                }

                // 매체광고 타겟팅 리스트 조회 서비스를 호출한다.
                new TargetingCollectionManager(systemModel, commonModel).GetTargetingOAPList(model);

                if (model.ResultCD.Equals("0000") )
                {
                    Utility.SetDataTable(targetingCollectionDs.TargetOAP, model.OAPDataSet);
                }

                // 홈광고 타겟팅 리스트 조회 서비스를 호출한다.
                new TargetingCollectionManager(systemModel, commonModel).GetTargetingHomeList(model);

                if (model.ResultCD.Equals("0000"))
                {
                    Utility.SetDataTable(targetingCollectionDs.TargetHome, model.HomeDataSet);
                }

				IsAdding = false;
				ResetTargetingReadonly();
			}
			StatusMessage("준비");
		}
		
		/// <summary>
		/// 상세정보 ReadOnly
		/// </summary>
		private void SetTargetingReadonly()
		{
            btnAddCM.Enabled = false;
            btnAddHome.Enabled = false;
            btnAddOAP.Enabled = false;

            btnDeleteCM.Enabled = false;
            btnDeleteHome.Enabled = false;
            btnDeleteOAP.Enabled = false;
		}

		/// <summary>
		/// 상세정보 수정가능케
		/// </summary>
		private void ResetTargetingReadonly()
		{
            InitButton();
		}

        ItemMultiChoiceForm pForm = null;

        private void btnAddHome_Click(object sender, EventArgs e)
        {
            pForm = new ItemMultiChoiceForm(this);

            pForm.keySchType = "";
            pForm.callType = "SchHomeAdControl";  // 홈광고 조회형식으로 호출
            pForm.ReturnDate += new ItemMultiChoiceForm.PopupService(ItemMultiChoiceForm_Return_Home);
            pForm.ShowDialog();
            pForm.Dispose();
            pForm = null;
        }

        /// <summary>
        /// ItemMultiChoiceForm 폼에서 광고 선택
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        public void ItemMultiChoiceForm_Return_Home(object sender, EventArgs args)
        {
            ItemEventArgs itemEventArgs = (ItemEventArgs)args;
            ItemMultiChoice_pDs itemMultiChoice_pDs = (ItemMultiChoice_pDs)itemEventArgs.dataSet;

            keyCollectionCode = collectionCode;

            try
            {
                int SetCount = 0;

                for (int i = 0; i < itemMultiChoice_pDs.ChoiceAdItems.Rows.Count; i++)
                {
                    DataRow row = itemMultiChoice_pDs.ChoiceAdItems.Rows[i];

                    if (row["CheckYn"].ToString().Equals("True"))
                    {
                        // 이미 고객군에 설정되어있는지 검사
                        bool isExist = false;
                        foreach (GridEXRow gr in grdExTargetHome.GetRows())
                        {
                            if (row["ItemNo"].ToString().Equals(gr.Cells["ItemNo"].Value.ToString()))
                            {

                                MessageBox.Show("[" + row["ItemName"].ToString() + "]은(는) 이미 해당 고객군에 타겟팅 설정되어 있습니다.", "광고 타겟팅 추가",
                                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                                isExist = true;
                                break;
                            }
                        }

                        if (!isExist)
                        {

                            model.Init();

                            model.ItemNo = row["ItemNo"].ToString();
                            model.SetType = "2"; // 1:일반 2:홈광고
                            model.CollectionCode = keyCollectionCode;

                            new TargetingCollectionManager(systemModel, commonModel).SetTargetingCollectionAdd(model);

                            if (model.ResultCD.Equals("0000"))
                            {
                                SetCount++;
                            }
                        }
                    }
                }

                if (SetCount > 0)
                {
                    SearchCollections();
                    ScrollToLastRow();
                    InitButton();
                }

            }
            catch (FrameException fe)
            {
                FrameSystem.showMsgForm("고객군 광고타겟팅 오류", new string[] { fe.ErrCode, fe.ResultMsg });
            }
            catch (Exception ex)
            {
                FrameSystem.showMsgForm("고객군 광고타겟팅  오류", new string[] { "", ex.Message });
            }
        }
        


        private void btnAddOAP_Click(object sender, EventArgs e)
        {
            pForm = new ItemMultiChoiceForm(this);

            pForm.keySchType = "200";  // 매체광고류 조회하도록 설정
            pForm.callType = "SchChoiceAdControl";  // 일반광고 조회형식으로 호출
            pForm.ReturnDate += new ItemMultiChoiceForm.PopupService(ItemMultiChoiceForm_Return_OAP);
            pForm.ShowDialog();
            pForm.Dispose();
            pForm = null;
        }

        /// <summary>
        /// ItemMultiChoiceForm 폼에서 광고 선택
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        public void ItemMultiChoiceForm_Return_OAP(object sender, EventArgs args)
        {
            ItemEventArgs itemEventArgs = (ItemEventArgs)args;
            ItemMultiChoice_pDs itemMultiChoice_pDs = (ItemMultiChoice_pDs)itemEventArgs.dataSet;

            keyCollectionCode = collectionCode;

            try
            {
                int SetCount = 0;

                for (int i = 0; i < itemMultiChoice_pDs.ChoiceAdItems.Rows.Count; i++)
                {
                    DataRow row = itemMultiChoice_pDs.ChoiceAdItems.Rows[i];

                    if (row["CheckYn"].ToString().Equals("True"))
                    {
                        // 이미 고객군에 설정되어있는지 검사
                        bool isExist = false;
                        foreach (GridEXRow gr in grdExTargetOAP.GetRows())
                        {
                            if (row["ItemNo"].ToString().Equals(gr.Cells["ItemNo"].Value.ToString()))
                            {

                                MessageBox.Show("[" + row["ItemName"].ToString() + "]은(는) 이미 해당 고객군에 타겟팅 설정되어 있습니다.", "광고 타겟팅 추가",
                                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                                isExist = true;
                                break;
                            }
                        }

                        if (!isExist)
                        {

                            model.Init();

                            model.ItemNo = row["ItemNo"].ToString();
                            model.SetType = "1"; // 1:일반 2:홈광고
                            model.CollectionCode = keyCollectionCode;

                            new TargetingCollectionManager(systemModel, commonModel).SetTargetingCollectionAdd(model);

                            if (model.ResultCD.Equals("0000"))
                            {
                                SetCount++;
                            }
                        }
                    }
                }

                if (SetCount > 0)
                {
                    SearchCollections();
                    ScrollToLastRow();
                    InitButton();
                }

            }
            catch (FrameException fe)
            {
                FrameSystem.showMsgForm("고객군 광고타겟팅 오류", new string[] { fe.ErrCode, fe.ResultMsg });
            }
            catch (Exception ex)
            {
                FrameSystem.showMsgForm("고객군 광고타겟팅  오류", new string[] { "", ex.Message });
            }
        }
        



        private void btnAddCM_Click(object sender, EventArgs e)
        {
            pForm = new ItemMultiChoiceForm(this);

            pForm.keySchType = "100";  // 상업광고류 조회하도록 설정
            pForm.callType = "SchChoiceAdControl";  // 일반광고 조회형식으로 호출
            pForm.ReturnDate += new ItemMultiChoiceForm.PopupService(ItemMultiChoiceForm_Return_CM);
            pForm.ShowDialog();
            pForm.Dispose();
            pForm = null;

        }


        /// <summary>
        /// ItemMultiChoiceForm 폼에서 광고 선택
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        public void ItemMultiChoiceForm_Return_CM(object sender, EventArgs args)
        {
            ItemEventArgs itemEventArgs = (ItemEventArgs)args;
            ItemMultiChoice_pDs itemMultiChoice_pDs = (ItemMultiChoice_pDs)itemEventArgs.dataSet;

            keyCollectionCode = collectionCode;

            try
            {
                int SetCount = 0;

                for (int i = 0; i < itemMultiChoice_pDs.ChoiceAdItems.Rows.Count; i++)
                {
                    DataRow row = itemMultiChoice_pDs.ChoiceAdItems.Rows[i];

                    if (row["CheckYn"].ToString().Equals("True"))
                    {

                        // 이미 고객군에 설정되어있는지 검사
                        bool isExist = false;
                        foreach (GridEXRow gr in grdExTargetCM.GetRows())
                        {
                            if (row["ItemNo"].ToString().Equals(gr.Cells["ItemNo"].Value.ToString()))
                            {

                                MessageBox.Show("[" + row["ItemName"].ToString() + "]은(는) 이미 해당 고객군에 타겟팅 설정되어 있습니다.", "광고 타겟팅 추가",
                                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                                isExist = true;
                                break;
                            }
                        }

                        if (!isExist)
                        {

                            model.Init();

                            model.ItemNo = row["ItemNo"].ToString();
                            model.SetType = "1"; // 1:일반 2:홈광고
                            model.CollectionCode = keyCollectionCode;

                            new TargetingCollectionManager(systemModel, commonModel).SetTargetingCollectionAdd(model);

                            if (model.ResultCD.Equals("0000"))
                            {
                                SetCount++;
                            }
                        }
                    }
                }

                if (SetCount > 0)
                {
                    SearchCollections();
                    ScrollToLastRow();
                    InitButton();
                }

            }
            catch (FrameException fe)
            {
                FrameSystem.showMsgForm("고객군 광고타겟팅 오류", new string[] { fe.ErrCode, fe.ResultMsg });
            }
            catch (Exception ex)
            {
                FrameSystem.showMsgForm("고객군 광고타겟팅  오류", new string[] { "", ex.Message });
            }
        }
        



        private void btnDeleteHome_Click(object sender, EventArgs e)
        {
            keyCollectionCode = collectionCode;

            // 그리드에 변경된 데이터를 Datasource에 업데이트 한다.
            grdExTargetHome.UpdateData();

            try
            {
                int SetCount = 0;
                int CheckCount = 0;

                // 설정하려 선택된 고객군의 수
                CheckCount = grdExTargetHome.GetCheckedRows().Length;

                if (CheckCount == 0)
                {
                    MessageBox.Show("선택한 타겟팅광고가 없습니다.", "고객군타겟팅 삭제",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);

                    return;
                }

                foreach (GridEXRow gr in grdExTargetHome.GetCheckedRows())
                {
                    model.Init();

                    model.SetType = "2"; // 1:일반 2:홈광고
                    model.CollectionCode = keyCollectionCode;
                    model.ItemNo = gr.Cells["ItemNo"].Value.ToString();

                    new TargetingCollectionManager(systemModel, commonModel).SetTargetingCollectionDelete(model);

                    if (model.ResultCD.Equals("0000"))
                    {
                        SetCount++;
                    }

                    gr.IsChecked = false;  // 선택해제
                }

                if (SetCount > 0)
                {
                    SearchCollections();
                    ScrollToLastRow();
                    InitButton();
                }

            }
            catch (FrameException fe)
            {
                FrameSystem.showMsgForm("고객군타겟팅 삭제 오류", new string[] { fe.ErrCode, fe.ResultMsg });
            }
            catch (Exception ex)
            {
                FrameSystem.showMsgForm("고객군타겟팅 삭제 오류", new string[] { "", ex.Message });
            }
        }

        private void btnDeleteOAP_Click(object sender, EventArgs e)
        {
            keyCollectionCode = collectionCode;

            // 그리드에 변경된 데이터를 Datasource에 업데이트 한다.
            grdExTargetOAP.UpdateData();

            try
            {
                int SetCount = 0;
                int CheckCount = 0;

                // 설정하려 선택된 고객군의 수
                CheckCount = grdExTargetOAP.GetCheckedRows().Length;

                if (CheckCount == 0)
                {
                    MessageBox.Show("선택한 타겟팅광고가 없습니다.", "고객군타겟팅 삭제",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);

                    return;
                }

                foreach (GridEXRow gr in grdExTargetOAP.GetCheckedRows())
                {
                    model.Init();

                    model.SetType = "1"; // 1:일반 2:홈광고
                    model.CollectionCode = keyCollectionCode;
                    model.ItemNo = gr.Cells["ItemNo"].Value.ToString();

                    new TargetingCollectionManager(systemModel, commonModel).SetTargetingCollectionDelete(model);

                    if (model.ResultCD.Equals("0000"))
                    {
                        SetCount++;
                    }

                    gr.IsChecked = false;  // 선택해제
                }

                if (SetCount > 0)
                {
                    SearchCollections();
                    ScrollToLastRow();
                    InitButton();
                }

            }
            catch (FrameException fe)
            {
                FrameSystem.showMsgForm("고객군타겟팅 삭제 오류", new string[] { fe.ErrCode, fe.ResultMsg });
            }
            catch (Exception ex)
            {
                FrameSystem.showMsgForm("고객군타겟팅 삭제 오류", new string[] { "", ex.Message });
            }
        }

        private void btnDeleteCM_Click(object sender, EventArgs e)
        {
            keyCollectionCode = collectionCode;

            // 그리드에 변경된 데이터를 Datasource에 업데이트 한다.
            grdExTargetCM.UpdateData();

            try
            {
                int SetCount = 0;
                int CheckCount = 0;

                // 설정하려 선택된 고객군의 수
                CheckCount = grdExTargetCM.GetCheckedRows().Length;

                if (CheckCount == 0)
                {
                    MessageBox.Show("선택한 타겟팅광고가 없습니다.", "고객군타겟팅 삭제",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);

                    return;
                }

                foreach (GridEXRow gr in grdExTargetCM.GetCheckedRows())
                {
                    model.Init();

                    model.SetType = "1"; // 1:일반 2:홈광고
                    model.CollectionCode = keyCollectionCode;
                    model.ItemNo = gr.Cells["ItemNo"].Value.ToString();

                    new TargetingCollectionManager(systemModel, commonModel).SetTargetingCollectionDelete(model);

                    if (model.ResultCD.Equals("0000"))
                    {
                        SetCount++;
                    }

                    gr.IsChecked = false;  // 선택해제
                }

                if (SetCount > 0)
                {
                    SearchCollections();
                    ScrollToLastRow();
                    InitButton();
                }

            }
            catch (FrameException fe)
            {
                FrameSystem.showMsgForm("고객군타겟팅 삭제 오류", new string[] { fe.ErrCode, fe.ResultMsg });
            }
            catch (Exception ex)
            {
                FrameSystem.showMsgForm("고객군타겟팅 삭제 오류", new string[] { "", ex.Message });
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
