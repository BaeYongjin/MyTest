// ===============================================================================
//
// AdTypeMoniteringControl.cs
//
// �����ϰ�����Ʈ ��Ʈ���� �����մϴ�. 
//
// ===============================================================================
// Release history
// 2007.10.08 RH.Jung ������½� �̿��ڼ� �� Reach�� ǥ�þ���
// ===============================================================================
// Copyright (C) 2007 G-Inno Systems Inc.
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
using System.Text;

using WinFramework.Misc;
using WinFramework.Base;
using WinFramework.Data;

using AdManagerModel;
using Excel = Microsoft.Office.Interop.Excel; // ���� ����
using System.Reflection;

namespace AdManagerClient
{
	/// <summary>
	/// ���� ��û��Ȳ ���� ���� ��Ʈ��
	/// </summary>
    public class AdTypeMoniteringControl : System.Windows.Forms.UserControl, IUserControl
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
		AdTypeMoniteringModel adTypeMoniteringModel = new AdTypeMoniteringModel();	// �����ϰ�����Ʈ ��
		
		// ȭ��ó���� ����
		bool canRead			  = false;

		CurrencyManager cm        = null;					// ������ �׸����� ���濡 ���� �����ͼ� ������ ���Ͽ�			
		DataTable       dt        = null;

		// Key ������
		string  keyReportDay    = "";
		string  keyAdType       = "";
		string  keyAdTypeName   = "";
        string  keyRap          = "";

		bool canPrint  = false;

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

		private System.Windows.Forms.Panel pnlSearch;
		private Janus.Windows.EditControls.UIButton btnSearch;
		private Janus.Windows.UI.Dock.UIPanel uiPanelSearch;
		private Janus.Windows.UI.Dock.UIPanelInnerContainer uiPanelSearchContainer;
		private Janus.Windows.UI.Dock.UIPanel uiPanelList;
		private Janus.Windows.UI.Dock.UIPanelInnerContainer uiPanelListContainer;
		private Janus.Windows.UI.Dock.UIPanelGroup uiPanelChoiceAdSchedule;
		private System.Windows.Forms.Label label1;
		private Janus.Windows.GridEX.EditControls.EditBox editBox1;
		private System.Windows.Forms.Panel panMenuSchedule;
		private System.Windows.Forms.Panel panel2;
		private System.Windows.Forms.Panel panel3;
		private System.Windows.Forms.Label lbSearchDate;
		private Janus.Windows.EditControls.UIButton btnExcel;
		private System.Data.DataView dvReport;
		private Janus.Windows.CalendarCombo.CalendarCombo cbSearchDay;
		private Janus.Windows.GridEX.GridEX grdExRepoert;
		private Janus.Windows.UI.Dock.UIPanelManager uiPM;
		private Janus.Windows.GridEX.GridEX grdExRpt;
		private Janus.Windows.UI.Dock.UIPanel uiPanelList1;
		private Janus.Windows.UI.Dock.UIPanelInnerContainer uiPanelList1Container;
		private Janus.Windows.UI.Dock.UIPanelGroup uiPanelGroup1;
		private System.Windows.Forms.Panel panel1;
		private System.Data.DataView dvAdItem;
		private AdManagerClient._51_ReportSummaryAd._08_AdTypeMonitering.AdTypeMoniteringDs adTypeMoniteringDs;
		private Janus.Windows.GridEX.GridEX gridEX1;
        private Janus.Windows.EditControls.UIComboBox cbSearchRap;
		private System.ComponentModel.IContainer components;

		public AdTypeMoniteringControl()
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
			Janus.Windows.GridEX.GridEXLayout grdExRpt_DesignTimeLayout = new Janus.Windows.GridEX.GridEXLayout();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AdTypeMoniteringControl));
			Janus.Windows.GridEX.GridEXLayout gridEX1_DesignTimeLayout = new Janus.Windows.GridEX.GridEXLayout();
			this.uiPM = new Janus.Windows.UI.Dock.UIPanelManager(this.components);
			this.uiPanelChoiceAdSchedule = new Janus.Windows.UI.Dock.UIPanelGroup();
			this.uiPanelSearch = new Janus.Windows.UI.Dock.UIPanel();
			this.uiPanelSearchContainer = new Janus.Windows.UI.Dock.UIPanelInnerContainer();
			this.pnlSearch = new System.Windows.Forms.Panel();
			this.cbSearchRap = new Janus.Windows.EditControls.UIComboBox();
			this.cbSearchDay = new Janus.Windows.CalendarCombo.CalendarCombo();
			this.lbSearchDate = new System.Windows.Forms.Label();
			this.btnSearch = new Janus.Windows.EditControls.UIButton();
			this.btnExcel = new Janus.Windows.EditControls.UIButton();
			this.uiPanelGroup1 = new Janus.Windows.UI.Dock.UIPanelGroup();
			this.uiPanelList = new Janus.Windows.UI.Dock.UIPanel();
			this.uiPanelListContainer = new Janus.Windows.UI.Dock.UIPanelInnerContainer();
			this.grdExRpt = new Janus.Windows.GridEX.GridEX();
			this.dvReport = new System.Data.DataView();
			this.adTypeMoniteringDs = new AdManagerClient._51_ReportSummaryAd._08_AdTypeMonitering.AdTypeMoniteringDs();
			this.uiPanelList1 = new Janus.Windows.UI.Dock.UIPanel();
			this.uiPanelList1Container = new Janus.Windows.UI.Dock.UIPanelInnerContainer();
			this.panel1 = new System.Windows.Forms.Panel();
			this.gridEX1 = new Janus.Windows.GridEX.GridEX();
			this.dvAdItem = new System.Data.DataView();
			this.grdExRepoert = new Janus.Windows.GridEX.GridEX();
			this.panel3 = new System.Windows.Forms.Panel();
			this.panMenuSchedule = new System.Windows.Forms.Panel();
			this.panel2 = new System.Windows.Forms.Panel();
			this.label1 = new System.Windows.Forms.Label();
			this.editBox1 = new Janus.Windows.GridEX.EditControls.EditBox();
			((System.ComponentModel.ISupportInitialize)(this.uiPM)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.uiPanelChoiceAdSchedule)).BeginInit();
			this.uiPanelChoiceAdSchedule.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.uiPanelSearch)).BeginInit();
			this.uiPanelSearch.SuspendLayout();
			this.uiPanelSearchContainer.SuspendLayout();
			this.pnlSearch.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.uiPanelGroup1)).BeginInit();
			this.uiPanelGroup1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.uiPanelList)).BeginInit();
			this.uiPanelList.SuspendLayout();
			this.uiPanelListContainer.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.grdExRpt)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.dvReport)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.adTypeMoniteringDs)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.uiPanelList1)).BeginInit();
			this.uiPanelList1.SuspendLayout();
			this.uiPanelList1Container.SuspendLayout();
			this.panel1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.gridEX1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.dvAdItem)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.grdExRepoert)).BeginInit();
			this.panMenuSchedule.SuspendLayout();
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
			this.uiPanelChoiceAdSchedule.Id = new System.Guid("b162c53a-7940-47cd-a0ed-9762c59c50d8");
			this.uiPanelChoiceAdSchedule.StaticGroup = true;
			this.uiPanelSearch.Id = new System.Guid("afb3b7d0-aa79-443c-a1e8-3baba38f7a4d");
			this.uiPanelChoiceAdSchedule.Panels.Add(this.uiPanelSearch);
			this.uiPanelGroup1.Id = new System.Guid("4c1253f5-927a-451c-a91f-16970d28ed20");
			this.uiPanelGroup1.StaticGroup = true;
			this.uiPanelList.Id = new System.Guid("8e3140b9-bfcb-43af-b84e-2ee6cddf51dc");
			this.uiPanelGroup1.Panels.Add(this.uiPanelList);
			this.uiPanelList1.Id = new System.Guid("f75f97ec-19d0-4a97-a19b-7f30f998a81f");
			this.uiPanelGroup1.Panels.Add(this.uiPanelList1);
			this.uiPanelChoiceAdSchedule.Panels.Add(this.uiPanelGroup1);
			this.uiPM.Panels.Add(this.uiPanelChoiceAdSchedule);
			// 
			// Design Time Panel Info:
			// 
			this.uiPM.BeginPanelInfo();
			this.uiPM.AddDockPanelInfo(new System.Guid("b162c53a-7940-47cd-a0ed-9762c59c50d8"), Janus.Windows.UI.Dock.PanelGroupStyle.HorizontalTiles, Janus.Windows.UI.Dock.PanelDockStyle.Fill, true, new System.Drawing.Size(1010, 677), true);
			this.uiPM.AddDockPanelInfo(new System.Guid("afb3b7d0-aa79-443c-a1e8-3baba38f7a4d"), new System.Guid("b162c53a-7940-47cd-a0ed-9762c59c50d8"), 40, true);
			this.uiPM.AddDockPanelInfo(new System.Guid("4c1253f5-927a-451c-a91f-16970d28ed20"), new System.Guid("b162c53a-7940-47cd-a0ed-9762c59c50d8"), Janus.Windows.UI.Dock.PanelGroupStyle.HorizontalTiles, true, 591, true);
			this.uiPM.AddDockPanelInfo(new System.Guid("8e3140b9-bfcb-43af-b84e-2ee6cddf51dc"), new System.Guid("4c1253f5-927a-451c-a91f-16970d28ed20"), 202, true);
			this.uiPM.AddDockPanelInfo(new System.Guid("f75f97ec-19d0-4a97-a19b-7f30f998a81f"), new System.Guid("4c1253f5-927a-451c-a91f-16970d28ed20"), 386, true);
			this.uiPM.AddFloatingPanelInfo(new System.Guid("b162c53a-7940-47cd-a0ed-9762c59c50d8"), Janus.Windows.UI.Dock.PanelGroupStyle.HorizontalTiles, true, new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
			this.uiPM.AddFloatingPanelInfo(new System.Guid("8e3140b9-bfcb-43af-b84e-2ee6cddf51dc"), new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
			this.uiPM.AddFloatingPanelInfo(new System.Guid("b2973a57-d3e1-4d80-9387-7a6dc3332542"), new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
			this.uiPM.AddFloatingPanelInfo(new System.Guid("afb3b7d0-aa79-443c-a1e8-3baba38f7a4d"), new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
			this.uiPM.AddFloatingPanelInfo(new System.Guid("18e78aea-bf80-4094-94f6-0d7d6006bb3d"), Janus.Windows.UI.Dock.PanelGroupStyle.VerticalTiles, true, new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
			this.uiPM.AddFloatingPanelInfo(new System.Guid("02c920f2-57c5-4aa8-bf42-309880d3314e"), new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
			this.uiPM.AddFloatingPanelInfo(new System.Guid("f40b18e5-7e83-4164-be4f-97c23451198d"), new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
			this.uiPM.AddFloatingPanelInfo(new System.Guid("f94be314-c212-42b8-b676-497c4d5f5485"), new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
			this.uiPM.AddFloatingPanelInfo(new System.Guid("b518f914-abba-4e8f-9943-c509cd6f40cf"), Janus.Windows.UI.Dock.PanelGroupStyle.HorizontalTiles, true, new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
			this.uiPM.AddFloatingPanelInfo(new System.Guid("3392f1f8-653a-4a1d-ad4f-88640d56783e"), Janus.Windows.UI.Dock.PanelGroupStyle.HorizontalTiles, true, new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
			this.uiPM.AddFloatingPanelInfo(new System.Guid("040f3a2a-5696-47a2-8434-ab7bd5458070"), Janus.Windows.UI.Dock.PanelGroupStyle.HorizontalTiles, true, new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
			this.uiPM.AddFloatingPanelInfo(new System.Guid("20cbe533-a847-470d-953c-ec60518e1a19"), Janus.Windows.UI.Dock.PanelGroupStyle.HorizontalTiles, true, new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
			this.uiPM.AddFloatingPanelInfo(new System.Guid("5615b4ad-b726-4c5f-b82f-758e4aebd78c"), Janus.Windows.UI.Dock.PanelGroupStyle.HorizontalTiles, true, new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
			this.uiPM.AddFloatingPanelInfo(new System.Guid("4c1253f5-927a-451c-a91f-16970d28ed20"), Janus.Windows.UI.Dock.PanelGroupStyle.HorizontalTiles, true, new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
			this.uiPM.AddFloatingPanelInfo(new System.Guid("f75f97ec-19d0-4a97-a19b-7f30f998a81f"), new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
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
			this.uiPanelChoiceAdSchedule.Text = "�����������͸� ����";
			// 
			// uiPanelSearch
			// 
			this.uiPanelSearch.AllowResize = Janus.Windows.UI.InheritableBoolean.False;
			this.uiPanelSearch.CaptionVisible = Janus.Windows.UI.InheritableBoolean.False;
			this.uiPanelSearch.CloseButtonVisible = Janus.Windows.UI.InheritableBoolean.False;
			this.uiPanelSearch.InnerContainer = this.uiPanelSearchContainer;
			this.uiPanelSearch.Location = new System.Drawing.Point(0, 22);
			this.uiPanelSearch.Name = "uiPanelSearch";
			this.uiPanelSearch.Size = new System.Drawing.Size(1010, 41);
			this.uiPanelSearch.TabIndex = 4;
			this.uiPanelSearch.Text = "�˻�";
			// 
			// uiPanelSearchContainer
			// 
			this.uiPanelSearchContainer.Controls.Add(this.pnlSearch);
			this.uiPanelSearchContainer.Location = new System.Drawing.Point(1, 1);
			this.uiPanelSearchContainer.Name = "uiPanelSearchContainer";
			this.uiPanelSearchContainer.Size = new System.Drawing.Size(1008, 39);
			this.uiPanelSearchContainer.TabIndex = 0;
			// 
			// pnlSearch
			// 
			this.pnlSearch.BackColor = System.Drawing.SystemColors.Window;
			this.pnlSearch.Controls.Add(this.cbSearchRap);
			this.pnlSearch.Controls.Add(this.cbSearchDay);
			this.pnlSearch.Controls.Add(this.lbSearchDate);
			this.pnlSearch.Controls.Add(this.btnSearch);
			this.pnlSearch.Controls.Add(this.btnExcel);
			this.pnlSearch.Dock = System.Windows.Forms.DockStyle.Fill;
			this.pnlSearch.Location = new System.Drawing.Point(0, 0);
			this.pnlSearch.Name = "pnlSearch";
			this.pnlSearch.Size = new System.Drawing.Size(1008, 39);
			this.pnlSearch.TabIndex = 3;
			// 
			// cbSearchRap
			// 
			this.cbSearchRap.BackColor = System.Drawing.Color.WhiteSmoke;
			this.cbSearchRap.ComboStyle = Janus.Windows.EditControls.ComboStyle.DropDownList;
			this.cbSearchRap.Location = new System.Drawing.Point(216, 9);
			this.cbSearchRap.Name = "cbSearchRap";
			this.cbSearchRap.Size = new System.Drawing.Size(120, 20);
			this.cbSearchRap.TabIndex = 16;
			this.cbSearchRap.Text = "�̵�����";
			this.cbSearchRap.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
			// 
			// cbSearchDay
			// 
			// 
			// 
			// 
			this.cbSearchDay.DropDownCalendar.FirstMonth = new System.DateTime(2007, 8, 1, 0, 0, 0, 0);
			this.cbSearchDay.DropDownCalendar.VisualStyle = Janus.Windows.CalendarCombo.VisualStyle.Office2007;
			this.cbSearchDay.Location = new System.Drawing.Point(80, 9);
			this.cbSearchDay.Name = "cbSearchDay";
			this.cbSearchDay.Size = new System.Drawing.Size(120, 20);
			this.cbSearchDay.TabIndex = 4;
			this.cbSearchDay.Value = new System.DateTime(2007, 9, 14, 0, 0, 0, 0);
			this.cbSearchDay.VisualStyle = Janus.Windows.CalendarCombo.VisualStyle.Office2007;
			// 
			// lbSearchDate
			// 
			this.lbSearchDate.Location = new System.Drawing.Point(8, 9);
			this.lbSearchDate.Name = "lbSearchDate";
			this.lbSearchDate.Size = new System.Drawing.Size(72, 21);
			this.lbSearchDate.TabIndex = 14;
			this.lbSearchDate.Text = "���������";
			this.lbSearchDate.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// btnSearch
			// 
			this.btnSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnSearch.Enabled = false;
			this.btnSearch.Location = new System.Drawing.Point(783, 8);
			this.btnSearch.Name = "btnSearch";
			this.btnSearch.Size = new System.Drawing.Size(104, 24);
			this.btnSearch.TabIndex = 6;
			this.btnSearch.Text = "�� ȸ";
			this.btnSearch.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
			this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
			// 
			// btnExcel
			// 
			this.btnExcel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnExcel.Enabled = false;
			this.btnExcel.Location = new System.Drawing.Point(895, 8);
			this.btnExcel.Name = "btnExcel";
			this.btnExcel.Size = new System.Drawing.Size(104, 24);
			this.btnExcel.TabIndex = 7;
			this.btnExcel.Text = "EXCEL ���";
			this.btnExcel.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
			this.btnExcel.Click += new System.EventHandler(this.btnExcel_Click);
			// 
			// uiPanelGroup1
			// 
			this.uiPanelGroup1.CaptionHeight = 0;
			this.uiPanelGroup1.Location = new System.Drawing.Point(0, 67);
			this.uiPanelGroup1.Name = "uiPanelGroup1";
			this.uiPanelGroup1.Size = new System.Drawing.Size(1010, 610);
			this.uiPanelGroup1.TabIndex = 5;
			// 
			// uiPanelList
			// 
			this.uiPanelList.BackColor = System.Drawing.SystemColors.Window;
			this.uiPanelList.CaptionStyle = Janus.Windows.UI.Dock.PanelCaptionStyle.Dark;
			this.uiPanelList.CaptionVisible = Janus.Windows.UI.InheritableBoolean.True;
			this.uiPanelList.InnerContainer = this.uiPanelListContainer;
			this.uiPanelList.Location = new System.Drawing.Point(0, -1);
			this.uiPanelList.MinimumSize = new System.Drawing.Size(-1, 100);
			this.uiPanelList.Name = "uiPanelList";
			this.uiPanelList.Size = new System.Drawing.Size(1010, 209);
			this.uiPanelList.TabIndex = 4;
			this.uiPanelList.Text = "�ϰ� �������� ��Ȳ";
			// 
			// uiPanelListContainer
			// 
			this.uiPanelListContainer.BackColor = System.Drawing.SystemColors.Window;
			this.uiPanelListContainer.Controls.Add(this.grdExRpt);
			this.uiPanelListContainer.Location = new System.Drawing.Point(1, 23);
			this.uiPanelListContainer.Name = "uiPanelListContainer";
			this.uiPanelListContainer.Size = new System.Drawing.Size(1008, 185);
			this.uiPanelListContainer.TabIndex = 0;
			// 
			// grdExRpt
			// 
			this.grdExRpt.AllowEdit = Janus.Windows.GridEX.InheritableBoolean.False;
			this.grdExRpt.AlternatingColors = true;
			this.grdExRpt.AlternatingRowFormatStyle.BackgroundImageDrawMode = Janus.Windows.GridEX.BackgroundImageDrawMode.Tile;
			this.grdExRpt.BorderStyle = Janus.Windows.GridEX.BorderStyle.None;
			this.grdExRpt.DataSource = this.dvReport;
			grdExRpt_DesignTimeLayout.LayoutString = resources.GetString("grdExRpt_DesignTimeLayout.LayoutString");
			this.grdExRpt.DesignTimeLayout = grdExRpt_DesignTimeLayout;
			this.grdExRpt.Dock = System.Windows.Forms.DockStyle.Fill;
			this.grdExRpt.EditorsControlStyle.ButtonAppearance = Janus.Windows.GridEX.ButtonAppearance.Regular;
			this.grdExRpt.EmptyRows = true;
			this.grdExRpt.FocusCellFormatStyle.BackColor = System.Drawing.SystemColors.Highlight;
			this.grdExRpt.FocusCellFormatStyle.ForeColor = System.Drawing.Color.White;
			this.grdExRpt.FocusStyle = Janus.Windows.GridEX.FocusStyle.None;
			this.grdExRpt.FrozenColumns = 2;
			this.grdExRpt.GridLineColor = System.Drawing.Color.Silver;
			this.grdExRpt.GridLines = Janus.Windows.GridEX.GridLines.Vertical;
			this.grdExRpt.GridLineStyle = Janus.Windows.GridEX.GridLineStyle.Solid;
			this.grdExRpt.GroupByBoxVisible = false;
			this.grdExRpt.HeaderFormatStyle.TextAlignment = Janus.Windows.GridEX.TextAlignment.Center;
			this.grdExRpt.HideSelection = Janus.Windows.GridEX.HideSelection.HighlightInactive;
			this.grdExRpt.Location = new System.Drawing.Point(0, 0);
			this.grdExRpt.Name = "grdExRpt";
			this.grdExRpt.SelectedInactiveFormatStyle.BackColor = System.Drawing.Color.Gold;
			this.grdExRpt.Size = new System.Drawing.Size(1008, 185);
			this.grdExRpt.TabIndex = 5;
			this.grdExRpt.TabKeyBehavior = Janus.Windows.GridEX.TabKeyBehavior.ControlNavigation;
			this.grdExRpt.ThemedAreas = ((Janus.Windows.GridEX.ThemedArea)(((((((((Janus.Windows.GridEX.ThemedArea.ScrollBars | Janus.Windows.GridEX.ThemedArea.EditControls) 
            | Janus.Windows.GridEX.ThemedArea.Headers) 
            | Janus.Windows.GridEX.ThemedArea.GroupByBox) 
            | Janus.Windows.GridEX.ThemedArea.GroupRows) 
            | Janus.Windows.GridEX.ThemedArea.ControlBorder) 
            | Janus.Windows.GridEX.ThemedArea.Cards) 
            | Janus.Windows.GridEX.ThemedArea.Gridlines) 
            | Janus.Windows.GridEX.ThemedArea.CheckBoxes)));
			this.grdExRpt.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
			this.grdExRpt.Enter += new System.EventHandler(this.OnGrdRowChanged);
			// 
			// dvReport
			// 
			this.dvReport.Table = this.adTypeMoniteringDs.AdTypeMaster;
			// 
			// adTypeMoniteringDs
			// 
			this.adTypeMoniteringDs.DataSetName = "AdTypeMoniteringDs";
			this.adTypeMoniteringDs.Locale = new System.Globalization.CultureInfo("en-US");
			this.adTypeMoniteringDs.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
			// 
			// uiPanelList1
			// 
			this.uiPanelList1.InnerContainer = this.uiPanelList1Container;
			this.uiPanelList1.Location = new System.Drawing.Point(0, 212);
			this.uiPanelList1.Name = "uiPanelList1";
			this.uiPanelList1.Size = new System.Drawing.Size(1010, 398);
			this.uiPanelList1.TabIndex = 4;
			this.uiPanelList1.Text = "������";
			// 
			// uiPanelList1Container
			// 
			this.uiPanelList1Container.Controls.Add(this.panel1);
			this.uiPanelList1Container.Location = new System.Drawing.Point(1, 23);
			this.uiPanelList1Container.Name = "uiPanelList1Container";
			this.uiPanelList1Container.Size = new System.Drawing.Size(1008, 374);
			this.uiPanelList1Container.TabIndex = 0;
			// 
			// panel1
			// 
			this.panel1.BackColor = System.Drawing.SystemColors.Window;
			this.panel1.Controls.Add(this.gridEX1);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel1.Location = new System.Drawing.Point(0, 0);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(1008, 374);
			this.panel1.TabIndex = 4;
			// 
			// gridEX1
			// 
			this.gridEX1.AllowEdit = Janus.Windows.GridEX.InheritableBoolean.False;
			this.gridEX1.AlternatingColors = true;
			this.gridEX1.AlternatingRowFormatStyle.BackgroundImageDrawMode = Janus.Windows.GridEX.BackgroundImageDrawMode.Tile;
			this.gridEX1.BorderStyle = Janus.Windows.GridEX.BorderStyle.None;
			this.gridEX1.DataSource = this.dvAdItem;
			gridEX1_DesignTimeLayout.LayoutString = resources.GetString("gridEX1_DesignTimeLayout.LayoutString");
			this.gridEX1.DesignTimeLayout = gridEX1_DesignTimeLayout;
			this.gridEX1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.gridEX1.EditorsControlStyle.ButtonAppearance = Janus.Windows.GridEX.ButtonAppearance.Regular;
			this.gridEX1.EmptyRows = true;
			this.gridEX1.FocusCellFormatStyle.BackColor = System.Drawing.SystemColors.Highlight;
			this.gridEX1.FocusCellFormatStyle.ForeColor = System.Drawing.Color.White;
			this.gridEX1.FocusStyle = Janus.Windows.GridEX.FocusStyle.None;
			this.gridEX1.FrozenColumns = 2;
			this.gridEX1.GridLineColor = System.Drawing.Color.Silver;
			this.gridEX1.GridLines = Janus.Windows.GridEX.GridLines.Vertical;
			this.gridEX1.GridLineStyle = Janus.Windows.GridEX.GridLineStyle.Solid;
			this.gridEX1.GroupByBoxVisible = false;
			this.gridEX1.HeaderFormatStyle.TextAlignment = Janus.Windows.GridEX.TextAlignment.Center;
			this.gridEX1.HideSelection = Janus.Windows.GridEX.HideSelection.HighlightInactive;
			this.gridEX1.Location = new System.Drawing.Point(0, 0);
			this.gridEX1.Name = "gridEX1";
			this.gridEX1.SelectedInactiveFormatStyle.BackColor = System.Drawing.Color.Gold;
			this.gridEX1.Size = new System.Drawing.Size(1008, 374);
			this.gridEX1.TabIndex = 6;
			this.gridEX1.TabKeyBehavior = Janus.Windows.GridEX.TabKeyBehavior.ControlNavigation;
			this.gridEX1.ThemedAreas = ((Janus.Windows.GridEX.ThemedArea)(((((((((Janus.Windows.GridEX.ThemedArea.ScrollBars | Janus.Windows.GridEX.ThemedArea.EditControls) 
            | Janus.Windows.GridEX.ThemedArea.Headers) 
            | Janus.Windows.GridEX.ThemedArea.GroupByBox) 
            | Janus.Windows.GridEX.ThemedArea.GroupRows) 
            | Janus.Windows.GridEX.ThemedArea.ControlBorder) 
            | Janus.Windows.GridEX.ThemedArea.Cards) 
            | Janus.Windows.GridEX.ThemedArea.Gridlines) 
            | Janus.Windows.GridEX.ThemedArea.CheckBoxes)));
			this.gridEX1.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
			// 
			// dvAdItem
			// 
			this.dvAdItem.Table = this.adTypeMoniteringDs.AdTypeDetail;
			// 
			// grdExRepoert
			// 
			this.grdExRepoert.Location = new System.Drawing.Point(0, 0);
			this.grdExRepoert.Name = "grdExRepoert";
			this.grdExRepoert.Size = new System.Drawing.Size(400, 376);
			this.grdExRepoert.TabIndex = 0;
			// 
			// panel3
			// 
			this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel3.Location = new System.Drawing.Point(0, 0);
			this.panel3.Name = "panel3";
			this.panel3.Size = new System.Drawing.Size(851, 322);
			this.panel3.TabIndex = 25;
			// 
			// panMenuSchedule
			// 
			this.panMenuSchedule.BackColor = System.Drawing.SystemColors.Window;
			this.panMenuSchedule.Controls.Add(this.panel2);
			this.panMenuSchedule.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panMenuSchedule.Location = new System.Drawing.Point(0, 0);
			this.panMenuSchedule.Name = "panMenuSchedule";
			this.panMenuSchedule.Size = new System.Drawing.Size(851, 81);
			this.panMenuSchedule.TabIndex = 6;
			// 
			// panel2
			// 
			this.panel2.BackColor = System.Drawing.SystemColors.Window;
			this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel2.Location = new System.Drawing.Point(0, 0);
			this.panel2.Name = "panel2";
			this.panel2.Size = new System.Drawing.Size(851, 81);
			this.panel2.TabIndex = 4;
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
			this.editBox1.Size = new System.Drawing.Size(100, 21);
			this.editBox1.TabIndex = 0;
			this.editBox1.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
			// 
			// AdTypeMoniteringControl
			// 
			this.Controls.Add(this.uiPanelChoiceAdSchedule);
			this.Font = global::AdManagerClient.Properties.Settings.Default.Font1;
			this.Name = "AdTypeMoniteringControl";
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
			((System.ComponentModel.ISupportInitialize)(this.uiPanelGroup1)).EndInit();
			this.uiPanelGroup1.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.uiPanelList)).EndInit();
			this.uiPanelList.ResumeLayout(false);
			this.uiPanelListContainer.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.grdExRpt)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.dvReport)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.adTypeMoniteringDs)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.uiPanelList1)).EndInit();
			this.uiPanelList1.ResumeLayout(false);
			this.uiPanelList1Container.ResumeLayout(false);
			this.panel1.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.gridEX1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.dvAdItem)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.grdExRepoert)).EndInit();
			this.panMenuSchedule.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		#region ��Ʈ�� �ε�
		private void UserControl_Load(object sender, System.EventArgs e)
		{
			// �����Ͱ����� ��ü����
			dt = ((DataView)grdExRpt.DataSource).Table;  
			cm = (CurrencyManager) this.BindingContext[grdExRpt.DataSource]; 
			cm.PositionChanged += new System.EventHandler(OnGrdRowChanged); 
			// ��Ʈ�� �ʱ�ȭ
			InitControl();	
		}

		#endregion


        #region ��Ʈ�� �ʱ�ȭ
        private void InitCombo()
        {
            Init_RapCode();
            cbSearchDay.Value = DateTime.Now.AddDays(0);	
            InitCombo_Level();
        }

        private void Init_RapCode()
        {
            // ���� ��ȸ�Ѵ�.
            MediaRapCodeModel mediaRapCodeModel = new MediaRapCodeModel();
            new MediaRapCodeManager(systemModel, commonModel).GetMediaRapCodeList(mediaRapCodeModel);
			
            if (mediaRapCodeModel.ResultCD.Equals("0000"))
            {
                // �����ͼ¿� ����
                Utility.SetDataTable(adTypeMoniteringDs.MediaRaps, mediaRapCodeModel.MediaRapCodeDataSet);				
            }

            // �˻������� �޺�
            this.cbSearchRap.Items.Clear();
           
            // �޺��ڽ��� ��Ʈ�� �ڵ����� ���� Item�迭�� ����
            Janus.Windows.EditControls.UIComboBoxItem[] comboItems = new Janus.Windows.EditControls.UIComboBoxItem[mediaRapCodeModel.ResultCnt + 1];

            comboItems[0] = new Janus.Windows.EditControls.UIComboBoxItem("�̵�����","00");
			
            for(int i=0;i<mediaRapCodeModel.ResultCnt;i++)
            {
                DataRow row = adTypeMoniteringDs.MediaRaps.Rows[i];

                string val = row["RapCode"].ToString();
                string txt = row["RapName"].ToString();
                comboItems[i+1] = new Janus.Windows.EditControls.UIComboBoxItem(txt,val);
            }
            // �޺��� ��Ʈ
            this.cbSearchRap.Items.AddRange(comboItems);
            this.cbSearchRap.SelectedIndex = 0;

            Application.DoEvents();

        }

        
        private void InitCombo_Level()
        {

            if (commonModel.UserLevel == "30")
            {
                cbSearchRap.SelectedValue = commonModel.RapCode;
                cbSearchRap.ReadOnly = true;
            }


            Application.DoEvents();
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
			}

			InitButton();

			ProgressStop();
		}


		private void InitButton()
		{
			if(canRead)
			{
				btnSearch.Enabled = true;
			}
			
			if(canPrint)
			{
				btnExcel.Enabled = true;
			}

			grdExRpt.Focus();

			Application.DoEvents();
		}
       
		private void DisableButton()
		{
			btnSearch.Enabled	= false;
			btnExcel.Enabled    = false;

			Application.DoEvents();
		}

		#endregion

		#region �׼�ó�� �޼ҵ�

		/// <summary>
		/// <summary>
		/// ��ȸ��ư Ŭ��
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnSearch_Click(object sender, System.EventArgs e)
		{
			DisableButton();
			SearchReport();
			InitButton();
		}

		private void OnGrdRowChanged(object sender, System.EventArgs e)
		{
			if(grdExRpt.RowCount > 0)
			{
				SetDetailText();
				InitButton();
			}
		}

		#endregion

		#region ó���޼ҵ�

		/// <summary>
		/// ����͸� ������ ��ȸ
		/// </summary>
		private void SearchReport()
		{
			StatusMessage("����͸� �������� ��ȸ�մϴ�.");

			ProgressStart();

			try
			{
				// �����͸� �ʱ�ȭ
				adTypeMoniteringModel.Init();

				adTypeMoniteringModel.LogDay	= cbSearchDay.Value.ToString("yyMMdd");
                adTypeMoniteringModel.Rap       = cbSearchRap.SelectedItem.Value.ToString();
				
				uiPanelList.Text = "����͸� ������"; 

				//  �Ϻ� �����û������ȸ ���񽺸� ȣ���Ѵ�.
				new AdTypeMoniteringManager(systemModel, commonModel).GetAdTypeMaster(adTypeMoniteringModel);
		
				if (adTypeMoniteringModel.ResultCD.Equals("0000"))
				{
					Utility.SetDataTable(adTypeMoniteringDs.AdTypeMaster, adTypeMoniteringModel.ReportDataSet);						
					StatusMessage(adTypeMoniteringModel.ResultCnt + "���� ��ȸ�Ǿ����ϴ�.");

					keyReportDay   = cbSearchDay.Value.ToString("yyyy-MM-dd");

					uiPanelList.Text = "����͸� ������ : " + keyReportDay;
										
					canPrint = true;
					SetDetailText();
				}
			}
			catch(FrameException fe)
			{
				FrameSystem.showMsgForm("����͸� ������ ��Ȳ ��ȸ����", new string[] {fe.ErrCode, fe.ResultMsg});
			}
			catch(Exception ex)
			{
				FrameSystem.showMsgForm("����͸� ������ ��Ȳ ��ȸ����",new string[] {"",ex.Message});
			}
			finally
			{
				ProgressStop();
			}
		}

		/// <summary>
		/// ����͸� ������ ��ȸ
		/// </summary>
		private void SearchDetail()
		{
			StatusMessage("����͸� ������ ��ȸ�մϴ�.");

			ProgressStart();

			try
			{
				// �����͸� �ʱ�ȭ
				adTypeMoniteringModel.Init();

				adTypeMoniteringModel.LogDay	= cbSearchDay.Value.ToString("yyMMdd");
				adTypeMoniteringModel.AdType	= keyAdType;
                adTypeMoniteringModel.Rap       = keyRap;
                			

				//  �Ϻ� �����û������ȸ ���񽺸� ȣ���Ѵ�.
				new AdTypeMoniteringManager(systemModel, commonModel).GetAdTypeDetail(adTypeMoniteringModel);
		
				if (adTypeMoniteringModel.ResultCD.Equals("0000"))
				{
					Utility.SetDataTable(adTypeMoniteringDs.AdTypeDetail, adTypeMoniteringModel.ReportDataSet);						
					StatusMessage(adTypeMoniteringModel.ResultCnt + "���� ��ȸ�Ǿ����ϴ�.");

					keyReportDay   = cbSearchDay.Value.ToString("yyyy-MM-dd");
										
					canPrint = true;
				}
			}
			catch(FrameException fe)
			{
				FrameSystem.showMsgForm("����͸� ������ ��Ȳ ��ȸ����", new string[] {fe.ErrCode, fe.ResultMsg});
			}
			catch(Exception ex)
			{
				FrameSystem.showMsgForm("����͸� ������ ��Ȳ ��ȸ����",new string[] {"",ex.Message});
			}
			finally
			{
				ProgressStop();
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
				keyAdType       = dt.Rows[cm.Position]["adType"].ToString();
				keyAdTypeName   = dt.Rows[cm.Position]["adTypeName"].ToString();
                keyRap          = cbSearchRap.SelectedValue.ToString();
                
				SearchDetail();		//��������				
			}
									  
			StatusMessage("�غ�");
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
		private void btnExcel_Click(object sender, System.EventArgs e)
		{	
			Excel.Application xlApp     = null;
			Excel._Workbook   oWB       = null;
			Excel._Worksheet  oSheet    = null;
			Excel.Range       oRng      = null;
			Excel._Worksheet  oSheet_r  = null;
			Excel.Range       oRng_r    = null;
			
			try
			{	
				int ColMax          = 26; // �÷���   				
				int TitleRow        = 1;
				int ConditionRow    = 2;
				int HeaderRow       = 5;
				int DataRow         = 6;
				string StartCol     = "A";
				string EndCol       = "";
				string TitleCol     = "C";
				int DataCount       = 0;
				int CondCount       = 0;
				int HeaderCount     = 0;
				
				// ������ �÷��� �ε�������
				EndCol = GetColumnIndex(ColMax);

				xlApp = new Excel.Application();
				oWB = (Excel._Workbook)(xlApp.Workbooks.Add(Missing.Value));
				oSheet = (Excel._Worksheet)oWB.ActiveSheet;

				// Ÿ��Ʋ �ۼ�
				oSheet.Cells[TitleRow,1] = "�����������͸�";
				oRng = oSheet.get_Range(StartCol+Convert.ToString(TitleRow), TitleCol+Convert.ToString(TitleRow));
				oRng.Merge(true);
				oRng.Font.Bold = true;
				oRng.Font.Size = 16;
				oRng.VerticalAlignment   = Excel.XlVAlign.xlVAlignCenter;			
				oRng.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;		

				// �������� �ۼ�
				oSheet.Cells[ConditionRow+CondCount,1] = "���������";
				oRng = oSheet.get_Range("B"+Convert.ToString(ConditionRow+CondCount), TitleCol+Convert.ToString(ConditionRow+CondCount));
				oRng.Merge(true);
				oSheet.Cells[ConditionRow+CondCount,2] = keyReportDay;
				CondCount++;

                oSheet.Cells[ConditionRow+CondCount,1] = "�̵�";
                oRng = oSheet.get_Range("B"+Convert.ToString(ConditionRow+CondCount), TitleCol+Convert.ToString(ConditionRow+CondCount));
                oRng.Merge(true);
                oSheet.Cells[ConditionRow+CondCount,2] = cbSearchRap.SelectedItem.Text.ToString();
                CondCount++;

				// ���Ǻ� �׵θ�
				oRng = oSheet.get_Range(StartCol+Convert.ToString(ConditionRow), TitleCol+Convert.ToString(ConditionRow+(CondCount-1)));
				oRng.VerticalAlignment   = Excel.XlVAlign.xlVAlignCenter;			
				oRng.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;		
				oRng.Borders.LineStyle =  Excel.XlLineStyle.xlContinuous;	// �׵θ��Ӽ� �Ʒ��� �Ǽ�
				oRng.Borders.Weight    = Excel.XlBorderWeight.xlThin;		// �׵θ��Ӽ� �Ʒ��� ���¼�


				// ��� ���� �ۼ�
				HeaderCount = 1;
				oSheet.Cells[HeaderRow,HeaderCount++] = "Ÿ�Ը�";
				oSheet.Cells[HeaderRow,HeaderCount++] = "�Ѱ�";
                oSheet.Cells[HeaderRow,HeaderCount++] = "0��";
				oSheet.Cells[HeaderRow,HeaderCount++] = "1��";
				oSheet.Cells[HeaderRow,HeaderCount++] = "2��";
				oSheet.Cells[HeaderRow,HeaderCount++] = "3��";
				oSheet.Cells[HeaderRow,HeaderCount++] = "4��";
				oSheet.Cells[HeaderRow,HeaderCount++] = "5��";
				oSheet.Cells[HeaderRow,HeaderCount++] = "6��";
				oSheet.Cells[HeaderRow,HeaderCount++] = "7��";
				oSheet.Cells[HeaderRow,HeaderCount++] = "8��";
				oSheet.Cells[HeaderRow,HeaderCount++] = "9��";
				oSheet.Cells[HeaderRow,HeaderCount++] = "10��";
				oSheet.Cells[HeaderRow,HeaderCount++] = "11��";
				oSheet.Cells[HeaderRow,HeaderCount++] = "12��";
				oSheet.Cells[HeaderRow,HeaderCount++] = "13��";
				oSheet.Cells[HeaderRow,HeaderCount++] = "14��";
				oSheet.Cells[HeaderRow,HeaderCount++] = "15��";
				oSheet.Cells[HeaderRow,HeaderCount++] = "16��";
				oSheet.Cells[HeaderRow,HeaderCount++] = "17��";
				oSheet.Cells[HeaderRow,HeaderCount++] = "18��";
				oSheet.Cells[HeaderRow,HeaderCount++] = "19��";
				oSheet.Cells[HeaderRow,HeaderCount++] = "20��";
				oSheet.Cells[HeaderRow,HeaderCount++] = "21��";
				oSheet.Cells[HeaderRow,HeaderCount++] = "22��";
				oSheet.Cells[HeaderRow,HeaderCount++] = "23��";

				oRng = oSheet.get_Range(StartCol+Convert.ToString(HeaderRow), EndCol+Convert.ToString(HeaderRow));	// ����� ����
				oRng.Font.Bold           = true;							// ��Ʈ ����
                oRng.Font.Size           = 10;
				oRng.VerticalAlignment   = Excel.XlVAlign.xlVAlignCenter;	// �����߾�����
				oRng.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;	// �����߾�����	 
				oRng.Interior.Color      = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.CornflowerBlue);   //�� ���� 
				oRng.Font.Color          = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.White);			//�ؽ�Ʈ��			
				
				DataCount = 0;
				// ������ ����
				for (int inx =0; inx < adTypeMoniteringDs.AdTypeMaster.Rows.Count; inx++)
				{		
					DataRow Row = adTypeMoniteringDs.AdTypeMaster.Rows[inx];

					int ColCnt = 1;

					oSheet.Cells[DataRow+DataCount,ColCnt++] = Row["adTypeName"].ToString();
					oSheet.Cells[DataRow+DataCount,ColCnt++] = Row["Tot"].ToString();
                    oSheet.Cells[DataRow+DataCount,ColCnt++] = Row["H00"].ToString();
					oSheet.Cells[DataRow+DataCount,ColCnt++] = Row["H01"].ToString();
					oSheet.Cells[DataRow+DataCount,ColCnt++] = Row["H02"].ToString();
					oSheet.Cells[DataRow+DataCount,ColCnt++] = Row["H03"].ToString();
					oSheet.Cells[DataRow+DataCount,ColCnt++] = Row["H04"].ToString();
					oSheet.Cells[DataRow+DataCount,ColCnt++] = Row["H05"].ToString();
					oSheet.Cells[DataRow+DataCount,ColCnt++] = Row["H06"].ToString();
					oSheet.Cells[DataRow+DataCount,ColCnt++] = Row["H07"].ToString();
					oSheet.Cells[DataRow+DataCount,ColCnt++] = Row["H08"].ToString();
					oSheet.Cells[DataRow+DataCount,ColCnt++] = Row["H09"].ToString();
					oSheet.Cells[DataRow+DataCount,ColCnt++] = Row["H10"].ToString();
					oSheet.Cells[DataRow+DataCount,ColCnt++] = Row["H11"].ToString();
					oSheet.Cells[DataRow+DataCount,ColCnt++] = Row["H12"].ToString();
					oSheet.Cells[DataRow+DataCount,ColCnt++] = Row["H13"].ToString();
					oSheet.Cells[DataRow+DataCount,ColCnt++] = Row["H14"].ToString();
					oSheet.Cells[DataRow+DataCount,ColCnt++] = Row["H15"].ToString();
					oSheet.Cells[DataRow+DataCount,ColCnt++] = Row["H16"].ToString();
					oSheet.Cells[DataRow+DataCount,ColCnt++] = Row["H17"].ToString();
					oSheet.Cells[DataRow+DataCount,ColCnt++] = Row["H18"].ToString();
					oSheet.Cells[DataRow+DataCount,ColCnt++] = Row["H19"].ToString();
					oSheet.Cells[DataRow+DataCount,ColCnt++] = Row["H20"].ToString();
					oSheet.Cells[DataRow+DataCount,ColCnt++] = Row["H21"].ToString();
					oSheet.Cells[DataRow+DataCount,ColCnt++] = Row["H22"].ToString();
					oSheet.Cells[DataRow+DataCount,ColCnt++] = Row["H23"].ToString();
					DataCount++;
				}

				DataCount--;

				// ������ �ۼ�
				oRng = oSheet.get_Range(StartCol+Convert.ToString(HeaderRow), EndCol+Convert.ToString(DataRow+DataCount));	// �������� ����
                oRng.Font.Size = 9;
				oRng.EntireColumn.AutoFit();					// �������� ũ�⿡ ���� ����ũ�� ����
				oRng.Borders.LineStyle =  Excel.XlLineStyle.xlContinuous;	// �׵θ��Ӽ� �Ʒ��� �Ǽ�
				oRng.Borders.Weight    = Excel.XlBorderWeight.xlThin;		// �׵θ��Ӽ� �Ʒ��� ���¼�
				//�����ȣ �߾�����
				oRng = oSheet.get_Range(GetColumnIndex(1)+Convert.ToString(DataRow), GetColumnIndex(1)+Convert.ToString(DataRow+DataCount)); 
				oRng.HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft;
				
				//�����ȣ �߾�����
				oRng = oSheet.get_Range(GetColumnIndex(2)+Convert.ToString(DataRow), GetColumnIndex(2)+Convert.ToString(DataRow+DataCount)); 
				oRng.HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;
				oRng.NumberFormatLocal = "#,##0";
				
				//��ûȽ�� right����
				oRng = oSheet.get_Range(GetColumnIndex(3)+Convert.ToString(DataRow), GetColumnIndex(3)+Convert.ToString(DataRow+DataCount)); 
				oRng.HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;
				oRng.NumberFormatLocal = "#,##0";

				//��û���� right����
				oRng = oSheet.get_Range(GetColumnIndex(4)+Convert.ToString(DataRow), GetColumnIndex(4)+Convert.ToString(DataRow+DataCount)); 
				oRng.HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;
				oRng.NumberFormatLocal = "#,##0";

				//rate right����
				oRng = oSheet.get_Range(GetColumnIndex(5)+Convert.ToString(DataRow), GetColumnIndex(5)+Convert.ToString(DataRow+DataCount)); 
				oRng.HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;
				oRng.NumberFormatLocal = "#,##0";

				//Cove right����
				oRng = oSheet.get_Range(GetColumnIndex(6)+Convert.ToString(DataRow), GetColumnIndex(6)+Convert.ToString(DataRow+DataCount)); 
				oRng.HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;
				oRng.NumberFormatLocal = "#,##0";

				//Freq right����
				oRng = oSheet.get_Range(GetColumnIndex(7)+Convert.ToString(DataRow), GetColumnIndex(7)+Convert.ToString(DataRow+DataCount)); 
				oRng.HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;
				oRng.NumberFormatLocal = "#,##0";

				oRng = oSheet.get_Range(GetColumnIndex(8)+Convert.ToString(DataRow), GetColumnIndex(8)+Convert.ToString(DataRow+DataCount)); 
				oRng.HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;
				oRng.NumberFormatLocal = "#,##0";

				oRng = oSheet.get_Range(GetColumnIndex(9)+Convert.ToString(DataRow), GetColumnIndex(9)+Convert.ToString(DataRow+DataCount)); 
				oRng.HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;
				oRng.NumberFormatLocal = "#,##0";

				oRng = oSheet.get_Range(GetColumnIndex(10)+Convert.ToString(DataRow), GetColumnIndex(10)+Convert.ToString(DataRow+DataCount)); 
				oRng.HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;
				oRng.NumberFormatLocal = "#,##0";

				oRng = oSheet.get_Range(GetColumnIndex(11)+Convert.ToString(DataRow), GetColumnIndex(11)+Convert.ToString(DataRow+DataCount)); 
				oRng.HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;
				oRng.NumberFormatLocal = "#,##0";

				oRng = oSheet.get_Range(GetColumnIndex(12)+Convert.ToString(DataRow), GetColumnIndex(12)+Convert.ToString(DataRow+DataCount)); 
				oRng.HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;
				oRng.NumberFormatLocal = "#,##0";

				oRng = oSheet.get_Range(GetColumnIndex(13)+Convert.ToString(DataRow), GetColumnIndex(13)+Convert.ToString(DataRow+DataCount)); 
				oRng.HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;
				oRng.NumberFormatLocal = "#,##0";

				oRng = oSheet.get_Range(GetColumnIndex(14)+Convert.ToString(DataRow), GetColumnIndex(14)+Convert.ToString(DataRow+DataCount)); 
				oRng.HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;
				oRng.NumberFormatLocal = "#,##0";

				oRng = oSheet.get_Range(GetColumnIndex(15)+Convert.ToString(DataRow), GetColumnIndex(15)+Convert.ToString(DataRow+DataCount)); 
				oRng.HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;
				oRng.NumberFormatLocal = "#,##0";

				oRng = oSheet.get_Range(GetColumnIndex(16)+Convert.ToString(DataRow), GetColumnIndex(16)+Convert.ToString(DataRow+DataCount)); 
				oRng.HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;
				oRng.NumberFormatLocal = "#,##0";

				oRng = oSheet.get_Range(GetColumnIndex(17)+Convert.ToString(DataRow), GetColumnIndex(17)+Convert.ToString(DataRow+DataCount)); 
				oRng.HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;
				oRng.NumberFormatLocal = "#,##0";

				oRng = oSheet.get_Range(GetColumnIndex(18)+Convert.ToString(DataRow), GetColumnIndex(18)+Convert.ToString(DataRow+DataCount)); 
				oRng.HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;
				oRng.NumberFormatLocal = "#,##0";

				oRng = oSheet.get_Range(GetColumnIndex(19)+Convert.ToString(DataRow), GetColumnIndex(19)+Convert.ToString(DataRow+DataCount)); 
				oRng.HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;
				oRng.NumberFormatLocal = "#,##0";

				oRng = oSheet.get_Range(GetColumnIndex(20)+Convert.ToString(DataRow), GetColumnIndex(20)+Convert.ToString(DataRow+DataCount)); 
				oRng.HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;
				oRng.NumberFormatLocal = "#,##0";

				oRng = oSheet.get_Range(GetColumnIndex(21)+Convert.ToString(DataRow), GetColumnIndex(21)+Convert.ToString(DataRow+DataCount)); 
				oRng.HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;
				oRng.NumberFormatLocal = "#,##0";

				oRng = oSheet.get_Range(GetColumnIndex(22)+Convert.ToString(DataRow), GetColumnIndex(22)+Convert.ToString(DataRow+DataCount)); 
				oRng.HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;
				oRng.NumberFormatLocal = "#,##0";

				oRng = oSheet.get_Range(GetColumnIndex(23)+Convert.ToString(DataRow), GetColumnIndex(23)+Convert.ToString(DataRow+DataCount)); 
				oRng.HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;
				oRng.NumberFormatLocal = "#,##0";

				oRng = oSheet.get_Range(GetColumnIndex(24)+Convert.ToString(DataRow), GetColumnIndex(24)+Convert.ToString(DataRow+DataCount)); 
				oRng.HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;
				oRng.NumberFormatLocal = "#,##0";

				oRng = oSheet.get_Range(GetColumnIndex(25)+Convert.ToString(DataRow), GetColumnIndex(25)+Convert.ToString(DataRow+DataCount)); 
				oRng.HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;
				oRng.NumberFormatLocal = "#,##0";

				oRng = oSheet.get_Range(GetColumnIndex(26)+Convert.ToString(DataRow), GetColumnIndex(26)+Convert.ToString(DataRow+DataCount)); 
				oRng.HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;
				oRng.NumberFormatLocal = "#,##0";
				
				//================================================================================================================================================

				int ColMax_r  = 26; // �÷��� 

				int HeaderRow_r = 11;
				int DataRow_r   = 12;
				string StartCol_r = "A";
				string EndCol_r   = "";				
				int DataCount_r = 0;		
				int HeaderCount_r = 0;

				// ������ �÷��� �ε�������
				EndCol_r = GetColumnIndex_r(ColMax_r);
				oSheet_r = (Excel._Worksheet)oWB.ActiveSheet;
				
				HeaderCount_r = 1;
				oSheet_r.Cells[HeaderRow_r,HeaderCount_r++] = "�����";
				oSheet_r.Cells[HeaderRow_r,HeaderCount_r++] = "�Ѱ�";
   				oSheet_r.Cells[HeaderRow_r,HeaderCount_r++] = "0��";
				oSheet_r.Cells[HeaderRow_r,HeaderCount_r++] = "1��";
				oSheet_r.Cells[HeaderRow_r,HeaderCount_r++] = "2��";
				oSheet_r.Cells[HeaderRow_r,HeaderCount_r++] = "3��";
				oSheet_r.Cells[HeaderRow_r,HeaderCount_r++] = "4��";
				oSheet_r.Cells[HeaderRow_r,HeaderCount_r++] = "5��";
				oSheet_r.Cells[HeaderRow_r,HeaderCount_r++] = "6��";
				oSheet_r.Cells[HeaderRow_r,HeaderCount_r++] = "7��";
				oSheet_r.Cells[HeaderRow_r,HeaderCount_r++] = "8��";
				oSheet_r.Cells[HeaderRow_r,HeaderCount_r++] = "9��";
				oSheet_r.Cells[HeaderRow_r,HeaderCount_r++] = "10��";
				oSheet_r.Cells[HeaderRow_r,HeaderCount_r++] = "11��";
				oSheet_r.Cells[HeaderRow_r,HeaderCount_r++] = "12��";
				oSheet_r.Cells[HeaderRow_r,HeaderCount_r++] = "13��";
				oSheet_r.Cells[HeaderRow_r,HeaderCount_r++] = "14��";
				oSheet_r.Cells[HeaderRow_r,HeaderCount_r++] = "15��";
				oSheet_r.Cells[HeaderRow_r,HeaderCount_r++] = "16��";
				oSheet_r.Cells[HeaderRow_r,HeaderCount_r++] = "17��";
				oSheet_r.Cells[HeaderRow_r,HeaderCount_r++] = "18��";
				oSheet_r.Cells[HeaderRow_r,HeaderCount_r++] = "19��";
				oSheet_r.Cells[HeaderRow_r,HeaderCount_r++] = "20��";
				oSheet_r.Cells[HeaderRow_r,HeaderCount_r++] = "21��";
				oSheet_r.Cells[HeaderRow_r,HeaderCount_r++] = "22��";
				oSheet_r.Cells[HeaderRow_r,HeaderCount_r++] = "23��";
				
				oRng_r = oSheet_r.get_Range(StartCol_r+Convert.ToString(HeaderRow_r), EndCol_r+Convert.ToString(HeaderRow_r));	// ����� ����
				oRng_r.Font.Bold        = true;							// ��Ʈ ����
                oRng_r.Font.Size        = 10;

				oRng_r.VerticalAlignment   = Excel.XlVAlign.xlVAlignCenter;	// �����߾�����
				oRng_r.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;	// �����߾�����	 
				oRng_r.Interior.Color      = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.CornflowerBlue);   //�� ���� 
				oRng_r.Font.Color          = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.White);			//�ؽ�Ʈ��			
				
				DataCount_r = 0;
				// ������ ����
				for (int inx =0; inx < adTypeMoniteringDs.AdTypeDetail.Rows.Count; inx++)
				{		
					DataRow Row_r = adTypeMoniteringDs.AdTypeDetail.Rows[inx];

					int ColCnt = 1;
		
					oSheet_r.Cells[DataRow_r+DataCount_r,ColCnt++] = Row_r["ItemName"].ToString();
					oSheet_r.Cells[DataRow_r+DataCount_r,ColCnt++] = Row_r["totCnt"].ToString();
                    oSheet_r.Cells[DataRow_r+DataCount_r,ColCnt++] = Row_r["H00"].ToString();
					oSheet_r.Cells[DataRow_r+DataCount_r,ColCnt++] = Row_r["H01"].ToString();
					oSheet_r.Cells[DataRow_r+DataCount_r,ColCnt++] = Row_r["H02"].ToString();
					oSheet_r.Cells[DataRow_r+DataCount_r,ColCnt++] = Row_r["H03"].ToString();
					oSheet_r.Cells[DataRow_r+DataCount_r,ColCnt++] = Row_r["H04"].ToString();
					oSheet_r.Cells[DataRow_r+DataCount_r,ColCnt++] = Row_r["H05"].ToString();
					oSheet_r.Cells[DataRow_r+DataCount_r,ColCnt++] = Row_r["H06"].ToString();
					oSheet_r.Cells[DataRow_r+DataCount_r,ColCnt++] = Row_r["H07"].ToString();
					oSheet_r.Cells[DataRow_r+DataCount_r,ColCnt++] = Row_r["H08"].ToString();
					oSheet_r.Cells[DataRow_r+DataCount_r,ColCnt++] = Row_r["H09"].ToString();
					oSheet_r.Cells[DataRow_r+DataCount_r,ColCnt++] = Row_r["H10"].ToString();
					oSheet_r.Cells[DataRow_r+DataCount_r,ColCnt++] = Row_r["H11"].ToString();
					oSheet_r.Cells[DataRow_r+DataCount_r,ColCnt++] = Row_r["H12"].ToString();
					oSheet_r.Cells[DataRow_r+DataCount_r,ColCnt++] = Row_r["H13"].ToString();
					oSheet_r.Cells[DataRow_r+DataCount_r,ColCnt++] = Row_r["H14"].ToString();
					oSheet_r.Cells[DataRow_r+DataCount_r,ColCnt++] = Row_r["H15"].ToString();
					oSheet_r.Cells[DataRow_r+DataCount_r,ColCnt++] = Row_r["H16"].ToString();
					oSheet_r.Cells[DataRow_r+DataCount_r,ColCnt++] = Row_r["H17"].ToString();
					oSheet_r.Cells[DataRow_r+DataCount_r,ColCnt++] = Row_r["H18"].ToString();
					oSheet_r.Cells[DataRow_r+DataCount_r,ColCnt++] = Row_r["H19"].ToString();
					oSheet_r.Cells[DataRow_r+DataCount_r,ColCnt++] = Row_r["H20"].ToString();
					oSheet_r.Cells[DataRow_r+DataCount_r,ColCnt++] = Row_r["H21"].ToString();
					oSheet_r.Cells[DataRow_r+DataCount_r,ColCnt++] = Row_r["H22"].ToString();
					oSheet_r.Cells[DataRow_r+DataCount_r,ColCnt++] = Row_r["H23"].ToString();
					DataCount_r++;
				}

				DataCount_r--;

				// ������ �ۼ�
				oRng_r = oSheet_r.get_Range(StartCol_r+Convert.ToString(HeaderRow_r), EndCol_r+Convert.ToString(DataRow_r+DataCount_r));	// �������� ����
                oRng_r.Font.Size = 9;
				oRng_r.EntireColumn.AutoFit();					// �������� ũ�⿡ ���� ����ũ�� ����
				oRng_r.Borders.LineStyle =  Excel.XlLineStyle.xlContinuous;	// �׵θ��Ӽ� �Ʒ��� �Ǽ�
				oRng_r.Borders.Weight    = Excel.XlBorderWeight.xlThin;		// �׵θ��Ӽ� �Ʒ��� ���¼�

				//�����ȣ �߾�����
				oRng = oSheet.get_Range(GetColumnIndex(1)+Convert.ToString(DataRow_r), GetColumnIndex(1)+Convert.ToString(DataRow_r+DataCount_r)); 
				oRng.HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft;
				
				//�����ȣ �߾�����
				oRng = oSheet.get_Range(GetColumnIndex(2)+Convert.ToString(DataRow_r), GetColumnIndex(2)+Convert.ToString(DataRow_r+DataCount_r)); 
				oRng.HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;
				oRng.NumberFormatLocal = "#,##0";
				
				//��ûȽ�� right����
				oRng = oSheet.get_Range(GetColumnIndex(3)+Convert.ToString(DataRow_r), GetColumnIndex(3)+Convert.ToString(DataRow_r+DataCount_r)); 
				oRng.HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;
				oRng.NumberFormatLocal = "#,##0";

				//��û���� right����
				oRng = oSheet.get_Range(GetColumnIndex(4)+Convert.ToString(DataRow_r), GetColumnIndex(4)+Convert.ToString(DataRow_r+DataCount_r)); 
				oRng.HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;
				oRng.NumberFormatLocal = "#,##0";

				//rate right����
				oRng = oSheet.get_Range(GetColumnIndex(5)+Convert.ToString(DataRow_r), GetColumnIndex(5)+Convert.ToString(DataRow_r+DataCount_r)); 
				oRng.HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;
				oRng.NumberFormatLocal = "#,##0";

				//Cove right����
				oRng = oSheet.get_Range(GetColumnIndex(6)+Convert.ToString(DataRow_r), GetColumnIndex(6)+Convert.ToString(DataRow_r+DataCount_r)); 
				oRng.HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;
				oRng.NumberFormatLocal = "#,##0";

				//Freq right����
				oRng = oSheet.get_Range(GetColumnIndex(7)+Convert.ToString(DataRow_r), GetColumnIndex(7)+Convert.ToString(DataRow_r+DataCount_r)); 
				oRng.HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;
				oRng.NumberFormatLocal = "#,##0";

				oRng = oSheet.get_Range(GetColumnIndex(8)+Convert.ToString(DataRow_r), GetColumnIndex(8)+Convert.ToString(DataRow_r+DataCount_r)); 
				oRng.HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;
				oRng.NumberFormatLocal = "#,##0";

				oRng = oSheet.get_Range(GetColumnIndex(9)+Convert.ToString(DataRow_r), GetColumnIndex(9)+Convert.ToString(DataRow_r+DataCount_r)); 
				oRng.HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;
				oRng.NumberFormatLocal = "#,##0";

				oRng = oSheet.get_Range(GetColumnIndex(10)+Convert.ToString(DataRow_r), GetColumnIndex(10)+Convert.ToString(DataRow_r+DataCount_r)); 
				oRng.HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;
				oRng.NumberFormatLocal = "#,##0";

				oRng = oSheet.get_Range(GetColumnIndex(11)+Convert.ToString(DataRow_r), GetColumnIndex(11)+Convert.ToString(DataRow_r+DataCount_r)); 
				oRng.HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;
				oRng.NumberFormatLocal = "#,##0";

				oRng = oSheet.get_Range(GetColumnIndex(12)+Convert.ToString(DataRow_r), GetColumnIndex(12)+Convert.ToString(DataRow_r+DataCount_r)); 
				oRng.HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;
				oRng.NumberFormatLocal = "#,##0";

				oRng = oSheet.get_Range(GetColumnIndex(13)+Convert.ToString(DataRow_r), GetColumnIndex(13)+Convert.ToString(DataRow_r+DataCount_r)); 
				oRng.HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;
				oRng.NumberFormatLocal = "#,##0";

				oRng = oSheet.get_Range(GetColumnIndex(14)+Convert.ToString(DataRow_r), GetColumnIndex(14)+Convert.ToString(DataRow_r+DataCount_r)); 
				oRng.HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;
				oRng.NumberFormatLocal = "#,##0";

				oRng = oSheet.get_Range(GetColumnIndex(15)+Convert.ToString(DataRow_r), GetColumnIndex(15)+Convert.ToString(DataRow_r+DataCount_r)); 
				oRng.HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;
				oRng.NumberFormatLocal = "#,##0";

				oRng = oSheet.get_Range(GetColumnIndex(16)+Convert.ToString(DataRow_r), GetColumnIndex(16)+Convert.ToString(DataRow_r+DataCount_r)); 
				oRng.HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;
				oRng.NumberFormatLocal = "#,##0";

				oRng = oSheet.get_Range(GetColumnIndex(17)+Convert.ToString(DataRow_r), GetColumnIndex(17)+Convert.ToString(DataRow_r+DataCount_r)); 
				oRng.HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;
				oRng.NumberFormatLocal = "#,##0";

				oRng = oSheet.get_Range(GetColumnIndex(18)+Convert.ToString(DataRow_r), GetColumnIndex(18)+Convert.ToString(DataRow_r+DataCount_r)); 
				oRng.HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;
				oRng.NumberFormatLocal = "#,##0";

				oRng = oSheet.get_Range(GetColumnIndex(19)+Convert.ToString(DataRow_r), GetColumnIndex(19)+Convert.ToString(DataRow_r+DataCount_r)); 
				oRng.HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;
				oRng.NumberFormatLocal = "#,##0";

				oRng = oSheet.get_Range(GetColumnIndex(20)+Convert.ToString(DataRow_r), GetColumnIndex(20)+Convert.ToString(DataRow_r+DataCount_r)); 
				oRng.HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;
				oRng.NumberFormatLocal = "#,##0";

				oRng = oSheet.get_Range(GetColumnIndex(21)+Convert.ToString(DataRow_r), GetColumnIndex(21)+Convert.ToString(DataRow_r+DataCount_r)); 
				oRng.HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;
				oRng.NumberFormatLocal = "#,##0";

				oRng = oSheet.get_Range(GetColumnIndex(22)+Convert.ToString(DataRow_r), GetColumnIndex(22)+Convert.ToString(DataRow_r+DataCount_r)); 
				oRng.HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;
				oRng.NumberFormatLocal = "#,##0";

				oRng = oSheet.get_Range(GetColumnIndex(23)+Convert.ToString(DataRow_r), GetColumnIndex(23)+Convert.ToString(DataRow_r+DataCount_r)); 
				oRng.HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;
				oRng.NumberFormatLocal = "#,##0";

				oRng = oSheet.get_Range(GetColumnIndex(24)+Convert.ToString(DataRow_r), GetColumnIndex(24)+Convert.ToString(DataRow_r+DataCount_r)); 
				oRng.HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;
				oRng.NumberFormatLocal = "#,##0";

				oRng = oSheet.get_Range(GetColumnIndex(25)+Convert.ToString(DataRow_r), GetColumnIndex(25)+Convert.ToString(DataRow_r+DataCount_r)); 
				oRng.HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;
				oRng.NumberFormatLocal = "#,##0";

				oRng = oSheet.get_Range(GetColumnIndex(26)+Convert.ToString(DataRow_r), GetColumnIndex(26)+Convert.ToString(DataRow_r+DataCount_r)); 
				oRng.HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;
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

		private string GetColumnIndex_r(int ColCount)
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

	}
}
