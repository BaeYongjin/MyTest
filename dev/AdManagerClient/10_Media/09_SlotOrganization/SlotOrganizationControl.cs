// ===============================================================================
// SlotOrganizationControl for Charites Project
//
// SlotOrganizationControl.cs
//
// Ã¤³ÎÁ¤º¸°ü¸® ÄÁµå·ÑÀ» Á¤ÀÇÇÕ´Ï´Ù. 
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
using Excel = Microsoft.Office.Interop.Excel; // ¿¢¼¿ ÂüÁ¶
using System.Reflection;

namespace AdManagerClient
{
	/// <summary>
	/// Ã¤³Î°ü¸® ÄÁÆ®·Ñ
	/// </summary>
	public class SlotOrganizationControl : System.Windows.Forms.UserControl, IUserControl
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
		SlotOrganizationModel slotOrganizationModel  = new SlotOrganizationModel();	// Ã¤³ÎÁ¤º¸¸ðµ¨

		// È­¸éÃ³¸®¿ë º¯¼ö		
		CurrencyManager cm        = null;					// µ¥ÀÌÅÍ ±×¸®µåÀÇ º¯°æ¿¡ µû¸¥ µ¥ÀÌÅÍ¼Â °ü¸®¸¦ À§ÇÏ¿©					
		DataTable       dt        = null;
		DataTable       dtChild        = null;
		private string        genreCode = null;

		private string        mediaCode_old = null;
		private string        categoryCode_old = null;
		private string        genreCode_old = null;
		private string        channelNo_old = null;

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
		private Janus.Windows.UI.Dock.UIPanel uiPanelUserDetail;
		private Janus.Windows.UI.Dock.UIPanelInnerContainer uiPanelUserDetailContainer;
		private Janus.Windows.UI.Dock.UIPanel uiPanelUsersSearch;
		private Janus.Windows.UI.Dock.UIPanelInnerContainer uiPanelUsersSearchContainer;
		private Janus.Windows.EditControls.UIButton uiButton2;
		private Janus.Windows.EditControls.UIButton uiButton1;
		private Janus.Windows.UI.Dock.UIPanelGroup uiPanelUsers;
		private System.Windows.Forms.Panel pnlSearch;
		private Janus.Windows.EditControls.UIButton btnSearch;
		private System.Windows.Forms.Panel pnlUserDetail;
		private Janus.Windows.EditControls.UIButton btnDelete;
		private Janus.Windows.EditControls.UIButton btnAdd;
		private Janus.Windows.EditControls.UIButton btnSave;
		private Janus.Windows.GridEX.EditControls.EditBox ebChannelNo;
		private Janus.Windows.GridEX.EditControls.EditBox ebModDt;
		private Janus.Windows.EditControls.UIComboBox cbMediaName;
		private Janus.Windows.EditControls.UIComboBox cbSearchMediaName;
		private Janus.Windows.EditControls.UIComboBox cbSearchCategoryName;
		private Janus.Windows.EditControls.UIComboBox cbSearchGenreName;
		private Janus.Windows.EditControls.UIComboBox cbCategoryName;
		private System.Windows.Forms.Label lbGenreName;
		private System.Windows.Forms.Label lbCategoryName;
		private System.Windows.Forms.Label lbMediaName;
		private System.Windows.Forms.Label lbModDt;
		private System.Windows.Forms.Label lbChannelNo;
		private Janus.Windows.GridEX.EditControls.EditBox ebGenreName;
		private Janus.Windows.UI.Dock.UIPanelGroup uiPanelChannelSet;
		private Janus.Windows.UI.Dock.UIPanel uiPanel2;
		private Janus.Windows.UI.Dock.UIPanelInnerContainer uiPanel2Container;
		private Janus.Windows.GridEX.GridEX grdExChannelSetList;
		private Janus.Windows.EditControls.UIButton btnGenreName;
		private Janus.Windows.EditControls.UIButton btnChannelNo;
		private Janus.Windows.EditControls.UIGroupBox gpSlot;
		private System.Windows.Forms.Label lbSlot1;
		private System.Windows.Forms.Label lbSlot2;
		private System.Windows.Forms.Label lbSlot3;
		private System.Windows.Forms.Label lbSlot4;
		private System.Windows.Forms.Label lbSlot5;
		private System.Windows.Forms.Label lbSlot10;
		private System.Windows.Forms.Label lbSlot9;
		private System.Windows.Forms.Label lbSlot8;
		private System.Windows.Forms.Label lbSlot7;
		private System.Windows.Forms.Label lbSlot6;
		private Janus.Windows.EditControls.UIComboBox cbSlot1;
		private Janus.Windows.EditControls.UIComboBox cbSlot2;
		private Janus.Windows.EditControls.UIComboBox cbSlot3;
		private Janus.Windows.EditControls.UIComboBox cbSlot4;
		private Janus.Windows.EditControls.UIComboBox cbSlot5;
		private Janus.Windows.EditControls.UIComboBox cbSlot10;
		private Janus.Windows.EditControls.UIComboBox cbSlot9;
		private Janus.Windows.EditControls.UIComboBox cbSlot8;
		private Janus.Windows.EditControls.UIComboBox cbSlot7;
		private Janus.Windows.EditControls.UIComboBox cbSlot6;
		private System.Windows.Forms.Label lbRegDt;
		private Janus.Windows.GridEX.EditControls.EditBox ebRegDt;
		private System.Windows.Forms.Label lbRegID;
		private Janus.Windows.GridEX.EditControls.EditBox ebRegID;
		private Janus.Windows.EditControls.UICheckBox chkUseYn;
		private System.Windows.Forms.Label lbUseYn;
		private Janus.Windows.EditControls.UIRadioButton rbUseYn_N;
		private Janus.Windows.EditControls.UIRadioButton rbUseYn_Y;
		private System.Data.DataView dvSlot;
		private AdManagerClient._10_Media._09_SlotOrganization.SlotOrganizationDs slotOrganizationDs;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;		
		private System.ComponentModel.IContainer components;

		public SlotOrganizationControl()
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
            Janus.Windows.GridEX.GridEXLayout grdExChannelSetList_DesignTimeLayout = new Janus.Windows.GridEX.GridEXLayout();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SlotOrganizationControl));
            this.uiPM = new Janus.Windows.UI.Dock.UIPanelManager(this.components);
            this.uiPanelUsers = new Janus.Windows.UI.Dock.UIPanelGroup();
            this.uiPanelUsersSearch = new Janus.Windows.UI.Dock.UIPanel();
            this.uiPanelUsersSearchContainer = new Janus.Windows.UI.Dock.UIPanelInnerContainer();
            this.pnlSearch = new System.Windows.Forms.Panel();
            this.chkUseYn = new Janus.Windows.EditControls.UICheckBox();
            this.cbSearchMediaName = new Janus.Windows.EditControls.UIComboBox();
            this.cbSearchCategoryName = new Janus.Windows.EditControls.UIComboBox();
            this.cbSearchGenreName = new Janus.Windows.EditControls.UIComboBox();
            this.btnSearch = new Janus.Windows.EditControls.UIButton();
            this.uiPanelChannelSet = new Janus.Windows.UI.Dock.UIPanelGroup();
            this.uiPanel2 = new Janus.Windows.UI.Dock.UIPanel();
            this.uiPanel2Container = new Janus.Windows.UI.Dock.UIPanelInnerContainer();
            this.grdExChannelSetList = new Janus.Windows.GridEX.GridEX();
            this.dvSlot = new System.Data.DataView();
            this.slotOrganizationDs = new AdManagerClient._10_Media._09_SlotOrganization.SlotOrganizationDs();
            this.uiPanelUserDetail = new Janus.Windows.UI.Dock.UIPanel();
            this.uiPanelUserDetailContainer = new Janus.Windows.UI.Dock.UIPanelInnerContainer();
            this.pnlUserDetail = new System.Windows.Forms.Panel();
            this.lbUseYn = new System.Windows.Forms.Label();
            this.rbUseYn_N = new Janus.Windows.EditControls.UIRadioButton();
            this.rbUseYn_Y = new Janus.Windows.EditControls.UIRadioButton();
            this.ebRegID = new Janus.Windows.GridEX.EditControls.EditBox();
            this.lbRegID = new System.Windows.Forms.Label();
            this.ebRegDt = new Janus.Windows.GridEX.EditControls.EditBox();
            this.lbRegDt = new System.Windows.Forms.Label();
            this.gpSlot = new Janus.Windows.EditControls.UIGroupBox();
            this.cbSlot10 = new Janus.Windows.EditControls.UIComboBox();
            this.lbSlot10 = new System.Windows.Forms.Label();
            this.cbSlot9 = new Janus.Windows.EditControls.UIComboBox();
            this.lbSlot9 = new System.Windows.Forms.Label();
            this.cbSlot8 = new Janus.Windows.EditControls.UIComboBox();
            this.lbSlot8 = new System.Windows.Forms.Label();
            this.cbSlot7 = new Janus.Windows.EditControls.UIComboBox();
            this.lbSlot7 = new System.Windows.Forms.Label();
            this.cbSlot6 = new Janus.Windows.EditControls.UIComboBox();
            this.lbSlot6 = new System.Windows.Forms.Label();
            this.cbSlot5 = new Janus.Windows.EditControls.UIComboBox();
            this.lbSlot5 = new System.Windows.Forms.Label();
            this.cbSlot4 = new Janus.Windows.EditControls.UIComboBox();
            this.lbSlot4 = new System.Windows.Forms.Label();
            this.cbSlot3 = new Janus.Windows.EditControls.UIComboBox();
            this.lbSlot3 = new System.Windows.Forms.Label();
            this.cbSlot2 = new Janus.Windows.EditControls.UIComboBox();
            this.lbSlot2 = new System.Windows.Forms.Label();
            this.cbSlot1 = new Janus.Windows.EditControls.UIComboBox();
            this.lbSlot1 = new System.Windows.Forms.Label();
            this.btnChannelNo = new Janus.Windows.EditControls.UIButton();
            this.btnGenreName = new Janus.Windows.EditControls.UIButton();
            this.ebGenreName = new Janus.Windows.GridEX.EditControls.EditBox();
            this.ebModDt = new Janus.Windows.GridEX.EditControls.EditBox();
            this.lbModDt = new System.Windows.Forms.Label();
            this.cbCategoryName = new Janus.Windows.EditControls.UIComboBox();
            this.ebChannelNo = new Janus.Windows.GridEX.EditControls.EditBox();
            this.lbChannelNo = new System.Windows.Forms.Label();
            this.btnDelete = new Janus.Windows.EditControls.UIButton();
            this.btnAdd = new Janus.Windows.EditControls.UIButton();
            this.btnSave = new Janus.Windows.EditControls.UIButton();
            this.cbMediaName = new Janus.Windows.EditControls.UIComboBox();
            this.lbMediaName = new System.Windows.Forms.Label();
            this.lbCategoryName = new System.Windows.Forms.Label();
            this.lbGenreName = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
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
            ((System.ComponentModel.ISupportInitialize)(this.uiPanel2)).BeginInit();
            this.uiPanel2.SuspendLayout();
            this.uiPanel2Container.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdExChannelSetList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dvSlot)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.slotOrganizationDs)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelUserDetail)).BeginInit();
            this.uiPanelUserDetail.SuspendLayout();
            this.uiPanelUserDetailContainer.SuspendLayout();
            this.pnlUserDetail.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gpSlot)).BeginInit();
            this.gpSlot.SuspendLayout();
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
            this.uiPanel2.Id = new System.Guid("3d64f731-de32-42ba-a10f-ac3b5ac56703");
            this.uiPanelChannelSet.Panels.Add(this.uiPanel2);
            this.uiPanelUsers.Panels.Add(this.uiPanelChannelSet);
            this.uiPanelUserDetail.Id = new System.Guid("b2973a57-d3e1-4d80-9387-7a6dc3332542");
            this.uiPanelUsers.Panels.Add(this.uiPanelUserDetail);
            this.uiPM.Panels.Add(this.uiPanelUsers);
            // 
            // Design Time Panel Info:
            // 
            this.uiPM.BeginPanelInfo();
            this.uiPM.AddDockPanelInfo(new System.Guid("b162c53a-7940-47cd-a0ed-9762c59c50d8"), Janus.Windows.UI.Dock.PanelGroupStyle.HorizontalTiles, Janus.Windows.UI.Dock.PanelDockStyle.Fill, true, new System.Drawing.Size(1010, 677), true);
            this.uiPM.AddDockPanelInfo(new System.Guid("afb3b7d0-aa79-443c-a1e8-3baba38f7a4d"), new System.Guid("b162c53a-7940-47cd-a0ed-9762c59c50d8"), 42, true);
            this.uiPM.AddDockPanelInfo(new System.Guid("88f86479-98e4-43d4-bf03-12fd789fcfc5"), new System.Guid("b162c53a-7940-47cd-a0ed-9762c59c50d8"), Janus.Windows.UI.Dock.PanelGroupStyle.VerticalTiles, true, 350, true);
            this.uiPM.AddDockPanelInfo(new System.Guid("3d64f731-de32-42ba-a10f-ac3b5ac56703"), new System.Guid("88f86479-98e4-43d4-bf03-12fd789fcfc5"), 422, true);
            this.uiPM.AddDockPanelInfo(new System.Guid("b2973a57-d3e1-4d80-9387-7a6dc3332542"), new System.Guid("b162c53a-7940-47cd-a0ed-9762c59c50d8"), 235, true);
            this.uiPM.AddFloatingPanelInfo(new System.Guid("b162c53a-7940-47cd-a0ed-9762c59c50d8"), Janus.Windows.UI.Dock.PanelGroupStyle.HorizontalTiles, true, new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
            this.uiPM.AddFloatingPanelInfo(new System.Guid("8e3140b9-bfcb-43af-b84e-2ee6cddf51dc"), new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
            this.uiPM.AddFloatingPanelInfo(new System.Guid("b2973a57-d3e1-4d80-9387-7a6dc3332542"), new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
            this.uiPM.AddFloatingPanelInfo(new System.Guid("afb3b7d0-aa79-443c-a1e8-3baba38f7a4d"), new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
            this.uiPM.AddFloatingPanelInfo(new System.Guid("88f86479-98e4-43d4-bf03-12fd789fcfc5"), Janus.Windows.UI.Dock.PanelGroupStyle.VerticalTiles, true, new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
            this.uiPM.AddFloatingPanelInfo(new System.Guid("91eeb7fc-856c-4a9c-b37e-db742cfa9b2e"), new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
            this.uiPM.AddFloatingPanelInfo(new System.Guid("3d64f731-de32-42ba-a10f-ac3b5ac56703"), new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
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
            this.uiPanelUsers.Text = "Slot±¸¼º°ü¸®";
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
            this.uiPanelUsersSearch.TabIndex = 0;
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
            this.pnlSearch.Controls.Add(this.chkUseYn);
            this.pnlSearch.Controls.Add(this.cbSearchMediaName);
            this.pnlSearch.Controls.Add(this.cbSearchCategoryName);
            this.pnlSearch.Controls.Add(this.cbSearchGenreName);
            this.pnlSearch.Controls.Add(this.btnSearch);
            this.pnlSearch.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlSearch.Location = new System.Drawing.Point(0, 0);
            this.pnlSearch.Name = "pnlSearch";
            this.pnlSearch.Size = new System.Drawing.Size(1008, 41);
            this.pnlSearch.TabIndex = 0;
            // 
            // chkUseYn
            // 
            this.chkUseYn.BackColor = System.Drawing.SystemColors.Window;
            this.chkUseYn.Location = new System.Drawing.Point(632, 14);
            this.chkUseYn.Name = "chkUseYn";
            this.chkUseYn.Size = new System.Drawing.Size(104, 16);
            this.chkUseYn.TabIndex = 31;
            this.chkUseYn.Text = "»ç¿ë¾ÈÇÔ Æ÷ÇÔ";
            this.chkUseYn.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            // 
            // cbSearchMediaName
            // 
            this.cbSearchMediaName.ComboStyle = Janus.Windows.EditControls.ComboStyle.DropDownList;
            this.cbSearchMediaName.Font = new System.Drawing.Font("±¼¸²Ã¼", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.cbSearchMediaName.Location = new System.Drawing.Point(8, 11);
            this.cbSearchMediaName.Name = "cbSearchMediaName";
            this.cbSearchMediaName.Size = new System.Drawing.Size(152, 21);
            this.cbSearchMediaName.TabIndex = 1;
            this.cbSearchMediaName.Text = "¸ÅÃ¼¼±ÅÃ";
            this.cbSearchMediaName.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            // 
            // cbSearchCategoryName
            // 
            this.cbSearchCategoryName.ComboStyle = Janus.Windows.EditControls.ComboStyle.DropDownList;
            this.cbSearchCategoryName.Font = new System.Drawing.Font("±¼¸²Ã¼", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.cbSearchCategoryName.Location = new System.Drawing.Point(168, 11);
            this.cbSearchCategoryName.Name = "cbSearchCategoryName";
            this.cbSearchCategoryName.Size = new System.Drawing.Size(152, 21);
            this.cbSearchCategoryName.TabIndex = 2;
            this.cbSearchCategoryName.Text = "Ä«Å×°í¸®¼±ÅÃ";
            this.cbSearchCategoryName.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            // 
            // cbSearchGenreName
            // 
            this.cbSearchGenreName.ComboStyle = Janus.Windows.EditControls.ComboStyle.DropDownList;
            this.cbSearchGenreName.Font = new System.Drawing.Font("±¼¸²Ã¼", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.cbSearchGenreName.Location = new System.Drawing.Point(328, 11);
            this.cbSearchGenreName.Name = "cbSearchGenreName";
            this.cbSearchGenreName.Size = new System.Drawing.Size(152, 21);
            this.cbSearchGenreName.TabIndex = 3;
            this.cbSearchGenreName.Text = "Àå¸£¼±ÅÃ";
            this.cbSearchGenreName.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            // 
            // btnSearch
            // 
            this.btnSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSearch.Enabled = false;
            this.btnSearch.Location = new System.Drawing.Point(903, 8);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(96, 24);
            this.btnSearch.TabIndex = 5;
            this.btnSearch.Text = "Á¶ È¸";
            this.btnSearch.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // uiPanelChannelSet
            // 
            this.uiPanelChannelSet.CaptionStyle = Janus.Windows.UI.Dock.PanelCaptionStyle.Dark;
            this.uiPanelChannelSet.GroupStyle = Janus.Windows.UI.Dock.PanelGroupStyle.VerticalTiles;
            this.uiPanelChannelSet.Location = new System.Drawing.Point(0, 69);
            this.uiPanelChannelSet.Name = "uiPanelChannelSet";
            this.uiPanelChannelSet.Size = new System.Drawing.Size(1010, 362);
            this.uiPanelChannelSet.TabIndex = 0;
            this.uiPanelChannelSet.Text = "Slot±¸¼º";
            // 
            // uiPanel2
            // 
            this.uiPanel2.InnerContainer = this.uiPanel2Container;
            this.uiPanel2.Location = new System.Drawing.Point(0, 22);
            this.uiPanel2.Name = "uiPanel2";
            this.uiPanel2.Size = new System.Drawing.Size(1010, 340);
            this.uiPanel2.TabIndex = 0;
            this.uiPanel2.Text = "Slot";
            // 
            // uiPanel2Container
            // 
            this.uiPanel2Container.Controls.Add(this.grdExChannelSetList);
            this.uiPanel2Container.Location = new System.Drawing.Point(1, 23);
            this.uiPanel2Container.Name = "uiPanel2Container";
            this.uiPanel2Container.Size = new System.Drawing.Size(1008, 316);
            this.uiPanel2Container.TabIndex = 0;
            // 
            // grdExChannelSetList
            // 
            this.grdExChannelSetList.AllowEdit = Janus.Windows.GridEX.InheritableBoolean.False;
            this.grdExChannelSetList.AlternatingColors = true;
            this.grdExChannelSetList.AutomaticSort = false;
            this.grdExChannelSetList.BorderStyle = Janus.Windows.GridEX.BorderStyle.None;
            this.grdExChannelSetList.DataSource = this.dvSlot;
            grdExChannelSetList_DesignTimeLayout.LayoutString = resources.GetString("grdExChannelSetList_DesignTimeLayout.LayoutString");
            this.grdExChannelSetList.DesignTimeLayout = grdExChannelSetList_DesignTimeLayout;
            this.grdExChannelSetList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdExChannelSetList.EditorsControlStyle.ButtonAppearance = Janus.Windows.GridEX.ButtonAppearance.Regular;
            this.grdExChannelSetList.EmptyRows = true;
            this.grdExChannelSetList.FocusCellFormatStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.grdExChannelSetList.FocusStyle = Janus.Windows.GridEX.FocusStyle.None;
            this.grdExChannelSetList.Font = new System.Drawing.Font("±¼¸²Ã¼", 9F);
            this.grdExChannelSetList.FrozenColumns = 2;
            this.grdExChannelSetList.GridLineColor = System.Drawing.Color.Silver;
            this.grdExChannelSetList.GridLines = Janus.Windows.GridEX.GridLines.Vertical;
            this.grdExChannelSetList.GridLineStyle = Janus.Windows.GridEX.GridLineStyle.Solid;
            this.grdExChannelSetList.GroupByBoxVisible = false;
            this.grdExChannelSetList.HeaderFormatStyle.TextAlignment = Janus.Windows.GridEX.TextAlignment.Center;
            this.grdExChannelSetList.HideSelection = Janus.Windows.GridEX.HideSelection.HighlightInactive;
            this.grdExChannelSetList.Location = new System.Drawing.Point(0, 0);
            this.grdExChannelSetList.Name = "grdExChannelSetList";
            this.grdExChannelSetList.SelectedFormatStyle.BackColor = System.Drawing.Color.Empty;
            this.grdExChannelSetList.SelectedFormatStyle.ForeColor = System.Drawing.Color.Empty;
            this.grdExChannelSetList.SelectedInactiveFormatStyle.BackColor = System.Drawing.Color.Gold;
            this.grdExChannelSetList.Size = new System.Drawing.Size(1008, 316);
            this.grdExChannelSetList.TabIndex = 7;
            this.grdExChannelSetList.TabKeyBehavior = Janus.Windows.GridEX.TabKeyBehavior.ControlNavigation;
            this.grdExChannelSetList.ThemedAreas = ((Janus.Windows.GridEX.ThemedArea)(((((((((Janus.Windows.GridEX.ThemedArea.ScrollBars | Janus.Windows.GridEX.ThemedArea.EditControls) 
            | Janus.Windows.GridEX.ThemedArea.Headers) 
            | Janus.Windows.GridEX.ThemedArea.GroupByBox) 
            | Janus.Windows.GridEX.ThemedArea.GroupRows) 
            | Janus.Windows.GridEX.ThemedArea.ControlBorder) 
            | Janus.Windows.GridEX.ThemedArea.Cards) 
            | Janus.Windows.GridEX.ThemedArea.Gridlines) 
            | Janus.Windows.GridEX.ThemedArea.CheckBoxes)));
            this.grdExChannelSetList.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            this.grdExChannelSetList.Enter += new System.EventHandler(this.OnGrdRowDetailChanged);
            // 
            // dvSlot
            // 
            this.dvSlot.Table = this.slotOrganizationDs.SlotOrganization;
            // 
            // slotOrganizationDs
            // 
            this.slotOrganizationDs.DataSetName = "SlotOrganizationDs";
            this.slotOrganizationDs.Locale = new System.Globalization.CultureInfo("en-US");
            this.slotOrganizationDs.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // uiPanelUserDetail
            // 
            this.uiPanelUserDetail.BackColor = System.Drawing.SystemColors.Window;
            this.uiPanelUserDetail.CaptionStyle = Janus.Windows.UI.Dock.PanelCaptionStyle.Dark;
            this.uiPanelUserDetail.InnerContainer = this.uiPanelUserDetailContainer;
            this.uiPanelUserDetail.Location = new System.Drawing.Point(0, 435);
            this.uiPanelUserDetail.Name = "uiPanelUserDetail";
            this.uiPanelUserDetail.Size = new System.Drawing.Size(1010, 242);
            this.uiPanelUserDetail.TabIndex = 0;
            this.uiPanelUserDetail.Text = "»ó¼¼Á¤º¸";
            // 
            // uiPanelUserDetailContainer
            // 
            this.uiPanelUserDetailContainer.Controls.Add(this.pnlUserDetail);
            this.uiPanelUserDetailContainer.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.uiPanelUserDetailContainer.Location = new System.Drawing.Point(1, 23);
            this.uiPanelUserDetailContainer.Name = "uiPanelUserDetailContainer";
            this.uiPanelUserDetailContainer.Size = new System.Drawing.Size(1008, 218);
            this.uiPanelUserDetailContainer.TabIndex = 0;
            // 
            // pnlUserDetail
            // 
            this.pnlUserDetail.BackColor = System.Drawing.SystemColors.Window;
            this.pnlUserDetail.Controls.Add(this.lbUseYn);
            this.pnlUserDetail.Controls.Add(this.rbUseYn_N);
            this.pnlUserDetail.Controls.Add(this.rbUseYn_Y);
            this.pnlUserDetail.Controls.Add(this.ebRegID);
            this.pnlUserDetail.Controls.Add(this.lbRegID);
            this.pnlUserDetail.Controls.Add(this.ebRegDt);
            this.pnlUserDetail.Controls.Add(this.lbRegDt);
            this.pnlUserDetail.Controls.Add(this.gpSlot);
            this.pnlUserDetail.Controls.Add(this.btnChannelNo);
            this.pnlUserDetail.Controls.Add(this.btnGenreName);
            this.pnlUserDetail.Controls.Add(this.ebGenreName);
            this.pnlUserDetail.Controls.Add(this.ebModDt);
            this.pnlUserDetail.Controls.Add(this.lbModDt);
            this.pnlUserDetail.Controls.Add(this.cbCategoryName);
            this.pnlUserDetail.Controls.Add(this.ebChannelNo);
            this.pnlUserDetail.Controls.Add(this.lbChannelNo);
            this.pnlUserDetail.Controls.Add(this.btnDelete);
            this.pnlUserDetail.Controls.Add(this.btnAdd);
            this.pnlUserDetail.Controls.Add(this.btnSave);
            this.pnlUserDetail.Controls.Add(this.cbMediaName);
            this.pnlUserDetail.Controls.Add(this.lbMediaName);
            this.pnlUserDetail.Controls.Add(this.lbCategoryName);
            this.pnlUserDetail.Controls.Add(this.lbGenreName);
            this.pnlUserDetail.Controls.Add(this.label1);
            this.pnlUserDetail.Controls.Add(this.label2);
            this.pnlUserDetail.Controls.Add(this.label3);
            this.pnlUserDetail.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlUserDetail.Location = new System.Drawing.Point(0, 0);
            this.pnlUserDetail.Name = "pnlUserDetail";
            this.pnlUserDetail.Size = new System.Drawing.Size(1008, 218);
            this.pnlUserDetail.TabIndex = 0;
            // 
            // lbUseYn
            // 
            this.lbUseYn.Location = new System.Drawing.Point(584, 146);
            this.lbUseYn.Name = "lbUseYn";
            this.lbUseYn.Size = new System.Drawing.Size(72, 21);
            this.lbUseYn.TabIndex = 40;
            this.lbUseYn.Text = "»ç¿ë¿©ºÎ";
            this.lbUseYn.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // rbUseYn_N
            // 
            this.rbUseYn_N.BackColor = System.Drawing.Color.Transparent;
            this.rbUseYn_N.Location = new System.Drawing.Point(736, 144);
            this.rbUseYn_N.Name = "rbUseYn_N";
            this.rbUseYn_N.Size = new System.Drawing.Size(72, 23);
            this.rbUseYn_N.TabIndex = 39;
            this.rbUseYn_N.Text = "»ç¿ë¾ÈÇÔ";
            this.rbUseYn_N.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            // 
            // rbUseYn_Y
            // 
            this.rbUseYn_Y.BackColor = System.Drawing.Color.Transparent;
            this.rbUseYn_Y.Location = new System.Drawing.Point(656, 144);
            this.rbUseYn_Y.Name = "rbUseYn_Y";
            this.rbUseYn_Y.Size = new System.Drawing.Size(72, 23);
            this.rbUseYn_Y.TabIndex = 38;
            this.rbUseYn_Y.Text = "»ç¿ëÇÔ";
            this.rbUseYn_Y.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            // 
            // ebRegID
            // 
            this.ebRegID.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ebRegID.Font = new System.Drawing.Font("±¼¸²Ã¼", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.ebRegID.Location = new System.Drawing.Point(448, 144);
            this.ebRegID.Name = "ebRegID";
            this.ebRegID.ReadOnly = true;
            this.ebRegID.Size = new System.Drawing.Size(112, 21);
            this.ebRegID.TabIndex = 23;
            this.ebRegID.TabStop = false;
            this.ebRegID.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
            this.ebRegID.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            // 
            // lbRegID
            // 
            this.lbRegID.Font = new System.Drawing.Font("±¼¸²Ã¼", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lbRegID.Location = new System.Drawing.Point(400, 144);
            this.lbRegID.Name = "lbRegID";
            this.lbRegID.Size = new System.Drawing.Size(72, 21);
            this.lbRegID.TabIndex = 22;
            this.lbRegID.Text = "µî·ÏÀÚ";
            this.lbRegID.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ebRegDt
            // 
            this.ebRegDt.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ebRegDt.Font = new System.Drawing.Font("±¼¸²Ã¼", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.ebRegDt.Location = new System.Drawing.Point(80, 144);
            this.ebRegDt.Name = "ebRegDt";
            this.ebRegDt.ReadOnly = true;
            this.ebRegDt.Size = new System.Drawing.Size(120, 21);
            this.ebRegDt.TabIndex = 21;
            this.ebRegDt.TabStop = false;
            this.ebRegDt.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
            this.ebRegDt.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            // 
            // lbRegDt
            // 
            this.lbRegDt.Font = new System.Drawing.Font("±¼¸²Ã¼", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lbRegDt.Location = new System.Drawing.Point(16, 144);
            this.lbRegDt.Name = "lbRegDt";
            this.lbRegDt.Size = new System.Drawing.Size(72, 21);
            this.lbRegDt.TabIndex = 20;
            this.lbRegDt.Text = "µî·ÏÀÏÀÚ";
            this.lbRegDt.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // gpSlot
            // 
            this.gpSlot.Controls.Add(this.cbSlot10);
            this.gpSlot.Controls.Add(this.lbSlot10);
            this.gpSlot.Controls.Add(this.cbSlot9);
            this.gpSlot.Controls.Add(this.lbSlot9);
            this.gpSlot.Controls.Add(this.cbSlot8);
            this.gpSlot.Controls.Add(this.lbSlot8);
            this.gpSlot.Controls.Add(this.cbSlot7);
            this.gpSlot.Controls.Add(this.lbSlot7);
            this.gpSlot.Controls.Add(this.cbSlot6);
            this.gpSlot.Controls.Add(this.lbSlot6);
            this.gpSlot.Controls.Add(this.cbSlot5);
            this.gpSlot.Controls.Add(this.lbSlot5);
            this.gpSlot.Controls.Add(this.cbSlot4);
            this.gpSlot.Controls.Add(this.lbSlot4);
            this.gpSlot.Controls.Add(this.cbSlot3);
            this.gpSlot.Controls.Add(this.lbSlot3);
            this.gpSlot.Controls.Add(this.cbSlot2);
            this.gpSlot.Controls.Add(this.lbSlot2);
            this.gpSlot.Controls.Add(this.cbSlot1);
            this.gpSlot.Controls.Add(this.lbSlot1);
            this.gpSlot.Location = new System.Drawing.Point(8, 56);
            this.gpSlot.Name = "gpSlot";
            this.gpSlot.Size = new System.Drawing.Size(832, 80);
            this.gpSlot.TabIndex = 19;
            this.gpSlot.Text = "SlotÁ¤º¸";
            // 
            // cbSlot10
            // 
            this.cbSlot10.ComboStyle = Janus.Windows.EditControls.ComboStyle.DropDownList;
            this.cbSlot10.DisplayMember = "CategoryName";
            this.cbSlot10.Font = new System.Drawing.Font("±¼¸²Ã¼", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.cbSlot10.Location = new System.Drawing.Point(704, 48);
            this.cbSlot10.Name = "cbSlot10";
            this.cbSlot10.Size = new System.Drawing.Size(112, 21);
            this.cbSlot10.TabIndex = 38;
            this.cbSlot10.ValueMember = "CategoryCode";
            this.cbSlot10.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            // 
            // lbSlot10
            // 
            this.lbSlot10.Font = new System.Drawing.Font("±¼¸²Ã¼", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lbSlot10.Location = new System.Drawing.Point(656, 48);
            this.lbSlot10.Name = "lbSlot10";
            this.lbSlot10.Size = new System.Drawing.Size(72, 20);
            this.lbSlot10.TabIndex = 39;
            this.lbSlot10.Text = "Slot10";
            this.lbSlot10.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cbSlot9
            // 
            this.cbSlot9.ComboStyle = Janus.Windows.EditControls.ComboStyle.DropDownList;
            this.cbSlot9.DisplayMember = "CategoryName";
            this.cbSlot9.Font = new System.Drawing.Font("±¼¸²Ã¼", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.cbSlot9.Location = new System.Drawing.Point(536, 48);
            this.cbSlot9.Name = "cbSlot9";
            this.cbSlot9.Size = new System.Drawing.Size(112, 21);
            this.cbSlot9.TabIndex = 36;
            this.cbSlot9.ValueMember = "CategoryCode";
            this.cbSlot9.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            // 
            // lbSlot9
            // 
            this.lbSlot9.Font = new System.Drawing.Font("±¼¸²Ã¼", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lbSlot9.Location = new System.Drawing.Point(488, 48);
            this.lbSlot9.Name = "lbSlot9";
            this.lbSlot9.Size = new System.Drawing.Size(72, 20);
            this.lbSlot9.TabIndex = 37;
            this.lbSlot9.Text = "Slot9";
            this.lbSlot9.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cbSlot8
            // 
            this.cbSlot8.ComboStyle = Janus.Windows.EditControls.ComboStyle.DropDownList;
            this.cbSlot8.DisplayMember = "CategoryName";
            this.cbSlot8.Font = new System.Drawing.Font("±¼¸²Ã¼", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.cbSlot8.Location = new System.Drawing.Point(368, 48);
            this.cbSlot8.Name = "cbSlot8";
            this.cbSlot8.Size = new System.Drawing.Size(112, 21);
            this.cbSlot8.TabIndex = 34;
            this.cbSlot8.ValueMember = "CategoryCode";
            this.cbSlot8.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            // 
            // lbSlot8
            // 
            this.lbSlot8.Font = new System.Drawing.Font("±¼¸²Ã¼", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lbSlot8.Location = new System.Drawing.Point(328, 48);
            this.lbSlot8.Name = "lbSlot8";
            this.lbSlot8.Size = new System.Drawing.Size(72, 20);
            this.lbSlot8.TabIndex = 35;
            this.lbSlot8.Text = "Slot8";
            this.lbSlot8.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cbSlot7
            // 
            this.cbSlot7.ComboStyle = Janus.Windows.EditControls.ComboStyle.DropDownList;
            this.cbSlot7.DisplayMember = "CategoryName";
            this.cbSlot7.Font = new System.Drawing.Font("±¼¸²Ã¼", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.cbSlot7.Location = new System.Drawing.Point(208, 48);
            this.cbSlot7.Name = "cbSlot7";
            this.cbSlot7.Size = new System.Drawing.Size(112, 21);
            this.cbSlot7.TabIndex = 32;
            this.cbSlot7.ValueMember = "CategoryCode";
            this.cbSlot7.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            // 
            // lbSlot7
            // 
            this.lbSlot7.Font = new System.Drawing.Font("±¼¸²Ã¼", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lbSlot7.Location = new System.Drawing.Point(168, 48);
            this.lbSlot7.Name = "lbSlot7";
            this.lbSlot7.Size = new System.Drawing.Size(72, 20);
            this.lbSlot7.TabIndex = 33;
            this.lbSlot7.Text = "Slot7";
            this.lbSlot7.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cbSlot6
            // 
            this.cbSlot6.ComboStyle = Janus.Windows.EditControls.ComboStyle.DropDownList;
            this.cbSlot6.DisplayMember = "CategoryName";
            this.cbSlot6.Font = new System.Drawing.Font("±¼¸²Ã¼", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.cbSlot6.Location = new System.Drawing.Point(48, 48);
            this.cbSlot6.Name = "cbSlot6";
            this.cbSlot6.Size = new System.Drawing.Size(112, 21);
            this.cbSlot6.TabIndex = 30;
            this.cbSlot6.ValueMember = "CategoryCode";
            this.cbSlot6.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            // 
            // lbSlot6
            // 
            this.lbSlot6.Font = new System.Drawing.Font("±¼¸²Ã¼", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lbSlot6.Location = new System.Drawing.Point(8, 48);
            this.lbSlot6.Name = "lbSlot6";
            this.lbSlot6.Size = new System.Drawing.Size(72, 20);
            this.lbSlot6.TabIndex = 31;
            this.lbSlot6.Text = "Slot6";
            this.lbSlot6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cbSlot5
            // 
            this.cbSlot5.ComboStyle = Janus.Windows.EditControls.ComboStyle.DropDownList;
            this.cbSlot5.DisplayMember = "CategoryName";
            this.cbSlot5.Font = new System.Drawing.Font("±¼¸²Ã¼", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.cbSlot5.Location = new System.Drawing.Point(704, 16);
            this.cbSlot5.Name = "cbSlot5";
            this.cbSlot5.Size = new System.Drawing.Size(112, 21);
            this.cbSlot5.TabIndex = 28;
            this.cbSlot5.ValueMember = "CategoryCode";
            this.cbSlot5.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            // 
            // lbSlot5
            // 
            this.lbSlot5.Font = new System.Drawing.Font("±¼¸²Ã¼", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lbSlot5.Location = new System.Drawing.Point(656, 16);
            this.lbSlot5.Name = "lbSlot5";
            this.lbSlot5.Size = new System.Drawing.Size(72, 20);
            this.lbSlot5.TabIndex = 29;
            this.lbSlot5.Text = "Slot5";
            this.lbSlot5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cbSlot4
            // 
            this.cbSlot4.ComboStyle = Janus.Windows.EditControls.ComboStyle.DropDownList;
            this.cbSlot4.DisplayMember = "CategoryName";
            this.cbSlot4.Font = new System.Drawing.Font("±¼¸²Ã¼", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.cbSlot4.Location = new System.Drawing.Point(536, 16);
            this.cbSlot4.Name = "cbSlot4";
            this.cbSlot4.Size = new System.Drawing.Size(112, 21);
            this.cbSlot4.TabIndex = 26;
            this.cbSlot4.ValueMember = "CategoryCode";
            this.cbSlot4.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            // 
            // lbSlot4
            // 
            this.lbSlot4.Font = new System.Drawing.Font("±¼¸²Ã¼", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lbSlot4.Location = new System.Drawing.Point(488, 16);
            this.lbSlot4.Name = "lbSlot4";
            this.lbSlot4.Size = new System.Drawing.Size(72, 20);
            this.lbSlot4.TabIndex = 27;
            this.lbSlot4.Text = "Slot4";
            this.lbSlot4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cbSlot3
            // 
            this.cbSlot3.ComboStyle = Janus.Windows.EditControls.ComboStyle.DropDownList;
            this.cbSlot3.DisplayMember = "CategoryName";
            this.cbSlot3.Font = new System.Drawing.Font("±¼¸²Ã¼", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.cbSlot3.Location = new System.Drawing.Point(368, 16);
            this.cbSlot3.Name = "cbSlot3";
            this.cbSlot3.Size = new System.Drawing.Size(112, 21);
            this.cbSlot3.TabIndex = 24;
            this.cbSlot3.ValueMember = "CategoryCode";
            this.cbSlot3.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            // 
            // lbSlot3
            // 
            this.lbSlot3.Font = new System.Drawing.Font("±¼¸²Ã¼", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lbSlot3.Location = new System.Drawing.Point(328, 16);
            this.lbSlot3.Name = "lbSlot3";
            this.lbSlot3.Size = new System.Drawing.Size(72, 20);
            this.lbSlot3.TabIndex = 25;
            this.lbSlot3.Text = "Slot3";
            this.lbSlot3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cbSlot2
            // 
            this.cbSlot2.ComboStyle = Janus.Windows.EditControls.ComboStyle.DropDownList;
            this.cbSlot2.DisplayMember = "CategoryName";
            this.cbSlot2.Font = new System.Drawing.Font("±¼¸²Ã¼", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.cbSlot2.Location = new System.Drawing.Point(208, 16);
            this.cbSlot2.Name = "cbSlot2";
            this.cbSlot2.Size = new System.Drawing.Size(112, 21);
            this.cbSlot2.TabIndex = 22;
            this.cbSlot2.ValueMember = "CategoryCode";
            this.cbSlot2.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            // 
            // lbSlot2
            // 
            this.lbSlot2.Font = new System.Drawing.Font("±¼¸²Ã¼", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lbSlot2.Location = new System.Drawing.Point(168, 16);
            this.lbSlot2.Name = "lbSlot2";
            this.lbSlot2.Size = new System.Drawing.Size(72, 20);
            this.lbSlot2.TabIndex = 23;
            this.lbSlot2.Text = "Slot2";
            this.lbSlot2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cbSlot1
            // 
            this.cbSlot1.ComboStyle = Janus.Windows.EditControls.ComboStyle.DropDownList;
            this.cbSlot1.DisplayMember = "CategoryName";
            this.cbSlot1.Font = new System.Drawing.Font("±¼¸²Ã¼", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.cbSlot1.Location = new System.Drawing.Point(48, 16);
            this.cbSlot1.Name = "cbSlot1";
            this.cbSlot1.Size = new System.Drawing.Size(112, 21);
            this.cbSlot1.TabIndex = 20;
            this.cbSlot1.ValueMember = "CategoryCode";
            this.cbSlot1.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            // 
            // lbSlot1
            // 
            this.lbSlot1.Font = new System.Drawing.Font("±¼¸²Ã¼", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lbSlot1.Location = new System.Drawing.Point(8, 16);
            this.lbSlot1.Name = "lbSlot1";
            this.lbSlot1.Size = new System.Drawing.Size(72, 20);
            this.lbSlot1.TabIndex = 21;
            this.lbSlot1.Text = "Slot1";
            this.lbSlot1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnChannelNo
            // 
            this.btnChannelNo.Location = new System.Drawing.Point(624, 40);
            this.btnChannelNo.Name = "btnChannelNo";
            this.btnChannelNo.Size = new System.Drawing.Size(104, 24);
            this.btnChannelNo.TabIndex = 10;
            this.btnChannelNo.Text = "Ã¤³Î¼±ÅÃ";
            this.btnChannelNo.Visible = false;
            this.btnChannelNo.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.btnChannelNo.Click += new System.EventHandler(this.btnChannelNo_Click);
            // 
            // btnGenreName
            // 
            this.btnGenreName.Location = new System.Drawing.Point(320, 32);
            this.btnGenreName.Name = "btnGenreName";
            this.btnGenreName.Size = new System.Drawing.Size(104, 22);
            this.btnGenreName.TabIndex = 11;
            this.btnGenreName.Text = "Àå¸£¼±ÅÃ";
            this.btnGenreName.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.btnGenreName.Click += new System.EventHandler(this.btnGenreName_Click);
            // 
            // ebGenreName
            // 
            this.ebGenreName.Font = new System.Drawing.Font("±¼¸²Ã¼", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.ebGenreName.Location = new System.Drawing.Point(320, 8);
            this.ebGenreName.MaxLength = 120;
            this.ebGenreName.Name = "ebGenreName";
            this.ebGenreName.Size = new System.Drawing.Size(280, 21);
            this.ebGenreName.TabIndex = 10;
            this.ebGenreName.TabStop = false;
            this.ebGenreName.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
            this.ebGenreName.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            // 
            // ebModDt
            // 
            this.ebModDt.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ebModDt.Font = new System.Drawing.Font("±¼¸²Ã¼", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.ebModDt.Location = new System.Drawing.Point(272, 144);
            this.ebModDt.Name = "ebModDt";
            this.ebModDt.ReadOnly = true;
            this.ebModDt.Size = new System.Drawing.Size(120, 21);
            this.ebModDt.TabIndex = 18;
            this.ebModDt.TabStop = false;
            this.ebModDt.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
            this.ebModDt.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            // 
            // lbModDt
            // 
            this.lbModDt.Font = new System.Drawing.Font("±¼¸²Ã¼", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lbModDt.Location = new System.Drawing.Point(208, 144);
            this.lbModDt.Name = "lbModDt";
            this.lbModDt.Size = new System.Drawing.Size(72, 21);
            this.lbModDt.TabIndex = 0;
            this.lbModDt.Text = "¼öÁ¤ÀÏÀÚ";
            this.lbModDt.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cbCategoryName
            // 
            this.cbCategoryName.ComboStyle = Janus.Windows.EditControls.ComboStyle.DropDownList;
            this.cbCategoryName.DisplayMember = "CategoryName";
            this.cbCategoryName.Font = new System.Drawing.Font("±¼¸²Ã¼", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.cbCategoryName.Location = new System.Drawing.Point(80, 32);
            this.cbCategoryName.Name = "cbCategoryName";
            this.cbCategoryName.Size = new System.Drawing.Size(152, 21);
            this.cbCategoryName.TabIndex = 9;
            this.cbCategoryName.ValueMember = "CategoryCode";
            this.cbCategoryName.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.cbCategoryName.SelectedItemChanged += new System.EventHandler(this.cbCategoryName_SelectedItemChanged);
            // 
            // ebChannelNo
            // 
            this.ebChannelNo.Font = new System.Drawing.Font("±¼¸²Ã¼", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.ebChannelNo.Location = new System.Drawing.Point(512, 40);
            this.ebChannelNo.MaxLength = 120;
            this.ebChannelNo.Name = "ebChannelNo";
            this.ebChannelNo.Size = new System.Drawing.Size(104, 21);
            this.ebChannelNo.TabIndex = 12;
            this.ebChannelNo.TabStop = false;
            this.ebChannelNo.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
            this.ebChannelNo.Visible = false;
            this.ebChannelNo.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            // 
            // lbChannelNo
            // 
            this.lbChannelNo.Font = new System.Drawing.Font("±¼¸²Ã¼", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lbChannelNo.Location = new System.Drawing.Point(440, 40);
            this.lbChannelNo.Name = "lbChannelNo";
            this.lbChannelNo.Size = new System.Drawing.Size(72, 21);
            this.lbChannelNo.TabIndex = 0;
            this.lbChannelNo.Text = "Ã¤³Î¹øÈ£ ";
            this.lbChannelNo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lbChannelNo.Visible = false;
            // 
            // btnDelete
            // 
            this.btnDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnDelete.BackColor = System.Drawing.SystemColors.Window;
            this.btnDelete.Enabled = false;
            this.btnDelete.Location = new System.Drawing.Point(128, 182);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(104, 24);
            this.btnDelete.TabIndex = 15;
            this.btnDelete.Text = "»è Á¦";
            this.btnDelete.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnAdd
            // 
            this.btnAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnAdd.BackColor = System.Drawing.SystemColors.Window;
            this.btnAdd.Enabled = false;
            this.btnAdd.Location = new System.Drawing.Point(240, 182);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(104, 24);
            this.btnAdd.TabIndex = 16;
            this.btnAdd.Text = "Ãß °¡";
            this.btnAdd.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnSave.BackColor = System.Drawing.SystemColors.Window;
            this.btnSave.Enabled = false;
            this.btnSave.Location = new System.Drawing.Point(16, 182);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(104, 24);
            this.btnSave.TabIndex = 14;
            this.btnSave.Text = "Àú Àå";
            this.btnSave.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // cbMediaName
            // 
            this.cbMediaName.ComboStyle = Janus.Windows.EditControls.ComboStyle.DropDownList;
            this.cbMediaName.DisplayMember = "MediaName";
            this.cbMediaName.Font = new System.Drawing.Font("±¼¸²Ã¼", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.cbMediaName.Location = new System.Drawing.Point(80, 8);
            this.cbMediaName.Name = "cbMediaName";
            this.cbMediaName.Size = new System.Drawing.Size(152, 21);
            this.cbMediaName.TabIndex = 8;
            this.cbMediaName.ValueMember = "MediaCode";
            this.cbMediaName.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            // 
            // lbMediaName
            // 
            this.lbMediaName.Font = new System.Drawing.Font("±¼¸²Ã¼", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lbMediaName.Location = new System.Drawing.Point(8, 8);
            this.lbMediaName.Name = "lbMediaName";
            this.lbMediaName.Size = new System.Drawing.Size(72, 21);
            this.lbMediaName.TabIndex = 12;
            this.lbMediaName.Text = "¸ÅÃ¼";
            this.lbMediaName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lbCategoryName
            // 
            this.lbCategoryName.Font = new System.Drawing.Font("±¼¸²Ã¼", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lbCategoryName.Location = new System.Drawing.Point(8, 32);
            this.lbCategoryName.Name = "lbCategoryName";
            this.lbCategoryName.Size = new System.Drawing.Size(72, 20);
            this.lbCategoryName.TabIndex = 14;
            this.lbCategoryName.Text = "Ä«Å×°í¸®";
            this.lbCategoryName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lbGenreName
            // 
            this.lbGenreName.Font = new System.Drawing.Font("±¼¸²Ã¼", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lbGenreName.Location = new System.Drawing.Point(248, 8);
            this.lbGenreName.Name = "lbGenreName";
            this.lbGenreName.Size = new System.Drawing.Size(72, 21);
            this.lbGenreName.TabIndex = 15;
            this.lbGenreName.Text = "Àå¸£";
            this.lbGenreName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label1
            // 
            this.label1.Image = ((System.Drawing.Image)(resources.GetObject("label1.Image")));
            this.label1.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label1.Location = new System.Drawing.Point(672, 176);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(56, 21);
            this.label1.TabIndex = 40;
            this.label1.Text = "ÇÊ¼ö";
            this.label1.TextAlign = System.Drawing.ContentAlignment.BottomRight;
            // 
            // label2
            // 
            this.label2.Image = ((System.Drawing.Image)(resources.GetObject("label2.Image")));
            this.label2.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label2.Location = new System.Drawing.Point(600, 176);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(56, 21);
            this.label2.TabIndex = 40;
            this.label2.Text = "µ¶Á¡";
            this.label2.TextAlign = System.Drawing.ContentAlignment.BottomRight;
            // 
            // label3
            // 
            this.label3.Image = ((System.Drawing.Image)(resources.GetObject("label3.Image")));
            this.label3.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label3.Location = new System.Drawing.Point(744, 176);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(56, 21);
            this.label3.TabIndex = 40;
            this.label3.Text = "¿É¼Ç";
            this.label3.TextAlign = System.Drawing.ContentAlignment.BottomRight;
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
            // SlotOrganizationControl
            // 
            this.Controls.Add(this.uiPanelUsers);
            this.Name = "SlotOrganizationControl";
            this.Size = new System.Drawing.Size(1010, 677);
            this.Load += new System.EventHandler(this.SlotOrganizationControl_Load);
            ((System.ComponentModel.ISupportInitialize)(this.uiPM)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelUsers)).EndInit();
            this.uiPanelUsers.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelUsersSearch)).EndInit();
            this.uiPanelUsersSearch.ResumeLayout(false);
            this.uiPanelUsersSearchContainer.ResumeLayout(false);
            this.pnlSearch.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelChannelSet)).EndInit();
            this.uiPanelChannelSet.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.uiPanel2)).EndInit();
            this.uiPanel2.ResumeLayout(false);
            this.uiPanel2Container.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdExChannelSetList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dvSlot)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.slotOrganizationDs)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelUserDetail)).EndInit();
            this.uiPanelUserDetail.ResumeLayout(false);
            this.uiPanelUserDetailContainer.ResumeLayout(false);
            this.pnlUserDetail.ResumeLayout(false);
            this.pnlUserDetail.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gpSlot)).EndInit();
            this.gpSlot.ResumeLayout(false);
            this.ResumeLayout(false);

		}
		#endregion

		#region ÄÁÆ®·Ñ ·Îµå
		private void SlotOrganizationControl_Load(object sender, System.EventArgs e)
		{		
			// µ¥ÀÌÅÍ°ü¸®¿ë °´Ã¼»ý¼º
			dt = ((DataView)grdExChannelSetList.DataSource).Table;
			
			cm = (CurrencyManager) this.BindingContext[grdExChannelSetList.DataSource]; 
			cm.PositionChanged += new System.EventHandler(OnGrdRowDetailChanged); 		

			// ÄÁÆ®·Ñ ÃÊ±âÈ­
			InitControl();			
		}

		#endregion

		#region ÄÁÆ®·Ñ ÃÊ±âÈ­
		private void InitControl()
		{
			ProgressStart();
			InitCombo();
			InitCombo_Category();
			InitCombo_Genre();
			InitCombo_Level();
			//===============Slot_Combo=============
			InitCombo_Media();
			InitCombo_CategoryDetail();
			InitCombo_Slot1();
			InitCombo_Slot2();
			InitCombo_Slot3();
			InitCombo_Slot4();
			InitCombo_Slot5();
			InitCombo_Slot6();
			InitCombo_Slot7();
			InitCombo_Slot8();
			InitCombo_Slot9();
			InitCombo_Slot10();

			// Á¶È¸±ÇÇÑ °Ë»ç
			if(menu.CanRead(MenuCode))
			{
				canRead = true;
				SearchSlotOrganization();
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
			MediaCodeModel mediacodeModel = new MediaCodeModel();		
			new MediaCodeManager(systemModel, commonModel).GetMediaCodeList(mediacodeModel);
			
			if (mediacodeModel.ResultCD.Equals("0000"))
			{
				// µ¥ÀÌÅÍ¼Â¿¡ ¼ÂÆÃ
				Utility.SetDataTable(slotOrganizationDs.Medias, mediacodeModel.MediaCodeDataSet);				
			}

			// »ó¼¼Á¶È¸ ÄÞº¸
			// »ó¼¼Á¤º¸ÀÇ ÄÞº¸´Â DatasetÀ» µ¥ÀÌÅÍ¼Ò½º·Î °¡Áø´Ù.

			// °Ë»öÁ¶°ÇÀÇ ÄÞº¸
			this.cbSearchMediaName.Items.Clear();
			
			// ÄÞº¸¹Ú½º¿¡ ¼ÂÆ®ÇÒ ÄÚµå¸ñ·ÏÀ» ´ãÀ» Item¹è¿­À» ¼±¾ð
			Janus.Windows.EditControls.UIComboBoxItem[] comboItems = new Janus.Windows.EditControls.UIComboBoxItem[mediacodeModel.ResultCnt + 1];

			comboItems[0] = new Janus.Windows.EditControls.UIComboBoxItem("¸ÅÃ¼¼±ÅÃ","00");
			
			for(int i=0;i<mediacodeModel.ResultCnt;i++)
			{
				DataRow row = slotOrganizationDs.Medias.Rows[i];

				string val = row["MediaCode"].ToString();
				string txt = row["MediaName"].ToString();
				comboItems[i+1] = new Janus.Windows.EditControls.UIComboBoxItem(txt,val);
			}
			// ÄÞº¸¿¡ ¼ÂÆ®
			this.cbSearchMediaName.Items.AddRange(comboItems);
			this.cbSearchMediaName.SelectedIndex = 0;

			Application.DoEvents();
		}

		private void InitCombo_Media()
		{			
			MediaCodeModel mediacodeModel = new MediaCodeModel();		
			new MediaCodeManager(systemModel, commonModel).GetMediaCodeList(mediacodeModel);
			
			if (mediacodeModel.ResultCD.Equals("0000"))
			{
				// µ¥ÀÌÅÍ¼Â¿¡ ¼ÂÆÃ
				Utility.SetDataTable(slotOrganizationDs.Medias, mediacodeModel.MediaCodeDataSet);				
			}

			// »ó¼¼Á¶È¸ ÄÞº¸
			// »ó¼¼Á¤º¸ÀÇ ÄÞº¸´Â DatasetÀ» µ¥ÀÌÅÍ¼Ò½º·Î °¡Áø´Ù.

			// °Ë»öÁ¶°ÇÀÇ ÄÞº¸
			this.cbMediaName.Items.Clear();
			
			// ÄÞº¸¹Ú½º¿¡ ¼ÂÆ®ÇÒ ÄÚµå¸ñ·ÏÀ» ´ãÀ» Item¹è¿­À» ¼±¾ð
			Janus.Windows.EditControls.UIComboBoxItem[] comboItems = new Janus.Windows.EditControls.UIComboBoxItem[mediacodeModel.ResultCnt + 1];

			comboItems[0] = new Janus.Windows.EditControls.UIComboBoxItem("¸ÅÃ¼¼±ÅÃ","00");
			
			for(int i=0;i<mediacodeModel.ResultCnt;i++)
			{
				DataRow row = slotOrganizationDs.Medias.Rows[i];

				string val = row["MediaCode"].ToString();
				string txt = row["MediaName"].ToString();
				comboItems[i+1] = new Janus.Windows.EditControls.UIComboBoxItem(txt,val);
			}
			// ÄÞº¸¿¡ ¼ÂÆ®
			this.cbMediaName.Items.AddRange(comboItems);
			this.cbMediaName.SelectedIndex = 0;

			Application.DoEvents();
		}
		
		public void InitCombo_Category()
		{
			// ÄÚµå¿¡¼­ º¸¾È·¹º§À» Á¶È¸ÇÑ´Ù.
			SlotOrganizationModel slotOrganizationModel = new SlotOrganizationModel();
			new SlotOrganizationManager(systemModel, commonModel).GetCategoryList(slotOrganizationModel);
			
			if (slotOrganizationModel.ResultCD.Equals("0000"))
			{
				// µ¥ÀÌÅÍ¼Â¿¡ ¼ÂÆÃ
				Utility.SetDataTable(slotOrganizationDs.Categorys, slotOrganizationModel.CategoryDataSet);	
			}

			// ÇÏ´Ü ¸ÅÃ¼ ÄÞº¸ÀÇ °ª ÃÊ±âÈ­. 
			this.cbSearchCategoryName.Items.Clear();
            
			// ÄÞº¸¹Ú½º¿¡ ¼ÂÆ®ÇÒ ÄÚµå¸ñ·ÏÀ» ´ãÀ» Item¹è¿­À» ¼±¾ð
			Janus.Windows.EditControls.UIComboBoxItem[] comboItems = new Janus.Windows.EditControls.UIComboBoxItem[slotOrganizationModel.ResultCnt + 1];
            
			comboItems[0] = new Janus.Windows.EditControls.UIComboBoxItem("Ä«Å×°í¸®¼±ÅÃ","00");
			
			for(int i=0;i<slotOrganizationModel.ResultCnt;i++)
			{
				DataRow row = slotOrganizationDs.Categorys.Rows[i];

				string val = row["CategoryCode"].ToString();
				string txt = row["CategoryName"].ToString();
				comboItems[i+1] = new Janus.Windows.EditControls.UIComboBoxItem(txt,val);
			}


			// ÇÏ´Ü ÄÞº¸¿¡ ¼ÂÆ®
			this.cbSearchCategoryName.Items.AddRange(comboItems);
			this.cbSearchCategoryName.SelectedIndex = 0;

			Application.DoEvents();
		}

		public void InitCombo_CategoryDetail()
		{
			// ÄÚµå¿¡¼­ º¸¾È·¹º§À» Á¶È¸ÇÑ´Ù.
			SlotOrganizationModel slotOrganizationModel = new SlotOrganizationModel();
			new SlotOrganizationManager(systemModel, commonModel).GetCategoryList(slotOrganizationModel);
			
			if (slotOrganizationModel.ResultCD.Equals("0000"))
			{
				// µ¥ÀÌÅÍ¼Â¿¡ ¼ÂÆÃ
				Utility.SetDataTable(slotOrganizationDs.Categorys, slotOrganizationModel.CategoryDataSet);	
			}

			// ÇÏ´Ü ¸ÅÃ¼ ÄÞº¸ÀÇ °ª ÃÊ±âÈ­. 
			this.cbCategoryName.Items.Clear();
            
			// ÄÞº¸¹Ú½º¿¡ ¼ÂÆ®ÇÒ ÄÚµå¸ñ·ÏÀ» ´ãÀ» Item¹è¿­À» ¼±¾ð
			Janus.Windows.EditControls.UIComboBoxItem[] comboItems = new Janus.Windows.EditControls.UIComboBoxItem[slotOrganizationModel.ResultCnt + 1];
            
			comboItems[0] = new Janus.Windows.EditControls.UIComboBoxItem("Ä«Å×°í¸®¼±ÅÃ","00");
			
			for(int i=0;i<slotOrganizationModel.ResultCnt;i++)
			{
				DataRow row = slotOrganizationDs.Categorys.Rows[i];

				string val = row["CategoryCode"].ToString();
				string txt = row["CategoryName"].ToString();
				comboItems[i+1] = new Janus.Windows.EditControls.UIComboBoxItem(txt,val);
			}


			// ÇÏ´Ü ÄÞº¸¿¡ ¼ÂÆ®
			this.cbCategoryName.Items.AddRange(comboItems);
			this.cbCategoryName.SelectedIndex = 0;

			Application.DoEvents();
		}

		public void InitCombo_Genre()
		{
			// ÄÚµå¿¡¼­ º¸¾È·¹º§À» Á¶È¸ÇÑ´Ù.
			SlotOrganizationModel slotOrganizationModel = new SlotOrganizationModel();
			new SlotOrganizationManager(systemModel, commonModel).GetGenreList(slotOrganizationModel);
			
			if (slotOrganizationModel.ResultCD.Equals("0000"))
			{
				// µ¥ÀÌÅÍ¼Â¿¡ ¼ÂÆÃ
				Utility.SetDataTable(slotOrganizationDs.Genres, slotOrganizationModel.GenreDataSet);	
			}

			// ÇÏ´Ü ¸ÅÃ¼ ÄÞº¸ÀÇ °ª ÃÊ±âÈ­. 
			this.cbSearchGenreName.Items.Clear();
            
			// ÄÞº¸¹Ú½º¿¡ ¼ÂÆ®ÇÒ ÄÚµå¸ñ·ÏÀ» ´ãÀ» Item¹è¿­À» ¼±¾ð
			Janus.Windows.EditControls.UIComboBoxItem[] comboItems = new Janus.Windows.EditControls.UIComboBoxItem[slotOrganizationModel.ResultCnt + 1];
            
			comboItems[0] = new Janus.Windows.EditControls.UIComboBoxItem("Àå¸£¼±ÅÃ","00");
			
			for(int i=0;i<slotOrganizationModel.ResultCnt;i++)
			{
				DataRow row = slotOrganizationDs.Genres.Rows[i];

				string val = row["GenreCode"].ToString();
				string txt = row["GenreName"].ToString();
				comboItems[i+1] = new Janus.Windows.EditControls.UIComboBoxItem(txt,val);
			}


			// ÇÏ´Ü ÄÞº¸¿¡ ¼ÂÆ®
			this.cbSearchGenreName.Items.AddRange(comboItems);
			this.cbSearchGenreName.SelectedIndex = 0;

			Application.DoEvents();

		}        

		private void InitCombo_Level()
		{			
			if(commonModel.UserLevel=="20")
			{
				// ÄÞº¸ÇÈ½º						
				cbSearchMediaName.SelectedValue = commonModel.MediaCode;			
				cbSearchMediaName.ReadOnly = true;										
			}
			else
			{
				for(int i=0;i < slotOrganizationDs.Medias.Rows.Count;i++)
				{
					DataRow row = slotOrganizationDs.Medias.Rows[i];					
					if(row["MediaCode"].ToString().Equals(FrameSystem._HANATV.ToString()))
					{
						cbSearchMediaName.SelectedValue = FrameSystem._HANATV; // ÇÏ³ªTV¸¦ ±âº»°ªÀ¸·Î ÇÑ´Ù.	 		
						break;															
					}
					else
					{
						cbSearchMediaName.SelectedValue="00";
					}
				}	
			}
			Application.DoEvents();
		}

		private void InitCombo_Slot1()
		{			
			SlotOrganizationModel slotOrganizationModel = new SlotOrganizationModel();		
			new SlotOrganizationManager(systemModel, commonModel).GetSlotCodeList(slotOrganizationModel);
			
			if (slotOrganizationModel.ResultCD.Equals("0000"))
			{
				// µ¥ÀÌÅÍ¼Â¿¡ ¼ÂÆÃ
				Utility.SetDataTable(slotOrganizationDs.SlotCode, slotOrganizationModel.SlotCodeDataSet);				
			}

			// »ó¼¼Á¶È¸ ÄÞº¸
			// »ó¼¼Á¤º¸ÀÇ ÄÞº¸´Â DatasetÀ» µ¥ÀÌÅÍ¼Ò½º·Î °¡Áø´Ù.

			// °Ë»öÁ¶°ÇÀÇ ÄÞº¸
			this.cbSlot1.Items.Clear();
			
			// ÄÞº¸¹Ú½º¿¡ ¼ÂÆ®ÇÒ ÄÚµå¸ñ·ÏÀ» ´ãÀ» Item¹è¿­À» ¼±¾ð
			Janus.Windows.EditControls.UIComboBoxItem[] comboItems = new Janus.Windows.EditControls.UIComboBoxItem[slotOrganizationModel.ResultCnt + 1];

			comboItems[0] = new Janus.Windows.EditControls.UIComboBoxItem("½½·Ô¼±ÅÃ1","00");
			
			for(int i=0;i<slotOrganizationModel.ResultCnt;i++)
			{
				DataRow row = slotOrganizationDs.SlotCode.Rows[i];

				string val = row["SlotCode"].ToString();
				string txt = row["SlotName"].ToString();
				comboItems[i+1] = new Janus.Windows.EditControls.UIComboBoxItem(txt,val);
			}
			// ÄÞº¸¿¡ ¼ÂÆ®
			this.cbSlot1.Items.AddRange(comboItems);
			this.cbSlot1.SelectedIndex = 0;

			Application.DoEvents();
		}

		private void InitCombo_Slot2()
		{			
			SlotOrganizationModel slotOrganizationModel = new SlotOrganizationModel();		
			new SlotOrganizationManager(systemModel, commonModel).GetSlotCodeList(slotOrganizationModel);
			
			if (slotOrganizationModel.ResultCD.Equals("0000"))
			{
				// µ¥ÀÌÅÍ¼Â¿¡ ¼ÂÆÃ
				Utility.SetDataTable(slotOrganizationDs.SlotCode, slotOrganizationModel.SlotCodeDataSet);				
			}

			// »ó¼¼Á¶È¸ ÄÞº¸
			// »ó¼¼Á¤º¸ÀÇ ÄÞº¸´Â DatasetÀ» µ¥ÀÌÅÍ¼Ò½º·Î °¡Áø´Ù.

			// °Ë»öÁ¶°ÇÀÇ ÄÞº¸
			this.cbSlot2.Items.Clear();
			
			// ÄÞº¸¹Ú½º¿¡ ¼ÂÆ®ÇÒ ÄÚµå¸ñ·ÏÀ» ´ãÀ» Item¹è¿­À» ¼±¾ð
			Janus.Windows.EditControls.UIComboBoxItem[] comboItems = new Janus.Windows.EditControls.UIComboBoxItem[slotOrganizationModel.ResultCnt + 1];

			comboItems[0] = new Janus.Windows.EditControls.UIComboBoxItem("½½·Ô¼±ÅÃ2","00");
			
			for(int i=0;i<slotOrganizationModel.ResultCnt;i++)
			{
				DataRow row = slotOrganizationDs.SlotCode.Rows[i];

				string val = row["SlotCode"].ToString();
				string txt = row["SlotName"].ToString();
				comboItems[i+1] = new Janus.Windows.EditControls.UIComboBoxItem(txt,val);
			}
			// ÄÞº¸¿¡ ¼ÂÆ®
			this.cbSlot2.Items.AddRange(comboItems);
			this.cbSlot2.SelectedIndex = 0;

			Application.DoEvents();
		}

		private void InitCombo_Slot3()
		{			
			SlotOrganizationModel slotOrganizationModel = new SlotOrganizationModel();		
			new SlotOrganizationManager(systemModel, commonModel).GetSlotCodeList(slotOrganizationModel);
			
			if (slotOrganizationModel.ResultCD.Equals("0000"))
			{
				// µ¥ÀÌÅÍ¼Â¿¡ ¼ÂÆÃ
				Utility.SetDataTable(slotOrganizationDs.SlotCode, slotOrganizationModel.SlotCodeDataSet);				
			}

			// »ó¼¼Á¶È¸ ÄÞº¸
			// »ó¼¼Á¤º¸ÀÇ ÄÞº¸´Â DatasetÀ» µ¥ÀÌÅÍ¼Ò½º·Î °¡Áø´Ù.

			// °Ë»öÁ¶°ÇÀÇ ÄÞº¸
			this.cbSlot3.Items.Clear();
			
			// ÄÞº¸¹Ú½º¿¡ ¼ÂÆ®ÇÒ ÄÚµå¸ñ·ÏÀ» ´ãÀ» Item¹è¿­À» ¼±¾ð
			Janus.Windows.EditControls.UIComboBoxItem[] comboItems = new Janus.Windows.EditControls.UIComboBoxItem[slotOrganizationModel.ResultCnt + 1];

			comboItems[0] = new Janus.Windows.EditControls.UIComboBoxItem("½½·Ô¼±ÅÃ3","00");
			
			for(int i=0;i<slotOrganizationModel.ResultCnt;i++)
			{
				DataRow row = slotOrganizationDs.SlotCode.Rows[i];

				string val = row["SlotCode"].ToString();
				string txt = row["SlotName"].ToString();
				comboItems[i+1] = new Janus.Windows.EditControls.UIComboBoxItem(txt,val);
			}
			// ÄÞº¸¿¡ ¼ÂÆ®
			this.cbSlot3.Items.AddRange(comboItems);
			this.cbSlot3.SelectedIndex = 0;

			Application.DoEvents();
		}

		private void InitCombo_Slot4()
		{			
			SlotOrganizationModel slotOrganizationModel = new SlotOrganizationModel();		
			new SlotOrganizationManager(systemModel, commonModel).GetSlotCodeList(slotOrganizationModel);
			
			if (slotOrganizationModel.ResultCD.Equals("0000"))
			{
				// µ¥ÀÌÅÍ¼Â¿¡ ¼ÂÆÃ
				Utility.SetDataTable(slotOrganizationDs.SlotCode, slotOrganizationModel.SlotCodeDataSet);				
			}

			// »ó¼¼Á¶È¸ ÄÞº¸
			// »ó¼¼Á¤º¸ÀÇ ÄÞº¸´Â DatasetÀ» µ¥ÀÌÅÍ¼Ò½º·Î °¡Áø´Ù.

			// °Ë»öÁ¶°ÇÀÇ ÄÞº¸
			this.cbSlot4.Items.Clear();
			
			// ÄÞº¸¹Ú½º¿¡ ¼ÂÆ®ÇÒ ÄÚµå¸ñ·ÏÀ» ´ãÀ» Item¹è¿­À» ¼±¾ð
			Janus.Windows.EditControls.UIComboBoxItem[] comboItems = new Janus.Windows.EditControls.UIComboBoxItem[slotOrganizationModel.ResultCnt + 1];

			comboItems[0] = new Janus.Windows.EditControls.UIComboBoxItem("½½·Ô¼±ÅÃ4","00");
			
			for(int i=0;i<slotOrganizationModel.ResultCnt;i++)
			{
				DataRow row = slotOrganizationDs.SlotCode.Rows[i];

				string val = row["SlotCode"].ToString();
				string txt = row["SlotName"].ToString();
				comboItems[i+1] = new Janus.Windows.EditControls.UIComboBoxItem(txt,val);
			}
			// ÄÞº¸¿¡ ¼ÂÆ®
			this.cbSlot4.Items.AddRange(comboItems);
			this.cbSlot4.SelectedIndex = 0;

			Application.DoEvents();
		}

		private void InitCombo_Slot5()
		{			
			SlotOrganizationModel slotOrganizationModel = new SlotOrganizationModel();		
			new SlotOrganizationManager(systemModel, commonModel).GetSlotCodeList(slotOrganizationModel);
			
			if (slotOrganizationModel.ResultCD.Equals("0000"))
			{
				// µ¥ÀÌÅÍ¼Â¿¡ ¼ÂÆÃ
				Utility.SetDataTable(slotOrganizationDs.SlotCode, slotOrganizationModel.SlotCodeDataSet);				
			}

			// »ó¼¼Á¶È¸ ÄÞº¸
			// »ó¼¼Á¤º¸ÀÇ ÄÞº¸´Â DatasetÀ» µ¥ÀÌÅÍ¼Ò½º·Î °¡Áø´Ù.

			// °Ë»öÁ¶°ÇÀÇ ÄÞº¸
			this.cbSlot5.Items.Clear();
			
			// ÄÞº¸¹Ú½º¿¡ ¼ÂÆ®ÇÒ ÄÚµå¸ñ·ÏÀ» ´ãÀ» Item¹è¿­À» ¼±¾ð
			Janus.Windows.EditControls.UIComboBoxItem[] comboItems = new Janus.Windows.EditControls.UIComboBoxItem[slotOrganizationModel.ResultCnt + 1];

			comboItems[0] = new Janus.Windows.EditControls.UIComboBoxItem("½½·Ô¼±ÅÃ5","00");
			
			for(int i=0;i<slotOrganizationModel.ResultCnt;i++)
			{
				DataRow row = slotOrganizationDs.SlotCode.Rows[i];

				string val = row["SlotCode"].ToString();
				string txt = row["SlotName"].ToString();
				comboItems[i+1] = new Janus.Windows.EditControls.UIComboBoxItem(txt,val);
			}
			// ÄÞº¸¿¡ ¼ÂÆ®
			this.cbSlot5.Items.AddRange(comboItems);
			this.cbSlot5.SelectedIndex = 0;

			Application.DoEvents();
		}

		private void InitCombo_Slot6()
		{			
			SlotOrganizationModel slotOrganizationModel = new SlotOrganizationModel();		
			new SlotOrganizationManager(systemModel, commonModel).GetSlotCodeList(slotOrganizationModel);
			
			if (slotOrganizationModel.ResultCD.Equals("0000"))
			{
				// µ¥ÀÌÅÍ¼Â¿¡ ¼ÂÆÃ
				Utility.SetDataTable(slotOrganizationDs.SlotCode, slotOrganizationModel.SlotCodeDataSet);				
			}

			// »ó¼¼Á¶È¸ ÄÞº¸
			// »ó¼¼Á¤º¸ÀÇ ÄÞº¸´Â DatasetÀ» µ¥ÀÌÅÍ¼Ò½º·Î °¡Áø´Ù.

			// °Ë»öÁ¶°ÇÀÇ ÄÞº¸
			this.cbSlot6.Items.Clear();
			
			// ÄÞº¸¹Ú½º¿¡ ¼ÂÆ®ÇÒ ÄÚµå¸ñ·ÏÀ» ´ãÀ» Item¹è¿­À» ¼±¾ð
			Janus.Windows.EditControls.UIComboBoxItem[] comboItems = new Janus.Windows.EditControls.UIComboBoxItem[slotOrganizationModel.ResultCnt + 1];

			comboItems[0] = new Janus.Windows.EditControls.UIComboBoxItem("½½·Ô¼±ÅÃ6","00");
			
			for(int i=0;i<slotOrganizationModel.ResultCnt;i++)
			{
				DataRow row = slotOrganizationDs.SlotCode.Rows[i];

				string val = row["SlotCode"].ToString();
				string txt = row["SlotName"].ToString();
				comboItems[i+1] = new Janus.Windows.EditControls.UIComboBoxItem(txt,val);
			}
			// ÄÞº¸¿¡ ¼ÂÆ®
			this.cbSlot6.Items.AddRange(comboItems);
			this.cbSlot6.SelectedIndex = 0;

			Application.DoEvents();
		}

		private void InitCombo_Slot7()
		{			
			SlotOrganizationModel slotOrganizationModel = new SlotOrganizationModel();		
			new SlotOrganizationManager(systemModel, commonModel).GetSlotCodeList(slotOrganizationModel);
			
			if (slotOrganizationModel.ResultCD.Equals("0000"))
			{
				// µ¥ÀÌÅÍ¼Â¿¡ ¼ÂÆÃ
				Utility.SetDataTable(slotOrganizationDs.SlotCode, slotOrganizationModel.SlotCodeDataSet);				
			}

			// »ó¼¼Á¶È¸ ÄÞº¸
			// »ó¼¼Á¤º¸ÀÇ ÄÞº¸´Â DatasetÀ» µ¥ÀÌÅÍ¼Ò½º·Î °¡Áø´Ù.

			// °Ë»öÁ¶°ÇÀÇ ÄÞº¸
			this.cbSlot7.Items.Clear();
			
			// ÄÞº¸¹Ú½º¿¡ ¼ÂÆ®ÇÒ ÄÚµå¸ñ·ÏÀ» ´ãÀ» Item¹è¿­À» ¼±¾ð
			Janus.Windows.EditControls.UIComboBoxItem[] comboItems = new Janus.Windows.EditControls.UIComboBoxItem[slotOrganizationModel.ResultCnt + 1];

			comboItems[0] = new Janus.Windows.EditControls.UIComboBoxItem("½½·Ô¼±ÅÃ7","00");
			
			for(int i=0;i<slotOrganizationModel.ResultCnt;i++)
			{
				DataRow row = slotOrganizationDs.SlotCode.Rows[i];

				string val = row["SlotCode"].ToString();
				string txt = row["SlotName"].ToString();
				comboItems[i+1] = new Janus.Windows.EditControls.UIComboBoxItem(txt,val);
			}
			// ÄÞº¸¿¡ ¼ÂÆ®
			this.cbSlot7.Items.AddRange(comboItems);
			this.cbSlot7.SelectedIndex = 0;

			Application.DoEvents();
		}

		private void InitCombo_Slot8()
		{			
			SlotOrganizationModel slotOrganizationModel = new SlotOrganizationModel();		
			new SlotOrganizationManager(systemModel, commonModel).GetSlotCodeList(slotOrganizationModel);
			
			if (slotOrganizationModel.ResultCD.Equals("0000"))
			{
				// µ¥ÀÌÅÍ¼Â¿¡ ¼ÂÆÃ
				Utility.SetDataTable(slotOrganizationDs.SlotCode, slotOrganizationModel.SlotCodeDataSet);				
			}

			// »ó¼¼Á¶È¸ ÄÞº¸
			// »ó¼¼Á¤º¸ÀÇ ÄÞº¸´Â DatasetÀ» µ¥ÀÌÅÍ¼Ò½º·Î °¡Áø´Ù.

			// °Ë»öÁ¶°ÇÀÇ ÄÞº¸
			this.cbSlot8.Items.Clear();
			
			// ÄÞº¸¹Ú½º¿¡ ¼ÂÆ®ÇÒ ÄÚµå¸ñ·ÏÀ» ´ãÀ» Item¹è¿­À» ¼±¾ð
			Janus.Windows.EditControls.UIComboBoxItem[] comboItems = new Janus.Windows.EditControls.UIComboBoxItem[slotOrganizationModel.ResultCnt + 1];

			comboItems[0] = new Janus.Windows.EditControls.UIComboBoxItem("½½·Ô¼±ÅÃ8","00");
			
			for(int i=0;i<slotOrganizationModel.ResultCnt;i++)
			{
				DataRow row = slotOrganizationDs.SlotCode.Rows[i];

				string val = row["SlotCode"].ToString();
				string txt = row["SlotName"].ToString();
				comboItems[i+1] = new Janus.Windows.EditControls.UIComboBoxItem(txt,val);
			}
			// ÄÞº¸¿¡ ¼ÂÆ®
			this.cbSlot8.Items.AddRange(comboItems);
			this.cbSlot8.SelectedIndex = 0;

			Application.DoEvents();
		}

		private void InitCombo_Slot9()
		{			
			SlotOrganizationModel slotOrganizationModel = new SlotOrganizationModel();		
			new SlotOrganizationManager(systemModel, commonModel).GetSlotCodeList(slotOrganizationModel);
			
			if (slotOrganizationModel.ResultCD.Equals("0000"))
			{
				// µ¥ÀÌÅÍ¼Â¿¡ ¼ÂÆÃ
				Utility.SetDataTable(slotOrganizationDs.SlotCode, slotOrganizationModel.SlotCodeDataSet);				
			}

			// »ó¼¼Á¶È¸ ÄÞº¸
			// »ó¼¼Á¤º¸ÀÇ ÄÞº¸´Â DatasetÀ» µ¥ÀÌÅÍ¼Ò½º·Î °¡Áø´Ù.

			// °Ë»öÁ¶°ÇÀÇ ÄÞº¸
			this.cbSlot9.Items.Clear();
			
			// ÄÞº¸¹Ú½º¿¡ ¼ÂÆ®ÇÒ ÄÚµå¸ñ·ÏÀ» ´ãÀ» Item¹è¿­À» ¼±¾ð
			Janus.Windows.EditControls.UIComboBoxItem[] comboItems = new Janus.Windows.EditControls.UIComboBoxItem[slotOrganizationModel.ResultCnt + 1];

			comboItems[0] = new Janus.Windows.EditControls.UIComboBoxItem("½½·Ô¼±ÅÃ9","00");
			
			for(int i=0;i<slotOrganizationModel.ResultCnt;i++)
			{
				DataRow row = slotOrganizationDs.SlotCode.Rows[i];

				string val = row["SlotCode"].ToString();
				string txt = row["SlotName"].ToString();
				comboItems[i+1] = new Janus.Windows.EditControls.UIComboBoxItem(txt,val);
			}
			// ÄÞº¸¿¡ ¼ÂÆ®
			this.cbSlot9.Items.AddRange(comboItems);
			this.cbSlot9.SelectedIndex = 0;

			Application.DoEvents();
		}

		private void InitCombo_Slot10()
		{			
			SlotOrganizationModel slotOrganizationModel = new SlotOrganizationModel();		
			new SlotOrganizationManager(systemModel, commonModel).GetSlotCodeList(slotOrganizationModel);
			
			if (slotOrganizationModel.ResultCD.Equals("0000"))
			{
				// µ¥ÀÌÅÍ¼Â¿¡ ¼ÂÆÃ
				Utility.SetDataTable(slotOrganizationDs.SlotCode, slotOrganizationModel.SlotCodeDataSet);				
			}

			// »ó¼¼Á¶È¸ ÄÞº¸
			// »ó¼¼Á¤º¸ÀÇ ÄÞº¸´Â DatasetÀ» µ¥ÀÌÅÍ¼Ò½º·Î °¡Áø´Ù.

			// °Ë»öÁ¶°ÇÀÇ ÄÞº¸
			this.cbSlot10.Items.Clear();
			
			// ÄÞº¸¹Ú½º¿¡ ¼ÂÆ®ÇÒ ÄÚµå¸ñ·ÏÀ» ´ãÀ» Item¹è¿­À» ¼±¾ð
			Janus.Windows.EditControls.UIComboBoxItem[] comboItems = new Janus.Windows.EditControls.UIComboBoxItem[slotOrganizationModel.ResultCnt + 1];

			comboItems[0] = new Janus.Windows.EditControls.UIComboBoxItem("½½·Ô¼±ÅÃ10","00");
			
			for(int i=0;i<slotOrganizationModel.ResultCnt;i++)
			{
				DataRow row = slotOrganizationDs.SlotCode.Rows[i];

				string val = row["SlotCode"].ToString();
				string txt = row["SlotName"].ToString();
				comboItems[i+1] = new Janus.Windows.EditControls.UIComboBoxItem(txt,val);
			}
			// ÄÞº¸¿¡ ¼ÂÆ®
			this.cbSlot10.Items.AddRange(comboItems);
			this.cbSlot10.SelectedIndex = 0;

			Application.DoEvents();
		}

		private void InitButton()
		{
			if(canRead)  
			{
				btnSearch.Enabled = true;				
			}
			if(canCreate) btnAdd.Enabled    = true;
			
			if(grdExChannelSetList.RecordCount > 0) 
			{
				if(canDelete) btnDelete.Enabled = true;
				if(canUpdate) btnSave.Enabled   = true;
			}
			if(IsAdding)
			{
				if(canCreate) cbMediaName.Enabled    = true;
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
                //			if(grdExCategenList.RecordCount > 0 )
                //			{				
                //				SetCategenDetailText();
                //			}
                SetTextReadonly();
                InitButton();
            }
		}

		
		private void OnGrdRowDetailChanged(object sender, System.EventArgs e) 
		{		
			if(grdExChannelSetList.RecordCount > 0 )
			{
				SetChannelSetDetailText();
			
				mediaCode_old = "";
				categoryCode_old = "";
				genreCode_old = "";
				channelNo_old = "";

				int curRow = cm.Position;
					
				if(curRow >= 0)
				{
					mediaCode_old	 = dt.Rows[curRow]["MediaCode"].ToString();
					categoryCode_old = dt.Rows[curRow]["CategoryCode"].ToString();
					genreCode_old	 = dt.Rows[curRow]["GenreCode"].ToString();					
					channelNo_old    = dt.Rows[curRow]["ChannelNo"].ToString();	
				}

//				mediaCode_old		= grdExChannelSetList.SelectedItems[0].GetRow().Cells["MediaCode"].Value.ToString();
//				categoryCode_old	= grdExChannelSetList.SelectedItems[0].GetRow().Cells["CategoryCode"].Value.ToString();
//				genreCode_old		= grdExChannelSetList.SelectedItems[0].GetRow().Cells["GenreCode"].Value.ToString();
//				channelNo_old		= grdExChannelSetList.SelectedItems[0].GetRow().Cells["ChannelNo"].Value.ToString();				
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
			if(cbSearchMediaName.SelectedValue.ToString() == "00") 
			{
				FrameSystem.showMsgForm("Ã¤³ÎÁ¤º¸°Ë»ö ¿À·ù",new string[] {"", "¸ÅÃ¼À» ¼±ÅÃÇÏ¿© ÁÖ¼¼¿ä.", "" });
				return;
			}
			ReSetChannelSetDetailText();
			ReSetGridData();
			DisableButton();
			SetTextReadonly();			
			SearchSlotOrganization();			
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
			ReSetChannelSetDetailText();			
			//ReSetGridData();			
			//ebChannelNo.Focus();
		}

		/// <summary>
		/// ÀúÀå¹öÆ° Å¬¸¯
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnSave_Click(object sender, System.EventArgs e)
		{			
			SaveChannelSetDetail();		
			//ReSetGridData();
			//SearchSlotOrganizationDetail();			
		}
		/// <summary>
		/// »èÁ¦¹öÆ° Å¬¸¯
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnDelete_Click(object sender, System.EventArgs e)
		{			
			DeleteChannelSet();		
		}

		private void ebSearchKey_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
		{
			if(e.KeyCode == Keys.Enter)
			{
				SearchSlotOrganization();
			}
		}

		private void cbCategoryName_SelectedItemChanged(object sender, System.EventArgs e)
		{
			for(int i=0;i < slotOrganizationDs.SlotOrganization.Rows.Count;i++)
			{
				DataRow row = slotOrganizationDs.SlotOrganization.Rows[i];

				if(row["CategoryCode"].ToString() == cbCategoryName.SelectedValue.ToString())
				{	
					cbMediaName.SelectedValue = row["MediaCode"].ToString();					
					genreCode			  = row["GenreCode"].ToString();
					ebGenreName.Text	  = row["GenreName"].ToString();
					ebChannelNo.Text      = row["ChannelNo"].ToString();	
					cbSlot1.SelectedValue = row["Slot1"].ToString();
					cbSlot2.SelectedValue = row["Slot2"].ToString();
					cbSlot3.SelectedValue = row["Slot3"].ToString();
					cbSlot4.SelectedValue = row["Slot4"].ToString();
					cbSlot5.SelectedValue = row["Slot5"].ToString();
					cbSlot6.SelectedValue = row["Slot6"].ToString();
					cbSlot7.SelectedValue = row["Slot7"].ToString();
					cbSlot8.SelectedValue = row["Slot8"].ToString();
					cbSlot9.SelectedValue = row["Slot9"].ToString();
					cbSlot10.SelectedValue = row["Slot10"].ToString();
				}
			}
		}

		#endregion

		#region Ã³¸®¸Þ¼Òµå

		/// <summary>
		/// Ã¤³Î¸ñ·Ï Á¶È¸
		/// </summary>
		private void SearchSlotOrganization()
		{
            IsSearching = true;

			StatusMessage("Ã¤³Î Á¤º¸¸¦ Á¶È¸ÇÕ´Ï´Ù.");
			//			if(cbSearchMediaName.SelectedItem.Value.Equals("00")) 
			//			{
			//				MessageBox.Show("¸ÅÃ¼¸¦ ¼±ÅÃÇÏ¿© ÁÖ½Ã±â ¹Ù¶ø´Ï´Ù.","Ã¤³Î±¸¼º Á¶È¸", 
			//					MessageBoxButtons.OK, MessageBoxIcon.Information );
			//				return;
			//			}

			try
			{
				slotOrganizationModel.Init();
				// µ¥ÀÌÅÍ¸ðµ¨¿¡ Àü¼ÛÇÒ ³»¿ëÀ» ¼ÂÆ®ÇÑ´Ù.				
				if(chkUseYn.Checked)
				{
					slotOrganizationModel.SearchchkUseYn   = "Y";
				}
				else
				{
					slotOrganizationModel.SearchchkUseYn   = "N";
				}
				slotOrganizationModel.SearchMediaName = cbSearchMediaName.SelectedItem.Value.ToString();
				slotOrganizationModel.SearchCategoryName = cbSearchCategoryName.SelectedItem.Value.ToString();
				slotOrganizationModel.SearchGenreName = cbSearchGenreName.SelectedItem.Value.ToString();

				ReSetChannelSetDetailText();

				// Ã¤³Î¸ñ·ÏÁ¶È¸ ¼­ºñ½º¸¦ È£ÃâÇÑ´Ù.
				new SlotOrganizationManager(systemModel,commonModel).GetSlotList(slotOrganizationModel);

				if (slotOrganizationModel.ResultCD.Equals("0000"))
				{
					Utility.SetDataTable(slotOrganizationDs.SlotOrganization, slotOrganizationModel.SlotDataSet);
					StatusMessage(slotOrganizationModel.ResultCnt + "°ÇÀÇ Ã¤³Î Á¤º¸°¡ Á¶È¸µÇ¾ú½À´Ï´Ù.");
					if(canUpdate)
					{
						AddSchChoice();									
					}	
					SetChannelSetDetailText();					
				}
			}
			catch(FrameException fe)
			{
				FrameSystem.showMsgForm("½½·ÔÁ¶È¸¿À·ù", new string[] {fe.ErrCode, fe.ResultMsg});
			}
			catch(Exception ex)
			{
				FrameSystem.showMsgForm("½½·ÔÁ¶È¸¿À·ù",new string[] {"",ex.Message});
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
				if ( slotOrganizationDs.Tables["SlotOrganization"].Rows.Count < 1 ) return;
              
				foreach (DataRow row in slotOrganizationDs.Tables["SlotOrganization"].Rows)
				{					
					if(IsAdding)
					{
						cm.Position = 0;
						mediaCode_old = null;
						categoryCode_old = null;
						genreCode_old = null;
						channelNo_old = null;
					}
					else
					{						
						if(row["MediaCode"].ToString().Equals(mediaCode_old) && row["CategoryCode"].ToString().Equals(categoryCode_old) && row["GenreCode"].ToString().Equals(genreCode_old) && row["ChannelNo"].ToString().Equals(channelNo_old))
						{					
							cm.Position = rowIndex;
							break;								
						}
					}

					rowIndex++;
					grdExChannelSetList.EnsureVisible();
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
     
		
		private void SearchSlotOrganizationDetail()
		{
			StatusMessage("Ã¤³Î µðÅ×ÀÏ Á¤º¸¸¦ Á¶È¸ÇÕ´Ï´Ù.");

			try
			{
               
				// µ¥ÀÌÅÍ¸ðµ¨¿¡ Àü¼ÛÇÒ ³»¿ëÀ» ¼ÂÆ®ÇÑ´Ù.
                
				int curRow = cm.Position;
             
				ebChannelNo.Text                = dtChild.Rows[curRow]["ChannelNo"].ToString();
								
				// Ã¤³Î¸ñ·ÏÁ¶È¸ ¼­ºñ½º¸¦ È£ÃâÇÑ´Ù.
				//new SlotOrganizationManager(systemModel,commonModel).GetChannelSetList(slotOrganizationModel);

				if (slotOrganizationModel.ResultCD.Equals("0000"))
				{
					//Utility.SetDataTable(slotOrganizationDs.ChannelSets, slotOrganizationModel.ChannelSetDataSet);
					StatusMessage(slotOrganizationModel.ResultCnt + "°ÇÀÇ Ã¤³Î Á¤º¸°¡ Á¶È¸µÇ¾ú½À´Ï´Ù.");					
				}			
			}
			catch(FrameException fe)
			{
				FrameSystem.showMsgForm("½½·ÔÁ¶È¸¿À·ù", new string[] {fe.ErrCode, fe.ResultMsg});
			}
			catch(Exception ex)
			{
				FrameSystem.showMsgForm("½½·ÔÁ¶È¸¿À·ù",new string[] {"",ex.Message});
			}
		}


		/// <summary>
		/// Ã¤³Î»ó¼¼Á¤º¸ ÀúÀå
		/// </summary>
		private void SaveChannelSetDetail()
		{
			//IsAdding = true;

			StatusMessage("Ã¤³Î Á¤º¸¸¦ ÀúÀåÇÕ´Ï´Ù.");                        
                      
			if(cbMediaName.SelectedValue.ToString().Length == 0) 
			{
				MessageBox.Show("¸ÅÃ¼¸íÀÌ ÀÔ·ÂµÇÁö ¾Ê¾Ò½À´Ï´Ù.","Ã¤³Î±¸¼º ÀúÀå", 
					MessageBoxButtons.OK, MessageBoxIcon.Information );
				return;								
			}		
                        
			try
			{
				//ÀúÀå Àü¿¡ ¸ðµ¨À» ÃÊ±âÈ­ ÇØÁØ´Ù.
				slotOrganizationModel.Init();
				// µ¥ÀÌÅÍ¸ðµ¨¿¡ Àü¼ÛÇÒ ³»¿ëÀ» ¼ÂÆ®ÇÑ´Ù.
				slotOrganizationModel.MediaCode      = cbMediaName.SelectedValue.ToString();
				slotOrganizationModel.CategoryCode   = cbCategoryName.SelectedValue.ToString();
				slotOrganizationModel.GenreCode      = genreCode;
				slotOrganizationModel.ChannelNo      = ebChannelNo.Text;		

				slotOrganizationModel.Slot1      = cbSlot1.SelectedValue.ToString();		
				slotOrganizationModel.Slot2      = cbSlot2.SelectedValue.ToString();		
				slotOrganizationModel.Slot3      = cbSlot3.SelectedValue.ToString();		
				slotOrganizationModel.Slot4      = cbSlot4.SelectedValue.ToString();		
				slotOrganizationModel.Slot5      = cbSlot5.SelectedValue.ToString();		
				slotOrganizationModel.Slot6      = cbSlot6.SelectedValue.ToString();		
				slotOrganizationModel.Slot7      = cbSlot7.SelectedValue.ToString();		
				slotOrganizationModel.Slot8      = cbSlot8.SelectedValue.ToString();		
				slotOrganizationModel.Slot9      = cbSlot9.SelectedValue.ToString();		
				slotOrganizationModel.Slot10      = cbSlot10.SelectedValue.ToString();		

				//»ç¿ë¿©ºÎ
				if(rbUseYn_Y.Checked)
				{
					slotOrganizationModel.UseYn       = "Y";
				}
				else
				{
					slotOrganizationModel.UseYn       = "N";
				}
				
				slotOrganizationModel.MediaCode_old = mediaCode_old;
				slotOrganizationModel.CategoryCode_old = categoryCode_old;
				slotOrganizationModel.GenreCode_old = genreCode_old;
				slotOrganizationModel.ChannelNo_old = channelNo_old;
					
				//slotOrganizationModel.ChannelSetDataSet = slotOrganizationDs.Copy();                         

				// Ã¤³Î »ó¼¼Á¤º¸ ÀúÀå ¼­ºñ½º¸¦ È£ÃâÇÑ´Ù.
				if (IsAdding)
				{
					new SlotOrganizationManager(systemModel,commonModel).SetSlotAdd(slotOrganizationModel);
					StatusMessage("Ã¤³Î±¸¼º Á¤º¸°¡ Ãß°¡µÇ¾ú½À´Ï´Ù.");
					IsAdding = false;
					ReSetChannelSetDetailText();
				}
				else
				{
					new SlotOrganizationManager(systemModel,commonModel).SetSlotUpdate(slotOrganizationModel);
					StatusMessage("Ã¤³Î±¸¼º Á¤º¸°¡ ¼öÁ¤µÇ¾ú½À´Ï´Ù.");
				}
				
				DisableButton();
				//SearchSlotOrganizationDetail();
				SearchSlotOrganization();
				InitButton();
                        
			}
			catch(FrameException fe)
			{
				FrameSystem.showMsgForm("Ã¤³ÎÁ¤º¸ ÀúÀå¿À·ù", new string[] {fe.ErrCode, fe.ResultMsg});
			}
			catch(Exception ex)
			{
				FrameSystem.showMsgForm("Ã¤³ÎÁ¤º¸ ÀúÀå¿À·ù",new string[] {"",ex.Message});
			}			
		}

		/// <summary>
		/// Ã¤³ÎÁ¤º¸ »èÁ¦
		/// </summary>
		private void DeleteChannelSet()
		{
			StatusMessage("Ã¤³Î Á¤º¸¸¦ »èÁ¦ÇÕ´Ï´Ù.");
                        
			if(cbMediaName.SelectedValue.ToString().Length == 0) 
			{
				MessageBox.Show("»èÁ¦ÇÒ ½½·Ô Á¤º¸°¡ ¾ø½À´Ï´Ù.","½½·Ô »èÁ¦", 
					MessageBoxButtons.OK, MessageBoxIcon.Information );
				return;
			}
                        
			DialogResult result = MessageBox.Show("ÇØ´ç ½½·Ô Á¤º¸¸¦ »èÁ¦ ÇÏ½Ã°Ú½À´Ï±î?","½½·Ô »èÁ¦",
				MessageBoxButtons.YesNo, MessageBoxIcon.Question,
				MessageBoxDefaultButton.Button2);
                        
			if (result == DialogResult.No) return;
                        
			try
			{				
				// µ¥ÀÌÅÍ¸ðµ¨¿¡ Àü¼ÛÇÒ ³»¿ëÀ» ¼ÂÆ®ÇÑ´Ù.
				slotOrganizationModel.MediaCode      = cbMediaName.SelectedValue.ToString();
				slotOrganizationModel.CategoryCode   = cbCategoryName.SelectedValue.ToString();
				slotOrganizationModel.GenreCode      = genreCode;
				slotOrganizationModel.ChannelNo      = ebChannelNo.Text;		
                        
				// Ã¤³Î »ó¼¼Á¤º¸ ÀúÀå ¼­ºñ½º¸¦ È£ÃâÇÑ´Ù.
				new SlotOrganizationManager(systemModel,commonModel).SetSlotDelete(slotOrganizationModel);
                        			
				ReSetChannelSetDetailText();			
				SearchSlotOrganization();
				StatusMessage("Ã¤³Î Á¤º¸°¡ »èÁ¦µÇ¾ú½À´Ï´Ù.");			
				//ReSetGridData();
					
				DisableButton();
				//SearchSlotOrganizationDetail();
				InitButton();
			}
			catch(FrameException fe)
			{
				FrameSystem.showMsgForm("Ã¤³ÎÁ¤º¸ »èÁ¦¿À·ù", new string[] {fe.ErrCode, fe.ResultMsg});
			}
			catch(Exception ex)
			{
				FrameSystem.showMsgForm("Ã¤³ÎÁ¤º¸ »èÁ¦¿À·ù",new string[] {"",ex.Message});
			}			

		}
		
		/// <summary>
		/// Ã¤³Î »ó¼¼Á¤º¸ÀÇ ¼ÂÆ®
		/// </summary>
		private void SetCategenDetailText()
		{
			int curRow = cm.Position;
            
			cbMediaName.ReadOnly         = false;			
			cbCategoryName.ReadOnly         = false;			
			ebGenreName.ReadOnly         = false;											
						
			cbMediaName.BackColor        = Color.White;			
			cbCategoryName.BackColor        = Color.White;			
			ebGenreName.BackColor        = Color.White;						
            
			IsAdding = false;
            
			StatusMessage("ÁØºñ");
		}

		private void SetChannelSetDetailText()
		{		
			int curRow = cm.Position;
					
			if(curRow >= 0)
			{
				cbMediaName.SelectedValue = dt.Rows[curRow]["MediaCode"].ToString();
				cbCategoryName.SelectedValue = dt.Rows[curRow]["CategoryCode"].ToString();
				genreCode = dt.Rows[curRow]["GenreCode"].ToString();
				ebGenreName.Text = dt.Rows[curRow]["GenreName"].ToString();
				ebChannelNo.Text          = dt.Rows[curRow]["ChannelNo"].ToString();	
				cbSlot1.SelectedValue = dt.Rows[curRow]["Slot1"].ToString();
				cbSlot2.SelectedValue = dt.Rows[curRow]["Slot2"].ToString();
				cbSlot3.SelectedValue = dt.Rows[curRow]["Slot3"].ToString();
				cbSlot4.SelectedValue = dt.Rows[curRow]["Slot4"].ToString();
				cbSlot5.SelectedValue = dt.Rows[curRow]["Slot5"].ToString();
				cbSlot6.SelectedValue = dt.Rows[curRow]["Slot6"].ToString();
				cbSlot7.SelectedValue = dt.Rows[curRow]["Slot7"].ToString();
				cbSlot8.SelectedValue = dt.Rows[curRow]["Slot8"].ToString();
				cbSlot9.SelectedValue = dt.Rows[curRow]["Slot9"].ToString();
				cbSlot10.SelectedValue = dt.Rows[curRow]["Slot10"].ToString();
				ebRegDt.Text          = dt.Rows[curRow]["RegDt"].ToString();
				ebModDt.Text          = dt.Rows[curRow]["ModDt"].ToString();
				ebRegID.Text          = dt.Rows[curRow]["RegName"].ToString();

				string UseYn              = dt.Rows[curRow]["UseYn"].ToString();

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

				IsAdding = false;
				ResetTextReadonly();
			}					
				
			StatusMessage("ÁØºñ");
		}

		private void ReSetChannelSetDetailText()
		{	
			//cbMediaName.SelectedIndex =  0;
			cbCategoryName.SelectedIndex =  0;
			ebGenreName.Text                 = "";						
			ebChannelNo.Text                 = "";						
			cbSlot1.SelectedIndex =  0;
			cbSlot2.SelectedIndex =  0;
			cbSlot3.SelectedIndex =  0;
			cbSlot4.SelectedIndex =  0;
			cbSlot5.SelectedIndex =  0;
			cbSlot6.SelectedIndex =  0;
			cbSlot7.SelectedIndex =  0;
			cbSlot8.SelectedIndex =  0;
			cbSlot9.SelectedIndex =  0;
			cbSlot10.SelectedIndex =  0;			

			rbUseYn_Y.Checked         = true;
			rbUseYn_N.Checked         = false;

			ebRegDt.Text                 = "";
			ebModDt.Text                 = "";
			ebRegID.Text                 = "";
		}
        
		private void ReSetGridData()
		{			
			//Ãß°¡¸¦ ÇÏ¸é Ã¤³Î¼Â±×¸®µå¸¦ ¸®¼ÂÇÑ´Ù.
			slotOrganizationDs.SlotOrganization.Clear();        
		}
        
		
		/// <summary>
		/// »ó¼¼Á¤º¸ ReadOnly
		/// </summary>
		private void SetTextReadonly()
		{
			cbMediaName.ReadOnly = true;
			cbCategoryName.ReadOnly = true;
			ebGenreName.ReadOnly = true;			
			ebChannelNo.ReadOnly       = true;

			ebChannelNo.BackColor = Color.WhiteSmoke;
			cbMediaName.BackColor = Color.White;
			cbCategoryName.BackColor = Color.White;
			ebGenreName.BackColor = Color.WhiteSmoke;			
		}

		/// <summary>
		/// »ó¼¼Á¤º¸ ¼öÁ¤°¡´ÉÄÉ
		/// </summary>
		private void ResetTextReadonly()
		{	
			cbMediaName.ReadOnly = false; 
			cbCategoryName.ReadOnly = false; 
			ebGenreName.ReadOnly = false;			
			ebChannelNo.ReadOnly       = false;

			cbMediaName.BackColor      = Color.White;
			cbCategoryName.BackColor     = Color.White;			
			ebGenreName.BackColor      = Color.White;									
			ebChannelNo.BackColor   = Color.White;					
		}

		/// <summary>
		/// »ó¼¼Á¤º¸ ¼öÁ¤°¡´ÉÄÉ
		/// </summary>
		private void setSeriesNo()
		{
			int i=1;
			//Debug.WriteLine("setSeriesNo1:" + slotOrganizationDs.ChannelSets.Rows.Count);
			if(slotOrganizationDs.Tables["SlotOrganization"].Rows.Count == 1)
			{
				i--;
			}

			//ÄÁÅÙÃ÷ ¸ñ·Ï °¹¼ö°¡ 1°³ÀÌ¸é 0¹øÀ¸·Î ÀÔ·ÂÀ» ÇÏ°í w
			//n°³ ÀÌ¸é 1ºÎÅÍ ÀÔ·ÂÇÑ´Ù.
			Debug.WriteLine("setSeriesNo21:" + slotOrganizationDs.SlotOrganization.Rows.Count);
			Debug.WriteLine("setSeriesNo22:" + slotOrganizationDs.Tables["SlotOrganization"].Rows.Count);

			foreach (DataRow row in slotOrganizationDs.Tables["ChannelSets"].Rows)
			{
             
				row["SeriesNo"] = i;
				i++;
			}

			//ÀüÃ¼ Ã¼Å©¹Ú½º Ç¥½Ã ÇßÀ»¶§ 0À¸·Î ¹Ù²îÁö ¾Ê¾Æ¼­ °­Á¦Àû ¼ÂÆÃ
			//ÃßÈÄ ¹æ¹ýÀÌ ÇØ°á¹æ¾È Ã£À¸¸é ¼öÁ¤
			Debug.WriteLine("setSeriesNo3:" + slotOrganizationDs.SlotOrganization.Rows.Count);
            
			if(slotOrganizationDs.Tables["ChannelSets"].Rows.Count ==1)
			{
				DataRow row = slotOrganizationDs.Tables["ChannelSets"].Rows[0];
				row["SeriesNo"] = 0;
			}
         

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

		
		private void btnDetailDelete_Click(object sender, System.EventArgs e)
		{
			try
			{
				int i=0;
              
				//gridÀÇ Ã¼Å© ÇÃ·¡±×¸¦ dtChild¿¡ Àû¿ëÇØÁØ´Ù.
				foreach (Janus.Windows.GridEX.GridEXRow gr in grdExChannelSetList.GetRows())
				{
					if(gr.Cells["CheckYn"].Value.ToString().Equals("True"))
					{
						dtChild.Rows[i]["CheckYn"]="True";
					}
					i++;
				} 
				Debug.WriteLine("----------------------------------------------");
				//Debug.WriteLine("Delect_Click1:" + slotOrganizationDs.ChannelSets.Rows.Count);

				//gridÀÇ Ã¼Å© ÇÃ·¡±×°¡ "True"ÀÎ°Í¸¸ »èÁ¦. 
				DataRow[] deleteRows = slotOrganizationDs.SlotOrganization.Select("CheckYn='True'");
                

				Debug.WriteLine("Delect_Click2:" + slotOrganizationDs.SlotOrganization.Rows.Count);
				for( int j = 0; j < deleteRows.Length;j++)
				{
					deleteRows[j].Delete();
				}
				Debug.WriteLine("Delect_Click3:" + slotOrganizationDs.SlotOrganization.Rows.Count);
				setSeriesNo();
				Debug.WriteLine("Delect_Click4:" + slotOrganizationDs.SlotOrganization.Rows.Count);
			}
			catch(Exception ex)
			{
				Debug.WriteLine(ex.Message);
			}
		}


		#region ¿ÜºÎ³ëÃâ ¸Þ¼Òµå
		/// <summary>
		/// ¼±ÅÃµÈ RowµéÀ» ÀÔ·Â½ÃÅ´
		/// </summary>
		/// <param name="dtc"></param>
		public void adOn_AddContent(DataSet contentsDs)
		{
            
			//¸ð´ÞÃ¢¿¡¼­ ¹Þ¾Æ¿Â DataSetÀ» Loop¸¸Å­ µ¹·Á¼­ Ã¼Å©µÈ °ªÀ»
			//ÀÎ¼­Æ® ½ÃÅ´
			for(int i=0;i< contentsDs.Tables["ChannelSets"].Rows.Count;i++)
			{

				DataRow row = contentsDs.Tables["ChannelSets"].Rows[i];

				if(row["CheckYn"].ToString().Equals("True"))
				{

                
					DataRow[] foundRows = dtChild.Select("ContentId = '" + row["ContentId"].ToString() + "'");
                    
					if( foundRows.Length == 0 )
					{
						DataRow newRow = dtChild.NewRow();

						newRow[0] = "False";
						newRow[1] = row["ContentId"].ToString();
						newRow[2] = row["Title"].ToString();
             
						dtChild.Rows.Add(newRow);
						dtChild.Rows.Add(new Object[] {1, "False",2,row["ContentId"].ToString(),3,row["Title"].ToString()});

						newRow = null;
					}
					if( null != foundRows ) foundRows = null;
				}
				if( null != row ) row = null;


			}
			//ÄÁÅÙÃ÷¸®½ºÆ®°¡ 0º¸´Ù Å¬°æ¿ì¿¡ »èÁ¦ ¹öÆ° È°¼ºÈ­
			if(contentsDs.Tables["ChannelSets"].Rows.Count >0)
			{
				//btnDetailDelete.Enabled = true;
			}
			setSeriesNo();
		}

		#endregion

		//ÀÌ ¸Þ¼Òµå°¡ ½ÇÇàÀÌ µÇ¸é ÇØ´ç ÇÊµåÀÇ ÆË¾÷ÀÌ È£ÃâµÈ´Ù.
		private void ResetPop()
		{			
			SlotChannelNoPopForm slotChannelNoPopForm = new SlotChannelNoPopForm(this);
			slotChannelNoPopForm.ShowDialog();
			slotChannelNoPopForm.Dispose();
			slotChannelNoPopForm = null;
		}

		private void btnChannelNo_Click(object sender, System.EventArgs e)
		{
			// Ã¤³Î ¸ñ·Ï °Ë»ö ÆË¾÷ ¶ì¿ì±â
			SlotChannelNoPopForm slotChannelNoPopForm = new SlotChannelNoPopForm(this);
			slotChannelNoPopForm.ShowDialog();
			slotChannelNoPopForm.Dispose();
			slotChannelNoPopForm = null;
		}	
		
		public string ChannelNo
		{				
			set
			{
				this.ebChannelNo.Text = value;
			}			
		}
		
		private void btnGenreName_Click(object sender, System.EventArgs e)
		{
			// Ã¤³Î ¸ñ·Ï °Ë»ö ÆË¾÷ ¶ì¿ì±â
			SlotGenrePopForm SlotGenrePopForm = new SlotGenrePopForm(this);
			SlotGenrePopForm.KeyCategoryCode = cbCategoryName.SelectedValue.ToString();
			SlotGenrePopForm.KeyMediaCode = cbMediaName.SelectedValue.ToString();
			SlotGenrePopForm.ShowDialog();
			SlotGenrePopForm.Dispose();
			SlotGenrePopForm = null;
		}

		public string GenreCode
		{
			set
			{
				this.genreCode = value;
			}
		}

		public string GenreName
		{				
			set
			{
				this.ebGenreName.Text = value;
			}			
		}
	}
}