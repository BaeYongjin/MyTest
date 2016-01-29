namespace AdManagerClient
{
    partial class CombineRptCtrl
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
            DevExpress.XtraGrid.StyleFormatCondition styleFormatCondition1 = new DevExpress.XtraGrid.StyleFormatCondition();
            this.colClass2Code = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            this.uiPM = new Janus.Windows.UI.Dock.UIPanelManager(this.components);
            this.uiPanelSearch = new Janus.Windows.UI.Dock.UIPanel();
            this.uiPanelSearchContainer = new Janus.Windows.UI.Dock.UIPanelInnerContainer();
            this.rptHeader = new AdManagerClient.ReportHeaderControl();
            this.uiPanelList = new Janus.Windows.UI.Dock.UIPanel();
            this.uiPanelListContainer = new Janus.Windows.UI.Dock.UIPanelInnerContainer();
            this.gridControl1 = new DevExpress.XtraGrid.GridControl();
            this.bsList = new System.Windows.Forms.BindingSource(this.components);
            this.combineRptDataSet = new AdManagerClient._51_ReportSummaryAd._11_CombineRpt.CombineRptDataSet();
            this.bandedGridView1 = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridView();
            this.gridBand1 = new DevExpress.XtraGrid.Views.BandedGrid.GridBand();
            this.colClass1Code = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            this.colClass1Name = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            this.colClass2Name = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            this.gridBand2 = new DevExpress.XtraGrid.Views.BandedGrid.GridBand();
            this.colAdsImps = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            this.colAdsRate = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            this.colAdsHouse = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            this.colAdsReach = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            this.colAdsFreq = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            this.gridBand3 = new DevExpress.XtraGrid.Views.BandedGrid.GridBand();
            this.colPnsImps = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            this.colPnsRate = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            this.colPnsHouse = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            this.colPnsReach = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            this.colPnsFreq = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            ((System.ComponentModel.ISupportInitialize)(this.uiPM)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelSearch)).BeginInit();
            this.uiPanelSearch.SuspendLayout();
            this.uiPanelSearchContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelList)).BeginInit();
            this.uiPanelList.SuspendLayout();
            this.uiPanelListContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.combineRptDataSet)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bandedGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // colClass2Code
            // 
            this.colClass2Code.FieldName = "Class2Code";
            this.colClass2Code.Name = "colClass2Code";
            this.colClass2Code.OptionsColumn.AllowEdit = false;
            this.colClass2Code.OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.True;
            // 
            // uiPM
            // 
            this.uiPM.ContainerControl = this;
            this.uiPM.DefaultPanelSettings.CaptionFormatStyle.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.uiPM.SplitterSize = 2;
            this.uiPM.VisualStyle = Janus.Windows.UI.Dock.PanelVisualStyle.Office2007;
            this.uiPanelSearch.Id = new System.Guid("4648dd32-ac5f-4d6c-b120-89582d85c18d");
            this.uiPM.Panels.Add(this.uiPanelSearch);
            this.uiPanelList.Id = new System.Guid("92769e07-7b7c-4741-b2f8-b35128610f42");
            this.uiPM.Panels.Add(this.uiPanelList);
            // 
            // Design Time Panel Info:
            // 
            this.uiPM.BeginPanelInfo();
            this.uiPM.AddDockPanelInfo(new System.Guid("4648dd32-ac5f-4d6c-b120-89582d85c18d"), Janus.Windows.UI.Dock.PanelDockStyle.Top, new System.Drawing.Size(1004, 136), true);
            this.uiPM.AddDockPanelInfo(new System.Guid("92769e07-7b7c-4741-b2f8-b35128610f42"), Janus.Windows.UI.Dock.PanelDockStyle.Fill, new System.Drawing.Size(1004, 535), true);
            this.uiPM.AddFloatingPanelInfo(new System.Guid("4648dd32-ac5f-4d6c-b120-89582d85c18d"), new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
            this.uiPM.AddFloatingPanelInfo(new System.Guid("92769e07-7b7c-4741-b2f8-b35128610f42"), new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
            this.uiPM.EndPanelInfo();
            // 
            // uiPanelSearch
            // 
            this.uiPanelSearch.AutoHideButtonVisible = Janus.Windows.UI.InheritableBoolean.False;
            this.uiPanelSearch.CaptionDoubleClickAction = Janus.Windows.UI.Dock.CaptionDoubleClickAction.None;
            this.uiPanelSearch.CaptionStyle = Janus.Windows.UI.Dock.PanelCaptionStyle.Dark;
            this.uiPanelSearch.CloseButtonVisible = Janus.Windows.UI.InheritableBoolean.False;
            this.uiPanelSearch.InnerContainer = this.uiPanelSearchContainer;
            this.uiPanelSearch.Location = new System.Drawing.Point(3, 3);
            this.uiPanelSearch.Name = "uiPanelSearch";
            this.uiPanelSearch.Size = new System.Drawing.Size(1004, 136);
            this.uiPanelSearch.TabIndex = 4;
            this.uiPanelSearch.Text = "광고/팝업 통합 레포트";
            // 
            // uiPanelSearchContainer
            // 
            this.uiPanelSearchContainer.Controls.Add(this.rptHeader);
            this.uiPanelSearchContainer.Location = new System.Drawing.Point(1, 23);
            this.uiPanelSearchContainer.Name = "uiPanelSearchContainer";
            this.uiPanelSearchContainer.Size = new System.Drawing.Size(1002, 110);
            this.uiPanelSearchContainer.TabIndex = 0;
            // 
            // rptHeader
            // 
            this.rptHeader.BackColor = System.Drawing.Color.DeepSkyBlue;
            this.rptHeader.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rptHeader.Location = new System.Drawing.Point(0, 0);
            this.rptHeader.Name = "rptHeader";
            this.rptHeader.Size = new System.Drawing.Size(1002, 110);
            this.rptHeader.TabIndex = 1;
            this.rptHeader.u_IsPrint = false;
            this.rptHeader.u_MenuName = "";
            // 
            // uiPanelList
            // 
            this.uiPanelList.CloseButtonVisible = Janus.Windows.UI.InheritableBoolean.False;
            this.uiPanelList.InnerContainer = this.uiPanelListContainer;
            this.uiPanelList.Location = new System.Drawing.Point(3, 139);
            this.uiPanelList.Name = "uiPanelList";
            this.uiPanelList.Size = new System.Drawing.Size(1004, 535);
            this.uiPanelList.TabIndex = 4;
            this.uiPanelList.Text = "목록";
            // 
            // uiPanelListContainer
            // 
            this.uiPanelListContainer.Controls.Add(this.gridControl1);
            this.uiPanelListContainer.Location = new System.Drawing.Point(1, 23);
            this.uiPanelListContainer.Name = "uiPanelListContainer";
            this.uiPanelListContainer.Size = new System.Drawing.Size(1002, 511);
            this.uiPanelListContainer.TabIndex = 0;
            // 
            // gridControl1
            // 
            this.gridControl1.DataSource = this.bsList;
            this.gridControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridControl1.Location = new System.Drawing.Point(0, 0);
            this.gridControl1.LookAndFeel.SkinName = "Metropolis";
            this.gridControl1.LookAndFeel.UseDefaultLookAndFeel = false;
            this.gridControl1.MainView = this.bandedGridView1;
            this.gridControl1.Name = "gridControl1";
            this.gridControl1.Size = new System.Drawing.Size(1002, 511);
            this.gridControl1.TabIndex = 0;
            this.gridControl1.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.bandedGridView1});
            // 
            // bsList
            // 
            this.bsList.DataMember = "ListTable";
            this.bsList.DataSource = this.combineRptDataSet;
            // 
            // combineRptDataSet
            // 
            this.combineRptDataSet.DataSetName = "CombineRptDataSet";
            this.combineRptDataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // bandedGridView1
            // 
            this.bandedGridView1.Bands.AddRange(new DevExpress.XtraGrid.Views.BandedGrid.GridBand[] {
            this.gridBand1,
            this.gridBand2,
            this.gridBand3});
            this.bandedGridView1.Columns.AddRange(new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn[] {
            this.colClass1Code,
            this.colClass1Name,
            this.colClass2Code,
            this.colClass2Name,
            this.colAdsImps,
            this.colAdsRate,
            this.colAdsHouse,
            this.colAdsReach,
            this.colAdsFreq,
            this.colPnsImps,
            this.colPnsRate,
            this.colPnsHouse,
            this.colPnsReach,
            this.colPnsFreq});
            styleFormatCondition1.Appearance.BackColor = System.Drawing.Color.SkyBlue;
            styleFormatCondition1.Appearance.Options.UseBackColor = true;
            styleFormatCondition1.ApplyToRow = true;
            styleFormatCondition1.Column = this.colClass2Code;
            styleFormatCondition1.Condition = DevExpress.XtraGrid.FormatConditionEnum.Equal;
            styleFormatCondition1.Value1 = "Z99999";
            this.bandedGridView1.FormatConditions.AddRange(new DevExpress.XtraGrid.StyleFormatCondition[] {
            styleFormatCondition1});
            this.bandedGridView1.GridControl = this.gridControl1;
            this.bandedGridView1.HorzScrollStep = 1;
            this.bandedGridView1.Name = "bandedGridView1";
            this.bandedGridView1.OptionsBehavior.AutoUpdateTotalSummary = false;
            this.bandedGridView1.OptionsCustomization.AllowColumnMoving = false;
            this.bandedGridView1.OptionsCustomization.AllowColumnResizing = false;
            this.bandedGridView1.OptionsCustomization.AllowFilter = false;
            this.bandedGridView1.OptionsCustomization.AllowGroup = false;
            this.bandedGridView1.OptionsMenu.EnableColumnMenu = false;
            this.bandedGridView1.OptionsMenu.EnableFooterMenu = false;
            this.bandedGridView1.OptionsMenu.EnableGroupPanelMenu = false;
            this.bandedGridView1.OptionsView.AllowCellMerge = true;
            this.bandedGridView1.OptionsView.ColumnAutoWidth = false;
            this.bandedGridView1.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never;
            this.bandedGridView1.OptionsView.ShowGroupPanel = false;
            this.bandedGridView1.OptionsView.ShowHorizontalLines = DevExpress.Utils.DefaultBoolean.True;
            this.bandedGridView1.OptionsView.ShowIndicator = false;
            this.bandedGridView1.OptionsView.ShowVerticalLines = DevExpress.Utils.DefaultBoolean.True;
            this.bandedGridView1.VertScrollVisibility = DevExpress.XtraGrid.Views.Base.ScrollVisibility.Always;
            // 
            // gridBand1
            // 
            this.gridBand1.AppearanceHeader.Options.UseTextOptions = true;
            this.gridBand1.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridBand1.Caption = "분류";
            this.gridBand1.Columns.Add(this.colClass1Code);
            this.gridBand1.Columns.Add(this.colClass1Name);
            this.gridBand1.Columns.Add(this.colClass2Code);
            this.gridBand1.Columns.Add(this.colClass2Name);
            this.gridBand1.Name = "gridBand1";
            this.gridBand1.Width = 270;
            // 
            // colClass1Code
            // 
            this.colClass1Code.FieldName = "Class1Code";
            this.colClass1Code.Name = "colClass1Code";
            this.colClass1Code.OptionsColumn.AllowEdit = false;
            this.colClass1Code.OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.True;
            // 
            // colClass1Name
            // 
            this.colClass1Name.AppearanceHeader.Options.UseTextOptions = true;
            this.colClass1Name.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colClass1Name.Caption = "항목";
            this.colClass1Name.FieldName = "Class1Name";
            this.colClass1Name.Name = "colClass1Name";
            this.colClass1Name.OptionsColumn.AllowEdit = false;
            this.colClass1Name.OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.True;
            this.colClass1Name.Visible = true;
            this.colClass1Name.Width = 60;
            // 
            // colClass2Name
            // 
            this.colClass2Name.AppearanceHeader.Options.UseTextOptions = true;
            this.colClass2Name.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colClass2Name.Caption = "세부항목";
            this.colClass2Name.FieldName = "Class2Name";
            this.colClass2Name.Name = "colClass2Name";
            this.colClass2Name.OptionsColumn.AllowEdit = false;
            this.colClass2Name.OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.True;
            this.colClass2Name.Visible = true;
            this.colClass2Name.Width = 210;
            // 
            // gridBand2
            // 
            this.gridBand2.AppearanceHeader.Options.UseTextOptions = true;
            this.gridBand2.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridBand2.Caption = "광고 집행 정보";
            this.gridBand2.Columns.Add(this.colAdsImps);
            this.gridBand2.Columns.Add(this.colAdsRate);
            this.gridBand2.Columns.Add(this.colAdsHouse);
            this.gridBand2.Columns.Add(this.colAdsReach);
            this.gridBand2.Columns.Add(this.colAdsFreq);
            this.gridBand2.Name = "gridBand2";
            this.gridBand2.Width = 357;
            // 
            // colAdsImps
            // 
            this.colAdsImps.AppearanceCell.Options.UseTextOptions = true;
            this.colAdsImps.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.colAdsImps.AppearanceHeader.Options.UseTextOptions = true;
            this.colAdsImps.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colAdsImps.Caption = "노출수";
            this.colAdsImps.DisplayFormat.FormatString = "##,##0";
            this.colAdsImps.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.colAdsImps.FieldName = "AdsImps";
            this.colAdsImps.Name = "colAdsImps";
            this.colAdsImps.OptionsColumn.AllowEdit = false;
            this.colAdsImps.OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.False;
            this.colAdsImps.Visible = true;
            this.colAdsImps.Width = 89;
            // 
            // colAdsRate
            // 
            this.colAdsRate.AppearanceCell.Options.UseTextOptions = true;
            this.colAdsRate.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.colAdsRate.AppearanceHeader.Options.UseTextOptions = true;
            this.colAdsRate.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colAdsRate.Caption = "비율";
            this.colAdsRate.DisplayFormat.FormatString = "#00.00";
            this.colAdsRate.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.colAdsRate.FieldName = "AdsRate";
            this.colAdsRate.Name = "colAdsRate";
            this.colAdsRate.OptionsColumn.AllowEdit = false;
            this.colAdsRate.OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.False;
            this.colAdsRate.Visible = true;
            this.colAdsRate.Width = 55;
            // 
            // colAdsHouse
            // 
            this.colAdsHouse.AppearanceCell.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.colAdsHouse.AppearanceCell.Options.UseFont = true;
            this.colAdsHouse.AppearanceCell.Options.UseTextOptions = true;
            this.colAdsHouse.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.colAdsHouse.AppearanceHeader.Options.UseTextOptions = true;
            this.colAdsHouse.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colAdsHouse.Caption = "시청가구";
            this.colAdsHouse.DisplayFormat.FormatString = "##,##0";
            this.colAdsHouse.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.colAdsHouse.FieldName = "AdsHouse";
            this.colAdsHouse.Name = "colAdsHouse";
            this.colAdsHouse.OptionsColumn.AllowEdit = false;
            this.colAdsHouse.OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.False;
            this.colAdsHouse.Visible = true;
            this.colAdsHouse.Width = 89;
            // 
            // colAdsReach
            // 
            this.colAdsReach.AppearanceHeader.Options.UseTextOptions = true;
            this.colAdsReach.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colAdsReach.Caption = "Reach";
            this.colAdsReach.FieldName = "AdsReach";
            this.colAdsReach.Name = "colAdsReach";
            this.colAdsReach.OptionsColumn.AllowEdit = false;
            this.colAdsReach.OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.False;
            this.colAdsReach.Visible = true;
            this.colAdsReach.Width = 55;
            // 
            // colAdsFreq
            // 
            this.colAdsFreq.AppearanceCell.Options.UseTextOptions = true;
            this.colAdsFreq.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.colAdsFreq.AppearanceHeader.Options.UseTextOptions = true;
            this.colAdsFreq.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colAdsFreq.Caption = "Freq";
            this.colAdsFreq.FieldName = "AdsFreq";
            this.colAdsFreq.Name = "colAdsFreq";
            this.colAdsFreq.OptionsColumn.AllowEdit = false;
            this.colAdsFreq.OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.False;
            this.colAdsFreq.Visible = true;
            this.colAdsFreq.Width = 69;
            // 
            // gridBand3
            // 
            this.gridBand3.AppearanceHeader.Options.UseTextOptions = true;
            this.gridBand3.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridBand3.Caption = "팝업 집행 정보";
            this.gridBand3.Columns.Add(this.colPnsImps);
            this.gridBand3.Columns.Add(this.colPnsRate);
            this.gridBand3.Columns.Add(this.colPnsHouse);
            this.gridBand3.Columns.Add(this.colPnsReach);
            this.gridBand3.Columns.Add(this.colPnsFreq);
            this.gridBand3.Name = "gridBand3";
            this.gridBand3.Width = 357;
            // 
            // colPnsImps
            // 
            this.colPnsImps.AppearanceCell.Options.UseTextOptions = true;
            this.colPnsImps.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.colPnsImps.AppearanceHeader.Options.UseTextOptions = true;
            this.colPnsImps.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colPnsImps.Caption = "노출수";
            this.colPnsImps.DisplayFormat.FormatString = "##,##0";
            this.colPnsImps.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.colPnsImps.FieldName = "PnsImps";
            this.colPnsImps.Name = "colPnsImps";
            this.colPnsImps.OptionsColumn.AllowEdit = false;
            this.colPnsImps.OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.False;
            this.colPnsImps.Visible = true;
            this.colPnsImps.Width = 89;
            // 
            // colPnsRate
            // 
            this.colPnsRate.AppearanceCell.Options.UseTextOptions = true;
            this.colPnsRate.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.colPnsRate.AppearanceHeader.Options.UseTextOptions = true;
            this.colPnsRate.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colPnsRate.Caption = "비율";
            this.colPnsRate.FieldName = "PnsRate";
            this.colPnsRate.Name = "colPnsRate";
            this.colPnsRate.OptionsColumn.AllowEdit = false;
            this.colPnsRate.OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.False;
            this.colPnsRate.Visible = true;
            this.colPnsRate.Width = 58;
            // 
            // colPnsHouse
            // 
            this.colPnsHouse.AppearanceCell.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.colPnsHouse.AppearanceCell.Options.UseFont = true;
            this.colPnsHouse.AppearanceCell.Options.UseTextOptions = true;
            this.colPnsHouse.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.colPnsHouse.AppearanceHeader.Options.UseTextOptions = true;
            this.colPnsHouse.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colPnsHouse.Caption = "시청가구";
            this.colPnsHouse.DisplayFormat.FormatString = "##,##0";
            this.colPnsHouse.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.colPnsHouse.FieldName = "PnsHouse";
            this.colPnsHouse.Name = "colPnsHouse";
            this.colPnsHouse.OptionsColumn.AllowEdit = false;
            this.colPnsHouse.OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.False;
            this.colPnsHouse.Visible = true;
            this.colPnsHouse.Width = 89;
            // 
            // colPnsReach
            // 
            this.colPnsReach.AppearanceHeader.Options.UseTextOptions = true;
            this.colPnsReach.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colPnsReach.Caption = "Reach";
            this.colPnsReach.FieldName = "PnsReach";
            this.colPnsReach.Name = "colPnsReach";
            this.colPnsReach.OptionsColumn.AllowEdit = false;
            this.colPnsReach.OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.False;
            this.colPnsReach.Visible = true;
            this.colPnsReach.Width = 58;
            // 
            // colPnsFreq
            // 
            this.colPnsFreq.AppearanceCell.Options.UseTextOptions = true;
            this.colPnsFreq.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.colPnsFreq.AppearanceHeader.Options.UseTextOptions = true;
            this.colPnsFreq.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colPnsFreq.Caption = "Freq";
            this.colPnsFreq.DisplayFormat.FormatString = "#0.00";
            this.colPnsFreq.FieldName = "PnsFreq";
            this.colPnsFreq.Name = "colPnsFreq";
            this.colPnsFreq.OptionsColumn.AllowEdit = false;
            this.colPnsFreq.OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.False;
            this.colPnsFreq.Visible = true;
            this.colPnsFreq.Width = 63;
            // 
            // CombineRptCtrl
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.uiPanelList);
            this.Controls.Add(this.uiPanelSearch);
            this.Font = global::AdManagerClient.Properties.Settings.Default.Font1;
            this.Name = "CombineRptCtrl";
            this.Size = new System.Drawing.Size(1010, 677);
            this.Load += new System.EventHandler(this.CombineRptCtrl_Load);
            ((System.ComponentModel.ISupportInitialize)(this.uiPM)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelSearch)).EndInit();
            this.uiPanelSearch.ResumeLayout(false);
            this.uiPanelSearchContainer.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelList)).EndInit();
            this.uiPanelList.ResumeLayout(false);
            this.uiPanelListContainer.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.combineRptDataSet)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bandedGridView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Janus.Windows.UI.Dock.UIPanelManager uiPM;
        private Janus.Windows.UI.Dock.UIPanel uiPanelSearch;
        private Janus.Windows.UI.Dock.UIPanelInnerContainer uiPanelSearchContainer;
        private Janus.Windows.UI.Dock.UIPanel uiPanelList;
        private Janus.Windows.UI.Dock.UIPanelInnerContainer uiPanelListContainer;
        private ReportHeaderControl rptHeader;
        private DevExpress.XtraGrid.GridControl gridControl1;
        private System.Windows.Forms.BindingSource bsList;
        private _51_ReportSummaryAd._11_CombineRpt.CombineRptDataSet combineRptDataSet;
        private DevExpress.XtraGrid.Views.BandedGrid.BandedGridView bandedGridView1;
        private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn colClass1Code;
        private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn colClass1Name;
        private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn colClass2Code;
        private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn colClass2Name;
        private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn colAdsImps;
        private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn colAdsRate;
        private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn colAdsHouse;
        private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn colAdsFreq;
        private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn colPnsImps;
        private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn colPnsRate;
        private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn colPnsHouse;
        private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn colPnsFreq;
        private DevExpress.XtraGrid.Views.BandedGrid.GridBand gridBand1;
        private DevExpress.XtraGrid.Views.BandedGrid.GridBand gridBand2;
        private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn colAdsReach;
        private DevExpress.XtraGrid.Views.BandedGrid.GridBand gridBand3;
        private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn colPnsReach;

    }
}
