// ===============================================================================
// AgencyControl for Charites Project
//
// AgencyControl.cs
//
// »ç¿ëÀÚÁ¤º¸°ü¸® ÄÁµå·ÑÀ» Á¤ÀÇÇÕ´Ï´Ù. 
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

using WinFramework.Misc;
using WinFramework.Base;
using WinFramework.Data;

using AdManagerModel;

namespace AdManagerClient
{
	/// <summary>
	/// »ç¿ëÀÚ°ü¸® ÄÁÆ®·Ñ
	/// </summary>
    public class AgencyControl : System.Windows.Forms.UserControl, IUserControl
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
		AgencyModel agencyModel  = new AgencyModel();	// »ç¿ëÀÚÁ¤º¸¸ðµ¨

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


		// Key µ¥ÀÌÅÍ
		string strAgencyCode       = "";
        private Janus.Windows.EditControls.UICheckBox uiCheckBox1;
        private Janus.Windows.EditControls.UICheckBox uiCheckBox2;
		string keyRepCode         = "";

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
		private Janus.Windows.GridEX.EditControls.EditBox ebSearchKey;
		private Janus.Windows.EditControls.UIButton btnSearch;
		private System.Windows.Forms.Panel pnlUserDetail;
		private System.Windows.Forms.Label lbRegDt;
		private Janus.Windows.EditControls.UIButton btnAdd;
		private Janus.Windows.EditControls.UIButton btnSave;
		private Janus.Windows.GridEX.EditControls.EditBox ebRegDt;
		private Janus.Windows.EditControls.UIButton btnDelete;
		private Janus.Windows.UI.Dock.UIPanelGroup uiPanelAgency;
		private Janus.Windows.UI.Dock.UIPanel uiPanelSearch;
		private Janus.Windows.UI.Dock.UIPanelInnerContainer uiPanelSearchContainer;
		private Janus.Windows.UI.Dock.UIPanel uiPanelList;
		private Janus.Windows.UI.Dock.UIPanelInnerContainer uiPanelListContainer;
		private Janus.Windows.UI.Dock.UIPanel uiPanelDetail;
		private Janus.Windows.UI.Dock.UIPanelInnerContainer uiPanelDetailContainer;
		private Janus.Windows.EditControls.UIComboBox cbSearchAgencyType;
		private System.Windows.Forms.Label lbAgencyName;
		private Janus.Windows.GridEX.EditControls.EditBox ebAgencyName;
		private Janus.Windows.EditControls.UIComboBox cbAgencyType;
		private System.Windows.Forms.Label lbAgencyType;
		private System.Windows.Forms.Label lbTell;
		private Janus.Windows.GridEX.EditControls.EditBox ebTell;
		private System.Windows.Forms.Label lbRegID;
		private Janus.Windows.GridEX.EditControls.EditBox ebModDt;
		private System.Windows.Forms.Label lbModDt;
		private System.Windows.Forms.Label lbAddress;
		private Janus.Windows.GridEX.EditControls.EditBox ebAddress;
		private System.Windows.Forms.Label lbUseYn;
		private Janus.Windows.EditControls.UIRadioButton rbUseYn_N;
		private Janus.Windows.GridEX.EditControls.EditBox ebComment;
		private System.Windows.Forms.Label lbComment;
		private Janus.Windows.EditControls.UIRadioButton rbUseYn_Y;
		private Janus.Windows.GridEX.GridEX grdExAgencyList;
		private Janus.Windows.GridEX.EditControls.EditBox ebRegName;
		private System.Data.DataView dvAgency;
        private AdManagerClient.AgencyDs agencyDs;
        private Janus.Windows.EditControls.UIComboBox cbSearchRap;
		private Janus.Windows.EditControls.UIButton btnRapSearch;
		private Janus.Windows.GridEX.EditControls.EditBox ebRapName;
		private System.Windows.Forms.Label label3;
		private System.ComponentModel.IContainer components;

		public AgencyControl()
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
            Janus.Windows.EditControls.UIComboBoxItem uiComboBoxItem1 = new Janus.Windows.EditControls.UIComboBoxItem();
            Janus.Windows.GridEX.GridEXLayout grdExAgencyList_DesignTimeLayout = new Janus.Windows.GridEX.GridEXLayout();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AgencyControl));
            this.uiPM = new Janus.Windows.UI.Dock.UIPanelManager(this.components);
            this.uiPanelAgency = new Janus.Windows.UI.Dock.UIPanelGroup();
            this.uiPanelSearch = new Janus.Windows.UI.Dock.UIPanel();
            this.uiPanelSearchContainer = new Janus.Windows.UI.Dock.UIPanelInnerContainer();
            this.pnlSearch = new System.Windows.Forms.Panel();
            this.uiCheckBox2 = new Janus.Windows.EditControls.UICheckBox();
            this.cbSearchRap = new Janus.Windows.EditControls.UIComboBox();
            this.cbSearchAgencyType = new Janus.Windows.EditControls.UIComboBox();
            this.ebSearchKey = new Janus.Windows.GridEX.EditControls.EditBox();
            this.btnSearch = new Janus.Windows.EditControls.UIButton();
            this.uiPanelList = new Janus.Windows.UI.Dock.UIPanel();
            this.uiPanelListContainer = new Janus.Windows.UI.Dock.UIPanelInnerContainer();
            this.grdExAgencyList = new Janus.Windows.GridEX.GridEX();
            this.dvAgency = new System.Data.DataView();
            this.agencyDs = new AdManagerClient.AgencyDs();
            this.uiPanelDetail = new Janus.Windows.UI.Dock.UIPanel();
            this.uiPanelDetailContainer = new Janus.Windows.UI.Dock.UIPanelInnerContainer();
            this.pnlUserDetail = new System.Windows.Forms.Panel();
            this.uiCheckBox1 = new Janus.Windows.EditControls.UICheckBox();
            this.btnRapSearch = new Janus.Windows.EditControls.UIButton();
            this.ebRapName = new Janus.Windows.GridEX.EditControls.EditBox();
            this.label3 = new System.Windows.Forms.Label();
            this.rbUseYn_N = new Janus.Windows.EditControls.UIRadioButton();
            this.rbUseYn_Y = new Janus.Windows.EditControls.UIRadioButton();
            this.ebComment = new Janus.Windows.GridEX.EditControls.EditBox();
            this.ebRegName = new Janus.Windows.GridEX.EditControls.EditBox();
            this.cbAgencyType = new Janus.Windows.EditControls.UIComboBox();
            this.lbAgencyType = new System.Windows.Forms.Label();
            this.ebAgencyName = new Janus.Windows.GridEX.EditControls.EditBox();
            this.lbAgencyName = new System.Windows.Forms.Label();
            this.ebTell = new Janus.Windows.GridEX.EditControls.EditBox();
            this.lbTell = new System.Windows.Forms.Label();
            this.lbRegID = new System.Windows.Forms.Label();
            this.ebModDt = new Janus.Windows.GridEX.EditControls.EditBox();
            this.lbModDt = new System.Windows.Forms.Label();
            this.lbRegDt = new System.Windows.Forms.Label();
            this.ebRegDt = new Janus.Windows.GridEX.EditControls.EditBox();
            this.lbComment = new System.Windows.Forms.Label();
            this.lbAddress = new System.Windows.Forms.Label();
            this.ebAddress = new Janus.Windows.GridEX.EditControls.EditBox();
            this.lbUseYn = new System.Windows.Forms.Label();
            this.btnDelete = new Janus.Windows.EditControls.UIButton();
            this.btnAdd = new Janus.Windows.EditControls.UIButton();
            this.btnSave = new Janus.Windows.EditControls.UIButton();
            ((System.ComponentModel.ISupportInitialize)(this.uiPM)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelAgency)).BeginInit();
            this.uiPanelAgency.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelSearch)).BeginInit();
            this.uiPanelSearch.SuspendLayout();
            this.uiPanelSearchContainer.SuspendLayout();
            this.pnlSearch.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelList)).BeginInit();
            this.uiPanelList.SuspendLayout();
            this.uiPanelListContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdExAgencyList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dvAgency)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.agencyDs)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelDetail)).BeginInit();
            this.uiPanelDetail.SuspendLayout();
            this.uiPanelDetailContainer.SuspendLayout();
            this.pnlUserDetail.SuspendLayout();
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
            this.uiPanelAgency.Id = new System.Guid("b162c53a-7940-47cd-a0ed-9762c59c50d8");
            this.uiPanelAgency.StaticGroup = true;
            this.uiPanelSearch.Id = new System.Guid("afb3b7d0-aa79-443c-a1e8-3baba38f7a4d");
            this.uiPanelAgency.Panels.Add(this.uiPanelSearch);
            this.uiPanelList.Id = new System.Guid("8e3140b9-bfcb-43af-b84e-2ee6cddf51dc");
            this.uiPanelAgency.Panels.Add(this.uiPanelList);
            this.uiPanelDetail.Id = new System.Guid("b2973a57-d3e1-4d80-9387-7a6dc3332542");
            this.uiPanelAgency.Panels.Add(this.uiPanelDetail);
            this.uiPM.Panels.Add(this.uiPanelAgency);
            // 
            // Design Time Panel Info:
            // 
            this.uiPM.BeginPanelInfo();
            this.uiPM.AddDockPanelInfo(new System.Guid("b162c53a-7940-47cd-a0ed-9762c59c50d8"), Janus.Windows.UI.Dock.PanelGroupStyle.HorizontalTiles, Janus.Windows.UI.Dock.PanelDockStyle.Fill, true, new System.Drawing.Size(1010, 677), true);
            this.uiPM.AddDockPanelInfo(new System.Guid("afb3b7d0-aa79-443c-a1e8-3baba38f7a4d"), new System.Guid("b162c53a-7940-47cd-a0ed-9762c59c50d8"), 42, true);
            this.uiPM.AddDockPanelInfo(new System.Guid("8e3140b9-bfcb-43af-b84e-2ee6cddf51dc"), new System.Guid("b162c53a-7940-47cd-a0ed-9762c59c50d8"), 390, true);
            this.uiPM.AddDockPanelInfo(new System.Guid("b2973a57-d3e1-4d80-9387-7a6dc3332542"), new System.Guid("b162c53a-7940-47cd-a0ed-9762c59c50d8"), 195, true);
            this.uiPM.AddFloatingPanelInfo(new System.Guid("b162c53a-7940-47cd-a0ed-9762c59c50d8"), Janus.Windows.UI.Dock.PanelGroupStyle.HorizontalTiles, true, new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
            this.uiPM.AddFloatingPanelInfo(new System.Guid("8e3140b9-bfcb-43af-b84e-2ee6cddf51dc"), new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
            this.uiPM.AddFloatingPanelInfo(new System.Guid("b2973a57-d3e1-4d80-9387-7a6dc3332542"), new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
            this.uiPM.AddFloatingPanelInfo(new System.Guid("afb3b7d0-aa79-443c-a1e8-3baba38f7a4d"), new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
            this.uiPM.EndPanelInfo();
            // 
            // uiPanelAgency
            // 
            this.uiPanelAgency.ActiveCaptionMode = Janus.Windows.UI.Dock.ActiveCaptionMode.Never;
            this.uiPanelAgency.BackColor = System.Drawing.SystemColors.Window;
            this.uiPanelAgency.CaptionVisible = Janus.Windows.UI.InheritableBoolean.True;
            this.uiPanelAgency.CloseButtonVisible = Janus.Windows.UI.InheritableBoolean.False;
            this.uiPanelAgency.Location = new System.Drawing.Point(0, 0);
            this.uiPanelAgency.Name = "uiPanelAgency";
            this.uiPanelAgency.Size = new System.Drawing.Size(1010, 677);
            this.uiPanelAgency.TabIndex = 4;
            this.uiPanelAgency.Text = "´ëÇà»ç°ü¸®";
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
            this.pnlSearch.Controls.Add(this.uiCheckBox2);
            this.pnlSearch.Controls.Add(this.cbSearchRap);
            this.pnlSearch.Controls.Add(this.cbSearchAgencyType);
            this.pnlSearch.Controls.Add(this.ebSearchKey);
            this.pnlSearch.Controls.Add(this.btnSearch);
            this.pnlSearch.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlSearch.Location = new System.Drawing.Point(0, 0);
            this.pnlSearch.Name = "pnlSearch";
            this.pnlSearch.Size = new System.Drawing.Size(1008, 41);
            this.pnlSearch.TabIndex = 3;
            // 
            // uiCheckBox2
            // 
            this.uiCheckBox2.Location = new System.Drawing.Point(486, 6);
            this.uiCheckBox2.Name = "uiCheckBox2";
            this.uiCheckBox2.Size = new System.Drawing.Size(104, 23);
            this.uiCheckBox2.TabIndex = 6;
            this.uiCheckBox2.Text = "»ç¿ë¾ÈÇÔ Æ÷ÇÔ";
            // 
            // cbSearchRap
            // 
            this.cbSearchRap.ComboStyle = Janus.Windows.EditControls.ComboStyle.DropDownList;
            this.cbSearchRap.Location = new System.Drawing.Point(144, 8);
            this.cbSearchRap.Name = "cbSearchRap";
            this.cbSearchRap.Size = new System.Drawing.Size(120, 21);
            this.cbSearchRap.TabIndex = 2;
            this.cbSearchRap.Text = "¹Ìµð¾î·¾¼±ÅÃ";
            this.cbSearchRap.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            // 
            // cbSearchAgencyType
            // 
            this.cbSearchAgencyType.ComboStyle = Janus.Windows.EditControls.ComboStyle.DropDownList;
            uiComboBoxItem1.FormatStyle.Alpha = 0;
            uiComboBoxItem1.IsSeparator = false;
            uiComboBoxItem1.Text = "´ëÇà»ç±¸ºÐ ¼±ÅÃ";
            uiComboBoxItem1.Value = "00";
            this.cbSearchAgencyType.Items.AddRange(new Janus.Windows.EditControls.UIComboBoxItem[] {
            uiComboBoxItem1});
            this.cbSearchAgencyType.Location = new System.Drawing.Point(8, 8);
            this.cbSearchAgencyType.Name = "cbSearchAgencyType";
            this.cbSearchAgencyType.SelectedIndex = 0;
            this.cbSearchAgencyType.Size = new System.Drawing.Size(128, 21);
            this.cbSearchAgencyType.TabIndex = 1;
            this.cbSearchAgencyType.Text = "´ëÇà»ç±¸ºÐ ¼±ÅÃ";
            this.cbSearchAgencyType.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            // 
            // ebSearchKey
            // 
            this.ebSearchKey.Location = new System.Drawing.Point(272, 8);
            this.ebSearchKey.Name = "ebSearchKey";
            this.ebSearchKey.Size = new System.Drawing.Size(208, 21);
            this.ebSearchKey.TabIndex = 3;
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
            this.btnSearch.Location = new System.Drawing.Point(895, 8);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(104, 24);
            this.btnSearch.TabIndex = 5;
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
            this.uiPanelList.Size = new System.Drawing.Size(1010, 403);
            this.uiPanelList.TabIndex = 4;
            this.uiPanelList.TabStop = false;
            this.uiPanelList.Text = "´ëÇà»ç¸ñ·Ï";
            // 
            // uiPanelListContainer
            // 
            this.uiPanelListContainer.BackColor = System.Drawing.SystemColors.Window;
            this.uiPanelListContainer.Controls.Add(this.grdExAgencyList);
            this.uiPanelListContainer.Location = new System.Drawing.Point(1, 23);
            this.uiPanelListContainer.Name = "uiPanelListContainer";
            this.uiPanelListContainer.Size = new System.Drawing.Size(1008, 379);
            this.uiPanelListContainer.TabIndex = 0;
            // 
            // grdExAgencyList
            // 
            this.grdExAgencyList.AllowEdit = Janus.Windows.GridEX.InheritableBoolean.False;
            this.grdExAgencyList.AlternatingColors = true;
            this.grdExAgencyList.BorderStyle = Janus.Windows.GridEX.BorderStyle.None;
            this.grdExAgencyList.DataSource = this.dvAgency;
            grdExAgencyList_DesignTimeLayout.LayoutString = resources.GetString("grdExAgencyList_DesignTimeLayout.LayoutString");
            this.grdExAgencyList.DesignTimeLayout = grdExAgencyList_DesignTimeLayout;
            this.grdExAgencyList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdExAgencyList.EditorsControlStyle.ButtonAppearance = Janus.Windows.GridEX.ButtonAppearance.Regular;
            this.grdExAgencyList.EmptyRows = true;
            this.grdExAgencyList.FocusCellFormatStyle.BackColor = System.Drawing.SystemColors.Highlight;
            this.grdExAgencyList.FocusCellFormatStyle.ForeColor = System.Drawing.Color.White;
            this.grdExAgencyList.FocusStyle = Janus.Windows.GridEX.FocusStyle.None;
            this.grdExAgencyList.GridLineColor = System.Drawing.Color.Silver;
            this.grdExAgencyList.GridLines = Janus.Windows.GridEX.GridLines.Vertical;
            this.grdExAgencyList.GridLineStyle = Janus.Windows.GridEX.GridLineStyle.Solid;
            this.grdExAgencyList.GroupByBoxVisible = false;
            this.grdExAgencyList.HideSelection = Janus.Windows.GridEX.HideSelection.HighlightInactive;
            this.grdExAgencyList.Location = new System.Drawing.Point(0, 0);
            this.grdExAgencyList.Name = "grdExAgencyList";
            this.grdExAgencyList.ScrollBars = Janus.Windows.GridEX.ScrollBars.Vertical;
            this.grdExAgencyList.SelectedInactiveFormatStyle.BackColor = System.Drawing.Color.Gold;
            this.grdExAgencyList.Size = new System.Drawing.Size(1008, 379);
            this.grdExAgencyList.TabIndex = 6;
            this.grdExAgencyList.TabKeyBehavior = Janus.Windows.GridEX.TabKeyBehavior.ControlNavigation;
            this.grdExAgencyList.ThemedAreas = ((Janus.Windows.GridEX.ThemedArea)(((((((((Janus.Windows.GridEX.ThemedArea.ScrollBars | Janus.Windows.GridEX.ThemedArea.EditControls) 
            | Janus.Windows.GridEX.ThemedArea.Headers) 
            | Janus.Windows.GridEX.ThemedArea.GroupByBox) 
            | Janus.Windows.GridEX.ThemedArea.GroupRows) 
            | Janus.Windows.GridEX.ThemedArea.ControlBorder) 
            | Janus.Windows.GridEX.ThemedArea.Cards) 
            | Janus.Windows.GridEX.ThemedArea.Gridlines) 
            | Janus.Windows.GridEX.ThemedArea.CheckBoxes)));
            this.grdExAgencyList.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            this.grdExAgencyList.Enter += new System.EventHandler(this.OnGrdRowChanged);
            // 
            // dvAgency
            // 
            this.dvAgency.Table = this.agencyDs.Agency;
            // 
            // agencyDs
            // 
            this.agencyDs.DataSetName = "AgencyDs";
            this.agencyDs.Locale = new System.Globalization.CultureInfo("en-US");
            this.agencyDs.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // uiPanelDetail
            // 
            this.uiPanelDetail.AllowResize = Janus.Windows.UI.InheritableBoolean.False;
            this.uiPanelDetail.BackColor = System.Drawing.SystemColors.Window;
            this.uiPanelDetail.CaptionStyle = Janus.Windows.UI.Dock.PanelCaptionStyle.Dark;
            this.uiPanelDetail.InnerContainer = this.uiPanelDetailContainer;
            this.uiPanelDetail.Location = new System.Drawing.Point(0, 476);
            this.uiPanelDetail.Name = "uiPanelDetail";
            this.uiPanelDetail.Size = new System.Drawing.Size(1010, 201);
            this.uiPanelDetail.TabIndex = 4;
            this.uiPanelDetail.TabStop = false;
            this.uiPanelDetail.Text = "»ó¼¼Á¤º¸";
            // 
            // uiPanelDetailContainer
            // 
            this.uiPanelDetailContainer.Controls.Add(this.pnlUserDetail);
            this.uiPanelDetailContainer.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.uiPanelDetailContainer.Location = new System.Drawing.Point(1, 23);
            this.uiPanelDetailContainer.Name = "uiPanelDetailContainer";
            this.uiPanelDetailContainer.Size = new System.Drawing.Size(1008, 177);
            this.uiPanelDetailContainer.TabIndex = 0;
            // 
            // pnlUserDetail
            // 
            this.pnlUserDetail.BackColor = System.Drawing.SystemColors.Window;
            this.pnlUserDetail.Controls.Add(this.uiCheckBox1);
            this.pnlUserDetail.Controls.Add(this.btnRapSearch);
            this.pnlUserDetail.Controls.Add(this.ebRapName);
            this.pnlUserDetail.Controls.Add(this.label3);
            this.pnlUserDetail.Controls.Add(this.rbUseYn_N);
            this.pnlUserDetail.Controls.Add(this.rbUseYn_Y);
            this.pnlUserDetail.Controls.Add(this.ebComment);
            this.pnlUserDetail.Controls.Add(this.ebRegName);
            this.pnlUserDetail.Controls.Add(this.cbAgencyType);
            this.pnlUserDetail.Controls.Add(this.lbAgencyType);
            this.pnlUserDetail.Controls.Add(this.ebAgencyName);
            this.pnlUserDetail.Controls.Add(this.lbAgencyName);
            this.pnlUserDetail.Controls.Add(this.ebTell);
            this.pnlUserDetail.Controls.Add(this.lbTell);
            this.pnlUserDetail.Controls.Add(this.lbRegID);
            this.pnlUserDetail.Controls.Add(this.ebModDt);
            this.pnlUserDetail.Controls.Add(this.lbModDt);
            this.pnlUserDetail.Controls.Add(this.lbRegDt);
            this.pnlUserDetail.Controls.Add(this.ebRegDt);
            this.pnlUserDetail.Controls.Add(this.lbComment);
            this.pnlUserDetail.Controls.Add(this.lbAddress);
            this.pnlUserDetail.Controls.Add(this.ebAddress);
            this.pnlUserDetail.Controls.Add(this.lbUseYn);
            this.pnlUserDetail.Controls.Add(this.btnDelete);
            this.pnlUserDetail.Controls.Add(this.btnAdd);
            this.pnlUserDetail.Controls.Add(this.btnSave);
            this.pnlUserDetail.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlUserDetail.Location = new System.Drawing.Point(0, 0);
            this.pnlUserDetail.Name = "pnlUserDetail";
            this.pnlUserDetail.Size = new System.Drawing.Size(1008, 177);
            this.pnlUserDetail.TabIndex = 3;
            // 
            // uiCheckBox1
            // 
            this.uiCheckBox1.Location = new System.Drawing.Point(390, 6);
            this.uiCheckBox1.Name = "uiCheckBox1";
            this.uiCheckBox1.Size = new System.Drawing.Size(104, 23);
            this.uiCheckBox1.TabIndex = 196;
            this.uiCheckBox1.Text = "¹Ìµð¾î·¾ °ø¿ë";
            // 
            // btnRapSearch
            // 
            this.btnRapSearch.Enabled = false;
            this.btnRapSearch.Location = new System.Drawing.Point(280, 8);
            this.btnRapSearch.Name = "btnRapSearch";
            this.btnRapSearch.Size = new System.Drawing.Size(104, 21);
            this.btnRapSearch.TabIndex = 8;
            this.btnRapSearch.Text = "¹Ìµð¾î·¾¼±ÅÃ";
            this.btnRapSearch.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.btnRapSearch.Click += new System.EventHandler(this.btnRapSearch_Click);
            // 
            // ebRapName
            // 
            this.ebRapName.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ebRapName.Location = new System.Drawing.Point(80, 8);
            this.ebRapName.Name = "ebRapName";
            this.ebRapName.Size = new System.Drawing.Size(192, 21);
            this.ebRapName.TabIndex = 7;
            this.ebRapName.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
            this.ebRapName.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(8, 8);
            this.label3.Name = "label3";
            this.label3.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.label3.Size = new System.Drawing.Size(72, 21);
            this.label3.TabIndex = 195;
            this.label3.Text = "¹Ìµð¾î·¾";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // rbUseYn_N
            // 
            this.rbUseYn_N.BackColor = System.Drawing.Color.Transparent;
            this.rbUseYn_N.CheckedValue = "N";
            this.rbUseYn_N.Location = new System.Drawing.Point(744, 104);
            this.rbUseYn_N.Name = "rbUseYn_N";
            this.rbUseYn_N.Size = new System.Drawing.Size(80, 21);
            this.rbUseYn_N.TabIndex = 13;
            this.rbUseYn_N.Text = "»ç¿ë¾ÈÇÔ";
            this.rbUseYn_N.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            // 
            // rbUseYn_Y
            // 
            this.rbUseYn_Y.BackColor = System.Drawing.Color.Transparent;
            this.rbUseYn_Y.CheckedValue = "Y";
            this.rbUseYn_Y.Location = new System.Drawing.Point(672, 104);
            this.rbUseYn_Y.Name = "rbUseYn_Y";
            this.rbUseYn_Y.Size = new System.Drawing.Size(72, 21);
            this.rbUseYn_Y.TabIndex = 15;
            this.rbUseYn_Y.Text = "»ç¿ëÇÔ";
            this.rbUseYn_Y.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            // 
            // ebComment
            // 
            this.ebComment.Location = new System.Drawing.Point(80, 80);
            this.ebComment.MaxLength = 50;
            this.ebComment.Multiline = true;
            this.ebComment.Name = "ebComment";
            this.ebComment.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.ebComment.Size = new System.Drawing.Size(504, 48);
            this.ebComment.TabIndex = 14;
            this.ebComment.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
            this.ebComment.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            // 
            // ebRegName
            // 
            this.ebRegName.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ebRegName.Location = new System.Drawing.Point(672, 80);
            this.ebRegName.MaxLength = 15;
            this.ebRegName.Name = "ebRegName";
            this.ebRegName.ReadOnly = true;
            this.ebRegName.Size = new System.Drawing.Size(234, 21);
            this.ebRegName.TabIndex = 16;
            this.ebRegName.TabStop = false;
            this.ebRegName.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
            this.ebRegName.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            // 
            // cbAgencyType
            // 
            this.cbAgencyType.ComboStyle = Janus.Windows.EditControls.ComboStyle.DropDownList;
            this.cbAgencyType.DataSource = this.agencyDs.AgencyType;
            this.cbAgencyType.DisplayMember = "CodeName";
            this.cbAgencyType.Location = new System.Drawing.Point(472, 32);
            this.cbAgencyType.Name = "cbAgencyType";
            this.cbAgencyType.Size = new System.Drawing.Size(112, 21);
            this.cbAgencyType.TabIndex = 11;
            this.cbAgencyType.ValueMember = "Code";
            this.cbAgencyType.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            // 
            // lbAgencyType
            // 
            this.lbAgencyType.Location = new System.Drawing.Point(432, 32);
            this.lbAgencyType.Name = "lbAgencyType";
            this.lbAgencyType.Size = new System.Drawing.Size(32, 21);
            this.lbAgencyType.TabIndex = 22;
            this.lbAgencyType.Text = "±¸ºÐ";
            this.lbAgencyType.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ebAgencyName
            // 
            this.ebAgencyName.Location = new System.Drawing.Point(80, 32);
            this.ebAgencyName.MaxLength = 40;
            this.ebAgencyName.Name = "ebAgencyName";
            this.ebAgencyName.ReadOnly = true;
            this.ebAgencyName.Size = new System.Drawing.Size(320, 21);
            this.ebAgencyName.TabIndex = 10;
            this.ebAgencyName.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
            this.ebAgencyName.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            // 
            // lbAgencyName
            // 
            this.lbAgencyName.Location = new System.Drawing.Point(8, 32);
            this.lbAgencyName.Name = "lbAgencyName";
            this.lbAgencyName.Size = new System.Drawing.Size(72, 21);
            this.lbAgencyName.TabIndex = 18;
            this.lbAgencyName.Text = "´ëÇà»ç¸í";
            this.lbAgencyName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ebTell
            // 
            this.ebTell.Location = new System.Drawing.Point(80, 56);
            this.ebTell.MaxLength = 15;
            this.ebTell.Name = "ebTell";
            this.ebTell.Size = new System.Drawing.Size(136, 21);
            this.ebTell.TabIndex = 12;
            this.ebTell.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
            this.ebTell.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            // 
            // lbTell
            // 
            this.lbTell.Location = new System.Drawing.Point(8, 56);
            this.lbTell.Name = "lbTell";
            this.lbTell.Size = new System.Drawing.Size(72, 21);
            this.lbTell.TabIndex = 19;
            this.lbTell.Text = "ÀüÈ­¹øÈ£";
            this.lbTell.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lbRegID
            // 
            this.lbRegID.Location = new System.Drawing.Point(600, 80);
            this.lbRegID.Name = "lbRegID";
            this.lbRegID.Size = new System.Drawing.Size(72, 21);
            this.lbRegID.TabIndex = 29;
            this.lbRegID.Text = "µî·ÏÀÚ";
            this.lbRegID.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ebModDt
            // 
            this.ebModDt.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ebModDt.Location = new System.Drawing.Point(672, 56);
            this.ebModDt.Name = "ebModDt";
            this.ebModDt.ReadOnly = true;
            this.ebModDt.Size = new System.Drawing.Size(234, 21);
            this.ebModDt.TabIndex = 15;
            this.ebModDt.TabStop = false;
            this.ebModDt.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
            this.ebModDt.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            // 
            // lbModDt
            // 
            this.lbModDt.Location = new System.Drawing.Point(600, 56);
            this.lbModDt.Name = "lbModDt";
            this.lbModDt.Size = new System.Drawing.Size(72, 21);
            this.lbModDt.TabIndex = 29;
            this.lbModDt.Text = "ÃÖÁ¾¼öÁ¤ ";
            this.lbModDt.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lbRegDt
            // 
            this.lbRegDt.Location = new System.Drawing.Point(600, 32);
            this.lbRegDt.Name = "lbRegDt";
            this.lbRegDt.Size = new System.Drawing.Size(72, 21);
            this.lbRegDt.TabIndex = 29;
            this.lbRegDt.Text = "µî·ÏÀÏ½Ã ";
            this.lbRegDt.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ebRegDt
            // 
            this.ebRegDt.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ebRegDt.Location = new System.Drawing.Point(672, 32);
            this.ebRegDt.Name = "ebRegDt";
            this.ebRegDt.ReadOnly = true;
            this.ebRegDt.Size = new System.Drawing.Size(234, 21);
            this.ebRegDt.TabIndex = 14;
            this.ebRegDt.TabStop = false;
            this.ebRegDt.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
            this.ebRegDt.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            // 
            // lbComment
            // 
            this.lbComment.Location = new System.Drawing.Point(8, 80);
            this.lbComment.Name = "lbComment";
            this.lbComment.Size = new System.Drawing.Size(72, 21);
            this.lbComment.TabIndex = 32;
            this.lbComment.Text = "ºñ°í";
            this.lbComment.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lbAddress
            // 
            this.lbAddress.Location = new System.Drawing.Point(256, 56);
            this.lbAddress.Name = "lbAddress";
            this.lbAddress.Size = new System.Drawing.Size(40, 21);
            this.lbAddress.TabIndex = 30;
            this.lbAddress.Text = "ÁÖ¼Ò";
            this.lbAddress.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lbAddress.Visible = false;
            // 
            // ebAddress
            // 
            this.ebAddress.Location = new System.Drawing.Point(304, 56);
            this.ebAddress.MaxLength = 50;
            this.ebAddress.Name = "ebAddress";
            this.ebAddress.Size = new System.Drawing.Size(280, 21);
            this.ebAddress.TabIndex = 13;
            this.ebAddress.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
            this.ebAddress.Visible = false;
            this.ebAddress.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            // 
            // lbUseYn
            // 
            this.lbUseYn.Location = new System.Drawing.Point(600, 104);
            this.lbUseYn.Name = "lbUseYn";
            this.lbUseYn.Size = new System.Drawing.Size(72, 21);
            this.lbUseYn.TabIndex = 19;
            this.lbUseYn.Text = "»ç¿ë¿©ºÎ";
            this.lbUseYn.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnDelete
            // 
            this.btnDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnDelete.Enabled = false;
            this.btnDelete.Location = new System.Drawing.Point(190, 143);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(104, 24);
            this.btnDelete.TabIndex = 17;
            this.btnDelete.Text = "»è Á¦";
            this.btnDelete.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnAdd
            // 
            this.btnAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnAdd.Enabled = false;
            this.btnAdd.Location = new System.Drawing.Point(302, 143);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(104, 24);
            this.btnAdd.TabIndex = 18;
            this.btnAdd.Text = "Ãß °¡";
            this.btnAdd.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnSave.Enabled = false;
            this.btnSave.Location = new System.Drawing.Point(78, 143);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(104, 24);
            this.btnSave.TabIndex = 16;
            this.btnSave.Text = "Àú Àå";
            this.btnSave.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // AgencyControl
            // 
            this.Controls.Add(this.uiPanelAgency);
            this.Font = global::AdManagerClient.Properties.Settings.Default.Font1;
            this.Name = "AgencyControl";
            this.Size = new System.Drawing.Size(1010, 677);
            this.Load += new System.EventHandler(this.UserControl_Load);
            ((System.ComponentModel.ISupportInitialize)(this.uiPM)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelAgency)).EndInit();
            this.uiPanelAgency.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelSearch)).EndInit();
            this.uiPanelSearch.ResumeLayout(false);
            this.uiPanelSearchContainer.ResumeLayout(false);
            this.pnlSearch.ResumeLayout(false);
            this.pnlSearch.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelList)).EndInit();
            this.uiPanelList.ResumeLayout(false);
            this.uiPanelListContainer.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdExAgencyList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dvAgency)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.agencyDs)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelDetail)).EndInit();
            this.uiPanelDetail.ResumeLayout(false);
            this.uiPanelDetailContainer.ResumeLayout(false);
            this.pnlUserDetail.ResumeLayout(false);
            this.pnlUserDetail.PerformLayout();
            this.ResumeLayout(false);

		}
		#endregion

		#region ÄÁÆ®·Ñ ·Îµå
		private void UserControl_Load(object sender, System.EventArgs e)
		{
			// µ¥ÀÌÅÍ°ü¸®¿ë °´Ã¼»ý¼º
			dt = ((DataView)grdExAgencyList.DataSource).Table;  
			cm = (CurrencyManager) this.BindingContext[grdExAgencyList.DataSource]; 
			cm.PositionChanged += new System.EventHandler(OnGrdRowChanged); 

			// ÄÁÆ®·Ñ ÃÊ±âÈ­
			InitControl();	
		}

		#endregion

		#region ÄÁÆ®·Ñ ÃÊ±âÈ­
		private void InitControl()
		{
            ProgressStart();
			Init_RapCode();
			InitCombo();			
			// Á¶È¸±ÇÇÑ °Ë»ç
			if(menu.CanRead(MenuCode))
			{
				canRead = true;
				SearchAgencys();
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
				//ResetTextReadonly();
				canUpdate = true;
			}
			else
			{
				SetTextReadonly();
			}

			InitButton();
			InitCombo_Level();
            
            ProgressStop();
		}

		private void InitCombo()
		{
			// ÄÚµå¿¡¼­ º¸¾È·¹º§À» Á¶È¸ÇÑ´Ù.
			CodeModel codeModel = new CodeModel();
			codeModel.Section = "22";				// ÄÚµåºÐ·ù '22':´ëÇà»ç±¸ºÐ  TODO: ÄÚµåºÐ·ù´Â ÃßÈÄ XML·Î °ü¸®µÇ¾î¾ß...
			new CodeManager(systemModel, commonModel).GetCodeList(codeModel);
			
			if (codeModel.ResultCD.Equals("0000"))
			{
				// µ¥ÀÌÅÍ¼Â¿¡ ¼ÂÆÃ
				Utility.SetDataTable(agencyDs.AgencyType, codeModel.CodeDataSet);				
			}

			// »ó¼¼Á¶È¸ ÄÞº¸
			// »ó¼¼Á¤º¸ÀÇ ÄÞº¸´Â DatasetÀ» µ¥ÀÌÅÍ¼Ò½º·Î °¡Áø´Ù.

			// °Ë»öÁ¶°ÇÀÇ ÄÞº¸
			this.cbSearchAgencyType.Items.Clear();
			
			// ÄÞº¸¹Ú½º¿¡ ¼ÂÆ®ÇÒ ÄÚµå¸ñ·ÏÀ» ´ãÀ» Item¹è¿­À» ¼±¾ð
			Janus.Windows.EditControls.UIComboBoxItem[] comboItems = new Janus.Windows.EditControls.UIComboBoxItem[codeModel.ResultCnt + 1];

			comboItems[0] = new Janus.Windows.EditControls.UIComboBoxItem("´ëÇà»ç±¸ºÐ¼±ÅÃ","00");
			
			for(int i=0;i<codeModel.ResultCnt;i++)
			{
				DataRow row = agencyDs.AgencyType.Rows[i];

				string val = row["Code"].ToString();
				string txt = row["CodeName"].ToString();
				comboItems[i+1] = new Janus.Windows.EditControls.UIComboBoxItem(txt,val);
			}
			// ÄÞº¸¿¡ ¼ÂÆ®
			this.cbSearchAgencyType.Items.AddRange(comboItems);
			this.cbSearchAgencyType.SelectedIndex = 0;
			Application.DoEvents();
		}

		private void InitCombo_Level() 
		{			

			//¹Ìµð¾î·¾ ·¹º§ÀÏ°æ¿ìÀÇ º¸¾Èµî±ÞÃ³¸®
			if (commonModel.UserLevel == "30")
			{
				//Á¶È¸ ÄÞº¸ ÇÈ½º
				cbSearchRap.SelectedValue = commonModel.RapCode;
				cbSearchRap.ReadOnly = true;                
			}
			Application.DoEvents();
		}   

		private void Init_RapCode()
		{
            /* ·¦»ç´Â ¸ð¹ÙÀÏ·Î °íÁ¤ Ã³¸® ÇØ¼­ ¿¬µ¿ ÇÏÁö ¾ÊÀ½
			// ·¦À» Á¶È¸ÇÑ´Ù.
			MediaRapCodeModel mediaRapCodeModel = new MediaRapCodeModel();
			new MediaRapCodeManager(systemModel, commonModel).GetMediaRapCodeList(mediaRapCodeModel);
			
			if (mediaRapCodeModel.ResultCD.Equals("0000"))
			{
				// µ¥ÀÌÅÍ¼Â¿¡ ¼ÂÆÃ
				Utility.SetDataTable(agencyDs.MediaRap, mediaRapCodeModel.MediaRapCodeDataSet);				
			}

			// °Ë»öÁ¶°ÇÀÇ ÄÞº¸
			this.cbSearchRap.Items.Clear();
			// ÄÞº¸¹Ú½º¿¡ ¼ÂÆ®ÇÒ ÄÚµå¸ñ·ÏÀ» ´ãÀ» Item¹è¿­À» ¼±¾ð
			Janus.Windows.EditControls.UIComboBoxItem[] comboItems = new Janus.Windows.EditControls.UIComboBoxItem[mediaRapCodeModel.ResultCnt + 1];

			comboItems[0] = new Janus.Windows.EditControls.UIComboBoxItem("¹Ìµð¾î·¾¼±ÅÃ","00");
			
			for(int i=0;i<mediaRapCodeModel.ResultCnt;i++)
			{
				DataRow row = agencyDs.MediaRap.Rows[i];

				string val = row["RapCode"].ToString();
				string txt = row["RapName"].ToString();
				comboItems[i+1] = new Janus.Windows.EditControls.UIComboBoxItem(txt,val);
			}
			// ÄÞº¸¿¡ ¼ÂÆ®
			this.cbSearchRap.Items.AddRange(comboItems);
			this.cbSearchRap.SelectedIndex = 0;

			Application.DoEvents();
            */
            this.cbSearchRap.Items.Clear();
            DataSet ds = new DataSet("rapset");
            DataTable dt = new DataTable("rap");
            dt.Columns.Add("RapCode", typeof(string));
            dt.Columns.Add("RapName", typeof(string));
            DataRow nRow = dt.NewRow();
            nRow["RapCode"] = "1";
            nRow["RapName"] = "¸ð¹ÙÀÏ Æí¼ºÆÀ";
            dt.Rows.Add(nRow);
            ds.Tables.Add(dt);

            Utility.SetDataTable(agencyDs.MediaRap, ds);
            // °Ë»öÁ¶°ÇÀÇ ÄÞº¸
            this.cbSearchRap.Items.Clear();
            // ÄÞº¸¹Ú½º¿¡ ¼ÂÆ®ÇÒ ÄÚµå¸ñ·ÏÀ» ´ãÀ» Item¹è¿­À» ¼±¾ð
            Janus.Windows.EditControls.UIComboBoxItem[] comboItems = new Janus.Windows.EditControls.UIComboBoxItem[2];

            comboItems[0] = new Janus.Windows.EditControls.UIComboBoxItem("¹Ìµð¾î·¾¼±ÅÃ", "00");

            for (int i = 0; i < 1; i++)
            {
                DataRow row = agencyDs.MediaRap.Rows[i];

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
	
		private void InitButton()
		{
			if(canRead)   btnSearch.Enabled = true;
			if(canCreate) btnAdd.Enabled    = true;

			if(ebAgencyName.Text.Trim().Length > 0) 
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
            if (!IsSearching) // 2011.11.29 JH.Park Á¶È¸ÁßÀÌ ¾Æ´Ò°æ¿ì¿¡¸¸ µ¿ÀÛÇÏµµ·Ï º¯°æ
            {
                if (grdExAgencyList.RecordCount > 0)
                {
                    SetDetailText();
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
			SearchAgencys();
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

			IsAdding = true;

			ResetTextReadonly();
			ResetDetailText();

			ebAgencyName.Focus();
		}

		/// <summary>
		/// ÀúÀå¹öÆ° Å¬¸¯
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnSave_Click(object sender, System.EventArgs e)
		{
			SaveAgencyDetail();
		}

		/// <summary>
		/// »èÁ¦¹öÆ° Å¬¸¯
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnDelete_Click(object sender, System.EventArgs e)
		{
			DeleteAgency();
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
				SearchAgencys();
			}
		}

		#endregion

		#region Ã³¸®¸Þ¼Òµå

		/// <summary>
		/// ´ëÇà»ç¸ñ·Ï Á¶È¸
		/// </summary>
		private void SearchAgencys()
		{
            IsSearching = true;

			StatusMessage("´ëÇà»ç Á¤º¸¸¦ Á¶È¸ÇÕ´Ï´Ù.");
			
			try
			{
				agencyModel.Init();
				// µ¥ÀÌÅÍ¸ðµ¨¿¡ Àü¼ÛÇÒ ³»¿ëÀ» ¼ÂÆ®ÇÑ´Ù.
				if(IsNewSearchKey)
				{
					agencyModel.SearchKey = "";
				}
				else
				{
					agencyModel.SearchKey  = ebSearchKey.Text;
				}

				agencyModel.SearchAgencyType = cbSearchAgencyType.SelectedItem.Value.ToString();
				agencyModel.SearchRap        = cbSearchRap.SelectedValue.ToString();

                if (uiCheckBox2.Checked)
				{
					agencyModel.SearchchkAdState_10   = "Y";
				}
				else
				{
					agencyModel.SearchchkAdState_10   = "N";
				}

				// ´ëÇà»ç¸ñ·ÏÁ¶È¸ ¼­ºñ½º¸¦ È£ÃâÇÑ´Ù.
				new AgencyManager(systemModel,commonModel).GetAgencyList(agencyModel);

				if (agencyModel.ResultCD.Equals("0000"))
				{
					Utility.SetDataTable(agencyDs.Agency, agencyModel.AgencyDataSet);		
					StatusMessage(agencyModel.ResultCnt + "°ÇÀÇ ´ëÇà»ç Á¤º¸°¡ Á¶È¸µÇ¾ú½À´Ï´Ù.");
					if(canUpdate)
					{
						AddSchChoice();									
					}					
					SetDetailText();
				}
			}
			catch(FrameException fe)
			{
				FrameSystem.showMsgForm("´ëÇà»çÁ¶È¸¿À·ù", new string[] {fe.ErrCode, fe.ResultMsg});
			}
			catch(Exception ex)
			{
				FrameSystem.showMsgForm("´ëÇà»çÁ¶È¸¿À·ù",new string[] {"",ex.Message});
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
				if ( agencyDs.Tables["Agency"].Rows.Count < 1 ) return;
              
				foreach (DataRow row in agencyDs.Tables["Agency"].Rows)
				{					
					if(IsAdding)
					{
						cm.Position = 0;
						strAgencyCode = null;						
					}
					else
					{						
						if(row["AgencyCode"].ToString().Equals(strAgencyCode))
						{					
							cm.Position = rowIndex;
							break;								
						}
					}

					rowIndex++;
					grdExAgencyList.EnsureVisible();
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
		/// ´ëÇà»ç»ó¼¼Á¤º¸ ÀúÀå
		/// </summary>
		private void SaveAgencyDetail()
		{
			StatusMessage("´ëÇà»ç Á¤º¸¸¦ ÀúÀåÇÕ´Ï´Ù.");

			if(ebAgencyName.Text.Trim().Length == 0) 
			{
				MessageBox.Show("´ëÇà»ç¸íÀÌ ÀÔ·ÂµÇÁö ¾Ê¾Ò½À´Ï´Ù.","´ëÇà»ç ÀúÀå", 
					MessageBoxButtons.OK, MessageBoxIcon.Information );
                ebAgencyName.Focus();
				return;							
			}

			try
			{

				// µ¥ÀÌÅÍ¸ðµ¨¿¡ Àü¼ÛÇÒ ³»¿ëÀ» ¼ÂÆ®ÇÑ´Ù.
				agencyModel.AgencyCode       = strAgencyCode;
				agencyModel.AgencyName       = ebAgencyName.Text.Trim();
				agencyModel.RapCode			 = keyRepCode;
				agencyModel.AgencyType       = cbAgencyType.SelectedValue.ToString(); 
				agencyModel.Address         = ebAddress.Text.Trim();
				agencyModel.Tell            = ebTell.Text.Trim();
				agencyModel.Comment         = ebComment.Text.Trim();

				if(rbUseYn_Y.Checked)
				{
					agencyModel.UseYn       = "Y";
				}
				else
				{
					agencyModel.UseYn       = "N";
				}

				// ´ëÇà»ç »ó¼¼Á¤º¸ ÀúÀå ¼­ºñ½º¸¦ È£ÃâÇÑ´Ù.
				if (IsAdding)
				{
					new AgencyManager(systemModel,commonModel).SetAgencyAdd(agencyModel);
				    StatusMessage("´ëÇà»ç Á¤º¸°¡ Ãß°¡µÇ¾ú½À´Ï´Ù.");
                    IsAdding = false;
                    ResetDetailText();
                }
				else
				{
					new AgencyManager(systemModel,commonModel).SetAgencyUpdate(agencyModel);
				    StatusMessage("´ëÇà»ç Á¤º¸°¡ ÀúÀåµÇ¾ú½À´Ï´Ù.");
                }
                    
                DisableButton();
				SearchAgencys();
                InitButton();
					
			}
			catch(FrameException fe)
			{
				FrameSystem.showMsgForm("´ëÇà»çÁ¤º¸ ÀúÀå¿À·ù", new string[] {fe.ErrCode, fe.ResultMsg});
			}
			catch(Exception ex)
			{
				FrameSystem.showMsgForm("´ëÇà»çÁ¤º¸ ÀúÀå¿À·ù",new string[] {"",ex.Message});
			}			
		}

		/// <summary>
		/// »ç¿ëÀÚÁ¤º¸ »èÁ¦
		/// </summary>
		private void DeleteAgency()
		{
			StatusMessage("´ëÇà»ç Á¤º¸¸¦ »èÁ¦ÇÕ´Ï´Ù.");

			if(ebAgencyName.Text.Trim().Length == 0) 
			{
				MessageBox.Show("»èÁ¦ÇÒ ´ëÇà»ç Á¤º¸°¡ ¾ø½À´Ï´Ù.","´ëÇà»ç »èÁ¦", 
					MessageBoxButtons.OK, MessageBoxIcon.Information );
				return;
			}

			DialogResult result = MessageBox.Show("ÇØ´ç ´ëÇà»ç Á¤º¸¸¦ »èÁ¦ ÇÏ½Ã°Ú½À´Ï±î?","´ëÇà»ç »èÁ¦",
				MessageBoxButtons.YesNo, MessageBoxIcon.Question,
				MessageBoxDefaultButton.Button2);

			if (result == DialogResult.No) return;

			try
			{
				// µ¥ÀÌÅÍ¸ðµ¨¿¡ Àü¼ÛÇÒ ³»¿ëÀ» ¼ÂÆ®ÇÑ´Ù.
				agencyModel.AgencyCode       = strAgencyCode;

				// ´ëÇà»ç »ó¼¼Á¤º¸ ÀúÀå ¼­ºñ½º¸¦ È£ÃâÇÑ´Ù.
				new AgencyManager(systemModel,commonModel).SetAgencyDelete(agencyModel);
                
                StatusMessage("´ëÇà»ç Á¤º¸°¡ »èÁ¦µÇ¾ú½À´Ï´Ù.");			
				ResetDetailText();
                
                DisableButton();
				SearchAgencys();
                InitButton();
					
			}
			catch(FrameException fe)
			{
				FrameSystem.showMsgForm("´ëÇà»çÁ¤º¸ »èÁ¦¿À·ù", new string[] {fe.ErrCode, fe.ResultMsg});
			}
			catch(Exception ex)
			{
				FrameSystem.showMsgForm("´ëÇà»çÁ¤º¸ »èÁ¦¿À·ù",new string[] {"",ex.Message});
			}			

		}
		
		/// <summary>
		/// »ç¿ëÀÚ »ó¼¼Á¤º¸ÀÇ ¼ÂÆ®
		/// </summary>
		private void SetDetailText()
		{
			int curRow = cm.Position;
			if(curRow >= 0)
			{
				strAgencyCode              = dt.Rows[curRow]["AgencyCode"].ToString();
				ebAgencyName.Text          = dt.Rows[curRow]["AgencyName"].ToString();
				keyRepCode				   = dt.Rows[cm.Position]["RapCode"].ToString();
				ebRapName.Text			   = dt.Rows[cm.Position]["RapName"].ToString();
				cbAgencyType.SelectedValue = dt.Rows[curRow]["AgencyType"].ToString();
				ebAddress.Text            = dt.Rows[curRow]["Address"].ToString();
				ebTell.Text               = dt.Rows[curRow]["Tell"].ToString();
				ebComment.Text            = dt.Rows[curRow]["Comment"].ToString();
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

				if(keyRepCode.Equals("0"))
				{
                    uiCheckBox1.Checked = true;
				}
				else
				{
                    uiCheckBox1.Checked = false;
				}
				if(canUpdate) 
				{
					btnSave.Enabled   = true;
					ResetTextReadonly();
				}
				else
				{
					btnSave.Enabled   = false;
					SetTextReadonly();           
				}
				ebRegDt.Text             = dt.Rows[curRow]["RegDt"].ToString();
				ebModDt.Text             = dt.Rows[curRow]["ModDt"].ToString();
				ebRegName.Text           = dt.Rows[curRow]["RegName"].ToString();

				IsAdding = false;
			}
			StatusMessage("ÁØºñ");
		}

		private void ResetDetailText()
		{
			strAgencyCode				= "";
			ebAgencyName.Text			= "";
			cbAgencyType.SelectedIndex	= 0;
			ebAddress.Text				= "";
			ebTell.Text					= "";
			ebComment.Text				= "";
			rbUseYn_Y.Checked			= true;
			rbUseYn_N.Checked			= false;
			ebRegDt.Text				= "";
			ebModDt.Text				= "";
			ebRegName.Text				= "";

			if(commonModel.UserLevel=="30")
			{			
				ebRapName.Text = commonModel.RapName;
				keyRepCode     = commonModel.RapCode;
			}
			else
			{
				ebRapName.Text = "°ø¿ë";
				keyRepCode     = "0";
			}
		}
		
		/// <summary>
		/// »ó¼¼Á¤º¸ ReadOnly
		/// </summary>
		private void SetTextReadonly()
		{
			ebAgencyName.ReadOnly        = true;
			cbAgencyType.ReadOnly		= true;
			ebAddress.ReadOnly			= true;
			ebTell.ReadOnly				= true;
			ebComment.ReadOnly			= true;
			rbUseYn_Y.Enabled			= false;
			rbUseYn_N.Enabled			= false;
			btnRapSearch.Enabled     = false;															

			ebAgencyName.BackColor       = Color.WhiteSmoke;
			cbAgencyType.BackColor		= Color.WhiteSmoke;
			ebAddress.BackColor			= Color.WhiteSmoke;
			ebTell.BackColor			= Color.WhiteSmoke;
			ebComment.BackColor			= Color.WhiteSmoke;

		}

		/// <summary>
		/// »ó¼¼Á¤º¸ ¼öÁ¤°¡´ÉÄÉ
		/// </summary>
		private void ResetTextReadonly()
		{
			ebAgencyName.ReadOnly        = false;
			cbAgencyType.ReadOnly		= false;
			ebAddress.ReadOnly			= false;
			ebTell.ReadOnly				= false;
			ebComment.ReadOnly			= false;
			rbUseYn_Y.Enabled			= true;
			rbUseYn_N.Enabled			= true;

			ebAgencyName.BackColor       = Color.White;
			cbAgencyType.BackColor		= Color.White;
			ebAddress.BackColor			= Color.White;
			ebTell.BackColor			= Color.White;
			ebComment.BackColor			= Color.White;

			if(commonModel.UserLevel=="30")
			{			
				btnRapSearch.Enabled     = false;	
			}
			else
			{
                if (uiCheckBox1.Checked)
				{
					btnRapSearch.Enabled     = false;	
				}
				else
				{
					btnRapSearch.Enabled     = true;
				}
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

		#region ÆË¾÷À» À§ÇÑ ¸Þ¼Òµå

		public void SetMediaRep(string RepCode, string RepName)
		{	
			keyRepCode      = RepCode;
			ebRapName.Text  = RepName;		}


		#endregion

		private void chkAllRep_CheckedChanged(object sender, System.EventArgs e)
		{
            if (uiCheckBox1.Checked)
			{
				ebRapName.Text = "°ø¿ë";
				keyRepCode     = "0";
				btnRapSearch.Enabled     = false;	
			}
			else
			{
				if(commonModel.UserLevel=="30")
				{
					ebRapName.Text = commonModel.RapName;
					keyRepCode     = commonModel.RapCode;
					btnRapSearch.Enabled     = false;
				}
				else
				{
					btnRapSearch.Enabled     = true;
				}
			}		
		}

		private void btnRapSearch_Click(object sender, System.EventArgs e)
		{
			Agency_RepSearchForm rapForm = new Agency_RepSearchForm(this);
			rapForm.ShowDialog();            
			rapForm.Dispose();
			rapForm = null;		
		}
	}
}
