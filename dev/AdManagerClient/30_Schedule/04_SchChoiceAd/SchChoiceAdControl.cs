// ===============================================================================
// SchChoiceAdControl for Charites Project
//
// SchChoiceAdControl.cs
//
// 지정광고편성 컨트롤을 정의합니다. 
//
// ===============================================================================
// Release history
//
// ===============================================================================
// Copyright (C) 2006 G-Inno Systems Inc.
// All rights reserved.
// 
/*
 * [수정사항]
 * -------------------------------------------------------
 * 수정코드  : [E_01]
 * 수정자    : YJ.Park
 * 수정일    : 2014.08.8
 * 수정내용  :        
 *            - 편성복사 버튼 추가
 * --------------------------------------------------------
 */

using System;
using System.Data;
using System.Diagnostics;
using System.Windows.Forms;

using WinFramework.Misc;
using WinFramework.Base;

using AdManagerModel;
using AdManagerClient.Common.Args;

namespace AdManagerClient
{
	/// <summary>
	/// 광고파일관리 컨트롤
	/// </summary>
    public class SchChoiceAdControl : System.Windows.Forms.UserControl, IUserControl
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
		SchChoiceAdModel schChoiceAdModel  = new SchChoiceAdModel();	// 지정광고편성모델
		SchPublishModel schPublishModel  = new SchPublishModel();	// 광고승인모델

		// 화면처리용 변수
		bool IsNewSearchKey		  = true;					// 검색어입력 여부
		CurrencyManager cm        = null;					// 데이터 그리드의 변경에 따른 데이터셋 관리를 위하여			
		DataTable       dt        = null;
		DataTable       dtMenu    = null;
		DataTable       dtChannel = null;

        CurrencyManager cmMenu    = null;                   //데이터 그리드 변경에따른 데이터셋 관리
        CurrencyManager cmChannel = null;

        bool IsSearching = false; // 조회중 상세화면이 업데이트 되는 것을 방지 하기위함 2011.11.29 JH.Park
		bool canRead			  = false;
		bool canCreate            = false;
		bool canDelete            = false;

		// Key 데이터
		bool IsNotLoading		  = true;					// 상세조회중이 아님
		string keyMediaCode    = "";
		public string keyItemNo       = "";
		string keyAdType       = "";

		public string ItemNo
		{
			get { return keyItemNo;	}
			set { keyItemNo = value;	}
		}

		public string MediaCode
		{
			get { return keyMediaCode;	}
			set { keyMediaCode = value;	}
		}
		
		public string AdType
		{
			get { return keyAdType;	}
			set { keyAdType = value;	}
		}


		// 편성배포 승인상태 처리용
		private string keyAckNoCm    = "";
		private string keyAckNoOap   = "";
		
		private string keyAckStateCm = "";
        private Janus.Windows.EditControls.UICheckBox chkAdState_20;
        private Janus.Windows.EditControls.UICheckBox chkAdState_40;
        private Janus.Windows.EditControls.UICheckBox chkAdState_30;
        private Janus.Windows.EditControls.UIButton btnCopy;
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
		private System.Windows.Forms.Panel pnlSearch;
		private System.Windows.Forms.Panel pnlSearchBtn;
		private Janus.Windows.EditControls.UIButton btnSearch;
		private Janus.Windows.UI.Dock.UIPanel uiPanelSearch;
		private Janus.Windows.UI.Dock.UIPanelInnerContainer uiPanelSearchContainer;
		private Janus.Windows.UI.Dock.UIPanel uiPanelList;
		private Janus.Windows.UI.Dock.UIPanelInnerContainer uiPanelListContainer;
		private Janus.Windows.UI.Dock.UIPanel uiPanelDetail;
		private Janus.Windows.UI.Dock.UIPanelInnerContainer uiPanelDetailContainer;
		private Janus.Windows.GridEX.EditControls.EditBox ebSearchKey;
		private System.Data.DataView dvSchedule;
        private Janus.Windows.UI.Dock.UIPanelGroup uiPanelChoiceAdSchedule;
		private System.Windows.Forms.Label lbAdState;
		private Janus.Windows.EditControls.UIComboBox cbSearchRap;
		private Janus.Windows.GridEX.GridEX grdExScheduleList;
        private Janus.Windows.EditControls.UIComboBox cbSearchAdClass;
		private Janus.Windows.EditControls.UIButton btnAdd;
		private Janus.Windows.EditControls.UIButton btnDelete;
		private System.Windows.Forms.Label label1;
		private Janus.Windows.GridEX.EditControls.EditBox editBox1;
		private System.Windows.Forms.Panel pnlDetail;
		private AdManagerClient.SchChoiceAdDs schChoiceAdDs;
		private Janus.Windows.UI.Dock.UIPanelGroup uiPanelSchedule;
		private Janus.Windows.UI.Dock.UIPanel uiPanelMenu;
		private Janus.Windows.UI.Dock.UIPanelInnerContainer uiPanelMenuContainer;
		private Janus.Windows.UI.Dock.UIPanel uiPanelChannel;
		private Janus.Windows.UI.Dock.UIPanelInnerContainer uiPanelChannelContainer;
		private System.Windows.Forms.Panel panMenuSchedule;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Panel panel2;
		private System.Windows.Forms.Panel panel5;
		private System.Windows.Forms.Panel panel6;
		private System.Windows.Forms.Panel panel3;
		private Janus.Windows.GridEX.GridEX grdExGenreList;
		private System.Windows.Forms.Panel panel4;
		private Janus.Windows.GridEX.GridEX grdExChannelList;
		private System.Data.DataView dvMenu;
		private System.Data.DataView dvChannel;
		private Janus.Windows.EditControls.UIButton btnDeleteMenu;
		private Janus.Windows.EditControls.UIButton btnAddMenu;
		private Janus.Windows.EditControls.UIButton btnAddChannel;
		private Janus.Windows.EditControls.UIButton btnDeleteChannel;
		private System.Windows.Forms.Label lbMsg;
		private System.ComponentModel.IContainer components;

		public SchChoiceAdControl()
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
			Janus.Windows.GridEX.GridEXLayout grdExScheduleList_DesignTimeLayout = new Janus.Windows.GridEX.GridEXLayout();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SchChoiceAdControl));
			Janus.Windows.GridEX.GridEXLayout grdExGenreList_DesignTimeLayout = new Janus.Windows.GridEX.GridEXLayout();
			Janus.Windows.GridEX.GridEXLayout grdExChannelList_DesignTimeLayout = new Janus.Windows.GridEX.GridEXLayout();
			this.uiPM = new Janus.Windows.UI.Dock.UIPanelManager(this.components);
			this.uiPanelChoiceAdSchedule = new Janus.Windows.UI.Dock.UIPanelGroup();
			this.uiPanelSearch = new Janus.Windows.UI.Dock.UIPanel();
			this.uiPanelSearchContainer = new Janus.Windows.UI.Dock.UIPanelInnerContainer();
			this.pnlSearch = new System.Windows.Forms.Panel();
			this.chkAdState_40 = new Janus.Windows.EditControls.UICheckBox();
			this.chkAdState_30 = new Janus.Windows.EditControls.UICheckBox();
			this.chkAdState_20 = new Janus.Windows.EditControls.UICheckBox();
			this.lbAdState = new System.Windows.Forms.Label();
			this.ebSearchKey = new Janus.Windows.GridEX.EditControls.EditBox();
			this.cbSearchRap = new Janus.Windows.EditControls.UIComboBox();
			this.pnlSearchBtn = new System.Windows.Forms.Panel();
			this.btnSearch = new Janus.Windows.EditControls.UIButton();
			this.cbSearchAdClass = new Janus.Windows.EditControls.UIComboBox();
			this.uiPanelList = new Janus.Windows.UI.Dock.UIPanel();
			this.uiPanelListContainer = new Janus.Windows.UI.Dock.UIPanelInnerContainer();
			this.grdExScheduleList = new Janus.Windows.GridEX.GridEX();
			this.dvSchedule = new System.Data.DataView();
			this.schChoiceAdDs = new AdManagerClient.SchChoiceAdDs();
			this.uiPanelDetail = new Janus.Windows.UI.Dock.UIPanel();
			this.uiPanelDetailContainer = new Janus.Windows.UI.Dock.UIPanelInnerContainer();
			this.pnlDetail = new System.Windows.Forms.Panel();
			this.btnCopy = new Janus.Windows.EditControls.UIButton();
			this.lbMsg = new System.Windows.Forms.Label();
			this.btnDelete = new Janus.Windows.EditControls.UIButton();
			this.btnAdd = new Janus.Windows.EditControls.UIButton();
			this.uiPanelSchedule = new Janus.Windows.UI.Dock.UIPanelGroup();
			this.uiPanelMenu = new Janus.Windows.UI.Dock.UIPanel();
			this.uiPanelMenuContainer = new Janus.Windows.UI.Dock.UIPanelInnerContainer();
			this.panMenuSchedule = new System.Windows.Forms.Panel();
			this.panel2 = new System.Windows.Forms.Panel();
			this.panel3 = new System.Windows.Forms.Panel();
			this.grdExGenreList = new Janus.Windows.GridEX.GridEX();
			this.dvMenu = new System.Data.DataView();
			this.panel5 = new System.Windows.Forms.Panel();
			this.btnDeleteMenu = new Janus.Windows.EditControls.UIButton();
			this.btnAddMenu = new Janus.Windows.EditControls.UIButton();
			this.uiPanelChannel = new Janus.Windows.UI.Dock.UIPanel();
			this.uiPanelChannelContainer = new Janus.Windows.UI.Dock.UIPanelInnerContainer();
			this.panel1 = new System.Windows.Forms.Panel();
			this.panel4 = new System.Windows.Forms.Panel();
			this.grdExChannelList = new Janus.Windows.GridEX.GridEX();
			this.dvChannel = new System.Data.DataView();
			this.panel6 = new System.Windows.Forms.Panel();
			this.btnDeleteChannel = new Janus.Windows.EditControls.UIButton();
			this.btnAddChannel = new Janus.Windows.EditControls.UIButton();
			this.label1 = new System.Windows.Forms.Label();
			this.editBox1 = new Janus.Windows.GridEX.EditControls.EditBox();
			((System.ComponentModel.ISupportInitialize)(this.uiPM)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.uiPanelChoiceAdSchedule)).BeginInit();
			this.uiPanelChoiceAdSchedule.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.uiPanelSearch)).BeginInit();
			this.uiPanelSearch.SuspendLayout();
			this.uiPanelSearchContainer.SuspendLayout();
			this.pnlSearch.SuspendLayout();
			this.pnlSearchBtn.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.uiPanelList)).BeginInit();
			this.uiPanelList.SuspendLayout();
			this.uiPanelListContainer.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.grdExScheduleList)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.dvSchedule)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.schChoiceAdDs)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.uiPanelDetail)).BeginInit();
			this.uiPanelDetail.SuspendLayout();
			this.uiPanelDetailContainer.SuspendLayout();
			this.pnlDetail.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.uiPanelSchedule)).BeginInit();
			this.uiPanelSchedule.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.uiPanelMenu)).BeginInit();
			this.uiPanelMenu.SuspendLayout();
			this.uiPanelMenuContainer.SuspendLayout();
			this.panMenuSchedule.SuspendLayout();
			this.panel2.SuspendLayout();
			this.panel3.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.grdExGenreList)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.dvMenu)).BeginInit();
			this.panel5.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.uiPanelChannel)).BeginInit();
			this.uiPanelChannel.SuspendLayout();
			this.uiPanelChannelContainer.SuspendLayout();
			this.panel1.SuspendLayout();
			this.panel4.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.grdExChannelList)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.dvChannel)).BeginInit();
			this.panel6.SuspendLayout();
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
			this.uiPanelChoiceAdSchedule.Id = new System.Guid("b162c53a-7940-47cd-a0ed-9762c59c50d8");
			this.uiPanelChoiceAdSchedule.StaticGroup = true;
			this.uiPanelSearch.Id = new System.Guid("afb3b7d0-aa79-443c-a1e8-3baba38f7a4d");
			this.uiPanelChoiceAdSchedule.Panels.Add(this.uiPanelSearch);
			this.uiPanelList.Id = new System.Guid("8e3140b9-bfcb-43af-b84e-2ee6cddf51dc");
			this.uiPanelChoiceAdSchedule.Panels.Add(this.uiPanelList);
			this.uiPanelDetail.Id = new System.Guid("b2973a57-d3e1-4d80-9387-7a6dc3332542");
			this.uiPanelChoiceAdSchedule.Panels.Add(this.uiPanelDetail);
			this.uiPanelSchedule.Id = new System.Guid("18e78aea-bf80-4094-94f6-0d7d6006bb3d");
			this.uiPanelSchedule.StaticGroup = true;
			this.uiPanelMenu.Id = new System.Guid("02c920f2-57c5-4aa8-bf42-309880d3314e");
			this.uiPanelSchedule.Panels.Add(this.uiPanelMenu);
			this.uiPanelChannel.Id = new System.Guid("f40b18e5-7e83-4164-be4f-97c23451198d");
			this.uiPanelSchedule.Panels.Add(this.uiPanelChannel);
			this.uiPanelChoiceAdSchedule.Panels.Add(this.uiPanelSchedule);
			this.uiPM.Panels.Add(this.uiPanelChoiceAdSchedule);
			// 
			// Design Time Panel Info:
			// 
			this.uiPM.BeginPanelInfo();
			this.uiPM.AddDockPanelInfo(new System.Guid("b162c53a-7940-47cd-a0ed-9762c59c50d8"), Janus.Windows.UI.Dock.PanelGroupStyle.HorizontalTiles, Janus.Windows.UI.Dock.PanelDockStyle.Fill, true, new System.Drawing.Size(1010, 677), true);
			this.uiPM.AddDockPanelInfo(new System.Guid("afb3b7d0-aa79-443c-a1e8-3baba38f7a4d"), new System.Guid("b162c53a-7940-47cd-a0ed-9762c59c50d8"), 43, true);
			this.uiPM.AddDockPanelInfo(new System.Guid("8e3140b9-bfcb-43af-b84e-2ee6cddf51dc"), new System.Guid("b162c53a-7940-47cd-a0ed-9762c59c50d8"), 270, true);
			this.uiPM.AddDockPanelInfo(new System.Guid("b2973a57-d3e1-4d80-9387-7a6dc3332542"), new System.Guid("b162c53a-7940-47cd-a0ed-9762c59c50d8"), 60, true);
			this.uiPM.AddDockPanelInfo(new System.Guid("18e78aea-bf80-4094-94f6-0d7d6006bb3d"), new System.Guid("b162c53a-7940-47cd-a0ed-9762c59c50d8"), Janus.Windows.UI.Dock.PanelGroupStyle.VerticalTiles, true, 270, true);
			this.uiPM.AddDockPanelInfo(new System.Guid("02c920f2-57c5-4aa8-bf42-309880d3314e"), new System.Guid("18e78aea-bf80-4094-94f6-0d7d6006bb3d"), 412, true);
			this.uiPM.AddDockPanelInfo(new System.Guid("f40b18e5-7e83-4164-be4f-97c23451198d"), new System.Guid("18e78aea-bf80-4094-94f6-0d7d6006bb3d"), 594, true);
			this.uiPM.AddFloatingPanelInfo(new System.Guid("b162c53a-7940-47cd-a0ed-9762c59c50d8"), Janus.Windows.UI.Dock.PanelGroupStyle.HorizontalTiles, true, new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
			this.uiPM.AddFloatingPanelInfo(new System.Guid("8e3140b9-bfcb-43af-b84e-2ee6cddf51dc"), new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
			this.uiPM.AddFloatingPanelInfo(new System.Guid("b2973a57-d3e1-4d80-9387-7a6dc3332542"), new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
			this.uiPM.AddFloatingPanelInfo(new System.Guid("afb3b7d0-aa79-443c-a1e8-3baba38f7a4d"), new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
			this.uiPM.AddFloatingPanelInfo(new System.Guid("18e78aea-bf80-4094-94f6-0d7d6006bb3d"), Janus.Windows.UI.Dock.PanelGroupStyle.VerticalTiles, true, new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
			this.uiPM.AddFloatingPanelInfo(new System.Guid("02c920f2-57c5-4aa8-bf42-309880d3314e"), new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
			this.uiPM.AddFloatingPanelInfo(new System.Guid("f40b18e5-7e83-4164-be4f-97c23451198d"), new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
			this.uiPM.EndPanelInfo();
			// 
			// uiPanelChoiceAdSchedule
			// 
			this.uiPanelChoiceAdSchedule.ActiveCaptionMode = Janus.Windows.UI.Dock.ActiveCaptionMode.Never;
			this.uiPanelChoiceAdSchedule.BackColor = System.Drawing.SystemColors.Window;
			this.uiPanelChoiceAdSchedule.CaptionVisible = Janus.Windows.UI.InheritableBoolean.True;
			this.uiPanelChoiceAdSchedule.CloseButtonVisible = Janus.Windows.UI.InheritableBoolean.False;
			this.uiPanelChoiceAdSchedule.Location = new System.Drawing.Point(0, 0);
			this.uiPanelChoiceAdSchedule.Name = "uiPanelChoiceAdSchedule";
			this.uiPanelChoiceAdSchedule.Size = new System.Drawing.Size(1010, 677);
			this.uiPanelChoiceAdSchedule.TabIndex = 4;
			this.uiPanelChoiceAdSchedule.Text = "광고별 편성";
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
			this.pnlSearch.Controls.Add(this.chkAdState_40);
			this.pnlSearch.Controls.Add(this.chkAdState_30);
			this.pnlSearch.Controls.Add(this.chkAdState_20);
			this.pnlSearch.Controls.Add(this.lbAdState);
			this.pnlSearch.Controls.Add(this.ebSearchKey);
			this.pnlSearch.Controls.Add(this.cbSearchRap);
			this.pnlSearch.Controls.Add(this.pnlSearchBtn);
			this.pnlSearch.Controls.Add(this.cbSearchAdClass);
			this.pnlSearch.Dock = System.Windows.Forms.DockStyle.Fill;
			this.pnlSearch.Location = new System.Drawing.Point(0, 0);
			this.pnlSearch.Name = "pnlSearch";
			this.pnlSearch.Size = new System.Drawing.Size(1008, 41);
			this.pnlSearch.TabIndex = 0;
			// 
			// chkAdState_40
			// 
			this.chkAdState_40.Location = new System.Drawing.Point(694, 10);
			this.chkAdState_40.Name = "chkAdState_40";
			this.chkAdState_40.Size = new System.Drawing.Size(49, 23);
			this.chkAdState_40.TabIndex = 14;
			this.chkAdState_40.Text = "종료";
			// 
			// chkAdState_30
			// 
			this.chkAdState_30.Checked = true;
			this.chkAdState_30.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkAdState_30.Location = new System.Drawing.Point(639, 10);
			this.chkAdState_30.Name = "chkAdState_30";
			this.chkAdState_30.Size = new System.Drawing.Size(49, 23);
			this.chkAdState_30.TabIndex = 14;
			this.chkAdState_30.Text = "중지";
			// 
			// chkAdState_20
			// 
			this.chkAdState_20.Checked = true;
			this.chkAdState_20.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkAdState_20.Location = new System.Drawing.Point(584, 10);
			this.chkAdState_20.Name = "chkAdState_20";
			this.chkAdState_20.Size = new System.Drawing.Size(49, 23);
			this.chkAdState_20.TabIndex = 14;
			this.chkAdState_20.Text = "편성";
			// 
			// lbAdState
			// 
			this.lbAdState.Location = new System.Drawing.Point(521, 10);
			this.lbAdState.Name = "lbAdState";
			this.lbAdState.Size = new System.Drawing.Size(57, 23);
			this.lbAdState.TabIndex = 13;
			this.lbAdState.Text = "광고상태";
			this.lbAdState.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// ebSearchKey
			// 
			this.ebSearchKey.Location = new System.Drawing.Point(313, 9);
			this.ebSearchKey.Name = "ebSearchKey";
			this.ebSearchKey.Size = new System.Drawing.Size(197, 21);
			this.ebSearchKey.TabIndex = 5;
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
			this.cbSearchRap.Location = new System.Drawing.Point(10, 9);
			this.cbSearchRap.Name = "cbSearchRap";
			this.cbSearchRap.Size = new System.Drawing.Size(166, 21);
			this.cbSearchRap.TabIndex = 2;
			this.cbSearchRap.Text = "미디어랩선택";
			this.cbSearchRap.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
			// 
			// pnlSearchBtn
			// 
			this.pnlSearchBtn.BackColor = System.Drawing.SystemColors.Window;
			this.pnlSearchBtn.Controls.Add(this.btnSearch);
			this.pnlSearchBtn.Dock = System.Windows.Forms.DockStyle.Right;
			this.pnlSearchBtn.Location = new System.Drawing.Point(888, 0);
			this.pnlSearchBtn.Name = "pnlSearchBtn";
			this.pnlSearchBtn.Size = new System.Drawing.Size(120, 41);
			this.pnlSearchBtn.TabIndex = 11;
			// 
			// btnSearch
			// 
			this.btnSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnSearch.ButtonStyle = Janus.Windows.EditControls.ButtonStyle.Button;
			this.btnSearch.Cursor = System.Windows.Forms.Cursors.Hand;
			this.btnSearch.Enabled = false;
			this.btnSearch.Location = new System.Drawing.Point(8, 8);
			this.btnSearch.Name = "btnSearch";
			this.btnSearch.ShowFocusRectangle = false;
			this.btnSearch.Size = new System.Drawing.Size(104, 24);
			this.btnSearch.TabIndex = 11;
			this.btnSearch.Text = "조 회";
			this.btnSearch.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
			this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
			// 
			// cbSearchAdClass
			// 
			this.cbSearchAdClass.BackColor = System.Drawing.Color.White;
			this.cbSearchAdClass.ComboStyle = Janus.Windows.EditControls.ComboStyle.DropDownList;
			this.cbSearchAdClass.Location = new System.Drawing.Point(182, 9);
			this.cbSearchAdClass.Name = "cbSearchAdClass";
			this.cbSearchAdClass.Size = new System.Drawing.Size(120, 21);
			this.cbSearchAdClass.TabIndex = 7;
			this.cbSearchAdClass.Text = "광고종류";
			this.cbSearchAdClass.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
			// 
			// uiPanelList
			// 
			this.uiPanelList.AllowResize = Janus.Windows.UI.InheritableBoolean.True;
			this.uiPanelList.BackColor = System.Drawing.SystemColors.Window;
			this.uiPanelList.CaptionStyle = Janus.Windows.UI.Dock.PanelCaptionStyle.Dark;
			this.uiPanelList.CaptionVisible = Janus.Windows.UI.InheritableBoolean.False;
			this.uiPanelList.InnerContainer = this.uiPanelListContainer;
			this.uiPanelList.Location = new System.Drawing.Point(0, 69);
			this.uiPanelList.MinimumSize = new System.Drawing.Size(-1, 100);
			this.uiPanelList.Name = "uiPanelList";
			this.uiPanelList.Size = new System.Drawing.Size(1010, 270);
			this.uiPanelList.TabIndex = 4;
			this.uiPanelList.Text = "광고목록";
			// 
			// uiPanelListContainer
			// 
			this.uiPanelListContainer.BackColor = System.Drawing.SystemColors.Window;
			this.uiPanelListContainer.Controls.Add(this.grdExScheduleList);
			this.uiPanelListContainer.Location = new System.Drawing.Point(1, 1);
			this.uiPanelListContainer.Name = "uiPanelListContainer";
			this.uiPanelListContainer.Size = new System.Drawing.Size(1008, 268);
			this.uiPanelListContainer.TabIndex = 0;
			// 
			// grdExScheduleList
			// 
			this.grdExScheduleList.AlternatingColors = true;
			this.grdExScheduleList.BorderStyle = Janus.Windows.GridEX.BorderStyle.None;
			this.grdExScheduleList.DataSource = this.dvSchedule;
			grdExScheduleList_DesignTimeLayout.LayoutString = resources.GetString("grdExScheduleList_DesignTimeLayout.LayoutString");
			this.grdExScheduleList.DesignTimeLayout = grdExScheduleList_DesignTimeLayout;
			this.grdExScheduleList.Dock = System.Windows.Forms.DockStyle.Fill;
			this.grdExScheduleList.EmptyRows = true;
			this.grdExScheduleList.FocusCellFormatStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(174)))), ((int)(((byte)(196)))), ((int)(((byte)(232)))));
			this.grdExScheduleList.FocusCellFormatStyle.BackColorGradient = System.Drawing.Color.FromArgb(((int)(((byte)(174)))), ((int)(((byte)(196)))), ((int)(((byte)(232)))));
			this.grdExScheduleList.FocusCellFormatStyle.FontBold = Janus.Windows.GridEX.TriState.True;
			this.grdExScheduleList.FocusCellFormatStyle.ForeColor = System.Drawing.Color.White;
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
			this.grdExScheduleList.Size = new System.Drawing.Size(1008, 268);
			this.grdExScheduleList.TabIndex = 12;
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
			this.grdExScheduleList.SelectionChanged += new System.EventHandler(this.OnGrdRowChanged);
			// 
			// dvSchedule
			// 
			this.dvSchedule.Table = this.schChoiceAdDs.ChoiceAdSchedule;
			// 
			// schChoiceAdDs
			// 
			this.schChoiceAdDs.DataSetName = "SchChoiceAdDs";
			this.schChoiceAdDs.Locale = new System.Globalization.CultureInfo("en-US");
			this.schChoiceAdDs.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
			// 
			// uiPanelDetail
			// 
			this.uiPanelDetail.ActiveCaptionMode = Janus.Windows.UI.Dock.ActiveCaptionMode.Never;
			this.uiPanelDetail.AllowResize = Janus.Windows.UI.InheritableBoolean.True;
			this.uiPanelDetail.BackColor = System.Drawing.SystemColors.Window;
			this.uiPanelDetail.CaptionStyle = Janus.Windows.UI.Dock.PanelCaptionStyle.Dark;
			this.uiPanelDetail.CaptionVisible = Janus.Windows.UI.InheritableBoolean.True;
			this.uiPanelDetail.InnerContainer = this.uiPanelDetailContainer;
			this.uiPanelDetail.Location = new System.Drawing.Point(0, 343);
			this.uiPanelDetail.Name = "uiPanelDetail";
			this.uiPanelDetail.Size = new System.Drawing.Size(1010, 60);
			this.uiPanelDetail.TabIndex = 4;
			this.uiPanelDetail.Text = "편성광고";
			// 
			// uiPanelDetailContainer
			// 
			this.uiPanelDetailContainer.Controls.Add(this.pnlDetail);
			this.uiPanelDetailContainer.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.uiPanelDetailContainer.Location = new System.Drawing.Point(1, 23);
			this.uiPanelDetailContainer.Name = "uiPanelDetailContainer";
			this.uiPanelDetailContainer.Size = new System.Drawing.Size(1008, 36);
			this.uiPanelDetailContainer.TabIndex = 0;
			// 
			// pnlDetail
			// 
			this.pnlDetail.BackColor = System.Drawing.SystemColors.Window;
			this.pnlDetail.Controls.Add(this.btnCopy);
			this.pnlDetail.Controls.Add(this.lbMsg);
			this.pnlDetail.Controls.Add(this.btnDelete);
			this.pnlDetail.Controls.Add(this.btnAdd);
			this.pnlDetail.Dock = System.Windows.Forms.DockStyle.Fill;
			this.pnlDetail.Location = new System.Drawing.Point(0, 0);
			this.pnlDetail.Name = "pnlDetail";
			this.pnlDetail.Size = new System.Drawing.Size(1008, 36);
			this.pnlDetail.TabIndex = 13;
			// 
			// btnCopy
			// 
			this.btnCopy.Cursor = System.Windows.Forms.Cursors.Hand;
			this.btnCopy.Enabled = false;
			this.btnCopy.Location = new System.Drawing.Point(228, 6);
			this.btnCopy.Name = "btnCopy";
			this.btnCopy.Size = new System.Drawing.Size(104, 24);
			this.btnCopy.TabIndex = 45;
			this.btnCopy.Text = "편성 복사";
			this.btnCopy.ToolTipText = "편성되어 있는 광고의 모든 내역을 불러와서 선택한 광고에 복사합니다.";
			this.btnCopy.Visible = false;
			this.btnCopy.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
			this.btnCopy.Click += new System.EventHandler(this.btnCopy_Click);
			// 
			// lbMsg
			// 
			this.lbMsg.Location = new System.Drawing.Point(338, 8);
			this.lbMsg.Name = "lbMsg";
			this.lbMsg.Size = new System.Drawing.Size(566, 21);
			this.lbMsg.TabIndex = 43;
			this.lbMsg.Text = "편성 메시지를 출력하는 곳입니다.";
			this.lbMsg.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// btnDelete
			// 
			this.btnDelete.Cursor = System.Windows.Forms.Cursors.Hand;
			this.btnDelete.Enabled = false;
			this.btnDelete.Location = new System.Drawing.Point(118, 6);
			this.btnDelete.Name = "btnDelete";
			this.btnDelete.Size = new System.Drawing.Size(104, 24);
			this.btnDelete.TabIndex = 14;
			this.btnDelete.Text = "삭 제";
			this.btnDelete.ToolTipText = "편성되어 있는 광고의 모든 내역을 삭제합니다";
			this.btnDelete.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
			this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
			// 
			// btnAdd
			// 
			this.btnAdd.Cursor = System.Windows.Forms.Cursors.Hand;
			this.btnAdd.Enabled = false;
			this.btnAdd.Location = new System.Drawing.Point(8, 6);
			this.btnAdd.Name = "btnAdd";
			this.btnAdd.Size = new System.Drawing.Size(104, 24);
			this.btnAdd.TabIndex = 13;
			this.btnAdd.Text = "추 가";
			this.btnAdd.ToolTipText = "지정편성할 광고를 추가 합니다";
			this.btnAdd.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
			this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
			// 
			// uiPanelSchedule
			// 
			this.uiPanelSchedule.AllowResize = Janus.Windows.UI.InheritableBoolean.True;
			this.uiPanelSchedule.CaptionVisible = Janus.Windows.UI.InheritableBoolean.False;
			this.uiPanelSchedule.GroupStyle = Janus.Windows.UI.Dock.PanelGroupStyle.VerticalTiles;
			this.uiPanelSchedule.Location = new System.Drawing.Point(0, 407);
			this.uiPanelSchedule.Name = "uiPanelSchedule";
			this.uiPanelSchedule.Size = new System.Drawing.Size(1010, 270);
			this.uiPanelSchedule.TabIndex = 4;
			this.uiPanelSchedule.Text = "편성내역";
			// 
			// uiPanelMenu
			// 
			this.uiPanelMenu.AllowResize = Janus.Windows.UI.InheritableBoolean.True;
			this.uiPanelMenu.InnerContainer = this.uiPanelMenuContainer;
			this.uiPanelMenu.Location = new System.Drawing.Point(0, 0);
			this.uiPanelMenu.Name = "uiPanelMenu";
			this.uiPanelMenu.Size = new System.Drawing.Size(412, 270);
			this.uiPanelMenu.TabIndex = 4;
			this.uiPanelMenu.Text = "메뉴편성 목록";
			// 
			// uiPanelMenuContainer
			// 
			this.uiPanelMenuContainer.Controls.Add(this.panMenuSchedule);
			this.uiPanelMenuContainer.Location = new System.Drawing.Point(1, 23);
			this.uiPanelMenuContainer.Name = "uiPanelMenuContainer";
			this.uiPanelMenuContainer.Size = new System.Drawing.Size(410, 246);
			this.uiPanelMenuContainer.TabIndex = 0;
			// 
			// panMenuSchedule
			// 
			this.panMenuSchedule.BackColor = System.Drawing.SystemColors.Window;
			this.panMenuSchedule.Controls.Add(this.panel2);
			this.panMenuSchedule.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panMenuSchedule.Location = new System.Drawing.Point(0, 0);
			this.panMenuSchedule.Name = "panMenuSchedule";
			this.panMenuSchedule.Size = new System.Drawing.Size(410, 246);
			this.panMenuSchedule.TabIndex = 6;
			// 
			// panel2
			// 
			this.panel2.BackColor = System.Drawing.SystemColors.Window;
			this.panel2.Controls.Add(this.panel3);
			this.panel2.Controls.Add(this.panel5);
			this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel2.Location = new System.Drawing.Point(0, 0);
			this.panel2.Name = "panel2";
			this.panel2.Size = new System.Drawing.Size(410, 246);
			this.panel2.TabIndex = 4;
			// 
			// panel3
			// 
			this.panel3.Controls.Add(this.grdExGenreList);
			this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel3.Location = new System.Drawing.Point(0, 0);
			this.panel3.Name = "panel3";
			this.panel3.Size = new System.Drawing.Size(410, 206);
			this.panel3.TabIndex = 23;
			// 
			// grdExGenreList
			// 
			this.grdExGenreList.AlternatingColors = true;
			this.grdExGenreList.BorderStyle = Janus.Windows.GridEX.BorderStyle.None;
			this.grdExGenreList.DataSource = this.dvMenu;
			grdExGenreList_DesignTimeLayout.LayoutString = resources.GetString("grdExGenreList_DesignTimeLayout.LayoutString");
			this.grdExGenreList.DesignTimeLayout = grdExGenreList_DesignTimeLayout;
			this.grdExGenreList.Dock = System.Windows.Forms.DockStyle.Fill;
			this.grdExGenreList.EditorsControlStyle.ButtonAppearance = Janus.Windows.GridEX.ButtonAppearance.Regular;
			this.grdExGenreList.EmptyRows = true;
			this.grdExGenreList.FocusCellFormatStyle.BackColor = System.Drawing.SystemColors.Highlight;
			this.grdExGenreList.FocusStyle = Janus.Windows.GridEX.FocusStyle.None;
			this.grdExGenreList.FrozenColumns = 2;
			this.grdExGenreList.GridLineColor = System.Drawing.Color.Silver;
			this.grdExGenreList.GridLines = Janus.Windows.GridEX.GridLines.Vertical;
			this.grdExGenreList.GridLineStyle = Janus.Windows.GridEX.GridLineStyle.Solid;
			this.grdExGenreList.GroupByBoxVisible = false;
			this.grdExGenreList.Location = new System.Drawing.Point(0, 0);
			this.grdExGenreList.Name = "grdExGenreList";
			this.grdExGenreList.ScrollBars = Janus.Windows.GridEX.ScrollBars.Vertical;
			this.grdExGenreList.SelectedInactiveFormatStyle.BackColor = System.Drawing.Color.DarkOrange;
			this.grdExGenreList.Size = new System.Drawing.Size(410, 206);
			this.grdExGenreList.TabIndex = 15;
			this.grdExGenreList.TabKeyBehavior = Janus.Windows.GridEX.TabKeyBehavior.ControlNavigation;
			this.grdExGenreList.ThemedAreas = ((Janus.Windows.GridEX.ThemedArea)(((((((((Janus.Windows.GridEX.ThemedArea.ScrollBars | Janus.Windows.GridEX.ThemedArea.EditControls) 
            | Janus.Windows.GridEX.ThemedArea.Headers) 
            | Janus.Windows.GridEX.ThemedArea.GroupByBox) 
            | Janus.Windows.GridEX.ThemedArea.GroupRows) 
            | Janus.Windows.GridEX.ThemedArea.ControlBorder) 
            | Janus.Windows.GridEX.ThemedArea.Cards) 
            | Janus.Windows.GridEX.ThemedArea.Gridlines) 
            | Janus.Windows.GridEX.ThemedArea.CheckBoxes)));
			this.grdExGenreList.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
			this.grdExGenreList.CellValueChanged += new Janus.Windows.GridEX.ColumnActionEventHandler(this.grdExGenreList_CellValueChanged);
			this.grdExGenreList.FormattingRow += new Janus.Windows.GridEX.RowLoadEventHandler(this.grdExGenreList_FormattingRow);
			this.grdExGenreList.ColumnHeaderClick += new Janus.Windows.GridEX.ColumnActionEventHandler(this.grdExGenreList_ColumnHeaderClick);
			// 
			// dvMenu
			// 
			this.dvMenu.Table = this.schChoiceAdDs.Genre;
			// 
			// panel5
			// 
			this.panel5.BackColor = System.Drawing.SystemColors.Window;
			this.panel5.Controls.Add(this.btnDeleteMenu);
			this.panel5.Controls.Add(this.btnAddMenu);
			this.panel5.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.panel5.Location = new System.Drawing.Point(0, 206);
			this.panel5.Name = "panel5";
			this.panel5.Size = new System.Drawing.Size(410, 40);
			this.panel5.TabIndex = 16;
			// 
			// btnDeleteMenu
			// 
			this.btnDeleteMenu.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.btnDeleteMenu.Cursor = System.Windows.Forms.Cursors.Hand;
			this.btnDeleteMenu.Enabled = false;
			this.btnDeleteMenu.Location = new System.Drawing.Point(120, 8);
			this.btnDeleteMenu.Name = "btnDeleteMenu";
			this.btnDeleteMenu.Size = new System.Drawing.Size(104, 24);
			this.btnDeleteMenu.TabIndex = 17;
			this.btnDeleteMenu.Text = "삭 제";
			this.btnDeleteMenu.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
			this.btnDeleteMenu.Click += new System.EventHandler(this.btnDeleteMenu_Click);
			// 
			// btnAddMenu
			// 
			this.btnAddMenu.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.btnAddMenu.Cursor = System.Windows.Forms.Cursors.Hand;
			this.btnAddMenu.Enabled = false;
			this.btnAddMenu.Location = new System.Drawing.Point(8, 8);
			this.btnAddMenu.Name = "btnAddMenu";
			this.btnAddMenu.Size = new System.Drawing.Size(104, 24);
			this.btnAddMenu.TabIndex = 16;
			this.btnAddMenu.Text = "추 가";
			this.btnAddMenu.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
			this.btnAddMenu.Click += new System.EventHandler(this.btnAddMenu_Click);
			// 
			// uiPanelChannel
			// 
			this.uiPanelChannel.AllowResize = Janus.Windows.UI.InheritableBoolean.True;
			this.uiPanelChannel.InnerContainer = this.uiPanelChannelContainer;
			this.uiPanelChannel.Location = new System.Drawing.Point(416, 0);
			this.uiPanelChannel.Name = "uiPanelChannel";
			this.uiPanelChannel.Size = new System.Drawing.Size(594, 270);
			this.uiPanelChannel.TabIndex = 4;
			this.uiPanelChannel.Text = "타이틀편성 목록";
			// 
			// uiPanelChannelContainer
			// 
			this.uiPanelChannelContainer.Controls.Add(this.panel1);
			this.uiPanelChannelContainer.Location = new System.Drawing.Point(1, 23);
			this.uiPanelChannelContainer.Name = "uiPanelChannelContainer";
			this.uiPanelChannelContainer.Size = new System.Drawing.Size(592, 246);
			this.uiPanelChannelContainer.TabIndex = 0;
			// 
			// panel1
			// 
			this.panel1.BackColor = System.Drawing.SystemColors.Window;
			this.panel1.Controls.Add(this.panel4);
			this.panel1.Controls.Add(this.panel6);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel1.Location = new System.Drawing.Point(0, 0);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(592, 246);
			this.panel1.TabIndex = 7;
			// 
			// panel4
			// 
			this.panel4.Controls.Add(this.grdExChannelList);
			this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel4.Location = new System.Drawing.Point(0, 0);
			this.panel4.Name = "panel4";
			this.panel4.Size = new System.Drawing.Size(592, 206);
			this.panel4.TabIndex = 24;
			// 
			// grdExChannelList
			// 
			this.grdExChannelList.AlternatingColors = true;
			this.grdExChannelList.BorderStyle = Janus.Windows.GridEX.BorderStyle.None;
			this.grdExChannelList.DataSource = this.dvChannel;
			grdExChannelList_DesignTimeLayout.LayoutString = resources.GetString("grdExChannelList_DesignTimeLayout.LayoutString");
			this.grdExChannelList.DesignTimeLayout = grdExChannelList_DesignTimeLayout;
			this.grdExChannelList.Dock = System.Windows.Forms.DockStyle.Fill;
			this.grdExChannelList.EditorsControlStyle.ButtonAppearance = Janus.Windows.GridEX.ButtonAppearance.Regular;
			this.grdExChannelList.EmptyRows = true;
			this.grdExChannelList.FocusCellFormatStyle.BackColor = System.Drawing.SystemColors.Highlight;
			this.grdExChannelList.FrozenColumns = 2;
			this.grdExChannelList.GridLineColor = System.Drawing.Color.Silver;
			this.grdExChannelList.GridLines = Janus.Windows.GridEX.GridLines.Vertical;
			this.grdExChannelList.GridLineStyle = Janus.Windows.GridEX.GridLineStyle.Solid;
			this.grdExChannelList.GroupByBoxVisible = false;
			this.grdExChannelList.HeaderFormatStyle.TextAlignment = Janus.Windows.GridEX.TextAlignment.Center;
			this.grdExChannelList.Location = new System.Drawing.Point(0, 0);
			this.grdExChannelList.Name = "grdExChannelList";
			this.grdExChannelList.ScrollBars = Janus.Windows.GridEX.ScrollBars.Vertical;
			this.grdExChannelList.SelectedInactiveFormatStyle.BackColor = System.Drawing.Color.DarkOrange;
			this.grdExChannelList.Size = new System.Drawing.Size(592, 206);
			this.grdExChannelList.TabIndex = 18;
			this.grdExChannelList.TabKeyBehavior = Janus.Windows.GridEX.TabKeyBehavior.ControlNavigation;
			this.grdExChannelList.ThemedAreas = ((Janus.Windows.GridEX.ThemedArea)(((((((((Janus.Windows.GridEX.ThemedArea.ScrollBars | Janus.Windows.GridEX.ThemedArea.EditControls) 
            | Janus.Windows.GridEX.ThemedArea.Headers) 
            | Janus.Windows.GridEX.ThemedArea.GroupByBox) 
            | Janus.Windows.GridEX.ThemedArea.GroupRows) 
            | Janus.Windows.GridEX.ThemedArea.ControlBorder) 
            | Janus.Windows.GridEX.ThemedArea.Cards) 
            | Janus.Windows.GridEX.ThemedArea.Gridlines) 
            | Janus.Windows.GridEX.ThemedArea.CheckBoxes)));
			this.grdExChannelList.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
			this.grdExChannelList.CellValueChanged += new Janus.Windows.GridEX.ColumnActionEventHandler(this.grdExChannelList_CellValueChanged);
			this.grdExChannelList.ColumnHeaderClick += new Janus.Windows.GridEX.ColumnActionEventHandler(this.grdExChannelList_ColumnHeaderClick);
			// 
			// dvChannel
			// 
			this.dvChannel.Table = this.schChoiceAdDs.Channel;
			// 
			// panel6
			// 
			this.panel6.BackColor = System.Drawing.SystemColors.Window;
			this.panel6.Controls.Add(this.btnDeleteChannel);
			this.panel6.Controls.Add(this.btnAddChannel);
			this.panel6.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.panel6.Location = new System.Drawing.Point(0, 206);
			this.panel6.Name = "panel6";
			this.panel6.Size = new System.Drawing.Size(592, 40);
			this.panel6.TabIndex = 19;
			// 
			// btnDeleteChannel
			// 
			this.btnDeleteChannel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.btnDeleteChannel.Cursor = System.Windows.Forms.Cursors.Hand;
			this.btnDeleteChannel.Enabled = false;
			this.btnDeleteChannel.Location = new System.Drawing.Point(120, 8);
			this.btnDeleteChannel.Name = "btnDeleteChannel";
			this.btnDeleteChannel.Size = new System.Drawing.Size(104, 24);
			this.btnDeleteChannel.TabIndex = 21;
			this.btnDeleteChannel.Text = "삭 제";
			this.btnDeleteChannel.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
			this.btnDeleteChannel.Click += new System.EventHandler(this.btnDeleteChannel_Click);
			// 
			// btnAddChannel
			// 
			this.btnAddChannel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.btnAddChannel.Cursor = System.Windows.Forms.Cursors.Hand;
			this.btnAddChannel.Enabled = false;
			this.btnAddChannel.Location = new System.Drawing.Point(8, 8);
			this.btnAddChannel.Name = "btnAddChannel";
			this.btnAddChannel.Size = new System.Drawing.Size(104, 24);
			this.btnAddChannel.TabIndex = 20;
			this.btnAddChannel.Text = "추 가";
			this.btnAddChannel.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
			this.btnAddChannel.Click += new System.EventHandler(this.btnAddChannel_Click);
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(0, 0);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(100, 23);
			this.label1.TabIndex = 0;
			// 
			// editBox1
			// 
			this.editBox1.Location = new System.Drawing.Point(0, 0);
			this.editBox1.Name = "editBox1";
			this.editBox1.Size = new System.Drawing.Size(0, 21);
			this.editBox1.TabIndex = 0;
			this.editBox1.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
			// 
			// SchChoiceAdControl
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.Controls.Add(this.uiPanelChoiceAdSchedule);
			this.Font = global::AdManagerClient.Properties.Settings.Default.Font1;
			this.Name = "SchChoiceAdControl";
			this.Size = new System.Drawing.Size(1010, 677);
			this.Load += new System.EventHandler(this.UserControl_Load);
			((System.ComponentModel.ISupportInitialize)(this.uiPM)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.uiPanelChoiceAdSchedule)).EndInit();
			this.uiPanelChoiceAdSchedule.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.uiPanelSearch)).EndInit();
			this.uiPanelSearch.ResumeLayout(false);
			this.uiPanelSearchContainer.ResumeLayout(false);
			this.pnlSearch.ResumeLayout(false);
			this.pnlSearch.PerformLayout();
			this.pnlSearchBtn.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.uiPanelList)).EndInit();
			this.uiPanelList.ResumeLayout(false);
			this.uiPanelListContainer.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.grdExScheduleList)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.dvSchedule)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.schChoiceAdDs)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.uiPanelDetail)).EndInit();
			this.uiPanelDetail.ResumeLayout(false);
			this.uiPanelDetailContainer.ResumeLayout(false);
			this.pnlDetail.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.uiPanelSchedule)).EndInit();
			this.uiPanelSchedule.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.uiPanelMenu)).EndInit();
			this.uiPanelMenu.ResumeLayout(false);
			this.uiPanelMenuContainer.ResumeLayout(false);
			this.panMenuSchedule.ResumeLayout(false);
			this.panel2.ResumeLayout(false);
			this.panel3.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.grdExGenreList)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.dvMenu)).EndInit();
			this.panel5.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.uiPanelChannel)).EndInit();
			this.uiPanelChannel.ResumeLayout(false);
			this.uiPanelChannelContainer.ResumeLayout(false);
			this.panel1.ResumeLayout(false);
			this.panel4.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.grdExChannelList)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.dvChannel)).EndInit();
			this.panel6.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		#region 컨트롤 로드
		private void UserControl_Load(object sender, System.EventArgs e)
		{
			// 데이터관리용 객체생성
			dt = ((DataView)grdExScheduleList.DataSource).Table;  
			cm = (CurrencyManager) this.BindingContext[grdExScheduleList.DataSource];

            dtMenu = ((DataView)grdExGenreList.DataSource).Table;
            dtChannel = ((DataView)grdExChannelList.DataSource).Table;

            //데이터 그리드변경에 따른 데이터셋 관리
            cmMenu = (CurrencyManager)this.BindingContext[grdExGenreList.DataSource];
            cmChannel = (CurrencyManager)this.BindingContext[grdExChannelList.DataSource]; 

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

            if(menu.CanCreate(MenuCode))	canCreate = true;
            if(menu.CanDelete(MenuCode))	canDelete = true;
			if(menu.CanRead(MenuCode))		canRead = true;
			
    		InitButton();
            ProgressStop();

			if (canRead)
			{
				SearchScheduleChoiceAd();
			}
		}

		private void InitCombo()
		{
			Init_RapCode();
			Init_AdClass();
			InitCombo_Level();
		}
				
		private void Init_RapCode()
		{
			// 랩을 조회한다.
			MediaRapCodeModel mediarapcodeModel = new MediaRapCodeModel();
			new MediaRapCodeManager(systemModel, commonModel).GetMediaRapCodeList(mediarapcodeModel);
			
			if (mediarapcodeModel.ResultCD.Equals("0000"))
			{
				// 데이터셋에 셋팅
				Utility.SetDataTable(schChoiceAdDs.MediaRaps, mediarapcodeModel.MediaRapCodeDataSet);				
			}

			// 검색조건의 콤보
			this.cbSearchRap.Items.Clear();
			
			// 콤보박스에 셋트할 코드목록을 담을 Item배열을 선언
			Janus.Windows.EditControls.UIComboBoxItem[] comboItems = new Janus.Windows.EditControls.UIComboBoxItem[mediarapcodeModel.ResultCnt + 1];

			comboItems[0] = new Janus.Windows.EditControls.UIComboBoxItem("미디어렙선택","00");
			
			for(int i=0;i<mediarapcodeModel.ResultCnt;i++)
			{
				DataRow row = schChoiceAdDs.MediaRaps.Rows[i];

				string val = row["RapCode"].ToString();
				string txt = row["RapName"].ToString();
				comboItems[i+1] = new Janus.Windows.EditControls.UIComboBoxItem(txt,val);
			}
			// 콤보에 셋트
			this.cbSearchRap.Items.AddRange(comboItems);
			this.cbSearchRap.SelectedIndex = 0;

			Application.DoEvents();
		}
		
		private void Init_AdClass()
		{
			// 코드에서 광고용도를 조회한다.(29)
            // 2009.06.23 광고종류로 변경한다
			CodeModel codeModel = new CodeModel();
			codeModel.Section = "26";
			new CodeManager(systemModel, commonModel).GetCodeList(codeModel);
			
			if (codeModel.ResultCD.Equals("0000"))
			{
				// 데이터셋에 셋팅
				Utility.SetDataTable(schChoiceAdDs.AdClass, codeModel.CodeDataSet);				
			}

			// 검색조건의 콤보
			this.cbSearchAdClass.Items.Clear();
			
			// 콤보박스에 셋트할 코드목록을 담을 Item배열을 선언
			Janus.Windows.EditControls.UIComboBoxItem[] comboItems = new Janus.Windows.EditControls.UIComboBoxItem[codeModel.ResultCnt + 1];

			comboItems[0] = new Janus.Windows.EditControls.UIComboBoxItem("광고종류선택","00");
			
			for(int i=0;i<codeModel.ResultCnt;i++)
			{
				DataRow row = schChoiceAdDs.AdClass.Rows[i];

				string val = row["Code"].ToString();
				string txt = row["CodeName"].ToString();
				comboItems[i+1] = new Janus.Windows.EditControls.UIComboBoxItem(txt,val);
			}
			// 콤보에 셋트
			this.cbSearchAdClass.Items.AddRange(comboItems);
			this.cbSearchAdClass.SelectedIndex = 0;

			Application.DoEvents();
		}

		private void InitCombo_Level()
		{
			if(commonModel.UserLevel=="30")
			{
				cbSearchRap.SelectedValue = commonModel.RapCode;			
				cbSearchRap.ReadOnly = true;				
			}
			Application.DoEvents();
		}

		private void InitButton()
		{
			if(canRead)   btnSearch.Enabled = true;

			grdExScheduleList.Focus();

			Application.DoEvents();
		}

		private void DisableButton()
		{
			btnSearch.Enabled	= false;
			btnAdd.Enabled		= true;
			btnDelete.Enabled   = false;

			btnAddMenu.Enabled       = false;
			btnAddChannel.Enabled    = false;
			btnDeleteMenu.Enabled    = false;
			btnDeleteChannel.Enabled = false;

			Application.DoEvents();
		}

		#endregion

		#region 광고파일 액션처리 메소드

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
			SearchScheduleChoiceAd();
			InitButton();
            ProgressStop();
		}

		/// <summary>
		/// 추가버튼 클릭
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		/// 
		private void btnAdd_Click(object sender, System.EventArgs e)
		{
			DisableButton();	
			//  지정광고 대상목록 검색창 
            ItemMultiChoiceForm pForm = new ItemMultiChoiceForm(this);

			// 매체코드셋트
			//pForm.keyMediaCode = "1";
			if( keyAckStateCm.Equals("") || keyAckStateCm.Equals("10") || keyAckStateCm.Equals("30") )
			{
				if( keyAckStateOap.Equals("") || keyAckStateOap.Equals("10") || keyAckStateOap.Equals("30") )
				{
					pForm.keySchType = "000";
				}
				else
				{
					pForm.keySchType = "100";
				}
			}
			else
			{
				if( keyAckStateOap.Equals("") || keyAckStateOap.Equals("10") || keyAckStateOap.Equals("30") )
				{
					pForm.keySchType = "200";
				}
				else
				{
					// 이건 해당버튼이 Disable되서 호출되지 않는다
					// 혹시 몰라서
					pForm.keySchType = "77";
				}
			
			}

            pForm.callType = "SchChoiceAdControl";
            pForm.ReturnDate += new ItemMultiChoiceForm.PopupService(ItemMultiChoiceForm_Return);
            pForm.ShowDialog();
            pForm.Dispose();
            pForm = null;
			
			InitButton();			
		}
        /// <summary>
        /// 편성 복사버튼 클릭 [E_01]
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCopy_Click(object sender, EventArgs e)
        {
            ItemSingleChoiceForm pForm = new ItemSingleChoiceForm(this);
            //매체 코드셋트
			pForm.keyMediaCode = "1";
            pForm.callType = "GetSchAdItemList";
            pForm.callItemNo = keyItemNo;   //선택한 광고번호(편성을 입력할)
            pForm.ReturnDate += new ItemSingleChoiceForm.PopupService(ItemSingleChoiceCopyForm_Return);
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
                string ItemNo = "";
                //인서트 시킴
                for (int i = 0; i < itemMultiChoice_pDs.ChoiceAdItems.Rows.Count; i++)
                {
                    DataRow row = itemMultiChoice_pDs.ChoiceAdItems.Rows[i];

                    if (row["CheckYn"].ToString().Equals("True"))
                    {
                        schChoiceAdModel.Init();

                        schChoiceAdModel.MediaCode = keyMediaCode;
                        schChoiceAdModel.ItemNo = row["ItemNo"].ToString();
                        schChoiceAdModel.ItemName = row["ItemName"].ToString();

                        ItemNo = row["ItemNo"].ToString();

                        new SchChoiceAdManager(systemModel, commonModel).SetSchChoiceAdAdd(schChoiceAdModel);
                    }
                }

                keyItemNo = ItemNo;
                ReloadList();

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
        /// 광고 단일선택 폼에서 광고 선택 [E_01]
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        public void ItemSingleChoiceCopyForm_Return(object sender, EventArgs args)
        {
            ItemEventArgs itemEventArgs = (ItemEventArgs)args;
            SchChoiceAdModel schChoiceAdModel = new SchChoiceAdModel();
            try
            {

                if (itemEventArgs.keyItemNo > 0)
                {
                    schChoiceAdModel.ItemNo = keyItemNo;    // 편성을 적용할 광고 
                    schChoiceAdModel.ItemNoCopy = itemEventArgs.keyItemNo.ToString(); // 편성을 복사해올 광고  
                    schChoiceAdModel.AdType = keyAdType;    // 광고 종류
                    schChoiceAdModel.MediaCode = keyMediaCode;
                    schChoiceAdModel.CheckSchResult = itemEventArgs.CheckSchResult.ToString(); // 기편성상태를 체크하여 편성을 새로 넣을지, 삭제후 넣을지

                    new SchChoiceAdManager(systemModel, commonModel).SetSchChoiceAdCopy(schChoiceAdModel);

                    ReloadList();
                }
            }
            catch (FrameException fe)
            {
                FrameSystem.showMsgForm("편성 복사 붙여넣기 오류", new string[] { fe.ErrCode, fe.ResultMsg });
            }
            catch (Exception ex)
            {
                FrameSystem.showMsgForm("편성 복사 붙여넣기 오류", new string[] { "", ex.Message });
            }
        }

		/// <summary>
		/// 삭제버튼 클릭
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnDelete_Click(object sender, System.EventArgs e)
		{
			DeleteScheduleChoiceAd();
			InitButton();
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
				SearchScheduleChoiceAd();
			}
		}

		private void btnAddMenu_Click(object sender, System.EventArgs e)
		{
			//  메뉴목록 검색창 
			SchChoiceAdSearchMenu_pForm pForm = new SchChoiceAdSearchMenu_pForm(this);
			//SchChoiceAdSearchGenre_pForm pForm = new SchChoiceAdSearchGenre_pForm(this);

			pForm.ShowDialog();            
			pForm.Dispose();
			pForm = null;		
		}

		private void btnDeleteMenu_Click(object sender, System.EventArgs e)
		{
			DeleteChoiceAdMenuDetail();
		}


		private void grdExGenreList_ColumnHeaderClick(object sender, Janus.Windows.GridEX.ColumnActionEventArgs e)
        {

			//컬럼Index 0(체크박스컬럼이)이 아니면 빠져나가게 처리.
			if(e.Column.Index != 0)
            {
				return;
			}
            
			//ColumnHeader Click시에 dt Setting 
			DataRow[] foundRows = dtMenu.Select("CheckYn = 'False'");
         
			// if(grdExItemList.CurrentColumn.Position == 0){
			if( foundRows.Length == 0 )
			{
				for(int i=0;i < dtMenu.Rows.Count;i++)
				{
					dtMenu.Rows[i].BeginEdit();
					dtMenu.Rows[i]["CheckYn"]="False";
					dtMenu.Rows[i].EndEdit();
				}
			}
			else
			{
				for(int i=0;i < dtMenu.Rows.Count;i++)
				{
					dtMenu.Rows[i].BeginEdit();
					dtMenu.Rows[i]["CheckYn"]="True";
					dtMenu.Rows[i].EndEdit();
				}
			}
		}

		private void btnAddChannel_Click(object sender, System.EventArgs e)
		{
			//  메뉴목록 검색창 
			SchChoiceAdSearchChannel_pForm pForm = new SchChoiceAdSearchChannel_pForm(this);
			pForm.ShowDialog();            
			pForm.Dispose();
			pForm = null;				
		}

		/// <summary>
		/// 채널편성 삭제 버튼 클릭처리
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnDeleteChannel_Click(object sender, System.EventArgs e)
		{
			DeleteChoiceAdChannelDetail();
		}

		private void grdExChannelList_ColumnHeaderClick(object sender, Janus.Windows.GridEX.ColumnActionEventArgs e)
        {

			//컬럼Index 0(체크박스컬럼이)이 아니면 빠져나가게 처리.
			if(e.Column.Index != 0)
            {
                return;
			}
            
			//ColumnHeader Click시에 dt Setting 
			DataRow[] foundRows = dtChannel.Select("CheckYn = 'False'");
         
			// if(grdExItemList.CurrentColumn.Position == 0){
			if( foundRows.Length == 0 )
			{
				for(int i=0;i < dtChannel.Rows.Count;i++)
				{
					dtChannel.Rows[i].BeginEdit();
					dtChannel.Rows[i]["CheckYn"]="False";
					dtChannel.Rows[i].EndEdit();
				}
			}
			else
			{
				for(int i=0;i < dtChannel.Rows.Count;i++)
				{
					dtChannel.Rows[i].BeginEdit();
					dtChannel.Rows[i]["CheckYn"]="True";
					dtChannel.Rows[i].EndEdit();
				}
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
			DataRow[] foundRows = dt.Select("CheckYn = 'False'");
         
			// if(grdExItemList.CurrentColumn.Position == 0){
			if( foundRows.Length == 0 )
			{
				for(int i=0;i < dt.Rows.Count;i++)
				{
					dt.Rows[i].BeginEdit();
					dt.Rows[i]["CheckYn"]="False";
					dt.Rows[i].EndEdit();
				}
			}
			else
			{
				for(int i=0;i < dt.Rows.Count;i++)
				{
					dt.Rows[i].BeginEdit();
					dt.Rows[i]["CheckYn"]="True";
					dt.Rows[i].EndEdit();
				}
			}
		}


		#endregion

		#region 처리메소드

		/// <summary>
		/// 편성된 광고목록 조회
		/// </summary>
		private void SearchScheduleChoiceAd()
		{
            IsSearching = true;
			StatusMessage("광고 편성현황을 조회합니다.");

			ProgressStart();
			try
			{
				// 모든 체크박스의 체크를 푼다.
				grdExScheduleList.UnCheckAllRecords(); 

				// 데이터모델 초기화
				schChoiceAdModel.Init();

				// 데이터모델에 전송할 내용을 셋트한다.
				if(IsNewSearchKey)  schChoiceAdModel.SearchKey = "";
				else                schChoiceAdModel.SearchKey  = ebSearchKey.Text;

				schChoiceAdModel.SearchMediaCode		= "1";
				schChoiceAdModel.SearchRapCode			=  cbSearchRap.SelectedItem.Value.ToString();
				schChoiceAdModel.SearchAgencyCode		= "00";
				schChoiceAdModel.SearchAdvertiserCode	= "00";
				schChoiceAdModel.SearchContractState	= "00";
				schChoiceAdModel.SearchAdClass			=  cbSearchAdClass.SelectedItem.Value.ToString();  
				if(chkAdState_20.Checked) schChoiceAdModel.SearchchkAdState_20 = "Y";
				if(chkAdState_30.Checked) schChoiceAdModel.SearchchkAdState_30 = "Y";
				if(chkAdState_40.Checked) schChoiceAdModel.SearchchkAdState_40 = "Y";

				new SchChoiceAdManager(systemModel,commonModel).GetSchChoiceAdList(schChoiceAdModel);

                schChoiceAdDs.ChoiceAdSchedule.Clear();
                schChoiceAdDs.Genre.Clear();
                schChoiceAdDs.Channel.Clear();

				if (schChoiceAdModel.ResultCD.Equals("0000"))
				{					
					Utility.SetDataTable(schChoiceAdDs.ChoiceAdSchedule, schChoiceAdModel.ScheduleDataSet);		
					StatusMessage(schChoiceAdModel.ResultCnt + "건의 광고파일 정보가 조회되었습니다.");

					//uiPanelList.Text = "지정광고 편성현황 : " + cbSearchMedia.SelectedItem.Text;
					AddSchChoice();
				}

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
                    keyAckNoCm		= schPublishModel.AckNo;
                    keyAckStateCm	= schPublishModel.State;
					lbMsg.Text		= "";
                }

				//new SchPublishManager(systemModel,commonModel).GetPublishState(schPublishModel,20);
				//if (schPublishModel.ResultCD.Equals("0000"))
				//{
				//    keyAckNoOap		= schPublishModel.AckNo;
				//    keyAckStateOap	= schPublishModel.State;
				//    lbMsg.Text		= "";
				//}

				// 상세정보 표시
				ProgressStop();
				//상세내용 읽어오기 아직 미작업
				SetDetailText();

			}
			catch(FrameException fe)
			{
				FrameSystem.showMsgForm("지정광고 편성현황 조회오류", new string[] {fe.ErrCode, fe.ResultMsg});
			}
			catch(Exception ex)
			{
				FrameSystem.showMsgForm("지정광고 편성현황 조회오류",new string[] {"",ex.Message});
			}
			finally
			{
                IsSearching = false; // 조회중 Flag 리셋
				ProgressStop();
			}
		}

		/// <summary>
		/// 키캆을찾아 그리드 키에 해당되는로우로..
		/// </summary>
		private void AddSchChoice()
		{
			int rowIndex = 0;
			if ( dt.Rows.Count < 1 ) return;
            
			foreach (DataRow row in dt.Rows)
			{					
				if(row["ItemNo"].ToString().Equals(keyItemNo))
				{					
					cm.Position = rowIndex;
					break;								
				}					
				rowIndex++;
			}

			grdExScheduleList.EnsureVisible();
		
		}


		/// <summary>
		/// 지정광고 편성내역 삭제
		/// </summary>
		private void DeleteScheduleChoiceAd()
		{
			StatusMessage("지정광고 편성내역을 삭제합니다.");

			DialogResult result = MessageBox.Show("해당 편성내역을 삭제 하시겠습니까?","광고 편성내역 삭제",MessageBoxButtons.YesNo, MessageBoxIcon.Question,MessageBoxDefaultButton.Button2);

			if (result == DialogResult.No) return;

			DisableButton();

			// 그리드에 변경된 데이터를 Datasource에 업데이트 한다.
			grdExScheduleList.UpdateData();

			try
			{
				int SetCount = 0;

				// 삭제 시킴
				for(int i = 0;i < schChoiceAdDs.ChoiceAdSchedule.Rows.Count;i++)
				{
					DataRow row = schChoiceAdDs.ChoiceAdSchedule.Rows[i];

					Debug.WriteLine( i.ToString() + ":" + row["CheckYn"].ToString() + "|" + row["ItemName"].ToString() );

					if(row["CheckYn"].ToString().Equals("True"))
					{
						schChoiceAdModel.Init();

						// 데이터모델에 전송할 내용을 셋트한다.
						schChoiceAdModel.MediaCode     = row["MediaCode"].ToString();;
						schChoiceAdModel.ItemNo        = row["ItemNo"].ToString();
						schChoiceAdModel.ItemName      = row["ItemName"].ToString();

                        // 지정광고 편성내역 삭제 서비스를 호출한다.
                        new SchChoiceAdManager(systemModel,commonModel).SetSchChoiceAdDelete(schChoiceAdModel);

						if (schChoiceAdModel.ResultCD.Equals("0000"))
						{
							SetCount++;
						}	
					}
				}

				if(SetCount > 0)
				{
					ResetDetailText();

					keyItemNo = schChoiceAdModel.ItemNo;
					SearchScheduleChoiceAd();
					StatusMessage("지정광고 편성내역이 삭제되었습니다.");			
				}	
				else
				{
					MessageBox.Show("선택된 지정광고 편성내역이 없습니다.", "지정광고편성",
						MessageBoxButtons.OK, MessageBoxIcon.Information);

					SetDetailText();
				}			
			}
			catch(FrameException fe)
			{
				FrameSystem.showMsgForm("지정광고 편성내역삭제오류", new string[] {fe.ErrCode, fe.ResultMsg});
			}
			catch(Exception ex)
			{
				FrameSystem.showMsgForm("지정광고 편성내역 삭제오류",new string[] {"",ex.Message});
			}		

		}
		
		/// <summary>
		/// 상위광고목록의 선택광고가 변경될때 해당 광고의 메뉴및 채널편성데이터를 읽어온다
		/// </summary>
		private void SetDetailText()
		{
			int curRow = cm.Position;

			if(curRow >= 0 )
			{
				IsNotLoading = false;	// 조회중 다시 조회되는 것을 방지함.
				try
				{

					uiPanelDetail.Text = ""
						+ "편성광고 : " 
						+ dt.Rows[curRow]["AdType"].ToString() + "|" + dt.Rows[curRow]["AdTypeName"].ToString() + " / ["
						+ dt.Rows[curRow]["ItemNo"].ToString() + "]" 
						+ dt.Rows[curRow]["ItemName"].ToString() + " / "
						+ dt.Rows[curRow]["AdStateName"].ToString() + " / "
						+ dt.Rows[curRow]["FileStateName"].ToString() + " / "
						+ dt.Rows[curRow]["MenuCount"].ToString() + " / "
						+ dt.Rows[curRow]["ChannelCount"].ToString();		

					keyItemNo            = dt.Rows[curRow]["ItemNo"].ToString();
					keyMediaCode         = dt.Rows[curRow]["MediaCode"].ToString();
					keyAdType            = dt.Rows[curRow]["AdType"].ToString();

					lbMsg.Text = "";

                    #region [ 버튼 활성/비활성화 ] 편성상태에 따른 처리는 보류함.
                    if (canCreate)
                    {
                        btnAdd.Enabled = true;
                        btnCopy.Enabled = true;
                    }
                    else
                    {
                        btnAdd.Enabled = false;
                        btnCopy.Enabled = false;
                    }

					//삭제버튼 활성화
					if(canDelete)
					{
						btnDelete.Enabled = true;

						//if ( keyAdType.Equals("10") || keyAdType.Equals("16") || keyAdType.Equals("17") )
						//{
						//    if( keyAckStateCm.Equals("") || keyAckStateCm.Equals("10") || keyAckStateCm.Equals("30") )
						//    {
						//        // 편성중, 편성승인이면 편성가능
						//        btnDelete.Enabled = true;
						//    }
						//    else
						//    {
						//        lbMsg.Text = "편성승인작업중에는 신규편성을 진행할 수 없습니다.";
						//        btnDelete.Enabled = false;
						//    }
						//}
						//else  if ( keyAdType.Equals("11") || keyAdType.Equals("12") || keyAdType.Equals("20") )
						//{
						//    if( keyAckStateOap.Equals("") || keyAckStateOap.Equals("10") || keyAckStateOap.Equals("30") )
						//    {
						//        // 편성중, 편성승인이면 편성가능
						//        btnDelete.Enabled = true;
						//    }
						//    else
						//    {
						//        lbMsg.Text = "편성승인작업중에는 신규편성을 진행할 수 없습니다.";
						//        btnDelete.Enabled = false;
						//    }
						//}
					}

					if(canCreate)
					{
						btnAddMenu.Enabled = true;
						btnAddChannel.Enabled = true;

						//if (keyAdType.Equals("10") || keyAdType.Equals("16") || keyAdType.Equals("17") || keyAdType.Equals("19"))
						//{
						//    if( keyAckStateCm.Equals("") || keyAckStateCm.Equals("10") || keyAckStateCm.Equals("30") )
						//    {
						//        // 편성중, 편성승인이면 편성가능
						//        btnAddMenu.Enabled = true;
						//        btnAddChannel.Enabled = true;
						//    }
						//    else
						//    {
						//        lbMsg.Text = "편성승인작업중에는 신규편성을 진행할 수 없습니다.";
						//        btnAddMenu.Enabled = false;
						//        btnAddChannel.Enabled = false;
						//    }
						//}
						//else if ( keyAdType.Equals("11") || keyAdType.Equals("12") || keyAdType.Equals("20") )
						//{
						//    if( keyAckStateOap.Equals("") || keyAckStateOap.Equals("10") || keyAckStateOap.Equals("30") )
						//    {
						//        // 편성중, 편성승인이면 편성가능
						//        btnAddMenu.Enabled = true;
						//        btnAddChannel.Enabled = true;
						//    }
						//    else
						//    {
						//        lbMsg.Text = "편성승인작업중에는 신규편성을 진행할 수 없습니다.";
						//        btnAddMenu.Enabled = false;
						//        btnAddChannel.Enabled = false;
						//    }
						//}
                    }
                    #endregion

                    // 메뉴 및 채널 편성 정보를 삭제한다.
					schChoiceAdDs.Genre.Clear();
					LoadDetailMenu();

					schChoiceAdDs.Channel.Clear();
					LoadDetailChannel();
				}
				finally
				{
					IsNotLoading = true;
				}
			}

			StatusMessage("준비");
		}

        /// <summary>
        /// 광고당 메뉴편성분 읽어오기
        /// </summary>
		private void LoadDetailMenu()
		{
			try
			{
                grdExGenreList.UnCheckAllRecords();
                schChoiceAdModel.Init();

				// 데이터모델에 전송할 내용을 셋트한다.
				schChoiceAdModel.ItemNo        = keyItemNo;

				// 지정메뉴광고 상세편성내역 조회 서비스를 호출한다.
				new SchChoiceAdManager(systemModel,commonModel).GetSchChoiceMenuDetailList(schChoiceAdModel);

				if (schChoiceAdModel.ResultCD.Equals("0000"))
				{
					Utility.SetDataTableFast(schChoiceAdDs.Genre, schChoiceAdModel.ScheduleDataSet);

					// 삭제버튼 활성화
                    if(canDelete && schChoiceAdDs.Genre.Rows.Count > 0)
                    {
						if( btnDelete.Enabled )	btnDeleteMenu.Enabled = true;
						else					btnDeleteMenu.Enabled = false;
                        uiPanelMenu.Text = "메뉴편성 : " + schChoiceAdDs.Genre.Rows.Count.ToString("##,##0") + "건";
                    }
                    else
                    {
                        btnDeleteMenu.Enabled = false;
                        uiPanelMenu.Text = "메뉴편성 : 없슴";
                    }
				}
			}
			catch(FrameException fe)
			{
				FrameSystem.showMsgForm("광고별편성 조회오류", new string[] {fe.ErrCode, fe.ResultMsg});
			}
			catch(Exception ex)
			{
				FrameSystem.showMsgForm("광고별편성 조회오류",new string[] {"",ex.Message});
			}
			
		}

        /// <summary>
        /// 광고당 채널편성분 읽어오기
        /// </summary>
		private void LoadDetailChannel()
		{
			// 지정채널 상세편성 정보를 조회한다.
			try
			{

				grdExChannelList.UnCheckAllRecords();

				schChoiceAdModel.Init();
				schChoiceAdModel.ItemNo = keyItemNo;

				// 지정메뉴광고 상세편성내역 조회 서비스를 호출한다.
				new SchChoiceAdManager(systemModel,commonModel).GetSchChoiceChannelDetailList(schChoiceAdModel);

				if (schChoiceAdModel.ResultCD.Equals("0000"))
				{
					Utility.SetDataTable(schChoiceAdDs.Channel, schChoiceAdModel.ScheduleDataSet);		
					//삭제버튼 활성화
                    if(canDelete && schChoiceAdDs.Channel.Rows.Count > 0) 
                    {
                        if( btnDelete.Enabled )	btnDeleteChannel.Enabled = true;
						else					btnDeleteChannel.Enabled = false;

                        uiPanelChannel.Text = "타이틀편성 : " + schChoiceAdDs.Channel.Rows.Count.ToString("##,##0") + "건";
                    }
                    else
                    {
                        btnDeleteChannel.Enabled = false;
                        uiPanelChannel.Text = "타이틀편성 : 없슴";
                    }
				}
				// 추가버튼 활성화
				//if(canCreate) btnAddChannel.Enabled = true;
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
		/// 선택된 장르를 지정메뉴광고 상세내역을 삭제
		/// </summary>
		private void DeleteChoiceAdMenuDetail()
		{
			StatusMessage("선택된 지정메뉴광고  편성상세내역을 삭제합니다.");

			// 그리드에 변경된 데이터를 Datasource에 업데이트 한다.
			grdExGenreList.UpdateData();

			DialogResult result = MessageBox.Show("해당 지정메뉴광고 편성내역을 삭제 하시겠습니까?","지정메뉴광고 편성내역 삭제",
				MessageBoxButtons.YesNo, MessageBoxIcon.Question,
				MessageBoxDefaultButton.Button2);

			if (result == DialogResult.No) return;

			try
			{
				//인서트 시킴
				for(int i=0;i < schChoiceAdDs.Genre.Rows.Count;i++)
				{
					DataRow row = schChoiceAdDs.Genre.Rows[i];

					Debug.WriteLine( i.ToString() + ":" + row["CheckYn"].ToString() + "|" + row["GenreCode"].ToString() + "|" + row["GenreName"].ToString());

					if(row["CheckYn"].ToString().Equals("True"))
					{
						schChoiceAdModel.Init();

						schChoiceAdModel.ItemNo    = keyItemNo;
						schChoiceAdModel.MediaCode = keyMediaCode;
						schChoiceAdModel.GenreCode = row["GenreCode"].ToString();
						//schChoiceAdModel.CategoryCode = row["CategoryCode"].ToString();
						schChoiceAdModel.GenreName = row["GenreName"].ToString();

                        new SchChoiceAdManager(systemModel, commonModel).SetSchChoiceMenuDetailDelete(schChoiceAdModel);
					}
				}

				if (schChoiceAdModel.ResultCD.Equals("0000"))
				{
					LoadDetailMenu();
					StatusMessage("지정메뉴광고 편성상세내역이 삭제되었습니다.");			
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
		/// 선택된 채널광고 상세내역을 삭제
		/// </summary>
		private void DeleteChoiceAdChannelDetail()
		{
			StatusMessage("선택된 채널광고  편성상세내역을 삭제합니다.");
			FrameSystem.oLog.Debug("광고별 편성 채널편성 삭제");

			// 그리드에 변경된 데이터를 Datasource에 업데이트 한다.
			grdExChannelList.UpdateData();

			DialogResult result = MessageBox.Show("해당 지정채널광고 편성내역을 삭제 하시겠습니까?","지정채널광고 편성내역 삭제",
				MessageBoxButtons.YesNo, MessageBoxIcon.Question,
				MessageBoxDefaultButton.Button2);

			if (result == DialogResult.No) return;

			try
			{
				//FrameSystem.oLog.Message("광고별 편성 채널편성 삭제 : " + keyItemNo);

				for(int i=0;i < schChoiceAdDs.Channel.Rows.Count;i++)
				{
					DataRow row = schChoiceAdDs.Channel.Rows[i];

					if (row["CheckYn"].ToString().Equals("True"))
					{
						schChoiceAdModel.Init();
						schChoiceAdModel.ItemNo = keyItemNo;
						schChoiceAdModel.MediaCode = keyMediaCode;
						schChoiceAdModel.ChannelNo = row["ChannelNo"].ToString();
						schChoiceAdModel.Title = row["Title"].ToString();

                        new SchChoiceAdManager(systemModel, commonModel).SetSchChoiceChannelDetailDelete(schChoiceAdModel);
						//FrameSystem.oLog.Message(string.Format("ItemNo:{0} Seq:{1} Check:{2} Ch:{3} Title:{4} -> 삭제처리", keyItemNo, i, row["CheckYn"].ToString(), row["ChannelNo"].ToString(), row["Title"].ToString()));
					}
					//else
					//{
						//FrameSystem.oLog.Message(string.Format("ItemNo:{0} Seq:{1} Check:{2} Ch:{3} Title:{4}", keyItemNo, i, row["CheckYn"].ToString(), row["ChannelNo"].ToString(), row["Title"].ToString()));
					//}
				}

				if (schChoiceAdModel.ResultCD.Equals("0000"))
				{
					LoadDetailChannel();
					StatusMessage("지정채널광고 편성상세내역이 삭제되었습니다.");			
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


		private void ResetDetailText()
		{
			keyItemNo            = "";
			uiPanelDetail.Text = "편성광고";

			DisableButton();
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
			
		public void ReloadList()
		{
			SearchScheduleChoiceAd();
		}

		public void ReloadMenuList()
		{
			LoadDetailMenu();
		}

		public void ReloadChannelList()
		{
			LoadDetailChannel();
		}

		#endregion

        private void grdExScheduleList_CellValueChanged(object sender, Janus.Windows.GridEX.ColumnActionEventArgs e)
        {
            if (e.Column.EditType == Janus.Windows.GridEX.EditType.CheckBox)
            {
                int curRow = cm.Position;
                if (curRow >= 0)
                {
                    dt.Rows[curRow].BeginEdit();
                    dt.Rows[curRow]["CheckYn"] = grdExScheduleList.GetValue(e.Column).ToString();
                    dt.Rows[curRow].EndEdit();
                }
            }
        }

        private void grdExGenreList_CellValueChanged(object sender, Janus.Windows.GridEX.ColumnActionEventArgs e)
        {
            if (e.Column.EditType == Janus.Windows.GridEX.EditType.CheckBox)
            {
                int curRow = cmMenu.Position;
                if (curRow >= 0)
                {
                    dtMenu.Rows[curRow].BeginEdit();
                    dtMenu.Rows[curRow]["CheckYn"] = grdExGenreList.GetValue(e.Column).ToString();
                    dtMenu.Rows[curRow].EndEdit();
                }
            }
        }

        private void grdExChannelList_CellValueChanged(object sender, Janus.Windows.GridEX.ColumnActionEventArgs e)
        {
            if (e.Column.EditType == Janus.Windows.GridEX.EditType.CheckBox)
            {
                int curRow = cmChannel.Position;
                if (curRow >= 0)
                {
                    dtChannel.Rows[curRow].BeginEdit();
                    dtChannel.Rows[curRow]["CheckYn"] = grdExChannelList.GetValue(e.Column).ToString();
                    dtChannel.Rows[curRow].EndEdit();
                }
            }
        }

		private void grdExGenreList_FormattingRow(object sender, Janus.Windows.GridEX.RowLoadEventArgs e)
		{

		}
	}
}
