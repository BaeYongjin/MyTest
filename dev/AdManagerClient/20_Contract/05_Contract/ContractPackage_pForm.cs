// ===============================================================================
// ContractPackageControl for Charites Project
//
// ContractPackageControl.cs
//
// �����ǰ���� ������� �����մϴ�. 
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
	/// ����������� ��Ʈ��
	/// </summary>
	public class ContractPackage_pForm : System.Windows.Forms.Form
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
		public string        MenuCode		= "";

		// ����� ������
		ContractModel contractModel  = new ContractModel();	// ���������

		// ȭ��ó���� ����
		bool IsNewSearchKey		  = true;					// �˻����Է� ����
		CurrencyManager cm        = null;					// ������ �׸����� ���濡 ���� �����ͼ� ������ ���Ͽ�			
		DataTable       dt        = null;
	
		// �� â�� �� ��Ʈ��		
		ContractControl parentCtl = null;

		#endregion

		#region ȭ�� ������Ʈ, ������, �Ҹ���

		private Janus.Windows.UI.Dock.UIPanelManager uiPM;
		private System.Windows.Forms.Panel pnlSearch;
		private Janus.Windows.EditControls.UIButton btnSearch;
		private Janus.Windows.UI.Dock.UIPanel uiPanelSearch;
		private Janus.Windows.UI.Dock.UIPanelInnerContainer uiPanelSearchContainer;
		private Janus.Windows.UI.Dock.UIPanel uiPanelList;
		private Janus.Windows.UI.Dock.UIPanelInnerContainer uiPanelListContainer;
		private Janus.Windows.UI.Dock.UIPanelGroup uiPanelContractPackage;
		private Janus.Windows.GridEX.EditControls.EditBox ebSearchKey;
		private Janus.Windows.EditControls.UIComboBox cbSearchRap;
		private System.Data.DataView dvContractPackage;
		private Janus.Windows.GridEX.GridEX grdExContractPackageList;
		private System.Windows.Forms.Panel panel1;
		private Janus.Windows.GridEX.GridEX gridEX1;
		private AdManagerClient._20_Contract._05_Contract.Contractpackage_pDs contractpackage_pDs;
		private System.Windows.Forms.Panel pnlBtn;
		private Janus.Windows.EditControls.UIButton btnClose;
		private Janus.Windows.EditControls.UIButton btnOk;


		private System.ComponentModel.IContainer components;

		//private ContractControl parentCtl = null;
		public ContractPackage_pForm()
		{
			//
			// Windows Form �����̳� ������ �ʿ��մϴ�.
			//
			InitializeComponent();

			//
			
			//
		}
		/// <summary>
		/// ������ �Ѱܾ� �� ��
		/// </summary>
		/// <param name="sender"></param>
		public ContractPackage_pForm(ContractControl sender)
		{
			//
			// Windows Form �����̳� ������ �ʿ��մϴ�.
			//
			InitializeComponent();

			//
			
			//
			parentCtl = sender;			
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
            Janus.Windows.GridEX.GridEXLayout grdExContractPackageList_DesignTimeLayout = new Janus.Windows.GridEX.GridEXLayout();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ContractPackage_pForm));
            this.uiPM = new Janus.Windows.UI.Dock.UIPanelManager(this.components);
            this.uiPanelContractPackage = new Janus.Windows.UI.Dock.UIPanelGroup();
            this.uiPanelSearch = new Janus.Windows.UI.Dock.UIPanel();
            this.uiPanelSearchContainer = new Janus.Windows.UI.Dock.UIPanelInnerContainer();
            this.pnlSearch = new System.Windows.Forms.Panel();
            this.ebSearchKey = new Janus.Windows.GridEX.EditControls.EditBox();
            this.cbSearchRap = new Janus.Windows.EditControls.UIComboBox();
            this.btnSearch = new Janus.Windows.EditControls.UIButton();
            this.uiPanelList = new Janus.Windows.UI.Dock.UIPanel();
            this.uiPanelListContainer = new Janus.Windows.UI.Dock.UIPanelInnerContainer();
            this.pnlBtn = new System.Windows.Forms.Panel();
            this.btnClose = new Janus.Windows.EditControls.UIButton();
            this.btnOk = new Janus.Windows.EditControls.UIButton();
            this.grdExContractPackageList = new Janus.Windows.GridEX.GridEX();
            this.dvContractPackage = new System.Data.DataView();
            this.contractpackage_pDs = new AdManagerClient._20_Contract._05_Contract.Contractpackage_pDs();
            this.panel1 = new System.Windows.Forms.Panel();
            this.gridEX1 = new Janus.Windows.GridEX.GridEX();
            ((System.ComponentModel.ISupportInitialize)(this.uiPM)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelContractPackage)).BeginInit();
            this.uiPanelContractPackage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelSearch)).BeginInit();
            this.uiPanelSearch.SuspendLayout();
            this.uiPanelSearchContainer.SuspendLayout();
            this.pnlSearch.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelList)).BeginInit();
            this.uiPanelList.SuspendLayout();
            this.uiPanelListContainer.SuspendLayout();
            this.pnlBtn.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdExContractPackageList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dvContractPackage)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.contractpackage_pDs)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridEX1)).BeginInit();
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
            this.uiPanelContractPackage.Id = new System.Guid("b162c53a-7940-47cd-a0ed-9762c59c50d8");
            this.uiPanelContractPackage.StaticGroup = true;
            this.uiPanelSearch.Id = new System.Guid("afb3b7d0-aa79-443c-a1e8-3baba38f7a4d");
            this.uiPanelContractPackage.Panels.Add(this.uiPanelSearch);
            this.uiPanelList.Id = new System.Guid("8e3140b9-bfcb-43af-b84e-2ee6cddf51dc");
            this.uiPanelContractPackage.Panels.Add(this.uiPanelList);
            this.uiPM.Panels.Add(this.uiPanelContractPackage);
            // 
            // Design Time Panel Info:
            // 
            this.uiPM.BeginPanelInfo();
            this.uiPM.AddDockPanelInfo(new System.Guid("b162c53a-7940-47cd-a0ed-9762c59c50d8"), Janus.Windows.UI.Dock.PanelGroupStyle.HorizontalTiles, Janus.Windows.UI.Dock.PanelDockStyle.Fill, true, new System.Drawing.Size(496, 493), true);
            this.uiPM.AddDockPanelInfo(new System.Guid("afb3b7d0-aa79-443c-a1e8-3baba38f7a4d"), new System.Guid("b162c53a-7940-47cd-a0ed-9762c59c50d8"), 41, true);
            this.uiPM.AddDockPanelInfo(new System.Guid("8e3140b9-bfcb-43af-b84e-2ee6cddf51dc"), new System.Guid("b162c53a-7940-47cd-a0ed-9762c59c50d8"), 456, true);
            this.uiPM.AddFloatingPanelInfo(new System.Guid("b162c53a-7940-47cd-a0ed-9762c59c50d8"), Janus.Windows.UI.Dock.PanelGroupStyle.HorizontalTiles, true, new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
            this.uiPM.AddFloatingPanelInfo(new System.Guid("8e3140b9-bfcb-43af-b84e-2ee6cddf51dc"), new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
            this.uiPM.AddFloatingPanelInfo(new System.Guid("b2973a57-d3e1-4d80-9387-7a6dc3332542"), new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
            this.uiPM.AddFloatingPanelInfo(new System.Guid("afb3b7d0-aa79-443c-a1e8-3baba38f7a4d"), new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
            this.uiPM.EndPanelInfo();
            // 
            // uiPanelContractPackage
            // 
            this.uiPanelContractPackage.ActiveCaptionMode = Janus.Windows.UI.Dock.ActiveCaptionMode.Never;
            this.uiPanelContractPackage.AllowResize = Janus.Windows.UI.InheritableBoolean.False;
            this.uiPanelContractPackage.BackColor = System.Drawing.SystemColors.Window;
            this.uiPanelContractPackage.CaptionVisible = Janus.Windows.UI.InheritableBoolean.True;
            this.uiPanelContractPackage.CloseButtonVisible = Janus.Windows.UI.InheritableBoolean.False;
            this.uiPanelContractPackage.Location = new System.Drawing.Point(0, 0);
            this.uiPanelContractPackage.Name = "uiPanelContractPackage";
            this.uiPanelContractPackage.Size = new System.Drawing.Size(496, 493);
            this.uiPanelContractPackage.TabIndex = 4;
            this.uiPanelContractPackage.Text = "�����ǰ����";
            // 
            // uiPanelSearch
            // 
            this.uiPanelSearch.AllowResize = Janus.Windows.UI.InheritableBoolean.False;
            this.uiPanelSearch.CaptionVisible = Janus.Windows.UI.InheritableBoolean.False;
            this.uiPanelSearch.CloseButtonVisible = Janus.Windows.UI.InheritableBoolean.False;
            this.uiPanelSearch.InnerContainer = this.uiPanelSearchContainer;
            this.uiPanelSearch.Location = new System.Drawing.Point(0, 22);
            this.uiPanelSearch.Name = "uiPanelSearch";
            this.uiPanelSearch.Size = new System.Drawing.Size(496, 40);
            this.uiPanelSearch.TabIndex = 4;
            this.uiPanelSearch.Text = "�˻�";
            // 
            // uiPanelSearchContainer
            // 
            this.uiPanelSearchContainer.Controls.Add(this.pnlSearch);
            this.uiPanelSearchContainer.Location = new System.Drawing.Point(1, 1);
            this.uiPanelSearchContainer.Name = "uiPanelSearchContainer";
            this.uiPanelSearchContainer.Size = new System.Drawing.Size(494, 38);
            this.uiPanelSearchContainer.TabIndex = 0;
            // 
            // pnlSearch
            // 
            this.pnlSearch.BackColor = System.Drawing.SystemColors.Window;
            this.pnlSearch.Controls.Add(this.ebSearchKey);
            this.pnlSearch.Controls.Add(this.cbSearchRap);
            this.pnlSearch.Controls.Add(this.btnSearch);
            this.pnlSearch.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlSearch.Location = new System.Drawing.Point(0, 0);
            this.pnlSearch.Name = "pnlSearch";
            this.pnlSearch.Size = new System.Drawing.Size(494, 38);
            this.pnlSearch.TabIndex = 3;
            // 
            // ebSearchKey
            // 
            this.ebSearchKey.Location = new System.Drawing.Point(136, 8);
            this.ebSearchKey.Name = "ebSearchKey";
            this.ebSearchKey.Size = new System.Drawing.Size(200, 21);
            this.ebSearchKey.TabIndex = 2;
            this.ebSearchKey.Text = "�˻�� �Է��ϼ���";
            this.ebSearchKey.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
            this.ebSearchKey.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            this.ebSearchKey.TextChanged += new System.EventHandler(this.ebSearchKey_TextChanged);
            this.ebSearchKey.Click += new System.EventHandler(this.ebSearchKey_Click);
            this.ebSearchKey.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ebSearchKey_KeyDown);
            // 
            // cbSearchRap
            // 
            this.cbSearchRap.ComboStyle = Janus.Windows.EditControls.ComboStyle.DropDownList;
            this.cbSearchRap.Location = new System.Drawing.Point(8, 8);
            this.cbSearchRap.Name = "cbSearchRap";
            this.cbSearchRap.Size = new System.Drawing.Size(120, 21);
            this.cbSearchRap.TabIndex = 1;
            this.cbSearchRap.Text = "�̵�����";
            this.cbSearchRap.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            // 
            // btnSearch
            // 
            this.btnSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSearch.Location = new System.Drawing.Point(381, 8);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(104, 24);
            this.btnSearch.TabIndex = 3;
            this.btnSearch.Text = "�� ȸ";
            this.btnSearch.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // uiPanelList
            // 
            this.uiPanelList.BackColor = System.Drawing.SystemColors.Window;
            this.uiPanelList.CaptionStyle = Janus.Windows.UI.Dock.PanelCaptionStyle.Dark;
            this.uiPanelList.InnerContainer = this.uiPanelListContainer;
            this.uiPanelList.Location = new System.Drawing.Point(0, 66);
            this.uiPanelList.MinimumSize = new System.Drawing.Size(-1, 100);
            this.uiPanelList.Name = "uiPanelList";
            this.uiPanelList.Size = new System.Drawing.Size(496, 427);
            this.uiPanelList.TabIndex = 8;
            this.uiPanelList.TabStop = false;
            this.uiPanelList.Text = "�����ǰ���";
            // 
            // uiPanelListContainer
            // 
            this.uiPanelListContainer.BackColor = System.Drawing.SystemColors.Window;
            this.uiPanelListContainer.Controls.Add(this.pnlBtn);
            this.uiPanelListContainer.Controls.Add(this.grdExContractPackageList);
            this.uiPanelListContainer.Location = new System.Drawing.Point(1, 23);
            this.uiPanelListContainer.Name = "uiPanelListContainer";
            this.uiPanelListContainer.Size = new System.Drawing.Size(494, 403);
            this.uiPanelListContainer.TabIndex = 0;
            // 
            // pnlBtn
            // 
            this.pnlBtn.BackColor = System.Drawing.Color.WhiteSmoke;
            this.pnlBtn.Controls.Add(this.btnClose);
            this.pnlBtn.Controls.Add(this.btnOk);
            this.pnlBtn.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlBtn.Location = new System.Drawing.Point(0, 363);
            this.pnlBtn.Name = "pnlBtn";
            this.pnlBtn.Size = new System.Drawing.Size(494, 40);
            this.pnlBtn.TabIndex = 17;
            // 
            // btnClose
            // 
            this.btnClose.BackColor = System.Drawing.SystemColors.Control;
            this.btnClose.ImageIndex = 1;
            this.btnClose.Location = new System.Drawing.Point(260, 8);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(64, 23);
            this.btnClose.TabIndex = 6;
            this.btnClose.Text = "�ݱ�";
            this.btnClose.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnOk
            // 
            this.btnOk.BackColor = System.Drawing.SystemColors.Control;
            this.btnOk.ImageIndex = 0;
            this.btnOk.Location = new System.Drawing.Point(180, 8);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(64, 23);
            this.btnOk.TabIndex = 5;
            this.btnOk.Text = "Ȯ��";
            this.btnOk.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // grdExContractPackageList
            // 
            this.grdExContractPackageList.AllowEdit = Janus.Windows.GridEX.InheritableBoolean.False;
            this.grdExContractPackageList.AlternatingColors = true;
            this.grdExContractPackageList.BorderStyle = Janus.Windows.GridEX.BorderStyle.None;
            this.grdExContractPackageList.DataSource = this.dvContractPackage;
            grdExContractPackageList_DesignTimeLayout.LayoutString = resources.GetString("grdExContractPackageList_DesignTimeLayout.LayoutString");
            this.grdExContractPackageList.DesignTimeLayout = grdExContractPackageList_DesignTimeLayout;
            this.grdExContractPackageList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdExContractPackageList.EditorsControlStyle.ButtonAppearance = Janus.Windows.GridEX.ButtonAppearance.Regular;
            this.grdExContractPackageList.EmptyRows = true;
            this.grdExContractPackageList.FocusCellFormatStyle.BackColor = System.Drawing.SystemColors.Highlight;
            this.grdExContractPackageList.FocusCellFormatStyle.ForeColor = System.Drawing.Color.White;
            this.grdExContractPackageList.FocusStyle = Janus.Windows.GridEX.FocusStyle.None;
            this.grdExContractPackageList.Font = new System.Drawing.Font("����ü", 9F);
            this.grdExContractPackageList.GridLineColor = System.Drawing.Color.Silver;
            this.grdExContractPackageList.GridLines = Janus.Windows.GridEX.GridLines.Vertical;
            this.grdExContractPackageList.GridLineStyle = Janus.Windows.GridEX.GridLineStyle.Solid;
            this.grdExContractPackageList.GroupByBoxVisible = false;
            this.grdExContractPackageList.HeaderFormatStyle.TextAlignment = Janus.Windows.GridEX.TextAlignment.Center;
            this.grdExContractPackageList.HideSelection = Janus.Windows.GridEX.HideSelection.HighlightInactive;
            this.grdExContractPackageList.Location = new System.Drawing.Point(0, 0);
            this.grdExContractPackageList.Name = "grdExContractPackageList";
            this.grdExContractPackageList.SelectedInactiveFormatStyle.BackColor = System.Drawing.Color.Gold;
            this.grdExContractPackageList.Size = new System.Drawing.Size(494, 403);
            this.grdExContractPackageList.TabIndex = 4;
            this.grdExContractPackageList.TabKeyBehavior = Janus.Windows.GridEX.TabKeyBehavior.ControlNavigation;
            this.grdExContractPackageList.ThemedAreas = ((Janus.Windows.GridEX.ThemedArea)(((((((((Janus.Windows.GridEX.ThemedArea.ScrollBars | Janus.Windows.GridEX.ThemedArea.EditControls)
                        | Janus.Windows.GridEX.ThemedArea.Headers)
                        | Janus.Windows.GridEX.ThemedArea.GroupByBox)
                        | Janus.Windows.GridEX.ThemedArea.GroupRows)
                        | Janus.Windows.GridEX.ThemedArea.ControlBorder)
                        | Janus.Windows.GridEX.ThemedArea.Cards)
                        | Janus.Windows.GridEX.ThemedArea.Gridlines)
                        | Janus.Windows.GridEX.ThemedArea.CheckBoxes)));
            this.grdExContractPackageList.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            this.grdExContractPackageList.DoubleClick += new System.EventHandler(this.grdExContractPackageList_DoubleClick);
            this.grdExContractPackageList.Enter += new System.EventHandler(this.OnGrdRowChanged);
            // 
            // dvContractPackage
            // 
            this.dvContractPackage.Table = this.contractpackage_pDs.Package;
            // 
            // contractpackage_pDs
            // 
            this.contractpackage_pDs.DataSetName = "Contractpackage_pDs";
            this.contractpackage_pDs.Locale = new System.Globalization.CultureInfo("en-US");
            this.contractpackage_pDs.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // panel1
            // 
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(200, 100);
            this.panel1.TabIndex = 0;
            // 
            // gridEX1
            // 
            this.gridEX1.Location = new System.Drawing.Point(0, 0);
            this.gridEX1.Name = "gridEX1";
            this.gridEX1.Size = new System.Drawing.Size(400, 376);
            this.gridEX1.TabIndex = 0;
            // 
            // ContractPackage_pForm
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(6, 14);
            this.ClientSize = new System.Drawing.Size(496, 493);
            this.Controls.Add(this.uiPanelContractPackage);
            this.Name = "ContractPackage_pForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "��ǰ�˻�";
            this.Load += new System.EventHandler(this.UserControl_Load);
            ((System.ComponentModel.ISupportInitialize)(this.uiPM)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelContractPackage)).EndInit();
            this.uiPanelContractPackage.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelSearch)).EndInit();
            this.uiPanelSearch.ResumeLayout(false);
            this.uiPanelSearchContainer.ResumeLayout(false);
            this.pnlSearch.ResumeLayout(false);
            this.pnlSearch.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelList)).EndInit();
            this.uiPanelList.ResumeLayout(false);
            this.uiPanelListContainer.ResumeLayout(false);
            this.pnlBtn.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdExContractPackageList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dvContractPackage)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.contractpackage_pDs)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridEX1)).EndInit();
            this.ResumeLayout(false);

		}
		#endregion

		#region ��Ʈ�� �ε�
		private void UserControl_Load(object sender, System.EventArgs e)
		{
			// �����Ͱ����� ��ü����
			dt = ((DataView)grdExContractPackageList.DataSource).Table;  
			cm = (CurrencyManager) this.BindingContext[grdExContractPackageList.DataSource]; 
			cm.PositionChanged += new System.EventHandler(OnGrdRowChanged); 

			// ��Ʈ�� �ʱ�ȭ
			InitControl();	
		}

		#endregion

		#region ��Ʈ�� �ʱ�ȭ
		private void InitControl()
		{
			ProgressStart();
			InitCombo();			
			SearchContractPackage();			
			ProgressStop();
		}

		private void InitCombo()
		{
			Init_RapCode();
			InitCombo_Level();

		}

		private void InitCombo_Level() 
		{			

			//�̵� �����ϰ���� ���ȵ��ó��
			if (commonModel.UserLevel == "30")
			{
				//��ȸ �޺� �Ƚ�
				cbSearchRap.SelectedValue = commonModel.RapCode;
				cbSearchRap.ReadOnly = true;                
			}
			Application.DoEvents();
		}   
        

		private void Init_RapCode()
		{
			// ���� ��ȸ�Ѵ�.
			MediaRapCodeModel mediaRapCodeModel = new MediaRapCodeModel();
			new MediaRapCodeManager(systemModel, commonModel).GetMediaRapCodeList(mediaRapCodeModel);
			
			if (mediaRapCodeModel.ResultCD.Equals("0000"))
			{
				// �����ͼ¿� ����
				Utility.SetDataTable(contractpackage_pDs.MediaRap, mediaRapCodeModel.MediaRapCodeDataSet);				
			}

			// �˻������� �޺�
			this.cbSearchRap.Items.Clear();
			// �޺��ڽ��� ��Ʈ�� �ڵ����� ���� Item�迭�� ����
			Janus.Windows.EditControls.UIComboBoxItem[] comboItems = new Janus.Windows.EditControls.UIComboBoxItem[mediaRapCodeModel.ResultCnt + 1];

			comboItems[0] = new Janus.Windows.EditControls.UIComboBoxItem("�̵�����","00");
			
			for(int i=0;i<mediaRapCodeModel.ResultCnt;i++)
			{
				DataRow row = contractpackage_pDs.MediaRap.Rows[i];

				string val = row["RapCode"].ToString();
				string txt = row["RapName"].ToString();
				comboItems[i+1] = new Janus.Windows.EditControls.UIComboBoxItem(txt,val);
			}
			// �޺��� ��Ʈ
			this.cbSearchRap.Items.AddRange(comboItems);
			this.cbSearchRap.SelectedIndex = 0;

			Application.DoEvents();
		}

		
		#endregion

		#region ������� �׼�ó�� �޼ҵ�

		/// <summary>
		/// �׸����� Row�����
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void OnGrdRowChanged(object sender, System.EventArgs e) 
		{
			if(grdExContractPackageList.RowCount > 0)
			{
				
			}
		}

		/// <summary>
		/// ��ȸ��ư Ŭ��
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnSearch_Click(object sender, System.EventArgs e)
		{
			ProgressStart();			
			SearchContractPackage();			
			ProgressStop();
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
				SearchContractPackage();
			}
		}
		
		#endregion

		#region ó���޼ҵ�

		/// <summary>
		/// �������� ��ȸ
		/// </summary>
		private void SearchContractPackage()
		{
			StatusMessage("������ ������ ��ȸ�մϴ�.");

			try
			{				
				//���� ���� ���� �ʱ�ȭ ���ش�.
				contractModel.Init();				
				// �����͸𵨿� ������ ������ ��Ʈ�Ѵ�.
				contractModel.SearchRap        = cbSearchRap.SelectedValue.ToString();
				
				if(IsNewSearchKey)
				{
					contractModel.SearchKey = "";					
				}
				else
				{
					contractModel.SearchKey  = ebSearchKey.Text;
				}				
				
				// ���� ��� ��� ���񽺸� ȣ���Ѵ�.
				new ContractManager(systemModel,commonModel).GetContractPackageList(contractModel);				

				if (contractModel.ResultCD.Equals("0000"))
				{											
					Utility.SetDataTable(contractpackage_pDs.Package, contractModel.ContractDataSet);			
					StatusMessage(contractModel.ResultCnt + "���� ������� ������ ��ȸ�Ǿ����ϴ�."); 					
					
				}
			}
			catch(FrameException fe)
			{
				FrameSystem.showMsgForm("���������ȸ����", new string[] {fe.ErrCode, fe.ResultMsg});
			}
			catch(Exception ex)
			{
				FrameSystem.showMsgForm("���������ȸ����",new string[] {"",ex.Message});
			}
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

		private void btnOk_Click(object sender, System.EventArgs e)
		{
			string newAdTime = grdExContractPackageList.SelectedItems[0].GetRow().Cells["AdTime"].Value.ToString();			
			string newBonusRate = grdExContractPackageList.SelectedItems[0].GetRow().Cells["BonusRate"].Value.ToString();			
			string newContractAmt = grdExContractPackageList.SelectedItems[0].GetRow().Cells["ContractAmt"].Value.ToString();			
			string newPrice = grdExContractPackageList.SelectedItems[0].GetRow().Cells["Price"].Value.ToString();			
			string newPackageName = grdExContractPackageList.SelectedItems[0].GetRow().Cells["PackageName"].Value.ToString();			
			this.parentCtl.AdTime = newAdTime;				
			this.parentCtl.BonusRate = newBonusRate;				
			this.parentCtl.ContractAmt1 = newContractAmt;				
			this.parentCtl.Price = newPrice;				
			this.parentCtl.PackageName = newPackageName;				
			this.Close();
		}

		
		private void btnClose_Click(object sender, System.EventArgs e)
		{
			this.Close();
		}

		private void grdExContractPackageList_DoubleClick(object sender, System.EventArgs e)
		{
			string newAdTime = grdExContractPackageList.SelectedItems[0].GetRow().Cells["AdTime"].Value.ToString();			
			string newBonusRate = grdExContractPackageList.SelectedItems[0].GetRow().Cells["BonusRate"].Value.ToString();
			string newContractAmt = grdExContractPackageList.SelectedItems[0].GetRow().Cells["ContractAmt"].Value.ToString();			
			string newPrice = grdExContractPackageList.SelectedItems[0].GetRow().Cells["Price"].Value.ToString();
			string newPackageName = grdExContractPackageList.SelectedItems[0].GetRow().Cells["PackageName"].Value.ToString();			
			this.parentCtl.AdTime = newAdTime;				
			this.parentCtl.BonusRate = newBonusRate;		
			this.parentCtl.ContractAmt1 = newContractAmt;				
			this.parentCtl.Price = newPrice;		
			this.parentCtl.PackageName = newPackageName;	
			this.Close();
		}

		

	
	}

}

