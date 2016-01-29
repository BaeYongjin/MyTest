namespace AdManagerClient
{
    partial class GroupOrganizationControl
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
            Janus.Windows.GridEX.GridEXLayout grdExGroupList_DesignTimeLayout = new Janus.Windows.GridEX.GridEXLayout();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GroupOrganizationControl));
            Janus.Windows.GridEX.GridEXLayout grdExScheduleList_DesignTimeLayout = new Janus.Windows.GridEX.GridEXLayout();
            Janus.Windows.Common.Layouts.JanusLayoutReference grdExScheduleList_DesignTimeLayout_Reference_0 = new Janus.Windows.Common.Layouts.JanusLayoutReference("GridEXLayoutData.RootTable.FormatConditions.Condition3.FormatStyle.BackgroundImag" +
        "e");
            Janus.Windows.Common.Layouts.JanusLayoutReference grdExScheduleList_DesignTimeLayout_Reference_1 = new Janus.Windows.Common.Layouts.JanusLayoutReference("GridEXLayoutData.RootTable.FormatConditions.Condition4.FormatStyle.BackgroundImag" +
        "e");
            this.uiPM = new Janus.Windows.UI.Dock.UIPanelManager(this.components);
            this.uiPanelGroup = new Janus.Windows.UI.Dock.UIPanelGroup();
            this.uiPanelChooseAdSchedule = new Janus.Windows.UI.Dock.UIPanelGroup();
            this.uiPanelGroup1 = new Janus.Windows.UI.Dock.UIPanelGroup();
            this.uiPanelGroupList = new Janus.Windows.UI.Dock.UIPanel();
            this.uiPanel1Container = new Janus.Windows.UI.Dock.UIPanelInnerContainer();
            this.grdExGroupList = new Janus.Windows.GridEX.GridEX();
            this.dvGroupList = new System.Data.DataView();
            this.groupOrganizationDs = new AdManagerClient._10_Media._12_GroupOrganization.GroupOrganizationDs();
            this.uiPanel2 = new Janus.Windows.UI.Dock.UIPanel();
            this.uiPanel2Container = new Janus.Windows.UI.Dock.UIPanelInnerContainer();
            this.panel2 = new System.Windows.Forms.Panel();
            this.ebComment = new Janus.Windows.GridEX.EditControls.EditBox();
            this.lbComment = new System.Windows.Forms.Label();
            this.ebGroupName = new Janus.Windows.GridEX.EditControls.EditBox();
            this.lbGroupName = new System.Windows.Forms.Label();
            this.lbUseYn = new System.Windows.Forms.Label();
            this.rbUseYn_N = new Janus.Windows.EditControls.UIRadioButton();
            this.rbUseYn_Y = new Janus.Windows.EditControls.UIRadioButton();
            this.btnDelete = new Janus.Windows.EditControls.UIButton();
            this.btnSave = new Janus.Windows.EditControls.UIButton();
            this.btnAdd = new Janus.Windows.EditControls.UIButton();
            this.uiPanelGroupSchedule = new Janus.Windows.UI.Dock.UIPanel();
            this.uiPanelGroupScheduleContainer = new Janus.Windows.UI.Dock.UIPanelInnerContainer();
            this.grdExScheduleList = new Janus.Windows.GridEX.GridEX();
            this.dvGroupDetail = new System.Data.DataView();
            this.uiPanelCommand = new Janus.Windows.UI.Dock.UIPanel();
            this.uiPanelCommandContainer = new Janus.Windows.UI.Dock.UIPanelInnerContainer();
            this.pnlDetail = new System.Windows.Forms.Panel();
            this.lbl_CntKaon = new System.Windows.Forms.Label();
            this.lbl_CntDefault = new System.Windows.Forms.Label();
            this.lbl_SizeKaon = new System.Windows.Forms.Label();
            this.lbl_CntSamSung = new System.Windows.Forms.Label();
            this.lblKaon = new System.Windows.Forms.Label();
            this.lbl_SizeDefault = new System.Windows.Forms.Label();
            this.lblDefault = new System.Windows.Forms.Label();
            this.lbl_SizeSamSung = new System.Windows.Forms.Label();
            this.lblSamSung = new System.Windows.Forms.Label();
            this.gbCommand = new Janus.Windows.EditControls.UIGroupBox();
            this.btnAdd1 = new Janus.Windows.EditControls.UIButton();
            this.btnDelete1 = new Janus.Windows.EditControls.UIButton();
            this.btnAddCm = new Janus.Windows.EditControls.UIButton();
            this.gbSTB = new Janus.Windows.EditControls.UIGroupBox();
            this.btnSTB_HDD = new Janus.Windows.EditControls.UIButton();
            this.btnSTB_Common = new Janus.Windows.EditControls.UIButton();
            this.gbLog = new Janus.Windows.EditControls.UIGroupBox();
            this.btnNot = new Janus.Windows.EditControls.UIButton();
            this.btnOk = new Janus.Windows.EditControls.UIButton();
            this.gbScheduling = new Janus.Windows.EditControls.UIGroupBox();
            this.btnOrderUp = new Janus.Windows.EditControls.UIButton();
            this.btnOrderDown = new Janus.Windows.EditControls.UIButton();
            this.btnOrderFirst = new Janus.Windows.EditControls.UIButton();
            this.btnOrderLast = new Janus.Windows.EditControls.UIButton();
            this.pnlSearch = new System.Windows.Forms.Panel();
            this.cbSearchMedia = new Janus.Windows.EditControls.UIComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.uiPM)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelGroup)).BeginInit();
            this.uiPanelGroup.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelChooseAdSchedule)).BeginInit();
            this.uiPanelChooseAdSchedule.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelGroup1)).BeginInit();
            this.uiPanelGroup1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelGroupList)).BeginInit();
            this.uiPanelGroupList.SuspendLayout();
            this.uiPanel1Container.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdExGroupList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dvGroupList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupOrganizationDs)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanel2)).BeginInit();
            this.uiPanel2.SuspendLayout();
            this.uiPanel2Container.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelGroupSchedule)).BeginInit();
            this.uiPanelGroupSchedule.SuspendLayout();
            this.uiPanelGroupScheduleContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdExScheduleList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dvGroupDetail)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelCommand)).BeginInit();
            this.uiPanelCommand.SuspendLayout();
            this.uiPanelCommandContainer.SuspendLayout();
            this.pnlDetail.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gbCommand)).BeginInit();
            this.gbCommand.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gbSTB)).BeginInit();
            this.gbSTB.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gbLog)).BeginInit();
            this.gbLog.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gbScheduling)).BeginInit();
            this.gbScheduling.SuspendLayout();
            this.pnlSearch.SuspendLayout();
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
            this.uiPanelGroup.Id = new System.Guid("a51f5afe-fbb7-4068-8dd2-1d74cc5f28ae");
            this.uiPanelGroup.StaticGroup = true;
            this.uiPanelChooseAdSchedule.Id = new System.Guid("097eed5c-e6f9-4f65-a0d9-e81277dd5b94");
            this.uiPanelChooseAdSchedule.StaticGroup = true;
            this.uiPanelGroup1.Id = new System.Guid("5c2a5642-9bd0-49a4-905a-7b158c9cc0b8");
            this.uiPanelGroup1.StaticGroup = true;
            this.uiPanelGroupList.Id = new System.Guid("b46414ad-1780-4909-8bd9-a7a6e9d753d7");
            this.uiPanelGroup1.Panels.Add(this.uiPanelGroupList);
            this.uiPanel2.Id = new System.Guid("8226c01b-08fa-4a5e-ac96-dd9a7b428ec4");
            this.uiPanelGroup1.Panels.Add(this.uiPanel2);
            this.uiPanelChooseAdSchedule.Panels.Add(this.uiPanelGroup1);
            this.uiPanelGroup.Panels.Add(this.uiPanelChooseAdSchedule);
            this.uiPanelGroupSchedule.Id = new System.Guid("48659b33-8f11-405a-b830-b9cb8eb7fd6a");
            this.uiPanelGroup.Panels.Add(this.uiPanelGroupSchedule);
            this.uiPanelCommand.Id = new System.Guid("b5ff0d0c-ece3-49b2-bf27-3281c91576d7");
            this.uiPanelGroup.Panels.Add(this.uiPanelCommand);
            this.uiPM.Panels.Add(this.uiPanelGroup);
            // 
            // Design Time Panel Info:
            // 
            this.uiPM.BeginPanelInfo();
            this.uiPM.AddDockPanelInfo(new System.Guid("a51f5afe-fbb7-4068-8dd2-1d74cc5f28ae"), Janus.Windows.UI.Dock.PanelGroupStyle.HorizontalTiles, Janus.Windows.UI.Dock.PanelDockStyle.Fill, true, new System.Drawing.Size(1010, 677), true);
            this.uiPM.AddDockPanelInfo(new System.Guid("097eed5c-e6f9-4f65-a0d9-e81277dd5b94"), new System.Guid("a51f5afe-fbb7-4068-8dd2-1d74cc5f28ae"), Janus.Windows.UI.Dock.PanelGroupStyle.VerticalTiles, true, 203, true);
            this.uiPM.AddDockPanelInfo(new System.Guid("5c2a5642-9bd0-49a4-905a-7b158c9cc0b8"), new System.Guid("097eed5c-e6f9-4f65-a0d9-e81277dd5b94"), Janus.Windows.UI.Dock.PanelGroupStyle.VerticalTiles, true, 1010, true);
            this.uiPM.AddDockPanelInfo(new System.Guid("b46414ad-1780-4909-8bd9-a7a6e9d753d7"), new System.Guid("5c2a5642-9bd0-49a4-905a-7b158c9cc0b8"), 531, true);
            this.uiPM.AddDockPanelInfo(new System.Guid("8226c01b-08fa-4a5e-ac96-dd9a7b428ec4"), new System.Guid("5c2a5642-9bd0-49a4-905a-7b158c9cc0b8"), 475, true);
            this.uiPM.AddDockPanelInfo(new System.Guid("48659b33-8f11-405a-b830-b9cb8eb7fd6a"), new System.Guid("a51f5afe-fbb7-4068-8dd2-1d74cc5f28ae"), 365, true);
            this.uiPM.AddDockPanelInfo(new System.Guid("b5ff0d0c-ece3-49b2-bf27-3281c91576d7"), new System.Guid("a51f5afe-fbb7-4068-8dd2-1d74cc5f28ae"), 79, true);
            this.uiPM.AddFloatingPanelInfo(new System.Guid("a51f5afe-fbb7-4068-8dd2-1d74cc5f28ae"), Janus.Windows.UI.Dock.PanelGroupStyle.HorizontalTiles, true, new System.Drawing.Point(-1, -1), new System.Drawing.Size(200, 200), false);
            this.uiPM.AddFloatingPanelInfo(new System.Guid("097eed5c-e6f9-4f65-a0d9-e81277dd5b94"), Janus.Windows.UI.Dock.PanelGroupStyle.VerticalTiles, true, new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
            this.uiPM.AddFloatingPanelInfo(new System.Guid("5c2a5642-9bd0-49a4-905a-7b158c9cc0b8"), Janus.Windows.UI.Dock.PanelGroupStyle.VerticalTiles, true, new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
            this.uiPM.AddFloatingPanelInfo(new System.Guid("b46414ad-1780-4909-8bd9-a7a6e9d753d7"), new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
            this.uiPM.AddFloatingPanelInfo(new System.Guid("8226c01b-08fa-4a5e-ac96-dd9a7b428ec4"), new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
            this.uiPM.AddFloatingPanelInfo(new System.Guid("48659b33-8f11-405a-b830-b9cb8eb7fd6a"), new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
            this.uiPM.AddFloatingPanelInfo(new System.Guid("b5ff0d0c-ece3-49b2-bf27-3281c91576d7"), new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
            this.uiPM.EndPanelInfo();
            // 
            // uiPanelGroup
            // 
            this.uiPanelGroup.ActiveCaptionMode = Janus.Windows.UI.Dock.ActiveCaptionMode.Never;
            this.uiPanelGroup.AllowResize = Janus.Windows.UI.InheritableBoolean.True;
            this.uiPanelGroup.BackColor = System.Drawing.SystemColors.Window;
            this.uiPanelGroup.CaptionStyle = Janus.Windows.UI.Dock.PanelCaptionStyle.Standard;
            this.uiPanelGroup.CaptionVisible = Janus.Windows.UI.InheritableBoolean.True;
            this.uiPanelGroup.CloseButtonVisible = Janus.Windows.UI.InheritableBoolean.False;
            this.uiPanelGroup.Location = new System.Drawing.Point(0, 0);
            this.uiPanelGroup.Name = "uiPanelGroup";
            this.uiPanelGroup.Size = new System.Drawing.Size(1010, 677);
            this.uiPanelGroup.TabIndex = 4;
            this.uiPanelGroup.Text = "OAP 편성그룹관리";
            // 
            // uiPanelChooseAdSchedule
            // 
            this.uiPanelChooseAdSchedule.AllowResize = Janus.Windows.UI.InheritableBoolean.True;
            this.uiPanelChooseAdSchedule.CaptionHeight = 20;
            this.uiPanelChooseAdSchedule.CaptionStyle = Janus.Windows.UI.Dock.PanelCaptionStyle.Dark;
            this.uiPanelChooseAdSchedule.CaptionVisible = Janus.Windows.UI.InheritableBoolean.False;
            this.uiPanelChooseAdSchedule.GroupStyle = Janus.Windows.UI.Dock.PanelGroupStyle.VerticalTiles;
            this.uiPanelChooseAdSchedule.Location = new System.Drawing.Point(0, 22);
            this.uiPanelChooseAdSchedule.Name = "uiPanelChooseAdSchedule";
            this.uiPanelChooseAdSchedule.Size = new System.Drawing.Size(1010, 203);
            this.uiPanelChooseAdSchedule.TabIndex = 4;
            this.uiPanelChooseAdSchedule.Text = "그룹목록";
            // 
            // uiPanelGroup1
            // 
            this.uiPanelGroup1.CaptionHeight = 0;
            this.uiPanelGroup1.GroupStyle = Janus.Windows.UI.Dock.PanelGroupStyle.VerticalTiles;
            this.uiPanelGroup1.Location = new System.Drawing.Point(0, 0);
            this.uiPanelGroup1.Name = "uiPanelGroup1";
            this.uiPanelGroup1.Size = new System.Drawing.Size(1010, 203);
            this.uiPanelGroup1.TabIndex = 4;
            // 
            // uiPanelGroupList
            // 
            this.uiPanelGroupList.CaptionStyle = Janus.Windows.UI.Dock.PanelCaptionStyle.Light;
            this.uiPanelGroupList.CaptionVisible = Janus.Windows.UI.InheritableBoolean.False;
            this.uiPanelGroupList.InnerContainer = this.uiPanel1Container;
            this.uiPanelGroupList.Location = new System.Drawing.Point(0, -1);
            this.uiPanelGroupList.Name = "uiPanelGroupList";
            this.uiPanelGroupList.Size = new System.Drawing.Size(531, 204);
            this.uiPanelGroupList.TabIndex = 4;
            this.uiPanelGroupList.Text = "편성그룹 목록";
            // 
            // uiPanel1Container
            // 
            this.uiPanel1Container.Controls.Add(this.grdExGroupList);
            this.uiPanel1Container.Location = new System.Drawing.Point(1, 1);
            this.uiPanel1Container.Name = "uiPanel1Container";
            this.uiPanel1Container.Size = new System.Drawing.Size(529, 202);
            this.uiPanel1Container.TabIndex = 0;
            // 
            // grdExGroupList
            // 
            this.grdExGroupList.AllowEdit = Janus.Windows.GridEX.InheritableBoolean.False;
            this.grdExGroupList.AlternatingColors = true;
            this.grdExGroupList.BorderStyle = Janus.Windows.GridEX.BorderStyle.None;
            this.grdExGroupList.DataSource = this.dvGroupList;
            grdExGroupList_DesignTimeLayout.LayoutString = resources.GetString("grdExGroupList_DesignTimeLayout.LayoutString");
            this.grdExGroupList.DesignTimeLayout = grdExGroupList_DesignTimeLayout;
            this.grdExGroupList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdExGroupList.EmptyRows = true;
            this.grdExGroupList.FocusCellFormatStyle.BackColor = System.Drawing.SystemColors.Highlight;
            this.grdExGroupList.FocusCellFormatStyle.ForeColor = System.Drawing.Color.White;
            this.grdExGroupList.FocusStyle = Janus.Windows.GridEX.FocusStyle.None;
            this.grdExGroupList.GridLineColor = System.Drawing.Color.Silver;
            this.grdExGroupList.GridLines = Janus.Windows.GridEX.GridLines.Vertical;
            this.grdExGroupList.GridLineStyle = Janus.Windows.GridEX.GridLineStyle.Solid;
            this.grdExGroupList.GroupByBoxVisible = false;
            this.grdExGroupList.HeaderFormatStyle.TextAlignment = Janus.Windows.GridEX.TextAlignment.Center;
            this.grdExGroupList.HideSelection = Janus.Windows.GridEX.HideSelection.HighlightInactive;
            this.grdExGroupList.Location = new System.Drawing.Point(0, 0);
            this.grdExGroupList.Name = "grdExGroupList";
            this.grdExGroupList.ScrollBars = Janus.Windows.GridEX.ScrollBars.Vertical;
            this.grdExGroupList.SelectedInactiveFormatStyle.BackColor = System.Drawing.Color.Gold;
            this.grdExGroupList.Size = new System.Drawing.Size(529, 202);
            this.grdExGroupList.TabIndex = 5;
            this.grdExGroupList.ThemedAreas = ((Janus.Windows.GridEX.ThemedArea)(((((((((Janus.Windows.GridEX.ThemedArea.ScrollBars | Janus.Windows.GridEX.ThemedArea.EditControls) 
            | Janus.Windows.GridEX.ThemedArea.Headers) 
            | Janus.Windows.GridEX.ThemedArea.GroupByBox) 
            | Janus.Windows.GridEX.ThemedArea.GroupRows) 
            | Janus.Windows.GridEX.ThemedArea.ControlBorder) 
            | Janus.Windows.GridEX.ThemedArea.Cards) 
            | Janus.Windows.GridEX.ThemedArea.Gridlines) 
            | Janus.Windows.GridEX.ThemedArea.CheckBoxes)));
            this.grdExGroupList.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            this.grdExGroupList.SelectionChanged += new System.EventHandler(this.grdExGroupList_SelectionChanged);
            this.grdExGroupList.Enter += new System.EventHandler(this.OnGrdGroupRowChange);
            // 
            // dvGroupList
            // 
            this.dvGroupList.Table = this.groupOrganizationDs.GroupList;
            // 
            // groupOrganizationDs
            // 
            this.groupOrganizationDs.DataSetName = "GroupOrganizationDs";
            this.groupOrganizationDs.Locale = new System.Globalization.CultureInfo("en-US");
            this.groupOrganizationDs.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // uiPanel2
            // 
            this.uiPanel2.CaptionStyle = Janus.Windows.UI.Dock.PanelCaptionStyle.Light;
            this.uiPanel2.CaptionVisible = Janus.Windows.UI.InheritableBoolean.False;
            this.uiPanel2.InnerContainer = this.uiPanel2Container;
            this.uiPanel2.Location = new System.Drawing.Point(535, -1);
            this.uiPanel2.Name = "uiPanel2";
            this.uiPanel2.Size = new System.Drawing.Size(475, 204);
            this.uiPanel2.TabIndex = 4;
            this.uiPanel2.Text = "편성그룹 내역";
            // 
            // uiPanel2Container
            // 
            this.uiPanel2Container.Controls.Add(this.panel2);
            this.uiPanel2Container.Location = new System.Drawing.Point(1, 1);
            this.uiPanel2Container.Name = "uiPanel2Container";
            this.uiPanel2Container.Size = new System.Drawing.Size(473, 202);
            this.uiPanel2Container.TabIndex = 0;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.ebComment);
            this.panel2.Controls.Add(this.lbComment);
            this.panel2.Controls.Add(this.ebGroupName);
            this.panel2.Controls.Add(this.lbGroupName);
            this.panel2.Controls.Add(this.lbUseYn);
            this.panel2.Controls.Add(this.rbUseYn_N);
            this.panel2.Controls.Add(this.rbUseYn_Y);
            this.panel2.Controls.Add(this.btnDelete);
            this.panel2.Controls.Add(this.btnSave);
            this.panel2.Controls.Add(this.btnAdd);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(473, 202);
            this.panel2.TabIndex = 1;
            // 
            // ebComment
            // 
            this.ebComment.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.ebComment.Location = new System.Drawing.Point(56, 55);
            this.ebComment.MaxLength = 1000;
            this.ebComment.Multiline = true;
            this.ebComment.Name = "ebComment";
            this.ebComment.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.ebComment.Size = new System.Drawing.Size(295, 56);
            this.ebComment.TabIndex = 7;
            this.ebComment.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
            this.ebComment.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2003;
            // 
            // lbComment
            // 
            this.lbComment.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lbComment.Location = new System.Drawing.Point(8, 55);
            this.lbComment.Name = "lbComment";
            this.lbComment.Size = new System.Drawing.Size(40, 21);
            this.lbComment.TabIndex = 23;
            this.lbComment.Text = "비고";
            this.lbComment.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ebGroupName
            // 
            this.ebGroupName.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.ebGroupName.Location = new System.Drawing.Point(56, 31);
            this.ebGroupName.MaxLength = 200;
            this.ebGroupName.Name = "ebGroupName";
            this.ebGroupName.Size = new System.Drawing.Size(295, 22);
            this.ebGroupName.TabIndex = 6;
            this.ebGroupName.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
            this.ebGroupName.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2003;
            // 
            // lbGroupName
            // 
            this.lbGroupName.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lbGroupName.BackColor = System.Drawing.SystemColors.Window;
            this.lbGroupName.Location = new System.Drawing.Point(8, 31);
            this.lbGroupName.Name = "lbGroupName";
            this.lbGroupName.Size = new System.Drawing.Size(48, 21);
            this.lbGroupName.TabIndex = 13;
            this.lbGroupName.Text = "그룹명";
            this.lbGroupName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lbUseYn
            // 
            this.lbUseYn.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lbUseYn.Location = new System.Drawing.Point(8, 115);
            this.lbUseYn.Name = "lbUseYn";
            this.lbUseYn.Size = new System.Drawing.Size(72, 21);
            this.lbUseYn.TabIndex = 43;
            this.lbUseYn.Text = "사용여부";
            this.lbUseYn.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // rbUseYn_N
            // 
            this.rbUseYn_N.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.rbUseYn_N.BackColor = System.Drawing.Color.Transparent;
            this.rbUseYn_N.ForeColor = System.Drawing.Color.OrangeRed;
            this.rbUseYn_N.Location = new System.Drawing.Point(160, 115);
            this.rbUseYn_N.Name = "rbUseYn_N";
            this.rbUseYn_N.Size = new System.Drawing.Size(72, 23);
            this.rbUseYn_N.TabIndex = 42;
            this.rbUseYn_N.Text = "사용안함";
            // 
            // rbUseYn_Y
            // 
            this.rbUseYn_Y.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.rbUseYn_Y.BackColor = System.Drawing.Color.Transparent;
            this.rbUseYn_Y.Checked = true;
            this.rbUseYn_Y.Location = new System.Drawing.Point(80, 115);
            this.rbUseYn_Y.Name = "rbUseYn_Y";
            this.rbUseYn_Y.Size = new System.Drawing.Size(72, 23);
            this.rbUseYn_Y.TabIndex = 8;
            this.rbUseYn_Y.TabStop = true;
            this.rbUseYn_Y.Text = "사용함";
            // 
            // btnDelete
            // 
            this.btnDelete.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.btnDelete.Enabled = false;
            this.btnDelete.Location = new System.Drawing.Point(368, 62);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(90, 24);
            this.btnDelete.TabIndex = 11;
            this.btnDelete.Text = "삭 제";
            this.btnDelete.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnSave
            // 
            this.btnSave.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.btnSave.Enabled = false;
            this.btnSave.Location = new System.Drawing.Point(368, 94);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(90, 24);
            this.btnSave.TabIndex = 12;
            this.btnSave.Text = "저 장";
            this.btnSave.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnAdd
            // 
            this.btnAdd.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.btnAdd.Enabled = false;
            this.btnAdd.Location = new System.Drawing.Point(368, 31);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(90, 24);
            this.btnAdd.TabIndex = 10;
            this.btnAdd.Text = "추 가";
            this.btnAdd.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // uiPanelGroupSchedule
            // 
            this.uiPanelGroupSchedule.ActiveCaptionMode = Janus.Windows.UI.Dock.ActiveCaptionMode.Never;
            this.uiPanelGroupSchedule.AllowResize = Janus.Windows.UI.InheritableBoolean.False;
            this.uiPanelGroupSchedule.CaptionHeight = 20;
            this.uiPanelGroupSchedule.CaptionStyle = Janus.Windows.UI.Dock.PanelCaptionStyle.Dark;
            this.uiPanelGroupSchedule.CaptionVisible = Janus.Windows.UI.InheritableBoolean.True;
            this.uiPanelGroupSchedule.InnerContainer = this.uiPanelGroupScheduleContainer;
            this.uiPanelGroupSchedule.Location = new System.Drawing.Point(0, 229);
            this.uiPanelGroupSchedule.Name = "uiPanelGroupSchedule";
            this.uiPanelGroupSchedule.Size = new System.Drawing.Size(1010, 365);
            this.uiPanelGroupSchedule.TabIndex = 4;
            this.uiPanelGroupSchedule.Text = "홈광고 편성현황";
            // 
            // uiPanelGroupScheduleContainer
            // 
            this.uiPanelGroupScheduleContainer.Controls.Add(this.grdExScheduleList);
            this.uiPanelGroupScheduleContainer.Location = new System.Drawing.Point(1, 20);
            this.uiPanelGroupScheduleContainer.Name = "uiPanelGroupScheduleContainer";
            this.uiPanelGroupScheduleContainer.Size = new System.Drawing.Size(1008, 344);
            this.uiPanelGroupScheduleContainer.TabIndex = 0;
            // 
            // grdExScheduleList
            // 
            this.grdExScheduleList.AutomaticSort = false;
            this.grdExScheduleList.BorderStyle = Janus.Windows.GridEX.BorderStyle.None;
            this.grdExScheduleList.DataSource = this.dvGroupDetail;
            grdExScheduleList_DesignTimeLayout_Reference_0.Instance = ((object)(resources.GetObject("grdExScheduleList_DesignTimeLayout_Reference_0.Instance")));
            grdExScheduleList_DesignTimeLayout_Reference_1.Instance = ((object)(resources.GetObject("grdExScheduleList_DesignTimeLayout_Reference_1.Instance")));
            grdExScheduleList_DesignTimeLayout.LayoutReferences.AddRange(new Janus.Windows.Common.Layouts.JanusLayoutReference[] {
            grdExScheduleList_DesignTimeLayout_Reference_0,
            grdExScheduleList_DesignTimeLayout_Reference_1});
            grdExScheduleList_DesignTimeLayout.LayoutString = resources.GetString("grdExScheduleList_DesignTimeLayout.LayoutString");
            this.grdExScheduleList.DesignTimeLayout = grdExScheduleList_DesignTimeLayout;
            this.grdExScheduleList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdExScheduleList.EmptyRows = true;
            this.grdExScheduleList.FocusCellFormatStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(174)))), ((int)(((byte)(196)))), ((int)(((byte)(232)))));
            this.grdExScheduleList.FocusCellFormatStyle.BackColorGradient = System.Drawing.Color.FromArgb(((int)(((byte)(174)))), ((int)(((byte)(196)))), ((int)(((byte)(232)))));
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
            this.grdExScheduleList.Size = new System.Drawing.Size(1008, 344);
            this.grdExScheduleList.TabIndex = 6;
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
            // 
            // dvGroupDetail
            // 
            this.dvGroupDetail.Table = this.groupOrganizationDs.AdSchedule;
            // 
            // uiPanelCommand
            // 
            this.uiPanelCommand.AllowResize = Janus.Windows.UI.InheritableBoolean.False;
            this.uiPanelCommand.CaptionStyle = Janus.Windows.UI.Dock.PanelCaptionStyle.Dark;
            this.uiPanelCommand.CaptionVisible = Janus.Windows.UI.InheritableBoolean.False;
            this.uiPanelCommand.InnerContainer = this.uiPanelCommandContainer;
            this.uiPanelCommand.Location = new System.Drawing.Point(0, 598);
            this.uiPanelCommand.Name = "uiPanelCommand";
            this.uiPanelCommand.Size = new System.Drawing.Size(1010, 79);
            this.uiPanelCommand.TabIndex = 4;
            this.uiPanelCommand.Text = "편성";
            // 
            // uiPanelCommandContainer
            // 
            this.uiPanelCommandContainer.Controls.Add(this.pnlDetail);
            this.uiPanelCommandContainer.Location = new System.Drawing.Point(1, 1);
            this.uiPanelCommandContainer.Name = "uiPanelCommandContainer";
            this.uiPanelCommandContainer.Size = new System.Drawing.Size(1008, 77);
            this.uiPanelCommandContainer.TabIndex = 0;
            // 
            // pnlDetail
            // 
            this.pnlDetail.BackColor = System.Drawing.SystemColors.Window;
            this.pnlDetail.Controls.Add(this.lbl_CntKaon);
            this.pnlDetail.Controls.Add(this.lbl_CntDefault);
            this.pnlDetail.Controls.Add(this.lbl_SizeKaon);
            this.pnlDetail.Controls.Add(this.lbl_CntSamSung);
            this.pnlDetail.Controls.Add(this.lblKaon);
            this.pnlDetail.Controls.Add(this.lbl_SizeDefault);
            this.pnlDetail.Controls.Add(this.lblDefault);
            this.pnlDetail.Controls.Add(this.lbl_SizeSamSung);
            this.pnlDetail.Controls.Add(this.lblSamSung);
            this.pnlDetail.Controls.Add(this.gbCommand);
            this.pnlDetail.Controls.Add(this.gbSTB);
            this.pnlDetail.Controls.Add(this.gbLog);
            this.pnlDetail.Controls.Add(this.gbScheduling);
            this.pnlDetail.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlDetail.Location = new System.Drawing.Point(0, 0);
            this.pnlDetail.Name = "pnlDetail";
            this.pnlDetail.Size = new System.Drawing.Size(1008, 77);
            this.pnlDetail.TabIndex = 12;
            // 
            // lbl_CntKaon
            // 
            this.lbl_CntKaon.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lbl_CntKaon.Location = new System.Drawing.Point(63, 6);
            this.lbl_CntKaon.Name = "lbl_CntKaon";
            this.lbl_CntKaon.Size = new System.Drawing.Size(51, 19);
            this.lbl_CntKaon.TabIndex = 59;
            this.lbl_CntKaon.Text = "0";
            this.lbl_CntKaon.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lbl_CntDefault
            // 
            this.lbl_CntDefault.BackColor = System.Drawing.Color.Transparent;
            this.lbl_CntDefault.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lbl_CntDefault.Location = new System.Drawing.Point(63, 52);
            this.lbl_CntDefault.Name = "lbl_CntDefault";
            this.lbl_CntDefault.Size = new System.Drawing.Size(51, 19);
            this.lbl_CntDefault.TabIndex = 56;
            this.lbl_CntDefault.Text = "0";
            this.lbl_CntDefault.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lbl_SizeKaon
            // 
            this.lbl_SizeKaon.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lbl_SizeKaon.Location = new System.Drawing.Point(126, 6);
            this.lbl_SizeKaon.Name = "lbl_SizeKaon";
            this.lbl_SizeKaon.Size = new System.Drawing.Size(109, 19);
            this.lbl_SizeKaon.TabIndex = 58;
            this.lbl_SizeKaon.Text = "2,000,000,000";
            this.lbl_SizeKaon.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lbl_CntSamSung
            // 
            this.lbl_CntSamSung.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lbl_CntSamSung.Location = new System.Drawing.Point(63, 30);
            this.lbl_CntSamSung.Name = "lbl_CntSamSung";
            this.lbl_CntSamSung.Size = new System.Drawing.Size(51, 19);
            this.lbl_CntSamSung.TabIndex = 55;
            this.lbl_CntSamSung.Text = "0";
            this.lbl_CntSamSung.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblKaon
            // 
            this.lblKaon.Location = new System.Drawing.Point(14, 6);
            this.lblKaon.Name = "lblKaon";
            this.lblKaon.Size = new System.Drawing.Size(47, 19);
            this.lblKaon.TabIndex = 57;
            this.lblKaon.Text = "가온";
            this.lblKaon.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lbl_SizeDefault
            // 
            this.lbl_SizeDefault.BackColor = System.Drawing.Color.Transparent;
            this.lbl_SizeDefault.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lbl_SizeDefault.Location = new System.Drawing.Point(126, 52);
            this.lbl_SizeDefault.Name = "lbl_SizeDefault";
            this.lbl_SizeDefault.Size = new System.Drawing.Size(109, 19);
            this.lbl_SizeDefault.TabIndex = 54;
            this.lbl_SizeDefault.Text = "2,000,000,000";
            this.lbl_SizeDefault.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblDefault
            // 
            this.lblDefault.Location = new System.Drawing.Point(14, 52);
            this.lblDefault.Name = "lblDefault";
            this.lblDefault.Size = new System.Drawing.Size(47, 19);
            this.lblDefault.TabIndex = 53;
            this.lblDefault.Text = "기본";
            this.lblDefault.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lbl_SizeSamSung
            // 
            this.lbl_SizeSamSung.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lbl_SizeSamSung.Location = new System.Drawing.Point(126, 30);
            this.lbl_SizeSamSung.Name = "lbl_SizeSamSung";
            this.lbl_SizeSamSung.Size = new System.Drawing.Size(109, 19);
            this.lbl_SizeSamSung.TabIndex = 52;
            this.lbl_SizeSamSung.Text = "2,000,000,000";
            this.lbl_SizeSamSung.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblSamSung
            // 
            this.lblSamSung.Location = new System.Drawing.Point(14, 30);
            this.lblSamSung.Name = "lblSamSung";
            this.lblSamSung.Size = new System.Drawing.Size(47, 19);
            this.lblSamSung.TabIndex = 51;
            this.lblSamSung.Text = "삼성";
            this.lblSamSung.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // gbCommand
            // 
            this.gbCommand.Controls.Add(this.btnAdd1);
            this.gbCommand.Controls.Add(this.btnDelete1);
            this.gbCommand.Controls.Add(this.btnAddCm);
            this.gbCommand.Location = new System.Drawing.Point(259, 12);
            this.gbCommand.Name = "gbCommand";
            this.gbCommand.Size = new System.Drawing.Size(243, 52);
            this.gbCommand.TabIndex = 50;
            this.gbCommand.Text = "명령구분";
            this.gbCommand.VisualStyle = Janus.Windows.UI.Dock.PanelVisualStyle.Office2007;
            // 
            // btnAdd1
            // 
            this.btnAdd1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnAdd1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnAdd1.Location = new System.Drawing.Point(8, 20);
            this.btnAdd1.Name = "btnAdd1";
            this.btnAdd1.Size = new System.Drawing.Size(70, 24);
            this.btnAdd1.TabIndex = 13;
            this.btnAdd1.Text = "추 가";
            this.btnAdd1.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.btnAdd1.Click += new System.EventHandler(this.btnAdd1_Click);
            // 
            // btnDelete1
            // 
            this.btnDelete1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnDelete1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnDelete1.Location = new System.Drawing.Point(180, 20);
            this.btnDelete1.Name = "btnDelete1";
            this.btnDelete1.Size = new System.Drawing.Size(54, 24);
            this.btnDelete1.TabIndex = 15;
            this.btnDelete1.Text = "삭 제";
            this.btnDelete1.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.btnDelete1.Click += new System.EventHandler(this.btnDelete1_Click);
            // 
            // btnAddCm
            // 
            this.btnAddCm.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnAddCm.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnAddCm.Location = new System.Drawing.Point(81, 20);
            this.btnAddCm.Name = "btnAddCm";
            this.btnAddCm.Size = new System.Drawing.Size(93, 24);
            this.btnAddCm.TabIndex = 14;
            this.btnAddCm.Text = "상업광고추가";
            this.btnAddCm.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.btnAddCm.Click += new System.EventHandler(this.btnAddCm_Click);
            // 
            // gbSTB
            // 
            this.gbSTB.Controls.Add(this.btnSTB_HDD);
            this.gbSTB.Controls.Add(this.btnSTB_Common);
            this.gbSTB.Location = new System.Drawing.Point(509, 12);
            this.gbSTB.Name = "gbSTB";
            this.gbSTB.Size = new System.Drawing.Size(99, 52);
            this.gbSTB.TabIndex = 49;
            this.gbSTB.Text = "셋탑구분";
            this.gbSTB.Visible = false;
            this.gbSTB.VisualStyle = Janus.Windows.UI.Dock.PanelVisualStyle.Office2007;
            // 
            // btnSTB_HDD
            // 
            this.btnSTB_HDD.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSTB_HDD.Location = new System.Drawing.Point(51, 20);
            this.btnSTB_HDD.Name = "btnSTB_HDD";
            this.btnSTB_HDD.Size = new System.Drawing.Size(40, 24);
            this.btnSTB_HDD.TabIndex = 17;
            this.btnSTB_HDD.Text = "기본";
            this.btnSTB_HDD.ToolTipText = "HDD가 있는 셋탑에 적용됩니다. 셀론/현대 셋탑";
            this.btnSTB_HDD.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.btnSTB_HDD.Click += new System.EventHandler(this.btnSTB_HDD_Click);
            // 
            // btnSTB_Common
            // 
            this.btnSTB_Common.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSTB_Common.Location = new System.Drawing.Point(8, 20);
            this.btnSTB_Common.Name = "btnSTB_Common";
            this.btnSTB_Common.Size = new System.Drawing.Size(40, 24);
            this.btnSTB_Common.TabIndex = 16;
            this.btnSTB_Common.Text = "공용";
            this.btnSTB_Common.ToolTipText = "전체 셋탑에 적용됩니다";
            this.btnSTB_Common.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.btnSTB_Common.Click += new System.EventHandler(this.btnSTB_Common_Click);
            // 
            // gbLog
            // 
            this.gbLog.Controls.Add(this.btnNot);
            this.gbLog.Controls.Add(this.btnOk);
            this.gbLog.Location = new System.Drawing.Point(616, 12);
            this.gbLog.Name = "gbLog";
            this.gbLog.Size = new System.Drawing.Size(100, 52);
            this.gbLog.TabIndex = 44;
            this.gbLog.Text = "로그설정";
            this.gbLog.VisualStyle = Janus.Windows.UI.Dock.PanelVisualStyle.Office2007;
            // 
            // btnNot
            // 
            this.btnNot.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnNot.Location = new System.Drawing.Point(51, 20);
            this.btnNot.Name = "btnNot";
            this.btnNot.Size = new System.Drawing.Size(40, 24);
            this.btnNot.TabIndex = 19;
            this.btnNot.Text = "해제";
            this.btnNot.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.btnNot.Click += new System.EventHandler(this.btnNot_Click);
            // 
            // btnOk
            // 
            this.btnOk.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnOk.Location = new System.Drawing.Point(8, 20);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(40, 24);
            this.btnOk.TabIndex = 18;
            this.btnOk.Text = "설정";
            this.btnOk.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // gbScheduling
            // 
            this.gbScheduling.Controls.Add(this.btnOrderUp);
            this.gbScheduling.Controls.Add(this.btnOrderDown);
            this.gbScheduling.Controls.Add(this.btnOrderFirst);
            this.gbScheduling.Controls.Add(this.btnOrderLast);
            this.gbScheduling.Location = new System.Drawing.Point(722, 12);
            this.gbScheduling.Name = "gbScheduling";
            this.gbScheduling.Size = new System.Drawing.Size(267, 52);
            this.gbScheduling.TabIndex = 41;
            this.gbScheduling.Text = "편성순서변경";
            this.gbScheduling.VisualStyle = Janus.Windows.UI.Dock.PanelVisualStyle.Office2007;
            // 
            // btnOrderUp
            // 
            this.btnOrderUp.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnOrderUp.Image = ((System.Drawing.Image)(resources.GetObject("btnOrderUp.Image")));
            this.btnOrderUp.ImageSize = new System.Drawing.Size(13, 12);
            this.btnOrderUp.Location = new System.Drawing.Point(72, 20);
            this.btnOrderUp.Name = "btnOrderUp";
            this.btnOrderUp.Size = new System.Drawing.Size(60, 24);
            this.btnOrderUp.TabIndex = 21;
            this.btnOrderUp.Text = "올림";
            this.btnOrderUp.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.btnOrderUp.Click += new System.EventHandler(this.btnOrderUp_Click);
            // 
            // btnOrderDown
            // 
            this.btnOrderDown.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnOrderDown.Image = ((System.Drawing.Image)(resources.GetObject("btnOrderDown.Image")));
            this.btnOrderDown.ImageSize = new System.Drawing.Size(13, 12);
            this.btnOrderDown.Location = new System.Drawing.Point(136, 20);
            this.btnOrderDown.Name = "btnOrderDown";
            this.btnOrderDown.Size = new System.Drawing.Size(60, 24);
            this.btnOrderDown.TabIndex = 22;
            this.btnOrderDown.Text = "내림";
            this.btnOrderDown.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.btnOrderDown.Click += new System.EventHandler(this.btnOrderDown_Click);
            // 
            // btnOrderFirst
            // 
            this.btnOrderFirst.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnOrderFirst.Image = ((System.Drawing.Image)(resources.GetObject("btnOrderFirst.Image")));
            this.btnOrderFirst.ImageSize = new System.Drawing.Size(13, 16);
            this.btnOrderFirst.Location = new System.Drawing.Point(8, 20);
            this.btnOrderFirst.Name = "btnOrderFirst";
            this.btnOrderFirst.Size = new System.Drawing.Size(60, 24);
            this.btnOrderFirst.TabIndex = 20;
            this.btnOrderFirst.Text = "처음";
            this.btnOrderFirst.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.btnOrderFirst.Click += new System.EventHandler(this.btnOrderFirst_Click);
            // 
            // btnOrderLast
            // 
            this.btnOrderLast.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnOrderLast.Image = ((System.Drawing.Image)(resources.GetObject("btnOrderLast.Image")));
            this.btnOrderLast.ImageSize = new System.Drawing.Size(13, 16);
            this.btnOrderLast.Location = new System.Drawing.Point(200, 20);
            this.btnOrderLast.Name = "btnOrderLast";
            this.btnOrderLast.Size = new System.Drawing.Size(60, 24);
            this.btnOrderLast.TabIndex = 23;
            this.btnOrderLast.Text = "끝";
            this.btnOrderLast.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.btnOrderLast.Click += new System.EventHandler(this.btnOrderLast_Click);
            // 
            // pnlSearch
            // 
            this.pnlSearch.BackColor = System.Drawing.SystemColors.Window;
            this.pnlSearch.Controls.Add(this.cbSearchMedia);
            this.pnlSearch.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlSearch.Location = new System.Drawing.Point(0, 0);
            this.pnlSearch.Name = "pnlSearch";
            this.pnlSearch.Size = new System.Drawing.Size(1010, 42);
            this.pnlSearch.TabIndex = 4;
            // 
            // cbSearchMedia
            // 
            this.cbSearchMedia.ComboStyle = Janus.Windows.EditControls.ComboStyle.DropDownList;
            this.cbSearchMedia.Location = new System.Drawing.Point(8, 8);
            this.cbSearchMedia.Name = "cbSearchMedia";
            this.cbSearchMedia.Size = new System.Drawing.Size(152, 21);
            this.cbSearchMedia.TabIndex = 34;
            this.cbSearchMedia.Text = "매체선택";
            this.cbSearchMedia.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            // 
            // GroupOrganizationControl
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.uiPanelGroup);
            this.Font = global::AdManagerClient.Properties.Settings.Default.Font1;
            this.Name = "GroupOrganizationControl";
            this.Size = new System.Drawing.Size(1010, 677);
            this.Load += new System.EventHandler(this.GroupOrganization_Load);
            ((System.ComponentModel.ISupportInitialize)(this.uiPM)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelGroup)).EndInit();
            this.uiPanelGroup.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelChooseAdSchedule)).EndInit();
            this.uiPanelChooseAdSchedule.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelGroup1)).EndInit();
            this.uiPanelGroup1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelGroupList)).EndInit();
            this.uiPanelGroupList.ResumeLayout(false);
            this.uiPanel1Container.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdExGroupList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dvGroupList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupOrganizationDs)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanel2)).EndInit();
            this.uiPanel2.ResumeLayout(false);
            this.uiPanel2Container.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelGroupSchedule)).EndInit();
            this.uiPanelGroupSchedule.ResumeLayout(false);
            this.uiPanelGroupScheduleContainer.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdExScheduleList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dvGroupDetail)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelCommand)).EndInit();
            this.uiPanelCommand.ResumeLayout(false);
            this.uiPanelCommandContainer.ResumeLayout(false);
            this.pnlDetail.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gbCommand)).EndInit();
            this.gbCommand.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gbSTB)).EndInit();
            this.gbSTB.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gbLog)).EndInit();
            this.gbLog.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gbScheduling)).EndInit();
            this.gbScheduling.ResumeLayout(false);
            this.pnlSearch.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Janus.Windows.UI.Dock.UIPanelManager uiPM;
        private Janus.Windows.UI.Dock.UIPanelGroup uiPanelGroup;
        private Janus.Windows.UI.Dock.UIPanelGroup uiPanelChooseAdSchedule;
        private Janus.Windows.UI.Dock.UIPanel uiPanelGroupSchedule;
        private Janus.Windows.UI.Dock.UIPanelInnerContainer uiPanelGroupScheduleContainer;
        private Janus.Windows.UI.Dock.UIPanel uiPanelCommand;
        private Janus.Windows.UI.Dock.UIPanelInnerContainer uiPanelCommandContainer;
        private Janus.Windows.UI.Dock.UIPanelGroup uiPanelGroup1;
        private Janus.Windows.UI.Dock.UIPanel uiPanelGroupList;
        private Janus.Windows.UI.Dock.UIPanelInnerContainer uiPanel1Container;
        private Janus.Windows.UI.Dock.UIPanel uiPanel2;
        private Janus.Windows.UI.Dock.UIPanelInnerContainer uiPanel2Container;
        private System.Windows.Forms.Panel pnlSearch;
        private Janus.Windows.EditControls.UIComboBox cbSearchMedia;
        private Janus.Windows.GridEX.GridEX grdExGroupList;
        private System.Windows.Forms.Panel panel2;
        private Janus.Windows.GridEX.EditControls.EditBox ebComment;
        private System.Windows.Forms.Label lbComment;
        private Janus.Windows.GridEX.EditControls.EditBox ebGroupName;
        private System.Windows.Forms.Label lbGroupName;
        private System.Windows.Forms.Label lbUseYn;
        private Janus.Windows.EditControls.UIRadioButton rbUseYn_N;
        private Janus.Windows.EditControls.UIRadioButton rbUseYn_Y;
        private Janus.Windows.EditControls.UIButton btnDelete;
        private Janus.Windows.EditControls.UIButton btnSave;
        private Janus.Windows.EditControls.UIButton btnAdd;
        private Janus.Windows.GridEX.GridEX grdExScheduleList;
        private System.Windows.Forms.Panel pnlDetail;
        private System.Windows.Forms.Label lbl_CntKaon;
        private System.Windows.Forms.Label lbl_CntDefault;
        private System.Windows.Forms.Label lbl_SizeKaon;
        private System.Windows.Forms.Label lbl_CntSamSung;
        private System.Windows.Forms.Label lblKaon;
        private System.Windows.Forms.Label lbl_SizeDefault;
        private System.Windows.Forms.Label lblDefault;
        private System.Windows.Forms.Label lbl_SizeSamSung;
        private System.Windows.Forms.Label lblSamSung;
        private Janus.Windows.EditControls.UIGroupBox gbCommand;
        private Janus.Windows.EditControls.UIButton btnAdd1;
        private Janus.Windows.EditControls.UIButton btnDelete1;
        private Janus.Windows.EditControls.UIButton btnAddCm;
        private Janus.Windows.EditControls.UIGroupBox gbSTB;
        private Janus.Windows.EditControls.UIButton btnSTB_HDD;
        private Janus.Windows.EditControls.UIButton btnSTB_Common;
        private Janus.Windows.EditControls.UIGroupBox gbLog;
        private Janus.Windows.EditControls.UIButton btnNot;
        private Janus.Windows.EditControls.UIButton btnOk;
        private Janus.Windows.EditControls.UIGroupBox gbScheduling;
        private Janus.Windows.EditControls.UIButton btnOrderUp;
        private Janus.Windows.EditControls.UIButton btnOrderDown;
        private Janus.Windows.EditControls.UIButton btnOrderFirst;
        private Janus.Windows.EditControls.UIButton btnOrderLast;
        private _10_Media._12_GroupOrganization.GroupOrganizationDs groupOrganizationDs;
        private System.Data.DataView dvGroupList;
        private System.Data.DataView dvGroupDetail;

    }
}
