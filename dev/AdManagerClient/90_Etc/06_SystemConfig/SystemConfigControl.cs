// ===============================================================================
// SystemConfigControl for Charites Project
//
// SystemConfigControl.cs
//
// 환경설정관리 컨드롤을 정의합니다. 
//
// ===============================================================================
// Release history
//	1. 2010-09-06 CMS관련 Url Query항목 추가함( CmsMasUrl,CmsMasQuery )
//		파일검수완료 작업시 테스트검수서버에서 연동하여 호출
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
	/// 사용자관리 컨트롤
	/// </summary>
	public class SystemConfigControl : System.Windows.Forms.UserControl, IUserControl
	{
		#region 이벤트핸들러
		public event StatusEventHandler 			StatusEvent;			// 상태이벤트 핸들러
		public event ProgressEventHandler 			ProgressEvent;			// 처리중이벤트 핸들러
		#endregion
			
		#region 사용자정의 객체 및 변수

		// 시스템 정보 : 화면공통
		private SystemModel   systemModel   = FrameSystem.oSysModel;
		private CommonModel   commonModel   = FrameSystem.oComModel;
		private Logger        log           = FrameSystem.oLog;
		private MenuPower     menu          = FrameSystem.oMenu;

		public string        menuCode		= "";

		// 사용할 정보모델
		SystemConfigModel systemConfigModel  = new SystemConfigModel();	// 환경설정모델

		// 화면처리용 변수		
		bool canRead			  = false;
		bool canUpdate			  = false;

		#endregion

        #region IUserControl 구현
        /// <summary>
        /// 메뉴 코드-보안이 필요한 화면에 필요함
        /// </summary>
        public string MenuCode
        {
            set { this.menuCode = value; }
            get { return this.menuCode; }
        }

        /// <summary>
        /// 부모컨트롤 지정
        /// </summary>
        /// <param name="control"></param>
        public void SetParent(Control control)
        {
            this.Parent = control;
        }
        /// <summary>
        /// DockStype지정
        /// </summary>
        /// <param name="style"></param>
        public void SetDockStyle(DockStyle style)
        {
            this.Dock = style;
        }
        #endregion

		#region 화면 컴포넌트, 생성자, 소멸자
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
			// 이 호출은 Windows.Forms Form 디자이너에 필요합니다.
			InitializeComponent();

			

		}

		/// <summary> 
		/// 사용 중인 모든 리소스를 정리합니다.
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

		#region 구성 요소 디자이너에서 생성한 코드
		/// <summary> 
		/// 디자이너 지원에 필요한 메서드입니다. 
		/// 이 메서드의 내용을 코드 편집기로 수정하지 마십시오.
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
			this.uiPanelUsers.Text = "환경설정";
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
			this.uiPanelUsersSearch.Text = "검색";
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
			this.uiGroupBox6.Text = "CMS연동 인터페이스";
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
			this.uiGroupBox5.Text = "CDN서버환경";
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
			this.label23.Text = "CDN서버의 FTP 사용자ID";
			this.label23.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label24
			// 
			this.label24.Location = new System.Drawing.Point(272, 40);
			this.label24.Name = "label24";
			this.label24.Size = new System.Drawing.Size(256, 21);
			this.label24.TabIndex = 42;
			this.label24.Text = "CDN서버의 FTP 비밀번호";
			this.label24.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label25
			// 
			this.label25.Location = new System.Drawing.Point(272, 64);
			this.label25.Name = "label25";
			this.label25.Size = new System.Drawing.Size(256, 21);
			this.label25.TabIndex = 39;
			this.label25.Text = "CDN서버의 FTP IP주소";
			this.label25.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label26
			// 
			this.label26.Location = new System.Drawing.Point(272, 88);
			this.label26.Name = "label26";
			this.label26.Size = new System.Drawing.Size(256, 21);
			this.label26.TabIndex = 40;
			this.label26.Text = "CDN서버의 FTP 접속포트";
			this.label26.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label19
			// 
			this.label19.Location = new System.Drawing.Point(8, 16);
			this.label19.Name = "label19";
			this.label19.Size = new System.Drawing.Size(72, 21);
			this.label19.TabIndex = 31;
			this.label19.Text = "FTP 아이디";
			this.label19.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label20
			// 
			this.label20.Location = new System.Drawing.Point(8, 40);
			this.label20.Name = "label20";
			this.label20.Size = new System.Drawing.Size(80, 24);
			this.label20.TabIndex = 33;
			this.label20.Text = "FTP 비밀번호";
			this.label20.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label21
			// 
			this.label21.Location = new System.Drawing.Point(8, 64);
			this.label21.Name = "label21";
			this.label21.Size = new System.Drawing.Size(72, 21);
			this.label21.TabIndex = 32;
			this.label21.Text = "FTP 호스트";
			this.label21.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label22
			// 
			this.label22.Location = new System.Drawing.Point(8, 88);
			this.label22.Name = "label22";
			this.label22.Size = new System.Drawing.Size(72, 21);
			this.label22.TabIndex = 34;
			this.label22.Text = "FTP 포트";
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
			this.uiGroupBox4.Text = "광고파일이동";
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(8, 16);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(80, 21);
			this.label1.TabIndex = 30;
			this.label1.Text = "이동요청위치";
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
			this.rbFtpMoveUseYn_Y.Text = "사용함";
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
			this.label2.Text = "이동요청사용";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// rbFtpMoveUseYn_N
			// 
			this.rbFtpMoveUseYn_N.Location = new System.Drawing.Point(176, 40);
			this.rbFtpMoveUseYn_N.Name = "rbFtpMoveUseYn_N";
			this.rbFtpMoveUseYn_N.Size = new System.Drawing.Size(88, 21);
			this.rbFtpMoveUseYn_N.TabIndex = 9;
			this.rbFtpMoveUseYn_N.Text = "사용안함";
			// 
			// label8
			// 
			this.label8.Location = new System.Drawing.Point(272, 16);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(256, 21);
			this.label8.TabIndex = 18;
			this.label8.Text = "CDN서버로의 이동요청위치";
			this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label9
			// 
			this.label9.Location = new System.Drawing.Point(272, 40);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(256, 21);
			this.label9.TabIndex = 18;
			this.label9.Text = "CDN서버로의 이동요청 사용여부";
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
			this.uiGroupBox1.Text = "검수서버환경";
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
			this.lbFtpID.Text = "FTP 아이디";
			this.lbFtpID.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// lbFtpPassword
			// 
			this.lbFtpPassword.Location = new System.Drawing.Point(8, 48);
			this.lbFtpPassword.Name = "lbFtpPassword";
			this.lbFtpPassword.Size = new System.Drawing.Size(80, 24);
			this.lbFtpPassword.TabIndex = 19;
			this.lbFtpPassword.Text = "FTP 비밀번호";
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
			this.lbFtpHost.Text = "FTP 호스트";
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
			this.lbFtpPort.Text = "FTP 포트";
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
			this.lbFtpPath.Text = "FTP 위치";
			this.lbFtpPath.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(272, 24);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(256, 21);
			this.label3.TabIndex = 18;
			this.label3.Text = "광고파일업로드 FTP의 사용자ID";
			this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(272, 48);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(256, 21);
			this.label4.TabIndex = 18;
			this.label4.Text = "광고파일업로드 FTP의 비밀번호";
			this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label5
			// 
			this.label5.Location = new System.Drawing.Point(272, 72);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(256, 21);
			this.label5.TabIndex = 18;
			this.label5.Text = "광고파일업로드 FTP의 IP주소";
			this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label6
			// 
			this.label6.Location = new System.Drawing.Point(272, 96);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(256, 21);
			this.label6.TabIndex = 18;
			this.label6.Text = "광고파일업로드 FTP의 접속포트";
			this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label7
			// 
			this.label7.Location = new System.Drawing.Point(272, 120);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(256, 21);
			this.label7.TabIndex = 18;
			this.label7.Text = "광고파일업로드 FTP의 업로드위치";
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
			this.btnSave.Text = "저 장";
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
			this.uiButton1.Text = "저 장";
			this.uiButton1.VisualStyle = Janus.Windows.UI.VisualStyle.Office2003;
			// 
			// uiButton2
			// 
			this.uiButton2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.uiButton2.Location = new System.Drawing.Point(8, 8);
			this.uiButton2.Name = "uiButton2";
			this.uiButton2.Size = new System.Drawing.Size(120, 24);
			this.uiButton2.TabIndex = 6;
			this.uiButton2.Text = "추 가";
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

		#region 컨트롤 로드
		private void UserControl_Load(object sender, System.EventArgs e)
		{
			// 컨트롤 초기화
			InitControl();		
		}

		#endregion

		#region 컨트롤 초기화
		private void InitControl()
		{			
			ProgressStart();		

			// 조회권한 검사
			if(menu.CanRead(MenuCode))
			{
				canRead = true;
				SearchSystemConfig();
			}

			// 사용자구분이 어드민 또는 수퍼유저인 경우만 전체  CRUD에 대한 설정을 처리한다.
			// 다른 사용자들은 자기정보만을 수정할 수 있도록 한다.
			if(commonModel.UserClass.Equals("10") || commonModel.UserClass.Equals("20"))
			{					
				// 저장버튼 활성화
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

		#region 사용자 액션처리 메소드

		/// <summary>
		/// 그리드의 Row변경시
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void OnGrdRowChanged(object sender, System.EventArgs e) 
		{		
			SetConfigDetailText();
			InitButton();
		}

		/// <summary>
		/// 조회버튼 클릭
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
		/// 추가버튼 클릭
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
		/// 저장버튼 클릭
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnSave_Click(object sender, System.EventArgs e)
		{			
			SaveUserDetail();			
		}
				

		/// <summary>
		/// 검색어 클릭 
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

		#region 처리메소드

		/// <summary>
		/// 데이터 조회
		/// </summary>
		private void SearchSystemConfig()
		{
			StatusMessage("환경설정를 조회합니다.");

			try
			{
				systemConfigModel.Init();
				ResetUserDetailText();

				// 사용자목록조회 서비스를 호출한다.
				new SystemConfigManager(systemModel,commonModel).GetSystemConfigList(systemConfigModel);

				if (systemConfigModel.ResultCD.Equals("0000"))
				{
					Utility.SetDataTable(systemConfigDs.SystemConfig, systemConfigModel.SystemConfigDataSet);				
					StatusMessage(systemConfigModel.ResultCnt + "건의 환경설정가 조회되었습니다.");
					SetConfigDetailText();
				}
			}
			catch(FrameException fe)
			{
				FrameSystem.showMsgForm("환경설정조회오류", new string[] {fe.ErrCode, fe.ResultMsg});
			}
			catch(Exception ex)
			{
				FrameSystem.showMsgForm("환경설정조회오류",new string[] {"",ex.Message});
			}
		}

		
		/// <summary>
		/// 사용자상세정보 저장
		/// </summary>
		private void SaveUserDetail()
		{
			StatusMessage("환경설정 정보를 저장합니다.");

			if(ebFtpID.Text.Trim().Length == 0) 
			{
				MessageBox.Show("FtpID가 입력되지 않았습니다.","환경설정 저장", 
					MessageBoxButtons.OK, MessageBoxIcon.Information );
				ebFtpID.Focus();
				return;						
			}
			if(ebFtpPassword.Text.Trim().Length == 0) 
			{
				MessageBox.Show("Ftp비밀번호가 입력되지 않았습니다.","환경설정 저장", 
					MessageBoxButtons.OK, MessageBoxIcon.Information );
				ebFtpHost.Focus();
				return;				
			}

			try
			{

				// 데이터모델에 전송할 내용을 셋트한다.
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

				// 사용자 상세정보 저장 서비스를 호출한다.
				new SystemConfigManager(systemModel,commonModel).SetSystemConfigUpdate(systemConfigModel);
				StatusMessage("환경설정 추가되었습니다.");				
				ResetUserDetailText();
				
				
				DisableButton();
				SearchSystemConfig();
				InitButton();

			}
			catch(FrameException fe)
			{
				FrameSystem.showMsgForm("환경설정 저장오류", new string[] {fe.ErrCode, fe.ResultMsg});
			}
			catch(Exception ex)
			{
				FrameSystem.showMsgForm("환경설정 저장오류",new string[] {"",ex.Message});
			}			
		}

		
		
		/// <summary>
		/// 환경설정 셋트
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
				
				// CMS연동정보
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
		/// 상세정보 ReadOnly
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
		/// 상세정보 수정가능케
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

		#region 이벤트함수

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
