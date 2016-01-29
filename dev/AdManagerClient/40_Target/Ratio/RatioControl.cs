// ===============================================================================
// RatioControl for Charites Project
//
// RatioControl.cs
//
// ÁöÁ¤±¤°íÆí¼º ÄÁÆ®·ÑÀ» Á¤ÀÇÇÕ´Ï´Ù. 
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
	/// ±¤°íÆÄÀÏ°ü¸® ÄÁÆ®·Ñ
	/// </summary>
    public class RatioControl : System.Windows.Forms.UserControl, IUserControl
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
		RatioModel ratioModel  = new RatioModel();	// Æí¼ººñÀ²¸ðµ¨
		SchChoiceAdModel schChoiceAdModel  = new SchChoiceAdModel();	// ÁöÁ¤±¤°íÆí¼º¸ðµ¨
		SchPublishModel schPublishModel  = new SchPublishModel();	// ±¤°í½ÂÀÎ¸ðµ¨

		// È­¸éÃ³¸®¿ë º¯¼ö
		bool IsNewSearchKey		  = true;					// °Ë»ö¾îÀÔ·Â ¿©ºÎ
		CurrencyManager cm        = null;					// µ¥ÀÌÅÍ ±×¸®µåÀÇ º¯°æ¿¡ µû¸¥ µ¥ÀÌÅÍ¼Â °ü¸®¸¦ À§ÇÏ¿©			
		CurrencyManager cmMenu        = null;					// µ¥ÀÌÅÍ ±×¸®µåÀÇ º¯°æ¿¡ µû¸¥ µ¥ÀÌÅÍ¼Â °ü¸®¸¦ À§ÇÏ¿©			
		CurrencyManager cmMenu2        = null;					// µ¥ÀÌÅÍ ±×¸®µåÀÇ º¯°æ¿¡ µû¸¥ µ¥ÀÌÅÍ¼Â °ü¸®¸¦ À§ÇÏ¿©			
		CurrencyManager cmMenu3        = null;					// µ¥ÀÌÅÍ ±×¸®µåÀÇ º¯°æ¿¡ µû¸¥ µ¥ÀÌÅÍ¼Â °ü¸®¸¦ À§ÇÏ¿©			
		CurrencyManager cmMenu4        = null;					// µ¥ÀÌÅÍ ±×¸®µåÀÇ º¯°æ¿¡ µû¸¥ µ¥ÀÌÅÍ¼Â °ü¸®¸¦ À§ÇÏ¿©			
		CurrencyManager cmMenu5        = null;					// µ¥ÀÌÅÍ ±×¸®µåÀÇ º¯°æ¿¡ µû¸¥ µ¥ÀÌÅÍ¼Â °ü¸®¸¦ À§ÇÏ¿©			
		DataTable       dt        = null;
		DataTable       dtMenu    = null;
		DataTable       dtMenu2    = null;
		DataTable       dtMenu3    = null;
		DataTable       dtMenu4    = null;
		DataTable       dtMenu5    = null;

        bool IsSearching = false; // Á¶È¸Áß »ó¼¼È­¸éÀÌ ¾÷µ¥ÀÌÆ® µÇ´Â °ÍÀ» ¹æÁö ÇÏ±âÀ§ÇÔ 2011.11.29 JH.Park
		bool canRead			  = false;
		bool canCreate            = false;
		bool canDelete            = false;

		// Key µ¥ÀÌÅÍ
		bool IsNotLoading		  = true;					// »ó¼¼Á¶È¸ÁßÀÌ ¾Æ´Ô
		string keyMediaCode    = "";
		public string keyItemNo       = "";
		string keyAdType       = "";


		public string ItemNo
		{
			get { return keyItemNo;	}
			set { keyItemNo = value;	}
		}

		public string MediaCode
		{
			get { return keyMediaCode;	}
			set { keyMediaCode = value;	}
		}
		
		public string AdType
		{
			get { return keyAdType;	}
			set { keyAdType = value;	}
		}


		// Æí¼º¹èÆ÷ ½ÂÀÎ»óÅÂ Ã³¸®¿ë
		private string keyAckNo    = "";
        private Janus.Windows.EditControls.UICheckBox chkAdState_40;
        private Janus.Windows.EditControls.UICheckBox chkAdState_30;
        private Janus.Windows.EditControls.UICheckBox chkAdState_20;
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
		private System.Windows.Forms.Panel pnlSearch;
		private System.Windows.Forms.Panel pnlSearchBtn;
		private Janus.Windows.EditControls.UIButton btnSearch;
		private Janus.Windows.UI.Dock.UIPanel uiPanelSearch;
		private Janus.Windows.UI.Dock.UIPanelInnerContainer uiPanelSearchContainer;
		private Janus.Windows.UI.Dock.UIPanel uiPanelList;
		private Janus.Windows.UI.Dock.UIPanelInnerContainer uiPanelListContainer;
		private Janus.Windows.GridEX.EditControls.EditBox ebSearchKey;
		private System.Data.DataView dvSchedule;
        private Janus.Windows.UI.Dock.UIPanelGroup uiPanelChoiceAdSchedule;
		private System.Windows.Forms.Label lbAdState;
		private Janus.Windows.EditControls.UIComboBox cbSearchMedia;
        private Janus.Windows.EditControls.UIComboBox cbSearchRap;
        private Janus.Windows.GridEX.GridEX grdExScheduleList;
		private Janus.Windows.EditControls.UIButton btnAdd;
		private System.Windows.Forms.Label label1;
		private Janus.Windows.GridEX.EditControls.EditBox editBox1;
		private System.Windows.Forms.Panel panMenuSchedule;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Panel panel2;
		private System.Windows.Forms.Panel panel3;
		private System.Windows.Forms.Panel panel4;
		private System.Data.DataView dvMenu;
		private System.Windows.Forms.Label lbMsg;
		private Janus.Windows.UI.Dock.UIPanel uiPanelGroup3;
		private Janus.Windows.UI.Dock.UIPanelInnerContainer uiPanelGroup3Container;
		private Janus.Windows.UI.Dock.UIPanel uiPanelGroup4;
        private Janus.Windows.UI.Dock.UIPanelInnerContainer uiPanelGroup4Container;
		private System.Windows.Forms.Panel panel7;
        private System.Windows.Forms.Panel panel9;
		private AdManagerClient._40_Target.Ratio.RatioDs ratioDs;
		private System.Data.DataView dvMenu2;
		private System.Data.DataView dvMenu3;
		private System.Data.DataView dvMenu4;
		private System.Data.DataView dvMenu5;
        private Janus.Windows.UI.Dock.UIPanelGroup uiPanelGroup;
        private Janus.Windows.UI.Dock.UIPanel uiPanelGroup1;
        private Janus.Windows.UI.Dock.UIPanelInnerContainer uiPanelGroup1Container;
        private Janus.Windows.UI.Dock.UIPanel uiPanelGroup2;
        private Janus.Windows.UI.Dock.UIPanelInnerContainer uiPanelGroup2Container;
        private Janus.Windows.UI.Dock.UIPanelGroup uiPanelGroup0;
        private Janus.Windows.GridEX.GridEX grdExGroup1;
        private Janus.Windows.GridEX.GridEX grdExGroup2;
        private Janus.Windows.GridEX.GridEX grdExGroup3;
        private Janus.Windows.GridEX.GridEX grdExGroup4;
		private System.ComponentModel.IContainer components;

		public RatioControl()
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
            Janus.Windows.GridEX.GridEXLayout grdExScheduleList_DesignTimeLayout = new Janus.Windows.GridEX.GridEXLayout();
            Janus.Windows.Common.Layouts.JanusLayoutReference grdExScheduleList_DesignTimeLayout_Reference_0 = new Janus.Windows.Common.Layouts.JanusLayoutReference("GridEXLayoutData.RootTable.FormatConditions.Condition0.FormatStyle.BackgroundImag" +
        "e");
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RatioControl));
            Janus.Windows.Common.Layouts.JanusLayoutReference grdExScheduleList_DesignTimeLayout_Reference_1 = new Janus.Windows.Common.Layouts.JanusLayoutReference("GridEXLayoutData.RootTable.FormatConditions.Condition1.FormatStyle.BackgroundImag" +
        "e");
            Janus.Windows.Common.Layouts.JanusLayoutReference grdExScheduleList_DesignTimeLayout_Reference_2 = new Janus.Windows.Common.Layouts.JanusLayoutReference("GridEXLayoutData.RootTable.FormatConditions.Condition2.FormatStyle.BackgroundImag" +
        "e");
            Janus.Windows.Common.Layouts.JanusLayoutReference grdExScheduleList_DesignTimeLayout_Reference_3 = new Janus.Windows.Common.Layouts.JanusLayoutReference("GridEXLayoutData.RootTable.FormatConditions.Condition4.FormatStyle.BackgroundImag" +
        "e");
            Janus.Windows.Common.Layouts.JanusLayoutReference grdExScheduleList_DesignTimeLayout_Reference_4 = new Janus.Windows.Common.Layouts.JanusLayoutReference("GridEXLayoutData.RootTable.FormatConditions.Condition5.FormatStyle.BackgroundImag" +
        "e");
            Janus.Windows.GridEX.GridEXLayout grdExGroup1_DesignTimeLayout = new Janus.Windows.GridEX.GridEXLayout();
            Janus.Windows.GridEX.GridEXLayout grdExGroup2_DesignTimeLayout = new Janus.Windows.GridEX.GridEXLayout();
            Janus.Windows.GridEX.GridEXLayout grdExGroup3_DesignTimeLayout = new Janus.Windows.GridEX.GridEXLayout();
            Janus.Windows.GridEX.GridEXLayout grdExGroup4_DesignTimeLayout = new Janus.Windows.GridEX.GridEXLayout();
            this.uiPM = new Janus.Windows.UI.Dock.UIPanelManager(this.components);
            this.uiPanelChoiceAdSchedule = new Janus.Windows.UI.Dock.UIPanelGroup();
            this.uiPanelSearch = new Janus.Windows.UI.Dock.UIPanel();
            this.uiPanelSearchContainer = new Janus.Windows.UI.Dock.UIPanelInnerContainer();
            this.pnlSearch = new System.Windows.Forms.Panel();
            this.chkAdState_40 = new Janus.Windows.EditControls.UICheckBox();
            this.chkAdState_30 = new Janus.Windows.EditControls.UICheckBox();
            this.chkAdState_20 = new Janus.Windows.EditControls.UICheckBox();
            this.lbAdState = new System.Windows.Forms.Label();
            this.ebSearchKey = new Janus.Windows.GridEX.EditControls.EditBox();
            this.cbSearchMedia = new Janus.Windows.EditControls.UIComboBox();
            this.cbSearchRap = new Janus.Windows.EditControls.UIComboBox();
            this.pnlSearchBtn = new System.Windows.Forms.Panel();
            this.btnSearch = new Janus.Windows.EditControls.UIButton();
            this.btnAdd = new Janus.Windows.EditControls.UIButton();
            this.lbMsg = new System.Windows.Forms.Label();
            this.uiPanelList = new Janus.Windows.UI.Dock.UIPanel();
            this.uiPanelListContainer = new Janus.Windows.UI.Dock.UIPanelInnerContainer();
            this.grdExScheduleList = new Janus.Windows.GridEX.GridEX();
            this.dvSchedule = new System.Data.DataView();
            this.ratioDs = new AdManagerClient._40_Target.Ratio.RatioDs();
            this.uiPanelGroup = new Janus.Windows.UI.Dock.UIPanelGroup();
            this.uiPanelGroup0 = new Janus.Windows.UI.Dock.UIPanelGroup();
            this.uiPanelGroup1 = new Janus.Windows.UI.Dock.UIPanel();
            this.uiPanelGroup1Container = new Janus.Windows.UI.Dock.UIPanelInnerContainer();
            this.panMenuSchedule = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.grdExGroup1 = new Janus.Windows.GridEX.GridEX();
            this.dvMenu = new System.Data.DataView();
            this.uiPanelGroup2 = new Janus.Windows.UI.Dock.UIPanel();
            this.uiPanelGroup2Container = new Janus.Windows.UI.Dock.UIPanelInnerContainer();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel4 = new System.Windows.Forms.Panel();
            this.grdExGroup2 = new Janus.Windows.GridEX.GridEX();
            this.dvMenu2 = new System.Data.DataView();
            this.uiPanelGroup3 = new Janus.Windows.UI.Dock.UIPanel();
            this.uiPanelGroup3Container = new Janus.Windows.UI.Dock.UIPanelInnerContainer();
            this.panel7 = new System.Windows.Forms.Panel();
            this.grdExGroup3 = new Janus.Windows.GridEX.GridEX();
            this.dvMenu3 = new System.Data.DataView();
            this.uiPanelGroup4 = new Janus.Windows.UI.Dock.UIPanel();
            this.uiPanelGroup4Container = new Janus.Windows.UI.Dock.UIPanelInnerContainer();
            this.panel9 = new System.Windows.Forms.Panel();
            this.grdExGroup4 = new Janus.Windows.GridEX.GridEX();
            this.dvMenu4 = new System.Data.DataView();
            this.dvMenu5 = new System.Data.DataView();
            this.label1 = new System.Windows.Forms.Label();
            this.editBox1 = new Janus.Windows.GridEX.EditControls.EditBox();
            ((System.ComponentModel.ISupportInitialize)(this.uiPM)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelChoiceAdSchedule)).BeginInit();
            this.uiPanelChoiceAdSchedule.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelSearch)).BeginInit();
            this.uiPanelSearch.SuspendLayout();
            this.uiPanelSearchContainer.SuspendLayout();
            this.pnlSearch.SuspendLayout();
            this.pnlSearchBtn.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelList)).BeginInit();
            this.uiPanelList.SuspendLayout();
            this.uiPanelListContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdExScheduleList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dvSchedule)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ratioDs)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelGroup)).BeginInit();
            this.uiPanelGroup.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelGroup0)).BeginInit();
            this.uiPanelGroup0.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelGroup1)).BeginInit();
            this.uiPanelGroup1.SuspendLayout();
            this.uiPanelGroup1Container.SuspendLayout();
            this.panMenuSchedule.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdExGroup1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dvMenu)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelGroup2)).BeginInit();
            this.uiPanelGroup2.SuspendLayout();
            this.uiPanelGroup2Container.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdExGroup2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dvMenu2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelGroup3)).BeginInit();
            this.uiPanelGroup3.SuspendLayout();
            this.uiPanelGroup3Container.SuspendLayout();
            this.panel7.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdExGroup3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dvMenu3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelGroup4)).BeginInit();
            this.uiPanelGroup4.SuspendLayout();
            this.uiPanelGroup4Container.SuspendLayout();
            this.panel9.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdExGroup4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dvMenu4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dvMenu5)).BeginInit();
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
            this.uiPM.SplitterSize = 1;
            this.uiPM.VisualStyle = Janus.Windows.UI.Dock.PanelVisualStyle.Office2007;
            this.uiPanelChoiceAdSchedule.Id = new System.Guid("b162c53a-7940-47cd-a0ed-9762c59c50d8");
            this.uiPanelChoiceAdSchedule.StaticGroup = true;
            this.uiPanelSearch.Id = new System.Guid("afb3b7d0-aa79-443c-a1e8-3baba38f7a4d");
            this.uiPanelChoiceAdSchedule.Panels.Add(this.uiPanelSearch);
            this.uiPanelList.Id = new System.Guid("8e3140b9-bfcb-43af-b84e-2ee6cddf51dc");
            this.uiPanelChoiceAdSchedule.Panels.Add(this.uiPanelList);
            this.uiPanelGroup.Id = new System.Guid("8d086974-c05e-4790-b433-fc60b53fdd4f");
            this.uiPanelGroup.StaticGroup = true;
            this.uiPanelGroup0.Id = new System.Guid("18e78aea-bf80-4094-94f6-0d7d6006bb3d");
            this.uiPanelGroup0.StaticGroup = true;
            this.uiPanelGroup1.Id = new System.Guid("02c920f2-57c5-4aa8-bf42-309880d3314e");
            this.uiPanelGroup0.Panels.Add(this.uiPanelGroup1);
            this.uiPanelGroup2.Id = new System.Guid("f40b18e5-7e83-4164-be4f-97c23451198d");
            this.uiPanelGroup0.Panels.Add(this.uiPanelGroup2);
            this.uiPanelGroup3.Id = new System.Guid("aa1f6a69-d6bc-434a-a10e-7107cb5e655d");
            this.uiPanelGroup0.Panels.Add(this.uiPanelGroup3);
            this.uiPanelGroup4.Id = new System.Guid("2b15e0cb-4e3a-4682-a3d0-67614f53ff72");
            this.uiPanelGroup0.Panels.Add(this.uiPanelGroup4);
            this.uiPanelGroup.Panels.Add(this.uiPanelGroup0);
            this.uiPanelChoiceAdSchedule.Panels.Add(this.uiPanelGroup);
            this.uiPM.Panels.Add(this.uiPanelChoiceAdSchedule);
            // 
            // Design Time Panel Info:
            // 
            this.uiPM.BeginPanelInfo();
            this.uiPM.AddDockPanelInfo(new System.Guid("b162c53a-7940-47cd-a0ed-9762c59c50d8"), Janus.Windows.UI.Dock.PanelGroupStyle.HorizontalTiles, Janus.Windows.UI.Dock.PanelDockStyle.Fill, true, new System.Drawing.Size(1010, 677), true);
            this.uiPM.AddDockPanelInfo(new System.Guid("afb3b7d0-aa79-443c-a1e8-3baba38f7a4d"), new System.Guid("b162c53a-7940-47cd-a0ed-9762c59c50d8"), 62, true);
            this.uiPM.AddDockPanelInfo(new System.Guid("8e3140b9-bfcb-43af-b84e-2ee6cddf51dc"), new System.Guid("b162c53a-7940-47cd-a0ed-9762c59c50d8"), 229, true);
            this.uiPM.AddDockPanelInfo(new System.Guid("8d086974-c05e-4790-b433-fc60b53fdd4f"), new System.Guid("b162c53a-7940-47cd-a0ed-9762c59c50d8"), Janus.Windows.UI.Dock.PanelGroupStyle.VerticalTiles, true, 336, true);
            this.uiPM.AddDockPanelInfo(new System.Guid("18e78aea-bf80-4094-94f6-0d7d6006bb3d"), new System.Guid("8d086974-c05e-4790-b433-fc60b53fdd4f"), Janus.Windows.UI.Dock.PanelGroupStyle.VerticalTiles, true, 425, true);
            this.uiPM.AddDockPanelInfo(new System.Guid("02c920f2-57c5-4aa8-bf42-309880d3314e"), new System.Guid("18e78aea-bf80-4094-94f6-0d7d6006bb3d"), 176, true);
            this.uiPM.AddDockPanelInfo(new System.Guid("f40b18e5-7e83-4164-be4f-97c23451198d"), new System.Guid("18e78aea-bf80-4094-94f6-0d7d6006bb3d"), 170, true);
            this.uiPM.AddDockPanelInfo(new System.Guid("aa1f6a69-d6bc-434a-a10e-7107cb5e655d"), new System.Guid("18e78aea-bf80-4094-94f6-0d7d6006bb3d"), 171, true);
            this.uiPM.AddDockPanelInfo(new System.Guid("2b15e0cb-4e3a-4682-a3d0-67614f53ff72"), new System.Guid("18e78aea-bf80-4094-94f6-0d7d6006bb3d"), 155, true);
            this.uiPM.AddFloatingPanelInfo(new System.Guid("b162c53a-7940-47cd-a0ed-9762c59c50d8"), Janus.Windows.UI.Dock.PanelGroupStyle.HorizontalTiles, true, new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
            this.uiPM.AddFloatingPanelInfo(new System.Guid("8e3140b9-bfcb-43af-b84e-2ee6cddf51dc"), new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
            this.uiPM.AddFloatingPanelInfo(new System.Guid("b2973a57-d3e1-4d80-9387-7a6dc3332542"), new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
            this.uiPM.AddFloatingPanelInfo(new System.Guid("afb3b7d0-aa79-443c-a1e8-3baba38f7a4d"), new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
            this.uiPM.AddFloatingPanelInfo(new System.Guid("18e78aea-bf80-4094-94f6-0d7d6006bb3d"), Janus.Windows.UI.Dock.PanelGroupStyle.VerticalTiles, true, new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
            this.uiPM.AddFloatingPanelInfo(new System.Guid("02c920f2-57c5-4aa8-bf42-309880d3314e"), new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
            this.uiPM.AddFloatingPanelInfo(new System.Guid("f40b18e5-7e83-4164-be4f-97c23451198d"), new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
            this.uiPM.AddFloatingPanelInfo(new System.Guid("8d086974-c05e-4790-b433-fc60b53fdd4f"), Janus.Windows.UI.Dock.PanelGroupStyle.VerticalTiles, true, new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
            this.uiPM.AddFloatingPanelInfo(new System.Guid("aa1f6a69-d6bc-434a-a10e-7107cb5e655d"), new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
            this.uiPM.AddFloatingPanelInfo(new System.Guid("2b15e0cb-4e3a-4682-a3d0-67614f53ff72"), new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
            this.uiPM.AddFloatingPanelInfo(new System.Guid("597354f6-55a9-4173-9b5d-57598ee39074"), new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
            this.uiPM.EndPanelInfo();
            // 
            // uiPanelChoiceAdSchedule
            // 
            this.uiPanelChoiceAdSchedule.ActiveCaptionMode = Janus.Windows.UI.Dock.ActiveCaptionMode.Never;
            this.uiPanelChoiceAdSchedule.BackColor = System.Drawing.SystemColors.Window;
            this.uiPanelChoiceAdSchedule.CaptionVisible = Janus.Windows.UI.InheritableBoolean.True;
            this.uiPanelChoiceAdSchedule.CloseButtonVisible = Janus.Windows.UI.InheritableBoolean.False;
            this.uiPanelChoiceAdSchedule.Location = new System.Drawing.Point(0, 0);
            this.uiPanelChoiceAdSchedule.Name = "uiPanelChoiceAdSchedule";
            this.uiPanelChoiceAdSchedule.Size = new System.Drawing.Size(1010, 677);
            this.uiPanelChoiceAdSchedule.TabIndex = 4;
            this.uiPanelChoiceAdSchedule.Text = "Æí¼ººñÀ²Á¶Á¤";
            // 
            // uiPanelSearch
            // 
            this.uiPanelSearch.AllowResize = Janus.Windows.UI.InheritableBoolean.False;
            this.uiPanelSearch.CaptionVisible = Janus.Windows.UI.InheritableBoolean.False;
            this.uiPanelSearch.CloseButtonVisible = Janus.Windows.UI.InheritableBoolean.False;
            this.uiPanelSearch.InnerContainer = this.uiPanelSearchContainer;
            this.uiPanelSearch.Location = new System.Drawing.Point(0, 22);
            this.uiPanelSearch.Name = "uiPanelSearch";
            this.uiPanelSearch.Size = new System.Drawing.Size(1010, 64);
            this.uiPanelSearch.TabIndex = 4;
            this.uiPanelSearch.Text = "°Ë»ö";
            // 
            // uiPanelSearchContainer
            // 
            this.uiPanelSearchContainer.Controls.Add(this.pnlSearch);
            this.uiPanelSearchContainer.Location = new System.Drawing.Point(1, 1);
            this.uiPanelSearchContainer.Name = "uiPanelSearchContainer";
            this.uiPanelSearchContainer.Size = new System.Drawing.Size(1008, 62);
            this.uiPanelSearchContainer.TabIndex = 0;
            // 
            // pnlSearch
            // 
            this.pnlSearch.BackColor = System.Drawing.SystemColors.Window;
            this.pnlSearch.Controls.Add(this.chkAdState_40);
            this.pnlSearch.Controls.Add(this.chkAdState_30);
            this.pnlSearch.Controls.Add(this.chkAdState_20);
            this.pnlSearch.Controls.Add(this.lbAdState);
            this.pnlSearch.Controls.Add(this.ebSearchKey);
            this.pnlSearch.Controls.Add(this.cbSearchMedia);
            this.pnlSearch.Controls.Add(this.cbSearchRap);
            this.pnlSearch.Controls.Add(this.pnlSearchBtn);
            this.pnlSearch.Controls.Add(this.lbMsg);
            this.pnlSearch.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlSearch.Location = new System.Drawing.Point(0, 0);
            this.pnlSearch.Name = "pnlSearch";
            this.pnlSearch.Size = new System.Drawing.Size(1008, 62);
            this.pnlSearch.TabIndex = 3;
            // 
            // chkAdState_40
            // 
            this.chkAdState_40.Location = new System.Drawing.Point(448, 8);
            this.chkAdState_40.Name = "chkAdState_40";
            this.chkAdState_40.Size = new System.Drawing.Size(49, 23);
            this.chkAdState_40.TabIndex = 44;
            this.chkAdState_40.Text = "Á¾·á";
            // 
            // chkAdState_30
            // 
            this.chkAdState_30.Checked = true;
            this.chkAdState_30.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkAdState_30.Location = new System.Drawing.Point(393, 8);
            this.chkAdState_30.Name = "chkAdState_30";
            this.chkAdState_30.Size = new System.Drawing.Size(49, 23);
            this.chkAdState_30.TabIndex = 44;
            this.chkAdState_30.Text = "ÁßÁö";
            // 
            // chkAdState_20
            // 
            this.chkAdState_20.Checked = true;
            this.chkAdState_20.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkAdState_20.Location = new System.Drawing.Point(338, 8);
            this.chkAdState_20.Name = "chkAdState_20";
            this.chkAdState_20.Size = new System.Drawing.Size(49, 23);
            this.chkAdState_20.TabIndex = 44;
            this.chkAdState_20.Text = "Æí¼º";
            // 
            // lbAdState
            // 
            this.lbAdState.Location = new System.Drawing.Point(275, 13);
            this.lbAdState.Name = "lbAdState";
            this.lbAdState.Size = new System.Drawing.Size(57, 16);
            this.lbAdState.TabIndex = 13;
            this.lbAdState.Text = "±¤°í»óÅÂ";
            // 
            // ebSearchKey
            // 
            this.ebSearchKey.Location = new System.Drawing.Point(529, 8);
            this.ebSearchKey.Name = "ebSearchKey";
            this.ebSearchKey.Size = new System.Drawing.Size(165, 21);
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
            this.cbSearchRap.Location = new System.Drawing.Point(136, 8);
            this.cbSearchRap.Name = "cbSearchRap";
            this.cbSearchRap.Size = new System.Drawing.Size(120, 21);
            this.cbSearchRap.TabIndex = 2;
            this.cbSearchRap.Text = "¹Ìµð¾î·¦¼±ÅÃ";
            this.cbSearchRap.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            // 
            // pnlSearchBtn
            // 
            this.pnlSearchBtn.BackColor = System.Drawing.SystemColors.Window;
            this.pnlSearchBtn.Controls.Add(this.btnSearch);
            this.pnlSearchBtn.Controls.Add(this.btnAdd);
            this.pnlSearchBtn.Dock = System.Windows.Forms.DockStyle.Right;
            this.pnlSearchBtn.Location = new System.Drawing.Point(888, 0);
            this.pnlSearchBtn.Name = "pnlSearchBtn";
            this.pnlSearchBtn.Size = new System.Drawing.Size(120, 62);
            this.pnlSearchBtn.TabIndex = 5;
            // 
            // btnSearch
            // 
            this.btnSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSearch.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSearch.Enabled = false;
            this.btnSearch.Location = new System.Drawing.Point(8, 4);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(104, 24);
            this.btnSearch.TabIndex = 11;
            this.btnSearch.Text = "Á¶ È¸";
            this.btnSearch.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // btnAdd
            // 
            this.btnAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAdd.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnAdd.Enabled = false;
            this.btnAdd.Location = new System.Drawing.Point(8, 32);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(104, 24);
            this.btnAdd.TabIndex = 14;
            this.btnAdd.Text = "ºñÀ²Æí¼ºÁ¶Á¤";
            this.btnAdd.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // lbMsg
            // 
            this.lbMsg.Location = new System.Drawing.Point(5, 37);
            this.lbMsg.Name = "lbMsg";
            this.lbMsg.Size = new System.Drawing.Size(442, 17);
            this.lbMsg.TabIndex = 43;
            this.lbMsg.Text = "Æí¼º";
            this.lbMsg.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // uiPanelList
            // 
            this.uiPanelList.AllowResize = Janus.Windows.UI.InheritableBoolean.True;
            this.uiPanelList.BackColor = System.Drawing.SystemColors.Window;
            this.uiPanelList.CaptionStyle = Janus.Windows.UI.Dock.PanelCaptionStyle.Dark;
            this.uiPanelList.InnerContainer = this.uiPanelListContainer;
            this.uiPanelList.Location = new System.Drawing.Point(0, 87);
            this.uiPanelList.MinimumSize = new System.Drawing.Size(-1, 100);
            this.uiPanelList.Name = "uiPanelList";
            this.uiPanelList.Size = new System.Drawing.Size(1010, 239);
            this.uiPanelList.TabIndex = 4;
            this.uiPanelList.Text = "±¤°íÇöÈ²";
            // 
            // uiPanelListContainer
            // 
            this.uiPanelListContainer.BackColor = System.Drawing.SystemColors.Window;
            this.uiPanelListContainer.Controls.Add(this.grdExScheduleList);
            this.uiPanelListContainer.Location = new System.Drawing.Point(1, 23);
            this.uiPanelListContainer.Name = "uiPanelListContainer";
            this.uiPanelListContainer.Size = new System.Drawing.Size(1008, 215);
            this.uiPanelListContainer.TabIndex = 0;
            // 
            // grdExScheduleList
            // 
            this.grdExScheduleList.AlternatingColors = true;
            this.grdExScheduleList.BorderStyle = Janus.Windows.GridEX.BorderStyle.None;
            this.grdExScheduleList.DataSource = this.dvSchedule;
            grdExScheduleList_DesignTimeLayout_Reference_0.Instance = ((object)(resources.GetObject("grdExScheduleList_DesignTimeLayout_Reference_0.Instance")));
            grdExScheduleList_DesignTimeLayout_Reference_1.Instance = ((object)(resources.GetObject("grdExScheduleList_DesignTimeLayout_Reference_1.Instance")));
            grdExScheduleList_DesignTimeLayout_Reference_2.Instance = ((object)(resources.GetObject("grdExScheduleList_DesignTimeLayout_Reference_2.Instance")));
            grdExScheduleList_DesignTimeLayout_Reference_3.Instance = ((object)(resources.GetObject("grdExScheduleList_DesignTimeLayout_Reference_3.Instance")));
            grdExScheduleList_DesignTimeLayout_Reference_4.Instance = ((object)(resources.GetObject("grdExScheduleList_DesignTimeLayout_Reference_4.Instance")));
            grdExScheduleList_DesignTimeLayout.LayoutReferences.AddRange(new Janus.Windows.Common.Layouts.JanusLayoutReference[] {
            grdExScheduleList_DesignTimeLayout_Reference_0,
            grdExScheduleList_DesignTimeLayout_Reference_1,
            grdExScheduleList_DesignTimeLayout_Reference_2,
            grdExScheduleList_DesignTimeLayout_Reference_3,
            grdExScheduleList_DesignTimeLayout_Reference_4});
            grdExScheduleList_DesignTimeLayout.LayoutString = resources.GetString("grdExScheduleList_DesignTimeLayout.LayoutString");
            this.grdExScheduleList.DesignTimeLayout = grdExScheduleList_DesignTimeLayout;
            this.grdExScheduleList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdExScheduleList.EditorsControlStyle.ButtonAppearance = Janus.Windows.GridEX.ButtonAppearance.Regular;
            this.grdExScheduleList.EmptyRows = true;
            this.grdExScheduleList.FocusCellFormatStyle.BackColor = System.Drawing.Color.CornflowerBlue;
            this.grdExScheduleList.FocusCellFormatStyle.FontBold = Janus.Windows.GridEX.TriState.True;
            this.grdExScheduleList.FocusStyle = Janus.Windows.GridEX.FocusStyle.None;
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
            this.grdExScheduleList.SelectedFormatStyle.BackColor = System.Drawing.Color.CornflowerBlue;
            this.grdExScheduleList.SelectedFormatStyle.FontBold = Janus.Windows.GridEX.TriState.True;
            this.grdExScheduleList.SelectedFormatStyle.ForeColor = System.Drawing.Color.Empty;
            this.grdExScheduleList.SelectedInactiveFormatStyle.BackColor = System.Drawing.Color.Gold;
            this.grdExScheduleList.Size = new System.Drawing.Size(1008, 215);
            this.grdExScheduleList.TabIndex = 12;
            this.grdExScheduleList.ThemedAreas = ((Janus.Windows.GridEX.ThemedArea)(((((((((Janus.Windows.GridEX.ThemedArea.ScrollBars | Janus.Windows.GridEX.ThemedArea.EditControls) 
            | Janus.Windows.GridEX.ThemedArea.Headers) 
            | Janus.Windows.GridEX.ThemedArea.GroupByBox) 
            | Janus.Windows.GridEX.ThemedArea.GroupRows) 
            | Janus.Windows.GridEX.ThemedArea.ControlBorder) 
            | Janus.Windows.GridEX.ThemedArea.Cards) 
            | Janus.Windows.GridEX.ThemedArea.Gridlines) 
            | Janus.Windows.GridEX.ThemedArea.CheckBoxes)));
            this.grdExScheduleList.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            this.grdExScheduleList.SelectionChanged += new System.EventHandler(this.OnGrdRowChanged);
            // 
            // dvSchedule
            // 
            this.dvSchedule.Table = this.ratioDs.ChoiceAdSchedule;
            // 
            // ratioDs
            // 
            this.ratioDs.DataSetName = "RatioDs";
            this.ratioDs.Locale = new System.Globalization.CultureInfo("en-US");
            this.ratioDs.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // uiPanelGroup
            // 
            this.uiPanelGroup.CaptionHeight = 20;
            this.uiPanelGroup.CaptionVisible = Janus.Windows.UI.InheritableBoolean.False;
            this.uiPanelGroup.GroupStyle = Janus.Windows.UI.Dock.PanelGroupStyle.VerticalTiles;
            this.uiPanelGroup.Location = new System.Drawing.Point(0, 327);
            this.uiPanelGroup.Name = "uiPanelGroup";
            this.uiPanelGroup.Size = new System.Drawing.Size(1010, 350);
            this.uiPanelGroup.TabIndex = 5;
            this.uiPanelGroup.Text = "¾È³ç";
            // 
            // uiPanelGroup0
            // 
            this.uiPanelGroup0.AllowResize = Janus.Windows.UI.InheritableBoolean.True;
            this.uiPanelGroup0.CaptionStyle = Janus.Windows.UI.Dock.PanelCaptionStyle.Light;
            this.uiPanelGroup0.GroupStyle = Janus.Windows.UI.Dock.PanelGroupStyle.VerticalTiles;
            this.uiPanelGroup0.Location = new System.Drawing.Point(0, 0);
            this.uiPanelGroup0.Name = "uiPanelGroup0";
            this.uiPanelGroup0.Size = new System.Drawing.Size(1010, 350);
            this.uiPanelGroup0.TabIndex = 4;
            this.uiPanelGroup0.Text = "¤±¤±";
            // 
            // uiPanelGroup1
            // 
            this.uiPanelGroup1.AllowResize = Janus.Windows.UI.InheritableBoolean.True;
            this.uiPanelGroup1.CaptionHeight = 20;
            this.uiPanelGroup1.CaptionStyle = Janus.Windows.UI.Dock.PanelCaptionStyle.Light;
            this.uiPanelGroup1.CaptionVisible = Janus.Windows.UI.InheritableBoolean.True;
            this.uiPanelGroup1.InnerContainer = this.uiPanelGroup1Container;
            this.uiPanelGroup1.Location = new System.Drawing.Point(0, 22);
            this.uiPanelGroup1.Name = "uiPanelGroup1";
            this.uiPanelGroup1.Size = new System.Drawing.Size(264, 328);
            this.uiPanelGroup1.TabIndex = 4;
            this.uiPanelGroup1.Text = "±×·ì1";
            // 
            // uiPanelGroup1Container
            // 
            this.uiPanelGroup1Container.Controls.Add(this.panMenuSchedule);
            this.uiPanelGroup1Container.Location = new System.Drawing.Point(1, 20);
            this.uiPanelGroup1Container.Name = "uiPanelGroup1Container";
            this.uiPanelGroup1Container.Size = new System.Drawing.Size(262, 307);
            this.uiPanelGroup1Container.TabIndex = 0;
            // 
            // panMenuSchedule
            // 
            this.panMenuSchedule.BackColor = System.Drawing.SystemColors.Window;
            this.panMenuSchedule.Controls.Add(this.panel2);
            this.panMenuSchedule.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panMenuSchedule.Location = new System.Drawing.Point(0, 0);
            this.panMenuSchedule.Name = "panMenuSchedule";
            this.panMenuSchedule.Size = new System.Drawing.Size(262, 307);
            this.panMenuSchedule.TabIndex = 6;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.SystemColors.Window;
            this.panel2.Controls.Add(this.panel3);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(262, 307);
            this.panel2.TabIndex = 4;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.grdExGroup1);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(0, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(262, 307);
            this.panel3.TabIndex = 23;
            // 
            // grdExGroup1
            // 
            this.grdExGroup1.AlternatingColors = true;
            this.grdExGroup1.BorderStyle = Janus.Windows.GridEX.BorderStyle.None;
            this.grdExGroup1.ColumnHeaders = Janus.Windows.GridEX.InheritableBoolean.False;
            this.grdExGroup1.DataSource = this.dvMenu;
            grdExGroup1_DesignTimeLayout.LayoutString = resources.GetString("grdExGroup1_DesignTimeLayout.LayoutString");
            this.grdExGroup1.DesignTimeLayout = grdExGroup1_DesignTimeLayout;
            this.grdExGroup1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdExGroup1.EditorsControlStyle.ButtonAppearance = Janus.Windows.GridEX.ButtonAppearance.Regular;
            this.grdExGroup1.EmptyRows = true;
            this.grdExGroup1.FocusCellFormatStyle.BackColor = System.Drawing.SystemColors.Highlight;
            this.grdExGroup1.FocusStyle = Janus.Windows.GridEX.FocusStyle.None;
            this.grdExGroup1.FrozenColumns = 2;
            this.grdExGroup1.GridLineColor = System.Drawing.Color.Silver;
            this.grdExGroup1.GridLines = Janus.Windows.GridEX.GridLines.Vertical;
            this.grdExGroup1.GridLineStyle = Janus.Windows.GridEX.GridLineStyle.Solid;
            this.grdExGroup1.GroupByBoxVisible = false;
            this.grdExGroup1.Location = new System.Drawing.Point(0, 0);
            this.grdExGroup1.Name = "grdExGroup1";
            this.grdExGroup1.ScrollBars = Janus.Windows.GridEX.ScrollBars.Vertical;
            this.grdExGroup1.SelectedInactiveFormatStyle.BackColor = System.Drawing.Color.DarkOrange;
            this.grdExGroup1.Size = new System.Drawing.Size(262, 307);
            this.grdExGroup1.TabIndex = 16;
            this.grdExGroup1.ThemedAreas = ((Janus.Windows.GridEX.ThemedArea)(((((((((Janus.Windows.GridEX.ThemedArea.ScrollBars | Janus.Windows.GridEX.ThemedArea.EditControls) 
            | Janus.Windows.GridEX.ThemedArea.Headers) 
            | Janus.Windows.GridEX.ThemedArea.GroupByBox) 
            | Janus.Windows.GridEX.ThemedArea.GroupRows) 
            | Janus.Windows.GridEX.ThemedArea.ControlBorder) 
            | Janus.Windows.GridEX.ThemedArea.Cards) 
            | Janus.Windows.GridEX.ThemedArea.Gridlines) 
            | Janus.Windows.GridEX.ThemedArea.CheckBoxes)));
            this.grdExGroup1.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            // 
            // dvMenu
            // 
            this.dvMenu.Table = this.ratioDs.Group1;
            // 
            // uiPanelGroup2
            // 
            this.uiPanelGroup2.AllowResize = Janus.Windows.UI.InheritableBoolean.True;
            this.uiPanelGroup2.CaptionHeight = 20;
            this.uiPanelGroup2.CaptionStyle = Janus.Windows.UI.Dock.PanelCaptionStyle.Light;
            this.uiPanelGroup2.CaptionVisible = Janus.Windows.UI.InheritableBoolean.True;
            this.uiPanelGroup2.InnerContainer = this.uiPanelGroup2Container;
            this.uiPanelGroup2.Location = new System.Drawing.Point(265, 22);
            this.uiPanelGroup2.Name = "uiPanelGroup2";
            this.uiPanelGroup2.Size = new System.Drawing.Size(255, 328);
            this.uiPanelGroup2.TabIndex = 4;
            this.uiPanelGroup2.Text = "±×·ì2";
            // 
            // uiPanelGroup2Container
            // 
            this.uiPanelGroup2Container.Controls.Add(this.panel1);
            this.uiPanelGroup2Container.Location = new System.Drawing.Point(1, 20);
            this.uiPanelGroup2Container.Name = "uiPanelGroup2Container";
            this.uiPanelGroup2Container.Size = new System.Drawing.Size(253, 307);
            this.uiPanelGroup2Container.TabIndex = 0;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.Window;
            this.panel1.Controls.Add(this.panel4);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(253, 307);
            this.panel1.TabIndex = 7;
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.grdExGroup2);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel4.Location = new System.Drawing.Point(0, 0);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(253, 307);
            this.panel4.TabIndex = 24;
            // 
            // grdExGroup2
            // 
            this.grdExGroup2.AlternatingColors = true;
            this.grdExGroup2.BorderStyle = Janus.Windows.GridEX.BorderStyle.None;
            this.grdExGroup2.ColumnHeaders = Janus.Windows.GridEX.InheritableBoolean.False;
            this.grdExGroup2.DataSource = this.dvMenu2;
            grdExGroup2_DesignTimeLayout.LayoutString = resources.GetString("grdExGroup2_DesignTimeLayout.LayoutString");
            this.grdExGroup2.DesignTimeLayout = grdExGroup2_DesignTimeLayout;
            this.grdExGroup2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdExGroup2.EditorsControlStyle.ButtonAppearance = Janus.Windows.GridEX.ButtonAppearance.Regular;
            this.grdExGroup2.EmptyRows = true;
            this.grdExGroup2.FocusCellFormatStyle.BackColor = System.Drawing.SystemColors.Highlight;
            this.grdExGroup2.FocusStyle = Janus.Windows.GridEX.FocusStyle.None;
            this.grdExGroup2.FrozenColumns = 2;
            this.grdExGroup2.GridLineColor = System.Drawing.Color.Silver;
            this.grdExGroup2.GridLines = Janus.Windows.GridEX.GridLines.RowOutline;
            this.grdExGroup2.GridLineStyle = Janus.Windows.GridEX.GridLineStyle.Solid;
            this.grdExGroup2.GroupByBoxVisible = false;
            this.grdExGroup2.Location = new System.Drawing.Point(0, 0);
            this.grdExGroup2.Name = "grdExGroup2";
            this.grdExGroup2.ScrollBars = Janus.Windows.GridEX.ScrollBars.Vertical;
            this.grdExGroup2.SelectedInactiveFormatStyle.BackColor = System.Drawing.Color.DarkOrange;
            this.grdExGroup2.Size = new System.Drawing.Size(253, 307);
            this.grdExGroup2.TabIndex = 17;
            this.grdExGroup2.ThemedAreas = ((Janus.Windows.GridEX.ThemedArea)(((((((((Janus.Windows.GridEX.ThemedArea.ScrollBars | Janus.Windows.GridEX.ThemedArea.EditControls) 
            | Janus.Windows.GridEX.ThemedArea.Headers) 
            | Janus.Windows.GridEX.ThemedArea.GroupByBox) 
            | Janus.Windows.GridEX.ThemedArea.GroupRows) 
            | Janus.Windows.GridEX.ThemedArea.ControlBorder) 
            | Janus.Windows.GridEX.ThemedArea.Cards) 
            | Janus.Windows.GridEX.ThemedArea.Gridlines) 
            | Janus.Windows.GridEX.ThemedArea.CheckBoxes)));
            this.grdExGroup2.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            // 
            // dvMenu2
            // 
            this.dvMenu2.Table = this.ratioDs.Group2;
            // 
            // uiPanelGroup3
            // 
            this.uiPanelGroup3.CaptionHeight = 20;
            this.uiPanelGroup3.CaptionStyle = Janus.Windows.UI.Dock.PanelCaptionStyle.Light;
            this.uiPanelGroup3.CaptionVisible = Janus.Windows.UI.InheritableBoolean.True;
            this.uiPanelGroup3.InnerContainer = this.uiPanelGroup3Container;
            this.uiPanelGroup3.Location = new System.Drawing.Point(521, 22);
            this.uiPanelGroup3.Name = "uiPanelGroup3";
            this.uiPanelGroup3.Size = new System.Drawing.Size(256, 328);
            this.uiPanelGroup3.TabIndex = 4;
            this.uiPanelGroup3.Text = "±×·ì3";
            // 
            // uiPanelGroup3Container
            // 
            this.uiPanelGroup3Container.Controls.Add(this.panel7);
            this.uiPanelGroup3Container.Location = new System.Drawing.Point(1, 20);
            this.uiPanelGroup3Container.Name = "uiPanelGroup3Container";
            this.uiPanelGroup3Container.Size = new System.Drawing.Size(254, 307);
            this.uiPanelGroup3Container.TabIndex = 0;
            // 
            // panel7
            // 
            this.panel7.Controls.Add(this.grdExGroup3);
            this.panel7.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel7.Location = new System.Drawing.Point(0, 0);
            this.panel7.Name = "panel7";
            this.panel7.Size = new System.Drawing.Size(254, 307);
            this.panel7.TabIndex = 26;
            // 
            // grdExGroup3
            // 
            this.grdExGroup3.AlternatingColors = true;
            this.grdExGroup3.BorderStyle = Janus.Windows.GridEX.BorderStyle.None;
            this.grdExGroup3.ColumnHeaders = Janus.Windows.GridEX.InheritableBoolean.False;
            this.grdExGroup3.DataSource = this.dvMenu3;
            grdExGroup3_DesignTimeLayout.LayoutString = resources.GetString("grdExGroup3_DesignTimeLayout.LayoutString");
            this.grdExGroup3.DesignTimeLayout = grdExGroup3_DesignTimeLayout;
            this.grdExGroup3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdExGroup3.EditorsControlStyle.ButtonAppearance = Janus.Windows.GridEX.ButtonAppearance.Regular;
            this.grdExGroup3.EmptyRows = true;
            this.grdExGroup3.FocusCellFormatStyle.BackColor = System.Drawing.SystemColors.Highlight;
            this.grdExGroup3.FocusStyle = Janus.Windows.GridEX.FocusStyle.None;
            this.grdExGroup3.FrozenColumns = 2;
            this.grdExGroup3.GridLineColor = System.Drawing.Color.Silver;
            this.grdExGroup3.GridLines = Janus.Windows.GridEX.GridLines.Vertical;
            this.grdExGroup3.GridLineStyle = Janus.Windows.GridEX.GridLineStyle.Solid;
            this.grdExGroup3.GroupByBoxVisible = false;
            this.grdExGroup3.Location = new System.Drawing.Point(0, 0);
            this.grdExGroup3.Name = "grdExGroup3";
            this.grdExGroup3.ScrollBars = Janus.Windows.GridEX.ScrollBars.Vertical;
            this.grdExGroup3.SelectedInactiveFormatStyle.BackColor = System.Drawing.Color.DarkOrange;
            this.grdExGroup3.Size = new System.Drawing.Size(254, 307);
            this.grdExGroup3.TabIndex = 17;
            this.grdExGroup3.ThemedAreas = ((Janus.Windows.GridEX.ThemedArea)(((((((((Janus.Windows.GridEX.ThemedArea.ScrollBars | Janus.Windows.GridEX.ThemedArea.EditControls) 
            | Janus.Windows.GridEX.ThemedArea.Headers) 
            | Janus.Windows.GridEX.ThemedArea.GroupByBox) 
            | Janus.Windows.GridEX.ThemedArea.GroupRows) 
            | Janus.Windows.GridEX.ThemedArea.ControlBorder) 
            | Janus.Windows.GridEX.ThemedArea.Cards) 
            | Janus.Windows.GridEX.ThemedArea.Gridlines) 
            | Janus.Windows.GridEX.ThemedArea.CheckBoxes)));
            this.grdExGroup3.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            // 
            // dvMenu3
            // 
            this.dvMenu3.Table = this.ratioDs.Group3;
            // 
            // uiPanelGroup4
            // 
            this.uiPanelGroup4.CaptionHeight = 20;
            this.uiPanelGroup4.CaptionStyle = Janus.Windows.UI.Dock.PanelCaptionStyle.Light;
            this.uiPanelGroup4.CaptionVisible = Janus.Windows.UI.InheritableBoolean.True;
            this.uiPanelGroup4.InnerContainer = this.uiPanelGroup4Container;
            this.uiPanelGroup4.Location = new System.Drawing.Point(778, 22);
            this.uiPanelGroup4.Name = "uiPanelGroup4";
            this.uiPanelGroup4.Size = new System.Drawing.Size(232, 328);
            this.uiPanelGroup4.TabIndex = 4;
            this.uiPanelGroup4.Text = "±×·ì4";
            // 
            // uiPanelGroup4Container
            // 
            this.uiPanelGroup4Container.Controls.Add(this.panel9);
            this.uiPanelGroup4Container.Location = new System.Drawing.Point(1, 20);
            this.uiPanelGroup4Container.Name = "uiPanelGroup4Container";
            this.uiPanelGroup4Container.Size = new System.Drawing.Size(230, 307);
            this.uiPanelGroup4Container.TabIndex = 0;
            // 
            // panel9
            // 
            this.panel9.Controls.Add(this.grdExGroup4);
            this.panel9.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel9.Location = new System.Drawing.Point(0, 0);
            this.panel9.Name = "panel9";
            this.panel9.Size = new System.Drawing.Size(230, 307);
            this.panel9.TabIndex = 26;
            // 
            // grdExGroup4
            // 
            this.grdExGroup4.AlternatingColors = true;
            this.grdExGroup4.BorderStyle = Janus.Windows.GridEX.BorderStyle.None;
            this.grdExGroup4.ColumnHeaders = Janus.Windows.GridEX.InheritableBoolean.False;
            this.grdExGroup4.DataSource = this.dvMenu4;
            grdExGroup4_DesignTimeLayout.LayoutString = resources.GetString("grdExGroup4_DesignTimeLayout.LayoutString");
            this.grdExGroup4.DesignTimeLayout = grdExGroup4_DesignTimeLayout;
            this.grdExGroup4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdExGroup4.EditorsControlStyle.ButtonAppearance = Janus.Windows.GridEX.ButtonAppearance.Regular;
            this.grdExGroup4.EmptyRows = true;
            this.grdExGroup4.FocusCellFormatStyle.BackColor = System.Drawing.SystemColors.Highlight;
            this.grdExGroup4.FocusStyle = Janus.Windows.GridEX.FocusStyle.None;
            this.grdExGroup4.FrozenColumns = 2;
            this.grdExGroup4.GridLineColor = System.Drawing.Color.Silver;
            this.grdExGroup4.GridLines = Janus.Windows.GridEX.GridLines.Vertical;
            this.grdExGroup4.GridLineStyle = Janus.Windows.GridEX.GridLineStyle.Solid;
            this.grdExGroup4.GroupByBoxVisible = false;
            this.grdExGroup4.Location = new System.Drawing.Point(0, 0);
            this.grdExGroup4.Name = "grdExGroup4";
            this.grdExGroup4.ScrollBars = Janus.Windows.GridEX.ScrollBars.Vertical;
            this.grdExGroup4.SelectedInactiveFormatStyle.BackColor = System.Drawing.Color.DarkOrange;
            this.grdExGroup4.Size = new System.Drawing.Size(230, 307);
            this.grdExGroup4.TabIndex = 17;
            this.grdExGroup4.ThemedAreas = ((Janus.Windows.GridEX.ThemedArea)(((((((((Janus.Windows.GridEX.ThemedArea.ScrollBars | Janus.Windows.GridEX.ThemedArea.EditControls) 
            | Janus.Windows.GridEX.ThemedArea.Headers) 
            | Janus.Windows.GridEX.ThemedArea.GroupByBox) 
            | Janus.Windows.GridEX.ThemedArea.GroupRows) 
            | Janus.Windows.GridEX.ThemedArea.ControlBorder) 
            | Janus.Windows.GridEX.ThemedArea.Cards) 
            | Janus.Windows.GridEX.ThemedArea.Gridlines) 
            | Janus.Windows.GridEX.ThemedArea.CheckBoxes)));
            this.grdExGroup4.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            // 
            // dvMenu4
            // 
            this.dvMenu4.Table = this.ratioDs.Group4;
            // 
            // dvMenu5
            // 
            this.dvMenu5.Table = this.ratioDs.Group5;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(100, 23);
            this.label1.TabIndex = 0;
            // 
            // editBox1
            // 
            this.editBox1.Location = new System.Drawing.Point(0, 0);
            this.editBox1.Name = "editBox1";
            this.editBox1.Size = new System.Drawing.Size(0, 21);
            this.editBox1.TabIndex = 0;
            this.editBox1.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
            // 
            // RatioControl
            // 
            this.Controls.Add(this.uiPanelChoiceAdSchedule);
            this.Font = global::AdManagerClient.Properties.Settings.Default.Font1;
            this.Name = "RatioControl";
            this.Size = new System.Drawing.Size(1010, 677);
            this.Load += new System.EventHandler(this.UserControl_Load);
            ((System.ComponentModel.ISupportInitialize)(this.uiPM)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelChoiceAdSchedule)).EndInit();
            this.uiPanelChoiceAdSchedule.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelSearch)).EndInit();
            this.uiPanelSearch.ResumeLayout(false);
            this.uiPanelSearchContainer.ResumeLayout(false);
            this.pnlSearch.ResumeLayout(false);
            this.pnlSearch.PerformLayout();
            this.pnlSearchBtn.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelList)).EndInit();
            this.uiPanelList.ResumeLayout(false);
            this.uiPanelListContainer.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdExScheduleList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dvSchedule)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ratioDs)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelGroup)).EndInit();
            this.uiPanelGroup.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelGroup0)).EndInit();
            this.uiPanelGroup0.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelGroup1)).EndInit();
            this.uiPanelGroup1.ResumeLayout(false);
            this.uiPanelGroup1Container.ResumeLayout(false);
            this.panMenuSchedule.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdExGroup1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dvMenu)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelGroup2)).EndInit();
            this.uiPanelGroup2.ResumeLayout(false);
            this.uiPanelGroup2Container.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdExGroup2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dvMenu2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelGroup3)).EndInit();
            this.uiPanelGroup3.ResumeLayout(false);
            this.uiPanelGroup3Container.ResumeLayout(false);
            this.panel7.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdExGroup3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dvMenu3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelGroup4)).EndInit();
            this.uiPanelGroup4.ResumeLayout(false);
            this.uiPanelGroup4Container.ResumeLayout(false);
            this.panel9.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdExGroup4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dvMenu4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dvMenu5)).EndInit();
            this.ResumeLayout(false);

        }
		#endregion

		#region ÄÁÆ®·Ñ ·Îµå
		private void UserControl_Load(object sender, System.EventArgs e)
		{
			// µ¥ÀÌÅÍ°ü¸®¿ë °´Ã¼»ý¼º
			dt = ((DataView)grdExScheduleList.DataSource).Table;  
			cm = (CurrencyManager) this.BindingContext[grdExScheduleList.DataSource]; 

			dtMenu    = ((DataView)grdExGroup1.DataSource).Table;  
			cmMenu = (CurrencyManager) this.BindingContext[grdExGroup1.DataSource]; 

			dtMenu2    = ((DataView)grdExGroup2.DataSource).Table;  
			cmMenu2 = (CurrencyManager) this.BindingContext[grdExGroup2.DataSource]; 

			dtMenu3    = ((DataView)grdExGroup3.DataSource).Table;  
			cmMenu3 = (CurrencyManager) this.BindingContext[grdExGroup3.DataSource]; 
			
			dtMenu4    = ((DataView)grdExGroup4.DataSource).Table;  
			cmMenu4 = (CurrencyManager) this.BindingContext[grdExGroup4.DataSource]; 

            //dtMenu5    = ((DataView)grdExGroup5.DataSource).Table;  
            //cmMenu5 = (CurrencyManager) this.BindingContext[grdExGroup5.DataSource]; 
			
			// ÄÁÆ®·Ñ ÃÊ±âÈ­
			InitControl();	
		}

		#endregion

		#region ÄÁÆ®·Ñ ÃÊ±âÈ­
		private void InitControl()
		{
			ProgressStart();

			InitCombo();

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

			// Á¶È¸±ÇÇÑ °Ë»ç
			if(menu.CanRead(MenuCode))
			{
				canRead = true;
			}
			
			InitButton();
			ProgressStop();

			if(canRead) SearchScheduleChoiceAd();
		}

		private void InitCombo()
		{
			Init_MediaCode();
			Init_RapCode();
			Init_AgencyCode();
			Init_AdvertiserCode();
			InitCombo_Level();
		}

		private void Init_MediaCode()
		{
			// ¸ÅÃ¼¸¦ Á¶È¸ÇÑ´Ù.
			MediaCodeModel mediacodeModel = new MediaCodeModel();
			new MediaCodeManager(systemModel, commonModel).GetMediaCodeList(mediacodeModel);
			
			if (mediacodeModel.ResultCD.Equals("0000"))
			{
				// µ¥ÀÌÅÍ¼Â¿¡ ¼ÂÆÃ
				Utility.SetDataTable(ratioDs.Medias, mediacodeModel.MediaCodeDataSet);				
			}

			// °Ë»öÁ¶°ÇÀÇ ÄÞº¸
			this.cbSearchMedia.Items.Clear();
			
			// ÄÞº¸¹Ú½º¿¡ ¼ÂÆ®ÇÒ ÄÚµå¸ñ·ÏÀ» ´ãÀ» Item¹è¿­À» ¼±¾ð
			Janus.Windows.EditControls.UIComboBoxItem[] comboItems = new Janus.Windows.EditControls.UIComboBoxItem[mediacodeModel.ResultCnt + 1];

			comboItems[0] = new Janus.Windows.EditControls.UIComboBoxItem("¸ÅÃ¼¼±ÅÃ","00");
			
			for(int i=0;i<mediacodeModel.ResultCnt;i++)
			{
				DataRow row = ratioDs.Medias.Rows[i];

				string val = row["MediaCode"].ToString();
				string txt = row["MediaName"].ToString();
				comboItems[i+1] = new Janus.Windows.EditControls.UIComboBoxItem(txt,val);
			}
			// ÄÞº¸¿¡ ¼ÂÆ®
			this.cbSearchMedia.Items.AddRange(comboItems);
			this.cbSearchMedia.SelectedIndex = 0;

			Application.DoEvents();
		}

		private void Init_RapCode()
		{
            /*
			// ·¦À» Á¶È¸ÇÑ´Ù.
			MediaRapCodeModel mediarapcodeModel = new MediaRapCodeModel();
			new MediaRapCodeManager(systemModel, commonModel).GetMediaRapCodeList(mediarapcodeModel);
			
			if (mediarapcodeModel.ResultCD.Equals("0000"))
			{
				// µ¥ÀÌÅÍ¼Â¿¡ ¼ÂÆÃ
				Utility.SetDataTable(ratioDs.MediaRaps, mediarapcodeModel.MediaRapCodeDataSet);				
			}

			// °Ë»öÁ¶°ÇÀÇ ÄÞº¸
			this.cbSearchRap.Items.Clear();
			
			// ÄÞº¸¹Ú½º¿¡ ¼ÂÆ®ÇÒ ÄÚµå¸ñ·ÏÀ» ´ãÀ» Item¹è¿­À» ¼±¾ð
			Janus.Windows.EditControls.UIComboBoxItem[] comboItems = new Janus.Windows.EditControls.UIComboBoxItem[mediarapcodeModel.ResultCnt + 1];

			comboItems[0] = new Janus.Windows.EditControls.UIComboBoxItem("¹Ìµð¾î·¾¼±ÅÃ","00");
			
			for(int i=0;i<mediarapcodeModel.ResultCnt;i++)
			{
				DataRow row = ratioDs.MediaRaps.Rows[i];

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

            Utility.SetDataTable(ratioDs.MediaRaps, ds);
            // °Ë»öÁ¶°ÇÀÇ ÄÞº¸
            this.cbSearchRap.Items.Clear();
            // ÄÞº¸¹Ú½º¿¡ ¼ÂÆ®ÇÒ ÄÚµå¸ñ·ÏÀ» ´ãÀ» Item¹è¿­À» ¼±¾ð
            Janus.Windows.EditControls.UIComboBoxItem[] comboItems = new Janus.Windows.EditControls.UIComboBoxItem[2];

            comboItems[0] = new Janus.Windows.EditControls.UIComboBoxItem("¹Ìµð¾î·¾¼±ÅÃ", "00");

            for (int i = 0; i < 1; i++)
            {
                DataRow row = ratioDs.MediaRaps.Rows[i];

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
            //// ´ëÇà»ç¸¦ Á¶È¸ÇÑ´Ù.
            //AgencyCodeModel agencycodeModel = new AgencyCodeModel();
            //new AgencyCodeManager(systemModel, commonModel).GetAgencyCodeList(agencycodeModel);
			
            //if (agencycodeModel.ResultCD.Equals("0000"))
            //{
            //    // µ¥ÀÌÅÍ¼Â¿¡ ¼ÂÆÃ
            //    Utility.SetDataTable(ratioDs.Agencys, agencycodeModel.AgencyCodeDataSet);				
            //}

            //// °Ë»öÁ¶°ÇÀÇ ÄÞº¸
            //this.cbSearchAgency.Items.Clear();
			
            //// ÄÞº¸¹Ú½º¿¡ ¼ÂÆ®ÇÒ ÄÚµå¸ñ·ÏÀ» ´ãÀ» Item¹è¿­À» ¼±¾ð
            //Janus.Windows.EditControls.UIComboBoxItem[] comboItems = new Janus.Windows.EditControls.UIComboBoxItem[agencycodeModel.ResultCnt + 1];

            //comboItems[0] = new Janus.Windows.EditControls.UIComboBoxItem("´ëÇà»ç¼±ÅÃ","00");
			
            //for(int i=0;i<agencycodeModel.ResultCnt;i++)
            //{
            //    DataRow row = ratioDs.Agencys.Rows[i];

            //    string val = row["AgencyCode"].ToString();
            //    string txt = row["AgencyName"].ToString();
            //    comboItems[i+1] = new Janus.Windows.EditControls.UIComboBoxItem(txt,val);
            //}
			
            //// ÄÞº¸¿¡ ¼ÂÆ®
            //this.cbSearchAgency.Items.AddRange(comboItems);
            //this.cbSearchAgency.SelectedIndex = 0;

            //Application.DoEvents();
		}

		private void Init_AdvertiserCode()
		{
            //// ±¤°íÁÖ¸¦ Á¶È¸ÇÑ´Ù.
            //ClientModel clientModel = new ClientModel();
            //new ClientManager(systemModel, commonModel).GetAdvertiserList(clientModel);
			
            //if (clientModel.ResultCD.Equals("0000"))
            //{
            //    // µ¥ÀÌÅÍ¼Â¿¡ ¼ÂÆÃ
            //    Utility.SetDataTable(ratioDs.Advertisers, clientModel.ClientDataSet);				
            //}

            //// °Ë»öÁ¶°ÇÀÇ ÄÞº¸
            //this.cbSearchAdvertiser.Items.Clear();
			
            //// ÄÞº¸¹Ú½º¿¡ ¼ÂÆ®ÇÒ ÄÚµå¸ñ·ÏÀ» ´ãÀ» Item¹è¿­À» ¼±¾ð
            //Janus.Windows.EditControls.UIComboBoxItem[] comboItems = new Janus.Windows.EditControls.UIComboBoxItem[clientModel.ResultCnt + 1];

            //comboItems[0] = new Janus.Windows.EditControls.UIComboBoxItem("±¤°íÁÖ¼±ÅÃ","00");
			
            //for(int i=0;i<clientModel.ResultCnt;i++)
            //{
            //    DataRow row = ratioDs.Advertisers.Rows[i];

            //    string val = row["AdvertiserCode"].ToString();
            //    string txt = row["AdvertiserName"].ToString();
            //    comboItems[i+1] = new Janus.Windows.EditControls.UIComboBoxItem(txt,val);
            //}
			
            //// ÄÞº¸¿¡ ¼ÂÆ®
            //this.cbSearchAdvertiser.Items.AddRange(comboItems);
            //this.cbSearchAdvertiser.SelectedIndex = 0;

            //Application.DoEvents();
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
				for(int i=0;i < ratioDs.Medias.Rows.Count;i++)
				{
					DataRow row = ratioDs.Medias.Rows[i];					
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
			if(commonModel.UserLevel=="30")
			{
				cbSearchRap.SelectedValue = commonModel.RapCode;			
				cbSearchRap.ReadOnly = true;				
			}
			if(commonModel.UserLevel=="40")
			{
                //cbSearchAgency.SelectedValue = commonModel.AgencyCode;			
                //cbSearchAgency.ReadOnly = true;					
			}	
		

			Application.DoEvents();
		}

		private void InitButton()
		{
			if(canRead)   btnSearch.Enabled = true;

			grdExScheduleList.Focus();

			Application.DoEvents();
		}

		private void DisableButton()
		{
			btnSearch.Enabled	= false;
			btnAdd.Enabled		= true;
			//btnDelete.Enabled   = false;

//			btnAddMenu.Enabled       = false;
//			btnAddChannel.Enabled    = false;
//			btnDeleteMenu.Enabled    = false;
//			btnDeleteChannel.Enabled = false;

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
                if (IsNotLoading)
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
			SearchScheduleChoiceAd();
			InitButton();
			ProgressStop();
		}

		/// <summary>
		/// ºñÀ²Á¶Á¤
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnAdd_Click(object sender, System.EventArgs e)
		{
			DisableButton();	
			//  ÁöÁ¤±¤°í ´ë»ó¸ñ·Ï °Ë»öÃ¢ 
			Ratio_InsertForm pForm = new Ratio_InsertForm(this);

			// ÄÚµå¼ÂÆ®			
			pForm.keyMediaCode = keyMediaCode;
			pForm.keyItemNo	   = keyItemNo;

			pForm.ShowDialog();            
			pForm.Dispose();
			pForm = null;
			
			InitButton();			
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
				SearchScheduleChoiceAd();
			}
		}		

		#endregion

		#region Ã³¸®¸Þ¼Òµå

		/// <summary>
		/// ±¤°íÆÄÀÏ¸ñ·Ï Á¶È¸
		/// </summary>
		private void SearchScheduleChoiceAd()
		{
            IsSearching = true;

			StatusMessage("Æí¼ººñÀ² ÇöÈ²À» Á¶È¸ÇÕ´Ï´Ù.");

			ProgressStart();
			try
			{
				// ¸ðµç Ã¼Å©¹Ú½ºÀÇ Ã¼Å©¸¦ Ç¬´Ù.
				grdExScheduleList.UnCheckAllRecords(); 

                #region ±¤°í¸ñ·ÏÀ» ÀÐ¾î¿Â´Ù
				// µ¥ÀÌÅÍ¸ðµ¨ ÃÊ±âÈ­
				schChoiceAdModel.Init();

				// µ¥ÀÌÅÍ¸ðµ¨¿¡ Àü¼ÛÇÒ ³»¿ëÀ» ¼ÂÆ®ÇÑ´Ù.
				if(IsNewSearchKey)
				{
					schChoiceAdModel.SearchKey = "";
				}
				else
				{
					schChoiceAdModel.SearchKey  = ebSearchKey.Text;
				}

				schChoiceAdModel.AdType					= "10";
				schChoiceAdModel.SearchMediaCode		=  cbSearchMedia.SelectedItem.Value.ToString(); 
				schChoiceAdModel.SearchRapCode			=  cbSearchRap.SelectedItem.Value.ToString();     
                //schChoiceAdModel.SearchAgencyCode	    =  cbSearchAgency.SelectedItem.Value.ToString();  
				//schChoiceAdModel.SearchAdvertiserCode   =  cbSearchAdvertiser.SelectedItem.Value.ToString();  
				if(chkAdState_20.Checked) schChoiceAdModel.SearchchkAdState_20 = "Y";
				if(chkAdState_30.Checked) schChoiceAdModel.SearchchkAdState_30 = "Y";
				if(chkAdState_40.Checked) schChoiceAdModel.SearchchkAdState_40 = "Y";

				// ±¤°í¸ñ·ÏÁ¶È¸ ¼­ºñ½º¸¦ È£ÃâÇÑ´Ù.
				new SchChoiceAdManager(systemModel,commonModel).GetAdList10(schChoiceAdModel);

				if (schChoiceAdModel.ResultCD.Equals("0000"))
				{					
					Utility.SetDataTableFast(ratioDs.ChoiceAdSchedule, schChoiceAdModel.ScheduleDataSet);		
					StatusMessage(schChoiceAdModel.ResultCnt + "°ÇÀÇ ±¤°íÆÄÀÏ Á¤º¸°¡ Á¶È¸µÇ¾ú½À´Ï´Ù.");

					uiPanelList.Text = "±¤°í¸ñ·Ï : " + schChoiceAdModel.ResultCnt.ToString() + " °Ç";

					AddSchChoice();									
				}
                #endregion

                #region Æí¼º»óÅÂ ÀÐ¾î¿Â´Ù
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
						lbMsg.Text = "Æí¼º ÁøÇàÁßÀÔ´Ï´Ù.";
					}
					else if(keyAckState.Equals("20")) // ½ÂÀÎ»óÅÂ°¡ 20:Æí¼º½ÂÀÎ »óÅÂÀÌ¸é Æí¼ºÀÌ ºÒ°¡ÇÏ´Ù.
					{
						lbMsg.Text = "°Ë¼ö½ÂÀÎ ´ë±âÁßÀÔ´Ï´Ù.";
						canCreate = false;
						canDelete = false;

						MessageBox.Show("ÇöÀç Æí¼º½ÂÀÎÈÄ °Ë¼ö½ÂÀÎ ´ë±â»óÅÂÀÌ¹Ç·Î º¯°æÇÒ ¼ö ¾ø½À´Ï´Ù.", "Æí¼ººñÀ²Á¶Á¤",
							MessageBoxButtons.OK, MessageBoxIcon.Information);
						
					}
					else if(keyAckState.Equals("25")) // ½ÂÀÎ»óÅÂ°¡ 25:¹èÆ÷´ë±â½ÂÀÎ »óÅÂÀÌ¸é Æí¼ºÀÌ ºÒ°¡ÇÏ´Ù.
					{
						lbMsg.Text = "¹èÆ÷½ÂÀÎ ´ë±âÁßÀÔ´Ï´Ù.";
						canCreate = false;
						canDelete = false;

						MessageBox.Show("ÇöÀç °Ë¼ö½ÂÀÎ ÈÄ ¹èÆ÷½ÂÀÎ ´ë±â»óÅÂÀÌ¹Ç·Î º¯°æÇÒ ¼ö ¾ø½À´Ï´Ù.", "Æí¼ººñÀ²Á¶Á¤",
							MessageBoxButtons.OK, MessageBoxIcon.Information);
						
					}
					else if(keyAckState.Equals("30")) // ½ÂÀÎ»óÅÂ°¡ 30:¹èÆ÷½ÂÀÎ »óÅÂÀÌ¸é ½Å±ÔÆí¼ºÀÌ °¡´ÉÇÏ´Ù.
					{
						lbMsg.Text = "";
					}
				}
                #endregion

				// »ó¼¼Á¤º¸ Ç¥½Ã
				SetDetailText();

			}
			catch(FrameException fe)
			{
				FrameSystem.showMsgForm("Æí¼ººñÀ² ÇöÈ² Á¶È¸¿À·ù", new string[] {fe.ErrCode, fe.ResultMsg});
			}
			catch(Exception ex)
			{
				FrameSystem.showMsgForm("Æí¼ººñÀ² ÇöÈ² Á¶È¸¿À·ù",new string[] {"",ex.Message});
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
				if(row["ItemNo"].ToString().Equals(keyItemNo))
				{					
					cm.Position = rowIndex;
					break;								
				}					
				rowIndex++;
			}

			grdExScheduleList.EnsureVisible();
		
		}

	
		/// <summary>
		/// ±¤°íÆÄÀÏ »ó¼¼Á¤º¸ÀÇ ¼ÂÆ®
		/// </summary>
		private void SetDetailText()
		{
			int curRow = cm.Position;

			if(curRow >= 0 )
			{
				IsNotLoading = false;	// Á¶È¸Áß ´Ù½Ã Á¶È¸µÇ´Â °ÍÀ» ¹æÁöÇÔ.
				try
				{

					uiPanelGroup0.Text = "¢Â Æí¼ººñÀ² [ " + dt.Rows[curRow]["ItemNo"].ToString() + " ] " + dt.Rows[curRow]["ItemName"].ToString();		
					keyItemNo            = dt.Rows[curRow]["ItemNo"].ToString();
					keyMediaCode         = cbSearchMedia.SelectedItem.Value.ToString();
					//keyAdType            = dt.Rows[curRow]["AdType"].ToString();

					if(canCreate) btnAdd.Enabled    = true;
					LoadDetailGroup();					

				}
				finally
				{
					IsNotLoading = true;
				}
			}

			StatusMessage("ÁØºñ");
		}

        /// <summary>
        /// ±×·ìº° ¼³Á¤µ¥ÀÌÅÍ¸¦ ÀÐ¾î¿Â´Ù
        /// </summary>
		private void LoadDetailGroup()
		{
			// ÁöÁ¤¸Þ´º »ó¼¼Æí¼º Á¤º¸¸¦ Á¶È¸ÇÑ´Ù.
			try
			{
				ratioModel.Init();

				// µ¥ÀÌÅÍ¸ðµ¨¿¡ Àü¼ÛÇÒ ³»¿ëÀ» ¼ÂÆ®ÇÑ´Ù.
				ratioModel.ItemNo        = keyItemNo;

				// ±×·ì1
				new RatioManager(systemModel,commonModel).GetGroup1List(ratioModel);

                ratioDs.Group1.Clear();
				if (ratioModel.ResultCD.Equals("0000"))
				{					
					Utility.SetDataTableFast(ratioDs.Group1, ratioModel.Group1DataSet);		
				}

				int curRow = cmMenu.Position;
				if(curRow >= 0) uiPanelGroup1.Text = " Æí¼ººñÀ² [ " + dtMenu.Rows[curRow]["EntryRate"].ToString() + "£¥ ]";
				else            uiPanelGroup1.Text = "";


                // ±×·ì2
                new RatioManager(systemModel,commonModel).GetGroup2List(ratioModel);

                ratioDs.Group2.Clear();
				if (ratioModel.ResultCD.Equals("0000"))
				{					
					Utility.SetDataTableFast(ratioDs.Group2, ratioModel.Group2DataSet);		
				}

				int curRow2 = cmMenu2.Position;
				if(curRow2 >= 0)    uiPanelGroup2.Text = " Æí¼ººñÀ² [ " + dtMenu2.Rows[curRow2]["EntryRate"].ToString() + "£¥ ]";
				else                uiPanelGroup2.Text = "";

                // ±×·ì3
				new RatioManager(systemModel,commonModel).GetGroup3List(ratioModel);

                ratioDs.Group3.Clear();
				if (ratioModel.ResultCD.Equals("0000"))
				{					
					Utility.SetDataTableFast(ratioDs.Group3, ratioModel.Group3DataSet);		
				}

				int curRow3 = cmMenu3.Position;
                if(curRow3 >= 0)    uiPanelGroup3.Text = " Æí¼ººñÀ² [ " + dtMenu3.Rows[curRow3]["EntryRate"].ToString() + "£¥ ]";
                else                uiPanelGroup3.Text = "";

                // ±×·ì4
				new RatioManager(systemModel,commonModel).GetGroup4List(ratioModel);

                ratioDs.Group4.Clear();
				if (ratioModel.ResultCD.Equals("0000"))
				{					
					Utility.SetDataTableFast(ratioDs.Group4, ratioModel.Group4DataSet);		
				}

				int curRow4 = cmMenu4.Position;
                if(curRow4 >= 0)    uiPanelGroup4.Text = " Æí¼ººñÀ² [ " + dtMenu4.Rows[curRow4]["EntryRate"].ToString() + "£¥ ]";
                else                uiPanelGroup4.Text = "";


                //// ±×·ì5
                //ratioDs.Group5.Clear();
                //new RatioManager(systemModel,commonModel).GetGroup5List(ratioModel);

                //if (ratioModel.ResultCD.Equals("0000"))
                //{					
                //    Utility.SetDataTableFast(ratioDs.Group5, ratioModel.Group5DataSet);		
                //}

                //int curRow5 = cmMenu5.Position;
                //if(curRow5 >= 0)    uiPanelGroup5.Text = " Æí¼ººñÀ² [ " + dtMenu5.Rows[curRow5]["EntryRate"].ToString() + "£¥ ]";
                //else                uiPanelGroup5.Text = "";
			}
			catch(FrameException fe)
			{
				FrameSystem.showMsgForm("ÁöÁ¤±¤°í ±×·ì1ÀÇ Á¶È¸¿À·ù", new string[] {fe.ErrCode, fe.ResultMsg});
			}
			catch(Exception ex)
			{
				FrameSystem.showMsgForm("ÁöÁ¤±¤°í ±×·ì1ÀÇ Á¶È¸¿À·ù",new string[] {"",ex.Message});
			}			
		}
		

		private void ResetDetailText()
		{
			keyItemNo            = "";
			uiPanelGroup0.Text = "Æí¼º±¤°í";

			DisableButton();
		}
				
		/// <summary>
		/// »ó¼¼Á¤º¸ ¼öÁ¤°¡´ÉÄÉ
		/// </summary>
		private void ResetTextReadonly()
		{
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
			SearchScheduleChoiceAd();
		}

		public void ReloadMenuList()
		{
			LoadDetailGroup();
		}
		#endregion

	}
}
