// ===============================================================================
// AdFileControl for Charites Project
//
// AdFileControl.cs
//
// ����������� ������� �����մϴ�. 
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
 * [��������]
 * -------------------------------------------------------
 * �����ڵ�  : [A_01]
 * ������    : JH.Kim
 * ������    : 2015.11.
 * ��������  : �������� ��� �÷��� �߰�
 * --------------------------------------------------------
 */
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
using AdManagerClient._20_Contract._11_Campaign;

namespace AdManagerClient
{
	/// <summary>
	/// ����������� ��Ʈ��
	/// </summary>
    public class CampaignControl : System.Windows.Forms.UserControl, IUserControl
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
		CampaignModel campaignModel  = new CampaignModel();	// ���������

		// ȭ��ó���� ����
		bool IsNewSearchKey		  = true;					// �˻����Է� ����
		CurrencyManager cm        = null;					// ������ �׸����� ���濡 ���� �����ͼ� ������ ���Ͽ�			
		DataTable       dt        = null;

		CurrencyManager cmCampaign        = null;					// ������ �׸����� ���濡 ���� �����ͼ� ������ ���Ͽ�			
		DataTable       dtCampaign        = null;

		CurrencyManager cmItem        = null;					// ������ �׸����� ���濡 ���� �����ͼ� ������ ���Ͽ�			
		DataTable       dtItem        = null;

        bool IsSearching = false; // ��ȸ�� ��ȭ���� ������Ʈ �Ǵ� ���� ���� �ϱ����� 2011.11.29 JH.Park
		bool IsAdding             = false;
		bool canRead			  = false;
		bool canUpdate			  = false;
		bool canCreate            = false;
		bool canDelete            = false;

		string MediaCode      = "";
		string RapCode        = "";
		string AgencyCode     = "";
		string AdvertiserCode = "";
				
		// Key ������
		string keyContractSeq       = "";
        private DevExpress.XtraTab.XtraTabControl tbItem;
        private DevExpress.XtraTab.XtraTabPage tbp_Ad;
        private Janus.Windows.GridEX.GridEX grdExItemList;
        private BindingSource bsCampaignPns;
		string keyCampaign       = "";

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
		private Janus.Windows.EditControls.UIButton btnSearch;
		private Janus.Windows.UI.Dock.UIPanel uiPanelSearch;
		private Janus.Windows.UI.Dock.UIPanelInnerContainer uiPanelSearchContainer;
		private Janus.Windows.UI.Dock.UIPanel uiPanelList;
		private Janus.Windows.UI.Dock.UIPanelInnerContainer uiPanelListContainer;
		private Janus.Windows.UI.Dock.UIPanel uiPanelDetail;
		private Janus.Windows.UI.Dock.UIPanelInnerContainer uiPanelDetailContainer;
		private Janus.Windows.UI.Dock.UIPanelGroup uiPanelContract;
		private Janus.Windows.GridEX.EditControls.EditBox ebSearchKey;
		private Janus.Windows.EditControls.UIComboBox cbSearchMedia;
		private Janus.Windows.EditControls.UIComboBox cbSearchRap;
        private Janus.Windows.EditControls.UIComboBox cbSearchAgency;
		private System.Data.DataView dvContract;
		private Janus.Windows.GridEX.GridEX grdExContractList;
		private System.Windows.Forms.Panel panel1;
		private Janus.Windows.GridEX.GridEX gridEX1;
		private Janus.Windows.EditControls.UIComboBox cbSearchContractState;
		private System.Data.DataView dvItem;
		private Janus.Windows.UI.Dock.UIPanel uiPanelItemList;
		private Janus.Windows.UI.Dock.UIPanelInnerContainer uiPanelItemListContainer;
		private Janus.Windows.UI.Dock.UIPanelGroup uiPanelGroup1;
		private Janus.Windows.UI.Dock.UIPanel uiPanelDetail1;
		private Janus.Windows.UI.Dock.UIPanelInnerContainer uiPanelDetail1Container;
		private Janus.Windows.UI.Dock.UIPanelGroup uiPanelGroup2;
		private Janus.Windows.UI.Dock.UIPanel uiPanelDetail2;
		private Janus.Windows.UI.Dock.UIPanelInnerContainer uiPanelDetail2Container;
		private Janus.Windows.UI.Dock.UIPanelGroup uiPanelGroup3;
		private System.Windows.Forms.Panel pnlUserDetail;
		private Janus.Windows.EditControls.UIButton btnAdd;
		private Janus.Windows.EditControls.UIButton btnDelete;
        private Janus.Windows.EditControls.UIButton btnSave;
		private System.Windows.Forms.Panel panel2;
		private Janus.Windows.EditControls.UIRadioButton rbUseYn_N;
		private Janus.Windows.EditControls.UIRadioButton rbUseYn_Y;
		private System.Windows.Forms.Label lbUseYn;
		private Janus.Windows.GridEX.EditControls.EditBox ebCampaignName;
		private System.Windows.Forms.Label lbCampaignName;
		private Janus.Windows.GridEX.GridEX grdCampaignList;
		private Janus.Windows.EditControls.UIButton btnItem;
		private AdManagerClient._20_Contract._11_Campaign.CampaignDs campaignDs;
		private System.Data.DataView dvCampaign;
		private Janus.Windows.EditControls.UIButton btnDelete1;
		private Janus.Windows.GridEX.EditControls.NumericEditBox ebPrice;
		private System.Windows.Forms.Label label1;


		private System.ComponentModel.IContainer components;

		public CampaignControl()
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
            Janus.Windows.GridEX.GridEXLayout grdExContractList_DesignTimeLayout = new Janus.Windows.GridEX.GridEXLayout();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CampaignControl));
            Janus.Windows.GridEX.GridEXLayout grdCampaignList_DesignTimeLayout = new Janus.Windows.GridEX.GridEXLayout();
            Janus.Windows.GridEX.GridEXLayout grdExItemList_DesignTimeLayout = new Janus.Windows.GridEX.GridEXLayout();
            this.uiPM = new Janus.Windows.UI.Dock.UIPanelManager(this.components);
            this.uiPanelContract = new Janus.Windows.UI.Dock.UIPanelGroup();
            this.uiPanelSearch = new Janus.Windows.UI.Dock.UIPanel();
            this.uiPanelSearchContainer = new Janus.Windows.UI.Dock.UIPanelInnerContainer();
            this.pnlSearch = new System.Windows.Forms.Panel();
            this.cbSearchContractState = new Janus.Windows.EditControls.UIComboBox();
            this.ebSearchKey = new Janus.Windows.GridEX.EditControls.EditBox();
            this.cbSearchMedia = new Janus.Windows.EditControls.UIComboBox();
            this.cbSearchRap = new Janus.Windows.EditControls.UIComboBox();
            this.cbSearchAgency = new Janus.Windows.EditControls.UIComboBox();
            this.btnSearch = new Janus.Windows.EditControls.UIButton();
            this.uiPanelList = new Janus.Windows.UI.Dock.UIPanel();
            this.uiPanelListContainer = new Janus.Windows.UI.Dock.UIPanelInnerContainer();
            this.grdExContractList = new Janus.Windows.GridEX.GridEX();
            this.dvContract = new System.Data.DataView();
            this.campaignDs = new AdManagerClient._20_Contract._11_Campaign.CampaignDs();
            this.uiPanelGroup1 = new Janus.Windows.UI.Dock.UIPanelGroup();
            this.uiPanelGroup2 = new Janus.Windows.UI.Dock.UIPanelGroup();
            this.uiPanelDetail = new Janus.Windows.UI.Dock.UIPanel();
            this.uiPanelDetailContainer = new Janus.Windows.UI.Dock.UIPanelInnerContainer();
            this.grdCampaignList = new Janus.Windows.GridEX.GridEX();
            this.dvCampaign = new System.Data.DataView();
            this.uiPanelDetail1 = new Janus.Windows.UI.Dock.UIPanel();
            this.uiPanelDetail1Container = new Janus.Windows.UI.Dock.UIPanelInnerContainer();
            this.pnlUserDetail = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.ebPrice = new Janus.Windows.GridEX.EditControls.NumericEditBox();
            this.rbUseYn_N = new Janus.Windows.EditControls.UIRadioButton();
            this.rbUseYn_Y = new Janus.Windows.EditControls.UIRadioButton();
            this.lbUseYn = new System.Windows.Forms.Label();
            this.ebCampaignName = new Janus.Windows.GridEX.EditControls.EditBox();
            this.lbCampaignName = new System.Windows.Forms.Label();
            this.btnAdd = new Janus.Windows.EditControls.UIButton();
            this.btnDelete = new Janus.Windows.EditControls.UIButton();
            this.btnSave = new Janus.Windows.EditControls.UIButton();
            this.uiPanelGroup3 = new Janus.Windows.UI.Dock.UIPanelGroup();
            this.uiPanelItemList = new Janus.Windows.UI.Dock.UIPanel();
            this.uiPanelItemListContainer = new Janus.Windows.UI.Dock.UIPanelInnerContainer();
            this.tbItem = new DevExpress.XtraTab.XtraTabControl();
            this.tbp_Ad = new DevExpress.XtraTab.XtraTabPage();
            this.grdExItemList = new Janus.Windows.GridEX.GridEX();
            this.dvItem = new System.Data.DataView();
            this.uiPanelDetail2 = new Janus.Windows.UI.Dock.UIPanel();
            this.uiPanelDetail2Container = new Janus.Windows.UI.Dock.UIPanelInnerContainer();
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnDelete1 = new Janus.Windows.EditControls.UIButton();
            this.btnItem = new Janus.Windows.EditControls.UIButton();
            this.bsCampaignPns = new System.Windows.Forms.BindingSource(this.components);
            this.panel1 = new System.Windows.Forms.Panel();
            this.gridEX1 = new Janus.Windows.GridEX.GridEX();
            ((System.ComponentModel.ISupportInitialize)(this.uiPM)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelContract)).BeginInit();
            this.uiPanelContract.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelSearch)).BeginInit();
            this.uiPanelSearch.SuspendLayout();
            this.uiPanelSearchContainer.SuspendLayout();
            this.pnlSearch.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelList)).BeginInit();
            this.uiPanelList.SuspendLayout();
            this.uiPanelListContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdExContractList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dvContract)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.campaignDs)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelGroup1)).BeginInit();
            this.uiPanelGroup1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelGroup2)).BeginInit();
            this.uiPanelGroup2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelDetail)).BeginInit();
            this.uiPanelDetail.SuspendLayout();
            this.uiPanelDetailContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdCampaignList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dvCampaign)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelDetail1)).BeginInit();
            this.uiPanelDetail1.SuspendLayout();
            this.uiPanelDetail1Container.SuspendLayout();
            this.pnlUserDetail.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelGroup3)).BeginInit();
            this.uiPanelGroup3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelItemList)).BeginInit();
            this.uiPanelItemList.SuspendLayout();
            this.uiPanelItemListContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tbItem)).BeginInit();
            this.tbItem.SuspendLayout();
            this.tbp_Ad.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdExItemList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dvItem)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelDetail2)).BeginInit();
            this.uiPanelDetail2.SuspendLayout();
            this.uiPanelDetail2Container.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bsCampaignPns)).BeginInit();
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
            this.uiPM.SplitterSize = 0;
            this.uiPM.VisualStyle = Janus.Windows.UI.Dock.PanelVisualStyle.Office2007;
            this.uiPanelContract.Id = new System.Guid("b162c53a-7940-47cd-a0ed-9762c59c50d8");
            this.uiPanelContract.StaticGroup = true;
            this.uiPanelSearch.Id = new System.Guid("afb3b7d0-aa79-443c-a1e8-3baba38f7a4d");
            this.uiPanelContract.Panels.Add(this.uiPanelSearch);
            this.uiPanelList.Id = new System.Guid("8e3140b9-bfcb-43af-b84e-2ee6cddf51dc");
            this.uiPanelContract.Panels.Add(this.uiPanelList);
            this.uiPanelGroup1.Id = new System.Guid("f3ad2785-82aa-4786-80fa-43ea052638f6");
            this.uiPanelGroup1.StaticGroup = true;
            this.uiPanelGroup2.Id = new System.Guid("c645852d-540b-4f95-a373-942ef63bbbaf");
            this.uiPanelGroup2.StaticGroup = true;
            this.uiPanelDetail.Id = new System.Guid("b2973a57-d3e1-4d80-9387-7a6dc3332542");
            this.uiPanelGroup2.Panels.Add(this.uiPanelDetail);
            this.uiPanelDetail1.Id = new System.Guid("2dcc70c8-c8b0-4ab9-9ad9-09f3d36320fb");
            this.uiPanelGroup2.Panels.Add(this.uiPanelDetail1);
            this.uiPanelGroup1.Panels.Add(this.uiPanelGroup2);
            this.uiPanelGroup3.Id = new System.Guid("56dd3fd5-236d-4f13-8a18-52c24e9948c9");
            this.uiPanelGroup3.StaticGroup = true;
            this.uiPanelItemList.Id = new System.Guid("fbe2f79a-054a-4dbc-84cf-7affe8bc146b");
            this.uiPanelGroup3.Panels.Add(this.uiPanelItemList);
            this.uiPanelDetail2.Id = new System.Guid("4f8fbb47-9992-49c7-80ec-44233e3ddd8f");
            this.uiPanelGroup3.Panels.Add(this.uiPanelDetail2);
            this.uiPanelGroup1.Panels.Add(this.uiPanelGroup3);
            this.uiPanelContract.Panels.Add(this.uiPanelGroup1);
            this.uiPM.Panels.Add(this.uiPanelContract);
            // 
            // Design Time Panel Info:
            // 
            this.uiPM.BeginPanelInfo();
            this.uiPM.AddDockPanelInfo(new System.Guid("b162c53a-7940-47cd-a0ed-9762c59c50d8"), Janus.Windows.UI.Dock.PanelGroupStyle.HorizontalTiles, Janus.Windows.UI.Dock.PanelDockStyle.Fill, true, new System.Drawing.Size(1010, 677), true);
            this.uiPM.AddDockPanelInfo(new System.Guid("afb3b7d0-aa79-443c-a1e8-3baba38f7a4d"), new System.Guid("b162c53a-7940-47cd-a0ed-9762c59c50d8"), 40, true);
            this.uiPM.AddDockPanelInfo(new System.Guid("8e3140b9-bfcb-43af-b84e-2ee6cddf51dc"), new System.Guid("b162c53a-7940-47cd-a0ed-9762c59c50d8"), 198, true);
            this.uiPM.AddDockPanelInfo(new System.Guid("f3ad2785-82aa-4786-80fa-43ea052638f6"), new System.Guid("b162c53a-7940-47cd-a0ed-9762c59c50d8"), Janus.Windows.UI.Dock.PanelGroupStyle.VerticalTiles, true, 417, true);
            this.uiPM.AddDockPanelInfo(new System.Guid("c645852d-540b-4f95-a373-942ef63bbbaf"), new System.Guid("f3ad2785-82aa-4786-80fa-43ea052638f6"), Janus.Windows.UI.Dock.PanelGroupStyle.HorizontalTiles, true, 505, true);
            this.uiPM.AddDockPanelInfo(new System.Guid("b2973a57-d3e1-4d80-9387-7a6dc3332542"), new System.Guid("c645852d-540b-4f95-a373-942ef63bbbaf"), 300, true);
            this.uiPM.AddDockPanelInfo(new System.Guid("2dcc70c8-c8b0-4ab9-9ad9-09f3d36320fb"), new System.Guid("c645852d-540b-4f95-a373-942ef63bbbaf"), 119, true);
            this.uiPM.AddDockPanelInfo(new System.Guid("56dd3fd5-236d-4f13-8a18-52c24e9948c9"), new System.Guid("f3ad2785-82aa-4786-80fa-43ea052638f6"), Janus.Windows.UI.Dock.PanelGroupStyle.HorizontalTiles, true, 505, true);
            this.uiPM.AddDockPanelInfo(new System.Guid("fbe2f79a-054a-4dbc-84cf-7affe8bc146b"), new System.Guid("56dd3fd5-236d-4f13-8a18-52c24e9948c9"), 410, true);
            this.uiPM.AddDockPanelInfo(new System.Guid("4f8fbb47-9992-49c7-80ec-44233e3ddd8f"), new System.Guid("56dd3fd5-236d-4f13-8a18-52c24e9948c9"), 9, true);
            this.uiPM.AddFloatingPanelInfo(new System.Guid("b162c53a-7940-47cd-a0ed-9762c59c50d8"), Janus.Windows.UI.Dock.PanelGroupStyle.HorizontalTiles, true, new System.Drawing.Point(870, 281), new System.Drawing.Size(200, 200), false);
            this.uiPM.AddFloatingPanelInfo(new System.Guid("afb3b7d0-aa79-443c-a1e8-3baba38f7a4d"), new System.Guid("b162c53a-7940-47cd-a0ed-9762c59c50d8"), 218, false);
            this.uiPM.AddFloatingPanelInfo(new System.Guid("8e3140b9-bfcb-43af-b84e-2ee6cddf51dc"), new System.Guid("b162c53a-7940-47cd-a0ed-9762c59c50d8"), 218, false);
            this.uiPM.AddFloatingPanelInfo(new System.Guid("f3ad2785-82aa-4786-80fa-43ea052638f6"), new System.Guid("b162c53a-7940-47cd-a0ed-9762c59c50d8"), Janus.Windows.UI.Dock.PanelGroupStyle.VerticalTiles, true, 218, false);
            this.uiPM.AddFloatingPanelInfo(new System.Guid("c645852d-540b-4f95-a373-942ef63bbbaf"), new System.Guid("f3ad2785-82aa-4786-80fa-43ea052638f6"), Janus.Windows.UI.Dock.PanelGroupStyle.HorizontalTiles, true, 505, false);
            this.uiPM.AddFloatingPanelInfo(new System.Guid("b2973a57-d3e1-4d80-9387-7a6dc3332542"), new System.Guid("c645852d-540b-4f95-a373-942ef63bbbaf"), 172, false);
            this.uiPM.AddFloatingPanelInfo(new System.Guid("2dcc70c8-c8b0-4ab9-9ad9-09f3d36320fb"), new System.Guid("c645852d-540b-4f95-a373-942ef63bbbaf"), 172, false);
            this.uiPM.AddFloatingPanelInfo(new System.Guid("56dd3fd5-236d-4f13-8a18-52c24e9948c9"), new System.Guid("f3ad2785-82aa-4786-80fa-43ea052638f6"), Janus.Windows.UI.Dock.PanelGroupStyle.HorizontalTiles, true, 505, false);
            this.uiPM.AddFloatingPanelInfo(new System.Guid("fbe2f79a-054a-4dbc-84cf-7affe8bc146b"), new System.Guid("56dd3fd5-236d-4f13-8a18-52c24e9948c9"), 172, false);
            this.uiPM.AddFloatingPanelInfo(new System.Guid("4f8fbb47-9992-49c7-80ec-44233e3ddd8f"), new System.Guid("56dd3fd5-236d-4f13-8a18-52c24e9948c9"), 172, false);
            this.uiPM.AddFloatingPanelInfo(new System.Guid("e4c8b43b-23fa-42dd-9545-cba44cda221e"), Janus.Windows.UI.Dock.PanelGroupStyle.HorizontalTiles, true, new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
            this.uiPM.AddFloatingPanelInfo(new System.Guid("ec2a0192-2913-4083-85e6-2c715537b80b"), new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
            this.uiPM.EndPanelInfo();
            // 
            // uiPanelContract
            // 
            this.uiPanelContract.ActiveCaptionMode = Janus.Windows.UI.Dock.ActiveCaptionMode.Never;
            this.uiPanelContract.AllowResize = Janus.Windows.UI.InheritableBoolean.False;
            this.uiPanelContract.BackColor = System.Drawing.SystemColors.Window;
            this.uiPanelContract.CaptionVisible = Janus.Windows.UI.InheritableBoolean.True;
            this.uiPanelContract.CloseButtonVisible = Janus.Windows.UI.InheritableBoolean.False;
            this.uiPanelContract.FloatingLocation = new System.Drawing.Point(870, 281);
            this.uiPanelContract.Location = new System.Drawing.Point(0, 0);
            this.uiPanelContract.Name = "uiPanelContract";
            this.uiPanelContract.Size = new System.Drawing.Size(1010, 677);
            this.uiPanelContract.TabIndex = 4;
            this.uiPanelContract.Text = "ķ���ΰ���";
            // 
            // uiPanelSearch
            // 
            this.uiPanelSearch.AllowResize = Janus.Windows.UI.InheritableBoolean.False;
            this.uiPanelSearch.CaptionVisible = Janus.Windows.UI.InheritableBoolean.False;
            this.uiPanelSearch.CloseButtonVisible = Janus.Windows.UI.InheritableBoolean.False;
            this.uiPanelSearch.InnerContainer = this.uiPanelSearchContainer;
            this.uiPanelSearch.Location = new System.Drawing.Point(0, 22);
            this.uiPanelSearch.Name = "uiPanelSearch";
            this.uiPanelSearch.Padding = new System.Windows.Forms.Padding(2);
            this.uiPanelSearch.Size = new System.Drawing.Size(1010, 40);
            this.uiPanelSearch.TabIndex = 4;
            this.uiPanelSearch.Text = "�˻�";
            // 
            // uiPanelSearchContainer
            // 
            this.uiPanelSearchContainer.Controls.Add(this.pnlSearch);
            this.uiPanelSearchContainer.Location = new System.Drawing.Point(1, 1);
            this.uiPanelSearchContainer.Name = "uiPanelSearchContainer";
            this.uiPanelSearchContainer.Size = new System.Drawing.Size(1008, 38);
            this.uiPanelSearchContainer.TabIndex = 0;
            // 
            // pnlSearch
            // 
            this.pnlSearch.BackColor = System.Drawing.SystemColors.Window;
            this.pnlSearch.Controls.Add(this.cbSearchContractState);
            this.pnlSearch.Controls.Add(this.ebSearchKey);
            this.pnlSearch.Controls.Add(this.cbSearchMedia);
            this.pnlSearch.Controls.Add(this.cbSearchRap);
            this.pnlSearch.Controls.Add(this.cbSearchAgency);
            this.pnlSearch.Controls.Add(this.btnSearch);
            this.pnlSearch.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlSearch.Location = new System.Drawing.Point(0, 0);
            this.pnlSearch.Name = "pnlSearch";
            this.pnlSearch.Size = new System.Drawing.Size(1008, 38);
            this.pnlSearch.TabIndex = 0;
            // 
            // cbSearchContractState
            // 
            this.cbSearchContractState.BackColor = System.Drawing.Color.White;
            this.cbSearchContractState.ComboStyle = Janus.Windows.EditControls.ComboStyle.DropDownList;
            this.cbSearchContractState.Location = new System.Drawing.Point(393, 9);
            this.cbSearchContractState.Name = "cbSearchContractState";
            this.cbSearchContractState.Size = new System.Drawing.Size(120, 21);
            this.cbSearchContractState.TabIndex = 6;
            this.cbSearchContractState.Text = "������";
            this.cbSearchContractState.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            // 
            // ebSearchKey
            // 
            this.ebSearchKey.Location = new System.Drawing.Point(519, 9);
            this.ebSearchKey.Name = "ebSearchKey";
            this.ebSearchKey.Size = new System.Drawing.Size(304, 21);
            this.ebSearchKey.TabIndex = 5;
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
            this.cbSearchMedia.Location = new System.Drawing.Point(8, 9);
            this.cbSearchMedia.Name = "cbSearchMedia";
            this.cbSearchMedia.Size = new System.Drawing.Size(120, 21);
            this.cbSearchMedia.TabIndex = 1;
            this.cbSearchMedia.Text = "��ü����";
            this.cbSearchMedia.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            // 
            // cbSearchRap
            // 
            this.cbSearchRap.ComboStyle = Janus.Windows.EditControls.ComboStyle.DropDownList;
            this.cbSearchRap.Location = new System.Drawing.Point(136, 9);
            this.cbSearchRap.Name = "cbSearchRap";
            this.cbSearchRap.Size = new System.Drawing.Size(120, 21);
            this.cbSearchRap.TabIndex = 2;
            this.cbSearchRap.Text = "�̵�����";
            this.cbSearchRap.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            // 
            // cbSearchAgency
            // 
            this.cbSearchAgency.ComboStyle = Janus.Windows.EditControls.ComboStyle.DropDownList;
            this.cbSearchAgency.Location = new System.Drawing.Point(264, 9);
            this.cbSearchAgency.Name = "cbSearchAgency";
            this.cbSearchAgency.Size = new System.Drawing.Size(120, 21);
            this.cbSearchAgency.TabIndex = 3;
            this.cbSearchAgency.Text = "����缱��";
            this.cbSearchAgency.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            // 
            // btnSearch
            // 
            this.btnSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSearch.Enabled = false;
            this.btnSearch.Location = new System.Drawing.Point(895, 6);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(104, 24);
            this.btnSearch.TabIndex = 7;
            this.btnSearch.Text = "�� ȸ";
            this.btnSearch.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // uiPanelList
            // 
            this.uiPanelList.BackColor = System.Drawing.SystemColors.Window;
            this.uiPanelList.CaptionStyle = Janus.Windows.UI.Dock.PanelCaptionStyle.Dark;
            this.uiPanelList.InnerContainer = this.uiPanelListContainer;
            this.uiPanelList.Location = new System.Drawing.Point(0, 62);
            this.uiPanelList.MinimumSize = new System.Drawing.Size(-1, 100);
            this.uiPanelList.Name = "uiPanelList";
            this.uiPanelList.Size = new System.Drawing.Size(1010, 198);
            this.uiPanelList.TabIndex = 8;
            this.uiPanelList.TabStop = false;
            this.uiPanelList.Text = "��������";
            // 
            // uiPanelListContainer
            // 
            this.uiPanelListContainer.BackColor = System.Drawing.SystemColors.Window;
            this.uiPanelListContainer.Controls.Add(this.grdExContractList);
            this.uiPanelListContainer.Location = new System.Drawing.Point(1, 23);
            this.uiPanelListContainer.Name = "uiPanelListContainer";
            this.uiPanelListContainer.Size = new System.Drawing.Size(1008, 174);
            this.uiPanelListContainer.TabIndex = 0;
            // 
            // grdExContractList
            // 
            this.grdExContractList.AllowEdit = Janus.Windows.GridEX.InheritableBoolean.False;
            this.grdExContractList.AlternatingColors = true;
            this.grdExContractList.BorderStyle = Janus.Windows.GridEX.BorderStyle.None;
            this.grdExContractList.DataSource = this.dvContract;
            grdExContractList_DesignTimeLayout.LayoutString = resources.GetString("grdExContractList_DesignTimeLayout.LayoutString");
            this.grdExContractList.DesignTimeLayout = grdExContractList_DesignTimeLayout;
            this.grdExContractList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdExContractList.EditorsControlStyle.ButtonAppearance = Janus.Windows.GridEX.ButtonAppearance.Regular;
            this.grdExContractList.EmptyRows = true;
            this.grdExContractList.FocusCellFormatStyle.BackColor = System.Drawing.SystemColors.Highlight;
            this.grdExContractList.FocusCellFormatStyle.ForeColor = System.Drawing.Color.White;
            this.grdExContractList.FocusStyle = Janus.Windows.GridEX.FocusStyle.None;
            this.grdExContractList.GridLineColor = System.Drawing.Color.Silver;
            this.grdExContractList.GridLines = Janus.Windows.GridEX.GridLines.Vertical;
            this.grdExContractList.GridLineStyle = Janus.Windows.GridEX.GridLineStyle.Solid;
            this.grdExContractList.GroupByBoxVisible = false;
            this.grdExContractList.HideSelection = Janus.Windows.GridEX.HideSelection.HighlightInactive;
            this.grdExContractList.Location = new System.Drawing.Point(0, 0);
            this.grdExContractList.Name = "grdExContractList";
            this.grdExContractList.ScrollBars = Janus.Windows.GridEX.ScrollBars.Vertical;
            this.grdExContractList.SelectedInactiveFormatStyle.BackColor = System.Drawing.Color.Gold;
            this.grdExContractList.Size = new System.Drawing.Size(1008, 174);
            this.grdExContractList.TabIndex = 9;
            this.grdExContractList.TabKeyBehavior = Janus.Windows.GridEX.TabKeyBehavior.ControlNavigation;
            this.grdExContractList.ThemedAreas = ((Janus.Windows.GridEX.ThemedArea)(((((((((Janus.Windows.GridEX.ThemedArea.ScrollBars | Janus.Windows.GridEX.ThemedArea.EditControls) 
            | Janus.Windows.GridEX.ThemedArea.Headers) 
            | Janus.Windows.GridEX.ThemedArea.GroupByBox) 
            | Janus.Windows.GridEX.ThemedArea.GroupRows) 
            | Janus.Windows.GridEX.ThemedArea.ControlBorder) 
            | Janus.Windows.GridEX.ThemedArea.Cards) 
            | Janus.Windows.GridEX.ThemedArea.Gridlines) 
            | Janus.Windows.GridEX.ThemedArea.CheckBoxes)));
            this.grdExContractList.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            this.grdExContractList.Enter += new System.EventHandler(this.OnGrdRowChanged);
            // 
            // dvContract
            // 
            this.dvContract.Table = this.campaignDs.Contract;
            // 
            // campaignDs
            // 
            this.campaignDs.DataSetName = "CampaignDs";
            this.campaignDs.Locale = new System.Globalization.CultureInfo("en-US");
            this.campaignDs.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // uiPanelGroup1
            // 
            this.uiPanelGroup1.CaptionHeight = 0;
            this.uiPanelGroup1.GroupStyle = Janus.Windows.UI.Dock.PanelGroupStyle.VerticalTiles;
            this.uiPanelGroup1.Location = new System.Drawing.Point(0, 260);
            this.uiPanelGroup1.Margin = new System.Windows.Forms.Padding(2);
            this.uiPanelGroup1.Name = "uiPanelGroup1";
            this.uiPanelGroup1.Size = new System.Drawing.Size(1010, 417);
            this.uiPanelGroup1.TabIndex = 9;
            // 
            // uiPanelGroup2
            // 
            this.uiPanelGroup2.CaptionHeight = 0;
            this.uiPanelGroup2.Location = new System.Drawing.Point(0, -1);
            this.uiPanelGroup2.Name = "uiPanelGroup2";
            this.uiPanelGroup2.Size = new System.Drawing.Size(505, 418);
            this.uiPanelGroup2.TabIndex = 5;
            // 
            // uiPanelDetail
            // 
            this.uiPanelDetail.AllowResize = Janus.Windows.UI.InheritableBoolean.False;
            this.uiPanelDetail.BackColor = System.Drawing.SystemColors.Window;
            this.uiPanelDetail.CaptionStyle = Janus.Windows.UI.Dock.PanelCaptionStyle.Dark;
            this.uiPanelDetail.InnerContainer = this.uiPanelDetailContainer;
            this.uiPanelDetail.Location = new System.Drawing.Point(0, -1);
            this.uiPanelDetail.Name = "uiPanelDetail";
            this.uiPanelDetail.Size = new System.Drawing.Size(505, 300);
            this.uiPanelDetail.TabIndex = 10;
            this.uiPanelDetail.TabStop = false;
            this.uiPanelDetail.Text = "ķ����";
            // 
            // uiPanelDetailContainer
            // 
            this.uiPanelDetailContainer.Controls.Add(this.grdCampaignList);
            this.uiPanelDetailContainer.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.uiPanelDetailContainer.Location = new System.Drawing.Point(1, 23);
            this.uiPanelDetailContainer.Name = "uiPanelDetailContainer";
            this.uiPanelDetailContainer.Size = new System.Drawing.Size(503, 276);
            this.uiPanelDetailContainer.TabIndex = 0;
            // 
            // grdCampaignList
            // 
            this.grdCampaignList.AllowEdit = Janus.Windows.GridEX.InheritableBoolean.False;
            this.grdCampaignList.AlternatingColors = true;
            this.grdCampaignList.BorderStyle = Janus.Windows.GridEX.BorderStyle.None;
            this.grdCampaignList.DataSource = this.dvCampaign;
            grdCampaignList_DesignTimeLayout.LayoutString = resources.GetString("grdCampaignList_DesignTimeLayout.LayoutString");
            this.grdCampaignList.DesignTimeLayout = grdCampaignList_DesignTimeLayout;
            this.grdCampaignList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdCampaignList.EditorsControlStyle.ButtonAppearance = Janus.Windows.GridEX.ButtonAppearance.Regular;
            this.grdCampaignList.EmptyRows = true;
            this.grdCampaignList.FocusCellFormatStyle.BackColor = System.Drawing.SystemColors.Highlight;
            this.grdCampaignList.FocusCellFormatStyle.ForeColor = System.Drawing.Color.White;
            this.grdCampaignList.FocusStyle = Janus.Windows.GridEX.FocusStyle.None;
            this.grdCampaignList.GridLineColor = System.Drawing.Color.Silver;
            this.grdCampaignList.GridLines = Janus.Windows.GridEX.GridLines.Vertical;
            this.grdCampaignList.GridLineStyle = Janus.Windows.GridEX.GridLineStyle.Solid;
            this.grdCampaignList.GroupByBoxVisible = false;
            this.grdCampaignList.HideSelection = Janus.Windows.GridEX.HideSelection.HighlightInactive;
            this.grdCampaignList.Location = new System.Drawing.Point(0, 0);
            this.grdCampaignList.Name = "grdCampaignList";
            this.grdCampaignList.ScrollBars = Janus.Windows.GridEX.ScrollBars.Vertical;
            this.grdCampaignList.SelectedInactiveFormatStyle.BackColor = System.Drawing.Color.Gold;
            this.grdCampaignList.Size = new System.Drawing.Size(503, 276);
            this.grdCampaignList.TabIndex = 16;
            this.grdCampaignList.TabKeyBehavior = Janus.Windows.GridEX.TabKeyBehavior.ControlNavigation;
            this.grdCampaignList.ThemedAreas = ((Janus.Windows.GridEX.ThemedArea)(((((((((Janus.Windows.GridEX.ThemedArea.ScrollBars | Janus.Windows.GridEX.ThemedArea.EditControls) 
            | Janus.Windows.GridEX.ThemedArea.Headers) 
            | Janus.Windows.GridEX.ThemedArea.GroupByBox) 
            | Janus.Windows.GridEX.ThemedArea.GroupRows) 
            | Janus.Windows.GridEX.ThemedArea.ControlBorder) 
            | Janus.Windows.GridEX.ThemedArea.Cards) 
            | Janus.Windows.GridEX.ThemedArea.Gridlines) 
            | Janus.Windows.GridEX.ThemedArea.CheckBoxes)));
            this.grdCampaignList.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            this.grdCampaignList.Enter += new System.EventHandler(this.grdCampaignList_Enter);
            // 
            // dvCampaign
            // 
            this.dvCampaign.Table = this.campaignDs.Campaign;
            // 
            // uiPanelDetail1
            // 
            this.uiPanelDetail1.InnerContainer = this.uiPanelDetail1Container;
            this.uiPanelDetail1.Location = new System.Drawing.Point(0, 299);
            this.uiPanelDetail1.Name = "uiPanelDetail1";
            this.uiPanelDetail1.Size = new System.Drawing.Size(505, 119);
            this.uiPanelDetail1.TabIndex = 4;
            this.uiPanelDetail1.Text = "ķ���λ�";
            // 
            // uiPanelDetail1Container
            // 
            this.uiPanelDetail1Container.Controls.Add(this.pnlUserDetail);
            this.uiPanelDetail1Container.Location = new System.Drawing.Point(1, 23);
            this.uiPanelDetail1Container.Name = "uiPanelDetail1Container";
            this.uiPanelDetail1Container.Size = new System.Drawing.Size(503, 95);
            this.uiPanelDetail1Container.TabIndex = 0;
            // 
            // pnlUserDetail
            // 
            this.pnlUserDetail.BackColor = System.Drawing.SystemColors.Window;
            this.pnlUserDetail.Controls.Add(this.label1);
            this.pnlUserDetail.Controls.Add(this.ebPrice);
            this.pnlUserDetail.Controls.Add(this.rbUseYn_N);
            this.pnlUserDetail.Controls.Add(this.rbUseYn_Y);
            this.pnlUserDetail.Controls.Add(this.lbUseYn);
            this.pnlUserDetail.Controls.Add(this.ebCampaignName);
            this.pnlUserDetail.Controls.Add(this.lbCampaignName);
            this.pnlUserDetail.Controls.Add(this.btnAdd);
            this.pnlUserDetail.Controls.Add(this.btnDelete);
            this.pnlUserDetail.Controls.Add(this.btnSave);
            this.pnlUserDetail.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlUserDetail.Location = new System.Drawing.Point(0, 0);
            this.pnlUserDetail.Name = "pnlUserDetail";
            this.pnlUserDetail.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.pnlUserDetail.Size = new System.Drawing.Size(503, 95);
            this.pnlUserDetail.TabIndex = 30;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(206, 33);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(50, 21);
            this.label1.TabIndex = 285;
            this.label1.Text = "�����";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ebPrice
            // 
            this.ebPrice.BorderStyle = Janus.Windows.GridEX.BorderStyle.SunkenLight3D;
            this.ebPrice.DecimalDigits = 0;
            this.ebPrice.FormatString = "#,##0";
            this.ebPrice.Location = new System.Drawing.Point(258, 33);
            this.ebPrice.MaxLength = 17;
            this.ebPrice.Name = "ebPrice";
            this.ebPrice.Size = new System.Drawing.Size(118, 21);
            this.ebPrice.TabIndex = 284;
            this.ebPrice.Text = "0";
            this.ebPrice.TextAlignment = Janus.Windows.GridEX.TextAlignment.Far;
            this.ebPrice.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.ebPrice.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            // 
            // rbUseYn_N
            // 
            this.rbUseYn_N.BackColor = System.Drawing.Color.Transparent;
            this.rbUseYn_N.CheckedValue = "N";
            this.rbUseYn_N.Location = new System.Drawing.Point(126, 33);
            this.rbUseYn_N.Name = "rbUseYn_N";
            this.rbUseYn_N.Size = new System.Drawing.Size(62, 21);
            this.rbUseYn_N.TabIndex = 281;
            this.rbUseYn_N.Text = "�̻��";
            this.rbUseYn_N.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            // 
            // rbUseYn_Y
            // 
            this.rbUseYn_Y.BackColor = System.Drawing.Color.Transparent;
            this.rbUseYn_Y.CheckedValue = "Y";
            this.rbUseYn_Y.Location = new System.Drawing.Point(74, 33);
            this.rbUseYn_Y.Name = "rbUseYn_Y";
            this.rbUseYn_Y.Size = new System.Drawing.Size(50, 21);
            this.rbUseYn_Y.TabIndex = 282;
            this.rbUseYn_Y.Text = "���";
            this.rbUseYn_Y.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            // 
            // lbUseYn
            // 
            this.lbUseYn.Location = new System.Drawing.Point(8, 33);
            this.lbUseYn.Name = "lbUseYn";
            this.lbUseYn.Size = new System.Drawing.Size(58, 21);
            this.lbUseYn.TabIndex = 283;
            this.lbUseYn.Text = "��뿩��";
            this.lbUseYn.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ebCampaignName
            // 
            this.ebCampaignName.Location = new System.Drawing.Point(72, 6);
            this.ebCampaignName.MaxLength = 100;
            this.ebCampaignName.Name = "ebCampaignName";
            this.ebCampaignName.Size = new System.Drawing.Size(304, 21);
            this.ebCampaignName.TabIndex = 279;
            this.ebCampaignName.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
            this.ebCampaignName.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            // 
            // lbCampaignName
            // 
            this.lbCampaignName.BackColor = System.Drawing.Color.Transparent;
            this.lbCampaignName.Location = new System.Drawing.Point(8, 6);
            this.lbCampaignName.Name = "lbCampaignName";
            this.lbCampaignName.Size = new System.Drawing.Size(72, 21);
            this.lbCampaignName.TabIndex = 280;
            this.lbCampaignName.Text = "ķ���θ�";
            this.lbCampaignName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnAdd
            // 
            this.btnAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnAdd.BackColor = System.Drawing.SystemColors.Window;
            this.btnAdd.Enabled = false;
            this.btnAdd.Location = new System.Drawing.Point(231, 65);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(104, 24);
            this.btnAdd.TabIndex = 32;
            this.btnAdd.Text = "�� ��";
            this.btnAdd.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnDelete.BackColor = System.Drawing.SystemColors.Window;
            this.btnDelete.Enabled = false;
            this.btnDelete.Location = new System.Drawing.Point(119, 65);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(104, 24);
            this.btnDelete.TabIndex = 31;
            this.btnDelete.TabStop = false;
            this.btnDelete.Text = "�� ��";
            this.btnDelete.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnSave.BackColor = System.Drawing.SystemColors.Window;
            this.btnSave.Enabled = false;
            this.btnSave.Location = new System.Drawing.Point(8, 65);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(104, 24);
            this.btnSave.TabIndex = 30;
            this.btnSave.Text = "�� ��";
            this.btnSave.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // uiPanelGroup3
            // 
            this.uiPanelGroup3.CaptionHeight = 0;
            this.uiPanelGroup3.Location = new System.Drawing.Point(505, -1);
            this.uiPanelGroup3.Name = "uiPanelGroup3";
            this.uiPanelGroup3.Size = new System.Drawing.Size(505, 418);
            this.uiPanelGroup3.TabIndex = 6;
            // 
            // uiPanelItemList
            // 
            this.uiPanelItemList.CaptionStyle = Janus.Windows.UI.Dock.PanelCaptionStyle.Dark;
            this.uiPanelItemList.InnerContainer = this.uiPanelItemListContainer;
            this.uiPanelItemList.Location = new System.Drawing.Point(0, -1);
            this.uiPanelItemList.Name = "uiPanelItemList";
            this.uiPanelItemList.Size = new System.Drawing.Size(505, 379);
            this.uiPanelItemList.TabIndex = 4;
            this.uiPanelItemList.Text = "����";
            // 
            // uiPanelItemListContainer
            // 
            this.uiPanelItemListContainer.Controls.Add(this.tbItem);
            this.uiPanelItemListContainer.Location = new System.Drawing.Point(1, 23);
            this.uiPanelItemListContainer.Name = "uiPanelItemListContainer";
            this.uiPanelItemListContainer.Size = new System.Drawing.Size(503, 355);
            this.uiPanelItemListContainer.TabIndex = 0;
            // 
            // tbItem
            // 
            this.tbItem.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Office2003;
            this.tbItem.BorderStylePage = DevExpress.XtraEditors.Controls.BorderStyles.Office2003;
            this.tbItem.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbItem.Location = new System.Drawing.Point(0, 0);
            this.tbItem.LookAndFeel.SkinName = "Metropolis";
            this.tbItem.LookAndFeel.UseDefaultLookAndFeel = false;
            this.tbItem.Name = "tbItem";
            this.tbItem.SelectedTabPage = this.tbp_Ad;
            this.tbItem.Size = new System.Drawing.Size(503, 355);
            this.tbItem.TabIndex = 17;
            this.tbItem.TabPages.AddRange(new DevExpress.XtraTab.XtraTabPage[] {
            this.tbp_Ad});
            // 
            // tbp_Ad
            // 
            this.tbp_Ad.Controls.Add(this.grdExItemList);
            this.tbp_Ad.Name = "tbp_Ad";
            this.tbp_Ad.Size = new System.Drawing.Size(495, 324);
            this.tbp_Ad.Text = "����";
            // 
            // grdExItemList
            // 
            this.grdExItemList.AlternatingColors = true;
            this.grdExItemList.BorderStyle = Janus.Windows.GridEX.BorderStyle.Sunken;
            this.grdExItemList.DataSource = this.dvItem;
            grdExItemList_DesignTimeLayout.LayoutString = resources.GetString("grdExItemList_DesignTimeLayout.LayoutString");
            this.grdExItemList.DesignTimeLayout = grdExItemList_DesignTimeLayout;
            this.grdExItemList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdExItemList.EmptyRows = true;
            this.grdExItemList.FocusCellFormatStyle.BackColor = System.Drawing.SystemColors.Highlight;
            this.grdExItemList.FocusCellFormatStyle.ForeColor = System.Drawing.Color.White;
            this.grdExItemList.FocusStyle = Janus.Windows.GridEX.FocusStyle.None;
            this.grdExItemList.Font = new System.Drawing.Font("���� ���", 8.25F);
            this.grdExItemList.GridLineColor = System.Drawing.Color.Silver;
            this.grdExItemList.GridLines = Janus.Windows.GridEX.GridLines.Vertical;
            this.grdExItemList.GridLineStyle = Janus.Windows.GridEX.GridLineStyle.Solid;
            this.grdExItemList.GroupByBoxVisible = false;
            this.grdExItemList.Location = new System.Drawing.Point(0, 0);
            this.grdExItemList.Name = "grdExItemList";
            this.grdExItemList.ScrollBars = Janus.Windows.GridEX.ScrollBars.Vertical;
            this.grdExItemList.SelectedInactiveFormatStyle.BackColor = System.Drawing.Color.Gold;
            this.grdExItemList.Size = new System.Drawing.Size(495, 324);
            this.grdExItemList.TabIndex = 17;
            this.grdExItemList.TabKeyBehavior = Janus.Windows.GridEX.TabKeyBehavior.ControlNavigation;
            this.grdExItemList.ThemedAreas = ((Janus.Windows.GridEX.ThemedArea)(((((((((Janus.Windows.GridEX.ThemedArea.ScrollBars | Janus.Windows.GridEX.ThemedArea.EditControls) 
            | Janus.Windows.GridEX.ThemedArea.Headers) 
            | Janus.Windows.GridEX.ThemedArea.GroupByBox) 
            | Janus.Windows.GridEX.ThemedArea.GroupRows) 
            | Janus.Windows.GridEX.ThemedArea.ControlBorder) 
            | Janus.Windows.GridEX.ThemedArea.Cards) 
            | Janus.Windows.GridEX.ThemedArea.Gridlines) 
            | Janus.Windows.GridEX.ThemedArea.CheckBoxes)));
            this.grdExItemList.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            this.grdExItemList.CellValueChanged += new Janus.Windows.GridEX.ColumnActionEventHandler(this.grdExItemList_CellValueChanged);
            // 
            // dvItem
            // 
            this.dvItem.Table = this.campaignDs.ContractItem;
            // 
            // uiPanelDetail2
            // 
            this.uiPanelDetail2.CaptionHeight = 0;
            this.uiPanelDetail2.InnerContainer = this.uiPanelDetail2Container;
            this.uiPanelDetail2.Location = new System.Drawing.Point(0, 378);
            this.uiPanelDetail2.Name = "uiPanelDetail2";
            this.uiPanelDetail2.Size = new System.Drawing.Size(505, 40);
            this.uiPanelDetail2.TabIndex = 4;
            this.uiPanelDetail2.Text = "uiPanelDetail2";
            // 
            // uiPanelDetail2Container
            // 
            this.uiPanelDetail2Container.Controls.Add(this.panel2);
            this.uiPanelDetail2Container.Location = new System.Drawing.Point(1, 0);
            this.uiPanelDetail2Container.Name = "uiPanelDetail2Container";
            this.uiPanelDetail2Container.Size = new System.Drawing.Size(503, 39);
            this.uiPanelDetail2Container.TabIndex = 0;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.SystemColors.Window;
            this.panel2.Controls.Add(this.btnDelete1);
            this.panel2.Controls.Add(this.btnItem);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.panel2.Size = new System.Drawing.Size(503, 39);
            this.panel2.TabIndex = 31;
            // 
            // btnDelete1
            // 
            this.btnDelete1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnDelete1.BackColor = System.Drawing.SystemColors.Window;
            this.btnDelete1.Enabled = false;
            this.btnDelete1.Location = new System.Drawing.Point(386, 8);
            this.btnDelete1.Name = "btnDelete1";
            this.btnDelete1.Size = new System.Drawing.Size(104, 24);
            this.btnDelete1.TabIndex = 33;
            this.btnDelete1.TabStop = false;
            this.btnDelete1.Text = "�� ��";
            this.btnDelete1.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.btnDelete1.Click += new System.EventHandler(this.btnDelete1_Click);
            // 
            // btnItem
            // 
            this.btnItem.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnItem.BackColor = System.Drawing.SystemColors.Window;
            this.btnItem.Enabled = false;
            this.btnItem.Location = new System.Drawing.Point(276, 8);
            this.btnItem.Name = "btnItem";
            this.btnItem.Size = new System.Drawing.Size(104, 24);
            this.btnItem.TabIndex = 32;
            this.btnItem.Text = "������";
            this.btnItem.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.btnItem.Click += new System.EventHandler(this.btnItem_Click);
            // 
            // bsCampaignPns
            // 
            this.bsCampaignPns.DataMember = "CampaignPns";
            this.bsCampaignPns.DataSource = this.campaignDs;
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
            // CampaignControl
            // 
            this.Controls.Add(this.uiPanelContract);
            this.Font = global::AdManagerClient.Properties.Settings.Default.Font1;
            this.Name = "CampaignControl";
            this.Size = new System.Drawing.Size(1010, 677);
            this.Load += new System.EventHandler(this.UserControl_Load);
            ((System.ComponentModel.ISupportInitialize)(this.uiPM)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelContract)).EndInit();
            this.uiPanelContract.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelSearch)).EndInit();
            this.uiPanelSearch.ResumeLayout(false);
            this.uiPanelSearchContainer.ResumeLayout(false);
            this.pnlSearch.ResumeLayout(false);
            this.pnlSearch.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelList)).EndInit();
            this.uiPanelList.ResumeLayout(false);
            this.uiPanelListContainer.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdExContractList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dvContract)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.campaignDs)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelGroup1)).EndInit();
            this.uiPanelGroup1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelGroup2)).EndInit();
            this.uiPanelGroup2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelDetail)).EndInit();
            this.uiPanelDetail.ResumeLayout(false);
            this.uiPanelDetailContainer.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdCampaignList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dvCampaign)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelDetail1)).EndInit();
            this.uiPanelDetail1.ResumeLayout(false);
            this.uiPanelDetail1Container.ResumeLayout(false);
            this.pnlUserDetail.ResumeLayout(false);
            this.pnlUserDetail.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelGroup3)).EndInit();
            this.uiPanelGroup3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelItemList)).EndInit();
            this.uiPanelItemList.ResumeLayout(false);
            this.uiPanelItemListContainer.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.tbItem)).EndInit();
            this.tbItem.ResumeLayout(false);
            this.tbp_Ad.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdExItemList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dvItem)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelDetail2)).EndInit();
            this.uiPanelDetail2.ResumeLayout(false);
            this.uiPanelDetail2Container.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.bsCampaignPns)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridEX1)).EndInit();
            this.ResumeLayout(false);

		}
		#endregion

		#region ��Ʈ�� �ε�
		private void UserControl_Load(object sender, System.EventArgs e)
		{
			// �����Ͱ����� ��ü����
			dt = ((DataView)grdExContractList.DataSource).Table;  
			cm = (CurrencyManager) this.BindingContext[grdExContractList.DataSource]; 

			dtCampaign = ((DataView)grdCampaignList.DataSource).Table;  
			cmCampaign = (CurrencyManager) this.BindingContext[grdCampaignList.DataSource]; 

			dtItem = ((DataView)grdExItemList.DataSource).Table;  
			cmItem = (CurrencyManager) this.BindingContext[grdExItemList.DataSource]; 

			cm.PositionChanged += new System.EventHandler(OnGrdRowChanged); 
			cmCampaign.PositionChanged += new System.EventHandler(grdCampaignList_Enter); 

			// ��Ʈ�� �ʱ�ȭ
			InitControl();	
		}

		#endregion

		#region ��Ʈ�� �ʱ�ȭ
		private void InitControl()
		{
			ProgressStart();
			InitCombo();

			// ��ȸ���� �˻�
			if(menu.CanRead(MenuCode))
			{
				canRead = true;
				SearchContract();
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
				//ResetTextReadOnly();
				canUpdate = true;
			}
			else
			{
				SetTextReadOnly();
			}

			InitButton();
			SetTextReadOnly();
			ProgressStop();
		}

		private void InitCombo()
		{
			Init_MediaCode();
			Init_RapCode();
			Init_AgencyCode();
			InitCombo_Level();
			Init_ContractState();
			cbSearchContractState.SelectedValue = "10";
		}

		private void Init_MediaCode()
		{

			// ��ü�� ��ȸ�Ѵ�.
			MediaCodeModel mediaCodeModel = new MediaCodeModel();
			new MediaCodeManager(systemModel, commonModel).GetMediaCodeList(mediaCodeModel);
			
			if (mediaCodeModel.ResultCD.Equals("0000"))
			{
				// �����ͼ¿� ����
				Utility.SetDataTable(campaignDs.Medias, mediaCodeModel.MediaCodeDataSet);				
			}

			// �˻������� �޺�
			this.cbSearchMedia.Items.Clear();
			
			// �޺��ڽ��� ��Ʈ�� �ڵ����� ���� Item�迭�� ����
			Janus.Windows.EditControls.UIComboBoxItem[] comboItems = new Janus.Windows.EditControls.UIComboBoxItem[mediaCodeModel.ResultCnt + 1];

			comboItems[0] = new Janus.Windows.EditControls.UIComboBoxItem("��ü����","00");
			
			for(int i=0;i<mediaCodeModel.ResultCnt;i++)
			{
				DataRow row = campaignDs.Medias.Rows[i];

				string val = row["MediaCode"].ToString();
				string txt = row["MediaName"].ToString();
				comboItems[i+1] = new Janus.Windows.EditControls.UIComboBoxItem(txt,val);
			}
			// �˻� �޺��� ��Ʈ
			this.cbSearchMedia.Items.AddRange(comboItems);
			this.cbSearchMedia.SelectedIndex = 0;
			
			Application.DoEvents();

		}

		private void InitCombo_Level() 
		{			
			//��ü�����ϰ�� ���ȵ�� ó��
			if(commonModel.UserLevel == "20")
			{
				// �޺��Ƚ�						
				cbSearchMedia.SelectedValue = commonModel.MediaCode;		
				cbSearchMedia.ReadOnly = true;
			}  
			else if(commonModel.UserLevel != "20")
			{
				for(int i=0;i < campaignDs.Medias.Rows.Count;i++)
				{
					DataRow row = campaignDs.Medias.Rows[i];					
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
			//�̵� �����ϰ���� ���ȵ��ó��
			if (commonModel.UserLevel == "30")
			{
				//��ȸ �޺� �Ƚ�
				cbSearchRap.SelectedValue = commonModel.RapCode;
				cbSearchRap.ReadOnly = true;                
			}
			//�����/������ ������ ����� ���ȵ��ó��
			if (commonModel.UserLevel == "40")
			{
				//��ȸ �޺��Ƚ�						
				cbSearchAgency.SelectedValue = commonModel.AgencyCode;			
				cbSearchAgency.ReadOnly = true;
			}


			Application.DoEvents();
		}   
    
		private void Init_RapCode()
		{
            /*
			// ���� ��ȸ�Ѵ�.
			MediaRapCodeModel mediaRapCodeModel = new MediaRapCodeModel();
			new MediaRapCodeManager(systemModel, commonModel).GetMediaRapCodeList(mediaRapCodeModel);
			
			if (mediaRapCodeModel.ResultCD.Equals("0000"))
			{
				// �����ͼ¿� ����
				Utility.SetDataTable(campaignDs.MediaRaps, mediaRapCodeModel.MediaRapCodeDataSet);				
			}

			// �˻������� �޺�
			this.cbSearchRap.Items.Clear();
			// �޺��ڽ��� ��Ʈ�� �ڵ����� ���� Item�迭�� ����
			Janus.Windows.EditControls.UIComboBoxItem[] comboItems = new Janus.Windows.EditControls.UIComboBoxItem[mediaRapCodeModel.ResultCnt + 1];

			comboItems[0] = new Janus.Windows.EditControls.UIComboBoxItem("�̵�����","00");
			
			for(int i=0;i<mediaRapCodeModel.ResultCnt;i++)
			{
				DataRow row = campaignDs.MediaRaps.Rows[i];

				string val = row["RapCode"].ToString();
				string txt = row["RapName"].ToString();
				comboItems[i+1] = new Janus.Windows.EditControls.UIComboBoxItem(txt,val);
			}
			// �޺��� ��Ʈ
			this.cbSearchRap.Items.AddRange(comboItems);
			this.cbSearchRap.SelectedIndex = 0;

			Application.DoEvents();
            */
            // ���� �������� ó��
            this.cbSearchRap.Items.Clear();
            DataSet ds = new DataSet("rapset");
            DataTable dt = new DataTable("rap");
            dt.Columns.Add("RapCode", typeof(string));
            dt.Columns.Add("RapName", typeof(string));
            DataRow nRow = dt.NewRow();
            nRow["RapCode"] = "1";
            nRow["RapName"] = "����� ����";
            dt.Rows.Add(nRow);
            ds.Tables.Add(dt);

            Utility.SetDataTable(campaignDs.MediaRaps, ds);
            // �˻������� �޺�
            this.cbSearchRap.Items.Clear();
            // �޺��ڽ��� ��Ʈ�� �ڵ����� ���� Item�迭�� ����
            Janus.Windows.EditControls.UIComboBoxItem[] comboItems = new Janus.Windows.EditControls.UIComboBoxItem[2];

            comboItems[0] = new Janus.Windows.EditControls.UIComboBoxItem("�̵�����", "00");

            for (int i = 0; i < 1; i++)
            {
                DataRow row = campaignDs.MediaRaps.Rows[i];

                string val = row["RapCode"].ToString();
                string txt = row["RapName"].ToString();
                comboItems[i + 1] = new Janus.Windows.EditControls.UIComboBoxItem(txt, val);
            }
            // �޺��� ��Ʈ
            this.cbSearchRap.Items.AddRange(comboItems);
            this.cbSearchRap.SelectedIndex = 1;
            this.cbSearchRap.ReadOnly = true;
            Application.DoEvents();
		}

		private void Init_AgencyCode()
		{
			// ����縦 ��ȸ�Ѵ�.
			AgencyCodeModel agencyCodeModel = new AgencyCodeModel();
			new AgencyCodeManager(systemModel, commonModel).GetAgencyCodeList(agencyCodeModel);
			
			if (agencyCodeModel.ResultCD.Equals("0000"))
			{
				// �����ͼ¿� ����
				Utility.SetDataTable(campaignDs.Agencys, agencyCodeModel.AgencyCodeDataSet);				
			}

			// �˻������� �޺�
			this.cbSearchAgency.Items.Clear();

			// �޺��ڽ��� ��Ʈ�� �ڵ����� ���� Item�迭�� ����
			Janus.Windows.EditControls.UIComboBoxItem[] comboItems = new Janus.Windows.EditControls.UIComboBoxItem[agencyCodeModel.ResultCnt + 1];

			comboItems[0] = new Janus.Windows.EditControls.UIComboBoxItem("����缱��","00");
			
			for(int i=0;i<agencyCodeModel.ResultCnt;i++)
			{
				DataRow row = campaignDs.Agencys.Rows[i];

				string val = row["AgencyCode"].ToString();
				string txt = row["AgencyName"].ToString();
				comboItems[i+1] = new Janus.Windows.EditControls.UIComboBoxItem(txt,val);
			}
			
			// �޺��� ��Ʈ            
			this.cbSearchAgency.Items.AddRange(comboItems);
			this.cbSearchAgency.SelectedIndex = 0;

			Application.DoEvents();
		}

		private void Init_ContractState()
		{
			// �ڵ忡�� �����¸� ��ȸ�Ѵ�.
			CodeModel codeModel = new CodeModel();
			codeModel.Section = "23";				// �ڵ�з� '23':������  TODO: �ڵ�з��� ���� XML�� �����Ǿ��...
			new CodeManager(systemModel, commonModel).GetCodeList(codeModel);
			
			if (codeModel.ResultCD.Equals("0000"))
			{
				// �����ͼ¿� ����
				Utility.SetDataTable(campaignDs.ContractState, codeModel.CodeDataSet);				
			}

			// �˻������� �޺�
			this.cbSearchContractState.Items.Clear();
						
			// �޺��ڽ��� ��Ʈ�� �ڵ����� ���� Item�迭�� ����
			Janus.Windows.EditControls.UIComboBoxItem[] comboItems = new Janus.Windows.EditControls.UIComboBoxItem[codeModel.ResultCnt + 1];

			comboItems[0] = new Janus.Windows.EditControls.UIComboBoxItem("�����¼���","00");
			
			for(int i=0;i<codeModel.ResultCnt;i++)
			{
				DataRow row = campaignDs.ContractState.Rows[i];

				string val = row["Code"].ToString();
				string txt = row["CodeName"].ToString();
				comboItems[i+1] = new Janus.Windows.EditControls.UIComboBoxItem(txt,val);
			}
			// �޺��� ��Ʈ
			this.cbSearchContractState.Items.AddRange(comboItems);
			this.cbSearchContractState.SelectedIndex = 0;
			
			Application.DoEvents();
		}
		
		private void InitButton()
		{
			if(canRead)   btnSearch.Enabled = true;
			if(canCreate) btnAdd.Enabled    = true;
			if(canDelete) btnDelete.Enabled    = true;
			if(ebCampaignName.Text.Trim().Length > 0) 
			{
				if(canDelete) btnDelete.Enabled = true;
				if(canUpdate) btnSave.Enabled   = true;
			}
//			if(IsAdding)
//			{
//				if(canCreate) btnAdvertise.Enabled = true;
//			}
//			else
//			{
//				btnAdvertise.Enabled = false;
//			}


			Application.DoEvents();
		}

		private void DisableButton()
		{
			btnSearch.Enabled = false;
			btnAdd.Enabled    = false;
			btnSave.Enabled   = false;
			btnDelete.Enabled = false;
			btnItem.Enabled = false;
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
            if (!IsSearching) // 2011.11.29 JH.Park ��ȸ���� �ƴҰ�쿡�� �����ϵ��� ����
            {
                if (grdExContractList.RowCount > 0)
                {
                    SetDetailText();
                    InitButton();
                }
            }
		}

        /// <summary>
        /// ķ���� �׸��� Row����� ���� ���� ó���ϴ� �κ�
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
		private void grdCampaignList_Enter(object sender, System.EventArgs e)
		{
			if(grdCampaignList.RowCount > 0)
			{
				SetCampaignText();
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
			SearchContract();
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
			btnDelete1.Enabled = false;
			if(canCreate)btnSave.Enabled   = true;
			if(canCreate)btnItem.Enabled = true;
            
			IsAdding = true;
			ResetTextReadOnly();
			ResetDetailText();
			ebCampaignName.Focus();	// �߰��� ���� ��ġ
		}

		/// <summary>
		/// �����ư Ŭ��
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnSave_Click(object sender, System.EventArgs e)
		{
			SaveCampaignDetail();
		}

		/// <summary>
		/// ������ư Ŭ��
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnDelete_Click(object sender, System.EventArgs e)
		{
			DeleteCampaign();
		}

		private void btnDelete1_Click(object sender, System.EventArgs e)
		{
            if (tbItem.SelectedTabPage == tbp_Ad)
            {
                DeleteCampaignDetail();
            }
            else
            {
                DeleteCampaignPns();
            }
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
				SearchContract();
			}
		}

		#endregion

		#region ó���޼ҵ�

		/// <summary>
		/// �������� ��ȸ
		/// </summary>
		private void SearchContract()
		{
            IsSearching = true;

			StatusMessage("������ ������ ��ȸ�մϴ�.");

			try
			{				
				//���� ���� ���� �ʱ�ȭ ���ش�.
				campaignModel.Init();				
				// �����͸𵨿� ������ ������ ��Ʈ�Ѵ�.
				campaignModel.MediaCode      = cbSearchMedia.SelectedValue.ToString();
				campaignModel.RapCode        = cbSearchRap.SelectedValue.ToString();
				campaignModel.AgencyCode     = cbSearchAgency.SelectedValue.ToString();
				campaignModel.State          = cbSearchContractState.SelectedValue.ToString();
				
				if(IsNewSearchKey)
				{
					campaignModel.SearchKey = "";					
				}
				else
				{
					campaignModel.SearchKey  = ebSearchKey.Text;
				}
                
				ResetDetailText();
				// ���� ��� ��� ���񽺸� ȣ���Ѵ�.
				new CampaignManager(systemModel,commonModel).GetContractList(campaignModel);				

				if (campaignModel.ResultCD.Equals("0000"))
				{											
					Utility.SetDataTable(campaignDs.Contract, campaignModel.ContractDataSet);			
					StatusMessage(campaignModel.ResultCnt + "���� ������� ������ ��ȸ�Ǿ����ϴ�.");                  
					if(canUpdate)
					{						
						AddSchChoice();									
					}				
					SetDetailText();
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
				if ( campaignDs.Tables["Contract"].Rows.Count < 1 ) return;
              
				foreach (DataRow row in campaignDs.Tables["Contract"].Rows)
				{					
					if(IsAdding)
					{
						
						cm.Position = 0;
						keyContractSeq = null;
					}
					else
					{						
						if(row["ContractSeq"].ToString().Equals(keyContractSeq))
						{					
							cm.Position = rowIndex;
							break;								
						}
					}

					rowIndex++;
					grdExContractList.EnsureVisible();
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
		/// ķ���θ�� ��ȸ
		/// </summary>
		private void SearchCampaign()
		{
			StatusMessage("ķ���� ������ ��ȸ�մϴ�.");

			try
			{				
				//���� ���� ���� �ʱ�ȭ ���ش�.
				campaignModel.Init();				
				campaignModel.MediaCode		= cbSearchMedia.SelectedValue.ToString();
				campaignModel.ContractSeq   = keyContractSeq;				
								
				if(IsNewSearchKey)	campaignModel.SearchKey = "";					
				else				campaignModel.SearchKey  = ebSearchKey.Text;
                
				ResetDetailText();
				// ���� ��� ��� ���񽺸� ȣ���Ѵ�.
				new CampaignManager(systemModel,commonModel).GetCampaignList(campaignModel);				

                campaignDs.Campaign.Clear();
				if (campaignModel.ResultCD.Equals("0000"))
				{											
					Utility.SetDataTable(campaignDs.Campaign, campaignModel.CampaignDataSet);			
					StatusMessage(campaignModel.ResultCnt + "���� ������� ������ ��ȸ�Ǿ����ϴ�.");                  
					if(canUpdate)
					{						
						AddSchChoice();									
					}				
					SetCampaignText();
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

		private void SearchContractItem()
		{
			StatusMessage("������ ������ ��ȸ�մϴ�.");

			try
			{
				//�˻� ���� ���� �ʱ�ȭ ���ش�.
				campaignModel.Init();
				campaignModel.MediaCode      = cbSearchMedia.SelectedValue.ToString();
				campaignModel.RapCode        = cbSearchRap.SelectedValue.ToString();
				campaignModel.AgencyCode     = cbSearchAgency.SelectedValue.ToString();
				campaignModel.CampaignCode   = keyCampaign;				

				// ���� ���� ��� ���񽺸� ȣ���Ѵ�.
				new CampaignManager(systemModel,commonModel).GetContractItemList(campaignModel);

                campaignDs.ContractItem.Clear();
                campaignDs.CampaignPns.Clear();

				if (campaignModel.ResultCD.Equals("0000"))
				{
					Utility.SetDataTable(campaignDs.ContractItem, campaignModel.ContractDataSet);
                    // ������
                    //Utility.SetDataTable(campaignDs.CampaignPns, campaignModel.CampaignDataSet);
					StatusMessage(campaignModel.ResultCnt + "���� �������� ������ ��ȸ�Ǿ����ϴ�.");					
				}

				if(grdExItemList.RowCount > 0)
				{
					btnDelete1.Enabled = true;
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
		/// ������������� ����
		/// </summary>
		private void SaveCampaignDetail()
		{
			StatusMessage("ķ���� ������ �����մϴ�.");
                        
			if(keyContractSeq.Length == 0) 
			{
				MessageBox.Show("����� ���õ��� �ʾҽ��ϴ�.","ķ�������� ����", MessageBoxButtons.OK, MessageBoxIcon.Information );
				return;				               
			}			
                      
			try
			{
				//���� ���� ���� �ʱ�ȭ ���ش�.
				campaignModel.Init();			
				// �����͸𵨿� ������ ������ ��Ʈ�Ѵ�.
				campaignModel.MediaCode     = MediaCode;				
				campaignModel.ContractSeq	= keyContractSeq;								
				campaignModel.CampaignName  = ebCampaignName.Text;
				campaignModel.Price			=	ebPrice.Value.ToString();

               
				if(rbUseYn_Y.Checked)
				{
					campaignModel.UseYn       = "Y";
				}
				else
				{
					campaignModel.UseYn       = "N";
				}
				
				// ������� ������ ���� ���񽺸� ȣ���Ѵ�.
				if (IsAdding)
				{
					new CampaignManager(systemModel,commonModel).SetCampaignCreate(campaignModel);
                
					IsAdding = false;					
					ResetDetailText();
					StatusMessage("������� ������ �߰��Ǿ����ϴ�.");
                
				}
				else
				{   					
					campaignModel.CampaignCode      = keyCampaign;
					new CampaignManager(systemModel,commonModel).SetCampaignUpdate(campaignModel);
					StatusMessage("������� ������ ����Ǿ����ϴ�.");                
				}

				DisableButton();
				SearchContract();					
				InitButton();
        
			}
			catch(FrameException fe)
			{
				FrameSystem.showMsgForm("ķ�������� �������", new string[] {fe.ErrCode, fe.ResultMsg});
			}
			catch(Exception ex)
			{
				FrameSystem.showMsgForm("ķ�������� �������",new string[] {"",ex.Message});
			}		
	
		}


		/// <summary>
		/// ķ�������� ����
		/// </summary>
		private void DeleteCampaign()
		{
			StatusMessage("ķ���� ������ �����մϴ�.");
            
			if(keyContractSeq.Trim().Length == 0) 
			{
				FrameSystem.showMsgForm("ķ�������� ��������",new string[] {"", "������ ķ���� ������ �����ϴ�.", "" });
				return;
			}

			DialogResult result = MessageBox.Show("�ش� ķ���� ������ ���� �Ͻðڽ��ϱ�?","ķ���� ����",
				MessageBoxButtons.YesNo, MessageBoxIcon.Question,
				MessageBoxDefaultButton.Button2);

			if (result == DialogResult.No) return;

			try 
			{
				// �����͸𵨿� ������ ������ ��Ʈ�Ѵ�.
				campaignModel.MediaCode	  = MediaCode;
				campaignModel.ContractSeq = keyContractSeq;
				campaignModel.CampaignCode = keyCampaign;

				// ��ü���౤���� ������ ���� ���񽺸� ȣ���Ѵ�.
				new CampaignManager(systemModel,commonModel).SetCampaignDelete(campaignModel);

				StatusMessage("ķ���� ������ �����Ǿ����ϴ�.");			
                
				ResetDetailText();
				DisableButton();
				SearchContract();
				InitButton();


			}
			catch(FrameException fe)
			{
				FrameSystem.showMsgForm("ķ�������� ��������", new string[] {fe.ErrCode, fe.ResultMsg});
			}
			catch(Exception ex)
			{
				FrameSystem.showMsgForm("ķ�������� ��������",new string[] {"",ex.Message});
			}	
		}
		

        /// <summary>
        /// ķ���γ���-���� ����
        /// </summary>
		private void DeleteCampaignDetail()
		{
			StatusMessage("ķ���� ������ �����մϴ�.");
            
			if(keyContractSeq.Trim().Length == 0) 
			{
				FrameSystem.showMsgForm("ķ�������� ��������",new string[] {"", "������ ķ���ε����� ������ �����ϴ�.", "" });
				return;
			}

			DialogResult result = MessageBox.Show("�ش� ķ���ε����� ������ ���� �Ͻðڽ��ϱ�?","ķ���ε����� ����",
				MessageBoxButtons.YesNo, MessageBoxIcon.Question,
				MessageBoxDefaultButton.Button2);

			if (result == DialogResult.No) return;

			grdExItemList.UpdateData();

			try 
			{
				int SetCount = 0;

				//�μ�Ʈ ��Ŵ
				for(int i=0;i < campaignDs.ContractItem.Rows.Count;i++)
				{
					DataRow row = campaignDs.ContractItem.Rows[i];

					if(row["CheckYn"].ToString().Equals("True"))
					{
						campaignModel.Init();						
						campaignModel.CampaignCode = keyCampaign;						
						campaignModel.ItemNo    = row["ItemNo"].ToString();
						
						new CampaignManager(systemModel,commonModel).SetCampaignDetailDelete(campaignModel);
						
						if(campaignModel.ResultCD.Equals("0000"))
						{
							SetCount++;
						}
					}
				}

				// üũ�� ��� �׸��� Ŭ����
				ClearListCheck();				
				
				if(SetCount > 0)
				{
					ResetDetailText();
					DisableButton();
					SearchContractItem();
					InitButton();	
				}			

				StatusMessage("ķ���ε����� ������ �����Ǿ����ϴ�.");
			}
			catch(FrameException fe)
			{
				FrameSystem.showMsgForm("ķ���ε��������� ��������", new string[] {fe.ErrCode, fe.ResultMsg});
			}
			catch(Exception ex)
			{
				FrameSystem.showMsgForm("ķ���ε��������� ��������",new string[] {"",ex.Message});
			}	
		}

        /// <summary>
        /// ķ���γ���-�˾� ����(�������������-�����ּ�)
        /// </summary>
        private void DeleteCampaignPns()
        {
            //StatusMessage("ķ���γ��� �˾� ������ �����մϴ�.");

            //DialogResult result = MessageBox.Show("�ش� ķ���ε�����(�˾�) ������ ���� �Ͻðڽ��ϱ�?", "ķ���ΰ���",
            //    MessageBoxButtons.YesNo, MessageBoxIcon.Question,
            //    MessageBoxDefaultButton.Button2);

            //if (result == DialogResult.No) return;

            //grdExItemList.UpdateData();

            //try
            //{
            //    int SetCount = 0;

            //    for (int i = 0; i < campaignDs.CampaignPns.Rows.Count; i++)
            //    {
            //        DataRow row = campaignDs.CampaignPns.Rows[i];

            //        if (row["CheckYn"].ToString().Equals("True"))
            //        {
            //            campaignModel.Init();
            //            campaignModel.CampaignCode = keyCampaign;
            //            campaignModel.ItemNo = row["PopupID"].ToString();

            //            new CampaignManager(systemModel, commonModel).SetCampaignPnsDelete(campaignModel);

            //            if (campaignModel.ResultCD.Equals("0000"))
            //            {
            //                SetCount++;
            //            }
            //        }
            //    }

            //    if (SetCount > 0)
            //    {
            //        DisableButton();
            //        SearchContractItem();
            //        InitButton();
            //    }

            //    StatusMessage("ķ���ε�����-�˾� ������ �����Ǿ����ϴ�.");
            //}
            //catch (FrameException fe)
            //{
            //    FrameSystem.showMsgForm("ķ���ε�����-�˾����� ��������", new string[] { fe.ErrCode, fe.ResultMsg });
            //}
            //catch (Exception ex)
            //{
            //    FrameSystem.showMsgForm("ķ���ε�����-�˾����� ��������", new string[] { "", ex.Message });
            //}
        }

		private void ClearListCheck()
		{

			// üũ�� ��� �׸��� Ŭ����
			grdExItemList.UnCheckAllRecords();
			grdExItemList.UpdateData();
				   
			// ������ Ŭ����
			for(int i=0;i < dtItem.Rows.Count;i++)
			{
				dtItem.Rows[i].BeginEdit();
				dtItem.Rows[i]["CheckYn"]="False";
				dtItem.Rows[i].EndEdit();
			}
		}
		
		/// <summary>
		/// ������� �������� ��Ʈ
		/// </summary>
		private void SetDetailText()
		{
			int curRow = cm.Position;

			if(curRow >= 0)
			{
				keyContractSeq = dt.Rows[cm.Position]["ContractSeq"].ToString();
				MediaCode      = dt.Rows[cm.Position]["MediaCode"].ToString();				
				IsAdding       = false;

				SearchCampaign();			// �ش� ��࿡ ������ ķ�������� �о����
				SearchContractItem();		//	ķ������ �� ������ �о����
				SetTextReadOnly();           
			}
									  
			StatusMessage("�غ�");
		}

		/// <summary>
		/// ������� �������� ��Ʈ
		/// </summary>
		private void SetCampaignText()
		{
			keyCampaign = "";
			int curRow = cmCampaign.Position;

			if(curRow >= 0)
			{
				keyCampaign			= dtCampaign.Rows[curRow]["CampaignCode"].ToString();
				ebCampaignName.Text = dtCampaign.Rows[curRow]["CampaignName"].ToString();		
				ebPrice.Value		= Convert.ToInt32( dtCampaign.Rows[curRow]["Price"].ToString() ) ;

                              
                string UseYn		= dtCampaign.Rows[curRow]["UseYn"].ToString();
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
								
				SearchContractItem();		//��������		
				
				IsAdding = false;
				SetTextReadOnly();           
			}
									  
			StatusMessage("�غ�");
		}

		private void ResetDetailText()
		{
			//keyContractSeq = "";
			ebCampaignName.Text = "";		
			ebPrice.Value = 0;

           
		}
		
		/// <summary>
		/// ������ ReadOnly
		/// </summary>
		private void SetTextReadOnly()
		{
			//        btnAdvertise.Enabled = true;
			btnItem.Enabled = true;			
		}

		/// <summary>
		/// ������ ����������
		/// </summary>
		private void ResetTextReadOnly()
		{
			//            btnAdvertise.Enabled = false;
			btnItem.Enabled = true;			
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
		
		#region �ܺγ��� �޼ҵ�
		/// <summary>
		/// ���õ� Row���� �Է½�Ŵ
		/// </summary>
		/// <param name="dtc"></param>
		public void adOn_AddContract(CampaignModel campaignModel )
		{
			MediaCode       = campaignModel.MediaCode;
			RapCode         = campaignModel.RapCode;
			AgencyCode      = campaignModel.AgencyCode;
			AdvertiserCode  = campaignModel.AdvertiserCode;

//			ebMedia.Text        = campaignModel.MediaName;
//			ebRap.Text          = campaignModel.RapName;
//			ebAgency.Text       = campaignModel.AgencyName;
//			ebAdvertiser.Text   = campaignModel.AdvertiserName;
		}

		#endregion

		public void ReloadList()
		{
			SearchContractItem();
		}

        /// <summary>
        /// ķ���� ���� ����/�˾� �߰���
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
		private void btnItem_Click(object sender, System.EventArgs e)
		{
			DisableButton();

            if (tbItem.SelectedTabPage == tbp_Ad)
            {
                // ���� ������ ���
                ItemAd_pForm pForm = new ItemAd_pForm(this);

                // ��ü�ڵ��Ʈ
                pForm.keyMediaCode = MediaCode;
                pForm.keyCampaign = keyCampaign;
                pForm.keyContractSeq = keyContractSeq;
                pForm.ShowDialog();
                pForm.Dispose();
                pForm = null;
            }
            else
            { 
                // �˾��� ������ ���
                PnsAd_pForm pForm = new PnsAd_pForm(this);

                pForm.keyMediaCode = MediaCode;
                pForm.keyCampaign = keyCampaign;
                pForm.keyContractSeq = keyContractSeq;
                pForm.ShowDialog();
                pForm.Dispose();
                pForm = null;
            }

			InitButton();		
		}

		private void grdExItemList_ColumnHeaderClick(object sender, Janus.Windows.GridEX.ColumnActionEventArgs e)
		{
			//�÷�Index 0(üũ�ڽ��÷���)�� �ƴϸ� ���������� ó��.
			if(e.Column.Index != 0)
			{
				return;
			}
            
			//ColumnHeader Click�ÿ� dt Setting 
			DataRow[] foundRows = dtItem.Select("CheckYn = 'False'");
         
			// if(grdExItemList.CurrentColumn.Position == 0){
			if( foundRows.Length == 0 )
			{
				for(int i=0;i < dtItem.Rows.Count;i++)
				{
					dtItem.Rows[i].BeginEdit();
					dtItem.Rows[i]["CheckYn"]="False";
					dtItem.Rows[i].EndEdit();
				}
			}
			else
			{
				for(int i=0;i < dtItem.Rows.Count;i++)
				{
					dtItem.Rows[i].BeginEdit();
					dtItem.Rows[i]["CheckYn"]="True";
					dtItem.Rows[i].EndEdit();
				}
			}
		}

        private void grdExItemList_CellValueChanged(object sender, Janus.Windows.GridEX.ColumnActionEventArgs e)
        {
            if (e.Column.EditType == Janus.Windows.GridEX.EditType.CheckBox)
            {
                int curRow = cmItem.Position;
                if (curRow >= 0)
                {
                    dtItem.Rows[curRow].BeginEdit();
                    dtItem.Rows[curRow]["CheckYn"] = grdExItemList.GetValue(e.Column).ToString();
                    dtItem.Rows[curRow].EndEdit();
                }
            }
        }
	}
}

