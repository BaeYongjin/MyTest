// ===============================================================================
// SchAppendAdControl for Charites Project
//
// SchAppendAdControl.cs
//
// 추가광고편성 컨트롤을 정의합니다. 
//
// ===============================================================================
// Release history
// 2007.10.02 RH.Jung 파일리스트건수 검사
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
	/// 광고파일관리 컨트롤
	/// </summary>
    public class SchAppendAdControl : System.Windows.Forms.UserControl, IUserControl
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
		SchAppendAdModel schAppendAdModel    = new SchAppendAdModel();	// 추가광고편성모델
		SchPublishModel schPublishModel  = new SchPublishModel();	// 광고승인모델

		// 화면처리용 변수
		CurrencyManager cm        = null;					// 데이터 그리드의 변경에 따른 데이터셋 관리를 위하여			
		DataTable       dt        = null;

        bool IsSearching = false; // 조회중 상세화면이 업데이트 되는 것을 방지 하기위함 2011.11.29 JH.Park
		bool canRead			  = false;
		bool canUpdate			  = false;
		bool canCreate            = false;
		bool canDelete            = false;

		// Key 데이터
		string keyMediaCode     = "";
		string keyItemNo        = "";
		string keyItemName      = "";
		public string keyScheduleOrder = "";		// 팝업에서도 사용하기위해  public
		string keyLastOrder     = "";

		string defMedaiCode     = "";			   //  편성대상광고를 위한 매체코드. 편성목록조회시 셋트

		// 편성배포 승인상태 처리용
		private string keyAckNo    = "";
		private string keyAckState = "";

		// 순위변경구분
		const int ORDER_FIRST = 1;
		const int ORDER_LAST  = 2;
		const int ORDER_UP    = 3;
		const int ORDER_DOWN  = 4;

		// 
		private	const int	FILEMAX	= 430;	// 최대 파일리스트 건수 >= 현재파일리스트 = 추가광고건수 + CDN배포완료 상태파일 건수
		private int FileListCnt = 0;		// 현재파일리스트 건수
		
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
		private Janus.Windows.UI.Dock.UIPanel uiPanelSearch;
		private Janus.Windows.UI.Dock.UIPanelInnerContainer uiPanelSearchContainer;
		private Janus.Windows.UI.Dock.UIPanel uiPanelList;
		private Janus.Windows.UI.Dock.UIPanelInnerContainer uiPanelListContainer;
		private Janus.Windows.UI.Dock.UIPanel uiPanelDetail;
		private Janus.Windows.UI.Dock.UIPanelInnerContainer uiPanelDetailContainer;
		private System.Data.DataView dvSchedule;
		private Janus.Windows.UI.Dock.UIPanelGroup uiPanelHomeAdSchedule;
		private Janus.Windows.EditControls.UIComboBox cbSearchMedia;
		private Janus.Windows.GridEX.GridEX grdExScheduleList;
		private Janus.Windows.EditControls.UIGroupBox gbScheduling;
		private Janus.Windows.EditControls.UIButton btnOrderUp;
		private Janus.Windows.EditControls.UIButton btnOrderDown;
		private Janus.Windows.EditControls.UIButton btnOrderFirst;
		private Janus.Windows.EditControls.UIButton btnOrderLast;
		private Janus.Windows.EditControls.UIButton btnAdd;
		private Janus.Windows.EditControls.UIButton btnDelete;
		private System.Windows.Forms.Label label1;
		private Janus.Windows.GridEX.EditControls.EditBox editBox1;
		private System.Windows.Forms.Panel pnlDetail;
		private AdManagerClient.SchHomeAdDs schAppendAdDs;
		private System.Windows.Forms.Label lbMsg;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label lbFileListCount;
		private System.ComponentModel.IContainer components;

		public SchAppendAdControl()
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SchAppendAdControl));
            this.uiPM = new Janus.Windows.UI.Dock.UIPanelManager(this.components);
            this.uiPanelHomeAdSchedule = new Janus.Windows.UI.Dock.UIPanelGroup();
            this.uiPanelSearch = new Janus.Windows.UI.Dock.UIPanel();
            this.uiPanelSearchContainer = new Janus.Windows.UI.Dock.UIPanelInnerContainer();
            this.pnlSearch = new System.Windows.Forms.Panel();
            this.cbSearchMedia = new Janus.Windows.EditControls.UIComboBox();
            this.btnSearch = new Janus.Windows.EditControls.UIButton();
            this.uiPanelList = new Janus.Windows.UI.Dock.UIPanel();
            this.uiPanelListContainer = new Janus.Windows.UI.Dock.UIPanelInnerContainer();
            this.grdExScheduleList = new Janus.Windows.GridEX.GridEX();
            this.dvSchedule = new System.Data.DataView();
            this.schAppendAdDs = new AdManagerClient.SchHomeAdDs();
            this.uiPanelDetail = new Janus.Windows.UI.Dock.UIPanel();
            this.uiPanelDetailContainer = new Janus.Windows.UI.Dock.UIPanelInnerContainer();
            this.pnlDetail = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.lbFileListCount = new System.Windows.Forms.Label();
            this.lbMsg = new System.Windows.Forms.Label();
            this.gbScheduling = new Janus.Windows.EditControls.UIGroupBox();
            this.btnOrderUp = new Janus.Windows.EditControls.UIButton();
            this.btnOrderDown = new Janus.Windows.EditControls.UIButton();
            this.btnOrderFirst = new Janus.Windows.EditControls.UIButton();
            this.btnOrderLast = new Janus.Windows.EditControls.UIButton();
            this.btnDelete = new Janus.Windows.EditControls.UIButton();
            this.btnAdd = new Janus.Windows.EditControls.UIButton();
            this.label1 = new System.Windows.Forms.Label();
            this.editBox1 = new Janus.Windows.GridEX.EditControls.EditBox();
            ((System.ComponentModel.ISupportInitialize)(this.uiPM)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelHomeAdSchedule)).BeginInit();
            this.uiPanelHomeAdSchedule.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelSearch)).BeginInit();
            this.uiPanelSearch.SuspendLayout();
            this.uiPanelSearchContainer.SuspendLayout();
            this.pnlSearch.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelList)).BeginInit();
            this.uiPanelList.SuspendLayout();
            this.uiPanelListContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdExScheduleList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dvSchedule)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.schAppendAdDs)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelDetail)).BeginInit();
            this.uiPanelDetail.SuspendLayout();
            this.uiPanelDetailContainer.SuspendLayout();
            this.pnlDetail.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gbScheduling)).BeginInit();
            this.gbScheduling.SuspendLayout();
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
            this.uiPanelHomeAdSchedule.Id = new System.Guid("b162c53a-7940-47cd-a0ed-9762c59c50d8");
            this.uiPanelHomeAdSchedule.StaticGroup = true;
            this.uiPanelSearch.Id = new System.Guid("afb3b7d0-aa79-443c-a1e8-3baba38f7a4d");
            this.uiPanelHomeAdSchedule.Panels.Add(this.uiPanelSearch);
            this.uiPanelList.Id = new System.Guid("8e3140b9-bfcb-43af-b84e-2ee6cddf51dc");
            this.uiPanelHomeAdSchedule.Panels.Add(this.uiPanelList);
            this.uiPanelDetail.Id = new System.Guid("b2973a57-d3e1-4d80-9387-7a6dc3332542");
            this.uiPanelHomeAdSchedule.Panels.Add(this.uiPanelDetail);
            this.uiPM.Panels.Add(this.uiPanelHomeAdSchedule);
            // 
            // Design Time Panel Info:
            // 
            this.uiPM.BeginPanelInfo();
            this.uiPM.AddDockPanelInfo(new System.Guid("b162c53a-7940-47cd-a0ed-9762c59c50d8"), Janus.Windows.UI.Dock.PanelGroupStyle.HorizontalTiles, Janus.Windows.UI.Dock.PanelDockStyle.Fill, true, new System.Drawing.Size(1010, 677), true);
            this.uiPM.AddDockPanelInfo(new System.Guid("afb3b7d0-aa79-443c-a1e8-3baba38f7a4d"), new System.Guid("b162c53a-7940-47cd-a0ed-9762c59c50d8"), 42, true);
            this.uiPM.AddDockPanelInfo(new System.Guid("8e3140b9-bfcb-43af-b84e-2ee6cddf51dc"), new System.Guid("b162c53a-7940-47cd-a0ed-9762c59c50d8"), 498, true);
            this.uiPM.AddDockPanelInfo(new System.Guid("b2973a57-d3e1-4d80-9387-7a6dc3332542"), new System.Guid("b162c53a-7940-47cd-a0ed-9762c59c50d8"), 87, true);
            this.uiPM.AddFloatingPanelInfo(new System.Guid("b162c53a-7940-47cd-a0ed-9762c59c50d8"), Janus.Windows.UI.Dock.PanelGroupStyle.HorizontalTiles, true, new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
            this.uiPM.AddFloatingPanelInfo(new System.Guid("8e3140b9-bfcb-43af-b84e-2ee6cddf51dc"), new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
            this.uiPM.AddFloatingPanelInfo(new System.Guid("b2973a57-d3e1-4d80-9387-7a6dc3332542"), new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
            this.uiPM.AddFloatingPanelInfo(new System.Guid("afb3b7d0-aa79-443c-a1e8-3baba38f7a4d"), new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
            this.uiPM.EndPanelInfo();
            // 
            // uiPanelHomeAdSchedule
            // 
            this.uiPanelHomeAdSchedule.ActiveCaptionMode = Janus.Windows.UI.Dock.ActiveCaptionMode.Never;
            this.uiPanelHomeAdSchedule.BackColor = System.Drawing.SystemColors.Window;
            this.uiPanelHomeAdSchedule.CaptionVisible = Janus.Windows.UI.InheritableBoolean.True;
            this.uiPanelHomeAdSchedule.CloseButtonVisible = Janus.Windows.UI.InheritableBoolean.False;
            this.uiPanelHomeAdSchedule.Location = new System.Drawing.Point(0, 0);
            this.uiPanelHomeAdSchedule.Name = "uiPanelHomeAdSchedule";
            this.uiPanelHomeAdSchedule.Size = new System.Drawing.Size(1010, 677);
            this.uiPanelHomeAdSchedule.TabIndex = 4;
            this.uiPanelHomeAdSchedule.Text = "추가광고편성";
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
            this.pnlSearch.Controls.Add(this.cbSearchMedia);
            this.pnlSearch.Controls.Add(this.btnSearch);
            this.pnlSearch.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlSearch.Location = new System.Drawing.Point(0, 0);
            this.pnlSearch.Name = "pnlSearch";
            this.pnlSearch.Size = new System.Drawing.Size(1008, 41);
            this.pnlSearch.TabIndex = 3;
            // 
            // cbSearchMedia
            // 
            this.cbSearchMedia.BackColor = System.Drawing.Color.White;
            this.cbSearchMedia.ComboStyle = Janus.Windows.EditControls.ComboStyle.DropDownList;
            this.cbSearchMedia.Location = new System.Drawing.Point(8, 8);
            this.cbSearchMedia.Name = "cbSearchMedia";
            this.cbSearchMedia.Size = new System.Drawing.Size(120, 21);
            this.cbSearchMedia.TabIndex = 1;
            this.cbSearchMedia.Text = "매체선택";
            this.cbSearchMedia.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            // 
            // btnSearch
            // 
            this.btnSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSearch.Enabled = false;
            this.btnSearch.Location = new System.Drawing.Point(895, 8);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(104, 24);
            this.btnSearch.TabIndex = 2;
            this.btnSearch.Text = "조 회";
            this.btnSearch.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // uiPanelList
            // 
            this.uiPanelList.BackColor = System.Drawing.SystemColors.Window;
            this.uiPanelList.CaptionStyle = Janus.Windows.UI.Dock.PanelCaptionStyle.Dark;
            this.uiPanelList.InnerContainer = this.uiPanelListContainer;
            this.uiPanelList.Location = new System.Drawing.Point(0, 69);
            this.uiPanelList.MinimumSize = new System.Drawing.Size(-1, 100);
            this.uiPanelList.Name = "uiPanelList";
            this.uiPanelList.Size = new System.Drawing.Size(1010, 515);
            this.uiPanelList.TabIndex = 12;
            this.uiPanelList.TabStop = false;
            this.uiPanelList.Text = "추가광고편성현황";
            // 
            // uiPanelListContainer
            // 
            this.uiPanelListContainer.BackColor = System.Drawing.SystemColors.Window;
            this.uiPanelListContainer.Controls.Add(this.grdExScheduleList);
            this.uiPanelListContainer.Location = new System.Drawing.Point(1, 23);
            this.uiPanelListContainer.Name = "uiPanelListContainer";
            this.uiPanelListContainer.Size = new System.Drawing.Size(1008, 491);
            this.uiPanelListContainer.TabIndex = 0;
            // 
            // grdExScheduleList
            // 
            this.grdExScheduleList.AlternatingColors = true;
            this.grdExScheduleList.AutomaticSort = false;
            this.grdExScheduleList.BorderStyle = Janus.Windows.GridEX.BorderStyle.None;
            this.grdExScheduleList.DataSource = this.dvSchedule;
            grdExScheduleList_DesignTimeLayout.LayoutString = resources.GetString("grdExScheduleList_DesignTimeLayout.LayoutString");
            this.grdExScheduleList.DesignTimeLayout = grdExScheduleList_DesignTimeLayout;
            this.grdExScheduleList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdExScheduleList.EditorsControlStyle.ButtonAppearance = Janus.Windows.GridEX.ButtonAppearance.Regular;
            this.grdExScheduleList.EmptyRows = true;
            this.grdExScheduleList.FocusCellFormatStyle.BackColor = System.Drawing.SystemColors.Highlight;
            this.grdExScheduleList.FocusCellFormatStyle.ForeColor = System.Drawing.Color.White;
            this.grdExScheduleList.FocusStyle = Janus.Windows.GridEX.FocusStyle.None;
            this.grdExScheduleList.Font = new System.Drawing.Font("굴림체", 9F);
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
            this.grdExScheduleList.Size = new System.Drawing.Size(1008, 491);
            this.grdExScheduleList.TabIndex = 4;
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
            this.grdExScheduleList.ColumnHeaderClick += new Janus.Windows.GridEX.ColumnActionEventHandler(this.grdExScheduleList_ColumnHeaderClick);
            this.grdExScheduleList.Enter += new System.EventHandler(this.OnGrdRowChanged);
            // 
            // dvSchedule
            // 
            this.dvSchedule.Table = this.schAppendAdDs.HomeAdSchedule;
            // 
            // schAppendAdDs
            // 
            this.schAppendAdDs.DataSetName = "SchAppendAdDs";
            this.schAppendAdDs.Locale = new System.Globalization.CultureInfo("en-US");
            this.schAppendAdDs.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // uiPanelDetail
            // 
            this.uiPanelDetail.AllowResize = Janus.Windows.UI.InheritableBoolean.False;
            this.uiPanelDetail.BackColor = System.Drawing.SystemColors.Window;
            this.uiPanelDetail.CaptionStyle = Janus.Windows.UI.Dock.PanelCaptionStyle.Dark;
            this.uiPanelDetail.InnerContainer = this.uiPanelDetailContainer;
            this.uiPanelDetail.Location = new System.Drawing.Point(0, 588);
            this.uiPanelDetail.Name = "uiPanelDetail";
            this.uiPanelDetail.Size = new System.Drawing.Size(1010, 89);
            this.uiPanelDetail.TabIndex = 14;
            this.uiPanelDetail.TabStop = false;
            this.uiPanelDetail.Text = "편성";
            // 
            // uiPanelDetailContainer
            // 
            this.uiPanelDetailContainer.Controls.Add(this.pnlDetail);
            this.uiPanelDetailContainer.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.uiPanelDetailContainer.Location = new System.Drawing.Point(1, 23);
            this.uiPanelDetailContainer.Name = "uiPanelDetailContainer";
            this.uiPanelDetailContainer.Size = new System.Drawing.Size(1008, 65);
            this.uiPanelDetailContainer.TabIndex = 0;
            // 
            // pnlDetail
            // 
            this.pnlDetail.BackColor = System.Drawing.SystemColors.Window;
            this.pnlDetail.Controls.Add(this.label2);
            this.pnlDetail.Controls.Add(this.lbFileListCount);
            this.pnlDetail.Controls.Add(this.lbMsg);
            this.pnlDetail.Controls.Add(this.gbScheduling);
            this.pnlDetail.Controls.Add(this.btnDelete);
            this.pnlDetail.Controls.Add(this.btnAdd);
            this.pnlDetail.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlDetail.Location = new System.Drawing.Point(0, 0);
            this.pnlDetail.Name = "pnlDetail";
            this.pnlDetail.Size = new System.Drawing.Size(1008, 65);
            this.pnlDetail.TabIndex = 5;
            // 
            // label2
            // 
            this.label2.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.label2.Location = new System.Drawing.Point(232, 32);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(144, 21);
            this.label2.TabIndex = 44;
            this.label2.Text = "현재홈+파일리스트갯수:";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lbFileListCount
            // 
            this.lbFileListCount.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.lbFileListCount.Location = new System.Drawing.Point(376, 32);
            this.lbFileListCount.Name = "lbFileListCount";
            this.lbFileListCount.Size = new System.Drawing.Size(64, 21);
            this.lbFileListCount.TabIndex = 43;
            this.lbFileListCount.Text = "0/0";
            this.lbFileListCount.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lbMsg
            // 
            this.lbMsg.Location = new System.Drawing.Point(8, 8);
            this.lbMsg.Name = "lbMsg";
            this.lbMsg.Size = new System.Drawing.Size(360, 21);
            this.lbMsg.TabIndex = 42;
            this.lbMsg.Text = "편성";
            this.lbMsg.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // gbScheduling
            // 
            this.gbScheduling.Controls.Add(this.btnOrderUp);
            this.gbScheduling.Controls.Add(this.btnOrderDown);
            this.gbScheduling.Controls.Add(this.btnOrderFirst);
            this.gbScheduling.Controls.Add(this.btnOrderLast);
            this.gbScheduling.Location = new System.Drawing.Point(448, 8);
            this.gbScheduling.Name = "gbScheduling";
            this.gbScheduling.Size = new System.Drawing.Size(392, 48);
            this.gbScheduling.TabIndex = 41;
            this.gbScheduling.Text = "편성순서변경";
            // 
            // btnOrderUp
            // 
            this.btnOrderUp.Enabled = false;
            this.btnOrderUp.Location = new System.Drawing.Point(104, 16);
            this.btnOrderUp.Name = "btnOrderUp";
            this.btnOrderUp.Size = new System.Drawing.Size(88, 24);
            this.btnOrderUp.TabIndex = 9;
            this.btnOrderUp.Text = "순서올림";
            this.btnOrderUp.VisualStyle = Janus.Windows.UI.VisualStyle.Office2003;
            this.btnOrderUp.Click += new System.EventHandler(this.btnOrderUp_Click);
            // 
            // btnOrderDown
            // 
            this.btnOrderDown.Enabled = false;
            this.btnOrderDown.Location = new System.Drawing.Point(200, 16);
            this.btnOrderDown.Name = "btnOrderDown";
            this.btnOrderDown.Size = new System.Drawing.Size(88, 24);
            this.btnOrderDown.TabIndex = 10;
            this.btnOrderDown.Text = "순서내림";
            this.btnOrderDown.VisualStyle = Janus.Windows.UI.VisualStyle.Office2003;
            this.btnOrderDown.Click += new System.EventHandler(this.btnOrderDown_Click);
            // 
            // btnOrderFirst
            // 
            this.btnOrderFirst.Enabled = false;
            this.btnOrderFirst.Location = new System.Drawing.Point(8, 16);
            this.btnOrderFirst.Name = "btnOrderFirst";
            this.btnOrderFirst.Size = new System.Drawing.Size(88, 24);
            this.btnOrderFirst.TabIndex = 8;
            this.btnOrderFirst.Text = "첫순서로";
            this.btnOrderFirst.VisualStyle = Janus.Windows.UI.VisualStyle.Office2003;
            this.btnOrderFirst.Click += new System.EventHandler(this.btnOrderFirst_Click);
            // 
            // btnOrderLast
            // 
            this.btnOrderLast.Enabled = false;
            this.btnOrderLast.Location = new System.Drawing.Point(296, 16);
            this.btnOrderLast.Name = "btnOrderLast";
            this.btnOrderLast.Size = new System.Drawing.Size(88, 24);
            this.btnOrderLast.TabIndex = 11;
            this.btnOrderLast.Text = "끝순서로";
            this.btnOrderLast.VisualStyle = Janus.Windows.UI.VisualStyle.Office2003;
            this.btnOrderLast.Click += new System.EventHandler(this.btnOrderLast_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnDelete.Enabled = false;
            this.btnDelete.Location = new System.Drawing.Point(120, 34);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(104, 24);
            this.btnDelete.TabIndex = 7;
            this.btnDelete.Text = "삭 제";
            this.btnDelete.VisualStyle = Janus.Windows.UI.VisualStyle.Office2003;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnAdd
            // 
            this.btnAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnAdd.Enabled = false;
            this.btnAdd.Location = new System.Drawing.Point(8, 34);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(104, 24);
            this.btnAdd.TabIndex = 6;
            this.btnAdd.Text = "추 가";
            this.btnAdd.VisualStyle = Janus.Windows.UI.VisualStyle.Office2003;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
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
            this.editBox1.Size = new System.Drawing.Size(100, 21);
            this.editBox1.TabIndex = 0;
            this.editBox1.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
            // 
            // SchAppendAdControl
            // 
            this.Controls.Add(this.uiPanelHomeAdSchedule);
            this.Name = "SchAppendAdControl";
            this.Size = new System.Drawing.Size(1010, 677);
            this.Load += new System.EventHandler(this.UserControl_Load);
            ((System.ComponentModel.ISupportInitialize)(this.uiPM)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelHomeAdSchedule)).EndInit();
            this.uiPanelHomeAdSchedule.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelSearch)).EndInit();
            this.uiPanelSearch.ResumeLayout(false);
            this.uiPanelSearchContainer.ResumeLayout(false);
            this.pnlSearch.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelList)).EndInit();
            this.uiPanelList.ResumeLayout(false);
            this.uiPanelListContainer.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdExScheduleList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dvSchedule)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.schAppendAdDs)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelDetail)).EndInit();
            this.uiPanelDetail.ResumeLayout(false);
            this.uiPanelDetailContainer.ResumeLayout(false);
            this.pnlDetail.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gbScheduling)).EndInit();
            this.gbScheduling.ResumeLayout(false);
            this.ResumeLayout(false);

		}
		#endregion

		#region 컨트롤 로드
		private void UserControl_Load(object sender, System.EventArgs e)
		{
			// 데이터관리용 객체생성
			dt = ((DataView)grdExScheduleList.DataSource).Table;  
			cm = (CurrencyManager) this.BindingContext[grdExScheduleList.DataSource]; 
			cm.PositionChanged += new System.EventHandler(OnGrdRowChanged); 

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

			// 조회권한 검사
			if(menu.CanRead(MenuCode))
			{
				canRead = true;
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

			ProgressStop();

			if(canRead)
			{
				SearchScheduleAppendAd();
			}

			InitButton();
		}

		private void InitCombo()
		{
			Init_MediaCode();
			InitCombo_Level();
		}

		private void Init_MediaCode()
		{
			// 매체를 조회한다.
			MediaCodeModel mediacodeModel = new MediaCodeModel();
			new MediaCodeManager(systemModel, commonModel).GetMediaCodeList(mediacodeModel);
			
			if (mediacodeModel.ResultCD.Equals("0000"))
			{
				// 데이터셋에 셋팅
				Utility.SetDataTable(schAppendAdDs.Medias, mediacodeModel.MediaCodeDataSet);				
			}

			// 검색조건의 콤보
			this.cbSearchMedia.Items.Clear();
			
			// 콤보박스에 셋트할 코드목록을 담을 Item배열을 선언
			Janus.Windows.EditControls.UIComboBoxItem[] comboItems = new Janus.Windows.EditControls.UIComboBoxItem[mediacodeModel.ResultCnt + 1];

			comboItems[0] = new Janus.Windows.EditControls.UIComboBoxItem("매체선택","00");
			
			for(int i=0;i<mediacodeModel.ResultCnt;i++)
			{
				DataRow row = schAppendAdDs.Medias.Rows[i];

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
				for(int i=0;i < schAppendAdDs.Medias.Rows.Count;i++)
				{
					DataRow row = schAppendAdDs.Medias.Rows[i];					
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
			if(canRead)
			{
				btnSearch.Enabled			= true;
			}
			if(canCreate)
			{
				btnAdd.Enabled			= true;
			}
			
			grdExScheduleList.Focus();

			Application.DoEvents();
		}

		private void DisableButton()
		{
			btnSearch.Enabled			= false;
			btnAdd.Enabled				= false;
			btnDelete.Enabled			= false;
			btnOrderUp.Enabled			= false;
			btnOrderDown.Enabled		= false;
			btnOrderFirst.Enabled		= false;
			btnOrderLast.Enabled		= false;

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
			SearchScheduleAppendAd();
			InitButton();
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
			//  추가광고 대상목록 검색창 
			SchAppendAdSearch_pForm pForm = new SchAppendAdSearch_pForm(this);

			// 매체코드셋트
			pForm.keyMediaCode = defMedaiCode;
			pForm.ShowDialog();            
			pForm.Dispose();
			pForm = null;

			InitButton();			
		}

		/// <summary>
		/// 삭제버튼 클릭
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnDelete_Click(object sender, System.EventArgs e)
		{
			DeleteScheduleAppendAd();
		}

		private void btnOrderFirst_Click(object sender, System.EventArgs e)
		{
			OrderSetScheduleAppendAd(ORDER_FIRST);
		}


		private void btnOrderUp_Click(object sender, System.EventArgs e)
		{
			OrderSetScheduleAppendAd(ORDER_UP);
		}

		private void btnOrderDown_Click(object sender, System.EventArgs e)
		{
			OrderSetScheduleAppendAd(ORDER_DOWN);
		}

		private void btnOrderLast_Click(object sender, System.EventArgs e)
		{
			OrderSetScheduleAppendAd(ORDER_LAST);
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
		/// 광고파일목록 조회
		/// </summary>
		private void SearchScheduleAppendAd()
		{
            IsSearching = true;

			StatusMessage("추가광고 편성현황을 조회합니다.");

			if(cbSearchMedia.SelectedItem.Value.Equals("00")) 
			{
				MessageBox.Show("매체를 선택하여 주시기 바랍니다.","추가광고 편성현황 조회", 
					MessageBoxButtons.OK, MessageBoxIcon.Information );
				return;
			}

			ProgressStart();

			try
			{
				// 모든 체크박스의 체크를 푼다.
				grdExScheduleList.UnCheckAllRecords(); 

				// 추가광고편성현황을 조회한다.

				// 데이터모델 초기화
				schAppendAdModel.Init();
				schAppendAdModel.SearchMediaCode		 =  cbSearchMedia.SelectedItem.Value.ToString(); 

				// 광고파일목록조회 서비스를 호출한다.
				new SchAppendAdManager(systemModel,commonModel).GetSchAppendAdList(schAppendAdModel);

				if (schAppendAdModel.ResultCD.Equals("0000"))
				{
					Utility.SetDataTable(schAppendAdDs.HomeAdSchedule, schAppendAdModel.ScheduleDataSet);		
					StatusMessage(schAppendAdModel.ResultCnt + "건의 광고파일 정보가 조회되었습니다.");

					uiPanelList.Text = "추가광고 편성현황 : " + cbSearchMedia.SelectedItem.Text;
					defMedaiCode     = cbSearchMedia.SelectedItem.Value.ToString();

					// 2007.10.02 
					// 파일리스트건수 표시
					FileListCnt      = schAppendAdModel.FileListCount;
					lbFileListCount.Text = FileListCnt.ToString() + "/" + FILEMAX.ToString(); 

					keyLastOrder = schAppendAdModel.LastOrder;
					AddSchChoice();		
							
				}

				// 편성배포승인 처리상태를 조회한다.
				keyAckNo    = "";
				keyAckState = "";

				schPublishModel.Init();
				schPublishModel.SearchMediaCode		 =  cbSearchMedia.SelectedItem.Value.ToString(); 

				// 현재 승인상태조회 서비스를 호출한다.
				new SchPublishManager(systemModel,commonModel).GetPublishState(schPublishModel,10);


				if (schPublishModel.ResultCD.Equals("0000"))
				{
					keyAckNo    = schPublishModel.AckNo;
					keyAckState = schPublishModel.State;

					if(keyAckState.Equals("10"))	// 승인상태가 10:편성중이면
					{
						lbMsg.Text = "편성 진행중입니다.";
					}
					else if(keyAckState.Equals("20")) // 승인상태가 20:편성승인 상태이면 편성이 불가하다.
					{
						lbMsg.Text = "편성승인 후 검수승인 대기중입니다.";
						canCreate = false;
						canUpdate = false;
						canDelete = false;

						ProgressStop();
						MessageBox.Show("현재 편성승인 후 검수승인 대기상태이므로 편성을 변경할 수 없습니다.", "추가광고편성",
							MessageBoxButtons.OK, MessageBoxIcon.Information);
						
					}
					else if(keyAckState.Equals("25") ) // 승인상태가 25:배포대기 상태이면 편성이 불가하다.
					{
						lbMsg.Text = "검수승인 후 배포승인 대기중입니다.";
						canCreate = false;
						canUpdate = false;
						canDelete = false;

						ProgressStop();
						MessageBox.Show("현재 검수승인 후 배포승인 대기상태이므로 편성을 변경할 수 없습니다.", "추가광고편성",
							MessageBoxButtons.OK, MessageBoxIcon.Information);
						
					}
					else if(keyAckState.Equals("30")) // 승인상태가 30:배포승인 상태이면 신규편성이 가능하다.
					{
						lbMsg.Text = "";
					}
				}

				// 상세내역 표시
				SetDetailText();

			}
			catch(FrameException fe)
			{
				FrameSystem.showMsgForm("추가광고 편성현황 조회오류", new string[] {fe.ErrCode, fe.ResultMsg});
			}
			catch(Exception ex)
			{
				FrameSystem.showMsgForm("추가광고 편성현황 조회오류",new string[] {"",ex.Message});
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
				if(row["ScheduleOrder"].ToString().Equals(keyScheduleOrder))
				{					
					cm.Position = rowIndex;
					break;								
				}				
				rowIndex++;
			}
			grdExScheduleList.EnsureVisible();
		}

		/// <summary>
		/// 추가광고 편성내역 삭제
		/// </summary>
		private void DeleteScheduleAppendAd()
		{
			StatusMessage("추가광고 편성내역을 삭제합니다.");

			DialogResult result = MessageBox.Show("해당 추가광고 편성내역을 삭제 하시겠습니까?","추가광고 편성내역 삭제",
				MessageBoxButtons.YesNo, MessageBoxIcon.Question,
				MessageBoxDefaultButton.Button2);

			if (result == DialogResult.No) return;


			// 그리드에 변경된 데이터를 Datasource에 업데이트 한다.
			grdExScheduleList.UpdateData();

			try
			{
				int SetCount = 0;

				// 삭제 시킴
				// 추가광고는 삭제할때 노출순서도 바꿔줘야 하므로 
				// 삭제순서는 순위가 높은 것부터 낮은 순으로 해야 한다.			
				for(int i = schAppendAdDs.HomeAdSchedule.Rows.Count - 1;i >= 0;i--)
				{

					DataRow row = schAppendAdDs.HomeAdSchedule.Rows[i];

					Debug.WriteLine( i.ToString() + ":" + row["CheckYn"].ToString() + "|" + row["ItemName"].ToString() + "|" + row["ScheduleOrder"].ToString());

					if(row["CheckYn"].ToString().Equals("True"))
					{
						schAppendAdModel.Init();

						// 데이터모델에 전송할 내용을 셋트한다.
						schAppendAdModel.MediaCode     = row["MediaCode"].ToString();
						schAppendAdModel.ItemNo        = row["ItemNo"].ToString();
						schAppendAdModel.ItemName      = row["ItemName"].ToString();
						schAppendAdModel.ScheduleOrder = row["ScheduleOrder"].ToString();

						// 추가광고 편성내역 삭제 서비스를 호출한다.
						new SchAppendAdManager(systemModel,commonModel).SetSchAppendAdDelete(schAppendAdModel);

						if(schAppendAdModel.ResultCD.Equals("0000"))
						{
							SetCount++;
						}
					}
				}

				if(SetCount > 0)
				{
					keyScheduleOrder = schAppendAdModel.ScheduleOrder;
					ResetDetailText();
					DisableButton();
					SearchScheduleAppendAd();
					InitButton();

					StatusMessage("추가광고 편성내역이 삭제되었습니다.");			
				}
				else
				{
					MessageBox.Show("선택된 추가광고 편성내역이 없습니다.", "추가광고편성",
						MessageBoxButtons.OK, MessageBoxIcon.Information);
				}
			}
			catch(FrameException fe)
			{
				FrameSystem.showMsgForm("추가광고 편성내역삭제오류", new string[] {fe.ErrCode, fe.ResultMsg});
			}
			catch(Exception ex)
			{
				FrameSystem.showMsgForm("추가광고 편성내역 삭제오류",new string[] {"",ex.Message});
			}		

		}

		/// <summary>
		/// 추가광고 편성내역 순위변경
		/// </summary>
		private void OrderSetScheduleAppendAd(int OrderSet)
		{
			StatusMessage("추가광고 편성내역의 편성순위를 변경합니다.");

			if(keyItemName.Trim().Length == 0) 
			{
				MessageBox.Show("변경할 추가광고 편성내역이 선택되지 않았습니다.","추가광고 편성내역 순위변경", 
					MessageBoxButtons.OK, MessageBoxIcon.Information );
				return;
			}

			try
			{
				// 데이터모델에 전송할 내용을 셋트한다.
				schAppendAdModel.MediaCode     = keyMediaCode;
				schAppendAdModel.ItemNo        = keyItemNo;
				schAppendAdModel.ItemName      = keyItemName;
				schAppendAdModel.ScheduleOrder = keyScheduleOrder;
	
				int NowOrder  = Convert.ToInt32(keyScheduleOrder);
				int LastOrder = Convert.ToInt32(keyLastOrder);

				switch(OrderSet)
				{
					case ORDER_FIRST:
						if(NowOrder <= 1) 
						{
							MessageBox.Show("해당 추가광고 편성내역이 첫번째 순위입니다.","추가광고 편성내역 순위변경", 
								MessageBoxButtons.OK, MessageBoxIcon.Information );							
							return;
						}
						break;
					case ORDER_UP:
						if(NowOrder <= 1) 
						{
							MessageBox.Show("해당 추가광고 편성내역이 첫번째 순위입니다.","추가광고 편성내역 순위변경", 
								MessageBoxButtons.OK, MessageBoxIcon.Information );							
							return;
						}
						break;
					case ORDER_DOWN:
						if(NowOrder >= LastOrder) 
						{
							MessageBox.Show("해당 추가광고 편성내역이 마지막 순위입니다.","추가광고 편성내역 순위변경", 
								MessageBoxButtons.OK, MessageBoxIcon.Information );							
							return;
						}
						break;
					case ORDER_LAST:
						if(NowOrder >= LastOrder) 
						{
							MessageBox.Show("해당 추가광고 편성내역이 마지막 순위입니다.","추가광고 편성내역 순위변경", 
								MessageBoxButtons.OK, MessageBoxIcon.Information );							
							return;
						}
						break;
				}

				// 추가광고 편성내역 순서변경 서비스를 호출한다.
				new SchAppendAdManager(systemModel,commonModel).SetSchAppendAdOrderSet(schAppendAdModel, OrderSet);
				keyScheduleOrder	=	schAppendAdModel.ScheduleOrder;
				StatusMessage("추가광고 편성내역의 순위가 변경되었습니다.");			

				ResetDetailText();
				DisableButton();
				SearchScheduleAppendAd();
				InitButton();
						
		
			}
			catch(FrameException fe)
			{
				FrameSystem.showMsgForm("추가광고 편성내역 순위변경 오류", new string[] {fe.ErrCode, fe.ResultMsg});
			}
			catch(Exception ex)
			{
				FrameSystem.showMsgForm("추가광고 편성내역 순위변경 오류",new string[] {"",ex.Message});
			}		

		}
		
		/// <summary>
		/// 광고파일 상세정보의 셋트
		/// </summary>
		private void SetDetailText()
		{
			int curRow = cm.Position;

			if(curRow >= 0)
			{
				uiPanelDetail.Text = ""
					+ "편성광고 : " 
					+ dt.Rows[curRow]["AdTypeName"].ToString().Trim() + " / " 
					+ dt.Rows[curRow]["ItemName"].ToString().Trim()
					;

				keyMediaCode     = dt.Rows[curRow]["MediaCode"].ToString();
				keyItemNo        = dt.Rows[curRow]["ItemNo"].ToString();
				keyItemName      = dt.Rows[curRow]["ItemName"].ToString();
				keyScheduleOrder = dt.Rows[curRow]["ScheduleOrder"].ToString();
								
				if(canCreate)
				{
					btnAdd.Enabled		= true;
				}
				if(canDelete)
				{
					btnDelete.Enabled			= true;
				}
				if(canUpdate)
				{
					btnOrderUp.Enabled			= true;
					btnOrderDown.Enabled		= true;
					btnOrderFirst.Enabled		= true;
					btnOrderLast.Enabled		= true;
				}

			}
			Application.DoEvents();

			StatusMessage("준비");
		}

		private void ResetDetailText()
		{
			keyMediaCode     = "";
			keyItemNo        = "";
			keyItemName      = "";
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
			SearchScheduleAppendAd();
		}

		#endregion

	}
}
