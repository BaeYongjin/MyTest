// ===============================================================================
// MediaRapControl for Charites Project
//
// MediaRapControl.cs
//
// ¹Ìµð¾î·¾Á¤º¸°ü¸® ÄÁµå·ÑÀ» Á¤ÀÇÇÕ´Ï´Ù. 
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
	/// ¹Ìµð¾î·¾°ü¸® ÄÁÆ®·Ñ
	/// </summary>
	public class MediaRapControl : System.Windows.Forms.UserControl, IUserControl
	{
		#region ÀÌº¥Æ®ÇÚµé·¯
		public event StatusEventHandler 			StatusEvent;			// »óÅÂÀÌº¥Æ® ÇÚµé·¯
		public event ProgressEventHandler 			ProgressEvent;			// Ã³¸®ÁßÀÌº¥Æ® ÇÚµé·¯
		#endregion
			
		#region ¹Ìµð¾î·¾Á¤ÀÇ °´Ã¼ ¹× º¯¼ö

		// ½Ã½ºÅÛ Á¤º¸ : È­¸é°øÅë
		private SystemModel   systemModel   = FrameSystem.oSysModel;
		private CommonModel   commonModel   = FrameSystem.oComModel;
		private Logger        log           = FrameSystem.oLog;
		private MenuPower     menu          = FrameSystem.oMenu;

		// ¸Þ´ºÄÚµå : º¸¾ÈÀÌ ÇÊ¿äÇÑ È­¸é¿¡ ÇÊ¿äÇÔ
		public string        menuCode		= "";

		// »ç¿ëÇÒ Á¤º¸¸ðµ¨
		MediaRapModel mediarapModel  = new MediaRapModel();	// ¹Ìµð¾î·¾Á¤º¸¸ðµ¨

		// È­¸éÃ³¸®¿ë º¯¼ö
		bool IsNewSearchKey		  = true;					// °Ë»ö¾îÀÔ·Â ¿©ºÎ
		CurrencyManager cm        = null;					// µ¥ÀÌÅÍ ±×¸®µåÀÇ º¯°æ¿¡ µû¸¥ µ¥ÀÌÅÍ¼Â °ü¸®¸¦ À§ÇÏ¿©			
		DataTable       dt        = null;

		bool IsAdding             = false;
		bool canRead			  = false;
		bool canUpdate			  = false;
		bool canCreate            = false;
		bool canDelete            = false;
        private Janus.Windows.EditControls.UICheckBox uiCheckBox1;

		private string        rapCode = null;

        // Á¶È¸Áß Ã³¸® : Á¶È¸Áß »ó¼¼È­¸éÀÌ ¾÷µ¥ÀÌÆ® µÇ´Â °ÍÀ» ¹æÁö ÇÏ±âÀ§ÇÔ 2011.11.28 RH.Jung
        bool IsSearching = false;

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
		private Janus.Windows.UI.Dock.UIPanel uiPanelMediaRapList;
		private Janus.Windows.UI.Dock.UIPanelInnerContainer uiPanelMediaRapListContainer;
		private Janus.Windows.UI.Dock.UIPanel uiPanelMediaRapDetail;
		private Janus.Windows.UI.Dock.UIPanelInnerContainer uiPanelMediaRapDetailContainer;
		private Janus.Windows.UI.Dock.UIPanel uiPanelMediaRapsSearch;
		private Janus.Windows.UI.Dock.UIPanelInnerContainer uiPanelMediaRapsSearchContainer;
		private Janus.Windows.EditControls.UIButton uiButton2;
		private Janus.Windows.EditControls.UIButton uiButton1;
		private Janus.Windows.UI.Dock.UIPanelGroup uiPanelMediaRaps;
		private System.Windows.Forms.Panel pnlSearch;
		private Janus.Windows.GridEX.EditControls.EditBox ebSearchKey;
		private Janus.Windows.EditControls.UIButton btnSearch;
		private System.Windows.Forms.Panel pnlMediaRapDetail;
		private System.Windows.Forms.Label lbRegDt;
		private Janus.Windows.EditControls.UIButton btnAdd;
		private Janus.Windows.EditControls.UIButton btnSave;
		private System.Data.DataView dvMediaRap;
		private Janus.Windows.GridEX.EditControls.EditBox ebRegDt;
		private Janus.Windows.EditControls.UIButton btnDelete;		
		private System.Windows.Forms.Label lbModDt;
		private Janus.Windows.GridEX.EditControls.EditBox ebModDt;
		private Janus.Windows.EditControls.UIComboBox cbSearchMediaRap;
		private Janus.Windows.GridEX.EditControls.EditBox ebMediaRapName;
		private System.Windows.Forms.Label lbMediaRapName;
		private System.Windows.Forms.Label lbMediaRapTell;
		private Janus.Windows.GridEX.EditControls.EditBox ebMediaRapTell;
		private System.Windows.Forms.Label lbMediaRapType;
		private Janus.Windows.EditControls.UIComboBox cbMediaRapType;
		private Janus.Windows.GridEX.EditControls.EditBox ebMediaRapComment;
		private System.Windows.Forms.Label lbMediaRapComment;
		private AdManagerClient.MediaRapDs mediaRapDs;
		private Janus.Windows.GridEX.EditControls.EditBox ebRegID;
		private System.Windows.Forms.Label lbRegID;
		private Janus.Windows.GridEX.GridEX grdExMediaRapList;
		private System.Windows.Forms.Label lbUseYn;
		private Janus.Windows.EditControls.UIRadioButton rbUseYn_N;
        private Janus.Windows.EditControls.UIRadioButton rbUseYn_Y;
		private System.ComponentModel.IContainer components;

		public MediaRapControl()
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
			Janus.Windows.GridEX.GridEXLayout grdExMediaRapList_DesignTimeLayout = new Janus.Windows.GridEX.GridEXLayout();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MediaRapControl));
			this.uiPM = new Janus.Windows.UI.Dock.UIPanelManager(this.components);
			this.uiPanelMediaRaps = new Janus.Windows.UI.Dock.UIPanelGroup();
			this.uiPanelMediaRapsSearch = new Janus.Windows.UI.Dock.UIPanel();
			this.uiPanelMediaRapsSearchContainer = new Janus.Windows.UI.Dock.UIPanelInnerContainer();
			this.pnlSearch = new System.Windows.Forms.Panel();
			this.uiCheckBox1 = new Janus.Windows.EditControls.UICheckBox();
			this.cbSearchMediaRap = new Janus.Windows.EditControls.UIComboBox();
			this.ebSearchKey = new Janus.Windows.GridEX.EditControls.EditBox();
			this.btnSearch = new Janus.Windows.EditControls.UIButton();
			this.uiPanelMediaRapList = new Janus.Windows.UI.Dock.UIPanel();
			this.uiPanelMediaRapListContainer = new Janus.Windows.UI.Dock.UIPanelInnerContainer();
			this.grdExMediaRapList = new Janus.Windows.GridEX.GridEX();
			this.dvMediaRap = new System.Data.DataView();
			this.mediaRapDs = new AdManagerClient.MediaRapDs();
			this.uiPanelMediaRapDetail = new Janus.Windows.UI.Dock.UIPanel();
			this.uiPanelMediaRapDetailContainer = new Janus.Windows.UI.Dock.UIPanelInnerContainer();
			this.pnlMediaRapDetail = new System.Windows.Forms.Panel();
			this.ebRegDt = new Janus.Windows.GridEX.EditControls.EditBox();
			this.ebModDt = new Janus.Windows.GridEX.EditControls.EditBox();
			this.lbModDt = new System.Windows.Forms.Label();
			this.ebMediaRapComment = new Janus.Windows.GridEX.EditControls.EditBox();
			this.lbMediaRapTell = new System.Windows.Forms.Label();
			this.ebMediaRapTell = new Janus.Windows.GridEX.EditControls.EditBox();
			this.cbMediaRapType = new Janus.Windows.EditControls.UIComboBox();
			this.lbMediaRapType = new System.Windows.Forms.Label();
			this.ebRegID = new Janus.Windows.GridEX.EditControls.EditBox();
			this.lbRegID = new System.Windows.Forms.Label();
			this.ebMediaRapName = new Janus.Windows.GridEX.EditControls.EditBox();
			this.lbMediaRapName = new System.Windows.Forms.Label();
			this.lbRegDt = new System.Windows.Forms.Label();
			this.lbMediaRapComment = new System.Windows.Forms.Label();
			this.btnDelete = new Janus.Windows.EditControls.UIButton();
			this.btnAdd = new Janus.Windows.EditControls.UIButton();
			this.btnSave = new Janus.Windows.EditControls.UIButton();
			this.lbUseYn = new System.Windows.Forms.Label();
			this.rbUseYn_N = new Janus.Windows.EditControls.UIRadioButton();
			this.rbUseYn_Y = new Janus.Windows.EditControls.UIRadioButton();
			this.uiButton1 = new Janus.Windows.EditControls.UIButton();
			this.uiButton2 = new Janus.Windows.EditControls.UIButton();
			((System.ComponentModel.ISupportInitialize)(this.uiPM)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.uiPanelMediaRaps)).BeginInit();
			this.uiPanelMediaRaps.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.uiPanelMediaRapsSearch)).BeginInit();
			this.uiPanelMediaRapsSearch.SuspendLayout();
			this.uiPanelMediaRapsSearchContainer.SuspendLayout();
			this.pnlSearch.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.uiPanelMediaRapList)).BeginInit();
			this.uiPanelMediaRapList.SuspendLayout();
			this.uiPanelMediaRapListContainer.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.grdExMediaRapList)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.dvMediaRap)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.mediaRapDs)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.uiPanelMediaRapDetail)).BeginInit();
			this.uiPanelMediaRapDetail.SuspendLayout();
			this.uiPanelMediaRapDetailContainer.SuspendLayout();
			this.pnlMediaRapDetail.SuspendLayout();
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
			this.uiPanelMediaRaps.Id = new System.Guid("b162c53a-7940-47cd-a0ed-9762c59c50d8");
			this.uiPanelMediaRaps.StaticGroup = true;
			this.uiPanelMediaRapsSearch.Id = new System.Guid("afb3b7d0-aa79-443c-a1e8-3baba38f7a4d");
			this.uiPanelMediaRaps.Panels.Add(this.uiPanelMediaRapsSearch);
			this.uiPanelMediaRapList.Id = new System.Guid("8e3140b9-bfcb-43af-b84e-2ee6cddf51dc");
			this.uiPanelMediaRaps.Panels.Add(this.uiPanelMediaRapList);
			this.uiPanelMediaRapDetail.Id = new System.Guid("b2973a57-d3e1-4d80-9387-7a6dc3332542");
			this.uiPanelMediaRaps.Panels.Add(this.uiPanelMediaRapDetail);
			this.uiPM.Panels.Add(this.uiPanelMediaRaps);
			// 
			// Design Time Panel Info:
			// 
			this.uiPM.BeginPanelInfo();
			this.uiPM.AddDockPanelInfo(new System.Guid("b162c53a-7940-47cd-a0ed-9762c59c50d8"), Janus.Windows.UI.Dock.PanelGroupStyle.HorizontalTiles, Janus.Windows.UI.Dock.PanelDockStyle.Fill, true, new System.Drawing.Size(1010, 677), true);
			this.uiPM.AddDockPanelInfo(new System.Guid("afb3b7d0-aa79-443c-a1e8-3baba38f7a4d"), new System.Guid("b162c53a-7940-47cd-a0ed-9762c59c50d8"), 42, true);
			this.uiPM.AddDockPanelInfo(new System.Guid("8e3140b9-bfcb-43af-b84e-2ee6cddf51dc"), new System.Guid("b162c53a-7940-47cd-a0ed-9762c59c50d8"), 415, true);
			this.uiPM.AddDockPanelInfo(new System.Guid("b2973a57-d3e1-4d80-9387-7a6dc3332542"), new System.Guid("b162c53a-7940-47cd-a0ed-9762c59c50d8"), 170, true);
			this.uiPM.AddFloatingPanelInfo(new System.Guid("b162c53a-7940-47cd-a0ed-9762c59c50d8"), Janus.Windows.UI.Dock.PanelGroupStyle.HorizontalTiles, true, new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
			this.uiPM.AddFloatingPanelInfo(new System.Guid("8e3140b9-bfcb-43af-b84e-2ee6cddf51dc"), new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
			this.uiPM.AddFloatingPanelInfo(new System.Guid("b2973a57-d3e1-4d80-9387-7a6dc3332542"), new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
			this.uiPM.AddFloatingPanelInfo(new System.Guid("afb3b7d0-aa79-443c-a1e8-3baba38f7a4d"), new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
			this.uiPM.EndPanelInfo();
			// 
			// uiPanelMediaRaps
			// 
			this.uiPanelMediaRaps.ActiveCaptionMode = Janus.Windows.UI.Dock.ActiveCaptionMode.Never;
			this.uiPanelMediaRaps.BackColor = System.Drawing.SystemColors.Window;
			this.uiPanelMediaRaps.CaptionVisible = Janus.Windows.UI.InheritableBoolean.True;
			this.uiPanelMediaRaps.CloseButtonVisible = Janus.Windows.UI.InheritableBoolean.False;
			this.uiPanelMediaRaps.Location = new System.Drawing.Point(0, 0);
			this.uiPanelMediaRaps.Name = "uiPanelMediaRaps";
			this.uiPanelMediaRaps.Size = new System.Drawing.Size(1010, 677);
			this.uiPanelMediaRaps.TabIndex = 4;
			this.uiPanelMediaRaps.TabStop = false;
			this.uiPanelMediaRaps.Text = "¹Ìµð¾î·¾°ü¸®";
			// 
			// uiPanelMediaRapsSearch
			// 
			this.uiPanelMediaRapsSearch.AllowResize = Janus.Windows.UI.InheritableBoolean.False;
			this.uiPanelMediaRapsSearch.CaptionVisible = Janus.Windows.UI.InheritableBoolean.False;
			this.uiPanelMediaRapsSearch.CloseButtonVisible = Janus.Windows.UI.InheritableBoolean.False;
			this.uiPanelMediaRapsSearch.InnerContainer = this.uiPanelMediaRapsSearchContainer;
			this.uiPanelMediaRapsSearch.Location = new System.Drawing.Point(0, 22);
			this.uiPanelMediaRapsSearch.Name = "uiPanelMediaRapsSearch";
			this.uiPanelMediaRapsSearch.Size = new System.Drawing.Size(1010, 43);
			this.uiPanelMediaRapsSearch.TabIndex = 0;
			this.uiPanelMediaRapsSearch.Text = "°Ë»ö";
			// 
			// uiPanelMediaRapsSearchContainer
			// 
			this.uiPanelMediaRapsSearchContainer.Controls.Add(this.pnlSearch);
			this.uiPanelMediaRapsSearchContainer.Location = new System.Drawing.Point(1, 1);
			this.uiPanelMediaRapsSearchContainer.Name = "uiPanelMediaRapsSearchContainer";
			this.uiPanelMediaRapsSearchContainer.Size = new System.Drawing.Size(1008, 41);
			this.uiPanelMediaRapsSearchContainer.TabIndex = 0;
			// 
			// pnlSearch
			// 
			this.pnlSearch.BackColor = System.Drawing.SystemColors.Window;
			this.pnlSearch.Controls.Add(this.uiCheckBox1);
			this.pnlSearch.Controls.Add(this.cbSearchMediaRap);
			this.pnlSearch.Controls.Add(this.ebSearchKey);
			this.pnlSearch.Controls.Add(this.btnSearch);
			this.pnlSearch.Dock = System.Windows.Forms.DockStyle.Fill;
			this.pnlSearch.Location = new System.Drawing.Point(0, 0);
			this.pnlSearch.Name = "pnlSearch";
			this.pnlSearch.Size = new System.Drawing.Size(1008, 41);
			this.pnlSearch.TabIndex = 0;
			// 
			// uiCheckBox1
			// 
			this.uiCheckBox1.Location = new System.Drawing.Point(366, 10);
			this.uiCheckBox1.Name = "uiCheckBox1";
			this.uiCheckBox1.Size = new System.Drawing.Size(104, 23);
			this.uiCheckBox1.TabIndex = 5;
			this.uiCheckBox1.Text = "»ç¿ë¾ÈÇÔ Æ÷ÇÔ";
			// 
			// cbSearchMediaRap
			// 
			this.cbSearchMediaRap.ComboStyle = Janus.Windows.EditControls.ComboStyle.DropDownList;
			this.cbSearchMediaRap.Location = new System.Drawing.Point(16, 11);
			this.cbSearchMediaRap.Name = "cbSearchMediaRap";
			this.cbSearchMediaRap.Size = new System.Drawing.Size(128, 21);
			this.cbSearchMediaRap.TabIndex = 1;
			this.cbSearchMediaRap.Text = "·¾±¸ºÐ¼±ÅÃ";
			this.cbSearchMediaRap.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
			// 
			// ebSearchKey
			// 
			this.ebSearchKey.Location = new System.Drawing.Point(152, 11);
			this.ebSearchKey.Name = "ebSearchKey";
			this.ebSearchKey.Size = new System.Drawing.Size(208, 21);
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
			this.btnSearch.Location = new System.Drawing.Point(887, 8);
			this.btnSearch.Name = "btnSearch";
			this.btnSearch.Size = new System.Drawing.Size(104, 24);
			this.btnSearch.TabIndex = 4;
			this.btnSearch.Text = "Á¶ È¸";
			this.btnSearch.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
			this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
			// 
			// uiPanelMediaRapList
			// 
			this.uiPanelMediaRapList.BackColor = System.Drawing.SystemColors.Window;
			this.uiPanelMediaRapList.CaptionStyle = Janus.Windows.UI.Dock.PanelCaptionStyle.Dark;
			this.uiPanelMediaRapList.InnerContainer = this.uiPanelMediaRapListContainer;
			this.uiPanelMediaRapList.Location = new System.Drawing.Point(0, 69);
			this.uiPanelMediaRapList.MinimumSize = new System.Drawing.Size(-1, 100);
			this.uiPanelMediaRapList.Name = "uiPanelMediaRapList";
			this.uiPanelMediaRapList.Size = new System.Drawing.Size(1010, 429);
			this.uiPanelMediaRapList.TabIndex = 1;
			this.uiPanelMediaRapList.TabStop = false;
			this.uiPanelMediaRapList.Text = "¹Ìµð¾î·¾¸ñ·Ï";
			// 
			// uiPanelMediaRapListContainer
			// 
			this.uiPanelMediaRapListContainer.BackColor = System.Drawing.SystemColors.Window;
			this.uiPanelMediaRapListContainer.Controls.Add(this.grdExMediaRapList);
			this.uiPanelMediaRapListContainer.Location = new System.Drawing.Point(1, 23);
			this.uiPanelMediaRapListContainer.Name = "uiPanelMediaRapListContainer";
			this.uiPanelMediaRapListContainer.Size = new System.Drawing.Size(1008, 405);
			this.uiPanelMediaRapListContainer.TabIndex = 0;
			// 
			// grdExMediaRapList
			// 
			this.grdExMediaRapList.AllowEdit = Janus.Windows.GridEX.InheritableBoolean.False;
			this.grdExMediaRapList.AlternatingColors = true;
			this.grdExMediaRapList.AlternatingRowFormatStyle.BackgroundImageDrawMode = Janus.Windows.GridEX.BackgroundImageDrawMode.Tile;
			this.grdExMediaRapList.BorderStyle = Janus.Windows.GridEX.BorderStyle.None;
			this.grdExMediaRapList.DataSource = this.dvMediaRap;
			grdExMediaRapList_DesignTimeLayout.LayoutString = resources.GetString("grdExMediaRapList_DesignTimeLayout.LayoutString");
			this.grdExMediaRapList.DesignTimeLayout = grdExMediaRapList_DesignTimeLayout;
			this.grdExMediaRapList.Dock = System.Windows.Forms.DockStyle.Fill;
			this.grdExMediaRapList.EditorsControlStyle.ButtonAppearance = Janus.Windows.GridEX.ButtonAppearance.Regular;
			this.grdExMediaRapList.EmptyRows = true;
			this.grdExMediaRapList.FocusCellFormatStyle.BackColor = System.Drawing.SystemColors.Highlight;
			this.grdExMediaRapList.FocusCellFormatStyle.ForeColor = System.Drawing.Color.White;
			this.grdExMediaRapList.FocusStyle = Janus.Windows.GridEX.FocusStyle.None;
			this.grdExMediaRapList.FrozenColumns = 2;
			this.grdExMediaRapList.GridLineColor = System.Drawing.Color.Silver;
			this.grdExMediaRapList.GridLines = Janus.Windows.GridEX.GridLines.Vertical;
			this.grdExMediaRapList.GridLineStyle = Janus.Windows.GridEX.GridLineStyle.Solid;
			this.grdExMediaRapList.GroupByBoxVisible = false;
			this.grdExMediaRapList.HeaderFormatStyle.TextAlignment = Janus.Windows.GridEX.TextAlignment.Center;
			this.grdExMediaRapList.HideSelection = Janus.Windows.GridEX.HideSelection.HighlightInactive;
			this.grdExMediaRapList.Location = new System.Drawing.Point(0, 0);
			this.grdExMediaRapList.Name = "grdExMediaRapList";
			this.grdExMediaRapList.ScrollBars = Janus.Windows.GridEX.ScrollBars.Vertical;
			this.grdExMediaRapList.SelectedInactiveFormatStyle.BackColor = System.Drawing.Color.Gold;
			this.grdExMediaRapList.Size = new System.Drawing.Size(1008, 405);
			this.grdExMediaRapList.TabIndex = 5;
			this.grdExMediaRapList.TabKeyBehavior = Janus.Windows.GridEX.TabKeyBehavior.ControlNavigation;
			this.grdExMediaRapList.ThemedAreas = ((Janus.Windows.GridEX.ThemedArea)(((((((((Janus.Windows.GridEX.ThemedArea.ScrollBars | Janus.Windows.GridEX.ThemedArea.EditControls) 
            | Janus.Windows.GridEX.ThemedArea.Headers) 
            | Janus.Windows.GridEX.ThemedArea.GroupByBox) 
            | Janus.Windows.GridEX.ThemedArea.GroupRows) 
            | Janus.Windows.GridEX.ThemedArea.ControlBorder) 
            | Janus.Windows.GridEX.ThemedArea.Cards) 
            | Janus.Windows.GridEX.ThemedArea.Gridlines) 
            | Janus.Windows.GridEX.ThemedArea.CheckBoxes)));
			this.grdExMediaRapList.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
			this.grdExMediaRapList.Enter += new System.EventHandler(this.OnGrdRowChanged);
			// 
			// dvMediaRap
			// 
			this.dvMediaRap.Table = this.mediaRapDs.MediaRaps;
			// 
			// mediaRapDs
			// 
			this.mediaRapDs.DataSetName = "MediaRapDs";
			this.mediaRapDs.Locale = new System.Globalization.CultureInfo("en-US");
			this.mediaRapDs.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
			// 
			// uiPanelMediaRapDetail
			// 
			this.uiPanelMediaRapDetail.AllowResize = Janus.Windows.UI.InheritableBoolean.False;
			this.uiPanelMediaRapDetail.BackColor = System.Drawing.SystemColors.Window;
			this.uiPanelMediaRapDetail.CaptionStyle = Janus.Windows.UI.Dock.PanelCaptionStyle.Dark;
			this.uiPanelMediaRapDetail.InnerContainer = this.uiPanelMediaRapDetailContainer;
			this.uiPanelMediaRapDetail.Location = new System.Drawing.Point(0, 502);
			this.uiPanelMediaRapDetail.Name = "uiPanelMediaRapDetail";
			this.uiPanelMediaRapDetail.Size = new System.Drawing.Size(1010, 175);
			this.uiPanelMediaRapDetail.TabIndex = 2;
			this.uiPanelMediaRapDetail.TabStop = false;
			this.uiPanelMediaRapDetail.Text = "»ó¼¼Á¤º¸";
			// 
			// uiPanelMediaRapDetailContainer
			// 
			this.uiPanelMediaRapDetailContainer.Controls.Add(this.pnlMediaRapDetail);
			this.uiPanelMediaRapDetailContainer.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.uiPanelMediaRapDetailContainer.Location = new System.Drawing.Point(1, 23);
			this.uiPanelMediaRapDetailContainer.Name = "uiPanelMediaRapDetailContainer";
			this.uiPanelMediaRapDetailContainer.Size = new System.Drawing.Size(1008, 151);
			this.uiPanelMediaRapDetailContainer.TabIndex = 0;
			// 
			// pnlMediaRapDetail
			// 
			this.pnlMediaRapDetail.BackColor = System.Drawing.SystemColors.Window;
			this.pnlMediaRapDetail.Controls.Add(this.ebRegDt);
			this.pnlMediaRapDetail.Controls.Add(this.ebModDt);
			this.pnlMediaRapDetail.Controls.Add(this.lbModDt);
			this.pnlMediaRapDetail.Controls.Add(this.ebMediaRapComment);
			this.pnlMediaRapDetail.Controls.Add(this.lbMediaRapTell);
			this.pnlMediaRapDetail.Controls.Add(this.ebMediaRapTell);
			this.pnlMediaRapDetail.Controls.Add(this.cbMediaRapType);
			this.pnlMediaRapDetail.Controls.Add(this.lbMediaRapType);
			this.pnlMediaRapDetail.Controls.Add(this.ebRegID);
			this.pnlMediaRapDetail.Controls.Add(this.lbRegID);
			this.pnlMediaRapDetail.Controls.Add(this.ebMediaRapName);
			this.pnlMediaRapDetail.Controls.Add(this.lbMediaRapName);
			this.pnlMediaRapDetail.Controls.Add(this.lbRegDt);
			this.pnlMediaRapDetail.Controls.Add(this.lbMediaRapComment);
			this.pnlMediaRapDetail.Controls.Add(this.btnDelete);
			this.pnlMediaRapDetail.Controls.Add(this.btnAdd);
			this.pnlMediaRapDetail.Controls.Add(this.btnSave);
			this.pnlMediaRapDetail.Controls.Add(this.lbUseYn);
			this.pnlMediaRapDetail.Controls.Add(this.rbUseYn_N);
			this.pnlMediaRapDetail.Controls.Add(this.rbUseYn_Y);
			this.pnlMediaRapDetail.Dock = System.Windows.Forms.DockStyle.Fill;
			this.pnlMediaRapDetail.Location = new System.Drawing.Point(0, 0);
			this.pnlMediaRapDetail.Name = "pnlMediaRapDetail";
			this.pnlMediaRapDetail.Size = new System.Drawing.Size(1008, 151);
			this.pnlMediaRapDetail.TabIndex = 0;
			// 
			// ebRegDt
			// 
			this.ebRegDt.BackColor = System.Drawing.Color.WhiteSmoke;
			this.ebRegDt.Location = new System.Drawing.Point(704, 8);
			this.ebRegDt.Name = "ebRegDt";
			this.ebRegDt.ReadOnly = true;
			this.ebRegDt.Size = new System.Drawing.Size(204, 21);
			this.ebRegDt.TabIndex = 0;
			this.ebRegDt.TabStop = false;
			this.ebRegDt.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
			this.ebRegDt.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
			// 
			// ebModDt
			// 
			this.ebModDt.BackColor = System.Drawing.Color.WhiteSmoke;
			this.ebModDt.Location = new System.Drawing.Point(704, 32);
			this.ebModDt.Name = "ebModDt";
			this.ebModDt.ReadOnly = true;
			this.ebModDt.Size = new System.Drawing.Size(204, 21);
			this.ebModDt.TabIndex = 0;
			this.ebModDt.TabStop = false;
			this.ebModDt.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
			this.ebModDt.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
			// 
			// lbModDt
			// 
			this.lbModDt.Location = new System.Drawing.Point(640, 32);
			this.lbModDt.Name = "lbModDt";
			this.lbModDt.Size = new System.Drawing.Size(72, 21);
			this.lbModDt.TabIndex = 2;
			this.lbModDt.Text = "¼öÁ¤ÀÏ½Ã";
			this.lbModDt.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// ebMediaRapComment
			// 
			this.ebMediaRapComment.Location = new System.Drawing.Point(88, 56);
			this.ebMediaRapComment.Multiline = true;
			this.ebMediaRapComment.Name = "ebMediaRapComment";
			this.ebMediaRapComment.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.ebMediaRapComment.Size = new System.Drawing.Size(496, 48);
			this.ebMediaRapComment.TabIndex = 10;
			this.ebMediaRapComment.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
			this.ebMediaRapComment.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
			// 
			// lbMediaRapTell
			// 
			this.lbMediaRapTell.Location = new System.Drawing.Point(8, 32);
			this.lbMediaRapTell.Name = "lbMediaRapTell";
			this.lbMediaRapTell.Size = new System.Drawing.Size(80, 21);
			this.lbMediaRapTell.TabIndex = 4;
			this.lbMediaRapTell.Text = "ÀüÈ­¹øÈ£";
			this.lbMediaRapTell.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// ebMediaRapTell
			// 
			this.ebMediaRapTell.Location = new System.Drawing.Point(88, 32);
			this.ebMediaRapTell.MaxLength = 15;
			this.ebMediaRapTell.Name = "ebMediaRapTell";
			this.ebMediaRapTell.Size = new System.Drawing.Size(248, 21);
			this.ebMediaRapTell.TabIndex = 8;
			this.ebMediaRapTell.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
			this.ebMediaRapTell.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
			// 
			// cbMediaRapType
			// 
			this.cbMediaRapType.ComboStyle = Janus.Windows.EditControls.ComboStyle.DropDownList;
			this.cbMediaRapType.DisplayMember = "CodeName";
			this.cbMediaRapType.Location = new System.Drawing.Point(448, 8);
			this.cbMediaRapType.Name = "cbMediaRapType";
			this.cbMediaRapType.Size = new System.Drawing.Size(136, 21);
			this.cbMediaRapType.TabIndex = 7;
			this.cbMediaRapType.ValueMember = "Code";
			this.cbMediaRapType.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
			// 
			// lbMediaRapType
			// 
			this.lbMediaRapType.Location = new System.Drawing.Point(360, 8);
			this.lbMediaRapType.Name = "lbMediaRapType";
			this.lbMediaRapType.Size = new System.Drawing.Size(80, 21);
			this.lbMediaRapType.TabIndex = 7;
			this.lbMediaRapType.Text = "¹Ìµð¾î·¾±¸ºÐ";
			this.lbMediaRapType.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// ebRegID
			// 
			this.ebRegID.BackColor = System.Drawing.Color.WhiteSmoke;
			this.ebRegID.Location = new System.Drawing.Point(704, 56);
			this.ebRegID.MaxLength = 10;
			this.ebRegID.Name = "ebRegID";
			this.ebRegID.ReadOnly = true;
			this.ebRegID.Size = new System.Drawing.Size(204, 21);
			this.ebRegID.TabIndex = 0;
			this.ebRegID.TabStop = false;
			this.ebRegID.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
			this.ebRegID.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
			// 
			// lbRegID
			// 
			this.lbRegID.Location = new System.Drawing.Point(640, 56);
			this.lbRegID.Name = "lbRegID";
			this.lbRegID.Size = new System.Drawing.Size(72, 21);
			this.lbRegID.TabIndex = 9;
			this.lbRegID.Text = "µî·ÏÀÚ";
			this.lbRegID.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// ebMediaRapName
			// 
			this.ebMediaRapName.Location = new System.Drawing.Point(88, 8);
			this.ebMediaRapName.MaxLength = 20;
			this.ebMediaRapName.Name = "ebMediaRapName";
			this.ebMediaRapName.Size = new System.Drawing.Size(248, 21);
			this.ebMediaRapName.TabIndex = 6;
			this.ebMediaRapName.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
			this.ebMediaRapName.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
			// 
			// lbMediaRapName
			// 
			this.lbMediaRapName.Location = new System.Drawing.Point(8, 8);
			this.lbMediaRapName.Name = "lbMediaRapName";
			this.lbMediaRapName.Size = new System.Drawing.Size(80, 21);
			this.lbMediaRapName.TabIndex = 11;
			this.lbMediaRapName.Text = "¹Ìµð¾î·¾¸í";
			this.lbMediaRapName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// lbRegDt
			// 
			this.lbRegDt.Location = new System.Drawing.Point(640, 8);
			this.lbRegDt.Name = "lbRegDt";
			this.lbRegDt.Size = new System.Drawing.Size(72, 21);
			this.lbRegDt.TabIndex = 12;
			this.lbRegDt.Text = "µî·ÏÀÏ½Ã";
			this.lbRegDt.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// lbMediaRapComment
			// 
			this.lbMediaRapComment.Location = new System.Drawing.Point(8, 56);
			this.lbMediaRapComment.Name = "lbMediaRapComment";
			this.lbMediaRapComment.Size = new System.Drawing.Size(80, 21);
			this.lbMediaRapComment.TabIndex = 13;
			this.lbMediaRapComment.Text = "ºñ°í";
			this.lbMediaRapComment.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// btnDelete
			// 
			this.btnDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.btnDelete.Enabled = false;
			this.btnDelete.Location = new System.Drawing.Point(120, 117);
			this.btnDelete.Name = "btnDelete";
			this.btnDelete.Size = new System.Drawing.Size(104, 24);
			this.btnDelete.TabIndex = 12;
			this.btnDelete.Text = "»è Á¦";
			this.btnDelete.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
			this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
			// 
			// btnAdd
			// 
			this.btnAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.btnAdd.Enabled = false;
			this.btnAdd.Location = new System.Drawing.Point(232, 117);
			this.btnAdd.Name = "btnAdd";
			this.btnAdd.Size = new System.Drawing.Size(104, 24);
			this.btnAdd.TabIndex = 13;
			this.btnAdd.Text = "Ãß °¡";
			this.btnAdd.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
			this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
			// 
			// btnSave
			// 
			this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.btnSave.Enabled = false;
			this.btnSave.Location = new System.Drawing.Point(8, 117);
			this.btnSave.Name = "btnSave";
			this.btnSave.Size = new System.Drawing.Size(104, 24);
			this.btnSave.TabIndex = 11;
			this.btnSave.Text = "Àú Àå";
			this.btnSave.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
			this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
			// 
			// lbUseYn
			// 
			this.lbUseYn.Location = new System.Drawing.Point(360, 32);
			this.lbUseYn.Name = "lbUseYn";
			this.lbUseYn.Size = new System.Drawing.Size(72, 21);
			this.lbUseYn.TabIndex = 40;
			this.lbUseYn.Text = "»ç¿ë¿©ºÎ";
			this.lbUseYn.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// rbUseYn_N
			// 
			this.rbUseYn_N.BackColor = System.Drawing.Color.Transparent;
			this.rbUseYn_N.Location = new System.Drawing.Point(512, 32);
			this.rbUseYn_N.Name = "rbUseYn_N";
			this.rbUseYn_N.Size = new System.Drawing.Size(72, 23);
			this.rbUseYn_N.TabIndex = 9;
			this.rbUseYn_N.Text = "»ç¿ë¾ÈÇÔ";
			this.rbUseYn_N.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
			// 
			// rbUseYn_Y
			// 
			this.rbUseYn_Y.BackColor = System.Drawing.Color.Transparent;
			this.rbUseYn_Y.Location = new System.Drawing.Point(432, 32);
			this.rbUseYn_Y.Name = "rbUseYn_Y";
			this.rbUseYn_Y.Size = new System.Drawing.Size(72, 23);
			this.rbUseYn_Y.TabIndex = 9;
			this.rbUseYn_Y.Text = "»ç¿ëÇÔ";
			this.rbUseYn_Y.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
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
			// MediaRapControl
			// 
			this.Controls.Add(this.uiPanelMediaRaps);
			this.Font = global::AdManagerClient.Properties.Settings.Default.Font1;
			this.Name = "MediaRapControl";
			this.Size = new System.Drawing.Size(1010, 677);
			this.Load += new System.EventHandler(this.MediaRapControl_Load);
			((System.ComponentModel.ISupportInitialize)(this.uiPM)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.uiPanelMediaRaps)).EndInit();
			this.uiPanelMediaRaps.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.uiPanelMediaRapsSearch)).EndInit();
			this.uiPanelMediaRapsSearch.ResumeLayout(false);
			this.uiPanelMediaRapsSearchContainer.ResumeLayout(false);
			this.pnlSearch.ResumeLayout(false);
			this.pnlSearch.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.uiPanelMediaRapList)).EndInit();
			this.uiPanelMediaRapList.ResumeLayout(false);
			this.uiPanelMediaRapListContainer.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.grdExMediaRapList)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.dvMediaRap)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.mediaRapDs)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.uiPanelMediaRapDetail)).EndInit();
			this.uiPanelMediaRapDetail.ResumeLayout(false);
			this.uiPanelMediaRapDetailContainer.ResumeLayout(false);
			this.pnlMediaRapDetail.ResumeLayout(false);
			this.pnlMediaRapDetail.PerformLayout();
			this.ResumeLayout(false);

		}
		#endregion

		#region ÄÁÆ®·Ñ ·Îµå
		private void MediaRapControl_Load(object sender, System.EventArgs e)
		{
			// µ¥ÀÌÅÍ°ü¸®¿ë °´Ã¼»ý¼º
			dt = ((DataView)grdExMediaRapList.DataSource).Table;  
			cm = (CurrencyManager) this.BindingContext[grdExMediaRapList.DataSource]; 
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
			InitCombo_Type();	

			// Á¶È¸±ÇÇÑ °Ë»ç
            
			if(menu.CanRead(MenuCode))
			{
				canRead = true;
				SearchMediaRap();
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
			// ÄÚµå¿¡¼­ º¸¾È·¹º§À» Á¶È¸ÇÑ´Ù.
			CodeModel codeModel = new CodeModel();
			codeModel.Section = "21";				// ÄÚµåºÐ·ù '11':º¸¾È·¹º§  TODO: ÄÚµåºÐ·ù´Â ÃßÈÄ XML·Î °ü¸®µÇ¾î¾ß...
			new CodeManager(systemModel, commonModel).GetCodeList(codeModel);
			
			if (codeModel.ResultCD.Equals("0000"))
			{
				// µ¥ÀÌÅÍ¼Â¿¡ ¼ÂÆÃ
				Utility.SetDataTable(mediaRapDs.Levels, codeModel.CodeDataSet);				
			}

			// »ó¼¼Á¶È¸ ÄÞº¸
			// »ó¼¼Á¤º¸ÀÇ ÄÞº¸´Â DatasetÀ» µ¥ÀÌÅÍ¼Ò½º·Î °¡Áø´Ù.

			// °Ë»öÁ¶°ÇÀÇ ÄÞº¸
			this.cbSearchMediaRap.Items.Clear();
			
			// ÄÞº¸¹Ú½º¿¡ ¼ÂÆ®ÇÒ ÄÚµå¸ñ·ÏÀ» ´ãÀ» Item¹è¿­À» ¼±¾ð
			Janus.Windows.EditControls.UIComboBoxItem[] comboItems = new Janus.Windows.EditControls.UIComboBoxItem[codeModel.ResultCnt + 1];

			comboItems[0] = new Janus.Windows.EditControls.UIComboBoxItem("·¦±¸ºÐ¼±ÅÃ","00");
			
			for(int i=0;i<codeModel.ResultCnt;i++)
			{
				DataRow row = mediaRapDs.Levels.Rows[i];

				string val = row["Code"].ToString();
				string txt = row["CodeName"].ToString();
				comboItems[i+1] = new Janus.Windows.EditControls.UIComboBoxItem(txt,val);
			}
			// ÄÞº¸¿¡ ¼ÂÆ®
			this.cbSearchMediaRap.Items.AddRange(comboItems);
			this.cbSearchMediaRap.SelectedIndex = 0;

			Application.DoEvents();
		}

		private void InitCombo_Type()
		{
			// ÄÚµå¿¡¼­ º¸¾È·¹º§À» Á¶È¸ÇÑ´Ù.
			CodeModel codeModel = new CodeModel();
			codeModel.Section = "21";				// ÄÚµåºÐ·ù '11':º¸¾È·¹º§  TODO: ÄÚµåºÐ·ù´Â ÃßÈÄ XML·Î °ü¸®µÇ¾î¾ß...
			new CodeManager(systemModel, commonModel).GetCodeList(codeModel);
			
			if (codeModel.ResultCD.Equals("0000"))
			{
				// µ¥ÀÌÅÍ¼Â¿¡ ¼ÂÆÃ
				Utility.SetDataTable(mediaRapDs.Levels, codeModel.CodeDataSet);				
			}

			// »ó¼¼Á¶È¸ ÄÞº¸
			// »ó¼¼Á¤º¸ÀÇ ÄÞº¸´Â DatasetÀ» µ¥ÀÌÅÍ¼Ò½º·Î °¡Áø´Ù.

			// °Ë»öÁ¶°ÇÀÇ ÄÞº¸
			this.cbMediaRapType.Items.Clear();
			
			// ÄÞº¸¹Ú½º¿¡ ¼ÂÆ®ÇÒ ÄÚµå¸ñ·ÏÀ» ´ãÀ» Item¹è¿­À» ¼±¾ð
			Janus.Windows.EditControls.UIComboBoxItem[] comboItems = new Janus.Windows.EditControls.UIComboBoxItem[codeModel.ResultCnt + 1];

			comboItems[0] = new Janus.Windows.EditControls.UIComboBoxItem("","00");
			
			for(int i=0;i<codeModel.ResultCnt;i++)
			{
				DataRow row = mediaRapDs.Levels.Rows[i];

				string val = row["Code"].ToString();
				string txt = row["CodeName"].ToString();
				comboItems[i+1] = new Janus.Windows.EditControls.UIComboBoxItem(txt,val);
			}
			// ÄÞº¸¿¡ ¼ÂÆ®
			this.cbMediaRapType.Items.AddRange(comboItems);
			this.cbMediaRapType.SelectedIndex = 0;

			Application.DoEvents();
		}

		private void InitButton()
		{
			if(canRead)   btnSearch.Enabled = true;
			if(canCreate) btnAdd.Enabled    = true;

			if(ebMediaRapName.Text.Trim().Length > 0) 
			{
				if(canDelete) btnDelete.Enabled = true;
				if(canUpdate) btnSave.Enabled   = true;
			}
			Application.DoEvents();
		}

		private void DisableButton()
		{
			btnSearch.Enabled = true;
			btnAdd.Enabled    = true;
			btnSave.Enabled   = true;
			btnDelete.Enabled = true;
			Application.DoEvents();
		}

		#endregion

		#region ¹Ìµð¾î·¾ ¾×¼ÇÃ³¸® ¸Þ¼Òµå

		/// <summary>
		/// ±×¸®µåÀÇ Rowº¯°æ½Ã
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void OnGrdRowChanged(object sender, System.EventArgs e) 
		{
            if (!IsSearching)    // 2011.11.28 RH.Jung Á¶È¸ÁßÀÌ ¾Æ´Ò°æ¿ì¿¡¸¸ µ¿ÀÛÇÏµµ·Ï º¯°æ
            {
                if (grdExMediaRapList.RecordCount > 0)
                {
                    SetMediaRapDetailText();
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
			SearchMediaRap();
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
			ResetMediaRapDetailText();

			ebMediaRapName.Focus();
		}

		/// <summary>
		/// ÀúÀå¹öÆ° Å¬¸¯
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnSave_Click(object sender, System.EventArgs e)
		{			
			SaveMediaRapDetail();		
		}

		/// <summary>
		/// »èÁ¦¹öÆ° Å¬¸¯
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnDelete_Click(object sender, System.EventArgs e)
		{			
			DeleteMediaRap();		
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
				SearchMediaRap();
			}
		}

		#endregion

		#region Ã³¸®¸Þ¼Òµå

		/// <summary>
		/// ¹Ìµð¾î·¾¸ñ·Ï Á¶È¸
		/// </summary>
		private void SearchMediaRap()
		{
            IsSearching = true;  // 2011.11.28 RH.Jung Á¶È¸Áß Ã³¸® 

			StatusMessage("¹Ìµð¾î·¾ Á¤º¸¸¦ Á¶È¸ÇÕ´Ï´Ù.");		

			try
			{
				mediarapModel.Init();
				// µ¥ÀÌÅÍ¸ðµ¨¿¡ Àü¼ÛÇÒ ³»¿ëÀ» ¼ÂÆ®ÇÑ´Ù.
				if(IsNewSearchKey)
				{
					mediarapModel.SearchKey = "";
				}
				else
				{
					mediarapModel.SearchKey  = ebSearchKey.Text;
				}

				mediarapModel.SearchMediaRap = cbSearchMediaRap.SelectedItem.Value.ToString();

                if (uiCheckBox1.Checked)
				{
					mediarapModel.SearchchkAdState_10   = "Y";
				}
				else
				{
					mediarapModel.SearchchkAdState_10   = "N";
				}

				ResetMediaRapDetailText();

				// ¹Ìµð¾î·¾¸ñ·ÏÁ¶È¸ ¼­ºñ½º¸¦ È£ÃâÇÑ´Ù.
				new MediaRapManager(systemModel,commonModel).GetMediaRapList(mediarapModel);

				if (mediarapModel.ResultCD.Equals("0000"))
				{
					Utility.SetDataTable(mediaRapDs.MediaRaps, mediarapModel.MediaRapDataSet);				
					StatusMessage(mediarapModel.ResultCnt + "°ÇÀÇ ¹Ìµð¾î·¾ Á¤º¸°¡ Á¶È¸µÇ¾ú½À´Ï´Ù.");
					if(canUpdate)
					{
						AddSchChoice();									
					}		
					SetMediaRapDetailText();
				}
			}
			catch(FrameException fe)
			{
				FrameSystem.showMsgForm("¹Ìµð¾î·¾Á¶È¸¿À·ù", new string[] {fe.ErrCode, fe.ResultMsg});
			}
			catch(Exception ex)
			{
				FrameSystem.showMsgForm("¹Ìµð¾î·¾Á¶È¸¿À·ù",new string[] {"",ex.Message});
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
				if ( mediaRapDs.Tables["MediaRaps"].Rows.Count < 1 ) return;
              
				foreach (DataRow row in mediaRapDs.Tables["MediaRaps"].Rows)
				{					
					if(IsAdding)
					{
						cm.Position = 0;
						rapCode = null;						
					}
					else
					{						
						if(row["RapCode"].ToString().Equals(rapCode))
						{					
							cm.Position = rowIndex;
							break;								
						}
					}

					rowIndex++;
					grdExMediaRapList.EnsureVisible();
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
		/// ¹Ìµð¾î·¾»ó¼¼Á¤º¸ ÀúÀå
		/// </summary>
		private void SaveMediaRapDetail()
		{
			StatusMessage("¹Ìµð¾î·¾ Á¤º¸¸¦ ÀúÀåÇÕ´Ï´Ù.");
			
			if(ebMediaRapName.Text.Trim().Length == 0) 
			{
				MessageBox.Show("¹Ìµð¾î·¾ÀÌ¸§ÀÌ ÀÔ·ÂµÇÁö ¾Ê¾Ò½À´Ï´Ù.","·¦»ç ÀúÀå", 
					MessageBoxButtons.OK, MessageBoxIcon.Information );
					ebMediaRapName.Focus();
				return;				
			}

			try
			{

				// µ¥ÀÌÅÍ¸ðµ¨¿¡ Àü¼ÛÇÒ ³»¿ëÀ» ¼ÂÆ®ÇÑ´Ù.
				mediarapModel.RapCode       = rapCode;				
				mediarapModel.RapName     = ebMediaRapName.Text;
				mediarapModel.RapType    = cbMediaRapType.SelectedValue.ToString(); 				
				mediarapModel.Tell     = ebMediaRapTell.Text;				
				mediarapModel.Comment  = ebMediaRapComment.Text;
				//»ç¿ë¿©ºÎ
				if(rbUseYn_Y.Checked)
				{
					mediarapModel.UseYn       = "Y";
				}
				else
				{
					mediarapModel.UseYn       = "N";
				}
				
				// ¹Ìµð¾î·¾ »ó¼¼Á¤º¸ ÀúÀå ¼­ºñ½º¸¦ È£ÃâÇÑ´Ù.
				if (IsAdding)
				{
					new MediaRapManager(systemModel,commonModel).SetMediaRapAdd(mediarapModel);
					StatusMessage("´ëÇà»ç Á¤º¸°¡ Ãß°¡µÇ¾ú½À´Ï´Ù.");
					IsAdding = false;
					ResetMediaRapDetailText();
				}
				else
				{
					new MediaRapManager(systemModel,commonModel).SetMediaRapUpdate(mediarapModel);
				}

				DisableButton();
				SearchMediaRap();
				InitButton();

			}
			catch(FrameException fe)
			{
				FrameSystem.showMsgForm("¹Ìµð¾î·¾Á¤º¸ ÀúÀå¿À·ù", new string[] {fe.ErrCode, fe.ResultMsg});
			}
			catch(Exception ex)
			{
				FrameSystem.showMsgForm("¹Ìµð¾î·¾Á¤º¸ ÀúÀå¿À·ù",new string[] {"",ex.Message});
			}			
		}

		/// <summary>
		/// ¹Ìµð¾î·¾Á¤º¸ »èÁ¦
		/// </summary>
		private void DeleteMediaRap()
		{
			StatusMessage("¹Ìµð¾î·¾ Á¤º¸¸¦ »èÁ¦ÇÕ´Ï´Ù.");

			if(ebMediaRapName.Text.Trim().Length == 0) 
			{
				MessageBox.Show("»èÁ¦ÇÒ ¹Ìµð¾î·¾ Á¤º¸°¡ ¾ø½À´Ï´Ù.","·¦»ç »èÁ¦", 
					MessageBoxButtons.OK, MessageBoxIcon.Information );
				return;
			}

			DialogResult result = MessageBox.Show("ÇØ´ç ¹Ìµð¾î·¾ Á¤º¸¸¦ »èÁ¦ ÇÏ½Ã°Ú½À´Ï±î?","¹Ìµð¾î·¾ »èÁ¦",
				MessageBoxButtons.YesNo, MessageBoxIcon.Question,
				MessageBoxDefaultButton.Button2);

			if (result == DialogResult.No) return;

			try
			{
				// µ¥ÀÌÅÍ¸ðµ¨¿¡ Àü¼ÛÇÒ ³»¿ëÀ» ¼ÂÆ®ÇÑ´Ù.
				mediarapModel.RapCode       = rapCode;

				// ¹Ìµð¾î·¾ »ó¼¼Á¤º¸ ÀúÀå ¼­ºñ½º¸¦ È£ÃâÇÑ´Ù.
				new MediaRapManager(systemModel,commonModel).SetMediaRapDelete(mediarapModel);
				
				ResetMediaRapDetailText();
				StatusMessage("¹Ìµð¾î·¾ Á¤º¸°¡ »èÁ¦µÇ¾ú½À´Ï´Ù.");							
				DisableButton();
				SearchMediaRap();
				InitButton();
			}
			catch(FrameException fe)
			{
				FrameSystem.showMsgForm("¹Ìµð¾î·¾Á¤º¸ »èÁ¦¿À·ù", new string[] {fe.ErrCode, fe.ResultMsg});
			}
			catch(Exception ex)
			{
				FrameSystem.showMsgForm("¹Ìµð¾î·¾Á¤º¸ »èÁ¦¿À·ù",new string[] {"",ex.Message});
			}			

		}
		
		/// <summary>
		/// ¹Ìµð¾î·¾ »ó¼¼Á¤º¸ÀÇ ¼ÂÆ®
		/// </summary>
		private void SetMediaRapDetailText()
		{
			int curRow = cm.Position;
			if(curRow >= 0)
			{
				rapCode             = dt.Rows[curRow]["RapCode"].ToString();			
				ebMediaRapName.Text           = dt.Rows[curRow]["RapName"].ToString();
				cbMediaRapType.SelectedValue = dt.Rows[curRow]["RapType"].ToString();			
				ebMediaRapTell.Text           = dt.Rows[curRow]["Tell"].ToString();			
				ebRegDt.Text              = dt.Rows[curRow]["RegDt"].ToString();
				ebModDt.Text             = dt.Rows[curRow]["ModDt"].ToString();
				ebRegID.Text             = dt.Rows[curRow]["RegName"].ToString();
				ebMediaRapComment.Text        = dt.Rows[curRow]["Comment"].ToString();
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

		private void ResetMediaRapDetailText()
		{			
			ebMediaRapName.Text           = "";
			cbMediaRapType.SelectedIndex =  0;			
			ebMediaRapTell.Text           = "";			
			ebRegDt.Text              = "";
			ebModDt.Text				= "";
			ebMediaRapComment.Text        = "";			
			rbUseYn_Y.Checked         = true;
			rbUseYn_N.Checked         = false;
		}
		
		/// <summary>
		/// »ó¼¼Á¤º¸ ReadOnly
		/// </summary>
		private void SetTextReadonly()
		{				
			ebMediaRapName.ReadOnly       = true;
			cbMediaRapType.ReadOnly      = true;			
			ebMediaRapTell.ReadOnly       = true;
			ebMediaRapComment.ReadOnly    = true;
			cbMediaRapType.ReadOnly    = true;
			rbUseYn_Y.Enabled         = false;
			rbUseYn_N.Enabled         = false;
						
			ebMediaRapName.BackColor      = Color.WhiteSmoke;
			cbMediaRapType.BackColor     = Color.WhiteSmoke;			
			ebMediaRapTell.BackColor      = Color.WhiteSmoke;			
			ebMediaRapComment.BackColor   = Color.WhiteSmoke;
			cbMediaRapType.BackColor   = Color.WhiteSmoke;
			
		}

		/// <summary>
		/// »ó¼¼Á¤º¸ ¼öÁ¤°¡´ÉÄÉ
		/// </summary>
		private void ResetTextReadonly()
		{			
			ebMediaRapName.ReadOnly       = false;
			cbMediaRapType.ReadOnly      = false;
			ebMediaRapTell.ReadOnly       = false;
			ebMediaRapComment.ReadOnly      = false;		
			cbMediaRapType.ReadOnly    = false;
			// »ç¿ëÀÚ±¸ºÐÀÌ ¾îµå¹Î ¶Ç´Â ¼öÆÛÀ¯ÀúÀÎ °æ¿ì¸¸ »ç¿ë·¹º§, »ç¿ëÀÚ±¸ºÐ, »ç¿ë¿©ºÎ¸¦ ¼öÁ¤ÇÒ ¼ö ÀÖ´Ù.
			if(commonModel.UserClass.Equals("10") || commonModel.UserClass.Equals("20"))
			{
				rbUseYn_Y.Enabled         = true;
				rbUseYn_N.Enabled         = true;
			}
			
			ebMediaRapName.BackColor      = Color.White;
			cbMediaRapType.BackColor     = Color.White;			
			ebMediaRapTell.BackColor      = Color.White;			
			ebMediaRapComment.BackColor   = Color.White;
			cbMediaRapType.BackColor   = Color.White;			
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
