// ===============================================================================
// AdFileWideControl for Charites Project
//
// AdFileWideControl.cs
//
// �������ϰ��� ������� �����մϴ�. 
//
// ===============================================================================
// Release history
// 2007.10.01 RH.Jung ��ž�������� ������ - ���� �������� �� ����
//                     CDN����Ȯ�ν� ���ϸ���Ʈ�Ǽ� �˻�
// 2007.12.18 RH.Jung CDN����Ȯ�ν� FTP������ DB���� �о ������
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
using System.Threading;
using System.Diagnostics;

using WinFramework.Misc;
using WinFramework.Base;
using WinFramework.Data;

using AdManagerModel;
using Excel = Microsoft.Office.Interop.Excel; // ���� ����
using System.Reflection;

using System.Net;
using System.IO;
using System.Text;

namespace AdManagerClient
{	
	/// <summary>
	/// �������ϰ��� ��Ʈ��
	/// </summary>
    public class AdFileWideControl : System.Windows.Forms.UserControl, IUserControl
	{
		#region �̺�Ʈ�ڵ鷯
		public event StatusEventHandler 			StatusEvent;			// �����̺�Ʈ �ڵ鷯
		public event ProgressEventHandler 			ProgressEvent;			// ó�����̺�Ʈ �ڵ鷯

		#endregion
			
		#region ��������� ��ü �� ����

		// �ý��� ���� : ȭ�����
		private SystemModel   systemModel   = FrameSystem.oSysModel;
		private CommonModel   commonModel   = FrameSystem.oComModel;
		private Logger        log           = FrameSystem.oLog;
		private MenuPower     menu          = FrameSystem.oMenu;

		// �޴��ڵ� : ������ �ʿ��� ȭ�鿡 �ʿ���
		public string        menuCode		= "";

		// ����� ������
		AdFileWideModel adFileWideModel  = new AdFileWideModel();	// �������ϸ�
		AdFileModel adFileModel  = new AdFileModel();	// �������ϸ�

		// ȭ��ó���� ����
		bool IsNewSearchKey		  = true;					// �˻����Է� ����
		CurrencyManager cmCount        = null;					// ������ �׸����� ���濡 ���� �����ͼ� ������ ���Ͽ�			
		DataTable       dtCount        = null;

		CurrencyManager cmFile       = null;					// ������ �׸����� ���濡 ���� �����ͼ� ������ ���Ͽ�			
		DataTable       dtFile        = null;


        bool IsSearching = false; // ��ȸ�� ��ȭ���� ������Ʈ �Ǵ� ���� ���� �ϱ����� 2011.11.29 JH.Park
		bool canRead			  = false;
		bool canUpdate			  = false;

		string keyMediaCode       = "";
		string keyFileState       = "";
		string keychkAdState_10   = "";
		string keychkAdState_20   = "";
		string keychkAdState_30   = "";
		string keychkAdState_40   = "";
		string keySearchKey       = "";
		string keyItemNo          = "";
		string keyFileStateName   = "";

		// FTP������
		private FtpManager	ftmCDN;
		private FtpManager	ftmTEST;

		// FTP���ε�����
		string FtpUploadHost;
		string FtpUploadPort;
		string FtpUploadID;
		string FtpUploadPW;

		// �����̵� 
		string FtpMovePath;
		string FtpMoveUseYn;

		// CDN��������
		string FtpCdnHost;
		string FtpCdnPort;
		string FtpCdnID;
		string FtpCdnPW;
		
		string	mCmsMasUrl	= "";
		string  mCmsMasQuery= "";

		private	const int	FILEMAX	= 1000;	// �ִ� ���ϸ���Ʈ �Ǽ� >= �������ϸ���Ʈ = Ȩ����Ǽ� + CDN�����Ϸ� �������� �Ǽ�
        private int FileListCnt = 0;
        private Janus.Windows.UI.Dock.UIPanelGroup uiPanelAdFile;
        private Janus.Windows.UI.Dock.UIPanel uiPanelSearch;
        private Janus.Windows.UI.Dock.UIPanelInnerContainer uiPanel1Container;
        private Panel pnlSearch;
        private Janus.Windows.EditControls.UICheckBox chkAdState_40;
        private Janus.Windows.EditControls.UICheckBox chkAdState_30;
        private Janus.Windows.EditControls.UICheckBox chkAdState_20;
        private Janus.Windows.EditControls.UICheckBox chkAdState_10;
        private Janus.Windows.EditControls.UIButton btnExcel;
        private Janus.Windows.GridEX.EditControls.EditBox ebSearchKey;
        private Label lbAdState;
        private Janus.Windows.EditControls.UIButton btnSearch;
        private Janus.Windows.EditControls.UIComboBox cbSearchMedia;
        private Janus.Windows.UI.Dock.UIPanelGroup uiPanelDetail;
        private Janus.Windows.UI.Dock.UIPanelGroup uiPanel3;
        private Janus.Windows.UI.Dock.UIPanel uiPanelState;
        private Janus.Windows.UI.Dock.UIPanelInnerContainer uiPanel4Container;
        private Janus.Windows.GridEX.GridEX grdExAdFileWideList;
        private Janus.Windows.UI.Dock.UIPanel uiPanelStateList;
        private Janus.Windows.UI.Dock.UIPanelInnerContainer uiPanel5Container;
        private Janus.Windows.GridEX.GridEX grdExFileCount;
        private Janus.Windows.UI.Dock.UIPanel uiPanelFiles;
        private Janus.Windows.UI.Dock.UIPanelInnerContainer uiPanel6Container;
        private Panel pnlUserDetail;
        private Janus.Windows.EditControls.UICheckBox chkTestCheck;
        private Label lbMsg;
        private Janus.Windows.EditControls.UICheckBox chkCMS;
        private Label lblMsg;
        private Janus.Windows.EditControls.UIButton btnCDNSync;
        private Label label1;
        private Janus.Windows.EditControls.UIButton btnSTBDelete;
        private Janus.Windows.EditControls.UIButton btnCDNPublish;
        private Janus.Windows.EditControls.UIButton btnChkComplete;
        private Janus.Windows.EditControls.UIButton btnChkCompleteCancel;
        private Janus.Windows.EditControls.UIButton btnCDNPublishCancel;
        private Janus.Windows.EditControls.UIButton btnSTBDeleteCancel;
        private Label lbFileListCount;
        private Janus.Windows.EditControls.UIButton btnCDNSyncCancel;		// �������ϸ���Ʈ �Ǽ�

		bool IsAllCheck = true;

		#endregion

        #region IUserControl ����
        /// <summary>
        /// �޴� �ڵ�-������ �ʿ��� ȭ�鿡 �ʿ���
        /// </summary>
        public string MenuCode
        {
            set { this.menuCode = value; }
            get { return this.menuCode; }
        }

        /// <summary>
        /// �θ���Ʈ�� ����
        /// </summary>
        /// <param name="control"></param>
        public void SetParent(Control control)
        {
            this.Parent = control;
        }
        /// <summary>
        /// DockStype����
        /// </summary>
        /// <param name="style"></param>
        public void SetDockStyle(DockStyle style)
        {
            this.Dock = style;
        }
        #endregion

		#region ȭ�� ������Ʈ, ������, �Ҹ���

        private Janus.Windows.UI.Dock.UIPanelManager uiPM;
        private Janus.Windows.GridEX.GridEX grdExAdStatusList;
        private System.Data.DataView dvFileCount;
        private System.Data.DataView dvAdFileWide;
        private AdManagerClient.AdFileWideDs adFileWideDs;
        private System.Data.DataView dsItemSchedule;
        private System.Windows.Forms.ToolTip toolTip1;
		private System.ComponentModel.IContainer components;

		public AdFileWideControl()
		{
			// �� ȣ���� Windows.Forms Form �����̳ʿ� �ʿ��մϴ�.
			InitializeComponent();

			

		}

		/// <summary> 
		/// ��� ���� ��� ���ҽ��� �����մϴ�.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if (ftmCDN != null)
				{
					ftmCDN.Close();
				}

				if (ftmTEST != null)
				{
					ftmTEST.Close();
				}
				
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}
		#endregion

		#region ���� ��� �����̳ʿ��� ������ �ڵ�
		/// <summary> 
		/// �����̳� ������ �ʿ��� �޼����Դϴ�. 
		/// �� �޼����� ������ �ڵ� ������� �������� ���ʽÿ�.
		/// </summary>
		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            Janus.Windows.GridEX.GridEXLayout grdExFileCount_DesignTimeLayout = new Janus.Windows.GridEX.GridEXLayout();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AdFileWideControl));
            Janus.Windows.GridEX.GridEXLayout grdExAdFileWideList_DesignTimeLayout = new Janus.Windows.GridEX.GridEXLayout();
            Janus.Windows.GridEX.GridEXLayout grdExAdStatusList_DesignTimeLayout = new Janus.Windows.GridEX.GridEXLayout();
            this.uiPM = new Janus.Windows.UI.Dock.UIPanelManager(this.components);
            this.uiPanelAdFile = new Janus.Windows.UI.Dock.UIPanelGroup();
            this.uiPanelSearch = new Janus.Windows.UI.Dock.UIPanel();
            this.uiPanel1Container = new Janus.Windows.UI.Dock.UIPanelInnerContainer();
            this.pnlSearch = new System.Windows.Forms.Panel();
            this.chkAdState_40 = new Janus.Windows.EditControls.UICheckBox();
            this.chkAdState_30 = new Janus.Windows.EditControls.UICheckBox();
            this.chkAdState_20 = new Janus.Windows.EditControls.UICheckBox();
            this.chkAdState_10 = new Janus.Windows.EditControls.UICheckBox();
            this.btnExcel = new Janus.Windows.EditControls.UIButton();
            this.ebSearchKey = new Janus.Windows.GridEX.EditControls.EditBox();
            this.lbAdState = new System.Windows.Forms.Label();
            this.btnSearch = new Janus.Windows.EditControls.UIButton();
            this.cbSearchMedia = new Janus.Windows.EditControls.UIComboBox();
            this.uiPanelDetail = new Janus.Windows.UI.Dock.UIPanelGroup();
            this.uiPanel3 = new Janus.Windows.UI.Dock.UIPanelGroup();
            this.uiPanelStateList = new Janus.Windows.UI.Dock.UIPanel();
            this.uiPanel5Container = new Janus.Windows.UI.Dock.UIPanelInnerContainer();
            this.grdExFileCount = new Janus.Windows.GridEX.GridEX();
            this.dvFileCount = new System.Data.DataView();
            this.adFileWideDs = new AdManagerClient.AdFileWideDs();
            this.uiPanelFiles = new Janus.Windows.UI.Dock.UIPanel();
            this.uiPanel6Container = new Janus.Windows.UI.Dock.UIPanelInnerContainer();
            this.pnlUserDetail = new System.Windows.Forms.Panel();
            this.chkTestCheck = new Janus.Windows.EditControls.UICheckBox();
            this.lbMsg = new System.Windows.Forms.Label();
            this.chkCMS = new Janus.Windows.EditControls.UICheckBox();
            this.lblMsg = new System.Windows.Forms.Label();
            this.btnCDNSync = new Janus.Windows.EditControls.UIButton();
            this.label1 = new System.Windows.Forms.Label();
            this.btnSTBDelete = new Janus.Windows.EditControls.UIButton();
            this.btnCDNPublish = new Janus.Windows.EditControls.UIButton();
            this.btnChkComplete = new Janus.Windows.EditControls.UIButton();
            this.btnChkCompleteCancel = new Janus.Windows.EditControls.UIButton();
            this.btnCDNPublishCancel = new Janus.Windows.EditControls.UIButton();
            this.btnSTBDeleteCancel = new Janus.Windows.EditControls.UIButton();
            this.lbFileListCount = new System.Windows.Forms.Label();
            this.btnCDNSyncCancel = new Janus.Windows.EditControls.UIButton();
            this.uiPanelState = new Janus.Windows.UI.Dock.UIPanel();
            this.uiPanel4Container = new Janus.Windows.UI.Dock.UIPanelInnerContainer();
            this.grdExAdFileWideList = new Janus.Windows.GridEX.GridEX();
            this.dvAdFileWide = new System.Data.DataView();
            this.grdExAdStatusList = new Janus.Windows.GridEX.GridEX();
            this.dsItemSchedule = new System.Data.DataView();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.uiPM)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelAdFile)).BeginInit();
            this.uiPanelAdFile.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelSearch)).BeginInit();
            this.uiPanelSearch.SuspendLayout();
            this.uiPanel1Container.SuspendLayout();
            this.pnlSearch.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelDetail)).BeginInit();
            this.uiPanelDetail.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanel3)).BeginInit();
            this.uiPanel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelStateList)).BeginInit();
            this.uiPanelStateList.SuspendLayout();
            this.uiPanel5Container.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdExFileCount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dvFileCount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.adFileWideDs)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelFiles)).BeginInit();
            this.uiPanelFiles.SuspendLayout();
            this.uiPanel6Container.SuspendLayout();
            this.pnlUserDetail.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelState)).BeginInit();
            this.uiPanelState.SuspendLayout();
            this.uiPanel4Container.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdExAdFileWideList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dvAdFileWide)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdExAdStatusList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dsItemSchedule)).BeginInit();
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
            this.uiPanelAdFile.Id = new System.Guid("4fef83bf-72f4-4e45-b8cc-6e7662e65f5b");
            this.uiPanelAdFile.StaticGroup = true;
            this.uiPanelSearch.Id = new System.Guid("e5d01963-b192-43b1-ac21-264a511751bf");
            this.uiPanelAdFile.Panels.Add(this.uiPanelSearch);
            this.uiPanelDetail.Id = new System.Guid("1f2d57b6-824c-4823-98d6-27f856dde265");
            this.uiPanelDetail.StaticGroup = true;
            this.uiPanel3.Id = new System.Guid("093eb809-a79f-4fdf-8ec4-3ac1a5e75da6");
            this.uiPanel3.StaticGroup = true;
            this.uiPanelStateList.Id = new System.Guid("97912ae8-3203-4b0d-af33-d0227dcf1905");
            this.uiPanel3.Panels.Add(this.uiPanelStateList);
            this.uiPanelFiles.Id = new System.Guid("4f19892b-156c-49aa-9f84-543d53945feb");
            this.uiPanel3.Panels.Add(this.uiPanelFiles);
            this.uiPanelDetail.Panels.Add(this.uiPanel3);
            this.uiPanelState.Id = new System.Guid("7ec558bc-8e38-4f64-90ba-07193906f73f");
            this.uiPanelDetail.Panels.Add(this.uiPanelState);
            this.uiPanelAdFile.Panels.Add(this.uiPanelDetail);
            this.uiPM.Panels.Add(this.uiPanelAdFile);
            // 
            // Design Time Panel Info:
            // 
            this.uiPM.BeginPanelInfo();
            this.uiPM.AddDockPanelInfo(new System.Guid("4fef83bf-72f4-4e45-b8cc-6e7662e65f5b"), Janus.Windows.UI.Dock.PanelGroupStyle.HorizontalTiles, Janus.Windows.UI.Dock.PanelDockStyle.Fill, true, new System.Drawing.Size(1010, 677), true);
            this.uiPM.AddDockPanelInfo(new System.Guid("e5d01963-b192-43b1-ac21-264a511751bf"), new System.Guid("4fef83bf-72f4-4e45-b8cc-6e7662e65f5b"), 41, true);
            this.uiPM.AddDockPanelInfo(new System.Guid("1f2d57b6-824c-4823-98d6-27f856dde265"), new System.Guid("4fef83bf-72f4-4e45-b8cc-6e7662e65f5b"), Janus.Windows.UI.Dock.PanelGroupStyle.VerticalTiles, true, 610, true);
            this.uiPM.AddDockPanelInfo(new System.Guid("093eb809-a79f-4fdf-8ec4-3ac1a5e75da6"), new System.Guid("1f2d57b6-824c-4823-98d6-27f856dde265"), Janus.Windows.UI.Dock.PanelGroupStyle.HorizontalTiles, true, 234, true);
            this.uiPM.AddDockPanelInfo(new System.Guid("97912ae8-3203-4b0d-af33-d0227dcf1905"), new System.Guid("093eb809-a79f-4fdf-8ec4-3ac1a5e75da6"), 281, true);
            this.uiPM.AddDockPanelInfo(new System.Guid("4f19892b-156c-49aa-9f84-543d53945feb"), new System.Guid("093eb809-a79f-4fdf-8ec4-3ac1a5e75da6"), 281, true);
            this.uiPM.AddDockPanelInfo(new System.Guid("7ec558bc-8e38-4f64-90ba-07193906f73f"), new System.Guid("1f2d57b6-824c-4823-98d6-27f856dde265"), 772, true);
            this.uiPM.AddFloatingPanelInfo(new System.Guid("b162c53a-7940-47cd-a0ed-9762c59c50d8"), Janus.Windows.UI.Dock.PanelGroupStyle.HorizontalTiles, true, new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
            this.uiPM.AddFloatingPanelInfo(new System.Guid("8e3140b9-bfcb-43af-b84e-2ee6cddf51dc"), new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
            this.uiPM.AddFloatingPanelInfo(new System.Guid("b2973a57-d3e1-4d80-9387-7a6dc3332542"), new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
            this.uiPM.AddFloatingPanelInfo(new System.Guid("afb3b7d0-aa79-443c-a1e8-3baba38f7a4d"), new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
            this.uiPM.AddFloatingPanelInfo(new System.Guid("94ede22a-71ca-4b5a-9bce-a31e2977a4b0"), Janus.Windows.UI.Dock.PanelGroupStyle.VerticalTiles, true, new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
            this.uiPM.AddFloatingPanelInfo(new System.Guid("c9588374-7aaf-43de-8b49-c4abcb7ed22d"), new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
            this.uiPM.AddFloatingPanelInfo(new System.Guid("eb4e7d1f-47d5-4fb6-be2c-dafe98e2770d"), new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
            this.uiPM.AddFloatingPanelInfo(new System.Guid("9198135a-c97a-40de-9c64-2f99f55a4129"), Janus.Windows.UI.Dock.PanelGroupStyle.VerticalTiles, true, new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
            this.uiPM.AddFloatingPanelInfo(new System.Guid("2bef1cc4-de3e-473a-96f0-6cf1cf93d310"), new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
            this.uiPM.AddFloatingPanelInfo(new System.Guid("44105dc7-ebcc-4549-a0ba-272c10af5508"), new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
            this.uiPM.AddFloatingPanelInfo(new System.Guid("4fef83bf-72f4-4e45-b8cc-6e7662e65f5b"), Janus.Windows.UI.Dock.PanelGroupStyle.HorizontalTiles, true, new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
            this.uiPM.AddFloatingPanelInfo(new System.Guid("e5d01963-b192-43b1-ac21-264a511751bf"), new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
            this.uiPM.AddFloatingPanelInfo(new System.Guid("1f2d57b6-824c-4823-98d6-27f856dde265"), Janus.Windows.UI.Dock.PanelGroupStyle.VerticalTiles, true, new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
            this.uiPM.AddFloatingPanelInfo(new System.Guid("093eb809-a79f-4fdf-8ec4-3ac1a5e75da6"), Janus.Windows.UI.Dock.PanelGroupStyle.HorizontalTiles, true, new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
            this.uiPM.AddFloatingPanelInfo(new System.Guid("97912ae8-3203-4b0d-af33-d0227dcf1905"), new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
            this.uiPM.AddFloatingPanelInfo(new System.Guid("4f19892b-156c-49aa-9f84-543d53945feb"), new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
            this.uiPM.AddFloatingPanelInfo(new System.Guid("7ec558bc-8e38-4f64-90ba-07193906f73f"), new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
            this.uiPM.EndPanelInfo();
            // 
            // uiPanelAdFile
            // 
            this.uiPanelAdFile.ActiveCaptionMode = Janus.Windows.UI.Dock.ActiveCaptionMode.Never;
            this.uiPanelAdFile.CaptionVisible = Janus.Windows.UI.InheritableBoolean.True;
            this.uiPanelAdFile.CloseButtonVisible = Janus.Windows.UI.InheritableBoolean.True;
            this.uiPanelAdFile.Location = new System.Drawing.Point(0, 0);
            this.uiPanelAdFile.Name = "uiPanelAdFile";
            this.uiPanelAdFile.Size = new System.Drawing.Size(1010, 677);
            this.uiPanelAdFile.TabIndex = 4;
            this.uiPanelAdFile.Text = "�������Ϲ�������";
            // 
            // uiPanelSearch
            // 
            this.uiPanelSearch.CaptionVisible = Janus.Windows.UI.InheritableBoolean.False;
            this.uiPanelSearch.CloseButtonVisible = Janus.Windows.UI.InheritableBoolean.False;
            this.uiPanelSearch.InnerContainer = this.uiPanel1Container;
            this.uiPanelSearch.Location = new System.Drawing.Point(0, 22);
            this.uiPanelSearch.Name = "uiPanelSearch";
            this.uiPanelSearch.Size = new System.Drawing.Size(1010, 41);
            this.uiPanelSearch.TabIndex = 4;
            this.uiPanelSearch.Text = "�˻�";
            // 
            // uiPanel1Container
            // 
            this.uiPanel1Container.Controls.Add(this.pnlSearch);
            this.uiPanel1Container.Location = new System.Drawing.Point(1, 1);
            this.uiPanel1Container.Name = "uiPanel1Container";
            this.uiPanel1Container.Size = new System.Drawing.Size(1008, 39);
            this.uiPanel1Container.TabIndex = 0;
            // 
            // pnlSearch
            // 
            this.pnlSearch.BackColor = System.Drawing.SystemColors.Window;
            this.pnlSearch.Controls.Add(this.chkAdState_40);
            this.pnlSearch.Controls.Add(this.chkAdState_30);
            this.pnlSearch.Controls.Add(this.chkAdState_20);
            this.pnlSearch.Controls.Add(this.chkAdState_10);
            this.pnlSearch.Controls.Add(this.btnExcel);
            this.pnlSearch.Controls.Add(this.ebSearchKey);
            this.pnlSearch.Controls.Add(this.lbAdState);
            this.pnlSearch.Controls.Add(this.btnSearch);
            this.pnlSearch.Controls.Add(this.cbSearchMedia);
            this.pnlSearch.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlSearch.Location = new System.Drawing.Point(0, 0);
            this.pnlSearch.Name = "pnlSearch";
            this.pnlSearch.Size = new System.Drawing.Size(1008, 39);
            this.pnlSearch.TabIndex = 4;
            // 
            // chkAdState_40
            // 
            this.chkAdState_40.Location = new System.Drawing.Point(378, 8);
            this.chkAdState_40.Name = "chkAdState_40";
            this.chkAdState_40.Size = new System.Drawing.Size(46, 23);
            this.chkAdState_40.TabIndex = 29;
            this.chkAdState_40.Text = "����";
            this.chkAdState_40.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            // 
            // chkAdState_30
            // 
            this.chkAdState_30.Location = new System.Drawing.Point(321, 8);
            this.chkAdState_30.Name = "chkAdState_30";
            this.chkAdState_30.Size = new System.Drawing.Size(46, 23);
            this.chkAdState_30.TabIndex = 29;
            this.chkAdState_30.Text = "����";
            this.chkAdState_30.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            // 
            // chkAdState_20
            // 
            this.chkAdState_20.Checked = true;
            this.chkAdState_20.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkAdState_20.Location = new System.Drawing.Point(264, 8);
            this.chkAdState_20.Name = "chkAdState_20";
            this.chkAdState_20.Size = new System.Drawing.Size(46, 23);
            this.chkAdState_20.TabIndex = 29;
            this.chkAdState_20.Text = "��";
            this.chkAdState_20.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            // 
            // chkAdState_10
            // 
            this.chkAdState_10.Checked = true;
            this.chkAdState_10.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkAdState_10.Location = new System.Drawing.Point(207, 8);
            this.chkAdState_10.Name = "chkAdState_10";
            this.chkAdState_10.Size = new System.Drawing.Size(46, 23);
            this.chkAdState_10.TabIndex = 28;
            this.chkAdState_10.Text = "���";
            this.chkAdState_10.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            // 
            // btnExcel
            // 
            this.btnExcel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExcel.Enabled = false;
            this.btnExcel.Location = new System.Drawing.Point(895, 8);
            this.btnExcel.Name = "btnExcel";
            this.btnExcel.Size = new System.Drawing.Size(104, 24);
            this.btnExcel.TabIndex = 8;
            this.btnExcel.Text = "EXCEL ���";
            this.btnExcel.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.btnExcel.Click += new System.EventHandler(this.btnExcel_Click);
            // 
            // ebSearchKey
            // 
            this.ebSearchKey.Location = new System.Drawing.Point(444, 9);
            this.ebSearchKey.Name = "ebSearchKey";
            this.ebSearchKey.Size = new System.Drawing.Size(193, 21);
            this.ebSearchKey.TabIndex = 6;
            this.ebSearchKey.Text = "�˻�� �Է��ϼ���";
            this.ebSearchKey.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
            this.ebSearchKey.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            this.ebSearchKey.TextChanged += new System.EventHandler(this.ebSearchKey_TextChanged);
            this.ebSearchKey.Click += new System.EventHandler(this.ebSearchKey_Click);
            this.ebSearchKey.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ebSearchKey_KeyDown);
            // 
            // lbAdState
            // 
            this.lbAdState.Location = new System.Drawing.Point(144, 9);
            this.lbAdState.Name = "lbAdState";
            this.lbAdState.Size = new System.Drawing.Size(64, 21);
            this.lbAdState.TabIndex = 27;
            this.lbAdState.Text = "�������";
            this.lbAdState.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnSearch
            // 
            this.btnSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSearch.Enabled = false;
            this.btnSearch.Location = new System.Drawing.Point(783, 8);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(104, 24);
            this.btnSearch.TabIndex = 7;
            this.btnSearch.Text = "�� ȸ";
            this.btnSearch.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // cbSearchMedia
            // 
            this.cbSearchMedia.BackColor = System.Drawing.Color.White;
            this.cbSearchMedia.ComboStyle = Janus.Windows.EditControls.ComboStyle.DropDownList;
            this.cbSearchMedia.Location = new System.Drawing.Point(8, 9);
            this.cbSearchMedia.Name = "cbSearchMedia";
            this.cbSearchMedia.Size = new System.Drawing.Size(120, 21);
            this.cbSearchMedia.TabIndex = 1;
            this.cbSearchMedia.Text = "��ü����";
            this.cbSearchMedia.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            // 
            // uiPanelDetail
            // 
            this.uiPanelDetail.CaptionVisible = Janus.Windows.UI.InheritableBoolean.False;
            this.uiPanelDetail.GroupStyle = Janus.Windows.UI.Dock.PanelGroupStyle.VerticalTiles;
            this.uiPanelDetail.Location = new System.Drawing.Point(0, 67);
            this.uiPanelDetail.Name = "uiPanelDetail";
            this.uiPanelDetail.Size = new System.Drawing.Size(1010, 610);
            this.uiPanelDetail.TabIndex = 4;
            this.uiPanelDetail.Text = "Panel 2";
            // 
            // uiPanel3
            // 
            this.uiPanel3.CaptionVisible = Janus.Windows.UI.InheritableBoolean.False;
            this.uiPanel3.Location = new System.Drawing.Point(0, 0);
            this.uiPanel3.Name = "uiPanel3";
            this.uiPanel3.Size = new System.Drawing.Size(234, 610);
            this.uiPanel3.TabIndex = 4;
            this.uiPanel3.Text = "Panel 3";
            // 
            // uiPanelStateList
            // 
            this.uiPanelStateList.InnerContainer = this.uiPanel5Container;
            this.uiPanelStateList.Location = new System.Drawing.Point(0, 0);
            this.uiPanelStateList.Name = "uiPanelStateList";
            this.uiPanelStateList.Size = new System.Drawing.Size(234, 303);
            this.uiPanelStateList.TabIndex = 4;
            this.uiPanelStateList.Text = "��������";
            // 
            // uiPanel5Container
            // 
            this.uiPanel5Container.Controls.Add(this.grdExFileCount);
            this.uiPanel5Container.Location = new System.Drawing.Point(1, 23);
            this.uiPanel5Container.Name = "uiPanel5Container";
            this.uiPanel5Container.Size = new System.Drawing.Size(232, 279);
            this.uiPanel5Container.TabIndex = 0;
            // 
            // grdExFileCount
            // 
            this.grdExFileCount.AllowEdit = Janus.Windows.GridEX.InheritableBoolean.False;
            this.grdExFileCount.AlternatingColors = true;
            this.grdExFileCount.BorderStyle = Janus.Windows.GridEX.BorderStyle.None;
            this.grdExFileCount.DataSource = this.dvFileCount;
            grdExFileCount_DesignTimeLayout.LayoutString = resources.GetString("grdExFileCount_DesignTimeLayout.LayoutString");
            this.grdExFileCount.DesignTimeLayout = grdExFileCount_DesignTimeLayout;
            this.grdExFileCount.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdExFileCount.EditorsControlStyle.ButtonAppearance = Janus.Windows.GridEX.ButtonAppearance.Regular;
            this.grdExFileCount.EmptyRows = true;
            this.grdExFileCount.FocusCellFormatStyle.BackColor = System.Drawing.SystemColors.Highlight;
            this.grdExFileCount.FocusCellFormatStyle.ForeColor = System.Drawing.Color.White;
            this.grdExFileCount.FocusStyle = Janus.Windows.GridEX.FocusStyle.None;
            this.grdExFileCount.FrozenColumns = 2;
            this.grdExFileCount.GridLineColor = System.Drawing.Color.Silver;
            this.grdExFileCount.GridLines = Janus.Windows.GridEX.GridLines.Vertical;
            this.grdExFileCount.GridLineStyle = Janus.Windows.GridEX.GridLineStyle.Solid;
            this.grdExFileCount.GroupByBoxVisible = false;
            this.grdExFileCount.HeaderFormatStyle.TextAlignment = Janus.Windows.GridEX.TextAlignment.Center;
            this.grdExFileCount.HideSelection = Janus.Windows.GridEX.HideSelection.HighlightInactive;
            this.grdExFileCount.Location = new System.Drawing.Point(0, 0);
            this.grdExFileCount.Name = "grdExFileCount";
            this.grdExFileCount.ScrollBars = Janus.Windows.GridEX.ScrollBars.None;
            this.grdExFileCount.SelectedInactiveFormatStyle.BackColor = System.Drawing.Color.Gold;
            this.grdExFileCount.Size = new System.Drawing.Size(232, 279);
            this.grdExFileCount.TabIndex = 10;
            this.grdExFileCount.TabKeyBehavior = Janus.Windows.GridEX.TabKeyBehavior.ControlNavigation;
            this.grdExFileCount.ThemedAreas = ((Janus.Windows.GridEX.ThemedArea)(((((((((Janus.Windows.GridEX.ThemedArea.ScrollBars | Janus.Windows.GridEX.ThemedArea.EditControls)
                        | Janus.Windows.GridEX.ThemedArea.Headers)
                        | Janus.Windows.GridEX.ThemedArea.GroupByBox)
                        | Janus.Windows.GridEX.ThemedArea.GroupRows)
                        | Janus.Windows.GridEX.ThemedArea.ControlBorder)
                        | Janus.Windows.GridEX.ThemedArea.Cards)
                        | Janus.Windows.GridEX.ThemedArea.Gridlines)
                        | Janus.Windows.GridEX.ThemedArea.CheckBoxes)));
            this.grdExFileCount.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            // 
            // dvFileCount
            // 
            this.dvFileCount.Table = this.adFileWideDs.FileCount;
            // 
            // adFileWideDs
            // 
            this.adFileWideDs.DataSetName = "AdFileWideDs";
            this.adFileWideDs.Locale = new System.Globalization.CultureInfo("en-US");
            this.adFileWideDs.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // uiPanelFiles
            // 
            this.uiPanelFiles.InnerContainer = this.uiPanel6Container;
            this.uiPanelFiles.Location = new System.Drawing.Point(0, 307);
            this.uiPanelFiles.Name = "uiPanelFiles";
            this.uiPanelFiles.Size = new System.Drawing.Size(234, 303);
            this.uiPanelFiles.TabIndex = 4;
            this.uiPanelFiles.Text = "�������Ϲ������";
            // 
            // uiPanel6Container
            // 
            this.uiPanel6Container.Controls.Add(this.pnlUserDetail);
            this.uiPanel6Container.Location = new System.Drawing.Point(1, 23);
            this.uiPanel6Container.Name = "uiPanel6Container";
            this.uiPanel6Container.Size = new System.Drawing.Size(232, 279);
            this.uiPanel6Container.TabIndex = 0;
            // 
            // pnlUserDetail
            // 
            this.pnlUserDetail.BackColor = System.Drawing.SystemColors.Window;
            this.pnlUserDetail.Controls.Add(this.chkTestCheck);
            this.pnlUserDetail.Controls.Add(this.lbMsg);
            this.pnlUserDetail.Controls.Add(this.chkCMS);
            this.pnlUserDetail.Controls.Add(this.lblMsg);
            this.pnlUserDetail.Controls.Add(this.btnCDNSync);
            this.pnlUserDetail.Controls.Add(this.label1);
            this.pnlUserDetail.Controls.Add(this.btnSTBDelete);
            this.pnlUserDetail.Controls.Add(this.btnCDNPublish);
            this.pnlUserDetail.Controls.Add(this.btnChkComplete);
            this.pnlUserDetail.Controls.Add(this.btnChkCompleteCancel);
            this.pnlUserDetail.Controls.Add(this.btnCDNPublishCancel);
            this.pnlUserDetail.Controls.Add(this.btnSTBDeleteCancel);
            this.pnlUserDetail.Controls.Add(this.lbFileListCount);
            this.pnlUserDetail.Controls.Add(this.btnCDNSyncCancel);
            this.pnlUserDetail.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlUserDetail.Location = new System.Drawing.Point(0, 0);
            this.pnlUserDetail.Name = "pnlUserDetail";
            this.pnlUserDetail.Size = new System.Drawing.Size(232, 279);
            this.pnlUserDetail.TabIndex = 12;
            // 
            // chkTestCheck
            // 
            this.chkTestCheck.Location = new System.Drawing.Point(42, 22);
            this.chkTestCheck.Name = "chkTestCheck";
            this.chkTestCheck.Size = new System.Drawing.Size(165, 23);
            this.chkTestCheck.TabIndex = 28;
            this.chkTestCheck.Text = "����Ȯ��(�׽�Ʈ�˼�����)";
            // 
            // lbMsg
            // 
            this.lbMsg.Font = new System.Drawing.Font("�������", 8.249999F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lbMsg.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.lbMsg.Location = new System.Drawing.Point(14, 48);
            this.lbMsg.Name = "lbMsg";
            this.lbMsg.Size = new System.Drawing.Size(206, 16);
            this.lbMsg.TabIndex = 26;
            this.lbMsg.Text = "����";
            this.lbMsg.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // chkCMS
            // 
            this.chkCMS.Location = new System.Drawing.Point(42, 3);
            this.chkCMS.Name = "chkCMS";
            this.chkCMS.Size = new System.Drawing.Size(166, 23);
            this.chkCMS.TabIndex = 28;
            this.chkCMS.Text = "CMS���� ȣ��";
            // 
            // lblMsg
            // 
            this.lblMsg.ForeColor = System.Drawing.Color.Green;
            this.lblMsg.Location = new System.Drawing.Point(8, 185);
            this.lblMsg.Name = "lblMsg";
            this.lblMsg.Size = new System.Drawing.Size(216, 64);
            this.lblMsg.TabIndex = 23;
            this.lblMsg.Text = "�۾��ȳ� �޼����� ǥ�� �մϴ�.";
            // 
            // btnCDNSync
            // 
            this.btnCDNSync.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnCDNSync.Enabled = false;
            this.btnCDNSync.Location = new System.Drawing.Point(8, 96);
            this.btnCDNSync.Name = "btnCDNSync";
            this.btnCDNSync.Size = new System.Drawing.Size(104, 24);
            this.btnCDNSync.TabIndex = 13;
            this.btnCDNSync.Text = "CDN����Ȯ��";
            this.btnCDNSync.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.btnCDNSync.Click += new System.EventHandler(this.btnCDNSync_Click);
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("�������", 8.249999F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label1.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.label1.Location = new System.Drawing.Point(31, 253);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(96, 23);
            this.label1.TabIndex = 22;
            this.label1.Text = "���ϸ���Ʈ���� :";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnSTBDelete
            // 
            this.btnSTBDelete.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSTBDelete.Enabled = false;
            this.btnSTBDelete.Location = new System.Drawing.Point(8, 150);
            this.btnSTBDelete.Name = "btnSTBDelete";
            this.btnSTBDelete.Size = new System.Drawing.Size(104, 24);
            this.btnSTBDelete.TabIndex = 17;
            this.btnSTBDelete.Text = "���ϼ�ž����";
            this.btnSTBDelete.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.btnSTBDelete.Click += new System.EventHandler(this.btnSTBDelete_Click);
            // 
            // btnCDNPublish
            // 
            this.btnCDNPublish.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnCDNPublish.Enabled = false;
            this.btnCDNPublish.Location = new System.Drawing.Point(8, 123);
            this.btnCDNPublish.Name = "btnCDNPublish";
            this.btnCDNPublish.Size = new System.Drawing.Size(104, 24);
            this.btnCDNPublish.TabIndex = 15;
            this.btnCDNPublish.Text = "CDN�����Ϸ�";
            this.btnCDNPublish.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.btnCDNPublish.Click += new System.EventHandler(this.btnCDNPublish_Click);
            // 
            // btnChkComplete
            // 
            this.btnChkComplete.ButtonStyle = Janus.Windows.EditControls.ButtonStyle.Button;
            this.btnChkComplete.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnChkComplete.Enabled = false;
            this.btnChkComplete.Location = new System.Drawing.Point(8, 69);
            this.btnChkComplete.Name = "btnChkComplete";
            this.btnChkComplete.Size = new System.Drawing.Size(104, 24);
            this.btnChkComplete.TabIndex = 11;
            this.btnChkComplete.Text = "�˼��Ϸ�";
            this.btnChkComplete.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.btnChkComplete.Click += new System.EventHandler(this.btnChkComplete_Click);
            // 
            // btnChkCompleteCancel
            // 
            this.btnChkCompleteCancel.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnChkCompleteCancel.Enabled = false;
            this.btnChkCompleteCancel.Location = new System.Drawing.Point(120, 69);
            this.btnChkCompleteCancel.Name = "btnChkCompleteCancel";
            this.btnChkCompleteCancel.Size = new System.Drawing.Size(104, 24);
            this.btnChkCompleteCancel.TabIndex = 12;
            this.btnChkCompleteCancel.Text = "�˼��Ϸ����";
            this.btnChkCompleteCancel.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.btnChkCompleteCancel.Click += new System.EventHandler(this.btnChkCompleteCancel_Click);
            // 
            // btnCDNPublishCancel
            // 
            this.btnCDNPublishCancel.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnCDNPublishCancel.Enabled = false;
            this.btnCDNPublishCancel.Location = new System.Drawing.Point(120, 123);
            this.btnCDNPublishCancel.Name = "btnCDNPublishCancel";
            this.btnCDNPublishCancel.Size = new System.Drawing.Size(104, 24);
            this.btnCDNPublishCancel.TabIndex = 16;
            this.btnCDNPublishCancel.Text = "CDN�������";
            this.btnCDNPublishCancel.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.btnCDNPublishCancel.Click += new System.EventHandler(this.btnCDNPublishCancel_Click);
            // 
            // btnSTBDeleteCancel
            // 
            this.btnSTBDeleteCancel.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSTBDeleteCancel.Enabled = false;
            this.btnSTBDeleteCancel.Location = new System.Drawing.Point(120, 150);
            this.btnSTBDeleteCancel.Name = "btnSTBDeleteCancel";
            this.btnSTBDeleteCancel.Size = new System.Drawing.Size(104, 24);
            this.btnSTBDeleteCancel.TabIndex = 18;
            this.btnSTBDeleteCancel.Text = "��ž�������";
            this.btnSTBDeleteCancel.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.btnSTBDeleteCancel.Click += new System.EventHandler(this.btnSTBDeleteCancel_Click);
            // 
            // lbFileListCount
            // 
            this.lbFileListCount.Font = new System.Drawing.Font("�������", 8.249999F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lbFileListCount.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.lbFileListCount.Location = new System.Drawing.Point(135, 253);
            this.lbFileListCount.Name = "lbFileListCount";
            this.lbFileListCount.Size = new System.Drawing.Size(72, 23);
            this.lbFileListCount.TabIndex = 22;
            this.lbFileListCount.Text = "0/0";
            this.lbFileListCount.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnCDNSyncCancel
            // 
            this.btnCDNSyncCancel.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnCDNSyncCancel.Enabled = false;
            this.btnCDNSyncCancel.Location = new System.Drawing.Point(120, 96);
            this.btnCDNSyncCancel.Name = "btnCDNSyncCancel";
            this.btnCDNSyncCancel.Size = new System.Drawing.Size(104, 24);
            this.btnCDNSyncCancel.TabIndex = 14;
            this.btnCDNSyncCancel.Text = "CDN�������";
            this.btnCDNSyncCancel.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.btnCDNSyncCancel.Click += new System.EventHandler(this.btnCDNSyncCancel_Click);
            // 
            // uiPanelState
            // 
            this.uiPanelState.CaptionVisible = Janus.Windows.UI.InheritableBoolean.True;
            this.uiPanelState.InnerContainer = this.uiPanel4Container;
            this.uiPanelState.Location = new System.Drawing.Point(238, 0);
            this.uiPanelState.Name = "uiPanelState";
            this.uiPanelState.Size = new System.Drawing.Size(772, 610);
            this.uiPanelState.TabIndex = 4;
            this.uiPanelState.Text = "���ϻ��º���";
            // 
            // uiPanel4Container
            // 
            this.uiPanel4Container.Controls.Add(this.grdExAdFileWideList);
            this.uiPanel4Container.Location = new System.Drawing.Point(1, 23);
            this.uiPanel4Container.Name = "uiPanel4Container";
            this.uiPanel4Container.Size = new System.Drawing.Size(770, 586);
            this.uiPanel4Container.TabIndex = 0;
            // 
            // grdExAdFileWideList
            // 
            this.grdExAdFileWideList.AlternatingColors = true;
            this.grdExAdFileWideList.BorderStyle = Janus.Windows.GridEX.BorderStyle.None;
            this.grdExAdFileWideList.DataSource = this.dvAdFileWide;
            grdExAdFileWideList_DesignTimeLayout.LayoutString = resources.GetString("grdExAdFileWideList_DesignTimeLayout.LayoutString");
            this.grdExAdFileWideList.DesignTimeLayout = grdExAdFileWideList_DesignTimeLayout;
            this.grdExAdFileWideList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdExAdFileWideList.EmptyRows = true;
            this.grdExAdFileWideList.FocusCellFormatStyle.BackColor = System.Drawing.SystemColors.Highlight;
            this.grdExAdFileWideList.FocusCellFormatStyle.ForeColor = System.Drawing.Color.White;
            this.grdExAdFileWideList.FocusStyle = Janus.Windows.GridEX.FocusStyle.None;
            this.grdExAdFileWideList.Font = new System.Drawing.Font("�������", 8.5F);
            this.grdExAdFileWideList.FrozenColumns = 3;
            this.grdExAdFileWideList.GridLineColor = System.Drawing.Color.Silver;
            this.grdExAdFileWideList.GridLines = Janus.Windows.GridEX.GridLines.Vertical;
            this.grdExAdFileWideList.GridLineStyle = Janus.Windows.GridEX.GridLineStyle.Solid;
            this.grdExAdFileWideList.GroupByBoxVisible = false;
            this.grdExAdFileWideList.HeaderFormatStyle.TextAlignment = Janus.Windows.GridEX.TextAlignment.Center;
            this.grdExAdFileWideList.HideSelection = Janus.Windows.GridEX.HideSelection.HighlightInactive;
            this.grdExAdFileWideList.Location = new System.Drawing.Point(0, 0);
            this.grdExAdFileWideList.Name = "grdExAdFileWideList";
            this.grdExAdFileWideList.ScrollBars = Janus.Windows.GridEX.ScrollBars.Both;
            this.grdExAdFileWideList.SelectedInactiveFormatStyle.BackColor = System.Drawing.Color.Gold;
            this.grdExAdFileWideList.Size = new System.Drawing.Size(770, 586);
            this.grdExAdFileWideList.TabIndex = 11;
            this.grdExAdFileWideList.TabKeyBehavior = Janus.Windows.GridEX.TabKeyBehavior.ControlNavigation;
            this.grdExAdFileWideList.ThemedAreas = ((Janus.Windows.GridEX.ThemedArea)(((((((((Janus.Windows.GridEX.ThemedArea.ScrollBars | Janus.Windows.GridEX.ThemedArea.EditControls)
                        | Janus.Windows.GridEX.ThemedArea.Headers)
                        | Janus.Windows.GridEX.ThemedArea.GroupByBox)
                        | Janus.Windows.GridEX.ThemedArea.GroupRows)
                        | Janus.Windows.GridEX.ThemedArea.ControlBorder)
                        | Janus.Windows.GridEX.ThemedArea.Cards)
                        | Janus.Windows.GridEX.ThemedArea.Gridlines)
                        | Janus.Windows.GridEX.ThemedArea.CheckBoxes)));
            this.grdExAdFileWideList.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            this.grdExAdFileWideList.CellValueChanged += new Janus.Windows.GridEX.ColumnActionEventHandler(this.grdExAdFileWideList_CellValueChanged);
            this.grdExAdFileWideList.ColumnHeaderClick += new Janus.Windows.GridEX.ColumnActionEventHandler(this.grdExAdFileWideList_ColumnHeaderClick);
            // 
            // dvAdFileWide
            // 
            this.dvAdFileWide.Table = this.adFileWideDs.AdFileWide;
            // 
            // grdExAdStatusList
            // 
            this.grdExAdStatusList.AllowEdit = Janus.Windows.GridEX.InheritableBoolean.False;
            this.grdExAdStatusList.AlternatingColors = true;
            this.grdExAdStatusList.BorderStyle = Janus.Windows.GridEX.BorderStyle.None;
            this.grdExAdStatusList.DataSource = this.dsItemSchedule;
            grdExAdStatusList_DesignTimeLayout.LayoutString = resources.GetString("grdExAdStatusList_DesignTimeLayout.LayoutString");
            this.grdExAdStatusList.DesignTimeLayout = grdExAdStatusList_DesignTimeLayout;
            this.grdExAdStatusList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdExAdStatusList.EditorsControlStyle.ButtonAppearance = Janus.Windows.GridEX.ButtonAppearance.Regular;
            this.grdExAdStatusList.EmptyRows = true;
            this.grdExAdStatusList.FocusCellFormatStyle.BackColor = System.Drawing.SystemColors.Highlight;
            this.grdExAdStatusList.FocusCellFormatStyle.ForeColor = System.Drawing.Color.White;
            this.grdExAdStatusList.FocusStyle = Janus.Windows.GridEX.FocusStyle.None;
            this.grdExAdStatusList.GridLineColor = System.Drawing.Color.Silver;
            this.grdExAdStatusList.GridLines = Janus.Windows.GridEX.GridLines.Vertical;
            this.grdExAdStatusList.GridLineStyle = Janus.Windows.GridEX.GridLineStyle.Solid;
            this.grdExAdStatusList.GroupByBoxVisible = false;
            this.grdExAdStatusList.HeaderFormatStyle.TextAlignment = Janus.Windows.GridEX.TextAlignment.Center;
            this.grdExAdStatusList.Location = new System.Drawing.Point(0, 0);
            this.grdExAdStatusList.Name = "grdExAdStatusList";
            this.grdExAdStatusList.ScrollBars = Janus.Windows.GridEX.ScrollBars.Vertical;
            this.grdExAdStatusList.SelectedInactiveFormatStyle.BackColor = System.Drawing.Color.DarkOrange;
            this.grdExAdStatusList.Size = new System.Drawing.Size(810, 278);
            this.grdExAdStatusList.TabIndex = 20;
            this.grdExAdStatusList.TabKeyBehavior = Janus.Windows.GridEX.TabKeyBehavior.ControlNavigation;
            this.grdExAdStatusList.ThemedAreas = ((Janus.Windows.GridEX.ThemedArea)(((((((((Janus.Windows.GridEX.ThemedArea.ScrollBars | Janus.Windows.GridEX.ThemedArea.EditControls)
                        | Janus.Windows.GridEX.ThemedArea.Headers)
                        | Janus.Windows.GridEX.ThemedArea.GroupByBox)
                        | Janus.Windows.GridEX.ThemedArea.GroupRows)
                        | Janus.Windows.GridEX.ThemedArea.ControlBorder)
                        | Janus.Windows.GridEX.ThemedArea.Cards)
                        | Janus.Windows.GridEX.ThemedArea.Gridlines)
                        | Janus.Windows.GridEX.ThemedArea.CheckBoxes)));
            this.grdExAdStatusList.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            // 
            // dsItemSchedule
            // 
            this.dsItemSchedule.Table = this.adFileWideDs.AdSchedule;
            // 
            // AdFileWideControl
            // 
            this.Controls.Add(this.uiPanelAdFile);
            this.Font = global::AdManagerClient.Properties.Settings.Default.Font1;
            this.Name = "AdFileWideControl";
            this.Size = new System.Drawing.Size(1010, 677);
            this.Load += new System.EventHandler(this.UserControl_Load);
            ((System.ComponentModel.ISupportInitialize)(this.uiPM)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelAdFile)).EndInit();
            this.uiPanelAdFile.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelSearch)).EndInit();
            this.uiPanelSearch.ResumeLayout(false);
            this.uiPanel1Container.ResumeLayout(false);
            this.pnlSearch.ResumeLayout(false);
            this.pnlSearch.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelDetail)).EndInit();
            this.uiPanelDetail.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.uiPanel3)).EndInit();
            this.uiPanel3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelStateList)).EndInit();
            this.uiPanelStateList.ResumeLayout(false);
            this.uiPanel5Container.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdExFileCount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dvFileCount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.adFileWideDs)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelFiles)).EndInit();
            this.uiPanelFiles.ResumeLayout(false);
            this.uiPanel6Container.ResumeLayout(false);
            this.pnlUserDetail.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelState)).EndInit();
            this.uiPanelState.ResumeLayout(false);
            this.uiPanel4Container.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdExAdFileWideList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dvAdFileWide)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdExAdStatusList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dsItemSchedule)).EndInit();
            this.ResumeLayout(false);

		}
		#endregion

		#region ��Ʈ�� �ε�
		private void UserControl_Load(object sender, System.EventArgs e)
		{
			Application.DoEvents();

			// �����Ͱ����� ��ü����
			dtCount = ((DataView)grdExFileCount.DataSource).Table;  
			cmCount = (CurrencyManager) this.BindingContext[grdExFileCount.DataSource]; 
			cmCount.PositionChanged += new System.EventHandler(OnGrdRowChanged); 

			dtFile = ((DataView)grdExAdFileWideList.DataSource).Table;   
			cmFile = (CurrencyManager) this.BindingContext[grdExAdFileWideList.DataSource]; 
			cmFile.PositionChanged += new System.EventHandler(OnFileRowChanged); 

			// ��Ʈ�� �ʱ�ȭ
			InitControl();	
		}

		#endregion

		#region ��Ʈ�� �ʱ�ȭ
		private void InitControl()
		{
			ProgressStart();
			InitCombo();
	
			if(menu.CanRead(MenuCode))      canRead = true;
			if(menu.CanUpdate(MenuCode))    canUpdate = true;
			//if(menu.CanDelete(MenuCode))    //canDelete = true;
			
            Debug.WriteLine("���ε��� �ʱ��۾� ����");
			InitButton();
			SetConfig();
			createCDNFtp();
			createTESTFtp();
            Debug.WriteLine("���ε��� �ʱ��۾� �Ϸ�");

			ProgressStop();

			if(canRead) SearchFileCount();
		}


		private void InitCombo()
		{
			Init_MediaCode();
			
			InitCombo_Level();
		}

		private void Init_MediaCode()
		{
			// ��ü�� ��ȸ�Ѵ�.
			MediaCodeModel mediacodeModel = new MediaCodeModel();
			new MediaCodeManager(systemModel, commonModel).GetMediaCodeList(mediacodeModel);
			
			if (mediacodeModel.ResultCD.Equals("0000"))
			{
				// �����ͼ¿� ����
				Utility.SetDataTable(adFileWideDs.Medias, mediacodeModel.MediaCodeDataSet);				
			}

			// �˻������� �޺�
			this.cbSearchMedia.Items.Clear();
			
			// �޺��ڽ��� ��Ʈ�� �ڵ����� ���� Item�迭�� ����
			Janus.Windows.EditControls.UIComboBoxItem[] comboItems = new Janus.Windows.EditControls.UIComboBoxItem[mediacodeModel.ResultCnt + 1];

			comboItems[0] = new Janus.Windows.EditControls.UIComboBoxItem("��ü����","00");
			
			for(int i=0;i<mediacodeModel.ResultCnt;i++)
			{
				DataRow row = adFileWideDs.Medias.Rows[i];

				string val = row["MediaCode"].ToString();
				string txt = row["MediaName"].ToString();
				comboItems[i+1] = new Janus.Windows.EditControls.UIComboBoxItem(txt,val);
			}
			// �޺��� ��Ʈ
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
				for(int i=0;i < adFileWideDs.Medias.Rows.Count;i++)
				{
					DataRow row = adFileWideDs.Medias.Rows[i];					
					if(row["MediaCode"].ToString().Equals(FrameSystem._HANATV.ToString()))
					{
						cbSearchMedia.SelectedValue = FrameSystem._HANATV; // �ϳ�TV�� �⺻������ �Ѵ�.	 		
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
			btnExcel.Enabled = false;	

			chkCMS.Enabled					= false;
			chkTestCheck.Enabled			= false;
			btnChkComplete.Enabled			= false;
			btnChkCompleteCancel.Enabled	= false;
			btnCDNSync.Enabled				= false;
			btnCDNSyncCancel.Enabled		= false;
			btnCDNPublish.Enabled			= false;
			btnCDNPublishCancel.Enabled		= false;
			btnSTBDelete.Enabled			= false;
			btnSTBDeleteCancel.Enabled		= false;		
		
			Application.DoEvents();
		}

		#endregion

		#region �������� �׼�ó�� �޼ҵ�

		/// <summary>
		/// �������º� ��� �׸��� Row�����
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void OnGrdRowChanged(object sender, System.EventArgs e) 
		{

            if (!IsSearching) // 2011.11.29 JH.Park ��ȸ���� �ƴҰ�쿡�� �����ϵ��� ����
            {
                Debug.WriteLine("Row����->��������");
                SetDetailText();
                InitButton();
            }
		}

        /// <summary>
        /// ������� �׸��� Row�����
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
		private void OnFileRowChanged(object sender, System.EventArgs e) 
		{

            if (!IsSearching) // 2011.11.29 JH.Park ��ȸ���� �ƴҰ�쿡�� �����ϵ��� ����
            {
                Debug.WriteLine("Row����->�������");
                //SetDetailTextSchedule();
                InitButton();
            }
		}

		/// <summary>
		/// ��ȸ��ư Ŭ��
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnSearch_Click(object sender, System.EventArgs e)
		{
			DisableButton();
			SearchFileCount();
			InitButton();
		}

		/// <summary>
		/// �˻��� ����
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void ebSearchKey_TextChanged(object sender, System.EventArgs e)
		{
			IsNewSearchKey = false;
		}

		/// <summary>
		/// �˻��� Ŭ�� 
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
				SearchFileCount();
			}
		}

		/// <summary>
		/// ó����ư Ŭ��
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		/// 
		private void btnChkRequest_Click(object sender, System.EventArgs e)
		{
			SetAdFileChkRequest();			
		}

		private void btnChkRequestCancel_Click(object sender, System.EventArgs e)
		{
			SetAdFileChkRequestCancel();			
		}

		/// <summary>
		/// ���ϰ˼��Ϸ�
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnChkComplete_Click(object sender, System.EventArgs e)
		{
			SetAdFileChkComplete();			
		}

		/// <summary>
		/// ���ϰ˼����
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnChkCompleteCancel_Click(object sender, System.EventArgs e)
		{
			SetAdFileChkCompleteCancel();			
		}

		/// <summary>
		/// CDN����Ȯ��
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnCDNSync_Click(object sender, System.EventArgs e)
		{
			SetAdFileCDNSync();			
		}

		private void btnCDNSyncCancel_Click(object sender, System.EventArgs e)
		{
			SetAdFileCDNSyncCancel();			
		}

		private void btnCDNPublish_Click(object sender, System.EventArgs e)
		{
			SetAdFileCDNPublish();			
		}

		private void btnCDNPublishCancel_Click(object sender, System.EventArgs e)
		{
			SetAdFileCDNPublishCancel();			
		}

		private void btnSTBDelete_Click(object sender, System.EventArgs e)
		{
			SetAdFileSTBDelete();
		}

		private void btnSTBDeleteCancel_Click(object sender, System.EventArgs e)
		{
			SetAdFileSTBDeleteCancel();
		}

		#region [ �̻�� ]
//		private void btnFileChange_Click(object sender, System.EventArgs e)
//		{
//			SetAdFileChange();		
//		}
		#endregion


		private void grdExAdFileWideList_ColumnHeaderClick(object sender, Janus.Windows.GridEX.ColumnActionEventArgs e)
		{	
		
			//�÷�Index 0(üũ�ڽ��÷���)�� �ƴϸ� ���������� ó��.
			if(e.Column.Index != 0)
			{
				return;
			}

			if(IsAllCheck)
			{
				grdExAdFileWideList.UnCheckAllRecords();
				for(int i=0;i < dtFile.Rows.Count;i++)
				{
					dtFile.Rows[i].BeginEdit();
					dtFile.Rows[i]["CheckYn"]="False";  
					dtFile.Rows[i].EndEdit();
				}
				IsAllCheck = false;
			}
			else
			{
				grdExAdFileWideList.CheckAllRecords();
				for(int i=0;i < dtFile.Rows.Count;i++)
				{
					dtFile.Rows[i].BeginEdit();
					dtFile.Rows[i]["CheckYn"]="True";
					dtFile.Rows[i].EndEdit();
				}
				IsAllCheck = true;
			}	
		}

		#endregion

		#region ó���޼ҵ�

		/// <summary>
		/// �������ϰǼ� ��ȸ
		/// </summary>
		private void SearchFileCount()
		{
            IsSearching = true;

			StatusMessage("�������� ������ ��ȸ�մϴ�.");

			if(cbSearchMedia.SelectedItem.Value.Equals("00")) 
			{
				MessageBox.Show("��ü�� �����Ͽ� �ֽñ� �ٶ��ϴ�.","�������� ��������", MessageBoxButtons.OK, MessageBoxIcon.Information );
				return;
			}

			ProgressStart();

			try
			{
                Debug.WriteLine("�������� ��ȸ");
				keyMediaCode     = "";
				keychkAdState_10 = "";
				keychkAdState_20 = "";
				keychkAdState_30 = "";
				keychkAdState_40 = "";
                keySearchKey = "" ;

				adFileWideModel.Init();
				adFileWideModel.SearchMediaCode		 =  cbSearchMedia.SelectedItem.Value.ToString(); 

				if(IsNewSearchKey)  adFileWideModel.SearchKey = "";
				else                adFileWideModel.SearchKey  = ebSearchKey.Text;

				if(chkAdState_10.Checked)   adFileWideModel.SearchchkAdState_10   = "Y";
				if(chkAdState_20.Checked)   adFileWideModel.SearchchkAdState_20   = "Y";
				if(chkAdState_30.Checked)   adFileWideModel.SearchchkAdState_30   = "Y";
				if(chkAdState_40.Checked)   adFileWideModel.SearchchkAdState_40   = "Y";				
				
				// �������Ϲ�����ȸ ���񽺸� ȣ���Ѵ�.
				new AdFileWideManager(systemModel,commonModel).GetFileCount(adFileWideModel);

				if (adFileWideModel.ResultCD.Equals("0000"))
				{
					Utility.SetDataTable(adFileWideDs.FileCount, adFileWideModel.CountDataSet);		
					keyMediaCode     = adFileWideModel.SearchMediaCode;
					keychkAdState_10 = adFileWideModel.SearchchkAdState_10;
					keychkAdState_20 = adFileWideModel.SearchchkAdState_20;
					keychkAdState_30 = adFileWideModel.SearchchkAdState_30;
					keychkAdState_40 = adFileWideModel.SearchchkAdState_40;
					keySearchKey     = adFileWideModel.SearchKey;

					// 2007.10.01 ���ϸ���Ʈ�Ǽ� �˻�
					FileListCnt      = adFileWideModel.FileListCount;
					lbFileListCount.Text = FileListCnt.ToString() + "/" + FILEMAX.ToString(); 

					AddSchChoice();									
					SetDetailText();

				}
			}
			catch(FrameException fe)
			{
				FrameSystem.showMsgForm("����������ȸ����", new string[] {fe.ErrCode, fe.ResultMsg});
			}
			catch(Exception ex)
			{
				FrameSystem.showMsgForm("����������ȸ����",new string[] {"",ex.Message});
			}
			finally
			{
                IsSearching = false; // ��ȸ�� Flag ����
				ProgressStop();
			}
		}

		/// <summary>
		/// Ű����ã�� �׸��� Ű�� �ش�Ǵ·ο��..
		/// </summary>
		private void AddSchChoice()
		{
			StatusMessage("Ű��");		

			try
			{
				int rowIndex = 0;
				if ( dtCount.Rows.Count < 1 ) return;
              
				foreach (DataRow row in dtCount.Rows)
				{					
				
					if(row["FileState"].ToString().Equals(keyFileState))
					{					
						cmCount.Position = rowIndex;
						break;								
					}
				
					rowIndex++;
					grdExFileCount.EnsureVisible();
				}
			}
			catch(FrameException fe)
			{
				FrameSystem.showMsgForm("Ű������", new string[] {fe.ErrCode, fe.ResultMsg});
			}
			catch(Exception ex)
			{
				FrameSystem.showMsgForm("Ű������",new string[] {"",ex.Message});
			}			
		}


		/// <summary>
		/// �������� �������� ��Ʈ
		/// </summary>
		private void SetDetailText()
		{
			int curRow = cmCount.Position;

			uiPanelFiles.Text = "�������Ϲ������";

			if(curRow < 0) return;	// �����Ͱ� ������ �������� �ʴ´�.

			keyFileState          = dtCount.Rows[curRow]["FileState"].ToString();
			keyFileStateName      = dtCount.Rows[curRow]["FileStateName"].ToString();

			uiPanelFiles.Text += " : " + keyFileStateName;
			if( keyFileState.Equals("10") )
			{
				lblMsg.Text	= "�̵�ϻ��´� ���Ϲ����� �� �� �����ϴ�. �������ϰ������� ������ ����Ͻʽÿ�.";
			}
			else if( keyFileState.Equals("12") )
			{
				lblMsg.Text	= "�˼��Ϸ�� ������ ���������·� ����Ǿ��ٰ�, CDN����ȭ�� �Ϸ�Ǹ� �ڵ����� CDN����ȭ ���·� ����˴ϴ�.";
			}
			else if( keyFileState.Equals("15") )
			{
				lblMsg.Text	= "CMS�� ��û���� �����̸�, CMS���� ������ ��ٸ��� �ִ� ���Դϴ�. ��� ��ٷ� �ֽʽÿ�";
			}
			else if( keyFileState.Equals("20") )
			{
				lblMsg.Text	= "CDN����ȭ ������ ������ �ֱ������� �����Ϸ���·� ����˴ϴ�. ��� ��ٷ� �ֽʽÿ�";
			}
			else if( keyFileState.Equals("30") )
			{
				lblMsg.Text	= "�����غ� �Ϸ�� ���ϵ� �Դϴ�. ���� �������� ���� ������ Blue������ ǥ�� �˴ϴ�.";
			}
			else
			{
				lblMsg.Text	= "";
			}

			// ���������� ��ȸ�Ѵ�.
			SearchAdFile();
		}

		/// <summary>
		/// �������Ϲ��� ��ȸ
		/// </summary>
		private void SearchAdFile()
		{
			StatusMessage("�������� ������ ��ȸ�մϴ�.");
            Debug.WriteLine("���������ȸ");

			try
			{
				adFileWideModel.Init();
				adFileWideDs.AdFileWide.Clear();

				adFileWideModel.SearchMediaCode		 =  keyMediaCode; 
				adFileWideModel.SearchMediaCode		 =  "1"; 

				adFileWideModel.SearchFileState		 =  keyFileState; 
				adFileWideModel.SearchKey			 =  keySearchKey; 
				
				adFileWideModel.SearchchkAdState_10 = keychkAdState_10;
				adFileWideModel.SearchchkAdState_20 = keychkAdState_20;
				adFileWideModel.SearchchkAdState_30 = keychkAdState_30;
				adFileWideModel.SearchchkAdState_40 = keychkAdState_40;


				// �������Ϲ�����ȸ ���񽺸� ȣ���Ѵ�.
				new AdFileWideManager(systemModel,commonModel).GetAdFileWideList(adFileWideModel);

				if (adFileWideModel.ResultCD.Equals("0000"))
				{
					Utility.SetDataTable(adFileWideDs.AdFileWide, adFileWideModel.FileDataSet);		
					StatusMessage(adFileWideModel.ResultCnt + "���� �������� ������ ��ȸ�Ǿ����ϴ�.");

					grdExAdFileWideList.UnCheckAllRecords();

					AddSchChoiceFile();

					if(canUpdate)
					{

						// �⺻�� �����Ұ�, ���κҰ�
						chkCMS.Enabled					= false;
						chkTestCheck.Enabled			= false;
						btnChkComplete.Enabled			= false;
						btnChkCompleteCancel.Enabled	= false;
						btnCDNSync.Enabled				= false;
						btnCDNSyncCancel.Enabled		= false;
						btnCDNPublish.Enabled			= false;
						btnCDNPublishCancel.Enabled		= false;
						btnSTBDelete.Enabled			= false;
						btnSTBDeleteCancel.Enabled		= false;

						// ��ȸ�ϴ� ������ ���¿� ���� ó����ư Ȱ��ȭ
						if(keyFileState.Equals("10"))	// ���ϻ��°� 10:�̵�� �̸� ó���Ұ��� ����.
						{
							lbMsg.Text = "���ϵ�� ������Դϴ�.";
						}
						if(keyFileState.Equals("11"))	// ���ϻ��°� 11:���米ü���
						{
							lbMsg.Text = "�������ϱ�ü ������Դϴ�.";
						}
						if(keyFileState.Equals("12"))	// ���ϻ��°� 12:�˼���� �̸� �˼��Ϸ� ��ư Ȱ��ȭ
						{
							lbMsg.Text = "�˼��Ϸ� ������Դϴ�.";

							chkCMS.Enabled			= true;
							chkTestCheck.Enabled	= true;
							btnChkComplete.Enabled	= true;
						}
						if(keyFileState.Equals("15"))	// ���ϻ��°� 15:������� �̸� CDN����Ȯ�� �� �˼��Ϸ���� ��ư Ȱ��ȭ
						{
							lbMsg.Text = "CDN����ȭ ������Դϴ�.";

							// �����̵���û�� ����� ���� ������� ���Ѵ�.
							if(!FtpMoveUseYn.Equals("Y"))
							{
								btnChkCompleteCancel.Enabled	= true;
							}
							btnCDNSync.Enabled				= true;
						}
						if(keyFileState.Equals("20"))	// ���ϻ��°� 20:CDN����ȭ �̸� CDN����Ȯ�� �� CDN����Ȯ����� ��ư Ȱ��ȭ
						{
							lbMsg.Text = "CDN����Ȯ�� ������Դϴ�.";

							btnCDNSyncCancel.Enabled		= true;
							btnCDNPublish.Enabled			= true;
						}
						if(keyFileState.Equals("30"))	// ���ϻ��°� 30:�����Ϸ� �̸� ��ž���� �� ����Ȯ����� ��ư Ȱ��ȭ
						{
							lbMsg.Text = "CDN���� �Ϸ�Ǿ����ϴ�.";

							btnCDNPublishCancel.Enabled		= true;
							btnSTBDelete.Enabled			= true;
						}				
						if(keyFileState.Equals("90"))	// ���ϻ��°� 90:��ž���� �̸� ��ž������� ��ư Ȱ��ȭ
						{
							lbMsg.Text = "��ž������ �����Դϴ�.";

							btnSTBDeleteCancel.Enabled			= true;
						}

						btnExcel.Enabled = true;	
					}	
					else
					{
						btnExcel.Enabled = false;	
					}
					// ����Ȳ Ŭ����
					adFileWideDs.AdSchedule.Clear();
					grdExAdStatusList.EnsureVisible();
					//uiPanelSchedule.Text = "������";
					
					// �������� �������� ��Ʈ		
					//SetDetailTextSchedule();


				}
			}
			catch(FrameException fe)
			{
				FrameSystem.showMsgForm("����������ȸ����", new string[] {fe.ErrCode, fe.ResultMsg});
			}
			catch(Exception ex)
			{
				FrameSystem.showMsgForm("����������ȸ����",new string[] {"",ex.Message});
			}
		}

		/// <summary>
		/// �������� �������� ��Ʈ
		/// </summary>
		private void SetDetailTextSchedule()
		{
			int curRow = cmFile.Position;

			if(curRow < 0) return;	// �����Ͱ� ������ �������� �ʴ´�.

			keyItemNo          = dtFile.Rows[curRow]["ItemNo"].ToString();

			//uiPanelSchedule.Text = "������ : [" +  keyItemNo + "]" + dtFile.Rows[curRow]["ItemName"].ToString();

			// �ش籤���� ����Ȳ�� ��ȸ�Ѵ�.
			SearchItemSchedule();
		}


		/// <summary>
		/// Ű����ã�� �׸��� Ű�� �ش�Ǵ·ο��..
		/// </summary>
		private void AddSchChoiceFile()
		{
			StatusMessage("Ű��");		

			try
			{
				int rowIndex = 0;
				if ( dtFile.Rows.Count < 1 ) return;
              
				foreach (DataRow row in dtFile.Rows)
				{					
				
					if(row["ItemNo"].ToString().Equals(keyItemNo))
					{					
						cmFile.Position = rowIndex;
						break;								
					}
					rowIndex++;
				}

				grdExAdFileWideList.EnsureVisible();
			}
			catch(FrameException fe)
			{
				FrameSystem.showMsgForm("Ű������", new string[] {fe.ErrCode, fe.ResultMsg});
			}
			catch(Exception ex)
			{
				FrameSystem.showMsgForm("Ű������",new string[] {"",ex.Message});
			}			
		}



		/// <summary>
		/// �������� ����Ȳ ��ȸ
		/// </summary>
		private void SearchItemSchedule()
		{
            Debug.WriteLine("����Ȳ ��ȸ");
			try
			{
				adFileWideModel.Init();
				adFileWideDs.AdSchedule.Clear();

				adFileWideModel.SearchMediaCode		 =  keyMediaCode; 
				adFileWideModel.ItemNo  			 =  keyItemNo; 
				
				// �������Ϲ�����ȸ ���񽺸� ȣ���Ѵ�.
				new AdFileWideManager(systemModel,commonModel).GetAdFileSchedule(adFileWideModel);

				if (adFileWideModel.ResultCD.Equals("0000"))
				{
					Utility.SetDataTable(adFileWideDs.AdSchedule, adFileWideModel.ScheduleDataSet);		
				}
			}
			catch(FrameException fe)
			{
				FrameSystem.showMsgForm("�������� ����Ȳ ��ȸ����", new string[] {fe.ErrCode, fe.ResultMsg});
			}
			catch(Exception ex)
			{
				FrameSystem.showMsgForm("�������� ����Ȳ ��ȸ����",new string[] {"",ex.Message});
			}
		}


		#region [���ϻ��� �����۾� �Լ��� ]
		/// <summary>
		/// ���õȱ��������� �˼���û
		/// </summary>
		private void SetAdFileChkRequest()
		{
			StatusMessage("���õ� ���������� �˼���û�մϴ�.");

			// �׸��忡 ����� �����͸� Datasource�� ������Ʈ �Ѵ�.
			grdExAdFileWideList.UpdateData();

			ProgressStart();

			try
			{				
				int rc = 0;
				for(int i=0;i < adFileWideDs.AdFileWide.Rows.Count;i++)
				{
					DataRow row = adFileWideDs.AdFileWide.Rows[i];					
					if(row["CheckYn"].ToString().Equals("True"))
					{
						rc++;
						adFileWideModel.Init();

						adFileWideModel.MediaCode   = keyMediaCode;
						adFileWideModel.ItemNo      = row["ItemNo"].ToString();
						adFileWideModel.ItemName	= row["ItemName"].ToString();						
						adFileWideModel.FileName	= row["FileName"].ToString();	
						
						string Path = row["FilePath"].ToString();	

						// TEST FTP�� �ִ��� �˻��Ѵ�. ������ ������� ���·� �����Ѵ�.
						if(checkTESTFile(Path,adFileWideModel.FileName))
						{
							new AdFileWideManager(systemModel,commonModel).SetAdFileChkRequest(adFileWideModel);
						}
						else
						{
							ProgressStop();

							keyItemNo          = row["ItemNo"].ToString();

							MessageBox.Show("�׽�Ʈ������ ����[" + Path + "/" + adFileWideModel.FileName + "]�� �������� �ʽ��ϴ�.\n���ϸ� �� ��θ� Ȯ���� �ֽʽÿ�.","�������� ��������", 
								MessageBoxButtons.OK, MessageBoxIcon.Information );
							break;
						}						
					}
				}
				ProgressStop();

				if( rc == 0 )
				{
					ProgressStop();
					MessageBox.Show("���õ� ���������� �����ϴ�.","�������� ��������", 
						MessageBoxButtons.OK, MessageBoxIcon.Information );
					return;
				}

			
				DisableButton();
				SearchFileCount();
				InitButton();

			}
			catch(FrameException fe)
			{
				FrameSystem.showMsgForm("�������� �˼���û����", new string[] {fe.ErrCode, fe.ResultMsg});
			}
			catch(Exception ex)
			{
				FrameSystem.showMsgForm("�������ϰ˼���û����",new string[] {"",ex.Message});
			}
			finally
			{
				ProgressStop();
			}			
		}


		/// <summary>
		/// ���õȱ��������� �˼���û ���
		/// </summary>
		private void SetAdFileChkRequestCancel()
		{
			StatusMessage("���õ� ���������� �˼���û ����մϴ�.");

			// �׸��忡 ����� �����͸� Datasource�� ������Ʈ �Ѵ�.
			grdExAdFileWideList.UpdateData();

			ProgressStart();

			try
			{				
				int rc = 0;
				for(int i=0;i < adFileWideDs.AdFileWide.Rows.Count;i++)
				{
					rc++;
					DataRow row = adFileWideDs.AdFileWide.Rows[i];					
					if(row["CheckYn"].ToString().Equals("True"))
					{
						adFileWideModel.Init();

						adFileWideModel.MediaCode   = keyMediaCode;
						adFileWideModel.ItemNo      = row["ItemNo"].ToString();
						adFileWideModel.ItemName	= row["ItemName"].ToString();						
						adFileWideModel.FileName	= row["FileName"].ToString();											

						new AdFileWideManager(systemModel,commonModel).SetAdFileChkRequestCancel(adFileWideModel);
					}
				}
				ProgressStop();

				if(rc == 0) 
				{
					MessageBox.Show("���õ� ���������� �����ϴ�.","�������� ��������", 
						MessageBoxButtons.OK, MessageBoxIcon.Information );
					return;
				}
			
				DisableButton();
				SearchFileCount();
				InitButton();

			}
			catch(FrameException fe)
			{
				FrameSystem.showMsgForm("�������� �˼���û ��ҿ���", new string[] {fe.ErrCode, fe.ResultMsg});
			}
			catch(Exception ex)
			{
				FrameSystem.showMsgForm("�������ϰ˼���û ��ҿ���",new string[] {"",ex.Message});
			}			
			finally
			{
				ProgressStop();
			}			
		}


		/// <summary>
		/// 1. ���õȱ��������� �˼��Ϸ� ó���Ѵ�.
		/// </summary>
		private void SetAdFileChkComplete()
		{
			DialogResult result = MessageBox.Show("������ ���ϵ��� �˼��Ϸ� �մϴ�\n"
													+ "\n�ش����ϵ��� ���ε�� CDN���� ���� ��û�Ǹ�, ��û�� ������ ��ҵɼ� �����ϴ�\t"
													+ "\n�˼��� ���ϵ��� ���������·� �̵��ϸ�"
													+ "\nCDN���� ������ �Ϸ�Ǹ� CDN����ȭ ���·� �ڵ�����˴ϴ�"
													,"�����������"
													,MessageBoxButtons.YesNo
													,MessageBoxIcon.Question
													,MessageBoxDefaultButton.Button2);

			if (result == DialogResult.No)
			{
				StatusMessage("����˼� ���!!!");
				return;
			}

			if( chkCMS.Checked == false )
			{
				DialogResult result2 = MessageBox.Show("CDN���Ϲ��� �ɼ��� [����] �Ǿ� �ֽ��ϴ�\n"
					+ "\n�������� CDN Master���� ������ CDN������ �������� ó���ؾ� �մϴ�.\t"
					,"�����������"
					,MessageBoxButtons.YesNo
					,MessageBoxIcon.Question
					,MessageBoxDefaultButton.Button2);

				if (result2 == DialogResult.No)
				{
					StatusMessage("����˼� ���!!!");
					return;
				}
			
			}

			StatusMessage("���õ� ���������� �˼�Ȯ�δϴ�.");
			Application.DoEvents();

			// �׸��忡 ����� �����͸� Datasource�� ������Ʈ �Ѵ�.
			grdExAdFileWideList.UpdateData();
			ProgressStart();
			try
			{				
				int rc = 0;
				string	sysTm = Convert.ToString( DateTime.Now.Ticks / 10000 );

				for(int i=0;i < adFileWideDs.AdFileWide.Rows.Count;i++)
				{
					DataRow row = adFileWideDs.AdFileWide.Rows[i];					
					if(row["CheckYn"].ToString().Equals("True"))
					{
						adFileWideModel.Init();
						adFileWideModel.MediaCode   = keyMediaCode;
						adFileWideModel.ItemNo      = row["ItemNo"].ToString();
						adFileWideModel.ItemName	= row["ItemName"].ToString();						
						adFileWideModel.FileName	= row["FileName"].ToString();	
						
						string	Path		= row["FilePath"].ToString();
						bool	fileFound	= false;


						// �׽�Ʈ�˼������� ���������� �ִ��� Ȯ���ϴ� ýũ�� �Ǿ� �ִ°�쿡�� Ȯ���Ѵ�.
						// �� �׽�Ʈ�� �����ؼ� �������. ���忡�� ýũ�ϰ� ó���ϴ°� ����.
						if ( chkTestCheck.Checked )	fileFound	= checkTESTFile(Path,adFileWideModel.FileName);
						else						fileFound	= true;

						if( fileFound )
						{
							#region [ CMSȣ�� ]
							string	cmsCid		= "";
							string  cmsFileList = "";
							string	cmsCall		= "";

							// Checked�� ��쿡�� CMS�������̽��� ȣ���Ѵ�.
							if( chkCMS.Checked == true )
							{
								// �׽�Ʈ�������� �����н��� ������� �ʰ�, ��Ʈ�� ����Ѵ�.
								// CMS���� /contents/dcdn�� /�� �����ؼ� �߻��� ������
								// ����Ȯ�νÿ� ������ ����ؾ� �Ѵ�.
								// CMS���� �����ý��ۿ� �����ؾ� �Ѵٰ�..�����ϴ��� �׳� ����� ��
//								if( FrameSystem.m_ClientType == FrameSystem._TEST )
//								{
//									Path = "/";
//								}

								cmsCid		= "adv_" + adFileWideModel.FileName.Substring(0, adFileWideModel.FileName.LastIndexOf(".")) + "V" + sysTm;
								cmsFileList = Path + "/" + adFileWideModel.FileName + "|";
								cmsCall		= RequestCMS( mCmsMasUrl, mCmsMasQuery, cmsCid, cmsFileList);

								adFileWideModel.FilePath		= Path;
								adFileWideModel.CmsCid			= cmsCid;
								adFileWideModel.CmsCmd			= "UPLOAD_CDN";
								adFileWideModel.CmsRequestStatus= cmsCall.Trim();
								adFileWideModel.CmsProcessStatus= "0";
								adFileWideModel.CmsSyncCount	=  0;
								adFileWideModel.CmsDescCount	=  0;
							}
							else
							{
								cmsCall = "1";
								adFileWideModel.CmsCid	= "0000";
							}
							#endregion

							#region [ �������� ���� ���� ]

							// CDN���ε�� ����ȭ�۾��� �����ϰ� ��û�۾��� �����ϸ� 1, ���нÿ� 2�� ���ϵ�
							if( cmsCall.Trim().Equals("1") )
							{
								rc++;
								new AdFileWideManager(systemModel,commonModel).SetAdFileChkComplete(adFileWideModel);

								// �̵���û����� ��� �ش������� �̵���û��ġ�� �̵���Ų��.
								// �׽�Ʈ���忡�� �ʿ��� ����ε�..��� ���ǰ�?
								if(FtpMoveUseYn.Equals("Y"))	moveTESTFile(Path, adFileWideModel.FileName, FtpMovePath);
							}
							else
							{
								ProgressStop();
								keyItemNo = row["ItemNo"].ToString();
								MessageBox.Show(keyItemNo + "[" + adFileWideModel.FileName + "] ���� ó����\n\n"
									+ "CMS �������̽� ���������� ȣ������ڵ�(" + cmsCall + ")�� �����Ͽ����ϴ�."
									+ "�����ߴ�� Ȥ�� CMS���� �Ŵ������� �����Ͻʽÿ�"
									,"�������� ��������"
									,MessageBoxButtons.OK
									,MessageBoxIcon.Error );
								break;
							}
							#endregion
						}
						else
						{
							ProgressStop();
							keyItemNo = row["ItemNo"].ToString();
							MessageBox.Show("�׽�Ʈ������ ����[" + Path + "/" + adFileWideModel.FileName + "]�� �������� �ʽ��ϴ�.\n���ϸ� �� ��θ� Ȯ���� �ֽʽÿ�.","�������� ��������", 
								MessageBoxButtons.OK, MessageBoxIcon.Information );
							break;
						}
					}
				}
				ProgressStop();

				if(rc == 0) 
				{
					MessageBox.Show("���õ� ���������� �����ϴ�.","�������� ��������", 
						MessageBoxButtons.OK, MessageBoxIcon.Information );
					return;
				}
			
				DisableButton();
				SearchFileCount();
				InitButton();

			}
			catch(FrameException fe)
			{
				FrameSystem.showMsgForm("�������� �˼�Ȯ�� ����", new string[] {fe.ErrCode, fe.ResultMsg});
			}
			catch(Exception ex)
			{
				FrameSystem.showMsgForm("�������� �˼�Ȯ�� ����",new string[] {"",ex.Message});
			}	
			finally
			{
				ProgressStop();
			}			
		
		}


		/// <summary>
		/// ���õȱ��������� �˼�Ȯ�� ���
		/// </summary>
		private void SetAdFileChkCompleteCancel()
		{
			StatusMessage("���õ� ���������� �˼�Ȯ�� ����մϴ�.");

			DialogResult result = MessageBox.Show("������ ���ϵ��� �˼���� �մϴ�\n"
				+ "\nCMS����ó���� ������ ����Ͽ���, �����۾��� �״�� ����˴ϴ�.\t"
				+ "\n�˼��ּҵ� ���ϵ��� �˼���� ���·� ����˴ϴ�."
				,"�����������"
				,MessageBoxButtons.YesNo
				,MessageBoxIcon.Question
				,MessageBoxDefaultButton.Button2);

			if (result == DialogResult.No)
			{
				StatusMessage("�˼���� ���!!!");
				return;
			}

			grdExAdFileWideList.UpdateData();

			ProgressStart();
			try
			{				
				int rc = 0;
				for(int i=0;i < adFileWideDs.AdFileWide.Rows.Count;i++)
				{
					rc++;
					DataRow row = adFileWideDs.AdFileWide.Rows[i];					
					if(row["CheckYn"].ToString().Equals("True"))
					{
						adFileWideModel.Init();

						adFileWideModel.MediaCode   = keyMediaCode;
						adFileWideModel.ItemNo      = row["ItemNo"].ToString();
						adFileWideModel.ItemName	= row["ItemName"].ToString();						
						adFileWideModel.FileName	= row["FileName"].ToString();											

						new AdFileWideManager(systemModel,commonModel).SetAdFileChkCompleteCancel(adFileWideModel);
					}
				}
				ProgressStop();

				if(rc == 0) 
				{
					MessageBox.Show("���õ� ���������� �����ϴ�.","�������� ��������", MessageBoxButtons.OK, MessageBoxIcon.Information );
					return;
				}
			
				DisableButton();
				SearchFileCount();
				InitButton();

			}
			catch(FrameException fe)
			{
				FrameSystem.showMsgForm("�������� �˼�Ȯ�� ��� ����", new string[] {fe.ErrCode, fe.ResultMsg});
			}
			catch(Exception ex)
			{
				FrameSystem.showMsgForm("�������� �˼�Ȯ�� ��� ����",new string[] {"",ex.Message});
			}			
			finally
			{
				ProgressStop();
			}			
		}


		/// <summary>
		/// ���õȱ������� CDN����Ȯ��
		/// </summary>
		private void SetAdFileCDNSync()
		{
			StatusMessage("���õ� ���������� CDN���⸦ Ȯ���մϴ�.");

			DialogResult result = MessageBox.Show("������ ���ϵ��� CDN����ȭ Ȯ��ó�� �մϴ�\n"
				+ "\nCMS����ó���� ���ϵ��� CMS���� �ڵ����� ����ȭ �Ǹ�\t"
				+ "\n�����ÿ� CDN����ȭ ���·�, ���н� �˼������·� ��ȯ�˴ϴ�."
				+ "\n����ó���� ���� ���� ���ϵ鸸 �������� ó���Ͻñ� �ٶ��ϴ�."
				,"�����������"
				,MessageBoxButtons.YesNo
				,MessageBoxIcon.Question
				,MessageBoxDefaultButton.Button2);

			if (result == DialogResult.No)
			{
				StatusMessage("CDN����Ȯ�� ���!!!");
				return;
			}

			// �׸��忡 ����� �����͸� Datasource�� ������Ʈ �Ѵ�.
			grdExAdFileWideList.UpdateData();

			ProgressStart();
			try
			{				

				int rc = 0;
				for(int i=0;i < adFileWideDs.AdFileWide.Rows.Count;i++)
				{
					DataRow row = adFileWideDs.AdFileWide.Rows[i];					
					if(row["CheckYn"].ToString().Equals("True"))
					{
						rc++;
						adFileWideModel.Init();
						adFileWideModel.MediaCode   = keyMediaCode;
						adFileWideModel.ItemNo      = row["ItemNo"].ToString();
						adFileWideModel.ItemName	= row["ItemName"].ToString();						
						adFileWideModel.FileName	= row["FileName"].ToString();						

						new AdFileWideManager(systemModel,commonModel).SetAdFileCDNSync(adFileWideModel);
					}
				}
				ProgressStop();

				if( rc == 0 )
				{
					ProgressStop();
					MessageBox.Show("���õ� ���������� �����ϴ�.","�������� ��������", 
						MessageBoxButtons.OK, MessageBoxIcon.Information );
					return;
				}

			
				DisableButton();
				SearchFileCount();
				InitButton();			

			}
			catch(FrameException fe)
			{
				FrameSystem.showMsgForm("�������� CDN����Ȯ�� ����", new string[] {fe.ErrCode, fe.ResultMsg});
			}
			catch(Exception ex)
			{
				FrameSystem.showMsgForm("�������� ����Ȯ�� ����",new string[] {"",ex.Message});
			}		
			finally
			{
				ProgressStop();
			}
			
		}


		/// <summary>
		/// ���õȱ������� CDN����Ȯ�� ���
		/// </summary>
		private void SetAdFileCDNSyncCancel()
		{
			StatusMessage("���õ� ���������� CDN����Ȯ���� ����մϴ�.");

			DialogResult result = MessageBox.Show("������ ���ϵ��� CDN����ȭ [���]ó�� �մϴ�\n"
				,"�����������"
				,MessageBoxButtons.YesNo
				,MessageBoxIcon.Question
				,MessageBoxDefaultButton.Button2);

			if (result == DialogResult.No)
			{
				StatusMessage("CDN������� �۾����!!!");
				return;
			}

			// �׸��忡 ����� �����͸� Datasource�� ������Ʈ �Ѵ�.
			grdExAdFileWideList.UpdateData();

			ProgressStart();
			try
			{				

				int rc = 0;
				for(int i=0;i < adFileWideDs.AdFileWide.Rows.Count;i++)
				{
					DataRow row = adFileWideDs.AdFileWide.Rows[i];					
					if(row["CheckYn"].ToString().Equals("True"))
					{
						rc++;
						adFileWideModel.Init();

						adFileWideModel.MediaCode   = keyMediaCode;
						adFileWideModel.ItemNo      = row["ItemNo"].ToString();
						adFileWideModel.ItemName	= row["ItemName"].ToString();						
						adFileWideModel.FileName	= row["FileName"].ToString();						

						new AdFileWideManager(systemModel,commonModel).SetAdFileCDNSyncCancel(adFileWideModel);
					}
				}
				ProgressStop();

				if( rc == 0 )
				{
					ProgressStop();
					MessageBox.Show("���õ� ���������� �����ϴ�.","�������� ��������", 
						MessageBoxButtons.OK, MessageBoxIcon.Information );
					return;
				}

			
				DisableButton();
				SearchFileCount();
				InitButton();			

			}
			catch(FrameException fe)
			{
				FrameSystem.showMsgForm("�������� CDN����Ȯ�� ��� ����", new string[] {fe.ErrCode, fe.ResultMsg});
			}
			catch(Exception ex)
			{
				FrameSystem.showMsgForm("�������� ����Ȯ�� ��� ����",new string[] {"",ex.Message});
			}		
			finally
			{
				ProgressStop();
			}
			
		}


		/// <summary>
		/// ���õȱ������� CDN����Ȯ��
		/// </summary>
		private void SetAdFileCDNPublish()
		{
			StatusMessage("���õ� ���������� CDN����Ȯ���մϴ�.");

			DialogResult result = MessageBox.Show("������ ���ϵ��� �����Ϸ� ó�� �մϴ�\n"
				+ "\nCDN����ȭ ������ ���ϵ��� �ý��ۿ� ���� �ֱ������� �ڵ�[�����Ϸ�]ó���˴ϴ�\t"
				+ "\n����ڰ� ���Ƿ� �����Ϸ� ó���Ͻ� ��쿡 �����Ͻñ� �ٶ��ϴ�."
				,"�����������"
				,MessageBoxButtons.YesNo
				,MessageBoxIcon.Question
				,MessageBoxDefaultButton.Button2);

			if (result == DialogResult.No)
			{
				StatusMessage("");
				return;
			}

			// �׸��忡 ����� �����͸� Datasource�� ������Ʈ �Ѵ�.
			grdExAdFileWideList.UpdateData();

			DataRow[] foundRows = adFileWideDs.AdFileWide.Select("CheckYn = 'True'");

			if(foundRows.Length == 0 )
			{
				MessageBox.Show("���õ� ���������� �����ϴ�.","�������� ��������", 
					MessageBoxButtons.OK, MessageBoxIcon.Information );
				return;
			}


			if(FILEMAX < (FileListCnt + foundRows.Length)) 
			{
				MessageBox.Show("�������� �����ѵ��� �ʰ��Ͽ����ϴ�.\n\nȨ������ �Ǽ��� ���ϸ���Ʈ �������� ���� "
					           + FILEMAX.ToString() + "���� �ʰ��� �� �����ϴ�" ,"�������� ��������", 
					MessageBoxButtons.OK, MessageBoxIcon.Information );

				return;
			}


			ProgressStart();
			try
			{				

				int rc = 0;
				for(int i=0;i < adFileWideDs.AdFileWide.Rows.Count;i++)
				{
					DataRow row = adFileWideDs.AdFileWide.Rows[i];					
					if(row["CheckYn"].ToString().Equals("True"))
					{
						rc++;
						adFileWideModel.Init();

						adFileWideModel.MediaCode   = keyMediaCode;
						adFileWideModel.ItemNo      = row["ItemNo"].ToString();
						adFileWideModel.ItemName	= row["ItemName"].ToString();						
						adFileWideModel.FileName	= row["FileName"].ToString();						

						string Path = row["FilePath"].ToString();	

						// CDN�� �ִ��� �˻��Ѵ�. ������ CDN�����Ϸ� ���·� �����Ѵ�.
						// CMS�����ÿ��� �����ؾ� �ϴ��� Ȯ���� �ʿ��ϴ�

						if(checkCDNFile(Path,adFileWideModel.FileName))
						{					
							new AdFileWideManager(systemModel,commonModel).SetAdFileCDNPublish(adFileWideModel);
						}
						else
						{
							ProgressStop();

							keyItemNo          = row["ItemNo"].ToString();

							MessageBox.Show("CDN������ ����[" + Path + "/" + adFileWideModel.FileName + "]�� �������� �ʽ��ϴ�.\n���ϸ� �� ��θ� Ȯ���� �ֽʽÿ�.","�������� ��������", 
								MessageBoxButtons.OK, MessageBoxIcon.Information );
							break;
						}
					}
				}
				ProgressStop();
		
				DisableButton();
				SearchFileCount();
				InitButton();			

			}
			catch(FrameException fe)
			{
				FrameSystem.showMsgForm("�������� ��������", new string[] {fe.ErrCode, fe.ResultMsg});
			}
			catch(Exception ex)
			{
				FrameSystem.showMsgForm("�������� ��������",new string[] {"",ex.Message});
			}		
			finally
			{
				ProgressStop();
			}
			
		}


		/// <summary>
		/// ���õȱ��������� CDN����Ȯ�� ���
		/// </summary>
		private void SetAdFileCDNPublishCancel()
		{
			StatusMessage("���õ� ���������� CDN����Ȯ�� ����մϴ�.");

			DialogResult result = MessageBox.Show("������ ���ϵ��� ������� ó�� �մϴ�\n"
				+ "\n��ҵ� ���ϵ��� CDN����ȭ ���°� �˴ϴ�"
				,"�����������"
				,MessageBoxButtons.YesNo
				,MessageBoxIcon.Question
				,MessageBoxDefaultButton.Button2);

			if (result == DialogResult.No)
			{
				StatusMessage("CDN����Ȯ�� ���!!!");
				return;
			}

			// �׸��忡 ����� �����͸� Datasource�� ������Ʈ �Ѵ�.
			grdExAdFileWideList.UpdateData();

			ProgressStart();
			try
			{				
				int rc = 0;
				for(int i=0;i < adFileWideDs.AdFileWide.Rows.Count;i++)
				{ 
					rc++;
					DataRow row = adFileWideDs.AdFileWide.Rows[i];					
					if(row["CheckYn"].ToString().Equals("True"))
					{
						adFileWideModel.Init();

						adFileWideModel.MediaCode   = keyMediaCode;
						adFileWideModel.ItemNo      = row["ItemNo"].ToString();
						adFileWideModel.ItemName	= row["ItemName"].ToString();						
						adFileWideModel.FileName	= row["FileName"].ToString();						
						
						new AdFileWideManager(systemModel,commonModel).SetAdFileCDNPublishCancel(adFileWideModel);
					}
				}
				ProgressStop();

				if(rc == 0) 
				{
					MessageBox.Show("���õ� ���������� �����ϴ�.","�������� ��������", 
						MessageBoxButtons.OK, MessageBoxIcon.Information );
					return;
				}
			
				DisableButton();
				SearchFileCount();
				InitButton();

			}
			catch(FrameException fe)
			{
				FrameSystem.showMsgForm("�������� CDN����Ȯ�� ��� ����", new string[] {fe.ErrCode, fe.ResultMsg});
			}
			catch(Exception ex)
			{
				FrameSystem.showMsgForm("�������� CDN����Ȯ�� ��� ����",new string[] {"",ex.Message});
			}			
			finally
			{
				ProgressStop();
			}			
		}


		/// <summary>
		/// ���õȱ������� ��ž����
		/// </summary>
		private void SetAdFileSTBDelete()
		{
			StatusMessage("���õ� ���������� ��ž�����մϴ�.");

			DialogResult result = MessageBox.Show("������ ���ϵ��� ��ž���� ó���մϴ�\n"
				,"�����������"
				,MessageBoxButtons.YesNo
				,MessageBoxIcon.Question
				,MessageBoxDefaultButton.Button2);

			if (result == DialogResult.No)
			{
				StatusMessage("");
				return;
			}

			// �׸��忡 ����� �����͸� Datasource�� ������Ʈ �Ѵ�.
			grdExAdFileWideList.UpdateData();

			ProgressStart();
			try
			{				
				int rc = 0;
				for(int i=0;i < adFileWideDs.AdFileWide.Rows.Count;i++)
				{
					rc++;
					DataRow row = adFileWideDs.AdFileWide.Rows[i];					
					if(row["CheckYn"].ToString().Equals("True"))
					{
						adFileWideModel.Init();

						adFileWideModel.MediaCode   = keyMediaCode;
						adFileWideModel.ItemNo		= row["ItemNo"].ToString();						
						adFileWideModel.ItemName	= row["ItemName"].ToString();						
						adFileWideModel.FileName	= row["FileName"].ToString();		
				
						new AdFileWideManager(systemModel,commonModel).SetAdFileSTBDelete(adFileWideModel);						
					}
				}
				ProgressStop();

				if(rc == 0) 
				{
					MessageBox.Show("���õ� ���������� �����ϴ�.","�������� ��������", 
						MessageBoxButtons.OK, MessageBoxIcon.Information );
					return;
				}
			
                DisableButton();		
                SearchFileCount();
                InitButton();

            }
			catch(FrameException fe)
			{
				FrameSystem.showMsgForm("�������� ��ž���� ����", new string[] {fe.ErrCode, fe.ResultMsg});
			}
			catch(Exception ex)
			{
				FrameSystem.showMsgForm("�������� ��ž���� ����",new string[] {"",ex.Message});
			}			
			finally
			{
				ProgressStop();
			}			
		}	
		

		/// <summary>
		/// ���õȱ������� ��ž���� ���
		/// </summary>
		private void SetAdFileSTBDeleteCancel()
		{
			StatusMessage("���õ� ���������� ��ž���� ����մϴ�.");

			// �׸��忡 ����� �����͸� Datasource�� ������Ʈ �Ѵ�.
			grdExAdFileWideList.UpdateData();

			ProgressStart();
			try
			{				
				int rc = 0;
				for(int i=0;i < adFileWideDs.AdFileWide.Rows.Count;i++)
				{
					rc++;
					DataRow row = adFileWideDs.AdFileWide.Rows[i];					
					if(row["CheckYn"].ToString().Equals("True"))
					{
						adFileWideModel.Init();

						adFileWideModel.MediaCode   = keyMediaCode;
						adFileWideModel.ItemNo		= row["ItemNo"].ToString();						
						adFileWideModel.ItemName	= row["ItemName"].ToString();						
						adFileWideModel.FileName	= row["FileName"].ToString();						

						new AdFileWideManager(systemModel,commonModel).SetAdFileSTBDeleteCancel(adFileWideModel);						
					}
				}
				ProgressStop();

				if(rc == 0) 
				{
					MessageBox.Show("���õ� ���������� �����ϴ�.","�������� ��������", 
						MessageBoxButtons.OK, MessageBoxIcon.Information );
					return;
				}
			
				DisableButton();		
				SearchFileCount();
				InitButton();
			}
			catch(FrameException fe)
			{
				FrameSystem.showMsgForm("�������� ��ž���� ��� ����", new string[] {fe.ErrCode, fe.ResultMsg});
			}
			catch(Exception ex)
			{
				FrameSystem.showMsgForm("�������� ��ž���� ��� ����",new string[] {"",ex.Message});
			}			
			finally
			{
				ProgressStop();
			}			
		}	


		#region [�̻�� �޼ҵ� ]
		/// <summary>
		/// ���õȱ������� ���米ü
		/// 2010/09�� ������� �ʴ� ������. �������ϰ����� �����̰�
		/// </summary>
//		private void SetAdFileChange()
//		{
//			StatusMessage("���õ� ���������� ���米ü�մϴ�.");
//
//			// �׸��忡 ����� �����͸� Datasource�� ������Ʈ �Ѵ�.
//			grdExAdFileWideList.UpdateData();
//
//			ProgressStart();
//			try
//			{				
//				int rc = 0;
//				for(int i=0;i < adFileWideDs.AdFileWide.Rows.Count;i++)
//				{
//					rc++;
//					DataRow row = adFileWideDs.AdFileWide.Rows[i];					
//					if(row["CheckYn"].ToString().Equals("True"))
//					{
//						adFileWideModel.Init();
//
//						adFileWideModel.MediaCode   = keyMediaCode;
//						adFileWideModel.ItemNo		= row["ItemNo"].ToString();						
//						adFileWideModel.ItemName	= row["ItemName"].ToString();						
//						adFileWideModel.FileName	= row["FileName"].ToString();			
//						adFileWideModel.FileState   = row["FileState"].ToString();			
//
//						new AdFileWideManager(systemModel,commonModel).SetAdFileChange(adFileWideModel);						
//
//					}
//				}
//
//				ProgressStop();
//
//				if(rc == 0) 
//				{
//					MessageBox.Show("���õ� ���������� �����ϴ�.","�������� ��������", 
//						MessageBoxButtons.OK, MessageBoxIcon.Information );
//					return;
//				}
//			
//				DisableButton();		
//				SearchFileCount();
//				InitButton();
//			}
//			catch(FrameException fe)
//			{
//				FrameSystem.showMsgForm("�������� ���米ü ����", new string[] {fe.ErrCode, fe.ResultMsg});
//			}
//			catch(Exception ex)
//			{
//				FrameSystem.showMsgForm("�������� ���米ü ����",new string[] {"",ex.Message});
//			}			
//			finally
//			{
//				ProgressStop();
//			}			
//		}	

		#endregion
		#endregion

		#endregion

		#region �̺�Ʈ�Լ�

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

		#region FTPó���Լ�

		/// <summary>
		/// FTP�� CDN, CMS���� ������ �о�´�.
		/// </summary>
		private void SetConfig()
		{
			try
			{
				
				new AdFileManager(systemModel,commonModel).GetFtpConfig(adFileModel);

				if (adFileModel.ResultCD.Equals("0000"))
				{
					FtpUploadHost  = adFileModel.FtpUploadHost;
					FtpUploadPort  = adFileModel.FtpUploadPort;
					FtpUploadID    = adFileModel.FtpUploadID;
					FtpUploadPW    = Security.Decrypt(adFileModel.FtpUploadPW);

					FtpMovePath    = adFileModel.FtpMovePath;
					FtpMoveUseYn   = adFileModel.FtpMoveUseYn;

					FtpCdnHost  = adFileModel.FtpCdnHost;
					FtpCdnPort  = adFileModel.FtpCdnPort;
					FtpCdnID    = adFileModel.FtpCdnID;
					FtpCdnPW    = Security.Decrypt(adFileModel.FtpCdnPW);
					
					mCmsMasUrl	=	adFileModel.CmsMasUrl;
					mCmsMasQuery=	adFileModel.CmsMasQuery;
					
				}
				else
				{
					FtpUploadHost = "218.237.55.246";
					FtpUploadPort = "2401";
					FtpUploadID   = "adv_ftpuser";
					FtpUploadPW   = Security.Decrypt("wEKP/Sn+SrvPh4LC94E6Aw==");

					FtpMovePath    = "/adv_mov";
					FtpMoveUseYn   = "N";

					FtpCdnHost = "121.125.24.51";
					FtpCdnPort = "2401";
					FtpCdnID   = "adv_ftpuser";
					FtpCdnPW   = Security.Decrypt("wEKP/Sn+SrvPh4LC94E6Aw==");

					mCmsMasUrl	=	"";
					mCmsMasQuery=	"";

				}
			}
			catch (Exception ex)
			{
				FrameSystem.oLog.Error("�������� ��ȸ ����:"+ex.Message);
			}
		}

		private void createCDNFtp()
		{
			//--------------
			// Ftp ��ü ����
			//--------------
			try
			{
				if (ftmCDN == null)
				{
					ftmCDN = new FtpManager();

					ftmCDN.SetIpAddress	= FtpCdnHost;
					ftmCDN.SetPort		= Convert.ToInt32(FtpCdnPort);
					ftmCDN.SetUserId	= FtpCdnID;
					ftmCDN.SetUserPwd	= FtpCdnPW;
				}
			}
			catch (Exception ex)
			{
				FrameSystem.oLog.Error("CDN���� �������:"+ex.Message);
			}
		}

		private bool checkCDNFile(string Path, string FileName)
		{
			//------------------
			// �������� �������翩�� üũ
			//------------------
			try
			{
				if (ftmCDN.IsConnected == false)
				{
					// �̿���� 3ȸ�õ�
					for(int retry = 3; retry > 0; retry--)
					{
						try
						{
							ftmCDN.Connect();
							if(ftmCDN.IsConnected == true) break;
						}
						catch(Exception)
						{
							Thread.Sleep(500);
						}
					}
				}

				try
				{
					long sz = ftmCDN.GetFileSize(Path + "/" + FileName);
					return true;
				}
				catch
				{
					return false;
				}
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
		}

		private void createTESTFtp()
		{
			//--------------
			// Ftp ��ü ����
			//--------------
			try
			{


				if (ftmTEST == null)
				{
					ftmTEST = new FtpManager();

					ftmTEST.SetIpAddress	= FtpUploadHost;
					ftmTEST.SetPort			= Convert.ToInt32(FtpUploadPort);
					ftmTEST.SetUserId		= FtpUploadID;
					ftmTEST.SetUserPwd		= FtpUploadPW;
				}
			}
			catch (Exception ex)
			{
				FrameSystem.oLog.Error("TEST FTP���� �������:"+ex.Message);
			}
		}

		private bool checkTESTFile(string Path, string FileName)
		{
			//------------------
			// �������� �������翩�� üũ
			//------------------
			try
			{

				if (ftmTEST.IsConnected == false)
				{
					// �̿���� 3ȸ�õ�
					for(int retry = 3; retry > 0; retry--)
					{
						try
						{
							ftmTEST.Connect();
							if(ftmTEST.IsConnected == true) break;
						}
						catch(Exception)
						{
							Thread.Sleep(500);
						}
					}
				}

				try
				{
					long sz = ftmTEST.GetFileSize(Path + "/" + FileName);
					return true;
				}
				catch
				{
					return false;
				}
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
		}


		private bool moveTESTFile(string fromPath, string FileName, string toPath)
		{
			//------------------
			// �������� �������翩�� üũ
			//------------------
			try
			{

				if (ftmTEST.IsConnected == false)
				{
					// �̿���� 3ȸ�õ�
					for(int retry = 3; retry > 0; retry--)
					{
						try
						{
							ftmTEST.Connect();
							if(ftmTEST.IsConnected == true) break;
						}
						catch(Exception)
						{
							Thread.Sleep(500);
						}
					}
				}

				try
				{
					// ������ �̵���Ų��.
					ftmTEST.RenameFile(fromPath + "/" + FileName,  toPath + "/" + FileName, true);

					return true;
				}
				catch
				{
					return false;
				}
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
		}



		#endregion

		#region ���� ���
		/// <summary>
		/// ���� ����
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnExcel_Click(object sender, System.EventArgs e)
		{	

			Excel.Application xlApp= null;
			Excel._Workbook   oWB = null;
			Excel._Worksheet  oSheet = null;
			Excel.Range       oRng = null;
			
			try
			{	

				int ColMax  = 19; // �÷���   				

				int TitleRow  = 1;		
				int ConditionRow = 2;   
				int HeaderRow = 5;
				int DataRow   = 6;
				string StartCol = "A";
				string EndCol   = "";
				string TitleCol = "E";
				int DataCount = 0;
				int CondCount = 0;
				int HeaderCount = 0;

				// ������ �÷��� �ε�������
				EndCol = GetColumnIndex(ColMax);

				xlApp = new Excel.Application();
				oWB = (Excel._Workbook)(xlApp.Workbooks.Add(Missing.Value));
				oSheet = (Excel._Worksheet)oWB.ActiveSheet;


				// Ÿ��Ʋ �ۼ�
				oSheet.Cells[TitleRow,1] = "�������� ��������";
				oRng = oSheet.get_Range(StartCol+Convert.ToString(TitleRow), TitleCol+Convert.ToString(TitleRow));
				oRng.Merge(true);
				oRng.Font.Bold = true;
				oRng.Font.Size = 16;
				oRng.VerticalAlignment   = Excel.XlVAlign.xlVAlignCenter;			
				oRng.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;		

				// �������� �ۼ�
				oSheet.Cells[ConditionRow+CondCount,1] = "��ȸ�Ͻ�";
				oRng = oSheet.get_Range("B"+Convert.ToString(ConditionRow+CondCount), TitleCol+Convert.ToString(ConditionRow+CondCount));
				oRng.Merge(true);
				oSheet.Cells[ConditionRow+CondCount,2] = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
				CondCount++;

				oSheet.Cells[ConditionRow+CondCount,1] = "���ϻ���";
				oRng = oSheet.get_Range("B"+Convert.ToString(ConditionRow+CondCount), TitleCol+Convert.ToString(ConditionRow+CondCount));
				oRng.Merge(true);
				oSheet.Cells[ConditionRow+CondCount,2] = keyFileStateName;
				CondCount++;

				
				// ���Ǻ� �׵θ�
				oRng = oSheet.get_Range(StartCol+Convert.ToString(ConditionRow), TitleCol+Convert.ToString(ConditionRow+(CondCount-1)));
				oRng.VerticalAlignment   = Excel.XlVAlign.xlVAlignCenter;			
				oRng.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;		
				oRng.Borders.LineStyle =  Excel.XlLineStyle.xlContinuous;	// �׵θ��Ӽ� �Ʒ��� �Ǽ�
				oRng.Borders.Weight    = Excel.XlBorderWeight.xlThin;		// �׵θ��Ӽ� �Ʒ��� ���¼�


				// ��� ���� �ۼ�
				HeaderCount = 1;
				oSheet.Cells[HeaderRow,HeaderCount++] = "�����ȣ"; 
				oSheet.Cells[HeaderRow,HeaderCount++] = "�����";
				oSheet.Cells[HeaderRow,HeaderCount++] = "�������";
				oSheet.Cells[HeaderRow,HeaderCount++] = "���ϻ���";
				oSheet.Cells[HeaderRow,HeaderCount++] = "������ġ";
				oSheet.Cells[HeaderRow,HeaderCount++] = "���ϸ�";
				oSheet.Cells[HeaderRow,HeaderCount++] = "���ϱ���";
				oSheet.Cells[HeaderRow,HeaderCount++] = "����ũ��";
				oSheet.Cells[HeaderRow,HeaderCount++] = "�ٿ����";
				oSheet.Cells[HeaderRow,HeaderCount++] = "���ϵ���Ͻ�";
				oSheet.Cells[HeaderRow,HeaderCount++] = "���ϵ����";
				oSheet.Cells[HeaderRow,HeaderCount++] = "�˼�Ȯ���Ͻ�";
				oSheet.Cells[HeaderRow,HeaderCount++] = "�˼�Ȯ����";
				oSheet.Cells[HeaderRow,HeaderCount++] = "CDN�����Ͻ�";
				oSheet.Cells[HeaderRow,HeaderCount++] = "CDN����Ȯ����";
				oSheet.Cells[HeaderRow,HeaderCount++] = "�����Ϸ��Ͻ�";
				oSheet.Cells[HeaderRow,HeaderCount++] = "�����Ϸ�Ȯ����";
				oSheet.Cells[HeaderRow,HeaderCount++] = "��ž�����Ͻ�";
				oSheet.Cells[HeaderRow,HeaderCount++] = "��ž������";

				oRng = oSheet.get_Range(StartCol+Convert.ToString(HeaderRow), EndCol+Convert.ToString(HeaderRow)); // ����� ����
				oRng.Font.Bold           = true;							// ��Ʈ ����
				oRng.VerticalAlignment   = Excel.XlVAlign.xlVAlignCenter;	// �����߾�����
				oRng.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;	// �����߾�����	 
				oRng.Interior.Color      = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.CornflowerBlue);   //�� ���� 
				oRng.Font.Color          = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.White);	  //�ؽ�Ʈ��			
				

				DataCount = 0;

				// ������ ����
				for (int inx =0; inx < adFileWideDs.AdFileWide.Rows.Count; inx++)
				{

					DataRow Row = adFileWideDs.AdFileWide.Rows[inx];			

					int ColCnt = 1;

					oSheet.Cells[DataRow+DataCount,ColCnt++] = Convert.ToInt32(Row["ItemNo"].ToString());		// 1  �����ȣ 
					oSheet.Cells[DataRow+DataCount,ColCnt++] = Row["ItemName"].ToString();						// 2  �����
					oSheet.Cells[DataRow+DataCount,ColCnt++] = Row["AdStateName"].ToString();					// 3  �������
					oSheet.Cells[DataRow+DataCount,ColCnt++] = Row["FileStateName"].ToString();					// 4  ���ϻ���
					oSheet.Cells[DataRow+DataCount,ColCnt++] = Row["FilePath"].ToString();						// 5  ������ġ
					oSheet.Cells[DataRow+DataCount,ColCnt++] = Row["FileName"].ToString();						// 6  ���ϸ�
					oSheet.Cells[DataRow+DataCount,ColCnt++] = Row["FileTypeName"].ToString();					// 7  ���ϱ���
					if(Row["FileLength"].ToString().Length > 0)
					{
						oSheet.Cells[DataRow+DataCount,ColCnt++] = Convert.ToInt32(Row["FileLength"].ToString());	// 8  ����ũ��
					}
					else
					{
						oSheet.Cells[DataRow+DataCount,ColCnt++] = "";	// 8  ����ũ��
					}
					oSheet.Cells[DataRow+DataCount,ColCnt++] = Row["DownLevelName"].ToString();					// 9  �ٿ����
					oSheet.Cells[DataRow+DataCount,ColCnt++] = Row["FileRegDt"].ToString();						// 10 ���ϵ���Ͻ�
					oSheet.Cells[DataRow+DataCount,ColCnt++] = Row["FileRegName"].ToString();					// 11 ���ϵ����
					oSheet.Cells[DataRow+DataCount,ColCnt++] = Row["CheckDt"].ToString();						// 12 �˼�Ȯ���Ͻ�
					oSheet.Cells[DataRow+DataCount,ColCnt++] = Row["CheckName"].ToString();						// 13 �˼�Ȯ����
					oSheet.Cells[DataRow+DataCount,ColCnt++] = Row["CDNSyncDt"].ToString();						// 14 CDN�����Ͻ�
					oSheet.Cells[DataRow+DataCount,ColCnt++] = Row["CDNSyncName"].ToString();					// 15 CDN����Ȯ����
					oSheet.Cells[DataRow+DataCount,ColCnt++] = Row["CDNPubDt"].ToString();						// 16 �����Ϸ��Ͻ�
					oSheet.Cells[DataRow+DataCount,ColCnt++] = Row["CDNPubName"].ToString();					// 17 �����Ϸ�Ȯ����
					oSheet.Cells[DataRow+DataCount,ColCnt++] = Row["STBDelDt"].ToString();						// 18 ��ž�����Ͻ�
					oSheet.Cells[DataRow+DataCount,ColCnt++] = Row["STBDelName"].ToString();					// 19 ��ž������
					DataCount++;
				}

				DataCount--;


				// ������ �ۼ�
				oRng = oSheet.get_Range(StartCol+Convert.ToString(HeaderRow), EndCol+Convert.ToString(DataRow+DataCount));	// �������� ����
				oRng.EntireColumn.AutoFit();					// �������� ũ�⿡ ���� ����ũ�� ����
				oRng.Borders.LineStyle =  Excel.XlLineStyle.xlContinuous;	// �׵θ��Ӽ� �Ʒ��� �Ǽ�
				oRng.Borders.Weight    = Excel.XlBorderWeight.xlThin;		// �׵θ��Ӽ� �Ʒ��� ���¼�

				// ����ũ�� ��Ÿ�� ����
				oRng = oSheet.get_Range(GetColumnIndex(8)+Convert.ToString(DataRow), GetColumnIndex(8)+Convert.ToString(DataRow+DataCount));	// �������� ����
				oRng.NumberFormatLocal = "#,##0";
			
				xlApp.Visible = true;
				xlApp.UserControl = true;


			}
			catch(Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
		}

		private string GetColumnIndex(int ColCount)
		{
			string[] ColName = {"Z","A","B","C","D","E","F","G","H","I","J","K","L","M","N","O","P","Q","R","S","T","U","V","W","X","Y"};

			string ColumnIndex;

			// 26���� ũ��
			if(ColCount > ColName.Length)
			{
				// 2�ڸ� �ε������� 26 => Z;  27->AA
				ColumnIndex = ColName[(int)(Math.Floor((float)(ColCount/ColName.Length)))] + ColName[(int)(Math.Floor((float)(ColCount%ColName.Length)))];
			}
			else
			{
				ColumnIndex = ColName[(int)(Math.Floor((float)(ColCount%ColName.Length)))];
			}

			return ColumnIndex;
		}

		#endregion

		/// <summary>
		/// CMS����ó�� InnoSyncCommunicator�� ȣ���Ѵ�.
		/// </summary>
		/// <param name="cmsUrl">�������̽� Url</param>
		/// <param name="cmsQuery">�������̽� Query(Post���)</param>
		/// <param name="cid">����Key adv_�������ϸ�</param>
		/// <param name="filelist">���ε����ϸ��</param>
		/// <returns>ȣ�⿡ ���� ����( 1:����, 2:����) </returns>
		private string RequestCMS( string cmsUrl, string cmsQuery, string cid, string filelist )
		{
			HttpWebRequest		request					= null;
			HttpWebResponse		response				= null;
			Stream				responseStream			= null;
			StreamReader		responseStreamReader	= null;
			Stream				sw						= null;
			string	readStr		= string.Empty;
			string	postData	= "";
            
			try
			{
				postData = cmsQuery + "&cid=" + cid + "&filelist=" + filelist;

				request					= (HttpWebRequest)WebRequest.Create( cmsUrl );
				request.Method			= "POST";
				request.KeepAlive		= true;
				request.Timeout			= 10000;
				request.ContentType		="application/x-www-form-urlencoded";
				request.ContentLength	= postData.Length;
				
				sw = request.GetRequestStream();
				byte[] sendBuffer = Encoding.ASCII.GetBytes(postData);
				sw.Write( sendBuffer, 0 , sendBuffer.Length );
				sw.Close();
                
				response	= (HttpWebResponse)request.GetResponse();
				responseStream = response.GetResponseStream();
				responseStreamReader = new StreamReader( responseStream, System.Text.Encoding.UTF8);
				readStr = responseStreamReader.ReadToEnd();
			}
			catch(Exception ex)
			{
				throw ex;
			}
			finally
			{
				if ( null != responseStreamReader )
				{
					responseStreamReader.Close();
					responseStreamReader = null;
				}
				
				if ( null != responseStream )
				{
					responseStream.Close();
					responseStream = null;
				}

				if ( null != response )
				{
					response.Close();
					response = null;
				}

				if ( null != request )
				{
					request = null;
				}
			}
			return readStr.Trim();
		}

		private void grdExAdFileWideList_CellValueChanged(object sender, Janus.Windows.GridEX.ColumnActionEventArgs e)
		{
			if (e.Column.EditType == Janus.Windows.GridEX.EditType.CheckBox)
			{
				int curRow = cmFile.Position;
				if (curRow >= 0)
				{
					dtFile.Rows[curRow].BeginEdit();
					dtFile.Rows[curRow]["CheckYn"] = grdExAdFileWideList.GetValue(e.Column).ToString();
					dtFile.Rows[curRow].EndEdit();
				}
			}
		}

        private void uiButton1_Click(object sender, EventArgs e)
        {
            int truecount = 0;
            int rc = 0;
            for (int i = 0; i < adFileWideDs.AdFileWide.Rows.Count; i++)
            {
                rc++;
                DataRow row = adFileWideDs.AdFileWide.Rows[i];

                if (row["CheckYn"].ToString().Equals("True"))
                {
                    adFileWideModel.Init();

                    adFileWideModel.MediaCode = keyMediaCode;
                    adFileWideModel.ItemNo = row["ItemNo"].ToString();
                    adFileWideModel.ItemName = row["ItemName"].ToString();
                    adFileWideModel.FileName = row["FileName"].ToString();
                    truecount++;
                    //new AdFileWideManager(systemModel, commonModel).SetAdFileCDNPublishCancel(adFileWideModel);
                }
            }
            MessageBox.Show(truecount + "�� TRUE");
        }
        
	}
}
