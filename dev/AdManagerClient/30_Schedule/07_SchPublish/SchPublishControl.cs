// ===============================================================================
// SchPublishControl for Charites Project
//
// SchPublishControl.cs
//
// 광고파일관리 컨드롤을 정의합니다. 
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
using Excel = Microsoft.Office.Interop.Excel; // 엑셀 참조
using System.Reflection;


namespace AdManagerClient
{	
	/// <summary>
	/// 광고파일관리 컨트롤
	/// </summary>
    public class SchPublishControl : System.Windows.Forms.UserControl, IUserControl
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
		SchPublishModel schPublishModel  = new SchPublishModel();	// 광고파일모델

		// 화면처리용 변수
		CurrencyManager cm        = null;					// 데이터 그리드의 변경에 따른 데이터셋 관리를 위하여			
		DataTable  dt        = null;

		CurrencyManager cmChild        = null;					// 데이터 그리드의 변경에 따른 데이터셋 관리를 위하여			
		DataTable       dtChild        = null;

        bool IsSearching = false; // 조회중 상세화면이 업데이트 되는 것을 방지 하기위함 2011.11.29 JH.Park
		bool canRead			  = false;
		bool canUpdate			  = false;


		private string keyAckNo    = "";
		private string keyAckState = "";
		private string keyPublishDay = "";
		
				
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

		private Janus.Windows.EditControls.UIComboBox cbSearchMedia;
		private Janus.Windows.EditControls.UIButton btnSearch;
		private Janus.Windows.UI.Dock.UIPanelManager uiPM;
		private System.Windows.Forms.Panel pnlSearch;
		private Janus.Windows.UI.Dock.UIPanelGroup uiPanelAdFile;
		private Janus.Windows.UI.Dock.UIPanel uiPanelSearch;
		private Janus.Windows.UI.Dock.UIPanelInnerContainer uiPanelSearchContainer;
		private System.ComponentModel.IContainer components;
		private AdManagerClient.SchPublishDs schPublishDs;
		private Janus.Windows.UI.Dock.UIPanel uiPanel1;
		private Janus.Windows.UI.Dock.UIPanelInnerContainer uiPanel1Container;
		private Janus.Windows.UI.Dock.UIPanel uiPanel2;
		private Janus.Windows.UI.Dock.UIPanelInnerContainer uiPanel2Container;
		private Janus.Windows.UI.Dock.UIPanelGroup uiPanelConfirm;
		private Janus.Windows.UI.Dock.UIPanel uiPanelSchduleConfirm;
		private Janus.Windows.UI.Dock.UIPanelInnerContainer uiPanelSchduleConfirmContainer;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Panel panel2;
		private Janus.Windows.GridEX.GridEX grdExScheduleList;
		private Janus.Windows.GridEX.GridEX grdExPublishList;
		private Janus.Windows.UI.Dock.UIPanel uiPanelMaster;
		private Janus.Windows.UI.Dock.UIPanelInnerContainer uiPanelMasterContainer;
		private System.Windows.Forms.Panel panel3;
		private System.Windows.Forms.Panel panel5;
		private System.Windows.Forms.Panel panel6;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private Janus.Windows.GridEX.EditControls.EditBox ebPublishDesc;
		private Janus.Windows.EditControls.UIButton btnPublishConfirm;
		private System.Windows.Forms.Label lbMemo2;
		private Janus.Windows.GridEX.EditControls.EditBox ebPublishDay;
		private Janus.Windows.GridEX.EditControls.EditBox ebPublishUserName;
		private Janus.Windows.GridEX.EditControls.EditBox ebChkUserName;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Label label7;
		private Janus.Windows.GridEX.EditControls.EditBox ebChkDay;
		private Janus.Windows.GridEX.EditControls.EditBox ebChkDesc;
		private Janus.Windows.EditControls.UIButton btnChkConfirm;
		private Janus.Windows.EditControls.UIButton btnChkCancel;
		private System.Windows.Forms.Label label8;
		private Janus.Windows.EditControls.UIButton btnAckCancel;
		private System.Windows.Forms.Label lbUser1;
		private Janus.Windows.GridEX.EditControls.EditBox ebAckUserName;
		private Janus.Windows.GridEX.EditControls.EditBox ebAckDesc;
		private Janus.Windows.EditControls.UIButton btnAckConfirm;
		private System.Windows.Forms.Label lbMemo1;
		private System.Windows.Forms.Label label1;
		private Janus.Windows.GridEX.EditControls.EditBox ebAckDay;
		private System.Windows.Forms.Label label4;
		private Janus.Windows.GridEX.EditControls.EditBox ebAckStateName;
		private System.Windows.Forms.Label label5;
		private Janus.Windows.GridEX.EditControls.EditBox ebModifyStartDay;
		private System.Windows.Forms.Panel panal4;
		private System.Windows.Forms.Label lbMsg;
		private Janus.Windows.UI.Dock.UIPanel uiPanelPublishConfirm;
		private Janus.Windows.UI.Dock.UIPanelInnerContainer uiPanelPublishConfirmContainer;
		private Janus.Windows.GridEX.GridEX grdExAdStatusList;
		private Janus.Windows.UI.Dock.UIPanel uiPanelSchedule;
		private Janus.Windows.UI.Dock.UIPanelInnerContainer uiPanelScheduleContainer;
		private System.Data.DataView dvSchedule;
		private Janus.Windows.EditControls.UIButton btnPublish_Search;
		private Janus.Windows.EditControls.UIButton btnExcel;
		private System.Data.DataView dvSchPublish;

		public SchPublishControl()
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
            Janus.Windows.GridEX.GridEXLayout grdExPublishList_DesignTimeLayout = new Janus.Windows.GridEX.GridEXLayout();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SchPublishControl));
            Janus.Windows.GridEX.GridEXLayout grdExAdStatusList_DesignTimeLayout = new Janus.Windows.GridEX.GridEXLayout();
            Janus.Windows.GridEX.GridEXLayout grdExScheduleList_DesignTimeLayout = new Janus.Windows.GridEX.GridEXLayout();
            this.uiPM = new Janus.Windows.UI.Dock.UIPanelManager(this.components);
            this.uiPanelAdFile = new Janus.Windows.UI.Dock.UIPanelGroup();
            this.uiPanelSearch = new Janus.Windows.UI.Dock.UIPanel();
            this.uiPanelSearchContainer = new Janus.Windows.UI.Dock.UIPanelInnerContainer();
            this.pnlSearch = new System.Windows.Forms.Panel();
            this.btnSearch = new Janus.Windows.EditControls.UIButton();
            this.cbSearchMedia = new Janus.Windows.EditControls.UIComboBox();
            this.lbMsg = new System.Windows.Forms.Label();
            this.uiPanelConfirm = new Janus.Windows.UI.Dock.UIPanelGroup();
            this.uiPanelSchduleConfirm = new Janus.Windows.UI.Dock.UIPanel();
            this.uiPanelSchduleConfirmContainer = new Janus.Windows.UI.Dock.UIPanelInnerContainer();
            this.panel1 = new System.Windows.Forms.Panel();
            this.grdExPublishList = new Janus.Windows.GridEX.GridEX();
            this.dvSchPublish = new System.Data.DataView();
            this.schPublishDs = new AdManagerClient.SchPublishDs();
            this.uiPanelPublishConfirm = new Janus.Windows.UI.Dock.UIPanel();
            this.uiPanelPublishConfirmContainer = new Janus.Windows.UI.Dock.UIPanelInnerContainer();
            this.panel3 = new System.Windows.Forms.Panel();
            this.btnExcel = new Janus.Windows.EditControls.UIButton();
            this.label4 = new System.Windows.Forms.Label();
            this.ebAckStateName = new Janus.Windows.GridEX.EditControls.EditBox();
            this.label5 = new System.Windows.Forms.Label();
            this.ebModifyStartDay = new Janus.Windows.GridEX.EditControls.EditBox();
            this.ebAckUserName = new Janus.Windows.GridEX.EditControls.EditBox();
            this.ebAckDesc = new Janus.Windows.GridEX.EditControls.EditBox();
            this.btnAckConfirm = new Janus.Windows.EditControls.UIButton();
            this.lbMemo1 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.ebAckDay = new Janus.Windows.GridEX.EditControls.EditBox();
            this.btnAckCancel = new Janus.Windows.EditControls.UIButton();
            this.lbUser1 = new System.Windows.Forms.Label();
            this.btnChkCancel = new Janus.Windows.EditControls.UIButton();
            this.label8 = new System.Windows.Forms.Label();
            this.ebChkUserName = new Janus.Windows.GridEX.EditControls.EditBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.ebChkDay = new Janus.Windows.GridEX.EditControls.EditBox();
            this.ebChkDesc = new Janus.Windows.GridEX.EditControls.EditBox();
            this.btnChkConfirm = new Janus.Windows.EditControls.UIButton();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.ebPublishDesc = new Janus.Windows.GridEX.EditControls.EditBox();
            this.btnPublishConfirm = new Janus.Windows.EditControls.UIButton();
            this.lbMemo2 = new System.Windows.Forms.Label();
            this.ebPublishDay = new Janus.Windows.GridEX.EditControls.EditBox();
            this.ebPublishUserName = new Janus.Windows.GridEX.EditControls.EditBox();
            this.btnPublish_Search = new Janus.Windows.EditControls.UIButton();
            this.uiPanelSchedule = new Janus.Windows.UI.Dock.UIPanel();
            this.uiPanelScheduleContainer = new Janus.Windows.UI.Dock.UIPanelInnerContainer();
            this.grdExAdStatusList = new Janus.Windows.GridEX.GridEX();
            this.dvSchedule = new System.Data.DataView();
            this.uiPanelMaster = new Janus.Windows.UI.Dock.UIPanel();
            this.uiPanelMasterContainer = new Janus.Windows.UI.Dock.UIPanelInnerContainer();
            this.panal4 = new System.Windows.Forms.Panel();
            this.panel5 = new System.Windows.Forms.Panel();
            this.panel6 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.grdExScheduleList = new Janus.Windows.GridEX.GridEX();
            this.uiPanel1 = new Janus.Windows.UI.Dock.UIPanel();
            this.uiPanel1Container = new Janus.Windows.UI.Dock.UIPanelInnerContainer();
            this.uiPanel2 = new Janus.Windows.UI.Dock.UIPanel();
            this.uiPanel2Container = new Janus.Windows.UI.Dock.UIPanelInnerContainer();
            ((System.ComponentModel.ISupportInitialize)(this.uiPM)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelAdFile)).BeginInit();
            this.uiPanelAdFile.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelSearch)).BeginInit();
            this.uiPanelSearch.SuspendLayout();
            this.uiPanelSearchContainer.SuspendLayout();
            this.pnlSearch.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelConfirm)).BeginInit();
            this.uiPanelConfirm.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelSchduleConfirm)).BeginInit();
            this.uiPanelSchduleConfirm.SuspendLayout();
            this.uiPanelSchduleConfirmContainer.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdExPublishList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dvSchPublish)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.schPublishDs)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelPublishConfirm)).BeginInit();
            this.uiPanelPublishConfirm.SuspendLayout();
            this.uiPanelPublishConfirmContainer.SuspendLayout();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelSchedule)).BeginInit();
            this.uiPanelSchedule.SuspendLayout();
            this.uiPanelScheduleContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdExAdStatusList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dvSchedule)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelMaster)).BeginInit();
            this.uiPanelMaster.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdExScheduleList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanel1)).BeginInit();
            this.uiPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanel2)).BeginInit();
            this.uiPanel2.SuspendLayout();
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
            this.uiPanelAdFile.Id = new System.Guid("b162c53a-7940-47cd-a0ed-9762c59c50d8");
            this.uiPanelAdFile.StaticGroup = true;
            this.uiPanelSearch.Id = new System.Guid("afb3b7d0-aa79-443c-a1e8-3baba38f7a4d");
            this.uiPanelAdFile.Panels.Add(this.uiPanelSearch);
            this.uiPanelConfirm.Id = new System.Guid("14c5930c-031d-4da3-9b76-a4aafd861b22");
            this.uiPanelConfirm.StaticGroup = true;
            this.uiPanelSchduleConfirm.Id = new System.Guid("e1f833ff-3c5b-408f-9422-30aa36d1814e");
            this.uiPanelConfirm.Panels.Add(this.uiPanelSchduleConfirm);
            this.uiPanelPublishConfirm.Id = new System.Guid("b2278df3-8e2a-4ffe-98df-e6bce62014d0");
            this.uiPanelConfirm.Panels.Add(this.uiPanelPublishConfirm);
            this.uiPanelAdFile.Panels.Add(this.uiPanelConfirm);
            this.uiPanelSchedule.Id = new System.Guid("e3017d1d-ec9a-4a06-afa9-7bf7c7b4758a");
            this.uiPanelAdFile.Panels.Add(this.uiPanelSchedule);
            this.uiPM.Panels.Add(this.uiPanelAdFile);
            // 
            // Design Time Panel Info:
            // 
            this.uiPM.BeginPanelInfo();
            this.uiPM.AddDockPanelInfo(new System.Guid("b162c53a-7940-47cd-a0ed-9762c59c50d8"), Janus.Windows.UI.Dock.PanelGroupStyle.HorizontalTiles, Janus.Windows.UI.Dock.PanelDockStyle.Fill, true, new System.Drawing.Size(1010, 677), true);
            this.uiPM.AddDockPanelInfo(new System.Guid("afb3b7d0-aa79-443c-a1e8-3baba38f7a4d"), new System.Guid("b162c53a-7940-47cd-a0ed-9762c59c50d8"), 41, true);
            this.uiPM.AddDockPanelInfo(new System.Guid("14c5930c-031d-4da3-9b76-a4aafd861b22"), new System.Guid("b162c53a-7940-47cd-a0ed-9762c59c50d8"), Janus.Windows.UI.Dock.PanelGroupStyle.VerticalTiles, true, 409, true);
            this.uiPM.AddDockPanelInfo(new System.Guid("e1f833ff-3c5b-408f-9422-30aa36d1814e"), new System.Guid("14c5930c-031d-4da3-9b76-a4aafd861b22"), 607, true);
            this.uiPM.AddDockPanelInfo(new System.Guid("b2278df3-8e2a-4ffe-98df-e6bce62014d0"), new System.Guid("14c5930c-031d-4da3-9b76-a4aafd861b22"), 399, true);
            this.uiPM.AddDockPanelInfo(new System.Guid("e3017d1d-ec9a-4a06-afa9-7bf7c7b4758a"), new System.Guid("b162c53a-7940-47cd-a0ed-9762c59c50d8"), 197, true);
            this.uiPM.AddFloatingPanelInfo(new System.Guid("b162c53a-7940-47cd-a0ed-9762c59c50d8"), Janus.Windows.UI.Dock.PanelGroupStyle.HorizontalTiles, true, new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
            this.uiPM.AddFloatingPanelInfo(new System.Guid("8e3140b9-bfcb-43af-b84e-2ee6cddf51dc"), new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
            this.uiPM.AddFloatingPanelInfo(new System.Guid("b2973a57-d3e1-4d80-9387-7a6dc3332542"), new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
            this.uiPM.AddFloatingPanelInfo(new System.Guid("afb3b7d0-aa79-443c-a1e8-3baba38f7a4d"), new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
            this.uiPM.AddFloatingPanelInfo(new System.Guid("14c5930c-031d-4da3-9b76-a4aafd861b22"), Janus.Windows.UI.Dock.PanelGroupStyle.VerticalTiles, true, new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
            this.uiPM.AddFloatingPanelInfo(new System.Guid("e1f833ff-3c5b-408f-9422-30aa36d1814e"), new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
            this.uiPM.AddFloatingPanelInfo(new System.Guid("639e85a8-1091-4e4f-9c71-cba7af3330f5"), new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
            this.uiPM.AddFloatingPanelInfo(new System.Guid("aa97407d-9777-4dac-bc00-3751ab8a3aba"), Janus.Windows.UI.Dock.PanelGroupStyle.HorizontalTiles, true, new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
            this.uiPM.AddFloatingPanelInfo(new System.Guid("3656b1fa-facf-4a8e-9665-cff9e8da0f57"), new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
            this.uiPM.AddFloatingPanelInfo(new System.Guid("97fdb5cb-6280-408b-9821-4851e4735c6b"), new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
            this.uiPM.AddFloatingPanelInfo(new System.Guid("f2f8e0a0-5a41-487e-b61a-c55fd0a9d359"), new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
            this.uiPM.AddFloatingPanelInfo(new System.Guid("a0d9865d-9993-4140-abc8-aef64ac0146a"), new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
            this.uiPM.AddFloatingPanelInfo(new System.Guid("b2278df3-8e2a-4ffe-98df-e6bce62014d0"), new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
            this.uiPM.AddFloatingPanelInfo(new System.Guid("e3017d1d-ec9a-4a06-afa9-7bf7c7b4758a"), new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
            this.uiPM.EndPanelInfo();
            // 
            // uiPanelAdFile
            // 
            this.uiPanelAdFile.ActiveCaptionMode = Janus.Windows.UI.Dock.ActiveCaptionMode.Never;
            this.uiPanelAdFile.AllowResize = Janus.Windows.UI.InheritableBoolean.False;
            this.uiPanelAdFile.BackColor = System.Drawing.SystemColors.Window;
            this.uiPanelAdFile.CloseButtonVisible = Janus.Windows.UI.InheritableBoolean.False;
            this.uiPanelAdFile.Location = new System.Drawing.Point(0, 0);
            this.uiPanelAdFile.Name = "uiPanelAdFile";
            this.uiPanelAdFile.Size = new System.Drawing.Size(1010, 677);
            this.uiPanelAdFile.TabIndex = 4;
            this.uiPanelAdFile.Text = "편성배포승인";
            // 
            // uiPanelSearch
            // 
            this.uiPanelSearch.AllowResize = Janus.Windows.UI.InheritableBoolean.False;
            this.uiPanelSearch.CaptionVisible = Janus.Windows.UI.InheritableBoolean.False;
            this.uiPanelSearch.CloseButtonVisible = Janus.Windows.UI.InheritableBoolean.False;
            this.uiPanelSearch.InnerContainer = this.uiPanelSearchContainer;
            this.uiPanelSearch.Location = new System.Drawing.Point(0, 22);
            this.uiPanelSearch.Name = "uiPanelSearch";
            this.uiPanelSearch.Size = new System.Drawing.Size(1010, 41);
            this.uiPanelSearch.TabIndex = 4;
            this.uiPanelSearch.Text = "검색";
            // 
            // uiPanelSearchContainer
            // 
            this.uiPanelSearchContainer.Controls.Add(this.pnlSearch);
            this.uiPanelSearchContainer.Location = new System.Drawing.Point(1, 1);
            this.uiPanelSearchContainer.Name = "uiPanelSearchContainer";
            this.uiPanelSearchContainer.Size = new System.Drawing.Size(1008, 39);
            this.uiPanelSearchContainer.TabIndex = 0;
            // 
            // pnlSearch
            // 
            this.pnlSearch.BackColor = System.Drawing.SystemColors.Window;
            this.pnlSearch.Controls.Add(this.btnSearch);
            this.pnlSearch.Controls.Add(this.cbSearchMedia);
            this.pnlSearch.Controls.Add(this.lbMsg);
            this.pnlSearch.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlSearch.Location = new System.Drawing.Point(0, 0);
            this.pnlSearch.Name = "pnlSearch";
            this.pnlSearch.Size = new System.Drawing.Size(1008, 39);
            this.pnlSearch.TabIndex = 2;
            // 
            // btnSearch
            // 
            this.btnSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSearch.Enabled = false;
            this.btnSearch.Location = new System.Drawing.Point(895, 8);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(104, 24);
            this.btnSearch.TabIndex = 3;
            this.btnSearch.Text = "조 회";
            this.btnSearch.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // cbSearchMedia
            // 
            this.cbSearchMedia.BackColor = System.Drawing.Color.White;
            this.cbSearchMedia.ComboStyle = Janus.Windows.EditControls.ComboStyle.DropDownList;
            this.cbSearchMedia.Location = new System.Drawing.Point(8, 8);
            this.cbSearchMedia.Name = "cbSearchMedia";
            this.cbSearchMedia.Size = new System.Drawing.Size(160, 21);
            this.cbSearchMedia.TabIndex = 1;
            this.cbSearchMedia.Text = "매체선택";
            this.cbSearchMedia.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            // 
            // lbMsg
            // 
            this.lbMsg.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.lbMsg.Location = new System.Drawing.Point(168, 8);
            this.lbMsg.Name = "lbMsg";
            this.lbMsg.Size = new System.Drawing.Size(504, 21);
            this.lbMsg.TabIndex = 20;
            this.lbMsg.Text = "편성배포승인을 처리합니다.";
            this.lbMsg.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // uiPanelConfirm
            // 
            this.uiPanelConfirm.AllowResize = Janus.Windows.UI.InheritableBoolean.False;
            this.uiPanelConfirm.CaptionVisible = Janus.Windows.UI.InheritableBoolean.False;
            this.uiPanelConfirm.GroupStyle = Janus.Windows.UI.Dock.PanelGroupStyle.VerticalTiles;
            this.uiPanelConfirm.Location = new System.Drawing.Point(0, 67);
            this.uiPanelConfirm.Name = "uiPanelConfirm";
            this.uiPanelConfirm.Size = new System.Drawing.Size(1010, 409);
            this.uiPanelConfirm.TabIndex = 4;
            this.uiPanelConfirm.Text = "편성/배포 승인";
            this.uiPanelConfirm.SelectedPanelChanged += new Janus.Windows.UI.Dock.PanelActionEventHandler(this.uiPanelConfirm_SelectedPanelChanged);
            // 
            // uiPanelSchduleConfirm
            // 
            this.uiPanelSchduleConfirm.InnerContainer = this.uiPanelSchduleConfirmContainer;
            this.uiPanelSchduleConfirm.Location = new System.Drawing.Point(0, 0);
            this.uiPanelSchduleConfirm.Name = "uiPanelSchduleConfirm";
            this.uiPanelSchduleConfirm.Size = new System.Drawing.Size(607, 409);
            this.uiPanelSchduleConfirm.TabIndex = 4;
            this.uiPanelSchduleConfirm.Text = "승인이력";
            // 
            // uiPanelSchduleConfirmContainer
            // 
            this.uiPanelSchduleConfirmContainer.Controls.Add(this.panel1);
            this.uiPanelSchduleConfirmContainer.Location = new System.Drawing.Point(1, 23);
            this.uiPanelSchduleConfirmContainer.Name = "uiPanelSchduleConfirmContainer";
            this.uiPanelSchduleConfirmContainer.Size = new System.Drawing.Size(605, 385);
            this.uiPanelSchduleConfirmContainer.TabIndex = 0;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.Window;
            this.panel1.Controls.Add(this.grdExPublishList);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(605, 385);
            this.panel1.TabIndex = 4;
            // 
            // grdExPublishList
            // 
            this.grdExPublishList.AllowEdit = Janus.Windows.GridEX.InheritableBoolean.False;
            this.grdExPublishList.AlternatingColors = true;
            this.grdExPublishList.BorderStyle = Janus.Windows.GridEX.BorderStyle.None;
            this.grdExPublishList.DataSource = this.dvSchPublish;
            grdExPublishList_DesignTimeLayout.LayoutString = resources.GetString("grdExPublishList_DesignTimeLayout.LayoutString");
            this.grdExPublishList.DesignTimeLayout = grdExPublishList_DesignTimeLayout;
            this.grdExPublishList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdExPublishList.EmptyRows = true;
            this.grdExPublishList.FocusCellFormatStyle.BackColor = System.Drawing.SystemColors.Highlight;
            this.grdExPublishList.FocusCellFormatStyle.ForeColor = System.Drawing.Color.White;
            this.grdExPublishList.FocusStyle = Janus.Windows.GridEX.FocusStyle.None;
            this.grdExPublishList.FrozenColumns = 2;
            this.grdExPublishList.GridLineColor = System.Drawing.Color.Silver;
            this.grdExPublishList.GridLines = Janus.Windows.GridEX.GridLines.Vertical;
            this.grdExPublishList.GridLineStyle = Janus.Windows.GridEX.GridLineStyle.Solid;
            this.grdExPublishList.GroupByBoxVisible = false;
            this.grdExPublishList.HeaderFormatStyle.TextAlignment = Janus.Windows.GridEX.TextAlignment.Center;
            this.grdExPublishList.HideSelection = Janus.Windows.GridEX.HideSelection.HighlightInactive;
            this.grdExPublishList.Location = new System.Drawing.Point(0, 0);
            this.grdExPublishList.Name = "grdExPublishList";
            this.grdExPublishList.ScrollBars = Janus.Windows.GridEX.ScrollBars.Vertical;
            this.grdExPublishList.SelectedInactiveFormatStyle.BackColor = System.Drawing.Color.Gold;
            this.grdExPublishList.Size = new System.Drawing.Size(605, 385);
            this.grdExPublishList.TabIndex = 4;
            this.grdExPublishList.TabKeyBehavior = Janus.Windows.GridEX.TabKeyBehavior.ControlNavigation;
            this.grdExPublishList.ThemedAreas = ((Janus.Windows.GridEX.ThemedArea)(((((((((Janus.Windows.GridEX.ThemedArea.ScrollBars | Janus.Windows.GridEX.ThemedArea.EditControls)
                        | Janus.Windows.GridEX.ThemedArea.Headers)
                        | Janus.Windows.GridEX.ThemedArea.GroupByBox)
                        | Janus.Windows.GridEX.ThemedArea.GroupRows)
                        | Janus.Windows.GridEX.ThemedArea.ControlBorder)
                        | Janus.Windows.GridEX.ThemedArea.Cards)
                        | Janus.Windows.GridEX.ThemedArea.Gridlines)
                        | Janus.Windows.GridEX.ThemedArea.CheckBoxes)));
            this.grdExPublishList.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            this.grdExPublishList.Enter += new System.EventHandler(this.OnGrdRowChanged);
            // 
            // dvSchPublish
            // 
            this.dvSchPublish.Table = this.schPublishDs.SchPublish;
            // 
            // schPublishDs
            // 
            this.schPublishDs.DataSetName = "SchPublishDs";
            this.schPublishDs.Locale = new System.Globalization.CultureInfo("en-US");
            this.schPublishDs.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // uiPanelPublishConfirm
            // 
            this.uiPanelPublishConfirm.InnerContainer = this.uiPanelPublishConfirmContainer;
            this.uiPanelPublishConfirm.Location = new System.Drawing.Point(611, 0);
            this.uiPanelPublishConfirm.Name = "uiPanelPublishConfirm";
            this.uiPanelPublishConfirm.Size = new System.Drawing.Size(399, 409);
            this.uiPanelPublishConfirm.TabIndex = 4;
            this.uiPanelPublishConfirm.Text = "승인처리";
            // 
            // uiPanelPublishConfirmContainer
            // 
            this.uiPanelPublishConfirmContainer.Controls.Add(this.panel3);
            this.uiPanelPublishConfirmContainer.Location = new System.Drawing.Point(1, 23);
            this.uiPanelPublishConfirmContainer.Name = "uiPanelPublishConfirmContainer";
            this.uiPanelPublishConfirmContainer.Size = new System.Drawing.Size(397, 385);
            this.uiPanelPublishConfirmContainer.TabIndex = 0;
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.SystemColors.Window;
            this.panel3.Controls.Add(this.btnExcel);
            this.panel3.Controls.Add(this.label4);
            this.panel3.Controls.Add(this.ebAckStateName);
            this.panel3.Controls.Add(this.label5);
            this.panel3.Controls.Add(this.ebModifyStartDay);
            this.panel3.Controls.Add(this.ebAckUserName);
            this.panel3.Controls.Add(this.ebAckDesc);
            this.panel3.Controls.Add(this.btnAckConfirm);
            this.panel3.Controls.Add(this.lbMemo1);
            this.panel3.Controls.Add(this.label1);
            this.panel3.Controls.Add(this.ebAckDay);
            this.panel3.Controls.Add(this.btnAckCancel);
            this.panel3.Controls.Add(this.lbUser1);
            this.panel3.Controls.Add(this.btnChkCancel);
            this.panel3.Controls.Add(this.label8);
            this.panel3.Controls.Add(this.ebChkUserName);
            this.panel3.Controls.Add(this.label6);
            this.panel3.Controls.Add(this.label7);
            this.panel3.Controls.Add(this.ebChkDay);
            this.panel3.Controls.Add(this.ebChkDesc);
            this.panel3.Controls.Add(this.btnChkConfirm);
            this.panel3.Controls.Add(this.label2);
            this.panel3.Controls.Add(this.label3);
            this.panel3.Controls.Add(this.ebPublishDesc);
            this.panel3.Controls.Add(this.btnPublishConfirm);
            this.panel3.Controls.Add(this.lbMemo2);
            this.panel3.Controls.Add(this.ebPublishDay);
            this.panel3.Controls.Add(this.ebPublishUserName);
            this.panel3.Controls.Add(this.btnPublish_Search);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(0, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(397, 385);
            this.panel3.TabIndex = 5;
            // 
            // btnExcel
            // 
            this.btnExcel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExcel.Enabled = false;
            this.btnExcel.Location = new System.Drawing.Point(200, 320);
            this.btnExcel.Name = "btnExcel";
            this.btnExcel.Size = new System.Drawing.Size(104, 24);
            this.btnExcel.TabIndex = 31;
            this.btnExcel.Text = "EXCEL출력";
            this.btnExcel.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.btnExcel.Click += new System.EventHandler(this.btnExcel_Click);
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(184, 8);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(80, 21);
            this.label4.TabIndex = 21;
            this.label4.Text = "편성시작일시";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ebAckStateName
            // 
            this.ebAckStateName.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ebAckStateName.Font = new System.Drawing.Font("맑은 고딕", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.ebAckStateName.Location = new System.Drawing.Point(88, 8);
            this.ebAckStateName.Name = "ebAckStateName";
            this.ebAckStateName.ReadOnly = true;
            this.ebAckStateName.Size = new System.Drawing.Size(88, 22);
            this.ebAckStateName.TabIndex = 22;
            this.ebAckStateName.TabStop = false;
            this.ebAckStateName.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
            this.ebAckStateName.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(8, 8);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(72, 21);
            this.label5.TabIndex = 20;
            this.label5.Text = "승인상태";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ebModifyStartDay
            // 
            this.ebModifyStartDay.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ebModifyStartDay.Font = new System.Drawing.Font("맑은 고딕", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.ebModifyStartDay.Location = new System.Drawing.Point(264, 8);
            this.ebModifyStartDay.MaxLength = 15;
            this.ebModifyStartDay.Name = "ebModifyStartDay";
            this.ebModifyStartDay.ReadOnly = true;
            this.ebModifyStartDay.Size = new System.Drawing.Size(120, 22);
            this.ebModifyStartDay.TabIndex = 23;
            this.ebModifyStartDay.TabStop = false;
            this.ebModifyStartDay.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
            this.ebModifyStartDay.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            // 
            // ebAckUserName
            // 
            this.ebAckUserName.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ebAckUserName.Font = new System.Drawing.Font("맑은 고딕", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.ebAckUserName.Location = new System.Drawing.Point(88, 32);
            this.ebAckUserName.Name = "ebAckUserName";
            this.ebAckUserName.ReadOnly = true;
            this.ebAckUserName.Size = new System.Drawing.Size(88, 22);
            this.ebAckUserName.TabIndex = 27;
            this.ebAckUserName.TabStop = false;
            this.ebAckUserName.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
            this.ebAckUserName.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            // 
            // ebAckDesc
            // 
            this.ebAckDesc.Font = new System.Drawing.Font("맑은 고딕", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.ebAckDesc.Location = new System.Drawing.Point(88, 56);
            this.ebAckDesc.MaxLength = 100;
            this.ebAckDesc.Multiline = true;
            this.ebAckDesc.Name = "ebAckDesc";
            this.ebAckDesc.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.ebAckDesc.Size = new System.Drawing.Size(296, 33);
            this.ebAckDesc.TabIndex = 22;
            this.ebAckDesc.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
            this.ebAckDesc.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            // 
            // btnAckConfirm
            // 
            this.btnAckConfirm.Enabled = false;
            this.btnAckConfirm.Location = new System.Drawing.Point(88, 96);
            this.btnAckConfirm.Name = "btnAckConfirm";
            this.btnAckConfirm.Size = new System.Drawing.Size(104, 24);
            this.btnAckConfirm.TabIndex = 23;
            this.btnAckConfirm.Text = "편성승인";
            this.btnAckConfirm.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.btnAckConfirm.Click += new System.EventHandler(this.btnAckConfirm_Click);
            // 
            // lbMemo1
            // 
            this.lbMemo1.Location = new System.Drawing.Point(8, 56);
            this.lbMemo1.Name = "lbMemo1";
            this.lbMemo1.Size = new System.Drawing.Size(80, 21);
            this.lbMemo1.TabIndex = 24;
            this.lbMemo1.Text = "편성승인메모";
            this.lbMemo1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(184, 32);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(80, 21);
            this.label1.TabIndex = 25;
            this.label1.Text = "편성승인일시";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ebAckDay
            // 
            this.ebAckDay.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ebAckDay.Font = new System.Drawing.Font("맑은 고딕", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.ebAckDay.Location = new System.Drawing.Point(264, 32);
            this.ebAckDay.MaxLength = 15;
            this.ebAckDay.Name = "ebAckDay";
            this.ebAckDay.ReadOnly = true;
            this.ebAckDay.Size = new System.Drawing.Size(120, 22);
            this.ebAckDay.TabIndex = 28;
            this.ebAckDay.TabStop = false;
            this.ebAckDay.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
            this.ebAckDay.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            // 
            // btnAckCancel
            // 
            this.btnAckCancel.Enabled = false;
            this.btnAckCancel.Location = new System.Drawing.Point(200, 96);
            this.btnAckCancel.Name = "btnAckCancel";
            this.btnAckCancel.Size = new System.Drawing.Size(104, 24);
            this.btnAckCancel.TabIndex = 24;
            this.btnAckCancel.Text = "편성승인취소";
            this.btnAckCancel.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.btnAckCancel.Click += new System.EventHandler(this.btnAckCancel_Click);
            // 
            // lbUser1
            // 
            this.lbUser1.Location = new System.Drawing.Point(8, 32);
            this.lbUser1.Name = "lbUser1";
            this.lbUser1.Size = new System.Drawing.Size(72, 21);
            this.lbUser1.TabIndex = 26;
            this.lbUser1.Text = "편성승인자";
            this.lbUser1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnChkCancel
            // 
            this.btnChkCancel.Enabled = false;
            this.btnChkCancel.Location = new System.Drawing.Point(200, 192);
            this.btnChkCancel.Name = "btnChkCancel";
            this.btnChkCancel.Size = new System.Drawing.Size(104, 24);
            this.btnChkCancel.TabIndex = 27;
            this.btnChkCancel.Text = "검수승인취소";
            this.btnChkCancel.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.btnChkCancel.Click += new System.EventHandler(this.btnChkCancel_Click);
            // 
            // label8
            // 
            this.label8.Location = new System.Drawing.Point(8, 152);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(80, 21);
            this.label8.TabIndex = 26;
            this.label8.Text = "검수승인메모";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ebChkUserName
            // 
            this.ebChkUserName.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ebChkUserName.Font = new System.Drawing.Font("맑은 고딕", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.ebChkUserName.Location = new System.Drawing.Point(88, 128);
            this.ebChkUserName.Name = "ebChkUserName";
            this.ebChkUserName.ReadOnly = true;
            this.ebChkUserName.Size = new System.Drawing.Size(88, 22);
            this.ebChkUserName.TabIndex = 31;
            this.ebChkUserName.TabStop = false;
            this.ebChkUserName.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
            this.ebChkUserName.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            // 
            // label6
            // 
            this.label6.Location = new System.Drawing.Point(8, 128);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(72, 21);
            this.label6.TabIndex = 27;
            this.label6.Text = "검수승인자";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label7
            // 
            this.label7.Location = new System.Drawing.Point(184, 128);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(80, 21);
            this.label7.TabIndex = 28;
            this.label7.Text = "검수승인일시";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ebChkDay
            // 
            this.ebChkDay.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ebChkDay.Font = new System.Drawing.Font("맑은 고딕", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.ebChkDay.Location = new System.Drawing.Point(264, 128);
            this.ebChkDay.MaxLength = 15;
            this.ebChkDay.Name = "ebChkDay";
            this.ebChkDay.ReadOnly = true;
            this.ebChkDay.Size = new System.Drawing.Size(120, 22);
            this.ebChkDay.TabIndex = 29;
            this.ebChkDay.TabStop = false;
            this.ebChkDay.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
            this.ebChkDay.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            // 
            // ebChkDesc
            // 
            this.ebChkDesc.Font = new System.Drawing.Font("맑은 고딕", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.ebChkDesc.Location = new System.Drawing.Point(88, 152);
            this.ebChkDesc.MaxLength = 100;
            this.ebChkDesc.Multiline = true;
            this.ebChkDesc.Name = "ebChkDesc";
            this.ebChkDesc.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.ebChkDesc.Size = new System.Drawing.Size(296, 32);
            this.ebChkDesc.TabIndex = 25;
            this.ebChkDesc.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
            this.ebChkDesc.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            // 
            // btnChkConfirm
            // 
            this.btnChkConfirm.Enabled = false;
            this.btnChkConfirm.Location = new System.Drawing.Point(88, 192);
            this.btnChkConfirm.Name = "btnChkConfirm";
            this.btnChkConfirm.Size = new System.Drawing.Size(104, 24);
            this.btnChkConfirm.TabIndex = 26;
            this.btnChkConfirm.Text = "검수승인";
            this.btnChkConfirm.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.btnChkConfirm.Click += new System.EventHandler(this.btnChkConfirm_Click);
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(8, 224);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(72, 21);
            this.label2.TabIndex = 26;
            this.label2.Text = "배포승인자";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(184, 224);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(80, 21);
            this.label3.TabIndex = 27;
            this.label3.Text = "배포승인일시";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ebPublishDesc
            // 
            this.ebPublishDesc.Font = new System.Drawing.Font("맑은 고딕", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.ebPublishDesc.Location = new System.Drawing.Point(88, 248);
            this.ebPublishDesc.MaxLength = 100;
            this.ebPublishDesc.Multiline = true;
            this.ebPublishDesc.Name = "ebPublishDesc";
            this.ebPublishDesc.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.ebPublishDesc.Size = new System.Drawing.Size(296, 32);
            this.ebPublishDesc.TabIndex = 28;
            this.ebPublishDesc.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
            this.ebPublishDesc.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            // 
            // btnPublishConfirm
            // 
            this.btnPublishConfirm.Enabled = false;
            this.btnPublishConfirm.Location = new System.Drawing.Point(88, 288);
            this.btnPublishConfirm.Name = "btnPublishConfirm";
            this.btnPublishConfirm.Size = new System.Drawing.Size(104, 24);
            this.btnPublishConfirm.TabIndex = 29;
            this.btnPublishConfirm.Text = "배포승인";
            this.btnPublishConfirm.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.btnPublishConfirm.Click += new System.EventHandler(this.btnPublishConfirm_Click);
            // 
            // lbMemo2
            // 
            this.lbMemo2.Location = new System.Drawing.Point(8, 248);
            this.lbMemo2.Name = "lbMemo2";
            this.lbMemo2.Size = new System.Drawing.Size(80, 21);
            this.lbMemo2.TabIndex = 25;
            this.lbMemo2.Text = "배포승인메모";
            this.lbMemo2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ebPublishDay
            // 
            this.ebPublishDay.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ebPublishDay.Font = new System.Drawing.Font("맑은 고딕", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.ebPublishDay.Location = new System.Drawing.Point(264, 224);
            this.ebPublishDay.MaxLength = 15;
            this.ebPublishDay.Name = "ebPublishDay";
            this.ebPublishDay.ReadOnly = true;
            this.ebPublishDay.Size = new System.Drawing.Size(120, 22);
            this.ebPublishDay.TabIndex = 28;
            this.ebPublishDay.TabStop = false;
            this.ebPublishDay.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
            this.ebPublishDay.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            // 
            // ebPublishUserName
            // 
            this.ebPublishUserName.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ebPublishUserName.Font = new System.Drawing.Font("맑은 고딕", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.ebPublishUserName.Location = new System.Drawing.Point(88, 224);
            this.ebPublishUserName.Name = "ebPublishUserName";
            this.ebPublishUserName.ReadOnly = true;
            this.ebPublishUserName.Size = new System.Drawing.Size(88, 22);
            this.ebPublishUserName.TabIndex = 29;
            this.ebPublishUserName.TabStop = false;
            this.ebPublishUserName.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
            this.ebPublishUserName.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            // 
            // btnPublish_Search
            // 
            this.btnPublish_Search.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnPublish_Search.Enabled = false;
            this.btnPublish_Search.Location = new System.Drawing.Point(88, 320);
            this.btnPublish_Search.Name = "btnPublish_Search";
            this.btnPublish_Search.Size = new System.Drawing.Size(104, 24);
            this.btnPublish_Search.TabIndex = 30;
            this.btnPublish_Search.Text = "승인내역조회";
            this.btnPublish_Search.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.btnPublish_Search.Click += new System.EventHandler(this.btnPublish_Search_Click);
            // 
            // uiPanelSchedule
            // 
            this.uiPanelSchedule.InnerContainer = this.uiPanelScheduleContainer;
            this.uiPanelSchedule.Location = new System.Drawing.Point(0, 480);
            this.uiPanelSchedule.Name = "uiPanelSchedule";
            this.uiPanelSchedule.Size = new System.Drawing.Size(1010, 197);
            this.uiPanelSchedule.TabIndex = 4;
            this.uiPanelSchedule.Text = "배포승인내역";
            // 
            // uiPanelScheduleContainer
            // 
            this.uiPanelScheduleContainer.Controls.Add(this.grdExAdStatusList);
            this.uiPanelScheduleContainer.Location = new System.Drawing.Point(1, 23);
            this.uiPanelScheduleContainer.Name = "uiPanelScheduleContainer";
            this.uiPanelScheduleContainer.Size = new System.Drawing.Size(1008, 173);
            this.uiPanelScheduleContainer.TabIndex = 0;
            // 
            // grdExAdStatusList
            // 
            this.grdExAdStatusList.AllowEdit = Janus.Windows.GridEX.InheritableBoolean.False;
            this.grdExAdStatusList.AlternatingColors = true;
            this.grdExAdStatusList.BorderStyle = Janus.Windows.GridEX.BorderStyle.None;
            this.grdExAdStatusList.DataSource = this.dvSchedule;
            grdExAdStatusList_DesignTimeLayout.LayoutString = resources.GetString("grdExAdStatusList_DesignTimeLayout.LayoutString");
            this.grdExAdStatusList.DesignTimeLayout = grdExAdStatusList_DesignTimeLayout;
            this.grdExAdStatusList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdExAdStatusList.EditorsControlStyle.ButtonAppearance = Janus.Windows.GridEX.ButtonAppearance.Regular;
            this.grdExAdStatusList.EmptyRows = true;
            this.grdExAdStatusList.FocusCellFormatStyle.BackColor = System.Drawing.SystemColors.Highlight;
            this.grdExAdStatusList.FocusCellFormatStyle.ForeColor = System.Drawing.Color.White;
            this.grdExAdStatusList.FocusStyle = Janus.Windows.GridEX.FocusStyle.None;
            this.grdExAdStatusList.GridLineColor = System.Drawing.Color.Silver;
            this.grdExAdStatusList.GridLines = Janus.Windows.GridEX.GridLines.Vertical;
            this.grdExAdStatusList.GridLineStyle = Janus.Windows.GridEX.GridLineStyle.Solid;
            this.grdExAdStatusList.GroupByBoxVisible = false;
            this.grdExAdStatusList.HeaderFormatStyle.TextAlignment = Janus.Windows.GridEX.TextAlignment.Center;
            this.grdExAdStatusList.Location = new System.Drawing.Point(0, 0);
            this.grdExAdStatusList.Name = "grdExAdStatusList";
            this.grdExAdStatusList.ScrollBars = Janus.Windows.GridEX.ScrollBars.Both;
            this.grdExAdStatusList.SelectedInactiveFormatStyle.BackColor = System.Drawing.Color.DarkOrange;
            this.grdExAdStatusList.Size = new System.Drawing.Size(1008, 173);
            this.grdExAdStatusList.TabIndex = 32;
            this.grdExAdStatusList.TabKeyBehavior = Janus.Windows.GridEX.TabKeyBehavior.ControlNavigation;
            this.grdExAdStatusList.ThemedAreas = ((Janus.Windows.GridEX.ThemedArea)(((((((((Janus.Windows.GridEX.ThemedArea.ScrollBars | Janus.Windows.GridEX.ThemedArea.EditControls)
                        | Janus.Windows.GridEX.ThemedArea.Headers)
                        | Janus.Windows.GridEX.ThemedArea.GroupByBox)
                        | Janus.Windows.GridEX.ThemedArea.GroupRows)
                        | Janus.Windows.GridEX.ThemedArea.ControlBorder)
                        | Janus.Windows.GridEX.ThemedArea.Cards)
                        | Janus.Windows.GridEX.ThemedArea.Gridlines)
                        | Janus.Windows.GridEX.ThemedArea.CheckBoxes)));
            this.grdExAdStatusList.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            // 
            // dvSchedule
            // 
            this.dvSchedule.Table = this.schPublishDs.AdSchedule;
            // 
            // uiPanelMaster
            // 
            this.uiPanelMaster.InnerContainer = this.uiPanelMasterContainer;
            this.uiPanelMaster.Location = new System.Drawing.Point(0, 0);
            this.uiPanelMaster.Name = "uiPanelMaster";
            this.uiPanelMaster.Size = new System.Drawing.Size(263, 569);
            this.uiPanelMaster.TabIndex = 4;
            this.uiPanelMaster.Text = "승인처리";
            // 
            // uiPanelMasterContainer
            // 
            this.uiPanelMasterContainer.Location = new System.Drawing.Point(0, 0);
            this.uiPanelMasterContainer.Name = "uiPanelMasterContainer";
            this.uiPanelMasterContainer.Size = new System.Drawing.Size(263, 569);
            this.uiPanelMasterContainer.TabIndex = 0;
            // 
            // panal4
            // 
            this.panal4.BackColor = System.Drawing.SystemColors.Window;
            this.panal4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panal4.Location = new System.Drawing.Point(0, 0);
            this.panal4.Name = "panal4";
            this.panal4.Size = new System.Drawing.Size(395, 132);
            this.panal4.TabIndex = 5;
            // 
            // panel5
            // 
            this.panel5.BackColor = System.Drawing.SystemColors.Window;
            this.panel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel5.Location = new System.Drawing.Point(0, 0);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(395, 147);
            this.panel5.TabIndex = 5;
            // 
            // panel6
            // 
            this.panel6.BackColor = System.Drawing.SystemColors.Window;
            this.panel6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel6.Location = new System.Drawing.Point(0, 0);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(395, 191);
            this.panel6.TabIndex = 5;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.SystemColors.Window;
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(158, 569);
            this.panel2.TabIndex = 5;
            // 
            // grdExScheduleList
            // 
            this.grdExScheduleList.AllowEdit = Janus.Windows.GridEX.InheritableBoolean.False;
            this.grdExScheduleList.AlternatingColors = true;
            this.grdExScheduleList.BorderStyle = Janus.Windows.GridEX.BorderStyle.None;
            grdExScheduleList_DesignTimeLayout.LayoutString = resources.GetString("grdExScheduleList_DesignTimeLayout.LayoutString");
            this.grdExScheduleList.DesignTimeLayout = grdExScheduleList_DesignTimeLayout;
            this.grdExScheduleList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdExScheduleList.EditorsControlStyle.ButtonAppearance = Janus.Windows.GridEX.ButtonAppearance.Regular;
            this.grdExScheduleList.EmptyRows = true;
            this.grdExScheduleList.FocusCellFormatStyle.BackColor = System.Drawing.SystemColors.Highlight;
            this.grdExScheduleList.FocusCellFormatStyle.ForeColor = System.Drawing.Color.White;
            this.grdExScheduleList.FocusStyle = Janus.Windows.GridEX.FocusStyle.None;
            this.grdExScheduleList.Font = new System.Drawing.Font("굴림체", 9F);
            this.grdExScheduleList.GridLineColor = System.Drawing.Color.Silver;
            this.grdExScheduleList.GridLines = Janus.Windows.GridEX.GridLines.Vertical;
            this.grdExScheduleList.GridLineStyle = Janus.Windows.GridEX.GridLineStyle.Solid;
            this.grdExScheduleList.GroupByBoxVisible = false;
            this.grdExScheduleList.HeaderFormatStyle.TextAlignment = Janus.Windows.GridEX.TextAlignment.Center;
            this.grdExScheduleList.Location = new System.Drawing.Point(0, 0);
            this.grdExScheduleList.Name = "grdExScheduleList";
            this.grdExScheduleList.SelectedInactiveFormatStyle.BackColor = System.Drawing.Color.DarkOrange;
            this.grdExScheduleList.Size = new System.Drawing.Size(851, 183);
            this.grdExScheduleList.TabIndex = 17;
            this.grdExScheduleList.TabKeyBehavior = Janus.Windows.GridEX.TabKeyBehavior.ControlNavigation;
            this.grdExScheduleList.ThemedAreas = ((Janus.Windows.GridEX.ThemedArea)(((((((((Janus.Windows.GridEX.ThemedArea.ScrollBars | Janus.Windows.GridEX.ThemedArea.EditControls)
                        | Janus.Windows.GridEX.ThemedArea.Headers)
                        | Janus.Windows.GridEX.ThemedArea.GroupByBox)
                        | Janus.Windows.GridEX.ThemedArea.GroupRows)
                        | Janus.Windows.GridEX.ThemedArea.ControlBorder)
                        | Janus.Windows.GridEX.ThemedArea.Cards)
                        | Janus.Windows.GridEX.ThemedArea.Gridlines)
                        | Janus.Windows.GridEX.ThemedArea.CheckBoxes)));
            this.grdExScheduleList.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2003;
            // 
            // uiPanel1
            // 
            this.uiPanel1.InnerContainer = this.uiPanel1Container;
            this.uiPanel1.Location = new System.Drawing.Point(0, 22);
            this.uiPanel1.Name = "uiPanel1";
            this.uiPanel1.Size = new System.Drawing.Size(851, 88);
            this.uiPanel1.TabIndex = 4;
            this.uiPanel1.Text = "Panel 1";
            // 
            // uiPanel1Container
            // 
            this.uiPanel1Container.Location = new System.Drawing.Point(0, 0);
            this.uiPanel1Container.Name = "uiPanel1Container";
            this.uiPanel1Container.Size = new System.Drawing.Size(851, 88);
            this.uiPanel1Container.TabIndex = 0;
            // 
            // uiPanel2
            // 
            this.uiPanel2.InnerContainer = this.uiPanel2Container;
            this.uiPanel2.Location = new System.Drawing.Point(0, 114);
            this.uiPanel2.Name = "uiPanel2";
            this.uiPanel2.Size = new System.Drawing.Size(851, 88);
            this.uiPanel2.TabIndex = 4;
            this.uiPanel2.Text = "Panel 2";
            // 
            // uiPanel2Container
            // 
            this.uiPanel2Container.Location = new System.Drawing.Point(0, 0);
            this.uiPanel2Container.Name = "uiPanel2Container";
            this.uiPanel2Container.Size = new System.Drawing.Size(851, 88);
            this.uiPanel2Container.TabIndex = 0;
            // 
            // SchPublishControl
            // 
            this.Controls.Add(this.uiPanelAdFile);
            this.Font = global::AdManagerClient.Properties.Settings.Default.Font1;
            this.Name = "SchPublishControl";
            this.Size = new System.Drawing.Size(1010, 677);
            this.Load += new System.EventHandler(this.UserControl_Load);
            ((System.ComponentModel.ISupportInitialize)(this.uiPM)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelAdFile)).EndInit();
            this.uiPanelAdFile.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelSearch)).EndInit();
            this.uiPanelSearch.ResumeLayout(false);
            this.uiPanelSearchContainer.ResumeLayout(false);
            this.pnlSearch.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelConfirm)).EndInit();
            this.uiPanelConfirm.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelSchduleConfirm)).EndInit();
            this.uiPanelSchduleConfirm.ResumeLayout(false);
            this.uiPanelSchduleConfirmContainer.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdExPublishList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dvSchPublish)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.schPublishDs)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelPublishConfirm)).EndInit();
            this.uiPanelPublishConfirm.ResumeLayout(false);
            this.uiPanelPublishConfirmContainer.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelSchedule)).EndInit();
            this.uiPanelSchedule.ResumeLayout(false);
            this.uiPanelScheduleContainer.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdExAdStatusList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dvSchedule)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelMaster)).EndInit();
            this.uiPanelMaster.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdExScheduleList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanel1)).EndInit();
            this.uiPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.uiPanel2)).EndInit();
            this.uiPanel2.ResumeLayout(false);
            this.ResumeLayout(false);

		}
		#endregion

		#region 컨트롤 로드
		private void UserControl_Load(object sender, System.EventArgs e)
		{
			Application.DoEvents();

			// 데이터관리용 객체생성
			dt = ((DataView)grdExPublishList.DataSource).Table;  
			dtChild  = ((DataView)grdExAdStatusList.DataSource).Table;

			cm = (CurrencyManager) this.BindingContext[grdExPublishList.DataSource]; 
			cm.PositionChanged += new System.EventHandler(OnGrdRowChanged); 

			cmChild = (CurrencyManager) this.BindingContext[grdExAdStatusList.DataSource]; 
			//cmChild.PositionChanged += new System.EventHandler(OnGrdRowDetailChanged); 

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
		
			// 삭제버튼 활성화
			if(menu.CanDelete(MenuCode))
			{
				//canDelete = true;
			}
			InitButton();
			ProgressStop();

			if(canRead) SearchSchedule();

		}


		private void InitCombo()
		{
			Init_MediaCode();
		}

		private void Init_MediaCode()
		{
			// 매체를 조회한다.
			MediaCodeModel mediacodeModel = new MediaCodeModel();
			new MediaCodeManager(systemModel, commonModel).GetMediaCodeList(mediacodeModel);
			
			if (mediacodeModel.ResultCD.Equals("0000"))
			{
				// 데이터셋에 셋팅
				Utility.SetDataTable(schPublishDs.Medias, mediacodeModel.MediaCodeDataSet);				
			}

			// 검색조건의 콤보
			this.cbSearchMedia.Items.Clear();
			
			// 콤보박스에 셋트할 코드목록을 담을 Item배열을 선언
			Janus.Windows.EditControls.UIComboBoxItem[] comboItems = new Janus.Windows.EditControls.UIComboBoxItem[mediacodeModel.ResultCnt + 1];

			comboItems[0] = new Janus.Windows.EditControls.UIComboBoxItem("매체선택","00");
			
			for(int i=0;i<mediacodeModel.ResultCnt;i++)
			{
				DataRow row = schPublishDs.Medias.Rows[i];

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
				for(int i=0;i < schPublishDs.Medias.Rows.Count;i++)
				{
					DataRow row = schPublishDs.Medias.Rows[i];					
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
			if(canRead)   btnPublish_Search.Enabled = true;		
			Application.DoEvents();	
		}
	

		private void DisableButton()
		{
			btnSearch.Enabled = false;		

			btnAckConfirm.Enabled = false;
			btnPublishConfirm.Enabled = false;
		
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
                SetDetailText();
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
			DisableButton();
			SearchSchedule();
			InitButton();
		}

		private void btnPublish_Search_Click(object sender, System.EventArgs e)
		{
			SearchPublishDetail();
			int curRow = cmChild.Position;       

			if(curRow >= 0)	btnExcel.Enabled = true;		
			else			btnExcel.Enabled = false;
		}

		/// <summary>
		/// 편성승인조회
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnAckConfirm_Click(object sender, System.EventArgs e)
		{
			DisableButton();
			ScheduleAck();
			InitButton();		
		}


		private void btnPublishConfirm_Click(object sender, System.EventArgs e)
		{
			DialogResult result = MessageBox.Show("배포승인하시겠습니까?\n배포승인후 취소할 수 없습니다.","편성배포승인",
				MessageBoxButtons.YesNo, MessageBoxIcon.Question,
				MessageBoxDefaultButton.Button2);

			if (result == DialogResult.No) return;

			DisableButton();
			SchedulePublish();
			InitButton();		
		}	
	

		private void btnAckCancel_Click(object sender, System.EventArgs e)
		{
			DisableButton();
			ScheduleAckCancel();
			InitButton();				
		}

		private void btnChkConfirm_Click(object sender, System.EventArgs e)
		{
			DisableButton();
			ScheduleChk();
			InitButton();		
		
		}

		private void btnChkCancel_Click(object sender, System.EventArgs e)
		{
			DisableButton();
			ScheduleChkCancel();
			InitButton();		
		
		}


		#endregion

		#region 처리메소드

		/// <summary>
		/// 광고승인이력 조회
		/// </summary>
		private void SearchSchedule()
		{
            IsSearching = true;

			StatusMessage("광고편성 승인이력을 조회합니다.");

			if(cbSearchMedia.SelectedItem.Value.Equals("00")) 
			{
				MessageBox.Show("매체를 선택하여 주시기 바랍니다.","광고편성 승인이력 조회", MessageBoxButtons.OK, MessageBoxIcon.Information );
				return;
			}

			ProgressStart();

			try
			{
				//keyAckNo    = "";
				//keyAckState = "";
				schPublishModel.Init();
				schPublishModel.SearchMediaCode		 =  cbSearchMedia.SelectedItem.Value.ToString(); 
			
				// 광고승인이력 조회 서비스를 호출한다.
				new SchPublishManager(systemModel,commonModel).GetSchPublishList(schPublishModel);

				if (schPublishModel.ResultCD.Equals("0000"))
				{
					schPublishDs.SchPublish.Clear();
					Utility.SetDataTable(schPublishDs.SchPublish, schPublishModel.PublishDataSet);
					StatusMessage(schPublishModel.ResultCnt + "건이 조회되었습니다.");

					AddSchChoice();
					SetDetailText();
				}

				schPublishModel.Init();
				schPublishModel.SearchMediaCode		 =  cbSearchMedia.SelectedItem.Value.ToString(); 

			}
			catch(FrameException fe)
			{
				FrameSystem.showMsgForm("광고파일조회오류", new string[] {fe.ErrCode, fe.ResultMsg});
			}
			catch(Exception ex)
			{
				FrameSystem.showMsgForm("광고파일조회오류",new string[] {"",ex.Message});
			}
			finally
			{
                IsSearching = false; // 조회중 Flag 리셋
				ProgressStop();
			}
		}


		/// <summary>
		/// 편성배포승인내역 조회
		/// </summary>
		private void SearchPublishDetail()
		{
			StatusMessage("편성배포승인내역 디테일 정보를 조회합니다.");

			ProgressStart();

			try
			{				
				schPublishModel.Init();
				int curRow = cm.Position;          

				if(curRow < 0) return;
							
				schPublishModel.SearchMediaCode		 =  cbSearchMedia.SelectedItem.Value.ToString();			
				schPublishModel.AckNo = dt.Rows[curRow]["AckNo"].ToString();
			
				// 채널목록조회 서비스를 호출한다.
				new SchPublishManager(systemModel,commonModel).GetScheduleList(schPublishModel);

				if (schPublishModel.ResultCD.Equals("0000"))
				{
					schPublishDs.AdSchedule.Clear();
					Utility.SetDataTable(schPublishDs.AdSchedule, schPublishModel.ScheduleDataSet);
					StatusMessage(schPublishModel.ResultCnt + "건이 조회되었습니다.");
				}				
			}
			catch(FrameException fe)
			{
				FrameSystem.showMsgForm("편성배포승인내역오류", new string[] {fe.ErrCode, fe.ResultMsg});
			}
			catch(Exception ex)
			{
				FrameSystem.showMsgForm("편성배포승인내역오류",new string[] {"",ex.Message});
			}
			finally
			{
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
				if(row["AckNo"].ToString().Equals(keyAckNo))
				{					
					cm.Position = rowIndex;
					break;								
				}				
				rowIndex++;
			}
			grdExPublishList.EnsureVisible();
		}

		
		/// <summary>
		/// 광고파일 상세정보의 셋트
		/// </summary>
		private void SetDetailText()
		{
			int curRow = cm.Position;

			if(curRow >= 0)
			{
				keyAckNo				= dt.Rows[curRow]["AckNo"].ToString();

				ebAckStateName.Text     = dt.Rows[curRow]["AckStateName"].ToString();
				ebModifyStartDay.Text   = dt.Rows[curRow]["ModifyStartDay"].ToString();
				ebAckUserName.Text      = dt.Rows[curRow]["AckUserName"].ToString();
				ebAckDay.Text           = dt.Rows[curRow]["AckDay"].ToString();
				ebAckDesc.Text          = dt.Rows[curRow]["AckDesc"].ToString().Replace("\n","\r\n");
				ebChkUserName.Text      = dt.Rows[curRow]["ChkUserName"].ToString();
				ebChkDay.Text           = dt.Rows[curRow]["ChkDay"].ToString();
				ebChkDesc.Text          = dt.Rows[curRow]["ChkDesc"].ToString().Replace("\n","\r\n");
				ebPublishUserName.Text  = dt.Rows[curRow]["PublishUserName"].ToString();
				ebPublishDay.Text       = dt.Rows[curRow]["PublishDay"].ToString();
				keyPublishDay			= dt.Rows[curRow]["PublishDay"].ToString(); 
				ebPublishDesc.Text      = dt.Rows[curRow]["PublishDesc"].ToString().Replace("\n","\r\n");
				string AckState         = dt.Rows[curRow]["AckState"].ToString();


				// 기본은 수정불가, 승인불가
				btnAckConfirm.Enabled			= false;
				btnAckCancel.Enabled			= false;
				btnChkConfirm.Enabled			= false;
				btnChkCancel.Enabled			= false;
				btnPublishConfirm.Enabled		= false;
				btnExcel.Enabled = false;

				ebAckDesc.ReadOnly     = true;
				ebAckDesc.BackColor    = Color.WhiteSmoke;

				ebChkDesc.ReadOnly     = true;
				ebChkDesc.BackColor    = Color.WhiteSmoke;

				ebPublishDesc.ReadOnly = true;
				ebPublishDesc.BackColor = Color.WhiteSmoke;

				uiPanelSchedule.Text = ""
					+ "배포승인내역 ▶ " 
					+ "승인번호 : " + dt.Rows[curRow]["AckNo"].ToString().Trim() + " / " 
					+ "배포승인일시 : " + dt.Rows[curRow]["PublishDay"].ToString().Trim()
					;
				schPublishDs.AdSchedule.Clear();
				
				if(canUpdate) // 업데이트 권한이 있을때
				{
					// 금번 승인내역과 선택한 승인내역이 같으면
					//if(keyAckNo.Equals(dt.Rows[curRow]["AckNo"].ToString()))
					//{
						if(AckState.Equals("10")) // 금번 승인내역의 상태가 10:편성중 이면 편성승인 가능하다.
						{
							uiPanelPublishConfirm.Text = "승인처리 : 현재 광고편성현황을 편성승인합니다.";

							btnAckConfirm.Enabled			= true;
							ebAckDesc.ReadOnly				= false;
							ebAckDesc.BackColor				= Color.White;
						}
						else if(AckState.Equals("20")) // 금번 승인내역의 상태가 20:편성승인 이면 검수승인 가능하다
						{
							uiPanelPublishConfirm.Text		= "승인처리 : 현재 광고편성현황을 검수승인(배포대기)합니다."; 
							
							btnAckCancel.Enabled			= true;
							ebAckDesc.ReadOnly				= false;
							ebAckDesc.BackColor				= Color.White;

							btnChkConfirm.Enabled			= true;
							ebChkDesc.ReadOnly				= false;
							ebChkDesc.BackColor				= Color.White;
						}
						else if(AckState.Equals("25")) // 금번 승인내역의 상태가 25:배포대기 이면 배포승인 가능하다
						{
							uiPanelPublishConfirm.Text		= "승인처리 : 현재 광고편성현황을 배포승인합니다.";
							
							btnChkCancel.Enabled			= true;
							ebChkDesc.ReadOnly				= false;
							ebChkDesc.BackColor				= Color.White;

							btnPublishConfirm.Enabled		= true;
							ebPublishDesc.ReadOnly			= false;
							ebPublishDesc.BackColor			= Color.White;
						}
						else
						{
							uiPanelPublishConfirm.Text = "승인처리 : 현재 광고편성현황은 배포승인되었습니다.";
						}
					//}
				}
			}
			Application.DoEvents();

			StatusMessage("준비");
		}


		/// <summary>
		/// 광고편성승인
		/// </summary>
		private void ScheduleAck()
		{
			StatusMessage("광고편성승인 처리합니다.");

			if(keyAckNo.Length == 0) 
			{
				MessageBox.Show("선택된 광고승인내역이 없습니다.","광고편성승인",MessageBoxButtons.OK, MessageBoxIcon.Information );
				return;
			}

			try
			{
				schPublishModel.Init();
				schPublishModel.AckNo   = keyAckNo;
				schPublishModel.AckDesc = ebAckDesc.Text;

				// 광고편성승인 처리 서비스를 호출한다.
				new SchPublishManager(systemModel,commonModel).SetScheduleAck(schPublishModel);

				if (schPublishModel.ResultCD.Equals("0000"))
				{
					StatusMessage("광고편성승인 처리되었습니다.");
					SearchSchedule();
				}
			}
			catch(FrameException fe)
			{
				FrameSystem.showMsgForm("광고편성승인 오류", new string[] {fe.ErrCode, fe.ResultMsg});
			}
			catch(Exception ex)
			{
				FrameSystem.showMsgForm("광고편성승인 오류",new string[] {"",ex.Message});
			}
		}


		/// <summary>
		/// 광고편성승인 취소
		/// </summary>
		private void ScheduleAckCancel()
		{
			StatusMessage("광고편성승인 취소합니다.");

			if(keyAckNo.Length == 0) 
			{
				MessageBox.Show("선택된 광고승인내역이 없습니다.","광고편성승인 취소", 
					MessageBoxButtons.OK, MessageBoxIcon.Information );
				return;
			}

			try
			{
				schPublishModel.Init();
				schPublishModel.AckNo   = keyAckNo;
				schPublishModel.AckDesc = ebAckDesc.Text;

				// 광고편성승인 처리 서비스를 호출한다.
				new SchPublishManager(systemModel,commonModel).SetScheduleAckCancel(schPublishModel);

				if (schPublishModel.ResultCD.Equals("0000"))
				{
					StatusMessage("광고편성승인이 취소되었습니다.");
					SearchSchedule();
				}
			}
			catch(FrameException fe)
			{
				FrameSystem.showMsgForm("광고편성승인 취소 오류", new string[] {fe.ErrCode, fe.ResultMsg});
			}
			catch(Exception ex)
			{
				FrameSystem.showMsgForm("광고편성승인 취소 오류",new string[] {"",ex.Message});
			}
		}



		/// <summary>
		/// 광고검수승인
		/// </summary>
		private void ScheduleChk()
		{
			StatusMessage("광고검수승인 처리합니다.");

			if(keyAckNo.Length == 0) 
			{
				MessageBox.Show("선택된 광고승인내역이 없습니다.","광고검수승인", 
					MessageBoxButtons.OK, MessageBoxIcon.Information );
				return;
			}

			try
			{
				schPublishModel.Init();
				schPublishModel.AckNo   = keyAckNo;
				schPublishModel.ChkDesc = ebChkDesc.Text;

				// 광고검수승인 처리 서비스를 호출한다.
				new SchPublishManager(systemModel,commonModel).SetScheduleChk(schPublishModel);

				if (schPublishModel.ResultCD.Equals("0000"))
				{
					StatusMessage("광고검수승인 처리되었습니다.");
					SearchSchedule();
				}
			}
			catch(FrameException fe)
			{
				FrameSystem.showMsgForm("광고검수승인 오류", new string[] {fe.ErrCode, fe.ResultMsg});
			}
			catch(Exception ex)
			{
				FrameSystem.showMsgForm("광고검수승인 오류",new string[] {"",ex.Message});
			}
		}


		/// <summary>
		/// 광고검수승인 취소
		/// </summary>
		private void ScheduleChkCancel()
		{
			StatusMessage("광고검수승인 취소합니다.");

			if(keyAckNo.Length == 0) 
			{
				MessageBox.Show("선택된 광고승인내역이 없습니다.","광고검수승인 취소", 
					MessageBoxButtons.OK, MessageBoxIcon.Information );
				return;
			}

			try
			{
				schPublishModel.Init();
				schPublishModel.AckNo   = keyAckNo;
				schPublishModel.ChkDesc = ebChkDesc.Text;

				// 광고검수승인 취소 처리 서비스를 호출한다.
				new SchPublishManager(systemModel,commonModel).SetScheduleChkCancel(schPublishModel);

				if (schPublishModel.ResultCD.Equals("0000"))
				{
					StatusMessage("광고검수승인이 취소되었습니다.");
					SearchSchedule();
				}
			}
			catch(FrameException fe)
			{
				FrameSystem.showMsgForm("광고검수승인 취소 오류", new string[] {fe.ErrCode, fe.ResultMsg});
			}
			catch(Exception ex)
			{
				FrameSystem.showMsgForm("광고검수승인 취소 오류",new string[] {"",ex.Message});
			}
		}



		/// <summary>
		/// 광고배포승인
		/// </summary>
		private void SchedulePublish()
		{
			StatusMessage("광고배포승인 처리합니다.");

			if(keyAckNo.Length == 0) 
			{
				MessageBox.Show("선택된 광고승인내역이 없습니다.","광고배포승인", MessageBoxButtons.OK, MessageBoxIcon.Information );
				return;
			}

			try
			{
				schPublishModel.Init();
				schPublishModel.AckNo       = keyAckNo;
				schPublishModel.PublishDesc = ebPublishDesc.Text;

				// 광고배포승인 처리 서비스를 호출한다.
				new SchPublishManager(systemModel,commonModel).SetSchedulePublish(schPublishModel);

				if (schPublishModel.ResultCD.Equals("0000"))
				{
					StatusMessage("광고배포승인 처리되었습니다.");
					SearchSchedule();
				}
			}
			catch(FrameException fe)
			{
				FrameSystem.showMsgForm("광고배포승인 오류", new string[] {fe.ErrCode, fe.ResultMsg});
			}
			catch(Exception ex)
			{
				FrameSystem.showMsgForm("광고배포승인 오류",new string[] {"",ex.Message});
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

			Excel.Application xlApp = null;
			Excel._Workbook oWB = null;
			Excel._Worksheet oSheet = null;
			Excel.Range oRng = null;

			try
			{

				int ColMax = 14; // 컬럼수   				

				int TitleRow = 1;
				int ConditionRow1 = 2;
				int ConditionRow = 3;
				int HeaderRow = 4;
				string StartCol = "A";
				string EndCol = "";
				string TitleCol = "N";
				int CondCount = 0;
				int HeaderCount = 0;

				// 마지막 컬럼의 인덱스문자
				EndCol = GetColumnIndex(ColMax);

				#region [엑셀 오브젝트]
				xlApp = new Excel.Application();
				oWB = (Excel._Workbook)(xlApp.Workbooks.Add(Missing.Value));
				oSheet = (Excel._Worksheet)oWB.ActiveSheet;
				oSheet.Name = "편성배포승인내역";
				#endregion

				#region [타이틀 및 조건 부분]
				// 타이틀 작성
				oSheet.Cells[TitleRow, 1] = "편성배포승인내역";
				oRng = oSheet.get_Range(StartCol + Convert.ToString(TitleRow), TitleCol + Convert.ToString(TitleRow));
				oRng.Merge(true);
				oRng.Font.Bold = true;
				oRng.Font.Size = 16;
				oRng.VerticalAlignment = Excel.XlVAlign.xlVAlignCenter;
				oRng.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;

				// 조건정보 작성
				oSheet.Cells[ConditionRow1 + CondCount, 1] = "승인번호";
				oRng = oSheet.get_Range("B" + Convert.ToString(ConditionRow1 + CondCount), TitleCol + Convert.ToString(ConditionRow + CondCount));
				oRng.Merge(true);

				string AckNo = keyAckNo;
				oSheet.Cells[ConditionRow1 + CondCount, 2] = keyAckNo;

				// 조건부 테두리
				oRng = oSheet.get_Range(StartCol + Convert.ToString(ConditionRow1), TitleCol + Convert.ToString(ConditionRow1 + (CondCount - 1)));
				oRng.VerticalAlignment = Excel.XlVAlign.xlVAlignCenter;
				oRng.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;

				oRng.Borders.LineStyle = Excel.XlLineStyle.xlContinuous;	// 테두리속성 아래에 실선
				oRng.Borders.Weight = Excel.XlBorderWeight.xlThin;		// 테두리속성 아래에 가는선

				// 조건정보 작성
				oSheet.Cells[ConditionRow + CondCount, 1] = "승인일시";
				oRng = oSheet.get_Range("B" + Convert.ToString(ConditionRow + CondCount), TitleCol + Convert.ToString(ConditionRow + CondCount));
				oRng.Merge(true);
				string ReportType = keyPublishDay;

				oSheet.Cells[ConditionRow + CondCount, 2] = ReportType;
				CondCount++;

				// 조건부 테두리
				oRng = oSheet.get_Range(StartCol + Convert.ToString(ConditionRow), TitleCol + Convert.ToString(ConditionRow + (CondCount - 1)));
				oRng.VerticalAlignment = Excel.XlVAlign.xlVAlignCenter;
				oRng.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;

				oRng.Borders.LineStyle = Excel.XlLineStyle.xlContinuous;	// 테두리속성 아래에 실선
				oRng.Borders.Weight = Excel.XlBorderWeight.xlThin;		// 테두리속성 아래에 가는선
				#endregion

				#region [헤더부분]
				// 헤더 정보 작성
				HeaderCount = 1;
				oSheet.Cells[HeaderRow, HeaderCount++] = "구분";
				oSheet.Cells[HeaderRow, HeaderCount++] = "CUG";
				oSheet.Cells[HeaderRow, HeaderCount++] = "카테고리";
				oSheet.Cells[HeaderRow, HeaderCount++] = "장르";
				oSheet.Cells[HeaderRow, HeaderCount++] = "채널";
				oSheet.Cells[HeaderRow, HeaderCount++] = "회차";
				oSheet.Cells[HeaderRow, HeaderCount++] = "지정";
				oSheet.Cells[HeaderRow, HeaderCount++] = "광고";
				oSheet.Cells[HeaderRow, HeaderCount++] = "광고명";
				oSheet.Cells[HeaderRow, HeaderCount++] = "SEQ";
				oSheet.Cells[HeaderRow, HeaderCount++] = "종류";
				oSheet.Cells[HeaderRow, HeaderCount++] = "상태";
				oSheet.Cells[HeaderRow, HeaderCount++] = "배포";
				oSheet.Cells[HeaderRow, HeaderCount++] = "파일명";


				oRng = oSheet.get_Range(StartCol + Convert.ToString(HeaderRow), EndCol + Convert.ToString(HeaderRow)); // 헤더의 범위
				oRng.Font.Bold = true;							// 폰트 굵게
				oRng.VerticalAlignment = Excel.XlVAlign.xlVAlignCenter;	// 세로중앙정렬
				oRng.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;	// 가로중앙정렬	 
				oRng.Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.CornflowerBlue);   //셀 배경색 
				oRng.Font.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.White);	  //텍스트색			
				#endregion

				string[,] items = new string[schPublishDs.Tables[2].Rows.Count, 14];
				// 데이터 추출
				for (int inx = 0; inx < schPublishDs.Tables[2].Rows.Count; inx++)
				{
					items[inx, 0] = schPublishDs.Tables[2].Rows[inx]["SchTypeNm"].ToString();
					items[inx, 1] = schPublishDs.Tables[2].Rows[inx]["CugNm"].ToString();
					items[inx, 2] = schPublishDs.Tables[2].Rows[inx]["CategoryNm"].ToString();
					items[inx, 3] = schPublishDs.Tables[2].Rows[inx]["GenreNm"].ToString();
					items[inx, 4] = schPublishDs.Tables[2].Rows[inx]["ChannelNm"].ToString();
					items[inx, 5] = schPublishDs.Tables[2].Rows[inx]["Series"].ToString();
					items[inx, 6] = schPublishDs.Tables[2].Rows[inx]["IsExclusive"].ToString();
					items[inx, 7] = schPublishDs.Tables[2].Rows[inx]["ItemNo"].ToString();
					items[inx, 8] = schPublishDs.Tables[2].Rows[inx]["ItemName"].ToString();

					items[inx, 9] = schPublishDs.Tables[2].Rows[inx]["SchSeq"].ToString();
					items[inx, 10] = schPublishDs.Tables[2].Rows[inx]["AdType"].ToString();
					items[inx, 11] = schPublishDs.Tables[2].Rows[inx]["AdStatus"].ToString();
					items[inx, 12] = schPublishDs.Tables[2].Rows[inx]["FileStatus"].ToString();
					items[inx, 13] = schPublishDs.Tables[2].Rows[inx]["FileName"].ToString();

				}
				oSheet.get_Range("A5", "N" + Convert.ToString((items.Length / 14) + HeaderRow)).set_Value(Missing.Value, items);

				oRng = oSheet.get_Range(StartCol + Convert.ToString(HeaderRow), EndCol + Convert.ToString(schPublishDs.Tables[2].Rows.Count + HeaderRow));	// 데이터의 범위
				oRng.EntireColumn.AutoFit();					// 데이터의 크기에 셀의 가로크기 맞춤

				oRng.Borders.LineStyle = Excel.XlLineStyle.xlContinuous;	// 테두리속성 아래에 실선
				oRng.Borders.Weight = Excel.XlBorderWeight.xlThin;		// 테두리속성 아래에 가는선

				oRng = oSheet.get_Range("B2", "O2");
				oRng.EntireColumn.AutoFit();

				xlApp.Visible = true;
				xlApp.UserControl = true;


			}
			catch (Exception ex)
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

        private void uiPanelConfirm_SelectedPanelChanged(object sender, Janus.Windows.UI.Dock.PanelActionEventArgs e)
        {

        }

		
	}
}
