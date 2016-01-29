using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.Diagnostics;
using System.Threading;

using WinFramework.Misc;
using WinFramework.Base;
using WinFramework.Data;
using AdManagerModel;

namespace AdManagerClient._30_Schedule
{
	/// <summary>
	/// Sch3Form�� ���� ��� �����Դϴ�.
	/// </summary>
	public class Sch3Form : System.Windows.Forms.Form
	{
		#region ȭ�� ������Ʈ, ������, �Ҹ���
		private Janus.Windows.UI.Dock.UIPanelManager uiPM;
		private Janus.Windows.UI.Dock.UIPanelGroup uiPanelSchGroup;
		private Janus.Windows.UI.Dock.UIPanel uiPanelBottom;
		private Janus.Windows.UI.Dock.UIPanelInnerContainer uiPanelBottomContainer;
		private Janus.Windows.UI.Dock.UIPanel uiPanelGenre;
		private Janus.Windows.UI.Dock.UIPanelInnerContainer uiPanelGenreContainer;
		private Janus.Windows.UI.Dock.UIPanel uiPanelChannel;
		private Janus.Windows.UI.Dock.UIPanelInnerContainer uiPanelChannelContainer;
		private Janus.Windows.UI.Dock.UIPanel uiPanelSeries;
		private Janus.Windows.UI.Dock.UIPanelInnerContainer uiPanelSeriesContainer;
		private Janus.Windows.GridEX.GridEX grdExGenre;
		private Janus.Windows.GridEX.GridEX grdExChannel;
		private AdManagerClient._30_Schedule.Sch3FormDs sch3FormDs;
		private System.Data.DataView dvGenre;
		private System.Data.DataView	dvChannel;
		private System.Data.DataView dvSeries;
		private Janus.Windows.GridEX.GridEX grdExSeries;
		private System.ComponentModel.IContainer components;
		#endregion

		#region ��������� ��ü �� ����

		// �ý��� ���� : ȭ�����
		private SystemModel   systemModel   = FrameSystem.oSysModel;
		private CommonModel   commonModel   = FrameSystem.oComModel;
		private Logger        log           = FrameSystem.oLog;

		// ����� ������
		private	Sch3FormModel	mainModel	= new Sch3FormModel();

		// ȭ��ó���� ����
		private	CurrencyManager	cmGenre		= null;
		private	CurrencyManager	cmChannel	= null;
		private	CurrencyManager	cmSeries	= null;
		
		private	DataTable       dtGenre		= null;
		private	DataTable		dtChannel	= null;
		private	DataTable		dtSeries	= null;

		//
		private	int		keyGenreFound1	= 0;
		private	int		keyGenreFound2	= 0;
		private	int		keyGenreFound3	= 0;

		private	int		keyChannelFound1	= 0;
		private	int		keyChannelFound2	= 0;
		private	int		keyChannelFound3	= 0;

		private	int		keySeriesFound1	= 0;
		private	int		keySeriesFound2	= 0;
		private	int		keySeriesFound3	= 0;

		private	int		keySearchType	= 0;

		private	int		keyItemNo		= 0;
		private	string	keyItemName		= "";
		private	int		keyMedia		= 0;
		private	int		keyCategory		= 0;
		private	int		keyGenre		= 0;
		private	string	keyGenreName	= "";
		private	int		keyChannel		= 0;
		private Janus.Windows.EditControls.UIButton btnClose;
		private Janus.Windows.EditControls.UIButton btnOk;
		private Janus.Windows.EditControls.UIButton btnSearch;
		private	string	keyChannelName	= "";
		#endregion

		#region ������ �� �Ҹ��� ��Ÿ �ڵ�
		public Sch3Form()
		{
			InitializeComponent();
		}

		/// <summary>
		/// ��� ���� ��� ���ҽ��� �����մϴ�.
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

		#region Windows Form �����̳ʿ��� ������ �ڵ�
		/// <summary>
		/// �����̳� ������ �ʿ��� �޼����Դϴ�.
		/// �� �޼����� ������ �ڵ� ������� �������� ���ʽÿ�.
		/// </summary>
		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            Janus.Windows.GridEX.GridEXLayout grdExGenre_DesignTimeLayout = new Janus.Windows.GridEX.GridEXLayout();
            Janus.Windows.Common.Layouts.JanusLayoutReference grdExGenre_DesignTimeLayout_Reference_0 = new Janus.Windows.Common.Layouts.JanusLayoutReference("GridEXLayoutData.RootTable.FormatConditions.Condition0.FormatStyle.BackgroundImag" +
                    "e");
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Sch3Form));
            Janus.Windows.Common.Layouts.JanusLayoutReference grdExGenre_DesignTimeLayout_Reference_1 = new Janus.Windows.Common.Layouts.JanusLayoutReference("GridEXLayoutData.RootTable.FormatConditions.Condition1.FormatStyle.BackgroundImag" +
                    "e");
            Janus.Windows.Common.Layouts.JanusLayoutReference grdExGenre_DesignTimeLayout_Reference_2 = new Janus.Windows.Common.Layouts.JanusLayoutReference("GridEXLayoutData.RootTable.FormatConditions.Condition2.FormatStyle.BackgroundImag" +
                    "e");
            Janus.Windows.GridEX.GridEXLayout grdExGenre_Layout_0 = new Janus.Windows.GridEX.GridEXLayout();
            Janus.Windows.GridEX.GridEXLayout grdExChannel_DesignTimeLayout = new Janus.Windows.GridEX.GridEXLayout();
            Janus.Windows.Common.Layouts.JanusLayoutReference grdExChannel_DesignTimeLayout_Reference_0 = new Janus.Windows.Common.Layouts.JanusLayoutReference("GridEXLayoutData.RootTable.FormatConditions.Condition0.FormatStyle.BackgroundImag" +
                    "e");
            Janus.Windows.Common.Layouts.JanusLayoutReference grdExChannel_DesignTimeLayout_Reference_1 = new Janus.Windows.Common.Layouts.JanusLayoutReference("GridEXLayoutData.RootTable.FormatConditions.Condition1.FormatStyle.BackgroundImag" +
                    "e");
            Janus.Windows.Common.Layouts.JanusLayoutReference grdExChannel_DesignTimeLayout_Reference_2 = new Janus.Windows.Common.Layouts.JanusLayoutReference("GridEXLayoutData.RootTable.FormatConditions.Condition2.FormatStyle.BackgroundImag" +
                    "e");
            Janus.Windows.Common.Layouts.JanusLayoutReference grdExChannel_DesignTimeLayout_Reference_3 = new Janus.Windows.Common.Layouts.JanusLayoutReference("GridEXLayoutData.RootTable.FormatConditions.Condition4.FormatStyle.BackgroundImag" +
                    "e");
            Janus.Windows.Common.Layouts.JanusLayoutReference grdExChannel_DesignTimeLayout_Reference_4 = new Janus.Windows.Common.Layouts.JanusLayoutReference("GridEXLayoutData.RootTable.FormatConditions.Condition5.FormatStyle.BackgroundImag" +
                    "e");
            Janus.Windows.GridEX.GridEXLayout grdExChannel_Layout_0 = new Janus.Windows.GridEX.GridEXLayout();
            Janus.Windows.GridEX.GridEXLayout grdExSeries_DesignTimeLayout = new Janus.Windows.GridEX.GridEXLayout();
            Janus.Windows.Common.Layouts.JanusLayoutReference grdExSeries_DesignTimeLayout_Reference_0 = new Janus.Windows.Common.Layouts.JanusLayoutReference("GridEXLayoutData.RootTable.FormatConditions.Condition0.FormatStyle.BackgroundImag" +
                    "e");
            Janus.Windows.Common.Layouts.JanusLayoutReference grdExSeries_DesignTimeLayout_Reference_1 = new Janus.Windows.Common.Layouts.JanusLayoutReference("GridEXLayoutData.RootTable.FormatConditions.Condition1.FormatStyle.BackgroundImag" +
                    "e");
            Janus.Windows.Common.Layouts.JanusLayoutReference grdExSeries_DesignTimeLayout_Reference_2 = new Janus.Windows.Common.Layouts.JanusLayoutReference("GridEXLayoutData.RootTable.FormatConditions.Condition2.FormatStyle.BackgroundImag" +
                    "e");
            Janus.Windows.Common.Layouts.JanusLayoutReference grdExSeries_DesignTimeLayout_Reference_3 = new Janus.Windows.Common.Layouts.JanusLayoutReference("GridEXLayoutData.RootTable.FormatConditions.Condition3.FormatStyle.BackgroundImag" +
                    "e");
            Janus.Windows.GridEX.GridEXLayout grdExSeries_Layout_0 = new Janus.Windows.GridEX.GridEXLayout();
            this.uiPM = new Janus.Windows.UI.Dock.UIPanelManager(this.components);
            this.uiPanelBottom = new Janus.Windows.UI.Dock.UIPanel();
            this.uiPanelBottomContainer = new Janus.Windows.UI.Dock.UIPanelInnerContainer();
            this.btnSearch = new Janus.Windows.EditControls.UIButton();
            this.btnClose = new Janus.Windows.EditControls.UIButton();
            this.btnOk = new Janus.Windows.EditControls.UIButton();
            this.uiPanelSchGroup = new Janus.Windows.UI.Dock.UIPanelGroup();
            this.uiPanelGenre = new Janus.Windows.UI.Dock.UIPanel();
            this.uiPanelGenreContainer = new Janus.Windows.UI.Dock.UIPanelInnerContainer();
            this.grdExGenre = new Janus.Windows.GridEX.GridEX();
            this.dvGenre = new System.Data.DataView();
            this.sch3FormDs = new AdManagerClient._30_Schedule.Sch3FormDs();
            this.uiPanelChannel = new Janus.Windows.UI.Dock.UIPanel();
            this.uiPanelChannelContainer = new Janus.Windows.UI.Dock.UIPanelInnerContainer();
            this.grdExChannel = new Janus.Windows.GridEX.GridEX();
            this.dvChannel = new System.Data.DataView();
            this.uiPanelSeries = new Janus.Windows.UI.Dock.UIPanel();
            this.uiPanelSeriesContainer = new Janus.Windows.UI.Dock.UIPanelInnerContainer();
            this.grdExSeries = new Janus.Windows.GridEX.GridEX();
            this.dvSeries = new System.Data.DataView();
            ((System.ComponentModel.ISupportInitialize)(this.uiPM)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelBottom)).BeginInit();
            this.uiPanelBottom.SuspendLayout();
            this.uiPanelBottomContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelSchGroup)).BeginInit();
            this.uiPanelSchGroup.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelGenre)).BeginInit();
            this.uiPanelGenre.SuspendLayout();
            this.uiPanelGenreContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdExGenre)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dvGenre)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.sch3FormDs)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelChannel)).BeginInit();
            this.uiPanelChannel.SuspendLayout();
            this.uiPanelChannelContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdExChannel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dvChannel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelSeries)).BeginInit();
            this.uiPanelSeries.SuspendLayout();
            this.uiPanelSeriesContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdExSeries)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dvSeries)).BeginInit();
            this.SuspendLayout();
            // 
            // uiPM
            // 
            this.uiPM.BackColorGradientAutoHideStrip = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(218)))), ((int)(((byte)(250)))));
            this.uiPM.ContainerControl = this;
            this.uiPM.SplitterSize = 2;
            this.uiPM.VisualStyle = Janus.Windows.UI.Dock.PanelVisualStyle.Office2007;
            this.uiPanelBottom.Id = new System.Guid("2426be36-d1e4-41c8-a8c9-f5b063e6ac3b");
            this.uiPM.Panels.Add(this.uiPanelBottom);
            this.uiPanelSchGroup.Id = new System.Guid("ee478995-103f-435d-9369-beed1b29feac");
            this.uiPanelSchGroup.StaticGroup = true;
            this.uiPanelGenre.Id = new System.Guid("b029d617-3054-4de7-ab5c-c3a887b15db6");
            this.uiPanelSchGroup.Panels.Add(this.uiPanelGenre);
            this.uiPanelChannel.Id = new System.Guid("e41b02af-16fe-49c7-a179-4dd20b1843b0");
            this.uiPanelSchGroup.Panels.Add(this.uiPanelChannel);
            this.uiPanelSeries.Id = new System.Guid("6443ffa1-37cd-440a-86f8-9ddc12111282");
            this.uiPanelSchGroup.Panels.Add(this.uiPanelSeries);
            this.uiPM.Panels.Add(this.uiPanelSchGroup);
            // 
            // Design Time Panel Info:
            // 
            this.uiPM.BeginPanelInfo();
            this.uiPM.AddDockPanelInfo(new System.Guid("ee478995-103f-435d-9369-beed1b29feac"), Janus.Windows.UI.Dock.PanelGroupStyle.VerticalTiles, Janus.Windows.UI.Dock.PanelDockStyle.Fill, true, new System.Drawing.Size(1228, 695), true);
            this.uiPM.AddDockPanelInfo(new System.Guid("b029d617-3054-4de7-ab5c-c3a887b15db6"), new System.Guid("ee478995-103f-435d-9369-beed1b29feac"), 343, true);
            this.uiPM.AddDockPanelInfo(new System.Guid("e41b02af-16fe-49c7-a179-4dd20b1843b0"), new System.Guid("ee478995-103f-435d-9369-beed1b29feac"), 474, true);
            this.uiPM.AddDockPanelInfo(new System.Guid("6443ffa1-37cd-440a-86f8-9ddc12111282"), new System.Guid("ee478995-103f-435d-9369-beed1b29feac"), 407, true);
            this.uiPM.AddDockPanelInfo(new System.Guid("2426be36-d1e4-41c8-a8c9-f5b063e6ac3b"), Janus.Windows.UI.Dock.PanelDockStyle.Bottom, new System.Drawing.Size(1228, 40), true);
            this.uiPM.AddFloatingPanelInfo(new System.Guid("2426be36-d1e4-41c8-a8c9-f5b063e6ac3b"), new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
            this.uiPM.AddFloatingPanelInfo(new System.Guid("ee478995-103f-435d-9369-beed1b29feac"), Janus.Windows.UI.Dock.PanelGroupStyle.VerticalTiles, true, new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
            this.uiPM.AddFloatingPanelInfo(new System.Guid("b029d617-3054-4de7-ab5c-c3a887b15db6"), new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
            this.uiPM.AddFloatingPanelInfo(new System.Guid("e41b02af-16fe-49c7-a179-4dd20b1843b0"), new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
            this.uiPM.AddFloatingPanelInfo(new System.Guid("6443ffa1-37cd-440a-86f8-9ddc12111282"), new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
            this.uiPM.EndPanelInfo();
            // 
            // uiPanelBottom
            // 
            this.uiPanelBottom.ActiveCaptionMode = Janus.Windows.UI.Dock.ActiveCaptionMode.Never;
            this.uiPanelBottom.CaptionVisible = Janus.Windows.UI.InheritableBoolean.False;
            this.uiPanelBottom.CloseButtonVisible = Janus.Windows.UI.InheritableBoolean.False;
            this.uiPanelBottom.InnerAreaStyle = Janus.Windows.UI.Dock.PanelInnerAreaStyle.Window;
            this.uiPanelBottom.InnerContainer = this.uiPanelBottomContainer;
            this.uiPanelBottom.Location = new System.Drawing.Point(3, 698);
            this.uiPanelBottom.Name = "uiPanelBottom";
            this.uiPanelBottom.Size = new System.Drawing.Size(1228, 40);
            this.uiPanelBottom.TabIndex = 4;
            this.uiPanelBottom.Text = "Panel 1";
            // 
            // uiPanelBottomContainer
            // 
            this.uiPanelBottomContainer.Controls.Add(this.btnSearch);
            this.uiPanelBottomContainer.Controls.Add(this.btnClose);
            this.uiPanelBottomContainer.Controls.Add(this.btnOk);
            this.uiPanelBottomContainer.Location = new System.Drawing.Point(1, 3);
            this.uiPanelBottomContainer.Name = "uiPanelBottomContainer";
            this.uiPanelBottomContainer.Size = new System.Drawing.Size(1226, 36);
            this.uiPanelBottomContainer.TabIndex = 0;
            // 
            // btnSearch
            // 
            this.btnSearch.BackColor = System.Drawing.SystemColors.Control;
            this.btnSearch.Location = new System.Drawing.Point(10, 7);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(104, 23);
            this.btnSearch.TabIndex = 12;
            this.btnSearch.Text = "�� ȸ";
            this.btnSearch.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // btnClose
            // 
            this.btnClose.BackColor = System.Drawing.SystemColors.Control;
            this.btnClose.Location = new System.Drawing.Point(597, 7);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(104, 23);
            this.btnClose.TabIndex = 11;
            this.btnClose.Text = "�� ��";
            this.btnClose.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnOk
            // 
            this.btnOk.BackColor = System.Drawing.SystemColors.Control;
            this.btnOk.Location = new System.Drawing.Point(485, 7);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(104, 23);
            this.btnOk.TabIndex = 10;
            this.btnOk.Text = "�� ��";
            this.btnOk.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // uiPanelSchGroup
            // 
            this.uiPanelSchGroup.CaptionVisible = Janus.Windows.UI.InheritableBoolean.False;
            this.uiPanelSchGroup.GroupStyle = Janus.Windows.UI.Dock.PanelGroupStyle.VerticalTiles;
            this.uiPanelSchGroup.Location = new System.Drawing.Point(3, 3);
            this.uiPanelSchGroup.Name = "uiPanelSchGroup";
            this.uiPanelSchGroup.Size = new System.Drawing.Size(1228, 695);
            this.uiPanelSchGroup.TabIndex = 4;
            this.uiPanelSchGroup.Text = "Panel 0";
            // 
            // uiPanelGenre
            // 
            this.uiPanelGenre.CaptionStyle = Janus.Windows.UI.Dock.PanelCaptionStyle.Light;
            this.uiPanelGenre.InnerContainer = this.uiPanelGenreContainer;
            this.uiPanelGenre.Location = new System.Drawing.Point(0, 0);
            this.uiPanelGenre.Name = "uiPanelGenre";
            this.uiPanelGenre.Size = new System.Drawing.Size(343, 695);
            this.uiPanelGenre.TabIndex = 4;
            this.uiPanelGenre.Text = "�帣";
            // 
            // uiPanelGenreContainer
            // 
            this.uiPanelGenreContainer.Controls.Add(this.grdExGenre);
            this.uiPanelGenreContainer.Location = new System.Drawing.Point(1, 23);
            this.uiPanelGenreContainer.Name = "uiPanelGenreContainer";
            this.uiPanelGenreContainer.Size = new System.Drawing.Size(341, 671);
            this.uiPanelGenreContainer.TabIndex = 0;
            // 
            // grdExGenre
            // 
            this.grdExGenre.AlternatingColors = true;
            this.grdExGenre.AlternatingRowFormatStyle.BackgroundImageDrawMode = Janus.Windows.GridEX.BackgroundImageDrawMode.Tile;
            this.grdExGenre.AutomaticSort = false;
            this.grdExGenre.BorderStyle = Janus.Windows.GridEX.BorderStyle.None;
            this.grdExGenre.BuiltInTextsData = "<LocalizableData ID=\"LocalizableStrings\" Collection=\"true\"><GroupByBoxInfo>�׷����� ��" +
                "���� �巹�� �ؼ� �÷������� �˴ϴ�</GroupByBoxInfo></LocalizableData>";
            this.grdExGenre.DataSource = this.dvGenre;
            grdExGenre_DesignTimeLayout_Reference_0.Instance = ((object)(resources.GetObject("grdExGenre_DesignTimeLayout_Reference_0.Instance")));
            grdExGenre_DesignTimeLayout_Reference_1.Instance = ((object)(resources.GetObject("grdExGenre_DesignTimeLayout_Reference_1.Instance")));
            grdExGenre_DesignTimeLayout_Reference_2.Instance = ((object)(resources.GetObject("grdExGenre_DesignTimeLayout_Reference_2.Instance")));
            grdExGenre_DesignTimeLayout.LayoutReferences.AddRange(new Janus.Windows.Common.Layouts.JanusLayoutReference[] {
            grdExGenre_DesignTimeLayout_Reference_0,
            grdExGenre_DesignTimeLayout_Reference_1,
            grdExGenre_DesignTimeLayout_Reference_2});
            grdExGenre_DesignTimeLayout.LayoutString = resources.GetString("grdExGenre_DesignTimeLayout.LayoutString");
            this.grdExGenre.DesignTimeLayout = grdExGenre_DesignTimeLayout;
            this.grdExGenre.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdExGenre.EditorsControlStyle.ButtonAppearance = Janus.Windows.GridEX.ButtonAppearance.Regular;
            this.grdExGenre.EmptyRows = true;
            this.grdExGenre.FocusCellFormatStyle.BackColor = System.Drawing.SystemColors.Highlight;
            this.grdExGenre.FocusCellFormatStyle.ForeColor = System.Drawing.Color.White;
            this.grdExGenre.FocusStyle = Janus.Windows.GridEX.FocusStyle.None;
            this.grdExGenre.Font = new System.Drawing.Font("���� ���", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.grdExGenre.FrozenColumns = 2;
            this.grdExGenre.GridLineColor = System.Drawing.Color.Silver;
            this.grdExGenre.GridLines = Janus.Windows.GridEX.GridLines.Vertical;
            this.grdExGenre.GridLineStyle = Janus.Windows.GridEX.GridLineStyle.Solid;
            this.grdExGenre.GroupByBoxVisible = false;
            this.grdExGenre.GroupMode = Janus.Windows.GridEX.GroupMode.Collapsed;
            this.grdExGenre.GroupRowVisualStyle = Janus.Windows.GridEX.GroupRowVisualStyle.Outlook2003;
            this.grdExGenre.HeaderFormatStyle.TextAlignment = Janus.Windows.GridEX.TextAlignment.Center;
            this.grdExGenre.HideSelection = Janus.Windows.GridEX.HideSelection.HighlightInactive;
            grdExGenre_Layout_0.Key = "bea";
            this.grdExGenre.Layouts.AddRange(new Janus.Windows.GridEX.GridEXLayout[] {
            grdExGenre_Layout_0});
            this.grdExGenre.Location = new System.Drawing.Point(0, 0);
            this.grdExGenre.Name = "grdExGenre";
            this.grdExGenre.ScrollBars = Janus.Windows.GridEX.ScrollBars.Vertical;
            this.grdExGenre.SelectedInactiveFormatStyle.BackColor = System.Drawing.Color.Gold;
            this.grdExGenre.Size = new System.Drawing.Size(341, 671);
            this.grdExGenre.TabIndex = 4;
            this.grdExGenre.TabKeyBehavior = Janus.Windows.GridEX.TabKeyBehavior.ControlNavigation;
            this.grdExGenre.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            this.grdExGenre.UpdatingCell += new Janus.Windows.GridEX.UpdatingCellEventHandler(this.grdExGenre_UpdatingCell);
            // 
            // dvGenre
            // 
            this.dvGenre.Table = this.sch3FormDs.Genre;
            // 
            // sch3FormDs
            // 
            this.sch3FormDs.DataSetName = "Sch3FormDs";
            this.sch3FormDs.Locale = new System.Globalization.CultureInfo("en-US");
            this.sch3FormDs.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // uiPanelChannel
            // 
            this.uiPanelChannel.CaptionStyle = Janus.Windows.UI.Dock.PanelCaptionStyle.Light;
            this.uiPanelChannel.InnerContainer = this.uiPanelChannelContainer;
            this.uiPanelChannel.Location = new System.Drawing.Point(345, 0);
            this.uiPanelChannel.Name = "uiPanelChannel";
            this.uiPanelChannel.Size = new System.Drawing.Size(474, 695);
            this.uiPanelChannel.TabIndex = 4;
            this.uiPanelChannel.Text = "ä��";
            // 
            // uiPanelChannelContainer
            // 
            this.uiPanelChannelContainer.Controls.Add(this.grdExChannel);
            this.uiPanelChannelContainer.Location = new System.Drawing.Point(1, 23);
            this.uiPanelChannelContainer.Name = "uiPanelChannelContainer";
            this.uiPanelChannelContainer.Size = new System.Drawing.Size(472, 671);
            this.uiPanelChannelContainer.TabIndex = 0;
            // 
            // grdExChannel
            // 
            this.grdExChannel.AlternatingColors = true;
            this.grdExChannel.AlternatingRowFormatStyle.BackgroundImageDrawMode = Janus.Windows.GridEX.BackgroundImageDrawMode.Tile;
            this.grdExChannel.BorderStyle = Janus.Windows.GridEX.BorderStyle.None;
            this.grdExChannel.BuiltInTextsData = "<LocalizableData ID=\"LocalizableStrings\" Collection=\"true\"><GroupByBoxInfo>�׷����� ��" +
                "���� �巹�� �ؼ� �÷������� �˴ϴ�</GroupByBoxInfo></LocalizableData>";
            this.grdExChannel.DataSource = this.dvChannel;
            grdExChannel_DesignTimeLayout_Reference_0.Instance = ((object)(resources.GetObject("grdExChannel_DesignTimeLayout_Reference_0.Instance")));
            grdExChannel_DesignTimeLayout_Reference_1.Instance = ((object)(resources.GetObject("grdExChannel_DesignTimeLayout_Reference_1.Instance")));
            grdExChannel_DesignTimeLayout_Reference_2.Instance = ((object)(resources.GetObject("grdExChannel_DesignTimeLayout_Reference_2.Instance")));
            grdExChannel_DesignTimeLayout_Reference_3.Instance = ((object)(resources.GetObject("grdExChannel_DesignTimeLayout_Reference_3.Instance")));
            grdExChannel_DesignTimeLayout_Reference_4.Instance = ((object)(resources.GetObject("grdExChannel_DesignTimeLayout_Reference_4.Instance")));
            grdExChannel_DesignTimeLayout.LayoutReferences.AddRange(new Janus.Windows.Common.Layouts.JanusLayoutReference[] {
            grdExChannel_DesignTimeLayout_Reference_0,
            grdExChannel_DesignTimeLayout_Reference_1,
            grdExChannel_DesignTimeLayout_Reference_2,
            grdExChannel_DesignTimeLayout_Reference_3,
            grdExChannel_DesignTimeLayout_Reference_4});
            grdExChannel_DesignTimeLayout.LayoutString = resources.GetString("grdExChannel_DesignTimeLayout.LayoutString");
            this.grdExChannel.DesignTimeLayout = grdExChannel_DesignTimeLayout;
            this.grdExChannel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdExChannel.EditorsControlStyle.ButtonAppearance = Janus.Windows.GridEX.ButtonAppearance.Regular;
            this.grdExChannel.EmptyRows = true;
            this.grdExChannel.FocusCellFormatStyle.BackColor = System.Drawing.SystemColors.Highlight;
            this.grdExChannel.FocusCellFormatStyle.ForeColor = System.Drawing.Color.White;
            this.grdExChannel.FocusStyle = Janus.Windows.GridEX.FocusStyle.None;
            this.grdExChannel.Font = new System.Drawing.Font("���� ���", 8.25F);
            this.grdExChannel.FrozenColumns = 2;
            this.grdExChannel.GridLineColor = System.Drawing.Color.Silver;
            this.grdExChannel.GridLines = Janus.Windows.GridEX.GridLines.Vertical;
            this.grdExChannel.GridLineStyle = Janus.Windows.GridEX.GridLineStyle.Solid;
            this.grdExChannel.GroupByBoxVisible = false;
            this.grdExChannel.GroupMode = Janus.Windows.GridEX.GroupMode.Collapsed;
            this.grdExChannel.GroupRowVisualStyle = Janus.Windows.GridEX.GroupRowVisualStyle.Outlook2003;
            this.grdExChannel.HeaderFormatStyle.TextAlignment = Janus.Windows.GridEX.TextAlignment.Center;
            this.grdExChannel.HideSelection = Janus.Windows.GridEX.HideSelection.HighlightInactive;
            grdExChannel_Layout_0.Key = "bea";
            this.grdExChannel.Layouts.AddRange(new Janus.Windows.GridEX.GridEXLayout[] {
            grdExChannel_Layout_0});
            this.grdExChannel.Location = new System.Drawing.Point(0, 0);
            this.grdExChannel.Name = "grdExChannel";
            this.grdExChannel.ScrollBars = Janus.Windows.GridEX.ScrollBars.Vertical;
            this.grdExChannel.SelectedInactiveFormatStyle.BackColor = System.Drawing.Color.Gold;
            this.grdExChannel.Size = new System.Drawing.Size(472, 671);
            this.grdExChannel.TabIndex = 5;
            this.grdExChannel.TabKeyBehavior = Janus.Windows.GridEX.TabKeyBehavior.ControlNavigation;
            this.grdExChannel.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            this.grdExChannel.UpdatingCell += new Janus.Windows.GridEX.UpdatingCellEventHandler(this.grdExChannel_UpdatingCell);
            // 
            // dvChannel
            // 
            this.dvChannel.Table = this.sch3FormDs.ChannelSets;
            // 
            // uiPanelSeries
            // 
            this.uiPanelSeries.CaptionStyle = Janus.Windows.UI.Dock.PanelCaptionStyle.Light;
            this.uiPanelSeries.InnerContainer = this.uiPanelSeriesContainer;
            this.uiPanelSeries.Location = new System.Drawing.Point(821, 0);
            this.uiPanelSeries.Name = "uiPanelSeries";
            this.uiPanelSeries.Size = new System.Drawing.Size(407, 695);
            this.uiPanelSeries.TabIndex = 4;
            this.uiPanelSeries.Text = "ȸ��";
            // 
            // uiPanelSeriesContainer
            // 
            this.uiPanelSeriesContainer.Controls.Add(this.grdExSeries);
            this.uiPanelSeriesContainer.Location = new System.Drawing.Point(1, 23);
            this.uiPanelSeriesContainer.Name = "uiPanelSeriesContainer";
            this.uiPanelSeriesContainer.Size = new System.Drawing.Size(405, 671);
            this.uiPanelSeriesContainer.TabIndex = 0;
            // 
            // grdExSeries
            // 
            this.grdExSeries.AlternatingColors = true;
            this.grdExSeries.AlternatingRowFormatStyle.BackgroundImageDrawMode = Janus.Windows.GridEX.BackgroundImageDrawMode.Tile;
            this.grdExSeries.AutomaticSort = false;
            this.grdExSeries.BorderStyle = Janus.Windows.GridEX.BorderStyle.None;
            this.grdExSeries.BuiltInTextsData = "<LocalizableData ID=\"LocalizableStrings\" Collection=\"true\"><GroupByBoxInfo>�׷����� ��" +
                "���� �巹�� �ؼ� �÷������� �˴ϴ�</GroupByBoxInfo></LocalizableData>";
            this.grdExSeries.DataSource = this.dvSeries;
            grdExSeries_DesignTimeLayout_Reference_0.Instance = ((object)(resources.GetObject("grdExSeries_DesignTimeLayout_Reference_0.Instance")));
            grdExSeries_DesignTimeLayout_Reference_1.Instance = ((object)(resources.GetObject("grdExSeries_DesignTimeLayout_Reference_1.Instance")));
            grdExSeries_DesignTimeLayout_Reference_2.Instance = ((object)(resources.GetObject("grdExSeries_DesignTimeLayout_Reference_2.Instance")));
            grdExSeries_DesignTimeLayout_Reference_3.Instance = ((object)(resources.GetObject("grdExSeries_DesignTimeLayout_Reference_3.Instance")));
            grdExSeries_DesignTimeLayout.LayoutReferences.AddRange(new Janus.Windows.Common.Layouts.JanusLayoutReference[] {
            grdExSeries_DesignTimeLayout_Reference_0,
            grdExSeries_DesignTimeLayout_Reference_1,
            grdExSeries_DesignTimeLayout_Reference_2,
            grdExSeries_DesignTimeLayout_Reference_3});
            grdExSeries_DesignTimeLayout.LayoutString = resources.GetString("grdExSeries_DesignTimeLayout.LayoutString");
            this.grdExSeries.DesignTimeLayout = grdExSeries_DesignTimeLayout;
            this.grdExSeries.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdExSeries.EditorsControlStyle.ButtonAppearance = Janus.Windows.GridEX.ButtonAppearance.Regular;
            this.grdExSeries.EmptyRows = true;
            this.grdExSeries.FocusCellFormatStyle.BackColor = System.Drawing.SystemColors.Highlight;
            this.grdExSeries.FocusCellFormatStyle.ForeColor = System.Drawing.Color.White;
            this.grdExSeries.FocusStyle = Janus.Windows.GridEX.FocusStyle.None;
            this.grdExSeries.Font = new System.Drawing.Font("���� ���", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.grdExSeries.FrozenColumns = 2;
            this.grdExSeries.GridLineColor = System.Drawing.Color.Silver;
            this.grdExSeries.GridLines = Janus.Windows.GridEX.GridLines.Vertical;
            this.grdExSeries.GridLineStyle = Janus.Windows.GridEX.GridLineStyle.Solid;
            this.grdExSeries.GroupByBoxVisible = false;
            this.grdExSeries.GroupMode = Janus.Windows.GridEX.GroupMode.Collapsed;
            this.grdExSeries.GroupRowVisualStyle = Janus.Windows.GridEX.GroupRowVisualStyle.Outlook2003;
            this.grdExSeries.HeaderFormatStyle.TextAlignment = Janus.Windows.GridEX.TextAlignment.Center;
            this.grdExSeries.HideSelection = Janus.Windows.GridEX.HideSelection.HighlightInactive;
            grdExSeries_Layout_0.Key = "bea";
            this.grdExSeries.Layouts.AddRange(new Janus.Windows.GridEX.GridEXLayout[] {
            grdExSeries_Layout_0});
            this.grdExSeries.Location = new System.Drawing.Point(0, 0);
            this.grdExSeries.Name = "grdExSeries";
            this.grdExSeries.ScrollBars = Janus.Windows.GridEX.ScrollBars.Vertical;
            this.grdExSeries.SelectedInactiveFormatStyle.BackColor = System.Drawing.Color.Gold;
            this.grdExSeries.Size = new System.Drawing.Size(405, 671);
            this.grdExSeries.TabIndex = 6;
            this.grdExSeries.TabKeyBehavior = Janus.Windows.GridEX.TabKeyBehavior.ControlNavigation;
            this.grdExSeries.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            this.grdExSeries.UpdatingCell += new Janus.Windows.GridEX.UpdatingCellEventHandler(this.grdExSeries_UpdatingCell);
            // 
            // dvSeries
            // 
            this.dvSeries.Table = this.sch3FormDs.Series;
            // 
            // Sch3Form
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(6, 15);
            this.ClientSize = new System.Drawing.Size(1234, 741);
            this.Controls.Add(this.uiPanelSchGroup);
            this.Controls.Add(this.uiPanelBottom);
            this.Font = new System.Drawing.Font("���� ���", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "Sch3Form";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "��������";
            this.Load += new System.EventHandler(this.Sch3Form_Load);
            ((System.ComponentModel.ISupportInitialize)(this.uiPM)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelBottom)).EndInit();
            this.uiPanelBottom.ResumeLayout(false);
            this.uiPanelBottomContainer.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelSchGroup)).EndInit();
            this.uiPanelSchGroup.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelGenre)).EndInit();
            this.uiPanelGenre.ResumeLayout(false);
            this.uiPanelGenreContainer.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdExGenre)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dvGenre)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.sch3FormDs)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelChannel)).EndInit();
            this.uiPanelChannel.ResumeLayout(false);
            this.uiPanelChannelContainer.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdExChannel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dvChannel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelSeries)).EndInit();
            this.uiPanelSeries.ResumeLayout(false);
            this.uiPanelSeriesContainer.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdExSeries)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dvSeries)).EndInit();
            this.ResumeLayout(false);

		}
		#endregion
		#endregion

		#region ���Ǹ޼ҵ�
		
		/// <summary>
		/// �帣Row����� ó�� �̺�Ʈ�ڵ鷯
		/// ������ġ�� ī�װ�/�帣�� ä�ε����͸� �о�´�
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void OnGridRowChangedGenre(object sender, System.EventArgs e) 
		{
			int curRow = cmGenre.Position;

			if(curRow >= 0 )
			{
				keyMedia	= int.Parse(dtGenre.Rows[curRow]["MediaCode"].ToString());
				keyCategory = int.Parse(dtGenre.Rows[curRow]["CategoryCode"].ToString());
				keyGenre	= int.Parse(dtGenre.Rows[curRow]["GenreCode"].ToString());
				keyGenreName= dtGenre.Rows[curRow]["GenreName"].ToString();

				// ���� ����Row�� �޴�/ä��/�ø������Ǽ�
				// ������ ���θ� �����Ҷ� ����
				keyGenreFound1	= int.Parse(dtGenre.Rows[curRow]["AdFound1"].ToString());
				keyGenreFound2	= int.Parse(dtGenre.Rows[curRow]["AdFound2"].ToString());
				keyGenreFound3	= int.Parse(dtGenre.Rows[curRow]["AdFound3"].ToString());

				this.SearchChannel();
			}
		}


		/// <summary>
		/// ä��Row����� ó�� �̺�Ʈ �ڵ鷯
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void OnGridRowChangedChannel(object sender, System.EventArgs e) 
		{
			int curRow = cmChannel.Position;

			if(curRow >= 0 )
			{
				// �����ڵ���� ����
				// �� ���� ī�װ�/�帣�� ���� ä�ε��� Row Postion�� �̵��ϴ� ����
				keyChannel		=	int.Parse(dtChannel.Rows[curRow]["ChannelNo"].ToString());
				keyChannelName	=	dtChannel.Rows[curRow]["Title"].ToString();

				keyChannelFound1	= keyGenreFound1;
				keyChannelFound2	= int.Parse(dtChannel.Rows[curRow]["AdFound2"].ToString());
				keyChannelFound3	= int.Parse(dtChannel.Rows[curRow]["AdFound3"].ToString());

				this.SearchSeriesNo();
			}
		}

		/// <summary>
		/// ȸ��Row����� ó�� �̺�Ʈ �ڵ鷯
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void OnGridRowChangedSeries(object sender, System.EventArgs e) 
		{
			int curRow = cmSeries.Position;

			if(curRow >= 0 )
			{
				// �����ڵ���� ����
				// �� ���� ī�װ�/�帣�� ���� ä�ε��� Row Postion�� �̵��ϴ� ����
				keySeriesFound1	= keyChannelFound1;
				keySeriesFound2	= keyChannelFound2;
				keySeriesFound3	= int.Parse(dtSeries.Rows[curRow]["AdFound"].ToString());
			}
		}

		#endregion

		#region ó���޼ҵ�

		/// <summary>
		/// �帣������ ��ȸ
		/// </summary>
		private void SearchGenre()
		{
			try
			{
				mainModel.Init();
				sch3FormDs.Genre.Clear();

				mainModel.Media	= 1;
				mainModel.ItemNo	= keyItemNo;
				
				Sch3FormManager	Manager = new Sch3FormManager(systemModel, commonModel);
				
				if( keySearchType == 0 )
				{
					Manager.GetGenreListTot( mainModel );
				}
				else if( keySearchType == 13 )	// CSS���� ȣ���Ҷ�
				{
					Manager.GetGenreListCss( mainModel );
				}
				else if( keySearchType == 77 )	// ������
				{
					Manager.GetGenreListDesign( mainModel );
				}

				//new Sch3FormManager(systemModel, commonModel).GetGenreListCss( mainModel );
				
				if (mainModel.ResultCD.Equals("0000"))
				{
					Utility.SetDataTable(sch3FormDs.Genre, mainModel.DsGenre);
					uiPanelGenre.Text = "�帣 : " + mainModel.ResultCnt.ToString("#,##") + "��";
					// ó���ε��� Pos�����̺�Ʈ ��������
					grdExGenre.Focus();
					OnGridRowChangedGenre(null,null);
				}
			}
			catch(FrameException fe)
			{
				FrameSystem.showMsgForm("�޴� ��ȸ����", new string[] {fe.ErrCode, fe.ResultMsg});
			}
			catch(Exception ex)
			{
				FrameSystem.showMsgForm("�޴� ��ȸ����",new string[] {"",ex.Message});
			}
		}


		/// <summary>
		/// ä�ε����� ��ȸ
		/// </summary>
		private void SearchChannel()
		{
			try
			{
				mainModel.Init();
				mainModel.Media		=	keyMedia;
				mainModel.Category	=	keyCategory;
				mainModel.Genre		=	keyGenre;
				mainModel.ItemNo	=	keyItemNo;
				mainModel.DataType	=	keySearchType;

				sch3FormDs.ChannelSets.Clear();

				new Sch3FormManager(systemModel, commonModel).GetChannelList( mainModel );
				
				if ( mainModel.ResultCD.Equals("0000") )
				{
					Utility.SetDataTable(sch3FormDs.ChannelSets, mainModel.DsChannel);
					grdExChannel.Focus();
					uiPanelChannel.Text = "�����帣[ " + keyGenreName +  " ] ä��:" + mainModel.ResultCnt.ToString("#,##") + "��";
					OnGridRowChangedChannel(null,null);
				}
			}
			catch(FrameException fe)
			{
				FrameSystem.showMsgForm("ä�� ��ȸ����", new string[] {fe.ErrCode, fe.ResultMsg});
			}
			catch(Exception ex)
			{
				FrameSystem.showMsgForm("ä�� ��ȸ����",new string[] {"",ex.Message});
			}
		}

		private void SearchSeriesNo()
		{
			try
			{
				mainModel.Init();
				mainModel.Media		=	keyMedia;
				mainModel.Category	=	keyCategory;
				mainModel.Genre		=	keyGenre;
				mainModel.Channel	=	keyChannel;
				mainModel.ItemNo	=	keyItemNo;

				sch3FormDs.Series.Clear();

				new Sch3FormManager(systemModel, commonModel).GetSeriesList( mainModel );
				
				if ( mainModel.ResultCD.Equals("0000") )
				{
					Utility.SetDataTable(sch3FormDs.Series, mainModel.DsSeries);
					//grdExSeries.Focus();
					uiPanelSeries.Text = "����ä��[ " + keyChannelName +  " ] ȸ��:" + mainModel.ResultCnt.ToString("#,##") + "��";
				}
			}
			catch(FrameException fe)
			{
				FrameSystem.showMsgForm("�ø��� ��ȸ����", new string[] {fe.ErrCode, fe.ResultMsg});
			}
			catch(Exception ex)
			{
				FrameSystem.showMsgForm("�ø��� ��ȸ����",new string[] {"",ex.Message});
			}
		}
		#endregion

		#region [ �̺�Ʈ ]
		/// <summary>
		/// �������͸� �������� ����ư�� Ŭ���ϸ� �߻��մϴ�
		/// </summary>
		public event AdManagerClient.Schedule.SchedulePerItemInsertEventHandler	ScheduleSelected;
		private void	OnScheduleSelected()
		{
			ArrayList	schData = new ArrayList();

			#region [ �帣����ó�� ]
			try
			{
				grdExGenre.UpdateData();
				foreach( DataRow row in sch3FormDs.Genre.Rows )
				{
					if( row["IsCheck"].ToString().Equals("True") )
					{
						SchedulePerItemModel newData = new SchedulePerItemModel();
						newData.DeleteJobType	= TYPE_ScheduleDelete.Genre;
						newData.Media		=	Convert.ToInt32( row["MediaCode"].ToString() );
						newData.Category	=	Convert.ToInt32( row["CategoryCode"].ToString() );
						newData.Genre		=	Convert.ToInt32( row["GenreCode"].ToString() );
						schData.Add( newData );
					}
				}
			}
			catch(FrameException fe)
			{
				FrameSystem.showMsgForm("�� ���� ����", new string[] {fe.ErrCode, fe.ResultMsg});
			}
			catch(Exception ex)
			{
				FrameSystem.showMsgForm("�� ���� ����",new string[] {"",ex.Message});
			}			
			#endregion

			#region [ ä�μ���ó�� ]
			try
			{
				grdExChannel.UpdateData();
				foreach( DataRow row in sch3FormDs.ChannelSets.Rows )
				{
					if( row["IsCheck"].ToString().Equals("True") )
					{
						SchedulePerItemModel newData = new SchedulePerItemModel();
						newData.DeleteJobType	= TYPE_ScheduleDelete.Channel;
						newData.Media		=	Convert.ToInt32( row["MediaCode"].ToString() );
						newData.Category	=	Convert.ToInt32( row["CategoryCode"].ToString() );
						newData.Genre		=	Convert.ToInt32( row["GenreCode"].ToString() );
						newData.Channel		=	Convert.ToInt32( row["ChannelNo"].ToString() );
						schData.Add( newData );
					}
				}
			}
			catch(FrameException fe)
			{
				FrameSystem.showMsgForm("�� ���� ����", new string[] {fe.ErrCode, fe.ResultMsg});
			}
			catch(Exception ex)
			{
				FrameSystem.showMsgForm("�� ���� ����",new string[] {"",ex.Message});
			}			
			#endregion

			#region [ ȸ������ó�� ]
			try
			{
				grdExSeries.UpdateData();
				foreach( DataRow row in sch3FormDs.Series.Rows )
				{
					if( row["IsCheck"].ToString().Equals("True") )
					{
						SchedulePerItemModel newData = new SchedulePerItemModel();
						newData.DeleteJobType	= TYPE_ScheduleDelete.Series;
						newData.Media		=	keyMedia;
						newData.Category	=	keyCategory;
						newData.Genre		=	keyGenre;
						newData.Channel		=	keyChannel;
						newData.Series		=	Convert.ToInt32( row["SeriesNo"].ToString() );
						schData.Add( newData );
					}
				}
			}
			catch(FrameException fe)
			{
				FrameSystem.showMsgForm("�� ���� ����", new string[] {fe.ErrCode, fe.ResultMsg});
			}
			catch(Exception ex)
			{
				FrameSystem.showMsgForm("�� ���� ����",new string[] {"",ex.Message});
			}			
			#endregion

			#region [ �̺�Ʈ ȣ�� ]
			// ���õ� ���� �ִ� ���
			if( schData.Count > 0 )
			{
				// �̺�Ʈó���Ⱑ ��ϵǾ��� ��쿡�� ó��
				if( null != ScheduleSelected )
				{
					try
					{
						ScheduleSelected( this, new Schedule.SchedulePerItemEventArgs( schData ) );
                        this.Close();
					}
					catch(Exception ex)
					{
						// �̺�Ʈ����� �޾Ƽ�...
						// �����̸� Check�� �ʱ�ȭ �ϰ�, �ǻ��� �״�� ���д�....
						FrameSystem.showMsgForm("�� ���� ����",new string[] {"",ex.Message});	
					}
				}
			}
			#endregion
		}
		#endregion

		#region [ �� �� ]

		/// <summary>
		/// ���õ� �����ȣ�� �������ų� �����մϴ�
		/// </summary>
		public int	ItemNo
		{
			get
			{
				return keyItemNo;
			}
			set
			{
				keyItemNo = value;
				this.Text	= " " + keyItemNo + " || " + keyItemName + " || �������� ������";
			}
		}

		/// <summary>
		/// ���õ� �����ȣ�� �������ų� �����մϴ�
		/// </summary>
		public string	ItemName
		{
			get
			{
				return keyItemName;
			}
			set
			{
				keyItemName = value;
				this.Text	= " " + keyItemNo + " || " + keyItemName + " || �������� ������";
			}
		}


		public int	DataType
		{
			get
			{
				return keySearchType;
			}
			set
			{
				keySearchType = value;
			}
		}

		#endregion

		#region [ ��Ʈ�� �̺�Ʈ ]

		/// <summary>
		/// ���ε��� ȣ��
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void Sch3Form_Load(object sender, System.EventArgs e)
		{
			dtGenre		= ((DataView)grdExGenre.DataSource).Table;
			dtChannel	= ((DataView)grdExChannel.DataSource).Table;
			dtSeries	= ((DataView)grdExSeries.DataSource).Table;

			cmGenre		= (CurrencyManager)this.BindingContext[grdExGenre.DataSource]; 
			cmChannel	= (CurrencyManager)this.BindingContext[grdExChannel.DataSource]; 
			cmSeries	= (CurrencyManager)this.BindingContext[grdExSeries.DataSource]; 

			cmGenre.PositionChanged		+= new EventHandler(OnGridRowChangedGenre);
			cmChannel.PositionChanged	+= new EventHandler(OnGridRowChangedChannel);
			cmSeries.PositionChanged	+= new EventHandler(OnGridRowChangedSeries);

			ProgressStart();
			SearchGenre();
			ProgressStart();
		}

		/// <summary>
		/// �ݱ��ư Ŭ��
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnClose_Click(object sender, System.EventArgs e)
		{
			this.Close();
		}

		/// <summary>
		/// ����ư Ŭ��
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnOk_Click(object sender, System.EventArgs e)
		{
			this.OnScheduleSelected();
		}

		/// <summary>
		/// ��ȸ��ư Ŭ��
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnSearch_Click(object sender, System.EventArgs e)
		{
			SearchGenre();
		}
		#endregion

		#region [ ���ó��� Ȯ�κκ� ]
		/// <summary>
		/// �帣 �� ýũ����� �����
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void grdExGenre_UpdatingCell(object sender, Janus.Windows.GridEX.UpdatingCellEventArgs e)
		{
			Debug.WriteLine("UpdatingCell->" + e.InitialValue.ToString() + "/" + e.Value.ToString() );

			// ��������� ������ �����ҽ� ����
			if( e.Value.ToString().Equals("True") )
			{
				if( keyGenreFound1 > 0 )
				{
					e.Cancel = true;
					e.Value	= false;
					MessageBox.Show(this,"���Ǿ� �ִ� �޴��Դϴ�","�޴���",MessageBoxButtons.OK, MessageBoxIcon.Warning);
				}
				else if( keyGenreFound2 > 0 )
				{
					e.Cancel = true;
					e.Value	= false;
					MessageBox.Show(this,"����ä�ο� ���� ���� �մϴ�\n\n�޴����� �ʿ��� ��쿣, �������������� �������� �ٽ� �õ��Ͻʽÿ�","�޴���",MessageBoxButtons.OK, MessageBoxIcon.Warning);
				}
				else if( keyGenreFound3 > 0 )
				{
					e.Cancel = true;
					e.Value	= false;
					MessageBox.Show(this,"����ä��ȸ���� ���� ���� �մϴ�\n\n�޴����� �ʿ��� ��쿣, �������������� �������� �ٽ� �õ��Ͻʽÿ�","�޴���",MessageBoxButtons.OK, MessageBoxIcon.Warning);
				}
				else
				{
					e.Cancel = false;
				}
			}
		}


		/// <summary>
		/// ä�� �� ýũ����� �����
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void grdExChannel_UpdatingCell(object sender, Janus.Windows.GridEX.UpdatingCellEventArgs e)
		{
			if( e.Value.ToString().Equals("True") )
			{
				if( keyChannelFound1 > 0 )
				{
					e.Cancel = true;
					e.Value	= false;
					MessageBox.Show(this,"�����޴��� ���Ǿ� �ִ� ä���Դϴ�\n\nä������ �ʿ��� ��쿣, �����޴������� ������ �� �ٽ� �õ� �Ͻʽÿ�","ä����",MessageBoxButtons.OK, MessageBoxIcon.Warning);
				}
				else if( keyChannelFound2 > 0 )
				{
					e.Cancel = true;
					e.Value	= false;
					MessageBox.Show(this,"���Ǿ� �ִ� ä���Դϴ�!!!","ä����",MessageBoxButtons.OK, MessageBoxIcon.Warning);
				}
				else if( keyChannelFound3 > 0 )
				{
					e.Cancel = true;
					e.Value	= false;
					MessageBox.Show(this,"����ä��ȸ���� ���� ���� �մϴ�\n\nä������ �ʿ��� ��쿣, �������������� �������� �ٽ� �õ��Ͻʽÿ�","ä����",MessageBoxButtons.OK, MessageBoxIcon.Warning);
				}
				else
				{
					e.Cancel = false;
				}
			}
		}


		/// <summary>
		/// ȸ�� �� ýũ����� �����
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void grdExSeries_UpdatingCell(object sender, Janus.Windows.GridEX.UpdatingCellEventArgs e)
		{
			if( e.Value.ToString().Equals("True") )
			{
				if( keySeriesFound1 > 0 )
				{
					e.Cancel = true;
					e.Value	= false;
					MessageBox.Show(this,"�����޴��� ���Ǿ� �ִ� ȸ���Դϴ�\n\nȸ������ �ʿ��� ��쿣, �����޴������� ������ �� �ٽ� �õ� �Ͻʽÿ�","ȸ����",MessageBoxButtons.OK, MessageBoxIcon.Warning);
				}
				else if( keySeriesFound2 > 0 )
				{
					e.Cancel = true;
					e.Value	= false;
					MessageBox.Show(this,"����ä���� ���Ǿ� �ִ� ȸ���Դϴ�\n\nȸ������ �ʿ��� ��쿣, ����ä�������� ������ �� �ٽ� �õ� �Ͻʽÿ�","ȸ����",MessageBoxButtons.OK, MessageBoxIcon.Warning);
				}
				else if( keySeriesFound3 > 0 )
				{
					e.Cancel = true;
					e.Value	= false;
					MessageBox.Show(this,"���Ǿ� �ִ� ȸ���Դϴ�!!!","ȸ����",MessageBoxButtons.OK, MessageBoxIcon.Warning);
					
				}
				else
				{
					e.Cancel = false;
				}
			}
		}
		#endregion

		#region [����] Event Handler & Function
		public event StatusEventHandler 			StatusEvent;			// �����̺�Ʈ �ڵ鷯

		private void StatusMessage(string strMessage)
		{
			if (StatusEvent != null) 
			{
				StatusEventArgs ea = new StatusEventArgs();
				ea.Message   = strMessage;
				StatusEvent(this,ea);
			}
		}

		public event ProgressEventHandler 			ProgressEvent;			// ó�����̺�Ʈ �ڵ鷯
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
	}
}