// ===============================================================================
// ContractPackageControl for Charites Project
//
// ContractPackageControl.cs
//
// 광고상품관리 컨드롤을 정의합니다. 
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
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using System.Threading;

using WinFramework.Misc;
using WinFramework.Base;
using WinFramework.Data;

using AdManagerModel;

namespace AdManagerClient
{
    /// <summary>
    /// 계약정보관리 컨트롤
    /// </summary>
    public class ContractPackageControl : System.Windows.Forms.UserControl, IUserControl
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
        ContractPackageModel contractPackageModel  = new ContractPackageModel();	// 계약정보모델

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


        // Key 데이터
        string keyPackageNo       = "";
		string keyPackageName       = "";
        private Janus.Windows.EditControls.UICheckBox chkSearchUse;
        private Janus.Windows.EditControls.UICheckBox chkAllRep;
		string keyRepCode         = "";

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
        private System.Windows.Forms.Panel pnlSearch;
        private Janus.Windows.EditControls.UIButton btnSearch;
        private System.Windows.Forms.Panel pnlUserDetail;
        private System.Windows.Forms.Label lbRegDt;
        private Janus.Windows.GridEX.EditControls.EditBox ebRegDt;
        private Janus.Windows.UI.Dock.UIPanel uiPanelSearch;
        private Janus.Windows.UI.Dock.UIPanelInnerContainer uiPanelSearchContainer;
        private Janus.Windows.UI.Dock.UIPanel uiPanelList;
        private Janus.Windows.UI.Dock.UIPanelInnerContainer uiPanelListContainer;
        private Janus.Windows.UI.Dock.UIPanel uiPanelDetail;
        private Janus.Windows.UI.Dock.UIPanelInnerContainer uiPanelDetailContainer;
        private System.Windows.Forms.Label lbRegID;
        private Janus.Windows.GridEX.EditControls.EditBox ebModDt;
        private System.Windows.Forms.Label lbModDt;
        private Janus.Windows.GridEX.EditControls.EditBox ebRegName;
        private Janus.Windows.UI.Dock.UIPanelGroup uiPanelContractPackage;
        private Janus.Windows.GridEX.EditControls.EditBox ebSearchKey;
        private Janus.Windows.EditControls.UIComboBox cbSearchRap;
        private System.Data.DataView dvContractPackage;
        private Janus.Windows.GridEX.GridEX grdExContractPackageList;
        private System.Windows.Forms.Label label3;
        private Janus.Windows.GridEX.EditControls.EditBox ebComment;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Panel panel1;
        private Janus.Windows.GridEX.GridEX gridEX1;
        private Janus.Windows.EditControls.UIButton btnDelete;
        private Janus.Windows.EditControls.UIButton btnAdd;
        private Janus.Windows.EditControls.UIButton btnSave;
        private System.Windows.Forms.Label lbContractPackageAmt;
		private System.Windows.Forms.Label lbPackageName;
		private Janus.Windows.EditControls.UIButton btnRapSearch;
		private System.Windows.Forms.Label lbAdTime;
		private System.Windows.Forms.Label label10;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label4;
		private Janus.Windows.EditControls.UIRadioButton rbUseYn_N;
		private Janus.Windows.EditControls.UIRadioButton rbUseYn_Y;
		private System.Windows.Forms.Label lbUseYn;
		private AdManagerClient.ContractPackageDs contractPackageDs;
		private Janus.Windows.GridEX.EditControls.NumericEditBox ebContractAmt;
		private Janus.Windows.GridEX.EditControls.EditBox ebPackageName;
		private Janus.Windows.GridEX.EditControls.EditBox ebRapName;
		private Janus.Windows.GridEX.EditControls.NumericEditBox ebAdTime;
		private Janus.Windows.GridEX.EditControls.NumericEditBox ebBonusRate;
        private Janus.Windows.GridEX.EditControls.NumericEditBox ebPrice;


        private System.ComponentModel.IContainer components;

        public ContractPackageControl()
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
			Janus.Windows.GridEX.GridEXLayout grdExContractPackageList_DesignTimeLayout = new Janus.Windows.GridEX.GridEXLayout();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ContractPackageControl));
			this.uiPM = new Janus.Windows.UI.Dock.UIPanelManager(this.components);
			this.uiPanelContractPackage = new Janus.Windows.UI.Dock.UIPanelGroup();
			this.uiPanelSearch = new Janus.Windows.UI.Dock.UIPanel();
			this.uiPanelSearchContainer = new Janus.Windows.UI.Dock.UIPanelInnerContainer();
			this.pnlSearch = new System.Windows.Forms.Panel();
			this.chkSearchUse = new Janus.Windows.EditControls.UICheckBox();
			this.ebSearchKey = new Janus.Windows.GridEX.EditControls.EditBox();
			this.cbSearchRap = new Janus.Windows.EditControls.UIComboBox();
			this.btnSearch = new Janus.Windows.EditControls.UIButton();
			this.uiPanelList = new Janus.Windows.UI.Dock.UIPanel();
			this.uiPanelListContainer = new Janus.Windows.UI.Dock.UIPanelInnerContainer();
			this.grdExContractPackageList = new Janus.Windows.GridEX.GridEX();
			this.dvContractPackage = new System.Data.DataView();
			this.contractPackageDs = new AdManagerClient.ContractPackageDs();
			this.uiPanelDetail = new Janus.Windows.UI.Dock.UIPanel();
			this.uiPanelDetailContainer = new Janus.Windows.UI.Dock.UIPanelInnerContainer();
			this.pnlUserDetail = new System.Windows.Forms.Panel();
			this.chkAllRep = new Janus.Windows.EditControls.UICheckBox();
			this.ebAdTime = new Janus.Windows.GridEX.EditControls.NumericEditBox();
			this.rbUseYn_N = new Janus.Windows.EditControls.UIRadioButton();
			this.rbUseYn_Y = new Janus.Windows.EditControls.UIRadioButton();
			this.lbUseYn = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.ebContractAmt = new Janus.Windows.GridEX.EditControls.NumericEditBox();
			this.label1 = new System.Windows.Forms.Label();
			this.ebPrice = new Janus.Windows.GridEX.EditControls.NumericEditBox();
			this.lbAdTime = new System.Windows.Forms.Label();
			this.label10 = new System.Windows.Forms.Label();
			this.btnRapSearch = new Janus.Windows.EditControls.UIButton();
			this.ebRapName = new Janus.Windows.GridEX.EditControls.EditBox();
			this.lbContractPackageAmt = new System.Windows.Forms.Label();
			this.ebComment = new Janus.Windows.GridEX.EditControls.EditBox();
			this.ebRegName = new Janus.Windows.GridEX.EditControls.EditBox();
			this.lbRegID = new System.Windows.Forms.Label();
			this.ebModDt = new Janus.Windows.GridEX.EditControls.EditBox();
			this.lbModDt = new System.Windows.Forms.Label();
			this.lbRegDt = new System.Windows.Forms.Label();
			this.ebRegDt = new Janus.Windows.GridEX.EditControls.EditBox();
			this.ebPackageName = new Janus.Windows.GridEX.EditControls.EditBox();
			this.lbPackageName = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.label5 = new System.Windows.Forms.Label();
			this.btnAdd = new Janus.Windows.EditControls.UIButton();
			this.btnDelete = new Janus.Windows.EditControls.UIButton();
			this.btnSave = new Janus.Windows.EditControls.UIButton();
			this.label2 = new System.Windows.Forms.Label();
			this.ebBonusRate = new Janus.Windows.GridEX.EditControls.NumericEditBox();
			this.panel1 = new System.Windows.Forms.Panel();
			this.gridEX1 = new Janus.Windows.GridEX.GridEX();
			((System.ComponentModel.ISupportInitialize)(this.uiPM)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.uiPanelContractPackage)).BeginInit();
			this.uiPanelContractPackage.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.uiPanelSearch)).BeginInit();
			this.uiPanelSearch.SuspendLayout();
			this.uiPanelSearchContainer.SuspendLayout();
			this.pnlSearch.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.uiPanelList)).BeginInit();
			this.uiPanelList.SuspendLayout();
			this.uiPanelListContainer.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.grdExContractPackageList)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.dvContractPackage)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.contractPackageDs)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.uiPanelDetail)).BeginInit();
			this.uiPanelDetail.SuspendLayout();
			this.uiPanelDetailContainer.SuspendLayout();
			this.pnlUserDetail.SuspendLayout();
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
			this.uiPanelContractPackage.Id = new System.Guid("b162c53a-7940-47cd-a0ed-9762c59c50d8");
			this.uiPanelContractPackage.StaticGroup = true;
			this.uiPanelSearch.Id = new System.Guid("afb3b7d0-aa79-443c-a1e8-3baba38f7a4d");
			this.uiPanelContractPackage.Panels.Add(this.uiPanelSearch);
			this.uiPanelList.Id = new System.Guid("8e3140b9-bfcb-43af-b84e-2ee6cddf51dc");
			this.uiPanelContractPackage.Panels.Add(this.uiPanelList);
			this.uiPanelDetail.Id = new System.Guid("b2973a57-d3e1-4d80-9387-7a6dc3332542");
			this.uiPanelContractPackage.Panels.Add(this.uiPanelDetail);
			this.uiPM.Panels.Add(this.uiPanelContractPackage);
			// 
			// Design Time Panel Info:
			// 
			this.uiPM.BeginPanelInfo();
			this.uiPM.AddDockPanelInfo(new System.Guid("b162c53a-7940-47cd-a0ed-9762c59c50d8"), Janus.Windows.UI.Dock.PanelGroupStyle.HorizontalTiles, Janus.Windows.UI.Dock.PanelDockStyle.Fill, true, new System.Drawing.Size(1010, 677), true);
			this.uiPM.AddDockPanelInfo(new System.Guid("afb3b7d0-aa79-443c-a1e8-3baba38f7a4d"), new System.Guid("b162c53a-7940-47cd-a0ed-9762c59c50d8"), 41, true);
			this.uiPM.AddDockPanelInfo(new System.Guid("8e3140b9-bfcb-43af-b84e-2ee6cddf51dc"), new System.Guid("b162c53a-7940-47cd-a0ed-9762c59c50d8"), 364, true);
			this.uiPM.AddDockPanelInfo(new System.Guid("b2973a57-d3e1-4d80-9387-7a6dc3332542"), new System.Guid("b162c53a-7940-47cd-a0ed-9762c59c50d8"), 222, true);
			this.uiPM.AddFloatingPanelInfo(new System.Guid("b162c53a-7940-47cd-a0ed-9762c59c50d8"), Janus.Windows.UI.Dock.PanelGroupStyle.HorizontalTiles, true, new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
			this.uiPM.AddFloatingPanelInfo(new System.Guid("8e3140b9-bfcb-43af-b84e-2ee6cddf51dc"), new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
			this.uiPM.AddFloatingPanelInfo(new System.Guid("b2973a57-d3e1-4d80-9387-7a6dc3332542"), new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
			this.uiPM.AddFloatingPanelInfo(new System.Guid("afb3b7d0-aa79-443c-a1e8-3baba38f7a4d"), new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
			this.uiPM.EndPanelInfo();
			// 
			// uiPanelContractPackage
			// 
			this.uiPanelContractPackage.ActiveCaptionMode = Janus.Windows.UI.Dock.ActiveCaptionMode.Never;
			this.uiPanelContractPackage.AllowResize = Janus.Windows.UI.InheritableBoolean.False;
			this.uiPanelContractPackage.BackColor = System.Drawing.SystemColors.Window;
			this.uiPanelContractPackage.CaptionVisible = Janus.Windows.UI.InheritableBoolean.True;
			this.uiPanelContractPackage.CloseButtonVisible = Janus.Windows.UI.InheritableBoolean.False;
			this.uiPanelContractPackage.Location = new System.Drawing.Point(0, 0);
			this.uiPanelContractPackage.Name = "uiPanelContractPackage";
			this.uiPanelContractPackage.Size = new System.Drawing.Size(1010, 677);
			this.uiPanelContractPackage.TabIndex = 4;
			this.uiPanelContractPackage.Text = "광고상품관리";
			// 
			// uiPanelSearch
			// 
			this.uiPanelSearch.AllowResize = Janus.Windows.UI.InheritableBoolean.False;
			this.uiPanelSearch.CaptionVisible = Janus.Windows.UI.InheritableBoolean.False;
			this.uiPanelSearch.CloseButtonVisible = Janus.Windows.UI.InheritableBoolean.False;
			this.uiPanelSearch.InnerContainer = this.uiPanelSearchContainer;
			this.uiPanelSearch.Location = new System.Drawing.Point(0, 22);
			this.uiPanelSearch.Name = "uiPanelSearch";
			this.uiPanelSearch.Size = new System.Drawing.Size(1010, 42);
			this.uiPanelSearch.TabIndex = 4;
			this.uiPanelSearch.Text = "검색";
			// 
			// uiPanelSearchContainer
			// 
			this.uiPanelSearchContainer.Controls.Add(this.pnlSearch);
			this.uiPanelSearchContainer.Location = new System.Drawing.Point(1, 1);
			this.uiPanelSearchContainer.Name = "uiPanelSearchContainer";
			this.uiPanelSearchContainer.Size = new System.Drawing.Size(1008, 40);
			this.uiPanelSearchContainer.TabIndex = 0;
			// 
			// pnlSearch
			// 
			this.pnlSearch.BackColor = System.Drawing.SystemColors.Window;
			this.pnlSearch.Controls.Add(this.chkSearchUse);
			this.pnlSearch.Controls.Add(this.ebSearchKey);
			this.pnlSearch.Controls.Add(this.cbSearchRap);
			this.pnlSearch.Controls.Add(this.btnSearch);
			this.pnlSearch.Dock = System.Windows.Forms.DockStyle.Fill;
			this.pnlSearch.Location = new System.Drawing.Point(0, 0);
			this.pnlSearch.Name = "pnlSearch";
			this.pnlSearch.Size = new System.Drawing.Size(1008, 40);
			this.pnlSearch.TabIndex = 3;
			// 
			// chkSearchUse
			// 
			this.chkSearchUse.Location = new System.Drawing.Point(342, 8);
			this.chkSearchUse.Name = "chkSearchUse";
			this.chkSearchUse.Size = new System.Drawing.Size(104, 23);
			this.chkSearchUse.TabIndex = 5;
			this.chkSearchUse.Text = "사용안함 포함";
			// 
			// ebSearchKey
			// 
			this.ebSearchKey.Location = new System.Drawing.Point(136, 8);
			this.ebSearchKey.Name = "ebSearchKey";
			this.ebSearchKey.Size = new System.Drawing.Size(200, 20);
			this.ebSearchKey.TabIndex = 2;
			this.ebSearchKey.Text = "검색어를 입력하세요";
			this.ebSearchKey.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
			this.ebSearchKey.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
			this.ebSearchKey.TextChanged += new System.EventHandler(this.ebSearchKey_TextChanged);
			this.ebSearchKey.Click += new System.EventHandler(this.ebSearchKey_Click);
			this.ebSearchKey.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ebSearchKey_KeyDown);
			// 
			// cbSearchRap
			// 
			this.cbSearchRap.ComboStyle = Janus.Windows.EditControls.ComboStyle.DropDownList;
			this.cbSearchRap.Location = new System.Drawing.Point(8, 8);
			this.cbSearchRap.Name = "cbSearchRap";
			this.cbSearchRap.Size = new System.Drawing.Size(120, 20);
			this.cbSearchRap.TabIndex = 1;
			this.cbSearchRap.Text = "미디어렙선택";
			this.cbSearchRap.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
			// 
			// btnSearch
			// 
			this.btnSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnSearch.Enabled = false;
			this.btnSearch.Location = new System.Drawing.Point(895, 8);
			this.btnSearch.Name = "btnSearch";
			this.btnSearch.Size = new System.Drawing.Size(104, 24);
			this.btnSearch.TabIndex = 4;
			this.btnSearch.Text = "조 회";
			this.btnSearch.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
			this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
			// 
			// uiPanelList
			// 
			this.uiPanelList.BackColor = System.Drawing.SystemColors.Window;
			this.uiPanelList.CaptionStyle = Janus.Windows.UI.Dock.PanelCaptionStyle.Dark;
			this.uiPanelList.InnerContainer = this.uiPanelListContainer;
			this.uiPanelList.Location = new System.Drawing.Point(0, 68);
			this.uiPanelList.MinimumSize = new System.Drawing.Size(-1, 100);
			this.uiPanelList.Name = "uiPanelList";
			this.uiPanelList.Size = new System.Drawing.Size(1010, 376);
			this.uiPanelList.TabIndex = 8;
			this.uiPanelList.TabStop = false;
			this.uiPanelList.Text = "광고상품목록";
			// 
			// uiPanelListContainer
			// 
			this.uiPanelListContainer.BackColor = System.Drawing.SystemColors.Window;
			this.uiPanelListContainer.Controls.Add(this.grdExContractPackageList);
			this.uiPanelListContainer.Location = new System.Drawing.Point(1, 23);
			this.uiPanelListContainer.Name = "uiPanelListContainer";
			this.uiPanelListContainer.Size = new System.Drawing.Size(1008, 352);
			this.uiPanelListContainer.TabIndex = 0;
			// 
			// grdExContractPackageList
			// 
			this.grdExContractPackageList.AllowEdit = Janus.Windows.GridEX.InheritableBoolean.False;
			this.grdExContractPackageList.AlternatingColors = true;
			this.grdExContractPackageList.BorderStyle = Janus.Windows.GridEX.BorderStyle.None;
			this.grdExContractPackageList.DataSource = this.dvContractPackage;
			grdExContractPackageList_DesignTimeLayout.LayoutString = resources.GetString("grdExContractPackageList_DesignTimeLayout.LayoutString");
			this.grdExContractPackageList.DesignTimeLayout = grdExContractPackageList_DesignTimeLayout;
			this.grdExContractPackageList.Dock = System.Windows.Forms.DockStyle.Fill;
			this.grdExContractPackageList.EditorsControlStyle.ButtonAppearance = Janus.Windows.GridEX.ButtonAppearance.Regular;
			this.grdExContractPackageList.EmptyRows = true;
			this.grdExContractPackageList.FocusCellFormatStyle.BackColor = System.Drawing.SystemColors.Highlight;
			this.grdExContractPackageList.FocusCellFormatStyle.ForeColor = System.Drawing.Color.White;
			this.grdExContractPackageList.FocusStyle = Janus.Windows.GridEX.FocusStyle.None;
			this.grdExContractPackageList.GridLineColor = System.Drawing.Color.Silver;
			this.grdExContractPackageList.GridLines = Janus.Windows.GridEX.GridLines.Vertical;
			this.grdExContractPackageList.GridLineStyle = Janus.Windows.GridEX.GridLineStyle.Solid;
			this.grdExContractPackageList.GroupByBoxVisible = false;
			this.grdExContractPackageList.HeaderFormatStyle.TextAlignment = Janus.Windows.GridEX.TextAlignment.Center;
			this.grdExContractPackageList.HideSelection = Janus.Windows.GridEX.HideSelection.HighlightInactive;
			this.grdExContractPackageList.Location = new System.Drawing.Point(0, 0);
			this.grdExContractPackageList.Name = "grdExContractPackageList";
			this.grdExContractPackageList.ScrollBars = Janus.Windows.GridEX.ScrollBars.Vertical;
			this.grdExContractPackageList.SelectedInactiveFormatStyle.BackColor = System.Drawing.Color.Gold;
			this.grdExContractPackageList.Size = new System.Drawing.Size(1008, 352);
			this.grdExContractPackageList.TabIndex = 5;
			this.grdExContractPackageList.TabKeyBehavior = Janus.Windows.GridEX.TabKeyBehavior.ControlNavigation;
			this.grdExContractPackageList.ThemedAreas = ((Janus.Windows.GridEX.ThemedArea)(((((((((Janus.Windows.GridEX.ThemedArea.ScrollBars | Janus.Windows.GridEX.ThemedArea.EditControls) 
            | Janus.Windows.GridEX.ThemedArea.Headers) 
            | Janus.Windows.GridEX.ThemedArea.GroupByBox) 
            | Janus.Windows.GridEX.ThemedArea.GroupRows) 
            | Janus.Windows.GridEX.ThemedArea.ControlBorder) 
            | Janus.Windows.GridEX.ThemedArea.Cards) 
            | Janus.Windows.GridEX.ThemedArea.Gridlines) 
            | Janus.Windows.GridEX.ThemedArea.CheckBoxes)));
			this.grdExContractPackageList.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
			this.grdExContractPackageList.Enter += new System.EventHandler(this.OnGrdRowChanged);
			// 
			// dvContractPackage
			// 
			this.dvContractPackage.Table = this.contractPackageDs.Package;
			// 
			// contractPackageDs
			// 
			this.contractPackageDs.DataSetName = "ContractPackageDs";
			this.contractPackageDs.Locale = new System.Globalization.CultureInfo("en-US");
			this.contractPackageDs.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
			// 
			// uiPanelDetail
			// 
			this.uiPanelDetail.AllowResize = Janus.Windows.UI.InheritableBoolean.False;
			this.uiPanelDetail.BackColor = System.Drawing.SystemColors.Window;
			this.uiPanelDetail.CaptionStyle = Janus.Windows.UI.Dock.PanelCaptionStyle.Dark;
			this.uiPanelDetail.InnerContainer = this.uiPanelDetailContainer;
			this.uiPanelDetail.Location = new System.Drawing.Point(0, 448);
			this.uiPanelDetail.Name = "uiPanelDetail";
			this.uiPanelDetail.Size = new System.Drawing.Size(1010, 229);
			this.uiPanelDetail.TabIndex = 10;
			this.uiPanelDetail.TabStop = false;
			this.uiPanelDetail.Text = "상세정보";
			// 
			// uiPanelDetailContainer
			// 
			this.uiPanelDetailContainer.Controls.Add(this.pnlUserDetail);
			this.uiPanelDetailContainer.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.uiPanelDetailContainer.Location = new System.Drawing.Point(1, 23);
			this.uiPanelDetailContainer.Name = "uiPanelDetailContainer";
			this.uiPanelDetailContainer.Size = new System.Drawing.Size(1008, 205);
			this.uiPanelDetailContainer.TabIndex = 0;
			// 
			// pnlUserDetail
			// 
			this.pnlUserDetail.BackColor = System.Drawing.SystemColors.Window;
			this.pnlUserDetail.Controls.Add(this.chkAllRep);
			this.pnlUserDetail.Controls.Add(this.ebAdTime);
			this.pnlUserDetail.Controls.Add(this.rbUseYn_N);
			this.pnlUserDetail.Controls.Add(this.rbUseYn_Y);
			this.pnlUserDetail.Controls.Add(this.lbUseYn);
			this.pnlUserDetail.Controls.Add(this.label4);
			this.pnlUserDetail.Controls.Add(this.ebContractAmt);
			this.pnlUserDetail.Controls.Add(this.label1);
			this.pnlUserDetail.Controls.Add(this.ebPrice);
			this.pnlUserDetail.Controls.Add(this.lbAdTime);
			this.pnlUserDetail.Controls.Add(this.label10);
			this.pnlUserDetail.Controls.Add(this.btnRapSearch);
			this.pnlUserDetail.Controls.Add(this.ebRapName);
			this.pnlUserDetail.Controls.Add(this.lbContractPackageAmt);
			this.pnlUserDetail.Controls.Add(this.ebComment);
			this.pnlUserDetail.Controls.Add(this.ebRegName);
			this.pnlUserDetail.Controls.Add(this.lbRegID);
			this.pnlUserDetail.Controls.Add(this.ebModDt);
			this.pnlUserDetail.Controls.Add(this.lbModDt);
			this.pnlUserDetail.Controls.Add(this.lbRegDt);
			this.pnlUserDetail.Controls.Add(this.ebRegDt);
			this.pnlUserDetail.Controls.Add(this.ebPackageName);
			this.pnlUserDetail.Controls.Add(this.lbPackageName);
			this.pnlUserDetail.Controls.Add(this.label3);
			this.pnlUserDetail.Controls.Add(this.label5);
			this.pnlUserDetail.Controls.Add(this.btnAdd);
			this.pnlUserDetail.Controls.Add(this.btnDelete);
			this.pnlUserDetail.Controls.Add(this.btnSave);
			this.pnlUserDetail.Controls.Add(this.label2);
			this.pnlUserDetail.Controls.Add(this.ebBonusRate);
			this.pnlUserDetail.Dock = System.Windows.Forms.DockStyle.Fill;
			this.pnlUserDetail.Location = new System.Drawing.Point(0, 0);
			this.pnlUserDetail.Name = "pnlUserDetail";
			this.pnlUserDetail.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.pnlUserDetail.Size = new System.Drawing.Size(1008, 205);
			this.pnlUserDetail.TabIndex = 0;
			// 
			// chkAllRep
			// 
			this.chkAllRep.Location = new System.Drawing.Point(448, 11);
			this.chkAllRep.Name = "chkAllRep";
			this.chkAllRep.Size = new System.Drawing.Size(104, 23);
			this.chkAllRep.TabIndex = 5;
			this.chkAllRep.Text = "미디어렙 공용";
			// 
			// ebAdTime
			// 
			this.ebAdTime.DecimalDigits = 0;
			this.ebAdTime.FormatString = "#,##0";
			this.ebAdTime.Location = new System.Drawing.Point(80, 64);
			this.ebAdTime.MaxLength = 4;
			this.ebAdTime.Name = "ebAdTime";
			this.ebAdTime.Size = new System.Drawing.Size(56, 20);
			this.ebAdTime.TabIndex = 11;
			this.ebAdTime.Text = "15";
			this.ebAdTime.TextAlignment = Janus.Windows.GridEX.TextAlignment.Far;
			this.ebAdTime.Value = new decimal(new int[] {
            15,
            0,
            0,
            0});
			this.ebAdTime.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
			// 
			// rbUseYn_N
			// 
			this.rbUseYn_N.BackColor = System.Drawing.Color.Transparent;
			this.rbUseYn_N.Location = new System.Drawing.Point(720, 136);
			this.rbUseYn_N.Name = "rbUseYn_N";
			this.rbUseYn_N.Size = new System.Drawing.Size(72, 23);
			this.rbUseYn_N.TabIndex = 15;
			this.rbUseYn_N.Text = "사용안함";
			this.rbUseYn_N.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
			// 
			// rbUseYn_Y
			// 
			this.rbUseYn_Y.BackColor = System.Drawing.Color.Transparent;
			this.rbUseYn_Y.Checked = true;
			this.rbUseYn_Y.Location = new System.Drawing.Point(648, 136);
			this.rbUseYn_Y.Name = "rbUseYn_Y";
			this.rbUseYn_Y.Size = new System.Drawing.Size(72, 23);
			this.rbUseYn_Y.TabIndex = 15;
			this.rbUseYn_Y.TabStop = true;
			this.rbUseYn_Y.Text = "사용함";
			this.rbUseYn_Y.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
			// 
			// lbUseYn
			// 
			this.lbUseYn.Location = new System.Drawing.Point(568, 136);
			this.lbUseYn.Name = "lbUseYn";
			this.lbUseYn.Size = new System.Drawing.Size(72, 21);
			this.lbUseYn.TabIndex = 190;
			this.lbUseYn.Text = "사용여부";
			this.lbUseYn.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(320, 64);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(16, 21);
			this.label4.TabIndex = 186;
			this.label4.Text = "%";
			this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// ebContractAmt
			// 
			this.ebContractAmt.DecimalDigits = 0;
			this.ebContractAmt.FormatString = "#,##0";
			this.ebContractAmt.Location = new System.Drawing.Point(640, 40);
			this.ebContractAmt.MaxLength = 17;
			this.ebContractAmt.Name = "ebContractAmt";
			this.ebContractAmt.Size = new System.Drawing.Size(192, 20);
			this.ebContractAmt.TabIndex = 10;
			this.ebContractAmt.Text = "0";
			this.ebContractAmt.TextAlignment = Janus.Windows.GridEX.TextAlignment.Far;
			this.ebContractAmt.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
			this.ebContractAmt.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
			// 
			// label1
			// 
			this.label1.BackColor = System.Drawing.Color.Transparent;
			this.label1.Location = new System.Drawing.Point(368, 64);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(64, 21);
			this.label1.TabIndex = 182;
			this.label1.Text = "광고단가";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// ebPrice
			// 
			this.ebPrice.DecimalDigits = 0;
			this.ebPrice.FormatString = "#,##0";
			this.ebPrice.Location = new System.Drawing.Point(432, 64);
			this.ebPrice.MaxLength = 17;
			this.ebPrice.Name = "ebPrice";
			this.ebPrice.Size = new System.Drawing.Size(120, 20);
			this.ebPrice.TabIndex = 13;
			this.ebPrice.Text = "0";
			this.ebPrice.TextAlignment = Janus.Windows.GridEX.TextAlignment.Far;
			this.ebPrice.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
			this.ebPrice.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
			// 
			// lbAdTime
			// 
			this.lbAdTime.BackColor = System.Drawing.Color.Transparent;
			this.lbAdTime.Location = new System.Drawing.Point(8, 64);
			this.lbAdTime.Name = "lbAdTime";
			this.lbAdTime.Size = new System.Drawing.Size(56, 21);
			this.lbAdTime.TabIndex = 121;
			this.lbAdTime.Text = "광고길이";
			this.lbAdTime.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label10
			// 
			this.label10.BackColor = System.Drawing.Color.Transparent;
			this.label10.Location = new System.Drawing.Point(144, 64);
			this.label10.Name = "label10";
			this.label10.Size = new System.Drawing.Size(24, 21);
			this.label10.TabIndex = 120;
			this.label10.Text = "초";
			this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// btnRapSearch
			// 
			this.btnRapSearch.Enabled = false;
			this.btnRapSearch.Location = new System.Drawing.Point(280, 8);
			this.btnRapSearch.Name = "btnRapSearch";
			this.btnRapSearch.Size = new System.Drawing.Size(104, 23);
			this.btnRapSearch.TabIndex = 7;
			this.btnRapSearch.Text = "미디어렙선택";
			this.btnRapSearch.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
			this.btnRapSearch.Click += new System.EventHandler(this.btnRapSearch_Click);
			// 
			// ebRapName
			// 
			this.ebRapName.BackColor = System.Drawing.Color.WhiteSmoke;
			this.ebRapName.Location = new System.Drawing.Point(80, 8);
			this.ebRapName.Name = "ebRapName";
			this.ebRapName.Size = new System.Drawing.Size(192, 20);
			this.ebRapName.TabIndex = 6;
			this.ebRapName.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
			this.ebRapName.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
			// 
			// lbContractPackageAmt
			// 
			this.lbContractPackageAmt.Location = new System.Drawing.Point(568, 40);
			this.lbContractPackageAmt.Name = "lbContractPackageAmt";
			this.lbContractPackageAmt.Size = new System.Drawing.Size(56, 21);
			this.lbContractPackageAmt.TabIndex = 109;
			this.lbContractPackageAmt.Text = "계약물량";
			this.lbContractPackageAmt.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// ebComment
			// 
			this.ebComment.Location = new System.Drawing.Point(80, 88);
			this.ebComment.MaxLength = 50;
			this.ebComment.Multiline = true;
			this.ebComment.Name = "ebComment";
			this.ebComment.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.ebComment.Size = new System.Drawing.Size(472, 72);
			this.ebComment.TabIndex = 14;
			this.ebComment.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
			this.ebComment.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
			// 
			// ebRegName
			// 
			this.ebRegName.BackColor = System.Drawing.Color.WhiteSmoke;
			this.ebRegName.Location = new System.Drawing.Point(640, 112);
			this.ebRegName.MaxLength = 15;
			this.ebRegName.Name = "ebRegName";
			this.ebRegName.ReadOnly = true;
			this.ebRegName.Size = new System.Drawing.Size(192, 20);
			this.ebRegName.TabIndex = 16;
			this.ebRegName.TabStop = false;
			this.ebRegName.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
			this.ebRegName.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
			// 
			// lbRegID
			// 
			this.lbRegID.Location = new System.Drawing.Point(568, 112);
			this.lbRegID.Name = "lbRegID";
			this.lbRegID.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.lbRegID.Size = new System.Drawing.Size(72, 21);
			this.lbRegID.TabIndex = 29;
			this.lbRegID.Text = "등록자  ";
			this.lbRegID.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// ebModDt
			// 
			this.ebModDt.BackColor = System.Drawing.Color.WhiteSmoke;
			this.ebModDt.Location = new System.Drawing.Point(640, 88);
			this.ebModDt.Name = "ebModDt";
			this.ebModDt.ReadOnly = true;
			this.ebModDt.Size = new System.Drawing.Size(192, 20);
			this.ebModDt.TabIndex = 21;
			this.ebModDt.TabStop = false;
			this.ebModDt.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
			this.ebModDt.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
			// 
			// lbModDt
			// 
			this.lbModDt.Location = new System.Drawing.Point(568, 88);
			this.lbModDt.Name = "lbModDt";
			this.lbModDt.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.lbModDt.Size = new System.Drawing.Size(72, 21);
			this.lbModDt.TabIndex = 29;
			this.lbModDt.Text = "최종수정";
			this.lbModDt.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// lbRegDt
			// 
			this.lbRegDt.Location = new System.Drawing.Point(568, 64);
			this.lbRegDt.Name = "lbRegDt";
			this.lbRegDt.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.lbRegDt.Size = new System.Drawing.Size(72, 21);
			this.lbRegDt.TabIndex = 29;
			this.lbRegDt.Text = "등록일시";
			this.lbRegDt.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// ebRegDt
			// 
			this.ebRegDt.BackColor = System.Drawing.Color.WhiteSmoke;
			this.ebRegDt.Location = new System.Drawing.Point(640, 64);
			this.ebRegDt.Name = "ebRegDt";
			this.ebRegDt.ReadOnly = true;
			this.ebRegDt.Size = new System.Drawing.Size(192, 20);
			this.ebRegDt.TabIndex = 18;
			this.ebRegDt.TabStop = false;
			this.ebRegDt.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
			this.ebRegDt.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
			// 
			// ebPackageName
			// 
			this.ebPackageName.Location = new System.Drawing.Point(80, 40);
			this.ebPackageName.MaxLength = 50;
			this.ebPackageName.Name = "ebPackageName";
			this.ebPackageName.Size = new System.Drawing.Size(472, 20);
			this.ebPackageName.TabIndex = 9;
			this.ebPackageName.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
			this.ebPackageName.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
			// 
			// lbPackageName
			// 
			this.lbPackageName.Location = new System.Drawing.Point(8, 40);
			this.lbPackageName.Name = "lbPackageName";
			this.lbPackageName.Size = new System.Drawing.Size(72, 21);
			this.lbPackageName.TabIndex = 18;
			this.lbPackageName.Text = "상품명";
			this.lbPackageName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(8, 8);
			this.label3.Name = "label3";
			this.label3.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.label3.Size = new System.Drawing.Size(72, 21);
			this.label3.TabIndex = 18;
			this.label3.Text = "미디어렙";
			this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label5
			// 
			this.label5.Location = new System.Drawing.Point(8, 88);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(72, 21);
			this.label5.TabIndex = 46;
			this.label5.Text = "비고";
			this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// btnAdd
			// 
			this.btnAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.btnAdd.BackColor = System.Drawing.SystemColors.Window;
			this.btnAdd.Enabled = false;
			this.btnAdd.Location = new System.Drawing.Point(231, 175);
			this.btnAdd.Name = "btnAdd";
			this.btnAdd.Size = new System.Drawing.Size(104, 24);
			this.btnAdd.TabIndex = 18;
			this.btnAdd.Text = "추 가";
			this.btnAdd.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
			this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
			// 
			// btnDelete
			// 
			this.btnDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.btnDelete.BackColor = System.Drawing.SystemColors.Window;
			this.btnDelete.Enabled = false;
			this.btnDelete.Location = new System.Drawing.Point(119, 175);
			this.btnDelete.Name = "btnDelete";
			this.btnDelete.Size = new System.Drawing.Size(104, 24);
			this.btnDelete.TabIndex = 17;
			this.btnDelete.Text = "삭 제";
			this.btnDelete.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
			this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
			// 
			// btnSave
			// 
			this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.btnSave.BackColor = System.Drawing.SystemColors.Window;
			this.btnSave.Enabled = false;
			this.btnSave.Location = new System.Drawing.Point(8, 175);
			this.btnSave.Name = "btnSave";
			this.btnSave.Size = new System.Drawing.Size(104, 24);
			this.btnSave.TabIndex = 16;
			this.btnSave.Text = "저 장";
			this.btnSave.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
			this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(192, 64);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(72, 21);
			this.label2.TabIndex = 109;
			this.label2.Text = "보너스율";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// ebBonusRate
			// 
			this.ebBonusRate.DecimalDigits = 0;
			this.ebBonusRate.FormatString = "#,##0";
			this.ebBonusRate.Location = new System.Drawing.Point(264, 64);
			this.ebBonusRate.MaxLength = 4;
			this.ebBonusRate.Name = "ebBonusRate";
			this.ebBonusRate.Size = new System.Drawing.Size(56, 20);
			this.ebBonusRate.TabIndex = 12;
			this.ebBonusRate.Text = "0";
			this.ebBonusRate.TextAlignment = Janus.Windows.GridEX.TextAlignment.Far;
			this.ebBonusRate.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
			this.ebBonusRate.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
			// 
			// panel1
			// 
			this.panel1.Location = new System.Drawing.Point(0, 0);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(200, 100);
			this.panel1.TabIndex = 0;
			// 
			// gridEX1
			// 
			this.gridEX1.Location = new System.Drawing.Point(0, 0);
			this.gridEX1.Name = "gridEX1";
			this.gridEX1.Size = new System.Drawing.Size(400, 376);
			this.gridEX1.TabIndex = 0;
			// 
			// ContractPackageControl
			// 
			this.Controls.Add(this.uiPanelContractPackage);
			this.Font = global::AdManagerClient.Properties.Settings.Default.Font1;
			this.Name = "ContractPackageControl";
			this.Size = new System.Drawing.Size(1010, 677);
			this.Load += new System.EventHandler(this.UserControl_Load);
			((System.ComponentModel.ISupportInitialize)(this.uiPM)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.uiPanelContractPackage)).EndInit();
			this.uiPanelContractPackage.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.uiPanelSearch)).EndInit();
			this.uiPanelSearch.ResumeLayout(false);
			this.uiPanelSearchContainer.ResumeLayout(false);
			this.pnlSearch.ResumeLayout(false);
			this.pnlSearch.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.uiPanelList)).EndInit();
			this.uiPanelList.ResumeLayout(false);
			this.uiPanelListContainer.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.grdExContractPackageList)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.dvContractPackage)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.contractPackageDs)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.uiPanelDetail)).EndInit();
			this.uiPanelDetail.ResumeLayout(false);
			this.uiPanelDetailContainer.ResumeLayout(false);
			this.pnlUserDetail.ResumeLayout(false);
			this.pnlUserDetail.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.gridEX1)).EndInit();
			this.ResumeLayout(false);

		}
        #endregion

        #region 컨트롤 로드
        private void UserControl_Load(object sender, System.EventArgs e)
        {
            // 데이터관리용 객체생성
            dt = ((DataView)grdExContractPackageList.DataSource).Table;  
            cm = (CurrencyManager) this.BindingContext[grdExContractPackageList.DataSource]; 
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

            // 조회권한 검사
            if(menu.CanRead(MenuCode))
            {
                canRead = true;
                SearchContractPackage();
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
                //ResetTextReadOnly();
                canUpdate = true;
            }
            else
            {
 //               SetTextReadOnly();
            }

            InitButton();
			ProgressStop();
        }

        private void InitCombo()
        {
            Init_RapCode();
            InitCombo_Level();

        }

        private void InitCombo_Level() 
        {			

            //미디어렙 레벨일경우의 보안등급처리
            if (commonModel.UserLevel == "30")
            {
                //조회 콤보 픽스
                cbSearchRap.SelectedValue = commonModel.RapCode;
                cbSearchRap.ReadOnly = true;                
            }
            Application.DoEvents();
        }   
    

    

        private void Init_RapCode()
        {
            // 랩을 조회한다.
            MediaRapCodeModel mediaRapCodeModel = new MediaRapCodeModel();
            new MediaRapCodeManager(systemModel, commonModel).GetMediaRapCodeList(mediaRapCodeModel);
			
            if (mediaRapCodeModel.ResultCD.Equals("0000"))
            {
                // 데이터셋에 셋팅
                Utility.SetDataTable(contractPackageDs.MediaRap, mediaRapCodeModel.MediaRapCodeDataSet);				
            }

            // 검색조건의 콤보
            this.cbSearchRap.Items.Clear();
            // 콤보박스에 셋트할 코드목록을 담을 Item배열을 선언
            Janus.Windows.EditControls.UIComboBoxItem[] comboItems = new Janus.Windows.EditControls.UIComboBoxItem[mediaRapCodeModel.ResultCnt + 1];

            comboItems[0] = new Janus.Windows.EditControls.UIComboBoxItem("미디어렙선택","00");
			
            for(int i=0;i<mediaRapCodeModel.ResultCnt;i++)
            {
                DataRow row = contractPackageDs.MediaRap.Rows[i];

                string val = row["RapCode"].ToString();
                string txt = row["RapName"].ToString();
                comboItems[i+1] = new Janus.Windows.EditControls.UIComboBoxItem(txt,val);
            }
            // 콤보에 셋트
            this.cbSearchRap.Items.AddRange(comboItems);
            this.cbSearchRap.SelectedIndex = 0;

            Application.DoEvents();
        }

        private void InitButton()
        {
            if(canRead)   btnSearch.Enabled = true;
            if(canCreate) btnAdd.Enabled    = true;
            if(canDelete) btnDelete.Enabled    = true;

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

        #region 액션처리 메소드

        /// <summary>
        /// 그리드의 Row변경시
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnGrdRowChanged(object sender, System.EventArgs e) 
        {
            if (!IsSearching) // 2011.11.29 JH.Park 조회중이 아닐경우에만 동작하도록 변경
            {
                if (grdExContractPackageList.RowCount > 0)
                {
                    SetDetailText();
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
			ProgressStart();
            DisableButton();
            SearchContractPackage();
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
            if(canCreate)btnSave.Enabled   = true;
            
            IsAdding = true;
            ResetDetailText();
			if(canCreate) ResetTextReadOnly();
            ebPackageName.Focus();	// 추가시 최초 위치
        }

        /// <summary>
        /// 저장버튼 클릭
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_Click(object sender, System.EventArgs e)
        {
            SaveContractPackageDetail();
        }

        /// <summary>
        /// 삭제버튼 클릭
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDelete_Click(object sender, System.EventArgs e)
        {
            DeleteContractPackage();
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
                SearchContractPackage();
            }
        }


		private void btnRapSearch_Click(object sender, System.EventArgs e)
		{
			ContractPackage_RepSearchForm rapForm = new ContractPackage_RepSearchForm(this);
			rapForm.ShowDialog();            
			rapForm.Dispose();
			rapForm = null;			
		}

        #endregion

        #region 처리메소드

        /// <summary>
        /// 광고계약목록 조회
        /// </summary>
        private void SearchContractPackage()
        {
            IsSearching = true;

            StatusMessage("광고계약 정보를 조회합니다.");

            try
            {
				
                //저장 전에 모델을 초기화 해준다.
                contractPackageModel.Init();				
                // 데이터모델에 전송할 내용을 셋트한다.
                contractPackageModel.SearchRap        = cbSearchRap.SelectedValue.ToString();
				
                if(IsNewSearchKey)
                {
                    contractPackageModel.SearchKey = "";					
                }
                else
                {
                    contractPackageModel.SearchKey  = ebSearchKey.Text;
                }
				
				if(chkSearchUse.Checked)
				{
					contractPackageModel.SearchUse = "N";
				}
				else
				{
					contractPackageModel.SearchUse = "Y";
				}
                
                ResetDetailText();
                // 광고 계약 목록 서비스를 호출한다.
                new ContractPackageManager(systemModel,commonModel).GetContractPackageList(contractPackageModel);				

                if (contractPackageModel.ResultCD.Equals("0000"))
                {											
                    Utility.SetDataTable(contractPackageDs.Package, contractPackageModel.PackageDataSet);			
                    StatusMessage(contractPackageModel.ResultCnt + "건의 계약정보 정보가 조회되었습니다.");                  
                    if(canUpdate)
                    {						
                        AddSchChoice();									
                    }				
                    SetDetailText();
                }
            }
            catch(FrameException fe)
            {
                FrameSystem.showMsgForm("계약정보조회오류", new string[] {fe.ErrCode, fe.ResultMsg});
            }
            catch(Exception ex)
            {
                FrameSystem.showMsgForm("계약정보조회오류",new string[] {"",ex.Message});
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
                if ( contractPackageDs.Tables["Package"].Rows.Count < 1 ) return;
              
                foreach (DataRow row in contractPackageDs.Tables["Package"].Rows)
                {					
                    if(IsAdding)
                    {
						
                        cm.Position = 0;
                        keyPackageNo = null;
                    }
                    else
                    {						
                        if(row["PackageNo"].ToString().Equals(keyPackageNo))
                        {					
                            cm.Position = rowIndex;
                            break;								
                        }
                    }

                    rowIndex++;
                    grdExContractPackageList.EnsureVisible();
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
        /// 광고상품 저장
        /// </summary>
        private void SaveContractPackageDetail()
        {
		
            StatusMessage("광고상품 정보를 저장합니다.");
                        
            if(ebPackageName.Text.Trim().Length == 0) 
            {
				MessageBox.Show("광고상품명이 입력되지 않았습니다.","광고상품 저장", 
					MessageBoxButtons.OK, MessageBoxIcon.Information );
				ebPackageName.Focus();
				return;	                
            }
			if(ebPackageName.Text.Trim().Length > 50) 
			{
				MessageBox.Show("광고상품명은 50Byte를 초과할 수 없습니다.","광고상품 저장", 
					MessageBoxButtons.OK, MessageBoxIcon.Information );
				ebPackageName.Focus();
				return;	                
			}
			if(ebAdTime.Text.Trim().Length == 0) 
			{
				MessageBox.Show("광고길이가 입력되지 않았습니다.","광고상품 저장", 
					MessageBoxButtons.OK, MessageBoxIcon.Information );
				ebAdTime.Focus();
				return;	                
			}
			if(ebBonusRate.Text.Trim().Length == 0) 
			{
				MessageBox.Show("보너스율이 입력되지 않았습니다.","광고상품 저장", 
					MessageBoxButtons.OK, MessageBoxIcon.Information );
				ebBonusRate.Focus();
				return;	                
			}
			if(ebPrice.Text.Trim().Length == 0) 
			{
				MessageBox.Show("광고단가가 입력되지 않았습니다.","광고상품 저장", 
					MessageBoxButtons.OK, MessageBoxIcon.Information );
				ebPrice.Focus();
				return;	                
			}
			if(ebContractAmt.Text.Trim().Length == 0) 
			{
				MessageBox.Show("계약물량이 입력되지 않았습니다.","광고상품 저장", 
					MessageBoxButtons.OK, MessageBoxIcon.Information );
				ebContractAmt.Focus();
				return;	                
			}
			if(ebComment.Text.Trim().Length > 2000) 
			{
				MessageBox.Show("메모는 200Byte를 초과할 수 없습니다.","광고상품 저장", 
					MessageBoxButtons.OK, MessageBoxIcon.Information );
				ebPackageName.Focus();
				return;	                
			} 
                      
			try
			{
				//저장 전에 모델을 초기화 해준다.
				contractPackageModel.Init();

				// 데이터모델에 전송할 내용을 셋트한다.
				contractPackageModel.PackageNo      = keyPackageNo;
				contractPackageModel.RapCode        = keyRepCode;

				contractPackageModel.PackageName    = ebPackageName.Text;
				contractPackageModel.AdTime         = ebAdTime.Text.Trim().Replace(",","");
				contractPackageModel.BonusRate      = ebBonusRate.Text.Trim().Replace(",","");
				contractPackageModel.Price          = ebPrice.Text.Trim().Replace(",","");
				contractPackageModel.ContractAmt    = ebContractAmt.Text.Trim().Replace(",","");
				contractPackageModel.Comment        = ebComment.Text;
				if(rbUseYn_Y.Checked)
				{
					contractPackageModel.UseYn      = "Y";
				}
				else
				{
					contractPackageModel.UseYn      = "N";
				}

				// 계약정보 상세정보 저장 서비스를 호출한다.
				if (IsAdding)
				{
					new ContractPackageManager(systemModel,commonModel).SetContractPackageAdd(contractPackageModel);
                
					ResetDetailText();
					StatusMessage("광고상품 정보가 추가되었습니다.");
                
				}
				else
				{   
					//저장이 아니고 업데이트일경우에는 계약순번코드를 설정
					new ContractPackageManager(systemModel,commonModel).SetContractPackageUpdate(contractPackageModel);
					StatusMessage("광고상품 정보가 저장되었습니다.");
                
				}

				DisableButton();
				SearchContractPackage();					
				InitButton();
        
			}
			catch(FrameException fe)
			{
				FrameSystem.showMsgForm("광고상품 저장오류", new string[] {fe.ErrCode, fe.ResultMsg});
			}
			catch(Exception ex)
			{
				FrameSystem.showMsgForm("광고상품 저장오류",new string[] {"",ex.Message});
			}
			finally
			{
				IsAdding = false;					
			}
	
        }


        /// <summary>
        /// 계약정보 삭제
        /// </summary>
        private void DeleteContractPackage()
        {
            StatusMessage("광고상품 정보를 삭제합니다.");
            
            if(keyPackageNo.Length == 0) 
            {
                FrameSystem.showMsgForm("광고상품 삭제오류",new string[] {"", "삭제할 광고상품 정보가 없습니다.", "" });
                return;
            }

                

            DialogResult result = MessageBox.Show("해당 광고상품 정보를 삭제 하시겠습니까?","광고상품 삭제",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question,
                MessageBoxDefaultButton.Button2);

            if (result == DialogResult.No) return;

            try 
            {
                // 데이터모델에 전송할 내용을 셋트한다.
				contractPackageModel.PackageNo   = keyPackageNo;
				contractPackageModel.PackageName = keyPackageName;

                // 광고상품 삭제 서비스를 호출한다.
                new ContractPackageManager(systemModel,commonModel).SetContractPackageDelete(contractPackageModel);

                StatusMessage("광고상품 정보가 삭제되었습니다.");			
                
                ResetDetailText();
                DisableButton();
                SearchContractPackage();
                InitButton();


            }
            catch(FrameException fe)
            {
                FrameSystem.showMsgForm("광고상품 삭제오류", new string[] {fe.ErrCode, fe.ResultMsg});
            }
            catch(Exception ex)
            {
                FrameSystem.showMsgForm("광고상품 삭제오류",new string[] {"",ex.Message});
            }	


        }
		
        /// <summary>
        /// 계약정보 상세정보의 셋트
        /// </summary>
        private void SetDetailText()
        {
            int curRow = cm.Position;

            if(curRow >= 0)
            {
				keyPackageNo        = dt.Rows[cm.Position]["PackageNo"].ToString();
				keyPackageName      = dt.Rows[cm.Position]["PackageName"].ToString();
				keyRepCode          = dt.Rows[cm.Position]["RapCode"].ToString();

				ebPackageName.Text  = dt.Rows[cm.Position]["PackageName"].ToString();
				ebRapName.Text      = dt.Rows[cm.Position]["RapName"].ToString();
	            
				ebAdTime.Text       = dt.Rows[cm.Position]["AdTime"].ToString();
				ebBonusRate.Text    = dt.Rows[cm.Position]["BonusRate"].ToString();
				ebPrice.Text        = dt.Rows[cm.Position]["Price"].ToString();
				ebContractAmt.Text  = dt.Rows[cm.Position]["ContractAmt"].ToString();
				ebComment.Text      = dt.Rows[cm.Position]["Comment"].ToString().Replace("\n","\r\n");


                ebModDt.Text        = dt.Rows[cm.Position]["ModDt"].ToString();
                ebRegDt.Text        = dt.Rows[cm.Position]["RegDt"].ToString();
                ebRegName.Text      = dt.Rows[cm.Position]["RegName"].ToString();

				string UseYn        = dt.Rows[cm.Position]["UseYn"].ToString();

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


				if(keyRepCode.Equals("0"))
				{
					chkAllRep.Checked = true;
				}
				else
				{
					chkAllRep.Checked = false;
				}

                IsAdding = false;
				
				if(canUpdate) 
				{
					btnSave.Enabled   = true;
					ResetTextReadOnly();
				}
				else
				{
					btnSave.Enabled   = false;
					SetTextReadOnly();           
				}

				if(canDelete)
				{
					btnDelete.Enabled = true;
				}
				else
				{
					btnDelete.Enabled = false;
				}

            }

            StatusMessage("준비");
        }

        private void ResetDetailText()
        {
            ebRapName.Text       = "";
			ebPackageName.Text   = "";

			ebAdTime.Text        = "15";
			ebBonusRate.Text     = "0";
			ebPrice.Text         = "0";
			ebContractAmt.Text   = "0";
			ebComment.Text       = "";

			ebModDt.Text         = "";
            ebRegDt.Text         = "";
            ebRegName.Text       = "";

			rbUseYn_Y.Checked = true;
			rbUseYn_N.Checked = false;

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

		private void SetTextReadOnly()
		{
			ebPackageName.ReadOnly  = true;
			ebAdTime.ReadOnly       = true;
			ebBonusRate.ReadOnly    = true;
			ebPrice.ReadOnly        = true;
			ebContractAmt.ReadOnly  = true;
			ebComment.ReadOnly      = true;

			rbUseYn_Y.Enabled = false;
			rbUseYn_N.Enabled = false;

			ebPackageName.BackColor = Color.WhiteSmoke;
			ebAdTime.BackColor      = Color.WhiteSmoke;
			ebBonusRate.BackColor   = Color.WhiteSmoke;
			ebPrice.BackColor       = Color.WhiteSmoke;
			ebContractAmt.BackColor = Color.WhiteSmoke;
			ebComment.BackColor     = Color.WhiteSmoke;

			btnRapSearch.Enabled     = false;															

		}

		private void ResetTextReadOnly()
		{
			ebPackageName.ReadOnly  = false;
			ebAdTime.ReadOnly       = false;
			ebBonusRate.ReadOnly    = false;
			ebPrice.ReadOnly        = false;
			ebContractAmt.ReadOnly  = false;
			ebComment.ReadOnly      = false;

			rbUseYn_Y.Enabled = true;
			rbUseYn_N.Enabled = true;


			ebPackageName.BackColor = Color.White;
			ebAdTime.BackColor      = Color.White;
			ebBonusRate.BackColor   = Color.White;
			ebPrice.BackColor       = Color.White;
			ebContractAmt.BackColor = Color.White;
			ebComment.BackColor     = Color.White;


			if(commonModel.UserLevel=="30")
			{			
				btnRapSearch.Enabled     = false;	
			}
			else
			{
				if(chkAllRep.Checked)
				{
					btnRapSearch.Enabled     = false;	
				}
				else
				{
					btnRapSearch.Enabled     = true;
				}
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


		#region 팝업을 위한 메소드

		public void SetMediaRep(string RepCode, string RepName)
		{	
			keyRepCode      = RepCode;
			ebRapName.Text  = RepName;		}


		#endregion

		private void chkAllRep_CheckedChanged(object sender, System.EventArgs e)
		{
			if(chkAllRep.Checked)
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



	}
}

