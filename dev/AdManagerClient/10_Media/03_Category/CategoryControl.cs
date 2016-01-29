// ===============================================================================
// UserInfoControl for Charites Project
//
// UserInfoControl.cs
//
// »ç¿ëÀÚÁ¤º¸°ü¸® ÄÁµå·ÑÀ» Á¤ÀÇÇÕ´Ï´Ù. 
//
// ===============================================================================
// Release history
//
// ===============================================================================
// Copyright (C) 2006 G-Inno Systems Inc.
// All rights reserved.
// zz
using System;
using System.Collections;
using System.Diagnostics;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

using WinFramework.Misc;
using WinFramework.Base;
using WinFramework.Data;

using AdManagerModel;

namespace AdManagerClient
{
	/// <summary>
	/// »ç¿ëÀÚ°ü¸® ÄÁÆ®·Ñ
	/// </summary>
	public class CategoryControl : System.Windows.Forms.UserControl,IUserControl
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
		CategoryModel categoryModel  = new CategoryModel();	// »ç¿ëÀÚÁ¤º¸¸ðµ¨

		// È­¸éÃ³¸®¿ë º¯¼ö
		bool IsNewSearchKey		  = true;					// °Ë»ö¾îÀÔ·Â ¿©ºÎ
		CurrencyManager cm        = null;					// µ¥ÀÌÅÍ ±×¸®µåÀÇ º¯°æ¿¡ µû¸¥ µ¥ÀÌÅÍ¼Â °ü¸®¸¦ À§ÇÏ¿©			
		DataTable       dt        = null;

        bool IsSearching = false; // Á¶È¸Áß »ó¼¼È­¸éÀÌ ¾÷µ¥ÀÌÆ® µÇ´Â °ÍÀ» ¹æÁö ÇÏ±âÀ§ÇÔ 2011.11.29 JH.Park
		bool IsAdding             = false;
		bool canRead			  = false;
		bool canUpdate			  = false;
		bool canCreate            = false;
		bool canDelete            = false;
		private string        mediaCode = null;
		private string        categoryCode = null;

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
		private Janus.Windows.UI.Dock.UIPanel uiPanelUserList;
		private Janus.Windows.UI.Dock.UIPanelInnerContainer uiPanelUserListContainer;
		private Janus.Windows.UI.Dock.UIPanel uiPanelUserDetail;
		private Janus.Windows.UI.Dock.UIPanelInnerContainer uiPanelUserDetailContainer;
		private Janus.Windows.UI.Dock.UIPanel uiPanelUsersSearch;
		private Janus.Windows.UI.Dock.UIPanelInnerContainer uiPanelUsersSearchContainer;
		private Janus.Windows.EditControls.UIButton uiButton2;
		private Janus.Windows.EditControls.UIButton uiButton1;
		private Janus.Windows.UI.Dock.UIPanelGroup uiPanelUsers;
		private System.Windows.Forms.Panel pnlSearch;
		private Janus.Windows.GridEX.EditControls.EditBox ebSearchKey;
		private Janus.Windows.EditControls.UIButton btnSearch;
		private System.Windows.Forms.Panel pnlUserDetail;
		private Janus.Windows.GridEX.EditControls.EditBox ebCategoryName;
		private System.Data.DataView dvCategory;		
		private Janus.Windows.GridEX.EditControls.EditBox ebUserName;
		private System.Windows.Forms.Label lbCategoryName;
		private System.Windows.Forms.Label lbMediaName;
		private Janus.Windows.GridEX.EditControls.EditBox ebMediaName;
		private Janus.Windows.EditControls.UIComboBox cbMediaLevel;
		private AdManagerClient.CategoryDs categoryDs;
		private Janus.Windows.GridEX.EditControls.EditBox ebModDt;
		private System.Windows.Forms.Label lbModDt;
		private Janus.Windows.GridEX.GridEX grdExCategoryList;
		private Janus.Windows.EditControls.UIButton btnDelete;
		private Janus.Windows.EditControls.UIButton btnAdd;
		private Janus.Windows.EditControls.UIButton btnSave;
		private System.Windows.Forms.Label label2;
		private Janus.Windows.EditControls.UICheckBox p_Flag;
		private Janus.Windows.GridEX.EditControls.IntegerUpDown p_SortNo;
		private Janus.Windows.EditControls.UICheckBox p_Css;
		private Janus.Windows.EditControls.UICheckBox p_Inventory;
		private Janus.Windows.GridEX.EditControls.NumericEditBox p_InventoryRate;
		private System.Windows.Forms.Label label3;				
		private System.ComponentModel.IContainer components;

		public CategoryControl()
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
            Janus.Windows.GridEX.GridEXLayout grdExCategoryList_DesignTimeLayout = new Janus.Windows.GridEX.GridEXLayout();
            Janus.Windows.Common.Layouts.JanusLayoutReference grdExCategoryList_DesignTimeLayout_Reference_0 = new Janus.Windows.Common.Layouts.JanusLayoutReference("GridEXLayoutData.RootTable.FormatConditions.Condition0.FormatStyle.BackgroundImag" +
        "e");
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CategoryControl));
            Janus.Windows.Common.Layouts.JanusLayoutReference grdExCategoryList_DesignTimeLayout_Reference_1 = new Janus.Windows.Common.Layouts.JanusLayoutReference("GridEXLayoutData.RootTable.FormatConditions.Condition1.FormatStyle.BackgroundImag" +
        "e");
            Janus.Windows.Common.Layouts.JanusLayoutReference grdExCategoryList_DesignTimeLayout_Reference_2 = new Janus.Windows.Common.Layouts.JanusLayoutReference("GridEXLayoutData.RootTable.FormatConditions.Condition2.FormatStyle.BackgroundImag" +
        "e");
            this.uiPM = new Janus.Windows.UI.Dock.UIPanelManager(this.components);
            this.uiPanelUsers = new Janus.Windows.UI.Dock.UIPanelGroup();
            this.uiPanelUsersSearch = new Janus.Windows.UI.Dock.UIPanel();
            this.uiPanelUsersSearchContainer = new Janus.Windows.UI.Dock.UIPanelInnerContainer();
            this.pnlSearch = new System.Windows.Forms.Panel();
            this.cbMediaLevel = new Janus.Windows.EditControls.UIComboBox();
            this.ebSearchKey = new Janus.Windows.GridEX.EditControls.EditBox();
            this.btnSearch = new Janus.Windows.EditControls.UIButton();
            this.uiPanelUserList = new Janus.Windows.UI.Dock.UIPanel();
            this.uiPanelUserListContainer = new Janus.Windows.UI.Dock.UIPanelInnerContainer();
            this.grdExCategoryList = new Janus.Windows.GridEX.GridEX();
            this.dvCategory = new System.Data.DataView();
            this.categoryDs = new AdManagerClient.CategoryDs();
            this.uiPanelUserDetail = new Janus.Windows.UI.Dock.UIPanel();
            this.uiPanelUserDetailContainer = new Janus.Windows.UI.Dock.UIPanelInnerContainer();
            this.pnlUserDetail = new System.Windows.Forms.Panel();
            this.label3 = new System.Windows.Forms.Label();
            this.p_InventoryRate = new Janus.Windows.GridEX.EditControls.NumericEditBox();
            this.p_Inventory = new Janus.Windows.EditControls.UICheckBox();
            this.p_Css = new Janus.Windows.EditControls.UICheckBox();
            this.p_SortNo = new Janus.Windows.GridEX.EditControls.IntegerUpDown();
            this.p_Flag = new Janus.Windows.EditControls.UICheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btnDelete = new Janus.Windows.EditControls.UIButton();
            this.btnAdd = new Janus.Windows.EditControls.UIButton();
            this.btnSave = new Janus.Windows.EditControls.UIButton();
            this.ebCategoryName = new Janus.Windows.GridEX.EditControls.EditBox();
            this.ebMediaName = new Janus.Windows.GridEX.EditControls.EditBox();
            this.lbCategoryName = new System.Windows.Forms.Label();
            this.lbMediaName = new System.Windows.Forms.Label();
            this.ebModDt = new Janus.Windows.GridEX.EditControls.EditBox();
            this.lbModDt = new System.Windows.Forms.Label();
            this.ebUserName = new Janus.Windows.GridEX.EditControls.EditBox();
            this.uiButton1 = new Janus.Windows.EditControls.UIButton();
            this.uiButton2 = new Janus.Windows.EditControls.UIButton();
            ((System.ComponentModel.ISupportInitialize)(this.uiPM)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelUsers)).BeginInit();
            this.uiPanelUsers.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelUsersSearch)).BeginInit();
            this.uiPanelUsersSearch.SuspendLayout();
            this.uiPanelUsersSearchContainer.SuspendLayout();
            this.pnlSearch.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelUserList)).BeginInit();
            this.uiPanelUserList.SuspendLayout();
            this.uiPanelUserListContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdExCategoryList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dvCategory)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.categoryDs)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelUserDetail)).BeginInit();
            this.uiPanelUserDetail.SuspendLayout();
            this.uiPanelUserDetailContainer.SuspendLayout();
            this.pnlUserDetail.SuspendLayout();
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
            this.uiPanelUsers.Id = new System.Guid("b162c53a-7940-47cd-a0ed-9762c59c50d8");
            this.uiPanelUsers.StaticGroup = true;
            this.uiPanelUsersSearch.Id = new System.Guid("afb3b7d0-aa79-443c-a1e8-3baba38f7a4d");
            this.uiPanelUsers.Panels.Add(this.uiPanelUsersSearch);
            this.uiPanelUserList.Id = new System.Guid("8e3140b9-bfcb-43af-b84e-2ee6cddf51dc");
            this.uiPanelUsers.Panels.Add(this.uiPanelUserList);
            this.uiPanelUserDetail.Id = new System.Guid("b2973a57-d3e1-4d80-9387-7a6dc3332542");
            this.uiPanelUsers.Panels.Add(this.uiPanelUserDetail);
            this.uiPM.Panels.Add(this.uiPanelUsers);
            // 
            // Design Time Panel Info:
            // 
            this.uiPM.BeginPanelInfo();
            this.uiPM.AddDockPanelInfo(new System.Guid("b162c53a-7940-47cd-a0ed-9762c59c50d8"), Janus.Windows.UI.Dock.PanelGroupStyle.HorizontalTiles, Janus.Windows.UI.Dock.PanelDockStyle.Fill, true, new System.Drawing.Size(1010, 677), true);
            this.uiPM.AddDockPanelInfo(new System.Guid("afb3b7d0-aa79-443c-a1e8-3baba38f7a4d"), new System.Guid("b162c53a-7940-47cd-a0ed-9762c59c50d8"), 42, true);
            this.uiPM.AddDockPanelInfo(new System.Guid("8e3140b9-bfcb-43af-b84e-2ee6cddf51dc"), new System.Guid("b162c53a-7940-47cd-a0ed-9762c59c50d8"), 457, true);
            this.uiPM.AddDockPanelInfo(new System.Guid("b2973a57-d3e1-4d80-9387-7a6dc3332542"), new System.Guid("b162c53a-7940-47cd-a0ed-9762c59c50d8"), 122, true);
            this.uiPM.AddFloatingPanelInfo(new System.Guid("b162c53a-7940-47cd-a0ed-9762c59c50d8"), Janus.Windows.UI.Dock.PanelGroupStyle.HorizontalTiles, true, new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
            this.uiPM.AddFloatingPanelInfo(new System.Guid("8e3140b9-bfcb-43af-b84e-2ee6cddf51dc"), new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
            this.uiPM.AddFloatingPanelInfo(new System.Guid("b2973a57-d3e1-4d80-9387-7a6dc3332542"), new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
            this.uiPM.AddFloatingPanelInfo(new System.Guid("afb3b7d0-aa79-443c-a1e8-3baba38f7a4d"), new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
            this.uiPM.EndPanelInfo();
            // 
            // uiPanelUsers
            // 
            this.uiPanelUsers.ActiveCaptionMode = Janus.Windows.UI.Dock.ActiveCaptionMode.Never;
            this.uiPanelUsers.BackColor = System.Drawing.SystemColors.Window;
            this.uiPanelUsers.CaptionVisible = Janus.Windows.UI.InheritableBoolean.True;
            this.uiPanelUsers.CloseButtonVisible = Janus.Windows.UI.InheritableBoolean.False;
            this.uiPanelUsers.Location = new System.Drawing.Point(0, 0);
            this.uiPanelUsers.Name = "uiPanelUsers";
            this.uiPanelUsers.Size = new System.Drawing.Size(1010, 677);
            this.uiPanelUsers.TabIndex = 4;
            this.uiPanelUsers.Text = "Ä«Å×°í¸®°ü¸®";
            // 
            // uiPanelUsersSearch
            // 
            this.uiPanelUsersSearch.AllowResize = Janus.Windows.UI.InheritableBoolean.False;
            this.uiPanelUsersSearch.CaptionVisible = Janus.Windows.UI.InheritableBoolean.False;
            this.uiPanelUsersSearch.CloseButtonVisible = Janus.Windows.UI.InheritableBoolean.False;
            this.uiPanelUsersSearch.InnerContainer = this.uiPanelUsersSearchContainer;
            this.uiPanelUsersSearch.Location = new System.Drawing.Point(0, 22);
            this.uiPanelUsersSearch.Name = "uiPanelUsersSearch";
            this.uiPanelUsersSearch.Size = new System.Drawing.Size(1010, 43);
            this.uiPanelUsersSearch.TabIndex = 4;
            this.uiPanelUsersSearch.Text = "°Ë»ö";
            // 
            // uiPanelUsersSearchContainer
            // 
            this.uiPanelUsersSearchContainer.Controls.Add(this.pnlSearch);
            this.uiPanelUsersSearchContainer.Location = new System.Drawing.Point(1, 1);
            this.uiPanelUsersSearchContainer.Name = "uiPanelUsersSearchContainer";
            this.uiPanelUsersSearchContainer.Size = new System.Drawing.Size(1008, 41);
            this.uiPanelUsersSearchContainer.TabIndex = 0;
            // 
            // pnlSearch
            // 
            this.pnlSearch.BackColor = System.Drawing.SystemColors.Window;
            this.pnlSearch.Controls.Add(this.cbMediaLevel);
            this.pnlSearch.Controls.Add(this.ebSearchKey);
            this.pnlSearch.Controls.Add(this.btnSearch);
            this.pnlSearch.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlSearch.Location = new System.Drawing.Point(0, 0);
            this.pnlSearch.Name = "pnlSearch";
            this.pnlSearch.Size = new System.Drawing.Size(1008, 41);
            this.pnlSearch.TabIndex = 3;
            // 
            // cbMediaLevel
            // 
            this.cbMediaLevel.ComboStyle = Janus.Windows.EditControls.ComboStyle.DropDownList;
            this.cbMediaLevel.Location = new System.Drawing.Point(8, 8);
            this.cbMediaLevel.Name = "cbMediaLevel";
            this.cbMediaLevel.Size = new System.Drawing.Size(160, 23);
            this.cbMediaLevel.TabIndex = 1;
            this.cbMediaLevel.Text = "¸ÅÃ¼¼±ÅÃ";
            this.cbMediaLevel.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            // 
            // ebSearchKey
            // 
            this.ebSearchKey.Location = new System.Drawing.Point(176, 8);
            this.ebSearchKey.Name = "ebSearchKey";
            this.ebSearchKey.Size = new System.Drawing.Size(208, 23);
            this.ebSearchKey.TabIndex = 2;
            this.ebSearchKey.Text = "°Ë»ö¾î¸¦ ÀÔ·ÂÇÏ¼¼¿ä";
            this.ebSearchKey.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
            this.ebSearchKey.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            this.ebSearchKey.TextChanged += new System.EventHandler(this.ebSearchKey_TextChanged);
            this.ebSearchKey.Click += new System.EventHandler(this.ebSearchKey_Click);
            this.ebSearchKey.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ebSearchKey_KeyDown);
            // 
            // btnSearch
            // 
            this.btnSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSearch.Enabled = false;
            this.btnSearch.Location = new System.Drawing.Point(893, 8);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(104, 24);
            this.btnSearch.TabIndex = 3;
            this.btnSearch.Text = "Á¶ È¸";
            this.btnSearch.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // uiPanelUserList
            // 
            this.uiPanelUserList.ActiveCaptionMode = Janus.Windows.UI.Dock.ActiveCaptionMode.Never;
            this.uiPanelUserList.BackColor = System.Drawing.SystemColors.InactiveCaptionText;
            this.uiPanelUserList.CaptionStyle = Janus.Windows.UI.Dock.PanelCaptionStyle.Dark;
            this.uiPanelUserList.CaptionVisible = Janus.Windows.UI.InheritableBoolean.True;
            this.uiPanelUserList.ImeMode = System.Windows.Forms.ImeMode.AlphaFull;
            this.uiPanelUserList.InnerContainer = this.uiPanelUserListContainer;
            this.uiPanelUserList.Location = new System.Drawing.Point(0, 69);
            this.uiPanelUserList.MinimumSize = new System.Drawing.Size(-1, 100);
            this.uiPanelUserList.Name = "uiPanelUserList";
            this.uiPanelUserList.Size = new System.Drawing.Size(1010, 477);
            this.uiPanelUserList.TabIndex = 4;
            this.uiPanelUserList.TabStop = false;
            this.uiPanelUserList.Text = "Ä«Å×°í¸®";
            // 
            // uiPanelUserListContainer
            // 
            this.uiPanelUserListContainer.BackColor = System.Drawing.SystemColors.Window;
            this.uiPanelUserListContainer.Controls.Add(this.grdExCategoryList);
            this.uiPanelUserListContainer.Location = new System.Drawing.Point(1, 23);
            this.uiPanelUserListContainer.Name = "uiPanelUserListContainer";
            this.uiPanelUserListContainer.Size = new System.Drawing.Size(1008, 453);
            this.uiPanelUserListContainer.TabIndex = 0;
            // 
            // grdExCategoryList
            // 
            this.grdExCategoryList.AlternatingColors = true;
            this.grdExCategoryList.AlternatingRowFormatStyle.BackgroundImageDrawMode = Janus.Windows.GridEX.BackgroundImageDrawMode.Tile;
            this.grdExCategoryList.BorderStyle = Janus.Windows.GridEX.BorderStyle.None;
            this.grdExCategoryList.DataSource = this.dvCategory;
            grdExCategoryList_DesignTimeLayout_Reference_0.Instance = ((object)(resources.GetObject("grdExCategoryList_DesignTimeLayout_Reference_0.Instance")));
            grdExCategoryList_DesignTimeLayout_Reference_1.Instance = ((object)(resources.GetObject("grdExCategoryList_DesignTimeLayout_Reference_1.Instance")));
            grdExCategoryList_DesignTimeLayout_Reference_2.Instance = ((object)(resources.GetObject("grdExCategoryList_DesignTimeLayout_Reference_2.Instance")));
            grdExCategoryList_DesignTimeLayout.LayoutReferences.AddRange(new Janus.Windows.Common.Layouts.JanusLayoutReference[] {
            grdExCategoryList_DesignTimeLayout_Reference_0,
            grdExCategoryList_DesignTimeLayout_Reference_1,
            grdExCategoryList_DesignTimeLayout_Reference_2});
            grdExCategoryList_DesignTimeLayout.LayoutString = resources.GetString("grdExCategoryList_DesignTimeLayout.LayoutString");
            this.grdExCategoryList.DesignTimeLayout = grdExCategoryList_DesignTimeLayout;
            this.grdExCategoryList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdExCategoryList.EditorsControlStyle.ButtonAppearance = Janus.Windows.GridEX.ButtonAppearance.Regular;
            this.grdExCategoryList.EmptyRows = true;
            this.grdExCategoryList.FocusCellFormatStyle.BackColor = System.Drawing.SystemColors.Highlight;
            this.grdExCategoryList.FocusCellFormatStyle.ForeColor = System.Drawing.Color.White;
            this.grdExCategoryList.FocusStyle = Janus.Windows.GridEX.FocusStyle.None;
            this.grdExCategoryList.Font = new System.Drawing.Font("¸¼Àº °íµñ", 8.25F);
            this.grdExCategoryList.FrozenColumns = 2;
            this.grdExCategoryList.GridLineColor = System.Drawing.Color.Silver;
            this.grdExCategoryList.GridLines = Janus.Windows.GridEX.GridLines.Vertical;
            this.grdExCategoryList.GridLineStyle = Janus.Windows.GridEX.GridLineStyle.Solid;
            this.grdExCategoryList.GroupByBoxVisible = false;
            this.grdExCategoryList.HeaderFormatStyle.TextAlignment = Janus.Windows.GridEX.TextAlignment.Center;
            this.grdExCategoryList.HideSelection = Janus.Windows.GridEX.HideSelection.HighlightInactive;
            this.grdExCategoryList.Location = new System.Drawing.Point(0, 0);
            this.grdExCategoryList.Name = "grdExCategoryList";
            this.grdExCategoryList.ScrollBars = Janus.Windows.GridEX.ScrollBars.Vertical;
            this.grdExCategoryList.SelectedInactiveFormatStyle.BackColor = System.Drawing.Color.Gold;
            this.grdExCategoryList.Size = new System.Drawing.Size(1008, 453);
            this.grdExCategoryList.TabIndex = 5;
            this.grdExCategoryList.TabKeyBehavior = Janus.Windows.GridEX.TabKeyBehavior.ControlNavigation;
            this.grdExCategoryList.ThemedAreas = ((Janus.Windows.GridEX.ThemedArea)(((((((((Janus.Windows.GridEX.ThemedArea.ScrollBars | Janus.Windows.GridEX.ThemedArea.EditControls) 
            | Janus.Windows.GridEX.ThemedArea.Headers) 
            | Janus.Windows.GridEX.ThemedArea.GroupByBox) 
            | Janus.Windows.GridEX.ThemedArea.GroupRows) 
            | Janus.Windows.GridEX.ThemedArea.ControlBorder) 
            | Janus.Windows.GridEX.ThemedArea.Cards) 
            | Janus.Windows.GridEX.ThemedArea.Gridlines) 
            | Janus.Windows.GridEX.ThemedArea.CheckBoxes)));
            this.grdExCategoryList.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            this.grdExCategoryList.FormattingRow += new Janus.Windows.GridEX.RowLoadEventHandler(this.grdExCategoryList_FormattingRow);
            this.grdExCategoryList.Enter += new System.EventHandler(this.OnGrdRowChanged);
            // 
            // dvCategory
            // 
            this.dvCategory.Table = this.categoryDs.Categorys;
            // 
            // categoryDs
            // 
            this.categoryDs.DataSetName = "CategoryDs";
            this.categoryDs.Locale = new System.Globalization.CultureInfo("en-US");
            this.categoryDs.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // uiPanelUserDetail
            // 
            this.uiPanelUserDetail.AllowResize = Janus.Windows.UI.InheritableBoolean.False;
            this.uiPanelUserDetail.BackColor = System.Drawing.SystemColors.Window;
            this.uiPanelUserDetail.CaptionStyle = Janus.Windows.UI.Dock.PanelCaptionStyle.Dark;
            this.uiPanelUserDetail.InnerContainer = this.uiPanelUserDetailContainer;
            this.uiPanelUserDetail.Location = new System.Drawing.Point(0, 550);
            this.uiPanelUserDetail.Name = "uiPanelUserDetail";
            this.uiPanelUserDetail.Size = new System.Drawing.Size(1010, 127);
            this.uiPanelUserDetail.TabIndex = 6;
            this.uiPanelUserDetail.TabStop = false;
            this.uiPanelUserDetail.Text = "»ó¼¼Á¤º¸";
            // 
            // uiPanelUserDetailContainer
            // 
            this.uiPanelUserDetailContainer.Controls.Add(this.pnlUserDetail);
            this.uiPanelUserDetailContainer.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.uiPanelUserDetailContainer.Location = new System.Drawing.Point(1, 23);
            this.uiPanelUserDetailContainer.Name = "uiPanelUserDetailContainer";
            this.uiPanelUserDetailContainer.Size = new System.Drawing.Size(1008, 103);
            this.uiPanelUserDetailContainer.TabIndex = 0;
            // 
            // pnlUserDetail
            // 
            this.pnlUserDetail.BackColor = System.Drawing.SystemColors.Window;
            this.pnlUserDetail.Controls.Add(this.label3);
            this.pnlUserDetail.Controls.Add(this.p_InventoryRate);
            this.pnlUserDetail.Controls.Add(this.p_Inventory);
            this.pnlUserDetail.Controls.Add(this.p_Css);
            this.pnlUserDetail.Controls.Add(this.p_SortNo);
            this.pnlUserDetail.Controls.Add(this.p_Flag);
            this.pnlUserDetail.Controls.Add(this.label2);
            this.pnlUserDetail.Controls.Add(this.btnDelete);
            this.pnlUserDetail.Controls.Add(this.btnAdd);
            this.pnlUserDetail.Controls.Add(this.btnSave);
            this.pnlUserDetail.Controls.Add(this.ebCategoryName);
            this.pnlUserDetail.Controls.Add(this.ebMediaName);
            this.pnlUserDetail.Controls.Add(this.lbCategoryName);
            this.pnlUserDetail.Controls.Add(this.lbMediaName);
            this.pnlUserDetail.Controls.Add(this.ebModDt);
            this.pnlUserDetail.Controls.Add(this.lbModDt);
            this.pnlUserDetail.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlUserDetail.Font = new System.Drawing.Font("¸¼Àº °íµñ", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.pnlUserDetail.Location = new System.Drawing.Point(0, 0);
            this.pnlUserDetail.Name = "pnlUserDetail";
            this.pnlUserDetail.Size = new System.Drawing.Size(1008, 103);
            this.pnlUserDetail.TabIndex = 3;
            // 
            // label3
            // 
            this.label3.BackColor = System.Drawing.SystemColors.Window;
            this.label3.Location = new System.Drawing.Point(476, 36);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(62, 16);
            this.label3.TabIndex = 39;
            this.label3.Text = "¼ÛÃâºñÀ²";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // p_InventoryRate
            // 
            this.p_InventoryRate.ButtonFont = new System.Drawing.Font("¸¼Àº °íµñ", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.p_InventoryRate.Font = new System.Drawing.Font("¸¼Àº °íµñ", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.p_InventoryRate.Location = new System.Drawing.Point(558, 36);
            this.p_InventoryRate.Name = "p_InventoryRate";
            this.p_InventoryRate.Size = new System.Drawing.Size(78, 23);
            this.p_InventoryRate.TabIndex = 38;
            this.p_InventoryRate.Text = "0.00";
            this.p_InventoryRate.TextAlignment = Janus.Windows.GridEX.TextAlignment.Center;
            this.p_InventoryRate.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.p_InventoryRate.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            // 
            // p_Inventory
            // 
            this.p_Inventory.Location = new System.Drawing.Point(370, 36);
            this.p_Inventory.Name = "p_Inventory";
            this.p_Inventory.Size = new System.Drawing.Size(98, 18);
            this.p_Inventory.TabIndex = 37;
            this.p_Inventory.Text = "ÀÎº¥Åä¸®¿©ºÎ";
            this.p_Inventory.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            // 
            // p_Css
            // 
            this.p_Css.Location = new System.Drawing.Point(266, 36);
            this.p_Css.Name = "p_Css";
            this.p_Css.Size = new System.Drawing.Size(76, 18);
            this.p_Css.TabIndex = 36;
            this.p_Css.Text = "CSS¿©ºÎ";
            this.p_Css.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            // 
            // p_SortNo
            // 
            this.p_SortNo.Location = new System.Drawing.Point(56, 36);
            this.p_SortNo.Name = "p_SortNo";
            this.p_SortNo.Size = new System.Drawing.Size(58, 22);
            this.p_SortNo.TabIndex = 35;
            this.p_SortNo.TextAlignment = Janus.Windows.GridEX.TextAlignment.Center;
            this.p_SortNo.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            // 
            // p_Flag
            // 
            this.p_Flag.Location = new System.Drawing.Point(180, 36);
            this.p_Flag.Name = "p_Flag";
            this.p_Flag.Size = new System.Drawing.Size(76, 18);
            this.p_Flag.TabIndex = 34;
            this.p_Flag.Text = "»ç¿ë¿©ºÎ";
            this.p_Flag.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.SystemColors.Window;
            this.label2.Location = new System.Drawing.Point(8, 36);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(48, 16);
            this.label2.TabIndex = 33;
            this.label2.Text = "¼ø¼­";
            // 
            // btnDelete
            // 
            this.btnDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnDelete.Enabled = false;
            this.btnDelete.Location = new System.Drawing.Point(120, 67);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(104, 24);
            this.btnDelete.TabIndex = 11;
            this.btnDelete.Text = "»è Á¦";
            this.btnDelete.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnAdd
            // 
            this.btnAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnAdd.Enabled = false;
            this.btnAdd.Location = new System.Drawing.Point(232, 67);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(104, 24);
            this.btnAdd.TabIndex = 12;
            this.btnAdd.Text = "Ãß °¡";
            this.btnAdd.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnSave.Enabled = false;
            this.btnSave.Location = new System.Drawing.Point(8, 67);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(104, 24);
            this.btnSave.TabIndex = 10;
            this.btnSave.Text = "Àú Àå";
            this.btnSave.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // ebCategoryName
            // 
            this.ebCategoryName.Location = new System.Drawing.Point(328, 9);
            this.ebCategoryName.MaxLength = 40;
            this.ebCategoryName.Name = "ebCategoryName";
            this.ebCategoryName.Size = new System.Drawing.Size(190, 22);
            this.ebCategoryName.TabIndex = 8;
            this.ebCategoryName.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
            this.ebCategoryName.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            // 
            // ebMediaName
            // 
            this.ebMediaName.Location = new System.Drawing.Point(56, 9);
            this.ebMediaName.MaxLength = 40;
            this.ebMediaName.Name = "ebMediaName";
            this.ebMediaName.Size = new System.Drawing.Size(190, 22);
            this.ebMediaName.TabIndex = 7;
            this.ebMediaName.TabStop = false;
            this.ebMediaName.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
            this.ebMediaName.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            // 
            // lbCategoryName
            // 
            this.lbCategoryName.Location = new System.Drawing.Point(256, 12);
            this.lbCategoryName.Name = "lbCategoryName";
            this.lbCategoryName.Size = new System.Drawing.Size(80, 16);
            this.lbCategoryName.TabIndex = 24;
            this.lbCategoryName.Text = "Ä«Å×°í¸®¸í";
            // 
            // lbMediaName
            // 
            this.lbMediaName.BackColor = System.Drawing.SystemColors.Window;
            this.lbMediaName.Location = new System.Drawing.Point(8, 12);
            this.lbMediaName.Name = "lbMediaName";
            this.lbMediaName.Size = new System.Drawing.Size(48, 16);
            this.lbMediaName.TabIndex = 18;
            this.lbMediaName.Text = "¸ÅÃ¼";
            // 
            // ebModDt
            // 
            this.ebModDt.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ebModDt.Location = new System.Drawing.Point(584, 9);
            this.ebModDt.Name = "ebModDt";
            this.ebModDt.ReadOnly = true;
            this.ebModDt.Size = new System.Drawing.Size(190, 22);
            this.ebModDt.TabIndex = 9;
            this.ebModDt.TabStop = false;
            this.ebModDt.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
            this.ebModDt.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            // 
            // lbModDt
            // 
            this.lbModDt.Location = new System.Drawing.Point(528, 12);
            this.lbModDt.Name = "lbModDt";
            this.lbModDt.Size = new System.Drawing.Size(72, 16);
            this.lbModDt.TabIndex = 31;
            this.lbModDt.Text = "¼öÁ¤ÀÏ½Ã";
            // 
            // ebUserName
            // 
            this.ebUserName.Location = new System.Drawing.Point(0, 0);
            this.ebUserName.Name = "ebUserName";
            this.ebUserName.Size = new System.Drawing.Size(0, 21);
            this.ebUserName.TabIndex = 0;
            this.ebUserName.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
            // 
            // uiButton1
            // 
            this.uiButton1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.uiButton1.Location = new System.Drawing.Point(136, 8);
            this.uiButton1.Name = "uiButton1";
            this.uiButton1.Size = new System.Drawing.Size(112, 24);
            this.uiButton1.TabIndex = 5;
            this.uiButton1.Text = "Àú Àå";
            this.uiButton1.VisualStyle = Janus.Windows.UI.VisualStyle.Office2003;
            // 
            // uiButton2
            // 
            this.uiButton2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.uiButton2.Location = new System.Drawing.Point(8, 8);
            this.uiButton2.Name = "uiButton2";
            this.uiButton2.Size = new System.Drawing.Size(120, 24);
            this.uiButton2.TabIndex = 6;
            this.uiButton2.Text = "Ãß °¡";
            this.uiButton2.VisualStyle = Janus.Windows.UI.VisualStyle.Office2003;
            // 
            // CategoryControl
            // 
            this.Controls.Add(this.uiPanelUsers);
            this.Font = new System.Drawing.Font("¸¼Àº °íµñ", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.Name = "CategoryControl";
            this.Size = new System.Drawing.Size(1010, 677);
            this.Load += new System.EventHandler(this.UserControl_Load);
            ((System.ComponentModel.ISupportInitialize)(this.uiPM)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelUsers)).EndInit();
            this.uiPanelUsers.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelUsersSearch)).EndInit();
            this.uiPanelUsersSearch.ResumeLayout(false);
            this.uiPanelUsersSearchContainer.ResumeLayout(false);
            this.pnlSearch.ResumeLayout(false);
            this.pnlSearch.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelUserList)).EndInit();
            this.uiPanelUserList.ResumeLayout(false);
            this.uiPanelUserListContainer.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdExCategoryList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dvCategory)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.categoryDs)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelUserDetail)).EndInit();
            this.uiPanelUserDetail.ResumeLayout(false);
            this.uiPanelUserDetailContainer.ResumeLayout(false);
            this.pnlUserDetail.ResumeLayout(false);
            this.pnlUserDetail.PerformLayout();
            this.ResumeLayout(false);

		}
		#endregion

		#region ÄÁÆ®·Ñ ·Îµå
		private void UserControl_Load(object sender, System.EventArgs e)
		{
			// µ¥ÀÌÅÍ°ü¸®¿ë °´Ã¼»ý¼º
			dt = ((DataView)grdExCategoryList.DataSource).Table;  
			cm = (CurrencyManager) this.BindingContext[grdExCategoryList.DataSource]; 
			cm.PositionChanged += new System.EventHandler(OnGrdRowChanged); 

			// ÄÁÆ®·Ñ ÃÊ±âÈ­
			InitControl();
		}

		#endregion

		#region ÄÁÆ®·Ñ ÃÊ±âÈ­
		private void InitControl()
		{			
			ProgressStart();
			InitCombo();
			InitCombo_Level();
			// Á¶È¸±ÇÇÑ °Ë»ç
			if(menu.CanRead(MenuCode))
			{
				canRead = true;
				SearchCategory();
			}
			
			// Ãß°¡¹öÆ° È°¼ºÈ­
			//if(menu.CanCreate(MenuCode))
			//{
			//	canCreate = true;
			//}

			// »èÁ¦¹öÆ° È°¼ºÈ­
			//if(menu.CanDelete(MenuCode))
			//{
			//	canDelete = true;
			//}

			// ÀúÀå¹öÆ° È°¼ºÈ­
			if(menu.CanUpdate(MenuCode))
			{
				ResetTextReadonly();
				canUpdate = true;
			}
			else
			{
				SetTextReadonly();
			}			
			InitButton();			
			SetTextReadonly();
			ProgressStop();
		}

		private void InitCombo()
		{
			// ÄÚµå¿¡¼­ º¸¾È·¹º§À» Á¶È¸ÇÑ´Ù.
			MediaCodeModel mediacodeModel = new MediaCodeModel();
			new MediaCodeManager(systemModel, commonModel).GetMediaCodeList(mediacodeModel);
			
			if (mediacodeModel.ResultCD.Equals("0000"))
			{
				// µ¥ÀÌÅÍ¼Â¿¡ ¼ÂÆÃ
				Utility.SetDataTable(categoryDs.Medias, mediacodeModel.MediaCodeDataSet);				
			}

			// »ó¼¼Á¶È¸ ÄÞº¸
			// »ó¼¼Á¤º¸ÀÇ ÄÞº¸´Â DatasetÀ» µ¥ÀÌÅÍ¼Ò½º·Î °¡Áø´Ù.

			// °Ë»öÁ¶°ÇÀÇ ÄÞº¸
			this.cbMediaLevel.Items.Clear();
			
			// ÄÞº¸¹Ú½º¿¡ ¼ÂÆ®ÇÒ ÄÚµå¸ñ·ÏÀ» ´ãÀ» Item¹è¿­À» ¼±¾ð
			Janus.Windows.EditControls.UIComboBoxItem[] comboItems = new Janus.Windows.EditControls.UIComboBoxItem[mediacodeModel.ResultCnt + 1];

			comboItems[0] = new Janus.Windows.EditControls.UIComboBoxItem("¸ÅÃ¼¸í¼±ÅÃ","00");
			
			for(int i=0;i<mediacodeModel.ResultCnt;i++)
			{
				DataRow row = categoryDs.Medias.Rows[i];

				string val = row["MediaCode"].ToString();
				string txt = row["MediaName"].ToString();
				comboItems[i+1] = new Janus.Windows.EditControls.UIComboBoxItem(txt,val);
			}
			// ÄÞº¸¿¡ ¼ÂÆ®
			this.cbMediaLevel.Items.AddRange(comboItems);
			this.cbMediaLevel.SelectedIndex = 0;

			Application.DoEvents();
		}

		private void InitCombo_Level()
		{			
			// ÄÞº¸ÇÈ½º					
			if(commonModel.UserLevel=="20")
			{
				cbMediaLevel.SelectedValue = commonModel.MediaCode;			
				cbMediaLevel.ReadOnly = true;				
			}
			else
			{
				for(int i=0;i < categoryDs.Medias.Rows.Count;i++)
				{
					DataRow row = categoryDs.Medias.Rows[i];					
					if(row["MediaCode"].ToString().Equals(FrameSystem._HANATV.ToString()))
					{
						cbMediaLevel.SelectedValue = FrameSystem._HANATV; // ÇÏ³ªTV¸¦ ±âº»°ªÀ¸·Î ÇÑ´Ù.	 		
						break;															
					}
					else
					{
						cbMediaLevel.SelectedValue="00";
					}
				}	
			}

			Application.DoEvents();
		}

		private void InitButton()
		{
			if(canRead)   btnSearch.Enabled = true;
			if(canCreate) btnAdd.Enabled    = true;

			if(ebMediaName.Text.Trim().Length > 0) 
			{
				if(canDelete) btnDelete.Enabled = true;
				if(canUpdate) btnSave.Enabled   = true;
			}
			Application.DoEvents();
		}

		private void DisableButton()
		{
			btnSearch.Enabled = false;
			btnAdd.Enabled    = false;
			btnSave.Enabled   = false;
			btnDelete.Enabled = false;
			Application.DoEvents();
		}

		#endregion

		#region »ç¿ëÀÚ ¾×¼ÇÃ³¸® ¸Þ¼Òµå

		/// <summary>
		/// ±×¸®µåÀÇ Rowº¯°æ½Ã
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void OnGrdRowChanged(object sender, System.EventArgs e) 
		{
            if (!IsSearching)
            {
                if (grdExCategoryList.RecordCount > 0)
                {
                    SetCategoryDetailText();
                }
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
			ProgressStart();
			DisableButton();
			SearchCategory();
			InitButton();
			ProgressStop();
		}

		/// <summary>
		/// Ãß°¡¹öÆ° Å¬¸¯
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnAdd_Click(object sender, System.EventArgs e)
		{
			btnAdd.Enabled    = false;
			btnDelete.Enabled = false;
			btnSave.Enabled   = true;

			IsAdding = true;

			ResetTextReadonly();
			ResetCategoryDetailText();

			ebCategoryName.Focus();
		}

		/// <summary>
		/// ÀúÀå¹öÆ° Å¬¸¯
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnSave_Click(object sender, System.EventArgs e)
		{			
			SaveUserDetail();			
		}

		/// <summary>
		/// »èÁ¦¹öÆ° Å¬¸¯
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnDelete_Click(object sender, System.EventArgs e)
		{			
			DeleteCategory();		
		}


		/// <summary>
		/// °Ë»ö¾î º¯°æ
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void ebSearchKey_TextChanged(object sender, System.EventArgs e)
		{
			IsNewSearchKey = false;
		}

		/// <summary>
		/// °Ë»ö¾î Å¬¸¯ 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void ebSearchKey_Click(object sender, System.EventArgs e)
		{
			if(IsNewSearchKey)
			{
				ebSearchKey.Text = "";
			}
			else
			{
				ebSearchKey.SelectAll();
			}
		}

		private void ebSearchKey_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
		{
			if(e.KeyCode == Keys.Enter)
			{
				SearchCategory();
			}
		}

		#endregion

		#region Ã³¸®¸Þ¼Òµå

		/// <summary>
		/// »ç¿ëÀÚ¸ñ·Ï Á¶È¸
		/// </summary>
		private void SearchCategory()
		{
            IsSearching = true;

			StatusMessage("Ä«Å×°í¸® Á¤º¸¸¦ Á¶È¸ÇÕ´Ï´Ù.");

            try
            {
                categoryModel.Init();
                // µ¥ÀÌÅÍ¸ðµ¨¿¡ Àü¼ÛÇÒ ³»¿ëÀ» ¼ÂÆ®ÇÑ´Ù.
                if (IsNewSearchKey)
                {
                    categoryModel.SearchKey = "";
                }
                else
                {
                    categoryModel.SearchKey = ebSearchKey.Text;
                }

                categoryModel.SearchCategoryLevel = cbMediaLevel.SelectedItem.Value.ToString();

                ResetCategoryDetailText();

                // »ç¿ëÀÚ¸ñ·ÏÁ¶È¸ ¼­ºñ½º¸¦ È£ÃâÇÑ´Ù.
                new CategoryManager(systemModel, commonModel).GetCategoryList(categoryModel);

                if (categoryModel.ResultCD.Equals("0000"))
                {
                    Utility.SetDataTable(categoryDs.Categorys, categoryModel.UserDataSet);
                    StatusMessage(categoryModel.ResultCnt + "°ÇÀÇ Ä«Å×°í¸® Á¤º¸°¡ Á¶È¸µÇ¾ú½À´Ï´Ù.");
                    if (canUpdate)
                    {
                        AddSchChoice();
                    }

                    SetCategoryDetailText();
                    //ListCheck_Category();
                }
            }
            catch (FrameException fe)
            {
                FrameSystem.showMsgForm("Ä«Å×°í¸®Á¶È¸¿À·ù", new string[] { fe.ErrCode, fe.ResultMsg });
            }
            catch (Exception ex)
            {
                FrameSystem.showMsgForm("Ä«Å×°í¸®Á¶È¸¿À·ù", new string[] { "", ex.Message });
            }
            finally
            {
                IsSearching = false;
            }
		}

		/// <summary>
		/// Å°¯“À»Ã£¾Æ ±×¸®µå Å°¿¡ ÇØ´çµÇ´Â·Î¿ì·Î..
		/// </summary>
		private void AddSchChoice()
		{
			StatusMessage("Å°¯“");		

			try
			{
				int rowIndex = 0;
				if ( categoryDs.Tables["Categorys"].Rows.Count < 1 ) return;
              
				foreach (DataRow row in categoryDs.Tables["Categorys"].Rows)
				{					
					if(IsAdding)
					{
						cm.Position = 0;
						categoryCode = null;
					}
					else
					{						
						if(row["CategoryCode"].ToString().Equals(categoryCode))
						{					
							cm.Position = rowIndex;
							break;								
						}
					}

					rowIndex++;
					grdExCategoryList.EnsureVisible();
				}
			}
			catch(FrameException fe)
			{
				FrameSystem.showMsgForm("Å°¯“¿À·ù", new string[] {fe.ErrCode, fe.ResultMsg});
			}
			catch(Exception ex)
			{
				FrameSystem.showMsgForm("Å°¯“¿À·ù",new string[] {"",ex.Message});
			}			
		}

		private void ListCheck_Category()
		{

			// Ã¼Å©µÈ ¸ðµç Ç×¸ñÀ» Å¬¸®¾î
			grdExCategoryList.UnCheckAllRecords();
			grdExCategoryList.UpdateData();
				   			
			foreach (Janus.Windows.GridEX.GridEXRow gr in grdExCategoryList.GetRows())
			{
				gr.Cells["CheckYn"].Value = Janus.Windows.GridEX.TriState.True;
				
			}
			// µ¥ÀÌÅÍ Å¬¸®¾î
			for(int i=0;i < categoryDs.Categorys.Count;i++)
			{
				DataRow Row = categoryDs.Categorys.Rows[i];
				if(Row["CheckYn"].ToString().Equals("Y"))
				{				
					foreach (Janus.Windows.GridEX.GridEXRow gr in grdExCategoryList.GetRows())
					{		
						gr.BeginEdit();
						gr.Cells["CheckYn"].Value = Janus.Windows.GridEX.TriState.True;
						gr.EndEdit();
					}
//					dt.Rows[i].BeginEdit();
//					dt.Rows[i]["CheckYn"]= Janus.Windows.GridEX.TriState.True;
//					dt.Rows[i].EndEdit();
				}								
			}
		}

		/// <summary>
		/// »ç¿ëÀÚ»ó¼¼Á¤º¸ ÀúÀå
		/// </summary>
		private void SaveUserDetail()
		{
			StatusMessage("Ä«Å×°í¸® Á¤º¸¸¦ ÀúÀåÇÕ´Ï´Ù.");

			grdExCategoryList.UpdateData();
			if(ebCategoryName.Text.Trim().Length == 0) 
			{
				MessageBox.Show("Ä«Å×°í¸®¸íÀÌ ÀÔ·ÂµÇÁö ¾Ê¾Ò½À´Ï´Ù.","Ä«Å×°í¸® ÀúÀå", MessageBoxButtons.OK, MessageBoxIcon.Information );
				ebCategoryName.Focus();
				return;				
			}
			
			try
			{	
				categoryModel.Init();
				categoryModel.MediaCode		= mediaCode;
				categoryModel.CategoryCode	= categoryCode;
				categoryModel.CategoryName	= ebCategoryName.Text.Trim();
				categoryModel.Flag			= p_Flag.Checked == true ? "Y" : "N";
				categoryModel.SortNo		= p_SortNo.Value;	
				categoryModel.CssFlag		= p_Css.Checked == true ? "Y" : "N";
				categoryModel.InventoryYn	= p_Inventory.Checked == true ? "Y" : "N";
				categoryModel.InventoryRate = Convert.ToDecimal(p_InventoryRate.Value);
			
				// »ç¿ëÀÚ »ó¼¼Á¤º¸ ÀúÀå ¼­ºñ½º¸¦ È£ÃâÇÑ´Ù.						
				new CategoryManager(systemModel,commonModel).SetCategoryUpdate(categoryModel);
				StatusMessage("Ä«Å×°í¸® Á¤º¸°¡ ÀúÀåµÇ¾ú½À´Ï´Ù.");

				DisableButton();
				SearchCategory();
				InitButton();
			}
			catch(FrameException fe)
			{
				FrameSystem.showMsgForm("Ä«Å×°í¸®Á¤º¸ ÀúÀå¿À·ù", new string[] {fe.ErrCode, fe.ResultMsg});
			}
			catch(Exception ex)
			{
				FrameSystem.showMsgForm("Ä«Å×°í¸®Á¤º¸ ÀúÀå¿À·ù",new string[] {"",ex.Message});
			}			
		}

		/// <summary>
		/// »ç¿ëÀÚÁ¤º¸ »èÁ¦
		/// </summary>
		private void DeleteCategory()
		{
			StatusMessage("Ä«Å×°í¸® Á¤º¸¸¦ »èÁ¦ÇÕ´Ï´Ù.");

			if(ebMediaName.Text.Trim().Length == 0) 
			{
				MessageBox.Show("»èÁ¦ÇÒ Ä«Å×°í¸® Á¤º¸°¡ ¾ø½À´Ï´Ù.","Ä«Å×°í¸® »èÁ¦", 
					MessageBoxButtons.OK, MessageBoxIcon.Information );
				return;
			}

			DialogResult result = MessageBox.Show("ÇØ´ç Ä«Å×°í¸® Á¤º¸¸¦ »èÁ¦ ÇÏ½Ã°Ú½À´Ï±î?","Ä«Å×°í¸® »èÁ¦",
				MessageBoxButtons.YesNo, MessageBoxIcon.Question,
				MessageBoxDefaultButton.Button2);

			if (result == DialogResult.No) return;

			try
			{
				// µ¥ÀÌÅÍ¸ðµ¨¿¡ Àü¼ÛÇÒ ³»¿ëÀ» ¼ÂÆ®ÇÑ´Ù.
				categoryModel.MediaCode       = mediaCode.Trim();
				categoryModel.CategoryCode       = categoryCode.Trim();

				// »ç¿ëÀÚ »ó¼¼Á¤º¸ ÀúÀå ¼­ºñ½º¸¦ È£ÃâÇÑ´Ù.
				new CategoryManager(systemModel,commonModel).SetCategoryDelete(categoryModel);
				
				ResetCategoryDetailText();				
				StatusMessage("Ä«Å×°í¸® Á¤º¸°¡ »èÁ¦µÇ¾ú½À´Ï´Ù.");			
						
				DisableButton();
				SearchCategory();
				InitButton();
			}
			catch(FrameException fe)
			{
				FrameSystem.showMsgForm("Ä«Å×°í¸®Á¤º¸ »èÁ¦¿À·ù", new string[] {fe.ErrCode, fe.ResultMsg});
			}
			catch(Exception ex)
			{
				FrameSystem.showMsgForm("Ä«Å×°í¸®Á¤º¸ »èÁ¦¿À·ù",new string[] {"",ex.Message});
			}

		}
		
		/// <summary>
		/// »ç¿ëÀÚ »ó¼¼Á¤º¸ÀÇ ¼ÂÆ®
		/// </summary>
		private void SetCategoryDetailText()
		{
			int curRow = cm.Position;
			if(curRow >= 0)
			{
				mediaCode				= dt.Rows[curRow]["MediaCode"].ToString();
				categoryCode			= dt.Rows[curRow]["CategoryCode"].ToString();			
				ebMediaName.Text		= dt.Rows[curRow]["MediaName"].ToString();
				ebCategoryName.Text		= dt.Rows[curRow]["CategoryName"].ToString();
				ebModDt.Text			= dt.Rows[curRow]["ModDt"].ToString();

//				<xs:element name="Flag" type="xs:boolean" minOccurs="0" />
//				<xs:element name="CheckYn" type="xs:boolean" minOccurs="0" />
//				<xs:element name="FlagName" type="xs:boolean" minOccurs="0" />
//				<xs:element name="InventoryYn" type="xs:boolean" minOccurs="0" />
//				<xs:element name="SortNo" type="xs:short" minOccurs="0" />
//				<xs:element name="InventoryRate" type="xs:decimal" minOccurs="0" />
				p_SortNo.TextBox.Text	= dt.Rows[curRow]["SortNo"].ToString();
				p_InventoryRate.Text	= dt.Rows[curRow]["InventoryRate"].ToString();

				if(dt.Rows[curRow]["Flag"].ToString().Equals("True"))		p_Flag.Checked	= true;						
				else														p_Flag.Checked	= false;

				if(dt.Rows[curRow]["CheckYn"].ToString().Equals("True"))	p_Css.Checked	= true;						
				else														p_Css.Checked	= false;

				if(dt.Rows[curRow]["InventoryYn"].ToString().Equals("True"))	p_Inventory.Checked	= true;						
				else															p_Inventory.Checked	= false;

				IsAdding = false;
				ResetTextReadonly();
			}

			StatusMessage("ÁØºñ");
		}

		private void ResetCategoryDetailText()
		{
			ebMediaName.Text              = "";			
			ebCategoryName.Text           = "";		
            ebModDt.Text                  = "";
			if(!IsAdding)
			{
				ebMediaName.ReadOnly         = true;
				ebCategoryName.ReadOnly         = true;
				ebMediaName.BackColor        = Color.WhiteSmoke;
				ebCategoryName.BackColor        = Color.WhiteSmoke;
			}
		}
		
		/// <summary>
		/// »ó¼¼Á¤º¸ ReadOnly
		/// </summary>
		private void SetTextReadonly()
		{
			ebMediaName.ReadOnly         = true;			
			ebCategoryName.ReadOnly       = true;			

			ebMediaName.BackColor        = Color.WhiteSmoke;	
			ebCategoryName.BackColor      = Color.WhiteSmoke;			
		}

		/// <summary>
		/// »ó¼¼Á¤º¸ ¼öÁ¤°¡´ÉÄÉ
		/// </summary>
		private void ResetTextReadonly()
		{			
			ebMediaName.ReadOnly       = true;			
			ebCategoryName.ReadOnly       = false;			
			ebMediaName.BackColor      = Color.WhiteSmoke;
			ebCategoryName.BackColor      = Color.White;
					
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

        private void grdExCategoryList_FormattingRow(object sender, Janus.Windows.GridEX.RowLoadEventArgs e)
        {

        }

	}
}