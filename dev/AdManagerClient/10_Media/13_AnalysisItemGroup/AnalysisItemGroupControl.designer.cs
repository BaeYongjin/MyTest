namespace AdManagerClient
{
    partial class AnalysisItemGroupControl
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
            Janus.Windows.GridEX.GridEXLayout grdExAnalysisItemGroupList_Layout_0 = new Janus.Windows.GridEX.GridEXLayout();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AnalysisItemGroupControl));
            Janus.Windows.GridEX.GridEXLayout grdExAnalysisItemGroupDetailList_Layout_0 = new Janus.Windows.GridEX.GridEXLayout();
            Janus.Windows.GridEX.GridEXLayout grdExContractItemList_Layout_0 = new Janus.Windows.GridEX.GridEXLayout();
            this.dvAnalysisItemGroup = new System.Data.DataView();
            this.analysisItemGroupDs = new AdManagerClient._10_Media._13_AnalysisItemGroup.AnalysisItemGroupDs();
            this.bsAnalysisItemGroupDetail = new System.Windows.Forms.BindingSource(this.components);
            this.bsContractItem = new System.Windows.Forms.BindingSource(this.components);
            this.uiPM = new Janus.Windows.UI.Dock.UIPanelManager(this.components);
            this.uiPanelAnalysisItem = new Janus.Windows.UI.Dock.UIPanelGroup();
            this.uiPanelAnalysisItemGroup = new Janus.Windows.UI.Dock.UIPanel();
            this.uiPanelAnalysisItemGroupContainer = new Janus.Windows.UI.Dock.UIPanelInnerContainer();
            this.cbSearchMonth = new Janus.Windows.EditControls.UIComboBox();
            this.ebSearchKey = new Janus.Windows.GridEX.EditControls.EditBox();
            this.btnSearch = new Janus.Windows.EditControls.UIButton();
            this.uiPanelAnalysisItemGroupList = new Janus.Windows.UI.Dock.UIPanel();
            this.uiPanelAnalysisItemGroupDetailContainer = new Janus.Windows.UI.Dock.UIPanelInnerContainer();
            this.grdExAnalysisItemGroupList = new Janus.Windows.GridEX.GridEX();
            this.uiPanelAnalysisItemGroupDetail = new Janus.Windows.UI.Dock.UIPanel();
            this.uiPanel0Container = new Janus.Windows.UI.Dock.UIPanelInnerContainer();
            this.tabAnalysisItemGroup = new Janus.Windows.UI.Tab.UITab();
            this.uiTabPage1 = new Janus.Windows.UI.Tab.UITabPage();
            this.ebModDt = new Janus.Windows.GridEX.EditControls.EditBox();
            this.cbAnalysisItemGroupType = new Janus.Windows.EditControls.UIComboBox();
            this.ebComment = new Janus.Windows.GridEX.EditControls.EditBox();
            this.ebAnalysisItemGroupName = new Janus.Windows.GridEX.EditControls.EditBox();
            this.ebRegDt = new Janus.Windows.GridEX.EditControls.EditBox();
            this.lbModDt = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.ebRegID = new Janus.Windows.GridEX.EditControls.EditBox();
            this.lbRegID = new System.Windows.Forms.Label();
            this.ebRegName = new Janus.Windows.GridEX.EditControls.EditBox();
            this.uiTabPage2 = new Janus.Windows.UI.Tab.UITabPage();
            this.grdExAnalysisItemGroupDetailList = new Janus.Windows.GridEX.GridEX();
            this.uiPanelAnalysisItemGroupDetailBtn = new Janus.Windows.UI.Dock.UIPanel();
            this.uiPanel1Container = new Janus.Windows.UI.Dock.UIPanelInnerContainer();
            this.btnDelete = new Janus.Windows.EditControls.UIButton();
            this.btnSave = new Janus.Windows.EditControls.UIButton();
            this.btnAdd = new Janus.Windows.EditControls.UIButton();
            this.uiPanelContract = new Janus.Windows.UI.Dock.UIPanelGroup();
            this.uiPanelContractItem = new Janus.Windows.UI.Dock.UIPanel();
            this.uiPanelContractItemContainer = new Janus.Windows.UI.Dock.UIPanelInnerContainer();
            this.btnSearch1 = new Janus.Windows.EditControls.UIButton();
            this.ebSearchKey1 = new Janus.Windows.GridEX.EditControls.EditBox();
            this.uiPanelContractItemList = new Janus.Windows.UI.Dock.UIPanel();
            this.uiPanel2Container = new Janus.Windows.UI.Dock.UIPanelInnerContainer();
            this.grdExContractItemList = new Janus.Windows.GridEX.GridEX();
            this.uiPanelContractItemBtn = new Janus.Windows.UI.Dock.UIPanel();
            this.uiPanel3Container = new Janus.Windows.UI.Dock.UIPanelInnerContainer();
            this.btnAdd1 = new Janus.Windows.EditControls.UIButton();
            this.ebAutoAdd = new Janus.Windows.EditControls.UIButton();
            ((System.ComponentModel.ISupportInitialize)(this.dvAnalysisItemGroup)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.analysisItemGroupDs)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsAnalysisItemGroupDetail)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsContractItem)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiPM)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelAnalysisItem)).BeginInit();
            this.uiPanelAnalysisItem.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelAnalysisItemGroup)).BeginInit();
            this.uiPanelAnalysisItemGroup.SuspendLayout();
            this.uiPanelAnalysisItemGroupContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelAnalysisItemGroupList)).BeginInit();
            this.uiPanelAnalysisItemGroupList.SuspendLayout();
            this.uiPanelAnalysisItemGroupDetailContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdExAnalysisItemGroupList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelAnalysisItemGroupDetail)).BeginInit();
            this.uiPanelAnalysisItemGroupDetail.SuspendLayout();
            this.uiPanel0Container.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tabAnalysisItemGroup)).BeginInit();
            this.tabAnalysisItemGroup.SuspendLayout();
            this.uiTabPage1.SuspendLayout();
            this.uiTabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdExAnalysisItemGroupDetailList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelAnalysisItemGroupDetailBtn)).BeginInit();
            this.uiPanelAnalysisItemGroupDetailBtn.SuspendLayout();
            this.uiPanel1Container.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelContract)).BeginInit();
            this.uiPanelContract.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelContractItem)).BeginInit();
            this.uiPanelContractItem.SuspendLayout();
            this.uiPanelContractItemContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelContractItemList)).BeginInit();
            this.uiPanelContractItemList.SuspendLayout();
            this.uiPanel2Container.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdExContractItemList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelContractItemBtn)).BeginInit();
            this.uiPanelContractItemBtn.SuspendLayout();
            this.uiPanel3Container.SuspendLayout();
            this.SuspendLayout();
            // 
            // dvAnalysisItemGroup
            // 
            this.dvAnalysisItemGroup.Table = this.analysisItemGroupDs.AnalysisItemGroup;
            // 
            // analysisItemGroupDs
            // 
            this.analysisItemGroupDs.DataSetName = "AnalysisItemGroupDs";
            this.analysisItemGroupDs.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // bsAnalysisItemGroupDetail
            // 
            this.bsAnalysisItemGroupDetail.DataMember = "AnalysisItemGroupDetail";
            this.bsAnalysisItemGroupDetail.DataSource = this.analysisItemGroupDs;
            // 
            // bsContractItem
            // 
            this.bsContractItem.DataMember = "ContractItem";
            this.bsContractItem.DataSource = this.analysisItemGroupDs;
            // 
            // uiPM
            // 
            this.uiPM.BackColorGradientAutoHideStrip = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(245)))), ((int)(((byte)(243)))));
            this.uiPM.ContainerControl = this;
            this.uiPM.SplitterSize = 2;
            this.uiPM.VisualStyle = Janus.Windows.UI.Dock.PanelVisualStyle.Office2007;
            this.uiPanelAnalysisItem.Id = new System.Guid("b162c53a-7940-47cd-a0ed-9762c59c50d8");
            this.uiPanelAnalysisItem.StaticGroup = true;
            this.uiPanelAnalysisItemGroup.Id = new System.Guid("afb3b7d0-aa79-443c-a1e8-3baba38f7a4d");
            this.uiPanelAnalysisItem.Panels.Add(this.uiPanelAnalysisItemGroup);
            this.uiPanelAnalysisItemGroupList.Id = new System.Guid("8e3140b9-bfcb-43af-b84e-2ee6cddf51dc");
            this.uiPanelAnalysisItem.Panels.Add(this.uiPanelAnalysisItemGroupList);
            this.uiPanelAnalysisItemGroupDetail.Id = new System.Guid("9ba636d9-19be-49d9-8aaa-bee584a607da");
            this.uiPanelAnalysisItem.Panels.Add(this.uiPanelAnalysisItemGroupDetail);
            this.uiPanelAnalysisItemGroupDetailBtn.Id = new System.Guid("fea5686c-22bd-4ec5-be8a-237e71dac549");
            this.uiPanelAnalysisItem.Panels.Add(this.uiPanelAnalysisItemGroupDetailBtn);
            this.uiPM.Panels.Add(this.uiPanelAnalysisItem);
            this.uiPanelContract.Id = new System.Guid("4d8a4dab-0cff-4972-b1f9-4dc70862bcdf");
            this.uiPanelContract.StaticGroup = true;
            this.uiPanelContractItem.Id = new System.Guid("be6601ff-bb16-4911-bff5-f7babf712cba");
            this.uiPanelContract.Panels.Add(this.uiPanelContractItem);
            this.uiPanelContractItemList.Id = new System.Guid("8916b127-4b71-4a8d-8d10-4eeac8e55f33");
            this.uiPanelContract.Panels.Add(this.uiPanelContractItemList);
            this.uiPanelContractItemBtn.Id = new System.Guid("1afd1add-942f-4ea3-ba3b-7995cfd1de51");
            this.uiPanelContract.Panels.Add(this.uiPanelContractItemBtn);
            this.uiPM.Panels.Add(this.uiPanelContract);
            // 
            // Design Time Panel Info:
            // 
            this.uiPM.BeginPanelInfo();
            this.uiPM.AddDockPanelInfo(new System.Guid("b162c53a-7940-47cd-a0ed-9762c59c50d8"), Janus.Windows.UI.Dock.PanelGroupStyle.HorizontalTiles, Janus.Windows.UI.Dock.PanelDockStyle.Left, true, new System.Drawing.Size(491, 671), true);
            this.uiPM.AddDockPanelInfo(new System.Guid("afb3b7d0-aa79-443c-a1e8-3baba38f7a4d"), new System.Guid("b162c53a-7940-47cd-a0ed-9762c59c50d8"), 60, true);
            this.uiPM.AddDockPanelInfo(new System.Guid("8e3140b9-bfcb-43af-b84e-2ee6cddf51dc"), new System.Guid("b162c53a-7940-47cd-a0ed-9762c59c50d8"), 271, true);
            this.uiPM.AddDockPanelInfo(new System.Guid("9ba636d9-19be-49d9-8aaa-bee584a607da"), new System.Guid("b162c53a-7940-47cd-a0ed-9762c59c50d8"), 255, true);
            this.uiPM.AddDockPanelInfo(new System.Guid("fea5686c-22bd-4ec5-be8a-237e71dac549"), new System.Guid("b162c53a-7940-47cd-a0ed-9762c59c50d8"), 73, true);
            this.uiPM.AddDockPanelInfo(new System.Guid("4d8a4dab-0cff-4972-b1f9-4dc70862bcdf"), Janus.Windows.UI.Dock.PanelGroupStyle.HorizontalTiles, Janus.Windows.UI.Dock.PanelDockStyle.Fill, true, new System.Drawing.Size(513, 671), true);
            this.uiPM.AddDockPanelInfo(new System.Guid("be6601ff-bb16-4911-bff5-f7babf712cba"), new System.Guid("4d8a4dab-0cff-4972-b1f9-4dc70862bcdf"), 60, true);
            this.uiPM.AddDockPanelInfo(new System.Guid("8916b127-4b71-4a8d-8d10-4eeac8e55f33"), new System.Guid("4d8a4dab-0cff-4972-b1f9-4dc70862bcdf"), 567, true);
            this.uiPM.AddDockPanelInfo(new System.Guid("1afd1add-942f-4ea3-ba3b-7995cfd1de51"), new System.Guid("4d8a4dab-0cff-4972-b1f9-4dc70862bcdf"), 40, true);
            this.uiPM.AddFloatingPanelInfo(new System.Guid("b162c53a-7940-47cd-a0ed-9762c59c50d8"), Janus.Windows.UI.Dock.PanelGroupStyle.HorizontalTiles, true, new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
            this.uiPM.AddFloatingPanelInfo(new System.Guid("8e3140b9-bfcb-43af-b84e-2ee6cddf51dc"), new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
            this.uiPM.AddFloatingPanelInfo(new System.Guid("b2973a57-d3e1-4d80-9387-7a6dc3332542"), new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
            this.uiPM.AddFloatingPanelInfo(new System.Guid("afb3b7d0-aa79-443c-a1e8-3baba38f7a4d"), new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
            this.uiPM.AddFloatingPanelInfo(new System.Guid("4d8a4dab-0cff-4972-b1f9-4dc70862bcdf"), Janus.Windows.UI.Dock.PanelGroupStyle.HorizontalTiles, true, new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
            this.uiPM.AddFloatingPanelInfo(new System.Guid("be6601ff-bb16-4911-bff5-f7babf712cba"), new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
            this.uiPM.AddFloatingPanelInfo(new System.Guid("9ba636d9-19be-49d9-8aaa-bee584a607da"), new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
            this.uiPM.AddFloatingPanelInfo(new System.Guid("fea5686c-22bd-4ec5-be8a-237e71dac549"), new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
            this.uiPM.AddFloatingPanelInfo(new System.Guid("8916b127-4b71-4a8d-8d10-4eeac8e55f33"), new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
            this.uiPM.AddFloatingPanelInfo(new System.Guid("1afd1add-942f-4ea3-ba3b-7995cfd1de51"), new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
            this.uiPM.EndPanelInfo();
            // 
            // uiPanelAnalysisItem
            // 
            this.uiPanelAnalysisItem.ActiveCaptionMode = Janus.Windows.UI.Dock.ActiveCaptionMode.Never;
            this.uiPanelAnalysisItem.CaptionVisible = Janus.Windows.UI.InheritableBoolean.False;
            this.uiPanelAnalysisItem.CloseButtonVisible = Janus.Windows.UI.InheritableBoolean.False;
            this.uiPanelAnalysisItem.Location = new System.Drawing.Point(3, 3);
            this.uiPanelAnalysisItem.Name = "uiPanelAnalysisItem";
            this.uiPanelAnalysisItem.Size = new System.Drawing.Size(491, 671);
            this.uiPanelAnalysisItem.SplitterSize = 2;
            this.uiPanelAnalysisItem.TabIndex = 4;
            // 
            // uiPanelAnalysisItemGroup
            // 
            this.uiPanelAnalysisItemGroup.AllowResize = Janus.Windows.UI.InheritableBoolean.True;
            this.uiPanelAnalysisItemGroup.CaptionVisible = Janus.Windows.UI.InheritableBoolean.True;
            this.uiPanelAnalysisItemGroup.CloseButtonVisible = Janus.Windows.UI.InheritableBoolean.False;
            this.uiPanelAnalysisItemGroup.InnerAreaStyle = Janus.Windows.UI.Dock.PanelInnerAreaStyle.Window;
            this.uiPanelAnalysisItemGroup.InnerContainer = this.uiPanelAnalysisItemGroupContainer;
            this.uiPanelAnalysisItemGroup.Location = new System.Drawing.Point(0, 0);
            this.uiPanelAnalysisItemGroup.MaximumSize = new System.Drawing.Size(-1, 60);
            this.uiPanelAnalysisItemGroup.MinimumSize = new System.Drawing.Size(-1, 60);
            this.uiPanelAnalysisItemGroup.Name = "uiPanelAnalysisItemGroup";
            this.uiPanelAnalysisItemGroup.Size = new System.Drawing.Size(489, 60);
            this.uiPanelAnalysisItemGroup.TabIndex = 0;
            this.uiPanelAnalysisItemGroup.Text = "분석용 광고묶음";
            // 
            // uiPanelAnalysisItemGroupContainer
            // 
            this.uiPanelAnalysisItemGroupContainer.BackColor = System.Drawing.SystemColors.Window;
            this.uiPanelAnalysisItemGroupContainer.Controls.Add(this.cbSearchMonth);
            this.uiPanelAnalysisItemGroupContainer.Controls.Add(this.ebSearchKey);
            this.uiPanelAnalysisItemGroupContainer.Controls.Add(this.btnSearch);
            this.uiPanelAnalysisItemGroupContainer.Location = new System.Drawing.Point(1, 23);
            this.uiPanelAnalysisItemGroupContainer.Name = "uiPanelAnalysisItemGroupContainer";
            this.uiPanelAnalysisItemGroupContainer.Size = new System.Drawing.Size(487, 36);
            this.uiPanelAnalysisItemGroupContainer.TabIndex = 0;
            // 
            // cbSearchMonth
            // 
            this.cbSearchMonth.ComboStyle = Janus.Windows.EditControls.ComboStyle.DropDownList;
            this.cbSearchMonth.Location = new System.Drawing.Point(8, 8);
            this.cbSearchMonth.Name = "cbSearchMonth";
            this.cbSearchMonth.Size = new System.Drawing.Size(128, 20);
            this.cbSearchMonth.TabIndex = 1;
            this.cbSearchMonth.Text = "수행월 전체";
            this.cbSearchMonth.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.cbSearchMonth.SelectedIndexChanged += new System.EventHandler(this.cbSearchMonth_SelectedIndexChanged);
            // 
            // ebSearchKey
            // 
            this.ebSearchKey.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ebSearchKey.Location = new System.Drawing.Point(163, 8);
            this.ebSearchKey.MaxLength = 25;
            this.ebSearchKey.Name = "ebSearchKey";
            this.ebSearchKey.Size = new System.Drawing.Size(208, 20);
            this.ebSearchKey.TabIndex = 2;
            this.ebSearchKey.Text = "검색어를 입력하세요";
            this.ebSearchKey.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
            this.ebSearchKey.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            this.ebSearchKey.Click += new System.EventHandler(this.ebSearchKey_Click);
            // 
            // btnSearch
            // 
            this.btnSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSearch.Location = new System.Drawing.Point(377, 6);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(104, 24);
            this.btnSearch.TabIndex = 3;
            this.btnSearch.Text = "조 회";
            this.btnSearch.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // uiPanelAnalysisItemGroupList
            // 
            this.uiPanelAnalysisItemGroupList.CaptionStyle = Janus.Windows.UI.Dock.PanelCaptionStyle.Dark;
            this.uiPanelAnalysisItemGroupList.CaptionVisible = Janus.Windows.UI.InheritableBoolean.False;
            this.uiPanelAnalysisItemGroupList.InnerAreaStyle = Janus.Windows.UI.Dock.PanelInnerAreaStyle.Window;
            this.uiPanelAnalysisItemGroupList.InnerContainer = this.uiPanelAnalysisItemGroupDetailContainer;
            this.uiPanelAnalysisItemGroupList.Location = new System.Drawing.Point(0, 62);
            this.uiPanelAnalysisItemGroupList.MinimumSize = new System.Drawing.Size(-1, 100);
            this.uiPanelAnalysisItemGroupList.Name = "uiPanelAnalysisItemGroupList";
            this.uiPanelAnalysisItemGroupList.Size = new System.Drawing.Size(489, 294);
            this.uiPanelAnalysisItemGroupList.TabIndex = 1;
            // 
            // uiPanelAnalysisItemGroupDetailContainer
            // 
            this.uiPanelAnalysisItemGroupDetailContainer.BackColor = System.Drawing.SystemColors.Window;
            this.uiPanelAnalysisItemGroupDetailContainer.Controls.Add(this.grdExAnalysisItemGroupList);
            this.uiPanelAnalysisItemGroupDetailContainer.Location = new System.Drawing.Point(1, 1);
            this.uiPanelAnalysisItemGroupDetailContainer.Name = "uiPanelAnalysisItemGroupDetailContainer";
            this.uiPanelAnalysisItemGroupDetailContainer.Size = new System.Drawing.Size(487, 292);
            this.uiPanelAnalysisItemGroupDetailContainer.TabIndex = 0;
            // 
            // grdExAnalysisItemGroupList
            // 
            this.grdExAnalysisItemGroupList.AlternatingColors = true;
            this.grdExAnalysisItemGroupList.BorderStyle = Janus.Windows.GridEX.BorderStyle.Flat;
            this.grdExAnalysisItemGroupList.ColumnAutoResize = true;
            this.grdExAnalysisItemGroupList.DataSource = this.dvAnalysisItemGroup;
            this.grdExAnalysisItemGroupList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdExAnalysisItemGroupList.EmptyRows = true;
            this.grdExAnalysisItemGroupList.FocusCellFormatStyle.BackColor = System.Drawing.SystemColors.Highlight;
            this.grdExAnalysisItemGroupList.FocusCellFormatStyle.ForeColor = System.Drawing.Color.White;
            this.grdExAnalysisItemGroupList.FocusStyle = Janus.Windows.GridEX.FocusStyle.None;
            this.grdExAnalysisItemGroupList.Font = new System.Drawing.Font("굴림", 9F);
            this.grdExAnalysisItemGroupList.GridLines = Janus.Windows.GridEX.GridLines.Vertical;
            this.grdExAnalysisItemGroupList.GridLineStyle = Janus.Windows.GridEX.GridLineStyle.Solid;
            this.grdExAnalysisItemGroupList.GroupByBoxVisible = false;
            this.grdExAnalysisItemGroupList.GroupMode = Janus.Windows.GridEX.GroupMode.Collapsed;
            this.grdExAnalysisItemGroupList.HeaderFormatStyle.TextAlignment = Janus.Windows.GridEX.TextAlignment.Center;
            grdExAnalysisItemGroupList_Layout_0.DataSource = this.dvAnalysisItemGroup;
            grdExAnalysisItemGroupList_Layout_0.IsCurrentLayout = true;
            grdExAnalysisItemGroupList_Layout_0.Key = "bae";
            grdExAnalysisItemGroupList_Layout_0.LayoutString = resources.GetString("grdExAnalysisItemGroupList_Layout_0.LayoutString");
            this.grdExAnalysisItemGroupList.Layouts.AddRange(new Janus.Windows.GridEX.GridEXLayout[] {
            grdExAnalysisItemGroupList_Layout_0});
            this.grdExAnalysisItemGroupList.Location = new System.Drawing.Point(0, 0);
            this.grdExAnalysisItemGroupList.Name = "grdExAnalysisItemGroupList";
            this.grdExAnalysisItemGroupList.ScrollBars = Janus.Windows.GridEX.ScrollBars.Vertical;
            this.grdExAnalysisItemGroupList.Size = new System.Drawing.Size(487, 292);
            this.grdExAnalysisItemGroupList.TabIndex = 4;
            this.grdExAnalysisItemGroupList.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            this.grdExAnalysisItemGroupList.SelectionChanged += new System.EventHandler(this.grdExAnalysisItemGroupList_SelectionChanged);
            // 
            // uiPanelAnalysisItemGroupDetail
            // 
            this.uiPanelAnalysisItemGroupDetail.InnerAreaStyle = Janus.Windows.UI.Dock.PanelInnerAreaStyle.Window;
            this.uiPanelAnalysisItemGroupDetail.InnerContainer = this.uiPanel0Container;
            this.uiPanelAnalysisItemGroupDetail.Location = new System.Drawing.Point(0, 358);
            this.uiPanelAnalysisItemGroupDetail.MaximumSize = new System.Drawing.Size(-1, 271);
            this.uiPanelAnalysisItemGroupDetail.MinimumSize = new System.Drawing.Size(-1, 271);
            this.uiPanelAnalysisItemGroupDetail.Name = "uiPanelAnalysisItemGroupDetail";
            this.uiPanelAnalysisItemGroupDetail.Size = new System.Drawing.Size(489, 271);
            this.uiPanelAnalysisItemGroupDetail.TabIndex = 4;
            this.uiPanelAnalysisItemGroupDetail.Text = "광고묶음 내용";
            // 
            // uiPanel0Container
            // 
            this.uiPanel0Container.Controls.Add(this.tabAnalysisItemGroup);
            this.uiPanel0Container.Location = new System.Drawing.Point(1, 23);
            this.uiPanel0Container.Name = "uiPanel0Container";
            this.uiPanel0Container.Size = new System.Drawing.Size(487, 247);
            this.uiPanel0Container.TabIndex = 0;
            // 
            // tabAnalysisItemGroup
            // 
            this.tabAnalysisItemGroup.BackColor = System.Drawing.SystemColors.Window;
            this.tabAnalysisItemGroup.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabAnalysisItemGroup.Location = new System.Drawing.Point(0, 0);
            this.tabAnalysisItemGroup.Name = "tabAnalysisItemGroup";
            this.tabAnalysisItemGroup.Size = new System.Drawing.Size(487, 247);
            this.tabAnalysisItemGroup.TabIndex = 0;
            this.tabAnalysisItemGroup.TabPages.AddRange(new Janus.Windows.UI.Tab.UITabPage[] {
            this.uiTabPage1,
            this.uiTabPage2});
            this.tabAnalysisItemGroup.SelectedTabChanged += new Janus.Windows.UI.Tab.TabEventHandler(this.tabAnalysisItemGroup_SelectedTabChanged);
            // 
            // uiTabPage1
            // 
            this.uiTabPage1.Controls.Add(this.ebModDt);
            this.uiTabPage1.Controls.Add(this.cbAnalysisItemGroupType);
            this.uiTabPage1.Controls.Add(this.ebComment);
            this.uiTabPage1.Controls.Add(this.ebAnalysisItemGroupName);
            this.uiTabPage1.Controls.Add(this.ebRegDt);
            this.uiTabPage1.Controls.Add(this.lbModDt);
            this.uiTabPage1.Controls.Add(this.label4);
            this.uiTabPage1.Controls.Add(this.label3);
            this.uiTabPage1.Controls.Add(this.label1);
            this.uiTabPage1.Controls.Add(this.label2);
            this.uiTabPage1.Controls.Add(this.ebRegID);
            this.uiTabPage1.Controls.Add(this.lbRegID);
            this.uiTabPage1.Controls.Add(this.ebRegName);
            this.uiTabPage1.Location = new System.Drawing.Point(1, 21);
            this.uiTabPage1.Name = "uiTabPage1";
            this.uiTabPage1.Size = new System.Drawing.Size(483, 223);
            this.uiTabPage1.TabStop = true;
            this.uiTabPage1.Text = "상세정보";
            // 
            // ebModDt
            // 
            this.ebModDt.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ebModDt.Location = new System.Drawing.Point(80, 170);
            this.ebModDt.Name = "ebModDt";
            this.ebModDt.ReadOnly = true;
            this.ebModDt.Size = new System.Drawing.Size(168, 20);
            this.ebModDt.TabIndex = 15;
            this.ebModDt.TabStop = false;
            this.ebModDt.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
            this.ebModDt.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            // 
            // cbAnalysisItemGroupType
            // 
            this.cbAnalysisItemGroupType.ComboStyle = Janus.Windows.EditControls.ComboStyle.DropDownList;
            this.cbAnalysisItemGroupType.Location = new System.Drawing.Point(80, 31);
            this.cbAnalysisItemGroupType.Name = "cbAnalysisItemGroupType";
            this.cbAnalysisItemGroupType.Size = new System.Drawing.Size(128, 20);
            this.cbAnalysisItemGroupType.TabIndex = 6;
            this.cbAnalysisItemGroupType.Text = "기본";
            this.cbAnalysisItemGroupType.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            // 
            // ebComment
            // 
            this.ebComment.Location = new System.Drawing.Point(80, 55);
            this.ebComment.MaxLength = 100;
            this.ebComment.Multiline = true;
            this.ebComment.Name = "ebComment";
            this.ebComment.Size = new System.Drawing.Size(342, 88);
            this.ebComment.TabIndex = 7;
            this.ebComment.TabStop = false;
            this.ebComment.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
            this.ebComment.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            // 
            // ebAnalysisItemGroupName
            // 
            this.ebAnalysisItemGroupName.Location = new System.Drawing.Point(80, 7);
            this.ebAnalysisItemGroupName.MaxLength = 25;
            this.ebAnalysisItemGroupName.Name = "ebAnalysisItemGroupName";
            this.ebAnalysisItemGroupName.Size = new System.Drawing.Size(342, 20);
            this.ebAnalysisItemGroupName.TabIndex = 5;
            this.ebAnalysisItemGroupName.TabStop = false;
            this.ebAnalysisItemGroupName.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
            this.ebAnalysisItemGroupName.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            // 
            // ebRegDt
            // 
            this.ebRegDt.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ebRegDt.Location = new System.Drawing.Point(80, 146);
            this.ebRegDt.Name = "ebRegDt";
            this.ebRegDt.ReadOnly = true;
            this.ebRegDt.Size = new System.Drawing.Size(168, 20);
            this.ebRegDt.TabIndex = 14;
            this.ebRegDt.TabStop = false;
            this.ebRegDt.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
            this.ebRegDt.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            // 
            // lbModDt
            // 
            this.lbModDt.Location = new System.Drawing.Point(8, 170);
            this.lbModDt.Name = "lbModDt";
            this.lbModDt.Size = new System.Drawing.Size(72, 21);
            this.lbModDt.TabIndex = 13;
            this.lbModDt.Text = "수정일시";
            this.lbModDt.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(8, 8);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(83, 21);
            this.label4.TabIndex = 11;
            this.label4.Text = "광고묶음명";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(8, 32);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(72, 21);
            this.label3.TabIndex = 11;
            this.label3.Text = "타입";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(8, 56);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(72, 21);
            this.label1.TabIndex = 11;
            this.label1.Text = "비고";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(8, 146);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(72, 21);
            this.label2.TabIndex = 11;
            this.label2.Text = "등록일시";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ebRegID
            // 
            this.ebRegID.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ebRegID.Location = new System.Drawing.Point(80, 194);
            this.ebRegID.Name = "ebRegID";
            this.ebRegID.ReadOnly = true;
            this.ebRegID.Size = new System.Drawing.Size(168, 20);
            this.ebRegID.TabIndex = 16;
            this.ebRegID.TabStop = false;
            this.ebRegID.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
            this.ebRegID.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            // 
            // lbRegID
            // 
            this.lbRegID.Location = new System.Drawing.Point(8, 194);
            this.lbRegID.Name = "lbRegID";
            this.lbRegID.Size = new System.Drawing.Size(72, 21);
            this.lbRegID.TabIndex = 12;
            this.lbRegID.Text = "등록자ID";
            this.lbRegID.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ebRegName
            // 
            this.ebRegName.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ebRegName.Location = new System.Drawing.Point(254, 194);
            this.ebRegName.Name = "ebRegName";
            this.ebRegName.ReadOnly = true;
            this.ebRegName.Size = new System.Drawing.Size(168, 20);
            this.ebRegName.TabIndex = 16;
            this.ebRegName.TabStop = false;
            this.ebRegName.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
            this.ebRegName.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            // 
            // uiTabPage2
            // 
            this.uiTabPage2.Controls.Add(this.grdExAnalysisItemGroupDetailList);
            this.uiTabPage2.Location = new System.Drawing.Point(1, 21);
            this.uiTabPage2.Name = "uiTabPage2";
            this.uiTabPage2.Size = new System.Drawing.Size(483, 223);
            this.uiTabPage2.TabStop = true;
            this.uiTabPage2.Text = "광고목록";
            // 
            // grdExAnalysisItemGroupDetailList
            // 
            this.grdExAnalysisItemGroupDetailList.AlternatingColors = true;
            this.grdExAnalysisItemGroupDetailList.BorderStyle = Janus.Windows.GridEX.BorderStyle.Flat;
            this.grdExAnalysisItemGroupDetailList.ColumnAutoResize = true;
            this.grdExAnalysisItemGroupDetailList.DataSource = this.bsAnalysisItemGroupDetail;
            this.grdExAnalysisItemGroupDetailList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdExAnalysisItemGroupDetailList.EmptyRows = true;
            this.grdExAnalysisItemGroupDetailList.FocusCellFormatStyle.BackColor = System.Drawing.SystemColors.Highlight;
            this.grdExAnalysisItemGroupDetailList.FocusCellFormatStyle.ForeColor = System.Drawing.Color.White;
            this.grdExAnalysisItemGroupDetailList.FocusStyle = Janus.Windows.GridEX.FocusStyle.None;
            this.grdExAnalysisItemGroupDetailList.Font = new System.Drawing.Font("굴림", 9F);
            this.grdExAnalysisItemGroupDetailList.GridLines = Janus.Windows.GridEX.GridLines.Vertical;
            this.grdExAnalysisItemGroupDetailList.GridLineStyle = Janus.Windows.GridEX.GridLineStyle.Solid;
            this.grdExAnalysisItemGroupDetailList.GroupByBoxVisible = false;
            this.grdExAnalysisItemGroupDetailList.GroupMode = Janus.Windows.GridEX.GroupMode.Collapsed;
            this.grdExAnalysisItemGroupDetailList.HeaderFormatStyle.TextAlignment = Janus.Windows.GridEX.TextAlignment.Center;
            grdExAnalysisItemGroupDetailList_Layout_0.DataSource = this.bsAnalysisItemGroupDetail;
            grdExAnalysisItemGroupDetailList_Layout_0.IsCurrentLayout = true;
            grdExAnalysisItemGroupDetailList_Layout_0.Key = "bae";
            grdExAnalysisItemGroupDetailList_Layout_0.LayoutString = resources.GetString("grdExAnalysisItemGroupDetailList_Layout_0.LayoutString");
            this.grdExAnalysisItemGroupDetailList.Layouts.AddRange(new Janus.Windows.GridEX.GridEXLayout[] {
            grdExAnalysisItemGroupDetailList_Layout_0});
            this.grdExAnalysisItemGroupDetailList.Location = new System.Drawing.Point(0, 0);
            this.grdExAnalysisItemGroupDetailList.Name = "grdExAnalysisItemGroupDetailList";
            this.grdExAnalysisItemGroupDetailList.ScrollBars = Janus.Windows.GridEX.ScrollBars.Vertical;
            this.grdExAnalysisItemGroupDetailList.Size = new System.Drawing.Size(483, 223);
            this.grdExAnalysisItemGroupDetailList.TabIndex = 0;
            this.grdExAnalysisItemGroupDetailList.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            // 
            // uiPanelAnalysisItemGroupDetailBtn
            // 
            this.uiPanelAnalysisItemGroupDetailBtn.CaptionVisible = Janus.Windows.UI.InheritableBoolean.False;
            this.uiPanelAnalysisItemGroupDetailBtn.InnerAreaStyle = Janus.Windows.UI.Dock.PanelInnerAreaStyle.Window;
            this.uiPanelAnalysisItemGroupDetailBtn.InnerContainer = this.uiPanel1Container;
            this.uiPanelAnalysisItemGroupDetailBtn.Location = new System.Drawing.Point(0, 631);
            this.uiPanelAnalysisItemGroupDetailBtn.MaximumSize = new System.Drawing.Size(-1, 40);
            this.uiPanelAnalysisItemGroupDetailBtn.MinimumSize = new System.Drawing.Size(-1, 40);
            this.uiPanelAnalysisItemGroupDetailBtn.Name = "uiPanelAnalysisItemGroupDetailBtn";
            this.uiPanelAnalysisItemGroupDetailBtn.Size = new System.Drawing.Size(489, 40);
            this.uiPanelAnalysisItemGroupDetailBtn.TabIndex = 4;
            this.uiPanelAnalysisItemGroupDetailBtn.Text = "Panel 1";
            // 
            // uiPanel1Container
            // 
            this.uiPanel1Container.BackColor = System.Drawing.SystemColors.Window;
            this.uiPanel1Container.Controls.Add(this.btnDelete);
            this.uiPanel1Container.Controls.Add(this.btnSave);
            this.uiPanel1Container.Controls.Add(this.btnAdd);
            this.uiPanel1Container.Location = new System.Drawing.Point(1, 1);
            this.uiPanel1Container.Name = "uiPanel1Container";
            this.uiPanel1Container.Size = new System.Drawing.Size(487, 38);
            this.uiPanel1Container.TabIndex = 0;
            // 
            // btnDelete
            // 
            this.btnDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnDelete.Location = new System.Drawing.Point(116, 7);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(104, 24);
            this.btnDelete.TabIndex = 9;
            this.btnDelete.Text = "삭 제";
            this.btnDelete.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnSave.Location = new System.Drawing.Point(6, 7);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(104, 24);
            this.btnSave.TabIndex = 8;
            this.btnSave.Text = "저 장";
            this.btnSave.ToolTipText = "입력된 정보로 저장됩니다.";
            this.btnSave.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnAdd
            // 
            this.btnAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnAdd.Location = new System.Drawing.Point(226, 7);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(104, 24);
            this.btnAdd.TabIndex = 10;
            this.btnAdd.Text = "추 가";
            this.btnAdd.ToolTipText = "새로운 광고묶음을 생성합니다.";
            this.btnAdd.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // uiPanelContract
            // 
            this.uiPanelContract.CaptionVisible = Janus.Windows.UI.InheritableBoolean.False;
            this.uiPanelContract.Location = new System.Drawing.Point(494, 3);
            this.uiPanelContract.Name = "uiPanelContract";
            this.uiPanelContract.Size = new System.Drawing.Size(513, 671);
            this.uiPanelContract.SplitterSize = 2;
            this.uiPanelContract.TabIndex = 4;
            // 
            // uiPanelContractItem
            // 
            this.uiPanelContractItem.InnerAreaStyle = Janus.Windows.UI.Dock.PanelInnerAreaStyle.Window;
            this.uiPanelContractItem.InnerContainer = this.uiPanelContractItemContainer;
            this.uiPanelContractItem.Location = new System.Drawing.Point(0, 0);
            this.uiPanelContractItem.MaximumSize = new System.Drawing.Size(-1, 60);
            this.uiPanelContractItem.MinimumSize = new System.Drawing.Size(-1, 60);
            this.uiPanelContractItem.Name = "uiPanelContractItem";
            this.uiPanelContractItem.Size = new System.Drawing.Size(513, 60);
            this.uiPanelContractItem.TabIndex = 4;
            this.uiPanelContractItem.Text = "분석대상 광고목록";
            // 
            // uiPanelContractItemContainer
            // 
            this.uiPanelContractItemContainer.BackColor = System.Drawing.SystemColors.Window;
            this.uiPanelContractItemContainer.Controls.Add(this.btnSearch1);
            this.uiPanelContractItemContainer.Controls.Add(this.ebSearchKey1);
            this.uiPanelContractItemContainer.Location = new System.Drawing.Point(1, 23);
            this.uiPanelContractItemContainer.Name = "uiPanelContractItemContainer";
            this.uiPanelContractItemContainer.Size = new System.Drawing.Size(511, 36);
            this.uiPanelContractItemContainer.TabIndex = 0;
            // 
            // btnSearch1
            // 
            this.btnSearch1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSearch1.Location = new System.Drawing.Point(403, 6);
            this.btnSearch1.Name = "btnSearch1";
            this.btnSearch1.Size = new System.Drawing.Size(104, 24);
            this.btnSearch1.TabIndex = 12;
            this.btnSearch1.Text = "조 회";
            this.btnSearch1.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.btnSearch1.Click += new System.EventHandler(this.btnSearch1_Click);
            // 
            // ebSearchKey1
            // 
            this.ebSearchKey1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ebSearchKey1.Location = new System.Drawing.Point(189, 8);
            this.ebSearchKey1.MaxLength = 25;
            this.ebSearchKey1.Name = "ebSearchKey1";
            this.ebSearchKey1.Size = new System.Drawing.Size(208, 20);
            this.ebSearchKey1.TabIndex = 11;
            this.ebSearchKey1.Text = "검색어를 입력하세요";
            this.ebSearchKey1.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
            this.ebSearchKey1.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            this.ebSearchKey1.Click += new System.EventHandler(this.ebSearchKey1_Click);
            // 
            // uiPanelContractItemList
            // 
            this.uiPanelContractItemList.CaptionVisible = Janus.Windows.UI.InheritableBoolean.False;
            this.uiPanelContractItemList.InnerAreaStyle = Janus.Windows.UI.Dock.PanelInnerAreaStyle.Window;
            this.uiPanelContractItemList.InnerContainer = this.uiPanel2Container;
            this.uiPanelContractItemList.Location = new System.Drawing.Point(0, 62);
            this.uiPanelContractItemList.Name = "uiPanelContractItemList";
            this.uiPanelContractItemList.Size = new System.Drawing.Size(513, 567);
            this.uiPanelContractItemList.TabIndex = 4;
            this.uiPanelContractItemList.Text = "Panel 2";
            // 
            // uiPanel2Container
            // 
            this.uiPanel2Container.Controls.Add(this.grdExContractItemList);
            this.uiPanel2Container.Location = new System.Drawing.Point(1, 1);
            this.uiPanel2Container.Name = "uiPanel2Container";
            this.uiPanel2Container.Size = new System.Drawing.Size(511, 565);
            this.uiPanel2Container.TabIndex = 0;
            // 
            // grdExContractItemList
            // 
            this.grdExContractItemList.AlternatingColors = true;
            this.grdExContractItemList.BorderStyle = Janus.Windows.GridEX.BorderStyle.Flat;
            this.grdExContractItemList.ColumnAutoResize = true;
            this.grdExContractItemList.DataSource = this.bsContractItem;
            this.grdExContractItemList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdExContractItemList.EmptyRows = true;
            this.grdExContractItemList.FocusCellFormatStyle.BackColor = System.Drawing.SystemColors.Highlight;
            this.grdExContractItemList.FocusCellFormatStyle.ForeColor = System.Drawing.Color.White;
            this.grdExContractItemList.FocusStyle = Janus.Windows.GridEX.FocusStyle.None;
            this.grdExContractItemList.Font = new System.Drawing.Font("굴림", 9F);
            this.grdExContractItemList.GridLines = Janus.Windows.GridEX.GridLines.Vertical;
            this.grdExContractItemList.GridLineStyle = Janus.Windows.GridEX.GridLineStyle.Solid;
            this.grdExContractItemList.GroupByBoxVisible = false;
            this.grdExContractItemList.GroupMode = Janus.Windows.GridEX.GroupMode.Collapsed;
            this.grdExContractItemList.HeaderFormatStyle.TextAlignment = Janus.Windows.GridEX.TextAlignment.Center;
            grdExContractItemList_Layout_0.DataSource = this.bsContractItem;
            grdExContractItemList_Layout_0.IsCurrentLayout = true;
            grdExContractItemList_Layout_0.Key = "Lee";
            grdExContractItemList_Layout_0.LayoutString = resources.GetString("grdExContractItemList_Layout_0.LayoutString");
            this.grdExContractItemList.Layouts.AddRange(new Janus.Windows.GridEX.GridEXLayout[] {
            grdExContractItemList_Layout_0});
            this.grdExContractItemList.Location = new System.Drawing.Point(0, 0);
            this.grdExContractItemList.Name = "grdExContractItemList";
            this.grdExContractItemList.ScrollBars = Janus.Windows.GridEX.ScrollBars.Vertical;
            this.grdExContractItemList.Size = new System.Drawing.Size(511, 565);
            this.grdExContractItemList.TabIndex = 13;
            this.grdExContractItemList.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            this.grdExContractItemList.SelectionChanged += new System.EventHandler(this.grdExContractItemList_SelectionChanged);
            // 
            // uiPanelContractItemBtn
            // 
            this.uiPanelContractItemBtn.CaptionVisible = Janus.Windows.UI.InheritableBoolean.False;
            this.uiPanelContractItemBtn.InnerAreaStyle = Janus.Windows.UI.Dock.PanelInnerAreaStyle.Window;
            this.uiPanelContractItemBtn.InnerContainer = this.uiPanel3Container;
            this.uiPanelContractItemBtn.Location = new System.Drawing.Point(0, 631);
            this.uiPanelContractItemBtn.MaximumSize = new System.Drawing.Size(-1, 40);
            this.uiPanelContractItemBtn.MinimumSize = new System.Drawing.Size(-1, 40);
            this.uiPanelContractItemBtn.Name = "uiPanelContractItemBtn";
            this.uiPanelContractItemBtn.Size = new System.Drawing.Size(513, 40);
            this.uiPanelContractItemBtn.TabIndex = 4;
            this.uiPanelContractItemBtn.Text = "Panel 3";
            // 
            // uiPanel3Container
            // 
            this.uiPanel3Container.Controls.Add(this.btnAdd1);
            this.uiPanel3Container.Controls.Add(this.ebAutoAdd);
            this.uiPanel3Container.Location = new System.Drawing.Point(1, 1);
            this.uiPanel3Container.Name = "uiPanel3Container";
            this.uiPanel3Container.Size = new System.Drawing.Size(511, 38);
            this.uiPanel3Container.TabIndex = 0;
            // 
            // btnAdd1
            // 
            this.btnAdd1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAdd1.Location = new System.Drawing.Point(403, 7);
            this.btnAdd1.Name = "btnAdd1";
            this.btnAdd1.Size = new System.Drawing.Size(104, 24);
            this.btnAdd1.TabIndex = 15;
            this.btnAdd1.Tag = "";
            this.btnAdd1.Text = "추가";
            this.btnAdd1.ToolTipText = "선택된 광고목록이 광고묶음에 추가됩니다.";
            this.btnAdd1.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.btnAdd1.Click += new System.EventHandler(this.btnAdd1_Click);
            // 
            // ebAutoAdd
            // 
            this.ebAutoAdd.Location = new System.Drawing.Point(5, 7);
            this.ebAutoAdd.Name = "ebAutoAdd";
            this.ebAutoAdd.Size = new System.Drawing.Size(104, 24);
            this.ebAutoAdd.TabIndex = 14;
            this.ebAutoAdd.Tag = "";
            this.ebAutoAdd.Text = "자동등록";
            this.ebAutoAdd.ToolTipText = "선택된 광고목록과 같은 미디어랩, 대행사, 광고주의 목록을 자동등록합니다.";
            this.ebAutoAdd.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.ebAutoAdd.Click += new System.EventHandler(this.ebAutoAdd_Click);
            // 
            // AnalysisItemGroupControl
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.Controls.Add(this.uiPanelContract);
            this.Controls.Add(this.uiPanelAnalysisItem);
            this.Font = global::AdManagerClient.Properties.Settings.Default.Font1;
            this.Name = "AnalysisItemGroupControl";
            this.Size = new System.Drawing.Size(1010, 677);
            this.Load += new System.EventHandler(this.AnalysisItemGroupControl_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dvAnalysisItemGroup)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.analysisItemGroupDs)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsAnalysisItemGroupDetail)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsContractItem)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiPM)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelAnalysisItem)).EndInit();
            this.uiPanelAnalysisItem.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelAnalysisItemGroup)).EndInit();
            this.uiPanelAnalysisItemGroup.ResumeLayout(false);
            this.uiPanelAnalysisItemGroupContainer.ResumeLayout(false);
            this.uiPanelAnalysisItemGroupContainer.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelAnalysisItemGroupList)).EndInit();
            this.uiPanelAnalysisItemGroupList.ResumeLayout(false);
            this.uiPanelAnalysisItemGroupDetailContainer.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdExAnalysisItemGroupList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelAnalysisItemGroupDetail)).EndInit();
            this.uiPanelAnalysisItemGroupDetail.ResumeLayout(false);
            this.uiPanel0Container.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.tabAnalysisItemGroup)).EndInit();
            this.tabAnalysisItemGroup.ResumeLayout(false);
            this.uiTabPage1.ResumeLayout(false);
            this.uiTabPage1.PerformLayout();
            this.uiTabPage2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdExAnalysisItemGroupDetailList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelAnalysisItemGroupDetailBtn)).EndInit();
            this.uiPanelAnalysisItemGroupDetailBtn.ResumeLayout(false);
            this.uiPanel1Container.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelContract)).EndInit();
            this.uiPanelContract.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelContractItem)).EndInit();
            this.uiPanelContractItem.ResumeLayout(false);
            this.uiPanelContractItemContainer.ResumeLayout(false);
            this.uiPanelContractItemContainer.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelContractItemList)).EndInit();
            this.uiPanelContractItemList.ResumeLayout(false);
            this.uiPanel2Container.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdExContractItemList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelContractItemBtn)).EndInit();
            this.uiPanelContractItemBtn.ResumeLayout(false);
            this.uiPanel3Container.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Janus.Windows.UI.Dock.UIPanelManager uiPM;
        private Janus.Windows.UI.Dock.UIPanelGroup uiPanelAnalysisItem;
        private Janus.Windows.UI.Dock.UIPanel uiPanelAnalysisItemGroup;
        private Janus.Windows.UI.Dock.UIPanelInnerContainer uiPanelAnalysisItemGroupContainer;
        private Janus.Windows.UI.Dock.UIPanel uiPanelAnalysisItemGroupList;
        private Janus.Windows.UI.Dock.UIPanelInnerContainer uiPanelAnalysisItemGroupDetailContainer;
        private Janus.Windows.UI.Dock.UIPanelGroup uiPanelContract;
        private Janus.Windows.UI.Dock.UIPanel uiPanelContractItem;
        private Janus.Windows.UI.Dock.UIPanelInnerContainer uiPanelContractItemContainer;
        private Janus.Windows.UI.Tab.UITab tabAnalysisItemGroup;
        private Janus.Windows.UI.Tab.UITabPage uiTabPage1;
        private Janus.Windows.UI.Tab.UITabPage uiTabPage2;
        private Janus.Windows.GridEX.EditControls.EditBox ebSearchKey;
        private Janus.Windows.EditControls.UIButton btnSearch;
        private Janus.Windows.EditControls.UIComboBox cbSearchMonth;
        private Janus.Windows.EditControls.UIButton btnSearch1;
        private Janus.Windows.GridEX.EditControls.EditBox ebSearchKey1;
        private Janus.Windows.GridEX.GridEX grdExAnalysisItemGroupList;
        private Janus.Windows.GridEX.GridEX grdExAnalysisItemGroupDetailList;
        private Janus.Windows.GridEX.GridEX grdExContractItemList;
        private Janus.Windows.EditControls.UIButton ebAutoAdd;
        private Janus.Windows.EditControls.UIButton btnAdd1;
        private Janus.Windows.GridEX.EditControls.EditBox ebRegDt;
        private System.Windows.Forms.Label lbModDt;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private Janus.Windows.GridEX.EditControls.EditBox ebRegID;
        private System.Windows.Forms.Label lbRegID;
        private Janus.Windows.EditControls.UIComboBox cbAnalysisItemGroupType;
        private Janus.Windows.GridEX.EditControls.EditBox ebComment;
        private Janus.Windows.GridEX.EditControls.EditBox ebAnalysisItemGroupName;
        private Janus.Windows.EditControls.UIButton btnDelete;
        private Janus.Windows.EditControls.UIButton btnAdd;
        private Janus.Windows.EditControls.UIButton btnSave;
        private Janus.Windows.GridEX.EditControls.EditBox ebModDt;
        private Janus.Windows.GridEX.EditControls.EditBox ebRegName;
        private System.Windows.Forms.BindingSource bsAnalysisItemGroupDetail;
        private System.Windows.Forms.BindingSource bsContractItem;
        private System.Data.DataView dvAnalysisItemGroup;
        private Janus.Windows.UI.Dock.UIPanel uiPanelAnalysisItemGroupDetail;
        private Janus.Windows.UI.Dock.UIPanelInnerContainer uiPanel0Container;
        private Janus.Windows.UI.Dock.UIPanel uiPanelAnalysisItemGroupDetailBtn;
        private Janus.Windows.UI.Dock.UIPanelInnerContainer uiPanel1Container;
        private Janus.Windows.UI.Dock.UIPanel uiPanelContractItemList;
        private Janus.Windows.UI.Dock.UIPanelInnerContainer uiPanel2Container;
        private Janus.Windows.UI.Dock.UIPanel uiPanelContractItemBtn;
        private Janus.Windows.UI.Dock.UIPanelInnerContainer uiPanel3Container;
        private _10_Media._13_AnalysisItemGroup.AnalysisItemGroupDs analysisItemGroupDs;
    }
}
