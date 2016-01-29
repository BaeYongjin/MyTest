// ===============================================================================
// ChannelJumpControl 
//
// ChannelJumpControl.cs
//
// 채널점핑 컨드롤을 정의합니다. 
//
// ===============================================================================
// Release history
// ===============================================================================
// Copyright (C) 2007 G-Inno Systems Inc.
// All rights reserved.
// 
/*
 * -------------------------------------------------------
 * 수정코드  : [E_01]
 * 수정자    : HJ
 * 수정일    : 2015.05.08
 * 수정내용  : [NEXT-UI] 
 *            - 장르점핑기능을 메뉴점핑으로 수정 및 활성화
 *            - 기존 장르점핑이 GenreCode(int) 컬럼에 저장하는데
 *            입력되는형식이 00X|00X|00X 로 VARCHAR형이 필요함
 *            GenreCode(int) -> ChannelManager(varchar)로 변경
 * -------------------------------------------------------
 */ 
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
    /// 채널점핑관리 컨트롤
    /// </summary>
    public class ChannelJumpControl : System.Windows.Forms.UserControl, IUserControl
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
        ChannelJumpModel channelJumpModel  = new ChannelJumpModel();	// 채널점핑모델

        // 화면처리용 변수
        bool IsNewSearchKey		  = true;					// 검색어입력 여부
        CurrencyManager cm        = null;					// 데이터 그리드의 변경에 따른 데이터셋 관리를 위하여			
        DataTable       dt        = null;

        bool IsNotLoading		  = true;					// 상세조회중이 아님

        bool IsSearching = false; // 조회중 상세화면이 업데이트 되는 것을 방지 하기위함 2011.11.29 JH.Park
        bool IsAdding             = false;
        bool canRead			  = false;
        bool canUpdate			  = false;
        bool canCreate            = false;
        bool canDelete            = false;

		// 상세Key
		string keyItemNo        = "";
        string keyItemName      = "";
		string keyMediaCode     = "";
        string keyCatgoryCode   = "";
		string keyGenreCode     = "";
		string keyType          = "";
        private Janus.Windows.EditControls.UICheckBox chkAdState_10;
        private Janus.Windows.EditControls.UICheckBox chkAdState_40;
        private Janus.Windows.EditControls.UICheckBox chkAdState_30;
        private Janus.Windows.EditControls.UICheckBox chkAdState_20;
		private Janus.Windows.GridEX.EditControls.EditBox editBox6;
        private Janus.Windows.EditControls.UIButton btnAdd7;
        private Label label9;
        private Janus.Windows.GridEX.EditControls.EditBox ebMenu;
        private Janus.Windows.UI.Tab.UITabPage uiTabPage2;
        private Janus.Windows.GridEX.EditControls.EditBox ebOutLink;
        private Janus.Windows.GridEX.EditControls.EditBox editBox7;
        private Label label4;
		string keyPopID         = "";

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
        private Janus.Windows.UI.Dock.UIPanel uiPanelSearch;
        private Janus.Windows.UI.Dock.UIPanelInnerContainer uiPanelSearchContainer;
        private Janus.Windows.UI.Dock.UIPanel uiPanelList;
        private Janus.Windows.UI.Dock.UIPanelInnerContainer uiPanelListContainer;
        private Janus.Windows.UI.Dock.UIPanel uiPanelDetail;
        private Janus.Windows.UI.Dock.UIPanelInnerContainer uiPanelDetailContainer;
        private Janus.Windows.UI.Dock.UIPanelGroup uiPanelContract;
        private Janus.Windows.GridEX.EditControls.EditBox ebSearchKey;
        private System.Windows.Forms.Panel panel1;
        private Janus.Windows.GridEX.GridEX gridEX1;
        private Janus.Windows.EditControls.UIButton btnDelete;
        private Janus.Windows.EditControls.UIButton btnSave;
        private Janus.Windows.GridEX.GridEX grdExItemList;
        private Janus.Windows.EditControls.UIComboBox cbSearchMedia;
        private Janus.Windows.EditControls.UIComboBox cbSearchRap;
        private System.Data.DataView dvChannelJump;
        private System.Windows.Forms.Label lbContModDt;
        private System.Windows.Forms.Label lbContRegName;
        private System.Windows.Forms.Label lbContRegDt;
        private System.Windows.Forms.Label lbAgency;
        private System.Windows.Forms.Label lbContractState;
        private System.Windows.Forms.Label lbContStartDay;
        private System.Windows.Forms.Label lbContractName2;
        private System.Windows.Forms.Label lbContEndDay;
        private System.Windows.Forms.Label lbMedia;
        private System.Windows.Forms.Label lbRap;
        private System.Windows.Forms.Label lbComment;
        private System.Windows.Forms.Label label13;
		private Janus.Windows.EditControls.UIComboBox cbSearchAdType;
		private AdManagerClient.ChannelJumpDs channelJumpDs;
		private Janus.Windows.EditControls.UIComboBox cbSearchJumpType;
		private System.Windows.Forms.Label lbItemName;
		private Janus.Windows.GridEX.EditControls.EditBox ebItemName;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private Janus.Windows.GridEX.EditControls.EditBox ebGenreNameChannel;
		private Janus.Windows.GridEX.EditControls.EditBox ebChannelNo;
		private Janus.Windows.GridEX.EditControls.EditBox ebChannelName;
		private Janus.Windows.GridEX.EditControls.EditBox ebGenreNameContent;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label label6;
		private Janus.Windows.GridEX.EditControls.EditBox ebContentID;
		private System.Windows.Forms.Label label7;
        private Janus.Windows.GridEX.EditControls.EditBox ebContentName;
        private Janus.Windows.UI.Tab.UITabPage uiTabPage1;
        private System.Windows.Forms.Label label11;
        private Janus.Windows.GridEX.EditControls.EditBox ebPopupID;
        private System.Windows.Forms.Label label12;
        private Janus.Windows.GridEX.EditControls.EditBox ebPopupTitle;
        private System.Windows.Forms.Label label14;
        private Janus.Windows.EditControls.UIButton btnAdd1;
        private System.Windows.Forms.GroupBox gbAdd;
        private Janus.Windows.GridEX.EditControls.EditBox ebChannelManager;
        private Janus.Windows.UI.Tab.UITab uiTabJumpType;
        private Janus.Windows.GridEX.EditControls.EditBox editBox1;
        private Janus.Windows.GridEX.EditControls.EditBox editBox2;
        private Janus.Windows.GridEX.EditControls.EditBox editBox3;
        private Janus.Windows.GridEX.EditControls.EditBox editBox4;
        private Janus.Windows.GridEX.EditControls.EditBox editBox5;
        private Janus.Windows.GridEX.EditControls.EditBox ebChannelCID;
        private System.ComponentModel.IContainer components;

        public ChannelJumpControl()
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
            Janus.Windows.GridEX.GridEXLayout grdExItemList_DesignTimeLayout = new Janus.Windows.GridEX.GridEXLayout();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ChannelJumpControl));
            this.uiPM = new Janus.Windows.UI.Dock.UIPanelManager(this.components);
            this.uiPanelContract = new Janus.Windows.UI.Dock.UIPanelGroup();
            this.uiPanelSearch = new Janus.Windows.UI.Dock.UIPanel();
            this.uiPanelSearchContainer = new Janus.Windows.UI.Dock.UIPanelInnerContainer();
            this.pnlSearch = new System.Windows.Forms.Panel();
            this.chkAdState_40 = new Janus.Windows.EditControls.UICheckBox();
            this.chkAdState_30 = new Janus.Windows.EditControls.UICheckBox();
            this.chkAdState_20 = new Janus.Windows.EditControls.UICheckBox();
            this.chkAdState_10 = new Janus.Windows.EditControls.UICheckBox();
            this.label13 = new System.Windows.Forms.Label();
            this.cbSearchMedia = new Janus.Windows.EditControls.UIComboBox();
            this.cbSearchRap = new Janus.Windows.EditControls.UIComboBox();
            this.ebSearchKey = new Janus.Windows.GridEX.EditControls.EditBox();
            this.cbSearchAdType = new Janus.Windows.EditControls.UIComboBox();
            this.btnSearch = new Janus.Windows.EditControls.UIButton();
            this.cbSearchJumpType = new Janus.Windows.EditControls.UIComboBox();
            this.uiPanelList = new Janus.Windows.UI.Dock.UIPanel();
            this.uiPanelListContainer = new Janus.Windows.UI.Dock.UIPanelInnerContainer();
            this.grdExItemList = new Janus.Windows.GridEX.GridEX();
            this.dvChannelJump = new System.Data.DataView();
            this.channelJumpDs = new AdManagerClient.ChannelJumpDs();
            this.uiPanelDetail = new Janus.Windows.UI.Dock.UIPanel();
            this.uiPanelDetailContainer = new Janus.Windows.UI.Dock.UIPanelInnerContainer();
            this.pnlUserDetail = new System.Windows.Forms.Panel();
            this.gbAdd = new System.Windows.Forms.GroupBox();
            this.btnAdd7 = new Janus.Windows.EditControls.UIButton();
            this.btnAdd1 = new Janus.Windows.EditControls.UIButton();
            this.uiTabJumpType = new Janus.Windows.UI.Tab.UITab();
            this.uiTabPage1 = new Janus.Windows.UI.Tab.UITabPage();
            this.ebChannelCID = new Janus.Windows.GridEX.EditControls.EditBox();
            this.editBox2 = new Janus.Windows.GridEX.EditControls.EditBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.ebGenreNameChannel = new Janus.Windows.GridEX.EditControls.EditBox();
            this.ebChannelNo = new Janus.Windows.GridEX.EditControls.EditBox();
            this.ebChannelName = new Janus.Windows.GridEX.EditControls.EditBox();
            this.uiTabPage2 = new Janus.Windows.UI.Tab.UITabPage();
            this.ebOutLink = new Janus.Windows.GridEX.EditControls.EditBox();
            this.editBox7 = new Janus.Windows.GridEX.EditControls.EditBox();
            this.label4 = new System.Windows.Forms.Label();
            this.lbItemName = new System.Windows.Forms.Label();
            this.ebItemName = new Janus.Windows.GridEX.EditControls.EditBox();
            this.btnDelete = new Janus.Windows.EditControls.UIButton();
            this.btnSave = new Janus.Windows.EditControls.UIButton();
            this.editBox3 = new Janus.Windows.GridEX.EditControls.EditBox();
            this.ebGenreNameContent = new Janus.Windows.GridEX.EditControls.EditBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.ebContentID = new Janus.Windows.GridEX.EditControls.EditBox();
            this.label7 = new System.Windows.Forms.Label();
            this.ebContentName = new Janus.Windows.GridEX.EditControls.EditBox();
            this.editBox1 = new Janus.Windows.GridEX.EditControls.EditBox();
            this.editBox4 = new Janus.Windows.GridEX.EditControls.EditBox();
            this.label11 = new System.Windows.Forms.Label();
            this.ebPopupID = new Janus.Windows.GridEX.EditControls.EditBox();
            this.label12 = new System.Windows.Forms.Label();
            this.ebPopupTitle = new Janus.Windows.GridEX.EditControls.EditBox();
            this.editBox5 = new Janus.Windows.GridEX.EditControls.EditBox();
            this.label9 = new System.Windows.Forms.Label();
            this.ebMenu = new Janus.Windows.GridEX.EditControls.EditBox();
            this.label14 = new System.Windows.Forms.Label();
            this.ebChannelManager = new Janus.Windows.GridEX.EditControls.EditBox();
            this.editBox6 = new Janus.Windows.GridEX.EditControls.EditBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.gridEX1 = new Janus.Windows.GridEX.GridEX();
            this.lbContModDt = new System.Windows.Forms.Label();
            this.lbContRegName = new System.Windows.Forms.Label();
            this.lbContRegDt = new System.Windows.Forms.Label();
            this.lbAgency = new System.Windows.Forms.Label();
            this.lbContractState = new System.Windows.Forms.Label();
            this.lbContStartDay = new System.Windows.Forms.Label();
            this.lbContractName2 = new System.Windows.Forms.Label();
            this.lbContEndDay = new System.Windows.Forms.Label();
            this.lbMedia = new System.Windows.Forms.Label();
            this.lbRap = new System.Windows.Forms.Label();
            this.lbComment = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.uiPM)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelContract)).BeginInit();
            this.uiPanelContract.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelSearch)).BeginInit();
            this.uiPanelSearch.SuspendLayout();
            this.uiPanelSearchContainer.SuspendLayout();
            this.pnlSearch.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelList)).BeginInit();
            this.uiPanelList.SuspendLayout();
            this.uiPanelListContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdExItemList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dvChannelJump)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.channelJumpDs)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelDetail)).BeginInit();
            this.uiPanelDetail.SuspendLayout();
            this.uiPanelDetailContainer.SuspendLayout();
            this.pnlUserDetail.SuspendLayout();
            this.gbAdd.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uiTabJumpType)).BeginInit();
            this.uiTabJumpType.SuspendLayout();
            this.uiTabPage1.SuspendLayout();
            this.uiTabPage2.SuspendLayout();
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
            this.uiPanelContract.Id = new System.Guid("b162c53a-7940-47cd-a0ed-9762c59c50d8");
            this.uiPanelContract.StaticGroup = true;
            this.uiPanelSearch.Id = new System.Guid("afb3b7d0-aa79-443c-a1e8-3baba38f7a4d");
            this.uiPanelContract.Panels.Add(this.uiPanelSearch);
            this.uiPanelList.Id = new System.Guid("8e3140b9-bfcb-43af-b84e-2ee6cddf51dc");
            this.uiPanelContract.Panels.Add(this.uiPanelList);
            this.uiPanelDetail.Id = new System.Guid("b2973a57-d3e1-4d80-9387-7a6dc3332542");
            this.uiPanelContract.Panels.Add(this.uiPanelDetail);
            this.uiPM.Panels.Add(this.uiPanelContract);
            // 
            // Design Time Panel Info:
            // 
            this.uiPM.BeginPanelInfo();
            this.uiPM.AddDockPanelInfo(new System.Guid("b162c53a-7940-47cd-a0ed-9762c59c50d8"), Janus.Windows.UI.Dock.PanelGroupStyle.HorizontalTiles, Janus.Windows.UI.Dock.PanelDockStyle.Fill, true, new System.Drawing.Size(1010, 677), true);
            this.uiPM.AddDockPanelInfo(new System.Guid("afb3b7d0-aa79-443c-a1e8-3baba38f7a4d"), new System.Guid("b162c53a-7940-47cd-a0ed-9762c59c50d8"), 40, true);
            this.uiPM.AddDockPanelInfo(new System.Guid("8e3140b9-bfcb-43af-b84e-2ee6cddf51dc"), new System.Guid("b162c53a-7940-47cd-a0ed-9762c59c50d8"), 402, true);
            this.uiPM.AddDockPanelInfo(new System.Guid("b2973a57-d3e1-4d80-9387-7a6dc3332542"), new System.Guid("b162c53a-7940-47cd-a0ed-9762c59c50d8"), 205, true);
            this.uiPM.AddFloatingPanelInfo(new System.Guid("b162c53a-7940-47cd-a0ed-9762c59c50d8"), Janus.Windows.UI.Dock.PanelGroupStyle.HorizontalTiles, true, new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
            this.uiPM.AddFloatingPanelInfo(new System.Guid("8e3140b9-bfcb-43af-b84e-2ee6cddf51dc"), new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
            this.uiPM.AddFloatingPanelInfo(new System.Guid("b2973a57-d3e1-4d80-9387-7a6dc3332542"), new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
            this.uiPM.AddFloatingPanelInfo(new System.Guid("afb3b7d0-aa79-443c-a1e8-3baba38f7a4d"), new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
            this.uiPM.EndPanelInfo();
            // 
            // uiPanelContract
            // 
            this.uiPanelContract.ActiveCaptionMode = Janus.Windows.UI.Dock.ActiveCaptionMode.Never;
            this.uiPanelContract.AllowResize = Janus.Windows.UI.InheritableBoolean.False;
            this.uiPanelContract.BackColor = System.Drawing.SystemColors.Window;
            this.uiPanelContract.CaptionVisible = Janus.Windows.UI.InheritableBoolean.True;
            this.uiPanelContract.CloseButtonVisible = Janus.Windows.UI.InheritableBoolean.False;
            this.uiPanelContract.Location = new System.Drawing.Point(0, 0);
            this.uiPanelContract.Name = "uiPanelContract";
            this.uiPanelContract.Size = new System.Drawing.Size(1010, 677);
            this.uiPanelContract.TabIndex = 4;
            this.uiPanelContract.Text = "채널점핑관리";
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
            this.pnlSearch.Controls.Add(this.chkAdState_40);
            this.pnlSearch.Controls.Add(this.chkAdState_30);
            this.pnlSearch.Controls.Add(this.chkAdState_20);
            this.pnlSearch.Controls.Add(this.chkAdState_10);
            this.pnlSearch.Controls.Add(this.label13);
            this.pnlSearch.Controls.Add(this.cbSearchMedia);
            this.pnlSearch.Controls.Add(this.cbSearchRap);
            this.pnlSearch.Controls.Add(this.ebSearchKey);
            this.pnlSearch.Controls.Add(this.cbSearchAdType);
            this.pnlSearch.Controls.Add(this.btnSearch);
            this.pnlSearch.Controls.Add(this.cbSearchJumpType);
            this.pnlSearch.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlSearch.Location = new System.Drawing.Point(0, 0);
            this.pnlSearch.Name = "pnlSearch";
            this.pnlSearch.Size = new System.Drawing.Size(1008, 38);
            this.pnlSearch.TabIndex = 3;
            // 
            // chkAdState_40
            // 
            this.chkAdState_40.Location = new System.Drawing.Point(822, 8);
            this.chkAdState_40.Name = "chkAdState_40";
            this.chkAdState_40.Size = new System.Drawing.Size(49, 23);
            this.chkAdState_40.TabIndex = 30;
            this.chkAdState_40.Text = "종료";
            this.chkAdState_40.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            // 
            // chkAdState_30
            // 
            this.chkAdState_30.Location = new System.Drawing.Point(764, 8);
            this.chkAdState_30.Name = "chkAdState_30";
            this.chkAdState_30.Size = new System.Drawing.Size(49, 23);
            this.chkAdState_30.TabIndex = 30;
            this.chkAdState_30.Text = "중지";
            this.chkAdState_30.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            // 
            // chkAdState_20
            // 
            this.chkAdState_20.Checked = true;
            this.chkAdState_20.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkAdState_20.Location = new System.Drawing.Point(706, 8);
            this.chkAdState_20.Name = "chkAdState_20";
            this.chkAdState_20.Size = new System.Drawing.Size(49, 23);
            this.chkAdState_20.TabIndex = 30;
            this.chkAdState_20.Text = "편성";
            this.chkAdState_20.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            // 
            // chkAdState_10
            // 
            this.chkAdState_10.Checked = true;
            this.chkAdState_10.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkAdState_10.Location = new System.Drawing.Point(648, 8);
            this.chkAdState_10.Name = "chkAdState_10";
            this.chkAdState_10.Size = new System.Drawing.Size(49, 23);
            this.chkAdState_10.TabIndex = 30;
            this.chkAdState_10.Text = "대기";
            this.chkAdState_10.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            // 
            // label13
            // 
            this.label13.Location = new System.Drawing.Point(584, 10);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(58, 21);
            this.label13.TabIndex = 29;
            this.label13.Text = "광고상태";
            this.label13.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cbSearchMedia
            // 
            this.cbSearchMedia.BackColor = System.Drawing.Color.White;
            this.cbSearchMedia.ComboStyle = Janus.Windows.EditControls.ComboStyle.DropDownList;
            this.cbSearchMedia.Location = new System.Drawing.Point(919, 35);
            this.cbSearchMedia.Name = "cbSearchMedia";
            this.cbSearchMedia.Size = new System.Drawing.Size(46, 21);
            this.cbSearchMedia.TabIndex = 1;
            this.cbSearchMedia.Text = "매체선택";
            this.cbSearchMedia.Visible = false;
            this.cbSearchMedia.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            // 
            // cbSearchRap
            // 
            this.cbSearchRap.ComboStyle = Janus.Windows.EditControls.ComboStyle.DropDownList;
            this.cbSearchRap.Location = new System.Drawing.Point(6, 10);
            this.cbSearchRap.Name = "cbSearchRap";
            this.cbSearchRap.Size = new System.Drawing.Size(120, 21);
            this.cbSearchRap.TabIndex = 2;
            this.cbSearchRap.Text = "미디어렙선택";
            this.cbSearchRap.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            // 
            // ebSearchKey
            // 
            this.ebSearchKey.Location = new System.Drawing.Point(390, 10);
            this.ebSearchKey.Name = "ebSearchKey";
            this.ebSearchKey.Size = new System.Drawing.Size(170, 21);
            this.ebSearchKey.TabIndex = 5;
            this.ebSearchKey.Text = "검색어를 입력하세요";
            this.ebSearchKey.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
            this.ebSearchKey.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            this.ebSearchKey.TextChanged += new System.EventHandler(this.ebSearchKey_TextChanged);
            this.ebSearchKey.Click += new System.EventHandler(this.ebSearchKey_Click);
            this.ebSearchKey.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ebSearchKey_KeyDown);
            // 
            // cbSearchAdType
            // 
            this.cbSearchAdType.BackColor = System.Drawing.Color.White;
            this.cbSearchAdType.ComboStyle = Janus.Windows.EditControls.ComboStyle.DropDownList;
            this.cbSearchAdType.Location = new System.Drawing.Point(134, 10);
            this.cbSearchAdType.Name = "cbSearchAdType";
            this.cbSearchAdType.Size = new System.Drawing.Size(120, 21);
            this.cbSearchAdType.TabIndex = 3;
            this.cbSearchAdType.Text = "광고종류선택";
            this.cbSearchAdType.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            // 
            // btnSearch
            // 
            this.btnSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSearch.Enabled = false;
            this.btnSearch.Location = new System.Drawing.Point(895, 8);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(104, 24);
            this.btnSearch.TabIndex = 10;
            this.btnSearch.Text = "조 회";
            this.btnSearch.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // cbSearchJumpType
            // 
            this.cbSearchJumpType.BackColor = System.Drawing.Color.White;
            this.cbSearchJumpType.ComboStyle = Janus.Windows.EditControls.ComboStyle.DropDownList;
            this.cbSearchJumpType.Location = new System.Drawing.Point(262, 10);
            this.cbSearchJumpType.Name = "cbSearchJumpType";
            this.cbSearchJumpType.Size = new System.Drawing.Size(120, 21);
            this.cbSearchJumpType.TabIndex = 4;
            this.cbSearchJumpType.Text = "점핑구분선택";
            this.cbSearchJumpType.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            // 
            // uiPanelList
            // 
            this.uiPanelList.BackColor = System.Drawing.SystemColors.Window;
            this.uiPanelList.CaptionStyle = Janus.Windows.UI.Dock.PanelCaptionStyle.Dark;
            this.uiPanelList.CaptionVisible = Janus.Windows.UI.InheritableBoolean.False;
            this.uiPanelList.InnerContainer = this.uiPanelListContainer;
            this.uiPanelList.Location = new System.Drawing.Point(0, 66);
            this.uiPanelList.MinimumSize = new System.Drawing.Size(-1, 100);
            this.uiPanelList.Name = "uiPanelList";
            this.uiPanelList.Size = new System.Drawing.Size(1010, 402);
            this.uiPanelList.TabIndex = 13;
            this.uiPanelList.TabStop = false;
            this.uiPanelList.Text = "점핑채널 광고목록";
            // 
            // uiPanelListContainer
            // 
            this.uiPanelListContainer.BackColor = System.Drawing.SystemColors.Window;
            this.uiPanelListContainer.Controls.Add(this.grdExItemList);
            this.uiPanelListContainer.Location = new System.Drawing.Point(1, 1);
            this.uiPanelListContainer.Name = "uiPanelListContainer";
            this.uiPanelListContainer.Size = new System.Drawing.Size(1008, 400);
            this.uiPanelListContainer.TabIndex = 0;
            // 
            // grdExItemList
            // 
            this.grdExItemList.AllowEdit = Janus.Windows.GridEX.InheritableBoolean.False;
            this.grdExItemList.AlternatingColors = true;
            this.grdExItemList.BorderStyle = Janus.Windows.GridEX.BorderStyle.None;
            this.grdExItemList.DataSource = this.dvChannelJump;
            grdExItemList_DesignTimeLayout.LayoutString = resources.GetString("grdExItemList_DesignTimeLayout.LayoutString");
            this.grdExItemList.DesignTimeLayout = grdExItemList_DesignTimeLayout;
            this.grdExItemList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdExItemList.EditorsControlStyle.ButtonAppearance = Janus.Windows.GridEX.ButtonAppearance.Regular;
            this.grdExItemList.EmptyRows = true;
            this.grdExItemList.FocusCellFormatStyle.BackColor = System.Drawing.SystemColors.Highlight;
            this.grdExItemList.FocusCellFormatStyle.ForeColor = System.Drawing.Color.White;
            this.grdExItemList.FocusStyle = Janus.Windows.GridEX.FocusStyle.None;
            this.grdExItemList.FrozenColumns = 4;
            this.grdExItemList.GridLineColor = System.Drawing.Color.Silver;
            this.grdExItemList.GridLines = Janus.Windows.GridEX.GridLines.Vertical;
            this.grdExItemList.GridLineStyle = Janus.Windows.GridEX.GridLineStyle.Solid;
            this.grdExItemList.GroupByBoxVisible = false;
            this.grdExItemList.HideSelection = Janus.Windows.GridEX.HideSelection.HighlightInactive;
            this.grdExItemList.Location = new System.Drawing.Point(0, 0);
            this.grdExItemList.Name = "grdExItemList";
            this.grdExItemList.ScrollBars = Janus.Windows.GridEX.ScrollBars.Vertical;
            this.grdExItemList.SelectedInactiveFormatStyle.BackColor = System.Drawing.Color.Gold;
            this.grdExItemList.Size = new System.Drawing.Size(1008, 400);
            this.grdExItemList.TabIndex = 11;
            this.grdExItemList.TabKeyBehavior = Janus.Windows.GridEX.TabKeyBehavior.ControlNavigation;
            this.grdExItemList.ThemedAreas = ((Janus.Windows.GridEX.ThemedArea)(((((((((Janus.Windows.GridEX.ThemedArea.ScrollBars | Janus.Windows.GridEX.ThemedArea.EditControls)
                        | Janus.Windows.GridEX.ThemedArea.Headers)
                        | Janus.Windows.GridEX.ThemedArea.GroupByBox)
                        | Janus.Windows.GridEX.ThemedArea.GroupRows)
                        | Janus.Windows.GridEX.ThemedArea.ControlBorder)
                        | Janus.Windows.GridEX.ThemedArea.Cards)
                        | Janus.Windows.GridEX.ThemedArea.Gridlines)
                        | Janus.Windows.GridEX.ThemedArea.CheckBoxes)));
            this.grdExItemList.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            this.grdExItemList.LoadingRow += new Janus.Windows.GridEX.RowLoadEventHandler(this.grdExItemList_LoadingRow);
            this.grdExItemList.SelectionChanged += new System.EventHandler(this.OnGrdRowChanged);
            this.grdExItemList.Enter += new System.EventHandler(this.OnGrdRowChanged);
            // 
            // dvChannelJump
            // 
            this.dvChannelJump.Table = this.channelJumpDs.ChannelJump;
            // 
            // channelJumpDs
            // 
            this.channelJumpDs.DataSetName = "ChannelJumpDs";
            this.channelJumpDs.Locale = new System.Globalization.CultureInfo("en-US");
            this.channelJumpDs.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // uiPanelDetail
            // 
            this.uiPanelDetail.AllowResize = Janus.Windows.UI.InheritableBoolean.False;
            this.uiPanelDetail.BackColor = System.Drawing.SystemColors.Window;
            this.uiPanelDetail.CaptionStyle = Janus.Windows.UI.Dock.PanelCaptionStyle.Dark;
            this.uiPanelDetail.InnerContainer = this.uiPanelDetailContainer;
            this.uiPanelDetail.Location = new System.Drawing.Point(0, 472);
            this.uiPanelDetail.Name = "uiPanelDetail";
            this.uiPanelDetail.Size = new System.Drawing.Size(1010, 205);
            this.uiPanelDetail.TabIndex = 15;
            this.uiPanelDetail.TabStop = false;
            this.uiPanelDetail.Text = "상세정보";
            // 
            // uiPanelDetailContainer
            // 
            this.uiPanelDetailContainer.Controls.Add(this.pnlUserDetail);
            this.uiPanelDetailContainer.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.uiPanelDetailContainer.Location = new System.Drawing.Point(1, 23);
            this.uiPanelDetailContainer.Name = "uiPanelDetailContainer";
            this.uiPanelDetailContainer.Size = new System.Drawing.Size(1008, 181);
            this.uiPanelDetailContainer.TabIndex = 0;
            // 
            // pnlUserDetail
            // 
            this.pnlUserDetail.BackColor = System.Drawing.SystemColors.Window;
            this.pnlUserDetail.Controls.Add(this.gbAdd);
            this.pnlUserDetail.Controls.Add(this.uiTabJumpType);
            this.pnlUserDetail.Controls.Add(this.lbItemName);
            this.pnlUserDetail.Controls.Add(this.ebItemName);
            this.pnlUserDetail.Controls.Add(this.btnDelete);
            this.pnlUserDetail.Controls.Add(this.btnSave);
            this.pnlUserDetail.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlUserDetail.Location = new System.Drawing.Point(0, 0);
            this.pnlUserDetail.Name = "pnlUserDetail";
            this.pnlUserDetail.Size = new System.Drawing.Size(1008, 181);
            this.pnlUserDetail.TabIndex = 12;
            // 
            // gbAdd
            // 
            this.gbAdd.Controls.Add(this.btnAdd7);
            this.gbAdd.Controls.Add(this.btnAdd1);
            this.gbAdd.Location = new System.Drawing.Point(814, 4);
            this.gbAdd.Name = "gbAdd";
            this.gbAdd.Size = new System.Drawing.Size(185, 170);
            this.gbAdd.TabIndex = 116;
            this.gbAdd.TabStop = false;
            this.gbAdd.Text = "신규입력";
            // 
            // btnAdd7
            // 
            this.btnAdd7.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnAdd7.ButtonStyle = Janus.Windows.EditControls.ButtonStyle.Button;
            this.btnAdd7.Location = new System.Drawing.Point(96, 20);
            this.btnAdd7.Name = "btnAdd7";
            this.btnAdd7.Size = new System.Drawing.Size(83, 20);
            this.btnAdd7.TabIndex = 116;
            this.btnAdd7.Text = "OutLink";
            this.btnAdd7.VisualStyle = Janus.Windows.UI.VisualStyle.Office2003;
            this.btnAdd7.Click += new System.EventHandler(this.btnAdd2_Click);
            // 
            // btnAdd1
            // 
            this.btnAdd1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnAdd1.ButtonStyle = Janus.Windows.EditControls.ButtonStyle.Button;
            this.btnAdd1.Location = new System.Drawing.Point(8, 20);
            this.btnAdd1.Name = "btnAdd1";
            this.btnAdd1.Size = new System.Drawing.Size(83, 20);
            this.btnAdd1.TabIndex = 31;
            this.btnAdd1.Text = "시놉바로가기";
            this.btnAdd1.VisualStyle = Janus.Windows.UI.VisualStyle.Office2003;
            this.btnAdd1.Click += new System.EventHandler(this.btnAdd1_Click);
            // 
            // uiTabJumpType
            // 
            this.uiTabJumpType.Location = new System.Drawing.Point(14, 37);
            this.uiTabJumpType.Name = "uiTabJumpType";
            this.uiTabJumpType.ShowFocusRectangle = false;
            this.uiTabJumpType.Size = new System.Drawing.Size(794, 133);
            this.uiTabJumpType.TabIndex = 110;
            this.uiTabJumpType.TabPages.AddRange(new Janus.Windows.UI.Tab.UITabPage[] {
            this.uiTabPage1,
            this.uiTabPage2});
            this.uiTabJumpType.TabsStateStyles.SelectedFormatStyle.FontBold = Janus.Windows.UI.TriState.True;
            this.uiTabJumpType.TextOrientation = Janus.Windows.UI.Tab.TextOrientation.Horizontal;
            this.uiTabJumpType.VisualStyle = Janus.Windows.UI.Tab.TabVisualStyle.Office2007;
            // 
            // uiTabPage1
            // 
            this.uiTabPage1.Controls.Add(this.ebChannelCID);
            this.uiTabPage1.Controls.Add(this.editBox2);
            this.uiTabPage1.Controls.Add(this.label1);
            this.uiTabPage1.Controls.Add(this.label2);
            this.uiTabPage1.Controls.Add(this.label3);
            this.uiTabPage1.Controls.Add(this.ebGenreNameChannel);
            this.uiTabPage1.Controls.Add(this.ebChannelNo);
            this.uiTabPage1.Controls.Add(this.ebChannelName);
            this.uiTabPage1.Location = new System.Drawing.Point(1, 22);
            this.uiTabPage1.Name = "uiTabPage1";
            this.uiTabPage1.Size = new System.Drawing.Size(792, 110);
            this.uiTabPage1.TabStop = true;
            this.uiTabPage1.Text = "시놉바로가기";
            // 
            // ebChannelCID
            // 
            this.ebChannelCID.Location = new System.Drawing.Point(148, 34);
            this.ebChannelCID.MaxLength = 38;
            this.ebChannelCID.Name = "ebChannelCID";
            this.ebChannelCID.ReadOnly = true;
            this.ebChannelCID.Size = new System.Drawing.Size(238, 21);
            this.ebChannelCID.TabIndex = 104;
            this.ebChannelCID.TabStop = false;
            this.ebChannelCID.Text = "컨텐츠ID";
            this.ebChannelCID.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
            this.ebChannelCID.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            // 
            // editBox2
            // 
            this.editBox2.BackColor = System.Drawing.Color.Gainsboro;
            this.editBox2.ButtonFont = new System.Drawing.Font("나눔고딕", 8.999999F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.editBox2.Font = new System.Drawing.Font("나눔고딕", 8.999999F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.editBox2.Location = new System.Drawing.Point(402, 12);
            this.editBox2.Multiline = true;
            this.editBox2.Name = "editBox2";
            this.editBox2.ReadOnly = true;
            this.editBox2.Size = new System.Drawing.Size(385, 67);
            this.editBox2.TabIndex = 103;
            this.editBox2.Text = "\r\n선택하신 채널의 시놉화면으로\r\n이동시키는 기능을 제공합니다";
            this.editBox2.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Location = new System.Drawing.Point(18, 34);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(48, 20);
            this.label1.TabIndex = 102;
            this.label1.Text = "채널";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Location = new System.Drawing.Point(18, 56);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(48, 20);
            this.label2.TabIndex = 102;
            this.label2.Text = "제목";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label3
            // 
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Location = new System.Drawing.Point(18, 12);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(48, 20);
            this.label3.TabIndex = 102;
            this.label3.Text = "장르";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ebGenreNameChannel
            // 
            this.ebGenreNameChannel.ButtonStyle = Janus.Windows.GridEX.EditControls.EditButtonStyle.TextButton;
            this.ebGenreNameChannel.ButtonText = "찾기";
            this.ebGenreNameChannel.Location = new System.Drawing.Point(82, 12);
            this.ebGenreNameChannel.MaxLength = 50;
            this.ebGenreNameChannel.Name = "ebGenreNameChannel";
            this.ebGenreNameChannel.ReadOnly = true;
            this.ebGenreNameChannel.Size = new System.Drawing.Size(304, 21);
            this.ebGenreNameChannel.TabIndex = 15;
            this.ebGenreNameChannel.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
            this.ebGenreNameChannel.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            this.ebGenreNameChannel.ButtonClick += new System.EventHandler(this.ebGenreNameChannel_ButtonClick);
            // 
            // ebChannelNo
            // 
            this.ebChannelNo.Location = new System.Drawing.Point(82, 34);
            this.ebChannelNo.MaxLength = 50;
            this.ebChannelNo.Name = "ebChannelNo";
            this.ebChannelNo.ReadOnly = true;
            this.ebChannelNo.Size = new System.Drawing.Size(64, 21);
            this.ebChannelNo.TabIndex = 17;
            this.ebChannelNo.TabStop = false;
            this.ebChannelNo.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
            this.ebChannelNo.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            // 
            // ebChannelName
            // 
            this.ebChannelName.Location = new System.Drawing.Point(82, 56);
            this.ebChannelName.MaxLength = 50;
            this.ebChannelName.Name = "ebChannelName";
            this.ebChannelName.ReadOnly = true;
            this.ebChannelName.Size = new System.Drawing.Size(304, 21);
            this.ebChannelName.TabIndex = 18;
            this.ebChannelName.TabStop = false;
            this.ebChannelName.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
            this.ebChannelName.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            // 
            // uiTabPage2
            // 
            this.uiTabPage2.Controls.Add(this.ebOutLink);
            this.uiTabPage2.Controls.Add(this.editBox7);
            this.uiTabPage2.Controls.Add(this.label4);
            this.uiTabPage2.Location = new System.Drawing.Point(1, 22);
            this.uiTabPage2.Name = "uiTabPage2";
            this.uiTabPage2.Size = new System.Drawing.Size(792, 110);
            this.uiTabPage2.TabStop = true;
            this.uiTabPage2.Text = "OutLink";
            // 
            // ebOutLink
            // 
            this.ebOutLink.Location = new System.Drawing.Point(82, 12);
            this.ebOutLink.MaxLength = 300;
            this.ebOutLink.Name = "ebOutLink";
            this.ebOutLink.Size = new System.Drawing.Size(304, 21);
            this.ebOutLink.TabIndex = 117;
            this.ebOutLink.TabStop = false;
            this.ebOutLink.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
            this.ebOutLink.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            // 
            // editBox7
            // 
            this.editBox7.BackColor = System.Drawing.Color.Gainsboro;
            this.editBox7.ButtonFont = new System.Drawing.Font("나눔고딕", 8.999999F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.editBox7.Font = new System.Drawing.Font("나눔고딕", 8.999999F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.editBox7.Location = new System.Drawing.Point(402, 12);
            this.editBox7.Multiline = true;
            this.editBox7.Name = "editBox7";
            this.editBox7.ReadOnly = true;
            this.editBox7.Size = new System.Drawing.Size(385, 67);
            this.editBox7.TabIndex = 119;
            this.editBox7.Text = "\r\n입력하신 URL 페이지로 \r\n이동시키는 기능을 제공합니다";
            this.editBox7.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
            // 
            // label4
            // 
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.Location = new System.Drawing.Point(18, 12);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(48, 20);
            this.label4.TabIndex = 118;
            this.label4.Text = "URL";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lbItemName
            // 
            this.lbItemName.BackColor = System.Drawing.Color.Transparent;
            this.lbItemName.Location = new System.Drawing.Point(14, 8);
            this.lbItemName.Name = "lbItemName";
            this.lbItemName.Size = new System.Drawing.Size(72, 21);
            this.lbItemName.TabIndex = 102;
            this.lbItemName.Text = "광고명";
            this.lbItemName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ebItemName
            // 
            this.ebItemName.ButtonStyle = Janus.Windows.GridEX.EditControls.EditButtonStyle.TextButton;
            this.ebItemName.ButtonText = "찾기";
            this.ebItemName.Location = new System.Drawing.Point(88, 8);
            this.ebItemName.MaxLength = 50;
            this.ebItemName.Name = "ebItemName";
            this.ebItemName.ReadOnly = true;
            this.ebItemName.Size = new System.Drawing.Size(304, 21);
            this.ebItemName.TabIndex = 12;
            this.ebItemName.Text = "광고명";
            this.ebItemName.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
            this.ebItemName.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            this.ebItemName.ButtonClick += new System.EventHandler(this.ebItemName_ButtonClick);
            // 
            // btnDelete
            // 
            this.btnDelete.Enabled = false;
            this.btnDelete.Location = new System.Drawing.Point(728, 7);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(80, 22);
            this.btnDelete.TabIndex = 30;
            this.btnDelete.Text = "삭 제";
            this.btnDelete.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnSave
            // 
            this.btnSave.Enabled = false;
            this.btnSave.Location = new System.Drawing.Point(640, 7);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(80, 22);
            this.btnSave.TabIndex = 29;
            this.btnSave.Text = "저 장";
            this.btnSave.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // editBox3
            // 
            this.editBox3.BackColor = System.Drawing.Color.Gainsboro;
            this.editBox3.Font = new System.Drawing.Font("나눔고딕", 8.999999F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.editBox3.Location = new System.Drawing.Point(432, 12);
            this.editBox3.Multiline = true;
            this.editBox3.Name = "editBox3";
            this.editBox3.ReadOnly = true;
            this.editBox3.Size = new System.Drawing.Size(355, 67);
            this.editBox3.TabIndex = 104;
            this.editBox3.Text = "\r\n선택하신 컨텐츠로\r\n이동시키는 기능을 제공합니다";
            this.editBox3.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
            // 
            // ebGenreNameContent
            // 
            this.ebGenreNameContent.ButtonStyle = Janus.Windows.GridEX.EditControls.EditButtonStyle.TextButton;
            this.ebGenreNameContent.ButtonText = "찾기";
            this.ebGenreNameContent.Location = new System.Drawing.Point(82, 12);
            this.ebGenreNameContent.MaxLength = 50;
            this.ebGenreNameContent.Name = "ebGenreNameContent";
            this.ebGenreNameContent.ReadOnly = true;
            this.ebGenreNameContent.Size = new System.Drawing.Size(344, 21);
            this.ebGenreNameContent.TabIndex = 19;
            this.ebGenreNameContent.Text = "장르";
            this.ebGenreNameContent.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
            this.ebGenreNameContent.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            this.ebGenreNameContent.ButtonClick += new System.EventHandler(this.ebGenreNameContent_ButtonClick);
            // 
            // label5
            // 
            this.label5.BackColor = System.Drawing.Color.Transparent;
            this.label5.Location = new System.Drawing.Point(18, 12);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(60, 20);
            this.label5.TabIndex = 102;
            this.label5.Text = "장르";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label6
            // 
            this.label6.BackColor = System.Drawing.Color.Transparent;
            this.label6.Location = new System.Drawing.Point(18, 34);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(60, 20);
            this.label6.TabIndex = 102;
            this.label6.Text = "컨텐츠ID";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ebContentID
            // 
            this.ebContentID.Location = new System.Drawing.Point(82, 34);
            this.ebContentID.MaxLength = 50;
            this.ebContentID.Name = "ebContentID";
            this.ebContentID.ReadOnly = true;
            this.ebContentID.Size = new System.Drawing.Size(232, 21);
            this.ebContentID.TabIndex = 21;
            this.ebContentID.TabStop = false;
            this.ebContentID.Text = "컨텐츠ID";
            this.ebContentID.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
            this.ebContentID.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            // 
            // label7
            // 
            this.label7.BackColor = System.Drawing.Color.Transparent;
            this.label7.Location = new System.Drawing.Point(18, 56);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(60, 20);
            this.label7.TabIndex = 102;
            this.label7.Text = "컨텐츠명";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ebContentName
            // 
            this.ebContentName.Location = new System.Drawing.Point(82, 56);
            this.ebContentName.MaxLength = 50;
            this.ebContentName.Name = "ebContentName";
            this.ebContentName.ReadOnly = true;
            this.ebContentName.Size = new System.Drawing.Size(344, 21);
            this.ebContentName.TabIndex = 22;
            this.ebContentName.TabStop = false;
            this.ebContentName.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
            this.ebContentName.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            // 
            // editBox1
            // 
            this.editBox1.BackColor = System.Drawing.Color.Gainsboro;
            this.editBox1.Font = new System.Drawing.Font("나눔고딕", 8.999999F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.editBox1.Location = new System.Drawing.Point(46, 12);
            this.editBox1.Multiline = true;
            this.editBox1.Name = "editBox1";
            this.editBox1.ReadOnly = true;
            this.editBox1.Size = new System.Drawing.Size(741, 62);
            this.editBox1.TabIndex = 0;
            this.editBox1.Text = "상업광고인 경우엔 자동연동되기 때문에 채널점핑을 설정하지 않으셔도 됩니다\r\n기타광고에 AD팝업을 설정하실 경우엔 해당광고를 팝업공지 형으로 저장하" +
                "신후\r\n팝업시스템에서 등록하여 사용하십시요";
            this.editBox1.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
            // 
            // editBox4
            // 
            this.editBox4.BackColor = System.Drawing.Color.Gainsboro;
            this.editBox4.Font = new System.Drawing.Font("나눔고딕", 8.999999F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.editBox4.Location = new System.Drawing.Point(334, 12);
            this.editBox4.Multiline = true;
            this.editBox4.Name = "editBox4";
            this.editBox4.ReadOnly = true;
            this.editBox4.Size = new System.Drawing.Size(453, 62);
            this.editBox4.TabIndex = 104;
            this.editBox4.Text = "멀티채널점핑 기능입니다. 팝업연동버튼을 클릭하여 \r\n팝업시스템 에 해당팝업을 입력하시면  연동이 완료 됩니다\r\n연동없이 작업하시려면, 저장하신후," +
                " \r\n팝업시스템에서 따로 입력하셔야 합니다";
            this.editBox4.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
            // 
            // label11
            // 
            this.label11.BackColor = System.Drawing.Color.Transparent;
            this.label11.Location = new System.Drawing.Point(18, 12);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(54, 21);
            this.label11.TabIndex = 102;
            this.label11.Text = "공지ID";
            this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ebPopupID
            // 
            this.ebPopupID.ButtonStyle = Janus.Windows.GridEX.EditControls.EditButtonStyle.TextButton;
            this.ebPopupID.ButtonText = "팝업연동";
            this.ebPopupID.Location = new System.Drawing.Point(82, 12);
            this.ebPopupID.MaxLength = 50;
            this.ebPopupID.Name = "ebPopupID";
            this.ebPopupID.ReadOnly = true;
            this.ebPopupID.Size = new System.Drawing.Size(192, 21);
            this.ebPopupID.TabIndex = 23;
            this.ebPopupID.TabStop = false;
            this.ebPopupID.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
            this.ebPopupID.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            this.ebPopupID.ButtonClick += new System.EventHandler(this.ebPopupID_ButtonClick);
            // 
            // label12
            // 
            this.label12.BackColor = System.Drawing.Color.Transparent;
            this.label12.Location = new System.Drawing.Point(18, 36);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(54, 21);
            this.label12.TabIndex = 102;
            this.label12.Text = "공지제목";
            this.label12.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ebPopupTitle
            // 
            this.ebPopupTitle.Location = new System.Drawing.Point(82, 36);
            this.ebPopupTitle.MaxLength = 50;
            this.ebPopupTitle.Multiline = true;
            this.ebPopupTitle.Name = "ebPopupTitle";
            this.ebPopupTitle.Size = new System.Drawing.Size(248, 38);
            this.ebPopupTitle.TabIndex = 25;
            this.ebPopupTitle.TabStop = false;
            this.ebPopupTitle.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
            this.ebPopupTitle.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            // 
            // editBox5
            // 
            this.editBox5.BackColor = System.Drawing.Color.Gainsboro;
            this.editBox5.Location = new System.Drawing.Point(486, 12);
            this.editBox5.Multiline = true;
            this.editBox5.Name = "editBox5";
            this.editBox5.ReadOnly = true;
            this.editBox5.Size = new System.Drawing.Size(301, 64);
            this.editBox5.TabIndex = 105;
            this.editBox5.TabStop = false;
            this.editBox5.Text = resources.GetString("editBox5.Text");
            this.editBox5.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
            // 
            // label9
            // 
            this.label9.BackColor = System.Drawing.Color.Transparent;
            this.label9.Location = new System.Drawing.Point(18, 13);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(53, 20);
            this.label9.TabIndex = 104;
            this.label9.Text = "메뉴코드";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ebMenu
            // 
            this.ebMenu.ButtonText = "찾기";
            this.ebMenu.Location = new System.Drawing.Point(82, 13);
            this.ebMenu.MaxLength = 43;
            this.ebMenu.Name = "ebMenu";
            this.ebMenu.Size = new System.Drawing.Size(394, 21);
            this.ebMenu.TabIndex = 51;
            this.ebMenu.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
            this.ebMenu.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            this.ebMenu.ButtonClick += new System.EventHandler(this.ebMenu_ButtonClick);
            // 
            // label14
            // 
            this.label14.BackColor = System.Drawing.Color.Transparent;
            this.label14.Location = new System.Drawing.Point(13, 12);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(63, 20);
            this.label14.TabIndex = 106;
            this.label14.Text = "이동정보";
            this.label14.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ebChannelManager
            // 
            this.ebChannelManager.Location = new System.Drawing.Point(82, 12);
            this.ebChannelManager.MaxLength = 120;
            this.ebChannelManager.Multiline = true;
            this.ebChannelManager.Name = "ebChannelManager";
            this.ebChannelManager.Size = new System.Drawing.Size(694, 36);
            this.ebChannelManager.TabIndex = 105;
            this.ebChannelManager.TabStop = false;
            this.ebChannelManager.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
            this.ebChannelManager.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            // 
            // editBox6
            // 
            this.editBox6.BackColor = System.Drawing.Color.Gainsboro;
            this.editBox6.Font = new System.Drawing.Font("나눔고딕", 8.999999F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.editBox6.Location = new System.Drawing.Point(29, 15);
            this.editBox6.Multiline = true;
            this.editBox6.Name = "editBox6";
            this.editBox6.ReadOnly = true;
            this.editBox6.Size = new System.Drawing.Size(750, 62);
            this.editBox6.TabIndex = 1;
            this.editBox6.Text = "\r\nEvent팝업은 AD팝업과 동일한 팝업을 사용합니다.\r\n차이점은 AD팝업은 팝업에서 등록한 트리거를 사용하는 것이고\r\nEvent팝업은 셋탑에서" +
                " 자체적으로 가지는 트리거를 사용하는 것입니다.";
            this.editBox6.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
            this.editBox6.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
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
            // lbContModDt
            // 
            this.lbContModDt.BackColor = System.Drawing.Color.Transparent;
            this.lbContModDt.Font = new System.Drawing.Font("굴림체", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lbContModDt.Location = new System.Drawing.Point(632, 34);
            this.lbContModDt.Name = "lbContModDt";
            this.lbContModDt.Size = new System.Drawing.Size(56, 21);
            this.lbContModDt.TabIndex = 117;
            this.lbContModDt.Text = "최종수정";
            // 
            // lbContRegName
            // 
            this.lbContRegName.BackColor = System.Drawing.Color.Transparent;
            this.lbContRegName.Font = new System.Drawing.Font("굴림체", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lbContRegName.Location = new System.Drawing.Point(632, 59);
            this.lbContRegName.Name = "lbContRegName";
            this.lbContRegName.Size = new System.Drawing.Size(48, 14);
            this.lbContRegName.TabIndex = 118;
            this.lbContRegName.Text = "등록자";
            // 
            // lbContRegDt
            // 
            this.lbContRegDt.BackColor = System.Drawing.Color.Transparent;
            this.lbContRegDt.Font = new System.Drawing.Font("굴림체", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lbContRegDt.Location = new System.Drawing.Point(632, 10);
            this.lbContRegDt.Name = "lbContRegDt";
            this.lbContRegDt.Size = new System.Drawing.Size(56, 21);
            this.lbContRegDt.TabIndex = 116;
            this.lbContRegDt.Text = "등록일시";
            // 
            // lbAgency
            // 
            this.lbAgency.BackColor = System.Drawing.Color.Transparent;
            this.lbAgency.Font = new System.Drawing.Font("굴림체", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lbAgency.Location = new System.Drawing.Point(8, 82);
            this.lbAgency.Name = "lbAgency";
            this.lbAgency.Size = new System.Drawing.Size(56, 21);
            this.lbAgency.TabIndex = 18;
            this.lbAgency.Text = "대행사";
            // 
            // lbContractState
            // 
            this.lbContractState.BackColor = System.Drawing.Color.Transparent;
            this.lbContractState.Font = new System.Drawing.Font("굴림체", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lbContractState.Location = new System.Drawing.Point(424, 10);
            this.lbContractState.Name = "lbContractState";
            this.lbContractState.Size = new System.Drawing.Size(56, 21);
            this.lbContractState.TabIndex = 18;
            this.lbContractState.Text = "내역상태";
            // 
            // lbContStartDay
            // 
            this.lbContStartDay.BackColor = System.Drawing.Color.Transparent;
            this.lbContStartDay.Font = new System.Drawing.Font("굴림체", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lbContStartDay.Location = new System.Drawing.Point(216, 34);
            this.lbContStartDay.Name = "lbContStartDay";
            this.lbContStartDay.Size = new System.Drawing.Size(72, 21);
            this.lbContStartDay.TabIndex = 46;
            this.lbContStartDay.Text = "내역시작일";
            // 
            // lbContractName2
            // 
            this.lbContractName2.BackColor = System.Drawing.Color.Transparent;
            this.lbContractName2.Font = new System.Drawing.Font("굴림체", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lbContractName2.Location = new System.Drawing.Point(8, 10);
            this.lbContractName2.Name = "lbContractName2";
            this.lbContractName2.Size = new System.Drawing.Size(48, 21);
            this.lbContractName2.TabIndex = 18;
            this.lbContractName2.Text = "내역명";
            // 
            // lbContEndDay
            // 
            this.lbContEndDay.BackColor = System.Drawing.Color.Transparent;
            this.lbContEndDay.Font = new System.Drawing.Font("굴림체", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lbContEndDay.Location = new System.Drawing.Point(424, 34);
            this.lbContEndDay.Name = "lbContEndDay";
            this.lbContEndDay.Size = new System.Drawing.Size(72, 21);
            this.lbContEndDay.TabIndex = 46;
            this.lbContEndDay.Text = "내역종료일";
            // 
            // lbMedia
            // 
            this.lbMedia.BackColor = System.Drawing.Color.Transparent;
            this.lbMedia.Font = new System.Drawing.Font("굴림체", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lbMedia.Location = new System.Drawing.Point(8, 34);
            this.lbMedia.Name = "lbMedia";
            this.lbMedia.Size = new System.Drawing.Size(56, 21);
            this.lbMedia.TabIndex = 18;
            this.lbMedia.Text = "매체";
            // 
            // lbRap
            // 
            this.lbRap.BackColor = System.Drawing.Color.Transparent;
            this.lbRap.Font = new System.Drawing.Font("굴림체", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lbRap.Location = new System.Drawing.Point(8, 58);
            this.lbRap.Name = "lbRap";
            this.lbRap.Size = new System.Drawing.Size(56, 21);
            this.lbRap.TabIndex = 18;
            this.lbRap.Text = "랩사";
            // 
            // lbComment
            // 
            this.lbComment.BackColor = System.Drawing.Color.Transparent;
            this.lbComment.Font = new System.Drawing.Font("굴림체", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lbComment.Location = new System.Drawing.Point(216, 58);
            this.lbComment.Name = "lbComment";
            this.lbComment.Size = new System.Drawing.Size(72, 21);
            this.lbComment.TabIndex = 46;
            this.lbComment.Text = "비고";
            // 
            // ChannelJumpControl
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.uiPanelContract);
            this.Font = global::AdManagerClient.Properties.Settings.Default.Font1;
            this.Name = "ChannelJumpControl";
            this.Size = new System.Drawing.Size(1010, 677);
            this.Load += new System.EventHandler(this.UserControl_Load);
            ((System.ComponentModel.ISupportInitialize)(this.uiPM)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelContract)).EndInit();
            this.uiPanelContract.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelSearch)).EndInit();
            this.uiPanelSearch.ResumeLayout(false);
            this.uiPanelSearchContainer.ResumeLayout(false);
            this.pnlSearch.ResumeLayout(false);
            this.pnlSearch.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelList)).EndInit();
            this.uiPanelList.ResumeLayout(false);
            this.uiPanelListContainer.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdExItemList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dvChannelJump)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.channelJumpDs)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelDetail)).EndInit();
            this.uiPanelDetail.ResumeLayout(false);
            this.uiPanelDetailContainer.ResumeLayout(false);
            this.pnlUserDetail.ResumeLayout(false);
            this.pnlUserDetail.PerformLayout();
            this.gbAdd.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.uiTabJumpType)).EndInit();
            this.uiTabJumpType.ResumeLayout(false);
            this.uiTabPage1.ResumeLayout(false);
            this.uiTabPage1.PerformLayout();
            this.uiTabPage2.ResumeLayout(false);
            this.uiTabPage2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridEX1)).EndInit();
            this.ResumeLayout(false);

        }
        #endregion

        #region 컨트롤 로드
        private void UserControl_Load(object sender, System.EventArgs e)
        {
            // 데이터관리용 객체생성
            dt = ((DataView)grdExItemList.DataSource).Table;  
            cm = (CurrencyManager) this.BindingContext[grdExItemList.DataSource]; 
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

            if(menu.CanRead(MenuCode))      canRead = true;
            if(menu.CanCreate(MenuCode))    canCreate = true;
            if(menu.CanDelete(MenuCode))    canDelete = true;
            if(menu.CanUpdate(MenuCode))
            {
                ResetTextReadonly();
                canUpdate = true;
            }
            else
            {
                SetTextReadonly();
            }

			ProgressStop();

			if(canRead) SearchChannelJump();
			InitButton();
		}

        private void InitCombo()
        {
            Init_MediaCode();
            Init_RapCode();
            Init_AdType();
            Init_JumpType();
			Init_STBList();
            InitCombo_Level();
            
        }

        private void InitCombo_Level()
        {
            if(commonModel.UserLevel == "20")
            {
                // 콤보픽스						
                cbSearchMedia.SelectedValue = commonModel.MediaCode;			
                cbSearchMedia.ReadOnly = true;				            
            }
            else
            {
				for(int i=0;i < channelJumpDs.Media.Rows.Count;i++)
				{
					DataRow row = channelJumpDs.Media.Rows[i];					
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
            if (commonModel.UserLevel == "30")
            {
                cbSearchRap.SelectedValue = commonModel.RapCode;
                cbSearchRap.ReadOnly = true;
            }
 
            Application.DoEvents();
        }

        private void Init_MediaCode()
        {
            // 매체를 조회한다.
            MediaCodeModel mediaCodeModel = new MediaCodeModel();
            new MediaCodeManager(systemModel, commonModel).GetMediaCodeList(mediaCodeModel);
			
            if (mediaCodeModel.ResultCD.Equals("0000"))
            {
                // 데이터셋에 셋팅
                Utility.SetDataTable(channelJumpDs.Media, mediaCodeModel.MediaCodeDataSet);				
            }

            // 검색조건의 콤보
            this.cbSearchMedia.Items.Clear();
			
            // 콤보박스에 셋트할 코드목록을 담을 Item배열을 선언
            Janus.Windows.EditControls.UIComboBoxItem[] comboItems = new Janus.Windows.EditControls.UIComboBoxItem[mediaCodeModel.ResultCnt + 1];

            comboItems[0] = new Janus.Windows.EditControls.UIComboBoxItem("매체선택","00");
			
            for(int i=0;i<mediaCodeModel.ResultCnt;i++)
            {
                DataRow row = channelJumpDs.Media.Rows[i];

                string val = row["MediaCode"].ToString();
                string txt = row["MediaName"].ToString();
                comboItems[i+1] = new Janus.Windows.EditControls.UIComboBoxItem(txt,val);
            }
            // 검색 콤보에 셋트
            this.cbSearchMedia.Items.AddRange(comboItems);
            this.cbSearchMedia.SelectedIndex = 0;

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
                Utility.SetDataTable(channelJumpDs.MediaRap, mediaRapCodeModel.MediaRapCodeDataSet);				
            }

            // 검색조건의 콤보
            this.cbSearchRap.Items.Clear();
			
            // 콤보박스에 셋트할 코드목록을 담을 Item배열을 선언
            Janus.Windows.EditControls.UIComboBoxItem[] comboItems = new Janus.Windows.EditControls.UIComboBoxItem[mediaRapCodeModel.ResultCnt + 1];

            comboItems[0] = new Janus.Windows.EditControls.UIComboBoxItem("미디어렙선택","00");
			
            for(int i=0;i<mediaRapCodeModel.ResultCnt;i++)
            {
                DataRow row = channelJumpDs.MediaRap.Rows[i];

                string val = row["RapCode"].ToString();
                string txt = row["RapName"].ToString();
                comboItems[i+1] = new Janus.Windows.EditControls.UIComboBoxItem(txt,val);
            }
            // 콤보에 셋트
            this.cbSearchRap.Items.AddRange(comboItems);
            this.cbSearchRap.SelectedIndex = 0;

            Application.DoEvents();
        }
  
        private void Init_AdType()
        {
			// 코드에서 내역상태를 조회한다.
			CodeModel codeModel = new CodeModel();
			codeModel.Section = "26";				// '26':광고종류  TODO: 코드분류는 추후 XML로 관리되어야...
			new CodeManager(systemModel, commonModel).GetCodeList(codeModel);
			
			if (codeModel.ResultCD.Equals("0000"))
			{
				// 데이터셋에 셋팅
				Utility.SetDataTable(channelJumpDs.AdType, codeModel.CodeDataSet);				
			}
 
            // 검색조건의 콤보
            this.cbSearchAdType.Items.Clear();
			
            // 콤보박스에 셋트할 코드목록을 담을 Item배열을 선언
            Janus.Windows.EditControls.UIComboBoxItem[] comboItems = new Janus.Windows.EditControls.UIComboBoxItem[codeModel.ResultCnt + 1];

            comboItems[0] = new Janus.Windows.EditControls.UIComboBoxItem("광고종류","00");
			
            for(int i=0;i<codeModel.ResultCnt;i++)
            {
                DataRow row = channelJumpDs.AdType.Rows[i];

                string val = row["Code"].ToString();
                string txt = row["CodeName"].ToString();
                comboItems[i+1] = new Janus.Windows.EditControls.UIComboBoxItem(txt,val);
            }
            // 콤보에 셋트
            this.cbSearchAdType.Items.AddRange(comboItems);
            this.cbSearchAdType.SelectedIndex = 0;

            Application.DoEvents();
        }

        private void Init_JumpType()
        {
            // 코드에서 내역상태를 조회한다.
            CodeModel codeModel = new CodeModel();
            codeModel.Section = "34";				// 점핑종류 '34'  TODO: 코드분류는 추후 XML로 관리되어야...
            new CodeManager(systemModel, commonModel).GetCodeList(codeModel);
			
            if (codeModel.ResultCD.Equals("0000"))
            {
                // 데이터셋에 셋팅
                Utility.SetDataTable(channelJumpDs.JumpType, codeModel.CodeDataSet);				
            }

			// 검색조건의 콤보
			this.cbSearchJumpType.Items.Clear();
			
			// 콤보박스에 셋트할 코드목록을 담을 Item배열을 선언
			Janus.Windows.EditControls.UIComboBoxItem[] comboItems = new Janus.Windows.EditControls.UIComboBoxItem[codeModel.ResultCnt + 1];

			comboItems[0] = new Janus.Windows.EditControls.UIComboBoxItem("점핑구분","00");
			
			for(int i=0;i<codeModel.ResultCnt;i++)
			{
				DataRow row = channelJumpDs.JumpType.Rows[i];

				string val = row["Code"].ToString();
				string txt = row["CodeName"].ToString();
				comboItems[i+1] = new Janus.Windows.EditControls.UIComboBoxItem(txt,val);
			}
			// 콤보에 셋트
			this.cbSearchJumpType.Items.AddRange(comboItems);
			this.cbSearchJumpType.SelectedIndex = 0;

            Application.DoEvents();
        }

		private Janus.Windows.EditControls.UICheckBox[] mCheckBox;

		private void Init_STBList()
		{
			// 코드에서 내역상태를 조회한다.
			CodeModel codeModel = new CodeModel();
			codeModel.Section = "38";				// 셋탑모델 '34'  TODO: 코드분류는 추후 XML로 관리되어야...
			new CodeManager(systemModel, commonModel).GetCodeList(codeModel);

			if (codeModel.ResultCD.Equals("0000"))
			{
				// 데이터셋에 셋팅
				Utility.SetDataTable(channelJumpDs.STBList, codeModel.CodeDataSet);
			}

			if (channelJumpDs.STBList.Rows.Count > 0)
			{ 
				mCheckBox = new Janus.Windows.EditControls.UICheckBox[channelJumpDs.STBList.Rows.Count];
				for (int i = 0; i < channelJumpDs.STBList.Rows.Count; i++)
				{
					Janus.Windows.EditControls.UICheckBox checkBox = new Janus.Windows.EditControls.UICheckBox();

					checkBox.BackColor = System.Drawing.Color.Transparent;
					checkBox.Checked = false;
					checkBox.CheckedValue = "";
					//checkBox.CheckState = System.Windows.Forms.CheckState.Checked;
					checkBox.ForeColor = System.Drawing.SystemColors.ControlText;
					if (i <= 3)
					{
						checkBox.Location = new System.Drawing.Point(14 + (120 * i), 10);
					}
					else if( i <= 7)
					{
						checkBox.Location = new System.Drawing.Point(14 + (120 * (i-4)), 40);
					}
					else
					{
						checkBox.Location = new System.Drawing.Point(14 + (120 * (i-8)), 70);
					}

					checkBox.Name = "chkSTB" + i.ToString();
					checkBox.Size = new System.Drawing.Size(100, 21);
					checkBox.Text = channelJumpDs.STBList.Rows[i][channelJumpDs.STBList.CodeNameColumn].ToString();
					checkBox.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;

					mCheckBox[i] = checkBox;
					Application.DoEvents();
				}
			}
		}

		private void InitButton()
		{
			if(canRead)   btnSearch.Enabled = true;

            if(canCreate)
            {
                gbAdd.Enabled =true;
            }
			Application.DoEvents();
		}

        private void DisableButton()
        {
            btnSearch.Enabled = false;
            gbAdd.Enabled    = false;
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
                if (IsNotLoading)
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
            ResetDetailText();
            SearchChannelJump();
            InitButton();
			ProgressStop();
        }

        /// <summary>
        /// 저장버튼 클릭
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_Click(object sender, System.EventArgs e)
        {

			{
				channelJumpModel.StbTypeYn = "N";
				channelJumpModel.StbTypeString = "";
			}

			if( channelJumpModel.StbTypeYn.Equals("Y") || channelJumpModel.StbTypeYn.Equals("N") )
				SaveChannelJump();
        }

        /// <summary>
        /// 삭제버튼 클릭
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDelete_Click(object sender, System.EventArgs e)
        {
            DeleteChannelJump();
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
                SearchChannelJump();
            }
        }



			
//		private void btnSearchPopup_Click(object sender, System.EventArgs e)
//		{
//			// 팝업공지 검색 팝업 띠우기
//			ChannelJump_SearchAdPopForm pForm = new ChannelJump_SearchAdPopForm(this);
//
//			pForm.keyType = keyType;
//			pForm.ShowDialog();
//   
//			pForm.Dispose();
//			pForm = null;							
//		}

        /// <summary>
        /// 입력버튼을 클릭한후, 입력준비를 위해 컨트롤들을 초기화 하는 부분
        /// </summary>
        /// <param name="jumpType"></param>
        private void InitInsert(int jumpType)
        {
            ResetDetailText();
            gbAdd.Enabled = false;
            btnDelete.Enabled = false;
            if (canCreate) btnSave.Enabled = true;

            keyType = jumpType.ToString();
            IsAdding = true;

            uiTabPage1.Enabled = false;
            uiTabPage2.Enabled = false;

            if (jumpType == 1)
            {
                #region 1. 채널점핑
                /*
                 * 채널점핑 입력
                 * 기본적으로 홈에서 주로 사용된다
                 * 탭페이지 설정
                 * */
                uiPanelDetail.Text = "시놉바로가기 입력";

                uiTabJumpType.SelectedIndex = 0;
                uiTabPage1.Enabled = true;

                ebGenreNameChannel.Text = "";
                ebGenreNameChannel.ButtonEnabled = true;
                ebChannelNo.Text = "";
                ebChannelName.Text = "";
                ebChannelCID.Text = "";
                #endregion
            }
            else if (jumpType == 2)
            {
                #region 2. 프리컨텐츠
                /*
                 * 프리컨텐츠 입력
                 * 기본적으로 홈에서 주로 사용된다
                 * 탭페이지 설정
                 * */
                uiPanelDetail.Text = "OutLink 입력";

                uiTabJumpType.SelectedIndex = 1;
                uiTabPage2.Enabled = true;

                ebGenreNameContent.Text = "찾기버튼클릭!!!";
                ebGenreNameContent.ButtonEnabled = true;
                ebContentID.Text = "";
                ebContentName.Text = "";
                #endregion
            }
        }
        #endregion

        #region 처리메소드

        /// <summary>
        /// 채널점핑목록 조회
        /// </summary>
        private void SearchChannelJump()
        {
            IsSearching = true;
            StatusMessage("채널점핑 정보를 조회합니다.");
            try
            {
                //검색 전에 모델을 초기화 해준다.
                channelJumpModel.Init();
                // 데이터모델에 전송할 내용을 셋트한다.
                channelJumpModel.SearchMediaCode = cbSearchMedia.SelectedValue.ToString();
                channelJumpModel.SearchRapCode   = cbSearchRap.SelectedValue.ToString();
				channelJumpModel.SearchAdType    = cbSearchAdType.SelectedValue.ToString();
                channelJumpModel.SearchJumpType = cbSearchJumpType.SelectedValue.ToString();

				if(chkAdState_10.Checked)   channelJumpModel.SearchchkAdState_10   = "Y";
                if(chkAdState_20.Checked)   channelJumpModel.SearchchkAdState_20   = "Y";
                if(chkAdState_30.Checked)   channelJumpModel.SearchchkAdState_30   = "Y";
                if(chkAdState_40.Checked)   channelJumpModel.SearchchkAdState_40   = "Y";

                if(IsNewSearchKey)
                {
                    channelJumpModel.SearchKey = "";
                }
                else
                {
                    channelJumpModel.SearchKey  = ebSearchKey.Text;
                }

                // 광고 내역 목록 서비스를 호출한다.
                new ChannelJumpManager(systemModel,commonModel).GetChannelJumpList(channelJumpModel);

                if (channelJumpModel.ResultCD.Equals("0000"))
                {
					Utility.SetDataTable(channelJumpDs.ChannelJump, channelJumpModel.ChannelJumpDataSet);			
                    StatusMessage(channelJumpModel.ResultCnt + "건의 내역정보 정보가 조회되었습니다.");

					keyMediaCode = cbSearchMedia.SelectedValue.ToString();

                    if(canUpdate)
                    {
                        AddSchChoice();									
                    }										
                    SetDetailText();
                }
            }
            catch(FrameException fe)
            {
                FrameSystem.showMsgForm("채널점핑 목록조회 오류", new string[] {fe.ErrCode, fe.ResultMsg});
            }
            catch(Exception ex)
            {
                FrameSystem.showMsgForm("채널점핑 목록조회 오류",new string[] {"",ex.Message});
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
                if ( channelJumpDs.Tables["ChannelJump"].Rows.Count < 1 ) return;
              
                foreach (DataRow row in channelJumpDs.Tables["ChannelJump"].Rows)
                {					
                    if(IsAdding)
                    {
                        cm.Position = 0;
                        keyItemNo = null;									
                    }
                    else
                    {						
                        if(row["ItemNo"].ToString().Equals(keyItemNo))
                        {					
                            cm.Position = rowIndex;
                            break;								
                        }
                    }

                    rowIndex++;
                    grdExItemList.EnsureVisible();
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


        #region [ F:채널점핑 정보 입력및 수정 작업 ]
        /// <summary>
        /// 채널점핑 정보 저장
        /// </summary>
        private void SaveChannelJump()
        {
            StatusMessage("채널점핑 정보를 저장합니다.");
            
            #region 선행작업
            if(ebItemName.Text.Trim().Length == 0) 
            {
				MessageBox.Show("광고가 선택되지 않았습니다.","채널점핑 저장", MessageBoxButtons.OK, MessageBoxIcon.Information );
				ebItemName.Focus();
				return;	               
            }
            #endregion
			
            try
            {
                #region 모델설정
                // 데이터모델에 전송할 내용을 셋트한다.
                //계약명 선택 
				channelJumpModel.MediaCode      = keyMediaCode;     
				channelJumpModel.ItemNo         = keyItemNo;
				channelJumpModel.ItemName       = ebItemName.Text; 

                if( keyType.Equals("1") )
                {
                    #region [ 시놉바로가기 타입 ]
                    if(ebChannelNo.Text.Trim().Length == 0) 
                    {
                        MessageBox.Show("컨텐츠가 선택되지 않았습니다.","시놉바로가기 저장", MessageBoxButtons.OK, MessageBoxIcon.Information );
                        ebItemName.Focus();
                        return;	               
                    }

                    channelJumpModel.JumpType = "1"; // 시놉바로가기
                    channelJumpModel.GenreCode  = keyGenreCode;
                    channelJumpModel.ChannelNo  = ebChannelNo.Text;
                    channelJumpModel.ContentID  = ebChannelCID.Text.Trim();
                    channelJumpModel.PopupID    = "";
                    channelJumpModel.PopupTitle = "";
                    channelJumpModel.ChannelManager = "";
                    #endregion
                }
                else if( keyType.Equals("2") )
                {
                    #region [ 프리컨텐츠 타입 ]
                    if (ebOutLink.Text.Trim().Length == 0) 
                    {
                        MessageBox.Show("URL주소가 입력되지 않았습니다.","OutLink 저장", MessageBoxButtons.OK, MessageBoxIcon.Information );
                        ebOutLink.Focus();
                        return;	               
                    }

                    channelJumpModel.JumpType = "2"; // OutLink
                    channelJumpModel.GenreCode  = "";
                    channelJumpModel.ChannelNo  = "";
                    channelJumpModel.ContentID  = ebOutLink.Text;
                    channelJumpModel.PopupID    = "";
                    channelJumpModel.PopupTitle = "";
                    channelJumpModel.ChannelManager = "";
                    #endregion
                }
                else
                {
                    MessageBox.Show("바로가기 타입이 설정되어 있지 않습니다","채널점핑 저장", MessageBoxButtons.OK, MessageBoxIcon.Information );
                    return;	               
                }
           
                #endregion

                #region 서비스 호출
				// 계약정보 상세정보 저장 서비스를 호출한다.
                if (IsAdding)
                {
                    new ChannelJumpManager(systemModel,commonModel).SetChannelJumpCreate(channelJumpModel);
                    StatusMessage("점핑광고정보 정보가 추가되었습니다.");
                }
                else
                {   
                    new ChannelJumpManager(systemModel,commonModel).SetChannelJumpUpdate(channelJumpModel);
                    StatusMessage("점핑광고정보 정보가 저장되었습니다.");
                }
                #endregion

                #region 후행작업
                DisableButton();
                SearchChannelJump();
                InitButton();
                        
				IsAdding = false;
                #endregion
			}
            catch(FrameException fe)
            {
                FrameSystem.showMsgForm("채널점핑 저장오류", new string[] {fe.ErrCode, fe.ResultMsg});
            }
            catch(Exception ex)
            {
                FrameSystem.showMsgForm("채널점핑 저장오류",new string[] {"",ex.Message});
            }		
        }
        #endregion

        /// <summary>
        /// 채널점핑 삭제
        /// </summary>
        private void DeleteChannelJump()
        {
            StatusMessage("채널점핑 정보를 삭제합니다.");
            if(keyItemNo.Equals("")) 
            {
				MessageBox.Show("컨텐츠가 선택되지 않았습니다.","채널점핑 저장", 	MessageBoxButtons.OK, MessageBoxIcon.Information );
				return;
            }

            DialogResult result = MessageBox.Show("해당 채널점핑 정보를 삭제 하시겠습니까?","채널점핑 삭제",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question,
                MessageBoxDefaultButton.Button2);

            if (result == DialogResult.No) return;

            try 
            {
                // 데이터모델에 전송할 내용을 셋트한다.
				channelJumpModel.MediaCode      = keyMediaCode;     
				channelJumpModel.ItemNo         = keyItemNo;
				channelJumpModel.ItemName       = ebItemName.Text; 

                // 채널점핑 저장 서비스를 호출한다.
                new ChannelJumpManager(systemModel,commonModel).SetChannelJumpDelete(channelJumpModel);
                StatusMessage("채널점핑 정보가 삭제되었습니다.");			

                ResetDetailText();
                DisableButton();
                SearchChannelJump();
                InitButton();

            }
            catch(FrameException fe)
            {
                FrameSystem.showMsgForm("채널점핑 삭제오류", new string[] {fe.ErrCode, fe.ResultMsg});
            }
            catch(Exception ex)
            {
                FrameSystem.showMsgForm("채널점핑 삭제오류",new string[] {"",ex.Message});
            }	
			
        }
		
        /// <summary>
        /// 채널점핑 상세정보의 셋트
        /// </summary>
        private void SetDetailText()
        {
            int curRow = cm.Position;

            if(curRow >= 0 )
            {	
				ResetDetailText();

                // Key셋트
                keyItemNo       = dt.Rows[curRow]["ItemNo"].ToString();
                keyItemName     = dt.Rows[curRow]["ItemName"].ToString();
				keyGenreCode    = dt.Rows[curRow]["GenreCode"].ToString();
                keyType         = dt.Rows[curRow]["JumpType"].ToString();

                // 상세 데이터 설정
                ebItemName.Text = dt.Rows[curRow]["ItemName"].ToString();
				uiTabPage1.Enabled = false;
				uiTabPage2.Enabled = false;


				if(keyType.Equals("1"))	// 채널점핑
				{
                    uiTabJumpType.SelectedIndex = 0;
                    uiTabPage1.Enabled  = true;

					ebGenreNameChannel.Text = dt.Rows[curRow]["GenreName"].ToString();
					ebChannelNo.Text        = dt.Rows[curRow]["ChannelNo"].ToString();
                    ebChannelCID.Text       = dt.Rows[curRow]["ContentID"].ToString();
					ebChannelName.Text      = dt.Rows[curRow]["ContentName"].ToString();
                    
                    if(canUpdate)   ebGenreNameChannel.ButtonEnabled = true;
                    else            ebGenreNameChannel.ButtonEnabled = false;
				}
				else if(keyType.Equals("2"))	// OutLink
				{
                    uiTabJumpType.SelectedIndex = 1;
                    uiTabPage2.Enabled  = true;

                    ebOutLink.Text = dt.Rows[curRow]["ContentID"].ToString();

					if(canUpdate)   ebGenreNameContent.ButtonEnabled = true;
                    else            ebGenreNameContent.ButtonEnabled = false;
				}

				
				if(canUpdate) btnSave.Enabled    = true;
				if(canDelete) btnDelete.Enabled  = true;

				Application.DoEvents();
            }
            StatusMessage("준비");
        }

        /// <summary>
        /// 신규입력후 해당광고번호로 데이터를 읽어온다
        /// 연동후 처리부분
        /// </summary>
        private void ReLoadDetailText()
        {
            try
            {
                channelJumpModel.Init();
                channelJumpModel.ItemNo     = keyItemNo;
                new ChannelJumpManager(systemModel,commonModel).GetChannelJump(channelJumpModel);

                if (channelJumpModel.ResultCD.Equals("0000"))
                {
                    ebPopupID.Text = channelJumpModel.PopupID;
                    IsAdding = false;
                    //lblMessage.Text = "팝업시스템과의 연동작업이 완료되었습니다.\n노출대상을 선택하신 후 저장하십시요";
                }
                else
                {
                    FrameSystem.showMsgForm("채널점핑 연동 오류", new string[] {channelJumpModel.ResultCD, channelJumpModel.ResultDesc});
                }
            }
            catch(FrameException fe)
            {
                FrameSystem.showMsgForm("채널점핑 목록조회 오류", new string[] {fe.ErrCode, fe.ResultMsg});
            }
            catch(Exception ex)
            {
                FrameSystem.showMsgForm("채널점핑 목록조회 오류",new string[] {"",ex.Message});
            }

        }

        private void ResetDetailText()
        {
			IsAdding = false;

            ebItemName.Text = "";

            // 시놉바로가기
			ebGenreNameChannel.Text = "";
			ebChannelNo.Text        = "";
			ebChannelName.Text      = "";

            // OutLink
            ebOutLink.Text = "";


        }
		

        /// <summary>
        /// 상세정보 ReadOnly
        /// </summary>
        private void SetTextReadonly()
        {

        }

        /// <summary>
        /// 상세정보 수정가능케
        /// </summary>
        private void ResetTextReadonly()
        {
  
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
			
		public void SetContractItem(string ItemNo, string ItemName)
		{
			keyItemNo       = ItemNo;
            keyItemName     = ItemName;
			ebItemName.Text = ItemName;					
		}

		public void SetChannel(string GenreCode, string GenreName, string ChannelNo, string ChannelName)
		{
			keyGenreCode = GenreCode;
			ebGenreNameChannel.Text = GenreName;
			ebChannelNo.Text        = ChannelNo;
			ebChannelName.Text      = ChannelName;
		}

		public void SetContent(string GenreCode, string GenreName, string ContentID, string ContentName)
		{
			keyGenreCode = GenreCode;
			ebGenreNameContent.Text = GenreName;
			ebContentID.Text        = ContentID;
			ebContentName.Text      = ContentName;
		}

		public void SetPopup(string PopID, string Title)
		{
			ebPopupID.Text = PopID;
			ebPopupTitle.Text = Title;					
		}

		#endregion

        private void grdExItemList_LoadingRow(object sender, Janus.Windows.GridEX.RowLoadEventArgs e)
        {
            try
            {
                e.Row.Cells["ColSeq"].Value =e.Row.Position + 1;
            }
            catch(Exception ex) 
            {
                Debug.WriteLine("Grid_LoadingRow:" + ex.Message);
            }
        }

        #region 팝업윈도우 오픈 버튼 처리
        
        /// <summary>
        /// 채널점핑 광고팝업 띄우기
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ebItemName_ButtonClick(object sender, System.EventArgs e)
        {
            // 광고 검색 팝업 띠우기
            ChannelJump_SearchItemForm pForm = new ChannelJump_SearchItemForm(this);

            pForm.keyMediaCode = keyMediaCode;
            pForm.ShowDialog();            

            pForm.Dispose();
            pForm = null;		

            if( keyType.Equals("1") )       ebGenreNameChannel.Focus();
            else if( keyType.Equals("2") )  ebGenreNameContent.Focus();
        }


        /// <summary>
        /// 채널점핑 장르찾기
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ebGenreNameChannel_ButtonClick(object sender, System.EventArgs e)
        {
            Common.ContentsForm pForm = new AdManagerClient.Common.ContentsForm();
            pForm.SelectContents += new AdManagerClient.Common.ContentsEventHandler(pForm_SelectContents);

            // 채널 검색 팝업 띠우기
            //ChannelJump_SearchChannelForm pForm = new ChannelJump_SearchChannelForm(this);

            //pForm.keyMediaCode = keyMediaCode;
            if( pForm.ShowDialog() == DialogResult.No )
            {
                keyCatgoryCode  = "";
                keyGenreCode = "";
                ebMenu.Text = "";
            }
            
            pForm.Dispose();
            pForm = null;
        }


        /// <summary>
        /// 컨텐츠찾기
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ebGenreNameContent_ButtonClick(object sender, System.EventArgs e)
        {
            ChannelJump_SearchContentForm pForm = new ChannelJump_SearchContentForm(this);

            pForm.keyMediaCode = keyMediaCode;
            pForm.ShowDialog();
   
            pForm.Dispose();
            pForm = null;					
        }


        /// <summary>
        /// 팝업시스템 연동
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ebPopupID_ButtonClick(object sender, System.EventArgs e)
        {
            return;
        }
	
        /*  //2015-05-08 [NEXT_UI]기존에 쓰던 5번타입 장르점핑은 않쓰고 메뉴점핑으로 변경됨
        /// <summary>
        /// 장르점핑 장르찾기
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ebGenre_ButtonClick(object sender, System.EventArgs e)
        {
            Common.CategoryGenreForm pForm = new AdManagerClient.Common.CategoryGenreForm();
            pForm.SelectCategoryGenre += new AdManagerClient.Common.CategoryGenreEventHandler(pForm_SelectCategoryGenre);
            pForm.ShowDialog(this);

            pForm.Dispose();
            pForm = null;        
        }
        */
        /// <summary>
        /// 메뉴점핑 메뉴찾기
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ebMenu_ButtonClick(object sender, EventArgs e)
        {
            //메뉴찾기용 데이터를 받아서 Form으로 띄워 보여줘야함
            Common.CategoryGenreForm pForm = new AdManagerClient.Common.CategoryGenreForm();
            pForm.SelectCategoryGenre += new AdManagerClient.Common.CategoryGenreEventHandler(pForm_SelectCategoryGenre);
            pForm.ShowDialog(this);

            pForm.Dispose();
            pForm = null;   
        }

        private void pForm_SelectCategoryGenre(object sender, AdManagerClient.Common.CategoryGenreEventArgs e)
        {
            keyCatgoryCode  = e.Category.ToString();
            keyGenreCode    = e.Genre.ToString();
            ebMenu.Text = e.CategoryName.ToString() + "||" + e.GenreName.ToString();
        }

        private void pForm_SelectContents(object sender, AdManagerClient.Common.ContentsEventArgs e)
        {
            keyCatgoryCode  = e.CategoryCode.ToString();
            keyGenreCode    = e.GenreCode.ToString();
                        
            ebGenreNameChannel.Text = e.CategoryName.ToString() + "||" + e.GenreName.ToString();
            ebChannelNo.Text        = e.ChannelNo.ToString();
            ebChannelCID.Text       = e.ContentId.ToString();    
            ebChannelName.Text      = e.Title.ToString() + "||" + e.SubTitle.ToString();
        }
        #endregion

        #region 입력버튼 클릭이벤트 처리
        /// <summary>
        /// 채널점핑타입 입력버튼 처리
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAdd1_Click(object sender, System.EventArgs e)
        {
            InitInsert(1);
            ebItemName.Text = "";
            ebItemName.ButtonEnabled = true;
            ebItemName.Focus();
        }

        private void btnAdd2_Click(object sender, EventArgs e)
        {
            InitInsert(2);
            ebItemName.Text = "";
            ebItemName.ButtonEnabled = true;
            ebItemName.Focus();
        }

        #endregion




    }
}
