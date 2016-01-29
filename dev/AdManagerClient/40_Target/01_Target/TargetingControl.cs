// ===============================================================================
// TargetingControl for Charites Project
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
 * Class Name: TargetingControl
 * �ֿ���  : ������� Ÿ����
 * �ۼ���    : ��
 * �ۼ���    : ��
 * Ư�̻���  : �������� Ȯ���� ���� ��� �߰� -bae
 * -------------------------------------------------------
 * [��������]
 * -------------------------------------------------------
 * �����ڵ�  : [E_01]
 * ������    : bae
 * ������    : 2010.07.28
 * ��������  :        
 *            - �������� ����Ʈ(Ʈ��) ���� ����
 *              2�� �������� 3�� ����.
 * �����Լ�  :
 *            - GetAllNodeCheckedTag(..)  
 *            - Init_tvRegion()
 * --------------------------------------------------------
 * �����ڵ�  : [E_02]
 * ������    : bae
 * ������    : 2010.08.13
 * �����κ�  :
 *            - ebZipCode_ButtonClick(..)
 * ��������  : 
 *            - �����ȣ ��Ƽ ���� �����ϵ��� ��.
 * --------------------------------------------------------
 * �����ڵ�  : [E_03]
 * ������    : bae
 * ������    : 2010.08.25
 * �����κ�  :
 *            - string strTimes ��� ���� �߰�.
 *              setTimesBinding(), checkingWeeknTime(), getDayOfNum()
 *              getTargetTimes(), rbWeekAll_Click(), rbWeekDay_Click()
 *              rbWeekEnd_Click(), rbWeek_Click()
 *  
 *            - InitCombo()
 * ��������  : 
 *            - ����ð� ���/����/�ָ��� �����ؼ� ���
 *              
 *            - AdTargetsService������ �� ù���ڸ� üũ�Ѵ�.
 *             ���� ������ �����ؼ� �ش������ ���� ���� ����,�ָ� ���о���(���)���� ó��
 * --------------------------------------------------------
 * �����ڵ�  : [E_04]
 * ������    : bae
 * ������    : 2010.09.03
 * �����κ�  :
 *				getTargetRegion(), setTargetRegion()
 *              clearCheckingRegion(),
 *            
 * ��������  : �������� Ÿ���� treeView �� TreeListView ����
 *             why? ���� ���� üũ �� �� ����(�θ�)�� üũ ���� 
 *                  ������ ǥ�� �� �Ǵ� ���� ��Ʈ�� ��ü.
 * -----------------------------------------------------------
 * �����ڵ�  : [E_05]
 * ������    : bae
 * ������    : 2010.10.04
 * ��������  :        
 *            - 2Slot ����
 *              
 * �����Լ�  :
 *            - chkSlotYn_CheckedChanged(..)�߰�
 *            - 
 * -----------------------------------------------------------
 * �����ڵ�  : [E_06]
 * ������    : Jang
 * ������    : 2012.01.20
 * ��������  : 
 *      1. �ش系���� ���� ����� �� ������ ����ؼ� �����ش�. ebContractTot
 *      1. ������ư �߰� - ������ �⺻�� ���������� ��忴����, ����ڰ� �������� �����ư�� Ŭ������ �ʴ� ������ �־, ��������� ������ư�� ������.
 *              
 * �����Լ�  :
 * --------------------------------------------------------  
 * �����ڵ�  : [E_07]
 * ������    : RH.Jung
 * ������    : 2012.02.14~
 * ��������  : 
 *      1. ���� Ÿ���� �� �߰� : �ټ�(�ִ�20��)�� ������ Ÿ���ÿ� ������ �� �ֵ��� ��
 * --------------------------------------------------------
 * �����ڵ�  : [E_08]
 * ������    : 2013.04.01
 * ��������  : 
 *      1. ��ž�� Ʈ���� �߰�
 * --------------------------------------------------------
 * �����ڵ�  : [E_09]
 * ������    : 2013.07.09
 * ��������  : 
 *      1. ��ȣ�� �����˾� ��� �߰� 
 * -------------------------------------------------------- 
 * �����ڵ�  : [E_10]
 * ������    : 2013.10.16
 * ��������  : 
 *      1. �������� Ÿ���� ��� �߰�
 * -------------------------------------------------------- 
 * �����ڵ�  : [E_11]
 * ������    : 2015.10.30
 * ��������  : 
 *      1. ������ ���� Ÿ���� ��ȸ �߰�
 * -------------------------------------------------------- 
 */

using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using AdManagerModel;
using WinFramework.Base;
using WinFramework.Data;
using WinFramework.Misc;
using Excel = Microsoft.Office.Interop.Excel; // ���� ����

using Janus.Windows.GridEX;

namespace AdManagerClient
{
    /// <summary>
    /// ���� Ÿ���� ���� ��Ʈ��
    /// </summary>
    public class TargetingControl : System.Windows.Forms.UserControl, IUserControl
    {
        #region �̺�Ʈ�ڵ鷯
        public event StatusEventHandler StatusEvent;			// �����̺�Ʈ �ڵ鷯
        public event ProgressEventHandler ProgressEvent;			// ó�����̺�Ʈ �ڵ鷯
        #endregion

        #region ��������� ��ü �� ����

        // �ý��� ���� : ȭ�����
        private SystemModel systemModel = FrameSystem.oSysModel;
        private CommonModel commonModel = FrameSystem.oComModel;
        private Logger log = FrameSystem.oLog;
        private MenuPower menu = FrameSystem.oMenu;

        // �޴��ڵ� : ������ �ʿ��� ȭ�鿡 �ʿ���
        public string menuCode = "";

        // ����� ������
        TargetingModel targetingModel = new TargetingModel();	// ���� Ÿ��������

        // ȭ��ó���� ����
        bool IsNewSearchKey = true;					// �˻����Է� ����
        CurrencyManager cm = null;					// ������ �׸����� ���濡 ���� �����ͼ� ������ ���Ͽ�			
        DataTable dt = null;

        bool canRead = false;
        bool canUpdate = false;

        bool IsSearching = false; // ��ȸ�� ��ȭ���� ������Ʈ �Ǵ� ���� ���� �ϱ����� 2011.11.29 JH.Park

        //private DataTable dtZip = null;

        /// <summary>
        /// Ÿ�� ������ �ð� ������ ���ڿ�[E_03]
        /// </summary>
        private string strTimes = string.Empty;

        // Ÿ���ü�������
        private bool isSettedTargeting = false;

        // Key ������
        string keyItemNo = "";
        string keyMediaCode = "";
        string keyItemName = "";
        string keyAdType = "";
        string keyCollectionYn = "N";

        string sTypeName = "";

        string keyMon = "";
        string keyThu = "";
        string keyWed = "";
        string keyThe = "";
        string keyFri = "";
        string keySat = "";
        string keySun = "";

        string RegionPreRow = "";
        string RegionNextRow = "";

        string TimePreRow = "";
        string TimeNextRow = "";

        string WeekPreRow = "";
        string WeekNextRow = "";


        // Ʈ�����
        private bool canChecking = true;

        //private int tvRegionNodeCount = 0;
        private int tvAgeNodeCount = 0;
        private int tvStbNodeCount = 0; // [E_08] ��ž�� Ʈ����

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

        private int rate1 = 0;
        private int rate2 = 0;
        private int rate3 = 0;
        private int rate4 = 0;
        private int rate5 = 0;
        private int rate6 = 0;
        private int rate7 = 0;
        private int rate8 = 0;
        private int rate9 = 0;
        private int rate10 = 0;
        private int rate11 = 0;
        private int rate12 = 0;
        private int rate13 = 0;
        private int rate14 = 0;
        private int rate15 = 0;
        private int rate16 = 0;
        private int rate17 = 0;
        private int rate18 = 0;
        private int rate19 = 0;
        private int rate20 = 0;
        private int rate21 = 0;
        private int rate22 = 0;
        private int rate23 = 0;
        private int rate24 = 0;
        private int tot1 = 0;
        private int tot2 = 0;
        private int tot3 = 0;
        private Label lblCollectionChk;
        private DevExpress.XtraEditors.GroupControl uiGroupBox2;
        private DevExpress.XtraEditors.TrackBarControl tbReliability;
        private DevExpress.XtraEditors.GroupControl grbProfileAge;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.MemoEdit memoEdit1;
        private Janus.Windows.EditControls.UICheckBox chkU03;
        private Janus.Windows.EditControls.UICheckBox chkU02;
        private Janus.Windows.EditControls.UICheckBox chkPPxOnly;

        string keyAge = ""; // [E_10] �������� Ÿ���� - ���ɴ� �� ����� ��� 

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
            //this.OnLoad(null);
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
        private Janus.Windows.UI.Tab.UITabPage uiTabPage2;
        private Janus.Windows.GridEX.EditControls.IntegerUpDown udControlRate;
        private Janus.Windows.GridEX.EditControls.NumericEditBox ebContractAmt;
        private System.Windows.Forms.Label lblContractAmt;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label7;
        private Janus.Windows.EditControls.UICheckBox chkWeekYn;
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
        private Janus.Windows.EditControls.UIGroupBox uiGroupBox4;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.Label label21;
        private System.Windows.Forms.Label label22;
        private System.Windows.Forms.Label label23;
        private System.Windows.Forms.Label label24;
        private Janus.Windows.EditControls.UIGroupBox uiGroupBox5;
        private System.Windows.Forms.Label label41;
        private System.Windows.Forms.Label label25;
        private System.Windows.Forms.Label label26;
        private System.Windows.Forms.Label label27;
        private System.Windows.Forms.Label label28;
        private System.Windows.Forms.Label label29;
        private System.Windows.Forms.Label label30;
        private System.Windows.Forms.Label label31;
        private System.Windows.Forms.Label label32;
        private System.Windows.Forms.Label label33;
        private System.Windows.Forms.Label label34;
        private System.Windows.Forms.Label label35;
        private System.Windows.Forms.Label label36;
        private System.Windows.Forms.Label label37;
        private System.Windows.Forms.Label label38;
        private System.Windows.Forms.Label label39;
        private System.Windows.Forms.Label label40;
        private System.Windows.Forms.Label label42;
        private System.Windows.Forms.Label label43;
        private System.Windows.Forms.Label label44;
        private System.Windows.Forms.Label label45;
        private System.Windows.Forms.Label label46;
        private System.Windows.Forms.Label label47;
        private Janus.Windows.EditControls.UIGroupBox uiGroupBox6;
        private System.Windows.Forms.Label label64;
        private System.Windows.Forms.Label label48;
        private System.Windows.Forms.Label label49;
        private System.Windows.Forms.Label label50;
        private System.Windows.Forms.Label label51;
        private System.Windows.Forms.Label label52;
        private System.Windows.Forms.Label label53;
        private Janus.Windows.GridEX.EditControls.IntegerUpDown udRegionRate1;
        private Janus.Windows.GridEX.EditControls.IntegerUpDown udRegionRate2;
        private Janus.Windows.GridEX.EditControls.IntegerUpDown udRegionRate3;
        private Janus.Windows.GridEX.EditControls.IntegerUpDown udRegionRate4;
        private Janus.Windows.GridEX.EditControls.IntegerUpDown udRegionRate5;
        private Janus.Windows.GridEX.EditControls.IntegerUpDown udRegionRate6;
        private Janus.Windows.GridEX.EditControls.IntegerUpDown udRegionRate7;
        private Janus.Windows.GridEX.EditControls.IntegerUpDown udRegionRate8;
        private Janus.Windows.GridEX.EditControls.IntegerUpDown udRegionRate9;
        private Janus.Windows.GridEX.EditControls.IntegerUpDown udRegionRate10;
        private Janus.Windows.GridEX.EditControls.IntegerUpDown udRegionRate11;
        private Janus.Windows.GridEX.EditControls.IntegerUpDown udRegionRate12;
        private Janus.Windows.GridEX.EditControls.IntegerUpDown udRegionRate13;
        private Janus.Windows.GridEX.EditControls.IntegerUpDown udRegionRate14;
        private Janus.Windows.GridEX.EditControls.IntegerUpDown udRegionRate15;
        private Janus.Windows.GridEX.EditControls.IntegerUpDown udRegionRate16;
        private Janus.Windows.GridEX.EditControls.IntegerUpDown udRegionRate17;
        private Janus.Windows.GridEX.EditControls.IntegerUpDown udTimeRate0;
        private Janus.Windows.GridEX.EditControls.IntegerUpDown udTimeRate1;
        private Janus.Windows.GridEX.EditControls.IntegerUpDown udTimeRate2;
        private Janus.Windows.GridEX.EditControls.IntegerUpDown udTimeRate3;
        private Janus.Windows.GridEX.EditControls.IntegerUpDown udTimeRate4;
        private Janus.Windows.GridEX.EditControls.IntegerUpDown udTimeRate5;
        private Janus.Windows.GridEX.EditControls.IntegerUpDown udTimeRate6;
        private Janus.Windows.GridEX.EditControls.IntegerUpDown udTimeRate7;
        private Janus.Windows.GridEX.EditControls.IntegerUpDown udTimeRate8;
        private Janus.Windows.GridEX.EditControls.IntegerUpDown udTimeRate9;
        private Janus.Windows.GridEX.EditControls.IntegerUpDown udTimeRate10;
        private Janus.Windows.GridEX.EditControls.IntegerUpDown udTimeRate11;
        private Janus.Windows.GridEX.EditControls.IntegerUpDown udTimeRate12;
        private Janus.Windows.GridEX.EditControls.IntegerUpDown udTimeRate13;
        private Janus.Windows.GridEX.EditControls.IntegerUpDown udTimeRate14;
        private Janus.Windows.GridEX.EditControls.IntegerUpDown udTimeRate15;
        private Janus.Windows.GridEX.EditControls.IntegerUpDown udTimeRate16;
        private Janus.Windows.GridEX.EditControls.IntegerUpDown udTimeRate17;
        private Janus.Windows.GridEX.EditControls.IntegerUpDown udTimeRate18;
        private System.Windows.Forms.Label label54;
        private Janus.Windows.GridEX.EditControls.IntegerUpDown udTimeRate20;
        private Janus.Windows.GridEX.EditControls.IntegerUpDown udTimeRate21;
        private Janus.Windows.GridEX.EditControls.IntegerUpDown udTimeRate22;
        private Janus.Windows.GridEX.EditControls.IntegerUpDown udTimeRate23;
        private Janus.Windows.GridEX.EditControls.IntegerUpDown udTimeRate19;
        private Janus.Windows.GridEX.EditControls.IntegerUpDown udMonRate;
        private Janus.Windows.GridEX.EditControls.IntegerUpDown udThuRate;
        private Janus.Windows.GridEX.EditControls.IntegerUpDown udWedRate;
        private Janus.Windows.GridEX.EditControls.IntegerUpDown udTheRate;
        private Janus.Windows.GridEX.EditControls.IntegerUpDown udFriRate;
        private Janus.Windows.GridEX.EditControls.IntegerUpDown udSatRate;
        private Janus.Windows.GridEX.EditControls.IntegerUpDown udSunRate;
        private Janus.Windows.EditControls.UIButton btnSaveRate;
        private System.Windows.Forms.Label label55;
        private System.Windows.Forms.Label label56;
        private System.Windows.Forms.Label label57;
        private Janus.Windows.GridEX.EditControls.NumericEditBox ebRegionTot;
        private Janus.Windows.GridEX.EditControls.NumericEditBox ebTimeTot;
        private Janus.Windows.GridEX.EditControls.NumericEditBox ebWeekTot;
        private Janus.Windows.EditControls.UIGroupBox grpRateType;
        private Janus.Windows.EditControls.UIRadioButton rbRateMinus;
        private Janus.Windows.EditControls.UIRadioButton rbRatePlus;
        private System.Windows.Forms.ToolTip ttMsg;
        private System.Windows.Forms.Label label58;
        private Janus.Windows.EditControls.UIGroupBox uiGroupBox3;
        private Janus.Windows.EditControls.UICheckBox chkZip;
        private System.Windows.Forms.Label lblZipName;
        private Janus.Windows.GridEX.EditControls.EditBox ebZipCode;
        private Janus.Windows.EditControls.UICheckBox chkPPx;
        private Janus.Windows.EditControls.UICheckBox chkFreq;
        private Janus.Windows.EditControls.UIGroupBox grpFreq;
        private Janus.Windows.GridEX.EditControls.IntegerUpDown freqDay;
        private Janus.Windows.GridEX.EditControls.IntegerUpDown freqPeriod;
        private System.Windows.Forms.Label label59;
        private System.Windows.Forms.Label label60;
        private Janus.Windows.EditControls.UICheckBox chkPVS;
        private Janus.Windows.EditControls.UIButton btnAddZip;
        private Janus.Windows.EditControls.UIButton btnNewZip;
        private System.Windows.Forms.RadioButton rbWeekDay;
        private System.Windows.Forms.RadioButton rbWeekEnd;
        private System.Windows.Forms.RadioButton rbWeekAll;
        private Janus.Windows.EditControls.UIGroupBox gbTime;
        private Janus.Windows.EditControls.UIGroupBox gbDays;
        private System.Windows.Forms.RadioButton rbWeek;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.TreeListView tlvRegion;
        private System.Windows.Forms.ImageList imgRegion;
        private Janus.Windows.EditControls.UICheckBox chkSlotYn;
        private Janus.Windows.EditControls.UIGroupBox grpSlot;
        private Janus.Windows.EditControls.UIRadioButton rbSlotBackward;
        private Janus.Windows.EditControls.UIGroupBox gbPost;
        private System.ComponentModel.IContainer components;
        private Janus.Windows.EditControls.UIRadioButton rbSlotForward;
        private Janus.Windows.EditControls.UIGroupBox grpAge;
        private Janus.Windows.EditControls.UIButton btnModify;
        private Janus.Windows.GridEX.EditControls.NumericEditBox ebContractTot;
        private Label lblContractSum;
        private Janus.Windows.EditControls.UIComboBox cbSearchAdType;
        private Janus.Windows.EditControls.UIButton btnCancel;
        private Janus.Windows.UI.Tab.UITabPage uiTabPage3;
        private Panel panel1;
        private Janus.Windows.EditControls.UIButton btnDeleteCollection;
        private Panel panel4;
        private Panel panel6;
        private Janus.Windows.GridEX.GridEX grdExSouceCollectionList;
        private Label label3;
        private Panel panel5;
        private DataView dvTargetingCollection;
        private Janus.Windows.EditControls.UICheckBox chkCollectionYn;
        private Label label63;
        private Label label62;
        private GridEX grdExScheduleList;
        private GridEX grdExTargetList;
        private Janus.Windows.EditControls.UIButton btnAddCollectionMinus;
        private Label lbMsg2;
        private Janus.Windows.EditControls.UIButton btnAddCollection;
        private Janus.Windows.EditControls.UIGroupBox grpStb;
        private TreeView tvStb;
        private Janus.Windows.EditControls.UICheckBox chkStbModel;
        private Janus.Windows.EditControls.UIGroupBox grpPreference;
        private Janus.Windows.EditControls.UICheckBox chkResponse;
        private Label label65;
        private Label label66;
        private Janus.Windows.GridEX.EditControls.IntegerUpDown udPrefRate;
        private Janus.Windows.EditControls.UICheckBox chkPreference;
        private Janus.Windows.UI.Tab.UITabPage uiTabPage4;
        private Janus.Windows.EditControls.UICheckBox chkProfileYn;
        private Janus.Windows.EditControls.UICheckBox chkF60;
        private Janus.Windows.EditControls.UICheckBox chkM60;
        private Janus.Windows.EditControls.UICheckBox chkF50;
        private Janus.Windows.EditControls.UICheckBox chkM50;
        private Janus.Windows.EditControls.UICheckBox chkF40;
        private Janus.Windows.EditControls.UICheckBox chkM40;
        private Janus.Windows.EditControls.UICheckBox chkF30;
        private Janus.Windows.EditControls.UICheckBox chkM30;
        private Janus.Windows.EditControls.UICheckBox chkF20;
        private Janus.Windows.EditControls.UICheckBox chKM20;
        private Janus.Windows.EditControls.UICheckBox chkF10;
        private Janus.Windows.EditControls.UICheckBox chkM10;
        private Janus.Windows.EditControls.UICheckBox chkU00;
        private Janus.Windows.EditControls.UIButton btnSaveProfile;
        private Label label67;
        private Label label5;

        public TargetingControl()
        {
            // �� ȣ���� Windows.Forms Form �����̳ʿ� �ʿ��մϴ�.
            InitializeComponent();
        }

        /// <summary> 
        /// ��� ���� ��� ���ҽ��� �����մϴ�.
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

        #region ���� ��� �����̳ʿ��� ������ �ڵ�
        /// <summary> 
        /// �����̳� ������ �ʿ��� �޼����Դϴ�. 
        /// �� �޼����� ������ �ڵ� ������� �������� ���ʽÿ�.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            Janus.Windows.EditControls.UIComboBoxItem uiComboBoxItem2 = new Janus.Windows.EditControls.UIComboBoxItem();
            Janus.Windows.GridEX.GridEXLayout grdExScheduleList_DesignTimeLayout = new Janus.Windows.GridEX.GridEXLayout();
            Janus.Windows.Common.Layouts.JanusLayoutReference grdExScheduleList_DesignTimeLayout_Reference_0 = new Janus.Windows.Common.Layouts.JanusLayoutReference("GridEXLayoutData.RootTable.FormatConditions.Condition1.FormatStyle.BackgroundImag" +
        "e");
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TargetingControl));
            Janus.Windows.Common.Layouts.JanusLayoutReference grdExScheduleList_DesignTimeLayout_Reference_1 = new Janus.Windows.Common.Layouts.JanusLayoutReference("GridEXLayoutData.RootTable.FormatConditions.Condition6.FormatStyle.BackgroundImag" +
        "e");
            Janus.Windows.Common.Layouts.JanusLayoutReference grdExScheduleList_DesignTimeLayout_Reference_2 = new Janus.Windows.Common.Layouts.JanusLayoutReference("GridEXLayoutData.RootTable.FormatConditions.Condition7.FormatStyle.BackgroundImag" +
        "e");
            Janus.Windows.Common.Layouts.JanusLayoutReference grdExScheduleList_DesignTimeLayout_Reference_3 = new Janus.Windows.Common.Layouts.JanusLayoutReference("GridEXLayoutData.RootTable.FormatConditions.Condition8.FormatStyle.BackgroundImag" +
        "e");
            Janus.Windows.Common.Layouts.JanusLayoutReference grdExScheduleList_DesignTimeLayout_Reference_4 = new Janus.Windows.Common.Layouts.JanusLayoutReference("GridEXLayoutData.RootTable.FormatConditions.Condition9.FormatStyle.BackgroundImag" +
        "e");
            Janus.Windows.Common.Layouts.JanusLayoutReference grdExScheduleList_DesignTimeLayout_Reference_5 = new Janus.Windows.Common.Layouts.JanusLayoutReference("GridEXLayoutData.FormatStyles.Style0.BackgroundImage");
            System.Windows.Forms.TreeNode treeNode9 = new System.Windows.Forms.TreeNode("��ž�𵨺�");
            System.Windows.Forms.TreeNode treeNode10 = new System.Windows.Forms.TreeNode("0~19��");
            System.Windows.Forms.TreeNode treeNode11 = new System.Windows.Forms.TreeNode("20~29��");
            System.Windows.Forms.TreeNode treeNode12 = new System.Windows.Forms.TreeNode("30~39��");
            System.Windows.Forms.TreeNode treeNode13 = new System.Windows.Forms.TreeNode("40~49��");
            System.Windows.Forms.TreeNode treeNode14 = new System.Windows.Forms.TreeNode("50~59��");
            System.Windows.Forms.TreeNode treeNode15 = new System.Windows.Forms.TreeNode("60���̻�");
            System.Windows.Forms.TreeNode treeNode16 = new System.Windows.Forms.TreeNode("��ü���ɴ�", new System.Windows.Forms.TreeNode[] {
            treeNode10,
            treeNode11,
            treeNode12,
            treeNode13,
            treeNode14,
            treeNode15});
            System.Windows.Forms.TreeListViewItemCollection.TreeListViewItemCollectionComparer treeListViewItemCollectionComparer2 = new System.Windows.Forms.TreeListViewItemCollection.TreeListViewItemCollectionComparer();
            Janus.Windows.GridEX.GridEXLayout grdExSouceCollectionList_DesignTimeLayout = new Janus.Windows.GridEX.GridEXLayout();
            Janus.Windows.GridEX.GridEXLayout grdExTargetList_DesignTimeLayout = new Janus.Windows.GridEX.GridEXLayout();
            this.targetingDs = new AdManagerClient.TargetingDs();
            this.uiPM = new Janus.Windows.UI.Dock.UIPanelManager(this.components);
            this.uiPanelChoiceAdSchedule = new Janus.Windows.UI.Dock.UIPanelGroup();
            this.uiPanelSearch = new Janus.Windows.UI.Dock.UIPanel();
            this.uiPanelSearchContainer = new Janus.Windows.UI.Dock.UIPanelInnerContainer();
            this.pnlSearch = new System.Windows.Forms.Panel();
            this.cbSearchAdType = new Janus.Windows.EditControls.UIComboBox();
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
            this.chkPPxOnly = new Janus.Windows.EditControls.UICheckBox();
            this.lblCollectionChk = new System.Windows.Forms.Label();
            this.imgRegion = new System.Windows.Forms.ImageList(this.components);
            this.chkProfileYn = new Janus.Windows.EditControls.UICheckBox();
            this.grpPreference = new Janus.Windows.EditControls.UIGroupBox();
            this.chkResponse = new Janus.Windows.EditControls.UICheckBox();
            this.label65 = new System.Windows.Forms.Label();
            this.label66 = new System.Windows.Forms.Label();
            this.udPrefRate = new Janus.Windows.GridEX.EditControls.IntegerUpDown();
            this.chkPreference = new Janus.Windows.EditControls.UICheckBox();
            this.grpStb = new Janus.Windows.EditControls.UIGroupBox();
            this.tvStb = new System.Windows.Forms.TreeView();
            this.chkStbModel = new Janus.Windows.EditControls.UICheckBox();
            this.chkCollectionYn = new Janus.Windows.EditControls.UICheckBox();
            this.btnCancel = new Janus.Windows.EditControls.UIButton();
            this.ebContractTot = new Janus.Windows.GridEX.EditControls.NumericEditBox();
            this.lblContractSum = new System.Windows.Forms.Label();
            this.btnModify = new Janus.Windows.EditControls.UIButton();
            this.grpAge = new Janus.Windows.EditControls.UIGroupBox();
            this.tvAge = new System.Windows.Forms.TreeView();
            this.chkSlotYn = new Janus.Windows.EditControls.UICheckBox();
            this.grpSlot = new Janus.Windows.EditControls.UIGroupBox();
            this.rbSlotForward = new Janus.Windows.EditControls.UIRadioButton();
            this.rbSlotBackward = new Janus.Windows.EditControls.UIRadioButton();
            this.tlvRegion = new System.Windows.Forms.TreeListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.gbTime = new Janus.Windows.EditControls.UIGroupBox();
            this.rbWeek = new System.Windows.Forms.RadioButton();
            this.rbWeekDay = new System.Windows.Forms.RadioButton();
            this.rbWeekAll = new System.Windows.Forms.RadioButton();
            this.rbWeekEnd = new System.Windows.Forms.RadioButton();
            this.chkPVS = new Janus.Windows.EditControls.UICheckBox();
            this.grpFreq = new Janus.Windows.EditControls.UIGroupBox();
            this.label60 = new System.Windows.Forms.Label();
            this.label59 = new System.Windows.Forms.Label();
            this.freqPeriod = new Janus.Windows.GridEX.EditControls.IntegerUpDown();
            this.freqDay = new Janus.Windows.GridEX.EditControls.IntegerUpDown();
            this.chkFreq = new Janus.Windows.EditControls.UICheckBox();
            this.chkPPx = new Janus.Windows.EditControls.UICheckBox();
            this.gbPost = new Janus.Windows.EditControls.UIGroupBox();
            this.btnAddZip = new Janus.Windows.EditControls.UIButton();
            this.btnNewZip = new Janus.Windows.EditControls.UIButton();
            this.ebZipCode = new Janus.Windows.GridEX.EditControls.EditBox();
            this.lblZipName = new System.Windows.Forms.Label();
            this.chkZip = new Janus.Windows.EditControls.UICheckBox();
            this.uiGroupBox3 = new Janus.Windows.EditControls.UIGroupBox();
            this.lbNotice = new System.Windows.Forms.Label();
            this.label58 = new System.Windows.Forms.Label();
            this.grpRateType = new Janus.Windows.EditControls.UIGroupBox();
            this.rbRateMinus = new Janus.Windows.EditControls.UIRadioButton();
            this.rbRatePlus = new Janus.Windows.EditControls.UIRadioButton();
            this.udControlRate = new Janus.Windows.GridEX.EditControls.IntegerUpDown();
            this.ebContractAmt = new Janus.Windows.GridEX.EditControls.NumericEditBox();
            this.lblContractAmt = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.chkWeekYn = new Janus.Windows.EditControls.UICheckBox();
            this.gbDays = new Janus.Windows.EditControls.UIGroupBox();
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
            this.grpRate = new Janus.Windows.EditControls.UIGroupBox();
            this.rbRate19 = new Janus.Windows.EditControls.UIRadioButton();
            this.rbRate15 = new Janus.Windows.EditControls.UIRadioButton();
            this.rbRate12 = new Janus.Windows.EditControls.UIRadioButton();
            this.rbRateAll = new Janus.Windows.EditControls.UIRadioButton();
            this.btnSave = new Janus.Windows.EditControls.UIButton();
            this.chkAgeYn = new Janus.Windows.EditControls.UICheckBox();
            this.chkSexYn = new Janus.Windows.EditControls.UICheckBox();
            this.grpSex = new Janus.Windows.EditControls.UIGroupBox();
            this.chkSexMan = new Janus.Windows.EditControls.UICheckBox();
            this.chkSexWoman = new Janus.Windows.EditControls.UICheckBox();
            this.chkAgeBtnYn = new Janus.Windows.EditControls.UICheckBox();
            this.udAgeBtnBegin = new Janus.Windows.GridEX.EditControls.IntegerUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.udAgeBtnEnd = new Janus.Windows.GridEX.EditControls.IntegerUpDown();
            this.rbControlYn_Y = new Janus.Windows.EditControls.UIRadioButton();
            this.rbControlYn_N = new Janus.Windows.EditControls.UIRadioButton();
            this.uiTabPage2 = new Janus.Windows.UI.Tab.UITabPage();
            this.btnSaveRate = new Janus.Windows.EditControls.UIButton();
            this.uiGroupBox4 = new Janus.Windows.EditControls.UIGroupBox();
            this.ebRegionTot = new Janus.Windows.GridEX.EditControls.NumericEditBox();
            this.label55 = new System.Windows.Forms.Label();
            this.udRegionRate17 = new Janus.Windows.GridEX.EditControls.IntegerUpDown();
            this.udRegionRate16 = new Janus.Windows.GridEX.EditControls.IntegerUpDown();
            this.udRegionRate15 = new Janus.Windows.GridEX.EditControls.IntegerUpDown();
            this.udRegionRate14 = new Janus.Windows.GridEX.EditControls.IntegerUpDown();
            this.udRegionRate13 = new Janus.Windows.GridEX.EditControls.IntegerUpDown();
            this.udRegionRate12 = new Janus.Windows.GridEX.EditControls.IntegerUpDown();
            this.udRegionRate11 = new Janus.Windows.GridEX.EditControls.IntegerUpDown();
            this.udRegionRate10 = new Janus.Windows.GridEX.EditControls.IntegerUpDown();
            this.udRegionRate9 = new Janus.Windows.GridEX.EditControls.IntegerUpDown();
            this.udRegionRate8 = new Janus.Windows.GridEX.EditControls.IntegerUpDown();
            this.udRegionRate7 = new Janus.Windows.GridEX.EditControls.IntegerUpDown();
            this.udRegionRate6 = new Janus.Windows.GridEX.EditControls.IntegerUpDown();
            this.udRegionRate5 = new Janus.Windows.GridEX.EditControls.IntegerUpDown();
            this.udRegionRate4 = new Janus.Windows.GridEX.EditControls.IntegerUpDown();
            this.udRegionRate3 = new Janus.Windows.GridEX.EditControls.IntegerUpDown();
            this.udRegionRate2 = new Janus.Windows.GridEX.EditControls.IntegerUpDown();
            this.udRegionRate1 = new Janus.Windows.GridEX.EditControls.IntegerUpDown();
            this.label24 = new System.Windows.Forms.Label();
            this.label23 = new System.Windows.Forms.Label();
            this.label22 = new System.Windows.Forms.Label();
            this.label21 = new System.Windows.Forms.Label();
            this.label20 = new System.Windows.Forms.Label();
            this.label19 = new System.Windows.Forms.Label();
            this.label18 = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.uiGroupBox5 = new Janus.Windows.EditControls.UIGroupBox();
            this.ebTimeTot = new Janus.Windows.GridEX.EditControls.NumericEditBox();
            this.label56 = new System.Windows.Forms.Label();
            this.udTimeRate19 = new Janus.Windows.GridEX.EditControls.IntegerUpDown();
            this.label54 = new System.Windows.Forms.Label();
            this.udTimeRate23 = new Janus.Windows.GridEX.EditControls.IntegerUpDown();
            this.udTimeRate22 = new Janus.Windows.GridEX.EditControls.IntegerUpDown();
            this.udTimeRate21 = new Janus.Windows.GridEX.EditControls.IntegerUpDown();
            this.udTimeRate20 = new Janus.Windows.GridEX.EditControls.IntegerUpDown();
            this.udTimeRate18 = new Janus.Windows.GridEX.EditControls.IntegerUpDown();
            this.udTimeRate17 = new Janus.Windows.GridEX.EditControls.IntegerUpDown();
            this.udTimeRate16 = new Janus.Windows.GridEX.EditControls.IntegerUpDown();
            this.udTimeRate15 = new Janus.Windows.GridEX.EditControls.IntegerUpDown();
            this.udTimeRate14 = new Janus.Windows.GridEX.EditControls.IntegerUpDown();
            this.udTimeRate13 = new Janus.Windows.GridEX.EditControls.IntegerUpDown();
            this.udTimeRate12 = new Janus.Windows.GridEX.EditControls.IntegerUpDown();
            this.udTimeRate11 = new Janus.Windows.GridEX.EditControls.IntegerUpDown();
            this.udTimeRate10 = new Janus.Windows.GridEX.EditControls.IntegerUpDown();
            this.udTimeRate9 = new Janus.Windows.GridEX.EditControls.IntegerUpDown();
            this.udTimeRate8 = new Janus.Windows.GridEX.EditControls.IntegerUpDown();
            this.udTimeRate7 = new Janus.Windows.GridEX.EditControls.IntegerUpDown();
            this.udTimeRate6 = new Janus.Windows.GridEX.EditControls.IntegerUpDown();
            this.udTimeRate5 = new Janus.Windows.GridEX.EditControls.IntegerUpDown();
            this.udTimeRate4 = new Janus.Windows.GridEX.EditControls.IntegerUpDown();
            this.udTimeRate3 = new Janus.Windows.GridEX.EditControls.IntegerUpDown();
            this.udTimeRate2 = new Janus.Windows.GridEX.EditControls.IntegerUpDown();
            this.udTimeRate1 = new Janus.Windows.GridEX.EditControls.IntegerUpDown();
            this.udTimeRate0 = new Janus.Windows.GridEX.EditControls.IntegerUpDown();
            this.label47 = new System.Windows.Forms.Label();
            this.label46 = new System.Windows.Forms.Label();
            this.label45 = new System.Windows.Forms.Label();
            this.label44 = new System.Windows.Forms.Label();
            this.label43 = new System.Windows.Forms.Label();
            this.label42 = new System.Windows.Forms.Label();
            this.label40 = new System.Windows.Forms.Label();
            this.label39 = new System.Windows.Forms.Label();
            this.label38 = new System.Windows.Forms.Label();
            this.label37 = new System.Windows.Forms.Label();
            this.label36 = new System.Windows.Forms.Label();
            this.label35 = new System.Windows.Forms.Label();
            this.label34 = new System.Windows.Forms.Label();
            this.label33 = new System.Windows.Forms.Label();
            this.label32 = new System.Windows.Forms.Label();
            this.label31 = new System.Windows.Forms.Label();
            this.label30 = new System.Windows.Forms.Label();
            this.label29 = new System.Windows.Forms.Label();
            this.label28 = new System.Windows.Forms.Label();
            this.label27 = new System.Windows.Forms.Label();
            this.label26 = new System.Windows.Forms.Label();
            this.label25 = new System.Windows.Forms.Label();
            this.label41 = new System.Windows.Forms.Label();
            this.uiGroupBox6 = new Janus.Windows.EditControls.UIGroupBox();
            this.ebWeekTot = new Janus.Windows.GridEX.EditControls.NumericEditBox();
            this.label57 = new System.Windows.Forms.Label();
            this.udSunRate = new Janus.Windows.GridEX.EditControls.IntegerUpDown();
            this.udSatRate = new Janus.Windows.GridEX.EditControls.IntegerUpDown();
            this.udFriRate = new Janus.Windows.GridEX.EditControls.IntegerUpDown();
            this.udTheRate = new Janus.Windows.GridEX.EditControls.IntegerUpDown();
            this.udWedRate = new Janus.Windows.GridEX.EditControls.IntegerUpDown();
            this.udMonRate = new Janus.Windows.GridEX.EditControls.IntegerUpDown();
            this.label53 = new System.Windows.Forms.Label();
            this.label52 = new System.Windows.Forms.Label();
            this.label51 = new System.Windows.Forms.Label();
            this.label50 = new System.Windows.Forms.Label();
            this.label49 = new System.Windows.Forms.Label();
            this.label48 = new System.Windows.Forms.Label();
            this.label64 = new System.Windows.Forms.Label();
            this.udThuRate = new Janus.Windows.GridEX.EditControls.IntegerUpDown();
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
            this.label3 = new System.Windows.Forms.Label();
            this.btnDeleteCollection = new Janus.Windows.EditControls.UIButton();
            this.uiTabPage4 = new Janus.Windows.UI.Tab.UITabPage();
            this.memoEdit1 = new DevExpress.XtraEditors.MemoEdit();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.grbProfileAge = new DevExpress.XtraEditors.GroupControl();
            this.chkU03 = new Janus.Windows.EditControls.UICheckBox();
            this.chkU02 = new Janus.Windows.EditControls.UICheckBox();
            this.chkF60 = new Janus.Windows.EditControls.UICheckBox();
            this.chkF10 = new Janus.Windows.EditControls.UICheckBox();
            this.chkM60 = new Janus.Windows.EditControls.UICheckBox();
            this.chkU00 = new Janus.Windows.EditControls.UICheckBox();
            this.chkF50 = new Janus.Windows.EditControls.UICheckBox();
            this.chkM10 = new Janus.Windows.EditControls.UICheckBox();
            this.chkM50 = new Janus.Windows.EditControls.UICheckBox();
            this.chKM20 = new Janus.Windows.EditControls.UICheckBox();
            this.chkF40 = new Janus.Windows.EditControls.UICheckBox();
            this.chkF20 = new Janus.Windows.EditControls.UICheckBox();
            this.chkM40 = new Janus.Windows.EditControls.UICheckBox();
            this.chkM30 = new Janus.Windows.EditControls.UICheckBox();
            this.chkF30 = new Janus.Windows.EditControls.UICheckBox();
            this.uiGroupBox2 = new DevExpress.XtraEditors.GroupControl();
            this.tbReliability = new DevExpress.XtraEditors.TrackBarControl();
            this.label5 = new System.Windows.Forms.Label();
            this.label67 = new System.Windows.Forms.Label();
            this.btnSaveProfile = new Janus.Windows.EditControls.UIButton();
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
            ((System.ComponentModel.ISupportInitialize)(this.grpPreference)).BeginInit();
            this.grpPreference.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grpStb)).BeginInit();
            this.grpStb.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grpAge)).BeginInit();
            this.grpAge.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grpSlot)).BeginInit();
            this.grpSlot.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gbTime)).BeginInit();
            this.gbTime.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grpFreq)).BeginInit();
            this.grpFreq.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gbPost)).BeginInit();
            this.gbPost.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uiGroupBox3)).BeginInit();
            this.uiGroupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grpRateType)).BeginInit();
            this.grpRateType.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gbDays)).BeginInit();
            this.gbDays.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grpRate)).BeginInit();
            this.grpRate.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grpSex)).BeginInit();
            this.grpSex.SuspendLayout();
            this.uiTabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uiGroupBox4)).BeginInit();
            this.uiGroupBox4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uiGroupBox5)).BeginInit();
            this.uiGroupBox5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uiGroupBox6)).BeginInit();
            this.uiGroupBox6.SuspendLayout();
            this.uiTabPage3.SuspendLayout();
            this.panel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdExSouceCollectionList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dvCollection)).BeginInit();
            this.panel6.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdExTargetList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dvTargetingCollection)).BeginInit();
            this.panel5.SuspendLayout();
            this.uiTabPage4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.memoEdit1.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grbProfileAge)).BeginInit();
            this.grbProfileAge.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uiGroupBox2)).BeginInit();
            this.uiGroupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tbReliability)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbReliability.Properties)).BeginInit();
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
            this.uiPanelChoiceAdSchedule.Text = "������� Ÿ������ ����";
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
            this.pnlSearch.Controls.Add(this.cbSearchAdType);
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
            // cbSearchAdType
            // 
            this.cbSearchAdType.AutoSize = false;
            this.cbSearchAdType.ComboStyle = Janus.Windows.EditControls.ComboStyle.DropDownList;
            this.cbSearchAdType.Location = new System.Drawing.Point(372, 11);
            this.cbSearchAdType.Name = "cbSearchAdType";
            this.cbSearchAdType.Size = new System.Drawing.Size(120, 20);
            this.cbSearchAdType.TabIndex = 12;
            this.cbSearchAdType.Text = "������������";
            this.cbSearchAdType.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            // 
            // chkAdState_20
            // 
            this.chkAdState_20.Checked = true;
            this.chkAdState_20.CheckedValue = "";
            this.chkAdState_20.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkAdState_20.Font = new System.Drawing.Font("���� ���", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.chkAdState_20.ForeColor = System.Drawing.SystemColors.ControlText;
            this.chkAdState_20.Location = new System.Drawing.Point(507, 11);
            this.chkAdState_20.Name = "chkAdState_20";
            this.chkAdState_20.Size = new System.Drawing.Size(44, 20);
            this.chkAdState_20.TabIndex = 8;
            this.chkAdState_20.Text = "��";
            this.chkAdState_20.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            // 
            // ebSearchKey
            // 
            this.ebSearchKey.Location = new System.Drawing.Point(647, 11);
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
            uiComboBoxItem2.FormatStyle.Alpha = 0;
            uiComboBoxItem2.IsSeparator = false;
            uiComboBoxItem2.Text = "��ε��TV";
            this.cbSearchMedia.Items.AddRange(new Janus.Windows.EditControls.UIComboBoxItem[] {
            uiComboBoxItem2});
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
            this.pnlSearchBtn.Location = new System.Drawing.Point(820, 0);
            this.pnlSearchBtn.Name = "pnlSearchBtn";
            this.pnlSearchBtn.Size = new System.Drawing.Size(188, 38);
            this.pnlSearchBtn.TabIndex = 11;
            // 
            // btnExcel
            // 
            this.btnExcel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExcel.Enabled = false;
            this.btnExcel.Location = new System.Drawing.Point(100, 8);
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
            this.btnSearch.Location = new System.Drawing.Point(14, 8);
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
            this.chkAdState_30.Location = new System.Drawing.Point(551, 11);
            this.chkAdState_30.Name = "chkAdState_30";
            this.chkAdState_30.Size = new System.Drawing.Size(44, 20);
            this.chkAdState_30.TabIndex = 9;
            this.chkAdState_30.Text = "����";
            this.chkAdState_30.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            // 
            // chkAdState_40
            // 
            this.chkAdState_40.Font = new System.Drawing.Font("���� ���", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.chkAdState_40.Location = new System.Drawing.Point(595, 11);
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
            grdExScheduleList_DesignTimeLayout_Reference_3.Instance = ((object)(resources.GetObject("grdExScheduleList_DesignTimeLayout_Reference_3.Instance")));
            grdExScheduleList_DesignTimeLayout_Reference_4.Instance = ((object)(resources.GetObject("grdExScheduleList_DesignTimeLayout_Reference_4.Instance")));
            grdExScheduleList_DesignTimeLayout_Reference_5.Instance = ((object)(resources.GetObject("grdExScheduleList_DesignTimeLayout_Reference_5.Instance")));
            grdExScheduleList_DesignTimeLayout.LayoutReferences.AddRange(new Janus.Windows.Common.Layouts.JanusLayoutReference[] {
            grdExScheduleList_DesignTimeLayout_Reference_0,
            grdExScheduleList_DesignTimeLayout_Reference_1,
            grdExScheduleList_DesignTimeLayout_Reference_2,
            grdExScheduleList_DesignTimeLayout_Reference_3,
            grdExScheduleList_DesignTimeLayout_Reference_4,
            grdExScheduleList_DesignTimeLayout_Reference_5});
            grdExScheduleList_DesignTimeLayout.LayoutString = resources.GetString("grdExScheduleList_DesignTimeLayout.LayoutString");
            this.grdExScheduleList.DesignTimeLayout = grdExScheduleList_DesignTimeLayout;
            this.grdExScheduleList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdExScheduleList.EmptyRows = true;
            this.grdExScheduleList.FocusCellFormatStyle.BackColor = System.Drawing.SystemColors.Highlight;
            this.grdExScheduleList.FocusCellFormatStyle.ForeColor = System.Drawing.Color.White;
            this.grdExScheduleList.FocusStyle = Janus.Windows.GridEX.FocusStyle.None;
            this.grdExScheduleList.Font = new System.Drawing.Font("�������", 8.5F);
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
            this.grdExScheduleList.TabIndex = 13;
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
            this.uiTab1.BackColor = System.Drawing.Color.Transparent;
            this.uiTab1.Location = new System.Drawing.Point(8, 8);
            this.uiTab1.Name = "uiTab1";
            this.uiTab1.Size = new System.Drawing.Size(992, 365);
            this.uiTab1.TabIndex = 194;
            this.uiTab1.TabPages.AddRange(new Janus.Windows.UI.Tab.UITabPage[] {
            this.uiTabPage1,
            this.uiTabPage2,
            this.uiTabPage3,
            this.uiTabPage4});
            this.uiTab1.TabsStateStyles.SelectedFormatStyle.FontBold = Janus.Windows.UI.TriState.True;
            this.uiTab1.TabStripAlignment = Janus.Windows.UI.Tab.TabStripAlignment.Left;
            this.uiTab1.VisualStyle = Janus.Windows.UI.Tab.TabVisualStyle.Office2007;
            // 
            // uiTabPage1
            // 
            this.uiTabPage1.Controls.Add(this.chkPPxOnly);
            this.uiTabPage1.Controls.Add(this.lblCollectionChk);
            this.uiTabPage1.Controls.Add(this.chkProfileYn);
            this.uiTabPage1.Controls.Add(this.grpPreference);
            this.uiTabPage1.Controls.Add(this.chkPreference);
            this.uiTabPage1.Controls.Add(this.grpStb);
            this.uiTabPage1.Controls.Add(this.chkStbModel);
            this.uiTabPage1.Controls.Add(this.chkCollectionYn);
            this.uiTabPage1.Controls.Add(this.btnCancel);
            this.uiTabPage1.Controls.Add(this.ebContractTot);
            this.uiTabPage1.Controls.Add(this.lblContractSum);
            this.uiTabPage1.Controls.Add(this.btnModify);
            this.uiTabPage1.Controls.Add(this.grpAge);
            this.uiTabPage1.Controls.Add(this.chkSlotYn);
            this.uiTabPage1.Controls.Add(this.grpSlot);
            this.uiTabPage1.Controls.Add(this.tlvRegion);
            this.uiTabPage1.Controls.Add(this.gbTime);
            this.uiTabPage1.Controls.Add(this.chkPVS);
            this.uiTabPage1.Controls.Add(this.grpFreq);
            this.uiTabPage1.Controls.Add(this.chkFreq);
            this.uiTabPage1.Controls.Add(this.chkPPx);
            this.uiTabPage1.Controls.Add(this.gbPost);
            this.uiTabPage1.Controls.Add(this.chkZip);
            this.uiTabPage1.Controls.Add(this.uiGroupBox3);
            this.uiTabPage1.Controls.Add(this.label58);
            this.uiTabPage1.Controls.Add(this.grpRateType);
            this.uiTabPage1.Controls.Add(this.udControlRate);
            this.uiTabPage1.Controls.Add(this.ebContractAmt);
            this.uiTabPage1.Controls.Add(this.lblContractAmt);
            this.uiTabPage1.Controls.Add(this.label2);
            this.uiTabPage1.Controls.Add(this.label7);
            this.uiTabPage1.Controls.Add(this.chkWeekYn);
            this.uiTabPage1.Controls.Add(this.gbDays);
            this.uiTabPage1.Controls.Add(this.chkRateYn);
            this.uiTabPage1.Controls.Add(this.chkTimeYn);
            this.uiTabPage1.Controls.Add(this.chkRegionYn);
            this.uiTabPage1.Controls.Add(this.grpRate);
            this.uiTabPage1.Controls.Add(this.btnSave);
            this.uiTabPage1.Controls.Add(this.chkAgeYn);
            this.uiTabPage1.Controls.Add(this.chkSexYn);
            this.uiTabPage1.Controls.Add(this.grpSex);
            this.uiTabPage1.Controls.Add(this.chkAgeBtnYn);
            this.uiTabPage1.Controls.Add(this.udAgeBtnBegin);
            this.uiTabPage1.Controls.Add(this.label4);
            this.uiTabPage1.Controls.Add(this.udAgeBtnEnd);
            this.uiTabPage1.Controls.Add(this.rbControlYn_Y);
            this.uiTabPage1.Controls.Add(this.rbControlYn_N);
            this.uiTabPage1.Icon = ((System.Drawing.Icon)(resources.GetObject("uiTabPage1.Icon")));
            this.uiTabPage1.Location = new System.Drawing.Point(23, 1);
            this.uiTabPage1.Name = "uiTabPage1";
            this.uiTabPage1.PanelFormatStyle.BackColor = System.Drawing.Color.Transparent;
            this.uiTabPage1.Size = new System.Drawing.Size(968, 363);
            this.uiTabPage1.TabStop = true;
            this.uiTabPage1.Text = "Ÿ����";
            // 
            // chkPPxOnly
            // 
            this.chkPPxOnly.BackColor = System.Drawing.Color.Transparent;
            this.chkPPxOnly.Location = new System.Drawing.Point(851, 39);
            this.chkPPxOnly.Name = "chkPPxOnly";
            this.chkPPxOnly.Size = new System.Drawing.Size(94, 20);
            this.chkPPxOnly.TabIndex = 259;
            this.chkPPxOnly.Text = "Only����ä��";
            this.ttMsg.SetToolTip(this.chkPPxOnly, "����ä�ο��� ���� ����˴ϴ�....");
            this.chkPPxOnly.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.chkPPxOnly.CheckedChanged += new System.EventHandler(this.onPPxChanged);
            // 
            // lblCollectionChk
            // 
            this.lblCollectionChk.BackColor = System.Drawing.Color.Transparent;
            this.lblCollectionChk.ImageIndex = 3;
            this.lblCollectionChk.ImageList = this.imgRegion;
            this.lblCollectionChk.Location = new System.Drawing.Point(844, 75);
            this.lblCollectionChk.Margin = new System.Windows.Forms.Padding(0);
            this.lblCollectionChk.Name = "lblCollectionChk";
            this.lblCollectionChk.Size = new System.Drawing.Size(26, 26);
            this.lblCollectionChk.TabIndex = 258;
            this.lblCollectionChk.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lblCollectionChk.Visible = false;
            // 
            // imgRegion
            // 
            this.imgRegion.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imgRegion.ImageStream")));
            this.imgRegion.TransparentColor = System.Drawing.Color.Transparent;
            this.imgRegion.Images.SetKeyName(0, "");
            this.imgRegion.Images.SetKeyName(1, "");
            this.imgRegion.Images.SetKeyName(2, "");
            this.imgRegion.Images.SetKeyName(3, "error-icon.png");
            this.imgRegion.Images.SetKeyName(4, "danger-icon.png");
            // 
            // chkProfileYn
            // 
            this.chkProfileYn.BackColor = System.Drawing.Color.Transparent;
            this.chkProfileYn.Font = new System.Drawing.Font("���� ���", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.chkProfileYn.Location = new System.Drawing.Point(711, 59);
            this.chkProfileYn.Name = "chkProfileYn";
            this.chkProfileYn.Size = new System.Drawing.Size(130, 20);
            this.chkProfileYn.TabIndex = 256;
            this.chkProfileYn.Text = "�������� Ÿ���� ���";
            this.ttMsg.SetToolTip(this.chkProfileYn, "��û�� �������� Ÿ������ ����մϴ�");
            this.chkProfileYn.Visible = false;
            this.chkProfileYn.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.chkProfileYn.CheckedChanged += new System.EventHandler(this.chkProfileYn_CheckedChanged);
            // 
            // grpPreference
            // 
            this.grpPreference.BackColor = System.Drawing.Color.Transparent;
            this.grpPreference.BorderColor = System.Drawing.Color.DimGray;
            this.grpPreference.Controls.Add(this.chkResponse);
            this.grpPreference.Controls.Add(this.label65);
            this.grpPreference.Controls.Add(this.label66);
            this.grpPreference.Controls.Add(this.udPrefRate);
            this.grpPreference.Location = new System.Drawing.Point(826, 207);
            this.grpPreference.Name = "grpPreference";
            this.grpPreference.Size = new System.Drawing.Size(136, 69);
            this.grpPreference.TabIndex = 254;
            this.grpPreference.Visible = false;
            this.grpPreference.VisualStyle = Janus.Windows.UI.Dock.PanelVisualStyle.Office2003;
            // 
            // chkResponse
            // 
            this.chkResponse.Checked = true;
            this.chkResponse.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkResponse.Location = new System.Drawing.Point(82, 40);
            this.chkResponse.Name = "chkResponse";
            this.chkResponse.Size = new System.Drawing.Size(23, 21);
            this.chkResponse.TabIndex = 221;
            // 
            // label65
            // 
            this.label65.BackColor = System.Drawing.Color.Transparent;
            this.label65.Location = new System.Drawing.Point(4, 13);
            this.label65.Name = "label65";
            this.label65.Size = new System.Drawing.Size(80, 21);
            this.label65.TabIndex = 220;
            this.label65.Text = "�������(%)";
            this.label65.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label66
            // 
            this.label66.BackColor = System.Drawing.Color.Transparent;
            this.label66.Location = new System.Drawing.Point(4, 40);
            this.label66.Name = "label66";
            this.label66.Size = new System.Drawing.Size(77, 21);
            this.label66.TabIndex = 219;
            this.label66.Text = "�����ڹ̼���";
            this.label66.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // udPrefRate
            // 
            this.udPrefRate.Location = new System.Drawing.Point(87, 13);
            this.udPrefRate.MaxLength = 3;
            this.udPrefRate.Name = "udPrefRate";
            this.udPrefRate.Size = new System.Drawing.Size(40, 21);
            this.udPrefRate.TabIndex = 208;
            this.udPrefRate.TextAlignment = Janus.Windows.GridEX.TextAlignment.Far;
            this.udPrefRate.Value = 50;
            this.udPrefRate.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            // 
            // chkPreference
            // 
            this.chkPreference.BackColor = System.Drawing.Color.Transparent;
            this.chkPreference.Location = new System.Drawing.Point(840, 190);
            this.chkPreference.Name = "chkPreference";
            this.chkPreference.Size = new System.Drawing.Size(100, 20);
            this.chkPreference.TabIndex = 253;
            this.chkPreference.Text = "��ȣ�������˾�";
            this.chkPreference.Visible = false;
            this.chkPreference.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.chkPreference.CheckedChanged += new System.EventHandler(this.chkPreference_CheckedChanged);
            // 
            // grpStb
            // 
            this.grpStb.BackColor = System.Drawing.Color.Transparent;
            this.grpStb.BorderColor = System.Drawing.Color.DimGray;
            this.grpStb.Controls.Add(this.tvStb);
            this.grpStb.Location = new System.Drawing.Point(557, 56);
            this.grpStb.Name = "grpStb";
            this.grpStb.Size = new System.Drawing.Size(141, 219);
            this.grpStb.TabIndex = 252;
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
            this.tvStb.Location = new System.Drawing.Point(7, 15);
            this.tvStb.Name = "tvStb";
            treeNode9.Name = "";
            treeNode9.Text = "��ž�𵨺�";
            this.tvStb.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode9});
            this.tvStb.ShowRootLines = false;
            this.tvStb.Size = new System.Drawing.Size(128, 198);
            this.tvStb.TabIndex = 203;
            this.tvStb.AfterCheck += new System.Windows.Forms.TreeViewEventHandler(this.tvStb_AfterCheck);
            this.tvStb.AfterCollapse += new System.Windows.Forms.TreeViewEventHandler(this.tvStb_AfterCollapse);
            // 
            // chkStbModel
            // 
            this.chkStbModel.BackColor = System.Drawing.Color.Transparent;
            this.chkStbModel.Location = new System.Drawing.Point(567, 39);
            this.chkStbModel.Name = "chkStbModel";
            this.chkStbModel.Size = new System.Drawing.Size(88, 20);
            this.chkStbModel.TabIndex = 251;
            this.chkStbModel.Text = "��ž��";
            this.ttMsg.SetToolTip(this.chkStbModel, "������ ��ž�𵨿��� ���� ����˴ϴ�");
            this.chkStbModel.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.chkStbModel.CheckedChanged += new System.EventHandler(this.chkStbModel_CheckedChanged);
            // 
            // chkCollectionYn
            // 
            this.chkCollectionYn.BackColor = System.Drawing.Color.Transparent;
            this.chkCollectionYn.Font = new System.Drawing.Font("���� ���", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.chkCollectionYn.Location = new System.Drawing.Point(711, 79);
            this.chkCollectionYn.Name = "chkCollectionYn";
            this.chkCollectionYn.Size = new System.Drawing.Size(136, 20);
            this.chkCollectionYn.TabIndex = 245;
            this.chkCollectionYn.Text = "����Ÿ���� ��� ";
            this.chkCollectionYn.ToolTipText = "Ÿ�ٱ� Ÿ������ EAP���� ������ �� �ֽ��ϴ�";
            this.chkCollectionYn.Visible = false;
            this.chkCollectionYn.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnCancel.BackColor = System.Drawing.SystemColors.Window;
            this.btnCancel.Enabled = false;
            this.btnCancel.Location = new System.Drawing.Point(892, 6);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(60, 24);
            this.btnCancel.TabIndex = 244;
            this.btnCancel.Text = "�� ��";
            this.btnCancel.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // ebContractTot
            // 
            this.ebContractTot.DecimalDigits = 0;
            this.ebContractTot.Font = new System.Drawing.Font("���� ���", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.ebContractTot.FormatString = "#,##0";
            this.ebContractTot.Location = new System.Drawing.Point(236, 5);
            this.ebContractTot.Name = "ebContractTot";
            this.ebContractTot.Size = new System.Drawing.Size(78, 23);
            this.ebContractTot.TabIndex = 243;
            this.ebContractTot.Text = "10,000,000";
            this.ebContractTot.TextAlignment = Janus.Windows.GridEX.TextAlignment.Far;
            this.ebContractTot.Value = new decimal(new int[] {
            10000000,
            0,
            0,
            0});
            this.ebContractTot.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            // 
            // lblContractSum
            // 
            this.lblContractSum.BackColor = System.Drawing.Color.Transparent;
            this.lblContractSum.Location = new System.Drawing.Point(169, 6);
            this.lblContractSum.Name = "lblContractSum";
            this.lblContractSum.Size = new System.Drawing.Size(58, 20);
            this.lblContractSum.TabIndex = 242;
            this.lblContractSum.Text = "�հ蹰��";
            this.lblContractSum.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.ttMsg.SetToolTip(this.lblContractSum, "���������� ���ذ����� ��������� �ش� ��๰���������� ������ �ǵ��� �����մϴ�");
            // 
            // btnModify
            // 
            this.btnModify.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnModify.BackColor = System.Drawing.SystemColors.Window;
            this.btnModify.Enabled = false;
            this.btnModify.Location = new System.Drawing.Point(731, 6);
            this.btnModify.Name = "btnModify";
            this.btnModify.Size = new System.Drawing.Size(88, 24);
            this.btnModify.TabIndex = 241;
            this.btnModify.Text = "Ÿ������ ����";
            this.btnModify.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.btnModify.Click += new System.EventHandler(this.btnModify_Click);
            // 
            // grpAge
            // 
            this.grpAge.BackColor = System.Drawing.Color.Transparent;
            this.grpAge.BorderColor = System.Drawing.Color.DimGray;
            this.grpAge.Controls.Add(this.tvAge);
            this.grpAge.Location = new System.Drawing.Point(323, 56);
            this.grpAge.Name = "grpAge";
            this.grpAge.Size = new System.Drawing.Size(118, 143);
            this.grpAge.TabIndex = 240;
            this.grpAge.VisualStyle = Janus.Windows.UI.Dock.PanelVisualStyle.Office2007;
            // 
            // tvAge
            // 
            this.tvAge.BackColor = System.Drawing.Color.WhiteSmoke;
            this.tvAge.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tvAge.CheckBoxes = true;
            this.tvAge.FullRowSelect = true;
            this.tvAge.HideSelection = false;
            this.tvAge.ItemHeight = 16;
            this.tvAge.Location = new System.Drawing.Point(7, 14);
            this.tvAge.Name = "tvAge";
            treeNode10.Name = "";
            treeNode10.Text = "0~19��";
            treeNode11.Name = "";
            treeNode11.Text = "20~29��";
            treeNode12.Name = "";
            treeNode12.Text = "30~39��";
            treeNode13.Name = "";
            treeNode13.Text = "40~49��";
            treeNode14.Name = "";
            treeNode14.Text = "50~59��";
            treeNode15.Name = "";
            treeNode15.Text = "60���̻�";
            treeNode16.Name = "";
            treeNode16.Text = "��ü���ɴ�";
            this.tvAge.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode16});
            this.tvAge.ShowRootLines = false;
            this.tvAge.Size = new System.Drawing.Size(105, 120);
            this.tvAge.TabIndex = 203;
            this.tvAge.AfterCheck += new System.Windows.Forms.TreeViewEventHandler(this.tvAge_AfterCheck);
            this.tvAge.AfterCollapse += new System.Windows.Forms.TreeViewEventHandler(this.tvAge_AfterCollapse);
            // 
            // chkSlotYn
            // 
            this.chkSlotYn.BackColor = System.Drawing.Color.Transparent;
            this.chkSlotYn.Location = new System.Drawing.Point(187, 282);
            this.chkSlotYn.Name = "chkSlotYn";
            this.chkSlotYn.Size = new System.Drawing.Size(127, 20);
            this.chkSlotYn.TabIndex = 238;
            this.chkSlotYn.Text = "Slot��������ġ";
            this.chkSlotYn.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.chkSlotYn.CheckedChanged += new System.EventHandler(this.chkSlotYn_CheckedChanged);
            // 
            // grpSlot
            // 
            this.grpSlot.BackColor = System.Drawing.Color.Transparent;
            this.grpSlot.BorderColor = System.Drawing.Color.DimGray;
            this.grpSlot.Controls.Add(this.rbSlotForward);
            this.grpSlot.Controls.Add(this.rbSlotBackward);
            this.grpSlot.Location = new System.Drawing.Point(182, 300);
            this.grpSlot.Name = "grpSlot";
            this.grpSlot.Size = new System.Drawing.Size(130, 54);
            this.grpSlot.TabIndex = 239;
            this.grpSlot.VisualStyle = Janus.Windows.UI.Dock.PanelVisualStyle.Office2007;
            // 
            // rbSlotForward
            // 
            this.rbSlotForward.Checked = true;
            this.rbSlotForward.Location = new System.Drawing.Point(10, 21);
            this.rbSlotForward.Name = "rbSlotForward";
            this.rbSlotForward.Size = new System.Drawing.Size(30, 21);
            this.rbSlotForward.TabIndex = 35;
            this.rbSlotForward.TabStop = true;
            this.rbSlotForward.Text = "��";
            // 
            // rbSlotBackward
            // 
            this.rbSlotBackward.Location = new System.Drawing.Point(77, 21);
            this.rbSlotBackward.Name = "rbSlotBackward";
            this.rbSlotBackward.Size = new System.Drawing.Size(30, 21);
            this.rbSlotBackward.TabIndex = 33;
            this.rbSlotBackward.Text = "��";
            // 
            // tlvRegion
            // 
            this.tlvRegion.BackColor = System.Drawing.Color.WhiteSmoke;
            this.tlvRegion.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tlvRegion.CheckBoxes = System.Windows.Forms.CheckBoxesTypes.Recursive;
            this.tlvRegion.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1});
            treeListViewItemCollectionComparer2.Column = 0;
            treeListViewItemCollectionComparer2.SortOrder = System.Windows.Forms.SortOrder.None;
            this.tlvRegion.Comparer = treeListViewItemCollectionComparer2;
            this.tlvRegion.Location = new System.Drawing.Point(4, 62);
            this.tlvRegion.Name = "tlvRegion";
            this.tlvRegion.Size = new System.Drawing.Size(172, 296);
            this.tlvRegion.SmallImageList = this.imgRegion;
            this.tlvRegion.Sorting = System.Windows.Forms.SortOrder.None;
            this.tlvRegion.TabIndex = 237;
            this.tlvRegion.UseCompatibleStateImageBehavior = false;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "��������";
            this.columnHeader1.Width = 183;
            // 
            // gbTime
            // 
            this.gbTime.BackColor = System.Drawing.Color.Transparent;
            this.gbTime.BorderColor = System.Drawing.Color.DimGray;
            this.gbTime.Controls.Add(this.rbWeek);
            this.gbTime.Controls.Add(this.rbWeekDay);
            this.gbTime.Controls.Add(this.rbWeekAll);
            this.gbTime.Controls.Add(this.rbWeekEnd);
            this.gbTime.Location = new System.Drawing.Point(182, 56);
            this.gbTime.Name = "gbTime";
            this.gbTime.Size = new System.Drawing.Size(64, 219);
            this.gbTime.TabIndex = 236;
            this.gbTime.VisualStyle = Janus.Windows.UI.Dock.PanelVisualStyle.Office2007;
            // 
            // rbWeek
            // 
            this.rbWeek.Location = new System.Drawing.Point(7, 27);
            this.rbWeek.Name = "rbWeek";
            this.rbWeek.Size = new System.Drawing.Size(51, 21);
            this.rbWeek.TabIndex = 236;
            this.rbWeek.Tag = "e";
            this.rbWeek.Text = "�и�";
            this.rbWeek.Click += new System.EventHandler(this.rbWeek_Click);
            // 
            // rbWeekDay
            // 
            this.rbWeekDay.Location = new System.Drawing.Point(7, 77);
            this.rbWeekDay.Name = "rbWeekDay";
            this.rbWeekDay.Size = new System.Drawing.Size(51, 21);
            this.rbWeekDay.TabIndex = 233;
            this.rbWeekDay.Tag = "d";
            this.rbWeekDay.Text = "����";
            this.rbWeekDay.Click += new System.EventHandler(this.rbWeekDay_Click);
            // 
            // rbWeekAll
            // 
            this.rbWeekAll.Checked = true;
            this.rbWeekAll.Location = new System.Drawing.Point(7, 177);
            this.rbWeekAll.Name = "rbWeekAll";
            this.rbWeekAll.Size = new System.Drawing.Size(51, 21);
            this.rbWeekAll.TabIndex = 235;
            this.rbWeekAll.TabStop = true;
            this.rbWeekAll.Tag = "a";
            this.rbWeekAll.Text = "����";
            this.rbWeekAll.Click += new System.EventHandler(this.rbWeekAll_Click);
            // 
            // rbWeekEnd
            // 
            this.rbWeekEnd.Location = new System.Drawing.Point(7, 127);
            this.rbWeekEnd.Name = "rbWeekEnd";
            this.rbWeekEnd.Size = new System.Drawing.Size(51, 21);
            this.rbWeekEnd.TabIndex = 234;
            this.rbWeekEnd.Tag = "e";
            this.rbWeekEnd.Text = "�ָ�";
            this.rbWeekEnd.Click += new System.EventHandler(this.rbWeekEnd_Click);
            // 
            // chkPVS
            // 
            this.chkPVS.BackColor = System.Drawing.Color.Transparent;
            this.chkPVS.Location = new System.Drawing.Point(930, 81);
            this.chkPVS.Name = "chkPVS";
            this.chkPVS.Size = new System.Drawing.Size(148, 20);
            this.chkPVS.TabIndex = 232;
            this.chkPVS.Text = "���ν�ûDB Ÿ���� ���";
            this.ttMsg.SetToolTip(this.chkPVS, "������ �Ͻø� ���ν�ûDB Ÿ������ ����մϴ�");
            this.chkPVS.Visible = false;
            this.chkPVS.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            // 
            // grpFreq
            // 
            this.grpFreq.BackColor = System.Drawing.Color.Transparent;
            this.grpFreq.BorderColor = System.Drawing.Color.DimGray;
            this.grpFreq.Controls.Add(this.label60);
            this.grpFreq.Controls.Add(this.label59);
            this.grpFreq.Controls.Add(this.freqPeriod);
            this.grpFreq.Controls.Add(this.freqDay);
            this.grpFreq.Location = new System.Drawing.Point(707, 207);
            this.grpFreq.Name = "grpFreq";
            this.grpFreq.Size = new System.Drawing.Size(111, 69);
            this.grpFreq.TabIndex = 231;
            this.grpFreq.VisualStyle = Janus.Windows.UI.Dock.PanelVisualStyle.Office2003;
            // 
            // label60
            // 
            this.label60.BackColor = System.Drawing.Color.Transparent;
            this.label60.Location = new System.Drawing.Point(8, 15);
            this.label60.Name = "label60";
            this.label60.Size = new System.Drawing.Size(32, 21);
            this.label60.TabIndex = 220;
            this.label60.Text = "�ϰ�";
            this.label60.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label59
            // 
            this.label59.BackColor = System.Drawing.Color.Transparent;
            this.label59.Location = new System.Drawing.Point(8, 40);
            this.label59.Name = "label59";
            this.label59.Size = new System.Drawing.Size(34, 21);
            this.label59.TabIndex = 219;
            this.label59.Text = "�Ⱓ";
            this.label59.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // freqPeriod
            // 
            this.freqPeriod.Location = new System.Drawing.Point(44, 39);
            this.freqPeriod.Maximum = 150;
            this.freqPeriod.MaxLength = 3;
            this.freqPeriod.Name = "freqPeriod";
            this.freqPeriod.Size = new System.Drawing.Size(50, 21);
            this.freqPeriod.TabIndex = 209;
            this.freqPeriod.TextAlignment = Janus.Windows.GridEX.TextAlignment.Far;
            this.freqPeriod.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            // 
            // freqDay
            // 
            this.freqDay.Location = new System.Drawing.Point(44, 14);
            this.freqDay.Maximum = 150;
            this.freqDay.MaxLength = 3;
            this.freqDay.Name = "freqDay";
            this.freqDay.Size = new System.Drawing.Size(50, 21);
            this.freqDay.TabIndex = 208;
            this.freqDay.TextAlignment = Janus.Windows.GridEX.TextAlignment.Far;
            this.freqDay.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            // 
            // chkFreq
            // 
            this.chkFreq.BackColor = System.Drawing.Color.Transparent;
            this.chkFreq.Location = new System.Drawing.Point(712, 190);
            this.chkFreq.Name = "chkFreq";
            this.chkFreq.Size = new System.Drawing.Size(84, 20);
            this.chkFreq.TabIndex = 230;
            this.chkFreq.Text = "�����";
            this.chkFreq.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.chkFreq.CheckedChanged += new System.EventHandler(this.chkFreq_CheckedChanged);
            // 
            // chkPPx
            // 
            this.chkPPx.BackColor = System.Drawing.Color.Transparent;
            this.chkPPx.Location = new System.Drawing.Point(711, 39);
            this.chkPPx.Name = "chkPPx";
            this.chkPPx.Size = new System.Drawing.Size(130, 20);
            this.chkPPx.TabIndex = 229;
            this.chkPPx.Text = "����ä������";
            this.ttMsg.SetToolTip(this.chkPPx, "������ �Ͻø� ����ä��(PPx)���� ���� �����մϴ�.");
            this.chkPPx.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.chkPPx.CheckedChanged += new System.EventHandler(this.onPPxChanged);
            // 
            // gbPost
            // 
            this.gbPost.BackColor = System.Drawing.Color.Transparent;
            this.gbPost.BorderColor = System.Drawing.Color.DimGray;
            this.gbPost.Controls.Add(this.btnAddZip);
            this.gbPost.Controls.Add(this.btnNewZip);
            this.gbPost.Controls.Add(this.ebZipCode);
            this.gbPost.Controls.Add(this.lblZipName);
            this.gbPost.Location = new System.Drawing.Point(706, 118);
            this.gbPost.Name = "gbPost";
            this.gbPost.Size = new System.Drawing.Size(254, 64);
            this.gbPost.TabIndex = 228;
            this.gbPost.Visible = false;
            this.gbPost.VisualStyle = Janus.Windows.UI.Dock.PanelVisualStyle.Office2007;
            // 
            // btnAddZip
            // 
            this.btnAddZip.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnAddZip.BackColor = System.Drawing.SystemColors.Window;
            this.btnAddZip.Enabled = false;
            this.btnAddZip.Location = new System.Drawing.Point(133, 40);
            this.btnAddZip.Name = "btnAddZip";
            this.btnAddZip.Size = new System.Drawing.Size(76, 20);
            this.btnAddZip.TabIndex = 213;
            this.btnAddZip.Text = "�߰�";
            this.btnAddZip.ToolTipText = "�����ȣ�� �߰��մϴ�.";
            this.btnAddZip.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.btnAddZip.Click += new System.EventHandler(this.btnAddZip_Click);
            // 
            // btnNewZip
            // 
            this.btnNewZip.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnNewZip.BackColor = System.Drawing.SystemColors.Window;
            this.btnNewZip.Enabled = false;
            this.btnNewZip.Location = new System.Drawing.Point(49, 40);
            this.btnNewZip.Name = "btnNewZip";
            this.btnNewZip.Size = new System.Drawing.Size(76, 20);
            this.btnNewZip.TabIndex = 212;
            this.btnNewZip.Text = "�ű�";
            this.btnNewZip.ToolTipText = "�����ȣ�� �űԷ� �Է��մϴ�, ���� �����ʹ� �����˴ϴ�";
            this.btnNewZip.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.btnNewZip.Click += new System.EventHandler(this.btnNewZip_Click);
            // 
            // ebZipCode
            // 
            this.ebZipCode.ButtonStyle = Janus.Windows.GridEX.EditControls.EditButtonStyle.Ellipsis;
            this.ebZipCode.Cursor = System.Windows.Forms.Cursors.Hand;
            this.ebZipCode.Location = new System.Drawing.Point(13, 13);
            this.ebZipCode.MaxLength = 800;
            this.ebZipCode.Name = "ebZipCode";
            this.ebZipCode.ReadOnly = true;
            this.ebZipCode.Size = new System.Drawing.Size(228, 21);
            this.ebZipCode.TabIndex = 7;
            this.ebZipCode.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
            this.ttMsg.SetToolTip(this.ebZipCode, "���õ� �����ȣ�� ǥ�õ˴ϴ�. ��ư�� Ŭ���Ͻø� ���� ���õ� �����ȣ�� �� �� �ֽ��ϴ�");
            this.ebZipCode.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2003;
            this.ebZipCode.ButtonClick += new System.EventHandler(this.ebZipCode_ButtonClick);
            // 
            // lblZipName
            // 
            this.lblZipName.Location = new System.Drawing.Point(702, 37);
            this.lblZipName.Name = "lblZipName";
            this.lblZipName.Size = new System.Drawing.Size(272, 16);
            this.lblZipName.TabIndex = 6;
            // 
            // chkZip
            // 
            this.chkZip.BackColor = System.Drawing.Color.Transparent;
            this.chkZip.Location = new System.Drawing.Point(711, 99);
            this.chkZip.Name = "chkZip";
            this.chkZip.Size = new System.Drawing.Size(84, 20);
            this.chkZip.TabIndex = 227;
            this.chkZip.Text = "�����ȣ";
            this.ttMsg.SetToolTip(this.chkZip, "������ �����ȣ���� Ÿ���� ����˴ϴ�");
            this.chkZip.Visible = false;
            this.chkZip.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.chkZip.CheckedChanged += new System.EventHandler(this.chkZip_CheckedChanged);
            // 
            // uiGroupBox3
            // 
            this.uiGroupBox3.BackColor = System.Drawing.Color.Transparent;
            this.uiGroupBox3.BorderColor = System.Drawing.Color.CornflowerBlue;
            this.uiGroupBox3.Controls.Add(this.lbNotice);
            this.uiGroupBox3.ForeColor = System.Drawing.Color.Black;
            this.uiGroupBox3.Location = new System.Drawing.Point(323, 278);
            this.uiGroupBox3.Name = "uiGroupBox3";
            this.uiGroupBox3.Size = new System.Drawing.Size(635, 77);
            this.uiGroupBox3.TabIndex = 226;
            this.uiGroupBox3.Text = "�޼���";
            this.uiGroupBox3.VisualStyle = Janus.Windows.UI.Dock.PanelVisualStyle.Office2007;
            // 
            // lbNotice
            // 
            this.lbNotice.BackColor = System.Drawing.Color.Transparent;
            this.lbNotice.ForeColor = System.Drawing.Color.Red;
            this.lbNotice.Location = new System.Drawing.Point(15, 17);
            this.lbNotice.Name = "lbNotice";
            this.lbNotice.Size = new System.Drawing.Size(613, 52);
            this.lbNotice.TabIndex = 219;
            this.lbNotice.Text = "���������� ������� �ʾҽ��ϴ�. ������ �Ϸ����� ������ Ÿ������ ���� �ʽ��ϴ�.";
            this.lbNotice.Visible = false;
            // 
            // label58
            // 
            this.label58.BackColor = System.Drawing.Color.Gray;
            this.label58.Location = new System.Drawing.Point(6, 33);
            this.label58.Name = "label58";
            this.label58.Size = new System.Drawing.Size(946, 1);
            this.label58.TabIndex = 225;
            // 
            // grpRateType
            // 
            this.grpRateType.BackColor = System.Drawing.Color.Transparent;
            this.grpRateType.BorderColor = System.Drawing.Color.DimGray;
            this.grpRateType.Controls.Add(this.rbRateMinus);
            this.grpRateType.Controls.Add(this.rbRatePlus);
            this.grpRateType.Location = new System.Drawing.Point(449, 240);
            this.grpRateType.Name = "grpRateType";
            this.grpRateType.Size = new System.Drawing.Size(100, 35);
            this.grpRateType.TabIndex = 224;
            this.grpRateType.VisualStyle = Janus.Windows.UI.Dock.PanelVisualStyle.Office2003;
            // 
            // rbRateMinus
            // 
            this.rbRateMinus.Location = new System.Drawing.Point(51, 10);
            this.rbRateMinus.Name = "rbRateMinus";
            this.rbRateMinus.Size = new System.Drawing.Size(48, 21);
            this.rbRateMinus.TabIndex = 31;
            this.rbRateMinus.Text = "�̸�";
            this.rbRateMinus.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            // 
            // rbRatePlus
            // 
            this.rbRatePlus.Checked = true;
            this.rbRatePlus.Location = new System.Drawing.Point(6, 10);
            this.rbRatePlus.Name = "rbRatePlus";
            this.rbRatePlus.Size = new System.Drawing.Size(56, 21);
            this.rbRatePlus.TabIndex = 30;
            this.rbRatePlus.TabStop = true;
            this.rbRatePlus.Text = "�̻�";
            this.rbRatePlus.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            // 
            // udControlRate
            // 
            this.udControlRate.Font = new System.Drawing.Font("���� ���", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.udControlRate.Location = new System.Drawing.Point(625, 5);
            this.udControlRate.Maximum = 900;
            this.udControlRate.MaxLength = 3;
            this.udControlRate.Minimum = 20;
            this.udControlRate.Name = "udControlRate";
            this.udControlRate.Size = new System.Drawing.Size(56, 23);
            this.udControlRate.TabIndex = 197;
            this.udControlRate.TextAlignment = Janus.Windows.GridEX.TextAlignment.Far;
            this.udControlRate.Value = 100;
            this.udControlRate.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            // 
            // ebContractAmt
            // 
            this.ebContractAmt.DecimalDigits = 0;
            this.ebContractAmt.Font = new System.Drawing.Font("���� ���", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.ebContractAmt.FormatString = "#,##0";
            this.ebContractAmt.Location = new System.Drawing.Point(75, 5);
            this.ebContractAmt.Name = "ebContractAmt";
            this.ebContractAmt.Size = new System.Drawing.Size(79, 23);
            this.ebContractAmt.TabIndex = 194;
            this.ebContractAmt.Text = "10,000,000";
            this.ebContractAmt.TextAlignment = Janus.Windows.GridEX.TextAlignment.Far;
            this.ebContractAmt.Value = new decimal(new int[] {
            10000000,
            0,
            0,
            0});
            this.ebContractAmt.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            // 
            // lblContractAmt
            // 
            this.lblContractAmt.BackColor = System.Drawing.Color.Transparent;
            this.lblContractAmt.Location = new System.Drawing.Point(11, 6);
            this.lblContractAmt.Name = "lblContractAmt";
            this.lblContractAmt.Size = new System.Drawing.Size(58, 20);
            this.lblContractAmt.TabIndex = 215;
            this.lblContractAmt.Text = "��๰��";
            this.lblContractAmt.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.ttMsg.SetToolTip(this.lblContractAmt, "���������� ���ذ����� ��������� �ش� ��๰���������� ������ �ǵ��� �����մϴ�");
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Location = new System.Drawing.Point(531, 6);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(84, 21);
            this.label2.TabIndex = 216;
            this.label2.Text = "��������(%)";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.ttMsg.SetToolTip(this.label2, "���ⷮ�� ������ŭ �ø��ų� ���Դϴ�");
            // 
            // label7
            // 
            this.label7.BackColor = System.Drawing.Color.Transparent;
            this.label7.Location = new System.Drawing.Point(342, 6);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(56, 21);
            this.label7.TabIndex = 213;
            this.label7.Text = "��������";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.ttMsg.SetToolTip(this.label7, "��๰�� �̻����� ������ ���� �ʵ��� ���ⷮ�� �����մϴ�");
            // 
            // chkWeekYn
            // 
            this.chkWeekYn.BackColor = System.Drawing.Color.Transparent;
            this.chkWeekYn.Location = new System.Drawing.Point(263, 39);
            this.chkWeekYn.Name = "chkWeekYn";
            this.chkWeekYn.Size = new System.Drawing.Size(59, 20);
            this.chkWeekYn.TabIndex = 220;
            this.chkWeekYn.Text = "����";
            this.chkWeekYn.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.chkWeekYn.CheckedChanged += new System.EventHandler(this.chkWeekYn_CheckedChanged);
            // 
            // gbDays
            // 
            this.gbDays.BackColor = System.Drawing.Color.Transparent;
            this.gbDays.BorderColor = System.Drawing.Color.DimGray;
            this.gbDays.Controls.Add(this.chkSun);
            this.gbDays.Controls.Add(this.chkSat);
            this.gbDays.Controls.Add(this.chkFri);
            this.gbDays.Controls.Add(this.chkThe);
            this.gbDays.Controls.Add(this.chkWed);
            this.gbDays.Controls.Add(this.chkMon);
            this.gbDays.Controls.Add(this.chkThu);
            this.gbDays.Location = new System.Drawing.Point(254, 56);
            this.gbDays.Name = "gbDays";
            this.gbDays.Size = new System.Drawing.Size(61, 220);
            this.gbDays.TabIndex = 221;
            this.gbDays.VisualStyle = Janus.Windows.UI.Dock.PanelVisualStyle.Office2007;
            // 
            // chkSun
            // 
            this.chkSun.ForeColor = System.Drawing.Color.Red;
            this.chkSun.Location = new System.Drawing.Point(8, 195);
            this.chkSun.Name = "chkSun";
            this.chkSun.Size = new System.Drawing.Size(32, 21);
            this.chkSun.TabIndex = 30;
            this.chkSun.Tag = "17";
            this.chkSun.Text = "��";
            this.chkSun.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            // 
            // chkSat
            // 
            this.chkSat.ForeColor = System.Drawing.Color.Blue;
            this.chkSat.Location = new System.Drawing.Point(8, 165);
            this.chkSat.Name = "chkSat";
            this.chkSat.Size = new System.Drawing.Size(32, 21);
            this.chkSat.TabIndex = 29;
            this.chkSat.Tag = "16";
            this.chkSat.Text = "��";
            this.chkSat.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            // 
            // chkFri
            // 
            this.chkFri.Location = new System.Drawing.Point(8, 135);
            this.chkFri.Name = "chkFri";
            this.chkFri.Size = new System.Drawing.Size(32, 21);
            this.chkFri.TabIndex = 28;
            this.chkFri.Tag = "5";
            this.chkFri.Text = "��";
            this.chkFri.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            // 
            // chkThe
            // 
            this.chkThe.Location = new System.Drawing.Point(8, 100);
            this.chkThe.Name = "chkThe";
            this.chkThe.Size = new System.Drawing.Size(32, 21);
            this.chkThe.TabIndex = 27;
            this.chkThe.Tag = "4";
            this.chkThe.Text = "��";
            this.chkThe.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            // 
            // chkWed
            // 
            this.chkWed.Location = new System.Drawing.Point(8, 70);
            this.chkWed.Name = "chkWed";
            this.chkWed.Size = new System.Drawing.Size(32, 21);
            this.chkWed.TabIndex = 26;
            this.chkWed.Tag = "3";
            this.chkWed.Text = "��";
            this.chkWed.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            // 
            // chkMon
            // 
            this.chkMon.Location = new System.Drawing.Point(8, 10);
            this.chkMon.Name = "chkMon";
            this.chkMon.Size = new System.Drawing.Size(32, 21);
            this.chkMon.TabIndex = 24;
            this.chkMon.Tag = "1";
            this.chkMon.Text = "��";
            this.chkMon.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            // 
            // chkThu
            // 
            this.chkThu.Location = new System.Drawing.Point(8, 40);
            this.chkThu.Name = "chkThu";
            this.chkThu.Size = new System.Drawing.Size(32, 21);
            this.chkThu.TabIndex = 25;
            this.chkThu.Tag = "2";
            this.chkThu.Text = "ȭ";
            this.chkThu.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            // 
            // chkRateYn
            // 
            this.chkRateYn.BackColor = System.Drawing.Color.Transparent;
            this.chkRateYn.Location = new System.Drawing.Point(454, 117);
            this.chkRateYn.Name = "chkRateYn";
            this.chkRateYn.Size = new System.Drawing.Size(93, 21);
            this.chkRateYn.TabIndex = 209;
            this.chkRateYn.Text = "���������";
            this.chkRateYn.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.chkRateYn.CheckedChanged += new System.EventHandler(this.chkRateYn_CheckedChanged);
            // 
            // chkTimeYn
            // 
            this.chkTimeYn.BackColor = System.Drawing.Color.Transparent;
            this.chkTimeYn.Location = new System.Drawing.Point(187, 39);
            this.chkTimeYn.Name = "chkTimeYn";
            this.chkTimeYn.Size = new System.Drawing.Size(69, 20);
            this.chkTimeYn.TabIndex = 200;
            this.chkTimeYn.Text = "�ð���";
            this.chkTimeYn.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.chkTimeYn.CheckedChanged += new System.EventHandler(this.chkTimeYn_CheckedChanged);
            // 
            // chkRegionYn
            // 
            this.chkRegionYn.BackColor = System.Drawing.Color.Transparent;
            this.chkRegionYn.Location = new System.Drawing.Point(11, 39);
            this.chkRegionYn.Name = "chkRegionYn";
            this.chkRegionYn.Size = new System.Drawing.Size(80, 20);
            this.chkRegionYn.TabIndex = 198;
            this.chkRegionYn.Text = "����";
            this.chkRegionYn.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.chkRegionYn.CheckedChanged += new System.EventHandler(this.chkRegionYn_CheckedChanged);
            // 
            // grpRate
            // 
            this.grpRate.BackColor = System.Drawing.Color.Transparent;
            this.grpRate.BorderColor = System.Drawing.Color.DimGray;
            this.grpRate.Controls.Add(this.rbRate19);
            this.grpRate.Controls.Add(this.rbRate15);
            this.grpRate.Controls.Add(this.rbRate12);
            this.grpRate.Controls.Add(this.rbRateAll);
            this.grpRate.Location = new System.Drawing.Point(449, 133);
            this.grpRate.Name = "grpRate";
            this.grpRate.Size = new System.Drawing.Size(100, 106);
            this.grpRate.TabIndex = 210;
            this.grpRate.VisualStyle = Janus.Windows.UI.Dock.PanelVisualStyle.Office2007;
            // 
            // rbRate19
            // 
            this.rbRate19.Location = new System.Drawing.Point(7, 80);
            this.rbRate19.Name = "rbRate19";
            this.rbRate19.Size = new System.Drawing.Size(80, 21);
            this.rbRate19.TabIndex = 33;
            this.rbRate19.Text = "19��";
            this.rbRate19.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            // 
            // rbRate15
            // 
            this.rbRate15.Location = new System.Drawing.Point(7, 56);
            this.rbRate15.Name = "rbRate15";
            this.rbRate15.Size = new System.Drawing.Size(80, 21);
            this.rbRate15.TabIndex = 32;
            this.rbRate15.Text = "15��";
            this.rbRate15.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            // 
            // rbRate12
            // 
            this.rbRate12.Location = new System.Drawing.Point(7, 32);
            this.rbRate12.Name = "rbRate12";
            this.rbRate12.Size = new System.Drawing.Size(80, 21);
            this.rbRate12.TabIndex = 31;
            this.rbRate12.Text = "12��";
            this.rbRate12.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            // 
            // rbRateAll
            // 
            this.rbRateAll.Checked = true;
            this.rbRateAll.Location = new System.Drawing.Point(7, 8);
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
            this.btnSave.Location = new System.Drawing.Point(826, 6);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(60, 24);
            this.btnSave.TabIndex = 211;
            this.btnSave.Text = "�� ��";
            this.btnSave.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // chkAgeYn
            // 
            this.chkAgeYn.BackColor = System.Drawing.Color.Transparent;
            this.chkAgeYn.Location = new System.Drawing.Point(333, 39);
            this.chkAgeYn.Name = "chkAgeYn";
            this.chkAgeYn.Size = new System.Drawing.Size(88, 20);
            this.chkAgeYn.TabIndex = 202;
            this.chkAgeYn.Text = "���ɴ�";
            this.chkAgeYn.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.chkAgeYn.CheckedChanged += new System.EventHandler(this.chkAgeYn_CheckedChanged);
            // 
            // chkSexYn
            // 
            this.chkSexYn.BackColor = System.Drawing.Color.Transparent;
            this.chkSexYn.Location = new System.Drawing.Point(456, 39);
            this.chkSexYn.Name = "chkSexYn";
            this.chkSexYn.Size = new System.Drawing.Size(72, 20);
            this.chkSexYn.TabIndex = 204;
            this.chkSexYn.Text = "����";
            this.chkSexYn.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.chkSexYn.CheckedChanged += new System.EventHandler(this.chkSexYn_CheckedChanged);
            // 
            // grpSex
            // 
            this.grpSex.BackColor = System.Drawing.Color.Transparent;
            this.grpSex.BorderColor = System.Drawing.Color.DimGray;
            this.grpSex.Controls.Add(this.chkSexMan);
            this.grpSex.Controls.Add(this.chkSexWoman);
            this.grpSex.Location = new System.Drawing.Point(449, 56);
            this.grpSex.Name = "grpSex";
            this.grpSex.Size = new System.Drawing.Size(100, 57);
            this.grpSex.TabIndex = 205;
            this.grpSex.VisualStyle = Janus.Windows.UI.Dock.PanelVisualStyle.Office2007;
            // 
            // chkSexMan
            // 
            this.chkSexMan.Location = new System.Drawing.Point(7, 11);
            this.chkSexMan.Name = "chkSexMan";
            this.chkSexMan.Size = new System.Drawing.Size(64, 21);
            this.chkSexMan.TabIndex = 24;
            this.chkSexMan.Text = "����";
            // 
            // chkSexWoman
            // 
            this.chkSexWoman.Location = new System.Drawing.Point(7, 32);
            this.chkSexWoman.Name = "chkSexWoman";
            this.chkSexWoman.Size = new System.Drawing.Size(64, 21);
            this.chkSexWoman.TabIndex = 25;
            this.chkSexWoman.Text = "����";
            // 
            // chkAgeBtnYn
            // 
            this.chkAgeBtnYn.BackColor = System.Drawing.Color.Transparent;
            this.chkAgeBtnYn.Location = new System.Drawing.Point(328, 201);
            this.chkAgeBtnYn.Name = "chkAgeBtnYn";
            this.chkAgeBtnYn.Size = new System.Drawing.Size(104, 21);
            this.chkAgeBtnYn.TabIndex = 206;
            this.chkAgeBtnYn.Text = "���ɱ���";
            this.chkAgeBtnYn.Visible = false;
            this.chkAgeBtnYn.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.chkAgeBtnYn.CheckedChanged += new System.EventHandler(this.chkAgeBtnYn_CheckedChanged);
            // 
            // udAgeBtnBegin
            // 
            this.udAgeBtnBegin.Location = new System.Drawing.Point(323, 224);
            this.udAgeBtnBegin.Maximum = 150;
            this.udAgeBtnBegin.MaxLength = 3;
            this.udAgeBtnBegin.Name = "udAgeBtnBegin";
            this.udAgeBtnBegin.Size = new System.Drawing.Size(45, 21);
            this.udAgeBtnBegin.TabIndex = 207;
            this.udAgeBtnBegin.TextAlignment = Janus.Windows.GridEX.TextAlignment.Far;
            this.udAgeBtnBegin.Visible = false;
            this.udAgeBtnBegin.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            // 
            // label4
            // 
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.Location = new System.Drawing.Point(375, 224);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(14, 21);
            this.label4.TabIndex = 212;
            this.label4.Text = "~";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label4.Visible = false;
            // 
            // udAgeBtnEnd
            // 
            this.udAgeBtnEnd.Location = new System.Drawing.Point(396, 224);
            this.udAgeBtnEnd.Maximum = 150;
            this.udAgeBtnEnd.MaxLength = 3;
            this.udAgeBtnEnd.Name = "udAgeBtnEnd";
            this.udAgeBtnEnd.Size = new System.Drawing.Size(45, 21);
            this.udAgeBtnEnd.TabIndex = 208;
            this.udAgeBtnEnd.TextAlignment = Janus.Windows.GridEX.TextAlignment.Far;
            this.udAgeBtnEnd.Visible = false;
            this.udAgeBtnEnd.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            // 
            // rbControlYn_Y
            // 
            this.rbControlYn_Y.BackColor = System.Drawing.Color.Transparent;
            this.rbControlYn_Y.Checked = true;
            this.rbControlYn_Y.Location = new System.Drawing.Point(406, 6);
            this.rbControlYn_Y.Name = "rbControlYn_Y";
            this.rbControlYn_Y.Size = new System.Drawing.Size(56, 20);
            this.rbControlYn_Y.TabIndex = 15;
            this.rbControlYn_Y.TabStop = true;
            this.rbControlYn_Y.Text = "����";
            this.rbControlYn_Y.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.rbControlYn_Y.CheckedChanged += new System.EventHandler(this.OnControlYnCheckedChanged);
            // 
            // rbControlYn_N
            // 
            this.rbControlYn_N.BackColor = System.Drawing.Color.Transparent;
            this.rbControlYn_N.Location = new System.Drawing.Point(466, 6);
            this.rbControlYn_N.Name = "rbControlYn_N";
            this.rbControlYn_N.Size = new System.Drawing.Size(60, 20);
            this.rbControlYn_N.TabIndex = 15;
            this.rbControlYn_N.Text = "����";
            this.rbControlYn_N.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.rbControlYn_N.CheckedChanged += new System.EventHandler(this.OnControlYnCheckedChanged);
            // 
            // uiTabPage2
            // 
            this.uiTabPage2.Controls.Add(this.btnSaveRate);
            this.uiTabPage2.Controls.Add(this.uiGroupBox4);
            this.uiTabPage2.Controls.Add(this.uiGroupBox5);
            this.uiTabPage2.Controls.Add(this.uiGroupBox6);
            this.uiTabPage2.Font = new System.Drawing.Font("���� ���", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.uiTabPage2.Icon = ((System.Drawing.Icon)(resources.GetObject("uiTabPage2.Icon")));
            this.uiTabPage2.Location = new System.Drawing.Point(23, 1);
            this.uiTabPage2.Name = "uiTabPage2";
            this.uiTabPage2.Size = new System.Drawing.Size(968, 363);
            this.uiTabPage2.TabStop = true;
            this.uiTabPage2.TabVisible = false;
            this.uiTabPage2.Text = "�������";
            // 
            // btnSaveRate
            // 
            this.btnSaveRate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnSaveRate.BackColor = System.Drawing.SystemColors.Window;
            this.btnSaveRate.Enabled = false;
            this.btnSaveRate.Location = new System.Drawing.Point(640, 306);
            this.btnSaveRate.Name = "btnSaveRate";
            this.btnSaveRate.Size = new System.Drawing.Size(104, 24);
            this.btnSaveRate.TabIndex = 234;
            this.btnSaveRate.Text = "�� ��";
            this.btnSaveRate.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.btnSaveRate.Click += new System.EventHandler(this.btnSaveRate_Click);
            // 
            // uiGroupBox4
            // 
            this.uiGroupBox4.BackColor = System.Drawing.Color.Transparent;
            this.uiGroupBox4.BorderColor = System.Drawing.Color.Black;
            this.uiGroupBox4.Controls.Add(this.ebRegionTot);
            this.uiGroupBox4.Controls.Add(this.label55);
            this.uiGroupBox4.Controls.Add(this.udRegionRate17);
            this.uiGroupBox4.Controls.Add(this.udRegionRate16);
            this.uiGroupBox4.Controls.Add(this.udRegionRate15);
            this.uiGroupBox4.Controls.Add(this.udRegionRate14);
            this.uiGroupBox4.Controls.Add(this.udRegionRate13);
            this.uiGroupBox4.Controls.Add(this.udRegionRate12);
            this.uiGroupBox4.Controls.Add(this.udRegionRate11);
            this.uiGroupBox4.Controls.Add(this.udRegionRate10);
            this.uiGroupBox4.Controls.Add(this.udRegionRate9);
            this.uiGroupBox4.Controls.Add(this.udRegionRate8);
            this.uiGroupBox4.Controls.Add(this.udRegionRate7);
            this.uiGroupBox4.Controls.Add(this.udRegionRate6);
            this.uiGroupBox4.Controls.Add(this.udRegionRate5);
            this.uiGroupBox4.Controls.Add(this.udRegionRate4);
            this.uiGroupBox4.Controls.Add(this.udRegionRate3);
            this.uiGroupBox4.Controls.Add(this.udRegionRate2);
            this.uiGroupBox4.Controls.Add(this.udRegionRate1);
            this.uiGroupBox4.Controls.Add(this.label24);
            this.uiGroupBox4.Controls.Add(this.label23);
            this.uiGroupBox4.Controls.Add(this.label22);
            this.uiGroupBox4.Controls.Add(this.label21);
            this.uiGroupBox4.Controls.Add(this.label20);
            this.uiGroupBox4.Controls.Add(this.label19);
            this.uiGroupBox4.Controls.Add(this.label18);
            this.uiGroupBox4.Controls.Add(this.label17);
            this.uiGroupBox4.Controls.Add(this.label16);
            this.uiGroupBox4.Controls.Add(this.label15);
            this.uiGroupBox4.Controls.Add(this.label14);
            this.uiGroupBox4.Controls.Add(this.label13);
            this.uiGroupBox4.Controls.Add(this.label12);
            this.uiGroupBox4.Controls.Add(this.label11);
            this.uiGroupBox4.Controls.Add(this.label10);
            this.uiGroupBox4.Controls.Add(this.label9);
            this.uiGroupBox4.Controls.Add(this.label8);
            this.uiGroupBox4.Location = new System.Drawing.Point(10, 16);
            this.uiGroupBox4.Name = "uiGroupBox4";
            this.uiGroupBox4.Size = new System.Drawing.Size(192, 312);
            this.uiGroupBox4.TabIndex = 222;
            this.uiGroupBox4.Text = "������";
            this.uiGroupBox4.VisualStyle = Janus.Windows.UI.Dock.PanelVisualStyle.Office2007;
            // 
            // ebRegionTot
            // 
            this.ebRegionTot.DecimalDigits = 0;
            this.ebRegionTot.FormatString = "#,##0";
            this.ebRegionTot.Location = new System.Drawing.Point(136, 136);
            this.ebRegionTot.MaxLength = 4;
            this.ebRegionTot.Name = "ebRegionTot";
            this.ebRegionTot.Size = new System.Drawing.Size(48, 22);
            this.ebRegionTot.TabIndex = 253;
            this.ebRegionTot.Text = "0";
            this.ebRegionTot.TextAlignment = Janus.Windows.GridEX.TextAlignment.Far;
            this.ebRegionTot.Value = 0;
            this.ebRegionTot.ValueType = Janus.Windows.GridEX.NumericEditValueType.Int32;
            this.ebRegionTot.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            // 
            // label55
            // 
            this.label55.BackColor = System.Drawing.Color.Transparent;
            this.label55.Font = new System.Drawing.Font("���� ���", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label55.ForeColor = System.Drawing.Color.Black;
            this.label55.Location = new System.Drawing.Point(104, 136);
            this.label55.Name = "label55";
            this.label55.Size = new System.Drawing.Size(32, 21);
            this.label55.TabIndex = 250;
            this.label55.Text = "�հ�";
            this.label55.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // udRegionRate17
            // 
            this.udRegionRate17.Location = new System.Drawing.Point(136, 112);
            this.udRegionRate17.MaxLength = 3;
            this.udRegionRate17.Name = "udRegionRate17";
            this.udRegionRate17.Size = new System.Drawing.Size(48, 22);
            this.udRegionRate17.TabIndex = 249;
            this.udRegionRate17.TextAlignment = Janus.Windows.GridEX.TextAlignment.Far;
            this.udRegionRate17.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            this.udRegionRate17.KeyUp += new System.Windows.Forms.KeyEventHandler(this.udRegionRate17_KeyUp);
            // 
            // udRegionRate16
            // 
            this.udRegionRate16.Location = new System.Drawing.Point(136, 88);
            this.udRegionRate16.MaxLength = 3;
            this.udRegionRate16.Name = "udRegionRate16";
            this.udRegionRate16.Size = new System.Drawing.Size(48, 22);
            this.udRegionRate16.TabIndex = 248;
            this.udRegionRate16.TextAlignment = Janus.Windows.GridEX.TextAlignment.Far;
            this.udRegionRate16.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            this.udRegionRate16.KeyUp += new System.Windows.Forms.KeyEventHandler(this.udRegionRate16_KeyUp);
            // 
            // udRegionRate15
            // 
            this.udRegionRate15.Location = new System.Drawing.Point(136, 64);
            this.udRegionRate15.MaxLength = 3;
            this.udRegionRate15.Name = "udRegionRate15";
            this.udRegionRate15.Size = new System.Drawing.Size(48, 22);
            this.udRegionRate15.TabIndex = 247;
            this.udRegionRate15.TextAlignment = Janus.Windows.GridEX.TextAlignment.Far;
            this.udRegionRate15.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            this.udRegionRate15.KeyUp += new System.Windows.Forms.KeyEventHandler(this.udRegionRate15_KeyUp);
            // 
            // udRegionRate14
            // 
            this.udRegionRate14.Location = new System.Drawing.Point(136, 40);
            this.udRegionRate14.MaxLength = 3;
            this.udRegionRate14.Name = "udRegionRate14";
            this.udRegionRate14.Size = new System.Drawing.Size(48, 22);
            this.udRegionRate14.TabIndex = 246;
            this.udRegionRate14.TextAlignment = Janus.Windows.GridEX.TextAlignment.Far;
            this.udRegionRate14.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            this.udRegionRate14.KeyUp += new System.Windows.Forms.KeyEventHandler(this.udRegionRate14_KeyUp);
            // 
            // udRegionRate13
            // 
            this.udRegionRate13.Location = new System.Drawing.Point(136, 16);
            this.udRegionRate13.MaxLength = 3;
            this.udRegionRate13.Name = "udRegionRate13";
            this.udRegionRate13.Size = new System.Drawing.Size(48, 22);
            this.udRegionRate13.TabIndex = 245;
            this.udRegionRate13.TextAlignment = Janus.Windows.GridEX.TextAlignment.Far;
            this.udRegionRate13.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            this.udRegionRate13.KeyUp += new System.Windows.Forms.KeyEventHandler(this.udRegionRate13_KeyUp);
            // 
            // udRegionRate12
            // 
            this.udRegionRate12.Location = new System.Drawing.Point(40, 280);
            this.udRegionRate12.MaxLength = 3;
            this.udRegionRate12.Name = "udRegionRate12";
            this.udRegionRate12.Size = new System.Drawing.Size(48, 22);
            this.udRegionRate12.TabIndex = 244;
            this.udRegionRate12.TextAlignment = Janus.Windows.GridEX.TextAlignment.Far;
            this.udRegionRate12.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            this.udRegionRate12.KeyUp += new System.Windows.Forms.KeyEventHandler(this.udRegionRate12_KeyUp);
            // 
            // udRegionRate11
            // 
            this.udRegionRate11.Location = new System.Drawing.Point(40, 256);
            this.udRegionRate11.MaxLength = 3;
            this.udRegionRate11.Name = "udRegionRate11";
            this.udRegionRate11.Size = new System.Drawing.Size(48, 22);
            this.udRegionRate11.TabIndex = 243;
            this.udRegionRate11.TextAlignment = Janus.Windows.GridEX.TextAlignment.Far;
            this.udRegionRate11.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            this.udRegionRate11.KeyUp += new System.Windows.Forms.KeyEventHandler(this.udRegionRate11_KeyUp);
            // 
            // udRegionRate10
            // 
            this.udRegionRate10.Location = new System.Drawing.Point(40, 232);
            this.udRegionRate10.MaxLength = 3;
            this.udRegionRate10.Name = "udRegionRate10";
            this.udRegionRate10.Size = new System.Drawing.Size(48, 22);
            this.udRegionRate10.TabIndex = 242;
            this.udRegionRate10.TextAlignment = Janus.Windows.GridEX.TextAlignment.Far;
            this.udRegionRate10.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            this.udRegionRate10.KeyUp += new System.Windows.Forms.KeyEventHandler(this.udRegionRate10_KeyUp);
            // 
            // udRegionRate9
            // 
            this.udRegionRate9.Location = new System.Drawing.Point(40, 208);
            this.udRegionRate9.MaxLength = 3;
            this.udRegionRate9.Name = "udRegionRate9";
            this.udRegionRate9.Size = new System.Drawing.Size(48, 22);
            this.udRegionRate9.TabIndex = 241;
            this.udRegionRate9.TextAlignment = Janus.Windows.GridEX.TextAlignment.Far;
            this.udRegionRate9.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            this.udRegionRate9.KeyUp += new System.Windows.Forms.KeyEventHandler(this.udRegionRate9_KeyUp);
            // 
            // udRegionRate8
            // 
            this.udRegionRate8.Location = new System.Drawing.Point(40, 184);
            this.udRegionRate8.MaxLength = 3;
            this.udRegionRate8.Name = "udRegionRate8";
            this.udRegionRate8.Size = new System.Drawing.Size(48, 22);
            this.udRegionRate8.TabIndex = 240;
            this.udRegionRate8.TextAlignment = Janus.Windows.GridEX.TextAlignment.Far;
            this.udRegionRate8.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            this.udRegionRate8.KeyUp += new System.Windows.Forms.KeyEventHandler(this.udRegionRate8_KeyUp);
            // 
            // udRegionRate7
            // 
            this.udRegionRate7.Location = new System.Drawing.Point(40, 160);
            this.udRegionRate7.MaxLength = 3;
            this.udRegionRate7.Name = "udRegionRate7";
            this.udRegionRate7.Size = new System.Drawing.Size(48, 22);
            this.udRegionRate7.TabIndex = 239;
            this.udRegionRate7.TextAlignment = Janus.Windows.GridEX.TextAlignment.Far;
            this.udRegionRate7.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            this.udRegionRate7.KeyUp += new System.Windows.Forms.KeyEventHandler(this.udRegionRate7_KeyUp);
            // 
            // udRegionRate6
            // 
            this.udRegionRate6.Location = new System.Drawing.Point(40, 136);
            this.udRegionRate6.MaxLength = 3;
            this.udRegionRate6.Name = "udRegionRate6";
            this.udRegionRate6.Size = new System.Drawing.Size(48, 22);
            this.udRegionRate6.TabIndex = 238;
            this.udRegionRate6.TextAlignment = Janus.Windows.GridEX.TextAlignment.Far;
            this.udRegionRate6.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            this.udRegionRate6.KeyUp += new System.Windows.Forms.KeyEventHandler(this.udRegionRate6_KeyUp);
            // 
            // udRegionRate5
            // 
            this.udRegionRate5.Location = new System.Drawing.Point(40, 112);
            this.udRegionRate5.MaxLength = 3;
            this.udRegionRate5.Name = "udRegionRate5";
            this.udRegionRate5.Size = new System.Drawing.Size(48, 22);
            this.udRegionRate5.TabIndex = 237;
            this.udRegionRate5.TextAlignment = Janus.Windows.GridEX.TextAlignment.Far;
            this.udRegionRate5.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            this.udRegionRate5.KeyUp += new System.Windows.Forms.KeyEventHandler(this.udRegionRate5_KeyUp);
            // 
            // udRegionRate4
            // 
            this.udRegionRate4.Location = new System.Drawing.Point(40, 88);
            this.udRegionRate4.MaxLength = 3;
            this.udRegionRate4.Name = "udRegionRate4";
            this.udRegionRate4.Size = new System.Drawing.Size(48, 22);
            this.udRegionRate4.TabIndex = 236;
            this.udRegionRate4.TextAlignment = Janus.Windows.GridEX.TextAlignment.Far;
            this.udRegionRate4.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            this.udRegionRate4.KeyUp += new System.Windows.Forms.KeyEventHandler(this.udRegionRate4_KeyUp);
            // 
            // udRegionRate3
            // 
            this.udRegionRate3.Location = new System.Drawing.Point(40, 64);
            this.udRegionRate3.MaxLength = 3;
            this.udRegionRate3.Name = "udRegionRate3";
            this.udRegionRate3.Size = new System.Drawing.Size(48, 22);
            this.udRegionRate3.TabIndex = 235;
            this.udRegionRate3.TextAlignment = Janus.Windows.GridEX.TextAlignment.Far;
            this.udRegionRate3.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            this.udRegionRate3.KeyUp += new System.Windows.Forms.KeyEventHandler(this.udRegionRate3_KeyUp);
            // 
            // udRegionRate2
            // 
            this.udRegionRate2.Location = new System.Drawing.Point(40, 40);
            this.udRegionRate2.MaxLength = 3;
            this.udRegionRate2.Name = "udRegionRate2";
            this.udRegionRate2.Size = new System.Drawing.Size(48, 22);
            this.udRegionRate2.TabIndex = 234;
            this.udRegionRate2.TextAlignment = Janus.Windows.GridEX.TextAlignment.Far;
            this.udRegionRate2.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            this.udRegionRate2.KeyUp += new System.Windows.Forms.KeyEventHandler(this.udRegionRate2_KeyUp);
            // 
            // udRegionRate1
            // 
            this.udRegionRate1.Location = new System.Drawing.Point(40, 16);
            this.udRegionRate1.MaxLength = 3;
            this.udRegionRate1.Name = "udRegionRate1";
            this.udRegionRate1.Size = new System.Drawing.Size(48, 22);
            this.udRegionRate1.TabIndex = 233;
            this.udRegionRate1.TextAlignment = Janus.Windows.GridEX.TextAlignment.Far;
            this.udRegionRate1.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            this.udRegionRate1.KeyUp += new System.Windows.Forms.KeyEventHandler(this.udRegionRate1_KeyUp);
            // 
            // label24
            // 
            this.label24.BackColor = System.Drawing.Color.Transparent;
            this.label24.Font = new System.Drawing.Font("���� ���", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label24.Location = new System.Drawing.Point(104, 112);
            this.label24.Name = "label24";
            this.label24.Size = new System.Drawing.Size(32, 21);
            this.label24.TabIndex = 232;
            this.label24.Text = "��Ÿ";
            this.label24.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label23
            // 
            this.label23.BackColor = System.Drawing.Color.Transparent;
            this.label23.Font = new System.Drawing.Font("���� ���", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label23.Location = new System.Drawing.Point(104, 88);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(32, 21);
            this.label23.TabIndex = 231;
            this.label23.Text = "����";
            this.label23.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label22
            // 
            this.label22.BackColor = System.Drawing.Color.Transparent;
            this.label22.Font = new System.Drawing.Font("���� ���", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label22.Location = new System.Drawing.Point(104, 64);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(32, 21);
            this.label22.TabIndex = 230;
            this.label22.Text = "����";
            this.label22.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label21
            // 
            this.label21.BackColor = System.Drawing.Color.Transparent;
            this.label21.Font = new System.Drawing.Font("���� ���", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label21.Location = new System.Drawing.Point(104, 40);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(32, 21);
            this.label21.TabIndex = 229;
            this.label21.Text = "����";
            this.label21.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label20
            // 
            this.label20.BackColor = System.Drawing.Color.Transparent;
            this.label20.Font = new System.Drawing.Font("���� ���", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label20.Location = new System.Drawing.Point(104, 16);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(32, 21);
            this.label20.TabIndex = 228;
            this.label20.Text = "���";
            this.label20.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label19
            // 
            this.label19.BackColor = System.Drawing.Color.Transparent;
            this.label19.Font = new System.Drawing.Font("���� ���", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label19.Location = new System.Drawing.Point(8, 280);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(32, 21);
            this.label19.TabIndex = 227;
            this.label19.Text = "�泲";
            this.label19.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label18
            // 
            this.label18.BackColor = System.Drawing.Color.Transparent;
            this.label18.Font = new System.Drawing.Font("���� ���", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label18.Location = new System.Drawing.Point(8, 256);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(32, 21);
            this.label18.TabIndex = 226;
            this.label18.Text = "���";
            this.label18.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label17
            // 
            this.label17.BackColor = System.Drawing.Color.Transparent;
            this.label17.Font = new System.Drawing.Font("���� ���", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label17.Location = new System.Drawing.Point(8, 232);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(32, 21);
            this.label17.TabIndex = 225;
            this.label17.Text = "�泲";
            this.label17.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label16
            // 
            this.label16.BackColor = System.Drawing.Color.Transparent;
            this.label16.Font = new System.Drawing.Font("���� ���", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label16.Location = new System.Drawing.Point(8, 208);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(32, 21);
            this.label16.TabIndex = 224;
            this.label16.Text = "����";
            this.label16.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label15
            // 
            this.label15.BackColor = System.Drawing.Color.Transparent;
            this.label15.Font = new System.Drawing.Font("���� ���", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label15.Location = new System.Drawing.Point(8, 184);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(32, 21);
            this.label15.TabIndex = 223;
            this.label15.Text = "���";
            this.label15.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label14
            // 
            this.label14.BackColor = System.Drawing.Color.Transparent;
            this.label14.Font = new System.Drawing.Font("���� ���", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label14.Location = new System.Drawing.Point(8, 136);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(32, 21);
            this.label14.TabIndex = 222;
            this.label14.Text = "����";
            this.label14.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label13
            // 
            this.label13.BackColor = System.Drawing.Color.Transparent;
            this.label13.Font = new System.Drawing.Font("���� ���", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label13.Location = new System.Drawing.Point(8, 160);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(32, 21);
            this.label13.TabIndex = 221;
            this.label13.Text = "����";
            this.label13.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label12
            // 
            this.label12.BackColor = System.Drawing.Color.Transparent;
            this.label12.Font = new System.Drawing.Font("���� ���", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label12.Location = new System.Drawing.Point(8, 112);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(32, 21);
            this.label12.TabIndex = 220;
            this.label12.Text = "��õ";
            this.label12.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label11
            // 
            this.label11.BackColor = System.Drawing.Color.Transparent;
            this.label11.Font = new System.Drawing.Font("���� ���", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label11.Location = new System.Drawing.Point(8, 88);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(32, 21);
            this.label11.TabIndex = 219;
            this.label11.Text = "�뱸";
            this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label10
            // 
            this.label10.BackColor = System.Drawing.Color.Transparent;
            this.label10.Font = new System.Drawing.Font("���� ���", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label10.Location = new System.Drawing.Point(8, 64);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(32, 21);
            this.label10.TabIndex = 218;
            this.label10.Text = "�λ�";
            this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label9
            // 
            this.label9.BackColor = System.Drawing.Color.Transparent;
            this.label9.Font = new System.Drawing.Font("���� ���", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label9.Location = new System.Drawing.Point(8, 40);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(32, 21);
            this.label9.TabIndex = 217;
            this.label9.Text = "���";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label8
            // 
            this.label8.BackColor = System.Drawing.Color.Transparent;
            this.label8.Font = new System.Drawing.Font("���� ���", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label8.Location = new System.Drawing.Point(8, 16);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(32, 21);
            this.label8.TabIndex = 216;
            this.label8.Text = "����";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // uiGroupBox5
            // 
            this.uiGroupBox5.BackColor = System.Drawing.Color.Transparent;
            this.uiGroupBox5.BorderColor = System.Drawing.Color.Black;
            this.uiGroupBox5.Controls.Add(this.ebTimeTot);
            this.uiGroupBox5.Controls.Add(this.label56);
            this.uiGroupBox5.Controls.Add(this.udTimeRate19);
            this.uiGroupBox5.Controls.Add(this.label54);
            this.uiGroupBox5.Controls.Add(this.udTimeRate23);
            this.uiGroupBox5.Controls.Add(this.udTimeRate22);
            this.uiGroupBox5.Controls.Add(this.udTimeRate21);
            this.uiGroupBox5.Controls.Add(this.udTimeRate20);
            this.uiGroupBox5.Controls.Add(this.udTimeRate18);
            this.uiGroupBox5.Controls.Add(this.udTimeRate17);
            this.uiGroupBox5.Controls.Add(this.udTimeRate16);
            this.uiGroupBox5.Controls.Add(this.udTimeRate15);
            this.uiGroupBox5.Controls.Add(this.udTimeRate14);
            this.uiGroupBox5.Controls.Add(this.udTimeRate13);
            this.uiGroupBox5.Controls.Add(this.udTimeRate12);
            this.uiGroupBox5.Controls.Add(this.udTimeRate11);
            this.uiGroupBox5.Controls.Add(this.udTimeRate10);
            this.uiGroupBox5.Controls.Add(this.udTimeRate9);
            this.uiGroupBox5.Controls.Add(this.udTimeRate8);
            this.uiGroupBox5.Controls.Add(this.udTimeRate7);
            this.uiGroupBox5.Controls.Add(this.udTimeRate6);
            this.uiGroupBox5.Controls.Add(this.udTimeRate5);
            this.uiGroupBox5.Controls.Add(this.udTimeRate4);
            this.uiGroupBox5.Controls.Add(this.udTimeRate3);
            this.uiGroupBox5.Controls.Add(this.udTimeRate2);
            this.uiGroupBox5.Controls.Add(this.udTimeRate1);
            this.uiGroupBox5.Controls.Add(this.udTimeRate0);
            this.uiGroupBox5.Controls.Add(this.label47);
            this.uiGroupBox5.Controls.Add(this.label46);
            this.uiGroupBox5.Controls.Add(this.label45);
            this.uiGroupBox5.Controls.Add(this.label44);
            this.uiGroupBox5.Controls.Add(this.label43);
            this.uiGroupBox5.Controls.Add(this.label42);
            this.uiGroupBox5.Controls.Add(this.label40);
            this.uiGroupBox5.Controls.Add(this.label39);
            this.uiGroupBox5.Controls.Add(this.label38);
            this.uiGroupBox5.Controls.Add(this.label37);
            this.uiGroupBox5.Controls.Add(this.label36);
            this.uiGroupBox5.Controls.Add(this.label35);
            this.uiGroupBox5.Controls.Add(this.label34);
            this.uiGroupBox5.Controls.Add(this.label33);
            this.uiGroupBox5.Controls.Add(this.label32);
            this.uiGroupBox5.Controls.Add(this.label31);
            this.uiGroupBox5.Controls.Add(this.label30);
            this.uiGroupBox5.Controls.Add(this.label29);
            this.uiGroupBox5.Controls.Add(this.label28);
            this.uiGroupBox5.Controls.Add(this.label27);
            this.uiGroupBox5.Controls.Add(this.label26);
            this.uiGroupBox5.Controls.Add(this.label25);
            this.uiGroupBox5.Controls.Add(this.label41);
            this.uiGroupBox5.Location = new System.Drawing.Point(210, 16);
            this.uiGroupBox5.Name = "uiGroupBox5";
            this.uiGroupBox5.Size = new System.Drawing.Size(300, 312);
            this.uiGroupBox5.TabIndex = 233;
            this.uiGroupBox5.Text = "�ð��뺰";
            this.uiGroupBox5.VisualStyle = Janus.Windows.UI.Dock.PanelVisualStyle.Office2007;
            // 
            // ebTimeTot
            // 
            this.ebTimeTot.DecimalDigits = 0;
            this.ebTimeTot.FormatString = "#,##0";
            this.ebTimeTot.Location = new System.Drawing.Point(232, 16);
            this.ebTimeTot.MaxLength = 4;
            this.ebTimeTot.Name = "ebTimeTot";
            this.ebTimeTot.Size = new System.Drawing.Size(48, 22);
            this.ebTimeTot.TabIndex = 266;
            this.ebTimeTot.Text = "0";
            this.ebTimeTot.TextAlignment = Janus.Windows.GridEX.TextAlignment.Far;
            this.ebTimeTot.Value = 0;
            this.ebTimeTot.ValueType = Janus.Windows.GridEX.NumericEditValueType.Int32;
            this.ebTimeTot.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            // 
            // label56
            // 
            this.label56.BackColor = System.Drawing.Color.Transparent;
            this.label56.Font = new System.Drawing.Font("���� ���", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label56.ForeColor = System.Drawing.Color.Black;
            this.label56.Location = new System.Drawing.Point(200, 16);
            this.label56.Name = "label56";
            this.label56.Size = new System.Drawing.Size(32, 21);
            this.label56.TabIndex = 264;
            this.label56.Text = "�հ�";
            this.label56.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // udTimeRate19
            // 
            this.udTimeRate19.Location = new System.Drawing.Point(144, 184);
            this.udTimeRate19.MaxLength = 3;
            this.udTimeRate19.Name = "udTimeRate19";
            this.udTimeRate19.Size = new System.Drawing.Size(48, 22);
            this.udTimeRate19.TabIndex = 263;
            this.udTimeRate19.TextAlignment = Janus.Windows.GridEX.TextAlignment.Far;
            this.udTimeRate19.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            this.udTimeRate19.KeyUp += new System.Windows.Forms.KeyEventHandler(this.udTimeRate19_KeyUp);
            // 
            // label54
            // 
            this.label54.BackColor = System.Drawing.Color.Transparent;
            this.label54.Location = new System.Drawing.Point(112, 184);
            this.label54.Name = "label54";
            this.label54.Size = new System.Drawing.Size(32, 21);
            this.label54.TabIndex = 262;
            this.label54.Text = "19��";
            this.label54.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // udTimeRate23
            // 
            this.udTimeRate23.Location = new System.Drawing.Point(144, 280);
            this.udTimeRate23.MaxLength = 3;
            this.udTimeRate23.Name = "udTimeRate23";
            this.udTimeRate23.Size = new System.Drawing.Size(48, 22);
            this.udTimeRate23.TabIndex = 261;
            this.udTimeRate23.TextAlignment = Janus.Windows.GridEX.TextAlignment.Far;
            this.udTimeRate23.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            this.udTimeRate23.KeyUp += new System.Windows.Forms.KeyEventHandler(this.udTimeRate23_KeyUp);
            // 
            // udTimeRate22
            // 
            this.udTimeRate22.Location = new System.Drawing.Point(144, 256);
            this.udTimeRate22.MaxLength = 3;
            this.udTimeRate22.Name = "udTimeRate22";
            this.udTimeRate22.Size = new System.Drawing.Size(48, 22);
            this.udTimeRate22.TabIndex = 260;
            this.udTimeRate22.TextAlignment = Janus.Windows.GridEX.TextAlignment.Far;
            this.udTimeRate22.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            this.udTimeRate22.KeyUp += new System.Windows.Forms.KeyEventHandler(this.udTimeRate22_KeyUp);
            // 
            // udTimeRate21
            // 
            this.udTimeRate21.Location = new System.Drawing.Point(144, 232);
            this.udTimeRate21.MaxLength = 3;
            this.udTimeRate21.Name = "udTimeRate21";
            this.udTimeRate21.Size = new System.Drawing.Size(48, 22);
            this.udTimeRate21.TabIndex = 259;
            this.udTimeRate21.TextAlignment = Janus.Windows.GridEX.TextAlignment.Far;
            this.udTimeRate21.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            this.udTimeRate21.KeyUp += new System.Windows.Forms.KeyEventHandler(this.udTimeRate21_KeyUp);
            // 
            // udTimeRate20
            // 
            this.udTimeRate20.Location = new System.Drawing.Point(144, 208);
            this.udTimeRate20.MaxLength = 3;
            this.udTimeRate20.Name = "udTimeRate20";
            this.udTimeRate20.Size = new System.Drawing.Size(48, 22);
            this.udTimeRate20.TabIndex = 258;
            this.udTimeRate20.TextAlignment = Janus.Windows.GridEX.TextAlignment.Far;
            this.udTimeRate20.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            this.udTimeRate20.KeyUp += new System.Windows.Forms.KeyEventHandler(this.udTimeRate20_KeyUp);
            // 
            // udTimeRate18
            // 
            this.udTimeRate18.Location = new System.Drawing.Point(144, 160);
            this.udTimeRate18.MaxLength = 3;
            this.udTimeRate18.Name = "udTimeRate18";
            this.udTimeRate18.Size = new System.Drawing.Size(48, 22);
            this.udTimeRate18.TabIndex = 257;
            this.udTimeRate18.TextAlignment = Janus.Windows.GridEX.TextAlignment.Far;
            this.udTimeRate18.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            this.udTimeRate18.KeyUp += new System.Windows.Forms.KeyEventHandler(this.udTimeRate18_KeyUp);
            // 
            // udTimeRate17
            // 
            this.udTimeRate17.Location = new System.Drawing.Point(144, 136);
            this.udTimeRate17.MaxLength = 3;
            this.udTimeRate17.Name = "udTimeRate17";
            this.udTimeRate17.Size = new System.Drawing.Size(48, 22);
            this.udTimeRate17.TabIndex = 256;
            this.udTimeRate17.TextAlignment = Janus.Windows.GridEX.TextAlignment.Far;
            this.udTimeRate17.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            this.udTimeRate17.KeyUp += new System.Windows.Forms.KeyEventHandler(this.udTimeRate17_KeyUp);
            // 
            // udTimeRate16
            // 
            this.udTimeRate16.Location = new System.Drawing.Point(144, 112);
            this.udTimeRate16.MaxLength = 3;
            this.udTimeRate16.Name = "udTimeRate16";
            this.udTimeRate16.Size = new System.Drawing.Size(48, 22);
            this.udTimeRate16.TabIndex = 255;
            this.udTimeRate16.TextAlignment = Janus.Windows.GridEX.TextAlignment.Far;
            this.udTimeRate16.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            this.udTimeRate16.KeyUp += new System.Windows.Forms.KeyEventHandler(this.udTimeRate16_KeyUp);
            // 
            // udTimeRate15
            // 
            this.udTimeRate15.Location = new System.Drawing.Point(144, 88);
            this.udTimeRate15.MaxLength = 3;
            this.udTimeRate15.Name = "udTimeRate15";
            this.udTimeRate15.Size = new System.Drawing.Size(48, 22);
            this.udTimeRate15.TabIndex = 254;
            this.udTimeRate15.TextAlignment = Janus.Windows.GridEX.TextAlignment.Far;
            this.udTimeRate15.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            this.udTimeRate15.KeyUp += new System.Windows.Forms.KeyEventHandler(this.udTimeRate15_KeyUp);
            // 
            // udTimeRate14
            // 
            this.udTimeRate14.Location = new System.Drawing.Point(144, 64);
            this.udTimeRate14.MaxLength = 3;
            this.udTimeRate14.Name = "udTimeRate14";
            this.udTimeRate14.Size = new System.Drawing.Size(48, 22);
            this.udTimeRate14.TabIndex = 253;
            this.udTimeRate14.TextAlignment = Janus.Windows.GridEX.TextAlignment.Far;
            this.udTimeRate14.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            this.udTimeRate14.KeyUp += new System.Windows.Forms.KeyEventHandler(this.udTimeRate14_KeyUp);
            // 
            // udTimeRate13
            // 
            this.udTimeRate13.Location = new System.Drawing.Point(144, 40);
            this.udTimeRate13.MaxLength = 3;
            this.udTimeRate13.Name = "udTimeRate13";
            this.udTimeRate13.Size = new System.Drawing.Size(48, 22);
            this.udTimeRate13.TabIndex = 252;
            this.udTimeRate13.TextAlignment = Janus.Windows.GridEX.TextAlignment.Far;
            this.udTimeRate13.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            this.udTimeRate13.KeyUp += new System.Windows.Forms.KeyEventHandler(this.udTimeRate13_KeyUp);
            // 
            // udTimeRate12
            // 
            this.udTimeRate12.Location = new System.Drawing.Point(144, 16);
            this.udTimeRate12.MaxLength = 3;
            this.udTimeRate12.Name = "udTimeRate12";
            this.udTimeRate12.Size = new System.Drawing.Size(48, 22);
            this.udTimeRate12.TabIndex = 251;
            this.udTimeRate12.TextAlignment = Janus.Windows.GridEX.TextAlignment.Far;
            this.udTimeRate12.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            this.udTimeRate12.KeyUp += new System.Windows.Forms.KeyEventHandler(this.udTimeRate12_KeyUp);
            // 
            // udTimeRate11
            // 
            this.udTimeRate11.Location = new System.Drawing.Point(40, 280);
            this.udTimeRate11.MaxLength = 3;
            this.udTimeRate11.Name = "udTimeRate11";
            this.udTimeRate11.Size = new System.Drawing.Size(48, 22);
            this.udTimeRate11.TabIndex = 250;
            this.udTimeRate11.TextAlignment = Janus.Windows.GridEX.TextAlignment.Far;
            this.udTimeRate11.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            this.udTimeRate11.KeyUp += new System.Windows.Forms.KeyEventHandler(this.udTimeRate11_KeyUp);
            // 
            // udTimeRate10
            // 
            this.udTimeRate10.Location = new System.Drawing.Point(40, 256);
            this.udTimeRate10.MaxLength = 3;
            this.udTimeRate10.Name = "udTimeRate10";
            this.udTimeRate10.Size = new System.Drawing.Size(48, 22);
            this.udTimeRate10.TabIndex = 249;
            this.udTimeRate10.TextAlignment = Janus.Windows.GridEX.TextAlignment.Far;
            this.udTimeRate10.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            this.udTimeRate10.KeyUp += new System.Windows.Forms.KeyEventHandler(this.udTimeRate10_KeyUp);
            // 
            // udTimeRate9
            // 
            this.udTimeRate9.Location = new System.Drawing.Point(40, 232);
            this.udTimeRate9.MaxLength = 3;
            this.udTimeRate9.Name = "udTimeRate9";
            this.udTimeRate9.Size = new System.Drawing.Size(48, 22);
            this.udTimeRate9.TabIndex = 248;
            this.udTimeRate9.TextAlignment = Janus.Windows.GridEX.TextAlignment.Far;
            this.udTimeRate9.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            this.udTimeRate9.KeyUp += new System.Windows.Forms.KeyEventHandler(this.udTimeRate9_KeyUp);
            // 
            // udTimeRate8
            // 
            this.udTimeRate8.Location = new System.Drawing.Point(40, 208);
            this.udTimeRate8.MaxLength = 3;
            this.udTimeRate8.Name = "udTimeRate8";
            this.udTimeRate8.Size = new System.Drawing.Size(48, 22);
            this.udTimeRate8.TabIndex = 247;
            this.udTimeRate8.TextAlignment = Janus.Windows.GridEX.TextAlignment.Far;
            this.udTimeRate8.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            this.udTimeRate8.KeyUp += new System.Windows.Forms.KeyEventHandler(this.udTimeRate8_KeyUp);
            // 
            // udTimeRate7
            // 
            this.udTimeRate7.Location = new System.Drawing.Point(40, 184);
            this.udTimeRate7.MaxLength = 3;
            this.udTimeRate7.Name = "udTimeRate7";
            this.udTimeRate7.Size = new System.Drawing.Size(48, 22);
            this.udTimeRate7.TabIndex = 246;
            this.udTimeRate7.TextAlignment = Janus.Windows.GridEX.TextAlignment.Far;
            this.udTimeRate7.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            this.udTimeRate7.KeyUp += new System.Windows.Forms.KeyEventHandler(this.udTimeRate7_KeyUp);
            // 
            // udTimeRate6
            // 
            this.udTimeRate6.Location = new System.Drawing.Point(40, 160);
            this.udTimeRate6.MaxLength = 3;
            this.udTimeRate6.Name = "udTimeRate6";
            this.udTimeRate6.Size = new System.Drawing.Size(48, 22);
            this.udTimeRate6.TabIndex = 245;
            this.udTimeRate6.TextAlignment = Janus.Windows.GridEX.TextAlignment.Far;
            this.udTimeRate6.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            this.udTimeRate6.KeyUp += new System.Windows.Forms.KeyEventHandler(this.udTimeRate6_KeyUp);
            // 
            // udTimeRate5
            // 
            this.udTimeRate5.Location = new System.Drawing.Point(40, 136);
            this.udTimeRate5.MaxLength = 3;
            this.udTimeRate5.Name = "udTimeRate5";
            this.udTimeRate5.Size = new System.Drawing.Size(48, 22);
            this.udTimeRate5.TabIndex = 244;
            this.udTimeRate5.TextAlignment = Janus.Windows.GridEX.TextAlignment.Far;
            this.udTimeRate5.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            this.udTimeRate5.KeyUp += new System.Windows.Forms.KeyEventHandler(this.udTimeRate5_KeyUp);
            // 
            // udTimeRate4
            // 
            this.udTimeRate4.Location = new System.Drawing.Point(40, 112);
            this.udTimeRate4.MaxLength = 3;
            this.udTimeRate4.Name = "udTimeRate4";
            this.udTimeRate4.Size = new System.Drawing.Size(48, 22);
            this.udTimeRate4.TabIndex = 243;
            this.udTimeRate4.TextAlignment = Janus.Windows.GridEX.TextAlignment.Far;
            this.udTimeRate4.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            this.udTimeRate4.KeyUp += new System.Windows.Forms.KeyEventHandler(this.udTimeRate4_KeyUp);
            // 
            // udTimeRate3
            // 
            this.udTimeRate3.Location = new System.Drawing.Point(40, 88);
            this.udTimeRate3.MaxLength = 3;
            this.udTimeRate3.Name = "udTimeRate3";
            this.udTimeRate3.Size = new System.Drawing.Size(48, 22);
            this.udTimeRate3.TabIndex = 242;
            this.udTimeRate3.TextAlignment = Janus.Windows.GridEX.TextAlignment.Far;
            this.udTimeRate3.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            this.udTimeRate3.KeyUp += new System.Windows.Forms.KeyEventHandler(this.udTimeRate3_KeyUp);
            // 
            // udTimeRate2
            // 
            this.udTimeRate2.Location = new System.Drawing.Point(40, 64);
            this.udTimeRate2.MaxLength = 3;
            this.udTimeRate2.Name = "udTimeRate2";
            this.udTimeRate2.Size = new System.Drawing.Size(48, 22);
            this.udTimeRate2.TabIndex = 241;
            this.udTimeRate2.TextAlignment = Janus.Windows.GridEX.TextAlignment.Far;
            this.udTimeRate2.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            this.udTimeRate2.KeyUp += new System.Windows.Forms.KeyEventHandler(this.udTimeRate2_KeyUp);
            // 
            // udTimeRate1
            // 
            this.udTimeRate1.Location = new System.Drawing.Point(40, 40);
            this.udTimeRate1.MaxLength = 3;
            this.udTimeRate1.Name = "udTimeRate1";
            this.udTimeRate1.Size = new System.Drawing.Size(48, 22);
            this.udTimeRate1.TabIndex = 240;
            this.udTimeRate1.TextAlignment = Janus.Windows.GridEX.TextAlignment.Far;
            this.udTimeRate1.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            this.udTimeRate1.KeyUp += new System.Windows.Forms.KeyEventHandler(this.udTimeRate1_KeyUp);
            // 
            // udTimeRate0
            // 
            this.udTimeRate0.Location = new System.Drawing.Point(40, 16);
            this.udTimeRate0.MaxLength = 3;
            this.udTimeRate0.Name = "udTimeRate0";
            this.udTimeRate0.Size = new System.Drawing.Size(48, 22);
            this.udTimeRate0.TabIndex = 239;
            this.udTimeRate0.TextAlignment = Janus.Windows.GridEX.TextAlignment.Far;
            this.udTimeRate0.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            this.udTimeRate0.KeyUp += new System.Windows.Forms.KeyEventHandler(this.udTimeRate0_KeyUp);
            // 
            // label47
            // 
            this.label47.BackColor = System.Drawing.Color.Transparent;
            this.label47.Location = new System.Drawing.Point(112, 280);
            this.label47.Name = "label47";
            this.label47.Size = new System.Drawing.Size(32, 21);
            this.label47.TabIndex = 238;
            this.label47.Tag = "";
            this.label47.Text = "23��";
            this.label47.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label46
            // 
            this.label46.BackColor = System.Drawing.Color.Transparent;
            this.label46.Location = new System.Drawing.Point(112, 256);
            this.label46.Name = "label46";
            this.label46.Size = new System.Drawing.Size(32, 21);
            this.label46.TabIndex = 237;
            this.label46.Text = "22��";
            this.label46.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label45
            // 
            this.label45.BackColor = System.Drawing.Color.Transparent;
            this.label45.Location = new System.Drawing.Point(112, 232);
            this.label45.Name = "label45";
            this.label45.Size = new System.Drawing.Size(32, 21);
            this.label45.TabIndex = 236;
            this.label45.Text = "21��";
            this.label45.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label44
            // 
            this.label44.BackColor = System.Drawing.Color.Transparent;
            this.label44.Location = new System.Drawing.Point(112, 208);
            this.label44.Name = "label44";
            this.label44.Size = new System.Drawing.Size(32, 21);
            this.label44.TabIndex = 235;
            this.label44.Text = "20��";
            this.label44.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label43
            // 
            this.label43.BackColor = System.Drawing.Color.Transparent;
            this.label43.Location = new System.Drawing.Point(112, 160);
            this.label43.Name = "label43";
            this.label43.Size = new System.Drawing.Size(32, 21);
            this.label43.TabIndex = 234;
            this.label43.Text = "18��";
            this.label43.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label42
            // 
            this.label42.BackColor = System.Drawing.Color.Transparent;
            this.label42.Location = new System.Drawing.Point(112, 136);
            this.label42.Name = "label42";
            this.label42.Size = new System.Drawing.Size(32, 21);
            this.label42.TabIndex = 233;
            this.label42.Text = "17��";
            this.label42.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label40
            // 
            this.label40.BackColor = System.Drawing.Color.Transparent;
            this.label40.Location = new System.Drawing.Point(112, 112);
            this.label40.Name = "label40";
            this.label40.Size = new System.Drawing.Size(32, 21);
            this.label40.TabIndex = 232;
            this.label40.Text = "16��";
            this.label40.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label39
            // 
            this.label39.BackColor = System.Drawing.Color.Transparent;
            this.label39.Location = new System.Drawing.Point(112, 88);
            this.label39.Name = "label39";
            this.label39.Size = new System.Drawing.Size(32, 21);
            this.label39.TabIndex = 231;
            this.label39.Text = "15��";
            this.label39.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label38
            // 
            this.label38.BackColor = System.Drawing.Color.Transparent;
            this.label38.Location = new System.Drawing.Point(112, 64);
            this.label38.Name = "label38";
            this.label38.Size = new System.Drawing.Size(32, 21);
            this.label38.TabIndex = 230;
            this.label38.Text = "14��";
            this.label38.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label37
            // 
            this.label37.BackColor = System.Drawing.Color.Transparent;
            this.label37.Location = new System.Drawing.Point(112, 40);
            this.label37.Name = "label37";
            this.label37.Size = new System.Drawing.Size(32, 21);
            this.label37.TabIndex = 229;
            this.label37.Text = "13��";
            this.label37.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label36
            // 
            this.label36.BackColor = System.Drawing.Color.Transparent;
            this.label36.Location = new System.Drawing.Point(112, 16);
            this.label36.Name = "label36";
            this.label36.Size = new System.Drawing.Size(32, 21);
            this.label36.TabIndex = 228;
            this.label36.Text = "12��";
            this.label36.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label35
            // 
            this.label35.BackColor = System.Drawing.Color.Transparent;
            this.label35.Location = new System.Drawing.Point(8, 280);
            this.label35.Name = "label35";
            this.label35.Size = new System.Drawing.Size(40, 21);
            this.label35.TabIndex = 227;
            this.label35.Text = "11��";
            this.label35.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label34
            // 
            this.label34.BackColor = System.Drawing.Color.Transparent;
            this.label34.Location = new System.Drawing.Point(8, 256);
            this.label34.Name = "label34";
            this.label34.Size = new System.Drawing.Size(40, 21);
            this.label34.TabIndex = 226;
            this.label34.Text = "10��";
            this.label34.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label33
            // 
            this.label33.BackColor = System.Drawing.Color.Transparent;
            this.label33.Location = new System.Drawing.Point(8, 232);
            this.label33.Name = "label33";
            this.label33.Size = new System.Drawing.Size(40, 21);
            this.label33.TabIndex = 225;
            this.label33.Text = "09��";
            this.label33.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label32
            // 
            this.label32.BackColor = System.Drawing.Color.Transparent;
            this.label32.Location = new System.Drawing.Point(8, 208);
            this.label32.Name = "label32";
            this.label32.Size = new System.Drawing.Size(40, 21);
            this.label32.TabIndex = 224;
            this.label32.Text = "08��";
            this.label32.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label31
            // 
            this.label31.BackColor = System.Drawing.Color.Transparent;
            this.label31.Location = new System.Drawing.Point(8, 184);
            this.label31.Name = "label31";
            this.label31.Size = new System.Drawing.Size(40, 21);
            this.label31.TabIndex = 223;
            this.label31.Text = "07��";
            this.label31.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label30
            // 
            this.label30.BackColor = System.Drawing.Color.Transparent;
            this.label30.Location = new System.Drawing.Point(8, 160);
            this.label30.Name = "label30";
            this.label30.Size = new System.Drawing.Size(40, 21);
            this.label30.TabIndex = 222;
            this.label30.Text = "06��";
            this.label30.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label29
            // 
            this.label29.BackColor = System.Drawing.Color.Transparent;
            this.label29.Location = new System.Drawing.Point(8, 136);
            this.label29.Name = "label29";
            this.label29.Size = new System.Drawing.Size(40, 21);
            this.label29.TabIndex = 221;
            this.label29.Text = "05��";
            this.label29.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label28
            // 
            this.label28.BackColor = System.Drawing.Color.Transparent;
            this.label28.Location = new System.Drawing.Point(8, 112);
            this.label28.Name = "label28";
            this.label28.Size = new System.Drawing.Size(40, 21);
            this.label28.TabIndex = 220;
            this.label28.Text = "04��";
            this.label28.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label27
            // 
            this.label27.BackColor = System.Drawing.Color.Transparent;
            this.label27.Location = new System.Drawing.Point(8, 88);
            this.label27.Name = "label27";
            this.label27.Size = new System.Drawing.Size(40, 21);
            this.label27.TabIndex = 219;
            this.label27.Text = "03��";
            this.label27.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label26
            // 
            this.label26.BackColor = System.Drawing.Color.Transparent;
            this.label26.Location = new System.Drawing.Point(8, 64);
            this.label26.Name = "label26";
            this.label26.Size = new System.Drawing.Size(40, 21);
            this.label26.TabIndex = 218;
            this.label26.Text = "02��";
            this.label26.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label25
            // 
            this.label25.BackColor = System.Drawing.Color.Transparent;
            this.label25.Location = new System.Drawing.Point(8, 40);
            this.label25.Name = "label25";
            this.label25.Size = new System.Drawing.Size(40, 21);
            this.label25.TabIndex = 217;
            this.label25.Text = "01��";
            this.label25.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label41
            // 
            this.label41.BackColor = System.Drawing.Color.Transparent;
            this.label41.Location = new System.Drawing.Point(8, 16);
            this.label41.Name = "label41";
            this.label41.Size = new System.Drawing.Size(40, 21);
            this.label41.TabIndex = 216;
            this.label41.Text = "00��";
            this.label41.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // uiGroupBox6
            // 
            this.uiGroupBox6.BackColor = System.Drawing.Color.Transparent;
            this.uiGroupBox6.BorderColor = System.Drawing.Color.Black;
            this.uiGroupBox6.Controls.Add(this.ebWeekTot);
            this.uiGroupBox6.Controls.Add(this.label57);
            this.uiGroupBox6.Controls.Add(this.udSunRate);
            this.uiGroupBox6.Controls.Add(this.udSatRate);
            this.uiGroupBox6.Controls.Add(this.udFriRate);
            this.uiGroupBox6.Controls.Add(this.udTheRate);
            this.uiGroupBox6.Controls.Add(this.udWedRate);
            this.uiGroupBox6.Controls.Add(this.udMonRate);
            this.uiGroupBox6.Controls.Add(this.label53);
            this.uiGroupBox6.Controls.Add(this.label52);
            this.uiGroupBox6.Controls.Add(this.label51);
            this.uiGroupBox6.Controls.Add(this.label50);
            this.uiGroupBox6.Controls.Add(this.label49);
            this.uiGroupBox6.Controls.Add(this.label48);
            this.uiGroupBox6.Controls.Add(this.label64);
            this.uiGroupBox6.Controls.Add(this.udThuRate);
            this.uiGroupBox6.Location = new System.Drawing.Point(516, 16);
            this.uiGroupBox6.Name = "uiGroupBox6";
            this.uiGroupBox6.Size = new System.Drawing.Size(96, 312);
            this.uiGroupBox6.TabIndex = 233;
            this.uiGroupBox6.Text = "���Ϻ�";
            this.uiGroupBox6.VisualStyle = Janus.Windows.UI.Dock.PanelVisualStyle.Office2007;
            // 
            // ebWeekTot
            // 
            this.ebWeekTot.DecimalDigits = 0;
            this.ebWeekTot.FormatString = "#,##0";
            this.ebWeekTot.Location = new System.Drawing.Point(40, 240);
            this.ebWeekTot.MaxLength = 4;
            this.ebWeekTot.Name = "ebWeekTot";
            this.ebWeekTot.Size = new System.Drawing.Size(48, 22);
            this.ebWeekTot.TabIndex = 255;
            this.ebWeekTot.Text = "0";
            this.ebWeekTot.TextAlignment = Janus.Windows.GridEX.TextAlignment.Far;
            this.ebWeekTot.Value = 0;
            this.ebWeekTot.ValueType = Janus.Windows.GridEX.NumericEditValueType.Int32;
            this.ebWeekTot.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            // 
            // label57
            // 
            this.label57.BackColor = System.Drawing.Color.Transparent;
            this.label57.Font = new System.Drawing.Font("���� ���", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label57.ForeColor = System.Drawing.Color.Black;
            this.label57.Location = new System.Drawing.Point(8, 240);
            this.label57.Name = "label57";
            this.label57.Size = new System.Drawing.Size(32, 21);
            this.label57.TabIndex = 253;
            this.label57.Text = "�հ�";
            this.label57.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // udSunRate
            // 
            this.udSunRate.Location = new System.Drawing.Point(40, 208);
            this.udSunRate.Maximum = 150;
            this.udSunRate.MaxLength = 3;
            this.udSunRate.Name = "udSunRate";
            this.udSunRate.Size = new System.Drawing.Size(48, 22);
            this.udSunRate.TabIndex = 228;
            this.udSunRate.TextAlignment = Janus.Windows.GridEX.TextAlignment.Far;
            this.udSunRate.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            this.udSunRate.KeyUp += new System.Windows.Forms.KeyEventHandler(this.udSunRate_KeyUp);
            // 
            // udSatRate
            // 
            this.udSatRate.Location = new System.Drawing.Point(40, 176);
            this.udSatRate.Maximum = 150;
            this.udSatRate.MaxLength = 3;
            this.udSatRate.Name = "udSatRate";
            this.udSatRate.Size = new System.Drawing.Size(48, 22);
            this.udSatRate.TabIndex = 227;
            this.udSatRate.TextAlignment = Janus.Windows.GridEX.TextAlignment.Far;
            this.udSatRate.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            this.udSatRate.KeyUp += new System.Windows.Forms.KeyEventHandler(this.udSatRate_KeyUp);
            // 
            // udFriRate
            // 
            this.udFriRate.Location = new System.Drawing.Point(40, 144);
            this.udFriRate.Maximum = 150;
            this.udFriRate.MaxLength = 3;
            this.udFriRate.Name = "udFriRate";
            this.udFriRate.Size = new System.Drawing.Size(48, 22);
            this.udFriRate.TabIndex = 226;
            this.udFriRate.TextAlignment = Janus.Windows.GridEX.TextAlignment.Far;
            this.udFriRate.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            this.udFriRate.KeyUp += new System.Windows.Forms.KeyEventHandler(this.udFriRate_KeyUp);
            // 
            // udTheRate
            // 
            this.udTheRate.Location = new System.Drawing.Point(40, 112);
            this.udTheRate.Maximum = 150;
            this.udTheRate.MaxLength = 3;
            this.udTheRate.Name = "udTheRate";
            this.udTheRate.Size = new System.Drawing.Size(48, 22);
            this.udTheRate.TabIndex = 225;
            this.udTheRate.TextAlignment = Janus.Windows.GridEX.TextAlignment.Far;
            this.udTheRate.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            this.udTheRate.KeyUp += new System.Windows.Forms.KeyEventHandler(this.udTheRate_KeyUp);
            // 
            // udWedRate
            // 
            this.udWedRate.Location = new System.Drawing.Point(40, 80);
            this.udWedRate.Maximum = 150;
            this.udWedRate.MaxLength = 3;
            this.udWedRate.Name = "udWedRate";
            this.udWedRate.Size = new System.Drawing.Size(48, 22);
            this.udWedRate.TabIndex = 224;
            this.udWedRate.TextAlignment = Janus.Windows.GridEX.TextAlignment.Far;
            this.udWedRate.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            this.udWedRate.KeyUp += new System.Windows.Forms.KeyEventHandler(this.udWedRate_KeyUp);
            // 
            // udMonRate
            // 
            this.udMonRate.Location = new System.Drawing.Point(40, 16);
            this.udMonRate.MaxLength = 3;
            this.udMonRate.Name = "udMonRate";
            this.udMonRate.Size = new System.Drawing.Size(48, 22);
            this.udMonRate.TabIndex = 223;
            this.udMonRate.TextAlignment = Janus.Windows.GridEX.TextAlignment.Far;
            this.udMonRate.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            this.udMonRate.KeyUp += new System.Windows.Forms.KeyEventHandler(this.udMonRate_KeyUp);
            // 
            // label53
            // 
            this.label53.BackColor = System.Drawing.Color.Transparent;
            this.label53.Font = new System.Drawing.Font("���� ���", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label53.ForeColor = System.Drawing.Color.Red;
            this.label53.Location = new System.Drawing.Point(8, 208);
            this.label53.Name = "label53";
            this.label53.Size = new System.Drawing.Size(16, 21);
            this.label53.TabIndex = 222;
            this.label53.Text = "��";
            this.label53.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label52
            // 
            this.label52.BackColor = System.Drawing.Color.Transparent;
            this.label52.Font = new System.Drawing.Font("���� ���", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label52.Location = new System.Drawing.Point(8, 48);
            this.label52.Name = "label52";
            this.label52.Size = new System.Drawing.Size(16, 21);
            this.label52.TabIndex = 221;
            this.label52.Text = "ȭ";
            this.label52.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label51
            // 
            this.label51.BackColor = System.Drawing.Color.Transparent;
            this.label51.Font = new System.Drawing.Font("���� ���", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label51.Location = new System.Drawing.Point(8, 80);
            this.label51.Name = "label51";
            this.label51.Size = new System.Drawing.Size(16, 21);
            this.label51.TabIndex = 220;
            this.label51.Text = "��";
            this.label51.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label50
            // 
            this.label50.BackColor = System.Drawing.Color.Transparent;
            this.label50.Font = new System.Drawing.Font("���� ���", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label50.Location = new System.Drawing.Point(8, 112);
            this.label50.Name = "label50";
            this.label50.Size = new System.Drawing.Size(16, 21);
            this.label50.TabIndex = 219;
            this.label50.Text = "��";
            this.label50.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label49
            // 
            this.label49.BackColor = System.Drawing.Color.Transparent;
            this.label49.Font = new System.Drawing.Font("���� ���", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label49.Location = new System.Drawing.Point(8, 144);
            this.label49.Name = "label49";
            this.label49.Size = new System.Drawing.Size(16, 21);
            this.label49.TabIndex = 218;
            this.label49.Text = "��";
            this.label49.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label48
            // 
            this.label48.BackColor = System.Drawing.Color.Transparent;
            this.label48.Font = new System.Drawing.Font("���� ���", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label48.ForeColor = System.Drawing.Color.Blue;
            this.label48.Location = new System.Drawing.Point(8, 176);
            this.label48.Name = "label48";
            this.label48.Size = new System.Drawing.Size(16, 21);
            this.label48.TabIndex = 217;
            this.label48.Text = "��";
            this.label48.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label64
            // 
            this.label64.BackColor = System.Drawing.Color.Transparent;
            this.label64.Font = new System.Drawing.Font("���� ���", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label64.Location = new System.Drawing.Point(8, 16);
            this.label64.Name = "label64";
            this.label64.Size = new System.Drawing.Size(16, 21);
            this.label64.TabIndex = 216;
            this.label64.Text = "��";
            this.label64.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // udThuRate
            // 
            this.udThuRate.Location = new System.Drawing.Point(40, 48);
            this.udThuRate.Maximum = 150;
            this.udThuRate.MaxLength = 3;
            this.udThuRate.Name = "udThuRate";
            this.udThuRate.Size = new System.Drawing.Size(48, 22);
            this.udThuRate.TabIndex = 208;
            this.udThuRate.TextAlignment = Janus.Windows.GridEX.TextAlignment.Far;
            this.udThuRate.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            this.udThuRate.KeyUp += new System.Windows.Forms.KeyEventHandler(this.udThuRate_KeyUp);
            // 
            // uiTabPage3
            // 
            this.uiTabPage3.Controls.Add(this.label63);
            this.uiTabPage3.Controls.Add(this.label62);
            this.uiTabPage3.Controls.Add(this.panel4);
            this.uiTabPage3.Controls.Add(this.panel1);
            this.uiTabPage3.Icon = ((System.Drawing.Icon)(resources.GetObject("uiTabPage3.Icon")));
            this.uiTabPage3.Location = new System.Drawing.Point(23, 1);
            this.uiTabPage3.Name = "uiTabPage3";
            this.uiTabPage3.Size = new System.Drawing.Size(968, 363);
            this.uiTabPage3.TabStop = true;
            this.uiTabPage3.TabVisible = false;
            this.uiTabPage3.Text = "����";
            // 
            // label63
            // 
            this.label63.BackColor = System.Drawing.Color.Transparent;
            this.label63.Font = new System.Drawing.Font("���� ���", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label63.Location = new System.Drawing.Point(393, 21);
            this.label63.Name = "label63";
            this.label63.Size = new System.Drawing.Size(103, 21);
            this.label63.TabIndex = 238;
            this.label63.Text = "������";
            this.label63.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label62
            // 
            this.label62.BackColor = System.Drawing.Color.Transparent;
            this.label62.Font = new System.Drawing.Font("���� ���", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label62.Location = new System.Drawing.Point(13, 20);
            this.label62.Name = "label62";
            this.label62.Size = new System.Drawing.Size(103, 21);
            this.label62.TabIndex = 238;
            this.label62.Text = "������ ����";
            this.label62.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.grdExSouceCollectionList);
            this.panel4.Controls.Add(this.panel6);
            this.panel4.Location = new System.Drawing.Point(393, 45);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(548, 300);
            this.panel4.TabIndex = 237;
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
            this.btnAddCollectionMinus.TabIndex = 41;
            this.btnAddCollectionMinus.Text = "�������� �߰�";
            this.btnAddCollectionMinus.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.btnAddCollectionMinus.Click += new System.EventHandler(this.btnAddCollectionMinus_Click);
            // 
            // lbMsg2
            // 
            this.lbMsg2.Location = new System.Drawing.Point(298, 11);
            this.lbMsg2.Name = "lbMsg2";
            this.lbMsg2.Size = new System.Drawing.Size(247, 21);
            this.lbMsg2.TabIndex = 40;
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
            this.btnAddCollection.TabIndex = 39;
            this.btnAddCollection.Text = "�������� �߰�";
            this.btnAddCollection.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.btnAddCollection.Click += new System.EventHandler(this.btnAddCollection_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.grdExTargetList);
            this.panel1.Controls.Add(this.panel5);
            this.panel1.Location = new System.Drawing.Point(12, 44);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(375, 302);
            this.panel1.TabIndex = 236;
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
            this.grdExTargetList.TabIndex = 19;
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
            this.panel5.Controls.Add(this.label3);
            this.panel5.Controls.Add(this.btnDeleteCollection);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel5.Location = new System.Drawing.Point(0, 262);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(375, 40);
            this.panel5.TabIndex = 17;
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(110, 9);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(262, 21);
            this.label3.TabIndex = 37;
            this.label3.Text = "���õ� ������ Ÿ���ü������� �����մϴ�.";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
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
            // uiTabPage4
            // 
            this.uiTabPage4.Controls.Add(this.memoEdit1);
            this.uiTabPage4.Controls.Add(this.labelControl1);
            this.uiTabPage4.Controls.Add(this.grbProfileAge);
            this.uiTabPage4.Controls.Add(this.uiGroupBox2);
            this.uiTabPage4.Controls.Add(this.btnSaveProfile);
            this.uiTabPage4.Icon = ((System.Drawing.Icon)(resources.GetObject("uiTabPage4.Icon")));
            this.uiTabPage4.Location = new System.Drawing.Point(23, 1);
            this.uiTabPage4.Name = "uiTabPage4";
            this.uiTabPage4.Size = new System.Drawing.Size(968, 363);
            this.uiTabPage4.TabStop = true;
            this.uiTabPage4.TabVisible = false;
            this.uiTabPage4.Text = "��������";
            // 
            // memoEdit1
            // 
            this.memoEdit1.EditValue = resources.GetString("memoEdit1.EditValue");
            this.memoEdit1.Location = new System.Drawing.Point(249, 37);
            this.memoEdit1.Name = "memoEdit1";
            this.memoEdit1.Properties.AcceptsReturn = false;
            this.memoEdit1.Properties.AllowFocused = false;
            this.memoEdit1.Properties.AllowMouseWheel = false;
            this.memoEdit1.Properties.Appearance.BackColor = System.Drawing.Color.Transparent;
            this.memoEdit1.Properties.Appearance.Font = new System.Drawing.Font("���� ���", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.memoEdit1.Properties.Appearance.Options.UseBackColor = true;
            this.memoEdit1.Properties.Appearance.Options.UseFont = true;
            this.memoEdit1.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Office2003;
            this.memoEdit1.Properties.LookAndFeel.SkinName = "Metropolis";
            this.memoEdit1.Properties.LookAndFeel.UseDefaultLookAndFeel = false;
            this.memoEdit1.Properties.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.memoEdit1.Size = new System.Drawing.Size(412, 145);
            this.memoEdit1.TabIndex = 240;
            this.memoEdit1.UseOptimizedRendering = true;
            // 
            // labelControl1
            // 
            this.labelControl1.Appearance.Font = new System.Drawing.Font("���� ���", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelControl1.Location = new System.Drawing.Point(249, 16);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(36, 15);
            this.labelControl1.TabIndex = 239;
            this.labelControl1.Text = "����";
            this.labelControl1.ToolTip = resources.GetString("labelControl1.ToolTip");
            this.labelControl1.ToolTipIconType = DevExpress.Utils.ToolTipIconType.Information;
            this.labelControl1.ToolTipTitle = "�˸�";
            // 
            // grbProfileAge
            // 
            this.grbProfileAge.Appearance.BackColor = System.Drawing.Color.Transparent;
            this.grbProfileAge.Appearance.Font = new System.Drawing.Font("���� ���", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grbProfileAge.Appearance.Options.UseBackColor = true;
            this.grbProfileAge.Appearance.Options.UseFont = true;
            this.grbProfileAge.AppearanceCaption.Font = new System.Drawing.Font("���� ���", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.grbProfileAge.AppearanceCaption.Options.UseFont = true;
            this.grbProfileAge.Controls.Add(this.chkU03);
            this.grbProfileAge.Controls.Add(this.chkU02);
            this.grbProfileAge.Controls.Add(this.chkF60);
            this.grbProfileAge.Controls.Add(this.chkF10);
            this.grbProfileAge.Controls.Add(this.chkM60);
            this.grbProfileAge.Controls.Add(this.chkU00);
            this.grbProfileAge.Controls.Add(this.chkF50);
            this.grbProfileAge.Controls.Add(this.chkM10);
            this.grbProfileAge.Controls.Add(this.chkM50);
            this.grbProfileAge.Controls.Add(this.chKM20);
            this.grbProfileAge.Controls.Add(this.chkF40);
            this.grbProfileAge.Controls.Add(this.chkF20);
            this.grbProfileAge.Controls.Add(this.chkM40);
            this.grbProfileAge.Controls.Add(this.chkM30);
            this.grbProfileAge.Controls.Add(this.chkF30);
            this.grbProfileAge.Location = new System.Drawing.Point(10, 16);
            this.grbProfileAge.LookAndFeel.UseDefaultLookAndFeel = false;
            this.grbProfileAge.Name = "grbProfileAge";
            this.grbProfileAge.Size = new System.Drawing.Size(233, 251);
            this.grbProfileAge.TabIndex = 238;
            this.grbProfileAge.Text = "���ɴ� ����";
            // 
            // chkU03
            // 
            this.chkU03.BackColor = System.Drawing.Color.Transparent;
            this.chkU03.Font = new System.Drawing.Font("���� ���", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.chkU03.Location = new System.Drawing.Point(125, 40);
            this.chkU03.Name = "chkU03";
            this.chkU03.Size = new System.Drawing.Size(82, 20);
            this.chkU03.TabIndex = 247;
            this.chkU03.Tag = "U03";
            this.chkU03.Text = "�ʵ� ���г�";
            this.chkU03.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            // 
            // chkU02
            // 
            this.chkU02.BackColor = System.Drawing.Color.Transparent;
            this.chkU02.Font = new System.Drawing.Font("���� ���", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.chkU02.Location = new System.Drawing.Point(18, 40);
            this.chkU02.Name = "chkU02";
            this.chkU02.Size = new System.Drawing.Size(82, 20);
            this.chkU02.TabIndex = 246;
            this.chkU02.Tag = "U02";
            this.chkU02.Text = "�����оƵ�";
            this.chkU02.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            // 
            // chkF60
            // 
            this.chkF60.BackColor = System.Drawing.Color.Transparent;
            this.chkF60.Font = new System.Drawing.Font("���� ���", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.chkF60.Location = new System.Drawing.Point(125, 225);
            this.chkF60.Name = "chkF60";
            this.chkF60.Size = new System.Drawing.Size(98, 20);
            this.chkF60.TabIndex = 245;
            this.chkF60.Tag = "F60";
            this.chkF60.Text = "60���̻� ����";
            this.chkF60.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            // 
            // chkF10
            // 
            this.chkF10.BackColor = System.Drawing.Color.Transparent;
            this.chkF10.Font = new System.Drawing.Font("���� ���", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.chkF10.Location = new System.Drawing.Point(125, 95);
            this.chkF10.Name = "chkF10";
            this.chkF10.Size = new System.Drawing.Size(82, 20);
            this.chkF10.TabIndex = 235;
            this.chkF10.Tag = "F10";
            this.chkF10.Text = "10�� ����";
            this.chkF10.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            // 
            // chkM60
            // 
            this.chkM60.BackColor = System.Drawing.Color.Transparent;
            this.chkM60.Font = new System.Drawing.Font("���� ���", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.chkM60.Location = new System.Drawing.Point(18, 225);
            this.chkM60.Name = "chkM60";
            this.chkM60.Size = new System.Drawing.Size(96, 20);
            this.chkM60.TabIndex = 244;
            this.chkM60.Tag = "M60";
            this.chkM60.Text = "60���̻� ����";
            this.chkM60.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            // 
            // chkU00
            // 
            this.chkU00.BackColor = System.Drawing.Color.Transparent;
            this.chkU00.Font = new System.Drawing.Font("���� ���", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.chkU00.Location = new System.Drawing.Point(18, 68);
            this.chkU00.Name = "chkU00";
            this.chkU00.Size = new System.Drawing.Size(82, 20);
            this.chkU00.TabIndex = 233;
            this.chkU00.Tag = "U10";
            this.chkU00.Text = "10�� �̸�";
            this.chkU00.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            // 
            // chkF50
            // 
            this.chkF50.BackColor = System.Drawing.Color.Transparent;
            this.chkF50.Font = new System.Drawing.Font("���� ���", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.chkF50.Location = new System.Drawing.Point(125, 199);
            this.chkF50.Name = "chkF50";
            this.chkF50.Size = new System.Drawing.Size(82, 20);
            this.chkF50.TabIndex = 243;
            this.chkF50.Tag = "F50";
            this.chkF50.Text = "50�� ����";
            this.chkF50.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            // 
            // chkM10
            // 
            this.chkM10.BackColor = System.Drawing.Color.Transparent;
            this.chkM10.Font = new System.Drawing.Font("���� ���", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.chkM10.Location = new System.Drawing.Point(18, 95);
            this.chkM10.Name = "chkM10";
            this.chkM10.Size = new System.Drawing.Size(82, 20);
            this.chkM10.TabIndex = 234;
            this.chkM10.Tag = "M10";
            this.chkM10.Text = "10�� ����";
            this.chkM10.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            // 
            // chkM50
            // 
            this.chkM50.BackColor = System.Drawing.Color.Transparent;
            this.chkM50.Font = new System.Drawing.Font("���� ���", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.chkM50.Location = new System.Drawing.Point(18, 199);
            this.chkM50.Name = "chkM50";
            this.chkM50.Size = new System.Drawing.Size(82, 20);
            this.chkM50.TabIndex = 242;
            this.chkM50.Tag = "M50";
            this.chkM50.Text = "50�� ����";
            this.chkM50.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            // 
            // chKM20
            // 
            this.chKM20.BackColor = System.Drawing.Color.Transparent;
            this.chKM20.Font = new System.Drawing.Font("���� ���", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.chKM20.Location = new System.Drawing.Point(18, 121);
            this.chKM20.Name = "chKM20";
            this.chKM20.Size = new System.Drawing.Size(82, 20);
            this.chKM20.TabIndex = 236;
            this.chKM20.Tag = "M20";
            this.chKM20.Text = "20�� ����";
            this.chKM20.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            // 
            // chkF40
            // 
            this.chkF40.BackColor = System.Drawing.Color.Transparent;
            this.chkF40.Font = new System.Drawing.Font("���� ���", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.chkF40.Location = new System.Drawing.Point(125, 173);
            this.chkF40.Name = "chkF40";
            this.chkF40.Size = new System.Drawing.Size(82, 20);
            this.chkF40.TabIndex = 241;
            this.chkF40.Tag = "F40";
            this.chkF40.Text = "40�� ����";
            this.chkF40.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            // 
            // chkF20
            // 
            this.chkF20.BackColor = System.Drawing.Color.Transparent;
            this.chkF20.Font = new System.Drawing.Font("���� ���", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.chkF20.Location = new System.Drawing.Point(125, 121);
            this.chkF20.Name = "chkF20";
            this.chkF20.Size = new System.Drawing.Size(82, 20);
            this.chkF20.TabIndex = 237;
            this.chkF20.Tag = "F20";
            this.chkF20.Text = "20�� ����";
            this.chkF20.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            // 
            // chkM40
            // 
            this.chkM40.BackColor = System.Drawing.Color.Transparent;
            this.chkM40.Font = new System.Drawing.Font("���� ���", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.chkM40.Location = new System.Drawing.Point(18, 173);
            this.chkM40.Name = "chkM40";
            this.chkM40.Size = new System.Drawing.Size(82, 20);
            this.chkM40.TabIndex = 240;
            this.chkM40.Tag = "M40";
            this.chkM40.Text = "40�� ����";
            this.chkM40.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            // 
            // chkM30
            // 
            this.chkM30.BackColor = System.Drawing.Color.Transparent;
            this.chkM30.Font = new System.Drawing.Font("���� ���", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.chkM30.Location = new System.Drawing.Point(18, 147);
            this.chkM30.Name = "chkM30";
            this.chkM30.Size = new System.Drawing.Size(82, 20);
            this.chkM30.TabIndex = 238;
            this.chkM30.Tag = "M30";
            this.chkM30.Text = "30�� ����";
            this.chkM30.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            // 
            // chkF30
            // 
            this.chkF30.BackColor = System.Drawing.Color.Transparent;
            this.chkF30.Font = new System.Drawing.Font("���� ���", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.chkF30.Location = new System.Drawing.Point(125, 147);
            this.chkF30.Name = "chkF30";
            this.chkF30.Size = new System.Drawing.Size(82, 20);
            this.chkF30.TabIndex = 239;
            this.chkF30.Tag = "F30";
            this.chkF30.Text = "30�� ����";
            this.chkF30.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            // 
            // uiGroupBox2
            // 
            this.uiGroupBox2.Appearance.BackColor = System.Drawing.Color.Transparent;
            this.uiGroupBox2.Appearance.Font = new System.Drawing.Font("���� ���", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.uiGroupBox2.Appearance.Options.UseBackColor = true;
            this.uiGroupBox2.Appearance.Options.UseFont = true;
            this.uiGroupBox2.AppearanceCaption.Font = new System.Drawing.Font("���� ���", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.uiGroupBox2.AppearanceCaption.Options.UseFont = true;
            this.uiGroupBox2.Controls.Add(this.tbReliability);
            this.uiGroupBox2.Controls.Add(this.label5);
            this.uiGroupBox2.Controls.Add(this.label67);
            this.uiGroupBox2.Location = new System.Drawing.Point(10, 273);
            this.uiGroupBox2.LookAndFeel.UseDefaultLookAndFeel = false;
            this.uiGroupBox2.Name = "uiGroupBox2";
            this.uiGroupBox2.Size = new System.Drawing.Size(233, 81);
            this.uiGroupBox2.TabIndex = 237;
            this.uiGroupBox2.Text = "�߷нŷڵ�";
            // 
            // tbReliability
            // 
            this.tbReliability.EditValue = 5;
            this.tbReliability.Location = new System.Drawing.Point(8, 26);
            this.tbReliability.Name = "tbReliability";
            this.tbReliability.Properties.AutoSize = false;
            this.tbReliability.Properties.LookAndFeel.SkinName = "Metropolis";
            this.tbReliability.Properties.LookAndFeel.UseDefaultLookAndFeel = false;
            this.tbReliability.Properties.Minimum = 1;
            this.tbReliability.Size = new System.Drawing.Size(212, 31);
            this.tbReliability.TabIndex = 0;
            this.tbReliability.Value = 5;
            this.tbReliability.ValueChanged += new System.EventHandler(this.tbreliability_ValueChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(11, 57);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(14, 14);
            this.label5.TabIndex = 1;
            this.label5.Text = "1";
            // 
            // label67
            // 
            this.label67.AutoSize = true;
            this.label67.Location = new System.Drawing.Point(201, 57);
            this.label67.Name = "label67";
            this.label67.Size = new System.Drawing.Size(21, 14);
            this.label67.TabIndex = 2;
            this.label67.Text = "10";
            // 
            // btnSaveProfile
            // 
            this.btnSaveProfile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnSaveProfile.BackColor = System.Drawing.SystemColors.Window;
            this.btnSaveProfile.Enabled = false;
            this.btnSaveProfile.Location = new System.Drawing.Point(249, 330);
            this.btnSaveProfile.Name = "btnSaveProfile";
            this.btnSaveProfile.Size = new System.Drawing.Size(88, 24);
            this.btnSaveProfile.TabIndex = 235;
            this.btnSaveProfile.Text = "�� ��";
            this.btnSaveProfile.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.btnSaveProfile.Click += new System.EventHandler(this.btnSaveProfile_Click);
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
            // TargetingControl
            // 
            this.Controls.Add(this.uiPanelChoiceAdSchedule);
            this.Font = new System.Drawing.Font("�������", 8.5F);
            this.Name = "TargetingControl";
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
            ((System.ComponentModel.ISupportInitialize)(this.grpPreference)).EndInit();
            this.grpPreference.ResumeLayout(false);
            this.grpPreference.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grpStb)).EndInit();
            this.grpStb.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grpAge)).EndInit();
            this.grpAge.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grpSlot)).EndInit();
            this.grpSlot.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gbTime)).EndInit();
            this.gbTime.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grpFreq)).EndInit();
            this.grpFreq.ResumeLayout(false);
            this.grpFreq.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gbPost)).EndInit();
            this.gbPost.ResumeLayout(false);
            this.gbPost.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uiGroupBox3)).EndInit();
            this.uiGroupBox3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grpRateType)).EndInit();
            this.grpRateType.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gbDays)).EndInit();
            this.gbDays.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grpRate)).EndInit();
            this.grpRate.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grpSex)).EndInit();
            this.grpSex.ResumeLayout(false);
            this.uiTabPage2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.uiGroupBox4)).EndInit();
            this.uiGroupBox4.ResumeLayout(false);
            this.uiGroupBox4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uiGroupBox5)).EndInit();
            this.uiGroupBox5.ResumeLayout(false);
            this.uiGroupBox5.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uiGroupBox6)).EndInit();
            this.uiGroupBox6.ResumeLayout(false);
            this.uiGroupBox6.PerformLayout();
            this.uiTabPage3.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdExSouceCollectionList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dvCollection)).EndInit();
            this.panel6.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdExTargetList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dvTargetingCollection)).EndInit();
            this.panel5.ResumeLayout(false);
            this.uiTabPage4.ResumeLayout(false);
            this.uiTabPage4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.memoEdit1.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grbProfileAge)).EndInit();
            this.grbProfileAge.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.uiGroupBox2)).EndInit();
            this.uiGroupBox2.ResumeLayout(false);
            this.uiGroupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tbReliability.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbReliability)).EndInit();
            this.panMenuSchedule.ResumeLayout(false);
            this.ResumeLayout(false);

        }
        #endregion

        #region ��Ʈ�� �ε�
        private void UserControl_Load(object sender, System.EventArgs e)
        {
#if (DEBUG)
            Debug.WriteLine(MethodBase.GetCurrentMethod().Name);
#endif
            // �����Ͱ����� ��ü����
            dt = ((DataView)grdExScheduleList.DataSource).Table;
            cm = (CurrencyManager)this.BindingContext[grdExScheduleList.DataSource];
            cm.PositionChanged += new System.EventHandler(OnGrdRowChanged);
            //bsList.PositionChanged += new EventHandler(OnGrdRowChanged);

            // ��Ʈ�� �ʱ�ȭ
            InitControl();

            DetailReadOnly(true);
        }
        #endregion

        #region ��Ʈ�� �ʱ�ȭ

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
            //Init_Collection();
            Init_AgencyCode();
            Common.CommonCode.Init_AdType(systemModel, commonModel, cbSearchAdType);

            InitCombo_Level();

            //����켱��������ڵ� �ʱ�ȭ
            //Janus.Windows.EditControls.UIComboBoxItem[] comboItems = new Janus.Windows.EditControls.UIComboBoxItem[10];
            //for(int i=0;i < 10;i++)
            //{
            //    comboItems[i] = new Janus.Windows.EditControls.UIComboBoxItem(i+1,i);
            //}

            //// �޺��� ��Ʈ
            //this.cbPriorityCd.Items.AddRange(comboItems);
            //this.cbPriorityCd.SelectedIndex = 4;

            Init_RegionData();	// �������� ������ �ʱ�ȭ
            Init_AgeData();		// ���ɴ� ������ �ʱ�ȭ
            Init_StbData();     // [E_08] ��ž������ ������ �ʱ�ȭ

            Init_tlvRegion();   // [E_04]
            Init_tvAge();		// ���ɴ� TreeView �ʱ�ȭ
            Init_tvStb();       // [E_08] ��ž������ TreeView �ʱ�ȭ
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

            comboItems[0] = new Janus.Windows.EditControls.UIComboBoxItem("��ü����", "00");

            for (int i = 0; i < mediacodeModel.ResultCnt; i++)
            {
                DataRow row = targetingDs.Medias.Rows[i];

                string val = row["MediaCode"].ToString();
                string txt = row["MediaName"].ToString();
                comboItems[i + 1] = new Janus.Windows.EditControls.UIComboBoxItem(txt, val);
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

            comboItems[0] = new Janus.Windows.EditControls.UIComboBoxItem("�̵�����", "00");

            for (int i = 0; i < mediarapcodeModel.ResultCnt; i++)
            {
                DataRow row = targetingDs.MediaRaps.Rows[i];

                string val = row["RapCode"].ToString();
                string txt = row["RapName"].ToString();
                comboItems[i + 1] = new Janus.Windows.EditControls.UIComboBoxItem(txt, val);
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

            comboItems[0] = new Janus.Windows.EditControls.UIComboBoxItem("����缱��", "00");

            for (int i = 0; i < agencycodeModel.ResultCnt; i++)
            {
                DataRow row = targetingDs.Agencys.Rows[i];

                string val = row["AgencyCode"].ToString();
                string txt = row["AgencyName"].ToString();
                comboItems[i + 1] = new Janus.Windows.EditControls.UIComboBoxItem(txt, val);
            }

            // �޺��� ��Ʈ
            this.cbSearchAgency.Items.AddRange(comboItems);
            this.cbSearchAgency.SelectedIndex = 0;

            Application.DoEvents();
        }

        //������ �ҽ� �߰� 2015.10.30 HJ
        private void Init_Collection()
        {
            // �����ָ� ��ȸ�Ѵ�.
            TargetingModel targetingModel = new TargetingModel();
            new TargetingManager(systemModel, commonModel).GetCollectionsList(targetingModel);

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
            if (commonModel.UserLevel == "20")
            {
                cbSearchMedia.SelectedValue = commonModel.MediaCode;
                cbSearchMedia.ReadOnly = true;
            }
            else
            {
                for (int i = 0; i < targetingDs.Medias.Rows.Count; i++)
                {
                    DataRow row = targetingDs.Medias.Rows[i];
                    if (row["MediaCode"].ToString().Equals(FrameSystem._HANATV.ToString()))
                    {
                        cbSearchMedia.SelectedValue = FrameSystem._HANATV; // �ϳ�TV�� �⺻������ �Ѵ�.	 		
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
            //grdExScheduleList.Focus();
            Application.DoEvents();
        }

        private void DisableButton()
        {
            btnSearch.Enabled = false;
            btnExcel.Enabled = false;
            btnSave.Enabled = false;
            btnSaveRate.Enabled = false;
            btnModify.Enabled = false;

            Application.DoEvents();
        }

        private void Init_RegionData()
        {
            try
            {
                // �����͸� �ʱ�ȭ
                targetingModel.Init();

                // ���� Ÿ���� �����ȸ ���񽺸� ȣ���Ѵ�.
                new TargetingManager(systemModel, commonModel).GetRegionList(targetingModel);

                if (targetingModel.ResultCD.Equals("0000"))
                {
                    Utility.SetDataTable(targetingDs.TargetRegion, targetingModel.RegionDataSet);
                }
            }
            catch (FrameException fe)
            {
                FrameSystem.showMsgForm("������������ ��ȸ����", new string[] { fe.ErrCode, fe.ResultMsg });
            }
            catch (Exception ex)
            {
                FrameSystem.showMsgForm("������������ ��ȸ����", new string[] { "", ex.Message });
            }
        }

        private void Init_AgeData()
        {
            try
            {
                // �����͸� �ʱ�ȭ
                targetingModel.Init();

                // ���� Ÿ���� �����ȸ ���񽺸� ȣ���Ѵ�.
                new TargetingManager(systemModel, commonModel).GetAgeList(targetingModel);

                if (targetingModel.ResultCD.Equals("0000"))
                {
                    Utility.SetDataTable(targetingDs.TargetAge, targetingModel.AgeDataSet);
                }
            }
            catch (FrameException fe)
            {
                FrameSystem.showMsgForm("�����ɴ� ��ȸ����", new string[] { fe.ErrCode, fe.ResultMsg });
            }
            catch (Exception ex)
            {
                FrameSystem.showMsgForm("�����ɴ� ��ȸ����", new string[] { "", ex.Message });
            }
        }

        /// <summary>
        /// ��ž������ ������ �ʱ�ȭ [E_08]
        /// </summary>
        private void Init_StbData()
        {
            try
            {
                // �����͸� �ʱ�ȭ
                targetingModel.Init();

                // �ڵ忡�� ���ȷ����� ��ȸ�Ѵ�.
                CodeModel codeModel = new CodeModel();
                codeModel.Section = "38";				// �ڵ�з� '38' - OS �з�
                new CodeManager(systemModel, commonModel).GetCodeList(codeModel);

                if (codeModel.ResultCD.Equals("0000"))
                {
                    // �����ͼ¿� ����
                    Utility.SetDataTable(targetingDs.TargetStb, codeModel.CodeDataSet);
                }
            }
            catch (FrameException fe)
            {
                FrameSystem.showMsgForm("�ܸ��� OS���� ��ȸ����", new string[] { fe.ErrCode, fe.ResultMsg });
            }
            catch (Exception ex)
            {
                FrameSystem.showMsgForm("�ܸ��� OS���� ��ȸ����", new string[] { "", ex.Message });
            }
        }

        /// <summary>
        /// [E_04] �� ���ؼ� �ּ�
        /// </summary>
        private void Init_tvRegion()
        {
            /*
            tvRegionNodeCount = 0;

            canChecking = false;	// üũó�� �̺�Ʈ���� ó�������� �۵����� �ʵ���

            tvRegion.Nodes.Clear();

            tvRegion.Nodes.Add(new TreeNode("��ü����"));

            if(targetingDs.TargetRegion.Count == 0) return;
			
            TreeNode Region1Node = null;
            TreeNode Region2Node = null;
            TreeNode Region3Node = null;  // [E_01] �߰�

            for(int i=0;i<targetingDs.TargetRegion.Count;i++)
            {
                DataRow Row = targetingDs.TargetRegion.Rows[i];
                string Name   = Row["RegionName"].ToString();
                string Code   = Row["RegionCode"].ToString();
                string Level  = Row["Level"].ToString();

                if(Code.Length < 5) Code = Utility.Fixlength(Code,2,5); // 5�ڸ��� �����

                if(Level.Equals("1"))
                {
                    // 1�ܰ� �������� �߰�
                    Region1Node = new TreeNode(Name);
                    Region1Node.Tag = Code;
                    tvRegion.Nodes[0].Nodes.Add(Region1Node);
                }
                else if(Level.Equals("2"))
                {
                    // 2�ܰ� �������� �߰�
                    Region2Node = new TreeNode(Name);
                    Region2Node.Tag = Code;
                    Region1Node.Nodes.Add(Region2Node);
                }
                else if (Level.Equals("3")) //[E_01] �� ���� �߰��� ���
                {
                    // 3�ܰ� �������� �߰�
                    Region3Node = new TreeNode(Name);
                    Region3Node.Tag = Code;
                    Region2Node.Nodes.Add(Region3Node);
                }

                tvRegionNodeCount++;
            }

            // 1�ܰ踦 Ȯ���Ͽ� �����ش�.
            tvRegion.Nodes[0].Expand();		

            canChecking = true;	// üũó�� �̺�Ʈ���� ó�������� �ٽ� �۵��ϵ���

            Init_tlvRegion();
            */
        }

        /// <summary>
        /// �������� �ʱ�ȭ ����[E_04]
        /// </summary>
        private void Init_tlvRegion()
        {
            if (targetingDs.TargetRegion.Count == 0) return;
            string name = "";
            string code = "";
            string level = "";

            TreeListViewItem rootItem = new TreeListViewItem("��������");
            TreeListViewItem level_1 = null;
            TreeListViewItem level_2 = null;
            TreeListViewItem level_3 = null;

            rootItem.Items.SortOrder = System.Windows.Forms.SortOrder.None;
            for (int i = 0; i < targetingDs.TargetRegion.Count; i++)
            {
                DataRow Row = targetingDs.TargetRegion.Rows[i];
                name = Row["RegionName"].ToString();
                code = Row["RegionCode"].ToString();
                level = Row["Level"].ToString();

                if (code.Length < 5) code = Utility.Fixlength(code, 2, 5); // 5�ڸ��� �����
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
        }

        private void Init_tvAge()
        {
            tvAgeNodeCount = 0;
            canChecking = false;	// üũó�� �̺�Ʈ���� ó�������� �۵����� �ʵ���

            tvAge.Nodes.Clear();
            tvAge.Nodes.Add(new TreeNode("��ü���ɴ�"));

            if (targetingDs.TargetAge.Count == 0) return;

            TreeNode AgeNode = null;

            for (int i = 0; i < targetingDs.TargetAge.Count; i++)
            {
                DataRow Row = targetingDs.TargetAge.Rows[i];
                string Name = Row["AgeName"].ToString();
                string Code = Row["AgeCode"].ToString();

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

        /// <summary>
        /// ��ž������ TreeView �ʱ�ȭ [E_08]
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
                string Model = Row["CodeName"].ToString();

                // �߰�
                //stbNode = new TreeNode(Name);
                stbNode = new TreeNode(Model);
                stbNode.Tag = Row["Code"].ToString();
                tvStb.Nodes[0].Nodes.Add(stbNode);

                tvStbNodeCount++;
            }

            // 1�ܰ踦 Ȯ���Ͽ� �����ش�.
            tvStb.Nodes[0].Expand();

            canChecking = true;	// üũó�� �̺�Ʈ���� ó�������� �ٽ� �۵��ϵ���
        }

        #endregion

        #region ���� Ÿ���� �׼�ó�� �޼ҵ�

        /// <summary>
        /// �׸����� Row����� ���ο� ������ Ÿ�������� �о�´�
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnGrdRowChanged(object sender, System.EventArgs e)
        {
            if (!IsSearching) // 2011.11.29 JH.Park ��ȸ���� �ƴҰ�쿡�� �����ϵ��� ����
            {
                GetTargetingData();
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
#if (DEBUG)
            Debug.WriteLine(MethodBase.GetCurrentMethod().Name);
#endif
            DisableButton();
            SearchTargeting();
            InitButton();
        }

        private void btnSave_Click(object sender, System.EventArgs e)
        {
#if (DEBUG)
            Debug.WriteLine(MethodBase.GetCurrentMethod().Name);
#endif
            grdExScheduleList.Enabled = true;
            SetTargetingDetailAdd();
        }

        private void btnSaveRate_Click(object sender, System.EventArgs e)
        {
#if (DEBUG)
            Debug.WriteLine(MethodBase.GetCurrentMethod().Name);
#endif
            SetTargetingRateAdd();
        }

        /// <summary>
        /// [E_10] ��������Ÿ���� �����ư Ŭ��
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSaveProfile_Click(object sender, EventArgs e)
        {
            SetTargetingProfileAdd();
        }

        /// <summary>
        /// �˻��� ����
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ebSearchKey_TextChanged(object sender, System.EventArgs e)
        {
#if (DEBUG)
            Debug.WriteLine(MethodBase.GetCurrentMethod().Name);
#endif
            IsNewSearchKey = false;
        }

        /// <summary>
        /// �˻��� Ŭ�� 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ebSearchKey_Click(object sender, System.EventArgs e)
        {
#if (DEBUG)
            Debug.WriteLine(MethodBase.GetCurrentMethod().Name);
#endif
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
#if (DEBUG)
            Debug.WriteLine(MethodBase.GetCurrentMethod().Name);
#endif
            if (e.KeyCode == Keys.Enter)
            {
                SearchTargeting();
            }
        }

        private void tvRegion_AfterCheck(object sender, System.Windows.Forms.TreeViewEventArgs e)
        {
#if (DEBUG)
            Debug.WriteLine(MethodBase.GetCurrentMethod().Name);
#endif
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

        private void tvAge_AfterCheck(object sender, System.Windows.Forms.TreeViewEventArgs e)
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

        /// <summary>
        /// [E_08]
        /// </summary>
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

        private void chkRegionYn_CheckedChanged(object sender, System.EventArgs e)
        {
#if (DEBUG)
            Debug.WriteLine(MethodBase.GetCurrentMethod().Name);
#endif
            if (chkRegionYn.Checked)
            {
                //tvRegion.Enabled = true;
                tlvRegion.Enabled = true; //[E_04]
            }
            else
            {
                clearCheckingRegion();
                // tvRegion.Enabled = false;
                tlvRegion.Enabled = false; //[E_04]
            }
        }

        private void chkTimeYn_CheckedChanged(object sender, System.EventArgs e)
        {
#if (DEBUG)
            Debug.WriteLine(MethodBase.GetCurrentMethod().Name);
#endif
            if (chkTimeYn.Checked)
            {
                gbTime.Enabled = true;
            }
            else
            {
                gbTime.Enabled = false;
            }
        }

        private void chkAgeYn_CheckedChanged(object sender, System.EventArgs e)
        {
#if (DEBUG)
            Debug.WriteLine(MethodBase.GetCurrentMethod().Name);
#endif
            if (chkAgeYn.Checked)
            {
                tvAge.Enabled = true;
                grpAge.Enabled = true;
            }
            else
            {
                SetTargetAge(null);
                tvAge.Enabled = false;
                grpAge.Enabled = false;
            }
        }

        private void chkAgeBtnYn_CheckedChanged(object sender, System.EventArgs e)
        {
#if (DEBUG)
            Debug.WriteLine(MethodBase.GetCurrentMethod().Name);
#endif
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
#if (DEBUG)
            Debug.WriteLine(MethodBase.GetCurrentMethod().Name);
#endif
            if (chkSexYn.Checked)
            {
                grpSex.Enabled = true;
                chkSexMan.Enabled = true;
                chkSexWoman.Enabled = true;
            }
            else
            {
                grpSex.Enabled = false;
                chkSexMan.Enabled = false;
                chkSexWoman.Enabled = false;
            }
        }

        private void chkCollectionYn_CheckedChanged(object sender, System.EventArgs e)
        {
#if (DEBUG)
            Debug.WriteLine(MethodBase.GetCurrentMethod().Name);
#endif
            if (chkCollectionYn.Checked)
            {
                btnAddCollection.Enabled = true;
                btnDeleteCollection.Enabled = true;
            }
            else
            {
                btnAddCollection.Enabled = false;
                btnDeleteCollection.Enabled = false;
            }
        }

        private void chkRateYn_CheckedChanged(object sender, System.EventArgs e)
        {
#if (DEBUG)
            Debug.WriteLine(MethodBase.GetCurrentMethod().Name);
#endif
            if (chkRateYn.Checked)
            {
                rbRateAll.Enabled = true;
                rbRate12.Enabled = true;
                rbRate15.Enabled = true;
                rbRate19.Enabled = true;
                rbRatePlus.Enabled = true;
                rbRateMinus.Enabled = true;
                grpRate.Enabled = true;
                grpRateType.Enabled = true;
            }
            else
            {
                rbRateAll.Enabled = false;
                rbRate12.Enabled = false;
                rbRate15.Enabled = false;
                rbRate19.Enabled = false;
                rbRatePlus.Enabled = false;
                rbRateMinus.Enabled = false;
                grpRate.Enabled = false;
                grpRateType.Enabled = false;
            }
        }

        //���Ϻ�
        private void chkWeekYn_CheckedChanged(object sender, System.EventArgs e)
        {
#if (DEBUG)
            Debug.WriteLine(MethodBase.GetCurrentMethod().Name);
#endif
            if (chkWeekYn.Checked)
            {
                gbDays.Enabled = true;
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
                gbDays.Enabled = false;
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

        /// <summary>
        /// 2slot [E_05]
        /// </summary>		
        private void chkSlotYn_CheckedChanged(object sender, System.EventArgs e)
        {
#if (DEBUG)
            Debug.WriteLine(MethodBase.GetCurrentMethod().Name);
#endif
            if (chkSlotYn.Checked)
            {
                grpSlot.Enabled = true;
                //rbSlotForward.Enabled  = true;
                //rbSlotBackward.Enabled = true;

                rbSlotForward.Checked = true;
            }
            else
            {
                grpSlot.Enabled = false;
                //rbSlotForward.Enabled  = false;
                //rbSlotBackward.Enabled = false;
            }
        }

        /// <summary>
        /// [E_08] ��ž�� üũ�ڽ� �����
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chkStbModel_CheckedChanged(object sender, EventArgs e)
        {
            if (chkStbModel.Checked)
            {
                grpStb.Enabled = true;
            }
            else
            {
                SetTargetStb(null);
                grpStb.Enabled = false;
            }
        }

        /// <summary>
        /// [E_08] ���ɴ� Ʈ���信�� ��尡 �׻� Ȯ��ǵ���.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tvAge_AfterCollapse(object sender, TreeViewEventArgs e)
        {
            tvAge.SelectedNode.ExpandAll();
        }

        /// <summary>
        /// [E_08] ��ž�� Ʈ���信�� ��尡 �׻� Ȯ��ǵ���. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tvStb_AfterCollapse(object sender, TreeViewEventArgs e)
        {
            tvStb.SelectedNode.ExpandAll();
        }

        /// <summary>
        /// [E_09] ��ȣ�������˾� üũ�ڽ� �����
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chkPreference_CheckedChanged(object sender, EventArgs e)
        {
            if (chkPreference.Checked)
            {
                grpPreference.Enabled = true;
            }
            else
            {
                grpPreference.Enabled = false;
            }
        }

        /// <summary>
        /// [E_10] �������� Ÿ���� üũ�ڽ� �����
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chkProfileYn_CheckedChanged(object sender, EventArgs e)
        {
            if (chkProfileYn.Checked)
            {
                btnSaveProfile.Enabled = true;
            }
            else
            {
                btnSaveProfile.Enabled = false;
            }
        }

        /// <summary>
        /// [E_10] �������� - �ŷڵ� �� �����
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tbreliability_ValueChanged(object sender, EventArgs e)
        {
            ttMsg.SetToolTip(tbReliability, tbReliability.Value.ToString());
            //lbreliability.Text = tbReliability.Value.ToString();

            int tbWidth = 17;
            int intOffset = tbWidth * tbReliability.Value;

            //lbreliability.Location = new Point(intOffset, 17); 
        }

        #endregion

        #region ó���޼ҵ�

        /// <summary>
        /// Ÿ����ȸ ���� �Լ�
        /// Ÿ������ ��ȸ
        /// </summary>
        private void SearchTargeting()
        {
            IsSearching = true;
            isSettedTargeting = false;  // �ش籤�� Ÿ���ü������� Ŭ����
            StatusMessage("����Ÿ���� ����Ȳ�� ��ȸ�մϴ�.");

            try
            {
                uiPanelDetail.Text = "Ÿ���ü���";

                // �����͸� �ʱ�ȭ
                targetingModel.Init();

                // �����͸𵨿� ������ ������ ��Ʈ�Ѵ�.
                if (IsNewSearchKey) targetingModel.SearchKey = "";
                else targetingModel.SearchKey = ebSearchKey.Text;

                targetingModel.SearchMediaCode = cbSearchMedia.SelectedItem.Value.ToString();
                targetingModel.SearchRapCode = cbSearchRap.SelectedItem.Value.ToString();
                targetingModel.SearchAgencyCode = cbSearchAgency.SelectedItem.Value.ToString();
                // �������� ������ AdClass�׸����� ������. AdType�� ��ü/��� �������� ���ǰ� �ִ� ������
                targetingModel.SearchAdClass = cbSearchAdType.SelectedItem.Value.ToString();
                targetingModel.SearchAdvertiserCode = "00";
                if (chkAdState_20.Checked) targetingModel.SearchchkAdState_20 = "Y";
                if (chkAdState_30.Checked) targetingModel.SearchchkAdState_30 = "Y";
                if (chkAdState_40.Checked) targetingModel.SearchchkAdState_40 = "Y";

                // ���� Ÿ���� �����ȸ ���񽺸� ȣ���Ѵ�.
                new TargetingManager(systemModel, commonModel).GetTargetingList(targetingModel, "10");

                if (targetingModel.ResultCD.Equals("0000"))
                {
                    Utility.SetDataTableNoEvent(targetingDs.TargetList, targetingModel.TargetingDataSet);
                    StatusMessage(targetingModel.ResultCnt + "���� ���� ������ ��ȸ�Ǿ����ϴ�.");
                    ScrollToCurrent();
                    GetTargetingData();
                    //SetRateText();
                    //SetTargetingCollectionList();  // [E-07] 
                }
            }
            catch (FrameException fe)
            {
                FrameSystem.showMsgForm("���� Ÿ���� ����Ȳ ��ȸ����", new string[] { fe.ErrCode, fe.ResultMsg });
            }
            catch (Exception ex)
            {
                FrameSystem.showMsgForm("���� Ÿ���� ����Ȳ ��ȸ����", new string[] { "", ex.Message });
            }
            finally
            {
                IsSearching = false; // ��ȸ�� Flag ����
            }
        }

        /// <summary>
        /// Ű����ã�� �׸��� Ű�� �ش�Ǵ·ο��..
        /// </summary>
        private void ScrollToCurrent()
        {
#if (DEBUG)
            Debug.WriteLine(MethodBase.GetCurrentMethod().Name);
#endif
            try
            {
                if (dt.Rows.Count < 1) return;
                if (keyItemNo.Length == 0 || keyItemNo == "") return;

                int rowIndex = 0;

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
                FrameSystem.showMsgForm("Ű������", new string[] { fe.ErrCode, fe.ResultMsg });
            }
            catch (Exception ex)
            {
                FrameSystem.showMsgForm("Ű������", new string[] { "", ex.Message });
            }
        }

        /// <summary>
        /// �׸���Row�� ������ ������ Ÿ�������� �о�´�
        /// </summary>
        private void GetTargetingData()
        {
            IsSearching = true;
            int curRow = cm.Position;
            //grdExScheduleList.GetRow().RowIndex
            //int curRow = bsList.Position;

            try
            {
                if (curRow >= 0)
                {
                    keyItemNo = dt.Rows[curRow]["ItemNo"].ToString();		//�����ȣ
                    keyItemName = dt.Rows[curRow]["ItemName"].ToString();   //����� 
                    keyAdType = dt.Rows[curRow]["AdType"].ToString();     //����Ÿ�� 
                    sTypeName = dt.Rows[curRow]["ScheduleTypeName"].ToString();

                    //keyItemNo = aa.ItemNo.ToString();
                    //keyItemName = aa.ItemName;
                    //keyAdType = aa.AdType;
                    //sTypeName = aa.ScheduleTypeName;
#if (DEBUG)
                    Debug.WriteLine(MethodBase.GetCurrentMethod().Name + " -> " + keyItemName);
#endif
                    ResetDetailText();

                    lbNotice.Text = "";
                    uiPanelDetail.Text = "Ÿ���� ���� ���� : [" + keyItemNo + "] " + keyItemName;

                    targetingModel.Init();
                    targetingModel.ItemNo = keyItemNo;

                    // Ÿ���� ����ȸ ���񽺸� ȣ���Ѵ�.
                    new TargetingManager(systemModel, commonModel).GetTargetingDetail2(targetingModel);

                    if (targetingModel.ResultCD.Equals("0000") && targetingModel.ResultCnt > 0)
                    {
                        #region DB���� �о�� ���� �����ش�.
                        Utility.SetDataTable(targetingDs.TargetDetial, targetingModel.DetailDataSet);
                        DataRow row = targetingDs.TargetDetial.Rows[0];

                        #region [ Ÿ�������� ���� 1 �⺻���� ]
                        ebContractAmt.Text = row["ContractAmt"].ToString();       //���������
                        ebContractTot.Text = row["SumAmt"].ToString();

                        // ���������
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

                        // �������� ����
                        if (row["AmtControlRate"].ToString().Length > 0)
                        {
                            udControlRate.Value = Convert.ToInt16(row["AmtControlRate"].ToString());
                        }

                        #endregion

                        #region [ Ÿ�������� ���� 2 �������� ] [E_04] ����
                        // �������� 
                        if (row["TgtRegion1Yn"].ToString().Equals("Y"))
                        {
                            chkRegionYn.Checked = true;
                            tlvRegion.Enabled = true;

                            string[] chkTgtRegionSplit = Utility.SplitByString(row["TgtRegion1"].ToString(), "^");
                            setTargetRegion(chkTgtRegionSplit);
                        }
                        else
                        {
                            chkRegionYn.Checked = false;
                            tlvRegion.Enabled = false;
                        }

                        #endregion

                        #region [ Ÿ�������� ���� 3 ���ɴ����� ]
                        // ���⿬�ɴ�
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
                        #endregion

                        #region [ Ÿ�������� ���� 4 ���ɱ��� ]
                        if (row["TgtAgeBtnYn"].ToString().Equals("Y"))
                        {
                            chkAgeBtnYn.Checked = true;
                            udAgeBtnBegin.Enabled = true;
                            udAgeBtnEnd.Enabled = true;

                            if (row["TgtAgeBtnBegin"].ToString().Length > 0)
                                udAgeBtnBegin.Value = Convert.ToInt16(row["TgtAgeBtnBegin"].ToString());
                            else
                                udAgeBtnBegin.Value = 0;

                            if (row["TgtAgeBtnEnd"].ToString().Length > 0)
                                udAgeBtnEnd.Value = Convert.ToInt16(row["TgtAgeBtnEnd"].ToString());
                            else
                                udAgeBtnEnd.Value = 0;
                        }
                        else
                        {
                            chkAgeBtnYn.Checked = false;
                            udAgeBtnBegin.Enabled = false;
                            udAgeBtnEnd.Enabled = false;
                        }
                        #endregion

                        #region [ Ÿ�������� ���� 5 ���� ]
                        if (row["TgtSexYn"].ToString().Equals("Y"))
                        {
                            chkSexYn.Checked = true;
                            chkSexMan.Enabled = true;
                            chkSexWoman.Enabled = true;

                            if (row["TgtSexMan"].ToString().Equals("Y")) chkSexMan.Checked = true;
                            else chkSexMan.Checked = false;

                            if (row["TgtSexWoman"].ToString().Equals("Y")) chkSexWoman.Checked = true;
                            else chkSexWoman.Checked = false;
                        }
                        else
                        {
                            chkSexYn.Checked = false;
                            chkSexMan.Enabled = false;
                            chkSexWoman.Enabled = false;
                        }
                        #endregion

                        #region [ Ÿ�������� ���� 6 ��žŸ�ٱ� ]
                        // Ÿ�ٱ���
                        if (row["TgtCollectionYn"].ToString().Equals("Y") || row["TgtCollectionYn"].ToString().Equals("-"))
                        {
                            keyCollectionYn = row["TgtCollectionYn"].ToString();
                            chkCollectionYn.Checked = true;

                            // Ÿ�ٱ������Ǽ�
                            int cnt = Convert.ToInt32(row["TgtCollection"].ToString());
                            // 2014.02.05 �߰�
                            // 1.5���� ������ ������ ����
                            // �ܼ��Ǽ����� �����ڵ帮�Ϲ������ ������.
                            //  0 : ����
                            //  1 : ���, �űԿ��� �̵�ϵǾ� �ֽ�
                            //  2 : ����, ���ʿ� ��ϵǾ� ������ ����/���� �ڵ尡 ����
                            //  3 : ����, �ű��ʿ� �������� ��ϵǾ� �ֽ�( ����/���� ���� 1�Ǿ�?)
                            if (cnt == 0)
                            {
                                //lblCollectionChk.ForeColor  = Color.Black;
                                //lblCollectionChk.BackColor  = Color.Transparent;
                                //lblCollectionChk.Text = "����";
                                lblCollectionChk.ImageIndex = -1;
                                this.ttMsg.SetToolTip(this.lblCollectionChk, "������� �Դϴ�.");
                            }
                            else if (cnt == 1)
                            {
                                //lblCollectionChk.ForeColor = Color.Black;
                                //lblCollectionChk.BackColor = Color.Transparent;
                                //lblCollectionChk.Text = "���";
                                lblCollectionChk.ImageIndex = 4;
                                this.ttMsg.SetToolTip(this.lblCollectionChk, "���!!!\n������������ ����� ������ �����մϴ�.\n���� �������� �ű��Է��Ͻʽÿ�.");

                            }
                            else
                            {
                                //lblCollectionChk.ForeColor = Color.Black;
                                //lblCollectionChk.BackColor = Color.Transparent;
                                //lblCollectionChk.Text = "����";
                                lblCollectionChk.ImageIndex = 3;
                                if (cnt == 2)
                                {
                                    this.ttMsg.SetToolTip(this.lblCollectionChk, "����!!!\n����/���� ������ ���� �մϴ�\nȮ�� �Ͻʽÿ�");
                                }
                                else if (cnt == 9)
                                {
                                    this.ttMsg.SetToolTip(this.lblCollectionChk, "����!!!\n��ϵ� ������ �������� �ʽ��ϴ�\n������ ����Ͻʽÿ�");
                                }
                                else
                                {
                                    this.ttMsg.SetToolTip(this.lblCollectionChk, "����!!!\n���� ������ ������ ��ϵǾ� �ֽ��ϴ�\nȮ�� �Ͻʽÿ�");
                                }
                            }
                            //lblCollectionChk.Visible = true;
                        }
                        else
                        {
                            keyCollectionYn = row["TgtCollectionYn"].ToString();
                            chkCollectionYn.Checked = false;

                            //lblCollectionChk.ForeColor = Color.Black;
                            //lblCollectionChk.BackColor = Color.Transparent;
                            //lblCollectionChk.Text = "";
                            lblCollectionChk.ImageIndex = -1;
                            //lblCollectionChk.Visible = false;
                        }

                        #endregion

                        #region [ Ÿ�������� ���� 7 ������ ]
                        if (row["TgtRateYn"].ToString().Equals("Y") || row["TgtRateYn"].ToString().Equals("-"))
                        {
                            chkRateYn.Checked = true;
                            rbRateAll.Enabled = true;
                            rbRate12.Enabled = true;
                            rbRate15.Enabled = true;
                            rbRate19.Enabled = true;

                            rbRatePlus.Enabled = true;
                            rbRateMinus.Enabled = true;

                            switch (row["TgtRate"].ToString())
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
                                default:
                                    rbRateAll.Checked = true;
                                    break;
                            }

                            if (row["TgtRateYn"].ToString().Equals("-")) rbRateMinus.Checked = true;
                            else rbRatePlus.Checked = true;
                        }
                        else
                        {
                            chkRateYn.Checked = false;
                            rbRateAll.Enabled = false;
                            rbRate12.Enabled = false;
                            rbRate15.Enabled = false;
                            rbRate19.Enabled = false;

                            rbRatePlus.Enabled = false;
                            rbRateMinus.Enabled = false;
                        }
                        #endregion

                        #region [ Ÿ�������� ���� 9 �������� ]
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

                            //�������� üũ�ڽ��� ���� �� �ʵ忡 ������ �����ö�..'^'�߶� string�迭�� �ִ´�.
                            string[] chkTgtWeekSplit = Utility.SplitByString(row["TgtWeek"].ToString(), "^");
                            //string�迭�� ���� ������ ����..
                            for (int i = 0; i < chkTgtWeekSplit.Length; i++)
                            {
                                //�������� ���°����� string������ ��´�..
                                string chkTgtWeek = chkTgtWeekSplit[i];
                                //string������ case������ �ش簪���� ���Ͽ�..�ش� üũ�ڽ��� ��Ʈ���Ѵ�.
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

                        #region [ Ÿ�������� ���� 10 ����ä�� ���࿩�� ]
                        //2014/09/05 ���� by Youngil.Yi
                        if (row["TgtPPxYn"].ToString().Equals("Y"))
                        {
                            chkPPx.Checked = true;
                            chkPPxOnly.Checked = false;
                        }
                        //TgtPPxYn�� ���� S�ϰ��� Only����ä��(chkPPxOnly)�� ���� true�� �Ѵ�.
                        else if (row["TgtPPxYn"].ToString().Equals("S"))
                        {
                            chkPPx.Checked = false;
                            chkPPxOnly.Checked = true;
                        }
                        else
                        {
                            chkPPx.Checked = false;
                            chkPPxOnly.Checked = false;

                        }
                        #endregion

                        #region [ Ÿ�������� ���� 11 ���ν�û ���࿩�� ]
                        if (row["TgtPVSYn"].ToString().Equals("Y"))
                        {
                            chkPVS.Checked = true;
                            chkPVS.FormatStyle.FontBold = Janus.Windows.UI.TriState.True;
                        }
                        else
                        {
                            chkPVS.Checked = false;
                            chkPVS.FormatStyle.FontBold = Janus.Windows.UI.TriState.False;
                        }
                        #endregion

                        #region [ Ÿ�������� ���� 11 ��������� ]
                        if (row["TgtFreqYn"].ToString().Equals("Y"))
                        {
                            chkFreq.Checked = true;
                            grpFreq.Enabled = true;
                            freqDay.Value = Convert.ToInt32(row["TgtFreqDay"].ToString());
                            freqPeriod.Value = Convert.ToInt32(row["TgtFreqPeriod"].ToString());
                        }
                        else
                        {
                            chkFreq.Checked = false;
                            grpFreq.Enabled = false;
                            freqDay.Value = 0;
                            freqPeriod.Value = 0;
                        }
                        #endregion

                        #region [ Ÿ�������� ���� 12 �����ȣ ����]
                        // �����ȣ[E_02]
                        if (row["TgtZipYn"].ToString().Equals("Y"))
                        {
                            chkZip.Checked = true;
                            ebZipCode.Enabled = true;
                            btnNewZip.Enabled = true;
                            btnAddZip.Enabled = true;

                            ebZipCode.Text = row["TgtZip"].ToString();
                        }
                        else
                        {
                            chkZip.Checked = false;
                            ebZipCode.Enabled = false;
                            btnNewZip.Enabled = false;
                            btnAddZip.Enabled = false;
                            ebZipCode.Text = "";
                        }
                        #endregion

                        #region [ Ÿ�������� ���� 13 �ð����� ] [E_03]
                        // ����ð���
                        // ���� �������� �Ŀ� üũ
                        if (row["TgtTimeYn"].ToString().Equals("Y"))
                        {
                            chkTimeYn.Checked = true;
                            gbTime.Enabled = true;
                            strTimes = row["TgtTime"].ToString();
                            setTimesBinding();
                        }
                        else
                        {
                            strTimes = string.Empty;
                            chkTimeYn.Checked = false;
                            rbWeekAll.Checked = true;
                            gbTime.Enabled = false;
                        }
                        #endregion

                        #region [ Ÿ�������� ���� 14 Slot��ġ] [E_05]
                        if (row["SlotExt"] != null && Convert.ToInt32(row["SlotExt"]) > 0)
                        {
                            int slot = Convert.ToInt32(row["SlotExt"]);
                            chkSlotYn.Checked = true;
                            if (slot == 3)
                                rbSlotForward.Checked = true; //2slot ��
                            else if (slot == 6)
                                rbSlotBackward.Checked = true; //2slot ��				
                        }
                        else
                        {
                            chkSlotYn.Checked = false;
                        }
                        #endregion

                        #region [Ÿ�������� 13 ��ž�𵨺� ] [E_08]

                        if (row["TgtStbTypeYn"].ToString().Equals("Y"))
                        {
                            chkStbModel.Checked = true;
                            grpStb.Enabled = true;

                            string[] chkTgtAgeSplit = Utility.SplitByString(row["TgtStbType"].ToString(), "^");
                            SetTargetStb(chkTgtAgeSplit);
                        }
                        else
                        {
                            chkStbModel.Checked = false;
                            grpStb.Enabled = false;
                        }

                        #endregion

                        #region [Ÿ�������� 14 ��ȣ�������˾� ] [E_09]

                        if (row["TgtPrefYn"].ToString().Equals("Y"))
                        {
                            chkPreference.Checked = true;
                            grpPreference.Enabled = true;

                            if (row["TgtPrefRate"].ToString().Length > 0)
                            {
                                udPrefRate.Value = Convert.ToInt32(row["TgtPrefRate"].ToString());
                            }

                            if (row["TgtPrefNosendYn"].ToString().Equals("Y"))
                            {
                                chkResponse.Checked = true;
                            }
                            else
                            {
                                chkResponse.Checked = false;
                            }
                        }
                        else
                        {
                            chkPreference.Checked = false;
                            grpPreference.Enabled = false;
                            udPrefRate.Value = 50;
                        }

                        #endregion

                        #region [Ÿ�������� 15 �������� Ÿ���� ] [E_10]

                        if (row["TgtProfileYn"].ToString().Equals("Y"))
                        {
                            chkProfileYn.Checked = true;
                            uiTabPage4.Enabled = true;

                            // �������� üũ�ڽ��� ���� �� �ʵ忡 ������ �����ö�..'^'�߶� string�迭�� �ִ´�.
                            string[] chkProfileAgeSplit = Utility.SplitByString(row["TgtProfile"].ToString(), "^");
                            // string �迭�� ���� ������ ����
                            for (int i = 0; i < chkProfileAgeSplit.Length; i++)
                            {
                                // ���� ���� ���� ������ string ������ ��´�
                                string chkProfileAge = chkProfileAgeSplit[i];

                                // ��������Ÿ���� ���� ���ɴ� ���� �׷�ڽ��� �ִ� üũ�ڽ��� ���鼭 �ش� üũ�ڽ��� üũ��.
                                foreach (Janus.Windows.EditControls.UICheckBox cb in grbProfileAge.Controls)
                                {
                                    if (chkProfileAge.ToString() == cb.Tag.ToString())
                                        cb.Checked = true;
                                }
                            }

                            if (Convert.ToInt32(row["TgtReliability"].ToString()) != 0)
                            {
                                tbReliability.Value = Convert.ToInt32(row["TgtReliability"].ToString());
                                //lbreliability.Text = tbReliability.Value.ToString();
                            }
                            else
                            {
                                // �ŷڵ� ���� 0�� ��� Defalt������ 5�� ����.
                                //tbReliability.Value = 5;
                                //lbreliability.Text = tbReliability.Value.ToString();
                            }
                        }
                        else
                        {
                            chkProfileYn.Checked = false;
                            foreach (Janus.Windows.EditControls.UICheckBox cb in grbProfileAge.Controls)
                            {
                                cb.Checked = false;
                            }

                            uiTabPage4.Enabled = false;
                            tbReliability.Value = 5;
                            //lbreliability.Text = tbReliability.Value.ToString();
                        }

                        #endregion

                        #region [Ÿ�������� 16 ������� �̼��� ���]
                        // ���� ���� �������� �����
                        // 2014/02 YS.Jang ���ʿ��� �ȳ��ε� �Ͽ� ����� ����
                        if ((!row["TgtRegion1"].ToString().Equals("") && !row["TgtRegion1"].ToString().Equals(null)) || (!row["TgtTime"].ToString().Equals("") && !row["TgtTime"].ToString().Equals(null)) || (!row["TgtWeek"].ToString().Equals("") && !row["TgtWeek"].ToString().Equals(null)))
                        {
                            DataRow[] FoundRows = targetingDs.TargetRate.Select("ItemNo = '" + keyItemNo + "'");
                            //FoundRows�� 0�̸� ������ ������ �߰�
                            if (FoundRows.Length == 0)
                            {
                                lbNotice.Text = "Ÿ���ú����� �������� �ʾҽ��ϴ�.";
                                lbNotice.Visible = true;
                            }
                        }
                        #endregion

                        #endregion

                        isSettedTargeting = true;  // �ش籤�� Ÿ���ü������� ��Ʈ

                        //SetRateText();
                        //SetTargetingCollectionList(); //[E-07] ����Ÿ���ø���Ʈ ��ȸ �߰�
                        DetailReadOnly(true);
                    }
                    else
                    {
                        ebContractAmt.Text = dt.Rows[curRow]["ContractAmt"].ToString();
                        lbNotice.Text = "Ÿ���������� �������� �ʾҽ��ϴ�.";
                        lbNotice.Visible = true;
                        chkSlotYn.Checked = false; // [E_05]
                        isSettedTargeting = false;  // �ش籤�� Ÿ���ü������� ��Ʈ
                    }

                    // �󼼳����� ��ȸ�� �Ŀ��� ������ �� �� ���� �Ѵ�.
                    // �����۾��� ������ư�� �����Ŀ� �����ϰ� �Ѵ�.
                    if (canUpdate)
                    {
                        btnModify.Enabled = true;
                        btnSave.Enabled = false;
                        btnCancel.Enabled = false;
                    }
                }
            }
            catch (FrameException fe)
            {
                FrameSystem.showMsgForm("���� Ÿ���� ��ȸ����", new string[] { fe.ErrCode, fe.ResultMsg });
            }
            catch (Exception ex)
            {
                FrameSystem.showMsgForm("���� Ÿ���� ��ȸ����", new string[] { "", ex.Message });
            }
            finally
            {
                StatusMessage("�غ�");
                IsSearching = false; // ��ȸ�� Flag ����
            }
        }

        /// <summary>
        /// ���� Ÿ���� ���������� ��Ʈ
        /// </summary>
        private void SetRateText()
        {
#if (DEBUG)
            Debug.WriteLine(MethodBase.GetCurrentMethod().Name);
#endif
            ResetRateText();
            int curRow = cm.Position;

            if (curRow >= 0)
            {
                // �����͸� �ʱ�ȭ
                //targetingModel.Init();
                //targetingModel.ItemNo		= keyItemNo;

                //new TargetingManager(systemModel,commonModel).GetTargetingDetail2(targetingModel);

                if (targetingModel.ResultCD.Equals("0000") && targetingModel.ResultCnt > 0)
                {
                    //Utility.SetDataTable(targetingDs.TargetDetial, targetingModel.DetailDataSet);		
                    DataRow rowDetail = targetingDs.TargetDetial.Rows[0];

                    string[] chkTgtRegionSplit = Utility.SplitByString(rowDetail["TgtRegion1"].ToString(), "^");
                    for (int j = 0; j < chkTgtRegionSplit.Length; j++)
                    {
                        //�������� ���°����� string������ ��´�..
                        string regionRate = chkTgtRegionSplit[j];

                        #region ��������
                        //string������ case������ �ش簪���� ���Ͽ�..�ش� üũ�ڽ��� ��Ʈ���Ѵ�.
                        switch (regionRate)
                        {
                            case "01000":
                                udRegionRate1.Enabled = true;
                                break;
                            case "05000":
                                udRegionRate2.Enabled = true;
                                break;
                            case "06000":
                                udRegionRate3.Enabled = true;
                                break;
                            case "07000":
                                udRegionRate4.Enabled = true;
                                break;
                            case "08000":
                                udRegionRate5.Enabled = true;
                                break;
                            case "09000":
                                udRegionRate6.Enabled = true;
                                break;
                            case "10000":
                                udRegionRate7.Enabled = true;
                                break;
                            case "11000":
                                udRegionRate8.Enabled = true;
                                break;
                            case "12000":
                                udRegionRate9.Enabled = true;
                                break;
                            case "13000":
                                udRegionRate10.Enabled = true;
                                break;
                            case "14000":
                                udRegionRate11.Enabled = true;
                                break;
                            case "15000":
                                udRegionRate12.Enabled = true;
                                break;
                            case "16000":
                                udRegionRate13.Enabled = true;
                                break;
                            case "17000":
                                udRegionRate14.Enabled = true;
                                break;
                            case "18000":
                                udRegionRate15.Enabled = true;
                                break;
                            case "19000":
                                udRegionRate16.Enabled = true;
                                break;
                            case "90000":
                                udRegionRate17.Enabled = true;
                                break;
                        }
                        #endregion
                    }

                    #region Ÿ�Ӽ���
                    string[] chkTgtTimeSplit = Utility.SplitByString(rowDetail["TgtTime"].ToString(), "^");
                    for (int j = 0; j < chkTgtTimeSplit.Length; j++)
                    {
                        //�������� ���°����� string������ ��´�..
                        string timeRate = chkTgtTimeSplit[j];
                        //string������ case������ �ش簪���� ���Ͽ�..�ش� üũ�ڽ��� ��Ʈ���Ѵ�.
                        switch (timeRate)
                        {
                            case "00":
                                udTimeRate0.Enabled = true;
                                break;
                            case "01":
                                udTimeRate1.Enabled = true;
                                break;
                            case "02":
                                udTimeRate2.Enabled = true;
                                break;
                            case "03":
                                udTimeRate3.Enabled = true;
                                break;
                            case "04":
                                udTimeRate4.Enabled = true;
                                break;
                            case "05":
                                udTimeRate5.Enabled = true;
                                break;
                            case "06":
                                udTimeRate6.Enabled = true;
                                break;
                            case "07":
                                udTimeRate7.Enabled = true;
                                break;
                            case "08":
                                udTimeRate8.Enabled = true;
                                break;
                            case "09":
                                udTimeRate9.Enabled = true;
                                break;
                            case "10":
                                udTimeRate10.Enabled = true;
                                break;
                            case "11":
                                udTimeRate11.Enabled = true;
                                break;
                            case "12":
                                udTimeRate12.Enabled = true;
                                break;
                            case "13":
                                udTimeRate13.Enabled = true;
                                break;
                            case "14":
                                udTimeRate14.Enabled = true;
                                break;
                            case "15":
                                udTimeRate15.Enabled = true;
                                break;
                            case "16":
                                udTimeRate16.Enabled = true;
                                break;
                            case "17":
                                udTimeRate17.Enabled = true;
                                break;
                            case "18":
                                udTimeRate18.Enabled = true;
                                break;
                            case "19":
                                udTimeRate19.Enabled = true;
                                break;
                            case "20":
                                udTimeRate20.Enabled = true;
                                break;
                            case "21":
                                udTimeRate21.Enabled = true;
                                break;
                            case "22":
                                udTimeRate22.Enabled = true;
                                break;
                            case "23":
                                udTimeRate23.Enabled = true;
                                break;
                        }
                    }
                    #endregion

                    #region ���ϼ���
                    string[] chkTgtWeekSplit = Utility.SplitByString(rowDetail["TgtWeek"].ToString(), "^");
                    for (int j = 0; j < chkTgtWeekSplit.Length; j++)
                    {
                        //�������� ���°����� string������ ��´�..
                        string weekRate = chkTgtWeekSplit[j];
                        //string������ case������ �ش簪���� ���Ͽ�..�ش� üũ�ڽ��� ��Ʈ���Ѵ�.
                        switch (weekRate)
                        {
                            case "2":
                                if (udSunRate.Enabled.Equals(false))
                                {
                                    udMonRate.Enabled = true;
                                }
                                else
                                {
                                    udMonRate.Value = 0;
                                    udThuRate.Value = 0;
                                    udWedRate.Value = 0;
                                    udTheRate.Value = 0;
                                    udFriRate.Value = 0;
                                    udSatRate.Value = 0;
                                    udSunRate.Value = 0;
                                    ebWeekTot.Text = "0";
                                }
                                udMonRate.Enabled = true;
                                break;
                            case "3":
                                udThuRate.Enabled = true;
                                break;
                            case "4":
                                udWedRate.Enabled = true;
                                break;
                            case "5":
                                udTheRate.Enabled = true;
                                break;
                            case "6":
                                udFriRate.Enabled = true;
                                break;
                            case "7":
                                udSatRate.Enabled = true;
                                break;
                            case "1":
                                udSunRate.Enabled = true;
                                break;
                        }
                    }
                    #endregion

                    // ���� Ÿ���� ����ȸ ���񽺸� ȣ���Ѵ�.
                    new TargetingManager(systemModel, commonModel).GetTargetingRate(targetingModel);

                    if (targetingModel.ResultCD.Equals("0000") && targetingModel.ResultCnt > 0)
                    {
                        Utility.SetDataTable(targetingDs.TargetRate, targetingModel.RateDataSet);

                        for (int i = 0; i < targetingDs.TargetRate.Rows.Count; i++)
                        {
                            DataRow row = targetingDs.TargetRate.Rows[i];

                            #region ��������
                            if (row["Type"].ToString().Equals("01"))
                            {
                                if (row["Rate1"].ToString().Length > 0)
                                {
                                    udRegionRate1.Value = Convert.ToInt16(row["Rate1"].ToString());
                                }
                                else
                                {
                                    udRegionRate1.Value = 0;
                                    udRegionRate1.Enabled = false;
                                }
                                if (row["Rate2"].ToString().Length > 0)
                                {
                                    udRegionRate2.Value = Convert.ToInt16(row["Rate2"].ToString());
                                }
                                else
                                {
                                    udRegionRate2.Value = 0;
                                }
                                if (row["Rate3"].ToString().Length > 0)
                                {
                                    udRegionRate3.Value = Convert.ToInt16(row["Rate3"].ToString());
                                }
                                else
                                {
                                    udRegionRate3.Value = 0;
                                }
                                if (row["Rate4"].ToString().Length > 0)
                                {
                                    udRegionRate4.Value = Convert.ToInt16(row["Rate4"].ToString());
                                }
                                else
                                {
                                    udRegionRate4.Value = 0;
                                }
                                if (row["Rate5"].ToString().Length > 0)
                                {
                                    udRegionRate5.Value = Convert.ToInt16(row["Rate5"].ToString());
                                }
                                else
                                {
                                    udRegionRate5.Value = 0;
                                }
                                if (row["Rate6"].ToString().Length > 0)
                                {
                                    udRegionRate6.Value = Convert.ToInt16(row["Rate6"].ToString());
                                }
                                else
                                {
                                    udRegionRate6.Value = 0;
                                }
                                if (row["Rate7"].ToString().Length > 0)
                                {
                                    udRegionRate7.Value = Convert.ToInt16(row["Rate7"].ToString());
                                }
                                else
                                {
                                    udRegionRate7.Value = 0;
                                }
                                if (row["Rate8"].ToString().Length > 0)
                                {
                                    udRegionRate8.Value = Convert.ToInt16(row["Rate8"].ToString());
                                }
                                else
                                {
                                    udRegionRate8.Value = 0;
                                }
                                if (row["Rate9"].ToString().Length > 0)
                                {
                                    udRegionRate9.Value = Convert.ToInt16(row["Rate9"].ToString());
                                }
                                else
                                {
                                    udRegionRate9.Value = 0;
                                }
                                if (row["Rate10"].ToString().Length > 0)
                                {
                                    udRegionRate10.Value = Convert.ToInt16(row["Rate10"].ToString());
                                }
                                else
                                {
                                    udRegionRate10.Value = 0;
                                }
                                if (row["Rate11"].ToString().Length > 0)
                                {
                                    udRegionRate11.Value = Convert.ToInt16(row["Rate11"].ToString());
                                }
                                else
                                {
                                    udRegionRate11.Value = 0;
                                }
                                if (row["Rate12"].ToString().Length > 0)
                                {
                                    udRegionRate12.Value = Convert.ToInt16(row["Rate12"].ToString());
                                }
                                else
                                {
                                    udRegionRate12.Value = 0;
                                }
                                if (row["Rate13"].ToString().Length > 0)
                                {
                                    udRegionRate13.Value = Convert.ToInt16(row["Rate13"].ToString());
                                }
                                else
                                {
                                    udRegionRate13.Value = 0;
                                }
                                if (row["Rate14"].ToString().Length > 0)
                                {
                                    udRegionRate14.Value = Convert.ToInt16(row["Rate14"].ToString());
                                }
                                else
                                {
                                    udRegionRate14.Value = 0;
                                }
                                if (row["Rate15"].ToString().Length > 0)
                                {
                                    udRegionRate15.Value = Convert.ToInt16(row["Rate15"].ToString());
                                }
                                else
                                {
                                    udRegionRate15.Value = 0;
                                }
                                if (row["Rate16"].ToString().Length > 0)
                                {
                                    udRegionRate16.Value = Convert.ToInt16(row["Rate16"].ToString());
                                }
                                else
                                {
                                    udRegionRate16.Value = 0;
                                }
                                if (row["Rate17"].ToString().Length > 0)
                                {
                                    udRegionRate17.Value = Convert.ToInt16(row["Rate17"].ToString());
                                }
                                else
                                {
                                    udRegionRate17.Value = 0;
                                }
                                TotRegionText();
                            }
                            #endregion

                            #region �ð�����
                            if (row["Type"].ToString().Equals("02"))
                            {
                                udTimeRate0.Value = 0;
                                udTimeRate1.Value = 0;
                                udTimeRate2.Value = 0;
                                udTimeRate3.Value = 0;
                                udTimeRate4.Value = 0;
                                udTimeRate5.Value = 0;
                                udTimeRate6.Value = 0;
                                udTimeRate7.Value = 0;
                                udTimeRate8.Value = 0;
                                udTimeRate9.Value = 0;
                                udTimeRate10.Value = 0;
                                udTimeRate11.Value = 0;
                                udTimeRate12.Value = 0;
                                udTimeRate13.Value = 0;
                                udTimeRate14.Value = 0;
                                udTimeRate15.Value = 0;
                                udTimeRate16.Value = 0;
                                udTimeRate17.Value = 0;
                                udTimeRate18.Value = 0;
                                udTimeRate19.Value = 0;
                                udTimeRate20.Value = 0;
                                udTimeRate21.Value = 0;
                                udTimeRate22.Value = 0;
                                udTimeRate23.Value = 0;

                                if (row["Rate1"].ToString().Length > 0) udTimeRate0.Value = Convert.ToInt16(row["Rate1"].ToString());
                                if (row["Rate2"].ToString().Length > 0) udTimeRate1.Value = Convert.ToInt16(row["Rate2"].ToString());
                                if (row["Rate3"].ToString().Length > 0) udTimeRate2.Value = Convert.ToInt16(row["Rate3"].ToString());
                                if (row["Rate4"].ToString().Length > 0) udTimeRate3.Value = Convert.ToInt16(row["Rate4"].ToString());
                                if (row["Rate5"].ToString().Length > 0) udTimeRate4.Value = Convert.ToInt16(row["Rate5"].ToString());
                                if (row["Rate6"].ToString().Length > 0) udTimeRate5.Value = Convert.ToInt16(row["Rate6"].ToString());
                                if (row["Rate7"].ToString().Length > 0) udTimeRate6.Value = Convert.ToInt16(row["Rate7"].ToString());
                                if (row["Rate8"].ToString().Length > 0) udTimeRate7.Value = Convert.ToInt16(row["Rate8"].ToString());
                                if (row["Rate9"].ToString().Length > 0) udTimeRate8.Value = Convert.ToInt16(row["Rate9"].ToString());
                                if (row["Rate10"].ToString().Length > 0) udTimeRate9.Value = Convert.ToInt16(row["Rate10"].ToString());
                                if (row["Rate11"].ToString().Length > 0) udTimeRate10.Value = Convert.ToInt16(row["Rate11"].ToString());
                                if (row["Rate12"].ToString().Length > 0) udTimeRate11.Value = Convert.ToInt16(row["Rate12"].ToString());
                                if (row["Rate13"].ToString().Length > 0) udTimeRate12.Value = Convert.ToInt16(row["Rate13"].ToString());
                                if (row["Rate14"].ToString().Length > 0) udTimeRate13.Value = Convert.ToInt16(row["Rate14"].ToString());
                                if (row["Rate15"].ToString().Length > 0) udTimeRate14.Value = Convert.ToInt16(row["Rate15"].ToString());
                                if (row["Rate16"].ToString().Length > 0) udTimeRate15.Value = Convert.ToInt16(row["Rate16"].ToString());
                                if (row["Rate17"].ToString().Length > 0) udTimeRate16.Value = Convert.ToInt16(row["Rate17"].ToString());
                                if (row["Rate18"].ToString().Length > 0) udTimeRate17.Value = Convert.ToInt16(row["Rate18"].ToString());
                                if (row["Rate19"].ToString().Length > 0) udTimeRate18.Value = Convert.ToInt16(row["Rate19"].ToString());
                                if (row["Rate20"].ToString().Length > 0) udTimeRate19.Value = Convert.ToInt16(row["Rate20"].ToString());
                                if (row["Rate21"].ToString().Length > 0) udTimeRate20.Value = Convert.ToInt16(row["Rate21"].ToString());
                                if (row["Rate22"].ToString().Length > 0) udTimeRate21.Value = Convert.ToInt16(row["Rate22"].ToString());
                                if (row["Rate23"].ToString().Length > 0) udTimeRate22.Value = Convert.ToInt16(row["Rate23"].ToString());
                                if (row["Rate24"].ToString().Length > 0) udTimeRate23.Value = Convert.ToInt16(row["Rate24"].ToString());

                                TotTimeText();
                            }
                            #endregion

                            #region ���ϼ���
                            if (row["Type"].ToString().Equals("03"))
                            {
                                if (row["Rate2"].ToString().Length > 0)
                                {
                                    udMonRate.Value = Convert.ToInt16(row["Rate2"].ToString());
                                }
                                else
                                {
                                    udMonRate.Value = 0;
                                }
                                if (row["Rate3"].ToString().Length > 0)
                                {
                                    udThuRate.Value = Convert.ToInt16(row["Rate3"].ToString());
                                }
                                else
                                {
                                    udThuRate.Value = 0;
                                }
                                if (row["Rate4"].ToString().Length > 0)
                                {
                                    udWedRate.Value = Convert.ToInt16(row["Rate4"].ToString());
                                }
                                else
                                {
                                    udWedRate.Value = 0;
                                }
                                if (row["Rate5"].ToString().Length > 0)
                                {
                                    udTheRate.Value = Convert.ToInt16(row["Rate5"].ToString());
                                }
                                else
                                {
                                    udTheRate.Value = 0;
                                }
                                if (row["Rate6"].ToString().Length > 0)
                                {
                                    udFriRate.Value = Convert.ToInt16(row["Rate6"].ToString());
                                }
                                else
                                {
                                    udFriRate.Value = 0;
                                }
                                if (row["Rate7"].ToString().Length > 0)
                                {
                                    udSatRate.Value = Convert.ToInt16(row["Rate7"].ToString());
                                }
                                else
                                {
                                    udSatRate.Value = 0;
                                }
                                if (row["Rate1"].ToString().Length > 0)
                                {
                                    udSunRate.Value = Convert.ToInt16(row["Rate1"].ToString());
                                }
                                else
                                {
                                    udSunRate.Value = 0;
                                }
                                TotWeekText();
                            }
                            #endregion
                        }
                    }
                }
                if (canUpdate) btnSaveRate.Enabled = true;
            }

            StatusMessage("�غ�");
        }

        private void SetTargetAge(string[] chkList)
        {
            canChecking = false;	// üũó�� �̺�Ʈ���� ó�������� �۵����� �ʵ���

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
#if (DEBUG)
            Debug.WriteLine(MethodBase.GetCurrentMethod().Name);
#endif
            chkMon.Checked = false;
            chkThu.Checked = false;
            chkWed.Checked = false;
            chkThe.Checked = false;
            chkFri.Checked = false;
            chkSat.Checked = false;
            chkSun.Checked = false;
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

        #endregion

        #region ���� Ÿ���� ���������� ����

        /// <summary>
        /// ���� Ÿ���� ���������� ����
        /// </summary>
        private void SetTargetingDetailAdd()
        {
#if (DEBUG)
            Debug.WriteLine(MethodBase.GetCurrentMethod().Name);
#endif
            StatusMessage("Ÿ���� ������ �����մϴ�.");

            if (ebContractAmt.Text.Trim().Length == 0)
            {
                MessageBox.Show("�����๰���� �Էµ��� �ʾҽ��ϴ�.", "Ÿ���ó��� ����", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ebContractAmt.Focus();
                return;
            }

            // ���ϰ� �ð� Ÿ���� ����/�ָ� ������ üũ[E_03]
            if (chkWeekYn.Checked) // ���Ϻ� Ÿ�� ������ �� ���
            {
                if (checkingWeeknTime() == false)
                {
                    MessageBox.Show("���ϰ� �ð��� Ÿ���� ������ ���� �ʽ��ϴ�.!", "���Ϲ׽ð�Ÿ����", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
            }

            try
            {
                //���� ���� ���� �ʱ�ȭ ���ش�.
                targetingModel.Init();

                int curRow = cm.Position;

                // �����͸𵨿� ������ ������ ��Ʈ�Ѵ�.
                //�����ȣ
                targetingModel.ItemNo = keyItemNo;
                targetingModel.ItemName = keyItemName;

                // ������
                targetingModel.ContractAmt = ebContractAmt.Text.Replace(",", "");

                // ��
                //targetingModel.PriorityCd    = cbPriorityCd.SelectedValue.ToString();
                targetingModel.PriorityCd = "0"; // �������� Default�� 0�� ���� 2012.02.21 RH.Jung from YS.Jang

                // ���������
                if (rbControlYn_Y.Checked) targetingModel.AmtControlYn = "Y";
                if (rbControlYn_N.Checked) targetingModel.AmtControlYn = "N";

                // �����������
                targetingModel.AmtControlRate = udControlRate.Value.ToString();

                //�������� ����
                #region [E_04] ���� �� �ҽ�
                /* 				
				if(chkRegionYn.Checked)
				{
					targetingModel.TgtRegion1Yn = "Y";
					targetingModel.TgtRegion1   = GetAllNodeCheckedTag(tvRegion.Nodes[0],"^");
					//������ �����ڴ� ����
					targetingModel.TgtRegion1   = targetingModel.TgtRegion1.Substring(0,targetingModel.TgtRegion1.Length-1);	
				}
				else
				{
					targetingModel.TgtRegion1Yn = "N";
					targetingModel.TgtRegion1   = "";
				}
				
				*/
                #endregion
                #region ���������� ����
                if (chkRegionYn.Checked)
                {
                    targetingModel.TgtRegion1Yn = "Y";
                    //targetingModel.TgtRegion1 = getTargetRegion();
                    if (tlvRegion.CheckedItems.Length > 0 && targetingModel.TgtRegion1Yn.Equals("Y"))
                    {
                        targetingModel.TgtRegion1 = getTargetRegion();
                    }
                    else
                    {
                        MessageBox.Show("���������밡 ���õ��� �ʾҽ��ϴ�.", "���������뼳�� ����", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                }
                else
                {
                    targetingModel.TgtRegion1Yn = "N";
                    targetingModel.TgtRegion1 = "";
                }
                #endregion

                #region ����ð��� ����
                if (chkTimeYn.Checked)
                {
                    targetingModel.TgtTimeYn = "Y";
                    targetingModel.TgtTime = strTimes;
                }
                else
                {
                    targetingModel.TgtTimeYn = "N";
                    targetingModel.TgtTime = "";
                }
                #endregion

                #region ���⿬�ɴ� ����
                if (chkAgeYn.Checked)
                {
                    targetingModel.TgtAgeYn = "Y";
                    targetingModel.TgtAge = GetAllNodeCheckedTag(tvAge.Nodes[0], "^");
                    //if (targetingModel.TgtAge.Length > 1)
                    //{
                    //    //������ �����ڴ� ����
                    //    targetingModel.TgtAge = targetingModel.TgtAge.Substring(0, targetingModel.TgtAge.Length - 1);
                    //}
                    if (targetingModel.TgtAge.Length > 1 && targetingModel.TgtAgeYn.Equals("Y"))
                    {
                        // ������ �����ڴ� ����
                        targetingModel.TgtAge = targetingModel.TgtAge.Substring(0, targetingModel.TgtAge.Length - 1);
                    }
                    else
                    {
                        MessageBox.Show("���⿬�ɴ밡 ���õ��� �ʾҽ��ϴ�.", "���⿬�ɴ뼳�� ����", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                }
                else
                {
                    targetingModel.TgtAgeYn = "N";
                    targetingModel.TgtAge = "";
                }
                #endregion

                #region ���⿬�ɱ��� ����
                if (chkAgeBtnYn.Checked)
                {
                    targetingModel.TgtAgeBtnYn = "Y";
                    targetingModel.TgtAgeBtnBegin = udAgeBtnBegin.Value.ToString();
                    targetingModel.TgtAgeBtnEnd = udAgeBtnEnd.Value.ToString();
                }
                else
                {
                    targetingModel.TgtAgeBtnYn = "N";
                    targetingModel.TgtAgeBtnBegin = "0";
                    targetingModel.TgtAgeBtnEnd = "0";
                }
                #endregion

                #region ���⼺�� ����
                if (chkSexYn.Checked)
                {
                    targetingModel.TgtSexYn = "Y";
                    if (chkSexMan.Checked)
                    {
                        targetingModel.TgtSexMan = "Y";
                    }
                    else
                    {
                        targetingModel.TgtSexMan = "N";
                    }
                    if (chkSexWoman.Checked)
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
                    targetingModel.TgtSexYn = "N";
                    if (chkSexMan.Checked) targetingModel.TgtSexMan = "N";
                    if (chkSexWoman.Checked) targetingModel.TgtSexWoman = "N";
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
                if (chkRateYn.Checked)
                {
                    if (rbRateMinus.Checked)
                    {
                        targetingModel.TgtRateYn = "-";
                    }
                    else
                    {
                        targetingModel.TgtRateYn = "Y";
                    }
                    if (rbRateAll.Checked) targetingModel.TgtRate = "0";
                    if (rbRate12.Checked) targetingModel.TgtRate = "12";
                    if (rbRate15.Checked) targetingModel.TgtRate = "15";
                    if (rbRate19.Checked) targetingModel.TgtRate = "19";
                }
                else
                {
                    targetingModel.TgtRateYn = "N";
                    targetingModel.TgtRate = "0";
                }
                #endregion

                #region ���Ϻ� ����
                if (chkWeekYn.Checked)
                {
                    targetingModel.TgtWeekYn = "Y";
                    if (chkSun.Checked)
                    {
                        keySun = "1^";
                    }
                    else
                    { keySun = ""; }
                    if (chkMon.Checked)
                    {
                        //'^'�� �� �ʵ忡 �������� üũ�ڽ������� �Է��ϱ� ������ ������ ���� ���..
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
                    targetingModel.TgtWeekYn = "N";
                    keySun = "";
                    keyMon = "";
                    keyThu = "";
                    keyWed = "";
                    keyThe = "";
                    keyFri = "";
                    keySat = "";
                }
                //üũ�ڽ��� ������ string���� �����Ͽ� Model�� ����..
                targetingModel.TgtWeek = keySun.ToString() + keyMon.ToString() + keyThu.ToString() + keyWed.ToString() + keyThe.ToString() + keyFri.ToString() + keySat.ToString();

                //Model�� ��� ���� Substring���� ������ ���ڸ��� �ڸ���..������ �ϳ��� ���ڿ� �پ��ִ� '^'�� �����ϱ� ����..
                //ex)DB�� ����� ������ ������ �׻�'^'�پ��ֱ� ������..
                if (targetingModel.TgtWeekYn.Equals("Y"))
                {
                    targetingModel.TgtWeek = targetingModel.TgtWeek.Substring(0, targetingModel.TgtWeek.Length - 1);
                }
                #endregion

                #region ��������ȣ ����
                if (chkZip.Checked)
                {
                    targetingModel.TgtZipYn = "Y";
                    targetingModel.TgtZip = ebZipCode.Text.Trim();
                }
                else
                {
                    targetingModel.TgtZipYn = "N";
                    targetingModel.TgtZip = "";
                }
                #endregion

                #region ����ä�� �������࿩�� ����
                /*
					if( chkPPx.Checked )	targetingModel.TgtPPxYn	= "Y";
					else					targetingModel.TgtPPxYn	= "N";
				 *	2014/04/03 ����
				 *	���� ȣȯ���� ���� Y/N�� �״�� �ΰ� [S]pecial�� ����Ѵ�.
				 *	N : �⺻��      ���� : ����ä�ο� ������������ ����( �⺻��å )
				 *	Y : �ɼǰ� ����+���� : ����ä�ο��� ���� ������
				 *	S : ������ ����      : ä�ο��� ���� ������
				 */

                if (chkPPx.Checked)
                {
                    targetingModel.TgtPPxYn = "Y";
                }
                else if (chkPPxOnly.Checked)
                {
                    targetingModel.TgtPPxYn = "S";
                }
                else
                {
                    targetingModel.TgtPPxYn = "N";
                }
                #endregion

                #region ���ν�ûDB ����
                if (chkPVS.Checked) targetingModel.TgtPVSYn = "Y";
                else targetingModel.TgtPVSYn = "N";

                #endregion

                #region ����� ����
                if (chkFreq.Checked)
                {
                    targetingModel.TgtFreqYn = "Y";
                    targetingModel.TgtFreqDay = freqDay.Value;
                    targetingModel.TgtFreqFeriod = freqPeriod.Value;
                }
                else
                {
                    targetingModel.TgtFreqYn = "N";
                    targetingModel.TgtFreqDay = 0;
                    targetingModel.TgtFreqFeriod = 0;
                }
                #endregion

                #region 2Slot ó��[E_05]
                if (chkSlotYn.Checked)
                {
                    if (rbSlotForward.Checked)
                        targetingModel.SlotExt = 3; //2slot ��
                    else if (rbSlotBackward.Checked)
                        targetingModel.SlotExt = 6; // 2slot ��
                }
                else
                {
                    targetingModel.SlotExt = 0; // 2slot ���� ������ ����
                }
                #endregion

                #region ��ž�� ����[E_08]

                if (chkStbModel.Checked)
                {
                    targetingModel.TgtStbModelYn = "Y";
                    targetingModel.TgtStbModel = GetAllNodeCheckedTag(tvStb.Nodes[0], "^");

                    //if (targetingModel.TgtStbModel.Length > 1)
                    //{
                    //    // ������ �����ڴ� ����
                    //    targetingModel.TgtStbModel = targetingModel.TgtStbModel.Substring(0, targetingModel.TgtStbModel.Length - 1);
                    //}
                    if (targetingModel.TgtStbModel.Length > 1 && targetingModel.TgtStbModelYn.Equals("Y"))
                    {
                        // ������ �����ڴ� ����
                        targetingModel.TgtStbModel = targetingModel.TgtStbModel.Substring(0, targetingModel.TgtStbModel.Length - 1);
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

                #region ��ȣ�������˾� [E_09]

                if (chkPreference.Checked)
                {
                    targetingModel.TgtPrefYn = "Y";
                    targetingModel.TgtPrefRate = Convert.ToInt32(udPrefRate.Value.ToString());
                    if (chkResponse.Checked)
                    {
                        targetingModel.TgtPrefNosend = "Y";
                    }
                    else
                    {
                        targetingModel.TgtPrefNosend = "N";
                    }
                }
                else
                {
                    targetingModel.TgtPrefYn = "N";
                    targetingModel.TgtPrefRate = 0;
                    targetingModel.TgtPrefNosend = "N";
                }

                #endregion

                #region �������� Ÿ���� [E_10]
                if (chkProfileYn.Checked)
                {
                    targetingModel.TgtProfileYn = "Y";
                }
                else
                {
                    targetingModel.TgtProfileYn = "N";
                    targetingModel.TgtProfile = "";
                    targetingModel.TgtReliablilty = 0;
                }

                #endregion

                // ���� Ÿ���� ����ȸ ���񽺸� ȣ���Ѵ�.
                // ������ �����͸� �о�´�
                new TargetingManager(systemModel, commonModel).GetTargetingDetail2(targetingModel);

                if (targetingModel.ResultCD.Equals("0000") && targetingModel.ResultCnt > 0)
                {
                    Utility.SetDataTable(targetingDs.TargetDetial, targetingModel.DetailDataSet);

                    DataRow Prerow = targetingDs.TargetDetial.Rows[0];

                    RegionPreRow = Prerow["TgtRegion1"].ToString();
                    TimePreRow = Prerow["TgtTime"].ToString();
                    WeekPreRow = Prerow["TgtWeek"].ToString();
                }

                // Ÿ���� ������ ���� ���񽺸� ȣ���Ѵ�.
                new TargetingManager(systemModel, commonModel).SetTargetingDetailUpdate(targetingModel);

                // ������ �����͸� �о�´�
                new TargetingManager(systemModel, commonModel).GetTargetingDetail2(targetingModel);

                // ������� �����׸��� ����� ��쿣 �ش� �����͵��� ��ũ�� �����
                if (targetingModel.ResultCD.Equals("0000") && targetingModel.ResultCnt > 0)
                {
                    Utility.SetDataTable(targetingDs.TargetDetial, targetingModel.DetailDataSet);
                    DataRow rowDetail = targetingDs.TargetDetial.Rows[0];
                    RegionNextRow = rowDetail["TgtRegion1"].ToString();
                    TimeNextRow = rowDetail["TgtTime"].ToString();
                    WeekNextRow = rowDetail["TgtWeek"].ToString();

                    #region �������� - �ӽ� ��� ����
                    //if (!RegionPreRow.Equals(RegionNextRow))
                    //{
                    //    string[] chkTgtRegionSplit = Utility.SplitByString(rowDetail["TgtRegion1"].ToString(), "^");
                    //    for (int j = 0; j < chkTgtRegionSplit.Length; j++)
                    //    {
                    //        //�������� ���°����� string������ ��´�..
                    //        string regionRate = chkTgtRegionSplit[j];
                    //        //string������ case������ �ش簪���� ���Ͽ�..�ش� üũ�ڽ��� ��Ʈ���Ѵ�.						
                    //        switch (regionRate)
                    //        {
                    //            case "01000":
                    //                if (udRegionRate1.Enabled.Equals(true))
                    //                {
                    //                    ebRegionTot.Text = "0";
                    //                    SetRegionRateResetAdd();
                    //                }
                    //                else
                    //                {
                    //                    ebRegionTot.Text = "0";
                    //                    SetRegionRateResetAdd();
                    //                }
                    //                break;
                    //            case "05000":
                    //                if (udRegionRate2.Enabled.Equals(true))
                    //                {
                    //                    ebRegionTot.Text = "0";
                    //                    SetRegionRateResetAdd();
                    //                }
                    //                else
                    //                {
                    //                    ebRegionTot.Text = "0";
                    //                    SetRegionRateResetAdd();
                    //                }
                    //                break;
                    //            case "06000":
                    //                if (udRegionRate3.Enabled.Equals(true))
                    //                {
                    //                    ebRegionTot.Text = "0";
                    //                    SetRegionRateResetAdd();
                    //                }
                    //                else
                    //                {
                    //                    ebRegionTot.Text = "0";
                    //                    SetRegionRateResetAdd();
                    //                }
                    //                break;
                    //            case "07000":
                    //                if (udRegionRate4.Enabled.Equals(true))
                    //                {
                    //                    ebRegionTot.Text = "0";
                    //                    SetRegionRateResetAdd();
                    //                }
                    //                else
                    //                {
                    //                    ebRegionTot.Text = "0";
                    //                    SetRegionRateResetAdd();
                    //                }
                    //                break;
                    //            case "08000":
                    //                if (udRegionRate5.Enabled.Equals(true))
                    //                {
                    //                    ebRegionTot.Text = "0";
                    //                    SetRegionRateResetAdd();
                    //                }
                    //                else
                    //                {
                    //                    ebRegionTot.Text = "0";
                    //                    SetRegionRateResetAdd();
                    //                }
                    //                break;
                    //            case "09000":
                    //                if (udRegionRate6.Enabled.Equals(true))
                    //                {
                    //                    ebRegionTot.Text = "0";
                    //                    SetRegionRateResetAdd();
                    //                }
                    //                else
                    //                {
                    //                    ebRegionTot.Text = "0";
                    //                    SetRegionRateResetAdd();
                    //                }
                    //                break;
                    //            case "10000":
                    //                if (udRegionRate7.Enabled.Equals(true))
                    //                {
                    //                    ebRegionTot.Text = "0";
                    //                    SetRegionRateResetAdd();
                    //                }
                    //                else
                    //                {
                    //                    ebRegionTot.Text = "0";
                    //                    SetRegionRateResetAdd();
                    //                }
                    //                break;
                    //            case "11000":
                    //                if (udRegionRate8.Enabled.Equals(true))
                    //                {
                    //                    ebRegionTot.Text = "0";
                    //                    SetRegionRateResetAdd();
                    //                }
                    //                else
                    //                {
                    //                    ebRegionTot.Text = "0";
                    //                    SetRegionRateResetAdd();
                    //                }
                    //                break;
                    //            case "12000":
                    //                if (udRegionRate9.Enabled.Equals(true))
                    //                {
                    //                    ebRegionTot.Text = "0";
                    //                    SetRegionRateResetAdd();
                    //                }
                    //                else
                    //                {
                    //                    ebRegionTot.Text = "0";
                    //                    SetRegionRateResetAdd();
                    //                }
                    //                break;
                    //            case "13000":
                    //                if (udRegionRate10.Enabled.Equals(true))
                    //                {
                    //                    ebRegionTot.Text = "0";
                    //                    SetRegionRateResetAdd();
                    //                }
                    //                else
                    //                {
                    //                    ebRegionTot.Text = "0";
                    //                    SetRegionRateResetAdd();
                    //                }
                    //                break;
                    //            case "14000":
                    //                if (udRegionRate11.Enabled.Equals(true))
                    //                {
                    //                    ebRegionTot.Text = "0";
                    //                    SetRegionRateResetAdd();
                    //                }
                    //                else
                    //                {
                    //                    ebRegionTot.Text = "0";
                    //                    SetRegionRateResetAdd();
                    //                }
                    //                break;
                    //            case "15000":
                    //                if (udRegionRate12.Enabled.Equals(true))
                    //                {
                    //                    ebRegionTot.Text = "0";
                    //                    SetRegionRateResetAdd();
                    //                }
                    //                else
                    //                {
                    //                    ebRegionTot.Text = "0";
                    //                    SetRegionRateResetAdd();
                    //                }
                    //                break;
                    //            case "16000":
                    //                if (udRegionRate13.Enabled.Equals(true))
                    //                {
                    //                    ebRegionTot.Text = "0";
                    //                    SetRegionRateResetAdd();
                    //                }
                    //                else
                    //                {
                    //                    ebRegionTot.Text = "0";
                    //                    SetRegionRateResetAdd();
                    //                }
                    //                break;
                    //            case "17000":
                    //                if (udRegionRate14.Enabled.Equals(true))
                    //                {
                    //                    ebRegionTot.Text = "0";
                    //                    SetRegionRateResetAdd();
                    //                }
                    //                else
                    //                {
                    //                    ebRegionTot.Text = "0";
                    //                    SetRegionRateResetAdd();
                    //                }
                    //                break;
                    //            case "18000":
                    //                if (udRegionRate15.Enabled.Equals(true))
                    //                {
                    //                    ebRegionTot.Text = "0";
                    //                    SetRegionRateResetAdd();
                    //                }
                    //                else
                    //                {
                    //                    ebRegionTot.Text = "0";
                    //                    SetRegionRateResetAdd();
                    //                }
                    //                break;
                    //            case "19000":
                    //                if (udRegionRate16.Enabled.Equals(true))
                    //                {
                    //                    ebRegionTot.Text = "0";
                    //                    SetRegionRateResetAdd();
                    //                }
                    //                else
                    //                {
                    //                    ebRegionTot.Text = "0";
                    //                    SetRegionRateResetAdd();
                    //                }
                    //                break;
                    //            case "90000":
                    //                if (udRegionRate17.Enabled.Equals(true))
                    //                {
                    //                    ebRegionTot.Text = "0";
                    //                    SetRegionRateResetAdd();
                    //                }
                    //                else
                    //                {
                    //                    ebRegionTot.Text = "0";
                    //                    SetRegionRateResetAdd();
                    //                }
                    //                break;
                    //        }
                    //    }
                    //}
                    #endregion

                    #region Time ���� - �ӽ� ��� ����
                    //if (!TimePreRow.Equals(TimeNextRow))
                    //{

                    //    string[] chkTgtTimeSplit = Utility.SplitByString(rowDetail["TgtTime"].ToString(), "^");
                    //    for (int j = 0; j < chkTgtTimeSplit.Length; j++)
                    //    {
                    //        //�������� ���°����� string������ ��´�..
                    //        string timeRate = chkTgtTimeSplit[j];
                    //        //string������ case������ �ش簪���� ���Ͽ�..�ش� üũ�ڽ��� ��Ʈ���Ѵ�.
                    //        switch (timeRate)
                    //        {
                    //            case "00":
                    //                if (udTimeRate0.Enabled.Equals(true))
                    //                {
                    //                    ebTimeTot.Text = "0";
                    //                    SetTimeRateResetAdd();
                    //                }
                    //                else
                    //                {
                    //                    ebTimeTot.Text = "0";
                    //                    SetTimeRateResetAdd();
                    //                }
                    //                break;
                    //            case "01":
                    //                if (udTimeRate1.Enabled.Equals(true))
                    //                {
                    //                    ebTimeTot.Text = "0";
                    //                    SetTimeRateResetAdd();
                    //                }
                    //                else
                    //                {
                    //                    ebTimeTot.Text = "0";
                    //                    SetTimeRateResetAdd();
                    //                }
                    //                break;
                    //            case "02":
                    //                if (udTimeRate2.Enabled.Equals(true))
                    //                {
                    //                    ebTimeTot.Text = "0";
                    //                    SetTimeRateResetAdd();
                    //                }
                    //                else
                    //                {
                    //                    ebTimeTot.Text = "0";
                    //                    SetTimeRateResetAdd();
                    //                }
                    //                break;
                    //            case "03":
                    //                if (udTimeRate3.Enabled.Equals(true))
                    //                {
                    //                    ebTimeTot.Text = "0";
                    //                    SetTimeRateResetAdd();
                    //                }
                    //                else
                    //                {
                    //                    ebTimeTot.Text = "0";
                    //                    SetTimeRateResetAdd();
                    //                }
                    //                break;
                    //            case "04":
                    //                if (udTimeRate4.Enabled.Equals(true))
                    //                {
                    //                    ebTimeTot.Text = "0";
                    //                    SetTimeRateResetAdd();
                    //                }
                    //                else
                    //                {
                    //                    ebTimeTot.Text = "0";
                    //                    SetTimeRateResetAdd();
                    //                }
                    //                break;
                    //            case "05":
                    //                if (udTimeRate5.Enabled.Equals(true))
                    //                {
                    //                    ebTimeTot.Text = "0";
                    //                    SetTimeRateResetAdd();
                    //                }
                    //                else
                    //                {
                    //                    ebTimeTot.Text = "0";
                    //                    SetTimeRateResetAdd();
                    //                }
                    //                break;
                    //            case "06":
                    //                if (udTimeRate6.Enabled.Equals(true))
                    //                {
                    //                    ebTimeTot.Text = "0";
                    //                    SetTimeRateResetAdd();
                    //                }
                    //                else
                    //                {
                    //                    ebTimeTot.Text = "0";
                    //                    SetTimeRateResetAdd();
                    //                }
                    //                break;
                    //            case "07":
                    //                if (udTimeRate7.Enabled.Equals(true))
                    //                {
                    //                    ebTimeTot.Text = "0";
                    //                    SetTimeRateResetAdd();
                    //                }
                    //                else
                    //                {
                    //                    ebTimeTot.Text = "0";
                    //                    SetTimeRateResetAdd();
                    //                }
                    //                break;
                    //            case "08":
                    //                if (udTimeRate8.Enabled.Equals(true))
                    //                {
                    //                    ebTimeTot.Text = "0";
                    //                    SetTimeRateResetAdd();
                    //                }
                    //                else
                    //                {
                    //                    ebTimeTot.Text = "0";
                    //                    SetTimeRateResetAdd();
                    //                }
                    //                break;
                    //            case "09":
                    //                if (udTimeRate9.Enabled.Equals(true))
                    //                {
                    //                    ebTimeTot.Text = "0";
                    //                    SetTimeRateResetAdd();
                    //                }
                    //                else
                    //                {
                    //                    ebTimeTot.Text = "0";
                    //                    SetTimeRateResetAdd();
                    //                }
                    //                break;
                    //            case "10":
                    //                if (udTimeRate10.Enabled.Equals(true))
                    //                {
                    //                    ebTimeTot.Text = "0";
                    //                    SetTimeRateResetAdd();
                    //                }
                    //                else
                    //                {
                    //                    ebTimeTot.Text = "0";
                    //                    SetTimeRateResetAdd();
                    //                }
                    //                break;
                    //            case "11":
                    //                if (udTimeRate11.Enabled.Equals(true))
                    //                {
                    //                    ebTimeTot.Text = "0";
                    //                    SetTimeRateResetAdd();
                    //                }
                    //                else
                    //                {
                    //                    ebTimeTot.Text = "0";
                    //                    SetTimeRateResetAdd();
                    //                }
                    //                break;
                    //            case "12":
                    //                if (udTimeRate12.Enabled.Equals(true))
                    //                {
                    //                    ebTimeTot.Text = "0";
                    //                    SetTimeRateResetAdd();
                    //                }
                    //                else
                    //                {
                    //                    ebTimeTot.Text = "0";
                    //                    SetTimeRateResetAdd();
                    //                }
                    //                break;
                    //            case "13":
                    //                if (udTimeRate13.Enabled.Equals(true))
                    //                {
                    //                    ebTimeTot.Text = "0";
                    //                    SetTimeRateResetAdd();
                    //                }
                    //                else
                    //                {
                    //                    ebTimeTot.Text = "0";
                    //                    SetTimeRateResetAdd();
                    //                }
                    //                break;
                    //            case "14":
                    //                if (udTimeRate14.Enabled.Equals(true))
                    //                {
                    //                    ebTimeTot.Text = "0";
                    //                    SetTimeRateResetAdd();
                    //                }
                    //                else
                    //                {
                    //                    ebTimeTot.Text = "0";
                    //                    SetTimeRateResetAdd();
                    //                }
                    //                break;
                    //            case "15":
                    //                if (udTimeRate15.Enabled.Equals(true))
                    //                {
                    //                    ebTimeTot.Text = "0";
                    //                    SetTimeRateResetAdd();
                    //                }
                    //                else
                    //                {
                    //                    ebTimeTot.Text = "0";
                    //                    SetTimeRateResetAdd();
                    //                }
                    //                break;
                    //            case "16":
                    //                if (udTimeRate16.Enabled.Equals(true))
                    //                {
                    //                    ebTimeTot.Text = "0";
                    //                    SetTimeRateResetAdd();
                    //                }
                    //                else
                    //                {
                    //                    ebTimeTot.Text = "0";
                    //                    SetTimeRateResetAdd();
                    //                }
                    //                break;
                    //            case "17":
                    //                if (udTimeRate17.Enabled.Equals(true))
                    //                {
                    //                    ebTimeTot.Text = "0";
                    //                    SetTimeRateResetAdd();
                    //                }
                    //                else
                    //                {
                    //                    ebTimeTot.Text = "0";
                    //                    SetTimeRateResetAdd();
                    //                }
                    //                break;
                    //            case "18":
                    //                if (udTimeRate18.Enabled.Equals(true))
                    //                {
                    //                    ebTimeTot.Text = "0";
                    //                    SetTimeRateResetAdd();
                    //                }
                    //                else
                    //                {
                    //                    ebTimeTot.Text = "0";
                    //                    SetTimeRateResetAdd();
                    //                }
                    //                break;
                    //            case "19":
                    //                if (udTimeRate19.Enabled.Equals(true))
                    //                {
                    //                    ebTimeTot.Text = "0";
                    //                    SetTimeRateResetAdd();
                    //                }
                    //                else
                    //                {
                    //                    ebTimeTot.Text = "0";
                    //                    SetTimeRateResetAdd();
                    //                }
                    //                break;
                    //            case "20":
                    //                if (udTimeRate20.Enabled.Equals(true))
                    //                {
                    //                    ebTimeTot.Text = "0";
                    //                    SetTimeRateResetAdd();
                    //                }
                    //                else
                    //                {
                    //                    ebTimeTot.Text = "0";
                    //                    SetTimeRateResetAdd();
                    //                }
                    //                break;
                    //            case "21":
                    //                if (udTimeRate21.Enabled.Equals(true))
                    //                {
                    //                    ebTimeTot.Text = "0";
                    //                    SetTimeRateResetAdd();
                    //                }
                    //                else
                    //                {
                    //                    ebTimeTot.Text = "0";
                    //                    SetTimeRateResetAdd();
                    //                }
                    //                break;
                    //            case "22":
                    //                if (udTimeRate22.Enabled.Equals(true))
                    //                {
                    //                    ebTimeTot.Text = "0";
                    //                    SetTimeRateResetAdd();
                    //                }
                    //                else
                    //                {
                    //                    ebTimeTot.Text = "0";
                    //                    SetTimeRateResetAdd();
                    //                }
                    //                break;
                    //            case "23":
                    //                if (udTimeRate23.Enabled.Equals(true))
                    //                {
                    //                    ebTimeTot.Text = "0";
                    //                    SetTimeRateResetAdd();
                    //                }
                    //                else
                    //                {
                    //                    ebTimeTot.Text = "0";
                    //                    SetTimeRateResetAdd();
                    //                }
                    //                break;
                    //        }
                    //    }
                    //}
                    #endregion

                    #region �ָ����� - �ӽ� ��� ����
                    //if (!WeekPreRow.Equals(WeekNextRow))
                    //{

                    //    string[] chkTgtWeekSplit = Utility.SplitByString(rowDetail["TgtWeek"].ToString(), "^");
                    //    for (int j = 0; j < chkTgtWeekSplit.Length; j++)
                    //    {
                    //        //�������� ���°����� string������ ��´�..
                    //        string weekRate = chkTgtWeekSplit[j];
                    //        //string������ case������ �ش簪���� ���Ͽ�..�ش� üũ�ڽ��� ��Ʈ���Ѵ�.
                    //        switch (weekRate)
                    //        {
                    //            case "2":
                    //                if (udMonRate.Enabled.Equals(true))
                    //                {
                    //                    ebWeekTot.Text = "0";
                    //                    SetWeekRateResetAdd();
                    //                }
                    //                else
                    //                {
                    //                    ebWeekTot.Text = "0";
                    //                    SetWeekRateResetAdd();
                    //                }
                    //                break;
                    //            case "3":
                    //                if (udThuRate.Enabled.Equals(true))
                    //                {
                    //                    ebWeekTot.Text = "0";
                    //                    SetWeekRateResetAdd();
                    //                }
                    //                else
                    //                {
                    //                    ebWeekTot.Text = "0";
                    //                    SetWeekRateResetAdd();
                    //                }
                    //                break;
                    //            case "4":
                    //                if (udWedRate.Enabled.Equals(true))
                    //                {
                    //                    ebWeekTot.Text = "0";
                    //                    SetWeekRateResetAdd();
                    //                }
                    //                else
                    //                {
                    //                    ebWeekTot.Text = "0";
                    //                    SetWeekRateResetAdd();
                    //                }
                    //                break;
                    //            case "5":
                    //                if (udTheRate.Enabled.Equals(true))
                    //                {
                    //                    ebWeekTot.Text = "0";
                    //                    SetWeekRateResetAdd();
                    //                }
                    //                else
                    //                {
                    //                    ebWeekTot.Text = "0";
                    //                    SetWeekRateResetAdd();
                    //                }
                    //                break;
                    //            case "6":
                    //                if (udFriRate.Enabled.Equals(true))
                    //                {
                    //                    ebWeekTot.Text = "0";
                    //                    SetWeekRateResetAdd();
                    //                }
                    //                else
                    //                {
                    //                    ebWeekTot.Text = "0";
                    //                    SetWeekRateResetAdd();
                    //                }
                    //                break;
                    //            case "7":
                    //                if (udSatRate.Enabled.Equals(true))
                    //                {
                    //                    ebWeekTot.Text = "0";
                    //                    SetWeekRateResetAdd();
                    //                }
                    //                else
                    //                {
                    //                    ebWeekTot.Text = "0";
                    //                    SetWeekRateResetAdd();
                    //                }
                    //                break;
                    //            case "1":
                    //                if (udSunRate.Enabled.Equals(true))
                    //                {
                    //                    ebWeekTot.Text = "0";
                    //                    SetWeekRateResetAdd();
                    //                }
                    //                else
                    //                {
                    //                    ebWeekTot.Text = "0";
                    //                    SetWeekRateResetAdd();
                    //                }
                    //                break;
                    //        }
                    //    }
                    //}
                    #endregion

                }

                if (targetingModel.ResultCD.Equals("0000"))
                {
                    StatusMessage("Ÿ���� ������ ����Ǿ����ϴ�.");

                    DisableButton();
                    ResetDetailText();
                    SearchTargeting();
                    DetailReadOnly(true);
                }
            }
            catch (FrameException fe)
            {
                FrameSystem.showMsgForm("Ÿ���ó��� �������", new string[] { fe.ErrCode, fe.ResultMsg });
            }
            catch (Exception ex)
            {
                FrameSystem.showMsgForm("Ÿ���ó��� �������", new string[] { "", ex.Message });
            }
        }

        #endregion

        #region reset
        private void SetRegionRateResetAdd()
        {
#if (DEBUG)
            Debug.WriteLine(MethodBase.GetCurrentMethod().Name);
#endif
            targetingModel.Type = "01";
            targetingModel.Rate1 = "0";
            targetingModel.Rate2 = "0";
            targetingModel.Rate3 = "0";
            targetingModel.Rate4 = "0";
            targetingModel.Rate5 = "0";
            targetingModel.Rate6 = "0";
            targetingModel.Rate7 = "0";
            targetingModel.Rate8 = "0";
            targetingModel.Rate9 = "0";
            targetingModel.Rate10 = "0";
            targetingModel.Rate11 = "0";
            targetingModel.Rate12 = "0";
            targetingModel.Rate13 = "0";
            targetingModel.Rate14 = "0";
            targetingModel.Rate15 = "0";
            targetingModel.Rate16 = "0";
            targetingModel.Rate17 = "0";
            targetingModel.Rate18 = "0";
            targetingModel.Rate19 = "0";
            targetingModel.Rate20 = "0";
            targetingModel.Rate21 = "0";
            targetingModel.Rate22 = "0";
            targetingModel.Rate23 = "0";
            targetingModel.Rate24 = "0";

            // Ÿ���� ������ ���� ���񽺸� ȣ���Ѵ�.						
            new TargetingManager(systemModel, commonModel).SetTargetingRateUpdate(targetingModel);
        }

        private void SetTimeRateResetAdd()
        {
#if (DEBUG)
            Debug.WriteLine(MethodBase.GetCurrentMethod().Name);
#endif
            targetingModel.Type = "02";
            targetingModel.Rate1 = "0";
            targetingModel.Rate2 = "0";
            targetingModel.Rate3 = "0";
            targetingModel.Rate4 = "0";
            targetingModel.Rate5 = "0";
            targetingModel.Rate6 = "0";
            targetingModel.Rate7 = "0";
            targetingModel.Rate8 = "0";
            targetingModel.Rate9 = "0";
            targetingModel.Rate10 = "0";
            targetingModel.Rate11 = "0";
            targetingModel.Rate12 = "0";
            targetingModel.Rate13 = "0";
            targetingModel.Rate14 = "0";
            targetingModel.Rate15 = "0";
            targetingModel.Rate16 = "0";
            targetingModel.Rate17 = "0";
            targetingModel.Rate18 = "0";
            targetingModel.Rate19 = "0";
            targetingModel.Rate20 = "0";
            targetingModel.Rate21 = "0";
            targetingModel.Rate22 = "0";
            targetingModel.Rate23 = "0";
            targetingModel.Rate24 = "0";

            // Ÿ���� ������ ���� ���񽺸� ȣ���Ѵ�.						
            new TargetingManager(systemModel, commonModel).SetTargetingRateUpdate(targetingModel);
        }

        private void SetWeekRateResetAdd()
        {
#if (DEBUG)
            Debug.WriteLine(MethodBase.GetCurrentMethod().Name);
#endif
            targetingModel.Type = "03";
            targetingModel.Rate1 = "0";
            targetingModel.Rate2 = "0";
            targetingModel.Rate3 = "0";
            targetingModel.Rate4 = "0";
            targetingModel.Rate5 = "0";
            targetingModel.Rate6 = "0";
            targetingModel.Rate7 = "0";
            targetingModel.Rate8 = "0";
            targetingModel.Rate9 = "0";
            targetingModel.Rate10 = "0";
            targetingModel.Rate11 = "0";
            targetingModel.Rate12 = "0";
            targetingModel.Rate13 = "0";
            targetingModel.Rate14 = "0";
            targetingModel.Rate15 = "0";
            targetingModel.Rate16 = "0";
            targetingModel.Rate17 = "0";
            targetingModel.Rate18 = "0";
            targetingModel.Rate19 = "0";
            targetingModel.Rate20 = "0";
            targetingModel.Rate21 = "0";
            targetingModel.Rate22 = "0";
            targetingModel.Rate23 = "0";
            targetingModel.Rate24 = "0";

            // Ÿ���� ������ ���� ���񽺸� ȣ���Ѵ�.						
            new TargetingManager(systemModel, commonModel).SetTargetingRateUpdate(targetingModel);
        }
        #endregion

        #region Ÿ�� ����
        /// <summary>
        /// ���� Ÿ���� ����
        /// </summary>
        private void SetTargetingRateAdd()
        {
#if (DEBUG)
            Debug.WriteLine(MethodBase.GetCurrentMethod().Name);
#endif
            StatusMessage("Ÿ���� ������ �����մϴ�.");

            try
            {
                //���� ���� ���� �ʱ�ȭ ���ش�.
                targetingModel.Init();

                int curRow = cm.Position;

                // �����͸𵨿� ������ ������ ��Ʈ�Ѵ�.
                //�����ȣ
                targetingModel.ItemNo = keyItemNo;

                #region [�������� ó���κ�]
                if (chkRegionYn.Checked)
                {
                    if (tot1 < 100)
                    {
                        // �հ谡 100�̸��϶��� �ش� ���� ��ȿ�� ���� �������� �����Ѵ�
                        // ��ü �׸��� 0�̸�, ������ ������� �ʰ�, �⺻����Ѵ�
                        if (udRegionRate1.Value
                            + udRegionRate2.Value
                            + udRegionRate3.Value
                            + udRegionRate4.Value
                            + udRegionRate5.Value
                            + udRegionRate6.Value
                            + udRegionRate7.Value
                            + udRegionRate8.Value
                            + udRegionRate9.Value
                            + udRegionRate10.Value
                            + udRegionRate11.Value
                            + udRegionRate12.Value
                            + udRegionRate13.Value
                            + udRegionRate14.Value
                            + udRegionRate15.Value
                            + udRegionRate16.Value
                            + udRegionRate17.Value > 0)
                        {
                            MessageBox.Show("�������������� 100% �̸��Դϴ�. \n Ȯ���Ͽ��ּ���!!", "Ÿ���ú��� ����", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return;
                        }
                    }
                    else if (tot1 > 100)
                    {
                        MessageBox.Show("�������������� 100%�� �ʰ��Ͽ����ϴ� \n Ȯ���Ͽ��ּ���!!", "Ÿ���ú��� ����", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }

                    targetingModel.Type = "01";
                    targetingModel.Rate1 = udRegionRate1.Value.ToString();
                    targetingModel.Rate2 = udRegionRate2.Value.ToString();
                    targetingModel.Rate3 = udRegionRate3.Value.ToString();
                    targetingModel.Rate4 = udRegionRate4.Value.ToString();
                    targetingModel.Rate5 = udRegionRate5.Value.ToString();
                    targetingModel.Rate6 = udRegionRate6.Value.ToString();
                    targetingModel.Rate7 = udRegionRate7.Value.ToString();
                    targetingModel.Rate8 = udRegionRate8.Value.ToString();
                    targetingModel.Rate9 = udRegionRate9.Value.ToString();
                    targetingModel.Rate10 = udRegionRate10.Value.ToString();
                    targetingModel.Rate11 = udRegionRate11.Value.ToString();
                    targetingModel.Rate12 = udRegionRate12.Value.ToString();
                    targetingModel.Rate13 = udRegionRate13.Value.ToString();
                    targetingModel.Rate14 = udRegionRate14.Value.ToString();
                    targetingModel.Rate15 = udRegionRate15.Value.ToString();
                    targetingModel.Rate16 = udRegionRate16.Value.ToString();
                    targetingModel.Rate17 = udRegionRate17.Value.ToString();
                    targetingModel.Rate18 = "0";
                    targetingModel.Rate19 = "0";
                    targetingModel.Rate20 = "0";
                    targetingModel.Rate21 = "0";
                    targetingModel.Rate22 = "0";
                    targetingModel.Rate23 = "0";
                    targetingModel.Rate24 = "0";

                    // Ÿ���� ������ ���� ���񽺸� ȣ���Ѵ�.
                    new TargetingManager(systemModel, commonModel).SetTargetingRateUpdate(targetingModel);

                }
                #endregion

                #region [�ð��뺰 ó���κ�]
                if (chkTimeYn.Checked)
                {
                    if (tot2 < 100)
                    {
                        if (udTimeRate0.Value + udTimeRate1.Value + udTimeRate2.Value +
                            udTimeRate3.Value + udTimeRate4.Value + udTimeRate5.Value +
                            udTimeRate6.Value + udTimeRate7.Value + udTimeRate8.Value +
                            udTimeRate9.Value + udTimeRate10.Value + udTimeRate11.Value +
                            udTimeRate12.Value + udTimeRate13.Value + udTimeRate14.Value +
                            udTimeRate15.Value + udTimeRate16.Value + udTimeRate17.Value +
                            udTimeRate18.Value + udTimeRate19.Value + udTimeRate20.Value +
                            udTimeRate21.Value + udTimeRate22.Value + udTimeRate23.Value > 0)
                        {
                            MessageBox.Show("����ð������� 100% �̸��Դϴ�. \n Ȯ���Ͽ��ּ���!!", "Ÿ���ú��� ����", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return;
                        }
                    }
                    else if (tot2 > 100)
                    {
                        MessageBox.Show("����ð������� 100%�� �ʰ��Ͽ����ϴ� \n Ȯ���Ͽ��ּ���!!", "Ÿ���ú��� ����", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }

                    targetingModel.Type = "02";
                    targetingModel.Rate1 = udTimeRate0.Value.ToString();
                    targetingModel.Rate2 = udTimeRate1.Value.ToString();
                    targetingModel.Rate3 = udTimeRate2.Value.ToString();
                    targetingModel.Rate4 = udTimeRate3.Value.ToString();
                    targetingModel.Rate5 = udTimeRate4.Value.ToString();
                    targetingModel.Rate6 = udTimeRate5.Value.ToString();
                    targetingModel.Rate7 = udTimeRate6.Value.ToString();
                    targetingModel.Rate8 = udTimeRate7.Value.ToString();
                    targetingModel.Rate9 = udTimeRate8.Value.ToString();
                    targetingModel.Rate10 = udTimeRate9.Value.ToString();
                    targetingModel.Rate11 = udTimeRate10.Value.ToString();
                    targetingModel.Rate12 = udTimeRate11.Value.ToString();
                    targetingModel.Rate13 = udTimeRate12.Value.ToString();
                    targetingModel.Rate14 = udTimeRate13.Value.ToString();
                    targetingModel.Rate15 = udTimeRate14.Value.ToString();
                    targetingModel.Rate16 = udTimeRate15.Value.ToString();
                    targetingModel.Rate17 = udTimeRate16.Value.ToString();
                    targetingModel.Rate18 = udTimeRate17.Value.ToString();
                    targetingModel.Rate19 = udTimeRate18.Value.ToString();
                    targetingModel.Rate20 = udTimeRate19.Value.ToString();
                    targetingModel.Rate21 = udTimeRate20.Value.ToString();
                    targetingModel.Rate22 = udTimeRate21.Value.ToString();
                    targetingModel.Rate23 = udTimeRate22.Value.ToString();
                    targetingModel.Rate24 = udTimeRate23.Value.ToString();

                    // Ÿ���� ������ ���� ���񽺸� ȣ���Ѵ�.
                    new TargetingManager(systemModel, commonModel).SetTargetingRateUpdate(targetingModel);
                }
                #endregion

                #region [���ϴ뺰 ó���κ�]
                if (chkWeekYn.Checked)
                {
                    if (tot3 < 100)
                    {
                        if (udSunRate.Value + udMonRate.Value +
                            udThuRate.Value + udWedRate.Value +
                            udTheRate.Value + udFriRate.Value + udSatRate.Value > 0)
                        {
                            MessageBox.Show("������Ϻ����� 100% �̸��Դϴ�. \n Ȯ���Ͽ��ּ���!!", "Ÿ���ú��� ����", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return;
                        }
                    }
                    else if (tot3 > 100)
                    {
                        MessageBox.Show("������Ϻ����� 100%�� �ʰ��Ͽ����ϴ� \n Ȯ���Ͽ��ּ���!!", "Ÿ���ú��� ����", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }

                    targetingModel.Type = "03";
                    targetingModel.Rate1 = udSunRate.Value.ToString();
                    targetingModel.Rate2 = udMonRate.Value.ToString();
                    targetingModel.Rate3 = udThuRate.Value.ToString();
                    targetingModel.Rate4 = udWedRate.Value.ToString();
                    targetingModel.Rate5 = udTheRate.Value.ToString();
                    targetingModel.Rate6 = udFriRate.Value.ToString();
                    targetingModel.Rate7 = udSatRate.Value.ToString();
                    targetingModel.Rate8 = "0";
                    targetingModel.Rate9 = "0";
                    targetingModel.Rate10 = "0";
                    targetingModel.Rate11 = "0";
                    targetingModel.Rate12 = "0";
                    targetingModel.Rate13 = "0";
                    targetingModel.Rate14 = "0";
                    targetingModel.Rate15 = "0";
                    targetingModel.Rate16 = "0";
                    targetingModel.Rate17 = "0";
                    targetingModel.Rate18 = "0";
                    targetingModel.Rate19 = "0";
                    targetingModel.Rate20 = "0";
                    targetingModel.Rate21 = "0";
                    targetingModel.Rate22 = "0";
                    targetingModel.Rate23 = "0";
                    targetingModel.Rate24 = "0";

                    // Ÿ���� ������ ���� ���񽺸� ȣ���Ѵ�.						
                    new TargetingManager(systemModel, commonModel).SetTargetingRateUpdate(targetingModel);
                }
                #endregion

                if (targetingModel.ResultCD.Equals("0000"))
                {

                    StatusMessage("Ÿ���� ������ ����Ǿ����ϴ�.");

                    DisableButton();
                    ResetRateText();
                    SetRateText();
                    GetTargetingData();
                }
            }
            catch (FrameException fe)
            {
                FrameSystem.showMsgForm("Ÿ���ó��� �������", new string[] { fe.ErrCode, fe.ResultMsg });
            }
            catch (Exception ex)
            {
                FrameSystem.showMsgForm("Ÿ���ó��� �������", new string[] { "", ex.Message });
            }
        }

        /// <summary>
        /// �Է��׸���� ��Ȱ��ȭ ��Ų��.
        /// </summary>
        /// <param name="readMode"></param>
        private void DetailReadOnly(bool readMode)
        {
            if (readMode)
            {
                ebContractAmt.ReadOnly = true;
                ebContractTot.ReadOnly = true;
                udControlRate.ReadOnly = true;
                chkRegionYn.Enabled = false;
                chkTimeYn.Enabled = false;
                chkWeekYn.Enabled = false;
                chkSlotYn.Enabled = false;
                chkAgeYn.Enabled = false;
                chkAgeBtnYn.Enabled = false;
                chkSexYn.Enabled = false;
                chkRateYn.Enabled = false;
                chkPPx.Enabled = false;
                chkPPxOnly.Enabled = false;
                chkPVS.Enabled = false;
                chkZip.Enabled = false;
                chkFreq.Enabled = false;
                chkCollectionYn.Enabled = false;
                chkStbModel.Enabled = false;    // [E_08] ��ž��
                chkPreference.Enabled = false;  // [E_09] ��ȣ�������˾�
                chkProfileYn.Enabled = false;   // [E_10] �������� Ÿ���� 

                tlvRegion.Enabled = false;
                gbTime.Enabled = false;
                grpSlot.Enabled = false;
                gbDays.Enabled = false;
                grpAge.Enabled = false;
                grpSex.Enabled = false;
                grpRate.Enabled = false;
                grpRateType.Enabled = false;
                grpStb.Enabled = false; // [E_08] ��ž��
                gbPost.Enabled = false;
                grpFreq.Enabled = false;
                rbControlYn_Y.Enabled = false;
                rbControlYn_N.Enabled = false;
                grpPreference.Enabled = false; // [E_09] ��ȣ�������˾�
            }
            else
            {
                ebContractAmt.ReadOnly = false;
                ebContractTot.ReadOnly = false;
                udControlRate.ReadOnly = false;
                chkRegionYn.Enabled = true;
                chkTimeYn.Enabled = true;
                chkWeekYn.Enabled = true;
                chkSlotYn.Enabled = true;
                chkAgeYn.Enabled = true;
                chkAgeBtnYn.Enabled = true;
                chkSexYn.Enabled = true;
                chkRateYn.Enabled = true;
                chkPPx.Enabled = true;
                chkPPxOnly.Enabled = true;
                chkPVS.Enabled = true;
                chkZip.Enabled = true;
                chkFreq.Enabled = true;
                chkCollectionYn.Enabled = true;
                chkStbModel.Enabled = true;    // [E_08] ��ž��
                chkPreference.Enabled = true;  // [E_09] ��ȣ�������˾�
                chkProfileYn.Enabled = true;   // [E_10] �������� Ÿ���� 

                if (chkRegionYn.Checked) tlvRegion.Enabled = true;
                if (chkTimeYn.Checked) gbTime.Enabled = true;
                if (chkSlotYn.Checked) grpSlot.Enabled = true;
                if (chkWeekYn.Checked) gbDays.Enabled = true;
                if (chkAgeYn.Checked) grpAge.Enabled = true;
                if (chkSexYn.Checked) grpSex.Enabled = true;
                if (chkRateYn.Checked) grpRate.Enabled = true;
                if (chkRateYn.Checked) grpRateType.Enabled = true;
                if (chkStbModel.Checked) grpStb.Enabled = true;
                if (chkZip.Checked) gbPost.Enabled = true;
                if (chkFreq.Checked) grpFreq.Enabled = true;
                if (chkPreference.Checked) grpPreference.Enabled = true; // [E_09] ��ȣ�������˾�
                rbControlYn_Y.Enabled = true;
                rbControlYn_N.Enabled = true;
            }

            // �ٸ� ���� ���ý� Ÿ���� ���� �ٷ� ���� �ֵ��� ����
            //if (!uiTabPage1.Selected)
            //{
            //    uiTabPage1.Selected = true;
            //}
        }

        /// <summary>
        /// �󼼳��� Text �ʱ�ȭ
        /// </summary>
        private void ResetDetailText()
        {
            // EAP�� ��� �⺻��
            if (keyAdType.Equals("11") || keyAdType.Equals("12"))
            {
                // ���⹰��-������
                ebContractAmt.Text = "0";
                ebContractAmt.Enabled = false;

                ebContractTot.Text = "0";
                ebContractTot.ReadOnly = true;

                // ��-���
                //cbPriorityCd.SelectedIndex = 5;
                //cbPriorityCd.Enabled    = true;

                // �����-������
                rbControlYn_Y.Checked = false;
                rbControlYn_N.Checked = true;
                rbControlYn_Y.Enabled = false;
                rbControlYn_N.Enabled = false;

                // �������-������
                udControlRate.Value = 100;
                udControlRate.Enabled = false;

                // �������� �ʱ�ȭ 
                chkTimeYn.Checked = false;
                clearCheckingRegion();

                // ����ð��� �ʱ�ȭ
                chkRegionYn.Checked = false;

                // ���⿬�ɴ� �ʱ�ȭ
                chkAgeYn.Checked = false;
                SetTargetAge(null);

                // ���⿬�ɱ���
                chkAgeBtnYn.Checked = false;
                udAgeBtnBegin.Value = 0;
                udAgeBtnEnd.Value = 99;

                // ���⼺��
                chkSexYn.Checked = false;
                chkSexMan.Checked = false;
                chkSexWoman.Checked = false;

                // ����
                chkCollectionYn.Enabled = true;
                chkCollectionYn.Checked = false;
                // [E-07]
                //cbCollection.Enabled        =	false;		
                //rbCollectionPlus.Checked    =   true;
                //rbCollectionPlus.Enabled    =   false;
                //rbCollectionMinus.Enabled   =   false;

                if (keyAdType.Equals("12"))
                {
                    // 12:SCM�� Ÿ�ٰ����� ������� �ʴ´�
                    chkCollectionYn.Enabled = false;
                }

                // ������
                chkRateYn.Checked = false;
                rbRateAll.Checked = true;
                rbRate12.Checked = false;
                rbRate15.Checked = false;
                rbRate19.Checked = false;

                rbRatePlus.Checked = true;
                rbRateMinus.Checked = false;

                // ���Ϻ�
                chkWeekYn.Checked = false;
                chkMon.Checked = false;
                chkThu.Checked = false;
                chkWed.Checked = false;
                chkThe.Checked = false;
                chkFri.Checked = false;
                chkSat.Checked = false;
                chkSun.Checked = false;

                chkPPx.Checked = false;
                chkPPxOnly.Checked = false;  //����ä�θ� ���� ���� - 2014.09.05 Youngil.Yi ����
                chkPVS.Checked = false;

                // ������������
                chkFreq.Checked = false;
                freqDay.Value = 0;
                freqPeriod.Value = 0;

                // 2����
                chkSlotYn.Checked = false;
                rbSlotForward.Checked = false;
                rbSlotBackward.Checked = false;
                grpSlot.Enabled = false;

                // ��ž�𵨺� [E_08]
                chkStbModel.Checked = false;
                grpStb.Enabled = false;
                SetTargetStb(null);

                // [E_09] ��ȣ�������˾�
                chkPreference.Checked = false;
                grpPreference.Enabled = false;

                // [E_10] �������� Ÿ����
                chkProfileYn.Checked = false;
                tbReliability.Value = 5;

                keyAge = "";
            }
            else
            {
                // ���⹰��-���
                ebContractAmt.Text = "0";
                ebContractAmt.Enabled = true;

                ebContractTot.Text = "0";
                ebContractTot.Enabled = false;

                // ��-������
                //cbPriorityCd.SelectedIndex = 5;
                //cbPriorityCd.Enabled    = false;

                if (!sTypeName.Equals("�ð��뵶����"))
                {
                    // �����-���
                    rbControlYn_Y.Checked = true;
                    rbControlYn_N.Checked = false;
                    rbControlYn_Y.Enabled = true;
                    rbControlYn_N.Enabled = true;

                    // �������-���
                    udControlRate.Value = 100;
                    udControlRate.Enabled = true;
                }
                else
                {
                    rbControlYn_Y.Checked = false;
                    rbControlYn_N.Checked = true;
                    rbControlYn_Y.Enabled = false;
                    rbControlYn_N.Enabled = false;
                    udControlRate.Value = 100;
                    udControlRate.Enabled = false;
                }

                // �������� �ʱ�ȭ 
                chkTimeYn.Checked = false;
                clearCheckingRegion();

                // ����ð��� �ʱ�ȭ
                chkRegionYn.Checked = false;

                // ���⿬�ɴ� �ʱ�ȭ
                chkAgeYn.Checked = false;
                SetTargetAge(null);

                // ���⿬�ɱ���
                chkAgeBtnYn.Checked = false;
                udAgeBtnBegin.Value = 0;
                udAgeBtnEnd.Value = 0;

                // ���⼺��
                chkSexYn.Checked = false;
                chkSexMan.Checked = false;
                chkSexWoman.Checked = false;

                // ����ž���� EAP�� ���
                chkCollectionYn.Enabled = true;
                chkCollectionYn.Checked = false;

                // [E-07]
                //cbCollection.Enabled        =   false;		
                //rbCollectionPlus.Checked    =   true;
                //rbCollectionPlus.Enabled    =   false;
                //rbCollectionMinus.Enabled   =   false;

                // ������
                chkRateYn.Checked = false;
                rbRateAll.Checked = true;
                rbRate12.Checked = false;
                rbRate15.Checked = false;
                rbRate19.Checked = false;

                rbRatePlus.Checked = true;
                rbRateMinus.Checked = false;

                // ���Ϻ�
                chkWeekYn.Checked = false;
                chkMon.Checked = false;
                chkThu.Checked = false;
                chkWed.Checked = false;
                chkThe.Checked = false;
                chkFri.Checked = false;
                chkSat.Checked = false;
                chkSun.Checked = false;

                chkPPx.Checked = false;
                chkPPxOnly.Checked = false;  //����ä�θ� ���� ���� - 2014.09.05 Youngil.Yi ����
                chkPVS.Checked = false;
                // ������������
                chkFreq.Checked = false;
                freqDay.Value = 0;
                freqPeriod.Value = 0;

                // 2����
                chkSlotYn.Checked = false;
                rbSlotForward.Checked = false;
                rbSlotBackward.Checked = false;
                grpSlot.Enabled = true;

                // ��ž�𵨺� [E_08]
                chkStbModel.Checked = false;
                grpStb.Enabled = false;
                //tvStb.Enabled = false;
                SetTargetStb(null);

                //[E_09] ��ȣ�������˾�
                chkPreference.Checked = false;
                grpPreference.Enabled = false;

                // [E_10] �������� Ÿ����
                chkProfileYn.Checked = false;
                chkU00.Checked = false;
                chkM10.Checked = false;
                chkF10.Checked = false;
                chKM20.Checked = false;
                chkF20.Checked = false;
                chkM30.Checked = false;
                chkF30.Checked = false;
                chkM40.Checked = false;
                chkF40.Checked = false;
                chkM50.Checked = false;
                chkF50.Checked = false;
                chkM60.Checked = false;
                chkF60.Checked = false;
                tbReliability.Value = 5;

                keyAge = "";
            }
        }

        private void ResetRateText()
        {
#if (DEBUG)
            Debug.WriteLine(MethodBase.GetCurrentMethod().Name);
#endif
            udRegionRate1.Enabled = false;
            udRegionRate2.Enabled = false;
            udRegionRate3.Enabled = false;
            udRegionRate4.Enabled = false;
            udRegionRate5.Enabled = false;
            udRegionRate6.Enabled = false;
            udRegionRate7.Enabled = false;
            udRegionRate8.Enabled = false;
            udRegionRate9.Enabled = false;
            udRegionRate10.Enabled = false;
            udRegionRate11.Enabled = false;
            udRegionRate12.Enabled = false;
            udRegionRate13.Enabled = false;
            udRegionRate14.Enabled = false;
            udRegionRate15.Enabled = false;
            udRegionRate16.Enabled = false;
            udRegionRate17.Enabled = false;

            udTimeRate0.Enabled = false;
            udTimeRate1.Enabled = false;
            udTimeRate2.Enabled = false;
            udTimeRate3.Enabled = false;
            udTimeRate4.Enabled = false;
            udTimeRate5.Enabled = false;
            udTimeRate6.Enabled = false;
            udTimeRate7.Enabled = false;
            udTimeRate8.Enabled = false;
            udTimeRate9.Enabled = false;
            udTimeRate10.Enabled = false;
            udTimeRate11.Enabled = false;
            udTimeRate12.Enabled = false;
            udTimeRate13.Enabled = false;
            udTimeRate14.Enabled = false;
            udTimeRate15.Enabled = false;
            udTimeRate16.Enabled = false;
            udTimeRate17.Enabled = false;
            udTimeRate18.Enabled = false;
            udTimeRate19.Enabled = false;
            udTimeRate20.Enabled = false;
            udTimeRate21.Enabled = false;
            udTimeRate22.Enabled = false;
            udTimeRate23.Enabled = false;

            udMonRate.Enabled = false;
            udThuRate.Enabled = false;
            udWedRate.Enabled = false;
            udTheRate.Enabled = false;
            udFriRate.Enabled = false;
            udSatRate.Enabled = false;
            udSunRate.Enabled = false;

            udRegionRate1.Value = 0;
            udRegionRate2.Value = 0;
            udRegionRate3.Value = 0;
            udRegionRate4.Value = 0;
            udRegionRate5.Value = 0;
            udRegionRate6.Value = 0;
            udRegionRate7.Value = 0;
            udRegionRate8.Value = 0;
            udRegionRate9.Value = 0;
            udRegionRate10.Value = 0;
            udRegionRate11.Value = 0;
            udRegionRate12.Value = 0;
            udRegionRate13.Value = 0;
            udRegionRate14.Value = 0;
            udRegionRate15.Value = 0;
            udRegionRate16.Value = 0;
            udRegionRate17.Value = 0;

            udTimeRate0.Value = 0;
            udTimeRate1.Value = 0;
            udTimeRate2.Value = 0;
            udTimeRate3.Value = 0;
            udTimeRate4.Value = 0;
            udTimeRate5.Value = 0;
            udTimeRate6.Value = 0;
            udTimeRate7.Value = 0;
            udTimeRate8.Value = 0;
            udTimeRate9.Value = 0;
            udTimeRate10.Value = 0;
            udTimeRate11.Value = 0;
            udTimeRate12.Value = 0;
            udTimeRate13.Value = 0;
            udTimeRate14.Value = 0;
            udTimeRate15.Value = 0;
            udTimeRate16.Value = 0;
            udTimeRate17.Value = 0;
            udTimeRate18.Value = 0;
            udTimeRate19.Value = 0;
            udTimeRate20.Value = 0;
            udTimeRate21.Value = 0;
            udTimeRate22.Value = 0;
            udTimeRate23.Value = 0;

            udMonRate.Value = 0;
            udThuRate.Value = 0;
            udWedRate.Value = 0;
            udTheRate.Value = 0;
            udFriRate.Value = 0;
            udSatRate.Value = 0;
            udSunRate.Value = 0;

        }

        #endregion

        #region �̺�Ʈ�Լ�

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

        #region ���� ���
        /// <summary>
        /// ���� ����
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnExcel_Click(object sender, System.EventArgs e)
        {
#if (DEBUG)
            Debug.WriteLine(MethodBase.GetCurrentMethod().Name);
#endif

            Excel.Application xlApp = null;
            Excel._Workbook oWB = null;
            Excel._Worksheet oSheet = null;
            Excel.Range oRng = null;

            try
            {
                int ColMax = 13; // �÷���   				

                int TitleRow = 1;
                int ConditionRow1 = 2;
                int HeaderRow = 3;
                string StartCol = "A";
                string EndCol = "";
                string TitleCol = "M";
                int CondCount = 0;
                int HeaderCount = 0;

                // ������ �÷��� �ε�������
                EndCol = GetColumnIndex(ColMax);

                xlApp = new Excel.Application();
                oWB = (Excel._Workbook)(xlApp.Workbooks.Add(Missing.Value));
                oSheet = (Excel._Worksheet)oWB.ActiveSheet;


                // Ÿ��Ʋ �ۼ�
                oSheet.Cells[TitleRow, 1] = "���������";
                oRng = oSheet.get_Range(StartCol + Convert.ToString(TitleRow), TitleCol + Convert.ToString(TitleRow));
                oRng.Merge(true);
                oRng.Font.Bold = true;
                oRng.Font.Size = 16;
                oRng.VerticalAlignment = Excel.XlVAlign.xlVAlignCenter;
                oRng.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;

                // �������� �ۼ�
                oSheet.Cells[ConditionRow1 + CondCount, 1] = "�������";
                oRng = oSheet.get_Range("B" + Convert.ToString(ConditionRow1 + CondCount), TitleCol + Convert.ToString(ConditionRow1 + CondCount));
                oRng.Merge(true);

                if (chkAdState_20.Checked)
                {
                    oSheet.Cells[ConditionRow1 + CondCount, 2] = "��";
                }
                if (chkAdState_30.Checked)
                {
                    oSheet.Cells[ConditionRow1 + CondCount, 2] = "����";
                }
                if (chkAdState_40.Checked)
                {
                    oSheet.Cells[ConditionRow1 + CondCount, 2] = "����";
                }
                if (chkAdState_20.Checked && chkAdState_30.Checked)
                {
                    oSheet.Cells[ConditionRow1 + CondCount, 2] = "��, ����";
                }
                if (chkAdState_20.Checked && chkAdState_40.Checked)
                {
                    oSheet.Cells[ConditionRow1 + CondCount, 2] = "��, ����";
                }
                if (chkAdState_30.Checked && chkAdState_40.Checked)
                {
                    oSheet.Cells[ConditionRow1 + CondCount, 2] = "����, ����";
                }
                if (chkAdState_20.Checked && chkAdState_30.Checked && chkAdState_40.Checked)
                {
                    oSheet.Cells[ConditionRow1 + CondCount, 2] = "��, ����, ����";
                }
                // ���Ǻ� �׵θ�
                oRng = oSheet.get_Range(StartCol + Convert.ToString(ConditionRow1), TitleCol + Convert.ToString(ConditionRow1 + (CondCount - 1)));
                oRng.VerticalAlignment = Excel.XlVAlign.xlVAlignCenter;
                oRng.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;

                oRng.Borders.LineStyle = Excel.XlLineStyle.xlContinuous;	// �׵θ��Ӽ� �Ʒ��� �Ǽ�
                oRng.Borders.Weight = Excel.XlBorderWeight.xlThin;		// �׵θ��Ӽ� �Ʒ��� ���¼�

                CondCount++;

                // ��� ���� �ۼ�
                HeaderCount = 1;
                oSheet.Cells[HeaderRow, HeaderCount++] = "�����ȣ";
                //				oRng = oSheet.get_Range(GetColumnIndex(1)+Convert.ToString(HeaderRow), GetColumnIndex(1)+Convert.ToString(HeaderRow));
                //				oRng.Merge(true);
                //				HeaderCount++;

                oSheet.Cells[HeaderRow, HeaderCount++] = "�����";
                oSheet.Cells[HeaderRow, HeaderCount++] = "������";
                oSheet.Cells[HeaderRow, HeaderCount++] = "���������";
                oSheet.Cells[HeaderRow, HeaderCount++] = "����������";
                oSheet.Cells[HeaderRow, HeaderCount++] = "����������";
                oSheet.Cells[HeaderRow, HeaderCount++] = "��������";
                oSheet.Cells[HeaderRow, HeaderCount++] = "�������";
                oSheet.Cells[HeaderRow, HeaderCount++] = "�������";
                oSheet.Cells[HeaderRow, HeaderCount++] = "��๰��";
                oSheet.Cells[HeaderRow, HeaderCount++] = "�����";
                oSheet.Cells[HeaderRow, HeaderCount++] = "����";
                oSheet.Cells[HeaderRow, HeaderCount++] = "������";


                oRng = oSheet.get_Range(StartCol + Convert.ToString(HeaderRow), EndCol + Convert.ToString(HeaderRow)); // ����� ����
                oRng.Font.Bold = true;							// ��Ʈ ����
                oRng.VerticalAlignment = Excel.XlVAlign.xlVAlignCenter;	// �����߾�����
                oRng.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;	// �����߾�����	 
                oRng.Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.CornflowerBlue);   //�� ���� 
                oRng.Font.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.White);	  //�ؽ�Ʈ��			

                string[,] items = new string[targetingDs.Tables[0].Rows.Count, 13];
                // ������ ����
                for (int inx = 0; inx < targetingDs.Tables[0].Rows.Count; inx++)
                {
                    items[inx, 0] = targetingDs.Tables[0].Rows[inx]["ItemNo"].ToString();
                    items[inx, 1] = targetingDs.Tables[0].Rows[inx]["ItemName"].ToString();
                    items[inx, 2] = targetingDs.Tables[0].Rows[inx]["ContStateName"].ToString();
                    items[inx, 3] = targetingDs.Tables[0].Rows[inx]["ExcuteStartDay"].ToString();
                    items[inx, 4] = targetingDs.Tables[0].Rows[inx]["ExcuteEndDay"].ToString();
                    items[inx, 5] = targetingDs.Tables[0].Rows[inx]["RealEndDay"].ToString();
                    items[inx, 6] = targetingDs.Tables[0].Rows[inx]["AdTypeName"].ToString();
                    items[inx, 7] = targetingDs.Tables[0].Rows[inx]["AdTime"].ToString();
                    items[inx, 8] = targetingDs.Tables[0].Rows[inx]["AdStateName"].ToString();
                    items[inx, 9] = targetingDs.Tables[0].Rows[inx]["TgtAmount"].ToString();
                    items[inx, 10] = targetingDs.Tables[0].Rows[inx]["PriorityCd"].ToString();
                    items[inx, 11] = targetingDs.Tables[0].Rows[inx]["ContractName"].ToString();
                    items[inx, 12] = targetingDs.Tables[0].Rows[inx]["AdvertiserName"].ToString();
                }
                oSheet.get_Range("A4", "L" + Convert.ToString((items.Length / 13) + 3)).set_Value(Missing.Value, items);

                oRng = oSheet.get_Range(StartCol + Convert.ToString(HeaderRow), EndCol + Convert.ToString(targetingDs.Tables[0].Rows.Count+3));	// �������� ����
                oRng.EntireColumn.AutoFit();					// �������� ũ�⿡ ���� ����ũ�� ����

                oRng.Borders.LineStyle = Excel.XlLineStyle.xlContinuous;	// �׵θ��Ӽ� �Ʒ��� �Ǽ�
                oRng.Borders.Weight = Excel.XlBorderWeight.xlThin;		// �׵θ��Ӽ� �Ʒ��� ���¼�

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
#if (DEBUG)
            Debug.WriteLine(MethodBase.GetCurrentMethod().Name);
#endif
            string[] ColName = { "Z", "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y" };

            string ColumnIndex;

            // 26���� ũ��
            if (ColCount > ColName.Length)
            {
                // 2�ڸ� �ε������� 26 => Z;  27->AA
                ColumnIndex = ColName[(int)(Math.Floor((float)(ColCount / ColName.Length)))] + ColName[(int)(Math.Floor((float)(ColCount % ColName.Length)))];
            }
            else
            {
                ColumnIndex = ColName[(int)(Math.Floor((float)(ColCount % ColName.Length)))];
            }

            return ColumnIndex;
        }

        #endregion

        #region TreeView ����

        private void ChangeNodeCheck(TreeNode nowNode)
        {
            TreeNode parentNode = nowNode.Parent; // �θ���

            if (parentNode != null)
            {
                int nodeCount = parentNode.GetNodeCount(false);	// �θ����� �ڽĳ��� = ��������� ��, �������� ���Ծ��� 
                int checkedCount = 0;

                // �������鿡�� üũ�� ���� ��´�.
                foreach (TreeNode childNode in parentNode.Nodes)
                {
                    if (childNode.Checked) checkedCount++;
                }

                // ��� üũ�� �Ǿ��ٸ�
                if (nodeCount == checkedCount)
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

            if (node.Checked)
            {
                // ���� �� : [E_01] ���� ���� �ڵ�
                /*
                if(node.Tag != null)
                {
                    tag = node.Tag.ToString() + dilm;
                }
                */

                // �����ڵ�: [E_01]
                // �ڽĳ�尡 �ִ� �ڵ�� �Է¾���(Select �� �ߺ��� ��)
                if (node.Nodes.Count == 0)
                {
                    if (node.Tag != null)
                    {
                        tag = node.Tag.ToString() + dilm;
                    }
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
            if (chkList == null) return false;

            for (int i = 0; i < chkList.Length; i++)
            {
                if (Code.Equals(chkList[i]))
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

            if (node.Tag != null)
            {
                tag = node.Tag.ToString();

                // �������� ���� üũ����Ʈ�� �����ϴ��� �˻�
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

            // ������� �˻�
            foreach (TreeNode childNode in node.Nodes)
            {
                cnt += SetAllNodeTagCheck(childNode, chkList);
            }
            return cnt;
        }

        #endregion

        #region �հ��̺�Ʈ
        private void TotRegionText()
        {
#if (DEBUG)
            Debug.WriteLine(MethodBase.GetCurrentMethod().Name);
#endif
            rate1 = Convert.ToInt32(udRegionRate1.Value);
            rate2 = Convert.ToInt32(udRegionRate2.Value);
            rate3 = Convert.ToInt32(udRegionRate3.Value);
            rate4 = Convert.ToInt32(udRegionRate4.Value);
            rate5 = Convert.ToInt32(udRegionRate5.Value);
            rate6 = Convert.ToInt32(udRegionRate6.Value);
            rate7 = Convert.ToInt32(udRegionRate7.Value);
            rate8 = Convert.ToInt32(udRegionRate8.Value);
            rate9 = Convert.ToInt32(udRegionRate9.Value);
            rate10 = Convert.ToInt32(udRegionRate10.Value);
            rate11 = Convert.ToInt32(udRegionRate11.Value);
            rate12 = Convert.ToInt32(udRegionRate12.Value);
            rate13 = Convert.ToInt32(udRegionRate13.Value);
            rate14 = Convert.ToInt32(udRegionRate14.Value);
            rate15 = Convert.ToInt32(udRegionRate15.Value);
            rate16 = Convert.ToInt32(udRegionRate16.Value);
            rate17 = Convert.ToInt32(udRegionRate17.Value);
            tot1 = rate1 + rate2 + rate3 + rate4 + rate5 + rate6 + rate7 + rate8 + rate9 + rate10 + rate11 + rate12 + rate13 + rate14 + rate15 + rate16 + rate17;
            if (tot1 > 100)
            {
                MessageBox.Show("�������������� 100%�� �ʰ��Ͽ����ϴ� \n Ȯ���Ͽ��ּ���!!", "Ÿ���ú���",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                ebRegionTot.Text = tot1.ToString();
            }
        }

        private void TotTimeText()
        {
#if (DEBUG)
            Debug.WriteLine(MethodBase.GetCurrentMethod().Name);
#endif
            rate1 = Convert.ToInt32(udTimeRate0.Value);
            rate2 = Convert.ToInt32(udTimeRate1.Value);
            rate3 = Convert.ToInt32(udTimeRate2.Value);
            rate4 = Convert.ToInt32(udTimeRate3.Value);
            rate5 = Convert.ToInt32(udTimeRate4.Value);
            rate6 = Convert.ToInt32(udTimeRate5.Value);
            rate7 = Convert.ToInt32(udTimeRate6.Value);
            rate8 = Convert.ToInt32(udTimeRate7.Value);
            rate9 = Convert.ToInt32(udTimeRate8.Value);
            rate10 = Convert.ToInt32(udTimeRate9.Value);
            rate11 = Convert.ToInt32(udTimeRate10.Value);
            rate12 = Convert.ToInt32(udTimeRate11.Value);
            rate13 = Convert.ToInt32(udTimeRate12.Value);
            rate14 = Convert.ToInt32(udTimeRate13.Value);
            rate15 = Convert.ToInt32(udTimeRate14.Value);
            rate16 = Convert.ToInt32(udTimeRate15.Value);
            rate17 = Convert.ToInt32(udTimeRate16.Value);
            rate18 = Convert.ToInt32(udTimeRate17.Value);
            rate19 = Convert.ToInt32(udTimeRate18.Value);
            rate20 = Convert.ToInt32(udTimeRate19.Value);
            rate21 = Convert.ToInt32(udTimeRate20.Value);
            rate22 = Convert.ToInt32(udTimeRate21.Value);
            rate23 = Convert.ToInt32(udTimeRate22.Value);
            rate24 = Convert.ToInt32(udTimeRate23.Value);
            tot2 = rate1 + rate2 + rate3 + rate4 + rate5 + rate6 + rate7 + rate8 + rate9 + rate10 + rate11 + rate12 + rate13 + rate14 + rate15 + rate16 + rate17 + rate18 + rate19 + rate20 + rate21 + rate22 + rate23 + rate24;
            if (tot2 > 100)
            {
                MessageBox.Show("����ð������� 100%�� �ʰ��Ͽ����ϴ� \n Ȯ���Ͽ��ּ���!!", "Ÿ���ú���",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                ebTimeTot.Text = tot2.ToString();
            }
        }

        private void TotWeekText()
        {
#if (DEBUG)
            Debug.WriteLine(MethodBase.GetCurrentMethod().Name);
#endif
            rate1 = Convert.ToInt32(udMonRate.Value);
            rate2 = Convert.ToInt32(udThuRate.Value);
            rate3 = Convert.ToInt32(udWedRate.Value);
            rate4 = Convert.ToInt32(udTheRate.Value);
            rate5 = Convert.ToInt32(udFriRate.Value);
            rate6 = Convert.ToInt32(udSatRate.Value);
            rate7 = Convert.ToInt32(udSunRate.Value);

            tot3 = rate1 + rate2 + rate3 + rate4 + rate5 + rate6 + rate7;
            if (tot3 > 100)
            {
                MessageBox.Show("������Ϻ����� 100%�� �ʰ��Ͽ����ϴ� \n Ȯ���Ͽ��ּ���!!", "Ÿ���ú���",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                ebWeekTot.Text = tot3.ToString();
            }
        }

        private void udRegionRate3_KeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
        {
#if (DEBUG)
            Debug.WriteLine(MethodBase.GetCurrentMethod().Name);
#endif
            TotRegionText();
        }

        private void udRegionRate2_KeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
        {
#if (DEBUG)
            Debug.WriteLine(MethodBase.GetCurrentMethod().Name);
#endif
            TotRegionText();
        }

        private void udRegionRate1_KeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
        {
#if (DEBUG)
            Debug.WriteLine(MethodBase.GetCurrentMethod().Name);
#endif
            TotRegionText();
        }

        private void udRegionRate4_KeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
        {
#if (DEBUG)
            Debug.WriteLine(MethodBase.GetCurrentMethod().Name);
#endif
            TotRegionText();
        }

        private void udRegionRate5_KeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
        {
#if (DEBUG)
            Debug.WriteLine(MethodBase.GetCurrentMethod().Name);
#endif
            TotRegionText();
        }

        private void udRegionRate6_KeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
        {
#if (DEBUG)
            Debug.WriteLine(MethodBase.GetCurrentMethod().Name);
#endif
            TotRegionText();
        }

        private void udRegionRate7_KeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
        {
#if (DEBUG)
            Debug.WriteLine(MethodBase.GetCurrentMethod().Name);
#endif
            TotRegionText();
        }

        private void udRegionRate8_KeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
        {
#if (DEBUG)
            Debug.WriteLine(MethodBase.GetCurrentMethod().Name);
#endif
            TotRegionText();
        }

        private void udRegionRate9_KeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
        {
#if (DEBUG)
            Debug.WriteLine(MethodBase.GetCurrentMethod().Name);
#endif
            TotRegionText();
        }

        private void udRegionRate10_KeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
        {
#if (DEBUG)
            Debug.WriteLine(MethodBase.GetCurrentMethod().Name);
#endif
            TotRegionText();
        }

        private void udRegionRate11_KeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
        {
#if (DEBUG)
            Debug.WriteLine(MethodBase.GetCurrentMethod().Name);
#endif
            TotRegionText();
        }

        private void udRegionRate12_KeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
        {
#if (DEBUG)
            Debug.WriteLine(MethodBase.GetCurrentMethod().Name);
#endif
            TotRegionText();
        }

        private void udRegionRate13_KeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
        {
#if (DEBUG)
            Debug.WriteLine(MethodBase.GetCurrentMethod().Name);
#endif
            TotRegionText();
        }

        private void udRegionRate14_KeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
        {
#if (DEBUG)
            Debug.WriteLine(MethodBase.GetCurrentMethod().Name);
#endif
            TotRegionText();
        }

        private void udRegionRate15_KeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
        {
#if (DEBUG)
            Debug.WriteLine(MethodBase.GetCurrentMethod().Name);
#endif
            TotRegionText();
        }

        private void udRegionRate16_KeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
        {
#if (DEBUG)
            Debug.WriteLine(MethodBase.GetCurrentMethod().Name);
#endif
            TotRegionText();
        }

        private void udRegionRate17_KeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
        {
#if (DEBUG)
            Debug.WriteLine(MethodBase.GetCurrentMethod().Name);
#endif
            TotRegionText();
        }

        private void udTimeRate0_KeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
        {
#if (DEBUG)
            Debug.WriteLine(MethodBase.GetCurrentMethod().Name);
#endif
            TotTimeText();
        }

        private void udTimeRate1_KeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
        {
#if (DEBUG)
            Debug.WriteLine(MethodBase.GetCurrentMethod().Name);
#endif
            TotTimeText();
        }

        private void udTimeRate2_KeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
        {
#if (DEBUG)
            Debug.WriteLine(MethodBase.GetCurrentMethod().Name);
#endif
            TotTimeText();
        }

        private void udTimeRate3_KeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
        {
#if (DEBUG)
            Debug.WriteLine(MethodBase.GetCurrentMethod().Name);
#endif
            TotTimeText();
        }

        private void udTimeRate4_KeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
        {
#if (DEBUG)
            Debug.WriteLine(MethodBase.GetCurrentMethod().Name);
#endif
            TotTimeText();
        }

        private void udTimeRate5_KeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
        {
#if (DEBUG)
            Debug.WriteLine(MethodBase.GetCurrentMethod().Name);
#endif
            TotTimeText();
        }

        private void udTimeRate6_KeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
        {
#if (DEBUG)
            Debug.WriteLine(MethodBase.GetCurrentMethod().Name);
#endif
            TotTimeText();
        }

        private void udTimeRate7_KeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
        {
#if (DEBUG)
            Debug.WriteLine(MethodBase.GetCurrentMethod().Name);
#endif
            TotTimeText();
        }

        private void udTimeRate8_KeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
        {
#if (DEBUG)
            Debug.WriteLine(MethodBase.GetCurrentMethod().Name);
#endif
            TotTimeText();
        }

        private void udTimeRate9_KeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
        {
#if (DEBUG)
            Debug.WriteLine(MethodBase.GetCurrentMethod().Name);
#endif
            TotTimeText();
        }

        private void udTimeRate10_KeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
        {
#if (DEBUG)
            Debug.WriteLine(MethodBase.GetCurrentMethod().Name);
#endif
            TotTimeText();
        }

        private void udTimeRate11_KeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
        {
#if (DEBUG)
            Debug.WriteLine(MethodBase.GetCurrentMethod().Name);
#endif
            TotTimeText();
        }

        private void udTimeRate12_KeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
        {
#if (DEBUG)
            Debug.WriteLine(MethodBase.GetCurrentMethod().Name);
#endif
            TotTimeText();
        }

        private void udTimeRate13_KeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
        {
#if (DEBUG)
            Debug.WriteLine(MethodBase.GetCurrentMethod().Name);
#endif
            TotTimeText();
        }

        private void udTimeRate14_KeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
        {
#if (DEBUG)
            Debug.WriteLine(MethodBase.GetCurrentMethod().Name);
#endif
            TotTimeText();
        }

        private void udTimeRate15_KeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
        {
#if (DEBUG)
            Debug.WriteLine(MethodBase.GetCurrentMethod().Name);
#endif
            TotTimeText();
        }

        private void udTimeRate16_KeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
        {
#if (DEBUG)
            Debug.WriteLine(MethodBase.GetCurrentMethod().Name);
#endif
            TotTimeText();
        }

        private void udTimeRate17_KeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
        {
#if (DEBUG)
            Debug.WriteLine(MethodBase.GetCurrentMethod().Name);
#endif
            TotTimeText();
        }

        private void udTimeRate18_KeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
        {
#if (DEBUG)
            Debug.WriteLine(MethodBase.GetCurrentMethod().Name);
#endif
            TotTimeText();
        }

        private void udTimeRate19_KeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
        {
#if (DEBUG)
            Debug.WriteLine(MethodBase.GetCurrentMethod().Name);
#endif
            TotTimeText();
        }

        private void udTimeRate20_KeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
        {
#if (DEBUG)
            Debug.WriteLine(MethodBase.GetCurrentMethod().Name);
#endif
            TotTimeText();
        }

        private void udTimeRate21_KeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
        {
#if (DEBUG)
            Debug.WriteLine(MethodBase.GetCurrentMethod().Name);
#endif
            TotTimeText();
        }

        private void udTimeRate22_KeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
        {
#if (DEBUG)
            Debug.WriteLine(MethodBase.GetCurrentMethod().Name);
#endif
            TotTimeText();
        }

        private void udTimeRate23_KeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
        {
#if (DEBUG)
            Debug.WriteLine(MethodBase.GetCurrentMethod().Name);
#endif
            TotTimeText();
        }

        private void udMonRate_KeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
        {
#if (DEBUG)
            Debug.WriteLine(MethodBase.GetCurrentMethod().Name);
#endif
            TotWeekText();
        }

        private void udThuRate_KeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
        {
#if (DEBUG)
            Debug.WriteLine(MethodBase.GetCurrentMethod().Name);
#endif
            TotWeekText();
        }

        private void udWedRate_KeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
        {
#if (DEBUG)
            Debug.WriteLine(MethodBase.GetCurrentMethod().Name);
#endif
            TotWeekText();
        }

        private void udTheRate_KeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
        {
#if (DEBUG)
            Debug.WriteLine(MethodBase.GetCurrentMethod().Name);
#endif
            TotWeekText();
        }

        private void udFriRate_KeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
        {
#if (DEBUG)
            Debug.WriteLine(MethodBase.GetCurrentMethod().Name);
#endif
            TotWeekText();
        }

        private void udSatRate_KeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
        {
#if (DEBUG)
            Debug.WriteLine(MethodBase.GetCurrentMethod().Name);
#endif
            TotWeekText();
        }

        private void udSunRate_KeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
        {
#if (DEBUG)
            Debug.WriteLine(MethodBase.GetCurrentMethod().Name);
#endif
            TotWeekText();
        }
        #endregion

        private void OnControlYnCheckedChanged(object sender, System.EventArgs e)
        {
#if (DEBUG)
            Debug.WriteLine(MethodBase.GetCurrentMethod().Name);
#endif
            //if( rbControlYn_N.Checked ) cbPriorityCd.Enabled = false;
            //if( rbControlYn_Y.Checked ) cbPriorityCd.Enabled = true;
        }

        #region �����ȣ Ÿ����
        //private string keyZipCode = "";
        //private string keyZipName = "";

        /// <summary>
        /// Ÿ���� ���� �� �����ȣ ���� Ȯ��(read only)
        /// </summary>		
        private void ebZipCode_ButtonClick(object sender, System.EventArgs e)
        {
#if (DEBUG)
            Debug.WriteLine(MethodBase.GetCurrentMethod().Name);
#endif
            #region [E_02] �ּ�...
            /*
			Common.ZipNoForm pForm = new AdManagerClient.Common.ZipNoForm();
			pForm.SelectZipCode += new AdManagerClient.Common.ZipCodeEventHandler(pForm_SelectZipCode);
			//pForm.SelectContents += new AdManagerClient.Common.ContentsEventHandler(pForm_SelectContents);

			if( pForm.ShowDialog() == DialogResult.No )
			{
				keyZipCode	 = "";
				keyZipName	= "";
			}
			
			pForm.Dispose();
			pForm = null;
			*/
            #endregion

            Common.ZipNoForm pForm = null;
            DataTable dt = new DataTable("zip");
            dt.Columns.Add("ZipCode", typeof(string));

            try
            {
                // ���� ���� �� �����ȣ�� �ִٸ� ��Ī �����ش�.
                if (ebZipCode.Text.Length > 6)
                {
                    pForm = new AdManagerClient.Common.ZipNoForm();
                    pForm.IsReadOnly = true; // ���� �����ȣ �� Ȯ�ο� ��.
                    pForm.IsNewZip = false;  // �߰� �۾��� �ƴ�.		
                    string[] zipCode = Utility.SplitByString(ebZipCode.Text.Trim(), "^");

                    for (int i = 0; i < zipCode.Length; i++)
                    {
                        DataRow row = dt.NewRow();
                        row["ZipCode"] = zipCode[i].ToString();
                        dt.Rows.Add(row);
                    }

                    pForm.ZipCodes = dt.Copy();
                    pForm.ShowDialog();
                }
            }
            catch (Exception)
            {
            }
            finally
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }

                if (pForm != null)
                {
                    pForm.Dispose();
                    pForm = null;
                }
            }
        }

        /// <summary>
        /// �����ȣ �ű� �߰�
        /// </summary>		
        private void btnNewZip_Click(object sender, System.EventArgs e)
        {
#if (DEBUG)
            Debug.WriteLine(MethodBase.GetCurrentMethod().Name);
#endif
            Common.ZipNoForm pForm = null;
            DataTable dt = null;

            try
            {
                pForm = new AdManagerClient.Common.ZipNoForm();
                pForm.IsReadOnly = false;
                pForm.IsNewZip = true; // �ű� �߰�(���� ���� ��� �����Ǵ� ����)							
                if (pForm.ShowDialog() == DialogResult.Yes)
                {
                    dt = pForm.ZipCodes;
                    ebZipCode.Text = "";
                    foreach (DataRow row in dt.Rows)
                    {
                        ebZipCode.Text += row["ZipCode"].ToString() + "^";
                    }

                    if (ebZipCode.Text.Length > 6) // ������ ���� '^' ����
                        ebZipCode.Text = ebZipCode.Text.Substring(0, ebZipCode.Text.Length - 1);
                }
            }
            catch (Exception)
            {
            }
            finally
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }

                if (pForm != null)
                {
                    pForm.Dispose();
                    pForm = null;
                }
            }
        }


        /// <summary>
        /// �����ȣ �߰�(���� ���� �̾)[E_02]
        /// </summary>		
        private void btnAddZip_Click(object sender, System.EventArgs e)
        {
#if (DEBUG)
            Debug.WriteLine(MethodBase.GetCurrentMethod().Name);
#endif
            Common.ZipNoForm pForm = null;
            DataTable dt = new DataTable("zip");
            dt.Columns.Add("ZipCode", typeof(string));

            try
            {
                pForm = new AdManagerClient.Common.ZipNoForm();
                pForm.IsReadOnly = false;
                pForm.IsNewZip = false; // ���������ȣ�� �̾ �߰��ϴ� ��.

                // ���� ���� �� �����ȣ�� �ִٸ� ��Ī �����ش�.
                if (ebZipCode.Text.Length >= 6)
                {
                    string[] zipCode = Utility.SplitByString(ebZipCode.Text.Trim(), "^");

                    for (int i = 0; i < zipCode.Length; i++)
                    {
                        DataRow row = dt.NewRow();
                        row["ZipCode"] = zipCode[i];
                        dt.Rows.Add(row);
                    }
                    pForm.ZipCodes = dt.Copy();
                }

                if (pForm.ShowDialog() == DialogResult.Yes)
                {
                    dt.Clear();

                    dt = pForm.ZipCodes;
                    ebZipCode.Text = "";
                    foreach (DataRow row in dt.Rows)
                    {
                        ebZipCode.Text += row["ZipCode"].ToString() + "^";
                    }

                    if (ebZipCode.Text.Length > 6) // ������ ���� '^' ����
                        ebZipCode.Text = ebZipCode.Text.Substring(0, ebZipCode.Text.Length - 1);
                }
            }
            catch (Exception)
            {
            }
            finally
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }

                if (pForm != null)
                {
                    pForm.Dispose();
                    pForm = null;
                }
            }
        }

        private void chkZip_CheckedChanged(object sender, System.EventArgs e)
        {
#if (DEBUG)
            Debug.WriteLine(MethodBase.GetCurrentMethod().Name);
#endif
            if (chkZip.Checked)
            {
                ebZipCode.Enabled = true;
                btnNewZip.Enabled = true;
                btnAddZip.Enabled = true;
                gbPost.Enabled = true;
            }
            else
            {
                ebZipCode.Enabled = false;
                btnNewZip.Enabled = false;
                btnAddZip.Enabled = false;
                gbPost.Enabled = false;
            }
        }

        private void pForm_SelectZipCode(object sender, AdManagerClient.Common.ZipCodeEventArgs e)
        {
#if (DEBUG)
            Debug.WriteLine(MethodBase.GetCurrentMethod().Name);
#endif
            ebZipCode.Text = e.ZipCode;
            lblZipName.Text = e.ZipAddr;
        }

        #endregion

        #region �����

        private void chkFreq_CheckedChanged(object sender, System.EventArgs e)
        {
#if (DEBUG)
            Debug.WriteLine(MethodBase.GetCurrentMethod().Name);
#endif
            if (chkFreq.Checked)
            {
                grpFreq.Enabled = true;
                freqDay.Value = 5;
                freqPeriod.Value = 10;
            }
            else
            {
                freqDay.Value = 0;
                freqPeriod.Value = 0;

                grpFreq.Enabled = false;
            }
        }

        #endregion

        #region �������� Ÿ���� �߰� �� �κ�[E_04]
        /// <summary>
        /// ���� Ÿ���� treeviewlist clear[E_04]
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
        /// üŷ�� ���� Ÿ���� ���ڿ� ��������[E_04]
        /// </summary>
        private string getTargetRegion()
        {
#if (DEBUG)
            Debug.WriteLine(MethodBase.GetCurrentMethod().Name);
#endif
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

                // ������ ������ ����ó��
                region = region.Substring(0, region.Length - 1);
            }
            catch (Exception ex)
            {
                Trace.Write(ex.Message);
            }

            return region;
        }


        /// <summary>
        /// ���� Ÿ���� ���� TreeListView�� üŷó��[E_04]
        /// </summary>		
        private void setTargetRegion(string[] splits)
        {
#if (DEBUG)
            Debug.WriteLine(MethodBase.GetCurrentMethod().Name);
#endif
            if (splits != null && splits.Length > 0)
            {
                TreeListViewItem level_2 = null;

                for (int h = 0; h < splits.Length; h++)  //�������� ���ڿ� �迭
                {
                    for (int i = 0; i < tlvRegion.Items[0].Items.Count; i++) // level 1(����,���...)
                    {
                        for (int j = 0; j < tlvRegion.Items[0].Items[i].Items.Count; j++) //level 2(�Ⱦ��,���ֽ�...,)
                        {
                            level_2 = tlvRegion.Items[0].Items[i].Items[j];

                            if (level_2.Items.Count > 0) // level 3�ܰ� ����üũ  
                            {
                                for (int k = 0; k < level_2.Items.Count; k++)  // û�ֽ��� ��(��籸,�����)..
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
        /// üŷ ������ ��Ʈ�� �ٽ� �˼�[E_04]
        /// </summary>
        private void reCheckingRegion()
        {
#if (DEBUG)
            Debug.WriteLine(MethodBase.GetCurrentMethod().Name);
#endif
            int nLevel_2 = 0;
            int nLevel_3 = 0;

            TreeListViewItem level_2 = null;
            // ���� �������� ��� ���� �Ǹ�
            // ���� �����۵� �Բ� üŷ�Ǿ�� �ϴµ�
            // �ڵ����� ���� �ʾƼ� �� �� �� �˼��ϴ� ����.
            for (int i = 0; i < tlvRegion.Items[0].Items.Count; i++) // level 1(����,���...)
            {
                nLevel_2 = 0;
                for (int j = 0; j < tlvRegion.Items[0].Items[i].Items.Count; j++) //level 2(�Ⱦ��,���ֽ�...,)
                {
                    level_2 = tlvRegion.Items[0].Items[i].Items[j];

                    if (level_2.Items.Count > 0) // level 3�ܰ� ����üũ  
                    {
                        nLevel_3 = 0;
                        for (int k = 0; k < level_2.Items.Count; k++)  // û�ֽ��� ��(��籸,�����)..
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

        #region �ð� Ÿ���� ���� �߰� �� �κ�[E_03]
        /// <summary>
        /// �ð� ������(���ڿ�)�м��ؼ� �ð� Ÿ�� radio��ư üŷ
        /// [E_03]
        /// </summary>
        private void setTimesBinding()
        {
#if (DEBUG)
            Debug.WriteLine(MethodBase.GetCurrentMethod().Name);
#endif
            int dayOfNum = 0;
            dayOfNum = getDayOfNum();

            if (strTimes != null && strTimes.Length > 3)
            {
                string[] weeks = Utility.SplitByString(strTimes, "-");

                if (weeks != null && weeks.Length > 1)
                    rbWeek.Checked = true;	// ����, �ָ� �и�			
                else
                {
                    if (strTimes.Substring(0, 1) == "d") // ����											
                        rbWeekDay.Checked = true;
                    else if (strTimes.Substring(0, 1) == "e") // �ָ�					
                        rbWeekEnd.Checked = true;
                    else
                    {
                        string[] splits = strTimes.Split('^');

                        //**���ſ� ��ϵ� ������ ó��**//
                        // ���Ϻ� üũ �׸��� �ִٸ� ����/�ָ� �����ؼ� ó���ϰ�
                        // ���ٸ� ȥ�� ���� ó�� �Ѵ�.
                        if (dayOfNum >= 1 && dayOfNum <= 15)
                            rbWeekDay.Checked = true; // ����						
                        else if (dayOfNum == 16 || dayOfNum == 17 || dayOfNum == 33)
                            rbWeekEnd.Checked = true; // �ָ�						
                        else
                            rbWeekAll.Checked = true; // ����, �ָ� ����(����,�ָ� ���� ���ٴ� �ǹ�)																																									
                    }
                }
            }
        }

        /// <summary>
        /// ���� Ÿ�ٰ� �ð�Ÿ���� ����/�ָ� üũ �� �˻�(true:�� Ÿ�ٰ��� ���� ����)
        /// [E_03]
        /// </summary>
        private bool checkingWeeknTime()
        {
#if (DEBUG)
            Debug.WriteLine(MethodBase.GetCurrentMethod().Name);
#endif
            int dayOfNum = 0;

            // ������ üũ�� ���¿���
            // �ð� Ÿ���� ���ϰ� �ð��� ����/�ָ� �ɼ� ������
            // �´��� üũ
            // 1. ����(������~�ݿ���) üũ ��  ���� ����
            //    (��Ʈ���� tag�� �� ���Ϻ� ����1~5(����) ,�����:16, �Ͽ���:17 �� �Է��� ����.
            // 2. 1~5:��� ���� �ּ�1~15�̸� ����
            //    16,17,33�̸� �ָ�
            // 3. �� �� ���� �������� ó��
            if (chkWeekYn.Checked)
            {
                foreach (Control ctrl in gbDays.Controls)
                {
                    if (ctrl is Janus.Windows.EditControls.UICheckBox)
                    {
                        Janus.Windows.EditControls.UICheckBox chk = (Janus.Windows.EditControls.UICheckBox)ctrl;
                        if (chk.Checked)
                            dayOfNum += Convert.ToInt32(chk.Tag);
                    }
                }

                if (chkTimeYn.Checked)
                {
                    if (dayOfNum >= 1 && dayOfNum <= 15)
                    {
                        // ����
                        if (rbWeekDay.Checked)
                            return true;
                        else
                            return false;
                    }
                    else if (dayOfNum == 16 || dayOfNum == 17 || dayOfNum == 33)
                    {
                        // �ָ�
                        if (rbWeekEnd.Checked)
                            return true;
                        else
                            return false;
                    }
                    else
                    {
                        // ����
                        return true;
                    }
                }
            }

            return true;
        }


        /// <summary>
        /// ������ üũ �� ���¿��� ����/�ָ� �� ���ڷ� ������[E_03]
        /// </summary>		
        private int getDayOfNum()
        {
#if (DEBUG)
            Debug.WriteLine(MethodBase.GetCurrentMethod().Name);
#endif
            int dayOfNum = 0;
            if (chkWeekYn.Checked)
            {
                foreach (Control ctrl in gbDays.Controls)
                {
                    if (ctrl is Janus.Windows.EditControls.UICheckBox)
                    {
                        Janus.Windows.EditControls.UICheckBox chk = (Janus.Windows.EditControls.UICheckBox)ctrl;
                        if (chk.Checked)
                            dayOfNum += Convert.ToInt32(chk.Tag);
                    }
                }
            }
            return dayOfNum;
        }


        /// <summary>
        /// Ÿ���� �� �ð� ���� �� ������
        /// </summary>		
        private void getTargetTimes(int dayOfNum)
        {
#if (DEBUG)
            Debug.WriteLine(MethodBase.GetCurrentMethod().Name);
#endif
            // ����,�ָ� ���� ����.
            TimeTarget tp = null;
            try
            {
                tp = new TimeTarget();
                tp.DaysOfNumber = dayOfNum;
                tp.TargetingTime = strTimes;

                tp.STypeNm = sTypeName;

                if (tp.ShowDialog() == DialogResult.Yes)
                {
                    strTimes = tp.TargetingTime;
                    Trace.WriteLine(strTimes);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("�ð����� ����:" + ex.Message, "�ð� Ÿ����", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }


        /// <summary>
        /// �ð� Ÿ���� ����,�ָ� ���� ���� ȣ��
        /// </summary>	
        private void rbWeekAll_Click(object sender, System.EventArgs e)
        {
#if (DEBUG)
            Debug.WriteLine(MethodBase.GetCurrentMethod().Name);
#endif
            // ���� Ÿ���ð� ��Ī�� �Ǿ�� �ϰ� �Ķ���� ������ -1 ����
            int num = getDayOfNum();

            if (num >= 1 && num <= 15)
            {
                MessageBox.Show("�ð� Ÿ�ٰ� ���� Ÿ�� ���� ��ġ ���� �ʽ��ϴ�!", "Ÿ����"
                    , MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else if (num == 16 || num == 17 || num == 33)
            {
                MessageBox.Show("�ð� Ÿ�ٰ� ���� Ÿ�� ���� ��ġ ���� �ʽ��ϴ�!", "Ÿ����"
                    , MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                getTargetTimes(-1);
            }
        }

        /// <summary>
        /// �ð� Ÿ���� ���߸� ȣ��
        /// </summary>		
        private void rbWeekDay_Click(object sender, System.EventArgs e)
        {
#if (DEBUG)
            Debug.WriteLine(MethodBase.GetCurrentMethod().Name);
#endif
            // ����
            int num = getDayOfNum();

            if (num == 0)  // ����Ÿ���� �������� ���� �ð��� �ǹ� �ϴ� ������ ����
                getTargetTimes(1);
            else
            {
                if (num >= 1 && num <= 15)
                    getTargetTimes(1);
                else
                {
                    MessageBox.Show("�ð� Ÿ�ٰ� ���� Ÿ�� ���� ��ġ ���� �ʽ��ϴ�!", "Ÿ����"
                        , MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        /// <summary>
        /// �ð� Ÿ���� �ָ��� ȣ��
        /// </summary>		
        private void rbWeekEnd_Click(object sender, System.EventArgs e)
        {
#if (DEBUG)
            Debug.WriteLine(MethodBase.GetCurrentMethod().Name);
#endif
            // �ָ�
            // ����
            int num = getDayOfNum();

            if (num == 0)  // ����Ÿ���� �������� �ָ� �ð��� �ǹ� �ϴ� ������ ����
                getTargetTimes(16);
            else
            {
                if (num == 16 || num == 17 || num == 33)
                    getTargetTimes(16);
                else
                {
                    MessageBox.Show("�ð� Ÿ�ٰ� ���� Ÿ�� ���� ��ġ ���� �ʽ��ϴ�!", "Ÿ����"
                        , MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        /// <summary>
        /// �ð� Ÿ���� ����/�ָ� �и� ȣ��
        /// </summary>		
        private void rbWeek_Click(object sender, System.EventArgs e)
        {
#if (DEBUG)
            Debug.WriteLine(MethodBase.GetCurrentMethod().Name);
#endif
            // ����, �ָ� �и�
            int num = getDayOfNum();
            if (num >= 1 && num <= 15)
            {
                MessageBox.Show("�ð� Ÿ�ٰ� ���� Ÿ�� ���� ��ġ ���� �ʽ��ϴ�!", "Ÿ����"
                    , MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else if (num == 16 || num == 17 || num == 33)
            {
                MessageBox.Show("�ð� Ÿ�ٰ� ���� Ÿ�� ���� ��ġ ���� �ʽ��ϴ�!", "Ÿ����"
                    , MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                getTargetTimes(0); // �Ķ���� ������ ���� ���� ���� �ƴϸ� ��(-1�� ����)
            }
        }
        #endregion

        /// <summary>
        /// Ÿ���� ���� ����
        /// ���� ������ �ƴϰ�, �������� ���� ��ư��.
        /// ���� �����۾��� �����ư����
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnModify_Click(object sender, EventArgs e)
        {
            DetailReadOnly(false);
            btnModify.Enabled = false;
            btnSave.Enabled = true;
            btnCancel.Enabled = true;
            grdExScheduleList.Enabled = false;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DetailReadOnly(true);
            btnModify.Enabled = true;
            btnSave.Enabled = false;
            grdExScheduleList.Enabled = true;
        }

        #region ����Ÿ���� ��ȸ �� ����
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
                    MessageBox.Show("������ ������ �����ϴ�.", "����Ÿ���� �߰�", MessageBoxButtons.OK, MessageBoxIcon.Information);
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


        private void btnAddCollection_Click(object sender, EventArgs e)
        {
            this.AddCollection("+");
        }

        private void btnAddCollectionMinus_Click(object sender, EventArgs e)
        {
            this.AddCollection("-");
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

        #region [E_10] �������� Ÿ���� ����

        /// <summary>
        /// [E_10] �������� Ÿ���� ����
        /// </summary>
        private void SetTargetingProfileAdd()
        {
            try
            {
                //���� ���� ���� �ʱ�ȭ ���ش�.
                targetingModel.Init();

                // �����͸𵨿� ������ ������ ��Ʈ�Ѵ�.
                targetingModel.ItemNo = keyItemNo;  // �����ȣ
                foreach (Janus.Windows.EditControls.UICheckBox cb in grbProfileAge.Controls)
                {
                    if (cb.Checked)
                        keyAge += cb.Tag + "^";
                    else
                        keyAge += "";
                }

                if (keyAge.Length > 0)        //���ɴ� ����
                    targetingModel.TgtProfile = keyAge.Substring(0, keyAge.Length - 1);
                else
                    targetingModel.TgtProfile = "";     // ���ɴ븦 �ϳ��� �������� �ʾ������ �� ��.

                targetingModel.TgtReliablilty = tbReliability.Value; // �ŷڵ�

                // Ÿ���� ������ ���� ���񽺸� ȣ���Ѵ�.
                new TargetingManager(systemModel, commonModel).SetTargetingProfileAdd(targetingModel);

                if (targetingModel.ResultCD.Equals("0000"))
                {
                    StatusMessage("�������� Ÿ���� ������ ����Ǿ����ϴ�.");

                    DisableButton();
                    ResetDetailText();
                    SearchTargeting();
                }
            }
            catch (FrameException fe)
            {
                FrameSystem.showMsgForm("�������� Ÿ���ó��� �������", new string[] { fe.ErrCode, fe.ResultMsg });
            }
            catch (Exception ex)
            {
                FrameSystem.showMsgForm("�������� Ÿ���ó��� �������", new string[] { "", ex.Message });
            }

            //Application.DoEvents();
        }

        #endregion

        /// <summary>
        /// ����ä�� ���� �ɼ� �����
        /// ����ä�� ����, ����ä�θ�
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void onPPxChanged(object sender, EventArgs e)
        {
            Janus.Windows.EditControls.UICheckBox checkBox = (Janus.Windows.EditControls.UICheckBox)sender;

            if (chkPPx.Checked)
            {
                if (chkPPxOnly.Checked)
                {
                    lbNotice.Text = "����ä�� ���� �ɼǰ� Only����ä�� ����ɼ��� �� �� ������ �� �����ϴ�.";
                    //�̺�Ʈ�� �߻��� �ش� üũ�ڽ��� ������ �������� �ʴ´�. - 2014/09/05 by Youngil.Yl ����
                    if (checkBox.Equals(chkPPxOnly)) chkPPx.Checked = false;
                    if (checkBox.Equals(chkPPx)) chkPPxOnly.Checked = false;
                }
                else
                {
                    lbNotice.Text = "����ä�� ���� �ɼ��� ���õǾ����ϴ�. ����� ���� �� ���� ä�ο� ���� �˴ϴ�.";
                }
            }
            else
            {
                if (chkPPxOnly.Checked)
                {
                    lbNotice.Text = "Only����ä�� �ɼ��� ���õǾ����ϴ�. ����� ����ä�ο��� ���������� ����˴ϴ�.";
                }
                else
                {
                    lbNotice.Text = "";
                }
            }
        }

    }
}