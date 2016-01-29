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

    public class ContractJob_pForm : System.Windows.Forms.Form
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
		ContractModel  contractModel   = new ContractModel();	// 지정광고편성모델

		// 이 창을 연 컨트롤
		ContractControl Opener = null;
       

        // 화면처리용 변수
        public String MediaCode   = null;
        CurrencyManager cm        = null;					// 데이터 그리드의 변경에 따른 데이터셋 관리를 위하여			
        DataTable       dt        = null;
        DataTable       dtChannel = null;
		bool IsNewSearchKey		  = true;					// 검색어입력 여부

        #endregion

		#region  생성자 소멸자 컨트롤선언

        private System.Windows.Forms.Panel pnlBtn;
		private Janus.Windows.UI.Dock.UIPanelManager uiPM;
		private Janus.Windows.UI.Dock.UIPanel uiPanel1;
		private Janus.Windows.UI.Dock.UIPanelInnerContainer uiPanel1Container;
		private Janus.Windows.UI.Dock.UIPanel uiPanel2;
		private Janus.Windows.UI.Dock.UIPanelInnerContainer uiPanel2Container;		
		private System.Windows.Forms.Panel pnlSearch;
		private Janus.Windows.EditControls.UIButton uiButton11;
		private Janus.Windows.EditControls.UIComboBox cbLevel1;
		private Janus.Windows.UI.Dock.UIPanelGroup uiPanel0;
		private Janus.Windows.UI.Dock.UIPanel uiPanel3;
		private Janus.Windows.UI.Dock.UIPanelInnerContainer uiPanel3Container;
		private Janus.Windows.UI.Dock.UIPanel uiPanel4;
		private Janus.Windows.UI.Dock.UIPanelInnerContainer uiPanel4Container;
		private Janus.Windows.EditControls.UIButton uiButton1;
		private Janus.Windows.EditControls.UIButton uiButton2;
		private Janus.Windows.GridEX.GridEX grdExLevel1;
		private System.Data.DataView dvJob;
		private System.Data.DataView dvLevel3;
		private AdManagerClient.Contract_pDs contract_pDs;
		private Janus.Windows.GridEX.GridEX grdEXLevel3;
		private Janus.Windows.GridEX.EditControls.EditBox ebSearchKey;
        private System.ComponentModel.IContainer components;

        /// <summary>
        /// 데이터 넘겨야 할 넘
        /// </summary>
        /// <param name="sender"></param>
        public ContractJob_pForm(UserControl parent)
        {
            //
            // Windows Form 디자이너 지원에 필요합니다.
            //
            InitializeComponent();

			// 이창을 호출한 컨트롤
			Opener = (ContractControl) parent;
			contractModel.Init();
		}

        /// <summary>
        /// 일반사용자
        /// </summary>
        public ContractJob_pForm()
        {
            //
            // Windows Form 디자이너 지원에 필요합니다.
            //
            InitializeComponent();

			contractModel.Init();
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
            this.components = new System.ComponentModel.Container();
            Janus.Windows.GridEX.GridEXLayout grdExLevel1_Layout_0 = new Janus.Windows.GridEX.GridEXLayout();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ContractJob_pForm));
            Janus.Windows.GridEX.GridEXLayout grdEXLevel3_Layout_0 = new Janus.Windows.GridEX.GridEXLayout();
            this.dvJob = new System.Data.DataView();
            this.contract_pDs = new AdManagerClient.Contract_pDs();
            this.dvLevel3 = new System.Data.DataView();
            this.pnlBtn = new System.Windows.Forms.Panel();
            this.uiButton1 = new Janus.Windows.EditControls.UIButton();
            this.uiButton2 = new Janus.Windows.EditControls.UIButton();
            this.uiPM = new Janus.Windows.UI.Dock.UIPanelManager(this.components);
            this.uiPanel0 = new Janus.Windows.UI.Dock.UIPanelGroup();
            this.uiPanel3 = new Janus.Windows.UI.Dock.UIPanel();
            this.uiPanel3Container = new Janus.Windows.UI.Dock.UIPanelInnerContainer();
            this.grdExLevel1 = new Janus.Windows.GridEX.GridEX();
            this.uiPanel4 = new Janus.Windows.UI.Dock.UIPanel();
            this.uiPanel4Container = new Janus.Windows.UI.Dock.UIPanelInnerContainer();
            this.grdEXLevel3 = new Janus.Windows.GridEX.GridEX();
            this.uiPanel1 = new Janus.Windows.UI.Dock.UIPanel();
            this.uiPanel1Container = new Janus.Windows.UI.Dock.UIPanelInnerContainer();
            this.uiPanel2 = new Janus.Windows.UI.Dock.UIPanel();
            this.uiPanel2Container = new Janus.Windows.UI.Dock.UIPanelInnerContainer();
            this.pnlSearch = new System.Windows.Forms.Panel();
            this.uiButton11 = new Janus.Windows.EditControls.UIButton();
            this.ebSearchKey = new Janus.Windows.GridEX.EditControls.EditBox();
            this.cbLevel1 = new Janus.Windows.EditControls.UIComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.dvJob)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.contract_pDs)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dvLevel3)).BeginInit();
            this.pnlBtn.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uiPM)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanel0)).BeginInit();
            this.uiPanel0.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanel3)).BeginInit();
            this.uiPanel3.SuspendLayout();
            this.uiPanel3Container.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdExLevel1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanel4)).BeginInit();
            this.uiPanel4.SuspendLayout();
            this.uiPanel4Container.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdEXLevel3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanel1)).BeginInit();
            this.uiPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanel2)).BeginInit();
            this.uiPanel2.SuspendLayout();
            this.pnlSearch.SuspendLayout();
            this.SuspendLayout();
            // 
            // dvJob
            // 
            this.dvJob.Table = this.contract_pDs.Job;
            // 
            // contract_pDs
            // 
            this.contract_pDs.DataSetName = "Contract_pDs";
            this.contract_pDs.Locale = new System.Globalization.CultureInfo("en-US");
            this.contract_pDs.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // dvLevel3
            // 
            this.dvLevel3.Table = this.contract_pDs.Level3;
            // 
            // pnlBtn
            // 
            this.pnlBtn.BackColor = System.Drawing.Color.WhiteSmoke;
            this.pnlBtn.Controls.Add(this.uiButton1);
            this.pnlBtn.Controls.Add(this.uiButton2);
            this.pnlBtn.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlBtn.Location = new System.Drawing.Point(0, 426);
            this.pnlBtn.Name = "pnlBtn";
            this.pnlBtn.Size = new System.Drawing.Size(792, 40);
            this.pnlBtn.TabIndex = 6;
            // 
            // uiButton1
            // 
            this.uiButton1.BackColor = System.Drawing.SystemColors.Control;
            this.uiButton1.ImageIndex = 1;
            this.uiButton1.Location = new System.Drawing.Point(405, 9);
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
            this.uiButton2.Location = new System.Drawing.Point(317, 9);
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
            this.uiPM.AddDockPanelInfo(new System.Guid("e76ce5a4-55db-4768-a82b-99554f672f7a"), Janus.Windows.UI.Dock.PanelGroupStyle.VerticalTiles, Janus.Windows.UI.Dock.PanelDockStyle.Fill, true, new System.Drawing.Size(786, 380), true);
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
            this.uiPanel0.CloseButtonVisible = Janus.Windows.UI.InheritableBoolean.False;
            this.uiPanel0.GroupStyle = Janus.Windows.UI.Dock.PanelGroupStyle.VerticalTiles;
            this.uiPanel0.Location = new System.Drawing.Point(3, 43);
            this.uiPanel0.Name = "uiPanel0";
            this.uiPanel0.Size = new System.Drawing.Size(786, 380);
            this.uiPanel0.TabIndex = 4;
            this.uiPanel0.Text = "업종";
            // 
            // uiPanel3
            // 
            this.uiPanel3.InnerContainer = this.uiPanel3Container;
            this.uiPanel3.Location = new System.Drawing.Point(0, 22);
            this.uiPanel3.Name = "uiPanel3";
            this.uiPanel3.Size = new System.Drawing.Size(391, 358);
            this.uiPanel3.TabIndex = 4;
            this.uiPanel3.Text = "대분류/중분류";
            // 
            // uiPanel3Container
            // 
            this.uiPanel3Container.Controls.Add(this.grdExLevel1);
            this.uiPanel3Container.Location = new System.Drawing.Point(1, 23);
            this.uiPanel3Container.Name = "uiPanel3Container";
            this.uiPanel3Container.Size = new System.Drawing.Size(389, 334);
            this.uiPanel3Container.TabIndex = 0;
            // 
            // grdExLevel1
            // 
            this.grdExLevel1.AlternatingColors = true;
            this.grdExLevel1.AutomaticSort = false;
            this.grdExLevel1.BorderStyle = Janus.Windows.GridEX.BorderStyle.None;
            this.grdExLevel1.DataSource = this.dvJob;
            this.grdExLevel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdExLevel1.EmptyRows = true;
            this.grdExLevel1.FocusStyle = Janus.Windows.GridEX.FocusStyle.None;
            this.grdExLevel1.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.grdExLevel1.GridLines = Janus.Windows.GridEX.GridLines.Vertical;
            this.grdExLevel1.GridLineStyle = Janus.Windows.GridEX.GridLineStyle.Solid;
            this.grdExLevel1.GroupByBoxVisible = false;
            this.grdExLevel1.GroupMode = Janus.Windows.GridEX.GroupMode.Collapsed;
            this.grdExLevel1.HideSelection = Janus.Windows.GridEX.HideSelection.HighlightInactive;
            grdExLevel1_Layout_0.DataSource = this.dvJob;
            grdExLevel1_Layout_0.IsCurrentLayout = true;
            grdExLevel1_Layout_0.Key = "bae";
            grdExLevel1_Layout_0.LayoutString = resources.GetString("grdExLevel1_Layout_0.LayoutString");
            this.grdExLevel1.Layouts.AddRange(new Janus.Windows.GridEX.GridEXLayout[] {
            grdExLevel1_Layout_0});
            this.grdExLevel1.Location = new System.Drawing.Point(0, 0);
            this.grdExLevel1.Name = "grdExLevel1";
            this.grdExLevel1.ScrollBars = Janus.Windows.GridEX.ScrollBars.Vertical;
            this.grdExLevel1.SelectedInactiveFormatStyle.BackColor = System.Drawing.Color.Gold;
            this.grdExLevel1.Size = new System.Drawing.Size(389, 334);
            this.grdExLevel1.TabIndex = 4;
            this.grdExLevel1.TabKeyBehavior = Janus.Windows.GridEX.TabKeyBehavior.ControlNavigation;
            this.grdExLevel1.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            this.grdExLevel1.FormattingRow += new Janus.Windows.GridEX.RowLoadEventHandler(this.grdExLevel1_FormattingRow);
            this.grdExLevel1.Enter += new System.EventHandler(this.OnGrdRowChanged);
            // 
            // uiPanel4
            // 
            this.uiPanel4.InnerContainer = this.uiPanel4Container;
            this.uiPanel4.Location = new System.Drawing.Point(395, 22);
            this.uiPanel4.Name = "uiPanel4";
            this.uiPanel4.Size = new System.Drawing.Size(391, 358);
            this.uiPanel4.TabIndex = 4;
            this.uiPanel4.Text = "소분류";
            // 
            // uiPanel4Container
            // 
            this.uiPanel4Container.Controls.Add(this.grdEXLevel3);
            this.uiPanel4Container.Location = new System.Drawing.Point(1, 23);
            this.uiPanel4Container.Name = "uiPanel4Container";
            this.uiPanel4Container.Size = new System.Drawing.Size(389, 334);
            this.uiPanel4Container.TabIndex = 0;
            // 
            // grdEXLevel3
            // 
            this.grdEXLevel3.AlternatingColors = true;
            this.grdEXLevel3.BorderStyle = Janus.Windows.GridEX.BorderStyle.None;
            this.grdEXLevel3.DataSource = this.dvLevel3;
            this.grdEXLevel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdEXLevel3.EmptyRows = true;
            this.grdEXLevel3.FocusStyle = Janus.Windows.GridEX.FocusStyle.None;
            this.grdEXLevel3.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.grdEXLevel3.GridLines = Janus.Windows.GridEX.GridLines.Vertical;
            this.grdEXLevel3.GridLineStyle = Janus.Windows.GridEX.GridLineStyle.Solid;
            this.grdEXLevel3.GroupByBoxVisible = false;
            this.grdEXLevel3.GroupMode = Janus.Windows.GridEX.GroupMode.Collapsed;
            grdEXLevel3_Layout_0.DataSource = this.dvLevel3;
            grdEXLevel3_Layout_0.IsCurrentLayout = true;
            grdEXLevel3_Layout_0.Key = "bae";
            grdEXLevel3_Layout_0.LayoutString = resources.GetString("grdEXLevel3_Layout_0.LayoutString");
            this.grdEXLevel3.Layouts.AddRange(new Janus.Windows.GridEX.GridEXLayout[] {
            grdEXLevel3_Layout_0});
            this.grdEXLevel3.Location = new System.Drawing.Point(0, 0);
            this.grdEXLevel3.Name = "grdEXLevel3";
            this.grdEXLevel3.Size = new System.Drawing.Size(389, 334);
            this.grdEXLevel3.TabIndex = 5;
            this.grdEXLevel3.TabKeyBehavior = Janus.Windows.GridEX.TabKeyBehavior.ControlNavigation;
            this.grdEXLevel3.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            this.grdEXLevel3.DoubleClick += new System.EventHandler(this.grdEXLevel3_DoubleClick);
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
            this.pnlSearch.Controls.Add(this.uiButton11);
            this.pnlSearch.Controls.Add(this.ebSearchKey);
            this.pnlSearch.Controls.Add(this.cbLevel1);
            this.pnlSearch.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlSearch.Location = new System.Drawing.Point(0, 0);
            this.pnlSearch.Name = "pnlSearch";
            this.pnlSearch.Size = new System.Drawing.Size(792, 40);
            this.pnlSearch.TabIndex = 0;
            // 
            // uiButton11
            // 
            this.uiButton11.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.uiButton11.Location = new System.Drawing.Point(678, 8);
            this.uiButton11.Name = "uiButton11";
            this.uiButton11.Size = new System.Drawing.Size(104, 24);
            this.uiButton11.TabIndex = 3;
            this.uiButton11.Text = "조 회";
            this.uiButton11.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.uiButton11.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // ebSearchKey
            // 
            this.ebSearchKey.Location = new System.Drawing.Point(232, 8);
            this.ebSearchKey.Name = "ebSearchKey";
            this.ebSearchKey.Size = new System.Drawing.Size(224, 21);
            this.ebSearchKey.TabIndex = 2;
            this.ebSearchKey.Text = "검색어를 입력하세요";
            this.ebSearchKey.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
            this.ebSearchKey.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            this.ebSearchKey.TextChanged += new System.EventHandler(this.ebSearchKey_TextChanged);
            this.ebSearchKey.Click += new System.EventHandler(this.ebSearchKey_Click);
            this.ebSearchKey.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ebSearchKey_KeyDown);
            // 
            // cbLevel1
            // 
            this.cbLevel1.BackColor = System.Drawing.Color.White;
            this.cbLevel1.ComboStyle = Janus.Windows.EditControls.ComboStyle.DropDownList;
            this.cbLevel1.Location = new System.Drawing.Point(8, 8);
            this.cbLevel1.Name = "cbLevel1";
            this.cbLevel1.Size = new System.Drawing.Size(216, 21);
            this.cbLevel1.TabIndex = 1;
            this.cbLevel1.Text = "대분류선택";
            this.cbLevel1.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            // 
            // ContractJob_pForm
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(6, 14);
            this.ClientSize = new System.Drawing.Size(792, 466);
            this.Controls.Add(this.uiPanel0);
            this.Controls.Add(this.pnlSearch);
            this.Controls.Add(this.pnlBtn);
            this.Name = "ContractJob_pForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "업종선택";
            this.Load += new System.EventHandler(this.ContractJob_pForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dvJob)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.contract_pDs)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dvLevel3)).EndInit();
            this.pnlBtn.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.uiPM)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanel0)).EndInit();
            this.uiPanel0.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.uiPanel3)).EndInit();
            this.uiPanel3.ResumeLayout(false);
            this.uiPanel3Container.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdExLevel1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanel4)).EndInit();
            this.uiPanel4.ResumeLayout(false);
            this.uiPanel4Container.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdEXLevel3)).EndInit();
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
            dt        = ((DataView)grdExLevel1.DataSource).Table;  
            dtChannel = ((DataView)grdEXLevel3.DataSource).Table;  
            cm        = (CurrencyManager) this.BindingContext[grdExLevel1.DataSource]; 
            cm.PositionChanged += new System.EventHandler(OnGrdRowChanged); 
			
			InitControl();
		}

		private void InitControl()
		{
			InitCombo();

			SearchJobList();
		}

		private void InitCombo()
		{
			Init_Level1();
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
			if(grdExLevel1.RecordCount > 0 )
			{
				SearchLevel3List();
			}

		}
     
		private void btnClose_Click(object sender, System.EventArgs e)
		{
			this.Close();
		}

		private void btnOk_Click(object sender, System.EventArgs e)
		{
			//코드 넘겨줌
			contractModel.JobCode = dtChannel.Rows[grdEXLevel3.GetRow().RowIndex]["JobCode"].ToString();
			contractModel.JobName = dtChannel.Rows[grdEXLevel3.GetRow().RowIndex]["JobName"].ToString();
			contractModel.JobName2 = dtChannel.Rows[grdEXLevel3.GetRow().RowIndex]["JobName2"].ToString();
			contractModel.JobName3 = dtChannel.Rows[grdEXLevel3.GetRow().RowIndex]["JobName3"].ToString();
						
			Opener.adOn_Contract(contractModel);
			this.Close();
		}

		private void grdEXLevel3_DoubleClick(object sender, System.EventArgs e)
		{
			//코드 넘겨줌
			contractModel.JobCode = dtChannel.Rows[grdEXLevel3.GetRow().RowIndex]["JobCode"].ToString();
			contractModel.JobName = dtChannel.Rows[grdEXLevel3.GetRow().RowIndex]["JobName"].ToString();
			contractModel.JobName2 = dtChannel.Rows[grdEXLevel3.GetRow().RowIndex]["JobName2"].ToString();
			contractModel.JobName3 = dtChannel.Rows[grdEXLevel3.GetRow().RowIndex]["JobName3"].ToString();
						
			Opener.adOn_Contract(contractModel);
			this.Close();
		}

		private void btnSearch_Click(object sender, System.EventArgs e)
		{
			SearchJobList();
		}
		
		private void ebChannelNo_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
		{
			if(e.KeyCode == Keys.Enter)
			{
				//AddSchChoiceAdDeailChannel();
			}		
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
				SearchJobList();
			}
		}

		private void Init_Level1()
		{
			// 매체를 조회한다.
			ContractModel contractModel = new ContractModel();
			new ContractManager(systemModel, commonModel).GetLevel1List(contractModel);
			
			if (contractModel.ResultCD.Equals("0000"))
			{
				// 데이터셋에 셋팅
				Utility.SetDataTable(contract_pDs.Level1, contractModel.ContractDataSet);				
			}

			// 검색조건의 콤보
			this.cbLevel1.Items.Clear();
			
			// 콤보박스에 셋트할 코드목록을 담을 Item배열을 선언
			Janus.Windows.EditControls.UIComboBoxItem[] comboItems = new Janus.Windows.EditControls.UIComboBoxItem[contractModel.ResultCnt + 1];

			comboItems[0] = new Janus.Windows.EditControls.UIComboBoxItem("대분류선택","00");
			
			for(int i=0;i<contractModel.ResultCnt;i++)
			{
				DataRow row = contract_pDs.Level1.Rows[i];

				string val = row["JobCode"].ToString();
				string txt = row["JobName"].ToString();
				comboItems[i+1] = new Janus.Windows.EditControls.UIComboBoxItem(txt,val);
			}
			// 검색 콤보에 셋트
			this.cbLevel1.Items.AddRange(comboItems);
			this.cbLevel1.SelectedIndex = 0;

			Application.DoEvents();
		}

		#endregion
  
        #region 처리메소드

        /// <summary>
        /// 장르목록 조회
        /// </summary>
        private void SearchJobList()
        {
            StatusMessage("장르 정보를 조회합니다.");

            try
            {
				contractModel.Init();

				if(IsNewSearchKey)
				{
					contractModel.SearchKey = "";
				}
				else
				{
					contractModel.SearchKey  = ebSearchKey.Text;
				}
	
				contractModel.JobCode		 =  cbLevel1.SelectedItem.Value.ToString(); 
                // 장르목록조회 서비스를 호출한다.
                new ContractManager(systemModel,commonModel).GetJobList(contractModel);

                if (contractModel.ResultCD.Equals("0000"))
                {
                    Utility.SetDataTable(contract_pDs.Job, contractModel.ContractDataSet);				
                    StatusMessage(contractModel.ResultCnt + "건의 장르 정보가 조회되었습니다.");
					//SearchChannelList();
                }
            }
            catch(FrameException fe)
            {
                FrameSystem.showMsgForm("업종조회오류", new string[] {fe.ErrCode, fe.ResultMsg});
            }
            catch(Exception ex)
            {
                FrameSystem.showMsgForm("업종조회오류",new string[] {"",ex.Message});
            }
        }
        /// <summary>
        /// 채널목록 조회
        /// </summary>
        private void SearchLevel3List()
        {
            StatusMessage("채널 정보를 조회합니다.");

			int curRow = cm.Position;

			if(curRow < 0) return;

            try
            {
				grdEXLevel3.UnCheckAllRecords();

                // 데이터모델에 전송할 내용을 셋트한다.                
                contractModel.Level2Code    = dt.Rows[curRow]["Level2Code"].ToString();  

                // 채널목록조회 서비스를 호출한다.
                new ContractManager(systemModel,commonModel).GetLevel3List(contractModel);

                if (contractModel.ResultCD.Equals("0000"))
                {
                    Utility.SetDataTable(contract_pDs.Level3, contractModel.ContractDataSet);				
                    StatusMessage(contractModel.ResultCnt + "건의 장르 정보가 조회되었습니다.");
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

		private void grdExLevel1_FormattingRow(object sender, Janus.Windows.GridEX.RowLoadEventArgs e)
		{
		
		}		
    }
    #endregion


}
