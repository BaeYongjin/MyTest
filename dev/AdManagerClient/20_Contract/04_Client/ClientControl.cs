// ===============================================================================
// ClientControl for Charites Project
//
// ClientControl.cs
//
// 매체대행광고주정보관리 컨드롤을 정의합니다. 
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

namespace AdManagerClient
{
	/// <summary>
	/// 매체대행광고주관리 컨트롤
	/// </summary>
    public class ClientControl : System.Windows.Forms.UserControl, IUserControl
	{
		#region 이벤트핸들러
		public event StatusEventHandler 			StatusEvent;			// 상태이벤트 핸들러
		public event ProgressEventHandler 			ProgressEvent;			// 처리중이벤트 핸들러
        #endregion
			
		#region 매체대행광고주정의 객체 및 변수

		// 시스템 정보 : 화면공통
		private SystemModel   systemModel   = FrameSystem.oSysModel;
		private CommonModel   commonModel   = FrameSystem.oComModel;
		private Logger        log           = FrameSystem.oLog;
		private MenuPower     menu          = FrameSystem.oMenu;

		// 메뉴코드 : 보안이 필요한 화면에 필요함
		public string        menuCode		= "";

		// 사용할 정보모델
		ClientModel clientModel  = new ClientModel();	// 매체대행광고주정보모델

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

		private string        mediaCode = "";
		private string        rapCode = "";
		private string        agencyCode = "";
		private string        advertiserCode = "";
		private string        Search_advertiserCode = "";
        private Janus.Windows.EditControls.UICheckBox uiCheckBox1;
		
		int LastOrder = 0;

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
		private Janus.Windows.UI.Dock.UIPanel uiPanelClientList;
		private Janus.Windows.UI.Dock.UIPanelInnerContainer uiPanelClientListContainer;
		private Janus.Windows.UI.Dock.UIPanel uiPanelClientDetail;
		private Janus.Windows.UI.Dock.UIPanelInnerContainer uiPanelClientDetailContainer;
		private Janus.Windows.UI.Dock.UIPanel uiPanelClientsSearch;
		private Janus.Windows.UI.Dock.UIPanelInnerContainer uiPanelClientsSearchContainer;
		private Janus.Windows.EditControls.UIButton uiButton2;
		private Janus.Windows.EditControls.UIButton uiButton1;
		private Janus.Windows.UI.Dock.UIPanelGroup uiPanelClients;
		private System.Windows.Forms.Panel pnlSearch;
		private Janus.Windows.EditControls.UIButton btnSearch;
		private System.Windows.Forms.Panel pnlClientDetail;
		private System.Windows.Forms.Label lbRegDt;
		private Janus.Windows.EditControls.UIButton btnAdd;
		private Janus.Windows.EditControls.UIButton btnSave;
		private System.Data.DataView dvClient;
		private Janus.Windows.GridEX.EditControls.EditBox ebRegDt;
		private Janus.Windows.EditControls.UIButton btnDelete;		
		private System.Windows.Forms.Label lbModDt;
		private Janus.Windows.GridEX.EditControls.EditBox ebModDt;
		private Janus.Windows.GridEX.GridEX grdExClientList;
		private System.Windows.Forms.Label lbUseYn;
		private Janus.Windows.EditControls.UIRadioButton rbUseYn_Y;
		private Janus.Windows.EditControls.UIRadioButton rbUseYn_N;
		private Janus.Windows.EditControls.UIComboBox cbSearchMediaName;
		private Janus.Windows.EditControls.UIComboBox cbSearchRapName;		
		private System.Windows.Forms.Label lbRapName;
		private System.Windows.Forms.Label lbMediaName;
		private System.Windows.Forms.Label lbAgencyName;
		private Janus.Windows.EditControls.UIComboBox uiComboBox1;
		private Janus.Windows.EditControls.UIComboBox cbSearchMediaAgency;
		private System.Windows.Forms.Label label1;
		private Janus.Windows.GridEX.EditControls.EditBox ebComment;
		private System.Windows.Forms.Label lbAdvertiserName;
		private AdManagerClient._20_Contract._04_Client.MediaAgencyDs clientDs;
		private Janus.Windows.GridEX.EditControls.EditBox ebSearchKey;
		private System.Windows.Forms.Label lbRegID;
		private Janus.Windows.GridEX.EditControls.EditBox ebRegID;
		private Janus.Windows.GridEX.EditControls.EditBox ebMedia;
		private Janus.Windows.GridEX.EditControls.EditBox ebRap;
		private Janus.Windows.GridEX.EditControls.EditBox ebAgency;
		private Janus.Windows.EditControls.UIButton btnAgencySearch;
		private Janus.Windows.GridEX.EditControls.EditBox ebAdvertiserName;
        private Janus.Windows.EditControls.UIButton btnAdvertiserSearch;
		private Janus.Windows.GridEX.EditControls.EditBox ebSearchAdvertiserName;
		private Janus.Windows.EditControls.UIButton btnAdvertiserSearch_1;
		private System.ComponentModel.IContainer components;

		public ClientControl()
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
			Janus.Windows.GridEX.GridEXLayout grdExClientList_DesignTimeLayout = new Janus.Windows.GridEX.GridEXLayout();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ClientControl));
			this.uiPM = new Janus.Windows.UI.Dock.UIPanelManager(this.components);
			this.uiPanelClients = new Janus.Windows.UI.Dock.UIPanelGroup();
			this.uiPanelClientsSearch = new Janus.Windows.UI.Dock.UIPanel();
			this.uiPanelClientsSearchContainer = new Janus.Windows.UI.Dock.UIPanelInnerContainer();
			this.pnlSearch = new System.Windows.Forms.Panel();
			this.uiCheckBox1 = new Janus.Windows.EditControls.UICheckBox();
			this.ebSearchKey = new Janus.Windows.GridEX.EditControls.EditBox();
			this.cbSearchMediaName = new Janus.Windows.EditControls.UIComboBox();
			this.cbSearchRapName = new Janus.Windows.EditControls.UIComboBox();
			this.cbSearchMediaAgency = new Janus.Windows.EditControls.UIComboBox();
			this.btnSearch = new Janus.Windows.EditControls.UIButton();
			this.btnAdvertiserSearch_1 = new Janus.Windows.EditControls.UIButton();
			this.ebSearchAdvertiserName = new Janus.Windows.GridEX.EditControls.EditBox();
			this.uiPanelClientList = new Janus.Windows.UI.Dock.UIPanel();
			this.uiPanelClientListContainer = new Janus.Windows.UI.Dock.UIPanelInnerContainer();
			this.grdExClientList = new Janus.Windows.GridEX.GridEX();
			this.dvClient = new System.Data.DataView();
			this.clientDs = new AdManagerClient._20_Contract._04_Client.MediaAgencyDs();
			this.uiPanelClientDetail = new Janus.Windows.UI.Dock.UIPanel();
			this.uiPanelClientDetailContainer = new Janus.Windows.UI.Dock.UIPanelInnerContainer();
			this.pnlClientDetail = new System.Windows.Forms.Panel();
			this.ebAdvertiserName = new Janus.Windows.GridEX.EditControls.EditBox();
			this.btnAgencySearch = new Janus.Windows.EditControls.UIButton();
			this.ebAgency = new Janus.Windows.GridEX.EditControls.EditBox();
			this.ebRap = new Janus.Windows.GridEX.EditControls.EditBox();
			this.ebMedia = new Janus.Windows.GridEX.EditControls.EditBox();
			this.ebRegID = new Janus.Windows.GridEX.EditControls.EditBox();
			this.ebRegDt = new Janus.Windows.GridEX.EditControls.EditBox();
			this.lbRegID = new System.Windows.Forms.Label();
			this.ebModDt = new Janus.Windows.GridEX.EditControls.EditBox();
			this.ebComment = new Janus.Windows.GridEX.EditControls.EditBox();
			this.label1 = new System.Windows.Forms.Label();
			this.rbUseYn_N = new Janus.Windows.EditControls.UIRadioButton();
			this.rbUseYn_Y = new Janus.Windows.EditControls.UIRadioButton();
			this.lbUseYn = new System.Windows.Forms.Label();
			this.lbModDt = new System.Windows.Forms.Label();
			this.lbRegDt = new System.Windows.Forms.Label();
			this.lbRapName = new System.Windows.Forms.Label();
			this.lbMediaName = new System.Windows.Forms.Label();
			this.lbAgencyName = new System.Windows.Forms.Label();
			this.lbAdvertiserName = new System.Windows.Forms.Label();
			this.btnDelete = new Janus.Windows.EditControls.UIButton();
			this.btnAdd = new Janus.Windows.EditControls.UIButton();
			this.btnSave = new Janus.Windows.EditControls.UIButton();
			this.btnAdvertiserSearch = new Janus.Windows.EditControls.UIButton();
			this.uiComboBox1 = new Janus.Windows.EditControls.UIComboBox();
			this.uiButton1 = new Janus.Windows.EditControls.UIButton();
			this.uiButton2 = new Janus.Windows.EditControls.UIButton();
			((System.ComponentModel.ISupportInitialize)(this.uiPM)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.uiPanelClients)).BeginInit();
			this.uiPanelClients.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.uiPanelClientsSearch)).BeginInit();
			this.uiPanelClientsSearch.SuspendLayout();
			this.uiPanelClientsSearchContainer.SuspendLayout();
			this.pnlSearch.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.uiPanelClientList)).BeginInit();
			this.uiPanelClientList.SuspendLayout();
			this.uiPanelClientListContainer.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.grdExClientList)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.dvClient)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.clientDs)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.uiPanelClientDetail)).BeginInit();
			this.uiPanelClientDetail.SuspendLayout();
			this.uiPanelClientDetailContainer.SuspendLayout();
			this.pnlClientDetail.SuspendLayout();
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
			this.uiPanelClients.Id = new System.Guid("b162c53a-7940-47cd-a0ed-9762c59c50d8");
			this.uiPanelClients.StaticGroup = true;
			this.uiPanelClientsSearch.Id = new System.Guid("afb3b7d0-aa79-443c-a1e8-3baba38f7a4d");
			this.uiPanelClients.Panels.Add(this.uiPanelClientsSearch);
			this.uiPanelClientList.Id = new System.Guid("8e3140b9-bfcb-43af-b84e-2ee6cddf51dc");
			this.uiPanelClients.Panels.Add(this.uiPanelClientList);
			this.uiPanelClientDetail.Id = new System.Guid("b2973a57-d3e1-4d80-9387-7a6dc3332542");
			this.uiPanelClients.Panels.Add(this.uiPanelClientDetail);
			this.uiPM.Panels.Add(this.uiPanelClients);
			// 
			// Design Time Panel Info:
			// 
			this.uiPM.BeginPanelInfo();
			this.uiPM.AddDockPanelInfo(new System.Guid("b162c53a-7940-47cd-a0ed-9762c59c50d8"), Janus.Windows.UI.Dock.PanelGroupStyle.HorizontalTiles, Janus.Windows.UI.Dock.PanelDockStyle.Fill, true, new System.Drawing.Size(1010, 677), true);
			this.uiPM.AddDockPanelInfo(new System.Guid("afb3b7d0-aa79-443c-a1e8-3baba38f7a4d"), new System.Guid("b162c53a-7940-47cd-a0ed-9762c59c50d8"), 43, true);
			this.uiPM.AddDockPanelInfo(new System.Guid("8e3140b9-bfcb-43af-b84e-2ee6cddf51dc"), new System.Guid("b162c53a-7940-47cd-a0ed-9762c59c50d8"), 377, true);
			this.uiPM.AddDockPanelInfo(new System.Guid("b2973a57-d3e1-4d80-9387-7a6dc3332542"), new System.Guid("b162c53a-7940-47cd-a0ed-9762c59c50d8"), 227, true);
			this.uiPM.AddFloatingPanelInfo(new System.Guid("b162c53a-7940-47cd-a0ed-9762c59c50d8"), Janus.Windows.UI.Dock.PanelGroupStyle.HorizontalTiles, true, new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
			this.uiPM.AddFloatingPanelInfo(new System.Guid("8e3140b9-bfcb-43af-b84e-2ee6cddf51dc"), new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
			this.uiPM.AddFloatingPanelInfo(new System.Guid("b2973a57-d3e1-4d80-9387-7a6dc3332542"), new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
			this.uiPM.AddFloatingPanelInfo(new System.Guid("afb3b7d0-aa79-443c-a1e8-3baba38f7a4d"), new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
			this.uiPM.EndPanelInfo();
			// 
			// uiPanelClients
			// 
			this.uiPanelClients.ActiveCaptionMode = Janus.Windows.UI.Dock.ActiveCaptionMode.Never;
			this.uiPanelClients.BackColor = System.Drawing.SystemColors.Window;
			this.uiPanelClients.CaptionVisible = Janus.Windows.UI.InheritableBoolean.True;
			this.uiPanelClients.CloseButtonVisible = Janus.Windows.UI.InheritableBoolean.False;
			this.uiPanelClients.Location = new System.Drawing.Point(0, 0);
			this.uiPanelClients.Name = "uiPanelClients";
			this.uiPanelClients.Size = new System.Drawing.Size(1010, 677);
			this.uiPanelClients.TabIndex = 4;
			this.uiPanelClients.Text = "대행사별 광고주관리";
			// 
			// uiPanelClientsSearch
			// 
			this.uiPanelClientsSearch.AllowResize = Janus.Windows.UI.InheritableBoolean.False;
			this.uiPanelClientsSearch.CaptionVisible = Janus.Windows.UI.InheritableBoolean.False;
			this.uiPanelClientsSearch.CloseButtonVisible = Janus.Windows.UI.InheritableBoolean.False;
			this.uiPanelClientsSearch.InnerContainer = this.uiPanelClientsSearchContainer;
			this.uiPanelClientsSearch.Location = new System.Drawing.Point(0, 22);
			this.uiPanelClientsSearch.Name = "uiPanelClientsSearch";
			this.uiPanelClientsSearch.Size = new System.Drawing.Size(1010, 43);
			this.uiPanelClientsSearch.TabIndex = 4;
			this.uiPanelClientsSearch.Text = "검색";
			// 
			// uiPanelClientsSearchContainer
			// 
			this.uiPanelClientsSearchContainer.Controls.Add(this.pnlSearch);
			this.uiPanelClientsSearchContainer.Location = new System.Drawing.Point(1, 1);
			this.uiPanelClientsSearchContainer.Name = "uiPanelClientsSearchContainer";
			this.uiPanelClientsSearchContainer.Size = new System.Drawing.Size(1008, 41);
			this.uiPanelClientsSearchContainer.TabIndex = 0;
			// 
			// pnlSearch
			// 
			this.pnlSearch.BackColor = System.Drawing.SystemColors.Window;
			this.pnlSearch.Controls.Add(this.uiCheckBox1);
			this.pnlSearch.Controls.Add(this.ebSearchKey);
			this.pnlSearch.Controls.Add(this.cbSearchMediaName);
			this.pnlSearch.Controls.Add(this.cbSearchRapName);
			this.pnlSearch.Controls.Add(this.cbSearchMediaAgency);
			this.pnlSearch.Controls.Add(this.btnSearch);
			this.pnlSearch.Controls.Add(this.btnAdvertiserSearch_1);
			this.pnlSearch.Controls.Add(this.ebSearchAdvertiserName);
			this.pnlSearch.Dock = System.Windows.Forms.DockStyle.Fill;
			this.pnlSearch.Location = new System.Drawing.Point(0, 0);
			this.pnlSearch.Name = "pnlSearch";
			this.pnlSearch.Size = new System.Drawing.Size(1008, 41);
			this.pnlSearch.TabIndex = 0;
			// 
			// uiCheckBox1
			// 
			this.uiCheckBox1.Location = new System.Drawing.Point(566, 6);
			this.uiCheckBox1.Name = "uiCheckBox1";
			this.uiCheckBox1.Size = new System.Drawing.Size(104, 23);
			this.uiCheckBox1.TabIndex = 9;
			this.uiCheckBox1.Text = "사용안함 포함";
			// 
			// ebSearchKey
			// 
			this.ebSearchKey.Location = new System.Drawing.Point(368, 8);
			this.ebSearchKey.Name = "ebSearchKey";
			this.ebSearchKey.Size = new System.Drawing.Size(192, 20);
			this.ebSearchKey.TabIndex = 4;
			this.ebSearchKey.Text = "검색어를 입력하세요";
			this.ebSearchKey.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
			this.ebSearchKey.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
			this.ebSearchKey.TextChanged += new System.EventHandler(this.ebSearchKey_TextChanged);
			this.ebSearchKey.Click += new System.EventHandler(this.ebSearchKey_Click);
			this.ebSearchKey.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ebSearchKey_KeyDown);
			// 
			// cbSearchMediaName
			// 
			this.cbSearchMediaName.BackColor = System.Drawing.Color.White;
			this.cbSearchMediaName.ComboStyle = Janus.Windows.EditControls.ComboStyle.DropDownList;
			this.cbSearchMediaName.Location = new System.Drawing.Point(8, 8);
			this.cbSearchMediaName.Name = "cbSearchMediaName";
			this.cbSearchMediaName.Size = new System.Drawing.Size(115, 20);
			this.cbSearchMediaName.TabIndex = 1;
			this.cbSearchMediaName.Text = "매체선택";
			this.cbSearchMediaName.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
			// 
			// cbSearchRapName
			// 
			this.cbSearchRapName.ComboStyle = Janus.Windows.EditControls.ComboStyle.DropDownList;
			this.cbSearchRapName.Location = new System.Drawing.Point(128, 8);
			this.cbSearchRapName.Name = "cbSearchRapName";
			this.cbSearchRapName.Size = new System.Drawing.Size(115, 20);
			this.cbSearchRapName.TabIndex = 2;
			this.cbSearchRapName.Text = "미디어렙선택";
			this.cbSearchRapName.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
			// 
			// cbSearchMediaAgency
			// 
			this.cbSearchMediaAgency.ComboStyle = Janus.Windows.EditControls.ComboStyle.DropDownList;
			this.cbSearchMediaAgency.Location = new System.Drawing.Point(248, 8);
			this.cbSearchMediaAgency.Name = "cbSearchMediaAgency";
			this.cbSearchMediaAgency.Size = new System.Drawing.Size(115, 20);
			this.cbSearchMediaAgency.TabIndex = 3;
			this.cbSearchMediaAgency.Text = "대행사선택";
			this.cbSearchMediaAgency.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
			// 
			// btnSearch
			// 
			this.btnSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnSearch.Enabled = false;
			this.btnSearch.Location = new System.Drawing.Point(895, 8);
			this.btnSearch.Name = "btnSearch";
			this.btnSearch.Size = new System.Drawing.Size(104, 24);
			this.btnSearch.TabIndex = 6;
			this.btnSearch.Text = "조 회";
			this.btnSearch.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
			this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
			// 
			// btnAdvertiserSearch_1
			// 
			this.btnAdvertiserSearch_1.Enabled = false;
			this.btnAdvertiserSearch_1.Location = new System.Drawing.Point(192, 34);
			this.btnAdvertiserSearch_1.Name = "btnAdvertiserSearch_1";
			this.btnAdvertiserSearch_1.Size = new System.Drawing.Size(104, 21);
			this.btnAdvertiserSearch_1.TabIndex = 8;
			this.btnAdvertiserSearch_1.Text = "광고주선택";
			this.btnAdvertiserSearch_1.Visible = false;
			this.btnAdvertiserSearch_1.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
			this.btnAdvertiserSearch_1.Click += new System.EventHandler(this.btnAdvertiserSearch_1_Click);
			// 
			// ebSearchAdvertiserName
			// 
			this.ebSearchAdvertiserName.BackColor = System.Drawing.Color.WhiteSmoke;
			this.ebSearchAdvertiserName.Font = new System.Drawing.Font("굴림체", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
			this.ebSearchAdvertiserName.Location = new System.Drawing.Point(8, 34);
			this.ebSearchAdvertiserName.MaxLength = 20;
			this.ebSearchAdvertiserName.Name = "ebSearchAdvertiserName";
			this.ebSearchAdvertiserName.ReadOnly = true;
			this.ebSearchAdvertiserName.Size = new System.Drawing.Size(176, 21);
			this.ebSearchAdvertiserName.TabIndex = 7;
			this.ebSearchAdvertiserName.TabStop = false;
			this.ebSearchAdvertiserName.Text = "광고주를 선택해주세요";
			this.ebSearchAdvertiserName.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
			this.ebSearchAdvertiserName.Visible = false;
			this.ebSearchAdvertiserName.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
			// 
			// uiPanelClientList
			// 
			this.uiPanelClientList.BackColor = System.Drawing.SystemColors.Window;
			this.uiPanelClientList.CaptionStyle = Janus.Windows.UI.Dock.PanelCaptionStyle.Dark;
			this.uiPanelClientList.InnerContainer = this.uiPanelClientListContainer;
			this.uiPanelClientList.Location = new System.Drawing.Point(0, 69);
			this.uiPanelClientList.MinimumSize = new System.Drawing.Size(-1, 100);
			this.uiPanelClientList.Name = "uiPanelClientList";
			this.uiPanelClientList.Size = new System.Drawing.Size(1010, 377);
			this.uiPanelClientList.TabIndex = 7;
			this.uiPanelClientList.TabStop = false;
			this.uiPanelClientList.Text = "광고주목록";
			// 
			// uiPanelClientListContainer
			// 
			this.uiPanelClientListContainer.BackColor = System.Drawing.SystemColors.Window;
			this.uiPanelClientListContainer.Controls.Add(this.grdExClientList);
			this.uiPanelClientListContainer.Location = new System.Drawing.Point(1, 23);
			this.uiPanelClientListContainer.Name = "uiPanelClientListContainer";
			this.uiPanelClientListContainer.Size = new System.Drawing.Size(1008, 353);
			this.uiPanelClientListContainer.TabIndex = 0;
			// 
			// grdExClientList
			// 
			this.grdExClientList.AllowEdit = Janus.Windows.GridEX.InheritableBoolean.False;
			this.grdExClientList.AlternatingColors = true;
			this.grdExClientList.BorderStyle = Janus.Windows.GridEX.BorderStyle.None;
			this.grdExClientList.DataSource = this.dvClient;
			grdExClientList_DesignTimeLayout.LayoutString = resources.GetString("grdExClientList_DesignTimeLayout.LayoutString");
			this.grdExClientList.DesignTimeLayout = grdExClientList_DesignTimeLayout;
			this.grdExClientList.Dock = System.Windows.Forms.DockStyle.Fill;
			this.grdExClientList.EditorsControlStyle.ButtonAppearance = Janus.Windows.GridEX.ButtonAppearance.Regular;
			this.grdExClientList.EmptyRows = true;
			this.grdExClientList.FocusCellFormatStyle.BackColor = System.Drawing.SystemColors.Highlight;
			this.grdExClientList.FocusCellFormatStyle.ForeColor = System.Drawing.Color.White;
			this.grdExClientList.FocusStyle = Janus.Windows.GridEX.FocusStyle.None;
			this.grdExClientList.FrozenColumns = 2;
			this.grdExClientList.GridLineColor = System.Drawing.Color.Silver;
			this.grdExClientList.GridLines = Janus.Windows.GridEX.GridLines.Vertical;
			this.grdExClientList.GridLineStyle = Janus.Windows.GridEX.GridLineStyle.Solid;
			this.grdExClientList.GroupByBoxVisible = false;
			this.grdExClientList.HeaderFormatStyle.TextAlignment = Janus.Windows.GridEX.TextAlignment.Center;
			this.grdExClientList.HideSelection = Janus.Windows.GridEX.HideSelection.HighlightInactive;
			this.grdExClientList.Location = new System.Drawing.Point(0, 0);
			this.grdExClientList.Name = "grdExClientList";
			this.grdExClientList.ScrollBars = Janus.Windows.GridEX.ScrollBars.Vertical;
			this.grdExClientList.SelectedInactiveFormatStyle.BackColor = System.Drawing.Color.Gold;
			this.grdExClientList.Size = new System.Drawing.Size(1008, 353);
			this.grdExClientList.TabIndex = 9;
			this.grdExClientList.TabKeyBehavior = Janus.Windows.GridEX.TabKeyBehavior.ControlNavigation;
			this.grdExClientList.ThemedAreas = ((Janus.Windows.GridEX.ThemedArea)(((((((((Janus.Windows.GridEX.ThemedArea.ScrollBars | Janus.Windows.GridEX.ThemedArea.EditControls) 
            | Janus.Windows.GridEX.ThemedArea.Headers) 
            | Janus.Windows.GridEX.ThemedArea.GroupByBox) 
            | Janus.Windows.GridEX.ThemedArea.GroupRows) 
            | Janus.Windows.GridEX.ThemedArea.ControlBorder) 
            | Janus.Windows.GridEX.ThemedArea.Cards) 
            | Janus.Windows.GridEX.ThemedArea.Gridlines) 
            | Janus.Windows.GridEX.ThemedArea.CheckBoxes)));
			this.grdExClientList.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
			this.grdExClientList.Enter += new System.EventHandler(this.OnGrdRowChanged);
			// 
			// dvClient
			// 
			this.dvClient.Table = this.clientDs.Clients;
			// 
			// clientDs
			// 
			this.clientDs.DataSetName = "ClientDs";
			this.clientDs.Locale = new System.Globalization.CultureInfo("en-US");
			this.clientDs.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
			// 
			// uiPanelClientDetail
			// 
			this.uiPanelClientDetail.AllowResize = Janus.Windows.UI.InheritableBoolean.False;
			this.uiPanelClientDetail.BackColor = System.Drawing.SystemColors.Window;
			this.uiPanelClientDetail.CaptionStyle = Janus.Windows.UI.Dock.PanelCaptionStyle.Dark;
			this.uiPanelClientDetail.InnerContainer = this.uiPanelClientDetailContainer;
			this.uiPanelClientDetail.Location = new System.Drawing.Point(0, 450);
			this.uiPanelClientDetail.Name = "uiPanelClientDetail";
			this.uiPanelClientDetail.Size = new System.Drawing.Size(1010, 227);
			this.uiPanelClientDetail.TabIndex = 9;
			this.uiPanelClientDetail.TabStop = false;
			this.uiPanelClientDetail.Text = "상세정보";
			// 
			// uiPanelClientDetailContainer
			// 
			this.uiPanelClientDetailContainer.Controls.Add(this.pnlClientDetail);
			this.uiPanelClientDetailContainer.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.uiPanelClientDetailContainer.Location = new System.Drawing.Point(1, 23);
			this.uiPanelClientDetailContainer.Name = "uiPanelClientDetailContainer";
			this.uiPanelClientDetailContainer.Size = new System.Drawing.Size(1008, 203);
			this.uiPanelClientDetailContainer.TabIndex = 0;
			// 
			// pnlClientDetail
			// 
			this.pnlClientDetail.BackColor = System.Drawing.SystemColors.Window;
			this.pnlClientDetail.Controls.Add(this.ebAdvertiserName);
			this.pnlClientDetail.Controls.Add(this.btnAgencySearch);
			this.pnlClientDetail.Controls.Add(this.ebAgency);
			this.pnlClientDetail.Controls.Add(this.ebRap);
			this.pnlClientDetail.Controls.Add(this.ebMedia);
			this.pnlClientDetail.Controls.Add(this.ebRegID);
			this.pnlClientDetail.Controls.Add(this.ebRegDt);
			this.pnlClientDetail.Controls.Add(this.lbRegID);
			this.pnlClientDetail.Controls.Add(this.ebModDt);
			this.pnlClientDetail.Controls.Add(this.ebComment);
			this.pnlClientDetail.Controls.Add(this.label1);
			this.pnlClientDetail.Controls.Add(this.rbUseYn_N);
			this.pnlClientDetail.Controls.Add(this.rbUseYn_Y);
			this.pnlClientDetail.Controls.Add(this.lbUseYn);
			this.pnlClientDetail.Controls.Add(this.lbModDt);
			this.pnlClientDetail.Controls.Add(this.lbRegDt);
			this.pnlClientDetail.Controls.Add(this.lbRapName);
			this.pnlClientDetail.Controls.Add(this.lbMediaName);
			this.pnlClientDetail.Controls.Add(this.lbAgencyName);
			this.pnlClientDetail.Controls.Add(this.lbAdvertiserName);
			this.pnlClientDetail.Controls.Add(this.btnDelete);
			this.pnlClientDetail.Controls.Add(this.btnAdd);
			this.pnlClientDetail.Controls.Add(this.btnSave);
			this.pnlClientDetail.Controls.Add(this.btnAdvertiserSearch);
			this.pnlClientDetail.Dock = System.Windows.Forms.DockStyle.Fill;
			this.pnlClientDetail.Location = new System.Drawing.Point(0, 0);
			this.pnlClientDetail.Name = "pnlClientDetail";
			this.pnlClientDetail.Size = new System.Drawing.Size(1008, 203);
			this.pnlClientDetail.TabIndex = 0;
			// 
			// ebAdvertiserName
			// 
			this.ebAdvertiserName.BackColor = System.Drawing.Color.WhiteSmoke;
			this.ebAdvertiserName.Location = new System.Drawing.Point(648, 8);
			this.ebAdvertiserName.MaxLength = 20;
			this.ebAdvertiserName.Name = "ebAdvertiserName";
			this.ebAdvertiserName.Size = new System.Drawing.Size(192, 20);
			this.ebAdvertiserName.TabIndex = 12;
			this.ebAdvertiserName.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
			this.ebAdvertiserName.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
			// 
			// btnAgencySearch
			// 
			this.btnAgencySearch.Enabled = false;
			this.btnAgencySearch.Location = new System.Drawing.Point(80, 32);
			this.btnAgencySearch.Name = "btnAgencySearch";
			this.btnAgencySearch.Size = new System.Drawing.Size(104, 24);
			this.btnAgencySearch.TabIndex = 20;
			this.btnAgencySearch.Text = "대행사선택";
			this.btnAgencySearch.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
			this.btnAgencySearch.Click += new System.EventHandler(this.btnAgencySearch_Click);
			// 
			// ebAgency
			// 
			this.ebAgency.BackColor = System.Drawing.Color.WhiteSmoke;
			this.ebAgency.Location = new System.Drawing.Point(80, 8);
			this.ebAgency.MaxLength = 100;
			this.ebAgency.Name = "ebAgency";
			this.ebAgency.ReadOnly = true;
			this.ebAgency.Size = new System.Drawing.Size(192, 20);
			this.ebAgency.TabIndex = 10;
			this.ebAgency.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
			this.ebAgency.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
			// 
			// ebRap
			// 
			this.ebRap.BackColor = System.Drawing.Color.WhiteSmoke;
			this.ebRap.Location = new System.Drawing.Point(368, 32);
			this.ebRap.Name = "ebRap";
			this.ebRap.ReadOnly = true;
			this.ebRap.Size = new System.Drawing.Size(192, 20);
			this.ebRap.TabIndex = 13;
			this.ebRap.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
			this.ebRap.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
			// 
			// ebMedia
			// 
			this.ebMedia.BackColor = System.Drawing.Color.WhiteSmoke;
			this.ebMedia.Location = new System.Drawing.Point(368, 8);
			this.ebMedia.Name = "ebMedia";
			this.ebMedia.ReadOnly = true;
			this.ebMedia.Size = new System.Drawing.Size(192, 20);
			this.ebMedia.TabIndex = 11;
			this.ebMedia.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
			this.ebMedia.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
			// 
			// ebRegID
			// 
			this.ebRegID.BackColor = System.Drawing.Color.WhiteSmoke;
			this.ebRegID.Location = new System.Drawing.Point(648, 112);
			this.ebRegID.Name = "ebRegID";
			this.ebRegID.ReadOnly = true;
			this.ebRegID.Size = new System.Drawing.Size(192, 20);
			this.ebRegID.TabIndex = 0;
			this.ebRegID.TabStop = false;
			this.ebRegID.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
			this.ebRegID.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
			// 
			// ebRegDt
			// 
			this.ebRegDt.BackColor = System.Drawing.Color.WhiteSmoke;
			this.ebRegDt.Location = new System.Drawing.Point(648, 64);
			this.ebRegDt.Name = "ebRegDt";
			this.ebRegDt.ReadOnly = true;
			this.ebRegDt.Size = new System.Drawing.Size(192, 20);
			this.ebRegDt.TabIndex = 0;
			this.ebRegDt.TabStop = false;
			this.ebRegDt.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
			this.ebRegDt.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
			// 
			// lbRegID
			// 
			this.lbRegID.Location = new System.Drawing.Point(576, 112);
			this.lbRegID.Name = "lbRegID";
			this.lbRegID.Size = new System.Drawing.Size(72, 21);
			this.lbRegID.TabIndex = 46;
			this.lbRegID.Text = "등록자ID";
			this.lbRegID.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// ebModDt
			// 
			this.ebModDt.BackColor = System.Drawing.Color.WhiteSmoke;
			this.ebModDt.Location = new System.Drawing.Point(648, 88);
			this.ebModDt.Name = "ebModDt";
			this.ebModDt.ReadOnly = true;
			this.ebModDt.Size = new System.Drawing.Size(192, 20);
			this.ebModDt.TabIndex = 0;
			this.ebModDt.TabStop = false;
			this.ebModDt.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
			this.ebModDt.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
			// 
			// ebComment
			// 
			this.ebComment.Location = new System.Drawing.Point(80, 64);
			this.ebComment.Multiline = true;
			this.ebComment.Name = "ebComment";
			this.ebComment.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.ebComment.Size = new System.Drawing.Size(480, 88);
			this.ebComment.TabIndex = 14;
			this.ebComment.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
			this.ebComment.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(8, 64);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(72, 21);
			this.label1.TabIndex = 43;
			this.label1.Text = "비고";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// rbUseYn_N
			// 
			this.rbUseYn_N.BackColor = System.Drawing.Color.Transparent;
			this.rbUseYn_N.Location = new System.Drawing.Point(720, 136);
			this.rbUseYn_N.Name = "rbUseYn_N";
			this.rbUseYn_N.Size = new System.Drawing.Size(72, 23);
			this.rbUseYn_N.TabIndex = 16;
			this.rbUseYn_N.Text = "사용안함";
			this.rbUseYn_N.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
			// 
			// rbUseYn_Y
			// 
			this.rbUseYn_Y.BackColor = System.Drawing.Color.Transparent;
			this.rbUseYn_Y.Location = new System.Drawing.Point(648, 136);
			this.rbUseYn_Y.Name = "rbUseYn_Y";
			this.rbUseYn_Y.Size = new System.Drawing.Size(72, 23);
			this.rbUseYn_Y.TabIndex = 15;
			this.rbUseYn_Y.Text = "사용함";
			this.rbUseYn_Y.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
			// 
			// lbUseYn
			// 
			this.lbUseYn.Location = new System.Drawing.Point(576, 136);
			this.lbUseYn.Name = "lbUseYn";
			this.lbUseYn.Size = new System.Drawing.Size(72, 21);
			this.lbUseYn.TabIndex = 39;
			this.lbUseYn.Text = "사용여부";
			this.lbUseYn.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// lbModDt
			// 
			this.lbModDt.Location = new System.Drawing.Point(576, 88);
			this.lbModDt.Name = "lbModDt";
			this.lbModDt.Size = new System.Drawing.Size(72, 21);
			this.lbModDt.TabIndex = 37;
			this.lbModDt.Text = "수정일시";
			this.lbModDt.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// lbRegDt
			// 
			this.lbRegDt.Location = new System.Drawing.Point(576, 64);
			this.lbRegDt.Name = "lbRegDt";
			this.lbRegDt.Size = new System.Drawing.Size(72, 21);
			this.lbRegDt.TabIndex = 29;
			this.lbRegDt.Text = "등록일시";
			this.lbRegDt.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// lbRapName
			// 
			this.lbRapName.Location = new System.Drawing.Point(296, 32);
			this.lbRapName.Name = "lbRapName";
			this.lbRapName.Size = new System.Drawing.Size(72, 21);
			this.lbRapName.TabIndex = 30;
			this.lbRapName.Text = "미디어렙";
			this.lbRapName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// lbMediaName
			// 
			this.lbMediaName.Location = new System.Drawing.Point(296, 8);
			this.lbMediaName.Name = "lbMediaName";
			this.lbMediaName.Size = new System.Drawing.Size(72, 21);
			this.lbMediaName.TabIndex = 19;
			this.lbMediaName.Text = "매체";
			this.lbMediaName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// lbAgencyName
			// 
			this.lbAgencyName.Location = new System.Drawing.Point(8, 8);
			this.lbAgencyName.Name = "lbAgencyName";
			this.lbAgencyName.Size = new System.Drawing.Size(72, 21);
			this.lbAgencyName.TabIndex = 19;
			this.lbAgencyName.Text = "대행사";
			this.lbAgencyName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// lbAdvertiserName
			// 
			this.lbAdvertiserName.Location = new System.Drawing.Point(584, 8);
			this.lbAdvertiserName.Name = "lbAdvertiserName";
			this.lbAdvertiserName.Size = new System.Drawing.Size(72, 21);
			this.lbAdvertiserName.TabIndex = 30;
			this.lbAdvertiserName.Text = "광고주";
			this.lbAdvertiserName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// btnDelete
			// 
			this.btnDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.btnDelete.Enabled = false;
			this.btnDelete.Location = new System.Drawing.Point(120, 169);
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
			this.btnAdd.Enabled = false;
			this.btnAdd.Location = new System.Drawing.Point(232, 169);
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
			this.btnSave.Enabled = false;
			this.btnSave.Location = new System.Drawing.Point(8, 169);
			this.btnSave.Name = "btnSave";
			this.btnSave.Size = new System.Drawing.Size(104, 24);
			this.btnSave.TabIndex = 17;
			this.btnSave.Text = "저 장";
			this.btnSave.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
			this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
			// 
			// btnAdvertiserSearch
			// 
			this.btnAdvertiserSearch.Enabled = false;
			this.btnAdvertiserSearch.Location = new System.Drawing.Point(648, 32);
			this.btnAdvertiserSearch.Name = "btnAdvertiserSearch";
			this.btnAdvertiserSearch.Size = new System.Drawing.Size(104, 24);
			this.btnAdvertiserSearch.TabIndex = 21;
			this.btnAdvertiserSearch.Text = "광고주선택";
			this.btnAdvertiserSearch.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
			this.btnAdvertiserSearch.Click += new System.EventHandler(this.btnAdvertiserSearch_Click);
			// 
			// uiComboBox1
			// 
			this.uiComboBox1.Location = new System.Drawing.Point(0, 0);
			this.uiComboBox1.Name = "uiComboBox1";
			this.uiComboBox1.Size = new System.Drawing.Size(176, 21);
			this.uiComboBox1.TabIndex = 0;
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
			// ClientControl
			// 
			this.Controls.Add(this.uiPanelClients);
			this.Font = global::AdManagerClient.Properties.Settings.Default.Font1;
			this.Name = "ClientControl";
			this.Size = new System.Drawing.Size(1010, 677);
			this.Load += new System.EventHandler(this.ClientControl_Load);
			((System.ComponentModel.ISupportInitialize)(this.uiPM)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.uiPanelClients)).EndInit();
			this.uiPanelClients.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.uiPanelClientsSearch)).EndInit();
			this.uiPanelClientsSearch.ResumeLayout(false);
			this.uiPanelClientsSearchContainer.ResumeLayout(false);
			this.pnlSearch.ResumeLayout(false);
			this.pnlSearch.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.uiPanelClientList)).EndInit();
			this.uiPanelClientList.ResumeLayout(false);
			this.uiPanelClientListContainer.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.grdExClientList)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.dvClient)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.clientDs)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.uiPanelClientDetail)).EndInit();
			this.uiPanelClientDetail.ResumeLayout(false);
			this.uiPanelClientDetailContainer.ResumeLayout(false);
			this.pnlClientDetail.ResumeLayout(false);
			this.pnlClientDetail.PerformLayout();
			this.ResumeLayout(false);

		}
		#endregion

		#region 컨트롤 로드
		private void ClientControl_Load(object sender, System.EventArgs e)
		{
			// 데이터관리용 객체생성
			dt = ((DataView)grdExClientList.DataSource).Table;  
			cm = (CurrencyManager) this.BindingContext[grdExClientList.DataSource]; 
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
			InitCombo_Rap();	
			InitCombo_Agency();
			InitCombo_Advertiser();
			InitCombo_Level();	
           

			// 조회권한 검사
			if(menu.CanRead(MenuCode))
			{
				canRead = true;
				SearchClient();
				
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
            ProgressStop();
		}

		private void InitCombo()
		{
			// 코드에서 보안레벨을 조회한다.
			MediaCodeModel mediacodeModel = new MediaCodeModel();
			//MediacodeModel.Section = "21";				// 코드분류 '11':보안레벨  TODO: 코드분류는 추후 XML로 관리되어야...
			new MediaCodeManager(systemModel, commonModel).GetMediaCodeList(mediacodeModel);
			
			if (mediacodeModel.ResultCD.Equals("0000"))
			{
				// 데이터셋에 셋팅
				Utility.SetDataTable(clientDs.Medias, mediacodeModel.MediaCodeDataSet);				
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
				DataRow row = clientDs.Medias.Rows[i];

				string val = row["MediaCode"].ToString();
				string txt = row["MediaName"].ToString();
				comboItems[i+1] = new Janus.Windows.EditControls.UIComboBoxItem(txt,val);
			}
			// 콤보에 셋트
			this.cbSearchMediaName.Items.AddRange(comboItems);
			this.cbSearchMediaName.SelectedIndex = 0;			
			//MessageBox.Show(cbSearchMediaName.SelectedValue.ToString());
			Application.DoEvents();
		}

		private void InitCombo_Rap()
		{
            /* 랩사 모바일로 고정
			// 코드에서 보안레벨을 조회한다.
			MediaRapCodeModel mediarapcodeModel = new MediaRapCodeModel();
			//codeModel.Section = "21";				// 코드분류 '11':보안레벨  TODO: 코드분류는 추후 XML로 관리되어야...
			new MediaRapCodeManager(systemModel, commonModel).GetMediaRapCodeList(mediarapcodeModel);
			
			if (mediarapcodeModel.ResultCD.Equals("0000"))
			{
				// 데이터셋에 셋팅
				Utility.SetDataTable(clientDs.MediaRaps, mediarapcodeModel.MediaRapCodeDataSet);				
			}

			// 상세조회 콤보
			// 상세정보의 콤보는 Dataset을 데이터소스로 가진다.

			// 검색조건의 콤보
			this.cbSearchRapName.Items.Clear();
			
			// 콤보박스에 셋트할 코드목록을 담을 Item배열을 선언
			Janus.Windows.EditControls.UIComboBoxItem[] comboItems = new Janus.Windows.EditControls.UIComboBoxItem[mediarapcodeModel.ResultCnt + 1];

			comboItems[0] = new Janus.Windows.EditControls.UIComboBoxItem("미디어렙선택","00");
			
			for(int i=0;i<mediarapcodeModel.ResultCnt;i++)
			{
				DataRow row = clientDs.MediaRaps.Rows[i];

				string val = row["RapCode"].ToString();
				string txt = row["RapName"].ToString();
				comboItems[i+1] = new Janus.Windows.EditControls.UIComboBoxItem(txt,val);
			}
			// 콤보에 셋트
			this.cbSearchRapName.Items.AddRange(comboItems);
			this.cbSearchRapName.SelectedIndex = 0;
			//MessageBox.Show(cbSearchRapName.SelectedValue.ToString());
			Application.DoEvents();
            */
            this.cbSearchRapName.Items.Clear();
            DataSet ds = new DataSet("rapset");
            DataTable dt = new DataTable("rap");
            dt.Columns.Add("RapCode", typeof(string));
            dt.Columns.Add("RapName", typeof(string));
            DataRow nRow = dt.NewRow();
            nRow["RapCode"] = "1";
            nRow["RapName"] = "모바일 편성팀";
            dt.Rows.Add(nRow);
            ds.Tables.Add(dt);

            Utility.SetDataTable(clientDs.MediaRaps, ds);
            // 검색조건의 콤보
            this.cbSearchRapName.Items.Clear();
            // 콤보박스에 셋트할 코드목록을 담을 Item배열을 선언
            Janus.Windows.EditControls.UIComboBoxItem[] comboItems = new Janus.Windows.EditControls.UIComboBoxItem[2];

            comboItems[0] = new Janus.Windows.EditControls.UIComboBoxItem("미디어렙선택", "00");

            for (int i = 0; i < 1; i++)
            {
                DataRow row = clientDs.MediaRaps.Rows[i];

                string val = row["RapCode"].ToString();
                string txt = row["RapName"].ToString();
                comboItems[i + 1] = new Janus.Windows.EditControls.UIComboBoxItem(txt, val);
            }
            // 콤보에 셋트
            this.cbSearchRapName.Items.AddRange(comboItems);
            this.cbSearchRapName.SelectedIndex = 1;
            this.cbSearchRapName.ReadOnly = true;
            Application.DoEvents();

		}

		private void InitCombo_Agency()
		{
			// 코드에서 보안레벨을 조회한다.
			AgencyCodeModel agencycodeModel = new AgencyCodeModel();
			//codeModel.Section = "21";				// 코드분류 '11':보안레벨  TODO: 코드분류는 추후 XML로 관리되어야...
			new AgencyCodeManager(systemModel, commonModel).GetAgencyCodeList(agencycodeModel);
			
			if (agencycodeModel.ResultCD.Equals("0000"))
			{
				// 데이터셋에 셋팅
				Utility.SetDataTable(clientDs.Agencys, agencycodeModel.AgencyCodeDataSet);				
			}

			// 상세조회 콤보
			// 상세정보의 콤보는 Dataset을 데이터소스로 가진다.

			// 검색조건의 콤보
			this.cbSearchMediaAgency.Items.Clear();
			
			// 콤보박스에 셋트할 코드목록을 담을 Item배열을 선언
			Janus.Windows.EditControls.UIComboBoxItem[] comboItems = new Janus.Windows.EditControls.UIComboBoxItem[agencycodeModel.ResultCnt + 1];

			comboItems[0] = new Janus.Windows.EditControls.UIComboBoxItem("대행사선택","00");
			
			for(int i=0;i<agencycodeModel.ResultCnt;i++)
			{
				DataRow row = clientDs.Agencys.Rows[i];

				string val = row["AgencyCode"].ToString();
				string txt = row["AgencyName"].ToString();
				comboItems[i+1] = new Janus.Windows.EditControls.UIComboBoxItem(txt,val);
			}
			
			// 콤보에 셋트
			this.cbSearchMediaAgency.Items.AddRange(comboItems);
			this.cbSearchMediaAgency.SelectedIndex = 0;
			Application.DoEvents();
		}

		private void InitCombo_Level()
		{
			
            if(commonModel.UserLevel=="20")
            {
                cbSearchMediaName.SelectedValue = commonModel.MediaCode;			
                cbSearchMediaName.ReadOnly = true;					
            }
            else
            {
				for(int i=0;i < clientDs.Medias.Rows.Count;i++)
				{
					DataRow row = clientDs.Medias.Rows[i];					
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
			if(commonModel.UserLevel=="30")
			{
				cbSearchRapName.SelectedValue = commonModel.RapCode;			
				cbSearchRapName.ReadOnly = true;				
			}
			if(commonModel.UserLevel=="40")
			{
				cbSearchMediaAgency.SelectedValue = commonModel.AgencyCode;			
				cbSearchMediaAgency.ReadOnly = true;					
			}	
			Application.DoEvents();		
		}

		private void InitCombo_Advertiser()
		{			
//			ClientModel clientModel = new ClientModel();			
//			new ClientManager(systemModel, commonModel).GetAdvertiserList(clientModel);
//			
//			if (clientModel.ResultCD.Equals("0000"))
//			{
//				// 데이터셋에 셋팅
//				Utility.SetDataTable(clientDs.Advertisers, clientModel.ClientDataSet);				
//			}		
//			// 검색조건의 콤보
//			this.ebSearchAdvertiserName.Items.Clear();
//			
//			// 콤보박스에 셋트할 코드목록을 담을 Item배열을 선언
//			Janus.Windows.EditControls.UIComboBoxItem[] comboItems = new Janus.Windows.EditControls.UIComboBoxItem[clientModel.ResultCnt + 1];
//
//			comboItems[0] = new Janus.Windows.EditControls.UIComboBoxItem("광고주선택","00");
//			
//			for(int i=0;i<clientModel.ResultCnt;i++)
//			{
//				DataRow row = clientDs.Advertisers.Rows[i];
//
//				string val = row["AdvertiserCode"].ToString();
//				string txt = row["AdvertiserName"].ToString();
//				comboItems[i+1] = new Janus.Windows.EditControls.UIComboBoxItem(txt,val);
//			}
//			
//			// 콤보에 셋트
//			this.ebSearchAdvertiserName.Items.AddRange(comboItems);
//			this.ebSearchAdvertiserName.SelectedIndex = 0;
//			Application.DoEvents();
		}

		private void InitButton()
		{
			if(canRead)   btnSearch.Enabled = true;
			if(canRead)   btnAdvertiserSearch_1.Enabled = true;
			if(canCreate) btnAdd.Enabled    = true;
			
			if(!advertiserCode.Equals(""))
			{
				if(canDelete) btnDelete.Enabled = true;
				if(canUpdate) btnSave.Enabled   = true;
			}
			Application.DoEvents();
		}

		private void DisableButton()
		{
			btnSearch.Enabled = false;
			btnAdvertiserSearch_1.Enabled = false;
			btnAdd.Enabled    = false;
			btnSave.Enabled   = false;
			btnDelete.Enabled = false;
			Application.DoEvents();
		}

		#endregion

		#region 매체대행광고주 액션처리 메소드

		/// <summary>
		/// 그리드의 Row변경시
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void OnGrdRowChanged(object sender, System.EventArgs e) 
		{
            if (!IsSearching) // 2011.11.29 JH.Park 조회중이 아닐경우에만 동작하도록 변경
            {
                if (grdExClientList.RecordCount > 0)
                {
                    SetClientDetailText();
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
			if(cbSearchMediaName.SelectedValue.ToString() == "00") 
			{
				FrameSystem.showMsgForm("Client정보검색 오류",new string[] {"", "매체를 선택하여 주세요.", "" });
				return;
			}
	
            ProgressStart();
			
            ReSetGridData();
			DisableButton();			
			SearchClient();			
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
			ResetClientDetailText();

			ebComment.Focus();
		}

		/// <summary>
		/// 저장버튼 클릭
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnSave_Click(object sender, System.EventArgs e)
		{			
			SaveClientDetail();
		}

		/// <summary>
		/// 삭제버튼 클릭
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnDelete_Click(object sender, System.EventArgs e)
		{
			DeleteClient();
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
				SearchClient();
			}
		}

		private void ReSetGridData()
		{
			clientDs.Clients.Clear();        
		}

		//이 메소드가 실행이 되면 해당 필드의 팝업이 호출된다.
		private void ResetPop(object flag)
		{
			if(flag.Equals("client"))
			{
				Client_pForm pForm = new Client_pForm(this);
				pForm.ShowDialog();            
				pForm.Dispose();
				pForm = null;		
			}
			if(flag.Equals("advertiser"))
			{
				Advertiser_pForm advertiserForm = new Advertiser_pForm(this);
				advertiserForm.ShowDialog();            
				advertiserForm.Dispose();
				advertiserForm = null;		
			}
		}

		private void btnAgencySearch_Click(object sender, System.EventArgs e)
		{
			// 광고계약 목록 검색 팝업 띠우기
			Client_pForm pForm = new Client_pForm(this);

			pForm.ShowDialog();
            
			pForm.Dispose();
			pForm = null;		
		}

		#endregion

		#region 처리메소드

		/// <summary>
		/// 매체대행광고주목록 조회
		/// </summary>
		private void SearchClient()
		{
            IsSearching = true;

			StatusMessage("매체대행광고주 정보를 조회합니다.");
			
			try
			{
				clientModel.Init();
				// 데이터모델에 전송할 내용을 셋트한다.
				if(IsNewSearchKey)
				{
					clientModel.SearchKey = "";
				}
				else
				{
					clientModel.SearchKey  = ebSearchKey.Text;
				}				
				
				clientModel.SearchMediaName = cbSearchMediaName.SelectedItem.Value.ToString();
				clientModel.SearchRapName = cbSearchRapName.SelectedItem.Value.ToString();
				clientModel.SearchMediaAgency = cbSearchMediaAgency.SelectedItem.Value.ToString();
				clientModel.SearchAdvertiserName = Search_advertiserCode;

                if (uiCheckBox1.Checked)
				{
					clientModel.SearchchkAdState_10   = "Y";
				}
				else
				{
					clientModel.SearchchkAdState_10   = "N";
				}
				
				LastOrder = 0;


				// 매체대행광고주목록조회 서비스를 호출한다.
				new ClientManager(systemModel,commonModel).GetClientList(clientModel);
//				Search_advertiserCode = "";
//				ebSearchAdvertiserName.Text = "";

				if (clientModel.ResultCD.Equals("0000"))
				{					
					Utility.SetDataTable(clientDs.Clients, clientModel.ClientDataSet);				
					StatusMessage(clientModel.ResultCnt + "건의 매체대행광고주 정보가 조회되었습니다.");
					LastOrder = clientModel.ResultCnt;
					
					if(canUpdate)
					{
						AddSchChoice();
					}
					SetClientDetailText();
				}
			}
			catch(FrameException fe)
			{
				FrameSystem.showMsgForm("매체대행광고주조회오류", new string[] {fe.ErrCode, fe.ResultMsg});
			}
			catch(Exception ex)
			{
				FrameSystem.showMsgForm("매체대행광고주조회오류",new string[] {"",ex.Message});
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
				if ( clientDs.Tables["Clients"].Rows.Count < 1 ) return;
              
				foreach (DataRow row in clientDs.Tables["Clients"].Rows)
				{					
					if(IsAdding)
					{
						cm.Position = 0;
						mediaCode = "";
						rapCode = "";
						agencyCode = "";
						advertiserCode = "";
					}
					else
					{						
						if(row["MediaCode"].ToString().Equals(mediaCode) && row["RapCode"].ToString().Equals(rapCode) && row["AgencyCode"].ToString().Equals(agencyCode) && row["AdvertiserCode"].ToString().Equals(advertiserCode))
						{					
							cm.Position = rowIndex;
							break;								
						}
					}

					rowIndex++;
					grdExClientList.EnsureVisible();
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

		private void SearchClient_Combo()
		{
//			clientModel.MediaCode_C = cbMediaName.SelectedItem.Value.ToString();
//			clientModel.RapCode_C = cbRapName.SelectedItem.Value.ToString();
//			clientModel.AgencyCode_C = cbAgencyName.SelectedItem.Value.ToString();
//			clientModel.AdvertiserCode_C = ebAdvertiserName.SelectedItem.Value.ToString();
		}

		/// <summary>
		/// 매체대행광고주상세정보 저장
		/// </summary>
		private void SaveClientDetail()
		{
			StatusMessage("매체대행광고주 정보를 저장합니다.");
			
			if(agencyCode.Length == 0) 
			{
				MessageBox.Show("대행사가 입력되지 않았습니다.","매체별 광고주 저장", 
					MessageBoxButtons.OK, MessageBoxIcon.Information );
					ResetPop("client");
				return;				
			}
			if(mediaCode.Length == 0) 
			{
				MessageBox.Show("매체가 입력되지 않았습니다.","매체별 광고주 저장", 
					MessageBoxButtons.OK, MessageBoxIcon.Information );
					ResetPop("client");
				return;				
			}
			if(rapCode.Length == 0) 
			{
				MessageBox.Show("미디어렙이 입력되지 않았습니다.","매체별 광고주 저장", 
					MessageBoxButtons.OK, MessageBoxIcon.Information );
					ResetPop("client");
				return;				
			}
			if(advertiserCode.Length == 0) 
			{
				MessageBox.Show("광고주가 입력되지 않았습니다.","매체별 광고주 저장", 
					MessageBoxButtons.OK, MessageBoxIcon.Information );
					ResetPop("advertiser");
				return;				
			}		

			try
			{
				// 데이터모델에 전송할 내용을 셋트한다.
				clientModel.MediaCode_C       = mediaCode;
				clientModel.RapCode_C       = rapCode;
				clientModel.AgencyCode_C       = agencyCode;
				clientModel.AdvertiserCode_C       = advertiserCode;
				clientModel.Comment       = ebComment.Text.Trim();								

				//사용여부
				if(rbUseYn_Y.Checked)
				{
					clientModel.UseYn       = "Y";
				}
				else
				{
					clientModel.UseYn       = "N";
				}
				
				// 매체대행광고주 상세정보 저장 서비스를 호출한다.
				if (IsAdding)
				{
					new ClientManager(systemModel,commonModel).SetClientAdd(clientModel);
                    StatusMessage("매체대행광고주 정보가 추가되었습니다.");
                    IsAdding = false;
                    ResetClientDetailText();
                }
				else
				{
					new ClientManager(systemModel,commonModel).SetClientUpdate(clientModel);
                    StatusMessage("매체대행광고주 정보가 저장되었습니다.");
                }

                DisableButton();
                SearchClient();
                InitButton();			
			}
			catch(FrameException fe)
			{
				FrameSystem.showMsgForm("매체대행광고주정보 저장오류", new string[] {fe.ErrCode, fe.ResultMsg});
			}
			catch(Exception ex)
			{
				FrameSystem.showMsgForm("매체대행광고주정보 저장오류",new string[] {"",ex.Message});
			}			
		}

		/// <summary>
		/// 매체대행광고주정보 삭제
		/// </summary>
		private void DeleteClient()
		{
			StatusMessage("매체대행광고주 정보를 삭제합니다.");

			DialogResult result = MessageBox.Show("해당 매체대행광고주 정보를 삭제 하시겠습니까?","매체대행광고주 삭제",
				MessageBoxButtons.YesNo, MessageBoxIcon.Question,
				MessageBoxDefaultButton.Button2);

			if (result == DialogResult.No) return;

			try 
			{
				// 데이터모델에 전송할 내용을 셋트한다.
				clientModel.MediaCode       = mediaCode;
				clientModel.RapCode       = rapCode;
				clientModel.AgencyCode       = agencyCode;
				clientModel.AdvertiserCode       = advertiserCode;

				// 매체대행광고주 상세정보 저장 서비스를 호출한다.
				new ClientManager(systemModel,commonModel).SetClientDelete(clientModel);
                StatusMessage("매체대행광고주 정보가 삭제되었습니다.");			
				ResetClientDetailText();
				
                DisableButton();
                SearchClient();
				InitButton();

			}
			catch(FrameException fe)
			{
				FrameSystem.showMsgForm("매체대행광고주정보 삭제오류", new string[] {fe.ErrCode, fe.ResultMsg});
			}
			catch(Exception ex)
			{
				FrameSystem.showMsgForm("매체대행광고주정보 삭제오류",new string[] {"",ex.Message});
			}			

		}
		
		/// <summary>
		/// 매체대행광고주 상세정보의 셋트
		/// </summary>
		private void SetClientDetailText()
		{
			int curRow = cm.Position;
			if(curRow >= 0)
			{
				mediaCode	    = dt.Rows[curRow]["MediaCode"].ToString();			
				rapCode	        = dt.Rows[curRow]["RapCode"].ToString();			
				agencyCode	    = dt.Rows[curRow]["AgencyCode"].ToString();			
				advertiserCode	= dt.Rows[curRow]["AdvertiserCode"].ToString();	
		
				ebMedia.Text    = dt.Rows[curRow]["MediaName"].ToString();			
				ebRap.Text      = dt.Rows[curRow]["RapName"].ToString();			
				ebAgency.Text   = dt.Rows[curRow]["AgencyName"].ToString();						
				ebAdvertiserName.Text      = dt.Rows[curRow]["AdvertiserName"].ToString();		
				
				ebComment.Text  = dt.Rows[curRow]["Comment"].ToString();						
				ebRegDt.Text    = dt.Rows[curRow]["RegDt"].ToString();
				ebModDt.Text    = dt.Rows[curRow]["ModDt"].ToString();
				string UseYn              = dt.Rows[curRow]["UseYn"].ToString();

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

				IsAdding = false;
				ResetTextReadonly();
			}

			StatusMessage("준비");
		}

		private void ResetClientDetailText()
		{
			mediaCode      = "";
			rapCode        = "";
			agencyCode     = "";
			advertiserCode     = "";
			ebMedia.Text   = "";			
			ebRap.Text     = "";			
			ebAgency.Text  = "";		
			ebAdvertiserName.Text  = "";			
			ebComment.Text = "";			
			rbUseYn_Y.Checked         = true;
			rbUseYn_N.Checked         = false;
				
			ebRegDt.Text   = "";
			ebModDt.Text   = "";
			ebRegID.Text   = "";
			
			if(!IsAdding)
			{

				btnAgencySearch.Enabled     = false;

				ebMedia.ReadOnly   = false;	
				ebRap.ReadOnly   = false;	
				ebAgency.ReadOnly   = false;	
				ebAdvertiserName.ReadOnly   = false;	
				ebComment.ReadOnly          = false;							
				
				rbUseYn_Y.Enabled			= true;
				rbUseYn_N.Enabled			= true;
						
				ebMedia.BackColor  = Color.White;
				ebRap.BackColor  = Color.White;
				ebAgency.BackColor  = Color.White;
				ebAdvertiserName.BackColor  = Color.White;
				ebComment.BackColor         = Color.White;			
			
			}
		}
		
		/// <summary>
		/// 상세정보 ReadOnly
		/// </summary>
		private void SetTextReadonly()
		{

			btnAgencySearch.Enabled    = false;
			btnAdvertiserSearch.Enabled    = false;

			ebMedia.ReadOnly   = true;	
			ebRap.ReadOnly   = true;	
			ebAgency.ReadOnly   = true;
			ebAdvertiserName.ReadOnly  = true;	
			ebComment.ReadOnly         = true;									
			
			rbUseYn_Y.Enabled          = false;
			rbUseYn_N.Enabled          = false;
						
			ebMedia.BackColor  = Color.WhiteSmoke;
			ebRap.BackColor  = Color.WhiteSmoke;
			ebAgency.BackColor  = Color.WhiteSmoke;
			ebAdvertiserName.BackColor = Color.WhiteSmoke;	
			ebComment.BackColor        = Color.WhiteSmoke;			
				
		}

		/// <summary>
		/// 상세정보 수정가능케
		/// </summary>
		private void ResetTextReadonly()
		{			

			ebComment.ReadOnly          = false;																				
			ebComment.BackColor        = Color.White;			

			rbUseYn_Y.Enabled         = true;
			rbUseYn_N.Enabled         = true;

			// 신규작성이면 아이디까지 쓰기가능
			if (IsAdding)
			{
				btnAgencySearch.Enabled    = true;
				btnAdvertiserSearch.Enabled    = true;

				ebMedia.ReadOnly   = true;	
				ebRap.ReadOnly   = true;	
				ebAgency.ReadOnly   = true;
				ebAdvertiserName.ReadOnly  = true;	

				ebMedia.BackColor  = Color.White;
				ebRap.BackColor  = Color.White;
				ebAgency.BackColor  = Color.White;
				ebAdvertiserName.BackColor  = Color.White;
				ebComment.BackColor         = Color.White;		
			}
			else
			{
				btnAgencySearch.Enabled     = false;
				btnAdvertiserSearch.Enabled    = false;

				ebMedia.ReadOnly   = false;	
				ebRap.ReadOnly   = false;	
				ebAgency.ReadOnly   = false;	
				ebAdvertiserName.ReadOnly   = false;	

				ebMedia.BackColor  = Color.WhiteSmoke;
				ebRap.BackColor  = Color.WhiteSmoke;
				ebAgency.BackColor  = Color.WhiteSmoke;
				ebAdvertiserName.BackColor = Color.WhiteSmoke;	
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

		#region 외부노출 메소드
		/// <summary>
		/// 선택된 Row들을 입력시킴
		/// </summary>
		/// <param name="dtc"></param>
		public void adOn_AddContract(ClientModel clientModel )
		{
			mediaCode       = clientModel.MediaCode;
			rapCode         = clientModel.RapCode;
			agencyCode      = clientModel.AgencyCode;

			ebMedia.Text        = clientModel.MediaName;
			ebRap.Text          = clientModel.RapName;
			ebAgency.Text       = clientModel.AgencyName;
		}

		private void ResetAdvertiser(object flag)
		{
			//광고주선택 입력 팝업
			if(flag.Equals("Advertiser"))
			{
				// 광고주선택 검색 팝업 띠우기
				Advertiser_pForm advertiserForm = new Advertiser_pForm(this, "Advertiser");
				advertiserForm.ShowDialog();            
				advertiserForm.Dispose();
				advertiserForm = null;		
			}
			//광고주선택 검색 팝업
			if(flag.Equals("SearchAdvertiser"))
			{
				Advertiser_pForm Search_advertiserForm = new Advertiser_pForm(this, "SearchAdvertiser");
				Search_advertiserForm.ShowDialog();            
				Search_advertiserForm.Dispose();
				Search_advertiserForm = null;		
			}
		}

		private void btnAdvertiserSearch_Click(object sender, System.EventArgs e)
		{
			// 광고주선택 입력 팝업 띄우기
			ResetAdvertiser("Advertiser");
		}		
		
		public string AdvertiserCode
		{
			set
			{
				this.advertiserCode = value;
			}
		}

		public string AdvertiserName
		{				
			set
			{
				this.ebAdvertiserName.Text = value;
			}			
		}

		private void btnAdvertiserSearch_1_Click(object sender, System.EventArgs e)
		{
			// 광고주선택 검색 팝업 띄우기
			ResetAdvertiser("SearchAdvertiser");
		}

		public string SearchAdvertiserCode
		{
			set
			{
				this.Search_advertiserCode = value;
			}
		}

		public string SearchAdvertiserName
		{				
			set
			{
				this.ebSearchAdvertiserName.Text = value;
			}			
		}


		#endregion	
		
	}
}

