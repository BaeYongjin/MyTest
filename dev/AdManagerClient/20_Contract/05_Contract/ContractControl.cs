// ===============================================================================
// AdFileControl for Charites Project
//
// AdFileControl.cs
//
// °è¾àÁ¤º¸°ü¸® ÄÁµå·ÑÀ» Á¤ÀÇÇÕ´Ï´Ù. 
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
    /// °è¾àÁ¤º¸°ü¸® ÄÁÆ®·Ñ
    /// </summary>
    public class ContractControl : System.Windows.Forms.UserControl, IUserControl
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
        ContractModel contractModel  = new ContractModel();	// °è¾àÁ¤º¸¸ðµ¨

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

        string MediaCode      = "";
        string RapCode        = "";
        string AgencyCode     = "";
        string AdvertiserCode = "";

		string adtime = "";
		string bonusrate = "";
		string contractAmt1 = "";
		string price = "";
		string packageName = "";
		string jobCode = "";

        private string        Contract_Seq = null;		

		private decimal BonusRate1 = 0;
		private decimal LongBonus = 0;
		private decimal SpecialBonus = 0;
		private decimal TotalBonus = 0;
		private decimal EbPrice = 0;

		private decimal ContractAmt = 0;
		private decimal TotalTgt = 0;
		private decimal SecurityTgt = 0;
		
		

        // Key µ¥ÀÌÅÍ
        string keyContractSeq       = "";

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
        private System.Windows.Forms.Panel pnlUserDetail;
        private Janus.Windows.UI.Dock.UIPanel uiPanelSearch;
        private Janus.Windows.UI.Dock.UIPanelInnerContainer uiPanelSearchContainer;
        private Janus.Windows.UI.Dock.UIPanel uiPanelList;
        private Janus.Windows.UI.Dock.UIPanelInnerContainer uiPanelListContainer;
        private Janus.Windows.UI.Dock.UIPanel uiPanelDetail;
        private Janus.Windows.UI.Dock.UIPanelInnerContainer uiPanelDetailContainer;
        private Janus.Windows.UI.Dock.UIPanelGroup uiPanelContract;
        private Janus.Windows.GridEX.EditControls.EditBox ebSearchKey;
        private Janus.Windows.EditControls.UIComboBox cbSearchMedia;
        private Janus.Windows.EditControls.UIComboBox cbSearchRap;
        private Janus.Windows.EditControls.UIComboBox cbSearchAgency;
        private Janus.Windows.EditControls.UIComboBox cbSearchAdvertiser;
        private System.Data.DataView dvContract;
        private AdManagerClient.ContractDs contractDs;
        private Janus.Windows.GridEX.GridEX grdExContractList;
        private System.Windows.Forms.Panel panel1;
        private Janus.Windows.GridEX.GridEX gridEX1;
        private Janus.Windows.EditControls.UIComboBox cbSearchContractState;
        private Janus.Windows.EditControls.UIButton btnDelete;
        private Janus.Windows.EditControls.UIButton btnAdd;
        private Janus.Windows.EditControls.UIButton btnSave;
		private Janus.Windows.UI.Tab.UITab uiTab1;
		private Janus.Windows.UI.Tab.UITabPage uiTabPage1;
		private Janus.Windows.UI.Tab.UITabPage uiTabPage2;
		private Janus.Windows.GridEX.EditControls.EditBox ebAdvertiser;
		private Janus.Windows.GridEX.EditControls.EditBox ebRap;
		private Janus.Windows.GridEX.EditControls.EditBox ebMedia;
		private Janus.Windows.EditControls.UIComboBox cbContractState;
		private Janus.Windows.GridEX.EditControls.EditBox ebComment;
		private System.Windows.Forms.Label lbContStarDay;
		private System.Windows.Forms.Label lbContEndDay;
		private Janus.Windows.GridEX.EditControls.EditBox ebRegName;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label lbContractName;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label lbRegID;
		private System.Windows.Forms.Label label2;
		private Janus.Windows.GridEX.EditControls.EditBox ebModDt;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label lbModDt;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Label lbRegDt;
		private Janus.Windows.GridEX.EditControls.EditBox ebRegDt;
		private Janus.Windows.GridEX.EditControls.EditBox ebContractName;
		private Janus.Windows.GridEX.EditControls.NumericEditBox ebBonusRate;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.Label label8;
		private Janus.Windows.GridEX.EditControls.NumericEditBox ebPrice;
		private Janus.Windows.GridEX.EditControls.NumericEditBox ebAdTime;
		private System.Windows.Forms.Label lbAdTime;
		private System.Windows.Forms.Label label10;
		private Janus.Windows.EditControls.UIButton btnPackage;
		private Janus.Windows.CalendarCombo.CalendarCombo ebContEndDay;
		private Janus.Windows.CalendarCombo.CalendarCombo ebContStartDay;
		private Janus.Windows.EditControls.UIButton btnAdvertise;
		private Janus.Windows.GridEX.EditControls.EditBox ebAgency;
		private System.Windows.Forms.Panel panel2;
		private Janus.Windows.GridEX.GridEX grdExItemList;
		private System.Data.DataView dvItem;
		private System.Windows.Forms.Label lbJob;
		private Janus.Windows.EditControls.UIButton btnJob;
		private Janus.Windows.GridEX.EditControls.EditBox ebJob3;
		private Janus.Windows.GridEX.EditControls.EditBox ebJob2;
		private Janus.Windows.GridEX.EditControls.EditBox ebJob;
		private System.Windows.Forms.Label label14;
		private System.Windows.Forms.Label label16;
		private System.Windows.Forms.Label label18;
		private System.Windows.Forms.Label lbPackageName;
		private Janus.Windows.GridEX.EditControls.EditBox ebPackageName;
		private System.Windows.Forms.Label lbBonus;
		private System.Windows.Forms.Label lbLongBonus;
		private Janus.Windows.GridEX.EditControls.NumericEditBox ebLongBonus;
		private System.Windows.Forms.Label lbSpecialBonus;
		private Janus.Windows.GridEX.EditControls.NumericEditBox ebSpecialBonus;
		private System.Windows.Forms.Label lbTotalBonus;
		private Janus.Windows.GridEX.EditControls.NumericEditBox ebTotalBonus;
		private System.Windows.Forms.Label lbTotalTgt;
		private Janus.Windows.GridEX.EditControls.NumericEditBox ebTotalTgt;
		private Janus.Windows.GridEX.EditControls.NumericEditBox ebContractAmt;
		private System.Windows.Forms.Label lbContractAmt;
		private System.Windows.Forms.Label lbSecurityTgt;
		private Janus.Windows.GridEX.EditControls.NumericEditBox ebSecurityTgt;


        private System.ComponentModel.IContainer components;

        public ContractControl()
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
            Janus.Windows.GridEX.GridEXLayout grdExContractList_DesignTimeLayout = new Janus.Windows.GridEX.GridEXLayout();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ContractControl));
            Janus.Windows.GridEX.GridEXLayout grdExItemList_DesignTimeLayout = new Janus.Windows.GridEX.GridEXLayout();
            this.uiPM = new Janus.Windows.UI.Dock.UIPanelManager(this.components);
            this.uiPanelContract = new Janus.Windows.UI.Dock.UIPanelGroup();
            this.uiPanelSearch = new Janus.Windows.UI.Dock.UIPanel();
            this.uiPanelSearchContainer = new Janus.Windows.UI.Dock.UIPanelInnerContainer();
            this.pnlSearch = new System.Windows.Forms.Panel();
            this.cbSearchContractState = new Janus.Windows.EditControls.UIComboBox();
            this.ebSearchKey = new Janus.Windows.GridEX.EditControls.EditBox();
            this.cbSearchMedia = new Janus.Windows.EditControls.UIComboBox();
            this.cbSearchRap = new Janus.Windows.EditControls.UIComboBox();
            this.cbSearchAgency = new Janus.Windows.EditControls.UIComboBox();
            this.cbSearchAdvertiser = new Janus.Windows.EditControls.UIComboBox();
            this.btnSearch = new Janus.Windows.EditControls.UIButton();
            this.uiPanelList = new Janus.Windows.UI.Dock.UIPanel();
            this.uiPanelListContainer = new Janus.Windows.UI.Dock.UIPanelInnerContainer();
            this.grdExContractList = new Janus.Windows.GridEX.GridEX();
            this.dvContract = new System.Data.DataView();
            this.contractDs = new AdManagerClient.ContractDs();
            this.uiPanelDetail = new Janus.Windows.UI.Dock.UIPanel();
            this.uiPanelDetailContainer = new Janus.Windows.UI.Dock.UIPanelInnerContainer();
            this.pnlUserDetail = new System.Windows.Forms.Panel();
            this.btnAdd = new Janus.Windows.EditControls.UIButton();
            this.btnDelete = new Janus.Windows.EditControls.UIButton();
            this.btnSave = new Janus.Windows.EditControls.UIButton();
            this.uiTab1 = new Janus.Windows.UI.Tab.UITab();
            this.uiTabPage1 = new Janus.Windows.UI.Tab.UITabPage();
            this.ebSpecialBonus = new Janus.Windows.GridEX.EditControls.NumericEditBox();
            this.ebTotalTgt = new Janus.Windows.GridEX.EditControls.NumericEditBox();
            this.lbTotalTgt = new System.Windows.Forms.Label();
            this.ebTotalBonus = new Janus.Windows.GridEX.EditControls.NumericEditBox();
            this.label18 = new System.Windows.Forms.Label();
            this.lbTotalBonus = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.lbSpecialBonus = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.ebLongBonus = new Janus.Windows.GridEX.EditControls.NumericEditBox();
            this.lbLongBonus = new System.Windows.Forms.Label();
            this.ebPackageName = new Janus.Windows.GridEX.EditControls.EditBox();
            this.lbPackageName = new System.Windows.Forms.Label();
            this.ebContractAmt = new Janus.Windows.GridEX.EditControls.NumericEditBox();
            this.lbContractAmt = new System.Windows.Forms.Label();
            this.btnPackage = new Janus.Windows.EditControls.UIButton();
            this.ebJob2 = new Janus.Windows.GridEX.EditControls.EditBox();
            this.ebJob = new Janus.Windows.GridEX.EditControls.EditBox();
            this.btnJob = new Janus.Windows.EditControls.UIButton();
            this.ebJob3 = new Janus.Windows.GridEX.EditControls.EditBox();
            this.lbJob = new System.Windows.Forms.Label();
            this.ebAdvertiser = new Janus.Windows.GridEX.EditControls.EditBox();
            this.ebRap = new Janus.Windows.GridEX.EditControls.EditBox();
            this.ebMedia = new Janus.Windows.GridEX.EditControls.EditBox();
            this.lbSecurityTgt = new System.Windows.Forms.Label();
            this.cbContractState = new Janus.Windows.EditControls.UIComboBox();
            this.ebComment = new Janus.Windows.GridEX.EditControls.EditBox();
            this.lbContStarDay = new System.Windows.Forms.Label();
            this.lbContEndDay = new System.Windows.Forms.Label();
            this.ebRegName = new Janus.Windows.GridEX.EditControls.EditBox();
            this.label1 = new System.Windows.Forms.Label();
            this.lbContractName = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.lbRegID = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.ebModDt = new Janus.Windows.GridEX.EditControls.EditBox();
            this.label3 = new System.Windows.Forms.Label();
            this.lbModDt = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.lbRegDt = new System.Windows.Forms.Label();
            this.ebRegDt = new Janus.Windows.GridEX.EditControls.EditBox();
            this.ebContractName = new Janus.Windows.GridEX.EditControls.EditBox();
            this.ebBonusRate = new Janus.Windows.GridEX.EditControls.NumericEditBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.ebPrice = new Janus.Windows.GridEX.EditControls.NumericEditBox();
            this.lbBonus = new System.Windows.Forms.Label();
            this.ebAdTime = new Janus.Windows.GridEX.EditControls.NumericEditBox();
            this.lbAdTime = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.ebSecurityTgt = new Janus.Windows.GridEX.EditControls.NumericEditBox();
            this.ebContEndDay = new Janus.Windows.CalendarCombo.CalendarCombo();
            this.ebContStartDay = new Janus.Windows.CalendarCombo.CalendarCombo();
            this.btnAdvertise = new Janus.Windows.EditControls.UIButton();
            this.ebAgency = new Janus.Windows.GridEX.EditControls.EditBox();
            this.uiTabPage2 = new Janus.Windows.UI.Tab.UITabPage();
            this.panel2 = new System.Windows.Forms.Panel();
            this.grdExItemList = new Janus.Windows.GridEX.GridEX();
            this.dvItem = new System.Data.DataView();
            this.panel1 = new System.Windows.Forms.Panel();
            this.gridEX1 = new Janus.Windows.GridEX.GridEX();
            ((System.ComponentModel.ISupportInitialize)(this.uiPM)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelContract)).BeginInit();
            this.uiPanelContract.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelSearch)).BeginInit();
            this.uiPanelSearch.SuspendLayout();
            this.uiPanelSearchContainer.SuspendLayout();
            this.pnlSearch.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelList)).BeginInit();
            this.uiPanelList.SuspendLayout();
            this.uiPanelListContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdExContractList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dvContract)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.contractDs)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelDetail)).BeginInit();
            this.uiPanelDetail.SuspendLayout();
            this.uiPanelDetailContainer.SuspendLayout();
            this.pnlUserDetail.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uiTab1)).BeginInit();
            this.uiTab1.SuspendLayout();
            this.uiTabPage1.SuspendLayout();
            this.uiTabPage2.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdExItemList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dvItem)).BeginInit();
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
            this.uiPanelContract.Id = new System.Guid("b162c53a-7940-47cd-a0ed-9762c59c50d8");
            this.uiPanelContract.StaticGroup = true;
            this.uiPanelSearch.Id = new System.Guid("afb3b7d0-aa79-443c-a1e8-3baba38f7a4d");
            this.uiPanelContract.Panels.Add(this.uiPanelSearch);
            this.uiPanelList.Id = new System.Guid("8e3140b9-bfcb-43af-b84e-2ee6cddf51dc");
            this.uiPanelContract.Panels.Add(this.uiPanelList);
            this.uiPanelDetail.Id = new System.Guid("b2973a57-d3e1-4d80-9387-7a6dc3332542");
            this.uiPanelContract.Panels.Add(this.uiPanelDetail);
            this.uiPM.Panels.Add(this.uiPanelContract);
            // 
            // Design Time Panel Info:
            // 
            this.uiPM.BeginPanelInfo();
            this.uiPM.AddDockPanelInfo(new System.Guid("b162c53a-7940-47cd-a0ed-9762c59c50d8"), Janus.Windows.UI.Dock.PanelGroupStyle.HorizontalTiles, Janus.Windows.UI.Dock.PanelDockStyle.Fill, true, new System.Drawing.Size(1010, 677), true);
            this.uiPM.AddDockPanelInfo(new System.Guid("afb3b7d0-aa79-443c-a1e8-3baba38f7a4d"), new System.Guid("b162c53a-7940-47cd-a0ed-9762c59c50d8"), 43, true);
            this.uiPM.AddDockPanelInfo(new System.Guid("8e3140b9-bfcb-43af-b84e-2ee6cddf51dc"), new System.Guid("b162c53a-7940-47cd-a0ed-9762c59c50d8"), 244, true);
            this.uiPM.AddDockPanelInfo(new System.Guid("b2973a57-d3e1-4d80-9387-7a6dc3332542"), new System.Guid("b162c53a-7940-47cd-a0ed-9762c59c50d8"), 360, true);
            this.uiPM.AddFloatingPanelInfo(new System.Guid("b162c53a-7940-47cd-a0ed-9762c59c50d8"), Janus.Windows.UI.Dock.PanelGroupStyle.HorizontalTiles, true, new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
            this.uiPM.AddFloatingPanelInfo(new System.Guid("8e3140b9-bfcb-43af-b84e-2ee6cddf51dc"), new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
            this.uiPM.AddFloatingPanelInfo(new System.Guid("b2973a57-d3e1-4d80-9387-7a6dc3332542"), new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
            this.uiPM.AddFloatingPanelInfo(new System.Guid("afb3b7d0-aa79-443c-a1e8-3baba38f7a4d"), new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
            this.uiPM.EndPanelInfo();
            // 
            // uiPanelContract
            // 
            this.uiPanelContract.ActiveCaptionMode = Janus.Windows.UI.Dock.ActiveCaptionMode.Never;
            this.uiPanelContract.AllowResize = Janus.Windows.UI.InheritableBoolean.False;
            this.uiPanelContract.BackColor = System.Drawing.SystemColors.Window;
            this.uiPanelContract.CaptionVisible = Janus.Windows.UI.InheritableBoolean.True;
            this.uiPanelContract.CloseButtonVisible = Janus.Windows.UI.InheritableBoolean.False;
            this.uiPanelContract.Location = new System.Drawing.Point(0, 0);
            this.uiPanelContract.Name = "uiPanelContract";
            this.uiPanelContract.Size = new System.Drawing.Size(1010, 677);
            this.uiPanelContract.TabIndex = 4;
            this.uiPanelContract.Text = "±¤°í°è¾à°ü¸®";
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
            this.pnlSearch.Controls.Add(this.cbSearchContractState);
            this.pnlSearch.Controls.Add(this.ebSearchKey);
            this.pnlSearch.Controls.Add(this.cbSearchMedia);
            this.pnlSearch.Controls.Add(this.cbSearchRap);
            this.pnlSearch.Controls.Add(this.cbSearchAgency);
            this.pnlSearch.Controls.Add(this.cbSearchAdvertiser);
            this.pnlSearch.Controls.Add(this.btnSearch);
            this.pnlSearch.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlSearch.Location = new System.Drawing.Point(0, 0);
            this.pnlSearch.Name = "pnlSearch";
            this.pnlSearch.Size = new System.Drawing.Size(1008, 41);
            this.pnlSearch.TabIndex = 0;
            // 
            // cbSearchContractState
            // 
            this.cbSearchContractState.BackColor = System.Drawing.Color.White;
            this.cbSearchContractState.ComboStyle = Janus.Windows.EditControls.ComboStyle.DropDownList;
            this.cbSearchContractState.Location = new System.Drawing.Point(518, 8);
            this.cbSearchContractState.Name = "cbSearchContractState";
            this.cbSearchContractState.Size = new System.Drawing.Size(120, 21);
            this.cbSearchContractState.TabIndex = 6;
            this.cbSearchContractState.Text = "°è¾à»óÅÂ";
            this.cbSearchContractState.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            // 
            // ebSearchKey
            // 
            this.ebSearchKey.Location = new System.Drawing.Point(644, 8);
            this.ebSearchKey.Name = "ebSearchKey";
            this.ebSearchKey.Size = new System.Drawing.Size(200, 21);
            this.ebSearchKey.TabIndex = 5;
            this.ebSearchKey.Text = "°Ë»ö¾î¸¦ ÀÔ·ÂÇÏ¼¼¿ä";
            this.ebSearchKey.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
            this.ebSearchKey.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            this.ebSearchKey.TextChanged += new System.EventHandler(this.ebSearchKey_TextChanged);
            this.ebSearchKey.Click += new System.EventHandler(this.ebSearchKey_Click);
            this.ebSearchKey.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ebSearchKey_KeyDown);
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
            // cbSearchRap
            // 
            this.cbSearchRap.ComboStyle = Janus.Windows.EditControls.ComboStyle.DropDownList;
            this.cbSearchRap.Location = new System.Drawing.Point(135, 8);
            this.cbSearchRap.Name = "cbSearchRap";
            this.cbSearchRap.Size = new System.Drawing.Size(120, 21);
            this.cbSearchRap.TabIndex = 2;
            this.cbSearchRap.Text = "¹Ìµð¾î·¾¼±ÅÃ";
            this.cbSearchRap.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            // 
            // cbSearchAgency
            // 
            this.cbSearchAgency.ComboStyle = Janus.Windows.EditControls.ComboStyle.DropDownList;
            this.cbSearchAgency.Location = new System.Drawing.Point(264, 8);
            this.cbSearchAgency.Name = "cbSearchAgency";
            this.cbSearchAgency.Size = new System.Drawing.Size(120, 21);
            this.cbSearchAgency.TabIndex = 3;
            this.cbSearchAgency.Text = "´ëÇà»ç¼±ÅÃ";
            this.cbSearchAgency.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            // 
            // cbSearchAdvertiser
            // 
            this.cbSearchAdvertiser.ComboStyle = Janus.Windows.EditControls.ComboStyle.DropDownList;
            this.cbSearchAdvertiser.Location = new System.Drawing.Point(392, 8);
            this.cbSearchAdvertiser.Name = "cbSearchAdvertiser";
            this.cbSearchAdvertiser.Size = new System.Drawing.Size(120, 21);
            this.cbSearchAdvertiser.TabIndex = 4;
            this.cbSearchAdvertiser.Text = "±¤°íÁÖ¼±ÅÃ";
            this.cbSearchAdvertiser.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            // 
            // btnSearch
            // 
            this.btnSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSearch.Enabled = false;
            this.btnSearch.Location = new System.Drawing.Point(895, 8);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(104, 24);
            this.btnSearch.TabIndex = 7;
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
            this.uiPanelList.Size = new System.Drawing.Size(1010, 244);
            this.uiPanelList.TabIndex = 8;
            this.uiPanelList.TabStop = false;
            this.uiPanelList.Text = "±¤°í°è¾à¸ñ·Ï";
            // 
            // uiPanelListContainer
            // 
            this.uiPanelListContainer.BackColor = System.Drawing.SystemColors.Window;
            this.uiPanelListContainer.Controls.Add(this.grdExContractList);
            this.uiPanelListContainer.Location = new System.Drawing.Point(1, 23);
            this.uiPanelListContainer.Name = "uiPanelListContainer";
            this.uiPanelListContainer.Size = new System.Drawing.Size(1008, 220);
            this.uiPanelListContainer.TabIndex = 0;
            // 
            // grdExContractList
            // 
            this.grdExContractList.AllowEdit = Janus.Windows.GridEX.InheritableBoolean.False;
            this.grdExContractList.AlternatingColors = true;
            this.grdExContractList.BorderStyle = Janus.Windows.GridEX.BorderStyle.None;
            this.grdExContractList.DataSource = this.dvContract;
            grdExContractList_DesignTimeLayout.LayoutString = resources.GetString("grdExContractList_DesignTimeLayout.LayoutString");
            this.grdExContractList.DesignTimeLayout = grdExContractList_DesignTimeLayout;
            this.grdExContractList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdExContractList.EditorsControlStyle.ButtonAppearance = Janus.Windows.GridEX.ButtonAppearance.Regular;
            this.grdExContractList.EmptyRows = true;
            this.grdExContractList.FocusCellFormatStyle.BackColor = System.Drawing.SystemColors.Highlight;
            this.grdExContractList.FocusCellFormatStyle.ForeColor = System.Drawing.Color.White;
            this.grdExContractList.FocusStyle = Janus.Windows.GridEX.FocusStyle.None;
            this.grdExContractList.GridLineColor = System.Drawing.Color.Silver;
            this.grdExContractList.GridLines = Janus.Windows.GridEX.GridLines.Vertical;
            this.grdExContractList.GridLineStyle = Janus.Windows.GridEX.GridLineStyle.Solid;
            this.grdExContractList.GroupByBoxVisible = false;
            this.grdExContractList.HideSelection = Janus.Windows.GridEX.HideSelection.HighlightInactive;
            this.grdExContractList.Location = new System.Drawing.Point(0, 0);
            this.grdExContractList.Name = "grdExContractList";
            this.grdExContractList.ScrollBars = Janus.Windows.GridEX.ScrollBars.Vertical;
            this.grdExContractList.SelectedInactiveFormatStyle.BackColor = System.Drawing.Color.Gold;
            this.grdExContractList.Size = new System.Drawing.Size(1008, 220);
            this.grdExContractList.TabIndex = 9;
            this.grdExContractList.TabKeyBehavior = Janus.Windows.GridEX.TabKeyBehavior.ControlNavigation;
            this.grdExContractList.ThemedAreas = ((Janus.Windows.GridEX.ThemedArea)(((((((((Janus.Windows.GridEX.ThemedArea.ScrollBars | Janus.Windows.GridEX.ThemedArea.EditControls) 
            | Janus.Windows.GridEX.ThemedArea.Headers) 
            | Janus.Windows.GridEX.ThemedArea.GroupByBox) 
            | Janus.Windows.GridEX.ThemedArea.GroupRows) 
            | Janus.Windows.GridEX.ThemedArea.ControlBorder) 
            | Janus.Windows.GridEX.ThemedArea.Cards) 
            | Janus.Windows.GridEX.ThemedArea.Gridlines) 
            | Janus.Windows.GridEX.ThemedArea.CheckBoxes)));
            this.grdExContractList.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            this.grdExContractList.Enter += new System.EventHandler(this.OnGrdRowChanged);
            // 
            // dvContract
            // 
            this.dvContract.Table = this.contractDs.Contract;
            // 
            // contractDs
            // 
            this.contractDs.DataSetName = "ContractDs";
            this.contractDs.Locale = new System.Globalization.CultureInfo("en-US");
            this.contractDs.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // uiPanelDetail
            // 
            this.uiPanelDetail.AllowResize = Janus.Windows.UI.InheritableBoolean.False;
            this.uiPanelDetail.BackColor = System.Drawing.SystemColors.Window;
            this.uiPanelDetail.CaptionStyle = Janus.Windows.UI.Dock.PanelCaptionStyle.Dark;
            this.uiPanelDetail.InnerContainer = this.uiPanelDetailContainer;
            this.uiPanelDetail.Location = new System.Drawing.Point(0, 317);
            this.uiPanelDetail.Name = "uiPanelDetail";
            this.uiPanelDetail.Size = new System.Drawing.Size(1010, 360);
            this.uiPanelDetail.TabIndex = 10;
            this.uiPanelDetail.TabStop = false;
            this.uiPanelDetail.Text = "»ó¼¼Á¤º¸";
            // 
            // uiPanelDetailContainer
            // 
            this.uiPanelDetailContainer.Controls.Add(this.pnlUserDetail);
            this.uiPanelDetailContainer.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.uiPanelDetailContainer.Location = new System.Drawing.Point(1, 23);
            this.uiPanelDetailContainer.Name = "uiPanelDetailContainer";
            this.uiPanelDetailContainer.Size = new System.Drawing.Size(1008, 336);
            this.uiPanelDetailContainer.TabIndex = 0;
            // 
            // pnlUserDetail
            // 
            this.pnlUserDetail.BackColor = System.Drawing.SystemColors.Window;
            this.pnlUserDetail.Controls.Add(this.btnAdd);
            this.pnlUserDetail.Controls.Add(this.btnDelete);
            this.pnlUserDetail.Controls.Add(this.btnSave);
            this.pnlUserDetail.Controls.Add(this.uiTab1);
            this.pnlUserDetail.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlUserDetail.Location = new System.Drawing.Point(0, 0);
            this.pnlUserDetail.Name = "pnlUserDetail";
            this.pnlUserDetail.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.pnlUserDetail.Size = new System.Drawing.Size(1008, 336);
            this.pnlUserDetail.TabIndex = 29;
            // 
            // btnAdd
            // 
            this.btnAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnAdd.BackColor = System.Drawing.SystemColors.Window;
            this.btnAdd.Enabled = false;
            this.btnAdd.Location = new System.Drawing.Point(231, 306);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(104, 24);
            this.btnAdd.TabIndex = 32;
            this.btnAdd.Text = "Ãß °¡";
            this.btnAdd.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnDelete.BackColor = System.Drawing.SystemColors.Window;
            this.btnDelete.Enabled = false;
            this.btnDelete.Location = new System.Drawing.Point(119, 306);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(104, 24);
            this.btnDelete.TabIndex = 31;
            this.btnDelete.TabStop = false;
            this.btnDelete.Text = "»è Á¦";
            this.btnDelete.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnSave.BackColor = System.Drawing.SystemColors.Window;
            this.btnSave.Enabled = false;
            this.btnSave.Location = new System.Drawing.Point(8, 306);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(104, 24);
            this.btnSave.TabIndex = 30;
            this.btnSave.Text = "Àú Àå";
            this.btnSave.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // uiTab1
            // 
            this.uiTab1.Location = new System.Drawing.Point(16, 8);
            this.uiTab1.Name = "uiTab1";
            this.uiTab1.Size = new System.Drawing.Size(983, 288);
            this.uiTab1.TabIndex = 10;
            this.uiTab1.TabPages.AddRange(new Janus.Windows.UI.Tab.UITabPage[] {
            this.uiTabPage1,
            this.uiTabPage2});
            // 
            // uiTabPage1
            // 
            this.uiTabPage1.Controls.Add(this.ebSpecialBonus);
            this.uiTabPage1.Controls.Add(this.ebTotalTgt);
            this.uiTabPage1.Controls.Add(this.lbTotalTgt);
            this.uiTabPage1.Controls.Add(this.ebTotalBonus);
            this.uiTabPage1.Controls.Add(this.label18);
            this.uiTabPage1.Controls.Add(this.lbTotalBonus);
            this.uiTabPage1.Controls.Add(this.label16);
            this.uiTabPage1.Controls.Add(this.lbSpecialBonus);
            this.uiTabPage1.Controls.Add(this.label14);
            this.uiTabPage1.Controls.Add(this.ebLongBonus);
            this.uiTabPage1.Controls.Add(this.lbLongBonus);
            this.uiTabPage1.Controls.Add(this.ebPackageName);
            this.uiTabPage1.Controls.Add(this.lbPackageName);
            this.uiTabPage1.Controls.Add(this.ebContractAmt);
            this.uiTabPage1.Controls.Add(this.lbContractAmt);
            this.uiTabPage1.Controls.Add(this.btnPackage);
            this.uiTabPage1.Controls.Add(this.ebJob2);
            this.uiTabPage1.Controls.Add(this.ebJob);
            this.uiTabPage1.Controls.Add(this.btnJob);
            this.uiTabPage1.Controls.Add(this.ebJob3);
            this.uiTabPage1.Controls.Add(this.lbJob);
            this.uiTabPage1.Controls.Add(this.ebAdvertiser);
            this.uiTabPage1.Controls.Add(this.ebRap);
            this.uiTabPage1.Controls.Add(this.ebMedia);
            this.uiTabPage1.Controls.Add(this.lbSecurityTgt);
            this.uiTabPage1.Controls.Add(this.cbContractState);
            this.uiTabPage1.Controls.Add(this.ebComment);
            this.uiTabPage1.Controls.Add(this.lbContStarDay);
            this.uiTabPage1.Controls.Add(this.lbContEndDay);
            this.uiTabPage1.Controls.Add(this.ebRegName);
            this.uiTabPage1.Controls.Add(this.label1);
            this.uiTabPage1.Controls.Add(this.lbContractName);
            this.uiTabPage1.Controls.Add(this.label4);
            this.uiTabPage1.Controls.Add(this.lbRegID);
            this.uiTabPage1.Controls.Add(this.label2);
            this.uiTabPage1.Controls.Add(this.ebModDt);
            this.uiTabPage1.Controls.Add(this.label3);
            this.uiTabPage1.Controls.Add(this.lbModDt);
            this.uiTabPage1.Controls.Add(this.label5);
            this.uiTabPage1.Controls.Add(this.label6);
            this.uiTabPage1.Controls.Add(this.lbRegDt);
            this.uiTabPage1.Controls.Add(this.ebRegDt);
            this.uiTabPage1.Controls.Add(this.ebContractName);
            this.uiTabPage1.Controls.Add(this.ebBonusRate);
            this.uiTabPage1.Controls.Add(this.label7);
            this.uiTabPage1.Controls.Add(this.label8);
            this.uiTabPage1.Controls.Add(this.ebPrice);
            this.uiTabPage1.Controls.Add(this.lbBonus);
            this.uiTabPage1.Controls.Add(this.ebAdTime);
            this.uiTabPage1.Controls.Add(this.lbAdTime);
            this.uiTabPage1.Controls.Add(this.label10);
            this.uiTabPage1.Controls.Add(this.ebSecurityTgt);
            this.uiTabPage1.Controls.Add(this.ebContEndDay);
            this.uiTabPage1.Controls.Add(this.ebContStartDay);
            this.uiTabPage1.Controls.Add(this.btnAdvertise);
            this.uiTabPage1.Controls.Add(this.ebAgency);
            this.uiTabPage1.Location = new System.Drawing.Point(1, 22);
            this.uiTabPage1.Name = "uiTabPage1";
            this.uiTabPage1.Size = new System.Drawing.Size(979, 263);
            this.uiTabPage1.TabStop = true;
            this.uiTabPage1.Text = "»ó¼¼Á¤º¸";
            // 
            // ebSpecialBonus
            // 
            this.ebSpecialBonus.DecimalDigits = 0;
            this.ebSpecialBonus.FormatString = "#,##0";
            this.ebSpecialBonus.Location = new System.Drawing.Point(434, 106);
            this.ebSpecialBonus.MaxLength = 4;
            this.ebSpecialBonus.Name = "ebSpecialBonus";
            this.ebSpecialBonus.Size = new System.Drawing.Size(62, 21);
            this.ebSpecialBonus.TabIndex = 23;
            this.ebSpecialBonus.Text = "0";
            this.ebSpecialBonus.TextAlignment = Janus.Windows.GridEX.TextAlignment.Far;
            this.ebSpecialBonus.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.ebSpecialBonus.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            this.ebSpecialBonus.TextChanged += new System.EventHandler(this.ebSpecialBonus_TextChanged);
            // 
            // ebTotalTgt
            // 
            this.ebTotalTgt.DecimalDigits = 0;
            this.ebTotalTgt.FormatString = "#,##0";
            this.ebTotalTgt.Location = new System.Drawing.Point(360, 130);
            this.ebTotalTgt.MaxLength = 17;
            this.ebTotalTgt.Name = "ebTotalTgt";
            this.ebTotalTgt.ReadOnly = true;
            this.ebTotalTgt.Size = new System.Drawing.Size(144, 21);
            this.ebTotalTgt.TabIndex = 0;
            this.ebTotalTgt.TabStop = false;
            this.ebTotalTgt.Text = "0";
            this.ebTotalTgt.TextAlignment = Janus.Windows.GridEX.TextAlignment.Far;
            this.ebTotalTgt.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.ebTotalTgt.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            // 
            // lbTotalTgt
            // 
            this.lbTotalTgt.BackColor = System.Drawing.Color.Transparent;
            this.lbTotalTgt.Location = new System.Drawing.Point(272, 130);
            this.lbTotalTgt.Name = "lbTotalTgt";
            this.lbTotalTgt.Size = new System.Drawing.Size(88, 21);
            this.lbTotalTgt.TabIndex = 326;
            this.lbTotalTgt.Text = "ÃÑº¸³Ê½º³ëÃâ";
            this.lbTotalTgt.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ebTotalBonus
            // 
            this.ebTotalBonus.DecimalDigits = 0;
            this.ebTotalBonus.FormatString = "#,##0";
            this.ebTotalBonus.Location = new System.Drawing.Point(626, 104);
            this.ebTotalBonus.MaxLength = 4;
            this.ebTotalBonus.Name = "ebTotalBonus";
            this.ebTotalBonus.ReadOnly = true;
            this.ebTotalBonus.Size = new System.Drawing.Size(72, 21);
            this.ebTotalBonus.TabIndex = 0;
            this.ebTotalBonus.TabStop = false;
            this.ebTotalBonus.Text = "0";
            this.ebTotalBonus.TextAlignment = Janus.Windows.GridEX.TextAlignment.Far;
            this.ebTotalBonus.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.ebTotalBonus.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            // 
            // label18
            // 
            this.label18.BackColor = System.Drawing.Color.Transparent;
            this.label18.Location = new System.Drawing.Point(696, 104);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(16, 21);
            this.label18.TabIndex = 325;
            this.label18.Text = "%";
            this.label18.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lbTotalBonus
            // 
            this.lbTotalBonus.BackColor = System.Drawing.Color.Transparent;
            this.lbTotalBonus.Location = new System.Drawing.Point(554, 106);
            this.lbTotalBonus.Name = "lbTotalBonus";
            this.lbTotalBonus.Size = new System.Drawing.Size(72, 21);
            this.lbTotalBonus.TabIndex = 323;
            this.lbTotalBonus.Text = "ÃÑº¸³Ê½º";
            this.lbTotalBonus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label16
            // 
            this.label16.BackColor = System.Drawing.Color.Transparent;
            this.label16.Location = new System.Drawing.Point(496, 106);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(16, 21);
            this.label16.TabIndex = 322;
            this.label16.Text = "%";
            this.label16.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lbSpecialBonus
            // 
            this.lbSpecialBonus.BackColor = System.Drawing.Color.Transparent;
            this.lbSpecialBonus.Location = new System.Drawing.Point(368, 106);
            this.lbSpecialBonus.Name = "lbSpecialBonus";
            this.lbSpecialBonus.Size = new System.Drawing.Size(72, 21);
            this.lbSpecialBonus.TabIndex = 320;
            this.lbSpecialBonus.Text = "Æ¯º°º¸³Ê½º";
            this.lbSpecialBonus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label14
            // 
            this.label14.BackColor = System.Drawing.Color.Transparent;
            this.label14.Font = new System.Drawing.Font("±¼¸²Ã¼", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label14.Location = new System.Drawing.Point(344, 106);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(16, 21);
            this.label14.TabIndex = 319;
            this.label14.Text = "%";
            this.label14.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ebLongBonus
            // 
            this.ebLongBonus.DecimalDigits = 0;
            this.ebLongBonus.FormatString = "#,##0";
            this.ebLongBonus.Location = new System.Drawing.Point(288, 106);
            this.ebLongBonus.MaxLength = 4;
            this.ebLongBonus.Name = "ebLongBonus";
            this.ebLongBonus.Size = new System.Drawing.Size(56, 21);
            this.ebLongBonus.TabIndex = 22;
            this.ebLongBonus.Text = "0";
            this.ebLongBonus.TextAlignment = Janus.Windows.GridEX.TextAlignment.Far;
            this.ebLongBonus.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.ebLongBonus.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            this.ebLongBonus.TextChanged += new System.EventHandler(this.ebLongBonus_TextChanged);
            // 
            // lbLongBonus
            // 
            this.lbLongBonus.BackColor = System.Drawing.Color.Transparent;
            this.lbLongBonus.Location = new System.Drawing.Point(216, 106);
            this.lbLongBonus.Name = "lbLongBonus";
            this.lbLongBonus.Size = new System.Drawing.Size(72, 21);
            this.lbLongBonus.TabIndex = 317;
            this.lbLongBonus.Text = "Àå±âº¸³Ê½º";
            this.lbLongBonus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ebPackageName
            // 
            this.ebPackageName.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ebPackageName.Location = new System.Drawing.Point(722, 234);
            this.ebPackageName.Name = "ebPackageName";
            this.ebPackageName.Size = new System.Drawing.Size(134, 21);
            this.ebPackageName.TabIndex = 18;
            this.ebPackageName.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
            this.ebPackageName.Visible = false;
            this.ebPackageName.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            // 
            // lbPackageName
            // 
            this.lbPackageName.BackColor = System.Drawing.Color.Transparent;
            this.lbPackageName.Location = new System.Drawing.Point(672, 234);
            this.lbPackageName.Name = "lbPackageName";
            this.lbPackageName.Size = new System.Drawing.Size(72, 21);
            this.lbPackageName.TabIndex = 315;
            this.lbPackageName.Text = "»óÇ°¸í";
            this.lbPackageName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lbPackageName.Visible = false;
            // 
            // ebContractAmt
            // 
            this.ebContractAmt.DecimalDigits = 0;
            this.ebContractAmt.FormatString = "#,##0";
            this.ebContractAmt.Location = new System.Drawing.Point(82, 130);
            this.ebContractAmt.MaxLength = 17;
            this.ebContractAmt.Name = "ebContractAmt";
            this.ebContractAmt.Size = new System.Drawing.Size(134, 21);
            this.ebContractAmt.TabIndex = 24;
            this.ebContractAmt.Text = "0";
            this.ebContractAmt.TextAlignment = Janus.Windows.GridEX.TextAlignment.Far;
            this.ebContractAmt.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.ebContractAmt.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            // 
            // lbContractAmt
            // 
            this.lbContractAmt.BackColor = System.Drawing.Color.Transparent;
            this.lbContractAmt.Location = new System.Drawing.Point(8, 130);
            this.lbContractAmt.Name = "lbContractAmt";
            this.lbContractAmt.Size = new System.Drawing.Size(72, 21);
            this.lbContractAmt.TabIndex = 313;
            this.lbContractAmt.Text = "º¸Àå³ëÃâ";
            this.lbContractAmt.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnPackage
            // 
            this.btnPackage.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnPackage.BackColor = System.Drawing.SystemColors.Window;
            this.btnPackage.Enabled = false;
            this.btnPackage.Location = new System.Drawing.Point(862, 236);
            this.btnPackage.Name = "btnPackage";
            this.btnPackage.Size = new System.Drawing.Size(104, 21);
            this.btnPackage.TabIndex = 17;
            this.btnPackage.Text = "»óÇ°°Ë»ö";
            this.btnPackage.Visible = false;
            this.btnPackage.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.btnPackage.Click += new System.EventHandler(this.btnPackage_Click);
            // 
            // ebJob2
            // 
            this.ebJob2.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ebJob2.Enabled = false;
            this.ebJob2.Location = new System.Drawing.Point(224, 234);
            this.ebJob2.Name = "ebJob2";
            this.ebJob2.ReadOnly = true;
            this.ebJob2.Size = new System.Drawing.Size(128, 21);
            this.ebJob2.TabIndex = 0;
            this.ebJob2.TabStop = false;
            this.ebJob2.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
            this.ebJob2.Visible = false;
            this.ebJob2.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            // 
            // ebJob
            // 
            this.ebJob.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ebJob.Enabled = false;
            this.ebJob.Location = new System.Drawing.Point(80, 234);
            this.ebJob.Name = "ebJob";
            this.ebJob.ReadOnly = true;
            this.ebJob.Size = new System.Drawing.Size(136, 21);
            this.ebJob.TabIndex = 0;
            this.ebJob.TabStop = false;
            this.ebJob.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
            this.ebJob.Visible = false;
            this.ebJob.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            // 
            // btnJob
            // 
            this.btnJob.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnJob.BackColor = System.Drawing.SystemColors.Window;
            this.btnJob.Enabled = false;
            this.btnJob.Location = new System.Drawing.Point(552, 233);
            this.btnJob.Name = "btnJob";
            this.btnJob.Size = new System.Drawing.Size(104, 21);
            this.btnJob.TabIndex = 28;
            this.btnJob.Text = "¾÷Á¾¼±ÅÃ";
            this.btnJob.Visible = false;
            this.btnJob.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.btnJob.Click += new System.EventHandler(this.btnJob_Click);
            // 
            // ebJob3
            // 
            this.ebJob3.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ebJob3.Enabled = false;
            this.ebJob3.Location = new System.Drawing.Point(360, 234);
            this.ebJob3.Name = "ebJob3";
            this.ebJob3.ReadOnly = true;
            this.ebJob3.Size = new System.Drawing.Size(176, 21);
            this.ebJob3.TabIndex = 0;
            this.ebJob3.TabStop = false;
            this.ebJob3.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
            this.ebJob3.Visible = false;
            this.ebJob3.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            // 
            // lbJob
            // 
            this.lbJob.BackColor = System.Drawing.Color.Transparent;
            this.lbJob.Enabled = false;
            this.lbJob.Location = new System.Drawing.Point(8, 236);
            this.lbJob.Name = "lbJob";
            this.lbJob.Size = new System.Drawing.Size(72, 21);
            this.lbJob.TabIndex = 308;
            this.lbJob.Text = "¾÷Á¾";
            this.lbJob.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lbJob.Visible = false;
            // 
            // ebAdvertiser
            // 
            this.ebAdvertiser.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ebAdvertiser.Location = new System.Drawing.Point(82, 8);
            this.ebAdvertiser.Name = "ebAdvertiser";
            this.ebAdvertiser.Size = new System.Drawing.Size(184, 21);
            this.ebAdvertiser.TabIndex = 11;
            this.ebAdvertiser.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
            this.ebAdvertiser.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            // 
            // ebRap
            // 
            this.ebRap.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ebRap.Location = new System.Drawing.Point(626, 8);
            this.ebRap.Name = "ebRap";
            this.ebRap.Size = new System.Drawing.Size(176, 21);
            this.ebRap.TabIndex = 13;
            this.ebRap.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
            this.ebRap.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            // 
            // ebMedia
            // 
            this.ebMedia.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ebMedia.Location = new System.Drawing.Point(362, 8);
            this.ebMedia.Name = "ebMedia";
            this.ebMedia.Size = new System.Drawing.Size(176, 21);
            this.ebMedia.TabIndex = 12;
            this.ebMedia.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
            this.ebMedia.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            // 
            // lbSecurityTgt
            // 
            this.lbSecurityTgt.BackColor = System.Drawing.Color.Transparent;
            this.lbSecurityTgt.Location = new System.Drawing.Point(554, 130);
            this.lbSecurityTgt.Name = "lbSecurityTgt";
            this.lbSecurityTgt.Size = new System.Drawing.Size(72, 21);
            this.lbSecurityTgt.TabIndex = 290;
            this.lbSecurityTgt.Text = "ÃÑ³ëÃâ";
            this.lbSecurityTgt.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cbContractState
            // 
            this.cbContractState.BackColor = System.Drawing.Color.White;
            this.cbContractState.ComboStyle = Janus.Windows.EditControls.ComboStyle.DropDownList;
            this.cbContractState.Location = new System.Drawing.Point(626, 32);
            this.cbContractState.Name = "cbContractState";
            this.cbContractState.Size = new System.Drawing.Size(176, 21);
            this.cbContractState.TabIndex = 15;
            this.cbContractState.Text = "°è¾à»óÅÂ";
            this.cbContractState.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            // 
            // ebComment
            // 
            this.ebComment.Location = new System.Drawing.Point(80, 182);
            this.ebComment.MaxLength = 50;
            this.ebComment.Multiline = true;
            this.ebComment.Name = "ebComment";
            this.ebComment.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.ebComment.Size = new System.Drawing.Size(456, 48);
            this.ebComment.TabIndex = 27;
            this.ebComment.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
            this.ebComment.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            // 
            // lbContStarDay
            // 
            this.lbContStarDay.BackColor = System.Drawing.Color.Transparent;
            this.lbContStarDay.Location = new System.Drawing.Point(8, 156);
            this.lbContStarDay.Name = "lbContStarDay";
            this.lbContStarDay.Size = new System.Drawing.Size(72, 21);
            this.lbContStarDay.TabIndex = 288;
            this.lbContStarDay.Text = "°è¾à½ÃÀÛÀÏ";
            this.lbContStarDay.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lbContEndDay
            // 
            this.lbContEndDay.BackColor = System.Drawing.Color.Transparent;
            this.lbContEndDay.Location = new System.Drawing.Point(288, 156);
            this.lbContEndDay.Name = "lbContEndDay";
            this.lbContEndDay.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.lbContEndDay.Size = new System.Drawing.Size(72, 21);
            this.lbContEndDay.TabIndex = 289;
            this.lbContEndDay.Text = "°è¾àÁ¾·áÀÏ";
            this.lbContEndDay.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ebRegName
            // 
            this.ebRegName.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ebRegName.Location = new System.Drawing.Point(626, 202);
            this.ebRegName.MaxLength = 15;
            this.ebRegName.Name = "ebRegName";
            this.ebRegName.ReadOnly = true;
            this.ebRegName.Size = new System.Drawing.Size(176, 21);
            this.ebRegName.TabIndex = 0;
            this.ebRegName.TabStop = false;
            this.ebRegName.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
            this.ebRegName.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Location = new System.Drawing.Point(290, 8);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(72, 21);
            this.label1.TabIndex = 277;
            this.label1.Text = "¸ÅÃ¼";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lbContractName
            // 
            this.lbContractName.BackColor = System.Drawing.Color.Transparent;
            this.lbContractName.Location = new System.Drawing.Point(8, 58);
            this.lbContractName.Name = "lbContractName";
            this.lbContractName.Size = new System.Drawing.Size(72, 21);
            this.lbContractName.TabIndex = 276;
            this.lbContractName.Text = "°è¾à¸í";
            this.lbContractName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label4
            // 
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.Location = new System.Drawing.Point(8, 8);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(72, 21);
            this.label4.TabIndex = 281;
            this.label4.Text = "±¤°íÁÖ";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lbRegID
            // 
            this.lbRegID.BackColor = System.Drawing.Color.Transparent;
            this.lbRegID.Location = new System.Drawing.Point(554, 202);
            this.lbRegID.Name = "lbRegID";
            this.lbRegID.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.lbRegID.Size = new System.Drawing.Size(72, 21);
            this.lbRegID.TabIndex = 284;
            this.lbRegID.Text = "µî·ÏÀÚ  ";
            this.lbRegID.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Location = new System.Drawing.Point(290, 32);
            this.label2.Name = "label2";
            this.label2.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.label2.Size = new System.Drawing.Size(72, 21);
            this.label2.TabIndex = 275;
            this.label2.Text = "´ëÇà»ç";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ebModDt
            // 
            this.ebModDt.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ebModDt.Location = new System.Drawing.Point(626, 178);
            this.ebModDt.Name = "ebModDt";
            this.ebModDt.ReadOnly = true;
            this.ebModDt.Size = new System.Drawing.Size(176, 21);
            this.ebModDt.TabIndex = 0;
            this.ebModDt.TabStop = false;
            this.ebModDt.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
            this.ebModDt.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            // 
            // label3
            // 
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Location = new System.Drawing.Point(554, 8);
            this.label3.Name = "label3";
            this.label3.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.label3.Size = new System.Drawing.Size(72, 21);
            this.label3.TabIndex = 278;
            this.label3.Text = "¹Ìµð¾î·¾";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lbModDt
            // 
            this.lbModDt.BackColor = System.Drawing.Color.Transparent;
            this.lbModDt.Location = new System.Drawing.Point(554, 178);
            this.lbModDt.Name = "lbModDt";
            this.lbModDt.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.lbModDt.Size = new System.Drawing.Size(72, 21);
            this.lbModDt.TabIndex = 286;
            this.lbModDt.Text = "ÃÖÁ¾¼öÁ¤";
            this.lbModDt.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label5
            // 
            this.label5.BackColor = System.Drawing.Color.Transparent;
            this.label5.Location = new System.Drawing.Point(8, 183);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(72, 21);
            this.label5.TabIndex = 287;
            this.label5.Text = "ºñ°í";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label6
            // 
            this.label6.BackColor = System.Drawing.Color.Transparent;
            this.label6.Location = new System.Drawing.Point(554, 32);
            this.label6.Name = "label6";
            this.label6.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.label6.Size = new System.Drawing.Size(72, 21);
            this.label6.TabIndex = 280;
            this.label6.Text = "°è¾à»óÅÂ";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lbRegDt
            // 
            this.lbRegDt.BackColor = System.Drawing.Color.Transparent;
            this.lbRegDt.Location = new System.Drawing.Point(554, 154);
            this.lbRegDt.Name = "lbRegDt";
            this.lbRegDt.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.lbRegDt.Size = new System.Drawing.Size(72, 21);
            this.lbRegDt.TabIndex = 285;
            this.lbRegDt.Text = "µî·ÏÀÏ½Ã";
            this.lbRegDt.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ebRegDt
            // 
            this.ebRegDt.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ebRegDt.Location = new System.Drawing.Point(626, 154);
            this.ebRegDt.Name = "ebRegDt";
            this.ebRegDt.ReadOnly = true;
            this.ebRegDt.Size = new System.Drawing.Size(176, 21);
            this.ebRegDt.TabIndex = 0;
            this.ebRegDt.TabStop = false;
            this.ebRegDt.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
            this.ebRegDt.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            // 
            // ebContractName
            // 
            this.ebContractName.Location = new System.Drawing.Point(82, 58);
            this.ebContractName.MaxLength = 100;
            this.ebContractName.Name = "ebContractName";
            this.ebContractName.Size = new System.Drawing.Size(456, 21);
            this.ebContractName.TabIndex = 16;
            this.ebContractName.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
            this.ebContractName.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            // 
            // ebBonusRate
            // 
            this.ebBonusRate.DecimalDigits = 0;
            this.ebBonusRate.FormatString = "#,##0";
            this.ebBonusRate.Location = new System.Drawing.Point(82, 106);
            this.ebBonusRate.MaxLength = 4;
            this.ebBonusRate.Name = "ebBonusRate";
            this.ebBonusRate.Size = new System.Drawing.Size(62, 21);
            this.ebBonusRate.TabIndex = 21;
            this.ebBonusRate.Text = "0";
            this.ebBonusRate.TextAlignment = Janus.Windows.GridEX.TextAlignment.Far;
            this.ebBonusRate.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.ebBonusRate.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            this.ebBonusRate.TextChanged += new System.EventHandler(this.ebBonusRate_TextChanged);
            // 
            // label7
            // 
            this.label7.BackColor = System.Drawing.Color.Transparent;
            this.label7.Location = new System.Drawing.Point(144, 106);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(16, 21);
            this.label7.TabIndex = 306;
            this.label7.Text = "%";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label8
            // 
            this.label8.BackColor = System.Drawing.Color.Transparent;
            this.label8.Location = new System.Drawing.Point(216, 84);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(64, 21);
            this.label8.TabIndex = 305;
            this.label8.Text = "±¤°í´Ü°¡";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ebPrice
            // 
            this.ebPrice.DecimalDigits = 0;
            this.ebPrice.FormatString = "#,##0";
            this.ebPrice.Location = new System.Drawing.Point(288, 82);
            this.ebPrice.MaxLength = 17;
            this.ebPrice.Name = "ebPrice";
            this.ebPrice.Size = new System.Drawing.Size(104, 21);
            this.ebPrice.TabIndex = 20;
            this.ebPrice.Text = "0";
            this.ebPrice.TextAlignment = Janus.Windows.GridEX.TextAlignment.Far;
            this.ebPrice.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.ebPrice.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            // 
            // lbBonus
            // 
            this.lbBonus.BackColor = System.Drawing.Color.Transparent;
            this.lbBonus.Location = new System.Drawing.Point(8, 106);
            this.lbBonus.Name = "lbBonus";
            this.lbBonus.Size = new System.Drawing.Size(72, 21);
            this.lbBonus.TabIndex = 304;
            this.lbBonus.Text = "±âº»º¸³Ê½º";
            this.lbBonus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ebAdTime
            // 
            this.ebAdTime.DecimalDigits = 0;
            this.ebAdTime.FormatString = "#,##0";
            this.ebAdTime.Location = new System.Drawing.Point(82, 82);
            this.ebAdTime.MaxLength = 4;
            this.ebAdTime.Name = "ebAdTime";
            this.ebAdTime.Size = new System.Drawing.Size(56, 21);
            this.ebAdTime.TabIndex = 19;
            this.ebAdTime.Text = "15";
            this.ebAdTime.TextAlignment = Janus.Windows.GridEX.TextAlignment.Far;
            this.ebAdTime.Value = new decimal(new int[] {
            15,
            0,
            0,
            0});
            this.ebAdTime.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            // 
            // lbAdTime
            // 
            this.lbAdTime.BackColor = System.Drawing.Color.Transparent;
            this.lbAdTime.Location = new System.Drawing.Point(9, 84);
            this.lbAdTime.Name = "lbAdTime";
            this.lbAdTime.Size = new System.Drawing.Size(56, 21);
            this.lbAdTime.TabIndex = 301;
            this.lbAdTime.Text = "±¤°í±æÀÌ";
            this.lbAdTime.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label10
            // 
            this.label10.BackColor = System.Drawing.Color.Transparent;
            this.label10.Font = new System.Drawing.Font("±¼¸²Ã¼", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label10.Location = new System.Drawing.Point(139, 84);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(24, 21);
            this.label10.TabIndex = 300;
            this.label10.Text = "ÃÊ";
            this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ebSecurityTgt
            // 
            this.ebSecurityTgt.DecimalDigits = 0;
            this.ebSecurityTgt.FormatString = "#,##0";
            this.ebSecurityTgt.Location = new System.Drawing.Point(626, 130);
            this.ebSecurityTgt.MaxLength = 17;
            this.ebSecurityTgt.Name = "ebSecurityTgt";
            this.ebSecurityTgt.ReadOnly = true;
            this.ebSecurityTgt.Size = new System.Drawing.Size(176, 21);
            this.ebSecurityTgt.TabIndex = 0;
            this.ebSecurityTgt.TabStop = false;
            this.ebSecurityTgt.Text = "0";
            this.ebSecurityTgt.TextAlignment = Janus.Windows.GridEX.TextAlignment.Far;
            this.ebSecurityTgt.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.ebSecurityTgt.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            // 
            // ebContEndDay
            // 
            // 
            // 
            // 
            this.ebContEndDay.DropDownCalendar.FirstMonth = new System.DateTime(2007, 8, 1, 0, 0, 0, 0);
            this.ebContEndDay.DropDownCalendar.Name = "";
            this.ebContEndDay.DropDownCalendar.VisualStyle = Janus.Windows.CalendarCombo.VisualStyle.Office2007;
            this.ebContEndDay.Location = new System.Drawing.Point(360, 156);
            this.ebContEndDay.Name = "ebContEndDay";
            this.ebContEndDay.Size = new System.Drawing.Size(112, 21);
            this.ebContEndDay.TabIndex = 26;
            this.ebContEndDay.Value = new System.DateTime(2015, 11, 30, 0, 0, 0, 0);
            this.ebContEndDay.VisualStyle = Janus.Windows.CalendarCombo.VisualStyle.Office2007;
            // 
            // ebContStartDay
            // 
            // 
            // 
            // 
            this.ebContStartDay.DropDownCalendar.FirstMonth = new System.DateTime(2007, 8, 1, 0, 0, 0, 0);
            this.ebContStartDay.DropDownCalendar.Name = "";
            this.ebContStartDay.DropDownCalendar.VisualStyle = Janus.Windows.CalendarCombo.VisualStyle.Office2007;
            this.ebContStartDay.Location = new System.Drawing.Point(82, 156);
            this.ebContStartDay.Name = "ebContStartDay";
            this.ebContStartDay.Size = new System.Drawing.Size(112, 21);
            this.ebContStartDay.TabIndex = 25;
            this.ebContStartDay.Value = new System.DateTime(2015, 11, 30, 0, 0, 0, 0);
            this.ebContStartDay.VisualStyle = Janus.Windows.CalendarCombo.VisualStyle.Office2007;
            // 
            // btnAdvertise
            // 
            this.btnAdvertise.BackColor = System.Drawing.SystemColors.Window;
            this.btnAdvertise.Enabled = false;
            this.btnAdvertise.Location = new System.Drawing.Point(82, 33);
            this.btnAdvertise.Name = "btnAdvertise";
            this.btnAdvertise.Size = new System.Drawing.Size(104, 21);
            this.btnAdvertise.TabIndex = 0;
            this.btnAdvertise.TabStop = false;
            this.btnAdvertise.Text = "±¤°íÁÖ¼±ÅÃ";
            this.btnAdvertise.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.btnAdvertise.Click += new System.EventHandler(this.btnAdvertise_Click);
            // 
            // ebAgency
            // 
            this.ebAgency.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ebAgency.Location = new System.Drawing.Point(362, 32);
            this.ebAgency.MaxLength = 100;
            this.ebAgency.Name = "ebAgency";
            this.ebAgency.Size = new System.Drawing.Size(176, 21);
            this.ebAgency.TabIndex = 14;
            this.ebAgency.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
            this.ebAgency.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            // 
            // uiTabPage2
            // 
            this.uiTabPage2.Controls.Add(this.panel2);
            this.uiTabPage2.Location = new System.Drawing.Point(1, 22);
            this.uiTabPage2.Name = "uiTabPage2";
            this.uiTabPage2.Size = new System.Drawing.Size(979, 263);
            this.uiTabPage2.TabStop = true;
            this.uiTabPage2.Text = "±¤°í³»¿ª";
            // 
            // panel2
            // 
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.grdExItemList);
            this.panel2.Location = new System.Drawing.Point(8, 8);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(968, 240);
            this.panel2.TabIndex = 0;
            // 
            // grdExItemList
            // 
            this.grdExItemList.AllowEdit = Janus.Windows.GridEX.InheritableBoolean.False;
            this.grdExItemList.AlternatingColors = true;
            this.grdExItemList.BorderStyle = Janus.Windows.GridEX.BorderStyle.None;
            this.grdExItemList.DataSource = this.dvItem;
            grdExItemList_DesignTimeLayout.LayoutString = resources.GetString("grdExItemList_DesignTimeLayout.LayoutString");
            this.grdExItemList.DesignTimeLayout = grdExItemList_DesignTimeLayout;
            this.grdExItemList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdExItemList.EditorsControlStyle.ButtonAppearance = Janus.Windows.GridEX.ButtonAppearance.Regular;
            this.grdExItemList.EmptyRows = true;
            this.grdExItemList.FocusCellFormatStyle.BackColor = System.Drawing.SystemColors.Highlight;
            this.grdExItemList.FocusCellFormatStyle.ForeColor = System.Drawing.Color.White;
            this.grdExItemList.FocusStyle = Janus.Windows.GridEX.FocusStyle.None;
            this.grdExItemList.GridLineColor = System.Drawing.Color.Silver;
            this.grdExItemList.GridLines = Janus.Windows.GridEX.GridLines.Vertical;
            this.grdExItemList.GridLineStyle = Janus.Windows.GridEX.GridLineStyle.Solid;
            this.grdExItemList.GroupByBoxVisible = false;
            this.grdExItemList.HideSelection = Janus.Windows.GridEX.HideSelection.HighlightInactive;
            this.grdExItemList.Location = new System.Drawing.Point(0, 0);
            this.grdExItemList.Name = "grdExItemList";
            this.grdExItemList.SelectedInactiveFormatStyle.BackColor = System.Drawing.Color.Gold;
            this.grdExItemList.Size = new System.Drawing.Size(966, 238);
            this.grdExItemList.TabIndex = 15;
            this.grdExItemList.TabKeyBehavior = Janus.Windows.GridEX.TabKeyBehavior.ControlNavigation;
            this.grdExItemList.ThemedAreas = ((Janus.Windows.GridEX.ThemedArea)(((((((((Janus.Windows.GridEX.ThemedArea.ScrollBars | Janus.Windows.GridEX.ThemedArea.EditControls) 
            | Janus.Windows.GridEX.ThemedArea.Headers) 
            | Janus.Windows.GridEX.ThemedArea.GroupByBox) 
            | Janus.Windows.GridEX.ThemedArea.GroupRows) 
            | Janus.Windows.GridEX.ThemedArea.ControlBorder) 
            | Janus.Windows.GridEX.ThemedArea.Cards) 
            | Janus.Windows.GridEX.ThemedArea.Gridlines) 
            | Janus.Windows.GridEX.ThemedArea.CheckBoxes)));
            this.grdExItemList.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            // 
            // dvItem
            // 
            this.dvItem.Table = this.contractDs.ContractItem;
            // 
            // panel1
            // 
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(200, 100);
            this.panel1.TabIndex = 0;
            // 
            // gridEX1
            // 
            this.gridEX1.Location = new System.Drawing.Point(0, 0);
            this.gridEX1.Name = "gridEX1";
            this.gridEX1.Size = new System.Drawing.Size(400, 376);
            this.gridEX1.TabIndex = 0;
            // 
            // ContractControl
            // 
            this.Controls.Add(this.uiPanelContract);
            this.Font = global::AdManagerClient.Properties.Settings.Default.Font1;
            this.Name = "ContractControl";
            this.Size = new System.Drawing.Size(1010, 677);
            this.Load += new System.EventHandler(this.UserControl_Load);
            ((System.ComponentModel.ISupportInitialize)(this.uiPM)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelContract)).EndInit();
            this.uiPanelContract.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelSearch)).EndInit();
            this.uiPanelSearch.ResumeLayout(false);
            this.uiPanelSearchContainer.ResumeLayout(false);
            this.pnlSearch.ResumeLayout(false);
            this.pnlSearch.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelList)).EndInit();
            this.uiPanelList.ResumeLayout(false);
            this.uiPanelListContainer.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdExContractList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dvContract)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.contractDs)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelDetail)).EndInit();
            this.uiPanelDetail.ResumeLayout(false);
            this.uiPanelDetailContainer.ResumeLayout(false);
            this.pnlUserDetail.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.uiTab1)).EndInit();
            this.uiTab1.ResumeLayout(false);
            this.uiTabPage1.ResumeLayout(false);
            this.uiTabPage1.PerformLayout();
            this.uiTabPage2.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdExItemList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dvItem)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridEX1)).EndInit();
            this.ResumeLayout(false);

        }
        #endregion

        #region ÄÁÆ®·Ñ ·Îµå
        private void UserControl_Load(object sender, System.EventArgs e)
        {
            // µ¥ÀÌÅÍ°ü¸®¿ë °´Ã¼»ý¼º
            dt = ((DataView)grdExContractList.DataSource).Table;  
            cm = (CurrencyManager) this.BindingContext[grdExContractList.DataSource]; 
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

            // Á¶È¸±ÇÇÑ °Ë»ç
            if(menu.CanRead(MenuCode))
            {
                canRead = true;
                SearchContract();
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
                //ResetTextReadOnly();
                canUpdate = true;
            }
            else
            {
                SetTextReadOnly();
            }

            InitButton();
            SetTextReadOnly();
			ProgressStop();
        }

        private void InitCombo()
        {
            Init_MediaCode();
            Init_RapCode();
            Init_AgencyCode();
            Init_AdvertiserCode();
            Init_ContractState();
            InitCombo_Level();
			cbSearchContractState.SelectedValue = "10";
        }

        private void Init_MediaCode()
        {

            // ¸ÅÃ¼¸¦ Á¶È¸ÇÑ´Ù.
            MediaCodeModel mediaCodeModel = new MediaCodeModel();
            new MediaCodeManager(systemModel, commonModel).GetMediaCodeList(mediaCodeModel);
			
            if (mediaCodeModel.ResultCD.Equals("0000"))
            {
                // µ¥ÀÌÅÍ¼Â¿¡ ¼ÂÆÃ
                Utility.SetDataTable(contractDs.Medias, mediaCodeModel.MediaCodeDataSet);				
            }

            // °Ë»öÁ¶°ÇÀÇ ÄÞº¸
            this.cbSearchMedia.Items.Clear();
			
            // ÄÞº¸¹Ú½º¿¡ ¼ÂÆ®ÇÒ ÄÚµå¸ñ·ÏÀ» ´ãÀ» Item¹è¿­À» ¼±¾ð
            Janus.Windows.EditControls.UIComboBoxItem[] comboItems = new Janus.Windows.EditControls.UIComboBoxItem[mediaCodeModel.ResultCnt + 1];

            comboItems[0] = new Janus.Windows.EditControls.UIComboBoxItem("¸ÅÃ¼¼±ÅÃ","00");
			
            for(int i=0;i<mediaCodeModel.ResultCnt;i++)
            {
                DataRow row = contractDs.Medias.Rows[i];

                string val = row["MediaCode"].ToString();
                string txt = row["MediaName"].ToString();
                comboItems[i+1] = new Janus.Windows.EditControls.UIComboBoxItem(txt,val);
            }
            // °Ë»ö ÄÞº¸¿¡ ¼ÂÆ®
            this.cbSearchMedia.Items.AddRange(comboItems);
            this.cbSearchMedia.SelectedIndex = 0;
			
            Application.DoEvents();

        }

        private void InitCombo_Level() 
        {			
            //¸ÅÃ¼·¹º§ÀÏ°æ¿ì º¸¾Èµî±Þ Ã³¸®
            if(commonModel.UserLevel == "20")
            {
                // ÄÞº¸ÇÈ½º						
                cbSearchMedia.SelectedValue = commonModel.MediaCode;		
                cbSearchMedia.ReadOnly = true;
            }  
            else if(commonModel.UserLevel != "20")
            {
				for(int i=0;i < contractDs.Medias.Rows.Count;i++)
				{
					DataRow row = contractDs.Medias.Rows[i];					
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
            //¹Ìµð¾î·¾ ·¹º§ÀÏ°æ¿ìÀÇ º¸¾Èµî±ÞÃ³¸®
            if (commonModel.UserLevel == "30")
            {
                //Á¶È¸ ÄÞº¸ ÇÈ½º
                cbSearchRap.SelectedValue = commonModel.RapCode;
                cbSearchRap.ReadOnly = true;                
            }
            //´ëÇà»ç/±¤°íÁÖ ·¹º§ÀÏ °æ¿ìÀÇ º¸¾Èµî±ÞÃ³¸®
            if (commonModel.UserLevel == "40")
            {
                //Á¶È¸ ÄÞº¸ÇÈ½º						
                cbSearchAgency.SelectedValue = commonModel.AgencyCode;			
                cbSearchAgency.ReadOnly = true;
            }


            Application.DoEvents();
        }   
    

    

        private void Init_RapCode()
        {
            /* ÀÌºÎºÐÀ» ¸ð¹ÙÀÏ Æí¼ºÆÀÀ¸·Î °íÁ¤À¸·Î Ã³¸®
            // ·¦À» Á¶È¸ÇÑ´Ù.
            MediaRapCodeModel mediaRapCodeModel = new MediaRapCodeModel();
            new MediaRapCodeManager(systemModel, commonModel).GetMediaRapCodeList(mediaRapCodeModel);
			
            if (mediaRapCodeModel.ResultCD.Equals("0000"))
            {
                // µ¥ÀÌÅÍ¼Â¿¡ ¼ÂÆÃ
                Utility.SetDataTable(contractDs.MediaRaps, mediaRapCodeModel.MediaRapCodeDataSet);				
            }

            // °Ë»öÁ¶°ÇÀÇ ÄÞº¸
            this.cbSearchRap.Items.Clear();
            // ÄÞº¸¹Ú½º¿¡ ¼ÂÆ®ÇÒ ÄÚµå¸ñ·ÏÀ» ´ãÀ» Item¹è¿­À» ¼±¾ð
            Janus.Windows.EditControls.UIComboBoxItem[] comboItems = new Janus.Windows.EditControls.UIComboBoxItem[mediaRapCodeModel.ResultCnt + 1];

            comboItems[0] = new Janus.Windows.EditControls.UIComboBoxItem("¹Ìµð¾î·¾¼±ÅÃ","00");
			
            for(int i=0;i<mediaRapCodeModel.ResultCnt;i++)
            {
                DataRow row = contractDs.MediaRaps.Rows[i];

                string val = row["RapCode"].ToString();
                string txt = row["RapName"].ToString();
                comboItems[i+1] = new Janus.Windows.EditControls.UIComboBoxItem(txt,val);
            }
            // ÄÞº¸¿¡ ¼ÂÆ®
            this.cbSearchRap.Items.AddRange(comboItems);
            this.cbSearchRap.SelectedIndex = 0;                        
            */
            
            DataSet ds = new DataSet("rapset");
            DataTable dt = new DataTable("rap");
            dt.Columns.Add("RapCode", typeof(string));
            dt.Columns.Add("RapName", typeof(string));
            DataRow nRow = dt.NewRow();
            nRow["RapCode"] = "00";
            nRow["RapName"] = "ÀüÃ¼";
            dt.Rows.Add(nRow);
            ds.Tables.Add(dt);
            
            Utility.SetDataTable(contractDs.MediaRaps, ds);
            // °Ë»öÁ¶°ÇÀÇ ÄÞº¸
            this.cbSearchRap.Items.Clear();
            // ÄÞº¸¹Ú½º¿¡ ¼ÂÆ®ÇÒ ÄÚµå¸ñ·ÏÀ» ´ãÀ» Item¹è¿­À» ¼±¾ð
            Janus.Windows.EditControls.UIComboBoxItem[] comboItems = new Janus.Windows.EditControls.UIComboBoxItem[2];

            comboItems[0] = new Janus.Windows.EditControls.UIComboBoxItem("¹Ìµð¾î·¾¼±ÅÃ", "00");

            for (int i = 0; i < 1; i++)
            {
                DataRow row = contractDs.MediaRaps.Rows[i];

                string val = row["RapCode"].ToString();
                string txt = row["RapName"].ToString();
                comboItems[i + 1] = new Janus.Windows.EditControls.UIComboBoxItem(txt, val);
            }
            // ÄÞº¸¿¡ ¼ÂÆ®
            this.cbSearchRap.Items.AddRange(comboItems);
            this.cbSearchRap.SelectedIndex = 1;
            this.cbSearchRap.ReadOnly = true;
            Application.DoEvents();
        }

        private void Init_AgencyCode()
        {
            // ´ëÇà»ç¸¦ Á¶È¸ÇÑ´Ù.
            AgencyCodeModel agencyCodeModel = new AgencyCodeModel();
            new AgencyCodeManager(systemModel, commonModel).GetAgencyCodeList(agencyCodeModel);
			
            if (agencyCodeModel.ResultCD.Equals("0000"))
            {
                // µ¥ÀÌÅÍ¼Â¿¡ ¼ÂÆÃ
                Utility.SetDataTable(contractDs.Agencys, agencyCodeModel.AgencyCodeDataSet);				
            }

            // °Ë»öÁ¶°ÇÀÇ ÄÞº¸
            this.cbSearchAgency.Items.Clear();

            // ÄÞº¸¹Ú½º¿¡ ¼ÂÆ®ÇÒ ÄÚµå¸ñ·ÏÀ» ´ãÀ» Item¹è¿­À» ¼±¾ð
            Janus.Windows.EditControls.UIComboBoxItem[] comboItems = new Janus.Windows.EditControls.UIComboBoxItem[agencyCodeModel.ResultCnt + 1];

            comboItems[0] = new Janus.Windows.EditControls.UIComboBoxItem("´ëÇà»ç¼±ÅÃ","00");
			
            for(int i=0;i<agencyCodeModel.ResultCnt;i++)
            {
                DataRow row = contractDs.Agencys.Rows[i];

                string val = row["AgencyCode"].ToString();
                string txt = row["AgencyName"].ToString();
                comboItems[i+1] = new Janus.Windows.EditControls.UIComboBoxItem(txt,val);
            }
			
            // ÄÞº¸¿¡ ¼ÂÆ®            
            this.cbSearchAgency.Items.AddRange(comboItems);
            this.cbSearchAgency.SelectedIndex = 0;

            Application.DoEvents();
        }

        private void Init_AdvertiserCode()
        {
            // ±¤°íÁÖ¸¦ Á¶È¸ÇÑ´Ù.
            ClientModel clientModel = new ClientModel();
            new ClientManager(systemModel, commonModel).GetAdvertiserList(clientModel);
			
            if (clientModel.ResultCD.Equals("0000"))
            {
                // µ¥ÀÌÅÍ¼Â¿¡ ¼ÂÆÃ
                Utility.SetDataTable(contractDs.Advertisers, clientModel.ClientDataSet);				
            }

            // °Ë»öÁ¶°ÇÀÇ ÄÞº¸
            this.cbSearchAdvertiser.Items.Clear();


            // ÄÞº¸¹Ú½º¿¡ ¼ÂÆ®ÇÒ ÄÚµå¸ñ·ÏÀ» ´ãÀ» Item¹è¿­À» ¼±¾ð
            Janus.Windows.EditControls.UIComboBoxItem[] comboItems = new Janus.Windows.EditControls.UIComboBoxItem[clientModel.ResultCnt + 1];

            comboItems[0] = new Janus.Windows.EditControls.UIComboBoxItem("±¤°íÁÖ¼±ÅÃ","00");
			
            for(int i=0;i<clientModel.ResultCnt;i++)
            {
                DataRow row = contractDs.Advertisers.Rows[i];

                string val = row["AdvertiserCode"].ToString();
                string txt = row["AdvertiserName"].ToString();
                comboItems[i+1] = new Janus.Windows.EditControls.UIComboBoxItem(txt,val);
            }
			
            // ÄÞº¸¿¡ ¼ÂÆ®
            this.cbSearchAdvertiser.Items.AddRange(comboItems);
            this.cbSearchAdvertiser.SelectedIndex = 0;

        }

        private void Init_ContractState()
        {
            // ÄÚµå¿¡¼­ °è¾à»óÅÂ¸¦ Á¶È¸ÇÑ´Ù.
            CodeModel codeModel = new CodeModel();
            codeModel.Section = "23";				// ÄÚµåºÐ·ù '23':°è¾à»óÅÂ  TODO: ÄÚµåºÐ·ù´Â ÃßÈÄ XML·Î °ü¸®µÇ¾î¾ß...
            new CodeManager(systemModel, commonModel).GetCodeList(codeModel);
			
            if (codeModel.ResultCD.Equals("0000"))
            {
                // µ¥ÀÌÅÍ¼Â¿¡ ¼ÂÆÃ
                Utility.SetDataTable(contractDs.ContractState, codeModel.CodeDataSet);				
            }

            // °Ë»öÁ¶°ÇÀÇ ÄÞº¸
            this.cbSearchContractState.Items.Clear();
            // ÀÔ·Â ÄÞº¸
            this.cbContractState.Items.Clear();
			
            // ÄÞº¸¹Ú½º¿¡ ¼ÂÆ®ÇÒ ÄÚµå¸ñ·ÏÀ» ´ãÀ» Item¹è¿­À» ¼±¾ð
            Janus.Windows.EditControls.UIComboBoxItem[] comboItems = new Janus.Windows.EditControls.UIComboBoxItem[codeModel.ResultCnt + 1];

            comboItems[0] = new Janus.Windows.EditControls.UIComboBoxItem("°è¾à»óÅÂ¼±ÅÃ","00");
			
            for(int i=0;i<codeModel.ResultCnt;i++)
            {
                DataRow row = contractDs.ContractState.Rows[i];

                string val = row["Code"].ToString();
                string txt = row["CodeName"].ToString();
                comboItems[i+1] = new Janus.Windows.EditControls.UIComboBoxItem(txt,val);
            }
            // ÄÞº¸¿¡ ¼ÂÆ®
            this.cbSearchContractState.Items.AddRange(comboItems);
            this.cbSearchContractState.SelectedIndex = 0;

            // ÀÔ·ÂÄÞº¸¿¡ ¼ÂÆ®ÇÒ ÄÚµå¸ñ·ÏÀ» ´ãÀ» Item¹è¿­À» ¼±¾ð
            comboItems = new Janus.Windows.EditControls.UIComboBoxItem[codeModel.ResultCnt];
            
            for(int i=0;i<codeModel.ResultCnt;i++)
            {
                DataRow row = contractDs.ContractState.Rows[i];

                string val = row["Code"].ToString();
                string txt = row["CodeName"].ToString();
                comboItems[i] = new Janus.Windows.EditControls.UIComboBoxItem(txt,val);
            }
            // ÀÔ·ÂÄÞº¸¿¡ ¼ÂÆ®
            this.cbContractState.Items.AddRange(comboItems);
            this.cbContractState.SelectedIndex = 0;
			
            Application.DoEvents();
        }

        private void InitButton()
        {
            if(canRead)   btnSearch.Enabled = true;
            if(canCreate) btnAdd.Enabled    = true;
            if(canDelete) btnDelete.Enabled    = true;
            if(ebContractName.Text.Trim().Length > 0) 
            {
                if(canDelete) btnDelete.Enabled = true;
                if(canUpdate) btnSave.Enabled   = true;
            }
            if(IsAdding)
            {
                if(canCreate) btnAdvertise.Enabled = true;
            }
            else
            {
                btnAdvertise.Enabled = false;
            }


            Application.DoEvents();
        }

        private void DisableButton()
        {
            btnSearch.Enabled = false;
            btnAdd.Enabled    = false;
            btnSave.Enabled   = false;
            btnDelete.Enabled = false;
            btnAdvertise.Enabled = false;
            Application.DoEvents();
        }

        #endregion

        #region °è¾àÁ¤º¸ ¾×¼ÇÃ³¸® ¸Þ¼Òµå

        /// <summary>
        /// ±×¸®µåÀÇ Rowº¯°æ½Ã
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnGrdRowChanged(object sender, System.EventArgs e) 
        {
            if (!IsSearching) // 2011.11.29 JH.Park Á¶È¸ÁßÀÌ ¾Æ´Ò°æ¿ì¿¡¸¸ µ¿ÀÛÇÏµµ·Ï º¯°æ
            {
                if (grdExContractList.RowCount > 0)
                {
                    SetDetailText();
                    InitButton();
                }
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
            SearchContract();
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
            if(canCreate)btnSave.Enabled   = true;
            if(canCreate)btnAdvertise.Enabled = true;
            
            IsAdding = true;
            ResetTextReadOnly();
            ResetDetailText();
            ebContractName.Focus();	// Ãß°¡½Ã ÃÖÃÊ À§Ä¡
        }

        /// <summary>
        /// ÀúÀå¹öÆ° Å¬¸¯
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_Click(object sender, System.EventArgs e)
        {
            SaveContractDetail();
        }

        /// <summary>
        /// »èÁ¦¹öÆ° Å¬¸¯
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDelete_Click(object sender, System.EventArgs e)
        {
            DeleteContract();
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
                SearchContract();
            }
        }

        #endregion

        #region Ã³¸®¸Þ¼Òµå

        /// <summary>
        /// ±¤°í°è¾à¸ñ·Ï Á¶È¸
        /// </summary>
        private void SearchContract()
        {
            IsSearching = true;

            StatusMessage("±¤°í°è¾à Á¤º¸¸¦ Á¶È¸ÇÕ´Ï´Ù.");

            try
            {				
                //ÀúÀå Àü¿¡ ¸ðµ¨À» ÃÊ±âÈ­ ÇØÁØ´Ù.
                contractModel.Init();				
                // µ¥ÀÌÅÍ¸ðµ¨¿¡ Àü¼ÛÇÒ ³»¿ëÀ» ¼ÂÆ®ÇÑ´Ù.
                contractModel.MediaCode      = cbSearchMedia.SelectedValue.ToString();
                contractModel.RapCode        = cbSearchRap.SelectedValue.ToString();
                contractModel.AgencyCode     = cbSearchAgency.SelectedValue.ToString();
                contractModel.AdvertiserCode = cbSearchAdvertiser.SelectedValue.ToString();
                contractModel.State          = cbSearchContractState.SelectedValue.ToString();
				
                if(IsNewSearchKey)
                {
                    contractModel.SearchKey = "";					
                }
                else
                {
                    contractModel.SearchKey  = ebSearchKey.Text;
                }
                
                ResetDetailText();
                // ±¤°í °è¾à ¸ñ·Ï ¼­ºñ½º¸¦ È£ÃâÇÑ´Ù.
                new ContractManager(systemModel,commonModel).GetContractList(contractModel);				

                if (contractModel.ResultCD.Equals("0000"))
                {											
                    Utility.SetDataTable(contractDs.Contract, contractModel.ContractDataSet);			
                    StatusMessage(contractModel.ResultCnt + "°ÇÀÇ °è¾àÁ¤º¸ Á¤º¸°¡ Á¶È¸µÇ¾ú½À´Ï´Ù.");                  
                    if(canUpdate)
                    {						
                        AddSchChoice();									
                    }				
                    SetDetailText();
                }
            }
            catch(FrameException fe)
            {
                FrameSystem.showMsgForm("°è¾àÁ¤º¸Á¶È¸¿À·ù", new string[] {fe.ErrCode, fe.ResultMsg});
            }
            catch(Exception ex)
            {
                FrameSystem.showMsgForm("°è¾àÁ¤º¸Á¶È¸¿À·ù",new string[] {"",ex.Message});
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
                if ( contractDs.Tables["Contract"].Rows.Count < 1 ) return;
              
                foreach (DataRow row in contractDs.Tables["Contract"].Rows)
                {					
                    if(IsAdding)
                    {
						
                        cm.Position = 0;
                        Contract_Seq = null;
                    }
                    else
                    {						
                        if(row["ContractSeq"].ToString().Equals(Contract_Seq))
                        {					
                            cm.Position = rowIndex;
                            break;								
                        }
                    }

                    rowIndex++;
                    grdExContractList.EnsureVisible();
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

		private void SearchContractItem()
		{
			StatusMessage("±¤°í³»¿ª Á¤º¸¸¦ Á¶È¸ÇÕ´Ï´Ù.");

			try
			{
				//°Ë»ö Àü¿¡ ¸ðµ¨À» ÃÊ±âÈ­ ÇØÁØ´Ù.
				contractModel.Init();
				// µ¥ÀÌÅÍ¸ðµ¨¿¡ Àü¼ÛÇÒ ³»¿ëÀ» ¼ÂÆ®ÇÑ´Ù.
				contractModel.MediaCode      = cbSearchMedia.SelectedValue.ToString();
				contractModel.RapCode        = cbSearchRap.SelectedValue.ToString();
				contractModel.AgencyCode     = cbSearchAgency.SelectedValue.ToString();
				contractModel.AdvertiserCode = cbSearchAdvertiser.SelectedValue.ToString();
				//contractModel.AdType		 = cbSearchAdType.SelectedValue.ToString();
				contractModel.ContractSeq   = dt.Rows[cm.Position]["ContractSeq"].ToString();
				

				// ±¤°í ³»¿ª ¸ñ·Ï ¼­ºñ½º¸¦ È£ÃâÇÑ´Ù.
				new ContractManager(systemModel,commonModel).GetContractItemList(contractModel);

				if (contractModel.ResultCD.Equals("0000"))
				{
					Utility.SetDataTable(contractDs.ContractItem, contractModel.ContractDataSet);				
					StatusMessage(contractModel.ResultCnt + "°ÇÀÇ ³»¿ªÁ¤º¸ Á¤º¸°¡ Á¶È¸µÇ¾ú½À´Ï´Ù.");					
				}
			}
			catch(FrameException fe)
			{
				FrameSystem.showMsgForm("³»¿ªÁ¤º¸Á¶È¸¿À·ù", new string[] {fe.ErrCode, fe.ResultMsg});
			}
			catch(Exception ex)
			{
				FrameSystem.showMsgForm("³»¿ªÁ¤º¸Á¶È¸¿À·ù",new string[] {"",ex.Message});
			}
		}

        /// <summary>
        /// °è¾àÁ¤º¸»ó¼¼Á¤º¸ ÀúÀå
        /// </summary>
        private void SaveContractDetail()
        {
		
            StatusMessage("°è¾àÁ¤º¸ Á¤º¸¸¦ ÀúÀåÇÕ´Ï´Ù.");
                        
            if(AdvertiserCode.Length == 0) 
            {
				MessageBox.Show("±¤°íÁÖ°¡ ¼±ÅÃµÇÁö ¾Ê¾Ò½À´Ï´Ù.","°è¾àÁ¤º¸ ÀúÀå", 
					MessageBoxButtons.OK, MessageBoxIcon.Information );
				return;				               
            }
            if(ebContractName.Text.Trim().Length == 0) 
            {
				MessageBox.Show("±¤°í °è¾à¸íÀÌ ÀÔ·ÂµÇÁö ¾Ê¾Ò½À´Ï´Ù.","°è¾àÁ¤º¸ ÀúÀå", 
					MessageBoxButtons.OK, MessageBoxIcon.Information );
				ebContractName.Focus();
				return;	                
            }
                      
            try
            {
                //ÀúÀå Àü¿¡ ¸ðµ¨À» ÃÊ±âÈ­ ÇØÁØ´Ù.
                contractModel.Init();
				EbPrice = Convert.ToDecimal(ebPrice.Text);
                // µ¥ÀÌÅÍ¸ðµ¨¿¡ Àü¼ÛÇÒ ³»¿ëÀ» ¼ÂÆ®ÇÑ´Ù.
                contractModel.MediaCode      = MediaCode;
                contractModel.RapCode        = RapCode;
                contractModel.AgencyCode     = AgencyCode;
                contractModel.AdvertiserCode = AdvertiserCode;
                contractModel.State          = cbContractState.SelectedValue.ToString();
                contractModel.AdTime         = ebAdTime.Text; // adtime; //»óÇ°°Ë»ö ¿¬µ¿ÇÏÁö ¾ÊÀ½
				contractModel.BonusRate		 = ebBonusRate.Text;
				contractModel.LongBonus		 = ebLongBonus.Text;
				contractModel.SpecialBonus	 = ebSpecialBonus.Text;
				contractModel.TotalBonus	 = ebTotalBonus.Text;				
				contractModel.SecurityTgt	 = ebSecurityTgt.Text;
				// contractModel.PackageName	 = ebPackageName.Text; // »ç¿ë ¾ÈÇÔ
				// contractModel.JobClass		 = jobCode; // »ç¿ë ¾ÈÇÔ
				contractModel.Price			 = EbPrice.ToString();
                contractModel.ContractName   = ebContractName.Text;
                //³¯Â¥¸¦ 8ÀÚ¸® ¹Ù²Ù¾îÁØ´Ù.

				contractModel.ContStartDay   = ebContStartDay.Value.ToString("yyyyMMdd");
				contractModel.ContEndDay     = ebContEndDay.Value.ToString("yyyyMMdd");
                contractModel.ContractAmt    = ebContractAmt.Text;
                contractModel.Comment		 = ebComment.Text;
				if(ebContStartDay.Value > ebContEndDay.Value)
				{
					MessageBox.Show("Á¾·áÀÏÀÌ ½ÃÀÛÀÏº¸´Ù ÀÛ½À´Ï´Ù.","°è¾àÀÏ ¿À·ù", 
						MessageBoxButtons.OK, MessageBoxIcon.Information );
						ebContEndDay.Focus();
					return;		
					
				}

                // °è¾àÁ¤º¸ »ó¼¼Á¤º¸ ÀúÀå ¼­ºñ½º¸¦ È£ÃâÇÑ´Ù.
                if (IsAdding)
                {
                    new ContractManager(systemModel,commonModel).SetContractAdd(contractModel);
                
                    IsAdding = false;					
                    ResetDetailText();
                    StatusMessage("°è¾àÁ¤º¸ Á¤º¸°¡ Ãß°¡µÇ¾ú½À´Ï´Ù.");
                
                }
                else
                {   
                    //ÀúÀåÀÌ ¾Æ´Ï°í ¾÷µ¥ÀÌÆ®ÀÏ°æ¿ì¿¡´Â °è¾à¼ø¹øÄÚµå¸¦ ¼³Á¤
                    contractModel.ContractSeq   = dt.Rows[cm.Position]["ContractSeq"].ToString();
                    Contract_Seq = dt.Rows[cm.Position]["ContractSeq"].ToString();
                    new ContractManager(systemModel,commonModel).SetContractUpdate(contractModel);
                    StatusMessage("°è¾àÁ¤º¸ Á¤º¸°¡ ÀúÀåµÇ¾ú½À´Ï´Ù.");
                
                }

                DisableButton();
                SearchContract();					
                InitButton();
        
            }
            catch(FrameException fe)
            {
                FrameSystem.showMsgForm("°è¾àÁ¤º¸Á¤º¸ ÀúÀå¿À·ù", new string[] {fe.ErrCode, fe.ResultMsg});
            }
            catch(Exception ex)
            {
                FrameSystem.showMsgForm("°è¾àÁ¤º¸Á¤º¸ ÀúÀå¿À·ù",new string[] {"",ex.Message});
            }		
	
        }


        /// <summary>
        /// °è¾àÁ¤º¸ »èÁ¦
        /// </summary>
        private void DeleteContract()
        {
            StatusMessage("°è¾àÁ¤º¸ Á¤º¸¸¦ »èÁ¦ÇÕ´Ï´Ù.");
            
            if(keyContractSeq.Trim().Length == 0) 
            {
                FrameSystem.showMsgForm("±¤°í°è¾àÁ¤º¸ »èÁ¦¿À·ù",new string[] {"", "»èÁ¦ÇÒ ±¤°í°è¾à Á¤º¸°¡ ¾ø½À´Ï´Ù.", "" });
                return;
            }

                

            DialogResult result = MessageBox.Show("ÇØ´ç ±¤°í°è¾à Á¤º¸¸¦ »èÁ¦ ÇÏ½Ã°Ú½À´Ï±î?","±¤°í°è¾à »èÁ¦",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question,
                MessageBoxDefaultButton.Button2);

            if (result == DialogResult.No) return;

            try 
            {
                // µ¥ÀÌÅÍ¸ðµ¨¿¡ Àü¼ÛÇÒ ³»¿ëÀ» ¼ÂÆ®ÇÑ´Ù.
                contractModel.ContractSeq = keyContractSeq;

                // ¸ÅÃ¼´ëÇà±¤°íÁÖ »ó¼¼Á¤º¸ ÀúÀå ¼­ºñ½º¸¦ È£ÃâÇÑ´Ù.
                new ContractManager(systemModel,commonModel).SetContractDelete(contractModel);

                StatusMessage("±¤°í°è¾à Á¤º¸°¡ »èÁ¦µÇ¾ú½À´Ï´Ù.");			
                
                ResetDetailText();
                DisableButton();
                SearchContract();
                InitButton();


            }
            catch(FrameException fe)
            {
                FrameSystem.showMsgForm("±¤°í°è¾àÁ¤º¸ »èÁ¦¿À·ù", new string[] {fe.ErrCode, fe.ResultMsg});
            }
            catch(Exception ex)
            {
                FrameSystem.showMsgForm("±¤°í°è¾àÁ¤º¸ »èÁ¦¿À·ù",new string[] {"",ex.Message});
            }	


        }
		
        /// <summary>
        /// °è¾àÁ¤º¸ »ó¼¼Á¤º¸ÀÇ ¼ÂÆ®
        /// </summary>
        private void SetDetailText()
        {
            int curRow = cm.Position;

            if(curRow >= 0)
            {
                keyContractSeq = dt.Rows[cm.Position]["ContractSeq"].ToString();

                MediaCode            = dt.Rows[cm.Position]["MediaCode"].ToString();
                RapCode              = dt.Rows[cm.Position]["RapCode"].ToString();
                AgencyCode           = dt.Rows[cm.Position]["AgencyCode"].ToString();
                AdvertiserCode       = dt.Rows[cm.Position]["AdvertiserCode"].ToString();

                ebMedia.Text         = dt.Rows[cm.Position]["MediaName"].ToString();
                ebRap.Text           = dt.Rows[cm.Position]["RapName"].ToString();
                ebAgency.Text        = dt.Rows[cm.Position]["AgencyName"].ToString();
                ebAdvertiser.Text    = dt.Rows[cm.Position]["AdvertiserName"].ToString();
                cbContractState.SelectedValue = dt.Rows[cm.Position]["State"].ToString();
				ebAdTime.Text		 = dt.Rows[cm.Position]["AdTime"].ToString();
				ebBonusRate.Text	 = dt.Rows[cm.Position]["BonusRate"].ToString();
				ebLongBonus.Text	 = dt.Rows[cm.Position]["LongBonus"].ToString();
				ebSpecialBonus.Text	 = dt.Rows[cm.Position]["SpecialBonus"].ToString();
				ebTotalBonus.Text	 = dt.Rows[cm.Position]["TotalBonus"].ToString();
				ebSecurityTgt.Text	 = dt.Rows[cm.Position]["SecurityTgt"].ToString();
				ebPackageName.Text	 = dt.Rows[cm.Position]["packageName"].ToString();
				ebPrice.Text		 = dt.Rows[cm.Position]["Price"].ToString();

				C_ebTotalBonus();
				C_ebTotalTgt();		//ÃÑº¸³Ê½º ³ëÃâ
	            
                ebComment.Text      = dt.Rows[cm.Position]["Comment"].ToString();
				ebJob.Text			= dt.Rows[cm.Position]["JobName1"].ToString();
				ebJob2.Text			= dt.Rows[cm.Position]["JobName2"].ToString();
				ebJob3.Text			= dt.Rows[cm.Position]["JobName3"].ToString();

				string ContStartDay = dt.Rows[cm.Position]["ContStartDay"].ToString();

				SearchContractItem();		//°ü¸®³»¿ª
				
				if(ContStartDay.Length == 8)
				{
					int yyyy = Convert.ToInt32(ContStartDay.Substring(0, 4));
					int mm   = Convert.ToInt32(ContStartDay.Substring(4, 2));
					int dd   = Convert.ToInt32(ContStartDay.Substring(6, 2));
					ebContStartDay.Value = new DateTime(yyyy,mm,dd);	
				}
				else
				{
					ebContStartDay.Value = DateTime.Now;	
				}

				string ContEndDay = dt.Rows[cm.Position]["ContEndDay"].ToString();
				if(ContEndDay.Length == 8)
				{
					int yyyy = Convert.ToInt32(ContEndDay.Substring(0, 4));
					int mm   = Convert.ToInt32(ContEndDay.Substring(4, 2));
					int dd   = Convert.ToInt32(ContEndDay.Substring(6, 2));
					ebContEndDay.Value = new DateTime(yyyy,mm,dd);	
				}
				else
				{
					ebContEndDay.Value = DateTime.Now;	
				}

                ebContractAmt.Text   = dt.Rows[cm.Position]["ContractAmt"].ToString();
                ebContractName.Text  = dt.Rows[cm.Position]["ContractName"].ToString();
                ebModDt.Text        = dt.Rows[cm.Position]["ModDt"].ToString();
                ebRegDt.Text        = dt.Rows[cm.Position]["RegDt"].ToString();
                ebRegName.Text      = dt.Rows[cm.Position]["RegId"].ToString();

                IsAdding = false;
				
                SetTextReadOnly();           
            }
									  
            StatusMessage("ÁØºñ");
        }

        private void ResetDetailText()
        {
            keyContractSeq = "";
            ebMedia.Text = "";
            ebRap.Text = "";
            ebAgency.Text = "";
            ebAdvertiser.Text = "";
            cbContractState.SelectedIndex = 0;
            ebComment.Text       = "";
			ebContStartDay.Value = DateTime.Now;	
			ebContEndDay.Value   = DateTime.Now.AddMonths(1);	
            ebContractAmt.Text   = "";
            ebContractName.Text = "";
            ebModDt.Text        = "";
            ebRegDt.Text        = "";
            ebRegName.Text      = "";

        }
		
        /// <summary>
        /// »ó¼¼Á¤º¸ ReadOnly
        /// </summary>
        private void SetTextReadOnly()
        {
            //        btnAdvertise.Enabled = true;
			btnPackage.Enabled = true;
			btnJob.Enabled = true;
        }

        /// <summary>
        /// »ó¼¼Á¤º¸ ¼öÁ¤°¡´ÉÄÉ
        /// </summary>
        private void ResetTextReadOnly()
        {
            //            btnAdvertise.Enabled = false;
			btnPackage.Enabled = true;
			btnJob.Enabled = true;
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

        private void btnAdvertise_Click(object sender, System.EventArgs e)
        {
        
            // ±¤°í°è¾à ¸ñ·Ï °Ë»ö ÆË¾÷ ¶ì¿ì±â
            Contract_pForm pForm = new Contract_pForm(this);
					
            pForm.ShowDialog();
            
            pForm.Dispose();
            pForm = null;			
        }
		
        #region ¿ÜºÎ³ëÃâ ¸Þ¼Òµå
        /// <summary>
        /// ¼±ÅÃµÈ RowµéÀ» ÀÔ·Â½ÃÅ´
        /// </summary>
        /// <param name="dtc"></param>
        public void adOn_AddContract(ContractModel contractModel )
        {
            MediaCode       = contractModel.MediaCode;
            RapCode         = contractModel.RapCode;
            AgencyCode      = contractModel.AgencyCode;
            AdvertiserCode  = contractModel.AdvertiserCode;

            ebMedia.Text        = contractModel.MediaName;
            ebRap.Text          = contractModel.RapName;
            ebAgency.Text       = contractModel.AgencyName;
            ebAdvertiser.Text   = contractModel.AdvertiserName;
        }

        #endregion

		private void btnPackage_Click(object sender, System.EventArgs e)
		{
            /*
			// »óÇ°Á¤º¸ ¸ñ·Ï °Ë»ö ÆË¾÷ ¶ì¿ì±â - »óÇ°°Ë»ö¿¬µ¿ÇÏÁö ¾ÊÀ½
			ContractPackage_pForm pForm = new ContractPackage_pForm(this);

			pForm.ShowDialog();
            
			pForm.Dispose();
			pForm = null;
            */
		}
		

		public string AdTime
		{
			set
			{
				this.adtime = value;
				ebAdTime.Text      = adtime;
			}
		}
		public string BonusRate
		{
			set
			{
				this.bonusrate = value;
				ebBonusRate.Text      = bonusrate;
			}
		}
		public string ContractAmt1
		{
			set
			{
				this.contractAmt1 = value;
				ebContractAmt.Text      = contractAmt1;
			}
		}
		public string Price
		{
			set
			{
				this.price = value;
				ebPrice.Text      = price;
			}
		}

		public string PackageName
		{
			set
			{
				this.packageName = value;
				ebPackageName.Text      = packageName;
			}
		}

		private void btnJob_Click(object sender, System.EventArgs e)
		{
			// ¾÷Á¾ ¸ñ·Ï °Ë»ö ÆË¾÷ ¶ì¿ì±â
			ContractJob_pForm pForm = new ContractJob_pForm(this);

			pForm.ShowDialog();
            
			pForm.Dispose();
			pForm = null;

		}

		public void adOn_Contract(ContractModel contractModel )
		{
			jobCode			 = contractModel.JobCode;
			ebJob.Text       = contractModel.JobName;			
			ebJob2.Text       = contractModel.JobName2;			
			ebJob3.Text       = contractModel.JobName3;			
		}

		private void C_ebTotalBonus()
		{
			//ÃÑº¸³Ê½ºÀ²¸¦ ±¸ÇÏ´Â ºÎºÐ
			BonusRate1 = Convert.ToDecimal(ebBonusRate.Text);
			LongBonus = Convert.ToDecimal(ebLongBonus.Text);
			SpecialBonus = Convert.ToDecimal(ebSpecialBonus.Text);			
			TotalBonus = BonusRate1 + LongBonus + SpecialBonus;
			ebTotalBonus.Text = TotalBonus.ToString();

		}

		private void C_ebTotalTgt()
		{
			//ÃÑº¸³Ê½º³ëÃâÀ» ±¸ÇÏ´Â ºÎºÐ
			ContractAmt = Convert.ToDecimal(ebContractAmt.Text);
			TotalTgt = ContractAmt * TotalBonus/100;
			ebTotalTgt.Text = TotalTgt.ToString();
		}

		private void C_ebSecurityTgt()
		{	//ÃÑ³ëÃâÀ» ±¸ÇÏ´Â ºÎºÐ
			SecurityTgt = ContractAmt + TotalTgt;
			ebSecurityTgt.Text = SecurityTgt.ToString();
		}
	
		private void ebBonusRate_TextChanged(object sender, System.EventArgs e)
		{			
			if(ebBonusRate.Text.Equals(""))
			{
				return;				
			}
			else
			{
				C_ebTotalBonus();
			}
		}

		private void ebLongBonus_TextChanged(object sender, System.EventArgs e)
		{
			if(ebLongBonus.Text.Equals(""))
			{
				return;				
			}
			else
			{
				C_ebTotalBonus();
			}
		}

		private void ebSpecialBonus_TextChanged(object sender, System.EventArgs e)
		{
			if(ebSpecialBonus.Text.Equals(""))
			{
				return;				
			}
			else
			{
				C_ebTotalBonus();
				C_ebTotalTgt();
				C_ebSecurityTgt();
			}
        }		
						
    }
}

