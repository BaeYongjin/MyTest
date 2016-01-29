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
	/// MediaMenuSetCtrl에 대한 요약 설명입니다.
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
			// 이 호출은 Windows.Forms Form 디자이너에 필요합니다.
			InitializeComponent();
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

		#region 구성 요소 디자이너에서 생성한 코드
		/// <summary> 
		/// 디자이너 지원에 필요한 메서드입니다. 
		/// 이 메서드의 내용을 코드 편집기로 수정하지 마십시오.
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
            this.uiPanelSearch.Text = "편성대상관리";
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
            this.btnSearch.Text = "조 회";
            this.btnSearch.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // cbAdSlot
            // 
            uiComboBoxItem1.FormatStyle.Alpha = 0;
            uiComboBoxItem1.IsSeparator = false;
            uiComboBoxItem1.Text = "중간광고";
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
            this.lblAdSlot.Text = "광고슬롯";
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
            this.label2.Text = "추가하거나 삭제할 카테고리/장르를 선택하신후, 버튼을 클릭하시면 처리 됩니다";
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Location = new System.Drawing.Point(16, 5);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(232, 16);
            this.label1.TabIndex = 0;
            this.label1.Text = "슬롯편성 대상을 설정하는 메뉴입니다";
            // 
            // MediaMenuSetCtrl
            // 
            this.Controls.Add(this.uiPanelCommand);
            this.Controls.Add(this.uiPanelTop);
            this.Controls.Add(this.uiPanelSearch);
            this.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
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

        #region [ 이벤트 ]
        public event StatusEventHandler 			StatusEvent;			// 상태이벤트 핸들러

        private void StatusMessage(string strMessage)
        {
            if (StatusEvent != null) 
            {
                StatusEventArgs ea = new StatusEventArgs();
                ea.Message   = strMessage;
                StatusEvent(this,ea);
            }
        }

        public event ProgressEventHandler 			ProgressEvent;			// 처리중이벤트 핸들러
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
			
        #region [ 공용 변수및 클래스들 ]
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

        #region IUserControl 구현
        /// <summary>
        /// 메뉴 코드-보안이 필요한 화면에 필요함
        /// </summary>
        public string MenuCode
        {
            set { this.menuCode = value; }
            get { return this.menuCode; }
        }

        /// <summary>
        /// 부모컨트롤 지정
        /// </summary>
        /// <param name="control"></param>
        public void SetParent(Control control)
        {
            this.Parent = control;
        }
        /// <summary>
        /// DockStype지정
        /// </summary>
        /// <param name="style"></param>
        public void SetDockStyle(DockStyle style)
        {
            this.Dock = style;
        }
        #endregion

        #region [ 내부 처리 함수들 ]
        /// <summary>
        /// 주 업무 모델
        /// </summary>
        private MediaMenuSetModel   model = new MediaMenuSetModel();
        /// <summary>
        /// 카테고리정보를 읽어온다
        /// </summary>
        private void SearchCategoryList()
        {
            StatusMessage("카테고리를 조회합니다.");

            try
            {				
                model.Init();
                
                new MediaMenuSetManager( systemModel, commonModel).GetCategoryList( model );

                if (model.ResultCD.Equals("0000"))
                {
                    Utility.SetDataTable(mediaMenuSet.Category, model.CategoryDataSet);				
                    StatusMessage(model.ResultCnt + "건의 카테고리 정보가 조회되었습니다.");
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
                FrameSystem.showMsgForm("그룹조회오류", new string[] {fe.ErrCode, fe.ResultMsg});
            }
            catch(Exception ex)
            {
                FrameSystem.showMsgForm("그룹조회오류",new string[] {"",ex.Message});
            }
        }

        private void SearchGenreList(int category)
        {
            StatusMessage("장르를 조회합니다.");

            try
            {				
                model.Init();
                model.Category = category;
                mediaMenuSet.Genre.Clear();

                new MediaMenuSetManager( systemModel, commonModel).GetGenreList( model );

                
                if (model.ResultCD.Equals("0000"))
                {
                    Utility.SetDataTable(mediaMenuSet.Genre, model.GenreDataSet);				
                    StatusMessage(model.ResultCnt + "건의 장르 정보가 조회되었습니다.");
                }     
            }
            catch(FrameException fe)
            {
                FrameSystem.showMsgForm("그룹조회오류", new string[] {fe.ErrCode, fe.ResultMsg});
            }
            catch(Exception ex)
            {
                FrameSystem.showMsgForm("그룹조회오류",new string[] {"",ex.Message});
            }
        }

        private void CategoryInsert(int category)
        {
            StatusMessage("편성대상 카테고리 추가");

            try
            {				
                model.Init();
                model.Category = category;

                new MediaMenuSetManager( systemModel, commonModel).CategoryInsert( model );
                
                if (model.ResultCD.Equals("0000"))
                {
                    StatusMessage("편성대상 카테고리 추가 완료");
                }     
            }
            catch(FrameException fe)
            {
                FrameSystem.showMsgForm("편성대상 카테고리 추가 오류", new string[] {fe.ErrCode, fe.ResultMsg});
            }
            catch(Exception ex)
            {
                FrameSystem.showMsgForm("편성대상 카테고리 추가 오류",new string[] {"",ex.Message});
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
                    StatusMessage("편성대상 카테고리 삭제 완료");
                }     
            }
            catch(FrameException fe)
            {
                FrameSystem.showMsgForm("편성대상 카테고리 삭제 오류", new string[] {fe.ErrCode, fe.ResultMsg});
            }
            catch(Exception ex)
            {
                FrameSystem.showMsgForm("편성대상 카테고리 삭제 오류",new string[] {"",ex.Message});
            }
        }

        private void GenreInsert(int category,int genre)
        {
            StatusMessage("편성대상 장르 추가");

            try
            {				
                model.Init();
                model.Category  =   category;
                model.Genre     =   genre;

                new MediaMenuSetManager( systemModel, commonModel).GenreInsert( model );
                
                if (model.ResultCD.Equals("0000"))
                {
                    StatusMessage("편성대상 장르 추가 완료");
                }     
            }
            catch(FrameException fe)
            {
                FrameSystem.showMsgForm("편성대상 장르 추가 오류", new string[] {fe.ErrCode, fe.ResultMsg});
            }
            catch(Exception ex)
            {
                FrameSystem.showMsgForm("편성대상 장르 추가 오류",new string[] {"",ex.Message});
            }
        }

        private void GenreDelete(int category,int genre)
        {
            StatusMessage("편성대상 장르 삭제");

            try
            {				
                model.Init();
                model.Category  =   category;
                model.Genre     =   genre;

                new MediaMenuSetManager( systemModel, commonModel).GenreDelete( model );
                
                if (model.ResultCD.Equals("0000"))
                {
                    StatusMessage("편성대상 장르 삭제 완료");
                }     
            }
            catch(FrameException fe)
            {
                FrameSystem.showMsgForm("편성대상 장르 삭제 오류", new string[] {fe.ErrCode, fe.ResultMsg});
            }
            catch(Exception ex)
            {
                FrameSystem.showMsgForm("편성대상 장르 삭제 오류",new string[] {"",ex.Message});
            }
        }

        /// <summary>
        /// ScrollToRow 카테고리
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
                FrameSystem.showMsgForm("키캆오류", new string[] {fe.ErrCode, fe.ResultMsg});
            }
            catch(Exception ex)
            {
                FrameSystem.showMsgForm("키캆오류",new string[] {"",ex.Message});
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
                FrameSystem.showMsgForm("키캆오류", new string[] {fe.ErrCode, fe.ResultMsg});
            }
            catch(Exception ex)
            {
                FrameSystem.showMsgForm("키캆오류",new string[] {"",ex.Message});
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

            // 추가권한 검사
            //if(menu.CanCreate(MenuCode))    canCreate = true;
            // 조회권한 검사            
            if(menu.CanRead(MenuCode))      canRead = true;
            // 수정권한 검사
            //if(menu.CanUpdate(MenuCode))    canUpdate = true;

            // 삭제권한 검사
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
            // 검색조건의 콤보
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

        #region 카테고리 컬럼 버튼 클릭
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
                     * 편성대상인것을 제외하는 경우
                     * */
                    DialogResult result = MessageBox.Show("[" + categoryName + "] 카테고리를 편성대상에서 제외 합니다. ","편성대상설정",MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);

                    if( result == DialogResult.Yes )
                    {
                        ProgressStart();
                        this.CategoryDelete( int.Parse( categoryCode) );
                        this.SearchCategoryList();
                        this.ChoiceCategory( categoryCode );
                        ProgressStop();
                        StatusMessage( "작업완료!!!" );

                        //mediaMenuSet.Category.Rows[currRow].BeginEdit();
                        //mediaMenuSet.Category.Rows[currRow]["SetCategory"] = "False";
                        //mediaMenuSet.Category.Rows[currRow].EndEdit();
                    }
                    else
                    {
                        StatusMessage( "작업이 취소되었습니다!!!" );
                    }
                }
                else
                {
                    /*
                     * 신규로 편성대상에 추가하는 경우
                     * */
                    DialogResult    result;
                    if( setGenre > 0 || setChannel > 0 )
                    {
                        message = string.Format("[{0}] 카테고리를 편성대상으로 추가 합니다\n\n하위장르 혹은 채널에 편성대상이 설정된 상태이며, 해당 데이터는 삭제됩니다\n\n처리하시겠습니까? ", categoryName);
                        result = MessageBox.Show(message,"편성대상설정",MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
                    }
                    else
                    {
                        message = string.Format("[{0}] 카테고리를 편성대상으로 추가 합니다\n처리하시겠습니까? ", categoryName);
                        result = MessageBox.Show(message,"편성대상설정",MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
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
                        StatusMessage( "작업완료!!!" );
                    }
                    else
                    {
                        StatusMessage( "작업이 취소되었습니다!!!" );
                    }
                }
            }
        }

        #endregion

        #region 장르 컬럼 버튼 클릭
        /// <summary>
        /// 장르 컬럼버튼 클릭처리
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
                     * 카테고리 설정이 되어 있는 경우
                     * */
                    if( setGenre.Equals("True") )
                    {
                        /*
                        * 카테고리설정이 되어 있는 상태에서 장르편성대상을 제외하는 경우
                        * 이런 경우는 없다. 데이터 정합성이 잘못된 경우임
                        * */
                        message = string.Format("[{0}] 장르를 편성대상에서 제외 합니다\n상위카테고리가 편성대상으로 설정된 경우임으로\n 해당작업을 수행할 수 없습니다\n 담당자에게 문의 하십시요 ", genreName);
                        MessageBox.Show(message,"편성대상설정",MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    else
                    {
                        
                        if( setChannel > 0 )
                        {
                            /*
                             * 카테고리편성이 존재하고
                             * 채널편성이 존재하는 경우는 없다
                             * */
                            message = string.Format("[{0}] 장르를 편성대상에서 제외 합니다\n상위카테고리가 편성대상으로 설정된 경우임으로\n 해당작업을 수행할 수 없습니다\n 담당자에게 문의 하십시요 ", genreName);
                            MessageBox.Show(message,"편성대상설정",MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                        else
                        {
                            /*
                            * 카테고리설정이 되어 있는 상태에서 장르편성대상으로 추가하는 경우
                            * 하위채널에도 설정이 없는 경우
                            * 상위데이터는 삭제하고 처리하게 한다
                            * */
                            message = string.Format("[{0}] 장르를 편성대상으로 추가 합니다\n\n상위카테고리가 편성대상으로 설정된 경우임으로\n상위카테고리 편성대상은 삭제됩니다\n\n처리하시겠습니까? ", genreName);
                            result = MessageBox.Show(message,"편성대상설정",MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);

                            if( result == DialogResult.Yes )
                            {
                                ProgressStart();
                                this.GenreInsert( int.Parse( categoryCode), int.Parse( genreCode) );
                                this.SearchCategoryList();
                                this.ChoiceCategory( categoryCode );
                                this.ChoiceGenre( genreCode );
                                ProgressStop();
                                StatusMessage( "작업완료!!!" );
                            }
                            else
                            {
                                StatusMessage( "작업이 취소되었습니다!!!" );
                            }
                        }
                    }
                }
                else
                {
                    /*
                     * 카테고리 설정이 없는 경우
                     * */
                    if( setGenre.Equals("True") )
                    {
                        /*
                        * 장르편성대상을 제외하는 경우
                        * */
                        message = string.Format("[{0}] 장르를 편성대상에서 제외 합니다\n\n 처리하시겠습니까? ", genreName);
                        result = MessageBox.Show(message,"편성대상설정",MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);

                        if( result == DialogResult.Yes )
                        {
                            ProgressStart();
                            this.GenreDelete( int.Parse( categoryCode), int.Parse( genreCode) );
                            this.SearchCategoryList();
                            this.ChoiceCategory( categoryCode );
                            this.ChoiceGenre( genreCode );
                            ProgressStop();
                            StatusMessage( "작업완료!!!" );
                        }
                        else
                        {
                            StatusMessage( "작업이 취소되었습니다!!!" );
                        }
                    }
                    else
                    {
                        if( setChannel > 0 )
                        {
                            /*
                             * 채널편성대상으로 되어 있는 경우
                             * 장르편성대상으로 추가하는 경우
                             * 채널편성은 삭제한다
                             * 현재 사용않함
                             * */
                            message = string.Format("[{0}] 장르를 편성대상으로 추가 합니다\n\n하위 채널이 편성대상으로 설정되어 있슴으로, 채널은 삭제처리 됩니다. \n\n처리하시겠습니까? ", genreName);
                            result = MessageBox.Show(message,"편성대상설정",MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);

                            if( result == DialogResult.Yes )
                            {
                                ProgressStart();
                                this.GenreInsert( int.Parse( categoryCode), int.Parse( genreCode) );
                                this.SearchCategoryList();
                                this.ChoiceCategory( categoryCode );
                                this.ChoiceGenre( genreCode );
                                ProgressStop();
                                StatusMessage( "작업완료!!!" );
                            }
                            else
                            {
                                StatusMessage( "작업이 취소되었습니다!!!" );
                            }
                        }
                        else
                        {
                            /*
                             * 장르편성대상으로 추가하는 경우
                             * */
                            message = string.Format("[{0}] 장르를 편성대상으로 추가 합니다\n\n처리하시겠습니까? ", genreName);
                            result = MessageBox.Show(message,"편성대상설정",MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);

                            if( result == DialogResult.Yes )
                            {
                                ProgressStart();
                                this.GenreInsert( int.Parse( categoryCode), int.Parse( genreCode) );
                                this.SearchCategoryList();
                                this.ChoiceCategory( categoryCode );
                                this.ChoiceGenre( genreCode );
                                ProgressStop();
                                StatusMessage( "작업완료!!!" );
                            }
                            else
                            {
                                StatusMessage( "작업이 취소되었습니다!!!" );
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
                FrameSystem.showMsgForm("편성대상 조회 오류", new string[] {ex.Source, ex.Message});
            }
        }
    }
}
