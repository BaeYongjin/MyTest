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
	/// Ratio_InsertForm에 대한 요약 설명입니다.
	/// </summary>
	/// 


	public class Ratio_InsertForm : System.Windows.Forms.Form
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
		private Decimal[] LoadRate = new Decimal[4];	//처음 불러들였을 때 저장되는 비율

		// 메뉴코드 : 보안이 필요한 화면에 필요함
		public string        MenuCode		= "";

		// 사용할 정보모델
		ChannelGroupModel channelGroupModel  = new ChannelGroupModel();	// 장르정보모델
		RatioModel ratioModel  = new RatioModel();	// 편성비율모델
		SchChoiceAdModel schChoiceAdModel  = new SchChoiceAdModel();	// 지정광고편성모델

		//RatioControl RatioCtl = null;
        

		// 화면처리용 변수
		public String keyMediaCode   = null;
		public String keyItemNo   = null;
		CurrencyManager cm        = null;					// 데이터 그리드의 변경에 따른 데이터셋 관리를 위하여			
		DataTable       dt        = null;
		
		CurrencyManager cmMenu        = null;					// 데이터 그리드의 변경에 따른 데이터셋 관리를 위하여			
		CurrencyManager cmMenu2        = null;					// 데이터 그리드의 변경에 따른 데이터셋 관리를 위하여			
		CurrencyManager cmMenu3        = null;					// 데이터 그리드의 변경에 따른 데이터셋 관리를 위하여			
		CurrencyManager cmMenu4        = null;					// 데이터 그리드의 변경에 따른 데이터셋 관리를 위하여			
		CurrencyManager cmMenu5        = null;					// 데이터 그리드의 변경에 따른 데이터셋 관리를 위하여			
		
		DataTable       dtMenu    = null;
		DataTable       dtMenu2    = null;
		DataTable       dtMenu3    = null;
		DataTable       dtMenu4    = null;
		DataTable       dtMenu5    = null;

		public string keyCategoryCode       = "";
		public string keyGenreCode       = "";

		private int KeyEntryRate1 = 0;
		private int KeyEntryRate2 = 0;
		private int KeyEntryRate3 = 0;
		private int KeyEntryRate4 = 0;
		private int KeyEntryRate5 = 0;
		private int KeyTotalRate = 0;

		bool canRead			  = false;
		bool canCreate            = false;
		bool canDelete            = false;

		// 이 창을 연 컨트롤
		RatioControl Opener = null;

		#endregion

		#region 화면 컴포넌트 생성

		private System.Windows.Forms.Panel pnlBtn;
		private Janus.Windows.EditControls.UIButton btnClose;
		private Janus.Windows.EditControls.UIButton btnOk;
		private System.Data.DataView dvGenre;
		private Janus.Windows.UI.Dock.UIPanelManager uiPM;
		private Janus.Windows.UI.Dock.UIPanelGroup uiPanel0;
		private Janus.Windows.UI.Dock.UIPanel uiPanel1;
		private Janus.Windows.UI.Dock.UIPanelInnerContainer uiPanel1Container;
		private Janus.Windows.UI.Dock.UIPanel uiPanel2;
		private Janus.Windows.UI.Dock.UIPanelInnerContainer uiPanel2Container;
		private Janus.Windows.UI.Dock.UIPanel uiPanel3;
		private Janus.Windows.UI.Dock.UIPanelInnerContainer uiPanel3Container;
		private Janus.Windows.UI.Dock.UIPanel uiPanelGroup1;
		private Janus.Windows.UI.Dock.UIPanelInnerContainer uiPanelGroup1Container;
		private Janus.Windows.UI.Dock.UIPanelGroup uiPanelGroup2;
		private Janus.Windows.UI.Dock.UIPanel uiPanelGroupB;
		private Janus.Windows.UI.Dock.UIPanelInnerContainer uiPanelGroupBContainer;
		private Janus.Windows.UI.Dock.UIPanel uiPanelGroupC;
        private Janus.Windows.UI.Dock.UIPanelInnerContainer uiPanelGroupCContainer;
		private System.Windows.Forms.Panel panel6;
        private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Panel panel3;
		private System.Windows.Forms.Panel panel4;
		private System.Windows.Forms.Panel panel5;
		private System.Windows.Forms.Panel panel7;
		private Janus.Windows.GridEX.GridEX grdExGenreList;
		private Janus.Windows.EditControls.UIGroupBox uiGroupBox1;
		private Janus.Windows.EditControls.UIGroupBox uiGroupBox2;
		private Janus.Windows.EditControls.UIGroupBox uiGroupBox3;
        private Janus.Windows.EditControls.UIGroupBox uiGroupBox4;
		private Janus.Windows.GridEX.EditControls.NumericEditBox ebEntryRate1;
		private System.Windows.Forms.Label lbEntryRate1;
		private Janus.Windows.GridEX.EditControls.NumericEditBox ebEntryRate2;
		private System.Windows.Forms.Label lbEntryRate2;
		private Janus.Windows.GridEX.EditControls.NumericEditBox ebEntryRate3;
		private System.Windows.Forms.Label lbEntryRate3;
		private Janus.Windows.GridEX.EditControls.NumericEditBox ebEntryRate4;
        private System.Windows.Forms.Label lbEntryRate4;
		private Janus.Windows.EditControls.UIButton btnDeleteMenu;
		private Janus.Windows.GridEX.GridEX grdExGroupList1;
		private Janus.Windows.GridEX.GridEX grdExGroupList2;
		private Janus.Windows.GridEX.GridEX grdExGroupList3;
        private Janus.Windows.GridEX.GridEX grdExGroupList4;
		private Janus.Windows.EditControls.UIButton btnAddMenu;
		private Janus.Windows.EditControls.UIButton btnAddMenu2;
		private Janus.Windows.EditControls.UIButton btnAddMenu3;
        private Janus.Windows.EditControls.UIButton btnAddMenu4;
		private Janus.Windows.EditControls.UIButton btnDeleteMenu2;
		private Janus.Windows.EditControls.UIButton btnDeleteMenu3;
        private Janus.Windows.EditControls.UIButton btnDeleteMenu4;
		private Janus.Windows.EditControls.UIButton btnSave;
		private Janus.Windows.EditControls.UIButton btnSave2;
		private Janus.Windows.EditControls.UIButton btnSave3;
        private Janus.Windows.EditControls.UIButton btnSave4;
		private AdManagerClient._40_Target.Ratio.RatioDs ratio_InsertDs;
		private System.Data.DataView dvGroup1;
		private System.Data.DataView dvGroup2;
		private System.Data.DataView dvGroup3;
		private System.Data.DataView dvGroup4;
		private System.Data.DataView dvGroup5;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private Janus.Windows.EditControls.UIButton btnDeleteSync;
		private System.ComponentModel.IContainer components;

		/// <summary>
		/// 데이터 넘겨야 할 넘
		/// </summary>
		/// <param name="sender"></param>
		public Ratio_InsertForm(UserControl parent)
		{
			//
			// Windows Form 디자이너 지원에 필요합니다.
			//
			InitializeComponent();

			//
			
			//
            
			//RatioCtl = sender;
			//keyMediaCode = paramMediaCode;
			// 이창을 호출한 컨트롤
			Opener = (RatioControl) parent;
			ratioModel.Init();
			schChoiceAdModel.Init();
		}

		/// <summary>
		/// 일반사용자
		/// </summary>
		public Ratio_InsertForm()
		{
			//
			// Windows Form 디자이너 지원에 필요합니다.
			//
			InitializeComponent();

			//
			
			//
            
			//RatioCtl = null;
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

		#region Windows Form 디자이너에서 생성한 코드
		/// <summary>
		/// 디자이너 지원에 필요한 메서드입니다.
		/// 이 메서드의 내용을 코드 편집기로 수정하지 마십시오.
		/// </summary>
		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            Janus.Windows.GridEX.GridEXLayout grdExGenreList_DesignTimeLayout = new Janus.Windows.GridEX.GridEXLayout();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Ratio_InsertForm));
            Janus.Windows.GridEX.GridEXLayout grdExGroupList1_DesignTimeLayout = new Janus.Windows.GridEX.GridEXLayout();
            Janus.Windows.GridEX.GridEXLayout grdExGroupList2_DesignTimeLayout = new Janus.Windows.GridEX.GridEXLayout();
            Janus.Windows.GridEX.GridEXLayout grdExGroupList3_DesignTimeLayout = new Janus.Windows.GridEX.GridEXLayout();
            Janus.Windows.GridEX.GridEXLayout grdExGroupList4_DesignTimeLayout = new Janus.Windows.GridEX.GridEXLayout();
            this.dvGenre = new System.Data.DataView();
            this.ratio_InsertDs = new AdManagerClient._40_Target.Ratio.RatioDs();
            this.pnlBtn = new System.Windows.Forms.Panel();
            this.btnDeleteSync = new Janus.Windows.EditControls.UIButton();
            this.btnClose = new Janus.Windows.EditControls.UIButton();
            this.btnOk = new Janus.Windows.EditControls.UIButton();
            this.uiPM = new Janus.Windows.UI.Dock.UIPanelManager(this.components);
            this.uiPanel0 = new Janus.Windows.UI.Dock.UIPanelGroup();
            this.uiPanel1 = new Janus.Windows.UI.Dock.UIPanel();
            this.uiPanel1Container = new Janus.Windows.UI.Dock.UIPanelInnerContainer();
            this.panel6 = new System.Windows.Forms.Panel();
            this.grdExGenreList = new Janus.Windows.GridEX.GridEX();
            this.uiPanel2 = new Janus.Windows.UI.Dock.UIPanel();
            this.uiPanel2Container = new Janus.Windows.UI.Dock.UIPanelInnerContainer();
            this.panel1 = new System.Windows.Forms.Panel();
            this.uiGroupBox1 = new Janus.Windows.EditControls.UIGroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnSave = new Janus.Windows.EditControls.UIButton();
            this.btnDeleteMenu = new Janus.Windows.EditControls.UIButton();
            this.btnAddMenu = new Janus.Windows.EditControls.UIButton();
            this.ebEntryRate1 = new Janus.Windows.GridEX.EditControls.NumericEditBox();
            this.lbEntryRate1 = new System.Windows.Forms.Label();
            this.uiGroupBox2 = new Janus.Windows.EditControls.UIGroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btnSave2 = new Janus.Windows.EditControls.UIButton();
            this.btnDeleteMenu2 = new Janus.Windows.EditControls.UIButton();
            this.btnAddMenu2 = new Janus.Windows.EditControls.UIButton();
            this.ebEntryRate2 = new Janus.Windows.GridEX.EditControls.NumericEditBox();
            this.lbEntryRate2 = new System.Windows.Forms.Label();
            this.uiGroupBox3 = new Janus.Windows.EditControls.UIGroupBox();
            this.label3 = new System.Windows.Forms.Label();
            this.btnSave3 = new Janus.Windows.EditControls.UIButton();
            this.btnDeleteMenu3 = new Janus.Windows.EditControls.UIButton();
            this.btnAddMenu3 = new Janus.Windows.EditControls.UIButton();
            this.ebEntryRate3 = new Janus.Windows.GridEX.EditControls.NumericEditBox();
            this.lbEntryRate3 = new System.Windows.Forms.Label();
            this.uiGroupBox4 = new Janus.Windows.EditControls.UIGroupBox();
            this.label4 = new System.Windows.Forms.Label();
            this.btnSave4 = new Janus.Windows.EditControls.UIButton();
            this.btnDeleteMenu4 = new Janus.Windows.EditControls.UIButton();
            this.btnAddMenu4 = new Janus.Windows.EditControls.UIButton();
            this.ebEntryRate4 = new Janus.Windows.GridEX.EditControls.NumericEditBox();
            this.lbEntryRate4 = new System.Windows.Forms.Label();
            this.uiPanelGroup2 = new Janus.Windows.UI.Dock.UIPanelGroup();
            this.uiPanel3 = new Janus.Windows.UI.Dock.UIPanel();
            this.uiPanel3Container = new Janus.Windows.UI.Dock.UIPanelInnerContainer();
            this.panel7 = new System.Windows.Forms.Panel();
            this.grdExGroupList1 = new Janus.Windows.GridEX.GridEX();
            this.dvGroup1 = new System.Data.DataView();
            this.uiPanelGroup1 = new Janus.Windows.UI.Dock.UIPanel();
            this.uiPanelGroup1Container = new Janus.Windows.UI.Dock.UIPanelInnerContainer();
            this.panel5 = new System.Windows.Forms.Panel();
            this.grdExGroupList2 = new Janus.Windows.GridEX.GridEX();
            this.dvGroup2 = new System.Data.DataView();
            this.uiPanelGroupB = new Janus.Windows.UI.Dock.UIPanel();
            this.uiPanelGroupBContainer = new Janus.Windows.UI.Dock.UIPanelInnerContainer();
            this.panel4 = new System.Windows.Forms.Panel();
            this.grdExGroupList3 = new Janus.Windows.GridEX.GridEX();
            this.dvGroup3 = new System.Data.DataView();
            this.uiPanelGroupC = new Janus.Windows.UI.Dock.UIPanel();
            this.uiPanelGroupCContainer = new Janus.Windows.UI.Dock.UIPanelInnerContainer();
            this.panel3 = new System.Windows.Forms.Panel();
            this.grdExGroupList4 = new Janus.Windows.GridEX.GridEX();
            this.dvGroup4 = new System.Data.DataView();
            this.dvGroup5 = new System.Data.DataView();
            ((System.ComponentModel.ISupportInitialize)(this.dvGenre)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ratio_InsertDs)).BeginInit();
            this.pnlBtn.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uiPM)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanel0)).BeginInit();
            this.uiPanel0.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanel1)).BeginInit();
            this.uiPanel1.SuspendLayout();
            this.uiPanel1Container.SuspendLayout();
            this.panel6.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdExGenreList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanel2)).BeginInit();
            this.uiPanel2.SuspendLayout();
            this.uiPanel2Container.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uiGroupBox1)).BeginInit();
            this.uiGroupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uiGroupBox2)).BeginInit();
            this.uiGroupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uiGroupBox3)).BeginInit();
            this.uiGroupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uiGroupBox4)).BeginInit();
            this.uiGroupBox4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelGroup2)).BeginInit();
            this.uiPanelGroup2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanel3)).BeginInit();
            this.uiPanel3.SuspendLayout();
            this.uiPanel3Container.SuspendLayout();
            this.panel7.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdExGroupList1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dvGroup1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelGroup1)).BeginInit();
            this.uiPanelGroup1.SuspendLayout();
            this.uiPanelGroup1Container.SuspendLayout();
            this.panel5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdExGroupList2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dvGroup2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelGroupB)).BeginInit();
            this.uiPanelGroupB.SuspendLayout();
            this.uiPanelGroupBContainer.SuspendLayout();
            this.panel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdExGroupList3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dvGroup3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelGroupC)).BeginInit();
            this.uiPanelGroupC.SuspendLayout();
            this.uiPanelGroupCContainer.SuspendLayout();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdExGroupList4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dvGroup4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dvGroup5)).BeginInit();
            this.SuspendLayout();
            // 
            // dvGenre
            // 
            this.dvGenre.Table = this.ratio_InsertDs.Genre;
            // 
            // ratio_InsertDs
            // 
            this.ratio_InsertDs.DataSetName = "RatioDs";
            this.ratio_InsertDs.Locale = new System.Globalization.CultureInfo("en-US");
            this.ratio_InsertDs.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // pnlBtn
            // 
            this.pnlBtn.BackColor = System.Drawing.Color.WhiteSmoke;
            this.pnlBtn.Controls.Add(this.btnDeleteSync);
            this.pnlBtn.Controls.Add(this.btnClose);
            this.pnlBtn.Controls.Add(this.btnOk);
            this.pnlBtn.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlBtn.Location = new System.Drawing.Point(0, 739);
            this.pnlBtn.Name = "pnlBtn";
            this.pnlBtn.Size = new System.Drawing.Size(912, 43);
            this.pnlBtn.TabIndex = 16;
            // 
            // btnDeleteSync
            // 
            this.btnDeleteSync.BackColor = System.Drawing.SystemColors.Control;
            this.btnDeleteSync.ImageIndex = 1;
            this.btnDeleteSync.Location = new System.Drawing.Point(568, 10);
            this.btnDeleteSync.Name = "btnDeleteSync";
            this.btnDeleteSync.Size = new System.Drawing.Size(104, 24);
            this.btnDeleteSync.TabIndex = 10;
            this.btnDeleteSync.Text = "편성Sync";
            this.btnDeleteSync.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.btnDeleteSync.Click += new System.EventHandler(this.btnDeleteSync_Click);
            // 
            // btnClose
            // 
            this.btnClose.BackColor = System.Drawing.SystemColors.Control;
            this.btnClose.ImageIndex = 1;
            this.btnClose.Location = new System.Drawing.Point(456, 10);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(70, 24);
            this.btnClose.TabIndex = 8;
            this.btnClose.Text = "닫기";
            this.btnClose.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnOk
            // 
            this.btnOk.BackColor = System.Drawing.SystemColors.Control;
            this.btnOk.ImageIndex = 0;
            this.btnOk.Location = new System.Drawing.Point(376, 10);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(70, 24);
            this.btnOk.TabIndex = 9;
            this.btnOk.Text = "확인";
            this.btnOk.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // uiPM
            // 
            this.uiPM.BackColorGradientAutoHideStrip = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(218)))), ((int)(((byte)(250)))));
            this.uiPM.ContainerControl = this;
            this.uiPM.VisualStyle = Janus.Windows.UI.Dock.PanelVisualStyle.Office2007;
            this.uiPanel0.Id = new System.Guid("3c38e80f-5bb0-4f3c-a2bd-89500d36545c");
            this.uiPanel0.StaticGroup = true;
            this.uiPanel1.Id = new System.Guid("eaafd831-2625-4579-8b1d-09018693cbd4");
            this.uiPanel0.Panels.Add(this.uiPanel1);
            this.uiPanel2.Id = new System.Guid("5cd0a44d-9722-4061-9714-21276f91cb64");
            this.uiPanel0.Panels.Add(this.uiPanel2);
            this.uiPanelGroup2.Id = new System.Guid("4fd8edc5-5259-478c-9446-2f5ff96bf65f");
            this.uiPanelGroup2.StaticGroup = true;
            this.uiPanel3.Id = new System.Guid("a93efb03-a1d8-4978-838e-547abe008a79");
            this.uiPanelGroup2.Panels.Add(this.uiPanel3);
            this.uiPanelGroup1.Id = new System.Guid("7e2a9842-9709-4d9d-973a-60b8915319ae");
            this.uiPanelGroup2.Panels.Add(this.uiPanelGroup1);
            this.uiPanelGroupB.Id = new System.Guid("ba4876e8-bda0-4ed2-a706-75e6cee1c516");
            this.uiPanelGroup2.Panels.Add(this.uiPanelGroupB);
            this.uiPanelGroupC.Id = new System.Guid("8bd291fc-2f88-4215-bf81-88f3cb95e004");
            this.uiPanelGroup2.Panels.Add(this.uiPanelGroupC);
            this.uiPanel0.Panels.Add(this.uiPanelGroup2);
            this.uiPM.Panels.Add(this.uiPanel0);
            // 
            // Design Time Panel Info:
            // 
            this.uiPM.BeginPanelInfo();
            this.uiPM.AddDockPanelInfo(new System.Guid("3c38e80f-5bb0-4f3c-a2bd-89500d36545c"), Janus.Windows.UI.Dock.PanelGroupStyle.VerticalTiles, Janus.Windows.UI.Dock.PanelDockStyle.Fill, true, new System.Drawing.Size(906, 733), true);
            this.uiPM.AddDockPanelInfo(new System.Guid("eaafd831-2625-4579-8b1d-09018693cbd4"), new System.Guid("3c38e80f-5bb0-4f3c-a2bd-89500d36545c"), 339, true);
            this.uiPM.AddDockPanelInfo(new System.Guid("5cd0a44d-9722-4061-9714-21276f91cb64"), new System.Guid("3c38e80f-5bb0-4f3c-a2bd-89500d36545c"), 184, true);
            this.uiPM.AddDockPanelInfo(new System.Guid("4fd8edc5-5259-478c-9446-2f5ff96bf65f"), new System.Guid("3c38e80f-5bb0-4f3c-a2bd-89500d36545c"), Janus.Windows.UI.Dock.PanelGroupStyle.HorizontalTiles, false, 375, true);
            this.uiPM.AddDockPanelInfo(new System.Guid("a93efb03-a1d8-4978-838e-547abe008a79"), new System.Guid("4fd8edc5-5259-478c-9446-2f5ff96bf65f"), 168, true);
            this.uiPM.AddDockPanelInfo(new System.Guid("7e2a9842-9709-4d9d-973a-60b8915319ae"), new System.Guid("4fd8edc5-5259-478c-9446-2f5ff96bf65f"), 144, true);
            this.uiPM.AddDockPanelInfo(new System.Guid("ba4876e8-bda0-4ed2-a706-75e6cee1c516"), new System.Guid("4fd8edc5-5259-478c-9446-2f5ff96bf65f"), 155, true);
            this.uiPM.AddDockPanelInfo(new System.Guid("8bd291fc-2f88-4215-bf81-88f3cb95e004"), new System.Guid("4fd8edc5-5259-478c-9446-2f5ff96bf65f"), 155, true);
            this.uiPM.AddFloatingPanelInfo(new System.Guid("3c38e80f-5bb0-4f3c-a2bd-89500d36545c"), Janus.Windows.UI.Dock.PanelGroupStyle.VerticalTiles, true, new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
            this.uiPM.AddFloatingPanelInfo(new System.Guid("eaafd831-2625-4579-8b1d-09018693cbd4"), new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
            this.uiPM.AddFloatingPanelInfo(new System.Guid("5cd0a44d-9722-4061-9714-21276f91cb64"), new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
            this.uiPM.AddFloatingPanelInfo(new System.Guid("a93efb03-a1d8-4978-838e-547abe008a79"), new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
            this.uiPM.AddFloatingPanelInfo(new System.Guid("69874ed1-87d3-47b9-a9b6-fb261ef02d85"), new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
            this.uiPM.AddFloatingPanelInfo(new System.Guid("7e2a9842-9709-4d9d-973a-60b8915319ae"), new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
            this.uiPM.AddFloatingPanelInfo(new System.Guid("e368b6d0-bb35-4455-9215-0a2ab8cab64a"), new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
            this.uiPM.AddFloatingPanelInfo(new System.Guid("8fd67a7a-5056-4b34-a771-88a4b040f55c"), new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
            this.uiPM.AddFloatingPanelInfo(new System.Guid("0ce48011-f114-437b-b4b7-0e3fc3d83939"), new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
            this.uiPM.AddFloatingPanelInfo(new System.Guid("ba4876e8-bda0-4ed2-a706-75e6cee1c516"), new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
            this.uiPM.AddFloatingPanelInfo(new System.Guid("8bd291fc-2f88-4215-bf81-88f3cb95e004"), new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
            this.uiPM.AddFloatingPanelInfo(new System.Guid("f54e4724-c70c-4bbd-84ce-06c7373bb7c6"), new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
            this.uiPM.EndPanelInfo();
            // 
            // uiPanel0
            // 
            this.uiPanel0.CaptionVisible = Janus.Windows.UI.InheritableBoolean.False;
            this.uiPanel0.GroupStyle = Janus.Windows.UI.Dock.PanelGroupStyle.VerticalTiles;
            this.uiPanel0.Location = new System.Drawing.Point(3, 3);
            this.uiPanel0.Name = "uiPanel0";
            this.uiPanel0.Size = new System.Drawing.Size(906, 733);
            this.uiPanel0.TabIndex = 4;
            this.uiPanel0.Text = "그룹1";
            // 
            // uiPanel1
            // 
            this.uiPanel1.InnerContainer = this.uiPanel1Container;
            this.uiPanel1.Location = new System.Drawing.Point(0, 0);
            this.uiPanel1.Name = "uiPanel1";
            this.uiPanel1.Size = new System.Drawing.Size(339, 733);
            this.uiPanel1.TabIndex = 4;
            this.uiPanel1.Text = "메뉴목록";
            // 
            // uiPanel1Container
            // 
            this.uiPanel1Container.Controls.Add(this.panel6);
            this.uiPanel1Container.Location = new System.Drawing.Point(1, 23);
            this.uiPanel1Container.Name = "uiPanel1Container";
            this.uiPanel1Container.Size = new System.Drawing.Size(337, 709);
            this.uiPanel1Container.TabIndex = 0;
            // 
            // panel6
            // 
            this.panel6.BackColor = System.Drawing.SystemColors.Window;
            this.panel6.Controls.Add(this.grdExGenreList);
            this.panel6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel6.Location = new System.Drawing.Point(0, 0);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(337, 709);
            this.panel6.TabIndex = 24;
            // 
            // grdExGenreList
            // 
            this.grdExGenreList.AlternatingColors = true;
            this.grdExGenreList.BorderStyle = Janus.Windows.GridEX.BorderStyle.None;
            this.grdExGenreList.DataSource = this.dvGenre;
            grdExGenreList_DesignTimeLayout.LayoutString = resources.GetString("grdExGenreList_DesignTimeLayout.LayoutString");
            this.grdExGenreList.DesignTimeLayout = grdExGenreList_DesignTimeLayout;
            this.grdExGenreList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdExGenreList.EmptyRows = true;
            this.grdExGenreList.FocusCellFormatStyle.BackColor = System.Drawing.SystemColors.Highlight;
            this.grdExGenreList.FocusStyle = Janus.Windows.GridEX.FocusStyle.None;
            this.grdExGenreList.Font = new System.Drawing.Font("나눔고딕", 8.249999F);
            this.grdExGenreList.FrozenColumns = 2;
            this.grdExGenreList.GridLineColor = System.Drawing.Color.Silver;
            this.grdExGenreList.GridLines = Janus.Windows.GridEX.GridLines.Vertical;
            this.grdExGenreList.GridLineStyle = Janus.Windows.GridEX.GridLineStyle.Solid;
            this.grdExGenreList.GroupByBoxVisible = false;
            this.grdExGenreList.Location = new System.Drawing.Point(0, 0);
            this.grdExGenreList.Name = "grdExGenreList";
            this.grdExGenreList.ScrollBars = Janus.Windows.GridEX.ScrollBars.Vertical;
            this.grdExGenreList.SelectedInactiveFormatStyle.BackColor = System.Drawing.Color.DarkOrange;
            this.grdExGenreList.Size = new System.Drawing.Size(337, 709);
            this.grdExGenreList.TabIndex = 17;
            this.grdExGenreList.ThemedAreas = ((Janus.Windows.GridEX.ThemedArea)(((((((((Janus.Windows.GridEX.ThemedArea.ScrollBars | Janus.Windows.GridEX.ThemedArea.EditControls) 
            | Janus.Windows.GridEX.ThemedArea.Headers) 
            | Janus.Windows.GridEX.ThemedArea.GroupByBox) 
            | Janus.Windows.GridEX.ThemedArea.GroupRows) 
            | Janus.Windows.GridEX.ThemedArea.ControlBorder) 
            | Janus.Windows.GridEX.ThemedArea.Cards) 
            | Janus.Windows.GridEX.ThemedArea.Gridlines) 
            | Janus.Windows.GridEX.ThemedArea.CheckBoxes)));
            this.grdExGenreList.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            this.grdExGenreList.CellValueChanged += new Janus.Windows.GridEX.ColumnActionEventHandler(this.grdExGenreList_CellValueChanged);
            this.grdExGenreList.ColumnHeaderClick += new Janus.Windows.GridEX.ColumnActionEventHandler(this.grdExGenreList_ColumnHeaderClick);
            // 
            // uiPanel2
            // 
            this.uiPanel2.InnerContainer = this.uiPanel2Container;
            this.uiPanel2.Location = new System.Drawing.Point(343, 0);
            this.uiPanel2.Name = "uiPanel2";
            this.uiPanel2.Size = new System.Drawing.Size(184, 733);
            this.uiPanel2.TabIndex = 4;
            this.uiPanel2.Text = "등록 및 삭제";
            // 
            // uiPanel2Container
            // 
            this.uiPanel2Container.Controls.Add(this.panel1);
            this.uiPanel2Container.Location = new System.Drawing.Point(1, 23);
            this.uiPanel2Container.Name = "uiPanel2Container";
            this.uiPanel2Container.Size = new System.Drawing.Size(182, 709);
            this.uiPanel2Container.TabIndex = 0;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.Window;
            this.panel1.Controls.Add(this.uiGroupBox1);
            this.panel1.Controls.Add(this.uiGroupBox2);
            this.panel1.Controls.Add(this.uiGroupBox3);
            this.panel1.Controls.Add(this.uiGroupBox4);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(182, 709);
            this.panel1.TabIndex = 25;
            // 
            // uiGroupBox1
            // 
            this.uiGroupBox1.Controls.Add(this.label1);
            this.uiGroupBox1.Controls.Add(this.btnSave);
            this.uiGroupBox1.Controls.Add(this.btnDeleteMenu);
            this.uiGroupBox1.Controls.Add(this.btnAddMenu);
            this.uiGroupBox1.Controls.Add(this.ebEntryRate1);
            this.uiGroupBox1.Controls.Add(this.lbEntryRate1);
            this.uiGroupBox1.Location = new System.Drawing.Point(16, 8);
            this.uiGroupBox1.Name = "uiGroupBox1";
            this.uiGroupBox1.Size = new System.Drawing.Size(156, 120);
            this.uiGroupBox1.TabIndex = 0;
            this.uiGroupBox1.Text = "그룹1";
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("굴림체", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label1.Location = new System.Drawing.Point(96, 32);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(16, 23);
            this.label1.TabIndex = 315;
            this.label1.Text = "%";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnSave.Enabled = false;
            this.btnSave.Location = new System.Drawing.Point(12, 78);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(136, 25);
            this.btnSave.TabIndex = 314;
            this.btnSave.Text = "비율수정";
            this.btnSave.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnDeleteMenu
            // 
            this.btnDeleteMenu.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnDeleteMenu.Enabled = false;
            this.btnDeleteMenu.Location = new System.Drawing.Point(116, 45);
            this.btnDeleteMenu.Name = "btnDeleteMenu";
            this.btnDeleteMenu.Size = new System.Drawing.Size(32, 26);
            this.btnDeleteMenu.TabIndex = 313;
            this.btnDeleteMenu.Text = "◀";
            this.btnDeleteMenu.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.btnDeleteMenu.Click += new System.EventHandler(this.btnDeleteMenu_Click);
            // 
            // btnAddMenu
            // 
            this.btnAddMenu.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnAddMenu.Enabled = false;
            this.btnAddMenu.Location = new System.Drawing.Point(116, 17);
            this.btnAddMenu.Name = "btnAddMenu";
            this.btnAddMenu.Size = new System.Drawing.Size(32, 26);
            this.btnAddMenu.TabIndex = 312;
            this.btnAddMenu.Text = "▶";
            this.btnAddMenu.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.btnAddMenu.Click += new System.EventHandler(this.btnAddMenu_Click);
            // 
            // ebEntryRate1
            // 
            this.ebEntryRate1.BackColor = System.Drawing.Color.White;
            this.ebEntryRate1.DecimalDigits = 0;
            this.ebEntryRate1.FormatString = "#,##0";
            this.ebEntryRate1.Location = new System.Drawing.Point(40, 31);
            this.ebEntryRate1.MaxLength = 4;
            this.ebEntryRate1.Name = "ebEntryRate1";
            this.ebEntryRate1.Size = new System.Drawing.Size(56, 21);
            this.ebEntryRate1.TabIndex = 311;
            this.ebEntryRate1.Text = "0";
            this.ebEntryRate1.TextAlignment = Janus.Windows.GridEX.TextAlignment.Far;
            this.ebEntryRate1.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.ebEntryRate1.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            // 
            // lbEntryRate1
            // 
            this.lbEntryRate1.Location = new System.Drawing.Point(8, 31);
            this.lbEntryRate1.Name = "lbEntryRate1";
            this.lbEntryRate1.Size = new System.Drawing.Size(32, 23);
            this.lbEntryRate1.TabIndex = 310;
            this.lbEntryRate1.Text = "비율";
            this.lbEntryRate1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // uiGroupBox2
            // 
            this.uiGroupBox2.Controls.Add(this.label2);
            this.uiGroupBox2.Controls.Add(this.btnSave2);
            this.uiGroupBox2.Controls.Add(this.btnDeleteMenu2);
            this.uiGroupBox2.Controls.Add(this.btnAddMenu2);
            this.uiGroupBox2.Controls.Add(this.ebEntryRate2);
            this.uiGroupBox2.Controls.Add(this.lbEntryRate2);
            this.uiGroupBox2.Location = new System.Drawing.Point(16, 168);
            this.uiGroupBox2.Name = "uiGroupBox2";
            this.uiGroupBox2.Size = new System.Drawing.Size(156, 121);
            this.uiGroupBox2.TabIndex = 1;
            this.uiGroupBox2.Text = "그룹2";
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("굴림체", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label2.Location = new System.Drawing.Point(96, 34);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(16, 23);
            this.label2.TabIndex = 317;
            this.label2.Text = "%";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnSave2
            // 
            this.btnSave2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnSave2.Enabled = false;
            this.btnSave2.Location = new System.Drawing.Point(16, 78);
            this.btnSave2.Name = "btnSave2";
            this.btnSave2.Size = new System.Drawing.Size(132, 25);
            this.btnSave2.TabIndex = 316;
            this.btnSave2.Text = "비율수정";
            this.btnSave2.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.btnSave2.Click += new System.EventHandler(this.btnSave2_Click);
            // 
            // btnDeleteMenu2
            // 
            this.btnDeleteMenu2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnDeleteMenu2.Enabled = false;
            this.btnDeleteMenu2.Location = new System.Drawing.Point(116, 45);
            this.btnDeleteMenu2.Name = "btnDeleteMenu2";
            this.btnDeleteMenu2.Size = new System.Drawing.Size(32, 26);
            this.btnDeleteMenu2.TabIndex = 315;
            this.btnDeleteMenu2.Text = "◀";
            this.btnDeleteMenu2.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.btnDeleteMenu2.Click += new System.EventHandler(this.btnDeleteMenu2_Click);
            // 
            // btnAddMenu2
            // 
            this.btnAddMenu2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnAddMenu2.Enabled = false;
            this.btnAddMenu2.Location = new System.Drawing.Point(116, 17);
            this.btnAddMenu2.Name = "btnAddMenu2";
            this.btnAddMenu2.Size = new System.Drawing.Size(32, 26);
            this.btnAddMenu2.TabIndex = 314;
            this.btnAddMenu2.Text = "▶";
            this.btnAddMenu2.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.btnAddMenu2.Click += new System.EventHandler(this.btnAddMenu2_Click);
            // 
            // ebEntryRate2
            // 
            this.ebEntryRate2.BackColor = System.Drawing.Color.White;
            this.ebEntryRate2.DecimalDigits = 0;
            this.ebEntryRate2.FormatString = "#,##0";
            this.ebEntryRate2.Location = new System.Drawing.Point(40, 34);
            this.ebEntryRate2.MaxLength = 4;
            this.ebEntryRate2.Name = "ebEntryRate2";
            this.ebEntryRate2.Size = new System.Drawing.Size(56, 21);
            this.ebEntryRate2.TabIndex = 312;
            this.ebEntryRate2.Text = "0";
            this.ebEntryRate2.TextAlignment = Janus.Windows.GridEX.TextAlignment.Far;
            this.ebEntryRate2.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.ebEntryRate2.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            // 
            // lbEntryRate2
            // 
            this.lbEntryRate2.Location = new System.Drawing.Point(8, 34);
            this.lbEntryRate2.Name = "lbEntryRate2";
            this.lbEntryRate2.Size = new System.Drawing.Size(40, 23);
            this.lbEntryRate2.TabIndex = 311;
            this.lbEntryRate2.Text = "비율";
            this.lbEntryRate2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // uiGroupBox3
            // 
            this.uiGroupBox3.Controls.Add(this.label3);
            this.uiGroupBox3.Controls.Add(this.btnSave3);
            this.uiGroupBox3.Controls.Add(this.btnDeleteMenu3);
            this.uiGroupBox3.Controls.Add(this.btnAddMenu3);
            this.uiGroupBox3.Controls.Add(this.ebEntryRate3);
            this.uiGroupBox3.Controls.Add(this.lbEntryRate3);
            this.uiGroupBox3.Location = new System.Drawing.Point(16, 327);
            this.uiGroupBox3.Name = "uiGroupBox3";
            this.uiGroupBox3.Size = new System.Drawing.Size(156, 121);
            this.uiGroupBox3.TabIndex = 2;
            this.uiGroupBox3.Text = "그룹3";
            // 
            // label3
            // 
            this.label3.Font = new System.Drawing.Font("굴림체", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label3.Location = new System.Drawing.Point(96, 34);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(16, 23);
            this.label3.TabIndex = 318;
            this.label3.Text = "%";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnSave3
            // 
            this.btnSave3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnSave3.Enabled = false;
            this.btnSave3.Location = new System.Drawing.Point(16, 78);
            this.btnSave3.Name = "btnSave3";
            this.btnSave3.Size = new System.Drawing.Size(132, 25);
            this.btnSave3.TabIndex = 317;
            this.btnSave3.Text = "비율수정";
            this.btnSave3.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.btnSave3.Click += new System.EventHandler(this.btnSave3_Click);
            // 
            // btnDeleteMenu3
            // 
            this.btnDeleteMenu3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnDeleteMenu3.Enabled = false;
            this.btnDeleteMenu3.Location = new System.Drawing.Point(116, 45);
            this.btnDeleteMenu3.Name = "btnDeleteMenu3";
            this.btnDeleteMenu3.Size = new System.Drawing.Size(32, 26);
            this.btnDeleteMenu3.TabIndex = 315;
            this.btnDeleteMenu3.Text = "◀";
            this.btnDeleteMenu3.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.btnDeleteMenu3.Click += new System.EventHandler(this.btnDeleteMenu3_Click);
            // 
            // btnAddMenu3
            // 
            this.btnAddMenu3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnAddMenu3.Enabled = false;
            this.btnAddMenu3.Location = new System.Drawing.Point(116, 17);
            this.btnAddMenu3.Name = "btnAddMenu3";
            this.btnAddMenu3.Size = new System.Drawing.Size(32, 26);
            this.btnAddMenu3.TabIndex = 314;
            this.btnAddMenu3.Text = "▶";
            this.btnAddMenu3.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.btnAddMenu3.Click += new System.EventHandler(this.btnAddMenu3_Click);
            // 
            // ebEntryRate3
            // 
            this.ebEntryRate3.BackColor = System.Drawing.Color.White;
            this.ebEntryRate3.DecimalDigits = 0;
            this.ebEntryRate3.FormatString = "#,##0";
            this.ebEntryRate3.Location = new System.Drawing.Point(40, 34);
            this.ebEntryRate3.MaxLength = 4;
            this.ebEntryRate3.Name = "ebEntryRate3";
            this.ebEntryRate3.Size = new System.Drawing.Size(56, 21);
            this.ebEntryRate3.TabIndex = 312;
            this.ebEntryRate3.Text = "0";
            this.ebEntryRate3.TextAlignment = Janus.Windows.GridEX.TextAlignment.Far;
            this.ebEntryRate3.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.ebEntryRate3.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            // 
            // lbEntryRate3
            // 
            this.lbEntryRate3.Location = new System.Drawing.Point(8, 34);
            this.lbEntryRate3.Name = "lbEntryRate3";
            this.lbEntryRate3.Size = new System.Drawing.Size(40, 23);
            this.lbEntryRate3.TabIndex = 311;
            this.lbEntryRate3.Text = "비율";
            this.lbEntryRate3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // uiGroupBox4
            // 
            this.uiGroupBox4.Controls.Add(this.label4);
            this.uiGroupBox4.Controls.Add(this.btnSave4);
            this.uiGroupBox4.Controls.Add(this.btnDeleteMenu4);
            this.uiGroupBox4.Controls.Add(this.btnAddMenu4);
            this.uiGroupBox4.Controls.Add(this.ebEntryRate4);
            this.uiGroupBox4.Controls.Add(this.lbEntryRate4);
            this.uiGroupBox4.Location = new System.Drawing.Point(16, 488);
            this.uiGroupBox4.Name = "uiGroupBox4";
            this.uiGroupBox4.Size = new System.Drawing.Size(156, 120);
            this.uiGroupBox4.TabIndex = 3;
            this.uiGroupBox4.Text = "그룹4";
            // 
            // label4
            // 
            this.label4.Font = new System.Drawing.Font("굴림체", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label4.Location = new System.Drawing.Point(96, 34);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(16, 23);
            this.label4.TabIndex = 318;
            this.label4.Text = "%";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnSave4
            // 
            this.btnSave4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnSave4.Enabled = false;
            this.btnSave4.Location = new System.Drawing.Point(16, 78);
            this.btnSave4.Name = "btnSave4";
            this.btnSave4.Size = new System.Drawing.Size(132, 25);
            this.btnSave4.TabIndex = 317;
            this.btnSave4.Text = "비율수정";
            this.btnSave4.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.btnSave4.Click += new System.EventHandler(this.btnSave4_Click);
            // 
            // btnDeleteMenu4
            // 
            this.btnDeleteMenu4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnDeleteMenu4.Enabled = false;
            this.btnDeleteMenu4.Location = new System.Drawing.Point(116, 45);
            this.btnDeleteMenu4.Name = "btnDeleteMenu4";
            this.btnDeleteMenu4.Size = new System.Drawing.Size(32, 26);
            this.btnDeleteMenu4.TabIndex = 315;
            this.btnDeleteMenu4.Text = "◀";
            this.btnDeleteMenu4.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.btnDeleteMenu4.Click += new System.EventHandler(this.btnDeleteMenu4_Click);
            // 
            // btnAddMenu4
            // 
            this.btnAddMenu4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnAddMenu4.Enabled = false;
            this.btnAddMenu4.Location = new System.Drawing.Point(116, 17);
            this.btnAddMenu4.Name = "btnAddMenu4";
            this.btnAddMenu4.Size = new System.Drawing.Size(32, 26);
            this.btnAddMenu4.TabIndex = 314;
            this.btnAddMenu4.Text = "▶";
            this.btnAddMenu4.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.btnAddMenu4.Click += new System.EventHandler(this.btnAddMenu4_Click);
            // 
            // ebEntryRate4
            // 
            this.ebEntryRate4.BackColor = System.Drawing.Color.White;
            this.ebEntryRate4.DecimalDigits = 0;
            this.ebEntryRate4.FormatString = "#,##0";
            this.ebEntryRate4.Location = new System.Drawing.Point(40, 34);
            this.ebEntryRate4.MaxLength = 4;
            this.ebEntryRate4.Name = "ebEntryRate4";
            this.ebEntryRate4.Size = new System.Drawing.Size(56, 21);
            this.ebEntryRate4.TabIndex = 312;
            this.ebEntryRate4.Text = "0";
            this.ebEntryRate4.TextAlignment = Janus.Windows.GridEX.TextAlignment.Far;
            this.ebEntryRate4.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.ebEntryRate4.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            // 
            // lbEntryRate4
            // 
            this.lbEntryRate4.Location = new System.Drawing.Point(8, 34);
            this.lbEntryRate4.Name = "lbEntryRate4";
            this.lbEntryRate4.Size = new System.Drawing.Size(40, 23);
            this.lbEntryRate4.TabIndex = 311;
            this.lbEntryRate4.Text = "비율";
            this.lbEntryRate4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // uiPanelGroup2
            // 
            this.uiPanelGroup2.Location = new System.Drawing.Point(531, 0);
            this.uiPanelGroup2.Name = "uiPanelGroup2";
            this.uiPanelGroup2.Size = new System.Drawing.Size(375, 733);
            this.uiPanelGroup2.TabIndex = 5;
            // 
            // uiPanel3
            // 
            this.uiPanel3.CaptionVisible = Janus.Windows.UI.InheritableBoolean.False;
            this.uiPanel3.InnerContainer = this.uiPanel3Container;
            this.uiPanel3.Location = new System.Drawing.Point(0, 0);
            this.uiPanel3.Name = "uiPanel3";
            this.uiPanel3.Size = new System.Drawing.Size(375, 195);
            this.uiPanel3.TabIndex = 4;
            this.uiPanel3.Text = "그룹1";
            // 
            // uiPanel3Container
            // 
            this.uiPanel3Container.Controls.Add(this.panel7);
            this.uiPanel3Container.Location = new System.Drawing.Point(1, 1);
            this.uiPanel3Container.Name = "uiPanel3Container";
            this.uiPanel3Container.Size = new System.Drawing.Size(373, 193);
            this.uiPanel3Container.TabIndex = 0;
            // 
            // panel7
            // 
            this.panel7.BackColor = System.Drawing.SystemColors.Window;
            this.panel7.Controls.Add(this.grdExGroupList1);
            this.panel7.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel7.Location = new System.Drawing.Point(0, 0);
            this.panel7.Name = "panel7";
            this.panel7.Size = new System.Drawing.Size(373, 193);
            this.panel7.TabIndex = 25;
            // 
            // grdExGroupList1
            // 
            this.grdExGroupList1.AlternatingColors = true;
            this.grdExGroupList1.BorderStyle = Janus.Windows.GridEX.BorderStyle.None;
            this.grdExGroupList1.DataSource = this.dvGroup1;
            grdExGroupList1_DesignTimeLayout.LayoutString = resources.GetString("grdExGroupList1_DesignTimeLayout.LayoutString");
            this.grdExGroupList1.DesignTimeLayout = grdExGroupList1_DesignTimeLayout;
            this.grdExGroupList1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdExGroupList1.EmptyRows = true;
            this.grdExGroupList1.FocusCellFormatStyle.BackColor = System.Drawing.SystemColors.Highlight;
            this.grdExGroupList1.FocusStyle = Janus.Windows.GridEX.FocusStyle.None;
            this.grdExGroupList1.Font = new System.Drawing.Font("나눔고딕", 8.249999F);
            this.grdExGroupList1.FrozenColumns = 2;
            this.grdExGroupList1.GridLineColor = System.Drawing.Color.Silver;
            this.grdExGroupList1.GridLines = Janus.Windows.GridEX.GridLines.Vertical;
            this.grdExGroupList1.GridLineStyle = Janus.Windows.GridEX.GridLineStyle.Solid;
            this.grdExGroupList1.GroupByBoxVisible = false;
            this.grdExGroupList1.Location = new System.Drawing.Point(0, 0);
            this.grdExGroupList1.Name = "grdExGroupList1";
            this.grdExGroupList1.ScrollBars = Janus.Windows.GridEX.ScrollBars.Vertical;
            this.grdExGroupList1.SelectedInactiveFormatStyle.BackColor = System.Drawing.Color.DarkOrange;
            this.grdExGroupList1.Size = new System.Drawing.Size(373, 193);
            this.grdExGroupList1.TabIndex = 17;
            this.grdExGroupList1.ThemedAreas = ((Janus.Windows.GridEX.ThemedArea)(((((((((Janus.Windows.GridEX.ThemedArea.ScrollBars | Janus.Windows.GridEX.ThemedArea.EditControls) 
            | Janus.Windows.GridEX.ThemedArea.Headers) 
            | Janus.Windows.GridEX.ThemedArea.GroupByBox) 
            | Janus.Windows.GridEX.ThemedArea.GroupRows) 
            | Janus.Windows.GridEX.ThemedArea.ControlBorder) 
            | Janus.Windows.GridEX.ThemedArea.Cards) 
            | Janus.Windows.GridEX.ThemedArea.Gridlines) 
            | Janus.Windows.GridEX.ThemedArea.CheckBoxes)));
            this.grdExGroupList1.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            this.grdExGroupList1.CellValueChanged += new Janus.Windows.GridEX.ColumnActionEventHandler(this.grdExGroupList1_CellValueChanged);
            this.grdExGroupList1.ColumnHeaderClick += new Janus.Windows.GridEX.ColumnActionEventHandler(this.grdExGroupList1_ColumnHeaderClick);
            // 
            // dvGroup1
            // 
            this.dvGroup1.Table = this.ratio_InsertDs.Group1;
            // 
            // uiPanelGroup1
            // 
            this.uiPanelGroup1.CaptionVisible = Janus.Windows.UI.InheritableBoolean.False;
            this.uiPanelGroup1.InnerContainer = this.uiPanelGroup1Container;
            this.uiPanelGroup1.Location = new System.Drawing.Point(0, 199);
            this.uiPanelGroup1.Name = "uiPanelGroup1";
            this.uiPanelGroup1.Size = new System.Drawing.Size(375, 167);
            this.uiPanelGroup1.TabIndex = 4;
            this.uiPanelGroup1.Text = "그룹2";
            // 
            // uiPanelGroup1Container
            // 
            this.uiPanelGroup1Container.Controls.Add(this.panel5);
            this.uiPanelGroup1Container.Location = new System.Drawing.Point(1, 1);
            this.uiPanelGroup1Container.Name = "uiPanelGroup1Container";
            this.uiPanelGroup1Container.Size = new System.Drawing.Size(373, 165);
            this.uiPanelGroup1Container.TabIndex = 0;
            // 
            // panel5
            // 
            this.panel5.BackColor = System.Drawing.SystemColors.Window;
            this.panel5.Controls.Add(this.grdExGroupList2);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel5.Location = new System.Drawing.Point(0, 0);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(373, 165);
            this.panel5.TabIndex = 25;
            // 
            // grdExGroupList2
            // 
            this.grdExGroupList2.AlternatingColors = true;
            this.grdExGroupList2.BorderStyle = Janus.Windows.GridEX.BorderStyle.None;
            this.grdExGroupList2.DataSource = this.dvGroup2;
            grdExGroupList2_DesignTimeLayout.LayoutString = resources.GetString("grdExGroupList2_DesignTimeLayout.LayoutString");
            this.grdExGroupList2.DesignTimeLayout = grdExGroupList2_DesignTimeLayout;
            this.grdExGroupList2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdExGroupList2.EditorsControlStyle.ButtonAppearance = Janus.Windows.GridEX.ButtonAppearance.Regular;
            this.grdExGroupList2.EmptyRows = true;
            this.grdExGroupList2.FocusCellFormatStyle.BackColor = System.Drawing.SystemColors.Highlight;
            this.grdExGroupList2.FocusStyle = Janus.Windows.GridEX.FocusStyle.None;
            this.grdExGroupList2.FrozenColumns = 2;
            this.grdExGroupList2.GridLineColor = System.Drawing.Color.Silver;
            this.grdExGroupList2.GridLines = Janus.Windows.GridEX.GridLines.Vertical;
            this.grdExGroupList2.GridLineStyle = Janus.Windows.GridEX.GridLineStyle.Solid;
            this.grdExGroupList2.GroupByBoxVisible = false;
            this.grdExGroupList2.Location = new System.Drawing.Point(0, 0);
            this.grdExGroupList2.Name = "grdExGroupList2";
            this.grdExGroupList2.ScrollBars = Janus.Windows.GridEX.ScrollBars.Vertical;
            this.grdExGroupList2.SelectedInactiveFormatStyle.BackColor = System.Drawing.Color.DarkOrange;
            this.grdExGroupList2.Size = new System.Drawing.Size(373, 165);
            this.grdExGroupList2.TabIndex = 17;
            this.grdExGroupList2.ThemedAreas = ((Janus.Windows.GridEX.ThemedArea)(((((((((Janus.Windows.GridEX.ThemedArea.ScrollBars | Janus.Windows.GridEX.ThemedArea.EditControls) 
            | Janus.Windows.GridEX.ThemedArea.Headers) 
            | Janus.Windows.GridEX.ThemedArea.GroupByBox) 
            | Janus.Windows.GridEX.ThemedArea.GroupRows) 
            | Janus.Windows.GridEX.ThemedArea.ControlBorder) 
            | Janus.Windows.GridEX.ThemedArea.Cards) 
            | Janus.Windows.GridEX.ThemedArea.Gridlines) 
            | Janus.Windows.GridEX.ThemedArea.CheckBoxes)));
            this.grdExGroupList2.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            this.grdExGroupList2.CellValueChanged += new Janus.Windows.GridEX.ColumnActionEventHandler(this.grdExGroupList2_CellValueChanged);
            this.grdExGroupList2.ColumnHeaderClick += new Janus.Windows.GridEX.ColumnActionEventHandler(this.grdExGroupList2_ColumnHeaderClick);
            // 
            // dvGroup2
            // 
            this.dvGroup2.Table = this.ratio_InsertDs.Group2;
            // 
            // uiPanelGroupB
            // 
            this.uiPanelGroupB.CaptionVisible = Janus.Windows.UI.InheritableBoolean.False;
            this.uiPanelGroupB.InnerContainer = this.uiPanelGroupBContainer;
            this.uiPanelGroupB.Location = new System.Drawing.Point(0, 370);
            this.uiPanelGroupB.Name = "uiPanelGroupB";
            this.uiPanelGroupB.Size = new System.Drawing.Size(375, 180);
            this.uiPanelGroupB.TabIndex = 4;
            this.uiPanelGroupB.Text = "그룹3";
            // 
            // uiPanelGroupBContainer
            // 
            this.uiPanelGroupBContainer.Controls.Add(this.panel4);
            this.uiPanelGroupBContainer.Location = new System.Drawing.Point(1, 1);
            this.uiPanelGroupBContainer.Name = "uiPanelGroupBContainer";
            this.uiPanelGroupBContainer.Size = new System.Drawing.Size(373, 178);
            this.uiPanelGroupBContainer.TabIndex = 0;
            // 
            // panel4
            // 
            this.panel4.BackColor = System.Drawing.SystemColors.Window;
            this.panel4.Controls.Add(this.grdExGroupList3);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel4.Location = new System.Drawing.Point(0, 0);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(373, 178);
            this.panel4.TabIndex = 25;
            // 
            // grdExGroupList3
            // 
            this.grdExGroupList3.AlternatingColors = true;
            this.grdExGroupList3.BorderStyle = Janus.Windows.GridEX.BorderStyle.None;
            this.grdExGroupList3.DataSource = this.dvGroup3;
            grdExGroupList3_DesignTimeLayout.LayoutString = resources.GetString("grdExGroupList3_DesignTimeLayout.LayoutString");
            this.grdExGroupList3.DesignTimeLayout = grdExGroupList3_DesignTimeLayout;
            this.grdExGroupList3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdExGroupList3.EditorsControlStyle.ButtonAppearance = Janus.Windows.GridEX.ButtonAppearance.Regular;
            this.grdExGroupList3.EmptyRows = true;
            this.grdExGroupList3.FocusCellFormatStyle.BackColor = System.Drawing.SystemColors.Highlight;
            this.grdExGroupList3.FocusStyle = Janus.Windows.GridEX.FocusStyle.None;
            this.grdExGroupList3.FrozenColumns = 2;
            this.grdExGroupList3.GridLineColor = System.Drawing.Color.Silver;
            this.grdExGroupList3.GridLines = Janus.Windows.GridEX.GridLines.Vertical;
            this.grdExGroupList3.GridLineStyle = Janus.Windows.GridEX.GridLineStyle.Solid;
            this.grdExGroupList3.GroupByBoxVisible = false;
            this.grdExGroupList3.Location = new System.Drawing.Point(0, 0);
            this.grdExGroupList3.Name = "grdExGroupList3";
            this.grdExGroupList3.ScrollBars = Janus.Windows.GridEX.ScrollBars.Vertical;
            this.grdExGroupList3.SelectedInactiveFormatStyle.BackColor = System.Drawing.Color.DarkOrange;
            this.grdExGroupList3.Size = new System.Drawing.Size(373, 178);
            this.grdExGroupList3.TabIndex = 17;
            this.grdExGroupList3.ThemedAreas = ((Janus.Windows.GridEX.ThemedArea)(((((((((Janus.Windows.GridEX.ThemedArea.ScrollBars | Janus.Windows.GridEX.ThemedArea.EditControls) 
            | Janus.Windows.GridEX.ThemedArea.Headers) 
            | Janus.Windows.GridEX.ThemedArea.GroupByBox) 
            | Janus.Windows.GridEX.ThemedArea.GroupRows) 
            | Janus.Windows.GridEX.ThemedArea.ControlBorder) 
            | Janus.Windows.GridEX.ThemedArea.Cards) 
            | Janus.Windows.GridEX.ThemedArea.Gridlines) 
            | Janus.Windows.GridEX.ThemedArea.CheckBoxes)));
            this.grdExGroupList3.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            this.grdExGroupList3.CellValueChanged += new Janus.Windows.GridEX.ColumnActionEventHandler(this.grdExGroupList3_CellValueChanged);
            this.grdExGroupList3.ColumnHeaderClick += new Janus.Windows.GridEX.ColumnActionEventHandler(this.grdExGroupList3_ColumnHeaderClick);
            // 
            // dvGroup3
            // 
            this.dvGroup3.Table = this.ratio_InsertDs.Group3;
            // 
            // uiPanelGroupC
            // 
            this.uiPanelGroupC.CaptionVisible = Janus.Windows.UI.InheritableBoolean.False;
            this.uiPanelGroupC.InnerContainer = this.uiPanelGroupCContainer;
            this.uiPanelGroupC.Location = new System.Drawing.Point(0, 554);
            this.uiPanelGroupC.Name = "uiPanelGroupC";
            this.uiPanelGroupC.Size = new System.Drawing.Size(375, 179);
            this.uiPanelGroupC.TabIndex = 4;
            this.uiPanelGroupC.Text = "그룹4";
            // 
            // uiPanelGroupCContainer
            // 
            this.uiPanelGroupCContainer.Controls.Add(this.panel3);
            this.uiPanelGroupCContainer.Location = new System.Drawing.Point(1, 1);
            this.uiPanelGroupCContainer.Name = "uiPanelGroupCContainer";
            this.uiPanelGroupCContainer.Size = new System.Drawing.Size(373, 177);
            this.uiPanelGroupCContainer.TabIndex = 0;
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.SystemColors.Window;
            this.panel3.Controls.Add(this.grdExGroupList4);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(0, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(373, 177);
            this.panel3.TabIndex = 25;
            // 
            // grdExGroupList4
            // 
            this.grdExGroupList4.AlternatingColors = true;
            this.grdExGroupList4.BorderStyle = Janus.Windows.GridEX.BorderStyle.None;
            this.grdExGroupList4.DataSource = this.dvGroup4;
            grdExGroupList4_DesignTimeLayout.LayoutString = resources.GetString("grdExGroupList4_DesignTimeLayout.LayoutString");
            this.grdExGroupList4.DesignTimeLayout = grdExGroupList4_DesignTimeLayout;
            this.grdExGroupList4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdExGroupList4.EditorsControlStyle.ButtonAppearance = Janus.Windows.GridEX.ButtonAppearance.Regular;
            this.grdExGroupList4.EmptyRows = true;
            this.grdExGroupList4.FocusCellFormatStyle.BackColor = System.Drawing.SystemColors.Highlight;
            this.grdExGroupList4.FocusStyle = Janus.Windows.GridEX.FocusStyle.None;
            this.grdExGroupList4.FrozenColumns = 2;
            this.grdExGroupList4.GridLineColor = System.Drawing.Color.Silver;
            this.grdExGroupList4.GridLines = Janus.Windows.GridEX.GridLines.Vertical;
            this.grdExGroupList4.GridLineStyle = Janus.Windows.GridEX.GridLineStyle.Solid;
            this.grdExGroupList4.GroupByBoxVisible = false;
            this.grdExGroupList4.Location = new System.Drawing.Point(0, 0);
            this.grdExGroupList4.Name = "grdExGroupList4";
            this.grdExGroupList4.ScrollBars = Janus.Windows.GridEX.ScrollBars.Vertical;
            this.grdExGroupList4.SelectedInactiveFormatStyle.BackColor = System.Drawing.Color.DarkOrange;
            this.grdExGroupList4.Size = new System.Drawing.Size(373, 177);
            this.grdExGroupList4.TabIndex = 17;
            this.grdExGroupList4.ThemedAreas = ((Janus.Windows.GridEX.ThemedArea)(((((((((Janus.Windows.GridEX.ThemedArea.ScrollBars | Janus.Windows.GridEX.ThemedArea.EditControls) 
            | Janus.Windows.GridEX.ThemedArea.Headers) 
            | Janus.Windows.GridEX.ThemedArea.GroupByBox) 
            | Janus.Windows.GridEX.ThemedArea.GroupRows) 
            | Janus.Windows.GridEX.ThemedArea.ControlBorder) 
            | Janus.Windows.GridEX.ThemedArea.Cards) 
            | Janus.Windows.GridEX.ThemedArea.Gridlines) 
            | Janus.Windows.GridEX.ThemedArea.CheckBoxes)));
            this.grdExGroupList4.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            this.grdExGroupList4.CellValueChanged += new Janus.Windows.GridEX.ColumnActionEventHandler(this.grdExGroupList4_CellValueChanged);
            this.grdExGroupList4.ColumnHeaderClick += new Janus.Windows.GridEX.ColumnActionEventHandler(this.grdExGroupList4_ColumnHeaderClick);
            // 
            // dvGroup4
            // 
            this.dvGroup4.Table = this.ratio_InsertDs.Group4;
            // 
            // dvGroup5
            // 
            this.dvGroup5.Table = this.ratio_InsertDs.Group5;
            // 
            // Ratio_InsertForm
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(6, 14);
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(912, 782);
            this.Controls.Add(this.uiPanel0);
            this.Controls.Add(this.pnlBtn);
            this.Font = global::AdManagerClient.Properties.Settings.Default.Font1;
            this.Name = "Ratio_InsertForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "카테고리/장르 비율조정";
            this.Load += new System.EventHandler(this.Ratio_InsertForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dvGenre)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ratio_InsertDs)).EndInit();
            this.pnlBtn.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.uiPM)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanel0)).EndInit();
            this.uiPanel0.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.uiPanel1)).EndInit();
            this.uiPanel1.ResumeLayout(false);
            this.uiPanel1Container.ResumeLayout(false);
            this.panel6.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdExGenreList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanel2)).EndInit();
            this.uiPanel2.ResumeLayout(false);
            this.uiPanel2Container.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.uiGroupBox1)).EndInit();
            this.uiGroupBox1.ResumeLayout(false);
            this.uiGroupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uiGroupBox2)).EndInit();
            this.uiGroupBox2.ResumeLayout(false);
            this.uiGroupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uiGroupBox3)).EndInit();
            this.uiGroupBox3.ResumeLayout(false);
            this.uiGroupBox3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uiGroupBox4)).EndInit();
            this.uiGroupBox4.ResumeLayout(false);
            this.uiGroupBox4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelGroup2)).EndInit();
            this.uiPanelGroup2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.uiPanel3)).EndInit();
            this.uiPanel3.ResumeLayout(false);
            this.uiPanel3Container.ResumeLayout(false);
            this.panel7.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdExGroupList1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dvGroup1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelGroup1)).EndInit();
            this.uiPanelGroup1.ResumeLayout(false);
            this.uiPanelGroup1Container.ResumeLayout(false);
            this.panel5.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdExGroupList2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dvGroup2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelGroupB)).EndInit();
            this.uiPanelGroupB.ResumeLayout(false);
            this.uiPanelGroupBContainer.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdExGroupList3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dvGroup3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelGroupC)).EndInit();
            this.uiPanelGroupC.ResumeLayout(false);
            this.uiPanelGroupCContainer.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdExGroupList4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dvGroup4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dvGroup5)).EndInit();
            this.ResumeLayout(false);

        }
		#endregion

		#region 컨트롤 로드
		private void Ratio_InsertForm_Load(object sender, System.EventArgs e)
		{            
			// 데이터관리용 객체생성
			dt = ((DataView)grdExGenreList.DataSource).Table;  			
			cm = (CurrencyManager) this.BindingContext[grdExGenreList.DataSource]; 
			cm.PositionChanged += new System.EventHandler(OnGrdRowChanged); 

			dtMenu    = ((DataView)grdExGroupList1.DataSource).Table;  
			cmMenu = (CurrencyManager) this.BindingContext[grdExGroupList1.DataSource]; 

			dtMenu2    = ((DataView)grdExGroupList2.DataSource).Table;  
			cmMenu2 = (CurrencyManager) this.BindingContext[grdExGroupList2.DataSource]; 

			dtMenu3    = ((DataView)grdExGroupList3.DataSource).Table;  
			cmMenu3 = (CurrencyManager) this.BindingContext[grdExGroupList3.DataSource]; 
			
			dtMenu4    = ((DataView)grdExGroupList4.DataSource).Table;  
			cmMenu4 = (CurrencyManager) this.BindingContext[grdExGroupList4.DataSource]; 

            //dtMenu5    = ((DataView)grdExGroupList5.DataSource).Table;  
            //cmMenu5 = (CurrencyManager) this.BindingContext[grdExGroupList5.DataSource]; 
			
			// 컨트롤 초기화
			InitControl();
			setLoadRate();
		}
		#endregion

		#region 컨트롤 초기화
		private void InitControl()
		{
			ProgressStart();
		
			LoadDetailMenu();	
			LoadDetailGroup();

			// 추가버튼 활성화
			if(menu.CanCreate(MenuCode))    canCreate = true;

			// 삭제버튼 활성화
			if(menu.CanDelete(MenuCode))    canDelete = true;

			// 조회권한 검사
			if(menu.CanRead(MenuCode))      canRead = true;
			
			InitButton();
			ProgressStop();

			//if(canRead) LoadDetailMenu();
		}
		#endregion

		#region 사용자 액션처리 메소드

		private void grdExGenreList_ColumnHeaderClick(object sender, Janus.Windows.GridEX.ColumnActionEventArgs e)
		{
			//컬럼Index 0(체크박스컬럼이)이 아니면 빠져나가게 처리.
			if(e.Column.Index != 0)
			{
				return;
			}
            
			//ColumnHeader Click시에 dt Setting 
			DataRow[] foundRows = dt.Select("CheckYn = 'False'");
         
			// if(grdExItemList.CurrentColumn.Position == 0){
			if( foundRows.Length == 0 )
			{
				for(int i=0;i < dt.Rows.Count;i++)
				{
					dt.Rows[i].BeginEdit();
					dt.Rows[i]["CheckYn"]="False";
					dt.Rows[i].EndEdit();
				}
			}
			else
			{
				for(int i=0;i < dt.Rows.Count;i++)
				{
					dt.Rows[i].BeginEdit();
					dt.Rows[i]["CheckYn"]="True";
					dt.Rows[i].EndEdit();
				}
			}
		}

		private void grdExGroupList1_ColumnHeaderClick(object sender, Janus.Windows.GridEX.ColumnActionEventArgs e)
		{
			//컬럼Index 0(체크박스컬럼이)이 아니면 빠져나가게 처리.
			if(e.Column.Index != 0)
			{
				return;
			}
            
			//ColumnHeader Click시에 dt Setting 
			DataRow[] foundRows = dtMenu.Select("CheckYn = 'False'");
         
			// if(grdExItemList.CurrentColumn.Position == 0){
			if( foundRows.Length == 0 )
			{
				for(int i=0;i < dtMenu.Rows.Count;i++)
				{
					dtMenu.Rows[i].BeginEdit();
					dtMenu.Rows[i]["CheckYn"]="False";
					dtMenu.Rows[i].EndEdit();
				}
			}
			else
			{
				for(int i=0;i < dtMenu.Rows.Count;i++)
				{
					dtMenu.Rows[i].BeginEdit();
					dtMenu.Rows[i]["CheckYn"]="True";
					dtMenu.Rows[i].EndEdit();
				}
			}
		}

		private void grdExGroupList2_ColumnHeaderClick(object sender, Janus.Windows.GridEX.ColumnActionEventArgs e)
		{
			//컬럼Index 0(체크박스컬럼이)이 아니면 빠져나가게 처리.
			if(e.Column.Index != 0)
			{
				return;
			}
            
			//ColumnHeader Click시에 dt Setting 
			DataRow[] foundRows = dtMenu2.Select("CheckYn = 'False'");
         
			// if(grdExItemList.CurrentColumn.Position == 0){
			if( foundRows.Length == 0 )
			{
				for(int i=0;i < dtMenu2.Rows.Count;i++)
				{
					dtMenu2.Rows[i].BeginEdit();
					dtMenu2.Rows[i]["CheckYn"]="False";
					dtMenu2.Rows[i].EndEdit();
				}
			}
			else
			{
				for(int i=0;i < dtMenu2.Rows.Count;i++)
				{
					dtMenu2.Rows[i].BeginEdit();
					dtMenu2.Rows[i]["CheckYn"]="True";
					dtMenu2.Rows[i].EndEdit();
				}
			}
		}

		private void grdExGroupList3_ColumnHeaderClick(object sender, Janus.Windows.GridEX.ColumnActionEventArgs e)
		{
			//컬럼Index 0(체크박스컬럼이)이 아니면 빠져나가게 처리.
			if(e.Column.Index != 0)
			{
				return;
			}
            
			//ColumnHeader Click시에 dt Setting 
			DataRow[] foundRows = dtMenu3.Select("CheckYn = 'False'");
         
			// if(grdExItemList.CurrentColumn.Position == 0){
			if( foundRows.Length == 0 )
			{
				for(int i=0;i < dtMenu3.Rows.Count;i++)
				{
					dtMenu3.Rows[i].BeginEdit();
					dtMenu3.Rows[i]["CheckYn"]="False";
					dtMenu3.Rows[i].EndEdit();
				}
			}
			else
			{
				for(int i=0;i < dtMenu3.Rows.Count;i++)
				{
					dtMenu3.Rows[i].BeginEdit();
					dtMenu3.Rows[i]["CheckYn"]="True";
					dtMenu3.Rows[i].EndEdit();
				}
			}
		}

		private void grdExGroupList4_ColumnHeaderClick(object sender, Janus.Windows.GridEX.ColumnActionEventArgs e)
		{
			//컬럼Index 0(체크박스컬럼이)이 아니면 빠져나가게 처리.
			if(e.Column.Index != 0)
			{
				return;
			}
            
			//ColumnHeader Click시에 dt Setting 
			DataRow[] foundRows = dtMenu4.Select("CheckYn = 'False'");
         
			// if(grdExItemList.CurrentColumn.Position == 0){
			if( foundRows.Length == 0 )
			{
				for(int i=0;i < dtMenu4.Rows.Count;i++)
				{
					dtMenu4.Rows[i].BeginEdit();
					dtMenu4.Rows[i]["CheckYn"]="False";
					dtMenu4.Rows[i].EndEdit();
				}
			}
			else
			{
				for(int i=0;i < dtMenu4.Rows.Count;i++)
				{
					dtMenu4.Rows[i].BeginEdit();
					dtMenu4.Rows[i]["CheckYn"]="True";
					dtMenu4.Rows[i].EndEdit();
				}
			}
		}

		private void grdExGroupList5_ColumnHeaderClick(object sender, Janus.Windows.GridEX.ColumnActionEventArgs e)
		{
			//컬럼Index 0(체크박스컬럼이)이 아니면 빠져나가게 처리.
			if(e.Column.Index != 0)
			{
				return;
			}
            
			//ColumnHeader Click시에 dt Setting 
			DataRow[] foundRows = dtMenu5.Select("CheckYn = 'False'");
         
			// if(grdExItemList.CurrentColumn.Position == 0){
			if( foundRows.Length == 0 )
			{
				for(int i=0;i < dtMenu5.Rows.Count;i++)
				{
					dtMenu5.Rows[i].BeginEdit();
					dtMenu5.Rows[i]["CheckYn"]="False";
					dtMenu5.Rows[i].EndEdit();
				}
			}
			else
			{
				for(int i=0;i < dtMenu5.Rows.Count;i++)
				{
					dtMenu5.Rows[i].BeginEdit();
					dtMenu5.Rows[i]["CheckYn"]="True";
					dtMenu5.Rows[i].EndEdit();
				}
			}
		}

		private void InitButton()
		{
			// 추가버튼 활성화
			if(canCreate) btnAddMenu.Enabled    = true;			    			
			if(canCreate) btnAddMenu2.Enabled    = true;			    			
			if(canCreate) btnAddMenu3.Enabled    = true;			    			
			if(canCreate) btnAddMenu4.Enabled    = true;			    			
			

			grdExGenreList.Focus();

			Application.DoEvents();
		}

		private void btnAddMenu_Click(object sender, System.EventArgs e)
		{
			SaveGroup(1);
		}

		private void btnAddMenu2_Click(object sender, System.EventArgs e)
		{
			SaveGroup(2);
		}

		private void btnAddMenu3_Click(object sender, System.EventArgs e)
		{
			SaveGroup(3);
		}

		private void btnAddMenu4_Click(object sender, System.EventArgs e)
		{
			SaveGroup(4);
		}

		private void btnAddMenu5_Click(object sender, System.EventArgs e)
		{
			SaveGroup(5);
		}

		private void btnDeleteMenu_Click(object sender, System.EventArgs e)
		{
			DeleteGroup(1);
		}

		private void btnDeleteMenu2_Click(object sender, System.EventArgs e)
		{
			DeleteGroup(2);
		}

		private void btnDeleteMenu3_Click(object sender, System.EventArgs e)
		{
			DeleteGroup(3);
		}

		private void btnDeleteMenu4_Click(object sender, System.EventArgs e)
		{
			DeleteGroup(4);
		}

		private void btnDeleteMenu5_Click(object sender, System.EventArgs e)
		{
			DeleteGroup(5);
		}

		//저장
		private void btnSave_Click(object sender, System.EventArgs e)
		{
			SaveRate(1);
		}

		private void btnSave2_Click(object sender, System.EventArgs e)
		{
			SaveRate(2);
		}

		private void btnSave3_Click(object sender, System.EventArgs e)
		{
			SaveRate(3);
		}

		private void btnSave4_Click(object sender, System.EventArgs e)
		{
			SaveRate(4);
		}

		private void btnSave5_Click(object sender, System.EventArgs e)
		{
			SaveRate(5);
		}

        private void btnDeleteSync_Click(object sender, System.EventArgs e)
        {
            DeleteSync();
        }

		#endregion
  
		#region 처리메소드

		/// <summary>
		/// 그리드의 Row변경시
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void OnGrdRowChanged(object sender, System.EventArgs e) 
		{
			if(grdExGenreList.RecordCount > 0 )
			{
				//SearchChannelList();
			}
		}

		/// <summary>
		/// 장르목록 조회
		/// </summary>
		/// 

		private void LoadDetailMenu()
		{
			// 지정메뉴 상세편성 정보를 조회한다.
			try
			{
				grdExGenreList.UnCheckAllRecords();

				ratioModel.Init();

				// 데이터모델에 전송할 내용을 셋트한다.
				ratioModel.ItemNo        = keyItemNo;

				// 지정메뉴광고 상세편성내역 조회 서비스를 호출한다.
				new RatioManager(systemModel,commonModel).GetSchChoiceMenuDetailList(ratioModel);

				if (ratioModel.ResultCD.Equals("0000"))
				{
					Utility.SetDataTable(ratio_InsertDs.Genre, ratioModel.MenuDataSet);

					// 삭제버튼 활성화
					if(ratio_InsertDs.Genre.Rows.Count > 0)
					{
						btnAddMenu.Enabled  = true;
						btnAddMenu2.Enabled = true;
						btnAddMenu3.Enabled = true;
						btnAddMenu4.Enabled = true;
									
					}
					else
					{
						btnAddMenu.Enabled  = false;
						btnAddMenu2.Enabled = false;
						btnAddMenu3.Enabled = false;
						btnAddMenu4.Enabled = false;
											
						
					}					
				}		    			
			}
			catch(FrameException fe)
			{
				FrameSystem.showMsgForm("지정광고 상세편성내역 조회오류", new string[] {fe.ErrCode, fe.ResultMsg});
			}
			catch(Exception ex)
			{
				FrameSystem.showMsgForm("지정광고 상세편성내역 조회오류",new string[] {"",ex.Message});
			}			
		}

		private void LoadDetailGroup()
		{
			// 지정메뉴 상세편성 정보를 조회한다.
			try
			{
				grdExGenreList.UnCheckAllRecords();

				ratioModel.Init();

				// 데이터모델에 전송할 내용을 셋트한다.
				ratioModel.ItemNo        = keyItemNo;

				// 지정메뉴광고 상세편성내역 조회 서비스를 호출한다.
				new RatioManager(systemModel,commonModel).GetGroup1List(ratioModel);

				if (ratioModel.ResultCD.Equals("0000"))
				{					
					Utility.SetDataTable(ratio_InsertDs.Group1, ratioModel.Group1DataSet);		
					StatusMessage(ratioModel.ResultCnt + "건의 그룹1의 정보가 조회되었습니다.");					
				}		
			
				int curRow = cmMenu.Position;
				if(curRow >= 0)
				{				
					ebEntryRate1.Text	=	dtMenu.Rows[curRow]["EntryRate"].ToString();	
					btnDeleteMenu.Enabled  = true;		
					btnSave.Enabled = true;					
				}
				else
				{
					ebEntryRate1.Text	= "";
					btnDeleteMenu.Enabled  = false;	
					btnSave.Enabled = false;					
				}

				new RatioManager(systemModel,commonModel).GetGroup2List(ratioModel);

				if (ratioModel.ResultCD.Equals("0000"))
				{					
					Utility.SetDataTable(ratio_InsertDs.Group2, ratioModel.Group2DataSet);		
					StatusMessage(ratioModel.ResultCnt + "건의 그룹2의 정보가 조회되었습니다.");					
				}

				int curRow2 = cmMenu2.Position;
				if(curRow2 >= 0)
				{				
					ebEntryRate2.Text	=	dtMenu2.Rows[curRow2]["EntryRate"].ToString();		
					btnDeleteMenu2.Enabled  = true;			
					btnSave2.Enabled = true;					
				}
				else
				{
					ebEntryRate2.Text	= "";
					btnDeleteMenu2.Enabled  = false;		
					btnSave2.Enabled = false;					
				}

				new RatioManager(systemModel,commonModel).GetGroup3List(ratioModel);

				if (ratioModel.ResultCD.Equals("0000"))
				{					
					Utility.SetDataTable(ratio_InsertDs.Group3, ratioModel.Group3DataSet);		
					StatusMessage(ratioModel.ResultCnt + "건의 그룹3의 정보가 조회되었습니다.");					
				}

				int curRow3 = cmMenu3.Position;
				if(curRow3 >= 0)
				{				
					ebEntryRate3.Text	=	dtMenu3.Rows[curRow3]["EntryRate"].ToString();	
					btnDeleteMenu3.Enabled  = true;		
					btnSave3.Enabled = true;					
				}
				else
				{
					ebEntryRate3.Text	= "";
					btnDeleteMenu3.Enabled  = false;
					btnSave3.Enabled = false;	
				}

				new RatioManager(systemModel,commonModel).GetGroup4List(ratioModel);

				if (ratioModel.ResultCD.Equals("0000"))
				{					
					Utility.SetDataTable(ratio_InsertDs.Group4, ratioModel.Group4DataSet);		
					StatusMessage(ratioModel.ResultCnt + "건의 그룹4의 정보가 조회되었습니다.");					
				}

				int curRow4 = cmMenu4.Position;
				if(curRow4 >= 0)
				{				
					ebEntryRate4.Text	=	dtMenu4.Rows[curRow4]["EntryRate"].ToString();		
					btnDeleteMenu4.Enabled  = true;	
					btnSave4.Enabled = true;					
				}
				else
				{
					ebEntryRate4.Text	= "";
					btnDeleteMenu4.Enabled  = false;
					btnSave4.Enabled = false;		
				}

				new RatioManager(systemModel,commonModel).GetGroup5List(ratioModel);

				if (ratioModel.ResultCD.Equals("0000"))
				{					
					Utility.SetDataTable(ratio_InsertDs.Group5, ratioModel.Group5DataSet);		
					StatusMessage(ratioModel.ResultCnt + "건의 그룹5의 정보가 조회되었습니다.");					
				}

                //int curRow5 = cmMenu5.Position;
                //if(curRow5 >= 0)
                //{				
                //    ebEntryRate5.Text	=	dtMenu5.Rows[curRow5]["EntryRate"].ToString();	
                //    btnDeleteMenu5.Enabled  = true;	
                //    btnSave5.Enabled = true;
                //}
                //else
                //{
                //    ebEntryRate5.Text	= "";
                //    btnDeleteMenu5.Enabled  = false;
                //    btnSave5.Enabled = false;
                //}
				
				// 추가버튼 활성화
				//				if(canCreate) btnAddMenu.Enabled    = true;			    			
			}
			catch(FrameException fe)
			{
				FrameSystem.showMsgForm("지정광고 그룹1의 조회오류", new string[] {fe.ErrCode, fe.ResultMsg});
			}
			catch(Exception ex)
			{
				FrameSystem.showMsgForm("지정광고 그룹1의 조회오류",new string[] {"",ex.Message});
			}			
		}

		/// <summary>
		///  그룹비고저장
		/// </summary>
		private void SaveGroup(int Order)
		{
			StatusMessage("그룹 정보를 저장합니다.");
			grdExGenreList.UpdateData();
			
			try
			{
				ratioModel.Init();
				ratioModel.MediaCode   = keyMediaCode;
				ratioModel.ItemNo      = keyItemNo;

				switch(Order)
				{
					case 1:
						if(ebEntryRate1.Text.Trim().Length == 0) 
						{
							MessageBox.Show("비율1이 입력되지 않았습니다.","그룹 저장",MessageBoxButtons.OK, MessageBoxIcon.Information );
							ebEntryRate1.Focus();
							return;				
						}					

						for(int i=0;i < ratio_InsertDs.Genre.Rows.Count;i++)
						{
							DataRow Genre_row = ratio_InsertDs.Genre.Rows[i];
							if(Genre_row["CheckYn"].ToString().Equals("True"))
							{								
								ratioModel.EntrySeq		   = "1";
								keyCategoryCode			   = Genre_row["CategoryCode"].ToString();
								keyGenreCode			   = Genre_row["GenreCode"].ToString();
								ratioModel.CategoryCode    = keyCategoryCode;
								ratioModel.GenreCode	   = keyGenreCode;
								//그룹의 총 비율이 100%가 넘으면 안된다.
								KeyEntryRate1	= Convert.ToInt32(ebEntryRate1.Text);
								KeyEntryRate2	= Convert.ToInt32(ebEntryRate2.Text);
								KeyEntryRate3	= Convert.ToInt32(ebEntryRate3.Text);
								KeyEntryRate4	= Convert.ToInt32(ebEntryRate4.Text);
                                //KeyEntryRate5	= Convert.ToInt32(ebEntryRate5.Text);

								if((KeyEntryRate1 + KeyEntryRate2 + KeyEntryRate3 + KeyEntryRate4)<=100)
								{
									ratioModel.EntryRate	   = ebEntryRate1.Text;	
								}
								else
								{
									KeyTotalRate = 100 - (KeyEntryRate2 + KeyEntryRate3 + KeyEntryRate4);

									MessageBox.Show("그룹의 총비율이 100%를 초과하였습니다.\n 비율1에는 최고" + KeyTotalRate+"%"+"까지 입력할 수 있습니다.","그룹 저장", 
										MessageBoxButtons.OK, MessageBoxIcon.Information );
									ebEntryRate1.Focus();
									return;		
								}							

								new RatioManager(systemModel,commonModel).GetSchRateList(ratioModel);

								if (ratioModel.ResultCD.Equals("0000"))
								{					
									Utility.SetDataTable(ratio_InsertDs.SchRate, ratioModel.SchRateDataSet);		
									StatusMessage(ratioModel.ResultCnt + "건의 비율 정보가 조회되었습니다.");					
								}		
	
								new RatioManager(systemModel,commonModel).GetGroup1List(ratioModel);

								if (ratioModel.ResultCD.Equals("0000"))
								{					
									Utility.SetDataTable(ratio_InsertDs.Group1, ratioModel.Group1DataSet);		
									StatusMessage(ratioModel.ResultCnt + "건의 그룹1의 정보가 조회되었습니다.");					
								}		
																			
								DataRow[] RateRows = ratio_InsertDs.SchRate.Select("ItemNo ='"+keyItemNo+"' AND EntrySeq = '"+1+"'");
								DataRow[] RateDetailRows = ratio_InsertDs.Group1.Select("ItemNo ='"+keyItemNo+"' AND EntrySeq = '"+1+"' AND CategoryCode = '"+keyCategoryCode+"' AND GenreCode = '"+keyGenreCode+"'");

								if(RateRows.Length!=0)
								{											
									if(RateDetailRows.Length==0)
									{
										new RatioManager(systemModel,commonModel).SetSchRateDetailCreate(ratioModel);																	
									}
								}
								else
								{
									new RatioManager(systemModel,commonModel).SetSchRateCreate(ratioModel);
									new RatioManager(systemModel,commonModel).SetSchRateDetailCreate(ratioModel);									
								}
							}
						}										
						break;
					case 2:
						if(ebEntryRate2.Text.Trim().Length == 0) 
						{
							MessageBox.Show("비율2이 입력되지 않았습니다.","그룹 저장", 
								MessageBoxButtons.OK, MessageBoxIcon.Information );
							ebEntryRate2.Focus();
							return;				
						}		
						for(int i=0;i < ratio_InsertDs.Genre.Rows.Count;i++)
						{
							DataRow Genre_row = ratio_InsertDs.Genre.Rows[i];
							if(Genre_row["CheckYn"].ToString().Equals("True"))
							{								
								ratioModel.EntrySeq		   = "2";
								keyCategoryCode			   = Genre_row["CategoryCode"].ToString();
								keyGenreCode			   = Genre_row["GenreCode"].ToString();
								ratioModel.CategoryCode    = keyCategoryCode;
								ratioModel.GenreCode	   = keyGenreCode;							
								//그룹의 총 비율이 100%가 넘으면 안된다.
								KeyEntryRate1	= Convert.ToInt32(ebEntryRate1.Text);
								KeyEntryRate2	= Convert.ToInt32(ebEntryRate2.Text);
								KeyEntryRate3	= Convert.ToInt32(ebEntryRate3.Text);
								KeyEntryRate4	= Convert.ToInt32(ebEntryRate4.Text);
                                //KeyEntryRate5	= Convert.ToInt32(ebEntryRate5.Text);

								if((KeyEntryRate1 + KeyEntryRate2 + KeyEntryRate3 + KeyEntryRate4)<=100)
								{
									ratioModel.EntryRate	   = ebEntryRate2.Text;	
								}
								else
								{
									KeyTotalRate = 100 - (KeyEntryRate1 + KeyEntryRate3 + KeyEntryRate4);

									MessageBox.Show("그룹의 총비율이 100%를 초과하였습니다.\n 비율2에는 최고" + KeyTotalRate+"%"+"까지 입력할 수 있습니다.","그룹 저장", 
										MessageBoxButtons.OK, MessageBoxIcon.Information );
									ebEntryRate2.Focus();
									return;		
								}		

								new RatioManager(systemModel,commonModel).GetSchRateList(ratioModel);

								if (ratioModel.ResultCD.Equals("0000"))
								{					
									Utility.SetDataTable(ratio_InsertDs.SchRate, ratioModel.SchRateDataSet);		
									StatusMessage(ratioModel.ResultCnt + "건의 비율 정보가 조회되었습니다.");					
								}		
	
								new RatioManager(systemModel,commonModel).GetGroup2List(ratioModel);

								if (ratioModel.ResultCD.Equals("0000"))
								{					
									Utility.SetDataTable(ratio_InsertDs.Group2, ratioModel.Group2DataSet);		
									StatusMessage(ratioModel.ResultCnt + "건의 그룹1의 정보가 조회되었습니다.");					
								}		
																			
								DataRow[] RateRows = ratio_InsertDs.SchRate.Select("ItemNo ='"+keyItemNo+"' AND EntrySeq = '"+2+"'");
								DataRow[] RateDetailRows = ratio_InsertDs.Group2.Select("ItemNo ='"+keyItemNo+"' AND EntrySeq = '"+2+"' AND CategoryCode = '"+keyCategoryCode+"' AND GenreCode = '"+keyGenreCode+"'");

								if(RateRows.Length!=0)
								{											
									if(RateDetailRows.Length==0)
									{
										new RatioManager(systemModel,commonModel).SetSchRateDetailCreate(ratioModel);																	
									}
								}
								else
								{								
									ratioModel.EntryRate	   = ebEntryRate2.Text;									

									new RatioManager(systemModel,commonModel).SetSchRateCreate(ratioModel);

									new RatioManager(systemModel,commonModel).SetSchRateDetailCreate(ratioModel);									
								}
							}
						}	
						break;
					case 3:
						if(ebEntryRate3.Text.Trim().Length == 0) 
						{
							MessageBox.Show("비율3이 입력되지 않았습니다.","그룹 저장", 
								MessageBoxButtons.OK, MessageBoxIcon.Information );
							ebEntryRate3.Focus();
							return;				
						}
						for(int i=0;i < ratio_InsertDs.Genre.Rows.Count;i++)
						{
							DataRow Genre_row = ratio_InsertDs.Genre.Rows[i];
							if(Genre_row["CheckYn"].ToString().Equals("True"))
							{								
								ratioModel.EntrySeq		   = "3";
								keyCategoryCode			   = Genre_row["CategoryCode"].ToString();
								keyGenreCode			   = Genre_row["GenreCode"].ToString();
								ratioModel.CategoryCode    = keyCategoryCode;
								ratioModel.GenreCode	   = keyGenreCode;
							
								//그룹의 총 비율이 100%가 넘으면 안된다.
								KeyEntryRate1	= Convert.ToInt32(ebEntryRate1.Text);
								KeyEntryRate2	= Convert.ToInt32(ebEntryRate2.Text);
								KeyEntryRate3	= Convert.ToInt32(ebEntryRate3.Text);
								KeyEntryRate4	= Convert.ToInt32(ebEntryRate4.Text);
                                //KeyEntryRate5	= Convert.ToInt32(ebEntryRate5.Text);

								if((KeyEntryRate1 + KeyEntryRate2 + KeyEntryRate3 + KeyEntryRate4)<=100)
								{
									ratioModel.EntryRate	   = ebEntryRate3.Text;	
								}
								else
								{
									KeyTotalRate = 100 - (KeyEntryRate1 + KeyEntryRate2 + KeyEntryRate4);

									MessageBox.Show("그룹의 총비율이 100%를 초과하였습니다.\n 비율3에는 최고" + KeyTotalRate+"%"+"까지 입력할 수 있습니다.","그룹 저장", 
										MessageBoxButtons.OK, MessageBoxIcon.Information );
									ebEntryRate3.Focus();
									return;		
								}		
								new RatioManager(systemModel,commonModel).GetSchRateList(ratioModel);

								if (ratioModel.ResultCD.Equals("0000"))
								{					
									Utility.SetDataTable(ratio_InsertDs.SchRate, ratioModel.SchRateDataSet);		
									StatusMessage(ratioModel.ResultCnt + "건의 비율 정보가 조회되었습니다.");					
								}		
	
								new RatioManager(systemModel,commonModel).GetGroup3List(ratioModel);

								if (ratioModel.ResultCD.Equals("0000"))
								{					
									Utility.SetDataTable(ratio_InsertDs.Group3, ratioModel.Group3DataSet);		
									StatusMessage(ratioModel.ResultCnt + "건의 그룹1의 정보가 조회되었습니다.");					
								}		
																			
								DataRow[] RateRows = ratio_InsertDs.SchRate.Select("ItemNo ='"+keyItemNo+"' AND EntrySeq = '"+3+"'");
								DataRow[] RateDetailRows = ratio_InsertDs.Group3.Select("ItemNo ='"+keyItemNo+"' AND EntrySeq = '"+3+"' AND CategoryCode = '"+keyCategoryCode+"' AND GenreCode = '"+keyGenreCode+"'");

								if(RateRows.Length!=0)
								{											
									if(RateDetailRows.Length==0)
									{
										new RatioManager(systemModel,commonModel).SetSchRateDetailCreate(ratioModel);																	
									}
								}
								else
								{								
									ratioModel.EntryRate	   = ebEntryRate3.Text;									

									new RatioManager(systemModel,commonModel).SetSchRateCreate(ratioModel);

									new RatioManager(systemModel,commonModel).SetSchRateDetailCreate(ratioModel);									
								}
							}
						}	
						break;
					case 4:
						if(ebEntryRate4.Text.Trim().Length == 0) 
						{
							MessageBox.Show("비율4이 입력되지 않았습니다.","그룹 저장", 
								MessageBoxButtons.OK, MessageBoxIcon.Information );
							ebEntryRate4.Focus();
							return;				
						}
						for(int i=0;i < ratio_InsertDs.Genre.Rows.Count;i++)
						{
							DataRow Genre_row = ratio_InsertDs.Genre.Rows[i];
							if(Genre_row["CheckYn"].ToString().Equals("True"))
							{								
								ratioModel.EntrySeq		   = "4";
								keyCategoryCode			   = Genre_row["CategoryCode"].ToString();
								keyGenreCode			   = Genre_row["GenreCode"].ToString();
								ratioModel.CategoryCode    = keyCategoryCode;
								ratioModel.GenreCode	   = keyGenreCode;
								//그룹의 총 비율이 100%가 넘으면 안된다.
								KeyEntryRate1	= Convert.ToInt32(ebEntryRate1.Text);
								KeyEntryRate2	= Convert.ToInt32(ebEntryRate2.Text);
								KeyEntryRate3	= Convert.ToInt32(ebEntryRate3.Text);
								KeyEntryRate4	= Convert.ToInt32(ebEntryRate4.Text);
                                //KeyEntryRate5	= Convert.ToInt32(ebEntryRate5.Text);

								if((KeyEntryRate1 + KeyEntryRate2 + KeyEntryRate3 + KeyEntryRate4 )<=100)
								{
									ratioModel.EntryRate	   = ebEntryRate4.Text;	
								}
								else
								{
									KeyTotalRate = 100 - (KeyEntryRate1 + KeyEntryRate2 + KeyEntryRate3);

									MessageBox.Show("그룹의 총비율이 100%를 초과하였습니다.\n 비율4에는 최고" + KeyTotalRate+"%"+"까지 입력할 수 있습니다.","그룹 저장", 
										MessageBoxButtons.OK, MessageBoxIcon.Information );
									ebEntryRate4.Focus();
									return;		
								}		

								new RatioManager(systemModel,commonModel).GetSchRateList(ratioModel);

								if (ratioModel.ResultCD.Equals("0000"))
								{					
									Utility.SetDataTable(ratio_InsertDs.SchRate, ratioModel.SchRateDataSet);		
									StatusMessage(ratioModel.ResultCnt + "건의 비율 정보가 조회되었습니다.");					
								}		
	
								new RatioManager(systemModel,commonModel).GetGroup4List(ratioModel);

								if (ratioModel.ResultCD.Equals("0000"))
								{					
									Utility.SetDataTable(ratio_InsertDs.Group4, ratioModel.Group4DataSet);		
									StatusMessage(ratioModel.ResultCnt + "건의 그룹1의 정보가 조회되었습니다.");					
								}		
																			
								DataRow[] RateRows = ratio_InsertDs.SchRate.Select("ItemNo ='"+keyItemNo+"' AND EntrySeq = '"+4+"'");
								DataRow[] RateDetailRows = ratio_InsertDs.Group4.Select("ItemNo ='"+keyItemNo+"' AND EntrySeq = '"+4+"' AND CategoryCode = '"+keyCategoryCode+"' AND GenreCode = '"+keyGenreCode+"'");

								if(RateRows.Length!=0)
								{											
									if(RateDetailRows.Length==0)
									{
										new RatioManager(systemModel,commonModel).SetSchRateDetailCreate(ratioModel);																	
									}
								}
								else
								{								
									ratioModel.EntryRate	   = ebEntryRate4.Text;									

									new RatioManager(systemModel,commonModel).SetSchRateCreate(ratioModel);

									new RatioManager(systemModel,commonModel).SetSchRateDetailCreate(ratioModel);									
								}
							}
						}	
						break;
					case 5:
                        //if(ebEntryRate5.Text.Trim().Length == 0) 
                        //{
                        //    MessageBox.Show("비율5이 입력되지 않았습니다.","그룹 저장", 
                        //        MessageBoxButtons.OK, MessageBoxIcon.Information );
                        //    ebEntryRate5.Focus();
                        //    return;				
                        //}
                        //for(int i=0;i < ratio_InsertDs.Genre.Rows.Count;i++)
                        //{
                        //    DataRow Genre_row = ratio_InsertDs.Genre.Rows[i];
                        //    if(Genre_row["CheckYn"].ToString().Equals("True"))
                        //    {								
                        //        ratioModel.EntrySeq		   = "5";
                        //        keyCategoryCode			   = Genre_row["CategoryCode"].ToString();
                        //        keyGenreCode			   = Genre_row["GenreCode"].ToString();
                        //        ratioModel.CategoryCode    = keyCategoryCode;
                        //        ratioModel.GenreCode	   = keyGenreCode;
                        //        //그룹의 총 비율이 100%가 넘으면 안된다.
                        //        KeyEntryRate1	= Convert.ToInt32(ebEntryRate1.Text);
                        //        KeyEntryRate2	= Convert.ToInt32(ebEntryRate2.Text);
                        //        KeyEntryRate3	= Convert.ToInt32(ebEntryRate3.Text);
                        //        KeyEntryRate4	= Convert.ToInt32(ebEntryRate4.Text);
                        //        KeyEntryRate5	= Convert.ToInt32(ebEntryRate5.Text);

                        //        if((KeyEntryRate1 + KeyEntryRate2 + KeyEntryRate3 + KeyEntryRate4 + KeyEntryRate5)<=100)
                        //        {
                        //            ratioModel.EntryRate	   = ebEntryRate5.Text;	
                        //        }
                        //        else
                        //        {
                        //            KeyTotalRate = 100 - (KeyEntryRate1 + KeyEntryRate2 + KeyEntryRate3 + KeyEntryRate4);

                        //            MessageBox.Show("그룹의 총비율이 100%를 초과하였습니다.\n 비율5에는 최고" + KeyTotalRate+"%"+"까지 입력할 수 있습니다.","그룹 저장", 
                        //                MessageBoxButtons.OK, MessageBoxIcon.Information );
                        //            ebEntryRate5.Focus();
                        //            return;		
                        //        }		

                        //        new RatioManager(systemModel,commonModel).GetSchRateList(ratioModel);

                        //        if (ratioModel.ResultCD.Equals("0000"))
                        //        {					
                        //            Utility.SetDataTable(ratio_InsertDs.SchRate, ratioModel.SchRateDataSet);		
                        //            StatusMessage(ratioModel.ResultCnt + "건의 비율 정보가 조회되었습니다.");					
                        //        }		
	
                        //        new RatioManager(systemModel,commonModel).GetGroup5List(ratioModel);

                        //        if (ratioModel.ResultCD.Equals("0000"))
                        //        {					
                        //            Utility.SetDataTable(ratio_InsertDs.Group5, ratioModel.Group5DataSet);		
                        //            StatusMessage(ratioModel.ResultCnt + "건의 그룹1의 정보가 조회되었습니다.");					
                        //        }		
																			
                        //        DataRow[] RateRows = ratio_InsertDs.SchRate.Select("ItemNo ='"+keyItemNo+"' AND EntrySeq = '"+5+"'");
                        //        DataRow[] RateDetailRows = ratio_InsertDs.Group5.Select("ItemNo ='"+keyItemNo+"' AND EntrySeq = '"+5+"' AND CategoryCode = '"+keyCategoryCode+"' AND GenreCode = '"+keyGenreCode+"'");

                        //        if(RateRows.Length!=0)
                        //        {											
                        //            if(RateDetailRows.Length==0)
                        //            {
                        //                new RatioManager(systemModel,commonModel).SetSchRateDetailCreate(ratioModel);																	
                        //            }
                        //        }
                        //        else
                        //        {								
                        //            ratioModel.EntryRate	   = ebEntryRate5.Text;									

                        //            new RatioManager(systemModel,commonModel).SetSchRateCreate(ratioModel);

                        //            new RatioManager(systemModel,commonModel).SetSchRateDetailCreate(ratioModel);									
                        //        }
                        //    }
                        //}	
						break;
				}

				// 체크된 모든 항목을 클리어
				ClearListCheck();
				DisableButton();
				LoadDetailMenu();
				LoadDetailGroup();
				InitButton();

			}
			catch(FrameException fe)
			{
				FrameSystem.showMsgForm("그룹 비고 저장오류", new string[] {fe.ErrCode, fe.ResultMsg});
			}
			catch(Exception ex)
			{
				FrameSystem.showMsgForm("그룹 비고 저장오류",new string[] {"",ex.Message});
			}			
		}

		/// <summary>
		///  비율저장
		/// </summary>
		private void SaveRate(int Order)
		{
			StatusMessage("그룹 정보를 저장합니다.");
			
			try
			{
				ratioModel.Init();
				ratioModel.MediaCode   = keyMediaCode;
				ratioModel.ItemNo      = keyItemNo;

				Janus.Windows.GridEX.EditControls.NumericEditBox biyulTextBox = null;
				if(Order==1)
				{
					biyulTextBox = ebEntryRate1;
				}
				else if(Order==2)
				{
					biyulTextBox = ebEntryRate2;
				}
				else if(Order==3)
				{
					biyulTextBox = ebEntryRate3;
				}
				else if(Order==4)
				{
					biyulTextBox = ebEntryRate4;
				}
				

				if(biyulTextBox.Text.Trim().Length == 0) 
				{
					MessageBox.Show("비율"+ Order +"이 입력되지 않았습니다.","그룹 저장", MessageBoxButtons.OK, MessageBoxIcon.Information );
					biyulTextBox.Focus();
					return;				
				}					

				ratioModel.EntrySeq		   = ""+ Order;
				
				//그룹의 총 비율이 100%가 넘으면 안된다.
				KeyEntryRate1	= Convert.ToInt32(ebEntryRate1.Text);
				KeyEntryRate2	= Convert.ToInt32(ebEntryRate2.Text);
				KeyEntryRate3	= Convert.ToInt32(ebEntryRate3.Text);
				KeyEntryRate4	= Convert.ToInt32(ebEntryRate4.Text);
				
				if((KeyEntryRate1 + KeyEntryRate2 + KeyEntryRate3 + KeyEntryRate4 )<=100)
				{
					ratioModel.EntryRate	   = biyulTextBox.Text;	
					new RatioManager(systemModel,commonModel).SetSchRateUpdate(ratioModel);																	
				}
				else
				{
					KeyTotalRate = 100 - (KeyEntryRate2 + KeyEntryRate3 + KeyEntryRate4);

					MessageBox.Show("그룹의 총비율이 100%를 초과하였습니다.\n 비율1에는 최고" + KeyTotalRate+"%"+"까지 입력할 수 있습니다.","그룹 저장", 
						MessageBoxButtons.OK, MessageBoxIcon.Information );
					ebEntryRate1.Focus();
					return;		
				}																

				LoadRate[Order-1] = (Decimal)biyulTextBox.Value;
				// 체크된 모든 항목을 클리어
				DisableButton();
				LoadDetailMenu();
				LoadDetailGroup();
				InitButton();

			}
			catch(FrameException fe)
			{
				FrameSystem.showMsgForm("그룹 비고 저장오류", new string[] {fe.ErrCode, fe.ResultMsg});
			}
			catch(Exception ex)
			{
				FrameSystem.showMsgForm("그룹 비고 저장오류",new string[] {"",ex.Message});
			}			
		}


        /// <summary>
        /// 편성에 없는것은 일괄적으로 삭제한다
        /// </summary>
        private void DeleteSync()
        {
            DialogResult result = MessageBox.Show("편성에 없는 장르들을 정리합니다!","편성그룹",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question,
                MessageBoxDefaultButton.Button2);

            if (result == DialogResult.No) return;
			
            try
            {
                if( Convert.ToInt32(keyItemNo) > 0 )
                {
                    ratioModel.Init();
                    ratioModel.MediaCode   = keyMediaCode;
                    ratioModel.ItemNo      = keyItemNo;

                    new RatioManager(systemModel,commonModel).mDeleteSync(ratioModel);	
				
                    if (ratioModel.ResultCD.Equals("0000"))
                    {					
                        LoadDetailMenu();
                        LoadDetailGroup();								
                    }
                }
                else
                {
                    FrameSystem.showMsgForm("편성그룹정리 저장오류", new string[] {"0001", "선택한 광고번호가 없습니다."});
                }
            }
            catch(FrameException fe)
            {
                FrameSystem.showMsgForm("편성그룹정리 저장오류", new string[] {fe.ErrCode, fe.ResultMsg});
            }
            catch(Exception ex)
            {
                FrameSystem.showMsgForm("편성그룹정리 저장오류",new string[] {"",ex.Message});
            }			
        }


		/// <summary>
		/// 그룹 삭제
		/// </summary>
		private void DeleteGroup(int Order)
		{
			StatusMessage("그룹을 삭제합니다.");

			DialogResult result = MessageBox.Show("그룹내역을 삭제 하시겠습니까?","그룹내역 삭제",
				MessageBoxButtons.YesNo, MessageBoxIcon.Question,
				MessageBoxDefaultButton.Button2);

			if (result == DialogResult.No) return;

			DisableButton();

			// 그리드에 변경된 데이터를 Datasource에 업데이트 한다.
			grdExGroupList1.UpdateData();
			grdExGroupList2.UpdateData();
			grdExGroupList3.UpdateData();
			grdExGroupList4.UpdateData();
            //grdExGroupList5.UpdateData();
            ProgressStart();

			try
			{
				ratioModel.Init();		
				keyCategoryCode = "";
				keyGenreCode = "";
				ratioModel.MediaCode   = keyMediaCode;
				ratioModel.ItemNo      = keyItemNo;
				int SetCount = 0;

				switch(Order)
				{
					case 1:
                        #region 1. 선택한 장르를 삭제한다
						// 삭제 시킴
						for(int i = 0;i < ratio_InsertDs.Group1.Rows.Count;i++)
						{
							DataRow row1 = ratio_InsertDs.Group1.Rows[i];

							Debug.WriteLine( i.ToString() + ":" + row1["CategoryCode"].ToString() + "|" + row1["GenreCode"].ToString() );

							if(row1["CheckYn"].ToString().Equals("True"))
							{							
								// 데이터모델에 전송할 내용을 셋트한다.			
								ratioModel.EntrySeq		   = "1";
								keyCategoryCode			   = row1["CategoryCode"].ToString();
								keyGenreCode			   = row1["GenreCode"].ToString();
								ratioModel.CategoryCode    = keyCategoryCode;
								ratioModel.GenreCode	   = keyGenreCode;					
								
								// 지정광고 편성내역 삭제 서비스를 호출한다.
								new RatioManager(systemModel,commonModel).SetSchRateDetailDelete(ratioModel);
																				
								if (ratioModel.ResultCD.Equals("0000"))
								{
									SetCount++;
								}	
							}
                            Application.DoEvents();
						}
                        #endregion
                        #region 2. 해당그룹의 설정된 장르데이터를 읽어온다
						new RatioManager(systemModel,commonModel).GetSchRateDetailList(ratioModel);

						if (ratioModel.ResultCD.Equals("0000"))
						{					
							Utility.SetDataTable(ratio_InsertDs.SchRateDetail, ratioModel.SchRateDetailDataSet);		
							StatusMessage(ratioModel.ResultCnt + "건의 그룹1의 정보가 조회되었습니다.");					
						}		
                        #endregion
                        #region 3. 하위설정된 장르가 없으면 그룹을 삭제한다
						DataRow[] RateDetailRows = ratio_InsertDs.SchRateDetail.Select("ItemNo ='"+keyItemNo+"' AND EntrySeq = '"+1+"'");
						if(RateDetailRows.Length==0)
						{
							new RatioManager(systemModel,commonModel).SetSchRateDelete(ratioModel);																	
						}
                        #endregion
						break;
					case 2:
						// 삭제 시킴
						for(int i = 0;i < ratio_InsertDs.Group2.Rows.Count;i++)
						{
							DataRow row2 = ratio_InsertDs.Group2.Rows[i];

							Debug.WriteLine( i.ToString() + ":" + row2["CategoryCode"].ToString() + "|" + row2["GenreCode"].ToString() );

							if(row2["CheckYn"].ToString().Equals("True"))
							{							
								// 데이터모델에 전송할 내용을 셋트한다.			
								ratioModel.EntrySeq		   = "2";
								keyCategoryCode			   = row2["CategoryCode"].ToString();
								keyGenreCode			   = row2["GenreCode"].ToString();
								ratioModel.CategoryCode    = keyCategoryCode;
								ratioModel.GenreCode	   = keyGenreCode;					
								
								// 지정광고 편성내역 삭제 서비스를 호출한다.
								new RatioManager(systemModel,commonModel).SetSchRateDetailDelete(ratioModel);
								
								if (ratioModel.ResultCD.Equals("0000"))
								{
									SetCount++;
								}	
							}
                            Application.DoEvents();
						}

                        //ratioModel.MediaCode   = keyMediaCode;
                        //ratioModel.ItemNo      = keyItemNo;
						new RatioManager(systemModel,commonModel).GetSchRateDetailList(ratioModel);

						if (ratioModel.ResultCD.Equals("0000"))
						{					
							Utility.SetDataTable(ratio_InsertDs.SchRateDetail, ratioModel.SchRateDetailDataSet);		
							StatusMessage(ratioModel.ResultCnt + "건의 그룹1의 정보가 조회되었습니다.");					
						}		

						DataRow[] RateDetailRows2 = ratio_InsertDs.SchRateDetail.Select("ItemNo ='"+keyItemNo+"' AND EntrySeq = '"+2+"'");
						if(RateDetailRows2.Length==0)
						{
							new RatioManager(systemModel,commonModel).SetSchRateDelete(ratioModel);																	
						}			
						break;
					case 3:
						// 삭제 시킴
						for(int i = 0;i < ratio_InsertDs.Group3.Rows.Count;i++)
						{
							DataRow row3 = ratio_InsertDs.Group3.Rows[i];

							Debug.WriteLine( i.ToString() + ":" + row3["CategoryCode"].ToString() + "|" + row3["GenreCode"].ToString() );

							if(row3["CheckYn"].ToString().Equals("True"))
							{							
								// 데이터모델에 전송할 내용을 셋트한다.			
								ratioModel.EntrySeq		   = "3";
								keyCategoryCode			   = row3["CategoryCode"].ToString();
								keyGenreCode			   = row3["GenreCode"].ToString();
								ratioModel.CategoryCode    = keyCategoryCode;
								ratioModel.GenreCode	   = keyGenreCode;					
								
								// 지정광고 편성내역 삭제 서비스를 호출한다.
								new RatioManager(systemModel,commonModel).SetSchRateDetailDelete(ratioModel);
				
								if (ratioModel.ResultCD.Equals("0000"))
								{
									SetCount++;
								}	
							}
                            Application.DoEvents();
						}
						new RatioManager(systemModel,commonModel).GetSchRateDetailList(ratioModel);

						if (ratioModel.ResultCD.Equals("0000"))
						{					
							Utility.SetDataTable(ratio_InsertDs.SchRateDetail, ratioModel.SchRateDetailDataSet);		
							StatusMessage(ratioModel.ResultCnt + "건의 그룹1의 정보가 조회되었습니다.");					
						}		

						DataRow[] RateDetailRows3 = ratio_InsertDs.SchRateDetail.Select("ItemNo ='"+keyItemNo+"' AND EntrySeq = '"+3+"'");
						if(RateDetailRows3.Length==0)
						{
							new RatioManager(systemModel,commonModel).SetSchRateDelete(ratioModel);																	
						}			
						break;
					case 4:
						// 삭제 시킴
						for(int i = 0;i < ratio_InsertDs.Group4.Rows.Count;i++)
						{
							DataRow row4 = ratio_InsertDs.Group4.Rows[i];

							Debug.WriteLine( i.ToString() + ":" + row4["CategoryCode"].ToString() + "|" + row4["GenreCode"].ToString() );

							if(row4["CheckYn"].ToString().Equals("True"))
							{							
								// 데이터모델에 전송할 내용을 셋트한다.			
								ratioModel.EntrySeq		   = "4";
								keyCategoryCode			   = row4["CategoryCode"].ToString();
								keyGenreCode			   = row4["GenreCode"].ToString();
								ratioModel.CategoryCode    = keyCategoryCode;
								ratioModel.GenreCode	   = keyGenreCode;					
								
								// 지정광고 편성내역 삭제 서비스를 호출한다.
								new RatioManager(systemModel,commonModel).SetSchRateDetailDelete(ratioModel);
								
								if (ratioModel.ResultCD.Equals("0000"))
								{
									SetCount++;
								}	
							}
                            Application.DoEvents();
						}
						new RatioManager(systemModel,commonModel).GetSchRateDetailList(ratioModel);

						if (ratioModel.ResultCD.Equals("0000"))
						{					
							Utility.SetDataTable(ratio_InsertDs.SchRateDetail, ratioModel.SchRateDetailDataSet);		
							StatusMessage(ratioModel.ResultCnt + "건의 그룹1의 정보가 조회되었습니다.");					
						}		

						DataRow[] RateDetailRows4 = ratio_InsertDs.SchRateDetail.Select("ItemNo ='"+keyItemNo+"' AND EntrySeq = '"+4+"'");
						if(RateDetailRows4.Length==0)
						{
							new RatioManager(systemModel,commonModel).SetSchRateDelete(ratioModel);																	
						}			
						break;
					case 5:
						// 삭제 시킴
						for(int i = 0;i < ratio_InsertDs.Group5.Rows.Count;i++)
						{
							DataRow row5 = ratio_InsertDs.Group5.Rows[i];

							Debug.WriteLine( i.ToString() + ":" + row5["CategoryCode"].ToString() + "|" + row5["GenreCode"].ToString() );

							if(row5["CheckYn"].ToString().Equals("True"))
							{							
								// 데이터모델에 전송할 내용을 셋트한다.			
								ratioModel.EntrySeq		   = "5";
								keyCategoryCode			   = row5["CategoryCode"].ToString();
								keyGenreCode			   = row5["GenreCode"].ToString();
								ratioModel.CategoryCode    = keyCategoryCode;
								ratioModel.GenreCode	   = keyGenreCode;					
								
								// 지정광고 편성내역 삭제 서비스를 호출한다.
								new RatioManager(systemModel,commonModel).SetSchRateDetailDelete(ratioModel);
								
								if (ratioModel.ResultCD.Equals("0000"))
								{
									SetCount++;
								}	
							}
                            Application.DoEvents();
						}
						new RatioManager(systemModel,commonModel).GetSchRateDetailList(ratioModel);

						if (ratioModel.ResultCD.Equals("0000"))
						{					
							Utility.SetDataTable(ratio_InsertDs.SchRateDetail, ratioModel.SchRateDetailDataSet);		
							StatusMessage(ratioModel.ResultCnt + "건의 그룹1의 정보가 조회되었습니다.");					
						}		

						DataRow[] RateDetailRows5 = ratio_InsertDs.SchRateDetail.Select("ItemNo ='"+keyItemNo+"' AND EntrySeq = '"+5+"'");
						if(RateDetailRows5.Length==0)
						{
							new RatioManager(systemModel,commonModel).SetSchRateDelete(ratioModel);
						}			
						break;
				}				
				if(SetCount > 0)
				{
					keyItemNo = ratioModel.ItemNo;
					LoadDetailMenu();
					LoadDetailGroup();
					StatusMessage("그룹1내역이 삭제되었습니다.");			
				}	
				else
				{
					MessageBox.Show("선택된 그룹내역이 없습니다.", "비율편성",
						MessageBoxButtons.OK, MessageBoxIcon.Information);
						
					LoadDetailMenu();
					LoadDetailGroup();
				}			
			}
			catch(FrameException fe)
			{
				FrameSystem.showMsgForm("그룹내역삭제오류", new string[] {fe.ErrCode, fe.ResultMsg});
			}
			catch(Exception ex)
			{
				FrameSystem.showMsgForm("그룹내역삭제오류",new string[] {"",ex.Message});
			}		
            ProgressStop();
		}

		private void ClearListCheck()
		{

			// 체크된 모든 항목을 클리어
			grdExGenreList.UnCheckAllRecords();
			grdExGenreList.UpdateData();
				   
			// 데이터 클리어
			for(int i=0;i < dt.Rows.Count;i++)
			{
				dt.Rows[i].BeginEdit();
				dt.Rows[i]["CheckYn"]="False";
				dt.Rows[i].EndEdit();
			}
		}

		private void ClearGroup1ListCheck()
		{
			// 체크된 모든 항목을 클리어
			grdExGroupList1.UnCheckAllRecords();
			grdExGroupList1.UpdateData();
				   
			// 데이터 클리어
			for(int i=0;i < dtMenu.Rows.Count;i++)
			{
				dtMenu.Rows[i].BeginEdit();
				dtMenu.Rows[i]["CheckYn"]="False";
				dtMenu.Rows[i].EndEdit();
			}
		}

		private void ClearGroup2ListCheck()
		{
			// 체크된 모든 항목을 클리어
			grdExGroupList2.UnCheckAllRecords();
			grdExGroupList2.UpdateData();
				   
			// 데이터 클리어
			for(int i=0;i < dtMenu2.Rows.Count;i++)
			{
				dtMenu2.Rows[i].BeginEdit();
				dtMenu2.Rows[i]["CheckYn"]="False";
				dtMenu2.Rows[i].EndEdit();
			}
		}

		private void ClearGroup3ListCheck()
		{
			// 체크된 모든 항목을 클리어
			grdExGroupList3.UnCheckAllRecords();
			grdExGroupList3.UpdateData();
				   
			// 데이터 클리어
			for(int i=0;i < dtMenu3.Rows.Count;i++)
			{
				dtMenu3.Rows[i].BeginEdit();
				dtMenu3.Rows[i]["CheckYn"]="False";
				dtMenu3.Rows[i].EndEdit();
			}
		}

		private void ClearGroup4ListCheck()
		{
			// 체크된 모든 항목을 클리어
			grdExGroupList4.UnCheckAllRecords();
			grdExGroupList4.UpdateData();
				   
			// 데이터 클리어
			for(int i=0;i < dtMenu4.Rows.Count;i++)
			{
				dtMenu4.Rows[i].BeginEdit();
				dtMenu4.Rows[i]["CheckYn"]="False";
				dtMenu4.Rows[i].EndEdit();
			}
		}

		private void ClearGroup5ListCheck()
		{
            //// 체크된 모든 항목을 클리어
            //grdExGroupList5.UnCheckAllRecords();
            //grdExGroupList5.UpdateData();
				   
            //// 데이터 클리어
            //for(int i=0;i < dtMenu5.Rows.Count;i++)
            //{
            //    dtMenu5.Rows[i].BeginEdit();
            //    dtMenu5.Rows[i]["CheckYn"]="False";
            //    dtMenu5.Rows[i].EndEdit();
            //}
		}


		private void DisableButton()
		{
			btnAddMenu.Enabled  = false;
			btnAddMenu2.Enabled = false;
			btnAddMenu3.Enabled = false;
			btnAddMenu4.Enabled = false;
            //btnAddMenu5.Enabled = false;	
		
			btnDeleteMenu.Enabled  = false;
			btnDeleteMenu2.Enabled  = false;
			btnDeleteMenu3.Enabled  = false;
			btnDeleteMenu4.Enabled  = false;
            //btnDeleteMenu5.Enabled  = false;

			btnSave.Enabled = false;		
			btnSave2.Enabled = false;		
			btnSave3.Enabled = false;		
			btnSave4.Enabled = false;		
            //btnSave5.Enabled = false;		
			Application.DoEvents();
		}

		
		#endregion
	
		#region 이벤트함수

		private void setLoadRate()
		{
			LoadRate[0] = (Decimal)ebEntryRate1.Value;
			LoadRate[1] = (Decimal)ebEntryRate2.Value;
			LoadRate[2] = (Decimal)ebEntryRate3.Value;
			LoadRate[3] = (Decimal)ebEntryRate4.Value;
            //LoadRate[4] = (Decimal)ebEntryRate5.Value;
		}
		
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

		private void btnClose_Click(object sender, System.EventArgs e)
		{
			this.Close();
		}

		private void btnOk_Click(object sender, System.EventArgs e)
		{
			Decimal[] Rate = new Decimal[4];
			Rate[0] = (Decimal)ebEntryRate1.Value;
			Rate[1] = (Decimal)ebEntryRate2.Value;
			Rate[2] = (Decimal)ebEntryRate3.Value;
			Rate[3] = (Decimal)ebEntryRate4.Value;
            //Rate[4] = (Decimal)ebEntryRate5.Value;

			//저장이 안된 비율이 있는지 확인한다.
			for(int i=0; i<Rate.Length; i++)
			{
				if(LoadRate[i] != Rate[i])
				{
					DialogResult result = MessageBox.Show("저장이 안된 비율이 있습니다?\n정말 종료하시겠습니까?", "변경여부확인", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
					if(result == DialogResult.No)
					{
						return;
					}
					break;
				}
			}
			//지정된 비율의 합이 100%인지 확인한다.
			int RateSum = 0;
			for(int i=0; i<Rate.Length; i++)
			{
				RateSum += Convert.ToInt16(Rate[i]);
			}
			if(RateSum != 100)
			{
				DialogResult result = MessageBox.Show("비율의 합이 100% 가 아닙니다.\n 지정하신 비율이 맞습니까?", "비율확인", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
				if(result == DialogResult.No)
				{
					return;
				}

			}

			Opener.ReloadMenuList();
			this.Close();
		}
		#endregion

        private void grdExGenreList_CellValueChanged(object sender, Janus.Windows.GridEX.ColumnActionEventArgs e)
        {
            if (e.Column.EditType == Janus.Windows.GridEX.EditType.CheckBox)
            {
                int curRow = cm.Position;
                if (curRow >= 0)
                {
                    dt.Rows[curRow].BeginEdit();
                    dt.Rows[curRow]["CheckYn"] = grdExGenreList.GetValue(e.Column).ToString();
                    dt.Rows[curRow].EndEdit();
                }
            }
        }

        private void grdExGroupList1_CellValueChanged(object sender, Janus.Windows.GridEX.ColumnActionEventArgs e)
        {
            if (e.Column.EditType == Janus.Windows.GridEX.EditType.CheckBox)
            {
                int curRow = cmMenu.Position;
                if (curRow >= 0)
                {
                    dtMenu.Rows[curRow].BeginEdit();
                    dtMenu.Rows[curRow]["CheckYn"] = grdExGroupList1.GetValue(e.Column).ToString();
                    dtMenu.Rows[curRow].EndEdit();
                }
            }
        }

        private void grdExGroupList2_CellValueChanged(object sender, Janus.Windows.GridEX.ColumnActionEventArgs e)
        {
            if (e.Column.EditType == Janus.Windows.GridEX.EditType.CheckBox)
            {
                int curRow = cmMenu2.Position;
                if (curRow >= 0)
                {
                    dtMenu2.Rows[curRow].BeginEdit();
                    dtMenu2.Rows[curRow]["CheckYn"] = grdExGroupList2.GetValue(e.Column).ToString();
                    dtMenu2.Rows[curRow].EndEdit();
                }
            }
        }

        private void grdExGroupList3_CellValueChanged(object sender, Janus.Windows.GridEX.ColumnActionEventArgs e)
        {
            if (e.Column.EditType == Janus.Windows.GridEX.EditType.CheckBox)
            {
                int curRow = cmMenu3.Position;
                if (curRow >= 0)
                {
                    dtMenu3.Rows[curRow].BeginEdit();
                    dtMenu3.Rows[curRow]["CheckYn"] = grdExGroupList3.GetValue(e.Column).ToString();
                    dtMenu3.Rows[curRow].EndEdit();
                }
            }
        }

        private void grdExGroupList4_CellValueChanged(object sender, Janus.Windows.GridEX.ColumnActionEventArgs e)
        {
            if (e.Column.EditType == Janus.Windows.GridEX.EditType.CheckBox)
            {
                int curRow = cmMenu4.Position;
                if (curRow >= 0)
                {
                    dtMenu4.Rows[curRow].BeginEdit();
                    dtMenu4.Rows[curRow]["CheckYn"] = grdExGroupList4.GetValue(e.Column).ToString();
                    dtMenu4.Rows[curRow].EndEdit();
                }
            }
        }

        private void grdExGroupList5_CellValueChanged(object sender, Janus.Windows.GridEX.ColumnActionEventArgs e)
        {
            //if (e.Column.EditType == Janus.Windows.GridEX.EditType.CheckBox)
            //{
            //    int curRow = cmMenu5.Position;
            //    if (curRow >= 0)
            //    {
            //        dtMenu5.Rows[curRow].BeginEdit();
            //        dtMenu5.Rows[curRow]["CheckYn"] = grdExGroupList5.GetValue(e.Column).ToString();
            //        dtMenu5.Rows[curRow].EndEdit();
            //    }
            //}
        }
	}
}	
