// ===============================================================================
// GradeControl for Charites Project
//
// GradeControl.cs
//
// ä���������� ������� �����մϴ�. 
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
 * Class Name: GroupManager
 * �ֿ���  : �׷��������� ���� ȣ��
 * �ۼ���    : ��
 * �ۼ���    : ��
 * -------------------------------------------------------
 * [��������]
 * -------------------------------------------------------
 * �����ڵ�  : [E_01]
 * ������    : �躸��
 * ������    : 2013.02
 * ��������  :        
 *            - �׸���� Ŭ���� ���� ����
 * �����Լ�  :
 *            - OnGrdRowChanged()
 *            - OnGrdRowDetailChanged()
 * --------------------------------------------------------
 */

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
	/// ä�ΰ��� ��Ʈ��
	/// </summary>
    public class GradeControl : System.Windows.Forms.UserControl, IUserControl
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
		GradeModel gradeModel  = new GradeModel();	// ä��������

		// ȭ��ó���� ����
		bool IsNewSearchKey		  = true;					// �˻����Է� ����
		CurrencyManager cm        = null;					// ������ �׸����� ���濡 ���� �����ͼ� ������ ���Ͽ�			
		CurrencyManager cmChild        = null;					// ������ �׸����� ���濡 ���� �����ͼ� ������ ���Ͽ�			
		DataTable       dt        = null;
		DataTable       dtChild        = null;
		
		private string        mediaCode_old = null;
		private string        categoryCode_old = null;
		private string        genreCode_old = null;
		private string        channelNo_old = null;

        bool IsSearching = false; // ��ȸ�� ��ȭ���� ������Ʈ �Ǵ� ���� ���� �ϱ����� 2011.11.29 JH.Park
		bool IsAdding             = false;
		bool canRead			  = false;
		bool canUpdate			  = false;
		bool canCreate            = false;
        private Janus.Windows.EditControls.UICheckBox chkAdState_20;
        private Janus.Windows.EditControls.UICheckBox chkAdState_10;
        private Janus.Windows.EditControls.UICheckBox chkAdState_30;
        private Janus.Windows.EditControls.UICheckBox chkAdState_40;
		bool canDelete            = false;

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
		private Janus.Windows.UI.Dock.UIPanel uiPanelUserDetail;
		private Janus.Windows.UI.Dock.UIPanelInnerContainer uiPanelUserDetailContainer;
		private Janus.Windows.UI.Dock.UIPanel uiPanelUsersSearch;
		private Janus.Windows.UI.Dock.UIPanelInnerContainer uiPanelUsersSearchContainer;
		private Janus.Windows.EditControls.UIButton uiButton2;
		private Janus.Windows.EditControls.UIButton uiButton1;
		private Janus.Windows.UI.Dock.UIPanelGroup uiPanelUsers;
		private System.Windows.Forms.Panel pnlSearch;
		private Janus.Windows.GridEX.EditControls.EditBox ebSearchKey;
		private Janus.Windows.EditControls.UIButton btnSearch;
		private System.Windows.Forms.Panel pnlUserDetail;				
		private Janus.Windows.EditControls.UIComboBox cbSearchMediaName;
		private System.Windows.Forms.Label lbModDt;
		private System.Windows.Forms.ImageList imageList;
		private Janus.Windows.UI.Dock.UIPanelGroup uiPanelChannelSet;
		private Janus.Windows.UI.Dock.UIPanel uiPanel1;
		private Janus.Windows.UI.Dock.UIPanelInnerContainer uiPanel1Container;
		private Janus.Windows.UI.Dock.UIPanel uiPanel2;
		private Janus.Windows.UI.Dock.UIPanelInnerContainer uiPanel2Container;
		private Janus.Windows.GridEX.GridEX grdExCategenList;
		private Janus.Windows.GridEX.EditControls.IntegerUpDown udAgeBtnBegin;
		private System.Data.DataView dvGrade;
		private System.Data.DataView dvContractItem;
		private AdManagerClient._40_Target._04_Grade.GradeDs gradeDs;
		private Janus.Windows.GridEX.GridEX grdExChannelSetList;		
		private System.Windows.Forms.Label lbRegID;
		private System.Windows.Forms.Label lbRegDt;
        private System.Windows.Forms.Label label13;
		private System.Windows.Forms.Label lbGrade;		
		private System.Windows.Forms.Label lbExepose;		
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label lbCode;
		private Janus.Windows.GridEX.EditControls.EditBox ebCode;
		private Janus.Windows.GridEX.EditControls.EditBox ebGrade;
		private Janus.Windows.GridEX.EditControls.IntegerUpDown udGrade;
		private Janus.Windows.EditControls.UIButton btnAdd;
		private Janus.Windows.EditControls.UIButton btnDelete;
		private Janus.Windows.EditControls.UIButton btnSave;
		private Janus.Windows.GridEX.EditControls.EditBox ebRegDt;
		private Janus.Windows.GridEX.EditControls.EditBox ebModDt;
		private Janus.Windows.GridEX.EditControls.EditBox ebRegID;		
		private System.ComponentModel.IContainer components;

		public GradeControl()
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
            Janus.Windows.GridEX.GridEXLayout grdExCategenList_DesignTimeLayout = new Janus.Windows.GridEX.GridEXLayout();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GradeControl));
            Janus.Windows.GridEX.GridEXLayout grdExCategenList_Layout_0 = new Janus.Windows.GridEX.GridEXLayout();
            Janus.Windows.GridEX.GridEXLayout grdExChannelSetList_DesignTimeLayout = new Janus.Windows.GridEX.GridEXLayout();
            this.uiPM = new Janus.Windows.UI.Dock.UIPanelManager(this.components);
            this.uiPanelUsers = new Janus.Windows.UI.Dock.UIPanelGroup();
            this.uiPanelUsersSearch = new Janus.Windows.UI.Dock.UIPanel();
            this.uiPanelUsersSearchContainer = new Janus.Windows.UI.Dock.UIPanelInnerContainer();
            this.pnlSearch = new System.Windows.Forms.Panel();
            this.label13 = new System.Windows.Forms.Label();
            this.cbSearchMediaName = new Janus.Windows.EditControls.UIComboBox();
            this.ebSearchKey = new Janus.Windows.GridEX.EditControls.EditBox();
            this.btnSearch = new Janus.Windows.EditControls.UIButton();
            this.uiPanelChannelSet = new Janus.Windows.UI.Dock.UIPanelGroup();
            this.uiPanel1 = new Janus.Windows.UI.Dock.UIPanel();
            this.uiPanel1Container = new Janus.Windows.UI.Dock.UIPanelInnerContainer();
            this.grdExCategenList = new Janus.Windows.GridEX.GridEX();
            this.dvGrade = new System.Data.DataView();
            this.gradeDs = new AdManagerClient._40_Target._04_Grade.GradeDs();
            this.uiPanel2 = new Janus.Windows.UI.Dock.UIPanel();
            this.uiPanel2Container = new Janus.Windows.UI.Dock.UIPanelInnerContainer();
            this.grdExChannelSetList = new Janus.Windows.GridEX.GridEX();
            this.dvContractItem = new System.Data.DataView();
            this.uiPanelUserDetail = new Janus.Windows.UI.Dock.UIPanel();
            this.uiPanelUserDetailContainer = new Janus.Windows.UI.Dock.UIPanelInnerContainer();
            this.pnlUserDetail = new System.Windows.Forms.Panel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.ebCode = new Janus.Windows.GridEX.EditControls.EditBox();
            this.lbCode = new System.Windows.Forms.Label();
            this.ebRegDt = new Janus.Windows.GridEX.EditControls.EditBox();
            this.ebRegID = new Janus.Windows.GridEX.EditControls.EditBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.udGrade = new Janus.Windows.GridEX.EditControls.IntegerUpDown();
            this.ebGrade = new Janus.Windows.GridEX.EditControls.EditBox();
            this.ebModDt = new Janus.Windows.GridEX.EditControls.EditBox();
            this.label3 = new System.Windows.Forms.Label();
            this.btnDelete = new Janus.Windows.EditControls.UIButton();
            this.btnAdd = new Janus.Windows.EditControls.UIButton();
            this.btnSave = new Janus.Windows.EditControls.UIButton();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.lbRegID = new System.Windows.Forms.Label();
            this.lbRegDt = new System.Windows.Forms.Label();
            this.lbModDt = new System.Windows.Forms.Label();
            this.lbExepose = new System.Windows.Forms.Label();
            this.lbGrade = new System.Windows.Forms.Label();
            this.udAgeBtnBegin = new Janus.Windows.GridEX.EditControls.IntegerUpDown();
            this.uiButton1 = new Janus.Windows.EditControls.UIButton();
            this.uiButton2 = new Janus.Windows.EditControls.UIButton();
            this.imageList = new System.Windows.Forms.ImageList(this.components);
            this.chkAdState_10 = new Janus.Windows.EditControls.UICheckBox();
            this.chkAdState_20 = new Janus.Windows.EditControls.UICheckBox();
            this.chkAdState_30 = new Janus.Windows.EditControls.UICheckBox();
            this.chkAdState_40 = new Janus.Windows.EditControls.UICheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.uiPM)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelUsers)).BeginInit();
            this.uiPanelUsers.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelUsersSearch)).BeginInit();
            this.uiPanelUsersSearch.SuspendLayout();
            this.uiPanelUsersSearchContainer.SuspendLayout();
            this.pnlSearch.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelChannelSet)).BeginInit();
            this.uiPanelChannelSet.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanel1)).BeginInit();
            this.uiPanel1.SuspendLayout();
            this.uiPanel1Container.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdExCategenList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dvGrade)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gradeDs)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanel2)).BeginInit();
            this.uiPanel2.SuspendLayout();
            this.uiPanel2Container.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdExChannelSetList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dvContractItem)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelUserDetail)).BeginInit();
            this.uiPanelUserDetail.SuspendLayout();
            this.uiPanelUserDetailContainer.SuspendLayout();
            this.pnlUserDetail.SuspendLayout();
            this.panel1.SuspendLayout();
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
            this.uiPanelUsers.Id = new System.Guid("b162c53a-7940-47cd-a0ed-9762c59c50d8");
            this.uiPanelUsers.StaticGroup = true;
            this.uiPanelUsersSearch.Id = new System.Guid("afb3b7d0-aa79-443c-a1e8-3baba38f7a4d");
            this.uiPanelUsers.Panels.Add(this.uiPanelUsersSearch);
            this.uiPanelChannelSet.Id = new System.Guid("88f86479-98e4-43d4-bf03-12fd789fcfc5");
            this.uiPanelChannelSet.StaticGroup = true;
            this.uiPanel1.Id = new System.Guid("91eeb7fc-856c-4a9c-b37e-db742cfa9b2e");
            this.uiPanelChannelSet.Panels.Add(this.uiPanel1);
            this.uiPanel2.Id = new System.Guid("3d64f731-de32-42ba-a10f-ac3b5ac56703");
            this.uiPanelChannelSet.Panels.Add(this.uiPanel2);
            this.uiPanelUsers.Panels.Add(this.uiPanelChannelSet);
            this.uiPanelUserDetail.Id = new System.Guid("b2973a57-d3e1-4d80-9387-7a6dc3332542");
            this.uiPanelUsers.Panels.Add(this.uiPanelUserDetail);
            this.uiPM.Panels.Add(this.uiPanelUsers);
            // 
            // Design Time Panel Info:
            // 
            this.uiPM.BeginPanelInfo();
            this.uiPM.AddDockPanelInfo(new System.Guid("b162c53a-7940-47cd-a0ed-9762c59c50d8"), Janus.Windows.UI.Dock.PanelGroupStyle.HorizontalTiles, Janus.Windows.UI.Dock.PanelDockStyle.Fill, true, new System.Drawing.Size(1010, 677), true);
            this.uiPM.AddDockPanelInfo(new System.Guid("afb3b7d0-aa79-443c-a1e8-3baba38f7a4d"), new System.Guid("b162c53a-7940-47cd-a0ed-9762c59c50d8"), 42, true);
            this.uiPM.AddDockPanelInfo(new System.Guid("88f86479-98e4-43d4-bf03-12fd789fcfc5"), new System.Guid("b162c53a-7940-47cd-a0ed-9762c59c50d8"), Janus.Windows.UI.Dock.PanelGroupStyle.VerticalTiles, true, 435, true);
            this.uiPM.AddDockPanelInfo(new System.Guid("91eeb7fc-856c-4a9c-b37e-db742cfa9b2e"), new System.Guid("88f86479-98e4-43d4-bf03-12fd789fcfc5"), 189, true);
            this.uiPM.AddDockPanelInfo(new System.Guid("3d64f731-de32-42ba-a10f-ac3b5ac56703"), new System.Guid("88f86479-98e4-43d4-bf03-12fd789fcfc5"), 817, true);
            this.uiPM.AddDockPanelInfo(new System.Guid("b2973a57-d3e1-4d80-9387-7a6dc3332542"), new System.Guid("b162c53a-7940-47cd-a0ed-9762c59c50d8"), 150, true);
            this.uiPM.AddFloatingPanelInfo(new System.Guid("b162c53a-7940-47cd-a0ed-9762c59c50d8"), Janus.Windows.UI.Dock.PanelGroupStyle.HorizontalTiles, true, new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
            this.uiPM.AddFloatingPanelInfo(new System.Guid("8e3140b9-bfcb-43af-b84e-2ee6cddf51dc"), new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
            this.uiPM.AddFloatingPanelInfo(new System.Guid("b2973a57-d3e1-4d80-9387-7a6dc3332542"), new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
            this.uiPM.AddFloatingPanelInfo(new System.Guid("afb3b7d0-aa79-443c-a1e8-3baba38f7a4d"), new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
            this.uiPM.AddFloatingPanelInfo(new System.Guid("88f86479-98e4-43d4-bf03-12fd789fcfc5"), Janus.Windows.UI.Dock.PanelGroupStyle.VerticalTiles, true, new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
            this.uiPM.AddFloatingPanelInfo(new System.Guid("91eeb7fc-856c-4a9c-b37e-db742cfa9b2e"), new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
            this.uiPM.AddFloatingPanelInfo(new System.Guid("3d64f731-de32-42ba-a10f-ac3b5ac56703"), new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
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
            this.uiPanelUsers.TabIndex = 0;
            this.uiPanelUsers.Text = "�����õ�ް���";
            // 
            // uiPanelUsersSearch
            // 
            this.uiPanelUsersSearch.AllowResize = Janus.Windows.UI.InheritableBoolean.False;
            this.uiPanelUsersSearch.CaptionVisible = Janus.Windows.UI.InheritableBoolean.False;
            this.uiPanelUsersSearch.CloseButtonVisible = Janus.Windows.UI.InheritableBoolean.False;
            this.uiPanelUsersSearch.InnerContainer = this.uiPanelUsersSearchContainer;
            this.uiPanelUsersSearch.Location = new System.Drawing.Point(0, 22);
            this.uiPanelUsersSearch.Name = "uiPanelUsersSearch";
            this.uiPanelUsersSearch.Size = new System.Drawing.Size(1010, 43);
            this.uiPanelUsersSearch.TabIndex = 0;
            this.uiPanelUsersSearch.Text = "�˻�";
            // 
            // uiPanelUsersSearchContainer
            // 
            this.uiPanelUsersSearchContainer.Controls.Add(this.pnlSearch);
            this.uiPanelUsersSearchContainer.Location = new System.Drawing.Point(1, 1);
            this.uiPanelUsersSearchContainer.Name = "uiPanelUsersSearchContainer";
            this.uiPanelUsersSearchContainer.Size = new System.Drawing.Size(1008, 41);
            this.uiPanelUsersSearchContainer.TabIndex = 0;
            // 
            // pnlSearch
            // 
            this.pnlSearch.BackColor = System.Drawing.SystemColors.Window;
            this.pnlSearch.Controls.Add(this.chkAdState_40);
            this.pnlSearch.Controls.Add(this.chkAdState_30);
            this.pnlSearch.Controls.Add(this.chkAdState_20);
            this.pnlSearch.Controls.Add(this.chkAdState_10);
            this.pnlSearch.Controls.Add(this.label13);
            this.pnlSearch.Controls.Add(this.cbSearchMediaName);
            this.pnlSearch.Controls.Add(this.ebSearchKey);
            this.pnlSearch.Controls.Add(this.btnSearch);
            this.pnlSearch.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlSearch.Location = new System.Drawing.Point(0, 0);
            this.pnlSearch.Name = "pnlSearch";
            this.pnlSearch.Size = new System.Drawing.Size(1008, 41);
            this.pnlSearch.TabIndex = 0;
            // 
            // label13
            // 
            this.label13.Location = new System.Drawing.Point(168, 10);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(64, 21);
            this.label13.TabIndex = 11;
            this.label13.Text = "�������";
            this.label13.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cbSearchMediaName
            // 
            this.cbSearchMediaName.ComboStyle = Janus.Windows.EditControls.ComboStyle.DropDownList;
            this.cbSearchMediaName.Font = new System.Drawing.Font("����ü", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.cbSearchMediaName.Location = new System.Drawing.Point(8, 11);
            this.cbSearchMediaName.Name = "cbSearchMediaName";
            this.cbSearchMediaName.Size = new System.Drawing.Size(152, 21);
            this.cbSearchMediaName.TabIndex = 1;
            this.cbSearchMediaName.Text = "��ü����";
            this.cbSearchMediaName.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            // 
            // ebSearchKey
            // 
            this.ebSearchKey.Location = new System.Drawing.Point(448, 9);
            this.ebSearchKey.Name = "ebSearchKey";
            this.ebSearchKey.Size = new System.Drawing.Size(144, 21);
            this.ebSearchKey.TabIndex = 4;
            this.ebSearchKey.Text = "�˻�� �Է��ϼ���";
            this.ebSearchKey.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
            this.ebSearchKey.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            this.ebSearchKey.TextChanged += new System.EventHandler(this.ebSearchKey_TextChanged);
            this.ebSearchKey.Click += new System.EventHandler(this.ebSearchKey_Click);
            this.ebSearchKey.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ebSearchKey_KeyDown);
            // 
            // btnSearch
            // 
            this.btnSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSearch.Enabled = false;
            this.btnSearch.Location = new System.Drawing.Point(903, 8);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(96, 24);
            this.btnSearch.TabIndex = 5;
            this.btnSearch.Text = "�� ȸ";
            this.btnSearch.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // uiPanelChannelSet
            // 
            this.uiPanelChannelSet.CaptionStyle = Janus.Windows.UI.Dock.PanelCaptionStyle.Dark;
            this.uiPanelChannelSet.GroupStyle = Janus.Windows.UI.Dock.PanelGroupStyle.VerticalTiles;
            this.uiPanelChannelSet.Location = new System.Drawing.Point(0, 69);
            this.uiPanelChannelSet.Name = "uiPanelChannelSet";
            this.uiPanelChannelSet.Size = new System.Drawing.Size(1010, 449);
            this.uiPanelChannelSet.TabIndex = 0;
            this.uiPanelChannelSet.Text = "�����õ��";
            // 
            // uiPanel1
            // 
            this.uiPanel1.InnerContainer = this.uiPanel1Container;
            this.uiPanel1.Location = new System.Drawing.Point(0, 22);
            this.uiPanel1.Name = "uiPanel1";
            this.uiPanel1.Size = new System.Drawing.Size(189, 427);
            this.uiPanel1.TabIndex = 0;
            this.uiPanel1.Text = "����";
            // 
            // uiPanel1Container
            // 
            this.uiPanel1Container.Controls.Add(this.grdExCategenList);
            this.uiPanel1Container.Location = new System.Drawing.Point(1, 23);
            this.uiPanel1Container.Name = "uiPanel1Container";
            this.uiPanel1Container.Size = new System.Drawing.Size(187, 403);
            this.uiPanel1Container.TabIndex = 0;
            // 
            // grdExCategenList
            // 
            this.grdExCategenList.AllowEdit = Janus.Windows.GridEX.InheritableBoolean.False;
            this.grdExCategenList.AlternatingColors = true;
            this.grdExCategenList.AlternatingRowFormatStyle.BackgroundImageDrawMode = Janus.Windows.GridEX.BackgroundImageDrawMode.Tile;
            this.grdExCategenList.AutomaticSort = false;
            this.grdExCategenList.BorderStyle = Janus.Windows.GridEX.BorderStyle.None;
            this.grdExCategenList.DataSource = this.dvGrade;
            grdExCategenList_DesignTimeLayout.LayoutString = resources.GetString("grdExCategenList_DesignTimeLayout.LayoutString");
            this.grdExCategenList.DesignTimeLayout = grdExCategenList_DesignTimeLayout;
            this.grdExCategenList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdExCategenList.EditorsControlStyle.ButtonAppearance = Janus.Windows.GridEX.ButtonAppearance.Regular;
            this.grdExCategenList.EmptyRows = true;
            this.grdExCategenList.FocusCellFormatStyle.BackColor = System.Drawing.SystemColors.Highlight;
            this.grdExCategenList.FocusCellFormatStyle.ForeColor = System.Drawing.Color.White;
            this.grdExCategenList.FocusStyle = Janus.Windows.GridEX.FocusStyle.None;
            this.grdExCategenList.Font = new System.Drawing.Font("����ü", 9F);
            this.grdExCategenList.FrozenColumns = 2;
            this.grdExCategenList.GridLineColor = System.Drawing.Color.Silver;
            this.grdExCategenList.GridLines = Janus.Windows.GridEX.GridLines.Vertical;
            this.grdExCategenList.GridLineStyle = Janus.Windows.GridEX.GridLineStyle.Solid;
            this.grdExCategenList.GroupByBoxVisible = false;
            this.grdExCategenList.GroupMode = Janus.Windows.GridEX.GroupMode.Collapsed;
            this.grdExCategenList.GroupRowVisualStyle = Janus.Windows.GridEX.GroupRowVisualStyle.Outlook2003;
            this.grdExCategenList.HeaderFormatStyle.TextAlignment = Janus.Windows.GridEX.TextAlignment.Center;
            this.grdExCategenList.HideSelection = Janus.Windows.GridEX.HideSelection.HighlightInactive;
            grdExCategenList_Layout_0.Key = "bea";
            this.grdExCategenList.Layouts.AddRange(new Janus.Windows.GridEX.GridEXLayout[] {
            grdExCategenList_Layout_0});
            this.grdExCategenList.Location = new System.Drawing.Point(0, 0);
            this.grdExCategenList.Name = "grdExCategenList";
            this.grdExCategenList.ScrollBars = Janus.Windows.GridEX.ScrollBars.Vertical;
            this.grdExCategenList.SelectedInactiveFormatStyle.BackColor = System.Drawing.Color.Gold;
            this.grdExCategenList.Size = new System.Drawing.Size(187, 403);
            this.grdExCategenList.TabIndex = 6;
            this.grdExCategenList.TabKeyBehavior = Janus.Windows.GridEX.TabKeyBehavior.ControlNavigation;
            this.grdExCategenList.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            this.grdExCategenList.Enter += new System.EventHandler(this.OnGrdRowChanged);
            // 
            // dvGrade
            // 
            this.dvGrade.Table = this.gradeDs.Grade;
            // 
            // gradeDs
            // 
            this.gradeDs.DataSetName = "GradeDs";
            this.gradeDs.Locale = new System.Globalization.CultureInfo("en-US");
            this.gradeDs.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // uiPanel2
            // 
            this.uiPanel2.InnerContainer = this.uiPanel2Container;
            this.uiPanel2.Location = new System.Drawing.Point(193, 22);
            this.uiPanel2.Name = "uiPanel2";
            this.uiPanel2.Size = new System.Drawing.Size(817, 427);
            this.uiPanel2.TabIndex = 0;
            this.uiPanel2.Text = "�ش籤��";
            // 
            // uiPanel2Container
            // 
            this.uiPanel2Container.Controls.Add(this.grdExChannelSetList);
            this.uiPanel2Container.Location = new System.Drawing.Point(1, 23);
            this.uiPanel2Container.Name = "uiPanel2Container";
            this.uiPanel2Container.Size = new System.Drawing.Size(815, 403);
            this.uiPanel2Container.TabIndex = 0;
            // 
            // grdExChannelSetList
            // 
            this.grdExChannelSetList.AllowEdit = Janus.Windows.GridEX.InheritableBoolean.False;
            this.grdExChannelSetList.AlternatingColors = true;
            this.grdExChannelSetList.BorderStyle = Janus.Windows.GridEX.BorderStyle.None;
            this.grdExChannelSetList.DataSource = this.dvContractItem;
            grdExChannelSetList_DesignTimeLayout.LayoutString = resources.GetString("grdExChannelSetList_DesignTimeLayout.LayoutString");
            this.grdExChannelSetList.DesignTimeLayout = grdExChannelSetList_DesignTimeLayout;
            this.grdExChannelSetList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdExChannelSetList.EditorsControlStyle.ButtonAppearance = Janus.Windows.GridEX.ButtonAppearance.Regular;
            this.grdExChannelSetList.EmptyRows = true;
            this.grdExChannelSetList.FocusCellFormatStyle.BackColor = System.Drawing.SystemColors.Highlight;
            this.grdExChannelSetList.FocusCellFormatStyle.ForeColor = System.Drawing.Color.White;
            this.grdExChannelSetList.FocusStyle = Janus.Windows.GridEX.FocusStyle.None;
            this.grdExChannelSetList.Font = new System.Drawing.Font("����ü", 9F);
            this.grdExChannelSetList.FrozenColumns = 3;
            this.grdExChannelSetList.GridLineColor = System.Drawing.Color.Silver;
            this.grdExChannelSetList.GridLines = Janus.Windows.GridEX.GridLines.Vertical;
            this.grdExChannelSetList.GridLineStyle = Janus.Windows.GridEX.GridLineStyle.Solid;
            this.grdExChannelSetList.GroupByBoxVisible = false;
            this.grdExChannelSetList.HideSelection = Janus.Windows.GridEX.HideSelection.HighlightInactive;
            this.grdExChannelSetList.Location = new System.Drawing.Point(0, 0);
            this.grdExChannelSetList.Name = "grdExChannelSetList";
            this.grdExChannelSetList.SelectedInactiveFormatStyle.BackColor = System.Drawing.Color.Gold;
            this.grdExChannelSetList.Size = new System.Drawing.Size(815, 403);
            this.grdExChannelSetList.TabIndex = 15;
            this.grdExChannelSetList.TabKeyBehavior = Janus.Windows.GridEX.TabKeyBehavior.ControlNavigation;
            this.grdExChannelSetList.ThemedAreas = ((Janus.Windows.GridEX.ThemedArea)(((((((((Janus.Windows.GridEX.ThemedArea.ScrollBars | Janus.Windows.GridEX.ThemedArea.EditControls) 
            | Janus.Windows.GridEX.ThemedArea.Headers) 
            | Janus.Windows.GridEX.ThemedArea.GroupByBox) 
            | Janus.Windows.GridEX.ThemedArea.GroupRows) 
            | Janus.Windows.GridEX.ThemedArea.ControlBorder) 
            | Janus.Windows.GridEX.ThemedArea.Cards) 
            | Janus.Windows.GridEX.ThemedArea.Gridlines) 
            | Janus.Windows.GridEX.ThemedArea.CheckBoxes)));
            this.grdExChannelSetList.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            // 
            // dvContractItem
            // 
            this.dvContractItem.Table = this.gradeDs.ContractItem;
            // 
            // uiPanelUserDetail
            // 
            this.uiPanelUserDetail.BackColor = System.Drawing.SystemColors.Window;
            this.uiPanelUserDetail.CaptionStyle = Janus.Windows.UI.Dock.PanelCaptionStyle.Dark;
            this.uiPanelUserDetail.InnerContainer = this.uiPanelUserDetailContainer;
            this.uiPanelUserDetail.Location = new System.Drawing.Point(0, 522);
            this.uiPanelUserDetail.Name = "uiPanelUserDetail";
            this.uiPanelUserDetail.Size = new System.Drawing.Size(1010, 155);
            this.uiPanelUserDetail.TabIndex = 0;
            this.uiPanelUserDetail.Text = "������";
            // 
            // uiPanelUserDetailContainer
            // 
            this.uiPanelUserDetailContainer.Controls.Add(this.pnlUserDetail);
            this.uiPanelUserDetailContainer.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.uiPanelUserDetailContainer.Location = new System.Drawing.Point(1, 23);
            this.uiPanelUserDetailContainer.Name = "uiPanelUserDetailContainer";
            this.uiPanelUserDetailContainer.Size = new System.Drawing.Size(1008, 131);
            this.uiPanelUserDetailContainer.TabIndex = 0;
            // 
            // pnlUserDetail
            // 
            this.pnlUserDetail.BackColor = System.Drawing.SystemColors.Window;
            this.pnlUserDetail.Controls.Add(this.panel1);
            this.pnlUserDetail.Controls.Add(this.lbRegID);
            this.pnlUserDetail.Controls.Add(this.lbRegDt);
            this.pnlUserDetail.Controls.Add(this.lbModDt);
            this.pnlUserDetail.Controls.Add(this.lbExepose);
            this.pnlUserDetail.Controls.Add(this.lbGrade);
            this.pnlUserDetail.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlUserDetail.Location = new System.Drawing.Point(0, 0);
            this.pnlUserDetail.Name = "pnlUserDetail";
            this.pnlUserDetail.Size = new System.Drawing.Size(1008, 131);
            this.pnlUserDetail.TabIndex = 0;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.Window;
            this.panel1.Controls.Add(this.ebCode);
            this.panel1.Controls.Add(this.lbCode);
            this.panel1.Controls.Add(this.ebRegDt);
            this.panel1.Controls.Add(this.ebRegID);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.udGrade);
            this.panel1.Controls.Add(this.ebGrade);
            this.panel1.Controls.Add(this.ebModDt);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.btnDelete);
            this.panel1.Controls.Add(this.btnAdd);
            this.panel1.Controls.Add(this.btnSave);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1008, 131);
            this.panel1.TabIndex = 203;
            // 
            // ebCode
            // 
            this.ebCode.Font = new System.Drawing.Font("����ü", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.ebCode.Location = new System.Drawing.Point(56, 8);
            this.ebCode.MaxLength = 120;
            this.ebCode.Name = "ebCode";
            this.ebCode.Size = new System.Drawing.Size(48, 21);
            this.ebCode.TabIndex = 203;
            this.ebCode.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
            this.ebCode.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            // 
            // lbCode
            // 
            this.lbCode.Font = new System.Drawing.Font("����ü", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lbCode.Location = new System.Drawing.Point(8, 8);
            this.lbCode.Name = "lbCode";
            this.lbCode.Size = new System.Drawing.Size(32, 21);
            this.lbCode.TabIndex = 204;
            this.lbCode.Text = "�ڵ�";
            this.lbCode.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ebRegDt
            // 
            this.ebRegDt.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ebRegDt.Font = new System.Drawing.Font("����ü", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.ebRegDt.Location = new System.Drawing.Point(472, 8);
            this.ebRegDt.Name = "ebRegDt";
            this.ebRegDt.ReadOnly = true;
            this.ebRegDt.Size = new System.Drawing.Size(160, 21);
            this.ebRegDt.TabIndex = 200;
            this.ebRegDt.TabStop = false;
            this.ebRegDt.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
            this.ebRegDt.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            // 
            // ebRegID
            // 
            this.ebRegID.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ebRegID.Font = new System.Drawing.Font("����ü", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.ebRegID.Location = new System.Drawing.Point(472, 56);
            this.ebRegID.MaxLength = 10;
            this.ebRegID.Name = "ebRegID";
            this.ebRegID.ReadOnly = true;
            this.ebRegID.Size = new System.Drawing.Size(160, 21);
            this.ebRegID.TabIndex = 199;
            this.ebRegID.TabStop = false;
            this.ebRegID.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
            this.ebRegID.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("����ü", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label1.Location = new System.Drawing.Point(408, 56);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(72, 21);
            this.label1.TabIndex = 201;
            this.label1.Text = "�����";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("����ü", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label2.Location = new System.Drawing.Point(408, 8);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(72, 21);
            this.label2.TabIndex = 202;
            this.label2.Text = "����Ͻ�";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // udGrade
            // 
            this.udGrade.Location = new System.Drawing.Point(328, 8);
            this.udGrade.Maximum = 99;
            this.udGrade.MaxLength = 2;
            this.udGrade.Name = "udGrade";
            this.udGrade.Size = new System.Drawing.Size(64, 21);
            this.udGrade.TabIndex = 198;
            this.udGrade.TextAlignment = Janus.Windows.GridEX.TextAlignment.Far;
            this.udGrade.Value = 1;
            this.udGrade.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            // 
            // ebGrade
            // 
            this.ebGrade.Font = new System.Drawing.Font("����ü", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.ebGrade.Location = new System.Drawing.Point(168, 8);
            this.ebGrade.MaxLength = 120;
            this.ebGrade.Name = "ebGrade";
            this.ebGrade.Size = new System.Drawing.Size(96, 21);
            this.ebGrade.TabIndex = 12;
            this.ebGrade.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
            this.ebGrade.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            // 
            // ebModDt
            // 
            this.ebModDt.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ebModDt.Font = new System.Drawing.Font("����ü", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.ebModDt.Location = new System.Drawing.Point(472, 32);
            this.ebModDt.Name = "ebModDt";
            this.ebModDt.ReadOnly = true;
            this.ebModDt.Size = new System.Drawing.Size(160, 21);
            this.ebModDt.TabIndex = 18;
            this.ebModDt.TabStop = false;
            this.ebModDt.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
            this.ebModDt.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            // 
            // label3
            // 
            this.label3.Font = new System.Drawing.Font("����ü", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label3.Location = new System.Drawing.Point(408, 32);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(72, 21);
            this.label3.TabIndex = 0;
            this.label3.Text = "��������";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnDelete
            // 
            this.btnDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnDelete.BackColor = System.Drawing.SystemColors.Window;
            this.btnDelete.Enabled = false;
            this.btnDelete.Location = new System.Drawing.Point(128, 95);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(104, 24);
            this.btnDelete.TabIndex = 15;
            this.btnDelete.Text = "�� ��";
            this.btnDelete.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnAdd
            // 
            this.btnAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnAdd.BackColor = System.Drawing.SystemColors.Window;
            this.btnAdd.Enabled = false;
            this.btnAdd.Location = new System.Drawing.Point(240, 95);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(104, 24);
            this.btnAdd.TabIndex = 16;
            this.btnAdd.Text = "�� ��";
            this.btnAdd.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnSave.BackColor = System.Drawing.SystemColors.Window;
            this.btnSave.Enabled = false;
            this.btnSave.Location = new System.Drawing.Point(16, 95);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(104, 24);
            this.btnSave.TabIndex = 14;
            this.btnSave.Text = "�� ��";
            this.btnSave.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // label4
            // 
            this.label4.Font = new System.Drawing.Font("����ü", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label4.Location = new System.Drawing.Point(280, 8);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(72, 21);
            this.label4.TabIndex = 12;
            this.label4.Text = "����";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label5
            // 
            this.label5.Font = new System.Drawing.Font("����ü", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label5.Location = new System.Drawing.Point(120, 8);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(72, 21);
            this.label5.TabIndex = 18;
            this.label5.Text = "���";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lbRegID
            // 
            this.lbRegID.Font = new System.Drawing.Font("����ü", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lbRegID.Location = new System.Drawing.Point(384, 56);
            this.lbRegID.Name = "lbRegID";
            this.lbRegID.Size = new System.Drawing.Size(72, 21);
            this.lbRegID.TabIndex = 201;
            this.lbRegID.Text = "�����";
            this.lbRegID.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lbRegDt
            // 
            this.lbRegDt.Font = new System.Drawing.Font("����ü", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lbRegDt.Location = new System.Drawing.Point(384, 8);
            this.lbRegDt.Name = "lbRegDt";
            this.lbRegDt.Size = new System.Drawing.Size(72, 21);
            this.lbRegDt.TabIndex = 202;
            this.lbRegDt.Text = "����Ͻ�";
            this.lbRegDt.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lbModDt
            // 
            this.lbModDt.Font = new System.Drawing.Font("����ü", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lbModDt.Location = new System.Drawing.Point(384, 32);
            this.lbModDt.Name = "lbModDt";
            this.lbModDt.Size = new System.Drawing.Size(72, 21);
            this.lbModDt.TabIndex = 0;
            this.lbModDt.Text = "��������";
            this.lbModDt.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lbExepose
            // 
            this.lbExepose.Font = new System.Drawing.Font("����ü", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lbExepose.Location = new System.Drawing.Point(256, 8);
            this.lbExepose.Name = "lbExepose";
            this.lbExepose.Size = new System.Drawing.Size(72, 21);
            this.lbExepose.TabIndex = 12;
            this.lbExepose.Text = "����";
            this.lbExepose.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lbGrade
            // 
            this.lbGrade.Font = new System.Drawing.Font("����ü", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lbGrade.Location = new System.Drawing.Point(96, 8);
            this.lbGrade.Name = "lbGrade";
            this.lbGrade.Size = new System.Drawing.Size(72, 21);
            this.lbGrade.TabIndex = 18;
            this.lbGrade.Text = "���";
            this.lbGrade.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // udAgeBtnBegin
            // 
            this.udAgeBtnBegin.Location = new System.Drawing.Point(0, 0);
            this.udAgeBtnBegin.Name = "udAgeBtnBegin";
            this.udAgeBtnBegin.Size = new System.Drawing.Size(100, 21);
            this.udAgeBtnBegin.TabIndex = 0;
            this.udAgeBtnBegin.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
            // 
            // uiButton1
            // 
            this.uiButton1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.uiButton1.Location = new System.Drawing.Point(136, 8);
            this.uiButton1.Name = "uiButton1";
            this.uiButton1.Size = new System.Drawing.Size(112, 24);
            this.uiButton1.TabIndex = 0;
            this.uiButton1.Text = "�� ��";
            this.uiButton1.VisualStyle = Janus.Windows.UI.VisualStyle.Office2003;
            // 
            // uiButton2
            // 
            this.uiButton2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.uiButton2.Location = new System.Drawing.Point(8, 8);
            this.uiButton2.Name = "uiButton2";
            this.uiButton2.Size = new System.Drawing.Size(120, 24);
            this.uiButton2.TabIndex = 0;
            this.uiButton2.Text = "�� ��";
            this.uiButton2.VisualStyle = Janus.Windows.UI.VisualStyle.Office2003;
            // 
            // imageList
            // 
            this.imageList.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
            this.imageList.ImageSize = new System.Drawing.Size(16, 16);
            this.imageList.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // chkAdState_10
            // 
            this.chkAdState_10.Checked = true;
            this.chkAdState_10.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkAdState_10.Location = new System.Drawing.Point(238, 9);
            this.chkAdState_10.Name = "chkAdState_10";
            this.chkAdState_10.Size = new System.Drawing.Size(44, 23);
            this.chkAdState_10.TabIndex = 16;
            this.chkAdState_10.Text = "���";
            // 
            // chkAdState_20
            // 
            this.chkAdState_20.Checked = true;
            this.chkAdState_20.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkAdState_20.Location = new System.Drawing.Point(288, 9);
            this.chkAdState_20.Name = "chkAdState_20";
            this.chkAdState_20.Size = new System.Drawing.Size(56, 23);
            this.chkAdState_20.TabIndex = 16;
            this.chkAdState_20.Text = "��";
            // 
            // chkAdState_30
            // 
            this.chkAdState_30.Location = new System.Drawing.Point(336, 9);
            this.chkAdState_30.Name = "chkAdState_30";
            this.chkAdState_30.Size = new System.Drawing.Size(56, 23);
            this.chkAdState_30.TabIndex = 16;
            this.chkAdState_30.Text = "����";
            // 
            // chkAdState_40
            // 
            this.chkAdState_40.Location = new System.Drawing.Point(386, 8);
            this.chkAdState_40.Name = "chkAdState_40";
            this.chkAdState_40.Size = new System.Drawing.Size(56, 23);
            this.chkAdState_40.TabIndex = 16;
            this.chkAdState_40.Text = "����";
            // 
            // GradeControl
            // 
            this.Controls.Add(this.uiPanelUsers);
            this.Name = "GradeControl";
            this.Size = new System.Drawing.Size(1010, 677);
            this.Load += new System.EventHandler(this.GradeControl_Load);
            ((System.ComponentModel.ISupportInitialize)(this.uiPM)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelUsers)).EndInit();
            this.uiPanelUsers.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelUsersSearch)).EndInit();
            this.uiPanelUsersSearch.ResumeLayout(false);
            this.uiPanelUsersSearchContainer.ResumeLayout(false);
            this.pnlSearch.ResumeLayout(false);
            this.pnlSearch.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelChannelSet)).EndInit();
            this.uiPanelChannelSet.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.uiPanel1)).EndInit();
            this.uiPanel1.ResumeLayout(false);
            this.uiPanel1Container.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdExCategenList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dvGrade)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gradeDs)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanel2)).EndInit();
            this.uiPanel2.ResumeLayout(false);
            this.uiPanel2Container.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdExChannelSetList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dvContractItem)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelUserDetail)).EndInit();
            this.uiPanelUserDetail.ResumeLayout(false);
            this.uiPanelUserDetailContainer.ResumeLayout(false);
            this.pnlUserDetail.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

		}
		#endregion

		#region ��Ʈ�� �ε�
		private void GradeControl_Load(object sender, System.EventArgs e)
		{		
			// �����Ͱ����� ��ü����
			dt = ((DataView)grdExCategenList.DataSource).Table;
			dtChild  = ((DataView)grdExChannelSetList.DataSource).Table;

			cm = (CurrencyManager) this.BindingContext[grdExCategenList.DataSource]; 
			cm.PositionChanged += new System.EventHandler(OnGrdRowChanged); 

			cmChild = (CurrencyManager) this.BindingContext[grdExChannelSetList.DataSource]; 
			cmChild.PositionChanged += new System.EventHandler(OnGrdRowDetailChanged); 		

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
				SearchGrade();
			}
			
			// �߰���ư Ȱ��ȭ
			if(menu.CanCreate(MenuCode))
			{
				canCreate = true;
			}

			// ������ư Ȱ��ȭ
//			if(menu.CanDelete(MenuCode))
//			{
//				canDelete = true;
//			}

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
			SetTextReadonly();
			ProgressStop();
		}

		private void InitCombo()
		{			
			MediaCodeModel mediacodeModel = new MediaCodeModel();		
			new MediaCodeManager(systemModel, commonModel).GetMediaCodeList(mediacodeModel);
			
			if (mediacodeModel.ResultCD.Equals("0000"))
			{
				// �����ͼ¿� ����
				Utility.SetDataTable(gradeDs.Medias, mediacodeModel.MediaCodeDataSet);				
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
				DataRow row = gradeDs.Medias.Rows[i];

				string val = row["MediaCode"].ToString();
				string txt = row["MediaName"].ToString();
				comboItems[i+1] = new Janus.Windows.EditControls.UIComboBoxItem(txt,val);
			}
			// �޺��� ��Ʈ
			this.cbSearchMediaName.Items.AddRange(comboItems);
			this.cbSearchMediaName.SelectedIndex = 0;

			Application.DoEvents();
		}
				
		private void InitCombo_Level()
		{			
			if(commonModel.UserLevel=="20")
			{
				// �޺��Ƚ�						
				cbSearchMediaName.SelectedValue = commonModel.MediaCode;			
				cbSearchMediaName.ReadOnly = true;										
			}
			else
			{
				for(int i=0;i < gradeDs.Medias.Rows.Count;i++)
				{
					DataRow row = gradeDs.Medias.Rows[i];					
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
			Application.DoEvents();
		}

		private void InitButton()
		{
			if(canRead)  
			{
				btnSearch.Enabled = true;				
			}
			if(canCreate) btnAdd.Enabled    = true;
			
			if(ebGrade.Text.Trim().Length > 0) 
			{
				if(canDelete) btnDelete.Enabled = true;
				if(canUpdate) btnSave.Enabled   = true;
			}
			if(IsAdding)
			{
//				if(canCreate) cbMediaName.Enabled    = true;
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

		#region ����� �׼�ó�� �޼ҵ�

		/// <summary>
		/// �׸����� Row�����
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void OnGrdRowChanged(object sender, System.EventArgs e) 
		{
            if (!IsSearching) // 2011.11.29 JH.Park ��ȸ���� �ƴҰ�쿡�� �����ϵ��� ����
            {
                // [E_01]
                if (grdExCategenList.RecordCount > 0)
                {
                    SetCategenDetailText();
                }
                SetTextReadonly();
                InitButton();
            }
		}

		
		private void OnGrdRowDetailChanged(object sender, System.EventArgs e) 
		{
            // [E_01]
			if(grdExChannelSetList.RecordCount > 0 )
			{
				SetGradeDetailText();
			
                //mediaCode_old		= grdExChannelSetList.SelectedItems[0].GetRow().Cells["MediaCode"].Value.ToString();
                //categoryCode_old		= grdExChannelSetList.SelectedItems[0].GetRow().Cells["CategoryCode"].Value.ToString();
                //genreCode_old		= grdExChannelSetList.SelectedItems[0].GetRow().Cells["GenreCode"].Value.ToString();
                //channelNo_old		= grdExChannelSetList.SelectedItems[0].GetRow().Cells["ChannelNo"].Value.ToString();				
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
			if(cbSearchMediaName.SelectedValue.ToString() == "00") 
			{
				FrameSystem.showMsgForm("ä�������˻� ����",new string[] {"", "��ü�� �����Ͽ� �ּ���.", "" });
				return;
			}
			ReSetGradeDetailText();
			ReSetGridData();
			DisableButton();
			SetTextReadonly();			
			SearchGrade();			
			SearchContractItemDetail();	
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

			//btnDetailAdd.Enabled = true;
			IsAdding = true;

			ResetTextReadonly();
			ReSetGradeDetailText();
			//ReSetGridData();

			ebGrade.Focus();
		}

		/// <summary>
		/// �����ư Ŭ��
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnSave_Click(object sender, System.EventArgs e)
		{			
			SaveGradeDetail();		
			//ReSetGridData();
			//SearchGradeDetail();			
		}
		/// <summary>
		/// ������ư Ŭ��
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnDelete_Click(object sender, System.EventArgs e)
		{			
			DeleteChannelSet();		
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
				SearchGrade();
			}
		}

		#endregion

		#region ó���޼ҵ�

		/// <summary>
		/// ä�θ�� ��ȸ
		/// </summary>
		private void SearchGrade()
		{
            IsSearching = true;

			StatusMessage("ä�� ������ ��ȸ�մϴ�.");
			
			try
			{
				gradeModel.Init();
				// �����͸𵨿� ������ ������ ��Ʈ�Ѵ�.				
				if(IsNewSearchKey)
				{
					gradeModel.SearchKey = "";
				}
				else
				{
					gradeModel.SearchKey  = ebSearchKey.Text;
				}

				//ReSetGradeDetailText();

				// ä�θ����ȸ ���񽺸� ȣ���Ѵ�.
				new GradeManager(systemModel,commonModel).GetGradeList(gradeModel);

				if (gradeModel.ResultCD.Equals("0000"))
				{
					Utility.SetDataTable(gradeDs.Grade, gradeModel.GradeDataSet);
					StatusMessage(gradeModel.ResultCnt + "���� ��� ������ ��ȸ�Ǿ����ϴ�.");
					//SearchContractItemDetail();
				}
			}
			catch(FrameException fe)
			{
				FrameSystem.showMsgForm("ä����ȸ����", new string[] {fe.ErrCode, fe.ResultMsg});
			}
			catch(Exception ex)
			{
				FrameSystem.showMsgForm("ä����ȸ����",new string[] {"",ex.Message});
			}
            finally
            {
                IsSearching = false; // ��ȸ�� Flag ����
            }
		}
     
		/// <summary>
		/// ä�θ�� ��ȸ
		/// </summary>
		private void SearchContractItemDetail()
		{
            IsSearching = true;

			StatusMessage("������ ������ ������ ��ȸ�մϴ�.");

			try
			{				
				gradeModel.Init();
				int curRow = cm.Position;          

				if(curRow < 0) return;
											
				gradeModel.MediaCode = cbSearchMediaName.SelectedValue.ToString();
				gradeModel.Code = dt.Rows[curRow]["Code"].ToString();
				
				if(chkAdState_10.Checked)   gradeModel.SearchchkAdState_10   = "Y";
				if(chkAdState_20.Checked)   gradeModel.SearchchkAdState_20   = "Y";
				if(chkAdState_30.Checked)   gradeModel.SearchchkAdState_30   = "Y";
				if(chkAdState_40.Checked)   gradeModel.SearchchkAdState_40   = "Y";

				if(IsNewSearchKey)
				{
					gradeModel.SearchKey = "";
				}
				else
				{
					gradeModel.SearchKey  = ebSearchKey.Text;
				}
			
				// ä�θ����ȸ ���񽺸� ȣ���Ѵ�.
				new GradeManager(systemModel,commonModel).GetContractItemList(gradeModel);
                
                gradeDs.ContractItem.Clear();
				if (gradeModel.ResultCD.Equals("0000"))
				{
                    
					Utility.SetDataTable(gradeDs.ContractItem, gradeModel.ContractItemDataSet);				
					StatusMessage(gradeModel.ResultCnt + "���� ������ ������ ��ȸ�Ǿ����ϴ�.");
					SetGradeDetailText();
				}
				//btnDetailAdd.Enabled = true;
            
				if(gradeModel.ResultCnt > 0)
				{
					//btnDetailDelete.Enabled = true;   
				}
			}
			catch(FrameException fe)
			{
				FrameSystem.showMsgForm("ä����ȸ����", new string[] {fe.ErrCode, fe.ResultMsg});
			}
			catch(Exception ex)
			{
				FrameSystem.showMsgForm("ä����ȸ����",new string[] {"",ex.Message});
			}
            finally
            {
                IsSearching = false; // ��ȸ�� Flag ����
            }
		}

		private void SearchGradeDetail()
		{
			StatusMessage("��� ������ ��ȸ�մϴ�.");

			try
			{
               
				// �����͸𵨿� ������ ������ ��Ʈ�Ѵ�.
                
				int curRow = cm.Position;
             
				ebCode.Text             = dt.Rows[curRow]["Code"].ToString();
				ebGrade.Text			= dt.Rows[curRow]["CodeName"].ToString();
				udGrade.Value			= Convert.ToInt16(dt.Rows[curRow]["Grade"].ToString());
				
				// ä�θ����ȸ ���񽺸� ȣ���Ѵ�.
				new GradeManager(systemModel,commonModel).GetGradeList(gradeModel);

				if (gradeModel.ResultCD.Equals("0000"))
				{
					Utility.SetDataTable(gradeDs.Grade, gradeModel.GradeDataSet);
					StatusMessage(gradeModel.ResultCnt + "���� ä�� ������ ��ȸ�Ǿ����ϴ�.");					
				}			
			}
			catch(FrameException fe)
			{
				FrameSystem.showMsgForm("�����ȸ����", new string[] {fe.ErrCode, fe.ResultMsg});
			}
			catch(Exception ex)
			{
				FrameSystem.showMsgForm("�����ȸ����",new string[] {"",ex.Message});
			}
		}


		/// <summary>
		/// ä�λ����� ����
		/// </summary>
		private void SaveGradeDetail()
		{
			//IsAdding = true;

			StatusMessage("�����õ�� ������ �����մϴ�.");                        
                      
			if(ebCode.Text.ToString().Length == 0) 
			{
				MessageBox.Show("�ڵ尡 �Էµ��� �ʾҽ��ϴ�.","��� ����", 
					MessageBoxButtons.OK, MessageBoxIcon.Information );
				return;								
			}			
			if(ebGrade.Text.ToString().Length == 0) 
			{
				MessageBox.Show("�ڵ���� �Էµ��� �ʾҽ��ϴ�.","��� ����", 
					MessageBoxButtons.OK, MessageBoxIcon.Information );
				return;					
			}
			                        
			try
			{
				//���� ���� ���� �ʱ�ȭ ���ش�.
				gradeModel.Init();
				// �����͸𵨿� ������ ������ ��Ʈ�Ѵ�.				
				gradeModel.Code      = ebCode.Text;
				gradeModel.CodeName  = ebGrade.Text;		
				gradeModel.Grade     = udGrade.Value.ToString();

				gradeModel.Code_O = mediaCode_old;
									
				//gradeModel.ChannelSetDataSet = gradeDs.Copy();                         

				// ä�� ������ ���� ���񽺸� ȣ���Ѵ�.
				if (IsAdding)
				{
					new GradeManager(systemModel,commonModel).SetGradeCreate(gradeModel);
					StatusMessage("��� ������ �߰��Ǿ����ϴ�.");
					IsAdding = false;
					ReSetGradeDetailText();
				}
				else
				{
					new GradeManager(systemModel,commonModel).SetGradeUpdate(gradeModel);
					StatusMessage("��� ������ �����Ǿ����ϴ�.");
				}
				
				DisableButton();
				SearchGradeDetail();
				InitButton();
                        
			}
			catch(FrameException fe)
			{
				FrameSystem.showMsgForm("��� �������", new string[] {fe.ErrCode, fe.ResultMsg});
			}
			catch(Exception ex)
			{
				FrameSystem.showMsgForm("��� �������",new string[] {"",ex.Message});
			}			
		}

		/// <summary>
		/// ä������ ����
		/// </summary>
		private void DeleteChannelSet()
		{
			StatusMessage("ä�� ������ �����մϴ�.");
                        
			                        
			DialogResult result = MessageBox.Show("�ش� ��� ������ ���� �Ͻðڽ��ϱ�?","��� ����",
				MessageBoxButtons.YesNo, MessageBoxIcon.Question,
				MessageBoxDefaultButton.Button2);
                        
			if (result == DialogResult.No) return;
                        
			try
			{
				// �����͸𵨿� ������ ������ ��Ʈ�Ѵ�.				

				gradeModel.Code       = ebCode.Text;
                        
				// ä�� ������ ���� ���񽺸� ȣ���Ѵ�.
				new GradeManager(systemModel,commonModel).SetGradeDelete(gradeModel);
                        			
				ReSetGradeDetailText();			
				SearchGradeDetail();
				StatusMessage("��� ������ �����Ǿ����ϴ�.");			
				//ReSetGridData();
					
				DisableButton();
				SearchGradeDetail();
				InitButton();
			}
			catch(FrameException fe)
			{
				FrameSystem.showMsgForm("��� ��������", new string[] {fe.ErrCode, fe.ResultMsg});
			}
			catch(Exception ex)
			{
				FrameSystem.showMsgForm("��� ��������",new string[] {"",ex.Message});
			}			

		}
		
		/// <summary>
		/// ä�� �������� ��Ʈ
		/// </summary>
		private void SetCategenDetailText()
		{
			int curRow = cm.Position;
            
			udGrade.ReadOnly         = false;			
			ebCode.ReadOnly         = false;			
			ebGrade.ReadOnly         = false;			
			ebRegID.ReadOnly         = false;
			ebRegDt.ReadOnly         = false;	
			ebModDt.ReadOnly         = false;						
						
			udGrade.BackColor        = Color.White;			
			ebCode.BackColor        = Color.White;			
			ebGrade.BackColor        = Color.White;			
			ebRegID.BackColor        = Color.White;						
			ebRegDt.BackColor        = Color.White;						
			ebModDt.BackColor        = Color.White;						

			SearchContractItemDetail();
            
			IsAdding = false;
            
			StatusMessage("�غ�");
		}

		private void SetGradeDetailText()
		{		
			int curRow = cm.Position;
					
			if(curRow >= 0)
			{
				udGrade.Value = Convert.ToInt16(dt.Rows[curRow]["Grade"].ToString());
				ebCode.Text = dt.Rows[curRow]["Code"].ToString();
				ebGrade.Text = dt.Rows[curRow]["CodeName"].ToString();
				ebRegID.Text          = dt.Rows[curRow]["RegID"].ToString();
				ebRegDt.Text          = dt.Rows[curRow]["RegDt"].ToString();
				ebModDt.Text          = dt.Rows[curRow]["ModDt"].ToString();

				IsAdding = false;
				ResetTextReadonly();
			}					
				
			StatusMessage("�غ�");
		}

		private void ReSetGradeDetailText()
		{			
			udGrade.Value                 = 0;			
			ebCode.Text                 = "";
			ebGrade.Text                 = "";
			ebRegID.Text                 = "";
			ebRegDt.Text                 = "";
			ebModDt.Text                 = "";
		}
        
		private void ReSetGridData()
		{			
			//�߰��� �ϸ� ä�μ±׸��带 �����Ѵ�.
			gradeDs.Grade.Clear();        
		}
        
		
		/// <summary>
		/// ������ ReadOnly
		/// </summary>
		private void SetTextReadonly()
		{			
			//udGrade.ReadOnly = true;
			ebCode.ReadOnly = true;
			ebGrade.ReadOnly = true;
			ebRegID.ReadOnly = true;
			ebRegDt.ReadOnly         = true;						
			ebModDt.ReadOnly         = true;

			//udGrade.BackColor = Color.WhiteSmoke;
			ebCode.BackColor = Color.White;
			ebGrade.BackColor = Color.White;
			ebRegID.BackColor = Color.White;
			ebRegDt.BackColor = Color.WhiteSmoke;
			ebModDt.BackColor = Color.WhiteSmoke;			
		}

		/// <summary>
		/// ������ ����������
		/// </summary>
		private void ResetTextReadonly()
		{	
			udGrade.ReadOnly = false; 
			ebCode.ReadOnly = false; 
			ebGrade.ReadOnly = false; 
			ebRegID.ReadOnly = false;
			ebRegDt.ReadOnly         = false;						
			ebModDt.ReadOnly         = false;

			udGrade.BackColor      = Color.White;
			ebCode.BackColor     = Color.White;			
			ebGrade.BackColor     = Color.White;			
			ebRegID.BackColor      = Color.White;			
			ebRegDt.BackColor   = Color.White;
			ebModDt.BackColor   = Color.White;								
		}

		/// <summary>
		/// ������ ����������
		/// </summary>
		private void setSeriesNo()
		{
			int i=1;
			Debug.WriteLine("setSeriesNo1:" + gradeDs.Grade.Rows.Count);
			if(gradeDs.Tables["ChannelSets"].Rows.Count == 1)
			{
				i--;
			}

			//������ ��� ������ 1���̸� 0������ �Է��� �ϰ� w
			//n�� �̸� 1���� �Է��Ѵ�.
			Debug.WriteLine("setSeriesNo21:" + gradeDs.Grade.Rows.Count);
			Debug.WriteLine("setSeriesNo22:" + gradeDs.Tables["ChannelSets"].Rows.Count);

			foreach (DataRow row in gradeDs.Tables["ChannelSets"].Rows)
			{
             
				row["SeriesNo"] = i;
				i++;
			}

			//��ü üũ�ڽ� ǥ�� ������ 0���� �ٲ��� �ʾƼ� ������ ����
			//���� ����� �ذ��� ã���� ����
			Debug.WriteLine("setSeriesNo3:" + gradeDs.Grade.Rows.Count);
            
			if(gradeDs.Tables["ChannelSets"].Rows.Count ==1)
			{
				DataRow row = gradeDs.Tables["ChannelSets"].Rows[0];
				row["SeriesNo"] = 0;
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

		
		private void btnDetailDelete_Click(object sender, System.EventArgs e)
		{
			try
			{
				int i=0;
              
				//grid�� üũ �÷��׸� dtChild�� �������ش�.
				foreach (Janus.Windows.GridEX.GridEXRow gr in grdExChannelSetList.GetRows())
				{
					if(gr.Cells["CheckYn"].Value.ToString().Equals("True"))
					{
						dtChild.Rows[i]["CheckYn"]="True";
					}
					i++;
				} 
				Debug.WriteLine("----------------------------------------------");
				//Debug.WriteLine("Delect_Click1:" + gradeDs.Grade.Rows.Count);

				//grid�� üũ �÷��װ� "True"�ΰ͸� ����. 
				DataRow[] deleteRows = gradeDs.Grade.Select("CheckYn='True'");
                

				Debug.WriteLine("Delect_Click2:" + gradeDs.Grade.Rows.Count);
				for( int j = 0; j < deleteRows.Length;j++)
				{
					deleteRows[j].Delete();
				}
				Debug.WriteLine("Delect_Click3:" + gradeDs.Grade.Rows.Count);
				setSeriesNo();
				Debug.WriteLine("Delect_Click4:" + gradeDs.Grade.Rows.Count);
			}
			catch(Exception ex)
			{
				Debug.WriteLine(ex.Message);
			}
		}


		#region �ܺγ��� �޼ҵ�
		/// <summary>
		/// ���õ� Row���� �Է½�Ŵ
		/// </summary>
		/// <param name="dtc"></param>
		public void adOn_AddContent(DataSet contentsDs)
		{
            
			//���â���� �޾ƿ� DataSet�� Loop��ŭ ������ üũ�� ����
			//�μ�Ʈ ��Ŵ
			for(int i=0;i< contentsDs.Tables["ChannelSets"].Rows.Count;i++)
			{

				DataRow row = contentsDs.Tables["ChannelSets"].Rows[i];

				if(row["CheckYn"].ToString().Equals("True"))
				{

                
					DataRow[] foundRows = dtChild.Select("ContentId = '" + row["ContentId"].ToString() + "'");
                    
					if( foundRows.Length == 0 )
					{
						DataRow newRow = dtChild.NewRow();

						newRow[0] = "False";
						newRow[1] = row["ContentId"].ToString();
						newRow[2] = row["Title"].ToString();
             
						dtChild.Rows.Add(newRow);
						dtChild.Rows.Add(new Object[] {1, "False",2,row["ContentId"].ToString(),3,row["Title"].ToString()});

						newRow = null;
					}
					if( null != foundRows ) foundRows = null;
				}
				if( null != row ) row = null;


			}
			//����������Ʈ�� 0���� Ŭ��쿡 ���� ��ư Ȱ��ȭ
			if(contentsDs.Tables["ChannelSets"].Rows.Count >0)
			{
				//btnDetailDelete.Enabled = true;
			}
			setSeriesNo();
		}

		#endregion

		
	}
}