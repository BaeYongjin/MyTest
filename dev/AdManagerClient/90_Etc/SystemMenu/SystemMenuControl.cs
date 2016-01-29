// ===============================================================================
// SystemMenuControl for Charites Project
//
// SystemMenuControl.cs
//
// ¸Þ´ºÁ¤º¸°ü¸® ÄÁµå·ÑÀ» Á¤ÀÇÇÕ´Ï´Ù. 
//
// ===============================================================================
// Release history
//
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
using System.Windows.Forms;
using System.Diagnostics;

using WinFramework.Misc;
using WinFramework.Base;
using WinFramework.Data;

using AdManagerModel;
namespace AdManagerClient
{
	/// <summary>
	/// ¸Þ´º°ü¸® ÄÁÆ®·Ñ
	/// </summary>
	public class SystemMenuControl : System.Windows.Forms.UserControl, IUserControl
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
		// ¸Þ´º¸Þ´º : º¸¾ÈÀÌ ÇÊ¿äÇÑ È­¸é¿¡ ÇÊ¿äÇÔ
		public string        menuCode		= "";

		// »ç¿ëÇÒ Á¤º¸¸ðµ¨
		SystemMenuModel systemMenuModel  = new SystemMenuModel();	// ¸Þ´ºÁ¤º¸¸ðµ¨

		// È­¸éÃ³¸®¿ë º¯¼ö
		bool IsNewSearchKey		  = true;					// °Ë»ö¾îÀÔ·Â ¿©ºÎ
		CurrencyManager cm        = null;					// µ¥ÀÌÅÍ ±×¸®µåÀÇ º¯°æ¿¡ µû¸¥ µ¥ÀÌÅÍ¼Â °ü¸®¸¦ À§ÇÏ¿©			
		CurrencyManager cmChild        = null;					// µ¥ÀÌÅÍ ±×¸®µåÀÇ º¯°æ¿¡ µû¸¥ µ¥ÀÌÅÍ¼Â °ü¸®¸¦ À§ÇÏ¿©			
		DataTable       dt        = null;
		DataTable       dtChild        = null;		
		
		private string        upperCode = null;
		private string        nowCode = null;
        private string        nowCodeDetail = null;
		
		public string keyMenuOrder = "";		// ÆË¾÷¿¡¼­µµ »ç¿ëÇÏ±âÀ§ÇØ  public
		string keyLastOrder     = "";

		const int ORDER_UP    = 3;
		const int ORDER_DOWN  = 4;

		private string        nowCode_old = null;
		
		bool IsAdding             = false;
		bool canRead			  = false;
		bool canUpdate			  = false;
		bool canCreate            = false;
		bool canDelete            = false;
		
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
		private Janus.Windows.UI.Dock.UIPanel uiPanelUsersSearch;
		private Janus.Windows.UI.Dock.UIPanelInnerContainer uiPanelUsersSearchContainer;
		private Janus.Windows.EditControls.UIButton uiButton2;
		private Janus.Windows.EditControls.UIButton uiButton1;
		private Janus.Windows.UI.Dock.UIPanelGroup uiPanelUsers;
		private System.Windows.Forms.Panel pnlSearch;
		private Janus.Windows.EditControls.UIButton btnSearch;
		private Janus.Windows.EditControls.UIButton btnSave;
		private Janus.Windows.UI.Dock.UIPanelGroup uiPanelChannelSet;
		private Janus.Windows.UI.Dock.UIPanel uiPanel1;
		private Janus.Windows.UI.Dock.UIPanelInnerContainer uiPanel1Container;
		private Janus.Windows.UI.Dock.UIPanel uiPanel2;
		private Janus.Windows.UI.Dock.UIPanelInnerContainer uiPanel2Container;
		private Janus.Windows.UI.Dock.UIPanelGroup uiPanel0;
		private Janus.Windows.UI.Dock.UIPanel uiPanel3;
		private Janus.Windows.UI.Dock.UIPanelInnerContainer uiPanel3Container;
		private Janus.Windows.UI.Dock.UIPanel uiPanel4;
		private Janus.Windows.UI.Dock.UIPanelInnerContainer uiPanel4Container;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Panel panel2;
		private Janus.Windows.GridEX.EditControls.EditBox ebMenuCode;
		private Janus.Windows.GridEX.EditControls.EditBox ebMenuName2;
		private Janus.Windows.EditControls.UIButton btnSave2;
		private Janus.Windows.GridEX.EditControls.EditBox ebMenuName;
		private Janus.Windows.GridEX.EditControls.EditBox ebMenuCode2;
		private Janus.Windows.EditControls.UIRadioButton rbUseYn_N;
		private Janus.Windows.EditControls.UIRadioButton rbUseYn_Y;
		private System.Windows.Forms.Label lbUseYn;
		private Janus.Windows.EditControls.UIRadioButton rbUseYn_N2;
		private Janus.Windows.EditControls.UIRadioButton rbUseYn_Y2;
		private System.Windows.Forms.Label lbUseYn2;
		private System.Windows.Forms.Label lbMenuCode;
		private System.Windows.Forms.Label lbMenuName;
		private System.Windows.Forms.Label lbMenuCode2;
		private System.Windows.Forms.Label lbMenuName2;
		private System.Windows.Forms.Label lbUpperMenu;
		private Janus.Windows.GridEX.EditControls.EditBox ebUpperMenu;
		private Janus.Windows.GridEX.EditControls.EditBox ebMenuOrder;
		private System.Windows.Forms.Label lbMenuOrder;
		private System.Data.DataView dvUpperMenu;
		private System.Data.DataView dvMenuCode;
		private Janus.Windows.EditControls.UIButton btnAdd;
		private Janus.Windows.EditControls.UIButton btnDelete;
		private Janus.Windows.EditControls.UIButton btnAdd2;
		private Janus.Windows.EditControls.UIButton btnDelete2;
		private Janus.Windows.EditControls.UIButton btnOrderUp;
		private Janus.Windows.EditControls.UIButton btnOrderDown;
		private AdManagerClient._90_Etc.SystemMenu.SystemMenuDs systemMenuDs;
		private Janus.Windows.GridEX.GridEX grdExUpperMenuList;
		private Janus.Windows.GridEX.GridEX grdExMenuCodeList;		
		private System.ComponentModel.IContainer components;

		public SystemMenuControl()
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

		#region ±¸¼º ¿ä¼Ò µðÀÚÀÌ³Ê¿¡¼­ »ý¼ºÇÑ ¸Þ´º
		/// <summary> 
		/// µðÀÚÀÌ³Ê Áö¿ø¿¡ ÇÊ¿äÇÑ ¸Þ¼­µåÀÔ´Ï´Ù. 
		/// ÀÌ ¸Þ¼­µåÀÇ ³»¿ëÀ» ¸Þ´º ÆíÁý±â·Î ¼öÁ¤ÇÏÁö ¸¶½Ê½Ã¿À.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			Janus.Windows.GridEX.GridEXLayout grdExUpperMenuList_DesignTimeLayout = new Janus.Windows.GridEX.GridEXLayout();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SystemMenuControl));
			Janus.Windows.GridEX.GridEXLayout grdExUpperMenuList_Layout_0 = new Janus.Windows.GridEX.GridEXLayout();
			Janus.Windows.GridEX.GridEXLayout grdExMenuCodeList_DesignTimeLayout = new Janus.Windows.GridEX.GridEXLayout();
			this.uiPM = new Janus.Windows.UI.Dock.UIPanelManager(this.components);
			this.uiPanelUsers = new Janus.Windows.UI.Dock.UIPanelGroup();
			this.uiPanelUsersSearch = new Janus.Windows.UI.Dock.UIPanel();
			this.uiPanelUsersSearchContainer = new Janus.Windows.UI.Dock.UIPanelInnerContainer();
			this.pnlSearch = new System.Windows.Forms.Panel();
			this.btnSearch = new Janus.Windows.EditControls.UIButton();
			this.uiPanelChannelSet = new Janus.Windows.UI.Dock.UIPanelGroup();
			this.uiPanel1 = new Janus.Windows.UI.Dock.UIPanel();
			this.uiPanel1Container = new Janus.Windows.UI.Dock.UIPanelInnerContainer();
			this.grdExUpperMenuList = new Janus.Windows.GridEX.GridEX();
			this.dvUpperMenu = new System.Data.DataView();
			this.systemMenuDs = new AdManagerClient._90_Etc.SystemMenu.SystemMenuDs();
			this.uiPanel2 = new Janus.Windows.UI.Dock.UIPanel();
			this.uiPanel2Container = new Janus.Windows.UI.Dock.UIPanelInnerContainer();
			this.grdExMenuCodeList = new Janus.Windows.GridEX.GridEX();
			this.dvMenuCode = new System.Data.DataView();
			this.uiPanel0 = new Janus.Windows.UI.Dock.UIPanelGroup();
			this.uiPanel3 = new Janus.Windows.UI.Dock.UIPanel();
			this.uiPanel3Container = new Janus.Windows.UI.Dock.UIPanelInnerContainer();
			this.panel1 = new System.Windows.Forms.Panel();
			this.rbUseYn_N = new Janus.Windows.EditControls.UIRadioButton();
			this.rbUseYn_Y = new Janus.Windows.EditControls.UIRadioButton();
			this.lbUseYn = new System.Windows.Forms.Label();
			this.btnSave = new Janus.Windows.EditControls.UIButton();
			this.lbMenuName = new System.Windows.Forms.Label();
			this.ebMenuCode = new Janus.Windows.GridEX.EditControls.EditBox();
			this.lbMenuCode = new System.Windows.Forms.Label();
			this.ebMenuName = new Janus.Windows.GridEX.EditControls.EditBox();
			this.btnAdd2 = new Janus.Windows.EditControls.UIButton();
			this.btnDelete2 = new Janus.Windows.EditControls.UIButton();
			this.uiPanel4 = new Janus.Windows.UI.Dock.UIPanel();
			this.uiPanel4Container = new Janus.Windows.UI.Dock.UIPanelInnerContainer();
			this.panel2 = new System.Windows.Forms.Panel();
			this.btnOrderUp = new Janus.Windows.EditControls.UIButton();
			this.btnOrderDown = new Janus.Windows.EditControls.UIButton();
			this.ebMenuName2 = new Janus.Windows.GridEX.EditControls.EditBox();
			this.rbUseYn_N2 = new Janus.Windows.EditControls.UIRadioButton();
			this.rbUseYn_Y2 = new Janus.Windows.EditControls.UIRadioButton();
			this.lbUseYn2 = new System.Windows.Forms.Label();
			this.btnAdd = new Janus.Windows.EditControls.UIButton();
			this.btnSave2 = new Janus.Windows.EditControls.UIButton();
			this.btnDelete = new Janus.Windows.EditControls.UIButton();
			this.lbMenuName2 = new System.Windows.Forms.Label();
			this.ebMenuCode2 = new Janus.Windows.GridEX.EditControls.EditBox();
			this.lbMenuCode2 = new System.Windows.Forms.Label();
			this.ebUpperMenu = new Janus.Windows.GridEX.EditControls.EditBox();
			this.lbUpperMenu = new System.Windows.Forms.Label();
			this.ebMenuOrder = new Janus.Windows.GridEX.EditControls.EditBox();
			this.lbMenuOrder = new System.Windows.Forms.Label();
			this.uiButton1 = new Janus.Windows.EditControls.UIButton();
			this.uiButton2 = new Janus.Windows.EditControls.UIButton();
			((System.ComponentModel.ISupportInitialize)(this.uiPM)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.uiPanelUsers)).BeginInit();
			this.uiPanelUsers.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.uiPanelUsersSearch)).BeginInit();
			this.uiPanelUsersSearch.SuspendLayout();
			this.uiPanelUsersSearchContainer.SuspendLayout();
			this.pnlSearch.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.uiPanelChannelSet)).BeginInit();
			this.uiPanelChannelSet.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.uiPanel1)).BeginInit();
			this.uiPanel1.SuspendLayout();
			this.uiPanel1Container.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.grdExUpperMenuList)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.dvUpperMenu)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.systemMenuDs)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.uiPanel2)).BeginInit();
			this.uiPanel2.SuspendLayout();
			this.uiPanel2Container.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.grdExMenuCodeList)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.dvMenuCode)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.uiPanel0)).BeginInit();
			this.uiPanel0.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.uiPanel3)).BeginInit();
			this.uiPanel3.SuspendLayout();
			this.uiPanel3Container.SuspendLayout();
			this.panel1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.uiPanel4)).BeginInit();
			this.uiPanel4.SuspendLayout();
			this.uiPanel4Container.SuspendLayout();
			this.panel2.SuspendLayout();
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
			this.uiPanelUsers.Id = new System.Guid("b162c53a-7940-47cd-a0ed-9762c59c50d8");
			this.uiPanelUsers.StaticGroup = true;
			this.uiPanelUsersSearch.Id = new System.Guid("afb3b7d0-aa79-443c-a1e8-3baba38f7a4d");
			this.uiPanelUsers.Panels.Add(this.uiPanelUsersSearch);
			this.uiPanelChannelSet.Id = new System.Guid("88f86479-98e4-43d4-bf03-12fd789fcfc5");
			this.uiPanelChannelSet.StaticGroup = true;
			this.uiPanel1.Id = new System.Guid("91eeb7fc-856c-4a9c-b37e-db742cfa9b2e");
			this.uiPanelChannelSet.Panels.Add(this.uiPanel1);
			this.uiPanel2.Id = new System.Guid("3d64f731-de32-42ba-a10f-ac3b5ac56703");
			this.uiPanelChannelSet.Panels.Add(this.uiPanel2);
			this.uiPanelUsers.Panels.Add(this.uiPanelChannelSet);
			this.uiPanel0.Id = new System.Guid("b7763a2e-3139-4238-9443-b9c640f9b161");
			this.uiPanel0.StaticGroup = true;
			this.uiPanel3.Id = new System.Guid("db7d4ea4-ff77-441c-8e7a-1b3472f1c6a1");
			this.uiPanel0.Panels.Add(this.uiPanel3);
			this.uiPanel4.Id = new System.Guid("5a21a057-3899-434e-b2dd-9c167240e82c");
			this.uiPanel0.Panels.Add(this.uiPanel4);
			this.uiPanelUsers.Panels.Add(this.uiPanel0);
			this.uiPM.Panels.Add(this.uiPanelUsers);
			// 
			// Design Time Panel Info:
			// 
			this.uiPM.BeginPanelInfo();
			this.uiPM.AddDockPanelInfo(new System.Guid("b162c53a-7940-47cd-a0ed-9762c59c50d8"), Janus.Windows.UI.Dock.PanelGroupStyle.HorizontalTiles, Janus.Windows.UI.Dock.PanelDockStyle.Fill, true, new System.Drawing.Size(1010, 677), true);
			this.uiPM.AddDockPanelInfo(new System.Guid("afb3b7d0-aa79-443c-a1e8-3baba38f7a4d"), new System.Guid("b162c53a-7940-47cd-a0ed-9762c59c50d8"), 15, true);
			this.uiPM.AddDockPanelInfo(new System.Guid("88f86479-98e4-43d4-bf03-12fd789fcfc5"), new System.Guid("b162c53a-7940-47cd-a0ed-9762c59c50d8"), Janus.Windows.UI.Dock.PanelGroupStyle.VerticalTiles, true, 412, true);
			this.uiPM.AddDockPanelInfo(new System.Guid("91eeb7fc-856c-4a9c-b37e-db742cfa9b2e"), new System.Guid("88f86479-98e4-43d4-bf03-12fd789fcfc5"), 425, true);
			this.uiPM.AddDockPanelInfo(new System.Guid("3d64f731-de32-42ba-a10f-ac3b5ac56703"), new System.Guid("88f86479-98e4-43d4-bf03-12fd789fcfc5"), 422, true);
			this.uiPM.AddDockPanelInfo(new System.Guid("b7763a2e-3139-4238-9443-b9c640f9b161"), new System.Guid("b162c53a-7940-47cd-a0ed-9762c59c50d8"), Janus.Windows.UI.Dock.PanelGroupStyle.VerticalTiles, true, 200, true);
			this.uiPM.AddDockPanelInfo(new System.Guid("db7d4ea4-ff77-441c-8e7a-1b3472f1c6a1"), new System.Guid("b7763a2e-3139-4238-9443-b9c640f9b161"), 851, true);
			this.uiPM.AddDockPanelInfo(new System.Guid("5a21a057-3899-434e-b2dd-9c167240e82c"), new System.Guid("b7763a2e-3139-4238-9443-b9c640f9b161"), 851, true);
			this.uiPM.AddFloatingPanelInfo(new System.Guid("b162c53a-7940-47cd-a0ed-9762c59c50d8"), Janus.Windows.UI.Dock.PanelGroupStyle.HorizontalTiles, true, new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
			this.uiPM.AddFloatingPanelInfo(new System.Guid("8e3140b9-bfcb-43af-b84e-2ee6cddf51dc"), new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
			this.uiPM.AddFloatingPanelInfo(new System.Guid("b2973a57-d3e1-4d80-9387-7a6dc3332542"), new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
			this.uiPM.AddFloatingPanelInfo(new System.Guid("afb3b7d0-aa79-443c-a1e8-3baba38f7a4d"), new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
			this.uiPM.AddFloatingPanelInfo(new System.Guid("88f86479-98e4-43d4-bf03-12fd789fcfc5"), Janus.Windows.UI.Dock.PanelGroupStyle.VerticalTiles, true, new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
			this.uiPM.AddFloatingPanelInfo(new System.Guid("91eeb7fc-856c-4a9c-b37e-db742cfa9b2e"), new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
			this.uiPM.AddFloatingPanelInfo(new System.Guid("3d64f731-de32-42ba-a10f-ac3b5ac56703"), new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
			this.uiPM.AddFloatingPanelInfo(new System.Guid("fa8fbe33-6177-4be9-bda1-6ab92554d537"), Janus.Windows.UI.Dock.PanelGroupStyle.VerticalTiles, true, new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
			this.uiPM.AddFloatingPanelInfo(new System.Guid("2791500d-a408-4486-b827-d21b0c36c0b5"), Janus.Windows.UI.Dock.PanelGroupStyle.VerticalTiles, true, new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
			this.uiPM.AddFloatingPanelInfo(new System.Guid("0760187d-5fa5-4611-8c57-a5c04c97933f"), new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
			this.uiPM.AddFloatingPanelInfo(new System.Guid("d93d7173-f8c4-4e7f-96e0-ae1646f5a34f"), new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
			this.uiPM.AddFloatingPanelInfo(new System.Guid("8704f307-2516-422f-8b1d-631ccca32ac5"), Janus.Windows.UI.Dock.PanelGroupStyle.VerticalTiles, true, new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
			this.uiPM.AddFloatingPanelInfo(new System.Guid("f8cb4a0d-0380-45b8-8085-ed8885a8540b"), new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
			this.uiPM.AddFloatingPanelInfo(new System.Guid("198426e2-57ad-4794-a381-9f3a70f9e918"), new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
			this.uiPM.AddFloatingPanelInfo(new System.Guid("a396871e-517a-402b-833e-bcb171a79e0b"), Janus.Windows.UI.Dock.PanelGroupStyle.VerticalTiles, true, new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
			this.uiPM.AddFloatingPanelInfo(new System.Guid("c9913cfc-b119-499a-ae33-79987cf29558"), new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
			this.uiPM.AddFloatingPanelInfo(new System.Guid("a99e7bf9-1111-44d1-b38d-706b7c4c617d"), Janus.Windows.UI.Dock.PanelGroupStyle.VerticalTiles, true, new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
			this.uiPM.AddFloatingPanelInfo(new System.Guid("d2c1d4ca-04e6-4ca5-9812-a52ac971b680"), Janus.Windows.UI.Dock.PanelGroupStyle.VerticalTiles, true, new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
			this.uiPM.AddFloatingPanelInfo(new System.Guid("88b6ee95-850c-4491-bd56-18fbe5248fbf"), Janus.Windows.UI.Dock.PanelGroupStyle.VerticalTiles, true, new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
			this.uiPM.AddFloatingPanelInfo(new System.Guid("6289c73c-ab78-41a6-9d5d-136425dab349"), Janus.Windows.UI.Dock.PanelGroupStyle.VerticalTiles, true, new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
			this.uiPM.AddFloatingPanelInfo(new System.Guid("389993cc-0ea6-43d1-adf8-d970f6ff132d"), new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
			this.uiPM.AddFloatingPanelInfo(new System.Guid("213d032b-eb9f-48fc-889b-6f5e39179637"), Janus.Windows.UI.Dock.PanelGroupStyle.VerticalTiles, true, new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
			this.uiPM.AddFloatingPanelInfo(new System.Guid("b46811d1-f763-45c0-a4bc-353fd30e747c"), Janus.Windows.UI.Dock.PanelGroupStyle.VerticalTiles, true, new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
			this.uiPM.AddFloatingPanelInfo(new System.Guid("26cbbd26-78b8-4438-a054-9484797d8045"), Janus.Windows.UI.Dock.PanelGroupStyle.VerticalTiles, true, new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
			this.uiPM.AddFloatingPanelInfo(new System.Guid("55cfaebd-2c8f-4974-a061-271f973b64e9"), Janus.Windows.UI.Dock.PanelGroupStyle.VerticalTiles, true, new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
			this.uiPM.AddFloatingPanelInfo(new System.Guid("b7763a2e-3139-4238-9443-b9c640f9b161"), Janus.Windows.UI.Dock.PanelGroupStyle.VerticalTiles, true, new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
			this.uiPM.AddFloatingPanelInfo(new System.Guid("c1720263-710a-43fc-9ea8-d52a94281711"), new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
			this.uiPM.AddFloatingPanelInfo(new System.Guid("ebbc55e9-5d36-40ce-956b-d5dca9b1d329"), Janus.Windows.UI.Dock.PanelGroupStyle.VerticalTiles, true, new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
			this.uiPM.AddFloatingPanelInfo(new System.Guid("d512cfd3-17ee-4cbd-a6bf-89c89fcc6561"), Janus.Windows.UI.Dock.PanelGroupStyle.VerticalTiles, true, new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
			this.uiPM.AddFloatingPanelInfo(new System.Guid("8c29ed5e-1886-43d9-8557-069a51dd59e8"), Janus.Windows.UI.Dock.PanelGroupStyle.VerticalTiles, true, new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
			this.uiPM.AddFloatingPanelInfo(new System.Guid("635250f5-4079-4a21-9b89-ca25e2dd0b7e"), Janus.Windows.UI.Dock.PanelGroupStyle.VerticalTiles, true, new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
			this.uiPM.AddFloatingPanelInfo(new System.Guid("db7d4ea4-ff77-441c-8e7a-1b3472f1c6a1"), new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
			this.uiPM.AddFloatingPanelInfo(new System.Guid("5a21a057-3899-434e-b2dd-9c167240e82c"), new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
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
			this.uiPanelUsers.TabIndex = 0;
			this.uiPanelUsers.Text = "¸Þ´º°ü¸®1";
			// 
			// uiPanelUsersSearch
			// 
			this.uiPanelUsersSearch.AllowResize = Janus.Windows.UI.InheritableBoolean.False;
			this.uiPanelUsersSearch.CaptionVisible = Janus.Windows.UI.InheritableBoolean.False;
			this.uiPanelUsersSearch.CloseButtonVisible = Janus.Windows.UI.InheritableBoolean.False;
			this.uiPanelUsersSearch.InnerContainer = this.uiPanelUsersSearchContainer;
			this.uiPanelUsersSearch.Location = new System.Drawing.Point(0, 22);
			this.uiPanelUsersSearch.Name = "uiPanelUsersSearch";
			this.uiPanelUsersSearch.Size = new System.Drawing.Size(1010, 40);
			this.uiPanelUsersSearch.TabIndex = 0;
			this.uiPanelUsersSearch.Text = "°Ë»ö";
			// 
			// uiPanelUsersSearchContainer
			// 
			this.uiPanelUsersSearchContainer.Controls.Add(this.pnlSearch);
			this.uiPanelUsersSearchContainer.Location = new System.Drawing.Point(1, 1);
			this.uiPanelUsersSearchContainer.Name = "uiPanelUsersSearchContainer";
			this.uiPanelUsersSearchContainer.Size = new System.Drawing.Size(1008, 38);
			this.uiPanelUsersSearchContainer.TabIndex = 0;
			// 
			// pnlSearch
			// 
			this.pnlSearch.BackColor = System.Drawing.SystemColors.Window;
			this.pnlSearch.Controls.Add(this.btnSearch);
			this.pnlSearch.Dock = System.Windows.Forms.DockStyle.Fill;
			this.pnlSearch.Location = new System.Drawing.Point(0, 0);
			this.pnlSearch.Name = "pnlSearch";
			this.pnlSearch.Size = new System.Drawing.Size(1008, 38);
			this.pnlSearch.TabIndex = 0;
			// 
			// btnSearch
			// 
			this.btnSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnSearch.Enabled = false;
			this.btnSearch.Location = new System.Drawing.Point(887, 8);
			this.btnSearch.Name = "btnSearch";
			this.btnSearch.Size = new System.Drawing.Size(104, 24);
			this.btnSearch.TabIndex = 4;
			this.btnSearch.Text = "Á¶ È¸";
			this.btnSearch.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
			this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
			// 
			// uiPanelChannelSet
			// 
			this.uiPanelChannelSet.CaptionStyle = Janus.Windows.UI.Dock.PanelCaptionStyle.Dark;
			this.uiPanelChannelSet.GroupStyle = Janus.Windows.UI.Dock.PanelGroupStyle.VerticalTiles;
			this.uiPanelChannelSet.Location = new System.Drawing.Point(0, 66);
			this.uiPanelChannelSet.Name = "uiPanelChannelSet";
			this.uiPanelChannelSet.Size = new System.Drawing.Size(1010, 401);
			this.uiPanelChannelSet.TabIndex = 0;
			this.uiPanelChannelSet.Text = "¸Þ´º°ü¸®";
			// 
			// uiPanel1
			// 
			this.uiPanel1.InnerContainer = this.uiPanel1Container;
			this.uiPanel1.Location = new System.Drawing.Point(0, 22);
			this.uiPanel1.Name = "uiPanel1";
			this.uiPanel1.Size = new System.Drawing.Size(505, 379);
			this.uiPanel1.TabIndex = 0;
			this.uiPanel1.Text = "¸Þ´º±×·ì";
			// 
			// uiPanel1Container
			// 
			this.uiPanel1Container.Controls.Add(this.grdExUpperMenuList);
			this.uiPanel1Container.Location = new System.Drawing.Point(1, 23);
			this.uiPanel1Container.Name = "uiPanel1Container";
			this.uiPanel1Container.Size = new System.Drawing.Size(503, 355);
			this.uiPanel1Container.TabIndex = 0;
			// 
			// grdExUpperMenuList
			// 
			this.grdExUpperMenuList.AllowEdit = Janus.Windows.GridEX.InheritableBoolean.False;
			this.grdExUpperMenuList.AlternatingColors = true;
			this.grdExUpperMenuList.AlternatingRowFormatStyle.BackgroundImageDrawMode = Janus.Windows.GridEX.BackgroundImageDrawMode.Tile;
			this.grdExUpperMenuList.AutomaticSort = false;
			this.grdExUpperMenuList.BorderStyle = Janus.Windows.GridEX.BorderStyle.None;
			this.grdExUpperMenuList.DataSource = this.dvUpperMenu;
			grdExUpperMenuList_DesignTimeLayout.LayoutString = resources.GetString("grdExUpperMenuList_DesignTimeLayout.LayoutString");
			this.grdExUpperMenuList.DesignTimeLayout = grdExUpperMenuList_DesignTimeLayout;
			this.grdExUpperMenuList.Dock = System.Windows.Forms.DockStyle.Fill;
			this.grdExUpperMenuList.EditorsControlStyle.ButtonAppearance = Janus.Windows.GridEX.ButtonAppearance.Regular;
			this.grdExUpperMenuList.EmptyRows = true;
			this.grdExUpperMenuList.FocusCellFormatStyle.BackColor = System.Drawing.SystemColors.Highlight;
			this.grdExUpperMenuList.FocusCellFormatStyle.ForeColor = System.Drawing.Color.White;
			this.grdExUpperMenuList.FocusStyle = Janus.Windows.GridEX.FocusStyle.None;
			this.grdExUpperMenuList.FrozenColumns = 2;
			this.grdExUpperMenuList.GridLineColor = System.Drawing.Color.Silver;
			this.grdExUpperMenuList.GridLines = Janus.Windows.GridEX.GridLines.Vertical;
			this.grdExUpperMenuList.GridLineStyle = Janus.Windows.GridEX.GridLineStyle.Solid;
			this.grdExUpperMenuList.GroupByBoxVisible = false;
			this.grdExUpperMenuList.GroupMode = Janus.Windows.GridEX.GroupMode.Collapsed;
			this.grdExUpperMenuList.GroupRowVisualStyle = Janus.Windows.GridEX.GroupRowVisualStyle.Outlook2003;
			this.grdExUpperMenuList.HeaderFormatStyle.TextAlignment = Janus.Windows.GridEX.TextAlignment.Center;
			this.grdExUpperMenuList.HideSelection = Janus.Windows.GridEX.HideSelection.HighlightInactive;
			grdExUpperMenuList_Layout_0.Key = "bea";
			this.grdExUpperMenuList.Layouts.AddRange(new Janus.Windows.GridEX.GridEXLayout[] {
            grdExUpperMenuList_Layout_0});
			this.grdExUpperMenuList.Location = new System.Drawing.Point(0, 0);
			this.grdExUpperMenuList.Name = "grdExUpperMenuList";
			this.grdExUpperMenuList.ScrollBars = Janus.Windows.GridEX.ScrollBars.Vertical;
			this.grdExUpperMenuList.SelectedInactiveFormatStyle.BackColor = System.Drawing.Color.Gold;
			this.grdExUpperMenuList.Size = new System.Drawing.Size(503, 355);
			this.grdExUpperMenuList.TabIndex = 4;
			this.grdExUpperMenuList.TabKeyBehavior = Janus.Windows.GridEX.TabKeyBehavior.ControlNavigation;
			this.grdExUpperMenuList.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
			this.grdExUpperMenuList.Enter += new System.EventHandler(this.OnGrdRowChanged);
			// 
			// dvUpperMenu
			// 
			this.dvUpperMenu.Table = this.systemMenuDs.UpperMenu;
			// 
			// systemMenuDs
			// 
			this.systemMenuDs.DataSetName = "SystemMenuDs";
			this.systemMenuDs.Locale = new System.Globalization.CultureInfo("en-US");
			this.systemMenuDs.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
			// 
			// uiPanel2
			// 
			this.uiPanel2.InnerContainer = this.uiPanel2Container;
			this.uiPanel2.Location = new System.Drawing.Point(509, 22);
			this.uiPanel2.Name = "uiPanel2";
			this.uiPanel2.Size = new System.Drawing.Size(501, 379);
			this.uiPanel2.TabIndex = 0;
			this.uiPanel2.Text = "¸Þ´º";
			// 
			// uiPanel2Container
			// 
			this.uiPanel2Container.Controls.Add(this.grdExMenuCodeList);
			this.uiPanel2Container.Location = new System.Drawing.Point(1, 23);
			this.uiPanel2Container.Name = "uiPanel2Container";
			this.uiPanel2Container.Size = new System.Drawing.Size(499, 355);
			this.uiPanel2Container.TabIndex = 0;
			// 
			// grdExMenuCodeList
			// 
			this.grdExMenuCodeList.AllowEdit = Janus.Windows.GridEX.InheritableBoolean.False;
			this.grdExMenuCodeList.AlternatingColors = true;
			this.grdExMenuCodeList.AutomaticSort = false;
			this.grdExMenuCodeList.BorderStyle = Janus.Windows.GridEX.BorderStyle.None;
			this.grdExMenuCodeList.DataSource = this.dvMenuCode;
			grdExMenuCodeList_DesignTimeLayout.LayoutString = resources.GetString("grdExMenuCodeList_DesignTimeLayout.LayoutString");
			this.grdExMenuCodeList.DesignTimeLayout = grdExMenuCodeList_DesignTimeLayout;
			this.grdExMenuCodeList.Dock = System.Windows.Forms.DockStyle.Fill;
			this.grdExMenuCodeList.EditorsControlStyle.ButtonAppearance = Janus.Windows.GridEX.ButtonAppearance.Regular;
			this.grdExMenuCodeList.EmptyRows = true;
			this.grdExMenuCodeList.FocusCellFormatStyle.BackColor = System.Drawing.SystemColors.Highlight;
			this.grdExMenuCodeList.FocusCellFormatStyle.ForeColor = System.Drawing.Color.White;
			this.grdExMenuCodeList.FocusStyle = Janus.Windows.GridEX.FocusStyle.None;
			this.grdExMenuCodeList.FrozenColumns = 2;
			this.grdExMenuCodeList.GridLineColor = System.Drawing.Color.Silver;
			this.grdExMenuCodeList.GridLines = Janus.Windows.GridEX.GridLines.Vertical;
			this.grdExMenuCodeList.GridLineStyle = Janus.Windows.GridEX.GridLineStyle.Solid;
			this.grdExMenuCodeList.GroupByBoxVisible = false;
			this.grdExMenuCodeList.HeaderFormatStyle.TextAlignment = Janus.Windows.GridEX.TextAlignment.Center;
			this.grdExMenuCodeList.HideSelection = Janus.Windows.GridEX.HideSelection.HighlightInactive;
			this.grdExMenuCodeList.Location = new System.Drawing.Point(0, 0);
			this.grdExMenuCodeList.Name = "grdExMenuCodeList";
			this.grdExMenuCodeList.ScrollBars = Janus.Windows.GridEX.ScrollBars.Vertical;
			this.grdExMenuCodeList.SelectedInactiveFormatStyle.BackColor = System.Drawing.Color.Gold;
			this.grdExMenuCodeList.Size = new System.Drawing.Size(499, 355);
			this.grdExMenuCodeList.TabIndex = 4;
			this.grdExMenuCodeList.TabKeyBehavior = Janus.Windows.GridEX.TabKeyBehavior.ControlNavigation;
			this.grdExMenuCodeList.ThemedAreas = ((Janus.Windows.GridEX.ThemedArea)(((((((((Janus.Windows.GridEX.ThemedArea.ScrollBars | Janus.Windows.GridEX.ThemedArea.EditControls) 
            | Janus.Windows.GridEX.ThemedArea.Headers) 
            | Janus.Windows.GridEX.ThemedArea.GroupByBox) 
            | Janus.Windows.GridEX.ThemedArea.GroupRows) 
            | Janus.Windows.GridEX.ThemedArea.ControlBorder) 
            | Janus.Windows.GridEX.ThemedArea.Cards) 
            | Janus.Windows.GridEX.ThemedArea.Gridlines) 
            | Janus.Windows.GridEX.ThemedArea.CheckBoxes)));
			this.grdExMenuCodeList.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
			this.grdExMenuCodeList.Enter += new System.EventHandler(this.OnGrdRowDetailChanged);
			// 
			// dvMenuCode
			// 
			this.dvMenuCode.Table = this.systemMenuDs.MenuCode;
			// 
			// uiPanel0
			// 
			this.uiPanel0.AllowResize = Janus.Windows.UI.InheritableBoolean.False;
			this.uiPanel0.CaptionStyle = Janus.Windows.UI.Dock.PanelCaptionStyle.Dark;
			this.uiPanel0.GroupStyle = Janus.Windows.UI.Dock.PanelGroupStyle.VerticalTiles;
			this.uiPanel0.Location = new System.Drawing.Point(0, 471);
			this.uiPanel0.Name = "uiPanel0";
			this.uiPanel0.Size = new System.Drawing.Size(1010, 206);
			this.uiPanel0.TabIndex = 4;
			this.uiPanel0.Text = "¸Þ´º»ó¼¼";
			// 
			// uiPanel3
			// 
			this.uiPanel3.InnerContainer = this.uiPanel3Container;
			this.uiPanel3.Location = new System.Drawing.Point(0, 22);
			this.uiPanel3.Name = "uiPanel3";
			this.uiPanel3.Size = new System.Drawing.Size(503, 184);
			this.uiPanel3.TabIndex = 4;
			this.uiPanel3.Text = "¸Þ´º±×·ì»ó¼¼";
			// 
			// uiPanel3Container
			// 
			this.uiPanel3Container.Controls.Add(this.panel1);
			this.uiPanel3Container.ForeColor = System.Drawing.SystemColors.ControlText;
			this.uiPanel3Container.Location = new System.Drawing.Point(1, 23);
			this.uiPanel3Container.Name = "uiPanel3Container";
			this.uiPanel3Container.Size = new System.Drawing.Size(501, 160);
			this.uiPanel3Container.TabIndex = 0;
			// 
			// panel1
			// 
			this.panel1.Controls.Add(this.rbUseYn_N);
			this.panel1.Controls.Add(this.rbUseYn_Y);
			this.panel1.Controls.Add(this.lbUseYn);
			this.panel1.Controls.Add(this.btnSave);
			this.panel1.Controls.Add(this.lbMenuName);
			this.panel1.Controls.Add(this.ebMenuCode);
			this.panel1.Controls.Add(this.lbMenuCode);
			this.panel1.Controls.Add(this.ebMenuName);
			this.panel1.Controls.Add(this.btnAdd2);
			this.panel1.Controls.Add(this.btnDelete2);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel1.Location = new System.Drawing.Point(0, 0);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(501, 160);
			this.panel1.TabIndex = 19;
			// 
			// rbUseYn_N
			// 
			this.rbUseYn_N.BackColor = System.Drawing.Color.Transparent;
			this.rbUseYn_N.Location = new System.Drawing.Point(168, 56);
			this.rbUseYn_N.Name = "rbUseYn_N";
			this.rbUseYn_N.Size = new System.Drawing.Size(72, 23);
			this.rbUseYn_N.TabIndex = 6;
			this.rbUseYn_N.Text = "»ç¿ë¾ÈÇÔ";
			this.rbUseYn_N.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
			// 
			// rbUseYn_Y
			// 
			this.rbUseYn_Y.BackColor = System.Drawing.Color.Transparent;
			this.rbUseYn_Y.Location = new System.Drawing.Point(96, 56);
			this.rbUseYn_Y.Name = "rbUseYn_Y";
			this.rbUseYn_Y.Size = new System.Drawing.Size(72, 23);
			this.rbUseYn_Y.TabIndex = 6;
			this.rbUseYn_Y.Text = "»ç¿ëÇÔ";
			this.rbUseYn_Y.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
			// 
			// lbUseYn
			// 
			this.lbUseYn.Location = new System.Drawing.Point(8, 56);
			this.lbUseYn.Name = "lbUseYn";
			this.lbUseYn.Size = new System.Drawing.Size(72, 21);
			this.lbUseYn.TabIndex = 43;
			this.lbUseYn.Text = "»ç¿ë¿©ºÎ";
			this.lbUseYn.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// btnSave
			// 
			this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.btnSave.BackColor = System.Drawing.SystemColors.Window;
			this.btnSave.Enabled = false;
			this.btnSave.Location = new System.Drawing.Point(8, 131);
			this.btnSave.Name = "btnSave";
			this.btnSave.Size = new System.Drawing.Size(104, 24);
			this.btnSave.TabIndex = 7;
			this.btnSave.Text = "Àú Àå";
			this.btnSave.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
			this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
			// 
			// lbMenuName
			// 
			this.lbMenuName.Location = new System.Drawing.Point(8, 32);
			this.lbMenuName.Name = "lbMenuName";
			this.lbMenuName.Size = new System.Drawing.Size(72, 21);
			this.lbMenuName.TabIndex = 18;
			this.lbMenuName.Text = "¸Þ´º¸í";
			this.lbMenuName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// ebMenuCode
			// 
			this.ebMenuCode.Location = new System.Drawing.Point(96, 8);
			this.ebMenuCode.MaxLength = 120;
			this.ebMenuCode.Name = "ebMenuCode";
			this.ebMenuCode.Size = new System.Drawing.Size(136, 21);
			this.ebMenuCode.TabIndex = 5;
			this.ebMenuCode.TabStop = false;
			this.ebMenuCode.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
			this.ebMenuCode.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
			// 
			// lbMenuCode
			// 
			this.lbMenuCode.Location = new System.Drawing.Point(8, 8);
			this.lbMenuCode.Name = "lbMenuCode";
			this.lbMenuCode.Size = new System.Drawing.Size(80, 21);
			this.lbMenuCode.TabIndex = 0;
			this.lbMenuCode.Text = "¸Þ´º±×·ìÄÚµå";
			this.lbMenuCode.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// ebMenuName
			// 
			this.ebMenuName.Location = new System.Drawing.Point(96, 32);
			this.ebMenuName.MaxLength = 120;
			this.ebMenuName.Name = "ebMenuName";
			this.ebMenuName.Size = new System.Drawing.Size(136, 21);
			this.ebMenuName.TabIndex = 5;
			this.ebMenuName.TabStop = false;
			this.ebMenuName.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
			this.ebMenuName.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
			// 
			// btnAdd2
			// 
			this.btnAdd2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.btnAdd2.BackColor = System.Drawing.SystemColors.Window;
			this.btnAdd2.Enabled = false;
			this.btnAdd2.Location = new System.Drawing.Point(232, 131);
			this.btnAdd2.Name = "btnAdd2";
			this.btnAdd2.Size = new System.Drawing.Size(104, 24);
			this.btnAdd2.TabIndex = 9;
			this.btnAdd2.Text = "Ãß °¡";
			this.btnAdd2.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
			this.btnAdd2.Click += new System.EventHandler(this.btnAdd_Click2);
			// 
			// btnDelete2
			// 
			this.btnDelete2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.btnDelete2.Enabled = false;
			this.btnDelete2.Location = new System.Drawing.Point(120, 131);
			this.btnDelete2.Name = "btnDelete2";
			this.btnDelete2.Size = new System.Drawing.Size(104, 24);
			this.btnDelete2.TabIndex = 8;
			this.btnDelete2.Text = "»è Á¦";
			this.btnDelete2.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
			this.btnDelete2.Click += new System.EventHandler(this.btnDelete_Click2);
			// 
			// uiPanel4
			// 
			this.uiPanel4.InnerContainer = this.uiPanel4Container;
			this.uiPanel4.Location = new System.Drawing.Point(507, 22);
			this.uiPanel4.Name = "uiPanel4";
			this.uiPanel4.Size = new System.Drawing.Size(503, 184);
			this.uiPanel4.TabIndex = 4;
			this.uiPanel4.Text = "¸Þ´º»ó¼¼";
			// 
			// uiPanel4Container
			// 
			this.uiPanel4Container.Controls.Add(this.panel2);
			this.uiPanel4Container.Location = new System.Drawing.Point(1, 23);
			this.uiPanel4Container.Name = "uiPanel4Container";
			this.uiPanel4Container.Size = new System.Drawing.Size(501, 160);
			this.uiPanel4Container.TabIndex = 0;
			// 
			// panel2
			// 
			this.panel2.Controls.Add(this.btnOrderUp);
			this.panel2.Controls.Add(this.btnOrderDown);
			this.panel2.Controls.Add(this.ebMenuName2);
			this.panel2.Controls.Add(this.rbUseYn_N2);
			this.panel2.Controls.Add(this.rbUseYn_Y2);
			this.panel2.Controls.Add(this.lbUseYn2);
			this.panel2.Controls.Add(this.btnAdd);
			this.panel2.Controls.Add(this.btnSave2);
			this.panel2.Controls.Add(this.btnDelete);
			this.panel2.Controls.Add(this.lbMenuName2);
			this.panel2.Controls.Add(this.ebMenuCode2);
			this.panel2.Controls.Add(this.lbMenuCode2);
			this.panel2.Controls.Add(this.ebUpperMenu);
			this.panel2.Controls.Add(this.lbUpperMenu);
			this.panel2.Controls.Add(this.ebMenuOrder);
			this.panel2.Controls.Add(this.lbMenuOrder);
			this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel2.Location = new System.Drawing.Point(0, 0);
			this.panel2.Name = "panel2";
			this.panel2.Size = new System.Drawing.Size(501, 160);
			this.panel2.TabIndex = 14;
			// 
			// btnOrderUp
			// 
			this.btnOrderUp.Enabled = false;
			this.btnOrderUp.Location = new System.Drawing.Point(8, 96);
			this.btnOrderUp.Name = "btnOrderUp";
			this.btnOrderUp.Size = new System.Drawing.Size(104, 24);
			this.btnOrderUp.TabIndex = 12;
			this.btnOrderUp.Text = "¼ø¼­¿Ã¸²";
			this.btnOrderUp.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
			this.btnOrderUp.Click += new System.EventHandler(this.btnOrderUp_Click);
			// 
			// btnOrderDown
			// 
			this.btnOrderDown.Enabled = false;
			this.btnOrderDown.Location = new System.Drawing.Point(120, 96);
			this.btnOrderDown.Name = "btnOrderDown";
			this.btnOrderDown.Size = new System.Drawing.Size(104, 24);
			this.btnOrderDown.TabIndex = 13;
			this.btnOrderDown.Text = "¼ø¼­³»¸²";
			this.btnOrderDown.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
			this.btnOrderDown.Click += new System.EventHandler(this.btnOrderDown_Click);
			// 
			// ebMenuName2
			// 
			this.ebMenuName2.Location = new System.Drawing.Point(72, 32);
			this.ebMenuName2.MaxLength = 120;
			this.ebMenuName2.Name = "ebMenuName2";
			this.ebMenuName2.Size = new System.Drawing.Size(136, 21);
			this.ebMenuName2.TabIndex = 10;
			this.ebMenuName2.TabStop = false;
			this.ebMenuName2.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
			this.ebMenuName2.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
			// 
			// rbUseYn_N2
			// 
			this.rbUseYn_N2.BackColor = System.Drawing.Color.Transparent;
			this.rbUseYn_N2.Location = new System.Drawing.Point(144, 56);
			this.rbUseYn_N2.Name = "rbUseYn_N2";
			this.rbUseYn_N2.Size = new System.Drawing.Size(72, 23);
			this.rbUseYn_N2.TabIndex = 11;
			this.rbUseYn_N2.Text = "»ç¿ë¾ÈÇÔ";
			this.rbUseYn_N2.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
			// 
			// rbUseYn_Y2
			// 
			this.rbUseYn_Y2.BackColor = System.Drawing.Color.Transparent;
			this.rbUseYn_Y2.Location = new System.Drawing.Point(72, 56);
			this.rbUseYn_Y2.Name = "rbUseYn_Y2";
			this.rbUseYn_Y2.Size = new System.Drawing.Size(72, 23);
			this.rbUseYn_Y2.TabIndex = 11;
			this.rbUseYn_Y2.Text = "»ç¿ëÇÔ";
			this.rbUseYn_Y2.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
			// 
			// lbUseYn2
			// 
			this.lbUseYn2.Location = new System.Drawing.Point(8, 56);
			this.lbUseYn2.Name = "lbUseYn2";
			this.lbUseYn2.Size = new System.Drawing.Size(72, 21);
			this.lbUseYn2.TabIndex = 43;
			this.lbUseYn2.Text = "»ç¿ë¿©ºÎ";
			this.lbUseYn2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// btnAdd
			// 
			this.btnAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.btnAdd.BackColor = System.Drawing.SystemColors.Window;
			this.btnAdd.Enabled = false;
			this.btnAdd.Location = new System.Drawing.Point(232, 131);
			this.btnAdd.Name = "btnAdd";
			this.btnAdd.Size = new System.Drawing.Size(104, 24);
			this.btnAdd.TabIndex = 16;
			this.btnAdd.Text = "Ãß °¡";
			this.btnAdd.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
			this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
			// 
			// btnSave2
			// 
			this.btnSave2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.btnSave2.BackColor = System.Drawing.SystemColors.Window;
			this.btnSave2.Enabled = false;
			this.btnSave2.Location = new System.Drawing.Point(8, 131);
			this.btnSave2.Name = "btnSave2";
			this.btnSave2.Size = new System.Drawing.Size(104, 24);
			this.btnSave2.TabIndex = 14;
			this.btnSave2.Text = "Àú Àå";
			this.btnSave2.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
			this.btnSave2.Click += new System.EventHandler(this.btnSave2_Click);
			// 
			// btnDelete
			// 
			this.btnDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.btnDelete.Enabled = false;
			this.btnDelete.Location = new System.Drawing.Point(120, 131);
			this.btnDelete.Name = "btnDelete";
			this.btnDelete.Size = new System.Drawing.Size(104, 24);
			this.btnDelete.TabIndex = 15;
			this.btnDelete.Text = "»è Á¦";
			this.btnDelete.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
			this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
			// 
			// lbMenuName2
			// 
			this.lbMenuName2.Location = new System.Drawing.Point(8, 32);
			this.lbMenuName2.Name = "lbMenuName2";
			this.lbMenuName2.Size = new System.Drawing.Size(72, 21);
			this.lbMenuName2.TabIndex = 22;
			this.lbMenuName2.Text = "¸Þ´º¸í";
			this.lbMenuName2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// ebMenuCode2
			// 
			this.ebMenuCode2.Location = new System.Drawing.Point(72, 8);
			this.ebMenuCode2.MaxLength = 120;
			this.ebMenuCode2.Name = "ebMenuCode2";
			this.ebMenuCode2.Size = new System.Drawing.Size(136, 21);
			this.ebMenuCode2.TabIndex = 12;
			this.ebMenuCode2.TabStop = false;
			this.ebMenuCode2.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
			this.ebMenuCode2.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
			// 
			// lbMenuCode2
			// 
			this.lbMenuCode2.Location = new System.Drawing.Point(8, 8);
			this.lbMenuCode2.Name = "lbMenuCode2";
			this.lbMenuCode2.Size = new System.Drawing.Size(72, 21);
			this.lbMenuCode2.TabIndex = 19;
			this.lbMenuCode2.Text = "¸Þ´ºÄÚµå";
			this.lbMenuCode2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// ebUpperMenu
			// 
			this.ebUpperMenu.Location = new System.Drawing.Point(280, 8);
			this.ebUpperMenu.MaxLength = 120;
			this.ebUpperMenu.Name = "ebUpperMenu";
			this.ebUpperMenu.Size = new System.Drawing.Size(136, 21);
			this.ebUpperMenu.TabIndex = 13;
			this.ebUpperMenu.TabStop = false;
			this.ebUpperMenu.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
			this.ebUpperMenu.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
			// 
			// lbUpperMenu
			// 
			this.lbUpperMenu.Location = new System.Drawing.Point(216, 10);
			this.lbUpperMenu.Name = "lbUpperMenu";
			this.lbUpperMenu.Size = new System.Drawing.Size(72, 21);
			this.lbUpperMenu.TabIndex = 19;
			this.lbUpperMenu.Text = "»óÀ§¸Þ´º";
			this.lbUpperMenu.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// ebMenuOrder
			// 
			this.ebMenuOrder.Location = new System.Drawing.Point(280, 32);
			this.ebMenuOrder.MaxLength = 120;
			this.ebMenuOrder.Name = "ebMenuOrder";
			this.ebMenuOrder.Size = new System.Drawing.Size(136, 21);
			this.ebMenuOrder.TabIndex = 15;
			this.ebMenuOrder.TabStop = false;
			this.ebMenuOrder.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
			this.ebMenuOrder.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
			// 
			// lbMenuOrder
			// 
			this.lbMenuOrder.Location = new System.Drawing.Point(216, 32);
			this.lbMenuOrder.Name = "lbMenuOrder";
			this.lbMenuOrder.Size = new System.Drawing.Size(72, 21);
			this.lbMenuOrder.TabIndex = 19;
			this.lbMenuOrder.Text = "¸Þ´º¼ø¼­";
			this.lbMenuOrder.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// uiButton1
			// 
			this.uiButton1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.uiButton1.Location = new System.Drawing.Point(136, 8);
			this.uiButton1.Name = "uiButton1";
			this.uiButton1.Size = new System.Drawing.Size(112, 24);
			this.uiButton1.TabIndex = 0;
			this.uiButton1.Text = "Àú Àå";
			this.uiButton1.VisualStyle = Janus.Windows.UI.VisualStyle.Office2003;
			// 
			// uiButton2
			// 
			this.uiButton2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.uiButton2.Location = new System.Drawing.Point(8, 8);
			this.uiButton2.Name = "uiButton2";
			this.uiButton2.Size = new System.Drawing.Size(120, 24);
			this.uiButton2.TabIndex = 0;
			this.uiButton2.Text = "Ãß °¡";
			this.uiButton2.VisualStyle = Janus.Windows.UI.VisualStyle.Office2003;
			// 
			// SystemMenuControl
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.Controls.Add(this.uiPanelUsers);
			this.Font = global::AdManagerClient.Properties.Settings.Default.Font1;
			this.Name = "SystemMenuControl";
			this.Size = new System.Drawing.Size(1010, 677);
			this.Load += new System.EventHandler(this.SystemMenuControl_Load);
			((System.ComponentModel.ISupportInitialize)(this.uiPM)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.uiPanelUsers)).EndInit();
			this.uiPanelUsers.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.uiPanelUsersSearch)).EndInit();
			this.uiPanelUsersSearch.ResumeLayout(false);
			this.uiPanelUsersSearchContainer.ResumeLayout(false);
			this.pnlSearch.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.uiPanelChannelSet)).EndInit();
			this.uiPanelChannelSet.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.uiPanel1)).EndInit();
			this.uiPanel1.ResumeLayout(false);
			this.uiPanel1Container.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.grdExUpperMenuList)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.dvUpperMenu)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.systemMenuDs)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.uiPanel2)).EndInit();
			this.uiPanel2.ResumeLayout(false);
			this.uiPanel2Container.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.grdExMenuCodeList)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.dvMenuCode)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.uiPanel0)).EndInit();
			this.uiPanel0.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.uiPanel3)).EndInit();
			this.uiPanel3.ResumeLayout(false);
			this.uiPanel3Container.ResumeLayout(false);
			this.panel1.ResumeLayout(false);
			this.panel1.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.uiPanel4)).EndInit();
			this.uiPanel4.ResumeLayout(false);
			this.uiPanel4Container.ResumeLayout(false);
			this.panel2.ResumeLayout(false);
			this.panel2.PerformLayout();
			this.ResumeLayout(false);

		}
		#endregion

		#region ÄÁÆ®·Ñ ·Îµå
		private void SystemMenuControl_Load(object sender, System.EventArgs e)
		{		
			// µ¥ÀÌÅÍ°ü¸®¿ë °´Ã¼»ý¼º
			dt = ((DataView)grdExUpperMenuList.DataSource).Table;
			dtChild  = ((DataView)grdExMenuCodeList.DataSource).Table;

			cm = (CurrencyManager) this.BindingContext[grdExUpperMenuList.DataSource]; 
			cm.PositionChanged += new System.EventHandler(OnGrdRowChanged); 

			cmChild = (CurrencyManager) this.BindingContext[grdExMenuCodeList.DataSource]; 
			cmChild.PositionChanged += new System.EventHandler(OnGrdRowDetailChanged); 		

			// ÄÁÆ®·Ñ ÃÊ±âÈ­
			InitControl();			
		}

		#endregion

		#region ÄÁÆ®·Ñ ÃÊ±âÈ­
		private void InitControl()
		{
			ProgressStart();
			SetMenuCodeDetailText();
			
			if(menu.CanRead(MenuCode))
			{
				canRead = true;
				SearchUpperMenu();
			}
			
			if(menu.CanCreate(MenuCode))
			{
				canCreate = true;
			}

			if(menu.CanDelete(MenuCode))
			{
				canDelete = true;
			}		

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

		private void InitButton()
		{
			if(canRead)   btnSearch.Enabled = true;
			if(canCreate) btnAdd.Enabled    = true;
			if(canCreate) btnAdd2.Enabled    = true;
			
			if(ebMenuCode.Text.Trim().Length > 0 || ebMenuName.Text.Trim().Length > 0) 
			{				
				if(canUpdate) btnSave.Enabled   = true;
				if(canDelete) btnDelete2.Enabled = true;
			}
			if(ebMenuCode2.Text.Trim().Length > 0 || ebMenuName2.Text.Trim().Length > 0) 
			{				
				if(canUpdate) btnSave2.Enabled   = true;
				if(canDelete) btnDelete.Enabled = true;
				if(canUpdate) btnOrderUp.Enabled			= true;
				if(canUpdate) btnOrderDown.Enabled		= true;
			}
			if(IsAdding)
			{
				//if(canCreate) cbMediaName.Enabled    = true;
			}
			Application.DoEvents();
		}

		private void DisableButton()
		{
			btnSearch.Enabled = false;
			btnAdd.Enabled    = false;
			btnSave.Enabled   = false;
			btnDelete.Enabled = false;

			btnAdd2.Enabled    = false;
			btnSave2.Enabled   = false;
			btnDelete2.Enabled = false;

			btnOrderUp.Enabled			= false;
			btnOrderDown.Enabled		= false;
			
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
			if(grdExUpperMenuList.RecordCount > 0 && grdExUpperMenuList.RowCount > 0)
			{				
				SetMenuCodeDetailText();
                if (grdExUpperMenuList.SelectedItems.Count > 0)
                {
                    nowCode_old = grdExUpperMenuList.SelectedItems[0].GetRow().Cells["MenuCode"].Value.ToString();
                }
			}
			SetTextReadonly();
			InitButton();         
		}

		
		private void OnGrdRowDetailChanged(object sender, System.EventArgs e) 
		{
            if (grdExMenuCodeList.RecordCount > 0 && grdExMenuCodeList.RowCount > 0 && grdExMenuCodeList.SelectedItems.Count > 0)
			{
				SetChannelSetDetailText();				
				nowCodeDetail = grdExMenuCodeList.SelectedItems[0].GetRow().Cells["MenuCode"].Value.ToString();								
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

			SearchUpperMenu();
			ReSetMenuCodeDetailText();
			ReSetGridData();
			DisableButton();
			SetTextReadonly();			
			SearchUpperMenu();			
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
			btnDelete.Enabled    = false;	
			btnSave2.Enabled   = true;

			//ÇÏÀ§¸Þ´ºÂÊ Ãß°¡¹öÆ°À» ´­·¶À»¶§ ¸Þ´º±×·ì ¹öÆ°À» ¸·´Â´Ù.(ÀúÀåÀ» ÇÏÁö¾ÊÀº »óÅÂ¿¡¼­ Á¶ÀÛºÒ°¡´É)
			btnAdd2.Enabled    = false;		
			btnDelete2.Enabled    = false;		
			btnSave.Enabled   = false;

			//btnDetailAdd.Enabled = true;
			IsAdding = true;

			ResetTextReadonly();
			ReSetMenuCodeDetailText();			

			ebMenuCode2.Focus();
		}

		private void btnAdd_Click2(object sender, System.EventArgs e)
		{
			btnAdd2.Enabled    = false;		
			btnDelete2.Enabled    = false;		
			btnSave.Enabled   = true;
			//¸Þ´º±×·ìÂÊ Ãß°¡¹öÆ°À» ´­·¶À»¶§ ÇÏÀ§¸Þ´º ¹öÆ°À» ¸·´Â´Ù.(ÀúÀåÀ» ÇÏÁö¾ÊÀº »óÅÂ¿¡¼­ Á¶ÀÛºÒ°¡´É)
			btnAdd.Enabled    = false;			
			btnDelete.Enabled    = false;	
			btnSave2.Enabled   = false;

			IsAdding = true;

			ResetTextReadonly_Upper();
			ReSetUpperMenuDetailText();	

			ebMenuCode.Focus();
		}

		private void btnDelete_Click(object sender, System.EventArgs e)
		{
			DeleteMenuCode();
		}

		private void btnDelete_Click2(object sender, System.EventArgs e)
		{
			DeleteUpperMenu();
		}		

		private void btnOrderUp_Click(object sender, System.EventArgs e)
		{
			OrderSetMenuCode(ORDER_UP);
		}

		private void btnOrderDown_Click(object sender, System.EventArgs e)
		{
			OrderSetMenuCode(ORDER_DOWN);
		}
					
		/// <summary>
		/// ÀúÀå¹öÆ° Å¬¸¯
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnSave_Click(object sender, System.EventArgs e)
		{			
			SaveUpperMenuDetail();			
			//SearchUpperMenuDetail();			
		}

		private void btnSave2_Click(object sender, System.EventArgs e)
		{
			SaveMenuCodeDetail();
		}
		#endregion

		#region Ã³¸®¸Þ¼Òµå

		/// <summary>
		/// ¸Þ´º¸ñ·Ï Á¶È¸
		/// </summary>
		private void SearchUpperMenu()
		{
			StatusMessage("¸Þ´º Á¤º¸¸¦ Á¶È¸ÇÕ´Ï´Ù.");		

			try
			{
				systemMenuModel.Init();

				systemMenuModel.SearchKey = "";
				systemMenuModel.SearchMenuCode = "00";
				
				ReSetMenuCodeDetailText();

				// ¸Þ´º¸ñ·ÏÁ¶È¸ ¼­ºñ½º¸¦ È£ÃâÇÑ´Ù.
				new SystemMenuManager(systemModel,commonModel).GetUpperMenuList(systemMenuModel);

				if (systemMenuModel.ResultCD.Equals("0000"))
				{
					Utility.SetDataTable(systemMenuDs.UpperMenu, systemMenuModel.SystemMenuDataSet);
					StatusMessage(systemMenuModel.ResultCnt + "°ÇÀÇ ¸Þ´º Á¤º¸°¡ Á¶È¸µÇ¾ú½À´Ï´Ù.");
					if(canUpdate)
					{
						AddSchChoice();									
					}										
					if(canCreate)
					{
						AddSchChoice_UpperInsert();
					}					
					SearchUpperMenuDetail();
				}
			}
			catch(FrameException fe)
			{
				FrameSystem.showMsgForm("¸Þ´ºÁ¶È¸¿À·ù", new string[] {fe.ErrCode, fe.ResultMsg});
			}
			catch(Exception ex)
			{
				FrameSystem.showMsgForm("¸Þ´ºÁ¶È¸¿À·ù",new string[] {"",ex.Message});
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
				if ( systemMenuDs.Tables["UpperMenu"].Rows.Count < 1 ) return;
              
				foreach (DataRow row in systemMenuDs.Tables["UpperMenu"].Rows)
				{					
					
					if(row["MenuCode"].ToString().Equals(nowCode))
					{					
						cm.Position = rowIndex;
						break;								
					}					

					rowIndex++;
					grdExMenuCodeList.EnsureVisible();
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

		/// <summary>
		/// Å°¯“À»Ã£¾Æ ±×¸®µå Å°¿¡ ÇØ´çµÇ´Â·Î¿ì·Î..
		/// </summary>
		private void AddSchChoice_UpperInsert()
		{
			StatusMessage("Å°¯“");		

			try
			{
				int rowIndex = 0;
				if ( systemMenuDs.Tables["UpperMenu"].Rows.Count < 1 ) return;
              
				foreach (DataRow row in systemMenuDs.Tables["UpperMenu"].Rows)
				{					
					
					if(row["MenuCode"].ToString().Equals(upperCode))
					{					
						cm.Position = rowIndex;
						break;								
					}					

					rowIndex++;
					grdExMenuCodeList.EnsureVisible();
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
     
		/// <summary>
		/// ¸Þ´º¸ñ·Ï Á¶È¸
		/// </summary>
		private void SearchUpperMenuDetail()
		{
			StatusMessage("¸Þ´º µðÅ×ÀÏ Á¤º¸¸¦ Á¶È¸ÇÕ´Ï´Ù.");

			try
			{					
				systemMenuModel.Init();
				systemMenuModel.SearchKey = "";
				int curRow = cm.Position;          

				if(curRow < 0) return;
											
				systemMenuModel.UpperMenu	= dt.Rows[curRow]["UpperMenu"].ToString();				
				systemMenuModel.MenuCode	= dt.Rows[curRow]["MenuCode"].ToString();				
				ebMenuCode.Text             = dt.Rows[curRow]["MenuCode"].ToString();
				ebMenuName.Text				= dt.Rows[curRow]["MenuName"].ToString();				
				nowCode						= dt.Rows[curRow]["MenuCode"].ToString();
				string UseYn				= dt.Rows[curRow]["UseYn"].ToString();

				if(UseYn.Equals("Y"))
				{
					rbUseYn_Y.Checked = true;
					rbUseYn_N.Checked = false;
				}
				else
				{
					rbUseYn_Y.Checked = false;
					rbUseYn_N.Checked = true;
				}
			
				// ¸Þ´º¸ñ·ÏÁ¶È¸ ¼­ºñ½º¸¦ È£ÃâÇÑ´Ù.
				new SystemMenuManager(systemModel,commonModel).GetMenuList(systemMenuModel);

				if (systemMenuModel.ResultCD.Equals("0000"))
				{
					Utility.SetDataTable(systemMenuDs.MenuCode, systemMenuModel.SystemMenuDataSet);				
					StatusMessage(systemMenuModel.ResultCnt + "°ÇÀÇ ¸Þ´º Á¤º¸°¡ Á¶È¸µÇ¾ú½À´Ï´Ù.");

					keyLastOrder	=	systemMenuModel.LastOrder;
					SetChannelSetDetailText();
				}
				//btnDetailAdd.Enabled = true;
            
				if(systemMenuModel.ResultCnt > 0)
				{
					//btnDetailDelete.Enabled = true;   
				}
			}
			catch(FrameException fe)
			{
				FrameSystem.showMsgForm("¸Þ´ºÁ¶È¸¿À·ù", new string[] {fe.ErrCode, fe.ResultMsg});
			}
			catch(Exception ex)
			{
				FrameSystem.showMsgForm("¸Þ´ºÁ¶È¸¿À·ù",new string[] {"",ex.Message});
			}
		}

		private void SearchMenuCodeDetail()
		{
			StatusMessage("¸Þ´º µðÅ×ÀÏ Á¤º¸¸¦ Á¶È¸ÇÕ´Ï´Ù.");

			try
			{               
				// µ¥ÀÌÅÍ¸ðµ¨¿¡ Àü¼ÛÇÒ ³»¿ëÀ» ¼ÂÆ®ÇÑ´Ù.
				int curRow = cm.Position;          

				if(curRow < 0) return;
                systemMenuModel.MenuCode = dt.Rows[curRow]["MenuCode"].ToString();
				systemMenuModel.UpperMenu = dt.Rows[curRow]["UpperMenu"].ToString();				
				//SetChannelSetDetailText();
				
				// ¸Þ´º¸ñ·ÏÁ¶È¸ ¼­ºñ½º¸¦ È£ÃâÇÑ´Ù.
				new SystemMenuManager(systemModel,commonModel).GetMenuList(systemMenuModel);

				if (systemMenuModel.ResultCD.Equals("0000"))
				{
					Utility.SetDataTable(systemMenuDs.MenuCode, systemMenuModel.SystemMenuDataSet);
					StatusMessage(systemMenuModel.ResultCnt + "°ÇÀÇ ¸Þ´º Á¤º¸°¡ Á¶È¸µÇ¾ú½À´Ï´Ù.");						
					if(canUpdate)
					{
						AddSchChoice_Code();									
					}
					if(canCreate)
					{
						AddSchChoice_Insert();
					}								
				}			
			}
			catch(FrameException fe)
			{
				FrameSystem.showMsgForm("¸Þ´ºÁ¶È¸¿À·ù", new string[] {fe.ErrCode, fe.ResultMsg});
			}
			catch(Exception ex)
			{
				FrameSystem.showMsgForm("¸Þ´ºÁ¶È¸¿À·ù",new string[] {"",ex.Message});
			}
		}

		/// <summary>
		/// Å°¯“À»Ã£¾Æ ±×¸®µå Å°¿¡ ÇØ´çµÇ´Â·Î¿ì·Î..
		/// </summary>
		private void AddSchChoice_Code()
		{
			StatusMessage("Å°¯“");		

			try
			{
				int rowIndex = 0;
				if ( systemMenuDs.Tables["MenuCode"].Rows.Count < 1 ) return;
              
				foreach (DataRow row in systemMenuDs.Tables["MenuCode"].Rows)
				{					
					if(row["MenuOrder"].ToString().Equals(keyMenuOrder))
					{					
						cmChild.Position = rowIndex;
						break;								
					}					

					rowIndex++;
					grdExMenuCodeList.EnsureVisible();					
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

		/// <summary>
		/// Å°¯“À»Ã£¾Æ ±×¸®µå Å°¿¡ ÇØ´çµÇ´Â·Î¿ì·Î..
		/// </summary>
		private void AddSchChoice_Insert()
		{
			StatusMessage("Å°¯“");		

			try
			{
				int rowIndex = 0;
				if ( systemMenuDs.Tables["MenuCode"].Rows.Count < 1 ) return;
              
				foreach (DataRow row in systemMenuDs.Tables["MenuCode"].Rows)
				{					
					if(row["MenuCode"].ToString().Equals(nowCode))
					{					
						cmChild.Position = rowIndex;
						break;								
					}					

					rowIndex++;
					grdExMenuCodeList.EnsureVisible();					
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

		/// <summary>
		/// ¸Þ´º ÀúÀå
		/// </summary>
		private void SaveUpperMenuDetail()
		{
			//IsAdding = true;

			StatusMessage("¸Þ´º±¸ºÐ Á¤º¸¸¦ ÀúÀåÇÕ´Ï´Ù.");                        
                      
			if(ebMenuCode.Text.ToString().Length == 0) 
			{
				MessageBox.Show("¸Þ´º°¡ ÀÔ·ÂµÇÁö ¾Ê¾Ò½À´Ï´Ù.","¸Þ´º ÀúÀå", 
					MessageBoxButtons.OK, MessageBoxIcon.Information );
				return;								
			}
			if(ebMenuName.Text.ToString().Length == 0) 
			{
				MessageBox.Show("¸Þ´º¸íÀÌ ÀÔ·ÂµÇÁö ¾Ê¾Ò½À´Ï´Ù.","¸Þ´º ÀúÀå", 
					MessageBoxButtons.OK, MessageBoxIcon.Information );
				return;					
			}						
                        
			try
			{
				//ÀúÀå Àü¿¡ ¸ðµ¨À» ÃÊ±âÈ­ ÇØÁØ´Ù.
				systemMenuModel.Init();
				// µ¥ÀÌÅÍ¸ðµ¨¿¡ Àü¼ÛÇÒ ³»¿ëÀ» ¼ÂÆ®ÇÑ´Ù.
				systemMenuModel.MenuCode      = ebMenuCode.Text;
				systemMenuModel.MenuCode_2      = ebMenuCode.Text;
				systemMenuModel.MenuName      = ebMenuName.Text;
				systemMenuModel.MenuCode_3      = ebMenuCode.Text;
				upperCode = systemMenuModel.MenuCode;
			
				//»ç¿ë¿©ºÎ
				if(rbUseYn_Y.Checked)
				{
					systemMenuModel.UseYn       = "Y";
				}
				else
				{
					systemMenuModel.UseYn       = "N";
				}                      

				// ¸Þ´º »ó¼¼Á¤º¸ ÀúÀå ¼­ºñ½º¸¦ È£ÃâÇÑ´Ù.
				if (IsAdding)
				{
					new SystemMenuManager(systemModel,commonModel).SetUpperMenuCreate(systemMenuModel);
					StatusMessage("¸Þ´º Á¤º¸°¡ Ãß°¡µÇ¾ú½À´Ï´Ù.");
					IsAdding = false;
					ReSetMenuCodeDetailText();
				}
				else
				{
					new SystemMenuManager(systemModel,commonModel).SetUpperMenuUpdate(systemMenuModel);
					StatusMessage("¸Þ´º±¸ºÐ Á¤º¸°¡ ¼öÁ¤µÇ¾ú½À´Ï´Ù.");
				}
				
				DisableButton();
				SearchUpperMenu();
				InitButton();
                        
			}
			catch(FrameException fe)
			{
				FrameSystem.showMsgForm("¸Þ´º±¸ºÐ ÀúÀå¿À·ù", new string[] {fe.ErrCode, fe.ResultMsg});
			}
			catch(Exception ex)
			{
				FrameSystem.showMsgForm("¸Þ´º±¸ºÐ ÀúÀå¿À·ù",new string[] {"",ex.Message});
			}			
		}


		/// <summary>
		/// ¸Þ´º ÀúÀå
		/// </summary>
		private void SaveMenuCodeDetail()
		{
			//IsAdding = true;

			StatusMessage("¸Þ´º Á¤º¸¸¦ ÀúÀåÇÕ´Ï´Ù.");                        
                      
			if(ebMenuCode2.Text.ToString().Length == 0) 
			{
				MessageBox.Show("¸Þ´º°¡ ÀÔ·ÂµÇÁö ¾Ê¾Ò½À´Ï´Ù.","¸Þ´º ÀúÀå", 
					MessageBoxButtons.OK, MessageBoxIcon.Information );
				return;								
			}
			if(ebMenuName2.Text.ToString().Length == 0) 
			{
				MessageBox.Show("¸Þ´º¸íÀÌ ÀÔ·ÂµÇÁö ¾Ê¾Ò½À´Ï´Ù.","¸Þ´º ÀúÀå", 
					MessageBoxButtons.OK, MessageBoxIcon.Information );
				return;					
			}						
                        
			try
			{
				//ÀúÀå Àü¿¡ ¸ðµ¨À» ÃÊ±âÈ­ ÇØÁØ´Ù.
				systemMenuModel.Init();
				// µ¥ÀÌÅÍ¸ðµ¨¿¡ Àü¼ÛÇÒ ³»¿ëÀ» ¼ÂÆ®ÇÑ´Ù.															
				systemMenuModel.MenuCode      = ebMenuCode2.Text;
				systemMenuModel.MenuCode_2      = ebMenuCode.Text;
				systemMenuModel.MenuName      = ebMenuName2.Text;
				nowCode = systemMenuModel.MenuCode;
				
				//»ç¿ë¿©ºÎ
				if(rbUseYn_Y2.Checked)
				{
					systemMenuModel.UseYn       = "Y";
				}
				else
				{
					systemMenuModel.UseYn       = "N";
				}

				int curRow = cm.Position;
						
				if(curRow >= 0)
				{
					systemMenuModel.UpperMenu      = dt.Rows[curRow]["UpperMenu"].ToString();	
				}							
													
				//systemMenuModel.SystemMenuDataSet = systemMenuDs.Copy();                         

				// ¸Þ´º »ó¼¼Á¤º¸ ÀúÀå ¼­ºñ½º¸¦ È£ÃâÇÑ´Ù.
				if (IsAdding)
				{
					new SystemMenuManager(systemModel,commonModel).SetMenuCodeCreate(systemMenuModel);
					StatusMessage("¸Þ´º Á¤º¸°¡ Ãß°¡µÇ¾ú½À´Ï´Ù.");
					IsAdding = false;
					ReSetMenuCodeDetailText();
				}
				else
				{
					new SystemMenuManager(systemModel,commonModel).SetMenuCodeUpdate(systemMenuModel);
					StatusMessage("¸Þ´º Á¤º¸°¡ ¼öÁ¤µÇ¾ú½À´Ï´Ù.");
				}
				
				DisableButton();
				SearchMenuCodeDetail();
				InitButton();
                        
			}
			catch(FrameException fe)
			{
				FrameSystem.showMsgForm("¸Þ´º ÀúÀå¿À·ù", new string[] {fe.ErrCode, fe.ResultMsg});
			}
			catch(Exception ex)
			{
				FrameSystem.showMsgForm("¸Þ´º ÀúÀå¿À·ù",new string[] {"",ex.Message});
			}			
		}
//
		/// <summary>
		/// ¸Þ´ºÁ¤º¸ »èÁ¦
		/// </summary>
		private void DeleteUpperMenu()
		{
			StatusMessage("¸Þ´º Á¤º¸¸¦ »èÁ¦ÇÕ´Ï´Ù.");

			if(ebMenuCode.Text.Trim().Length == 0) 
			{
				MessageBox.Show("»èÁ¦ÇÒ ¸Þ´º Á¤º¸°¡ ¾ø½À´Ï´Ù.","¸Þ´º »èÁ¦", 
					MessageBoxButtons.OK, MessageBoxIcon.Information );
				return;
			}

			DialogResult result = MessageBox.Show("ÇØ´ç ¸Þ´º Á¤º¸¸¦ »èÁ¦ ÇÏ½Ã°Ú½À´Ï±î? \n ÇÏÀ§¸Þ´ºµµ »èÁ¦°¡µË´Ï´Ù.","¸Þ´º »èÁ¦",
				MessageBoxButtons.YesNo, MessageBoxIcon.Question,
				MessageBoxDefaultButton.Button2);

			if (result == DialogResult.No) return;

			try 
			{
				// µ¥ÀÌÅÍ¸ðµ¨¿¡ Àü¼ÛÇÒ ³»¿ëÀ» ¼ÂÆ®ÇÑ´Ù.
				int curRow = cm.Position;
						
				if(curRow >= 0)
				{
					systemMenuModel.MenuCode      = dt.Rows[curRow]["MenuCode"].ToString();						
					systemMenuModel.UpperMenu      = dt.Rows[curRow]["UpperMenu"].ToString();						
				}

				// ¸Þ´º »ó¼¼Á¤º¸ ÀúÀå ¼­ºñ½º¸¦ È£ÃâÇÑ´Ù.
				new SystemMenuManager(systemModel,commonModel).SetUpperMenuDelete(systemMenuModel);

				StatusMessage("¸Þ´º Á¤º¸°¡ »èÁ¦µÇ¾ú½À´Ï´Ù.");	
				
				DisableButton();
				SearchUpperMenu();
				InitButton();

			}
			catch(FrameException fe)
			{
				FrameSystem.showMsgForm("¸Þ´º »èÁ¦¿À·ù", new string[] {fe.ErrCode, fe.ResultMsg});
			}
			catch(Exception ex)
			{
				FrameSystem.showMsgForm("¸Þ´º »èÁ¦¿À·ù",new string[] {"",ex.Message});
			}			
		}


		/// <summary>
		/// ¸Þ´ºÁ¤º¸ »èÁ¦
		/// </summary>
		private void DeleteMenuCode()
		{
			StatusMessage("¸Þ´º Á¤º¸¸¦ »èÁ¦ÇÕ´Ï´Ù.");

			if(ebMenuCode2.Text.Trim().Length == 0) 
			{
				MessageBox.Show("»èÁ¦ÇÒ ¸Þ´º Á¤º¸°¡ ¾ø½À´Ï´Ù.","¸Þ´º »èÁ¦", 
					MessageBoxButtons.OK, MessageBoxIcon.Information );
				return;
			}

			DialogResult result = MessageBox.Show("ÇØ´ç ¸Þ´º Á¤º¸¸¦ »èÁ¦ ÇÏ½Ã°Ú½À´Ï±î?","¸Þ´º »èÁ¦",
				MessageBoxButtons.YesNo, MessageBoxIcon.Question,
				MessageBoxDefaultButton.Button2);

			if (result == DialogResult.No) return;

			try 
			{
				// µ¥ÀÌÅÍ¸ðµ¨¿¡ Àü¼ÛÇÒ ³»¿ëÀ» ¼ÂÆ®ÇÑ´Ù.
				int curRow = cmChild.Position;
						
				if(curRow >= 0)
				{
					systemMenuModel.MenuCode      = dtChild.Rows[curRow]["MenuCode"].ToString();						
				}

				// ¸Þ´º »ó¼¼Á¤º¸ ÀúÀå ¼­ºñ½º¸¦ È£ÃâÇÑ´Ù.
				new SystemMenuManager(systemModel,commonModel).SetMenuCodeDelete(systemMenuModel);

				StatusMessage("¸Þ´º Á¤º¸°¡ »èÁ¦µÇ¾ú½À´Ï´Ù.");	
				
				DisableButton();
				SearchMenuCodeDetail();
				InitButton();

			}
			catch(FrameException fe)
			{
				FrameSystem.showMsgForm("¸Þ´º »èÁ¦¿À·ù", new string[] {fe.ErrCode, fe.ResultMsg});
			}
			catch(Exception ex)
			{
				FrameSystem.showMsgForm("¸Þ´º »èÁ¦¿À·ù",new string[] {"",ex.Message});
			}			
		}

		/// <summary>
		/// ¸Þ´º ¼øÀ§º¯°æ
		/// </summary>
		private void OrderSetMenuCode(int OrderSet)
		{
			StatusMessage("¸Þ´ºÀÇ Æí¼º¼øÀ§¸¦ º¯°æÇÕ´Ï´Ù.");

			if(ebMenuCode2.Text.Trim().Length == 0) 
			{
				MessageBox.Show("º¯°æÇÒ ¸Þ´º°¡ ¼±ÅÃµÇÁö ¾Ê¾Ò½À´Ï´Ù.","¸Þ´º ¼øÀ§º¯°æ", 
					MessageBoxButtons.OK, MessageBoxIcon.Information );
				return;
			}

			try
			{
				// µ¥ÀÌÅÍ¸ðµ¨¿¡ Àü¼ÛÇÒ ³»¿ëÀ» ¼ÂÆ®ÇÑ´Ù.
				int curRow = cmChild.Position;
		
				if(curRow >= 0)
				{
					systemMenuModel.MenuCode      = dtChild.Rows[curRow]["MenuCode"].ToString();						
					systemMenuModel.UpperMenu      = dtChild.Rows[curRow]["UpperMenu"].ToString();						
					systemMenuModel.MenuOrder      = keyMenuOrder;						
				}	
									
				int NowOrder  = Convert.ToInt32(keyMenuOrder);
				int LastOrder = Convert.ToInt32(keyLastOrder);

				switch(OrderSet)
				{					
					case ORDER_UP:
						if(NowOrder <= 1) 
						{
							MessageBox.Show("ÇØ´ç ¸Þ´º°¡ Ã¹¹øÂ° ¼øÀ§ÀÔ´Ï´Ù.","¸Þ´º ¼øÀ§º¯°æ", 
								MessageBoxButtons.OK, MessageBoxIcon.Information );							
							return;
						}
						break;
					case ORDER_DOWN:
						if(NowOrder >= LastOrder) 
						{
							MessageBox.Show("ÇØ´ç ¸Þ´º°¡ ¸¶Áö¸· ¼øÀ§ÀÔ´Ï´Ù.","¸Þ´º ¼øÀ§º¯°æ", 
								MessageBoxButtons.OK, MessageBoxIcon.Information );							
							return;
						}
						break;					
				}

				// ¸Þ´º ¼ø¼­º¯°æ ¼­ºñ½º¸¦ È£ÃâÇÑ´Ù.
				new SystemMenuManager(systemModel,commonModel).SetMenuCodeOrderSet(systemMenuModel, OrderSet);
				keyMenuOrder	=	systemMenuModel.MenuOrder;				
				StatusMessage("¸Þ´ºÀÇ ¼øÀ§°¡ º¯°æµÇ¾ú½À´Ï´Ù.");			
				
				DisableButton();
				SearchMenuCodeDetail();
				//ResetDetailText();
				InitButton();
						
		
			}
			catch(FrameException fe)
			{
				FrameSystem.showMsgForm("¸Þ´º ¼øÀ§º¯°æ ¿À·ù", new string[] {fe.ErrCode, fe.ResultMsg});
			}
			catch(Exception ex)
			{
				FrameSystem.showMsgForm("¸Þ´º ¼øÀ§º¯°æ ¿À·ù",new string[] {"",ex.Message});
			}		
		}

		private void ResetDetailText()
		{
			keyMenuOrder     = "";
//			keyLastOrder        = "";			
		}
		
		/// <summary>
		/// ¸Þ´º±¸ºÐ »ó¼¼Á¤º¸ÀÇ ¼ÂÆ®
		/// </summary>
		private void SetMenuCodeDetailText()
		{
			int curRow = cm.Position;
            
			ebMenuCode.ReadOnly = true;
			ebMenuCode2.ReadOnly = true;		
			ebMenuName.ReadOnly         = false;			
			ebMenuName2.ReadOnly         = false;			
						
			ebUpperMenu.ReadOnly = true;
			ebMenuOrder.ReadOnly = true;

			ebMenuCode.BackColor = Color.WhiteSmoke;
			ebMenuCode2.BackColor = Color.WhiteSmoke;				
			ebUpperMenu.BackColor = Color.WhiteSmoke;
			ebMenuOrder.BackColor = Color.WhiteSmoke;	
			ebMenuName.BackColor        = Color.White;			
			ebMenuName2.BackColor        = Color.White;			

			SearchUpperMenuDetail();
            
			IsAdding = false;
            
			StatusMessage("ÁØºñ");
		}

		private void SetChannelSetDetailText()
		{		
			int curRow = cmChild.Position;
					
			if(curRow >= 0)
			{
				ebMenuCode2.Text = dtChild.Rows[curRow]["MenuCode"].ToString();
				ebMenuName2.Text = dtChild.Rows[curRow]["MenuName"].ToString();				
				ebUpperMenu.Text = dtChild.Rows[curRow]["UpperName"].ToString();
				ebMenuOrder.Text = dtChild.Rows[curRow]["MenuOrder"].ToString();
				keyMenuOrder = dtChild.Rows[curRow]["MenuOrder"].ToString();
				string UseYn              = dtChild.Rows[curRow]["UseYn"].ToString();

				if(UseYn.Equals("Y"))
				{
					rbUseYn_Y2.Checked = true;
					rbUseYn_N2.Checked = false;
				}
				else
				{
					rbUseYn_Y2.Checked = false;
					rbUseYn_N2.Checked = true;
				}

				IsAdding = false;			
			}					
				
			StatusMessage("ÁØºñ");
		}

		private void ReSetMenuCodeDetailText()
		{			
			ebMenuCode2.Text                 = "";			
			ebMenuName2.Text                 = "";
			ebMenuOrder.Text                 = "";
			ebUpperMenu.Text                 = "";			
		}

		private void ReSetUpperMenuDetailText()
		{			
			ebMenuCode.Text                 = "";			
			ebMenuName.Text                 = "";			
		}
        
		private void ReSetGridData()
		{			
			systemMenuDs.MenuCode.Clear();        
		}
	
		/// <summary>
		/// »ó¼¼Á¤º¸ ReadOnly
		/// </summary>
		private void SetTextReadonly()
		{
			ebMenuCode.ReadOnly = true;
			ebMenuCode2.ReadOnly = true;

			ebMenuCode.BackColor = Color.WhiteSmoke;
			ebMenuCode2.BackColor = Color.WhiteSmoke;
		}

		/// <summary>
		/// »ó¼¼Á¤º¸ ¼öÁ¤°¡´ÉÄÉ
		/// </summary>
		private void ResetTextReadonly()
		{				
			ebMenuCode2.ReadOnly = false;
			ebMenuName2.ReadOnly = false; 
			
			ebMenuCode2.BackColor = Color.White;
			ebMenuName2.BackColor     = Color.White;						
		}

		private void ResetTextReadonly_Upper()
		{				
			ebMenuCode.ReadOnly = false;
			ebMenuName.ReadOnly = false; 
			
			ebMenuCode.BackColor = Color.White;
			ebMenuName.BackColor     = Color.White;						
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
	}
}