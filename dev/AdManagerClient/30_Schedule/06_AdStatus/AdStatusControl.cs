// ===============================================================================
// AdStatusControl for Charites Project
//
// AdStatusControl.cs
//
// �������ϰ��� ������� �����մϴ�. 
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
using Excel = Microsoft.Office.Interop.Excel; // ���� ����
using System.Reflection;


namespace AdManagerClient
{	
	/// <summary>
	/// �������ϰ��� ��Ʈ��
	/// </summary>
    public class AdStatusControl : System.Windows.Forms.UserControl, IUserControl
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
		AdStatusModel adStatusModel  = new AdStatusModel();	// �������ϸ�

		// ȭ��ó���� ����
		bool IsNewSearchKey		  = true;					// �˻����Է� ����
		CurrencyManager cm        = null;					// ������ �׸����� ���濡 ���� �����ͼ� ������ ���Ͽ�			
		DataTable  dt        = null;

        bool IsSearching = false;
		bool canRead			  = false;
		bool canUpdate			  = false;
				
		private Janus.Windows.GridEX.EditControls.EditBox ebSearchKey;
		private Janus.Windows.EditControls.UIComboBox cbSearchMedia;
		private Janus.Windows.EditControls.UIButton btnSearch;
		private System.Data.DataView dvAdStatus;
		private Janus.Windows.GridEX.GridEX grdExAdStatusList;
		private Janus.Windows.EditControls.UIButton btnEcxel;
		private AdManagerClient._30_Schedule._06_AdStatus.AdStatusDs adStatusDs;

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
		private System.Windows.Forms.Panel pnlSearch;
		private Janus.Windows.UI.Dock.UIPanelGroup uiPanelAdFile;
		private Janus.Windows.UI.Dock.UIPanel uiPanelSearch;
		private Janus.Windows.UI.Dock.UIPanelInnerContainer uiPanelSearchContainer;
		private Janus.Windows.UI.Dock.UIPanel uiPanelList;
		private Janus.Windows.UI.Dock.UIPanelInnerContainer uiPanelListContainer;
		private System.ComponentModel.IContainer components;

		public AdStatusControl()
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
				//				if (ftm != null)
				//				{
				//					ftm.Close();
				//				}

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
			Janus.Windows.GridEX.GridEXLayout grdExAdStatusList_DesignTimeLayout = new Janus.Windows.GridEX.GridEXLayout();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AdStatusControl));
			this.uiPM = new Janus.Windows.UI.Dock.UIPanelManager(this.components);
			this.uiPanelAdFile = new Janus.Windows.UI.Dock.UIPanelGroup();
			this.uiPanelSearch = new Janus.Windows.UI.Dock.UIPanel();
			this.uiPanelSearchContainer = new Janus.Windows.UI.Dock.UIPanelInnerContainer();
			this.pnlSearch = new System.Windows.Forms.Panel();
			this.btnEcxel = new Janus.Windows.EditControls.UIButton();
			this.btnSearch = new Janus.Windows.EditControls.UIButton();
			this.ebSearchKey = new Janus.Windows.GridEX.EditControls.EditBox();
			this.cbSearchMedia = new Janus.Windows.EditControls.UIComboBox();
			this.uiPanelList = new Janus.Windows.UI.Dock.UIPanel();
			this.uiPanelListContainer = new Janus.Windows.UI.Dock.UIPanelInnerContainer();
			this.grdExAdStatusList = new Janus.Windows.GridEX.GridEX();
			this.dvAdStatus = new System.Data.DataView();
			this.adStatusDs = new AdManagerClient._30_Schedule._06_AdStatus.AdStatusDs();
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
			((System.ComponentModel.ISupportInitialize)(this.grdExAdStatusList)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.dvAdStatus)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.adStatusDs)).BeginInit();
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
			this.uiPM.Panels.Add(this.uiPanelAdFile);
			// 
			// Design Time Panel Info:
			// 
			this.uiPM.BeginPanelInfo();
			this.uiPM.AddDockPanelInfo(new System.Guid("b162c53a-7940-47cd-a0ed-9762c59c50d8"), Janus.Windows.UI.Dock.PanelGroupStyle.HorizontalTiles, Janus.Windows.UI.Dock.PanelDockStyle.Fill, true, new System.Drawing.Size(1010, 677), true);
			this.uiPM.AddDockPanelInfo(new System.Guid("afb3b7d0-aa79-443c-a1e8-3baba38f7a4d"), new System.Guid("b162c53a-7940-47cd-a0ed-9762c59c50d8"), 40, true);
			this.uiPM.AddDockPanelInfo(new System.Guid("8e3140b9-bfcb-43af-b84e-2ee6cddf51dc"), new System.Guid("b162c53a-7940-47cd-a0ed-9762c59c50d8"), 577, true);
			this.uiPM.AddFloatingPanelInfo(new System.Guid("b162c53a-7940-47cd-a0ed-9762c59c50d8"), Janus.Windows.UI.Dock.PanelGroupStyle.HorizontalTiles, true, new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
			this.uiPM.AddFloatingPanelInfo(new System.Guid("8e3140b9-bfcb-43af-b84e-2ee6cddf51dc"), new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
			this.uiPM.AddFloatingPanelInfo(new System.Guid("b2973a57-d3e1-4d80-9387-7a6dc3332542"), new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
			this.uiPM.AddFloatingPanelInfo(new System.Guid("afb3b7d0-aa79-443c-a1e8-3baba38f7a4d"), new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
			this.uiPM.EndPanelInfo();
			// 
			// uiPanelAdFile
			// 
			this.uiPanelAdFile.ActiveCaptionMode = Janus.Windows.UI.Dock.ActiveCaptionMode.Never;
			this.uiPanelAdFile.BackColor = System.Drawing.SystemColors.Window;
			this.uiPanelAdFile.CaptionVisible = Janus.Windows.UI.InheritableBoolean.True;
			this.uiPanelAdFile.CloseButtonVisible = Janus.Windows.UI.InheritableBoolean.False;
			this.uiPanelAdFile.Location = new System.Drawing.Point(0, 0);
			this.uiPanelAdFile.Name = "uiPanelAdFile";
			this.uiPanelAdFile.Size = new System.Drawing.Size(1010, 677);
			this.uiPanelAdFile.TabIndex = 4;
			this.uiPanelAdFile.Text = "��������Ȳ";
			// 
			// uiPanelSearch
			// 
			this.uiPanelSearch.AllowResize = Janus.Windows.UI.InheritableBoolean.False;
			this.uiPanelSearch.CaptionVisible = Janus.Windows.UI.InheritableBoolean.False;
			this.uiPanelSearch.CloseButtonVisible = Janus.Windows.UI.InheritableBoolean.False;
			this.uiPanelSearch.InnerContainer = this.uiPanelSearchContainer;
			this.uiPanelSearch.Location = new System.Drawing.Point(0, 22);
			this.uiPanelSearch.Name = "uiPanelSearch";
			this.uiPanelSearch.Size = new System.Drawing.Size(1010, 42);
			this.uiPanelSearch.TabIndex = 4;
			this.uiPanelSearch.Text = "�˻�";
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
			this.pnlSearch.Controls.Add(this.btnEcxel);
			this.pnlSearch.Controls.Add(this.btnSearch);
			this.pnlSearch.Controls.Add(this.ebSearchKey);
			this.pnlSearch.Controls.Add(this.cbSearchMedia);
			this.pnlSearch.Dock = System.Windows.Forms.DockStyle.Fill;
			this.pnlSearch.Location = new System.Drawing.Point(0, 0);
			this.pnlSearch.Name = "pnlSearch";
			this.pnlSearch.Size = new System.Drawing.Size(1008, 40);
			this.pnlSearch.TabIndex = 3;
			// 
			// btnEcxel
			// 
			this.btnEcxel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnEcxel.Enabled = false;
			this.btnEcxel.Location = new System.Drawing.Point(895, 8);
			this.btnEcxel.Name = "btnEcxel";
			this.btnEcxel.Size = new System.Drawing.Size(104, 24);
			this.btnEcxel.TabIndex = 4;
			this.btnEcxel.Text = "EXCEL ���";
			this.btnEcxel.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
			this.btnEcxel.Click += new System.EventHandler(this.btnExel_Click);
			// 
			// btnSearch
			// 
			this.btnSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnSearch.Enabled = false;
			this.btnSearch.Location = new System.Drawing.Point(783, 8);
			this.btnSearch.Name = "btnSearch";
			this.btnSearch.Size = new System.Drawing.Size(104, 24);
			this.btnSearch.TabIndex = 3;
			this.btnSearch.Text = "�� ȸ";
			this.btnSearch.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
			this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
			// 
			// ebSearchKey
			// 
			this.ebSearchKey.Location = new System.Drawing.Point(174, 8);
			this.ebSearchKey.Name = "ebSearchKey";
			this.ebSearchKey.Size = new System.Drawing.Size(216, 21);
			this.ebSearchKey.TabIndex = 2;
			this.ebSearchKey.Text = "�˻�� �Է��ϼ���";
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
			this.cbSearchMedia.Size = new System.Drawing.Size(160, 21);
			this.cbSearchMedia.TabIndex = 1;
			this.cbSearchMedia.Text = "��ü����";
			this.cbSearchMedia.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
			// 
			// uiPanelList
			// 
			this.uiPanelList.BackColor = System.Drawing.SystemColors.Window;
			this.uiPanelList.CaptionStyle = Janus.Windows.UI.Dock.PanelCaptionStyle.Dark;
			this.uiPanelList.InnerContainer = this.uiPanelListContainer;
			this.uiPanelList.Location = new System.Drawing.Point(0, 68);
			this.uiPanelList.MinimumSize = new System.Drawing.Size(-1, 100);
			this.uiPanelList.Name = "uiPanelList";
			this.uiPanelList.Size = new System.Drawing.Size(1010, 609);
			this.uiPanelList.TabIndex = 16;
			this.uiPanelList.TabStop = false;
			this.uiPanelList.Text = "��������Ȳ";
			// 
			// uiPanelListContainer
			// 
			this.uiPanelListContainer.BackColor = System.Drawing.SystemColors.Window;
			this.uiPanelListContainer.Controls.Add(this.grdExAdStatusList);
			this.uiPanelListContainer.Location = new System.Drawing.Point(1, 23);
			this.uiPanelListContainer.Name = "uiPanelListContainer";
			this.uiPanelListContainer.Size = new System.Drawing.Size(1008, 585);
			this.uiPanelListContainer.TabIndex = 0;
			// 
			// grdExAdStatusList
			// 
			this.grdExAdStatusList.AllowEdit = Janus.Windows.GridEX.InheritableBoolean.False;
			this.grdExAdStatusList.AlternatingColors = true;
			this.grdExAdStatusList.BorderStyle = Janus.Windows.GridEX.BorderStyle.None;
			this.grdExAdStatusList.DataSource = this.dvAdStatus;
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
			this.grdExAdStatusList.SelectedInactiveFormatStyle.BackColor = System.Drawing.Color.DarkOrange;
			this.grdExAdStatusList.Size = new System.Drawing.Size(1008, 585);
			this.grdExAdStatusList.TabIndex = 5;
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
			// dvAdStatus
			// 
			this.dvAdStatus.Table = this.adStatusDs.AdStatus;
			// 
			// adStatusDs
			// 
			this.adStatusDs.DataSetName = "AdStatusDs";
			this.adStatusDs.Locale = new System.Globalization.CultureInfo("en-US");
			this.adStatusDs.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
			// 
			// AdStatusControl
			// 
			this.Controls.Add(this.uiPanelAdFile);
			this.Font = global::AdManagerClient.Properties.Settings.Default.Font1;
			this.Name = "AdStatusControl";
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
			((System.ComponentModel.ISupportInitialize)(this.grdExAdStatusList)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.dvAdStatus)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.adStatusDs)).EndInit();
			this.ResumeLayout(false);

		}
		#endregion

		#region ��Ʈ�� �ε�
		private void UserControl_Load(object sender, System.EventArgs e)
		{
			Application.DoEvents();

			// �����Ͱ����� ��ü����
			dt = ((DataView)grdExAdStatusList.DataSource).Table;  
			cm = (CurrencyManager) this.BindingContext[grdExAdStatusList.DataSource]; 
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
			InitCombo_Level();
	
			// ��ȸ���� �˻�
			if(menu.CanRead(MenuCode))
			{
				canRead = true;
				//SearchAdFile();
			}

			// �������� �˻�
			if(menu.CanUpdate(MenuCode))
			{
				canUpdate = true;
			}	
		
			// ������ư Ȱ��ȭ
			if(menu.CanDelete(MenuCode))
			{
				//canDelete = true;
			}
			InitButton();
			ProgressStop();
		}


		private void InitCombo()
		{
			Init_MediaCode();
		}

		private void Init_MediaCode()
		{
			// ��ü�� ��ȸ�Ѵ�.
			MediaCodeModel mediacodeModel = new MediaCodeModel();
			new MediaCodeManager(systemModel, commonModel).GetMediaCodeList(mediacodeModel);
			
			if (mediacodeModel.ResultCD.Equals("0000"))
			{
				// �����ͼ¿� ����
				Utility.SetDataTable(adStatusDs.Medias, mediacodeModel.MediaCodeDataSet);				
			}

			// �˻������� �޺�
			this.cbSearchMedia.Items.Clear();
			
			// �޺��ڽ��� ��Ʈ�� �ڵ����� ���� Item�迭�� ����
			Janus.Windows.EditControls.UIComboBoxItem[] comboItems = new Janus.Windows.EditControls.UIComboBoxItem[mediacodeModel.ResultCnt + 1];

			comboItems[0] = new Janus.Windows.EditControls.UIComboBoxItem("��ü����","00");
			
			for(int i=0;i<mediacodeModel.ResultCnt;i++)
			{
				DataRow row = adStatusDs.Medias.Rows[i];

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
				for(int i=0;i < adStatusDs.Medias.Rows.Count;i++)
				{
					DataRow row = adStatusDs.Medias.Rows[i];					
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
			if(canRead)   btnEcxel.Enabled = true;		
			grdExAdStatusList.Focus();

			Application.DoEvents();	
		}
	

		private void DisableButton()
		{
			btnSearch.Enabled = false;		
		
			Application.DoEvents();
		}

		#endregion

		#region �������� �׼�ó�� �޼ҵ�

		/// <summary>
		/// �׸����� Row�����
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void OnGrdRowChanged(object sender, System.EventArgs e) 
		{
            if (!IsSearching) // 2011.11.29 JH.Park ��ȸ���� �ƴҰ�쿡�� �����ϵ��� ����
            {
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
			ProgressStart();
			DisableButton();
            SearchAdSchedule();
			InitButton();
			ProgressStop();
		}

		/// <summary>
		/// �����ư Ŭ��
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>

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
                SearchAdSchedule();
			}
		}


		private void grdExAdStatusList_ColumnHeaderClick(object sender, Janus.Windows.GridEX.ColumnActionEventArgs e)
		{			
			//�÷�Index 0(üũ�ڽ��÷���)�� �ƴϸ� ���������� ó��.
			if(e.Column.Index != 0)
			{
				return;
			}
        
			//ColumnHeader Click�ÿ� dt Setting 
			DataRow[] foundRows = dt.Select("CheckYn = 'False'");

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


		#endregion

		#region ó���޼ҵ�

		/// <summary>
		/// �������Ϲ��� ��ȸ
		/// </summary>
		private void SearchAdSchedule()
		{
            IsSearching = true;
			StatusMessage("������ ������ ��ȸ�մϴ�.");

			if(cbSearchMedia.SelectedItem.Value.Equals("00")) 
			{
				MessageBox.Show("��ü�� �����Ͽ� �ֽñ� �ٶ��ϴ�.","��������Ȳ ��ȸ", MessageBoxButtons.OK, MessageBoxIcon.Information );
				return;
			}

			try
			{
				adStatusModel.Init();

				// �����͸𵨿� ������ ������ ��Ʈ�Ѵ�.
				if(IsNewSearchKey)
				{
					adStatusModel.SearchKey = "";
				}
				else
				{
					adStatusModel.SearchKey  = ebSearchKey.Text;
				}

				adStatusModel.SearchMediaCode		 =  cbSearchMedia.SelectedItem.Value.ToString(); 

				// �������Ϲ�����ȸ ���񽺸� ȣ���Ѵ�.
				new AdStatusManager(systemModel,commonModel).GetAdStatusList(adStatusModel);

				if (adStatusModel.ResultCD.Equals("0000"))
				{
                    // �ܼ���ȸ���� _Fast�� ����ϼ���
					Utility.SetDataTableFast(adStatusDs.AdStatus, adStatusModel.AdStatusDataSet);		

					StatusMessage(adStatusModel.ResultCnt + "���� �� ������ ��ȸ�Ǿ����ϴ�.");
					if(canUpdate)
					{
						AddSchChoice();									
					}																				
				}
			}
			catch(FrameException fe)
			{
				FrameSystem.showMsgForm("��������ȸ����", new string[] {fe.ErrCode, fe.ResultMsg});
			}
			catch(Exception ex)
			{
				FrameSystem.showMsgForm("��������ȸ����",new string[] {"",ex.Message});
			}
            finally
            {
                if (adStatusModel.AdStatusDataSet != null)
                {
                    adStatusModel.AdStatusDataSet.Clear();
                    adStatusModel.AdStatusDataSet.Dispose();
                    adStatusModel.AdStatusDataSet = null;
                }

                IsSearching = false; // ��ȸ�� Flag ����
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
				if ( adStatusDs.Tables["AdStatus"].Rows.Count < 1 ) return;
              
				foreach (DataRow row in adStatusDs.Tables["AdStatus"].Rows)
				{					
				
					if(row["ItemNo"].ToString().Equals(adStatusModel.ItemNo))
					{					
						cm.Position = rowIndex;
						break;								
					}
				
					rowIndex++;
					grdExAdStatusList.EnsureVisible();
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
		
		#region ���� ���
		/// <summary>
		/// ���� ����
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnExel_Click(object sender, System.EventArgs e)
		{		
//			if (CineSystem.officeVer == "error")
//			{
//				MessageForm mf = new MessageForm();
//				mf.SetMessage = new string[]{"���ǽ� 2000,xp,2003 ����","���� PC�� ���ǽ��� ��ġ �Ǿ�����","Ȯ���� �ּ���!"};
//				mf.showMessage();
//				mf.ShowDialog();
//				return;
//			}

			Excel.Application xlApp= null;
			Excel._Workbook   oWB = null;
			Excel._Worksheet  oSheet = null;
			Excel.Range       oRng = null;
			
			try
			{						

				xlApp = new Excel.Application();
				oWB = (Excel._Workbook)(xlApp.Workbooks.Add(Missing.Value));
				oSheet = (Excel._Worksheet)oWB.ActiveSheet;
				
				// ��� ���� �ۼ�
				oSheet.Cells[2,1] = "����";
				oSheet.Cells[2,2] = "�ڵ�";
				oSheet.Cells[2,3] = "ī�װ�";
				oSheet.Cells[2,4] = "�ڵ�";
				oSheet.Cells[2,5] = "�帣";
				oSheet.Cells[2,6] = "ä��";
				oSheet.Cells[2,7] = "����";
				oSheet.Cells[2,8] = "�����ȣ";
				oSheet.Cells[2,9] = "�����";
				oSheet.Cells[2,10] = "��������";
				oSheet.Cells[2,11] = "�������";				
				oSheet.Cells[2,12] = "�������";				
				oSheet.Cells[2,13] = "������ġ";
				oSheet.Cells[2,14] = "���ϸ�";
				oSheet.Cells[2,15] = "���ϻ���";
				oSheet.Cells[2,16] = "����ũ��";
				oSheet.Cells[2,17] = "�ٿ����";
				
//				if (CineSystem.officeVer == "2000")
//					oSheet.get_Range("A1", "A1").Value2 = lblDesc.Text;
//				else
//					oSheet.get_Range("A1", "A1").set_Value(Missing.Value, lblDesc.Text);			

				oSheet.get_Range("A2", "Q2").Font.Bold = true;
				oSheet.get_Range("A2", "Q2").VerticalAlignment = Excel.XlVAlign.xlVAlignCenter;			
				
				
				//�ʿ��� �÷��� ���
				string[,] items = new string[adStatusDs.Tables[0].Rows.Count, 17];
				//string pType = string.Empty;

				for (int inx =0; inx < adStatusDs.Tables[0].Rows.Count; inx++)
				{
					items[inx, 0] = adStatusDs.Tables[0].Rows[inx]["Flag"].ToString();
					items[inx, 1] = adStatusDs.Tables[0].Rows[inx]["CategoryCode"].ToString();
					items[inx, 2] = adStatusDs.Tables[0].Rows[inx]["CategoryName"].ToString();
					items[inx, 3] = adStatusDs.Tables[0].Rows[inx]["GenreCode"].ToString();					
					items[inx, 4] = adStatusDs.Tables[0].Rows[inx]["GenreName"].ToString();					
					items[inx, 5] = adStatusDs.Tables[0].Rows[inx]["ChannelNo"].ToString();
					items[inx, 6] = adStatusDs.Tables[0].Rows[inx]["Title"].ToString();
					items[inx, 7] = adStatusDs.Tables[0].Rows[inx]["ItemNo"].ToString();
					items[inx, 8] = adStatusDs.Tables[0].Rows[inx]["ItemName"].ToString();

					items[inx, 9] = adStatusDs.Tables[0].Rows[inx]["AdTypeName"].ToString();
					items[inx, 10] = adStatusDs.Tables[0].Rows[inx]["AdStateName"].ToString();
					items[inx, 11] = adStatusDs.Tables[0].Rows[inx]["ScheduleOrder"].ToString();
					items[inx, 12] = adStatusDs.Tables[0].Rows[inx]["FilePath"].ToString();
					items[inx, 13] = adStatusDs.Tables[0].Rows[inx]["FileName"].ToString();
					items[inx, 14] = adStatusDs.Tables[0].Rows[inx]["FileStateName"].ToString();
					items[inx, 15] = adStatusDs.Tables[0].Rows[inx]["FileLength"].ToString();
					items[inx, 16] = adStatusDs.Tables[0].Rows[inx]["DownLevelName"].ToString();
				}
					oSheet.get_Range("A3", "O"+Convert.ToString((items.Length/17)+2)).set_Value(Missing.Value, items);
//				if (CineSystem.officeVer == "2000")
//					oSheet.get_Range("A3", "I"+Convert.ToString((items.Length/9)+2)).Value2 = items;
//				else
//					oSheet.get_Range("A3", "I"+Convert.ToString((items.Length/9)+2)).set_Value(Missing.Value, items);
				
				
				oRng = oSheet.get_Range("B2", "Q2");
				oRng.EntireColumn.AutoFit();
				
				xlApp.Visible = true;
				xlApp.UserControl = true;


			}
			catch(Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
		}
	}
	#endregion
}
