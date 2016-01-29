namespace AdManagerClient
{
    partial class PreferenceTotalizeControl
    {
        /// <summary> 
        /// 필수 디자이너 변수입니다.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 사용 중인 모든 리소스를 정리합니다.
        /// </summary>
        /// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 구성 요소 디자이너에서 생성한 코드

        /// <summary> 
        /// 디자이너 지원에 필요한 메서드입니다. 
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마십시오.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            Janus.Windows.GridEX.GridEXLayout grdAdvList_DesignTimeLayout = new Janus.Windows.GridEX.GridEXLayout();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PreferenceTotalizeControl));
            Janus.Windows.GridEX.GridEXLayout grdQList_DesignTimeLayout = new Janus.Windows.GridEX.GridEXLayout();
            this.uip = new Janus.Windows.UI.Dock.UIPanelManager(this.components);
            this.uiPanel0 = new Janus.Windows.UI.Dock.UIPanelGroup();
            this.uipUp = new Janus.Windows.UI.Dock.UIPanelGroup();
            this.uipUpSearch = new Janus.Windows.UI.Dock.UIPanel();
            this.uiPanel3Container = new Janus.Windows.UI.Dock.UIPanelInnerContainer();
            this.pnlSearch = new System.Windows.Forms.Panel();
            this.btnSearch = new Janus.Windows.EditControls.UIButton();
            this.ebAdName = new Janus.Windows.GridEX.EditControls.EditBox();
            this.uipUpList = new Janus.Windows.UI.Dock.UIPanel();
            this.uiPanel4Container = new Janus.Windows.UI.Dock.UIPanelInnerContainer();
            this.grdAdvList = new Janus.Windows.GridEX.GridEX();
            this.dvAdList = new System.Data.DataView();
            this.preferenceTotalizeDs = new AdManagerClient._51_ReportSummaryAd._10_PreferenceTotalize.PreferenceTotalizeDs();
            this.uipBottom = new Janus.Windows.UI.Dock.UIPanelGroup();
            this.uipQ = new Janus.Windows.UI.Dock.UIPanel();
            this.uiPanel5Container = new Janus.Windows.UI.Dock.UIPanelInnerContainer();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.ebPopExpCount = new Janus.Windows.GridEX.EditControls.NumericEditBox();
            this.ebAdExpCount = new Janus.Windows.GridEX.EditControls.NumericEditBox();
            this.ebRepCount = new Janus.Windows.GridEX.EditControls.NumericEditBox();
            this.ebRepRate = new Janus.Windows.GridEX.EditControls.EditBox();
            this.ebPopType = new Janus.Windows.GridEX.EditControls.EditBox();
            this.ebEndDt = new Janus.Windows.GridEX.EditControls.EditBox();
            this.ebStartDt = new Janus.Windows.GridEX.EditControls.EditBox();
            this.lblRepRate = new System.Windows.Forms.Label();
            this.lblPopCount = new System.Windows.Forms.Label();
            this.lblDate = new System.Windows.Forms.Label();
            this.lblRepCount = new System.Windows.Forms.Label();
            this.lblExpCount = new System.Windows.Forms.Label();
            this.lblQTitle = new System.Windows.Forms.Label();
            this.ebQTitle = new Janus.Windows.GridEX.EditControls.EditBox();
            this.uipQList = new Janus.Windows.UI.Dock.UIPanel();
            this.uiPanel6Container = new Janus.Windows.UI.Dock.UIPanelInnerContainer();
            this.grdQList = new Janus.Windows.GridEX.GridEX();
            this.dvPopupDetail = new System.Data.DataView();
            ((System.ComponentModel.ISupportInitialize)(this.uip)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanel0)).BeginInit();
            this.uiPanel0.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uipUp)).BeginInit();
            this.uipUp.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uipUpSearch)).BeginInit();
            this.uipUpSearch.SuspendLayout();
            this.uiPanel3Container.SuspendLayout();
            this.pnlSearch.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uipUpList)).BeginInit();
            this.uipUpList.SuspendLayout();
            this.uiPanel4Container.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdAdvList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dvAdList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.preferenceTotalizeDs)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.uipBottom)).BeginInit();
            this.uipBottom.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uipQ)).BeginInit();
            this.uipQ.SuspendLayout();
            this.uiPanel5Container.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uipQList)).BeginInit();
            this.uipQList.SuspendLayout();
            this.uiPanel6Container.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdQList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dvPopupDetail)).BeginInit();
            this.SuspendLayout();
            // 
            // uip
            // 
            this.uip.AllowPanelDrag = false;
            this.uip.AllowPanelDrop = false;
            this.uip.AllowPanelResize = false;
            this.uip.ContainerControl = this;
            this.uip.VisualStyle = Janus.Windows.UI.Dock.PanelVisualStyle.Office2007;
            this.uiPanel0.Id = new System.Guid("a6485395-c46c-4c28-a00d-f10c5dd86d56");
            this.uiPanel0.StaticGroup = true;
            this.uipUp.Id = new System.Guid("5a62f02d-af91-4569-9870-af30fcf04850");
            this.uipUp.StaticGroup = true;
            this.uipUpSearch.Id = new System.Guid("271abbc8-40f4-4d7d-9e15-c453ba04de48");
            this.uipUp.Panels.Add(this.uipUpSearch);
            this.uipUpList.Id = new System.Guid("407e93d5-70fa-4ac0-8738-324707776d84");
            this.uipUp.Panels.Add(this.uipUpList);
            this.uiPanel0.Panels.Add(this.uipUp);
            this.uipBottom.Id = new System.Guid("111f79aa-2eb8-4b5c-8c2e-7362f562932a");
            this.uipBottom.StaticGroup = true;
            this.uipQ.Id = new System.Guid("b05df4ec-c525-49de-9981-bd3e5bdce8c3");
            this.uipBottom.Panels.Add(this.uipQ);
            this.uipQList.Id = new System.Guid("357c0345-dc9b-4828-b783-42844610b251");
            this.uipBottom.Panels.Add(this.uipQList);
            this.uiPanel0.Panels.Add(this.uipBottom);
            this.uip.Panels.Add(this.uiPanel0);
            // 
            // Design Time Panel Info:
            // 
            this.uip.BeginPanelInfo();
            this.uip.AddDockPanelInfo(new System.Guid("a6485395-c46c-4c28-a00d-f10c5dd86d56"), Janus.Windows.UI.Dock.PanelGroupStyle.HorizontalTiles, Janus.Windows.UI.Dock.PanelDockStyle.Fill, true, new System.Drawing.Size(1004, 671), true);
            this.uip.AddDockPanelInfo(new System.Guid("5a62f02d-af91-4569-9870-af30fcf04850"), new System.Guid("a6485395-c46c-4c28-a00d-f10c5dd86d56"), Janus.Windows.UI.Dock.PanelGroupStyle.HorizontalTiles, true, 214, true);
            this.uip.AddDockPanelInfo(new System.Guid("271abbc8-40f4-4d7d-9e15-c453ba04de48"), new System.Guid("5a62f02d-af91-4569-9870-af30fcf04850"), 41, true);
            this.uip.AddDockPanelInfo(new System.Guid("407e93d5-70fa-4ac0-8738-324707776d84"), new System.Guid("5a62f02d-af91-4569-9870-af30fcf04850"), 174, true);
            this.uip.AddDockPanelInfo(new System.Guid("111f79aa-2eb8-4b5c-8c2e-7362f562932a"), new System.Guid("a6485395-c46c-4c28-a00d-f10c5dd86d56"), Janus.Windows.UI.Dock.PanelGroupStyle.HorizontalTiles, true, 431, true);
            this.uip.AddDockPanelInfo(new System.Guid("b05df4ec-c525-49de-9981-bd3e5bdce8c3"), new System.Guid("111f79aa-2eb8-4b5c-8c2e-7362f562932a"), 95, true);
            this.uip.AddDockPanelInfo(new System.Guid("357c0345-dc9b-4828-b783-42844610b251"), new System.Guid("111f79aa-2eb8-4b5c-8c2e-7362f562932a"), 332, true);
            this.uip.AddFloatingPanelInfo(new System.Guid("a6485395-c46c-4c28-a00d-f10c5dd86d56"), Janus.Windows.UI.Dock.PanelGroupStyle.HorizontalTiles, true, new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
            this.uip.AddFloatingPanelInfo(new System.Guid("5a62f02d-af91-4569-9870-af30fcf04850"), Janus.Windows.UI.Dock.PanelGroupStyle.HorizontalTiles, true, new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
            this.uip.AddFloatingPanelInfo(new System.Guid("271abbc8-40f4-4d7d-9e15-c453ba04de48"), new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
            this.uip.AddFloatingPanelInfo(new System.Guid("407e93d5-70fa-4ac0-8738-324707776d84"), new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
            this.uip.AddFloatingPanelInfo(new System.Guid("111f79aa-2eb8-4b5c-8c2e-7362f562932a"), Janus.Windows.UI.Dock.PanelGroupStyle.HorizontalTiles, true, new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
            this.uip.AddFloatingPanelInfo(new System.Guid("b05df4ec-c525-49de-9981-bd3e5bdce8c3"), new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
            this.uip.AddFloatingPanelInfo(new System.Guid("357c0345-dc9b-4828-b783-42844610b251"), new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
            this.uip.EndPanelInfo();
            // 
            // uiPanel0
            // 
            this.uiPanel0.CloseButtonVisible = Janus.Windows.UI.InheritableBoolean.False;
            this.uiPanel0.Location = new System.Drawing.Point(3, 3);
            this.uiPanel0.Name = "uiPanel0";
            this.uiPanel0.Size = new System.Drawing.Size(1004, 671);
            this.uiPanel0.TabIndex = 4;
            this.uiPanel0.Text = "선호도 조사 팝업 집계";
            // 
            // uipUp
            // 
            this.uipUp.CaptionVisible = Janus.Windows.UI.InheritableBoolean.False;
            this.uipUp.Location = new System.Drawing.Point(0, 22);
            this.uipUp.Name = "uipUp";
            this.uipUp.Size = new System.Drawing.Size(1004, 214);
            this.uipUp.TabIndex = 4;
            this.uipUp.Text = "Panel 1";
            // 
            // uipUpSearch
            // 
            this.uipUpSearch.CaptionVisible = Janus.Windows.UI.InheritableBoolean.False;
            this.uipUpSearch.InnerContainer = this.uiPanel3Container;
            this.uipUpSearch.Location = new System.Drawing.Point(0, 0);
            this.uipUpSearch.Name = "uipUpSearch";
            this.uipUpSearch.Size = new System.Drawing.Size(1004, 41);
            this.uipUpSearch.TabIndex = 4;
            this.uipUpSearch.Text = "광고 리스트";
            // 
            // uiPanel3Container
            // 
            this.uiPanel3Container.Controls.Add(this.pnlSearch);
            this.uiPanel3Container.Location = new System.Drawing.Point(1, 1);
            this.uiPanel3Container.Name = "uiPanel3Container";
            this.uiPanel3Container.Size = new System.Drawing.Size(1002, 39);
            this.uiPanel3Container.TabIndex = 0;
            // 
            // pnlSearch
            // 
            this.pnlSearch.BackColor = System.Drawing.Color.White;
            this.pnlSearch.Controls.Add(this.btnSearch);
            this.pnlSearch.Controls.Add(this.ebAdName);
            this.pnlSearch.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlSearch.Location = new System.Drawing.Point(0, 0);
            this.pnlSearch.Name = "pnlSearch";
            this.pnlSearch.Size = new System.Drawing.Size(1002, 39);
            this.pnlSearch.TabIndex = 0;
            // 
            // btnSearch
            // 
            this.btnSearch.Location = new System.Drawing.Point(311, 8);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(104, 24);
            this.btnSearch.TabIndex = 1;
            this.btnSearch.Text = "조 회";
            this.btnSearch.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // ebAdName
            // 
            this.ebAdName.Location = new System.Drawing.Point(10, 8);
            this.ebAdName.Name = "ebAdName";
            this.ebAdName.Size = new System.Drawing.Size(293, 23);
            this.ebAdName.TabIndex = 0;
            this.ebAdName.Text = "검색어를 입력하세요";
            this.ebAdName.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            this.ebAdName.TextChanged += new System.EventHandler(this.ebAdName_TextChanged);
            this.ebAdName.Click += new System.EventHandler(this.ebAdName_Click);
            this.ebAdName.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ebAdName_KeyDown);
            // 
            // uipUpList
            // 
            this.uipUpList.CaptionVisible = Janus.Windows.UI.InheritableBoolean.False;
            this.uipUpList.InnerContainer = this.uiPanel4Container;
            this.uipUpList.Location = new System.Drawing.Point(0, 45);
            this.uipUpList.Name = "uipUpList";
            this.uipUpList.Size = new System.Drawing.Size(1004, 169);
            this.uipUpList.TabIndex = 4;
            this.uipUpList.Text = "Panel 4";
            // 
            // uiPanel4Container
            // 
            this.uiPanel4Container.Controls.Add(this.grdAdvList);
            this.uiPanel4Container.Location = new System.Drawing.Point(1, 1);
            this.uiPanel4Container.Name = "uiPanel4Container";
            this.uiPanel4Container.Size = new System.Drawing.Size(1002, 167);
            this.uiPanel4Container.TabIndex = 0;
            // 
            // grdAdvList
            // 
            this.grdAdvList.AllowColumnDrag = false;
            this.grdAdvList.AllowEdit = Janus.Windows.GridEX.InheritableBoolean.False;
            this.grdAdvList.AlternatingColors = true;
            this.grdAdvList.AutomaticSort = false;
            this.grdAdvList.BorderStyle = Janus.Windows.GridEX.BorderStyle.None;
            this.grdAdvList.DataSource = this.dvAdList;
            grdAdvList_DesignTimeLayout.LayoutString = resources.GetString("grdAdvList_DesignTimeLayout.LayoutString");
            this.grdAdvList.DesignTimeLayout = grdAdvList_DesignTimeLayout;
            this.grdAdvList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdAdvList.EmptyRows = true;
            this.grdAdvList.FocusCellFormatStyle.BackColor = System.Drawing.SystemColors.Highlight;
            this.grdAdvList.FocusCellFormatStyle.ForeColor = System.Drawing.Color.White;
            this.grdAdvList.FocusStyle = Janus.Windows.GridEX.FocusStyle.None;
            this.grdAdvList.Font = new System.Drawing.Font("나눔고딕", 8.249999F);
            this.grdAdvList.FrozenColumns = 2;
            this.grdAdvList.GridLineColor = System.Drawing.Color.Silver;
            this.grdAdvList.GridLines = Janus.Windows.GridEX.GridLines.Vertical;
            this.grdAdvList.GridLineStyle = Janus.Windows.GridEX.GridLineStyle.Solid;
            this.grdAdvList.GroupByBoxVisible = false;
            this.grdAdvList.HeaderFormatStyle.TextAlignment = Janus.Windows.GridEX.TextAlignment.Center;
            this.grdAdvList.HideSelection = Janus.Windows.GridEX.HideSelection.Highlight;
            this.grdAdvList.Location = new System.Drawing.Point(0, 0);
            this.grdAdvList.Name = "grdAdvList";
            this.grdAdvList.ScrollBars = Janus.Windows.GridEX.ScrollBars.Vertical;
            this.grdAdvList.SelectedFormatStyle.FontBold = Janus.Windows.GridEX.TriState.True;
            this.grdAdvList.SelectedInactiveFormatStyle.BackColor = System.Drawing.Color.Gold;
            this.grdAdvList.Size = new System.Drawing.Size(1002, 167);
            this.grdAdvList.TabIndex = 0;
            this.grdAdvList.TabKeyBehavior = Janus.Windows.GridEX.TabKeyBehavior.ControlNavigation;
            this.grdAdvList.ThemedAreas = ((Janus.Windows.GridEX.ThemedArea)(((((((((Janus.Windows.GridEX.ThemedArea.ScrollBars | Janus.Windows.GridEX.ThemedArea.EditControls) 
            | Janus.Windows.GridEX.ThemedArea.Headers) 
            | Janus.Windows.GridEX.ThemedArea.GroupByBox) 
            | Janus.Windows.GridEX.ThemedArea.GroupRows) 
            | Janus.Windows.GridEX.ThemedArea.ControlBorder) 
            | Janus.Windows.GridEX.ThemedArea.Cards) 
            | Janus.Windows.GridEX.ThemedArea.Gridlines) 
            | Janus.Windows.GridEX.ThemedArea.CheckBoxes)));
            this.grdAdvList.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            this.grdAdvList.SelectionChanged += new System.EventHandler(this.OnGrdRowChanged);
            this.grdAdvList.Enter += new System.EventHandler(this.OnGrdRowChanged);
            // 
            // dvAdList
            // 
            this.dvAdList.Table = this.preferenceTotalizeDs.AdList;
            // 
            // preferenceTotalizeDs
            // 
            this.preferenceTotalizeDs.DataSetName = "PreferenceTotalizeDs";
            this.preferenceTotalizeDs.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // uipBottom
            // 
            this.uipBottom.CaptionVisible = Janus.Windows.UI.InheritableBoolean.False;
            this.uipBottom.Location = new System.Drawing.Point(0, 240);
            this.uipBottom.Name = "uipBottom";
            this.uipBottom.Size = new System.Drawing.Size(1004, 431);
            this.uipBottom.TabIndex = 4;
            // 
            // uipQ
            // 
            this.uipQ.CaptionVisible = Janus.Windows.UI.InheritableBoolean.False;
            this.uipQ.InnerContainer = this.uiPanel5Container;
            this.uipQ.Location = new System.Drawing.Point(0, 0);
            this.uipQ.Name = "uipQ";
            this.uipQ.Size = new System.Drawing.Size(1004, 95);
            this.uipQ.TabIndex = 4;
            this.uipQ.Text = "Panel 5";
            // 
            // uiPanel5Container
            // 
            this.uiPanel5Container.Controls.Add(this.panel1);
            this.uiPanel5Container.Location = new System.Drawing.Point(1, 1);
            this.uiPanel5Container.Name = "uiPanel5Container";
            this.uiPanel5Container.Size = new System.Drawing.Size(1002, 93);
            this.uiPanel5Container.TabIndex = 0;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.White;
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.ebPopExpCount);
            this.panel1.Controls.Add(this.ebAdExpCount);
            this.panel1.Controls.Add(this.ebRepCount);
            this.panel1.Controls.Add(this.ebRepRate);
            this.panel1.Controls.Add(this.ebPopType);
            this.panel1.Controls.Add(this.ebEndDt);
            this.panel1.Controls.Add(this.ebStartDt);
            this.panel1.Controls.Add(this.lblRepRate);
            this.panel1.Controls.Add(this.lblPopCount);
            this.panel1.Controls.Add(this.lblDate);
            this.panel1.Controls.Add(this.lblRepCount);
            this.panel1.Controls.Add(this.lblExpCount);
            this.panel1.Controls.Add(this.lblQTitle);
            this.panel1.Controls.Add(this.ebQTitle);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1002, 93);
            this.panel1.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Location = new System.Drawing.Point(506, 62);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(74, 23);
            this.label1.TabIndex = 10;
            this.label1.Text = "팝업형식";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ebPopExpCount
            // 
            this.ebPopExpCount.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ebPopExpCount.DecimalDigits = 0;
            this.ebPopExpCount.FormatString = "#,##0";
            this.ebPopExpCount.Location = new System.Drawing.Point(586, 10);
            this.ebPopExpCount.Name = "ebPopExpCount";
            this.ebPopExpCount.ReadOnly = true;
            this.ebPopExpCount.Size = new System.Drawing.Size(100, 23);
            this.ebPopExpCount.TabIndex = 7;
            this.ebPopExpCount.Text = "1,234,567";
            this.ebPopExpCount.Value = new decimal(new int[] {
            1234567,
            0,
            0,
            0});
            this.ebPopExpCount.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            // 
            // ebAdExpCount
            // 
            this.ebAdExpCount.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ebAdExpCount.DecimalDigits = 0;
            this.ebAdExpCount.FormatString = "#,##0";
            this.ebAdExpCount.Location = new System.Drawing.Point(834, 10);
            this.ebAdExpCount.Name = "ebAdExpCount";
            this.ebAdExpCount.ReadOnly = true;
            this.ebAdExpCount.Size = new System.Drawing.Size(92, 23);
            this.ebAdExpCount.TabIndex = 6;
            this.ebAdExpCount.Text = "0";
            this.ebAdExpCount.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.ebAdExpCount.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            // 
            // ebRepCount
            // 
            this.ebRepCount.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ebRepCount.DecimalDigits = 0;
            this.ebRepCount.FormatString = "#,##0";
            this.ebRepCount.Location = new System.Drawing.Point(586, 36);
            this.ebRepCount.Name = "ebRepCount";
            this.ebRepCount.ReadOnly = true;
            this.ebRepCount.Size = new System.Drawing.Size(100, 23);
            this.ebRepCount.TabIndex = 8;
            this.ebRepCount.Text = "312,000";
            this.ebRepCount.Value = new decimal(new int[] {
            312000,
            0,
            0,
            0});
            this.ebRepCount.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            // 
            // ebRepRate
            // 
            this.ebRepRate.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ebRepRate.Location = new System.Drawing.Point(834, 36);
            this.ebRepRate.Name = "ebRepRate";
            this.ebRepRate.ReadOnly = true;
            this.ebRepRate.Size = new System.Drawing.Size(92, 23);
            this.ebRepRate.TabIndex = 9;
            this.ebRepRate.TextAlignment = Janus.Windows.GridEX.TextAlignment.Far;
            this.ebRepRate.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            // 
            // ebPopType
            // 
            this.ebPopType.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ebPopType.Location = new System.Drawing.Point(586, 62);
            this.ebPopType.Name = "ebPopType";
            this.ebPopType.ReadOnly = true;
            this.ebPopType.Size = new System.Drawing.Size(100, 23);
            this.ebPopType.TabIndex = 5;
            this.ebPopType.TextAlignment = Janus.Windows.GridEX.TextAlignment.Center;
            this.ebPopType.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            // 
            // ebEndDt
            // 
            this.ebEndDt.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ebEndDt.Location = new System.Drawing.Point(210, 36);
            this.ebEndDt.Name = "ebEndDt";
            this.ebEndDt.ReadOnly = true;
            this.ebEndDt.Size = new System.Drawing.Size(100, 23);
            this.ebEndDt.TabIndex = 4;
            this.ebEndDt.TextAlignment = Janus.Windows.GridEX.TextAlignment.Center;
            this.ebEndDt.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            // 
            // ebStartDt
            // 
            this.ebStartDt.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ebStartDt.Location = new System.Drawing.Point(101, 36);
            this.ebStartDt.Name = "ebStartDt";
            this.ebStartDt.ReadOnly = true;
            this.ebStartDt.Size = new System.Drawing.Size(100, 23);
            this.ebStartDt.TabIndex = 3;
            this.ebStartDt.TextAlignment = Janus.Windows.GridEX.TextAlignment.Center;
            this.ebStartDt.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            // 
            // lblRepRate
            // 
            this.lblRepRate.BackColor = System.Drawing.Color.Transparent;
            this.lblRepRate.Location = new System.Drawing.Point(754, 36);
            this.lblRepRate.Name = "lblRepRate";
            this.lblRepRate.Size = new System.Drawing.Size(68, 23);
            this.lblRepRate.TabIndex = 7;
            this.lblRepRate.Text = "응답률(%)";
            this.lblRepRate.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblPopCount
            // 
            this.lblPopCount.BackColor = System.Drawing.Color.Transparent;
            this.lblPopCount.Location = new System.Drawing.Point(506, 10);
            this.lblPopCount.Name = "lblPopCount";
            this.lblPopCount.Size = new System.Drawing.Size(83, 23);
            this.lblPopCount.TabIndex = 6;
            this.lblPopCount.Text = "팝업노출수";
            this.lblPopCount.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblDate
            // 
            this.lblDate.BackColor = System.Drawing.Color.Transparent;
            this.lblDate.Location = new System.Drawing.Point(21, 36);
            this.lblDate.Name = "lblDate";
            this.lblDate.Size = new System.Drawing.Size(63, 23);
            this.lblDate.TabIndex = 5;
            this.lblDate.Text = "진행기간";
            this.lblDate.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblRepCount
            // 
            this.lblRepCount.BackColor = System.Drawing.Color.Transparent;
            this.lblRepCount.Location = new System.Drawing.Point(506, 36);
            this.lblRepCount.Name = "lblRepCount";
            this.lblRepCount.Size = new System.Drawing.Size(82, 23);
            this.lblRepCount.TabIndex = 4;
            this.lblRepCount.Text = "응답수";
            this.lblRepCount.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblExpCount
            // 
            this.lblExpCount.BackColor = System.Drawing.Color.Transparent;
            this.lblExpCount.Location = new System.Drawing.Point(754, 10);
            this.lblExpCount.Name = "lblExpCount";
            this.lblExpCount.Size = new System.Drawing.Size(74, 23);
            this.lblExpCount.TabIndex = 3;
            this.lblExpCount.Text = "광고노출수";
            this.lblExpCount.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblQTitle
            // 
            this.lblQTitle.BackColor = System.Drawing.Color.Transparent;
            this.lblQTitle.Location = new System.Drawing.Point(21, 10);
            this.lblQTitle.Name = "lblQTitle";
            this.lblQTitle.Size = new System.Drawing.Size(63, 23);
            this.lblQTitle.TabIndex = 2;
            this.lblQTitle.Text = "설문제목";
            this.lblQTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ebQTitle
            // 
            this.ebQTitle.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ebQTitle.Location = new System.Drawing.Point(101, 10);
            this.ebQTitle.Name = "ebQTitle";
            this.ebQTitle.ReadOnly = true;
            this.ebQTitle.Size = new System.Drawing.Size(344, 23);
            this.ebQTitle.TabIndex = 2;
            this.ebQTitle.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            // 
            // uipQList
            // 
            this.uipQList.CaptionVisible = Janus.Windows.UI.InheritableBoolean.False;
            this.uipQList.InnerContainer = this.uiPanel6Container;
            this.uipQList.Location = new System.Drawing.Point(0, 99);
            this.uipQList.Name = "uipQList";
            this.uipQList.Size = new System.Drawing.Size(1004, 332);
            this.uipQList.TabIndex = 4;
            this.uipQList.Text = "Panel 6";
            // 
            // uiPanel6Container
            // 
            this.uiPanel6Container.Controls.Add(this.grdQList);
            this.uiPanel6Container.Location = new System.Drawing.Point(1, 1);
            this.uiPanel6Container.Name = "uiPanel6Container";
            this.uiPanel6Container.Size = new System.Drawing.Size(1002, 330);
            this.uiPanel6Container.TabIndex = 0;
            // 
            // grdQList
            // 
            this.grdQList.AllowColumnDrag = false;
            this.grdQList.AllowEdit = Janus.Windows.GridEX.InheritableBoolean.False;
            this.grdQList.AlternatingColors = true;
            this.grdQList.AutomaticSort = false;
            this.grdQList.BorderStyle = Janus.Windows.GridEX.BorderStyle.None;
            this.grdQList.DataSource = this.dvPopupDetail;
            grdQList_DesignTimeLayout.LayoutString = resources.GetString("grdQList_DesignTimeLayout.LayoutString");
            this.grdQList.DesignTimeLayout = grdQList_DesignTimeLayout;
            this.grdQList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdQList.EmptyRows = true;
            this.grdQList.FocusCellFormatStyle.BackColor = System.Drawing.SystemColors.Highlight;
            this.grdQList.FocusCellFormatStyle.ForeColor = System.Drawing.Color.White;
            this.grdQList.FocusStyle = Janus.Windows.GridEX.FocusStyle.None;
            this.grdQList.Font = new System.Drawing.Font("나눔고딕", 8.249999F);
            this.grdQList.FrozenColumns = 2;
            this.grdQList.GridLineColor = System.Drawing.Color.Silver;
            this.grdQList.GridLines = Janus.Windows.GridEX.GridLines.Vertical;
            this.grdQList.GridLineStyle = Janus.Windows.GridEX.GridLineStyle.Solid;
            this.grdQList.GroupByBoxVisible = false;
            this.grdQList.HeaderFormatStyle.TextAlignment = Janus.Windows.GridEX.TextAlignment.Center;
            this.grdQList.HideSelection = Janus.Windows.GridEX.HideSelection.Highlight;
            this.grdQList.Location = new System.Drawing.Point(0, 0);
            this.grdQList.Name = "grdQList";
            this.grdQList.ScrollBars = Janus.Windows.GridEX.ScrollBars.Vertical;
            this.grdQList.SelectedFormatStyle.FontBold = Janus.Windows.GridEX.TriState.True;
            this.grdQList.SelectedInactiveFormatStyle.BackColor = System.Drawing.Color.Gold;
            this.grdQList.Size = new System.Drawing.Size(1002, 330);
            this.grdQList.TabIndex = 1;
            this.grdQList.TabKeyBehavior = Janus.Windows.GridEX.TabKeyBehavior.ControlNavigation;
            this.grdQList.ThemedAreas = ((Janus.Windows.GridEX.ThemedArea)(((((((((Janus.Windows.GridEX.ThemedArea.ScrollBars | Janus.Windows.GridEX.ThemedArea.EditControls) 
            | Janus.Windows.GridEX.ThemedArea.Headers) 
            | Janus.Windows.GridEX.ThemedArea.GroupByBox) 
            | Janus.Windows.GridEX.ThemedArea.GroupRows) 
            | Janus.Windows.GridEX.ThemedArea.ControlBorder) 
            | Janus.Windows.GridEX.ThemedArea.Cards) 
            | Janus.Windows.GridEX.ThemedArea.Gridlines) 
            | Janus.Windows.GridEX.ThemedArea.CheckBoxes)));
            this.grdQList.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            // 
            // dvPopupDetail
            // 
            this.dvPopupDetail.Table = this.preferenceTotalizeDs.PopupDetail;
            // 
            // PreferenceTotalizeControl
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.SystemColors.ControlLight;
            this.Controls.Add(this.uiPanel0);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "PreferenceTotalizeControl";
            this.Size = new System.Drawing.Size(1010, 677);
            this.Load += new System.EventHandler(this.PreferenceTotalizeControl_Load);
            ((System.ComponentModel.ISupportInitialize)(this.uip)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanel0)).EndInit();
            this.uiPanel0.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.uipUp)).EndInit();
            this.uipUp.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.uipUpSearch)).EndInit();
            this.uipUpSearch.ResumeLayout(false);
            this.uiPanel3Container.ResumeLayout(false);
            this.pnlSearch.ResumeLayout(false);
            this.pnlSearch.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uipUpList)).EndInit();
            this.uipUpList.ResumeLayout(false);
            this.uiPanel4Container.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdAdvList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dvAdList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.preferenceTotalizeDs)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.uipBottom)).EndInit();
            this.uipBottom.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.uipQ)).EndInit();
            this.uipQ.ResumeLayout(false);
            this.uiPanel5Container.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uipQList)).EndInit();
            this.uipQList.ResumeLayout(false);
            this.uiPanel6Container.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdQList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dvPopupDetail)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Janus.Windows.UI.Dock.UIPanelManager uip;
        private Janus.Windows.UI.Dock.UIPanelGroup uiPanel0;
        private Janus.Windows.UI.Dock.UIPanelGroup uipUp;
        private Janus.Windows.UI.Dock.UIPanelGroup uipBottom;
        private Janus.Windows.UI.Dock.UIPanel uipUpSearch;
        private Janus.Windows.UI.Dock.UIPanelInnerContainer uiPanel3Container;
        private Janus.Windows.UI.Dock.UIPanel uipUpList;
        private Janus.Windows.UI.Dock.UIPanelInnerContainer uiPanel4Container;
        private Janus.Windows.UI.Dock.UIPanel uipQ;
        private Janus.Windows.UI.Dock.UIPanelInnerContainer uiPanel5Container;
        private Janus.Windows.UI.Dock.UIPanel uipQList;
        private Janus.Windows.UI.Dock.UIPanelInnerContainer uiPanel6Container;
        private System.Windows.Forms.Panel pnlSearch;
        private System.Windows.Forms.Panel panel1;
        private Janus.Windows.EditControls.UIButton btnSearch;
        private Janus.Windows.GridEX.EditControls.EditBox ebAdName;
        private Janus.Windows.GridEX.EditControls.EditBox ebQTitle;
        private System.Windows.Forms.Label lblQTitle;
        private Janus.Windows.GridEX.EditControls.EditBox ebRepRate;
        private Janus.Windows.GridEX.EditControls.EditBox ebPopType;
        private Janus.Windows.GridEX.EditControls.EditBox ebEndDt;
        private Janus.Windows.GridEX.EditControls.EditBox ebStartDt;
        private System.Windows.Forms.Label lblRepRate;
        private System.Windows.Forms.Label lblPopCount;
        private System.Windows.Forms.Label lblDate;
        private System.Windows.Forms.Label lblRepCount;
        private System.Windows.Forms.Label lblExpCount;
        private Janus.Windows.GridEX.GridEX grdAdvList;
        private _51_ReportSummaryAd._10_PreferenceTotalize.PreferenceTotalizeDs preferenceTotalizeDs;
        private System.Data.DataView dvAdList;
        private Janus.Windows.GridEX.EditControls.NumericEditBox ebRepCount;
        private Janus.Windows.GridEX.EditControls.NumericEditBox ebAdExpCount;
        private Janus.Windows.GridEX.EditControls.NumericEditBox ebPopExpCount;
        private System.Windows.Forms.Label label1;
        private Janus.Windows.GridEX.GridEX grdQList;
        private System.Data.DataView dvPopupDetail;
    }
}
