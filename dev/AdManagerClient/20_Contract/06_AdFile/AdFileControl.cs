// ===============================================================================
// AdFileControl for Charites Project
//
// AdFileControl.cs
//
// 광고파일관리 컨드롤을 정의합니다. 
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
	/// 광고파일관리 컨트롤
	/// </summary>
    public class AdFileControl : System.Windows.Forms.UserControl, IUserControl
	{
		#region 이벤트핸들러
		public event StatusEventHandler 			StatusEvent;			// 상태이벤트 핸들러
		public event ProgressEventHandler 			ProgressEvent;			// 처리중이벤트 핸들러

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
			
		#region 사용자정의 객체 및 변수

		// 시스템 정보 : 화면공통
		private SystemModel   systemModel   = FrameSystem.oSysModel;
		private CommonModel   commonModel   = FrameSystem.oComModel;
		private Logger        log           = FrameSystem.oLog;
		private MenuPower     menu          = FrameSystem.oMenu;

		// 메뉴코드 : 보안이 필요한 화면에 필요함
		public string        menuCode		= "";

		// 사용할 정보모델
		AdFileWideModel adFileWideModel = new AdFileWideModel();	// 광고파일배포모델
		AdFileModel		adFileModel		= new AdFileModel();		// 광고파일모델

		// 화면처리용 변수
		bool IsNewSearchKey		  = true;					// 검색어입력 여부
		CurrencyManager cm        = null;					// 데이터 그리드의 변경에 따른 데이터셋 관리를 위하여			
		DataTable       dt        = null;

		CurrencyManager cmRep     = null;					// 데이터 그리드의 변경에 따른 데이터셋 관리를 위하여			
		DataTable       dtRep     = null;

        bool IsSearching = false; // 조회중 상세화면이 업데이트 되는 것을 방지 하기위함 2011.11.29 JH.Park
		bool canRead			  = false;
		bool canUpdate			  = false;

		// FTP업로드정보
		string FtpUploadHost;
		string FtpUploadPort;
		string FtpUploadID;
		string FtpUploadPW;

		// Key 데이터
		string keyMediaCode	   = "1";
		string keyItemNo       = "";
		string keyAdType       = "";
		string keyAdState      = "";
		string keyFileState    = "";
		string strDefaultPath  = "";
		string newItemNo  = "";				//등록파일검색에서 파일을 등록후 업데이트시 이용될 팝업에서 넘어온 Key		
		string Flag  = "";					//Y:기등록된 파일을 사용할 경우(팝업윈도우에서 선택시 설정됨), 일반업로드

		// 파일업로드용
		private FtpManager	ftm;
		private	string		fileMax		= string.Empty;
		private bool		firstDraw	= true;
		private DateTime	start;		
		private TimeSpan	timeSpan;
		private int tm, m, s = 0;
		string fileState    = "";
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

		#region 구성 요소 디자이너에서 생성한 코드
		/// <summary> 
		/// 디자이너 지원에 필요한 메서드입니다. 
		/// 이 메서드의 내용을 코드 편집기로 수정하지 마십시오.
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
			this.uiPanelAdFile.CaptionFormatStyle.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
			this.uiPanelAdFile.CaptionVisible = Janus.Windows.UI.InheritableBoolean.True;
			this.uiPanelAdFile.CloseButtonVisible = Janus.Windows.UI.InheritableBoolean.False;
			this.uiPanelAdFile.Location = new System.Drawing.Point(0, 0);
			this.uiPanelAdFile.Name = "uiPanelAdFile";
			this.uiPanelAdFile.Size = new System.Drawing.Size(1010, 677);
			this.uiPanelAdFile.TabIndex = 1;
			this.uiPanelAdFile.Text = "광고파일관리";
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
			this.uiPanelSearch.Text = "검색";
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
			this.btnSearch.Text = "조 회";
			this.btnSearch.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
			this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
			// 
			// ebSearchKey
			// 
			this.ebSearchKey.Location = new System.Drawing.Point(270, 40);
			this.ebSearchKey.Name = "ebSearchKey";
			this.ebSearchKey.Size = new System.Drawing.Size(230, 21);
			this.ebSearchKey.TabIndex = 5;
			this.ebSearchKey.Text = "검색어를 입력하세요";
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
			this.cbSearchAdType.Text = "광고종류";
			this.cbSearchAdType.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
			// 
			// cbSearchFileType
			// 
			this.cbSearchFileType.ComboStyle = Janus.Windows.EditControls.ComboStyle.DropDownList;
			uiComboBoxItem1.FormatStyle.Alpha = 0;
			uiComboBoxItem1.IsSeparator = false;
			uiComboBoxItem1.Text = "광고파일구분 선택";
			uiComboBoxItem1.Value = "00";
			this.cbSearchFileType.Items.AddRange(new Janus.Windows.EditControls.UIComboBoxItem[] {
            uiComboBoxItem1});
			this.cbSearchFileType.Location = new System.Drawing.Point(142, 40);
			this.cbSearchFileType.Name = "cbSearchFileType";
			this.cbSearchFileType.SelectedIndex = 0;
			this.cbSearchFileType.Size = new System.Drawing.Size(120, 21);
			this.cbSearchFileType.TabIndex = 7;
			this.cbSearchFileType.Text = "광고파일구분 선택";
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
			this.chkAdState_30.Text = "중지";
			this.chkAdState_30.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
			// 
			// chkAdState_40
			// 
			this.chkAdState_40.Location = new System.Drawing.Point(205, 11);
			this.chkAdState_40.Name = "chkAdState_40";
			this.chkAdState_40.Size = new System.Drawing.Size(44, 21);
			this.chkAdState_40.TabIndex = 11;
			this.chkAdState_40.Text = "종료";
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
			this.chkAdState_10.Text = "대기";
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
			this.chkAdState_20.Text = "편성";
			this.chkAdState_20.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
			// 
			// chkFileState_20
			// 
			this.chkFileState_20.Location = new System.Drawing.Point(554, 12);
			this.chkFileState_20.Name = "chkFileState_20";
			this.chkFileState_20.Size = new System.Drawing.Size(67, 21);
			this.chkFileState_20.TabIndex = 15;
			this.chkFileState_20.Text = "CDN동기";
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
			this.chkFileState_10.Text = "미등록";
			this.chkFileState_10.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
			// 
			// chkFileState_90
			// 
			this.chkFileState_90.Location = new System.Drawing.Point(712, 12);
			this.chkFileState_90.Name = "chkFileState_90";
			this.chkFileState_90.Size = new System.Drawing.Size(67, 21);
			this.chkFileState_90.TabIndex = 17;
			this.chkFileState_90.Text = "셋탑삭제";
			this.chkFileState_90.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
			// 
			// chkFileState_30
			// 
			this.chkFileState_30.Location = new System.Drawing.Point(633, 12);
			this.chkFileState_30.Name = "chkFileState_30";
			this.chkFileState_30.Size = new System.Drawing.Size(67, 21);
			this.chkFileState_30.TabIndex = 16;
			this.chkFileState_30.Text = "배포완료";
			this.chkFileState_30.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
			// 
			// chkFileState_12
			// 
			this.chkFileState_12.Location = new System.Drawing.Point(475, 12);
			this.chkFileState_12.Name = "chkFileState_12";
			this.chkFileState_12.Size = new System.Drawing.Size(67, 21);
			this.chkFileState_12.TabIndex = 13;
			this.chkFileState_12.Text = "검수대기";
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
			this.uiPanelList.Text = "광고파일목록";
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
			this.uiPanelDetail.Text = "상세정보";
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
			this.TPDetail.Text = "상세정보";
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
			this.btnFileReplace.StateStyles.FormatStyle.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
			this.btnFileReplace.StateStyles.FormatStyle.FontBold = Janus.Windows.UI.TriState.True;
			this.btnFileReplace.StateStyles.FormatStyle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
			this.btnFileReplace.TabIndex = 223;
			this.btnFileReplace.Text = "소재교체";
			this.btnFileReplace.ToolTipText = "광고소재 교체를 위해 파일상태를 미등록상태로 변경합니다.";
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
			this.btnView.Text = "소재확인";
			this.btnView.ToolTipText = "CDN에서 파일을 다운받아 재생합니다";
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
			this.btnRegFileSearch.Text = "등록파일찾기";
			this.btnRegFileSearch.ToolTipText = "기존에 등록된 파일을 찾아서 등록합니다";
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
			this.chkFileUploadUseYN.Text = "파일업로드사용";
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
			this.lbNoSave.Text = "(배포완료 및 셋탑삭제 상태인 파일은 수정할 수 없습니다.)";
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
			this.btnSave.Text = "저 장";
			this.btnSave.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
			this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
			// 
			// cbDownLevel
			// 
			this.cbDownLevel.ComboStyle = Janus.Windows.EditControls.ComboStyle.DropDownList;
			uiComboBoxItem2.FormatStyle.Alpha = 0;
			uiComboBoxItem2.IsSeparator = false;
			uiComboBoxItem2.Text = "1 순위";
			uiComboBoxItem2.Value = "1";
			uiComboBoxItem3.FormatStyle.Alpha = 0;
			uiComboBoxItem3.IsSeparator = false;
			uiComboBoxItem3.Text = "2 순위";
			uiComboBoxItem3.Value = "2";
			uiComboBoxItem4.FormatStyle.Alpha = 0;
			uiComboBoxItem4.IsSeparator = false;
			uiComboBoxItem4.Text = "3 순위";
			uiComboBoxItem4.Value = "3";
			uiComboBoxItem5.FormatStyle.Alpha = 0;
			uiComboBoxItem5.IsSeparator = false;
			uiComboBoxItem5.Text = "4 순위";
			uiComboBoxItem5.Value = "4";
			uiComboBoxItem6.FormatStyle.Alpha = 0;
			uiComboBoxItem6.IsSeparator = false;
			uiComboBoxItem6.Text = "5 순위";
			uiComboBoxItem6.Value = "5";
			uiComboBoxItem7.FormatStyle.Alpha = 0;
			uiComboBoxItem7.IsSeparator = false;
			uiComboBoxItem7.Text = "6 순위";
			uiComboBoxItem7.Value = "6";
			uiComboBoxItem8.FormatStyle.Alpha = 0;
			uiComboBoxItem8.IsSeparator = false;
			uiComboBoxItem8.Text = "7 순위";
			uiComboBoxItem8.Value = "7";
			uiComboBoxItem9.FormatStyle.Alpha = 0;
			uiComboBoxItem9.IsSeparator = false;
			uiComboBoxItem9.Text = "8 순위";
			uiComboBoxItem9.Value = "8";
			uiComboBoxItem10.FormatStyle.Alpha = 0;
			uiComboBoxItem10.IsSeparator = false;
			uiComboBoxItem10.Text = "9 순위";
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
			this.cbDownLevel.Text = "다운순위";
			this.cbDownLevel.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
			// 
			// lbFileLength
			// 
			this.lbFileLength.BackColor = System.Drawing.Color.Transparent;
			this.lbFileLength.Location = new System.Drawing.Point(15, 60);
			this.lbFileLength.Name = "lbFileLength";
			this.lbFileLength.Size = new System.Drawing.Size(56, 21);
			this.lbFileLength.TabIndex = 214;
			this.lbFileLength.Text = "파일크기";
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
			this.lbAdFileType.Text = "파일구분";
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
			this.lbAdFileName.Text = "파일명";
			this.lbAdFileName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// lbRegDt
			// 
			this.lbRegDt.BackColor = System.Drawing.Color.Transparent;
			this.lbRegDt.Location = new System.Drawing.Point(15, 110);
			this.lbRegDt.Name = "lbRegDt";
			this.lbRegDt.Size = new System.Drawing.Size(56, 21);
			this.lbRegDt.TabIndex = 212;
			this.lbRegDt.Text = "파일등록";
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
			this.lbDownloadOrderLevel.Text = "다운순위";
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
			this.lbFilePath.Text = "파일위치";
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
			this.ebItemName.Text = "카즈톡톡_이벤트(상시)";
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
			this.lbContentName.Text = "광고명";
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
			this.label1.Text = "파일상태";
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
			this.label2.Text = "광고길이";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label4
			// 
			this.label4.BackColor = System.Drawing.Color.Transparent;
			this.label4.Location = new System.Drawing.Point(483, 60);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(18, 21);
			this.label4.TabIndex = 216;
			this.label4.Text = "초";
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
			this.btnAdFileSearch.Text = "파일찾기";
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
			this.label6.Text = "배포완료";
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
			this.label7.Text = "셋탑삭제";
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
			this.TPReplace.Text = "파일교체이력";
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
			this.label13.Text = "파일길이";
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
			this.label10.Text = "원본명";
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
			this.label8.Text = "검수완료";
			this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label9
			// 
			this.label9.BackColor = System.Drawing.Color.Transparent;
			this.label9.Location = new System.Drawing.Point(734, 57);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(52, 21);
			this.label9.TabIndex = 227;
			this.label9.Text = "파일등록";
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
			this.label11.Text = "배포완료";
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
			this.label12.Text = "셋탑삭제";
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
			this.TPPub.Text = "파일배포이력";
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

		#region 컨트롤 로드
		private void UserControl_Load(object sender, System.EventArgs e)
		{
			Application.DoEvents();

			// 데이터관리용 객체생성
			dt = ((DataView)grdExAdFileList.DataSource).Table;  
			cm = (CurrencyManager) this.BindingContext[grdExAdFileList.DataSource]; 
			cm.PositionChanged += new System.EventHandler(OnGrdRowChanged); 

			dtRep = ((DataView)gridEX_Replace.DataSource).Table;  
			cmRep = (CurrencyManager) this.BindingContext[gridEX_Replace.DataSource]; 
			cmRep.PositionChanged += new System.EventHandler(OnGrdRowChangedReplace); 

			// 컨트롤 초기화
			InitControl();	
		}

		#endregion

		#region 컨트롤 초기화
		private void InitControl()
		{
			ProgressStart();
			InitCombo();

			// 권한 설정
			if(menu.CanRead(MenuCode))      canRead = true;

            if(menu.CanUpdate(MenuCode))    canUpdate = true;

            if (menu.CanUpdate(MenuCode)) ResetFileInfoReadonly();
            else
            {
                SetFileInfoReadonly();
            }
            
			ResetDetailText();

            // FTP관련정보
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
			// 코드에서 광고종류를 조회한다.
			CodeModel codeModel = new CodeModel();
			codeModel.Section = "26";				// 코드분류 '26':광고종류 TODO: 코드분류는 추후 XML로 관리되어야...
			new CodeManager(systemModel, commonModel).GetCodeList(codeModel);
			
			if (codeModel.ResultCD.Equals("0000"))
			{
				// 데이터셋에 셋팅
				Utility.SetDataTable(adFileDs.AdType, codeModel.CodeDataSet);				
			}

			// 검색조건의 콤보
			this.cbSearchAdType.Items.Clear();
			
			// 콤보박스에 셋트할 코드목록을 담을 Item배열을 선언
			Janus.Windows.EditControls.UIComboBoxItem[] comboItems = new Janus.Windows.EditControls.UIComboBoxItem[codeModel.ResultCnt + 1];

			comboItems[0] = new Janus.Windows.EditControls.UIComboBoxItem("광고종류선택","00");
			
			for(int i=0;i<codeModel.ResultCnt;i++)
			{
				DataRow row = adFileDs.AdType.Rows[i];

				string val = row["Code"].ToString();
				string txt = row["CodeName"].ToString();
				comboItems[i+1] = new Janus.Windows.EditControls.UIComboBoxItem(txt,val);
			}
			// 콤보에 셋트
			this.cbSearchAdType.Items.AddRange(comboItems);
			this.cbSearchAdType.SelectedIndex = 0;

			Application.DoEvents();
		}

		private void Init_FileType()
		{
			// 코드에서 보안레벨을 조회한다.
			CodeModel codeModel = new CodeModel();
			codeModel.Section = "24";				// 코드분류 '24':광고파일구분  TODO: 코드분류는 추후 XML로 관리되어야...
			new CodeManager(systemModel, commonModel).GetCodeList(codeModel);
			
			if (codeModel.ResultCD.Equals("0000"))
			{
				// 데이터셋에 셋팅
				Utility.SetDataTable(adFileDs.FileType, codeModel.CodeDataSet);				
			}

			// 상세조회 콤보
			// 상세정보의 콤보는 Dataset을 데이터소스로 가진다.

			// 검색조건의 콤보
			this.cbSearchFileType.Items.Clear();
			
			// 콤보박스에 셋트할 코드목록을 담을 Item배열을 선언
			Janus.Windows.EditControls.UIComboBoxItem[] comboItems = new Janus.Windows.EditControls.UIComboBoxItem[codeModel.ResultCnt + 1];

			comboItems[0] = new Janus.Windows.EditControls.UIComboBoxItem("광고파일구분선택","00");
			
			for(int i=0;i<codeModel.ResultCnt;i++)
			{
				DataRow row = adFileDs.FileType.Rows[i];

				string val = row["Code"].ToString();
				string txt = row["CodeName"].ToString();
				comboItems[i+1] = new Janus.Windows.EditControls.UIComboBoxItem(txt,val);
			}
			// 콤보에 셋트
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

		#region 광고파일 액션처리 메소드

		/// <summary>
		/// 그리드의 Row변경시
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void OnGrdRowChanged(object sender, System.EventArgs e) 
		{
            if (!IsSearching) // 2011.11.29 JH.Park 조회중이 아닐경우에만 동작하도록 변경
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
				if(curRow < 0) return;	// 데이터가 없으면 실행하지 않는다.

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
		/// 조회버튼 클릭
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
		/// 저장버튼 클릭
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnSave_Click(object sender, System.EventArgs e)
		{
			SaveAdFile();
		}

		/// <summary>
		/// 검색어 변경
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void ebSearchKey_TextChanged(object sender, System.EventArgs e)
		{
			IsNewSearchKey = false;
		}

		/// <summary>
		/// 검색어 클릭 
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

		#region 처리메소드

		/// <summary>
		/// 광고파일목록 조회
		/// </summary>
		private void SearchAdFile()
		{
            IsSearching = true;
			StatusMessage("광고파일 정보를 조회합니다.");
			ProgressStart();

			try
			{
				adFileModel.Init();

				// 데이터모델에 전송할 내용을 셋트한다.
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


				// 광고파일목록조회 서비스를 호출한다.
				new AdFileManager(systemModel,commonModel).GetAdFileList(adFileModel);

				if (adFileModel.ResultCD.Equals("0000"))
				{
					Utility.SetDataTable(adFileDs.AdFile, adFileModel.AdFileDataSet);		
					StatusMessage(adFileModel.ResultCnt + "건의 광고파일 정보가 조회되었습니다.");
					AddSchChoice();									
					SetDetailText();
				}
			}
			catch(FrameException fe)
			{
				FrameSystem.showMsgForm("광고파일조회오류", new string[] {fe.ErrCode, fe.ResultMsg});
			}
			catch(Exception ex)
			{
				FrameSystem.showMsgForm("광고파일조회오류",new string[] {"",ex.Message});
			}
			finally
			{
                IsSearching = false; // 조회중 Flag 리셋
				ProgressStop();
			}
		}

		/// <summary>
		/// 키캆을찾아 그리드 키에 해당되는로우로..
		/// </summary>
		private void AddSchChoice()
		{
			StatusMessage("키캆");		

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
				FrameSystem.showMsgForm("키캆오류", new string[] {fe.ErrCode, fe.ResultMsg});
			}
			catch(Exception ex)
			{
				FrameSystem.showMsgForm("키캆오류",new string[] {"",ex.Message});
			}			
		}

		/// <summary>
		/// 파일배포승인 History
		/// </summary>
		private void SchedulePublish()
		{
			StatusMessage("파일배포이력를 조회합니다.");
		
			try
			{
				adFileModel.Init();
				adFileModel.SearchMediaCode = "1";
				adFileModel.ItemNo			=  keyItemNo;

				// 광고배포승인 처리 서비스를 호출한다.
				new AdFileManager(systemModel,commonModel).GetPublishHistory(adFileModel);

				if (adFileModel.ResultCD.Equals("0000"))
				{
					Utility.SetDataTable(adFileDs.History, adFileModel.AdFileDataSet);		
					StatusMessage(adFileModel.ResultCnt + "건의 광고파일 정보가 조회되었습니다.");
				}				
				filePubCnt = adFileModel.ResultCnt;
			}
			catch(FrameException fe)
			{
				FrameSystem.showMsgForm("파일배포이력 조회 오류", new string[] {fe.ErrCode, fe.ResultMsg});
			}
			catch(Exception ex)
			{
				FrameSystem.showMsgForm("파일배포이력 조회 오류",new string[] {"",ex.Message});
			}
		}


		/// <summary>
		/// 파일교체이력 조회
		/// </summary>
		private void FileRepHistoryList()
		{
			try
			{
				adFileModel.Init();
				adFileModel.SearchMediaCode = "1";
				adFileModel.ItemNo			=  keyItemNo;

				// 광고배포승인 처리 서비스를 호출한다.
				new AdFileManager(systemModel,commonModel).GetFileRePublishHistory(adFileModel);

				if (adFileModel.ResultCD.Equals("0000"))
				{
					adFileDs.AdFileHistory.Clear();
					Utility.SetDataTable(adFileDs.AdFileHistory, adFileModel.AdFileDataSet);		
					StatusMessage(adFileModel.ResultCnt + "건의 광고파일 정보가 조회되었습니다.");
				}
				fileRepCnt = adFileModel.ResultCnt;
			}
			catch(FrameException fe)
			{
				FrameSystem.showMsgForm("파일교체이력 조회 오류", new string[] {fe.ErrCode, fe.ResultMsg});
			}
			catch(Exception ex)
			{
				FrameSystem.showMsgForm("파일교체이력 조회 오류",new string[] {"",ex.Message});
			}
		}


		/// <summary>
		/// 광고파일상세정보 저장
		/// </summary>
		private void SaveAdFile()
		{
			StatusMessage("광고파일 정보를 저장합니다.");

			// 등록파일찾기로 들어온경우
			if(Flag.Equals("Y"))
			{
				#region 등록파일 찾기 처리
				ProgressStart();
				// 파일정보 저장
				try
				{					
					adFileModel.Init();

					// 데이터모델에 전송할 내용을 셋트한다.
					adFileModel.ItemNo         = keyItemNo;
					adFileModel.newItemNo      = newItemNo;
					adFileModel.ItemName       = ebItemName.Text.Trim();
					adFileModel.FileType       = cbFileType.SelectedValue.ToString(); 
					adFileModel.FileLength     = ebFileLength.Text.Trim().Replace(",","");
					adFileModel.FilePath       = ebFilePath.Text.Trim();
					adFileModel.PreFileName    = ebPreFileName.Text.Trim();
					adFileModel.FileName       = ebFileName.Text.Trim();
					adFileModel.DownLevel      = cbDownLevel.SelectedValue.ToString();

					// 광고파일 등록 서비스를 호출한다.
					new AdFileManager(systemModel,commonModel).SetFileUpdate(adFileModel);
            
					StatusMessage("광고파일 정보가 등록되었습니다.");
            
					ResetDetailText();
					DisableButton();
					SearchAdFile();
					InitButton();				
					
				}
				catch(FrameException fe)
				{
					FrameSystem.showMsgForm("광고파일정보 저장 오류", new string[] {fe.ErrCode, fe.ResultMsg});
				}
				catch(Exception ex)
				{
					FrameSystem.showMsgForm("광고파일정보 저장 오류",new string[] {"",ex.Message});
				}			
				ProgressStop();
				#endregion
			}
			else
			{
				#region 입력항목검증
				if(ebFileName.Text.Trim().Length == 0) 
				{
					MessageBox.Show("광고파일명이 입력되지 않았습니다.","광고파일정보 저장", MessageBoxButtons.OK, MessageBoxIcon.Information );
					return;					
				}

				if(ebFileLength.Text.Trim().Length == 0) 
				{
					MessageBox.Show("광고파일크기가 입력되지 않았습니다.","광고파일정보 저장", MessageBoxButtons.OK, MessageBoxIcon.Information );
					return;					
				}
			
				if(ebFilePath.Text.Trim().Length == 0) 
				{
					MessageBox.Show("광고파일위치가 입력되지 않았습니다.","광고파일정보 저장", MessageBoxButtons.OK, MessageBoxIcon.Information );
					return;					
				}
				#endregion

				#region 테스트검수서버에 파일업로드처리
				if((chkFileUploadUseYN.Visible == false) || ((chkFileUploadUseYN.Visible == true)  && chkFileUploadUseYN.Checked))
				{
					// 파일을 선택하였으면 파일 FTP업로드
					if (ebLocalFile.Text.Trim().Length == 0)
					{
						MessageBox.Show("광고파일이 선택되지 않았습니다.","광고파일정보 저장", MessageBoxButtons.OK, MessageBoxIcon.Information );
						return;					
					}

					DialogResult result  = MessageBox.Show("["+ebItemName.Text + "]을(를) 업로드 하시겠습니까?\n파일명:" + ebFileName.Text 
						, "파일 업로드"
						,MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
				
					if (result  != DialogResult.Yes)	return;

					// 메시지창을 닫기위해 잠깐 쉬었다가..
					Application.DoEvents();				
					Thread.Sleep(100);				
					ProgressStart();

					try
					{

						if (ftm.IsConnected == false)
							ftm.ConnectFtp();

						// 같은위치에 같은파일명으로 존재하는지 검사
						if (checkFile(ebFilePath.Text, ebFileName.Text))
						{
							FrameSystem.showMsgForm("파일 업로드 오류",new string[] {"파일명: " + ebFilePath.Text + "/" + this.ebFileName.Text, "같은 이름의 파일이 존재합니다.", "다른 이름으로 변경하세요!"});
							ProgressStop();
							return;
						}

						string remotePath = "";

						try
						{				
							ftm.ChangeDir(ebFilePath.Text);	//해당 위치로 디렉터릭를 변경한다.
						}
						catch
						{
							//오류가 난다면 해당 디렉터리가 없는것...
							//새로 만든다.
							ftm.MakeDir(ebFilePath.Text);
						}

						// 해당 디렉터리를 변경한다.
						remotePath = ftm.ChangeDir(ebFilePath.Text);

						// 진행바를 보여준다.
						lbFileFlow.Visible = true;
						lbTimeSpan.Visible = true;
						progBar.Visible    = true;

						ftm.Upload(ebLocalFile.Text, ebFileName.Text);
					}
					catch(Exception ex)
					{
						ProgressStop();
						FrameSystem.showMsgForm("파일 업로드 오류",new string[] {"",ex.Message});					
						return;
					}			
					ProgressStop();
				}
				#endregion

				#region 파일상태를 변경한다(검수대기)
				try
				{
					adFileModel.Init();

					// 데이터모델에 전송할 내용을 셋트한다.
					adFileModel.ItemNo         = keyItemNo;
					adFileModel.ItemName       = ebItemName.Text.Trim();
					adFileModel.FileType       = cbFileType.SelectedValue.ToString(); 
					adFileModel.FileLength     = ebFileLength.Text.Trim().Replace(",","");
					adFileModel.FilePath       = ebFilePath.Text.Trim();
					adFileModel.PreFileName    = ebPreFileName.Text.Trim();
					adFileModel.FileName       = ebFileName.Text.Trim();
					adFileModel.DownLevel      = cbDownLevel.SelectedValue.ToString();

					// 광고파일 등록 서비스를 호출한다.
					new AdFileManager(systemModel,commonModel).SetAdFileUpdate(adFileModel);
                
					StatusMessage("광고파일 정보가 등록되었습니다.");
                
					ResetDetailText();
					DisableButton();
					SearchAdFile();
					InitButton();
				}
				catch(FrameException fe)
				{
					FrameSystem.showMsgForm("광고파일정보 저장 오류", new string[] {fe.ErrCode, fe.ResultMsg});
				}
				catch(Exception ex)
				{
					FrameSystem.showMsgForm("광고파일정보 저장 오류",new string[] {"",ex.Message});
				}
				#endregion
			}
		}


		private	int	filePubCnt	= 0;
		private	int	fileRepCnt	= 0;

		/// <summary>
		/// 광고파일 상세정보의 셋트
		/// </summary>
		private void SetDetailText()
		{
			int curRow = cm.Position;
			if(curRow < 0) return;	// 데이터가 없으면 실행하지 않는다.

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

			SchedulePublish();			// 파일배포이력조회
			FileRepHistoryList();		// 파일교체이력조회

			TPPub.Text = "배포이력( "		+ filePubCnt.ToString() + " )";
			TPReplace.Text = "교체이력( "	+ fileRepCnt.ToString() + " )";


			// 파일의 상태
			// 10:미등록		11:소재교체대기		12:검수대기	15:배포대기	20:CDN동기화	30:배포완료	90:셋탑삭제
			if(keyFileState.Equals("12") || keyFileState.Equals("15") || keyFileState.Equals("20") || keyFileState.Equals("30") || keyFileState.Equals("90"))
				btnView.Enabled = true;
			else
				btnView.Enabled = false;

			if(keyFileState.Equals("10") || keyFileState.Equals("11") || keyFileState.Equals("12") || keyFileState.Equals("15") || keyFileState.Equals("20"))	// 파일상태가 배포확인 또는 셋탑삭제 상태가 아니면 수정가능
			{
				// 준비상태이변 기본값을 셋트
				if(keyFileState.Equals("10"))
				{
					ResetDetailText();
				}
				// 수정가능하게
				ResetFileInfoReadonly();
                
				if(keyFileState.Equals("10") || keyFileState.Equals("11") || keyFileState.Equals("12") )	// 파일상태가 미등록 또는 검수대기 상태이면 파일업로드 가능
				{
					// 업로드가능하도록
					SetAutoUpload();
				}
				
				// 수정가능안내문구 비활성
				lbNoSave.Visible = false;
			}
			else
			{
				// 수정불가능하게
				SetFileInfoReadonly();
				// 수정가능안내문구 활성
				lbNoSave.Visible = true;
			}

			StatusMessage("준비");
		}

		private void ResetDetailText()
		{
			ebFileName.Text          = "";
			cbFileType.SelectedIndex = 0;
			cbDownLevel.SelectedIndex = 3;	// Default 3순위
			ebFileLength.Text        = "";
			ebFilePath.Text          = strDefaultPath;
			ebRegDt.Text             = "";
			ebRegName.Text           = "";

			ebLocalFile.Text         = "";

			// 파일교체이력 상세항목들
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
		/// 파일정보 수정불가능케
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
		/// 파일정보 수정가능케
		/// </summary>
		private void ResetFileInfoReadonly()
		{
			cbFileType.ReadOnly		= false;
			cbDownLevel.ReadOnly	= false;

			cbFileType.BackColor	= Color.White;
			cbDownLevel.BackColor	= Color.White;

			// 파일명 및 파일크기는 임의 수정이 불가하다.
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
		/// 자동업로드 가능하게
		/// </summary>
		private void SetAutoUpload()
		{
			// 파일명 및 파일크기는 임의 수정이 불가하다.
			// 파일업로드를 통해서만 수정할 수 있다.
			ebFilePath.ReadOnly      = false;
			ebFilePath.BackColor     = Color.White;

			// 2007.10.23
			// 만약 테스트라면 파일명을 수정할 수 있도록한다. 
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


			// 업로드버튼 활성화
			if(canUpdate)
			{
				btnAdFileSearch.Enabled  = true;
				btnRegFileSearch.Enabled  = true;
			}
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

		#region FTP처리함수

        /// <summary>
        /// DB에서 FTP관련 정보를 읽어온다.
        /// </summary>
		private void InitFtpInfo()
		{
			try
			{
				adFileModel.Init();

				// 광고파일목록조회 서비스를 호출한다.
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
				FrameSystem.showMsgForm("FTP업로드 정보조회 오류", new string[] {fe.ErrCode, fe.ResultMsg});
			}
			catch(Exception ex)
			{
				FrameSystem.showMsgForm("FTP업로드 정보조회 오류",new string[] {"",ex.Message});
			}
		}

		private void createFtp()
		{
			//--------------
			// Ftp 객체 생성
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

					//BMK PORT가 0이면 업로드 사용하지 않게 설정한다.
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
				FrameSystem.oLog.Error("FTP서버 연결오류:"+ex.Message);
			}
		}

		private bool checkFile(string Path, string FileName)
		{
			//------------------
			// 서버상의 파일존재여부 체크
			//------------------
			if (ftm.IsConnected == false)
			{
				// 미연결시 3회시도
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
			// 현재 파일 상태 프로그레스바에 그리기
			//-----------------------------------
			try
			{
				if (firstDraw)
				{
					start = DateTime.Now;	// 업로드 시작 시각
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
				mf.SetMessage = new string[]{"" ,"업로드 중 알 수 없는 에러가 발생했습니다.!",ex.Message};
				mf.showMessage();
				mf.ShowDialog();
			}
		}

		private void ftm_OnMaxPosition(int inx)
		{
			//-------------------------------------------------
			// 다운로드 파일의 크기 값을 프로그레스 maxValue에 설정
			//-------------------------------------------------
			fileMax = string.Format("{0:0,00#}",inx);
			progBar.Maximum = inx;
		}

		private void messgaeForm()
		{
			MessageForm msgFrm = new MessageForm();
			msgFrm.SetMessageType = 1;
			msgFrm.SetMessage = new string[]{"","업로드 준비 중입니다..","잠시만 기다리세요.!"};
			msgFrm.Width -= 50;
			msgFrm.showMessage();
			msgFrm.ShowDialog();
		}
		#endregion

		#region 파일검색
		private void btnAdFileSearch_Click(object sender, System.EventArgs e)
		{
			if(Flag.Equals("Y"))
			{
				DialogResult result = MessageBox.Show("등록파일검색을 종료하고 파일검색을 하시겠습니까?\n","파일",
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
						openFileDlg.Filter	= "B Mobile ADS contents files (*.ts)|*.ts|모든파일 (*.*)|*.*";	
						openFileDlg.FilterIndex = 1;
						openFileDlg.RestoreDirectory = true;
					
						if (openFileDlg.ShowDialog() == DialogResult.OK)
						{
							Application.DoEvents();

							openFileDlg.ShowReadOnly = true;
																	
							string path = openFileDlg.FileName;
						
							FileStream fs		 =  new FileStream(path, System.IO.FileMode.Open, System.IO.FileAccess.Read);					
						
							ebFileLength.Text	= fileMax = string.Format("{0:0,00#}",fs.Length);
	                                                                   				
							string localName= Path.GetFileName(path);	//파일 이름만 - Table 에 등록되는 데이타

							string ExtName = Path.GetExtension(path);	// 확장자명

							// DB및 FTP에 저장할 파일명을 생성한다.
							// 생성규칙
							// 규칙 Prefix(1) + 광고번호(6) + '-' + yyMMddHHmmss + '.ts'
							// Prefix : CM(10)->c OAP(20)->o EAP(11)->o SCM(12)->s 기타(ELSE)->x
							// Prefix : 기본광고(99)->DefaultAd
						
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
							else if(keyAdType.Equals("99")) // 기본광고
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
								// 저장할 파일명-기본광고일 경우는 광고번호까지만..
								ebFileName.Text = Prefix + keyItemNo + ExtName + "/playlist.m3u8";
							}
							else
							{
								// 저장할 파일명
								ebFileName.Text = Prefix + keyItemNo + "-" + Timestmp + ExtName + "/playlist.m3u8";
							}
							// 로컬파일명
							ebLocalFile.Text = openFileDlg.FileName;

							progBar.Maximum = Convert.ToInt32(fs.Length * 0.001);
							fs.Close();
						}
					}
					catch(Exception ex)
					{
						FrameSystem.oLog.Error("파일선택예외:"+ex.Message);
					}
				}
			}
			else
			{
				try
				{
					openFileDlg.DefaultExt = "ts";
					openFileDlg.Filter = "B Mobile ADS contents files (*.ts)|*.ts|모든파일 (*.*)|*.*";	
					openFileDlg.FilterIndex = 1;
					openFileDlg.RestoreDirectory = true;
					
					if (openFileDlg.ShowDialog() == DialogResult.OK)
					{
						Application.DoEvents();

						openFileDlg.ShowReadOnly = true;
																	
						string path = openFileDlg.FileName;
						
						FileStream fs		 =  new FileStream(path, System.IO.FileMode.Open, System.IO.FileAccess.Read);					
						
						ebFileLength.Text	= fileMax = string.Format("{0:0,00#}",fs.Length);
	                                                                   				
						string localName= Path.GetFileName(path);	//파일 이름만 - Table 에 등록되는 데이타

						string ExtName = Path.GetExtension(path);	// 확장자명

						// DB및 FTP에 저장할 파일명을 생성한다.
						// 생성규칙
						// 규칙 Prefix(1) + 광고번호(6) + '-' + yyMMddHHmmss + '.ts'
						// Prefix : CM(10)->c OAP(20)->o EAP(11)->o SCM(12)->s 기타(ELSE)->x
						
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
						else if (keyAdType.Equals("99")) // 기본광고
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
							// 저장할 파일명-기본광고일 경우는 광고번호까지만..
							ebFileName.Text = Prefix + keyItemNo + ExtName + "/playlist.m3u8";
						}
						else
						{
							// 저장할 파일명
							ebFileName.Text = Prefix + keyItemNo + "-" + Timestmp + ExtName + "/playlist.m3u8"; 
						}
						// 로컬파일명
						ebLocalFile.Text = openFileDlg.FileName;

						progBar.Maximum = Convert.ToInt32(fs.Length * 0.001);
						fs.Close();
					}
				}
				catch(FrameException fe)
				{
					FrameSystem.showMsgForm("광고파일조회오류", new string[] {fe.ErrCode, fe.ResultMsg});
				}
				catch(Exception ex)
				{
					FrameSystem.showMsgForm("광고파일조회오류",new string[] {"",ex.Message});
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
					if(keyFileState.Equals("10") || keyFileState.Equals("11") || keyFileState.Equals("12") || keyFileState.Equals("15") || keyFileState.Equals("20"))	// 파일상태가 배포확인 또는 셋탑삭제 상태가 아니면 수정가능
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
			//등록파일검색 목록 검색 팝업 띠우기
			AdFile_pForm pForm = new AdFile_pForm(this);

			pForm.ShowDialog();
            
			pForm.Dispose();
			pForm = null;	
		}

		/// <summary>
		/// 선택된 Row들을 입력시킴
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
				FrameSystem.showMsgForm("광고파일 보기 오류",new string[] {"",ex.Message});
			}			
		}

		/// <summary>
		/// 광고파일 상태를 교체대기 상태로 변경한다
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnFileReplace_Click(object sender, System.EventArgs e)
		{
			StatusMessage("선택된 광고파일을 소재교체합니다.");

            DialogResult result  = MessageBox.Show("["+ebItemName.Text + "]의 광고파일 소재를 교체 합니다."
                                                    ,"파일관리"
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
				FrameSystem.showMsgForm("광고파일 소재교체 오류", new string[] {fe.ErrCode, fe.ResultMsg});
			}
			catch(Exception ex)
			{
				FrameSystem.showMsgForm("광고파일 소재교체 오류",new string[] {"",ex.Message});
			}			
			finally
			{
				ProgressStop();
			}			
		}
	}
}