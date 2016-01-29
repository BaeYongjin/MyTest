// ===============================================================================
// MediaAgencyControl for Charites Project
//
// MediaAgencyControl.cs
//
// 매체대행사정보관리 컨드롤을 정의합니다. 
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
 * Class Name: MediaAgencyControl
 * 주요기능  : 매체대행사정보관리 컨트롤
 * 작성자    : 모름
 * 작성일    : 모름
 * -------------------------------------------------------
 * [수정사항]
 * -------------------------------------------------------
 * 수정코드  : [E_01]
 * 수정자    : 김보배
 * 수정일    : 2013.02
 * 수정내용  :        
 *            - 미디어랩 컨트롤 Disable로 검색 안함
 * 수정함수  :
 *            - GetAgencyCodeList
 *            - 미디어랩 컨트롤 Disable
 * --------------------------------------------------------
 */

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
	/// 매체대행사관리 컨트롤
	/// </summary>
    public class MediaAgencyControl : System.Windows.Forms.UserControl, IUserControl
	{
		#region 이벤트핸들러
		public event StatusEventHandler 			StatusEvent;			// 상태이벤트 핸들러
        public event ProgressEventHandler 			ProgressEvent;			// 처리중이벤트 핸들러
		#endregion
			
		#region 매체대행사정의 객체 및 변수

		// 시스템 정보 : 화면공통
		private SystemModel   systemModel   = FrameSystem.oSysModel;
		private CommonModel   commonModel   = FrameSystem.oComModel;
		private Logger        log           = FrameSystem.oLog;
		private MenuPower     menu          = FrameSystem.oMenu;

		// 메뉴코드 : 보안이 필요한 화면에 필요함
		public string        menuCode		= "";

		// 사용할 정보모델
		MediaAgencyModel mediaAgencyModel  = new MediaAgencyModel();	// 매체대행사정보모델

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

		private string        mediaCode = null;
		private string        rapCode = null;
        private Janus.Windows.EditControls.UICheckBox uiCheckBox1;
		private string        agencyCode = null;

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
		private Janus.Windows.UI.Dock.UIPanel uiPanelMediaAgencyList;
		private Janus.Windows.UI.Dock.UIPanelInnerContainer uiPanelMediaAgencyListContainer;
		private Janus.Windows.UI.Dock.UIPanel uiPanelMediaAgencyDetail;
		private Janus.Windows.UI.Dock.UIPanelInnerContainer uiPanelMediaAgencyDetailContainer;
		private Janus.Windows.UI.Dock.UIPanel uiPanelMediaAgencysSearch;
		private Janus.Windows.UI.Dock.UIPanelInnerContainer uiPanelMediaAgencysSearchContainer;
		private Janus.Windows.EditControls.UIButton uiButton2;
		private Janus.Windows.EditControls.UIButton uiButton1;
		private Janus.Windows.UI.Dock.UIPanelGroup uiPanelMediaAgencys;
		private System.Windows.Forms.Panel pnlSearch;
		private Janus.Windows.GridEX.EditControls.EditBox ebSearchKey;
		private Janus.Windows.EditControls.UIButton btnSearch;
		private System.Windows.Forms.Panel pnlMediaAgencyDetail;
		private System.Windows.Forms.Label lbRegDt;
		private Janus.Windows.EditControls.UIButton btnAdd;
		private Janus.Windows.EditControls.UIButton btnSave;
		private System.Data.DataView dvMediaAgency;
		private Janus.Windows.GridEX.EditControls.EditBox ebRegDt;
		private Janus.Windows.EditControls.UIButton btnDelete;		
		private System.Windows.Forms.Label lbModDt;
		private Janus.Windows.GridEX.EditControls.EditBox ebModDt;
		private Janus.Windows.GridEX.GridEX grdExMediaAgencyList;		
		private Janus.Windows.GridEX.EditControls.EditBox ebEmail;
		private System.Windows.Forms.Label lbTell;
		private System.Windows.Forms.Label lbUseYn;
		private System.Windows.Forms.Label lbEmail;
		private Janus.Windows.GridEX.EditControls.EditBox ebCharger;
		private System.Windows.Forms.Label lbCharger;
		private Janus.Windows.GridEX.EditControls.EditBox ebTell;
		private System.Windows.Forms.Label lbContStarDay;
		private System.Windows.Forms.Label lbContEndDay;
		private Janus.Windows.EditControls.UIRadioButton rbUseYn_Y;
		private Janus.Windows.EditControls.UIRadioButton rbUseYn_N;
		private Janus.Windows.EditControls.UIComboBox cbSearchMediaName;
		private Janus.Windows.EditControls.UIComboBox cbSearchMediaAgency;
		private Janus.Windows.EditControls.UIComboBox cbSearchRapName;
		private AdManagerClient.MediaAgencyDs mediaAgencyDs;
		private System.Windows.Forms.Label lbRapName;
		private System.Windows.Forms.Label lbMediaName;
		private System.Windows.Forms.Label lbAgencyName;
		private Janus.Windows.EditControls.UIComboBox uiComboBox1;
		private System.Windows.Forms.Label lbRegID;
		private Janus.Windows.GridEX.EditControls.EditBox ebRegID;
		private Janus.Windows.GridEX.EditControls.EditBox ebMediaName;
		private Janus.Windows.GridEX.EditControls.EditBox ebRapName;
		private Janus.Windows.GridEX.EditControls.EditBox ebAgencyName;
		private Janus.Windows.EditControls.UIButton btnMediaSearch;
		private Janus.Windows.EditControls.UIButton btnAgencySearch;
		private Janus.Windows.EditControls.UIButton btnRapSearch;
		private Janus.Windows.CalendarCombo.CalendarCombo ebContStartDay;
        private Janus.Windows.CalendarCombo.CalendarCombo ebContEndDay;
		private System.ComponentModel.IContainer components;

		public MediaAgencyControl()
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
			Janus.Windows.GridEX.GridEXLayout grdExMediaAgencyList_DesignTimeLayout = new Janus.Windows.GridEX.GridEXLayout();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MediaAgencyControl));
			this.uiPM = new Janus.Windows.UI.Dock.UIPanelManager(this.components);
			this.uiPanelMediaAgencys = new Janus.Windows.UI.Dock.UIPanelGroup();
			this.uiPanelMediaAgencysSearch = new Janus.Windows.UI.Dock.UIPanel();
			this.uiPanelMediaAgencysSearchContainer = new Janus.Windows.UI.Dock.UIPanelInnerContainer();
			this.pnlSearch = new System.Windows.Forms.Panel();
			this.uiCheckBox1 = new Janus.Windows.EditControls.UICheckBox();
			this.ebSearchKey = new Janus.Windows.GridEX.EditControls.EditBox();
			this.cbSearchMediaName = new Janus.Windows.EditControls.UIComboBox();
			this.cbSearchRapName = new Janus.Windows.EditControls.UIComboBox();
			this.cbSearchMediaAgency = new Janus.Windows.EditControls.UIComboBox();
			this.btnSearch = new Janus.Windows.EditControls.UIButton();
			this.uiPanelMediaAgencyList = new Janus.Windows.UI.Dock.UIPanel();
			this.uiPanelMediaAgencyListContainer = new Janus.Windows.UI.Dock.UIPanelInnerContainer();
			this.grdExMediaAgencyList = new Janus.Windows.GridEX.GridEX();
			this.dvMediaAgency = new System.Data.DataView();
			this.mediaAgencyDs = new AdManagerClient.MediaAgencyDs();
			this.uiPanelMediaAgencyDetail = new Janus.Windows.UI.Dock.UIPanel();
			this.uiPanelMediaAgencyDetailContainer = new Janus.Windows.UI.Dock.UIPanelInnerContainer();
			this.pnlMediaAgencyDetail = new System.Windows.Forms.Panel();
			this.ebContEndDay = new Janus.Windows.CalendarCombo.CalendarCombo();
			this.btnMediaSearch = new Janus.Windows.EditControls.UIButton();
			this.ebAgencyName = new Janus.Windows.GridEX.EditControls.EditBox();
			this.ebRapName = new Janus.Windows.GridEX.EditControls.EditBox();
			this.ebMediaName = new Janus.Windows.GridEX.EditControls.EditBox();
			this.rbUseYn_N = new Janus.Windows.EditControls.UIRadioButton();
			this.rbUseYn_Y = new Janus.Windows.EditControls.UIRadioButton();
			this.lbUseYn = new System.Windows.Forms.Label();
			this.lbEmail = new System.Windows.Forms.Label();
			this.lbModDt = new System.Windows.Forms.Label();
			this.lbTell = new System.Windows.Forms.Label();
			this.ebTell = new Janus.Windows.GridEX.EditControls.EditBox();
			this.ebCharger = new Janus.Windows.GridEX.EditControls.EditBox();
			this.lbCharger = new System.Windows.Forms.Label();
			this.lbRegDt = new System.Windows.Forms.Label();
			this.ebRegDt = new Janus.Windows.GridEX.EditControls.EditBox();
			this.ebModDt = new Janus.Windows.GridEX.EditControls.EditBox();
			this.ebEmail = new Janus.Windows.GridEX.EditControls.EditBox();
			this.lbContStarDay = new System.Windows.Forms.Label();
			this.lbContEndDay = new System.Windows.Forms.Label();
			this.lbRapName = new System.Windows.Forms.Label();
			this.lbMediaName = new System.Windows.Forms.Label();
			this.lbAgencyName = new System.Windows.Forms.Label();
			this.lbRegID = new System.Windows.Forms.Label();
			this.ebRegID = new Janus.Windows.GridEX.EditControls.EditBox();
			this.btnAdd = new Janus.Windows.EditControls.UIButton();
			this.btnDelete = new Janus.Windows.EditControls.UIButton();
			this.btnSave = new Janus.Windows.EditControls.UIButton();
			this.btnRapSearch = new Janus.Windows.EditControls.UIButton();
			this.btnAgencySearch = new Janus.Windows.EditControls.UIButton();
			this.ebContStartDay = new Janus.Windows.CalendarCombo.CalendarCombo();
			this.uiComboBox1 = new Janus.Windows.EditControls.UIComboBox();
			this.uiButton1 = new Janus.Windows.EditControls.UIButton();
			this.uiButton2 = new Janus.Windows.EditControls.UIButton();
			((System.ComponentModel.ISupportInitialize)(this.uiPM)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.uiPanelMediaAgencys)).BeginInit();
			this.uiPanelMediaAgencys.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.uiPanelMediaAgencysSearch)).BeginInit();
			this.uiPanelMediaAgencysSearch.SuspendLayout();
			this.uiPanelMediaAgencysSearchContainer.SuspendLayout();
			this.pnlSearch.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.uiPanelMediaAgencyList)).BeginInit();
			this.uiPanelMediaAgencyList.SuspendLayout();
			this.uiPanelMediaAgencyListContainer.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.grdExMediaAgencyList)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.dvMediaAgency)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.mediaAgencyDs)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.uiPanelMediaAgencyDetail)).BeginInit();
			this.uiPanelMediaAgencyDetail.SuspendLayout();
			this.uiPanelMediaAgencyDetailContainer.SuspendLayout();
			this.pnlMediaAgencyDetail.SuspendLayout();
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
			this.uiPanelMediaAgencys.Id = new System.Guid("b162c53a-7940-47cd-a0ed-9762c59c50d8");
			this.uiPanelMediaAgencys.StaticGroup = true;
			this.uiPanelMediaAgencysSearch.Id = new System.Guid("afb3b7d0-aa79-443c-a1e8-3baba38f7a4d");
			this.uiPanelMediaAgencys.Panels.Add(this.uiPanelMediaAgencysSearch);
			this.uiPanelMediaAgencyList.Id = new System.Guid("8e3140b9-bfcb-43af-b84e-2ee6cddf51dc");
			this.uiPanelMediaAgencys.Panels.Add(this.uiPanelMediaAgencyList);
			this.uiPanelMediaAgencyDetail.Id = new System.Guid("b2973a57-d3e1-4d80-9387-7a6dc3332542");
			this.uiPanelMediaAgencys.Panels.Add(this.uiPanelMediaAgencyDetail);
			this.uiPM.Panels.Add(this.uiPanelMediaAgencys);
			// 
			// Design Time Panel Info:
			// 
			this.uiPM.BeginPanelInfo();
			this.uiPM.AddDockPanelInfo(new System.Guid("b162c53a-7940-47cd-a0ed-9762c59c50d8"), Janus.Windows.UI.Dock.PanelGroupStyle.HorizontalTiles, Janus.Windows.UI.Dock.PanelDockStyle.Fill, true, new System.Drawing.Size(1010, 677), true);
			this.uiPM.AddDockPanelInfo(new System.Guid("afb3b7d0-aa79-443c-a1e8-3baba38f7a4d"), new System.Guid("b162c53a-7940-47cd-a0ed-9762c59c50d8"), 40, true);
			this.uiPM.AddDockPanelInfo(new System.Guid("8e3140b9-bfcb-43af-b84e-2ee6cddf51dc"), new System.Guid("b162c53a-7940-47cd-a0ed-9762c59c50d8"), 385, true);
			this.uiPM.AddDockPanelInfo(new System.Guid("b2973a57-d3e1-4d80-9387-7a6dc3332542"), new System.Guid("b162c53a-7940-47cd-a0ed-9762c59c50d8"), 202, true);
			this.uiPM.AddFloatingPanelInfo(new System.Guid("b162c53a-7940-47cd-a0ed-9762c59c50d8"), Janus.Windows.UI.Dock.PanelGroupStyle.HorizontalTiles, true, new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
			this.uiPM.AddFloatingPanelInfo(new System.Guid("8e3140b9-bfcb-43af-b84e-2ee6cddf51dc"), new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
			this.uiPM.AddFloatingPanelInfo(new System.Guid("b2973a57-d3e1-4d80-9387-7a6dc3332542"), new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
			this.uiPM.AddFloatingPanelInfo(new System.Guid("afb3b7d0-aa79-443c-a1e8-3baba38f7a4d"), new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
			this.uiPM.EndPanelInfo();
			// 
			// uiPanelMediaAgencys
			// 
			this.uiPanelMediaAgencys.ActiveCaptionMode = Janus.Windows.UI.Dock.ActiveCaptionMode.Never;
			this.uiPanelMediaAgencys.BackColor = System.Drawing.SystemColors.Window;
			this.uiPanelMediaAgencys.CaptionVisible = Janus.Windows.UI.InheritableBoolean.True;
			this.uiPanelMediaAgencys.CloseButtonVisible = Janus.Windows.UI.InheritableBoolean.False;
			this.uiPanelMediaAgencys.Location = new System.Drawing.Point(0, 0);
			this.uiPanelMediaAgencys.Name = "uiPanelMediaAgencys";
			this.uiPanelMediaAgencys.Size = new System.Drawing.Size(1010, 677);
			this.uiPanelMediaAgencys.TabIndex = 4;
			this.uiPanelMediaAgencys.Text = "매체별 대행사관리";
			// 
			// uiPanelMediaAgencysSearch
			// 
			this.uiPanelMediaAgencysSearch.AllowResize = Janus.Windows.UI.InheritableBoolean.False;
			this.uiPanelMediaAgencysSearch.CaptionVisible = Janus.Windows.UI.InheritableBoolean.False;
			this.uiPanelMediaAgencysSearch.CloseButtonVisible = Janus.Windows.UI.InheritableBoolean.False;
			this.uiPanelMediaAgencysSearch.InnerContainer = this.uiPanelMediaAgencysSearchContainer;
			this.uiPanelMediaAgencysSearch.Location = new System.Drawing.Point(0, 22);
			this.uiPanelMediaAgencysSearch.Name = "uiPanelMediaAgencysSearch";
			this.uiPanelMediaAgencysSearch.Size = new System.Drawing.Size(1010, 41);
			this.uiPanelMediaAgencysSearch.TabIndex = 4;
			this.uiPanelMediaAgencysSearch.Text = "검색";
			// 
			// uiPanelMediaAgencysSearchContainer
			// 
			this.uiPanelMediaAgencysSearchContainer.Controls.Add(this.pnlSearch);
			this.uiPanelMediaAgencysSearchContainer.Location = new System.Drawing.Point(1, 1);
			this.uiPanelMediaAgencysSearchContainer.Name = "uiPanelMediaAgencysSearchContainer";
			this.uiPanelMediaAgencysSearchContainer.Size = new System.Drawing.Size(1008, 39);
			this.uiPanelMediaAgencysSearchContainer.TabIndex = 0;
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
			this.pnlSearch.Dock = System.Windows.Forms.DockStyle.Fill;
			this.pnlSearch.Location = new System.Drawing.Point(0, 0);
			this.pnlSearch.Name = "pnlSearch";
			this.pnlSearch.Size = new System.Drawing.Size(1008, 39);
			this.pnlSearch.TabIndex = 0;
			// 
			// uiCheckBox1
			// 
			this.uiCheckBox1.Location = new System.Drawing.Point(582, 6);
			this.uiCheckBox1.Name = "uiCheckBox1";
			this.uiCheckBox1.Size = new System.Drawing.Size(104, 23);
			this.uiCheckBox1.TabIndex = 7;
			this.uiCheckBox1.Text = "사용안함 포함";
			// 
			// ebSearchKey
			// 
			this.ebSearchKey.Location = new System.Drawing.Point(416, 8);
			this.ebSearchKey.Name = "ebSearchKey";
			this.ebSearchKey.Size = new System.Drawing.Size(160, 20);
			this.ebSearchKey.TabIndex = 4;
			this.ebSearchKey.Text = "검색어를 입력 하세요";
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
			this.cbSearchMediaName.Size = new System.Drawing.Size(128, 20);
			this.cbSearchMediaName.TabIndex = 1;
			this.cbSearchMediaName.Text = "매체선택";
			this.cbSearchMediaName.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
			// 
			// cbSearchRapName
			// 
			this.cbSearchRapName.ComboStyle = Janus.Windows.EditControls.ComboStyle.DropDownList;
			this.cbSearchRapName.Location = new System.Drawing.Point(144, 8);
			this.cbSearchRapName.Name = "cbSearchRapName";
			this.cbSearchRapName.Size = new System.Drawing.Size(128, 20);
			this.cbSearchRapName.TabIndex = 2;
			this.cbSearchRapName.Text = "미디어렙선택";
			this.cbSearchRapName.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
			// 
			// cbSearchMediaAgency
			// 
			this.cbSearchMediaAgency.ComboStyle = Janus.Windows.EditControls.ComboStyle.DropDownList;
			this.cbSearchMediaAgency.Location = new System.Drawing.Point(280, 8);
			this.cbSearchMediaAgency.Name = "cbSearchMediaAgency";
			this.cbSearchMediaAgency.Size = new System.Drawing.Size(128, 20);
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
			// uiPanelMediaAgencyList
			// 
			this.uiPanelMediaAgencyList.BackColor = System.Drawing.SystemColors.Window;
			this.uiPanelMediaAgencyList.CaptionStyle = Janus.Windows.UI.Dock.PanelCaptionStyle.Dark;
			this.uiPanelMediaAgencyList.InnerContainer = this.uiPanelMediaAgencyListContainer;
			this.uiPanelMediaAgencyList.Location = new System.Drawing.Point(0, 67);
			this.uiPanelMediaAgencyList.MinimumSize = new System.Drawing.Size(-1, 100);
			this.uiPanelMediaAgencyList.Name = "uiPanelMediaAgencyList";
			this.uiPanelMediaAgencyList.Size = new System.Drawing.Size(1010, 398);
			this.uiPanelMediaAgencyList.TabIndex = 6;
			this.uiPanelMediaAgencyList.TabStop = false;
			this.uiPanelMediaAgencyList.Text = "대행사목록";
			// 
			// uiPanelMediaAgencyListContainer
			// 
			this.uiPanelMediaAgencyListContainer.BackColor = System.Drawing.SystemColors.Window;
			this.uiPanelMediaAgencyListContainer.Controls.Add(this.grdExMediaAgencyList);
			this.uiPanelMediaAgencyListContainer.Location = new System.Drawing.Point(1, 23);
			this.uiPanelMediaAgencyListContainer.Name = "uiPanelMediaAgencyListContainer";
			this.uiPanelMediaAgencyListContainer.Size = new System.Drawing.Size(1008, 374);
			this.uiPanelMediaAgencyListContainer.TabIndex = 0;
			// 
			// grdExMediaAgencyList
			// 
			this.grdExMediaAgencyList.AllowEdit = Janus.Windows.GridEX.InheritableBoolean.False;
			this.grdExMediaAgencyList.AlternatingColors = true;
			this.grdExMediaAgencyList.BorderStyle = Janus.Windows.GridEX.BorderStyle.None;
			this.grdExMediaAgencyList.DataSource = this.dvMediaAgency;
			grdExMediaAgencyList_DesignTimeLayout.LayoutString = resources.GetString("grdExMediaAgencyList_DesignTimeLayout.LayoutString");
			this.grdExMediaAgencyList.DesignTimeLayout = grdExMediaAgencyList_DesignTimeLayout;
			this.grdExMediaAgencyList.Dock = System.Windows.Forms.DockStyle.Fill;
			this.grdExMediaAgencyList.EditorsControlStyle.ButtonAppearance = Janus.Windows.GridEX.ButtonAppearance.Regular;
			this.grdExMediaAgencyList.EmptyRows = true;
			this.grdExMediaAgencyList.FocusCellFormatStyle.BackColor = System.Drawing.SystemColors.Highlight;
			this.grdExMediaAgencyList.FocusCellFormatStyle.ForeColor = System.Drawing.Color.White;
			this.grdExMediaAgencyList.FocusStyle = Janus.Windows.GridEX.FocusStyle.None;
			this.grdExMediaAgencyList.FrozenColumns = 2;
			this.grdExMediaAgencyList.GridLineColor = System.Drawing.Color.Silver;
			this.grdExMediaAgencyList.GridLines = Janus.Windows.GridEX.GridLines.Vertical;
			this.grdExMediaAgencyList.GridLineStyle = Janus.Windows.GridEX.GridLineStyle.Solid;
			this.grdExMediaAgencyList.GroupByBoxVisible = false;
			this.grdExMediaAgencyList.HeaderFormatStyle.TextAlignment = Janus.Windows.GridEX.TextAlignment.Center;
			this.grdExMediaAgencyList.HideSelection = Janus.Windows.GridEX.HideSelection.HighlightInactive;
			this.grdExMediaAgencyList.Location = new System.Drawing.Point(0, 0);
			this.grdExMediaAgencyList.Name = "grdExMediaAgencyList";
			this.grdExMediaAgencyList.ScrollBars = Janus.Windows.GridEX.ScrollBars.Vertical;
			this.grdExMediaAgencyList.SelectedInactiveFormatStyle.BackColor = System.Drawing.Color.Gold;
			this.grdExMediaAgencyList.SelectedInactiveFormatStyle.ForeColor = System.Drawing.Color.Empty;
			this.grdExMediaAgencyList.Size = new System.Drawing.Size(1008, 374);
			this.grdExMediaAgencyList.TabIndex = 7;
			this.grdExMediaAgencyList.TabKeyBehavior = Janus.Windows.GridEX.TabKeyBehavior.ControlNavigation;
			this.grdExMediaAgencyList.ThemedAreas = ((Janus.Windows.GridEX.ThemedArea)(((((((((Janus.Windows.GridEX.ThemedArea.ScrollBars | Janus.Windows.GridEX.ThemedArea.EditControls) 
            | Janus.Windows.GridEX.ThemedArea.Headers) 
            | Janus.Windows.GridEX.ThemedArea.GroupByBox) 
            | Janus.Windows.GridEX.ThemedArea.GroupRows) 
            | Janus.Windows.GridEX.ThemedArea.ControlBorder) 
            | Janus.Windows.GridEX.ThemedArea.Cards) 
            | Janus.Windows.GridEX.ThemedArea.Gridlines) 
            | Janus.Windows.GridEX.ThemedArea.CheckBoxes)));
			this.grdExMediaAgencyList.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
			this.grdExMediaAgencyList.Enter += new System.EventHandler(this.OnGrdRowChanged);
			// 
			// dvMediaAgency
			// 
			this.dvMediaAgency.Table = this.mediaAgencyDs.MediaAgencys;
			// 
			// mediaAgencyDs
			// 
			this.mediaAgencyDs.DataSetName = "MediaAgencyDs";
			this.mediaAgencyDs.Locale = new System.Globalization.CultureInfo("en-US");
			this.mediaAgencyDs.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
			// 
			// uiPanelMediaAgencyDetail
			// 
			this.uiPanelMediaAgencyDetail.AllowResize = Janus.Windows.UI.InheritableBoolean.False;
			this.uiPanelMediaAgencyDetail.BackColor = System.Drawing.SystemColors.Window;
			this.uiPanelMediaAgencyDetail.CaptionStyle = Janus.Windows.UI.Dock.PanelCaptionStyle.Dark;
			this.uiPanelMediaAgencyDetail.InnerContainer = this.uiPanelMediaAgencyDetailContainer;
			this.uiPanelMediaAgencyDetail.Location = new System.Drawing.Point(0, 469);
			this.uiPanelMediaAgencyDetail.Name = "uiPanelMediaAgencyDetail";
			this.uiPanelMediaAgencyDetail.Size = new System.Drawing.Size(1010, 208);
			this.uiPanelMediaAgencyDetail.TabIndex = 8;
			this.uiPanelMediaAgencyDetail.TabStop = false;
			this.uiPanelMediaAgencyDetail.Text = "상세정보";
			// 
			// uiPanelMediaAgencyDetailContainer
			// 
			this.uiPanelMediaAgencyDetailContainer.Controls.Add(this.pnlMediaAgencyDetail);
			this.uiPanelMediaAgencyDetailContainer.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.uiPanelMediaAgencyDetailContainer.Location = new System.Drawing.Point(1, 23);
			this.uiPanelMediaAgencyDetailContainer.Name = "uiPanelMediaAgencyDetailContainer";
			this.uiPanelMediaAgencyDetailContainer.Size = new System.Drawing.Size(1008, 184);
			this.uiPanelMediaAgencyDetailContainer.TabIndex = 0;
			// 
			// pnlMediaAgencyDetail
			// 
			this.pnlMediaAgencyDetail.BackColor = System.Drawing.SystemColors.Window;
			this.pnlMediaAgencyDetail.Controls.Add(this.ebContEndDay);
			this.pnlMediaAgencyDetail.Controls.Add(this.btnMediaSearch);
			this.pnlMediaAgencyDetail.Controls.Add(this.ebAgencyName);
			this.pnlMediaAgencyDetail.Controls.Add(this.ebRapName);
			this.pnlMediaAgencyDetail.Controls.Add(this.ebMediaName);
			this.pnlMediaAgencyDetail.Controls.Add(this.rbUseYn_N);
			this.pnlMediaAgencyDetail.Controls.Add(this.rbUseYn_Y);
			this.pnlMediaAgencyDetail.Controls.Add(this.lbUseYn);
			this.pnlMediaAgencyDetail.Controls.Add(this.lbEmail);
			this.pnlMediaAgencyDetail.Controls.Add(this.lbModDt);
			this.pnlMediaAgencyDetail.Controls.Add(this.lbTell);
			this.pnlMediaAgencyDetail.Controls.Add(this.ebTell);
			this.pnlMediaAgencyDetail.Controls.Add(this.ebCharger);
			this.pnlMediaAgencyDetail.Controls.Add(this.lbCharger);
			this.pnlMediaAgencyDetail.Controls.Add(this.lbRegDt);
			this.pnlMediaAgencyDetail.Controls.Add(this.ebRegDt);
			this.pnlMediaAgencyDetail.Controls.Add(this.ebModDt);
			this.pnlMediaAgencyDetail.Controls.Add(this.ebEmail);
			this.pnlMediaAgencyDetail.Controls.Add(this.lbContStarDay);
			this.pnlMediaAgencyDetail.Controls.Add(this.lbContEndDay);
			this.pnlMediaAgencyDetail.Controls.Add(this.lbRapName);
			this.pnlMediaAgencyDetail.Controls.Add(this.lbMediaName);
			this.pnlMediaAgencyDetail.Controls.Add(this.lbAgencyName);
			this.pnlMediaAgencyDetail.Controls.Add(this.lbRegID);
			this.pnlMediaAgencyDetail.Controls.Add(this.ebRegID);
			this.pnlMediaAgencyDetail.Controls.Add(this.btnAdd);
			this.pnlMediaAgencyDetail.Controls.Add(this.btnDelete);
			this.pnlMediaAgencyDetail.Controls.Add(this.btnSave);
			this.pnlMediaAgencyDetail.Controls.Add(this.btnRapSearch);
			this.pnlMediaAgencyDetail.Controls.Add(this.btnAgencySearch);
			this.pnlMediaAgencyDetail.Controls.Add(this.ebContStartDay);
			this.pnlMediaAgencyDetail.Dock = System.Windows.Forms.DockStyle.Fill;
			this.pnlMediaAgencyDetail.Location = new System.Drawing.Point(0, 0);
			this.pnlMediaAgencyDetail.Name = "pnlMediaAgencyDetail";
			this.pnlMediaAgencyDetail.Size = new System.Drawing.Size(1008, 184);
			this.pnlMediaAgencyDetail.TabIndex = 0;
			// 
			// ebContEndDay
			// 
			// 
			// 
			// 
			this.ebContEndDay.DropDownCalendar.FirstMonth = new System.DateTime(2007, 8, 1, 0, 0, 0, 0);
			this.ebContEndDay.DropDownCalendar.VisualStyle = Janus.Windows.CalendarCombo.VisualStyle.Office2007;
			this.ebContEndDay.Location = new System.Drawing.Point(280, 64);
			this.ebContEndDay.Name = "ebContEndDay";
			this.ebContEndDay.Size = new System.Drawing.Size(112, 20);
			this.ebContEndDay.TabIndex = 9;
			this.ebContEndDay.VisualStyle = Janus.Windows.CalendarCombo.VisualStyle.Office2007;
			// 
			// btnMediaSearch
			// 
			this.btnMediaSearch.Enabled = false;
			this.btnMediaSearch.Location = new System.Drawing.Point(80, 32);
			this.btnMediaSearch.Name = "btnMediaSearch";
			this.btnMediaSearch.Size = new System.Drawing.Size(104, 23);
			this.btnMediaSearch.TabIndex = 19;
			this.btnMediaSearch.Text = "매체선택";
			this.btnMediaSearch.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
			this.btnMediaSearch.Click += new System.EventHandler(this.btnMediaSearch_Click);
			// 
			// ebAgencyName
			// 
			this.ebAgencyName.Location = new System.Drawing.Point(656, 8);
			this.ebAgencyName.MaxLength = 20;
			this.ebAgencyName.Name = "ebAgencyName";
			this.ebAgencyName.Size = new System.Drawing.Size(184, 20);
			this.ebAgencyName.TabIndex = 0;
			this.ebAgencyName.TabStop = false;
			this.ebAgencyName.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
			this.ebAgencyName.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
			// 
			// ebRapName
			// 
			this.ebRapName.Location = new System.Drawing.Point(368, 8);
			this.ebRapName.MaxLength = 20;
			this.ebRapName.Name = "ebRapName";
			this.ebRapName.Size = new System.Drawing.Size(184, 20);
			this.ebRapName.TabIndex = 0;
			this.ebRapName.TabStop = false;
			this.ebRapName.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
			this.ebRapName.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
			// 
			// ebMediaName
			// 
			this.ebMediaName.Location = new System.Drawing.Point(80, 8);
			this.ebMediaName.MaxLength = 20;
			this.ebMediaName.Name = "ebMediaName";
			this.ebMediaName.Size = new System.Drawing.Size(184, 20);
			this.ebMediaName.TabIndex = 0;
			this.ebMediaName.TabStop = false;
			this.ebMediaName.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
			this.ebMediaName.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
			// 
			// rbUseYn_N
			// 
			this.rbUseYn_N.BackColor = System.Drawing.Color.Transparent;
			this.rbUseYn_N.Location = new System.Drawing.Point(728, 136);
			this.rbUseYn_N.Name = "rbUseYn_N";
			this.rbUseYn_N.Size = new System.Drawing.Size(72, 23);
			this.rbUseYn_N.TabIndex = 15;
			this.rbUseYn_N.Text = "사용안함";
			this.rbUseYn_N.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
			// 
			// rbUseYn_Y
			// 
			this.rbUseYn_Y.BackColor = System.Drawing.Color.Transparent;
			this.rbUseYn_Y.Location = new System.Drawing.Point(656, 136);
			this.rbUseYn_Y.Name = "rbUseYn_Y";
			this.rbUseYn_Y.Size = new System.Drawing.Size(72, 23);
			this.rbUseYn_Y.TabIndex = 14;
			this.rbUseYn_Y.Text = "사용함";
			this.rbUseYn_Y.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
			// 
			// lbUseYn
			// 
			this.lbUseYn.Location = new System.Drawing.Point(584, 136);
			this.lbUseYn.Name = "lbUseYn";
			this.lbUseYn.Size = new System.Drawing.Size(72, 21);
			this.lbUseYn.TabIndex = 39;
			this.lbUseYn.Text = "사용여부";
			this.lbUseYn.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// lbEmail
			// 
			this.lbEmail.Location = new System.Drawing.Point(8, 112);
			this.lbEmail.Name = "lbEmail";
			this.lbEmail.Size = new System.Drawing.Size(72, 21);
			this.lbEmail.TabIndex = 38;
			this.lbEmail.Text = "Email";
			this.lbEmail.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// lbModDt
			// 
			this.lbModDt.Location = new System.Drawing.Point(584, 88);
			this.lbModDt.Name = "lbModDt";
			this.lbModDt.Size = new System.Drawing.Size(72, 21);
			this.lbModDt.TabIndex = 37;
			this.lbModDt.Text = "수정일시";
			this.lbModDt.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// lbTell
			// 
			this.lbTell.Location = new System.Drawing.Point(208, 88);
			this.lbTell.Name = "lbTell";
			this.lbTell.Size = new System.Drawing.Size(72, 21);
			this.lbTell.TabIndex = 30;
			this.lbTell.Text = "전화번호";
			this.lbTell.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// ebTell
			// 
			this.ebTell.Location = new System.Drawing.Point(280, 88);
			this.ebTell.MaxLength = 15;
			this.ebTell.Name = "ebTell";
			this.ebTell.Size = new System.Drawing.Size(112, 20);
			this.ebTell.TabIndex = 12;
			this.ebTell.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
			this.ebTell.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
			// 
			// ebCharger
			// 
			this.ebCharger.Location = new System.Drawing.Point(80, 88);
			this.ebCharger.MaxLength = 20;
			this.ebCharger.Name = "ebCharger";
			this.ebCharger.Size = new System.Drawing.Size(112, 20);
			this.ebCharger.TabIndex = 11;
			this.ebCharger.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
			this.ebCharger.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
			// 
			// lbCharger
			// 
			this.lbCharger.Location = new System.Drawing.Point(8, 88);
			this.lbCharger.Name = "lbCharger";
			this.lbCharger.Size = new System.Drawing.Size(72, 21);
			this.lbCharger.TabIndex = 19;
			this.lbCharger.Text = "담당자명";
			this.lbCharger.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// lbRegDt
			// 
			this.lbRegDt.Location = new System.Drawing.Point(584, 64);
			this.lbRegDt.Name = "lbRegDt";
			this.lbRegDt.Size = new System.Drawing.Size(72, 21);
			this.lbRegDt.TabIndex = 29;
			this.lbRegDt.Text = "등록일시";
			this.lbRegDt.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// ebRegDt
			// 
			this.ebRegDt.BackColor = System.Drawing.Color.WhiteSmoke;
			this.ebRegDt.Location = new System.Drawing.Point(656, 64);
			this.ebRegDt.Name = "ebRegDt";
			this.ebRegDt.ReadOnly = true;
			this.ebRegDt.Size = new System.Drawing.Size(184, 20);
			this.ebRegDt.TabIndex = 0;
			this.ebRegDt.TabStop = false;
			this.ebRegDt.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
			this.ebRegDt.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
			// 
			// ebModDt
			// 
			this.ebModDt.BackColor = System.Drawing.Color.WhiteSmoke;
			this.ebModDt.Location = new System.Drawing.Point(656, 88);
			this.ebModDt.Name = "ebModDt";
			this.ebModDt.ReadOnly = true;
			this.ebModDt.Size = new System.Drawing.Size(184, 20);
			this.ebModDt.TabIndex = 0;
			this.ebModDt.TabStop = false;
			this.ebModDt.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
			this.ebModDt.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
			// 
			// ebEmail
			// 
			this.ebEmail.Location = new System.Drawing.Point(80, 112);
			this.ebEmail.MaxLength = 20;
			this.ebEmail.Name = "ebEmail";
			this.ebEmail.Size = new System.Drawing.Size(312, 20);
			this.ebEmail.TabIndex = 13;
			this.ebEmail.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
			this.ebEmail.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
			// 
			// lbContStarDay
			// 
			this.lbContStarDay.Location = new System.Drawing.Point(8, 64);
			this.lbContStarDay.Name = "lbContStarDay";
			this.lbContStarDay.Size = new System.Drawing.Size(72, 21);
			this.lbContStarDay.TabIndex = 29;
			this.lbContStarDay.Text = "대행시작일";
			this.lbContStarDay.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// lbContEndDay
			// 
			this.lbContEndDay.Location = new System.Drawing.Point(208, 64);
			this.lbContEndDay.Name = "lbContEndDay";
			this.lbContEndDay.Size = new System.Drawing.Size(72, 21);
			this.lbContEndDay.TabIndex = 37;
			this.lbContEndDay.Text = "대행종료일";
			this.lbContEndDay.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// lbRapName
			// 
			this.lbRapName.Location = new System.Drawing.Point(296, 8);
			this.lbRapName.Name = "lbRapName";
			this.lbRapName.Size = new System.Drawing.Size(72, 21);
			this.lbRapName.TabIndex = 30;
			this.lbRapName.Text = "미디어렙";
			this.lbRapName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// lbMediaName
			// 
			this.lbMediaName.Location = new System.Drawing.Point(8, 8);
			this.lbMediaName.Name = "lbMediaName";
			this.lbMediaName.Size = new System.Drawing.Size(72, 21);
			this.lbMediaName.TabIndex = 19;
			this.lbMediaName.Text = "매체";
			this.lbMediaName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// lbAgencyName
			// 
			this.lbAgencyName.Location = new System.Drawing.Point(584, 8);
			this.lbAgencyName.Name = "lbAgencyName";
			this.lbAgencyName.Size = new System.Drawing.Size(72, 21);
			this.lbAgencyName.TabIndex = 19;
			this.lbAgencyName.Text = "대행사";
			this.lbAgencyName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// lbRegID
			// 
			this.lbRegID.Location = new System.Drawing.Point(584, 112);
			this.lbRegID.Name = "lbRegID";
			this.lbRegID.Size = new System.Drawing.Size(72, 21);
			this.lbRegID.TabIndex = 37;
			this.lbRegID.Text = "등록자";
			this.lbRegID.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// ebRegID
			// 
			this.ebRegID.BackColor = System.Drawing.Color.WhiteSmoke;
			this.ebRegID.Location = new System.Drawing.Point(656, 112);
			this.ebRegID.Name = "ebRegID";
			this.ebRegID.ReadOnly = true;
			this.ebRegID.Size = new System.Drawing.Size(184, 20);
			this.ebRegID.TabIndex = 0;
			this.ebRegID.TabStop = false;
			this.ebRegID.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
			this.ebRegID.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
			// 
			// btnAdd
			// 
			this.btnAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.btnAdd.Enabled = false;
			this.btnAdd.Location = new System.Drawing.Point(232, 150);
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
			this.btnDelete.Enabled = false;
			this.btnDelete.Location = new System.Drawing.Point(120, 150);
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
			this.btnSave.Enabled = false;
			this.btnSave.Location = new System.Drawing.Point(8, 150);
			this.btnSave.Name = "btnSave";
			this.btnSave.Size = new System.Drawing.Size(104, 24);
			this.btnSave.TabIndex = 16;
			this.btnSave.Text = "저 장";
			this.btnSave.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
			this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
			// 
			// btnRapSearch
			// 
			this.btnRapSearch.Enabled = false;
			this.btnRapSearch.Location = new System.Drawing.Point(368, 32);
			this.btnRapSearch.Name = "btnRapSearch";
			this.btnRapSearch.Size = new System.Drawing.Size(104, 23);
			this.btnRapSearch.TabIndex = 20;
			this.btnRapSearch.Text = "미디어렙선택";
			this.btnRapSearch.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
			this.btnRapSearch.Click += new System.EventHandler(this.btnRapSearch_Click);
			// 
			// btnAgencySearch
			// 
			this.btnAgencySearch.Enabled = false;
			this.btnAgencySearch.Location = new System.Drawing.Point(656, 32);
			this.btnAgencySearch.Name = "btnAgencySearch";
			this.btnAgencySearch.Size = new System.Drawing.Size(104, 23);
			this.btnAgencySearch.TabIndex = 21;
			this.btnAgencySearch.Text = "대행사선택";
			this.btnAgencySearch.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
			this.btnAgencySearch.Click += new System.EventHandler(this.btnAgencySearch_Click);
			// 
			// ebContStartDay
			// 
			// 
			// 
			// 
			this.ebContStartDay.DropDownCalendar.FirstMonth = new System.DateTime(2007, 8, 1, 0, 0, 0, 0);
			this.ebContStartDay.DropDownCalendar.VisualStyle = Janus.Windows.CalendarCombo.VisualStyle.Office2007;
			this.ebContStartDay.Location = new System.Drawing.Point(80, 64);
			this.ebContStartDay.Name = "ebContStartDay";
			this.ebContStartDay.Size = new System.Drawing.Size(112, 20);
			this.ebContStartDay.TabIndex = 8;
			this.ebContStartDay.VisualStyle = Janus.Windows.CalendarCombo.VisualStyle.Office2007;
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
			// MediaAgencyControl
			// 
			this.Controls.Add(this.uiPanelMediaAgencys);
			this.Font = global::AdManagerClient.Properties.Settings.Default.Font1;
			this.Name = "MediaAgencyControl";
			this.Size = new System.Drawing.Size(1010, 677);
			this.Load += new System.EventHandler(this.MediaAgencyControl_Load);
			((System.ComponentModel.ISupportInitialize)(this.uiPM)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.uiPanelMediaAgencys)).EndInit();
			this.uiPanelMediaAgencys.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.uiPanelMediaAgencysSearch)).EndInit();
			this.uiPanelMediaAgencysSearch.ResumeLayout(false);
			this.uiPanelMediaAgencysSearchContainer.ResumeLayout(false);
			this.pnlSearch.ResumeLayout(false);
			this.pnlSearch.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.uiPanelMediaAgencyList)).EndInit();
			this.uiPanelMediaAgencyList.ResumeLayout(false);
			this.uiPanelMediaAgencyListContainer.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.grdExMediaAgencyList)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.dvMediaAgency)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.mediaAgencyDs)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.uiPanelMediaAgencyDetail)).EndInit();
			this.uiPanelMediaAgencyDetail.ResumeLayout(false);
			this.uiPanelMediaAgencyDetailContainer.ResumeLayout(false);
			this.pnlMediaAgencyDetail.ResumeLayout(false);
			this.pnlMediaAgencyDetail.PerformLayout();
			this.ResumeLayout(false);

		}
		#endregion

		#region 컨트롤 로드
		private void MediaAgencyControl_Load(object sender, System.EventArgs e)
		{
			// 데이터관리용 객체생성
			dt = ((DataView)grdExMediaAgencyList.DataSource).Table;  
			cm = (CurrencyManager) this.BindingContext[grdExMediaAgencyList.DataSource]; 
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
			InitCombo_Level();

//			InitCombo_Insert();
//			InitCombo_Rap_Insert();	
//			InitCombo_Agency_Insert();

			// 조회권한 검사
			if(menu.CanRead(MenuCode))
			{
				canRead = true;
				SearchMediaAgency();				
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
				Utility.SetDataTable(mediaAgencyDs.Medias, mediacodeModel.MediaCodeDataSet);				
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
				DataRow row = mediaAgencyDs.Medias.Rows[i];

				string val = row["MediaCode"].ToString();
				string txt = row["MediaName"].ToString();
				comboItems[i+1] = new Janus.Windows.EditControls.UIComboBoxItem(txt,val);
			}
			// 콤보에 셋트
			this.cbSearchMediaName.Items.AddRange(comboItems);
			this.cbSearchMediaName.SelectedIndex = 0;
			Application.DoEvents();
		}

		private void InitCombo_Rap()
		{
            /* 랩사는 고정 - 모바일 
			// 코드에서 보안레벨을 조회한다.
			MediaRapCodeModel mediarapcodeModel = new MediaRapCodeModel();
			//codeModel.Section = "21";				// 코드분류 '11':보안레벨  TODO: 코드분류는 추후 XML로 관리되어야...
			new MediaRapCodeManager(systemModel, commonModel).GetMediaRapCodeList(mediarapcodeModel);
			
			if (mediarapcodeModel.ResultCD.Equals("0000"))
			{
				// 데이터셋에 셋팅
				Utility.SetDataTable(mediaAgencyDs.MediaRaps, mediarapcodeModel.MediaRapCodeDataSet);				
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
				DataRow row = mediaAgencyDs.MediaRaps.Rows[i];

				string val = row["RapCode"].ToString();
				string txt = row["RapName"].ToString();
				comboItems[i+1] = new Janus.Windows.EditControls.UIComboBoxItem(txt,val);
			}
			// 콤보에 셋트
			this.cbSearchRapName.Items.AddRange(comboItems);
			this.cbSearchRapName.SelectedIndex = 0;
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

            Utility.SetDataTable(mediaAgencyDs.MediaRap, ds);
            // 검색조건의 콤보
            this.cbSearchRapName.Items.Clear();
            // 콤보박스에 셋트할 코드목록을 담을 Item배열을 선언
            Janus.Windows.EditControls.UIComboBoxItem[] comboItems = new Janus.Windows.EditControls.UIComboBoxItem[2];

            comboItems[0] = new Janus.Windows.EditControls.UIComboBoxItem("미디어렙선택", "00");

            for (int i = 0; i < 1; i++)
            {
                DataRow row = mediaAgencyDs.MediaRap.Rows[i];

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
				Utility.SetDataTable(mediaAgencyDs.Agencys, agencycodeModel.AgencyCodeDataSet);				
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
				DataRow row = mediaAgencyDs.Agencys.Rows[i];

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
				for(int i=0;i < mediaAgencyDs.Medias.Rows.Count;i++)
				{
					DataRow row = mediaAgencyDs.Medias.Rows[i];					
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
	

		private void InitButton()
		{
			if(canRead)   btnSearch.Enabled = true;
			if(canCreate) btnAdd.Enabled    = true;

			if(ebAgencyName.Text.Trim().Length > 0) 
			{
				if(canDelete) btnDelete.Enabled = true;
				if(canUpdate) btnSave.Enabled   = true;
			}
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

		#region 매체대행사 액션처리 메소드

		/// <summary>
		/// 그리드의 Row변경시
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void OnGrdRowChanged(object sender, System.EventArgs e) 
		{
            if (!IsSearching) // 2011.11.29 JH.Park 조회중이 아닐경우에만 동작하도록 변경
            {
                if (grdExMediaAgencyList.RecordCount > 0)
                {
                    SetMediaAgencyDetailText();
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
            ProgressStart();
			DisableButton();
			SearchMediaAgency();
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
			ResetMediaAgencyDetailText();
			
			ebCharger.Focus();
		}

		/// <summary>
		/// 저장버튼 클릭
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnSave_Click(object sender, System.EventArgs e)
		{			
			SaveMediaAgencyDetail();			
		}

		/// <summary>
		/// 삭제버튼 클릭
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnDelete_Click(object sender, System.EventArgs e)
		{			
			DeleteMediaAgency();			
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
				SearchMediaAgency();
			}
		}

		private void btnContStartDay_Click(object sender, System.EventArgs e)
		{
			CalendarForm calendarForm = new CalendarForm(this,"Start");
			calendarForm.ShowDialog(this);
			calendarForm.Dispose();
			calendarForm = null;			
		}

		private void btnContEndDay_Click(object sender, System.EventArgs e)
		{
			CalendarForm calendarForm = new CalendarForm(this,"End");
			calendarForm.ShowDialog(this);
			calendarForm.Dispose();
			calendarForm = null;			
		}		

		public string SetYear1
		{				
			set
			{
				this.ebContStartDay.Text = value;
			}			
		}

		public string SetYear2
		{				
			set
			{
				this.ebContEndDay.Text = value;
			}			
		}

		#endregion

		#region 처리메소드

		/// <summary>
		/// 매체대행사목록 조회
		/// </summary>
		private void SearchMediaAgency()
		{
            IsSearching = true;

			StatusMessage("매체대행사 정보를 조회합니다.");

			try
			{
				mediaAgencyModel.Init();
				// 데이터모델에 전송할 내용을 셋트한다.
				if(IsNewSearchKey)
				{
					mediaAgencyModel.SearchKey = "";
				}
				else
				{
					mediaAgencyModel.SearchKey  = ebSearchKey.Text;
				}

				mediaAgencyModel.SearchMediaName = cbSearchMediaName.SelectedItem.Value.ToString();
				mediaAgencyModel.SearchRapName = cbSearchRapName.SelectedItem.Value.ToString();
				mediaAgencyModel.SearchMediaAgency = cbSearchMediaAgency.SelectedItem.Value.ToString();

                if (uiCheckBox1.Checked)
				{
					mediaAgencyModel.SearchchkAdState_10   = "Y";
				}
				else
				{
					mediaAgencyModel.SearchchkAdState_10   = "N";
				}

				// 매체대행사목록조회 서비스를 호출한다.
				new MediaAgencyManager(systemModel,commonModel).GetMediaAgencyList(mediaAgencyModel);
				
				if (mediaAgencyModel.ResultCD.Equals("0000"))
				{
					Utility.SetDataTable(mediaAgencyDs.MediaAgencys, mediaAgencyModel.MediaAgencyDataSet);				
					StatusMessage(mediaAgencyModel.ResultCnt + "건의 매체대행사 정보가 조회되었습니다.");
					if(canUpdate)
					{
						AddSchChoice();										
					}										
					SetMediaAgencyDetailText();					
				}
			}
			catch(FrameException fe)
			{
				FrameSystem.showMsgForm("매체대행사조회오류", new string[] {fe.ErrCode, fe.ResultMsg});
			}
			catch(Exception ex)
			{
				FrameSystem.showMsgForm("매체대행사조회오류",new string[] {"",ex.Message});
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
				if ( mediaAgencyDs.Tables["MediaAgencys"].Rows.Count < 1 ) return;
              
				foreach (DataRow row in mediaAgencyDs.Tables["MediaAgencys"].Rows)
				{					
					if(IsAdding)
					{
						cm.Position = 0;
						mediaCode = null;
						rapCode = null;
						agencyCode = null;						
					}
					else
					{						
						if(row["MediaCode"].ToString().Equals(mediaCode) && row["RapCode"].ToString().Equals(rapCode) && row["AgencyCode"].ToString().Equals(agencyCode))
						{					
							cm.Position = rowIndex;
							break;								
						}
					}

					rowIndex++;
					grdExMediaAgencyList.EnsureVisible();
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
		/// 매체대행사상세정보 저장
		/// </summary>
		private void SaveMediaAgencyDetail()
		{
			StatusMessage("매체대행사 정보를 저장합니다.");
			
			if(mediaCode.Length == 0) 
			{
				MessageBox.Show("매체가 입력되지 않았습니다.","매체대행사 저장", 
					MessageBoxButtons.OK, MessageBoxIcon.Information );
					ResetPop("media");
				return;								
			}

			if(rapCode.Length == 0) 
			{
				MessageBox.Show("랩사가 입력되지 않았습니다.","매체대행사 저장", 
					MessageBoxButtons.OK, MessageBoxIcon.Information );
					ResetPop("rap");
				return;								
			}
			
			if(agencyCode.Length == 0) 
			{
				MessageBox.Show("대행사가 입력되지 않았습니다.","매체대행사 저장", 
					MessageBoxButtons.OK, MessageBoxIcon.Information );
					ResetPop("agency");
				return;								
			}

			try
			{
                if (IsAdding) // 수정 할 때는 예외
                {
                    //디트라이브..고현정씨 요청으로 수정 매체대행사는 대행사까지 유일무이한 값이 들어가야 하지만 그렇지 못한 경우
                    //DB에서 PK에러가 난다.
                    //메시지만 처리..08.04.16	박병준
                    foreach (DataRow row in mediaAgencyDs.Tables["MediaAgencys"].Rows)
                    {
                        if (row["MediaCode"].ToString().Equals(mediaCode))
                        {
                            if (row["RapCode"].ToString().Equals(rapCode))
                            {
                                if (row["AgencyCode"].ToString().Equals(agencyCode))
                                {
                                    MessageBox.Show("기존 매체와 렙사, 대행사가 있습니다. 확인해 주세요!!", "매체대행사 저장",
                                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    return;
                                }
                            }
                        }
                    }
                }

				// 데이터모델에 전송할 내용을 셋트한다.
				mediaAgencyModel.MediaCode       = mediaCode;
				mediaAgencyModel.RapCode       = rapCode;
				mediaAgencyModel.AgencyCode       = agencyCode;
				mediaAgencyModel.Charger       = ebCharger.Text.Trim();				
				mediaAgencyModel.ContStartDay     = ebContStartDay.Value.ToString("yyyyMMdd");
				mediaAgencyModel.ContEndDay    = ebContEndDay.Value.ToString("yyyyMMdd");	
				mediaAgencyModel.Tell     = ebTell.Text;				
				mediaAgencyModel.Email  = ebEmail.Text;

				//사용여부
				if(rbUseYn_Y.Checked)
				{
					mediaAgencyModel.UseYn       = "Y";
				}
				else
				{
					mediaAgencyModel.UseYn       = "N";
				}
				
				// 매체대행사 상세정보 저장 서비스를 호출한다.
				if (IsAdding)
				{
					new MediaAgencyManager(systemModel,commonModel).SetMediaAgencyAdd(mediaAgencyModel);
					StatusMessage("매체대행사 정보가 추가되었습니다.");
					IsAdding = false;
					ResetMediaAgencyDetailText();
				}
				else
				{
					new MediaAgencyManager(systemModel,commonModel).SetMediaAgencyUpdate(mediaAgencyModel);
					StatusMessage("매체대행사 정보가 저장되었습니다.");
				}
				
				DisableButton();
				SearchMediaAgency();
				InitButton();
			}
			catch(FrameException fe)
			{
				FrameSystem.showMsgForm("매체대행사정보 저장오류", new string[] {fe.ErrCode, fe.ResultMsg});
			}
			catch(Exception ex)
			{
				FrameSystem.showMsgForm("매체대행사정보 저장오류",new string[] {"",ex.Message});
			}			
		}

		/// <summary>
		/// 매체대행사정보 삭제
		/// </summary>
		private void DeleteMediaAgency()
		{
			StatusMessage("매체대행사 정보를 삭제합니다.");

			DialogResult result = MessageBox.Show("해당 매체대행사 정보를 삭제 하시겠습니까?","매체대행사 삭제",
				MessageBoxButtons.YesNo, MessageBoxIcon.Question,
				MessageBoxDefaultButton.Button2);

			if (result == DialogResult.No) return;

			try 
			{
				// 데이터모델에 전송할 내용을 셋트한다.
				mediaAgencyModel.MediaCode       = mediaCode;
				mediaAgencyModel.RapCode       = rapCode;
				mediaAgencyModel.AgencyCode       = agencyCode;

				// 매체대행사 상세정보 저장 서비스를 호출한다.
				new MediaAgencyManager(systemModel,commonModel).SetMediaAgencyDelete(mediaAgencyModel);

				StatusMessage("매체대행사 정보가 삭제되었습니다.");	
				ResetMediaAgencyDetailText();					

				DisableButton();
				SearchMediaAgency();
				InitButton();

			}
			catch(FrameException fe)
			{
				FrameSystem.showMsgForm("매체대행사정보 삭제오류", new string[] {fe.ErrCode, fe.ResultMsg});
			}
			catch(Exception ex)
			{
				FrameSystem.showMsgForm("매체대행사정보 삭제오류",new string[] {"",ex.Message});
			}			

		}

		/// <summary>
		/// 매체대행사 상세정보의 셋트
		/// </summary>
		private void SetMediaAgencyDetailText()
		{
			int curRow = cm.Position;
			if(curRow >= 0)
			{				
				mediaCode	=	dt.Rows[curRow]["MediaCode"].ToString();			
				rapCode	=	dt.Rows[curRow]["RapCode"].ToString();			
				agencyCode	=	dt.Rows[curRow]["AgencyCode"].ToString();			
				ebMediaName.Text        = dt.Rows[curRow]["MediaName"].ToString();			
				ebRapName.Text           = dt.Rows[curRow]["RapName"].ToString();			
				ebAgencyName.Text      = dt.Rows[curRow]["AgencyName"].ToString();			
				ebCharger.Text             = dt.Rows[curRow]["Charger"].ToString();			
				//ebContStartDay.Text           = dt.Rows[curRow]["ContStartDay"].ToString();
				string ContStartDay = dt.Rows[cm.Position]["ContStartDay"].ToString();
				if(ContStartDay.Length == 8)
				{
					int yyyy = Convert.ToInt32(ContStartDay.Substring(0, 4));
					int mm   = Convert.ToInt32(ContStartDay.Substring(4, 2));
					int dd   = Convert.ToInt32(ContStartDay.Substring(6, 2));
					ebContStartDay.Value = new DateTime(yyyy,mm,dd);	
				}
				else
				{
					ebContStartDay.Value = DateTime.Now;	
				}
				//ebContEndDay.Text           = dt.Rows[curRow]["ContEndDay"].ToString();
				string ContEndDay = dt.Rows[cm.Position]["ContEndDay"].ToString();
				if(ContEndDay.Length == 8)
				{
					int yyyy = Convert.ToInt32(ContEndDay.Substring(0, 4));
					int mm   = Convert.ToInt32(ContEndDay.Substring(4, 2));
					int dd   = Convert.ToInt32(ContEndDay.Substring(6, 2));
					ebContEndDay.Value = new DateTime(yyyy,mm,dd);	
				}
				else
				{
					ebContEndDay.Value = DateTime.Now;	
				}
				ebTell.Text           = dt.Rows[curRow]["Tell"].ToString();			
				ebEmail.Text           = dt.Rows[curRow]["Email"].ToString();			
				ebRegDt.Text              = dt.Rows[curRow]["RegDt"].ToString();
				ebModDt.Text             = dt.Rows[curRow]["ModDt"].ToString();
				ebRegID.Text             = dt.Rows[curRow]["RegName"].ToString();
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

		private void ResetMediaAgencyDetailText()
		{
			mediaCode             = "";			
			rapCode             = "";			
			agencyCode             = "";			
			ebMediaName.Text             = "";			
			ebRapName.Text             = "";			
			ebAgencyName.Text             = "";			
			ebCharger.Text             = "";			
			ebContStartDay.Value = DateTime.Now;	
			ebContEndDay.Value = DateTime.Now;	
			ebTell.Text           = "";			
			ebEmail.Text           = "";					
			ebRegDt.Text              = "";
			ebModDt.Text				= "";
			ebRegID.Text				= "";
			
			if(!IsAdding)
			{
				ebMediaName.ReadOnly         = true;			
				ebRapName.ReadOnly         = true;			
				ebAgencyName.ReadOnly         = true;						
				ebCharger.ReadOnly         = false;			
				ebContStartDay.ReadOnly       = false;
				ebContEndDay.ReadOnly      = false;			
				ebTell.ReadOnly       = false;
				ebEmail.ReadOnly       = false;
				rbUseYn_Y.Enabled			= true;
				rbUseYn_N.Enabled			= true;
						
				ebMediaName.BackColor        = Color.White;			
				ebRapName.BackColor        = Color.White;			
				ebAgencyName.BackColor        = Color.White;			
				ebCharger.BackColor        = Color.White;			
				ebContStartDay.BackColor      = Color.White;
				ebContEndDay.BackColor     = Color.White;			
				ebTell.BackColor      = Color.White;			
				ebEmail.BackColor      = Color.White;	
			}
			else
			{
				if(commonModel.UserLevel=="20")
				{				
					ebMediaName.Text = commonModel.MediaName;		
					mediaCode = commonModel.MediaCode;		
					btnMediaSearch.Enabled     = false;					
				}
			
				if(commonModel.UserLevel=="30")
				{			
					ebRapName.Text = commonModel.RapName;
					rapCode = commonModel.RapCode;
					btnRapSearch.Enabled     = false;															
				}
				if(commonModel.UserLevel=="40")
				{				
					ebAgencyName.Text = commonModel.AgencyName;	
					agencyCode = commonModel.AgencyCode;	
					btnAgencySearch.Enabled     = false;	
				}		
			}
		}
		
		/// <summary>
		/// 상세정보 ReadOnly
		/// </summary>
		private void SetTextReadonly()
		{
			ebMediaName.ReadOnly         = true;			
			ebRapName.ReadOnly         = true;			
			ebAgencyName.ReadOnly         = true;						
			ebCharger.ReadOnly         = false;			
			ebContStartDay.ReadOnly       = false;
			ebContEndDay.ReadOnly      = false;			
			ebTell.ReadOnly       = false;
			ebEmail.ReadOnly       = false;
			rbUseYn_Y.Enabled			= false;
			rbUseYn_N.Enabled			= false;
						
			ebMediaName.BackColor        = Color.WhiteSmoke;			
			ebRapName.BackColor        = Color.WhiteSmoke;			
			ebAgencyName.BackColor        = Color.WhiteSmoke;			
			ebCharger.BackColor        = Color.WhiteSmoke;			
			ebContStartDay.BackColor      = Color.WhiteSmoke;
			ebContEndDay.BackColor     = Color.WhiteSmoke;			
			ebTell.BackColor      = Color.WhiteSmoke;			
			ebEmail.BackColor      = Color.WhiteSmoke;	
						
		}

		/// <summary>
		/// 상세정보 수정가능케
		/// </summary>
		private void ResetTextReadonly()
		{											
			ebCharger.ReadOnly         = false;			
			ebContStartDay.ReadOnly       = false;
			ebContEndDay.ReadOnly      = false;			
			ebTell.ReadOnly       = false;
			ebEmail.ReadOnly       = false;

			rbUseYn_Y.Enabled         = true;
			rbUseYn_N.Enabled         = true;
			
			ebCharger.BackColor        = Color.White;			
			ebContStartDay.BackColor      = Color.White;
			ebContEndDay.BackColor     = Color.White;			
			ebTell.BackColor      = Color.White;			
			ebEmail.BackColor      = Color.White;	

			if (IsAdding)
			{
				btnMediaSearch.Enabled     = true;
				btnRapSearch.Enabled     = true;
				btnAgencySearch.Enabled     = true;
				ebMediaName.ReadOnly         = true;			
				ebRapName.ReadOnly         = true;			
				ebAgencyName.ReadOnly         = true;						
				ebCharger.ReadOnly         = false;			
				ebContStartDay.ReadOnly       = false;
				ebContEndDay.ReadOnly      = false;			
				ebTell.ReadOnly       = false;
				ebEmail.ReadOnly       = false;


				ebMediaName.BackColor        = Color.White;			
				ebRapName.BackColor        = Color.White;			
				ebAgencyName.BackColor        = Color.White;			
				ebCharger.BackColor        = Color.White;			
				ebContStartDay.BackColor      = Color.White;
				ebContEndDay.BackColor     = Color.White;			
				ebTell.BackColor      = Color.White;			
				ebEmail.BackColor      = Color.White;	
			}
			else
			{
				btnMediaSearch.Enabled     = false;
				btnRapSearch.Enabled     = false;
				btnAgencySearch.Enabled     = false;
				ebMediaName.ReadOnly         = true;			
				ebRapName.ReadOnly         = true;			
				ebAgencyName.ReadOnly         = true;						
				ebCharger.ReadOnly         = false;			
				ebContStartDay.ReadOnly       = false;
				ebContEndDay.ReadOnly      = false;			
				ebTell.ReadOnly       = false;
				ebEmail.ReadOnly       = false;
				
				ebMediaName.BackColor        = Color.WhiteSmoke;			
				ebRapName.BackColor        = Color.WhiteSmoke;			
				ebAgencyName.BackColor        = Color.WhiteSmoke;			
				ebCharger.BackColor        = Color.WhiteSmoke;			
				ebContStartDay.BackColor      = Color.WhiteSmoke;
				ebContEndDay.BackColor     = Color.WhiteSmoke;			
				ebTell.BackColor      = Color.WhiteSmoke;			
				ebEmail.BackColor      = Color.WhiteSmoke;	
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
		//이 함수는 매체, 랩사, 대행사를 입력하지않았을경우 경고창이 뜨고 확인을 누르면 이 메소드를 실행한다.
		//이 메소드가 실행이 되면 해당 필드의 팝업이 호출된다.
		private void ResetPop(object flag)
		{
			//매체
			if(flag.Equals("media"))
			{
				MediaPopForm mediaForm = new MediaPopForm(this);
				mediaForm.ShowDialog();	            
				mediaForm.Dispose();
				mediaForm = null;		
			}
			//랩사
			if(flag.Equals("rap"))
			{
				RapPopForm rapForm = new RapPopForm(this);
				rapForm.ShowDialog();            
				rapForm.Dispose();
				rapForm = null;		
			}
			//대행사
			if(flag.Equals("agency"))
			{
				AgencyPopForm agencyForm = new AgencyPopForm(this);
				agencyForm.ShowDialog();            
				agencyForm.Dispose();
				agencyForm = null;		
			}
		}
		private void btnMediaSearch_Click(object sender, System.EventArgs e)
		{
			// 매체선택 검색 팝업 띠우기
			MediaPopForm mediaForm = new MediaPopForm(this);

			mediaForm.ShowDialog();
            
			mediaForm.Dispose();
			mediaForm = null;		
		}
		public string MediaCode
		{
			set
			{
				this.mediaCode = value;
			}
		}

		public string MediaName
		{				
			set
			{
				this.ebMediaName.Text = value;
			}			
		}
		
		private void btnRapSearch_Click(object sender, System.EventArgs e)
		{
			// 미디어렙선택 목록 검색 팝업 띠우기
			RapPopForm rapForm = new RapPopForm(this);

			rapForm.ShowDialog();
            
			rapForm.Dispose();
			rapForm = null;		
		}
		public string RapCode
		{
			set
			{
				this.rapCode = value;
			}
		}

		public string RapName
		{				
			set
			{
				this.ebRapName.Text = value;
			}			
		}

		private void btnAgencySearch_Click(object sender, System.EventArgs e)
		{
			// 대행사선택 목록 검색 팝업 띠우기
			AgencyPopForm agencyForm = new AgencyPopForm(this);

			agencyForm.ShowDialog();
            
			agencyForm.Dispose();
			agencyForm = null;		
		}
			
		public string AgencyCode
		{
			set
			{
				this.agencyCode = value;
			}
		}

		public string AgencyName
		{				
			set
			{
				this.ebAgencyName.Text = value;
			}			
		}

	}
}