// ===============================================================================
// ChooseAdScheduleControl for Charites Project
//
// ChooseAdScheduleControl.cs
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
namespace AdManagerClient
{
    /// <summary>
    /// Ã¤³Î/¸Þ´ºº° Æí¼ºÇöÈ² ÄÁÆ®·Ñ
    /// </summary>
    public class SchMidAdControl : System.Windows.Forms.UserControl, IUserControl
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
        ChooseAdScheduleModel chooseAdScheduleModel = new ChooseAdScheduleModel();	// Ã¤³Î/¸Þ´º Æí¼ºÇöÈ² ¸ðµ¨
		SchChoiceAdModel schChoiceAdModel			= new SchChoiceAdModel();		// ÁöÁ¤±¤°íÆí¼º¸ðµ¨
		SchPublishModel schPublishModel				= new SchPublishModel();		// ±¤°í½ÂÀÎ¸ðµ¨
		
		// È­¸éÃ³¸®¿ë º¯¼ö
        CurrencyManager cmMenu        = null;					// µ¥ÀÌÅÍ ±×¸®µåÀÇ º¯°æ¿¡ µû¸¥ µ¥ÀÌÅÍ¼Â °ü¸®¸¦ À§ÇÏ¿©			
        CurrencyManager cmChannel     = null;					
		CurrencyManager cmSchedule    = null;					
		DataTable       dtMenu        = null;
        DataTable       dtChannel     = null;
		DataTable       dtSchedule    = null;

		// »ç¿ë±ÇÇÑ
        bool IsSearching = false; // Á¶È¸Áß »ó¼¼È­¸éÀÌ ¾÷µ¥ÀÌÆ® µÇ´Â °ÍÀ» ¹æÁö ÇÏ±âÀ§ÇÔ 2011.11.29 JH.Park
		bool canCreate			  = false;
		bool canRead			  = false;
		bool canUpdate			  = false;
		bool canDelete            = false;

		// Key
		bool IsNotLoading		       = true;					// »ó¼¼Á¶È¸ÁßÀÌ ¾Æ´Ô
		public string keyMediaCode     = "";
		public string keyCategoryCode  = "";
		public string keyGenreCode     = "";
		public string keyChannelNo     = "";
		public string keyItemNo        = "";
		public string keyScheduleOrder = "";
        public string keyTitle          = "";

		string keyCmType       = "";	
		string keyItemName     = "";
		string keyLastOrder    = "";
		
		// ¼øÀ§º¯°æ±¸ºÐ
		const int ORDER_FIRST = 1;
		const int ORDER_LAST  = 2;
		const int ORDER_UP    = 3;
		const int ORDER_DOWN  = 4;

		// Æí¼º¹èÆ÷ ½ÂÀÎ»óÅÂ Ã³¸®¿ë
		private string keyAckNo    = "";
		private string keyAckState = "";

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
        private Janus.Windows.EditControls.UIButton uiButton2;
        private Janus.Windows.EditControls.UIButton uiButton1;
        private Janus.Windows.UI.Dock.UIPanelGroup uiPanelUsers;
        private System.Windows.Forms.Panel pnlSearch;
        private System.Windows.Forms.Panel pnlSearchBtn;
        private Janus.Windows.EditControls.UIButton btnSearch;
        private System.Data.DataView dvChooseAdSchedule;
        private System.Data.DataView dvCateGen;
        private Janus.Windows.EditControls.UIComboBox cbSearchMedia;
        private Janus.Windows.UI.Dock.UIPanelGroup uiPanelChooseAdSchedule;
        private Janus.Windows.UI.Dock.UIPanel uiPanel1;
        private Janus.Windows.UI.Dock.UIPanelInnerContainer uiPanel1Container;
        private Janus.Windows.UI.Dock.UIPanel uiPanel2;
        private Janus.Windows.UI.Dock.UIPanelInnerContainer uiPanel2Container;
        private Janus.Windows.GridEX.GridEX grdExCategenList;
        private Janus.Windows.GridEX.GridEX grdExChooseAdScheduleList;
        private System.Windows.Forms.Button btnGenreName;
        private System.Windows.Forms.Panel pnlUserDetail;
        private System.Data.DataView dvChannelSet;
        private Janus.Windows.GridEX.GridEX grdExScheduleList;
		private Janus.Windows.UI.Dock.UIPanel uiPanelSearch;
		private Janus.Windows.UI.Dock.UIPanelInnerContainer uiPanelSearchContainer;
		private Janus.Windows.UI.Dock.UIPanel uiPanelList;
		private Janus.Windows.UI.Dock.UIPanelInnerContainer uiPanelListContainer;
		private System.Windows.Forms.Panel pnlDetail;
		private Janus.Windows.EditControls.UIButton btnDelete;
		private Janus.Windows.UI.Dock.UIPanel uiPanelDetail;
		private Janus.Windows.UI.Dock.UIPanelInnerContainer uiPanelDetailContainer;
		private System.Windows.Forms.Label lbMsg;
        private Janus.Windows.EditControls.UICheckBox chkAdState_20;
        private System.Windows.Forms.Label lbAdState;
        private Janus.Windows.EditControls.UICheckBox chkAdState_30;
        private Janus.Windows.EditControls.UICheckBox chkAdState_40;
        private System.Windows.Forms.Label label1;
        private Janus.Windows.EditControls.UIButton btnAdd_16;
        private AdManagerClient.SchMidAdDs schMidAdDs;		
        private System.ComponentModel.IContainer components;

        public SchMidAdControl()
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
            Janus.Windows.GridEX.GridEXLayout grdExCategenList_DesignTimeLayout = new Janus.Windows.GridEX.GridEXLayout();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SchMidAdControl));
            Janus.Windows.GridEX.GridEXLayout grdExCategenList_Layout_0 = new Janus.Windows.GridEX.GridEXLayout();
            Janus.Windows.GridEX.GridEXLayout grdExChooseAdScheduleList_DesignTimeLayout = new Janus.Windows.GridEX.GridEXLayout();
            Janus.Windows.Common.Layouts.JanusLayoutReference grdExChooseAdScheduleList_DesignTimeLayout_Reference_0 = new Janus.Windows.Common.Layouts.JanusLayoutReference("GridEXLayoutData.RootTable.FormatConditions.Condition2.FormatStyle.BackgroundImag" +
        "e");
            Janus.Windows.GridEX.GridEXLayout grdExScheduleList_DesignTimeLayout = new Janus.Windows.GridEX.GridEXLayout();
            this.uiPM = new Janus.Windows.UI.Dock.UIPanelManager(this.components);
            this.uiPanelUsers = new Janus.Windows.UI.Dock.UIPanelGroup();
            this.uiPanelSearch = new Janus.Windows.UI.Dock.UIPanel();
            this.uiPanelSearchContainer = new Janus.Windows.UI.Dock.UIPanelInnerContainer();
            this.pnlSearch = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.chkAdState_20 = new Janus.Windows.EditControls.UICheckBox();
            this.lbAdState = new System.Windows.Forms.Label();
            this.chkAdState_30 = new Janus.Windows.EditControls.UICheckBox();
            this.chkAdState_40 = new Janus.Windows.EditControls.UICheckBox();
            this.cbSearchMedia = new Janus.Windows.EditControls.UIComboBox();
            this.pnlSearchBtn = new System.Windows.Forms.Panel();
            this.btnSearch = new Janus.Windows.EditControls.UIButton();
            this.lbMsg = new System.Windows.Forms.Label();
            this.uiPanelChooseAdSchedule = new Janus.Windows.UI.Dock.UIPanelGroup();
            this.uiPanel1 = new Janus.Windows.UI.Dock.UIPanel();
            this.uiPanel1Container = new Janus.Windows.UI.Dock.UIPanelInnerContainer();
            this.grdExCategenList = new Janus.Windows.GridEX.GridEX();
            this.dvCateGen = new System.Data.DataView();
            this.schMidAdDs = new AdManagerClient.SchMidAdDs();
            this.uiPanel2 = new Janus.Windows.UI.Dock.UIPanel();
            this.uiPanel2Container = new Janus.Windows.UI.Dock.UIPanelInnerContainer();
            this.grdExChooseAdScheduleList = new Janus.Windows.GridEX.GridEX();
            this.dvChannelSet = new System.Data.DataView();
            this.uiPanelList = new Janus.Windows.UI.Dock.UIPanel();
            this.uiPanelListContainer = new Janus.Windows.UI.Dock.UIPanelInnerContainer();
            this.pnlUserDetail = new System.Windows.Forms.Panel();
            this.grdExScheduleList = new Janus.Windows.GridEX.GridEX();
            this.dvChooseAdSchedule = new System.Data.DataView();
            this.btnGenreName = new System.Windows.Forms.Button();
            this.uiPanelDetail = new Janus.Windows.UI.Dock.UIPanel();
            this.uiPanelDetailContainer = new Janus.Windows.UI.Dock.UIPanelInnerContainer();
            this.pnlDetail = new System.Windows.Forms.Panel();
            this.btnDelete = new Janus.Windows.EditControls.UIButton();
            this.btnAdd_16 = new Janus.Windows.EditControls.UIButton();
            this.uiButton1 = new Janus.Windows.EditControls.UIButton();
            this.uiButton2 = new Janus.Windows.EditControls.UIButton();
            ((System.ComponentModel.ISupportInitialize)(this.uiPM)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelUsers)).BeginInit();
            this.uiPanelUsers.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelSearch)).BeginInit();
            this.uiPanelSearch.SuspendLayout();
            this.uiPanelSearchContainer.SuspendLayout();
            this.pnlSearch.SuspendLayout();
            this.pnlSearchBtn.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelChooseAdSchedule)).BeginInit();
            this.uiPanelChooseAdSchedule.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanel1)).BeginInit();
            this.uiPanel1.SuspendLayout();
            this.uiPanel1Container.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdExCategenList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dvCateGen)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.schMidAdDs)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanel2)).BeginInit();
            this.uiPanel2.SuspendLayout();
            this.uiPanel2Container.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdExChooseAdScheduleList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dvChannelSet)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelList)).BeginInit();
            this.uiPanelList.SuspendLayout();
            this.uiPanelListContainer.SuspendLayout();
            this.pnlUserDetail.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdExScheduleList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dvChooseAdSchedule)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelDetail)).BeginInit();
            this.uiPanelDetail.SuspendLayout();
            this.uiPanelDetailContainer.SuspendLayout();
            this.pnlDetail.SuspendLayout();
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
            this.uiPanelSearch.Id = new System.Guid("afb3b7d0-aa79-443c-a1e8-3baba38f7a4d");
            this.uiPanelUsers.Panels.Add(this.uiPanelSearch);
            this.uiPanelChooseAdSchedule.Id = new System.Guid("88f86479-98e4-43d4-bf03-12fd789fcfc5");
            this.uiPanelChooseAdSchedule.StaticGroup = true;
            this.uiPanel1.Id = new System.Guid("91eeb7fc-856c-4a9c-b37e-db742cfa9b2e");
            this.uiPanelChooseAdSchedule.Panels.Add(this.uiPanel1);
            this.uiPanel2.Id = new System.Guid("3d64f731-de32-42ba-a10f-ac3b5ac56703");
            this.uiPanelChooseAdSchedule.Panels.Add(this.uiPanel2);
            this.uiPanelUsers.Panels.Add(this.uiPanelChooseAdSchedule);
            this.uiPanelList.Id = new System.Guid("b2973a57-d3e1-4d80-9387-7a6dc3332542");
            this.uiPanelUsers.Panels.Add(this.uiPanelList);
            this.uiPanelDetail.Id = new System.Guid("c1ac152b-71d8-497e-a36e-0758f080f6ec");
            this.uiPanelUsers.Panels.Add(this.uiPanelDetail);
            this.uiPM.Panels.Add(this.uiPanelUsers);
            // 
            // Design Time Panel Info:
            // 
            this.uiPM.BeginPanelInfo();
            this.uiPM.AddDockPanelInfo(new System.Guid("b162c53a-7940-47cd-a0ed-9762c59c50d8"), Janus.Windows.UI.Dock.PanelGroupStyle.HorizontalTiles, Janus.Windows.UI.Dock.PanelDockStyle.Fill, true, new System.Drawing.Size(1010, 677), true);
            this.uiPM.AddDockPanelInfo(new System.Guid("afb3b7d0-aa79-443c-a1e8-3baba38f7a4d"), new System.Guid("b162c53a-7940-47cd-a0ed-9762c59c50d8"), 42, true);
            this.uiPM.AddDockPanelInfo(new System.Guid("88f86479-98e4-43d4-bf03-12fd789fcfc5"), new System.Guid("b162c53a-7940-47cd-a0ed-9762c59c50d8"), Janus.Windows.UI.Dock.PanelGroupStyle.VerticalTiles, true, 259, true);
            this.uiPM.AddDockPanelInfo(new System.Guid("91eeb7fc-856c-4a9c-b37e-db742cfa9b2e"), new System.Guid("88f86479-98e4-43d4-bf03-12fd789fcfc5"), 357, true);
            this.uiPM.AddDockPanelInfo(new System.Guid("3d64f731-de32-42ba-a10f-ac3b5ac56703"), new System.Guid("88f86479-98e4-43d4-bf03-12fd789fcfc5"), 649, true);
            this.uiPM.AddDockPanelInfo(new System.Guid("b2973a57-d3e1-4d80-9387-7a6dc3332542"), new System.Guid("b162c53a-7940-47cd-a0ed-9762c59c50d8"), 280, true);
            this.uiPM.AddDockPanelInfo(new System.Guid("c1ac152b-71d8-497e-a36e-0758f080f6ec"), new System.Guid("b162c53a-7940-47cd-a0ed-9762c59c50d8"), 42, true);
            this.uiPM.AddFloatingPanelInfo(new System.Guid("b162c53a-7940-47cd-a0ed-9762c59c50d8"), Janus.Windows.UI.Dock.PanelGroupStyle.HorizontalTiles, true, new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
            this.uiPM.AddFloatingPanelInfo(new System.Guid("8e3140b9-bfcb-43af-b84e-2ee6cddf51dc"), new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
            this.uiPM.AddFloatingPanelInfo(new System.Guid("b2973a57-d3e1-4d80-9387-7a6dc3332542"), new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
            this.uiPM.AddFloatingPanelInfo(new System.Guid("afb3b7d0-aa79-443c-a1e8-3baba38f7a4d"), new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
            this.uiPM.AddFloatingPanelInfo(new System.Guid("88f86479-98e4-43d4-bf03-12fd789fcfc5"), Janus.Windows.UI.Dock.PanelGroupStyle.VerticalTiles, true, new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
            this.uiPM.AddFloatingPanelInfo(new System.Guid("91eeb7fc-856c-4a9c-b37e-db742cfa9b2e"), new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
            this.uiPM.AddFloatingPanelInfo(new System.Guid("3d64f731-de32-42ba-a10f-ac3b5ac56703"), new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
            this.uiPM.AddFloatingPanelInfo(new System.Guid("609188ab-b98f-4466-8472-b8b36f1af6d5"), Janus.Windows.UI.Dock.PanelGroupStyle.HorizontalTiles, true, new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
            this.uiPM.AddFloatingPanelInfo(new System.Guid("c1ac152b-71d8-497e-a36e-0758f080f6ec"), new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
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
            this.uiPanelUsers.Text = "Áß°£±¤°í Æí¼º";
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
            this.pnlSearch.Controls.Add(this.label1);
            this.pnlSearch.Controls.Add(this.chkAdState_20);
            this.pnlSearch.Controls.Add(this.lbAdState);
            this.pnlSearch.Controls.Add(this.chkAdState_30);
            this.pnlSearch.Controls.Add(this.chkAdState_40);
            this.pnlSearch.Controls.Add(this.cbSearchMedia);
            this.pnlSearch.Controls.Add(this.pnlSearchBtn);
            this.pnlSearch.Controls.Add(this.lbMsg);
            this.pnlSearch.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlSearch.Location = new System.Drawing.Point(0, 0);
            this.pnlSearch.Name = "pnlSearch";
            this.pnlSearch.Size = new System.Drawing.Size(1008, 41);
            this.pnlSearch.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(178, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(57, 16);
            this.label1.TabIndex = 44;
            this.label1.Text = "Æí¼º»óÅÂ";
            // 
            // chkAdState_20
            // 
            this.chkAdState_20.Checked = true;
            this.chkAdState_20.CheckedValue = "";
            this.chkAdState_20.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkAdState_20.ForeColor = System.Drawing.SystemColors.ControlText;
            this.chkAdState_20.Location = new System.Drawing.Point(534, 13);
            this.chkAdState_20.Name = "chkAdState_20";
            this.chkAdState_20.Size = new System.Drawing.Size(44, 16);
            this.chkAdState_20.TabIndex = 14;
            this.chkAdState_20.Text = "Æí¼º";
            this.chkAdState_20.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            // 
            // lbAdState
            // 
            this.lbAdState.Location = new System.Drawing.Point(470, 13);
            this.lbAdState.Name = "lbAdState";
            this.lbAdState.Size = new System.Drawing.Size(57, 16);
            this.lbAdState.TabIndex = 17;
            this.lbAdState.Text = "±¤°í»óÅÂ";
            // 
            // chkAdState_30
            // 
            this.chkAdState_30.BackColor = System.Drawing.SystemColors.Window;
            this.chkAdState_30.Checked = true;
            this.chkAdState_30.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkAdState_30.Location = new System.Drawing.Point(598, 13);
            this.chkAdState_30.Name = "chkAdState_30";
            this.chkAdState_30.Size = new System.Drawing.Size(44, 16);
            this.chkAdState_30.TabIndex = 15;
            this.chkAdState_30.Text = "ÁßÁö";
            this.chkAdState_30.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            // 
            // chkAdState_40
            // 
            this.chkAdState_40.Location = new System.Drawing.Point(662, 13);
            this.chkAdState_40.Name = "chkAdState_40";
            this.chkAdState_40.Size = new System.Drawing.Size(44, 16);
            this.chkAdState_40.TabIndex = 16;
            this.chkAdState_40.Text = "Á¾·á";
            this.chkAdState_40.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            // 
            // cbSearchMedia
            // 
            this.cbSearchMedia.ComboStyle = Janus.Windows.EditControls.ComboStyle.DropDownList;
            this.cbSearchMedia.Location = new System.Drawing.Point(8, 10);
            this.cbSearchMedia.Name = "cbSearchMedia";
            this.cbSearchMedia.Size = new System.Drawing.Size(152, 23);
            this.cbSearchMedia.TabIndex = 1;
            this.cbSearchMedia.Text = "¸ÅÃ¼¼±ÅÃ";
            this.cbSearchMedia.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            // 
            // pnlSearchBtn
            // 
            this.pnlSearchBtn.BackColor = System.Drawing.SystemColors.Window;
            this.pnlSearchBtn.Controls.Add(this.btnSearch);
            this.pnlSearchBtn.Dock = System.Windows.Forms.DockStyle.Right;
            this.pnlSearchBtn.Location = new System.Drawing.Point(888, 0);
            this.pnlSearchBtn.Name = "pnlSearchBtn";
            this.pnlSearchBtn.Size = new System.Drawing.Size(120, 41);
            this.pnlSearchBtn.TabIndex = 3;
            // 
            // btnSearch
            // 
            this.btnSearch.Enabled = false;
            this.btnSearch.Location = new System.Drawing.Point(8, 8);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(104, 24);
            this.btnSearch.TabIndex = 2;
            this.btnSearch.Text = "Á¶ È¸";
            this.btnSearch.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // lbMsg
            // 
            this.lbMsg.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lbMsg.Location = new System.Drawing.Point(238, 10);
            this.lbMsg.Name = "lbMsg";
            this.lbMsg.Size = new System.Drawing.Size(192, 22);
            this.lbMsg.TabIndex = 43;
            this.lbMsg.Text = "Æí¼º";
            this.lbMsg.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // uiPanelChooseAdSchedule
            // 
            this.uiPanelChooseAdSchedule.AllowResize = Janus.Windows.UI.InheritableBoolean.True;
            this.uiPanelChooseAdSchedule.CaptionStyle = Janus.Windows.UI.Dock.PanelCaptionStyle.Dark;
            this.uiPanelChooseAdSchedule.CaptionVisible = Janus.Windows.UI.InheritableBoolean.False;
            this.uiPanelChooseAdSchedule.GroupStyle = Janus.Windows.UI.Dock.PanelGroupStyle.VerticalTiles;
            this.uiPanelChooseAdSchedule.Location = new System.Drawing.Point(0, 69);
            this.uiPanelChooseAdSchedule.Name = "uiPanelChooseAdSchedule";
            this.uiPanelChooseAdSchedule.Size = new System.Drawing.Size(1010, 268);
            this.uiPanelChooseAdSchedule.TabIndex = 4;
            this.uiPanelChooseAdSchedule.Text = "¸Þ´º/Ã¤³Î ¸ñ·Ï";
            // 
            // uiPanel1
            // 
            this.uiPanel1.CaptionStyle = Janus.Windows.UI.Dock.PanelCaptionStyle.Light;
            this.uiPanel1.CaptionVisible = Janus.Windows.UI.InheritableBoolean.False;
            this.uiPanel1.InnerContainer = this.uiPanel1Container;
            this.uiPanel1.Location = new System.Drawing.Point(0, 0);
            this.uiPanel1.Name = "uiPanel1";
            this.uiPanel1.Size = new System.Drawing.Size(357, 268);
            this.uiPanel1.TabIndex = 4;
            this.uiPanel1.Text = "¸Þ´º";
            // 
            // uiPanel1Container
            // 
            this.uiPanel1Container.Controls.Add(this.grdExCategenList);
            this.uiPanel1Container.Location = new System.Drawing.Point(1, 1);
            this.uiPanel1Container.Name = "uiPanel1Container";
            this.uiPanel1Container.Size = new System.Drawing.Size(355, 266);
            this.uiPanel1Container.TabIndex = 0;
            // 
            // grdExCategenList
            // 
            this.grdExCategenList.AllowEdit = Janus.Windows.GridEX.InheritableBoolean.False;
            this.grdExCategenList.AlternatingColors = true;
            this.grdExCategenList.AlternatingRowFormatStyle.BackgroundImageDrawMode = Janus.Windows.GridEX.BackgroundImageDrawMode.Tile;
            this.grdExCategenList.AutomaticSort = false;
            this.grdExCategenList.BorderStyle = Janus.Windows.GridEX.BorderStyle.None;
            this.grdExCategenList.DataSource = this.dvCateGen;
            grdExCategenList_DesignTimeLayout.LayoutString = resources.GetString("grdExCategenList_DesignTimeLayout.LayoutString");
            this.grdExCategenList.DesignTimeLayout = grdExCategenList_DesignTimeLayout;
            this.grdExCategenList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdExCategenList.EditorsControlStyle.ButtonAppearance = Janus.Windows.GridEX.ButtonAppearance.Regular;
            this.grdExCategenList.EmptyRows = true;
            this.grdExCategenList.FocusCellFormatStyle.BackColor = System.Drawing.SystemColors.Highlight;
            this.grdExCategenList.FocusCellFormatStyle.ForeColor = System.Drawing.Color.White;
            this.grdExCategenList.FocusStyle = Janus.Windows.GridEX.FocusStyle.None;
            this.grdExCategenList.Font = new System.Drawing.Font("¸¼Àº °íµñ", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.grdExCategenList.FrozenColumns = 2;
            this.grdExCategenList.GridLineColor = System.Drawing.Color.Silver;
            this.grdExCategenList.GridLines = Janus.Windows.GridEX.GridLines.Vertical;
            this.grdExCategenList.GridLineStyle = Janus.Windows.GridEX.GridLineStyle.Solid;
            this.grdExCategenList.GroupByBoxVisible = false;
            this.grdExCategenList.GroupMode = Janus.Windows.GridEX.GroupMode.Collapsed;
            this.grdExCategenList.GroupRowVisualStyle = Janus.Windows.GridEX.GroupRowVisualStyle.Outlook2003;
            this.grdExCategenList.HeaderFormatStyle.TextAlignment = Janus.Windows.GridEX.TextAlignment.Center;
            this.grdExCategenList.HideSelection = Janus.Windows.GridEX.HideSelection.HighlightInactive;
            grdExCategenList_Layout_0.Key = "bea";
            this.grdExCategenList.Layouts.AddRange(new Janus.Windows.GridEX.GridEXLayout[] {
            grdExCategenList_Layout_0});
            this.grdExCategenList.Location = new System.Drawing.Point(0, 0);
            this.grdExCategenList.Name = "grdExCategenList";
            this.grdExCategenList.ScrollBars = Janus.Windows.GridEX.ScrollBars.Vertical;
            this.grdExCategenList.SelectedInactiveFormatStyle.BackColor = System.Drawing.Color.Gold;
            this.grdExCategenList.Size = new System.Drawing.Size(355, 266);
            this.grdExCategenList.TabIndex = 3;
            this.grdExCategenList.TabKeyBehavior = Janus.Windows.GridEX.TabKeyBehavior.ControlNavigation;
            this.grdExCategenList.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            this.grdExCategenList.SelectionChanged += new System.EventHandler(this.OnGrdRowChangedGanre);
            this.grdExCategenList.Enter += new System.EventHandler(this.OnGrdRowChangedGanre);
            // 
            // dvCateGen
            // 
            this.dvCateGen.Table = this.schMidAdDs.Categens;
            // 
            // schMidAdDs
            // 
            this.schMidAdDs.DataSetName = "SchMidAdDs";
            this.schMidAdDs.Locale = new System.Globalization.CultureInfo("en-US");
            this.schMidAdDs.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // uiPanel2
            // 
            this.uiPanel2.CaptionStyle = Janus.Windows.UI.Dock.PanelCaptionStyle.Light;
            this.uiPanel2.CaptionVisible = Janus.Windows.UI.InheritableBoolean.False;
            this.uiPanel2.InnerContainer = this.uiPanel2Container;
            this.uiPanel2.Location = new System.Drawing.Point(361, 0);
            this.uiPanel2.Name = "uiPanel2";
            this.uiPanel2.Size = new System.Drawing.Size(649, 268);
            this.uiPanel2.TabIndex = 4;
            this.uiPanel2.Text = "Ã¤³Î";
            // 
            // uiPanel2Container
            // 
            this.uiPanel2Container.Controls.Add(this.grdExChooseAdScheduleList);
            this.uiPanel2Container.Location = new System.Drawing.Point(1, 1);
            this.uiPanel2Container.Name = "uiPanel2Container";
            this.uiPanel2Container.Size = new System.Drawing.Size(647, 266);
            this.uiPanel2Container.TabIndex = 0;
            // 
            // grdExChooseAdScheduleList
            // 
            this.grdExChooseAdScheduleList.AllowEdit = Janus.Windows.GridEX.InheritableBoolean.False;
            this.grdExChooseAdScheduleList.AlternatingColors = true;
            this.grdExChooseAdScheduleList.BorderStyle = Janus.Windows.GridEX.BorderStyle.None;
            this.grdExChooseAdScheduleList.DataSource = this.dvChannelSet;
            grdExChooseAdScheduleList_DesignTimeLayout_Reference_0.Instance = ((object)(resources.GetObject("grdExChooseAdScheduleList_DesignTimeLayout_Reference_0.Instance")));
            grdExChooseAdScheduleList_DesignTimeLayout.LayoutReferences.AddRange(new Janus.Windows.Common.Layouts.JanusLayoutReference[] {
            grdExChooseAdScheduleList_DesignTimeLayout_Reference_0});
            grdExChooseAdScheduleList_DesignTimeLayout.LayoutString = resources.GetString("grdExChooseAdScheduleList_DesignTimeLayout.LayoutString");
            this.grdExChooseAdScheduleList.DesignTimeLayout = grdExChooseAdScheduleList_DesignTimeLayout;
            this.grdExChooseAdScheduleList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdExChooseAdScheduleList.EditorsControlStyle.ButtonAppearance = Janus.Windows.GridEX.ButtonAppearance.Regular;
            this.grdExChooseAdScheduleList.EmptyRows = true;
            this.grdExChooseAdScheduleList.FocusCellFormatStyle.BackColor = System.Drawing.SystemColors.Highlight;
            this.grdExChooseAdScheduleList.FocusCellFormatStyle.ForeColor = System.Drawing.Color.White;
            this.grdExChooseAdScheduleList.FocusStyle = Janus.Windows.GridEX.FocusStyle.None;
            this.grdExChooseAdScheduleList.Font = new System.Drawing.Font("¸¼Àº °íµñ", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.grdExChooseAdScheduleList.GridLineColor = System.Drawing.Color.Silver;
            this.grdExChooseAdScheduleList.GridLines = Janus.Windows.GridEX.GridLines.Vertical;
            this.grdExChooseAdScheduleList.GridLineStyle = Janus.Windows.GridEX.GridLineStyle.Solid;
            this.grdExChooseAdScheduleList.GroupByBoxVisible = false;
            this.grdExChooseAdScheduleList.HeaderFormatStyle.TextAlignment = Janus.Windows.GridEX.TextAlignment.Center;
            this.grdExChooseAdScheduleList.HideSelection = Janus.Windows.GridEX.HideSelection.HighlightInactive;
            this.grdExChooseAdScheduleList.Location = new System.Drawing.Point(0, 0);
            this.grdExChooseAdScheduleList.Name = "grdExChooseAdScheduleList";
            this.grdExChooseAdScheduleList.ScrollBars = Janus.Windows.GridEX.ScrollBars.Vertical;
            this.grdExChooseAdScheduleList.SelectedInactiveFormatStyle.BackColor = System.Drawing.Color.Gold;
            this.grdExChooseAdScheduleList.Size = new System.Drawing.Size(647, 266);
            this.grdExChooseAdScheduleList.TabIndex = 4;
            this.grdExChooseAdScheduleList.TabKeyBehavior = Janus.Windows.GridEX.TabKeyBehavior.ControlNavigation;
            this.grdExChooseAdScheduleList.ThemedAreas = ((Janus.Windows.GridEX.ThemedArea)(((((((((Janus.Windows.GridEX.ThemedArea.ScrollBars | Janus.Windows.GridEX.ThemedArea.EditControls) 
            | Janus.Windows.GridEX.ThemedArea.Headers) 
            | Janus.Windows.GridEX.ThemedArea.GroupByBox) 
            | Janus.Windows.GridEX.ThemedArea.GroupRows) 
            | Janus.Windows.GridEX.ThemedArea.ControlBorder) 
            | Janus.Windows.GridEX.ThemedArea.Cards) 
            | Janus.Windows.GridEX.ThemedArea.Gridlines) 
            | Janus.Windows.GridEX.ThemedArea.CheckBoxes)));
            this.grdExChooseAdScheduleList.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            this.grdExChooseAdScheduleList.ColumnButtonClick += new Janus.Windows.GridEX.ColumnActionEventHandler(this.grdExChooseAdScheduleList_ColumnButtonClick);
            this.grdExChooseAdScheduleList.SelectionChanged += new System.EventHandler(this.OnGrdRowChangedChannel);
            this.grdExChooseAdScheduleList.Enter += new System.EventHandler(this.OnGrdRowChangedChannel);
            // 
            // dvChannelSet
            // 
            this.dvChannelSet.Table = this.schMidAdDs.ChannelSets;
            // 
            // uiPanelList
            // 
            this.uiPanelList.ActiveCaptionMode = Janus.Windows.UI.Dock.ActiveCaptionMode.Never;
            this.uiPanelList.AllowResize = Janus.Windows.UI.InheritableBoolean.True;
            this.uiPanelList.BackColor = System.Drawing.SystemColors.Window;
            this.uiPanelList.CaptionFormatStyle.FontBold = Janus.Windows.UI.TriState.True;
            this.uiPanelList.CaptionFormatStyle.FontSize = 9F;
            this.uiPanelList.CaptionHeight = 26;
            this.uiPanelList.CaptionStyle = Janus.Windows.UI.Dock.PanelCaptionStyle.Light;
            this.uiPanelList.InnerContainer = this.uiPanelListContainer;
            this.uiPanelList.Location = new System.Drawing.Point(0, 341);
            this.uiPanelList.Name = "uiPanelList";
            this.uiPanelList.Size = new System.Drawing.Size(1010, 289);
            this.uiPanelList.TabIndex = 4;
            this.uiPanelList.Text = "¸Þ´ºº°Æí¼ºÇöÈ²";
            // 
            // uiPanelListContainer
            // 
            this.uiPanelListContainer.Controls.Add(this.pnlUserDetail);
            this.uiPanelListContainer.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.uiPanelListContainer.Location = new System.Drawing.Point(1, 26);
            this.uiPanelListContainer.Name = "uiPanelListContainer";
            this.uiPanelListContainer.Size = new System.Drawing.Size(1008, 262);
            this.uiPanelListContainer.TabIndex = 0;
            // 
            // pnlUserDetail
            // 
            this.pnlUserDetail.BackColor = System.Drawing.SystemColors.Window;
            this.pnlUserDetail.Controls.Add(this.grdExScheduleList);
            this.pnlUserDetail.Controls.Add(this.btnGenreName);
            this.pnlUserDetail.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlUserDetail.Location = new System.Drawing.Point(0, 0);
            this.pnlUserDetail.Name = "pnlUserDetail";
            this.pnlUserDetail.Size = new System.Drawing.Size(1008, 262);
            this.pnlUserDetail.TabIndex = 3;
            // 
            // grdExScheduleList
            // 
            this.grdExScheduleList.AutomaticSort = false;
            this.grdExScheduleList.BorderStyle = Janus.Windows.GridEX.BorderStyle.None;
            this.grdExScheduleList.DataSource = this.dvChooseAdSchedule;
            grdExScheduleList_DesignTimeLayout.LayoutString = resources.GetString("grdExScheduleList_DesignTimeLayout.LayoutString");
            this.grdExScheduleList.DesignTimeLayout = grdExScheduleList_DesignTimeLayout;
            this.grdExScheduleList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdExScheduleList.EditorsControlStyle.ButtonAppearance = Janus.Windows.GridEX.ButtonAppearance.Regular;
            this.grdExScheduleList.EmptyRows = true;
            this.grdExScheduleList.FocusCellFormatStyle.BackColor = System.Drawing.SystemColors.Highlight;
            this.grdExScheduleList.FocusCellFormatStyle.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.grdExScheduleList.FocusStyle = Janus.Windows.GridEX.FocusStyle.None;
            this.grdExScheduleList.Font = new System.Drawing.Font("¸¼Àº °íµñ", 8.25F);
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
            this.grdExScheduleList.Size = new System.Drawing.Size(1008, 262);
            this.grdExScheduleList.TabIndex = 5;
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
            this.grdExScheduleList.SelectionChanged += new System.EventHandler(this.OnGrdRowChangedSchedule);
            // 
            // dvChooseAdSchedule
            // 
            this.dvChooseAdSchedule.Table = this.schMidAdDs.ChooseAdSchedule;
            // 
            // btnGenreName
            // 
            this.btnGenreName.BackColor = System.Drawing.SystemColors.Window;
            this.btnGenreName.Location = new System.Drawing.Point(808, 8);
            this.btnGenreName.Name = "btnGenreName";
            this.btnGenreName.Size = new System.Drawing.Size(26, 21);
            this.btnGenreName.TabIndex = 6;
            this.btnGenreName.UseVisualStyleBackColor = false;
            // 
            // uiPanelDetail
            // 
            this.uiPanelDetail.AllowResize = Janus.Windows.UI.InheritableBoolean.False;
            this.uiPanelDetail.CaptionStyle = Janus.Windows.UI.Dock.PanelCaptionStyle.Dark;
            this.uiPanelDetail.CaptionVisible = Janus.Windows.UI.InheritableBoolean.False;
            this.uiPanelDetail.InnerContainer = this.uiPanelDetailContainer;
            this.uiPanelDetail.Location = new System.Drawing.Point(0, 634);
            this.uiPanelDetail.Name = "uiPanelDetail";
            this.uiPanelDetail.Size = new System.Drawing.Size(1010, 43);
            this.uiPanelDetail.TabIndex = 4;
            this.uiPanelDetail.Text = "Æí¼º";
            // 
            // uiPanelDetailContainer
            // 
            this.uiPanelDetailContainer.Controls.Add(this.pnlDetail);
            this.uiPanelDetailContainer.Location = new System.Drawing.Point(1, 1);
            this.uiPanelDetailContainer.Name = "uiPanelDetailContainer";
            this.uiPanelDetailContainer.Size = new System.Drawing.Size(1008, 41);
            this.uiPanelDetailContainer.TabIndex = 0;
            // 
            // pnlDetail
            // 
            this.pnlDetail.BackColor = System.Drawing.SystemColors.Window;
            this.pnlDetail.Controls.Add(this.btnDelete);
            this.pnlDetail.Controls.Add(this.btnAdd_16);
            this.pnlDetail.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlDetail.Location = new System.Drawing.Point(0, 0);
            this.pnlDetail.Name = "pnlDetail";
            this.pnlDetail.Size = new System.Drawing.Size(1008, 41);
            this.pnlDetail.TabIndex = 6;
            // 
            // btnDelete
            // 
            this.btnDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnDelete.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnDelete.Enabled = false;
            this.btnDelete.FlatBorderColor = System.Drawing.Color.DimGray;
            this.btnDelete.Location = new System.Drawing.Point(78, 11);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(60, 22);
            this.btnDelete.StateStyles.FormatStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(111)))), ((int)(((byte)(169)))));
            this.btnDelete.StateStyles.FormatStyle.BackColorAlphaMode = Janus.Windows.UI.AlphaMode.UseAlpha;
            this.btnDelete.StateStyles.FormatStyle.BackColorGradient = System.Drawing.Color.FromArgb(((int)(((byte)(205)))), ((int)(((byte)(6)))), ((int)(((byte)(48)))));
            this.btnDelete.StateStyles.FormatStyle.BackgroundGradientMode = Janus.Windows.UI.BackgroundGradientMode.Vertical;
            this.btnDelete.StateStyles.FormatStyle.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.btnDelete.TabIndex = 8;
            this.btnDelete.Text = "»è Á¦";
            this.btnDelete.UseThemes = false;
            this.btnDelete.VisualStyle = Janus.Windows.UI.VisualStyle.Office2003;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnAdd_16
            // 
            this.btnAdd_16.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnAdd_16.Location = new System.Drawing.Point(12, 10);
            this.btnAdd_16.Name = "btnAdd_16";
            this.btnAdd_16.Size = new System.Drawing.Size(60, 22);
            this.btnAdd_16.TabIndex = 10;
            this.btnAdd_16.Text = "Æí ¼º";
            this.btnAdd_16.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.btnAdd_16.Click += new System.EventHandler(this.btnAdd_16_Click);
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
            // SchMidAdControl
            // 
            this.Controls.Add(this.uiPanelUsers);
            this.Font = new System.Drawing.Font("¸¼Àº °íµñ", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.Name = "SchMidAdControl";
            this.Size = new System.Drawing.Size(1010, 677);
            this.Load += new System.EventHandler(this.ChooseAdScheduleControl_Load);
            ((System.ComponentModel.ISupportInitialize)(this.uiPM)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelUsers)).EndInit();
            this.uiPanelUsers.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelSearch)).EndInit();
            this.uiPanelSearch.ResumeLayout(false);
            this.uiPanelSearchContainer.ResumeLayout(false);
            this.pnlSearch.ResumeLayout(false);
            this.pnlSearchBtn.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelChooseAdSchedule)).EndInit();
            this.uiPanelChooseAdSchedule.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.uiPanel1)).EndInit();
            this.uiPanel1.ResumeLayout(false);
            this.uiPanel1Container.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdExCategenList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dvCateGen)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.schMidAdDs)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanel2)).EndInit();
            this.uiPanel2.ResumeLayout(false);
            this.uiPanel2Container.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdExChooseAdScheduleList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dvChannelSet)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelList)).EndInit();
            this.uiPanelList.ResumeLayout(false);
            this.uiPanelListContainer.ResumeLayout(false);
            this.pnlUserDetail.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdExScheduleList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dvChooseAdSchedule)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelDetail)).EndInit();
            this.uiPanelDetail.ResumeLayout(false);
            this.uiPanelDetailContainer.ResumeLayout(false);
            this.pnlDetail.ResumeLayout(false);
            this.ResumeLayout(false);

        }
        #endregion

        #region ÄÁÆ®·Ñ ·Îµå
        private void ChooseAdScheduleControl_Load(object sender, System.EventArgs e)
        {
            // µ¥ÀÌÅÍ°ü¸®¿ë °´Ã¼»ý¼º
            dtMenu= ((DataView)grdExCategenList.DataSource).Table;
            cmMenu= (CurrencyManager) this.BindingContext[grdExCategenList.DataSource]; 

			cmChannel = (CurrencyManager) this.BindingContext[grdExChooseAdScheduleList.DataSource]; 
			dtChannel  = ((DataView)grdExChooseAdScheduleList.DataSource).Table;
			
			cmSchedule = (CurrencyManager) this.BindingContext[grdExScheduleList.DataSource]; 
			dtSchedule = ((DataView)grdExScheduleList.DataSource).Table;

			// ÄÁÆ®·Ñ ÃÊ±âÈ­
			InitControl();
		}

        #endregion

        #region ÄÁÆ®·Ñ ÃÊ±âÈ­
        private void InitControl()
        {
            ProgressStart();

			lbMsg.Text = "";

            InitCombo_Media();
            InitCombo_Level();

			if(menu.CanCreate(MenuCode))    canCreate   = true;
            if(menu.CanRead(MenuCode))      canRead     = true;
			if(menu.CanUpdate(MenuCode))    canUpdate   = true;
			if(menu.CanDelete(MenuCode))    canDelete   = true;

			InitButton();
			ProgressStop();
			if(canRead) SearchMenu();
        }

        private void InitCombo_Media()
        {			
            MediaCodeModel mediacodeModel = new MediaCodeModel();		
            new MediaCodeManager(systemModel, commonModel).GetMediaCodeList(mediacodeModel);
			
            if (mediacodeModel.ResultCD.Equals("0000"))
            {
                // µ¥ÀÌÅÍ¼Â¿¡ ¼ÂÆÃ
                Utility.SetDataTable(schMidAdDs.Medias, mediacodeModel.MediaCodeDataSet);				
                
            }


            // °Ë»öÁ¶°ÇÀÇ ÄÞº¸
            this.cbSearchMedia.Items.Clear();
			
            // ÄÞº¸¹Ú½º¿¡ ¼ÂÆ®ÇÒ ÄÚµå¸ñ·ÏÀ» ´ãÀ» Item¹è¿­À» ¼±¾ð
            Janus.Windows.EditControls.UIComboBoxItem[] comboItems = new Janus.Windows.EditControls.UIComboBoxItem[mediacodeModel.ResultCnt + 1];

            comboItems[0] = new Janus.Windows.EditControls.UIComboBoxItem("¸ÅÃ¼¼±ÅÃ","00");
			
            for(int i=0;i<mediacodeModel.ResultCnt;i++)
            {
                DataRow row = schMidAdDs.Medias.Rows[i];

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
				for(int i=0;i < schMidAdDs.Medias.Rows.Count;i++)
				{
					DataRow row = schMidAdDs.Medias.Rows[i];					
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
			if(canRead)   btnSearch.Enabled = true;

            Application.DoEvents();
        }

        private void DisableButton()
        {
            btnSearch.Enabled = false;
            btnAdd_16.Enabled = false;
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
        private void OnGrdRowChangedGanre(object sender, System.EventArgs e) 
        {
            if (!IsSearching) // 2011.11.29 JH.Park Á¶È¸ÁßÀÌ ¾Æ´Ò°æ¿ì¿¡¸¸ µ¿ÀÛÇÏµµ·Ï º¯°æ
            {
                if (IsNotLoading)
                {
                    SetDetailTextMenu();
                    InitButton();
                }
            }
        }

		
        private void OnGrdRowChangedChannel(object sender, System.EventArgs e) 
        {
            if (!IsSearching) // 2011.11.29 JH.Park Á¶È¸ÁßÀÌ ¾Æ´Ò°æ¿ì¿¡¸¸ µ¿ÀÛÇÏµµ·Ï º¯°æ
            {
                if (IsNotLoading)
                {
                    SetDetailTextChannel();
                    InitButton();
                }
            }
		}

		private void OnGrdRowChangedSchedule(object sender, System.EventArgs e) 
		{
            if (!IsSearching) // 2011.11.29 JH.Park Á¶È¸ÁßÀÌ ¾Æ´Ò°æ¿ì¿¡¸¸ µ¿ÀÛÇÏµµ·Ï º¯°æ
            {
                SetDetailTextSchedule();
            }
		}
 

        /// <summary>
        /// Á¶È¸¹öÆ° Å¬¸¯
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSearch_Click(object sender, System.EventArgs e)
        {			
            ResetDetail();
            DisableButton();
            SearchMenu();			
            InitButton();
        }

		private void btnAdd_Click(object sender, System.EventArgs e)
		{
			//  ÁöÁ¤±¤°í ´ë»ó¸ñ·Ï °Ë»öÃ¢ 
			ChooseAdSearch_pForm pForm = new ChooseAdSearch_pForm(this);
			
			switch(keyCmType)
			{
				case "M":
					pForm.ScheduleType = TYPE_Schedule.Genre;
					pForm.keyMediaCode = keyMediaCode;
					pForm.keyGenreCode = keyGenreCode;
					break;
				case "C":
					pForm.ScheduleType = TYPE_Schedule.Channel;
					pForm.keyMediaCode = keyMediaCode;
					pForm.keyChannelNo = keyChannelNo;
					break;
			}
			pForm.ShowDialog();            
			pForm.Dispose();
			pForm = null;
		}


		private void btnDelete_Click(object sender, System.EventArgs e)
		{
			switch(keyCmType)
			{
				case "M":
					DeleteChoiceAdMenuDetail();		
					break;
				case "C":
					DeleteChoiceAdChannelDetail();		
					break;
			}
		}


		private void btnOrderFirst_Click(object sender, System.EventArgs e)
		{
			switch(keyCmType)
			{
				case "M":
					OrderSetScheduleMenuAd(ORDER_FIRST);
					break;
				case "C":
					OrderSetScheduleChannelAd(ORDER_FIRST);
					break;
			}		
		}


		private void btnOrderUp_Click(object sender, System.EventArgs e)
		{
			switch(keyCmType)
			{
				case "M":
					OrderSetScheduleMenuAd(ORDER_UP);
					break;
				case "C":
					OrderSetScheduleChannelAd(ORDER_UP);
					break;
			}				
		}

		private void btnOrderDown_Click(object sender, System.EventArgs e)
		{
			switch(keyCmType)
			{
				case "M":
					OrderSetScheduleMenuAd(ORDER_DOWN);
					break;
				case "C":
					OrderSetScheduleChannelAd(ORDER_DOWN);
					break;
			}				
		}

		private void btnOrderLast_Click(object sender, System.EventArgs e)
		{
			switch(keyCmType)
			{
				case "M":
					OrderSetScheduleMenuAd(ORDER_LAST);
					break;
				case "C":
					OrderSetScheduleChannelAd(ORDER_LAST);
					break;
			}				
		}

		private void grdExScheduleList_ColumnHeaderClick(object sender, Janus.Windows.GridEX.ColumnActionEventArgs e)
		{

			//ÄÃ·³Index 0(Ã¼Å©¹Ú½ºÄÃ·³ÀÌ)ÀÌ ¾Æ´Ï¸é ºüÁ®³ª°¡°Ô Ã³¸®.
			if(e.Column.Index != 0)
			{
				return;
			}
            
			//ColumnHeader Click½Ã¿¡ dt Setting 
			DataRow[] foundRows = dtSchedule.Select("CheckYn = 'False'");
         
			// if(grdExItemList.CurrentColumn.Position == 0){
			if( foundRows.Length == 0 )
			{
				for(int i=0;i < dtSchedule.Rows.Count;i++)
				{
					dtSchedule.Rows[i].BeginEdit();
					dtSchedule.Rows[i]["CheckYn"]="False";
					dtSchedule.Rows[i].EndEdit();
				}
			}
			else
			{
				for(int i=0;i < dtSchedule.Rows.Count;i++)
				{
					dtSchedule.Rows[i].BeginEdit();
					dtSchedule.Rows[i]["CheckYn"]="True";
					dtSchedule.Rows[i].EndEdit();
				}
			}
		}

   
        #endregion

        #region Ã³¸®¸Þ¼Òµå

        /// <summary>
        /// Ã¤³Î/¸Þ´º Æí¼ºÇöÈ² ¸ñ·Ï Á¶È¸
        /// </summary>
        private void SearchMenu()
        {
            IsSearching = true;

            StatusMessage("¸Þ´º Á¤º¸¸¦ Á¶È¸ÇÕ´Ï´Ù.");

			if(cbSearchMedia.SelectedItem.Value.Equals("00")) 
			{
				MessageBox.Show("¸ÅÃ¼¸¦ ¼±ÅÃÇÏ¿© ÁÖ½Ã±â ¹Ù¶ø´Ï´Ù.","Áß°£±¤°í Æí¼ºÇöÈ² Á¶È¸", MessageBoxButtons.OK, MessageBoxIcon.Information );
				return;
			}

			ProgressStart();
            try
            {
                #region [ Æí¼º½ÂÀÎ Á¤º¸ Á¶È¸ ]
				// Æí¼º¹èÆ÷½ÂÀÎ Ã³¸®»óÅÂ¸¦ Á¶È¸ÇÑ´Ù.
				keyAckNo    = "";
				keyAckState = "";

				schPublishModel.Init();
				schPublishModel.SearchMediaCode		 =  cbSearchMedia.SelectedItem.Value.ToString(); 

				// ÇöÀç ½ÂÀÎ»óÅÂÁ¶È¸ ¼­ºñ½º¸¦ È£ÃâÇÑ´Ù.
				new SchPublishManager(systemModel,commonModel).GetPublishState(schPublishModel,10);

				ProgressStop();

				if (schPublishModel.ResultCD.Equals("0000"))
				{
					keyAckNo    = schPublishModel.AckNo;
					keyAckState = schPublishModel.State;

					if(keyAckState.Equals("10"))	// ½ÂÀÎ»óÅÂ°¡ 10:Æí¼ºÁßÀÌ¸é
					{
						lbMsg.Text = "Æí¼º ÁøÇàÁß.";
					}
					else if(keyAckState.Equals("20")) // ½ÂÀÎ»óÅÂ°¡ 20:Æí¼º½ÂÀÎ »óÅÂÀÌ¸é Æí¼ºÀÌ ºÒ°¡ÇÏ´Ù.
					{
						lbMsg.Text = "°Ë¼ö½ÂÀÎ ´ë±âÁß.";
						canCreate = false;
						canUpdate = false;
						canDelete = false;
					}
					else if(keyAckState.Equals("25")) // ½ÂÀÎ»óÅÂ°¡ 25:¹èÆ÷´ë±â »óÅÂÀÌ¸é Æí¼ºÀÌ ºÒ°¡ÇÏ´Ù.
					{
						lbMsg.Text = "¹èÆ÷½ÂÀÎ ´ë±âÁß";
						canCreate = false;
						canUpdate = false;
						canDelete = false;
					}
					else if(keyAckState.Equals("30")) // ½ÂÀÎ»óÅÂ°¡ 30:¹èÆ÷½ÂÀÎ »óÅÂÀÌ¸é ½Å±ÔÆí¼ºÀÌ °¡´ÉÇÏ´Ù.
					{
						lbMsg.Text = "Æí¼º¿Ï·á.";
					}
				}
                #endregion

                chooseAdScheduleModel.Init();
				
				// µ¥ÀÌÅÍ Å¬¸®¾î
				schMidAdDs.Categens.Clear();
				schMidAdDs.ChannelSets.Clear();   
				schMidAdDs.ChooseAdSchedule.Clear();  
				ResetDetail();

                // µ¥ÀÌÅÍ¸ðµ¨¿¡ Àü¼ÛÇÒ ³»¿ëÀ» ¼ÂÆ®ÇÑ´Ù.				
                chooseAdScheduleModel.SearchMediaCode = cbSearchMedia.SelectedValue.ToString();

                // Àå¸£¸Þ´º Á¶È¸ ¼­ºñ½º¸¦ È£ÃâÇÑ´Ù.
                new SchMidAdManager(systemModel,commonModel).GetMenuList(chooseAdScheduleModel);

                if (chooseAdScheduleModel.ResultCD.Equals("0000"))
                {
                    Utility.SetDataTable(schMidAdDs.Categens, chooseAdScheduleModel.ChooseAdScheduleDataSet);
                    StatusMessage(chooseAdScheduleModel.ResultCnt + "°ÇÀÇ ¸Þ´º Á¤º¸°¡ Á¶È¸µÇ¾ú½À´Ï´Ù.");
					grdExCategenList.Focus();
				}

				// ¸Þ½ÃÁö Ã³¸®
				if(keyAckState.Equals("20")) 
				{
					MessageBox.Show("ÇöÀç Æí¼º½ÂÀÎÈÄ °Ë¼ö½ÂÀÎ ´ë±â»óÅÂÀÌ¹Ç·Î Æí¼ºÀ» º¯°æÇÒ ¼ö ¾ø½À´Ï´Ù.", "Áß°£±¤°íÆí¼º",MessageBoxButtons.OK, MessageBoxIcon.Information);
				}
				else if(keyAckState.Equals("25")) 
				{
					MessageBox.Show("ÇöÀç °Ë¼ö½ÂÀÎÈÄ ¹èÆ÷½ÂÀÎ ´ë±â»óÅÂÀÌ¹Ç·Î Æí¼ºÀ» º¯°æÇÒ ¼ö ¾ø½À´Ï´Ù.", "Áß°£±¤°íÆí¼º",MessageBoxButtons.OK, MessageBoxIcon.Information);
				}

            }
            catch(FrameException fe)
            {
                FrameSystem.showMsgForm("¸Þ´º Á¶È¸¿À·ù", new string[] {fe.ErrCode, fe.ResultMsg});
            }
            catch(Exception ex)
            {
                FrameSystem.showMsgForm("¸Þ´º Á¶È¸¿À·ù",new string[] {"",ex.Message});
            }
			finally
			{
                IsSearching = false; // Á¶È¸Áß Flag ¸®¼Â
				ProgressStop();
			}
        }


		private void SetDetailTextMenu()
		{
			int curRow = cmMenu.Position;

			if(curRow >= 0 )
			{
				IsNotLoading = false;	// Á¶È¸Áß ´Ù½Ã Á¶È¸µÇ´Â °ÍÀ» ¹æÁöÇÔ.
				try
				{
					keyCmType       = "M";
					keyMediaCode    = dtMenu.Rows[curRow]["MediaCode"].ToString();
					keyCategoryCode = dtMenu.Rows[curRow]["CategoryCode"].ToString();
					keyGenreCode    = dtMenu.Rows[curRow]["GenreCode"].ToString();

					schMidAdDs.ChannelSets.Clear();   
					SearchChannelList();

					//schMidAdDs.ChooseAdSchedule.Clear();        
					//SearchChooseAdScheduleMenu();
				}
				finally
				{
					IsNotLoading = true;
				}
			}

			StatusMessage("ÁØºñ");
		}

		private void SetDetailTextChannel()
		{
			int curRow = cmChannel.Position;

			if(curRow >= 0 )
			{
				IsNotLoading = false;	// Á¶È¸Áß ´Ù½Ã Á¶È¸µÇ´Â °ÍÀ» ¹æÁöÇÔ.
				try
				{
					keyCmType       = "C";

					uiPanelList.Text = ""
						+ "Ã¤³Îº° Æí¼ºÇöÈ² : " 
						+ dtChannel.Rows[curRow]["MediaName"].ToString().Trim() + " || " 
						+ dtChannel.Rows[curRow]["CategoryCode"].ToString() + " " + dtChannel.Rows[curRow]["CategoryName"].ToString().Trim() + " || " 
						+ dtChannel.Rows[curRow]["GenreCode"].ToString()    + " " + dtChannel.Rows[curRow]["GenreName"].ToString().Trim() + " || " 
						+ dtChannel.Rows[curRow]["ChannelNo"].ToString().Trim() + " "
						+ dtChannel.Rows[curRow]["Title"].ToString().Trim() 
						;

					keyMediaCode    = dtChannel.Rows[curRow]["MediaCode"].ToString();
					keyCategoryCode = dtChannel.Rows[curRow]["CategoryCode"].ToString();
					keyGenreCode    = dtChannel.Rows[curRow]["GenreCode"].ToString();
					keyChannelNo    = dtChannel.Rows[curRow]["ChannelNo"].ToString();
                    keyTitle        = dtChannel.Rows[curRow]["Title"].ToString();

					schMidAdDs.ChooseAdSchedule.Clear();        
					SearchChooseAdScheduleChannel();
				}
				finally
				{
					IsNotLoading = true;
				}
			}		
		}

		private void SetDetailTextSchedule()
		{
			ResetDetail();

			int curRow = cmSchedule.Position;

			if(curRow >= 0 )
			{
				uiPanelDetail.Text = ""
					+ "Æí¼º±¤°í : " 
					+ dtSchedule.Rows[curRow]["CmName"].ToString().Trim() + "Æí¼º / " 
					+ dtSchedule.Rows[curRow]["AdTypeName"].ToString().Trim() + " / [" 
					+ dtSchedule.Rows[curRow]["ItemNo"].ToString() + "]"
					+ dtSchedule.Rows[curRow]["ItemName"].ToString().Trim()
					;

				keyItemNo        = dtSchedule.Rows[curRow]["ItemNo"].ToString();
				keyItemName      = dtSchedule.Rows[curRow]["ItemName"].ToString();
				keyScheduleOrder = dtSchedule.Rows[curRow]["ScheduleOrder"].ToString(); 
				

				string AdType =  dtSchedule.Rows[curRow]["AdType"].ToString();
				string CmType =  dtSchedule.Rows[curRow]["CmType"].ToString();

                if(canCreate)
                {
                    btnAdd_16.Enabled = true;
                }

				if(canDelete)
				{	
					if(keyCmType.Equals(CmType))
					{
						btnDelete.Enabled			= true;
					}
				}
			}
		}
    

    
        /// <summary>
        /// Ã¤³Î¸ñ·Ï Á¶È¸
        /// </summary>
        private void SearchChannelList()
        {
            StatusMessage("Ã¤³Î¸ñ·ÏÀ» Á¶È¸ÇÕ´Ï´Ù.");

            try
            {				
                chooseAdScheduleModel.Init();
                chooseAdScheduleModel.MediaCode    = keyMediaCode;
                chooseAdScheduleModel.CategoryCode = keyCategoryCode;
                chooseAdScheduleModel.GenreCode    = keyGenreCode;

				// Ã¤³Î¸ñ·ÏÁ¶È¸ ¼­ºñ½º¸¦ È£ÃâÇÑ´Ù.
                new SchMidAdManager(systemModel,commonModel).GetChannelList(chooseAdScheduleModel);

                if (chooseAdScheduleModel.ResultCD.Equals("0000"))
                {
                    Utility.SetDataTable(schMidAdDs.ChannelSets, chooseAdScheduleModel.ChooseAdScheduleDataSet);				
                    StatusMessage(chooseAdScheduleModel.ResultCnt + "°ÇÀÇ Ã¤³Î Á¤º¸°¡ Á¶È¸µÇ¾ú½À´Ï´Ù.");
                }
                
            }
            catch(FrameException fe)
            {
                FrameSystem.showMsgForm("Ã¤³ÎÁ¶È¸¿À·ù", new string[] {fe.ErrCode, fe.ResultMsg});
            }
            catch(Exception ex)
            {
                FrameSystem.showMsgForm("Ã¤³ÎÁ¶È¸¿À·ù",new string[] {"",ex.Message});
            }
        }
       
       
		//Ã¤³Î ±¤°íÆí¼ºÇöÈ²Á¶È¸
		private void SearchChooseAdScheduleChannel()
		{
			StatusMessage("Ã¤³Î Æí¼ºÇöÈ²À» Á¶È¸ÇÕ´Ï´Ù.");

			try
			{
				grdExScheduleList.UnCheckAllRecords();

				// µ¥ÀÌÅÍ¸ðµ¨ ÃÊ±âÈ­
				chooseAdScheduleModel.Init();

				// µ¥ÀÌÅÍ¸ðµ¨¿¡ Àü¼ÛÇÒ ³»¿ëÀ» ¼ÂÆ®ÇÑ´Ù.
     
				chooseAdScheduleModel.MediaCode		=  keyMediaCode;   
				chooseAdScheduleModel.GenreCode     =  keyGenreCode;
				chooseAdScheduleModel.ChannelNo     =  keyChannelNo;

                if(chkAdState_20.Checked) chooseAdScheduleModel.SearchchkAdState_20 = "Y";
                if(chkAdState_30.Checked) chooseAdScheduleModel.SearchchkAdState_30 = "Y";
                if(chkAdState_40.Checked) chooseAdScheduleModel.SearchchkAdState_40 = "Y";

				// ±¤°íÆÄÀÏ¸ñ·ÏÁ¶È¸ ¼­ºñ½º¸¦ È£ÃâÇÑ´Ù.
				new SchMidAdManager(systemModel,commonModel).GetScheduleListChannel(chooseAdScheduleModel);
				
				if (chooseAdScheduleModel.ResultCD.Equals("0000"))
				{
					Utility.SetDataTable(schMidAdDs.ChooseAdSchedule, chooseAdScheduleModel.ChooseAdScheduleDataSet);		
					StatusMessage(chooseAdScheduleModel.ResultCnt + "°ÇÀÇ ±¤°íÆÄÀÏ Á¤º¸°¡ Á¶È¸µÇ¾ú½À´Ï´Ù.");

					keyLastOrder = chooseAdScheduleModel.LastOrder;
					SetCurrentRowSchedule();
				}

				SetDetailTextSchedule();

			}
			catch(FrameException fe)
			{
				FrameSystem.showMsgForm("Ã¤³Î Æí¼ºÇöÈ² Á¶È¸¿À·ù", new string[] {fe.ErrCode, fe.ResultMsg});
			}
			catch(Exception ex)
			{
				FrameSystem.showMsgForm("Ã¤³Î Æí¼ºÇöÈ² Á¶È¸¿À·ù",new string[] {"",ex.Message});
			}

		}

		/// <summary>
		/// ¼±ÅÃµÈ Àå¸£¸¦ ÁöÁ¤¸Þ´º±¤°í »ó¼¼³»¿ªÀ» »èÁ¦
		/// </summary>
		private void DeleteChoiceAdMenuDetail()
		{
			StatusMessage("ÁöÁ¤¸Þ´º±¤°í Æí¼º³»¿ªÀ» »èÁ¦ÇÕ´Ï´Ù.");


			DialogResult result = MessageBox.Show("ÇØ´ç ÁöÁ¤¸Þ´º±¤°í Æí¼º³»¿ªÀ» »èÁ¦ ÇÏ½Ã°Ú½À´Ï±î?","ÁöÁ¤¸Þ´º±¤°í Æí¼º³»¿ª »èÁ¦",
				MessageBoxButtons.YesNo, MessageBoxIcon.Question,
				MessageBoxDefaultButton.Button2);

			if (result == DialogResult.No) return;

			// ±×¸®µå¿¡ º¯°æµÈ µ¥ÀÌÅÍ¸¦ Datasource¿¡ ¾÷µ¥ÀÌÆ® ÇÑ´Ù.
			grdExScheduleList.UpdateData();

			try
			{
				int SetCount = 0;

				// »èÁ¦ ½ÃÅ´
				for(int i = 0;i < schMidAdDs.ChooseAdSchedule.Rows.Count;i++)
				{
					DataRow row = schMidAdDs.ChooseAdSchedule.Rows[i];

					Debug.WriteLine( i.ToString() + ":" + row["CheckYn"].ToString() + "|" + row["ItemName"].ToString() );

					if(row["CheckYn"].ToString().Equals("True"))
					{
						schChoiceAdModel.Init();

						schChoiceAdModel.ItemNo    =  row["ItemNo"].ToString();
						schChoiceAdModel.MediaCode =  row["MediaCode"].ToString();
						schChoiceAdModel.GenreCode =  row["GenreCode"].ToString();

						new SchChoiceAdManager(systemModel,commonModel).SetSchChoiceMenuDetailDelete(schChoiceAdModel);

						if (schChoiceAdModel.ResultCD.Equals("0000"))
						{
							SetCount++;
						}	
					}
				}

				if(SetCount > 0)
				{
					ReloadList();
					StatusMessage("ÁöÁ¤¸Þ´º±¤°í Æí¼º»ó¼¼³»¿ªÀÌ »èÁ¦µÇ¾ú½À´Ï´Ù.");			
				}	
				else
				{
					MessageBox.Show("¼±ÅÃµÈ ÁöÁ¤¸Þ´º±¤°í Æí¼º³»¿ªÀÌ ¾ø½À´Ï´Ù.", "¸Þ´º±¤°íÆí¼º",
						MessageBoxButtons.OK, MessageBoxIcon.Information);
				}	
			}
			catch(FrameException fe)
			{
				FrameSystem.showMsgForm("ÁöÁ¤¸Þ´º±¤°í »ó¼¼Æí¼º »èÁ¦¿À·ù", new string[] {fe.ErrCode, fe.ResultMsg});
			}
			catch(Exception ex)
			{
				FrameSystem.showMsgForm("ÁöÁ¤¸Þ´º±¤°í »ó¼¼Æí¼º »èÁ¦¿À·ù",new string[] {"",ex.Message});
			}			
		}


		/// <summary>
		/// ¼±ÅÃµÈ ÁöÁ¤Ã¤³Î±¤°í »ó¼¼³»¿ªÀ» »èÁ¦
		/// </summary>
		private void DeleteChoiceAdChannelDetail()
		{
			StatusMessage("ÁöÁ¤Ã¤³Î±¤°í Æí¼º»ó¼¼³»¿ªÀ» »èÁ¦ÇÕ´Ï´Ù.");

			DialogResult result = MessageBox.Show("ÇØ´ç ÁöÁ¤Ã¤³Î±¤°í Æí¼º³»¿ªÀ» »èÁ¦ ÇÏ½Ã°Ú½À´Ï±î?","ÁöÁ¤Ã¤³Î±¤°í Æí¼º³»¿ª »èÁ¦",
				MessageBoxButtons.YesNo, MessageBoxIcon.Question,
				MessageBoxDefaultButton.Button2);

			if (result == DialogResult.No) return;

			// ±×¸®µå¿¡ º¯°æµÈ µ¥ÀÌÅÍ¸¦ Datasource¿¡ ¾÷µ¥ÀÌÆ® ÇÑ´Ù.
			grdExScheduleList.UpdateData();

			try
			{
				int SetCount = 0;

				// »èÁ¦ ½ÃÅ´
				for(int i = 0;i < schMidAdDs.ChooseAdSchedule.Rows.Count;i++)
				{
					DataRow row = schMidAdDs.ChooseAdSchedule.Rows[i];

					Debug.WriteLine( i.ToString() + ":" + row["CheckYn"].ToString() + "|" + row["ItemName"].ToString() );

					if(row["CheckYn"].ToString().Equals("True"))
					{

						schChoiceAdModel.Init();

						schChoiceAdModel.ItemNo    = row["ItemNo"].ToString();
						schChoiceAdModel.MediaCode = row["MediaCode"].ToString();
						schChoiceAdModel.ChannelNo = row["ChannelNo"].ToString();

						new SchChoiceAdManager(systemModel,commonModel).SetSchChoiceChannelDetailDelete(schChoiceAdModel);

						if (schChoiceAdModel.ResultCD.Equals("0000"))
						{
							SetCount++;
						}	
					}
				}

				if(SetCount > 0)
				{
					ReloadList();
					StatusMessage("ÁöÁ¤Ã¤³Î±¤°í Æí¼º»ó¼¼³»¿ªÀÌ »èÁ¦µÇ¾ú½À´Ï´Ù.");			
				}	
				else
				{
					MessageBox.Show("¼±ÅÃµÈ ÁöÁ¤Ã¤³Î±¤°í Æí¼º³»¿ªÀÌ ¾ø½À´Ï´Ù.", "Ã¤³Î±¤°íÆí¼º",
						MessageBoxButtons.OK, MessageBoxIcon.Information);
				}	
			}
			catch(FrameException fe)
			{
				FrameSystem.showMsgForm("ÁöÁ¤Ã¤³Î±¤°í »ó¼¼Æí¼º »èÁ¦¿À·ù", new string[] {fe.ErrCode, fe.ResultMsg});
			}
			catch(Exception ex)
			{
				FrameSystem.showMsgForm("ÁöÁ¤Ã¤³Î±¤°í »ó¼¼Æí¼º »èÁ¦¿À·ù",new string[] {"",ex.Message});
			}			
		}
        
        /// <summary>
        /// È®ÀÎ
        /// </summary>
        private void ResetDetail()
        {	
			uiPanelDetail.Text = "Æí¼º±¤°í";	

			btnDelete.Enabled			= false;
        }



		/// <summary>
		/// ÁöÁ¤¸Þ´ºÆí¼º³»¿ª ¼øÀ§º¯°æ
		/// </summary>
		private void OrderSetScheduleMenuAd(int OrderSet)
		{
			StatusMessage("ÁöÁ¤¸Þ´º±¤°í Æí¼º³»¿ªÀÇ Æí¼º¼ø¼­¸¦ º¯°æÇÕ´Ï´Ù.");

			if(keyItemNo.Length == 0) 
			{
				MessageBox.Show("º¯°æÇÒ ÁöÁ¤¸Þ´º±¤°í Æí¼º³»¿ªÀÌ ¼±ÅÃµÇÁö ¾Ê¾Ò½À´Ï´Ù.","ÁöÁ¤¸Þ´º±¤°í Æí¼º³»¿ª ¼ø¼­º¯°æ", 
					MessageBoxButtons.OK, MessageBoxIcon.Information );
				return;
			}

			try
			{
				chooseAdScheduleModel.Init();

				// µ¥ÀÌÅÍ¸ðµ¨¿¡ Àü¼ÛÇÒ ³»¿ëÀ» ¼ÂÆ®ÇÑ´Ù.
				chooseAdScheduleModel.MediaCode     = keyMediaCode;
				chooseAdScheduleModel.GenreCode     = keyGenreCode;
				chooseAdScheduleModel.ItemNo        = keyItemNo;
				chooseAdScheduleModel.ScheduleOrder = keyScheduleOrder;
				chooseAdScheduleModel.ItemName      = keyItemName;

				int NowOrder  = Convert.ToInt32(keyScheduleOrder);
				int LastOrder = Convert.ToInt32(keyLastOrder); 

				if(NowOrder == 0)
				{
					MessageBox.Show("ÇØ´ç ÁöÁ¤¸Þ´º±¤°íÀÇ ¼ø¼­´Â º¯°æµÉ ¼ö ¾ø½À´Ï´Ù.","ÁöÁ¤¸Þ´º±¤°í Æí¼º³»¿ª ¼ø¼­º¯°æ", 
						MessageBoxButtons.OK, MessageBoxIcon.Information );							
					return;
				}

				switch(OrderSet)
				{
					case ORDER_FIRST:
						if(NowOrder == 1) 
						{
							MessageBox.Show("ÇØ´ç ÁöÁ¤¸Þ´º±¤°í Æí¼º³»¿ªÀÌ Ã¹¹øÂ° ¼ø¼­ÀÔ´Ï´Ù.","ÁöÁ¤¸Þ´º±¤°í Æí¼º³»¿ª ¼ø¼­º¯°æ", 
								MessageBoxButtons.OK, MessageBoxIcon.Information );							
							return;
						}
						break;
					case ORDER_UP:
						if(NowOrder == 1) 
						{
							MessageBox.Show("ÇØ´ç ÁöÁ¤¸Þ´º±¤°í Æí¼º³»¿ªÀÌ Ã¹¹øÂ° ¼ø¼­ÀÔ´Ï´Ù.","ÁöÁ¤¸Þ´º±¤°í Æí¼º³»¿ª ¼ø¼­º¯°æ", 
								MessageBoxButtons.OK, MessageBoxIcon.Information );							
							return;
						}
						break;
					case ORDER_DOWN:
						if(NowOrder == LastOrder) 
						{
							MessageBox.Show("ÇØ´ç ÁöÁ¤¸Þ´º±¤°í Æí¼º³»¿ªÀÌ ¸¶Áö¸· ¼ø¼­ÀÔ´Ï´Ù.","ÁöÁ¤¸Þ´º±¤°í Æí¼º³»¿ª ¼ø¼­º¯°æ", 
								MessageBoxButtons.OK, MessageBoxIcon.Information );							
							return;
						}
						break;
					case ORDER_LAST:
						if(NowOrder == LastOrder) 
						{
							MessageBox.Show("ÇØ´ç ÁöÁ¤¸Þ´º±¤°í Æí¼º³»¿ªÀÌ ¸¶Áö¸· ¼ø¼­ÀÔ´Ï´Ù.","ÁöÁ¤¸Þ´º±¤°í Æí¼º³»¿ª ¼ø¼­º¯°æ", 
								MessageBoxButtons.OK, MessageBoxIcon.Information );							
							return;
						}
						break;
				}

				// ÁöÁ¤¸Þ´º±¤°í Æí¼º³»¿ª ¼øÀ§ ¿Å±â´Â ¼­ºñ½º¸¦ È£ÃâÇÑ´Ù.
				new ChooseAdScheduleManager(systemModel,commonModel).SetSchMenuAdOrderSet(chooseAdScheduleModel, OrderSet);
				keyScheduleOrder = chooseAdScheduleModel.ScheduleOrder;
				
				if (chooseAdScheduleModel.ResultCD.Equals("0000"))
				{
					ReloadList();
					StatusMessage("ÁöÁ¤¸Þ´º±¤°í Æí¼º³»¿ªÀÇ ¼øÀ§°¡ º¯°æµÇ¾ú½À´Ï´Ù.");			
				}		
		
			}
			catch(FrameException fe)
			{
				FrameSystem.showMsgForm("ÁöÁ¤¸Þ´º±¤°í Æí¼º³»¿ª ¼ø¼­º¯°æ ¿À·ù", new string[] {fe.ErrCode, fe.ResultMsg});
			}
			catch(Exception ex)
			{
				FrameSystem.showMsgForm("ÁöÁ¤¸Þ´º±¤°í Æí¼º³»¿ª ¼ø¼­º¯°æ ¿À·ù",new string[] {"",ex.Message});
			}		

		}
		
		/// <summary>
		/// ÁöÁ¤Ã¤³ÎÆí¼º³»¿ª ¼ø¼­º¯°æ
		/// </summary>
		private void OrderSetScheduleChannelAd(int OrderSet)
		{
			StatusMessage("ÁöÁ¤Ã¤³Î±¤°í Æí¼º³»¿ªÀÇ Æí¼º¼øÀ§¸¦ º¯°æÇÕ´Ï´Ù.");

			if(keyItemNo.Length == 0) 
			{
				MessageBox.Show("º¯°æÇÒ ÁöÁ¤Ã¤³Î±¤°í Æí¼º³»¿ªÀÌ ¼±ÅÃµÇÁö ¾Ê¾Ò½À´Ï´Ù.","ÁöÁ¤Ã¤³Î±¤°í Æí¼º³»¿ª ¼øÀ§º¯°æ", 
					MessageBoxButtons.OK, MessageBoxIcon.Information );
				return;
			}

			try
			{
				chooseAdScheduleModel.Init();

				// µ¥ÀÌÅÍ¸ðµ¨¿¡ Àü¼ÛÇÒ ³»¿ëÀ» ¼ÂÆ®ÇÑ´Ù.
				chooseAdScheduleModel.MediaCode     = keyMediaCode;
				chooseAdScheduleModel.ChannelNo     = keyChannelNo;
				chooseAdScheduleModel.ItemNo        = keyItemNo;
				chooseAdScheduleModel.ScheduleOrder = keyScheduleOrder;
				chooseAdScheduleModel.ItemName      = keyItemName;

				int NowOrder  = Convert.ToInt32(keyScheduleOrder);
				int LastOrder = Convert.ToInt32(keyLastOrder); 

				if(NowOrder == 0)
				{
					MessageBox.Show("ÇØ´ç ÁöÁ¤Ã¤³Î±¤°íÀÇ ¼ø¼­´Â º¯°æµÉ ¼ö ¾ø½À´Ï´Ù.","ÁöÁ¤Ã¤³Î±¤°í Æí¼º³»¿ª ¼ø¼­º¯°æ", 
						MessageBoxButtons.OK, MessageBoxIcon.Information );							
					return;
				}

				switch(OrderSet)
				{
					case ORDER_FIRST:
						if(NowOrder <= 1) 
						{
							MessageBox.Show("ÇØ´ç ÁöÁ¤Ã¤³Î±¤°í Æí¼º³»¿ªÀÌ Ã¹¹øÂ° ¼ø¼­ÀÔ´Ï´Ù.","ÁöÁ¤Ã¤³Î±¤°í Æí¼º³»¿ª ¼ø¼­º¯°æ", 
								MessageBoxButtons.OK, MessageBoxIcon.Information );							
							return;
						}
						break;
					case ORDER_UP:
						if(NowOrder <= 1) 
						{
							MessageBox.Show("ÇØ´ç ÁöÁ¤Ã¤³Î±¤°í Æí¼º³»¿ªÀÌ Ã¹¹øÂ° ¼ø¼­ÀÔ´Ï´Ù.","ÁöÁ¤Ã¤³Î±¤°í Æí¼º³»¿ª ¼ø¼­º¯°æ", 
								MessageBoxButtons.OK, MessageBoxIcon.Information );							
							return;
						}
						break;
					case ORDER_DOWN:
						if(NowOrder >= LastOrder) 
						{
							MessageBox.Show("ÇØ´ç ÁöÁ¤Ã¤³Î±¤°í Æí¼º³»¿ªÀÌ ¸¶Áö¸· ¼ø¼­ÀÔ´Ï´Ù.","ÁöÁ¤Ã¤³Î±¤°í Æí¼º³»¿ª ¼ø¼­º¯°æ", 
								MessageBoxButtons.OK, MessageBoxIcon.Information );							
							return;
						}
						break;
					case ORDER_LAST:
						if(NowOrder >= LastOrder) 
						{
							MessageBox.Show("ÇØ´ç ÁöÁ¤Ã¤³Î±¤°í Æí¼º³»¿ªÀÌ ¸¶Áö¸· ¼ø¼­ÀÔ´Ï´Ù.","ÁöÁ¤Ã¤³Î±¤°í Æí¼º³»¿ª ¼ø¼­º¯°æ", 
								MessageBoxButtons.OK, MessageBoxIcon.Information );							
							return;
						}
						break;
				}

				// ÁöÁ¤Ã¤³Î±¤°í Æí¼º³»¿ª ¼øÀ§ ¿Å±â´Â ¼­ºñ½º¸¦ È£ÃâÇÑ´Ù.
				new ChooseAdScheduleManager(systemModel,commonModel).SetSchChannelAdOrderSet(chooseAdScheduleModel, OrderSet);
				keyScheduleOrder = chooseAdScheduleModel.ScheduleOrder;
				
				if (chooseAdScheduleModel.ResultCD.Equals("0000"))
				{
					ReloadSchedule();
					StatusMessage("ÁöÁ¤Ã¤³Î±¤°í Æí¼º³»¿ªÀÇ ¼ø¼­°¡ º¯°æµÇ¾ú½À´Ï´Ù.");			
				}		
		
			}
			catch(FrameException fe)
			{
				FrameSystem.showMsgForm("ÁöÁ¤Ã¤³Î±¤°í Æí¼º³»¿ª ¼ø¼­º¯°æ ¿À·ù", new string[] {fe.ErrCode, fe.ResultMsg});
			}
			catch(Exception ex)
			{
				FrameSystem.showMsgForm("ÁöÁ¤Ã¤³Î±¤°í Æí¼º³»¿ª ¼ø¼­º¯°æ ¿À·ù",new string[] {"",ex.Message});
			}		

		}

		/// <summary>
		/// Å°¯“À»Ã£¾Æ ±×¸®µå Å°¿¡ ÇØ´çµÇ´Â·Î¿ì·Î..
		/// </summary>
		private void SetCurrentRowSchedule()
		{

			int rowIndex = 0;
			if ( dtSchedule.Rows.Count < 1 ) return;              
			foreach (DataRow row in dtSchedule.Rows)
			{					
				if(row["ScheduleOrder"].ToString().Equals(keyScheduleOrder))
				{					
					cmSchedule.Position = rowIndex;
					break;								
				}
				rowIndex++;
			}
			grdExScheduleList.EnsureVisible();
		}


		private void ReloadSchedule()
		{
			switch(keyCmType)
			{
//				case "M":
//					SearchChooseAdScheduleMenu();
//					break;
				case "C":
					SearchChooseAdScheduleChannel();
					break;
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

		#region ÆË¾÷Ã¢À» À§ÇÑ ¸Þ¼Òµå
			
		public void ReloadList()
		{
			switch(keyCmType)
			{
				case "M":
					SetDetailTextMenu();
					break;
				case "C":
					SetDetailTextChannel();
					break;
			}
		}

		#endregion

        /// <summary>
        /// Æí¼ºÃß°¡ÇÒ ±¤°í ÆË¾÷À©µµ¿ì¸¦ ¿ÀÇÂÇÏ´Â ÇÔ¼ö
        /// °¢ Ãß°¡¹öÆ°¿¡¼­ Å¬¸¯¿¡¼­ È£ÃâÇÑ´Ù
        /// </summary>
        /// <param name="adType"></param>
        private void InsertOpenPopupWin(int adType)
        {
            ChooseAdSearch_pForm pForm = new ChooseAdSearch_pForm(this,"SchMidAdControl");
			
            switch(keyCmType)
            {
                case "M":
                    pForm.ScheduleType = TYPE_Schedule.Genre;
                    pForm.keyMediaCode = keyMediaCode;
                    pForm.keyGenreCode = keyGenreCode;
                    pForm.AdType        = adType;
                    break;
                case "C":
                    pForm.ScheduleType = TYPE_Schedule.Channel;
                    pForm.keyMediaCode = keyMediaCode;
                    pForm.keyChannelNo = keyChannelNo;
                    pForm.AdType       = adType;
                    break;
            }
            pForm.ShowDialog();            
            pForm.Dispose();
            pForm = null;
        }

        /// <summary>
        /// Áß°£±¤°í Ãß°¡ÆË¾÷À» ¿¬´Ù
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAdd_16_Click(object sender, System.EventArgs e)
        {
            this.InsertOpenPopupWin( 17 );
        }

        private void grdExChooseAdScheduleList_ColumnButtonClick(object sender, Janus.Windows.GridEX.ColumnActionEventArgs e)
        {
            Schedule.SchMidAd_pSet  pForm = new AdManagerClient.Schedule.SchMidAd_pSet();
            pForm.keyMediaCode      =   keyMediaCode;
            pForm.keyCategoryCode   =   keyCategoryCode;
            pForm.keyGenreCode      =   keyGenreCode;
            pForm.keyChannelNo      =   keyChannelNo;
            pForm.ChannelNo         =   keyChannelNo;
            pForm.Title             =   keyTitle;
            pForm.ShowDialog();            

            pForm.Dispose();
            pForm = null;		
        }
    }
}