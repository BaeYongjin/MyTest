// ===============================================================================
// MediaAgencyControl for Charites Project
//
// MediaAgencyControl.cs
//
// ��ü������������� ������� �����մϴ�. 
//
// ===============================================================================
// Release history
//
// ===============================================================================
// Copyright (C) 2006 G-Inno Systems Inc.
// All rights reserved.
// 

/*
 * -------------------------------------------------------
 * Class Name: MediaAgencyControl
 * �ֿ���  : ��ü������������� ��Ʈ��
 * �ۼ���    : ��
 * �ۼ���    : ��
 * -------------------------------------------------------
 * [��������]
 * -------------------------------------------------------
 * �����ڵ�  : [E_01]
 * ������    : �躸��
 * ������    : 2013.02
 * ��������  :        
 *            - �̵� ��Ʈ�� Disable�� �˻� ����
 * �����Լ�  :
 *            - GetAgencyCodeList
 *            - �̵� ��Ʈ�� Disable
 * --------------------------------------------------------
 */

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
	/// ��ü�������� ��Ʈ��
	/// </summary>
    public class MediaAgencyControl : System.Windows.Forms.UserControl, IUserControl
	{
		#region �̺�Ʈ�ڵ鷯
		public event StatusEventHandler 			StatusEvent;			// �����̺�Ʈ �ڵ鷯
        public event ProgressEventHandler 			ProgressEvent;			// ó�����̺�Ʈ �ڵ鷯
		#endregion
			
		#region ��ü��������� ��ü �� ����

		// �ý��� ���� : ȭ�����
		private SystemModel   systemModel   = FrameSystem.oSysModel;
		private CommonModel   commonModel   = FrameSystem.oComModel;
		private Logger        log           = FrameSystem.oLog;
		private MenuPower     menu          = FrameSystem.oMenu;

		// �޴��ڵ� : ������ �ʿ��� ȭ�鿡 �ʿ���
		public string        menuCode		= "";

		// ����� ������
		MediaAgencyModel mediaAgencyModel  = new MediaAgencyModel();	// ��ü�����������

		// ȭ��ó���� ����
		bool IsNewSearchKey		  = true;					// �˻����Է� ����		
		CurrencyManager cm        = null;					// ������ �׸����� ���濡 ���� �����ͼ� ������ ���Ͽ�			
		DataTable       dt        = null;

        bool IsSearching = false; // ��ȸ�� ��ȭ���� ������Ʈ �Ǵ� ���� ���� �ϱ����� 2011.11.29 JH.Park
		bool IsAdding             = false;
		bool canRead			  = false;
		bool canUpdate			  = false;
		bool canCreate            = false;
		bool canDelete            = false;

		private string        mediaCode = null;
		private string        rapCode = null;
        private Janus.Windows.EditControls.UICheckBox uiCheckBox1;
		private string        agencyCode = null;

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
		private Janus.Windows.UI.Dock.UIPanel uiPanelMediaAgencyList;
		private Janus.Windows.UI.Dock.UIPanelInnerContainer uiPanelMediaAgencyListContainer;
		private Janus.Windows.UI.Dock.UIPanel uiPanelMediaAgencyDetail;
		private Janus.Windows.UI.Dock.UIPanelInnerContainer uiPanelMediaAgencyDetailContainer;
		private Janus.Windows.UI.Dock.UIPanel uiPanelMediaAgencysSearch;
		private Janus.Windows.UI.Dock.UIPanelInnerContainer uiPanelMediaAgencysSearchContainer;
		private Janus.Windows.EditControls.UIButton uiButton2;
		private Janus.Windows.EditControls.UIButton uiButton1;
		private Janus.Windows.UI.Dock.UIPanelGroup uiPanelMediaAgencys;
		private System.Windows.Forms.Panel pnlSearch;
		private Janus.Windows.GridEX.EditControls.EditBox ebSearchKey;
		private Janus.Windows.EditControls.UIButton btnSearch;
		private System.Windows.Forms.Panel pnlMediaAgencyDetail;
		private System.Windows.Forms.Label lbRegDt;
		private Janus.Windows.EditControls.UIButton btnAdd;
		private Janus.Windows.EditControls.UIButton btnSave;
		private System.Data.DataView dvMediaAgency;
		private Janus.Windows.GridEX.EditControls.EditBox ebRegDt;
		private Janus.Windows.EditControls.UIButton btnDelete;		
		private System.Windows.Forms.Label lbModDt;
		private Janus.Windows.GridEX.EditControls.EditBox ebModDt;
		private Janus.Windows.GridEX.GridEX grdExMediaAgencyList;		
		private Janus.Windows.GridEX.EditControls.EditBox ebEmail;
		private System.Windows.Forms.Label lbTell;
		private System.Windows.Forms.Label lbUseYn;
		private System.Windows.Forms.Label lbEmail;
		private Janus.Windows.GridEX.EditControls.EditBox ebCharger;
		private System.Windows.Forms.Label lbCharger;
		private Janus.Windows.GridEX.EditControls.EditBox ebTell;
		private System.Windows.Forms.Label lbContStarDay;
		private System.Windows.Forms.Label lbContEndDay;
		private Janus.Windows.EditControls.UIRadioButton rbUseYn_Y;
		private Janus.Windows.EditControls.UIRadioButton rbUseYn_N;
		private Janus.Windows.EditControls.UIComboBox cbSearchMediaName;
		private Janus.Windows.EditControls.UIComboBox cbSearchMediaAgency;
		private Janus.Windows.EditControls.UIComboBox cbSearchRapName;
		private AdManagerClient.MediaAgencyDs mediaAgencyDs;
		private System.Windows.Forms.Label lbRapName;
		private System.Windows.Forms.Label lbMediaName;
		private System.Windows.Forms.Label lbAgencyName;
		private Janus.Windows.EditControls.UIComboBox uiComboBox1;
		private System.Windows.Forms.Label lbRegID;
		private Janus.Windows.GridEX.EditControls.EditBox ebRegID;
		private Janus.Windows.GridEX.EditControls.EditBox ebMediaName;
		private Janus.Windows.GridEX.EditControls.EditBox ebRapName;
		private Janus.Windows.GridEX.EditControls.EditBox ebAgencyName;
		private Janus.Windows.EditControls.UIButton btnMediaSearch;
		private Janus.Windows.EditControls.UIButton btnAgencySearch;
		private Janus.Windows.EditControls.UIButton btnRapSearch;
		private Janus.Windows.CalendarCombo.CalendarCombo ebContStartDay;
        private Janus.Windows.CalendarCombo.CalendarCombo ebContEndDay;
		private System.ComponentModel.IContainer components;

		public MediaAgencyControl()
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
			Janus.Windows.GridEX.GridEXLayout grdExMediaAgencyList_DesignTimeLayout = new Janus.Windows.GridEX.GridEXLayout();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MediaAgencyControl));
			this.uiPM = new Janus.Windows.UI.Dock.UIPanelManager(this.components);
			this.uiPanelMediaAgencys = new Janus.Windows.UI.Dock.UIPanelGroup();
			this.uiPanelMediaAgencysSearch = new Janus.Windows.UI.Dock.UIPanel();
			this.uiPanelMediaAgencysSearchContainer = new Janus.Windows.UI.Dock.UIPanelInnerContainer();
			this.pnlSearch = new System.Windows.Forms.Panel();
			this.uiCheckBox1 = new Janus.Windows.EditControls.UICheckBox();
			this.ebSearchKey = new Janus.Windows.GridEX.EditControls.EditBox();
			this.cbSearchMediaName = new Janus.Windows.EditControls.UIComboBox();
			this.cbSearchRapName = new Janus.Windows.EditControls.UIComboBox();
			this.cbSearchMediaAgency = new Janus.Windows.EditControls.UIComboBox();
			this.btnSearch = new Janus.Windows.EditControls.UIButton();
			this.uiPanelMediaAgencyList = new Janus.Windows.UI.Dock.UIPanel();
			this.uiPanelMediaAgencyListContainer = new Janus.Windows.UI.Dock.UIPanelInnerContainer();
			this.grdExMediaAgencyList = new Janus.Windows.GridEX.GridEX();
			this.dvMediaAgency = new System.Data.DataView();
			this.mediaAgencyDs = new AdManagerClient.MediaAgencyDs();
			this.uiPanelMediaAgencyDetail = new Janus.Windows.UI.Dock.UIPanel();
			this.uiPanelMediaAgencyDetailContainer = new Janus.Windows.UI.Dock.UIPanelInnerContainer();
			this.pnlMediaAgencyDetail = new System.Windows.Forms.Panel();
			this.ebContEndDay = new Janus.Windows.CalendarCombo.CalendarCombo();
			this.btnMediaSearch = new Janus.Windows.EditControls.UIButton();
			this.ebAgencyName = new Janus.Windows.GridEX.EditControls.EditBox();
			this.ebRapName = new Janus.Windows.GridEX.EditControls.EditBox();
			this.ebMediaName = new Janus.Windows.GridEX.EditControls.EditBox();
			this.rbUseYn_N = new Janus.Windows.EditControls.UIRadioButton();
			this.rbUseYn_Y = new Janus.Windows.EditControls.UIRadioButton();
			this.lbUseYn = new System.Windows.Forms.Label();
			this.lbEmail = new System.Windows.Forms.Label();
			this.lbModDt = new System.Windows.Forms.Label();
			this.lbTell = new System.Windows.Forms.Label();
			this.ebTell = new Janus.Windows.GridEX.EditControls.EditBox();
			this.ebCharger = new Janus.Windows.GridEX.EditControls.EditBox();
			this.lbCharger = new System.Windows.Forms.Label();
			this.lbRegDt = new System.Windows.Forms.Label();
			this.ebRegDt = new Janus.Windows.GridEX.EditControls.EditBox();
			this.ebModDt = new Janus.Windows.GridEX.EditControls.EditBox();
			this.ebEmail = new Janus.Windows.GridEX.EditControls.EditBox();
			this.lbContStarDay = new System.Windows.Forms.Label();
			this.lbContEndDay = new System.Windows.Forms.Label();
			this.lbRapName = new System.Windows.Forms.Label();
			this.lbMediaName = new System.Windows.Forms.Label();
			this.lbAgencyName = new System.Windows.Forms.Label();
			this.lbRegID = new System.Windows.Forms.Label();
			this.ebRegID = new Janus.Windows.GridEX.EditControls.EditBox();
			this.btnAdd = new Janus.Windows.EditControls.UIButton();
			this.btnDelete = new Janus.Windows.EditControls.UIButton();
			this.btnSave = new Janus.Windows.EditControls.UIButton();
			this.btnRapSearch = new Janus.Windows.EditControls.UIButton();
			this.btnAgencySearch = new Janus.Windows.EditControls.UIButton();
			this.ebContStartDay = new Janus.Windows.CalendarCombo.CalendarCombo();
			this.uiComboBox1 = new Janus.Windows.EditControls.UIComboBox();
			this.uiButton1 = new Janus.Windows.EditControls.UIButton();
			this.uiButton2 = new Janus.Windows.EditControls.UIButton();
			((System.ComponentModel.ISupportInitialize)(this.uiPM)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.uiPanelMediaAgencys)).BeginInit();
			this.uiPanelMediaAgencys.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.uiPanelMediaAgencysSearch)).BeginInit();
			this.uiPanelMediaAgencysSearch.SuspendLayout();
			this.uiPanelMediaAgencysSearchContainer.SuspendLayout();
			this.pnlSearch.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.uiPanelMediaAgencyList)).BeginInit();
			this.uiPanelMediaAgencyList.SuspendLayout();
			this.uiPanelMediaAgencyListContainer.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.grdExMediaAgencyList)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.dvMediaAgency)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.mediaAgencyDs)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.uiPanelMediaAgencyDetail)).BeginInit();
			this.uiPanelMediaAgencyDetail.SuspendLayout();
			this.uiPanelMediaAgencyDetailContainer.SuspendLayout();
			this.pnlMediaAgencyDetail.SuspendLayout();
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
			this.uiPanelMediaAgencys.Id = new System.Guid("b162c53a-7940-47cd-a0ed-9762c59c50d8");
			this.uiPanelMediaAgencys.StaticGroup = true;
			this.uiPanelMediaAgencysSearch.Id = new System.Guid("afb3b7d0-aa79-443c-a1e8-3baba38f7a4d");
			this.uiPanelMediaAgencys.Panels.Add(this.uiPanelMediaAgencysSearch);
			this.uiPanelMediaAgencyList.Id = new System.Guid("8e3140b9-bfcb-43af-b84e-2ee6cddf51dc");
			this.uiPanelMediaAgencys.Panels.Add(this.uiPanelMediaAgencyList);
			this.uiPanelMediaAgencyDetail.Id = new System.Guid("b2973a57-d3e1-4d80-9387-7a6dc3332542");
			this.uiPanelMediaAgencys.Panels.Add(this.uiPanelMediaAgencyDetail);
			this.uiPM.Panels.Add(this.uiPanelMediaAgencys);
			// 
			// Design Time Panel Info:
			// 
			this.uiPM.BeginPanelInfo();
			this.uiPM.AddDockPanelInfo(new System.Guid("b162c53a-7940-47cd-a0ed-9762c59c50d8"), Janus.Windows.UI.Dock.PanelGroupStyle.HorizontalTiles, Janus.Windows.UI.Dock.PanelDockStyle.Fill, true, new System.Drawing.Size(1010, 677), true);
			this.uiPM.AddDockPanelInfo(new System.Guid("afb3b7d0-aa79-443c-a1e8-3baba38f7a4d"), new System.Guid("b162c53a-7940-47cd-a0ed-9762c59c50d8"), 40, true);
			this.uiPM.AddDockPanelInfo(new System.Guid("8e3140b9-bfcb-43af-b84e-2ee6cddf51dc"), new System.Guid("b162c53a-7940-47cd-a0ed-9762c59c50d8"), 385, true);
			this.uiPM.AddDockPanelInfo(new System.Guid("b2973a57-d3e1-4d80-9387-7a6dc3332542"), new System.Guid("b162c53a-7940-47cd-a0ed-9762c59c50d8"), 202, true);
			this.uiPM.AddFloatingPanelInfo(new System.Guid("b162c53a-7940-47cd-a0ed-9762c59c50d8"), Janus.Windows.UI.Dock.PanelGroupStyle.HorizontalTiles, true, new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
			this.uiPM.AddFloatingPanelInfo(new System.Guid("8e3140b9-bfcb-43af-b84e-2ee6cddf51dc"), new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
			this.uiPM.AddFloatingPanelInfo(new System.Guid("b2973a57-d3e1-4d80-9387-7a6dc3332542"), new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
			this.uiPM.AddFloatingPanelInfo(new System.Guid("afb3b7d0-aa79-443c-a1e8-3baba38f7a4d"), new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
			this.uiPM.EndPanelInfo();
			// 
			// uiPanelMediaAgencys
			// 
			this.uiPanelMediaAgencys.ActiveCaptionMode = Janus.Windows.UI.Dock.ActiveCaptionMode.Never;
			this.uiPanelMediaAgencys.BackColor = System.Drawing.SystemColors.Window;
			this.uiPanelMediaAgencys.CaptionVisible = Janus.Windows.UI.InheritableBoolean.True;
			this.uiPanelMediaAgencys.CloseButtonVisible = Janus.Windows.UI.InheritableBoolean.False;
			this.uiPanelMediaAgencys.Location = new System.Drawing.Point(0, 0);
			this.uiPanelMediaAgencys.Name = "uiPanelMediaAgencys";
			this.uiPanelMediaAgencys.Size = new System.Drawing.Size(1010, 677);
			this.uiPanelMediaAgencys.TabIndex = 4;
			this.uiPanelMediaAgencys.Text = "��ü�� ��������";
			// 
			// uiPanelMediaAgencysSearch
			// 
			this.uiPanelMediaAgencysSearch.AllowResize = Janus.Windows.UI.InheritableBoolean.False;
			this.uiPanelMediaAgencysSearch.CaptionVisible = Janus.Windows.UI.InheritableBoolean.False;
			this.uiPanelMediaAgencysSearch.CloseButtonVisible = Janus.Windows.UI.InheritableBoolean.False;
			this.uiPanelMediaAgencysSearch.InnerContainer = this.uiPanelMediaAgencysSearchContainer;
			this.uiPanelMediaAgencysSearch.Location = new System.Drawing.Point(0, 22);
			this.uiPanelMediaAgencysSearch.Name = "uiPanelMediaAgencysSearch";
			this.uiPanelMediaAgencysSearch.Size = new System.Drawing.Size(1010, 41);
			this.uiPanelMediaAgencysSearch.TabIndex = 4;
			this.uiPanelMediaAgencysSearch.Text = "�˻�";
			// 
			// uiPanelMediaAgencysSearchContainer
			// 
			this.uiPanelMediaAgencysSearchContainer.Controls.Add(this.pnlSearch);
			this.uiPanelMediaAgencysSearchContainer.Location = new System.Drawing.Point(1, 1);
			this.uiPanelMediaAgencysSearchContainer.Name = "uiPanelMediaAgencysSearchContainer";
			this.uiPanelMediaAgencysSearchContainer.Size = new System.Drawing.Size(1008, 39);
			this.uiPanelMediaAgencysSearchContainer.TabIndex = 0;
			// 
			// pnlSearch
			// 
			this.pnlSearch.BackColor = System.Drawing.SystemColors.Window;
			this.pnlSearch.Controls.Add(this.uiCheckBox1);
			this.pnlSearch.Controls.Add(this.ebSearchKey);
			this.pnlSearch.Controls.Add(this.cbSearchMediaName);
			this.pnlSearch.Controls.Add(this.cbSearchRapName);
			this.pnlSearch.Controls.Add(this.cbSearchMediaAgency);
			this.pnlSearch.Controls.Add(this.btnSearch);
			this.pnlSearch.Dock = System.Windows.Forms.DockStyle.Fill;
			this.pnlSearch.Location = new System.Drawing.Point(0, 0);
			this.pnlSearch.Name = "pnlSearch";
			this.pnlSearch.Size = new System.Drawing.Size(1008, 39);
			this.pnlSearch.TabIndex = 0;
			// 
			// uiCheckBox1
			// 
			this.uiCheckBox1.Location = new System.Drawing.Point(582, 6);
			this.uiCheckBox1.Name = "uiCheckBox1";
			this.uiCheckBox1.Size = new System.Drawing.Size(104, 23);
			this.uiCheckBox1.TabIndex = 7;
			this.uiCheckBox1.Text = "������ ����";
			// 
			// ebSearchKey
			// 
			this.ebSearchKey.Location = new System.Drawing.Point(416, 8);
			this.ebSearchKey.Name = "ebSearchKey";
			this.ebSearchKey.Size = new System.Drawing.Size(160, 20);
			this.ebSearchKey.TabIndex = 4;
			this.ebSearchKey.Text = "�˻�� �Է� �ϼ���";
			this.ebSearchKey.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
			this.ebSearchKey.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
			this.ebSearchKey.TextChanged += new System.EventHandler(this.ebSearchKey_TextChanged);
			this.ebSearchKey.Click += new System.EventHandler(this.ebSearchKey_Click);
			this.ebSearchKey.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ebSearchKey_KeyDown);
			// 
			// cbSearchMediaName
			// 
			this.cbSearchMediaName.BackColor = System.Drawing.Color.White;
			this.cbSearchMediaName.ComboStyle = Janus.Windows.EditControls.ComboStyle.DropDownList;
			this.cbSearchMediaName.Location = new System.Drawing.Point(8, 8);
			this.cbSearchMediaName.Name = "cbSearchMediaName";
			this.cbSearchMediaName.Size = new System.Drawing.Size(128, 20);
			this.cbSearchMediaName.TabIndex = 1;
			this.cbSearchMediaName.Text = "��ü����";
			this.cbSearchMediaName.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
			// 
			// cbSearchRapName
			// 
			this.cbSearchRapName.ComboStyle = Janus.Windows.EditControls.ComboStyle.DropDownList;
			this.cbSearchRapName.Location = new System.Drawing.Point(144, 8);
			this.cbSearchRapName.Name = "cbSearchRapName";
			this.cbSearchRapName.Size = new System.Drawing.Size(128, 20);
			this.cbSearchRapName.TabIndex = 2;
			this.cbSearchRapName.Text = "�̵�����";
			this.cbSearchRapName.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
			// 
			// cbSearchMediaAgency
			// 
			this.cbSearchMediaAgency.ComboStyle = Janus.Windows.EditControls.ComboStyle.DropDownList;
			this.cbSearchMediaAgency.Location = new System.Drawing.Point(280, 8);
			this.cbSearchMediaAgency.Name = "cbSearchMediaAgency";
			this.cbSearchMediaAgency.Size = new System.Drawing.Size(128, 20);
			this.cbSearchMediaAgency.TabIndex = 3;
			this.cbSearchMediaAgency.Text = "����缱��";
			this.cbSearchMediaAgency.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
			// 
			// btnSearch
			// 
			this.btnSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnSearch.Enabled = false;
			this.btnSearch.Location = new System.Drawing.Point(895, 8);
			this.btnSearch.Name = "btnSearch";
			this.btnSearch.Size = new System.Drawing.Size(104, 24);
			this.btnSearch.TabIndex = 6;
			this.btnSearch.Text = "�� ȸ";
			this.btnSearch.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
			this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
			// 
			// uiPanelMediaAgencyList
			// 
			this.uiPanelMediaAgencyList.BackColor = System.Drawing.SystemColors.Window;
			this.uiPanelMediaAgencyList.CaptionStyle = Janus.Windows.UI.Dock.PanelCaptionStyle.Dark;
			this.uiPanelMediaAgencyList.InnerContainer = this.uiPanelMediaAgencyListContainer;
			this.uiPanelMediaAgencyList.Location = new System.Drawing.Point(0, 67);
			this.uiPanelMediaAgencyList.MinimumSize = new System.Drawing.Size(-1, 100);
			this.uiPanelMediaAgencyList.Name = "uiPanelMediaAgencyList";
			this.uiPanelMediaAgencyList.Size = new System.Drawing.Size(1010, 398);
			this.uiPanelMediaAgencyList.TabIndex = 6;
			this.uiPanelMediaAgencyList.TabStop = false;
			this.uiPanelMediaAgencyList.Text = "�������";
			// 
			// uiPanelMediaAgencyListContainer
			// 
			this.uiPanelMediaAgencyListContainer.BackColor = System.Drawing.SystemColors.Window;
			this.uiPanelMediaAgencyListContainer.Controls.Add(this.grdExMediaAgencyList);
			this.uiPanelMediaAgencyListContainer.Location = new System.Drawing.Point(1, 23);
			this.uiPanelMediaAgencyListContainer.Name = "uiPanelMediaAgencyListContainer";
			this.uiPanelMediaAgencyListContainer.Size = new System.Drawing.Size(1008, 374);
			this.uiPanelMediaAgencyListContainer.TabIndex = 0;
			// 
			// grdExMediaAgencyList
			// 
			this.grdExMediaAgencyList.AllowEdit = Janus.Windows.GridEX.InheritableBoolean.False;
			this.grdExMediaAgencyList.AlternatingColors = true;
			this.grdExMediaAgencyList.BorderStyle = Janus.Windows.GridEX.BorderStyle.None;
			this.grdExMediaAgencyList.DataSource = this.dvMediaAgency;
			grdExMediaAgencyList_DesignTimeLayout.LayoutString = resources.GetString("grdExMediaAgencyList_DesignTimeLayout.LayoutString");
			this.grdExMediaAgencyList.DesignTimeLayout = grdExMediaAgencyList_DesignTimeLayout;
			this.grdExMediaAgencyList.Dock = System.Windows.Forms.DockStyle.Fill;
			this.grdExMediaAgencyList.EditorsControlStyle.ButtonAppearance = Janus.Windows.GridEX.ButtonAppearance.Regular;
			this.grdExMediaAgencyList.EmptyRows = true;
			this.grdExMediaAgencyList.FocusCellFormatStyle.BackColor = System.Drawing.SystemColors.Highlight;
			this.grdExMediaAgencyList.FocusCellFormatStyle.ForeColor = System.Drawing.Color.White;
			this.grdExMediaAgencyList.FocusStyle = Janus.Windows.GridEX.FocusStyle.None;
			this.grdExMediaAgencyList.FrozenColumns = 2;
			this.grdExMediaAgencyList.GridLineColor = System.Drawing.Color.Silver;
			this.grdExMediaAgencyList.GridLines = Janus.Windows.GridEX.GridLines.Vertical;
			this.grdExMediaAgencyList.GridLineStyle = Janus.Windows.GridEX.GridLineStyle.Solid;
			this.grdExMediaAgencyList.GroupByBoxVisible = false;
			this.grdExMediaAgencyList.HeaderFormatStyle.TextAlignment = Janus.Windows.GridEX.TextAlignment.Center;
			this.grdExMediaAgencyList.HideSelection = Janus.Windows.GridEX.HideSelection.HighlightInactive;
			this.grdExMediaAgencyList.Location = new System.Drawing.Point(0, 0);
			this.grdExMediaAgencyList.Name = "grdExMediaAgencyList";
			this.grdExMediaAgencyList.ScrollBars = Janus.Windows.GridEX.ScrollBars.Vertical;
			this.grdExMediaAgencyList.SelectedInactiveFormatStyle.BackColor = System.Drawing.Color.Gold;
			this.grdExMediaAgencyList.SelectedInactiveFormatStyle.ForeColor = System.Drawing.Color.Empty;
			this.grdExMediaAgencyList.Size = new System.Drawing.Size(1008, 374);
			this.grdExMediaAgencyList.TabIndex = 7;
			this.grdExMediaAgencyList.TabKeyBehavior = Janus.Windows.GridEX.TabKeyBehavior.ControlNavigation;
			this.grdExMediaAgencyList.ThemedAreas = ((Janus.Windows.GridEX.ThemedArea)(((((((((Janus.Windows.GridEX.ThemedArea.ScrollBars | Janus.Windows.GridEX.ThemedArea.EditControls) 
            | Janus.Windows.GridEX.ThemedArea.Headers) 
            | Janus.Windows.GridEX.ThemedArea.GroupByBox) 
            | Janus.Windows.GridEX.ThemedArea.GroupRows) 
            | Janus.Windows.GridEX.ThemedArea.ControlBorder) 
            | Janus.Windows.GridEX.ThemedArea.Cards) 
            | Janus.Windows.GridEX.ThemedArea.Gridlines) 
            | Janus.Windows.GridEX.ThemedArea.CheckBoxes)));
			this.grdExMediaAgencyList.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
			this.grdExMediaAgencyList.Enter += new System.EventHandler(this.OnGrdRowChanged);
			// 
			// dvMediaAgency
			// 
			this.dvMediaAgency.Table = this.mediaAgencyDs.MediaAgencys;
			// 
			// mediaAgencyDs
			// 
			this.mediaAgencyDs.DataSetName = "MediaAgencyDs";
			this.mediaAgencyDs.Locale = new System.Globalization.CultureInfo("en-US");
			this.mediaAgencyDs.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
			// 
			// uiPanelMediaAgencyDetail
			// 
			this.uiPanelMediaAgencyDetail.AllowResize = Janus.Windows.UI.InheritableBoolean.False;
			this.uiPanelMediaAgencyDetail.BackColor = System.Drawing.SystemColors.Window;
			this.uiPanelMediaAgencyDetail.CaptionStyle = Janus.Windows.UI.Dock.PanelCaptionStyle.Dark;
			this.uiPanelMediaAgencyDetail.InnerContainer = this.uiPanelMediaAgencyDetailContainer;
			this.uiPanelMediaAgencyDetail.Location = new System.Drawing.Point(0, 469);
			this.uiPanelMediaAgencyDetail.Name = "uiPanelMediaAgencyDetail";
			this.uiPanelMediaAgencyDetail.Size = new System.Drawing.Size(1010, 208);
			this.uiPanelMediaAgencyDetail.TabIndex = 8;
			this.uiPanelMediaAgencyDetail.TabStop = false;
			this.uiPanelMediaAgencyDetail.Text = "������";
			// 
			// uiPanelMediaAgencyDetailContainer
			// 
			this.uiPanelMediaAgencyDetailContainer.Controls.Add(this.pnlMediaAgencyDetail);
			this.uiPanelMediaAgencyDetailContainer.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.uiPanelMediaAgencyDetailContainer.Location = new System.Drawing.Point(1, 23);
			this.uiPanelMediaAgencyDetailContainer.Name = "uiPanelMediaAgencyDetailContainer";
			this.uiPanelMediaAgencyDetailContainer.Size = new System.Drawing.Size(1008, 184);
			this.uiPanelMediaAgencyDetailContainer.TabIndex = 0;
			// 
			// pnlMediaAgencyDetail
			// 
			this.pnlMediaAgencyDetail.BackColor = System.Drawing.SystemColors.Window;
			this.pnlMediaAgencyDetail.Controls.Add(this.ebContEndDay);
			this.pnlMediaAgencyDetail.Controls.Add(this.btnMediaSearch);
			this.pnlMediaAgencyDetail.Controls.Add(this.ebAgencyName);
			this.pnlMediaAgencyDetail.Controls.Add(this.ebRapName);
			this.pnlMediaAgencyDetail.Controls.Add(this.ebMediaName);
			this.pnlMediaAgencyDetail.Controls.Add(this.rbUseYn_N);
			this.pnlMediaAgencyDetail.Controls.Add(this.rbUseYn_Y);
			this.pnlMediaAgencyDetail.Controls.Add(this.lbUseYn);
			this.pnlMediaAgencyDetail.Controls.Add(this.lbEmail);
			this.pnlMediaAgencyDetail.Controls.Add(this.lbModDt);
			this.pnlMediaAgencyDetail.Controls.Add(this.lbTell);
			this.pnlMediaAgencyDetail.Controls.Add(this.ebTell);
			this.pnlMediaAgencyDetail.Controls.Add(this.ebCharger);
			this.pnlMediaAgencyDetail.Controls.Add(this.lbCharger);
			this.pnlMediaAgencyDetail.Controls.Add(this.lbRegDt);
			this.pnlMediaAgencyDetail.Controls.Add(this.ebRegDt);
			this.pnlMediaAgencyDetail.Controls.Add(this.ebModDt);
			this.pnlMediaAgencyDetail.Controls.Add(this.ebEmail);
			this.pnlMediaAgencyDetail.Controls.Add(this.lbContStarDay);
			this.pnlMediaAgencyDetail.Controls.Add(this.lbContEndDay);
			this.pnlMediaAgencyDetail.Controls.Add(this.lbRapName);
			this.pnlMediaAgencyDetail.Controls.Add(this.lbMediaName);
			this.pnlMediaAgencyDetail.Controls.Add(this.lbAgencyName);
			this.pnlMediaAgencyDetail.Controls.Add(this.lbRegID);
			this.pnlMediaAgencyDetail.Controls.Add(this.ebRegID);
			this.pnlMediaAgencyDetail.Controls.Add(this.btnAdd);
			this.pnlMediaAgencyDetail.Controls.Add(this.btnDelete);
			this.pnlMediaAgencyDetail.Controls.Add(this.btnSave);
			this.pnlMediaAgencyDetail.Controls.Add(this.btnRapSearch);
			this.pnlMediaAgencyDetail.Controls.Add(this.btnAgencySearch);
			this.pnlMediaAgencyDetail.Controls.Add(this.ebContStartDay);
			this.pnlMediaAgencyDetail.Dock = System.Windows.Forms.DockStyle.Fill;
			this.pnlMediaAgencyDetail.Location = new System.Drawing.Point(0, 0);
			this.pnlMediaAgencyDetail.Name = "pnlMediaAgencyDetail";
			this.pnlMediaAgencyDetail.Size = new System.Drawing.Size(1008, 184);
			this.pnlMediaAgencyDetail.TabIndex = 0;
			// 
			// ebContEndDay
			// 
			// 
			// 
			// 
			this.ebContEndDay.DropDownCalendar.FirstMonth = new System.DateTime(2007, 8, 1, 0, 0, 0, 0);
			this.ebContEndDay.DropDownCalendar.VisualStyle = Janus.Windows.CalendarCombo.VisualStyle.Office2007;
			this.ebContEndDay.Location = new System.Drawing.Point(280, 64);
			this.ebContEndDay.Name = "ebContEndDay";
			this.ebContEndDay.Size = new System.Drawing.Size(112, 20);
			this.ebContEndDay.TabIndex = 9;
			this.ebContEndDay.VisualStyle = Janus.Windows.CalendarCombo.VisualStyle.Office2007;
			// 
			// btnMediaSearch
			// 
			this.btnMediaSearch.Enabled = false;
			this.btnMediaSearch.Location = new System.Drawing.Point(80, 32);
			this.btnMediaSearch.Name = "btnMediaSearch";
			this.btnMediaSearch.Size = new System.Drawing.Size(104, 23);
			this.btnMediaSearch.TabIndex = 19;
			this.btnMediaSearch.Text = "��ü����";
			this.btnMediaSearch.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
			this.btnMediaSearch.Click += new System.EventHandler(this.btnMediaSearch_Click);
			// 
			// ebAgencyName
			// 
			this.ebAgencyName.Location = new System.Drawing.Point(656, 8);
			this.ebAgencyName.MaxLength = 20;
			this.ebAgencyName.Name = "ebAgencyName";
			this.ebAgencyName.Size = new System.Drawing.Size(184, 20);
			this.ebAgencyName.TabIndex = 0;
			this.ebAgencyName.TabStop = false;
			this.ebAgencyName.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
			this.ebAgencyName.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
			// 
			// ebRapName
			// 
			this.ebRapName.Location = new System.Drawing.Point(368, 8);
			this.ebRapName.MaxLength = 20;
			this.ebRapName.Name = "ebRapName";
			this.ebRapName.Size = new System.Drawing.Size(184, 20);
			this.ebRapName.TabIndex = 0;
			this.ebRapName.TabStop = false;
			this.ebRapName.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
			this.ebRapName.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
			// 
			// ebMediaName
			// 
			this.ebMediaName.Location = new System.Drawing.Point(80, 8);
			this.ebMediaName.MaxLength = 20;
			this.ebMediaName.Name = "ebMediaName";
			this.ebMediaName.Size = new System.Drawing.Size(184, 20);
			this.ebMediaName.TabIndex = 0;
			this.ebMediaName.TabStop = false;
			this.ebMediaName.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
			this.ebMediaName.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
			// 
			// rbUseYn_N
			// 
			this.rbUseYn_N.BackColor = System.Drawing.Color.Transparent;
			this.rbUseYn_N.Location = new System.Drawing.Point(728, 136);
			this.rbUseYn_N.Name = "rbUseYn_N";
			this.rbUseYn_N.Size = new System.Drawing.Size(72, 23);
			this.rbUseYn_N.TabIndex = 15;
			this.rbUseYn_N.Text = "������";
			this.rbUseYn_N.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
			// 
			// rbUseYn_Y
			// 
			this.rbUseYn_Y.BackColor = System.Drawing.Color.Transparent;
			this.rbUseYn_Y.Location = new System.Drawing.Point(656, 136);
			this.rbUseYn_Y.Name = "rbUseYn_Y";
			this.rbUseYn_Y.Size = new System.Drawing.Size(72, 23);
			this.rbUseYn_Y.TabIndex = 14;
			this.rbUseYn_Y.Text = "�����";
			this.rbUseYn_Y.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
			// 
			// lbUseYn
			// 
			this.lbUseYn.Location = new System.Drawing.Point(584, 136);
			this.lbUseYn.Name = "lbUseYn";
			this.lbUseYn.Size = new System.Drawing.Size(72, 21);
			this.lbUseYn.TabIndex = 39;
			this.lbUseYn.Text = "��뿩��";
			this.lbUseYn.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// lbEmail
			// 
			this.lbEmail.Location = new System.Drawing.Point(8, 112);
			this.lbEmail.Name = "lbEmail";
			this.lbEmail.Size = new System.Drawing.Size(72, 21);
			this.lbEmail.TabIndex = 38;
			this.lbEmail.Text = "Email";
			this.lbEmail.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// lbModDt
			// 
			this.lbModDt.Location = new System.Drawing.Point(584, 88);
			this.lbModDt.Name = "lbModDt";
			this.lbModDt.Size = new System.Drawing.Size(72, 21);
			this.lbModDt.TabIndex = 37;
			this.lbModDt.Text = "�����Ͻ�";
			this.lbModDt.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// lbTell
			// 
			this.lbTell.Location = new System.Drawing.Point(208, 88);
			this.lbTell.Name = "lbTell";
			this.lbTell.Size = new System.Drawing.Size(72, 21);
			this.lbTell.TabIndex = 30;
			this.lbTell.Text = "��ȭ��ȣ";
			this.lbTell.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// ebTell
			// 
			this.ebTell.Location = new System.Drawing.Point(280, 88);
			this.ebTell.MaxLength = 15;
			this.ebTell.Name = "ebTell";
			this.ebTell.Size = new System.Drawing.Size(112, 20);
			this.ebTell.TabIndex = 12;
			this.ebTell.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
			this.ebTell.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
			// 
			// ebCharger
			// 
			this.ebCharger.Location = new System.Drawing.Point(80, 88);
			this.ebCharger.MaxLength = 20;
			this.ebCharger.Name = "ebCharger";
			this.ebCharger.Size = new System.Drawing.Size(112, 20);
			this.ebCharger.TabIndex = 11;
			this.ebCharger.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
			this.ebCharger.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
			// 
			// lbCharger
			// 
			this.lbCharger.Location = new System.Drawing.Point(8, 88);
			this.lbCharger.Name = "lbCharger";
			this.lbCharger.Size = new System.Drawing.Size(72, 21);
			this.lbCharger.TabIndex = 19;
			this.lbCharger.Text = "����ڸ�";
			this.lbCharger.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// lbRegDt
			// 
			this.lbRegDt.Location = new System.Drawing.Point(584, 64);
			this.lbRegDt.Name = "lbRegDt";
			this.lbRegDt.Size = new System.Drawing.Size(72, 21);
			this.lbRegDt.TabIndex = 29;
			this.lbRegDt.Text = "����Ͻ�";
			this.lbRegDt.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// ebRegDt
			// 
			this.ebRegDt.BackColor = System.Drawing.Color.WhiteSmoke;
			this.ebRegDt.Location = new System.Drawing.Point(656, 64);
			this.ebRegDt.Name = "ebRegDt";
			this.ebRegDt.ReadOnly = true;
			this.ebRegDt.Size = new System.Drawing.Size(184, 20);
			this.ebRegDt.TabIndex = 0;
			this.ebRegDt.TabStop = false;
			this.ebRegDt.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
			this.ebRegDt.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
			// 
			// ebModDt
			// 
			this.ebModDt.BackColor = System.Drawing.Color.WhiteSmoke;
			this.ebModDt.Location = new System.Drawing.Point(656, 88);
			this.ebModDt.Name = "ebModDt";
			this.ebModDt.ReadOnly = true;
			this.ebModDt.Size = new System.Drawing.Size(184, 20);
			this.ebModDt.TabIndex = 0;
			this.ebModDt.TabStop = false;
			this.ebModDt.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
			this.ebModDt.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
			// 
			// ebEmail
			// 
			this.ebEmail.Location = new System.Drawing.Point(80, 112);
			this.ebEmail.MaxLength = 20;
			this.ebEmail.Name = "ebEmail";
			this.ebEmail.Size = new System.Drawing.Size(312, 20);
			this.ebEmail.TabIndex = 13;
			this.ebEmail.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
			this.ebEmail.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
			// 
			// lbContStarDay
			// 
			this.lbContStarDay.Location = new System.Drawing.Point(8, 64);
			this.lbContStarDay.Name = "lbContStarDay";
			this.lbContStarDay.Size = new System.Drawing.Size(72, 21);
			this.lbContStarDay.TabIndex = 29;
			this.lbContStarDay.Text = "���������";
			this.lbContStarDay.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// lbContEndDay
			// 
			this.lbContEndDay.Location = new System.Drawing.Point(208, 64);
			this.lbContEndDay.Name = "lbContEndDay";
			this.lbContEndDay.Size = new System.Drawing.Size(72, 21);
			this.lbContEndDay.TabIndex = 37;
			this.lbContEndDay.Text = "����������";
			this.lbContEndDay.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// lbRapName
			// 
			this.lbRapName.Location = new System.Drawing.Point(296, 8);
			this.lbRapName.Name = "lbRapName";
			this.lbRapName.Size = new System.Drawing.Size(72, 21);
			this.lbRapName.TabIndex = 30;
			this.lbRapName.Text = "�̵�";
			this.lbRapName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// lbMediaName
			// 
			this.lbMediaName.Location = new System.Drawing.Point(8, 8);
			this.lbMediaName.Name = "lbMediaName";
			this.lbMediaName.Size = new System.Drawing.Size(72, 21);
			this.lbMediaName.TabIndex = 19;
			this.lbMediaName.Text = "��ü";
			this.lbMediaName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// lbAgencyName
			// 
			this.lbAgencyName.Location = new System.Drawing.Point(584, 8);
			this.lbAgencyName.Name = "lbAgencyName";
			this.lbAgencyName.Size = new System.Drawing.Size(72, 21);
			this.lbAgencyName.TabIndex = 19;
			this.lbAgencyName.Text = "�����";
			this.lbAgencyName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// lbRegID
			// 
			this.lbRegID.Location = new System.Drawing.Point(584, 112);
			this.lbRegID.Name = "lbRegID";
			this.lbRegID.Size = new System.Drawing.Size(72, 21);
			this.lbRegID.TabIndex = 37;
			this.lbRegID.Text = "�����";
			this.lbRegID.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// ebRegID
			// 
			this.ebRegID.BackColor = System.Drawing.Color.WhiteSmoke;
			this.ebRegID.Location = new System.Drawing.Point(656, 112);
			this.ebRegID.Name = "ebRegID";
			this.ebRegID.ReadOnly = true;
			this.ebRegID.Size = new System.Drawing.Size(184, 20);
			this.ebRegID.TabIndex = 0;
			this.ebRegID.TabStop = false;
			this.ebRegID.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
			this.ebRegID.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
			// 
			// btnAdd
			// 
			this.btnAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.btnAdd.Enabled = false;
			this.btnAdd.Location = new System.Drawing.Point(232, 150);
			this.btnAdd.Name = "btnAdd";
			this.btnAdd.Size = new System.Drawing.Size(104, 24);
			this.btnAdd.TabIndex = 18;
			this.btnAdd.Text = "�� ��";
			this.btnAdd.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
			this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
			// 
			// btnDelete
			// 
			this.btnDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.btnDelete.Enabled = false;
			this.btnDelete.Location = new System.Drawing.Point(120, 150);
			this.btnDelete.Name = "btnDelete";
			this.btnDelete.Size = new System.Drawing.Size(104, 24);
			this.btnDelete.TabIndex = 17;
			this.btnDelete.Text = "�� ��";
			this.btnDelete.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
			this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
			// 
			// btnSave
			// 
			this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.btnSave.Enabled = false;
			this.btnSave.Location = new System.Drawing.Point(8, 150);
			this.btnSave.Name = "btnSave";
			this.btnSave.Size = new System.Drawing.Size(104, 24);
			this.btnSave.TabIndex = 16;
			this.btnSave.Text = "�� ��";
			this.btnSave.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
			this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
			// 
			// btnRapSearch
			// 
			this.btnRapSearch.Enabled = false;
			this.btnRapSearch.Location = new System.Drawing.Point(368, 32);
			this.btnRapSearch.Name = "btnRapSearch";
			this.btnRapSearch.Size = new System.Drawing.Size(104, 23);
			this.btnRapSearch.TabIndex = 20;
			this.btnRapSearch.Text = "�̵�����";
			this.btnRapSearch.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
			this.btnRapSearch.Click += new System.EventHandler(this.btnRapSearch_Click);
			// 
			// btnAgencySearch
			// 
			this.btnAgencySearch.Enabled = false;
			this.btnAgencySearch.Location = new System.Drawing.Point(656, 32);
			this.btnAgencySearch.Name = "btnAgencySearch";
			this.btnAgencySearch.Size = new System.Drawing.Size(104, 23);
			this.btnAgencySearch.TabIndex = 21;
			this.btnAgencySearch.Text = "����缱��";
			this.btnAgencySearch.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
			this.btnAgencySearch.Click += new System.EventHandler(this.btnAgencySearch_Click);
			// 
			// ebContStartDay
			// 
			// 
			// 
			// 
			this.ebContStartDay.DropDownCalendar.FirstMonth = new System.DateTime(2007, 8, 1, 0, 0, 0, 0);
			this.ebContStartDay.DropDownCalendar.VisualStyle = Janus.Windows.CalendarCombo.VisualStyle.Office2007;
			this.ebContStartDay.Location = new System.Drawing.Point(80, 64);
			this.ebContStartDay.Name = "ebContStartDay";
			this.ebContStartDay.Size = new System.Drawing.Size(112, 20);
			this.ebContStartDay.TabIndex = 8;
			this.ebContStartDay.VisualStyle = Janus.Windows.CalendarCombo.VisualStyle.Office2007;
			// 
			// uiComboBox1
			// 
			this.uiComboBox1.Location = new System.Drawing.Point(0, 0);
			this.uiComboBox1.Name = "uiComboBox1";
			this.uiComboBox1.Size = new System.Drawing.Size(176, 21);
			this.uiComboBox1.TabIndex = 0;
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
			// MediaAgencyControl
			// 
			this.Controls.Add(this.uiPanelMediaAgencys);
			this.Font = global::AdManagerClient.Properties.Settings.Default.Font1;
			this.Name = "MediaAgencyControl";
			this.Size = new System.Drawing.Size(1010, 677);
			this.Load += new System.EventHandler(this.MediaAgencyControl_Load);
			((System.ComponentModel.ISupportInitialize)(this.uiPM)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.uiPanelMediaAgencys)).EndInit();
			this.uiPanelMediaAgencys.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.uiPanelMediaAgencysSearch)).EndInit();
			this.uiPanelMediaAgencysSearch.ResumeLayout(false);
			this.uiPanelMediaAgencysSearchContainer.ResumeLayout(false);
			this.pnlSearch.ResumeLayout(false);
			this.pnlSearch.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.uiPanelMediaAgencyList)).EndInit();
			this.uiPanelMediaAgencyList.ResumeLayout(false);
			this.uiPanelMediaAgencyListContainer.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.grdExMediaAgencyList)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.dvMediaAgency)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.mediaAgencyDs)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.uiPanelMediaAgencyDetail)).EndInit();
			this.uiPanelMediaAgencyDetail.ResumeLayout(false);
			this.uiPanelMediaAgencyDetailContainer.ResumeLayout(false);
			this.pnlMediaAgencyDetail.ResumeLayout(false);
			this.pnlMediaAgencyDetail.PerformLayout();
			this.ResumeLayout(false);

		}
		#endregion

		#region ��Ʈ�� �ε�
		private void MediaAgencyControl_Load(object sender, System.EventArgs e)
		{
			// �����Ͱ����� ��ü����
			dt = ((DataView)grdExMediaAgencyList.DataSource).Table;  
			cm = (CurrencyManager) this.BindingContext[grdExMediaAgencyList.DataSource]; 
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
			InitCombo_Rap();	
			InitCombo_Agency();
			InitCombo_Level();

//			InitCombo_Insert();
//			InitCombo_Rap_Insert();	
//			InitCombo_Agency_Insert();

			// ��ȸ���� �˻�
			if(menu.CanRead(MenuCode))
			{
				canRead = true;
				SearchMediaAgency();				
			}
			
			// �߰���ư Ȱ��ȭ
			if(menu.CanCreate(MenuCode))
			{
				canCreate = true;
			}

			// ������ư Ȱ��ȭ
			if(menu.CanDelete(MenuCode))
			{
				canDelete = true;
			}

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

			InitButton();
            ProgressStop();
		}

		private void InitCombo()
		{
			// �ڵ忡�� ���ȷ����� ��ȸ�Ѵ�.
			MediaCodeModel mediacodeModel = new MediaCodeModel();
			//MediacodeModel.Section = "21";				// �ڵ�з� '11':���ȷ���  TODO: �ڵ�з��� ���� XML�� �����Ǿ��...
			new MediaCodeManager(systemModel, commonModel).GetMediaCodeList(mediacodeModel);
			
			if (mediacodeModel.ResultCD.Equals("0000"))
			{
				// �����ͼ¿� ����
				Utility.SetDataTable(mediaAgencyDs.Medias, mediacodeModel.MediaCodeDataSet);				
			}

			// ����ȸ �޺�
			// �������� �޺��� Dataset�� �����ͼҽ��� ������.

			// �˻������� �޺�
			this.cbSearchMediaName.Items.Clear();
			
			// �޺��ڽ��� ��Ʈ�� �ڵ����� ���� Item�迭�� ����
			Janus.Windows.EditControls.UIComboBoxItem[] comboItems = new Janus.Windows.EditControls.UIComboBoxItem[mediacodeModel.ResultCnt + 1];

			comboItems[0] = new Janus.Windows.EditControls.UIComboBoxItem("��ü����","00");
			
			for(int i=0;i<mediacodeModel.ResultCnt;i++)
			{
				DataRow row = mediaAgencyDs.Medias.Rows[i];

				string val = row["MediaCode"].ToString();
				string txt = row["MediaName"].ToString();
				comboItems[i+1] = new Janus.Windows.EditControls.UIComboBoxItem(txt,val);
			}
			// �޺��� ��Ʈ
			this.cbSearchMediaName.Items.AddRange(comboItems);
			this.cbSearchMediaName.SelectedIndex = 0;
			Application.DoEvents();
		}

		private void InitCombo_Rap()
		{
            /* ����� ���� - ����� 
			// �ڵ忡�� ���ȷ����� ��ȸ�Ѵ�.
			MediaRapCodeModel mediarapcodeModel = new MediaRapCodeModel();
			//codeModel.Section = "21";				// �ڵ�з� '11':���ȷ���  TODO: �ڵ�з��� ���� XML�� �����Ǿ��...
			new MediaRapCodeManager(systemModel, commonModel).GetMediaRapCodeList(mediarapcodeModel);
			
			if (mediarapcodeModel.ResultCD.Equals("0000"))
			{
				// �����ͼ¿� ����
				Utility.SetDataTable(mediaAgencyDs.MediaRaps, mediarapcodeModel.MediaRapCodeDataSet);				
			}

			// ����ȸ �޺�
			// �������� �޺��� Dataset�� �����ͼҽ��� ������.

			// �˻������� �޺�
			this.cbSearchRapName.Items.Clear();
			
			// �޺��ڽ��� ��Ʈ�� �ڵ����� ���� Item�迭�� ����
			Janus.Windows.EditControls.UIComboBoxItem[] comboItems = new Janus.Windows.EditControls.UIComboBoxItem[mediarapcodeModel.ResultCnt + 1];

			comboItems[0] = new Janus.Windows.EditControls.UIComboBoxItem("�̵�����","00");
			
			for(int i=0;i<mediarapcodeModel.ResultCnt;i++)
			{
				DataRow row = mediaAgencyDs.MediaRaps.Rows[i];

				string val = row["RapCode"].ToString();
				string txt = row["RapName"].ToString();
				comboItems[i+1] = new Janus.Windows.EditControls.UIComboBoxItem(txt,val);
			}
			// �޺��� ��Ʈ
			this.cbSearchRapName.Items.AddRange(comboItems);
			this.cbSearchRapName.SelectedIndex = 0;
			Application.DoEvents();
            */
            this.cbSearchRapName.Items.Clear();
            DataSet ds = new DataSet("rapset");
            DataTable dt = new DataTable("rap");
            dt.Columns.Add("RapCode", typeof(string));
            dt.Columns.Add("RapName", typeof(string));
            DataRow nRow = dt.NewRow();
            nRow["RapCode"] = "1";
            nRow["RapName"] = "����� ����";
            dt.Rows.Add(nRow);
            ds.Tables.Add(dt);

            Utility.SetDataTable(mediaAgencyDs.MediaRap, ds);
            // �˻������� �޺�
            this.cbSearchRapName.Items.Clear();
            // �޺��ڽ��� ��Ʈ�� �ڵ����� ���� Item�迭�� ����
            Janus.Windows.EditControls.UIComboBoxItem[] comboItems = new Janus.Windows.EditControls.UIComboBoxItem[2];

            comboItems[0] = new Janus.Windows.EditControls.UIComboBoxItem("�̵�����", "00");

            for (int i = 0; i < 1; i++)
            {
                DataRow row = mediaAgencyDs.MediaRap.Rows[i];

                string val = row["RapCode"].ToString();
                string txt = row["RapName"].ToString();
                comboItems[i + 1] = new Janus.Windows.EditControls.UIComboBoxItem(txt, val);
            }
            // �޺��� ��Ʈ
            this.cbSearchRapName.Items.AddRange(comboItems);
            this.cbSearchRapName.SelectedIndex = 1;
            this.cbSearchRapName.ReadOnly = true;
            Application.DoEvents();
		}

		private void InitCombo_Agency()
		{
			// �ڵ忡�� ���ȷ����� ��ȸ�Ѵ�.
			AgencyCodeModel agencycodeModel = new AgencyCodeModel();
			//codeModel.Section = "21";				// �ڵ�з� '11':���ȷ���  TODO: �ڵ�з��� ���� XML�� �����Ǿ��...
			new AgencyCodeManager(systemModel, commonModel).GetAgencyCodeList(agencycodeModel);
			
			if (agencycodeModel.ResultCD.Equals("0000"))
			{
				// �����ͼ¿� ����
				Utility.SetDataTable(mediaAgencyDs.Agencys, agencycodeModel.AgencyCodeDataSet);				
			}

			// ����ȸ �޺�
			// �������� �޺��� Dataset�� �����ͼҽ��� ������.

			// �˻������� �޺�
			this.cbSearchMediaAgency.Items.Clear();
			
			// �޺��ڽ��� ��Ʈ�� �ڵ����� ���� Item�迭�� ����
			Janus.Windows.EditControls.UIComboBoxItem[] comboItems = new Janus.Windows.EditControls.UIComboBoxItem[agencycodeModel.ResultCnt + 1];

			comboItems[0] = new Janus.Windows.EditControls.UIComboBoxItem("����缱��","00");
			
			for(int i=0;i<agencycodeModel.ResultCnt;i++)
			{
				DataRow row = mediaAgencyDs.Agencys.Rows[i];

				string val = row["AgencyCode"].ToString();
				string txt = row["AgencyName"].ToString();
				comboItems[i+1] = new Janus.Windows.EditControls.UIComboBoxItem(txt,val);
			}
			
			// �޺��� ��Ʈ
			this.cbSearchMediaAgency.Items.AddRange(comboItems);
			this.cbSearchMediaAgency.SelectedIndex = 0;
			Application.DoEvents();
		}

		private void InitCombo_Level()
		{

            if(commonModel.UserLevel=="20")
            {
                cbSearchMediaName.SelectedValue = commonModel.MediaCode;							
                cbSearchMediaName.ReadOnly = true;					
            }
            else
            {
				for(int i=0;i < mediaAgencyDs.Medias.Rows.Count;i++)
				{
					DataRow row = mediaAgencyDs.Medias.Rows[i];					
					if(row["MediaCode"].ToString().Equals(FrameSystem._HANATV.ToString()))
					{
						cbSearchMediaName.SelectedValue = FrameSystem._HANATV; // �ϳ�TV�� �⺻������ �Ѵ�.	 		
						break;															
					}
					else
					{
						cbSearchMediaName.SelectedValue="00";
					}
				}				
            }

			if(commonModel.UserLevel=="30")
			{
				cbSearchRapName.SelectedValue = commonModel.RapCode;				
				cbSearchRapName.ReadOnly = true;				
			}
			if(commonModel.UserLevel=="40")
			{
				cbSearchMediaAgency.SelectedValue = commonModel.AgencyCode;						
				cbSearchMediaAgency.ReadOnly = true;					
			}		
			Application.DoEvents();	
		}	
	

		private void InitButton()
		{
			if(canRead)   btnSearch.Enabled = true;
			if(canCreate) btnAdd.Enabled    = true;

			if(ebAgencyName.Text.Trim().Length > 0) 
			{
				if(canDelete) btnDelete.Enabled = true;
				if(canUpdate) btnSave.Enabled   = true;
			}
			Application.DoEvents();
		}

		private void DisableButton()
		{
			btnSearch.Enabled = false;
			btnAdd.Enabled    = false;
			btnSave.Enabled   = false;
			btnDelete.Enabled = false;
			Application.DoEvents();
		}

		#endregion

		#region ��ü����� �׼�ó�� �޼ҵ�

		/// <summary>
		/// �׸����� Row�����
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void OnGrdRowChanged(object sender, System.EventArgs e) 
		{
            if (!IsSearching) // 2011.11.29 JH.Park ��ȸ���� �ƴҰ�쿡�� �����ϵ��� ����
            {
                if (grdExMediaAgencyList.RecordCount > 0)
                {
                    SetMediaAgencyDetailText();
                }
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
			SearchMediaAgency();
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
			btnAdd.Enabled    = false;
			btnDelete.Enabled = false;
			btnSave.Enabled   = true;

			IsAdding = true;			
			ResetTextReadonly();
			ResetMediaAgencyDetailText();
			
			ebCharger.Focus();
		}

		/// <summary>
		/// �����ư Ŭ��
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnSave_Click(object sender, System.EventArgs e)
		{			
			SaveMediaAgencyDetail();			
		}

		/// <summary>
		/// ������ư Ŭ��
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnDelete_Click(object sender, System.EventArgs e)
		{			
			DeleteMediaAgency();			
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
				SearchMediaAgency();
			}
		}

		private void btnContStartDay_Click(object sender, System.EventArgs e)
		{
			CalendarForm calendarForm = new CalendarForm(this,"Start");
			calendarForm.ShowDialog(this);
			calendarForm.Dispose();
			calendarForm = null;			
		}

		private void btnContEndDay_Click(object sender, System.EventArgs e)
		{
			CalendarForm calendarForm = new CalendarForm(this,"End");
			calendarForm.ShowDialog(this);
			calendarForm.Dispose();
			calendarForm = null;			
		}		

		public string SetYear1
		{				
			set
			{
				this.ebContStartDay.Text = value;
			}			
		}

		public string SetYear2
		{				
			set
			{
				this.ebContEndDay.Text = value;
			}			
		}

		#endregion

		#region ó���޼ҵ�

		/// <summary>
		/// ��ü������� ��ȸ
		/// </summary>
		private void SearchMediaAgency()
		{
            IsSearching = true;

			StatusMessage("��ü����� ������ ��ȸ�մϴ�.");

			try
			{
				mediaAgencyModel.Init();
				// �����͸𵨿� ������ ������ ��Ʈ�Ѵ�.
				if(IsNewSearchKey)
				{
					mediaAgencyModel.SearchKey = "";
				}
				else
				{
					mediaAgencyModel.SearchKey  = ebSearchKey.Text;
				}

				mediaAgencyModel.SearchMediaName = cbSearchMediaName.SelectedItem.Value.ToString();
				mediaAgencyModel.SearchRapName = cbSearchRapName.SelectedItem.Value.ToString();
				mediaAgencyModel.SearchMediaAgency = cbSearchMediaAgency.SelectedItem.Value.ToString();

                if (uiCheckBox1.Checked)
				{
					mediaAgencyModel.SearchchkAdState_10   = "Y";
				}
				else
				{
					mediaAgencyModel.SearchchkAdState_10   = "N";
				}

				// ��ü���������ȸ ���񽺸� ȣ���Ѵ�.
				new MediaAgencyManager(systemModel,commonModel).GetMediaAgencyList(mediaAgencyModel);
				
				if (mediaAgencyModel.ResultCD.Equals("0000"))
				{
					Utility.SetDataTable(mediaAgencyDs.MediaAgencys, mediaAgencyModel.MediaAgencyDataSet);				
					StatusMessage(mediaAgencyModel.ResultCnt + "���� ��ü����� ������ ��ȸ�Ǿ����ϴ�.");
					if(canUpdate)
					{
						AddSchChoice();										
					}										
					SetMediaAgencyDetailText();					
				}
			}
			catch(FrameException fe)
			{
				FrameSystem.showMsgForm("��ü�������ȸ����", new string[] {fe.ErrCode, fe.ResultMsg});
			}
			catch(Exception ex)
			{
				FrameSystem.showMsgForm("��ü�������ȸ����",new string[] {"",ex.Message});
			}
            finally
            {
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
				if ( mediaAgencyDs.Tables["MediaAgencys"].Rows.Count < 1 ) return;
              
				foreach (DataRow row in mediaAgencyDs.Tables["MediaAgencys"].Rows)
				{					
					if(IsAdding)
					{
						cm.Position = 0;
						mediaCode = null;
						rapCode = null;
						agencyCode = null;						
					}
					else
					{						
						if(row["MediaCode"].ToString().Equals(mediaCode) && row["RapCode"].ToString().Equals(rapCode) && row["AgencyCode"].ToString().Equals(agencyCode))
						{					
							cm.Position = rowIndex;
							break;								
						}
					}

					rowIndex++;
					grdExMediaAgencyList.EnsureVisible();
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
		/// ��ü���������� ����
		/// </summary>
		private void SaveMediaAgencyDetail()
		{
			StatusMessage("��ü����� ������ �����մϴ�.");
			
			if(mediaCode.Length == 0) 
			{
				MessageBox.Show("��ü�� �Էµ��� �ʾҽ��ϴ�.","��ü����� ����", 
					MessageBoxButtons.OK, MessageBoxIcon.Information );
					ResetPop("media");
				return;								
			}

			if(rapCode.Length == 0) 
			{
				MessageBox.Show("���簡 �Էµ��� �ʾҽ��ϴ�.","��ü����� ����", 
					MessageBoxButtons.OK, MessageBoxIcon.Information );
					ResetPop("rap");
				return;								
			}
			
			if(agencyCode.Length == 0) 
			{
				MessageBox.Show("����簡 �Էµ��� �ʾҽ��ϴ�.","��ü����� ����", 
					MessageBoxButtons.OK, MessageBoxIcon.Information );
					ResetPop("agency");
				return;								
			}

			try
			{
                if (IsAdding) // ���� �� ���� ����
                {
                    //��Ʈ���̺�..�������� ��û���� ���� ��ü������ �������� ���Ϲ����� ���� ���� ������ �׷��� ���� ���
                    //DB���� PK������ ����.
                    //�޽����� ó��..08.04.16	�ں���
                    foreach (DataRow row in mediaAgencyDs.Tables["MediaAgencys"].Rows)
                    {
                        if (row["MediaCode"].ToString().Equals(mediaCode))
                        {
                            if (row["RapCode"].ToString().Equals(rapCode))
                            {
                                if (row["AgencyCode"].ToString().Equals(agencyCode))
                                {
                                    MessageBox.Show("���� ��ü�� ����, ����簡 �ֽ��ϴ�. Ȯ���� �ּ���!!", "��ü����� ����",
                                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    return;
                                }
                            }
                        }
                    }
                }

				// �����͸𵨿� ������ ������ ��Ʈ�Ѵ�.
				mediaAgencyModel.MediaCode       = mediaCode;
				mediaAgencyModel.RapCode       = rapCode;
				mediaAgencyModel.AgencyCode       = agencyCode;
				mediaAgencyModel.Charger       = ebCharger.Text.Trim();				
				mediaAgencyModel.ContStartDay     = ebContStartDay.Value.ToString("yyyyMMdd");
				mediaAgencyModel.ContEndDay    = ebContEndDay.Value.ToString("yyyyMMdd");	
				mediaAgencyModel.Tell     = ebTell.Text;				
				mediaAgencyModel.Email  = ebEmail.Text;

				//��뿩��
				if(rbUseYn_Y.Checked)
				{
					mediaAgencyModel.UseYn       = "Y";
				}
				else
				{
					mediaAgencyModel.UseYn       = "N";
				}
				
				// ��ü����� ������ ���� ���񽺸� ȣ���Ѵ�.
				if (IsAdding)
				{
					new MediaAgencyManager(systemModel,commonModel).SetMediaAgencyAdd(mediaAgencyModel);
					StatusMessage("��ü����� ������ �߰��Ǿ����ϴ�.");
					IsAdding = false;
					ResetMediaAgencyDetailText();
				}
				else
				{
					new MediaAgencyManager(systemModel,commonModel).SetMediaAgencyUpdate(mediaAgencyModel);
					StatusMessage("��ü����� ������ ����Ǿ����ϴ�.");
				}
				
				DisableButton();
				SearchMediaAgency();
				InitButton();
			}
			catch(FrameException fe)
			{
				FrameSystem.showMsgForm("��ü��������� �������", new string[] {fe.ErrCode, fe.ResultMsg});
			}
			catch(Exception ex)
			{
				FrameSystem.showMsgForm("��ü��������� �������",new string[] {"",ex.Message});
			}			
		}

		/// <summary>
		/// ��ü��������� ����
		/// </summary>
		private void DeleteMediaAgency()
		{
			StatusMessage("��ü����� ������ �����մϴ�.");

			DialogResult result = MessageBox.Show("�ش� ��ü����� ������ ���� �Ͻðڽ��ϱ�?","��ü����� ����",
				MessageBoxButtons.YesNo, MessageBoxIcon.Question,
				MessageBoxDefaultButton.Button2);

			if (result == DialogResult.No) return;

			try 
			{
				// �����͸𵨿� ������ ������ ��Ʈ�Ѵ�.
				mediaAgencyModel.MediaCode       = mediaCode;
				mediaAgencyModel.RapCode       = rapCode;
				mediaAgencyModel.AgencyCode       = agencyCode;

				// ��ü����� ������ ���� ���񽺸� ȣ���Ѵ�.
				new MediaAgencyManager(systemModel,commonModel).SetMediaAgencyDelete(mediaAgencyModel);

				StatusMessage("��ü����� ������ �����Ǿ����ϴ�.");	
				ResetMediaAgencyDetailText();					

				DisableButton();
				SearchMediaAgency();
				InitButton();

			}
			catch(FrameException fe)
			{
				FrameSystem.showMsgForm("��ü��������� ��������", new string[] {fe.ErrCode, fe.ResultMsg});
			}
			catch(Exception ex)
			{
				FrameSystem.showMsgForm("��ü��������� ��������",new string[] {"",ex.Message});
			}			

		}

		/// <summary>
		/// ��ü����� �������� ��Ʈ
		/// </summary>
		private void SetMediaAgencyDetailText()
		{
			int curRow = cm.Position;
			if(curRow >= 0)
			{				
				mediaCode	=	dt.Rows[curRow]["MediaCode"].ToString();			
				rapCode	=	dt.Rows[curRow]["RapCode"].ToString();			
				agencyCode	=	dt.Rows[curRow]["AgencyCode"].ToString();			
				ebMediaName.Text        = dt.Rows[curRow]["MediaName"].ToString();			
				ebRapName.Text           = dt.Rows[curRow]["RapName"].ToString();			
				ebAgencyName.Text      = dt.Rows[curRow]["AgencyName"].ToString();			
				ebCharger.Text             = dt.Rows[curRow]["Charger"].ToString();			
				//ebContStartDay.Text           = dt.Rows[curRow]["ContStartDay"].ToString();
				string ContStartDay = dt.Rows[cm.Position]["ContStartDay"].ToString();
				if(ContStartDay.Length == 8)
				{
					int yyyy = Convert.ToInt32(ContStartDay.Substring(0, 4));
					int mm   = Convert.ToInt32(ContStartDay.Substring(4, 2));
					int dd   = Convert.ToInt32(ContStartDay.Substring(6, 2));
					ebContStartDay.Value = new DateTime(yyyy,mm,dd);	
				}
				else
				{
					ebContStartDay.Value = DateTime.Now;	
				}
				//ebContEndDay.Text           = dt.Rows[curRow]["ContEndDay"].ToString();
				string ContEndDay = dt.Rows[cm.Position]["ContEndDay"].ToString();
				if(ContEndDay.Length == 8)
				{
					int yyyy = Convert.ToInt32(ContEndDay.Substring(0, 4));
					int mm   = Convert.ToInt32(ContEndDay.Substring(4, 2));
					int dd   = Convert.ToInt32(ContEndDay.Substring(6, 2));
					ebContEndDay.Value = new DateTime(yyyy,mm,dd);	
				}
				else
				{
					ebContEndDay.Value = DateTime.Now;	
				}
				ebTell.Text           = dt.Rows[curRow]["Tell"].ToString();			
				ebEmail.Text           = dt.Rows[curRow]["Email"].ToString();			
				ebRegDt.Text              = dt.Rows[curRow]["RegDt"].ToString();
				ebModDt.Text             = dt.Rows[curRow]["ModDt"].ToString();
				ebRegID.Text             = dt.Rows[curRow]["RegName"].ToString();
				string UseYn              = dt.Rows[curRow]["UseYn"].ToString();

				if(UseYn.Equals("Y"))
				{
					rbUseYn_Y.Checked = true;
					rbUseYn_N.Checked = false;
				}
				else
				{
					rbUseYn_Y.Checked = false;
					rbUseYn_N.Checked = true;
				}

				IsAdding = false;
				ResetTextReadonly();
			}
			StatusMessage("�غ�");
		}

		private void ResetMediaAgencyDetailText()
		{
			mediaCode             = "";			
			rapCode             = "";			
			agencyCode             = "";			
			ebMediaName.Text             = "";			
			ebRapName.Text             = "";			
			ebAgencyName.Text             = "";			
			ebCharger.Text             = "";			
			ebContStartDay.Value = DateTime.Now;	
			ebContEndDay.Value = DateTime.Now;	
			ebTell.Text           = "";			
			ebEmail.Text           = "";					
			ebRegDt.Text              = "";
			ebModDt.Text				= "";
			ebRegID.Text				= "";
			
			if(!IsAdding)
			{
				ebMediaName.ReadOnly         = true;			
				ebRapName.ReadOnly         = true;			
				ebAgencyName.ReadOnly         = true;						
				ebCharger.ReadOnly         = false;			
				ebContStartDay.ReadOnly       = false;
				ebContEndDay.ReadOnly      = false;			
				ebTell.ReadOnly       = false;
				ebEmail.ReadOnly       = false;
				rbUseYn_Y.Enabled			= true;
				rbUseYn_N.Enabled			= true;
						
				ebMediaName.BackColor        = Color.White;			
				ebRapName.BackColor        = Color.White;			
				ebAgencyName.BackColor        = Color.White;			
				ebCharger.BackColor        = Color.White;			
				ebContStartDay.BackColor      = Color.White;
				ebContEndDay.BackColor     = Color.White;			
				ebTell.BackColor      = Color.White;			
				ebEmail.BackColor      = Color.White;	
			}
			else
			{
				if(commonModel.UserLevel=="20")
				{				
					ebMediaName.Text = commonModel.MediaName;		
					mediaCode = commonModel.MediaCode;		
					btnMediaSearch.Enabled     = false;					
				}
			
				if(commonModel.UserLevel=="30")
				{			
					ebRapName.Text = commonModel.RapName;
					rapCode = commonModel.RapCode;
					btnRapSearch.Enabled     = false;															
				}
				if(commonModel.UserLevel=="40")
				{				
					ebAgencyName.Text = commonModel.AgencyName;	
					agencyCode = commonModel.AgencyCode;	
					btnAgencySearch.Enabled     = false;	
				}		
			}
		}
		
		/// <summary>
		/// ������ ReadOnly
		/// </summary>
		private void SetTextReadonly()
		{
			ebMediaName.ReadOnly         = true;			
			ebRapName.ReadOnly         = true;			
			ebAgencyName.ReadOnly         = true;						
			ebCharger.ReadOnly         = false;			
			ebContStartDay.ReadOnly       = false;
			ebContEndDay.ReadOnly      = false;			
			ebTell.ReadOnly       = false;
			ebEmail.ReadOnly       = false;
			rbUseYn_Y.Enabled			= false;
			rbUseYn_N.Enabled			= false;
						
			ebMediaName.BackColor        = Color.WhiteSmoke;			
			ebRapName.BackColor        = Color.WhiteSmoke;			
			ebAgencyName.BackColor        = Color.WhiteSmoke;			
			ebCharger.BackColor        = Color.WhiteSmoke;			
			ebContStartDay.BackColor      = Color.WhiteSmoke;
			ebContEndDay.BackColor     = Color.WhiteSmoke;			
			ebTell.BackColor      = Color.WhiteSmoke;			
			ebEmail.BackColor      = Color.WhiteSmoke;	
						
		}

		/// <summary>
		/// ������ ����������
		/// </summary>
		private void ResetTextReadonly()
		{											
			ebCharger.ReadOnly         = false;			
			ebContStartDay.ReadOnly       = false;
			ebContEndDay.ReadOnly      = false;			
			ebTell.ReadOnly       = false;
			ebEmail.ReadOnly       = false;

			rbUseYn_Y.Enabled         = true;
			rbUseYn_N.Enabled         = true;
			
			ebCharger.BackColor        = Color.White;			
			ebContStartDay.BackColor      = Color.White;
			ebContEndDay.BackColor     = Color.White;			
			ebTell.BackColor      = Color.White;			
			ebEmail.BackColor      = Color.White;	

			if (IsAdding)
			{
				btnMediaSearch.Enabled     = true;
				btnRapSearch.Enabled     = true;
				btnAgencySearch.Enabled     = true;
				ebMediaName.ReadOnly         = true;			
				ebRapName.ReadOnly         = true;			
				ebAgencyName.ReadOnly         = true;						
				ebCharger.ReadOnly         = false;			
				ebContStartDay.ReadOnly       = false;
				ebContEndDay.ReadOnly      = false;			
				ebTell.ReadOnly       = false;
				ebEmail.ReadOnly       = false;


				ebMediaName.BackColor        = Color.White;			
				ebRapName.BackColor        = Color.White;			
				ebAgencyName.BackColor        = Color.White;			
				ebCharger.BackColor        = Color.White;			
				ebContStartDay.BackColor      = Color.White;
				ebContEndDay.BackColor     = Color.White;			
				ebTell.BackColor      = Color.White;			
				ebEmail.BackColor      = Color.White;	
			}
			else
			{
				btnMediaSearch.Enabled     = false;
				btnRapSearch.Enabled     = false;
				btnAgencySearch.Enabled     = false;
				ebMediaName.ReadOnly         = true;			
				ebRapName.ReadOnly         = true;			
				ebAgencyName.ReadOnly         = true;						
				ebCharger.ReadOnly         = false;			
				ebContStartDay.ReadOnly       = false;
				ebContEndDay.ReadOnly      = false;			
				ebTell.ReadOnly       = false;
				ebEmail.ReadOnly       = false;
				
				ebMediaName.BackColor        = Color.WhiteSmoke;			
				ebRapName.BackColor        = Color.WhiteSmoke;			
				ebAgencyName.BackColor        = Color.WhiteSmoke;			
				ebCharger.BackColor        = Color.WhiteSmoke;			
				ebContStartDay.BackColor      = Color.WhiteSmoke;
				ebContEndDay.BackColor     = Color.WhiteSmoke;			
				ebTell.BackColor      = Color.WhiteSmoke;			
				ebEmail.BackColor      = Color.WhiteSmoke;	
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
		//�� �Լ��� ��ü, ����, ����縦 �Է������ʾ������ ���â�� �߰� Ȯ���� ������ �� �޼ҵ带 �����Ѵ�.
		//�� �޼ҵ尡 ������ �Ǹ� �ش� �ʵ��� �˾��� ȣ��ȴ�.
		private void ResetPop(object flag)
		{
			//��ü
			if(flag.Equals("media"))
			{
				MediaPopForm mediaForm = new MediaPopForm(this);
				mediaForm.ShowDialog();	            
				mediaForm.Dispose();
				mediaForm = null;		
			}
			//����
			if(flag.Equals("rap"))
			{
				RapPopForm rapForm = new RapPopForm(this);
				rapForm.ShowDialog();            
				rapForm.Dispose();
				rapForm = null;		
			}
			//�����
			if(flag.Equals("agency"))
			{
				AgencyPopForm agencyForm = new AgencyPopForm(this);
				agencyForm.ShowDialog();            
				agencyForm.Dispose();
				agencyForm = null;		
			}
		}
		private void btnMediaSearch_Click(object sender, System.EventArgs e)
		{
			// ��ü���� �˻� �˾� ����
			MediaPopForm mediaForm = new MediaPopForm(this);

			mediaForm.ShowDialog();
            
			mediaForm.Dispose();
			mediaForm = null;		
		}
		public string MediaCode
		{
			set
			{
				this.mediaCode = value;
			}
		}

		public string MediaName
		{				
			set
			{
				this.ebMediaName.Text = value;
			}			
		}
		
		private void btnRapSearch_Click(object sender, System.EventArgs e)
		{
			// �̵����� ��� �˻� �˾� ����
			RapPopForm rapForm = new RapPopForm(this);

			rapForm.ShowDialog();
            
			rapForm.Dispose();
			rapForm = null;		
		}
		public string RapCode
		{
			set
			{
				this.rapCode = value;
			}
		}

		public string RapName
		{				
			set
			{
				this.ebRapName.Text = value;
			}			
		}

		private void btnAgencySearch_Click(object sender, System.EventArgs e)
		{
			// ����缱�� ��� �˻� �˾� ����
			AgencyPopForm agencyForm = new AgencyPopForm(this);

			agencyForm.ShowDialog();
            
			agencyForm.Dispose();
			agencyForm = null;		
		}
			
		public string AgencyCode
		{
			set
			{
				this.agencyCode = value;
			}
		}

		public string AgencyName
		{				
			set
			{
				this.ebAgencyName.Text = value;
			}			
		}

	}
}