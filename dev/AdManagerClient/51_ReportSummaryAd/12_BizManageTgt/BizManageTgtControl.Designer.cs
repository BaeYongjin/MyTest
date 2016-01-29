namespace AdManagerClient
{
    partial class BizManageTgtControl
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
            this.uiPM = new Janus.Windows.UI.Dock.UIPanelManager(this.components);
            this.uiPanelMain = new Janus.Windows.UI.Dock.UIPanelGroup();
            this.uiPanelSearch = new Janus.Windows.UI.Dock.UIPanel();
            this.uiPanelUsersSearchContainer = new Janus.Windows.UI.Dock.UIPanelInnerContainer();
            this.pnlSearch = new System.Windows.Forms.Panel();
            this.cbSearchEndDay = new Janus.Windows.CalendarCombo.CalendarCombo();
            this.cbSearchBgnDay = new Janus.Windows.CalendarCombo.CalendarCombo();
            this.lbSearchDate = new System.Windows.Forms.Label();
            this.cbSearchRap = new Janus.Windows.EditControls.UIComboBox();
            this.cbSearchAgency = new Janus.Windows.EditControls.UIComboBox();
            this.cbSearchAdvertiser = new Janus.Windows.EditControls.UIComboBox();
            this.cbSearchAdType = new Janus.Windows.EditControls.UIComboBox();
            this.btnSearch = new Janus.Windows.EditControls.UIButton();
            this.uiPanelList = new Janus.Windows.UI.Dock.UIPanel();
            this.uiPanelUserListContainer = new Janus.Windows.UI.Dock.UIPanelInnerContainer();
            this.pgrdBizManageTgt = new DevExpress.XtraPivotGrid.PivotGridControl();
            this.fieldAdTypeName = new DevExpress.XtraPivotGrid.PivotGridField();
            this.fieldAdvertiserName = new DevExpress.XtraPivotGrid.PivotGridField();
            this.fieldBrandName = new DevExpress.XtraPivotGrid.PivotGridField();
            this.fieldJobClassName = new DevExpress.XtraPivotGrid.PivotGridField();
            this.fieldRapName = new DevExpress.XtraPivotGrid.PivotGridField();
            this.fieldAgencyName = new DevExpress.XtraPivotGrid.PivotGridField();
            this.fieldStartDay = new DevExpress.XtraPivotGrid.PivotGridField();
            this.fieldEndDay = new DevExpress.XtraPivotGrid.PivotGridField();
            this.fieldEndMonth = new DevExpress.XtraPivotGrid.PivotGridField();
            this.fieldPrice = new DevExpress.XtraPivotGrid.PivotGridField();
            this.fieldAdTime = new DevExpress.XtraPivotGrid.PivotGridField();
            this.fieldOfferExpCnt = new DevExpress.XtraPivotGrid.PivotGridField();
            this.fieldRealExpCnt = new DevExpress.XtraPivotGrid.PivotGridField();
            this.fieldOfferCPM = new DevExpress.XtraPivotGrid.PivotGridField();
            this.fieldRealCPM = new DevExpress.XtraPivotGrid.PivotGridField();
            this.fieldTgtCategoryName = new DevExpress.XtraPivotGrid.PivotGridField();
            this.dvBizManageTgtList = new System.Data.DataView();
            this.bizManageTgtDs = new AdManagerClient.BizManageTgtDs();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.uiPM)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelMain)).BeginInit();
            this.uiPanelMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelSearch)).BeginInit();
            this.uiPanelSearch.SuspendLayout();
            this.uiPanelUsersSearchContainer.SuspendLayout();
            this.pnlSearch.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelList)).BeginInit();
            this.uiPanelList.SuspendLayout();
            this.uiPanelUserListContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pgrdBizManageTgt)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dvBizManageTgtList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bizManageTgtDs)).BeginInit();
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
            this.uiPanelMain.Id = new System.Guid("b162c53a-7940-47cd-a0ed-9762c59c50d8");
            this.uiPanelMain.StaticGroup = true;
            this.uiPanelSearch.Id = new System.Guid("afb3b7d0-aa79-443c-a1e8-3baba38f7a4d");
            this.uiPanelMain.Panels.Add(this.uiPanelSearch);
            this.uiPanelList.Id = new System.Guid("8e3140b9-bfcb-43af-b84e-2ee6cddf51dc");
            this.uiPanelMain.Panels.Add(this.uiPanelList);
            this.uiPM.Panels.Add(this.uiPanelMain);
            // 
            // Design Time Panel Info:
            // 
            this.uiPM.BeginPanelInfo();
            this.uiPM.AddDockPanelInfo(new System.Guid("b162c53a-7940-47cd-a0ed-9762c59c50d8"), Janus.Windows.UI.Dock.PanelGroupStyle.HorizontalTiles, Janus.Windows.UI.Dock.PanelDockStyle.Fill, true, new System.Drawing.Size(1010, 677), true);
            this.uiPM.AddDockPanelInfo(new System.Guid("afb3b7d0-aa79-443c-a1e8-3baba38f7a4d"), new System.Guid("b162c53a-7940-47cd-a0ed-9762c59c50d8"), 50, true);
            this.uiPM.AddDockPanelInfo(new System.Guid("8e3140b9-bfcb-43af-b84e-2ee6cddf51dc"), new System.Guid("b162c53a-7940-47cd-a0ed-9762c59c50d8"), 601, true);
            this.uiPM.AddFloatingPanelInfo(new System.Guid("b162c53a-7940-47cd-a0ed-9762c59c50d8"), Janus.Windows.UI.Dock.PanelGroupStyle.HorizontalTiles, true, new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
            this.uiPM.AddFloatingPanelInfo(new System.Guid("8e3140b9-bfcb-43af-b84e-2ee6cddf51dc"), new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
            this.uiPM.AddFloatingPanelInfo(new System.Guid("b2973a57-d3e1-4d80-9387-7a6dc3332542"), new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
            this.uiPM.AddFloatingPanelInfo(new System.Guid("afb3b7d0-aa79-443c-a1e8-3baba38f7a4d"), new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
            this.uiPM.EndPanelInfo();
            // 
            // uiPanelMain
            // 
            this.uiPanelMain.ActiveCaptionMode = Janus.Windows.UI.Dock.ActiveCaptionMode.Never;
            this.uiPanelMain.CaptionVisible = Janus.Windows.UI.InheritableBoolean.True;
            this.uiPanelMain.CloseButtonVisible = Janus.Windows.UI.InheritableBoolean.False;
            this.uiPanelMain.Location = new System.Drawing.Point(0, 0);
            this.uiPanelMain.Name = "uiPanelMain";
            this.uiPanelMain.Size = new System.Drawing.Size(1010, 677);
            this.uiPanelMain.TabIndex = 4;
            this.uiPanelMain.Text = "광고 판매 영업관리";
            // 
            // uiPanelSearch
            // 
            this.uiPanelSearch.AllowResize = Janus.Windows.UI.InheritableBoolean.False;
            this.uiPanelSearch.CaptionVisible = Janus.Windows.UI.InheritableBoolean.False;
            this.uiPanelSearch.CloseButtonVisible = Janus.Windows.UI.InheritableBoolean.False;
            this.uiPanelSearch.InnerContainer = this.uiPanelUsersSearchContainer;
            this.uiPanelSearch.Location = new System.Drawing.Point(0, 22);
            this.uiPanelSearch.Name = "uiPanelSearch";
            this.uiPanelSearch.Size = new System.Drawing.Size(1010, 50);
            this.uiPanelSearch.TabIndex = 0;
            this.uiPanelSearch.Text = "검색";
            // 
            // uiPanelUsersSearchContainer
            // 
            this.uiPanelUsersSearchContainer.Controls.Add(this.pnlSearch);
            this.uiPanelUsersSearchContainer.Location = new System.Drawing.Point(1, 1);
            this.uiPanelUsersSearchContainer.Name = "uiPanelUsersSearchContainer";
            this.uiPanelUsersSearchContainer.Size = new System.Drawing.Size(1008, 48);
            this.uiPanelUsersSearchContainer.TabIndex = 0;
            // 
            // pnlSearch
            // 
            this.pnlSearch.BackColor = System.Drawing.SystemColors.Window;
            this.pnlSearch.Controls.Add(this.label1);
            this.pnlSearch.Controls.Add(this.cbSearchEndDay);
            this.pnlSearch.Controls.Add(this.cbSearchBgnDay);
            this.pnlSearch.Controls.Add(this.lbSearchDate);
            this.pnlSearch.Controls.Add(this.cbSearchRap);
            this.pnlSearch.Controls.Add(this.cbSearchAgency);
            this.pnlSearch.Controls.Add(this.cbSearchAdvertiser);
            this.pnlSearch.Controls.Add(this.cbSearchAdType);
            this.pnlSearch.Controls.Add(this.btnSearch);
            this.pnlSearch.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlSearch.Location = new System.Drawing.Point(0, 0);
            this.pnlSearch.Name = "pnlSearch";
            this.pnlSearch.Size = new System.Drawing.Size(1008, 48);
            this.pnlSearch.TabIndex = 0;
            // 
            // cbSearchEndDay
            // 
            // 
            // 
            // 
            this.cbSearchEndDay.DropDownCalendar.FirstMonth = new System.DateTime(2007, 8, 1, 0, 0, 0, 0);
            this.cbSearchEndDay.DropDownCalendar.Name = "";
            this.cbSearchEndDay.DropDownCalendar.VisualStyle = Janus.Windows.CalendarCombo.VisualStyle.Office2007;
            this.cbSearchEndDay.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.cbSearchEndDay.Location = new System.Drawing.Point(739, 11);
            this.cbSearchEndDay.Name = "cbSearchEndDay";
            this.cbSearchEndDay.Size = new System.Drawing.Size(120, 23);
            this.cbSearchEndDay.TabIndex = 18;
            this.cbSearchEndDay.Value = new System.DateTime(2007, 9, 14, 0, 0, 0, 0);
            this.cbSearchEndDay.VisualStyle = Janus.Windows.CalendarCombo.VisualStyle.Office2007;
            this.cbSearchEndDay.ValueChanged += new System.EventHandler(this.cbSearchEndDay_ValueChanged);
            // 
            // cbSearchBgnDay
            // 
            // 
            // 
            // 
            this.cbSearchBgnDay.DropDownCalendar.FirstMonth = new System.DateTime(2007, 8, 1, 0, 0, 0, 0);
            this.cbSearchBgnDay.DropDownCalendar.Name = "";
            this.cbSearchBgnDay.DropDownCalendar.VisualStyle = Janus.Windows.CalendarCombo.VisualStyle.Office2007;
            this.cbSearchBgnDay.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.cbSearchBgnDay.Location = new System.Drawing.Point(596, 12);
            this.cbSearchBgnDay.Name = "cbSearchBgnDay";
            this.cbSearchBgnDay.SecondIncrement = 1;
            this.cbSearchBgnDay.Size = new System.Drawing.Size(120, 23);
            this.cbSearchBgnDay.TabIndex = 16;
            this.cbSearchBgnDay.Value = new System.DateTime(2007, 9, 14, 0, 0, 0, 0);
            this.cbSearchBgnDay.VisualStyle = Janus.Windows.CalendarCombo.VisualStyle.Office2007;
            this.cbSearchBgnDay.ValueChanged += new System.EventHandler(this.cbSearchBgnDay_ValueChanged);
            // 
            // lbSearchDate
            // 
            this.lbSearchDate.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lbSearchDate.Location = new System.Drawing.Point(524, 13);
            this.lbSearchDate.Name = "lbSearchDate";
            this.lbSearchDate.Size = new System.Drawing.Size(72, 21);
            this.lbSearchDate.TabIndex = 17;
            this.lbSearchDate.Text = "집계기준일";
            this.lbSearchDate.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cbSearchRap
            // 
            this.cbSearchRap.ComboStyle = Janus.Windows.EditControls.ComboStyle.DropDownList;
            this.cbSearchRap.DisplayMember = "RapName";
            this.cbSearchRap.Location = new System.Drawing.Point(16, 15);
            this.cbSearchRap.Name = "cbSearchRap";
            this.cbSearchRap.Size = new System.Drawing.Size(120, 19);
            this.cbSearchRap.TabIndex = 8;
            this.cbSearchRap.ValueMember = "RapCode";
            this.cbSearchRap.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            // 
            // cbSearchAgency
            // 
            this.cbSearchAgency.ComboStyle = Janus.Windows.EditControls.ComboStyle.DropDownList;
            this.cbSearchAgency.DisplayMember = "AgencyName";
            this.cbSearchAgency.Location = new System.Drawing.Point(144, 15);
            this.cbSearchAgency.Name = "cbSearchAgency";
            this.cbSearchAgency.Size = new System.Drawing.Size(120, 19);
            this.cbSearchAgency.TabIndex = 9;
            this.cbSearchAgency.ValueMember = "AgencyCode";
            this.cbSearchAgency.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            // 
            // cbSearchAdvertiser
            // 
            this.cbSearchAdvertiser.ComboStyle = Janus.Windows.EditControls.ComboStyle.DropDownList;
            this.cbSearchAdvertiser.DisplayMember = "AdvertiserName";
            this.cbSearchAdvertiser.Location = new System.Drawing.Point(272, 15);
            this.cbSearchAdvertiser.Name = "cbSearchAdvertiser";
            this.cbSearchAdvertiser.Size = new System.Drawing.Size(120, 19);
            this.cbSearchAdvertiser.TabIndex = 10;
            this.cbSearchAdvertiser.ValueMember = "AdvertiserCode";
            this.cbSearchAdvertiser.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            // 
            // cbSearchAdType
            // 
            this.cbSearchAdType.BackColor = System.Drawing.Color.White;
            this.cbSearchAdType.ComboStyle = Janus.Windows.EditControls.ComboStyle.DropDownList;
            this.cbSearchAdType.Location = new System.Drawing.Point(398, 15);
            this.cbSearchAdType.Name = "cbSearchAdType";
            this.cbSearchAdType.Size = new System.Drawing.Size(120, 19);
            this.cbSearchAdType.TabIndex = 11;
            this.cbSearchAdType.Text = "광고종류";
            this.cbSearchAdType.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            // 
            // btnSearch
            // 
            this.btnSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSearch.Enabled = false;
            this.btnSearch.Location = new System.Drawing.Point(892, 12);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(104, 24);
            this.btnSearch.TabIndex = 7;
            this.btnSearch.Text = "조 회";
            this.btnSearch.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // uiPanelList
            // 
            this.uiPanelList.CaptionStyle = Janus.Windows.UI.Dock.PanelCaptionStyle.Dark;
            this.uiPanelList.CaptionVisible = Janus.Windows.UI.InheritableBoolean.True;
            this.uiPanelList.InnerContainer = this.uiPanelUserListContainer;
            this.uiPanelList.Location = new System.Drawing.Point(0, 76);
            this.uiPanelList.MinimumSize = new System.Drawing.Size(-1, 100);
            this.uiPanelList.Name = "uiPanelList";
            this.uiPanelList.Size = new System.Drawing.Size(1010, 601);
            this.uiPanelList.TabIndex = 1;
            this.uiPanelList.Text = "영업관리";
            // 
            // uiPanelUserListContainer
            // 
            this.uiPanelUserListContainer.Controls.Add(this.pgrdBizManageTgt);
            this.uiPanelUserListContainer.Location = new System.Drawing.Point(1, 23);
            this.uiPanelUserListContainer.Name = "uiPanelUserListContainer";
            this.uiPanelUserListContainer.Size = new System.Drawing.Size(1008, 577);
            this.uiPanelUserListContainer.TabIndex = 0;
            // 
            // pgrdBizManageTgt
            // 
            this.pgrdBizManageTgt.Appearance.Cell.Font = new System.Drawing.Font("나눔고딕코딩", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.pgrdBizManageTgt.Appearance.Cell.Options.UseFont = true;
            this.pgrdBizManageTgt.Appearance.ColumnHeaderArea.Font = new System.Drawing.Font("나눔고딕", 8.249999F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.pgrdBizManageTgt.Appearance.ColumnHeaderArea.Options.UseFont = true;
            this.pgrdBizManageTgt.Appearance.ColumnHeaderArea.Options.UseTextOptions = true;
            this.pgrdBizManageTgt.Appearance.ColumnHeaderArea.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.pgrdBizManageTgt.Appearance.DataHeaderArea.Font = new System.Drawing.Font("나눔고딕", 8.249999F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.pgrdBizManageTgt.Appearance.DataHeaderArea.Options.UseFont = true;
            this.pgrdBizManageTgt.Appearance.FieldHeader.Font = new System.Drawing.Font("나눔고딕", 8.249999F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.pgrdBizManageTgt.Appearance.FieldHeader.Options.UseFont = true;
            this.pgrdBizManageTgt.Appearance.FieldValue.Font = new System.Drawing.Font("나눔고딕", 8.249999F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.pgrdBizManageTgt.Appearance.FieldValue.Options.UseFont = true;
            this.pgrdBizManageTgt.Appearance.FilterHeaderArea.Font = new System.Drawing.Font("나눔고딕", 8.249999F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.pgrdBizManageTgt.Appearance.FilterHeaderArea.Options.UseFont = true;
            this.pgrdBizManageTgt.Appearance.HeaderArea.Font = new System.Drawing.Font("나눔고딕", 8.249999F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.pgrdBizManageTgt.Appearance.HeaderArea.Options.UseFont = true;
            this.pgrdBizManageTgt.Appearance.HeaderArea.Options.UseTextOptions = true;
            this.pgrdBizManageTgt.Appearance.HeaderArea.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.pgrdBizManageTgt.Appearance.Lines.Font = new System.Drawing.Font("나눔고딕", 8.249999F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.pgrdBizManageTgt.Appearance.Lines.Options.UseFont = true;
            this.pgrdBizManageTgt.Appearance.RowHeaderArea.Font = new System.Drawing.Font("나눔고딕", 8.249999F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.pgrdBizManageTgt.Appearance.RowHeaderArea.Options.UseFont = true;
            this.pgrdBizManageTgt.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pgrdBizManageTgt.Fields.AddRange(new DevExpress.XtraPivotGrid.PivotGridField[] {
            this.fieldAdTypeName,
            this.fieldAdvertiserName,
            this.fieldBrandName,
            this.fieldJobClassName,
            this.fieldRapName,
            this.fieldAgencyName,
            this.fieldStartDay,
            this.fieldEndDay,
            this.fieldEndMonth,
            this.fieldPrice,
            this.fieldAdTime,
            this.fieldOfferExpCnt,
            this.fieldRealExpCnt,
            this.fieldOfferCPM,
            this.fieldRealCPM,
            this.fieldTgtCategoryName});
            this.pgrdBizManageTgt.Location = new System.Drawing.Point(0, 0);
            this.pgrdBizManageTgt.LookAndFeel.SkinName = "Office 2010 Blue";
            this.pgrdBizManageTgt.LookAndFeel.UseDefaultLookAndFeel = false;
            this.pgrdBizManageTgt.Name = "pgrdBizManageTgt";
            this.pgrdBizManageTgt.OptionsView.ShowColumnGrandTotalHeader = false;
            this.pgrdBizManageTgt.OptionsView.ShowColumnGrandTotals = false;
            this.pgrdBizManageTgt.OptionsView.ShowFilterHeaders = false;
            this.pgrdBizManageTgt.Size = new System.Drawing.Size(1008, 577);
            this.pgrdBizManageTgt.TabIndex = 0;
            this.pgrdBizManageTgt.CustomCellDisplayText += new DevExpress.XtraPivotGrid.PivotCellDisplayTextEventHandler(this.pgrdBizManageTgt_CustomCellDisplayText);
            // 
            // fieldAdTypeName
            // 
            this.fieldAdTypeName.AllowedAreas = ((DevExpress.XtraPivotGrid.PivotGridAllowedAreas)(((DevExpress.XtraPivotGrid.PivotGridAllowedAreas.RowArea | DevExpress.XtraPivotGrid.PivotGridAllowedAreas.ColumnArea) 
            | DevExpress.XtraPivotGrid.PivotGridAllowedAreas.FilterArea)));
            this.fieldAdTypeName.Area = DevExpress.XtraPivotGrid.PivotArea.RowArea;
            this.fieldAdTypeName.AreaIndex = 0;
            this.fieldAdTypeName.Caption = "구분1";
            this.fieldAdTypeName.FieldName = "AdTypeName";
            this.fieldAdTypeName.Name = "fieldAdTypeName";
            this.fieldAdTypeName.UnboundType = DevExpress.Data.UnboundColumnType.String;
            this.fieldAdTypeName.Width = 72;
            // 
            // fieldAdvertiserName
            // 
            this.fieldAdvertiserName.AllowedAreas = ((DevExpress.XtraPivotGrid.PivotGridAllowedAreas)(((DevExpress.XtraPivotGrid.PivotGridAllowedAreas.RowArea | DevExpress.XtraPivotGrid.PivotGridAllowedAreas.ColumnArea) 
            | DevExpress.XtraPivotGrid.PivotGridAllowedAreas.FilterArea)));
            this.fieldAdvertiserName.Area = DevExpress.XtraPivotGrid.PivotArea.RowArea;
            this.fieldAdvertiserName.AreaIndex = 2;
            this.fieldAdvertiserName.Caption = "광고주 법인명";
            this.fieldAdvertiserName.FieldName = "AdvertiserName";
            this.fieldAdvertiserName.Name = "fieldAdvertiserName";
            this.fieldAdvertiserName.TotalsVisibility = DevExpress.XtraPivotGrid.PivotTotalsVisibility.None;
            this.fieldAdvertiserName.UnboundType = DevExpress.Data.UnboundColumnType.String;
            this.fieldAdvertiserName.Width = 120;
            // 
            // fieldBrandName
            // 
            this.fieldBrandName.AllowedAreas = ((DevExpress.XtraPivotGrid.PivotGridAllowedAreas)(((DevExpress.XtraPivotGrid.PivotGridAllowedAreas.RowArea | DevExpress.XtraPivotGrid.PivotGridAllowedAreas.ColumnArea) 
            | DevExpress.XtraPivotGrid.PivotGridAllowedAreas.FilterArea)));
            this.fieldBrandName.Area = DevExpress.XtraPivotGrid.PivotArea.RowArea;
            this.fieldBrandName.AreaIndex = 3;
            this.fieldBrandName.Caption = "브랜드명";
            this.fieldBrandName.FieldName = "BrandName";
            this.fieldBrandName.Name = "fieldBrandName";
            this.fieldBrandName.TotalsVisibility = DevExpress.XtraPivotGrid.PivotTotalsVisibility.None;
            this.fieldBrandName.UnboundType = DevExpress.Data.UnboundColumnType.String;
            this.fieldBrandName.Width = 130;
            // 
            // fieldJobClassName
            // 
            this.fieldJobClassName.AllowedAreas = ((DevExpress.XtraPivotGrid.PivotGridAllowedAreas)(((DevExpress.XtraPivotGrid.PivotGridAllowedAreas.RowArea | DevExpress.XtraPivotGrid.PivotGridAllowedAreas.ColumnArea) 
            | DevExpress.XtraPivotGrid.PivotGridAllowedAreas.FilterArea)));
            this.fieldJobClassName.Area = DevExpress.XtraPivotGrid.PivotArea.RowArea;
            this.fieldJobClassName.AreaIndex = 4;
            this.fieldJobClassName.Caption = "업종분류 기타";
            this.fieldJobClassName.FieldName = "JobClassName";
            this.fieldJobClassName.Name = "fieldJobClassName";
            this.fieldJobClassName.TotalsVisibility = DevExpress.XtraPivotGrid.PivotTotalsVisibility.None;
            this.fieldJobClassName.UnboundType = DevExpress.Data.UnboundColumnType.String;
            // 
            // fieldRapName
            // 
            this.fieldRapName.AllowedAreas = ((DevExpress.XtraPivotGrid.PivotGridAllowedAreas)(((DevExpress.XtraPivotGrid.PivotGridAllowedAreas.RowArea | DevExpress.XtraPivotGrid.PivotGridAllowedAreas.ColumnArea) 
            | DevExpress.XtraPivotGrid.PivotGridAllowedAreas.FilterArea)));
            this.fieldRapName.Area = DevExpress.XtraPivotGrid.PivotArea.RowArea;
            this.fieldRapName.AreaIndex = 1;
            this.fieldRapName.Caption = "미디어렙";
            this.fieldRapName.FieldName = "RapName";
            this.fieldRapName.Name = "fieldRapName";
            this.fieldRapName.UnboundType = DevExpress.Data.UnboundColumnType.String;
            // 
            // fieldAgencyName
            // 
            this.fieldAgencyName.AllowedAreas = ((DevExpress.XtraPivotGrid.PivotGridAllowedAreas)(((DevExpress.XtraPivotGrid.PivotGridAllowedAreas.RowArea | DevExpress.XtraPivotGrid.PivotGridAllowedAreas.ColumnArea) 
            | DevExpress.XtraPivotGrid.PivotGridAllowedAreas.FilterArea)));
            this.fieldAgencyName.Area = DevExpress.XtraPivotGrid.PivotArea.RowArea;
            this.fieldAgencyName.AreaIndex = 5;
            this.fieldAgencyName.Caption = "광고대행사";
            this.fieldAgencyName.FieldName = "AgencyName";
            this.fieldAgencyName.Name = "fieldAgencyName";
            this.fieldAgencyName.TotalsVisibility = DevExpress.XtraPivotGrid.PivotTotalsVisibility.None;
            this.fieldAgencyName.UnboundType = DevExpress.Data.UnboundColumnType.String;
            // 
            // fieldStartDay
            // 
            this.fieldStartDay.AllowedAreas = ((DevExpress.XtraPivotGrid.PivotGridAllowedAreas)((DevExpress.XtraPivotGrid.PivotGridAllowedAreas.FilterArea | DevExpress.XtraPivotGrid.PivotGridAllowedAreas.DataArea)));
            this.fieldStartDay.Appearance.Cell.Options.UseTextOptions = true;
            this.fieldStartDay.Appearance.Cell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.fieldStartDay.Appearance.Header.Options.UseTextOptions = true;
            this.fieldStartDay.Appearance.Header.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.fieldStartDay.Appearance.Value.Options.UseTextOptions = true;
            this.fieldStartDay.Appearance.Value.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.fieldStartDay.Area = DevExpress.XtraPivotGrid.PivotArea.DataArea;
            this.fieldStartDay.AreaIndex = 0;
            this.fieldStartDay.Caption = "집행시작일";
            this.fieldStartDay.FieldName = "StartDay";
            this.fieldStartDay.Name = "fieldStartDay";
            this.fieldStartDay.Options.AllowFilter = DevExpress.Utils.DefaultBoolean.False;
            this.fieldStartDay.Options.ShowGrandTotal = false;
            this.fieldStartDay.SummaryType = DevExpress.Data.PivotGrid.PivotSummaryType.Min;
            this.fieldStartDay.TotalsVisibility = DevExpress.XtraPivotGrid.PivotTotalsVisibility.None;
            this.fieldStartDay.UnboundType = DevExpress.Data.UnboundColumnType.String;
            this.fieldStartDay.Width = 70;
            // 
            // fieldEndDay
            // 
            this.fieldEndDay.AllowedAreas = ((DevExpress.XtraPivotGrid.PivotGridAllowedAreas)((DevExpress.XtraPivotGrid.PivotGridAllowedAreas.FilterArea | DevExpress.XtraPivotGrid.PivotGridAllowedAreas.DataArea)));
            this.fieldEndDay.Appearance.Cell.Options.UseTextOptions = true;
            this.fieldEndDay.Appearance.Cell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.fieldEndDay.Appearance.Header.Options.UseTextOptions = true;
            this.fieldEndDay.Appearance.Header.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.fieldEndDay.Appearance.Value.Options.UseTextOptions = true;
            this.fieldEndDay.Appearance.Value.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.fieldEndDay.Area = DevExpress.XtraPivotGrid.PivotArea.DataArea;
            this.fieldEndDay.AreaIndex = 1;
            this.fieldEndDay.Caption = "집행종료일";
            this.fieldEndDay.FieldName = "EndDay";
            this.fieldEndDay.Name = "fieldEndDay";
            this.fieldEndDay.Options.AllowFilter = DevExpress.Utils.DefaultBoolean.False;
            this.fieldEndDay.Options.ShowGrandTotal = false;
            this.fieldEndDay.SummaryType = DevExpress.Data.PivotGrid.PivotSummaryType.Max;
            this.fieldEndDay.TotalsVisibility = DevExpress.XtraPivotGrid.PivotTotalsVisibility.None;
            this.fieldEndDay.UnboundType = DevExpress.Data.UnboundColumnType.String;
            this.fieldEndDay.Width = 70;
            // 
            // fieldEndMonth
            // 
            this.fieldEndMonth.AllowedAreas = ((DevExpress.XtraPivotGrid.PivotGridAllowedAreas)(((DevExpress.XtraPivotGrid.PivotGridAllowedAreas.RowArea | DevExpress.XtraPivotGrid.PivotGridAllowedAreas.ColumnArea) 
            | DevExpress.XtraPivotGrid.PivotGridAllowedAreas.FilterArea)));
            this.fieldEndMonth.Appearance.Header.Options.UseTextOptions = true;
            this.fieldEndMonth.Appearance.Header.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.fieldEndMonth.Appearance.Value.Options.UseTextOptions = true;
            this.fieldEndMonth.Appearance.Value.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.fieldEndMonth.Area = DevExpress.XtraPivotGrid.PivotArea.ColumnArea;
            this.fieldEndMonth.AreaIndex = 0;
            this.fieldEndMonth.Caption = "월";
            this.fieldEndMonth.FieldName = "EndMonth";
            this.fieldEndMonth.Name = "fieldEndMonth";
            this.fieldEndMonth.Options.ShowGrandTotal = false;
            this.fieldEndMonth.UnboundType = DevExpress.Data.UnboundColumnType.String;
            // 
            // fieldPrice
            // 
            this.fieldPrice.AllowedAreas = ((DevExpress.XtraPivotGrid.PivotGridAllowedAreas)((DevExpress.XtraPivotGrid.PivotGridAllowedAreas.FilterArea | DevExpress.XtraPivotGrid.PivotGridAllowedAreas.DataArea)));
            this.fieldPrice.Appearance.Value.Options.UseTextOptions = true;
            this.fieldPrice.Appearance.Value.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.fieldPrice.Area = DevExpress.XtraPivotGrid.PivotArea.DataArea;
            this.fieldPrice.AreaIndex = 2;
            this.fieldPrice.Caption = "매출금액";
            this.fieldPrice.CellFormat.FormatString = "c0";
            this.fieldPrice.CellFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.fieldPrice.FieldName = "Price";
            this.fieldPrice.Name = "fieldPrice";
            this.fieldPrice.TotalsVisibility = DevExpress.XtraPivotGrid.PivotTotalsVisibility.None;
            this.fieldPrice.UnboundType = DevExpress.Data.UnboundColumnType.Integer;
            this.fieldPrice.Width = 90;
            // 
            // fieldAdTime
            // 
            this.fieldAdTime.AllowedAreas = ((DevExpress.XtraPivotGrid.PivotGridAllowedAreas)((DevExpress.XtraPivotGrid.PivotGridAllowedAreas.FilterArea | DevExpress.XtraPivotGrid.PivotGridAllowedAreas.DataArea)));
            this.fieldAdTime.Appearance.Cell.Options.UseTextOptions = true;
            this.fieldAdTime.Appearance.Cell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.fieldAdTime.Appearance.Header.Options.UseTextOptions = true;
            this.fieldAdTime.Appearance.Header.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.fieldAdTime.Appearance.Value.Options.UseTextOptions = true;
            this.fieldAdTime.Appearance.Value.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.fieldAdTime.Area = DevExpress.XtraPivotGrid.PivotArea.DataArea;
            this.fieldAdTime.AreaIndex = 3;
            this.fieldAdTime.Caption = "초수";
            this.fieldAdTime.FieldName = "AdTime";
            this.fieldAdTime.Name = "fieldAdTime";
            this.fieldAdTime.Options.AllowFilter = DevExpress.Utils.DefaultBoolean.False;
            this.fieldAdTime.Options.ShowGrandTotal = false;
            this.fieldAdTime.SummaryType = DevExpress.Data.PivotGrid.PivotSummaryType.Max;
            this.fieldAdTime.TotalsVisibility = DevExpress.XtraPivotGrid.PivotTotalsVisibility.None;
            this.fieldAdTime.UnboundType = DevExpress.Data.UnboundColumnType.String;
            this.fieldAdTime.Width = 33;
            // 
            // fieldOfferExpCnt
            // 
            this.fieldOfferExpCnt.AllowedAreas = ((DevExpress.XtraPivotGrid.PivotGridAllowedAreas)((DevExpress.XtraPivotGrid.PivotGridAllowedAreas.FilterArea | DevExpress.XtraPivotGrid.PivotGridAllowedAreas.DataArea)));
            this.fieldOfferExpCnt.Appearance.Value.Options.UseTextOptions = true;
            this.fieldOfferExpCnt.Appearance.Value.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.fieldOfferExpCnt.Area = DevExpress.XtraPivotGrid.PivotArea.DataArea;
            this.fieldOfferExpCnt.AreaIndex = 5;
            this.fieldOfferExpCnt.Caption = "보장노출수";
            this.fieldOfferExpCnt.CellFormat.FormatString = "n0";
            this.fieldOfferExpCnt.CellFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.fieldOfferExpCnt.FieldName = "OfferExpCnt";
            this.fieldOfferExpCnt.Name = "fieldOfferExpCnt";
            this.fieldOfferExpCnt.TotalsVisibility = DevExpress.XtraPivotGrid.PivotTotalsVisibility.None;
            this.fieldOfferExpCnt.UnboundType = DevExpress.Data.UnboundColumnType.Integer;
            this.fieldOfferExpCnt.Width = 81;
            // 
            // fieldRealExpCnt
            // 
            this.fieldRealExpCnt.AllowedAreas = ((DevExpress.XtraPivotGrid.PivotGridAllowedAreas)((DevExpress.XtraPivotGrid.PivotGridAllowedAreas.FilterArea | DevExpress.XtraPivotGrid.PivotGridAllowedAreas.DataArea)));
            this.fieldRealExpCnt.Appearance.Value.Options.UseTextOptions = true;
            this.fieldRealExpCnt.Appearance.Value.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.fieldRealExpCnt.Area = DevExpress.XtraPivotGrid.PivotArea.DataArea;
            this.fieldRealExpCnt.AreaIndex = 4;
            this.fieldRealExpCnt.Caption = "실집행노출수";
            this.fieldRealExpCnt.CellFormat.FormatString = "n0";
            this.fieldRealExpCnt.CellFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.fieldRealExpCnt.FieldName = "RealExpCnt";
            this.fieldRealExpCnt.Name = "fieldRealExpCnt";
            this.fieldRealExpCnt.TotalsVisibility = DevExpress.XtraPivotGrid.PivotTotalsVisibility.None;
            this.fieldRealExpCnt.UnboundType = DevExpress.Data.UnboundColumnType.Integer;
            this.fieldRealExpCnt.Width = 81;
            // 
            // fieldOfferCPM
            // 
            this.fieldOfferCPM.AllowedAreas = ((DevExpress.XtraPivotGrid.PivotGridAllowedAreas)((DevExpress.XtraPivotGrid.PivotGridAllowedAreas.FilterArea | DevExpress.XtraPivotGrid.PivotGridAllowedAreas.DataArea)));
            this.fieldOfferCPM.Appearance.Value.Options.UseTextOptions = true;
            this.fieldOfferCPM.Appearance.Value.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.fieldOfferCPM.Area = DevExpress.XtraPivotGrid.PivotArea.DataArea;
            this.fieldOfferCPM.AreaIndex = 6;
            this.fieldOfferCPM.Caption = "제안CPM";
            this.fieldOfferCPM.CellFormat.FormatString = "c0";
            this.fieldOfferCPM.CellFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.fieldOfferCPM.FieldName = "OfferCPM";
            this.fieldOfferCPM.Name = "fieldOfferCPM";
            this.fieldOfferCPM.TotalsVisibility = DevExpress.XtraPivotGrid.PivotTotalsVisibility.None;
            this.fieldOfferCPM.UnboundType = DevExpress.Data.UnboundColumnType.Integer;
            this.fieldOfferCPM.Width = 90;
            // 
            // fieldRealCPM
            // 
            this.fieldRealCPM.AllowedAreas = ((DevExpress.XtraPivotGrid.PivotGridAllowedAreas)((DevExpress.XtraPivotGrid.PivotGridAllowedAreas.FilterArea | DevExpress.XtraPivotGrid.PivotGridAllowedAreas.DataArea)));
            this.fieldRealCPM.Appearance.Value.Options.UseTextOptions = true;
            this.fieldRealCPM.Appearance.Value.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.fieldRealCPM.Area = DevExpress.XtraPivotGrid.PivotArea.DataArea;
            this.fieldRealCPM.AreaIndex = 7;
            this.fieldRealCPM.Caption = "결과CPM";
            this.fieldRealCPM.CellFormat.FormatString = "c0";
            this.fieldRealCPM.CellFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.fieldRealCPM.FieldName = "RealCPM";
            this.fieldRealCPM.Name = "fieldRealCPM";
            this.fieldRealCPM.SortOrder = DevExpress.XtraPivotGrid.PivotSortOrder.Descending;
            this.fieldRealCPM.TotalsVisibility = DevExpress.XtraPivotGrid.PivotTotalsVisibility.None;
            this.fieldRealCPM.UnboundType = DevExpress.Data.UnboundColumnType.Integer;
            this.fieldRealCPM.Width = 90;
            // 
            // fieldTgtCategoryName
            // 
            this.fieldTgtCategoryName.AllowedAreas = ((DevExpress.XtraPivotGrid.PivotGridAllowedAreas)((DevExpress.XtraPivotGrid.PivotGridAllowedAreas.FilterArea | DevExpress.XtraPivotGrid.PivotGridAllowedAreas.DataArea)));
            this.fieldTgtCategoryName.Appearance.Cell.Options.UseTextOptions = true;
            this.fieldTgtCategoryName.Appearance.Cell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.fieldTgtCategoryName.Appearance.Value.Options.UseTextOptions = true;
            this.fieldTgtCategoryName.Appearance.Value.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.fieldTgtCategoryName.Area = DevExpress.XtraPivotGrid.PivotArea.DataArea;
            this.fieldTgtCategoryName.AreaIndex = 8;
            this.fieldTgtCategoryName.Caption = "타겟팅 현황";
            this.fieldTgtCategoryName.FieldName = "TgtCategoryName";
            this.fieldTgtCategoryName.Name = "fieldTgtCategoryName";
            this.fieldTgtCategoryName.Options.AllowFilter = DevExpress.Utils.DefaultBoolean.False;
            this.fieldTgtCategoryName.Options.ShowGrandTotal = false;
            this.fieldTgtCategoryName.SummaryType = DevExpress.Data.PivotGrid.PivotSummaryType.Max;
            this.fieldTgtCategoryName.TotalsVisibility = DevExpress.XtraPivotGrid.PivotTotalsVisibility.None;
            this.fieldTgtCategoryName.UnboundType = DevExpress.Data.UnboundColumnType.String;
            this.fieldTgtCategoryName.Width = 90;
            // 
            // bizManageTgtDs
            // 
            this.bizManageTgtDs.DataSetName = "BizManageTgtDs";
            this.bizManageTgtDs.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(722, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(11, 12);
            this.label1.TabIndex = 241;
            this.label1.Text = "~";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // BizManageTgtControl
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.Controls.Add(this.uiPanelMain);
            this.Font = new System.Drawing.Font("나눔고딕코딩", 8.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.Name = "BizManageTgtControl";
            this.Size = new System.Drawing.Size(1010, 677);
            this.Load += new System.EventHandler(this.UserControl_Load);
            ((System.ComponentModel.ISupportInitialize)(this.uiPM)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelMain)).EndInit();
            this.uiPanelMain.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelSearch)).EndInit();
            this.uiPanelSearch.ResumeLayout(false);
            this.uiPanelUsersSearchContainer.ResumeLayout(false);
            this.pnlSearch.ResumeLayout(false);
            this.pnlSearch.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelList)).EndInit();
            this.uiPanelList.ResumeLayout(false);
            this.uiPanelUserListContainer.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pgrdBizManageTgt)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dvBizManageTgtList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bizManageTgtDs)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Janus.Windows.UI.Dock.UIPanelManager uiPM;
        private Janus.Windows.UI.Dock.UIPanelGroup uiPanelMain;
        private Janus.Windows.UI.Dock.UIPanel uiPanelSearch;
        private Janus.Windows.UI.Dock.UIPanelInnerContainer uiPanelUsersSearchContainer;
        private Janus.Windows.UI.Dock.UIPanel uiPanelList;
        private Janus.Windows.UI.Dock.UIPanelInnerContainer uiPanelUserListContainer;
        private AdManagerClient.BizManageTgtDs bizManageTgtDs;
        private System.Data.DataView dvBizManageTgtList;
        private DevExpress.XtraPivotGrid.PivotGridControl pgrdBizManageTgt;
        private DevExpress.XtraPivotGrid.PivotGridField fieldAdTypeName;
        private DevExpress.XtraPivotGrid.PivotGridField fieldAdvertiserName;
        private DevExpress.XtraPivotGrid.PivotGridField fieldBrandName;
        private DevExpress.XtraPivotGrid.PivotGridField fieldJobClassName;
        private DevExpress.XtraPivotGrid.PivotGridField fieldRapName;
        private DevExpress.XtraPivotGrid.PivotGridField fieldAgencyName;
        private DevExpress.XtraPivotGrid.PivotGridField fieldStartDay;
        private DevExpress.XtraPivotGrid.PivotGridField fieldEndDay;
        private DevExpress.XtraPivotGrid.PivotGridField fieldEndMonth;
        private DevExpress.XtraPivotGrid.PivotGridField fieldPrice;
        private DevExpress.XtraPivotGrid.PivotGridField fieldAdTime;
        private DevExpress.XtraPivotGrid.PivotGridField fieldOfferExpCnt;
        private DevExpress.XtraPivotGrid.PivotGridField fieldRealExpCnt;
        private DevExpress.XtraPivotGrid.PivotGridField fieldOfferCPM;
        private DevExpress.XtraPivotGrid.PivotGridField fieldRealCPM;
        private DevExpress.XtraPivotGrid.PivotGridField fieldTgtCategoryName;
        private System.Windows.Forms.Panel pnlSearch;
        private Janus.Windows.EditControls.UIButton btnSearch;
        private Janus.Windows.EditControls.UIComboBox cbSearchRap;
        private Janus.Windows.EditControls.UIComboBox cbSearchAgency;
        private Janus.Windows.EditControls.UIComboBox cbSearchAdvertiser;
        private Janus.Windows.EditControls.UIComboBox cbSearchAdType;
        private Janus.Windows.CalendarCombo.CalendarCombo cbSearchEndDay;
        private Janus.Windows.CalendarCombo.CalendarCombo cbSearchBgnDay;
        private System.Windows.Forms.Label lbSearchDate;
        private System.Windows.Forms.Label label1;
    }
}
