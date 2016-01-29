// ===============================================================================
// FilePublishControl for Charites Project
//
// FilePublishControl.cs
//
// ÆÄÀÏ¹èÆ÷½ÂÀÎ ÄÁµå·ÑÀ» Á¤ÀÇÇÕ´Ï´Ù. 
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
	/// ÆÄÀÏ¹èÆ÷½ÂÀÎ ÄÁÆ®·Ñ
	/// </summary>
    public class FilePublishControl : System.Windows.Forms.UserControl, IUserControl
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
		FilePublishModel filePublishModel  = new FilePublishModel();	// ÆÄÀÏ¹èÆ÷½ÂÀÎ ¸ðµ¨

		// È­¸éÃ³¸®¿ë º¯¼ö
		CurrencyManager cm      = null;					// µ¥ÀÌÅÍ ±×¸®µåÀÇ º¯°æ¿¡ µû¸¥ µ¥ÀÌÅÍ¼Â °ü¸®¸¦ À§ÇÏ¿©			
		DataTable       dt      = null;

        // ÀÛ¾÷±×¸®µå¿ë
        CurrencyManager cmWork  = null;
        DataTable       dtWork  = null;

        bool IsSearching = false; // Á¶È¸Áß »ó¼¼È­¸éÀÌ ¾÷µ¥ÀÌÆ® µÇ´Â °ÍÀ» ¹æÁö ÇÏ±âÀ§ÇÔ 2011.11.29 JH.Park
		bool canRead			= false;
		bool canUpdate			= false;


		private string  keyAckNo    = "";	// ÇöÀç Ã³¸®ÁßÀÎ ½ÂÀÎ¹øÈ£
		private int     keyAckNoRow	= 0;	// ±×¸®µå »ó¿¡ ¼±ÅÃµÈ ½ÂÀÎ¹øÈ£
		private string  keyAckState = "";
		private string  keyMediaCode= "";
        private string  keyWorkDt   = "";   // ÀÛ¾÷±×¸®µåÀÇ ÀÛ¾÷ÀÏ½Ã
				
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

		private Janus.Windows.EditControls.UIComboBox cbSearchMedia;
		private Janus.Windows.EditControls.UIButton btnSearch;
		private Janus.Windows.UI.Dock.UIPanelManager uiPM;
		private System.Windows.Forms.Panel pnlSearch;
		private Janus.Windows.UI.Dock.UIPanelGroup uiPanelAdFile;
		private Janus.Windows.UI.Dock.UIPanel uiPanelSearch;
		private Janus.Windows.UI.Dock.UIPanelInnerContainer uiPanelSearchContainer;
		private System.ComponentModel.IContainer components;
		private Janus.Windows.UI.Dock.UIPanel uiPanel1;
		private Janus.Windows.UI.Dock.UIPanelInnerContainer uiPanel1Container;
		private Janus.Windows.UI.Dock.UIPanel uiPanel2;
		private Janus.Windows.UI.Dock.UIPanelInnerContainer uiPanel2Container;
		private Janus.Windows.UI.Dock.UIPanelGroup uiPanelConfirm;
		private Janus.Windows.UI.Dock.UIPanel uiPanelSchduleConfirm;
		private Janus.Windows.UI.Dock.UIPanelInnerContainer uiPanelSchduleConfirmContainer;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Panel panel2;
		private Janus.Windows.GridEX.GridEX grdExScheduleList;
		private Janus.Windows.GridEX.GridEX grdExPublishList;
		private Janus.Windows.UI.Dock.UIPanel uiPanelMaster;
		private Janus.Windows.UI.Dock.UIPanelInnerContainer uiPanelMasterContainer;
		private System.Windows.Forms.Panel panel3;
		private System.Windows.Forms.Panel panel5;
		private System.Windows.Forms.Panel panel6;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private Janus.Windows.GridEX.EditControls.EditBox ebPublishDesc;
		private Janus.Windows.EditControls.UIButton btnPublishConfirm;
		private System.Windows.Forms.Label lbMemo2;
		private Janus.Windows.GridEX.EditControls.EditBox ebPublishDay;
		private Janus.Windows.GridEX.EditControls.EditBox ebPublishUserName;
		private System.Windows.Forms.Label label4;
		private Janus.Windows.GridEX.EditControls.EditBox ebAckStateName;
		private System.Windows.Forms.Label label5;
		private Janus.Windows.GridEX.EditControls.EditBox ebModifyStartDay;
		private System.Windows.Forms.Panel panal4;
		private System.Windows.Forms.Label lbMsg;
		private Janus.Windows.UI.Dock.UIPanel uiPanelPublishConfirm;
		private Janus.Windows.UI.Dock.UIPanelInnerContainer uiPanelPublishConfirmContainer;
		private AdManagerClient.FilePublishDs filePublishDs;
		private System.Data.DataView dvHistory;
		private Janus.Windows.UI.Dock.UIPanel uiPanelDetail;
		private Janus.Windows.UI.Dock.UIPanelInnerContainer uiPanelDetailContainer;
		private Janus.Windows.UI.Dock.UIPanelGroup uiPanelPublish;
		private Janus.Windows.UI.Dock.UIPanel uiPanelHistory;
		private Janus.Windows.UI.Dock.UIPanelInnerContainer uiPanelHistoryContainer;
		private Janus.Windows.UI.Dock.UIPanel uiPanelPubDetail;
		private Janus.Windows.UI.Dock.UIPanelInnerContainer uiPanelPubDetailContainer;
		private Janus.Windows.GridEX.GridEX grdExHistory;
		private Janus.Windows.GridEX.GridEX grdExFileList;
        private System.Data.DataView dvFileList;
		private System.Data.DataView dvWork;
		private System.Data.DataView dvFilePublish;

		public FilePublishControl()
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
            Janus.Windows.GridEX.GridEXLayout grdExPublishList_DesignTimeLayout = new Janus.Windows.GridEX.GridEXLayout();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FilePublishControl));
            Janus.Windows.GridEX.GridEXLayout grdExHistory_DesignTimeLayout = new Janus.Windows.GridEX.GridEXLayout();
            Janus.Windows.GridEX.GridEXLayout grdExFileList_DesignTimeLayout = new Janus.Windows.GridEX.GridEXLayout();
            Janus.Windows.GridEX.GridEXLayout grdExScheduleList_DesignTimeLayout = new Janus.Windows.GridEX.GridEXLayout();
            this.uiPM = new Janus.Windows.UI.Dock.UIPanelManager(this.components);
            this.uiPanelAdFile = new Janus.Windows.UI.Dock.UIPanelGroup();
            this.uiPanelSearch = new Janus.Windows.UI.Dock.UIPanel();
            this.uiPanelSearchContainer = new Janus.Windows.UI.Dock.UIPanelInnerContainer();
            this.pnlSearch = new System.Windows.Forms.Panel();
            this.btnSearch = new Janus.Windows.EditControls.UIButton();
            this.cbSearchMedia = new Janus.Windows.EditControls.UIComboBox();
            this.lbMsg = new System.Windows.Forms.Label();
            this.uiPanelConfirm = new Janus.Windows.UI.Dock.UIPanelGroup();
            this.uiPanelSchduleConfirm = new Janus.Windows.UI.Dock.UIPanel();
            this.uiPanelSchduleConfirmContainer = new Janus.Windows.UI.Dock.UIPanelInnerContainer();
            this.panel1 = new System.Windows.Forms.Panel();
            this.grdExPublishList = new Janus.Windows.GridEX.GridEX();
            this.dvFilePublish = new System.Data.DataView();
            this.filePublishDs = new AdManagerClient.FilePublishDs();
            this.uiPanelPublishConfirm = new Janus.Windows.UI.Dock.UIPanel();
            this.uiPanelPublishConfirmContainer = new Janus.Windows.UI.Dock.UIPanelInnerContainer();
            this.panel3 = new System.Windows.Forms.Panel();
            this.label4 = new System.Windows.Forms.Label();
            this.ebAckStateName = new Janus.Windows.GridEX.EditControls.EditBox();
            this.label5 = new System.Windows.Forms.Label();
            this.ebModifyStartDay = new Janus.Windows.GridEX.EditControls.EditBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.ebPublishDesc = new Janus.Windows.GridEX.EditControls.EditBox();
            this.btnPublishConfirm = new Janus.Windows.EditControls.UIButton();
            this.lbMemo2 = new System.Windows.Forms.Label();
            this.ebPublishDay = new Janus.Windows.GridEX.EditControls.EditBox();
            this.ebPublishUserName = new Janus.Windows.GridEX.EditControls.EditBox();
            this.uiPanelPublish = new Janus.Windows.UI.Dock.UIPanelGroup();
            this.uiPanelHistory = new Janus.Windows.UI.Dock.UIPanel();
            this.uiPanelHistoryContainer = new Janus.Windows.UI.Dock.UIPanelInnerContainer();
            this.grdExHistory = new Janus.Windows.GridEX.GridEX();
            this.dvHistory = new System.Data.DataView();
            this.uiPanelPubDetail = new Janus.Windows.UI.Dock.UIPanel();
            this.uiPanelPubDetailContainer = new Janus.Windows.UI.Dock.UIPanelInnerContainer();
            this.grdExFileList = new Janus.Windows.GridEX.GridEX();
            this.dvFileList = new System.Data.DataView();
            this.dvWork = new System.Data.DataView();
            this.uiPanelDetail = new Janus.Windows.UI.Dock.UIPanel();
            this.uiPanelDetailContainer = new Janus.Windows.UI.Dock.UIPanelInnerContainer();
            this.uiPanelMaster = new Janus.Windows.UI.Dock.UIPanel();
            this.uiPanelMasterContainer = new Janus.Windows.UI.Dock.UIPanelInnerContainer();
            this.panal4 = new System.Windows.Forms.Panel();
            this.panel5 = new System.Windows.Forms.Panel();
            this.panel6 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.grdExScheduleList = new Janus.Windows.GridEX.GridEX();
            this.uiPanel1 = new Janus.Windows.UI.Dock.UIPanel();
            this.uiPanel1Container = new Janus.Windows.UI.Dock.UIPanelInnerContainer();
            this.uiPanel2 = new Janus.Windows.UI.Dock.UIPanel();
            this.uiPanel2Container = new Janus.Windows.UI.Dock.UIPanelInnerContainer();
            ((System.ComponentModel.ISupportInitialize)(this.uiPM)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelAdFile)).BeginInit();
            this.uiPanelAdFile.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelSearch)).BeginInit();
            this.uiPanelSearch.SuspendLayout();
            this.uiPanelSearchContainer.SuspendLayout();
            this.pnlSearch.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelConfirm)).BeginInit();
            this.uiPanelConfirm.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelSchduleConfirm)).BeginInit();
            this.uiPanelSchduleConfirm.SuspendLayout();
            this.uiPanelSchduleConfirmContainer.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdExPublishList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dvFilePublish)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.filePublishDs)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelPublishConfirm)).BeginInit();
            this.uiPanelPublishConfirm.SuspendLayout();
            this.uiPanelPublishConfirmContainer.SuspendLayout();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelPublish)).BeginInit();
            this.uiPanelPublish.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelHistory)).BeginInit();
            this.uiPanelHistory.SuspendLayout();
            this.uiPanelHistoryContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdExHistory)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dvHistory)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelPubDetail)).BeginInit();
            this.uiPanelPubDetail.SuspendLayout();
            this.uiPanelPubDetailContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdExFileList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dvFileList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dvWork)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelDetail)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelMaster)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdExScheduleList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanel1)).BeginInit();
            this.uiPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanel2)).BeginInit();
            this.uiPanel2.SuspendLayout();
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
            this.uiPanelConfirm.Id = new System.Guid("14c5930c-031d-4da3-9b76-a4aafd861b22");
            this.uiPanelConfirm.StaticGroup = true;
            this.uiPanelSchduleConfirm.Id = new System.Guid("e1f833ff-3c5b-408f-9422-30aa36d1814e");
            this.uiPanelConfirm.Panels.Add(this.uiPanelSchduleConfirm);
            this.uiPanelPublishConfirm.Id = new System.Guid("b2278df3-8e2a-4ffe-98df-e6bce62014d0");
            this.uiPanelConfirm.Panels.Add(this.uiPanelPublishConfirm);
            this.uiPanelAdFile.Panels.Add(this.uiPanelConfirm);
            this.uiPanelPublish.Id = new System.Guid("d4ad4cb3-b67d-4cd3-a415-92c65bf4f071");
            this.uiPanelPublish.StaticGroup = true;
            this.uiPanelHistory.Id = new System.Guid("d2b56dee-554c-4717-a64d-d6470d130f38");
            this.uiPanelPublish.Panels.Add(this.uiPanelHistory);
            this.uiPanelPubDetail.Id = new System.Guid("e361e85d-1fc5-4ede-8c6f-91c51df4db6f");
            this.uiPanelPublish.Panels.Add(this.uiPanelPubDetail);
            this.uiPanelAdFile.Panels.Add(this.uiPanelPublish);
            this.uiPM.Panels.Add(this.uiPanelAdFile);
            // 
            // Design Time Panel Info:
            // 
            this.uiPM.BeginPanelInfo();
            this.uiPM.AddDockPanelInfo(new System.Guid("b162c53a-7940-47cd-a0ed-9762c59c50d8"), Janus.Windows.UI.Dock.PanelGroupStyle.HorizontalTiles, Janus.Windows.UI.Dock.PanelDockStyle.Fill, true, new System.Drawing.Size(1010, 677), true);
            this.uiPM.AddDockPanelInfo(new System.Guid("afb3b7d0-aa79-443c-a1e8-3baba38f7a4d"), new System.Guid("b162c53a-7940-47cd-a0ed-9762c59c50d8"), 40, true);
            this.uiPM.AddDockPanelInfo(new System.Guid("14c5930c-031d-4da3-9b76-a4aafd861b22"), new System.Guid("b162c53a-7940-47cd-a0ed-9762c59c50d8"), Janus.Windows.UI.Dock.PanelGroupStyle.VerticalTiles, true, 235, true);
            this.uiPM.AddDockPanelInfo(new System.Guid("e1f833ff-3c5b-408f-9422-30aa36d1814e"), new System.Guid("14c5930c-031d-4da3-9b76-a4aafd861b22"), 438, true);
            this.uiPM.AddDockPanelInfo(new System.Guid("b2278df3-8e2a-4ffe-98df-e6bce62014d0"), new System.Guid("14c5930c-031d-4da3-9b76-a4aafd861b22"), 409, true);
            this.uiPM.AddDockPanelInfo(new System.Guid("d4ad4cb3-b67d-4cd3-a415-92c65bf4f071"), new System.Guid("b162c53a-7940-47cd-a0ed-9762c59c50d8"), Janus.Windows.UI.Dock.PanelGroupStyle.VerticalTiles, true, 352, true);
            this.uiPM.AddDockPanelInfo(new System.Guid("d2b56dee-554c-4717-a64d-d6470d130f38"), new System.Guid("d4ad4cb3-b67d-4cd3-a415-92c65bf4f071"), 438, true);
            this.uiPM.AddDockPanelInfo(new System.Guid("e361e85d-1fc5-4ede-8c6f-91c51df4db6f"), new System.Guid("d4ad4cb3-b67d-4cd3-a415-92c65bf4f071"), 409, true);
            this.uiPM.AddFloatingPanelInfo(new System.Guid("b162c53a-7940-47cd-a0ed-9762c59c50d8"), Janus.Windows.UI.Dock.PanelGroupStyle.HorizontalTiles, true, new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
            this.uiPM.AddFloatingPanelInfo(new System.Guid("8e3140b9-bfcb-43af-b84e-2ee6cddf51dc"), new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
            this.uiPM.AddFloatingPanelInfo(new System.Guid("b2973a57-d3e1-4d80-9387-7a6dc3332542"), new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
            this.uiPM.AddFloatingPanelInfo(new System.Guid("afb3b7d0-aa79-443c-a1e8-3baba38f7a4d"), new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
            this.uiPM.AddFloatingPanelInfo(new System.Guid("14c5930c-031d-4da3-9b76-a4aafd861b22"), Janus.Windows.UI.Dock.PanelGroupStyle.VerticalTiles, true, new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
            this.uiPM.AddFloatingPanelInfo(new System.Guid("e1f833ff-3c5b-408f-9422-30aa36d1814e"), new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
            this.uiPM.AddFloatingPanelInfo(new System.Guid("639e85a8-1091-4e4f-9c71-cba7af3330f5"), new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
            this.uiPM.AddFloatingPanelInfo(new System.Guid("aa97407d-9777-4dac-bc00-3751ab8a3aba"), Janus.Windows.UI.Dock.PanelGroupStyle.HorizontalTiles, true, new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
            this.uiPM.AddFloatingPanelInfo(new System.Guid("3656b1fa-facf-4a8e-9665-cff9e8da0f57"), new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
            this.uiPM.AddFloatingPanelInfo(new System.Guid("97fdb5cb-6280-408b-9821-4851e4735c6b"), new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
            this.uiPM.AddFloatingPanelInfo(new System.Guid("f2f8e0a0-5a41-487e-b61a-c55fd0a9d359"), new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
            this.uiPM.AddFloatingPanelInfo(new System.Guid("a0d9865d-9993-4140-abc8-aef64ac0146a"), new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
            this.uiPM.AddFloatingPanelInfo(new System.Guid("b2278df3-8e2a-4ffe-98df-e6bce62014d0"), new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
            this.uiPM.AddFloatingPanelInfo(new System.Guid("e3017d1d-ec9a-4a06-afa9-7bf7c7b4758a"), new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
            this.uiPM.AddFloatingPanelInfo(new System.Guid("abe7187b-8366-4611-a2c6-1c53c100233c"), Janus.Windows.UI.Dock.PanelGroupStyle.VerticalTiles, true, new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
            this.uiPM.AddFloatingPanelInfo(new System.Guid("c04105fe-8c76-43cd-9439-8c1925374fd7"), new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
            this.uiPM.AddFloatingPanelInfo(new System.Guid("d4ad4cb3-b67d-4cd3-a415-92c65bf4f071"), Janus.Windows.UI.Dock.PanelGroupStyle.VerticalTiles, true, new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
            this.uiPM.AddFloatingPanelInfo(new System.Guid("d2b56dee-554c-4717-a64d-d6470d130f38"), new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
            this.uiPM.AddFloatingPanelInfo(new System.Guid("e361e85d-1fc5-4ede-8c6f-91c51df4db6f"), new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
            this.uiPM.EndPanelInfo();
            // 
            // uiPanelAdFile
            // 
            this.uiPanelAdFile.ActiveCaptionMode = Janus.Windows.UI.Dock.ActiveCaptionMode.Never;
            this.uiPanelAdFile.BackColor = System.Drawing.SystemColors.Window;
            this.uiPanelAdFile.CloseButtonVisible = Janus.Windows.UI.InheritableBoolean.False;
            this.uiPanelAdFile.Location = new System.Drawing.Point(0, 0);
            this.uiPanelAdFile.Name = "uiPanelAdFile";
            this.uiPanelAdFile.Size = new System.Drawing.Size(1010, 677);
            this.uiPanelAdFile.TabIndex = 4;
            this.uiPanelAdFile.Text = "ÆÄÀÏ¹èÆ÷½ÂÀÎ";
            // 
            // uiPanelSearch
            // 
            this.uiPanelSearch.CaptionVisible = Janus.Windows.UI.InheritableBoolean.False;
            this.uiPanelSearch.CloseButtonVisible = Janus.Windows.UI.InheritableBoolean.False;
            this.uiPanelSearch.InnerContainer = this.uiPanelSearchContainer;
            this.uiPanelSearch.Location = new System.Drawing.Point(0, 22);
            this.uiPanelSearch.Name = "uiPanelSearch";
            this.uiPanelSearch.Size = new System.Drawing.Size(1010, 42);
            this.uiPanelSearch.TabIndex = 4;
            this.uiPanelSearch.Text = "°Ë»ö";
            // 
            // uiPanelSearchContainer
            // 
            this.uiPanelSearchContainer.Controls.Add(this.pnlSearch);
            this.uiPanelSearchContainer.Location = new System.Drawing.Point(1, 1);
            this.uiPanelSearchContainer.Name = "uiPanelSearchContainer";
            this.uiPanelSearchContainer.Size = new System.Drawing.Size(1008, 40);
            this.uiPanelSearchContainer.TabIndex = 0;
            // 
            // pnlSearch
            // 
            this.pnlSearch.BackColor = System.Drawing.SystemColors.Window;
            this.pnlSearch.Controls.Add(this.btnSearch);
            this.pnlSearch.Controls.Add(this.cbSearchMedia);
            this.pnlSearch.Controls.Add(this.lbMsg);
            this.pnlSearch.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlSearch.Location = new System.Drawing.Point(0, 0);
            this.pnlSearch.Name = "pnlSearch";
            this.pnlSearch.Size = new System.Drawing.Size(1008, 40);
            this.pnlSearch.TabIndex = 0;
            // 
            // btnSearch
            // 
            this.btnSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSearch.Enabled = false;
            this.btnSearch.Location = new System.Drawing.Point(895, 8);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(104, 24);
            this.btnSearch.TabIndex = 2;
            this.btnSearch.Text = "Á¶ È¸";
            this.btnSearch.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // cbSearchMedia
            // 
            this.cbSearchMedia.BackColor = System.Drawing.Color.White;
            this.cbSearchMedia.ComboStyle = Janus.Windows.EditControls.ComboStyle.DropDownList;
            this.cbSearchMedia.Location = new System.Drawing.Point(8, 8);
            this.cbSearchMedia.Name = "cbSearchMedia";
            this.cbSearchMedia.Size = new System.Drawing.Size(160, 21);
            this.cbSearchMedia.TabIndex = 1;
            this.cbSearchMedia.Text = "¸ÅÃ¼¼±ÅÃ";
            this.cbSearchMedia.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            // 
            // lbMsg
            // 
            this.lbMsg.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.lbMsg.Location = new System.Drawing.Point(176, 8);
            this.lbMsg.Name = "lbMsg";
            this.lbMsg.Size = new System.Drawing.Size(376, 24);
            this.lbMsg.TabIndex = 0;
            this.lbMsg.Text = "ÆÄÀÏ¹èÆ÷½ÂÀÎÀ» Ã³¸®ÇÕ´Ï´Ù.";
            this.lbMsg.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // uiPanelConfirm
            // 
            this.uiPanelConfirm.AllowResize = Janus.Windows.UI.InheritableBoolean.False;
            this.uiPanelConfirm.CaptionVisible = Janus.Windows.UI.InheritableBoolean.False;
            this.uiPanelConfirm.GroupStyle = Janus.Windows.UI.Dock.PanelGroupStyle.VerticalTiles;
            this.uiPanelConfirm.Location = new System.Drawing.Point(0, 68);
            this.uiPanelConfirm.Name = "uiPanelConfirm";
            this.uiPanelConfirm.Size = new System.Drawing.Size(1010, 242);
            this.uiPanelConfirm.TabIndex = 4;
            this.uiPanelConfirm.Text = "ÆÄÀÏ¹èÆ÷½ÂÀÎ";
            // 
            // uiPanelSchduleConfirm
            // 
            this.uiPanelSchduleConfirm.InnerContainer = this.uiPanelSchduleConfirmContainer;
            this.uiPanelSchduleConfirm.Location = new System.Drawing.Point(0, 0);
            this.uiPanelSchduleConfirm.Name = "uiPanelSchduleConfirm";
            this.uiPanelSchduleConfirm.Size = new System.Drawing.Size(521, 242);
            this.uiPanelSchduleConfirm.TabIndex = 4;
            this.uiPanelSchduleConfirm.Text = "½ÂÀÎÀÌ·Â";
            // 
            // uiPanelSchduleConfirmContainer
            // 
            this.uiPanelSchduleConfirmContainer.Controls.Add(this.panel1);
            this.uiPanelSchduleConfirmContainer.Location = new System.Drawing.Point(1, 23);
            this.uiPanelSchduleConfirmContainer.Name = "uiPanelSchduleConfirmContainer";
            this.uiPanelSchduleConfirmContainer.Size = new System.Drawing.Size(519, 218);
            this.uiPanelSchduleConfirmContainer.TabIndex = 0;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.Window;
            this.panel1.Controls.Add(this.grdExPublishList);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(519, 218);
            this.panel1.TabIndex = 4;
            // 
            // grdExPublishList
            // 
            this.grdExPublishList.AllowEdit = Janus.Windows.GridEX.InheritableBoolean.False;
            this.grdExPublishList.AlternatingColors = true;
            this.grdExPublishList.BorderStyle = Janus.Windows.GridEX.BorderStyle.None;
            this.grdExPublishList.DataSource = this.dvFilePublish;
            grdExPublishList_DesignTimeLayout.LayoutString = resources.GetString("grdExPublishList_DesignTimeLayout.LayoutString");
            this.grdExPublishList.DesignTimeLayout = grdExPublishList_DesignTimeLayout;
            this.grdExPublishList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdExPublishList.EditorsControlStyle.ButtonAppearance = Janus.Windows.GridEX.ButtonAppearance.Regular;
            this.grdExPublishList.EmptyRows = true;
            this.grdExPublishList.FocusCellFormatStyle.BackColor = System.Drawing.SystemColors.Highlight;
            this.grdExPublishList.FocusCellFormatStyle.ForeColor = System.Drawing.Color.White;
            this.grdExPublishList.FocusStyle = Janus.Windows.GridEX.FocusStyle.None;
            this.grdExPublishList.GridLineColor = System.Drawing.Color.Silver;
            this.grdExPublishList.GridLines = Janus.Windows.GridEX.GridLines.Vertical;
            this.grdExPublishList.GridLineStyle = Janus.Windows.GridEX.GridLineStyle.Solid;
            this.grdExPublishList.GroupByBoxVisible = false;
            this.grdExPublishList.HeaderFormatStyle.TextAlignment = Janus.Windows.GridEX.TextAlignment.Center;
            this.grdExPublishList.HideSelection = Janus.Windows.GridEX.HideSelection.HighlightInactive;
            this.grdExPublishList.Location = new System.Drawing.Point(0, 0);
            this.grdExPublishList.Name = "grdExPublishList";
            this.grdExPublishList.ScrollBars = Janus.Windows.GridEX.ScrollBars.Vertical;
            this.grdExPublishList.SelectedInactiveFormatStyle.BackColor = System.Drawing.Color.Gold;
            this.grdExPublishList.Size = new System.Drawing.Size(519, 218);
            this.grdExPublishList.TabIndex = 3;
            this.grdExPublishList.TabKeyBehavior = Janus.Windows.GridEX.TabKeyBehavior.ControlNavigation;
            this.grdExPublishList.ThemedAreas = ((Janus.Windows.GridEX.ThemedArea)(((((((((Janus.Windows.GridEX.ThemedArea.ScrollBars | Janus.Windows.GridEX.ThemedArea.EditControls)
                        | Janus.Windows.GridEX.ThemedArea.Headers)
                        | Janus.Windows.GridEX.ThemedArea.GroupByBox)
                        | Janus.Windows.GridEX.ThemedArea.GroupRows)
                        | Janus.Windows.GridEX.ThemedArea.ControlBorder)
                        | Janus.Windows.GridEX.ThemedArea.Cards)
                        | Janus.Windows.GridEX.ThemedArea.Gridlines)
                        | Janus.Windows.GridEX.ThemedArea.CheckBoxes)));
            this.grdExPublishList.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            this.grdExPublishList.Enter += new System.EventHandler(this.OnGrdRowChanged);
            // 
            // dvFilePublish
            // 
            this.dvFilePublish.Table = this.filePublishDs.Publish;
            // 
            // filePublishDs
            // 
            this.filePublishDs.DataSetName = "FilePublishDs";
            this.filePublishDs.Locale = new System.Globalization.CultureInfo("en-US");
            this.filePublishDs.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // uiPanelPublishConfirm
            // 
            this.uiPanelPublishConfirm.InnerContainer = this.uiPanelPublishConfirmContainer;
            this.uiPanelPublishConfirm.Location = new System.Drawing.Point(525, 0);
            this.uiPanelPublishConfirm.Name = "uiPanelPublishConfirm";
            this.uiPanelPublishConfirm.Size = new System.Drawing.Size(485, 242);
            this.uiPanelPublishConfirm.TabIndex = 4;
            this.uiPanelPublishConfirm.Text = "½ÂÀÎÃ³¸®";
            // 
            // uiPanelPublishConfirmContainer
            // 
            this.uiPanelPublishConfirmContainer.Controls.Add(this.panel3);
            this.uiPanelPublishConfirmContainer.Location = new System.Drawing.Point(1, 23);
            this.uiPanelPublishConfirmContainer.Name = "uiPanelPublishConfirmContainer";
            this.uiPanelPublishConfirmContainer.Size = new System.Drawing.Size(483, 218);
            this.uiPanelPublishConfirmContainer.TabIndex = 0;
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.SystemColors.Window;
            this.panel3.Controls.Add(this.label4);
            this.panel3.Controls.Add(this.ebAckStateName);
            this.panel3.Controls.Add(this.label5);
            this.panel3.Controls.Add(this.ebModifyStartDay);
            this.panel3.Controls.Add(this.label2);
            this.panel3.Controls.Add(this.label3);
            this.panel3.Controls.Add(this.ebPublishDesc);
            this.panel3.Controls.Add(this.btnPublishConfirm);
            this.panel3.Controls.Add(this.lbMemo2);
            this.panel3.Controls.Add(this.ebPublishDay);
            this.panel3.Controls.Add(this.ebPublishUserName);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(0, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(483, 218);
            this.panel3.TabIndex = 4;
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(184, 8);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(80, 21);
            this.label4.TabIndex = 21;
            this.label4.Text = "º¯°æ½ÃÀÛÀÏ½Ã";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ebAckStateName
            // 
            this.ebAckStateName.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ebAckStateName.Location = new System.Drawing.Point(88, 8);
            this.ebAckStateName.Name = "ebAckStateName";
            this.ebAckStateName.ReadOnly = true;
            this.ebAckStateName.Size = new System.Drawing.Size(88, 21);
            this.ebAckStateName.TabIndex = 22;
            this.ebAckStateName.TabStop = false;
            this.ebAckStateName.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
            this.ebAckStateName.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(8, 8);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(72, 21);
            this.label5.TabIndex = 20;
            this.label5.Text = "½ÂÀÎ»óÅÂ";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ebModifyStartDay
            // 
            this.ebModifyStartDay.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ebModifyStartDay.Location = new System.Drawing.Point(264, 8);
            this.ebModifyStartDay.MaxLength = 15;
            this.ebModifyStartDay.Name = "ebModifyStartDay";
            this.ebModifyStartDay.ReadOnly = true;
            this.ebModifyStartDay.Size = new System.Drawing.Size(120, 21);
            this.ebModifyStartDay.TabIndex = 23;
            this.ebModifyStartDay.TabStop = false;
            this.ebModifyStartDay.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
            this.ebModifyStartDay.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(8, 42);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(72, 21);
            this.label2.TabIndex = 26;
            this.label2.Text = "¹èÆ÷½ÂÀÎÀÚ";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(184, 42);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(80, 21);
            this.label3.TabIndex = 27;
            this.label3.Text = "¹èÆ÷½ÂÀÎÀÏ½Ã";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ebPublishDesc
            // 
            this.ebPublishDesc.Location = new System.Drawing.Point(88, 76);
            this.ebPublishDesc.MaxLength = 100;
            this.ebPublishDesc.Multiline = true;
            this.ebPublishDesc.Name = "ebPublishDesc";
            this.ebPublishDesc.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.ebPublishDesc.Size = new System.Drawing.Size(296, 115);
            this.ebPublishDesc.TabIndex = 5;
            this.ebPublishDesc.TabStop = false;
            this.ebPublishDesc.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
            this.ebPublishDesc.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            // 
            // btnPublishConfirm
            // 
            this.btnPublishConfirm.Enabled = false;
            this.btnPublishConfirm.Location = new System.Drawing.Point(9, 80);
            this.btnPublishConfirm.Name = "btnPublishConfirm";
            this.btnPublishConfirm.Size = new System.Drawing.Size(72, 24);
            this.btnPublishConfirm.TabIndex = 6;
            this.btnPublishConfirm.Text = "¹èÆ÷½ÂÀÎ";
            this.btnPublishConfirm.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.btnPublishConfirm.Click += new System.EventHandler(this.btnPublishConfirm_Click);
            // 
            // lbMemo2
            // 
            this.lbMemo2.Location = new System.Drawing.Point(8, 76);
            this.lbMemo2.Name = "lbMemo2";
            this.lbMemo2.Size = new System.Drawing.Size(80, 21);
            this.lbMemo2.TabIndex = 25;
            this.lbMemo2.Text = "¹èÆ÷½ÂÀÎ¸Þ¸ð";
            this.lbMemo2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ebPublishDay
            // 
            this.ebPublishDay.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ebPublishDay.Location = new System.Drawing.Point(264, 42);
            this.ebPublishDay.MaxLength = 15;
            this.ebPublishDay.Name = "ebPublishDay";
            this.ebPublishDay.ReadOnly = true;
            this.ebPublishDay.Size = new System.Drawing.Size(120, 21);
            this.ebPublishDay.TabIndex = 28;
            this.ebPublishDay.TabStop = false;
            this.ebPublishDay.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
            this.ebPublishDay.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            // 
            // ebPublishUserName
            // 
            this.ebPublishUserName.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ebPublishUserName.Location = new System.Drawing.Point(88, 42);
            this.ebPublishUserName.Name = "ebPublishUserName";
            this.ebPublishUserName.ReadOnly = true;
            this.ebPublishUserName.Size = new System.Drawing.Size(88, 21);
            this.ebPublishUserName.TabIndex = 29;
            this.ebPublishUserName.TabStop = false;
            this.ebPublishUserName.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
            this.ebPublishUserName.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            // 
            // uiPanelPublish
            // 
            this.uiPanelPublish.CaptionVisible = Janus.Windows.UI.InheritableBoolean.False;
            this.uiPanelPublish.GroupStyle = Janus.Windows.UI.Dock.PanelGroupStyle.VerticalTiles;
            this.uiPanelPublish.Location = new System.Drawing.Point(0, 314);
            this.uiPanelPublish.Name = "uiPanelPublish";
            this.uiPanelPublish.Size = new System.Drawing.Size(1010, 363);
            this.uiPanelPublish.TabIndex = 4;
            // 
            // uiPanelHistory
            // 
            this.uiPanelHistory.InnerContainer = this.uiPanelHistoryContainer;
            this.uiPanelHistory.Location = new System.Drawing.Point(0, 0);
            this.uiPanelHistory.Name = "uiPanelHistory";
            this.uiPanelHistory.Size = new System.Drawing.Size(521, 363);
            this.uiPanelHistory.TabIndex = 4;
            this.uiPanelHistory.Text = "º¯°æ³»¿ª";
            // 
            // uiPanelHistoryContainer
            // 
            this.uiPanelHistoryContainer.Controls.Add(this.grdExHistory);
            this.uiPanelHistoryContainer.Location = new System.Drawing.Point(1, 23);
            this.uiPanelHistoryContainer.Name = "uiPanelHistoryContainer";
            this.uiPanelHistoryContainer.Size = new System.Drawing.Size(519, 339);
            this.uiPanelHistoryContainer.TabIndex = 0;
            // 
            // grdExHistory
            // 
            this.grdExHistory.AllowEdit = Janus.Windows.GridEX.InheritableBoolean.False;
            this.grdExHistory.AlternatingColors = true;
            this.grdExHistory.BorderStyle = Janus.Windows.GridEX.BorderStyle.None;
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
            this.grdExHistory.Location = new System.Drawing.Point(0, 0);
            this.grdExHistory.Name = "grdExHistory";
            this.grdExHistory.ScrollBars = Janus.Windows.GridEX.ScrollBars.Vertical;
            this.grdExHistory.SelectedInactiveFormatStyle.BackColor = System.Drawing.Color.Gold;
            this.grdExHistory.Size = new System.Drawing.Size(519, 339);
            this.grdExHistory.TabIndex = 7;
            this.grdExHistory.TabKeyBehavior = Janus.Windows.GridEX.TabKeyBehavior.ControlNavigation;
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
            this.dvHistory.Table = this.filePublishDs.History;
            // 
            // uiPanelPubDetail
            // 
            this.uiPanelPubDetail.InnerContainer = this.uiPanelPubDetailContainer;
            this.uiPanelPubDetail.Location = new System.Drawing.Point(525, 0);
            this.uiPanelPubDetail.Name = "uiPanelPubDetail";
            this.uiPanelPubDetail.Size = new System.Drawing.Size(485, 363);
            this.uiPanelPubDetail.TabIndex = 4;
            this.uiPanelPubDetail.Text = "ÆÄÀÏ¹èÆ÷¸ñ·Ï(ÁøÇÏ°Ô:¹èÆ÷,º¸Åë:Á÷Àü,Èå¸®°Ô:±âÁ¸)";
            // 
            // uiPanelPubDetailContainer
            // 
            this.uiPanelPubDetailContainer.Controls.Add(this.grdExFileList);
            this.uiPanelPubDetailContainer.Location = new System.Drawing.Point(1, 23);
            this.uiPanelPubDetailContainer.Name = "uiPanelPubDetailContainer";
            this.uiPanelPubDetailContainer.Size = new System.Drawing.Size(483, 339);
            this.uiPanelPubDetailContainer.TabIndex = 0;
            // 
            // grdExFileList
            // 
            this.grdExFileList.AllowEdit = Janus.Windows.GridEX.InheritableBoolean.False;
            this.grdExFileList.AlternatingColors = true;
            this.grdExFileList.BorderStyle = Janus.Windows.GridEX.BorderStyle.None;
            this.grdExFileList.DataSource = this.dvFileList;
            grdExFileList_DesignTimeLayout.LayoutString = resources.GetString("grdExFileList_DesignTimeLayout.LayoutString");
            this.grdExFileList.DesignTimeLayout = grdExFileList_DesignTimeLayout;
            this.grdExFileList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdExFileList.EditorsControlStyle.ButtonAppearance = Janus.Windows.GridEX.ButtonAppearance.Regular;
            this.grdExFileList.EmptyRows = true;
            this.grdExFileList.FocusCellFormatStyle.BackColor = System.Drawing.SystemColors.Highlight;
            this.grdExFileList.FocusCellFormatStyle.ForeColor = System.Drawing.Color.White;
            this.grdExFileList.FocusStyle = Janus.Windows.GridEX.FocusStyle.None;
            this.grdExFileList.FrozenColumns = 2;
            this.grdExFileList.GridLineColor = System.Drawing.Color.Silver;
            this.grdExFileList.GridLines = Janus.Windows.GridEX.GridLines.Vertical;
            this.grdExFileList.GridLineStyle = Janus.Windows.GridEX.GridLineStyle.Solid;
            this.grdExFileList.GroupByBoxVisible = false;
            this.grdExFileList.HeaderFormatStyle.TextAlignment = Janus.Windows.GridEX.TextAlignment.Center;
            this.grdExFileList.HideSelection = Janus.Windows.GridEX.HideSelection.HighlightInactive;
            this.grdExFileList.Location = new System.Drawing.Point(0, 0);
            this.grdExFileList.Name = "grdExFileList";
            this.grdExFileList.ScrollBars = Janus.Windows.GridEX.ScrollBars.Vertical;
            this.grdExFileList.SelectedInactiveFormatStyle.BackColor = System.Drawing.Color.Gold;
            this.grdExFileList.Size = new System.Drawing.Size(483, 339);
            this.grdExFileList.TabIndex = 8;
            this.grdExFileList.TabKeyBehavior = Janus.Windows.GridEX.TabKeyBehavior.ControlNavigation;
            this.grdExFileList.ThemedAreas = ((Janus.Windows.GridEX.ThemedArea)(((((((((Janus.Windows.GridEX.ThemedArea.ScrollBars | Janus.Windows.GridEX.ThemedArea.EditControls)
                        | Janus.Windows.GridEX.ThemedArea.Headers)
                        | Janus.Windows.GridEX.ThemedArea.GroupByBox)
                        | Janus.Windows.GridEX.ThemedArea.GroupRows)
                        | Janus.Windows.GridEX.ThemedArea.ControlBorder)
                        | Janus.Windows.GridEX.ThemedArea.Cards)
                        | Janus.Windows.GridEX.ThemedArea.Gridlines)
                        | Janus.Windows.GridEX.ThemedArea.CheckBoxes)));
            this.grdExFileList.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            // 
            // dvFileList
            // 
            this.dvFileList.Table = this.filePublishDs.FileList;
            // 
            // dvWork
            // 
            this.dvWork.Table = this.filePublishDs.FIleReserve;
            // 
            // uiPanelDetail
            // 
            this.uiPanelDetail.Location = new System.Drawing.Point(0, 0);
            this.uiPanelDetail.Name = "uiPanelDetail";
            this.uiPanelDetail.Size = new System.Drawing.Size(200, 200);
            this.uiPanelDetail.TabIndex = 0;
            // 
            // uiPanelDetailContainer
            // 
            this.uiPanelDetailContainer.Location = new System.Drawing.Point(0, 0);
            this.uiPanelDetailContainer.Name = "uiPanelDetailContainer";
            this.uiPanelDetailContainer.Size = new System.Drawing.Size(0, 0);
            this.uiPanelDetailContainer.TabIndex = 0;
            // 
            // uiPanelMaster
            // 
            this.uiPanelMaster.Location = new System.Drawing.Point(0, 0);
            this.uiPanelMaster.Name = "uiPanelMaster";
            this.uiPanelMaster.Size = new System.Drawing.Size(200, 200);
            this.uiPanelMaster.TabIndex = 0;
            // 
            // uiPanelMasterContainer
            // 
            this.uiPanelMasterContainer.Location = new System.Drawing.Point(0, 0);
            this.uiPanelMasterContainer.Name = "uiPanelMasterContainer";
            this.uiPanelMasterContainer.Size = new System.Drawing.Size(0, 0);
            this.uiPanelMasterContainer.TabIndex = 0;
            // 
            // panal4
            // 
            this.panal4.BackColor = System.Drawing.SystemColors.Window;
            this.panal4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panal4.Location = new System.Drawing.Point(0, 0);
            this.panal4.Name = "panal4";
            this.panal4.Size = new System.Drawing.Size(395, 132);
            this.panal4.TabIndex = 5;
            // 
            // panel5
            // 
            this.panel5.BackColor = System.Drawing.SystemColors.Window;
            this.panel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel5.Location = new System.Drawing.Point(0, 0);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(395, 147);
            this.panel5.TabIndex = 5;
            // 
            // panel6
            // 
            this.panel6.BackColor = System.Drawing.SystemColors.Window;
            this.panel6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel6.Location = new System.Drawing.Point(0, 0);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(395, 191);
            this.panel6.TabIndex = 5;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.SystemColors.Window;
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(158, 569);
            this.panel2.TabIndex = 5;
            // 
            // grdExScheduleList
            // 
            this.grdExScheduleList.AllowEdit = Janus.Windows.GridEX.InheritableBoolean.False;
            this.grdExScheduleList.AlternatingColors = true;
            this.grdExScheduleList.BorderStyle = Janus.Windows.GridEX.BorderStyle.None;
            grdExScheduleList_DesignTimeLayout.LayoutString = resources.GetString("grdExScheduleList_DesignTimeLayout.LayoutString");
            this.grdExScheduleList.DesignTimeLayout = grdExScheduleList_DesignTimeLayout;
            this.grdExScheduleList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdExScheduleList.EditorsControlStyle.ButtonAppearance = Janus.Windows.GridEX.ButtonAppearance.Regular;
            this.grdExScheduleList.EmptyRows = true;
            this.grdExScheduleList.FocusCellFormatStyle.BackColor = System.Drawing.SystemColors.Highlight;
            this.grdExScheduleList.FocusCellFormatStyle.ForeColor = System.Drawing.Color.White;
            this.grdExScheduleList.FocusStyle = Janus.Windows.GridEX.FocusStyle.None;
            this.grdExScheduleList.Font = new System.Drawing.Font("±¼¸²Ã¼", 9F);
            this.grdExScheduleList.GridLineColor = System.Drawing.Color.Silver;
            this.grdExScheduleList.GridLines = Janus.Windows.GridEX.GridLines.Vertical;
            this.grdExScheduleList.GridLineStyle = Janus.Windows.GridEX.GridLineStyle.Solid;
            this.grdExScheduleList.GroupByBoxVisible = false;
            this.grdExScheduleList.HeaderFormatStyle.TextAlignment = Janus.Windows.GridEX.TextAlignment.Center;
            this.grdExScheduleList.Location = new System.Drawing.Point(0, 0);
            this.grdExScheduleList.Name = "grdExScheduleList";
            this.grdExScheduleList.SelectedInactiveFormatStyle.BackColor = System.Drawing.Color.DarkOrange;
            this.grdExScheduleList.Size = new System.Drawing.Size(851, 183);
            this.grdExScheduleList.TabIndex = 17;
            this.grdExScheduleList.TabKeyBehavior = Janus.Windows.GridEX.TabKeyBehavior.ControlNavigation;
            this.grdExScheduleList.ThemedAreas = ((Janus.Windows.GridEX.ThemedArea)(((((((((Janus.Windows.GridEX.ThemedArea.ScrollBars | Janus.Windows.GridEX.ThemedArea.EditControls)
                        | Janus.Windows.GridEX.ThemedArea.Headers)
                        | Janus.Windows.GridEX.ThemedArea.GroupByBox)
                        | Janus.Windows.GridEX.ThemedArea.GroupRows)
                        | Janus.Windows.GridEX.ThemedArea.ControlBorder)
                        | Janus.Windows.GridEX.ThemedArea.Cards)
                        | Janus.Windows.GridEX.ThemedArea.Gridlines)
                        | Janus.Windows.GridEX.ThemedArea.CheckBoxes)));
            this.grdExScheduleList.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2003;
            // 
            // uiPanel1
            // 
            this.uiPanel1.InnerContainer = this.uiPanel1Container;
            this.uiPanel1.Location = new System.Drawing.Point(0, 22);
            this.uiPanel1.Name = "uiPanel1";
            this.uiPanel1.Size = new System.Drawing.Size(851, 88);
            this.uiPanel1.TabIndex = 4;
            this.uiPanel1.Text = "Panel 1";
            // 
            // uiPanel1Container
            // 
            this.uiPanel1Container.Location = new System.Drawing.Point(0, 0);
            this.uiPanel1Container.Name = "uiPanel1Container";
            this.uiPanel1Container.Size = new System.Drawing.Size(851, 88);
            this.uiPanel1Container.TabIndex = 0;
            // 
            // uiPanel2
            // 
            this.uiPanel2.InnerContainer = this.uiPanel2Container;
            this.uiPanel2.Location = new System.Drawing.Point(0, 114);
            this.uiPanel2.Name = "uiPanel2";
            this.uiPanel2.Size = new System.Drawing.Size(851, 88);
            this.uiPanel2.TabIndex = 4;
            this.uiPanel2.Text = "Panel 2";
            // 
            // uiPanel2Container
            // 
            this.uiPanel2Container.Location = new System.Drawing.Point(0, 0);
            this.uiPanel2Container.Name = "uiPanel2Container";
            this.uiPanel2Container.Size = new System.Drawing.Size(851, 88);
            this.uiPanel2Container.TabIndex = 0;
            // 
            // FilePublishControl
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.uiPanelAdFile);
            this.Font = global::AdManagerClient.Properties.Settings.Default.Font1;
            this.Name = "FilePublishControl";
            this.Size = new System.Drawing.Size(1010, 677);
            this.Load += new System.EventHandler(this.UserControl_Load);
            ((System.ComponentModel.ISupportInitialize)(this.uiPM)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelAdFile)).EndInit();
            this.uiPanelAdFile.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelSearch)).EndInit();
            this.uiPanelSearch.ResumeLayout(false);
            this.uiPanelSearchContainer.ResumeLayout(false);
            this.pnlSearch.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelConfirm)).EndInit();
            this.uiPanelConfirm.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelSchduleConfirm)).EndInit();
            this.uiPanelSchduleConfirm.ResumeLayout(false);
            this.uiPanelSchduleConfirmContainer.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdExPublishList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dvFilePublish)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.filePublishDs)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelPublishConfirm)).EndInit();
            this.uiPanelPublishConfirm.ResumeLayout(false);
            this.uiPanelPublishConfirmContainer.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelPublish)).EndInit();
            this.uiPanelPublish.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelHistory)).EndInit();
            this.uiPanelHistory.ResumeLayout(false);
            this.uiPanelHistoryContainer.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdExHistory)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dvHistory)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelPubDetail)).EndInit();
            this.uiPanelPubDetail.ResumeLayout(false);
            this.uiPanelPubDetailContainer.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdExFileList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dvFileList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dvWork)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelDetail)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelMaster)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdExScheduleList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanel1)).EndInit();
            this.uiPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.uiPanel2)).EndInit();
            this.uiPanel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }
		#endregion

		#region ÄÁÆ®·Ñ ·Îµå
		private void UserControl_Load(object sender, System.EventArgs e)
		{
			Application.DoEvents();

			// µ¥ÀÌÅÍ°ü¸®¿ë °´Ã¼»ý¼º
			dt = ((DataView)grdExPublishList.DataSource).Table;  
			cm = (CurrencyManager) this.BindingContext[grdExPublishList.DataSource]; 
			cm.PositionChanged += new System.EventHandler(OnGrdRowChanged); 

			// ÄÁÆ®·Ñ ÃÊ±âÈ­
			InitControl();	
		}

		#endregion

		#region ÄÁÆ®·Ñ ÃÊ±âÈ­
		private void InitControl()
		{
			ProgressStart();

			lbMsg.Text = "";

			InitCombo();
			InitCombo_Level();
	
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
		
			// »èÁ¦¹öÆ° È°¼ºÈ­
			if(menu.CanDelete(MenuCode))
			{
				//canDelete = true;
			}
			InitButton();
			ProgressStop();

			if(canRead) SearchSchedule();

		}


		private void InitCombo()
		{
			Init_MediaCode();
		}

		private void Init_MediaCode()
		{
			// ¸ÅÃ¼¸¦ Á¶È¸ÇÑ´Ù.
			MediaCodeModel mediacodeModel = new MediaCodeModel();
			new MediaCodeManager(systemModel, commonModel).GetMediaCodeList(mediacodeModel);
			
			if (mediacodeModel.ResultCD.Equals("0000"))
			{
				// µ¥ÀÌÅÍ¼Â¿¡ ¼ÂÆÃ
				Utility.SetDataTable(filePublishDs.Medias, mediacodeModel.MediaCodeDataSet);				
			}

			// °Ë»öÁ¶°ÇÀÇ ÄÞº¸
			this.cbSearchMedia.Items.Clear();
			
			// ÄÞº¸¹Ú½º¿¡ ¼ÂÆ®ÇÒ ÄÚµå¸ñ·ÏÀ» ´ãÀ» Item¹è¿­À» ¼±¾ð
			Janus.Windows.EditControls.UIComboBoxItem[] comboItems = new Janus.Windows.EditControls.UIComboBoxItem[mediacodeModel.ResultCnt + 1];

			comboItems[0] = new Janus.Windows.EditControls.UIComboBoxItem("¸ÅÃ¼¼±ÅÃ","00");
			
			for(int i=0;i<mediacodeModel.ResultCnt;i++)
			{
				DataRow row = filePublishDs.Medias.Rows[i];

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
				for(int i=0;i < filePublishDs.Medias.Rows.Count;i++)
				{
					DataRow row = filePublishDs.Medias.Rows[i];					
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
			btnPublishConfirm.Enabled = false;
		
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
                Debug.WriteLine("RowPosition : " + cm.Position.ToString() + " | " + cm.Count.ToString() + " | " + dt.Rows.Count.ToString());
                SetDetailText();
                //SearchWork();
                SearchFileList();
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
			DisableButton();
			SearchSchedule();
			InitButton();
		}

		/// <summary>
		/// ¹èÆ÷½ÂÀÎ ¹öÆ° Ã³¸®
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnPublishConfirm_Click(object sender, System.EventArgs e)
		{
			DialogResult result = MessageBox.Show("¹èÆ÷½ÂÀÎÇÏ½Ã°Ú½À´Ï±î?\n¹èÆ÷½ÂÀÎÈÄ Ãë¼ÒÇÒ ¼ö ¾ø½À´Ï´Ù.","ÆÄÀÏ¹èÆ÷¹èÆ÷½ÂÀÎ",
				MessageBoxButtons.YesNo, MessageBoxIcon.Question,
				MessageBoxDefaultButton.Button2);

			if (result == DialogResult.No) return;

			result = MessageBox.Show("Ã³¸®ÇÏ½Ã¸é º¯°æ³»¿ªÀÇ ¸ðµç ÆÄÀÏÀÌ ¹èÆ÷µË´Ï´Ù.\n´Ù½Ã È®ÀÎÇÏ°í ÀÛ¾÷ÇÏ½Ê½Ã¿ä!!", "ÆÄÀÏ¹èÆ÷¹èÆ÷½ÂÀÎ",
				MessageBoxButtons.YesNo, MessageBoxIcon.Warning,
				MessageBoxDefaultButton.Button2);

			if (result == DialogResult.No) return;

			DisableButton();
			SchedulePublish();
			InitButton();		
		}	

		#endregion

		#region Ã³¸®¸Þ¼Òµå

		/// <summary>
		/// ÆÄÀÏ¹èÆ÷½ÂÀÎÀÌ·Â Á¶È¸
		/// </summary>
		private void SearchSchedule()
		{
            IsSearching = true;

			StatusMessage("ÆÄÀÏ¹èÆ÷½ÂÀÎÀÌ·ÂÀ» Á¶È¸ÇÕ´Ï´Ù.");

			if(cbSearchMedia.SelectedItem.Value.Equals("00")) 
			{
				MessageBox.Show("¸ÅÃ¼¸¦ ¼±ÅÃÇÏ¿© ÁÖ½Ã±â ¹Ù¶ø´Ï´Ù.","ÆÄÀÏ¹èÆ÷½ÂÀÎÀÌ·Â Á¶È¸", MessageBoxButtons.OK, MessageBoxIcon.Information );
				return;
			}

			ProgressStart();
			try
			{
				keyAckNo    = "";
				keyAckState = "";

				filePublishDs.Publish.Clear();
                filePublishDs.History.Clear();
				filePublishModel.Init();
				filePublishModel.SearchMediaCode		 =  cbSearchMedia.SelectedItem.Value.ToString(); 

				// ÇöÀç ½ÂÀÎ»óÅÂÁ¶È¸ ¼­ºñ½º¸¦ È£ÃâÇÑ´Ù.
				new FilePublishManager(systemModel,commonModel).GetPublishState(filePublishModel);

				if (filePublishModel.ResultCD.Equals("0000"))
				{
					keyAckNo    = filePublishModel.AckNo;
					keyAckState = filePublishModel.State;
				}

				filePublishModel.Init();
				filePublishModel.SearchMediaCode		 =  cbSearchMedia.SelectedItem.Value.ToString(); 


				// ±¤°í½ÂÀÎÀÌ·Â Á¶È¸ ¼­ºñ½º¸¦ È£ÃâÇÑ´Ù.
				new FilePublishManager(systemModel,commonModel).GetPublishList(filePublishModel);

				if (filePublishModel.ResultCD.Equals("0000"))
				{
                    filePublishDs.Publish.BeginLoadData();
					Utility.SetDataTable(filePublishDs.Publish, filePublishModel.PublishDataSet);
   					StatusMessage(filePublishModel.ResultCnt + "°ÇÀÌ Á¶È¸µÇ¾ú½À´Ï´Ù.");
                    filePublishDs.Publish.EndLoadData();
                    Application.DoEvents();
					AddSchChoice();

					keyMediaCode = cbSearchMedia.SelectedItem.Value.ToString(); 
					//SetDetailText();
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
			int rowIndex = 0;
			if ( dt.Rows.Count < 1 ) return;              
			foreach (DataRow row in dt.Rows)
			{					
				if(row["AckNo"].ToString().Equals(keyAckNo))
				{					
					cm.Position = rowIndex;
					break;								
				}				
				rowIndex++;
			}
			grdExPublishList.EnsureVisible();
		}

		
		/// <summary>
		/// ±¤°íÆÄÀÏ »ó¼¼Á¤º¸ÀÇ ¼ÂÆ®
		/// </summary>
		private void SetDetailText()
		{
			int curRow = cm.Position;

			if(curRow >= 0 && dt.Rows.Count > 0 )
			{
                #region [½ÂÀÎ³»¿ª»ó¼¼Á¤º¸]
				ebAckStateName.Text     = dt.Rows[curRow]["AckStateName"].ToString();
				ebModifyStartDay.Text   = dt.Rows[curRow]["ModifyStartDay"].ToString();
				ebPublishUserName.Text  = dt.Rows[curRow]["PublishUserName"].ToString();
				ebPublishDay.Text       = dt.Rows[curRow]["PublishDay"].ToString();
				ebPublishDesc.Text      = dt.Rows[curRow]["PublishDesc"].ToString().Replace("\n","\r\n");
				string AckState         = dt.Rows[curRow]["AckState"].ToString();
				string AckNo            = dt.Rows[curRow]["AckNo"].ToString();
				keyAckNoRow	= Convert.ToInt32(AckNo);

				// ±âº»Àº ¼öÁ¤ºÒ°¡, ½ÂÀÎºÒ°¡
				btnPublishConfirm.Enabled	= false;

				ebPublishDesc.ReadOnly = true;
				ebPublishDesc.BackColor = Color.WhiteSmoke;

				if(canUpdate) // ¾÷µ¥ÀÌÆ® ±ÇÇÑÀÌ ÀÖÀ»¶§
				{
					// ±Ý¹ø ½ÂÀÎ³»¿ª°ú ¼±ÅÃÇÑ ½ÂÀÎ³»¿ªÀÌ °°À¸¸é
					if(keyAckNo.Equals(AckNo))
					{
						if(AckState.Equals("10")) // ±Ý¹ø ½ÂÀÎ³»¿ªÀÇ »óÅÂ°¡ 10:½ÂÀÎ´ë±â ÀÌ¸é ÆÄÀÏ¹èÆ÷½ÂÀÎ °¡´ÉÇÏ´Ù.
						{
							uiPanelPublishConfirm.Text	= "½ÂÀÎÃ³¸® : ÇöÀç ÆÄÀÏ¹èÆ÷¸¦ ½ÂÀÎÇÕ´Ï´Ù.";
							
							btnPublishConfirm.Enabled	= true;
							ebPublishDesc.ReadOnly		= false;
							ebPublishDesc.BackColor		= Color.White;
						}
						else
						{
							uiPanelPublishConfirm.Text = "½ÂÀÎÃ³¸® : ÇöÀç ÆÄÀÏ¹èÆ÷Àº ¹èÆ÷½ÂÀÎµÇ¾ú½À´Ï´Ù.";
						}
					}
				}
				Application.DoEvents();
                #endregion

                #region	[º¯°æµÈ ±¤°íÆÄÀÏ ¸ñ·Ï]		
				filePublishDs.History.Clear();
				filePublishModel.Init();
				filePublishModel.MediaCode	=  keyMediaCode;
				filePublishModel.AckNo      =  AckNo;


				// ±¤°í½ÂÀÎÀÌ·Â Á¶È¸ ¼­ºñ½º¸¦ È£ÃâÇÑ´Ù.
                // ÇØ´ç½ÂÀÎ¹øÈ£¿¡ Ãß°¡/»èÁ¦µÈ ÆÄÀÏµé ¸ñ·Ï
				new FilePublishManager(systemModel,commonModel).GetPublishHistory(filePublishModel);
				if (filePublishModel.ResultCD.Equals("0000"))
				{
					Utility.SetDataTable(filePublishDs.History, filePublishModel.HistoryDataSet);
				}
				
				Application.DoEvents();
                #endregion
			}
		}


		/// <summary>
		/// ÆÄÀÏ¹èÆ÷½ÂÀÎ
		/// </summary>
		private void SchedulePublish()
		{
			StatusMessage("ÆÄÀÏ¹èÆ÷½ÂÀÎ Ã³¸®ÇÕ´Ï´Ù.");

			if(keyAckNo.Length == 0) 
			{
				MessageBox.Show("¼±ÅÃµÈ ÆÄÀÏ½ÂÀÎ³»¿ªÀÌ ¾ø½À´Ï´Ù.","ÆÄÀÏ¹èÆ÷½ÂÀÎ", MessageBoxButtons.OK, MessageBoxIcon.Information );
				return;
			}

			try
			{
				filePublishModel.Init();
				filePublishModel.AckNo       = keyAckNo;
				filePublishModel.PublishDesc = ebPublishDesc.Text;
				filePublishModel.MediaCode   = keyMediaCode;

				// ±¤°í¹èÆ÷½ÂÀÎ Ã³¸® ¼­ºñ½º¸¦ È£ÃâÇÑ´Ù.
				new FilePublishManager(systemModel,commonModel).SetFilePublish(filePublishModel);

				if (filePublishModel.ResultCD.Equals("0000"))
				{
					StatusMessage("ÆÄÀÏ¹èÆ÷½ÂÀÎ Ã³¸®µÇ¾ú½À´Ï´Ù.");
					SearchSchedule();
				}
			}
			catch(FrameException fe)
			{
				FrameSystem.showMsgForm("ÆÄÀÏ¹èÆ÷½ÂÀÎ ¿À·ù", new string[] {fe.ErrCode, fe.ResultMsg});
			}
			catch(Exception ex)
			{
				FrameSystem.showMsgForm("ÆÄÀÏ¹èÆ÷½ÂÀÎ ¿À·ù",new string[] {"",ex.Message});
			}
		}


		/// <summary>
		/// ¿¹¾àÀÛ¾÷ ÀÛ¾÷¸ñ·Ï
		/// </summary>
		private void SearchWork()
		{
			try
			{
				filePublishModel.Init();
				filePublishModel.MediaCode  = "0";
				filePublishModel.AckNo		= Convert.ToString(keyAckNoRow);

				new FilePublishManager(systemModel,commonModel).GetReserveWorkList(filePublishModel);

				if (filePublishModel.ResultCD.Equals("0000"))
				{
					foreach (DataRow row in filePublishModel.FileListDataSet.Tables[0].Rows)
					{
						if( row["ReserveState"].ToString() == "90" && Convert.ToInt32( row["ReserveCount"].ToString() ) == 0 )
						{
							row.Delete();
						}
					}

					filePublishModel.FileListDataSet.Tables[0].AcceptChanges();

					filePublishDs.FIleReserve.Clear();
                    filePublishDs.FileList.Clear();
                    filePublishDs.FIleReserve.BeginLoadData();
					Utility.SetDataTable(filePublishDs.FIleReserve, filePublishModel.FileListDataSet);
                    filePublishDs.FIleReserve.EndLoadData();
                    Application.DoEvents();

				}
			}
			catch(FrameException fe)
			{
				FrameSystem.showMsgForm("¿¹¾à½ÂÀÎ ÀÛ¾÷¸ñ·Ï Á¶È¸ ¿À·ù", new string[] {fe.ErrCode, fe.ResultMsg});
			}
			catch(Exception ex)
			{
				FrameSystem.showMsgForm("¿¹¾à½ÂÀÎ ÀÛ¾÷¸ñ·Ï Á¶È¸ ¿À·ù",new string[] {"",ex.Message});
			}
		}
		#endregion

        /// <summary>
        /// ±¤°í¹èÆ÷¸ñ·Ï
        /// </summary>
        private void SearchFileList()
        {
            {
                //keyWorkDt = dtWork.Rows[curRow]["ReserveDt"].ToString();

                #region [ ¹èÆ÷µÈ ÆÄÀÏ¸ñ·Ï ]
                filePublishDs.FileList.Clear();
                filePublishModel.Init();
                filePublishModel.MediaCode = keyMediaCode;
                filePublishModel.AckNo = keyAckNoRow.ToString();
                filePublishModel.ReserveDt = keyWorkDt;


                // ±¤°í½ÂÀÎÀÌ·Â Á¶È¸ ¼­ºñ½º¸¦ È£ÃâÇÑ´Ù.
                Cursor.Current = Cursors.WaitCursor;
                new FilePublishManager(systemModel, commonModel).GetPublishFileList(filePublishModel);

                if (filePublishModel.ResultCD.Equals("0000"))
                {
                    Utility.SetDataTable(filePublishDs.FileList, filePublishModel.FileListDataSet);
                    uiPanelPubDetail.Text = "½ÂÀÎÆÄÀÏ¸ñ·Ï : " + filePublishModel.ResultCnt.ToString("#,##0") + "°Ç Á¶È¸µÊ";
                }
                Cursor.Current = Cursors.Arrow;
                #endregion
            }
        }

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

		private void grdWork_RowDoubleClick(object sender, Janus.Windows.GridEX.RowActionEventArgs e)
		{
			string revKey = e.Row.Cells["ReserveDt"].Value.ToString();
			
			Reserve_pForm pForm = new Reserve_pForm();
			pForm.AckNo		= keyAckNoRow;
			pForm.ReserveDt = revKey;
			//pForm.Show();
			pForm.SearchMain();
			pForm.ShowDialog(this);
			//pForm.TopMost = true;
			//SearchWork();
		}

		private void grdWork_LoadingRow(object sender, Janus.Windows.GridEX.RowLoadEventArgs e)
		{
		}

        private void grdWork_SelectionChanged(object sender, System.EventArgs e)
        {
            Debug.WriteLine("SelectionChanged");
        }
	}
}
