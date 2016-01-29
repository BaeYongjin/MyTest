// ===============================================================================
// ClientControl for Charites Project
//
// ClientControl.cs
//
// ∏≈√º¥Î«‡±§∞Ì¡÷¡§∫∏∞¸∏Æ ƒ¡µÂ∑—¿ª ¡§¿««’¥œ¥Ÿ. 
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
	/// ∏≈√º¥Î«‡±§∞Ì¡÷∞¸∏Æ ƒ¡∆Æ∑—
	/// </summary>
    public class ClientControl : System.Windows.Forms.UserControl, IUserControl
	{
		#region ¿Ã∫•∆Æ«⁄µÈ∑Ø
		public event StatusEventHandler 			StatusEvent;			// ªÛ≈¬¿Ã∫•∆Æ «⁄µÈ∑Ø
		public event ProgressEventHandler 			ProgressEvent;			// √≥∏Æ¡ﬂ¿Ã∫•∆Æ «⁄µÈ∑Ø
        #endregion
			
		#region ∏≈√º¥Î«‡±§∞Ì¡÷¡§¿« ∞¥√º π◊ ∫Øºˆ

		// Ω√Ω∫≈€ ¡§∫∏ : »≠∏È∞¯≈Î
		private SystemModel   systemModel   = FrameSystem.oSysModel;
		private CommonModel   commonModel   = FrameSystem.oComModel;
		private Logger        log           = FrameSystem.oLog;
		private MenuPower     menu          = FrameSystem.oMenu;

		// ∏ﬁ¥∫ƒ⁄µÂ : ∫∏æ»¿Ã « ø‰«— »≠∏Èø° « ø‰«‘
		public string        menuCode		= "";

		// ªÁøÎ«“ ¡§∫∏∏µ®
		ClientModel clientModel  = new ClientModel();	// ∏≈√º¥Î«‡±§∞Ì¡÷¡§∫∏∏µ®

		// »≠∏È√≥∏ÆøÎ ∫Øºˆ
		bool IsNewSearchKey		  = true;					// ∞ÀªˆæÓ¿‘∑¬ ø©∫Œ		
		CurrencyManager cm        = null;					// µ•¿Ã≈Õ ±◊∏ÆµÂ¿« ∫Ø∞Êø° µ˚∏• µ•¿Ã≈Õº¬ ∞¸∏Æ∏¶ ¿ß«œø©			
		DataTable       dt        = null;

        bool IsSearching = false; // ¡∂»∏¡ﬂ ªÛºº»≠∏È¿Ã æ˜µ•¿Ã∆Æ µ«¥¬ ∞Õ¿ª πÊ¡ˆ «œ±‚¿ß«‘ 2011.11.29 JH.Park
		bool IsAdding             = false;
		bool canRead			  = false;
		bool canUpdate			  = false;
		bool canCreate            = false;
		bool canDelete            = false;

		private string        mediaCode = "";
		private string        rapCode = "";
		private string        agencyCode = "";
		private string        advertiserCode = "";
		private string        Search_advertiserCode = "";
        private Janus.Windows.EditControls.UICheckBox uiCheckBox1;
		
		int LastOrder = 0;

		#endregion

        #region IUserControl ±∏«ˆ
        /// <summary>
        /// ∏ﬁ¥∫ ƒ⁄µÂ-∫∏æ»¿Ã « ø‰«— »≠∏Èø° « ø‰«‘
        /// </summary>
        public string MenuCode
        {
            set { this.menuCode = value; }
            get { return this.menuCode; }
        }

        /// <summary>
        /// ∫Œ∏ƒ¡∆Æ∑— ¡ˆ¡§
        /// </summary>
        /// <param name="control"></param>
        public void SetParent(Control control)
        {
            this.Parent = control;
        }
        /// <summary>
        /// DockStype¡ˆ¡§
        /// </summary>
        /// <param name="style"></param>
        public void SetDockStyle(DockStyle style)
        {
            this.Dock = style;
        }
        #endregion

		#region »≠∏È ƒƒ∆˜≥Õ∆Æ, ª˝º∫¿⁄, º“∏Í¿⁄
		private Janus.Windows.UI.Dock.UIPanelManager uiPM;
		private Janus.Windows.UI.Dock.UIPanel uiPanelClientList;
		private Janus.Windows.UI.Dock.UIPanelInnerContainer uiPanelClientListContainer;
		private Janus.Windows.UI.Dock.UIPanel uiPanelClientDetail;
		private Janus.Windows.UI.Dock.UIPanelInnerContainer uiPanelClientDetailContainer;
		private Janus.Windows.UI.Dock.UIPanel uiPanelClientsSearch;
		private Janus.Windows.UI.Dock.UIPanelInnerContainer uiPanelClientsSearchContainer;
		private Janus.Windows.EditControls.UIButton uiButton2;
		private Janus.Windows.EditControls.UIButton uiButton1;
		private Janus.Windows.UI.Dock.UIPanelGroup uiPanelClients;
		private System.Windows.Forms.Panel pnlSearch;
		private Janus.Windows.EditControls.UIButton btnSearch;
		private System.Windows.Forms.Panel pnlClientDetail;
		private System.Windows.Forms.Label lbRegDt;
		private Janus.Windows.EditControls.UIButton btnAdd;
		private Janus.Windows.EditControls.UIButton btnSave;
		private System.Data.DataView dvClient;
		private Janus.Windows.GridEX.EditControls.EditBox ebRegDt;
		private Janus.Windows.EditControls.UIButton btnDelete;		
		private System.Windows.Forms.Label lbModDt;
		private Janus.Windows.GridEX.EditControls.EditBox ebModDt;
		private Janus.Windows.GridEX.GridEX grdExClientList;
		private System.Windows.Forms.Label lbUseYn;
		private Janus.Windows.EditControls.UIRadioButton rbUseYn_Y;
		private Janus.Windows.EditControls.UIRadioButton rbUseYn_N;
		private Janus.Windows.EditControls.UIComboBox cbSearchMediaName;
		private Janus.Windows.EditControls.UIComboBox cbSearchRapName;		
		private System.Windows.Forms.Label lbRapName;
		private System.Windows.Forms.Label lbMediaName;
		private System.Windows.Forms.Label lbAgencyName;
		private Janus.Windows.EditControls.UIComboBox uiComboBox1;
		private Janus.Windows.EditControls.UIComboBox cbSearchMediaAgency;
		private System.Windows.Forms.Label label1;
		private Janus.Windows.GridEX.EditControls.EditBox ebComment;
		private System.Windows.Forms.Label lbAdvertiserName;
		private AdManagerClient._20_Contract._04_Client.MediaAgencyDs clientDs;
		private Janus.Windows.GridEX.EditControls.EditBox ebSearchKey;
		private System.Windows.Forms.Label lbRegID;
		private Janus.Windows.GridEX.EditControls.EditBox ebRegID;
		private Janus.Windows.GridEX.EditControls.EditBox ebMedia;
		private Janus.Windows.GridEX.EditControls.EditBox ebRap;
		private Janus.Windows.GridEX.EditControls.EditBox ebAgency;
		private Janus.Windows.EditControls.UIButton btnAgencySearch;
		private Janus.Windows.GridEX.EditControls.EditBox ebAdvertiserName;
        private Janus.Windows.EditControls.UIButton btnAdvertiserSearch;
		private Janus.Windows.GridEX.EditControls.EditBox ebSearchAdvertiserName;
		private Janus.Windows.EditControls.UIButton btnAdvertiserSearch_1;
		private System.ComponentModel.IContainer components;

		public ClientControl()
		{
			// ¿Ã »£√‚¿∫ Windows.Forms Form µ¿⁄¿Ã≥ ø° « ø‰«’¥œ¥Ÿ.
			InitializeComponent();

			

		}

		/// <summary> 
		/// ªÁøÎ ¡ﬂ¿Œ ∏µÁ ∏Æº“Ω∫∏¶ ¡§∏Æ«’¥œ¥Ÿ.
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

		#region ±∏º∫ ø‰º“ µ¿⁄¿Ã≥ ø°º≠ ª˝º∫«— ƒ⁄µÂ
		/// <summary> 
		/// µ¿⁄¿Ã≥  ¡ˆø¯ø° « ø‰«— ∏ﬁº≠µÂ¿‘¥œ¥Ÿ. 
		/// ¿Ã ∏ﬁº≠µÂ¿« ≥ªøÎ¿ª ƒ⁄µÂ ∆Ì¡˝±‚∑Œ ºˆ¡§«œ¡ˆ ∏∂Ω Ω√ø¿.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			Janus.Windows.GridEX.GridEXLayout grdExClientList_DesignTimeLayout = new Janus.Windows.GridEX.GridEXLayout();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ClientControl));
			this.uiPM = new Janus.Windows.UI.Dock.UIPanelManager(this.components);
			this.uiPanelClients = new Janus.Windows.UI.Dock.UIPanelGroup();
			this.uiPanelClientsSearch = new Janus.Windows.UI.Dock.UIPanel();
			this.uiPanelClientsSearchContainer = new Janus.Windows.UI.Dock.UIPanelInnerContainer();
			this.pnlSearch = new System.Windows.Forms.Panel();
			this.uiCheckBox1 = new Janus.Windows.EditControls.UICheckBox();
			this.ebSearchKey = new Janus.Windows.GridEX.EditControls.EditBox();
			this.cbSearchMediaName = new Janus.Windows.EditControls.UIComboBox();
			this.cbSearchRapName = new Janus.Windows.EditControls.UIComboBox();
			this.cbSearchMediaAgency = new Janus.Windows.EditControls.UIComboBox();
			this.btnSearch = new Janus.Windows.EditControls.UIButton();
			this.btnAdvertiserSearch_1 = new Janus.Windows.EditControls.UIButton();
			this.ebSearchAdvertiserName = new Janus.Windows.GridEX.EditControls.EditBox();
			this.uiPanelClientList = new Janus.Windows.UI.Dock.UIPanel();
			this.uiPanelClientListContainer = new Janus.Windows.UI.Dock.UIPanelInnerContainer();
			this.grdExClientList = new Janus.Windows.GridEX.GridEX();
			this.dvClient = new System.Data.DataView();
			this.clientDs = new AdManagerClient._20_Contract._04_Client.MediaAgencyDs();
			this.uiPanelClientDetail = new Janus.Windows.UI.Dock.UIPanel();
			this.uiPanelClientDetailContainer = new Janus.Windows.UI.Dock.UIPanelInnerContainer();
			this.pnlClientDetail = new System.Windows.Forms.Panel();
			this.ebAdvertiserName = new Janus.Windows.GridEX.EditControls.EditBox();
			this.btnAgencySearch = new Janus.Windows.EditControls.UIButton();
			this.ebAgency = new Janus.Windows.GridEX.EditControls.EditBox();
			this.ebRap = new Janus.Windows.GridEX.EditControls.EditBox();
			this.ebMedia = new Janus.Windows.GridEX.EditControls.EditBox();
			this.ebRegID = new Janus.Windows.GridEX.EditControls.EditBox();
			this.ebRegDt = new Janus.Windows.GridEX.EditControls.EditBox();
			this.lbRegID = new System.Windows.Forms.Label();
			this.ebModDt = new Janus.Windows.GridEX.EditControls.EditBox();
			this.ebComment = new Janus.Windows.GridEX.EditControls.EditBox();
			this.label1 = new System.Windows.Forms.Label();
			this.rbUseYn_N = new Janus.Windows.EditControls.UIRadioButton();
			this.rbUseYn_Y = new Janus.Windows.EditControls.UIRadioButton();
			this.lbUseYn = new System.Windows.Forms.Label();
			this.lbModDt = new System.Windows.Forms.Label();
			this.lbRegDt = new System.Windows.Forms.Label();
			this.lbRapName = new System.Windows.Forms.Label();
			this.lbMediaName = new System.Windows.Forms.Label();
			this.lbAgencyName = new System.Windows.Forms.Label();
			this.lbAdvertiserName = new System.Windows.Forms.Label();
			this.btnDelete = new Janus.Windows.EditControls.UIButton();
			this.btnAdd = new Janus.Windows.EditControls.UIButton();
			this.btnSave = new Janus.Windows.EditControls.UIButton();
			this.btnAdvertiserSearch = new Janus.Windows.EditControls.UIButton();
			this.uiComboBox1 = new Janus.Windows.EditControls.UIComboBox();
			this.uiButton1 = new Janus.Windows.EditControls.UIButton();
			this.uiButton2 = new Janus.Windows.EditControls.UIButton();
			((System.ComponentModel.ISupportInitialize)(this.uiPM)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.uiPanelClients)).BeginInit();
			this.uiPanelClients.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.uiPanelClientsSearch)).BeginInit();
			this.uiPanelClientsSearch.SuspendLayout();
			this.uiPanelClientsSearchContainer.SuspendLayout();
			this.pnlSearch.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.uiPanelClientList)).BeginInit();
			this.uiPanelClientList.SuspendLayout();
			this.uiPanelClientListContainer.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.grdExClientList)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.dvClient)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.clientDs)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.uiPanelClientDetail)).BeginInit();
			this.uiPanelClientDetail.SuspendLayout();
			this.uiPanelClientDetailContainer.SuspendLayout();
			this.pnlClientDetail.SuspendLayout();
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
			this.uiPanelClients.Id = new System.Guid("b162c53a-7940-47cd-a0ed-9762c59c50d8");
			this.uiPanelClients.StaticGroup = true;
			this.uiPanelClientsSearch.Id = new System.Guid("afb3b7d0-aa79-443c-a1e8-3baba38f7a4d");
			this.uiPanelClients.Panels.Add(this.uiPanelClientsSearch);
			this.uiPanelClientList.Id = new System.Guid("8e3140b9-bfcb-43af-b84e-2ee6cddf51dc");
			this.uiPanelClients.Panels.Add(this.uiPanelClientList);
			this.uiPanelClientDetail.Id = new System.Guid("b2973a57-d3e1-4d80-9387-7a6dc3332542");
			this.uiPanelClients.Panels.Add(this.uiPanelClientDetail);
			this.uiPM.Panels.Add(this.uiPanelClients);
			// 
			// Design Time Panel Info:
			// 
			this.uiPM.BeginPanelInfo();
			this.uiPM.AddDockPanelInfo(new System.Guid("b162c53a-7940-47cd-a0ed-9762c59c50d8"), Janus.Windows.UI.Dock.PanelGroupStyle.HorizontalTiles, Janus.Windows.UI.Dock.PanelDockStyle.Fill, true, new System.Drawing.Size(1010, 677), true);
			this.uiPM.AddDockPanelInfo(new System.Guid("afb3b7d0-aa79-443c-a1e8-3baba38f7a4d"), new System.Guid("b162c53a-7940-47cd-a0ed-9762c59c50d8"), 43, true);
			this.uiPM.AddDockPanelInfo(new System.Guid("8e3140b9-bfcb-43af-b84e-2ee6cddf51dc"), new System.Guid("b162c53a-7940-47cd-a0ed-9762c59c50d8"), 377, true);
			this.uiPM.AddDockPanelInfo(new System.Guid("b2973a57-d3e1-4d80-9387-7a6dc3332542"), new System.Guid("b162c53a-7940-47cd-a0ed-9762c59c50d8"), 227, true);
			this.uiPM.AddFloatingPanelInfo(new System.Guid("b162c53a-7940-47cd-a0ed-9762c59c50d8"), Janus.Windows.UI.Dock.PanelGroupStyle.HorizontalTiles, true, new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
			this.uiPM.AddFloatingPanelInfo(new System.Guid("8e3140b9-bfcb-43af-b84e-2ee6cddf51dc"), new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
			this.uiPM.AddFloatingPanelInfo(new System.Guid("b2973a57-d3e1-4d80-9387-7a6dc3332542"), new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
			this.uiPM.AddFloatingPanelInfo(new System.Guid("afb3b7d0-aa79-443c-a1e8-3baba38f7a4d"), new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
			this.uiPM.EndPanelInfo();
			// 
			// uiPanelClients
			// 
			this.uiPanelClients.ActiveCaptionMode = Janus.Windows.UI.Dock.ActiveCaptionMode.Never;
			this.uiPanelClients.BackColor = System.Drawing.SystemColors.Window;
			this.uiPanelClients.CaptionVisible = Janus.Windows.UI.InheritableBoolean.True;
			this.uiPanelClients.CloseButtonVisible = Janus.Windows.UI.InheritableBoolean.False;
			this.uiPanelClients.Location = new System.Drawing.Point(0, 0);
			this.uiPanelClients.Name = "uiPanelClients";
			this.uiPanelClients.Size = new System.Drawing.Size(1010, 677);
			this.uiPanelClients.TabIndex = 4;
			this.uiPanelClients.Text = "¥Î«‡ªÁ∫∞ ±§∞Ì¡÷∞¸∏Æ";
			// 
			// uiPanelClientsSearch
			// 
			this.uiPanelClientsSearch.AllowResize = Janus.Windows.UI.InheritableBoolean.False;
			this.uiPanelClientsSearch.CaptionVisible = Janus.Windows.UI.InheritableBoolean.False;
			this.uiPanelClientsSearch.CloseButtonVisible = Janus.Windows.UI.InheritableBoolean.False;
			this.uiPanelClientsSearch.InnerContainer = this.uiPanelClientsSearchContainer;
			this.uiPanelClientsSearch.Location = new System.Drawing.Point(0, 22);
			this.uiPanelClientsSearch.Name = "uiPanelClientsSearch";
			this.uiPanelClientsSearch.Size = new System.Drawing.Size(1010, 43);
			this.uiPanelClientsSearch.TabIndex = 4;
			this.uiPanelClientsSearch.Text = "∞Àªˆ";
			// 
			// uiPanelClientsSearchContainer
			// 
			this.uiPanelClientsSearchContainer.Controls.Add(this.pnlSearch);
			this.uiPanelClientsSearchContainer.Location = new System.Drawing.Point(1, 1);
			this.uiPanelClientsSearchContainer.Name = "uiPanelClientsSearchContainer";
			this.uiPanelClientsSearchContainer.Size = new System.Drawing.Size(1008, 41);
			this.uiPanelClientsSearchContainer.TabIndex = 0;
			// 
			// pnlSearch
			// 
			this.pnlSearch.BackColor = System.Drawing.SystemColors.Window;
			this.pnlSearch.Controls.Add(this.uiCheckBox1);
			this.pnlSearch.Controls.Add(this.ebSearchKey);
			this.pnlSearch.Controls.Add(this.cbSearchMediaName);
			this.pnlSearch.Controls.Add(this.cbSearchRapName);
			this.pnlSearch.Controls.Add(this.cbSearchMediaAgency);
			this.pnlSearch.Controls.Add(this.btnSearch);
			this.pnlSearch.Controls.Add(this.btnAdvertiserSearch_1);
			this.pnlSearch.Controls.Add(this.ebSearchAdvertiserName);
			this.pnlSearch.Dock = System.Windows.Forms.DockStyle.Fill;
			this.pnlSearch.Location = new System.Drawing.Point(0, 0);
			this.pnlSearch.Name = "pnlSearch";
			this.pnlSearch.Size = new System.Drawing.Size(1008, 41);
			this.pnlSearch.TabIndex = 0;
			// 
			// uiCheckBox1
			// 
			this.uiCheckBox1.Location = new System.Drawing.Point(566, 6);
			this.uiCheckBox1.Name = "uiCheckBox1";
			this.uiCheckBox1.Size = new System.Drawing.Size(104, 23);
			this.uiCheckBox1.TabIndex = 9;
			this.uiCheckBox1.Text = "ªÁøÎæ»«‘ ∆˜«‘";
			// 
			// ebSearchKey
			// 
			this.ebSearchKey.Location = new System.Drawing.Point(368, 8);
			this.ebSearchKey.Name = "ebSearchKey";
			this.ebSearchKey.Size = new System.Drawing.Size(192, 20);
			this.ebSearchKey.TabIndex = 4;
			this.ebSearchKey.Text = "∞ÀªˆæÓ∏¶ ¿‘∑¬«œººø‰";
			this.ebSearchKey.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
			this.ebSearchKey.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
			this.ebSearchKey.TextChanged += new System.EventHandler(this.ebSearchKey_TextChanged);
			this.ebSearchKey.Click += new System.EventHandler(this.ebSearchKey_Click);
			this.ebSearchKey.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ebSearchKey_KeyDown);
			// 
			// cbSearchMediaName
			// 
			this.cbSearchMediaName.BackColor = System.Drawing.Color.White;
			this.cbSearchMediaName.ComboStyle = Janus.Windows.EditControls.ComboStyle.DropDownList;
			this.cbSearchMediaName.Location = new System.Drawing.Point(8, 8);
			this.cbSearchMediaName.Name = "cbSearchMediaName";
			this.cbSearchMediaName.Size = new System.Drawing.Size(115, 20);
			this.cbSearchMediaName.TabIndex = 1;
			this.cbSearchMediaName.Text = "∏≈√ºº±≈√";
			this.cbSearchMediaName.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
			// 
			// cbSearchRapName
			// 
			this.cbSearchRapName.ComboStyle = Janus.Windows.EditControls.ComboStyle.DropDownList;
			this.cbSearchRapName.Location = new System.Drawing.Point(128, 8);
			this.cbSearchRapName.Name = "cbSearchRapName";
			this.cbSearchRapName.Size = new System.Drawing.Size(115, 20);
			this.cbSearchRapName.TabIndex = 2;
			this.cbSearchRapName.Text = "πÃµæÓ∑æº±≈√";
			this.cbSearchRapName.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
			// 
			// cbSearchMediaAgency
			// 
			this.cbSearchMediaAgency.ComboStyle = Janus.Windows.EditControls.ComboStyle.DropDownList;
			this.cbSearchMediaAgency.Location = new System.Drawing.Point(248, 8);
			this.cbSearchMediaAgency.Name = "cbSearchMediaAgency";
			this.cbSearchMediaAgency.Size = new System.Drawing.Size(115, 20);
			this.cbSearchMediaAgency.TabIndex = 3;
			this.cbSearchMediaAgency.Text = "¥Î«‡ªÁº±≈√";
			this.cbSearchMediaAgency.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
			// 
			// btnSearch
			// 
			this.btnSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnSearch.Enabled = false;
			this.btnSearch.Location = new System.Drawing.Point(895, 8);
			this.btnSearch.Name = "btnSearch";
			this.btnSearch.Size = new System.Drawing.Size(104, 24);
			this.btnSearch.TabIndex = 6;
			this.btnSearch.Text = "¡∂ »∏";
			this.btnSearch.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
			this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
			// 
			// btnAdvertiserSearch_1
			// 
			this.btnAdvertiserSearch_1.Enabled = false;
			this.btnAdvertiserSearch_1.Location = new System.Drawing.Point(192, 34);
			this.btnAdvertiserSearch_1.Name = "btnAdvertiserSearch_1";
			this.btnAdvertiserSearch_1.Size = new System.Drawing.Size(104, 21);
			this.btnAdvertiserSearch_1.TabIndex = 8;
			this.btnAdvertiserSearch_1.Text = "±§∞Ì¡÷º±≈√";
			this.btnAdvertiserSearch_1.Visible = false;
			this.btnAdvertiserSearch_1.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
			this.btnAdvertiserSearch_1.Click += new System.EventHandler(this.btnAdvertiserSearch_1_Click);
			// 
			// ebSearchAdvertiserName
			// 
			this.ebSearchAdvertiserName.BackColor = System.Drawing.Color.WhiteSmoke;
			this.ebSearchAdvertiserName.Font = new System.Drawing.Font("±º∏≤√º", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
			this.ebSearchAdvertiserName.Location = new System.Drawing.Point(8, 34);
			this.ebSearchAdvertiserName.MaxLength = 20;
			this.ebSearchAdvertiserName.Name = "ebSearchAdvertiserName";
			this.ebSearchAdvertiserName.ReadOnly = true;
			this.ebSearchAdvertiserName.Size = new System.Drawing.Size(176, 21);
			this.ebSearchAdvertiserName.TabIndex = 7;
			this.ebSearchAdvertiserName.TabStop = false;
			this.ebSearchAdvertiserName.Text = "±§∞Ì¡÷∏¶ º±≈√«ÿ¡÷ººø‰";
			this.ebSearchAdvertiserName.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
			this.ebSearchAdvertiserName.Visible = false;
			this.ebSearchAdvertiserName.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
			// 
			// uiPanelClientList
			// 
			this.uiPanelClientList.BackColor = System.Drawing.SystemColors.Window;
			this.uiPanelClientList.CaptionStyle = Janus.Windows.UI.Dock.PanelCaptionStyle.Dark;
			this.uiPanelClientList.InnerContainer = this.uiPanelClientListContainer;
			this.uiPanelClientList.Location = new System.Drawing.Point(0, 69);
			this.uiPanelClientList.MinimumSize = new System.Drawing.Size(-1, 100);
			this.uiPanelClientList.Name = "uiPanelClientList";
			this.uiPanelClientList.Size = new System.Drawing.Size(1010, 377);
			this.uiPanelClientList.TabIndex = 7;
			this.uiPanelClientList.TabStop = false;
			this.uiPanelClientList.Text = "±§∞Ì¡÷∏Ò∑œ";
			// 
			// uiPanelClientListContainer
			// 
			this.uiPanelClientListContainer.BackColor = System.Drawing.SystemColors.Window;
			this.uiPanelClientListContainer.Controls.Add(this.grdExClientList);
			this.uiPanelClientListContainer.Location = new System.Drawing.Point(1, 23);
			this.uiPanelClientListContainer.Name = "uiPanelClientListContainer";
			this.uiPanelClientListContainer.Size = new System.Drawing.Size(1008, 353);
			this.uiPanelClientListContainer.TabIndex = 0;
			// 
			// grdExClientList
			// 
			this.grdExClientList.AllowEdit = Janus.Windows.GridEX.InheritableBoolean.False;
			this.grdExClientList.AlternatingColors = true;
			this.grdExClientList.BorderStyle = Janus.Windows.GridEX.BorderStyle.None;
			this.grdExClientList.DataSource = this.dvClient;
			grdExClientList_DesignTimeLayout.LayoutString = resources.GetString("grdExClientList_DesignTimeLayout.LayoutString");
			this.grdExClientList.DesignTimeLayout = grdExClientList_DesignTimeLayout;
			this.grdExClientList.Dock = System.Windows.Forms.DockStyle.Fill;
			this.grdExClientList.EditorsControlStyle.ButtonAppearance = Janus.Windows.GridEX.ButtonAppearance.Regular;
			this.grdExClientList.EmptyRows = true;
			this.grdExClientList.FocusCellFormatStyle.BackColor = System.Drawing.SystemColors.Highlight;
			this.grdExClientList.FocusCellFormatStyle.ForeColor = System.Drawing.Color.White;
			this.grdExClientList.FocusStyle = Janus.Windows.GridEX.FocusStyle.None;
			this.grdExClientList.FrozenColumns = 2;
			this.grdExClientList.GridLineColor = System.Drawing.Color.Silver;
			this.grdExClientList.GridLines = Janus.Windows.GridEX.GridLines.Vertical;
			this.grdExClientList.GridLineStyle = Janus.Windows.GridEX.GridLineStyle.Solid;
			this.grdExClientList.GroupByBoxVisible = false;
			this.grdExClientList.HeaderFormatStyle.TextAlignment = Janus.Windows.GridEX.TextAlignment.Center;
			this.grdExClientList.HideSelection = Janus.Windows.GridEX.HideSelection.HighlightInactive;
			this.grdExClientList.Location = new System.Drawing.Point(0, 0);
			this.grdExClientList.Name = "grdExClientList";
			this.grdExClientList.ScrollBars = Janus.Windows.GridEX.ScrollBars.Vertical;
			this.grdExClientList.SelectedInactiveFormatStyle.BackColor = System.Drawing.Color.Gold;
			this.grdExClientList.Size = new System.Drawing.Size(1008, 353);
			this.grdExClientList.TabIndex = 9;
			this.grdExClientList.TabKeyBehavior = Janus.Windows.GridEX.TabKeyBehavior.ControlNavigation;
			this.grdExClientList.ThemedAreas = ((Janus.Windows.GridEX.ThemedArea)(((((((((Janus.Windows.GridEX.ThemedArea.ScrollBars | Janus.Windows.GridEX.ThemedArea.EditControls) 
            | Janus.Windows.GridEX.ThemedArea.Headers) 
            | Janus.Windows.GridEX.ThemedArea.GroupByBox) 
            | Janus.Windows.GridEX.ThemedArea.GroupRows) 
            | Janus.Windows.GridEX.ThemedArea.ControlBorder) 
            | Janus.Windows.GridEX.ThemedArea.Cards) 
            | Janus.Windows.GridEX.ThemedArea.Gridlines) 
            | Janus.Windows.GridEX.ThemedArea.CheckBoxes)));
			this.grdExClientList.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
			this.grdExClientList.Enter += new System.EventHandler(this.OnGrdRowChanged);
			// 
			// dvClient
			// 
			this.dvClient.Table = this.clientDs.Clients;
			// 
			// clientDs
			// 
			this.clientDs.DataSetName = "ClientDs";
			this.clientDs.Locale = new System.Globalization.CultureInfo("en-US");
			this.clientDs.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
			// 
			// uiPanelClientDetail
			// 
			this.uiPanelClientDetail.AllowResize = Janus.Windows.UI.InheritableBoolean.False;
			this.uiPanelClientDetail.BackColor = System.Drawing.SystemColors.Window;
			this.uiPanelClientDetail.CaptionStyle = Janus.Windows.UI.Dock.PanelCaptionStyle.Dark;
			this.uiPanelClientDetail.InnerContainer = this.uiPanelClientDetailContainer;
			this.uiPanelClientDetail.Location = new System.Drawing.Point(0, 450);
			this.uiPanelClientDetail.Name = "uiPanelClientDetail";
			this.uiPanelClientDetail.Size = new System.Drawing.Size(1010, 227);
			this.uiPanelClientDetail.TabIndex = 9;
			this.uiPanelClientDetail.TabStop = false;
			this.uiPanelClientDetail.Text = "ªÛºº¡§∫∏";
			// 
			// uiPanelClientDetailContainer
			// 
			this.uiPanelClientDetailContainer.Controls.Add(this.pnlClientDetail);
			this.uiPanelClientDetailContainer.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.uiPanelClientDetailContainer.Location = new System.Drawing.Point(1, 23);
			this.uiPanelClientDetailContainer.Name = "uiPanelClientDetailContainer";
			this.uiPanelClientDetailContainer.Size = new System.Drawing.Size(1008, 203);
			this.uiPanelClientDetailContainer.TabIndex = 0;
			// 
			// pnlClientDetail
			// 
			this.pnlClientDetail.BackColor = System.Drawing.SystemColors.Window;
			this.pnlClientDetail.Controls.Add(this.ebAdvertiserName);
			this.pnlClientDetail.Controls.Add(this.btnAgencySearch);
			this.pnlClientDetail.Controls.Add(this.ebAgency);
			this.pnlClientDetail.Controls.Add(this.ebRap);
			this.pnlClientDetail.Controls.Add(this.ebMedia);
			this.pnlClientDetail.Controls.Add(this.ebRegID);
			this.pnlClientDetail.Controls.Add(this.ebRegDt);
			this.pnlClientDetail.Controls.Add(this.lbRegID);
			this.pnlClientDetail.Controls.Add(this.ebModDt);
			this.pnlClientDetail.Controls.Add(this.ebComment);
			this.pnlClientDetail.Controls.Add(this.label1);
			this.pnlClientDetail.Controls.Add(this.rbUseYn_N);
			this.pnlClientDetail.Controls.Add(this.rbUseYn_Y);
			this.pnlClientDetail.Controls.Add(this.lbUseYn);
			this.pnlClientDetail.Controls.Add(this.lbModDt);
			this.pnlClientDetail.Controls.Add(this.lbRegDt);
			this.pnlClientDetail.Controls.Add(this.lbRapName);
			this.pnlClientDetail.Controls.Add(this.lbMediaName);
			this.pnlClientDetail.Controls.Add(this.lbAgencyName);
			this.pnlClientDetail.Controls.Add(this.lbAdvertiserName);
			this.pnlClientDetail.Controls.Add(this.btnDelete);
			this.pnlClientDetail.Controls.Add(this.btnAdd);
			this.pnlClientDetail.Controls.Add(this.btnSave);
			this.pnlClientDetail.Controls.Add(this.btnAdvertiserSearch);
			this.pnlClientDetail.Dock = System.Windows.Forms.DockStyle.Fill;
			this.pnlClientDetail.Location = new System.Drawing.Point(0, 0);
			this.pnlClientDetail.Name = "pnlClientDetail";
			this.pnlClientDetail.Size = new System.Drawing.Size(1008, 203);
			this.pnlClientDetail.TabIndex = 0;
			// 
			// ebAdvertiserName
			// 
			this.ebAdvertiserName.BackColor = System.Drawing.Color.WhiteSmoke;
			this.ebAdvertiserName.Location = new System.Drawing.Point(648, 8);
			this.ebAdvertiserName.MaxLength = 20;
			this.ebAdvertiserName.Name = "ebAdvertiserName";
			this.ebAdvertiserName.Size = new System.Drawing.Size(192, 20);
			this.ebAdvertiserName.TabIndex = 12;
			this.ebAdvertiserName.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
			this.ebAdvertiserName.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
			// 
			// btnAgencySearch
			// 
			this.btnAgencySearch.Enabled = false;
			this.btnAgencySearch.Location = new System.Drawing.Point(80, 32);
			this.btnAgencySearch.Name = "btnAgencySearch";
			this.btnAgencySearch.Size = new System.Drawing.Size(104, 24);
			this.btnAgencySearch.TabIndex = 20;
			this.btnAgencySearch.Text = "¥Î«‡ªÁº±≈√";
			this.btnAgencySearch.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
			this.btnAgencySearch.Click += new System.EventHandler(this.btnAgencySearch_Click);
			// 
			// ebAgency
			// 
			this.ebAgency.BackColor = System.Drawing.Color.WhiteSmoke;
			this.ebAgency.Location = new System.Drawing.Point(80, 8);
			this.ebAgency.MaxLength = 100;
			this.ebAgency.Name = "ebAgency";
			this.ebAgency.ReadOnly = true;
			this.ebAgency.Size = new System.Drawing.Size(192, 20);
			this.ebAgency.TabIndex = 10;
			this.ebAgency.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
			this.ebAgency.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
			// 
			// ebRap
			// 
			this.ebRap.BackColor = System.Drawing.Color.WhiteSmoke;
			this.ebRap.Location = new System.Drawing.Point(368, 32);
			this.ebRap.Name = "ebRap";
			this.ebRap.ReadOnly = true;
			this.ebRap.Size = new System.Drawing.Size(192, 20);
			this.ebRap.TabIndex = 13;
			this.ebRap.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
			this.ebRap.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
			// 
			// ebMedia
			// 
			this.ebMedia.BackColor = System.Drawing.Color.WhiteSmoke;
			this.ebMedia.Location = new System.Drawing.Point(368, 8);
			this.ebMedia.Name = "ebMedia";
			this.ebMedia.ReadOnly = true;
			this.ebMedia.Size = new System.Drawing.Size(192, 20);
			this.ebMedia.TabIndex = 11;
			this.ebMedia.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
			this.ebMedia.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
			// 
			// ebRegID
			// 
			this.ebRegID.BackColor = System.Drawing.Color.WhiteSmoke;
			this.ebRegID.Location = new System.Drawing.Point(648, 112);
			this.ebRegID.Name = "ebRegID";
			this.ebRegID.ReadOnly = true;
			this.ebRegID.Size = new System.Drawing.Size(192, 20);
			this.ebRegID.TabIndex = 0;
			this.ebRegID.TabStop = false;
			this.ebRegID.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
			this.ebRegID.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
			// 
			// ebRegDt
			// 
			this.ebRegDt.BackColor = System.Drawing.Color.WhiteSmoke;
			this.ebRegDt.Location = new System.Drawing.Point(648, 64);
			this.ebRegDt.Name = "ebRegDt";
			this.ebRegDt.ReadOnly = true;
			this.ebRegDt.Size = new System.Drawing.Size(192, 20);
			this.ebRegDt.TabIndex = 0;
			this.ebRegDt.TabStop = false;
			this.ebRegDt.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
			this.ebRegDt.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
			// 
			// lbRegID
			// 
			this.lbRegID.Location = new System.Drawing.Point(576, 112);
			this.lbRegID.Name = "lbRegID";
			this.lbRegID.Size = new System.Drawing.Size(72, 21);
			this.lbRegID.TabIndex = 46;
			this.lbRegID.Text = "µÓ∑œ¿⁄ID";
			this.lbRegID.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// ebModDt
			// 
			this.ebModDt.BackColor = System.Drawing.Color.WhiteSmoke;
			this.ebModDt.Location = new System.Drawing.Point(648, 88);
			this.ebModDt.Name = "ebModDt";
			this.ebModDt.ReadOnly = true;
			this.ebModDt.Size = new System.Drawing.Size(192, 20);
			this.ebModDt.TabIndex = 0;
			this.ebModDt.TabStop = false;
			this.ebModDt.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
			this.ebModDt.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
			// 
			// ebComment
			// 
			this.ebComment.Location = new System.Drawing.Point(80, 64);
			this.ebComment.Multiline = true;
			this.ebComment.Name = "ebComment";
			this.ebComment.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.ebComment.Size = new System.Drawing.Size(480, 88);
			this.ebComment.TabIndex = 14;
			this.ebComment.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
			this.ebComment.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(8, 64);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(72, 21);
			this.label1.TabIndex = 43;
			this.label1.Text = "∫Ò∞Ì";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// rbUseYn_N
			// 
			this.rbUseYn_N.BackColor = System.Drawing.Color.Transparent;
			this.rbUseYn_N.Location = new System.Drawing.Point(720, 136);
			this.rbUseYn_N.Name = "rbUseYn_N";
			this.rbUseYn_N.Size = new System.Drawing.Size(72, 23);
			this.rbUseYn_N.TabIndex = 16;
			this.rbUseYn_N.Text = "ªÁøÎæ»«‘";
			this.rbUseYn_N.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
			// 
			// rbUseYn_Y
			// 
			this.rbUseYn_Y.BackColor = System.Drawing.Color.Transparent;
			this.rbUseYn_Y.Location = new System.Drawing.Point(648, 136);
			this.rbUseYn_Y.Name = "rbUseYn_Y";
			this.rbUseYn_Y.Size = new System.Drawing.Size(72, 23);
			this.rbUseYn_Y.TabIndex = 15;
			this.rbUseYn_Y.Text = "ªÁøÎ«‘";
			this.rbUseYn_Y.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
			// 
			// lbUseYn
			// 
			this.lbUseYn.Location = new System.Drawing.Point(576, 136);
			this.lbUseYn.Name = "lbUseYn";
			this.lbUseYn.Size = new System.Drawing.Size(72, 21);
			this.lbUseYn.TabIndex = 39;
			this.lbUseYn.Text = "ªÁøÎø©∫Œ";
			this.lbUseYn.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// lbModDt
			// 
			this.lbModDt.Location = new System.Drawing.Point(576, 88);
			this.lbModDt.Name = "lbModDt";
			this.lbModDt.Size = new System.Drawing.Size(72, 21);
			this.lbModDt.TabIndex = 37;
			this.lbModDt.Text = "ºˆ¡§¿œΩ√";
			this.lbModDt.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// lbRegDt
			// 
			this.lbRegDt.Location = new System.Drawing.Point(576, 64);
			this.lbRegDt.Name = "lbRegDt";
			this.lbRegDt.Size = new System.Drawing.Size(72, 21);
			this.lbRegDt.TabIndex = 29;
			this.lbRegDt.Text = "µÓ∑œ¿œΩ√";
			this.lbRegDt.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// lbRapName
			// 
			this.lbRapName.Location = new System.Drawing.Point(296, 32);
			this.lbRapName.Name = "lbRapName";
			this.lbRapName.Size = new System.Drawing.Size(72, 21);
			this.lbRapName.TabIndex = 30;
			this.lbRapName.Text = "πÃµæÓ∑æ";
			this.lbRapName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// lbMediaName
			// 
			this.lbMediaName.Location = new System.Drawing.Point(296, 8);
			this.lbMediaName.Name = "lbMediaName";
			this.lbMediaName.Size = new System.Drawing.Size(72, 21);
			this.lbMediaName.TabIndex = 19;
			this.lbMediaName.Text = "∏≈√º";
			this.lbMediaName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// lbAgencyName
			// 
			this.lbAgencyName.Location = new System.Drawing.Point(8, 8);
			this.lbAgencyName.Name = "lbAgencyName";
			this.lbAgencyName.Size = new System.Drawing.Size(72, 21);
			this.lbAgencyName.TabIndex = 19;
			this.lbAgencyName.Text = "¥Î«‡ªÁ";
			this.lbAgencyName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// lbAdvertiserName
			// 
			this.lbAdvertiserName.Location = new System.Drawing.Point(584, 8);
			this.lbAdvertiserName.Name = "lbAdvertiserName";
			this.lbAdvertiserName.Size = new System.Drawing.Size(72, 21);
			this.lbAdvertiserName.TabIndex = 30;
			this.lbAdvertiserName.Text = "±§∞Ì¡÷";
			this.lbAdvertiserName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// btnDelete
			// 
			this.btnDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.btnDelete.Enabled = false;
			this.btnDelete.Location = new System.Drawing.Point(120, 169);
			this.btnDelete.Name = "btnDelete";
			this.btnDelete.Size = new System.Drawing.Size(104, 24);
			this.btnDelete.TabIndex = 18;
			this.btnDelete.Text = "ªË ¡¶";
			this.btnDelete.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
			this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
			// 
			// btnAdd
			// 
			this.btnAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.btnAdd.Enabled = false;
			this.btnAdd.Location = new System.Drawing.Point(232, 169);
			this.btnAdd.Name = "btnAdd";
			this.btnAdd.Size = new System.Drawing.Size(104, 24);
			this.btnAdd.TabIndex = 19;
			this.btnAdd.Text = "√ﬂ ∞°";
			this.btnAdd.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
			this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
			// 
			// btnSave
			// 
			this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.btnSave.Enabled = false;
			this.btnSave.Location = new System.Drawing.Point(8, 169);
			this.btnSave.Name = "btnSave";
			this.btnSave.Size = new System.Drawing.Size(104, 24);
			this.btnSave.TabIndex = 17;
			this.btnSave.Text = "¿˙ ¿Â";
			this.btnSave.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
			this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
			// 
			// btnAdvertiserSearch
			// 
			this.btnAdvertiserSearch.Enabled = false;
			this.btnAdvertiserSearch.Location = new System.Drawing.Point(648, 32);
			this.btnAdvertiserSearch.Name = "btnAdvertiserSearch";
			this.btnAdvertiserSearch.Size = new System.Drawing.Size(104, 24);
			this.btnAdvertiserSearch.TabIndex = 21;
			this.btnAdvertiserSearch.Text = "±§∞Ì¡÷º±≈√";
			this.btnAdvertiserSearch.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
			this.btnAdvertiserSearch.Click += new System.EventHandler(this.btnAdvertiserSearch_Click);
			// 
			// uiComboBox1
			// 
			this.uiComboBox1.Location = new System.Drawing.Point(0, 0);
			this.uiComboBox1.Name = "uiComboBox1";
			this.uiComboBox1.Size = new System.Drawing.Size(176, 21);
			this.uiComboBox1.TabIndex = 0;
			// 
			// uiButton1
			// 
			this.uiButton1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.uiButton1.Location = new System.Drawing.Point(136, 8);
			this.uiButton1.Name = "uiButton1";
			this.uiButton1.Size = new System.Drawing.Size(112, 24);
			this.uiButton1.TabIndex = 5;
			this.uiButton1.Text = "¿˙ ¿Â";
			this.uiButton1.VisualStyle = Janus.Windows.UI.VisualStyle.Office2003;
			// 
			// uiButton2
			// 
			this.uiButton2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.uiButton2.Location = new System.Drawing.Point(8, 8);
			this.uiButton2.Name = "uiButton2";
			this.uiButton2.Size = new System.Drawing.Size(120, 24);
			this.uiButton2.TabIndex = 6;
			this.uiButton2.Text = "√ﬂ ∞°";
			this.uiButton2.VisualStyle = Janus.Windows.UI.VisualStyle.Office2003;
			// 
			// ClientControl
			// 
			this.Controls.Add(this.uiPanelClients);
			this.Font = global::AdManagerClient.Properties.Settings.Default.Font1;
			this.Name = "ClientControl";
			this.Size = new System.Drawing.Size(1010, 677);
			this.Load += new System.EventHandler(this.ClientControl_Load);
			((System.ComponentModel.ISupportInitialize)(this.uiPM)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.uiPanelClients)).EndInit();
			this.uiPanelClients.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.uiPanelClientsSearch)).EndInit();
			this.uiPanelClientsSearch.ResumeLayout(false);
			this.uiPanelClientsSearchContainer.ResumeLayout(false);
			this.pnlSearch.ResumeLayout(false);
			this.pnlSearch.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.uiPanelClientList)).EndInit();
			this.uiPanelClientList.ResumeLayout(false);
			this.uiPanelClientListContainer.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.grdExClientList)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.dvClient)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.clientDs)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.uiPanelClientDetail)).EndInit();
			this.uiPanelClientDetail.ResumeLayout(false);
			this.uiPanelClientDetailContainer.ResumeLayout(false);
			this.pnlClientDetail.ResumeLayout(false);
			this.pnlClientDetail.PerformLayout();
			this.ResumeLayout(false);

		}
		#endregion

		#region ƒ¡∆Æ∑— ∑ŒµÂ
		private void ClientControl_Load(object sender, System.EventArgs e)
		{
			// µ•¿Ã≈Õ∞¸∏ÆøÎ ∞¥√ºª˝º∫
			dt = ((DataView)grdExClientList.DataSource).Table;  
			cm = (CurrencyManager) this.BindingContext[grdExClientList.DataSource]; 
			cm.PositionChanged += new System.EventHandler(OnGrdRowChanged); 

			// ƒ¡∆Æ∑— √ ±‚»≠
			InitControl();		
		}

		#endregion

		#region ƒ¡∆Æ∑— √ ±‚»≠
		private void InitControl()
		{
            ProgressStart();
			InitCombo();
			InitCombo_Rap();	
			InitCombo_Agency();
			InitCombo_Advertiser();
			InitCombo_Level();	
           

			// ¡∂»∏±««— ∞ÀªÁ
			if(menu.CanRead(MenuCode))
			{
				canRead = true;
				SearchClient();
				
			}
			
			// √ﬂ∞°πˆ∆∞ »∞º∫»≠
			if(menu.CanCreate(MenuCode))
			{
				canCreate = true;
			}

			// ªË¡¶πˆ∆∞ »∞º∫»≠
			if(menu.CanDelete(MenuCode))
			{
				canDelete = true;
			}

			// ¿˙¿Âπˆ∆∞ »∞º∫»≠
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
            ProgressStop();
		}

		private void InitCombo()
		{
			// ƒ⁄µÂø°º≠ ∫∏æ»∑π∫ß¿ª ¡∂»∏«—¥Ÿ.
			MediaCodeModel mediacodeModel = new MediaCodeModel();
			//MediacodeModel.Section = "21";				// ƒ⁄µÂ∫–∑˘ '11':∫∏æ»∑π∫ß  TODO: ƒ⁄µÂ∫–∑˘¥¬ √ﬂ»ƒ XML∑Œ ∞¸∏Æµ«æÓæﬂ...
			new MediaCodeManager(systemModel, commonModel).GetMediaCodeList(mediacodeModel);
			
			if (mediacodeModel.ResultCD.Equals("0000"))
			{
				// µ•¿Ã≈Õº¬ø° º¬∆√
				Utility.SetDataTable(clientDs.Medias, mediacodeModel.MediaCodeDataSet);				
			}

			// ªÛºº¡∂»∏ ƒﬁ∫∏
			// ªÛºº¡§∫∏¿« ƒﬁ∫∏¥¬ Dataset¿ª µ•¿Ã≈Õº“Ω∫∑Œ ∞°¡¯¥Ÿ.

			// ∞Àªˆ¡∂∞«¿« ƒﬁ∫∏
			this.cbSearchMediaName.Items.Clear();
			
			// ƒﬁ∫∏π⁄Ω∫ø° º¬∆Æ«“ ƒ⁄µÂ∏Ò∑œ¿ª ¥„¿ª ItemπËø≠¿ª º±æ
			Janus.Windows.EditControls.UIComboBoxItem[] comboItems = new Janus.Windows.EditControls.UIComboBoxItem[mediacodeModel.ResultCnt + 1];

			comboItems[0] = new Janus.Windows.EditControls.UIComboBoxItem("∏≈√ºº±≈√","00");
			
			for(int i=0;i<mediacodeModel.ResultCnt;i++)
			{
				DataRow row = clientDs.Medias.Rows[i];

				string val = row["MediaCode"].ToString();
				string txt = row["MediaName"].ToString();
				comboItems[i+1] = new Janus.Windows.EditControls.UIComboBoxItem(txt,val);
			}
			// ƒﬁ∫∏ø° º¬∆Æ
			this.cbSearchMediaName.Items.AddRange(comboItems);
			this.cbSearchMediaName.SelectedIndex = 0;			
			//MessageBox.Show(cbSearchMediaName.SelectedValue.ToString());
			Application.DoEvents();
		}

		private void InitCombo_Rap()
		{
            /* ∑¶ªÁ ∏πŸ¿œ∑Œ ∞Ì¡§
			// ƒ⁄µÂø°º≠ ∫∏æ»∑π∫ß¿ª ¡∂»∏«—¥Ÿ.
			MediaRapCodeModel mediarapcodeModel = new MediaRapCodeModel();
			//codeModel.Section = "21";				// ƒ⁄µÂ∫–∑˘ '11':∫∏æ»∑π∫ß  TODO: ƒ⁄µÂ∫–∑˘¥¬ √ﬂ»ƒ XML∑Œ ∞¸∏Æµ«æÓæﬂ...
			new MediaRapCodeManager(systemModel, commonModel).GetMediaRapCodeList(mediarapcodeModel);
			
			if (mediarapcodeModel.ResultCD.Equals("0000"))
			{
				// µ•¿Ã≈Õº¬ø° º¬∆√
				Utility.SetDataTable(clientDs.MediaRaps, mediarapcodeModel.MediaRapCodeDataSet);				
			}

			// ªÛºº¡∂»∏ ƒﬁ∫∏
			// ªÛºº¡§∫∏¿« ƒﬁ∫∏¥¬ Dataset¿ª µ•¿Ã≈Õº“Ω∫∑Œ ∞°¡¯¥Ÿ.

			// ∞Àªˆ¡∂∞«¿« ƒﬁ∫∏
			this.cbSearchRapName.Items.Clear();
			
			// ƒﬁ∫∏π⁄Ω∫ø° º¬∆Æ«“ ƒ⁄µÂ∏Ò∑œ¿ª ¥„¿ª ItemπËø≠¿ª º±æ
			Janus.Windows.EditControls.UIComboBoxItem[] comboItems = new Janus.Windows.EditControls.UIComboBoxItem[mediarapcodeModel.ResultCnt + 1];

			comboItems[0] = new Janus.Windows.EditControls.UIComboBoxItem("πÃµæÓ∑æº±≈√","00");
			
			for(int i=0;i<mediarapcodeModel.ResultCnt;i++)
			{
				DataRow row = clientDs.MediaRaps.Rows[i];

				string val = row["RapCode"].ToString();
				string txt = row["RapName"].ToString();
				comboItems[i+1] = new Janus.Windows.EditControls.UIComboBoxItem(txt,val);
			}
			// ƒﬁ∫∏ø° º¬∆Æ
			this.cbSearchRapName.Items.AddRange(comboItems);
			this.cbSearchRapName.SelectedIndex = 0;
			//MessageBox.Show(cbSearchRapName.SelectedValue.ToString());
			Application.DoEvents();
            */
            this.cbSearchRapName.Items.Clear();
            DataSet ds = new DataSet("rapset");
            DataTable dt = new DataTable("rap");
            dt.Columns.Add("RapCode", typeof(string));
            dt.Columns.Add("RapName", typeof(string));
            DataRow nRow = dt.NewRow();
            nRow["RapCode"] = "1";
            nRow["RapName"] = "∏πŸ¿œ ∆Ìº∫∆¿";
            dt.Rows.Add(nRow);
            ds.Tables.Add(dt);

            Utility.SetDataTable(clientDs.MediaRaps, ds);
            // ∞Àªˆ¡∂∞«¿« ƒﬁ∫∏
            this.cbSearchRapName.Items.Clear();
            // ƒﬁ∫∏π⁄Ω∫ø° º¬∆Æ«“ ƒ⁄µÂ∏Ò∑œ¿ª ¥„¿ª ItemπËø≠¿ª º±æ
            Janus.Windows.EditControls.UIComboBoxItem[] comboItems = new Janus.Windows.EditControls.UIComboBoxItem[2];

            comboItems[0] = new Janus.Windows.EditControls.UIComboBoxItem("πÃµæÓ∑æº±≈√", "00");

            for (int i = 0; i < 1; i++)
            {
                DataRow row = clientDs.MediaRaps.Rows[i];

                string val = row["RapCode"].ToString();
                string txt = row["RapName"].ToString();
                comboItems[i + 1] = new Janus.Windows.EditControls.UIComboBoxItem(txt, val);
            }
            // ƒﬁ∫∏ø° º¬∆Æ
            this.cbSearchRapName.Items.AddRange(comboItems);
            this.cbSearchRapName.SelectedIndex = 1;
            this.cbSearchRapName.ReadOnly = true;
            Application.DoEvents();

		}

		private void InitCombo_Agency()
		{
			// ƒ⁄µÂø°º≠ ∫∏æ»∑π∫ß¿ª ¡∂»∏«—¥Ÿ.
			AgencyCodeModel agencycodeModel = new AgencyCodeModel();
			//codeModel.Section = "21";				// ƒ⁄µÂ∫–∑˘ '11':∫∏æ»∑π∫ß  TODO: ƒ⁄µÂ∫–∑˘¥¬ √ﬂ»ƒ XML∑Œ ∞¸∏Æµ«æÓæﬂ...
			new AgencyCodeManager(systemModel, commonModel).GetAgencyCodeList(agencycodeModel);
			
			if (agencycodeModel.ResultCD.Equals("0000"))
			{
				// µ•¿Ã≈Õº¬ø° º¬∆√
				Utility.SetDataTable(clientDs.Agencys, agencycodeModel.AgencyCodeDataSet);				
			}

			// ªÛºº¡∂»∏ ƒﬁ∫∏
			// ªÛºº¡§∫∏¿« ƒﬁ∫∏¥¬ Dataset¿ª µ•¿Ã≈Õº“Ω∫∑Œ ∞°¡¯¥Ÿ.

			// ∞Àªˆ¡∂∞«¿« ƒﬁ∫∏
			this.cbSearchMediaAgency.Items.Clear();
			
			// ƒﬁ∫∏π⁄Ω∫ø° º¬∆Æ«“ ƒ⁄µÂ∏Ò∑œ¿ª ¥„¿ª ItemπËø≠¿ª º±æ
			Janus.Windows.EditControls.UIComboBoxItem[] comboItems = new Janus.Windows.EditControls.UIComboBoxItem[agencycodeModel.ResultCnt + 1];

			comboItems[0] = new Janus.Windows.EditControls.UIComboBoxItem("¥Î«‡ªÁº±≈√","00");
			
			for(int i=0;i<agencycodeModel.ResultCnt;i++)
			{
				DataRow row = clientDs.Agencys.Rows[i];

				string val = row["AgencyCode"].ToString();
				string txt = row["AgencyName"].ToString();
				comboItems[i+1] = new Janus.Windows.EditControls.UIComboBoxItem(txt,val);
			}
			
			// ƒﬁ∫∏ø° º¬∆Æ
			this.cbSearchMediaAgency.Items.AddRange(comboItems);
			this.cbSearchMediaAgency.SelectedIndex = 0;
			Application.DoEvents();
		}

		private void InitCombo_Level()
		{
			
            if(commonModel.UserLevel=="20")
            {
                cbSearchMediaName.SelectedValue = commonModel.MediaCode;			
                cbSearchMediaName.ReadOnly = true;					
            }
            else
            {
				for(int i=0;i < clientDs.Medias.Rows.Count;i++)
				{
					DataRow row = clientDs.Medias.Rows[i];					
					if(row["MediaCode"].ToString().Equals(FrameSystem._HANATV.ToString()))
					{
						cbSearchMediaName.SelectedValue = FrameSystem._HANATV; // «œ≥™TV∏¶ ±‚∫ª∞™¿∏∑Œ «—¥Ÿ.	 		
						break;															
					}
					else
					{
						cbSearchMediaName.SelectedValue="00";
					}
				}	
            }
			if(commonModel.UserLevel=="30")
			{
				cbSearchRapName.SelectedValue = commonModel.RapCode;			
				cbSearchRapName.ReadOnly = true;				
			}
			if(commonModel.UserLevel=="40")
			{
				cbSearchMediaAgency.SelectedValue = commonModel.AgencyCode;			
				cbSearchMediaAgency.ReadOnly = true;					
			}	
			Application.DoEvents();		
		}

		private void InitCombo_Advertiser()
		{			
//			ClientModel clientModel = new ClientModel();			
//			new ClientManager(systemModel, commonModel).GetAdvertiserList(clientModel);
//			
//			if (clientModel.ResultCD.Equals("0000"))
//			{
//				// µ•¿Ã≈Õº¬ø° º¬∆√
//				Utility.SetDataTable(clientDs.Advertisers, clientModel.ClientDataSet);				
//			}		
//			// ∞Àªˆ¡∂∞«¿« ƒﬁ∫∏
//			this.ebSearchAdvertiserName.Items.Clear();
//			
//			// ƒﬁ∫∏π⁄Ω∫ø° º¬∆Æ«“ ƒ⁄µÂ∏Ò∑œ¿ª ¥„¿ª ItemπËø≠¿ª º±æ
//			Janus.Windows.EditControls.UIComboBoxItem[] comboItems = new Janus.Windows.EditControls.UIComboBoxItem[clientModel.ResultCnt + 1];
//
//			comboItems[0] = new Janus.Windows.EditControls.UIComboBoxItem("±§∞Ì¡÷º±≈√","00");
//			
//			for(int i=0;i<clientModel.ResultCnt;i++)
//			{
//				DataRow row = clientDs.Advertisers.Rows[i];
//
//				string val = row["AdvertiserCode"].ToString();
//				string txt = row["AdvertiserName"].ToString();
//				comboItems[i+1] = new Janus.Windows.EditControls.UIComboBoxItem(txt,val);
//			}
//			
//			// ƒﬁ∫∏ø° º¬∆Æ
//			this.ebSearchAdvertiserName.Items.AddRange(comboItems);
//			this.ebSearchAdvertiserName.SelectedIndex = 0;
//			Application.DoEvents();
		}

		private void InitButton()
		{
			if(canRead)   btnSearch.Enabled = true;
			if(canRead)   btnAdvertiserSearch_1.Enabled = true;
			if(canCreate) btnAdd.Enabled    = true;
			
			if(!advertiserCode.Equals(""))
			{
				if(canDelete) btnDelete.Enabled = true;
				if(canUpdate) btnSave.Enabled   = true;
			}
			Application.DoEvents();
		}

		private void DisableButton()
		{
			btnSearch.Enabled = false;
			btnAdvertiserSearch_1.Enabled = false;
			btnAdd.Enabled    = false;
			btnSave.Enabled   = false;
			btnDelete.Enabled = false;
			Application.DoEvents();
		}

		#endregion

		#region ∏≈√º¥Î«‡±§∞Ì¡÷ æ◊º«√≥∏Æ ∏ﬁº“µÂ

		/// <summary>
		/// ±◊∏ÆµÂ¿« Row∫Ø∞ÊΩ√
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void OnGrdRowChanged(object sender, System.EventArgs e) 
		{
            if (!IsSearching) // 2011.11.29 JH.Park ¡∂»∏¡ﬂ¿Ã æ∆¥“∞ÊøÏø°∏∏ µø¿€«œµµ∑œ ∫Ø∞Ê
            {
                if (grdExClientList.RecordCount > 0)
                {
                    SetClientDetailText();
                }
                InitButton();
            }

		}

		/// <summary>
		/// ¡∂»∏πˆ∆∞ ≈¨∏Ø
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnSearch_Click(object sender, System.EventArgs e)
		{
			if(cbSearchMediaName.SelectedValue.ToString() == "00") 
			{
				FrameSystem.showMsgForm("Client¡§∫∏∞Àªˆ ø¿∑˘",new string[] {"", "∏≈√º∏¶ º±≈√«œø© ¡÷ººø‰.", "" });
				return;
			}
	
            ProgressStart();
			
            ReSetGridData();
			DisableButton();			
			SearchClient();			
			InitButton();

            ProgressStop();
		}

		/// <summary>
		/// √ﬂ∞°πˆ∆∞ ≈¨∏Ø
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
			ResetClientDetailText();

			ebComment.Focus();
		}

		/// <summary>
		/// ¿˙¿Âπˆ∆∞ ≈¨∏Ø
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnSave_Click(object sender, System.EventArgs e)
		{			
			SaveClientDetail();
		}

		/// <summary>
		/// ªË¡¶πˆ∆∞ ≈¨∏Ø
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnDelete_Click(object sender, System.EventArgs e)
		{
			DeleteClient();
		}


		/// <summary>
		/// ∞ÀªˆæÓ ∫Ø∞Ê
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void ebSearchKey_TextChanged(object sender, System.EventArgs e)
		{
			IsNewSearchKey = false;
		}

		/// <summary>
		/// ∞ÀªˆæÓ ≈¨∏Ø 
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
				SearchClient();
			}
		}

		private void ReSetGridData()
		{
			clientDs.Clients.Clear();        
		}

		//¿Ã ∏ﬁº“µÂ∞° Ω««‡¿Ã µ«∏È «ÿ¥Á « µÂ¿« ∆Àæ˜¿Ã »£√‚µ»¥Ÿ.
		private void ResetPop(object flag)
		{
			if(flag.Equals("client"))
			{
				Client_pForm pForm = new Client_pForm(this);
				pForm.ShowDialog();            
				pForm.Dispose();
				pForm = null;		
			}
			if(flag.Equals("advertiser"))
			{
				Advertiser_pForm advertiserForm = new Advertiser_pForm(this);
				advertiserForm.ShowDialog();            
				advertiserForm.Dispose();
				advertiserForm = null;		
			}
		}

		private void btnAgencySearch_Click(object sender, System.EventArgs e)
		{
			// ±§∞Ì∞Ëæ‡ ∏Ò∑œ ∞Àªˆ ∆Àæ˜ ∂ÏøÏ±‚
			Client_pForm pForm = new Client_pForm(this);

			pForm.ShowDialog();
            
			pForm.Dispose();
			pForm = null;		
		}

		#endregion

		#region √≥∏Æ∏ﬁº“µÂ

		/// <summary>
		/// ∏≈√º¥Î«‡±§∞Ì¡÷∏Ò∑œ ¡∂»∏
		/// </summary>
		private void SearchClient()
		{
            IsSearching = true;

			StatusMessage("∏≈√º¥Î«‡±§∞Ì¡÷ ¡§∫∏∏¶ ¡∂»∏«’¥œ¥Ÿ.");
			
			try
			{
				clientModel.Init();
				// µ•¿Ã≈Õ∏µ®ø° ¿¸º€«“ ≥ªøÎ¿ª º¬∆Æ«—¥Ÿ.
				if(IsNewSearchKey)
				{
					clientModel.SearchKey = "";
				}
				else
				{
					clientModel.SearchKey  = ebSearchKey.Text;
				}				
				
				clientModel.SearchMediaName = cbSearchMediaName.SelectedItem.Value.ToString();
				clientModel.SearchRapName = cbSearchRapName.SelectedItem.Value.ToString();
				clientModel.SearchMediaAgency = cbSearchMediaAgency.SelectedItem.Value.ToString();
				clientModel.SearchAdvertiserName = Search_advertiserCode;

                if (uiCheckBox1.Checked)
				{
					clientModel.SearchchkAdState_10   = "Y";
				}
				else
				{
					clientModel.SearchchkAdState_10   = "N";
				}
				
				LastOrder = 0;


				// ∏≈√º¥Î«‡±§∞Ì¡÷∏Ò∑œ¡∂»∏ º≠∫ÒΩ∫∏¶ »£√‚«—¥Ÿ.
				new ClientManager(systemModel,commonModel).GetClientList(clientModel);
//				Search_advertiserCode = "";
//				ebSearchAdvertiserName.Text = "";

				if (clientModel.ResultCD.Equals("0000"))
				{					
					Utility.SetDataTable(clientDs.Clients, clientModel.ClientDataSet);				
					StatusMessage(clientModel.ResultCnt + "∞«¿« ∏≈√º¥Î«‡±§∞Ì¡÷ ¡§∫∏∞° ¡∂»∏µ«æ˙Ω¿¥œ¥Ÿ.");
					LastOrder = clientModel.ResultCnt;
					
					if(canUpdate)
					{
						AddSchChoice();
					}
					SetClientDetailText();
				}
			}
			catch(FrameException fe)
			{
				FrameSystem.showMsgForm("∏≈√º¥Î«‡±§∞Ì¡÷¡∂»∏ø¿∑˘", new string[] {fe.ErrCode, fe.ResultMsg});
			}
			catch(Exception ex)
			{
				FrameSystem.showMsgForm("∏≈√º¥Î«‡±§∞Ì¡÷¡∂»∏ø¿∑˘",new string[] {"",ex.Message});
			}
            finally
            {
                IsSearching = false; // ¡∂»∏¡ﬂ Flag ∏Æº¬
            }
		}

		/// <summary>
		/// ≈∞Øì¿ª√£æ∆ ±◊∏ÆµÂ ≈∞ø° «ÿ¥Áµ«¥¬∑ŒøÏ∑Œ..
		/// </summary>
		private void AddSchChoice()
		{
			StatusMessage("≈∞Øì");		

			try
			{
				int rowIndex = 0;
				if ( clientDs.Tables["Clients"].Rows.Count < 1 ) return;
              
				foreach (DataRow row in clientDs.Tables["Clients"].Rows)
				{					
					if(IsAdding)
					{
						cm.Position = 0;
						mediaCode = "";
						rapCode = "";
						agencyCode = "";
						advertiserCode = "";
					}
					else
					{						
						if(row["MediaCode"].ToString().Equals(mediaCode) && row["RapCode"].ToString().Equals(rapCode) && row["AgencyCode"].ToString().Equals(agencyCode) && row["AdvertiserCode"].ToString().Equals(advertiserCode))
						{					
							cm.Position = rowIndex;
							break;								
						}
					}

					rowIndex++;
					grdExClientList.EnsureVisible();
				}
			}
			catch(FrameException fe)
			{
				FrameSystem.showMsgForm("≈∞Øìø¿∑˘", new string[] {fe.ErrCode, fe.ResultMsg});
			}
			catch(Exception ex)
			{
				FrameSystem.showMsgForm("≈∞Øìø¿∑˘",new string[] {"",ex.Message});
			}			
		}

		private void SearchClient_Combo()
		{
//			clientModel.MediaCode_C = cbMediaName.SelectedItem.Value.ToString();
//			clientModel.RapCode_C = cbRapName.SelectedItem.Value.ToString();
//			clientModel.AgencyCode_C = cbAgencyName.SelectedItem.Value.ToString();
//			clientModel.AdvertiserCode_C = ebAdvertiserName.SelectedItem.Value.ToString();
		}

		/// <summary>
		/// ∏≈√º¥Î«‡±§∞Ì¡÷ªÛºº¡§∫∏ ¿˙¿Â
		/// </summary>
		private void SaveClientDetail()
		{
			StatusMessage("∏≈√º¥Î«‡±§∞Ì¡÷ ¡§∫∏∏¶ ¿˙¿Â«’¥œ¥Ÿ.");
			
			if(agencyCode.Length == 0) 
			{
				MessageBox.Show("¥Î«‡ªÁ∞° ¿‘∑¬µ«¡ˆ æ æ“Ω¿¥œ¥Ÿ.","∏≈√º∫∞ ±§∞Ì¡÷ ¿˙¿Â", 
					MessageBoxButtons.OK, MessageBoxIcon.Information );
					ResetPop("client");
				return;				
			}
			if(mediaCode.Length == 0) 
			{
				MessageBox.Show("∏≈√º∞° ¿‘∑¬µ«¡ˆ æ æ“Ω¿¥œ¥Ÿ.","∏≈√º∫∞ ±§∞Ì¡÷ ¿˙¿Â", 
					MessageBoxButtons.OK, MessageBoxIcon.Information );
					ResetPop("client");
				return;				
			}
			if(rapCode.Length == 0) 
			{
				MessageBox.Show("πÃµæÓ∑æ¿Ã ¿‘∑¬µ«¡ˆ æ æ“Ω¿¥œ¥Ÿ.","∏≈√º∫∞ ±§∞Ì¡÷ ¿˙¿Â", 
					MessageBoxButtons.OK, MessageBoxIcon.Information );
					ResetPop("client");
				return;				
			}
			if(advertiserCode.Length == 0) 
			{
				MessageBox.Show("±§∞Ì¡÷∞° ¿‘∑¬µ«¡ˆ æ æ“Ω¿¥œ¥Ÿ.","∏≈√º∫∞ ±§∞Ì¡÷ ¿˙¿Â", 
					MessageBoxButtons.OK, MessageBoxIcon.Information );
					ResetPop("advertiser");
				return;				
			}		

			try
			{
				// µ•¿Ã≈Õ∏µ®ø° ¿¸º€«“ ≥ªøÎ¿ª º¬∆Æ«—¥Ÿ.
				clientModel.MediaCode_C       = mediaCode;
				clientModel.RapCode_C       = rapCode;
				clientModel.AgencyCode_C       = agencyCode;
				clientModel.AdvertiserCode_C       = advertiserCode;
				clientModel.Comment       = ebComment.Text.Trim();								

				//ªÁøÎø©∫Œ
				if(rbUseYn_Y.Checked)
				{
					clientModel.UseYn       = "Y";
				}
				else
				{
					clientModel.UseYn       = "N";
				}
				
				// ∏≈√º¥Î«‡±§∞Ì¡÷ ªÛºº¡§∫∏ ¿˙¿Â º≠∫ÒΩ∫∏¶ »£√‚«—¥Ÿ.
				if (IsAdding)
				{
					new ClientManager(systemModel,commonModel).SetClientAdd(clientModel);
                    StatusMessage("∏≈√º¥Î«‡±§∞Ì¡÷ ¡§∫∏∞° √ﬂ∞°µ«æ˙Ω¿¥œ¥Ÿ.");
                    IsAdding = false;
                    ResetClientDetailText();
                }
				else
				{
					new ClientManager(systemModel,commonModel).SetClientUpdate(clientModel);
                    StatusMessage("∏≈√º¥Î«‡±§∞Ì¡÷ ¡§∫∏∞° ¿˙¿Âµ«æ˙Ω¿¥œ¥Ÿ.");
                }

                DisableButton();
                SearchClient();
                InitButton();			
			}
			catch(FrameException fe)
			{
				FrameSystem.showMsgForm("∏≈√º¥Î«‡±§∞Ì¡÷¡§∫∏ ¿˙¿Âø¿∑˘", new string[] {fe.ErrCode, fe.ResultMsg});
			}
			catch(Exception ex)
			{
				FrameSystem.showMsgForm("∏≈√º¥Î«‡±§∞Ì¡÷¡§∫∏ ¿˙¿Âø¿∑˘",new string[] {"",ex.Message});
			}			
		}

		/// <summary>
		/// ∏≈√º¥Î«‡±§∞Ì¡÷¡§∫∏ ªË¡¶
		/// </summary>
		private void DeleteClient()
		{
			StatusMessage("∏≈√º¥Î«‡±§∞Ì¡÷ ¡§∫∏∏¶ ªË¡¶«’¥œ¥Ÿ.");

			DialogResult result = MessageBox.Show("«ÿ¥Á ∏≈√º¥Î«‡±§∞Ì¡÷ ¡§∫∏∏¶ ªË¡¶ «œΩ√∞⁄Ω¿¥œ±Ó?","∏≈√º¥Î«‡±§∞Ì¡÷ ªË¡¶",
				MessageBoxButtons.YesNo, MessageBoxIcon.Question,
				MessageBoxDefaultButton.Button2);

			if (result == DialogResult.No) return;

			try 
			{
				// µ•¿Ã≈Õ∏µ®ø° ¿¸º€«“ ≥ªøÎ¿ª º¬∆Æ«—¥Ÿ.
				clientModel.MediaCode       = mediaCode;
				clientModel.RapCode       = rapCode;
				clientModel.AgencyCode       = agencyCode;
				clientModel.AdvertiserCode       = advertiserCode;

				// ∏≈√º¥Î«‡±§∞Ì¡÷ ªÛºº¡§∫∏ ¿˙¿Â º≠∫ÒΩ∫∏¶ »£√‚«—¥Ÿ.
				new ClientManager(systemModel,commonModel).SetClientDelete(clientModel);
                StatusMessage("∏≈√º¥Î«‡±§∞Ì¡÷ ¡§∫∏∞° ªË¡¶µ«æ˙Ω¿¥œ¥Ÿ.");			
				ResetClientDetailText();
				
                DisableButton();
                SearchClient();
				InitButton();

			}
			catch(FrameException fe)
			{
				FrameSystem.showMsgForm("∏≈√º¥Î«‡±§∞Ì¡÷¡§∫∏ ªË¡¶ø¿∑˘", new string[] {fe.ErrCode, fe.ResultMsg});
			}
			catch(Exception ex)
			{
				FrameSystem.showMsgForm("∏≈√º¥Î«‡±§∞Ì¡÷¡§∫∏ ªË¡¶ø¿∑˘",new string[] {"",ex.Message});
			}			

		}
		
		/// <summary>
		/// ∏≈√º¥Î«‡±§∞Ì¡÷ ªÛºº¡§∫∏¿« º¬∆Æ
		/// </summary>
		private void SetClientDetailText()
		{
			int curRow = cm.Position;
			if(curRow >= 0)
			{
				mediaCode	    = dt.Rows[curRow]["MediaCode"].ToString();			
				rapCode	        = dt.Rows[curRow]["RapCode"].ToString();			
				agencyCode	    = dt.Rows[curRow]["AgencyCode"].ToString();			
				advertiserCode	= dt.Rows[curRow]["AdvertiserCode"].ToString();	
		
				ebMedia.Text    = dt.Rows[curRow]["MediaName"].ToString();			
				ebRap.Text      = dt.Rows[curRow]["RapName"].ToString();			
				ebAgency.Text   = dt.Rows[curRow]["AgencyName"].ToString();						
				ebAdvertiserName.Text      = dt.Rows[curRow]["AdvertiserName"].ToString();		
				
				ebComment.Text  = dt.Rows[curRow]["Comment"].ToString();						
				ebRegDt.Text    = dt.Rows[curRow]["RegDt"].ToString();
				ebModDt.Text    = dt.Rows[curRow]["ModDt"].ToString();
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

			StatusMessage("¡ÿ∫Ò");
		}

		private void ResetClientDetailText()
		{
			mediaCode      = "";
			rapCode        = "";
			agencyCode     = "";
			advertiserCode     = "";
			ebMedia.Text   = "";			
			ebRap.Text     = "";			
			ebAgency.Text  = "";		
			ebAdvertiserName.Text  = "";			
			ebComment.Text = "";			
			rbUseYn_Y.Checked         = true;
			rbUseYn_N.Checked         = false;
				
			ebRegDt.Text   = "";
			ebModDt.Text   = "";
			ebRegID.Text   = "";
			
			if(!IsAdding)
			{

				btnAgencySearch.Enabled     = false;

				ebMedia.ReadOnly   = false;	
				ebRap.ReadOnly   = false;	
				ebAgency.ReadOnly   = false;	
				ebAdvertiserName.ReadOnly   = false;	
				ebComment.ReadOnly          = false;							
				
				rbUseYn_Y.Enabled			= true;
				rbUseYn_N.Enabled			= true;
						
				ebMedia.BackColor  = Color.White;
				ebRap.BackColor  = Color.White;
				ebAgency.BackColor  = Color.White;
				ebAdvertiserName.BackColor  = Color.White;
				ebComment.BackColor         = Color.White;			
			
			}
		}
		
		/// <summary>
		/// ªÛºº¡§∫∏ ReadOnly
		/// </summary>
		private void SetTextReadonly()
		{

			btnAgencySearch.Enabled    = false;
			btnAdvertiserSearch.Enabled    = false;

			ebMedia.ReadOnly   = true;	
			ebRap.ReadOnly   = true;	
			ebAgency.ReadOnly   = true;
			ebAdvertiserName.ReadOnly  = true;	
			ebComment.ReadOnly         = true;									
			
			rbUseYn_Y.Enabled          = false;
			rbUseYn_N.Enabled          = false;
						
			ebMedia.BackColor  = Color.WhiteSmoke;
			ebRap.BackColor  = Color.WhiteSmoke;
			ebAgency.BackColor  = Color.WhiteSmoke;
			ebAdvertiserName.BackColor = Color.WhiteSmoke;	
			ebComment.BackColor        = Color.WhiteSmoke;			
				
		}

		/// <summary>
		/// ªÛºº¡§∫∏ ºˆ¡§∞°¥…ƒ…
		/// </summary>
		private void ResetTextReadonly()
		{			

			ebComment.ReadOnly          = false;																				
			ebComment.BackColor        = Color.White;			

			rbUseYn_Y.Enabled         = true;
			rbUseYn_N.Enabled         = true;

			// Ω≈±‘¿€º∫¿Ã∏È æ∆¿Ãµ±Ó¡ˆ æ≤±‚∞°¥…
			if (IsAdding)
			{
				btnAgencySearch.Enabled    = true;
				btnAdvertiserSearch.Enabled    = true;

				ebMedia.ReadOnly   = true;	
				ebRap.ReadOnly   = true;	
				ebAgency.ReadOnly   = true;
				ebAdvertiserName.ReadOnly  = true;	

				ebMedia.BackColor  = Color.White;
				ebRap.BackColor  = Color.White;
				ebAgency.BackColor  = Color.White;
				ebAdvertiserName.BackColor  = Color.White;
				ebComment.BackColor         = Color.White;		
			}
			else
			{
				btnAgencySearch.Enabled     = false;
				btnAdvertiserSearch.Enabled    = false;

				ebMedia.ReadOnly   = false;	
				ebRap.ReadOnly   = false;	
				ebAgency.ReadOnly   = false;	
				ebAdvertiserName.ReadOnly   = false;	

				ebMedia.BackColor  = Color.WhiteSmoke;
				ebRap.BackColor  = Color.WhiteSmoke;
				ebAgency.BackColor  = Color.WhiteSmoke;
				ebAdvertiserName.BackColor = Color.WhiteSmoke;	
			}
		}
		#endregion

		#region ¿Ã∫•∆Æ«‘ºˆ

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

		#region ø‹∫Œ≥Î√‚ ∏ﬁº“µÂ
		/// <summary>
		/// º±≈√µ» RowµÈ¿ª ¿‘∑¬Ω√≈¥
		/// </summary>
		/// <param name="dtc"></param>
		public void adOn_AddContract(ClientModel clientModel )
		{
			mediaCode       = clientModel.MediaCode;
			rapCode         = clientModel.RapCode;
			agencyCode      = clientModel.AgencyCode;

			ebMedia.Text        = clientModel.MediaName;
			ebRap.Text          = clientModel.RapName;
			ebAgency.Text       = clientModel.AgencyName;
		}

		private void ResetAdvertiser(object flag)
		{
			//±§∞Ì¡÷º±≈√ ¿‘∑¬ ∆Àæ˜
			if(flag.Equals("Advertiser"))
			{
				// ±§∞Ì¡÷º±≈√ ∞Àªˆ ∆Àæ˜ ∂ÏøÏ±‚
				Advertiser_pForm advertiserForm = new Advertiser_pForm(this, "Advertiser");
				advertiserForm.ShowDialog();            
				advertiserForm.Dispose();
				advertiserForm = null;		
			}
			//±§∞Ì¡÷º±≈√ ∞Àªˆ ∆Àæ˜
			if(flag.Equals("SearchAdvertiser"))
			{
				Advertiser_pForm Search_advertiserForm = new Advertiser_pForm(this, "SearchAdvertiser");
				Search_advertiserForm.ShowDialog();            
				Search_advertiserForm.Dispose();
				Search_advertiserForm = null;		
			}
		}

		private void btnAdvertiserSearch_Click(object sender, System.EventArgs e)
		{
			// ±§∞Ì¡÷º±≈√ ¿‘∑¬ ∆Àæ˜ ∂ÁøÏ±‚
			ResetAdvertiser("Advertiser");
		}		
		
		public string AdvertiserCode
		{
			set
			{
				this.advertiserCode = value;
			}
		}

		public string AdvertiserName
		{				
			set
			{
				this.ebAdvertiserName.Text = value;
			}			
		}

		private void btnAdvertiserSearch_1_Click(object sender, System.EventArgs e)
		{
			// ±§∞Ì¡÷º±≈√ ∞Àªˆ ∆Àæ˜ ∂ÁøÏ±‚
			ResetAdvertiser("SearchAdvertiser");
		}

		public string SearchAdvertiserCode
		{
			set
			{
				this.Search_advertiserCode = value;
			}
		}

		public string SearchAdvertiserName
		{				
			set
			{
				this.ebSearchAdvertiserName.Text = value;
			}			
		}


		#endregion	
		
	}
}

