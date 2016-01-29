using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Diagnostics;
using System.Windows.Forms;

using WinFramework.Misc;
using WinFramework.Base;
using WinFramework.Data;

using AdManagerModel;

namespace AdManagerClient
{
	/// <summary>
	/// MediaMenuSetCtrl�� ���� ��� �����Դϴ�.
	/// </summary>
    public class MediaMenuSetCtrl : System.Windows.Forms.UserControl, IUserControl
	{
        private Janus.Windows.UI.Dock.UIPanelManager uiPM;
        private Janus.Windows.UI.Dock.UIPanel uiPanelSearch;
        private Janus.Windows.UI.Dock.UIPanelInnerContainer uiPanelSearchContainer;
        private Janus.Windows.UI.Dock.UIPanelGroup uiPanelTop;
        private Janus.Windows.UI.Dock.UIPanel uiPanelCommand;
        private Janus.Windows.UI.Dock.UIPanelInnerContainer uiPanelCommandContainer;
        private Janus.Windows.UI.Dock.UIPanel uiPanelCategory;
        private Janus.Windows.UI.Dock.UIPanelInnerContainer uiPanelCategoryContainer;
        private Janus.Windows.UI.Dock.UIPanel uiPanelGenre;
        private Janus.Windows.UI.Dock.UIPanelInnerContainer uiPanelGenreContainer;
        private Janus.Windows.GridEX.GridEX gridEXCategory;
        private AdManagerClient._10_Media._11_MediaMenuSet.MediaMenuSet mediaMenuSet;
        private System.Windows.Forms.Label lblAdSlot;
        private Janus.Windows.EditControls.UIComboBox cbAdSlot;
        private Janus.Windows.GridEX.GridEX gridGenre;
        private Janus.Windows.EditControls.UIButton btnSearch;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.ComponentModel.IContainer components;

		public MediaMenuSetCtrl()
		{
			// �� ȣ���� Windows.Forms Form �����̳ʿ� �ʿ��մϴ�.
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

		#region ���� ��� �����̳ʿ��� ������ �ڵ�
		/// <summary> 
		/// �����̳� ������ �ʿ��� �޼����Դϴ�. 
		/// �� �޼����� ������ �ڵ� ������� �������� ���ʽÿ�.
		/// </summary>
		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            Janus.Windows.EditControls.UIComboBoxItem uiComboBoxItem1 = new Janus.Windows.EditControls.UIComboBoxItem();
            Janus.Windows.GridEX.GridEXLayout gridEXCategory_DesignTimeLayout = new Janus.Windows.GridEX.GridEXLayout();
            Janus.Windows.Common.Layouts.JanusLayoutReference gridEXCategory_DesignTimeLayout_Reference_0 = new Janus.Windows.Common.Layouts.JanusLayoutReference("GridEXLayoutData.RootTable.FormatConditions.Condition0.FormatStyle.BackgroundImag" +
        "e");
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MediaMenuSetCtrl));
            Janus.Windows.GridEX.GridEXLayout gridGenre_DesignTimeLayout = new Janus.Windows.GridEX.GridEXLayout();
            Janus.Windows.Common.Layouts.JanusLayoutReference gridGenre_DesignTimeLayout_Reference_0 = new Janus.Windows.Common.Layouts.JanusLayoutReference("GridEXLayoutData.RootTable.FormatConditions.Condition0.FormatStyle.BackgroundImag" +
        "e");
            this.uiPM = new Janus.Windows.UI.Dock.UIPanelManager(this.components);
            this.uiPanelSearch = new Janus.Windows.UI.Dock.UIPanel();
            this.uiPanelSearchContainer = new Janus.Windows.UI.Dock.UIPanelInnerContainer();
            this.btnSearch = new Janus.Windows.EditControls.UIButton();
            this.cbAdSlot = new Janus.Windows.EditControls.UIComboBox();
            this.lblAdSlot = new System.Windows.Forms.Label();
            this.uiPanelTop = new Janus.Windows.UI.Dock.UIPanelGroup();
            this.uiPanelCategory = new Janus.Windows.UI.Dock.UIPanel();
            this.uiPanelCategoryContainer = new Janus.Windows.UI.Dock.UIPanelInnerContainer();
            this.gridEXCategory = new Janus.Windows.GridEX.GridEX();
            this.mediaMenuSet = new AdManagerClient._10_Media._11_MediaMenuSet.MediaMenuSet();
            this.uiPanelGenre = new Janus.Windows.UI.Dock.UIPanel();
            this.uiPanelGenreContainer = new Janus.Windows.UI.Dock.UIPanelInnerContainer();
            this.gridGenre = new Janus.Windows.GridEX.GridEX();
            this.uiPanelCommand = new Janus.Windows.UI.Dock.UIPanel();
            this.uiPanelCommandContainer = new Janus.Windows.UI.Dock.UIPanelInnerContainer();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.uiPM)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelSearch)).BeginInit();
            this.uiPanelSearch.SuspendLayout();
            this.uiPanelSearchContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelTop)).BeginInit();
            this.uiPanelTop.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelCategory)).BeginInit();
            this.uiPanelCategory.SuspendLayout();
            this.uiPanelCategoryContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridEXCategory)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.mediaMenuSet)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelGenre)).BeginInit();
            this.uiPanelGenre.SuspendLayout();
            this.uiPanelGenreContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridGenre)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelCommand)).BeginInit();
            this.uiPanelCommand.SuspendLayout();
            this.uiPanelCommandContainer.SuspendLayout();
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
            this.uiPanelSearch.Id = new System.Guid("05a55a53-f375-48b6-8f9d-3938e69e9d0a");
            this.uiPM.Panels.Add(this.uiPanelSearch);
            this.uiPanelTop.Id = new System.Guid("c97ed5e5-bae3-4e4d-80da-e988e58886c1");
            this.uiPanelTop.StaticGroup = true;
            this.uiPanelCategory.Id = new System.Guid("1dde8928-c799-4917-b10b-7e6497772831");
            this.uiPanelTop.Panels.Add(this.uiPanelCategory);
            this.uiPanelGenre.Id = new System.Guid("63b5a0a1-294a-430a-8344-58c7f2eda991");
            this.uiPanelTop.Panels.Add(this.uiPanelGenre);
            this.uiPM.Panels.Add(this.uiPanelTop);
            this.uiPanelCommand.Id = new System.Guid("f22b2818-3436-4ad1-91aa-78d598784a4a");
            this.uiPM.Panels.Add(this.uiPanelCommand);
            // 
            // Design Time Panel Info:
            // 
            this.uiPM.BeginPanelInfo();
            this.uiPM.AddDockPanelInfo(new System.Guid("05a55a53-f375-48b6-8f9d-3938e69e9d0a"), Janus.Windows.UI.Dock.PanelDockStyle.Top, new System.Drawing.Size(1010, 60), true);
            this.uiPM.AddDockPanelInfo(new System.Guid("c97ed5e5-bae3-4e4d-80da-e988e58886c1"), Janus.Windows.UI.Dock.PanelGroupStyle.VerticalTiles, Janus.Windows.UI.Dock.PanelDockStyle.Top, true, new System.Drawing.Size(1010, 551), true);
            this.uiPM.AddDockPanelInfo(new System.Guid("1dde8928-c799-4917-b10b-7e6497772831"), new System.Guid("c97ed5e5-bae3-4e4d-80da-e988e58886c1"), 845, true);
            this.uiPM.AddDockPanelInfo(new System.Guid("63b5a0a1-294a-430a-8344-58c7f2eda991"), new System.Guid("c97ed5e5-bae3-4e4d-80da-e988e58886c1"), 845, true);
            this.uiPM.AddDockPanelInfo(new System.Guid("f22b2818-3436-4ad1-91aa-78d598784a4a"), Janus.Windows.UI.Dock.PanelDockStyle.Fill, new System.Drawing.Size(1010, 66), true);
            this.uiPM.AddFloatingPanelInfo(new System.Guid("05a55a53-f375-48b6-8f9d-3938e69e9d0a"), new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
            this.uiPM.AddFloatingPanelInfo(new System.Guid("c97ed5e5-bae3-4e4d-80da-e988e58886c1"), Janus.Windows.UI.Dock.PanelGroupStyle.VerticalTiles, true, new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
            this.uiPM.AddFloatingPanelInfo(new System.Guid("1dde8928-c799-4917-b10b-7e6497772831"), new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
            this.uiPM.AddFloatingPanelInfo(new System.Guid("63b5a0a1-294a-430a-8344-58c7f2eda991"), new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
            this.uiPM.AddFloatingPanelInfo(new System.Guid("857b74a4-822f-467d-9bbb-2c3272108e97"), new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
            this.uiPM.AddFloatingPanelInfo(new System.Guid("f22b2818-3436-4ad1-91aa-78d598784a4a"), new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
            this.uiPM.EndPanelInfo();
            // 
            // uiPanelSearch
            // 
            this.uiPanelSearch.AutoHideButtonVisible = Janus.Windows.UI.InheritableBoolean.False;
            this.uiPanelSearch.CloseButtonVisible = Janus.Windows.UI.InheritableBoolean.False;
            this.uiPanelSearch.InnerAreaStyle = Janus.Windows.UI.Dock.PanelInnerAreaStyle.Window;
            this.uiPanelSearch.InnerContainer = this.uiPanelSearchContainer;
            this.uiPanelSearch.Location = new System.Drawing.Point(0, 0);
            this.uiPanelSearch.Name = "uiPanelSearch";
            this.uiPanelSearch.Size = new System.Drawing.Size(1010, 60);
            this.uiPanelSearch.TabIndex = 4;
            this.uiPanelSearch.Text = "��������";
            // 
            // uiPanelSearchContainer
            // 
            this.uiPanelSearchContainer.Controls.Add(this.btnSearch);
            this.uiPanelSearchContainer.Controls.Add(this.cbAdSlot);
            this.uiPanelSearchContainer.Controls.Add(this.lblAdSlot);
            this.uiPanelSearchContainer.Location = new System.Drawing.Point(1, 23);
            this.uiPanelSearchContainer.Name = "uiPanelSearchContainer";
            this.uiPanelSearchContainer.Size = new System.Drawing.Size(1008, 32);
            this.uiPanelSearchContainer.TabIndex = 0;
            // 
            // btnSearch
            // 
            this.btnSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSearch.Enabled = false;
            this.btnSearch.Location = new System.Drawing.Point(887, 4);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(104, 24);
            this.btnSearch.TabIndex = 4;
            this.btnSearch.Text = "�� ȸ";
            this.btnSearch.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // cbAdSlot
            // 
            uiComboBoxItem1.FormatStyle.Alpha = 0;
            uiComboBoxItem1.IsSeparator = false;
            uiComboBoxItem1.Text = "�߰�����";
            uiComboBoxItem1.Value = "M";
            this.cbAdSlot.Items.AddRange(new Janus.Windows.EditControls.UIComboBoxItem[] {
            uiComboBoxItem1});
            this.cbAdSlot.Location = new System.Drawing.Point(80, 5);
            this.cbAdSlot.Name = "cbAdSlot";
            this.cbAdSlot.Size = new System.Drawing.Size(184, 23);
            this.cbAdSlot.TabIndex = 1;
            this.cbAdSlot.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            // 
            // lblAdSlot
            // 
            this.lblAdSlot.BackColor = System.Drawing.Color.Transparent;
            this.lblAdSlot.Location = new System.Drawing.Point(16, 7);
            this.lblAdSlot.Name = "lblAdSlot";
            this.lblAdSlot.Size = new System.Drawing.Size(88, 16);
            this.lblAdSlot.TabIndex = 0;
            this.lblAdSlot.Text = "������";
            // 
            // uiPanelTop
            // 
            this.uiPanelTop.CaptionVisible = Janus.Windows.UI.InheritableBoolean.False;
            this.uiPanelTop.GroupStyle = Janus.Windows.UI.Dock.PanelGroupStyle.VerticalTiles;
            this.uiPanelTop.Location = new System.Drawing.Point(0, 60);
            this.uiPanelTop.Name = "uiPanelTop";
            this.uiPanelTop.Size = new System.Drawing.Size(1010, 551);
            this.uiPanelTop.TabIndex = 4;
            this.uiPanelTop.Text = "Panel 1";
            // 
            // uiPanelCategory
            // 
            this.uiPanelCategory.CaptionVisible = Janus.Windows.UI.InheritableBoolean.False;
            this.uiPanelCategory.InnerContainer = this.uiPanelCategoryContainer;
            this.uiPanelCategory.Location = new System.Drawing.Point(0, 0);
            this.uiPanelCategory.Name = "uiPanelCategory";
            this.uiPanelCategory.Size = new System.Drawing.Size(503, 547);
            this.uiPanelCategory.TabIndex = 4;
            this.uiPanelCategory.Text = "Panel 4";
            // 
            // uiPanelCategoryContainer
            // 
            this.uiPanelCategoryContainer.Controls.Add(this.gridEXCategory);
            this.uiPanelCategoryContainer.Location = new System.Drawing.Point(1, 1);
            this.uiPanelCategoryContainer.Name = "uiPanelCategoryContainer";
            this.uiPanelCategoryContainer.Size = new System.Drawing.Size(501, 545);
            this.uiPanelCategoryContainer.TabIndex = 0;
            // 
            // gridEXCategory
            // 
            this.gridEXCategory.AllowEdit = Janus.Windows.GridEX.InheritableBoolean.False;
            this.gridEXCategory.AlternatingColors = true;
            this.gridEXCategory.BorderStyle = Janus.Windows.GridEX.BorderStyle.None;
            this.gridEXCategory.DataSource = this.mediaMenuSet.Category;
            gridEXCategory_DesignTimeLayout_Reference_0.Instance = ((object)(resources.GetObject("gridEXCategory_DesignTimeLayout_Reference_0.Instance")));
            gridEXCategory_DesignTimeLayout.LayoutReferences.AddRange(new Janus.Windows.Common.Layouts.JanusLayoutReference[] {
            gridEXCategory_DesignTimeLayout_Reference_0});
            gridEXCategory_DesignTimeLayout.LayoutString = resources.GetString("gridEXCategory_DesignTimeLayout.LayoutString");
            this.gridEXCategory.DesignTimeLayout = gridEXCategory_DesignTimeLayout;
            this.gridEXCategory.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridEXCategory.FocusCellFormatStyle.BackColor = System.Drawing.SystemColors.Highlight;
            this.gridEXCategory.FocusCellFormatStyle.ForeColor = System.Drawing.Color.White;
            this.gridEXCategory.FocusStyle = Janus.Windows.GridEX.FocusStyle.None;
            this.gridEXCategory.GroupByBoxVisible = false;
            this.gridEXCategory.HideSelection = Janus.Windows.GridEX.HideSelection.Highlight;
            this.gridEXCategory.Location = new System.Drawing.Point(0, 0);
            this.gridEXCategory.Name = "gridEXCategory";
            this.gridEXCategory.ScrollBars = Janus.Windows.GridEX.ScrollBars.Vertical;
            this.gridEXCategory.Size = new System.Drawing.Size(501, 545);
            this.gridEXCategory.TabIndex = 0;
            this.gridEXCategory.UpdateMode = Janus.Windows.GridEX.UpdateMode.CellUpdate;
            this.gridEXCategory.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            this.gridEXCategory.ColumnButtonClick += new Janus.Windows.GridEX.ColumnActionEventHandler(this.gridEXCategory_ColumnButtonClick);
            this.gridEXCategory.Enter += new System.EventHandler(this.gridEXCategory_Enter);
            // 
            // mediaMenuSet
            // 
            this.mediaMenuSet.DataSetName = "MediaMenuSet";
            this.mediaMenuSet.Locale = new System.Globalization.CultureInfo("en-US");
            this.mediaMenuSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // uiPanelGenre
            // 
            this.uiPanelGenre.CaptionVisible = Janus.Windows.UI.InheritableBoolean.False;
            this.uiPanelGenre.InnerContainer = this.uiPanelGenreContainer;
            this.uiPanelGenre.Location = new System.Drawing.Point(507, 0);
            this.uiPanelGenre.Name = "uiPanelGenre";
            this.uiPanelGenre.Size = new System.Drawing.Size(503, 547);
            this.uiPanelGenre.TabIndex = 4;
            this.uiPanelGenre.Text = "Panel 5";
            // 
            // uiPanelGenreContainer
            // 
            this.uiPanelGenreContainer.Controls.Add(this.gridGenre);
            this.uiPanelGenreContainer.Location = new System.Drawing.Point(1, 1);
            this.uiPanelGenreContainer.Name = "uiPanelGenreContainer";
            this.uiPanelGenreContainer.Size = new System.Drawing.Size(501, 545);
            this.uiPanelGenreContainer.TabIndex = 0;
            // 
            // gridGenre
            // 
            this.gridGenre.AllowCardSizing = false;
            this.gridGenre.AllowColumnDrag = false;
            this.gridGenre.AllowEdit = Janus.Windows.GridEX.InheritableBoolean.False;
            this.gridGenre.AlternatingColors = true;
            this.gridGenre.BorderStyle = Janus.Windows.GridEX.BorderStyle.None;
            this.gridGenre.DataSource = this.mediaMenuSet.Genre;
            gridGenre_DesignTimeLayout_Reference_0.Instance = ((object)(resources.GetObject("gridGenre_DesignTimeLayout_Reference_0.Instance")));
            gridGenre_DesignTimeLayout.LayoutReferences.AddRange(new Janus.Windows.Common.Layouts.JanusLayoutReference[] {
            gridGenre_DesignTimeLayout_Reference_0});
            gridGenre_DesignTimeLayout.LayoutString = resources.GetString("gridGenre_DesignTimeLayout.LayoutString");
            this.gridGenre.DesignTimeLayout = gridGenre_DesignTimeLayout;
            this.gridGenre.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridGenre.FocusCellFormatStyle.BackColor = System.Drawing.SystemColors.Highlight;
            this.gridGenre.FocusCellFormatStyle.ForeColor = System.Drawing.Color.White;
            this.gridGenre.FocusStyle = Janus.Windows.GridEX.FocusStyle.None;
            this.gridGenre.GroupByBoxVisible = false;
            this.gridGenre.HideSelection = Janus.Windows.GridEX.HideSelection.HighlightInactive;
            this.gridGenre.Location = new System.Drawing.Point(0, 0);
            this.gridGenre.Name = "gridGenre";
            this.gridGenre.ScrollBars = Janus.Windows.GridEX.ScrollBars.Vertical;
            this.gridGenre.Size = new System.Drawing.Size(501, 545);
            this.gridGenre.TabIndex = 1;
            this.gridGenre.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            this.gridGenre.VisualStyleAreas.CheckBoxStyle = Janus.Windows.GridEX.VisualStyle.Office2003;
            this.gridGenre.ColumnButtonClick += new Janus.Windows.GridEX.ColumnActionEventHandler(this.gridGenre_ColumnButtonClick);
            // 
            // uiPanelCommand
            // 
            this.uiPanelCommand.CaptionVisible = Janus.Windows.UI.InheritableBoolean.False;
            this.uiPanelCommand.InnerAreaStyle = Janus.Windows.UI.Dock.PanelInnerAreaStyle.Window;
            this.uiPanelCommand.InnerContainer = this.uiPanelCommandContainer;
            this.uiPanelCommand.Location = new System.Drawing.Point(0, 611);
            this.uiPanelCommand.Name = "uiPanelCommand";
            this.uiPanelCommand.Size = new System.Drawing.Size(1010, 66);
            this.uiPanelCommand.TabIndex = 4;
            this.uiPanelCommand.Text = "Panel 3";
            // 
            // uiPanelCommandContainer
            // 
            this.uiPanelCommandContainer.Controls.Add(this.label2);
            this.uiPanelCommandContainer.Controls.Add(this.label1);
            this.uiPanelCommandContainer.Location = new System.Drawing.Point(1, 1);
            this.uiPanelCommandContainer.Name = "uiPanelCommandContainer";
            this.uiPanelCommandContainer.Size = new System.Drawing.Size(1008, 64);
            this.uiPanelCommandContainer.TabIndex = 0;
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Location = new System.Drawing.Point(15, 21);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(721, 16);
            this.label2.TabIndex = 1;
            this.label2.Text = "�߰��ϰų� ������ ī�װ�/�帣�� �����Ͻ���, ��ư�� Ŭ���Ͻø� ó�� �˴ϴ�";
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Location = new System.Drawing.Point(16, 5);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(232, 16);
            this.label1.TabIndex = 0;
            this.label1.Text = "������ ����� �����ϴ� �޴��Դϴ�";
            // 
            // MediaMenuSetCtrl
            // 
            this.Controls.Add(this.uiPanelCommand);
            this.Controls.Add(this.uiPanelTop);
            this.Controls.Add(this.uiPanelSearch);
            this.Font = new System.Drawing.Font("���� ���", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.Name = "MediaMenuSetCtrl";
            this.Size = new System.Drawing.Size(1010, 677);
            this.Load += new System.EventHandler(this.MediaMenuSetCtrl_Load);
            ((System.ComponentModel.ISupportInitialize)(this.uiPM)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelSearch)).EndInit();
            this.uiPanelSearch.ResumeLayout(false);
            this.uiPanelSearchContainer.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelTop)).EndInit();
            this.uiPanelTop.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelCategory)).EndInit();
            this.uiPanelCategory.ResumeLayout(false);
            this.uiPanelCategoryContainer.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridEXCategory)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.mediaMenuSet)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelGenre)).EndInit();
            this.uiPanelGenre.ResumeLayout(false);
            this.uiPanelGenreContainer.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridGenre)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelCommand)).EndInit();
            this.uiPanelCommand.ResumeLayout(false);
            this.uiPanelCommandContainer.ResumeLayout(false);
            this.ResumeLayout(false);

        }
		#endregion

        #region [ �̺�Ʈ ]
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
			
        #region [ ���� ������ Ŭ������ ]
        private SystemModel     systemModel = FrameSystem.oSysModel;
        private CommonModel     commonModel = FrameSystem.oComModel;
        private Logger          log         = FrameSystem.oLog;
        private MenuPower       menu        = FrameSystem.oMenu;
        private string          menuCode    = "";
        //private bool IsAdding             = false;
        private bool canRead			  = false;
        //private bool canUpdate			  = false;
        //private bool canCreate            = false;
        //private bool canDelete            = false;
        #endregion

        #region IUserControl ����
        /// <summary>
        /// �޴� �ڵ�-������ �ʿ��� ȭ�鿡 �ʿ���
        /// </summary>
        public string MenuCode
        {
            set { this.menuCode = value; }
            get { return this.menuCode; }
        }

        /// <summary>
        /// �θ���Ʈ�� ����
        /// </summary>
        /// <param name="control"></param>
        public void SetParent(Control control)
        {
            this.Parent = control;
        }
        /// <summary>
        /// DockStype����
        /// </summary>
        /// <param name="style"></param>
        public void SetDockStyle(DockStyle style)
        {
            this.Dock = style;
        }
        #endregion

        #region [ ���� ó�� �Լ��� ]
        /// <summary>
        /// �� ���� ��
        /// </summary>
        private MediaMenuSetModel   model = new MediaMenuSetModel();
        /// <summary>
        /// ī�װ������� �о�´�
        /// </summary>
        private void SearchCategoryList()
        {
            StatusMessage("ī�װ��� ��ȸ�մϴ�.");

            try
            {				
                model.Init();
                
                new MediaMenuSetManager( systemModel, commonModel).GetCategoryList( model );

                if (model.ResultCD.Equals("0000"))
                {
                    Utility.SetDataTable(mediaMenuSet.Category, model.CategoryDataSet);				
                    StatusMessage(model.ResultCnt + "���� ī�װ� ������ ��ȸ�Ǿ����ϴ�.");
                }     
                
//                SetGroupDetailText();
//
//                int curRow = cmChannel.Position;
//                if(canCreate) btnAdd1.Enabled = true;
//                if(curRow >= 0 )
//                {
//                    if(canCreate) btnAdd1.Enabled = true;
//                    if(canUpdate) btnSave.Enabled = true;
//                    if(canDelete) btnDelete1.Enabled = true;										
//                }
            }
            catch(FrameException fe)
            {
                FrameSystem.showMsgForm("�׷���ȸ����", new string[] {fe.ErrCode, fe.ResultMsg});
            }
            catch(Exception ex)
            {
                FrameSystem.showMsgForm("�׷���ȸ����",new string[] {"",ex.Message});
            }
        }

        private void SearchGenreList(int category)
        {
            StatusMessage("�帣�� ��ȸ�մϴ�.");

            try
            {				
                model.Init();
                model.Category = category;
                mediaMenuSet.Genre.Clear();

                new MediaMenuSetManager( systemModel, commonModel).GetGenreList( model );

                
                if (model.ResultCD.Equals("0000"))
                {
                    Utility.SetDataTable(mediaMenuSet.Genre, model.GenreDataSet);				
                    StatusMessage(model.ResultCnt + "���� �帣 ������ ��ȸ�Ǿ����ϴ�.");
                }     
            }
            catch(FrameException fe)
            {
                FrameSystem.showMsgForm("�׷���ȸ����", new string[] {fe.ErrCode, fe.ResultMsg});
            }
            catch(Exception ex)
            {
                FrameSystem.showMsgForm("�׷���ȸ����",new string[] {"",ex.Message});
            }
        }

        private void CategoryInsert(int category)
        {
            StatusMessage("����� ī�װ� �߰�");

            try
            {				
                model.Init();
                model.Category = category;

                new MediaMenuSetManager( systemModel, commonModel).CategoryInsert( model );
                
                if (model.ResultCD.Equals("0000"))
                {
                    StatusMessage("����� ī�װ� �߰� �Ϸ�");
                }     
            }
            catch(FrameException fe)
            {
                FrameSystem.showMsgForm("����� ī�װ� �߰� ����", new string[] {fe.ErrCode, fe.ResultMsg});
            }
            catch(Exception ex)
            {
                FrameSystem.showMsgForm("����� ī�װ� �߰� ����",new string[] {"",ex.Message});
            }
        }

        private void CategoryDelete(int category)
        {
            try
            {				
                model.Init();
                model.Category = category;

                new MediaMenuSetManager( systemModel, commonModel).CategoryDelete( model );
                
                if (model.ResultCD.Equals("0000"))
                {
                    StatusMessage("����� ī�װ� ���� �Ϸ�");
                }     
            }
            catch(FrameException fe)
            {
                FrameSystem.showMsgForm("����� ī�װ� ���� ����", new string[] {fe.ErrCode, fe.ResultMsg});
            }
            catch(Exception ex)
            {
                FrameSystem.showMsgForm("����� ī�װ� ���� ����",new string[] {"",ex.Message});
            }
        }

        private void GenreInsert(int category,int genre)
        {
            StatusMessage("����� �帣 �߰�");

            try
            {				
                model.Init();
                model.Category  =   category;
                model.Genre     =   genre;

                new MediaMenuSetManager( systemModel, commonModel).GenreInsert( model );
                
                if (model.ResultCD.Equals("0000"))
                {
                    StatusMessage("����� �帣 �߰� �Ϸ�");
                }     
            }
            catch(FrameException fe)
            {
                FrameSystem.showMsgForm("����� �帣 �߰� ����", new string[] {fe.ErrCode, fe.ResultMsg});
            }
            catch(Exception ex)
            {
                FrameSystem.showMsgForm("����� �帣 �߰� ����",new string[] {"",ex.Message});
            }
        }

        private void GenreDelete(int category,int genre)
        {
            StatusMessage("����� �帣 ����");

            try
            {				
                model.Init();
                model.Category  =   category;
                model.Genre     =   genre;

                new MediaMenuSetManager( systemModel, commonModel).GenreDelete( model );
                
                if (model.ResultCD.Equals("0000"))
                {
                    StatusMessage("����� �帣 ���� �Ϸ�");
                }     
            }
            catch(FrameException fe)
            {
                FrameSystem.showMsgForm("����� �帣 ���� ����", new string[] {fe.ErrCode, fe.ResultMsg});
            }
            catch(Exception ex)
            {
                FrameSystem.showMsgForm("����� �帣 ���� ����",new string[] {"",ex.Message});
            }
        }

        /// <summary>
        /// ScrollToRow ī�װ�
        /// </summary>
        private void ChoiceCategory( string category)
        {
            try
            {
                Application.DoEvents();
                int rowIndex = 0;
                if ( mediaMenuSet.Category.Rows.Count < 1 ) return;
              
                foreach (DataRow row in mediaMenuSet.Category.Rows)
                {					
                    if(row["Category"].ToString().Equals( category) )
                    {					
                        cmCategory.Position = rowIndex;
                        break;								
                    }

                    rowIndex++;
                }
                gridEXCategory.EnsureVisible();
            }
            catch(FrameException fe)
            {
                FrameSystem.showMsgForm("Ű������", new string[] {fe.ErrCode, fe.ResultMsg});
            }
            catch(Exception ex)
            {
                FrameSystem.showMsgForm("Ű������",new string[] {"",ex.Message});
            }			
        }

        private void ChoiceGenre( string genre)
        {
            try
            {
                Application.DoEvents();
                int rowIndex = 0;
                if ( mediaMenuSet.Genre.Rows.Count < 1 ) return;
              
                foreach (DataRow row in mediaMenuSet.Genre.Rows)
                {					
                    if(row["Genre"].ToString().Equals( genre) )
                    {					
                        cmGenre.Position = rowIndex;
                        break;								
                    }

                    rowIndex++;
                }
                gridGenre.EnsureVisible();
            }
            catch(FrameException fe)
            {
                FrameSystem.showMsgForm("Ű������", new string[] {fe.ErrCode, fe.ResultMsg});
            }
            catch(Exception ex)
            {
                FrameSystem.showMsgForm("Ű������",new string[] {"",ex.Message});
            }			
        }
        #endregion

        private CurrencyManager cmCategory  = null;
        private CurrencyManager cmGenre     = null;

        private void MediaMenuSetCtrl_Load(object sender, System.EventArgs e)
        {
            cmCategory  = (CurrencyManager) this.BindingContext[gridEXCategory.DataSource]; 
            cmCategory.PositionChanged += new System.EventHandler(gridEXCategory_Enter); 
            cmGenre     = (CurrencyManager) this.BindingContext[gridGenre.DataSource]; 
            
            InitControl();
        }

        private void InitControl()
        {
            ProgressStart();

            InitCombo_AdSlot();

            // �߰����� �˻�
            //if(menu.CanCreate(MenuCode))    canCreate = true;
            // ��ȸ���� �˻�            
            if(menu.CanRead(MenuCode))      canRead = true;
            // �������� �˻�
            //if(menu.CanUpdate(MenuCode))    canUpdate = true;

            // �������� �˻�
            //if(menu.CanDelete(MenuCode))    canDelete = true;

            InitButton();

            ProgressStop();
									
            if(canRead)
            {
                SearchCategoryList();
                gridEXCategory_Enter(null,null);
            }
        }	

        private void InitCombo_AdSlot()
        {			
            // �˻������� �޺�
            this.cbAdSlot.SelectedIndex = 0;
            Application.DoEvents();
        }

        private void InitButton()
        {
            if(canRead)   btnSearch.Enabled = true;
            //if(canCreate) btnAdd.Enabled    = true;

            //if(ebMediaName.Text.Trim().Length > 0) 
//            {
//                if(canDelete) btnDelete.Enabled = true;
//                if(canUpdate) btnSave.Enabled   = true;
//            }
            Application.DoEvents();
        }

        private void gridEXCategory_Enter(object sender, System.EventArgs e)
        {
            int currRow = cmCategory.Position;

            if ( currRow >= 0 )
            {
                int category = Convert.ToInt32( mediaMenuSet.Tables["Category"].Rows[currRow]["Category"].ToString() );
                this.SearchGenreList( category );
            }
        }

        #region ī�װ� �÷� ��ư Ŭ��
        private void gridEXCategory_ColumnButtonClick(object sender, Janus.Windows.GridEX.ColumnActionEventArgs e)
        {
            int     currRow = cmCategory.Position;
            string  message = "";

            if ( currRow >= 0 )
            {

                string categoryCode = mediaMenuSet.Category.Rows[currRow]["Category"].ToString();
                string categoryName = mediaMenuSet.Category.Rows[currRow]["CategoryName"].ToString();
                string setCategory  = mediaMenuSet.Category.Rows[currRow]["SetCategory"].ToString();
                int     setGenre    = int.Parse( mediaMenuSet.Category.Rows[currRow]["SetGenre"].ToString() );
                int     setChannel  = int.Parse( mediaMenuSet.Category.Rows[currRow]["SetChannel"].ToString() );

                if( setCategory.Equals("True") )
                {
                    /*
                     * ������ΰ��� �����ϴ� ���
                     * */
                    DialogResult result = MessageBox.Show("[" + categoryName + "] ī�װ��� ����󿡼� ���� �մϴ�. ","�������",MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);

                    if( result == DialogResult.Yes )
                    {
                        ProgressStart();
                        this.CategoryDelete( int.Parse( categoryCode) );
                        this.SearchCategoryList();
                        this.ChoiceCategory( categoryCode );
                        ProgressStop();
                        StatusMessage( "�۾��Ϸ�!!!" );

                        //mediaMenuSet.Category.Rows[currRow].BeginEdit();
                        //mediaMenuSet.Category.Rows[currRow]["SetCategory"] = "False";
                        //mediaMenuSet.Category.Rows[currRow].EndEdit();
                    }
                    else
                    {
                        StatusMessage( "�۾��� ��ҵǾ����ϴ�!!!" );
                    }
                }
                else
                {
                    /*
                     * �űԷ� ����� �߰��ϴ� ���
                     * */
                    DialogResult    result;
                    if( setGenre > 0 || setChannel > 0 )
                    {
                        message = string.Format("[{0}] ī�װ��� ��������� �߰� �մϴ�\n\n�����帣 Ȥ�� ä�ο� ������� ������ �����̸�, �ش� �����ʹ� �����˴ϴ�\n\nó���Ͻðڽ��ϱ�? ", categoryName);
                        result = MessageBox.Show(message,"�������",MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
                    }
                    else
                    {
                        message = string.Format("[{0}] ī�װ��� ��������� �߰� �մϴ�\nó���Ͻðڽ��ϱ�? ", categoryName);
                        result = MessageBox.Show(message,"�������",MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
                    }

                    if( result == DialogResult.Yes )
                    {
                        ProgressStart();
                        this.CategoryInsert( int.Parse( categoryCode) );
                        this.SearchCategoryList();
                        this.ChoiceCategory( categoryCode );
                        ProgressStop();

                        //mediaMenuSet.Category.Rows[currRow].BeginEdit();
                        //mediaMenuSet.Category.Rows[currRow]["SetCategory"] = "True";
                        //mediaMenuSet.Category.Rows[currRow].EndEdit();
                        StatusMessage( "�۾��Ϸ�!!!" );
                    }
                    else
                    {
                        StatusMessage( "�۾��� ��ҵǾ����ϴ�!!!" );
                    }
                }
            }
        }

        #endregion

        #region �帣 �÷� ��ư Ŭ��
        /// <summary>
        /// �帣 �÷���ư Ŭ��ó��
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridGenre_ColumnButtonClick(object sender, Janus.Windows.GridEX.ColumnActionEventArgs e)
        {
            int     currRow = cmGenre.Position;
            string  message = "";
            DialogResult    result;

            if ( currRow >= 0 )
            {
                string categoryCode = mediaMenuSet.Genre.Rows[currRow]["Category"].ToString();
                string genreCode    = mediaMenuSet.Genre.Rows[currRow]["Genre"].ToString();
                string genreName    = mediaMenuSet.Genre.Rows[currRow]["GenreName"].ToString();
                
                string setCategory  = mediaMenuSet.Genre.Rows[currRow]["SetCategory"].ToString();
                string setGenre     = mediaMenuSet.Genre.Rows[currRow]["SetGenre"].ToString();
                int    setChannel  = int.Parse( mediaMenuSet.Genre.Rows[currRow]["SetChannel"].ToString() );

                if( setCategory.Equals("True") )
                {
                    /*
                     * ī�װ� ������ �Ǿ� �ִ� ���
                     * */
                    if( setGenre.Equals("True") )
                    {
                        /*
                        * ī�װ������� �Ǿ� �ִ� ���¿��� �帣������� �����ϴ� ���
                        * �̷� ���� ����. ������ ���ռ��� �߸��� �����
                        * */
                        message = string.Format("[{0}] �帣�� ����󿡼� ���� �մϴ�\n����ī�װ��� ��������� ������ ���������\n �ش��۾��� ������ �� �����ϴ�\n ����ڿ��� ���� �Ͻʽÿ� ", genreName);
                        MessageBox.Show(message,"�������",MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    else
                    {
                        
                        if( setChannel > 0 )
                        {
                            /*
                             * ī�װ����� �����ϰ�
                             * ä������ �����ϴ� ���� ����
                             * */
                            message = string.Format("[{0}] �帣�� ����󿡼� ���� �մϴ�\n����ī�װ��� ��������� ������ ���������\n �ش��۾��� ������ �� �����ϴ�\n ����ڿ��� ���� �Ͻʽÿ� ", genreName);
                            MessageBox.Show(message,"�������",MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                        else
                        {
                            /*
                            * ī�װ������� �Ǿ� �ִ� ���¿��� �帣��������� �߰��ϴ� ���
                            * ����ä�ο��� ������ ���� ���
                            * ���������ʹ� �����ϰ� ó���ϰ� �Ѵ�
                            * */
                            message = string.Format("[{0}] �帣�� ��������� �߰� �մϴ�\n\n����ī�װ��� ��������� ������ ���������\n����ī�װ� ������� �����˴ϴ�\n\nó���Ͻðڽ��ϱ�? ", genreName);
                            result = MessageBox.Show(message,"�������",MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);

                            if( result == DialogResult.Yes )
                            {
                                ProgressStart();
                                this.GenreInsert( int.Parse( categoryCode), int.Parse( genreCode) );
                                this.SearchCategoryList();
                                this.ChoiceCategory( categoryCode );
                                this.ChoiceGenre( genreCode );
                                ProgressStop();
                                StatusMessage( "�۾��Ϸ�!!!" );
                            }
                            else
                            {
                                StatusMessage( "�۾��� ��ҵǾ����ϴ�!!!" );
                            }
                        }
                    }
                }
                else
                {
                    /*
                     * ī�װ� ������ ���� ���
                     * */
                    if( setGenre.Equals("True") )
                    {
                        /*
                        * �帣������� �����ϴ� ���
                        * */
                        message = string.Format("[{0}] �帣�� ����󿡼� ���� �մϴ�\n\n ó���Ͻðڽ��ϱ�? ", genreName);
                        result = MessageBox.Show(message,"�������",MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);

                        if( result == DialogResult.Yes )
                        {
                            ProgressStart();
                            this.GenreDelete( int.Parse( categoryCode), int.Parse( genreCode) );
                            this.SearchCategoryList();
                            this.ChoiceCategory( categoryCode );
                            this.ChoiceGenre( genreCode );
                            ProgressStop();
                            StatusMessage( "�۾��Ϸ�!!!" );
                        }
                        else
                        {
                            StatusMessage( "�۾��� ��ҵǾ����ϴ�!!!" );
                        }
                    }
                    else
                    {
                        if( setChannel > 0 )
                        {
                            /*
                             * ä����������� �Ǿ� �ִ� ���
                             * �帣��������� �߰��ϴ� ���
                             * ä������ �����Ѵ�
                             * ���� ������
                             * */
                            message = string.Format("[{0}] �帣�� ��������� �߰� �մϴ�\n\n���� ä���� ��������� �����Ǿ� �ֽ�����, ä���� ����ó�� �˴ϴ�. \n\nó���Ͻðڽ��ϱ�? ", genreName);
                            result = MessageBox.Show(message,"�������",MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);

                            if( result == DialogResult.Yes )
                            {
                                ProgressStart();
                                this.GenreInsert( int.Parse( categoryCode), int.Parse( genreCode) );
                                this.SearchCategoryList();
                                this.ChoiceCategory( categoryCode );
                                this.ChoiceGenre( genreCode );
                                ProgressStop();
                                StatusMessage( "�۾��Ϸ�!!!" );
                            }
                            else
                            {
                                StatusMessage( "�۾��� ��ҵǾ����ϴ�!!!" );
                            }
                        }
                        else
                        {
                            /*
                             * �帣��������� �߰��ϴ� ���
                             * */
                            message = string.Format("[{0}] �帣�� ��������� �߰� �մϴ�\n\nó���Ͻðڽ��ϱ�? ", genreName);
                            result = MessageBox.Show(message,"�������",MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);

                            if( result == DialogResult.Yes )
                            {
                                ProgressStart();
                                this.GenreInsert( int.Parse( categoryCode), int.Parse( genreCode) );
                                this.SearchCategoryList();
                                this.ChoiceCategory( categoryCode );
                                this.ChoiceGenre( genreCode );
                                ProgressStop();
                                StatusMessage( "�۾��Ϸ�!!!" );
                            }
                            else
                            {
                                StatusMessage( "�۾��� ��ҵǾ����ϴ�!!!" );
                            }
                        }
                    }
                
                }
            }        
        }
        #endregion

        private void btnSearch_Click(object sender, System.EventArgs e)
        {
            try
            {
                ProgressStart();
                this.SearchCategoryList();
                gridEXCategory_Enter(null,null);
                ProgressStop();
            }
            catch(Exception ex)
            {
                FrameSystem.showMsgForm("����� ��ȸ ����", new string[] {ex.Source, ex.Message});
            }
        }
    }
}
