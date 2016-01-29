// ===============================================================================
// SchAppendAdControl for Charites Project
//
// SchAppendAdControl.cs
//
// Ãß°¡±¤°íÆí¼º ÄÁÆ®·ÑÀ» Á¤ÀÇÇÕ´Ï´Ù. 
//
// ===============================================================================
// Release history
// 2007.10.02 RH.Jung ÆÄÀÏ¸®½ºÆ®°Ç¼ö °Ë»ç
// ===============================================================================
// Copyright (C) 2006 G-Inno Systems Inc.
// All rights reserved.
// 
using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using System.Threading;

using WinFramework.Misc;
using WinFramework.Base;
using WinFramework.Data;

using AdManagerModel;

namespace AdManagerClient
{
	/// <summary>
	/// ±¤°íÆÄÀÏ°ü¸® ÄÁÆ®·Ñ
	/// </summary>
    public class SchAppendAdControl : System.Windows.Forms.UserControl, IUserControl
	{
		#region ÀÌº¥Æ®ÇÚµé·¯
		public event StatusEventHandler 			StatusEvent;			// »óÅÂÀÌº¥Æ® ÇÚµé·¯
		public event ProgressEventHandler 			ProgressEvent;			// Ã³¸®ÁßÀÌº¥Æ® ÇÚµé·¯
		#endregion
			
		#region »ç¿ëÀÚÁ¤ÀÇ °´Ã¼ ¹× º¯¼ö

		// ½Ã½ºÅÛ Á¤º¸ : È­¸é°øÅë
		private SystemModel   systemModel   = FrameSystem.oSysModel;
		private CommonModel   commonModel   = FrameSystem.oComModel;
		private Logger        log           = FrameSystem.oLog;
		private MenuPower     menu          = FrameSystem.oMenu;

		// ¸Þ´ºÄÚµå : º¸¾ÈÀÌ ÇÊ¿äÇÑ È­¸é¿¡ ÇÊ¿äÇÔ
		public string        menuCode		= "";

		// »ç¿ëÇÒ Á¤º¸¸ðµ¨
		SchAppendAdModel schAppendAdModel    = new SchAppendAdModel();	// Ãß°¡±¤°íÆí¼º¸ðµ¨
		SchPublishModel schPublishModel  = new SchPublishModel();	// ±¤°í½ÂÀÎ¸ðµ¨

		// È­¸éÃ³¸®¿ë º¯¼ö
		CurrencyManager cm        = null;					// µ¥ÀÌÅÍ ±×¸®µåÀÇ º¯°æ¿¡ µû¸¥ µ¥ÀÌÅÍ¼Â °ü¸®¸¦ À§ÇÏ¿©			
		DataTable       dt        = null;

        bool IsSearching = false; // Á¶È¸Áß »ó¼¼È­¸éÀÌ ¾÷µ¥ÀÌÆ® µÇ´Â °ÍÀ» ¹æÁö ÇÏ±âÀ§ÇÔ 2011.11.29 JH.Park
		bool canRead			  = false;
		bool canUpdate			  = false;
		bool canCreate            = false;
		bool canDelete            = false;

		// Key µ¥ÀÌÅÍ
		string keyMediaCode     = "";
		string keyItemNo        = "";
		string keyItemName      = "";
		public string keyScheduleOrder = "";		// ÆË¾÷¿¡¼­µµ »ç¿ëÇÏ±âÀ§ÇØ  public
		string keyLastOrder     = "";

		string defMedaiCode     = "";			   //  Æí¼º´ë»ó±¤°í¸¦ À§ÇÑ ¸ÅÃ¼ÄÚµå. Æí¼º¸ñ·ÏÁ¶È¸½Ã ¼ÂÆ®

		// Æí¼º¹èÆ÷ ½ÂÀÎ»óÅÂ Ã³¸®¿ë
		private string keyAckNo    = "";
		private string keyAckState = "";

		// ¼øÀ§º¯°æ±¸ºÐ
		const int ORDER_FIRST = 1;
		const int ORDER_LAST  = 2;
		const int ORDER_UP    = 3;
		const int ORDER_DOWN  = 4;

		// 
		private	const int	FILEMAX	= 430;	// ÃÖ´ë ÆÄÀÏ¸®½ºÆ® °Ç¼ö >= ÇöÀçÆÄÀÏ¸®½ºÆ® = Ãß°¡±¤°í°Ç¼ö + CDN¹èÆ÷¿Ï·á »óÅÂÆÄÀÏ °Ç¼ö
		private int FileListCnt = 0;		// ÇöÀçÆÄÀÏ¸®½ºÆ® °Ç¼ö
		
		#endregion

        #region IUserControl ±¸Çö
        /// <summary>
        /// ¸Þ´º ÄÚµå-º¸¾ÈÀÌ ÇÊ¿äÇÑ È­¸é¿¡ ÇÊ¿äÇÔ
        /// </summary>
        public string MenuCode
        {
            set { this.menuCode = value; }
            get { return this.menuCode; }
        }

        /// <summary>
        /// ºÎ¸ðÄÁÆ®·Ñ ÁöÁ¤
        /// </summary>
        /// <param name="control"></param>
        public void SetParent(Control control)
        {
            this.Parent = control;
        }
        /// <summary>
        /// DockStypeÁöÁ¤
        /// </summary>
        /// <param name="style"></param>
        public void SetDockStyle(DockStyle style)
        {
            this.Dock = style;
        }
        #endregion

		#region È­¸é ÄÄÆ÷³ÍÆ®, »ý¼ºÀÚ, ¼Ò¸êÀÚ

		private Janus.Windows.UI.Dock.UIPanelManager uiPM;
		private System.Windows.Forms.Panel pnlSearch;
		private Janus.Windows.EditControls.UIButton btnSearch;
		private Janus.Windows.UI.Dock.UIPanel uiPanelSearch;
		private Janus.Windows.UI.Dock.UIPanelInnerContainer uiPanelSearchContainer;
		private Janus.Windows.UI.Dock.UIPanel uiPanelList;
		private Janus.Windows.UI.Dock.UIPanelInnerContainer uiPanelListContainer;
		private Janus.Windows.UI.Dock.UIPanel uiPanelDetail;
		private Janus.Windows.UI.Dock.UIPanelInnerContainer uiPanelDetailContainer;
		private System.Data.DataView dvSchedule;
		private Janus.Windows.UI.Dock.UIPanelGroup uiPanelHomeAdSchedule;
		private Janus.Windows.EditControls.UIComboBox cbSearchMedia;
		private Janus.Windows.GridEX.GridEX grdExScheduleList;
		private Janus.Windows.EditControls.UIGroupBox gbScheduling;
		private Janus.Windows.EditControls.UIButton btnOrderUp;
		private Janus.Windows.EditControls.UIButton btnOrderDown;
		private Janus.Windows.EditControls.UIButton btnOrderFirst;
		private Janus.Windows.EditControls.UIButton btnOrderLast;
		private Janus.Windows.EditControls.UIButton btnAdd;
		private Janus.Windows.EditControls.UIButton btnDelete;
		private System.Windows.Forms.Label label1;
		private Janus.Windows.GridEX.EditControls.EditBox editBox1;
		private System.Windows.Forms.Panel pnlDetail;
		private AdManagerClient.SchHomeAdDs schAppendAdDs;
		private System.Windows.Forms.Label lbMsg;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label lbFileListCount;
		private System.ComponentModel.IContainer components;

		public SchAppendAdControl()
		{
			// ÀÌ È£ÃâÀº Windows.Forms Form µðÀÚÀÌ³Ê¿¡ ÇÊ¿äÇÕ´Ï´Ù.
			InitializeComponent();

			

		}

		/// <summary> 
		/// »ç¿ë ÁßÀÎ ¸ðµç ¸®¼Ò½º¸¦ Á¤¸®ÇÕ´Ï´Ù.
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
		#endregion

		#region ±¸¼º ¿ä¼Ò µðÀÚÀÌ³Ê¿¡¼­ »ý¼ºÇÑ ÄÚµå
		/// <summary> 
		/// µðÀÚÀÌ³Ê Áö¿ø¿¡ ÇÊ¿äÇÑ ¸Þ¼­µåÀÔ´Ï´Ù. 
		/// ÀÌ ¸Þ¼­µåÀÇ ³»¿ëÀ» ÄÚµå ÆíÁý±â·Î ¼öÁ¤ÇÏÁö ¸¶½Ê½Ã¿À.
		/// </summary>
		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            Janus.Windows.GridEX.GridEXLayout grdExScheduleList_DesignTimeLayout = new Janus.Windows.GridEX.GridEXLayout();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SchAppendAdControl));
            this.uiPM = new Janus.Windows.UI.Dock.UIPanelManager(this.components);
            this.uiPanelHomeAdSchedule = new Janus.Windows.UI.Dock.UIPanelGroup();
            this.uiPanelSearch = new Janus.Windows.UI.Dock.UIPanel();
            this.uiPanelSearchContainer = new Janus.Windows.UI.Dock.UIPanelInnerContainer();
            this.pnlSearch = new System.Windows.Forms.Panel();
            this.cbSearchMedia = new Janus.Windows.EditControls.UIComboBox();
            this.btnSearch = new Janus.Windows.EditControls.UIButton();
            this.uiPanelList = new Janus.Windows.UI.Dock.UIPanel();
            this.uiPanelListContainer = new Janus.Windows.UI.Dock.UIPanelInnerContainer();
            this.grdExScheduleList = new Janus.Windows.GridEX.GridEX();
            this.dvSchedule = new System.Data.DataView();
            this.schAppendAdDs = new AdManagerClient.SchHomeAdDs();
            this.uiPanelDetail = new Janus.Windows.UI.Dock.UIPanel();
            this.uiPanelDetailContainer = new Janus.Windows.UI.Dock.UIPanelInnerContainer();
            this.pnlDetail = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.lbFileListCount = new System.Windows.Forms.Label();
            this.lbMsg = new System.Windows.Forms.Label();
            this.gbScheduling = new Janus.Windows.EditControls.UIGroupBox();
            this.btnOrderUp = new Janus.Windows.EditControls.UIButton();
            this.btnOrderDown = new Janus.Windows.EditControls.UIButton();
            this.btnOrderFirst = new Janus.Windows.EditControls.UIButton();
            this.btnOrderLast = new Janus.Windows.EditControls.UIButton();
            this.btnDelete = new Janus.Windows.EditControls.UIButton();
            this.btnAdd = new Janus.Windows.EditControls.UIButton();
            this.label1 = new System.Windows.Forms.Label();
            this.editBox1 = new Janus.Windows.GridEX.EditControls.EditBox();
            ((System.ComponentModel.ISupportInitialize)(this.uiPM)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelHomeAdSchedule)).BeginInit();
            this.uiPanelHomeAdSchedule.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelSearch)).BeginInit();
            this.uiPanelSearch.SuspendLayout();
            this.uiPanelSearchContainer.SuspendLayout();
            this.pnlSearch.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelList)).BeginInit();
            this.uiPanelList.SuspendLayout();
            this.uiPanelListContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdExScheduleList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dvSchedule)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.schAppendAdDs)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelDetail)).BeginInit();
            this.uiPanelDetail.SuspendLayout();
            this.uiPanelDetailContainer.SuspendLayout();
            this.pnlDetail.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gbScheduling)).BeginInit();
            this.gbScheduling.SuspendLayout();
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
            this.uiPanelHomeAdSchedule.Id = new System.Guid("b162c53a-7940-47cd-a0ed-9762c59c50d8");
            this.uiPanelHomeAdSchedule.StaticGroup = true;
            this.uiPanelSearch.Id = new System.Guid("afb3b7d0-aa79-443c-a1e8-3baba38f7a4d");
            this.uiPanelHomeAdSchedule.Panels.Add(this.uiPanelSearch);
            this.uiPanelList.Id = new System.Guid("8e3140b9-bfcb-43af-b84e-2ee6cddf51dc");
            this.uiPanelHomeAdSchedule.Panels.Add(this.uiPanelList);
            this.uiPanelDetail.Id = new System.Guid("b2973a57-d3e1-4d80-9387-7a6dc3332542");
            this.uiPanelHomeAdSchedule.Panels.Add(this.uiPanelDetail);
            this.uiPM.Panels.Add(this.uiPanelHomeAdSchedule);
            // 
            // Design Time Panel Info:
            // 
            this.uiPM.BeginPanelInfo();
            this.uiPM.AddDockPanelInfo(new System.Guid("b162c53a-7940-47cd-a0ed-9762c59c50d8"), Janus.Windows.UI.Dock.PanelGroupStyle.HorizontalTiles, Janus.Windows.UI.Dock.PanelDockStyle.Fill, true, new System.Drawing.Size(1010, 677), true);
            this.uiPM.AddDockPanelInfo(new System.Guid("afb3b7d0-aa79-443c-a1e8-3baba38f7a4d"), new System.Guid("b162c53a-7940-47cd-a0ed-9762c59c50d8"), 42, true);
            this.uiPM.AddDockPanelInfo(new System.Guid("8e3140b9-bfcb-43af-b84e-2ee6cddf51dc"), new System.Guid("b162c53a-7940-47cd-a0ed-9762c59c50d8"), 498, true);
            this.uiPM.AddDockPanelInfo(new System.Guid("b2973a57-d3e1-4d80-9387-7a6dc3332542"), new System.Guid("b162c53a-7940-47cd-a0ed-9762c59c50d8"), 87, true);
            this.uiPM.AddFloatingPanelInfo(new System.Guid("b162c53a-7940-47cd-a0ed-9762c59c50d8"), Janus.Windows.UI.Dock.PanelGroupStyle.HorizontalTiles, true, new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
            this.uiPM.AddFloatingPanelInfo(new System.Guid("8e3140b9-bfcb-43af-b84e-2ee6cddf51dc"), new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
            this.uiPM.AddFloatingPanelInfo(new System.Guid("b2973a57-d3e1-4d80-9387-7a6dc3332542"), new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
            this.uiPM.AddFloatingPanelInfo(new System.Guid("afb3b7d0-aa79-443c-a1e8-3baba38f7a4d"), new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
            this.uiPM.EndPanelInfo();
            // 
            // uiPanelHomeAdSchedule
            // 
            this.uiPanelHomeAdSchedule.ActiveCaptionMode = Janus.Windows.UI.Dock.ActiveCaptionMode.Never;
            this.uiPanelHomeAdSchedule.BackColor = System.Drawing.SystemColors.Window;
            this.uiPanelHomeAdSchedule.CaptionVisible = Janus.Windows.UI.InheritableBoolean.True;
            this.uiPanelHomeAdSchedule.CloseButtonVisible = Janus.Windows.UI.InheritableBoolean.False;
            this.uiPanelHomeAdSchedule.Location = new System.Drawing.Point(0, 0);
            this.uiPanelHomeAdSchedule.Name = "uiPanelHomeAdSchedule";
            this.uiPanelHomeAdSchedule.Size = new System.Drawing.Size(1010, 677);
            this.uiPanelHomeAdSchedule.TabIndex = 4;
            this.uiPanelHomeAdSchedule.Text = "Ãß°¡±¤°íÆí¼º";
            // 
            // uiPanelSearch
            // 
            this.uiPanelSearch.AllowResize = Janus.Windows.UI.InheritableBoolean.False;
            this.uiPanelSearch.CaptionVisible = Janus.Windows.UI.InheritableBoolean.False;
            this.uiPanelSearch.CloseButtonVisible = Janus.Windows.UI.InheritableBoolean.False;
            this.uiPanelSearch.InnerContainer = this.uiPanelSearchContainer;
            this.uiPanelSearch.Location = new System.Drawing.Point(0, 22);
            this.uiPanelSearch.Name = "uiPanelSearch";
            this.uiPanelSearch.Size = new System.Drawing.Size(1010, 43);
            this.uiPanelSearch.TabIndex = 4;
            this.uiPanelSearch.Text = "°Ë»ö";
            // 
            // uiPanelSearchContainer
            // 
            this.uiPanelSearchContainer.Controls.Add(this.pnlSearch);
            this.uiPanelSearchContainer.Location = new System.Drawing.Point(1, 1);
            this.uiPanelSearchContainer.Name = "uiPanelSearchContainer";
            this.uiPanelSearchContainer.Size = new System.Drawing.Size(1008, 41);
            this.uiPanelSearchContainer.TabIndex = 0;
            // 
            // pnlSearch
            // 
            this.pnlSearch.BackColor = System.Drawing.SystemColors.Window;
            this.pnlSearch.Controls.Add(this.cbSearchMedia);
            this.pnlSearch.Controls.Add(this.btnSearch);
            this.pnlSearch.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlSearch.Location = new System.Drawing.Point(0, 0);
            this.pnlSearch.Name = "pnlSearch";
            this.pnlSearch.Size = new System.Drawing.Size(1008, 41);
            this.pnlSearch.TabIndex = 3;
            // 
            // cbSearchMedia
            // 
            this.cbSearchMedia.BackColor = System.Drawing.Color.White;
            this.cbSearchMedia.ComboStyle = Janus.Windows.EditControls.ComboStyle.DropDownList;
            this.cbSearchMedia.Location = new System.Drawing.Point(8, 8);
            this.cbSearchMedia.Name = "cbSearchMedia";
            this.cbSearchMedia.Size = new System.Drawing.Size(120, 21);
            this.cbSearchMedia.TabIndex = 1;
            this.cbSearchMedia.Text = "¸ÅÃ¼¼±ÅÃ";
            this.cbSearchMedia.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            // 
            // btnSearch
            // 
            this.btnSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSearch.Enabled = false;
            this.btnSearch.Location = new System.Drawing.Point(895, 8);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(104, 24);
            this.btnSearch.TabIndex = 2;
            this.btnSearch.Text = "Á¶ È¸";
            this.btnSearch.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // uiPanelList
            // 
            this.uiPanelList.BackColor = System.Drawing.SystemColors.Window;
            this.uiPanelList.CaptionStyle = Janus.Windows.UI.Dock.PanelCaptionStyle.Dark;
            this.uiPanelList.InnerContainer = this.uiPanelListContainer;
            this.uiPanelList.Location = new System.Drawing.Point(0, 69);
            this.uiPanelList.MinimumSize = new System.Drawing.Size(-1, 100);
            this.uiPanelList.Name = "uiPanelList";
            this.uiPanelList.Size = new System.Drawing.Size(1010, 515);
            this.uiPanelList.TabIndex = 12;
            this.uiPanelList.TabStop = false;
            this.uiPanelList.Text = "Ãß°¡±¤°íÆí¼ºÇöÈ²";
            // 
            // uiPanelListContainer
            // 
            this.uiPanelListContainer.BackColor = System.Drawing.SystemColors.Window;
            this.uiPanelListContainer.Controls.Add(this.grdExScheduleList);
            this.uiPanelListContainer.Location = new System.Drawing.Point(1, 23);
            this.uiPanelListContainer.Name = "uiPanelListContainer";
            this.uiPanelListContainer.Size = new System.Drawing.Size(1008, 491);
            this.uiPanelListContainer.TabIndex = 0;
            // 
            // grdExScheduleList
            // 
            this.grdExScheduleList.AlternatingColors = true;
            this.grdExScheduleList.AutomaticSort = false;
            this.grdExScheduleList.BorderStyle = Janus.Windows.GridEX.BorderStyle.None;
            this.grdExScheduleList.DataSource = this.dvSchedule;
            grdExScheduleList_DesignTimeLayout.LayoutString = resources.GetString("grdExScheduleList_DesignTimeLayout.LayoutString");
            this.grdExScheduleList.DesignTimeLayout = grdExScheduleList_DesignTimeLayout;
            this.grdExScheduleList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdExScheduleList.EditorsControlStyle.ButtonAppearance = Janus.Windows.GridEX.ButtonAppearance.Regular;
            this.grdExScheduleList.EmptyRows = true;
            this.grdExScheduleList.FocusCellFormatStyle.BackColor = System.Drawing.SystemColors.Highlight;
            this.grdExScheduleList.FocusCellFormatStyle.ForeColor = System.Drawing.Color.White;
            this.grdExScheduleList.FocusStyle = Janus.Windows.GridEX.FocusStyle.None;
            this.grdExScheduleList.Font = new System.Drawing.Font("±¼¸²Ã¼", 9F);
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
            this.grdExScheduleList.Size = new System.Drawing.Size(1008, 491);
            this.grdExScheduleList.TabIndex = 4;
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
            this.grdExScheduleList.ColumnHeaderClick += new Janus.Windows.GridEX.ColumnActionEventHandler(this.grdExScheduleList_ColumnHeaderClick);
            this.grdExScheduleList.Enter += new System.EventHandler(this.OnGrdRowChanged);
            // 
            // dvSchedule
            // 
            this.dvSchedule.Table = this.schAppendAdDs.HomeAdSchedule;
            // 
            // schAppendAdDs
            // 
            this.schAppendAdDs.DataSetName = "SchAppendAdDs";
            this.schAppendAdDs.Locale = new System.Globalization.CultureInfo("en-US");
            this.schAppendAdDs.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // uiPanelDetail
            // 
            this.uiPanelDetail.AllowResize = Janus.Windows.UI.InheritableBoolean.False;
            this.uiPanelDetail.BackColor = System.Drawing.SystemColors.Window;
            this.uiPanelDetail.CaptionStyle = Janus.Windows.UI.Dock.PanelCaptionStyle.Dark;
            this.uiPanelDetail.InnerContainer = this.uiPanelDetailContainer;
            this.uiPanelDetail.Location = new System.Drawing.Point(0, 588);
            this.uiPanelDetail.Name = "uiPanelDetail";
            this.uiPanelDetail.Size = new System.Drawing.Size(1010, 89);
            this.uiPanelDetail.TabIndex = 14;
            this.uiPanelDetail.TabStop = false;
            this.uiPanelDetail.Text = "Æí¼º";
            // 
            // uiPanelDetailContainer
            // 
            this.uiPanelDetailContainer.Controls.Add(this.pnlDetail);
            this.uiPanelDetailContainer.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.uiPanelDetailContainer.Location = new System.Drawing.Point(1, 23);
            this.uiPanelDetailContainer.Name = "uiPanelDetailContainer";
            this.uiPanelDetailContainer.Size = new System.Drawing.Size(1008, 65);
            this.uiPanelDetailContainer.TabIndex = 0;
            // 
            // pnlDetail
            // 
            this.pnlDetail.BackColor = System.Drawing.SystemColors.Window;
            this.pnlDetail.Controls.Add(this.label2);
            this.pnlDetail.Controls.Add(this.lbFileListCount);
            this.pnlDetail.Controls.Add(this.lbMsg);
            this.pnlDetail.Controls.Add(this.gbScheduling);
            this.pnlDetail.Controls.Add(this.btnDelete);
            this.pnlDetail.Controls.Add(this.btnAdd);
            this.pnlDetail.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlDetail.Location = new System.Drawing.Point(0, 0);
            this.pnlDetail.Name = "pnlDetail";
            this.pnlDetail.Size = new System.Drawing.Size(1008, 65);
            this.pnlDetail.TabIndex = 5;
            // 
            // label2
            // 
            this.label2.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.label2.Location = new System.Drawing.Point(232, 32);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(144, 21);
            this.label2.TabIndex = 44;
            this.label2.Text = "ÇöÀçÈ¨+ÆÄÀÏ¸®½ºÆ®°¹¼ö:";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lbFileListCount
            // 
            this.lbFileListCount.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.lbFileListCount.Location = new System.Drawing.Point(376, 32);
            this.lbFileListCount.Name = "lbFileListCount";
            this.lbFileListCount.Size = new System.Drawing.Size(64, 21);
            this.lbFileListCount.TabIndex = 43;
            this.lbFileListCount.Text = "0/0";
            this.lbFileListCount.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lbMsg
            // 
            this.lbMsg.Location = new System.Drawing.Point(8, 8);
            this.lbMsg.Name = "lbMsg";
            this.lbMsg.Size = new System.Drawing.Size(360, 21);
            this.lbMsg.TabIndex = 42;
            this.lbMsg.Text = "Æí¼º";
            this.lbMsg.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // gbScheduling
            // 
            this.gbScheduling.Controls.Add(this.btnOrderUp);
            this.gbScheduling.Controls.Add(this.btnOrderDown);
            this.gbScheduling.Controls.Add(this.btnOrderFirst);
            this.gbScheduling.Controls.Add(this.btnOrderLast);
            this.gbScheduling.Location = new System.Drawing.Point(448, 8);
            this.gbScheduling.Name = "gbScheduling";
            this.gbScheduling.Size = new System.Drawing.Size(392, 48);
            this.gbScheduling.TabIndex = 41;
            this.gbScheduling.Text = "Æí¼º¼ø¼­º¯°æ";
            // 
            // btnOrderUp
            // 
            this.btnOrderUp.Enabled = false;
            this.btnOrderUp.Location = new System.Drawing.Point(104, 16);
            this.btnOrderUp.Name = "btnOrderUp";
            this.btnOrderUp.Size = new System.Drawing.Size(88, 24);
            this.btnOrderUp.TabIndex = 9;
            this.btnOrderUp.Text = "¼ø¼­¿Ã¸²";
            this.btnOrderUp.VisualStyle = Janus.Windows.UI.VisualStyle.Office2003;
            this.btnOrderUp.Click += new System.EventHandler(this.btnOrderUp_Click);
            // 
            // btnOrderDown
            // 
            this.btnOrderDown.Enabled = false;
            this.btnOrderDown.Location = new System.Drawing.Point(200, 16);
            this.btnOrderDown.Name = "btnOrderDown";
            this.btnOrderDown.Size = new System.Drawing.Size(88, 24);
            this.btnOrderDown.TabIndex = 10;
            this.btnOrderDown.Text = "¼ø¼­³»¸²";
            this.btnOrderDown.VisualStyle = Janus.Windows.UI.VisualStyle.Office2003;
            this.btnOrderDown.Click += new System.EventHandler(this.btnOrderDown_Click);
            // 
            // btnOrderFirst
            // 
            this.btnOrderFirst.Enabled = false;
            this.btnOrderFirst.Location = new System.Drawing.Point(8, 16);
            this.btnOrderFirst.Name = "btnOrderFirst";
            this.btnOrderFirst.Size = new System.Drawing.Size(88, 24);
            this.btnOrderFirst.TabIndex = 8;
            this.btnOrderFirst.Text = "Ã¹¼ø¼­·Î";
            this.btnOrderFirst.VisualStyle = Janus.Windows.UI.VisualStyle.Office2003;
            this.btnOrderFirst.Click += new System.EventHandler(this.btnOrderFirst_Click);
            // 
            // btnOrderLast
            // 
            this.btnOrderLast.Enabled = false;
            this.btnOrderLast.Location = new System.Drawing.Point(296, 16);
            this.btnOrderLast.Name = "btnOrderLast";
            this.btnOrderLast.Size = new System.Drawing.Size(88, 24);
            this.btnOrderLast.TabIndex = 11;
            this.btnOrderLast.Text = "³¡¼ø¼­·Î";
            this.btnOrderLast.VisualStyle = Janus.Windows.UI.VisualStyle.Office2003;
            this.btnOrderLast.Click += new System.EventHandler(this.btnOrderLast_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnDelete.Enabled = false;
            this.btnDelete.Location = new System.Drawing.Point(120, 34);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(104, 24);
            this.btnDelete.TabIndex = 7;
            this.btnDelete.Text = "»è Á¦";
            this.btnDelete.VisualStyle = Janus.Windows.UI.VisualStyle.Office2003;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnAdd
            // 
            this.btnAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnAdd.Enabled = false;
            this.btnAdd.Location = new System.Drawing.Point(8, 34);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(104, 24);
            this.btnAdd.TabIndex = 6;
            this.btnAdd.Text = "Ãß °¡";
            this.btnAdd.VisualStyle = Janus.Windows.UI.VisualStyle.Office2003;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(100, 23);
            this.label1.TabIndex = 0;
            // 
            // editBox1
            // 
            this.editBox1.Location = new System.Drawing.Point(0, 0);
            this.editBox1.Name = "editBox1";
            this.editBox1.Size = new System.Drawing.Size(100, 21);
            this.editBox1.TabIndex = 0;
            this.editBox1.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
            // 
            // SchAppendAdControl
            // 
            this.Controls.Add(this.uiPanelHomeAdSchedule);
            this.Name = "SchAppendAdControl";
            this.Size = new System.Drawing.Size(1010, 677);
            this.Load += new System.EventHandler(this.UserControl_Load);
            ((System.ComponentModel.ISupportInitialize)(this.uiPM)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelHomeAdSchedule)).EndInit();
            this.uiPanelHomeAdSchedule.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelSearch)).EndInit();
            this.uiPanelSearch.ResumeLayout(false);
            this.uiPanelSearchContainer.ResumeLayout(false);
            this.pnlSearch.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelList)).EndInit();
            this.uiPanelList.ResumeLayout(false);
            this.uiPanelListContainer.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdExScheduleList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dvSchedule)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.schAppendAdDs)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelDetail)).EndInit();
            this.uiPanelDetail.ResumeLayout(false);
            this.uiPanelDetailContainer.ResumeLayout(false);
            this.pnlDetail.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gbScheduling)).EndInit();
            this.gbScheduling.ResumeLayout(false);
            this.ResumeLayout(false);

		}
		#endregion

		#region ÄÁÆ®·Ñ ·Îµå
		private void UserControl_Load(object sender, System.EventArgs e)
		{
			// µ¥ÀÌÅÍ°ü¸®¿ë °´Ã¼»ý¼º
			dt = ((DataView)grdExScheduleList.DataSource).Table;  
			cm = (CurrencyManager) this.BindingContext[grdExScheduleList.DataSource]; 
			cm.PositionChanged += new System.EventHandler(OnGrdRowChanged); 

			// ÄÁÆ®·Ñ ÃÊ±âÈ­
			InitControl();	
		}

		#endregion

		#region ÄÁÆ®·Ñ ÃÊ±âÈ­
		private void InitControl()
		{
			ProgressStart();

			lbMsg.Text = "";

			InitCombo();

			// Á¶È¸±ÇÇÑ °Ë»ç
			if(menu.CanRead(MenuCode))
			{
				canRead = true;
			}
			
			// Ãß°¡¹öÆ° È°¼ºÈ­
			if(menu.CanCreate(MenuCode))
			{
				canCreate = true;
			}

			// »èÁ¦¹öÆ° È°¼ºÈ­
			if(menu.CanDelete(MenuCode))
			{
				canDelete = true;
			}

			// ÀúÀå¹öÆ° È°¼ºÈ­
			if(menu.CanUpdate(MenuCode))
			{
				ResetTextReadonly();
				canUpdate = true;
			}

			ProgressStop();

			if(canRead)
			{
				SearchScheduleAppendAd();
			}

			InitButton();
		}

		private void InitCombo()
		{
			Init_MediaCode();
			InitCombo_Level();
		}

		private void Init_MediaCode()
		{
			// ¸ÅÃ¼¸¦ Á¶È¸ÇÑ´Ù.
			MediaCodeModel mediacodeModel = new MediaCodeModel();
			new MediaCodeManager(systemModel, commonModel).GetMediaCodeList(mediacodeModel);
			
			if (mediacodeModel.ResultCD.Equals("0000"))
			{
				// µ¥ÀÌÅÍ¼Â¿¡ ¼ÂÆÃ
				Utility.SetDataTable(schAppendAdDs.Medias, mediacodeModel.MediaCodeDataSet);				
			}

			// °Ë»öÁ¶°ÇÀÇ ÄÞº¸
			this.cbSearchMedia.Items.Clear();
			
			// ÄÞº¸¹Ú½º¿¡ ¼ÂÆ®ÇÒ ÄÚµå¸ñ·ÏÀ» ´ãÀ» Item¹è¿­À» ¼±¾ð
			Janus.Windows.EditControls.UIComboBoxItem[] comboItems = new Janus.Windows.EditControls.UIComboBoxItem[mediacodeModel.ResultCnt + 1];

			comboItems[0] = new Janus.Windows.EditControls.UIComboBoxItem("¸ÅÃ¼¼±ÅÃ","00");
			
			for(int i=0;i<mediacodeModel.ResultCnt;i++)
			{
				DataRow row = schAppendAdDs.Medias.Rows[i];

				string val = row["MediaCode"].ToString();
				string txt = row["MediaName"].ToString();
				comboItems[i+1] = new Janus.Windows.EditControls.UIComboBoxItem(txt,val);
			}
			// ÄÞº¸¿¡ ¼ÂÆ®
			this.cbSearchMedia.Items.AddRange(comboItems);
			this.cbSearchMedia.SelectedIndex = 0;

			Application.DoEvents();
		}

		private void InitCombo_Level()
		{

			if(commonModel.UserLevel=="20")
			{
				cbSearchMedia.SelectedValue = commonModel.MediaCode;			
				cbSearchMedia.ReadOnly = true;					
			}
			else
			{
				for(int i=0;i < schAppendAdDs.Medias.Rows.Count;i++)
				{
					DataRow row = schAppendAdDs.Medias.Rows[i];					
					if(row["MediaCode"].ToString().Equals(FrameSystem._HANATV.ToString()))
					{
						cbSearchMedia.SelectedValue = FrameSystem._HANATV; // ÇÏ³ªTV¸¦ ±âº»°ªÀ¸·Î ÇÑ´Ù.	 		
						break;															
					}
					else
					{
						cbSearchMedia.SelectedValue="00";
					}
				}	
			}
		
			Application.DoEvents();
		}
	
		private void InitButton()
		{
			if(canRead)
			{
				btnSearch.Enabled			= true;
			}
			if(canCreate)
			{
				btnAdd.Enabled			= true;
			}
			
			grdExScheduleList.Focus();

			Application.DoEvents();
		}

		private void DisableButton()
		{
			btnSearch.Enabled			= false;
			btnAdd.Enabled				= false;
			btnDelete.Enabled			= false;
			btnOrderUp.Enabled			= false;
			btnOrderDown.Enabled		= false;
			btnOrderFirst.Enabled		= false;
			btnOrderLast.Enabled		= false;

			Application.DoEvents();
		}

		#endregion

		#region ±¤°íÆÄÀÏ ¾×¼ÇÃ³¸® ¸Þ¼Òµå

		/// <summary>
		/// ±×¸®µåÀÇ Rowº¯°æ½Ã
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void OnGrdRowChanged(object sender, System.EventArgs e) 
		{
            if (!IsSearching) // 2011.11.29 JH.Park Á¶È¸ÁßÀÌ ¾Æ´Ò°æ¿ì¿¡¸¸ µ¿ÀÛÇÏµµ·Ï º¯°æ
            {
                SetDetailText();
                InitButton();
            }
		}

		/// <summary>
		/// Á¶È¸¹öÆ° Å¬¸¯
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnSearch_Click(object sender, System.EventArgs e)
		{
			DisableButton();
			SearchScheduleAppendAd();
			InitButton();
		}

		/// <summary>
		/// Ãß°¡¹öÆ° Å¬¸¯
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		/// 

		private void btnAdd_Click(object sender, System.EventArgs e)
		{
			DisableButton();	
			//  Ãß°¡±¤°í ´ë»ó¸ñ·Ï °Ë»öÃ¢ 
			SchAppendAdSearch_pForm pForm = new SchAppendAdSearch_pForm(this);

			// ¸ÅÃ¼ÄÚµå¼ÂÆ®
			pForm.keyMediaCode = defMedaiCode;
			pForm.ShowDialog();            
			pForm.Dispose();
			pForm = null;

			InitButton();			
		}

		/// <summary>
		/// »èÁ¦¹öÆ° Å¬¸¯
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnDelete_Click(object sender, System.EventArgs e)
		{
			DeleteScheduleAppendAd();
		}

		private void btnOrderFirst_Click(object sender, System.EventArgs e)
		{
			OrderSetScheduleAppendAd(ORDER_FIRST);
		}


		private void btnOrderUp_Click(object sender, System.EventArgs e)
		{
			OrderSetScheduleAppendAd(ORDER_UP);
		}

		private void btnOrderDown_Click(object sender, System.EventArgs e)
		{
			OrderSetScheduleAppendAd(ORDER_DOWN);
		}

		private void btnOrderLast_Click(object sender, System.EventArgs e)
		{
			OrderSetScheduleAppendAd(ORDER_LAST);
		}

		private void grdExScheduleList_ColumnHeaderClick(object sender, Janus.Windows.GridEX.ColumnActionEventArgs e)
		{

			//ÄÃ·³Index 0(Ã¼Å©¹Ú½ºÄÃ·³ÀÌ)ÀÌ ¾Æ´Ï¸é ºüÁ®³ª°¡°Ô Ã³¸®.
			if(e.Column.Index != 0)
			{
				return;
			}
            
			//ColumnHeader Click½Ã¿¡ dt Setting 
			DataRow[] foundRows = dt.Select("CheckYn = 'False'");
         
			// if(grdExItemList.CurrentColumn.Position == 0){
			if( foundRows.Length == 0 )
			{
				for(int i=0;i < dt.Rows.Count;i++)
				{
					dt.Rows[i].BeginEdit();
					dt.Rows[i]["CheckYn"]="False";
					dt.Rows[i].EndEdit();
				}
			}
			else
			{
				for(int i=0;i < dt.Rows.Count;i++)
				{
					dt.Rows[i].BeginEdit();
					dt.Rows[i]["CheckYn"]="True";
					dt.Rows[i].EndEdit();
				}
			}
		}


		#endregion

		#region Ã³¸®¸Þ¼Òµå

		/// <summary>
		/// ±¤°íÆÄÀÏ¸ñ·Ï Á¶È¸
		/// </summary>
		private void SearchScheduleAppendAd()
		{
            IsSearching = true;

			StatusMessage("Ãß°¡±¤°í Æí¼ºÇöÈ²À» Á¶È¸ÇÕ´Ï´Ù.");

			if(cbSearchMedia.SelectedItem.Value.Equals("00")) 
			{
				MessageBox.Show("¸ÅÃ¼¸¦ ¼±ÅÃÇÏ¿© ÁÖ½Ã±â ¹Ù¶ø´Ï´Ù.","Ãß°¡±¤°í Æí¼ºÇöÈ² Á¶È¸", 
					MessageBoxButtons.OK, MessageBoxIcon.Information );
				return;
			}

			ProgressStart();

			try
			{
				// ¸ðµç Ã¼Å©¹Ú½ºÀÇ Ã¼Å©¸¦ Ç¬´Ù.
				grdExScheduleList.UnCheckAllRecords(); 

				// Ãß°¡±¤°íÆí¼ºÇöÈ²À» Á¶È¸ÇÑ´Ù.

				// µ¥ÀÌÅÍ¸ðµ¨ ÃÊ±âÈ­
				schAppendAdModel.Init();
				schAppendAdModel.SearchMediaCode		 =  cbSearchMedia.SelectedItem.Value.ToString(); 

				// ±¤°íÆÄÀÏ¸ñ·ÏÁ¶È¸ ¼­ºñ½º¸¦ È£ÃâÇÑ´Ù.
				new SchAppendAdManager(systemModel,commonModel).GetSchAppendAdList(schAppendAdModel);

				if (schAppendAdModel.ResultCD.Equals("0000"))
				{
					Utility.SetDataTable(schAppendAdDs.HomeAdSchedule, schAppendAdModel.ScheduleDataSet);		
					StatusMessage(schAppendAdModel.ResultCnt + "°ÇÀÇ ±¤°íÆÄÀÏ Á¤º¸°¡ Á¶È¸µÇ¾ú½À´Ï´Ù.");

					uiPanelList.Text = "Ãß°¡±¤°í Æí¼ºÇöÈ² : " + cbSearchMedia.SelectedItem.Text;
					defMedaiCode     = cbSearchMedia.SelectedItem.Value.ToString();

					// 2007.10.02 
					// ÆÄÀÏ¸®½ºÆ®°Ç¼ö Ç¥½Ã
					FileListCnt      = schAppendAdModel.FileListCount;
					lbFileListCount.Text = FileListCnt.ToString() + "/" + FILEMAX.ToString(); 

					keyLastOrder = schAppendAdModel.LastOrder;
					AddSchChoice();		
							
				}

				// Æí¼º¹èÆ÷½ÂÀÎ Ã³¸®»óÅÂ¸¦ Á¶È¸ÇÑ´Ù.
				keyAckNo    = "";
				keyAckState = "";

				schPublishModel.Init();
				schPublishModel.SearchMediaCode		 =  cbSearchMedia.SelectedItem.Value.ToString(); 

				// ÇöÀç ½ÂÀÎ»óÅÂÁ¶È¸ ¼­ºñ½º¸¦ È£ÃâÇÑ´Ù.
				new SchPublishManager(systemModel,commonModel).GetPublishState(schPublishModel,10);


				if (schPublishModel.ResultCD.Equals("0000"))
				{
					keyAckNo    = schPublishModel.AckNo;
					keyAckState = schPublishModel.State;

					if(keyAckState.Equals("10"))	// ½ÂÀÎ»óÅÂ°¡ 10:Æí¼ºÁßÀÌ¸é
					{
						lbMsg.Text = "Æí¼º ÁøÇàÁßÀÔ´Ï´Ù.";
					}
					else if(keyAckState.Equals("20")) // ½ÂÀÎ»óÅÂ°¡ 20:Æí¼º½ÂÀÎ »óÅÂÀÌ¸é Æí¼ºÀÌ ºÒ°¡ÇÏ´Ù.
					{
						lbMsg.Text = "Æí¼º½ÂÀÎ ÈÄ °Ë¼ö½ÂÀÎ ´ë±âÁßÀÔ´Ï´Ù.";
						canCreate = false;
						canUpdate = false;
						canDelete = false;

						ProgressStop();
						MessageBox.Show("ÇöÀç Æí¼º½ÂÀÎ ÈÄ °Ë¼ö½ÂÀÎ ´ë±â»óÅÂÀÌ¹Ç·Î Æí¼ºÀ» º¯°æÇÒ ¼ö ¾ø½À´Ï´Ù.", "Ãß°¡±¤°íÆí¼º",
							MessageBoxButtons.OK, MessageBoxIcon.Information);
						
					}
					else if(keyAckState.Equals("25") ) // ½ÂÀÎ»óÅÂ°¡ 25:¹èÆ÷´ë±â »óÅÂÀÌ¸é Æí¼ºÀÌ ºÒ°¡ÇÏ´Ù.
					{
						lbMsg.Text = "°Ë¼ö½ÂÀÎ ÈÄ ¹èÆ÷½ÂÀÎ ´ë±âÁßÀÔ´Ï´Ù.";
						canCreate = false;
						canUpdate = false;
						canDelete = false;

						ProgressStop();
						MessageBox.Show("ÇöÀç °Ë¼ö½ÂÀÎ ÈÄ ¹èÆ÷½ÂÀÎ ´ë±â»óÅÂÀÌ¹Ç·Î Æí¼ºÀ» º¯°æÇÒ ¼ö ¾ø½À´Ï´Ù.", "Ãß°¡±¤°íÆí¼º",
							MessageBoxButtons.OK, MessageBoxIcon.Information);
						
					}
					else if(keyAckState.Equals("30")) // ½ÂÀÎ»óÅÂ°¡ 30:¹èÆ÷½ÂÀÎ »óÅÂÀÌ¸é ½Å±ÔÆí¼ºÀÌ °¡´ÉÇÏ´Ù.
					{
						lbMsg.Text = "";
					}
				}

				// »ó¼¼³»¿ª Ç¥½Ã
				SetDetailText();

			}
			catch(FrameException fe)
			{
				FrameSystem.showMsgForm("Ãß°¡±¤°í Æí¼ºÇöÈ² Á¶È¸¿À·ù", new string[] {fe.ErrCode, fe.ResultMsg});
			}
			catch(Exception ex)
			{
				FrameSystem.showMsgForm("Ãß°¡±¤°í Æí¼ºÇöÈ² Á¶È¸¿À·ù",new string[] {"",ex.Message});
			}
			finally
			{
                IsSearching = false; // Á¶È¸Áß Flag ¸®¼Â
				ProgressStop();
			}
		}

		/// <summary>
		/// Å°¯“À»Ã£¾Æ ±×¸®µå Å°¿¡ ÇØ´çµÇ´Â·Î¿ì·Î..
		/// </summary>
		private void AddSchChoice()
		{
			int rowIndex = 0;
			if ( dt.Rows.Count < 1 ) return;              
			foreach (DataRow row in dt.Rows)
			{					
				if(row["ScheduleOrder"].ToString().Equals(keyScheduleOrder))
				{					
					cm.Position = rowIndex;
					break;								
				}				
				rowIndex++;
			}
			grdExScheduleList.EnsureVisible();
		}

		/// <summary>
		/// Ãß°¡±¤°í Æí¼º³»¿ª »èÁ¦
		/// </summary>
		private void DeleteScheduleAppendAd()
		{
			StatusMessage("Ãß°¡±¤°í Æí¼º³»¿ªÀ» »èÁ¦ÇÕ´Ï´Ù.");

			DialogResult result = MessageBox.Show("ÇØ´ç Ãß°¡±¤°í Æí¼º³»¿ªÀ» »èÁ¦ ÇÏ½Ã°Ú½À´Ï±î?","Ãß°¡±¤°í Æí¼º³»¿ª »èÁ¦",
				MessageBoxButtons.YesNo, MessageBoxIcon.Question,
				MessageBoxDefaultButton.Button2);

			if (result == DialogResult.No) return;


			// ±×¸®µå¿¡ º¯°æµÈ µ¥ÀÌÅÍ¸¦ Datasource¿¡ ¾÷µ¥ÀÌÆ® ÇÑ´Ù.
			grdExScheduleList.UpdateData();

			try
			{
				int SetCount = 0;

				// »èÁ¦ ½ÃÅ´
				// Ãß°¡±¤°í´Â »èÁ¦ÇÒ¶§ ³ëÃâ¼ø¼­µµ ¹Ù²ãÁà¾ß ÇÏ¹Ç·Î 
				// »èÁ¦¼ø¼­´Â ¼øÀ§°¡ ³ôÀº °ÍºÎÅÍ ³·Àº ¼øÀ¸·Î ÇØ¾ß ÇÑ´Ù.			
				for(int i = schAppendAdDs.HomeAdSchedule.Rows.Count - 1;i >= 0;i--)
				{

					DataRow row = schAppendAdDs.HomeAdSchedule.Rows[i];

					Debug.WriteLine( i.ToString() + ":" + row["CheckYn"].ToString() + "|" + row["ItemName"].ToString() + "|" + row["ScheduleOrder"].ToString());

					if(row["CheckYn"].ToString().Equals("True"))
					{
						schAppendAdModel.Init();

						// µ¥ÀÌÅÍ¸ðµ¨¿¡ Àü¼ÛÇÒ ³»¿ëÀ» ¼ÂÆ®ÇÑ´Ù.
						schAppendAdModel.MediaCode     = row["MediaCode"].ToString();
						schAppendAdModel.ItemNo        = row["ItemNo"].ToString();
						schAppendAdModel.ItemName      = row["ItemName"].ToString();
						schAppendAdModel.ScheduleOrder = row["ScheduleOrder"].ToString();

						// Ãß°¡±¤°í Æí¼º³»¿ª »èÁ¦ ¼­ºñ½º¸¦ È£ÃâÇÑ´Ù.
						new SchAppendAdManager(systemModel,commonModel).SetSchAppendAdDelete(schAppendAdModel);

						if(schAppendAdModel.ResultCD.Equals("0000"))
						{
							SetCount++;
						}
					}
				}

				if(SetCount > 0)
				{
					keyScheduleOrder = schAppendAdModel.ScheduleOrder;
					ResetDetailText();
					DisableButton();
					SearchScheduleAppendAd();
					InitButton();

					StatusMessage("Ãß°¡±¤°í Æí¼º³»¿ªÀÌ »èÁ¦µÇ¾ú½À´Ï´Ù.");			
				}
				else
				{
					MessageBox.Show("¼±ÅÃµÈ Ãß°¡±¤°í Æí¼º³»¿ªÀÌ ¾ø½À´Ï´Ù.", "Ãß°¡±¤°íÆí¼º",
						MessageBoxButtons.OK, MessageBoxIcon.Information);
				}
			}
			catch(FrameException fe)
			{
				FrameSystem.showMsgForm("Ãß°¡±¤°í Æí¼º³»¿ª»èÁ¦¿À·ù", new string[] {fe.ErrCode, fe.ResultMsg});
			}
			catch(Exception ex)
			{
				FrameSystem.showMsgForm("Ãß°¡±¤°í Æí¼º³»¿ª »èÁ¦¿À·ù",new string[] {"",ex.Message});
			}		

		}

		/// <summary>
		/// Ãß°¡±¤°í Æí¼º³»¿ª ¼øÀ§º¯°æ
		/// </summary>
		private void OrderSetScheduleAppendAd(int OrderSet)
		{
			StatusMessage("Ãß°¡±¤°í Æí¼º³»¿ªÀÇ Æí¼º¼øÀ§¸¦ º¯°æÇÕ´Ï´Ù.");

			if(keyItemName.Trim().Length == 0) 
			{
				MessageBox.Show("º¯°æÇÒ Ãß°¡±¤°í Æí¼º³»¿ªÀÌ ¼±ÅÃµÇÁö ¾Ê¾Ò½À´Ï´Ù.","Ãß°¡±¤°í Æí¼º³»¿ª ¼øÀ§º¯°æ", 
					MessageBoxButtons.OK, MessageBoxIcon.Information );
				return;
			}

			try
			{
				// µ¥ÀÌÅÍ¸ðµ¨¿¡ Àü¼ÛÇÒ ³»¿ëÀ» ¼ÂÆ®ÇÑ´Ù.
				schAppendAdModel.MediaCode     = keyMediaCode;
				schAppendAdModel.ItemNo        = keyItemNo;
				schAppendAdModel.ItemName      = keyItemName;
				schAppendAdModel.ScheduleOrder = keyScheduleOrder;
	
				int NowOrder  = Convert.ToInt32(keyScheduleOrder);
				int LastOrder = Convert.ToInt32(keyLastOrder);

				switch(OrderSet)
				{
					case ORDER_FIRST:
						if(NowOrder <= 1) 
						{
							MessageBox.Show("ÇØ´ç Ãß°¡±¤°í Æí¼º³»¿ªÀÌ Ã¹¹øÂ° ¼øÀ§ÀÔ´Ï´Ù.","Ãß°¡±¤°í Æí¼º³»¿ª ¼øÀ§º¯°æ", 
								MessageBoxButtons.OK, MessageBoxIcon.Information );							
							return;
						}
						break;
					case ORDER_UP:
						if(NowOrder <= 1) 
						{
							MessageBox.Show("ÇØ´ç Ãß°¡±¤°í Æí¼º³»¿ªÀÌ Ã¹¹øÂ° ¼øÀ§ÀÔ´Ï´Ù.","Ãß°¡±¤°í Æí¼º³»¿ª ¼øÀ§º¯°æ", 
								MessageBoxButtons.OK, MessageBoxIcon.Information );							
							return;
						}
						break;
					case ORDER_DOWN:
						if(NowOrder >= LastOrder) 
						{
							MessageBox.Show("ÇØ´ç Ãß°¡±¤°í Æí¼º³»¿ªÀÌ ¸¶Áö¸· ¼øÀ§ÀÔ´Ï´Ù.","Ãß°¡±¤°í Æí¼º³»¿ª ¼øÀ§º¯°æ", 
								MessageBoxButtons.OK, MessageBoxIcon.Information );							
							return;
						}
						break;
					case ORDER_LAST:
						if(NowOrder >= LastOrder) 
						{
							MessageBox.Show("ÇØ´ç Ãß°¡±¤°í Æí¼º³»¿ªÀÌ ¸¶Áö¸· ¼øÀ§ÀÔ´Ï´Ù.","Ãß°¡±¤°í Æí¼º³»¿ª ¼øÀ§º¯°æ", 
								MessageBoxButtons.OK, MessageBoxIcon.Information );							
							return;
						}
						break;
				}

				// Ãß°¡±¤°í Æí¼º³»¿ª ¼ø¼­º¯°æ ¼­ºñ½º¸¦ È£ÃâÇÑ´Ù.
				new SchAppendAdManager(systemModel,commonModel).SetSchAppendAdOrderSet(schAppendAdModel, OrderSet);
				keyScheduleOrder	=	schAppendAdModel.ScheduleOrder;
				StatusMessage("Ãß°¡±¤°í Æí¼º³»¿ªÀÇ ¼øÀ§°¡ º¯°æµÇ¾ú½À´Ï´Ù.");			

				ResetDetailText();
				DisableButton();
				SearchScheduleAppendAd();
				InitButton();
						
		
			}
			catch(FrameException fe)
			{
				FrameSystem.showMsgForm("Ãß°¡±¤°í Æí¼º³»¿ª ¼øÀ§º¯°æ ¿À·ù", new string[] {fe.ErrCode, fe.ResultMsg});
			}
			catch(Exception ex)
			{
				FrameSystem.showMsgForm("Ãß°¡±¤°í Æí¼º³»¿ª ¼øÀ§º¯°æ ¿À·ù",new string[] {"",ex.Message});
			}		

		}
		
		/// <summary>
		/// ±¤°íÆÄÀÏ »ó¼¼Á¤º¸ÀÇ ¼ÂÆ®
		/// </summary>
		private void SetDetailText()
		{
			int curRow = cm.Position;

			if(curRow >= 0)
			{
				uiPanelDetail.Text = ""
					+ "Æí¼º±¤°í : " 
					+ dt.Rows[curRow]["AdTypeName"].ToString().Trim() + " / " 
					+ dt.Rows[curRow]["ItemName"].ToString().Trim()
					;

				keyMediaCode     = dt.Rows[curRow]["MediaCode"].ToString();
				keyItemNo        = dt.Rows[curRow]["ItemNo"].ToString();
				keyItemName      = dt.Rows[curRow]["ItemName"].ToString();
				keyScheduleOrder = dt.Rows[curRow]["ScheduleOrder"].ToString();
								
				if(canCreate)
				{
					btnAdd.Enabled		= true;
				}
				if(canDelete)
				{
					btnDelete.Enabled			= true;
				}
				if(canUpdate)
				{
					btnOrderUp.Enabled			= true;
					btnOrderDown.Enabled		= true;
					btnOrderFirst.Enabled		= true;
					btnOrderLast.Enabled		= true;
				}

			}
			Application.DoEvents();

			StatusMessage("ÁØºñ");
		}

		private void ResetDetailText()
		{
			keyMediaCode     = "";
			keyItemNo        = "";
			keyItemName      = "";
		}
		
		/// <summary>
		/// »ó¼¼Á¤º¸ ¼öÁ¤°¡´ÉÄÉ
		/// </summary>
		private void ResetTextReadonly()
		{
		}
		#endregion

		#region ÀÌº¥Æ®ÇÔ¼ö

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
		#endregion

		#region ÆË¾÷Ã¢À» À§ÇÑ ¸Þ¼Òµå
			
		public void ReloadList()
		{
			SearchScheduleAppendAd();
		}

		#endregion

	}
}
