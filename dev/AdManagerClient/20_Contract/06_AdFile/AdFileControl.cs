// ===============================================================================
// AdFileControl for Charites Project
//
// AdFileControl.cs
//
// ±¤°íÆÄÀÏ°ü¸® ÄÁµå·ÑÀ» Á¤ÀÇÇÕ´Ï´Ù. 
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
using System.Runtime.InteropServices;

using WinFramework.Misc;
using WinFramework.Base;
using WinFramework.Data;

using AdManagerModel;

namespace AdManagerClient
{
	public delegate void GetUploadDelegate();
	public delegate	void CheckFileDelegate();

	/// <summary>
	/// ±¤°íÆÄÀÏ°ü¸® ÄÁÆ®·Ñ
	/// </summary>
    public class AdFileControl : System.Windows.Forms.UserControl, IUserControl
	{
		#region ÀÌº¥Æ®ÇÚµé·¯
		public event StatusEventHandler 			StatusEvent;			// »óÅÂÀÌº¥Æ® ÇÚµé·¯
		public event ProgressEventHandler 			ProgressEvent;			// Ã³¸®ÁßÀÌº¥Æ® ÇÚµé·¯

		public event GetUploadDelegate OnGetContent;
		protected virtual void GetContentList()
		{
			OnGetContent();
		}

		public event CheckFileDelegate OnCheckFile;
		protected virtual void CheckFileComplete()
		{
			OnCheckFile();
		}

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
		AdFileWideModel adFileWideModel = new AdFileWideModel();	// ±¤°íÆÄÀÏ¹èÆ÷¸ðµ¨
		AdFileModel		adFileModel		= new AdFileModel();		// ±¤°íÆÄÀÏ¸ðµ¨

		// È­¸éÃ³¸®¿ë º¯¼ö
		bool IsNewSearchKey		  = true;					// °Ë»ö¾îÀÔ·Â ¿©ºÎ
		CurrencyManager cm        = null;					// µ¥ÀÌÅÍ ±×¸®µåÀÇ º¯°æ¿¡ µû¸¥ µ¥ÀÌÅÍ¼Â °ü¸®¸¦ À§ÇÏ¿©			
		DataTable       dt        = null;

		CurrencyManager cmRep     = null;					// µ¥ÀÌÅÍ ±×¸®µåÀÇ º¯°æ¿¡ µû¸¥ µ¥ÀÌÅÍ¼Â °ü¸®¸¦ À§ÇÏ¿©			
		DataTable       dtRep     = null;

        bool IsSearching = false; // Á¶È¸Áß »ó¼¼È­¸éÀÌ ¾÷µ¥ÀÌÆ® µÇ´Â °ÍÀ» ¹æÁö ÇÏ±âÀ§ÇÔ 2011.11.29 JH.Park
		bool canRead			  = false;
		bool canUpdate			  = false;

		// FTP¾÷·ÎµåÁ¤º¸
		string FtpUploadHost;
		string FtpUploadPort;
		string FtpUploadID;
		string FtpUploadPW;

		// Key µ¥ÀÌÅÍ
		string keyMediaCode	   = "1";
		string keyItemNo       = "";
		string keyAdType       = "";
		string keyAdState      = "";
		string keyFileState    = "";
		string strDefaultPath  = "";
		string newItemNo  = "";				//µî·ÏÆÄÀÏ°Ë»ö¿¡¼­ ÆÄÀÏÀ» µî·ÏÈÄ ¾÷µ¥ÀÌÆ®½Ã ÀÌ¿ëµÉ ÆË¾÷¿¡¼­ ³Ñ¾î¿Â Key		
		string Flag  = "";					//Y:±âµî·ÏµÈ ÆÄÀÏÀ» »ç¿ëÇÒ °æ¿ì(ÆË¾÷À©µµ¿ì¿¡¼­ ¼±ÅÃ½Ã ¼³Á¤µÊ), ÀÏ¹Ý¾÷·Îµå

		// ÆÄÀÏ¾÷·Îµå¿ë
		private FtpManager	ftm;
		private	string		fileMax		= string.Empty;
		private bool		firstDraw	= true;
		private DateTime	start;		
		private TimeSpan	timeSpan;
		private int tm, m, s = 0;
		string fileState    = "";
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
		private System.Windows.Forms.Panel pnlUserDetail;
		private Janus.Windows.UI.Dock.UIPanelGroup uiPanelAdFile;
		private Janus.Windows.UI.Dock.UIPanel uiPanelSearch;
		private Janus.Windows.UI.Dock.UIPanelInnerContainer uiPanelSearchContainer;
		private Janus.Windows.UI.Dock.UIPanel uiPanelList;
		private Janus.Windows.UI.Dock.UIPanelInnerContainer uiPanelListContainer;
		private Janus.Windows.UI.Dock.UIPanel uiPanelDetail;
        private Janus.Windows.UI.Dock.UIPanelInnerContainer uiPanelDetailContainer;
		private System.Data.DataView dvAdFile;
		private Janus.Windows.EditControls.UIComboBox cbSearchFileType;
		private Janus.Windows.GridEX.EditControls.EditBox ebSearchKey;
		private Janus.Windows.EditControls.UICheckBox chkAdState_10;
		private Janus.Windows.EditControls.UICheckBox chkAdState_20;
		private Janus.Windows.EditControls.UICheckBox chkAdState_30;
		private Janus.Windows.EditControls.UICheckBox chkAdState_40;
		private AdManagerClient.AdFileDs adFileDs;
		private Janus.Windows.EditControls.UICheckBox chkFileState_10;
		private Janus.Windows.EditControls.UICheckBox chkFileState_90;
		private Janus.Windows.EditControls.UICheckBox chkFileState_20;
		private Janus.Windows.EditControls.UICheckBox chkFileState_30;
		private Janus.Windows.EditControls.UIButton btnSearch;
		private Janus.Windows.EditControls.UICheckBox chkFileState_12;
		private Janus.Windows.EditControls.UIComboBox cbSearchAdType;
		private System.Windows.Forms.OpenFileDialog openFileDlg;
		private Janus.Windows.GridEX.GridEX grdExAdFileList;
		private Janus.Windows.EditControls.UIButton btnRegFileSearch;
		private Janus.Windows.EditControls.UICheckBox chkFileUploadUseYN;
		private System.Windows.Forms.Label lbTimeSpan;
		private System.Windows.Forms.Label lbFileFlow;
		private System.Windows.Forms.Label lbNoSave;
		private Janus.Windows.GridEX.EditControls.NumericEditBox ebFileLength;
		private Janus.Windows.EditControls.UIButton btnSave;
		private Janus.Windows.EditControls.UIComboBox cbDownLevel;
		private System.Windows.Forms.Label lbFileLength;
		private Janus.Windows.GridEX.EditControls.EditBox ebRegName;
		private Janus.Windows.EditControls.UIComboBox cbFileType;
		private System.Windows.Forms.Label lbAdFileType;
		private Janus.Windows.GridEX.EditControls.EditBox ebFileName;
		private System.Windows.Forms.Label lbAdFileName;
		private System.Windows.Forms.Label lbRegDt;
		private Janus.Windows.GridEX.EditControls.EditBox ebRegDt;
		private System.Windows.Forms.Label lbDownloadOrderLevel;
		private Janus.Windows.GridEX.EditControls.EditBox ebFilePath;
		private System.Windows.Forms.Label lbFilePath;
		private Janus.Windows.GridEX.EditControls.EditBox ebItemName;
		private System.Windows.Forms.Label lbContentName;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label1;
		private Janus.Windows.GridEX.EditControls.EditBox ebFileState;
		private Janus.Windows.EditControls.UIProgressBar progBar;
		private Janus.Windows.GridEX.EditControls.EditBox ebAdTime;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label4;
		private Janus.Windows.GridEX.EditControls.EditBox ebLocalFile;
		private Janus.Windows.EditControls.UIButton btnAdFileSearch;
		private System.Windows.Forms.Label label6;
		private Janus.Windows.GridEX.EditControls.EditBox ebCDNPubDt;
		private Janus.Windows.GridEX.EditControls.EditBox ebCDNPubName;
		private System.Windows.Forms.Label label7;
		private Janus.Windows.GridEX.EditControls.EditBox ebSTBDelDt;
		private Janus.Windows.GridEX.EditControls.EditBox ebSTBDelName;
		private Janus.Windows.UI.Tab.UITab tabFile;
		private System.Data.DataView dvHistory;
		private System.Windows.Forms.Panel panel1;
		private Janus.Windows.GridEX.GridEX grdExHistory;
		private Janus.Windows.GridEX.EditControls.EditBox ebPreFileName;
		private Janus.Windows.EditControls.UIButton btnView;
		private Janus.Windows.UI.Tab.UITabPage TPDetail;
		private Janus.Windows.EditControls.UIButton btnFileReplace;
		private Janus.Windows.UI.Tab.UITabPage TPReplace;
		private System.Windows.Forms.Panel panel_Replace;
		private Janus.Windows.GridEX.GridEX gridEX_Replace;
		private System.Data.DataView dvFileReplace;
		private Janus.Windows.UI.Tab.UITabPage TPPub;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.Label label9;
		private System.Windows.Forms.Label label11;
		private System.Windows.Forms.Label label12;
		private System.Windows.Forms.Label label10;
		private System.Windows.Forms.Label label13;
		private Janus.Windows.GridEX.EditControls.EditBox p2_FileRegId;
		private Janus.Windows.GridEX.EditControls.EditBox p2_FileRegDt;
		private Janus.Windows.GridEX.EditControls.EditBox p2_CheckDt;
		private Janus.Windows.GridEX.EditControls.EditBox p2_CheckId;
		private Janus.Windows.GridEX.EditControls.EditBox p2_CdnPubDt;
		private Janus.Windows.GridEX.EditControls.EditBox p2_CdnPubId;
		private Janus.Windows.GridEX.EditControls.EditBox p2_StbDelDt;
		private Janus.Windows.GridEX.EditControls.EditBox p2_StbDelId;
		private Janus.Windows.GridEX.EditControls.EditBox p2_PreFileName;
		private Janus.Windows.GridEX.EditControls.EditBox p2_FileLength;
		private System.ComponentModel.IContainer components;

		public AdFileControl()
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
				if (ftm != null)
				{
					ftm.Close();
				}

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
			Janus.Windows.GridEX.GridEXLayout grdExAdFileList_DesignTimeLayout = new Janus.Windows.GridEX.GridEXLayout();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AdFileControl));
			Janus.Windows.EditControls.UIComboBoxItem uiComboBoxItem2 = new Janus.Windows.EditControls.UIComboBoxItem();
			Janus.Windows.EditControls.UIComboBoxItem uiComboBoxItem3 = new Janus.Windows.EditControls.UIComboBoxItem();
			Janus.Windows.EditControls.UIComboBoxItem uiComboBoxItem4 = new Janus.Windows.EditControls.UIComboBoxItem();
			Janus.Windows.EditControls.UIComboBoxItem uiComboBoxItem5 = new Janus.Windows.EditControls.UIComboBoxItem();
			Janus.Windows.EditControls.UIComboBoxItem uiComboBoxItem6 = new Janus.Windows.EditControls.UIComboBoxItem();
			Janus.Windows.EditControls.UIComboBoxItem uiComboBoxItem7 = new Janus.Windows.EditControls.UIComboBoxItem();
			Janus.Windows.EditControls.UIComboBoxItem uiComboBoxItem8 = new Janus.Windows.EditControls.UIComboBoxItem();
			Janus.Windows.EditControls.UIComboBoxItem uiComboBoxItem9 = new Janus.Windows.EditControls.UIComboBoxItem();
			Janus.Windows.EditControls.UIComboBoxItem uiComboBoxItem10 = new Janus.Windows.EditControls.UIComboBoxItem();
			Janus.Windows.GridEX.GridEXLayout gridEX_Replace_DesignTimeLayout = new Janus.Windows.GridEX.GridEXLayout();
			Janus.Windows.GridEX.GridEXLayout grdExHistory_DesignTimeLayout = new Janus.Windows.GridEX.GridEXLayout();
			this.uiPM = new Janus.Windows.UI.Dock.UIPanelManager(this.components);
			this.uiPanelAdFile = new Janus.Windows.UI.Dock.UIPanelGroup();
			this.uiPanelSearch = new Janus.Windows.UI.Dock.UIPanel();
			this.uiPanelSearchContainer = new Janus.Windows.UI.Dock.UIPanelInnerContainer();
			this.pnlSearch = new System.Windows.Forms.Panel();
			this.btnSearch = new Janus.Windows.EditControls.UIButton();
			this.ebSearchKey = new Janus.Windows.GridEX.EditControls.EditBox();
			this.cbSearchAdType = new Janus.Windows.EditControls.UIComboBox();
			this.cbSearchFileType = new Janus.Windows.EditControls.UIComboBox();
			this.chkAdState_30 = new Janus.Windows.EditControls.UICheckBox();
			this.chkAdState_40 = new Janus.Windows.EditControls.UICheckBox();
			this.chkAdState_10 = new Janus.Windows.EditControls.UICheckBox();
			this.chkAdState_20 = new Janus.Windows.EditControls.UICheckBox();
			this.chkFileState_20 = new Janus.Windows.EditControls.UICheckBox();
			this.chkFileState_10 = new Janus.Windows.EditControls.UICheckBox();
			this.chkFileState_90 = new Janus.Windows.EditControls.UICheckBox();
			this.chkFileState_30 = new Janus.Windows.EditControls.UICheckBox();
			this.chkFileState_12 = new Janus.Windows.EditControls.UICheckBox();
			this.uiPanelList = new Janus.Windows.UI.Dock.UIPanel();
			this.uiPanelListContainer = new Janus.Windows.UI.Dock.UIPanelInnerContainer();
			this.grdExAdFileList = new Janus.Windows.GridEX.GridEX();
			this.dvAdFile = new System.Data.DataView();
			this.adFileDs = new AdManagerClient.AdFileDs();
			this.uiPanelDetail = new Janus.Windows.UI.Dock.UIPanel();
			this.uiPanelDetailContainer = new Janus.Windows.UI.Dock.UIPanelInnerContainer();
			this.pnlUserDetail = new System.Windows.Forms.Panel();
			this.tabFile = new Janus.Windows.UI.Tab.UITab();
			this.TPDetail = new Janus.Windows.UI.Tab.UITabPage();
			this.btnFileReplace = new Janus.Windows.EditControls.UIButton();
			this.btnView = new Janus.Windows.EditControls.UIButton();
			this.btnRegFileSearch = new Janus.Windows.EditControls.UIButton();
			this.chkFileUploadUseYN = new Janus.Windows.EditControls.UICheckBox();
			this.lbTimeSpan = new System.Windows.Forms.Label();
			this.lbFileFlow = new System.Windows.Forms.Label();
			this.lbNoSave = new System.Windows.Forms.Label();
			this.ebFileLength = new Janus.Windows.GridEX.EditControls.NumericEditBox();
			this.btnSave = new Janus.Windows.EditControls.UIButton();
			this.cbDownLevel = new Janus.Windows.EditControls.UIComboBox();
			this.lbFileLength = new System.Windows.Forms.Label();
			this.ebRegName = new Janus.Windows.GridEX.EditControls.EditBox();
			this.cbFileType = new Janus.Windows.EditControls.UIComboBox();
			this.lbAdFileType = new System.Windows.Forms.Label();
			this.ebFileName = new Janus.Windows.GridEX.EditControls.EditBox();
			this.lbAdFileName = new System.Windows.Forms.Label();
			this.lbRegDt = new System.Windows.Forms.Label();
			this.ebRegDt = new Janus.Windows.GridEX.EditControls.EditBox();
			this.lbDownloadOrderLevel = new System.Windows.Forms.Label();
			this.ebFilePath = new Janus.Windows.GridEX.EditControls.EditBox();
			this.lbFilePath = new System.Windows.Forms.Label();
			this.ebItemName = new Janus.Windows.GridEX.EditControls.EditBox();
			this.lbContentName = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.ebFileState = new Janus.Windows.GridEX.EditControls.EditBox();
			this.progBar = new Janus.Windows.EditControls.UIProgressBar();
			this.ebAdTime = new Janus.Windows.GridEX.EditControls.EditBox();
			this.label2 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.ebLocalFile = new Janus.Windows.GridEX.EditControls.EditBox();
			this.btnAdFileSearch = new Janus.Windows.EditControls.UIButton();
			this.label6 = new System.Windows.Forms.Label();
			this.ebCDNPubDt = new Janus.Windows.GridEX.EditControls.EditBox();
			this.ebCDNPubName = new Janus.Windows.GridEX.EditControls.EditBox();
			this.label7 = new System.Windows.Forms.Label();
			this.ebSTBDelDt = new Janus.Windows.GridEX.EditControls.EditBox();
			this.ebSTBDelName = new Janus.Windows.GridEX.EditControls.EditBox();
			this.ebPreFileName = new Janus.Windows.GridEX.EditControls.EditBox();
			this.TPReplace = new Janus.Windows.UI.Tab.UITabPage();
			this.panel_Replace = new System.Windows.Forms.Panel();
			this.p2_FileLength = new Janus.Windows.GridEX.EditControls.EditBox();
			this.label13 = new System.Windows.Forms.Label();
			this.p2_PreFileName = new Janus.Windows.GridEX.EditControls.EditBox();
			this.label10 = new System.Windows.Forms.Label();
			this.p2_FileRegId = new Janus.Windows.GridEX.EditControls.EditBox();
			this.label8 = new System.Windows.Forms.Label();
			this.label9 = new System.Windows.Forms.Label();
			this.p2_FileRegDt = new Janus.Windows.GridEX.EditControls.EditBox();
			this.p2_CheckDt = new Janus.Windows.GridEX.EditControls.EditBox();
			this.p2_CheckId = new Janus.Windows.GridEX.EditControls.EditBox();
			this.label11 = new System.Windows.Forms.Label();
			this.p2_CdnPubDt = new Janus.Windows.GridEX.EditControls.EditBox();
			this.p2_CdnPubId = new Janus.Windows.GridEX.EditControls.EditBox();
			this.label12 = new System.Windows.Forms.Label();
			this.p2_StbDelDt = new Janus.Windows.GridEX.EditControls.EditBox();
			this.p2_StbDelId = new Janus.Windows.GridEX.EditControls.EditBox();
			this.gridEX_Replace = new Janus.Windows.GridEX.GridEX();
			this.dvFileReplace = new System.Data.DataView();
			this.TPPub = new Janus.Windows.UI.Tab.UITabPage();
			this.panel1 = new System.Windows.Forms.Panel();
			this.grdExHistory = new Janus.Windows.GridEX.GridEX();
			this.dvHistory = new System.Data.DataView();
			this.openFileDlg = new System.Windows.Forms.OpenFileDialog();
			((System.ComponentModel.ISupportInitialize)(this.uiPM)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.uiPanelAdFile)).BeginInit();
			this.uiPanelAdFile.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.uiPanelSearch)).BeginInit();
			this.uiPanelSearch.SuspendLayout();
			this.uiPanelSearchContainer.SuspendLayout();
			this.pnlSearch.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.uiPanelList)).BeginInit();
			this.uiPanelList.SuspendLayout();
			this.uiPanelListContainer.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.grdExAdFileList)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.dvAdFile)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.adFileDs)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.uiPanelDetail)).BeginInit();
			this.uiPanelDetail.SuspendLayout();
			this.uiPanelDetailContainer.SuspendLayout();
			this.pnlUserDetail.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.tabFile)).BeginInit();
			this.tabFile.SuspendLayout();
			this.TPDetail.SuspendLayout();
			this.TPReplace.SuspendLayout();
			this.panel_Replace.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.gridEX_Replace)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.dvFileReplace)).BeginInit();
			this.TPPub.SuspendLayout();
			this.panel1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.grdExHistory)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.dvHistory)).BeginInit();
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
			this.uiPanelAdFile.Id = new System.Guid("b162c53a-7940-47cd-a0ed-9762c59c50d8");
			this.uiPanelAdFile.StaticGroup = true;
			this.uiPanelSearch.Id = new System.Guid("afb3b7d0-aa79-443c-a1e8-3baba38f7a4d");
			this.uiPanelAdFile.Panels.Add(this.uiPanelSearch);
			this.uiPanelList.Id = new System.Guid("8e3140b9-bfcb-43af-b84e-2ee6cddf51dc");
			this.uiPanelAdFile.Panels.Add(this.uiPanelList);
			this.uiPanelDetail.Id = new System.Guid("b2973a57-d3e1-4d80-9387-7a6dc3332542");
			this.uiPanelAdFile.Panels.Add(this.uiPanelDetail);
			this.uiPM.Panels.Add(this.uiPanelAdFile);
			// 
			// Design Time Panel Info:
			// 
			this.uiPM.BeginPanelInfo();
			this.uiPM.AddDockPanelInfo(new System.Guid("b162c53a-7940-47cd-a0ed-9762c59c50d8"), Janus.Windows.UI.Dock.PanelGroupStyle.HorizontalTiles, Janus.Windows.UI.Dock.PanelDockStyle.Fill, true, new System.Drawing.Size(1010, 677), true);
			this.uiPM.AddDockPanelInfo(new System.Guid("afb3b7d0-aa79-443c-a1e8-3baba38f7a4d"), new System.Guid("b162c53a-7940-47cd-a0ed-9762c59c50d8"), 70, true);
			this.uiPM.AddDockPanelInfo(new System.Guid("8e3140b9-bfcb-43af-b84e-2ee6cddf51dc"), new System.Guid("b162c53a-7940-47cd-a0ed-9762c59c50d8"), 330, true);
			this.uiPM.AddDockPanelInfo(new System.Guid("b2973a57-d3e1-4d80-9387-7a6dc3332542"), new System.Guid("b162c53a-7940-47cd-a0ed-9762c59c50d8"), 247, true);
			this.uiPM.AddFloatingPanelInfo(new System.Guid("b162c53a-7940-47cd-a0ed-9762c59c50d8"), Janus.Windows.UI.Dock.PanelGroupStyle.HorizontalTiles, true, new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
			this.uiPM.AddFloatingPanelInfo(new System.Guid("8e3140b9-bfcb-43af-b84e-2ee6cddf51dc"), new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
			this.uiPM.AddFloatingPanelInfo(new System.Guid("b2973a57-d3e1-4d80-9387-7a6dc3332542"), new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
			this.uiPM.AddFloatingPanelInfo(new System.Guid("afb3b7d0-aa79-443c-a1e8-3baba38f7a4d"), new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
			this.uiPM.EndPanelInfo();
			// 
			// uiPanelAdFile
			// 
			this.uiPanelAdFile.ActiveCaptionMode = Janus.Windows.UI.Dock.ActiveCaptionMode.Never;
			this.uiPanelAdFile.AllowResize = Janus.Windows.UI.InheritableBoolean.False;
			this.uiPanelAdFile.BackColor = System.Drawing.SystemColors.Window;
			this.uiPanelAdFile.CaptionFormatStyle.Font = new System.Drawing.Font("¸¼Àº °íµñ", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
			this.uiPanelAdFile.CaptionVisible = Janus.Windows.UI.InheritableBoolean.True;
			this.uiPanelAdFile.CloseButtonVisible = Janus.Windows.UI.InheritableBoolean.False;
			this.uiPanelAdFile.Location = new System.Drawing.Point(0, 0);
			this.uiPanelAdFile.Name = "uiPanelAdFile";
			this.uiPanelAdFile.Size = new System.Drawing.Size(1010, 677);
			this.uiPanelAdFile.TabIndex = 1;
			this.uiPanelAdFile.Text = "±¤°íÆÄÀÏ°ü¸®";
			// 
			// uiPanelSearch
			// 
			this.uiPanelSearch.AllowResize = Janus.Windows.UI.InheritableBoolean.False;
			this.uiPanelSearch.CaptionVisible = Janus.Windows.UI.InheritableBoolean.False;
			this.uiPanelSearch.CloseButtonVisible = Janus.Windows.UI.InheritableBoolean.False;
			this.uiPanelSearch.InnerContainer = this.uiPanelSearchContainer;
			this.uiPanelSearch.Location = new System.Drawing.Point(0, 22);
			this.uiPanelSearch.Name = "uiPanelSearch";
			this.uiPanelSearch.Size = new System.Drawing.Size(1010, 70);
			this.uiPanelSearch.TabIndex = 4;
			this.uiPanelSearch.Text = "°Ë»ö";
			// 
			// uiPanelSearchContainer
			// 
			this.uiPanelSearchContainer.Controls.Add(this.pnlSearch);
			this.uiPanelSearchContainer.Location = new System.Drawing.Point(1, 1);
			this.uiPanelSearchContainer.Name = "uiPanelSearchContainer";
			this.uiPanelSearchContainer.Size = new System.Drawing.Size(1008, 68);
			this.uiPanelSearchContainer.TabIndex = 0;
			// 
			// pnlSearch
			// 
			this.pnlSearch.BackColor = System.Drawing.SystemColors.Window;
			this.pnlSearch.Controls.Add(this.btnSearch);
			this.pnlSearch.Controls.Add(this.ebSearchKey);
			this.pnlSearch.Controls.Add(this.cbSearchAdType);
			this.pnlSearch.Controls.Add(this.cbSearchFileType);
			this.pnlSearch.Controls.Add(this.chkAdState_30);
			this.pnlSearch.Controls.Add(this.chkAdState_40);
			this.pnlSearch.Controls.Add(this.chkAdState_10);
			this.pnlSearch.Controls.Add(this.chkAdState_20);
			this.pnlSearch.Controls.Add(this.chkFileState_20);
			this.pnlSearch.Controls.Add(this.chkFileState_10);
			this.pnlSearch.Controls.Add(this.chkFileState_90);
			this.pnlSearch.Controls.Add(this.chkFileState_30);
			this.pnlSearch.Controls.Add(this.chkFileState_12);
			this.pnlSearch.Dock = System.Windows.Forms.DockStyle.Fill;
			this.pnlSearch.Location = new System.Drawing.Point(0, 0);
			this.pnlSearch.Name = "pnlSearch";
			this.pnlSearch.Size = new System.Drawing.Size(1008, 68);
			this.pnlSearch.TabIndex = 1;
			// 
			// btnSearch
			// 
			this.btnSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnSearch.Enabled = false;
			this.btnSearch.Location = new System.Drawing.Point(891, 13);
			this.btnSearch.Name = "btnSearch";
			this.btnSearch.Size = new System.Drawing.Size(104, 46);
			this.btnSearch.TabIndex = 18;
			this.btnSearch.Text = "Á¶ È¸";
			this.btnSearch.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
			this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
			// 
			// ebSearchKey
			// 
			this.ebSearchKey.Location = new System.Drawing.Point(270, 40);
			this.ebSearchKey.Name = "ebSearchKey";
			this.ebSearchKey.Size = new System.Drawing.Size(230, 21);
			this.ebSearchKey.TabIndex = 5;
			this.ebSearchKey.Text = "°Ë»ö¾î¸¦ ÀÔ·ÂÇÏ¼¼¿ä";
			this.ebSearchKey.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
			this.ebSearchKey.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
			this.ebSearchKey.TextChanged += new System.EventHandler(this.ebSearchKey_TextChanged);
			this.ebSearchKey.Click += new System.EventHandler(this.ebSearchKey_Click);
			this.ebSearchKey.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ebSearchKey_KeyDown);
			// 
			// cbSearchAdType
			// 
			this.cbSearchAdType.BackColor = System.Drawing.Color.White;
			this.cbSearchAdType.ComboStyle = Janus.Windows.EditControls.ComboStyle.DropDownList;
			this.cbSearchAdType.Location = new System.Drawing.Point(16, 40);
			this.cbSearchAdType.Name = "cbSearchAdType";
			this.cbSearchAdType.Size = new System.Drawing.Size(120, 21);
			this.cbSearchAdType.TabIndex = 6;
			this.cbSearchAdType.Text = "±¤°íÁ¾·ù";
			this.cbSearchAdType.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
			// 
			// cbSearchFileType
			// 
			this.cbSearchFileType.ComboStyle = Janus.Windows.EditControls.ComboStyle.DropDownList;
			uiComboBoxItem1.FormatStyle.Alpha = 0;
			uiComboBoxItem1.IsSeparator = false;
			uiComboBoxItem1.Text = "±¤°íÆÄÀÏ±¸ºÐ ¼±ÅÃ";
			uiComboBoxItem1.Value = "00";
			this.cbSearchFileType.Items.AddRange(new Janus.Windows.EditControls.UIComboBoxItem[] {
            uiComboBoxItem1});
			this.cbSearchFileType.Location = new System.Drawing.Point(142, 40);
			this.cbSearchFileType.Name = "cbSearchFileType";
			this.cbSearchFileType.SelectedIndex = 0;
			this.cbSearchFileType.Size = new System.Drawing.Size(120, 21);
			this.cbSearchFileType.TabIndex = 7;
			this.cbSearchFileType.Text = "±¤°íÆÄÀÏ±¸ºÐ ¼±ÅÃ";
			this.cbSearchFileType.Visible = false;
			this.cbSearchFileType.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
			// 
			// chkAdState_30
			// 
			this.chkAdState_30.BackColor = System.Drawing.SystemColors.Window;
			this.chkAdState_30.Location = new System.Drawing.Point(142, 11);
			this.chkAdState_30.Name = "chkAdState_30";
			this.chkAdState_30.Size = new System.Drawing.Size(44, 21);
			this.chkAdState_30.TabIndex = 10;
			this.chkAdState_30.Text = "ÁßÁö";
			this.chkAdState_30.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
			// 
			// chkAdState_40
			// 
			this.chkAdState_40.Location = new System.Drawing.Point(205, 11);
			this.chkAdState_40.Name = "chkAdState_40";
			this.chkAdState_40.Size = new System.Drawing.Size(44, 21);
			this.chkAdState_40.TabIndex = 11;
			this.chkAdState_40.Text = "Á¾·á";
			this.chkAdState_40.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
			// 
			// chkAdState_10
			// 
			this.chkAdState_10.Checked = true;
			this.chkAdState_10.CheckedValue = "";
			this.chkAdState_10.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkAdState_10.ForeColor = System.Drawing.SystemColors.ControlText;
			this.chkAdState_10.Location = new System.Drawing.Point(16, 11);
			this.chkAdState_10.Name = "chkAdState_10";
			this.chkAdState_10.Size = new System.Drawing.Size(44, 21);
			this.chkAdState_10.TabIndex = 8;
			this.chkAdState_10.Text = "´ë±â";
			this.chkAdState_10.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
			// 
			// chkAdState_20
			// 
			this.chkAdState_20.Checked = true;
			this.chkAdState_20.CheckedValue = "";
			this.chkAdState_20.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkAdState_20.ForeColor = System.Drawing.SystemColors.ControlText;
			this.chkAdState_20.Location = new System.Drawing.Point(79, 11);
			this.chkAdState_20.Name = "chkAdState_20";
			this.chkAdState_20.Size = new System.Drawing.Size(44, 21);
			this.chkAdState_20.TabIndex = 9;
			this.chkAdState_20.Text = "Æí¼º";
			this.chkAdState_20.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
			// 
			// chkFileState_20
			// 
			this.chkFileState_20.Location = new System.Drawing.Point(554, 12);
			this.chkFileState_20.Name = "chkFileState_20";
			this.chkFileState_20.Size = new System.Drawing.Size(67, 21);
			this.chkFileState_20.TabIndex = 15;
			this.chkFileState_20.Text = "CDNµ¿±â";
			this.chkFileState_20.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
			// 
			// chkFileState_10
			// 
			this.chkFileState_10.Checked = true;
			this.chkFileState_10.CheckedValue = "";
			this.chkFileState_10.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkFileState_10.ForeColor = System.Drawing.SystemColors.ControlText;
			this.chkFileState_10.Location = new System.Drawing.Point(396, 12);
			this.chkFileState_10.Name = "chkFileState_10";
			this.chkFileState_10.Size = new System.Drawing.Size(67, 21);
			this.chkFileState_10.TabIndex = 12;
			this.chkFileState_10.Text = "¹Ìµî·Ï";
			this.chkFileState_10.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
			// 
			// chkFileState_90
			// 
			this.chkFileState_90.Location = new System.Drawing.Point(712, 12);
			this.chkFileState_90.Name = "chkFileState_90";
			this.chkFileState_90.Size = new System.Drawing.Size(67, 21);
			this.chkFileState_90.TabIndex = 17;
			this.chkFileState_90.Text = "¼ÂÅ¾»èÁ¦";
			this.chkFileState_90.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
			// 
			// chkFileState_30
			// 
			this.chkFileState_30.Location = new System.Drawing.Point(633, 12);
			this.chkFileState_30.Name = "chkFileState_30";
			this.chkFileState_30.Size = new System.Drawing.Size(67, 21);
			this.chkFileState_30.TabIndex = 16;
			this.chkFileState_30.Text = "¹èÆ÷¿Ï·á";
			this.chkFileState_30.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
			// 
			// chkFileState_12
			// 
			this.chkFileState_12.Location = new System.Drawing.Point(475, 12);
			this.chkFileState_12.Name = "chkFileState_12";
			this.chkFileState_12.Size = new System.Drawing.Size(67, 21);
			this.chkFileState_12.TabIndex = 13;
			this.chkFileState_12.Text = "°Ë¼ö´ë±â";
			this.chkFileState_12.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
			// 
			// uiPanelList
			// 
			this.uiPanelList.BackColor = System.Drawing.SystemColors.Window;
			this.uiPanelList.CaptionStyle = Janus.Windows.UI.Dock.PanelCaptionStyle.Dark;
			this.uiPanelList.InnerContainer = this.uiPanelListContainer;
			this.uiPanelList.Location = new System.Drawing.Point(0, 96);
			this.uiPanelList.MinimumSize = new System.Drawing.Size(-1, 100);
			this.uiPanelList.Name = "uiPanelList";
			this.uiPanelList.Size = new System.Drawing.Size(1010, 330);
			this.uiPanelList.TabIndex = 19;
			this.uiPanelList.TabStop = false;
			this.uiPanelList.Text = "±¤°íÆÄÀÏ¸ñ·Ï";
			// 
			// uiPanelListContainer
			// 
			this.uiPanelListContainer.BackColor = System.Drawing.SystemColors.Window;
			this.uiPanelListContainer.Controls.Add(this.grdExAdFileList);
			this.uiPanelListContainer.Location = new System.Drawing.Point(1, 23);
			this.uiPanelListContainer.Name = "uiPanelListContainer";
			this.uiPanelListContainer.Size = new System.Drawing.Size(1008, 306);
			this.uiPanelListContainer.TabIndex = 0;
			// 
			// grdExAdFileList
			// 
			this.grdExAdFileList.AllowEdit = Janus.Windows.GridEX.InheritableBoolean.False;
			this.grdExAdFileList.AlternatingColors = true;
			this.grdExAdFileList.BorderStyle = Janus.Windows.GridEX.BorderStyle.None;
			this.grdExAdFileList.DataSource = this.dvAdFile;
			grdExAdFileList_DesignTimeLayout.LayoutString = resources.GetString("grdExAdFileList_DesignTimeLayout.LayoutString");
			this.grdExAdFileList.DesignTimeLayout = grdExAdFileList_DesignTimeLayout;
			this.grdExAdFileList.Dock = System.Windows.Forms.DockStyle.Fill;
			this.grdExAdFileList.EditorsControlStyle.ButtonAppearance = Janus.Windows.GridEX.ButtonAppearance.Regular;
			this.grdExAdFileList.EmptyRows = true;
			this.grdExAdFileList.FocusCellFormatStyle.BackColor = System.Drawing.SystemColors.Highlight;
			this.grdExAdFileList.FocusCellFormatStyle.ForeColor = System.Drawing.Color.White;
			this.grdExAdFileList.FocusStyle = Janus.Windows.GridEX.FocusStyle.Solid;
			this.grdExAdFileList.GridLineColor = System.Drawing.Color.Silver;
			this.grdExAdFileList.GridLines = Janus.Windows.GridEX.GridLines.Vertical;
			this.grdExAdFileList.GridLineStyle = Janus.Windows.GridEX.GridLineStyle.Solid;
			this.grdExAdFileList.GroupByBoxVisible = false;
			this.grdExAdFileList.HeaderFormatStyle.TextAlignment = Janus.Windows.GridEX.TextAlignment.Center;
			this.grdExAdFileList.HideSelection = Janus.Windows.GridEX.HideSelection.Highlight;
			this.grdExAdFileList.Location = new System.Drawing.Point(0, 0);
			this.grdExAdFileList.Name = "grdExAdFileList";
			this.grdExAdFileList.ScrollBars = Janus.Windows.GridEX.ScrollBars.Vertical;
			this.grdExAdFileList.SelectedInactiveFormatStyle.BackColor = System.Drawing.Color.Gold;
			this.grdExAdFileList.Size = new System.Drawing.Size(1008, 306);
			this.grdExAdFileList.TabIndex = 19;
			this.grdExAdFileList.TabKeyBehavior = Janus.Windows.GridEX.TabKeyBehavior.ControlNavigation;
			this.grdExAdFileList.ThemedAreas = ((Janus.Windows.GridEX.ThemedArea)(((((((((Janus.Windows.GridEX.ThemedArea.ScrollBars | Janus.Windows.GridEX.ThemedArea.EditControls) 
            | Janus.Windows.GridEX.ThemedArea.Headers) 
            | Janus.Windows.GridEX.ThemedArea.GroupByBox) 
            | Janus.Windows.GridEX.ThemedArea.GroupRows) 
            | Janus.Windows.GridEX.ThemedArea.ControlBorder) 
            | Janus.Windows.GridEX.ThemedArea.Cards) 
            | Janus.Windows.GridEX.ThemedArea.Gridlines) 
            | Janus.Windows.GridEX.ThemedArea.CheckBoxes)));
			this.grdExAdFileList.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
			this.grdExAdFileList.VisualStyleAreas.HeadersStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
			this.grdExAdFileList.VisualStyleAreas.ScrollBarsStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
			this.grdExAdFileList.Enter += new System.EventHandler(this.OnGrdRowChanged);
			// 
			// dvAdFile
			// 
			this.dvAdFile.Table = this.adFileDs.AdFile;
			// 
			// adFileDs
			// 
			this.adFileDs.DataSetName = "AdFileDs";
			this.adFileDs.Locale = new System.Globalization.CultureInfo("en-US");
			this.adFileDs.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
			// 
			// uiPanelDetail
			// 
			this.uiPanelDetail.BackColor = System.Drawing.SystemColors.Window;
			this.uiPanelDetail.CaptionStyle = Janus.Windows.UI.Dock.PanelCaptionStyle.Dark;
			this.uiPanelDetail.InnerContainer = this.uiPanelDetailContainer;
			this.uiPanelDetail.Location = new System.Drawing.Point(0, 430);
			this.uiPanelDetail.Name = "uiPanelDetail";
			this.uiPanelDetail.Size = new System.Drawing.Size(1010, 247);
			this.uiPanelDetail.TabIndex = 20;
			this.uiPanelDetail.Text = "»ó¼¼Á¤º¸";
			// 
			// uiPanelDetailContainer
			// 
			this.uiPanelDetailContainer.Controls.Add(this.pnlUserDetail);
			this.uiPanelDetailContainer.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.uiPanelDetailContainer.Location = new System.Drawing.Point(1, 23);
			this.uiPanelDetailContainer.Name = "uiPanelDetailContainer";
			this.uiPanelDetailContainer.Size = new System.Drawing.Size(1008, 223);
			this.uiPanelDetailContainer.TabIndex = 0;
			// 
			// pnlUserDetail
			// 
			this.pnlUserDetail.BackColor = System.Drawing.SystemColors.Window;
			this.pnlUserDetail.Controls.Add(this.tabFile);
			this.pnlUserDetail.Dock = System.Windows.Forms.DockStyle.Fill;
			this.pnlUserDetail.Location = new System.Drawing.Point(0, 0);
			this.pnlUserDetail.Name = "pnlUserDetail";
			this.pnlUserDetail.Size = new System.Drawing.Size(1008, 223);
			this.pnlUserDetail.TabIndex = 0;
			// 
			// tabFile
			// 
			this.tabFile.Location = new System.Drawing.Point(8, 8);
			this.tabFile.Name = "tabFile";
			this.tabFile.Size = new System.Drawing.Size(991, 208);
			this.tabFile.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
			this.tabFile.TabIndex = 182;
			this.tabFile.TabPages.AddRange(new Janus.Windows.UI.Tab.UITabPage[] {
            this.TPDetail,
            this.TPReplace,
            this.TPPub});
			this.tabFile.TabStop = false;
			// 
			// TPDetail
			// 
			this.TPDetail.Controls.Add(this.btnFileReplace);
			this.TPDetail.Controls.Add(this.btnView);
			this.TPDetail.Controls.Add(this.btnRegFileSearch);
			this.TPDetail.Controls.Add(this.chkFileUploadUseYN);
			this.TPDetail.Controls.Add(this.lbTimeSpan);
			this.TPDetail.Controls.Add(this.lbFileFlow);
			this.TPDetail.Controls.Add(this.lbNoSave);
			this.TPDetail.Controls.Add(this.ebFileLength);
			this.TPDetail.Controls.Add(this.btnSave);
			this.TPDetail.Controls.Add(this.cbDownLevel);
			this.TPDetail.Controls.Add(this.lbFileLength);
			this.TPDetail.Controls.Add(this.ebRegName);
			this.TPDetail.Controls.Add(this.cbFileType);
			this.TPDetail.Controls.Add(this.lbAdFileType);
			this.TPDetail.Controls.Add(this.ebFileName);
			this.TPDetail.Controls.Add(this.lbAdFileName);
			this.TPDetail.Controls.Add(this.lbRegDt);
			this.TPDetail.Controls.Add(this.ebRegDt);
			this.TPDetail.Controls.Add(this.lbDownloadOrderLevel);
			this.TPDetail.Controls.Add(this.ebFilePath);
			this.TPDetail.Controls.Add(this.lbFilePath);
			this.TPDetail.Controls.Add(this.ebItemName);
			this.TPDetail.Controls.Add(this.lbContentName);
			this.TPDetail.Controls.Add(this.label3);
			this.TPDetail.Controls.Add(this.label1);
			this.TPDetail.Controls.Add(this.ebFileState);
			this.TPDetail.Controls.Add(this.progBar);
			this.TPDetail.Controls.Add(this.ebAdTime);
			this.TPDetail.Controls.Add(this.label2);
			this.TPDetail.Controls.Add(this.label4);
			this.TPDetail.Controls.Add(this.ebLocalFile);
			this.TPDetail.Controls.Add(this.btnAdFileSearch);
			this.TPDetail.Controls.Add(this.label6);
			this.TPDetail.Controls.Add(this.ebCDNPubDt);
			this.TPDetail.Controls.Add(this.ebCDNPubName);
			this.TPDetail.Controls.Add(this.label7);
			this.TPDetail.Controls.Add(this.ebSTBDelDt);
			this.TPDetail.Controls.Add(this.ebSTBDelName);
			this.TPDetail.Controls.Add(this.ebPreFileName);
			this.TPDetail.Location = new System.Drawing.Point(1, 22);
			this.TPDetail.Name = "TPDetail";
			this.TPDetail.Size = new System.Drawing.Size(987, 183);
			this.TPDetail.TabStop = true;
			this.TPDetail.Text = "»ó¼¼Á¤º¸";
			// 
			// btnFileReplace
			// 
			this.btnFileReplace.Cursor = System.Windows.Forms.Cursors.Hand;
			this.btnFileReplace.Location = new System.Drawing.Point(700, 6);
			this.btnFileReplace.Name = "btnFileReplace";
			this.btnFileReplace.Size = new System.Drawing.Size(86, 24);
			this.btnFileReplace.StateStyles.FormatStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(111)))), ((int)(((byte)(169)))));
			this.btnFileReplace.StateStyles.FormatStyle.BackColorAlphaMode = Janus.Windows.UI.AlphaMode.UseAlpha;
			this.btnFileReplace.StateStyles.FormatStyle.BackColorGradient = System.Drawing.Color.FromArgb(((int)(((byte)(205)))), ((int)(((byte)(6)))), ((int)(((byte)(48)))));
			this.btnFileReplace.StateStyles.FormatStyle.BackgroundGradientMode = Janus.Windows.UI.BackgroundGradientMode.Vertical;
			this.btnFileReplace.StateStyles.FormatStyle.Font = new System.Drawing.Font("¸¼Àº °íµñ", 9F, System.Drawing.FontStyle.Bold);
			this.btnFileReplace.StateStyles.FormatStyle.FontBold = Janus.Windows.UI.TriState.True;
			this.btnFileReplace.StateStyles.FormatStyle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
			this.btnFileReplace.TabIndex = 223;
			this.btnFileReplace.Text = "¼ÒÀç±³Ã¼";
			this.btnFileReplace.ToolTipText = "±¤°í¼ÒÀç ±³Ã¼¸¦ À§ÇØ ÆÄÀÏ»óÅÂ¸¦ ¹Ìµî·Ï»óÅÂ·Î º¯°æÇÕ´Ï´Ù.";
			this.btnFileReplace.UseThemes = false;
			this.btnFileReplace.VisualStyle = Janus.Windows.UI.VisualStyle.Office2003;
			this.btnFileReplace.Click += new System.EventHandler(this.btnFileReplace_Click);
			// 
			// btnView
			// 
			this.btnView.Cursor = System.Windows.Forms.Cursors.Hand;
			this.btnView.Location = new System.Drawing.Point(886, 6);
			this.btnView.Name = "btnView";
			this.btnView.Size = new System.Drawing.Size(86, 24);
			this.btnView.TabIndex = 222;
			this.btnView.Text = "¼ÒÀçÈ®ÀÎ";
			this.btnView.ToolTipText = "CDN¿¡¼­ ÆÄÀÏÀ» ´Ù¿î¹Þ¾Æ Àç»ýÇÕ´Ï´Ù";
			this.btnView.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
			this.btnView.Click += new System.EventHandler(this.btnView_Click);
			// 
			// btnRegFileSearch
			// 
			this.btnRegFileSearch.Cursor = System.Windows.Forms.Cursors.Hand;
			this.btnRegFileSearch.Enabled = false;
			this.btnRegFileSearch.Location = new System.Drawing.Point(793, 6);
			this.btnRegFileSearch.Name = "btnRegFileSearch";
			this.btnRegFileSearch.Size = new System.Drawing.Size(86, 24);
			this.btnRegFileSearch.TabIndex = 23;
			this.btnRegFileSearch.Text = "µî·ÏÆÄÀÏÃ£±â";
			this.btnRegFileSearch.ToolTipText = "±âÁ¸¿¡ µî·ÏµÈ ÆÄÀÏÀ» Ã£¾Æ¼­ µî·ÏÇÕ´Ï´Ù";
			this.btnRegFileSearch.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
			this.btnRegFileSearch.Click += new System.EventHandler(this.btnRegFileSearch_Click);
			// 
			// chkFileUploadUseYN
			// 
			this.chkFileUploadUseYN.BackColor = System.Drawing.Color.Transparent;
			this.chkFileUploadUseYN.Checked = true;
			this.chkFileUploadUseYN.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkFileUploadUseYN.Location = new System.Drawing.Point(866, 59);
			this.chkFileUploadUseYN.Name = "chkFileUploadUseYN";
			this.chkFileUploadUseYN.Size = new System.Drawing.Size(112, 23);
			this.chkFileUploadUseYN.TabIndex = 24;
			this.chkFileUploadUseYN.Text = "ÆÄÀÏ¾÷·Îµå»ç¿ë";
			this.chkFileUploadUseYN.Visible = false;
			this.chkFileUploadUseYN.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
			// 
			// lbTimeSpan
			// 
			this.lbTimeSpan.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.lbTimeSpan.BackColor = System.Drawing.Color.Transparent;
			this.lbTimeSpan.Location = new System.Drawing.Point(79, 162);
			this.lbTimeSpan.Name = "lbTimeSpan";
			this.lbTimeSpan.Size = new System.Drawing.Size(80, 16);
			this.lbTimeSpan.TabIndex = 221;
			this.lbTimeSpan.Text = "00:00";
			this.lbTimeSpan.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.lbTimeSpan.Visible = false;
			// 
			// lbFileFlow
			// 
			this.lbFileFlow.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.lbFileFlow.BackColor = System.Drawing.Color.Transparent;
			this.lbFileFlow.Location = new System.Drawing.Point(351, 162);
			this.lbFileFlow.Name = "lbFileFlow";
			this.lbFileFlow.Size = new System.Drawing.Size(136, 16);
			this.lbFileFlow.TabIndex = 220;
			this.lbFileFlow.Text = "0 / 0";
			this.lbFileFlow.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.lbFileFlow.Visible = false;
			// 
			// lbNoSave
			// 
			this.lbNoSave.BackColor = System.Drawing.Color.Transparent;
			this.lbNoSave.Location = new System.Drawing.Point(493, 138);
			this.lbNoSave.Name = "lbNoSave";
			this.lbNoSave.Size = new System.Drawing.Size(352, 21);
			this.lbNoSave.TabIndex = 218;
			this.lbNoSave.Text = "(¹èÆ÷¿Ï·á ¹× ¼ÂÅ¾»èÁ¦ »óÅÂÀÎ ÆÄÀÏÀº ¼öÁ¤ÇÒ ¼ö ¾ø½À´Ï´Ù.)";
			this.lbNoSave.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.lbNoSave.Visible = false;
			// 
			// ebFileLength
			// 
			this.ebFileLength.BackColor = System.Drawing.Color.WhiteSmoke;
			this.ebFileLength.DecimalDigits = 0;
			this.ebFileLength.FormatString = "#,##0";
			this.ebFileLength.Location = new System.Drawing.Point(79, 60);
			this.ebFileLength.Name = "ebFileLength";
			this.ebFileLength.Size = new System.Drawing.Size(88, 21);
			this.ebFileLength.TabIndex = 0;
			this.ebFileLength.TabStop = false;
			this.ebFileLength.Text = "0";
			this.ebFileLength.TextAlignment = Janus.Windows.GridEX.TextAlignment.Far;
			this.ebFileLength.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
			this.ebFileLength.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
			// 
			// btnSave
			// 
			this.btnSave.Cursor = System.Windows.Forms.Cursors.Hand;
			this.btnSave.Enabled = false;
			this.btnSave.Location = new System.Drawing.Point(607, 7);
			this.btnSave.Name = "btnSave";
			this.btnSave.Size = new System.Drawing.Size(86, 22);
			this.btnSave.TabIndex = 188;
			this.btnSave.Text = "Àú Àå";
			this.btnSave.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
			this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
			// 
			// cbDownLevel
			// 
			this.cbDownLevel.ComboStyle = Janus.Windows.EditControls.ComboStyle.DropDownList;
			uiComboBoxItem2.FormatStyle.Alpha = 0;
			uiComboBoxItem2.IsSeparator = false;
			uiComboBoxItem2.Text = "1 ¼øÀ§";
			uiComboBoxItem2.Value = "1";
			uiComboBoxItem3.FormatStyle.Alpha = 0;
			uiComboBoxItem3.IsSeparator = false;
			uiComboBoxItem3.Text = "2 ¼øÀ§";
			uiComboBoxItem3.Value = "2";
			uiComboBoxItem4.FormatStyle.Alpha = 0;
			uiComboBoxItem4.IsSeparator = false;
			uiComboBoxItem4.Text = "3 ¼øÀ§";
			uiComboBoxItem4.Value = "3";
			uiComboBoxItem5.FormatStyle.Alpha = 0;
			uiComboBoxItem5.IsSeparator = false;
			uiComboBoxItem5.Text = "4 ¼øÀ§";
			uiComboBoxItem5.Value = "4";
			uiComboBoxItem6.FormatStyle.Alpha = 0;
			uiComboBoxItem6.IsSeparator = false;
			uiComboBoxItem6.Text = "5 ¼øÀ§";
			uiComboBoxItem6.Value = "5";
			uiComboBoxItem7.FormatStyle.Alpha = 0;
			uiComboBoxItem7.IsSeparator = false;
			uiComboBoxItem7.Text = "6 ¼øÀ§";
			uiComboBoxItem7.Value = "6";
			uiComboBoxItem8.FormatStyle.Alpha = 0;
			uiComboBoxItem8.IsSeparator = false;
			uiComboBoxItem8.Text = "7 ¼øÀ§";
			uiComboBoxItem8.Value = "7";
			uiComboBoxItem9.FormatStyle.Alpha = 0;
			uiComboBoxItem9.IsSeparator = false;
			uiComboBoxItem9.Text = "8 ¼øÀ§";
			uiComboBoxItem9.Value = "8";
			uiComboBoxItem10.FormatStyle.Alpha = 0;
			uiComboBoxItem10.IsSeparator = false;
			uiComboBoxItem10.Text = "9 ¼øÀ§";
			uiComboBoxItem10.Value = "9";
			this.cbDownLevel.Items.AddRange(new Janus.Windows.EditControls.UIComboBoxItem[] {
            uiComboBoxItem2,
            uiComboBoxItem3,
            uiComboBoxItem4,
            uiComboBoxItem5,
            uiComboBoxItem6,
            uiComboBoxItem7,
            uiComboBoxItem8,
            uiComboBoxItem9,
            uiComboBoxItem10});
			this.cbDownLevel.Location = new System.Drawing.Point(407, 85);
			this.cbDownLevel.Name = "cbDownLevel";
			this.cbDownLevel.Size = new System.Drawing.Size(88, 21);
			this.cbDownLevel.TabIndex = 21;
			this.cbDownLevel.Text = "´Ù¿î¼øÀ§";
			this.cbDownLevel.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
			// 
			// lbFileLength
			// 
			this.lbFileLength.BackColor = System.Drawing.Color.Transparent;
			this.lbFileLength.Location = new System.Drawing.Point(15, 60);
			this.lbFileLength.Name = "lbFileLength";
			this.lbFileLength.Size = new System.Drawing.Size(56, 21);
			this.lbFileLength.TabIndex = 214;
			this.lbFileLength.Text = "ÆÄÀÏÅ©±â";
			this.lbFileLength.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// ebRegName
			// 
			this.ebRegName.BackColor = System.Drawing.Color.WhiteSmoke;
			this.ebRegName.Location = new System.Drawing.Point(238, 110);
			this.ebRegName.MaxLength = 15;
			this.ebRegName.Name = "ebRegName";
			this.ebRegName.ReadOnly = true;
			this.ebRegName.Size = new System.Drawing.Size(73, 21);
			this.ebRegName.TabIndex = 0;
			this.ebRegName.TabStop = false;
			this.ebRegName.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
			this.ebRegName.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
			// 
			// cbFileType
			// 
			this.cbFileType.ComboStyle = Janus.Windows.EditControls.ComboStyle.DropDownList;
			this.cbFileType.DataSource = this.adFileDs.FileType;
			this.cbFileType.DisplayMember = "CodeName";
			this.cbFileType.Location = new System.Drawing.Point(733, 85);
			this.cbFileType.Name = "cbFileType";
			this.cbFileType.Size = new System.Drawing.Size(114, 21);
			this.cbFileType.TabIndex = 22;
			this.cbFileType.ValueMember = "Code";
			this.cbFileType.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
			// 
			// lbAdFileType
			// 
			this.lbAdFileType.BackColor = System.Drawing.Color.Transparent;
			this.lbAdFileType.Location = new System.Drawing.Point(669, 85);
			this.lbAdFileType.Name = "lbAdFileType";
			this.lbAdFileType.Size = new System.Drawing.Size(56, 21);
			this.lbAdFileType.TabIndex = 194;
			this.lbAdFileType.Text = "ÆÄÀÏ±¸ºÐ";
			this.lbAdFileType.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// ebFileName
			// 
			this.ebFileName.BackColor = System.Drawing.Color.WhiteSmoke;
			this.ebFileName.Location = new System.Drawing.Point(733, 35);
			this.ebFileName.MaxLength = 40;
			this.ebFileName.Name = "ebFileName";
			this.ebFileName.ReadOnly = true;
			this.ebFileName.Size = new System.Drawing.Size(239, 21);
			this.ebFileName.TabIndex = 0;
			this.ebFileName.TabStop = false;
			this.ebFileName.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
			this.ebFileName.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
			// 
			// lbAdFileName
			// 
			this.lbAdFileName.BackColor = System.Drawing.Color.Transparent;
			this.lbAdFileName.Location = new System.Drawing.Point(669, 35);
			this.lbAdFileName.Name = "lbAdFileName";
			this.lbAdFileName.Size = new System.Drawing.Size(56, 21);
			this.lbAdFileName.TabIndex = 189;
			this.lbAdFileName.Text = "ÆÄÀÏ¸í";
			this.lbAdFileName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// lbRegDt
			// 
			this.lbRegDt.BackColor = System.Drawing.Color.Transparent;
			this.lbRegDt.Location = new System.Drawing.Point(15, 110);
			this.lbRegDt.Name = "lbRegDt";
			this.lbRegDt.Size = new System.Drawing.Size(56, 21);
			this.lbRegDt.TabIndex = 212;
			this.lbRegDt.Text = "ÆÄÀÏµî·Ï";
			this.lbRegDt.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// ebRegDt
			// 
			this.ebRegDt.BackColor = System.Drawing.Color.WhiteSmoke;
			this.ebRegDt.Location = new System.Drawing.Point(79, 110);
			this.ebRegDt.Name = "ebRegDt";
			this.ebRegDt.ReadOnly = true;
			this.ebRegDt.Size = new System.Drawing.Size(153, 21);
			this.ebRegDt.TabIndex = 0;
			this.ebRegDt.TabStop = false;
			this.ebRegDt.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
			this.ebRegDt.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
			// 
			// lbDownloadOrderLevel
			// 
			this.lbDownloadOrderLevel.BackColor = System.Drawing.Color.Transparent;
			this.lbDownloadOrderLevel.Location = new System.Drawing.Point(343, 85);
			this.lbDownloadOrderLevel.Name = "lbDownloadOrderLevel";
			this.lbDownloadOrderLevel.Size = new System.Drawing.Size(56, 21);
			this.lbDownloadOrderLevel.TabIndex = 192;
			this.lbDownloadOrderLevel.Text = "´Ù¿î¼øÀ§";
			this.lbDownloadOrderLevel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// ebFilePath
			// 
			this.ebFilePath.BackColor = System.Drawing.Color.WhiteSmoke;
			this.ebFilePath.Location = new System.Drawing.Point(733, 60);
			this.ebFilePath.Name = "ebFilePath";
			this.ebFilePath.ReadOnly = true;
			this.ebFilePath.Size = new System.Drawing.Size(112, 21);
			this.ebFilePath.TabIndex = 20;
			this.ebFilePath.Text = "/adv";
			this.ebFilePath.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
			this.ebFilePath.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
			// 
			// lbFilePath
			// 
			this.lbFilePath.BackColor = System.Drawing.Color.Transparent;
			this.lbFilePath.Location = new System.Drawing.Point(669, 60);
			this.lbFilePath.Name = "lbFilePath";
			this.lbFilePath.Size = new System.Drawing.Size(56, 21);
			this.lbFilePath.TabIndex = 215;
			this.lbFilePath.Text = "ÆÄÀÏÀ§Ä¡";
			this.lbFilePath.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// ebItemName
			// 
			this.ebItemName.BackColor = System.Drawing.Color.WhiteSmoke;
			this.ebItemName.Location = new System.Drawing.Point(79, 10);
			this.ebItemName.MaxLength = 40;
			this.ebItemName.Name = "ebItemName";
			this.ebItemName.ReadOnly = true;
			this.ebItemName.Size = new System.Drawing.Size(416, 21);
			this.ebItemName.TabIndex = 0;
			this.ebItemName.TabStop = false;
			this.ebItemName.Text = "Ä«ÁîÅåÅå_ÀÌº¥Æ®(»ó½Ã)";
			this.ebItemName.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
			this.ebItemName.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
			// 
			// lbContentName
			// 
			this.lbContentName.BackColor = System.Drawing.Color.Transparent;
			this.lbContentName.Location = new System.Drawing.Point(15, 10);
			this.lbContentName.Name = "lbContentName";
			this.lbContentName.Size = new System.Drawing.Size(56, 21);
			this.lbContentName.TabIndex = 190;
			this.lbContentName.Text = "±¤°í¸í";
			this.lbContentName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label3
			// 
			this.label3.BackColor = System.Drawing.Color.Transparent;
			this.label3.Location = new System.Drawing.Point(169, 60);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(40, 21);
			this.label3.TabIndex = 217;
			this.label3.Text = "Bytes";
			this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label1
			// 
			this.label1.BackColor = System.Drawing.Color.Transparent;
			this.label1.Location = new System.Drawing.Point(15, 85);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(56, 21);
			this.label1.TabIndex = 193;
			this.label1.Text = "ÆÄÀÏ»óÅÂ";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// ebFileState
			// 
			this.ebFileState.BackColor = System.Drawing.Color.WhiteSmoke;
			this.ebFileState.Location = new System.Drawing.Point(79, 85);
			this.ebFileState.MaxLength = 15;
			this.ebFileState.Name = "ebFileState";
			this.ebFileState.ReadOnly = true;
			this.ebFileState.Size = new System.Drawing.Size(88, 21);
			this.ebFileState.TabIndex = 0;
			this.ebFileState.TabStop = false;
			this.ebFileState.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
			this.ebFileState.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
			// 
			// progBar
			// 
			this.progBar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.progBar.Location = new System.Drawing.Point(79, 137);
			this.progBar.Maximum = 0;
			this.progBar.Name = "progBar";
			this.progBar.ShowPercentage = true;
			this.progBar.Size = new System.Drawing.Size(408, 22);
			this.progBar.TabIndex = 0;
			this.progBar.TabStop = false;
			this.progBar.Visible = false;
			this.progBar.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
			// 
			// ebAdTime
			// 
			this.ebAdTime.BackColor = System.Drawing.Color.WhiteSmoke;
			this.ebAdTime.Location = new System.Drawing.Point(407, 60);
			this.ebAdTime.MaxLength = 15;
			this.ebAdTime.Name = "ebAdTime";
			this.ebAdTime.ReadOnly = true;
			this.ebAdTime.Size = new System.Drawing.Size(70, 21);
			this.ebAdTime.TabIndex = 0;
			this.ebAdTime.TabStop = false;
			this.ebAdTime.TextAlignment = Janus.Windows.GridEX.TextAlignment.Far;
			this.ebAdTime.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
			// 
			// label2
			// 
			this.label2.BackColor = System.Drawing.Color.Transparent;
			this.label2.Location = new System.Drawing.Point(343, 60);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(56, 21);
			this.label2.TabIndex = 191;
			this.label2.Text = "±¤°í±æÀÌ";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label4
			// 
			this.label4.BackColor = System.Drawing.Color.Transparent;
			this.label4.Location = new System.Drawing.Point(483, 60);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(18, 21);
			this.label4.TabIndex = 216;
			this.label4.Text = "ÃÊ";
			this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// ebLocalFile
			// 
			this.ebLocalFile.BackColor = System.Drawing.Color.WhiteSmoke;
			this.ebLocalFile.Location = new System.Drawing.Point(79, 35);
			this.ebLocalFile.MaxLength = 40;
			this.ebLocalFile.Name = "ebLocalFile";
			this.ebLocalFile.ReadOnly = true;
			this.ebLocalFile.Size = new System.Drawing.Size(416, 21);
			this.ebLocalFile.TabIndex = 0;
			this.ebLocalFile.TabStop = false;
			this.ebLocalFile.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
			this.ebLocalFile.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
			// 
			// btnAdFileSearch
			// 
			this.btnAdFileSearch.Cursor = System.Windows.Forms.Cursors.Hand;
			this.btnAdFileSearch.Enabled = false;
			this.btnAdFileSearch.Location = new System.Drawing.Point(3, 34);
			this.btnAdFileSearch.Name = "btnAdFileSearch";
			this.btnAdFileSearch.Size = new System.Drawing.Size(71, 22);
			this.btnAdFileSearch.TabIndex = 187;
			this.btnAdFileSearch.Text = "ÆÄÀÏÃ£±â";
			this.btnAdFileSearch.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
			this.btnAdFileSearch.Click += new System.EventHandler(this.btnAdFileSearch_Click);
			// 
			// label6
			// 
			this.label6.BackColor = System.Drawing.Color.Transparent;
			this.label6.Location = new System.Drawing.Point(343, 110);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(56, 21);
			this.label6.TabIndex = 210;
			this.label6.Text = "¹èÆ÷¿Ï·á";
			this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// ebCDNPubDt
			// 
			this.ebCDNPubDt.BackColor = System.Drawing.Color.WhiteSmoke;
			this.ebCDNPubDt.Location = new System.Drawing.Point(407, 110);
			this.ebCDNPubDt.Name = "ebCDNPubDt";
			this.ebCDNPubDt.ReadOnly = true;
			this.ebCDNPubDt.Size = new System.Drawing.Size(153, 21);
			this.ebCDNPubDt.TabIndex = 0;
			this.ebCDNPubDt.TabStop = false;
			this.ebCDNPubDt.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
			this.ebCDNPubDt.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
			// 
			// ebCDNPubName
			// 
			this.ebCDNPubName.BackColor = System.Drawing.Color.WhiteSmoke;
			this.ebCDNPubName.Location = new System.Drawing.Point(566, 110);
			this.ebCDNPubName.MaxLength = 15;
			this.ebCDNPubName.Name = "ebCDNPubName";
			this.ebCDNPubName.ReadOnly = true;
			this.ebCDNPubName.Size = new System.Drawing.Size(73, 21);
			this.ebCDNPubName.TabIndex = 0;
			this.ebCDNPubName.TabStop = false;
			this.ebCDNPubName.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
			this.ebCDNPubName.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
			// 
			// label7
			// 
			this.label7.BackColor = System.Drawing.Color.Transparent;
			this.label7.Location = new System.Drawing.Point(669, 110);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(56, 21);
			this.label7.TabIndex = 211;
			this.label7.Text = "¼ÂÅ¾»èÁ¦";
			this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// ebSTBDelDt
			// 
			this.ebSTBDelDt.BackColor = System.Drawing.Color.WhiteSmoke;
			this.ebSTBDelDt.Location = new System.Drawing.Point(733, 110);
			this.ebSTBDelDt.Name = "ebSTBDelDt";
			this.ebSTBDelDt.ReadOnly = true;
			this.ebSTBDelDt.Size = new System.Drawing.Size(153, 21);
			this.ebSTBDelDt.TabIndex = 0;
			this.ebSTBDelDt.TabStop = false;
			this.ebSTBDelDt.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
			this.ebSTBDelDt.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
			// 
			// ebSTBDelName
			// 
			this.ebSTBDelName.BackColor = System.Drawing.Color.WhiteSmoke;
			this.ebSTBDelName.Location = new System.Drawing.Point(892, 110);
			this.ebSTBDelName.MaxLength = 15;
			this.ebSTBDelName.Name = "ebSTBDelName";
			this.ebSTBDelName.ReadOnly = true;
			this.ebSTBDelName.Size = new System.Drawing.Size(80, 21);
			this.ebSTBDelName.TabIndex = 0;
			this.ebSTBDelName.TabStop = false;
			this.ebSTBDelName.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
			this.ebSTBDelName.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
			// 
			// ebPreFileName
			// 
			this.ebPreFileName.BackColor = System.Drawing.Color.WhiteSmoke;
			this.ebPreFileName.Location = new System.Drawing.Point(515, 35);
			this.ebPreFileName.MaxLength = 40;
			this.ebPreFileName.Name = "ebPreFileName";
			this.ebPreFileName.ReadOnly = true;
			this.ebPreFileName.Size = new System.Drawing.Size(143, 21);
			this.ebPreFileName.TabIndex = 0;
			this.ebPreFileName.TabStop = false;
			this.ebPreFileName.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
			this.ebPreFileName.Visible = false;
			this.ebPreFileName.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
			// 
			// TPReplace
			// 
			this.TPReplace.Controls.Add(this.panel_Replace);
			this.TPReplace.Location = new System.Drawing.Point(1, 22);
			this.TPReplace.Name = "TPReplace";
			this.TPReplace.Size = new System.Drawing.Size(987, 183);
			this.TPReplace.TabStop = true;
			this.TPReplace.Text = "ÆÄÀÏ±³Ã¼ÀÌ·Â";
			// 
			// panel_Replace
			// 
			this.panel_Replace.BackColor = System.Drawing.Color.Transparent;
			this.panel_Replace.Controls.Add(this.p2_FileLength);
			this.panel_Replace.Controls.Add(this.label13);
			this.panel_Replace.Controls.Add(this.p2_PreFileName);
			this.panel_Replace.Controls.Add(this.label10);
			this.panel_Replace.Controls.Add(this.p2_FileRegId);
			this.panel_Replace.Controls.Add(this.label8);
			this.panel_Replace.Controls.Add(this.label9);
			this.panel_Replace.Controls.Add(this.p2_FileRegDt);
			this.panel_Replace.Controls.Add(this.p2_CheckDt);
			this.panel_Replace.Controls.Add(this.p2_CheckId);
			this.panel_Replace.Controls.Add(this.label11);
			this.panel_Replace.Controls.Add(this.p2_CdnPubDt);
			this.panel_Replace.Controls.Add(this.p2_CdnPubId);
			this.panel_Replace.Controls.Add(this.label12);
			this.panel_Replace.Controls.Add(this.p2_StbDelDt);
			this.panel_Replace.Controls.Add(this.p2_StbDelId);
			this.panel_Replace.Controls.Add(this.gridEX_Replace);
			this.panel_Replace.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel_Replace.Location = new System.Drawing.Point(0, 0);
			this.panel_Replace.Name = "panel_Replace";
			this.panel_Replace.Padding = new System.Windows.Forms.Padding(2);
			this.panel_Replace.Size = new System.Drawing.Size(987, 183);
			this.panel_Replace.TabIndex = 0;
			// 
			// p2_FileLength
			// 
			this.p2_FileLength.BackColor = System.Drawing.Color.WhiteSmoke;
			this.p2_FileLength.Location = new System.Drawing.Point(790, 9);
			this.p2_FileLength.Name = "p2_FileLength";
			this.p2_FileLength.ReadOnly = true;
			this.p2_FileLength.Size = new System.Drawing.Size(190, 21);
			this.p2_FileLength.TabIndex = 231;
			this.p2_FileLength.TabStop = false;
			this.p2_FileLength.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
			this.p2_FileLength.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
			// 
			// label13
			// 
			this.label13.BackColor = System.Drawing.Color.Transparent;
			this.label13.Location = new System.Drawing.Point(734, 9);
			this.label13.Name = "label13";
			this.label13.Size = new System.Drawing.Size(52, 21);
			this.label13.TabIndex = 230;
			this.label13.Text = "ÆÄÀÏ±æÀÌ";
			this.label13.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// p2_PreFileName
			// 
			this.p2_PreFileName.BackColor = System.Drawing.Color.WhiteSmoke;
			this.p2_PreFileName.Location = new System.Drawing.Point(790, 33);
			this.p2_PreFileName.Name = "p2_PreFileName";
			this.p2_PreFileName.ReadOnly = true;
			this.p2_PreFileName.Size = new System.Drawing.Size(190, 21);
			this.p2_PreFileName.TabIndex = 229;
			this.p2_PreFileName.TabStop = false;
			this.p2_PreFileName.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
			this.p2_PreFileName.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
			// 
			// label10
			// 
			this.label10.BackColor = System.Drawing.Color.Transparent;
			this.label10.Location = new System.Drawing.Point(734, 33);
			this.label10.Name = "label10";
			this.label10.Size = new System.Drawing.Size(52, 21);
			this.label10.TabIndex = 228;
			this.label10.Text = "¿øº»¸í";
			this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// p2_FileRegId
			// 
			this.p2_FileRegId.BackColor = System.Drawing.Color.WhiteSmoke;
			this.p2_FileRegId.Location = new System.Drawing.Point(936, 57);
			this.p2_FileRegId.MaxLength = 15;
			this.p2_FileRegId.Name = "p2_FileRegId";
			this.p2_FileRegId.ReadOnly = true;
			this.p2_FileRegId.Size = new System.Drawing.Size(44, 21);
			this.p2_FileRegId.TabIndex = 221;
			this.p2_FileRegId.TabStop = false;
			this.p2_FileRegId.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
			this.p2_FileRegId.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
			// 
			// label8
			// 
			this.label8.BackColor = System.Drawing.Color.Transparent;
			this.label8.Location = new System.Drawing.Point(734, 81);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(52, 21);
			this.label8.TabIndex = 224;
			this.label8.Text = "°Ë¼ö¿Ï·á";
			this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label9
			// 
			this.label9.BackColor = System.Drawing.Color.Transparent;
			this.label9.Location = new System.Drawing.Point(734, 57);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(52, 21);
			this.label9.TabIndex = 227;
			this.label9.Text = "ÆÄÀÏµî·Ï";
			this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// p2_FileRegDt
			// 
			this.p2_FileRegDt.BackColor = System.Drawing.Color.WhiteSmoke;
			this.p2_FileRegDt.Location = new System.Drawing.Point(790, 57);
			this.p2_FileRegDt.Name = "p2_FileRegDt";
			this.p2_FileRegDt.ReadOnly = true;
			this.p2_FileRegDt.Size = new System.Drawing.Size(140, 21);
			this.p2_FileRegDt.TabIndex = 220;
			this.p2_FileRegDt.TabStop = false;
			this.p2_FileRegDt.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
			this.p2_FileRegDt.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
			// 
			// p2_CheckDt
			// 
			this.p2_CheckDt.BackColor = System.Drawing.Color.WhiteSmoke;
			this.p2_CheckDt.Location = new System.Drawing.Point(790, 81);
			this.p2_CheckDt.Name = "p2_CheckDt";
			this.p2_CheckDt.ReadOnly = true;
			this.p2_CheckDt.Size = new System.Drawing.Size(140, 21);
			this.p2_CheckDt.TabIndex = 222;
			this.p2_CheckDt.TabStop = false;
			this.p2_CheckDt.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
			this.p2_CheckDt.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
			// 
			// p2_CheckId
			// 
			this.p2_CheckId.BackColor = System.Drawing.Color.WhiteSmoke;
			this.p2_CheckId.Location = new System.Drawing.Point(936, 81);
			this.p2_CheckId.MaxLength = 15;
			this.p2_CheckId.Name = "p2_CheckId";
			this.p2_CheckId.ReadOnly = true;
			this.p2_CheckId.Size = new System.Drawing.Size(44, 21);
			this.p2_CheckId.TabIndex = 223;
			this.p2_CheckId.TabStop = false;
			this.p2_CheckId.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
			this.p2_CheckId.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
			// 
			// label11
			// 
			this.label11.BackColor = System.Drawing.Color.Transparent;
			this.label11.Location = new System.Drawing.Point(734, 105);
			this.label11.Name = "label11";
			this.label11.Size = new System.Drawing.Size(52, 21);
			this.label11.TabIndex = 225;
			this.label11.Text = "¹èÆ÷¿Ï·á";
			this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// p2_CdnPubDt
			// 
			this.p2_CdnPubDt.BackColor = System.Drawing.Color.WhiteSmoke;
			this.p2_CdnPubDt.Location = new System.Drawing.Point(790, 105);
			this.p2_CdnPubDt.Name = "p2_CdnPubDt";
			this.p2_CdnPubDt.ReadOnly = true;
			this.p2_CdnPubDt.Size = new System.Drawing.Size(140, 21);
			this.p2_CdnPubDt.TabIndex = 214;
			this.p2_CdnPubDt.TabStop = false;
			this.p2_CdnPubDt.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
			this.p2_CdnPubDt.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
			// 
			// p2_CdnPubId
			// 
			this.p2_CdnPubId.BackColor = System.Drawing.Color.WhiteSmoke;
			this.p2_CdnPubId.Location = new System.Drawing.Point(936, 105);
			this.p2_CdnPubId.MaxLength = 15;
			this.p2_CdnPubId.Name = "p2_CdnPubId";
			this.p2_CdnPubId.ReadOnly = true;
			this.p2_CdnPubId.Size = new System.Drawing.Size(44, 21);
			this.p2_CdnPubId.TabIndex = 219;
			this.p2_CdnPubId.TabStop = false;
			this.p2_CdnPubId.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
			this.p2_CdnPubId.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
			// 
			// label12
			// 
			this.label12.BackColor = System.Drawing.Color.Transparent;
			this.label12.Location = new System.Drawing.Point(734, 129);
			this.label12.Name = "label12";
			this.label12.Size = new System.Drawing.Size(52, 21);
			this.label12.TabIndex = 226;
			this.label12.Text = "¼ÂÅ¾»èÁ¦";
			this.label12.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// p2_StbDelDt
			// 
			this.p2_StbDelDt.BackColor = System.Drawing.Color.WhiteSmoke;
			this.p2_StbDelDt.Location = new System.Drawing.Point(790, 129);
			this.p2_StbDelDt.Name = "p2_StbDelDt";
			this.p2_StbDelDt.ReadOnly = true;
			this.p2_StbDelDt.Size = new System.Drawing.Size(140, 21);
			this.p2_StbDelDt.TabIndex = 218;
			this.p2_StbDelDt.TabStop = false;
			this.p2_StbDelDt.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
			this.p2_StbDelDt.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
			// 
			// p2_StbDelId
			// 
			this.p2_StbDelId.BackColor = System.Drawing.Color.WhiteSmoke;
			this.p2_StbDelId.Location = new System.Drawing.Point(936, 129);
			this.p2_StbDelId.MaxLength = 15;
			this.p2_StbDelId.Name = "p2_StbDelId";
			this.p2_StbDelId.ReadOnly = true;
			this.p2_StbDelId.Size = new System.Drawing.Size(44, 21);
			this.p2_StbDelId.TabIndex = 217;
			this.p2_StbDelId.TabStop = false;
			this.p2_StbDelId.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
			this.p2_StbDelId.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
			// 
			// gridEX_Replace
			// 
			this.gridEX_Replace.AllowEdit = Janus.Windows.GridEX.InheritableBoolean.False;
			this.gridEX_Replace.AlternatingColors = true;
			this.gridEX_Replace.BorderStyle = Janus.Windows.GridEX.BorderStyle.RaisedLight3D;
			this.gridEX_Replace.DataSource = this.dvFileReplace;
			gridEX_Replace_DesignTimeLayout.LayoutString = resources.GetString("gridEX_Replace_DesignTimeLayout.LayoutString");
			this.gridEX_Replace.DesignTimeLayout = gridEX_Replace_DesignTimeLayout;
			this.gridEX_Replace.Dock = System.Windows.Forms.DockStyle.Left;
			this.gridEX_Replace.EditorsControlStyle.ButtonAppearance = Janus.Windows.GridEX.ButtonAppearance.Regular;
			this.gridEX_Replace.EmptyRows = true;
			this.gridEX_Replace.FocusCellFormatStyle.BackColor = System.Drawing.SystemColors.Highlight;
			this.gridEX_Replace.FocusCellFormatStyle.ForeColor = System.Drawing.Color.White;
			this.gridEX_Replace.FocusStyle = Janus.Windows.GridEX.FocusStyle.None;
			this.gridEX_Replace.GridLineColor = System.Drawing.Color.Silver;
			this.gridEX_Replace.GridLines = Janus.Windows.GridEX.GridLines.Vertical;
			this.gridEX_Replace.GridLineStyle = Janus.Windows.GridEX.GridLineStyle.Solid;
			this.gridEX_Replace.GroupByBoxVisible = false;
			this.gridEX_Replace.HeaderFormatStyle.TextAlignment = Janus.Windows.GridEX.TextAlignment.Center;
			this.gridEX_Replace.HideSelection = Janus.Windows.GridEX.HideSelection.HighlightInactive;
			this.gridEX_Replace.Location = new System.Drawing.Point(2, 2);
			this.gridEX_Replace.Name = "gridEX_Replace";
			this.gridEX_Replace.ScrollBars = Janus.Windows.GridEX.ScrollBars.Vertical;
			this.gridEX_Replace.SelectedInactiveFormatStyle.BackColor = System.Drawing.Color.Gold;
			this.gridEX_Replace.Size = new System.Drawing.Size(726, 179);
			this.gridEX_Replace.TabIndex = 14;
			this.gridEX_Replace.TabKeyBehavior = Janus.Windows.GridEX.TabKeyBehavior.ControlNavigation;
			this.gridEX_Replace.TabStop = false;
			this.gridEX_Replace.ThemedAreas = ((Janus.Windows.GridEX.ThemedArea)(((((((((Janus.Windows.GridEX.ThemedArea.ScrollBars | Janus.Windows.GridEX.ThemedArea.EditControls) 
            | Janus.Windows.GridEX.ThemedArea.Headers) 
            | Janus.Windows.GridEX.ThemedArea.GroupByBox) 
            | Janus.Windows.GridEX.ThemedArea.GroupRows) 
            | Janus.Windows.GridEX.ThemedArea.ControlBorder) 
            | Janus.Windows.GridEX.ThemedArea.Cards) 
            | Janus.Windows.GridEX.ThemedArea.Gridlines) 
            | Janus.Windows.GridEX.ThemedArea.CheckBoxes)));
			this.gridEX_Replace.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
			this.gridEX_Replace.Enter += new System.EventHandler(this.OnGrdRowChangedReplace);
			// 
			// dvFileReplace
			// 
			this.dvFileReplace.Table = this.adFileDs.AdFileHistory;
			// 
			// TPPub
			// 
			this.TPPub.Controls.Add(this.panel1);
			this.TPPub.Location = new System.Drawing.Point(1, 22);
			this.TPPub.Name = "TPPub";
			this.TPPub.Size = new System.Drawing.Size(987, 183);
			this.TPPub.TabStop = true;
			this.TPPub.Text = "ÆÄÀÏ¹èÆ÷ÀÌ·Â";
			// 
			// panel1
			// 
			this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.panel1.Controls.Add(this.grdExHistory);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel1.Location = new System.Drawing.Point(0, 0);
			this.panel1.Name = "panel1";
			this.panel1.Padding = new System.Windows.Forms.Padding(2);
			this.panel1.Size = new System.Drawing.Size(987, 183);
			this.panel1.TabIndex = 2;
			// 
			// grdExHistory
			// 
			this.grdExHistory.AllowEdit = Janus.Windows.GridEX.InheritableBoolean.False;
			this.grdExHistory.AlternatingColors = true;
			this.grdExHistory.BorderStyle = Janus.Windows.GridEX.BorderStyle.RaisedLight3D;
			this.grdExHistory.DataSource = this.dvHistory;
			grdExHistory_DesignTimeLayout.LayoutString = resources.GetString("grdExHistory_DesignTimeLayout.LayoutString");
			this.grdExHistory.DesignTimeLayout = grdExHistory_DesignTimeLayout;
			this.grdExHistory.Dock = System.Windows.Forms.DockStyle.Fill;
			this.grdExHistory.EditorsControlStyle.ButtonAppearance = Janus.Windows.GridEX.ButtonAppearance.Regular;
			this.grdExHistory.EmptyRows = true;
			this.grdExHistory.FocusCellFormatStyle.BackColor = System.Drawing.SystemColors.Highlight;
			this.grdExHistory.FocusCellFormatStyle.ForeColor = System.Drawing.Color.White;
			this.grdExHistory.FocusStyle = Janus.Windows.GridEX.FocusStyle.None;
			this.grdExHistory.GridLineColor = System.Drawing.Color.Silver;
			this.grdExHistory.GridLines = Janus.Windows.GridEX.GridLines.Vertical;
			this.grdExHistory.GridLineStyle = Janus.Windows.GridEX.GridLineStyle.Solid;
			this.grdExHistory.GroupByBoxVisible = false;
			this.grdExHistory.HeaderFormatStyle.TextAlignment = Janus.Windows.GridEX.TextAlignment.Center;
			this.grdExHistory.HideSelection = Janus.Windows.GridEX.HideSelection.HighlightInactive;
			this.grdExHistory.Location = new System.Drawing.Point(2, 2);
			this.grdExHistory.Name = "grdExHistory";
			this.grdExHistory.ScrollBars = Janus.Windows.GridEX.ScrollBars.Vertical;
			this.grdExHistory.SelectedInactiveFormatStyle.BackColor = System.Drawing.Color.Gold;
			this.grdExHistory.Size = new System.Drawing.Size(981, 177);
			this.grdExHistory.TabIndex = 13;
			this.grdExHistory.TabKeyBehavior = Janus.Windows.GridEX.TabKeyBehavior.ControlNavigation;
			this.grdExHistory.TabStop = false;
			this.grdExHistory.ThemedAreas = ((Janus.Windows.GridEX.ThemedArea)(((((((((Janus.Windows.GridEX.ThemedArea.ScrollBars | Janus.Windows.GridEX.ThemedArea.EditControls) 
            | Janus.Windows.GridEX.ThemedArea.Headers) 
            | Janus.Windows.GridEX.ThemedArea.GroupByBox) 
            | Janus.Windows.GridEX.ThemedArea.GroupRows) 
            | Janus.Windows.GridEX.ThemedArea.ControlBorder) 
            | Janus.Windows.GridEX.ThemedArea.Cards) 
            | Janus.Windows.GridEX.ThemedArea.Gridlines) 
            | Janus.Windows.GridEX.ThemedArea.CheckBoxes)));
			this.grdExHistory.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
			// 
			// dvHistory
			// 
			this.dvHistory.Table = this.adFileDs.History;
			// 
			// AdFileControl
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.Controls.Add(this.uiPanelAdFile);
			this.Font = global::AdManagerClient.Properties.Settings.Default.Font1;
			this.Name = "AdFileControl";
			this.Size = new System.Drawing.Size(1010, 677);
			this.Load += new System.EventHandler(this.UserControl_Load);
			((System.ComponentModel.ISupportInitialize)(this.uiPM)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.uiPanelAdFile)).EndInit();
			this.uiPanelAdFile.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.uiPanelSearch)).EndInit();
			this.uiPanelSearch.ResumeLayout(false);
			this.uiPanelSearchContainer.ResumeLayout(false);
			this.pnlSearch.ResumeLayout(false);
			this.pnlSearch.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.uiPanelList)).EndInit();
			this.uiPanelList.ResumeLayout(false);
			this.uiPanelListContainer.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.grdExAdFileList)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.dvAdFile)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.adFileDs)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.uiPanelDetail)).EndInit();
			this.uiPanelDetail.ResumeLayout(false);
			this.uiPanelDetailContainer.ResumeLayout(false);
			this.pnlUserDetail.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.tabFile)).EndInit();
			this.tabFile.ResumeLayout(false);
			this.TPDetail.ResumeLayout(false);
			this.TPDetail.PerformLayout();
			this.TPReplace.ResumeLayout(false);
			this.panel_Replace.ResumeLayout(false);
			this.panel_Replace.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.gridEX_Replace)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.dvFileReplace)).EndInit();
			this.TPPub.ResumeLayout(false);
			this.panel1.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.grdExHistory)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.dvHistory)).EndInit();
			this.ResumeLayout(false);

        }
		#endregion

		#region ÄÁÆ®·Ñ ·Îµå
		private void UserControl_Load(object sender, System.EventArgs e)
		{
			Application.DoEvents();

			// µ¥ÀÌÅÍ°ü¸®¿ë °´Ã¼»ý¼º
			dt = ((DataView)grdExAdFileList.DataSource).Table;  
			cm = (CurrencyManager) this.BindingContext[grdExAdFileList.DataSource]; 
			cm.PositionChanged += new System.EventHandler(OnGrdRowChanged); 

			dtRep = ((DataView)gridEX_Replace.DataSource).Table;  
			cmRep = (CurrencyManager) this.BindingContext[gridEX_Replace.DataSource]; 
			cmRep.PositionChanged += new System.EventHandler(OnGrdRowChangedReplace); 

			// ÄÁÆ®·Ñ ÃÊ±âÈ­
			InitControl();	
		}

		#endregion

		#region ÄÁÆ®·Ñ ÃÊ±âÈ­
		private void InitControl()
		{
			ProgressStart();
			InitCombo();

			// ±ÇÇÑ ¼³Á¤
			if(menu.CanRead(MenuCode))      canRead = true;

            if(menu.CanUpdate(MenuCode))    canUpdate = true;

            if (menu.CanUpdate(MenuCode)) ResetFileInfoReadonly();
            else
            {
                SetFileInfoReadonly();
            }
            
			ResetDetailText();

            // FTP°ü·ÃÁ¤º¸
			InitFtpInfo();

			createFtp();

			InitButton();
			ProgressStop();

			// 2007.10.23
			if(FrameSystem.m_ClientType != FrameSystem._REAL)
			{
				chkFileUploadUseYN.Visible = true;
			}
			else
			{
				chkFileUploadUseYN.Visible = false;
			}

			if(canRead) SearchAdFile();
		}


		private void InitCombo()
		{
			//Init_RapCode();
			//Init_AgencyCode();
			//Init_AdvertiserCode();
			
			Init_AdType();
			Init_FileType();

			InitCombo_Level();
		}
		
		private void Init_AdType()
		{
			// ÄÚµå¿¡¼­ ±¤°íÁ¾·ù¸¦ Á¶È¸ÇÑ´Ù.
			CodeModel codeModel = new CodeModel();
			codeModel.Section = "26";				// ÄÚµåºÐ·ù '26':±¤°íÁ¾·ù TODO: ÄÚµåºÐ·ù´Â ÃßÈÄ XML·Î °ü¸®µÇ¾î¾ß...
			new CodeManager(systemModel, commonModel).GetCodeList(codeModel);
			
			if (codeModel.ResultCD.Equals("0000"))
			{
				// µ¥ÀÌÅÍ¼Â¿¡ ¼ÂÆÃ
				Utility.SetDataTable(adFileDs.AdType, codeModel.CodeDataSet);				
			}

			// °Ë»öÁ¶°ÇÀÇ ÄÞº¸
			this.cbSearchAdType.Items.Clear();
			
			// ÄÞº¸¹Ú½º¿¡ ¼ÂÆ®ÇÒ ÄÚµå¸ñ·ÏÀ» ´ãÀ» Item¹è¿­À» ¼±¾ð
			Janus.Windows.EditControls.UIComboBoxItem[] comboItems = new Janus.Windows.EditControls.UIComboBoxItem[codeModel.ResultCnt + 1];

			comboItems[0] = new Janus.Windows.EditControls.UIComboBoxItem("±¤°íÁ¾·ù¼±ÅÃ","00");
			
			for(int i=0;i<codeModel.ResultCnt;i++)
			{
				DataRow row = adFileDs.AdType.Rows[i];

				string val = row["Code"].ToString();
				string txt = row["CodeName"].ToString();
				comboItems[i+1] = new Janus.Windows.EditControls.UIComboBoxItem(txt,val);
			}
			// ÄÞº¸¿¡ ¼ÂÆ®
			this.cbSearchAdType.Items.AddRange(comboItems);
			this.cbSearchAdType.SelectedIndex = 0;

			Application.DoEvents();
		}

		private void Init_FileType()
		{
			// ÄÚµå¿¡¼­ º¸¾È·¹º§À» Á¶È¸ÇÑ´Ù.
			CodeModel codeModel = new CodeModel();
			codeModel.Section = "24";				// ÄÚµåºÐ·ù '24':±¤°íÆÄÀÏ±¸ºÐ  TODO: ÄÚµåºÐ·ù´Â ÃßÈÄ XML·Î °ü¸®µÇ¾î¾ß...
			new CodeManager(systemModel, commonModel).GetCodeList(codeModel);
			
			if (codeModel.ResultCD.Equals("0000"))
			{
				// µ¥ÀÌÅÍ¼Â¿¡ ¼ÂÆÃ
				Utility.SetDataTable(adFileDs.FileType, codeModel.CodeDataSet);				
			}

			// »ó¼¼Á¶È¸ ÄÞº¸
			// »ó¼¼Á¤º¸ÀÇ ÄÞº¸´Â DatasetÀ» µ¥ÀÌÅÍ¼Ò½º·Î °¡Áø´Ù.

			// °Ë»öÁ¶°ÇÀÇ ÄÞº¸
			this.cbSearchFileType.Items.Clear();
			
			// ÄÞº¸¹Ú½º¿¡ ¼ÂÆ®ÇÒ ÄÚµå¸ñ·ÏÀ» ´ãÀ» Item¹è¿­À» ¼±¾ð
			Janus.Windows.EditControls.UIComboBoxItem[] comboItems = new Janus.Windows.EditControls.UIComboBoxItem[codeModel.ResultCnt + 1];

			comboItems[0] = new Janus.Windows.EditControls.UIComboBoxItem("±¤°íÆÄÀÏ±¸ºÐ¼±ÅÃ","00");
			
			for(int i=0;i<codeModel.ResultCnt;i++)
			{
				DataRow row = adFileDs.FileType.Rows[i];

				string val = row["Code"].ToString();
				string txt = row["CodeName"].ToString();
				comboItems[i+1] = new Janus.Windows.EditControls.UIComboBoxItem(txt,val);
			}
			// ÄÞº¸¿¡ ¼ÂÆ®
			this.cbSearchFileType.Items.AddRange(comboItems);
			this.cbSearchFileType.SelectedIndex = 0;

			Application.DoEvents();
		}

		private void InitCombo_Level()
		{
			Application.DoEvents();
		}
		
		private void InitButton()
		{
			if(canRead)   btnSearch.Enabled = true;

			lbFileFlow.Visible = false;
			lbTimeSpan.Visible = false;
			progBar.Visible    = false;

			grdExAdFileList.Focus();

			Application.DoEvents();
		}

		private void DisableButton()
		{
			btnSearch.Enabled = false;
			btnSave.Enabled   = false;

			btnAdFileSearch.Enabled = false;
			btnRegFileSearch.Enabled = false;

			Application.DoEvents();
		}

		#endregion

		#region ±¤°íÆÄÀÏ ¾×¼ÇÃ³¸® ¸Þ¼Òµå

		/// <summary>
		/// ±×¸®µåÀÇ Rowº¯°æ½Ã
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void OnGrdRowChanged(object sender, System.EventArgs e) 
		{
            if (!IsSearching) // 2011.11.29 JH.Park Á¶È¸ÁßÀÌ ¾Æ´Ò°æ¿ì¿¡¸¸ µ¿ÀÛÇÏµµ·Ï º¯°æ
            {
                SetDetailText();
                InitButton();
            }
		}

		private void OnGrdRowChangedReplace(object sender, System.EventArgs e) 
		{
            int curRow = cmRep.Position;
			try
			{
				if(curRow < 0) return;	// µ¥ÀÌÅÍ°¡ ¾øÀ¸¸é ½ÇÇàÇÏÁö ¾Ê´Â´Ù.

				p2_FileLength.Text	=	Convert.ToInt32(dtRep.Rows[curRow]["FileLength"].ToString()).ToString("##,##0 Bytes");
				p2_PreFileName.Text	=	dtRep.Rows[curRow]["PreFileName"].ToString();
				p2_FileRegDt.Text	=	dtRep.Rows[curRow]["FileRegDt"].ToString();
				p2_FileRegId.Text	=	dtRep.Rows[curRow]["FileRegID"].ToString();
				p2_CheckDt.Text		=	dtRep.Rows[curRow]["CheckDt"].ToString();
				p2_CheckId.Text		=	dtRep.Rows[curRow]["CheckName"].ToString();
				p2_CdnPubDt.Text	=	dtRep.Rows[curRow]["CdnPubDt"].ToString();
				p2_CdnPubId.Text	=	dtRep.Rows[curRow]["CdnPubName"].ToString();
				p2_StbDelDt.Text	=	dtRep.Rows[curRow]["StbDelDt"].ToString();
				p2_StbDelId.Text	=	dtRep.Rows[curRow]["StbDelName"].ToString();
			}
			catch(Exception)
			{
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
			SearchAdFile();
			InitButton();
			ProgressStop();
		}

		/// <summary>
		/// ÀúÀå¹öÆ° Å¬¸¯
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnSave_Click(object sender, System.EventArgs e)
		{
			SaveAdFile();
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
				SearchAdFile();
			}
		}

		#endregion

		#region Ã³¸®¸Þ¼Òµå

		/// <summary>
		/// ±¤°íÆÄÀÏ¸ñ·Ï Á¶È¸
		/// </summary>
		private void SearchAdFile()
		{
            IsSearching = true;
			StatusMessage("±¤°íÆÄÀÏ Á¤º¸¸¦ Á¶È¸ÇÕ´Ï´Ù.");
			ProgressStart();

			try
			{
				adFileModel.Init();

				// µ¥ÀÌÅÍ¸ðµ¨¿¡ Àü¼ÛÇÒ ³»¿ëÀ» ¼ÂÆ®ÇÑ´Ù.
				if(IsNewSearchKey)
				{
					adFileModel.SearchKey = "";
				}
				else
				{
					adFileModel.SearchKey  = ebSearchKey.Text;
				}

				adFileModel.SearchMediaCode = "1";
				adFileModel.SearchRapCode = "";
				adFileModel.SearchAgencyCode = "";
				adFileModel.SearchAdvertiserCode = "";
				adFileModel.SearchAdType   =  cbSearchAdType.SelectedItem.Value.ToString();  
				adFileModel.SearchFileType = cbSearchFileType.SelectedItem.Value.ToString();
				if(chkAdState_10.Checked)   adFileModel.SearchchkAdState_10   = "Y";
				if(chkAdState_20.Checked)   adFileModel.SearchchkAdState_20   = "Y";
				if(chkAdState_30.Checked)   adFileModel.SearchchkAdState_30   = "Y";
				if(chkAdState_40.Checked)   adFileModel.SearchchkAdState_40   = "Y";
				if(chkFileState_10.Checked) adFileModel.SearchchkFileState_10 = "Y";
				//if(chkFileState_11.Checked) adFileModel.SearchchkFileState_11 = "Y";
				if(chkFileState_12.Checked) adFileModel.SearchchkFileState_12 = "Y";
				//if(chkFileState_15.Checked) adFileModel.SearchchkFileState_15 = "Y";
				if(chkFileState_20.Checked) adFileModel.SearchchkFileState_20 = "Y";
				if(chkFileState_30.Checked) adFileModel.SearchchkFileState_30 = "Y";
				if(chkFileState_90.Checked) adFileModel.SearchchkFileState_90 = "Y";


				// ±¤°íÆÄÀÏ¸ñ·ÏÁ¶È¸ ¼­ºñ½º¸¦ È£ÃâÇÑ´Ù.
				new AdFileManager(systemModel,commonModel).GetAdFileList(adFileModel);

				if (adFileModel.ResultCD.Equals("0000"))
				{
					Utility.SetDataTable(adFileDs.AdFile, adFileModel.AdFileDataSet);		
					StatusMessage(adFileModel.ResultCnt + "°ÇÀÇ ±¤°íÆÄÀÏ Á¤º¸°¡ Á¶È¸µÇ¾ú½À´Ï´Ù.");
					AddSchChoice();									
					SetDetailText();
				}
			}
			catch(FrameException fe)
			{
				FrameSystem.showMsgForm("±¤°íÆÄÀÏÁ¶È¸¿À·ù", new string[] {fe.ErrCode, fe.ResultMsg});
			}
			catch(Exception ex)
			{
				FrameSystem.showMsgForm("±¤°íÆÄÀÏÁ¶È¸¿À·ù",new string[] {"",ex.Message});
			}
			finally
			{
                IsSearching = false; // Á¶È¸Áß Flag ¸®¼Â
				ProgressStop();
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
				if ( adFileDs.Tables["AdFile"].Rows.Count < 1 ) return;
              
				foreach (DataRow row in adFileDs.Tables["AdFile"].Rows)
				{					
					
					if(row["ItemNo"].ToString().Equals(keyItemNo))
					{					
						cm.Position = rowIndex;
						break;								
					}			

					rowIndex++;
					grdExAdFileList.EnsureVisible();
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
		/// ÆÄÀÏ¹èÆ÷½ÂÀÎ History
		/// </summary>
		private void SchedulePublish()
		{
			StatusMessage("ÆÄÀÏ¹èÆ÷ÀÌ·Â¸¦ Á¶È¸ÇÕ´Ï´Ù.");
		
			try
			{
				adFileModel.Init();
				adFileModel.SearchMediaCode = "1";
				adFileModel.ItemNo			=  keyItemNo;

				// ±¤°í¹èÆ÷½ÂÀÎ Ã³¸® ¼­ºñ½º¸¦ È£ÃâÇÑ´Ù.
				new AdFileManager(systemModel,commonModel).GetPublishHistory(adFileModel);

				if (adFileModel.ResultCD.Equals("0000"))
				{
					Utility.SetDataTable(adFileDs.History, adFileModel.AdFileDataSet);		
					StatusMessage(adFileModel.ResultCnt + "°ÇÀÇ ±¤°íÆÄÀÏ Á¤º¸°¡ Á¶È¸µÇ¾ú½À´Ï´Ù.");
				}				
				filePubCnt = adFileModel.ResultCnt;
			}
			catch(FrameException fe)
			{
				FrameSystem.showMsgForm("ÆÄÀÏ¹èÆ÷ÀÌ·Â Á¶È¸ ¿À·ù", new string[] {fe.ErrCode, fe.ResultMsg});
			}
			catch(Exception ex)
			{
				FrameSystem.showMsgForm("ÆÄÀÏ¹èÆ÷ÀÌ·Â Á¶È¸ ¿À·ù",new string[] {"",ex.Message});
			}
		}


		/// <summary>
		/// ÆÄÀÏ±³Ã¼ÀÌ·Â Á¶È¸
		/// </summary>
		private void FileRepHistoryList()
		{
			try
			{
				adFileModel.Init();
				adFileModel.SearchMediaCode = "1";
				adFileModel.ItemNo			=  keyItemNo;

				// ±¤°í¹èÆ÷½ÂÀÎ Ã³¸® ¼­ºñ½º¸¦ È£ÃâÇÑ´Ù.
				new AdFileManager(systemModel,commonModel).GetFileRePublishHistory(adFileModel);

				if (adFileModel.ResultCD.Equals("0000"))
				{
					adFileDs.AdFileHistory.Clear();
					Utility.SetDataTable(adFileDs.AdFileHistory, adFileModel.AdFileDataSet);		
					StatusMessage(adFileModel.ResultCnt + "°ÇÀÇ ±¤°íÆÄÀÏ Á¤º¸°¡ Á¶È¸µÇ¾ú½À´Ï´Ù.");
				}
				fileRepCnt = adFileModel.ResultCnt;
			}
			catch(FrameException fe)
			{
				FrameSystem.showMsgForm("ÆÄÀÏ±³Ã¼ÀÌ·Â Á¶È¸ ¿À·ù", new string[] {fe.ErrCode, fe.ResultMsg});
			}
			catch(Exception ex)
			{
				FrameSystem.showMsgForm("ÆÄÀÏ±³Ã¼ÀÌ·Â Á¶È¸ ¿À·ù",new string[] {"",ex.Message});
			}
		}


		/// <summary>
		/// ±¤°íÆÄÀÏ»ó¼¼Á¤º¸ ÀúÀå
		/// </summary>
		private void SaveAdFile()
		{
			StatusMessage("±¤°íÆÄÀÏ Á¤º¸¸¦ ÀúÀåÇÕ´Ï´Ù.");

			// µî·ÏÆÄÀÏÃ£±â·Î µé¾î¿Â°æ¿ì
			if(Flag.Equals("Y"))
			{
				#region µî·ÏÆÄÀÏ Ã£±â Ã³¸®
				ProgressStart();
				// ÆÄÀÏÁ¤º¸ ÀúÀå
				try
				{					
					adFileModel.Init();

					// µ¥ÀÌÅÍ¸ðµ¨¿¡ Àü¼ÛÇÒ ³»¿ëÀ» ¼ÂÆ®ÇÑ´Ù.
					adFileModel.ItemNo         = keyItemNo;
					adFileModel.newItemNo      = newItemNo;
					adFileModel.ItemName       = ebItemName.Text.Trim();
					adFileModel.FileType       = cbFileType.SelectedValue.ToString(); 
					adFileModel.FileLength     = ebFileLength.Text.Trim().Replace(",","");
					adFileModel.FilePath       = ebFilePath.Text.Trim();
					adFileModel.PreFileName    = ebPreFileName.Text.Trim();
					adFileModel.FileName       = ebFileName.Text.Trim();
					adFileModel.DownLevel      = cbDownLevel.SelectedValue.ToString();

					// ±¤°íÆÄÀÏ µî·Ï ¼­ºñ½º¸¦ È£ÃâÇÑ´Ù.
					new AdFileManager(systemModel,commonModel).SetFileUpdate(adFileModel);
            
					StatusMessage("±¤°íÆÄÀÏ Á¤º¸°¡ µî·ÏµÇ¾ú½À´Ï´Ù.");
            
					ResetDetailText();
					DisableButton();
					SearchAdFile();
					InitButton();				
					
				}
				catch(FrameException fe)
				{
					FrameSystem.showMsgForm("±¤°íÆÄÀÏÁ¤º¸ ÀúÀå ¿À·ù", new string[] {fe.ErrCode, fe.ResultMsg});
				}
				catch(Exception ex)
				{
					FrameSystem.showMsgForm("±¤°íÆÄÀÏÁ¤º¸ ÀúÀå ¿À·ù",new string[] {"",ex.Message});
				}			
				ProgressStop();
				#endregion
			}
			else
			{
				#region ÀÔ·ÂÇ×¸ñ°ËÁõ
				if(ebFileName.Text.Trim().Length == 0) 
				{
					MessageBox.Show("±¤°íÆÄÀÏ¸íÀÌ ÀÔ·ÂµÇÁö ¾Ê¾Ò½À´Ï´Ù.","±¤°íÆÄÀÏÁ¤º¸ ÀúÀå", MessageBoxButtons.OK, MessageBoxIcon.Information );
					return;					
				}

				if(ebFileLength.Text.Trim().Length == 0) 
				{
					MessageBox.Show("±¤°íÆÄÀÏÅ©±â°¡ ÀÔ·ÂµÇÁö ¾Ê¾Ò½À´Ï´Ù.","±¤°íÆÄÀÏÁ¤º¸ ÀúÀå", MessageBoxButtons.OK, MessageBoxIcon.Information );
					return;					
				}
			
				if(ebFilePath.Text.Trim().Length == 0) 
				{
					MessageBox.Show("±¤°íÆÄÀÏÀ§Ä¡°¡ ÀÔ·ÂµÇÁö ¾Ê¾Ò½À´Ï´Ù.","±¤°íÆÄÀÏÁ¤º¸ ÀúÀå", MessageBoxButtons.OK, MessageBoxIcon.Information );
					return;					
				}
				#endregion

				#region Å×½ºÆ®°Ë¼ö¼­¹ö¿¡ ÆÄÀÏ¾÷·ÎµåÃ³¸®
				if((chkFileUploadUseYN.Visible == false) || ((chkFileUploadUseYN.Visible == true)  && chkFileUploadUseYN.Checked))
				{
					// ÆÄÀÏÀ» ¼±ÅÃÇÏ¿´À¸¸é ÆÄÀÏ FTP¾÷·Îµå
					if (ebLocalFile.Text.Trim().Length == 0)
					{
						MessageBox.Show("±¤°íÆÄÀÏÀÌ ¼±ÅÃµÇÁö ¾Ê¾Ò½À´Ï´Ù.","±¤°íÆÄÀÏÁ¤º¸ ÀúÀå", MessageBoxButtons.OK, MessageBoxIcon.Information );
						return;					
					}

					DialogResult result  = MessageBox.Show("["+ebItemName.Text + "]À»(¸¦) ¾÷·Îµå ÇÏ½Ã°Ú½À´Ï±î?\nÆÄÀÏ¸í:" + ebFileName.Text 
						, "ÆÄÀÏ ¾÷·Îµå"
						,MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
				
					if (result  != DialogResult.Yes)	return;

					// ¸Þ½ÃÁöÃ¢À» ´Ý±âÀ§ÇØ Àá±ñ ½¬¾ú´Ù°¡..
					Application.DoEvents();				
					Thread.Sleep(100);				
					ProgressStart();

					try
					{

						if (ftm.IsConnected == false)
							ftm.ConnectFtp();

						// °°ÀºÀ§Ä¡¿¡ °°ÀºÆÄÀÏ¸íÀ¸·Î Á¸ÀçÇÏ´ÂÁö °Ë»ç
						if (checkFile(ebFilePath.Text, ebFileName.Text))
						{
							FrameSystem.showMsgForm("ÆÄÀÏ ¾÷·Îµå ¿À·ù",new string[] {"ÆÄÀÏ¸í: " + ebFilePath.Text + "/" + this.ebFileName.Text, "°°Àº ÀÌ¸§ÀÇ ÆÄÀÏÀÌ Á¸ÀçÇÕ´Ï´Ù.", "´Ù¸¥ ÀÌ¸§À¸·Î º¯°æÇÏ¼¼¿ä!"});
							ProgressStop();
							return;
						}

						string remotePath = "";

						try
						{				
							ftm.ChangeDir(ebFilePath.Text);	//ÇØ´ç À§Ä¡·Î µð·ºÅÍ¸¯¸¦ º¯°æÇÑ´Ù.
						}
						catch
						{
							//¿À·ù°¡ ³­´Ù¸é ÇØ´ç µð·ºÅÍ¸®°¡ ¾ø´Â°Í...
							//»õ·Î ¸¸µç´Ù.
							ftm.MakeDir(ebFilePath.Text);
						}

						// ÇØ´ç µð·ºÅÍ¸®¸¦ º¯°æÇÑ´Ù.
						remotePath = ftm.ChangeDir(ebFilePath.Text);

						// ÁøÇà¹Ù¸¦ º¸¿©ÁØ´Ù.
						lbFileFlow.Visible = true;
						lbTimeSpan.Visible = true;
						progBar.Visible    = true;

						ftm.Upload(ebLocalFile.Text, ebFileName.Text);
					}
					catch(Exception ex)
					{
						ProgressStop();
						FrameSystem.showMsgForm("ÆÄÀÏ ¾÷·Îµå ¿À·ù",new string[] {"",ex.Message});					
						return;
					}			
					ProgressStop();
				}
				#endregion

				#region ÆÄÀÏ»óÅÂ¸¦ º¯°æÇÑ´Ù(°Ë¼ö´ë±â)
				try
				{
					adFileModel.Init();

					// µ¥ÀÌÅÍ¸ðµ¨¿¡ Àü¼ÛÇÒ ³»¿ëÀ» ¼ÂÆ®ÇÑ´Ù.
					adFileModel.ItemNo         = keyItemNo;
					adFileModel.ItemName       = ebItemName.Text.Trim();
					adFileModel.FileType       = cbFileType.SelectedValue.ToString(); 
					adFileModel.FileLength     = ebFileLength.Text.Trim().Replace(",","");
					adFileModel.FilePath       = ebFilePath.Text.Trim();
					adFileModel.PreFileName    = ebPreFileName.Text.Trim();
					adFileModel.FileName       = ebFileName.Text.Trim();
					adFileModel.DownLevel      = cbDownLevel.SelectedValue.ToString();

					// ±¤°íÆÄÀÏ µî·Ï ¼­ºñ½º¸¦ È£ÃâÇÑ´Ù.
					new AdFileManager(systemModel,commonModel).SetAdFileUpdate(adFileModel);
                
					StatusMessage("±¤°íÆÄÀÏ Á¤º¸°¡ µî·ÏµÇ¾ú½À´Ï´Ù.");
                
					ResetDetailText();
					DisableButton();
					SearchAdFile();
					InitButton();
				}
				catch(FrameException fe)
				{
					FrameSystem.showMsgForm("±¤°íÆÄÀÏÁ¤º¸ ÀúÀå ¿À·ù", new string[] {fe.ErrCode, fe.ResultMsg});
				}
				catch(Exception ex)
				{
					FrameSystem.showMsgForm("±¤°íÆÄÀÏÁ¤º¸ ÀúÀå ¿À·ù",new string[] {"",ex.Message});
				}
				#endregion
			}
		}


		private	int	filePubCnt	= 0;
		private	int	fileRepCnt	= 0;

		/// <summary>
		/// ±¤°íÆÄÀÏ »ó¼¼Á¤º¸ÀÇ ¼ÂÆ®
		/// </summary>
		private void SetDetailText()
		{
			int curRow = cm.Position;
			if(curRow < 0) return;	// µ¥ÀÌÅÍ°¡ ¾øÀ¸¸é ½ÇÇàÇÏÁö ¾Ê´Â´Ù.

			ResetDetailText();
			keyItemNo                 = dt.Rows[curRow]["ItemNo"].ToString();
			keyFileState              = dt.Rows[curRow]["FileState"].ToString();
			keyAdState                = dt.Rows[curRow]["AdState"].ToString();
			keyAdType				  = dt.Rows[curRow]["AdType"].ToString();
			ebItemName.Text           = dt.Rows[curRow]["ItemName"].ToString();
			ebFileState.Text          = dt.Rows[curRow]["FileStateName"].ToString();
		
			cbFileType.SelectedValue  = dt.Rows[curRow]["FileType"].ToString();
			if(dt.Rows[curRow]["FileLength"].ToString().Length > 0)
			{
				ebFileLength.Text         = String.Format("{0:#,##0}",Convert.ToInt32(dt.Rows[curRow]["FileLength"].ToString()));
			}
			ebFilePath.Text           = dt.Rows[curRow]["FilePath"].ToString();
			ebPreFileName.Text        = dt.Rows[curRow]["PreFileName"].ToString();
			ebFileName.Text           = dt.Rows[curRow]["FileName"].ToString();
			cbDownLevel.SelectedValue = dt.Rows[curRow]["DownLevel"].ToString();
			ebRegDt.Text              = dt.Rows[curRow]["FileRegDt"].ToString();
			ebRegName.Text            = dt.Rows[curRow]["FileRegName"].ToString();
			ebAdTime.Text             = dt.Rows[curRow]["AdTime"].ToString();
			//ebCheckDt.Text            = dt.Rows[curRow]["CheckDt"].ToString();
			//ebCheckName.Text          = dt.Rows[curRow]["CheckName"].ToString();
			//ebCDNSyncDt.Text          = dt.Rows[curRow]["CDNSyncDt"].ToString();
			//ebCDNSyncName.Text        = dt.Rows[curRow]["CDNSyncName"].ToString();
			ebCDNPubDt.Text           = dt.Rows[curRow]["CDNPubDt"].ToString();
			ebCDNPubName.Text         = dt.Rows[curRow]["CDNPubName"].ToString();
			ebSTBDelDt.Text           = dt.Rows[curRow]["STBDelDt"].ToString();
			ebSTBDelName.Text         = dt.Rows[curRow]["STBDelName"].ToString();
			Flag = "";
			adFileDs.History.Clear();

			SchedulePublish();			// ÆÄÀÏ¹èÆ÷ÀÌ·ÂÁ¶È¸
			FileRepHistoryList();		// ÆÄÀÏ±³Ã¼ÀÌ·ÂÁ¶È¸

			TPPub.Text = "¹èÆ÷ÀÌ·Â( "		+ filePubCnt.ToString() + " )";
			TPReplace.Text = "±³Ã¼ÀÌ·Â( "	+ fileRepCnt.ToString() + " )";


			// ÆÄÀÏÀÇ »óÅÂ
			// 10:¹Ìµî·Ï		11:¼ÒÀç±³Ã¼´ë±â		12:°Ë¼ö´ë±â	15:¹èÆ÷´ë±â	20:CDNµ¿±âÈ­	30:¹èÆ÷¿Ï·á	90:¼ÂÅ¾»èÁ¦
			if(keyFileState.Equals("12") || keyFileState.Equals("15") || keyFileState.Equals("20") || keyFileState.Equals("30") || keyFileState.Equals("90"))
				btnView.Enabled = true;
			else
				btnView.Enabled = false;

			if(keyFileState.Equals("10") || keyFileState.Equals("11") || keyFileState.Equals("12") || keyFileState.Equals("15") || keyFileState.Equals("20"))	// ÆÄÀÏ»óÅÂ°¡ ¹èÆ÷È®ÀÎ ¶Ç´Â ¼ÂÅ¾»èÁ¦ »óÅÂ°¡ ¾Æ´Ï¸é ¼öÁ¤°¡´É
			{
				// ÁØºñ»óÅÂÀÌº¯ ±âº»°ªÀ» ¼ÂÆ®
				if(keyFileState.Equals("10"))
				{
					ResetDetailText();
				}
				// ¼öÁ¤°¡´ÉÇÏ°Ô
				ResetFileInfoReadonly();
                
				if(keyFileState.Equals("10") || keyFileState.Equals("11") || keyFileState.Equals("12") )	// ÆÄÀÏ»óÅÂ°¡ ¹Ìµî·Ï ¶Ç´Â °Ë¼ö´ë±â »óÅÂÀÌ¸é ÆÄÀÏ¾÷·Îµå °¡´É
				{
					// ¾÷·Îµå°¡´ÉÇÏµµ·Ï
					SetAutoUpload();
				}
				
				// ¼öÁ¤°¡´É¾È³»¹®±¸ ºñÈ°¼º
				lbNoSave.Visible = false;
			}
			else
			{
				// ¼öÁ¤ºÒ°¡´ÉÇÏ°Ô
				SetFileInfoReadonly();
				// ¼öÁ¤°¡´É¾È³»¹®±¸ È°¼º
				lbNoSave.Visible = true;
			}

			StatusMessage("ÁØºñ");
		}

		private void ResetDetailText()
		{
			ebFileName.Text          = "";
			cbFileType.SelectedIndex = 0;
			cbDownLevel.SelectedIndex = 3;	// Default 3¼øÀ§
			ebFileLength.Text        = "";
			ebFilePath.Text          = strDefaultPath;
			ebRegDt.Text             = "";
			ebRegName.Text           = "";

			ebLocalFile.Text         = "";

			// ÆÄÀÏ±³Ã¼ÀÌ·Â »ó¼¼Ç×¸ñµé
			p2_FileLength.Text	=	"";
			p2_PreFileName.Text	=	"";
			p2_FileRegDt.Text	=	"";
			p2_FileRegId.Text	=	"";
			p2_CheckDt.Text		=	"";
			p2_CheckId.Text		=	"";
			p2_CdnPubDt.Text	=	"";
			p2_CdnPubId.Text	=	"";
			p2_StbDelDt.Text	=	"";
			p2_StbDelId.Text	=	"";
		}
		
		/// <summary>
		/// ÆÄÀÏÁ¤º¸ ¼öÁ¤ºÒ°¡´ÉÄÉ
		/// </summary>
		private void SetFileInfoReadonly()
		{
			cbFileType.ReadOnly		= true;
			cbDownLevel.ReadOnly	= true;

			cbFileType.BackColor	= Color.WhiteSmoke;
			cbDownLevel.BackColor	= Color.WhiteSmoke;

			ebFilePath.ReadOnly      = true;
			ebFilePath.BackColor     = Color.WhiteSmoke;

			ebFileLength.ReadOnly    = true;
			ebFilePath.ReadOnly      = true;
			ebFileName.ReadOnly      = true;

			ebFileLength.BackColor   = Color.WhiteSmoke;
			ebFilePath.BackColor     = Color.WhiteSmoke;
			ebFileName.BackColor     = Color.WhiteSmoke;

			btnAdFileSearch.Enabled  = false;
			btnRegFileSearch.Enabled  = false;
			btnSave.Enabled = false;
		}

		/// <summary>
		/// ÆÄÀÏÁ¤º¸ ¼öÁ¤°¡´ÉÄÉ
		/// </summary>
		private void ResetFileInfoReadonly()
		{
			cbFileType.ReadOnly		= false;
			cbDownLevel.ReadOnly	= false;

			cbFileType.BackColor	= Color.White;
			cbDownLevel.BackColor	= Color.White;

			// ÆÄÀÏ¸í ¹× ÆÄÀÏÅ©±â´Â ÀÓÀÇ ¼öÁ¤ÀÌ ºÒ°¡ÇÏ´Ù.
			ebFilePath.ReadOnly      = true;
			ebFileName.ReadOnly      = true;
			ebFileLength.ReadOnly    = true;

			ebFilePath.BackColor     = Color.WhiteSmoke;
			ebFileName.BackColor     = Color.WhiteSmoke;
			ebFileLength.BackColor   = Color.WhiteSmoke;

			if(canUpdate)
			{
				btnSave.Enabled = true;
			}
			btnAdFileSearch.Enabled  = false;
			btnRegFileSearch.Enabled  = false;
		}

		/// <summary>
		/// ÀÚµ¿¾÷·Îµå °¡´ÉÇÏ°Ô
		/// </summary>
		private void SetAutoUpload()
		{
			// ÆÄÀÏ¸í ¹× ÆÄÀÏÅ©±â´Â ÀÓÀÇ ¼öÁ¤ÀÌ ºÒ°¡ÇÏ´Ù.
			// ÆÄÀÏ¾÷·Îµå¸¦ ÅëÇØ¼­¸¸ ¼öÁ¤ÇÒ ¼ö ÀÖ´Ù.
			ebFilePath.ReadOnly      = false;
			ebFilePath.BackColor     = Color.White;

			// 2007.10.23
			// ¸¸¾à Å×½ºÆ®¶ó¸é ÆÄÀÏ¸íÀ» ¼öÁ¤ÇÒ ¼ö ÀÖµµ·ÏÇÑ´Ù. 
			if(FrameSystem.m_ClientType != FrameSystem._REAL)
			{
				if(chkFileUploadUseYN.Visible && !chkFileUploadUseYN.Checked)
				{
					ebFileName.ReadOnly      = false;
					ebFileLength.ReadOnly    = false;

					ebFileName.BackColor     = Color.White;
					ebFileLength.BackColor   = Color.White;
				}
			}


			// ¾÷·Îµå¹öÆ° È°¼ºÈ­
			if(canUpdate)
			{
				btnAdFileSearch.Enabled  = true;
				btnRegFileSearch.Enabled  = true;
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

		#region FTPÃ³¸®ÇÔ¼ö

        /// <summary>
        /// DB¿¡¼­ FTP°ü·Ã Á¤º¸¸¦ ÀÐ¾î¿Â´Ù.
        /// </summary>
		private void InitFtpInfo()
		{
			try
			{
				adFileModel.Init();

				// ±¤°íÆÄÀÏ¸ñ·ÏÁ¶È¸ ¼­ºñ½º¸¦ È£ÃâÇÑ´Ù.
				new AdFileManager(systemModel,commonModel).GetFtpConfig(adFileModel);

				if (adFileModel.ResultCD.Equals("0000"))
				{
					FtpUploadHost  = adFileModel.FtpUploadHost;
					FtpUploadPort  = adFileModel.FtpUploadPort;
					FtpUploadID    = adFileModel.FtpUploadID;
					FtpUploadPW    = Security.Decrypt(adFileModel.FtpUploadPW);

					strDefaultPath = adFileModel.FtpUploadPath;
				}
				else
				{
					FtpUploadHost = "211.186.175.14";
					FtpUploadPort = "21";
					FtpUploadID   = "epgrenew";
					FtpUploadPW   = Security.Decrypt("xfvMYBEbQbxzIiN7PAugog==");

					strDefaultPath = "/adv";
				}

			}
			catch(FrameException fe)
			{
				FrameSystem.showMsgForm("FTP¾÷·Îµå Á¤º¸Á¶È¸ ¿À·ù", new string[] {fe.ErrCode, fe.ResultMsg});
			}
			catch(Exception ex)
			{
				FrameSystem.showMsgForm("FTP¾÷·Îµå Á¤º¸Á¶È¸ ¿À·ù",new string[] {"",ex.Message});
			}
		}

		private void createFtp()
		{
			//--------------
			// Ftp °´Ã¼ »ý¼º
			//--------------
			try
			{
				if (ftm == null)
				{
					ftm = new FtpManager();
					ftm.OnPosition		+= new PositionDelegate(ftm_OnPosition);
					ftm.OnMaxPosition	+= new PositionDelegate(ftm_OnMaxPosition);

					ftm.SetIpAddress	= FtpUploadHost;
					ftm.SetPort			= Convert.ToInt32(FtpUploadPort);
					ftm.SetUserId		= FtpUploadID;
					ftm.SetUserPwd		= FtpUploadPW;

					//BMK PORT°¡ 0ÀÌ¸é ¾÷·Îµå »ç¿ëÇÏÁö ¾Ê°Ô ¼³Á¤ÇÑ´Ù.
					if (Convert.ToInt32(FtpUploadPort) == 0)
					{
						chkFileUploadUseYN.Checked = false;
						chkFileUploadUseYN.Enabled = false;
					}
					else
					{
						chkFileUploadUseYN.Checked = true;
						chkFileUploadUseYN.Enabled = true;
					}
				}
			}
			catch (Exception ex)
			{
				FrameSystem.oLog.Error("FTP¼­¹ö ¿¬°á¿À·ù:"+ex.Message);
			}
		}

		private bool checkFile(string Path, string FileName)
		{
			//------------------
			// ¼­¹ö»óÀÇ ÆÄÀÏÁ¸Àç¿©ºÎ Ã¼Å©
			//------------------
			if (ftm.IsConnected == false)
			{
				// ¹Ì¿¬°á½Ã 3È¸½Ãµµ
				for(int retry = 3; retry > 0; retry--)
				{
					try
					{
						ftm.Connect();
						if(ftm.IsConnected == true) break;
					}
					catch(Exception)
					{
						Thread.Sleep(500);
					}
				}
			}

			try
			{
				long sz = ftm.GetFileSize(Path + "/" + FileName);
				return true;
			}
			catch
			{
				return false;
			}
		}

		private void ftm_OnPosition(int inx)
		{
			//-----------------------------------
			// ÇöÀç ÆÄÀÏ »óÅÂ ÇÁ·Î±×·¹½º¹Ù¿¡ ±×¸®±â
			//-----------------------------------
			try
			{
				if (firstDraw)
				{
					start = DateTime.Now;	// ¾÷·Îµå ½ÃÀÛ ½Ã°¢
					firstDraw = false;
				}
			
				Application.DoEvents();
			
				if (progBar.Maximum > inx)
				{
					lbFileFlow.Text = string.Format("{0:0,00#}",inx) + " / " + fileMax;
				
					progBar.Value = inx;
				
					timeSpan = DateTime.Now - start;
					tm = Convert.ToInt32(timeSpan.TotalSeconds);
					m = tm / 60;
					s = tm % 60;
					lbTimeSpan.Text  = string.Format("{0:0#}",m) + ":" + string.Format("{0:0#}",s);				
				}
				else if (progBar.Maximum == inx)
				{				
					if(firstDraw) return;
					progBar.Value = 0;
					lbFileFlow.Text = fileMax = string.Empty;
					lbTimeSpan.Text = string.Empty;
					firstDraw = true;
					tm = m = s = 0;						
				}
			}
			catch (Exception ex)
			{
				MessageForm mf = new MessageForm();
				mf.SetMessage = new string[]{"" ,"¾÷·Îµå Áß ¾Ë ¼ö ¾ø´Â ¿¡·¯°¡ ¹ß»ýÇß½À´Ï´Ù.!",ex.Message};
				mf.showMessage();
				mf.ShowDialog();
			}
		}

		private void ftm_OnMaxPosition(int inx)
		{
			//-------------------------------------------------
			// ´Ù¿î·Îµå ÆÄÀÏÀÇ Å©±â °ªÀ» ÇÁ·Î±×·¹½º maxValue¿¡ ¼³Á¤
			//-------------------------------------------------
			fileMax = string.Format("{0:0,00#}",inx);
			progBar.Maximum = inx;
		}

		private void messgaeForm()
		{
			MessageForm msgFrm = new MessageForm();
			msgFrm.SetMessageType = 1;
			msgFrm.SetMessage = new string[]{"","¾÷·Îµå ÁØºñ ÁßÀÔ´Ï´Ù..","Àá½Ã¸¸ ±â´Ù¸®¼¼¿ä.!"};
			msgFrm.Width -= 50;
			msgFrm.showMessage();
			msgFrm.ShowDialog();
		}
		#endregion

		#region ÆÄÀÏ°Ë»ö
		private void btnAdFileSearch_Click(object sender, System.EventArgs e)
		{
			if(Flag.Equals("Y"))
			{
				DialogResult result = MessageBox.Show("µî·ÏÆÄÀÏ°Ë»öÀ» Á¾·áÇÏ°í ÆÄÀÏ°Ë»öÀ» ÇÏ½Ã°Ú½À´Ï±î?\n","ÆÄÀÏ",
					MessageBoxButtons.YesNo, MessageBoxIcon.Question,
					MessageBoxDefaultButton.Button2);

				if (result == DialogResult.No) 
				{
					return;				
				}				
				else
				{
					Flag = "";
					newItemNo = "";					
					SetDetailText();
					try
					{
						openFileDlg.DefaultExt = "ts";
						openFileDlg.Filter	= "B Mobile ADS contents files (*.ts)|*.ts|¸ðµçÆÄÀÏ (*.*)|*.*";	
						openFileDlg.FilterIndex = 1;
						openFileDlg.RestoreDirectory = true;
					
						if (openFileDlg.ShowDialog() == DialogResult.OK)
						{
							Application.DoEvents();

							openFileDlg.ShowReadOnly = true;
																	
							string path = openFileDlg.FileName;
						
							FileStream fs		 =  new FileStream(path, System.IO.FileMode.Open, System.IO.FileAccess.Read);					
						
							ebFileLength.Text	= fileMax = string.Format("{0:0,00#}",fs.Length);
	                                                                   				
							string localName= Path.GetFileName(path);	//ÆÄÀÏ ÀÌ¸§¸¸ - Table ¿¡ µî·ÏµÇ´Â µ¥ÀÌÅ¸

							string ExtName = Path.GetExtension(path);	// È®ÀåÀÚ¸í

							// DB¹× FTP¿¡ ÀúÀåÇÒ ÆÄÀÏ¸íÀ» »ý¼ºÇÑ´Ù.
							// »ý¼º±ÔÄ¢
							// ±ÔÄ¢ Prefix(1) + ±¤°í¹øÈ£(6) + '-' + yyMMddHHmmss + '.ts'
							// Prefix : CM(10)->c OAP(20)->o EAP(11)->o SCM(12)->s ±âÅ¸(ELSE)->x
							// Prefix : ±âº»±¤°í(99)->DefaultAd
						
							string Prefix = "";
							if(keyAdType.Equals("10"))      // PreRoll
							{
								Prefix = "p";
							}
                            else if(keyAdType.Equals("18")) // MidRoll
                            {
                                Prefix = "m";
                            }
                            else if(keyAdType.Equals("19")) // PostRoll
                            {
                                Prefix = "t";
                            }
							else if(keyAdType.Equals("20")) // OAP
							{
								Prefix = "o";
							}
							else if(keyAdType.Equals("11")) // EAP
							{
								Prefix = "o";
							}
							else if(keyAdType.Equals("12")) // SCM
							{
								Prefix = "s";
							}
							else if(keyAdType.Equals("99")) // ±âº»±¤°í
							{
								Prefix = "DefaultAd";
								cbDownLevel.SelectedValue = "1";
							}
							else
							{
								Prefix = "x";
							}
							string Timestmp = DateTime.Now.ToString("yyMMddHHmmss");
							
							if(Prefix.Equals("DefaultAd"))
							{
								// ÀúÀåÇÒ ÆÄÀÏ¸í-±âº»±¤°íÀÏ °æ¿ì´Â ±¤°í¹øÈ£±îÁö¸¸..
								ebFileName.Text = Prefix + keyItemNo + ExtName + "/playlist.m3u8";
							}
							else
							{
								// ÀúÀåÇÒ ÆÄÀÏ¸í
								ebFileName.Text = Prefix + keyItemNo + "-" + Timestmp + ExtName + "/playlist.m3u8";
							}
							// ·ÎÄÃÆÄÀÏ¸í
							ebLocalFile.Text = openFileDlg.FileName;

							progBar.Maximum = Convert.ToInt32(fs.Length * 0.001);
							fs.Close();
						}
					}
					catch(Exception ex)
					{
						FrameSystem.oLog.Error("ÆÄÀÏ¼±ÅÃ¿¹¿Ü:"+ex.Message);
					}
				}
			}
			else
			{
				try
				{
					openFileDlg.DefaultExt = "ts";
					openFileDlg.Filter = "B Mobile ADS contents files (*.ts)|*.ts|¸ðµçÆÄÀÏ (*.*)|*.*";	
					openFileDlg.FilterIndex = 1;
					openFileDlg.RestoreDirectory = true;
					
					if (openFileDlg.ShowDialog() == DialogResult.OK)
					{
						Application.DoEvents();

						openFileDlg.ShowReadOnly = true;
																	
						string path = openFileDlg.FileName;
						
						FileStream fs		 =  new FileStream(path, System.IO.FileMode.Open, System.IO.FileAccess.Read);					
						
						ebFileLength.Text	= fileMax = string.Format("{0:0,00#}",fs.Length);
	                                                                   				
						string localName= Path.GetFileName(path);	//ÆÄÀÏ ÀÌ¸§¸¸ - Table ¿¡ µî·ÏµÇ´Â µ¥ÀÌÅ¸

						string ExtName = Path.GetExtension(path);	// È®ÀåÀÚ¸í

						// DB¹× FTP¿¡ ÀúÀåÇÒ ÆÄÀÏ¸íÀ» »ý¼ºÇÑ´Ù.
						// »ý¼º±ÔÄ¢
						// ±ÔÄ¢ Prefix(1) + ±¤°í¹øÈ£(6) + '-' + yyMMddHHmmss + '.ts'
						// Prefix : CM(10)->c OAP(20)->o EAP(11)->o SCM(12)->s ±âÅ¸(ELSE)->x
						
						string Prefix = "";
						if (keyAdType.Equals("10"))      // PreRoll
						{
							Prefix = "p";
						}
						else if (keyAdType.Equals("18")) // MidRoll
						{
							Prefix = "m";
						}
						else if (keyAdType.Equals("19")) // PostRoll
						{
							Prefix = "t";
						}
						else if (keyAdType.Equals("20")) // OAP
						{
							Prefix = "o";
						}
						else if (keyAdType.Equals("11")) // EAP
						{
							Prefix = "o";
						}
						else if (keyAdType.Equals("12")) // SCM
						{
							Prefix = "s";
						}
						else if (keyAdType.Equals("99")) // ±âº»±¤°í
						{
							Prefix = "DefaultAd";
							cbDownLevel.SelectedValue = "1";
						}
						else
						{
							Prefix = "x";
						}
						string Timestmp = DateTime.Now.ToString("yyMMddHHmmss");	
										
						ebPreFileName.Text = localName;

						if(Prefix.Equals("DefaultAd"))
						{
							// ÀúÀåÇÒ ÆÄÀÏ¸í-±âº»±¤°íÀÏ °æ¿ì´Â ±¤°í¹øÈ£±îÁö¸¸..
							ebFileName.Text = Prefix + keyItemNo + ExtName + "/playlist.m3u8";
						}
						else
						{
							// ÀúÀåÇÒ ÆÄÀÏ¸í
							ebFileName.Text = Prefix + keyItemNo + "-" + Timestmp + ExtName + "/playlist.m3u8"; 
						}
						// ·ÎÄÃÆÄÀÏ¸í
						ebLocalFile.Text = openFileDlg.FileName;

						progBar.Maximum = Convert.ToInt32(fs.Length * 0.001);
						fs.Close();
					}
				}
				catch(FrameException fe)
				{
					FrameSystem.showMsgForm("±¤°íÆÄÀÏÁ¶È¸¿À·ù", new string[] {fe.ErrCode, fe.ResultMsg});
				}
				catch(Exception ex)
				{
					FrameSystem.showMsgForm("±¤°íÆÄÀÏÁ¶È¸¿À·ù",new string[] {"",ex.Message});
				}				
			}
		}

		#endregion

		private void chkFileUploadUseYN_CheckedChanged(object sender, System.EventArgs e)
		{
			if(FrameSystem.m_ClientType != FrameSystem._REAL)
			{
				if(chkFileUploadUseYN.Visible)
				{
					if(keyFileState.Equals("10") || keyFileState.Equals("11") || keyFileState.Equals("12") || keyFileState.Equals("15") || keyFileState.Equals("20"))	// ÆÄÀÏ»óÅÂ°¡ ¹èÆ÷È®ÀÎ ¶Ç´Â ¼ÂÅ¾»èÁ¦ »óÅÂ°¡ ¾Æ´Ï¸é ¼öÁ¤°¡´É
					{
						if(chkFileUploadUseYN.Checked)
						{
							ebFileName.ReadOnly      = true;
							ebFileLength.ReadOnly    = true;

							ebFileName.BackColor     = Color.WhiteSmoke;
							ebFileLength.BackColor   = Color.WhiteSmoke;
						}
						else
						{
							ebFileName.ReadOnly      = false;
							ebFileLength.ReadOnly    = false;

							ebFileName.BackColor     = Color.White;
							ebFileLength.BackColor   = Color.White;
						}
					}
				}
			}		
		}

		private void btnRegFileSearch_Click(object sender, System.EventArgs e)
		{
			//µî·ÏÆÄÀÏ°Ë»ö ¸ñ·Ï °Ë»ö ÆË¾÷ ¶ì¿ì±â
			AdFile_pForm pForm = new AdFile_pForm(this);

			pForm.ShowDialog();
            
			pForm.Dispose();
			pForm = null;	
		}

		/// <summary>
		/// ¼±ÅÃµÈ RowµéÀ» ÀÔ·Â½ÃÅ´
		/// </summary>
		/// <param name="dtc"></param>
		public void adOn_AdFile(AdFileModel adFileModel )
		{
			newItemNo			  = adFileModel.newItemNo;
			ebFileName.Text       = adFileModel.FileName;
			fileState	          = adFileModel.FileState;
			Flag		          = adFileModel.Flag;
		}
		
		private void btnView_Click(object sender, System.EventArgs e)
		{
			try
			{
                AdFile.AdFile_Viewer2 pForm = new AdManagerClient.AdFile.AdFile_Viewer2();

                pForm.FileName = ebFileName.Text.Trim();

                if (keyFileState.Equals("30") || keyFileState.Equals("90"))
                    pForm.FileCDN = true;
                else
                    pForm.FileCDN = false;

                pForm.Show();
                pForm.RunJob();
			}
			catch(Exception ex)
			{
				FrameSystem.showMsgForm("±¤°íÆÄÀÏ º¸±â ¿À·ù",new string[] {"",ex.Message});
			}			
		}

		/// <summary>
		/// ±¤°íÆÄÀÏ »óÅÂ¸¦ ±³Ã¼´ë±â »óÅÂ·Î º¯°æÇÑ´Ù
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnFileReplace_Click(object sender, System.EventArgs e)
		{
			StatusMessage("¼±ÅÃµÈ ±¤°íÆÄÀÏÀ» ¼ÒÀç±³Ã¼ÇÕ´Ï´Ù.");

            DialogResult result  = MessageBox.Show("["+ebItemName.Text + "]ÀÇ ±¤°íÆÄÀÏ ¼ÒÀç¸¦ ±³Ã¼ ÇÕ´Ï´Ù."
                                                    ,"ÆÄÀÏ°ü¸®"
                                                    ,MessageBoxButtons.YesNo
                                                    ,MessageBoxIcon.Warning
                                                    ,MessageBoxDefaultButton.Button2);
				
            if (result  != DialogResult.Yes)	return;

			ProgressStart();
			try
			{				
				adFileWideModel.Init();
				adFileWideModel.MediaCode   = keyMediaCode;
				adFileWideModel.ItemNo		= keyItemNo;
				adFileWideModel.ItemName	= "";
				adFileWideModel.FileName	= "";
				adFileWideModel.FileState   = "";
				new AdManagerClient.AdFileWideManager(systemModel,commonModel).SetAdFileChange(adFileWideModel);						
			
				ProgressStop();
				//ResetDetailText();
				DisableButton();
				SearchAdFile();
				InitButton();

			}
			catch(FrameException fe)
			{
				FrameSystem.showMsgForm("±¤°íÆÄÀÏ ¼ÒÀç±³Ã¼ ¿À·ù", new string[] {fe.ErrCode, fe.ResultMsg});
			}
			catch(Exception ex)
			{
				FrameSystem.showMsgForm("±¤°íÆÄÀÏ ¼ÒÀç±³Ã¼ ¿À·ù",new string[] {"",ex.Message});
			}			
			finally
			{
				ProgressStop();
			}			
		}
	}
}