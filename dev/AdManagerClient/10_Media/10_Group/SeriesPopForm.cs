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
	public class SeriesPopForm : System.Windows.Forms.Form
	{
		#region �̺�Ʈ�ڵ鷯
		public event StatusEventHandler 			StatusEvent;			// �����̺�Ʈ �ڵ鷯
        public event ProgressEventHandler 			ProgressEvent;			// ó�����̺�Ʈ �ڵ鷯

		#endregion
			
		#region ��������� ��ü �� ����

		// �ý��� ���� : ȭ�����
		private SystemModel   systemModel   = FrameSystem.oSysModel;
		private CommonModel   commonModel   = FrameSystem.oComModel;
		private Logger        log           = FrameSystem.oLog;

		// ����� ������
		GroupModel groupModel  = new GroupModel();	// ������������
				
		GroupControl ChannelSetCtl = null;

		public string keyMedia		= "1";
		public string KeyCategory	= "";
		public string KeyGenre		= "";
		public string keyChannel	= "";
		public string keyChannelNm	= "";
		public string keySeries		= "";

		// ȭ��ó���� ����
        bool IsNewChannelSearchKey = true;					// �˻����Է� ����
        bool IsNewSeriesSearchKey = true;					// �˻����Է� ����
        CurrencyManager cm = null;					// ������ �׸����� ���濡 ���� �����ͼ� ������ ���Ͽ�			
		DataTable       dt        = null;

		#endregion

		private System.Data.DataView dvChannelSet;
		private Janus.Windows.EditControls.UIButton btnClose;
		private Janus.Windows.EditControls.UIButton btnOk;		
		private Janus.Windows.EditControls.UIComboBox cbSearchMediaName;
		private Janus.Windows.EditControls.UIComboBox cbSearchCategoryName;
		private Janus.Windows.EditControls.UIComboBox cbSearchGenreName;
		private AdManagerClient._10_Media._10_Group.GroupDs groupDs;
		private Janus.Windows.UI.Dock.UIPanelManager uiPM;
		private Janus.Windows.UI.Dock.UIPanel panelTop;
		private Janus.Windows.UI.Dock.UIPanelInnerContainer panelTopContainer;
		private Janus.Windows.UI.Dock.UIPanelGroup panelMain;
		private Janus.Windows.UI.Dock.UIPanel panelCommand;
		private Janus.Windows.UI.Dock.UIPanelInnerContainer panelCommandContainer;
		private Janus.Windows.UI.Dock.UIPanel panelChannel;
		private Janus.Windows.UI.Dock.UIPanelInnerContainer panelChannelContainer;
		private Janus.Windows.UI.Dock.UIPanel pannelSeries;
		private Janus.Windows.UI.Dock.UIPanelInnerContainer pannelSeriesContainer;
		private System.ComponentModel.IContainer components;
		private Janus.Windows.GridEX.GridEX grdExSeries;
		private System.Data.DataView dvSeries;
        private Label label1;
        private Label lbGenre;
        private Janus.Windows.UI.Dock.UIPanelGroup panelChannels;
        private Janus.Windows.UI.Dock.UIPanelGroup panelSerieses;
        private Janus.Windows.EditControls.UICheckBox chkInvalidMenu;
        private Janus.Windows.GridEX.EditControls.EditBox ebChannelSearchKey;
        private Janus.Windows.EditControls.UIButton btnChannelSearch;
        private Label lbChannel;
		private Janus.Windows.GridEX.GridEX grdExChannelNoList;
		//		private System.ComponentModel.IContainer components;

		/// <summary>
		/// ������ �Ѱܾ� �� ��
		/// </summary>
		/// <param name="sender"></param>
		public SeriesPopForm(GroupControl sender)
		{
			InitializeComponent();
			ChannelSetCtl = sender;
		}

		/// <summary>
		/// �Ϲݻ����
		/// </summary>
		public SeriesPopForm()
		{
			InitializeComponent();
			ChannelSetCtl = null;
		}

		/// <summary>
		/// ��� ���� ��� ���ҽ��� �����մϴ�.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				/*
				if(components != null)
				{
					components.Dispose();
				}
				*/
			}
			base.Dispose( disposing );
		}

		#region Windows Form �����̳ʿ��� ������ �ڵ�
		/// <summary>
		/// �����̳� ������ �ʿ��� �޼����Դϴ�.
		/// �� �޼����� ������ �ڵ� ������� �������� ���ʽÿ�.
		/// </summary>
		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            Janus.Windows.GridEX.GridEXLayout grdExChannelNoList_Layout_0 = new Janus.Windows.GridEX.GridEXLayout();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SeriesPopForm));
            Janus.Windows.GridEX.GridEXLayout grdExSeries_Layout_0 = new Janus.Windows.GridEX.GridEXLayout();
            this.dvChannelSet = new System.Data.DataView();
            this.groupDs = new AdManagerClient._10_Media._10_Group.GroupDs();
            this.dvSeries = new System.Data.DataView();
            this.cbSearchMediaName = new Janus.Windows.EditControls.UIComboBox();
            this.cbSearchCategoryName = new Janus.Windows.EditControls.UIComboBox();
            this.cbSearchGenreName = new Janus.Windows.EditControls.UIComboBox();
            this.grdExChannelNoList = new Janus.Windows.GridEX.GridEX();
            this.btnClose = new Janus.Windows.EditControls.UIButton();
            this.btnOk = new Janus.Windows.EditControls.UIButton();
            this.uiPM = new Janus.Windows.UI.Dock.UIPanelManager(this.components);
            this.panelTop = new Janus.Windows.UI.Dock.UIPanel();
            this.panelTopContainer = new Janus.Windows.UI.Dock.UIPanelInnerContainer();
            this.lbChannel = new System.Windows.Forms.Label();
            this.ebChannelSearchKey = new Janus.Windows.GridEX.EditControls.EditBox();
            this.btnChannelSearch = new Janus.Windows.EditControls.UIButton();
            this.label1 = new System.Windows.Forms.Label();
            this.lbGenre = new System.Windows.Forms.Label();
            this.chkInvalidMenu = new Janus.Windows.EditControls.UICheckBox();
            this.panelCommand = new Janus.Windows.UI.Dock.UIPanel();
            this.panelCommandContainer = new Janus.Windows.UI.Dock.UIPanelInnerContainer();
            this.panelMain = new Janus.Windows.UI.Dock.UIPanelGroup();
            this.panelChannels = new Janus.Windows.UI.Dock.UIPanelGroup();
            this.panelChannel = new Janus.Windows.UI.Dock.UIPanel();
            this.panelChannelContainer = new Janus.Windows.UI.Dock.UIPanelInnerContainer();
            this.panelSerieses = new Janus.Windows.UI.Dock.UIPanelGroup();
            this.pannelSeries = new Janus.Windows.UI.Dock.UIPanel();
            this.pannelSeriesContainer = new Janus.Windows.UI.Dock.UIPanelInnerContainer();
            this.grdExSeries = new Janus.Windows.GridEX.GridEX();
            ((System.ComponentModel.ISupportInitialize)(this.dvChannelSet)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupDs)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dvSeries)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdExChannelNoList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiPM)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelTop)).BeginInit();
            this.panelTop.SuspendLayout();
            this.panelTopContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelCommand)).BeginInit();
            this.panelCommand.SuspendLayout();
            this.panelCommandContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelMain)).BeginInit();
            this.panelMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelChannels)).BeginInit();
            this.panelChannels.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelChannel)).BeginInit();
            this.panelChannel.SuspendLayout();
            this.panelChannelContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelSerieses)).BeginInit();
            this.panelSerieses.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pannelSeries)).BeginInit();
            this.pannelSeries.SuspendLayout();
            this.pannelSeriesContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdExSeries)).BeginInit();
            this.SuspendLayout();
            // 
            // dvChannelSet
            // 
            this.dvChannelSet.Table = this.groupDs.Channel;
            // 
            // groupDs
            // 
            this.groupDs.DataSetName = "GroupDs";
            this.groupDs.Locale = new System.Globalization.CultureInfo("en-US");
            this.groupDs.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // dvSeries
            // 
            this.dvSeries.Table = this.groupDs.Series;
            // 
            // cbSearchMediaName
            // 
            this.cbSearchMediaName.ComboStyle = Janus.Windows.EditControls.ComboStyle.DropDownList;
            this.cbSearchMediaName.Location = new System.Drawing.Point(8, 67);
            this.cbSearchMediaName.Name = "cbSearchMediaName";
            this.cbSearchMediaName.Size = new System.Drawing.Size(28, 22);
            this.cbSearchMediaName.TabIndex = 0;
            this.cbSearchMediaName.Visible = false;
            this.cbSearchMediaName.VisualStyle = Janus.Windows.UI.VisualStyle.Office2003;
            // 
            // cbSearchCategoryName
            // 
            this.cbSearchCategoryName.ComboStyle = Janus.Windows.EditControls.ComboStyle.DropDownList;
            this.cbSearchCategoryName.Location = new System.Drawing.Point(111, 32);
            this.cbSearchCategoryName.Name = "cbSearchCategoryName";
            this.cbSearchCategoryName.Size = new System.Drawing.Size(234, 22);
            this.cbSearchCategoryName.TabIndex = 36;
            this.cbSearchCategoryName.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.cbSearchCategoryName.SelectedIndexChanged += new System.EventHandler(this.cbSearchCategoryName_SelectedIndexChanged);
            // 
            // cbSearchGenreName
            // 
            this.cbSearchGenreName.ComboStyle = Janus.Windows.EditControls.ComboStyle.DropDownList;
            this.cbSearchGenreName.Location = new System.Drawing.Point(442, 32);
            this.cbSearchGenreName.Name = "cbSearchGenreName";
            this.cbSearchGenreName.Size = new System.Drawing.Size(280, 22);
            this.cbSearchGenreName.TabIndex = 1;
            this.cbSearchGenreName.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.cbSearchGenreName.SelectedIndexChanged += new System.EventHandler(this.cbSearchGenreName_SelectedIndexChanged);
            // 
            // grdExChannelNoList
            // 
            this.grdExChannelNoList.AllowEdit = Janus.Windows.GridEX.InheritableBoolean.False;
            this.grdExChannelNoList.AlternatingColors = true;
            this.grdExChannelNoList.AutomaticSort = false;
            this.grdExChannelNoList.DataSource = this.dvChannelSet;
            this.grdExChannelNoList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdExChannelNoList.EmptyRows = true;
            this.grdExChannelNoList.FocusCellFormatStyle.BackColor = System.Drawing.SystemColors.Highlight;
            this.grdExChannelNoList.FocusCellFormatStyle.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.grdExChannelNoList.FocusStyle = Janus.Windows.GridEX.FocusStyle.None;
            this.grdExChannelNoList.Font = new System.Drawing.Font("���� ���", 8.25F);
            this.grdExChannelNoList.GridLineColor = System.Drawing.Color.Silver;
            this.grdExChannelNoList.GridLines = Janus.Windows.GridEX.GridLines.Vertical;
            this.grdExChannelNoList.GridLineStyle = Janus.Windows.GridEX.GridLineStyle.Solid;
            this.grdExChannelNoList.GroupByBoxVisible = false;
            this.grdExChannelNoList.GroupMode = Janus.Windows.GridEX.GroupMode.Collapsed;
            this.grdExChannelNoList.GroupRowVisualStyle = Janus.Windows.GridEX.GroupRowVisualStyle.Outlook2003;
            this.grdExChannelNoList.HideSelection = Janus.Windows.GridEX.HideSelection.HighlightInactive;
            grdExChannelNoList_Layout_0.DataSource = this.dvChannelSet;
            grdExChannelNoList_Layout_0.IsCurrentLayout = true;
            grdExChannelNoList_Layout_0.Key = "bae";
            grdExChannelNoList_Layout_0.LayoutString = resources.GetString("grdExChannelNoList_Layout_0.LayoutString");
            this.grdExChannelNoList.Layouts.AddRange(new Janus.Windows.GridEX.GridEXLayout[] {
            grdExChannelNoList_Layout_0});
            this.grdExChannelNoList.Location = new System.Drawing.Point(0, 0);
            this.grdExChannelNoList.Name = "grdExChannelNoList";
            this.grdExChannelNoList.ScrollBars = Janus.Windows.GridEX.ScrollBars.Vertical;
            this.grdExChannelNoList.SelectedInactiveFormatStyle.BackColor = System.Drawing.Color.Gold;
            this.grdExChannelNoList.Size = new System.Drawing.Size(360, 482);
            this.grdExChannelNoList.TabIndex = 0;
            this.grdExChannelNoList.TabKeyBehavior = Janus.Windows.GridEX.TabKeyBehavior.ControlNavigation;
            this.grdExChannelNoList.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            // 
            // btnClose
            // 
            this.btnClose.BackColor = System.Drawing.SystemColors.Control;
            this.btnClose.Location = new System.Drawing.Point(357, 9);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(70, 23);
            this.btnClose.TabIndex = 1;
            this.btnClose.Text = "�ݱ�";
            this.btnClose.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnOk
            // 
            this.btnOk.BackColor = System.Drawing.SystemColors.Control;
            this.btnOk.Location = new System.Drawing.Point(277, 9);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(70, 23);
            this.btnOk.TabIndex = 0;
            this.btnOk.Text = "��";
            this.btnOk.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // uiPM
            // 
            this.uiPM.BackColorGradientAutoHideStrip = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(218)))), ((int)(((byte)(250)))));
            this.uiPM.ContainerControl = this;
            this.uiPM.SplitterSize = 2;
            this.uiPM.VisualStyle = Janus.Windows.UI.Dock.PanelVisualStyle.Office2007;
            this.panelTop.Id = new System.Guid("e66ff17c-21a2-49a4-8f27-8c98444bc743");
            this.uiPM.Panels.Add(this.panelTop);
            this.panelCommand.Id = new System.Guid("d24b852b-a665-4e41-a3b8-aad454bd7f9e");
            this.uiPM.Panels.Add(this.panelCommand);
            this.panelMain.Id = new System.Guid("625eb345-691e-4af9-b09b-a067e4ba693f");
            this.panelMain.StaticGroup = true;
            this.panelChannels.Id = new System.Guid("a70f71a5-f15c-48d6-bc8e-cf74dd33c676");
            this.panelChannels.StaticGroup = true;
            this.panelChannel.Id = new System.Guid("91c12c8a-f823-446f-9fba-53b8a52f3956");
            this.panelChannels.Panels.Add(this.panelChannel);
            this.panelMain.Panels.Add(this.panelChannels);
            this.panelSerieses.Id = new System.Guid("7de40c90-f0d8-4483-b856-904311e416e2");
            this.panelSerieses.StaticGroup = true;
            this.pannelSeries.Id = new System.Guid("c6984d3c-322c-40f5-857d-2f8d1c65e27e");
            this.panelSerieses.Panels.Add(this.pannelSeries);
            this.panelMain.Panels.Add(this.panelSerieses);
            this.uiPM.Panels.Add(this.panelMain);
            // 
            // Design Time Panel Info:
            // 
            this.uiPM.BeginPanelInfo();
            this.uiPM.AddDockPanelInfo(new System.Guid("e66ff17c-21a2-49a4-8f27-8c98444bc743"), Janus.Windows.UI.Dock.PanelDockStyle.Top, new System.Drawing.Size(737, 65), true);
            this.uiPM.AddDockPanelInfo(new System.Guid("625eb345-691e-4af9-b09b-a067e4ba693f"), Janus.Windows.UI.Dock.PanelGroupStyle.VerticalTiles, Janus.Windows.UI.Dock.PanelDockStyle.Fill, true, new System.Drawing.Size(737, 482), true);
            this.uiPM.AddDockPanelInfo(new System.Guid("a70f71a5-f15c-48d6-bc8e-cf74dd33c676"), new System.Guid("625eb345-691e-4af9-b09b-a067e4ba693f"), Janus.Windows.UI.Dock.PanelGroupStyle.HorizontalTiles, true, 360, true);
            this.uiPM.AddDockPanelInfo(new System.Guid("91c12c8a-f823-446f-9fba-53b8a52f3956"), new System.Guid("a70f71a5-f15c-48d6-bc8e-cf74dd33c676"), 445, true);
            this.uiPM.AddDockPanelInfo(new System.Guid("7de40c90-f0d8-4483-b856-904311e416e2"), new System.Guid("625eb345-691e-4af9-b09b-a067e4ba693f"), Janus.Windows.UI.Dock.PanelGroupStyle.HorizontalTiles, true, 375, true);
            this.uiPM.AddDockPanelInfo(new System.Guid("c6984d3c-322c-40f5-857d-2f8d1c65e27e"), new System.Guid("7de40c90-f0d8-4483-b856-904311e416e2"), 466, true);
            this.uiPM.AddDockPanelInfo(new System.Guid("d24b852b-a665-4e41-a3b8-aad454bd7f9e"), Janus.Windows.UI.Dock.PanelDockStyle.Bottom, new System.Drawing.Size(737, 49), true);
            this.uiPM.AddFloatingPanelInfo(new System.Guid("e66ff17c-21a2-49a4-8f27-8c98444bc743"), new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
            this.uiPM.AddFloatingPanelInfo(new System.Guid("d24b852b-a665-4e41-a3b8-aad454bd7f9e"), new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
            this.uiPM.AddFloatingPanelInfo(new System.Guid("625eb345-691e-4af9-b09b-a067e4ba693f"), Janus.Windows.UI.Dock.PanelGroupStyle.VerticalTiles, true, new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
            this.uiPM.AddFloatingPanelInfo(new System.Guid("91c12c8a-f823-446f-9fba-53b8a52f3956"), new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
            this.uiPM.AddFloatingPanelInfo(new System.Guid("c6984d3c-322c-40f5-857d-2f8d1c65e27e"), new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
            this.uiPM.AddFloatingPanelInfo(new System.Guid("a70f71a5-f15c-48d6-bc8e-cf74dd33c676"), Janus.Windows.UI.Dock.PanelGroupStyle.HorizontalTiles, true, new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
            this.uiPM.AddFloatingPanelInfo(new System.Guid("799bbd1a-f865-4bcd-8d9a-6194b3d5537c"), new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
            this.uiPM.AddFloatingPanelInfo(new System.Guid("7de40c90-f0d8-4483-b856-904311e416e2"), Janus.Windows.UI.Dock.PanelGroupStyle.HorizontalTiles, true, new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
            this.uiPM.AddFloatingPanelInfo(new System.Guid("e97aa12c-ad20-4f24-ad61-ce3b89052758"), new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
            this.uiPM.EndPanelInfo();
            // 
            // panelTop
            // 
            this.panelTop.CaptionVisible = Janus.Windows.UI.InheritableBoolean.False;
            this.panelTop.InnerAreaStyle = Janus.Windows.UI.Dock.PanelInnerAreaStyle.Window;
            this.panelTop.InnerContainer = this.panelTopContainer;
            this.panelTop.Location = new System.Drawing.Point(3, 3);
            this.panelTop.Name = "panelTop";
            this.panelTop.Size = new System.Drawing.Size(737, 65);
            this.panelTop.TabIndex = 4;
            this.panelTop.Text = "Top";
            // 
            // panelTopContainer
            // 
            this.panelTopContainer.Controls.Add(this.lbChannel);
            this.panelTopContainer.Controls.Add(this.ebChannelSearchKey);
            this.panelTopContainer.Controls.Add(this.btnChannelSearch);
            this.panelTopContainer.Controls.Add(this.cbSearchCategoryName);
            this.panelTopContainer.Controls.Add(this.label1);
            this.panelTopContainer.Controls.Add(this.lbGenre);
            this.panelTopContainer.Controls.Add(this.cbSearchMediaName);
            this.panelTopContainer.Controls.Add(this.cbSearchGenreName);
            this.panelTopContainer.Controls.Add(this.chkInvalidMenu);
            this.panelTopContainer.Location = new System.Drawing.Point(1, 1);
            this.panelTopContainer.Name = "panelTopContainer";
            this.panelTopContainer.Size = new System.Drawing.Size(735, 61);
            this.panelTopContainer.TabIndex = 0;
            // 
            // lbChannel
            // 
            this.lbChannel.AutoSize = true;
            this.lbChannel.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.lbChannel.Location = new System.Drawing.Point(353, 11);
            this.lbChannel.Name = "lbChannel";
            this.lbChannel.Size = new System.Drawing.Size(88, 13);
            this.lbChannel.TabIndex = 53;
            this.lbChannel.Text = "���α׷��� �˻�";
            // 
            // ebChannelSearchKey
            // 
            this.ebChannelSearchKey.ButtonFont = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ebChannelSearchKey.Location = new System.Drawing.Point(442, 5);
            this.ebChannelSearchKey.Name = "ebChannelSearchKey";
            this.ebChannelSearchKey.Size = new System.Drawing.Size(202, 22);
            this.ebChannelSearchKey.TabIndex = 52;
            this.ebChannelSearchKey.Text = "�˻�� �Է��ϼ���";
            this.ebChannelSearchKey.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
            this.ebChannelSearchKey.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            this.ebChannelSearchKey.TextChanged += new System.EventHandler(this.ebChannelSearchKey_TextChanged);
            this.ebChannelSearchKey.Click += new System.EventHandler(this.ebChannelSearchKey_Click);
            this.ebChannelSearchKey.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ebChannelSearchKey_KeyDown);
            // 
            // btnChannelSearch
            // 
            this.btnChannelSearch.BackColor = System.Drawing.Color.White;
            this.btnChannelSearch.Location = new System.Drawing.Point(649, 5);
            this.btnChannelSearch.Name = "btnChannelSearch";
            this.btnChannelSearch.Size = new System.Drawing.Size(72, 21);
            this.btnChannelSearch.TabIndex = 51;
            this.btnChannelSearch.Text = "�� ȸ";
            this.btnChannelSearch.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.btnChannelSearch.Click += new System.EventHandler(this.btnChannelSearch_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.label1.Location = new System.Drawing.Point(5, 37);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(96, 13);
            this.label1.TabIndex = 38;
            this.label1.Text = "1���޴�(ī�װ�)";
            // 
            // lbGenre
            // 
            this.lbGenre.AutoSize = true;
            this.lbGenre.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.lbGenre.Location = new System.Drawing.Point(355, 37);
            this.lbGenre.Name = "lbGenre";
            this.lbGenre.Size = new System.Drawing.Size(82, 13);
            this.lbGenre.TabIndex = 37;
            this.lbGenre.Text = "2�� �޴� (�帣)";
            // 
            // chkInvalidMenu
            // 
            this.chkInvalidMenu.Location = new System.Drawing.Point(9, 0);
            this.chkInvalidMenu.Name = "chkInvalidMenu";
            this.chkInvalidMenu.ShowFocusRectangle = false;
            this.chkInvalidMenu.Size = new System.Drawing.Size(88, 26);
            this.chkInvalidMenu.TabIndex = 50;
            this.chkInvalidMenu.Text = "��ȿ�޴�����";
            this.chkInvalidMenu.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.chkInvalidMenu.CheckedChanged += new System.EventHandler(this.chkInvalidMenu_CheckedChanged);
            // 
            // panelCommand
            // 
            this.panelCommand.CaptionVisible = Janus.Windows.UI.InheritableBoolean.False;
            this.panelCommand.InnerAreaStyle = Janus.Windows.UI.Dock.PanelInnerAreaStyle.Window;
            this.panelCommand.InnerContainer = this.panelCommandContainer;
            this.panelCommand.Location = new System.Drawing.Point(3, 550);
            this.panelCommand.Name = "panelCommand";
            this.panelCommand.Size = new System.Drawing.Size(737, 49);
            this.panelCommand.TabIndex = 4;
            this.panelCommand.Text = "Command";
            // 
            // panelCommandContainer
            // 
            this.panelCommandContainer.Controls.Add(this.btnOk);
            this.panelCommandContainer.Controls.Add(this.btnClose);
            this.panelCommandContainer.Location = new System.Drawing.Point(1, 3);
            this.panelCommandContainer.Name = "panelCommandContainer";
            this.panelCommandContainer.Size = new System.Drawing.Size(735, 45);
            this.panelCommandContainer.TabIndex = 0;
            // 
            // panelMain
            // 
            this.panelMain.CaptionVisible = Janus.Windows.UI.InheritableBoolean.False;
            this.panelMain.GroupStyle = Janus.Windows.UI.Dock.PanelGroupStyle.VerticalTiles;
            this.panelMain.Location = new System.Drawing.Point(3, 68);
            this.panelMain.Name = "panelMain";
            this.panelMain.Size = new System.Drawing.Size(737, 482);
            this.panelMain.TabIndex = 4;
            this.panelMain.Text = "main";
            // 
            // panelChannels
            // 
            this.panelChannels.CaptionVisible = Janus.Windows.UI.InheritableBoolean.False;
            this.panelChannels.Location = new System.Drawing.Point(0, 0);
            this.panelChannels.Name = "panelChannels";
            this.panelChannels.Size = new System.Drawing.Size(360, 482);
            this.panelChannels.TabIndex = 4;
            this.panelChannels.Text = "ä��(�ø���)";
            // 
            // panelChannel
            // 
            this.panelChannel.BorderPanel = Janus.Windows.UI.InheritableBoolean.False;
            this.panelChannel.CaptionVisible = Janus.Windows.UI.InheritableBoolean.False;
            this.panelChannel.InnerContainer = this.panelChannelContainer;
            this.panelChannel.Location = new System.Drawing.Point(0, 0);
            this.panelChannel.Name = "panelChannel";
            this.panelChannel.Size = new System.Drawing.Size(360, 482);
            this.panelChannel.TabIndex = 4;
            this.panelChannel.Text = "ä�θ��";
            // 
            // panelChannelContainer
            // 
            this.panelChannelContainer.Controls.Add(this.grdExChannelNoList);
            this.panelChannelContainer.Location = new System.Drawing.Point(0, 0);
            this.panelChannelContainer.Name = "panelChannelContainer";
            this.panelChannelContainer.Size = new System.Drawing.Size(360, 482);
            this.panelChannelContainer.TabIndex = 0;
            // 
            // panelSerieses
            // 
            this.panelSerieses.CaptionVisible = Janus.Windows.UI.InheritableBoolean.False;
            this.panelSerieses.Location = new System.Drawing.Point(362, 0);
            this.panelSerieses.Name = "panelSerieses";
            this.panelSerieses.Size = new System.Drawing.Size(375, 482);
            this.panelSerieses.TabIndex = 4;
            this.panelSerieses.Text = "�ø���ȸ��";
            // 
            // pannelSeries
            // 
            this.pannelSeries.BorderPanel = Janus.Windows.UI.InheritableBoolean.False;
            this.pannelSeries.CaptionVisible = Janus.Windows.UI.InheritableBoolean.False;
            this.pannelSeries.InnerContainer = this.pannelSeriesContainer;
            this.pannelSeries.Location = new System.Drawing.Point(0, 0);
            this.pannelSeries.Name = "pannelSeries";
            this.pannelSeries.Size = new System.Drawing.Size(375, 482);
            this.pannelSeries.TabIndex = 4;
            this.pannelSeries.Text = "�ø�����";
            // 
            // pannelSeriesContainer
            // 
            this.pannelSeriesContainer.Controls.Add(this.grdExSeries);
            this.pannelSeriesContainer.Location = new System.Drawing.Point(0, 0);
            this.pannelSeriesContainer.Name = "pannelSeriesContainer";
            this.pannelSeriesContainer.Size = new System.Drawing.Size(375, 482);
            this.pannelSeriesContainer.TabIndex = 0;
            // 
            // grdExSeries
            // 
            this.grdExSeries.AllowEdit = Janus.Windows.GridEX.InheritableBoolean.False;
            this.grdExSeries.AlternatingColors = true;
            this.grdExSeries.AutomaticSort = false;
            this.grdExSeries.DataSource = this.dvSeries;
            this.grdExSeries.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdExSeries.EmptyRows = true;
            this.grdExSeries.FocusCellFormatStyle.BackColor = System.Drawing.SystemColors.Highlight;
            this.grdExSeries.FocusCellFormatStyle.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.grdExSeries.FocusStyle = Janus.Windows.GridEX.FocusStyle.None;
            this.grdExSeries.Font = new System.Drawing.Font("���� ���", 8.25F);
            this.grdExSeries.GridLineColor = System.Drawing.Color.Silver;
            this.grdExSeries.GridLines = Janus.Windows.GridEX.GridLines.Vertical;
            this.grdExSeries.GridLineStyle = Janus.Windows.GridEX.GridLineStyle.Solid;
            this.grdExSeries.GroupByBoxVisible = false;
            this.grdExSeries.GroupMode = Janus.Windows.GridEX.GroupMode.Collapsed;
            this.grdExSeries.GroupRowVisualStyle = Janus.Windows.GridEX.GroupRowVisualStyle.Outlook2003;
            this.grdExSeries.HideSelection = Janus.Windows.GridEX.HideSelection.HighlightInactive;
            grdExSeries_Layout_0.DataSource = this.dvSeries;
            grdExSeries_Layout_0.IsCurrentLayout = true;
            grdExSeries_Layout_0.Key = "bae";
            grdExSeries_Layout_0.LayoutString = resources.GetString("grdExSeries_Layout_0.LayoutString");
            this.grdExSeries.Layouts.AddRange(new Janus.Windows.GridEX.GridEXLayout[] {
            grdExSeries_Layout_0});
            this.grdExSeries.Location = new System.Drawing.Point(0, 0);
            this.grdExSeries.Name = "grdExSeries";
            this.grdExSeries.ScrollBars = Janus.Windows.GridEX.ScrollBars.Vertical;
            this.grdExSeries.SelectedInactiveFormatStyle.BackColor = System.Drawing.Color.Gold;
            this.grdExSeries.Size = new System.Drawing.Size(375, 482);
            this.grdExSeries.TabIndex = 1;
            this.grdExSeries.TabKeyBehavior = Janus.Windows.GridEX.TabKeyBehavior.ControlNavigation;
            this.grdExSeries.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            this.grdExSeries.RowDoubleClick += new Janus.Windows.GridEX.RowActionEventHandler(this.grdExSeries_RowDoubleClick);
            // 
            // SeriesPopForm
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(6, 15);
            this.ClientSize = new System.Drawing.Size(743, 602);
            this.Controls.Add(this.panelMain);
            this.Controls.Add(this.panelCommand);
            this.Controls.Add(this.panelTop);
            this.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.Font = new System.Drawing.Font("���� ���", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "SeriesPopForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "ȸ��(�ø���)��ϰ˻�";
            this.Load += new System.EventHandler(this.ChannelNoPopForm1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dvChannelSet)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupDs)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dvSeries)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdExChannelNoList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiPM)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelTop)).EndInit();
            this.panelTop.ResumeLayout(false);
            this.panelTopContainer.ResumeLayout(false);
            this.panelTopContainer.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelCommand)).EndInit();
            this.panelCommand.ResumeLayout(false);
            this.panelCommandContainer.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelMain)).EndInit();
            this.panelMain.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelChannels)).EndInit();
            this.panelChannels.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelChannel)).EndInit();
            this.panelChannel.ResumeLayout(false);
            this.panelChannelContainer.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelSerieses)).EndInit();
            this.panelSerieses.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pannelSeries)).EndInit();
            this.pannelSeries.ResumeLayout(false);
            this.pannelSeriesContainer.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdExSeries)).EndInit();
            this.ResumeLayout(false);

		}
		#endregion

		#region ��Ʈ�� �ε�
		private void ChannelNoPopForm1_Load(object sender, System.EventArgs e)
		{           
			// �����Ͱ����� ��ü����
			dt = ((DataView)grdExChannelNoList.DataSource).Table;  
			cm = (CurrencyManager) this.BindingContext[grdExChannelNoList.DataSource]; 
			cm.PositionChanged += new System.EventHandler(OnGrdRowChanged); 

			// ��Ʈ�� �ʱ�ȭ
			InitControl();
		}
		#endregion

		

		#region ��Ʈ�� �ʱ�ȭ
		private void InitControl()
		{
			//InitCombo();
			InitCombo_Category();
			//InitCombo_Genre();												
		}

		private void InitCombo()
		{			
			MediaCodeModel mediacodeModel = new MediaCodeModel();		
			new MediaCodeManager(systemModel, commonModel).GetMediaCodeList(mediacodeModel);
			
			if (mediacodeModel.ResultCD.Equals("0000"))
			{
				// �����ͼ¿� ����
				Utility.SetDataTable(groupDs.Medias, mediacodeModel.MediaCodeDataSet);				
			}

			// ����ȸ �޺�
			// �������� �޺��� Dataset�� �����ͼҽ��� ������.

			// �˻������� �޺�
			this.cbSearchMediaName.Items.Clear();
			
			// �޺��ڽ��� ��Ʈ�� �ڵ����� ���� Item�迭�� ����
			Janus.Windows.EditControls.UIComboBoxItem[] comboItems = new Janus.Windows.EditControls.UIComboBoxItem[mediacodeModel.ResultCnt];
			
			for(int i=0;i<mediacodeModel.ResultCnt;i++)
			{
				DataRow row = groupDs.Medias.Rows[i];

				string val = row["MediaCode"].ToString();
				string txt = row["MediaName"].ToString();
				comboItems[i] = new Janus.Windows.EditControls.UIComboBoxItem(txt,val);
			}
			// �޺��� ��Ʈ
			this.cbSearchMediaName.Items.AddRange(comboItems);
			this.cbSearchMediaName.SelectedIndex = 1;

			Application.DoEvents();

		}
		

		public void InitCombo_Category()
		{
			// �ڵ忡�� ���ȷ����� ��ȸ�Ѵ�.
			GroupModel groupModel = new GroupModel();


            //��ȿ �޴� üũ�Ǵ� - ī�װ��� �� �����´�.
            if (chkInvalidMenu.Checked)
            {
                groupModel.InvalidYn = true;
            }
            else
            {
                groupModel.InvalidYn = false;
            }

            groupModel.SearchType = "P";
			
            if (IsNewChannelSearchKey) groupModel.SearchKey = "";
            else groupModel.SearchKey = ebChannelSearchKey.Text.Trim();

            new GroupManager(systemModel, commonModel).GetCategoryList2(groupModel);
			
			if (groupModel.ResultCD.Equals("0000"))
			{
				Utility.SetDataTable(groupDs.Category, groupModel.CategoryDataSet);	
			}


			// �ϴ� ��ü �޺��� �� �ʱ�ȭ. 
			this.cbSearchCategoryName.Items.Clear();
            
			// �޺��ڽ��� ��Ʈ�� �ڵ����� ���� Item�迭�� ����
			Janus.Windows.EditControls.UIComboBoxItem[] comboItems = new Janus.Windows.EditControls.UIComboBoxItem[groupModel.ResultCnt];
			for(int i=0;i<groupModel.ResultCnt;i++)
			{
				DataRow row = groupDs.Category.Rows[i];

				string val = row["CategoryCode"].ToString();
				string txt = row["CategoryName"].ToString();
				comboItems[i] = new Janus.Windows.EditControls.UIComboBoxItem(txt,val);
			}

			// �ϴ� �޺��� ��Ʈ
			this.cbSearchCategoryName.Items.AddRange(comboItems);
			if(this.cbSearchCategoryName.Items.Count > 0) 
            {
                this.cbSearchCategoryName.SelectedIndex = 0;
            }
            else
            {
                MessageBox.Show(groupModel.SearchKey + "(��)�� �˻����� �ʾҽ��ϴ�.", "�˻�", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
			
			Application.DoEvents();
		}


		public void InitCombo_Genre()
		{
			// �ڵ忡�� ���ȷ����� ��ȸ�Ѵ�.
			GroupModel groupModel = new GroupModel();
			groupModel.CategoryCode	=	KeyCategory;

            groupModel.SearchType = "P";

            if (IsNewChannelSearchKey) groupModel.SearchKey = "";
            else groupModel.SearchKey = ebChannelSearchKey.Text.Trim();

            //��ȿ �޴� üũ�Ǵ�
            if (chkInvalidMenu.Checked)
            {
                groupModel.InvalidYn = true;
            }
            else
            {
                groupModel.InvalidYn = false;
            }

			new GroupManager(systemModel, commonModel).GetGenreList2(groupModel);
			
			if (groupModel.ResultCD.Equals("0000"))
			{
				// �����ͼ¿� ����
				Utility.SetDataTable(groupDs.Genre, groupModel.GenreDataSet);	
			}

			// �ϴ� ��ü �޺��� �� �ʱ�ȭ. 
			this.cbSearchGenreName.Items.Clear();
            
			// �޺��ڽ��� ��Ʈ�� �ڵ����� ���� Item�迭�� ����
			Janus.Windows.EditControls.UIComboBoxItem[] comboItems = new Janus.Windows.EditControls.UIComboBoxItem[groupModel.ResultCnt + 1];
            
			comboItems[0] = new Janus.Windows.EditControls.UIComboBoxItem("�帣����","00");
			
			for(int i=0;i<groupModel.ResultCnt;i++)
			{
				DataRow row = groupDs.Genre.Rows[i];

				string val = row["GenreCode"].ToString();
				string txt = row["GenreName"].ToString();
				comboItems[i+1] = new Janus.Windows.EditControls.UIComboBoxItem(txt,val);
			}


			// �ϴ� �޺��� ��Ʈ
			this.cbSearchGenreName.Items.AddRange(comboItems);
			if( cbSearchGenreName.Items.Count > 1 )	this.cbSearchGenreName.SelectedIndex = 1;
			else									this.cbSearchGenreName.SelectedIndex = 0;

			Application.DoEvents();

		}

		#endregion
  
		#region ó���޼ҵ�

		/// <summary>
		/// ��������� ��ȸ
		/// </summary>
		private void SearchChannelSet()
		{
			StatusMessage("������ ������ ��ȸ�մϴ�.");

			try
			{				
				groupModel.MediaCode = keyMedia;
				groupModel.CategoryCode = KeyCategory;
				groupModel.GenreCode = KeyGenre;

                if (IsNewChannelSearchKey) groupModel.SearchKey = "";
                else groupModel.SearchKey = ebChannelSearchKey.Text.Trim();

				// �����������ȸ ���񽺸� ȣ���Ѵ�.
				new GroupManager(systemModel,commonModel).GetChannelNoPopList2(groupModel);

				if (groupModel.ResultCD.Equals("0000"))
				{
					Utility.SetDataTable(groupDs.Channel, groupModel.ChannelDataSet);				
					StatusMessage(groupModel.ResultCnt + "���� ������ ������ ��ȸ�Ǿ����ϴ�.");
				}
				OnGrdRowChanged(null,null);
			}
			catch(FrameException fe)
			{
				FrameSystem.showMsgForm("��������ȸ����", new string[] {fe.ErrCode, fe.ResultMsg});
			}
			catch(Exception ex)
			{
				FrameSystem.showMsgForm("��������ȸ����",new string[] {"",ex.Message});
			}
		}

		private void SearchSeries()
		{
			StatusMessage("ȸ�������� ��ȸ�մϴ�.");

			try
			{				
				groupModel.Init();
				groupModel.MediaCode	= keyMedia;
				groupModel.CategoryCode = KeyCategory;
				groupModel.GenreCode	= KeyGenre;
				groupModel.ChannelNo	= keyChannel;
				groupDs.Series.Clear();

//                if (IsNewSeriesSearchKey) groupModel.SearchKey = "";
//                else groupModel.SearchKey = ebSeriesSearchKey.Text.Trim();

				// �����������ȸ ���񽺸� ȣ���Ѵ�.
				new GroupManager(systemModel,commonModel).GetSeriesList2(groupModel);

				if (groupModel.ResultCD.Equals("0000"))
				{
					Utility.SetDataTable(groupDs.Series, groupModel.SeriesDataSet);				
					StatusMessage(groupModel.ResultCnt + "���� ȸ�� ������ ��ȸ�Ǿ����ϴ�.");
				}
			}
			catch(FrameException fe)
			{
				FrameSystem.showMsgForm("��������ȸ����", new string[] {fe.ErrCode, fe.ResultMsg});
			}
			catch(Exception ex)
			{
				FrameSystem.showMsgForm("��������ȸ����",new string[] {"",ex.Message});
			}
		}
		#endregion

		#region �̺�Ʈ�Լ�
        
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

		private void btnOk_Click(object sender, System.EventArgs e)
		{
			

            ProgressStart();
            
            try
            {
                int rc = 0;
                string newKey = "";

                for (int i = 0; i < grdExSeries.RowCount; i++)
                {

                    if (grdExSeries.GetRows()[i].Cells["CheckYn"].Value.ToString().Equals("True"))
                    {
                        newKey = grdExSeries.GetRows()[i].Cells["SeriesNo"].Value.ToString();

                        if (!this.ChannelSetCtl.SeriesNo(KeyCategory, KeyGenre, keyChannel, newKey))
                            return;
                        
                        rc++;
                    }
                }

                if (rc > 0)
                    MessageBox.Show("�ø���[" + keyChannel + "] " + keyChannelNm + " �� [" + rc + "]���� ȸ�� ��� �Ϸ�", "�ø���ȸ�� ���", MessageBoxButtons.OK, MessageBoxIcon.Information);
                else return;

                ProgressStop();

            }
            catch (Exception ex)
            {
                FrameSystem.showMsgForm("�ø���ȸ�� ��� ����", new string[] { "", ex.Message });
            }		
			finally
			{
				ProgressStop();
			}
			//this.ChannelSetCtl.ChannelNo = newKey;				
			this.Close();
		}
     


		private void btnClose_Click(object sender, System.EventArgs e)
		{
			this.Close();
		}


		/// <summary>
		/// ī�װ��� ����ɴ�
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void cbSearchCategoryName_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			keyMedia	= "1";	//cbSearchMediaName.SelectedItem.Value.ToString();
			KeyCategory	= cbSearchCategoryName.SelectedItem.Value.ToString();
			InitCombo_Genre();
		}

		private void cbSearchGenreName_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			KeyGenre	= cbSearchGenreName.SelectedItem.Value.ToString();
			SearchChannelSet();
		}		

		private void OnGrdRowChanged(object sender, EventArgs e)
		{
			try
			{
				this.Cursor = Cursors.WaitCursor;

				int curRow = cm.Position;
				if(curRow >= 0 && dt.Rows.Count > 0 )
				{
					keyChannel	= dt.Rows[curRow]["ChannelNo"].ToString();
					keyChannelNm= dt.Rows[curRow]["Title"].ToString();
					pannelSeries.Text	= keyChannelNm;
					this.SearchSeries();
				}
			}
			catch(Exception ex)
			{
				MessageBox.Show("[" + keyChannel + "] " + keyChannelNm + "�����߻�\n\n" + ex.Message, "�׷��",MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
			finally
			{
				this.Cursor = Cursors.Default;
			}
		}

		/// <summary>
		/// �ø���׸����� Row�� ����Ŭ��������
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void grdExSeries_RowDoubleClick(object sender, Janus.Windows.GridEX.RowActionEventArgs e)
		{
			string newKey	= grdExSeries.SelectedItems[0].GetRow().Cells["SeriesNo"].Value.ToString();			
			if ( this.ChannelSetCtl.SeriesNo( KeyCategory, KeyGenre, keyChannel, newKey) )
			{
				MessageBox.Show("[" + keyChannel + "] " + keyChannelNm + " �� " + newKey + "ȸ�� ��� �Ϸ�", "�׷��",MessageBoxButtons.OK, MessageBoxIcon.Information);
			}
		}

        /// <summary>
        /// �˻��� ����
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// 
        private void btnChannelSearch_Click(object sender, System.EventArgs e)
        {

            InitCombo_Category();

    //        InitCombo_Genre();

    //        SearchChannelSet();
        }

        /// <summary>
        /// �˻��� ����
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// 
        private void ebChannelSearchKey_TextChanged(object sender, EventArgs e)
        {
            IsNewChannelSearchKey = false;
        }

        /// <summary>
        /// �˻��� Ŭ��
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// 
        private void ebChannelSearchKey_Click(object sender, EventArgs e)
        {
            if (IsNewChannelSearchKey)
            {
                ebChannelSearchKey.Text = "";
            }
            else
            {
                ebChannelSearchKey.SelectAll();
            }
        }

        private void btnSeriesSearch_Click(object sender, EventArgs e)
        {
            SearchSeries();
        }



        private void chkInvalidMenu_CheckedChanged(object sender, EventArgs e)
        {
            InitCombo_Category();
        }

        private void ebChannelSearchKey_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                InitCombo_Category();
            }
        }

        private void ebSeriesSearchKey_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SearchSeries();
            }
        }


	}
	#endregion


}
