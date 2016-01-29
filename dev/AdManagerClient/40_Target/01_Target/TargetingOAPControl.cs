// ===============================================================================
// TargetingOapControl for Charites Project
//
// TargetingCMControl.cs
//
// ���� Ÿ���� ��Ʈ���� �����մϴ�. 
// 13: CSS Ÿ���� ������ 1,000,000���� ����, ���� ��������� ���� ������, Ÿ���ø� �����
// ===============================================================================
// Release history
//
// ===============================================================================
// Copyright (C) 2006 G-Inno Systems Inc.
// All rights reserved.
// 

/*
 * -------------------------------------------------------
 * Class Name: TargetingOapControl
 * �ֿ���  : ��ü���� Ÿ����
 * �ۼ���    : ��
 * �ۼ���    : ��
 * Ư�̻���  : �������� Ȯ���� ���� ��� �߰� -bae
 * -------------------------------------------------------
 * [��������]
 * -------------------------------------------------------
 * �����ڵ�  : [E_01]
 * ������    : bae
 * ������    : 2010.07.29
 * ��������  : 
 *            - �������� ����Ʈ(Ʈ��) ���� ����
 *              2�� �������� 3�� ����.
 * �����Լ�  : 
 *            - Init_tvRegion()             
 * --------------------------------------------------------
 * �����ڵ�  : [E_02]
 * ������    : BAE
 * ������    : 2010.09.06
 * �����κ�  :
 *             
 *             getTargetRegion(), setTargetRegion()
 *             clearCheckingRegion(),
 * ��������  :
 * --------------------------------------------------------
  * �����ڵ�  : [E_03]
 * ������    : RH.Jung
 * ������    : 2012.02.21
 * ��������  : 
 *      1. ���� Ÿ���� �� �߰� : �ټ�(�ִ�20��)�� ������ Ÿ���ÿ� ������ �� �ֵ��� ��
 * -------------------------------------------------------- 
 * �����ڵ�  : [E_04]
 * ������    : HJ
 * ������    : 2015.06.02
 * ��������  : 
 *      1. ��ž��, POC Ÿ���� ��� �߰�
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

using Janus.Windows.GridEX;

using AdManagerModel;
using Excel = Microsoft.Office.Interop.Excel; // ���� ����
using System.Reflection;


namespace AdManagerClient
{
    /// <summary>
    /// ���� Ÿ���� ���� ��Ʈ��
    /// </summary>
    public class TargetingOapControl : System.Windows.Forms.UserControl, IUserControl
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
        TargetingModel targetingModel  = new TargetingModel();	// ���� Ÿ��������

        // ȭ��ó���� ����
        bool IsNewSearchKey		  = true;					// �˻����Է� ����
        CurrencyManager cm        = null;					// ������ �׸����� ���濡 ���� �����ͼ� ������ ���Ͽ�			
        DataTable       dt        = null;

        bool IsSearching = false; // ��ȸ�� ��ȭ���� ������Ʈ �Ǵ� ���� ���� �ϱ����� 2011.11.29 JH.Park
        bool canRead			  = false;
        bool canUpdate            = false;

        // Ÿ���ü�������
        private bool isSettedTargeting = false;

        // Key ������
        string keyItemNo    = "";
        string keyMediaCode = "";
		string keyItemName  = "";
        string keyAdType   = "";
		string keyCollectionYn = "N";

		string keyMon     = "";
		string keyThu     = "";
		string keyWed     = "";
		string keyThe     = "";
		string keyFri     = "";
		string keySat     = "";
		string keySun     = "";

		// Ʈ�����
		private bool canChecking = true;
		private int tvTimeNodeCount = 0;
		private int tvAgeNodeCount = 0;
        private int tvStbNodeCount = 0;

        public string ItemNo
        {
            get { return keyItemNo;	}
            set { keyItemNo = value;	}
        }

        public string MediaCode
        {
            get { return keyMediaCode;	}
            set { keyMediaCode = value;	}
        }

        private DataView dvTargetingCollection;
        private Janus.Windows.UI.Tab.UITabPage uiTabPage3;
        private Label label63;
        private Label label62;
        private Panel panel4;
        private Janus.Windows.GridEX.GridEX grdExSouceCollectionList;
        private Panel panel1;
        private Janus.Windows.GridEX.GridEX grdExTargetList;
        private Label label60;
        private Panel panel6;
        private Label lbMsg2;
        private Janus.Windows.EditControls.UIButton btnAddCollection;
        private Panel panel5;
        private Label label59;
        private Janus.Windows.EditControls.UIButton btnDeleteCollection;
		private Janus.Windows.EditControls.UIButton btnAddCollectionMinus;
        private Janus.Windows.EditControls.UIGroupBox grpStb;
        private TreeView tvStb;
        private Janus.Windows.EditControls.UICheckBox chkStbModel;
        private Janus.Windows.EditControls.UIGroupBox uiGroupBox1;
        private TreeView tvPoc;
        private Janus.Windows.EditControls.UICheckBox chkPoc;
		private Label lblCollMsg;
		
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
            this.OnLoad(null);
        }
        #endregion

        #region ȭ�� ������Ʈ, ������, �Ҹ���

        private Janus.Windows.UI.Dock.UIPanelManager uiPM;
        private System.Windows.Forms.Panel pnlSearch;
        private System.Windows.Forms.Panel pnlSearchBtn;
        private Janus.Windows.EditControls.UIButton btnSearch;
        private Janus.Windows.UI.Dock.UIPanel uiPanelSearch;
        private Janus.Windows.UI.Dock.UIPanelInnerContainer uiPanelSearchContainer;
        private Janus.Windows.UI.Dock.UIPanel uiPanelList;
        private Janus.Windows.UI.Dock.UIPanelInnerContainer uiPanelListContainer;
        private Janus.Windows.GridEX.EditControls.EditBox ebSearchKey;
        private Janus.Windows.UI.Dock.UIPanelGroup uiPanelChoiceAdSchedule;
        private Janus.Windows.EditControls.UICheckBox chkAdState_20;
        private Janus.Windows.EditControls.UIComboBox cbSearchMedia;
        private Janus.Windows.EditControls.UIComboBox cbSearchRap;
        private Janus.Windows.EditControls.UIComboBox cbSearchAgency;
        private Janus.Windows.EditControls.UICheckBox chkAdState_40;
        private Janus.Windows.GridEX.GridEX grdExScheduleList;
        private Janus.Windows.EditControls.UICheckBox chkAdState_30;
        private System.Windows.Forms.Label label1;
        private Janus.Windows.GridEX.EditControls.EditBox editBox1;
        private System.Windows.Forms.Panel panMenuSchedule;
        private System.Windows.Forms.Panel panel2;
        private AdManagerClient.TargetingDs targetingDs;
        private Janus.Windows.UI.Dock.UIPanel uiPanelDetail;
        private Janus.Windows.UI.Dock.UIPanelInnerContainer uiPanelDetailContainer;
        private System.Windows.Forms.Panel panel3;
		private System.Data.DataView dvTargeting;
		private Janus.Windows.EditControls.UIButton btnExcel;
		private System.Data.DataView dvCollection;
		private Janus.Windows.UI.Tab.UITab uiTab1;
		private Janus.Windows.UI.Tab.UITabPage uiTabPage1;
		private Janus.Windows.GridEX.EditControls.IntegerUpDown udControlRate;
		private Janus.Windows.GridEX.EditControls.NumericEditBox ebContractAmt;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label lbUserID;
		private Janus.Windows.EditControls.UIComboBox cbPriorityCd;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Label label7;
        private Janus.Windows.EditControls.UICheckBox chkCollectionYn;
		private Janus.Windows.EditControls.UICheckBox chkWeekYn;
		private Janus.Windows.EditControls.UIGroupBox grpWeek;
		private Janus.Windows.EditControls.UICheckBox chkSun;
		private Janus.Windows.EditControls.UICheckBox chkSat;
		private Janus.Windows.EditControls.UICheckBox chkFri;
		private Janus.Windows.EditControls.UICheckBox chkThe;
		private Janus.Windows.EditControls.UICheckBox chkWed;
		private Janus.Windows.EditControls.UICheckBox chkMon;
		private Janus.Windows.EditControls.UICheckBox chkThu;
		private System.Windows.Forms.Label lbNotice;
		private Janus.Windows.EditControls.UICheckBox chkRateYn;
		private Janus.Windows.EditControls.UICheckBox chkTimeYn;
		private Janus.Windows.EditControls.UICheckBox chkRegionYn;
		private System.Windows.Forms.TreeView tvTime;
		private Janus.Windows.EditControls.UIGroupBox grpRate;
		private Janus.Windows.EditControls.UIRadioButton rbRate19;
		private Janus.Windows.EditControls.UIRadioButton rbRate15;
		private Janus.Windows.EditControls.UIRadioButton rbRate12;
		private Janus.Windows.EditControls.UIRadioButton rbRateAll;
		private Janus.Windows.EditControls.UIButton btnSave;
		private System.Windows.Forms.TreeView tvAge;
		private Janus.Windows.EditControls.UICheckBox chkAgeYn;
		private Janus.Windows.EditControls.UICheckBox chkSexYn;
		private Janus.Windows.EditControls.UIGroupBox grpSex;
		private Janus.Windows.EditControls.UICheckBox chkSexMan;
		private Janus.Windows.EditControls.UICheckBox chkSexWoman;
		private Janus.Windows.EditControls.UIRadioButton rbControlYn_Y;
		private Janus.Windows.EditControls.UIRadioButton rbControlYn_N;
		private Janus.Windows.EditControls.UICheckBox chkAgeBtnYn;
		private Janus.Windows.GridEX.EditControls.IntegerUpDown udAgeBtnBegin;
		private System.Windows.Forms.Label label4;
		private Janus.Windows.GridEX.EditControls.IntegerUpDown udAgeBtnEnd;
		private System.Windows.Forms.Label label5;
        private Janus.Windows.EditControls.UIGroupBox grpRateType;
        private Janus.Windows.EditControls.UIRadioButton rbRateMinus;
        private Janus.Windows.EditControls.UIRadioButton rbRatePlus;
        private System.Windows.Forms.ToolTip ttMsg;
		private System.Windows.Forms.Label label58;
		private Janus.Windows.EditControls.UIGroupBox uiGroupBox3;
		private System.Windows.Forms.ImageList imgRegion;
		private System.Windows.Forms.TreeListView tlvRegion;
		private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.ComponentModel.IContainer components;

        public TargetingOapControl()
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
            Janus.Windows.EditControls.UIComboBoxItem uiComboBoxItem1 = new Janus.Windows.EditControls.UIComboBoxItem();
            Janus.Windows.GridEX.GridEXLayout grdExScheduleList_DesignTimeLayout = new Janus.Windows.GridEX.GridEXLayout();
            Janus.Windows.Common.Layouts.JanusLayoutReference grdExScheduleList_DesignTimeLayout_Reference_0 = new Janus.Windows.Common.Layouts.JanusLayoutReference("GridEXLayoutData.RootTable.FormatConditions.Condition5.FormatStyle.BackgroundImag" +
                    "e");
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TargetingOapControl));
            Janus.Windows.Common.Layouts.JanusLayoutReference grdExScheduleList_DesignTimeLayout_Reference_1 = new Janus.Windows.Common.Layouts.JanusLayoutReference("GridEXLayoutData.RootTable.FormatConditions.Condition6.FormatStyle.BackgroundImag" +
                    "e");
            Janus.Windows.Common.Layouts.JanusLayoutReference grdExScheduleList_DesignTimeLayout_Reference_2 = new Janus.Windows.Common.Layouts.JanusLayoutReference("GridEXLayoutData.FormatStyles.Style0.BackgroundImage");
            System.Windows.Forms.TreeNode treeNode1 = new System.Windows.Forms.TreeNode("POC ����");
            System.Windows.Forms.TreeNode treeNode2 = new System.Windows.Forms.TreeNode("��ž�𵨺�");
            System.Windows.Forms.TreeListViewItemCollection.TreeListViewItemCollectionComparer treeListViewItemCollectionComparer1 = new System.Windows.Forms.TreeListViewItemCollection.TreeListViewItemCollectionComparer();
            System.Windows.Forms.TreeNode treeNode3 = new System.Windows.Forms.TreeNode("00��~01��");
            System.Windows.Forms.TreeNode treeNode4 = new System.Windows.Forms.TreeNode("01��~02��");
            System.Windows.Forms.TreeNode treeNode5 = new System.Windows.Forms.TreeNode("02��~03��");
            System.Windows.Forms.TreeNode treeNode6 = new System.Windows.Forms.TreeNode("03��~04��");
            System.Windows.Forms.TreeNode treeNode7 = new System.Windows.Forms.TreeNode("��ü�ð���", new System.Windows.Forms.TreeNode[] {
            treeNode3,
            treeNode4,
            treeNode5,
            treeNode6});
            System.Windows.Forms.TreeNode treeNode8 = new System.Windows.Forms.TreeNode("0~19��");
            System.Windows.Forms.TreeNode treeNode9 = new System.Windows.Forms.TreeNode("20~29��");
            System.Windows.Forms.TreeNode treeNode10 = new System.Windows.Forms.TreeNode("30~39��");
            System.Windows.Forms.TreeNode treeNode11 = new System.Windows.Forms.TreeNode("40~49��");
            System.Windows.Forms.TreeNode treeNode12 = new System.Windows.Forms.TreeNode("50~59��");
            System.Windows.Forms.TreeNode treeNode13 = new System.Windows.Forms.TreeNode("60���̻�");
            System.Windows.Forms.TreeNode treeNode14 = new System.Windows.Forms.TreeNode("��ü���ɴ�", new System.Windows.Forms.TreeNode[] {
            treeNode8,
            treeNode9,
            treeNode10,
            treeNode11,
            treeNode12,
            treeNode13});
            Janus.Windows.GridEX.GridEXLayout grdExSouceCollectionList_DesignTimeLayout = new Janus.Windows.GridEX.GridEXLayout();
            Janus.Windows.GridEX.GridEXLayout grdExTargetList_DesignTimeLayout = new Janus.Windows.GridEX.GridEXLayout();
            this.targetingDs = new AdManagerClient.TargetingDs();
            this.uiPM = new Janus.Windows.UI.Dock.UIPanelManager(this.components);
            this.uiPanelChoiceAdSchedule = new Janus.Windows.UI.Dock.UIPanelGroup();
            this.uiPanelSearch = new Janus.Windows.UI.Dock.UIPanel();
            this.uiPanelSearchContainer = new Janus.Windows.UI.Dock.UIPanelInnerContainer();
            this.pnlSearch = new System.Windows.Forms.Panel();
            this.chkAdState_20 = new Janus.Windows.EditControls.UICheckBox();
            this.ebSearchKey = new Janus.Windows.GridEX.EditControls.EditBox();
            this.cbSearchMedia = new Janus.Windows.EditControls.UIComboBox();
            this.cbSearchRap = new Janus.Windows.EditControls.UIComboBox();
            this.cbSearchAgency = new Janus.Windows.EditControls.UIComboBox();
            this.pnlSearchBtn = new System.Windows.Forms.Panel();
            this.btnExcel = new Janus.Windows.EditControls.UIButton();
            this.btnSearch = new Janus.Windows.EditControls.UIButton();
            this.chkAdState_30 = new Janus.Windows.EditControls.UICheckBox();
            this.chkAdState_40 = new Janus.Windows.EditControls.UICheckBox();
            this.uiPanelList = new Janus.Windows.UI.Dock.UIPanel();
            this.uiPanelListContainer = new Janus.Windows.UI.Dock.UIPanelInnerContainer();
            this.grdExScheduleList = new Janus.Windows.GridEX.GridEX();
            this.dvTargeting = new System.Data.DataView();
            this.uiPanelDetail = new Janus.Windows.UI.Dock.UIPanel();
            this.uiPanelDetailContainer = new Janus.Windows.UI.Dock.UIPanelInnerContainer();
            this.panel3 = new System.Windows.Forms.Panel();
            this.uiTab1 = new Janus.Windows.UI.Tab.UITab();
            this.uiTabPage1 = new Janus.Windows.UI.Tab.UITabPage();
            this.uiGroupBox1 = new Janus.Windows.EditControls.UIGroupBox();
            this.tvPoc = new System.Windows.Forms.TreeView();
            this.chkPoc = new Janus.Windows.EditControls.UICheckBox();
            this.chkStbModel = new Janus.Windows.EditControls.UICheckBox();
            this.grpStb = new Janus.Windows.EditControls.UIGroupBox();
            this.tvStb = new System.Windows.Forms.TreeView();
            this.lblCollMsg = new System.Windows.Forms.Label();
            this.label60 = new System.Windows.Forms.Label();
            this.tlvRegion = new System.Windows.Forms.TreeListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.imgRegion = new System.Windows.Forms.ImageList(this.components);
            this.uiGroupBox3 = new Janus.Windows.EditControls.UIGroupBox();
            this.lbNotice = new System.Windows.Forms.Label();
            this.label58 = new System.Windows.Forms.Label();
            this.grpRateType = new Janus.Windows.EditControls.UIGroupBox();
            this.rbRateMinus = new Janus.Windows.EditControls.UIRadioButton();
            this.rbRatePlus = new Janus.Windows.EditControls.UIRadioButton();
            this.udControlRate = new Janus.Windows.GridEX.EditControls.IntegerUpDown();
            this.ebContractAmt = new Janus.Windows.GridEX.EditControls.NumericEditBox();
            this.label3 = new System.Windows.Forms.Label();
            this.lbUserID = new System.Windows.Forms.Label();
            this.cbPriorityCd = new Janus.Windows.EditControls.UIComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.chkCollectionYn = new Janus.Windows.EditControls.UICheckBox();
            this.chkWeekYn = new Janus.Windows.EditControls.UICheckBox();
            this.grpWeek = new Janus.Windows.EditControls.UIGroupBox();
            this.chkSun = new Janus.Windows.EditControls.UICheckBox();
            this.chkSat = new Janus.Windows.EditControls.UICheckBox();
            this.chkFri = new Janus.Windows.EditControls.UICheckBox();
            this.chkThe = new Janus.Windows.EditControls.UICheckBox();
            this.chkWed = new Janus.Windows.EditControls.UICheckBox();
            this.chkMon = new Janus.Windows.EditControls.UICheckBox();
            this.chkThu = new Janus.Windows.EditControls.UICheckBox();
            this.chkRateYn = new Janus.Windows.EditControls.UICheckBox();
            this.chkTimeYn = new Janus.Windows.EditControls.UICheckBox();
            this.chkRegionYn = new Janus.Windows.EditControls.UICheckBox();
            this.tvTime = new System.Windows.Forms.TreeView();
            this.grpRate = new Janus.Windows.EditControls.UIGroupBox();
            this.rbRate19 = new Janus.Windows.EditControls.UIRadioButton();
            this.rbRate15 = new Janus.Windows.EditControls.UIRadioButton();
            this.rbRate12 = new Janus.Windows.EditControls.UIRadioButton();
            this.rbRateAll = new Janus.Windows.EditControls.UIRadioButton();
            this.btnSave = new Janus.Windows.EditControls.UIButton();
            this.tvAge = new System.Windows.Forms.TreeView();
            this.chkAgeYn = new Janus.Windows.EditControls.UICheckBox();
            this.chkSexYn = new Janus.Windows.EditControls.UICheckBox();
            this.grpSex = new Janus.Windows.EditControls.UIGroupBox();
            this.chkSexMan = new Janus.Windows.EditControls.UICheckBox();
            this.chkSexWoman = new Janus.Windows.EditControls.UICheckBox();
            this.chkAgeBtnYn = new Janus.Windows.EditControls.UICheckBox();
            this.udAgeBtnBegin = new Janus.Windows.GridEX.EditControls.IntegerUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.udAgeBtnEnd = new Janus.Windows.GridEX.EditControls.IntegerUpDown();
            this.label5 = new System.Windows.Forms.Label();
            this.rbControlYn_Y = new Janus.Windows.EditControls.UIRadioButton();
            this.rbControlYn_N = new Janus.Windows.EditControls.UIRadioButton();
            this.uiTabPage3 = new Janus.Windows.UI.Tab.UITabPage();
            this.label63 = new System.Windows.Forms.Label();
            this.label62 = new System.Windows.Forms.Label();
            this.panel4 = new System.Windows.Forms.Panel();
            this.grdExSouceCollectionList = new Janus.Windows.GridEX.GridEX();
            this.dvCollection = new System.Data.DataView();
            this.panel6 = new System.Windows.Forms.Panel();
            this.btnAddCollectionMinus = new Janus.Windows.EditControls.UIButton();
            this.lbMsg2 = new System.Windows.Forms.Label();
            this.btnAddCollection = new Janus.Windows.EditControls.UIButton();
            this.panel1 = new System.Windows.Forms.Panel();
            this.grdExTargetList = new Janus.Windows.GridEX.GridEX();
            this.dvTargetingCollection = new System.Data.DataView();
            this.panel5 = new System.Windows.Forms.Panel();
            this.label59 = new System.Windows.Forms.Label();
            this.btnDeleteCollection = new Janus.Windows.EditControls.UIButton();
            this.panMenuSchedule = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.editBox1 = new Janus.Windows.GridEX.EditControls.EditBox();
            this.ttMsg = new System.Windows.Forms.ToolTip(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.targetingDs)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiPM)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelChoiceAdSchedule)).BeginInit();
            this.uiPanelChoiceAdSchedule.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelSearch)).BeginInit();
            this.uiPanelSearch.SuspendLayout();
            this.uiPanelSearchContainer.SuspendLayout();
            this.pnlSearch.SuspendLayout();
            this.pnlSearchBtn.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelList)).BeginInit();
            this.uiPanelList.SuspendLayout();
            this.uiPanelListContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdExScheduleList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dvTargeting)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelDetail)).BeginInit();
            this.uiPanelDetail.SuspendLayout();
            this.uiPanelDetailContainer.SuspendLayout();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uiTab1)).BeginInit();
            this.uiTab1.SuspendLayout();
            this.uiTabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uiGroupBox1)).BeginInit();
            this.uiGroupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grpStb)).BeginInit();
            this.grpStb.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uiGroupBox3)).BeginInit();
            this.uiGroupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grpRateType)).BeginInit();
            this.grpRateType.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grpWeek)).BeginInit();
            this.grpWeek.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grpRate)).BeginInit();
            this.grpRate.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grpSex)).BeginInit();
            this.grpSex.SuspendLayout();
            this.uiTabPage3.SuspendLayout();
            this.panel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdExSouceCollectionList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dvCollection)).BeginInit();
            this.panel6.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdExTargetList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dvTargetingCollection)).BeginInit();
            this.panel5.SuspendLayout();
            this.panMenuSchedule.SuspendLayout();
            this.SuspendLayout();
            // 
            // targetingDs
            // 
            this.targetingDs.DataSetName = "TargetingDs";
            this.targetingDs.Locale = new System.Globalization.CultureInfo("en-US");
            this.targetingDs.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
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
            this.uiPanelList.Id = new System.Guid("8e3140b9-bfcb-43af-b84e-2ee6cddf51dc");
            this.uiPanelChoiceAdSchedule.Panels.Add(this.uiPanelList);
            this.uiPanelDetail.Id = new System.Guid("f94be314-c212-42b8-b676-497c4d5f5485");
            this.uiPanelChoiceAdSchedule.Panels.Add(this.uiPanelDetail);
            this.uiPM.Panels.Add(this.uiPanelChoiceAdSchedule);
            // 
            // Design Time Panel Info:
            // 
            this.uiPM.BeginPanelInfo();
            this.uiPM.AddDockPanelInfo(new System.Guid("b162c53a-7940-47cd-a0ed-9762c59c50d8"), Janus.Windows.UI.Dock.PanelGroupStyle.HorizontalTiles, Janus.Windows.UI.Dock.PanelDockStyle.Fill, true, new System.Drawing.Size(1010, 677), true);
            this.uiPM.AddDockPanelInfo(new System.Guid("afb3b7d0-aa79-443c-a1e8-3baba38f7a4d"), new System.Guid("b162c53a-7940-47cd-a0ed-9762c59c50d8"), 38, true);
            this.uiPM.AddDockPanelInfo(new System.Guid("8e3140b9-bfcb-43af-b84e-2ee6cddf51dc"), new System.Guid("b162c53a-7940-47cd-a0ed-9762c59c50d8"), 187, true);
            this.uiPM.AddDockPanelInfo(new System.Guid("f94be314-c212-42b8-b676-497c4d5f5485"), new System.Guid("b162c53a-7940-47cd-a0ed-9762c59c50d8"), 402, true);
            this.uiPM.AddFloatingPanelInfo(new System.Guid("b162c53a-7940-47cd-a0ed-9762c59c50d8"), Janus.Windows.UI.Dock.PanelGroupStyle.HorizontalTiles, true, new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
            this.uiPM.AddFloatingPanelInfo(new System.Guid("8e3140b9-bfcb-43af-b84e-2ee6cddf51dc"), new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
            this.uiPM.AddFloatingPanelInfo(new System.Guid("b2973a57-d3e1-4d80-9387-7a6dc3332542"), new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
            this.uiPM.AddFloatingPanelInfo(new System.Guid("afb3b7d0-aa79-443c-a1e8-3baba38f7a4d"), new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
            this.uiPM.AddFloatingPanelInfo(new System.Guid("18e78aea-bf80-4094-94f6-0d7d6006bb3d"), Janus.Windows.UI.Dock.PanelGroupStyle.VerticalTiles, true, new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
            this.uiPM.AddFloatingPanelInfo(new System.Guid("02c920f2-57c5-4aa8-bf42-309880d3314e"), new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
            this.uiPM.AddFloatingPanelInfo(new System.Guid("f40b18e5-7e83-4164-be4f-97c23451198d"), new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
            this.uiPM.AddFloatingPanelInfo(new System.Guid("f94be314-c212-42b8-b676-497c4d5f5485"), new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
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
            this.uiPanelChoiceAdSchedule.Text = "��ü���� Ÿ������ ����";
            // 
            // uiPanelSearch
            // 
            this.uiPanelSearch.AllowResize = Janus.Windows.UI.InheritableBoolean.False;
            this.uiPanelSearch.CaptionVisible = Janus.Windows.UI.InheritableBoolean.False;
            this.uiPanelSearch.CloseButtonVisible = Janus.Windows.UI.InheritableBoolean.False;
            this.uiPanelSearch.InnerContainer = this.uiPanelSearchContainer;
            this.uiPanelSearch.Location = new System.Drawing.Point(0, 22);
            this.uiPanelSearch.Name = "uiPanelSearch";
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
            this.pnlSearch.Controls.Add(this.chkAdState_20);
            this.pnlSearch.Controls.Add(this.ebSearchKey);
            this.pnlSearch.Controls.Add(this.cbSearchMedia);
            this.pnlSearch.Controls.Add(this.cbSearchRap);
            this.pnlSearch.Controls.Add(this.cbSearchAgency);
            this.pnlSearch.Controls.Add(this.pnlSearchBtn);
            this.pnlSearch.Controls.Add(this.chkAdState_30);
            this.pnlSearch.Controls.Add(this.chkAdState_40);
            this.pnlSearch.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlSearch.Location = new System.Drawing.Point(0, 0);
            this.pnlSearch.Name = "pnlSearch";
            this.pnlSearch.Size = new System.Drawing.Size(1008, 38);
            this.pnlSearch.TabIndex = 3;
            // 
            // chkAdState_20
            // 
            this.chkAdState_20.Checked = true;
            this.chkAdState_20.CheckedValue = "";
            this.chkAdState_20.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkAdState_20.Font = new System.Drawing.Font("���� ���", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.chkAdState_20.ForeColor = System.Drawing.SystemColors.ControlText;
            this.chkAdState_20.Location = new System.Drawing.Point(372, 11);
            this.chkAdState_20.Name = "chkAdState_20";
            this.chkAdState_20.Size = new System.Drawing.Size(44, 20);
            this.chkAdState_20.TabIndex = 8;
            this.chkAdState_20.Text = "��";
            this.chkAdState_20.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            // 
            // ebSearchKey
            // 
            this.ebSearchKey.Location = new System.Drawing.Point(512, 11);
            this.ebSearchKey.MaxLength = 50;
            this.ebSearchKey.Name = "ebSearchKey";
            this.ebSearchKey.Size = new System.Drawing.Size(160, 21);
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
            this.cbSearchMedia.AutoSize = false;
            this.cbSearchMedia.BackColor = System.Drawing.Color.White;
            this.cbSearchMedia.ComboStyle = Janus.Windows.EditControls.ComboStyle.DropDownList;
            uiComboBoxItem1.FormatStyle.Alpha = 0;
            uiComboBoxItem1.IsSeparator = false;
            uiComboBoxItem1.Text = "��ε��TV";
            this.cbSearchMedia.Items.AddRange(new Janus.Windows.EditControls.UIComboBoxItem[] {
            uiComboBoxItem1});
            this.cbSearchMedia.Location = new System.Drawing.Point(8, 11);
            this.cbSearchMedia.Name = "cbSearchMedia";
            this.cbSearchMedia.Size = new System.Drawing.Size(112, 20);
            this.cbSearchMedia.TabIndex = 1;
            this.cbSearchMedia.Text = "��ü����";
            this.cbSearchMedia.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            // 
            // cbSearchRap
            // 
            this.cbSearchRap.AutoSize = false;
            this.cbSearchRap.ComboStyle = Janus.Windows.EditControls.ComboStyle.DropDownList;
            this.cbSearchRap.Location = new System.Drawing.Point(122, 11);
            this.cbSearchRap.Name = "cbSearchRap";
            this.cbSearchRap.Size = new System.Drawing.Size(120, 20);
            this.cbSearchRap.TabIndex = 2;
            this.cbSearchRap.Text = "�̵�����";
            this.cbSearchRap.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            // 
            // cbSearchAgency
            // 
            this.cbSearchAgency.AutoSize = false;
            this.cbSearchAgency.ComboStyle = Janus.Windows.EditControls.ComboStyle.DropDownList;
            this.cbSearchAgency.Location = new System.Drawing.Point(246, 11);
            this.cbSearchAgency.Name = "cbSearchAgency";
            this.cbSearchAgency.Size = new System.Drawing.Size(120, 20);
            this.cbSearchAgency.TabIndex = 3;
            this.cbSearchAgency.Text = "����缱��";
            this.cbSearchAgency.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            // 
            // pnlSearchBtn
            // 
            this.pnlSearchBtn.BackColor = System.Drawing.SystemColors.Window;
            this.pnlSearchBtn.Controls.Add(this.btnExcel);
            this.pnlSearchBtn.Controls.Add(this.btnSearch);
            this.pnlSearchBtn.Dock = System.Windows.Forms.DockStyle.Right;
            this.pnlSearchBtn.Location = new System.Drawing.Point(830, 0);
            this.pnlSearchBtn.Name = "pnlSearchBtn";
            this.pnlSearchBtn.Size = new System.Drawing.Size(178, 38);
            this.pnlSearchBtn.TabIndex = 11;
            // 
            // btnExcel
            // 
            this.btnExcel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExcel.Enabled = false;
            this.btnExcel.Location = new System.Drawing.Point(92, 8);
            this.btnExcel.Name = "btnExcel";
            this.btnExcel.Size = new System.Drawing.Size(80, 24);
            this.btnExcel.TabIndex = 12;
            this.btnExcel.Text = "EXCEL";
            this.btnExcel.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.btnExcel.Click += new System.EventHandler(this.btnExcel_Click);
            // 
            // btnSearch
            // 
            this.btnSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSearch.Enabled = false;
            this.btnSearch.Location = new System.Drawing.Point(6, 8);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(80, 24);
            this.btnSearch.TabIndex = 11;
            this.btnSearch.Text = "�� ȸ";
            this.btnSearch.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // chkAdState_30
            // 
            this.chkAdState_30.BackColor = System.Drawing.SystemColors.Window;
            this.chkAdState_30.Checked = true;
            this.chkAdState_30.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkAdState_30.Font = new System.Drawing.Font("���� ���", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.chkAdState_30.Location = new System.Drawing.Point(416, 11);
            this.chkAdState_30.Name = "chkAdState_30";
            this.chkAdState_30.Size = new System.Drawing.Size(44, 20);
            this.chkAdState_30.TabIndex = 9;
            this.chkAdState_30.Text = "����";
            this.chkAdState_30.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            // 
            // chkAdState_40
            // 
            this.chkAdState_40.Font = new System.Drawing.Font("���� ���", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.chkAdState_40.Location = new System.Drawing.Point(460, 11);
            this.chkAdState_40.Name = "chkAdState_40";
            this.chkAdState_40.Size = new System.Drawing.Size(44, 20);
            this.chkAdState_40.TabIndex = 10;
            this.chkAdState_40.Text = "����";
            this.chkAdState_40.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            // 
            // uiPanelList
            // 
            this.uiPanelList.BackColor = System.Drawing.SystemColors.Window;
            this.uiPanelList.CaptionStyle = Janus.Windows.UI.Dock.PanelCaptionStyle.Dark;
            this.uiPanelList.CaptionVisible = Janus.Windows.UI.InheritableBoolean.False;
            this.uiPanelList.InnerContainer = this.uiPanelListContainer;
            this.uiPanelList.Location = new System.Drawing.Point(0, 66);
            this.uiPanelList.MinimumSize = new System.Drawing.Size(-1, 100);
            this.uiPanelList.Name = "uiPanelList";
            this.uiPanelList.Size = new System.Drawing.Size(1010, 193);
            this.uiPanelList.TabIndex = 4;
            this.uiPanelList.Text = "���������";
            // 
            // uiPanelListContainer
            // 
            this.uiPanelListContainer.BackColor = System.Drawing.SystemColors.Window;
            this.uiPanelListContainer.Controls.Add(this.grdExScheduleList);
            this.uiPanelListContainer.Location = new System.Drawing.Point(1, 1);
            this.uiPanelListContainer.Name = "uiPanelListContainer";
            this.uiPanelListContainer.Size = new System.Drawing.Size(1008, 191);
            this.uiPanelListContainer.TabIndex = 0;
            // 
            // grdExScheduleList
            // 
            this.grdExScheduleList.AllowEdit = Janus.Windows.GridEX.InheritableBoolean.False;
            this.grdExScheduleList.AlternatingColors = true;
            this.grdExScheduleList.BorderStyle = Janus.Windows.GridEX.BorderStyle.None;
            this.grdExScheduleList.DataSource = this.dvTargeting;
            grdExScheduleList_DesignTimeLayout_Reference_0.Instance = ((object)(resources.GetObject("grdExScheduleList_DesignTimeLayout_Reference_0.Instance")));
            grdExScheduleList_DesignTimeLayout_Reference_1.Instance = ((object)(resources.GetObject("grdExScheduleList_DesignTimeLayout_Reference_1.Instance")));
            grdExScheduleList_DesignTimeLayout_Reference_2.Instance = ((object)(resources.GetObject("grdExScheduleList_DesignTimeLayout_Reference_2.Instance")));
            grdExScheduleList_DesignTimeLayout.LayoutReferences.AddRange(new Janus.Windows.Common.Layouts.JanusLayoutReference[] {
            grdExScheduleList_DesignTimeLayout_Reference_0,
            grdExScheduleList_DesignTimeLayout_Reference_1,
            grdExScheduleList_DesignTimeLayout_Reference_2});
            grdExScheduleList_DesignTimeLayout.LayoutString = resources.GetString("grdExScheduleList_DesignTimeLayout.LayoutString");
            this.grdExScheduleList.DesignTimeLayout = grdExScheduleList_DesignTimeLayout;
            this.grdExScheduleList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdExScheduleList.EmptyRows = true;
            this.grdExScheduleList.FocusCellFormatStyle.BackColor = System.Drawing.SystemColors.Highlight;
            this.grdExScheduleList.FocusCellFormatStyle.ForeColor = System.Drawing.Color.White;
            this.grdExScheduleList.FocusStyle = Janus.Windows.GridEX.FocusStyle.None;
            this.grdExScheduleList.Font = new System.Drawing.Font("�������", 8.249999F);
            this.grdExScheduleList.FrozenColumns = 2;
            this.grdExScheduleList.GridLineColor = System.Drawing.Color.Silver;
            this.grdExScheduleList.GridLines = Janus.Windows.GridEX.GridLines.Vertical;
            this.grdExScheduleList.GridLineStyle = Janus.Windows.GridEX.GridLineStyle.Solid;
            this.grdExScheduleList.GroupByBoxVisible = false;
            this.grdExScheduleList.HeaderFormatStyle.TextAlignment = Janus.Windows.GridEX.TextAlignment.Center;
            this.grdExScheduleList.HideSelection = Janus.Windows.GridEX.HideSelection.Highlight;
            this.grdExScheduleList.Location = new System.Drawing.Point(0, 0);
            this.grdExScheduleList.Name = "grdExScheduleList";
            this.grdExScheduleList.ScrollBars = Janus.Windows.GridEX.ScrollBars.Vertical;
            this.grdExScheduleList.SelectedFormatStyle.FontBold = Janus.Windows.GridEX.TriState.True;
            this.grdExScheduleList.SelectedInactiveFormatStyle.BackColor = System.Drawing.Color.Gold;
            this.grdExScheduleList.Size = new System.Drawing.Size(1008, 191);
            this.grdExScheduleList.TabIndex = 12;
            this.grdExScheduleList.TabKeyBehavior = Janus.Windows.GridEX.TabKeyBehavior.ControlNavigation;
            this.grdExScheduleList.ThemedAreas = ((Janus.Windows.GridEX.ThemedArea)(((((((((Janus.Windows.GridEX.ThemedArea.ScrollBars | Janus.Windows.GridEX.ThemedArea.EditControls)
                        | Janus.Windows.GridEX.ThemedArea.Headers)
                        | Janus.Windows.GridEX.ThemedArea.GroupByBox)
                        | Janus.Windows.GridEX.ThemedArea.GroupRows)
                        | Janus.Windows.GridEX.ThemedArea.ControlBorder)
                        | Janus.Windows.GridEX.ThemedArea.Cards)
                        | Janus.Windows.GridEX.ThemedArea.Gridlines)
                        | Janus.Windows.GridEX.ThemedArea.CheckBoxes)));
            this.grdExScheduleList.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            this.grdExScheduleList.Enter += new System.EventHandler(this.OnGrdRowChanged);
            // 
            // dvTargeting
            // 
            this.dvTargeting.Table = this.targetingDs.TargetList;
            // 
            // uiPanelDetail
            // 
            this.uiPanelDetail.ActiveCaptionMode = Janus.Windows.UI.Dock.ActiveCaptionMode.Never;
            this.uiPanelDetail.CaptionStyle = Janus.Windows.UI.Dock.PanelCaptionStyle.Dark;
            this.uiPanelDetail.InnerAreaStyle = Janus.Windows.UI.Dock.PanelInnerAreaStyle.Window;
            this.uiPanelDetail.InnerContainer = this.uiPanelDetailContainer;
            this.uiPanelDetail.Location = new System.Drawing.Point(0, 263);
            this.uiPanelDetail.Name = "uiPanelDetail";
            this.uiPanelDetail.Size = new System.Drawing.Size(1010, 414);
            this.uiPanelDetail.TabIndex = 4;
            this.uiPanelDetail.Text = "Ÿ���ü���";
            // 
            // uiPanelDetailContainer
            // 
            this.uiPanelDetailContainer.Controls.Add(this.panel3);
            this.uiPanelDetailContainer.Location = new System.Drawing.Point(1, 23);
            this.uiPanelDetailContainer.Name = "uiPanelDetailContainer";
            this.uiPanelDetailContainer.Size = new System.Drawing.Size(1008, 390);
            this.uiPanelDetailContainer.TabIndex = 0;
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.Transparent;
            this.panel3.Controls.Add(this.uiTab1);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(0, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(1008, 390);
            this.panel3.TabIndex = 13;
            // 
            // uiTab1
            // 
            this.uiTab1.Location = new System.Drawing.Point(8, 8);
            this.uiTab1.Name = "uiTab1";
            this.uiTab1.Size = new System.Drawing.Size(980, 365);
            this.uiTab1.TabIndex = 194;
            this.uiTab1.TabPages.AddRange(new Janus.Windows.UI.Tab.UITabPage[] {
            this.uiTabPage1,
            this.uiTabPage3});
            this.uiTab1.TabsStateStyles.SelectedFormatStyle.FontBold = Janus.Windows.UI.TriState.True;
            this.uiTab1.TabStripAlignment = Janus.Windows.UI.Tab.TabStripAlignment.Left;
            this.uiTab1.VisualStyle = Janus.Windows.UI.Tab.TabVisualStyle.Office2007;
            // 
            // uiTabPage1
            // 
            this.uiTabPage1.Controls.Add(this.uiGroupBox1);
            this.uiTabPage1.Controls.Add(this.chkPoc);
            this.uiTabPage1.Controls.Add(this.chkStbModel);
            this.uiTabPage1.Controls.Add(this.grpStb);
            this.uiTabPage1.Controls.Add(this.lblCollMsg);
            this.uiTabPage1.Controls.Add(this.label60);
            this.uiTabPage1.Controls.Add(this.tlvRegion);
            this.uiTabPage1.Controls.Add(this.uiGroupBox3);
            this.uiTabPage1.Controls.Add(this.label58);
            this.uiTabPage1.Controls.Add(this.grpRateType);
            this.uiTabPage1.Controls.Add(this.udControlRate);
            this.uiTabPage1.Controls.Add(this.ebContractAmt);
            this.uiTabPage1.Controls.Add(this.label3);
            this.uiTabPage1.Controls.Add(this.lbUserID);
            this.uiTabPage1.Controls.Add(this.cbPriorityCd);
            this.uiTabPage1.Controls.Add(this.label2);
            this.uiTabPage1.Controls.Add(this.label6);
            this.uiTabPage1.Controls.Add(this.label7);
            this.uiTabPage1.Controls.Add(this.chkCollectionYn);
            this.uiTabPage1.Controls.Add(this.chkWeekYn);
            this.uiTabPage1.Controls.Add(this.grpWeek);
            this.uiTabPage1.Controls.Add(this.chkRateYn);
            this.uiTabPage1.Controls.Add(this.chkTimeYn);
            this.uiTabPage1.Controls.Add(this.chkRegionYn);
            this.uiTabPage1.Controls.Add(this.tvTime);
            this.uiTabPage1.Controls.Add(this.grpRate);
            this.uiTabPage1.Controls.Add(this.btnSave);
            this.uiTabPage1.Controls.Add(this.tvAge);
            this.uiTabPage1.Controls.Add(this.chkAgeYn);
            this.uiTabPage1.Controls.Add(this.chkSexYn);
            this.uiTabPage1.Controls.Add(this.grpSex);
            this.uiTabPage1.Controls.Add(this.chkAgeBtnYn);
            this.uiTabPage1.Controls.Add(this.udAgeBtnBegin);
            this.uiTabPage1.Controls.Add(this.label4);
            this.uiTabPage1.Controls.Add(this.udAgeBtnEnd);
            this.uiTabPage1.Controls.Add(this.label5);
            this.uiTabPage1.Controls.Add(this.rbControlYn_Y);
            this.uiTabPage1.Controls.Add(this.rbControlYn_N);
            this.uiTabPage1.Icon = ((System.Drawing.Icon)(resources.GetObject("uiTabPage1.Icon")));
            this.uiTabPage1.Location = new System.Drawing.Point(23, 1);
            this.uiTabPage1.Name = "uiTabPage1";
            this.uiTabPage1.PanelFormatStyle.BackColor = System.Drawing.Color.Transparent;
            this.uiTabPage1.Size = new System.Drawing.Size(956, 363);
            this.uiTabPage1.TabStop = true;
            this.uiTabPage1.Text = "Ÿ��������";
            // 
            // uiGroupBox1
            // 
            this.uiGroupBox1.BackColor = System.Drawing.Color.Transparent;
            this.uiGroupBox1.BorderColor = System.Drawing.Color.DimGray;
            this.uiGroupBox1.Controls.Add(this.tvPoc);
            this.uiGroupBox1.Location = new System.Drawing.Point(941, 55);
            this.uiGroupBox1.Name = "uiGroupBox1";
            this.uiGroupBox1.Size = new System.Drawing.Size(37, 179);
            this.uiGroupBox1.TabIndex = 254;
            this.uiGroupBox1.Visible = false;
            this.uiGroupBox1.VisualStyle = Janus.Windows.UI.Dock.PanelVisualStyle.Office2007;
            // 
            // tvPoc
            // 
            this.tvPoc.BackColor = System.Drawing.Color.WhiteSmoke;
            this.tvPoc.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tvPoc.CheckBoxes = true;
            this.tvPoc.FullRowSelect = true;
            this.tvPoc.HideSelection = false;
            this.tvPoc.ItemHeight = 16;
            this.tvPoc.Location = new System.Drawing.Point(6, 15);
            this.tvPoc.Name = "tvPoc";
            treeNode1.Name = "";
            treeNode1.Text = "POC ����";
            this.tvPoc.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode1});
            this.tvPoc.ShowRootLines = false;
            this.tvPoc.Size = new System.Drawing.Size(137, 158);
            this.tvPoc.TabIndex = 203;
            // 
            // chkPoc
            // 
            this.chkPoc.BackColor = System.Drawing.Color.Transparent;
            this.chkPoc.Location = new System.Drawing.Point(946, 38);
            this.chkPoc.Name = "chkPoc";
            this.chkPoc.Size = new System.Drawing.Size(88, 20);
            this.chkPoc.TabIndex = 253;
            this.chkPoc.Text = "POC ����";
            this.chkPoc.Visible = false;
            this.chkPoc.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.chkPoc.CheckedChanged += new System.EventHandler(this.chkPoc_CheckedChanged);
            // 
            // chkStbModel
            // 
            this.chkStbModel.BackColor = System.Drawing.Color.Transparent;
            this.chkStbModel.Location = new System.Drawing.Point(694, 38);
            this.chkStbModel.Name = "chkStbModel";
            this.chkStbModel.Size = new System.Drawing.Size(88, 20);
            this.chkStbModel.TabIndex = 253;
            this.chkStbModel.Text = "��ž��";
            this.ttMsg.SetToolTip(this.chkStbModel, "������ ��ž�𵨿��� ���� ����˴ϴ�");
            this.chkStbModel.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.chkStbModel.CheckedChanged += new System.EventHandler(this.chkStbModel_CheckedChanged);
            // 
            // grpStb
            // 
            this.grpStb.BackColor = System.Drawing.Color.Transparent;
            this.grpStb.BorderColor = System.Drawing.Color.DimGray;
            this.grpStb.Controls.Add(this.tvStb);
            this.grpStb.Location = new System.Drawing.Point(682, 55);
            this.grpStb.Name = "grpStb";
            this.grpStb.Size = new System.Drawing.Size(147, 179);
            this.grpStb.TabIndex = 254;
            this.grpStb.VisualStyle = Janus.Windows.UI.Dock.PanelVisualStyle.Office2007;
            // 
            // tvStb
            // 
            this.tvStb.BackColor = System.Drawing.Color.WhiteSmoke;
            this.tvStb.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tvStb.CheckBoxes = true;
            this.tvStb.Enabled = false;
            this.tvStb.FullRowSelect = true;
            this.tvStb.HideSelection = false;
            this.tvStb.ItemHeight = 16;
            this.tvStb.Location = new System.Drawing.Point(6, 15);
            this.tvStb.Name = "tvStb";
            treeNode2.Name = "";
            treeNode2.Text = "��ž�𵨺�";
            this.tvStb.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode2});
            this.tvStb.ShowRootLines = false;
            this.tvStb.Size = new System.Drawing.Size(134, 158);
            this.tvStb.TabIndex = 203;
            this.tvStb.AfterCheck += new System.Windows.Forms.TreeViewEventHandler(this.tvStb_AfterCheck);
            // 
            // lblCollMsg
            // 
            this.lblCollMsg.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblCollMsg.BackColor = System.Drawing.Color.Red;
            this.lblCollMsg.ForeColor = System.Drawing.Color.White;
            this.lblCollMsg.Location = new System.Drawing.Point(578, 264);
            this.lblCollMsg.Name = "lblCollMsg";
            this.lblCollMsg.Size = new System.Drawing.Size(361, 35);
            this.lblCollMsg.TabIndex = 249;
            this.lblCollMsg.Text = "���� �����ȳ�";
            // 
            // label60
            // 
            this.label60.AutoSize = true;
            this.label60.BackColor = System.Drawing.Color.Transparent;
            this.label60.Location = new System.Drawing.Point(675, 240);
            this.label60.Name = "label60";
            this.label60.Size = new System.Drawing.Size(269, 14);
            this.label60.TabIndex = 247;
            this.label60.Text = "(���ý� ����Ÿ���ü��� �ǿ��� �����Ͽ� �ֽʽÿ�)";
            // 
            // tlvRegion
            // 
            this.tlvRegion.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tlvRegion.CheckBoxes = System.Windows.Forms.CheckBoxesTypes.Recursive;
            this.tlvRegion.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1});
            treeListViewItemCollectionComparer1.Column = 0;
            treeListViewItemCollectionComparer1.SortOrder = System.Windows.Forms.SortOrder.None;
            this.tlvRegion.Comparer = treeListViewItemCollectionComparer1;
            this.tlvRegion.Location = new System.Drawing.Point(24, 61);
            this.tlvRegion.Name = "tlvRegion";
            this.tlvRegion.Size = new System.Drawing.Size(172, 293);
            this.tlvRegion.SmallImageList = this.imgRegion;
            this.tlvRegion.Sorting = System.Windows.Forms.SortOrder.None;
            this.tlvRegion.TabIndex = 238;
            this.tlvRegion.UseCompatibleStateImageBehavior = false;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "��������";
            this.columnHeader1.Width = 200;
            // 
            // imgRegion
            // 
            this.imgRegion.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imgRegion.ImageStream")));
            this.imgRegion.TransparentColor = System.Drawing.Color.Transparent;
            this.imgRegion.Images.SetKeyName(0, "");
            this.imgRegion.Images.SetKeyName(1, "");
            this.imgRegion.Images.SetKeyName(2, "");
            // 
            // uiGroupBox3
            // 
            this.uiGroupBox3.BackColor = System.Drawing.Color.Transparent;
            this.uiGroupBox3.BorderColor = System.Drawing.Color.CornflowerBlue;
            this.uiGroupBox3.Controls.Add(this.lbNotice);
            this.uiGroupBox3.ForeColor = System.Drawing.Color.Black;
            this.uiGroupBox3.Location = new System.Drawing.Point(324, 304);
            this.uiGroupBox3.Name = "uiGroupBox3";
            this.uiGroupBox3.Size = new System.Drawing.Size(582, 52);
            this.uiGroupBox3.TabIndex = 226;
            this.uiGroupBox3.Text = "�޼���";
            // 
            // lbNotice
            // 
            this.lbNotice.BackColor = System.Drawing.Color.Transparent;
            this.lbNotice.ForeColor = System.Drawing.Color.Red;
            this.lbNotice.Location = new System.Drawing.Point(8, 24);
            this.lbNotice.Name = "lbNotice";
            this.lbNotice.Size = new System.Drawing.Size(520, 21);
            this.lbNotice.TabIndex = 219;
            this.lbNotice.Text = "���������� ������� �ʾҽ��ϴ�. ������ �Ϸ����� ������ Ÿ������ ���� �ʽ��ϴ�.";
            this.lbNotice.Visible = false;
            // 
            // label58
            // 
            this.label58.BackColor = System.Drawing.Color.Gray;
            this.label58.Location = new System.Drawing.Point(6, 33);
            this.label58.Name = "label58";
            this.label58.Size = new System.Drawing.Size(936, 1);
            this.label58.TabIndex = 225;
            // 
            // grpRateType
            // 
            this.grpRateType.BackColor = System.Drawing.Color.Transparent;
            this.grpRateType.BorderColor = System.Drawing.Color.Black;
            this.grpRateType.Controls.Add(this.rbRateMinus);
            this.grpRateType.Controls.Add(this.rbRatePlus);
            this.grpRateType.Location = new System.Drawing.Point(444, 261);
            this.grpRateType.Name = "grpRateType";
            this.grpRateType.Size = new System.Drawing.Size(108, 39);
            this.grpRateType.TabIndex = 224;
            this.grpRateType.VisualStyle = Janus.Windows.UI.Dock.PanelVisualStyle.Office2003;
            // 
            // rbRateMinus
            // 
            this.rbRateMinus.Location = new System.Drawing.Point(56, 12);
            this.rbRateMinus.Name = "rbRateMinus";
            this.rbRateMinus.Size = new System.Drawing.Size(48, 21);
            this.rbRateMinus.TabIndex = 31;
            this.rbRateMinus.Text = "�̸�";
            this.rbRateMinus.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            // 
            // rbRatePlus
            // 
            this.rbRatePlus.Checked = true;
            this.rbRatePlus.Location = new System.Drawing.Point(8, 12);
            this.rbRatePlus.Name = "rbRatePlus";
            this.rbRatePlus.Size = new System.Drawing.Size(56, 21);
            this.rbRatePlus.TabIndex = 30;
            this.rbRatePlus.TabStop = true;
            this.rbRatePlus.Text = "�̻�";
            this.rbRatePlus.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            // 
            // udControlRate
            // 
            this.udControlRate.Location = new System.Drawing.Point(596, 9);
            this.udControlRate.Maximum = 900;
            this.udControlRate.MaxLength = 3;
            this.udControlRate.Minimum = 20;
            this.udControlRate.Name = "udControlRate";
            this.udControlRate.Size = new System.Drawing.Size(56, 21);
            this.udControlRate.TabIndex = 197;
            this.udControlRate.TextAlignment = Janus.Windows.GridEX.TextAlignment.Far;
            this.udControlRate.Value = 100;
            this.udControlRate.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            // 
            // ebContractAmt
            // 
            this.ebContractAmt.DecimalDigits = 0;
            this.ebContractAmt.FormatString = "#,##0";
            this.ebContractAmt.Location = new System.Drawing.Point(68, 8);
            this.ebContractAmt.Name = "ebContractAmt";
            this.ebContractAmt.Size = new System.Drawing.Size(104, 21);
            this.ebContractAmt.TabIndex = 194;
            this.ebContractAmt.Text = "0";
            this.ebContractAmt.TextAlignment = Janus.Windows.GridEX.TextAlignment.Far;
            this.ebContractAmt.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.ebContractAmt.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            this.ebContractAmt.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ebContractAmt_KeyDown);
            // 
            // label3
            // 
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Location = new System.Drawing.Point(192, 8);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(52, 21);
            this.label3.TabIndex = 218;
            this.label3.Text = "�����";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.ttMsg.SetToolTip(this.label3, "������� ���� ���� ���, ������ �Ǵ� ����� ��� �Դϴ�.");
            // 
            // lbUserID
            // 
            this.lbUserID.BackColor = System.Drawing.Color.Transparent;
            this.lbUserID.Location = new System.Drawing.Point(4, 7);
            this.lbUserID.Name = "lbUserID";
            this.lbUserID.Size = new System.Drawing.Size(58, 20);
            this.lbUserID.TabIndex = 215;
            this.lbUserID.Text = "��๰��";
            this.lbUserID.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.ttMsg.SetToolTip(this.lbUserID, "���������� ���ذ����� ��������� �ش� ��๰���������� ������ �ǵ��� �����մϴ�");
            // 
            // cbPriorityCd
            // 
            this.cbPriorityCd.AutoSize = false;
            this.cbPriorityCd.BackColor = System.Drawing.Color.White;
            this.cbPriorityCd.ComboStyle = Janus.Windows.EditControls.ComboStyle.DropDownList;
            this.cbPriorityCd.Location = new System.Drawing.Point(248, 8);
            this.cbPriorityCd.Name = "cbPriorityCd";
            this.cbPriorityCd.Size = new System.Drawing.Size(48, 20);
            this.cbPriorityCd.TabIndex = 195;
            this.cbPriorityCd.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Location = new System.Drawing.Point(516, 8);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(76, 21);
            this.label2.TabIndex = 216;
            this.label2.Text = "�����������";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.ttMsg.SetToolTip(this.label2, "���ⷮ�� ������ŭ �ø��ų� ���Դϴ�");
            // 
            // label6
            // 
            this.label6.BackColor = System.Drawing.Color.Transparent;
            this.label6.Location = new System.Drawing.Point(652, 8);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(16, 20);
            this.label6.TabIndex = 217;
            this.label6.Text = "%";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label7
            // 
            this.label7.BackColor = System.Drawing.Color.Transparent;
            this.label7.Location = new System.Drawing.Point(312, 8);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(56, 21);
            this.label7.TabIndex = 213;
            this.label7.Text = "��������";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.ttMsg.SetToolTip(this.label7, "��๰�� �̻����� ������ ���� �ʵ��� ���ⷮ�� �����մϴ�");
            // 
            // chkCollectionYn
            // 
            this.chkCollectionYn.BackColor = System.Drawing.Color.Transparent;
            this.chkCollectionYn.Location = new System.Drawing.Point(562, 239);
            this.chkCollectionYn.Name = "chkCollectionYn";
            this.chkCollectionYn.Size = new System.Drawing.Size(139, 20);
            this.chkCollectionYn.TabIndex = 223;
            this.chkCollectionYn.Text = "����Ÿ���� ���";
            this.ttMsg.SetToolTip(this.chkCollectionYn, "Ÿ�ٱ� Ÿ������ EAP���� ������ �� �ֽ��ϴ�");
            this.chkCollectionYn.CheckedChanged += new System.EventHandler(this.chkCollectionYn_CheckedChanged);
            // 
            // chkWeekYn
            // 
            this.chkWeekYn.BackColor = System.Drawing.Color.Transparent;
            this.chkWeekYn.Location = new System.Drawing.Point(578, 38);
            this.chkWeekYn.Name = "chkWeekYn";
            this.chkWeekYn.Size = new System.Drawing.Size(68, 20);
            this.chkWeekYn.TabIndex = 220;
            this.chkWeekYn.Text = "���Ϻ�";
            this.chkWeekYn.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.chkWeekYn.CheckedChanged += new System.EventHandler(this.chkWeekYn_CheckedChanged);
            // 
            // grpWeek
            // 
            this.grpWeek.BackColor = System.Drawing.Color.Transparent;
            this.grpWeek.BorderColor = System.Drawing.Color.Black;
            this.grpWeek.Controls.Add(this.chkSun);
            this.grpWeek.Controls.Add(this.chkSat);
            this.grpWeek.Controls.Add(this.chkFri);
            this.grpWeek.Controls.Add(this.chkThe);
            this.grpWeek.Controls.Add(this.chkWed);
            this.grpWeek.Controls.Add(this.chkMon);
            this.grpWeek.Controls.Add(this.chkThu);
            this.grpWeek.Location = new System.Drawing.Point(565, 55);
            this.grpWeek.Name = "grpWeek";
            this.grpWeek.Size = new System.Drawing.Size(103, 179);
            this.grpWeek.TabIndex = 221;
            this.grpWeek.VisualStyle = Janus.Windows.UI.Dock.PanelVisualStyle.Office2007;
            // 
            // chkSun
            // 
            this.chkSun.ForeColor = System.Drawing.Color.Red;
            this.chkSun.Location = new System.Drawing.Point(12, 147);
            this.chkSun.Name = "chkSun";
            this.chkSun.Size = new System.Drawing.Size(32, 21);
            this.chkSun.TabIndex = 30;
            this.chkSun.Text = "��";
            this.chkSun.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            // 
            // chkSat
            // 
            this.chkSat.ForeColor = System.Drawing.Color.Blue;
            this.chkSat.Location = new System.Drawing.Point(12, 126);
            this.chkSat.Name = "chkSat";
            this.chkSat.Size = new System.Drawing.Size(32, 21);
            this.chkSat.TabIndex = 29;
            this.chkSat.Text = "��";
            this.chkSat.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            // 
            // chkFri
            // 
            this.chkFri.Location = new System.Drawing.Point(12, 104);
            this.chkFri.Name = "chkFri";
            this.chkFri.Size = new System.Drawing.Size(32, 21);
            this.chkFri.TabIndex = 28;
            this.chkFri.Text = "��";
            this.chkFri.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            // 
            // chkThe
            // 
            this.chkThe.Location = new System.Drawing.Point(12, 82);
            this.chkThe.Name = "chkThe";
            this.chkThe.Size = new System.Drawing.Size(32, 21);
            this.chkThe.TabIndex = 27;
            this.chkThe.Text = "��";
            this.chkThe.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            // 
            // chkWed
            // 
            this.chkWed.Location = new System.Drawing.Point(12, 60);
            this.chkWed.Name = "chkWed";
            this.chkWed.Size = new System.Drawing.Size(32, 21);
            this.chkWed.TabIndex = 26;
            this.chkWed.Text = "��";
            this.chkWed.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            // 
            // chkMon
            // 
            this.chkMon.Location = new System.Drawing.Point(12, 16);
            this.chkMon.Name = "chkMon";
            this.chkMon.Size = new System.Drawing.Size(32, 21);
            this.chkMon.TabIndex = 24;
            this.chkMon.Text = "��";
            this.chkMon.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            // 
            // chkThu
            // 
            this.chkThu.Location = new System.Drawing.Point(12, 39);
            this.chkThu.Name = "chkThu";
            this.chkThu.Size = new System.Drawing.Size(32, 21);
            this.chkThu.TabIndex = 25;
            this.chkThu.Text = "ȭ";
            this.chkThu.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            // 
            // chkRateYn
            // 
            this.chkRateYn.BackColor = System.Drawing.Color.Transparent;
            this.chkRateYn.Location = new System.Drawing.Point(452, 133);
            this.chkRateYn.Name = "chkRateYn";
            this.chkRateYn.Size = new System.Drawing.Size(96, 21);
            this.chkRateYn.TabIndex = 209;
            this.chkRateYn.Text = "������";
            this.chkRateYn.CheckedChanged += new System.EventHandler(this.chkRateYn_CheckedChanged);
            // 
            // chkTimeYn
            // 
            this.chkTimeYn.BackColor = System.Drawing.Color.Transparent;
            this.chkTimeYn.Location = new System.Drawing.Point(219, 38);
            this.chkTimeYn.Name = "chkTimeYn";
            this.chkTimeYn.Size = new System.Drawing.Size(88, 20);
            this.chkTimeYn.TabIndex = 200;
            this.chkTimeYn.Text = "����ð���";
            this.chkTimeYn.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.chkTimeYn.CheckedChanged += new System.EventHandler(this.chkTimeYn_CheckedChanged);
            // 
            // chkRegionYn
            // 
            this.chkRegionYn.BackColor = System.Drawing.Color.Transparent;
            this.chkRegionYn.Location = new System.Drawing.Point(27, 38);
            this.chkRegionYn.Name = "chkRegionYn";
            this.chkRegionYn.Size = new System.Drawing.Size(80, 20);
            this.chkRegionYn.TabIndex = 198;
            this.chkRegionYn.Text = "��������";
            this.chkRegionYn.CheckedChanged += new System.EventHandler(this.chkRegionYn_CheckedChanged);
            // 
            // tvTime
            // 
            this.tvTime.BackColor = System.Drawing.Color.White;
            this.tvTime.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tvTime.CheckBoxes = true;
            this.tvTime.FullRowSelect = true;
            this.tvTime.HideSelection = false;
            this.tvTime.Indent = 15;
            this.tvTime.ItemHeight = 14;
            this.tvTime.Location = new System.Drawing.Point(203, 61);
            this.tvTime.Name = "tvTime";
            treeNode3.Name = "";
            treeNode3.Text = "00��~01��";
            treeNode4.Name = "";
            treeNode4.Text = "01��~02��";
            treeNode5.Name = "";
            treeNode5.Text = "02��~03��";
            treeNode6.Name = "";
            treeNode6.Text = "03��~04��";
            treeNode7.Name = "";
            treeNode7.Text = "��ü�ð���";
            this.tvTime.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode7});
            this.tvTime.Size = new System.Drawing.Size(112, 295);
            this.tvTime.TabIndex = 201;
            this.tvTime.AfterCheck += new System.Windows.Forms.TreeViewEventHandler(this.tvTime_AfterCheck);
            // 
            // grpRate
            // 
            this.grpRate.BackColor = System.Drawing.Color.Transparent;
            this.grpRate.BorderColor = System.Drawing.Color.Black;
            this.grpRate.Controls.Add(this.rbRate19);
            this.grpRate.Controls.Add(this.rbRate15);
            this.grpRate.Controls.Add(this.rbRate12);
            this.grpRate.Controls.Add(this.rbRateAll);
            this.grpRate.Location = new System.Drawing.Point(444, 149);
            this.grpRate.Name = "grpRate";
            this.grpRate.Size = new System.Drawing.Size(108, 112);
            this.grpRate.TabIndex = 210;
            this.grpRate.VisualStyle = Janus.Windows.UI.Dock.PanelVisualStyle.Office2007;
            // 
            // rbRate19
            // 
            this.rbRate19.Location = new System.Drawing.Point(8, 88);
            this.rbRate19.Name = "rbRate19";
            this.rbRate19.Size = new System.Drawing.Size(80, 21);
            this.rbRate19.TabIndex = 33;
            this.rbRate19.Text = "19��";
            this.rbRate19.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            // 
            // rbRate15
            // 
            this.rbRate15.Location = new System.Drawing.Point(8, 64);
            this.rbRate15.Name = "rbRate15";
            this.rbRate15.Size = new System.Drawing.Size(80, 21);
            this.rbRate15.TabIndex = 32;
            this.rbRate15.Text = "15��";
            this.rbRate15.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            // 
            // rbRate12
            // 
            this.rbRate12.Location = new System.Drawing.Point(8, 40);
            this.rbRate12.Name = "rbRate12";
            this.rbRate12.Size = new System.Drawing.Size(80, 21);
            this.rbRate12.TabIndex = 31;
            this.rbRate12.Text = "12��";
            this.rbRate12.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            // 
            // rbRateAll
            // 
            this.rbRateAll.Checked = true;
            this.rbRateAll.Location = new System.Drawing.Point(8, 16);
            this.rbRateAll.Name = "rbRateAll";
            this.rbRateAll.Size = new System.Drawing.Size(88, 21);
            this.rbRateAll.TabIndex = 30;
            this.rbRateAll.TabStop = true;
            this.rbRateAll.Text = "��ü";
            this.rbRateAll.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnSave.BackColor = System.Drawing.SystemColors.Window;
            this.btnSave.Enabled = false;
            this.btnSave.Location = new System.Drawing.Point(862, 6);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(80, 24);
            this.btnSave.TabIndex = 211;
            this.btnSave.Text = "�� ��";
            this.btnSave.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // tvAge
            // 
            this.tvAge.BackColor = System.Drawing.Color.White;
            this.tvAge.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tvAge.CheckBoxes = true;
            this.tvAge.FullRowSelect = true;
            this.tvAge.HideSelection = false;
            this.tvAge.ItemHeight = 16;
            this.tvAge.Location = new System.Drawing.Point(320, 61);
            this.tvAge.Name = "tvAge";
            treeNode8.Name = "";
            treeNode8.Text = "0~19��";
            treeNode9.Name = "";
            treeNode9.Text = "20~29��";
            treeNode10.Name = "";
            treeNode10.Text = "30~39��";
            treeNode11.Name = "";
            treeNode11.Text = "40~49��";
            treeNode12.Name = "";
            treeNode12.Text = "50~59��";
            treeNode13.Name = "";
            treeNode13.Text = "60���̻�";
            treeNode14.Name = "";
            treeNode14.Text = "��ü���ɴ�";
            this.tvAge.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode14});
            this.tvAge.Size = new System.Drawing.Size(114, 152);
            this.tvAge.TabIndex = 203;
            this.tvAge.AfterCheck += new System.Windows.Forms.TreeViewEventHandler(this.tvAge_AfterCheck);
            // 
            // chkAgeYn
            // 
            this.chkAgeYn.BackColor = System.Drawing.Color.Transparent;
            this.chkAgeYn.Location = new System.Drawing.Point(344, 38);
            this.chkAgeYn.Name = "chkAgeYn";
            this.chkAgeYn.Size = new System.Drawing.Size(88, 20);
            this.chkAgeYn.TabIndex = 202;
            this.chkAgeYn.Text = "���⿬�ɴ�";
            this.chkAgeYn.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.chkAgeYn.CheckedChanged += new System.EventHandler(this.chkAgeYn_CheckedChanged);
            // 
            // chkSexYn
            // 
            this.chkSexYn.BackColor = System.Drawing.Color.Transparent;
            this.chkSexYn.Location = new System.Drawing.Point(452, 38);
            this.chkSexYn.Name = "chkSexYn";
            this.chkSexYn.Size = new System.Drawing.Size(92, 20);
            this.chkSexYn.TabIndex = 204;
            this.chkSexYn.Text = "���⼺��";
            this.chkSexYn.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.chkSexYn.CheckedChanged += new System.EventHandler(this.chkSexYn_CheckedChanged);
            // 
            // grpSex
            // 
            this.grpSex.BackColor = System.Drawing.Color.Transparent;
            this.grpSex.BorderColor = System.Drawing.Color.Black;
            this.grpSex.Controls.Add(this.chkSexMan);
            this.grpSex.Controls.Add(this.chkSexWoman);
            this.grpSex.Location = new System.Drawing.Point(444, 55);
            this.grpSex.Name = "grpSex";
            this.grpSex.Size = new System.Drawing.Size(108, 72);
            this.grpSex.TabIndex = 205;
            this.grpSex.VisualStyle = Janus.Windows.UI.Dock.PanelVisualStyle.Office2007;
            // 
            // chkSexMan
            // 
            this.chkSexMan.Location = new System.Drawing.Point(8, 16);
            this.chkSexMan.Name = "chkSexMan";
            this.chkSexMan.Size = new System.Drawing.Size(64, 21);
            this.chkSexMan.TabIndex = 24;
            this.chkSexMan.Text = "����";
            this.chkSexMan.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            // 
            // chkSexWoman
            // 
            this.chkSexWoman.Location = new System.Drawing.Point(8, 40);
            this.chkSexWoman.Name = "chkSexWoman";
            this.chkSexWoman.Size = new System.Drawing.Size(64, 21);
            this.chkSexWoman.TabIndex = 25;
            this.chkSexWoman.Text = "����";
            this.chkSexWoman.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            // 
            // chkAgeBtnYn
            // 
            this.chkAgeBtnYn.BackColor = System.Drawing.Color.Transparent;
            this.chkAgeBtnYn.Location = new System.Drawing.Point(320, 221);
            this.chkAgeBtnYn.Name = "chkAgeBtnYn";
            this.chkAgeBtnYn.Size = new System.Drawing.Size(104, 21);
            this.chkAgeBtnYn.TabIndex = 206;
            this.chkAgeBtnYn.Text = "���⿬�ɱ���";
            this.chkAgeBtnYn.CheckedChanged += new System.EventHandler(this.chkAgeBtnYn_CheckedChanged);
            // 
            // udAgeBtnBegin
            // 
            this.udAgeBtnBegin.Location = new System.Drawing.Point(320, 245);
            this.udAgeBtnBegin.Maximum = 150;
            this.udAgeBtnBegin.MaxLength = 3;
            this.udAgeBtnBegin.Name = "udAgeBtnBegin";
            this.udAgeBtnBegin.Size = new System.Drawing.Size(64, 21);
            this.udAgeBtnBegin.TabIndex = 207;
            this.udAgeBtnBegin.TextAlignment = Janus.Windows.GridEX.TextAlignment.Far;
            this.udAgeBtnBegin.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2003;
            // 
            // label4
            // 
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.Location = new System.Drawing.Point(384, 245);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(48, 21);
            this.label4.TabIndex = 212;
            this.label4.Text = "������";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // udAgeBtnEnd
            // 
            this.udAgeBtnEnd.Location = new System.Drawing.Point(320, 269);
            this.udAgeBtnEnd.Maximum = 150;
            this.udAgeBtnEnd.MaxLength = 3;
            this.udAgeBtnEnd.Name = "udAgeBtnEnd";
            this.udAgeBtnEnd.Size = new System.Drawing.Size(64, 21);
            this.udAgeBtnEnd.TabIndex = 208;
            this.udAgeBtnEnd.TextAlignment = Janus.Windows.GridEX.TextAlignment.Far;
            this.udAgeBtnEnd.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2003;
            // 
            // label5
            // 
            this.label5.BackColor = System.Drawing.Color.Transparent;
            this.label5.Location = new System.Drawing.Point(384, 269);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(44, 21);
            this.label5.TabIndex = 214;
            this.label5.Text = "������";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // rbControlYn_Y
            // 
            this.rbControlYn_Y.BackColor = System.Drawing.Color.Transparent;
            this.rbControlYn_Y.Checked = true;
            this.rbControlYn_Y.Location = new System.Drawing.Point(376, 8);
            this.rbControlYn_Y.Name = "rbControlYn_Y";
            this.rbControlYn_Y.Size = new System.Drawing.Size(56, 20);
            this.rbControlYn_Y.TabIndex = 15;
            this.rbControlYn_Y.TabStop = true;
            this.rbControlYn_Y.Text = "����";
            this.rbControlYn_Y.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            // 
            // rbControlYn_N
            // 
            this.rbControlYn_N.BackColor = System.Drawing.Color.Transparent;
            this.rbControlYn_N.Location = new System.Drawing.Point(436, 8);
            this.rbControlYn_N.Name = "rbControlYn_N";
            this.rbControlYn_N.Size = new System.Drawing.Size(64, 20);
            this.rbControlYn_N.TabIndex = 15;
            this.rbControlYn_N.Text = "�������";
            this.rbControlYn_N.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            // 
            // uiTabPage3
            // 
            this.uiTabPage3.Controls.Add(this.label63);
            this.uiTabPage3.Controls.Add(this.label62);
            this.uiTabPage3.Controls.Add(this.panel4);
            this.uiTabPage3.Controls.Add(this.panel1);
            this.uiTabPage3.Location = new System.Drawing.Point(23, 1);
            this.uiTabPage3.Name = "uiTabPage3";
            this.uiTabPage3.Size = new System.Drawing.Size(954, 361);
            this.uiTabPage3.TabStop = true;
            this.uiTabPage3.Text = "����Ÿ���ü���";
            // 
            // label63
            // 
            this.label63.BackColor = System.Drawing.Color.Transparent;
            this.label63.Location = new System.Drawing.Point(393, 18);
            this.label63.Name = "label63";
            this.label63.Size = new System.Drawing.Size(103, 21);
            this.label63.TabIndex = 241;
            this.label63.Text = "������";
            this.label63.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label62
            // 
            this.label62.BackColor = System.Drawing.Color.Transparent;
            this.label62.Location = new System.Drawing.Point(13, 17);
            this.label62.Name = "label62";
            this.label62.Size = new System.Drawing.Size(103, 21);
            this.label62.TabIndex = 242;
            this.label62.Text = "������ ����";
            this.label62.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.grdExSouceCollectionList);
            this.panel4.Controls.Add(this.panel6);
            this.panel4.Location = new System.Drawing.Point(393, 42);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(548, 300);
            this.panel4.TabIndex = 240;
            // 
            // grdExSouceCollectionList
            // 
            this.grdExSouceCollectionList.AlternatingColors = true;
            this.grdExSouceCollectionList.BorderStyle = Janus.Windows.GridEX.BorderStyle.Flat;
            this.grdExSouceCollectionList.DataSource = this.dvCollection;
            grdExSouceCollectionList_DesignTimeLayout.LayoutString = resources.GetString("grdExSouceCollectionList_DesignTimeLayout.LayoutString");
            this.grdExSouceCollectionList.DesignTimeLayout = grdExSouceCollectionList_DesignTimeLayout;
            this.grdExSouceCollectionList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdExSouceCollectionList.EditorsControlStyle.ButtonAppearance = Janus.Windows.GridEX.ButtonAppearance.Regular;
            this.grdExSouceCollectionList.EmptyRows = true;
            this.grdExSouceCollectionList.FocusCellFormatStyle.BackColor = System.Drawing.SystemColors.Highlight;
            this.grdExSouceCollectionList.FocusStyle = Janus.Windows.GridEX.FocusStyle.None;
            this.grdExSouceCollectionList.FrozenColumns = 2;
            this.grdExSouceCollectionList.GridLineColor = System.Drawing.Color.Silver;
            this.grdExSouceCollectionList.GridLines = Janus.Windows.GridEX.GridLines.Vertical;
            this.grdExSouceCollectionList.GridLineStyle = Janus.Windows.GridEX.GridLineStyle.Solid;
            this.grdExSouceCollectionList.GroupByBoxVisible = false;
            this.grdExSouceCollectionList.Location = new System.Drawing.Point(0, 0);
            this.grdExSouceCollectionList.Name = "grdExSouceCollectionList";
            this.grdExSouceCollectionList.ScrollBars = Janus.Windows.GridEX.ScrollBars.Vertical;
            this.grdExSouceCollectionList.SelectedInactiveFormatStyle.BackColor = System.Drawing.Color.DarkOrange;
            this.grdExSouceCollectionList.Size = new System.Drawing.Size(548, 260);
            this.grdExSouceCollectionList.TabIndex = 20;
            this.grdExSouceCollectionList.TabKeyBehavior = Janus.Windows.GridEX.TabKeyBehavior.ControlNavigation;
            this.grdExSouceCollectionList.ThemedAreas = ((Janus.Windows.GridEX.ThemedArea)(((((((((Janus.Windows.GridEX.ThemedArea.ScrollBars | Janus.Windows.GridEX.ThemedArea.EditControls)
                        | Janus.Windows.GridEX.ThemedArea.Headers)
                        | Janus.Windows.GridEX.ThemedArea.GroupByBox)
                        | Janus.Windows.GridEX.ThemedArea.GroupRows)
                        | Janus.Windows.GridEX.ThemedArea.ControlBorder)
                        | Janus.Windows.GridEX.ThemedArea.Cards)
                        | Janus.Windows.GridEX.ThemedArea.Gridlines)
                        | Janus.Windows.GridEX.ThemedArea.CheckBoxes)));
            this.grdExSouceCollectionList.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            // 
            // dvCollection
            // 
            this.dvCollection.Table = this.targetingDs.Collections;
            // 
            // panel6
            // 
            this.panel6.BackColor = System.Drawing.SystemColors.Window;
            this.panel6.Controls.Add(this.btnAddCollectionMinus);
            this.panel6.Controls.Add(this.lbMsg2);
            this.panel6.Controls.Add(this.btnAddCollection);
            this.panel6.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel6.Location = new System.Drawing.Point(0, 260);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(548, 40);
            this.panel6.TabIndex = 19;
            // 
            // btnAddCollectionMinus
            // 
            this.btnAddCollectionMinus.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnAddCollectionMinus.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnAddCollectionMinus.Enabled = false;
            this.btnAddCollectionMinus.Location = new System.Drawing.Point(106, 8);
            this.btnAddCollectionMinus.Name = "btnAddCollectionMinus";
            this.btnAddCollectionMinus.Size = new System.Drawing.Size(97, 24);
            this.btnAddCollectionMinus.TabIndex = 38;
            this.btnAddCollectionMinus.Text = "�������� �߰�";
            this.btnAddCollectionMinus.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.btnAddCollectionMinus.Click += new System.EventHandler(this.btnAddCollectionMinus_Click);
            // 
            // lbMsg2
            // 
            this.lbMsg2.Location = new System.Drawing.Point(298, 11);
            this.lbMsg2.Name = "lbMsg2";
            this.lbMsg2.Size = new System.Drawing.Size(247, 21);
            this.lbMsg2.TabIndex = 37;
            this.lbMsg2.Text = "���õ� �������� Ÿ���ü����� �߰��մϴ�.";
            this.lbMsg2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnAddCollection
            // 
            this.btnAddCollection.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnAddCollection.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnAddCollection.Enabled = false;
            this.btnAddCollection.Location = new System.Drawing.Point(4, 8);
            this.btnAddCollection.Name = "btnAddCollection";
            this.btnAddCollection.Size = new System.Drawing.Size(97, 24);
            this.btnAddCollection.TabIndex = 17;
            this.btnAddCollection.Text = "�������� �߰�";
            this.btnAddCollection.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.btnAddCollection.Click += new System.EventHandler(this.btnAddCollection_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.grdExTargetList);
            this.panel1.Controls.Add(this.panel5);
            this.panel1.Location = new System.Drawing.Point(12, 41);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(375, 302);
            this.panel1.TabIndex = 239;
            // 
            // grdExTargetList
            // 
            this.grdExTargetList.AlternatingColors = true;
            this.grdExTargetList.BorderStyle = Janus.Windows.GridEX.BorderStyle.Flat;
            this.grdExTargetList.DataSource = this.dvTargetingCollection;
            grdExTargetList_DesignTimeLayout.LayoutString = resources.GetString("grdExTargetList_DesignTimeLayout.LayoutString");
            this.grdExTargetList.DesignTimeLayout = grdExTargetList_DesignTimeLayout;
            this.grdExTargetList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdExTargetList.EmptyRows = true;
            this.grdExTargetList.FocusCellFormatStyle.BackColor = System.Drawing.SystemColors.Highlight;
            this.grdExTargetList.FocusStyle = Janus.Windows.GridEX.FocusStyle.None;
            this.grdExTargetList.FrozenColumns = 2;
            this.grdExTargetList.GridLineColor = System.Drawing.Color.Silver;
            this.grdExTargetList.GridLines = Janus.Windows.GridEX.GridLines.Vertical;
            this.grdExTargetList.GridLineStyle = Janus.Windows.GridEX.GridLineStyle.Solid;
            this.grdExTargetList.GroupByBoxVisible = false;
            this.grdExTargetList.Location = new System.Drawing.Point(0, 0);
            this.grdExTargetList.Name = "grdExTargetList";
            this.grdExTargetList.ScrollBars = Janus.Windows.GridEX.ScrollBars.Vertical;
            this.grdExTargetList.SelectedInactiveFormatStyle.BackColor = System.Drawing.Color.DarkOrange;
            this.grdExTargetList.Size = new System.Drawing.Size(375, 262);
            this.grdExTargetList.TabIndex = 18;
            this.grdExTargetList.TabKeyBehavior = Janus.Windows.GridEX.TabKeyBehavior.ControlNavigation;
            this.grdExTargetList.ThemedAreas = ((Janus.Windows.GridEX.ThemedArea)(((((((((Janus.Windows.GridEX.ThemedArea.ScrollBars | Janus.Windows.GridEX.ThemedArea.EditControls)
                        | Janus.Windows.GridEX.ThemedArea.Headers)
                        | Janus.Windows.GridEX.ThemedArea.GroupByBox)
                        | Janus.Windows.GridEX.ThemedArea.GroupRows)
                        | Janus.Windows.GridEX.ThemedArea.ControlBorder)
                        | Janus.Windows.GridEX.ThemedArea.Cards)
                        | Janus.Windows.GridEX.ThemedArea.Gridlines)
                        | Janus.Windows.GridEX.ThemedArea.CheckBoxes)));
            this.grdExTargetList.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            // 
            // dvTargetingCollection
            // 
            this.dvTargetingCollection.Table = this.targetingDs.TargetingCollection;
            // 
            // panel5
            // 
            this.panel5.BackColor = System.Drawing.SystemColors.Window;
            this.panel5.Controls.Add(this.label59);
            this.panel5.Controls.Add(this.btnDeleteCollection);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel5.Location = new System.Drawing.Point(0, 262);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(375, 40);
            this.panel5.TabIndex = 17;
            // 
            // label59
            // 
            this.label59.Location = new System.Drawing.Point(110, 9);
            this.label59.Name = "label59";
            this.label59.Size = new System.Drawing.Size(262, 21);
            this.label59.TabIndex = 37;
            this.label59.Text = "���õ� ������ Ÿ���ü������� �����մϴ�.";
            this.label59.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnDeleteCollection
            // 
            this.btnDeleteCollection.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnDeleteCollection.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnDeleteCollection.Enabled = false;
            this.btnDeleteCollection.Location = new System.Drawing.Point(0, 7);
            this.btnDeleteCollection.Name = "btnDeleteCollection";
            this.btnDeleteCollection.Size = new System.Drawing.Size(104, 24);
            this.btnDeleteCollection.TabIndex = 17;
            this.btnDeleteCollection.Text = "�� ��";
            this.btnDeleteCollection.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.btnDeleteCollection.Click += new System.EventHandler(this.btnDeleteCollection_Click);
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
            this.editBox1.Size = new System.Drawing.Size(0, 21);
            this.editBox1.TabIndex = 0;
            this.editBox1.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
            // 
            // ttMsg
            // 
            this.ttMsg.AutoPopDelay = 5000;
            this.ttMsg.InitialDelay = 100;
            this.ttMsg.ReshowDelay = 100;
            this.ttMsg.ShowAlways = true;
            // 
            // TargetingOapControl
            // 
            this.Controls.Add(this.uiPanelChoiceAdSchedule);
            this.Font = global::AdManagerClient.Properties.Settings.Default.Font1;
            this.Name = "TargetingOapControl";
            this.Size = new System.Drawing.Size(1010, 677);
            this.Load += new System.EventHandler(this.UserControl_Load);
            ((System.ComponentModel.ISupportInitialize)(this.targetingDs)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiPM)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelChoiceAdSchedule)).EndInit();
            this.uiPanelChoiceAdSchedule.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelSearch)).EndInit();
            this.uiPanelSearch.ResumeLayout(false);
            this.uiPanelSearchContainer.ResumeLayout(false);
            this.pnlSearch.ResumeLayout(false);
            this.pnlSearch.PerformLayout();
            this.pnlSearchBtn.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelList)).EndInit();
            this.uiPanelList.ResumeLayout(false);
            this.uiPanelListContainer.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdExScheduleList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dvTargeting)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelDetail)).EndInit();
            this.uiPanelDetail.ResumeLayout(false);
            this.uiPanelDetailContainer.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.uiTab1)).EndInit();
            this.uiTab1.ResumeLayout(false);
            this.uiTabPage1.ResumeLayout(false);
            this.uiTabPage1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uiGroupBox1)).EndInit();
            this.uiGroupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grpStb)).EndInit();
            this.grpStb.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.uiGroupBox3)).EndInit();
            this.uiGroupBox3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grpRateType)).EndInit();
            this.grpRateType.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grpWeek)).EndInit();
            this.grpWeek.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grpRate)).EndInit();
            this.grpRate.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grpSex)).EndInit();
            this.grpSex.ResumeLayout(false);
            this.uiTabPage3.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdExSouceCollectionList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dvCollection)).EndInit();
            this.panel6.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdExTargetList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dvTargetingCollection)).EndInit();
            this.panel5.ResumeLayout(false);
            this.panMenuSchedule.ResumeLayout(false);
            this.ResumeLayout(false);

		}
        #endregion

        #region ��Ʈ�� �ε�
        private void UserControl_Load(object sender, System.EventArgs e)
        {
            // �����Ͱ����� ��ü����
            dt = ((DataView)grdExScheduleList.DataSource).Table;  
            cm = (CurrencyManager) this.BindingContext[grdExScheduleList.DataSource]; 
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

            // ��ȸ���� �˻�
            if(menu.CanRead(MenuCode))
            {
                canRead = true;
            }
			
            // �����ư Ȱ��ȭ
            if(menu.CanUpdate(MenuCode))
            {
                canUpdate = true;
            }

            InitButton();

			ProgressStop();


			if(canRead) SearchTargeting();
			
		}

        private void InitCombo()
        {
            Init_MediaCode();
            Init_RapCode();
            Init_AgencyCode();
			Init_Collection();
            InitCombo_Level();

            //����켱��������ڵ� �ʱ�ȭ
            Janus.Windows.EditControls.UIComboBoxItem[] comboItems = new Janus.Windows.EditControls.UIComboBoxItem[10];
            for(int i=0;i < 10;i++)
            {
                comboItems[i] = new Janus.Windows.EditControls.UIComboBoxItem(i+1,i);
            }
            // �޺��� ��Ʈ
            this.cbPriorityCd.Items.AddRange(comboItems);
            this.cbPriorityCd.SelectedIndex = 4;

	
			Init_RegionData();	// �������� ������ �ʱ�ȭ
			Init_AgeData();		// ���ɴ� ������ �ʱ�ȭ

			Init_tlvRegion();	// �������� TreeListView �ʱ�ȭ[E_02]
			Init_tvTime();		// �ð��� TreeView �ʱ�ȭ
			Init_tvAge();		// ���ɴ� TreeView �ʱ�ȭ

            Init_StbData();     // [E_04] ��ž�𵨵����� �ʱ�ȭ
            Init_tvStb();       // [E_04] ��ž������ TreeView �ʱ�ȭ
		}


        private void Init_MediaCode()
        {
            // ��ü�� ��ȸ�Ѵ�.
            MediaCodeModel mediacodeModel = new MediaCodeModel();
            new MediaCodeManager(systemModel, commonModel).GetMediaCodeList(mediacodeModel);
			
            if (mediacodeModel.ResultCD.Equals("0000"))
            {
                // �����ͼ¿� ����
                Utility.SetDataTable(targetingDs.Medias, mediacodeModel.MediaCodeDataSet);				
            }

            // �˻������� �޺�
            this.cbSearchMedia.Items.Clear();
			
            // �޺��ڽ��� ��Ʈ�� �ڵ����� ���� Item�迭�� ����
            Janus.Windows.EditControls.UIComboBoxItem[] comboItems = new Janus.Windows.EditControls.UIComboBoxItem[mediacodeModel.ResultCnt + 1];

            comboItems[0] = new Janus.Windows.EditControls.UIComboBoxItem("��ü����","00");
			
            for(int i=0;i<mediacodeModel.ResultCnt;i++)
            {
                DataRow row = targetingDs.Medias.Rows[i];

                string val = row["MediaCode"].ToString();
                string txt = row["MediaName"].ToString();
                comboItems[i+1] = new Janus.Windows.EditControls.UIComboBoxItem(txt,val);
            }
            // �޺��� ��Ʈ
            this.cbSearchMedia.Items.AddRange(comboItems);
            this.cbSearchMedia.SelectedIndex = 0;

            Application.DoEvents();
        }

        private void Init_RapCode()
        {
            // ���� ��ȸ�Ѵ�.
            MediaRapCodeModel mediarapcodeModel = new MediaRapCodeModel();
            new MediaRapCodeManager(systemModel, commonModel).GetMediaRapCodeList(mediarapcodeModel);
			
            if (mediarapcodeModel.ResultCD.Equals("0000"))
            {
                // �����ͼ¿� ����
                Utility.SetDataTable(targetingDs.MediaRaps, mediarapcodeModel.MediaRapCodeDataSet);				
            }

            // �˻������� �޺�
            this.cbSearchRap.Items.Clear();
			
            // �޺��ڽ��� ��Ʈ�� �ڵ����� ���� Item�迭�� ����
            Janus.Windows.EditControls.UIComboBoxItem[] comboItems = new Janus.Windows.EditControls.UIComboBoxItem[mediarapcodeModel.ResultCnt + 1];

            comboItems[0] = new Janus.Windows.EditControls.UIComboBoxItem("�̵�����","00");
			
            for(int i=0;i<mediarapcodeModel.ResultCnt;i++)
            {
                DataRow row = targetingDs.MediaRaps.Rows[i];

                string val = row["RapCode"].ToString();
                string txt = row["RapName"].ToString();
                comboItems[i+1] = new Janus.Windows.EditControls.UIComboBoxItem(txt,val);
            }
            // �޺��� ��Ʈ
            this.cbSearchRap.Items.AddRange(comboItems);
            this.cbSearchRap.SelectedIndex = 0;

            Application.DoEvents();
        }

        private void Init_AgencyCode()
        {
            // ����縦 ��ȸ�Ѵ�.
            AgencyCodeModel agencycodeModel = new AgencyCodeModel();
            new AgencyCodeManager(systemModel, commonModel).GetAgencyCodeList(agencycodeModel);
			
            if (agencycodeModel.ResultCD.Equals("0000"))
            {
                // �����ͼ¿� ����
                Utility.SetDataTable(targetingDs.Agencys, agencycodeModel.AgencyCodeDataSet);				
            }

            // �˻������� �޺�
            this.cbSearchAgency.Items.Clear();
			
            // �޺��ڽ��� ��Ʈ�� �ڵ����� ���� Item�迭�� ����
            Janus.Windows.EditControls.UIComboBoxItem[] comboItems = new Janus.Windows.EditControls.UIComboBoxItem[agencycodeModel.ResultCnt + 1];

            comboItems[0] = new Janus.Windows.EditControls.UIComboBoxItem("����缱��","00");
			
            for(int i=0;i<agencycodeModel.ResultCnt;i++)
            {
                DataRow row = targetingDs.Agencys.Rows[i];

                string val = row["AgencyCode"].ToString();
                string txt = row["AgencyName"].ToString();
                comboItems[i+1] = new Janus.Windows.EditControls.UIComboBoxItem(txt,val);
            }
			
            // �޺��� ��Ʈ
            this.cbSearchAgency.Items.AddRange(comboItems);
            this.cbSearchAgency.SelectedIndex = 0;

            Application.DoEvents();
        }

		private void Init_Collection()
		{
			// �����ָ� ��ȸ�Ѵ�.
			TargetingModel targetingModel = new TargetingModel();
			new TargetingManager(systemModel,commonModel).GetCollectionsList(targetingModel);

			if (targetingModel.ResultCD.Equals("0000"))
			{
				Utility.SetDataTable(targetingDs.Collections, targetingModel.CollectionsDataSet);				
				StatusMessage(targetingModel.ResultCnt + "���� Ÿ�ٱ� ������ ��ȸ�Ǿ����ϴ�.");
			}			

            /* [E_03] �޺� ������ 
             * 2012.02.20 RH.Jung 
             * 
			// �˻������� �޺�
			this.cbCollection.Items.Clear();
			
			// �޺��ڽ��� ��Ʈ�� �ڵ����� ���� Item�迭�� ����
			Janus.Windows.EditControls.UIComboBoxItem[] comboItems = new Janus.Windows.EditControls.UIComboBoxItem[targetingModel.ResultCnt + 1];

			comboItems[0] = new Janus.Windows.EditControls.UIComboBoxItem("Ÿ�ٱ�����","00");
			
			for(int i=0;i<targetingModel.ResultCnt;i++)
			{
                DataRow row = targetingDs.Collections.Rows[i];

				string val = row["CollectionCode"].ToString();
				string txt = row["CollectionName"].ToString();
				comboItems[i+1] = new Janus.Windows.EditControls.UIComboBoxItem(txt,val);
			}
			
			// �޺��� ��Ʈ
			this.cbCollection.Items.AddRange(comboItems);
			this.cbCollection.SelectedIndex = 0;
            */

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
				for(int i=0;i < targetingDs.Medias.Rows.Count;i++)
				{
					DataRow row = targetingDs.Medias.Rows[i];					
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
            if(commonModel.UserLevel=="30")
            {
                cbSearchRap.SelectedValue = commonModel.RapCode;			
                cbSearchRap.ReadOnly = true;				
            }
            if(commonModel.UserLevel=="40")
            {
                cbSearchAgency.SelectedValue = commonModel.AgencyCode;			
                cbSearchAgency.ReadOnly = true;					
            }	
		
            Application.DoEvents();
        }

        private void InitButton()
        {
			if(canRead)
			{
				btnSearch.Enabled = true;
				btnExcel.Enabled = true;
			}
            grdExScheduleList.Focus();

            Application.DoEvents();
        }

       
        private void DisableButton()
        {
            btnSearch.Enabled	= false;
			btnExcel.Enabled = false;
            btnSave.Enabled		= false;
            Application.DoEvents();
        }

		private void Init_RegionData()
		{
			try
			{
				// �����͸� �ʱ�ȭ
				targetingModel.Init();

				// ���� Ÿ���� �����ȸ ���񽺸� ȣ���Ѵ�.
				new TargetingManager(systemModel,commonModel).GetRegionList(targetingModel);

				if (targetingModel.ResultCD.Equals("0000"))
				{
					Utility.SetDataTable(targetingDs.TargetRegion, targetingModel.RegionDataSet);		
				}
			}
			catch(FrameException fe)
			{
				FrameSystem.showMsgForm("������������ ��ȸ����", new string[] {fe.ErrCode, fe.ResultMsg});
			}
			catch(Exception ex)
			{
				FrameSystem.showMsgForm("������������ ��ȸ����",new string[] {"",ex.Message});
			}
		}

		private void Init_AgeData()
		{
			try
			{
				// �����͸� �ʱ�ȭ
				targetingModel.Init();

				// ���� Ÿ���� �����ȸ ���񽺸� ȣ���Ѵ�.
				new TargetingManager(systemModel,commonModel).GetAgeList(targetingModel);

				if (targetingModel.ResultCD.Equals("0000"))
				{
					Utility.SetDataTable(targetingDs.TargetAge, targetingModel.AgeDataSet);		
				}
			}
			catch(FrameException fe)
			{
				FrameSystem.showMsgForm("�����ɴ� ��ȸ����", new string[] {fe.ErrCode, fe.ResultMsg});
			}
			catch(Exception ex)
			{
				FrameSystem.showMsgForm("�����ɴ� ��ȸ����",new string[] {"",ex.Message});
			}
		}

		
		/// <summary>
		/// ����ð� Ʈ���� ��� ����
		/// </summary>
		private void Init_tvTime()
		{
			tvTimeNodeCount  = 0;

			canChecking = false;	// üũó�� �̺�Ʈ���� ó�������� �۵����� �ʵ���

			tvTime.Nodes.Clear();

			tvTime.Nodes.Add(new TreeNode("��ü�ð���"));
			
			TreeNode timeNode = null;

			for(int i=0;i<24;i++)
			{
				int From = i;
				int To   = i+1;

				string strFrom = From.ToString();
				string strTo   = To.ToString();

				if(strFrom.Length == 1) strFrom = "0" + strFrom;
				if(strTo.Length   == 1) strTo   = "0" + strTo;

				//string TimeText = strFrom + "�� ~ " + strTo + "��";
				//string TimeText = strFrom + "�� ~ " + strTo + "��";

				// �ð��� �߰�
				timeNode = new TreeNode(strFrom + "��");
				timeNode.Tag = strFrom;
				tvTime.Nodes[0].Nodes.Add(timeNode);
				tvTimeNodeCount++;
			}

			// 1�ܰ踦 Ȯ���Ͽ� �����ش�.
			tvTime.Nodes[0].Expand();		

			canChecking = true;	// üũó�� �̺�Ʈ���� ó�������� �ٽ� �۵��ϵ���
		}


		/// <summary>
		/// �������� �ʱ�ȭ ����[E_02]
		/// </summary>
		private void Init_tlvRegion()
		{
			if(targetingDs.TargetRegion.Count == 0) return;
			string name = "";
			string code = "";
			string level = "";
			
			TreeListViewItem rootItem = new TreeListViewItem("��������");
			TreeListViewItem level_1 = null;
			TreeListViewItem level_2 = null;
			TreeListViewItem level_3 = null;
			
			rootItem.Items.SortOrder = System.Windows.Forms.SortOrder.None;
			for(int i=0;i<targetingDs.TargetRegion.Count;i++)
			{
				DataRow Row = targetingDs.TargetRegion.Rows[i];
				name   = Row["RegionName"].ToString();
				code   = Row["RegionCode"].ToString();
				level  = Row["Level"].ToString();

				if(code.Length < 5) code = Utility.Fixlength(code,2,5); // 5�ڸ��� �����
				if (level.Equals("1"))
				{
					level_1 = new TreeListViewItem(name,0);
					level_1.Tag = code;
					rootItem.Items.Add(level_1);
					level_1.Items.SortOrder = System.Windows.Forms.SortOrder.None;
				}
				else if(level.Equals("2"))
				{
					level_2 = new TreeListViewItem(name, 1);
					level_2.Tag = code;
					level_2.Items.SortOrder = System.Windows.Forms.SortOrder.None;
					level_1.Items.Add(level_2);					
				}
				else if (level.Equals("3"))
				{
					level_3 = new TreeListViewItem(name, 2);
					level_3.Tag = code;
					level_3.Items.SortOrder = System.Windows.Forms.SortOrder.None;
					level_2.Items.Add(level_3);
				}
			}
			tlvRegion.Items.Add(rootItem);

			tlvRegion.Items[0].Expand();
		}


		private void Init_tvAge()
		{
			tvAgeNodeCount = 0;

			canChecking = false;	// üũó�� �̺�Ʈ���� ó�������� �۵����� �ʵ���

			tvAge.Nodes.Clear();

			tvAge.Nodes.Add(new TreeNode("��ü���ɴ�"));

			if(targetingDs.TargetAge.Count == 0) return;
			
			TreeNode AgeNode = null;

			for(int i=0;i<targetingDs.TargetAge.Count;i++)
			{
				DataRow Row = targetingDs.TargetAge.Rows[i];
				string Name   = Row["AgeName"].ToString();
				string Code   = Row["AgeCode"].ToString();

				// ���ɴ� �߰�
				AgeNode = new TreeNode(Name);
				AgeNode.Tag = Code;
				tvAge.Nodes[0].Nodes.Add(AgeNode);

				tvAgeNodeCount++;
			}

			// 1�ܰ踦 Ȯ���Ͽ� �����ش�.
			tvAge.Nodes[0].Expand();		

			canChecking = true;	// üũó�� �̺�Ʈ���� ó�������� �ٽ� �۵��ϵ���
		}


        #endregion

        #region ���� Ÿ���� �׼�ó�� �޼ҵ�

        /// <summary>
        /// �׸����� Row�����
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnGrdRowChanged(object sender, System.EventArgs e) 
        {
            if (!IsSearching) // 2011.11.29 JH.Park ��ȸ���� �ƴҰ�쿡�� �����ϵ��� ����
            {
                SetDetailText();
                SetTargetingCollectionList(); //[E-03] ����Ÿ���ø���Ʈ ��ȸ �߰�  
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
            DisableButton();
            SearchTargeting();
            InitButton();
        }

        private void btnSave_Click(object sender, System.EventArgs e)
        {
            SetTargetingDetailAdd();
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
                SearchTargeting();
            }
        }

		private void tvRegion_AfterCheck(object sender, System.Windows.Forms.TreeViewEventArgs e)
		{
			if(canChecking)
			{
				try
				{
					canChecking = false;
					ChangeNodeCheck(e.Node);	// üũ�� �ٲ� ���
				}
				finally
				{
					canChecking = true;
				}
			}
		}


		private void tvTime_AfterCheck(object sender, System.Windows.Forms.TreeViewEventArgs e)
		{
			if(canChecking)
			{
				try
				{
					canChecking = false;
					ChangeNodeCheck(e.Node);	// üũ�� �ٲ� ���
				}
				finally
				{
					canChecking = true;
				}
			}
		}

		private void tvAge_AfterCheck(object sender, System.Windows.Forms.TreeViewEventArgs e)
		{
			if(canChecking)
			{
				try
				{
					canChecking = false;
					ChangeNodeCheck(e.Node);	// üũ�� �ٲ� ���
				}
				finally
				{
					canChecking = true;
				}
			}		
		}


		private void chkRegionYn_CheckedChanged(object sender, System.EventArgs e)
		{
			if(chkRegionYn.Checked)
			{
				tlvRegion.Enabled = true;
			}
			else
			{
				tlvRegion.Enabled = false;
			}
		}

		private void chkTimeYn_CheckedChanged(object sender, System.EventArgs e)
		{
			if(chkTimeYn.Checked)
			{
				tvTime.Enabled = true;
			}
			else
			{
				tvTime.Enabled = false;
			}
		}

		private void chkAgeYn_CheckedChanged(object sender, System.EventArgs e)
		{
			if(chkAgeYn.Checked)
			{
				tvAge.Enabled = true;
			}
			else
			{
				tvAge.Enabled = false;
			}
		}

		private void chkAgeBtnYn_CheckedChanged(object sender, System.EventArgs e)
		{
			if(chkAgeBtnYn.Checked)
			{
				udAgeBtnBegin.Enabled = true;
				udAgeBtnEnd.Enabled   = true;
			}
			else
			{
				udAgeBtnBegin.Enabled = false;
				udAgeBtnEnd.Enabled   = false;
			}
		}

		private void chkSexYn_CheckedChanged(object sender, System.EventArgs e)
		{
			if(chkSexYn.Checked)
			{
				chkSexMan.Enabled   = true;
				chkSexWoman.Enabled = true;
			}
			else
			{
				chkSexMan.Enabled   = false;
				chkSexWoman.Enabled = false;
			}			
		}

		private void chkCollectionYn_CheckedChanged(object sender, System.EventArgs e)
		{
            //if(chkCollectionYn.Checked)
            //{
            //    cbCollection.Enabled        =   true;
            //    rbCollectionPlus.Checked    =   true;
            //    rbCollectionPlus.Enabled    =   true;
            //    rbCollectionMinus.Enabled   =   true;
            //}
            //else
            //{
            //    cbCollection.Enabled   = false;		
            //    rbCollectionPlus.Checked    =   true;
            //    rbCollectionPlus.Enabled    =   false;
            //    rbCollectionMinus.Enabled   =   false;
            //}	
		}

		private void chkRateYn_CheckedChanged(object sender, System.EventArgs e)
		{
			if(chkRateYn.Checked)
			{
				rbRateAll.Enabled = true;
				rbRate12.Enabled  = true;
				rbRate15.Enabled  = true;
				rbRate19.Enabled  = true;
                rbRatePlus.Enabled  = true;
                rbRateMinus.Enabled = true;
			}
			else
			{
				rbRateAll.Enabled = false;
				rbRate12.Enabled  = false;
				rbRate15.Enabled  = false;
				rbRate19.Enabled  = false;
                rbRatePlus.Enabled  = false;
                rbRateMinus.Enabled = false;
			}
		}
		//���Ϻ�
		private void chkWeekYn_CheckedChanged(object sender, System.EventArgs e)
		{
			if(chkWeekYn.Checked)
			{
				chkMon.Enabled = true;
				chkThu.Enabled = true;
				chkWed.Enabled = true;
				chkThe.Enabled = true;
				chkFri.Enabled = true;
				chkSat.Enabled = true;
				chkSun.Enabled = true;
				ChkWeekChecked();
			}
			else
			{
				chkMon.Enabled = false;
				chkThu.Enabled = false;
				chkWed.Enabled = false;
				chkThe.Enabled = false;
				chkFri.Enabled = false;
				chkSat.Enabled = false;
				chkSun.Enabled = false;
				ChkWeekChecked();
			}			
		}

		private void ebContractAmt_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
		{
			if(e.KeyCode == Keys.Enter)
			{
				btnSave.Focus();
			}		
		}

		#endregion

        #region ó���޼ҵ�

        /// <summary>
        /// [E_04] ��ž������ ������ �ʱ�ȭ 
        /// </summary>
        private void Init_StbData()
        {
            try
            {
                // �����͸� �ʱ�ȭ
                targetingModel.Init();

                // ���� Ÿ���� �����ȸ ���񽺸� ȣ���Ѵ�.
                new TargetingManager(systemModel, commonModel).GetStbList(targetingModel);

                if (targetingModel.ResultCD.Equals("0000"))
                {
                    Utility.SetDataTable(targetingDs.TargetStb, targetingModel.TargetingDataSet);
                }
            }
            catch (FrameException fe)
            {
                FrameSystem.showMsgForm("��ž������ ��ȸ����", new string[] { fe.ErrCode, fe.ResultMsg });
            }
            catch (Exception ex)
            {
                FrameSystem.showMsgForm("��ž������ ��ȸ����", new string[] { "", ex.Message });
            }
        }
        
        /// <summary>
        /// [E_04] ��ž������ TreeView �ʱ�ȭ 
        /// </summary>
        private void Init_tvStb()
        {
            tvStbNodeCount = 0;

            canChecking = false;	// üũó�� �̺�Ʈ���� ó�������� �۵����� �ʵ���

            tvStb.Nodes.Clear();

            tvStb.Nodes.Add(new TreeNode("��ž�𵨺�"));

            if (targetingDs.TargetStb.Count == 0) return;

            TreeNode stbNode = null;

            for (int i = 0; i < targetingDs.TargetStb.Count; i++)
            {
                DataRow Row = targetingDs.TargetStb.Rows[i];
                //string Name  = Row["StbName"].ToString();
                string Model = Row["StbModel"].ToString();

                // �߰�
                //stbNode = new TreeNode(Name);
                stbNode = new TreeNode(Model);
                stbNode.Tag = Model;
                tvStb.Nodes[0].Nodes.Add(stbNode);

                tvStbNodeCount++;
            }

            // 1�ܰ踦 Ȯ���Ͽ� �����ش�.
            tvStb.Nodes[0].Expand();

            canChecking = true;	// üũó�� �̺�Ʈ���� ó�������� �ٽ� �۵��ϵ���
        }

        /// <summary>
        /// Ÿ���ø�� ��ȸ
        /// </summary>
        private void SearchTargeting()
        {
            IsSearching = true;

            StatusMessage("����Ÿ���� ����Ȳ�� ��ȸ�մϴ�.");
            try
            {
				uiPanelDetail.Text = "Ÿ���ü���";

                // �����͸� �ʱ�ȭ
                targetingModel.Init();

                // �����͸𵨿� ������ ������ ��Ʈ�Ѵ�.
                if(IsNewSearchKey)  targetingModel.SearchKey = "";
                else                targetingModel.SearchKey  = ebSearchKey.Text;

                targetingModel.SearchMediaCode		=  cbSearchMedia.SelectedItem.Value.ToString(); 
                targetingModel.SearchRapCode		=  cbSearchRap.SelectedItem.Value.ToString();     
                targetingModel.SearchAgencyCode	    =  cbSearchAgency.SelectedItem.Value.ToString();  
                targetingModel.SearchAdvertiserCode =  "00";
                if(chkAdState_20.Checked) targetingModel.SearchchkAdState_20 = "Y";
                if(chkAdState_30.Checked) targetingModel.SearchchkAdState_30 = "Y";
                if(chkAdState_40.Checked) targetingModel.SearchchkAdState_40 = "Y";

                // ���� Ÿ���� �����ȸ ���񽺸� ȣ���Ѵ�.
                new TargetingManager(systemModel,commonModel).GetTargetingList(targetingModel,"20");

                if (targetingModel.ResultCD.Equals("0000"))
                {
                    Utility.SetDataTable(targetingDs.TargetList, targetingModel.TargetingDataSet);		
                    StatusMessage(targetingModel.ResultCnt + "���� ���� ������ ��ȸ�Ǿ����ϴ�.");
					AddSchChoice();									
                    SetDetailText();
                }
            }
            catch(FrameException fe)
            {
                FrameSystem.showMsgForm("���� Ÿ���� ����Ȳ ��ȸ����", new string[] {fe.ErrCode, fe.ResultMsg});
            }
            catch(Exception ex)
            {
                FrameSystem.showMsgForm("���� Ÿ���� ����Ȳ ��ȸ����",new string[] {"",ex.Message});
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
			try
			{
				int rowIndex = 0;
				if ( dt.Rows.Count < 1 ) return;
              
				foreach (DataRow row in dt.Rows)
				{					
					if(row["ItemNo"].ToString().Equals(keyItemNo))
					{					
						cm.Position = rowIndex;
						break;								
					}
									
					rowIndex++;
				}
				grdExScheduleList.EnsureVisible();
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
        /// ���� Ÿ���� �������� ��Ʈ
        /// </summary>
        private void SetDetailText()
        {
            int curRow = cm.Position;

            if(curRow >= 0)
            {
				keyItemNo   = dt.Rows[curRow]["ItemNo"].ToString();		//�����ȣ
				keyItemName = dt.Rows[curRow]["ItemName"].ToString();   //����� 
                keyAdType   = dt.Rows[curRow]["AdType"].ToString();     //����Ÿ�� 

				ResetDetailText();
				lbNotice.Text	= "";

				uiPanelDetail.Text = "Ÿ���ü��� : [" + keyItemNo + "] " + keyItemName;

				// �����͸� �ʱ�ȭ
				targetingModel.Init();

				targetingModel.ItemNo		= keyItemNo; 

				// Ÿ���� ����ȸ ���񽺸� ȣ���Ѵ�.
				new TargetingManager(systemModel,commonModel).GetTargetingDetail(targetingModel);
				if (targetingModel.ResultCD.Equals("0000") && targetingModel.ResultCnt > 0)
				{
					Utility.SetDataTable(targetingDs.TargetDetial, targetingModel.DetailDataSet);						
					
					DataRow row = targetingDs.TargetDetial.Rows[0];
              
					//ResetDetailText();

					#region [ Ÿ�������� ���� 1 �⺻���� ]
					ebContractAmt.Text = row["ContractAmt"].ToString();       //���������
					cbPriorityCd.SelectedValue = row["PriorityCd"].ToString();//������ �����ڵ�

					if(cbPriorityCd.SelectedIndex == -1 )	cbPriorityCd.SelectedIndex = 4;

					// ���������
					if(row["AmtControlYn"].ToString().Equals("Y"))
					{
						rbControlYn_Y.Checked = true;
						rbControlYn_N.Checked = false;
					}
					else
					{
						rbControlYn_Y.Checked = false;
						rbControlYn_N.Checked = true;
					}

					// �������� ����
					if(row["AmtControlRate"].ToString().Length > 0)
					{	
						udControlRate.Value = Convert.ToInt16(row["AmtControlRate"].ToString());
					}
					#endregion
			
					#region [ Ÿ�������� ���� 2 �������� ][E_02]
					// [E_02] ����Ÿ����
					if(row["TgtRegion1Yn"].ToString().Equals("Y"))
					{
						chkRegionYn.Checked = true;
						tlvRegion.Enabled    = true;

						string[] chkTgtRegionSplit = Utility.SplitByString(row["TgtRegion1"].ToString(),"^");									
						setTargetRegion(chkTgtRegionSplit);
					}
					else
					{
						chkRegionYn.Checked = false;
						tlvRegion.Enabled    = false;
					}	
					#endregion

					#region [ Ÿ�������� ���� 3 �ð����� ]
					// ����ð���
					if(row["TgtTimeYn"].ToString().Equals("Y"))
					{
						chkTimeYn.Checked = true;
						tvTime.Enabled    = true;

						string[] chkTgtTimeSplit = Utility.SplitByString(row["TgtTime"].ToString(),"^");
						SetTargetTime(chkTgtTimeSplit);
					}
					else
					{
						chkTimeYn.Checked = false;
						tvTime.Enabled    = false;
					}
					#endregion

					#region [ Ÿ�������� ���� 4 ���ɴ����� ]
					// ���⿬�ɴ�
					if(row["TgtAgeYn"].ToString().Equals("Y"))
					{
						chkAgeYn.Checked = true;
						tvAge.Enabled    = true;

						string[] chkTgtAgeSplit = Utility.SplitByString(row["TgtAge"].ToString(),"^");
						SetTargetAge(chkTgtAgeSplit);
					}
					else
					{
						chkAgeYn.Checked = false;
						tvAge.Enabled    = false;
					}
					#endregion

					#region [ Ÿ�������� ���� 5 ���ɱ��� ]
					if(row["TgtAgeBtnYn"].ToString().Equals("Y"))
					{
						chkAgeBtnYn.Checked   = true;
						udAgeBtnBegin.Enabled = true;
						udAgeBtnEnd.Enabled   = true;

						if(row["TgtAgeBtnBegin"].ToString().Length > 0)
							udAgeBtnBegin.Value = Convert.ToInt16(row["TgtAgeBtnBegin"].ToString());
						else
							udAgeBtnBegin.Value = 0;		

						if(row["TgtAgeBtnEnd"].ToString().Length > 0)
							udAgeBtnEnd.Value = Convert.ToInt16(row["TgtAgeBtnEnd"].ToString());
						else
							udAgeBtnEnd.Value = 0;		
					}
					else
					{
						chkAgeBtnYn.Checked   = false;
						udAgeBtnBegin.Enabled = false;
						udAgeBtnEnd.Enabled   = false;
					}
					#endregion

					#region [ Ÿ�������� ���� 6 ���� ]
					if(row["TgtSexYn"].ToString().Equals("Y"))
					{
						chkSexYn.Checked    = true;
						chkSexMan.Enabled   = true;
						chkSexWoman.Enabled = true;

						if(row["TgtSexMan"].ToString().Equals("Y"))		chkSexMan.Checked = true;
						else											chkSexMan.Checked = false;

						if(row["TgtSexWoman"].ToString().Equals("Y"))	chkSexWoman.Checked = true;
						else											chkSexWoman.Checked = false;
					}
					else
					{
						chkSexYn.Checked    = false;
						chkSexMan.Enabled   = false;
						chkSexWoman.Enabled = false;
					}
					#endregion

					#region [ Ÿ�������� ���� 7 ��žŸ�ٱ� ]
					// Ÿ�ٱ���
					if (row["TgtCollectionYn"].ToString().Equals("Y") || row["TgtCollectionYn"].ToString().Equals("-"))
					{
						keyCollectionYn = row["TgtCollectionYn"].ToString();
						chkCollectionYn.Checked = true;
						uiTabPage3.Enabled = true;
						lblCollMsg.Visible = true;

						int cnt = Convert.ToInt32(row["TgtCollection"].ToString());

						if (cnt == 0)
						{
							lblCollMsg.ForeColor = Color.Black;
							lblCollMsg.BackColor = Color.Transparent;
							lblCollMsg.Text = "Ÿ�ٰ����� ���������� �����Ǿ� �ֽ��ϴ�.";
						}
						else if (cnt == 1)
						{
							lblCollMsg.ForeColor = Color.Black;
							lblCollMsg.BackColor = Color.Transparent;
							lblCollMsg.Text = "���!!!\n������������ ����� ������ �����մϴ�.\n���� �������� �ű��Է��Ͻʽÿ�.";
						}
						else 
						{
							lblCollMsg.ForeColor = Color.White;
							lblCollMsg.BackColor = Color.Red;

							if (cnt == 2)
							{
								lblCollMsg.Text = "����!!!\n����/���� ������ ���� �մϴ�\nȮ�� �Ͻʽÿ�";
							}
							else if (cnt == 9)
							{
								lblCollMsg.Text = "����!!!\n��ϵ� ������ �����ϴ�.";
							}
							else
							{
								lblCollMsg.Text = "����!!!\n���� ������ ������ ��ϵǾ� �ֽ��ϴ�\nȮ�� �Ͻʽÿ�";
							}
						}
					}
					else
					{
						keyCollectionYn = row["TgtCollectionYn"].ToString();
						lblCollMsg.Visible = false;
						chkCollectionYn.Checked = false;
						uiTabPage3.Enabled = false;
					}

					#endregion

					#region [ Ÿ�������� ���� 8 ������ ]
					if(row["TgtRateYn"].ToString().Equals("Y") || row["TgtRateYn"].ToString().Equals("-"))
					{
						chkRateYn.Checked  = true;

						switch(row["TgtRate"].ToString())
						{
							case "12":
								rbRate12.Checked = true;
								break;
							case "15":
								rbRate15.Checked = true;
								break;
							case "19":
								rbRate19.Checked = true;
								break;
							default  :
								rbRateAll.Checked = true;
								break;
						}

                        if( row["TgtRateYn"].ToString().Equals("-") )	rbRateMinus.Checked = true;
                        else											rbRatePlus.Checked = true;
					}
					else
					{
						chkRateYn.Checked  = false;
					}
					#endregion

					#region [ Ÿ�������� ���� 9 �������� ]
					if(row["TgtWeekYn"].ToString().Equals("Y"))
					{
						chkWeekYn.Checked    = true;

						//�������� üũ�ڽ��� ���� �� �ʵ忡 ������ �����ö�..'^'�߶� string�迭�� �ִ´�.
						//string�迭�� ���� ������ ����..
						string[] chkTgtWeekSplit = Utility.SplitByString(row["TgtWeek"].ToString(),"^");						

						for(int i=0;i < chkTgtWeekSplit.Length;i++)
						{
							string chkTgtWeek = chkTgtWeekSplit[i];

							switch(chkTgtWeek)
							{
								case "1":
									chkSun.Checked = true;
									break;
								case "2":
									chkMon.Checked = true;
									break;
								case "3":
									chkThu.Checked = true;
									break;
								case "4":
									chkWed.Checked = true;
									break;
								case "5":
									chkThe.Checked = true;
									break;
								case "6":
									chkFri.Checked = true;
									break;
								case "7":
									chkSat.Checked = true;
									break;								
							}
						}						
					}
					else
					{
						chkWeekYn.Checked    = false;
					}
					#endregion

                    #region [Ÿ�������� 13 ��ž�𵨺� ]

                    if (row["TgtStbTypeYn"].ToString().Equals("Y"))
                    {
                        chkStbModel.Checked = true;
                        //grpStb.Enabled = true;

                        string[] chkTgtAgeSplit = Utility.SplitByString(row["TgtStbType"].ToString(), "^");
                        SetTargetStb(chkTgtAgeSplit);
                    }
                    else
                    {
                        chkStbModel.Checked = false;
                        //grpStb.Enabled = false;
                    }

                    #endregion


                    isSettedTargeting = true;  // �ش籤�� Ÿ���ü������� ��Ʈ
				}
				else
				{
					lblCollMsg.Visible = false;
					lbNotice.Text	= "Ÿ���������� �������� �ʾҽ��ϴ�.";
					lbNotice.Visible = true;
                    isSettedTargeting = false;  // �ش籤�� Ÿ���ü������� ��Ʈ
				}

				if (canUpdate) btnSave.Enabled = true;
			}
            StatusMessage("�غ�");
        }

		private void SetTargetTime(string[] chkList)
		{
			canChecking = false;	// üũó�� �̺�Ʈ���� ó�������� �۵����� �ʵ���

			int chkCount = SetAllNodeTagCheck(tvTime.Nodes[0], chkList);

			if(tvTimeNodeCount == chkCount)
			{
				tvTime.Nodes[0].Checked = true;
			}
			else
			{
				tvTime.Nodes[0].Checked = false;
			}
			canChecking = true;	

		}

		private void SetTargetAge(string[] chkList)
		{
			canChecking = false;	// üũó�� �̺�Ʈ���� ó�������� �۵����� �ʵ���

			int chkCount = SetAllNodeTagCheck(tvAge.Nodes[0], chkList);

			if(tvAgeNodeCount == chkCount)
			{
				tvAge.Nodes[0].Checked = true;
			}
			else
			{
				tvAge.Nodes[0].Checked = false;
			}
			canChecking = true;	

		}

        /// <summary>
        /// ��ž�𵨺� ����Ʈ [E_08]
        /// </summary>
        /// <param name="chkList"></param>
        private void SetTargetStb(string[] chkList)
        {
#if (DEBUG)
            Debug.WriteLine(MethodBase.GetCurrentMethod().Name);
#endif
            canChecking = false;	// üũó�� �̺�Ʈ���� ó�������� �۵����� �ʵ���

            int chkCount = SetAllNodeTagCheck(tvStb.Nodes[0], chkList);

            if (tvStbNodeCount == chkCount)
            {
                tvStb.Nodes[0].Checked = true;
            }
            else
            {
                tvStb.Nodes[0].Checked = false;
            }

            canChecking = true;
        }

		private void ChkWeekChecked()
		{			
			chkMon.Checked = false;
			chkThu.Checked = false;
			chkWed.Checked = false;
			chkThe.Checked = false;
			chkFri.Checked = false;
			chkSat.Checked = false;
			chkSun.Checked = false;			
		}

		#region
        /// <summary>
        /// ���� Ÿ���� ���������� ����
        /// </summary>
        private void SetTargetingDetailAdd()
        {
            StatusMessage("Ÿ���� ������ �����մϴ�.");
            
			if(ebContractAmt.Text.Trim().Length == 0) 
			{
				MessageBox.Show("��๰���� �Էµ��� �ʾҽ��ϴ�.","Ÿ���ó��� ����", MessageBoxButtons.OK, MessageBoxIcon.Information );
				ebContractAmt.Focus();
				return;			                
			}
			else if( keyAdType.Equals("11") &&  Convert.ToInt32(ebContractAmt.Value) < 1 )
			{
				MessageBox.Show("��๰���� �Էµ��� �ʾҽ��ϴ�.\nEAP����� ��๰���� �ʼ� �׸��Դϴ�.","Ÿ���ó��� ����", MessageBoxButtons.OK, MessageBoxIcon.Information );
				ebContractAmt.Focus();
				return;			                
			}
                       
            try
            {
                //���� ���� ���� �ʱ�ȭ ���ش�.
                targetingModel.Init();

                int curRow = cm.Position;
                
				// �����͸𵨿� ������ ������ ��Ʈ�Ѵ�.
				//�����ȣ
				targetingModel.ItemNo      = keyItemNo;      
				targetingModel.ItemName    = keyItemName;      
                    
				// ������
                targetingModel.ContractAmt   = ebContractAmt.Text.Replace(",","");

				// ��
                targetingModel.PriorityCd    = cbPriorityCd.SelectedValue.ToString();

				// ���������
				if(rbControlYn_Y.Checked) targetingModel.AmtControlYn      = "Y";
				if(rbControlYn_N.Checked) targetingModel.AmtControlYn      = "N";

				// �����������
				targetingModel.AmtControlRate = udControlRate.Value.ToString();

				#region �������� ����[E_02]
				if (chkRegionYn.Checked)
				{
					targetingModel.TgtRegion1Yn = "Y";
					targetingModel.TgtRegion1   = getTargetRegion();
				}
				else
				{
					targetingModel.TgtRegion1Yn = "N";
					targetingModel.TgtRegion1   = "";
				}
				#endregion

				#region ����ð��� ����
				if (chkTimeYn.Checked)
				{
					targetingModel.TgtTimeYn = "Y";
					targetingModel.TgtTime   = GetAllNodeCheckedTag(tvTime.Nodes[0],"^");
					//������ �����ڴ� ����
					targetingModel.TgtTime   = targetingModel.TgtTime.Substring(0,targetingModel.TgtTime.Length-1);		
				}  
				else
				{
					targetingModel.TgtTimeYn = "N";
					targetingModel.TgtTime   = "";
				}
				#endregion

				#region ���⿬�ɴ� ����
				if (chkAgeYn.Checked)
				{
					targetingModel.TgtAgeYn = "Y";
					targetingModel.TgtAge   = GetAllNodeCheckedTag(tvAge.Nodes[0],"^");
					//������ �����ڴ� ����
					targetingModel.TgtAge   = targetingModel.TgtAge.Substring(0,targetingModel.TgtAge.Length-1);		
				}  
				else
				{
					targetingModel.TgtAgeYn = "N";
					targetingModel.TgtAge   = "";
				}
				#endregion

				#region ���⿬�ɱ��� ����
				if (chkAgeBtnYn.Checked)
				{
					targetingModel.TgtAgeBtnYn    = "Y";
					targetingModel.TgtAgeBtnBegin = udAgeBtnBegin.Value.ToString();
					targetingModel.TgtAgeBtnEnd   = udAgeBtnEnd.Value.ToString();
				}  
				else
				{
					targetingModel.TgtAgeBtnYn    = "N";
					targetingModel.TgtAgeBtnBegin = "0";
					targetingModel.TgtAgeBtnEnd   = "0";
				}
				#endregion

				#region ���⼺�� ����
				if (chkSexYn.Checked)
				{
					targetingModel.TgtSexYn    = "Y";
					if(chkSexMan.Checked)
					{
						targetingModel.TgtSexMan   = "Y";
					}
					else
					{
						targetingModel.TgtSexMan   = "N";
					}
					if(chkSexWoman.Checked)
					{
						targetingModel.TgtSexWoman = "Y";
					}
					else
					{
						targetingModel.TgtSexWoman = "N";
					}

				}  
				else
				{
					targetingModel.TgtSexYn    = "N";
					if(chkSexMan.Checked)   targetingModel.TgtSexMan   = "N";
					if(chkSexWoman.Checked) targetingModel.TgtSexWoman = "N";
				}
				#endregion

				#region Ÿ�ٱ�����
				if (chkCollectionYn.Checked)
				{
					// targetingModel.TgtCollectionYn = "Y";
					// 1.5�������� (-)���� �ɼ����� ������ ���
					// 2.0�̻󿡼� ������ ����ؾ� �ϴµ�
					// �̵���� ��쿣 ������ "Y"�� ������ �ȴ�.
					// �̸� �ؼ��ϱ� ���ؼ� ó����.

					if (keyCollectionYn.Equals("-"))
						targetingModel.TgtCollectionYn = "-";
					else
						targetingModel.TgtCollectionYn = "Y";
				}
				else
				{
					targetingModel.TgtCollectionYn = "N";
				}

                #endregion

                #region �����޼���
				if(chkRateYn.Checked)
				{
                    if( rbRateMinus.Checked )
                    {
                        targetingModel.TgtRateYn = "-";
                    }
                    else
                    {
                        targetingModel.TgtRateYn = "Y";
                    }
					if(rbRateAll.Checked) targetingModel.TgtRate       = "0";
					if(rbRate12.Checked) targetingModel.TgtRate        = "12";
					if(rbRate15.Checked) targetingModel.TgtRate        = "15";
					if(rbRate19.Checked) targetingModel.TgtRate        = "19";
				}
				else
				{
					targetingModel.TgtRateYn = "N";
					targetingModel.TgtRate   = "0";
				}
                #endregion

				#region ���Ϻ� ������ üũ�Ǿ�����..
				if (chkWeekYn.Checked)
				{				
					targetingModel.TgtWeekYn    = "Y";
					if(chkSun.Checked)
					{
						keySun = "1^";
					}		
					else
					{keySun = "";}
					if(chkMon.Checked)
					{
						//'^'�� �� �ʵ忡 �������� üũ�ڽ������� �Է��ϱ� ������ ������ ���� ���..
						keyMon = "2^";
					}
					else
					{keyMon = "";}
					if(chkThu.Checked)
					{
						keyThu = "3^";
					}
					else
					{keyThu = "";}
					if(chkWed.Checked)
					{
						keyWed = "4^";
					}
					else
					{keyWed = "";}
					if(chkThe.Checked)
					{
						keyThe = "5^";
					}
					else
					{keyThe = "";}
					if(chkFri.Checked)
					{
						keyFri = "6^";
					}
					else
					{keyFri = "";}
					if(chkSat.Checked)
					{
						keySat = "7^";
					}			
					else
					{keySat = "";}
				}  
				else
				{
					targetingModel.TgtWeekYn    = "N";	
					keySun = "";
					keyMon = "";
					keyThu = "";
					keyWed = "";
					keyThe = "";
					keyFri = "";
					keySat = "";
				}
				#endregion

                #region ��ž����
                if (chkStbModel.Checked)
                {
                    targetingModel.TgtStbModelYn = "Y";
                    targetingModel.TgtStbModel = GetAllNodeCheckedTag(tvStb.Nodes[0], "^");

                    if (targetingModel.TgtStbModel.Length > 1 && targetingModel.TgtStbModelYn.Equals("Y"))
                    {
                        // ������ �����ڴ� ����
                        targetingModel.TgtStbModel = targetingModel.TgtStbModel.Substring(0, targetingModel.TgtStbModel.Length - 1).Replace("-","");
                    }
                    else
                    {
                        MessageBox.Show("��ž���� ���õ��� �ʾҽ��ϴ�.", "��ž�𵨼��� ����", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                }
                else
                {
                    targetingModel.TgtStbModelYn = "N";
                    targetingModel.TgtStbModel = "";
                }
                #endregion

                #region POC����
                if (chkPoc.Checked)
                {
                    targetingModel.TgtPocYn = "Y";
                    targetingModel.TgtPoc = GetAllNodeCheckedTag(tvPoc.Nodes[0], "^");

                    if (targetingModel.TgtPoc.Length > 1 && targetingModel.TgtPocYn.Equals("Y"))
                    {
                        // ������ �����ڴ� ����
                        targetingModel.TgtPoc = targetingModel.TgtPoc.Substring(0, targetingModel.TgtPoc.Length - 1);
                    }
                    else
                    {
                        MessageBox.Show("POC������ ���õ��� �ʾҽ��ϴ�.", "POC���� ����", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                }
                else
                {
                    targetingModel.TgtPocYn = "N";
                    targetingModel.TgtPoc = "";
                }
                #endregion


                //üũ�ڽ��� ������ string���� �����Ͽ� Model�� ����..
				targetingModel.TgtWeek = keySun.ToString() + keyMon.ToString() + keyThu.ToString() + keyWed.ToString() + 
					keyThe.ToString() + keyFri.ToString() + keySat.ToString();
				//Model�� ��� ���� Substring���� ������ ���ڸ��� �ڸ���..������ �ϳ��� ���ڿ� �پ��ִ� '^'�� �����ϱ� ����..
				//ex)DB�� ����� ������ ������ �׻�'^'�پ��ֱ� ������..
				if(targetingModel.TgtWeekYn.Equals("Y"))
				{
					targetingModel.TgtWeek = targetingModel.TgtWeek.Substring(0,targetingModel.TgtWeek.Length-1);
				}

                // Ÿ���� ������ ���� ���񽺸� ȣ���Ѵ�.
                new TargetingManager(systemModel,commonModel).SetTargetingDetailUpdate(targetingModel);

                if (targetingModel.ResultCD.Equals("0000"))
                {
                    StatusMessage("Ÿ���� ������ ����Ǿ����ϴ�.");
                    DisableButton();
                    ResetDetailText();
                    SearchTargeting();
                }                        
            }
            catch(FrameException fe)
            {
                FrameSystem.showMsgForm("Ÿ���ó��� �������", new string[] {fe.ErrCode, fe.ResultMsg});
            }
            catch(Exception ex)
            {
                FrameSystem.showMsgForm("Ÿ���ó��� �������",new string[] {"",ex.Message});
            }		
        }
		#endregion

		#region
		private void SetRegionRateResetAdd()
		{
			targetingModel.Type = "01";
			targetingModel.Rate1   = "0";
			targetingModel.Rate2   = "0";
			targetingModel.Rate3   = "0";
			targetingModel.Rate4   = "0";
			targetingModel.Rate5   = "0";
			targetingModel.Rate6   = "0";
			targetingModel.Rate7   = "0";
			targetingModel.Rate8   = "0";
			targetingModel.Rate9   = "0";
			targetingModel.Rate10   = "0";
			targetingModel.Rate11   = "0";
			targetingModel.Rate12   = "0";
			targetingModel.Rate13   = "0";
			targetingModel.Rate14   = "0";
			targetingModel.Rate15   = "0";
			targetingModel.Rate16   = "0";
			targetingModel.Rate17   = "0";
			targetingModel.Rate18   = "0";
			targetingModel.Rate19   = "0";
			targetingModel.Rate20   = "0";
			targetingModel.Rate21   = "0";
			targetingModel.Rate22   = "0";
			targetingModel.Rate23   = "0";
			targetingModel.Rate24   = "0";

			// Ÿ���� ������ ���� ���񽺸� ȣ���Ѵ�.						
			new TargetingManager(systemModel,commonModel).SetTargetingRateUpdate(targetingModel);
		}

		private void SetTimeRateResetAdd()
		{
			targetingModel.Type = "02";
			targetingModel.Rate1   = "0";
			targetingModel.Rate2   = "0";
			targetingModel.Rate3   = "0";
			targetingModel.Rate4   = "0";
			targetingModel.Rate5   = "0";
			targetingModel.Rate6   = "0";
			targetingModel.Rate7   = "0";
			targetingModel.Rate8   = "0";
			targetingModel.Rate9   = "0";
			targetingModel.Rate10   = "0";
			targetingModel.Rate11   = "0";
			targetingModel.Rate12   = "0";
			targetingModel.Rate13   = "0";
			targetingModel.Rate14   = "0";
			targetingModel.Rate15   = "0";
			targetingModel.Rate16   = "0";
			targetingModel.Rate17   = "0";
			targetingModel.Rate18   = "0";
			targetingModel.Rate19   = "0";
			targetingModel.Rate20   = "0";
			targetingModel.Rate21   = "0";
			targetingModel.Rate22   = "0";
			targetingModel.Rate23   = "0";
			targetingModel.Rate24   = "0";

			// Ÿ���� ������ ���� ���񽺸� ȣ���Ѵ�.						
			new TargetingManager(systemModel,commonModel).SetTargetingRateUpdate(targetingModel);
		}

		private void SetWeekRateResetAdd()
		{
			targetingModel.Type = "03";
			targetingModel.Rate1   = "0";
			targetingModel.Rate2   = "0";
			targetingModel.Rate3   = "0";
			targetingModel.Rate4   = "0";
			targetingModel.Rate5   = "0";
			targetingModel.Rate6   = "0";
			targetingModel.Rate7   = "0";
			targetingModel.Rate8   = "0";
			targetingModel.Rate9   = "0";
			targetingModel.Rate10   = "0";
			targetingModel.Rate11   = "0";
			targetingModel.Rate12   = "0";
			targetingModel.Rate13   = "0";
			targetingModel.Rate14   = "0";
			targetingModel.Rate15   = "0";
			targetingModel.Rate16   = "0";
			targetingModel.Rate17   = "0";
			targetingModel.Rate18   = "0";
			targetingModel.Rate19   = "0";
			targetingModel.Rate20   = "0";
			targetingModel.Rate21   = "0";
			targetingModel.Rate22   = "0";
			targetingModel.Rate23   = "0";
			targetingModel.Rate24   = "0";

			// Ÿ���� ������ ���� ���񽺸� ȣ���Ѵ�.						
			new TargetingManager(systemModel,commonModel).SetTargetingRateUpdate(targetingModel);
		}
		#endregion

        /// <summary>
        /// �󼼳��� Text �ʱ�ȭ
        /// </summary>
        private void ResetDetailText()
        {
            // OAP�� Ÿ���������� ��� ��������� ������� ����
			// OAP������ �����õ������ ó����
            if( keyAdType.Equals("20") )
            {
                // ���⹰��-������
                ebContractAmt.Text =  "0";
                ebContractAmt.Enabled = false;

                // ��-������
                cbPriorityCd.SelectedIndex = 5;
                cbPriorityCd.Enabled    = false;

                // �����-������
                rbControlYn_Y.Checked = false;
                rbControlYn_N.Checked = true;
                rbControlYn_Y.Enabled = false;
                rbControlYn_N.Enabled = false;

                // �������-������
                udControlRate.Value     = 100; 
                udControlRate.Enabled   = false;
            
                // �������� �ʱ�ȭ 
                chkTimeYn.Checked = false;
                clearCheckingRegion();

                // ����ð��� �ʱ�ȭ
                chkRegionYn.Checked = false;
                SetTargetTime(null);

                // ���⿬�ɴ� �ʱ�ȭ
                chkAgeYn.Checked = false;
                SetTargetAge(null);

                // ���⿬�ɱ���
                chkAgeBtnYn.Checked   = false;
                udAgeBtnBegin.Value = 0;		
                udAgeBtnEnd.Value   = 99;	

                // ���⼺��
                chkSexYn.Checked    = false;
                chkSexMan.Checked   = false;
                chkSexWoman.Checked = false;

                chkCollectionYn.Enabled		=	true;
                chkCollectionYn.Checked     =	false;

                //cbCollection.Enabled        =	false;		
                //rbCollectionPlus.Checked    =   true;
                //rbCollectionPlus.Enabled    =   false;
                //rbCollectionMinus.Enabled   =   false;

                // ������
                chkRateYn.Checked  = false;
                rbRateAll.Checked  = true;
                rbRate12.Checked   = false;
                rbRate15.Checked   = false;
                rbRate19.Checked   = false;
            
                rbRatePlus.Checked = true;
                rbRateMinus.Checked = false;

                // ���Ϻ�
                chkWeekYn.Checked    = false;
                chkMon.Checked = false;
                chkThu.Checked = false;
                chkWed.Checked = false;
                chkThe.Checked = false;
                chkFri.Checked = false;
                chkSat.Checked = false;
                chkSun.Checked = false;	
            }
            else if( keyAdType.Equals("11") )	// EAP �������� ������
            {
                // ���⹰��-���
                ebContractAmt.Text =  "0";
                ebContractAmt.Enabled = true;

                // ��-������
                cbPriorityCd.SelectedIndex = 5;
                cbPriorityCd.Enabled    = false;

                // �����-���
                rbControlYn_Y.Checked = true;
                rbControlYn_N.Checked = false;
                rbControlYn_Y.Enabled   = false;
                rbControlYn_N.Enabled   = false;

                // �������-���
                udControlRate.Value     = 100; 
                udControlRate.Enabled   = true;
            
                // �������� �ʱ�ȭ 
                chkTimeYn.Checked = false;
                clearCheckingRegion();

                // ����ð��� �ʱ�ȭ
                chkRegionYn.Checked = false;
                SetTargetTime(null);

                // ���⿬�ɴ� �ʱ�ȭ
                chkAgeYn.Checked = false;
                SetTargetAge(null);

                // ���⿬�ɱ���
                chkAgeBtnYn.Checked   = false;
                udAgeBtnBegin.Value = 0;		
                udAgeBtnEnd.Value   = 0;	

                // ���⼺��
                chkSexYn.Checked    = false;
                chkSexMan.Checked   = false;
                chkSexWoman.Checked = false;

                // ����ž���� EAP�� ���
				chkCollectionYn.Enabled		=	true;
				chkCollectionYn.Checked     =	false;
                //cbCollection.Enabled        =   false;		
                //rbCollectionPlus.Checked    =   true;
                //rbCollectionPlus.Enabled    =   false;
                //rbCollectionMinus.Enabled   =   false;

                // ������
                chkRateYn.Checked  = false;
                rbRateAll.Checked  = true;
                rbRate12.Checked   = false;
                rbRate15.Checked   = false;
                rbRate19.Checked   = false;
            
                rbRatePlus.Checked = true;
                rbRateMinus.Checked = false;

                // ���Ϻ�
                chkWeekYn.Checked    = false;
                chkMon.Checked = false;
                chkThu.Checked = false;
                chkWed.Checked = false;
                chkThe.Checked = false;
                chkFri.Checked = false;
                chkSat.Checked = false;
                chkSun.Checked = false;	
            }
			else // SCM��
			{
				// ���⹰��-�̻��
				ebContractAmt.Text =  "0";
				ebContractAmt.Enabled = false;

				// ��-������
				cbPriorityCd.SelectedIndex = 5;
				cbPriorityCd.Enabled    = true;

				// �����-������
				rbControlYn_Y.Checked = false;
				rbControlYn_N.Checked = true;
				rbControlYn_Y.Enabled = false;
				rbControlYn_N.Enabled = false;

				// �������-������
				udControlRate.Value     = 100; 
				udControlRate.Enabled   = false;
            
				// �������� �ʱ�ȭ 
				chkTimeYn.Checked = false;
				clearCheckingRegion();

				// ����ð��� �ʱ�ȭ
				chkRegionYn.Checked = false;
				SetTargetTime(null);

				// ���⿬�ɴ� �ʱ�ȭ
				chkAgeYn.Checked = false;
				SetTargetAge(null);

				// ���⿬�ɱ���
				chkAgeBtnYn.Checked   = false;
				udAgeBtnBegin.Value = 0;		
				udAgeBtnEnd.Value   = 0;	

				// ���⼺��
				chkSexYn.Checked    = false;
				chkSexMan.Checked   = false;
				chkSexWoman.Checked = false;

				// ����ž���� EAP�� ���
				chkCollectionYn.Enabled		=	true;
				chkCollectionYn.Checked     =	false;
                //cbCollection.Enabled        =   false;		
                //rbCollectionPlus.Checked    =   true;
                //rbCollectionPlus.Enabled    =   false;
                //rbCollectionMinus.Enabled   =   false;

				// ������
				chkRateYn.Checked  = false;
				rbRateAll.Checked  = true;
				rbRate12.Checked   = false;
				rbRate15.Checked   = false;
				rbRate19.Checked   = false;
            
				rbRatePlus.Checked = true;
				rbRateMinus.Checked = false;

				// ���Ϻ�
				chkWeekYn.Checked    = false;
				chkMon.Checked = false;
				chkThu.Checked = false;
				chkWed.Checked = false;
				chkThe.Checked = false;
				chkFri.Checked = false;
				chkSat.Checked = false;
				chkSun.Checked = false;	
			}
            // ��ž�𵨺� [E_08]
            chkStbModel.Checked = false;
            //grpStb.Enabled = false;
            SetTargetStb(null);



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
			Excel.Application xlApp= null;
			Excel._Workbook   oWB = null;
			Excel._Worksheet  oSheet = null;
			Excel.Range       oRng = null;
			
			try
			{	
				int ColMax  = 12; // �÷���   				

				int TitleRow  = 1;		
				int ConditionRow1 = 2; 				
				int HeaderRow = 3;				
				string StartCol = "A";
				string EndCol   = "";
				string TitleCol = "L";				
				int CondCount = 0;
				int HeaderCount = 0;

				// ������ �÷��� �ε�������
				EndCol = GetColumnIndex(ColMax);

				xlApp = new Excel.Application();
				oWB = (Excel._Workbook)(xlApp.Workbooks.Add(Missing.Value));
				oSheet = (Excel._Worksheet)oWB.ActiveSheet;


				// Ÿ��Ʋ �ۼ�
				oSheet.Cells[TitleRow,1] = "���������";
				oRng = oSheet.get_Range(StartCol+Convert.ToString(TitleRow), TitleCol+Convert.ToString(TitleRow));
				oRng.Merge(true);
				oRng.Font.Bold = true;
				oRng.Font.Size = 16;
				oRng.VerticalAlignment   = Excel.XlVAlign.xlVAlignCenter;			
				oRng.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;		

				// �������� �ۼ�
				oSheet.Cells[ConditionRow1+CondCount,1] = "�������";
				oRng = oSheet.get_Range("B"+Convert.ToString(ConditionRow1+CondCount), TitleCol+Convert.ToString(ConditionRow1+CondCount));
				oRng.Merge(true);
				
				if(chkAdState_20.Checked)
				{
					oSheet.Cells[ConditionRow1+CondCount,2] = "��";
				}
				if(chkAdState_30.Checked)
				{
					oSheet.Cells[ConditionRow1+CondCount,2] = "����";
				}
				if(chkAdState_40.Checked)
				{
					oSheet.Cells[ConditionRow1+CondCount,2] = "����";
				}
				if(chkAdState_20.Checked && chkAdState_30.Checked)
				{
					oSheet.Cells[ConditionRow1+CondCount,2] = "��, ����";
				}
				if(chkAdState_20.Checked && chkAdState_40.Checked)
				{
					oSheet.Cells[ConditionRow1+CondCount,2] = "��, ����";
				}
				if(chkAdState_30.Checked && chkAdState_40.Checked)
				{
					oSheet.Cells[ConditionRow1+CondCount,2] = "����, ����";
				}
				if(chkAdState_20.Checked && chkAdState_30.Checked && chkAdState_40.Checked)
				{
					oSheet.Cells[ConditionRow1+CondCount,2] = "��, ����, ����";
				}
				// ���Ǻ� �׵θ�
				oRng = oSheet.get_Range(StartCol+Convert.ToString(ConditionRow1), TitleCol+Convert.ToString(ConditionRow1+(CondCount-1)));
				oRng.VerticalAlignment   = Excel.XlVAlign.xlVAlignCenter;			
				oRng.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;	
	
				oRng.Borders.LineStyle =  Excel.XlLineStyle.xlContinuous;	// �׵θ��Ӽ� �Ʒ��� �Ǽ�
				oRng.Borders.Weight    = Excel.XlBorderWeight.xlThin;		// �׵θ��Ӽ� �Ʒ��� ���¼�
								
				CondCount++;
								
				// ��� ���� �ۼ�
				HeaderCount = 1;
				oSheet.Cells[HeaderRow,HeaderCount++] = "�����ȣ"; 
				//				oRng = oSheet.get_Range(GetColumnIndex(1)+Convert.ToString(HeaderRow), GetColumnIndex(1)+Convert.ToString(HeaderRow));
				//				oRng.Merge(true);
				//				HeaderCount++;

				oSheet.Cells[HeaderRow,HeaderCount++] = "�����";
				oSheet.Cells[HeaderRow,HeaderCount++] = "������";
				oSheet.Cells[HeaderRow,HeaderCount++] = "���������";
				oSheet.Cells[HeaderRow,HeaderCount++] = "����������";
				oSheet.Cells[HeaderRow,HeaderCount++] = "����������";
				oSheet.Cells[HeaderRow,HeaderCount++] = "��������";
				oSheet.Cells[HeaderRow,HeaderCount++] = "�������";
				oSheet.Cells[HeaderRow,HeaderCount++] = "��๰��";
				oSheet.Cells[HeaderRow,HeaderCount++] = "�����";				
				oSheet.Cells[HeaderRow,HeaderCount++] = "����";
				oSheet.Cells[HeaderRow,HeaderCount++] = "������";
												
				oRng = oSheet.get_Range(StartCol+Convert.ToString(HeaderRow), EndCol+Convert.ToString(HeaderRow)); // ����� ����
				oRng.Font.Bold           = true;							// ��Ʈ ����
				oRng.VerticalAlignment   = Excel.XlVAlign.xlVAlignCenter;	// �����߾�����
				oRng.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;	// �����߾�����	 
				oRng.Interior.Color      = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.CornflowerBlue);   //�� ���� 
				oRng.Font.Color          = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.White);	  //�ؽ�Ʈ��			

				string[,] items = new string[targetingDs.Tables[0].Rows.Count, 12];
				// ������ ����
				for (int inx =0; inx < targetingDs.Tables[0].Rows.Count; inx++)
				{
					items[inx, 0] = targetingDs.Tables[0].Rows[inx]["ItemNo"].ToString();					
					items[inx, 1] = targetingDs.Tables[0].Rows[inx]["ItemName"].ToString();					
					items[inx, 2] = targetingDs.Tables[0].Rows[inx]["ContStateName"].ToString();					
					items[inx, 3] = targetingDs.Tables[0].Rows[inx]["ExcuteStartDay"].ToString();
					items[inx, 4] = targetingDs.Tables[0].Rows[inx]["ExcuteEndDay"].ToString();
					items[inx, 5] = targetingDs.Tables[0].Rows[inx]["RealEndDay"].ToString();
					items[inx, 6] = targetingDs.Tables[0].Rows[inx]["AdTypeName"].ToString();

					items[inx, 7] = targetingDs.Tables[0].Rows[inx]["AdStateName"].ToString();
					items[inx, 8] = targetingDs.Tables[0].Rows[inx]["TgtAmount"].ToString();
					items[inx, 9] = targetingDs.Tables[0].Rows[inx]["PriorityCd"].ToString();
					items[inx, 10] = targetingDs.Tables[0].Rows[inx]["ContractName"].ToString();
					items[inx, 11] = targetingDs.Tables[0].Rows[inx]["AdvertiserName"].ToString();															
										
				}
				oSheet.get_Range("A4", "L"+Convert.ToString((items.Length/12)+3)).set_Value(Missing.Value, items);

				oRng = oSheet.get_Range(StartCol+Convert.ToString(HeaderRow), EndCol+Convert.ToString(targetingDs.Tables[0].Rows.Count));	// �������� ����
				oRng.EntireColumn.AutoFit();					// �������� ũ�⿡ ���� ����ũ�� ����

				oRng.Borders.LineStyle =  Excel.XlLineStyle.xlContinuous;	// �׵θ��Ӽ� �Ʒ��� �Ǽ�
				oRng.Borders.Weight    = Excel.XlBorderWeight.xlThin;		// �׵θ��Ӽ� �Ʒ��� ���¼�
				
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

		#endregion

		#region TreeView ����

		private void ChangeNodeCheck(TreeNode nowNode)
		{
			TreeNode parentNode = nowNode.Parent; // �θ���

			if(parentNode != null)
			{

				int nodeCount = parentNode.GetNodeCount(false);	// �θ����� �ڽĳ��� = ��������� ��, �������� ���Ծ��� 
				int checkedCount = 0;

				// �������鿡�� üũ�� ���� ��´�.
				foreach (TreeNode childNode in parentNode.Nodes)
				{
					if(childNode.Checked) checkedCount++;
				}		
				
				// ��� üũ�� �Ǿ��ٸ�
				if(nodeCount == checkedCount)
				{
					parentNode.Checked = true; // �θ����� üũ�� true;
				}
				else
				{
					parentNode.Checked = false; // �θ����� üũ�� false;
				}
			}

			// ��� �ڽĳ�忡 ���Ͽ� üũ�� �����Ѵ�. 
			bool chk = nowNode.Checked;
			CheckAllNode(nowNode, chk);
		}

		// ������ ��� ����� üũ�� �����Ѵ�.
		private void CheckAllNode(TreeNode node, bool chk)
		{
			node.Checked = chk;
			foreach (TreeNode childNode in node.Nodes)
			{
				CheckAllNode(childNode, chk);
			}
		}

		//������带 �����ϴ� Tag�� �����ڷ� �����Ͽ� ��ȯ�Ѵ�.
		private string GetAllNodeCheckedTag(TreeNode node, string dilm)
		{
			string tag = "";

			if(node.Checked)
			{
				if(node.Tag != null)
				{
					tag = node.Tag.ToString() + dilm;
				}
			}

			foreach (TreeNode childNode in node.Nodes)
			{
				tag += GetAllNodeCheckedTag(childNode, dilm);
			}	
			
			return tag;
		}


		// �ڵ尡 üũ����Ʈ�� �����ϴ��� �˻�
		private bool IsCheckedCode(string Code, string[] chkList)
		{
			if(chkList == null) return false;

			for(int i=0;i<chkList.Length;i++)
			{
				if(Code.Replace("-","").Equals(chkList[i]))
				{
					return true;
				}
			}
			return false;
		}
	

		// ������带 �����Ͽ� Tag�� üũ����Ʈ�� ������ Node�� üũ�Ѵ�.
		private int SetAllNodeTagCheck(TreeNode node, string[] chkList)
		{
			int cnt = 0;

			string tag = "";


			if(node.Tag != null)
			{
				tag = node.Tag.ToString();

				// �������� ���� üũ����Ʈ�� �����ϴ��� �˻�
				if(IsCheckedCode(tag, chkList))
				{
					node.Checked = true;
					cnt++;
				}
				else
				{
					node.Checked = false;
				}
			}
			else
			{
				node.Checked = false;
			}

			// ������� �˻�
			foreach (TreeNode childNode in node.Nodes)
			{
				cnt += SetAllNodeTagCheck(childNode, chkList);
			}	
			return cnt;
		}

		#endregion		

		#region �������� Ÿ���� �߰� �� �κ�[E_02]
		/// <summary>
		/// ���� Ÿ���� treeviewlist clear[E_02]
		/// </summary>
		private void clearCheckingRegion()
		{
			if (tlvRegion.Items == null) return;
			try
			{
				TreeListViewItem level_2 = null;
				
				for (int i = 0; i < tlvRegion.Items[0].Items.Count; i++)
				{
					tlvRegion.Items[0].Checked = false;
					for(int j=0; j<tlvRegion.Items[0].Items[i].Items.Count; j++)
					{
						level_2 = tlvRegion.Items[0].Items[i].Items[j];
						level_2.Checked  = false;
						if (level_2.Items.Count > 0)
						{
							for( int k =0; k < level_2.Items.Count; k++)							
								level_2.Items[k].Checked = false;							
						}						
					}
				}			
			}
			catch(Exception ex)
			{
				Trace.Write(ex.Message);
			}
		}


		/// <summary>
		/// üŷ�� ���� Ÿ���� ���ڿ� ��������[E_02]
		/// </summary>
		private string getTargetRegion()
		{
			/*
			 * ������ ����2~3�ܰ� tag(���ڿ�) ���� ���Ѵ�.
			 * - ����� �θ� ���� �ʿ�ġ ����.
			*/
			string region = "";
			try
			{
				TreeListViewItem level_2 = null;
			
				for (int i = 0; i < tlvRegion.Items[0].Items.Count; i++)
				{
					for(int j=0; j<tlvRegion.Items[0].Items[i].Items.Count; j++)
					{
						level_2 = tlvRegion.Items[0].Items[i].Items[j];
						if (level_2.Items.Count > 0)
						{
							for( int k =0; k < level_2.Items.Count; k++)
							{
								if (level_2.Items[k].Checked)
								{
									region += level_2.Items[k].Tag.ToString() + "^";
									Trace.WriteLine(level_2.Items[k].Tag);
								}
							}
						}
						else
						{
							if (level_2.Checked)
							{
								region += level_2.Tag.ToString() + "^";
								Trace.WriteLine(level_2.Tag);
							}
						}
					}
				}

				// ������ ������ ����ó��
				region = region.Substring(0, region.Length - 1);
			}
			catch(Exception ex)
			{
				Trace.Write(ex.Message);
			}
			
			return region;
		}


		/// <summary>
		/// ���� Ÿ���� ���� TreeListView�� üŷó��[E_02]
		/// </summary>		
		private void setTargetRegion(string[] splits)
		{
			if (splits != null && splits.Length > 0)
			{				
				TreeListViewItem level_2 = null;

				for (int h=0; h < splits.Length; h++)  //�������� ���ڿ� �迭
				{
					for (int i = 0; i < tlvRegion.Items[0].Items.Count; i++) // level 1(����,���...)
					{
						for(int j=0; j<tlvRegion.Items[0].Items[i].Items.Count; j++) //level 2(�Ⱦ��,���ֽ�...,)
						{
							level_2 = tlvRegion.Items[0].Items[i].Items[j]; 
							
							if (level_2.Items.Count > 0) // level 3�ܰ� ����üũ  
							{									
								for( int k = 0; k < level_2.Items.Count; k++)  // û�ֽ��� ��(��籸,�����)..
								{
									if (level_2.Items[k].Tag.ToString().Equals(splits[h]))
									{
										level_2.Items[k].Checked = true;										
										break;
									}
								}								
							}
							else
							{   
								if (level_2.Tag.ToString().Equals(splits[h])) // level 2�ܰ踸 ����
								{
									level_2.Checked = true;							
									break;
								}
							}

						}//end for level 2					
					}
				}

				reCheckingRegion();
			}
		}


		/// <summary>
		/// üŷ ������ ��Ʈ�� �ٽ� �˼�[E_02]
		/// </summary>
		private void reCheckingRegion()
		{
			int nLevel_2 = 0;
			int nLevel_3 = 0;

			TreeListViewItem level_2 = null;
			// ���� �������� ��� ���� �Ǹ�
			// ���� �����۵� �Բ� üŷ�Ǿ�� �ϴµ�
			// �ڵ����� ���� �ʾƼ� �� �� �� �˼��ϴ� ����.
			for (int i = 0; i < tlvRegion.Items[0].Items.Count; i++) // level 1(����,���...)
			{
				nLevel_2 = 0;
				for(int j=0; j<tlvRegion.Items[0].Items[i].Items.Count; j++) //level 2(�Ⱦ��,���ֽ�...,)
				{
					level_2 = tlvRegion.Items[0].Items[i].Items[j]; 
							
					if (level_2.Items.Count > 0) // level 3�ܰ� ����üũ  
					{				
						nLevel_3 = 0;
						for( int k = 0; k < level_2.Items.Count; k++)  // û�ֽ��� ��(��籸,�����)..
						{
							if (level_2.Items[k].Checked)
								nLevel_3 += 1;															
						}
						if (level_2.ChildrenCount == nLevel_3)
						{
							level_2.Checked = true;
							nLevel_2 += 1;
						}
					}
					else
					{   
						if (level_2.Checked)
							nLevel_2 += 1;
					}
				}//end for level 2

				if (nLevel_2 == tlvRegion.Items[0].Items[i].Items.Count)
					tlvRegion.Items[0].Items[i].Checked = true;
			}
		}


		#endregion

        #region ����Ÿ���� ��ȸ �� ���� [E-03]
        private void SetTargetingCollectionList()
        {
            
#if (DEBUG)
            Debug.WriteLine(MethodBase.GetCurrentMethod().Name);
#endif
            ResetTargetingCollection();

            int curRow = cm.Position;

            if (curRow >= 0)
            {
                // �����͸� �ʱ�ȭ
                targetingModel.Init();
                targetingModel.ItemNo = keyItemNo;

                new TargetingManager(systemModel, commonModel).GetTargetingCollectionList(targetingModel);

                if (targetingModel.ResultCD.Equals("0000"))
                {
                    Utility.SetDataTable(targetingDs.TargetingCollection, targetingModel.TargetingCollectionDataSet);
                }
            }

            // Ÿ���ü����� �Ǿ� �ִٸ� ����Ÿ���ü��� �߰�/���� ��ư�� Ȱ��ȭ ��Ų��.
            if (isSettedTargeting)
            {
                btnAddCollection.Enabled = true;
				btnAddCollectionMinus.Enabled = true;
                btnDeleteCollection.Enabled = true;
            }
            else
            {
                btnAddCollection.Enabled = false;
				btnAddCollectionMinus.Enabled = false;
                btnDeleteCollection.Enabled = false;
            }
             
        }


        private void ResetTargetingCollection()
        {
            
#if (DEBUG)
            Debug.WriteLine(MethodBase.GetCurrentMethod().Name);
#endif
            // �������� üũ Ŭ����
            grdExSouceCollectionList.UnCheckAllRecords();
             
        }


        // �߰�~
        private void btnAddCollection_Click(object sender, EventArgs e)
        {
			this.AddCollection("+");            
        }

		private void btnAddCollectionMinus_Click(object sender, EventArgs e)
		{
			this.AddCollection("-");
		}

		private void AddCollection(string dirt)
		{
			int MAX_COUNT = 20; // �ִ� 20������ ������ �����ϴ�. ���� �̰� �������Ϸ� �̵��Ͽ��� �� ��.

			// �׸��忡 ����� �����͸� Datasource�� ������Ʈ �Ѵ�.
			grdExSouceCollectionList.UpdateData();

			try
			{
				int SettedCount = 0;
				int SetCount = 0;
				int CheckCount = 0;


				// �����Ϸ� ���õ� ������ ��
				CheckCount = grdExSouceCollectionList.GetCheckedRows().Length;

				if (CheckCount == 0)
				{
					MessageBox.Show("������ ������ �����ϴ�.", "����Ÿ���� �߰�",MessageBoxButtons.OK, MessageBoxIcon.Information);
					return;
				}

				// ���� �����Ǿ��ִ� ������ ��
				SettedCount = grdExTargetList.GetRows().Length;

				Debug.WriteLine("SettedCount:" + SettedCount);

				foreach (GridEXRow gr in grdExSouceCollectionList.GetCheckedRows())
				{
					if ((SettedCount + SetCount) >= MAX_COUNT)  // �ִ�ġ �̻��ΰ�?
					{
						MessageBox.Show("����� �ִ� " + MAX_COUNT.ToString() + "���� ������ ���� �����մϴ�.", "����Ÿ���� �߰�",
							MessageBoxButtons.OK, MessageBoxIcon.Information);

						break;
					}

					// �̹� ������ �����Ǿ��ִ��� �˻�
					bool isExist = false;
					foreach (GridEXRow row in grdExTargetList.GetRows())
					{
						if (gr.Cells["Code"].Value.ToString().Equals(row.Cells["Code"].Value.ToString()))
						{

							MessageBox.Show("[" + gr.Cells["CollectionName"].Value.ToString() + "]��(��) �̹� Ÿ���ÿ� �����Ǿ� �ֽ��ϴ�.", "����Ÿ���� �߰�",
								MessageBoxButtons.OK, MessageBoxIcon.Information);

							isExist = true;
							break;
						}
					}

					// �����Ǿ��ִ� ������ �ƴѰ�쿡 �߰�
					if (!isExist)
					{
						targetingModel.Init();

						targetingModel.ItemNo = keyItemNo;
						targetingModel.CollectionCode = gr.Cells["Code"].Value.ToString();
						targetingModel.TgtCollectionYn = dirt;
						
						new TargetingManager(systemModel, commonModel).SetTargetingCollectionAdd(targetingModel);

						if (targetingModel.ResultCD.Equals("0000"))
						{
							SetCount++;
						}
					}

					gr.IsChecked = false;  // ��������

				}

				SetTargetingCollectionList();


			}
			catch (FrameException fe)
			{
				FrameSystem.showMsgForm("����Ÿ���� �߰� ����", new string[] { fe.ErrCode, fe.ResultMsg });
			}
			catch (Exception ex)
			{
				FrameSystem.showMsgForm("����Ÿ���� �߰� ����", new string[] { "", ex.Message });
			}
            
		}

        private void btnDeleteCollection_Click(object sender, EventArgs e)
        {
            
            // �׸��忡 ����� �����͸� Datasource�� ������Ʈ �Ѵ�.
            grdExTargetList.UpdateData();

            try
            {
                int SetCount = 0;

                foreach (GridEXRow gr in grdExTargetList.GetCheckedRows())
                {
                    targetingModel.Init();

                    targetingModel.ItemNo = keyItemNo;

                    targetingModel.CollectionCode = gr.Cells["Code"].Value.ToString();

                    new TargetingManager(systemModel, commonModel).SetTargetingCollectionDelete(targetingModel);

                    if (targetingModel.ResultCD.Equals("0000"))
                    {
                        SetCount++;
                    }

                    gr.IsChecked = false;  // ��������
                }

                SetTargetingCollectionList();

                if (SetCount == 0)
                {
                    MessageBox.Show("������ ������ �����ϴ�.", "����Ÿ���� ����",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

            }
            catch (FrameException fe)
            {
                FrameSystem.showMsgForm("����Ÿ���� �߰� ����", new string[] { fe.ErrCode, fe.ResultMsg });
            }
            catch (Exception ex)
            {
                FrameSystem.showMsgForm("����Ÿ���� �߰� ����", new string[] { "", ex.Message });
            }
             
        }

        #endregion

        private void chkPoc_CheckedChanged(object sender, EventArgs e)
        {
            if (chkPoc.Checked)
            {
                tvPoc.Enabled = true;
            }
            else
            {
                tvPoc.Enabled = false;
            }	
        }

        private void chkStbModel_CheckedChanged(object sender, EventArgs e)
        {
            if (chkStbModel.Checked)
            {
                tvStb.Enabled = true;
            }
            else
            {
                tvStb.Enabled = false;
            }
        }

        private void tvStb_AfterCheck(object sender, TreeViewEventArgs e)
        {
            if (canChecking)
            {
                try
                {
                    canChecking = false;
                    ChangeNodeCheck(e.Node);	// üũ�� �ٲ� ���
                }
                finally
                {
                    canChecking = true;
                }
            }
        }
    }
}
