// ===============================================================================
// TargetingHomeControl for Charites Project
//
// TargetingHomeControl.cs
//
// 광고 타겟팅 컨트롤을 정의합니다. 
//
// ===============================================================================
// Release history
//
// ===============================================================================
// Copyright (C) 2006 G-Inno Systems Inc.
// All rights reserved.
/* [수정사항]
 * -------------------------------------------------------
 * 수정코드  : [E_01]
 * 수정자    : HJ
 * 수정일    : 2015.06.02
 * 수정내용  : 
 *      1. 셋탑모델, POC 타겟팅 기능 추가
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

using Janus.Windows.GridEX;

using WinFramework.Misc;
using WinFramework.Base;
using WinFramework.Data;

using AdManagerModel;
using Excel = Microsoft.Office.Interop.Excel; // 엑셀 참조
using System.Reflection;


namespace AdManagerClient
{
	/// <summary>
	/// 광고 타겟팅 관리 컨트롤
	/// </summary>
    public class TargetingHomeControl : System.Windows.Forms.UserControl, IUserControl
    {
        #region 이벤트핸들러
        public event StatusEventHandler StatusEvent;			// 상태이벤트 핸들러
        public event ProgressEventHandler ProgressEvent;			// 처리중이벤트 핸들러
        #endregion

        #region 사용자정의 객체 및 변수

        // 시스템 정보 : 화면공통
        private SystemModel systemModel = FrameSystem.oSysModel;
        private CommonModel commonModel = FrameSystem.oComModel;
        private Logger log = FrameSystem.oLog;
        private MenuPower menu = FrameSystem.oMenu;

        // 메뉴코드 : 보안이 필요한 화면에 필요함
        public string menuCode = "";

        // 사용할 정보모델
        TargetingHomeModel targetingHomeModel = new TargetingHomeModel();	// 광고 타겟팅편성모델

        // 화면처리용 변수
        bool IsNewSearchKey = true;					// 검색어입력 여부
        CurrencyManager cm = null;					// 데이터 그리드의 변경에 따른 데이터셋 관리를 위하여			
        DataTable dt = null;

        bool IsSearching = false; // 조회중 상세화면이 업데이트 되는 것을 방지 하기위함 2011.11.29 JH.Park
        bool canRead = false;
        bool canUpdate = false;

        // 타겟팅설정여부
        private bool isSettedTargeting = false;

        // Key 데이터
        string keyItemNo = "";
        string keyMediaCode = "";
        string keyItemName = "";
        string keyTime00 = "";
        string keyTime01 = "";
        string keyTime02 = "";
        string keyTime03 = "";
        string keyTime04 = "";
        string keyTime05 = "";
        string keyTime06 = "";
        string keyTime07 = "";
        string keyTime08 = "";
        string keyTime09 = "";
        string keyTime10 = "";
        string keyTime11 = "";
        string keyTime12 = "";
        string keyTime13 = "";
        string keyTime14 = "";
        string keyTime15 = "";
        string keyTime16 = "";
        string keyTime17 = "";
        string keyTime18 = "";
        string keyTime19 = "";
        string keyTime20 = "";
        string keyTime21 = "";
        string keyTime22 = "";
        string keyTime23 = "";

        // 트리뷰용
        private bool canChecking = true;

        //private int tvRegionNodeCount = 0;
        private int tvTimeNodeCount = 0;
        private int tvAgeNodeCount = 0;
        private int tvStbNodeCount = 0;

        string keyMon = "";
        string keyThu = "";
        string keyWed = "";
        string keyThe = "";
        string keyFri = "";
        string keySat = "";
        private Janus.Windows.EditControls.UICheckBox chkTargetN;
        private Janus.Windows.EditControls.UICheckBox chkTargetY;
        private Janus.Windows.EditControls.UICheckBox chkAdState_40;
        private Janus.Windows.EditControls.UICheckBox chkAdState_30;
        private Janus.Windows.EditControls.UICheckBox chkAdState_20;
        private Janus.Windows.UI.Tab.UITab uiTab1;
        private Janus.Windows.UI.Tab.UITabPage uiTabPage1;
        private Janus.Windows.EditControls.UIRadioButton rbControlYn_Y;
        private Janus.Windows.EditControls.UICheckBox chkCollectionYn;
        private Janus.Windows.EditControls.UIRadioButton rbControlYn_N;
        private Janus.Windows.EditControls.UICheckBox chkWeekYn;
        private Janus.Windows.EditControls.UIGroupBox uiGroupBox1;
        private Janus.Windows.EditControls.UICheckBox chkSun;
        private Janus.Windows.EditControls.UICheckBox chkSat;
        private Janus.Windows.EditControls.UICheckBox chkFri;
        private Janus.Windows.EditControls.UICheckBox chkThe;
        private Janus.Windows.EditControls.UICheckBox chkWed;
        private Janus.Windows.EditControls.UICheckBox chkMon;
        private Janus.Windows.EditControls.UICheckBox chkThu;
        private Janus.Windows.EditControls.UICheckBox chkSexYn;
        private Janus.Windows.EditControls.UIGroupBox grpSex;
        private Janus.Windows.EditControls.UICheckBox chkSexMan;
        private Janus.Windows.EditControls.UICheckBox chkSexWoman;
        private Janus.Windows.EditControls.UICheckBox chkAgeBtnYn;
        private Janus.Windows.GridEX.EditControls.IntegerUpDown udAgeBtnBegin;
        private Label label4;
        private Janus.Windows.GridEX.EditControls.IntegerUpDown udAgeBtnEnd;
        private Label label5;
        private TreeView tvAge;
        private Janus.Windows.EditControls.UICheckBox chkAgeYn;
        private Janus.Windows.EditControls.UICheckBox chkRegionYn;
        private Label lbNotice;
        private Janus.Windows.GridEX.EditControls.IntegerUpDown udControlRate;
        private Janus.Windows.GridEX.EditControls.NumericEditBox ebContractAmt;
        private Label label3;
        private Label lbUserID;
        private Janus.Windows.EditControls.UIComboBox cbPriorityCd;
        private Janus.Windows.EditControls.UIButton btnSave;
        private Label label2;
        private Label label6;
        private Label label7;
        private Janus.Windows.EditControls.UICheckBox chkTimeYn;
        private Janus.Windows.EditControls.UIGroupBox grpTime;
        private Janus.Windows.EditControls.UICheckBox chkTime23;
        private Janus.Windows.EditControls.UICheckBox chkTime22;
        private Janus.Windows.EditControls.UICheckBox chkTime21;
        private Janus.Windows.EditControls.UICheckBox chkTime20;
        private Janus.Windows.EditControls.UICheckBox chkTime19;
        private Janus.Windows.EditControls.UICheckBox chkTime18;
        private Janus.Windows.EditControls.UICheckBox chkTime17;
        private Janus.Windows.EditControls.UICheckBox chkTime16;
        private Janus.Windows.EditControls.UICheckBox chkTime15;
        private Janus.Windows.EditControls.UICheckBox chkTime14;
        private Janus.Windows.EditControls.UICheckBox chkTime13;
        private Janus.Windows.EditControls.UICheckBox chkTime12;
        private Janus.Windows.EditControls.UICheckBox chkTime11;
        private Janus.Windows.EditControls.UICheckBox chkTime10;
        private Janus.Windows.EditControls.UICheckBox chkTime09;
        private Janus.Windows.EditControls.UICheckBox chkTime08;
        private Janus.Windows.EditControls.UICheckBox chkTime07;
        private Janus.Windows.EditControls.UICheckBox chkTime06;
        private Janus.Windows.EditControls.UICheckBox chkTime05;
        private Janus.Windows.EditControls.UICheckBox chkTime04;
        private Janus.Windows.EditControls.UICheckBox chkTime03;
        private Janus.Windows.EditControls.UICheckBox chkTime02;
        private Janus.Windows.EditControls.UICheckBox chkTime00;
        private Janus.Windows.EditControls.UICheckBox chkTime01;
        private Janus.Windows.UI.Tab.UITabPage uiTabPage2;
        private Label label63;
        private Label label62;
        private Panel panel4;
        private Panel panel1;
        private Label label58;
        private Label label9;
        private GridEX grdExSouceCollectionList;
        private GridEX grdExTargetList;
        private DataView dvCollection;
        private DataView dvTargetingCollection;
        private Panel panel6;
        private Label lbMsg2;
        private Janus.Windows.EditControls.UIButton btnAddCollection;
        private Panel panel5;
        private Label label8;
        private Janus.Windows.EditControls.UIButton btnDeleteCollection;
        private Label lblCollMsg;
        private Janus.Windows.EditControls.UIButton btnAddCollectionMinus;
        private TreeListView tlvRegion;
        private ColumnHeader columnHeader1;
        private ImageList imgRegion;
        private Janus.Windows.EditControls.UIGroupBox grpStb;
        private TreeView tvStb;
        private Janus.Windows.EditControls.UICheckBox chkStbModel;
        private Janus.Windows.EditControls.UIGroupBox uiGroupBox2;
        private TreeView tvPoc;
        private Janus.Windows.EditControls.UICheckBox chkPoc;
        string keySun = "";

        public string ItemNo
        {
            get { return keyItemNo; }
            set { keyItemNo = value; }
        }

        public string MediaCode
        {
            get { return keyMediaCode; }
            set { keyMediaCode = value; }
        }

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
            this.TargetingHomeControl_Load(this, null);
        }
        #endregion

        #region 화면 컴포넌트, 생성자, 소멸자

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
        private System.Windows.Forms.Label lbAdState;
        private Janus.Windows.EditControls.UIComboBox cbSearchMedia;
        private Janus.Windows.EditControls.UIComboBox cbSearchRap;
        private Janus.Windows.EditControls.UIComboBox cbSearchAgency;
        private Janus.Windows.EditControls.UIComboBox cbSearchAdvertiser;
        private Janus.Windows.GridEX.GridEX grdExScheduleList;
        private Janus.Windows.EditControls.UIComboBox cbSearchContractState;
        private System.Windows.Forms.Label label1;
        private Janus.Windows.GridEX.EditControls.EditBox editBox1;
        private System.Windows.Forms.Panel panMenuSchedule;
        private System.Windows.Forms.Panel panel2;
        private Janus.Windows.UI.Dock.UIPanel uiPanelDetail;
        private Janus.Windows.UI.Dock.UIPanelInnerContainer uiPanelDetailContainer;
        private System.Windows.Forms.Panel panel3;
        private System.Data.DataView dvTargeting;
        private Janus.Windows.EditControls.UIButton btnExcel;
        private AdManagerClient._40_Target._02_TargetHome.TargetingHomeDs targetingHomeDs;
        private System.Windows.Forms.Label lbTargetYn;
        private System.ComponentModel.IContainer components;

        public TargetingHomeControl()
        {
            // 이 호출은 Windows.Forms Form 디자이너에 필요합니다.
            InitializeComponent();
        }

        /// <summary> 
        /// 사용 중인 모든 리소스를 정리합니다.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
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
            Janus.Windows.GridEX.GridEXLayout grdExScheduleList_Layout_0 = new Janus.Windows.GridEX.GridEXLayout();
            Janus.Windows.Common.Layouts.JanusLayoutReference grdExScheduleList_Layout_0_Reference_0 = new Janus.Windows.Common.Layouts.JanusLayoutReference("GridEXLayoutData.RootTable.FormatConditions.Condition1.FormatStyle.BackgroundImag" +
                    "e");
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TargetingHomeControl));
            Janus.Windows.Common.Layouts.JanusLayoutReference grdExScheduleList_Layout_0_Reference_1 = new Janus.Windows.Common.Layouts.JanusLayoutReference("GridEXLayoutData.RootTable.FormatConditions.Condition2.FormatStyle.BackgroundImag" +
                    "e");
            Janus.Windows.Common.Layouts.JanusLayoutReference grdExScheduleList_Layout_0_Reference_2 = new Janus.Windows.Common.Layouts.JanusLayoutReference("GridEXLayoutData.RootTable.FormatConditions.Condition3.FormatStyle.BackgroundImag" +
                    "e");
            Janus.Windows.Common.Layouts.JanusLayoutReference grdExScheduleList_Layout_0_Reference_3 = new Janus.Windows.Common.Layouts.JanusLayoutReference("GridEXLayoutData.RootTable.FormatConditions.Condition4.FormatStyle.BackgroundImag" +
                    "e");
            Janus.Windows.Common.Layouts.JanusLayoutReference grdExScheduleList_Layout_0_Reference_4 = new Janus.Windows.Common.Layouts.JanusLayoutReference("GridEXLayoutData.RootTable.FormatConditions.Condition5.FormatStyle.BackgroundImag" +
                    "e");
            System.Windows.Forms.TreeNode treeNode1 = new System.Windows.Forms.TreeNode("POC 구분");
            System.Windows.Forms.TreeNode treeNode2 = new System.Windows.Forms.TreeNode("셋탑모델별");
            System.Windows.Forms.TreeListViewItemCollection.TreeListViewItemCollectionComparer treeListViewItemCollectionComparer1 = new System.Windows.Forms.TreeListViewItemCollection.TreeListViewItemCollectionComparer();
            System.Windows.Forms.TreeNode treeNode3 = new System.Windows.Forms.TreeNode("0~19세");
            System.Windows.Forms.TreeNode treeNode4 = new System.Windows.Forms.TreeNode("20~29세");
            System.Windows.Forms.TreeNode treeNode5 = new System.Windows.Forms.TreeNode("30~39세");
            System.Windows.Forms.TreeNode treeNode6 = new System.Windows.Forms.TreeNode("40~49세");
            System.Windows.Forms.TreeNode treeNode7 = new System.Windows.Forms.TreeNode("50~59세");
            System.Windows.Forms.TreeNode treeNode8 = new System.Windows.Forms.TreeNode("60세이상");
            System.Windows.Forms.TreeNode treeNode9 = new System.Windows.Forms.TreeNode("전체연령대", new System.Windows.Forms.TreeNode[] {
            treeNode3,
            treeNode4,
            treeNode5,
            treeNode6,
            treeNode7,
            treeNode8});
            Janus.Windows.GridEX.GridEXLayout grdExSouceCollectionList_DesignTimeLayout = new Janus.Windows.GridEX.GridEXLayout();
            Janus.Windows.GridEX.GridEXLayout grdExTargetList_DesignTimeLayout = new Janus.Windows.GridEX.GridEXLayout();
            this.dvTargeting = new System.Data.DataView();
            this.targetingHomeDs = new AdManagerClient._40_Target._02_TargetHome.TargetingHomeDs();
            this.uiPM = new Janus.Windows.UI.Dock.UIPanelManager(this.components);
            this.uiPanelChoiceAdSchedule = new Janus.Windows.UI.Dock.UIPanelGroup();
            this.uiPanelSearch = new Janus.Windows.UI.Dock.UIPanel();
            this.uiPanelSearchContainer = new Janus.Windows.UI.Dock.UIPanelInnerContainer();
            this.pnlSearch = new System.Windows.Forms.Panel();
            this.chkTargetN = new Janus.Windows.EditControls.UICheckBox();
            this.chkAdState_40 = new Janus.Windows.EditControls.UICheckBox();
            this.chkAdState_30 = new Janus.Windows.EditControls.UICheckBox();
            this.chkAdState_20 = new Janus.Windows.EditControls.UICheckBox();
            this.chkTargetY = new Janus.Windows.EditControls.UICheckBox();
            this.lbTargetYn = new System.Windows.Forms.Label();
            this.lbAdState = new System.Windows.Forms.Label();
            this.cbSearchContractState = new Janus.Windows.EditControls.UIComboBox();
            this.ebSearchKey = new Janus.Windows.GridEX.EditControls.EditBox();
            this.cbSearchMedia = new Janus.Windows.EditControls.UIComboBox();
            this.cbSearchRap = new Janus.Windows.EditControls.UIComboBox();
            this.cbSearchAgency = new Janus.Windows.EditControls.UIComboBox();
            this.cbSearchAdvertiser = new Janus.Windows.EditControls.UIComboBox();
            this.pnlSearchBtn = new System.Windows.Forms.Panel();
            this.btnExcel = new Janus.Windows.EditControls.UIButton();
            this.btnSearch = new Janus.Windows.EditControls.UIButton();
            this.uiPanelList = new Janus.Windows.UI.Dock.UIPanel();
            this.uiPanelListContainer = new Janus.Windows.UI.Dock.UIPanelInnerContainer();
            this.grdExScheduleList = new Janus.Windows.GridEX.GridEX();
            this.uiPanelDetail = new Janus.Windows.UI.Dock.UIPanel();
            this.uiPanelDetailContainer = new Janus.Windows.UI.Dock.UIPanelInnerContainer();
            this.panel3 = new System.Windows.Forms.Panel();
            this.uiTab1 = new Janus.Windows.UI.Tab.UITab();
            this.uiTabPage1 = new Janus.Windows.UI.Tab.UITabPage();
            this.uiGroupBox2 = new Janus.Windows.EditControls.UIGroupBox();
            this.tvPoc = new System.Windows.Forms.TreeView();
            this.chkPoc = new Janus.Windows.EditControls.UICheckBox();
            this.grpStb = new Janus.Windows.EditControls.UIGroupBox();
            this.tvStb = new System.Windows.Forms.TreeView();
            this.chkStbModel = new Janus.Windows.EditControls.UICheckBox();
            this.tlvRegion = new System.Windows.Forms.TreeListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.imgRegion = new System.Windows.Forms.ImageList(this.components);
            this.lblCollMsg = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label58 = new System.Windows.Forms.Label();
            this.rbControlYn_Y = new Janus.Windows.EditControls.UIRadioButton();
            this.chkCollectionYn = new Janus.Windows.EditControls.UICheckBox();
            this.rbControlYn_N = new Janus.Windows.EditControls.UIRadioButton();
            this.chkWeekYn = new Janus.Windows.EditControls.UICheckBox();
            this.uiGroupBox1 = new Janus.Windows.EditControls.UIGroupBox();
            this.chkSun = new Janus.Windows.EditControls.UICheckBox();
            this.chkSat = new Janus.Windows.EditControls.UICheckBox();
            this.chkFri = new Janus.Windows.EditControls.UICheckBox();
            this.chkThe = new Janus.Windows.EditControls.UICheckBox();
            this.chkWed = new Janus.Windows.EditControls.UICheckBox();
            this.chkMon = new Janus.Windows.EditControls.UICheckBox();
            this.chkThu = new Janus.Windows.EditControls.UICheckBox();
            this.chkSexYn = new Janus.Windows.EditControls.UICheckBox();
            this.grpSex = new Janus.Windows.EditControls.UIGroupBox();
            this.chkSexMan = new Janus.Windows.EditControls.UICheckBox();
            this.chkSexWoman = new Janus.Windows.EditControls.UICheckBox();
            this.chkAgeBtnYn = new Janus.Windows.EditControls.UICheckBox();
            this.udAgeBtnBegin = new Janus.Windows.GridEX.EditControls.IntegerUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.udAgeBtnEnd = new Janus.Windows.GridEX.EditControls.IntegerUpDown();
            this.label5 = new System.Windows.Forms.Label();
            this.tvAge = new System.Windows.Forms.TreeView();
            this.chkAgeYn = new Janus.Windows.EditControls.UICheckBox();
            this.chkRegionYn = new Janus.Windows.EditControls.UICheckBox();
            this.lbNotice = new System.Windows.Forms.Label();
            this.udControlRate = new Janus.Windows.GridEX.EditControls.IntegerUpDown();
            this.ebContractAmt = new Janus.Windows.GridEX.EditControls.NumericEditBox();
            this.label3 = new System.Windows.Forms.Label();
            this.lbUserID = new System.Windows.Forms.Label();
            this.cbPriorityCd = new Janus.Windows.EditControls.UIComboBox();
            this.btnSave = new Janus.Windows.EditControls.UIButton();
            this.label2 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.chkTimeYn = new Janus.Windows.EditControls.UICheckBox();
            this.grpTime = new Janus.Windows.EditControls.UIGroupBox();
            this.chkTime23 = new Janus.Windows.EditControls.UICheckBox();
            this.chkTime22 = new Janus.Windows.EditControls.UICheckBox();
            this.chkTime21 = new Janus.Windows.EditControls.UICheckBox();
            this.chkTime20 = new Janus.Windows.EditControls.UICheckBox();
            this.chkTime19 = new Janus.Windows.EditControls.UICheckBox();
            this.chkTime18 = new Janus.Windows.EditControls.UICheckBox();
            this.chkTime17 = new Janus.Windows.EditControls.UICheckBox();
            this.chkTime16 = new Janus.Windows.EditControls.UICheckBox();
            this.chkTime15 = new Janus.Windows.EditControls.UICheckBox();
            this.chkTime14 = new Janus.Windows.EditControls.UICheckBox();
            this.chkTime13 = new Janus.Windows.EditControls.UICheckBox();
            this.chkTime12 = new Janus.Windows.EditControls.UICheckBox();
            this.chkTime11 = new Janus.Windows.EditControls.UICheckBox();
            this.chkTime10 = new Janus.Windows.EditControls.UICheckBox();
            this.chkTime09 = new Janus.Windows.EditControls.UICheckBox();
            this.chkTime08 = new Janus.Windows.EditControls.UICheckBox();
            this.chkTime07 = new Janus.Windows.EditControls.UICheckBox();
            this.chkTime06 = new Janus.Windows.EditControls.UICheckBox();
            this.chkTime05 = new Janus.Windows.EditControls.UICheckBox();
            this.chkTime04 = new Janus.Windows.EditControls.UICheckBox();
            this.chkTime03 = new Janus.Windows.EditControls.UICheckBox();
            this.chkTime02 = new Janus.Windows.EditControls.UICheckBox();
            this.chkTime00 = new Janus.Windows.EditControls.UICheckBox();
            this.chkTime01 = new Janus.Windows.EditControls.UICheckBox();
            this.uiTabPage2 = new Janus.Windows.UI.Tab.UITabPage();
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
            this.label8 = new System.Windows.Forms.Label();
            this.btnDeleteCollection = new Janus.Windows.EditControls.UIButton();
            this.panMenuSchedule = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.editBox1 = new Janus.Windows.GridEX.EditControls.EditBox();
            ((System.ComponentModel.ISupportInitialize)(this.dvTargeting)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.targetingHomeDs)).BeginInit();
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
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelDetail)).BeginInit();
            this.uiPanelDetail.SuspendLayout();
            this.uiPanelDetailContainer.SuspendLayout();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uiTab1)).BeginInit();
            this.uiTab1.SuspendLayout();
            this.uiTabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uiGroupBox2)).BeginInit();
            this.uiGroupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grpStb)).BeginInit();
            this.grpStb.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uiGroupBox1)).BeginInit();
            this.uiGroupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grpSex)).BeginInit();
            this.grpSex.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grpTime)).BeginInit();
            this.grpTime.SuspendLayout();
            this.uiTabPage2.SuspendLayout();
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
            // dvTargeting
            // 
            this.dvTargeting.Table = this.targetingHomeDs.TargetList;
            // 
            // targetingHomeDs
            // 
            this.targetingHomeDs.DataSetName = "TargetingHomeDs";
            this.targetingHomeDs.Locale = new System.Globalization.CultureInfo("en-US");
            this.targetingHomeDs.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
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
            this.uiPM.AddDockPanelInfo(new System.Guid("afb3b7d0-aa79-443c-a1e8-3baba38f7a4d"), new System.Guid("b162c53a-7940-47cd-a0ed-9762c59c50d8"), 65, true);
            this.uiPM.AddDockPanelInfo(new System.Guid("8e3140b9-bfcb-43af-b84e-2ee6cddf51dc"), new System.Guid("b162c53a-7940-47cd-a0ed-9762c59c50d8"), 200, true);
            this.uiPM.AddDockPanelInfo(new System.Guid("f94be314-c212-42b8-b676-497c4d5f5485"), new System.Guid("b162c53a-7940-47cd-a0ed-9762c59c50d8"), 362, true);
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
            this.uiPanelChoiceAdSchedule.Text = "광고 홈타겟팅관리";
            // 
            // uiPanelSearch
            // 
            this.uiPanelSearch.AllowResize = Janus.Windows.UI.InheritableBoolean.False;
            this.uiPanelSearch.CaptionVisible = Janus.Windows.UI.InheritableBoolean.False;
            this.uiPanelSearch.CloseButtonVisible = Janus.Windows.UI.InheritableBoolean.False;
            this.uiPanelSearch.InnerContainer = this.uiPanelSearchContainer;
            this.uiPanelSearch.Location = new System.Drawing.Point(0, 22);
            this.uiPanelSearch.Name = "uiPanelSearch";
            this.uiPanelSearch.Size = new System.Drawing.Size(1010, 67);
            this.uiPanelSearch.TabIndex = 4;
            this.uiPanelSearch.Text = "검색";
            // 
            // uiPanelSearchContainer
            // 
            this.uiPanelSearchContainer.Controls.Add(this.pnlSearch);
            this.uiPanelSearchContainer.Location = new System.Drawing.Point(1, 1);
            this.uiPanelSearchContainer.Name = "uiPanelSearchContainer";
            this.uiPanelSearchContainer.Size = new System.Drawing.Size(1008, 65);
            this.uiPanelSearchContainer.TabIndex = 0;
            // 
            // pnlSearch
            // 
            this.pnlSearch.BackColor = System.Drawing.SystemColors.Window;
            this.pnlSearch.Controls.Add(this.chkTargetN);
            this.pnlSearch.Controls.Add(this.chkAdState_40);
            this.pnlSearch.Controls.Add(this.chkAdState_30);
            this.pnlSearch.Controls.Add(this.chkAdState_20);
            this.pnlSearch.Controls.Add(this.chkTargetY);
            this.pnlSearch.Controls.Add(this.lbTargetYn);
            this.pnlSearch.Controls.Add(this.lbAdState);
            this.pnlSearch.Controls.Add(this.cbSearchContractState);
            this.pnlSearch.Controls.Add(this.ebSearchKey);
            this.pnlSearch.Controls.Add(this.cbSearchMedia);
            this.pnlSearch.Controls.Add(this.cbSearchRap);
            this.pnlSearch.Controls.Add(this.cbSearchAgency);
            this.pnlSearch.Controls.Add(this.cbSearchAdvertiser);
            this.pnlSearch.Controls.Add(this.pnlSearchBtn);
            this.pnlSearch.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlSearch.Location = new System.Drawing.Point(0, 0);
            this.pnlSearch.Name = "pnlSearch";
            this.pnlSearch.Size = new System.Drawing.Size(1008, 65);
            this.pnlSearch.TabIndex = 3;
            // 
            // chkTargetN
            // 
            this.chkTargetN.Location = new System.Drawing.Point(128, 36);
            this.chkTargetN.Name = "chkTargetN";
            this.chkTargetN.Size = new System.Drawing.Size(65, 23);
            this.chkTargetN.TabIndex = 15;
            this.chkTargetN.Text = "미설정";
            this.chkTargetN.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            // 
            // chkAdState_40
            // 
            this.chkAdState_40.Location = new System.Drawing.Point(360, 36);
            this.chkAdState_40.Name = "chkAdState_40";
            this.chkAdState_40.Size = new System.Drawing.Size(44, 23);
            this.chkAdState_40.TabIndex = 15;
            this.chkAdState_40.Text = "종료";
            this.chkAdState_40.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            // 
            // chkAdState_30
            // 
            this.chkAdState_30.Checked = true;
            this.chkAdState_30.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkAdState_30.Location = new System.Drawing.Point(310, 36);
            this.chkAdState_30.Name = "chkAdState_30";
            this.chkAdState_30.Size = new System.Drawing.Size(44, 23);
            this.chkAdState_30.TabIndex = 15;
            this.chkAdState_30.Text = "중지";
            this.chkAdState_30.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            // 
            // chkAdState_20
            // 
            this.chkAdState_20.Checked = true;
            this.chkAdState_20.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkAdState_20.Location = new System.Drawing.Point(260, 36);
            this.chkAdState_20.Name = "chkAdState_20";
            this.chkAdState_20.Size = new System.Drawing.Size(44, 23);
            this.chkAdState_20.TabIndex = 15;
            this.chkAdState_20.Text = "편성";
            this.chkAdState_20.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            // 
            // chkTargetY
            // 
            this.chkTargetY.Checked = true;
            this.chkTargetY.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkTargetY.Location = new System.Drawing.Point(78, 36);
            this.chkTargetY.Name = "chkTargetY";
            this.chkTargetY.Size = new System.Drawing.Size(44, 23);
            this.chkTargetY.TabIndex = 15;
            this.chkTargetY.Text = "설정";
            this.chkTargetY.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            // 
            // lbTargetYn
            // 
            this.lbTargetYn.Location = new System.Drawing.Point(6, 36);
            this.lbTargetYn.Name = "lbTargetYn";
            this.lbTargetYn.Size = new System.Drawing.Size(72, 23);
            this.lbTargetYn.TabIndex = 14;
            this.lbTargetYn.Text = "타겟팅여부";
            this.lbTargetYn.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lbAdState
            // 
            this.lbAdState.Location = new System.Drawing.Point(199, 36);
            this.lbAdState.Name = "lbAdState";
            this.lbAdState.Size = new System.Drawing.Size(57, 23);
            this.lbAdState.TabIndex = 13;
            this.lbAdState.Text = "광고상태";
            this.lbAdState.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cbSearchContractState
            // 
            this.cbSearchContractState.BackColor = System.Drawing.Color.White;
            this.cbSearchContractState.ComboStyle = Janus.Windows.EditControls.ComboStyle.DropDownList;
            this.cbSearchContractState.Location = new System.Drawing.Point(520, 8);
            this.cbSearchContractState.Name = "cbSearchContractState";
            this.cbSearchContractState.Size = new System.Drawing.Size(120, 21);
            this.cbSearchContractState.TabIndex = 6;
            this.cbSearchContractState.Text = "계약상태";
            this.cbSearchContractState.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            // 
            // ebSearchKey
            // 
            this.ebSearchKey.Location = new System.Drawing.Point(648, 8);
            this.ebSearchKey.Name = "ebSearchKey";
            this.ebSearchKey.Size = new System.Drawing.Size(200, 21);
            this.ebSearchKey.TabIndex = 5;
            this.ebSearchKey.Text = "검색어를 입력하세요";
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
            this.cbSearchMedia.Size = new System.Drawing.Size(120, 21);
            this.cbSearchMedia.TabIndex = 1;
            this.cbSearchMedia.Text = "매체선택";
            this.cbSearchMedia.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            // 
            // cbSearchRap
            // 
            this.cbSearchRap.ComboStyle = Janus.Windows.EditControls.ComboStyle.DropDownList;
            this.cbSearchRap.Location = new System.Drawing.Point(136, 8);
            this.cbSearchRap.Name = "cbSearchRap";
            this.cbSearchRap.Size = new System.Drawing.Size(120, 21);
            this.cbSearchRap.TabIndex = 2;
            this.cbSearchRap.Text = "미디어렙선택";
            this.cbSearchRap.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            // 
            // cbSearchAgency
            // 
            this.cbSearchAgency.ComboStyle = Janus.Windows.EditControls.ComboStyle.DropDownList;
            this.cbSearchAgency.Location = new System.Drawing.Point(264, 8);
            this.cbSearchAgency.Name = "cbSearchAgency";
            this.cbSearchAgency.Size = new System.Drawing.Size(120, 21);
            this.cbSearchAgency.TabIndex = 3;
            this.cbSearchAgency.Text = "대행사선택";
            this.cbSearchAgency.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            // 
            // cbSearchAdvertiser
            // 
            this.cbSearchAdvertiser.ComboStyle = Janus.Windows.EditControls.ComboStyle.DropDownList;
            this.cbSearchAdvertiser.Location = new System.Drawing.Point(392, 8);
            this.cbSearchAdvertiser.Name = "cbSearchAdvertiser";
            this.cbSearchAdvertiser.Size = new System.Drawing.Size(120, 21);
            this.cbSearchAdvertiser.TabIndex = 4;
            this.cbSearchAdvertiser.Text = "광고주선택";
            this.cbSearchAdvertiser.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            // 
            // pnlSearchBtn
            // 
            this.pnlSearchBtn.BackColor = System.Drawing.SystemColors.Window;
            this.pnlSearchBtn.Controls.Add(this.btnExcel);
            this.pnlSearchBtn.Controls.Add(this.btnSearch);
            this.pnlSearchBtn.Dock = System.Windows.Forms.DockStyle.Right;
            this.pnlSearchBtn.Location = new System.Drawing.Point(888, 0);
            this.pnlSearchBtn.Name = "pnlSearchBtn";
            this.pnlSearchBtn.Size = new System.Drawing.Size(120, 65);
            this.pnlSearchBtn.TabIndex = 13;
            // 
            // btnExcel
            // 
            this.btnExcel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExcel.Enabled = false;
            this.btnExcel.Location = new System.Drawing.Point(8, 34);
            this.btnExcel.Name = "btnExcel";
            this.btnExcel.Size = new System.Drawing.Size(104, 24);
            this.btnExcel.TabIndex = 14;
            this.btnExcel.Text = "EXCEL출력";
            this.btnExcel.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.btnExcel.Click += new System.EventHandler(this.btnExcel_Click);
            // 
            // btnSearch
            // 
            this.btnSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSearch.Enabled = false;
            this.btnSearch.Location = new System.Drawing.Point(8, 7);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(104, 24);
            this.btnSearch.TabIndex = 13;
            this.btnSearch.Text = "조 회";
            this.btnSearch.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // uiPanelList
            // 
            this.uiPanelList.BackColor = System.Drawing.SystemColors.Window;
            this.uiPanelList.CaptionStyle = Janus.Windows.UI.Dock.PanelCaptionStyle.Dark;
            this.uiPanelList.InnerContainer = this.uiPanelListContainer;
            this.uiPanelList.Location = new System.Drawing.Point(0, 93);
            this.uiPanelList.MinimumSize = new System.Drawing.Size(-1, 100);
            this.uiPanelList.Name = "uiPanelList";
            this.uiPanelList.Size = new System.Drawing.Size(1010, 207);
            this.uiPanelList.TabIndex = 4;
            this.uiPanelList.Text = "홈광고내역목록";
            // 
            // uiPanelListContainer
            // 
            this.uiPanelListContainer.BackColor = System.Drawing.SystemColors.Window;
            this.uiPanelListContainer.Controls.Add(this.grdExScheduleList);
            this.uiPanelListContainer.Location = new System.Drawing.Point(1, 23);
            this.uiPanelListContainer.Name = "uiPanelListContainer";
            this.uiPanelListContainer.Size = new System.Drawing.Size(1008, 183);
            this.uiPanelListContainer.TabIndex = 0;
            // 
            // grdExScheduleList
            // 
            this.grdExScheduleList.AllowEdit = Janus.Windows.GridEX.InheritableBoolean.False;
            this.grdExScheduleList.AlternatingColors = true;
            this.grdExScheduleList.BorderStyle = Janus.Windows.GridEX.BorderStyle.None;
            this.grdExScheduleList.DataSource = this.dvTargeting;
            this.grdExScheduleList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdExScheduleList.EmptyRows = true;
            this.grdExScheduleList.FocusCellFormatStyle.BackColor = System.Drawing.SystemColors.Highlight;
            this.grdExScheduleList.FocusCellFormatStyle.ForeColor = System.Drawing.Color.White;
            this.grdExScheduleList.FocusStyle = Janus.Windows.GridEX.FocusStyle.None;
            this.grdExScheduleList.Font = new System.Drawing.Font("나눔고딕", 8.249999F);
            this.grdExScheduleList.GridLineColor = System.Drawing.Color.Silver;
            this.grdExScheduleList.GridLines = Janus.Windows.GridEX.GridLines.Vertical;
            this.grdExScheduleList.GridLineStyle = Janus.Windows.GridEX.GridLineStyle.Solid;
            this.grdExScheduleList.GroupByBoxVisible = false;
            this.grdExScheduleList.HeaderFormatStyle.TextAlignment = Janus.Windows.GridEX.TextAlignment.Center;
            this.grdExScheduleList.HideSelection = Janus.Windows.GridEX.HideSelection.Highlight;
            grdExScheduleList_Layout_0.DataSource = this.dvTargeting;
            grdExScheduleList_Layout_0.IsCurrentLayout = true;
            grdExScheduleList_Layout_0.Key = "1";
            grdExScheduleList_Layout_0_Reference_0.Instance = ((object)(resources.GetObject("grdExScheduleList_Layout_0_Reference_0.Instance")));
            grdExScheduleList_Layout_0_Reference_1.Instance = ((object)(resources.GetObject("grdExScheduleList_Layout_0_Reference_1.Instance")));
            grdExScheduleList_Layout_0_Reference_2.Instance = ((object)(resources.GetObject("grdExScheduleList_Layout_0_Reference_2.Instance")));
            grdExScheduleList_Layout_0_Reference_3.Instance = ((object)(resources.GetObject("grdExScheduleList_Layout_0_Reference_3.Instance")));
            grdExScheduleList_Layout_0_Reference_4.Instance = ((object)(resources.GetObject("grdExScheduleList_Layout_0_Reference_4.Instance")));
            grdExScheduleList_Layout_0.LayoutReferences.AddRange(new Janus.Windows.Common.Layouts.JanusLayoutReference[] {
            grdExScheduleList_Layout_0_Reference_0,
            grdExScheduleList_Layout_0_Reference_1,
            grdExScheduleList_Layout_0_Reference_2,
            grdExScheduleList_Layout_0_Reference_3,
            grdExScheduleList_Layout_0_Reference_4});
            grdExScheduleList_Layout_0.LayoutString = resources.GetString("grdExScheduleList_Layout_0.LayoutString");
            this.grdExScheduleList.Layouts.AddRange(new Janus.Windows.GridEX.GridEXLayout[] {
            grdExScheduleList_Layout_0});
            this.grdExScheduleList.Location = new System.Drawing.Point(0, 0);
            this.grdExScheduleList.Name = "grdExScheduleList";
            this.grdExScheduleList.ScrollBars = Janus.Windows.GridEX.ScrollBars.Vertical;
            this.grdExScheduleList.SelectedInactiveFormatStyle.BackColor = System.Drawing.Color.Gold;
            this.grdExScheduleList.Size = new System.Drawing.Size(1008, 183);
            this.grdExScheduleList.TabIndex = 15;
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
            // uiPanelDetail
            // 
            this.uiPanelDetail.InnerContainer = this.uiPanelDetailContainer;
            this.uiPanelDetail.Location = new System.Drawing.Point(0, 304);
            this.uiPanelDetail.Name = "uiPanelDetail";
            this.uiPanelDetail.Size = new System.Drawing.Size(1010, 373);
            this.uiPanelDetail.TabIndex = 4;
            this.uiPanelDetail.Text = "홈타겟팅설정";
            // 
            // uiPanelDetailContainer
            // 
            this.uiPanelDetailContainer.Controls.Add(this.panel3);
            this.uiPanelDetailContainer.Location = new System.Drawing.Point(1, 23);
            this.uiPanelDetailContainer.Name = "uiPanelDetailContainer";
            this.uiPanelDetailContainer.Size = new System.Drawing.Size(1008, 349);
            this.uiPanelDetailContainer.TabIndex = 0;
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.WhiteSmoke;
            this.panel3.Controls.Add(this.uiTab1);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(0, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(1008, 349);
            this.panel3.TabIndex = 25;
            // 
            // uiTab1
            // 
            this.uiTab1.Location = new System.Drawing.Point(9, 3);
            this.uiTab1.Name = "uiTab1";
            this.uiTab1.Size = new System.Drawing.Size(991, 343);
            this.uiTab1.TabIndex = 209;
            this.uiTab1.TabPages.AddRange(new Janus.Windows.UI.Tab.UITabPage[] {
            this.uiTabPage1,
            this.uiTabPage2});
            this.uiTab1.TabStripAlignment = Janus.Windows.UI.Tab.TabStripAlignment.Left;
            this.uiTab1.VisualStyle = Janus.Windows.UI.Tab.TabVisualStyle.Office2007;
            // 
            // uiTabPage1
            // 
            this.uiTabPage1.Controls.Add(this.uiGroupBox2);
            this.uiTabPage1.Controls.Add(this.chkPoc);
            this.uiTabPage1.Controls.Add(this.grpStb);
            this.uiTabPage1.Controls.Add(this.chkStbModel);
            this.uiTabPage1.Controls.Add(this.tlvRegion);
            this.uiTabPage1.Controls.Add(this.lblCollMsg);
            this.uiTabPage1.Controls.Add(this.label9);
            this.uiTabPage1.Controls.Add(this.label58);
            this.uiTabPage1.Controls.Add(this.rbControlYn_Y);
            this.uiTabPage1.Controls.Add(this.chkCollectionYn);
            this.uiTabPage1.Controls.Add(this.rbControlYn_N);
            this.uiTabPage1.Controls.Add(this.chkWeekYn);
            this.uiTabPage1.Controls.Add(this.uiGroupBox1);
            this.uiTabPage1.Controls.Add(this.chkSexYn);
            this.uiTabPage1.Controls.Add(this.grpSex);
            this.uiTabPage1.Controls.Add(this.chkAgeBtnYn);
            this.uiTabPage1.Controls.Add(this.udAgeBtnBegin);
            this.uiTabPage1.Controls.Add(this.label4);
            this.uiTabPage1.Controls.Add(this.udAgeBtnEnd);
            this.uiTabPage1.Controls.Add(this.label5);
            this.uiTabPage1.Controls.Add(this.tvAge);
            this.uiTabPage1.Controls.Add(this.chkAgeYn);
            this.uiTabPage1.Controls.Add(this.chkRegionYn);
            this.uiTabPage1.Controls.Add(this.lbNotice);
            this.uiTabPage1.Controls.Add(this.udControlRate);
            this.uiTabPage1.Controls.Add(this.ebContractAmt);
            this.uiTabPage1.Controls.Add(this.label3);
            this.uiTabPage1.Controls.Add(this.lbUserID);
            this.uiTabPage1.Controls.Add(this.cbPriorityCd);
            this.uiTabPage1.Controls.Add(this.btnSave);
            this.uiTabPage1.Controls.Add(this.label2);
            this.uiTabPage1.Controls.Add(this.label6);
            this.uiTabPage1.Controls.Add(this.label7);
            this.uiTabPage1.Controls.Add(this.chkTimeYn);
            this.uiTabPage1.Controls.Add(this.grpTime);
            this.uiTabPage1.Location = new System.Drawing.Point(22, 1);
            this.uiTabPage1.Name = "uiTabPage1";
            this.uiTabPage1.Size = new System.Drawing.Size(968, 341);
            this.uiTabPage1.TabStop = true;
            this.uiTabPage1.Text = "타겟팅정보";
            // 
            // uiGroupBox2
            // 
            this.uiGroupBox2.BackColor = System.Drawing.Color.Transparent;
            this.uiGroupBox2.BorderColor = System.Drawing.Color.DimGray;
            this.uiGroupBox2.Controls.Add(this.tvPoc);
            this.uiGroupBox2.Location = new System.Drawing.Point(551, 314);
            this.uiGroupBox2.Name = "uiGroupBox2";
            this.uiGroupBox2.Size = new System.Drawing.Size(128, 179);
            this.uiGroupBox2.TabIndex = 258;
            this.uiGroupBox2.Visible = false;
            this.uiGroupBox2.VisualStyle = Janus.Windows.UI.Dock.PanelVisualStyle.Office2007;
            // 
            // tvPoc
            // 
            this.tvPoc.BackColor = System.Drawing.Color.WhiteSmoke;
            this.tvPoc.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tvPoc.CheckBoxes = true;
            this.tvPoc.FullRowSelect = true;
            this.tvPoc.HideSelection = false;
            this.tvPoc.ItemHeight = 16;
            this.tvPoc.Location = new System.Drawing.Point(7, 15);
            this.tvPoc.Name = "tvPoc";
            treeNode1.Name = "";
            treeNode1.Text = "POC 구분";
            this.tvPoc.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode1});
            this.tvPoc.ShowRootLines = false;
            this.tvPoc.Size = new System.Drawing.Size(115, 158);
            this.tvPoc.TabIndex = 203;
            // 
            // chkPoc
            // 
            this.chkPoc.BackColor = System.Drawing.Color.Transparent;
            this.chkPoc.Location = new System.Drawing.Point(564, 297);
            this.chkPoc.Name = "chkPoc";
            this.chkPoc.Size = new System.Drawing.Size(88, 20);
            this.chkPoc.TabIndex = 257;
            this.chkPoc.Text = "POC 구분";
            this.chkPoc.Visible = false;
            this.chkPoc.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.chkPoc.CheckedChanged += new System.EventHandler(this.chkPoc_CheckedChanged);
            // 
            // grpStb
            // 
            this.grpStb.BackColor = System.Drawing.Color.Transparent;
            this.grpStb.BorderColor = System.Drawing.Color.DimGray;
            this.grpStb.Controls.Add(this.tvStb);
            this.grpStb.Location = new System.Drawing.Point(469, 65);
            this.grpStb.Name = "grpStb";
            this.grpStb.Size = new System.Drawing.Size(128, 252);
            this.grpStb.TabIndex = 256;
            this.grpStb.VisualStyle = Janus.Windows.UI.Dock.PanelVisualStyle.Office2007;
            // 
            // tvStb
            // 
            this.tvStb.BackColor = System.Drawing.Color.WhiteSmoke;
            this.tvStb.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tvStb.CheckBoxes = true;
            this.tvStb.FullRowSelect = true;
            this.tvStb.HideSelection = false;
            this.tvStb.ItemHeight = 16;
            this.tvStb.Location = new System.Drawing.Point(6, 15);
            this.tvStb.Name = "tvStb";
            treeNode2.Name = "";
            treeNode2.Text = "셋탑모델별";
            this.tvStb.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode2});
            this.tvStb.ShowRootLines = false;
            this.tvStb.Size = new System.Drawing.Size(116, 228);
            this.tvStb.TabIndex = 203;
            this.tvStb.AfterCheck += new System.Windows.Forms.TreeViewEventHandler(this.tvStb_AfterCheck);
            // 
            // chkStbModel
            // 
            this.chkStbModel.BackColor = System.Drawing.Color.Transparent;
            this.chkStbModel.Location = new System.Drawing.Point(479, 48);
            this.chkStbModel.Name = "chkStbModel";
            this.chkStbModel.Size = new System.Drawing.Size(88, 20);
            this.chkStbModel.TabIndex = 255;
            this.chkStbModel.Text = "셋탑모델";
            this.chkStbModel.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.chkStbModel.CheckedChanged += new System.EventHandler(this.chkStbModel_CheckedChanged);
            // 
            // tlvRegion
            // 
            this.tlvRegion.BackColor = System.Drawing.Color.WhiteSmoke;
            this.tlvRegion.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tlvRegion.CheckBoxes = System.Windows.Forms.CheckBoxesTypes.Recursive;
            this.tlvRegion.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1});
            treeListViewItemCollectionComparer1.Column = 0;
            treeListViewItemCollectionComparer1.SortOrder = System.Windows.Forms.SortOrder.None;
            this.tlvRegion.Comparer = treeListViewItemCollectionComparer1;
            this.tlvRegion.Location = new System.Drawing.Point(605, 70);
            this.tlvRegion.Name = "tlvRegion";
            this.tlvRegion.Size = new System.Drawing.Size(153, 247);
            this.tlvRegion.SmallImageList = this.imgRegion;
            this.tlvRegion.Sorting = System.Windows.Forms.SortOrder.None;
            this.tlvRegion.TabIndex = 249;
            this.tlvRegion.UseCompatibleStateImageBehavior = false;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "지역정보";
            this.columnHeader1.Width = 183;
            // 
            // imgRegion
            // 
            this.imgRegion.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imgRegion.ImageStream")));
            this.imgRegion.TransparentColor = System.Drawing.Color.Transparent;
            this.imgRegion.Images.SetKeyName(0, "");
            this.imgRegion.Images.SetKeyName(1, "");
            this.imgRegion.Images.SetKeyName(2, "");
            // 
            // lblCollMsg
            // 
            this.lblCollMsg.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblCollMsg.BackColor = System.Drawing.Color.Red;
            this.lblCollMsg.ForeColor = System.Drawing.Color.White;
            this.lblCollMsg.Location = new System.Drawing.Point(26, 279);
            this.lblCollMsg.Name = "lblCollMsg";
            this.lblCollMsg.Size = new System.Drawing.Size(435, 17);
            this.lblCollMsg.TabIndex = 248;
            this.lblCollMsg.Text = "고객군 설정안내";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.BackColor = System.Drawing.Color.Transparent;
            this.label9.Location = new System.Drawing.Point(123, 255);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(338, 14);
            this.label9.TabIndex = 247;
            this.label9.Text = "(선택시 고객군타겟팅설정 탭에서 타겟고객군을 설정하여 주십시오)";
            // 
            // label58
            // 
            this.label58.BackColor = System.Drawing.Color.Gray;
            this.label58.Location = new System.Drawing.Point(8, 43);
            this.label58.Name = "label58";
            this.label58.Size = new System.Drawing.Size(946, 1);
            this.label58.TabIndex = 239;
            // 
            // rbControlYn_Y
            // 
            this.rbControlYn_Y.BackColor = System.Drawing.Color.Transparent;
            this.rbControlYn_Y.Checked = true;
            this.rbControlYn_Y.Location = new System.Drawing.Point(285, 11);
            this.rbControlYn_Y.Name = "rbControlYn_Y";
            this.rbControlYn_Y.Size = new System.Drawing.Size(64, 21);
            this.rbControlYn_Y.TabIndex = 213;
            this.rbControlYn_Y.TabStop = true;
            this.rbControlYn_Y.Text = "제어함";
            this.rbControlYn_Y.Visible = false;
            this.rbControlYn_Y.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            // 
            // chkCollectionYn
            // 
            this.chkCollectionYn.BackColor = System.Drawing.Color.Transparent;
            this.chkCollectionYn.Location = new System.Drawing.Point(11, 252);
            this.chkCollectionYn.Name = "chkCollectionYn";
            this.chkCollectionYn.Size = new System.Drawing.Size(121, 21);
            this.chkCollectionYn.TabIndex = 238;
            this.chkCollectionYn.Text = "고객군타겟팅사용";
            this.chkCollectionYn.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.chkCollectionYn.CheckedChanged += new System.EventHandler(this.chkCollectionYn_CheckedChanged);
            // 
            // rbControlYn_N
            // 
            this.rbControlYn_N.BackColor = System.Drawing.Color.Transparent;
            this.rbControlYn_N.Location = new System.Drawing.Point(349, 11);
            this.rbControlYn_N.Name = "rbControlYn_N";
            this.rbControlYn_N.Size = new System.Drawing.Size(72, 21);
            this.rbControlYn_N.TabIndex = 214;
            this.rbControlYn_N.Text = "제어안함";
            this.rbControlYn_N.Visible = false;
            this.rbControlYn_N.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            // 
            // chkWeekYn
            // 
            this.chkWeekYn.BackColor = System.Drawing.Color.Transparent;
            this.chkWeekYn.Location = new System.Drawing.Point(766, 47);
            this.chkWeekYn.Name = "chkWeekYn";
            this.chkWeekYn.Size = new System.Drawing.Size(60, 21);
            this.chkWeekYn.TabIndex = 235;
            this.chkWeekYn.Text = "요일별";
            this.chkWeekYn.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.chkWeekYn.CheckedChanged += new System.EventHandler(this.chkWeekYn_CheckedChanged);
            // 
            // uiGroupBox1
            // 
            this.uiGroupBox1.BackColor = System.Drawing.Color.Transparent;
            this.uiGroupBox1.BorderColor = System.Drawing.Color.Black;
            this.uiGroupBox1.Controls.Add(this.chkSun);
            this.uiGroupBox1.Controls.Add(this.chkSat);
            this.uiGroupBox1.Controls.Add(this.chkFri);
            this.uiGroupBox1.Controls.Add(this.chkThe);
            this.uiGroupBox1.Controls.Add(this.chkWed);
            this.uiGroupBox1.Controls.Add(this.chkMon);
            this.uiGroupBox1.Controls.Add(this.chkThu);
            this.uiGroupBox1.Location = new System.Drawing.Point(766, 63);
            this.uiGroupBox1.Name = "uiGroupBox1";
            this.uiGroupBox1.Size = new System.Drawing.Size(64, 256);
            this.uiGroupBox1.TabIndex = 236;
            this.uiGroupBox1.VisualStyle = Janus.Windows.UI.Dock.PanelVisualStyle.Office2007;
            // 
            // chkSun
            // 
            this.chkSun.ForeColor = System.Drawing.Color.Red;
            this.chkSun.Location = new System.Drawing.Point(8, 160);
            this.chkSun.Name = "chkSun";
            this.chkSun.Size = new System.Drawing.Size(32, 21);
            this.chkSun.TabIndex = 30;
            this.chkSun.Text = "일";
            this.chkSun.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            // 
            // chkSat
            // 
            this.chkSat.ForeColor = System.Drawing.Color.Blue;
            this.chkSat.Location = new System.Drawing.Point(8, 136);
            this.chkSat.Name = "chkSat";
            this.chkSat.Size = new System.Drawing.Size(32, 21);
            this.chkSat.TabIndex = 29;
            this.chkSat.Text = "토";
            this.chkSat.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            // 
            // chkFri
            // 
            this.chkFri.Location = new System.Drawing.Point(8, 112);
            this.chkFri.Name = "chkFri";
            this.chkFri.Size = new System.Drawing.Size(32, 21);
            this.chkFri.TabIndex = 28;
            this.chkFri.Text = "금";
            this.chkFri.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            // 
            // chkThe
            // 
            this.chkThe.Location = new System.Drawing.Point(8, 88);
            this.chkThe.Name = "chkThe";
            this.chkThe.Size = new System.Drawing.Size(32, 21);
            this.chkThe.TabIndex = 27;
            this.chkThe.Text = "목";
            this.chkThe.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            // 
            // chkWed
            // 
            this.chkWed.Location = new System.Drawing.Point(8, 64);
            this.chkWed.Name = "chkWed";
            this.chkWed.Size = new System.Drawing.Size(32, 21);
            this.chkWed.TabIndex = 26;
            this.chkWed.Text = "수";
            this.chkWed.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            // 
            // chkMon
            // 
            this.chkMon.Location = new System.Drawing.Point(8, 16);
            this.chkMon.Name = "chkMon";
            this.chkMon.Size = new System.Drawing.Size(32, 21);
            this.chkMon.TabIndex = 24;
            this.chkMon.Text = "월";
            this.chkMon.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            // 
            // chkThu
            // 
            this.chkThu.Location = new System.Drawing.Point(8, 40);
            this.chkThu.Name = "chkThu";
            this.chkThu.Size = new System.Drawing.Size(32, 21);
            this.chkThu.TabIndex = 25;
            this.chkThu.Text = "화";
            this.chkThu.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            // 
            // chkSexYn
            // 
            this.chkSexYn.BackColor = System.Drawing.Color.Transparent;
            this.chkSexYn.Location = new System.Drawing.Point(840, 47);
            this.chkSexYn.Name = "chkSexYn";
            this.chkSexYn.Size = new System.Drawing.Size(104, 21);
            this.chkSexYn.TabIndex = 233;
            this.chkSexYn.Text = "노출성별";
            this.chkSexYn.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.chkSexYn.CheckedChanged += new System.EventHandler(this.chkSexYn_CheckedChanged);
            // 
            // grpSex
            // 
            this.grpSex.BackColor = System.Drawing.Color.Transparent;
            this.grpSex.BorderColor = System.Drawing.Color.Black;
            this.grpSex.Controls.Add(this.chkSexMan);
            this.grpSex.Controls.Add(this.chkSexWoman);
            this.grpSex.Location = new System.Drawing.Point(840, 63);
            this.grpSex.Name = "grpSex";
            this.grpSex.Size = new System.Drawing.Size(121, 69);
            this.grpSex.TabIndex = 234;
            this.grpSex.VisualStyle = Janus.Windows.UI.Dock.PanelVisualStyle.Office2007;
            // 
            // chkSexMan
            // 
            this.chkSexMan.Location = new System.Drawing.Point(8, 16);
            this.chkSexMan.Name = "chkSexMan";
            this.chkSexMan.Size = new System.Drawing.Size(64, 21);
            this.chkSexMan.TabIndex = 24;
            this.chkSexMan.Text = "남자";
            this.chkSexMan.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            // 
            // chkSexWoman
            // 
            this.chkSexWoman.Location = new System.Drawing.Point(8, 40);
            this.chkSexWoman.Name = "chkSexWoman";
            this.chkSexWoman.Size = new System.Drawing.Size(64, 21);
            this.chkSexWoman.TabIndex = 25;
            this.chkSexWoman.Text = "여자";
            this.chkSexWoman.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            // 
            // chkAgeBtnYn
            // 
            this.chkAgeBtnYn.BackColor = System.Drawing.Color.Transparent;
            this.chkAgeBtnYn.Location = new System.Drawing.Point(12, 209);
            this.chkAgeBtnYn.Name = "chkAgeBtnYn";
            this.chkAgeBtnYn.Size = new System.Drawing.Size(104, 21);
            this.chkAgeBtnYn.TabIndex = 228;
            this.chkAgeBtnYn.Text = "노출연령구간";
            this.chkAgeBtnYn.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.chkAgeBtnYn.CheckedChanged += new System.EventHandler(this.chkAgeBtnYn_CheckedChanged);
            // 
            // udAgeBtnBegin
            // 
            this.udAgeBtnBegin.Location = new System.Drawing.Point(122, 209);
            this.udAgeBtnBegin.Maximum = 150;
            this.udAgeBtnBegin.MaxLength = 3;
            this.udAgeBtnBegin.Name = "udAgeBtnBegin";
            this.udAgeBtnBegin.Size = new System.Drawing.Size(64, 21);
            this.udAgeBtnBegin.TabIndex = 229;
            this.udAgeBtnBegin.TextAlignment = Janus.Windows.GridEX.TextAlignment.Far;
            this.udAgeBtnBegin.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            // 
            // label4
            // 
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.Location = new System.Drawing.Point(186, 209);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(56, 21);
            this.label4.TabIndex = 232;
            this.label4.Text = "세부터";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // udAgeBtnEnd
            // 
            this.udAgeBtnEnd.Location = new System.Drawing.Point(242, 209);
            this.udAgeBtnEnd.Maximum = 150;
            this.udAgeBtnEnd.MaxLength = 3;
            this.udAgeBtnEnd.Name = "udAgeBtnEnd";
            this.udAgeBtnEnd.Size = new System.Drawing.Size(64, 21);
            this.udAgeBtnEnd.TabIndex = 230;
            this.udAgeBtnEnd.TextAlignment = Janus.Windows.GridEX.TextAlignment.Far;
            this.udAgeBtnEnd.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            // 
            // label5
            // 
            this.label5.BackColor = System.Drawing.Color.Transparent;
            this.label5.Location = new System.Drawing.Point(306, 209);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(56, 21);
            this.label5.TabIndex = 231;
            this.label5.Text = "세까지";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tvAge
            // 
            this.tvAge.BackColor = System.Drawing.Color.White;
            this.tvAge.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tvAge.CheckBoxes = true;
            this.tvAge.FullRowSelect = true;
            this.tvAge.HideSelection = false;
            this.tvAge.ItemHeight = 16;
            this.tvAge.Location = new System.Drawing.Point(840, 191);
            this.tvAge.Name = "tvAge";
            treeNode3.Name = "";
            treeNode3.Text = "0~19세";
            treeNode4.Name = "";
            treeNode4.Text = "20~29세";
            treeNode5.Name = "";
            treeNode5.Text = "30~39세";
            treeNode6.Name = "";
            treeNode6.Text = "40~49세";
            treeNode7.Name = "";
            treeNode7.Text = "50~59세";
            treeNode8.Name = "";
            treeNode8.Text = "60세이상";
            treeNode9.Name = "";
            treeNode9.Text = "전체연령대";
            this.tvAge.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode9});
            this.tvAge.Scrollable = false;
            this.tvAge.Size = new System.Drawing.Size(121, 128);
            this.tvAge.TabIndex = 227;
            this.tvAge.AfterCheck += new System.Windows.Forms.TreeViewEventHandler(this.tvAge_AfterCheck);
            // 
            // chkAgeYn
            // 
            this.chkAgeYn.BackColor = System.Drawing.Color.Transparent;
            this.chkAgeYn.Location = new System.Drawing.Point(840, 167);
            this.chkAgeYn.Name = "chkAgeYn";
            this.chkAgeYn.Size = new System.Drawing.Size(88, 21);
            this.chkAgeYn.TabIndex = 226;
            this.chkAgeYn.Text = "노출연령대";
            this.chkAgeYn.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.chkAgeYn.CheckedChanged += new System.EventHandler(this.chkAgeYn_CheckedChanged);
            // 
            // chkRegionYn
            // 
            this.chkRegionYn.BackColor = System.Drawing.Color.Transparent;
            this.chkRegionYn.Location = new System.Drawing.Point(605, 47);
            this.chkRegionYn.Name = "chkRegionYn";
            this.chkRegionYn.Size = new System.Drawing.Size(80, 21);
            this.chkRegionYn.TabIndex = 224;
            this.chkRegionYn.Text = "노출지역";
            this.chkRegionYn.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.chkRegionYn.CheckedChanged += new System.EventHandler(this.chkRegionYn_CheckedChanged);
            // 
            // lbNotice
            // 
            this.lbNotice.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lbNotice.ForeColor = System.Drawing.Color.Red;
            this.lbNotice.Location = new System.Drawing.Point(10, 305);
            this.lbNotice.Name = "lbNotice";
            this.lbNotice.Size = new System.Drawing.Size(451, 14);
            this.lbNotice.TabIndex = 223;
            this.lbNotice.Text = "설정정보가 저장되지 않았습니다. 설정을 완료하지 않으면 타겟팅이 되지 않습니다.";
            // 
            // udControlRate
            // 
            this.udControlRate.Location = new System.Drawing.Point(755, 10);
            this.udControlRate.Maximum = 500;
            this.udControlRate.MaxLength = 3;
            this.udControlRate.Minimum = 20;
            this.udControlRate.Name = "udControlRate";
            this.udControlRate.Size = new System.Drawing.Size(56, 21);
            this.udControlRate.TabIndex = 215;
            this.udControlRate.TextAlignment = Janus.Windows.GridEX.TextAlignment.Far;
            this.udControlRate.Value = 100;
            this.udControlRate.Visible = false;
            this.udControlRate.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            // 
            // ebContractAmt
            // 
            this.ebContractAmt.DecimalDigits = 0;
            this.ebContractAmt.FormatString = "#,##0";
            this.ebContractAmt.Location = new System.Drawing.Point(94, 10);
            this.ebContractAmt.Name = "ebContractAmt";
            this.ebContractAmt.Size = new System.Drawing.Size(104, 21);
            this.ebContractAmt.TabIndex = 211;
            this.ebContractAmt.Text = "0";
            this.ebContractAmt.TextAlignment = Janus.Windows.GridEX.TextAlignment.Far;
            this.ebContractAmt.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.ebContractAmt.Visible = false;
            this.ebContractAmt.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            this.ebContractAmt.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ebContractAmt_KeyDown);
            // 
            // label3
            // 
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Location = new System.Drawing.Point(445, 11);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(68, 21);
            this.label3.TabIndex = 221;
            this.label3.Text = "노출빈도값";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label3.Visible = false;
            // 
            // lbUserID
            // 
            this.lbUserID.BackColor = System.Drawing.Color.Transparent;
            this.lbUserID.Location = new System.Drawing.Point(9, 11);
            this.lbUserID.Name = "lbUserID";
            this.lbUserID.Size = new System.Drawing.Size(85, 21);
            this.lbUserID.TabIndex = 217;
            this.lbUserID.Text = "노출계약물량";
            this.lbUserID.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lbUserID.Visible = false;
            // 
            // cbPriorityCd
            // 
            this.cbPriorityCd.BackColor = System.Drawing.Color.White;
            this.cbPriorityCd.ComboStyle = Janus.Windows.EditControls.ComboStyle.DropDownList;
            this.cbPriorityCd.Location = new System.Drawing.Point(519, 10);
            this.cbPriorityCd.Name = "cbPriorityCd";
            this.cbPriorityCd.Size = new System.Drawing.Size(104, 21);
            this.cbPriorityCd.TabIndex = 212;
            this.cbPriorityCd.Text = "노출빈도값";
            this.cbPriorityCd.Visible = false;
            this.cbPriorityCd.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnSave.BackColor = System.Drawing.SystemColors.Window;
            this.btnSave.Enabled = false;
            this.btnSave.Location = new System.Drawing.Point(850, 12);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(104, 24);
            this.btnSave.TabIndex = 216;
            this.btnSave.Text = "저 장";
            this.btnSave.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Location = new System.Drawing.Point(645, 11);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(104, 21);
            this.label2.TabIndex = 220;
            this.label2.Text = "노출물량제어비율";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label2.Visible = false;
            // 
            // label6
            // 
            this.label6.BackColor = System.Drawing.Color.Transparent;
            this.label6.Location = new System.Drawing.Point(817, 11);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(16, 21);
            this.label6.TabIndex = 218;
            this.label6.Text = "%";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label6.Visible = false;
            // 
            // label7
            // 
            this.label7.BackColor = System.Drawing.Color.Transparent;
            this.label7.Location = new System.Drawing.Point(223, 11);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(56, 21);
            this.label7.TabIndex = 219;
            this.label7.Text = "노출제어";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label7.Visible = false;
            // 
            // chkTimeYn
            // 
            this.chkTimeYn.BackColor = System.Drawing.Color.Transparent;
            this.chkTimeYn.Location = new System.Drawing.Point(17, 47);
            this.chkTimeYn.Name = "chkTimeYn";
            this.chkTimeYn.Size = new System.Drawing.Size(80, 21);
            this.chkTimeYn.TabIndex = 209;
            this.chkTimeYn.Text = "시간대별";
            this.chkTimeYn.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.chkTimeYn.CheckedChanged += new System.EventHandler(this.chkTimeYn_CheckedChanged);
            // 
            // grpTime
            // 
            this.grpTime.BackColor = System.Drawing.Color.Transparent;
            this.grpTime.BorderColor = System.Drawing.Color.Black;
            this.grpTime.Controls.Add(this.chkTime23);
            this.grpTime.Controls.Add(this.chkTime22);
            this.grpTime.Controls.Add(this.chkTime21);
            this.grpTime.Controls.Add(this.chkTime20);
            this.grpTime.Controls.Add(this.chkTime19);
            this.grpTime.Controls.Add(this.chkTime18);
            this.grpTime.Controls.Add(this.chkTime17);
            this.grpTime.Controls.Add(this.chkTime16);
            this.grpTime.Controls.Add(this.chkTime15);
            this.grpTime.Controls.Add(this.chkTime14);
            this.grpTime.Controls.Add(this.chkTime13);
            this.grpTime.Controls.Add(this.chkTime12);
            this.grpTime.Controls.Add(this.chkTime11);
            this.grpTime.Controls.Add(this.chkTime10);
            this.grpTime.Controls.Add(this.chkTime09);
            this.grpTime.Controls.Add(this.chkTime08);
            this.grpTime.Controls.Add(this.chkTime07);
            this.grpTime.Controls.Add(this.chkTime06);
            this.grpTime.Controls.Add(this.chkTime05);
            this.grpTime.Controls.Add(this.chkTime04);
            this.grpTime.Controls.Add(this.chkTime03);
            this.grpTime.Controls.Add(this.chkTime02);
            this.grpTime.Controls.Add(this.chkTime00);
            this.grpTime.Controls.Add(this.chkTime01);
            this.grpTime.Location = new System.Drawing.Point(11, 65);
            this.grpTime.Name = "grpTime";
            this.grpTime.Size = new System.Drawing.Size(450, 123);
            this.grpTime.TabIndex = 210;
            this.grpTime.VisualStyle = Janus.Windows.UI.Dock.PanelVisualStyle.Office2007;
            // 
            // chkTime23
            // 
            this.chkTime23.Location = new System.Drawing.Point(399, 86);
            this.chkTime23.Name = "chkTime23";
            this.chkTime23.Size = new System.Drawing.Size(48, 21);
            this.chkTime23.TabIndex = 40;
            this.chkTime23.Text = "23시";
            this.chkTime23.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            // 
            // chkTime22
            // 
            this.chkTime22.Location = new System.Drawing.Point(343, 86);
            this.chkTime22.Name = "chkTime22";
            this.chkTime22.Size = new System.Drawing.Size(48, 21);
            this.chkTime22.TabIndex = 39;
            this.chkTime22.Text = "22시";
            this.chkTime22.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            // 
            // chkTime21
            // 
            this.chkTime21.Location = new System.Drawing.Point(287, 86);
            this.chkTime21.Name = "chkTime21";
            this.chkTime21.Size = new System.Drawing.Size(48, 21);
            this.chkTime21.TabIndex = 38;
            this.chkTime21.Text = "21시";
            this.chkTime21.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            // 
            // chkTime20
            // 
            this.chkTime20.Location = new System.Drawing.Point(231, 86);
            this.chkTime20.Name = "chkTime20";
            this.chkTime20.Size = new System.Drawing.Size(48, 21);
            this.chkTime20.TabIndex = 37;
            this.chkTime20.Text = "20시";
            this.chkTime20.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            // 
            // chkTime19
            // 
            this.chkTime19.Location = new System.Drawing.Point(175, 86);
            this.chkTime19.Name = "chkTime19";
            this.chkTime19.Size = new System.Drawing.Size(48, 21);
            this.chkTime19.TabIndex = 36;
            this.chkTime19.Text = "19시";
            this.chkTime19.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            // 
            // chkTime18
            // 
            this.chkTime18.Location = new System.Drawing.Point(119, 86);
            this.chkTime18.Name = "chkTime18";
            this.chkTime18.Size = new System.Drawing.Size(48, 21);
            this.chkTime18.TabIndex = 35;
            this.chkTime18.Text = "18시";
            this.chkTime18.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            // 
            // chkTime17
            // 
            this.chkTime17.Location = new System.Drawing.Point(62, 86);
            this.chkTime17.Name = "chkTime17";
            this.chkTime17.Size = new System.Drawing.Size(48, 21);
            this.chkTime17.TabIndex = 34;
            this.chkTime17.Text = "17시";
            this.chkTime17.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            // 
            // chkTime16
            // 
            this.chkTime16.Location = new System.Drawing.Point(6, 86);
            this.chkTime16.Name = "chkTime16";
            this.chkTime16.Size = new System.Drawing.Size(48, 21);
            this.chkTime16.TabIndex = 33;
            this.chkTime16.Text = "16시";
            this.chkTime16.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            // 
            // chkTime15
            // 
            this.chkTime15.Location = new System.Drawing.Point(399, 52);
            this.chkTime15.Name = "chkTime15";
            this.chkTime15.Size = new System.Drawing.Size(48, 21);
            this.chkTime15.TabIndex = 32;
            this.chkTime15.Text = "15시";
            this.chkTime15.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            // 
            // chkTime14
            // 
            this.chkTime14.Location = new System.Drawing.Point(342, 52);
            this.chkTime14.Name = "chkTime14";
            this.chkTime14.Size = new System.Drawing.Size(48, 21);
            this.chkTime14.TabIndex = 31;
            this.chkTime14.Text = "14시";
            this.chkTime14.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            // 
            // chkTime13
            // 
            this.chkTime13.Location = new System.Drawing.Point(287, 52);
            this.chkTime13.Name = "chkTime13";
            this.chkTime13.Size = new System.Drawing.Size(48, 21);
            this.chkTime13.TabIndex = 30;
            this.chkTime13.Text = "13시";
            this.chkTime13.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            // 
            // chkTime12
            // 
            this.chkTime12.Location = new System.Drawing.Point(231, 52);
            this.chkTime12.Name = "chkTime12";
            this.chkTime12.Size = new System.Drawing.Size(48, 21);
            this.chkTime12.TabIndex = 29;
            this.chkTime12.Text = "12시";
            this.chkTime12.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            // 
            // chkTime11
            // 
            this.chkTime11.Location = new System.Drawing.Point(175, 52);
            this.chkTime11.Name = "chkTime11";
            this.chkTime11.Size = new System.Drawing.Size(48, 21);
            this.chkTime11.TabIndex = 28;
            this.chkTime11.Text = "11시";
            this.chkTime11.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            // 
            // chkTime10
            // 
            this.chkTime10.Location = new System.Drawing.Point(119, 52);
            this.chkTime10.Name = "chkTime10";
            this.chkTime10.Size = new System.Drawing.Size(48, 21);
            this.chkTime10.TabIndex = 27;
            this.chkTime10.Text = "10시";
            this.chkTime10.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            // 
            // chkTime09
            // 
            this.chkTime09.Location = new System.Drawing.Point(63, 52);
            this.chkTime09.Name = "chkTime09";
            this.chkTime09.Size = new System.Drawing.Size(48, 21);
            this.chkTime09.TabIndex = 26;
            this.chkTime09.Text = "09시";
            this.chkTime09.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            // 
            // chkTime08
            // 
            this.chkTime08.Location = new System.Drawing.Point(6, 52);
            this.chkTime08.Name = "chkTime08";
            this.chkTime08.Size = new System.Drawing.Size(48, 21);
            this.chkTime08.TabIndex = 25;
            this.chkTime08.Text = "08시";
            this.chkTime08.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            // 
            // chkTime07
            // 
            this.chkTime07.Location = new System.Drawing.Point(399, 19);
            this.chkTime07.Name = "chkTime07";
            this.chkTime07.Size = new System.Drawing.Size(48, 21);
            this.chkTime07.TabIndex = 24;
            this.chkTime07.Text = "07시";
            this.chkTime07.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            // 
            // chkTime06
            // 
            this.chkTime06.Location = new System.Drawing.Point(343, 19);
            this.chkTime06.Name = "chkTime06";
            this.chkTime06.Size = new System.Drawing.Size(48, 21);
            this.chkTime06.TabIndex = 23;
            this.chkTime06.Text = "06시";
            this.chkTime06.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            // 
            // chkTime05
            // 
            this.chkTime05.Location = new System.Drawing.Point(287, 19);
            this.chkTime05.Name = "chkTime05";
            this.chkTime05.Size = new System.Drawing.Size(48, 21);
            this.chkTime05.TabIndex = 22;
            this.chkTime05.Text = "05시";
            this.chkTime05.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            // 
            // chkTime04
            // 
            this.chkTime04.Location = new System.Drawing.Point(231, 19);
            this.chkTime04.Name = "chkTime04";
            this.chkTime04.Size = new System.Drawing.Size(48, 21);
            this.chkTime04.TabIndex = 21;
            this.chkTime04.Text = "04시";
            this.chkTime04.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            // 
            // chkTime03
            // 
            this.chkTime03.Location = new System.Drawing.Point(175, 19);
            this.chkTime03.Name = "chkTime03";
            this.chkTime03.Size = new System.Drawing.Size(48, 21);
            this.chkTime03.TabIndex = 20;
            this.chkTime03.Text = "03시";
            this.chkTime03.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            // 
            // chkTime02
            // 
            this.chkTime02.Location = new System.Drawing.Point(119, 19);
            this.chkTime02.Name = "chkTime02";
            this.chkTime02.Size = new System.Drawing.Size(48, 21);
            this.chkTime02.TabIndex = 19;
            this.chkTime02.Text = "02시";
            this.chkTime02.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            // 
            // chkTime00
            // 
            this.chkTime00.Location = new System.Drawing.Point(7, 19);
            this.chkTime00.Name = "chkTime00";
            this.chkTime00.Size = new System.Drawing.Size(48, 21);
            this.chkTime00.TabIndex = 17;
            this.chkTime00.Text = "00시";
            this.chkTime00.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            // 
            // chkTime01
            // 
            this.chkTime01.Location = new System.Drawing.Point(65, 19);
            this.chkTime01.Name = "chkTime01";
            this.chkTime01.Size = new System.Drawing.Size(48, 21);
            this.chkTime01.TabIndex = 18;
            this.chkTime01.Text = "01시";
            this.chkTime01.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            // 
            // uiTabPage2
            // 
            this.uiTabPage2.Controls.Add(this.label63);
            this.uiTabPage2.Controls.Add(this.label62);
            this.uiTabPage2.Controls.Add(this.panel4);
            this.uiTabPage2.Controls.Add(this.panel1);
            this.uiTabPage2.Location = new System.Drawing.Point(22, 1);
            this.uiTabPage2.Name = "uiTabPage2";
            this.uiTabPage2.Size = new System.Drawing.Size(968, 341);
            this.uiTabPage2.TabStop = true;
            this.uiTabPage2.Text = "고객군타겟팅";
            // 
            // label63
            // 
            this.label63.BackColor = System.Drawing.Color.Transparent;
            this.label63.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label63.Location = new System.Drawing.Point(398, 7);
            this.label63.Name = "label63";
            this.label63.Size = new System.Drawing.Size(103, 21);
            this.label63.TabIndex = 241;
            this.label63.Text = "대상고객군";
            this.label63.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label62
            // 
            this.label62.BackColor = System.Drawing.Color.Transparent;
            this.label62.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label62.Location = new System.Drawing.Point(18, 6);
            this.label62.Name = "label62";
            this.label62.Size = new System.Drawing.Size(103, 21);
            this.label62.TabIndex = 242;
            this.label62.Text = "설정된 고객군";
            this.label62.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.grdExSouceCollectionList);
            this.panel4.Controls.Add(this.panel6);
            this.panel4.Location = new System.Drawing.Point(398, 31);
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
            this.grdExSouceCollectionList.EmptyRows = true;
            this.grdExSouceCollectionList.FocusCellFormatStyle.BackColor = System.Drawing.SystemColors.Highlight;
            this.grdExSouceCollectionList.FocusStyle = Janus.Windows.GridEX.FocusStyle.None;
            this.grdExSouceCollectionList.Font = new System.Drawing.Font("나눔고딕", 8.249999F);
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
            this.dvCollection.Table = this.targetingHomeDs.Collections;
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
            this.btnAddCollectionMinus.Location = new System.Drawing.Point(115, 8);
            this.btnAddCollectionMinus.Name = "btnAddCollectionMinus";
            this.btnAddCollectionMinus.Size = new System.Drawing.Size(104, 24);
            this.btnAddCollectionMinus.TabIndex = 38;
            this.btnAddCollectionMinus.Text = "제외조건 추가";
            this.btnAddCollectionMinus.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.btnAddCollectionMinus.Click += new System.EventHandler(this.btnAddCollectionMinus_Click);
            // 
            // lbMsg2
            // 
            this.lbMsg2.Location = new System.Drawing.Point(225, 10);
            this.lbMsg2.Name = "lbMsg2";
            this.lbMsg2.Size = new System.Drawing.Size(309, 21);
            this.lbMsg2.TabIndex = 37;
            this.lbMsg2.Text = "선택된 대상고객군을 타켓팅설정에 추가합니다.";
            this.lbMsg2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnAddCollection
            // 
            this.btnAddCollection.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnAddCollection.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnAddCollection.Enabled = false;
            this.btnAddCollection.Location = new System.Drawing.Point(4, 8);
            this.btnAddCollection.Name = "btnAddCollection";
            this.btnAddCollection.Size = new System.Drawing.Size(104, 24);
            this.btnAddCollection.TabIndex = 17;
            this.btnAddCollection.Text = "포함조건 추가";
            this.btnAddCollection.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.btnAddCollection.Click += new System.EventHandler(this.btnAddCollection_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.grdExTargetList);
            this.panel1.Controls.Add(this.panel5);
            this.panel1.Location = new System.Drawing.Point(17, 30);
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
            this.grdExTargetList.Font = new System.Drawing.Font("나눔고딕", 8.249999F);
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
            this.dvTargetingCollection.Table = this.targetingHomeDs.TargetingCollection;
            // 
            // panel5
            // 
            this.panel5.BackColor = System.Drawing.SystemColors.Window;
            this.panel5.Controls.Add(this.label8);
            this.panel5.Controls.Add(this.btnDeleteCollection);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel5.Location = new System.Drawing.Point(0, 262);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(375, 40);
            this.panel5.TabIndex = 17;
            // 
            // label8
            // 
            this.label8.Location = new System.Drawing.Point(110, 9);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(262, 21);
            this.label8.TabIndex = 37;
            this.label8.Text = "선택된 고객군을 타켓팅설정에서 삭제합니다.";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnDeleteCollection
            // 
            this.btnDeleteCollection.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnDeleteCollection.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnDeleteCollection.Enabled = false;
            this.btnDeleteCollection.Location = new System.Drawing.Point(4, 7);
            this.btnDeleteCollection.Name = "btnDeleteCollection";
            this.btnDeleteCollection.Size = new System.Drawing.Size(104, 24);
            this.btnDeleteCollection.TabIndex = 17;
            this.btnDeleteCollection.Text = "삭 제";
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
            this.editBox1.Size = new System.Drawing.Size(100, 21);
            this.editBox1.TabIndex = 0;
            this.editBox1.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
            // 
            // TargetingHomeControl
            // 
            this.AutoValidate = System.Windows.Forms.AutoValidate.Disable;
            this.Controls.Add(this.uiPanelChoiceAdSchedule);
            this.Font = global::AdManagerClient.Properties.Settings.Default.Font1;
            this.Name = "TargetingHomeControl";
            this.Size = new System.Drawing.Size(1010, 677);
            this.Load += new System.EventHandler(this.TargetingHomeControl_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dvTargeting)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.targetingHomeDs)).EndInit();
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
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelDetail)).EndInit();
            this.uiPanelDetail.ResumeLayout(false);
            this.uiPanelDetailContainer.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.uiTab1)).EndInit();
            this.uiTab1.ResumeLayout(false);
            this.uiTabPage1.ResumeLayout(false);
            this.uiTabPage1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uiGroupBox2)).EndInit();
            this.uiGroupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grpStb)).EndInit();
            this.grpStb.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.uiGroupBox1)).EndInit();
            this.uiGroupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grpSex)).EndInit();
            this.grpSex.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grpTime)).EndInit();
            this.grpTime.ResumeLayout(false);
            this.uiTabPage2.ResumeLayout(false);
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

        #region 컨트롤 로드
        #endregion

        #region 컨트롤 초기화
        private void InitControl()
        {
            ProgressStart();
            InitCombo();

            if (menu.CanRead(MenuCode)) canRead = true;
            if (menu.CanUpdate(MenuCode)) canUpdate = true;

            InitButton();
            ProgressStop();

            if (canRead) SearchTargeting();
        }

        private void InitCombo()
        {
            Init_MediaCode();
            Init_RapCode();
            Init_AgencyCode();
            Init_AdvertiserCode();

            Init_ContractState();

            Init_Collection();

            InitCombo_Level();

            //노출우선순위등급코드 초기화
            Janus.Windows.EditControls.UIComboBoxItem[] comboItems = new Janus.Windows.EditControls.UIComboBoxItem[10];
            for (int i = 0; i < 10; i++)
            {
                comboItems[i] = new Janus.Windows.EditControls.UIComboBoxItem(i + 1, i);
            }
            // 콤보에 셋트
            this.cbPriorityCd.Items.AddRange(comboItems);
            this.cbPriorityCd.SelectedIndex = 4;


            Init_RegionData();	// 지역구분 데이터 초기화
            Init_AgeData();		// 연령대 데이터 초기화

            //Init_tvRegion();	// 지역구분 TreeView 초기화
            Init_tlvRegion();   // [추가]지역구분 TreeView 초기화
            Init_tvTime();		// 시간대 TreeView 초기화
            Init_tvAge();		// 연령대 TreeView 초기화

            Init_StbData();     // [E_01] 셋탑모델정보 초기화
            Init_tvStb();       // [E_01] 셋탑정보모델 TreeView 초기화
        }

        private void Init_MediaCode()
        {
            // 매체를 조회한다.
            MediaCodeModel mediacodeModel = new MediaCodeModel();
            new MediaCodeManager(systemModel, commonModel).GetMediaCodeList(mediacodeModel);

            if (mediacodeModel.ResultCD.Equals("0000"))
            {
                // 데이터셋에 셋팅
                Utility.SetDataTable(targetingHomeDs.Medias, mediacodeModel.MediaCodeDataSet);
            }

            // 검색조건의 콤보
            this.cbSearchMedia.Items.Clear();

            // 콤보박스에 셋트할 코드목록을 담을 Item배열을 선언
            Janus.Windows.EditControls.UIComboBoxItem[] comboItems = new Janus.Windows.EditControls.UIComboBoxItem[mediacodeModel.ResultCnt + 1];

            comboItems[0] = new Janus.Windows.EditControls.UIComboBoxItem("매체선택", "00");

            for (int i = 0; i < mediacodeModel.ResultCnt; i++)
            {
                DataRow row = targetingHomeDs.Medias.Rows[i];

                string val = row["MediaCode"].ToString();
                string txt = row["MediaName"].ToString();
                comboItems[i + 1] = new Janus.Windows.EditControls.UIComboBoxItem(txt, val);
            }
            // 콤보에 셋트
            this.cbSearchMedia.Items.AddRange(comboItems);
            this.cbSearchMedia.SelectedIndex = 0;

            Application.DoEvents();
        }

        private void Init_RapCode()
        {
            // 랩을 조회한다.
            MediaRapCodeModel mediarapcodeModel = new MediaRapCodeModel();
            new MediaRapCodeManager(systemModel, commonModel).GetMediaRapCodeList(mediarapcodeModel);

            if (mediarapcodeModel.ResultCD.Equals("0000"))
            {
                // 데이터셋에 셋팅
                Utility.SetDataTable(targetingHomeDs.MediaRaps, mediarapcodeModel.MediaRapCodeDataSet);
            }

            // 검색조건의 콤보
            this.cbSearchRap.Items.Clear();

            // 콤보박스에 셋트할 코드목록을 담을 Item배열을 선언
            Janus.Windows.EditControls.UIComboBoxItem[] comboItems = new Janus.Windows.EditControls.UIComboBoxItem[mediarapcodeModel.ResultCnt + 1];

            comboItems[0] = new Janus.Windows.EditControls.UIComboBoxItem("미디어렙선택", "00");

            for (int i = 0; i < mediarapcodeModel.ResultCnt; i++)
            {
                DataRow row = targetingHomeDs.MediaRaps.Rows[i];

                string val = row["RapCode"].ToString();
                string txt = row["RapName"].ToString();
                comboItems[i + 1] = new Janus.Windows.EditControls.UIComboBoxItem(txt, val);
            }
            // 콤보에 셋트
            this.cbSearchRap.Items.AddRange(comboItems);
            this.cbSearchRap.SelectedIndex = 0;

            Application.DoEvents();
        }

        private void Init_AgencyCode()
        {
            // 대행사를 조회한다.
            AgencyCodeModel agencycodeModel = new AgencyCodeModel();
            new AgencyCodeManager(systemModel, commonModel).GetAgencyCodeList(agencycodeModel);

            if (agencycodeModel.ResultCD.Equals("0000"))
            {
                // 데이터셋에 셋팅
                Utility.SetDataTable(targetingHomeDs.Agencys, agencycodeModel.AgencyCodeDataSet);
            }

            // 검색조건의 콤보
            this.cbSearchAgency.Items.Clear();

            // 콤보박스에 셋트할 코드목록을 담을 Item배열을 선언
            Janus.Windows.EditControls.UIComboBoxItem[] comboItems = new Janus.Windows.EditControls.UIComboBoxItem[agencycodeModel.ResultCnt + 1];

            comboItems[0] = new Janus.Windows.EditControls.UIComboBoxItem("대행사선택", "00");

            for (int i = 0; i < agencycodeModel.ResultCnt; i++)
            {
                DataRow row = targetingHomeDs.Agencys.Rows[i];

                string val = row["AgencyCode"].ToString();
                string txt = row["AgencyName"].ToString();
                comboItems[i + 1] = new Janus.Windows.EditControls.UIComboBoxItem(txt, val);
            }

            // 콤보에 셋트
            this.cbSearchAgency.Items.AddRange(comboItems);
            this.cbSearchAgency.SelectedIndex = 0;

            Application.DoEvents();
        }

        private void Init_AdvertiserCode()
        {
            // 광고주를 조회한다.
            ClientModel clientModel = new ClientModel();
            new ClientManager(systemModel, commonModel).GetAdvertiserList(clientModel);

            if (clientModel.ResultCD.Equals("0000"))
            {
                // 데이터셋에 셋팅
                Utility.SetDataTable(targetingHomeDs.Advertisers, clientModel.ClientDataSet);
            }

            // 검색조건의 콤보
            this.cbSearchAdvertiser.Items.Clear();

            // 콤보박스에 셋트할 코드목록을 담을 Item배열을 선언
            Janus.Windows.EditControls.UIComboBoxItem[] comboItems = new Janus.Windows.EditControls.UIComboBoxItem[clientModel.ResultCnt + 1];

            comboItems[0] = new Janus.Windows.EditControls.UIComboBoxItem("광고주선택", "00");

            for (int i = 0; i < clientModel.ResultCnt; i++)
            {
                DataRow row = targetingHomeDs.Advertisers.Rows[i];

                string val = row["AdvertiserCode"].ToString();
                string txt = row["AdvertiserName"].ToString();
                comboItems[i + 1] = new Janus.Windows.EditControls.UIComboBoxItem(txt, val);
            }

            // 콤보에 셋트
            this.cbSearchAdvertiser.Items.AddRange(comboItems);
            this.cbSearchAdvertiser.SelectedIndex = 0;

            Application.DoEvents();
        }

        private void Init_ContractState()
        {
            // 코드에서 계약상태를 조회한다.
            CodeModel codeModel = new CodeModel();
            codeModel.Section = "23";				// 코드분류 '23':계약상태  TODO: 코드분류는 추후 XML로 관리되어야...
            new CodeManager(systemModel, commonModel).GetCodeList(codeModel);

            if (codeModel.ResultCD.Equals("0000"))
            {
                // 데이터셋에 셋팅
                Utility.SetDataTable(targetingHomeDs.ContractState, codeModel.CodeDataSet);
            }

            // 검색조건의 콤보
            this.cbSearchContractState.Items.Clear();

            // 콤보박스에 셋트할 코드목록을 담을 Item배열을 선언
            Janus.Windows.EditControls.UIComboBoxItem[] comboItems = new Janus.Windows.EditControls.UIComboBoxItem[codeModel.ResultCnt + 1];

            comboItems[0] = new Janus.Windows.EditControls.UIComboBoxItem("계약상태선택", "00");

            for (int i = 0; i < codeModel.ResultCnt; i++)
            {
                DataRow row = targetingHomeDs.ContractState.Rows[i];

                string val = row["Code"].ToString();
                string txt = row["CodeName"].ToString();
                comboItems[i + 1] = new Janus.Windows.EditControls.UIComboBoxItem(txt, val);
            }
            // 콤보에 셋트
            this.cbSearchContractState.Items.AddRange(comboItems);
            this.cbSearchContractState.SelectedIndex = 0;

            Application.DoEvents();
        }

        private void InitCombo_Level()
        {

            if (commonModel.UserLevel == "20")
            {
                cbSearchMedia.SelectedValue = commonModel.MediaCode;
                cbSearchMedia.ReadOnly = true;
            }
            else
            {
                for (int i = 0; i < targetingHomeDs.Medias.Rows.Count; i++)
                {
                    DataRow row = targetingHomeDs.Medias.Rows[i];
                    if (row["MediaCode"].ToString().Equals(FrameSystem._HANATV.ToString()))
                    {
                        cbSearchMedia.SelectedValue = FrameSystem._HANATV; // 하나TV를 기본값으로 한다.	 		
                        break;
                    }
                    else
                    {
                        cbSearchMedia.SelectedValue = "00";
                    }
                }
            }
            if (commonModel.UserLevel == "30")
            {
                cbSearchRap.SelectedValue = commonModel.RapCode;
                cbSearchRap.ReadOnly = true;
            }
            if (commonModel.UserLevel == "40")
            {
                cbSearchAgency.SelectedValue = commonModel.AgencyCode;
                cbSearchAgency.ReadOnly = true;
            }

            Application.DoEvents();
        }

        private void InitButton()
        {
            if (canRead)
            {
                btnSearch.Enabled = true;
                btnExcel.Enabled = true;
            }
            grdExScheduleList.Focus();

            Application.DoEvents();
        }


        private void DisableButton()
        {
            btnSearch.Enabled = false;
            btnExcel.Enabled = false;
            btnSave.Enabled = false;

            Application.DoEvents();
        }

        private void Init_RegionData()
        {
            try
            {
                // 데이터모델 초기화
                targetingHomeModel.Init();

                // 광고 타겟팅 목록조회 서비스를 호출한다.
                new TargetingHomeManager(systemModel, commonModel).GetRegionList(targetingHomeModel);

                if (targetingHomeModel.ResultCD.Equals("0000"))
                {
                    Utility.SetDataTable(targetingHomeDs.TargetRegion, targetingHomeModel.RegionDataSet);
                }
            }
            catch (FrameException fe)
            {
                FrameSystem.showMsgForm("광고지역정보 조회오류", new string[] { fe.ErrCode, fe.ResultMsg });
            }
            catch (Exception ex)
            {
                FrameSystem.showMsgForm("광고지역정보 조회오류", new string[] { "", ex.Message });
            }
        }

        private void Init_AgeData()
        {
            try
            {
                // 데이터모델 초기화
                targetingHomeModel.Init();

                // 광고 타겟팅 목록조회 서비스를 호출한다.
                new TargetingHomeManager(systemModel, commonModel).GetAgeList(targetingHomeModel);

                if (targetingHomeModel.ResultCD.Equals("0000"))
                {
                    Utility.SetDataTable(targetingHomeDs.TargetAge, targetingHomeModel.AgeDataSet);
                }
            }
            catch (FrameException fe)
            {
                FrameSystem.showMsgForm("광고연령대 조회오류", new string[] { fe.ErrCode, fe.ResultMsg });
            }
            catch (Exception ex)
            {
                FrameSystem.showMsgForm("광고연령대 조회오류", new string[] { "", ex.Message });
            }
        }

        private void Init_tvRegion()
        {
            /*
			tvRegionNodeCount = 0;

			canChecking = false;	// 체크처리 이벤트에서 처리로직이 작동하지 않도록

			tvRegion.Nodes.Clear();

			tvRegion.Nodes.Add(new TreeNode("전체지역"));

			if(targetingHomeDs.TargetRegion.Count == 0) return;
			
			TreeNode Region1Node = null;
			TreeNode Region2Node = null;

			for(int i=0;i<targetingHomeDs.TargetRegion.Count;i++)
			{
				DataRow Row = targetingHomeDs.TargetRegion.Rows[i];
				string Name   = Row["RegionName"].ToString();
				string Code   = Row["RegionCode"].ToString();
				string Level  = Row["Level"].ToString();

				if(Code.Length < 5) Code = Utility.Fixlength(Code,2,5); // 5자리로 만들기

				if(Level.Equals("1"))
				{
					// 1단계 지역구분 추가
					Region1Node = new TreeNode(Name);
					Region1Node.Tag = Code;
					tvRegion.Nodes[0].Nodes.Add(Region1Node);
				}
				else if(Level.Equals("2"))
				{
					// 2단계 지역구분 추가
					Region2Node = new TreeNode(Name);
					Region2Node.Tag = Code;
					Region1Node.Nodes.Add(Region2Node);
				}

				tvRegionNodeCount++;
			}

			// 1단계를 확장하여 보여준다.
			tvRegion.Nodes[0].Expand();		

			canChecking = true;	// 체크처리 이벤트에서 처리로직이 다시 작동하도록
            */
        }

        private void Init_tlvRegion()
        {
            canChecking = false;	// 체크처리 이벤트에서 처리로직이 작동하지 않도록

            if (targetingHomeDs.TargetRegion.Count == 0) return;
            string name = "";
            string code = "";
            string level = "";

            TreeListViewItem rootItem = new TreeListViewItem("지역정보");
            TreeListViewItem level_1 = null;
            TreeListViewItem level_2 = null;
            TreeListViewItem level_3 = null;

            rootItem.Items.SortOrder = System.Windows.Forms.SortOrder.None;
            for (int i = 0; i < targetingHomeDs.TargetRegion.Count; i++)
            {
                DataRow Row = targetingHomeDs.TargetRegion.Rows[i];
                name = Row["RegionName"].ToString();
                code = Row["RegionCode"].ToString();
                level = Row["Level"].ToString();

                if (code.Length < 5) code = Utility.Fixlength(code, 2, 5); // 5자리로 만들기
                if (level.Equals("1"))
                {
                    level_1 = new TreeListViewItem(name, 0);
                    level_1.Tag = code;
                    rootItem.Items.Add(level_1);
                    level_1.Items.SortOrder = System.Windows.Forms.SortOrder.None;
                }
                else if (level.Equals("2"))
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

            canChecking = true;	// 체크처리 이벤트에서 처리로직이 다시 작동하도록
        }

        private void Init_tvTime()
        {
            tvTimeNodeCount = 0;

            canChecking = false;	// 체크처리 이벤트에서 처리로직이 작동하지 않도록

            //tvTime.Nodes.Clear();

            //tvTime.Nodes.Add(new TreeNode("전체시간대"));

            TreeNode timeNode = null;

            for (int i = 0; i < 24; i++)
            {
                int From = i;
                int To = i + 1;

                string strFrom = From.ToString();
                string strTo = To.ToString();

                if (strFrom.Length == 1) strFrom = "0" + strFrom;
                if (strTo.Length == 1) strTo = "0" + strTo;

                string TimeText = strFrom + "시 ~ " + strTo + "시";

                // 시간대 추가
                timeNode = new TreeNode(TimeText);
                timeNode.Tag = strFrom;
                //tvTime.Nodes[0].Nodes.Add(timeNode);
                tvTimeNodeCount++;
            }


            // 1단계를 확장하여 보여준다.
            //tvTime.Nodes[0].Expand();		

            canChecking = true;	// 체크처리 이벤트에서 처리로직이 다시 작동하도록
        }


        private void Init_tvAge()
        {
            tvAgeNodeCount = 0;

            canChecking = false;	// 체크처리 이벤트에서 처리로직이 작동하지 않도록

            tvAge.Nodes.Clear();

            tvAge.Nodes.Add(new TreeNode("전체연령대"));

            if (targetingHomeDs.TargetAge.Count == 0) return;

            TreeNode AgeNode = null;

            for (int i = 0; i < targetingHomeDs.TargetAge.Count; i++)
            {
                DataRow Row = targetingHomeDs.TargetAge.Rows[i];
                string Name = Row["AgeName"].ToString();
                string Code = Row["AgeCode"].ToString();

                // 연령대 추가
                AgeNode = new TreeNode(Name);
                AgeNode.Tag = Code;
                tvAge.Nodes[0].Nodes.Add(AgeNode);

                tvAgeNodeCount++;
            }

            // 1단계를 확장하여 보여준다.
            tvAge.Nodes[0].Expand();

            canChecking = true;	// 체크처리 이벤트에서 처리로직이 다시 작동하도록
        }

        private void Init_Collection()
        {
            // 광고주를 조회한다.
            TargetingHomeModel targetingHomeModel = new TargetingHomeModel();
            new TargetingHomeManager(systemModel, commonModel).GetCollectionList(targetingHomeModel);

            if (targetingHomeModel.ResultCD.Equals("0000"))
            {
                Utility.SetDataTable(targetingHomeDs.Collections, targetingHomeModel.CollectionsDataSet);
                StatusMessage(targetingHomeModel.ResultCnt + "건의 타겟군 정보가 조회되었습니다.");
            }

            #region 사용안함
            /* 2012.02.20 고객군타겟팅은 콤보에서 리스트 선택으로 변경. RH.Jung
			// 검색조건의 콤보
			this.cbCollection.Items.Clear();
			
			// 콤보박스에 셋트할 코드목록을 담을 Item배열을 선언
			Janus.Windows.EditControls.UIComboBoxItem[] comboItems = new Janus.Windows.EditControls.UIComboBoxItem[targetingHomeModel.ResultCnt + 1];

			comboItems[0] = new Janus.Windows.EditControls.UIComboBoxItem("타겟군선택","00");
			
			for(int i=0;i<targetingHomeModel.ResultCnt;i++)
			{
				DataRow row = targetingHomeDs.Collections.Rows[i];

				string val = row["CollectionCode"].ToString();
				string txt = row["CollectionName"].ToString();
				comboItems[i+1] = new Janus.Windows.EditControls.UIComboBoxItem(txt,val);
			}
			
			// 콤보에 셋트
			this.cbCollection.Items.AddRange(comboItems);
			this.cbCollection.SelectedIndex = 0;

			Application.DoEvents();
*/
            #endregion
        }

        #endregion

        #region 광고 타겟팅 액션처리 메소드

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
                SetTargetingCollectionList(); // 고객군타겟팅리스트 조회 추가  
                InitButton();
            }

        }

        /// <summary>
        /// 조회버튼 클릭
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
            if (IsNewSearchKey)
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
            if (e.KeyCode == Keys.Enter)
            {
                SearchTargeting();
            }
        }

        private void tvRegion_AfterCheck(object sender, System.Windows.Forms.TreeViewEventArgs e)
        {
            if (canChecking)
            {
                try
                {
                    canChecking = false;
                    ChangeNodeCheck(e.Node);	// 체크가 바뀐 노드
                }
                finally
                {
                    canChecking = true;
                }
            }
        }


        private void tvTime_AfterCheck(object sender, System.Windows.Forms.TreeViewEventArgs e)
        {
            if (canChecking)
            {
                try
                {
                    canChecking = false;
                    ChangeNodeCheck(e.Node);	// 체크가 바뀐 노드
                }
                finally
                {
                    canChecking = true;
                }
            }
        }

        private void tvAge_AfterCheck(object sender, System.Windows.Forms.TreeViewEventArgs e)
        {
            if (canChecking)
            {
                try
                {
                    canChecking = false;
                    ChangeNodeCheck(e.Node);	// 체크가 바뀐 노드
                }
                finally
                {
                    canChecking = true;
                }
            }
        }


        private void chkRegionYn_CheckedChanged(object sender, System.EventArgs e)
        {
            if (chkRegionYn.Checked)
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
            if (chkTimeYn.Checked)
            {
                chkTime00.Enabled = true;
                chkTime01.Enabled = true;
                chkTime02.Enabled = true;
                chkTime03.Enabled = true;
                chkTime04.Enabled = true;
                chkTime05.Enabled = true;
                chkTime06.Enabled = true;
                chkTime07.Enabled = true;
                chkTime08.Enabled = true;
                chkTime09.Enabled = true;
                chkTime10.Enabled = true;
                chkTime11.Enabled = true;
                chkTime12.Enabled = true;
                chkTime13.Enabled = true;
                chkTime14.Enabled = true;
                chkTime15.Enabled = true;
                chkTime16.Enabled = true;
                chkTime17.Enabled = true;
                chkTime18.Enabled = true;
                chkTime19.Enabled = true;
                chkTime20.Enabled = true;
                chkTime21.Enabled = true;
                chkTime22.Enabled = true;
                chkTime23.Enabled = true;
                ChkTimeChecked();
            }
            else
            {
                chkTime00.Enabled = false;
                chkTime01.Enabled = false;
                chkTime02.Enabled = false;
                chkTime03.Enabled = false;
                chkTime04.Enabled = false;
                chkTime05.Enabled = false;
                chkTime06.Enabled = false;
                chkTime07.Enabled = false;
                chkTime08.Enabled = false;
                chkTime09.Enabled = false;
                chkTime10.Enabled = false;
                chkTime11.Enabled = false;
                chkTime12.Enabled = false;
                chkTime13.Enabled = false;
                chkTime14.Enabled = false;
                chkTime15.Enabled = false;
                chkTime16.Enabled = false;
                chkTime17.Enabled = false;
                chkTime18.Enabled = false;
                chkTime19.Enabled = false;
                chkTime20.Enabled = false;
                chkTime21.Enabled = false;
                chkTime22.Enabled = false;
                chkTime23.Enabled = false;
                ChkTimeChecked();
            }
        }

        private void chkAgeYn_CheckedChanged(object sender, System.EventArgs e)
        {
            if (chkAgeYn.Checked)
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
            if (chkAgeBtnYn.Checked)
            {
                udAgeBtnBegin.Enabled = true;
                udAgeBtnEnd.Enabled = true;
            }
            else
            {
                udAgeBtnBegin.Enabled = false;
                udAgeBtnEnd.Enabled = false;
            }
        }

        private void chkSexYn_CheckedChanged(object sender, System.EventArgs e)
        {
            if (chkSexYn.Checked)
            {
                chkSexMan.Enabled = true;
                chkSexWoman.Enabled = true;
            }
            else
            {
                chkSexMan.Enabled = false;
                chkSexWoman.Enabled = false;
            }
        }

        private void chkRateYn_CheckedChanged(object sender, System.EventArgs e)
        {
            //			if(chkRateYn.Checked)
            //			{
            //				rbRateAll.Enabled = true;
            //				rbRate12.Enabled  = true;
            //				rbRate15.Enabled  = true;
            //				rbRate19.Enabled  = true;
            //			}
            //			else
            //			{
            //				rbRateAll.Enabled = false;
            //				rbRate12.Enabled  = false;
            //				rbRate15.Enabled  = false;
            //				rbRate19.Enabled  = false;
            //			}
        }

        //요일별
        private void chkWeekYn_CheckedChanged(object sender, System.EventArgs e)
        {
            if (chkWeekYn.Checked)
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

        private void ebContractAmt_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnSave.Focus();
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

        #endregion

        #region 처리메소드

        /// <summary>
        /// [E_01] 셋탑정보모델 데이터 초기화 
        /// </summary>
        private void Init_StbData()
        {
            try
            {
                TargetingModel targetingModel = new TargetingModel();

                // 데이터모델 초기화
                targetingModel.Init();

                // 광고 타겟팅 목록조회 서비스를 호출한다.
                new TargetingManager(systemModel, commonModel).GetStbList(targetingModel);

                if (targetingModel.ResultCD.Equals("0000"))
                {
                    Utility.SetDataTable(targetingHomeDs.TargetStb, targetingModel.TargetingDataSet);
                }
            }
            catch (FrameException fe)
            {
                FrameSystem.showMsgForm("셋탑모델정보 조회오류", new string[] { fe.ErrCode, fe.ResultMsg });
            }
            catch (Exception ex)
            {
                FrameSystem.showMsgForm("셋탑모델정보 조회오류", new string[] { "", ex.Message });
            }
        }

        /// <summary>
        /// [E_01] 셋탑정보모델 TreeView 초기화 
        /// </summary>
        private void Init_tvStb()
        {
            tvStbNodeCount = 0;

            canChecking = false;	// 체크처리 이벤트에서 처리로직이 작동하지 않도록

            tvStb.Nodes.Clear();

            tvStb.Nodes.Add(new TreeNode("셋탑모델별"));

            if (targetingHomeDs.TargetStb.Count == 0) return;

            TreeNode stbNode = null;

            for (int i = 0; i < targetingHomeDs.TargetStb.Count; i++)
            {
                DataRow Row = targetingHomeDs.TargetStb.Rows[i];
                //string Name  = Row["StbName"].ToString();
                string Model = Row["StbModel"].ToString();

                // 추가
                //stbNode = new TreeNode(Name);
                stbNode = new TreeNode(Model);
                stbNode.Tag = Model;
                tvStb.Nodes[0].Nodes.Add(stbNode);

                tvStbNodeCount++;
            }

            // 1단계를 확장하여 보여준다.
            tvStb.Nodes[0].Expand();

            canChecking = true;	// 체크처리 이벤트에서 처리로직이 다시 작동하도록
        }



        /// <summary>
        /// 광고 타겟팅목록 조회
        /// </summary>
        private void SearchTargeting()
        {
            IsSearching = true;
            StatusMessage("광고타겟팅 편성현황을 조회합니다.");

            try
            {
                uiPanelDetail.Text = "타겟팅설정";

                // 데이터모델 초기화
                targetingHomeModel.Init();

                // 데이터모델에 전송할 내용을 셋트한다.
                if (IsNewSearchKey)
                {
                    targetingHomeModel.SearchKey = "";
                }
                else
                {
                    targetingHomeModel.SearchKey = ebSearchKey.Text;
                }

                targetingHomeModel.SearchMediaCode = cbSearchMedia.SelectedItem.Value.ToString();
                targetingHomeModel.SearchRapCode = cbSearchRap.SelectedItem.Value.ToString();
                targetingHomeModel.SearchAgencyCode = cbSearchAgency.SelectedItem.Value.ToString();
                targetingHomeModel.SearchAdvertiserCode = cbSearchAdvertiser.SelectedItem.Value.ToString();
                targetingHomeModel.SearchContractState = cbSearchContractState.SelectedItem.Value.ToString();
                if (chkAdState_20.Checked) targetingHomeModel.SearchchkAdState_20 = "Y";
                if (chkAdState_30.Checked) targetingHomeModel.SearchchkAdState_30 = "Y";
                if (chkAdState_40.Checked) targetingHomeModel.SearchchkAdState_40 = "Y";
                if (chkTargetY.Checked) targetingHomeModel.SearchchkTimeY = "Y";		//타겟팅 : 설정
                if (chkTargetN.Checked) targetingHomeModel.SearchchkTimeN = "Y";		//타겟팅 : 미설정

                // 광고 타겟팅 목록조회 서비스를 호출한다.
                new TargetingHomeManager(systemModel, commonModel).GetTargetingList2(targetingHomeModel);

                if (targetingHomeModel.ResultCD.Equals("0000"))
                {
                    Utility.SetDataTable(targetingHomeDs.TargetList, targetingHomeModel.TargetingDataSet);

                    StatusMessage(targetingHomeModel.ResultCnt + "건의 광고 정보가 조회되었습니다.");
                    AddSchChoice();
                    SetDetailText();
                    SetTargetingCollectionList();  // 2012.02.20 고객군타겟팅 추가
                }
            }
            catch (FrameException fe)
            {
                FrameSystem.showMsgForm("광고 타겟팅 편성현황 조회오류", new string[] { fe.ErrCode, fe.ResultMsg });
            }
            catch (Exception ex)
            {
                FrameSystem.showMsgForm("광고 타겟팅 편성현황 조회오류", new string[] { "", ex.Message });
            }
            finally
            {
                IsSearching = false; // 조회중 Flag 리셋
            }
        }

        /// <summary>
        /// 키캆을찾아 그리드 키에 해당되는로우로..
        /// </summary>
        private void AddSchChoice()
        {
            try
            {
                int rowIndex = 0;
                if (dt.Rows.Count < 1) return;

                foreach (DataRow row in dt.Rows)
                {
                    if (row["ItemNo"].ToString().Equals(keyItemNo))
                    {
                        cm.Position = rowIndex;
                        break;
                    }

                    rowIndex++;
                }
                grdExScheduleList.EnsureVisible();
            }
            catch (FrameException fe)
            {
                FrameSystem.showMsgForm("키캆오류", new string[] { fe.ErrCode, fe.ResultMsg });
            }
            catch (Exception ex)
            {
                FrameSystem.showMsgForm("키캆오류", new string[] { "", ex.Message });
            }
        }

        /// <summary>
        /// 광고 타겟팅 상세정보의 셋트
        /// </summary>
        private void SetDetailText()
        {
            int curRow = cm.Position;

            if (curRow >= 0)
            {
                ResetDetailText();

                keyItemNo = dt.Rows[curRow]["ItemNo"].ToString();       //광고번호
                keyItemName = dt.Rows[curRow]["ItemName"].ToString();     //광고명 

                uiPanelDetail.Text = "타겟팅설정 : [" + keyItemNo + "] " + keyItemName;

                // 데이터모델 초기화
                targetingHomeModel.Init();
                targetingHomeModel.ItemNo = keyItemNo;

                // 광고 타겟팅 상세조회 서비스를 호출한다.
                new TargetingHomeManager(systemModel, commonModel).GetTargetingDetail2(targetingHomeModel);

                if (targetingHomeModel.ResultCD.Equals("0000") && targetingHomeModel.ResultCnt > 0)
                {
                    Utility.SetDataTable(targetingHomeDs.TargetDetial, targetingHomeModel.DetailDataSet);

                    DataRow row = targetingHomeDs.TargetDetial.Rows[0];

                    ResetDetailText();
                    ebContractAmt.Text = row["ContractAmt"].ToString();       //노출계약수량
                    cbPriorityCd.SelectedValue = row["PriorityCd"].ToString();//노출등급 순위코드
                    if (cbPriorityCd.SelectedIndex == -1)
                    {
                        cbPriorityCd.SelectedIndex = 4;
                    }

                    // 노출제어여부
                    if (row["AmtControlYn"].ToString().Equals("Y"))
                    {
                        rbControlYn_Y.Checked = true;
                        rbControlYn_N.Checked = false;
                    }
                    else
                    {
                        rbControlYn_Y.Checked = false;
                        rbControlYn_N.Checked = true;
                    }

                    // 노출제어 비율
                    if (row["AmtControlRate"].ToString().Length > 0)
                    {
                        udControlRate.Value = Convert.ToInt16(row["AmtControlRate"].ToString());
                    }

                    // 노출지역 
                    if (row["TgtRegion1Yn"].ToString().Equals("Y"))
                    {
                        chkRegionYn.Checked = true;
                        tlvRegion.Enabled = true;

                        string[] chkTgtRegionSplit = Utility.SplitByString(row["TgtRegion1"].ToString(), "^");
                        SetTargetRegion(chkTgtRegionSplit);
                    }
                    else
                    {
                        chkRegionYn.Checked = false;
                        tlvRegion.Enabled = false;
                    }

                    // 노출시간대
                    if (row["TgtTimeYn"].ToString().Equals("Y"))
                    {
                        chkTimeYn.Checked = true;

                        string[] chkTgtTimeSplit = Utility.SplitByString(row["TgtTime"].ToString(), "^");
                        SetTargetTime(chkTgtTimeSplit);
                    }
                    else
                    {
                        chkTimeYn.Checked = false;
                    }

                    // 노출시간대
                    if (row["TgtTimeYn"].ToString().Equals("Y"))
                    {
                        chkTimeYn.Checked = true;

                        string[] chkTgtTimeSplit = Utility.SplitByString(row["TgtTime"].ToString(), "^");
                        SetTargetTime(chkTgtTimeSplit);
                    }
                    else
                    {
                        chkTimeYn.Checked = false;
                    }

                    // 노출연령대
                    if (row["TgtAgeYn"].ToString().Equals("Y"))
                    {
                        chkAgeYn.Checked = true;
                        tvAge.Enabled = true;

                        string[] chkTgtAgeSplit = Utility.SplitByString(row["TgtAge"].ToString(), "^");
                        SetTargetAge(chkTgtAgeSplit);
                    }
                    else
                    {
                        chkAgeYn.Checked = false;
                        tvAge.Enabled = false;
                    }

                    #region 연령 구간
                    if (row["TgtAgeBtnYn"].ToString().Equals("Y"))
                    {
                        chkAgeBtnYn.Checked = true;
                        udAgeBtnBegin.Enabled = true;
                        udAgeBtnEnd.Enabled = true;

                        if (row["TgtAgeBtnBegin"].ToString().Length > 0)
                        {
                            udAgeBtnBegin.Value = Convert.ToInt16(row["TgtAgeBtnBegin"].ToString());
                        }
                        else
                        {
                            udAgeBtnBegin.Value = 0;
                        }
                        if (row["TgtAgeBtnEnd"].ToString().Length > 0)
                        {
                            udAgeBtnEnd.Value = Convert.ToInt16(row["TgtAgeBtnEnd"].ToString());
                        }
                        else
                        {
                            udAgeBtnEnd.Value = 0;
                        }
                    }
                    else
                    {
                        chkAgeBtnYn.Checked = false;
                        udAgeBtnBegin.Enabled = false;
                        udAgeBtnEnd.Enabled = false;

                    }
                    #endregion

                    #region 성별
                    if (row["TgtSexYn"].ToString().Equals("Y"))
                    {
                        chkSexYn.Checked = true;
                        chkSexMan.Enabled = true;
                        chkSexWoman.Enabled = true;

                        if (row["TgtSexMan"].ToString().Equals("Y"))
                        {
                            chkSexMan.Checked = true;
                        }
                        else
                        {
                            chkSexMan.Checked = false;
                        }
                        if (row["TgtSexWoman"].ToString().Equals("Y"))
                        {
                            chkSexWoman.Checked = true;
                        }
                        else
                        {
                            chkSexWoman.Checked = false;
                        }
                    }
                    else
                    {
                        chkSexYn.Checked = false;
                        chkSexMan.Enabled = false;
                        chkSexWoman.Enabled = false;
                    }
                    #endregion

                    // 타겟군별
                    if (row["TgtCollectionYn"].ToString().Equals("Y") || row["TgtCollectionYn"].ToString().Equals("-"))
                    {
                        chkCollectionYn.Checked = true;
                        uiTabPage2.Enabled = true;
                        lblCollMsg.Visible = true;

                        int cnt = Convert.ToInt32(row["TgtCollection"].ToString());

                        if (cnt > 0)
                        {
                            lblCollMsg.ForeColor = Color.Black;
                            lblCollMsg.BackColor = Color.Transparent;
                            lblCollMsg.Text = "타겟고객군이 " + cnt.ToString() + "건 설정되어 있습니다.";
                        }
                        else
                        {
                            lblCollMsg.ForeColor = Color.White;
                            lblCollMsg.BackColor = Color.Red;
                            lblCollMsg.Text = "설정된 고객군이 없습니다. 고객군 설정탭에서 고객군을 설정하셔야 합니다!!!";
                        }
                    }
                    else
                    {
                        lblCollMsg.Visible = false;
                        chkCollectionYn.Checked = false;
                        uiTabPage2.Enabled = false;
                    }

                    #region 요일별
                    if (row["TgtWeekYn"].ToString().Equals("Y"))
                    {
                        chkWeekYn.Checked = true;
                        chkMon.Enabled = true;
                        chkThu.Enabled = true;
                        chkWed.Enabled = true;
                        chkThe.Enabled = true;
                        chkFri.Enabled = true;
                        chkSat.Enabled = true;
                        chkSun.Enabled = true;
                        ChkWeekChecked();

                        //여러개의 체크박스의 값을 한 필드에 저장후 꺼내올때..'^'잘라서 string배열에 넣는다.
                        string[] chkTgtWeekSplit = Utility.SplitByString(row["TgtWeek"].ToString(), "^");
                        //string배열에 넣은 값들을 루핑..
                        for (int i = 0; i < chkTgtWeekSplit.Length; i++)
                        {
                            //루프돌아 나온값들을 string변수에 담는다..
                            string chkTgtWeek = chkTgtWeekSplit[i];
                            //string변수를 case문으로 해당값들을 비교하여..해당 체크박스를 컨트롤한다.
                            switch (chkTgtWeek)
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
                        ChkWeekChecked();
                        chkWeekYn.Checked = false;
                        chkMon.Enabled = false;
                        chkThu.Enabled = false;
                        chkWed.Enabled = false;
                        chkThe.Enabled = false;
                        chkFri.Enabled = false;
                        chkSat.Enabled = false;
                        chkSun.Enabled = false;
                    }
                    #endregion

                    #region 시간대별
                    if (row["TgtTimeYn"].ToString().Equals("Y"))
                    {
                        chkTimeYn.Checked = true;
                        chkTime00.Enabled = true; chkTime01.Enabled = true; chkTime02.Enabled = true;
                        chkTime03.Enabled = true; chkTime04.Enabled = true; chkTime05.Enabled = true;
                        chkTime06.Enabled = true; chkTime07.Enabled = true; chkTime08.Enabled = true;
                        chkTime09.Enabled = true; chkTime10.Enabled = true; chkTime11.Enabled = true;
                        chkTime12.Enabled = true; chkTime13.Enabled = true; chkTime14.Enabled = true;
                        chkTime15.Enabled = true; chkTime16.Enabled = true; chkTime17.Enabled = true;
                        chkTime18.Enabled = true; chkTime19.Enabled = true; chkTime20.Enabled = true;
                        chkTime21.Enabled = true; chkTime22.Enabled = true; chkTime23.Enabled = true;
                        ChkTimeChecked();
                        //여러개의 체크박스의 값을 한 필드에 저장후 꺼내올때..'^'잘라서 string배열에 넣는다.
                        string[] chkTgtTimeSplit = Utility.SplitByString(row["TgtTime"].ToString(), "^");
                        //string배열에 넣은 값들을 루핑..
                        for (int i = 0; i < chkTgtTimeSplit.Length; i++)
                        {
                            //루프돌아 나온값들을 string변수에 담는다..
                            string chkTgtTime = chkTgtTimeSplit[i];
                            //string변수를 case문으로 해당값들을 비교하여..해당 체크박스를 컨트롤한다.
                            switch (chkTgtTime)
                            {
                                case "00":
                                    chkTime00.Checked = true;
                                    break;
                                case "01":
                                    chkTime01.Checked = true;
                                    break;
                                case "02":
                                    chkTime02.Checked = true;
                                    break;
                                case "03":
                                    chkTime03.Checked = true;
                                    break;
                                case "04":
                                    chkTime04.Checked = true;
                                    break;
                                case "05":
                                    chkTime05.Checked = true;
                                    break;
                                case "06":
                                    chkTime06.Checked = true;
                                    break;
                                case "07":
                                    chkTime07.Checked = true;
                                    break;
                                case "08":
                                    chkTime08.Checked = true;
                                    break;
                                case "09":
                                    chkTime09.Checked = true;
                                    break;
                                case "10":
                                    chkTime10.Checked = true;
                                    break;
                                case "11":
                                    chkTime11.Checked = true;
                                    break;
                                case "12":
                                    chkTime12.Checked = true;
                                    break;
                                case "13":
                                    chkTime13.Checked = true;
                                    break;
                                case "14":
                                    chkTime14.Checked = true;
                                    break;
                                case "15":
                                    chkTime15.Checked = true;
                                    break;
                                case "16":
                                    chkTime16.Checked = true;
                                    break;
                                case "17":
                                    chkTime17.Checked = true;
                                    break;
                                case "18":
                                    chkTime18.Checked = true;
                                    break;
                                case "19":
                                    chkTime19.Checked = true;
                                    break;
                                case "20":
                                    chkTime20.Checked = true;
                                    break;
                                case "21":
                                    chkTime21.Checked = true;
                                    break;
                                case "22":
                                    chkTime22.Checked = true;
                                    break;
                                case "23":
                                    chkTime23.Checked = true;
                                    break;
                            }
                        }
                    }
                    else
                    {
                        ChkTimeChecked();
                        chkTimeYn.Checked = false;
                        chkTime00.Enabled = false;
                        chkTime01.Enabled = false;
                        chkTime02.Enabled = false;
                        chkTime03.Enabled = false;
                        chkTime04.Enabled = false;
                        chkTime05.Enabled = false;
                        chkTime06.Enabled = false;
                        chkTime07.Enabled = false;
                        chkTime08.Enabled = false;
                        chkTime09.Enabled = false;
                        chkTime10.Enabled = false;
                        chkTime11.Enabled = false;
                        chkTime12.Enabled = false;
                        chkTime13.Enabled = false;
                        chkTime14.Enabled = false;
                        chkTime15.Enabled = false;
                        chkTime16.Enabled = false;
                        chkTime17.Enabled = false;
                        chkTime18.Enabled = false;
                        chkTime19.Enabled = false;
                        chkTime20.Enabled = false;
                        chkTime21.Enabled = false;
                        chkTime22.Enabled = false;
                        chkTime23.Enabled = false;
                    }
                    #endregion


                    #region [타겟팅정보 13 셋탑모델별 ]

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


                    lbNotice.Visible = false;
                    isSettedTargeting = true;  // 해당광고 타겟팅설정여부 셋트

                }
                else
                {
                    MessageBox.Show("해당 광고에 대한 타겟팅이 설정되지 않았습니다..", "타겟팅", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    ebContractAmt.Text = dt.Rows[curRow]["ContractAmt"].ToString();
                    lbNotice.Visible = true;

                    isSettedTargeting = false;  // 해당광고 타겟팅설정여부 셋트
                }

                if (canUpdate) btnSave.Enabled = true;

            }

            StatusMessage("준비");
        }

        /// <summary>
        /// 셋탑모델별 리스트 [E_08]
        /// </summary>
        /// <param name="chkList"></param>
        private void SetTargetStb(string[] chkList)
        {
#if (DEBUG)
            Debug.WriteLine(MethodBase.GetCurrentMethod().Name);
#endif
            canChecking = false;	// 체크처리 이벤트에서 처리로직이 작동하지 않도록

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


        private void ChkTimeChecked()
        {
            chkTime00.Checked = false;
            chkTime01.Checked = false;
            chkTime02.Checked = false;
            chkTime03.Checked = false;
            chkTime04.Checked = false;
            chkTime05.Checked = false;
            chkTime06.Checked = false;
            chkTime07.Checked = false;
            chkTime08.Checked = false;
            chkTime09.Checked = false;
            chkTime10.Checked = false;
            chkTime11.Checked = false;
            chkTime12.Checked = false;
            chkTime13.Checked = false;
            chkTime14.Checked = false;
            chkTime15.Checked = false;
            chkTime16.Checked = false;
            chkTime17.Checked = false;
            chkTime18.Checked = false;
            chkTime19.Checked = false;
            chkTime20.Checked = false;
            chkTime21.Checked = false;
            chkTime22.Checked = false;
            chkTime23.Checked = false;
        }

        private void SetTargetTime(string[] chkList)
        {
            //canChecking = false;	// 체크처리 이벤트에서 처리로직이 작동하지 않도록

            //int chkCount = SetAllNodeTagCheck(tvTime.Nodes[0], chkList);

            //if (tvTimeNodeCount == chkCount)
            //{
            //    tvTime.Nodes[0].Checked = true;
            //}
            //else
            //{
            //    tvTime.Nodes[0].Checked = false;
            //}
            //canChecking = true;	

        }

        private void SetTargetAge(string[] chkList)
        {
            canChecking = false;	// 체크처리 이벤트에서 처리로직이 작동하지 않도록

            int chkCount = SetAllNodeTagCheck(tvAge.Nodes[0], chkList);

            if (tvAgeNodeCount == chkCount)
            {
                tvAge.Nodes[0].Checked = true;
            }
            else
            {
                tvAge.Nodes[0].Checked = false;
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

        /// <summary>
        /// 광고 타겟팅 내역상세정보 저장
        /// </summary>
        private void SetTargetingDetailAdd()
        {
            StatusMessage("홈광고 타겟팅 정보를 저장합니다.");

            try
            {
                //저장 전에 모델을 초기화 해준다.
                targetingHomeModel.Init();

                int curRow = cm.Position;

                // 데이터모델에 전송할 내용을 셋트한다.

                //광고번호
                targetingHomeModel.ItemNo = keyItemNo;
                targetingHomeModel.ItemName = keyItemName;

                // 광고물량
                targetingHomeModel.ContractAmt = ebContractAmt.Text.Replace(",", "");

                // 빈도
                targetingHomeModel.PriorityCd = cbPriorityCd.SelectedValue.ToString();

                // 노출제어여부
                if (rbControlYn_Y.Checked) targetingHomeModel.AmtControlYn = "Y";
                if (rbControlYn_N.Checked) targetingHomeModel.AmtControlYn = "N";

                // 노출제어비율
                targetingHomeModel.AmtControlRate = udControlRate.Value.ToString();

                //노출지역 설정
                if (chkRegionYn.Checked)
                {
                    targetingHomeModel.TgtRegion1Yn = "Y";
                    //targetingHomeModel.TgtRegion1   = GetAllNodeCheckedTag(tvRegion.Nodes[0],"^");
                    targetingHomeModel.TgtRegion1 = getTargetRegion();

                    //마지막 구분자는 생략
                    //targetingHomeModel.TgtRegion1   = targetingHomeModel.TgtRegion1.Substring(0,targetingHomeModel.TgtRegion1.Length-1);	
                }
                else
                {
                    targetingHomeModel.TgtRegion1Yn = "N";
                    targetingHomeModel.TgtRegion1 = "";
                }

                //노출시간대 설정
                //				if(chkTimeYn.Checked)
                //				{
                //					targetingHomeModel.TgtTimeYn = "Y";
                //					targetingHomeModel.TgtTime   = GetAllNodeCheckedTag(tvTime.Nodes[0],"^");
                //					//마지막 구분자는 생략
                //					targetingHomeModel.TgtTime   = targetingHomeModel.TgtTime.Substring(0,targetingHomeModel.TgtTime.Length-1);		
                //				}  
                //				else
                //				{
                //					targetingHomeModel.TgtTimeYn = "N";
                //					targetingHomeModel.TgtTime   = "";
                //				}

                //노출연령대 설정
                if (chkAgeYn.Checked)
                {
                    targetingHomeModel.TgtAgeYn = "Y";
                    targetingHomeModel.TgtAge = GetAllNodeCheckedTag(tvAge.Nodes[0], "^");
                    //마지막 구분자는 생략
                    targetingHomeModel.TgtAge = targetingHomeModel.TgtAge.Substring(0, targetingHomeModel.TgtAge.Length - 1);
                }
                else
                {
                    targetingHomeModel.TgtAgeYn = "N";
                    targetingHomeModel.TgtAge = "";
                }

                //노출연령구간 설정
                if (chkAgeBtnYn.Checked)
                {
                    targetingHomeModel.TgtAgeBtnYn = "Y";
                    targetingHomeModel.TgtAgeBtnBegin = udAgeBtnBegin.Value.ToString();
                    targetingHomeModel.TgtAgeBtnEnd = udAgeBtnEnd.Value.ToString();
                }
                else
                {
                    targetingHomeModel.TgtAgeBtnYn = "N";
                    targetingHomeModel.TgtAgeBtnBegin = "0";
                    targetingHomeModel.TgtAgeBtnEnd = "0";
                }

                //노출성별 설정
                if (chkSexYn.Checked)
                {
                    targetingHomeModel.TgtSexYn = "Y";
                    if (chkSexMan.Checked)
                    {
                        targetingHomeModel.TgtSexMan = "Y";
                    }
                    else
                    {
                        targetingHomeModel.TgtSexMan = "N";
                    }
                    if (chkSexWoman.Checked)
                    {
                        targetingHomeModel.TgtSexWoman = "Y";
                    }
                    else
                    {
                        targetingHomeModel.TgtSexWoman = "N";
                    }

                }
                else
                {
                    targetingHomeModel.TgtSexYn = "N";
                    if (chkSexMan.Checked) targetingHomeModel.TgtSexMan = "N";
                    if (chkSexWoman.Checked) targetingHomeModel.TgtSexWoman = "N";
                }

                // 타겟군별
                if (chkCollectionYn.Checked)
                {
                    // 2012.02.20 고객군타켓팅 
                    //if( cbCollection.SelectedIndex < 1 )
                    //{
                    //    MessageBox.Show("특정 타겟군이 선택되지 않았습니다..","홈타겟팅", MessageBoxButtons.OK, MessageBoxIcon.Warning );
                    //    cbCollection.Focus();
                    //    return;
                    //}

                    //if( rbCollectionPlus.Checked )          targetingHomeModel.TgtCollectionYn    = "Y";
                    //else if( rbCollectionMinus.Checked )    targetingHomeModel.TgtCollectionYn    = "-";
                    //else                                    targetingHomeModel.TgtCollectionYn    = "Y";
                    //targetingHomeModel.TgtCollection    = cbCollection.SelectedValue.ToString();

                    targetingHomeModel.TgtCollectionYn = "Y";
                }
                else
                {
                    targetingHomeModel.TgtCollectionYn = "N";
                    //targetingHomeModel.TgtCollection	  = "0";
                }

                //시간대별 설정이 체크되었을때..
                if (chkTimeYn.Checked)
                {
                    targetingHomeModel.TgtTimeYn = "Y";
                    if (chkTime00.Checked)
                    {
                        //'^'는 한 필드에 여러개의 체크박스값들을 입력하기 때문에 구분을 위해 사용..
                        keyTime00 = "00^";
                    }
                    else { keyTime00 = ""; }
                    if (chkTime01.Checked)
                    {
                        keyTime01 = "01^";
                    }
                    else { keyTime01 = ""; }
                    if (chkTime02.Checked)
                    {
                        keyTime02 = "02^";
                    }
                    else { keyTime02 = ""; }
                    if (chkTime03.Checked)
                    {
                        keyTime03 = "03^";
                    }
                    else { keyTime03 = ""; }
                    if (chkTime04.Checked)
                    {
                        keyTime04 = "04^";
                    }
                    else { keyTime04 = ""; }
                    if (chkTime05.Checked)
                    {
                        keyTime05 = "05^";
                    }
                    else { keyTime05 = ""; }
                    if (chkTime06.Checked)
                    {
                        keyTime06 = "06^";
                    }
                    else { keyTime06 = ""; }
                    if (chkTime07.Checked)
                    {
                        keyTime07 = "07^";
                    }
                    else { keyTime07 = ""; }
                    if (chkTime08.Checked)
                    {
                        keyTime08 = "08^";
                    }
                    else { keyTime08 = ""; }
                    if (chkTime09.Checked)
                    {
                        keyTime09 = "09^";
                    }
                    else { keyTime09 = ""; }
                    if (chkTime10.Checked)
                    {
                        keyTime10 = "10^";
                    }
                    else { keyTime10 = ""; }
                    if (chkTime11.Checked)
                    {
                        keyTime11 = "11^";
                    }
                    else { keyTime11 = ""; }
                    if (chkTime12.Checked)
                    {
                        keyTime12 = "12^";
                    }
                    else { keyTime12 = ""; }
                    if (chkTime13.Checked)
                    {
                        keyTime13 = "13^";
                    }
                    else { keyTime13 = ""; }
                    if (chkTime14.Checked)
                    {
                        keyTime14 = "14^";
                    }
                    else { keyTime14 = ""; }
                    if (chkTime15.Checked)
                    {
                        keyTime15 = "15^";
                    }
                    else { keyTime15 = ""; }
                    if (chkTime16.Checked)
                    {
                        keyTime16 = "16^";
                    }
                    else { keyTime16 = ""; }
                    if (chkTime17.Checked)
                    {
                        keyTime17 = "17^";
                    }
                    else { keyTime17 = ""; }
                    if (chkTime18.Checked)
                    {
                        keyTime18 = "18^";
                    }
                    else { keyTime18 = ""; }
                    if (chkTime19.Checked)
                    {
                        keyTime19 = "19^";
                    }
                    else { keyTime19 = ""; }
                    if (chkTime20.Checked)
                    {
                        keyTime20 = "20^";
                    }
                    else { keyTime20 = ""; }
                    if (chkTime21.Checked)
                    {
                        keyTime21 = "21^";
                    }
                    else { keyTime21 = ""; }
                    if (chkTime22.Checked)
                    {
                        keyTime22 = "22^";
                    }
                    else { keyTime22 = ""; }
                    if (chkTime23.Checked)
                    {
                        keyTime23 = "23^";
                    }
                    else { keyTime23 = ""; }
                }
                else
                {
                    targetingHomeModel.TgtTimeYn = "N";
                    keyTime00 = "";
                    keyTime01 = "";
                    keyTime02 = "";
                    keyTime03 = "";
                    keyTime04 = "";
                    keyTime05 = "";
                    keyTime06 = "";
                    keyTime07 = "";
                    keyTime08 = "";
                    keyTime09 = "";
                    keyTime10 = "";
                    keyTime11 = "";
                    keyTime12 = "";
                    keyTime13 = "";
                    keyTime14 = "";
                    keyTime15 = "";
                    keyTime16 = "";
                    keyTime17 = "";
                    keyTime18 = "";
                    keyTime19 = "";
                    keyTime20 = "";
                    keyTime21 = "";
                    keyTime22 = "";
                    keyTime23 = "";
                }
                //체크박스의 값들을 string으로 결합하여 Model에 담음..
                targetingHomeModel.TgtTime = keyTime00.ToString() + keyTime01.ToString() + keyTime02.ToString() + keyTime03.ToString() +
                                             keyTime04.ToString() + keyTime05.ToString() + keyTime06.ToString() + keyTime07.ToString() +
                                             keyTime08.ToString() + keyTime09.ToString() + keyTime10.ToString() + keyTime11.ToString() +
                                             keyTime12.ToString() + keyTime13.ToString() + keyTime14.ToString() + keyTime15.ToString() +
                                             keyTime16.ToString() + keyTime17.ToString() + keyTime18.ToString() + keyTime19.ToString() +
                                             keyTime20.ToString() + keyTime21.ToString() + keyTime22.ToString() + keyTime23.ToString();
                //Model에 담긴 값을 Substring으로 마지막 한자리를 자른다..이유는 하나의 값뒤에 붙어있는 '^'를 제거하기 위해..
                //ex)DB에 저장된 마지막 값에는 항상'^'붙어있기 때문에..
                if (targetingHomeModel.TgtTimeYn.Equals("Y"))
                {
                    targetingHomeModel.TgtTime = targetingHomeModel.TgtTime.Substring(0, targetingHomeModel.TgtTime.Length - 1);
                }

                //노출등급 설정
                //				if(chkRateYn.Checked)
                //				{
                //					targetingHomeModel.TgtRateYn = "Y";
                //					if(rbRateAll.Checked) targetingHomeModel.TgtRate       = "0";
                //					if(rbRate12.Checked) targetingHomeModel.TgtRate        = "12";
                //					if(rbRate15.Checked) targetingHomeModel.TgtRate        = "15";
                //					if(rbRate19.Checked) targetingHomeModel.TgtRate        = "19";
                //				}
                //				else
                //				{
                //					targetingHomeModel.TgtRateYn = "N";
                //					targetingHomeModel.TgtRate   = "0";
                //				}

                //요일별 설정이 체크되었을때..
                if (chkWeekYn.Checked)
                {
                    targetingHomeModel.TgtWeekYn = "Y";
                    if (chkSun.Checked)
                    {
                        keySun = "1^";
                    }
                    else
                    { keySun = ""; }
                    if (chkMon.Checked)
                    {
                        //'^'는 한 필드에 여러개의 체크박스값들을 입력하기 때문에 구분을 위해 사용..
                        keyMon = "2^";
                    }
                    else
                    { keyMon = ""; }
                    if (chkThu.Checked)
                    {
                        keyThu = "3^";
                    }
                    else
                    { keyThu = ""; }
                    if (chkWed.Checked)
                    {
                        keyWed = "4^";
                    }
                    else
                    { keyWed = ""; }
                    if (chkThe.Checked)
                    {
                        keyThe = "5^";
                    }
                    else
                    { keyThe = ""; }
                    if (chkFri.Checked)
                    {
                        keyFri = "6^";
                    }
                    else
                    { keyFri = ""; }
                    if (chkSat.Checked)
                    {
                        keySat = "7^";
                    }
                    else
                    { keySat = ""; }
                }
                else
                {
                    targetingHomeModel.TgtWeekYn = "N";
                    keySun = "";
                    keyMon = "";
                    keyThu = "";
                    keyWed = "";
                    keyThe = "";
                    keyFri = "";
                    keySat = "";
                }

                #region 셋탑설정
                if (chkStbModel.Checked)
                {
                    targetingHomeModel.TgtStbModelYn = "Y";
                    targetingHomeModel.TgtStbModel = GetAllNodeCheckedTag(tvStb.Nodes[0], "^");

                    if (targetingHomeModel.TgtStbModel.Length > 1 && targetingHomeModel.TgtStbModelYn.Equals("Y"))
                    {
                        // 마지막 구분자는 생략
                        targetingHomeModel.TgtStbModel = targetingHomeModel.TgtStbModel.Substring(0, targetingHomeModel.TgtStbModel.Length - 1).Replace("-","");
                    }
                    else
                    {
                        MessageBox.Show("셋탑모델이 선택되지 않았습니다.", "셋탑모델설정 오류", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                }
                else
                {
                    targetingHomeModel.TgtStbModelYn = "N";
                    targetingHomeModel.TgtStbModel = "";
                }
                #endregion

                #region POC설정
                if (chkPoc.Checked)
                {
                    targetingHomeModel.TgtPocYn = "Y";
                    targetingHomeModel.TgtPoc = GetAllNodeCheckedTag(tvPoc.Nodes[0], "^");

                    if (targetingHomeModel.TgtPoc.Length > 1 && targetingHomeModel.TgtPocYn.Equals("Y"))
                    {
                        // 마지막 구분자는 생략
                        targetingHomeModel.TgtPoc = targetingHomeModel.TgtPoc.Substring(0, targetingHomeModel.TgtPoc.Length - 1);
                    }
                    else
                    {
                        MessageBox.Show("POC설정이 선택되지 않았습니다.", "POC설정 오류", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                }
                else
                {
                    targetingHomeModel.TgtPocYn = "N";
                    targetingHomeModel.TgtPoc = "";
                }
                #endregion

                //체크박스의 값들을 string으로 결합하여 Model에 담음..
                targetingHomeModel.TgtWeek = keySun.ToString() + keyMon.ToString() + keyThu.ToString() + keyWed.ToString() +
                    keyThe.ToString() + keyFri.ToString() + keySat.ToString();
                //Model에 담긴 값을 Substring으로 마지막 한자리를 자른다..이유는 하나의 값뒤에 붙어있는 '^'를 제거하기 위해..
                //ex)DB에 저장된 마지막 값에는 항상'^'붙어있기 때문에..
                if (targetingHomeModel.TgtWeekYn.Equals("Y"))
                {
                    targetingHomeModel.TgtWeek = targetingHomeModel.TgtWeek.Substring(0, targetingHomeModel.TgtWeek.Length - 1);
                }

                // 타겟팅 상세정보 저장 서비스를 호출한다.
                new TargetingHomeManager(systemModel, commonModel).SetTargetingDetailUpdate(targetingHomeModel);

                if (targetingHomeModel.ResultCD.Equals("0000"))
                {

                    StatusMessage("타겟팅 정보가 저장되었습니다.");

                    DisableButton();
                    //ResetDetailText();
                    SearchTargeting();
                }

            }
            catch (FrameException fe)
            {
                FrameSystem.showMsgForm("타겟팅내역 저장오류", new string[] { fe.ErrCode, fe.ResultMsg });
            }
            catch (Exception ex)
            {
                FrameSystem.showMsgForm("타겟팅내역 저장오류", new string[] { "", ex.Message });
            }
        }

        /// <summary>
        /// [추가]지역타겟팅 정보 TreeListView에 체크처리
        /// </summary>
        /// <param name="chkList"></param>
        private void SetTargetRegion(string[] chkList)
        {
            #region 수정 전(TreeView 사용할때)
            /*
            canChecking = false;	// 체크처리 이벤트에서 처리로직이 작동하지 않도록

            int chkCount = SetAllNodeTagCheck(tlvRegion.Nodes[0], chkList);

            if (tvRegionNodeCount == chkCount)
            {
                tlvRegion.Nodes[0].Checked = true;
            }
            else
            {
                tlvRegion.Nodes[0].Checked = false;
            }
            canChecking = true;	
            */
            #endregion

            if (chkList != null && chkList.Length > 0)
            {
                TreeListViewItem level_2 = null;

                for (int h = 0; h < chkList.Length; h++)  //지역정보 문자열 배열
                {
                    for (int i = 0; i < tlvRegion.Items[0].Items.Count; i++) // level 1(서울,경기...)
                    {
                        for (int j = 0; j < tlvRegion.Items[0].Items[i].Items.Count; j++) //level 2(안양시,광주시...,)
                        {
                            level_2 = tlvRegion.Items[0].Items[i].Items[j];

                            if (level_2.Items.Count > 0) // level 3단계 존재체크  
                            {
                                for (int k = 0; k < level_2.Items.Count; k++)  // 청주시의 구(상당구,흥덕구)..
                                {
                                    if (level_2.Items[k].Tag.ToString().Equals(chkList[h]))
                                    {
                                        level_2.Items[k].Checked = true;
                                        break;
                                    }
                                }
                            }
                            else
                            {
                                if (level_2.Tag.ToString().Equals(chkList[h])) // level 2단계만 존재
                                {
                                    level_2.Checked = true;
                                    break;
                                }
                            }
                        }
                    }
                }
                reCheckingRegion();
            }
        }

        /// <summary>
        /// [추가]지역타겟팅 체크리스트 중 체크한 리스트 다시 검수
        /// </summary>
        private void reCheckingRegion()
        {
            int nLevel_2 = 0;
            int nLevel_3 = 0;

            TreeListViewItem level_2 = null;
            // 서브 아이템이 모두 선택 되면
            // 상위 아이템도 함께 체킹되어야 하는데
            // 자동으로 되지 않아서 한 번 더 검수하는 것임.
            for (int i = 0; i < tlvRegion.Items[0].Items.Count; i++) // level 1(서울,경기...)
            {
                nLevel_2 = 0;
                for (int j = 0; j < tlvRegion.Items[0].Items[i].Items.Count; j++) //level 2(안양시,광주시...,)
                {
                    level_2 = tlvRegion.Items[0].Items[i].Items[j];

                    if (level_2.Items.Count > 0) // level 3단계 존재체크  
                    {
                        nLevel_3 = 0;
                        for (int k = 0; k < level_2.Items.Count; k++)  // 청주시의 구(상당구,흥덕구)..
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
                }

                if (nLevel_2 == tlvRegion.Items[0].Items[i].Items.Count)
                    tlvRegion.Items[0].Items[i].Checked = true;
            }
        }

        /// <summary>
        /// [추가]체킹한 지역 타겟팅 문자열 가져오기
        /// </summary>
        private string getTargetRegion()
        {
            /*
             * 지역은 레벨2~3단계 tag(문자열) 값만 취한다.
             * - 노드의 부모 값은 필요치 않음.
            */
            string region = "";
            try
            {
                TreeListViewItem level_2 = null;

                for (int i = 0; i < tlvRegion.Items[0].Items.Count; i++)
                {
                    for (int j = 0; j < tlvRegion.Items[0].Items[i].Items.Count; j++)
                    {
                        level_2 = tlvRegion.Items[0].Items[i].Items[j];
                        if (level_2.Items.Count > 0)
                        {
                            for (int k = 0; k < level_2.Items.Count; k++)
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

                // 마지막 구분자 제외처리
                region = region.Substring(0, region.Length - 1);
            }
            catch (Exception ex)
            {
                Trace.Write(ex.Message);
            }

            return region;
        }

        /// <summary>
        /// [추가]지역 타겟팅 TreeViewList 초기화
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
                    for (int j = 0; j < tlvRegion.Items[0].Items[i].Items.Count; j++)
                    {
                        level_2 = tlvRegion.Items[0].Items[i].Items[j];
                        level_2.Checked = false;
                        if (level_2.Items.Count > 0)
                        {
                            for (int k = 0; k < level_2.Items.Count; k++)
                                level_2.Items[k].Checked = false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 상세내역 Text 초기화
        /// </summary>
        private void ResetDetailText()
        {
            // 노출물량
            ebContractAmt.Text = "0";

            // 빈도
            cbPriorityCd.SelectedIndex = 4;

            // 제어여부
            rbControlYn_Y.Checked = true;
            rbControlYn_N.Checked = false;

            // 제어비율
            udControlRate.Value = 100;

            // 노출지역 초기화 
            chkTimeYn.Checked = false;
            clearCheckingRegion();
            //SetTargetRegion(null);

            // 노출시간대 초기화
            chkRegionYn.Checked = false;
            SetTargetTime(null);

            // 노출연령대 초기화
            chkAgeYn.Checked = false;
            SetTargetAge(null);

            // 노출연령구간
            chkAgeBtnYn.Checked = false;
            //			udAgeBtnBegin.Value = 0;		
            //			udAgeBtnEnd.Value   = 0;	

            // 노출성별
            chkSexYn.Checked = false;

            chkTimeYn.Checked = false;
            chkTime00.Checked = false;
            chkTime01.Checked = false;

            // 노출등급
            //			chkRateYn.Checked  = false;
            //			rbRateAll.Checked  = true;
            //			rbRate12.Checked   = false;
            //			rbRate15.Checked   = false;
            //			rbRate19.Checked   = false;

            // 요일별
            chkWeekYn.Checked = false;
            chkMon.Checked = false;
            chkThu.Checked = false;
            chkWed.Checked = false;
            chkThe.Checked = false;
            chkFri.Checked = false;
            chkSat.Checked = false;
            chkSun.Checked = false;

            // 셋탑모델별 [E_08]
            chkStbModel.Checked = false;
            //grpStb.Enabled = false;
            SetTargetStb(null);
        }

        #endregion

        #region 이벤트함수

        private void StatusMessage(string strMessage)
        {
            if (StatusEvent != null)
            {
                StatusEventArgs ea = new StatusEventArgs();
                ea.Message = strMessage;
                StatusEvent(this, ea);
            }
        }

        private void ProgressStart()
        {
            if (ProgressEvent != null)
            {
                ProgressEventArgs ea = new ProgressEventArgs();
                ea.Type = ProgressEventArgs.Start;
                ProgressEvent(this, ea);
            }
        }

        private void ProgressStop()
        {
            if (ProgressEvent != null)
            {
                ProgressEventArgs ea = new ProgressEventArgs();
                ea.Type = ProgressEventArgs.Stop;
                ProgressEvent(this, ea);
            }
        }
        #endregion

        #region 엑셀 출력
        /// <summary>
        /// 엑셀 생성
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnExcel_Click(object sender, System.EventArgs e)
        {

            Excel.Application xlApp = null;
            Excel._Workbook oWB = null;
            Excel._Worksheet oSheet = null;
            Excel.Range oRng = null;

            try
            {
                int ColMax = 12; // 컬럼수   				

                int TitleRow = 1;
                int ConditionRow1 = 2;
                int HeaderRow = 3;
                string StartCol = "A";
                string EndCol = "";
                string TitleCol = "L";
                int CondCount = 0;
                int HeaderCount = 0;

                // 마지막 컬럼의 인덱스문자
                EndCol = GetColumnIndex(ColMax);

                xlApp = new Excel.Application();
                oWB = (Excel._Workbook)(xlApp.Workbooks.Add(Missing.Value));
                oSheet = (Excel._Worksheet)oWB.ActiveSheet;


                // 타이틀 작성
                oSheet.Cells[TitleRow, 1] = "광고내역목록";
                oRng = oSheet.get_Range(StartCol + Convert.ToString(TitleRow), TitleCol + Convert.ToString(TitleRow));
                oRng.Merge(true);
                oRng.Font.Bold = true;
                oRng.Font.Size = 16;
                oRng.VerticalAlignment = Excel.XlVAlign.xlVAlignCenter;
                oRng.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;

                // 조건정보 작성
                oSheet.Cells[ConditionRow1 + CondCount, 1] = "광고상태";
                oRng = oSheet.get_Range("B" + Convert.ToString(ConditionRow1 + CondCount), TitleCol + Convert.ToString(ConditionRow1 + CondCount));
                oRng.Merge(true);

                if (chkAdState_20.Checked)
                {
                    oSheet.Cells[ConditionRow1 + CondCount, 2] = "편성";
                }
                if (chkAdState_30.Checked)
                {
                    oSheet.Cells[ConditionRow1 + CondCount, 2] = "중지";
                }
                if (chkAdState_40.Checked)
                {
                    oSheet.Cells[ConditionRow1 + CondCount, 2] = "종료";
                }
                if (chkAdState_20.Checked && chkAdState_30.Checked)
                {
                    oSheet.Cells[ConditionRow1 + CondCount, 2] = "편성, 중지";
                }
                if (chkAdState_20.Checked && chkAdState_40.Checked)
                {
                    oSheet.Cells[ConditionRow1 + CondCount, 2] = "편성, 종료";
                }
                if (chkAdState_30.Checked && chkAdState_40.Checked)
                {
                    oSheet.Cells[ConditionRow1 + CondCount, 2] = "중지, 종료";
                }
                if (chkAdState_20.Checked && chkAdState_30.Checked && chkAdState_40.Checked)
                {
                    oSheet.Cells[ConditionRow1 + CondCount, 2] = "편성, 중지, 종료";
                }
                // 조건부 테두리
                oRng = oSheet.get_Range(StartCol + Convert.ToString(ConditionRow1), TitleCol + Convert.ToString(ConditionRow1 + (CondCount - 1)));
                oRng.VerticalAlignment = Excel.XlVAlign.xlVAlignCenter;
                oRng.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;

                oRng.Borders.LineStyle = Excel.XlLineStyle.xlContinuous;	// 테두리속성 아래에 실선
                oRng.Borders.Weight = Excel.XlBorderWeight.xlThin;		// 테두리속성 아래에 가는선

                CondCount++;

                // 헤더 정보 작성
                HeaderCount = 1;
                oSheet.Cells[HeaderRow, HeaderCount++] = "광고번호";
                //				oRng = oSheet.get_Range(GetColumnIndex(1)+Convert.ToString(HeaderRow), GetColumnIndex(1)+Convert.ToString(HeaderRow));
                //				oRng.Merge(true);
                //				HeaderCount++;

                oSheet.Cells[HeaderRow, HeaderCount++] = "광고명";
                oSheet.Cells[HeaderRow, HeaderCount++] = "계약상태";
                oSheet.Cells[HeaderRow, HeaderCount++] = "집행시작일";
                oSheet.Cells[HeaderRow, HeaderCount++] = "집행종료일";
                oSheet.Cells[HeaderRow, HeaderCount++] = "실제종료일";
                oSheet.Cells[HeaderRow, HeaderCount++] = "광고종류";
                oSheet.Cells[HeaderRow, HeaderCount++] = "광고상태";
                oSheet.Cells[HeaderRow, HeaderCount++] = "계약물량";
                oSheet.Cells[HeaderRow, HeaderCount++] = "노출빈도";
                oSheet.Cells[HeaderRow, HeaderCount++] = "계약명";
                oSheet.Cells[HeaderRow, HeaderCount++] = "광고주";

                oRng = oSheet.get_Range(StartCol + Convert.ToString(HeaderRow), EndCol + Convert.ToString(HeaderRow)); // 헤더의 범위
                oRng.Font.Bold = true;							// 폰트 굵게
                oRng.VerticalAlignment = Excel.XlVAlign.xlVAlignCenter;	// 세로중앙정렬
                oRng.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;	// 가로중앙정렬	 
                oRng.Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.CornflowerBlue);   //셀 배경색 
                oRng.Font.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.White);	  //텍스트색			

                string[,] items = new string[targetingHomeDs.Tables[0].Rows.Count, 12];
                // 데이터 추출
                for (int inx = 0; inx < targetingHomeDs.Tables[0].Rows.Count; inx++)
                {
                    items[inx, 0] = targetingHomeDs.Tables[0].Rows[inx]["ItemNo"].ToString();
                    items[inx, 1] = targetingHomeDs.Tables[0].Rows[inx]["ItemName"].ToString();
                    items[inx, 2] = targetingHomeDs.Tables[0].Rows[inx]["ContStateName"].ToString();
                    items[inx, 3] = targetingHomeDs.Tables[0].Rows[inx]["ExcuteStartDay"].ToString();
                    items[inx, 4] = targetingHomeDs.Tables[0].Rows[inx]["ExcuteEndDay"].ToString();
                    items[inx, 5] = targetingHomeDs.Tables[0].Rows[inx]["RealEndDay"].ToString();
                    items[inx, 6] = targetingHomeDs.Tables[0].Rows[inx]["AdTypeName"].ToString();

                    items[inx, 7] = targetingHomeDs.Tables[0].Rows[inx]["AdStateName"].ToString();
                    items[inx, 8] = targetingHomeDs.Tables[0].Rows[inx]["TgtAmount"].ToString();
                    items[inx, 9] = targetingHomeDs.Tables[0].Rows[inx]["PriorityCd"].ToString();
                    items[inx, 10] = targetingHomeDs.Tables[0].Rows[inx]["ContractName"].ToString();
                    items[inx, 11] = targetingHomeDs.Tables[0].Rows[inx]["AdvertiserName"].ToString();

                }
                oSheet.get_Range("A4", "L" + Convert.ToString((items.Length / 12) + 3)).set_Value(Missing.Value, items);

                oRng = oSheet.get_Range(StartCol + Convert.ToString(HeaderRow), EndCol + Convert.ToString(targetingHomeDs.Tables[0].Rows.Count));	// 데이터의 범위
                oRng.EntireColumn.AutoFit();					// 데이터의 크기에 셀의 가로크기 맞춤

                oRng.Borders.LineStyle = Excel.XlLineStyle.xlContinuous;	// 테두리속성 아래에 실선
                oRng.Borders.Weight = Excel.XlBorderWeight.xlThin;		// 테두리속성 아래에 가는선

                oRng = oSheet.get_Range("B2", "Q2");
                oRng.EntireColumn.AutoFit();

                xlApp.Visible = true;
                xlApp.UserControl = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private string GetColumnIndex(int ColCount)
        {
            string[] ColName = { "Z", "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y" };

            string ColumnIndex;

            // 26보다 크면
            if (ColCount > ColName.Length)
            {
                // 2자리 인덱스문자 26 => Z;  27->AA
                ColumnIndex = ColName[(int)(Math.Floor((float)(ColCount / ColName.Length)))] + ColName[(int)(Math.Floor((float)(ColCount % ColName.Length)))];
            }
            else
            {
                ColumnIndex = ColName[(int)(Math.Floor((float)(ColCount % ColName.Length)))];
            }

            return ColumnIndex;
        }

        #endregion

        #region TreeView 관련

        private void ChangeNodeCheck(TreeNode nowNode)
        {
            TreeNode parentNode = nowNode.Parent; // 부모노드

            if (parentNode != null)
            {

                int nodeCount = parentNode.GetNodeCount(false);	// 부모노드의 자식노드수 = 형제노드의 수, 하위노드는 포함안함 
                int checkedCount = 0;

                // 형제노드들에서 체크된 수를 얻는다.
                foreach (TreeNode childNode in parentNode.Nodes)
                {
                    if (childNode.Checked) checkedCount++;
                }

                // 모두 체크가 되었다면
                if (nodeCount == checkedCount)
                {
                    parentNode.Checked = true; // 부모노드의 체크를 true;
                }
                else
                {
                    parentNode.Checked = false; // 부모노드의 체크를 false;
                }
            }

            // 모든 자식노드에 대하여 체크를 변경한다. 
            bool chk = nowNode.Checked;
            CheckAllNode(nowNode, chk);
        }

        // 하위의 모든 노드의 체크를 변경한다.
        private void CheckAllNode(TreeNode node, bool chk)
        {
            node.Checked = chk;
            foreach (TreeNode childNode in node.Nodes)
            {
                CheckAllNode(childNode, chk);
            }
        }

        //하위노드를 포함하는 Tag를 구분자로 구분하여 반환한다.
        private string GetAllNodeCheckedTag(TreeNode node, string dilm)
        {
            string tag = "";

            if (node.Checked)
            {
                if (node.Tag != null)
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


        // 코드가 체크리스트에 존재하는지 검사
        private bool IsCheckedCode(string Code, string[] chkList)
        {
            if (chkList == null) return false;

            for (int i = 0; i < chkList.Length; i++)
            {
                if (Code.Replace("-", "").Equals(chkList[i]))
                {
                    return true;
                }
            }
            return false;
        }


        // 하위노드를 포함하여 Tag가 체크리스트에 있으면 Node를 체크한다.
        private int SetAllNodeTagCheck(TreeNode node, string[] chkList)
        {
            int cnt = 0;

            string tag = "";


            if (node.Tag != null)
            {
                tag = node.Tag.ToString();

                // 현재노드의 값이 체크리스트에 존재하는지 검사
                if (IsCheckedCode(tag, chkList))
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

            // 하위노드 검사
            foreach (TreeNode childNode in node.Nodes)
            {
                cnt += SetAllNodeTagCheck(childNode, chkList);
            }
            return cnt;
        }

        #endregion

        #region 고객군타겟팅 조회 및 해제
        private void SetTargetingCollectionList()
        {

#if (DEBUG)
            Debug.WriteLine(MethodBase.GetCurrentMethod().Name);
#endif
            ResetTargetingCollection();

            int curRow = cm.Position;

            if (curRow >= 0)
            {
                // 데이터모델 초기화
                targetingHomeModel.Init();
                targetingHomeModel.ItemNo = keyItemNo;

                new TargetingHomeManager(systemModel, commonModel).GetTargetingCollectionList(targetingHomeModel);

                if (targetingHomeModel.ResultCD.Equals("0000"))
                {
                    Utility.SetDataTable(targetingHomeDs.TargetingCollection, targetingHomeModel.TargetingCollectionDataSet);
                }
            }

            // 타겟팅설정이 되어 있다면 고객군타겟팅설정 추가/삭제 버튼을 활성화 시킨다.
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
            // 대상고객군의 체크 클리어
            grdExSouceCollectionList.UnCheckAllRecords();

        }

        // 추가~
        private void btnAddCollection_Click(object sender, EventArgs e)
        {
            this.AddCollection("+");
        }

        private void btnAddCollectionMinus_Click(object sender, EventArgs e)
        {
            this.AddCollection("-");
        }

        private void AddCollection(string collDirt)
        {
            int MAX_COUNT = 20; // 최대 20개까지 설정이 가능하다. 추후 이건 설정파일로 이동하여야 할 듯.

            // 그리드에 변경된 데이터를 Datasource에 업데이트 한다.
            grdExSouceCollectionList.UpdateData();

            try
            {
                int SettedCount = 0;
                int SetCount = 0;
                int CheckCount = 0;

                // 설정하려 선택된 고객군의 수
                CheckCount = grdExSouceCollectionList.GetCheckedRows().Length;

                if (CheckCount == 0)
                {
                    MessageBox.Show("선택한 고객군이 없습니다.", "고객군타겟팅 추가", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                // 현재 설정되어있는 고객군의 수
                SettedCount = grdExTargetList.GetRows().Length;

                foreach (GridEXRow gr in grdExSouceCollectionList.GetCheckedRows())
                {
                    if ((SettedCount + SetCount) >= MAX_COUNT)  // 최대치 이상인가?
                    {
                        MessageBox.Show("광고당 최대 " + MAX_COUNT.ToString() + "개의 고객군이 설정 가능합니다.", "고객군타겟팅 추가", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        break;
                    }

                    // 이미 고객군에 설정되어있는지 검사
                    bool isExist = false;
                    foreach (GridEXRow row in grdExTargetList.GetRows())
                    {
                        if (gr.Cells["Code"].Value.ToString().Equals(row.Cells["Code"].Value.ToString()))
                        {

                            MessageBox.Show("[" + gr.Cells["CollectionName"].Value.ToString() + "]은(는) 이미 타겟팅에 설정되어 있습니다.", "고객군타겟팅 추가", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            isExist = true;
                            break;
                        }
                    }

                    // 설정되어있는 고객군이 아닌경우에 추가
                    if (!isExist)
                    {
                        targetingHomeModel.Init();
                        targetingHomeModel.ItemNo = keyItemNo;
                        targetingHomeModel.CollectionCode = gr.Cells["Code"].Value.ToString();
                        targetingHomeModel.TgtCollectionYn = collDirt;

                        new TargetingHomeManager(systemModel, commonModel).SetTargetingCollectionAdd(targetingHomeModel);

                        if (targetingHomeModel.ResultCD.Equals("0000"))
                        {
                            SetCount++;
                        }
                    }

                    gr.IsChecked = false;  // 선택해제

                }

                SetTargetingCollectionList();


            }
            catch (FrameException fe)
            {
                FrameSystem.showMsgForm("고객군타겟팅 추가 오류", new string[] { fe.ErrCode, fe.ResultMsg });
            }
            catch (Exception ex)
            {
                FrameSystem.showMsgForm("고객군타겟팅 추가 오류", new string[] { "", ex.Message });
            }
        }


        private void btnDeleteCollection_Click(object sender, EventArgs e)
        {

            // 그리드에 변경된 데이터를 Datasource에 업데이트 한다.
            grdExTargetList.UpdateData();

            try
            {
                int SetCount = 0;

                foreach (GridEXRow gr in grdExTargetList.GetCheckedRows())
                {
                    targetingHomeModel.Init();

                    targetingHomeModel.ItemNo = keyItemNo;

                    targetingHomeModel.CollectionCode = gr.Cells["Code"].Value.ToString();

                    new TargetingHomeManager(systemModel, commonModel).SetTargetingCollectionDelete(targetingHomeModel);

                    if (targetingHomeModel.ResultCD.Equals("0000"))
                    {
                        SetCount++;
                    }

                    gr.IsChecked = false;  // 선택해제
                }

                SetTargetingCollectionList();

                if (SetCount == 0)
                {
                    MessageBox.Show("선택한 고객군이 없습니다.", "고객군타겟팅 삭제",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

            }
            catch (FrameException fe)
            {
                FrameSystem.showMsgForm("고객군타겟팅 추가 오류", new string[] { fe.ErrCode, fe.ResultMsg });
            }
            catch (Exception ex)
            {
                FrameSystem.showMsgForm("고객군타겟팅 추가 오류", new string[] { "", ex.Message });
            }


        }

        #endregion

        private void TargetingHomeControl_Load(object sender, EventArgs e)
        {
            // 데이터관리용 객체생성
            dt = ((DataView)grdExScheduleList.DataSource).Table;
            cm = (CurrencyManager)this.BindingContext[grdExScheduleList.DataSource];
            cm.PositionChanged += new System.EventHandler(OnGrdRowChanged);

            // 컨트롤 초기화
            InitControl();
        }

        private void tvStb_AfterCheck(object sender, TreeViewEventArgs e)
        {
            if (canChecking)
            {
                try
                {
                    canChecking = false;
                    ChangeNodeCheck(e.Node);	// 체크가 바뀐 노드
                }
                finally
                {
                    canChecking = true;
                }
            }
        }

    }
}
