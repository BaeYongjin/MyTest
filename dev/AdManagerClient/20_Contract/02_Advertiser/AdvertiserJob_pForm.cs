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
    /// ContractJob_pForm에 대한 요약 설명입니다.
    /// </summary>
    /// 

    public class AdvertiserJob_pForm : System.Windows.Forms.Form
    {

        #region 이벤트핸들러
        public event StatusEventHandler 			StatusEvent;			// 상태이벤트 핸들러
        #endregion
			
        #region 사용자정의 객체 및 변수

        // 시스템 정보 : 화면공통
        private SystemModel   systemModel   = FrameSystem.oSysModel;
        private CommonModel   commonModel   = FrameSystem.oComModel;
        private Logger        log           = FrameSystem.oLog;

        // 사용할 정보모델        
		AdvertiserModel  model   = new AdvertiserModel();	// 광고주 정보 모델

		// 이 창을 연 컨트롤
		AdvertiserControl Opener = null;

        // 화면처리용 변수
        public String MediaCode   = null;
        CurrencyManager cmLevel1  = null;					// 데이터 그리드의 변경에 따른 데이터셋 관리를 위하여			
        DataTable       dtLevel1  = null;
        CurrencyManager cmLevel2  = null;
        DataTable       dtLevel2  = null;
		
        //bool IsNewSearchKey		  = true;					// 검색어입력 여부
        int keyCode = 0;
        #endregion

		#region  생성자 소멸자 컨트롤선언

        private System.Windows.Forms.Panel pnlBtn;
		private Janus.Windows.UI.Dock.UIPanelManager uiPM;
		private Janus.Windows.UI.Dock.UIPanel uiPanel1;
		private Janus.Windows.UI.Dock.UIPanelInnerContainer uiPanel1Container;
		private Janus.Windows.UI.Dock.UIPanel uiPanel2;
		private Janus.Windows.UI.Dock.UIPanelInnerContainer uiPanel2Container;
        private System.Windows.Forms.Panel pnlSearch;
		private Janus.Windows.UI.Dock.UIPanelGroup uiPanel0;
		private Janus.Windows.UI.Dock.UIPanel uiPanel3;
		private Janus.Windows.UI.Dock.UIPanelInnerContainer uiPanel3Container;
		private Janus.Windows.UI.Dock.UIPanel uiPanel4;
		private Janus.Windows.UI.Dock.UIPanelInnerContainer uiPanel4Container;
		private Janus.Windows.EditControls.UIButton uiButton1;
        private Janus.Windows.EditControls.UIButton uiButton2;
        private Label label1;
        private Janus.Windows.GridEX.EditControls.EditBox ebJobName2;
        private Janus.Windows.GridEX.EditControls.EditBox ebJobName1;
        private Janus.Windows.GridEX.EditControls.EditBox ebJobCode;
        private Janus.Windows.GridEX.GridEX grdExLevel1;
        private Janus.Windows.GridEX.GridEX grdExLevel2;
        private Advertiser_pDs advertiser_pDs1;
        private DataView dvJobLevel1;
        private DataView dvJonLevel2;
        private System.ComponentModel.IContainer components;

        /// <summary>
        /// 데이터 넘겨야 할 넘
        /// </summary>
        /// <param name="sender"></param>
        public AdvertiserJob_pForm(UserControl parent)
        {
            //
            // Windows Form 디자이너 지원에 필요합니다.
            //
            InitializeComponent();

			// 이창을 호출한 컨트롤
			Opener = (AdvertiserControl) parent;
			model.Init();
		}

        /// <summary>
        /// 일반사용자
        /// </summary>
        public AdvertiserJob_pForm()
        {
            //
            // Windows Form 디자이너 지원에 필요합니다.
            //
            InitializeComponent();

            model.Init();
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

        #region Windows Form 디자이너에서 생성한 코드
        /// <summary>
        /// 디자이너 지원에 필요한 메서드입니다.
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마십시오.
        /// </summary>
        private void InitializeComponent()
        {
            Janus.Windows.GridEX.GridEXLayout grdExLevel1_DesignTimeLayout = new Janus.Windows.GridEX.GridEXLayout();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AdvertiserJob_pForm));
            Janus.Windows.GridEX.GridEXLayout grdExLevel1_Layout_0 = new Janus.Windows.GridEX.GridEXLayout();
            Janus.Windows.GridEX.GridEXLayout grdExLevel2_DesignTimeLayout = new Janus.Windows.GridEX.GridEXLayout();
            Janus.Windows.GridEX.GridEXLayout grdExLevel2_Layout_0 = new Janus.Windows.GridEX.GridEXLayout();
            this.pnlBtn = new System.Windows.Forms.Panel();
            this.uiButton1 = new Janus.Windows.EditControls.UIButton();
            this.uiButton2 = new Janus.Windows.EditControls.UIButton();
            this.uiPM = new Janus.Windows.UI.Dock.UIPanelManager();
            this.uiPanel0 = new Janus.Windows.UI.Dock.UIPanelGroup();
            this.uiPanel3 = new Janus.Windows.UI.Dock.UIPanel();
            this.uiPanel3Container = new Janus.Windows.UI.Dock.UIPanelInnerContainer();
            this.grdExLevel1 = new Janus.Windows.GridEX.GridEX();
            this.dvJobLevel1 = new System.Data.DataView();
            this.advertiser_pDs1 = new AdManagerClient.Advertiser_pDs();
            this.uiPanel4 = new Janus.Windows.UI.Dock.UIPanel();
            this.uiPanel4Container = new Janus.Windows.UI.Dock.UIPanelInnerContainer();
            this.grdExLevel2 = new Janus.Windows.GridEX.GridEX();
            this.dvJonLevel2 = new System.Data.DataView();
            this.uiPanel1 = new Janus.Windows.UI.Dock.UIPanel();
            this.uiPanel1Container = new Janus.Windows.UI.Dock.UIPanelInnerContainer();
            this.uiPanel2 = new Janus.Windows.UI.Dock.UIPanel();
            this.uiPanel2Container = new Janus.Windows.UI.Dock.UIPanelInnerContainer();
            this.pnlSearch = new System.Windows.Forms.Panel();
            this.ebJobName2 = new Janus.Windows.GridEX.EditControls.EditBox();
            this.ebJobName1 = new Janus.Windows.GridEX.EditControls.EditBox();
            this.ebJobCode = new Janus.Windows.GridEX.EditControls.EditBox();
            this.label1 = new System.Windows.Forms.Label();
            this.pnlBtn.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uiPM)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanel0)).BeginInit();
            this.uiPanel0.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanel3)).BeginInit();
            this.uiPanel3.SuspendLayout();
            this.uiPanel3Container.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdExLevel1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dvJobLevel1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.advertiser_pDs1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanel4)).BeginInit();
            this.uiPanel4.SuspendLayout();
            this.uiPanel4Container.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdExLevel2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dvJonLevel2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanel1)).BeginInit();
            this.uiPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanel2)).BeginInit();
            this.uiPanel2.SuspendLayout();
            this.pnlSearch.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlBtn
            // 
            this.pnlBtn.BackColor = System.Drawing.Color.WhiteSmoke;
            this.pnlBtn.Controls.Add(this.uiButton1);
            this.pnlBtn.Controls.Add(this.uiButton2);
            this.pnlBtn.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlBtn.Location = new System.Drawing.Point(0, 451);
            this.pnlBtn.Name = "pnlBtn";
            this.pnlBtn.Size = new System.Drawing.Size(498, 40);
            this.pnlBtn.TabIndex = 6;
            // 
            // uiButton1
            // 
            this.uiButton1.BackColor = System.Drawing.SystemColors.Control;
            this.uiButton1.ImageIndex = 1;
            this.uiButton1.Location = new System.Drawing.Point(258, 9);
            this.uiButton1.Name = "uiButton1";
            this.uiButton1.Size = new System.Drawing.Size(70, 23);
            this.uiButton1.TabIndex = 8;
            this.uiButton1.Text = "닫기";
            this.uiButton1.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.uiButton1.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // uiButton2
            // 
            this.uiButton2.BackColor = System.Drawing.SystemColors.Control;
            this.uiButton2.ImageIndex = 0;
            this.uiButton2.Location = new System.Drawing.Point(170, 9);
            this.uiButton2.Name = "uiButton2";
            this.uiButton2.Size = new System.Drawing.Size(70, 23);
            this.uiButton2.TabIndex = 7;
            this.uiButton2.Text = "확인";
            this.uiButton2.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.uiButton2.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // uiPM
            // 
            this.uiPM.BackColorGradientAutoHideStrip = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(218)))), ((int)(((byte)(250)))));
            this.uiPM.ContainerControl = this;
            this.uiPM.VisualStyle = Janus.Windows.UI.Dock.PanelVisualStyle.Office2007;
            this.uiPanel0.Id = new System.Guid("e76ce5a4-55db-4768-a82b-99554f672f7a");
            this.uiPanel0.StaticGroup = true;
            this.uiPanel3.Id = new System.Guid("2af30c8f-5aad-43f1-9479-e5297477360e");
            this.uiPanel0.Panels.Add(this.uiPanel3);
            this.uiPanel4.Id = new System.Guid("c2de0312-4cbb-488c-8e7e-512f3d44468a");
            this.uiPanel0.Panels.Add(this.uiPanel4);
            this.uiPM.Panels.Add(this.uiPanel0);
            // 
            // Design Time Panel Info:
            // 
            this.uiPM.BeginPanelInfo();
            this.uiPM.AddDockPanelInfo(new System.Guid("e76ce5a4-55db-4768-a82b-99554f672f7a"), Janus.Windows.UI.Dock.PanelGroupStyle.VerticalTiles, Janus.Windows.UI.Dock.PanelDockStyle.Fill, true, new System.Drawing.Size(492, 405), true);
            this.uiPM.AddDockPanelInfo(new System.Guid("2af30c8f-5aad-43f1-9479-e5297477360e"), new System.Guid("e76ce5a4-55db-4768-a82b-99554f672f7a"), 786, true);
            this.uiPM.AddDockPanelInfo(new System.Guid("c2de0312-4cbb-488c-8e7e-512f3d44468a"), new System.Guid("e76ce5a4-55db-4768-a82b-99554f672f7a"), 786, true);
            this.uiPM.AddFloatingPanelInfo(new System.Guid("083e8f61-9d7d-4e6b-831f-be80f6b8df6d"), Janus.Windows.UI.Dock.PanelGroupStyle.VerticalTiles, true, new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
            this.uiPM.AddFloatingPanelInfo(new System.Guid("e04fffea-30bd-41ee-bac8-eb8c6dae0c44"), new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
            this.uiPM.AddFloatingPanelInfo(new System.Guid("af08113b-57a7-4b6c-8b5d-55bc6195bc72"), new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
            this.uiPM.AddFloatingPanelInfo(new System.Guid("e76ce5a4-55db-4768-a82b-99554f672f7a"), Janus.Windows.UI.Dock.PanelGroupStyle.VerticalTiles, true, new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
            this.uiPM.AddFloatingPanelInfo(new System.Guid("2af30c8f-5aad-43f1-9479-e5297477360e"), new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
            this.uiPM.AddFloatingPanelInfo(new System.Guid("c2de0312-4cbb-488c-8e7e-512f3d44468a"), new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
            this.uiPM.EndPanelInfo();
            // 
            // uiPanel0
            // 
            this.uiPanel0.CaptionVisible = Janus.Windows.UI.InheritableBoolean.False;
            this.uiPanel0.CloseButtonVisible = Janus.Windows.UI.InheritableBoolean.False;
            this.uiPanel0.GroupStyle = Janus.Windows.UI.Dock.PanelGroupStyle.VerticalTiles;
            this.uiPanel0.Location = new System.Drawing.Point(3, 43);
            this.uiPanel0.Name = "uiPanel0";
            this.uiPanel0.Size = new System.Drawing.Size(492, 405);
            this.uiPanel0.TabIndex = 4;
            this.uiPanel0.Text = "업종";
            // 
            // uiPanel3
            // 
            this.uiPanel3.InnerContainer = this.uiPanel3Container;
            this.uiPanel3.Location = new System.Drawing.Point(0, 0);
            this.uiPanel3.Name = "uiPanel3";
            this.uiPanel3.Size = new System.Drawing.Size(244, 405);
            this.uiPanel3.TabIndex = 4;
            this.uiPanel3.Text = "대분류";
            // 
            // uiPanel3Container
            // 
            this.uiPanel3Container.Controls.Add(this.grdExLevel1);
            this.uiPanel3Container.Location = new System.Drawing.Point(1, 23);
            this.uiPanel3Container.Name = "uiPanel3Container";
            this.uiPanel3Container.Size = new System.Drawing.Size(242, 381);
            this.uiPanel3Container.TabIndex = 0;
            // 
            // grdExLevel1
            // 
            this.grdExLevel1.AllowEdit = Janus.Windows.GridEX.InheritableBoolean.False;
            this.grdExLevel1.AlternatingColors = true;
            this.grdExLevel1.AlternatingRowFormatStyle.BackgroundImageDrawMode = Janus.Windows.GridEX.BackgroundImageDrawMode.Tile;
            this.grdExLevel1.AutomaticSort = false;
            this.grdExLevel1.BorderStyle = Janus.Windows.GridEX.BorderStyle.None;
            this.grdExLevel1.DataSource = this.dvJobLevel1;
            grdExLevel1_DesignTimeLayout.LayoutString = resources.GetString("grdExLevel1_DesignTimeLayout.LayoutString");
            this.grdExLevel1.DesignTimeLayout = grdExLevel1_DesignTimeLayout;
            this.grdExLevel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdExLevel1.EmptyRows = true;
            this.grdExLevel1.FocusCellFormatStyle.BackColor = System.Drawing.SystemColors.Highlight;
            this.grdExLevel1.FocusCellFormatStyle.ForeColor = System.Drawing.Color.White;
            this.grdExLevel1.FocusStyle = Janus.Windows.GridEX.FocusStyle.None;
            this.grdExLevel1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.249999F);
            this.grdExLevel1.FrozenColumns = 2;
            this.grdExLevel1.GridLineColor = System.Drawing.Color.Silver;
            this.grdExLevel1.GridLines = Janus.Windows.GridEX.GridLines.Vertical;
            this.grdExLevel1.GridLineStyle = Janus.Windows.GridEX.GridLineStyle.Solid;
            this.grdExLevel1.GroupByBoxVisible = false;
            this.grdExLevel1.GroupRowVisualStyle = Janus.Windows.GridEX.GroupRowVisualStyle.Outlook2003;
            this.grdExLevel1.HeaderFormatStyle.TextAlignment = Janus.Windows.GridEX.TextAlignment.Center;
            this.grdExLevel1.HideSelection = Janus.Windows.GridEX.HideSelection.HighlightInactive;
            grdExLevel1_Layout_0.Key = "bea";
            this.grdExLevel1.Layouts.AddRange(new Janus.Windows.GridEX.GridEXLayout[] {
            grdExLevel1_Layout_0});
            this.grdExLevel1.Location = new System.Drawing.Point(0, 0);
            this.grdExLevel1.Name = "grdExLevel1";
            this.grdExLevel1.ScrollBars = Janus.Windows.GridEX.ScrollBars.Vertical;
            this.grdExLevel1.SelectedInactiveFormatStyle.BackColor = System.Drawing.Color.Gold;
            this.grdExLevel1.Size = new System.Drawing.Size(242, 381);
            this.grdExLevel1.TabIndex = 5;
            this.grdExLevel1.TabKeyBehavior = Janus.Windows.GridEX.TabKeyBehavior.ControlNavigation;
            this.grdExLevel1.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            // 
            // dvJobLevel1
            // 
            this.dvJobLevel1.Table = this.advertiser_pDs1.JobLevel1;
            // 
            // advertiser_pDs1
            // 
            this.advertiser_pDs1.DataSetName = "Advertiser_pDs";
            this.advertiser_pDs1.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // uiPanel4
            // 
            this.uiPanel4.InnerContainer = this.uiPanel4Container;
            this.uiPanel4.Location = new System.Drawing.Point(248, 0);
            this.uiPanel4.Name = "uiPanel4";
            this.uiPanel4.Size = new System.Drawing.Size(244, 405);
            this.uiPanel4.TabIndex = 4;
            this.uiPanel4.Text = "중분류";
            // 
            // uiPanel4Container
            // 
            this.uiPanel4Container.Controls.Add(this.grdExLevel2);
            this.uiPanel4Container.Location = new System.Drawing.Point(1, 23);
            this.uiPanel4Container.Name = "uiPanel4Container";
            this.uiPanel4Container.Size = new System.Drawing.Size(242, 381);
            this.uiPanel4Container.TabIndex = 0;
            // 
            // grdExLevel2
            // 
            this.grdExLevel2.AllowEdit = Janus.Windows.GridEX.InheritableBoolean.False;
            this.grdExLevel2.AlternatingColors = true;
            this.grdExLevel2.AlternatingRowFormatStyle.BackgroundImageDrawMode = Janus.Windows.GridEX.BackgroundImageDrawMode.Tile;
            this.grdExLevel2.AutomaticSort = false;
            this.grdExLevel2.BorderStyle = Janus.Windows.GridEX.BorderStyle.None;
            this.grdExLevel2.DataSource = this.dvJonLevel2;
            grdExLevel2_DesignTimeLayout.LayoutString = resources.GetString("grdExLevel2_DesignTimeLayout.LayoutString");
            this.grdExLevel2.DesignTimeLayout = grdExLevel2_DesignTimeLayout;
            this.grdExLevel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdExLevel2.EmptyRows = true;
            this.grdExLevel2.FocusCellFormatStyle.BackColor = System.Drawing.SystemColors.Highlight;
            this.grdExLevel2.FocusCellFormatStyle.ForeColor = System.Drawing.Color.White;
            this.grdExLevel2.FocusStyle = Janus.Windows.GridEX.FocusStyle.None;
            this.grdExLevel2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.249999F);
            this.grdExLevel2.FrozenColumns = 2;
            this.grdExLevel2.GridLineColor = System.Drawing.Color.Silver;
            this.grdExLevel2.GridLines = Janus.Windows.GridEX.GridLines.Vertical;
            this.grdExLevel2.GridLineStyle = Janus.Windows.GridEX.GridLineStyle.Solid;
            this.grdExLevel2.GroupByBoxVisible = false;
            this.grdExLevel2.GroupRowVisualStyle = Janus.Windows.GridEX.GroupRowVisualStyle.Outlook2003;
            this.grdExLevel2.HeaderFormatStyle.TextAlignment = Janus.Windows.GridEX.TextAlignment.Center;
            this.grdExLevel2.HideSelection = Janus.Windows.GridEX.HideSelection.HighlightInactive;
            grdExLevel2_Layout_0.Key = "bea";
            this.grdExLevel2.Layouts.AddRange(new Janus.Windows.GridEX.GridEXLayout[] {
            grdExLevel2_Layout_0});
            this.grdExLevel2.Location = new System.Drawing.Point(0, 0);
            this.grdExLevel2.Name = "grdExLevel2";
            this.grdExLevel2.ScrollBars = Janus.Windows.GridEX.ScrollBars.Vertical;
            this.grdExLevel2.SelectedInactiveFormatStyle.BackColor = System.Drawing.Color.Gold;
            this.grdExLevel2.Size = new System.Drawing.Size(242, 381);
            this.grdExLevel2.TabIndex = 5;
            this.grdExLevel2.TabKeyBehavior = Janus.Windows.GridEX.TabKeyBehavior.ControlNavigation;
            this.grdExLevel2.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            // 
            // dvJonLevel2
            // 
            this.dvJonLevel2.Table = this.advertiser_pDs1.JobLevel2;
            // 
            // uiPanel1
            // 
            this.uiPanel1.InnerContainer = this.uiPanel1Container;
            this.uiPanel1.Location = new System.Drawing.Point(0, 0);
            this.uiPanel1.Name = "uiPanel1";
            this.uiPanel1.Size = new System.Drawing.Size(391, 420);
            this.uiPanel1.TabIndex = 4;
            this.uiPanel1.Text = "메뉴";
            // 
            // uiPanel1Container
            // 
            this.uiPanel1Container.Location = new System.Drawing.Point(0, 0);
            this.uiPanel1Container.Name = "uiPanel1Container";
            this.uiPanel1Container.Size = new System.Drawing.Size(391, 420);
            this.uiPanel1Container.TabIndex = 0;
            // 
            // uiPanel2
            // 
            this.uiPanel2.InnerContainer = this.uiPanel2Container;
            this.uiPanel2.Location = new System.Drawing.Point(395, 0);
            this.uiPanel2.Name = "uiPanel2";
            this.uiPanel2.Size = new System.Drawing.Size(391, 420);
            this.uiPanel2.TabIndex = 4;
            this.uiPanel2.Text = "채널";
            // 
            // uiPanel2Container
            // 
            this.uiPanel2Container.Location = new System.Drawing.Point(0, 0);
            this.uiPanel2Container.Name = "uiPanel2Container";
            this.uiPanel2Container.Size = new System.Drawing.Size(391, 420);
            this.uiPanel2Container.TabIndex = 0;
            // 
            // pnlSearch
            // 
            this.pnlSearch.BackColor = System.Drawing.SystemColors.Window;
            this.pnlSearch.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlSearch.Controls.Add(this.ebJobName2);
            this.pnlSearch.Controls.Add(this.ebJobName1);
            this.pnlSearch.Controls.Add(this.ebJobCode);
            this.pnlSearch.Controls.Add(this.label1);
            this.pnlSearch.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlSearch.Location = new System.Drawing.Point(0, 0);
            this.pnlSearch.Name = "pnlSearch";
            this.pnlSearch.Size = new System.Drawing.Size(498, 40);
            this.pnlSearch.TabIndex = 0;
            // 
            // ebJobName2
            // 
            this.ebJobName2.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ebJobName2.Location = new System.Drawing.Point(314, 8);
            this.ebJobName2.Name = "ebJobName2";
            this.ebJobName2.ReadOnly = true;
            this.ebJobName2.Size = new System.Drawing.Size(171, 21);
            this.ebJobName2.TabIndex = 11;
            this.ebJobName2.TabStop = false;
            this.ebJobName2.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
            this.ebJobName2.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            // 
            // ebJobName1
            // 
            this.ebJobName1.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ebJobName1.Location = new System.Drawing.Point(168, 8);
            this.ebJobName1.Name = "ebJobName1";
            this.ebJobName1.ReadOnly = true;
            this.ebJobName1.Size = new System.Drawing.Size(140, 21);
            this.ebJobName1.TabIndex = 9;
            this.ebJobName1.TabStop = false;
            this.ebJobName1.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
            this.ebJobName1.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            // 
            // ebJobCode
            // 
            this.ebJobCode.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ebJobCode.Location = new System.Drawing.Point(77, 8);
            this.ebJobCode.Name = "ebJobCode";
            this.ebJobCode.ReadOnly = true;
            this.ebJobCode.Size = new System.Drawing.Size(85, 21);
            this.ebJobCode.TabIndex = 8;
            this.ebJobCode.TabStop = false;
            this.ebJobCode.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
            this.ebJobCode.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(2, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(69, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "선택된 업종";
            // 
            // AdvertiserJob_pForm
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(6, 14);
            this.ClientSize = new System.Drawing.Size(498, 491);
            this.Controls.Add(this.uiPanel0);
            this.Controls.Add(this.pnlSearch);
            this.Controls.Add(this.pnlBtn);
            this.Name = "AdvertiserJob_pForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "업종선택";
            this.Load += new System.EventHandler(this.ContractJob_pForm_Load);
            this.pnlBtn.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.uiPM)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanel0)).EndInit();
            this.uiPanel0.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.uiPanel3)).EndInit();
            this.uiPanel3.ResumeLayout(false);
            this.uiPanel3Container.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdExLevel1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dvJobLevel1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.advertiser_pDs1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanel4)).EndInit();
            this.uiPanel4.ResumeLayout(false);
            this.uiPanel4Container.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdExLevel2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dvJonLevel2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanel1)).EndInit();
            this.uiPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.uiPanel2)).EndInit();
            this.uiPanel2.ResumeLayout(false);
            this.pnlSearch.ResumeLayout(false);
            this.pnlSearch.PerformLayout();
            this.ResumeLayout(false);

		}
        #endregion

        #region 컨트롤 로드
        private void ContractJob_pForm_Load(object sender, System.EventArgs e)
        {           
            // 데이터관리용 객체생성
            dtLevel1 = ((DataView)grdExLevel1.DataSource).Table;
            cmLevel1 = (CurrencyManager)this.BindingContext[grdExLevel1.DataSource];
            grdExLevel1.Click += new System.EventHandler(OnGrdLevel1RowChanged);

            dtLevel2 = ((DataView)grdExLevel2.DataSource).Table;
            cmLevel2 = (CurrencyManager)this.BindingContext[grdExLevel2.DataSource];
            grdExLevel2.Click += new System.EventHandler(OnGrdLevel2RowChanged); 

			InitControl();
		}

		private void InitControl()
		{
			InitList();

			//SearchJobList();
		}

		private void InitList()
		{
			Init_JobClassInfo();
		}

		#endregion

		#region 사용자 액션처리 메소드
        /// <summary>
        /// 대분류 그리드의 Row변경시
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnGrdLevel1RowChanged(object sender, System.EventArgs e)
        {
            if (grdExLevel1.RecordCount > 0)
            {
                SetLevel1Detail();
                SetDetailList();
            }
        }
        
        /// <summary>
		/// 중분류 그리드의 Row변경시
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
        private void OnGrdLevel2RowChanged(object sender, System.EventArgs e) 
		{
            if (grdExLevel2.RecordCount > 0)
            {
                SetLevel2Detail();
            }
		}
     
		private void btnClose_Click(object sender, System.EventArgs e)
		{
			this.Close();
		}

		private void btnOk_Click(object sender, System.EventArgs e)
		{
            string jobClassName = string.Empty;

            // 업종 코드가 없는 것은 선택을 안한 것으로 코드 입력 불가
            if (ebJobCode.Text.Equals(string.Empty))
            {
                MessageBox.Show(  "업종 종류를 선택하지 않았습니다."
                                , "업종 선택"
                                , MessageBoxButtons.OK
                                , MessageBoxIcon.Error);
                return;
            }

            jobClassName = ebJobName1.Text;

            if (!ebJobName2.Text.Equals(string.Empty))
            {
                jobClassName += " - " + ebJobName2.Text;
            }

            Opener.SetJobClass(ebJobCode.Text, jobClassName);

            this.Close();
		}

		private void Init_JobClassInfo()
		{
            model.Init();
            advertiser_pDs1.JobLevel1.Clear();
            advertiser_pDs1.JobLevel2.Clear();

			// 매체를 조회한다.
			new AdvertiserManager(systemModel, commonModel).GetJobClassList(model);
			
			if (model.ResultCD.Equals("0000"))
			{
				// 데이터셋에 셋팅
                SetDataTable(advertiser_pDs1.JobLevel1, model.AdvertiserDataSet, "JobLevel = 1");
			}

			Application.DoEvents();
		}

		#endregion
  
        #region 처리메소드

        /// <summary>
        /// 조건에 맞는 데이터 추출 DataTable에 셋팅
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="ds"></param>
        /// <param name="option">조건</param>
        private void SetDataTable(DataTable dt, DataSet ds, string option)
        {
            dt.Clear();

            foreach (DataRow row in ds.Tables[0].Select(option))
            {
                dt.ImportRow(row);
            }
        }

        /// <summary>
        /// 중분류 업종코드 리스트 검색
        /// </summary>
        private void SetDetailList()
        {
            int curRow = cmLevel1.Position;
            if (curRow < 0) return;

            keyCode = Convert.ToInt32(dtLevel1.Rows[curRow]["JobCode"].ToString());

            SearchJobCodeLevel2(keyCode);
        }

        /// <summary>
        /// Level1 코드로 Level2 업종코드 검색
        /// </summary>
        /// <param name="code">Level1 Code</param>
        private void SearchJobCodeLevel2(int code)
        {
            SetDataTable(advertiser_pDs1.JobLevel2, model.AdvertiserDataSet, "JobLevel = 2 and JobUpperCode = " + code);
        }

        private void SetLevel1Detail()
        {
            SetDetailInit();

            int curRow = cmLevel1.Position;
            if (curRow < 0) return;

            ebJobCode.Text = dtLevel1.Rows[curRow]["JobCode"].ToString();
            ebJobName1.Text = dtLevel1.Rows[curRow]["JobName"].ToString();
        }

        private void SetLevel2Detail()
        {
            int curRow = cmLevel2.Position;
            if (curRow < 0) return;

            ebJobCode.Text = dtLevel2.Rows[curRow]["JobCode"].ToString();
            ebJobName2.Text = dtLevel2.Rows[curRow]["JobName"].ToString();
        }

        private void SetDetailInit()
        {
            ebJobCode.Text = string.Empty;
            ebJobName1.Text = string.Empty;
            ebJobName2.Text = string.Empty;
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

        #endregion
    }
}
