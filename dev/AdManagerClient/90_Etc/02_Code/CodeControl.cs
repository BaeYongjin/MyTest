// ===============================================================================
// CodeControl for Charites Project
//
// CodeControl.cs
//
// ÄÚµåÁ¤º¸°ü¸® ÄÁµå·ÑÀ» Á¤ÀÇÇÕ´Ï´Ù. 
//
// ===============================================================================
// Release history
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
	/// ÄÚµå°ü¸® ÄÁÆ®·Ñ
	/// </summary>
	public class CodeControl : System.Windows.Forms.UserControl, IUserControl
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
		CodeModel codeModel  = new CodeModel();	// ÄÚµåÁ¤º¸¸ðµ¨

		// È­¸éÃ³¸®¿ë º¯¼ö
		bool IsNewSearchKey		  = true;					// °Ë»ö¾îÀÔ·Â ¿©ºÎ
		CurrencyManager cm        = null;					// µ¥ÀÌÅÍ ±×¸®µåÀÇ º¯°æ¿¡ µû¸¥ µ¥ÀÌÅÍ¼Â °ü¸®¸¦ À§ÇÏ¿©			
		CurrencyManager cmChild        = null;					// µ¥ÀÌÅÍ ±×¸®µåÀÇ º¯°æ¿¡ µû¸¥ µ¥ÀÌÅÍ¼Â °ü¸®¸¦ À§ÇÏ¿©			
		DataTable       dt        = null;
		DataTable       dtChild        = null;		
		
		private string        section = null;
		private string        code = null;

		private string        section_old = null;
		private string        code_old = null;

        bool IsSearching = false; // Á¶È¸Áß »ó¼¼È­¸éÀÌ ¾÷µ¥ÀÌÆ® µÇ´Â °ÍÀ» ¹æÁö ÇÏ±âÀ§ÇÔ 2011.11.29 JH.Park
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
		private Janus.Windows.GridEX.EditControls.EditBox ebSearchKey;
		private Janus.Windows.EditControls.UIButton btnSearch;
		private Janus.Windows.EditControls.UIButton btnAdd;
		private Janus.Windows.EditControls.UIButton btnSave;
		private System.Windows.Forms.Label lbChannelNo;
		private System.Windows.Forms.Label lbTitle;
		private Janus.Windows.UI.Dock.UIPanelGroup uiPanelChannelSet;
		private Janus.Windows.UI.Dock.UIPanel uiPanel1;
		private Janus.Windows.UI.Dock.UIPanelInnerContainer uiPanel1Container;
		private Janus.Windows.UI.Dock.UIPanel uiPanel2;
		private Janus.Windows.UI.Dock.UIPanelInnerContainer uiPanel2Container;
		private System.Data.DataView dvSection;
		private System.Data.DataView dvCode;
		private AdManagerClient._90_Etc._02_Code.CodeDs codeDs;
		private Janus.Windows.EditControls.UIComboBox cbSearchSection;
		private Janus.Windows.GridEX.GridEX grdExSectionList;
		private Janus.Windows.GridEX.GridEX grdExCodeList;
		private Janus.Windows.UI.Dock.UIPanelGroup uiPanel0;
		private Janus.Windows.UI.Dock.UIPanel uiPanel3;
		private Janus.Windows.UI.Dock.UIPanelInnerContainer uiPanel3Container;
		private Janus.Windows.UI.Dock.UIPanel uiPanel4;
		private Janus.Windows.UI.Dock.UIPanelInnerContainer uiPanel4Container;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Panel panel2;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private Janus.Windows.GridEX.EditControls.EditBox ebSection;
		private Janus.Windows.GridEX.EditControls.EditBox ebCodeName;
		private Janus.Windows.GridEX.EditControls.EditBox ebCode;
		private Janus.Windows.GridEX.EditControls.EditBox ebCodeName2;
		private Janus.Windows.EditControls.UIButton btnSave2;
		private Janus.Windows.EditControls.UIButton btnDelete;		
		private System.ComponentModel.IContainer components;

		public CodeControl()
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
			Janus.Windows.GridEX.GridEXLayout grdExSectionList_DesignTimeLayout = new Janus.Windows.GridEX.GridEXLayout();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CodeControl));
			Janus.Windows.GridEX.GridEXLayout grdExSectionList_Layout_0 = new Janus.Windows.GridEX.GridEXLayout();
			Janus.Windows.GridEX.GridEXLayout grdExCodeList_DesignTimeLayout = new Janus.Windows.GridEX.GridEXLayout();
			this.uiPM = new Janus.Windows.UI.Dock.UIPanelManager(this.components);
			this.uiPanelUsers = new Janus.Windows.UI.Dock.UIPanelGroup();
			this.uiPanelUsersSearch = new Janus.Windows.UI.Dock.UIPanel();
			this.uiPanelUsersSearchContainer = new Janus.Windows.UI.Dock.UIPanelInnerContainer();
			this.pnlSearch = new System.Windows.Forms.Panel();
			this.cbSearchSection = new Janus.Windows.EditControls.UIComboBox();
			this.ebSearchKey = new Janus.Windows.GridEX.EditControls.EditBox();
			this.btnSearch = new Janus.Windows.EditControls.UIButton();
			this.uiPanelChannelSet = new Janus.Windows.UI.Dock.UIPanelGroup();
			this.uiPanel1 = new Janus.Windows.UI.Dock.UIPanel();
			this.uiPanel1Container = new Janus.Windows.UI.Dock.UIPanelInnerContainer();
			this.grdExSectionList = new Janus.Windows.GridEX.GridEX();
			this.dvSection = new System.Data.DataView();
			this.codeDs = new AdManagerClient._90_Etc._02_Code.CodeDs();
			this.uiPanel2 = new Janus.Windows.UI.Dock.UIPanel();
			this.uiPanel2Container = new Janus.Windows.UI.Dock.UIPanelInnerContainer();
			this.grdExCodeList = new Janus.Windows.GridEX.GridEX();
			this.dvCode = new System.Data.DataView();
			this.uiPanel0 = new Janus.Windows.UI.Dock.UIPanelGroup();
			this.uiPanel3 = new Janus.Windows.UI.Dock.UIPanel();
			this.uiPanel3Container = new Janus.Windows.UI.Dock.UIPanelInnerContainer();
			this.panel1 = new System.Windows.Forms.Panel();
			this.btnSave = new Janus.Windows.EditControls.UIButton();
			this.lbTitle = new System.Windows.Forms.Label();
			this.ebSection = new Janus.Windows.GridEX.EditControls.EditBox();
			this.lbChannelNo = new System.Windows.Forms.Label();
			this.ebCodeName = new Janus.Windows.GridEX.EditControls.EditBox();
			this.uiPanel4 = new Janus.Windows.UI.Dock.UIPanel();
			this.uiPanel4Container = new Janus.Windows.UI.Dock.UIPanelInnerContainer();
			this.panel2 = new System.Windows.Forms.Panel();
			this.btnAdd = new Janus.Windows.EditControls.UIButton();
			this.btnSave2 = new Janus.Windows.EditControls.UIButton();
			this.btnDelete = new Janus.Windows.EditControls.UIButton();
			this.label1 = new System.Windows.Forms.Label();
			this.ebCode = new Janus.Windows.GridEX.EditControls.EditBox();
			this.label2 = new System.Windows.Forms.Label();
			this.ebCodeName2 = new Janus.Windows.GridEX.EditControls.EditBox();
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
			((System.ComponentModel.ISupportInitialize)(this.grdExSectionList)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.dvSection)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.codeDs)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.uiPanel2)).BeginInit();
			this.uiPanel2.SuspendLayout();
			this.uiPanel2Container.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.grdExCodeList)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.dvCode)).BeginInit();
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
			this.uiPM.AddDockPanelInfo(new System.Guid("88f86479-98e4-43d4-bf03-12fd789fcfc5"), new System.Guid("b162c53a-7940-47cd-a0ed-9762c59c50d8"), Janus.Windows.UI.Dock.PanelGroupStyle.VerticalTiles, true, 461, true);
			this.uiPM.AddDockPanelInfo(new System.Guid("91eeb7fc-856c-4a9c-b37e-db742cfa9b2e"), new System.Guid("88f86479-98e4-43d4-bf03-12fd789fcfc5"), 425, true);
			this.uiPM.AddDockPanelInfo(new System.Guid("3d64f731-de32-42ba-a10f-ac3b5ac56703"), new System.Guid("88f86479-98e4-43d4-bf03-12fd789fcfc5"), 422, true);
			this.uiPM.AddDockPanelInfo(new System.Guid("b7763a2e-3139-4238-9443-b9c640f9b161"), new System.Guid("b162c53a-7940-47cd-a0ed-9762c59c50d8"), Janus.Windows.UI.Dock.PanelGroupStyle.VerticalTiles, true, 151, true);
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
			this.uiPanelUsers.Text = "ÄÚµå°ü¸®";
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
			this.pnlSearch.Controls.Add(this.cbSearchSection);
			this.pnlSearch.Controls.Add(this.ebSearchKey);
			this.pnlSearch.Controls.Add(this.btnSearch);
			this.pnlSearch.Dock = System.Windows.Forms.DockStyle.Fill;
			this.pnlSearch.Location = new System.Drawing.Point(0, 0);
			this.pnlSearch.Name = "pnlSearch";
			this.pnlSearch.Size = new System.Drawing.Size(1008, 38);
			this.pnlSearch.TabIndex = 0;
			// 
			// cbSearchSection
			// 
			this.cbSearchSection.ComboStyle = Janus.Windows.EditControls.ComboStyle.DropDownList;
			this.cbSearchSection.Location = new System.Drawing.Point(8, 11);
			this.cbSearchSection.Name = "cbSearchSection";
			this.cbSearchSection.Size = new System.Drawing.Size(152, 21);
			this.cbSearchSection.TabIndex = 1;
			this.cbSearchSection.Text = "ÄÚµå±¸ºÐ¼±ÅÃ";
			this.cbSearchSection.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
			// 
			// ebSearchKey
			// 
			this.ebSearchKey.Location = new System.Drawing.Point(168, 11);
			this.ebSearchKey.Name = "ebSearchKey";
			this.ebSearchKey.Size = new System.Drawing.Size(176, 21);
			this.ebSearchKey.TabIndex = 2;
			this.ebSearchKey.Text = "ÄÚµå¸íÀ» ÀÔ·ÂÇÏ¼¼¿ä";
			this.ebSearchKey.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
			this.ebSearchKey.Visible = false;
			this.ebSearchKey.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
			this.ebSearchKey.TextChanged += new System.EventHandler(this.ebSearchKey_TextChanged);
			this.ebSearchKey.Click += new System.EventHandler(this.ebSearchKey_Click);
			this.ebSearchKey.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ebSearchKey_KeyDown);
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
			this.uiPanelChannelSet.Size = new System.Drawing.Size(1010, 452);
			this.uiPanelChannelSet.TabIndex = 0;
			this.uiPanelChannelSet.Text = "ÄÚµå°ü¸®";
			// 
			// uiPanel1
			// 
			this.uiPanel1.InnerContainer = this.uiPanel1Container;
			this.uiPanel1.Location = new System.Drawing.Point(0, 22);
			this.uiPanel1.Name = "uiPanel1";
			this.uiPanel1.Size = new System.Drawing.Size(505, 430);
			this.uiPanel1.TabIndex = 0;
			this.uiPanel1.Text = "ÄÚµå±¸ºÐ";
			// 
			// uiPanel1Container
			// 
			this.uiPanel1Container.Controls.Add(this.grdExSectionList);
			this.uiPanel1Container.Location = new System.Drawing.Point(1, 23);
			this.uiPanel1Container.Name = "uiPanel1Container";
			this.uiPanel1Container.Size = new System.Drawing.Size(503, 406);
			this.uiPanel1Container.TabIndex = 0;
			// 
			// grdExSectionList
			// 
			this.grdExSectionList.AllowEdit = Janus.Windows.GridEX.InheritableBoolean.False;
			this.grdExSectionList.AlternatingColors = true;
			this.grdExSectionList.AlternatingRowFormatStyle.BackgroundImageDrawMode = Janus.Windows.GridEX.BackgroundImageDrawMode.Tile;
			this.grdExSectionList.AutomaticSort = false;
			this.grdExSectionList.BorderStyle = Janus.Windows.GridEX.BorderStyle.None;
			this.grdExSectionList.DataSource = this.dvSection;
			grdExSectionList_DesignTimeLayout.LayoutString = resources.GetString("grdExSectionList_DesignTimeLayout.LayoutString");
			this.grdExSectionList.DesignTimeLayout = grdExSectionList_DesignTimeLayout;
			this.grdExSectionList.Dock = System.Windows.Forms.DockStyle.Fill;
			this.grdExSectionList.EditorsControlStyle.ButtonAppearance = Janus.Windows.GridEX.ButtonAppearance.Regular;
			this.grdExSectionList.EmptyRows = true;
			this.grdExSectionList.FocusCellFormatStyle.BackColor = System.Drawing.SystemColors.Highlight;
			this.grdExSectionList.FocusCellFormatStyle.ForeColor = System.Drawing.Color.White;
			this.grdExSectionList.FocusStyle = Janus.Windows.GridEX.FocusStyle.None;
			this.grdExSectionList.FrozenColumns = 2;
			this.grdExSectionList.GridLineColor = System.Drawing.Color.Silver;
			this.grdExSectionList.GridLines = Janus.Windows.GridEX.GridLines.Vertical;
			this.grdExSectionList.GridLineStyle = Janus.Windows.GridEX.GridLineStyle.Solid;
			this.grdExSectionList.GroupByBoxVisible = false;
			this.grdExSectionList.GroupMode = Janus.Windows.GridEX.GroupMode.Collapsed;
			this.grdExSectionList.GroupRowVisualStyle = Janus.Windows.GridEX.GroupRowVisualStyle.Outlook2003;
			this.grdExSectionList.HeaderFormatStyle.TextAlignment = Janus.Windows.GridEX.TextAlignment.Center;
			this.grdExSectionList.HideSelection = Janus.Windows.GridEX.HideSelection.HighlightInactive;
			grdExSectionList_Layout_0.Key = "bea";
			this.grdExSectionList.Layouts.AddRange(new Janus.Windows.GridEX.GridEXLayout[] {
            grdExSectionList_Layout_0});
			this.grdExSectionList.Location = new System.Drawing.Point(0, 0);
			this.grdExSectionList.Name = "grdExSectionList";
			this.grdExSectionList.ScrollBars = Janus.Windows.GridEX.ScrollBars.Vertical;
			this.grdExSectionList.SelectedInactiveFormatStyle.BackColor = System.Drawing.Color.Gold;
			this.grdExSectionList.Size = new System.Drawing.Size(503, 406);
			this.grdExSectionList.TabIndex = 5;
			this.grdExSectionList.TabKeyBehavior = Janus.Windows.GridEX.TabKeyBehavior.ControlNavigation;
			this.grdExSectionList.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
			this.grdExSectionList.Enter += new System.EventHandler(this.OnGrdRowChanged);
			// 
			// dvSection
			// 
			this.dvSection.Table = this.codeDs.Section;
			// 
			// codeDs
			// 
			this.codeDs.DataSetName = "CodeDs";
			this.codeDs.Locale = new System.Globalization.CultureInfo("en-US");
			this.codeDs.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
			// 
			// uiPanel2
			// 
			this.uiPanel2.InnerContainer = this.uiPanel2Container;
			this.uiPanel2.Location = new System.Drawing.Point(509, 22);
			this.uiPanel2.Name = "uiPanel2";
			this.uiPanel2.Size = new System.Drawing.Size(501, 430);
			this.uiPanel2.TabIndex = 0;
			this.uiPanel2.Text = "ÄÚµå";
			// 
			// uiPanel2Container
			// 
			this.uiPanel2Container.Controls.Add(this.grdExCodeList);
			this.uiPanel2Container.Location = new System.Drawing.Point(1, 23);
			this.uiPanel2Container.Name = "uiPanel2Container";
			this.uiPanel2Container.Size = new System.Drawing.Size(499, 406);
			this.uiPanel2Container.TabIndex = 0;
			// 
			// grdExCodeList
			// 
			this.grdExCodeList.AllowEdit = Janus.Windows.GridEX.InheritableBoolean.False;
			this.grdExCodeList.AlternatingColors = true;
			this.grdExCodeList.AutomaticSort = false;
			this.grdExCodeList.BorderStyle = Janus.Windows.GridEX.BorderStyle.None;
			this.grdExCodeList.DataSource = this.dvCode;
			grdExCodeList_DesignTimeLayout.LayoutString = resources.GetString("grdExCodeList_DesignTimeLayout.LayoutString");
			this.grdExCodeList.DesignTimeLayout = grdExCodeList_DesignTimeLayout;
			this.grdExCodeList.Dock = System.Windows.Forms.DockStyle.Fill;
			this.grdExCodeList.EditorsControlStyle.ButtonAppearance = Janus.Windows.GridEX.ButtonAppearance.Regular;
			this.grdExCodeList.EmptyRows = true;
			this.grdExCodeList.FocusCellFormatStyle.BackColor = System.Drawing.SystemColors.Highlight;
			this.grdExCodeList.FocusCellFormatStyle.ForeColor = System.Drawing.Color.White;
			this.grdExCodeList.FocusStyle = Janus.Windows.GridEX.FocusStyle.None;
			this.grdExCodeList.FrozenColumns = 2;
			this.grdExCodeList.GridLineColor = System.Drawing.Color.Silver;
			this.grdExCodeList.GridLines = Janus.Windows.GridEX.GridLines.Vertical;
			this.grdExCodeList.GridLineStyle = Janus.Windows.GridEX.GridLineStyle.Solid;
			this.grdExCodeList.GroupByBoxVisible = false;
			this.grdExCodeList.HeaderFormatStyle.TextAlignment = Janus.Windows.GridEX.TextAlignment.Center;
			this.grdExCodeList.HideSelection = Janus.Windows.GridEX.HideSelection.HighlightInactive;
			this.grdExCodeList.Location = new System.Drawing.Point(0, 0);
			this.grdExCodeList.Name = "grdExCodeList";
			this.grdExCodeList.ScrollBars = Janus.Windows.GridEX.ScrollBars.Vertical;
			this.grdExCodeList.SelectedInactiveFormatStyle.BackColor = System.Drawing.Color.Gold;
			this.grdExCodeList.Size = new System.Drawing.Size(499, 406);
			this.grdExCodeList.TabIndex = 6;
			this.grdExCodeList.TabKeyBehavior = Janus.Windows.GridEX.TabKeyBehavior.ControlNavigation;
			this.grdExCodeList.ThemedAreas = ((Janus.Windows.GridEX.ThemedArea)(((((((((Janus.Windows.GridEX.ThemedArea.ScrollBars | Janus.Windows.GridEX.ThemedArea.EditControls) 
            | Janus.Windows.GridEX.ThemedArea.Headers) 
            | Janus.Windows.GridEX.ThemedArea.GroupByBox) 
            | Janus.Windows.GridEX.ThemedArea.GroupRows) 
            | Janus.Windows.GridEX.ThemedArea.ControlBorder) 
            | Janus.Windows.GridEX.ThemedArea.Cards) 
            | Janus.Windows.GridEX.ThemedArea.Gridlines) 
            | Janus.Windows.GridEX.ThemedArea.CheckBoxes)));
			this.grdExCodeList.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
			this.grdExCodeList.Enter += new System.EventHandler(this.OnGrdRowDetailChanged);
			// 
			// dvCode
			// 
			this.dvCode.Table = this.codeDs.Code;
			// 
			// uiPanel0
			// 
			this.uiPanel0.AllowResize = Janus.Windows.UI.InheritableBoolean.False;
			this.uiPanel0.CaptionStyle = Janus.Windows.UI.Dock.PanelCaptionStyle.Dark;
			this.uiPanel0.GroupStyle = Janus.Windows.UI.Dock.PanelGroupStyle.VerticalTiles;
			this.uiPanel0.Location = new System.Drawing.Point(0, 522);
			this.uiPanel0.Name = "uiPanel0";
			this.uiPanel0.Size = new System.Drawing.Size(1010, 155);
			this.uiPanel0.TabIndex = 4;
			this.uiPanel0.Text = "ÄÚµå»ó¼¼";
			// 
			// uiPanel3
			// 
			this.uiPanel3.InnerContainer = this.uiPanel3Container;
			this.uiPanel3.Location = new System.Drawing.Point(0, 22);
			this.uiPanel3.Name = "uiPanel3";
			this.uiPanel3.Size = new System.Drawing.Size(503, 133);
			this.uiPanel3.TabIndex = 4;
			this.uiPanel3.Text = "±¸ºÐÄÚµå»ó¼¼";
			// 
			// uiPanel3Container
			// 
			this.uiPanel3Container.Controls.Add(this.panel1);
			this.uiPanel3Container.ForeColor = System.Drawing.SystemColors.ControlText;
			this.uiPanel3Container.Location = new System.Drawing.Point(1, 23);
			this.uiPanel3Container.Name = "uiPanel3Container";
			this.uiPanel3Container.Size = new System.Drawing.Size(501, 109);
			this.uiPanel3Container.TabIndex = 0;
			// 
			// panel1
			// 
			this.panel1.Controls.Add(this.btnSave);
			this.panel1.Controls.Add(this.lbTitle);
			this.panel1.Controls.Add(this.ebSection);
			this.panel1.Controls.Add(this.lbChannelNo);
			this.panel1.Controls.Add(this.ebCodeName);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel1.Location = new System.Drawing.Point(0, 0);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(501, 109);
			this.panel1.TabIndex = 19;
			// 
			// btnSave
			// 
			this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.btnSave.BackColor = System.Drawing.SystemColors.Window;
			this.btnSave.Enabled = false;
			this.btnSave.Location = new System.Drawing.Point(8, 80);
			this.btnSave.Name = "btnSave";
			this.btnSave.Size = new System.Drawing.Size(104, 24);
			this.btnSave.TabIndex = 9;
			this.btnSave.Text = "Àú Àå";
			this.btnSave.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
			this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
			// 
			// lbTitle
			// 
			this.lbTitle.Location = new System.Drawing.Point(8, 32);
			this.lbTitle.Name = "lbTitle";
			this.lbTitle.Size = new System.Drawing.Size(72, 21);
			this.lbTitle.TabIndex = 18;
			this.lbTitle.Text = "ÄÚµå¸í";
			this.lbTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// ebSection
			// 
			this.ebSection.Location = new System.Drawing.Point(80, 8);
			this.ebSection.MaxLength = 2;
			this.ebSection.Name = "ebSection";
			this.ebSection.Size = new System.Drawing.Size(160, 21);
			this.ebSection.TabIndex = 12;
			this.ebSection.TabStop = false;
			this.ebSection.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
			this.ebSection.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
			// 
			// lbChannelNo
			// 
			this.lbChannelNo.Location = new System.Drawing.Point(8, 8);
			this.lbChannelNo.Name = "lbChannelNo";
			this.lbChannelNo.Size = new System.Drawing.Size(72, 21);
			this.lbChannelNo.TabIndex = 0;
			this.lbChannelNo.Text = "±¸ºÐÄÚµå";
			this.lbChannelNo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// ebCodeName
			// 
			this.ebCodeName.Location = new System.Drawing.Point(80, 32);
			this.ebCodeName.MaxLength = 120;
			this.ebCodeName.Name = "ebCodeName";
			this.ebCodeName.Size = new System.Drawing.Size(160, 21);
			this.ebCodeName.TabIndex = 7;
			this.ebCodeName.TabStop = false;
			this.ebCodeName.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
			this.ebCodeName.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
			// 
			// uiPanel4
			// 
			this.uiPanel4.InnerContainer = this.uiPanel4Container;
			this.uiPanel4.Location = new System.Drawing.Point(507, 22);
			this.uiPanel4.Name = "uiPanel4";
			this.uiPanel4.Size = new System.Drawing.Size(503, 133);
			this.uiPanel4.TabIndex = 4;
			this.uiPanel4.Text = "ÄÚµå»ó¼¼";
			// 
			// uiPanel4Container
			// 
			this.uiPanel4Container.Controls.Add(this.panel2);
			this.uiPanel4Container.Location = new System.Drawing.Point(1, 23);
			this.uiPanel4Container.Name = "uiPanel4Container";
			this.uiPanel4Container.Size = new System.Drawing.Size(501, 109);
			this.uiPanel4Container.TabIndex = 0;
			// 
			// panel2
			// 
			this.panel2.Controls.Add(this.btnAdd);
			this.panel2.Controls.Add(this.btnSave2);
			this.panel2.Controls.Add(this.btnDelete);
			this.panel2.Controls.Add(this.label1);
			this.panel2.Controls.Add(this.ebCode);
			this.panel2.Controls.Add(this.label2);
			this.panel2.Controls.Add(this.ebCodeName2);
			this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel2.Location = new System.Drawing.Point(0, 0);
			this.panel2.Name = "panel2";
			this.panel2.Size = new System.Drawing.Size(501, 109);
			this.panel2.TabIndex = 13;
			// 
			// btnAdd
			// 
			this.btnAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.btnAdd.BackColor = System.Drawing.SystemColors.Window;
			this.btnAdd.Enabled = false;
			this.btnAdd.Location = new System.Drawing.Point(232, 80);
			this.btnAdd.Name = "btnAdd";
			this.btnAdd.Size = new System.Drawing.Size(104, 24);
			this.btnAdd.TabIndex = 12;
			this.btnAdd.Text = "Ãß °¡";
			this.btnAdd.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
			this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
			// 
			// btnSave2
			// 
			this.btnSave2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.btnSave2.BackColor = System.Drawing.SystemColors.Window;
			this.btnSave2.Enabled = false;
			this.btnSave2.Location = new System.Drawing.Point(8, 80);
			this.btnSave2.Name = "btnSave2";
			this.btnSave2.Size = new System.Drawing.Size(104, 24);
			this.btnSave2.TabIndex = 10;
			this.btnSave2.Text = "Àú Àå";
			this.btnSave2.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
			this.btnSave2.Click += new System.EventHandler(this.btnSave2_Click);
			// 
			// btnDelete
			// 
			this.btnDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.btnDelete.Enabled = false;
			this.btnDelete.Location = new System.Drawing.Point(120, 80);
			this.btnDelete.Name = "btnDelete";
			this.btnDelete.Size = new System.Drawing.Size(104, 24);
			this.btnDelete.TabIndex = 11;
			this.btnDelete.Text = "»è Á¦";
			this.btnDelete.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
			this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(8, 32);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(72, 21);
			this.label1.TabIndex = 22;
			this.label1.Text = "ÄÚµå¸í";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// ebCode
			// 
			this.ebCode.Location = new System.Drawing.Point(80, 8);
			this.ebCode.MaxLength = 3;
			this.ebCode.Name = "ebCode";
			this.ebCode.Size = new System.Drawing.Size(160, 21);
			this.ebCode.TabIndex = 21;
			this.ebCode.TabStop = false;
			this.ebCode.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
			this.ebCode.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(8, 8);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(72, 21);
			this.label2.TabIndex = 19;
			this.label2.Text = "ÄÚµå";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// ebCodeName2
			// 
			this.ebCodeName2.Location = new System.Drawing.Point(80, 32);
			this.ebCodeName2.MaxLength = 120;
			this.ebCodeName2.Name = "ebCodeName2";
			this.ebCodeName2.Size = new System.Drawing.Size(160, 21);
			this.ebCodeName2.TabIndex = 8;
			this.ebCodeName2.TabStop = false;
			this.ebCodeName2.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
			this.ebCodeName2.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
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
			// CodeControl
			// 
			this.Controls.Add(this.uiPanelUsers);
			this.Font = global::AdManagerClient.Properties.Settings.Default.Font1;
			this.Name = "CodeControl";
			this.Size = new System.Drawing.Size(1010, 677);
			this.Load += new System.EventHandler(this.CodeControl_Load);
			((System.ComponentModel.ISupportInitialize)(this.uiPM)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.uiPanelUsers)).EndInit();
			this.uiPanelUsers.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.uiPanelUsersSearch)).EndInit();
			this.uiPanelUsersSearch.ResumeLayout(false);
			this.uiPanelUsersSearchContainer.ResumeLayout(false);
			this.pnlSearch.ResumeLayout(false);
			this.pnlSearch.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.uiPanelChannelSet)).EndInit();
			this.uiPanelChannelSet.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.uiPanel1)).EndInit();
			this.uiPanel1.ResumeLayout(false);
			this.uiPanel1Container.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.grdExSectionList)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.dvSection)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.codeDs)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.uiPanel2)).EndInit();
			this.uiPanel2.ResumeLayout(false);
			this.uiPanel2Container.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.grdExCodeList)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.dvCode)).EndInit();
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
		private void CodeControl_Load(object sender, System.EventArgs e)
		{		
			// µ¥ÀÌÅÍ°ü¸®¿ë °´Ã¼»ý¼º
			dt = ((DataView)grdExSectionList.DataSource).Table;
			dtChild  = ((DataView)grdExCodeList.DataSource).Table;

			cm = (CurrencyManager) this.BindingContext[grdExSectionList.DataSource]; 
			cm.PositionChanged += new System.EventHandler(OnGrdRowChanged); 

			cmChild = (CurrencyManager) this.BindingContext[grdExCodeList.DataSource]; 
			cmChild.PositionChanged += new System.EventHandler(OnGrdRowDetailChanged); 		

			// ÄÁÆ®·Ñ ÃÊ±âÈ­
			InitControl();			
		}

		#endregion

		#region ÄÁÆ®·Ñ ÃÊ±âÈ­
		private void InitControl()
		{
			ProgressStart();
			InitCombo();
			
			// Á¶È¸±ÇÇÑ °Ë»ç
			if(menu.CanRead(MenuCode))
			{
				canRead = true;
				SearchSection();
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
			CodeModel codeModel = new CodeModel();		
			new CodeManager(systemModel, commonModel).GetSectionList(codeModel);
			
			if (codeModel.ResultCD.Equals("0000"))
			{
				// µ¥ÀÌÅÍ¼Â¿¡ ¼ÂÆÃ
				Utility.SetDataTable(codeDs.Section, codeModel.CodeDataSet);				
			}

			// »ó¼¼Á¶È¸ ÄÞº¸
			// »ó¼¼Á¤º¸ÀÇ ÄÞº¸´Â DatasetÀ» µ¥ÀÌÅÍ¼Ò½º·Î °¡Áø´Ù.

			// °Ë»öÁ¶°ÇÀÇ ÄÞº¸
			this.cbSearchSection.Items.Clear();
			
			// ÄÞº¸¹Ú½º¿¡ ¼ÂÆ®ÇÒ ÄÚµå¸ñ·ÏÀ» ´ãÀ» Item¹è¿­À» ¼±¾ð
			Janus.Windows.EditControls.UIComboBoxItem[] comboItems = new Janus.Windows.EditControls.UIComboBoxItem[codeModel.ResultCnt + 1];

			comboItems[0] = new Janus.Windows.EditControls.UIComboBoxItem("ÄÚµå±¸ºÐ¼±ÅÃ","00");
			
			for(int i=0;i<codeModel.ResultCnt;i++)
			{
				DataRow row = codeDs.Section.Rows[i];

				string val = row["Section"].ToString();
				string txt = row["CodeName"].ToString();
				comboItems[i+1] = new Janus.Windows.EditControls.UIComboBoxItem(txt,val);
			}
			// ÄÞº¸¿¡ ¼ÂÆ®
			this.cbSearchSection.Items.AddRange(comboItems);
			this.cbSearchSection.SelectedIndex = 0;

			Application.DoEvents();
		}
		
		
		private void InitButton()
		{
			if(canRead)   btnSearch.Enabled = true;
			if(canCreate) btnAdd.Enabled    = true;
			
			if(ebSection.Text.Trim().Length > 0 || ebCodeName.Text.Trim().Length > 0) 
			{				
				if(canUpdate) btnSave.Enabled   = true;
			}
			if(ebCode.Text.Trim().Length > 0 || ebCodeName2.Text.Trim().Length > 0) 
			{				
				if(canUpdate) btnSave2.Enabled   = true;
				if(canDelete) btnDelete.Enabled = true;
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
            if (!IsSearching) // 2011.11.29 JH.Park Á¶È¸ÁßÀÌ ¾Æ´Ò°æ¿ì¿¡¸¸ µ¿ÀÛÇÏµµ·Ï º¯°æ
            {
                if (grdExSectionList.RecordCount > 0 && grdExSectionList.RowCount > 0)
                {
                    SetSectionDetailText();
                    section_old = grdExSectionList.SelectedItems[0].GetRow().Cells["Section"].Value.ToString();
                }
                SetTextReadonly();
                InitButton();
            }
		}

		
		private void OnGrdRowDetailChanged(object sender, System.EventArgs e) 
		{
            if (grdExCodeList.RecordCount > 0 && grdExCodeList.RowCount > 0 && grdExCodeList.SelectedItems.Count > 0)
			{
				SetChannelSetDetailText();				
				code_old		= grdExCodeList.SelectedItems[0].GetRow().Cells["Code"].Value.ToString();								
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
			if(cbSearchSection.SelectedValue.ToString() == "00") 
			{
				SearchSection();
			}
			ReSetCodeDetailText();
			ReSetGridData();
			DisableButton();
			SetTextReadonly();			
			SearchSection();			
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
			btnSave2.Enabled   = true;

			//btnDetailAdd.Enabled = true;
			IsAdding = true;

			ResetTextReadonly();
			ReSetCodeDetailText();
			//ReSetGridData();

			ebCode.Focus();
		}

		private void btnDelete_Click(object sender, System.EventArgs e)
		{
			DeleteCode();
		}

		/// <summary>
		/// ÀúÀå¹öÆ° Å¬¸¯
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnSave_Click(object sender, System.EventArgs e)
		{			
			SaveSectionDetail();
			//ReSetGridData();
			//SearchSectionDetail();			
		}

		private void btnSave2_Click(object sender, System.EventArgs e)
		{
			SaveCodeDetail();
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
				SearchSection();
			}
		}

		#endregion

		#region Ã³¸®¸Þ¼Òµå

		/// <summary>
		/// ÄÚµå¸ñ·Ï Á¶È¸
		/// </summary>
		private void SearchSection()
		{
            IsSearching = true;

			StatusMessage("ÄÚµå Á¤º¸¸¦ Á¶È¸ÇÕ´Ï´Ù.");		

			try
			{
				codeModel.Init();
				// µ¥ÀÌÅÍ¸ðµ¨¿¡ Àü¼ÛÇÒ ³»¿ëÀ» ¼ÂÆ®ÇÑ´Ù.				
				if(IsNewSearchKey)
				{
					codeModel.SearchKey = "";
				}
				else
				{
					codeModel.SearchKey  = ebSearchKey.Text;
				}

				codeModel.SearchSection = cbSearchSection.SelectedItem.Value.ToString();
				
				ReSetCodeDetailText();

				// ÄÚµå¸ñ·ÏÁ¶È¸ ¼­ºñ½º¸¦ È£ÃâÇÑ´Ù.
				new CodeManager(systemModel,commonModel).GetSectionList(codeModel);

				if (codeModel.ResultCD.Equals("0000"))
				{
					Utility.SetDataTable(codeDs.Section, codeModel.CodeDataSet);
					StatusMessage(codeModel.ResultCnt + "°ÇÀÇ ÄÚµå Á¤º¸°¡ Á¶È¸µÇ¾ú½À´Ï´Ù.");
					if(canUpdate)
					{
						AddSchChoice();									
					}					
					SearchSectionDetail();
				}
			}
			catch(FrameException fe)
			{
				FrameSystem.showMsgForm("ÄÚµåÁ¶È¸¿À·ù", new string[] {fe.ErrCode, fe.ResultMsg});
			}
			catch(Exception ex)
			{
				FrameSystem.showMsgForm("ÄÚµåÁ¶È¸¿À·ù",new string[] {"",ex.Message});
            }
            finally
            {
                IsSearching = false; // Á¶È¸Áß Flag ¸®¼Â
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
				if ( codeDs.Tables["Section"].Rows.Count < 1 ) return;
              
				foreach (DataRow row in codeDs.Tables["Section"].Rows)
				{					
					
					if(row["Section"].ToString().Equals(section))
					{					
						cm.Position = rowIndex;
						break;								
					}					

					rowIndex++;
					grdExSectionList.EnsureVisible();
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
		/// ÄÚµå¸ñ·Ï Á¶È¸
		/// </summary>
		private void SearchSectionDetail()
		{
			StatusMessage("ÄÚµå µðÅ×ÀÏ Á¤º¸¸¦ Á¶È¸ÇÕ´Ï´Ù.");

			try
			{					
				codeModel.Init();
				// µ¥ÀÌÅÍ¸ðµ¨¿¡ Àü¼ÛÇÒ ³»¿ëÀ» ¼ÂÆ®ÇÑ´Ù.				
				if(IsNewSearchKey)
				{
					codeModel.SearchKey = "";
				}
				else
				{
					codeModel.SearchKey  = ebSearchKey.Text;
				}
				int curRow = cm.Position;          

				if(curRow < 0) return;
											
				codeModel.Section = dt.Rows[curRow]["Section"].ToString();
				codeModel.CodeName = dt.Rows[curRow]["CodeName"].ToString();
				ebSection.Text                = dt.Rows[curRow]["Section"].ToString();
				ebCodeName.Text          = dt.Rows[curRow]["CodeName"].ToString();
				section                = dt.Rows[curRow]["Section"].ToString();
			
				// ÄÚµå¸ñ·ÏÁ¶È¸ ¼­ºñ½º¸¦ È£ÃâÇÑ´Ù.
				new CodeManager(systemModel,commonModel).GetCodeList(codeModel);

				if (codeModel.ResultCD.Equals("0000"))
				{
					Utility.SetDataTable(codeDs.Code, codeModel.CodeDataSet);				
					StatusMessage(codeModel.ResultCnt + "°ÇÀÇ ÄÚµå Á¤º¸°¡ Á¶È¸µÇ¾ú½À´Ï´Ù.");
					SetChannelSetDetailText();
				}
				//btnDetailAdd.Enabled = true;
            
				if(codeModel.ResultCnt > 0)
				{
					//btnDetailDelete.Enabled = true;   
				}
			}
			catch(FrameException fe)
			{
				FrameSystem.showMsgForm("ÄÚµåÁ¶È¸¿À·ù", new string[] {fe.ErrCode, fe.ResultMsg});
			}
			catch(Exception ex)
			{
				FrameSystem.showMsgForm("ÄÚµåÁ¶È¸¿À·ù",new string[] {"",ex.Message});
			}
		}

		private void SearchCodeDetail()
		{
			StatusMessage("ÄÚµå µðÅ×ÀÏ Á¤º¸¸¦ Á¶È¸ÇÕ´Ï´Ù.");

			try
			{               
				// µ¥ÀÌÅÍ¸ðµ¨¿¡ Àü¼ÛÇÒ ³»¿ëÀ» ¼ÂÆ®ÇÑ´Ù.
                
				int curRow = cmChild.Position;
             
				ebCode.Text                = dtChild.Rows[curRow]["Code"].ToString();
				ebCodeName2.Text          = dtChild.Rows[curRow]["CodeName"].ToString();
				code          = dtChild.Rows[curRow]["Code"].ToString();
				
				// ÄÚµå¸ñ·ÏÁ¶È¸ ¼­ºñ½º¸¦ È£ÃâÇÑ´Ù.
				new CodeManager(systemModel,commonModel).GetCodeList(codeModel);

				if (codeModel.ResultCD.Equals("0000"))
				{
					Utility.SetDataTable(codeDs.Code, codeModel.CodeDataSet);
					StatusMessage(codeModel.ResultCnt + "°ÇÀÇ ÄÚµå Á¤º¸°¡ Á¶È¸µÇ¾ú½À´Ï´Ù.");		
					if(canUpdate)
					{
						AddSchChoice_Code();									
					}					
				}			
			}
			catch(FrameException fe)
			{
				FrameSystem.showMsgForm("ÄÚµåÁ¶È¸¿À·ù", new string[] {fe.ErrCode, fe.ResultMsg});
			}
			catch(Exception ex)
			{
				FrameSystem.showMsgForm("ÄÚµåÁ¶È¸¿À·ù",new string[] {"",ex.Message});
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
				if ( codeDs.Tables["Code"].Rows.Count < 1 ) return;
              
				foreach (DataRow row in codeDs.Tables["Code"].Rows)
				{					
					if(IsAdding)
					{
						//cm.Position = 0;
						code = "";						
					}
					else
					{						
						if(row["Code"].ToString().Equals(code))
						{					
							cmChild.Position = rowIndex;
							break;								
						}
					}

					rowIndex++;
					grdExCodeList.EnsureVisible();
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
		/// ÄÚµå ÀúÀå
		/// </summary>
		private void SaveSectionDetail()
		{
			//IsAdding = true;

			StatusMessage("ÄÚµå±¸ºÐ Á¤º¸¸¦ ÀúÀåÇÕ´Ï´Ù.");                        
                      
			if(ebSection.Text.ToString().Length == 0) 
			{
				MessageBox.Show("ÄÚµå°¡ ÀÔ·ÂµÇÁö ¾Ê¾Ò½À´Ï´Ù.","ÄÚµå ÀúÀå", 
					MessageBoxButtons.OK, MessageBoxIcon.Information );
				return;								
			}
			if(ebCodeName.Text.ToString().Length == 0) 
			{
				MessageBox.Show("ÄÚµå¸íÀÌ ÀÔ·ÂµÇÁö ¾Ê¾Ò½À´Ï´Ù.","ÄÚµå ÀúÀå", 
					MessageBoxButtons.OK, MessageBoxIcon.Information );
				return;					
			}						
                        
			try
			{
				//ÀúÀå Àü¿¡ ¸ðµ¨À» ÃÊ±âÈ­ ÇØÁØ´Ù.
				codeModel.Init();
				// µ¥ÀÌÅÍ¸ðµ¨¿¡ Àü¼ÛÇÒ ³»¿ëÀ» ¼ÂÆ®ÇÑ´Ù.
				codeModel.Section      = ebSection.Text;
				codeModel.CodeName      = ebCodeName.Text;				

				int curRow = cm.Position;
						
				if(curRow >= 0)
				{
					codeModel.Section_old      = dt.Rows[curRow]["Section"].ToString();						
				}			
									
				//codeModel.CodeDataSet = codeDs.Copy();                         

				// ÄÚµå »ó¼¼Á¤º¸ ÀúÀå ¼­ºñ½º¸¦ È£ÃâÇÑ´Ù.
				
				new CodeManager(systemModel,commonModel).SetSectionUpdate(codeModel);
				StatusMessage("ÄÚµå±¸ºÐ Á¤º¸°¡ ¼öÁ¤µÇ¾ú½À´Ï´Ù.");
				
				
				DisableButton();
				SearchSection();
				InitButton();
                        
			}
			catch(FrameException fe)
			{
				FrameSystem.showMsgForm("ÄÚµå±¸ºÐ ÀúÀå¿À·ù", new string[] {fe.ErrCode, fe.ResultMsg});
			}
			catch(Exception ex)
			{
				FrameSystem.showMsgForm("ÄÚµå±¸ºÐ ÀúÀå¿À·ù",new string[] {"",ex.Message});
			}			
		}


		/// <summary>
		/// ÄÚµå ÀúÀå
		/// </summary>
		private void SaveCodeDetail()
		{
			//IsAdding = true;

			StatusMessage("ÄÚµå Á¤º¸¸¦ ÀúÀåÇÕ´Ï´Ù.");                        
                      
			if(ebCode.Text.ToString().Length == 0) 
			{
				MessageBox.Show("ÄÚµå°¡ ÀÔ·ÂµÇÁö ¾Ê¾Ò½À´Ï´Ù.","ÄÚµå ÀúÀå", MessageBoxButtons.OK, MessageBoxIcon.Information );
				return;								
			}

			if(ebCodeName2.Text.ToString().Length == 0) 
			{
				MessageBox.Show("ÄÚµå¸íÀÌ ÀÔ·ÂµÇÁö ¾Ê¾Ò½À´Ï´Ù.","ÄÚµå ÀúÀå", MessageBoxButtons.OK, MessageBoxIcon.Information );
				return;					
			}						
                        
			try
			{
				codeModel.Init();
				codeModel.Code		= ebCode.Text;
				codeModel.CodeName  = ebCodeName2.Text;

				int curRow = cmChild.Position;
						
				if(curRow >= 0)
				{
					codeModel.Section	= dtChild.Rows[curRow]["Section"].ToString();	
					codeModel.Code_old	= dtChild.Rows[curRow]["Code"].ToString();					
				}							
													
				//codeModel.CodeDataSet = codeDs.Copy();                         

				// ÄÚµå »ó¼¼Á¤º¸ ÀúÀå ¼­ºñ½º¸¦ È£ÃâÇÑ´Ù.
				if (IsAdding)
				{
					new CodeManager(systemModel,commonModel).SetCodeCreate(codeModel);
					StatusMessage("ÄÚµå Á¤º¸°¡ Ãß°¡µÇ¾ú½À´Ï´Ù.");
					IsAdding = false;
					ReSetCodeDetailText();
				}
				else
				{
					new CodeManager(systemModel,commonModel).SetCodeUpdate(codeModel);
					StatusMessage("ÄÚµå Á¤º¸°¡ ¼öÁ¤µÇ¾ú½À´Ï´Ù.");
				}
				
				DisableButton();
				SearchCodeDetail();
				InitButton();
                        
			}
			catch(FrameException fe)
			{
				FrameSystem.showMsgForm("ÄÚµå ÀúÀå¿À·ù", new string[] {fe.ErrCode, fe.ResultMsg});
			}
			catch(Exception ex)
			{
				FrameSystem.showMsgForm("ÄÚµå ÀúÀå¿À·ù",new string[] {"",ex.Message});
			}			
		}

		/// <summary>
		/// ÄÚµåÁ¤º¸ »èÁ¦
		/// </summary>
		private void DeleteCode()
		{
			StatusMessage("ÄÚµå Á¤º¸¸¦ »èÁ¦ÇÕ´Ï´Ù.");

			if(ebCode.Text.Trim().Length == 0) 
			{
				MessageBox.Show("»èÁ¦ÇÒ ÄÚµå Á¤º¸°¡ ¾ø½À´Ï´Ù.","ÄÚµå »èÁ¦", 
					MessageBoxButtons.OK, MessageBoxIcon.Information );
				return;
			}

			DialogResult result = MessageBox.Show("ÇØ´ç ÄÚµå Á¤º¸¸¦ »èÁ¦ ÇÏ½Ã°Ú½À´Ï±î?","ÄÚµå »èÁ¦",
				MessageBoxButtons.YesNo, MessageBoxIcon.Question,
				MessageBoxDefaultButton.Button2);

			if (result == DialogResult.No) return;

			try 
			{
				// µ¥ÀÌÅÍ¸ðµ¨¿¡ Àü¼ÛÇÒ ³»¿ëÀ» ¼ÂÆ®ÇÑ´Ù.
				int curRow = cmChild.Position;
						
				if(curRow >= 0)
				{
					codeModel.Section      = dtChild.Rows[curRow]["Section"].ToString();	
					codeModel.Code_old = dtChild.Rows[curRow]["Code"].ToString();					
				}

				// ÄÚµå »ó¼¼Á¤º¸ ÀúÀå ¼­ºñ½º¸¦ È£ÃâÇÑ´Ù.
				new CodeManager(systemModel,commonModel).SetCodeDelete(codeModel);

				StatusMessage("ÄÚµå Á¤º¸°¡ »èÁ¦µÇ¾ú½À´Ï´Ù.");	
				
				DisableButton();
				SearchCodeDetail();
				InitButton();

			}
			catch(FrameException fe)
			{
				FrameSystem.showMsgForm("ÄÚµå »èÁ¦¿À·ù", new string[] {fe.ErrCode, fe.ResultMsg});
			}
			catch(Exception ex)
			{
				FrameSystem.showMsgForm("ÄÚµå »èÁ¦¿À·ù",new string[] {"",ex.Message});
			}			
		}

		
		/// <summary>
		/// ÄÚµå±¸ºÐ »ó¼¼Á¤º¸ÀÇ ¼ÂÆ®
		/// </summary>
		private void SetSectionDetailText()
		{
			int curRow = cm.Position;
            
			ebSection.ReadOnly = true;
			ebCode.ReadOnly = true;		
			ebCodeName.ReadOnly         = false;			

			ebSection.BackColor = Color.WhiteSmoke;
			ebCode.BackColor = Color.WhiteSmoke;	
			ebCodeName.BackColor        = Color.White;			

			SearchSectionDetail();
            
			IsAdding = false;
            
			StatusMessage("ÁØºñ");
		}

		private void SetChannelSetDetailText()
		{		
			int curRow = cmChild.Position;
					
			if(curRow >= 0)
			{
				ebCode.Text = dtChild.Rows[curRow]["Code"].ToString();
				ebCodeName2.Text = dtChild.Rows[curRow]["CodeName"].ToString();

				IsAdding = false;			
			}					
				
			StatusMessage("ÁØºñ");
		}

		private void ReSetCodeDetailText()
		{			
			ebCode.Text                 = "";			
			ebCodeName2.Text                 = "";
		}
        
		private void ReSetGridData()
		{			
			//Ãß°¡¸¦ ÇÏ¸é ÄÚµå¼Â±×¸®µå¸¦ ¸®¼ÂÇÑ´Ù.
			codeDs.Code.Clear();        
		}
        
		
		/// <summary>
		/// »ó¼¼Á¤º¸ ReadOnly
		/// </summary>
		private void SetTextReadonly()
		{
			ebSection.ReadOnly = true;
			ebCode.ReadOnly = true;

			ebSection.BackColor = Color.WhiteSmoke;
			ebCode.BackColor = Color.WhiteSmoke;
		}

		/// <summary>
		/// »ó¼¼Á¤º¸ ¼öÁ¤°¡´ÉÄÉ
		/// </summary>
		private void ResetTextReadonly()
		{	
			ebSection.ReadOnly = true;
			ebCode.ReadOnly = false;
			ebCodeName2.ReadOnly = false; 

			ebSection.BackColor = Color.WhiteSmoke;
			ebCode.BackColor = Color.White;
			ebCodeName2.BackColor     = Color.White;						
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