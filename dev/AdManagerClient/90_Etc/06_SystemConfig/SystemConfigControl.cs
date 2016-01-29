// ===============================================================================
// SystemConfigControl for Charites Project
//
// SystemConfigControl.cs
//
// ȯ�漳������ ������� �����մϴ�. 
//
// ===============================================================================
// Release history
//	1. 2010-09-06 CMS���� Url Query�׸� �߰���( CmsMasUrl,CmsMasQuery )
//		���ϰ˼��Ϸ� �۾��� �׽�Ʈ�˼��������� �����Ͽ� ȣ��
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
	/// ����ڰ��� ��Ʈ��
	/// </summary>
	public class SystemConfigControl : System.Windows.Forms.UserControl, IUserControl
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

		public string        menuCode		= "";

		// ����� ������
		SystemConfigModel systemConfigModel  = new SystemConfigModel();	// ȯ�漳����

		// ȭ��ó���� ����		
		bool canRead			  = false;
		bool canUpdate			  = false;

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
		private Janus.Windows.UI.Dock.UIPanel uiPanelUsersSearch;
		private Janus.Windows.UI.Dock.UIPanelInnerContainer uiPanelUsersSearchContainer;
		private Janus.Windows.EditControls.UIButton uiButton2;
		private Janus.Windows.EditControls.UIButton uiButton1;
		private Janus.Windows.UI.Dock.UIPanelGroup uiPanelUsers;
		private System.Windows.Forms.Panel pnlSearch;
		private Janus.Windows.EditControls.UIButton btnSave;
		private Janus.Windows.EditControls.UIGroupBox uiGroupBox1;
		private Janus.Windows.GridEX.EditControls.EditBox ebFtpPassword;
		private Janus.Windows.GridEX.EditControls.EditBox ebFtpID;
		private System.Windows.Forms.Label lbFtpID;
		private System.Windows.Forms.Label lbFtpHost;
		private System.Windows.Forms.Label lbFtpPassword;
		private Janus.Windows.GridEX.EditControls.EditBox ebFtpHost;
		private System.Windows.Forms.Label lbFtpPort;
		private Janus.Windows.GridEX.EditControls.EditBox ebFtpPort;
		private System.Windows.Forms.Label lbFtpPath;
		private Janus.Windows.GridEX.EditControls.EditBox ebFtpPath;
		private System.Data.DataView dvSystemConfig;
		private AdManagerClient._90_Etc._06_SystemConfig.SystemConfigDs systemConfigDs;
		private System.Windows.Forms.Label label1;
		private Janus.Windows.GridEX.EditControls.EditBox ebFtpMovePath;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.Label label9;
		private Janus.Windows.EditControls.UIRadioButton rbFtpMoveUseYn_Y;
		private Janus.Windows.EditControls.UIRadioButton rbFtpMoveUseYn_N;
		private Janus.Windows.EditControls.UIGroupBox uiGroupBox4;
		private Janus.Windows.EditControls.UIGroupBox uiGroupBox5;
		private System.Windows.Forms.Label label19;
		private System.Windows.Forms.Label label20;
		private System.Windows.Forms.Label label21;
		private System.Windows.Forms.Label label22;
		private Janus.Windows.GridEX.EditControls.EditBox ebFtpCdnID;
		private Janus.Windows.GridEX.EditControls.EditBox ebFtpCdnPassword;
		private Janus.Windows.GridEX.EditControls.EditBox ebFtpCdnHost;
		private Janus.Windows.GridEX.EditControls.EditBox ebFtpCdnPort;
		private System.Windows.Forms.Label label23;
		private System.Windows.Forms.Label label24;
		private System.Windows.Forms.Label label25;
		private System.Windows.Forms.Label label26;
		private Janus.Windows.EditControls.UIGroupBox uiGroupBox6;
		private System.Windows.Forms.Label label27;
		private System.Windows.Forms.Label label28;
		private Janus.Windows.GridEX.EditControls.EditBox ebCms_Url;
		private Janus.Windows.GridEX.EditControls.EditBox ebCms_Query;
		private System.ComponentModel.IContainer components;

		public SystemConfigControl()
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
			this.uiPM = new Janus.Windows.UI.Dock.UIPanelManager(this.components);
			this.uiPanelUsers = new Janus.Windows.UI.Dock.UIPanelGroup();
			this.uiPanelUsersSearch = new Janus.Windows.UI.Dock.UIPanel();
			this.uiPanelUsersSearchContainer = new Janus.Windows.UI.Dock.UIPanelInnerContainer();
			this.pnlSearch = new System.Windows.Forms.Panel();
			this.uiGroupBox6 = new Janus.Windows.EditControls.UIGroupBox();
			this.ebCms_Url = new Janus.Windows.GridEX.EditControls.EditBox();
			this.label27 = new System.Windows.Forms.Label();
			this.label28 = new System.Windows.Forms.Label();
			this.ebCms_Query = new Janus.Windows.GridEX.EditControls.EditBox();
			this.uiGroupBox5 = new Janus.Windows.EditControls.UIGroupBox();
			this.ebFtpCdnID = new Janus.Windows.GridEX.EditControls.EditBox();
			this.ebFtpCdnPassword = new Janus.Windows.GridEX.EditControls.EditBox();
			this.ebFtpCdnHost = new Janus.Windows.GridEX.EditControls.EditBox();
			this.ebFtpCdnPort = new Janus.Windows.GridEX.EditControls.EditBox();
			this.label23 = new System.Windows.Forms.Label();
			this.label24 = new System.Windows.Forms.Label();
			this.label25 = new System.Windows.Forms.Label();
			this.label26 = new System.Windows.Forms.Label();
			this.label19 = new System.Windows.Forms.Label();
			this.label20 = new System.Windows.Forms.Label();
			this.label21 = new System.Windows.Forms.Label();
			this.label22 = new System.Windows.Forms.Label();
			this.uiGroupBox4 = new Janus.Windows.EditControls.UIGroupBox();
			this.label1 = new System.Windows.Forms.Label();
			this.rbFtpMoveUseYn_Y = new Janus.Windows.EditControls.UIRadioButton();
			this.ebFtpMovePath = new Janus.Windows.GridEX.EditControls.EditBox();
			this.label2 = new System.Windows.Forms.Label();
			this.rbFtpMoveUseYn_N = new Janus.Windows.EditControls.UIRadioButton();
			this.label8 = new System.Windows.Forms.Label();
			this.label9 = new System.Windows.Forms.Label();
			this.uiGroupBox1 = new Janus.Windows.EditControls.UIGroupBox();
			this.ebFtpID = new Janus.Windows.GridEX.EditControls.EditBox();
			this.lbFtpID = new System.Windows.Forms.Label();
			this.lbFtpPassword = new System.Windows.Forms.Label();
			this.ebFtpPassword = new Janus.Windows.GridEX.EditControls.EditBox();
			this.ebFtpHost = new Janus.Windows.GridEX.EditControls.EditBox();
			this.lbFtpHost = new System.Windows.Forms.Label();
			this.ebFtpPort = new Janus.Windows.GridEX.EditControls.EditBox();
			this.lbFtpPort = new System.Windows.Forms.Label();
			this.ebFtpPath = new Janus.Windows.GridEX.EditControls.EditBox();
			this.lbFtpPath = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.label5 = new System.Windows.Forms.Label();
			this.label6 = new System.Windows.Forms.Label();
			this.label7 = new System.Windows.Forms.Label();
			this.btnSave = new Janus.Windows.EditControls.UIButton();
			this.dvSystemConfig = new System.Data.DataView();
			this.systemConfigDs = new AdManagerClient._90_Etc._06_SystemConfig.SystemConfigDs();
			this.uiButton1 = new Janus.Windows.EditControls.UIButton();
			this.uiButton2 = new Janus.Windows.EditControls.UIButton();
			((System.ComponentModel.ISupportInitialize)(this.uiPM)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.uiPanelUsers)).BeginInit();
			this.uiPanelUsers.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.uiPanelUsersSearch)).BeginInit();
			this.uiPanelUsersSearch.SuspendLayout();
			this.uiPanelUsersSearchContainer.SuspendLayout();
			this.pnlSearch.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.uiGroupBox6)).BeginInit();
			this.uiGroupBox6.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.uiGroupBox5)).BeginInit();
			this.uiGroupBox5.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.uiGroupBox4)).BeginInit();
			this.uiGroupBox4.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.uiGroupBox1)).BeginInit();
			this.uiGroupBox1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.dvSystemConfig)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.systemConfigDs)).BeginInit();
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
			this.uiPM.Panels.Add(this.uiPanelUsers);
			// 
			// Design Time Panel Info:
			// 
			this.uiPM.BeginPanelInfo();
			this.uiPM.AddDockPanelInfo(new System.Guid("b162c53a-7940-47cd-a0ed-9762c59c50d8"), Janus.Windows.UI.Dock.PanelGroupStyle.HorizontalTiles, Janus.Windows.UI.Dock.PanelDockStyle.Fill, true, new System.Drawing.Size(1010, 677), true);
			this.uiPM.AddDockPanelInfo(new System.Guid("afb3b7d0-aa79-443c-a1e8-3baba38f7a4d"), new System.Guid("b162c53a-7940-47cd-a0ed-9762c59c50d8"), 400, true);
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
			this.uiPanelUsers.Text = "ȯ�漳��";
			// 
			// uiPanelUsersSearch
			// 
			this.uiPanelUsersSearch.AllowResize = Janus.Windows.UI.InheritableBoolean.False;
			this.uiPanelUsersSearch.CaptionVisible = Janus.Windows.UI.InheritableBoolean.False;
			this.uiPanelUsersSearch.CloseButtonVisible = Janus.Windows.UI.InheritableBoolean.False;
			this.uiPanelUsersSearch.InnerContainer = this.uiPanelUsersSearchContainer;
			this.uiPanelUsersSearch.Location = new System.Drawing.Point(0, 22);
			this.uiPanelUsersSearch.Name = "uiPanelUsersSearch";
			this.uiPanelUsersSearch.Size = new System.Drawing.Size(1010, 655);
			this.uiPanelUsersSearch.TabIndex = 4;
			this.uiPanelUsersSearch.Text = "�˻�";
			// 
			// uiPanelUsersSearchContainer
			// 
			this.uiPanelUsersSearchContainer.Controls.Add(this.pnlSearch);
			this.uiPanelUsersSearchContainer.Location = new System.Drawing.Point(1, 1);
			this.uiPanelUsersSearchContainer.Name = "uiPanelUsersSearchContainer";
			this.uiPanelUsersSearchContainer.Size = new System.Drawing.Size(1008, 653);
			this.uiPanelUsersSearchContainer.TabIndex = 0;
			// 
			// pnlSearch
			// 
			this.pnlSearch.BackColor = System.Drawing.SystemColors.Window;
			this.pnlSearch.Controls.Add(this.uiGroupBox6);
			this.pnlSearch.Controls.Add(this.uiGroupBox5);
			this.pnlSearch.Controls.Add(this.uiGroupBox4);
			this.pnlSearch.Controls.Add(this.uiGroupBox1);
			this.pnlSearch.Controls.Add(this.btnSave);
			this.pnlSearch.Dock = System.Windows.Forms.DockStyle.Fill;
			this.pnlSearch.Location = new System.Drawing.Point(0, 0);
			this.pnlSearch.Name = "pnlSearch";
			this.pnlSearch.Size = new System.Drawing.Size(1008, 653);
			this.pnlSearch.TabIndex = 3;
			// 
			// uiGroupBox6
			// 
			this.uiGroupBox6.Controls.Add(this.ebCms_Url);
			this.uiGroupBox6.Controls.Add(this.label27);
			this.uiGroupBox6.Controls.Add(this.label28);
			this.uiGroupBox6.Controls.Add(this.ebCms_Query);
			this.uiGroupBox6.Location = new System.Drawing.Point(32, 406);
			this.uiGroupBox6.Name = "uiGroupBox6";
			this.uiGroupBox6.Size = new System.Drawing.Size(800, 80);
			this.uiGroupBox6.TabIndex = 23;
			this.uiGroupBox6.Text = "CMS���� �������̽�";
			this.uiGroupBox6.Visible = false;
			// 
			// ebCms_Url
			// 
			this.ebCms_Url.Location = new System.Drawing.Point(104, 24);
			this.ebCms_Url.MaxLength = 200;
			this.ebCms_Url.Name = "ebCms_Url";
			this.ebCms_Url.Size = new System.Drawing.Size(680, 21);
			this.ebCms_Url.TabIndex = 20;
			this.ebCms_Url.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
			this.ebCms_Url.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
			// 
			// label27
			// 
			this.label27.Location = new System.Drawing.Point(8, 24);
			this.label27.Name = "label27";
			this.label27.Size = new System.Drawing.Size(88, 21);
			this.label27.TabIndex = 18;
			this.label27.Text = "CMS URL";
			this.label27.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label28
			// 
			this.label28.Location = new System.Drawing.Point(8, 48);
			this.label28.Name = "label28";
			this.label28.Size = new System.Drawing.Size(96, 24);
			this.label28.TabIndex = 19;
			this.label28.Text = "CMS Query";
			this.label28.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// ebCms_Query
			// 
			this.ebCms_Query.Location = new System.Drawing.Point(104, 48);
			this.ebCms_Query.MaxLength = 200;
			this.ebCms_Query.Name = "ebCms_Query";
			this.ebCms_Query.Size = new System.Drawing.Size(680, 21);
			this.ebCms_Query.TabIndex = 21;
			this.ebCms_Query.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
			this.ebCms_Query.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
			// 
			// uiGroupBox5
			// 
			this.uiGroupBox5.Controls.Add(this.ebFtpCdnID);
			this.uiGroupBox5.Controls.Add(this.ebFtpCdnPassword);
			this.uiGroupBox5.Controls.Add(this.ebFtpCdnHost);
			this.uiGroupBox5.Controls.Add(this.ebFtpCdnPort);
			this.uiGroupBox5.Controls.Add(this.label23);
			this.uiGroupBox5.Controls.Add(this.label24);
			this.uiGroupBox5.Controls.Add(this.label25);
			this.uiGroupBox5.Controls.Add(this.label26);
			this.uiGroupBox5.Controls.Add(this.label19);
			this.uiGroupBox5.Controls.Add(this.label20);
			this.uiGroupBox5.Controls.Add(this.label21);
			this.uiGroupBox5.Controls.Add(this.label22);
			this.uiGroupBox5.Location = new System.Drawing.Point(32, 268);
			this.uiGroupBox5.Name = "uiGroupBox5";
			this.uiGroupBox5.Size = new System.Drawing.Size(552, 120);
			this.uiGroupBox5.TabIndex = 10;
			this.uiGroupBox5.Text = "CDN����ȯ��";
			// 
			// ebFtpCdnID
			// 
			this.ebFtpCdnID.Location = new System.Drawing.Point(104, 16);
			this.ebFtpCdnID.MaxLength = 20;
			this.ebFtpCdnID.Name = "ebFtpCdnID";
			this.ebFtpCdnID.Size = new System.Drawing.Size(160, 21);
			this.ebFtpCdnID.TabIndex = 11;
			this.ebFtpCdnID.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
			this.ebFtpCdnID.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
			// 
			// ebFtpCdnPassword
			// 
			this.ebFtpCdnPassword.Location = new System.Drawing.Point(104, 40);
			this.ebFtpCdnPassword.MaxLength = 30;
			this.ebFtpCdnPassword.Name = "ebFtpCdnPassword";
			this.ebFtpCdnPassword.Size = new System.Drawing.Size(160, 21);
			this.ebFtpCdnPassword.TabIndex = 12;
			this.ebFtpCdnPassword.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
			this.ebFtpCdnPassword.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
			// 
			// ebFtpCdnHost
			// 
			this.ebFtpCdnHost.Location = new System.Drawing.Point(104, 64);
			this.ebFtpCdnHost.MaxLength = 20;
			this.ebFtpCdnHost.Name = "ebFtpCdnHost";
			this.ebFtpCdnHost.Size = new System.Drawing.Size(160, 21);
			this.ebFtpCdnHost.TabIndex = 13;
			this.ebFtpCdnHost.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
			this.ebFtpCdnHost.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
			// 
			// ebFtpCdnPort
			// 
			this.ebFtpCdnPort.Location = new System.Drawing.Point(104, 88);
			this.ebFtpCdnPort.MaxLength = 5;
			this.ebFtpCdnPort.Name = "ebFtpCdnPort";
			this.ebFtpCdnPort.Size = new System.Drawing.Size(160, 21);
			this.ebFtpCdnPort.TabIndex = 14;
			this.ebFtpCdnPort.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
			this.ebFtpCdnPort.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
			// 
			// label23
			// 
			this.label23.Location = new System.Drawing.Point(272, 16);
			this.label23.Name = "label23";
			this.label23.Size = new System.Drawing.Size(256, 21);
			this.label23.TabIndex = 41;
			this.label23.Text = "CDN������ FTP �����ID";
			this.label23.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label24
			// 
			this.label24.Location = new System.Drawing.Point(272, 40);
			this.label24.Name = "label24";
			this.label24.Size = new System.Drawing.Size(256, 21);
			this.label24.TabIndex = 42;
			this.label24.Text = "CDN������ FTP ��й�ȣ";
			this.label24.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label25
			// 
			this.label25.Location = new System.Drawing.Point(272, 64);
			this.label25.Name = "label25";
			this.label25.Size = new System.Drawing.Size(256, 21);
			this.label25.TabIndex = 39;
			this.label25.Text = "CDN������ FTP IP�ּ�";
			this.label25.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label26
			// 
			this.label26.Location = new System.Drawing.Point(272, 88);
			this.label26.Name = "label26";
			this.label26.Size = new System.Drawing.Size(256, 21);
			this.label26.TabIndex = 40;
			this.label26.Text = "CDN������ FTP ������Ʈ";
			this.label26.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label19
			// 
			this.label19.Location = new System.Drawing.Point(8, 16);
			this.label19.Name = "label19";
			this.label19.Size = new System.Drawing.Size(72, 21);
			this.label19.TabIndex = 31;
			this.label19.Text = "FTP ���̵�";
			this.label19.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label20
			// 
			this.label20.Location = new System.Drawing.Point(8, 40);
			this.label20.Name = "label20";
			this.label20.Size = new System.Drawing.Size(80, 24);
			this.label20.TabIndex = 33;
			this.label20.Text = "FTP ��й�ȣ";
			this.label20.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label21
			// 
			this.label21.Location = new System.Drawing.Point(8, 64);
			this.label21.Name = "label21";
			this.label21.Size = new System.Drawing.Size(72, 21);
			this.label21.TabIndex = 32;
			this.label21.Text = "FTP ȣ��Ʈ";
			this.label21.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label22
			// 
			this.label22.Location = new System.Drawing.Point(8, 88);
			this.label22.Name = "label22";
			this.label22.Size = new System.Drawing.Size(72, 21);
			this.label22.TabIndex = 34;
			this.label22.Text = "FTP ��Ʈ";
			this.label22.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// uiGroupBox4
			// 
			this.uiGroupBox4.Controls.Add(this.label1);
			this.uiGroupBox4.Controls.Add(this.rbFtpMoveUseYn_Y);
			this.uiGroupBox4.Controls.Add(this.ebFtpMovePath);
			this.uiGroupBox4.Controls.Add(this.label2);
			this.uiGroupBox4.Controls.Add(this.rbFtpMoveUseYn_N);
			this.uiGroupBox4.Controls.Add(this.label8);
			this.uiGroupBox4.Controls.Add(this.label9);
			this.uiGroupBox4.Location = new System.Drawing.Point(32, 186);
			this.uiGroupBox4.Name = "uiGroupBox4";
			this.uiGroupBox4.Size = new System.Drawing.Size(552, 64);
			this.uiGroupBox4.TabIndex = 7;
			this.uiGroupBox4.Text = "���������̵�";
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(8, 16);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(80, 21);
			this.label1.TabIndex = 30;
			this.label1.Text = "�̵���û��ġ";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// rbFtpMoveUseYn_Y
			// 
			this.rbFtpMoveUseYn_Y.Checked = true;
			this.rbFtpMoveUseYn_Y.Location = new System.Drawing.Point(104, 40);
			this.rbFtpMoveUseYn_Y.Name = "rbFtpMoveUseYn_Y";
			this.rbFtpMoveUseYn_Y.Size = new System.Drawing.Size(64, 21);
			this.rbFtpMoveUseYn_Y.TabIndex = 9;
			this.rbFtpMoveUseYn_Y.TabStop = true;
			this.rbFtpMoveUseYn_Y.Text = "�����";
			// 
			// ebFtpMovePath
			// 
			this.ebFtpMovePath.Location = new System.Drawing.Point(104, 16);
			this.ebFtpMovePath.MaxLength = 50;
			this.ebFtpMovePath.Name = "ebFtpMovePath";
			this.ebFtpMovePath.Size = new System.Drawing.Size(160, 21);
			this.ebFtpMovePath.TabIndex = 8;
			this.ebFtpMovePath.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
			this.ebFtpMovePath.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(8, 40);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(80, 21);
			this.label2.TabIndex = 30;
			this.label2.Text = "�̵���û���";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// rbFtpMoveUseYn_N
			// 
			this.rbFtpMoveUseYn_N.Location = new System.Drawing.Point(176, 40);
			this.rbFtpMoveUseYn_N.Name = "rbFtpMoveUseYn_N";
			this.rbFtpMoveUseYn_N.Size = new System.Drawing.Size(88, 21);
			this.rbFtpMoveUseYn_N.TabIndex = 9;
			this.rbFtpMoveUseYn_N.Text = "������";
			// 
			// label8
			// 
			this.label8.Location = new System.Drawing.Point(272, 16);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(256, 21);
			this.label8.TabIndex = 18;
			this.label8.Text = "CDN�������� �̵���û��ġ";
			this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label9
			// 
			this.label9.Location = new System.Drawing.Point(272, 40);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(256, 21);
			this.label9.TabIndex = 18;
			this.label9.Text = "CDN�������� �̵���û ��뿩��";
			this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// uiGroupBox1
			// 
			this.uiGroupBox1.Controls.Add(this.ebFtpID);
			this.uiGroupBox1.Controls.Add(this.lbFtpID);
			this.uiGroupBox1.Controls.Add(this.lbFtpPassword);
			this.uiGroupBox1.Controls.Add(this.ebFtpPassword);
			this.uiGroupBox1.Controls.Add(this.ebFtpHost);
			this.uiGroupBox1.Controls.Add(this.lbFtpHost);
			this.uiGroupBox1.Controls.Add(this.ebFtpPort);
			this.uiGroupBox1.Controls.Add(this.lbFtpPort);
			this.uiGroupBox1.Controls.Add(this.ebFtpPath);
			this.uiGroupBox1.Controls.Add(this.lbFtpPath);
			this.uiGroupBox1.Controls.Add(this.label3);
			this.uiGroupBox1.Controls.Add(this.label4);
			this.uiGroupBox1.Controls.Add(this.label5);
			this.uiGroupBox1.Controls.Add(this.label6);
			this.uiGroupBox1.Controls.Add(this.label7);
			this.uiGroupBox1.Location = new System.Drawing.Point(32, 16);
			this.uiGroupBox1.Name = "uiGroupBox1";
			this.uiGroupBox1.Size = new System.Drawing.Size(552, 152);
			this.uiGroupBox1.TabIndex = 1;
			this.uiGroupBox1.Text = "�˼�����ȯ��";
			// 
			// ebFtpID
			// 
			this.ebFtpID.Location = new System.Drawing.Point(104, 24);
			this.ebFtpID.MaxLength = 20;
			this.ebFtpID.Name = "ebFtpID";
			this.ebFtpID.Size = new System.Drawing.Size(160, 21);
			this.ebFtpID.TabIndex = 2;
			this.ebFtpID.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
			this.ebFtpID.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
			// 
			// lbFtpID
			// 
			this.lbFtpID.Location = new System.Drawing.Point(8, 24);
			this.lbFtpID.Name = "lbFtpID";
			this.lbFtpID.Size = new System.Drawing.Size(72, 21);
			this.lbFtpID.TabIndex = 18;
			this.lbFtpID.Text = "FTP ���̵�";
			this.lbFtpID.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// lbFtpPassword
			// 
			this.lbFtpPassword.Location = new System.Drawing.Point(8, 48);
			this.lbFtpPassword.Name = "lbFtpPassword";
			this.lbFtpPassword.Size = new System.Drawing.Size(80, 24);
			this.lbFtpPassword.TabIndex = 19;
			this.lbFtpPassword.Text = "FTP ��й�ȣ";
			this.lbFtpPassword.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// ebFtpPassword
			// 
			this.ebFtpPassword.Location = new System.Drawing.Point(104, 48);
			this.ebFtpPassword.MaxLength = 30;
			this.ebFtpPassword.Name = "ebFtpPassword";
			this.ebFtpPassword.Size = new System.Drawing.Size(160, 21);
			this.ebFtpPassword.TabIndex = 3;
			this.ebFtpPassword.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
			this.ebFtpPassword.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
			// 
			// ebFtpHost
			// 
			this.ebFtpHost.Location = new System.Drawing.Point(104, 72);
			this.ebFtpHost.MaxLength = 20;
			this.ebFtpHost.Name = "ebFtpHost";
			this.ebFtpHost.Size = new System.Drawing.Size(160, 21);
			this.ebFtpHost.TabIndex = 4;
			this.ebFtpHost.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
			this.ebFtpHost.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
			// 
			// lbFtpHost
			// 
			this.lbFtpHost.Location = new System.Drawing.Point(8, 72);
			this.lbFtpHost.Name = "lbFtpHost";
			this.lbFtpHost.Size = new System.Drawing.Size(72, 21);
			this.lbFtpHost.TabIndex = 19;
			this.lbFtpHost.Text = "FTP ȣ��Ʈ";
			this.lbFtpHost.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// ebFtpPort
			// 
			this.ebFtpPort.Location = new System.Drawing.Point(104, 96);
			this.ebFtpPort.MaxLength = 5;
			this.ebFtpPort.Name = "ebFtpPort";
			this.ebFtpPort.Size = new System.Drawing.Size(160, 21);
			this.ebFtpPort.TabIndex = 5;
			this.ebFtpPort.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
			this.ebFtpPort.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
			// 
			// lbFtpPort
			// 
			this.lbFtpPort.Location = new System.Drawing.Point(8, 96);
			this.lbFtpPort.Name = "lbFtpPort";
			this.lbFtpPort.Size = new System.Drawing.Size(72, 21);
			this.lbFtpPort.TabIndex = 30;
			this.lbFtpPort.Text = "FTP ��Ʈ";
			this.lbFtpPort.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// ebFtpPath
			// 
			this.ebFtpPath.Location = new System.Drawing.Point(104, 120);
			this.ebFtpPath.MaxLength = 50;
			this.ebFtpPath.Name = "ebFtpPath";
			this.ebFtpPath.Size = new System.Drawing.Size(160, 21);
			this.ebFtpPath.TabIndex = 6;
			this.ebFtpPath.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
			this.ebFtpPath.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
			// 
			// lbFtpPath
			// 
			this.lbFtpPath.Location = new System.Drawing.Point(8, 120);
			this.lbFtpPath.Name = "lbFtpPath";
			this.lbFtpPath.Size = new System.Drawing.Size(72, 21);
			this.lbFtpPath.TabIndex = 30;
			this.lbFtpPath.Text = "FTP ��ġ";
			this.lbFtpPath.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(272, 24);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(256, 21);
			this.label3.TabIndex = 18;
			this.label3.Text = "�������Ͼ��ε� FTP�� �����ID";
			this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(272, 48);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(256, 21);
			this.label4.TabIndex = 18;
			this.label4.Text = "�������Ͼ��ε� FTP�� ��й�ȣ";
			this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label5
			// 
			this.label5.Location = new System.Drawing.Point(272, 72);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(256, 21);
			this.label5.TabIndex = 18;
			this.label5.Text = "�������Ͼ��ε� FTP�� IP�ּ�";
			this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label6
			// 
			this.label6.Location = new System.Drawing.Point(272, 96);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(256, 21);
			this.label6.TabIndex = 18;
			this.label6.Text = "�������Ͼ��ε� FTP�� ������Ʈ";
			this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label7
			// 
			this.label7.Location = new System.Drawing.Point(272, 120);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(256, 21);
			this.label7.TabIndex = 18;
			this.label7.Text = "�������Ͼ��ε� FTP�� ���ε���ġ";
			this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// btnSave
			// 
			this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.btnSave.Enabled = false;
			this.btnSave.Location = new System.Drawing.Point(32, 602);
			this.btnSave.Name = "btnSave";
			this.btnSave.Size = new System.Drawing.Size(104, 24);
			this.btnSave.TabIndex = 22;
			this.btnSave.Text = "�� ��";
			this.btnSave.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
			this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
			// 
			// systemConfigDs
			// 
			this.systemConfigDs.DataSetName = "SystemConfigDs";
			this.systemConfigDs.Locale = new System.Globalization.CultureInfo("en-US");
			this.systemConfigDs.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
			// 
			// uiButton1
			// 
			this.uiButton1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.uiButton1.Location = new System.Drawing.Point(136, 8);
			this.uiButton1.Name = "uiButton1";
			this.uiButton1.Size = new System.Drawing.Size(112, 24);
			this.uiButton1.TabIndex = 5;
			this.uiButton1.Text = "�� ��";
			this.uiButton1.VisualStyle = Janus.Windows.UI.VisualStyle.Office2003;
			// 
			// uiButton2
			// 
			this.uiButton2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.uiButton2.Location = new System.Drawing.Point(8, 8);
			this.uiButton2.Name = "uiButton2";
			this.uiButton2.Size = new System.Drawing.Size(120, 24);
			this.uiButton2.TabIndex = 6;
			this.uiButton2.Text = "�� ��";
			this.uiButton2.VisualStyle = Janus.Windows.UI.VisualStyle.Office2003;
			// 
			// SystemConfigControl
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.Controls.Add(this.uiPanelUsers);
			this.Font = global::AdManagerClient.Properties.Settings.Default.Font1;
			this.Name = "SystemConfigControl";
			this.Size = new System.Drawing.Size(1010, 677);
			this.Load += new System.EventHandler(this.UserControl_Load);
			((System.ComponentModel.ISupportInitialize)(this.uiPM)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.uiPanelUsers)).EndInit();
			this.uiPanelUsers.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.uiPanelUsersSearch)).EndInit();
			this.uiPanelUsersSearch.ResumeLayout(false);
			this.uiPanelUsersSearchContainer.ResumeLayout(false);
			this.pnlSearch.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.uiGroupBox6)).EndInit();
			this.uiGroupBox6.ResumeLayout(false);
			this.uiGroupBox6.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.uiGroupBox5)).EndInit();
			this.uiGroupBox5.ResumeLayout(false);
			this.uiGroupBox5.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.uiGroupBox4)).EndInit();
			this.uiGroupBox4.ResumeLayout(false);
			this.uiGroupBox4.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.uiGroupBox1)).EndInit();
			this.uiGroupBox1.ResumeLayout(false);
			this.uiGroupBox1.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.dvSystemConfig)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.systemConfigDs)).EndInit();
			this.ResumeLayout(false);

		}
		#endregion

		#region ��Ʈ�� �ε�
		private void UserControl_Load(object sender, System.EventArgs e)
		{
			// ��Ʈ�� �ʱ�ȭ
			InitControl();		
		}

		#endregion

		#region ��Ʈ�� �ʱ�ȭ
		private void InitControl()
		{			
			ProgressStart();		

			// ��ȸ���� �˻�
			if(menu.CanRead(MenuCode))
			{
				canRead = true;
				SearchSystemConfig();
			}

			// ����ڱ����� ���� �Ǵ� ���������� ��츸 ��ü  CRUD�� ���� ������ ó���Ѵ�.
			// �ٸ� ����ڵ��� �ڱ��������� ������ �� �ֵ��� �Ѵ�.
			if(commonModel.UserClass.Equals("10") || commonModel.UserClass.Equals("20"))
			{					
				// �����ư Ȱ��ȭ
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
			
		private void InitButton()
		{
			if(canRead)
			{
				//btnSearch.Enabled = true;
			}
			else
			{
				//btnSearch.Enabled = false;
			}
			
			//if(ebFtpID.Text.Trim().Length > 0) 
			//{
				
			if(canUpdate)
			{
				btnSave.Enabled   = true;
			}
			else
			{
				btnSave.Enabled   = false;
			}

			//}		

			Application.DoEvents();	
		}

		private void DisableButton()
		{
			//btnSearch.Enabled = false;			
			btnSave.Enabled   = false;			

			Application.DoEvents();
		}

		#endregion

		#region ����� �׼�ó�� �޼ҵ�

		/// <summary>
		/// �׸����� Row�����
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void OnGrdRowChanged(object sender, System.EventArgs e) 
		{		
			SetConfigDetailText();
			InitButton();
		}

		/// <summary>
		/// ��ȸ��ư Ŭ��
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnSearch_Click(object sender, System.EventArgs e)
		{
			ProgressStart();
			DisableButton();
			SearchSystemConfig();
			InitButton();
			ProgressStop();
		}

		/// <summary>
		/// �߰���ư Ŭ��
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnAdd_Click(object sender, System.EventArgs e)
		{			
			btnSave.Enabled   = true;

			ResetTextReadonly();
			ResetUserDetailText();			
		}

		/// <summary>
		/// �����ư Ŭ��
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnSave_Click(object sender, System.EventArgs e)
		{			
			SaveUserDetail();			
		}
				

		/// <summary>
		/// �˻��� Ŭ�� 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>		

		private void ebSearchKey_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
		{
			if(e.KeyCode == Keys.Enter)
			{
				SearchSystemConfig();
			}
		}



		
		#endregion

		#region ó���޼ҵ�

		/// <summary>
		/// ������ ��ȸ
		/// </summary>
		private void SearchSystemConfig()
		{
			StatusMessage("ȯ�漳���� ��ȸ�մϴ�.");

			try
			{
				systemConfigModel.Init();
				ResetUserDetailText();

				// ����ڸ����ȸ ���񽺸� ȣ���Ѵ�.
				new SystemConfigManager(systemModel,commonModel).GetSystemConfigList(systemConfigModel);

				if (systemConfigModel.ResultCD.Equals("0000"))
				{
					Utility.SetDataTable(systemConfigDs.SystemConfig, systemConfigModel.SystemConfigDataSet);				
					StatusMessage(systemConfigModel.ResultCnt + "���� ȯ�漳���� ��ȸ�Ǿ����ϴ�.");
					SetConfigDetailText();
				}
			}
			catch(FrameException fe)
			{
				FrameSystem.showMsgForm("ȯ�漳����ȸ����", new string[] {fe.ErrCode, fe.ResultMsg});
			}
			catch(Exception ex)
			{
				FrameSystem.showMsgForm("ȯ�漳����ȸ����",new string[] {"",ex.Message});
			}
		}

		
		/// <summary>
		/// ����ڻ����� ����
		/// </summary>
		private void SaveUserDetail()
		{
			StatusMessage("ȯ�漳�� ������ �����մϴ�.");

			if(ebFtpID.Text.Trim().Length == 0) 
			{
				MessageBox.Show("FtpID�� �Էµ��� �ʾҽ��ϴ�.","ȯ�漳�� ����", 
					MessageBoxButtons.OK, MessageBoxIcon.Information );
				ebFtpID.Focus();
				return;						
			}
			if(ebFtpPassword.Text.Trim().Length == 0) 
			{
				MessageBox.Show("Ftp��й�ȣ�� �Էµ��� �ʾҽ��ϴ�.","ȯ�漳�� ����", 
					MessageBoxButtons.OK, MessageBoxIcon.Information );
				ebFtpHost.Focus();
				return;				
			}

			try
			{

				// �����͸𵨿� ������ ������ ��Ʈ�Ѵ�.
				systemConfigModel.FtpUploadID     = ebFtpID.Text.Trim();
				systemConfigModel.FtpUploadPW     = Security.Encrypt(ebFtpPassword.Text);
				systemConfigModel.FtpUploadHost   = ebFtpHost.Text;
				systemConfigModel.FtpUploadPort   = ebFtpPort.Text; 				
				systemConfigModel.FtpUploadPath   = ebFtpPath.Text;
				systemConfigModel.FtpMovePath     = ebFtpMovePath.Text;
				
				if(rbFtpMoveUseYn_Y.Checked)	systemConfigModel.FtpMoveUseYn = "Y";
				else							systemConfigModel.FtpMoveUseYn = "N";

				systemConfigModel.FileQueueUseYn = "N";

				systemConfigModel.FtpCdnID			= ebFtpCdnID.Text.Trim();
				systemConfigModel.FtpCdnPW			= Security.Encrypt(ebFtpCdnPassword.Text);
				systemConfigModel.FtpCdnHost		= ebFtpCdnHost.Text;
				systemConfigModel.FtpCdnPort		= ebFtpCdnPort.Text; 				
				//systemConfigModel.FileQueueInterval = udFileQueueInterval.Value.ToString();
				//systemConfigModel.FileQueueCnt      = udFileQueueCnt.Value.ToString();
				//systemConfigModel.URLGetAdPopList	= ebURLGetAdPopList.Text;
				//systemConfigModel.URLSetAdPop		= ebURLSetAdPop.Text;
				systemConfigModel.CmsMasUrl			= ebCms_Url.Text.Trim();
				systemConfigModel.CmsMasQuery		= ebCms_Query.Text.Trim();

				// ����� ������ ���� ���񽺸� ȣ���Ѵ�.
				new SystemConfigManager(systemModel,commonModel).SetSystemConfigUpdate(systemConfigModel);
				StatusMessage("ȯ�漳�� �߰��Ǿ����ϴ�.");				
				ResetUserDetailText();
				
				
				DisableButton();
				SearchSystemConfig();
				InitButton();

			}
			catch(FrameException fe)
			{
				FrameSystem.showMsgForm("ȯ�漳�� �������", new string[] {fe.ErrCode, fe.ResultMsg});
			}
			catch(Exception ex)
			{
				FrameSystem.showMsgForm("ȯ�漳�� �������",new string[] {"",ex.Message});
			}			
		}

		
		
		/// <summary>
		/// ȯ�漳�� ��Ʈ
		/// </summary>
		private void SetConfigDetailText()
		{
			for(int i=0;i<systemConfigModel.ResultCnt;i++)
			{
				DataRow row = systemConfigDs.SystemConfig.Rows[i];

				ebFtpID.Text             = row["FtpUploadID"].ToString();				
				ebFtpPassword.Text       = Security.Decrypt(row["FtpUploadPW"].ToString());
				ebFtpHost.Text           = row["FtpUploadHost"].ToString();
				ebFtpPort.Text           = row["FtpUploadPort"].ToString();
				ebFtpPath.Text           = row["FtpUploadPath"].ToString();
				ebFtpMovePath.Text       = row["FtpMovePath"].ToString();

				if(row["FtpMoveUseYn"].ToString().Equals("Y"))
				{
					rbFtpMoveUseYn_Y.Checked = true;
					rbFtpMoveUseYn_N.Checked = false;

				}
				else
				{
					rbFtpMoveUseYn_Y.Checked = false;
					rbFtpMoveUseYn_N.Checked = true;
				}	
			
				ebFtpCdnID.Text             = row["FtpCdnID"].ToString();				
				ebFtpCdnPassword.Text       = Security.Decrypt(row["FtpCdnPW"].ToString());
				ebFtpCdnHost.Text           = row["FtpCdnHost"].ToString();
				ebFtpCdnPort.Text           = row["FtpCdnPort"].ToString();
				
				// CMS��������
				ebCms_Url.Text		= row["CmsMasUrl"].ToString();
				ebCms_Query.Text    = row["CmsMasQuery"].ToString();
			}
		}

		private void ResetUserDetailText()
		{
			ebFtpID.Text            = "";
			ebFtpPassword.Text      = "";
			ebFtpHost.Text          = "";
			ebFtpPort.Text          = "";			
			ebFtpPath.Text          = "";
			ebFtpMovePath.Text      = "";
			rbFtpMoveUseYn_Y.Checked= true;
			rbFtpMoveUseYn_N.Checked= false;		
			ebFtpCdnID.Text         = "";
			ebFtpCdnPassword.Text   = "";
			ebFtpCdnHost.Text       = "";
			ebFtpCdnPort.Text		= "";			

			ebCms_Url.Text			= "";
			ebCms_Query.Text		= "";
		}
		
		/// <summary>
		/// ������ ReadOnly
		/// </summary>
		private void SetTextReadonly()
		{
			ebFtpID.ReadOnly         = true;
			ebFtpPassword.ReadOnly   = true;
			ebFtpHost.ReadOnly       = true;
			ebFtpPort.ReadOnly       = true;			
			ebFtpPath.ReadOnly       = true;
			ebFtpMovePath.ReadOnly   = true;

			rbFtpMoveUseYn_Y.Enabled = false;
			rbFtpMoveUseYn_N.Enabled = false;
	
			ebFtpCdnID.ReadOnly         = true;
			ebFtpCdnPassword.ReadOnly   = true;
			ebFtpCdnHost.ReadOnly       = true;
			ebFtpCdnPort.ReadOnly       = true;			

			ebCms_Url.ReadOnly			= true;
			ebCms_Query.ReadOnly		= true;

			ebFtpID.BackColor        = Color.WhiteSmoke;
			ebFtpPassword.BackColor  = Color.WhiteSmoke;
			ebFtpHost.BackColor      = Color.WhiteSmoke;
			ebFtpPort.BackColor      = Color.WhiteSmoke;			
			ebFtpPath.BackColor      = Color.WhiteSmoke;
			ebFtpMovePath.BackColor  = Color.WhiteSmoke;

			ebFtpCdnID.BackColor        = Color.WhiteSmoke;
			ebFtpCdnPassword.BackColor  = Color.WhiteSmoke;
			ebFtpCdnHost.BackColor      = Color.WhiteSmoke;
			ebFtpCdnPort.BackColor      = Color.WhiteSmoke;			

			ebCms_Url.BackColor			= Color.WhiteSmoke;
			ebCms_Query.BackColor		= Color.WhiteSmoke;
		}

		/// <summary>
		/// ������ ����������
		/// </summary>
		private void ResetTextReadonly()
		{
			ebFtpID.ReadOnly         = false;
			ebFtpPassword.ReadOnly   = false;
			ebFtpHost.ReadOnly       = false;
			ebFtpPort.ReadOnly       = false;
			ebFtpPath.ReadOnly       = false;
			ebFtpMovePath.ReadOnly   = false;

			rbFtpMoveUseYn_Y.Enabled = true;
			rbFtpMoveUseYn_N.Enabled = true;

			ebFtpCdnID.ReadOnly         = false;
			ebFtpCdnPassword.ReadOnly   = false;
			ebFtpCdnHost.ReadOnly       = false;
			ebFtpCdnPort.ReadOnly       = false;

			ebCms_Url.ReadOnly			= false;
			ebCms_Query.ReadOnly		= false;
	
			ebFtpID.BackColor        = Color.White;
			ebFtpPassword.BackColor  = Color.White;
			ebFtpHost.BackColor      = Color.White;
			ebFtpPort.BackColor      = Color.White;
			ebFtpPath.BackColor      = Color.White;
			ebFtpMovePath.BackColor  = Color.White;

			ebFtpCdnID.BackColor        = Color.White;
			ebFtpCdnPassword.BackColor  = Color.White;
			ebFtpCdnHost.BackColor      = Color.White;
			ebFtpCdnPort.BackColor      = Color.White;

			ebCms_Url.BackColor			= Color.White;
			ebCms_Query.BackColor		= Color.White;

		}
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
	}
}
