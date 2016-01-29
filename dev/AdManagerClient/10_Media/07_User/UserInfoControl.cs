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
	public class UserInfoControl : System.Windows.Forms.UserControl, IUserControl
	{
		#region ÀÌº¥Æ®ÇÚµé·¯
		public event StatusEventHandler 			StatusEvent;			// »óÅÂÀÌº¥Æ® ÇÚµé·¯
		public event ProgressEventHandler 			ProgressEvent;			// Ã³¸®ÁßÀÌº¥Æ® ÇÚµé·¯
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
			
		#region »ç¿ëÀÚÁ¤ÀÇ °´Ã¼ ¹× º¯¼ö

		// ½Ã½ºÅÛ Á¤º¸ : È­¸é°øÅë
		private SystemModel   systemModel   = FrameSystem.oSysModel;
		private CommonModel   commonModel   = FrameSystem.oComModel;
		private Logger        log           = FrameSystem.oLog;
		private MenuPower     menu          = FrameSystem.oMenu;

		public string        menuCode		= "";

		// »ç¿ëÇÒ Á¤º¸¸ðµ¨
		UserInfoModel usersModel  = new UserInfoModel();	// »ç¿ëÀÚÁ¤º¸¸ðµ¨

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

		private string        userId = null;

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
		private Janus.Windows.GridEX.EditControls.EditBox ebUserComment;
		private Janus.Windows.GridEX.EditControls.EditBox ebUserMobile;
		private System.Windows.Forms.Label lbUserTell;
		private Janus.Windows.GridEX.EditControls.EditBox ebUserTell;
		private System.Windows.Forms.Label lbUserTitle;
		private Janus.Windows.GridEX.EditControls.EditBox ebUserTitle;
		private Janus.Windows.GridEX.EditControls.EditBox ebUserDept;
		private System.Windows.Forms.Label lbUserDept;
		private Janus.Windows.EditControls.UIComboBox cbUserLevel;
		private System.Windows.Forms.Label lbUserLevel;
		private Janus.Windows.GridEX.EditControls.EditBox ebUserID;
		private System.Windows.Forms.Label lbUserID;
		private Janus.Windows.GridEX.EditControls.EditBox ebUserName;
		private System.Windows.Forms.Label lbUserName;
		private System.Windows.Forms.Label lbMobile;
		private Janus.Windows.GridEX.EditControls.EditBox ebLastLogin;
		private System.Windows.Forms.Label lbLastLogin;
		private System.Windows.Forms.Label lbRegDt;
		private System.Windows.Forms.Label lbUserComment;
		private Janus.Windows.EditControls.UIButton btnAdd;
		private Janus.Windows.EditControls.UIButton btnSave;
		private System.Windows.Forms.Label lbUserPassword;
		private Janus.Windows.GridEX.EditControls.EditBox ebUserPassword;
		private System.Data.DataView dvUserInfo;
		private Janus.Windows.GridEX.EditControls.EditBox ebRegDt;
		private Janus.Windows.EditControls.UIButton btnDelete;
		private AdManagerClient.UserInfoDs UserInfoDs;
		private System.Windows.Forms.Label lbUserEmail;
		private Janus.Windows.GridEX.EditControls.EditBox ebUserEMail;
		private System.Windows.Forms.Label lbUseYn;
		private System.Windows.Forms.Label lbModDt;
		private Janus.Windows.GridEX.EditControls.EditBox ebModDt;
		private Janus.Windows.EditControls.UIRadioButton rbUseYn_Y;
		private Janus.Windows.EditControls.UIRadioButton rbUseYn_N;
		private System.Windows.Forms.Label lbUserClass;
		private Janus.Windows.EditControls.UIComboBox cbUserClass;
		private Janus.Windows.EditControls.UIComboBox cbSearchUserClass;
		private Janus.Windows.EditControls.UIComboBox cbSearchUserLevel;
		private Janus.Windows.GridEX.GridEX grdExUserList;
		private System.Windows.Forms.Label lbMedia;
		private System.Windows.Forms.Label lbRap;
		private System.Windows.Forms.Label lbAgnecy;
		private Janus.Windows.EditControls.UIComboBox cbMedia;
		private Janus.Windows.EditControls.UIComboBox cbRap;
        private Janus.Windows.EditControls.UIComboBox cbAgency;
		private System.ComponentModel.IContainer components;

		public UserInfoControl()
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
			Janus.Windows.GridEX.GridEXLayout grdExUserList_DesignTimeLayout = new Janus.Windows.GridEX.GridEXLayout();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UserInfoControl));
			this.uiPM = new Janus.Windows.UI.Dock.UIPanelManager(this.components);
			this.uiPanelUsers = new Janus.Windows.UI.Dock.UIPanelGroup();
			this.uiPanelUsersSearch = new Janus.Windows.UI.Dock.UIPanel();
			this.uiPanelUsersSearchContainer = new Janus.Windows.UI.Dock.UIPanelInnerContainer();
			this.pnlSearch = new System.Windows.Forms.Panel();
			this.uiCheckBox1 = new Janus.Windows.EditControls.UICheckBox();
			this.cbSearchUserLevel = new Janus.Windows.EditControls.UIComboBox();
			this.cbSearchUserClass = new Janus.Windows.EditControls.UIComboBox();
			this.ebSearchKey = new Janus.Windows.GridEX.EditControls.EditBox();
			this.btnSearch = new Janus.Windows.EditControls.UIButton();
			this.uiPanelUserList = new Janus.Windows.UI.Dock.UIPanel();
			this.uiPanelUserListContainer = new Janus.Windows.UI.Dock.UIPanelInnerContainer();
			this.grdExUserList = new Janus.Windows.GridEX.GridEX();
			this.dvUserInfo = new System.Data.DataView();
			this.UserInfoDs = new AdManagerClient.UserInfoDs();
			this.uiPanelUserDetail = new Janus.Windows.UI.Dock.UIPanel();
			this.uiPanelUserDetailContainer = new Janus.Windows.UI.Dock.UIPanelInnerContainer();
			this.pnlUserDetail = new System.Windows.Forms.Panel();
			this.cbRap = new Janus.Windows.EditControls.UIComboBox();
			this.ebUserDept = new Janus.Windows.GridEX.EditControls.EditBox();
			this.lbUserDept = new System.Windows.Forms.Label();
			this.rbUseYn_N = new Janus.Windows.EditControls.UIRadioButton();
			this.rbUseYn_Y = new Janus.Windows.EditControls.UIRadioButton();
			this.lbUseYn = new System.Windows.Forms.Label();
			this.ebUserEMail = new Janus.Windows.GridEX.EditControls.EditBox();
			this.lbUserEmail = new System.Windows.Forms.Label();
			this.ebRegDt = new Janus.Windows.GridEX.EditControls.EditBox();
			this.ebModDt = new Janus.Windows.GridEX.EditControls.EditBox();
			this.ebUserTell = new Janus.Windows.GridEX.EditControls.EditBox();
			this.ebUserTitle = new Janus.Windows.GridEX.EditControls.EditBox();
			this.ebUserPassword = new Janus.Windows.GridEX.EditControls.EditBox();
			this.lbModDt = new System.Windows.Forms.Label();
			this.ebUserComment = new Janus.Windows.GridEX.EditControls.EditBox();
			this.ebUserMobile = new Janus.Windows.GridEX.EditControls.EditBox();
			this.lbUserTell = new System.Windows.Forms.Label();
			this.lbUserTitle = new System.Windows.Forms.Label();
			this.cbUserLevel = new Janus.Windows.EditControls.UIComboBox();
			this.lbUserLevel = new System.Windows.Forms.Label();
			this.ebUserID = new Janus.Windows.GridEX.EditControls.EditBox();
			this.lbUserID = new System.Windows.Forms.Label();
			this.ebUserName = new Janus.Windows.GridEX.EditControls.EditBox();
			this.lbUserName = new System.Windows.Forms.Label();
			this.lbMobile = new System.Windows.Forms.Label();
			this.ebLastLogin = new Janus.Windows.GridEX.EditControls.EditBox();
			this.lbLastLogin = new System.Windows.Forms.Label();
			this.lbRegDt = new System.Windows.Forms.Label();
			this.lbUserComment = new System.Windows.Forms.Label();
			this.lbUserPassword = new System.Windows.Forms.Label();
			this.cbUserClass = new Janus.Windows.EditControls.UIComboBox();
			this.lbUserClass = new System.Windows.Forms.Label();
			this.cbMedia = new Janus.Windows.EditControls.UIComboBox();
			this.lbMedia = new System.Windows.Forms.Label();
			this.lbRap = new System.Windows.Forms.Label();
			this.cbAgency = new Janus.Windows.EditControls.UIComboBox();
			this.lbAgnecy = new System.Windows.Forms.Label();
			this.btnSave = new Janus.Windows.EditControls.UIButton();
			this.btnDelete = new Janus.Windows.EditControls.UIButton();
			this.btnAdd = new Janus.Windows.EditControls.UIButton();
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
			((System.ComponentModel.ISupportInitialize)(this.grdExUserList)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.dvUserInfo)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.UserInfoDs)).BeginInit();
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
			this.uiPM.AddDockPanelInfo(new System.Guid("afb3b7d0-aa79-443c-a1e8-3baba38f7a4d"), new System.Guid("b162c53a-7940-47cd-a0ed-9762c59c50d8"), 43, true);
			this.uiPM.AddDockPanelInfo(new System.Guid("8e3140b9-bfcb-43af-b84e-2ee6cddf51dc"), new System.Guid("b162c53a-7940-47cd-a0ed-9762c59c50d8"), 394, true);
			this.uiPM.AddDockPanelInfo(new System.Guid("b2973a57-d3e1-4d80-9387-7a6dc3332542"), new System.Guid("b162c53a-7940-47cd-a0ed-9762c59c50d8"), 210, true);
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
			this.uiPanelUsers.Text = "»ç¿ëÀÚ°ü¸®";
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
			this.pnlSearch.Controls.Add(this.uiCheckBox1);
			this.pnlSearch.Controls.Add(this.cbSearchUserLevel);
			this.pnlSearch.Controls.Add(this.cbSearchUserClass);
			this.pnlSearch.Controls.Add(this.ebSearchKey);
			this.pnlSearch.Controls.Add(this.btnSearch);
			this.pnlSearch.Dock = System.Windows.Forms.DockStyle.Fill;
			this.pnlSearch.Location = new System.Drawing.Point(0, 0);
			this.pnlSearch.Name = "pnlSearch";
			this.pnlSearch.Size = new System.Drawing.Size(1008, 41);
			this.pnlSearch.TabIndex = 3;
			// 
			// uiCheckBox1
			// 
			this.uiCheckBox1.Location = new System.Drawing.Point(494, 8);
			this.uiCheckBox1.Name = "uiCheckBox1";
			this.uiCheckBox1.Size = new System.Drawing.Size(104, 23);
			this.uiCheckBox1.TabIndex = 6;
			this.uiCheckBox1.Text = "»ç¿ë¾ÈÇÔ Æ÷ÇÔ";
			// 
			// cbSearchUserLevel
			// 
			this.cbSearchUserLevel.ComboStyle = Janus.Windows.EditControls.ComboStyle.DropDownList;
			this.cbSearchUserLevel.Location = new System.Drawing.Point(144, 8);
			this.cbSearchUserLevel.Name = "cbSearchUserLevel";
			this.cbSearchUserLevel.Size = new System.Drawing.Size(128, 21);
			this.cbSearchUserLevel.TabIndex = 2;
			this.cbSearchUserLevel.Text = "»ç¿ë·¹º§ ÀüÃ¼";
			this.cbSearchUserLevel.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
			// 
			// cbSearchUserClass
			// 
			this.cbSearchUserClass.ComboStyle = Janus.Windows.EditControls.ComboStyle.DropDownList;
			this.cbSearchUserClass.Location = new System.Drawing.Point(8, 8);
			this.cbSearchUserClass.Name = "cbSearchUserClass";
			this.cbSearchUserClass.Size = new System.Drawing.Size(128, 21);
			this.cbSearchUserClass.TabIndex = 1;
			this.cbSearchUserClass.Text = "»ç¿ëÀÚ±¸ºÐ ÀüÃ¼";
			this.cbSearchUserClass.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
			// 
			// ebSearchKey
			// 
			this.ebSearchKey.Location = new System.Drawing.Point(280, 8);
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
			// uiPanelUserList
			// 
			this.uiPanelUserList.BackColor = System.Drawing.SystemColors.Window;
			this.uiPanelUserList.CaptionStyle = Janus.Windows.UI.Dock.PanelCaptionStyle.Dark;
			this.uiPanelUserList.InnerContainer = this.uiPanelUserListContainer;
			this.uiPanelUserList.Location = new System.Drawing.Point(0, 69);
			this.uiPanelUserList.MinimumSize = new System.Drawing.Size(-1, 100);
			this.uiPanelUserList.Name = "uiPanelUserList";
			this.uiPanelUserList.Size = new System.Drawing.Size(1010, 394);
			this.uiPanelUserList.TabIndex = 5;
			this.uiPanelUserList.TabStop = false;
			this.uiPanelUserList.Text = "»ç¿ëÀÚ¸ñ·Ï";
			// 
			// uiPanelUserListContainer
			// 
			this.uiPanelUserListContainer.BackColor = System.Drawing.SystemColors.Window;
			this.uiPanelUserListContainer.Controls.Add(this.grdExUserList);
			this.uiPanelUserListContainer.Location = new System.Drawing.Point(1, 23);
			this.uiPanelUserListContainer.Name = "uiPanelUserListContainer";
			this.uiPanelUserListContainer.Size = new System.Drawing.Size(1008, 370);
			this.uiPanelUserListContainer.TabIndex = 0;
			// 
			// grdExUserList
			// 
			this.grdExUserList.AllowEdit = Janus.Windows.GridEX.InheritableBoolean.False;
			this.grdExUserList.AlternatingColors = true;
			this.grdExUserList.AlternatingRowFormatStyle.BackgroundImageDrawMode = Janus.Windows.GridEX.BackgroundImageDrawMode.Tile;
			this.grdExUserList.BorderStyle = Janus.Windows.GridEX.BorderStyle.None;
			this.grdExUserList.DataSource = this.dvUserInfo;
			grdExUserList_DesignTimeLayout.LayoutString = resources.GetString("grdExUserList_DesignTimeLayout.LayoutString");
			this.grdExUserList.DesignTimeLayout = grdExUserList_DesignTimeLayout;
			this.grdExUserList.Dock = System.Windows.Forms.DockStyle.Fill;
			this.grdExUserList.EditorsControlStyle.ButtonAppearance = Janus.Windows.GridEX.ButtonAppearance.Regular;
			this.grdExUserList.EmptyRows = true;
			this.grdExUserList.FocusCellFormatStyle.BackColor = System.Drawing.SystemColors.Highlight;
			this.grdExUserList.FocusCellFormatStyle.ForeColor = System.Drawing.Color.White;
			this.grdExUserList.FocusStyle = Janus.Windows.GridEX.FocusStyle.None;
			this.grdExUserList.FrozenColumns = 2;
			this.grdExUserList.GridLineColor = System.Drawing.Color.Silver;
			this.grdExUserList.GridLines = Janus.Windows.GridEX.GridLines.Vertical;
			this.grdExUserList.GridLineStyle = Janus.Windows.GridEX.GridLineStyle.Solid;
			this.grdExUserList.GroupByBoxVisible = false;
			this.grdExUserList.HeaderFormatStyle.TextAlignment = Janus.Windows.GridEX.TextAlignment.Center;
			this.grdExUserList.HideSelection = Janus.Windows.GridEX.HideSelection.HighlightInactive;
			this.grdExUserList.Location = new System.Drawing.Point(0, 0);
			this.grdExUserList.Name = "grdExUserList";
			this.grdExUserList.ScrollBars = Janus.Windows.GridEX.ScrollBars.Vertical;
			this.grdExUserList.SelectedInactiveFormatStyle.BackColor = System.Drawing.Color.Gold;
			this.grdExUserList.Size = new System.Drawing.Size(1008, 370);
			this.grdExUserList.TabIndex = 6;
			this.grdExUserList.TabKeyBehavior = Janus.Windows.GridEX.TabKeyBehavior.ControlNavigation;
			this.grdExUserList.ThemedAreas = ((Janus.Windows.GridEX.ThemedArea)(((((((((Janus.Windows.GridEX.ThemedArea.ScrollBars | Janus.Windows.GridEX.ThemedArea.EditControls) 
            | Janus.Windows.GridEX.ThemedArea.Headers) 
            | Janus.Windows.GridEX.ThemedArea.GroupByBox) 
            | Janus.Windows.GridEX.ThemedArea.GroupRows) 
            | Janus.Windows.GridEX.ThemedArea.ControlBorder) 
            | Janus.Windows.GridEX.ThemedArea.Cards) 
            | Janus.Windows.GridEX.ThemedArea.Gridlines) 
            | Janus.Windows.GridEX.ThemedArea.CheckBoxes)));
			this.grdExUserList.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
			this.grdExUserList.Enter += new System.EventHandler(this.OnGrdRowChanged);
			// 
			// dvUserInfo
			// 
			this.dvUserInfo.Table = this.UserInfoDs.Users;
			// 
			// UserInfoDs
			// 
			this.UserInfoDs.DataSetName = "UserInfoDs";
			this.UserInfoDs.Locale = new System.Globalization.CultureInfo("en-US");
			this.UserInfoDs.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
			// 
			// uiPanelUserDetail
			// 
			this.uiPanelUserDetail.AllowResize = Janus.Windows.UI.InheritableBoolean.False;
			this.uiPanelUserDetail.BackColor = System.Drawing.SystemColors.Window;
			this.uiPanelUserDetail.CaptionStyle = Janus.Windows.UI.Dock.PanelCaptionStyle.Dark;
			this.uiPanelUserDetail.InnerContainer = this.uiPanelUserDetailContainer;
			this.uiPanelUserDetail.Location = new System.Drawing.Point(0, 467);
			this.uiPanelUserDetail.Name = "uiPanelUserDetail";
			this.uiPanelUserDetail.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.uiPanelUserDetail.Size = new System.Drawing.Size(1010, 210);
			this.uiPanelUserDetail.TabIndex = 7;
			this.uiPanelUserDetail.TabStop = false;
			this.uiPanelUserDetail.Text = "»ó¼¼Á¤º¸";
			// 
			// uiPanelUserDetailContainer
			// 
			this.uiPanelUserDetailContainer.Controls.Add(this.pnlUserDetail);
			this.uiPanelUserDetailContainer.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.uiPanelUserDetailContainer.Location = new System.Drawing.Point(1, 23);
			this.uiPanelUserDetailContainer.Name = "uiPanelUserDetailContainer";
			this.uiPanelUserDetailContainer.Size = new System.Drawing.Size(1008, 186);
			this.uiPanelUserDetailContainer.TabIndex = 0;
			// 
			// pnlUserDetail
			// 
			this.pnlUserDetail.BackColor = System.Drawing.SystemColors.Window;
			this.pnlUserDetail.Controls.Add(this.cbRap);
			this.pnlUserDetail.Controls.Add(this.ebUserDept);
			this.pnlUserDetail.Controls.Add(this.lbUserDept);
			this.pnlUserDetail.Controls.Add(this.rbUseYn_N);
			this.pnlUserDetail.Controls.Add(this.rbUseYn_Y);
			this.pnlUserDetail.Controls.Add(this.lbUseYn);
			this.pnlUserDetail.Controls.Add(this.ebUserEMail);
			this.pnlUserDetail.Controls.Add(this.lbUserEmail);
			this.pnlUserDetail.Controls.Add(this.ebRegDt);
			this.pnlUserDetail.Controls.Add(this.ebModDt);
			this.pnlUserDetail.Controls.Add(this.ebUserTell);
			this.pnlUserDetail.Controls.Add(this.ebUserTitle);
			this.pnlUserDetail.Controls.Add(this.ebUserPassword);
			this.pnlUserDetail.Controls.Add(this.lbModDt);
			this.pnlUserDetail.Controls.Add(this.ebUserComment);
			this.pnlUserDetail.Controls.Add(this.ebUserMobile);
			this.pnlUserDetail.Controls.Add(this.lbUserTell);
			this.pnlUserDetail.Controls.Add(this.lbUserTitle);
			this.pnlUserDetail.Controls.Add(this.cbUserLevel);
			this.pnlUserDetail.Controls.Add(this.lbUserLevel);
			this.pnlUserDetail.Controls.Add(this.ebUserID);
			this.pnlUserDetail.Controls.Add(this.lbUserID);
			this.pnlUserDetail.Controls.Add(this.ebUserName);
			this.pnlUserDetail.Controls.Add(this.lbUserName);
			this.pnlUserDetail.Controls.Add(this.lbMobile);
			this.pnlUserDetail.Controls.Add(this.ebLastLogin);
			this.pnlUserDetail.Controls.Add(this.lbLastLogin);
			this.pnlUserDetail.Controls.Add(this.lbRegDt);
			this.pnlUserDetail.Controls.Add(this.lbUserComment);
			this.pnlUserDetail.Controls.Add(this.lbUserPassword);
			this.pnlUserDetail.Controls.Add(this.cbUserClass);
			this.pnlUserDetail.Controls.Add(this.lbUserClass);
			this.pnlUserDetail.Controls.Add(this.cbMedia);
			this.pnlUserDetail.Controls.Add(this.lbMedia);
			this.pnlUserDetail.Controls.Add(this.lbRap);
			this.pnlUserDetail.Controls.Add(this.cbAgency);
			this.pnlUserDetail.Controls.Add(this.lbAgnecy);
			this.pnlUserDetail.Controls.Add(this.btnSave);
			this.pnlUserDetail.Controls.Add(this.btnDelete);
			this.pnlUserDetail.Controls.Add(this.btnAdd);
			this.pnlUserDetail.Dock = System.Windows.Forms.DockStyle.Fill;
			this.pnlUserDetail.Location = new System.Drawing.Point(0, 0);
			this.pnlUserDetail.Name = "pnlUserDetail";
			this.pnlUserDetail.Size = new System.Drawing.Size(1008, 186);
			this.pnlUserDetail.TabIndex = 3;
			// 
			// cbRap
			// 
			this.cbRap.ComboStyle = Janus.Windows.EditControls.ComboStyle.DropDownList;
			this.cbRap.Location = new System.Drawing.Point(87, 104);
			this.cbRap.Name = "cbRap";
			this.cbRap.Size = new System.Drawing.Size(176, 21);
			this.cbRap.TabIndex = 22;
			this.cbRap.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
			// 
			// ebUserDept
			// 
			this.ebUserDept.Location = new System.Drawing.Point(376, 80);
			this.ebUserDept.MaxLength = 100;
			this.ebUserDept.Name = "ebUserDept";
			this.ebUserDept.Size = new System.Drawing.Size(176, 21);
			this.ebUserDept.TabIndex = 20;
			this.ebUserDept.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
			this.ebUserDept.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
			// 
			// lbUserDept
			// 
			this.lbUserDept.Location = new System.Drawing.Point(304, 80);
			this.lbUserDept.Name = "lbUserDept";
			this.lbUserDept.Size = new System.Drawing.Size(72, 21);
			this.lbUserDept.TabIndex = 24;
			this.lbUserDept.Text = "¼Ò¼ÓºÎ¼­";
			this.lbUserDept.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.lbUserDept.Click += new System.EventHandler(this.lbUserDept_Click);
			// 
			// rbUseYn_N
			// 
			this.rbUseYn_N.BackColor = System.Drawing.Color.Transparent;
			this.rbUseYn_N.Location = new System.Drawing.Point(744, 56);
			this.rbUseYn_N.Name = "rbUseYn_N";
			this.rbUseYn_N.Size = new System.Drawing.Size(82, 23);
			this.rbUseYn_N.TabIndex = 18;
			this.rbUseYn_N.Text = "»ç¿ë¾ÈÇÔ";
			this.rbUseYn_N.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
			// 
			// rbUseYn_Y
			// 
			this.rbUseYn_Y.BackColor = System.Drawing.Color.Transparent;
			this.rbUseYn_Y.Location = new System.Drawing.Point(664, 56);
			this.rbUseYn_Y.Name = "rbUseYn_Y";
			this.rbUseYn_Y.Size = new System.Drawing.Size(82, 23);
			this.rbUseYn_Y.TabIndex = 17;
			this.rbUseYn_Y.Text = "»ç¿ëÇÔ";
			this.rbUseYn_Y.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
			// 
			// lbUseYn
			// 
			this.lbUseYn.Location = new System.Drawing.Point(592, 56);
			this.lbUseYn.Name = "lbUseYn";
			this.lbUseYn.Size = new System.Drawing.Size(72, 21);
			this.lbUseYn.TabIndex = 34;
			this.lbUseYn.Text = "»ç¿ë¿©ºÎ";
			this.lbUseYn.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.lbUseYn.Click += new System.EventHandler(this.lbUseYn_Click);
			// 
			// ebUserEMail
			// 
			this.ebUserEMail.Location = new System.Drawing.Point(664, 32);
			this.ebUserEMail.Name = "ebUserEMail";
			this.ebUserEMail.Size = new System.Drawing.Size(186, 21);
			this.ebUserEMail.TabIndex = 14;
			this.ebUserEMail.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
			this.ebUserEMail.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
			// 
			// lbUserEmail
			// 
			this.lbUserEmail.Location = new System.Drawing.Point(592, 32);
			this.lbUserEmail.Name = "lbUserEmail";
			this.lbUserEmail.Size = new System.Drawing.Size(72, 21);
			this.lbUserEmail.TabIndex = 5;
			this.lbUserEmail.Text = "Email";
			this.lbUserEmail.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.lbUserEmail.Click += new System.EventHandler(this.lbUserEmail_Click);
			// 
			// ebRegDt
			// 
			this.ebRegDt.BackColor = System.Drawing.Color.WhiteSmoke;
			this.ebRegDt.Location = new System.Drawing.Point(664, 80);
			this.ebRegDt.Name = "ebRegDt";
			this.ebRegDt.ReadOnly = true;
			this.ebRegDt.Size = new System.Drawing.Size(186, 21);
			this.ebRegDt.TabIndex = 21;
			this.ebRegDt.TabStop = false;
			this.ebRegDt.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
			this.ebRegDt.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
			// 
			// ebModDt
			// 
			this.ebModDt.BackColor = System.Drawing.Color.WhiteSmoke;
			this.ebModDt.Location = new System.Drawing.Point(664, 104);
			this.ebModDt.Name = "ebModDt";
			this.ebModDt.ReadOnly = true;
			this.ebModDt.Size = new System.Drawing.Size(186, 21);
			this.ebModDt.TabIndex = 24;
			this.ebModDt.TabStop = false;
			this.ebModDt.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
			this.ebModDt.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
			// 
			// ebUserTell
			// 
			this.ebUserTell.Location = new System.Drawing.Point(376, 32);
			this.ebUserTell.MaxLength = 15;
			this.ebUserTell.Name = "ebUserTell";
			this.ebUserTell.Size = new System.Drawing.Size(176, 21);
			this.ebUserTell.TabIndex = 13;
			this.ebUserTell.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
			this.ebUserTell.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
			// 
			// ebUserTitle
			// 
			this.ebUserTitle.Location = new System.Drawing.Point(376, 104);
			this.ebUserTitle.MaxLength = 10;
			this.ebUserTitle.Name = "ebUserTitle";
			this.ebUserTitle.Size = new System.Drawing.Size(176, 21);
			this.ebUserTitle.TabIndex = 23;
			this.ebUserTitle.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
			this.ebUserTitle.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
			// 
			// ebUserPassword
			// 
			this.ebUserPassword.Location = new System.Drawing.Point(664, 8);
			this.ebUserPassword.MaxLength = 10;
			this.ebUserPassword.Name = "ebUserPassword";
			this.ebUserPassword.PasswordChar = '*';
			this.ebUserPassword.Size = new System.Drawing.Size(186, 21);
			this.ebUserPassword.TabIndex = 11;
			this.ebUserPassword.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
			this.ebUserPassword.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
			// 
			// lbModDt
			// 
			this.lbModDt.Location = new System.Drawing.Point(592, 104);
			this.lbModDt.Name = "lbModDt";
			this.lbModDt.Size = new System.Drawing.Size(72, 21);
			this.lbModDt.TabIndex = 37;
			this.lbModDt.Text = "¼öÁ¤ÀÏ½Ã";
			this.lbModDt.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// ebUserComment
			// 
			this.ebUserComment.Location = new System.Drawing.Point(376, 128);
			this.ebUserComment.Multiline = true;
			this.ebUserComment.Name = "ebUserComment";
			this.ebUserComment.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.ebUserComment.Size = new System.Drawing.Size(475, 48);
			this.ebUserComment.TabIndex = 26;
			this.ebUserComment.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
			this.ebUserComment.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
			// 
			// ebUserMobile
			// 
			this.ebUserMobile.Location = new System.Drawing.Point(376, 56);
			this.ebUserMobile.MaxLength = 15;
			this.ebUserMobile.Name = "ebUserMobile";
			this.ebUserMobile.Size = new System.Drawing.Size(176, 21);
			this.ebUserMobile.TabIndex = 16;
			this.ebUserMobile.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
			this.ebUserMobile.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
			// 
			// lbUserTell
			// 
			this.lbUserTell.Location = new System.Drawing.Point(304, 32);
			this.lbUserTell.Name = "lbUserTell";
			this.lbUserTell.Size = new System.Drawing.Size(72, 21);
			this.lbUserTell.TabIndex = 30;
			this.lbUserTell.Text = "ÀüÈ­¹øÈ£";
			this.lbUserTell.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.lbUserTell.Click += new System.EventHandler(this.lbUserTell_Click);
			// 
			// lbUserTitle
			// 
			this.lbUserTitle.Location = new System.Drawing.Point(304, 104);
			this.lbUserTitle.Name = "lbUserTitle";
			this.lbUserTitle.Size = new System.Drawing.Size(72, 21);
			this.lbUserTitle.TabIndex = 27;
			this.lbUserTitle.Text = "Á÷Ã¥Á÷ÇÔ";
			this.lbUserTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.lbUserTitle.Click += new System.EventHandler(this.lbUserTitle_Click);
			// 
			// cbUserLevel
			// 
			this.cbUserLevel.ComboStyle = Janus.Windows.EditControls.ComboStyle.DropDownList;
			this.cbUserLevel.DataSource = this.UserInfoDs.Levels;
			this.cbUserLevel.DisplayMember = "CodeName";
			this.cbUserLevel.Location = new System.Drawing.Point(87, 56);
			this.cbUserLevel.Name = "cbUserLevel";
			this.cbUserLevel.Size = new System.Drawing.Size(176, 21);
			this.cbUserLevel.TabIndex = 15;
			this.cbUserLevel.ValueMember = "Code";
			this.cbUserLevel.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
			this.cbUserLevel.SelectedIndexChanged += new System.EventHandler(this.cbUseLevel_SelectedIndexChanged);
			// 
			// lbUserLevel
			// 
			this.lbUserLevel.Location = new System.Drawing.Point(8, 32);
			this.lbUserLevel.Name = "lbUserLevel";
			this.lbUserLevel.Size = new System.Drawing.Size(72, 21);
			this.lbUserLevel.TabIndex = 22;
			this.lbUserLevel.Text = "»ç¿ë±¸ºÐ";
			this.lbUserLevel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.lbUserLevel.Click += new System.EventHandler(this.lbUserLevel_Click);
			// 
			// ebUserID
			// 
			this.ebUserID.BackColor = System.Drawing.Color.WhiteSmoke;
			this.ebUserID.Location = new System.Drawing.Point(87, 8);
			this.ebUserID.MaxLength = 10;
			this.ebUserID.Name = "ebUserID";
			this.ebUserID.ReadOnly = true;
			this.ebUserID.Size = new System.Drawing.Size(176, 21);
			this.ebUserID.TabIndex = 9;
			this.ebUserID.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
			this.ebUserID.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
			// 
			// lbUserID
			// 
			this.lbUserID.Location = new System.Drawing.Point(8, 8);
			this.lbUserID.Name = "lbUserID";
			this.lbUserID.Size = new System.Drawing.Size(72, 21);
			this.lbUserID.TabIndex = 18;
			this.lbUserID.Text = "»ç¿ëÀÚID";
			this.lbUserID.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.lbUserID.Click += new System.EventHandler(this.lbUserID_Click);
			// 
			// ebUserName
			// 
			this.ebUserName.Location = new System.Drawing.Point(376, 8);
			this.ebUserName.MaxLength = 20;
			this.ebUserName.Name = "ebUserName";
			this.ebUserName.Size = new System.Drawing.Size(176, 21);
			this.ebUserName.TabIndex = 10;
			this.ebUserName.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
			this.ebUserName.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
			// 
			// lbUserName
			// 
			this.lbUserName.Location = new System.Drawing.Point(304, 8);
			this.lbUserName.Name = "lbUserName";
			this.lbUserName.Size = new System.Drawing.Size(72, 21);
			this.lbUserName.TabIndex = 19;
			this.lbUserName.Text = "»ç¿ëÀÚ¸í";
			this.lbUserName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.lbUserName.Click += new System.EventHandler(this.lbUserName_Click);
			// 
			// lbMobile
			// 
			this.lbMobile.Location = new System.Drawing.Point(304, 56);
			this.lbMobile.Name = "lbMobile";
			this.lbMobile.Size = new System.Drawing.Size(72, 21);
			this.lbMobile.TabIndex = 29;
			this.lbMobile.Text = "ÈÞ´ëÀüÈ­";
			this.lbMobile.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.lbMobile.Click += new System.EventHandler(this.lbMobile_Click);
			// 
			// ebLastLogin
			// 
			this.ebLastLogin.BackColor = System.Drawing.Color.WhiteSmoke;
			this.ebLastLogin.Location = new System.Drawing.Point(87, 152);
			this.ebLastLogin.Name = "ebLastLogin";
			this.ebLastLogin.ReadOnly = true;
			this.ebLastLogin.Size = new System.Drawing.Size(176, 21);
			this.ebLastLogin.TabIndex = 27;
			this.ebLastLogin.TabStop = false;
			this.ebLastLogin.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
			this.ebLastLogin.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
			// 
			// lbLastLogin
			// 
			this.lbLastLogin.Location = new System.Drawing.Point(8, 150);
			this.lbLastLogin.Name = "lbLastLogin";
			this.lbLastLogin.Size = new System.Drawing.Size(72, 21);
			this.lbLastLogin.TabIndex = 29;
			this.lbLastLogin.Text = "ÃÖ±ÙÁ¢¼Ó";
			this.lbLastLogin.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.lbLastLogin.Click += new System.EventHandler(this.lbLastLogin_Click);
			// 
			// lbRegDt
			// 
			this.lbRegDt.Location = new System.Drawing.Point(592, 80);
			this.lbRegDt.Name = "lbRegDt";
			this.lbRegDt.Size = new System.Drawing.Size(72, 21);
			this.lbRegDt.TabIndex = 29;
			this.lbRegDt.Text = "µî·ÏÀÏ½Ã";
			this.lbRegDt.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// lbUserComment
			// 
			this.lbUserComment.Location = new System.Drawing.Point(304, 128);
			this.lbUserComment.Name = "lbUserComment";
			this.lbUserComment.Size = new System.Drawing.Size(72, 21);
			this.lbUserComment.TabIndex = 32;
			this.lbUserComment.Text = "ºñ°í";
			this.lbUserComment.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.lbUserComment.Click += new System.EventHandler(this.lbUserComment_Click);
			// 
			// lbUserPassword
			// 
			this.lbUserPassword.Location = new System.Drawing.Point(592, 8);
			this.lbUserPassword.Name = "lbUserPassword";
			this.lbUserPassword.Size = new System.Drawing.Size(72, 21);
			this.lbUserPassword.TabIndex = 19;
			this.lbUserPassword.Text = "ºñ¹Ð¹øÈ£";
			this.lbUserPassword.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.lbUserPassword.Click += new System.EventHandler(this.lbUserPassword_Click);
			// 
			// cbUserClass
			// 
			this.cbUserClass.ComboStyle = Janus.Windows.EditControls.ComboStyle.DropDownList;
			this.cbUserClass.Location = new System.Drawing.Point(87, 32);
			this.cbUserClass.Name = "cbUserClass";
			this.cbUserClass.Size = new System.Drawing.Size(176, 21);
			this.cbUserClass.TabIndex = 12;
			this.cbUserClass.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
			// 
			// lbUserClass
			// 
			this.lbUserClass.Location = new System.Drawing.Point(8, 56);
			this.lbUserClass.Name = "lbUserClass";
			this.lbUserClass.Size = new System.Drawing.Size(72, 21);
			this.lbUserClass.TabIndex = 22;
			this.lbUserClass.Text = "»ç¿ëµî±Þ";
			this.lbUserClass.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.lbUserClass.Click += new System.EventHandler(this.lbUserClass_Click);
			// 
			// cbMedia
			// 
			this.cbMedia.ComboStyle = Janus.Windows.EditControls.ComboStyle.DropDownList;
			this.cbMedia.Location = new System.Drawing.Point(87, 80);
			this.cbMedia.Name = "cbMedia";
			this.cbMedia.Size = new System.Drawing.Size(176, 21);
			this.cbMedia.TabIndex = 19;
			this.cbMedia.Visible = false;
			this.cbMedia.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
			// 
			// lbMedia
			// 
			this.lbMedia.Location = new System.Drawing.Point(8, 80);
			this.lbMedia.Name = "lbMedia";
			this.lbMedia.Size = new System.Drawing.Size(72, 21);
			this.lbMedia.TabIndex = 22;
			this.lbMedia.Text = "¸ÅÃ¼";
			this.lbMedia.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.lbMedia.Visible = false;
			this.lbMedia.Click += new System.EventHandler(this.lbMedia_Click);
			// 
			// lbRap
			// 
			this.lbRap.Location = new System.Drawing.Point(8, 104);
			this.lbRap.Name = "lbRap";
			this.lbRap.Size = new System.Drawing.Size(72, 21);
			this.lbRap.TabIndex = 22;
			this.lbRap.Text = "¹Ìµð¾î·¾";
			this.lbRap.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.lbRap.Click += new System.EventHandler(this.lbRap_Click);
			// 
			// cbAgency
			// 
			this.cbAgency.ComboStyle = Janus.Windows.EditControls.ComboStyle.DropDownList;
			this.cbAgency.Location = new System.Drawing.Point(87, 128);
			this.cbAgency.Name = "cbAgency";
			this.cbAgency.Size = new System.Drawing.Size(176, 21);
			this.cbAgency.TabIndex = 25;
			this.cbAgency.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
			// 
			// lbAgnecy
			// 
			this.lbAgnecy.Location = new System.Drawing.Point(8, 127);
			this.lbAgnecy.Name = "lbAgnecy";
			this.lbAgnecy.Size = new System.Drawing.Size(72, 21);
			this.lbAgnecy.TabIndex = 22;
			this.lbAgnecy.Text = "´ëÇà»ç";
			this.lbAgnecy.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.lbAgnecy.Click += new System.EventHandler(this.lbAgnecy_Click);
			// 
			// btnSave
			// 
			this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.btnSave.Enabled = false;
			this.btnSave.Location = new System.Drawing.Point(886, 8);
			this.btnSave.Name = "btnSave";
			this.btnSave.Size = new System.Drawing.Size(104, 24);
			this.btnSave.TabIndex = 28;
			this.btnSave.Text = "Àú Àå";
			this.btnSave.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
			this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
			// 
			// btnDelete
			// 
			this.btnDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.btnDelete.Enabled = false;
			this.btnDelete.Location = new System.Drawing.Point(886, 38);
			this.btnDelete.Name = "btnDelete";
			this.btnDelete.Size = new System.Drawing.Size(104, 24);
			this.btnDelete.TabIndex = 29;
			this.btnDelete.Text = "»è Á¦";
			this.btnDelete.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
			this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
			// 
			// btnAdd
			// 
			this.btnAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.btnAdd.Enabled = false;
			this.btnAdd.Location = new System.Drawing.Point(886, 68);
			this.btnAdd.Name = "btnAdd";
			this.btnAdd.Size = new System.Drawing.Size(104, 24);
			this.btnAdd.TabIndex = 30;
			this.btnAdd.Text = "Ãß °¡";
			this.btnAdd.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
			this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
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
			// UserInfoControl
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.Controls.Add(this.uiPanelUsers);
			this.Font = global::AdManagerClient.Properties.Settings.Default.Font1;
			this.Name = "UserInfoControl";
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
			((System.ComponentModel.ISupportInitialize)(this.grdExUserList)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.dvUserInfo)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.UserInfoDs)).EndInit();
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
			dt = ((DataView)grdExUserList.DataSource).Table;  
			cm = (CurrencyManager) this.BindingContext[grdExUserList.DataSource]; 
			cm.PositionChanged += new System.EventHandler(OnGrdRowChanged); 

			// ÄÁÆ®·Ñ ÃÊ±âÈ­
			InitControl();		
		}

		#endregion

		#region ÄÁÆ®·Ñ ÃÊ±âÈ­
		private void InitControl()
		{			
			ProgressStart();
			InitCombo_Level();
			
			InitCombo_Class();	
			InitCombo_DetailClass();
			//InitCombo_Media();	
			InitCombo_Rap();	
			InitCombo_Agency();


			// Á¶È¸±ÇÇÑ °Ë»ç
			if(menu.CanRead(MenuCode))
			{
				canRead = true;
				SearchUsers();
			}

			// »ç¿ëÀÚ±¸ºÐÀÌ ¾îµå¹Î ¶Ç´Â ¼öÆÛÀ¯ÀúÀÎ °æ¿ì¸¸ ÀüÃ¼  CRUD¿¡ ´ëÇÑ ¼³Á¤À» Ã³¸®ÇÑ´Ù.
			// ´Ù¸¥ »ç¿ëÀÚµéÀº ÀÚ±âÁ¤º¸¸¸À» ¼öÁ¤ÇÒ ¼ö ÀÖµµ·Ï ÇÑ´Ù.
			if(commonModel.UserClass.Equals("10") || commonModel.UserClass.Equals("20"))
			{			
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
			}

			InitButton();
			ProgressStop();
		}
		private void InitCombo_Level()
		{
			// ÄÚµå¿¡¼­ º¸¾È·¹º§À» Á¶È¸ÇÑ´Ù.
			CodeModel codeModel = new CodeModel();
			codeModel.Section = "11";				// ÄÚµåºÐ·ù '11':º¸¾È·¹º§  TODO: ÄÚµåºÐ·ù´Â ÃßÈÄ XML·Î °ü¸®µÇ¾î¾ß...
			new CodeManager(systemModel, commonModel).GetCodeList(codeModel);
			
			if (codeModel.ResultCD.Equals("0000"))
			{
				// µ¥ÀÌÅÍ¼Â¿¡ ¼ÂÆÃ
				Utility.SetDataTable(UserInfoDs.Levels, codeModel.CodeDataSet);				
			}

			// »ó¼¼Á¶È¸ ÄÞº¸
			// »ó¼¼Á¤º¸ÀÇ ÄÞº¸´Â DatasetÀ» µ¥ÀÌÅÍ¼Ò½º·Î °¡Áø´Ù.

			// °Ë»öÁ¶°ÇÀÇ ÄÞº¸
			this.cbSearchUserLevel.Items.Clear();
			
			// ÄÞº¸¹Ú½º¿¡ ¼ÂÆ®ÇÒ ÄÚµå¸ñ·ÏÀ» ´ãÀ» Item¹è¿­À» ¼±¾ð
			Janus.Windows.EditControls.UIComboBoxItem[] comboItems = new Janus.Windows.EditControls.UIComboBoxItem[codeModel.ResultCnt + 1];

			comboItems[0] = new Janus.Windows.EditControls.UIComboBoxItem("»ç¿ëÀÚ·¹º§ ÀüÃ¼","00");
			
			for(int i=0;i<codeModel.ResultCnt;i++)
			{
				DataRow row = UserInfoDs.Levels.Rows[i];

				string val = row["Code"].ToString();
				string txt = row["CodeName"].ToString();
				comboItems[i+1] = new Janus.Windows.EditControls.UIComboBoxItem(txt,val);
			}
			// ÄÞº¸¿¡ ¼ÂÆ®
			this.cbSearchUserLevel.Items.AddRange(comboItems);
			this.cbSearchUserLevel.SelectedIndex = 0;

			Application.DoEvents();
		}

		private void InitCombo_Class()
		{
			// ÄÚµå¿¡¼­ »ç¿ëÀÚ±¸ºÐÀ» Á¶È¸ÇÑ´Ù.
			CodeModel codeModel = new CodeModel();
			codeModel.Section = "12";				// ÄÚµåºÐ·ù '12':»ç¿ëÀÚ±¸ºÐ  TODO: ÄÚµåºÐ·ù´Â ÃßÈÄ XML·Î °ü¸®µÇ¾î¾ß...
			new CodeManager(systemModel, commonModel).GetCodeList(codeModel);
			
			if (codeModel.ResultCD.Equals("0000"))
			{
				// µ¥ÀÌÅÍ¼Â¿¡ ¼ÂÆÃ
				Utility.SetDataTable(UserInfoDs.Class, codeModel.CodeDataSet);				
			}

			// »ó¼¼Á¶È¸ ÄÞº¸
			// »ó¼¼Á¤º¸ÀÇ ÄÞº¸´Â DatasetÀ» µ¥ÀÌÅÍ¼Ò½º·Î °¡Áø´Ù.

			// °Ë»öÁ¶°ÇÀÇ ÄÞº¸
			this.cbSearchUserClass.Items.Clear();
			
			// ÄÞº¸¹Ú½º¿¡ ¼ÂÆ®ÇÒ ÄÚµå¸ñ·ÏÀ» ´ãÀ» Item¹è¿­À» ¼±¾ð
			Janus.Windows.EditControls.UIComboBoxItem[] comboItems = new Janus.Windows.EditControls.UIComboBoxItem[codeModel.ResultCnt];

			int j= 0;
			comboItems[j++] = new Janus.Windows.EditControls.UIComboBoxItem("»ç¿ëÀÚ±¸ºÐ ÀüÃ¼","00");

			for(int i=0;i<codeModel.ResultCnt;i++)
			{
				DataRow row = UserInfoDs.Class.Rows[i];

				string val = row["Code"].ToString();
				string txt = row["CodeName"].ToString();
				
				if(!val.Equals("10"))	// ¾îµå¹ÎÀº Á¦¿ÜÇÑ´Ù.
				{
					comboItems[j++] = new Janus.Windows.EditControls.UIComboBoxItem(txt,val);
				}
			}
			// ÄÞº¸¿¡ ¼ÂÆ®
			this.cbSearchUserClass.Items.AddRange(comboItems);
			this.cbSearchUserClass.SelectedIndex = 0;

			Application.DoEvents();
		}

		private void InitCombo_DetailClass()
		{
			// ÄÚµå¿¡¼­ »ç¿ëÀÚ±¸ºÐÀ» Á¶È¸ÇÑ´Ù.
			CodeModel codeModel = new CodeModel();
			codeModel.Section = "12";				// ÄÚµåºÐ·ù '12':»ç¿ëÀÚ±¸ºÐ  TODO: ÄÚµåºÐ·ù´Â ÃßÈÄ XML·Î °ü¸®µÇ¾î¾ß...
			new CodeManager(systemModel, commonModel).GetCodeList(codeModel);
			
			if (codeModel.ResultCD.Equals("0000"))
			{
				// µ¥ÀÌÅÍ¼Â¿¡ ¼ÂÆÃ
				Utility.SetDataTable(UserInfoDs.Class, codeModel.CodeDataSet);				
			}

			// »ó¼¼Á¶È¸ ÄÞº¸
			// »ó¼¼Á¤º¸ÀÇ ÄÞº¸´Â DatasetÀ» µ¥ÀÌÅÍ¼Ò½º·Î °¡Áø´Ù.

			// °Ë»öÁ¶°ÇÀÇ ÄÞº¸
			this.cbUserClass.Items.Clear();
			
			for(int i=0;i<codeModel.ResultCnt;i++)
			{
				DataRow row = UserInfoDs.Class.Rows[i];

				string val = row["Code"].ToString();
				string txt = row["CodeName"].ToString();

				if(commonModel.UserClass.Equals("10")) // ¾îµå¹ÎÀ¸·Î ·Î±äÇß´Ù¸é
				{
					this.cbUserClass.Items.Add(new Janus.Windows.EditControls.UIComboBoxItem(txt,val));
				}
				else
				{
					if(!val.Equals("10"))	// ¾îµå¹ÎÀº Á¦¿ÜÇÑ´Ù.
					{
						this.cbUserClass.Items.Add(new Janus.Windows.EditControls.UIComboBoxItem(txt,val));
					}
				}

			}
			// ÄÞº¸¿¡ ¼ÂÆ®
			this.cbUserClass.SelectedIndex = 0;

			Application.DoEvents();
		}

		private void InitCombo_Media()
		{
			// ¸ÅÃ¼¸ñ·ÏÀ» Á¶È¸ÇÑ´Ù.
			MediaCodeModel mediacodeModel = new MediaCodeModel();
			new MediaCodeManager(systemModel, commonModel).GetMediaCodeList(mediacodeModel);
			
			if (mediacodeModel.ResultCD.Equals("0000"))
			{
				// µ¥ÀÌÅÍ¼Â¿¡ ¼ÂÆÃ
				Utility.SetDataTable(UserInfoDs.Medias, mediacodeModel.MediaCodeDataSet);				
			}

			// »ó¼¼Á¶È¸ ÄÞº¸
			//this.cbMedia.Items.Clear();
			/*
			// ÄÞº¸¹Ú½º¿¡ ¼ÂÆ®ÇÒ ÄÚµå¸ñ·ÏÀ» ´ãÀ» Item¹è¿­À» ¼±¾ð
			Janus.Windows.EditControls.UIComboBoxItem[] comboItems = new Janus.Windows.EditControls.UIComboBoxItem[mediacodeModel.ResultCnt + 1];

			comboItems[0] = new Janus.Windows.EditControls.UIComboBoxItem("","0");
			
			for(int i=0;i<mediacodeModel.ResultCnt;i++)
			{
				DataRow row = UserInfoDs.Medias.Rows[i];

				string val = row["MediaCode"].ToString();
				string txt = row["MediaName"].ToString();
				comboItems[i+1] = new Janus.Windows.EditControls.UIComboBoxItem(txt,val);
			}
			// ÄÞº¸¿¡ ¼ÂÆ®
			//this.cbMedia.Items.AddRange(comboItems);
			//this.cbMedia.SelectedIndex = 0;
            */
			Application.DoEvents();
		}

		private void InitCombo_Rap()
		{			
			MediaRapCodeModel mediarapcodeModel = new MediaRapCodeModel();			
			new MediaRapCodeManager(systemModel, commonModel).GetMediaRapCodeList(mediarapcodeModel);
			
			if (mediarapcodeModel.ResultCD.Equals("0000"))
			{
				// µ¥ÀÌÅÍ¼Â¿¡ ¼ÂÆÃ
				Utility.SetDataTable(UserInfoDs.MediaRaps, mediarapcodeModel.MediaRapCodeDataSet);				
			}

			// »ó¼¼Á¶È¸ ÄÞº¸
			this.cbRap.Items.Clear();
			
			// ÄÞº¸¹Ú½º¿¡ ¼ÂÆ®ÇÒ ÄÚµå¸ñ·ÏÀ» ´ãÀ» Item¹è¿­À» ¼±¾ð
			Janus.Windows.EditControls.UIComboBoxItem[] comboItems = new Janus.Windows.EditControls.UIComboBoxItem[mediarapcodeModel.ResultCnt + 1];

			comboItems[0] = new Janus.Windows.EditControls.UIComboBoxItem("","0");
			
			for(int i=0;i<mediarapcodeModel.ResultCnt;i++)
			{
				DataRow row = UserInfoDs.MediaRaps.Rows[i];

				string val = row["RapCode"].ToString();
				string txt = row["RapName"].ToString();
				comboItems[i+1] = new Janus.Windows.EditControls.UIComboBoxItem(txt,val);
			}
			// ÄÞº¸¿¡ ¼ÂÆ®
			this.cbRap.Items.AddRange(comboItems);
			this.cbRap.SelectedIndex = 0;

			Application.DoEvents();
		}

		private void InitCombo_Agency()
		{			
			AgencyCodeModel agencycodeModel = new AgencyCodeModel();			
			new AgencyCodeManager(systemModel, commonModel).GetAgencyCodeList(agencycodeModel);
			
			if (agencycodeModel.ResultCD.Equals("0000"))
			{
				// µ¥ÀÌÅÍ¼Â¿¡ ¼ÂÆÃ
				Utility.SetDataTable(UserInfoDs.Agencys, agencycodeModel.AgencyCodeDataSet);				
			}

			// »ó¼¼Á¶È¸ ÄÞº¸
			this.cbAgency.Items.Clear();
			
			// ÄÞº¸¹Ú½º¿¡ ¼ÂÆ®ÇÒ ÄÚµå¸ñ·ÏÀ» ´ãÀ» Item¹è¿­À» ¼±¾ð
			Janus.Windows.EditControls.UIComboBoxItem[] comboItems = new Janus.Windows.EditControls.UIComboBoxItem[agencycodeModel.ResultCnt + 1];

			comboItems[0] = new Janus.Windows.EditControls.UIComboBoxItem("","0");
			
			for(int i=0;i<agencycodeModel.ResultCnt;i++)
			{
				DataRow row = UserInfoDs.Agencys.Rows[i];

				string val = row["AgencyCode"].ToString();
				string txt = row["AgencyName"].ToString();
				comboItems[i+1] = new Janus.Windows.EditControls.UIComboBoxItem(txt,val);
			}
			
			// ÄÞº¸¿¡ ¼ÂÆ®
			this.cbAgency.Items.AddRange(comboItems);
			this.cbAgency.SelectedIndex = 0;

			Application.DoEvents();
		}

		private void InitButton()
		{
			if(canRead)
			{
				btnSearch.Enabled = true;
			}
			else
			{
				btnSearch.Enabled = false;
			}
			if(canCreate)
			{
				btnAdd.Enabled    = true;
			}
			else
			{
				btnAdd.Enabled    = false;
			}

			if(ebUserID.Text.Trim().Length > 0) 
			{
				if(canDelete)
				{
					btnDelete.Enabled = true;
				}
				else
				{
					btnDelete.Enabled = false;
				}
				if(canUpdate)
				{
					btnSave.Enabled   = true;
				}
				else
				{
					btnSave.Enabled   = false;
				}

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
            if (!IsSearching) // 2011.11.28 RH.Jung Á¶È¸ÁßÀÌ ¾Æ´Ò°æ¿ì¿¡¸¸ µ¿ÀÛÇÏµµ·Ï º¯°æ
            {
                SetUserDetailText();
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
			SearchUsers();
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
			ResetUserDetailText();

			ebUserID.Focus();
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
			DeleteUser();		
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
				SearchUsers();
			}
		}



		private void cbUseLevel_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			// »ç¿ëµî±Þº° ¸ÅÃ¼, ·¾, ´ëÇà»ç ¼±ÅÃÀÇ ¹üÀ§ ¼³Á¤

			if(cbUserLevel.SelectedValue.Equals("10"))	// 10:½Ã½ºÅÛ·¹º§ - ¼±ÅÃ¾øÀ½.
			{
				//cbMedia.SelectedIndex = 0;
				//cbMedia.ReadOnly=true;
				//cbMedia.BackColor = Color.WhiteSmoke;

				cbRap.SelectedIndex = 0;
				cbRap.ReadOnly=true;
				cbRap.BackColor = Color.WhiteSmoke;

				cbAgency.SelectedIndex = 0;
				cbAgency.ReadOnly=true;
				cbAgency.BackColor = Color.WhiteSmoke;
			}
			else if(cbUserLevel.SelectedValue.Equals("20"))	// 20:¸ÅÃ¼·¹º§ - ¸ÅÃ¼ ¼±ÅÃ°¡´É.
			{
				// »ç¿ëÀÚ±¸ºÐÀÌ ¾îµå¹Î ¶Ç´Â ¼öÆÛÀ¯ÀúÀÎ °æ¿ì¸¸ »ç¿ë·¹º§À» ¼öÁ¤ÇÒ ¼ö ÀÖ´Ù.
				//cbMedia.SelectedIndex = 0;
				if(commonModel.UserClass.Equals("10") || commonModel.UserClass.Equals("20"))
				{
					//cbMedia.ReadOnly=false;
					//cbMedia.BackColor = Color.White;
				}
				cbRap.ReadOnly=true;
				cbRap.BackColor = Color.WhiteSmoke;

				cbAgency.SelectedIndex = 0;
				cbAgency.ReadOnly=true;
				cbAgency.BackColor = Color.WhiteSmoke;
			}
			else if(cbUserLevel.SelectedValue.Equals("30"))	// 30:¹Ìµð¾î·¾ ·¹º§ - ·¾ ¼±ÅÃ°¡´É
			{
				//cbMedia.SelectedIndex = 0;
				//cbMedia.ReadOnly=true;
				//cbMedia.BackColor = Color.WhiteSmoke;

				// »ç¿ëÀÚ±¸ºÐÀÌ ¾îµå¹Î ¶Ç´Â ¼öÆÛÀ¯ÀúÀÎ °æ¿ì¸¸ »ç¿ë·¹º§À» ¼öÁ¤ÇÒ ¼ö ÀÖ´Ù.
				cbRap.SelectedIndex = 0;
				if(commonModel.UserClass.Equals("10") || commonModel.UserClass.Equals("20"))
				{
					cbRap.ReadOnly=false;
					cbRap.BackColor = Color.White;
				}

				cbAgency.SelectedIndex = 0;
				cbAgency.ReadOnly=true;
				cbAgency.BackColor = Color.WhiteSmoke;
			}
			else if(cbUserLevel.SelectedValue.Equals("40"))	// 40:´ëÇà»ç ·¹º§ - ´ëÇà»ç ¼±ÅÃ°¡´É
			{
				//cbMedia.SelectedIndex = 0;
				//cbMedia.ReadOnly=true;
				//cbMedia.BackColor = Color.WhiteSmoke;

				cbRap.SelectedIndex = 0;
				cbRap.ReadOnly=true;
				cbRap.BackColor = Color.WhiteSmoke;

				// »ç¿ëÀÚ±¸ºÐÀÌ ¾îµå¹Î ¶Ç´Â ¼öÆÛÀ¯ÀúÀÎ °æ¿ì¸¸ »ç¿ë·¹º§À» ¼öÁ¤ÇÒ ¼ö ÀÖ´Ù.
				cbAgency.SelectedIndex = 0;
				if(commonModel.UserClass.Equals("10") || commonModel.UserClass.Equals("20"))
				{
					cbAgency.ReadOnly=false;
					cbAgency.BackColor = Color.White;
				}
			}	
		}

		#endregion

		#region Ã³¸®¸Þ¼Òµå

		/// <summary>
		/// »ç¿ëÀÚ¸ñ·Ï Á¶È¸
		/// </summary>
		private void SearchUsers()
		{
            IsSearching = true;
            
            StatusMessage("»ç¿ëÀÚ Á¤º¸¸¦ Á¶È¸ÇÕ´Ï´Ù.");

            try
            {
                usersModel.Init();
                // µ¥ÀÌÅÍ¸ðµ¨¿¡ Àü¼ÛÇÒ ³»¿ëÀ» ¼ÂÆ®ÇÑ´Ù.
                if (IsNewSearchKey)
                {
                    usersModel.SearchKey = "";
                }
                else
                {
                    usersModel.SearchKey = ebSearchKey.Text;
                }

                usersModel.SearchUserLevel = cbSearchUserLevel.SelectedItem.Value.ToString();
                usersModel.SearchUserClass = cbSearchUserClass.SelectedItem.Value.ToString();

                if (uiCheckBox1.Checked)
                {
                    usersModel.SearchchkAdState_10 = "Y";
                }
                else
                {
                    usersModel.SearchchkAdState_10 = "N";
                }

                ResetUserDetailText();

                //				// »ç¿ëÀÚ¸ñ·ÏÁ¶È¸ ¼­ºñ½º¸¦ È£ÃâÇÑ´Ù.
                new UserInfoManager(systemModel, commonModel).GetUserList(usersModel);

                if (usersModel.ResultCD.Equals("0000"))
                {
                    Utility.SetDataTable(UserInfoDs.Users, usersModel.UserDataSet);
                    StatusMessage(usersModel.ResultCnt + "°ÇÀÇ »ç¿ëÀÚ Á¤º¸°¡ Á¶È¸µÇ¾ú½À´Ï´Ù.");
                    if (canUpdate)
                    {
                        AddSchChoice();
                    }
                    SetUserDetailText();
                }
            }
            catch (FrameException fe)
            {
                FrameSystem.showMsgForm("»ç¿ëÀÚÁ¶È¸¿À·ù111", new string[] { fe.ErrCode, fe.ResultMsg });
            }
            catch (Exception ex)
            {
                FrameSystem.showMsgForm("»ç¿ëÀÚÁ¶È¸¿À·ù", new string[] { "", ex.Message });
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
				if ( UserInfoDs.Tables["Users"].Rows.Count < 1 ) return;
              
				foreach (DataRow row in UserInfoDs.Tables["Users"].Rows)
				{					
					if(IsAdding)
					{
						cm.Position = 0;
						userId = null;						
					}
					else
					{						
						if(row["UserID"].ToString().Equals(userId))
						{					
							cm.Position = rowIndex;
							break;								
						}
					}

					rowIndex++;
					grdExUserList.EnsureVisible();
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
		/// »ç¿ëÀÚ»ó¼¼Á¤º¸ ÀúÀå
		/// </summary>
		private void SaveUserDetail()
		{
			StatusMessage("»ç¿ëÀÚ Á¤º¸¸¦ ÀúÀåÇÕ´Ï´Ù.");

			if(ebUserID.Text.Trim().Length == 0) 
			{
				MessageBox.Show("»ç¿ëÀÚID°¡ ÀÔ·ÂµÇÁö ¾Ê¾Ò½À´Ï´Ù.","»ç¿ëÀÚÁ¤º¸ ÀúÀå", 
					MessageBoxButtons.OK, MessageBoxIcon.Information );
					ebUserID.Focus();
				return;						
			}
			if(ebUserName.Text.Trim().Length == 0) 
			{
				MessageBox.Show("»ç¿ëÀÚÀÌ¸§ÀÌ ÀÔ·ÂµÇÁö ¾Ê¾Ò½À´Ï´Ù.","»ç¿ëÀÚÁ¤º¸ ÀúÀå", 
					MessageBoxButtons.OK, MessageBoxIcon.Information );
					ebUserName.Focus();
				return;				
			}

			try
			{

				// µ¥ÀÌÅÍ¸ðµ¨¿¡ Àü¼ÛÇÒ ³»¿ëÀ» ¼ÂÆ®ÇÑ´Ù.
				usersModel.UserID       = ebUserID.Text.Trim();
				usersModel.UserPassword = Security.Encrypt(ebUserPassword.Text);
				usersModel.UserName     = ebUserName.Text;
				usersModel.UserLevel    = cbUserLevel.SelectedValue.ToString(); 
				usersModel.UserClass    = cbUserClass.SelectedValue.ToString(); 				
				//usersModel.MediaCode    = cbMedia.SelectedValue.ToString();
				usersModel.RapCode      = cbRap.SelectedValue.ToString();
				usersModel.AgencyCode   = cbAgency.SelectedValue.ToString();
				usersModel.UserDept     = ebUserDept.Text;
				usersModel.UserTitle    = ebUserTitle.Text;
				usersModel.UserTell     = ebUserTell.Text;
				usersModel.UserMobile   = ebUserMobile.Text;
				usersModel.UserEMail    = ebUserEMail.Text;
				usersModel.UserComment  = ebUserComment.Text;
				//»ç¿ë¿©ºÎ
				if(rbUseYn_Y.Checked)
				{
					usersModel.UseYn       = "Y";
				}
				else
				{
					usersModel.UseYn       = "N";
				}

				// »ç¿ëÀÚ »ó¼¼Á¤º¸ ÀúÀå ¼­ºñ½º¸¦ È£ÃâÇÑ´Ù.
				if (IsAdding)
				{
					new UserInfoManager(systemModel,commonModel).SetUserAdd(usersModel);
					StatusMessage("»ç¿ëÀÚ Á¤º¸°¡ Ãß°¡µÇ¾ú½À´Ï´Ù.");
					IsAdding = false;
					ResetUserDetailText();
				}
				else
				{
					new UserInfoManager(systemModel,commonModel).SetUserUpdate(usersModel);
					StatusMessage("»ç¿ëÀÚ Á¤º¸°¡ ÀúÀåµÇ¾ú½À´Ï´Ù.");
				}
				
				DisableButton();
				SearchUsers();
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

		/// <summary>
		/// »ç¿ëÀÚÁ¤º¸ »èÁ¦
		/// </summary>
		private void DeleteUser()
		{
			StatusMessage("»ç¿ëÀÚ Á¤º¸¸¦ »èÁ¦ÇÕ´Ï´Ù.");

			if(ebUserID.Text.Trim().Length == 0) 
			{
				MessageBox.Show("»èÁ¦ÇÒ »ç¿ëÀÚ Á¤º¸°¡ ¾ø½À´Ï´Ù.","»ç¿ëÀÚ »èÁ¦", 
					MessageBoxButtons.OK, MessageBoxIcon.Information );
				return;
			}

			DialogResult result = MessageBox.Show("ÇØ´ç »ç¿ëÀÚ Á¤º¸¸¦ »èÁ¦ ÇÏ½Ã°Ú½À´Ï±î?","»ç¿ëÀÚ »èÁ¦",
				MessageBoxButtons.YesNo, MessageBoxIcon.Question,
				MessageBoxDefaultButton.Button2);

			if (result == DialogResult.No) return;

			try
			{
				// µ¥ÀÌÅÍ¸ðµ¨¿¡ Àü¼ÛÇÒ ³»¿ëÀ» ¼ÂÆ®ÇÑ´Ù.
				usersModel.UserID       = ebUserID.Text.Trim();

				// »ç¿ëÀÚ »ó¼¼Á¤º¸ ÀúÀå ¼­ºñ½º¸¦ È£ÃâÇÑ´Ù.
				new UserInfoManager(systemModel,commonModel).SetUserDelete(usersModel);
				
				ResetUserDetailText();				
				StatusMessage("»ç¿ëÀÚ Á¤º¸°¡ »èÁ¦µÇ¾ú½À´Ï´Ù.");			
				
				DisableButton();
				SearchUsers();
				InitButton();
			}
			catch(FrameException fe)
			{
				FrameSystem.showMsgForm("»ç¿ëÀÚÁ¤º¸ »èÁ¦¿À·ù", new string[] {fe.ErrCode, fe.ResultMsg});
			}
			catch(Exception ex)
			{
				FrameSystem.showMsgForm("»ç¿ëÀÚÁ¤º¸ »èÁ¦¿À·ù",new string[] {"",ex.Message});
			}			

		}
		
		/// <summary>
		/// »ç¿ëÀÚ »ó¼¼Á¤º¸ÀÇ ¼ÂÆ®
		/// </summary>
		private void SetUserDetailText()
		{
			int curRow = cm.Position;
			if(curRow >= 0)
			{
				ebUserID.Text             = dt.Rows[curRow]["UserID"].ToString();
				userId             = dt.Rows[curRow]["UserID"].ToString();
				ebUserPassword.Text       = Security.Decrypt(dt.Rows[curRow]["UserPassword"].ToString());
				ebUserName.Text           = dt.Rows[curRow]["UserName"].ToString();
				cbUserClass.SelectedValue = dt.Rows[curRow]["UserClass"].ToString();
				if(commonModel.UserClass.Equals("10") && dt.Rows[curRow]["UserClass"].ToString().Equals("10"))
				{
//					cbUserClass.Text = "¾îµå¹Î";
					cbUserClass.ReadOnly      = true;
				}				
				else
				{
					cbUserClass.ReadOnly      = false;
				}
				ebUserDept.Text           = dt.Rows[curRow]["UserDept"].ToString();
				ebUserTitle.Text          = dt.Rows[curRow]["UserTitle"].ToString();
				ebUserTell.Text           = dt.Rows[curRow]["UserTell"].ToString();
				ebUserMobile.Text         = dt.Rows[curRow]["UserMobile"].ToString();
				ebUserEMail.Text          = dt.Rows[curRow]["UserEMail"].ToString();
				ebLastLogin.Text          = dt.Rows[curRow]["LastLogin"].ToString();
				ebRegDt.Text              = dt.Rows[curRow]["RegDt"].ToString();
				ebModDt.Text              = dt.Rows[curRow]["ModDt"].ToString();
				ebUserComment.Text        = dt.Rows[curRow]["UserComment"].ToString();
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

				ebUserID.ReadOnly         = true;
				ebUserID.BackColor        = Color.WhiteSmoke;

				// »ç¿ëÀÚ±¸ºÐÀÌ ¾îµå¹Î ¶Ç´Â ¼öÆÛÀ¯ÀúÀÎ ¾Æ´Ñ°æ¿ì
				// ÀÚ±âÁ¤º¸¸¸À» ¼öÁ¤ÇÒ ¼ö ÀÖµµ·Ï ÇÑ´Ù.
				if(!commonModel.UserClass.Equals("10") && !commonModel.UserClass.Equals("20"))
				{
					// »ç¿ëÀÚID¸¦ °Ë»çÇÏ¿© ·Î±×ÀÎID¿Í µ¿ÀÏÇÏ¸é ¼öÁ¤±ÇÇÑÀ» ÁØ´Ù.
					if(commonModel.UserID.Equals(ebUserID.Text))
					{
						canUpdate   = true;
						ResetTextReadonly();
					}
					else
					{
						canUpdate  = false;
						SetTextReadonly();
					}
				}
				else
				{
					if(menu.CanUpdate(MenuCode))
					{
						canUpdate   = true;
						ResetTextReadonly();
					}
					else
					{
						canUpdate   = false;
						SetTextReadonly();
					}

				}

				// »ç¿ë·¹º§Àº ³ªÁß¿¡ ¼ÂÆ®ÇØÁØ´Ù... 
				// ¿Ö? ·¹º§º° ¸ÅÃ¼,·¾,´ëÇà»ç ¼ÂÆ®¸¦ ÇØÁÖ¾î¾ßÇÏ¹Ç·Î..
				cbUserLevel.SelectedValue = dt.Rows[curRow]["UserLevel"].ToString();
				//cbMedia.SelectedValue     = dt.Rows[curRow]["MediaCode"].ToString();
				cbRap.SelectedValue       = dt.Rows[curRow]["RapCode"].ToString();
				cbAgency.SelectedValue    = dt.Rows[curRow]["AgencyCode"].ToString();


				IsAdding = false;
			}

			StatusMessage("ÁØºñ");
		}

		private void ResetUserDetailText()
		{
			ebUserID.Text             = "";
			ebUserPassword.Text       = "";
			ebUserName.Text           = "";
			//cbUserLevel.SelectedIndex =  0;
			//cbUserClass.SelectedIndex =  0;
			//cbMedia.SelectedIndex     =  0;
			//cbRap.SelectedIndex       =  0;
			//cbAgency.SelectedIndex    =  0;
			ebUserDept.Text           = "";
			ebUserTitle.Text          = "";
			ebUserTell.Text           = "";
			ebUserMobile.Text         = "";
			ebUserEMail.Text          = "";
			ebLastLogin.Text          = "";
			ebRegDt.Text              = "";
			ebModDt.Text              = "";
			ebUserComment.Text        = "";
			rbUseYn_Y.Checked         = true;
			rbUseYn_N.Checked         = false;

			if(!IsAdding)
			{
				ebUserID.ReadOnly         = true;
				ebUserID.BackColor        = Color.WhiteSmoke;
			}
		}
		
		/// <summary>
		/// »ó¼¼Á¤º¸ ReadOnly
		/// </summary>
		private void SetTextReadonly()
		{
			ebUserID.ReadOnly         = true;
			ebUserPassword.ReadOnly   = true;
			ebUserName.ReadOnly       = true;
			cbUserLevel.ReadOnly      = true;
			cbUserClass.ReadOnly      = true;
			ebUserDept.ReadOnly       = true;
			ebUserTitle.ReadOnly      = true;
			ebUserTell.ReadOnly       = true;
			ebUserMobile.ReadOnly     = true;
			ebUserEMail.ReadOnly      = true;
			ebUserComment.ReadOnly    = true;
			rbUseYn_Y.Enabled         = false;
			rbUseYn_N.Enabled         = false;

			ebUserID.BackColor        = Color.WhiteSmoke;
			ebUserPassword.BackColor  = Color.WhiteSmoke;
			ebUserName.BackColor      = Color.WhiteSmoke;
			cbUserLevel.BackColor     = Color.WhiteSmoke;
			cbUserClass.BackColor     = Color.WhiteSmoke;
			ebUserDept.BackColor      = Color.WhiteSmoke;
			ebUserTitle.BackColor     = Color.WhiteSmoke;
			ebUserTell.BackColor      = Color.WhiteSmoke;
			ebUserMobile.BackColor    = Color.WhiteSmoke;
			ebUserEMail.BackColor     = Color.WhiteSmoke;
			ebUserComment.BackColor   = Color.WhiteSmoke;

		}

		/// <summary>
		/// »ó¼¼Á¤º¸ ¼öÁ¤°¡´ÉÄÉ
		/// </summary>
		private void ResetTextReadonly()
		{
			ebUserPassword.ReadOnly   = false;
			ebUserName.ReadOnly       = false;
			ebUserDept.ReadOnly       = false;
			ebUserTitle.ReadOnly      = false;
			ebUserTell.ReadOnly       = false;
			ebUserMobile.ReadOnly     = false;
			ebUserEMail.ReadOnly      = false;
			ebUserComment.ReadOnly    = false;

			ebUserPassword.BackColor  = Color.White;
			ebUserName.BackColor      = Color.White;
			ebUserDept.BackColor      = Color.White;
			ebUserTitle.BackColor     = Color.White;
			ebUserTell.BackColor      = Color.White;
			ebUserMobile.BackColor    = Color.White;
			ebUserEMail.BackColor     = Color.White;
			ebUserComment.BackColor   = Color.White;
			ebUserEMail.BackColor     = Color.White;
			ebUserComment.BackColor	  = Color.White;

			// »ç¿ëÀÚ±¸ºÐÀÌ ¾îµå¹Î ¶Ç´Â ¼öÆÛÀ¯ÀúÀÎ °æ¿ì¸¸ »ç¿ë·¹º§, »ç¿ëÀÚ±¸ºÐ, »ç¿ë¿©ºÎ¸¦ ¼öÁ¤ÇÒ ¼ö ÀÖ´Ù.
			if(commonModel.UserClass.Equals("10") || commonModel.UserClass.Equals("20"))
			{
				cbUserLevel.ReadOnly      = false;
				//cbUserClass.ReadOnly      = true;
				cbUserLevel.BackColor     = Color.White;
				//cbUserClass.BackColor     = Color.White;

				rbUseYn_Y.Enabled         = true;
				rbUseYn_N.Enabled         = true;
			}

			// ½Å±ÔÀÛ¼ºÀÌ¸é ¾ÆÀÌµð±îÁö ¾²±â°¡´É
			if (IsAdding)
			{
				ebUserID.ReadOnly         = false;
				ebUserID.BackColor        = Color.White;
			}


		}
		#endregion

        private void lbUserID_Click(object sender, EventArgs e)
        {

        }

        private void lbUserLevel_Click(object sender, EventArgs e)
        {

        }

        private void lbUserClass_Click(object sender, EventArgs e)
        {

        }

        private void lbMedia_Click(object sender, EventArgs e)
        {

        }

        private void lbRap_Click(object sender, EventArgs e)
        {

        }

        private void lbAgnecy_Click(object sender, EventArgs e)
        {

        }

        private void lbLastLogin_Click(object sender, EventArgs e)
        {

        }

        private void lbUserComment_Click(object sender, EventArgs e)
        {

        }

        private void lbUserTitle_Click(object sender, EventArgs e)
        {

        }

        private void lbUserDept_Click(object sender, EventArgs e)
        {

        }

        private void lbMobile_Click(object sender, EventArgs e)
        {

        }

        private void lbUserTell_Click(object sender, EventArgs e)
        {

        }

        private void lbUserName_Click(object sender, EventArgs e)
        {

        }

        private void lbUserPassword_Click(object sender, EventArgs e)
        {

        }

        private void lbUserEmail_Click(object sender, EventArgs e)
        {

        }

        private void lbUseYn_Click(object sender, EventArgs e)
        {

        }

	}
}
