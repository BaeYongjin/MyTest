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
	/// GenrePopForm에 대한 요약 설명입니다.
	/// </summary>
	/// 
    
	public class GenrePopForm1 : System.Windows.Forms.Form
	{

		#region 이벤트핸들러
		public event StatusEventHandler     StatusEvent;			// 상태이벤트 핸들러
        public event ProgressEventHandler   ProgressEvent;			// 처리중이벤트 핸들러

		#endregion
			
		#region 사용자정의 객체 및 변수

		// 시스템 정보 : 화면공통
		private SystemModel   systemModel   = FrameSystem.oSysModel;
		private CommonModel   commonModel   = FrameSystem.oComModel;
		private Logger        log           = FrameSystem.oLog;

		// 사용할 정보모델
		GroupModel groupModel  = new GroupModel();	// 컨텐츠정보모델
				
		GroupControl GenreCtl = null;

		private	int		keyCategory		= 0;
		private	string	keyCategoryNm	= "";

		// 화면처리용 변수
        bool IsNewSearchKey = true;					// 검색어입력 여부
		CurrencyManager ccm        = null;			// 데이터 그리드의 변경에 따른 데이터셋 관리를 위하여			
		DataTable       dtt        = null;
		
		#endregion

        private System.Data.DataView dvGenre;
        private AdManagerClient._10_Media._10_Group.GroupDs groupDs;
        private System.Data.DataView dvCategory;
        private Janus.Windows.GridEX.EditControls.EditBox ebSearchKey;
        private Janus.Windows.EditControls.UIButton btnSearch;
        private Janus.Windows.UI.Dock.UIPanelManager pnlManager;
        private Janus.Windows.UI.Dock.UIPanelGroup pnlList;
        private Janus.Windows.UI.Dock.UIPanel pnlBottom;
        private Janus.Windows.UI.Dock.UIPanelInnerContainer pnlBottomContainer;
        private Janus.Windows.UI.Dock.UIPanel pnlSearch;
        private Janus.Windows.UI.Dock.UIPanelInnerContainer pnlSearchContainer;
        private Janus.Windows.UI.Dock.UIPanel pnlCategory;
        private Janus.Windows.UI.Dock.UIPanelInnerContainer pnlCategoryContainer;
        private Janus.Windows.GridEX.GridEX grdExCategoryList;
        private Janus.Windows.UI.Dock.UIPanel pnlGenre;
        private Janus.Windows.UI.Dock.UIPanelInnerContainer pnlGenreContainer;
        private Janus.Windows.GridEX.GridEX grdExGenreList;
        private Janus.Windows.EditControls.UIButton btnClose;
        private Janus.Windows.EditControls.UIButton btnOk;
        private Label label1;
        private Janus.Windows.EditControls.UICheckBox chkInvalidMenu;
        private IContainer components;
		//		private System.ComponentModel.IContainer components;

		/// <summary>
		/// 데이터 넘겨야 할 넘
		/// </summary>
		/// <param name="sender"></param>
		public GenrePopForm1(GroupControl sender)
		{
			//
			// Windows Form 디자이너 지원에 필요합니다.
			//
			InitializeComponent();
			GenreCtl = sender;
		}

		/// <summary>
		/// 일반사용자
		/// </summary>
		public GenrePopForm1()
		{
			//
			// Windows Form 디자이너 지원에 필요합니다.
			//
			InitializeComponent();
			GenreCtl = null;
		}

		/// <summary>
		/// 사용 중인 모든 리소스를 정리합니다.
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

		#region Windows Form 디자이너에서 생성한 코드
		/// <summary>
		/// 디자이너 지원에 필요한 메서드입니다.
		/// 이 메서드의 내용을 코드 편집기로 수정하지 마십시오.
		/// </summary>
		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            Janus.Windows.GridEX.GridEXLayout grdExCategoryList_Layout_0 = new Janus.Windows.GridEX.GridEXLayout();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GenrePopForm1));
            Janus.Windows.GridEX.GridEXLayout grdExGenreList_Layout_0 = new Janus.Windows.GridEX.GridEXLayout();
            this.dvCategory = new System.Data.DataView();
            this.groupDs = new AdManagerClient._10_Media._10_Group.GroupDs();
            this.dvGenre = new System.Data.DataView();
            this.ebSearchKey = new Janus.Windows.GridEX.EditControls.EditBox();
            this.btnSearch = new Janus.Windows.EditControls.UIButton();
            this.pnlManager = new Janus.Windows.UI.Dock.UIPanelManager(this.components);
            this.pnlBottom = new Janus.Windows.UI.Dock.UIPanel();
            this.pnlBottomContainer = new Janus.Windows.UI.Dock.UIPanelInnerContainer();
            this.btnClose = new Janus.Windows.EditControls.UIButton();
            this.btnOk = new Janus.Windows.EditControls.UIButton();
            this.pnlSearch = new Janus.Windows.UI.Dock.UIPanel();
            this.pnlSearchContainer = new Janus.Windows.UI.Dock.UIPanelInnerContainer();
            this.chkInvalidMenu = new Janus.Windows.EditControls.UICheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.pnlList = new Janus.Windows.UI.Dock.UIPanelGroup();
            this.pnlCategory = new Janus.Windows.UI.Dock.UIPanel();
            this.pnlCategoryContainer = new Janus.Windows.UI.Dock.UIPanelInnerContainer();
            this.grdExCategoryList = new Janus.Windows.GridEX.GridEX();
            this.pnlGenre = new Janus.Windows.UI.Dock.UIPanel();
            this.pnlGenreContainer = new Janus.Windows.UI.Dock.UIPanelInnerContainer();
            this.grdExGenreList = new Janus.Windows.GridEX.GridEX();
            ((System.ComponentModel.ISupportInitialize)(this.dvCategory)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupDs)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dvGenre)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pnlManager)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pnlBottom)).BeginInit();
            this.pnlBottom.SuspendLayout();
            this.pnlBottomContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pnlSearch)).BeginInit();
            this.pnlSearch.SuspendLayout();
            this.pnlSearchContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pnlList)).BeginInit();
            this.pnlList.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pnlCategory)).BeginInit();
            this.pnlCategory.SuspendLayout();
            this.pnlCategoryContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdExCategoryList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pnlGenre)).BeginInit();
            this.pnlGenre.SuspendLayout();
            this.pnlGenreContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdExGenreList)).BeginInit();
            this.SuspendLayout();
            // 
            // dvCategory
            // 
            this.dvCategory.Table = this.groupDs.Category;
            // 
            // groupDs
            // 
            this.groupDs.DataSetName = "GroupDs";
            this.groupDs.Locale = new System.Globalization.CultureInfo("en-US");
            this.groupDs.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // dvGenre
            // 
            this.dvGenre.Table = this.groupDs.Genre;
            // 
            // ebSearchKey
            // 
            this.ebSearchKey.ButtonFont = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ebSearchKey.Location = new System.Drawing.Point(393, 4);
            this.ebSearchKey.Name = "ebSearchKey";
            this.ebSearchKey.Size = new System.Drawing.Size(184, 22);
            this.ebSearchKey.TabIndex = 5;
            this.ebSearchKey.Text = "검색어를 입력하세요";
            this.ebSearchKey.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
            this.ebSearchKey.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            this.ebSearchKey.TextChanged += new System.EventHandler(this.ebSearchKey_TextChanged);
            this.ebSearchKey.Click += new System.EventHandler(this.ebSearchKey_Click);
            this.ebSearchKey.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ebSearchKey_KeyDown);
            // 
            // btnSearch
            // 
            this.btnSearch.BackColor = System.Drawing.Color.White;
            this.btnSearch.Location = new System.Drawing.Point(583, 3);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(104, 24);
            this.btnSearch.TabIndex = 4;
            this.btnSearch.Text = "조 회";
            this.btnSearch.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // pnlManager
            // 
            this.pnlManager.ContainerControl = this;
            this.pnlBottom.Id = new System.Guid("e8893766-d7af-4795-9450-7d55f1d69c77");
            this.pnlManager.Panels.Add(this.pnlBottom);
            this.pnlSearch.Id = new System.Guid("3346425a-e902-4fe0-8dcf-fe3c1656e731");
            this.pnlManager.Panels.Add(this.pnlSearch);
            this.pnlList.Id = new System.Guid("4d0e2986-0227-4880-b4e9-2b3c87dc6895");
            this.pnlList.StaticGroup = true;
            this.pnlCategory.Id = new System.Guid("5ac8e84b-458c-4075-828c-014a6cae089e");
            this.pnlList.Panels.Add(this.pnlCategory);
            this.pnlGenre.Id = new System.Guid("acedd7fa-cb43-4150-b959-1e40c6b99c94");
            this.pnlList.Panels.Add(this.pnlGenre);
            this.pnlManager.Panels.Add(this.pnlList);
            // 
            // Design Time Panel Info:
            // 
            this.pnlManager.BeginPanelInfo();
            this.pnlManager.AddDockPanelInfo(new System.Guid("e8893766-d7af-4795-9450-7d55f1d69c77"), Janus.Windows.UI.Dock.PanelDockStyle.Bottom, new System.Drawing.Size(703, 35), true);
            this.pnlManager.AddDockPanelInfo(new System.Guid("4d0e2986-0227-4880-b4e9-2b3c87dc6895"), Janus.Windows.UI.Dock.PanelGroupStyle.VerticalTiles, Janus.Windows.UI.Dock.PanelDockStyle.Fill, true, new System.Drawing.Size(703, 501), true);
            this.pnlManager.AddDockPanelInfo(new System.Guid("5ac8e84b-458c-4075-828c-014a6cae089e"), new System.Guid("4d0e2986-0227-4880-b4e9-2b3c87dc6895"), 280, true);
            this.pnlManager.AddDockPanelInfo(new System.Guid("acedd7fa-cb43-4150-b959-1e40c6b99c94"), new System.Guid("4d0e2986-0227-4880-b4e9-2b3c87dc6895"), 419, true);
            this.pnlManager.AddDockPanelInfo(new System.Guid("3346425a-e902-4fe0-8dcf-fe3c1656e731"), Janus.Windows.UI.Dock.PanelDockStyle.Top, new System.Drawing.Size(703, 35), true);
            this.pnlManager.AddFloatingPanelInfo(new System.Guid("3346425a-e902-4fe0-8dcf-fe3c1656e731"), new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
            this.pnlManager.AddFloatingPanelInfo(new System.Guid("e8893766-d7af-4795-9450-7d55f1d69c77"), new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
            this.pnlManager.AddFloatingPanelInfo(new System.Guid("4d0e2986-0227-4880-b4e9-2b3c87dc6895"), Janus.Windows.UI.Dock.PanelGroupStyle.VerticalTiles, true, new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
            this.pnlManager.AddFloatingPanelInfo(new System.Guid("5ac8e84b-458c-4075-828c-014a6cae089e"), new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
            this.pnlManager.AddFloatingPanelInfo(new System.Guid("acedd7fa-cb43-4150-b959-1e40c6b99c94"), new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
            this.pnlManager.AddFloatingPanelInfo(new System.Guid("c35a9899-5ef0-4317-a97d-8ee13c8ea75e"), Janus.Windows.UI.Dock.PanelGroupStyle.HorizontalTiles, true, new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
            this.pnlManager.EndPanelInfo();
            // 
            // pnlBottom
            // 
            this.pnlBottom.BorderCaption = Janus.Windows.UI.InheritableBoolean.False;
            this.pnlBottom.BorderPanel = Janus.Windows.UI.InheritableBoolean.False;
            this.pnlBottom.CaptionVisible = Janus.Windows.UI.InheritableBoolean.False;
            this.pnlBottom.InnerContainer = this.pnlBottomContainer;
            this.pnlBottom.Location = new System.Drawing.Point(5, 541);
            this.pnlBottom.Name = "pnlBottom";
            this.pnlBottom.Size = new System.Drawing.Size(703, 35);
            this.pnlBottom.TabIndex = 4;
            // 
            // pnlBottomContainer
            // 
            this.pnlBottomContainer.Controls.Add(this.btnClose);
            this.pnlBottomContainer.Controls.Add(this.btnOk);
            this.pnlBottomContainer.Location = new System.Drawing.Point(0, 4);
            this.pnlBottomContainer.Name = "pnlBottomContainer";
            this.pnlBottomContainer.Size = new System.Drawing.Size(703, 31);
            this.pnlBottomContainer.TabIndex = 0;
            // 
            // btnClose
            // 
            this.btnClose.BackColor = System.Drawing.SystemColors.Control;
            this.btnClose.Location = new System.Drawing.Point(348, 5);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(64, 23);
            this.btnClose.TabIndex = 10;
            this.btnClose.Text = "닫기";
            this.btnClose.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnOk
            // 
            this.btnOk.BackColor = System.Drawing.SystemColors.Control;
            this.btnOk.Location = new System.Drawing.Point(268, 5);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(64, 23);
            this.btnOk.TabIndex = 11;
            this.btnOk.Text = "편성";
            this.btnOk.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // pnlSearch
            // 
            this.pnlSearch.BackColor = System.Drawing.SystemColors.Control;
            this.pnlSearch.BorderCaption = Janus.Windows.UI.InheritableBoolean.False;
            this.pnlSearch.BorderCaptionColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.pnlSearch.BorderPanel = Janus.Windows.UI.InheritableBoolean.False;
            this.pnlSearch.CaptionStyle = Janus.Windows.UI.Dock.PanelCaptionStyle.Light;
            this.pnlSearch.CaptionVisible = Janus.Windows.UI.InheritableBoolean.False;
            this.pnlSearch.InnerContainer = this.pnlSearchContainer;
            this.pnlSearch.Location = new System.Drawing.Point(5, 5);
            this.pnlSearch.Name = "pnlSearch";
            this.pnlSearch.Size = new System.Drawing.Size(703, 35);
            this.pnlSearch.TabIndex = 4;
            // 
            // pnlSearchContainer
            // 
            this.pnlSearchContainer.Controls.Add(this.chkInvalidMenu);
            this.pnlSearchContainer.Controls.Add(this.label1);
            this.pnlSearchContainer.Controls.Add(this.btnSearch);
            this.pnlSearchContainer.Controls.Add(this.ebSearchKey);
            this.pnlSearchContainer.Location = new System.Drawing.Point(0, 0);
            this.pnlSearchContainer.Name = "pnlSearchContainer";
            this.pnlSearchContainer.Size = new System.Drawing.Size(703, 31);
            this.pnlSearchContainer.TabIndex = 0;
            // 
            // chkInvalidMenu
            // 
            this.chkInvalidMenu.Location = new System.Drawing.Point(3, 4);
            this.chkInvalidMenu.Name = "chkInvalidMenu";
            this.chkInvalidMenu.ShowFocusRectangle = false;
            this.chkInvalidMenu.Size = new System.Drawing.Size(101, 26);
            this.chkInvalidMenu.TabIndex = 48;
            this.chkInvalidMenu.Text = "무효메뉴포함";
            this.chkInvalidMenu.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.chkInvalidMenu.CheckedChanged += new System.EventHandler(this.chkInvalidMenu_CheckedChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(312, 8);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(74, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "2차메뉴검색 :";
            // 
            // pnlList
            // 
            this.pnlList.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.pnlList.BorderPanel = Janus.Windows.UI.InheritableBoolean.False;
            this.pnlList.CaptionDisplayMode = Janus.Windows.UI.Dock.PanelCaptionDisplayMode.Text;
            this.pnlList.CaptionVisible = Janus.Windows.UI.InheritableBoolean.False;
            this.pnlList.GroupStyle = Janus.Windows.UI.Dock.PanelGroupStyle.VerticalTiles;
            this.pnlList.Location = new System.Drawing.Point(5, 40);
            this.pnlList.Name = "pnlList";
            this.pnlList.Size = new System.Drawing.Size(703, 501);
            this.pnlList.TabIndex = 4;
            // 
            // pnlCategory
            // 
            this.pnlCategory.BorderCaption = Janus.Windows.UI.InheritableBoolean.False;
            this.pnlCategory.BorderPanel = Janus.Windows.UI.InheritableBoolean.False;
            this.pnlCategory.CaptionVisible = Janus.Windows.UI.InheritableBoolean.False;
            this.pnlCategory.InnerContainer = this.pnlCategoryContainer;
            this.pnlCategory.Location = new System.Drawing.Point(0, 0);
            this.pnlCategory.Name = "pnlCategory";
            this.pnlCategory.Size = new System.Drawing.Size(280, 501);
            this.pnlCategory.TabIndex = 4;
            // 
            // pnlCategoryContainer
            // 
            this.pnlCategoryContainer.Controls.Add(this.grdExCategoryList);
            this.pnlCategoryContainer.Location = new System.Drawing.Point(0, 0);
            this.pnlCategoryContainer.Name = "pnlCategoryContainer";
            this.pnlCategoryContainer.Size = new System.Drawing.Size(280, 501);
            this.pnlCategoryContainer.TabIndex = 0;
            // 
            // grdExCategoryList
            // 
            this.grdExCategoryList.AllowColumnDrag = false;
            this.grdExCategoryList.AllowEdit = Janus.Windows.GridEX.InheritableBoolean.False;
            this.grdExCategoryList.AlternatingColors = true;
            this.grdExCategoryList.AlternatingRowFormatStyle.BackColor = System.Drawing.Color.Gainsboro;
            this.grdExCategoryList.DataSource = this.dvCategory;
            this.grdExCategoryList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdExCategoryList.EmptyRows = true;
            this.grdExCategoryList.FocusCellFormatStyle.BackColor = System.Drawing.SystemColors.Highlight;
            this.grdExCategoryList.FocusCellFormatStyle.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.grdExCategoryList.FocusStyle = Janus.Windows.GridEX.FocusStyle.None;
            this.grdExCategoryList.Font = new System.Drawing.Font("맑은 고딕", 8.25F);
            this.grdExCategoryList.GridLines = Janus.Windows.GridEX.GridLines.Vertical;
            this.grdExCategoryList.GridLineStyle = Janus.Windows.GridEX.GridLineStyle.Solid;
            this.grdExCategoryList.GroupByBoxVisible = false;
            this.grdExCategoryList.HeaderFormatStyle.TextAlignment = Janus.Windows.GridEX.TextAlignment.Center;
            this.grdExCategoryList.HideSelection = Janus.Windows.GridEX.HideSelection.HighlightInactive;
            grdExCategoryList_Layout_0.DataSource = this.dvCategory;
            grdExCategoryList_Layout_0.IsCurrentLayout = true;
            grdExCategoryList_Layout_0.Key = "bae";
            grdExCategoryList_Layout_0.LayoutString = resources.GetString("grdExCategoryList_Layout_0.LayoutString");
            this.grdExCategoryList.Layouts.AddRange(new Janus.Windows.GridEX.GridEXLayout[] {
            grdExCategoryList_Layout_0});
            this.grdExCategoryList.Location = new System.Drawing.Point(0, 0);
            this.grdExCategoryList.Name = "grdExCategoryList";
            this.grdExCategoryList.ScrollBars = Janus.Windows.GridEX.ScrollBars.Vertical;
            this.grdExCategoryList.ScrollBarWidth = 0;
            this.grdExCategoryList.SelectedInactiveFormatStyle.BackColor = System.Drawing.Color.Gold;
            this.grdExCategoryList.Size = new System.Drawing.Size(280, 501);
            this.grdExCategoryList.TabIndex = 26;
            this.grdExCategoryList.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            // 
            // pnlGenre
            // 
            this.pnlGenre.BorderPanel = Janus.Windows.UI.InheritableBoolean.False;
            this.pnlGenre.CaptionVisible = Janus.Windows.UI.InheritableBoolean.False;
            this.pnlGenre.InnerContainer = this.pnlGenreContainer;
            this.pnlGenre.Location = new System.Drawing.Point(284, 0);
            this.pnlGenre.Name = "pnlGenre";
            this.pnlGenre.Size = new System.Drawing.Size(419, 501);
            this.pnlGenre.TabIndex = 4;
            // 
            // pnlGenreContainer
            // 
            this.pnlGenreContainer.Controls.Add(this.grdExGenreList);
            this.pnlGenreContainer.Location = new System.Drawing.Point(0, 0);
            this.pnlGenreContainer.Name = "pnlGenreContainer";
            this.pnlGenreContainer.Size = new System.Drawing.Size(419, 501);
            this.pnlGenreContainer.TabIndex = 0;
            // 
            // grdExGenreList
            // 
            this.grdExGenreList.AllowEdit = Janus.Windows.GridEX.InheritableBoolean.False;
            this.grdExGenreList.AlternatingColors = true;
            this.grdExGenreList.DataSource = this.dvGenre;
            this.grdExGenreList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdExGenreList.EmptyRows = true;
            this.grdExGenreList.FocusCellFormatStyle.BackColor = System.Drawing.SystemColors.Highlight;
            this.grdExGenreList.FocusCellFormatStyle.FontBold = Janus.Windows.GridEX.TriState.True;
            this.grdExGenreList.FocusCellFormatStyle.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.grdExGenreList.FocusStyle = Janus.Windows.GridEX.FocusStyle.None;
            this.grdExGenreList.Font = new System.Drawing.Font("맑은 고딕", 8.25F);
            this.grdExGenreList.GridLines = Janus.Windows.GridEX.GridLines.Vertical;
            this.grdExGenreList.GridLineStyle = Janus.Windows.GridEX.GridLineStyle.Solid;
            this.grdExGenreList.GroupByBoxVisible = false;
            this.grdExGenreList.HeaderFormatStyle.TextAlignment = Janus.Windows.GridEX.TextAlignment.Center;
            grdExGenreList_Layout_0.DataSource = this.dvGenre;
            grdExGenreList_Layout_0.IsCurrentLayout = true;
            grdExGenreList_Layout_0.Key = "bae";
            grdExGenreList_Layout_0.LayoutString = resources.GetString("grdExGenreList_Layout_0.LayoutString");
            this.grdExGenreList.Layouts.AddRange(new Janus.Windows.GridEX.GridEXLayout[] {
            grdExGenreList_Layout_0});
            this.grdExGenreList.Location = new System.Drawing.Point(0, 0);
            this.grdExGenreList.Name = "grdExGenreList";
            this.grdExGenreList.ScrollBars = Janus.Windows.GridEX.ScrollBars.Vertical;
            this.grdExGenreList.SelectedFormatStyle.FontBold = Janus.Windows.GridEX.TriState.True;
            this.grdExGenreList.SelectedInactiveFormatStyle.BackColor = System.Drawing.Color.Gold;
            this.grdExGenreList.Size = new System.Drawing.Size(419, 501);
            this.grdExGenreList.TabIndex = 18;
            this.grdExGenreList.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            this.grdExGenreList.RowDoubleClick += new Janus.Windows.GridEX.RowActionEventHandler(this.grdExGenreList_RowDoubleClick);
            // 
            // GenrePopForm1
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(6, 15);
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.ClientSize = new System.Drawing.Size(713, 581);
            this.Controls.Add(this.pnlList);
            this.Controls.Add(this.pnlSearch);
            this.Controls.Add(this.pnlBottom);
            this.Font = new System.Drawing.Font("맑은 고딕", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "GenrePopForm1";
            this.Padding = new System.Windows.Forms.Padding(2);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "2차메뉴(장르)목록검색";
            this.Load += new System.EventHandler(this.GenrePopForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dvCategory)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupDs)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dvGenre)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pnlManager)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pnlBottom)).EndInit();
            this.pnlBottom.ResumeLayout(false);
            this.pnlBottomContainer.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pnlSearch)).EndInit();
            this.pnlSearch.ResumeLayout(false);
            this.pnlSearchContainer.ResumeLayout(false);
            this.pnlSearchContainer.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pnlList)).EndInit();
            this.pnlList.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pnlCategory)).EndInit();
            this.pnlCategory.ResumeLayout(false);
            this.pnlCategoryContainer.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdExCategoryList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pnlGenre)).EndInit();
            this.pnlGenre.ResumeLayout(false);
            this.pnlGenreContainer.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdExGenreList)).EndInit();
            this.ResumeLayout(false);

		}
		#endregion

		#region 컨트롤 로드
		private void GenrePopForm_Load(object sender, System.EventArgs e)
		{
            
			// 데이터관리용 객체생성
			dtt = ((DataView)grdExCategoryList.DataSource).Table;  
			ccm = (CurrencyManager) this.BindingContext[grdExCategoryList.DataSource]; 
			ccm.PositionChanged += new EventHandler(OnGrdRowChanged);

			// 컨트롤 초기화
			InitControl();
			SearchCategory();
			OnGrdRowChanged(null,null);
		}
		#endregion

		#region 컨트롤 초기화
		private void InitControl()
		{
													
		}
		
		#endregion
  
		#region 처리메소드

		private void SearchCategory()
		{
			StatusMessage("카테고리 정보를 조회합니다.");
			try
			{
                
                //무효 메뉴 체크판단 - 카테 고리는 다가져온다.
                if (chkInvalidMenu.Checked)
                {
                    groupModel.InvalidYn = true;
                }
                else
                {
                    groupModel.InvalidYn = false;
                }

                //검색 기준은 장르 - 장르명에 검색어가 포함된 장르 목록을 가져온다.
                groupModel.SearchType = "G";

                if (IsNewSearchKey) groupModel.SearchKey = "";
                else groupModel.SearchKey = ebSearchKey.Text.Trim();
                
                new GroupManager(systemModel, commonModel).GetCategoryList2(groupModel);

				if (groupModel.ResultCD.Equals("0000"))
				{
					Utility.SetDataTable(groupDs.Category, groupModel.CategoryDataSet);				
					StatusMessage(groupModel.ResultCnt + "건의 카테고리 정보가 조회되었습니다.");
				}
			}
			catch(FrameException fe)
			{
				FrameSystem.showMsgForm("카테고리조회오류", new string[] {fe.ErrCode, fe.ResultMsg});
			}
			catch(Exception ex)
			{
				FrameSystem.showMsgForm("카테고리조회오류",new string[] {"",ex.Message});
			}
		}

		/// <summary>
		/// 장르코드를 읽어온다
		/// </summary>
		private void SearchGenre()
		{
			try
			{
				groupDs.Genre.Clear();
				groupModel.CategoryCode  = keyCategory.ToString();

                //검색 기준은 장르 - 장르명에 검색어가 포함된 장르 목록을 가져온다.
                groupModel.SearchType = "G";

                if (IsNewSearchKey) groupModel.SearchKey = "";
                else groupModel.SearchKey = ebSearchKey.Text.Trim();


                //무효 메뉴 체크판단
                if (chkInvalidMenu.Checked)
                {
                    groupModel.InvalidYn = true;
                }
                else
                {
                    groupModel.InvalidYn = false;
                }

				new GroupManager(systemModel,commonModel).GetGenreList2(groupModel);

				if (groupModel.ResultCD.Equals("0000"))
				{
					Utility.SetDataTable(groupDs.Genre, groupModel.GenreDataSet);				
				}
			}
			catch(FrameException fe)
			{
				FrameSystem.showMsgForm("장르조회오류", new string[] {fe.ErrCode, fe.ResultMsg});
			}
			catch(Exception ex)
			{
				FrameSystem.showMsgForm("장르조회오류",new string[] {"",ex.Message});
			}
		}
		#endregion

		#region 이벤트함수


        /// <summary>
        /// 검색어 변경
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// 
        private void ebSearchKey_Click(object sender, System.EventArgs e)
        {
            if (IsNewSearchKey)
            {
                ebSearchKey.Text = "";
            }
            else
            {
                ebSearchKey.SelectAll();
            }
        }

        /// <summary>
        /// 검색어 클릭 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ebSearchKey_TextChanged(object sender, System.EventArgs e)
        {
            IsNewSearchKey = false;
        }

        private void ebSearchKey_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SearchCategory();
                OnGrdRowChanged(null, null);
            }
        }

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
                ea.Type = ProgressEventArgs.Start;
                ProgressEvent(this, ea);
            }
        }

        private void ProgressStop()
        {
            if (ProgressEvent != null)
            {
                ProgressEventArgs ea = new ProgressEventArgs();
                ea.Type = ProgressEventArgs.Stop;
                ProgressEvent(this, ea);
            }
        }

		private void btnOk_Click(object sender, System.EventArgs e)
		{
			//string newKey = grdExGenreList.SelectedItems[0].GetRow().Cells["GenreCode"].Value.ToString();			
			//string newKey_N = grdExGenreList.SelectedItems[0].GetRow().Cells["GenreName"].Value.ToString();			
			//this.GenreCtl.SetGenre(keyCategory.ToString(), newKey);


            ProgressStart();

            try
            {
                int rc = 0;
                string newKey = "";

                for (int i = 0; i < grdExGenreList.RowCount; i++)
                {

                    if (grdExGenreList.GetRows()[i].Cells["CheckYn"].Value.ToString().Equals("True"))
                    {
                        newKey = grdExGenreList.GetRows()[i].Cells["GenreCode"].Value.ToString();

                        if (!this.GenreCtl.SetGenre(keyCategory.ToString(), newKey))
                            return;

                        rc++;
                    }
                }

                if (rc > 0)
                    MessageBox.Show("1차메뉴 [" + keyCategory + "] " + keyCategoryNm + " 의 [" + rc + "]건의 2차메뉴 등록 완료", "2차메뉴(장르) 등록", MessageBoxButtons.OK, MessageBoxIcon.Information);
                else return;

                ProgressStop();

            }
            catch (Exception ex)
            {
                FrameSystem.showMsgForm("2차메뉴 등록 오류", new string[] { "", ex.Message });
            }
            finally
            {
                ProgressStop();
            }


			//this.Close();
		}
     


		private void btnClose_Click(object sender, System.EventArgs e)
		{
			this.Close();
		}

		private void grdExGenreList_RowDoubleClick(object sender, Janus.Windows.GridEX.RowActionEventArgs e)
		{						
			string newKey = grdExGenreList.SelectedItems[0].GetRow().Cells["GenreCode"].Value.ToString();			
			string newKey_N = grdExGenreList.SelectedItems[0].GetRow().Cells["GenreName"].Value.ToString();			
			this.GenreCtl.SetGenre(keyCategory.ToString(), newKey);
			//this.Close();
		}

		/// <summary>
		/// 카테고리Row의 Position이 변경될때 발생한다
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void OnGrdRowChanged(object sender, EventArgs e)
		{
			int curRow = ccm.Position;

            if (curRow >= 0)
            {
                keyCategory = int.Parse(dtt.Rows[curRow]["CategoryCode"].ToString());
                keyCategoryNm = dtt.Rows[curRow]["CategoryName"].ToString();

                this.SearchGenre();
            }
            else
            {
                keyCategory = 0;
                keyCategoryNm = "";

                this.SearchGenre();
            }
		}

        private void btnSearch_Click(object sender, EventArgs e)
        {
            SearchCategory();
            OnGrdRowChanged(null, null);

            /*
            int curRow = ccm.Position;

            if (curRow >= 0)
            {
                keyCategory = int.Parse(dtt.Rows[curRow]["CategoryCode"].ToString());
                keyCategoryNm = dtt.Rows[curRow]["CategoryName"].ToString();

                this.SearchGenre();
            }
             * */
        }

  
        private void chkInvalidMenu_CheckedChanged(object sender, EventArgs e)
        {
            SearchCategory();
            OnGrdRowChanged(null, null);
        }

   
       
	}
   

	#endregion


}
