// ===============================================================================
// MenuPowerControl for Charites Project
//
// MenuPowerControl.cs
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
    public class MenuPowerControl : System.Windows.Forms.UserControl, IUserControl
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
        MenuModel menuModel  = new MenuModel();	// Ã¤³Î/¸Þ´º Æí¼ºÇöÈ² ¸ðµ¨
        SchChoiceAdModel schChoiceAdModel  = new SchChoiceAdModel();	// ÁöÁ¤±¤°íÆí¼º¸ðµ¨
		
        // È­¸éÃ³¸®¿ë º¯¼ö
        CurrencyManager cmUserClass        = null;					// µ¥ÀÌÅÍ ±×¸®µåÀÇ º¯°æ¿¡ µû¸¥ µ¥ÀÌÅÍ¼Â °ü¸®¸¦ À§ÇÏ¿©			
        DataTable       dtUserClass        = null;

        bool IsAdding             = false;
        
        // »ç¿ë±ÇÇÑ
        bool IsSearching = false; // Á¶È¸Áß »ó¼¼È­¸éÀÌ ¾÷µ¥ÀÌÆ® µÇ´Â °ÍÀ» ¹æÁö ÇÏ±âÀ§ÇÔ 2011.11.29 JH.Park
        bool canCreate			  = false;
        bool canRead			  = false;
        bool canUpdate			  = false;
        // Key
        bool IsNotLoading		       = true;					// »ó¼¼Á¶È¸ÁßÀÌ ¾Æ´Ô
        public string keyUserClassCode     = "";
        public string keyUserClassName  = "";
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
        private Janus.Windows.UI.Dock.UIPanelGroup uiPanelMenuPower;
        private Janus.Windows.UI.Dock.UIPanel uiPanel1;
        private Janus.Windows.UI.Dock.UIPanelInnerContainer uiPanel1Container;
        private Janus.Windows.UI.Dock.UIPanel uiPanel2;
        private Janus.Windows.UI.Dock.UIPanelInnerContainer uiPanel2Container;
        private Janus.Windows.GridEX.GridEX gridEX1;
        private AdManagerClient.MenuPowerDs menuPowerDs;
        private System.Data.DataView dvUserClass;
        private System.Data.DataView dvMenuPower;
        private Janus.Windows.GridEX.GridEX grdExUserClassList;
        private Janus.Windows.UI.Dock.UIPanel uiPanelMenuPowerDetail;
        private Janus.Windows.UI.Dock.UIPanelInnerContainer uiPanelMenuPowerDetailContainer;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label lbChannelNo;
        private Janus.Windows.EditControls.UIButton btnSave;
        private System.Windows.Forms.Label lbTitle;
        private Janus.Windows.EditControls.UIButton btnAdd;
        private Janus.Windows.GridEX.EditControls.EditBox ebUserClassName;
        private Janus.Windows.GridEX.EditControls.EditBox ebUserClassCode;
        private Janus.Windows.GridEX.GridEX grdExMenuPowerList;		
        private System.ComponentModel.IContainer components;

        public MenuPowerControl()
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
            Janus.Windows.GridEX.GridEXLayout grdExUserClassList_DesignTimeLayout = new Janus.Windows.GridEX.GridEXLayout();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MenuPowerControl));
            Janus.Windows.GridEX.GridEXLayout grdExMenuPowerList_DesignTimeLayout = new Janus.Windows.GridEX.GridEXLayout();
            this.uiPM = new Janus.Windows.UI.Dock.UIPanelManager(this.components);
            this.uiPanelUsers = new Janus.Windows.UI.Dock.UIPanelGroup();
            this.uiPanelMenuPower = new Janus.Windows.UI.Dock.UIPanelGroup();
            this.uiPanel1 = new Janus.Windows.UI.Dock.UIPanel();
            this.uiPanel1Container = new Janus.Windows.UI.Dock.UIPanelInnerContainer();
            this.grdExUserClassList = new Janus.Windows.GridEX.GridEX();
            this.dvUserClass = new System.Data.DataView();
            this.menuPowerDs = new AdManagerClient.MenuPowerDs();
            this.uiPanel2 = new Janus.Windows.UI.Dock.UIPanel();
            this.uiPanel2Container = new Janus.Windows.UI.Dock.UIPanelInnerContainer();
            this.grdExMenuPowerList = new Janus.Windows.GridEX.GridEX();
            this.dvMenuPower = new System.Data.DataView();
            this.uiPanelMenuPowerDetail = new Janus.Windows.UI.Dock.UIPanel();
            this.uiPanelMenuPowerDetailContainer = new Janus.Windows.UI.Dock.UIPanelInnerContainer();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnAdd = new Janus.Windows.EditControls.UIButton();
            this.ebUserClassName = new Janus.Windows.GridEX.EditControls.EditBox();
            this.ebUserClassCode = new Janus.Windows.GridEX.EditControls.EditBox();
            this.lbChannelNo = new System.Windows.Forms.Label();
            this.btnSave = new Janus.Windows.EditControls.UIButton();
            this.lbTitle = new System.Windows.Forms.Label();
            this.gridEX1 = new Janus.Windows.GridEX.GridEX();
            this.uiButton1 = new Janus.Windows.EditControls.UIButton();
            this.uiButton2 = new Janus.Windows.EditControls.UIButton();
            ((System.ComponentModel.ISupportInitialize)(this.uiPM)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelUsers)).BeginInit();
            this.uiPanelUsers.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelMenuPower)).BeginInit();
            this.uiPanelMenuPower.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanel1)).BeginInit();
            this.uiPanel1.SuspendLayout();
            this.uiPanel1Container.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdExUserClassList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dvUserClass)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.menuPowerDs)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanel2)).BeginInit();
            this.uiPanel2.SuspendLayout();
            this.uiPanel2Container.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdExMenuPowerList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dvMenuPower)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelMenuPowerDetail)).BeginInit();
            this.uiPanelMenuPowerDetail.SuspendLayout();
            this.uiPanelMenuPowerDetailContainer.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridEX1)).BeginInit();
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
            this.uiPanelMenuPower.Id = new System.Guid("88f86479-98e4-43d4-bf03-12fd789fcfc5");
            this.uiPanelMenuPower.StaticGroup = true;
            this.uiPanel1.Id = new System.Guid("91eeb7fc-856c-4a9c-b37e-db742cfa9b2e");
            this.uiPanelMenuPower.Panels.Add(this.uiPanel1);
            this.uiPanel2.Id = new System.Guid("3d64f731-de32-42ba-a10f-ac3b5ac56703");
            this.uiPanelMenuPower.Panels.Add(this.uiPanel2);
            this.uiPanelUsers.Panels.Add(this.uiPanelMenuPower);
            this.uiPanelMenuPowerDetail.Id = new System.Guid("0ae65184-cac1-4b98-924d-9e802237e79a");
            this.uiPanelUsers.Panels.Add(this.uiPanelMenuPowerDetail);
            this.uiPM.Panels.Add(this.uiPanelUsers);
            // 
            // Design Time Panel Info:
            // 
            this.uiPM.BeginPanelInfo();
            this.uiPM.AddDockPanelInfo(new System.Guid("b162c53a-7940-47cd-a0ed-9762c59c50d8"), Janus.Windows.UI.Dock.PanelGroupStyle.HorizontalTiles, Janus.Windows.UI.Dock.PanelDockStyle.Fill, true, new System.Drawing.Size(1010, 677), true);
            this.uiPM.AddDockPanelInfo(new System.Guid("88f86479-98e4-43d4-bf03-12fd789fcfc5"), new System.Guid("b162c53a-7940-47cd-a0ed-9762c59c50d8"), Janus.Windows.UI.Dock.PanelGroupStyle.VerticalTiles, true, 527, true);
            this.uiPM.AddDockPanelInfo(new System.Guid("91eeb7fc-856c-4a9c-b37e-db742cfa9b2e"), new System.Guid("88f86479-98e4-43d4-bf03-12fd789fcfc5"), 289, true);
            this.uiPM.AddDockPanelInfo(new System.Guid("3d64f731-de32-42ba-a10f-ac3b5ac56703"), new System.Guid("88f86479-98e4-43d4-bf03-12fd789fcfc5"), 717, true);
            this.uiPM.AddDockPanelInfo(new System.Guid("0ae65184-cac1-4b98-924d-9e802237e79a"), new System.Guid("b162c53a-7940-47cd-a0ed-9762c59c50d8"), 104, true);
            this.uiPM.AddFloatingPanelInfo(new System.Guid("b162c53a-7940-47cd-a0ed-9762c59c50d8"), Janus.Windows.UI.Dock.PanelGroupStyle.HorizontalTiles, true, new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
            this.uiPM.AddFloatingPanelInfo(new System.Guid("8e3140b9-bfcb-43af-b84e-2ee6cddf51dc"), new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
            this.uiPM.AddFloatingPanelInfo(new System.Guid("b2973a57-d3e1-4d80-9387-7a6dc3332542"), new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
            this.uiPM.AddFloatingPanelInfo(new System.Guid("afb3b7d0-aa79-443c-a1e8-3baba38f7a4d"), new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
            this.uiPM.AddFloatingPanelInfo(new System.Guid("88f86479-98e4-43d4-bf03-12fd789fcfc5"), Janus.Windows.UI.Dock.PanelGroupStyle.VerticalTiles, true, new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
            this.uiPM.AddFloatingPanelInfo(new System.Guid("91eeb7fc-856c-4a9c-b37e-db742cfa9b2e"), new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
            this.uiPM.AddFloatingPanelInfo(new System.Guid("3d64f731-de32-42ba-a10f-ac3b5ac56703"), new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
            this.uiPM.AddFloatingPanelInfo(new System.Guid("609188ab-b98f-4466-8472-b8b36f1af6d5"), Janus.Windows.UI.Dock.PanelGroupStyle.HorizontalTiles, true, new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
            this.uiPM.AddFloatingPanelInfo(new System.Guid("c1ac152b-71d8-497e-a36e-0758f080f6ec"), new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
            this.uiPM.AddFloatingPanelInfo(new System.Guid("8e7f9052-77ad-49de-84a8-26a099999d9e"), new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
            this.uiPM.AddFloatingPanelInfo(new System.Guid("0ae65184-cac1-4b98-924d-9e802237e79a"), new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
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
            this.uiPanelUsers.TabStop = false;
            this.uiPanelUsers.Text = "¸Þ´º±ÇÇÑ°ü¸®";
            // 
            // uiPanelMenuPower
            // 
            this.uiPanelMenuPower.AllowResize = Janus.Windows.UI.InheritableBoolean.True;
            this.uiPanelMenuPower.CaptionStyle = Janus.Windows.UI.Dock.PanelCaptionStyle.Dark;
            this.uiPanelMenuPower.CaptionVisible = Janus.Windows.UI.InheritableBoolean.False;
            this.uiPanelMenuPower.GroupStyle = Janus.Windows.UI.Dock.PanelGroupStyle.VerticalTiles;
            this.uiPanelMenuPower.Location = new System.Drawing.Point(0, 22);
            this.uiPanelMenuPower.Name = "uiPanelMenuPower";
            this.uiPanelMenuPower.Size = new System.Drawing.Size(1010, 544);
            this.uiPanelMenuPower.TabIndex = 0;
            // 
            // uiPanel1
            // 
            this.uiPanel1.InnerContainer = this.uiPanel1Container;
            this.uiPanel1.Location = new System.Drawing.Point(0, 0);
            this.uiPanel1.Name = "uiPanel1";
            this.uiPanel1.Size = new System.Drawing.Size(289, 544);
            this.uiPanel1.TabIndex = 0;
            this.uiPanel1.TabStop = false;
            this.uiPanel1.Text = "»ç¿ëÀÚÁ¤º¸";
            // 
            // uiPanel1Container
            // 
            this.uiPanel1Container.Controls.Add(this.grdExUserClassList);
            this.uiPanel1Container.Location = new System.Drawing.Point(1, 23);
            this.uiPanel1Container.Name = "uiPanel1Container";
            this.uiPanel1Container.Size = new System.Drawing.Size(287, 520);
            this.uiPanel1Container.TabIndex = 0;
            // 
            // grdExUserClassList
            // 
            this.grdExUserClassList.AllowEdit = Janus.Windows.GridEX.InheritableBoolean.False;
            this.grdExUserClassList.AlternatingColors = true;
            this.grdExUserClassList.BorderStyle = Janus.Windows.GridEX.BorderStyle.None;
            this.grdExUserClassList.DataSource = this.dvUserClass;
            grdExUserClassList_DesignTimeLayout.LayoutString = resources.GetString("grdExUserClassList_DesignTimeLayout.LayoutString");
            this.grdExUserClassList.DesignTimeLayout = grdExUserClassList_DesignTimeLayout;
            this.grdExUserClassList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdExUserClassList.EditorsControlStyle.ButtonAppearance = Janus.Windows.GridEX.ButtonAppearance.Regular;
            this.grdExUserClassList.EmptyRows = true;
            this.grdExUserClassList.FocusCellFormatStyle.BackColor = System.Drawing.SystemColors.Highlight;
            this.grdExUserClassList.FocusCellFormatStyle.ForeColor = System.Drawing.Color.White;
            this.grdExUserClassList.FocusStyle = Janus.Windows.GridEX.FocusStyle.None;
            this.grdExUserClassList.Font = new System.Drawing.Font("±¼¸²Ã¼", 9F);
            this.grdExUserClassList.FrozenColumns = 2;
            this.grdExUserClassList.GridLineColor = System.Drawing.Color.Silver;
            this.grdExUserClassList.GridLines = Janus.Windows.GridEX.GridLines.Vertical;
            this.grdExUserClassList.GridLineStyle = Janus.Windows.GridEX.GridLineStyle.Solid;
            this.grdExUserClassList.GroupByBoxVisible = false;
            this.grdExUserClassList.HeaderFormatStyle.TextAlignment = Janus.Windows.GridEX.TextAlignment.Center;
            this.grdExUserClassList.HideSelection = Janus.Windows.GridEX.HideSelection.HighlightInactive;
            this.grdExUserClassList.Location = new System.Drawing.Point(0, 0);
            this.grdExUserClassList.Name = "grdExUserClassList";
            this.grdExUserClassList.ScrollBars = Janus.Windows.GridEX.ScrollBars.Vertical;
            this.grdExUserClassList.SelectedInactiveFormatStyle.BackColor = System.Drawing.Color.Gold;
            this.grdExUserClassList.Size = new System.Drawing.Size(287, 520);
            this.grdExUserClassList.TabIndex = 1;
            this.grdExUserClassList.TabKeyBehavior = Janus.Windows.GridEX.TabKeyBehavior.ControlNavigation;
            this.grdExUserClassList.ThemedAreas = ((Janus.Windows.GridEX.ThemedArea)(((((((((Janus.Windows.GridEX.ThemedArea.ScrollBars | Janus.Windows.GridEX.ThemedArea.EditControls) 
            | Janus.Windows.GridEX.ThemedArea.Headers) 
            | Janus.Windows.GridEX.ThemedArea.GroupByBox) 
            | Janus.Windows.GridEX.ThemedArea.GroupRows) 
            | Janus.Windows.GridEX.ThemedArea.ControlBorder) 
            | Janus.Windows.GridEX.ThemedArea.Cards) 
            | Janus.Windows.GridEX.ThemedArea.Gridlines) 
            | Janus.Windows.GridEX.ThemedArea.CheckBoxes)));
            this.grdExUserClassList.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            this.grdExUserClassList.SelectionChanged += new System.EventHandler(this.OnGrdRowChangedUserClass);
            this.grdExUserClassList.Enter += new System.EventHandler(this.OnGrdRowChangedUserClass);
            // 
            // dvUserClass
            // 
            this.dvUserClass.Table = this.menuPowerDs.UserClass;
            // 
            // menuPowerDs
            // 
            this.menuPowerDs.DataSetName = "MenuPowerDs";
            this.menuPowerDs.Locale = new System.Globalization.CultureInfo("en-US");
            this.menuPowerDs.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // uiPanel2
            // 
            this.uiPanel2.InnerContainer = this.uiPanel2Container;
            this.uiPanel2.Location = new System.Drawing.Point(293, 0);
            this.uiPanel2.Name = "uiPanel2";
            this.uiPanel2.Size = new System.Drawing.Size(717, 544);
            this.uiPanel2.TabIndex = 0;
            this.uiPanel2.Text = "¸Þ´º±ÇÇÑÁ¤º¸";
            // 
            // uiPanel2Container
            // 
            this.uiPanel2Container.Controls.Add(this.grdExMenuPowerList);
            this.uiPanel2Container.Location = new System.Drawing.Point(1, 23);
            this.uiPanel2Container.Name = "uiPanel2Container";
            this.uiPanel2Container.Size = new System.Drawing.Size(715, 520);
            this.uiPanel2Container.TabIndex = 0;
            // 
            // grdExMenuPowerList
            // 
            this.grdExMenuPowerList.AlternatingColors = true;
            this.grdExMenuPowerList.AutoEdit = true;
            this.grdExMenuPowerList.BorderStyle = Janus.Windows.GridEX.BorderStyle.None;
            this.grdExMenuPowerList.DataSource = this.dvMenuPower;
            grdExMenuPowerList_DesignTimeLayout.LayoutString = resources.GetString("grdExMenuPowerList_DesignTimeLayout.LayoutString");
            this.grdExMenuPowerList.DesignTimeLayout = grdExMenuPowerList_DesignTimeLayout;
            this.grdExMenuPowerList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdExMenuPowerList.EditorsControlStyle.ButtonAppearance = Janus.Windows.GridEX.ButtonAppearance.Regular;
            this.grdExMenuPowerList.EmptyRows = true;
            this.grdExMenuPowerList.FocusCellFormatStyle.BackColor = System.Drawing.SystemColors.Highlight;
            this.grdExMenuPowerList.FocusStyle = Janus.Windows.GridEX.FocusStyle.None;
            this.grdExMenuPowerList.Font = new System.Drawing.Font("±¼¸²Ã¼", 9F);
            this.grdExMenuPowerList.FrozenColumns = 2;
            this.grdExMenuPowerList.GridLineColor = System.Drawing.Color.Silver;
            this.grdExMenuPowerList.GridLines = Janus.Windows.GridEX.GridLines.Vertical;
            this.grdExMenuPowerList.GridLineStyle = Janus.Windows.GridEX.GridLineStyle.Solid;
            this.grdExMenuPowerList.GroupByBoxVisible = false;
            this.grdExMenuPowerList.HideSelection = Janus.Windows.GridEX.HideSelection.HighlightInactive;
            this.grdExMenuPowerList.Location = new System.Drawing.Point(0, 0);
            this.grdExMenuPowerList.Name = "grdExMenuPowerList";
            this.grdExMenuPowerList.ScrollBars = Janus.Windows.GridEX.ScrollBars.Vertical;
            this.grdExMenuPowerList.SelectedInactiveFormatStyle.BackColor = System.Drawing.Color.Gold;
            this.grdExMenuPowerList.Size = new System.Drawing.Size(715, 520);
            this.grdExMenuPowerList.TabIndex = 2;
            this.grdExMenuPowerList.TabKeyBehavior = Janus.Windows.GridEX.TabKeyBehavior.ControlNavigation;
            this.grdExMenuPowerList.ThemedAreas = ((Janus.Windows.GridEX.ThemedArea)(((((((((Janus.Windows.GridEX.ThemedArea.ScrollBars | Janus.Windows.GridEX.ThemedArea.EditControls) 
            | Janus.Windows.GridEX.ThemedArea.Headers) 
            | Janus.Windows.GridEX.ThemedArea.GroupByBox) 
            | Janus.Windows.GridEX.ThemedArea.GroupRows) 
            | Janus.Windows.GridEX.ThemedArea.ControlBorder) 
            | Janus.Windows.GridEX.ThemedArea.Cards) 
            | Janus.Windows.GridEX.ThemedArea.Gridlines) 
            | Janus.Windows.GridEX.ThemedArea.CheckBoxes)));
            this.grdExMenuPowerList.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            // 
            // dvMenuPower
            // 
            this.dvMenuPower.Table = this.menuPowerDs.MenuPower;
            // 
            // uiPanelMenuPowerDetail
            // 
            this.uiPanelMenuPowerDetail.AllowResize = Janus.Windows.UI.InheritableBoolean.False;
            this.uiPanelMenuPowerDetail.InnerContainer = this.uiPanelMenuPowerDetailContainer;
            this.uiPanelMenuPowerDetail.Location = new System.Drawing.Point(0, 570);
            this.uiPanelMenuPowerDetail.Name = "uiPanelMenuPowerDetail";
            this.uiPanelMenuPowerDetail.Size = new System.Drawing.Size(1010, 107);
            this.uiPanelMenuPowerDetail.TabIndex = 0;
            this.uiPanelMenuPowerDetail.TabStop = false;
            this.uiPanelMenuPowerDetail.Text = "¼º¼¼Á¤º¸";
            // 
            // uiPanelMenuPowerDetailContainer
            // 
            this.uiPanelMenuPowerDetailContainer.Controls.Add(this.panel1);
            this.uiPanelMenuPowerDetailContainer.Location = new System.Drawing.Point(1, 23);
            this.uiPanelMenuPowerDetailContainer.Name = "uiPanelMenuPowerDetailContainer";
            this.uiPanelMenuPowerDetailContainer.Size = new System.Drawing.Size(1008, 83);
            this.uiPanelMenuPowerDetailContainer.TabIndex = 0;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnAdd);
            this.panel1.Controls.Add(this.ebUserClassName);
            this.panel1.Controls.Add(this.ebUserClassCode);
            this.panel1.Controls.Add(this.lbChannelNo);
            this.panel1.Controls.Add(this.btnSave);
            this.panel1.Controls.Add(this.lbTitle);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1008, 83);
            this.panel1.TabIndex = 7;
            // 
            // btnAdd
            // 
            this.btnAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnAdd.BackColor = System.Drawing.SystemColors.Window;
            this.btnAdd.Enabled = false;
            this.btnAdd.Location = new System.Drawing.Point(116, 46);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(104, 24);
            this.btnAdd.TabIndex = 6;
            this.btnAdd.Text = "Ãß °¡";
            this.btnAdd.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // ebUserClassName
            // 
            this.ebUserClassName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.ebUserClassName.Font = new System.Drawing.Font("±¼¸²Ã¼", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.ebUserClassName.Location = new System.Drawing.Point(294, 13);
            this.ebUserClassName.MaxLength = 120;
            this.ebUserClassName.Name = "ebUserClassName";
            this.ebUserClassName.Size = new System.Drawing.Size(152, 21);
            this.ebUserClassName.TabIndex = 4;
            this.ebUserClassName.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
            this.ebUserClassName.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            // 
            // ebUserClassCode
            // 
            this.ebUserClassCode.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.ebUserClassCode.Font = new System.Drawing.Font("±¼¸²Ã¼", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.ebUserClassCode.Location = new System.Drawing.Point(56, 13);
            this.ebUserClassCode.MaxLength = 120;
            this.ebUserClassCode.Name = "ebUserClassCode";
            this.ebUserClassCode.Size = new System.Drawing.Size(152, 21);
            this.ebUserClassCode.TabIndex = 3;
            this.ebUserClassCode.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
            this.ebUserClassCode.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            // 
            // lbChannelNo
            // 
            this.lbChannelNo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lbChannelNo.Font = new System.Drawing.Font("±¼¸²Ã¼", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lbChannelNo.Location = new System.Drawing.Point(8, 13);
            this.lbChannelNo.Name = "lbChannelNo";
            this.lbChannelNo.Size = new System.Drawing.Size(72, 21);
            this.lbChannelNo.TabIndex = 0;
            this.lbChannelNo.Text = "ÄÚµå";
            this.lbChannelNo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnSave.BackColor = System.Drawing.SystemColors.Window;
            this.btnSave.Enabled = false;
            this.btnSave.Location = new System.Drawing.Point(8, 46);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(104, 24);
            this.btnSave.TabIndex = 5;
            this.btnSave.Text = "Àú Àå";
            this.btnSave.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // lbTitle
            // 
            this.lbTitle.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lbTitle.Font = new System.Drawing.Font("±¼¸²Ã¼", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lbTitle.Location = new System.Drawing.Point(232, 13);
            this.lbTitle.Name = "lbTitle";
            this.lbTitle.Size = new System.Drawing.Size(72, 21);
            this.lbTitle.TabIndex = 0;
            this.lbTitle.Text = "±¸ºÐ¸í";
            this.lbTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // gridEX1
            // 
            this.gridEX1.Location = new System.Drawing.Point(0, 0);
            this.gridEX1.Name = "gridEX1";
            this.gridEX1.Size = new System.Drawing.Size(400, 376);
            this.gridEX1.TabIndex = 0;
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
            // MenuPowerControl
            // 
            this.Controls.Add(this.uiPanelUsers);
            this.Name = "MenuPowerControl";
            this.Size = new System.Drawing.Size(1010, 677);
            this.Load += new System.EventHandler(this.MenuPowerControl_Load);
            ((System.ComponentModel.ISupportInitialize)(this.uiPM)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelUsers)).EndInit();
            this.uiPanelUsers.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelMenuPower)).EndInit();
            this.uiPanelMenuPower.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.uiPanel1)).EndInit();
            this.uiPanel1.ResumeLayout(false);
            this.uiPanel1Container.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdExUserClassList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dvUserClass)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.menuPowerDs)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanel2)).EndInit();
            this.uiPanel2.ResumeLayout(false);
            this.uiPanel2Container.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdExMenuPowerList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dvMenuPower)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelMenuPowerDetail)).EndInit();
            this.uiPanelMenuPowerDetail.ResumeLayout(false);
            this.uiPanelMenuPowerDetailContainer.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridEX1)).EndInit();
            this.ResumeLayout(false);

		}
        #endregion

        #region ÄÁÆ®·Ñ ·Îµå
        private void MenuPowerControl_Load(object sender, System.EventArgs e)
        {
            // µ¥ÀÌÅÍ°ü¸®¿ë °´Ã¼»ý¼º
            dtUserClass= ((DataView)grdExUserClassList.DataSource).Table;
            cmUserClass= (CurrencyManager) this.BindingContext[grdExUserClassList.DataSource]; 
            //
            //            cmMenuPower = (CurrencyManager) this.BindingContext[grdExMenuPowerList.DataSource]; 
            //            dtMenuPower  = ((DataView)grdExMenuPowerList.DataSource).Table;
            //			
            //            // ÄÁÆ®·Ñ ÃÊ±âÈ­
            InitControl();
        }

        #endregion

        #region ÄÁÆ®·Ñ ÃÊ±âÈ­
        private void InitControl()
        {
            ProgressStart();

            // Ãß°¡±ÇÇÑ °Ë»ç
            if(menu.CanCreate(MenuCode))
            {
                canCreate = true;
            }


            // Á¶È¸±ÇÇÑ °Ë»ç
            if(menu.CanRead(MenuCode))
            {
                canRead = true;
            }

            // ¼öÁ¤±ÇÇÑ °Ë»ç
            if(menu.CanUpdate(MenuCode))
            {
                canUpdate = true;
            }

            if(canRead) SearchUserClass();
			
            InitButton();
            
            ProgressStop();
        }

	
        private void InitButton()
        {
            if(canCreate) btnAdd.Enabled = true;
            
            if(!IsAdding && ebUserClassCode.Text.Trim().Length > 0) 
            {
                if(canUpdate) btnSave.Enabled   = true;
                
            }
            if(IsAdding)
            {
                btnAdd.Enabled    = false;
                ResetTextReadonly();
            }
            else
            {
                btnAdd.Enabled    = true;
                SetTextReadonly();
            }

            Application.DoEvents();
        }

        private void DisableButton()
        {
            
            btnAdd.Enabled    = false;
            btnSave.Enabled   = false;
            
            Application.DoEvents();
        }

        #endregion

        #region »ç¿ëÀÚ ¾×¼ÇÃ³¸® ¸Þ¼Òµå

        /// <summary>
        /// ±×¸®µåÀÇ Rowº¯°æ½Ã
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnGrdRowChangedUserClass(object sender, System.EventArgs e) 
        {
            if (!IsSearching) // 2011.11.29 JH.Park Á¶È¸ÁßÀÌ ¾Æ´Ò°æ¿ì¿¡¸¸ µ¿ÀÛÇÏµµ·Ï º¯°æ
            {
                if (IsNotLoading)
                {
                    SetDetailTextUserClass();
                    InitButton();
                }
            }
        }

		
        private void OnGrdRowChangedMenuPower(object sender, System.EventArgs e) 
        {		
            //            if(IsNotLoading)
            //            {
            //                SetDetailTextChannel();
            //                InitButton();
            //            }
        }

        /// <summary>
        /// Ãß°¡¹öÆ° Å¬¸¯
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAdd_Click(object sender, System.EventArgs e)
        {
            btnAdd.Enabled    = false;
            btnSave.Enabled   = true;

            IsAdding = true;

            ResetTextReadonly();
            ResetDetailText();

            ebUserClassCode.Focus();
        }

        /// <summary>
        /// ÀúÀå¹öÆ° Å¬¸¯
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_Click(object sender, System.EventArgs e)
        {
            SaveUserClass();
        }


   
        #endregion

        #region Ã³¸®¸Þ¼Òµå

        /// <summary>
        /// Ã¤³Î/¸Þ´º Æí¼ºÇöÈ² ¸ñ·Ï Á¶È¸
        /// </summary>
        private void SearchUserClass()
        {
            StatusMessage("¸Þ´º Á¤º¸¸¦ Á¶È¸ÇÕ´Ï´Ù.");

            try
            {
                menuModel.Init();
                				
                // µ¥ÀÌÅÍ Å¬¸®¾î
                menuPowerDs.UserClass.Clear();
                menuPowerDs.MenuPower.Clear();   
                ResetDetailText();
                
                // Àå¸£¸Þ´º Á¶È¸ ¼­ºñ½º¸¦ È£ÃâÇÑ´Ù.
                new MenuManager(systemModel,commonModel).GetUserClassList(menuModel);
                
                if (menuModel.ResultCD.Equals("0000"))
                {
                    Utility.SetDataTable(menuPowerDs.UserClass, menuModel.MenuDataSet);
                    StatusMessage(menuModel.ResultCnt + "°ÇÀÇ ¸Þ´º Á¤º¸°¡ Á¶È¸µÇ¾ú½À´Ï´Ù.");
                    if(canUpdate)
                    {
                        AddMenuPower();									
                    }	
                    grdExUserClassList.Focus();
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
        }


        private void SetDetailTextUserClass()
        {
            int curRow = cmUserClass.Position;
            
            if(curRow >= 0 )
            {
                IsNotLoading = false;	// Á¶È¸Áß ´Ù½Ã Á¶È¸µÇ´Â °ÍÀ» ¹æÁöÇÔ.
                try
                {
                    keyUserClassCode    = dtUserClass.Rows[curRow]["UserClassCode"].ToString();
                    keyUserClassName = dtUserClass.Rows[curRow]["UserClassName"].ToString();
                    
                    ebUserClassCode.Text = keyUserClassCode;
                    ebUserClassName.Text = keyUserClassName;

                    menuPowerDs.MenuPower.Clear();        
            
                    SearchMenuPower();
                    IsAdding = false;
                }
                finally
                {
                    IsNotLoading = true;
                }
            }
            
            StatusMessage("ÁØºñ");
        }

        /// <summary>
        /// Ã¤³Î¸ñ·Ï Á¶È¸
        /// </summary>
        private void SearchMenuPower()
        {
            StatusMessage("Ã¤³Î¸ñ·ÏÀ» Á¶È¸ÇÕ´Ï´Ù.");

            try
            {				
                menuModel.Init();
                menuModel.UserClassCode    = keyUserClassCode;

                // Ã¤³Î¸ñ·ÏÁ¶È¸ ¼­ºñ½º¸¦ È£ÃâÇÑ´Ù.
                new MenuManager(systemModel,commonModel).GetMenuPowerList(menuModel);

                if (menuModel.ResultCD.Equals("0000"))
                {
                    Utility.SetDataTable(menuPowerDs.MenuPower, menuModel.MenuDataSet);				
                    StatusMessage(menuModel.ResultCnt + "°ÇÀÇ Ã¤³Î Á¤º¸°¡ Á¶È¸µÇ¾ú½À´Ï´Ù.");
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
    
        /// <summary>
        /// »ç¿ëÀÚÁ¤º¸ ÀúÀå
        /// </summary>
        private void SaveUserClass()
        {
            StatusMessage("»ç¿ëÀÚÁ¤º¸¸¦ ÀúÀåÇÕ´Ï´Ù.");

            if(ebUserClassCode.Text.Trim().Length == 0) 
            {
                MessageBox.Show("»ç¿ëÀÚ ÄÚµå°¡  ÀÔ·ÂµÇÁö ¾Ê¾Ò½À´Ï´Ù.","»ç¿ëÀÚÁ¤º¸ ÀúÀå", 
                    MessageBoxButtons.OK, MessageBoxIcon.Information );
                ebUserClassCode.Focus();
                return;							
            }
            if(ebUserClassCode.Text.Trim().Length > 2) 
            {
                MessageBox.Show("»ç¿ëÀÚ ÄÚµå´Â 2ÀÚ¸®±îÁö ÀÔ·ÂµË´Ï´Ù.´Ù½Ã ÀÔ·ÂÇØÁÖ¼¼¿ä.","»ç¿ëÀÚÁ¤º¸ ÀúÀå", 
                    MessageBoxButtons.OK, MessageBoxIcon.Information );
                ebUserClassCode.Text = "";
                ebUserClassCode.Focus();
                return;							
            }
            if(ebUserClassName.Text.Trim().Length == 0) 
            {
                MessageBox.Show("±¸ºÐ¸íÀÌ  ÀÔ·ÂµÇÁö ¾Ê¾Ò½À´Ï´Ù.","»ç¿ëÀÚÁ¤º¸ ÀúÀå", 
                    MessageBoxButtons.OK, MessageBoxIcon.Information );
                ebUserClassName.Focus();
                return;							
            }

            try
            {

                menuModel.UserClassCode            = ebUserClassCode.Text.Trim();
                menuModel.UserClassName         = ebUserClassName.Text.Trim();

                // »ç¿ëÀÚÁ¤º¸ ÀúÀå ¼­ºñ½º¸¦ È£ÃâÇÑ´Ù.
                if (IsAdding)
                {
                    new MenuManager(systemModel,commonModel).SetUserClassAdd(menuModel);
                    StatusMessage("»ç¿ëÀÚÁ¤º¸°¡ Ãß°¡µÇ¾ú½À´Ï´Ù.");
                    
                    ResetDetailText();
                }
                else
                {
                    //±×¸®µåÀÇ Ã¼Å©¹Ú½º µ¥ÀÌÅÍ¸¦ µ¥ÀÌÅÍ¼Â ÅëÂ°·Î ³Ñ±ä´Ù.
                    menuModel.MenuDataSet = menuPowerDs.Copy();
                    
                    new MenuManager(systemModel,commonModel).SetUserClassUpdate(menuModel);
                    StatusMessage("»ç¿ëÀÚÁ¤º¸°¡ ÀúÀåµÇ¾ú½À´Ï´Ù.");
                }
                    
                DisableButton();
                SearchUserClass();
                IsAdding = false;
                InitButton();
					
            }
            catch(FrameException fe)
            {
                FrameSystem.showMsgForm("»ç¿ëÀÚÁ¤º¸ ÀúÀå¿À·ù", new string[] {fe.ErrCode, fe.ResultMsg});
            }
            catch(Exception ex)
            {
                FrameSystem.showMsgForm("»ç¿ëÀÚÁ¤º¸ ÀúÀå¿À·ù",new string[] {"",ex.Message});
            }			
        }

        
        private void ResetDetailText()
        {	
            ebUserClassCode.Text = "";
            ebUserClassName.Text = "";
        }

        /// <summary>
        /// »ó¼¼Á¤º¸ ReadOnly
        /// </summary>
        private void SetTextReadonly()
        {
            ebUserClassCode.ReadOnly        = true;
            ebUserClassCode.BackColor       = Color.WhiteSmoke;
        }

        /// <summary>
        /// »ó¼¼Á¤º¸ ¼öÁ¤°¡´ÉÄÉ
        /// </summary>
        private void ResetTextReadonly()
        {
            ebUserClassCode.ReadOnly        = false;
            ebUserClassCode.BackColor       = Color.White;
        }


        /// <summary>
        /// Å°¯“À»Ã£¾Æ ±×¸®µå Å°¿¡ ÇØ´çµÇ´Â·Î¿ì·Î..
        /// </summary>
        private void AddMenuPower()
        {
            StatusMessage("Å°¯“");		

            try
            {
                int rowIndex = 0;
                if ( menuPowerDs.Tables["UserClass"].Rows.Count < 1 ) return;
              
                foreach (DataRow row in menuPowerDs.Tables["UserClass"].Rows)
                {					
                    if(IsAdding)
                    {
                        cmUserClass.Position = 0;
                        keyUserClassCode = null;						
                    }
                    else
                    {						
                        if(row["UserClassCode"].ToString().Equals(keyUserClassCode))
                        {					
                            cmUserClass.Position = rowIndex;
                            break;								
                        }
                    }

                    rowIndex++;
                    grdExUserClassList.EnsureVisible();
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