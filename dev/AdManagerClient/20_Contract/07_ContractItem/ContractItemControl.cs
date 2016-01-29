// ===============================================================================
// AdFileControl for Charites Project
//
// AdFileControl.cs
//
// ���������� ������� �����մϴ�. 
//
// ===============================================================================
// Release history
// 2007.09.05 RH.Jung 1.������ �κ� �ڵ�����
//                    2.���� �������� ����� ó��
// 2007.10.21 RH.Jung ������� ����ó���� ���ϻ��� ��ž����ó��
// ===============================================================================
// Copyright (C) 2006 G-Inno Systems Inc.
// All rights reserved.
// 

/*
 * -------------------------------------------------------
 * Class Name: ContractItemControl
 * �ֿ���  : ������ ���� ��Ʈ��
 * �ۼ���    : bae 
 * �ۼ���    : 2010.06.07
 * Ư�̻���  : 
 * -------------------------------------------------------
 * [��������]
 * -------------------------------------------------------
 * �����ڵ�  : [E_01]
 * ������    : bae
 * ������    : 2010.06.07
 * ��������  :           
 *            1.���� ���� �������� ���� ������� �ϰ����� ����߰�
 *             - �˾��޴� �߰�
 *             - �÷� üũ �ڽ� �߰�
 *            2.����ø��� �������� ���� ���ð����� ��� �߰�
 *             - �Է°� ����� ��ư �߰�(������ ��ϵ� ����ø����� 0 �� �� ��ư Ȱ��ȭ)
 *             - ���� ����ø�� ��ư �ڵ鷯 �ϳ��� ����-btnSetLinkChannel
 *             - ����ø�� ��ư�� Tag �Ӽ��� �̿��ؼ� ��ư Ŭ�� ����
 * --------------------------------------------------------
 * �����ڵ�  : [E_02]
 * ������    : bae
 * ������    : 2010.09.14
 * �����κ�  : 
 *            - SaveContractItemDetail()
 * ��������  :
 *            - contractItem �� CugYn �� üũ�ϴ� �κ� �߰�
 * ---------------------------------------------------------
 * �����ڵ�  : [E_03]
 * ������    : bae
 * ������    : 2010.10.04
 * �����κ�  : 
 *            - SaveContractItemDetail()
 * ��������  :
 *            - ����ð� �ʼ� ������ üũ
 * ---------------------------------------------------------
 */


/*
 * -------------------------------------------------------
 * [��������]
 * -------------------------------------------------------
 * �����ڵ�  : [E_04]
 * ������    : Youngil.Yi
 * ������    : 2015.05.13
 * ��������  :           
 *            1.����ø���(������) �߰�
 *             - ����ø���(������) �� �߰�
 *             - ���� ä�� ��ư �Է�â üũ �ڽ� �� ��Ʈ�� �߰�
 *            2.����ø��� ������ Ÿ�Կ� ���� �Է� ���� ���� ��� �߰�
 *             - ���� ä�� �Է½� ��ũŸ��(1:����ø���(����Ⱥ���),2:����ø���(������)) ���� �Է�/����/���� ��� �߰�
 * --------------------------------------------------------
 * 
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

using Janus.Windows.GridEX;
using System.Configuration;
using System.Reflection;
using AdManagerClient.Common.Args;

namespace AdManagerClient
{
    /// <summary>
    /// ���������� ��Ʈ��
    /// </summary>
    public class ContractItemControl : System.Windows.Forms.UserControl, IUserControl
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
		ContractItemModel	contractItemModel	= new ContractItemModel();	// ��������
		AdFileWideModel		adFileWideModel		= new AdFileWideModel();	// �������ϸ�

        // ȭ��ó���� ����
        bool IsNewSearchKey		  = true;					// �˻����Է� ����
        CurrencyManager cm        = null;					// ������ �׸����� ���濡 ���� �����ͼ� ������ ���Ͽ�			
        CurrencyManager cmDetail        = null;					// ������ �׸����� ���濡 ���� �����ͼ� ������ ���Ͽ�			
        DataTable       dt        = null;
        DataTable       dtDetail  = null;

        bool IsNotLoading		  = true;					// ����ȸ���� �ƴ�

        // ��ȸ�� ó�� : ��ȸ�� ��ȭ���� ������Ʈ �Ǵ� ���� ���� �ϱ����� 2011.11.28 RH.Jung
        bool IsSearching = false;

        bool IsAdding             = false;
        bool canRead			  = false;
        bool canUpdate			  = false;
        bool canCreate            = false;
        bool canDelete            = false;

        string MediaCode      = "";
        string RapCode        = "";
        string AgencyCode     = "";
        string AdvertiserCode = "";

		string AdState        = "";	// ��������ڵ�
        string AdType = "";	// ���������ڵ�
        
        // ��Key

        string keyItemNo   = "";
        string keySchType = "";
        string keyAdverNo = "";
        int    keyLinkType = 0;	// ��ũŸ���ڵ�
        private Janus.Windows.EditControls.UICheckBox chkAdState_10;
        private Janus.Windows.EditControls.UICheckBox chkAdState_20;
        private Janus.Windows.EditControls.UICheckBox chkAdState_30;
        private Janus.Windows.EditControls.UICheckBox chkAdState_40;
        private Janus.Windows.EditControls.UIRadioButton rbSch_All;
        private Janus.Windows.EditControls.UIRadioButton rbSch_Y;
        private Janus.Windows.EditControls.UIRadioButton rbSch_N;
        private Label lbScheduleType;
        private Janus.Windows.EditControls.UIComboBox cbScheduleType;
        private Label lblMsg;
		private ToolTip toolTip1;
		private Label lblError;
        private Janus.Windows.UI.Tab.UITabPage uiTabPage1;
		private Janus.Windows.UI.Tab.UITabPage tabBanner;
		private Janus.Windows.EditControls.UIGroupBox gbLinkType;
		private Janus.Windows.GridEX.EditControls.EditBox ebLinkContent;
		private Janus.Windows.EditControls.UIRadioButton rbLink2;
		private Janus.Windows.EditControls.UIRadioButton rbLink1;
		private Janus.Windows.EditControls.UIRadioButton rbLink0;
		private Janus.Windows.EditControls.UIGroupBox gbViewLoc;
		private Janus.Windows.EditControls.UIRadioButton rbView6;
		private Janus.Windows.EditControls.UIRadioButton rbView5;
		private Janus.Windows.EditControls.UIRadioButton rbView4;
		private Janus.Windows.EditControls.UIRadioButton rbView3;
		private Janus.Windows.EditControls.UIRadioButton rbView2;
		private Janus.Windows.EditControls.UIRadioButton rbView1;
		private Janus.Windows.EditControls.UIGroupBox uiGroupBox1;
		private Janus.Windows.EditControls.UIComboBox cbBannerOrd;
		private Label label15;
		private Janus.Windows.EditControls.UICheckBox chkIPad;
		private Janus.Windows.EditControls.UICheckBox chkAndroidTab;
		private Janus.Windows.EditControls.UICheckBox chkIphone;
		private Janus.Windows.EditControls.UICheckBox chkAndroid;
		private Janus.Windows.EditControls.UIGroupBox gbBanner;
		private PictureBox picAdFile;
		private Janus.Windows.EditControls.UIButton btnBannerFile;
		private Label label16;
		private Janus.Windows.GridEX.EditControls.EditBox ebBannerFileNm;
		string rowItemNo   = "";
		//string linkChannelNo   = "";		



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


		/// <summary>
		/// �������:����(40)
		/// </summary>
		private string AD_STATE_DEL = string.Empty;

		#region ��ȭ:�߰��׸�
		/// <summary>
		/// ��Ƽ ���õ� ������ ������� ����� �޺��ڽ�
		/// </summary>
		private Janus.Windows.EditControls.UIComboBox cbMultiAdState = null;
		#endregion

        #region ȭ�� ������Ʈ, ������, �Ҹ���

        private Janus.Windows.UI.Dock.UIPanelManager uiPM;
        private System.Windows.Forms.Panel pnlSearch;
        private Janus.Windows.EditControls.UIButton btnSearch;
        private System.Windows.Forms.Panel pnlUserDetail;
        private Janus.Windows.UI.Dock.UIPanel uiPanelSearch;
        private Janus.Windows.UI.Dock.UIPanelInnerContainer uiPanelSearchContainer;
        private Janus.Windows.UI.Dock.UIPanel uiPanelList;
        private Janus.Windows.UI.Dock.UIPanelInnerContainer uiPanelListContainer;
        private Janus.Windows.UI.Dock.UIPanel uiPanelDetail;
        private Janus.Windows.UI.Dock.UIPanelInnerContainer uiPanelDetailContainer;
        private Janus.Windows.UI.Dock.UIPanelGroup uiPanelContract;
        private Janus.Windows.GridEX.EditControls.EditBox ebSearchKey;
        private System.Windows.Forms.Panel panel1;
        private Janus.Windows.GridEX.GridEX gridEX1;
        private Janus.Windows.EditControls.UIButton btnDelete;
        private Janus.Windows.EditControls.UIButton btnAdd;
        private Janus.Windows.EditControls.UIButton btnSave;
        private Janus.Windows.GridEX.GridEX grdExItemList;
        private Janus.Windows.EditControls.UIComboBox cbSearchMedia;
        private Janus.Windows.EditControls.UIComboBox cbSearchRap;
        private Janus.Windows.EditControls.UIComboBox cbSearchAgency;
        private Janus.Windows.EditControls.UIComboBox cbSearchAdvertiser;
        private System.Data.DataView dvContractItem;
        private System.Windows.Forms.Label lbContModDt;
        private System.Windows.Forms.Label lbContRegName;
        private System.Windows.Forms.Label lbContRegDt;
        private System.Windows.Forms.Label lbAgency;
        private System.Windows.Forms.Label lbContractState;
        private System.Windows.Forms.Label lbContStartDay;
        private System.Windows.Forms.Label lbContractName2;
        private System.Windows.Forms.Label lbContEndDay;
        private System.Windows.Forms.Label lbMedia;
        private System.Windows.Forms.Label lbRap;
        private System.Windows.Forms.Label lbComment;
        private Janus.Windows.UI.Tab.UITabPage tabPageItem;
        private System.Windows.Forms.Label lbExcuteStartDay;
        private Janus.Windows.EditControls.UIComboBox cbAdState;
        private System.Windows.Forms.Label lbRealEndDay;
        private System.Windows.Forms.Label lbItemName;
        private Janus.Windows.GridEX.EditControls.EditBox ebItemName;
        private System.Windows.Forms.Label lbAdRate;
        private System.Windows.Forms.Label lbAdState;
        private Janus.Windows.EditControls.UIComboBox cbAdClass;
        private System.Windows.Forms.Label lbAdClass;
        private System.Windows.Forms.Label lbModDt;
        private Janus.Windows.GridEX.EditControls.EditBox ebRegDt;
        private System.Windows.Forms.Label lbRegName;
        private Janus.Windows.GridEX.EditControls.EditBox ebModDt;
        private System.Windows.Forms.Label lbAdType;
        private Janus.Windows.EditControls.UIComboBox cbAdType;
        private System.Windows.Forms.Label lbExcuteEndDay;
        private System.Windows.Forms.Label lbAdTime;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label lbRegDt;
		private Janus.Windows.GridEX.EditControls.EditBox ebRegName;
        private Janus.Windows.EditControls.UIButton btnSearchContract;
        private Janus.Windows.UI.Tab.UITabPage tabPageFile;
        private Janus.Windows.GridEX.EditControls.EditBox ebFileRegDt;
        private System.Windows.Forms.Label lbFileRegName;
        private System.Windows.Forms.Label lbFileRegDt;
        private Janus.Windows.GridEX.EditControls.EditBox ebFileRegName;
        private Janus.Windows.GridEX.EditControls.EditBox ebFileLength;
        private Janus.Windows.GridEX.EditControls.EditBox ebDownLevel;
        private Janus.Windows.GridEX.EditControls.EditBox ebFileType;
        private Janus.Windows.GridEX.EditControls.EditBox ebFileName;
        private System.Windows.Forms.Label lbFileState;
        private System.Windows.Forms.Label lbFileName;
        private System.Windows.Forms.Label lbFileType;
        private System.Windows.Forms.Label lbFileLength;
        private System.Windows.Forms.Label lbDownLevel;
        private System.Windows.Forms.Label lbFilePath;
        private Janus.Windows.GridEX.EditControls.EditBox ebFilePath;
        private Janus.Windows.GridEX.EditControls.EditBox ebFileState;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label12;
        private Janus.Windows.UI.Tab.UITabPage tabPageHistory;
        private System.Windows.Forms.Label lbItemName2;
        private System.Windows.Forms.Panel pnlHistory;
        private System.Windows.Forms.Label lbAdClass2;
        private System.Windows.Forms.Label lbScheduleType2;
        private Janus.Windows.GridEX.EditControls.EditBox ebExcuteEndDay2;
        private Janus.Windows.GridEX.EditControls.EditBox ebScheduleType2;
        private System.Windows.Forms.Label lbAdState2;
        private System.Windows.Forms.Label lbAdRate2;
        private Janus.Windows.GridEX.EditControls.EditBox ebAdState2;
        private System.Windows.Forms.Label lbExcuteStartDay2;
        private Janus.Windows.GridEX.EditControls.EditBox ebExcuteStartDay2;
        private Janus.Windows.GridEX.EditControls.EditBox ebAdTime2;
        private Janus.Windows.GridEX.EditControls.EditBox ebAdClass2;
        private Janus.Windows.GridEX.EditControls.EditBox ebAdType2;
        private Janus.Windows.GridEX.EditControls.EditBox ebItemName2;
        private System.Windows.Forms.Label lbExcuteEndDay2;
        private System.Windows.Forms.Label lbAdType2;
        private System.Windows.Forms.Label lbAdTime2;        
        private Janus.Windows.GridEX.EditControls.EditBox ebContEndDay;
        private Janus.Windows.GridEX.EditControls.EditBox ebContStartDay;
        private Janus.Windows.GridEX.EditControls.EditBox ebContractState;
        private Janus.Windows.GridEX.EditControls.EditBox ebComment;
        private Janus.Windows.GridEX.EditControls.EditBox ebRap;
        private Janus.Windows.GridEX.EditControls.EditBox ebAgency;
        private Janus.Windows.GridEX.EditControls.EditBox ebMedia;
        private Janus.Windows.GridEX.EditControls.EditBox ebContractRegDt;
        private Janus.Windows.GridEX.EditControls.EditBox ebContractModDt;
        private Janus.Windows.GridEX.EditControls.EditBox ebContractRegName;
        private Janus.Windows.UI.Tab.UITabPage tabPageContract;
        private Janus.Windows.GridEX.EditControls.EditBox ebContractName2;
        private Janus.Windows.GridEX.EditControls.EditBox ebContractName;
        private Janus.Windows.GridEX.EditControls.EditBox ebContractSeq;
        private System.Data.DataView dvContractHistory;
        private Janus.Windows.GridEX.GridEX grdExContactHistory;
        private System.Windows.Forms.Label label13;
        private Janus.Windows.EditControls.UIComboBox cbAdRate;
        private Janus.Windows.GridEX.EditControls.EditBox ebAdRate2;
		private Janus.Windows.CalendarCombo.CalendarCombo meExcuteStartDay;
		private Janus.Windows.CalendarCombo.CalendarCombo meExcuteEndDay;
		private Janus.Windows.CalendarCombo.CalendarCombo meRealEndDay;
		private Janus.Windows.EditControls.UIComboBox cbSearchAdType;
        private System.Windows.Forms.Label label14;
		private Janus.Windows.GridEX.EditControls.NumericEditBox ebAdTime;
		private Janus.Windows.UI.Tab.UITab tabContractItem;
		private System.Windows.Forms.OpenFileDialog openFileDlg;
        private System.Windows.Forms.SaveFileDialog saveFileDlg;
		private Janus.Windows.EditControls.UIButton btnCopy;
		private Janus.Windows.UI.CommandBars.UICommandManager cmdManager;
		private Janus.Windows.UI.CommandBars.UIContextMenu ctxMenu;
		private Janus.Windows.UI.CommandBars.UICommand SetAdState;
		private Janus.Windows.UI.CommandBars.UICommand SetAdState1;
		private Janus.Windows.UI.CommandBars.UIRebar TopRebar1;
		private Janus.Windows.UI.CommandBars.UIRebar BottomRebar1;
		private Janus.Windows.UI.CommandBars.UIRebar LeftRebar1;
        private Janus.Windows.UI.CommandBars.UIRebar RightRebar1;
        private AdManagerClient.ContractItemDs contractItemDs;		
        private System.ComponentModel.IContainer components;

        public ContractItemControl()
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
			Janus.Windows.GridEX.GridEXLayout grdExItemList_DesignTimeLayout = new Janus.Windows.GridEX.GridEXLayout();
			Janus.Windows.Common.Layouts.JanusLayoutReference grdExItemList_DesignTimeLayout_Reference_0 = new Janus.Windows.Common.Layouts.JanusLayoutReference("GridEXLayoutData.RootTable.FormatConditions.Condition5.FormatStyle.BackgroundImag" +
        "e");
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ContractItemControl));
			Janus.Windows.EditControls.UIComboBoxItem uiComboBoxItem1 = new Janus.Windows.EditControls.UIComboBoxItem();
			Janus.Windows.EditControls.UIComboBoxItem uiComboBoxItem2 = new Janus.Windows.EditControls.UIComboBoxItem();
			Janus.Windows.EditControls.UIComboBoxItem uiComboBoxItem3 = new Janus.Windows.EditControls.UIComboBoxItem();
			Janus.Windows.EditControls.UIComboBoxItem uiComboBoxItem4 = new Janus.Windows.EditControls.UIComboBoxItem();
			Janus.Windows.GridEX.GridEXLayout grdExContactHistory_DesignTimeLayout = new Janus.Windows.GridEX.GridEXLayout();
			Janus.Windows.EditControls.UIComboBoxItem uiComboBoxItem5 = new Janus.Windows.EditControls.UIComboBoxItem();
			Janus.Windows.EditControls.UIComboBoxItem uiComboBoxItem6 = new Janus.Windows.EditControls.UIComboBoxItem();
			Janus.Windows.EditControls.UIComboBoxItem uiComboBoxItem7 = new Janus.Windows.EditControls.UIComboBoxItem();
			Janus.Windows.EditControls.UIComboBoxItem uiComboBoxItem8 = new Janus.Windows.EditControls.UIComboBoxItem();
			Janus.Windows.EditControls.UIComboBoxItem uiComboBoxItem9 = new Janus.Windows.EditControls.UIComboBoxItem();
			Janus.Windows.EditControls.UIComboBoxItem uiComboBoxItem10 = new Janus.Windows.EditControls.UIComboBoxItem();
			Janus.Windows.EditControls.UIComboBoxItem uiComboBoxItem11 = new Janus.Windows.EditControls.UIComboBoxItem();
			Janus.Windows.EditControls.UIComboBoxItem uiComboBoxItem12 = new Janus.Windows.EditControls.UIComboBoxItem();
			Janus.Windows.EditControls.UIComboBoxItem uiComboBoxItem13 = new Janus.Windows.EditControls.UIComboBoxItem();
			this.uiPM = new Janus.Windows.UI.Dock.UIPanelManager(this.components);
			this.uiPanelContract = new Janus.Windows.UI.Dock.UIPanelGroup();
			this.uiPanelSearch = new Janus.Windows.UI.Dock.UIPanel();
			this.uiPanelSearchContainer = new Janus.Windows.UI.Dock.UIPanelInnerContainer();
			this.pnlSearch = new System.Windows.Forms.Panel();
			this.rbSch_N = new Janus.Windows.EditControls.UIRadioButton();
			this.rbSch_Y = new Janus.Windows.EditControls.UIRadioButton();
			this.rbSch_All = new Janus.Windows.EditControls.UIRadioButton();
			this.chkAdState_40 = new Janus.Windows.EditControls.UICheckBox();
			this.chkAdState_30 = new Janus.Windows.EditControls.UICheckBox();
			this.chkAdState_20 = new Janus.Windows.EditControls.UICheckBox();
			this.chkAdState_10 = new Janus.Windows.EditControls.UICheckBox();
			this.label13 = new System.Windows.Forms.Label();
			this.cbSearchMedia = new Janus.Windows.EditControls.UIComboBox();
			this.cbSearchRap = new Janus.Windows.EditControls.UIComboBox();
			this.cbSearchAgency = new Janus.Windows.EditControls.UIComboBox();
			this.cbSearchAdvertiser = new Janus.Windows.EditControls.UIComboBox();
			this.ebSearchKey = new Janus.Windows.GridEX.EditControls.EditBox();
			this.cbSearchAdType = new Janus.Windows.EditControls.UIComboBox();
			this.btnSearch = new Janus.Windows.EditControls.UIButton();
			this.label14 = new System.Windows.Forms.Label();
			this.uiPanelList = new Janus.Windows.UI.Dock.UIPanel();
			this.uiPanelListContainer = new Janus.Windows.UI.Dock.UIPanelInnerContainer();
			this.grdExItemList = new Janus.Windows.GridEX.GridEX();
			this.dvContractItem = new System.Data.DataView();
			this.contractItemDs = new AdManagerClient.ContractItemDs();
			this.uiPanelDetail = new Janus.Windows.UI.Dock.UIPanel();
			this.uiPanelDetailContainer = new Janus.Windows.UI.Dock.UIPanelInnerContainer();
			this.pnlUserDetail = new System.Windows.Forms.Panel();
			this.lblError = new System.Windows.Forms.Label();
			this.btnCopy = new Janus.Windows.EditControls.UIButton();
			this.tabContractItem = new Janus.Windows.UI.Tab.UITab();
			this.tabPageItem = new Janus.Windows.UI.Tab.UITabPage();
			this.lblMsg = new System.Windows.Forms.Label();
			this.cbScheduleType = new Janus.Windows.EditControls.UIComboBox();
			this.lbScheduleType = new System.Windows.Forms.Label();
			this.ebAdTime = new Janus.Windows.GridEX.EditControls.NumericEditBox();
			this.meRealEndDay = new Janus.Windows.CalendarCombo.CalendarCombo();
			this.meExcuteEndDay = new Janus.Windows.CalendarCombo.CalendarCombo();
			this.meExcuteStartDay = new Janus.Windows.CalendarCombo.CalendarCombo();
			this.ebContractSeq = new Janus.Windows.GridEX.EditControls.EditBox();
			this.ebContractName = new Janus.Windows.GridEX.EditControls.EditBox();
			this.lbExcuteStartDay = new System.Windows.Forms.Label();
			this.cbAdState = new Janus.Windows.EditControls.UIComboBox();
			this.lbRealEndDay = new System.Windows.Forms.Label();
			this.lbItemName = new System.Windows.Forms.Label();
			this.ebItemName = new Janus.Windows.GridEX.EditControls.EditBox();
			this.lbAdRate = new System.Windows.Forms.Label();
			this.lbAdState = new System.Windows.Forms.Label();
			this.cbAdClass = new Janus.Windows.EditControls.UIComboBox();
			this.lbAdClass = new System.Windows.Forms.Label();
			this.lbModDt = new System.Windows.Forms.Label();
			this.ebRegDt = new Janus.Windows.GridEX.EditControls.EditBox();
			this.lbRegName = new System.Windows.Forms.Label();
			this.ebModDt = new Janus.Windows.GridEX.EditControls.EditBox();
			this.lbAdType = new System.Windows.Forms.Label();
			this.cbAdType = new Janus.Windows.EditControls.UIComboBox();
			this.lbExcuteEndDay = new System.Windows.Forms.Label();
			this.lbAdTime = new System.Windows.Forms.Label();
			this.label10 = new System.Windows.Forms.Label();
			this.lbRegDt = new System.Windows.Forms.Label();
			this.ebRegName = new Janus.Windows.GridEX.EditControls.EditBox();
			this.btnSearchContract = new Janus.Windows.EditControls.UIButton();
			this.cbAdRate = new Janus.Windows.EditControls.UIComboBox();
			this.tabPageFile = new Janus.Windows.UI.Tab.UITabPage();
			this.ebFileRegDt = new Janus.Windows.GridEX.EditControls.EditBox();
			this.lbFileRegName = new System.Windows.Forms.Label();
			this.lbFileRegDt = new System.Windows.Forms.Label();
			this.ebFileRegName = new Janus.Windows.GridEX.EditControls.EditBox();
			this.ebFileLength = new Janus.Windows.GridEX.EditControls.EditBox();
			this.ebDownLevel = new Janus.Windows.GridEX.EditControls.EditBox();
			this.ebFileType = new Janus.Windows.GridEX.EditControls.EditBox();
			this.ebFileName = new Janus.Windows.GridEX.EditControls.EditBox();
			this.lbFileState = new System.Windows.Forms.Label();
			this.lbFileName = new System.Windows.Forms.Label();
			this.lbFileType = new System.Windows.Forms.Label();
			this.lbFileLength = new System.Windows.Forms.Label();
			this.lbDownLevel = new System.Windows.Forms.Label();
			this.lbFilePath = new System.Windows.Forms.Label();
			this.ebFilePath = new Janus.Windows.GridEX.EditControls.EditBox();
			this.ebFileState = new Janus.Windows.GridEX.EditControls.EditBox();
			this.tabPageContract = new Janus.Windows.UI.Tab.UITabPage();
			this.ebContractRegDt = new Janus.Windows.GridEX.EditControls.EditBox();
			this.ebContractModDt = new Janus.Windows.GridEX.EditControls.EditBox();
			this.ebContractRegName = new Janus.Windows.GridEX.EditControls.EditBox();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.ebComment = new Janus.Windows.GridEX.EditControls.EditBox();
			this.label4 = new System.Windows.Forms.Label();
			this.ebContEndDay = new Janus.Windows.GridEX.EditControls.EditBox();
			this.ebContractName2 = new Janus.Windows.GridEX.EditControls.EditBox();
			this.label5 = new System.Windows.Forms.Label();
			this.ebContStartDay = new Janus.Windows.GridEX.EditControls.EditBox();
			this.ebRap = new Janus.Windows.GridEX.EditControls.EditBox();
			this.ebAgency = new Janus.Windows.GridEX.EditControls.EditBox();
			this.label6 = new System.Windows.Forms.Label();
			this.label7 = new System.Windows.Forms.Label();
			this.label8 = new System.Windows.Forms.Label();
			this.ebMedia = new Janus.Windows.GridEX.EditControls.EditBox();
			this.label9 = new System.Windows.Forms.Label();
			this.ebContractState = new Janus.Windows.GridEX.EditControls.EditBox();
			this.label11 = new System.Windows.Forms.Label();
			this.label12 = new System.Windows.Forms.Label();
			this.tabPageHistory = new Janus.Windows.UI.Tab.UITabPage();
			this.ebAdRate2 = new Janus.Windows.GridEX.EditControls.EditBox();
			this.lbItemName2 = new System.Windows.Forms.Label();
			this.pnlHistory = new System.Windows.Forms.Panel();
			this.grdExContactHistory = new Janus.Windows.GridEX.GridEX();
			this.dvContractHistory = new System.Data.DataView();
			this.lbAdClass2 = new System.Windows.Forms.Label();
			this.lbScheduleType2 = new System.Windows.Forms.Label();
			this.ebExcuteEndDay2 = new Janus.Windows.GridEX.EditControls.EditBox();
			this.ebScheduleType2 = new Janus.Windows.GridEX.EditControls.EditBox();
			this.lbAdState2 = new System.Windows.Forms.Label();
			this.lbAdRate2 = new System.Windows.Forms.Label();
			this.ebAdState2 = new Janus.Windows.GridEX.EditControls.EditBox();
			this.lbExcuteStartDay2 = new System.Windows.Forms.Label();
			this.ebExcuteStartDay2 = new Janus.Windows.GridEX.EditControls.EditBox();
			this.ebAdTime2 = new Janus.Windows.GridEX.EditControls.EditBox();
			this.ebAdClass2 = new Janus.Windows.GridEX.EditControls.EditBox();
			this.ebAdType2 = new Janus.Windows.GridEX.EditControls.EditBox();
			this.ebItemName2 = new Janus.Windows.GridEX.EditControls.EditBox();
			this.lbExcuteEndDay2 = new System.Windows.Forms.Label();
			this.lbAdType2 = new System.Windows.Forms.Label();
			this.lbAdTime2 = new System.Windows.Forms.Label();
			this.btnDelete = new Janus.Windows.EditControls.UIButton();
			this.btnAdd = new Janus.Windows.EditControls.UIButton();
			this.btnSave = new Janus.Windows.EditControls.UIButton();
			this.panel1 = new System.Windows.Forms.Panel();
			this.gridEX1 = new Janus.Windows.GridEX.GridEX();
			this.lbContModDt = new System.Windows.Forms.Label();
			this.lbContRegName = new System.Windows.Forms.Label();
			this.lbContRegDt = new System.Windows.Forms.Label();
			this.lbAgency = new System.Windows.Forms.Label();
			this.lbContractState = new System.Windows.Forms.Label();
			this.lbContStartDay = new System.Windows.Forms.Label();
			this.lbContractName2 = new System.Windows.Forms.Label();
			this.lbContEndDay = new System.Windows.Forms.Label();
			this.lbMedia = new System.Windows.Forms.Label();
			this.lbRap = new System.Windows.Forms.Label();
			this.lbComment = new System.Windows.Forms.Label();
			this.openFileDlg = new System.Windows.Forms.OpenFileDialog();
			this.saveFileDlg = new System.Windows.Forms.SaveFileDialog();
			this.cmdManager = new Janus.Windows.UI.CommandBars.UICommandManager(this.components);
			this.BottomRebar1 = new Janus.Windows.UI.CommandBars.UIRebar();
			this.SetAdState = new Janus.Windows.UI.CommandBars.UICommand("SetAdState");
			this.ctxMenu = new Janus.Windows.UI.CommandBars.UIContextMenu();
			this.SetAdState1 = new Janus.Windows.UI.CommandBars.UICommand("SetAdState");
			this.LeftRebar1 = new Janus.Windows.UI.CommandBars.UIRebar();
			this.RightRebar1 = new Janus.Windows.UI.CommandBars.UIRebar();
			this.TopRebar1 = new Janus.Windows.UI.CommandBars.UIRebar();
			this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
			this.uiTabPage1 = new Janus.Windows.UI.Tab.UITabPage();
			this.tabBanner = new Janus.Windows.UI.Tab.UITabPage();
			this.rbLink0 = new Janus.Windows.EditControls.UIRadioButton();
			this.gbLinkType = new Janus.Windows.EditControls.UIGroupBox();
			this.rbLink1 = new Janus.Windows.EditControls.UIRadioButton();
			this.rbLink2 = new Janus.Windows.EditControls.UIRadioButton();
			this.ebLinkContent = new Janus.Windows.GridEX.EditControls.EditBox();
			this.gbViewLoc = new Janus.Windows.EditControls.UIGroupBox();
			this.rbView3 = new Janus.Windows.EditControls.UIRadioButton();
			this.rbView2 = new Janus.Windows.EditControls.UIRadioButton();
			this.rbView1 = new Janus.Windows.EditControls.UIRadioButton();
			this.rbView6 = new Janus.Windows.EditControls.UIRadioButton();
			this.rbView5 = new Janus.Windows.EditControls.UIRadioButton();
			this.rbView4 = new Janus.Windows.EditControls.UIRadioButton();
			this.label15 = new System.Windows.Forms.Label();
			this.cbBannerOrd = new Janus.Windows.EditControls.UIComboBox();
			this.uiGroupBox1 = new Janus.Windows.EditControls.UIGroupBox();
			this.chkAndroid = new Janus.Windows.EditControls.UICheckBox();
			this.chkIphone = new Janus.Windows.EditControls.UICheckBox();
			this.chkIPad = new Janus.Windows.EditControls.UICheckBox();
			this.chkAndroidTab = new Janus.Windows.EditControls.UICheckBox();
			this.gbBanner = new Janus.Windows.EditControls.UIGroupBox();
			this.ebBannerFileNm = new Janus.Windows.GridEX.EditControls.EditBox();
			this.label16 = new System.Windows.Forms.Label();
			this.btnBannerFile = new Janus.Windows.EditControls.UIButton();
			this.picAdFile = new System.Windows.Forms.PictureBox();
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
			((System.ComponentModel.ISupportInitialize)(this.grdExItemList)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.dvContractItem)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.contractItemDs)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.uiPanelDetail)).BeginInit();
			this.uiPanelDetail.SuspendLayout();
			this.uiPanelDetailContainer.SuspendLayout();
			this.pnlUserDetail.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.tabContractItem)).BeginInit();
			this.tabContractItem.SuspendLayout();
			this.tabPageItem.SuspendLayout();
			this.tabPageFile.SuspendLayout();
			this.tabPageContract.SuspendLayout();
			this.tabPageHistory.SuspendLayout();
			this.pnlHistory.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.grdExContactHistory)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.dvContractHistory)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.gridEX1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.cmdManager)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.BottomRebar1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.ctxMenu)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.LeftRebar1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.RightRebar1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.TopRebar1)).BeginInit();
			this.tabBanner.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.gbLinkType)).BeginInit();
			this.gbLinkType.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.gbViewLoc)).BeginInit();
			this.gbViewLoc.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.uiGroupBox1)).BeginInit();
			this.uiGroupBox1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.gbBanner)).BeginInit();
			this.gbBanner.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.picAdFile)).BeginInit();
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
			this.uiPanelContract.Id = new System.Guid("b162c53a-7940-47cd-a0ed-9762c59c50d8");
			this.uiPanelContract.StaticGroup = true;
			this.uiPanelSearch.Id = new System.Guid("afb3b7d0-aa79-443c-a1e8-3baba38f7a4d");
			this.uiPanelContract.Panels.Add(this.uiPanelSearch);
			this.uiPanelList.Id = new System.Guid("8e3140b9-bfcb-43af-b84e-2ee6cddf51dc");
			this.uiPanelContract.Panels.Add(this.uiPanelList);
			this.uiPanelDetail.Id = new System.Guid("b2973a57-d3e1-4d80-9387-7a6dc3332542");
			this.uiPanelContract.Panels.Add(this.uiPanelDetail);
			this.uiPM.Panels.Add(this.uiPanelContract);
			// 
			// Design Time Panel Info:
			// 
			this.uiPM.BeginPanelInfo();
			this.uiPM.AddDockPanelInfo(new System.Guid("b162c53a-7940-47cd-a0ed-9762c59c50d8"), Janus.Windows.UI.Dock.PanelGroupStyle.HorizontalTiles, Janus.Windows.UI.Dock.PanelDockStyle.Fill, true, new System.Drawing.Size(1010, 677), true);
			this.uiPM.AddDockPanelInfo(new System.Guid("afb3b7d0-aa79-443c-a1e8-3baba38f7a4d"), new System.Guid("b162c53a-7940-47cd-a0ed-9762c59c50d8"), 66, true);
			this.uiPM.AddDockPanelInfo(new System.Guid("8e3140b9-bfcb-43af-b84e-2ee6cddf51dc"), new System.Guid("b162c53a-7940-47cd-a0ed-9762c59c50d8"), 318, true);
			this.uiPM.AddDockPanelInfo(new System.Guid("b2973a57-d3e1-4d80-9387-7a6dc3332542"), new System.Guid("b162c53a-7940-47cd-a0ed-9762c59c50d8"), 263, true);
			this.uiPM.AddFloatingPanelInfo(new System.Guid("b162c53a-7940-47cd-a0ed-9762c59c50d8"), Janus.Windows.UI.Dock.PanelGroupStyle.HorizontalTiles, true, new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
			this.uiPM.AddFloatingPanelInfo(new System.Guid("8e3140b9-bfcb-43af-b84e-2ee6cddf51dc"), new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
			this.uiPM.AddFloatingPanelInfo(new System.Guid("b2973a57-d3e1-4d80-9387-7a6dc3332542"), new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
			this.uiPM.AddFloatingPanelInfo(new System.Guid("afb3b7d0-aa79-443c-a1e8-3baba38f7a4d"), new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
			this.uiPM.EndPanelInfo();
			// 
			// uiPanelContract
			// 
			this.uiPanelContract.ActiveCaptionMode = Janus.Windows.UI.Dock.ActiveCaptionMode.Never;
			this.uiPanelContract.BackColor = System.Drawing.SystemColors.Window;
			this.uiPanelContract.CaptionVisible = Janus.Windows.UI.InheritableBoolean.True;
			this.uiPanelContract.CloseButtonVisible = Janus.Windows.UI.InheritableBoolean.False;
			this.uiPanelContract.Location = new System.Drawing.Point(0, 0);
			this.uiPanelContract.Name = "uiPanelContract";
			this.uiPanelContract.Size = new System.Drawing.Size(1010, 677);
			this.uiPanelContract.TabIndex = 4;
			this.uiPanelContract.Text = "����������";
			// 
			// uiPanelSearch
			// 
			this.uiPanelSearch.AllowResize = Janus.Windows.UI.InheritableBoolean.False;
			this.uiPanelSearch.CaptionVisible = Janus.Windows.UI.InheritableBoolean.False;
			this.uiPanelSearch.CloseButtonVisible = Janus.Windows.UI.InheritableBoolean.False;
			this.uiPanelSearch.InnerContainer = this.uiPanelSearchContainer;
			this.uiPanelSearch.Location = new System.Drawing.Point(0, 22);
			this.uiPanelSearch.Name = "uiPanelSearch";
			this.uiPanelSearch.Size = new System.Drawing.Size(1010, 66);
			this.uiPanelSearch.TabIndex = 4;
			this.uiPanelSearch.Text = "�˻�";
			// 
			// uiPanelSearchContainer
			// 
			this.uiPanelSearchContainer.Controls.Add(this.pnlSearch);
			this.uiPanelSearchContainer.Location = new System.Drawing.Point(1, 1);
			this.uiPanelSearchContainer.Name = "uiPanelSearchContainer";
			this.uiPanelSearchContainer.Size = new System.Drawing.Size(1008, 64);
			this.uiPanelSearchContainer.TabIndex = 0;
			// 
			// pnlSearch
			// 
			this.pnlSearch.BackColor = System.Drawing.SystemColors.Window;
			this.pnlSearch.Controls.Add(this.rbSch_N);
			this.pnlSearch.Controls.Add(this.rbSch_Y);
			this.pnlSearch.Controls.Add(this.rbSch_All);
			this.pnlSearch.Controls.Add(this.chkAdState_40);
			this.pnlSearch.Controls.Add(this.chkAdState_30);
			this.pnlSearch.Controls.Add(this.chkAdState_20);
			this.pnlSearch.Controls.Add(this.chkAdState_10);
			this.pnlSearch.Controls.Add(this.label13);
			this.pnlSearch.Controls.Add(this.cbSearchMedia);
			this.pnlSearch.Controls.Add(this.cbSearchRap);
			this.pnlSearch.Controls.Add(this.cbSearchAgency);
			this.pnlSearch.Controls.Add(this.cbSearchAdvertiser);
			this.pnlSearch.Controls.Add(this.ebSearchKey);
			this.pnlSearch.Controls.Add(this.cbSearchAdType);
			this.pnlSearch.Controls.Add(this.btnSearch);
			this.pnlSearch.Controls.Add(this.label14);
			this.pnlSearch.Dock = System.Windows.Forms.DockStyle.Fill;
			this.pnlSearch.Location = new System.Drawing.Point(0, 0);
			this.pnlSearch.Name = "pnlSearch";
			this.pnlSearch.Size = new System.Drawing.Size(1008, 64);
			this.pnlSearch.TabIndex = 3;
			// 
			// rbSch_N
			// 
			this.rbSch_N.Location = new System.Drawing.Point(504, 34);
			this.rbSch_N.Name = "rbSch_N";
			this.rbSch_N.Size = new System.Drawing.Size(68, 23);
			this.rbSch_N.TabIndex = 35;
			this.rbSch_N.Text = "����";
			this.rbSch_N.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
			// 
			// rbSch_Y
			// 
			this.rbSch_Y.Location = new System.Drawing.Point(446, 34);
			this.rbSch_Y.Name = "rbSch_Y";
			this.rbSch_Y.Size = new System.Drawing.Size(52, 23);
			this.rbSch_Y.TabIndex = 35;
			this.rbSch_Y.Text = "��";
			this.rbSch_Y.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
			// 
			// rbSch_All
			// 
			this.rbSch_All.Checked = true;
			this.rbSch_All.Location = new System.Drawing.Point(392, 34);
			this.rbSch_All.Name = "rbSch_All";
			this.rbSch_All.Size = new System.Drawing.Size(50, 23);
			this.rbSch_All.TabIndex = 34;
			this.rbSch_All.TabStop = true;
			this.rbSch_All.Text = "��ü";
			this.rbSch_All.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
			// 
			// chkAdState_40
			// 
			this.chkAdState_40.Location = new System.Drawing.Point(241, 34);
			this.chkAdState_40.Name = "chkAdState_40";
			this.chkAdState_40.Size = new System.Drawing.Size(49, 23);
			this.chkAdState_40.TabIndex = 33;
			this.chkAdState_40.Text = "����";
			this.chkAdState_40.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
			// 
			// chkAdState_30
			// 
			this.chkAdState_30.Location = new System.Drawing.Point(186, 34);
			this.chkAdState_30.Name = "chkAdState_30";
			this.chkAdState_30.Size = new System.Drawing.Size(49, 23);
			this.chkAdState_30.TabIndex = 32;
			this.chkAdState_30.Text = "����";
			this.chkAdState_30.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
			// 
			// chkAdState_20
			// 
			this.chkAdState_20.Checked = true;
			this.chkAdState_20.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkAdState_20.Location = new System.Drawing.Point(131, 34);
			this.chkAdState_20.Name = "chkAdState_20";
			this.chkAdState_20.Size = new System.Drawing.Size(49, 23);
			this.chkAdState_20.TabIndex = 31;
			this.chkAdState_20.Text = "��";
			this.chkAdState_20.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
			// 
			// chkAdState_10
			// 
			this.chkAdState_10.Checked = true;
			this.chkAdState_10.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkAdState_10.Location = new System.Drawing.Point(76, 34);
			this.chkAdState_10.Name = "chkAdState_10";
			this.chkAdState_10.Size = new System.Drawing.Size(49, 23);
			this.chkAdState_10.TabIndex = 30;
			this.chkAdState_10.Text = "���";
			this.chkAdState_10.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
			// 
			// label13
			// 
			this.label13.Location = new System.Drawing.Point(6, 35);
			this.label13.Name = "label13";
			this.label13.Size = new System.Drawing.Size(64, 21);
			this.label13.TabIndex = 0;
			this.label13.Text = "�������";
			this.label13.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// cbSearchMedia
			// 
			this.cbSearchMedia.BackColor = System.Drawing.Color.White;
			this.cbSearchMedia.ComboStyle = Janus.Windows.EditControls.ComboStyle.DropDownList;
			this.cbSearchMedia.DisplayMember = "MediaName";
			this.cbSearchMedia.Location = new System.Drawing.Point(8, 9);
			this.cbSearchMedia.Name = "cbSearchMedia";
			this.cbSearchMedia.Size = new System.Drawing.Size(120, 21);
			this.cbSearchMedia.TabIndex = 1;
			this.cbSearchMedia.ValueMember = "MediaCode";
			this.cbSearchMedia.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
			// 
			// cbSearchRap
			// 
			this.cbSearchRap.ComboStyle = Janus.Windows.EditControls.ComboStyle.DropDownList;
			this.cbSearchRap.DisplayMember = "RapName";
			this.cbSearchRap.Location = new System.Drawing.Point(136, 9);
			this.cbSearchRap.Name = "cbSearchRap";
			this.cbSearchRap.Size = new System.Drawing.Size(120, 21);
			this.cbSearchRap.TabIndex = 2;
			this.cbSearchRap.ValueMember = "RapCode";
			this.cbSearchRap.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
			// 
			// cbSearchAgency
			// 
			this.cbSearchAgency.ComboStyle = Janus.Windows.EditControls.ComboStyle.DropDownList;
			this.cbSearchAgency.DisplayMember = "AgencyName";
			this.cbSearchAgency.Location = new System.Drawing.Point(264, 9);
			this.cbSearchAgency.Name = "cbSearchAgency";
			this.cbSearchAgency.Size = new System.Drawing.Size(120, 21);
			this.cbSearchAgency.TabIndex = 3;
			this.cbSearchAgency.ValueMember = "AgencyCode";
			this.cbSearchAgency.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
			// 
			// cbSearchAdvertiser
			// 
			this.cbSearchAdvertiser.ComboStyle = Janus.Windows.EditControls.ComboStyle.DropDownList;
			this.cbSearchAdvertiser.DisplayMember = "AdvertiserName";
			this.cbSearchAdvertiser.Location = new System.Drawing.Point(392, 9);
			this.cbSearchAdvertiser.Name = "cbSearchAdvertiser";
			this.cbSearchAdvertiser.Size = new System.Drawing.Size(120, 21);
			this.cbSearchAdvertiser.TabIndex = 4;
			this.cbSearchAdvertiser.ValueMember = "AdvertiserCode";
			this.cbSearchAdvertiser.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
			// 
			// ebSearchKey
			// 
			this.ebSearchKey.Location = new System.Drawing.Point(644, 9);
			this.ebSearchKey.Name = "ebSearchKey";
			this.ebSearchKey.Size = new System.Drawing.Size(200, 21);
			this.ebSearchKey.TabIndex = 6;
			this.ebSearchKey.Text = "�˻�� �Է��ϼ���";
			this.ebSearchKey.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
			this.ebSearchKey.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2003;
			this.ebSearchKey.TextChanged += new System.EventHandler(this.ebSearchKey_TextChanged);
			this.ebSearchKey.Click += new System.EventHandler(this.ebSearchKey_Click);
			this.ebSearchKey.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ebSearchKey_KeyDown);
			// 
			// cbSearchAdType
			// 
			this.cbSearchAdType.BackColor = System.Drawing.Color.White;
			this.cbSearchAdType.ComboStyle = Janus.Windows.EditControls.ComboStyle.DropDownList;
			this.cbSearchAdType.Location = new System.Drawing.Point(518, 9);
			this.cbSearchAdType.Name = "cbSearchAdType";
			this.cbSearchAdType.Size = new System.Drawing.Size(120, 21);
			this.cbSearchAdType.TabIndex = 5;
			this.cbSearchAdType.Text = "��������";
			this.cbSearchAdType.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
			// 
			// btnSearch
			// 
			this.btnSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnSearch.Enabled = false;
			this.btnSearch.Location = new System.Drawing.Point(895, 8);
			this.btnSearch.Name = "btnSearch";
			this.btnSearch.Size = new System.Drawing.Size(104, 24);
			this.btnSearch.TabIndex = 12;
			this.btnSearch.Text = "�� ȸ";
			this.btnSearch.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
			this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
			// 
			// label14
			// 
			this.label14.Location = new System.Drawing.Point(328, 35);
			this.label14.Name = "label14";
			this.label14.Size = new System.Drawing.Size(64, 21);
			this.label14.TabIndex = 29;
			this.label14.Text = "������";
			this.label14.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// uiPanelList
			// 
			this.uiPanelList.BackColor = System.Drawing.SystemColors.Window;
			this.uiPanelList.CaptionStyle = Janus.Windows.UI.Dock.PanelCaptionStyle.Dark;
			this.uiPanelList.InnerContainer = this.uiPanelListContainer;
			this.uiPanelList.Location = new System.Drawing.Point(0, 92);
			this.uiPanelList.MinimumSize = new System.Drawing.Size(-1, 100);
			this.uiPanelList.Name = "uiPanelList";
			this.uiPanelList.Size = new System.Drawing.Size(1010, 318);
			this.uiPanelList.TabIndex = 13;
			this.uiPanelList.TabStop = false;
			this.uiPanelList.Text = "���౤����";
			// 
			// uiPanelListContainer
			// 
			this.uiPanelListContainer.BackColor = System.Drawing.SystemColors.Window;
			this.uiPanelListContainer.Controls.Add(this.grdExItemList);
			this.uiPanelListContainer.Location = new System.Drawing.Point(1, 23);
			this.uiPanelListContainer.Name = "uiPanelListContainer";
			this.uiPanelListContainer.Size = new System.Drawing.Size(1008, 294);
			this.uiPanelListContainer.TabIndex = 0;
			// 
			// grdExItemList
			// 
			this.grdExItemList.AllowEdit = Janus.Windows.GridEX.InheritableBoolean.False;
			this.grdExItemList.AlternatingColors = true;
			this.grdExItemList.BorderStyle = Janus.Windows.GridEX.BorderStyle.None;
			this.grdExItemList.DataSource = this.dvContractItem;
			grdExItemList_DesignTimeLayout_Reference_0.Instance = ((object)(resources.GetObject("grdExItemList_DesignTimeLayout_Reference_0.Instance")));
			grdExItemList_DesignTimeLayout.LayoutReferences.AddRange(new Janus.Windows.Common.Layouts.JanusLayoutReference[] {
            grdExItemList_DesignTimeLayout_Reference_0});
			grdExItemList_DesignTimeLayout.LayoutString = resources.GetString("grdExItemList_DesignTimeLayout.LayoutString");
			this.grdExItemList.DesignTimeLayout = grdExItemList_DesignTimeLayout;
			this.grdExItemList.Dock = System.Windows.Forms.DockStyle.Fill;
			this.grdExItemList.EditorsControlStyle.ButtonAppearance = Janus.Windows.GridEX.ButtonAppearance.Regular;
			this.grdExItemList.EmptyRows = true;
			this.grdExItemList.FocusCellFormatStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(174)))), ((int)(((byte)(196)))), ((int)(((byte)(232)))));
			this.grdExItemList.FocusCellFormatStyle.FontBold = Janus.Windows.GridEX.TriState.True;
			this.grdExItemList.FocusCellFormatStyle.ForeColor = System.Drawing.Color.Black;
			this.grdExItemList.FocusStyle = Janus.Windows.GridEX.FocusStyle.None;
			this.grdExItemList.Font = new System.Drawing.Font("�������", 8.249999F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
			this.grdExItemList.FrozenColumns = 3;
			this.grdExItemList.GridLineColor = System.Drawing.Color.Silver;
			this.grdExItemList.GridLines = Janus.Windows.GridEX.GridLines.Vertical;
			this.grdExItemList.GridLineStyle = Janus.Windows.GridEX.GridLineStyle.Solid;
			this.grdExItemList.GroupByBoxVisible = false;
			this.grdExItemList.HideSelection = Janus.Windows.GridEX.HideSelection.HighlightInactive;
			this.grdExItemList.Location = new System.Drawing.Point(0, 0);
			this.grdExItemList.Name = "grdExItemList";
			this.grdExItemList.ScrollBars = Janus.Windows.GridEX.ScrollBars.Vertical;
			this.grdExItemList.SelectedInactiveFormatStyle.BackColor = System.Drawing.Color.Gold;
			this.grdExItemList.Size = new System.Drawing.Size(1008, 294);
			this.grdExItemList.TabIndex = 14;
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
			this.grdExItemList.SelectionChanged += new System.EventHandler(this.OnGrdRowChanged);
			this.grdExItemList.Enter += new System.EventHandler(this.OnGrdRowChanged);
			// 
			// dvContractItem
			// 
			this.dvContractItem.Table = this.contractItemDs.ContractItem;
			// 
			// contractItemDs
			// 
			this.contractItemDs.DataSetName = "ContractItemDs";
			this.contractItemDs.Locale = new System.Globalization.CultureInfo("en-US");
			this.contractItemDs.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
			// 
			// uiPanelDetail
			// 
			this.uiPanelDetail.AllowResize = Janus.Windows.UI.InheritableBoolean.False;
			this.uiPanelDetail.BackColor = System.Drawing.SystemColors.Window;
			this.uiPanelDetail.CaptionStyle = Janus.Windows.UI.Dock.PanelCaptionStyle.Dark;
			this.uiPanelDetail.CaptionVisible = Janus.Windows.UI.InheritableBoolean.True;
			this.uiPanelDetail.CloseButtonVisible = Janus.Windows.UI.InheritableBoolean.False;
			this.uiPanelDetail.InnerAreaStyle = Janus.Windows.UI.Dock.PanelInnerAreaStyle.ContainerPanel;
			this.uiPanelDetail.InnerContainer = this.uiPanelDetailContainer;
			this.uiPanelDetail.Location = new System.Drawing.Point(0, 414);
			this.uiPanelDetail.Name = "uiPanelDetail";
			this.uiPanelDetail.Size = new System.Drawing.Size(1010, 263);
			this.uiPanelDetail.TabIndex = 15;
			this.uiPanelDetail.TabStop = false;
			this.uiPanelDetail.Text = "������";
			// 
			// uiPanelDetailContainer
			// 
			this.uiPanelDetailContainer.Controls.Add(this.pnlUserDetail);
			this.uiPanelDetailContainer.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.uiPanelDetailContainer.Location = new System.Drawing.Point(1, 23);
			this.uiPanelDetailContainer.Name = "uiPanelDetailContainer";
			this.uiPanelDetailContainer.Size = new System.Drawing.Size(1008, 239);
			this.uiPanelDetailContainer.TabIndex = 0;
			// 
			// pnlUserDetail
			// 
			this.pnlUserDetail.BackColor = System.Drawing.Color.Transparent;
			this.pnlUserDetail.Controls.Add(this.lblError);
			this.pnlUserDetail.Controls.Add(this.btnCopy);
			this.pnlUserDetail.Controls.Add(this.tabContractItem);
			this.pnlUserDetail.Controls.Add(this.btnDelete);
			this.pnlUserDetail.Controls.Add(this.btnAdd);
			this.pnlUserDetail.Controls.Add(this.btnSave);
			this.pnlUserDetail.Dock = System.Windows.Forms.DockStyle.Fill;
			this.pnlUserDetail.Location = new System.Drawing.Point(0, 0);
			this.pnlUserDetail.Name = "pnlUserDetail";
			this.pnlUserDetail.Size = new System.Drawing.Size(1008, 239);
			this.pnlUserDetail.TabIndex = 300;
			// 
			// lblError
			// 
			this.lblError.AutoEllipsis = true;
			this.lblError.BackColor = System.Drawing.Color.OrangeRed;
			this.lblError.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.lblError.Font = new System.Drawing.Font("�������", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
			this.lblError.Image = ((System.Drawing.Image)(resources.GetObject("lblError.Image")));
			this.lblError.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.lblError.Location = new System.Drawing.Point(423, 206);
			this.lblError.Name = "lblError";
			this.lblError.Size = new System.Drawing.Size(573, 25);
			this.lblError.TabIndex = 229;
			this.lblError.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.lblError.UseCompatibleTextRendering = true;
			this.lblError.Visible = false;
			// 
			// btnCopy
			// 
			this.btnCopy.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.btnCopy.Enabled = false;
			this.btnCopy.Location = new System.Drawing.Point(248, 206);
			this.btnCopy.Name = "btnCopy";
			this.btnCopy.Size = new System.Drawing.Size(104, 24);
			this.btnCopy.TabIndex = 39;
			this.btnCopy.Text = "����������";
			this.btnCopy.Visible = false;
			this.btnCopy.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
			this.btnCopy.Click += new System.EventHandler(this.btnCopy_Click);
			// 
			// tabContractItem
			// 
			this.tabContractItem.Location = new System.Drawing.Point(8, 14);
			this.tabContractItem.Name = "tabContractItem";
			this.tabContractItem.ShowFocusRectangle = false;
			this.tabContractItem.Size = new System.Drawing.Size(991, 186);
			this.tabContractItem.TabIndex = 15;
			this.tabContractItem.TabPages.AddRange(new Janus.Windows.UI.Tab.UITabPage[] {
            this.tabPageItem,
            this.tabPageFile,
            this.tabPageContract,
            this.tabPageHistory,
            this.tabBanner});
			this.tabContractItem.TabStop = false;
			this.tabContractItem.UseCompatibleTextRendering = true;
			this.tabContractItem.VisualStyle = Janus.Windows.UI.Tab.TabVisualStyle.Office2007;
			this.tabContractItem.SelectedTabChanged += new Janus.Windows.UI.Tab.TabEventHandler(this.tabContractItem_SelectedTabChanged);
			// 
			// tabPageItem
			// 
			this.tabPageItem.Controls.Add(this.lblMsg);
			this.tabPageItem.Controls.Add(this.cbScheduleType);
			this.tabPageItem.Controls.Add(this.lbScheduleType);
			this.tabPageItem.Controls.Add(this.ebAdTime);
			this.tabPageItem.Controls.Add(this.meRealEndDay);
			this.tabPageItem.Controls.Add(this.meExcuteEndDay);
			this.tabPageItem.Controls.Add(this.meExcuteStartDay);
			this.tabPageItem.Controls.Add(this.ebContractSeq);
			this.tabPageItem.Controls.Add(this.ebContractName);
			this.tabPageItem.Controls.Add(this.lbExcuteStartDay);
			this.tabPageItem.Controls.Add(this.cbAdState);
			this.tabPageItem.Controls.Add(this.lbRealEndDay);
			this.tabPageItem.Controls.Add(this.lbItemName);
			this.tabPageItem.Controls.Add(this.ebItemName);
			this.tabPageItem.Controls.Add(this.lbAdRate);
			this.tabPageItem.Controls.Add(this.lbAdState);
			this.tabPageItem.Controls.Add(this.cbAdClass);
			this.tabPageItem.Controls.Add(this.lbAdClass);
			this.tabPageItem.Controls.Add(this.lbModDt);
			this.tabPageItem.Controls.Add(this.ebRegDt);
			this.tabPageItem.Controls.Add(this.lbRegName);
			this.tabPageItem.Controls.Add(this.ebModDt);
			this.tabPageItem.Controls.Add(this.lbAdType);
			this.tabPageItem.Controls.Add(this.cbAdType);
			this.tabPageItem.Controls.Add(this.lbExcuteEndDay);
			this.tabPageItem.Controls.Add(this.lbAdTime);
			this.tabPageItem.Controls.Add(this.label10);
			this.tabPageItem.Controls.Add(this.lbRegDt);
			this.tabPageItem.Controls.Add(this.ebRegName);
			this.tabPageItem.Controls.Add(this.btnSearchContract);
			this.tabPageItem.Controls.Add(this.cbAdRate);
			this.tabPageItem.Location = new System.Drawing.Point(1, 22);
			this.tabPageItem.Name = "tabPageItem";
			this.tabPageItem.Size = new System.Drawing.Size(989, 163);
			this.tabPageItem.TabStop = true;
			this.tabPageItem.Text = "������������";
			// 
			// lblMsg
			// 
			this.lblMsg.BackColor = System.Drawing.Color.Transparent;
			this.lblMsg.Font = new System.Drawing.Font("�������", 8.249999F, System.Drawing.FontStyle.Bold);
			this.lblMsg.Location = new System.Drawing.Point(643, 100);
			this.lblMsg.Name = "lblMsg";
			this.lblMsg.Size = new System.Drawing.Size(328, 21);
			this.lblMsg.TabIndex = 228;
			this.lblMsg.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// cbScheduleType
			// 
			this.cbScheduleType.BackColor = System.Drawing.Color.White;
			this.cbScheduleType.ComboStyle = Janus.Windows.EditControls.ComboStyle.DropDownList;
			this.cbScheduleType.Location = new System.Drawing.Point(80, 102);
			this.cbScheduleType.Name = "cbScheduleType";
			this.cbScheduleType.Size = new System.Drawing.Size(120, 21);
			this.cbScheduleType.TabIndex = 226;
			this.cbScheduleType.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
			// 
			// lbScheduleType
			// 
			this.lbScheduleType.BackColor = System.Drawing.Color.Transparent;
			this.lbScheduleType.Location = new System.Drawing.Point(8, 102);
			this.lbScheduleType.Name = "lbScheduleType";
			this.lbScheduleType.Size = new System.Drawing.Size(72, 21);
			this.lbScheduleType.TabIndex = 225;
			this.lbScheduleType.Text = "������";
			this.lbScheduleType.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// ebAdTime
			// 
			this.ebAdTime.DecimalDigits = 0;
			this.ebAdTime.FormatString = "#,##0";
			this.ebAdTime.Location = new System.Drawing.Point(656, 33);
			this.ebAdTime.MaxLength = 4;
			this.ebAdTime.Name = "ebAdTime";
			this.ebAdTime.Size = new System.Drawing.Size(56, 21);
			this.ebAdTime.TabIndex = 24;
			this.ebAdTime.Text = "15";
			this.ebAdTime.TextAlignment = Janus.Windows.GridEX.TextAlignment.Far;
			this.ebAdTime.Value = new decimal(new int[] {
            15,
            0,
            0,
            0});
			this.ebAdTime.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
			// 
			// meRealEndDay
			// 
			// 
			// 
			// 
			this.meRealEndDay.DropDownCalendar.FirstMonth = new System.DateTime(2007, 8, 1, 0, 0, 0, 0);
			this.meRealEndDay.DropDownCalendar.Name = "";
			this.meRealEndDay.DropDownCalendar.VisualStyle = Janus.Windows.CalendarCombo.VisualStyle.Office2007;
			this.meRealEndDay.Location = new System.Drawing.Point(80, 79);
			this.meRealEndDay.Name = "meRealEndDay";
			this.meRealEndDay.Size = new System.Drawing.Size(120, 21);
			this.meRealEndDay.TabIndex = 2;
			this.meRealEndDay.Value = new System.DateTime(2008, 1, 30, 0, 0, 0, 0);
			this.meRealEndDay.VisualStyle = Janus.Windows.CalendarCombo.VisualStyle.Office2007;
			// 
			// meExcuteEndDay
			// 
			// 
			// 
			// 
			this.meExcuteEndDay.DropDownCalendar.FirstMonth = new System.DateTime(2007, 8, 1, 0, 0, 0, 0);
			this.meExcuteEndDay.DropDownCalendar.Name = "";
			this.meExcuteEndDay.DropDownCalendar.VisualStyle = Janus.Windows.CalendarCombo.VisualStyle.Office2007;
			this.meExcuteEndDay.Location = new System.Drawing.Point(80, 57);
			this.meExcuteEndDay.Name = "meExcuteEndDay";
			this.meExcuteEndDay.Size = new System.Drawing.Size(120, 21);
			this.meExcuteEndDay.TabIndex = 1;
			this.meExcuteEndDay.Value = new System.DateTime(2008, 1, 30, 0, 0, 0, 0);
			this.meExcuteEndDay.VisualStyle = Janus.Windows.CalendarCombo.VisualStyle.Office2007;
			// 
			// meExcuteStartDay
			// 
			// 
			// 
			// 
			this.meExcuteStartDay.DropDownCalendar.FirstMonth = new System.DateTime(2007, 8, 1, 0, 0, 0, 0);
			this.meExcuteStartDay.DropDownCalendar.Name = "";
			this.meExcuteStartDay.DropDownCalendar.VisualStyle = Janus.Windows.CalendarCombo.VisualStyle.Office2007;
			this.meExcuteStartDay.Location = new System.Drawing.Point(80, 33);
			this.meExcuteStartDay.Name = "meExcuteStartDay";
			this.meExcuteStartDay.Size = new System.Drawing.Size(120, 21);
			this.meExcuteStartDay.TabIndex = 0;
			this.meExcuteStartDay.Value = new System.DateTime(2008, 1, 30, 0, 0, 0, 0);
			this.meExcuteStartDay.VisualStyle = Janus.Windows.CalendarCombo.VisualStyle.Office2007;
			// 
			// ebContractSeq
			// 
			this.ebContractSeq.BackColor = System.Drawing.Color.WhiteSmoke;
			this.ebContractSeq.Location = new System.Drawing.Point(695, 59);
			this.ebContractSeq.MaxLength = 10;
			this.ebContractSeq.Name = "ebContractSeq";
			this.ebContractSeq.ReadOnly = true;
			this.ebContractSeq.Size = new System.Drawing.Size(48, 21);
			this.ebContractSeq.TabIndex = 100;
			this.ebContractSeq.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
			this.ebContractSeq.Visible = false;
			this.ebContractSeq.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
			// 
			// ebContractName
			// 
			this.ebContractName.BackColor = System.Drawing.Color.WhiteSmoke;
			this.ebContractName.ButtonEnabled = false;
			this.ebContractName.Location = new System.Drawing.Point(488, 10);
			this.ebContractName.MaxLength = 50;
			this.ebContractName.Name = "ebContractName";
			this.ebContractName.ReadOnly = true;
			this.ebContractName.Size = new System.Drawing.Size(483, 21);
			this.ebContractName.TabIndex = 0;
			this.ebContractName.TabStop = false;
			this.ebContractName.Text = "����";
			this.ebContractName.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
			this.ebContractName.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
			// 
			// lbExcuteStartDay
			// 
			this.lbExcuteStartDay.BackColor = System.Drawing.Color.Transparent;
			this.lbExcuteStartDay.Location = new System.Drawing.Point(8, 33);
			this.lbExcuteStartDay.Name = "lbExcuteStartDay";
			this.lbExcuteStartDay.Size = new System.Drawing.Size(72, 21);
			this.lbExcuteStartDay.TabIndex = 96;
			this.lbExcuteStartDay.Text = "���������";
			this.lbExcuteStartDay.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// cbAdState
			// 
			this.cbAdState.ComboStyle = Janus.Windows.EditControls.ComboStyle.DropDownList;
			this.cbAdState.DataSource = this.contractItemDs.AdState;
			this.cbAdState.DisplayMember = "CodeName";
			this.cbAdState.Location = new System.Drawing.Point(288, 33);
			this.cbAdState.Name = "cbAdState";
			this.cbAdState.Size = new System.Drawing.Size(120, 21);
			this.cbAdState.TabIndex = 4;
			this.cbAdState.ValueMember = "Code";
			this.cbAdState.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
			this.cbAdState.SelectedIndexChanged += new System.EventHandler(this.cbAdState_SelectedItemChanged);
			// 
			// lbRealEndDay
			// 
			this.lbRealEndDay.BackColor = System.Drawing.Color.Transparent;
			this.lbRealEndDay.Location = new System.Drawing.Point(8, 79);
			this.lbRealEndDay.Name = "lbRealEndDay";
			this.lbRealEndDay.Size = new System.Drawing.Size(72, 21);
			this.lbRealEndDay.TabIndex = 87;
			this.lbRealEndDay.Text = "����������";
			this.lbRealEndDay.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// lbItemName
			// 
			this.lbItemName.BackColor = System.Drawing.Color.Transparent;
			this.lbItemName.Location = new System.Drawing.Point(8, 10);
			this.lbItemName.Name = "lbItemName";
			this.lbItemName.Size = new System.Drawing.Size(72, 21);
			this.lbItemName.TabIndex = 86;
			this.lbItemName.Text = "�����";
			this.lbItemName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// ebItemName
			// 
			this.ebItemName.Location = new System.Drawing.Point(80, 10);
			this.ebItemName.MaxLength = 50;
			this.ebItemName.Name = "ebItemName";
			this.ebItemName.Size = new System.Drawing.Size(328, 21);
			this.ebItemName.TabIndex = 15;
			this.ebItemName.Text = "�����";
			this.ebItemName.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
			this.ebItemName.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
			// 
			// lbAdRate
			// 
			this.lbAdRate.BackColor = System.Drawing.Color.Transparent;
			this.lbAdRate.Location = new System.Drawing.Point(424, 33);
			this.lbAdRate.Name = "lbAdRate";
			this.lbAdRate.Size = new System.Drawing.Size(56, 21);
			this.lbAdRate.TabIndex = 83;
			this.lbAdRate.Text = "������";
			this.lbAdRate.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// lbAdState
			// 
			this.lbAdState.BackColor = System.Drawing.Color.Transparent;
			this.lbAdState.Location = new System.Drawing.Point(216, 33);
			this.lbAdState.Name = "lbAdState";
			this.lbAdState.Size = new System.Drawing.Size(72, 21);
			this.lbAdState.TabIndex = 73;
			this.lbAdState.Text = "�������";
			this.lbAdState.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// cbAdClass
			// 
			this.cbAdClass.BackColor = System.Drawing.Color.White;
			this.cbAdClass.ComboStyle = Janus.Windows.EditControls.ComboStyle.DropDownList;
			this.cbAdClass.DataSource = this.contractItemDs.AdClass;
			this.cbAdClass.DisplayMember = "CodeName";
			this.cbAdClass.Location = new System.Drawing.Point(288, 57);
			this.cbAdClass.Name = "cbAdClass";
			this.cbAdClass.Size = new System.Drawing.Size(120, 21);
			this.cbAdClass.TabIndex = 5;
			this.cbAdClass.ValueMember = "Code";
			this.cbAdClass.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
			// 
			// lbAdClass
			// 
			this.lbAdClass.BackColor = System.Drawing.Color.Transparent;
			this.lbAdClass.Location = new System.Drawing.Point(216, 57);
			this.lbAdClass.Name = "lbAdClass";
			this.lbAdClass.Size = new System.Drawing.Size(72, 21);
			this.lbAdClass.TabIndex = 72;
			this.lbAdClass.Text = "OAP����";
			this.lbAdClass.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// lbModDt
			// 
			this.lbModDt.BackColor = System.Drawing.Color.Transparent;
			this.lbModDt.Location = new System.Drawing.Point(752, 57);
			this.lbModDt.Name = "lbModDt";
			this.lbModDt.Size = new System.Drawing.Size(56, 21);
			this.lbModDt.TabIndex = 74;
			this.lbModDt.Text = "��������";
			this.lbModDt.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// ebRegDt
			// 
			this.ebRegDt.BackColor = System.Drawing.Color.WhiteSmoke;
			this.ebRegDt.Location = new System.Drawing.Point(816, 33);
			this.ebRegDt.Name = "ebRegDt";
			this.ebRegDt.ReadOnly = true;
			this.ebRegDt.Size = new System.Drawing.Size(155, 21);
			this.ebRegDt.TabIndex = 0;
			this.ebRegDt.TabStop = false;
			this.ebRegDt.Text = "����Ͻ�";
			this.ebRegDt.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
			this.ebRegDt.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
			// 
			// lbRegName
			// 
			this.lbRegName.BackColor = System.Drawing.Color.Transparent;
			this.lbRegName.Location = new System.Drawing.Point(752, 79);
			this.lbRegName.Name = "lbRegName";
			this.lbRegName.Size = new System.Drawing.Size(56, 21);
			this.lbRegName.TabIndex = 75;
			this.lbRegName.Text = "�����";
			this.lbRegName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// ebModDt
			// 
			this.ebModDt.BackColor = System.Drawing.Color.WhiteSmoke;
			this.ebModDt.Location = new System.Drawing.Point(816, 57);
			this.ebModDt.MaxLength = 15;
			this.ebModDt.Name = "ebModDt";
			this.ebModDt.ReadOnly = true;
			this.ebModDt.Size = new System.Drawing.Size(155, 21);
			this.ebModDt.TabIndex = 0;
			this.ebModDt.TabStop = false;
			this.ebModDt.Text = "��������";
			this.ebModDt.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
			this.ebModDt.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
			// 
			// lbAdType
			// 
			this.lbAdType.BackColor = System.Drawing.Color.Transparent;
			this.lbAdType.Location = new System.Drawing.Point(216, 79);
			this.lbAdType.Name = "lbAdType";
			this.lbAdType.Size = new System.Drawing.Size(72, 21);
			this.lbAdType.TabIndex = 84;
			this.lbAdType.Text = "��������";
			this.lbAdType.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// cbAdType
			// 
			this.cbAdType.BackColor = System.Drawing.Color.White;
			this.cbAdType.ComboStyle = Janus.Windows.EditControls.ComboStyle.DropDownList;
			this.cbAdType.DataSource = this.contractItemDs.AdType;
			this.cbAdType.DisplayMember = "CodeName";
			this.cbAdType.Location = new System.Drawing.Point(288, 79);
			this.cbAdType.Name = "cbAdType";
			this.cbAdType.Size = new System.Drawing.Size(120, 21);
			this.cbAdType.TabIndex = 6;
			this.cbAdType.ValueMember = "Code";
			this.cbAdType.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
			// 
			// lbExcuteEndDay
			// 
			this.lbExcuteEndDay.BackColor = System.Drawing.Color.Transparent;
			this.lbExcuteEndDay.Location = new System.Drawing.Point(8, 57);
			this.lbExcuteEndDay.Name = "lbExcuteEndDay";
			this.lbExcuteEndDay.Size = new System.Drawing.Size(72, 21);
			this.lbExcuteEndDay.TabIndex = 96;
			this.lbExcuteEndDay.Text = "����������";
			this.lbExcuteEndDay.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// lbAdTime
			// 
			this.lbAdTime.BackColor = System.Drawing.Color.Transparent;
			this.lbAdTime.Location = new System.Drawing.Point(592, 33);
			this.lbAdTime.Name = "lbAdTime";
			this.lbAdTime.Size = new System.Drawing.Size(56, 21);
			this.lbAdTime.TabIndex = 83;
			this.lbAdTime.Text = "�������";
			this.lbAdTime.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label10
			// 
			this.label10.BackColor = System.Drawing.Color.Transparent;
			this.label10.Location = new System.Drawing.Point(720, 33);
			this.label10.Name = "label10";
			this.label10.Size = new System.Drawing.Size(24, 21);
			this.label10.TabIndex = 83;
			this.label10.Text = "��";
			this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// lbRegDt
			// 
			this.lbRegDt.BackColor = System.Drawing.Color.Transparent;
			this.lbRegDt.Location = new System.Drawing.Point(752, 33);
			this.lbRegDt.Name = "lbRegDt";
			this.lbRegDt.Size = new System.Drawing.Size(56, 21);
			this.lbRegDt.TabIndex = 74;
			this.lbRegDt.Text = "����Ͻ�";
			this.lbRegDt.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// ebRegName
			// 
			this.ebRegName.BackColor = System.Drawing.Color.WhiteSmoke;
			this.ebRegName.Location = new System.Drawing.Point(816, 79);
			this.ebRegName.MaxLength = 15;
			this.ebRegName.Name = "ebRegName";
			this.ebRegName.ReadOnly = true;
			this.ebRegName.Size = new System.Drawing.Size(155, 21);
			this.ebRegName.TabIndex = 0;
			this.ebRegName.TabStop = false;
			this.ebRegName.Text = "�����";
			this.ebRegName.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
			this.ebRegName.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
			// 
			// btnSearchContract
			// 
			this.btnSearchContract.Enabled = false;
			this.btnSearchContract.Location = new System.Drawing.Point(424, 10);
			this.btnSearchContract.Name = "btnSearchContract";
			this.btnSearchContract.Size = new System.Drawing.Size(58, 22);
			this.btnSearchContract.TabIndex = 18;
			this.btnSearchContract.Text = "��༱��";
			this.btnSearchContract.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
			this.btnSearchContract.Click += new System.EventHandler(this.btnSearchContract_Click);
			// 
			// cbAdRate
			// 
			this.cbAdRate.BackColor = System.Drawing.Color.White;
			this.cbAdRate.ComboStyle = Janus.Windows.EditControls.ComboStyle.DropDownList;
			uiComboBoxItem1.FormatStyle.Alpha = 0;
			uiComboBoxItem1.IsSeparator = false;
			uiComboBoxItem1.Text = "��ü�̿밡";
			uiComboBoxItem1.Value = "0";
			uiComboBoxItem2.FormatStyle.Alpha = 0;
			uiComboBoxItem2.IsSeparator = false;
			uiComboBoxItem2.Text = "12���̻�";
			uiComboBoxItem2.Value = "12";
			uiComboBoxItem3.FormatStyle.Alpha = 0;
			uiComboBoxItem3.IsSeparator = false;
			uiComboBoxItem3.Text = "15���̻�";
			uiComboBoxItem3.Value = "15";
			uiComboBoxItem4.FormatStyle.Alpha = 0;
			uiComboBoxItem4.IsSeparator = false;
			uiComboBoxItem4.Text = "19���̻�";
			uiComboBoxItem4.Value = "19";
			this.cbAdRate.Items.AddRange(new Janus.Windows.EditControls.UIComboBoxItem[] {
            uiComboBoxItem1,
            uiComboBoxItem2,
            uiComboBoxItem3,
            uiComboBoxItem4});
			this.cbAdRate.Location = new System.Drawing.Point(488, 33);
			this.cbAdRate.Name = "cbAdRate";
			this.cbAdRate.Size = new System.Drawing.Size(88, 21);
			this.cbAdRate.TabIndex = 21;
			this.cbAdRate.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
			// 
			// tabPageFile
			// 
			this.tabPageFile.Controls.Add(this.ebFileRegDt);
			this.tabPageFile.Controls.Add(this.lbFileRegName);
			this.tabPageFile.Controls.Add(this.lbFileRegDt);
			this.tabPageFile.Controls.Add(this.ebFileRegName);
			this.tabPageFile.Controls.Add(this.ebFileLength);
			this.tabPageFile.Controls.Add(this.ebDownLevel);
			this.tabPageFile.Controls.Add(this.ebFileType);
			this.tabPageFile.Controls.Add(this.ebFileName);
			this.tabPageFile.Controls.Add(this.lbFileState);
			this.tabPageFile.Controls.Add(this.lbFileName);
			this.tabPageFile.Controls.Add(this.lbFileType);
			this.tabPageFile.Controls.Add(this.lbFileLength);
			this.tabPageFile.Controls.Add(this.lbDownLevel);
			this.tabPageFile.Controls.Add(this.lbFilePath);
			this.tabPageFile.Controls.Add(this.ebFilePath);
			this.tabPageFile.Controls.Add(this.ebFileState);
			this.tabPageFile.Location = new System.Drawing.Point(1, 22);
			this.tabPageFile.Name = "tabPageFile";
			this.tabPageFile.Size = new System.Drawing.Size(989, 163);
			this.tabPageFile.TabStop = true;
			this.tabPageFile.Text = "��������";
			this.tabPageFile.Visible = false;
			// 
			// ebFileRegDt
			// 
			this.ebFileRegDt.BackColor = System.Drawing.Color.WhiteSmoke;
			this.ebFileRegDt.Location = new System.Drawing.Point(696, 10);
			this.ebFileRegDt.Name = "ebFileRegDt";
			this.ebFileRegDt.ReadOnly = true;
			this.ebFileRegDt.Size = new System.Drawing.Size(180, 21);
			this.ebFileRegDt.TabIndex = 0;
			this.ebFileRegDt.TabStop = false;
			this.ebFileRegDt.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
			this.ebFileRegDt.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
			// 
			// lbFileRegName
			// 
			this.lbFileRegName.BackColor = System.Drawing.Color.Transparent;
			this.lbFileRegName.Location = new System.Drawing.Point(636, 37);
			this.lbFileRegName.Name = "lbFileRegName";
			this.lbFileRegName.Size = new System.Drawing.Size(48, 14);
			this.lbFileRegName.TabIndex = 112;
			this.lbFileRegName.Text = "�����";
			this.lbFileRegName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// lbFileRegDt
			// 
			this.lbFileRegDt.BackColor = System.Drawing.Color.Transparent;
			this.lbFileRegDt.Location = new System.Drawing.Point(636, 10);
			this.lbFileRegDt.Name = "lbFileRegDt";
			this.lbFileRegDt.Size = new System.Drawing.Size(56, 21);
			this.lbFileRegDt.TabIndex = 110;
			this.lbFileRegDt.Text = "����Ͻ�";
			this.lbFileRegDt.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// ebFileRegName
			// 
			this.ebFileRegName.BackColor = System.Drawing.Color.WhiteSmoke;
			this.ebFileRegName.Location = new System.Drawing.Point(696, 34);
			this.ebFileRegName.MaxLength = 15;
			this.ebFileRegName.Name = "ebFileRegName";
			this.ebFileRegName.ReadOnly = true;
			this.ebFileRegName.Size = new System.Drawing.Size(180, 21);
			this.ebFileRegName.TabIndex = 0;
			this.ebFileRegName.TabStop = false;
			this.ebFileRegName.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
			this.ebFileRegName.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
			// 
			// ebFileLength
			// 
			this.ebFileLength.BackColor = System.Drawing.Color.WhiteSmoke;
			this.ebFileLength.Location = new System.Drawing.Point(288, 34);
			this.ebFileLength.MaxLength = 40;
			this.ebFileLength.Name = "ebFileLength";
			this.ebFileLength.ReadOnly = true;
			this.ebFileLength.Size = new System.Drawing.Size(120, 21);
			this.ebFileLength.TabIndex = 0;
			this.ebFileLength.TabStop = false;
			this.ebFileLength.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
			this.ebFileLength.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
			// 
			// ebDownLevel
			// 
			this.ebDownLevel.BackColor = System.Drawing.Color.WhiteSmoke;
			this.ebDownLevel.Location = new System.Drawing.Point(496, 34);
			this.ebDownLevel.MaxLength = 40;
			this.ebDownLevel.Name = "ebDownLevel";
			this.ebDownLevel.ReadOnly = true;
			this.ebDownLevel.Size = new System.Drawing.Size(120, 21);
			this.ebDownLevel.TabIndex = 0;
			this.ebDownLevel.TabStop = false;
			this.ebDownLevel.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
			this.ebDownLevel.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
			// 
			// ebFileType
			// 
			this.ebFileType.BackColor = System.Drawing.Color.WhiteSmoke;
			this.ebFileType.Location = new System.Drawing.Point(80, 34);
			this.ebFileType.MaxLength = 40;
			this.ebFileType.Name = "ebFileType";
			this.ebFileType.ReadOnly = true;
			this.ebFileType.Size = new System.Drawing.Size(120, 21);
			this.ebFileType.TabIndex = 0;
			this.ebFileType.TabStop = false;
			this.ebFileType.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
			this.ebFileType.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
			// 
			// ebFileName
			// 
			this.ebFileName.BackColor = System.Drawing.Color.WhiteSmoke;
			this.ebFileName.Location = new System.Drawing.Point(80, 10);
			this.ebFileName.MaxLength = 40;
			this.ebFileName.Name = "ebFileName";
			this.ebFileName.ReadOnly = true;
			this.ebFileName.Size = new System.Drawing.Size(328, 21);
			this.ebFileName.TabIndex = 0;
			this.ebFileName.TabStop = false;
			this.ebFileName.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
			this.ebFileName.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
			// 
			// lbFileState
			// 
			this.lbFileState.BackColor = System.Drawing.Color.Transparent;
			this.lbFileState.Location = new System.Drawing.Point(436, 10);
			this.lbFileState.Name = "lbFileState";
			this.lbFileState.Size = new System.Drawing.Size(56, 21);
			this.lbFileState.TabIndex = 102;
			this.lbFileState.Text = "���ϻ���";
			this.lbFileState.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// lbFileName
			// 
			this.lbFileName.BackColor = System.Drawing.Color.Transparent;
			this.lbFileName.Location = new System.Drawing.Point(8, 10);
			this.lbFileName.Name = "lbFileName";
			this.lbFileName.Size = new System.Drawing.Size(56, 21);
			this.lbFileName.TabIndex = 99;
			this.lbFileName.Text = "���ϸ�";
			this.lbFileName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// lbFileType
			// 
			this.lbFileType.BackColor = System.Drawing.Color.Transparent;
			this.lbFileType.Location = new System.Drawing.Point(8, 34);
			this.lbFileType.Name = "lbFileType";
			this.lbFileType.Size = new System.Drawing.Size(56, 21);
			this.lbFileType.TabIndex = 102;
			this.lbFileType.Text = "���ϱ���";
			this.lbFileType.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// lbFileLength
			// 
			this.lbFileLength.BackColor = System.Drawing.Color.Transparent;
			this.lbFileLength.Location = new System.Drawing.Point(216, 34);
			this.lbFileLength.Name = "lbFileLength";
			this.lbFileLength.Size = new System.Drawing.Size(56, 21);
			this.lbFileLength.TabIndex = 102;
			this.lbFileLength.Text = "���ϱ���";
			// 
			// lbDownLevel
			// 
			this.lbDownLevel.BackColor = System.Drawing.Color.Transparent;
			this.lbDownLevel.Location = new System.Drawing.Point(436, 34);
			this.lbDownLevel.Name = "lbDownLevel";
			this.lbDownLevel.Size = new System.Drawing.Size(56, 21);
			this.lbDownLevel.TabIndex = 102;
			this.lbDownLevel.Text = "�ٿ����";
			this.lbDownLevel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// lbFilePath
			// 
			this.lbFilePath.BackColor = System.Drawing.Color.Transparent;
			this.lbFilePath.Location = new System.Drawing.Point(8, 59);
			this.lbFilePath.Name = "lbFilePath";
			this.lbFilePath.Size = new System.Drawing.Size(56, 21);
			this.lbFilePath.TabIndex = 99;
			this.lbFilePath.Text = "������ġ";
			this.lbFilePath.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// ebFilePath
			// 
			this.ebFilePath.BackColor = System.Drawing.Color.WhiteSmoke;
			this.ebFilePath.Location = new System.Drawing.Point(80, 59);
			this.ebFilePath.MaxLength = 40;
			this.ebFilePath.Name = "ebFilePath";
			this.ebFilePath.ReadOnly = true;
			this.ebFilePath.Size = new System.Drawing.Size(328, 21);
			this.ebFilePath.TabIndex = 0;
			this.ebFilePath.TabStop = false;
			this.ebFilePath.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
			this.ebFilePath.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
			// 
			// ebFileState
			// 
			this.ebFileState.BackColor = System.Drawing.Color.WhiteSmoke;
			this.ebFileState.Location = new System.Drawing.Point(496, 10);
			this.ebFileState.MaxLength = 40;
			this.ebFileState.Name = "ebFileState";
			this.ebFileState.ReadOnly = true;
			this.ebFileState.Size = new System.Drawing.Size(120, 21);
			this.ebFileState.TabIndex = 0;
			this.ebFileState.TabStop = false;
			this.ebFileState.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
			this.ebFileState.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
			// 
			// tabPageContract
			// 
			this.tabPageContract.Controls.Add(this.ebContractRegDt);
			this.tabPageContract.Controls.Add(this.ebContractModDt);
			this.tabPageContract.Controls.Add(this.ebContractRegName);
			this.tabPageContract.Controls.Add(this.label1);
			this.tabPageContract.Controls.Add(this.label2);
			this.tabPageContract.Controls.Add(this.label3);
			this.tabPageContract.Controls.Add(this.ebComment);
			this.tabPageContract.Controls.Add(this.label4);
			this.tabPageContract.Controls.Add(this.ebContEndDay);
			this.tabPageContract.Controls.Add(this.ebContractName2);
			this.tabPageContract.Controls.Add(this.label5);
			this.tabPageContract.Controls.Add(this.ebContStartDay);
			this.tabPageContract.Controls.Add(this.ebRap);
			this.tabPageContract.Controls.Add(this.ebAgency);
			this.tabPageContract.Controls.Add(this.label6);
			this.tabPageContract.Controls.Add(this.label7);
			this.tabPageContract.Controls.Add(this.label8);
			this.tabPageContract.Controls.Add(this.ebMedia);
			this.tabPageContract.Controls.Add(this.label9);
			this.tabPageContract.Controls.Add(this.ebContractState);
			this.tabPageContract.Controls.Add(this.label11);
			this.tabPageContract.Controls.Add(this.label12);
			this.tabPageContract.Location = new System.Drawing.Point(1, 22);
			this.tabPageContract.Name = "tabPageContract";
			this.tabPageContract.Size = new System.Drawing.Size(989, 163);
			this.tabPageContract.TabStop = true;
			this.tabPageContract.Text = "�������";
			this.tabPageContract.Visible = false;
			// 
			// ebContractRegDt
			// 
			this.ebContractRegDt.BackColor = System.Drawing.Color.WhiteSmoke;
			this.ebContractRegDt.Location = new System.Drawing.Point(696, 10);
			this.ebContractRegDt.Name = "ebContractRegDt";
			this.ebContractRegDt.ReadOnly = true;
			this.ebContractRegDt.Size = new System.Drawing.Size(227, 21);
			this.ebContractRegDt.TabIndex = 0;
			this.ebContractRegDt.TabStop = false;
			this.ebContractRegDt.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
			this.ebContractRegDt.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
			// 
			// ebContractModDt
			// 
			this.ebContractModDt.BackColor = System.Drawing.Color.WhiteSmoke;
			this.ebContractModDt.Location = new System.Drawing.Point(696, 34);
			this.ebContractModDt.MaxLength = 15;
			this.ebContractModDt.Name = "ebContractModDt";
			this.ebContractModDt.ReadOnly = true;
			this.ebContractModDt.Size = new System.Drawing.Size(227, 21);
			this.ebContractModDt.TabIndex = 0;
			this.ebContractModDt.TabStop = false;
			this.ebContractModDt.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
			this.ebContractModDt.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
			// 
			// ebContractRegName
			// 
			this.ebContractRegName.BackColor = System.Drawing.Color.WhiteSmoke;
			this.ebContractRegName.Location = new System.Drawing.Point(696, 58);
			this.ebContractRegName.MaxLength = 15;
			this.ebContractRegName.Name = "ebContractRegName";
			this.ebContractRegName.ReadOnly = true;
			this.ebContractRegName.Size = new System.Drawing.Size(227, 21);
			this.ebContractRegName.TabIndex = 0;
			this.ebContractRegName.TabStop = false;
			this.ebContractRegName.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
			this.ebContractRegName.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
			// 
			// label1
			// 
			this.label1.BackColor = System.Drawing.Color.Transparent;
			this.label1.Location = new System.Drawing.Point(632, 34);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(56, 21);
			this.label1.TabIndex = 117;
			this.label1.Text = "��������";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label2
			// 
			this.label2.BackColor = System.Drawing.Color.Transparent;
			this.label2.Location = new System.Drawing.Point(632, 58);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(48, 21);
			this.label2.TabIndex = 118;
			this.label2.Text = "�����";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label3
			// 
			this.label3.BackColor = System.Drawing.Color.Transparent;
			this.label3.Location = new System.Drawing.Point(632, 10);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(56, 21);
			this.label3.TabIndex = 116;
			this.label3.Text = "����Ͻ�";
			this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// ebComment
			// 
			this.ebComment.BackColor = System.Drawing.Color.WhiteSmoke;
			this.ebComment.Location = new System.Drawing.Point(288, 58);
			this.ebComment.MaxLength = 50;
			this.ebComment.Multiline = true;
			this.ebComment.Name = "ebComment";
			this.ebComment.ReadOnly = true;
			this.ebComment.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.ebComment.Size = new System.Drawing.Size(328, 48);
			this.ebComment.TabIndex = 0;
			this.ebComment.TabStop = false;
			this.ebComment.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
			this.ebComment.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
			// 
			// label4
			// 
			this.label4.BackColor = System.Drawing.Color.Transparent;
			this.label4.Location = new System.Drawing.Point(8, 84);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(56, 21);
			this.label4.TabIndex = 18;
			this.label4.Text = "�����";
			this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// ebContEndDay
			// 
			this.ebContEndDay.BackColor = System.Drawing.Color.WhiteSmoke;
			this.ebContEndDay.Location = new System.Drawing.Point(496, 34);
			this.ebContEndDay.MaxLength = 40;
			this.ebContEndDay.Name = "ebContEndDay";
			this.ebContEndDay.ReadOnly = true;
			this.ebContEndDay.Size = new System.Drawing.Size(120, 21);
			this.ebContEndDay.TabIndex = 0;
			this.ebContEndDay.TabStop = false;
			this.ebContEndDay.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
			this.ebContEndDay.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
			// 
			// ebContractName2
			// 
			this.ebContractName2.BackColor = System.Drawing.Color.WhiteSmoke;
			this.ebContractName2.Location = new System.Drawing.Point(80, 10);
			this.ebContractName2.MaxLength = 40;
			this.ebContractName2.Name = "ebContractName2";
			this.ebContractName2.ReadOnly = true;
			this.ebContractName2.Size = new System.Drawing.Size(328, 21);
			this.ebContractName2.TabIndex = 0;
			this.ebContractName2.TabStop = false;
			this.ebContractName2.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
			this.ebContractName2.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
			// 
			// label5
			// 
			this.label5.BackColor = System.Drawing.Color.Transparent;
			this.label5.Location = new System.Drawing.Point(424, 10);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(56, 21);
			this.label5.TabIndex = 18;
			this.label5.Text = "������";
			this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// ebContStartDay
			// 
			this.ebContStartDay.BackColor = System.Drawing.Color.WhiteSmoke;
			this.ebContStartDay.Location = new System.Drawing.Point(288, 34);
			this.ebContStartDay.MaxLength = 40;
			this.ebContStartDay.Name = "ebContStartDay";
			this.ebContStartDay.ReadOnly = true;
			this.ebContStartDay.Size = new System.Drawing.Size(120, 21);
			this.ebContStartDay.TabIndex = 0;
			this.ebContStartDay.TabStop = false;
			this.ebContStartDay.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
			this.ebContStartDay.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
			// 
			// ebRap
			// 
			this.ebRap.BackColor = System.Drawing.Color.WhiteSmoke;
			this.ebRap.Location = new System.Drawing.Point(80, 58);
			this.ebRap.MaxLength = 40;
			this.ebRap.Name = "ebRap";
			this.ebRap.ReadOnly = true;
			this.ebRap.Size = new System.Drawing.Size(120, 21);
			this.ebRap.TabIndex = 0;
			this.ebRap.TabStop = false;
			this.ebRap.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
			this.ebRap.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
			// 
			// ebAgency
			// 
			this.ebAgency.BackColor = System.Drawing.Color.WhiteSmoke;
			this.ebAgency.Location = new System.Drawing.Point(80, 82);
			this.ebAgency.MaxLength = 40;
			this.ebAgency.Name = "ebAgency";
			this.ebAgency.ReadOnly = true;
			this.ebAgency.Size = new System.Drawing.Size(120, 21);
			this.ebAgency.TabIndex = 0;
			this.ebAgency.TabStop = false;
			this.ebAgency.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
			this.ebAgency.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
			// 
			// label6
			// 
			this.label6.BackColor = System.Drawing.Color.Transparent;
			this.label6.Location = new System.Drawing.Point(216, 34);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(72, 21);
			this.label6.TabIndex = 46;
			this.label6.Text = "��������";
			this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label7
			// 
			this.label7.BackColor = System.Drawing.Color.Transparent;
			this.label7.Location = new System.Drawing.Point(8, 10);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(48, 21);
			this.label7.TabIndex = 18;
			this.label7.Text = "����";
			this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label8
			// 
			this.label8.BackColor = System.Drawing.Color.Transparent;
			this.label8.Location = new System.Drawing.Point(424, 34);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(72, 21);
			this.label8.TabIndex = 46;
			this.label8.Text = "���������";
			this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// ebMedia
			// 
			this.ebMedia.BackColor = System.Drawing.Color.WhiteSmoke;
			this.ebMedia.Location = new System.Drawing.Point(80, 34);
			this.ebMedia.MaxLength = 40;
			this.ebMedia.Name = "ebMedia";
			this.ebMedia.ReadOnly = true;
			this.ebMedia.Size = new System.Drawing.Size(120, 21);
			this.ebMedia.TabIndex = 0;
			this.ebMedia.TabStop = false;
			this.ebMedia.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
			this.ebMedia.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
			// 
			// label9
			// 
			this.label9.BackColor = System.Drawing.Color.Transparent;
			this.label9.Location = new System.Drawing.Point(8, 34);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(56, 21);
			this.label9.TabIndex = 18;
			this.label9.Text = "��ü";
			this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// ebContractState
			// 
			this.ebContractState.BackColor = System.Drawing.Color.WhiteSmoke;
			this.ebContractState.Location = new System.Drawing.Point(496, 10);
			this.ebContractState.MaxLength = 40;
			this.ebContractState.Name = "ebContractState";
			this.ebContractState.ReadOnly = true;
			this.ebContractState.Size = new System.Drawing.Size(120, 21);
			this.ebContractState.TabIndex = 0;
			this.ebContractState.TabStop = false;
			this.ebContractState.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
			this.ebContractState.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
			// 
			// label11
			// 
			this.label11.BackColor = System.Drawing.Color.Transparent;
			this.label11.Location = new System.Drawing.Point(8, 58);
			this.label11.Name = "label11";
			this.label11.Size = new System.Drawing.Size(56, 21);
			this.label11.TabIndex = 18;
			this.label11.Text = "����";
			this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label12
			// 
			this.label12.BackColor = System.Drawing.Color.Transparent;
			this.label12.Location = new System.Drawing.Point(216, 58);
			this.label12.Name = "label12";
			this.label12.Size = new System.Drawing.Size(72, 21);
			this.label12.TabIndex = 46;
			this.label12.Text = "���";
			this.label12.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// tabPageHistory
			// 
			this.tabPageHistory.Controls.Add(this.ebAdRate2);
			this.tabPageHistory.Controls.Add(this.lbItemName2);
			this.tabPageHistory.Controls.Add(this.pnlHistory);
			this.tabPageHistory.Controls.Add(this.lbAdClass2);
			this.tabPageHistory.Controls.Add(this.lbScheduleType2);
			this.tabPageHistory.Controls.Add(this.ebExcuteEndDay2);
			this.tabPageHistory.Controls.Add(this.ebScheduleType2);
			this.tabPageHistory.Controls.Add(this.lbAdState2);
			this.tabPageHistory.Controls.Add(this.lbAdRate2);
			this.tabPageHistory.Controls.Add(this.ebAdState2);
			this.tabPageHistory.Controls.Add(this.lbExcuteStartDay2);
			this.tabPageHistory.Controls.Add(this.ebExcuteStartDay2);
			this.tabPageHistory.Controls.Add(this.ebAdTime2);
			this.tabPageHistory.Controls.Add(this.ebAdClass2);
			this.tabPageHistory.Controls.Add(this.ebAdType2);
			this.tabPageHistory.Controls.Add(this.ebItemName2);
			this.tabPageHistory.Controls.Add(this.lbExcuteEndDay2);
			this.tabPageHistory.Controls.Add(this.lbAdType2);
			this.tabPageHistory.Controls.Add(this.lbAdTime2);
			this.tabPageHistory.Location = new System.Drawing.Point(1, 22);
			this.tabPageHistory.Name = "tabPageHistory";
			this.tabPageHistory.Size = new System.Drawing.Size(989, 163);
			this.tabPageHistory.TabStop = true;
			this.tabPageHistory.Text = "�����̷�";
			this.tabPageHistory.Visible = false;
			// 
			// ebAdRate2
			// 
			this.ebAdRate2.BackColor = System.Drawing.Color.WhiteSmoke;
			this.ebAdRate2.Location = new System.Drawing.Point(696, 80);
			this.ebAdRate2.MaxLength = 40;
			this.ebAdRate2.Name = "ebAdRate2";
			this.ebAdRate2.ReadOnly = true;
			this.ebAdRate2.Size = new System.Drawing.Size(120, 21);
			this.ebAdRate2.TabIndex = 0;
			this.ebAdRate2.TabStop = false;
			this.ebAdRate2.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
			this.ebAdRate2.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
			// 
			// lbItemName2
			// 
			this.lbItemName2.BackColor = System.Drawing.Color.Transparent;
			this.lbItemName2.Location = new System.Drawing.Point(424, 8);
			this.lbItemName2.Name = "lbItemName2";
			this.lbItemName2.Size = new System.Drawing.Size(57, 21);
			this.lbItemName2.TabIndex = 110;
			this.lbItemName2.Text = "�����";
			this.lbItemName2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// pnlHistory
			// 
			this.pnlHistory.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.pnlHistory.Controls.Add(this.grdExContactHistory);
			this.pnlHistory.Location = new System.Drawing.Point(8, 8);
			this.pnlHistory.Name = "pnlHistory";
			this.pnlHistory.Size = new System.Drawing.Size(408, 120);
			this.pnlHistory.TabIndex = 109;
			// 
			// grdExContactHistory
			// 
			this.grdExContactHistory.AllowEdit = Janus.Windows.GridEX.InheritableBoolean.False;
			this.grdExContactHistory.AlternatingColors = true;
			this.grdExContactHistory.BorderStyle = Janus.Windows.GridEX.BorderStyle.None;
			this.grdExContactHistory.DataSource = this.dvContractHistory;
			grdExContactHistory_DesignTimeLayout.LayoutString = resources.GetString("grdExContactHistory_DesignTimeLayout.LayoutString");
			this.grdExContactHistory.DesignTimeLayout = grdExContactHistory_DesignTimeLayout;
			this.grdExContactHistory.Dock = System.Windows.Forms.DockStyle.Fill;
			this.grdExContactHistory.EditorsControlStyle.ButtonAppearance = Janus.Windows.GridEX.ButtonAppearance.Regular;
			this.grdExContactHistory.FocusStyle = Janus.Windows.GridEX.FocusStyle.None;
			this.grdExContactHistory.GridLineColor = System.Drawing.Color.Silver;
			this.grdExContactHistory.GridLines = Janus.Windows.GridEX.GridLines.Vertical;
			this.grdExContactHistory.GridLineStyle = Janus.Windows.GridEX.GridLineStyle.Solid;
			this.grdExContactHistory.GroupByBoxVisible = false;
			this.grdExContactHistory.HeaderFormatStyle.TextAlignment = Janus.Windows.GridEX.TextAlignment.Center;
			this.grdExContactHistory.HideSelection = Janus.Windows.GridEX.HideSelection.HighlightInactive;
			this.grdExContactHistory.Location = new System.Drawing.Point(0, 0);
			this.grdExContactHistory.Name = "grdExContactHistory";
			this.grdExContactHistory.ScrollBars = Janus.Windows.GridEX.ScrollBars.Vertical;
			this.grdExContactHistory.SelectedInactiveFormatStyle.BackColor = System.Drawing.Color.Gold;
			this.grdExContactHistory.Size = new System.Drawing.Size(406, 118);
			this.grdExContactHistory.TabIndex = 0;
			this.grdExContactHistory.TabKeyBehavior = Janus.Windows.GridEX.TabKeyBehavior.ControlNavigation;
			this.grdExContactHistory.TabStop = false;
			this.grdExContactHistory.ThemedAreas = ((Janus.Windows.GridEX.ThemedArea)(((((((((Janus.Windows.GridEX.ThemedArea.ScrollBars | Janus.Windows.GridEX.ThemedArea.EditControls) 
            | Janus.Windows.GridEX.ThemedArea.Headers) 
            | Janus.Windows.GridEX.ThemedArea.GroupByBox) 
            | Janus.Windows.GridEX.ThemedArea.GroupRows) 
            | Janus.Windows.GridEX.ThemedArea.ControlBorder) 
            | Janus.Windows.GridEX.ThemedArea.Cards) 
            | Janus.Windows.GridEX.ThemedArea.Gridlines) 
            | Janus.Windows.GridEX.ThemedArea.CheckBoxes)));
			this.grdExContactHistory.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
			this.grdExContactHistory.Enter += new System.EventHandler(this.OnGrdDetailRowChanged);
			// 
			// dvContractHistory
			// 
			this.dvContractHistory.Table = this.contractItemDs.ContractItemHistory;
			// 
			// lbAdClass2
			// 
			this.lbAdClass2.BackColor = System.Drawing.Color.Transparent;
			this.lbAdClass2.Location = new System.Drawing.Point(424, 80);
			this.lbAdClass2.Name = "lbAdClass2";
			this.lbAdClass2.Size = new System.Drawing.Size(56, 21);
			this.lbAdClass2.TabIndex = 107;
			this.lbAdClass2.Text = "����뵵";
			this.lbAdClass2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// lbScheduleType2
			// 
			this.lbScheduleType2.BackColor = System.Drawing.Color.Transparent;
			this.lbScheduleType2.Location = new System.Drawing.Point(424, 56);
			this.lbScheduleType2.Name = "lbScheduleType2";
			this.lbScheduleType2.Size = new System.Drawing.Size(56, 21);
			this.lbScheduleType2.TabIndex = 108;
			this.lbScheduleType2.Text = "������";
			this.lbScheduleType2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// ebExcuteEndDay2
			// 
			this.ebExcuteEndDay2.BackColor = System.Drawing.Color.WhiteSmoke;
			this.ebExcuteEndDay2.Location = new System.Drawing.Point(696, 32);
			this.ebExcuteEndDay2.Name = "ebExcuteEndDay2";
			this.ebExcuteEndDay2.ReadOnly = true;
			this.ebExcuteEndDay2.Size = new System.Drawing.Size(120, 21);
			this.ebExcuteEndDay2.TabIndex = 0;
			this.ebExcuteEndDay2.TabStop = false;
			this.ebExcuteEndDay2.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
			this.ebExcuteEndDay2.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
			// 
			// ebScheduleType2
			// 
			this.ebScheduleType2.BackColor = System.Drawing.Color.WhiteSmoke;
			this.ebScheduleType2.Location = new System.Drawing.Point(488, 56);
			this.ebScheduleType2.MaxLength = 15;
			this.ebScheduleType2.Name = "ebScheduleType2";
			this.ebScheduleType2.ReadOnly = true;
			this.ebScheduleType2.Size = new System.Drawing.Size(104, 21);
			this.ebScheduleType2.TabIndex = 0;
			this.ebScheduleType2.TabStop = false;
			this.ebScheduleType2.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
			this.ebScheduleType2.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
			// 
			// lbAdState2
			// 
			this.lbAdState2.BackColor = System.Drawing.Color.Transparent;
			this.lbAdState2.Location = new System.Drawing.Point(632, 58);
			this.lbAdState2.Name = "lbAdState2";
			this.lbAdState2.Size = new System.Drawing.Size(56, 21);
			this.lbAdState2.TabIndex = 97;
			this.lbAdState2.Text = "�������";
			this.lbAdState2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// lbAdRate2
			// 
			this.lbAdRate2.BackColor = System.Drawing.Color.Transparent;
			this.lbAdRate2.Location = new System.Drawing.Point(632, 82);
			this.lbAdRate2.Name = "lbAdRate2";
			this.lbAdRate2.Size = new System.Drawing.Size(56, 21);
			this.lbAdRate2.TabIndex = 94;
			this.lbAdRate2.Text = "������";
			this.lbAdRate2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// ebAdState2
			// 
			this.ebAdState2.BackColor = System.Drawing.Color.WhiteSmoke;
			this.ebAdState2.Location = new System.Drawing.Point(696, 56);
			this.ebAdState2.MaxLength = 40;
			this.ebAdState2.Name = "ebAdState2";
			this.ebAdState2.ReadOnly = true;
			this.ebAdState2.Size = new System.Drawing.Size(120, 21);
			this.ebAdState2.TabIndex = 0;
			this.ebAdState2.TabStop = false;
			this.ebAdState2.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
			this.ebAdState2.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
			// 
			// lbExcuteStartDay2
			// 
			this.lbExcuteStartDay2.BackColor = System.Drawing.Color.Transparent;
			this.lbExcuteStartDay2.Location = new System.Drawing.Point(424, 32);
			this.lbExcuteStartDay2.Name = "lbExcuteStartDay2";
			this.lbExcuteStartDay2.Size = new System.Drawing.Size(57, 21);
			this.lbExcuteStartDay2.TabIndex = 105;
			this.lbExcuteStartDay2.Text = "�������";
			this.lbExcuteStartDay2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// ebExcuteStartDay2
			// 
			this.ebExcuteStartDay2.BackColor = System.Drawing.Color.WhiteSmoke;
			this.ebExcuteStartDay2.Location = new System.Drawing.Point(488, 32);
			this.ebExcuteStartDay2.Name = "ebExcuteStartDay2";
			this.ebExcuteStartDay2.ReadOnly = true;
			this.ebExcuteStartDay2.Size = new System.Drawing.Size(104, 21);
			this.ebExcuteStartDay2.TabIndex = 0;
			this.ebExcuteStartDay2.TabStop = false;
			this.ebExcuteStartDay2.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
			this.ebExcuteStartDay2.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
			// 
			// ebAdTime2
			// 
			this.ebAdTime2.BackColor = System.Drawing.Color.WhiteSmoke;
			this.ebAdTime2.Location = new System.Drawing.Point(696, 104);
			this.ebAdTime2.Name = "ebAdTime2";
			this.ebAdTime2.ReadOnly = true;
			this.ebAdTime2.Size = new System.Drawing.Size(120, 21);
			this.ebAdTime2.TabIndex = 0;
			this.ebAdTime2.TabStop = false;
			this.ebAdTime2.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
			this.ebAdTime2.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
			// 
			// ebAdClass2
			// 
			this.ebAdClass2.BackColor = System.Drawing.Color.WhiteSmoke;
			this.ebAdClass2.Location = new System.Drawing.Point(488, 80);
			this.ebAdClass2.MaxLength = 15;
			this.ebAdClass2.Name = "ebAdClass2";
			this.ebAdClass2.ReadOnly = true;
			this.ebAdClass2.Size = new System.Drawing.Size(104, 21);
			this.ebAdClass2.TabIndex = 0;
			this.ebAdClass2.TabStop = false;
			this.ebAdClass2.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
			this.ebAdClass2.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
			// 
			// ebAdType2
			// 
			this.ebAdType2.BackColor = System.Drawing.Color.WhiteSmoke;
			this.ebAdType2.Location = new System.Drawing.Point(488, 104);
			this.ebAdType2.MaxLength = 15;
			this.ebAdType2.Name = "ebAdType2";
			this.ebAdType2.ReadOnly = true;
			this.ebAdType2.Size = new System.Drawing.Size(104, 21);
			this.ebAdType2.TabIndex = 0;
			this.ebAdType2.TabStop = false;
			this.ebAdType2.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
			this.ebAdType2.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
			// 
			// ebItemName2
			// 
			this.ebItemName2.BackColor = System.Drawing.Color.WhiteSmoke;
			this.ebItemName2.Location = new System.Drawing.Point(488, 8);
			this.ebItemName2.Name = "ebItemName2";
			this.ebItemName2.ReadOnly = true;
			this.ebItemName2.Size = new System.Drawing.Size(328, 21);
			this.ebItemName2.TabIndex = 0;
			this.ebItemName2.TabStop = false;
			this.ebItemName2.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
			this.ebItemName2.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
			// 
			// lbExcuteEndDay2
			// 
			this.lbExcuteEndDay2.BackColor = System.Drawing.Color.Transparent;
			this.lbExcuteEndDay2.Location = new System.Drawing.Point(632, 34);
			this.lbExcuteEndDay2.Name = "lbExcuteEndDay2";
			this.lbExcuteEndDay2.Size = new System.Drawing.Size(56, 21);
			this.lbExcuteEndDay2.TabIndex = 97;
			this.lbExcuteEndDay2.Text = "��������";
			this.lbExcuteEndDay2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// lbAdType2
			// 
			this.lbAdType2.BackColor = System.Drawing.Color.Transparent;
			this.lbAdType2.Location = new System.Drawing.Point(424, 104);
			this.lbAdType2.Name = "lbAdType2";
			this.lbAdType2.Size = new System.Drawing.Size(56, 21);
			this.lbAdType2.TabIndex = 107;
			this.lbAdType2.Text = "��������";
			this.lbAdType2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// lbAdTime2
			// 
			this.lbAdTime2.BackColor = System.Drawing.Color.Transparent;
			this.lbAdTime2.Location = new System.Drawing.Point(632, 106);
			this.lbAdTime2.Name = "lbAdTime2";
			this.lbAdTime2.Size = new System.Drawing.Size(56, 21);
			this.lbAdTime2.TabIndex = 94;
			this.lbAdTime2.Text = "�������";
			this.lbAdTime2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// btnDelete
			// 
			this.btnDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.btnDelete.Enabled = false;
			this.btnDelete.Location = new System.Drawing.Point(88, 206);
			this.btnDelete.Name = "btnDelete";
			this.btnDelete.Size = new System.Drawing.Size(72, 24);
			this.btnDelete.TabIndex = 30;
			this.btnDelete.Text = "�� ��";
			this.btnDelete.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
			this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
			// 
			// btnAdd
			// 
			this.btnAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.btnAdd.Enabled = false;
			this.btnAdd.Location = new System.Drawing.Point(168, 206);
			this.btnAdd.Name = "btnAdd";
			this.btnAdd.Size = new System.Drawing.Size(72, 24);
			this.btnAdd.TabIndex = 31;
			this.btnAdd.Text = "�� ��";
			this.btnAdd.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
			this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
			// 
			// btnSave
			// 
			this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.btnSave.Enabled = false;
			this.btnSave.Location = new System.Drawing.Point(8, 206);
			this.btnSave.Name = "btnSave";
			this.btnSave.Size = new System.Drawing.Size(72, 24);
			this.btnSave.TabIndex = 29;
			this.btnSave.Text = "�� ��";
			this.btnSave.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
			this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
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
			// lbContModDt
			// 
			this.lbContModDt.BackColor = System.Drawing.Color.Transparent;
			this.lbContModDt.Font = new System.Drawing.Font("����ü", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
			this.lbContModDt.Location = new System.Drawing.Point(632, 34);
			this.lbContModDt.Name = "lbContModDt";
			this.lbContModDt.Size = new System.Drawing.Size(56, 21);
			this.lbContModDt.TabIndex = 117;
			this.lbContModDt.Text = "��������";
			// 
			// lbContRegName
			// 
			this.lbContRegName.BackColor = System.Drawing.Color.Transparent;
			this.lbContRegName.Font = new System.Drawing.Font("����ü", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
			this.lbContRegName.Location = new System.Drawing.Point(632, 59);
			this.lbContRegName.Name = "lbContRegName";
			this.lbContRegName.Size = new System.Drawing.Size(48, 14);
			this.lbContRegName.TabIndex = 118;
			this.lbContRegName.Text = "�����";
			// 
			// lbContRegDt
			// 
			this.lbContRegDt.BackColor = System.Drawing.Color.Transparent;
			this.lbContRegDt.Font = new System.Drawing.Font("����ü", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
			this.lbContRegDt.Location = new System.Drawing.Point(632, 10);
			this.lbContRegDt.Name = "lbContRegDt";
			this.lbContRegDt.Size = new System.Drawing.Size(56, 21);
			this.lbContRegDt.TabIndex = 116;
			this.lbContRegDt.Text = "����Ͻ�";
			// 
			// lbAgency
			// 
			this.lbAgency.BackColor = System.Drawing.Color.Transparent;
			this.lbAgency.Font = new System.Drawing.Font("����ü", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
			this.lbAgency.Location = new System.Drawing.Point(8, 82);
			this.lbAgency.Name = "lbAgency";
			this.lbAgency.Size = new System.Drawing.Size(56, 21);
			this.lbAgency.TabIndex = 18;
			this.lbAgency.Text = "�����";
			// 
			// lbContractState
			// 
			this.lbContractState.BackColor = System.Drawing.Color.Transparent;
			this.lbContractState.Font = new System.Drawing.Font("����ü", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
			this.lbContractState.Location = new System.Drawing.Point(424, 10);
			this.lbContractState.Name = "lbContractState";
			this.lbContractState.Size = new System.Drawing.Size(56, 21);
			this.lbContractState.TabIndex = 18;
			this.lbContractState.Text = "��������";
			// 
			// lbContStartDay
			// 
			this.lbContStartDay.BackColor = System.Drawing.Color.Transparent;
			this.lbContStartDay.Font = new System.Drawing.Font("����ü", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
			this.lbContStartDay.Location = new System.Drawing.Point(216, 34);
			this.lbContStartDay.Name = "lbContStartDay";
			this.lbContStartDay.Size = new System.Drawing.Size(72, 21);
			this.lbContStartDay.TabIndex = 46;
			this.lbContStartDay.Text = "����������";
			// 
			// lbContractName2
			// 
			this.lbContractName2.BackColor = System.Drawing.Color.Transparent;
			this.lbContractName2.Font = new System.Drawing.Font("����ü", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
			this.lbContractName2.Location = new System.Drawing.Point(8, 10);
			this.lbContractName2.Name = "lbContractName2";
			this.lbContractName2.Size = new System.Drawing.Size(48, 21);
			this.lbContractName2.TabIndex = 18;
			this.lbContractName2.Text = "������";
			// 
			// lbContEndDay
			// 
			this.lbContEndDay.BackColor = System.Drawing.Color.Transparent;
			this.lbContEndDay.Font = new System.Drawing.Font("����ü", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
			this.lbContEndDay.Location = new System.Drawing.Point(424, 34);
			this.lbContEndDay.Name = "lbContEndDay";
			this.lbContEndDay.Size = new System.Drawing.Size(72, 21);
			this.lbContEndDay.TabIndex = 46;
			this.lbContEndDay.Text = "����������";
			// 
			// lbMedia
			// 
			this.lbMedia.BackColor = System.Drawing.Color.Transparent;
			this.lbMedia.Font = new System.Drawing.Font("����ü", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
			this.lbMedia.Location = new System.Drawing.Point(8, 34);
			this.lbMedia.Name = "lbMedia";
			this.lbMedia.Size = new System.Drawing.Size(56, 21);
			this.lbMedia.TabIndex = 18;
			this.lbMedia.Text = "��ü";
			// 
			// lbRap
			// 
			this.lbRap.BackColor = System.Drawing.Color.Transparent;
			this.lbRap.Font = new System.Drawing.Font("����ü", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
			this.lbRap.Location = new System.Drawing.Point(8, 58);
			this.lbRap.Name = "lbRap";
			this.lbRap.Size = new System.Drawing.Size(56, 21);
			this.lbRap.TabIndex = 18;
			this.lbRap.Text = "����";
			// 
			// lbComment
			// 
			this.lbComment.BackColor = System.Drawing.Color.Transparent;
			this.lbComment.Font = new System.Drawing.Font("����ü", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
			this.lbComment.Location = new System.Drawing.Point(216, 58);
			this.lbComment.Name = "lbComment";
			this.lbComment.Size = new System.Drawing.Size(72, 21);
			this.lbComment.TabIndex = 46;
			this.lbComment.Text = "���";
			// 
			// cmdManager
			// 
			this.cmdManager.BottomRebar = this.BottomRebar1;
			this.cmdManager.Commands.AddRange(new Janus.Windows.UI.CommandBars.UICommand[] {
            this.SetAdState});
			this.cmdManager.ContainerControl = this;
			this.cmdManager.ContextMenus.AddRange(new Janus.Windows.UI.CommandBars.UIContextMenu[] {
            this.ctxMenu});
			this.cmdManager.Id = new System.Guid("16c4f683-0ac5-47ec-8b1f-85b50170275a");
			this.cmdManager.LeftRebar = this.LeftRebar1;
			this.cmdManager.RightRebar = this.RightRebar1;
			this.cmdManager.TopRebar = this.TopRebar1;
			// 
			// BottomRebar1
			// 
			this.BottomRebar1.CommandManager = this.cmdManager;
			this.BottomRebar1.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.BottomRebar1.Location = new System.Drawing.Point(0, 0);
			this.BottomRebar1.Name = "BottomRebar1";
			this.BottomRebar1.Size = new System.Drawing.Size(0, 0);
			// 
			// SetAdState
			// 
			this.SetAdState.CommandType = Janus.Windows.UI.CommandBars.CommandType.ComboBoxCommand;
			this.SetAdState.IsEditableControl = Janus.Windows.UI.InheritableBoolean.True;
			this.SetAdState.Key = "SetAdState";
			this.SetAdState.Name = "SetAdState";
			this.SetAdState.Text = "�������(���߼���)";
			// 
			// ctxMenu
			// 
			this.ctxMenu.CommandManager = this.cmdManager;
			this.ctxMenu.Commands.AddRange(new Janus.Windows.UI.CommandBars.UICommand[] {
            this.SetAdState1});
			this.ctxMenu.Key = "ctxMenu";
			this.ctxMenu.VisualStyle = Janus.Windows.UI.VisualStyle.Office2003;
			// 
			// SetAdState1
			// 
			this.SetAdState1.Key = "SetAdState";
			this.SetAdState1.Name = "SetAdState1";
			// 
			// LeftRebar1
			// 
			this.LeftRebar1.CommandManager = this.cmdManager;
			this.LeftRebar1.Dock = System.Windows.Forms.DockStyle.Left;
			this.LeftRebar1.Location = new System.Drawing.Point(0, 0);
			this.LeftRebar1.Name = "LeftRebar1";
			this.LeftRebar1.Size = new System.Drawing.Size(0, 0);
			// 
			// RightRebar1
			// 
			this.RightRebar1.CommandManager = this.cmdManager;
			this.RightRebar1.Dock = System.Windows.Forms.DockStyle.Right;
			this.RightRebar1.Location = new System.Drawing.Point(0, 0);
			this.RightRebar1.Name = "RightRebar1";
			this.RightRebar1.Size = new System.Drawing.Size(0, 0);
			// 
			// TopRebar1
			// 
			this.TopRebar1.CommandManager = this.cmdManager;
			this.TopRebar1.Dock = System.Windows.Forms.DockStyle.Top;
			this.TopRebar1.Location = new System.Drawing.Point(0, 0);
			this.TopRebar1.Name = "TopRebar1";
			this.TopRebar1.Size = new System.Drawing.Size(1010, 0);
			// 
			// toolTip1
			// 
			this.toolTip1.IsBalloon = true;
			this.toolTip1.ToolTipIcon = System.Windows.Forms.ToolTipIcon.Info;
			this.toolTip1.ToolTipTitle = "�׸񼳸�";
			// 
			// uiTabPage1
			// 
			this.uiTabPage1.Location = new System.Drawing.Point(1, 22);
			this.uiTabPage1.Name = "uiTabPage1";
			this.uiTabPage1.Size = new System.Drawing.Size(989, 128);
			this.uiTabPage1.TabStop = true;
			this.uiTabPage1.TabVisible = false;
			this.uiTabPage1.Visible = false;
			// 
			// tabBanner
			// 
			this.tabBanner.Controls.Add(this.picAdFile);
			this.tabBanner.Controls.Add(this.gbBanner);
			this.tabBanner.Controls.Add(this.uiGroupBox1);
			this.tabBanner.Controls.Add(this.cbBannerOrd);
			this.tabBanner.Controls.Add(this.label15);
			this.tabBanner.Controls.Add(this.gbViewLoc);
			this.tabBanner.Controls.Add(this.gbLinkType);
			this.tabBanner.Location = new System.Drawing.Point(1, 22);
			this.tabBanner.Name = "tabBanner";
			this.tabBanner.Size = new System.Drawing.Size(989, 163);
			this.tabBanner.TabStop = true;
			this.tabBanner.Text = "��ʼ���";
			this.toolTip1.SetToolTip(this.tabBanner, "��ʱ����� ��� Ȱ��ȭ �˴ϴ�");
			// 
			// rbLink0
			// 
			this.rbLink0.AutoSize = true;
			this.rbLink0.Location = new System.Drawing.Point(12, 17);
			this.rbLink0.Name = "rbLink0";
			this.rbLink0.Size = new System.Drawing.Size(65, 18);
			this.rbLink0.TabIndex = 88;
			this.rbLink0.Text = "��ũ����";
			this.rbLink0.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
			// 
			// gbLinkType
			// 
			this.gbLinkType.BackColor = System.Drawing.Color.Transparent;
			this.gbLinkType.Controls.Add(this.ebLinkContent);
			this.gbLinkType.Controls.Add(this.rbLink2);
			this.gbLinkType.Controls.Add(this.rbLink1);
			this.gbLinkType.Controls.Add(this.rbLink0);
			this.gbLinkType.Location = new System.Drawing.Point(12, 4);
			this.gbLinkType.Name = "gbLinkType";
			this.gbLinkType.Size = new System.Drawing.Size(802, 40);
			this.gbLinkType.TabIndex = 89;
			this.gbLinkType.Text = "��ũ Ÿ��";
			this.gbLinkType.VisualStyle = Janus.Windows.UI.Dock.PanelVisualStyle.Office2007;
			// 
			// rbLink1
			// 
			this.rbLink1.AutoSize = true;
			this.rbLink1.Location = new System.Drawing.Point(103, 17);
			this.rbLink1.Name = "rbLink1";
			this.rbLink1.Size = new System.Drawing.Size(43, 18);
			this.rbLink1.TabIndex = 89;
			this.rbLink1.Text = "URL";
			this.rbLink1.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
			// 
			// rbLink2
			// 
			this.rbLink2.AutoSize = true;
			this.rbLink2.Location = new System.Drawing.Point(172, 17);
			this.rbLink2.Name = "rbLink2";
			this.rbLink2.Size = new System.Drawing.Size(65, 18);
			this.rbLink2.TabIndex = 90;
			this.rbLink2.Text = "�ó�ý�";
			this.rbLink2.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
			// 
			// ebLinkContent
			// 
			this.ebLinkContent.Location = new System.Drawing.Point(266, 16);
			this.ebLinkContent.MaxLength = 50;
			this.ebLinkContent.Name = "ebLinkContent";
			this.ebLinkContent.Size = new System.Drawing.Size(530, 21);
			this.ebLinkContent.TabIndex = 91;
			this.ebLinkContent.Text = "URL Ȥ�� ������ID�� �Է��ϼ���";
			this.ebLinkContent.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
			this.ebLinkContent.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
			// 
			// gbViewLoc
			// 
			this.gbViewLoc.BackColor = System.Drawing.Color.Transparent;
			this.gbViewLoc.Controls.Add(this.rbView6);
			this.gbViewLoc.Controls.Add(this.rbView5);
			this.gbViewLoc.Controls.Add(this.rbView4);
			this.gbViewLoc.Controls.Add(this.rbView3);
			this.gbViewLoc.Controls.Add(this.rbView2);
			this.gbViewLoc.Controls.Add(this.rbView1);
			this.gbViewLoc.Location = new System.Drawing.Point(12, 46);
			this.gbViewLoc.Name = "gbViewLoc";
			this.gbViewLoc.Size = new System.Drawing.Size(518, 40);
			this.gbViewLoc.TabIndex = 90;
			this.gbViewLoc.Text = "���� ����";
			this.gbViewLoc.VisualStyle = Janus.Windows.UI.Dock.PanelVisualStyle.Office2007;
			// 
			// rbView3
			// 
			this.rbView3.AutoSize = true;
			this.rbView3.Location = new System.Drawing.Point(194, 18);
			this.rbView3.Name = "rbView3";
			this.rbView3.Size = new System.Drawing.Size(76, 18);
			this.rbView3.TabIndex = 90;
			this.rbView3.Text = "���޼���";
			this.rbView3.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
			// 
			// rbView2
			// 
			this.rbView2.AutoSize = true;
			this.rbView2.Location = new System.Drawing.Point(125, 18);
			this.rbView2.Name = "rbView2";
			this.rbView2.Size = new System.Drawing.Size(45, 18);
			this.rbView2.TabIndex = 89;
			this.rbView2.Text = "VoD";
			this.rbView2.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
			// 
			// rbView1
			// 
			this.rbView1.AutoSize = true;
			this.rbView1.Location = new System.Drawing.Point(12, 18);
			this.rbView1.Name = "rbView1";
			this.rbView1.Size = new System.Drawing.Size(89, 18);
			this.rbView1.TabIndex = 88;
			this.rbView1.Text = "VoD���θ��";
			this.rbView1.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
			// 
			// rbView6
			// 
			this.rbView6.AutoSize = true;
			this.rbView6.Location = new System.Drawing.Point(456, 18);
			this.rbView6.Name = "rbView6";
			this.rbView6.Size = new System.Drawing.Size(45, 18);
			this.rbView6.TabIndex = 93;
			this.rbView6.Text = "Cast";
			this.rbView6.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
			// 
			// rbView5
			// 
			this.rbView5.AutoSize = true;
			this.rbView5.Location = new System.Drawing.Point(389, 18);
			this.rbView5.Name = "rbView5";
			this.rbView5.Size = new System.Drawing.Size(43, 18);
			this.rbView5.TabIndex = 92;
			this.rbView5.Text = "Live";
			this.rbView5.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
			// 
			// rbView4
			// 
			this.rbView4.AutoSize = true;
			this.rbView4.Location = new System.Drawing.Point(294, 18);
			this.rbView4.Name = "rbView4";
			this.rbView4.Size = new System.Drawing.Size(71, 18);
			this.rbView4.TabIndex = 91;
			this.rbView4.Text = "my�귣��";
			this.rbView4.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
			// 
			// label15
			// 
			this.label15.BackColor = System.Drawing.Color.Transparent;
			this.label15.Location = new System.Drawing.Point(840, 21);
			this.label15.Name = "label15";
			this.label15.Size = new System.Drawing.Size(56, 21);
			this.label15.TabIndex = 91;
			this.label15.Text = "�켱����";
			this.label15.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// cbBannerOrd
			// 
			this.cbBannerOrd.BackColor = System.Drawing.Color.White;
			this.cbBannerOrd.ComboStyle = Janus.Windows.EditControls.ComboStyle.DropDownList;
			uiComboBoxItem5.FormatStyle.Alpha = 0;
			uiComboBoxItem5.IsSeparator = false;
			uiComboBoxItem5.Text = "1����";
			uiComboBoxItem5.Value = "1";
			uiComboBoxItem6.FormatStyle.Alpha = 0;
			uiComboBoxItem6.IsSeparator = false;
			uiComboBoxItem6.Text = "2����";
			uiComboBoxItem6.Value = "2";
			uiComboBoxItem7.FormatStyle.Alpha = 0;
			uiComboBoxItem7.IsSeparator = false;
			uiComboBoxItem7.Text = "3����";
			uiComboBoxItem7.Value = "3";
			uiComboBoxItem8.FormatStyle.Alpha = 0;
			uiComboBoxItem8.IsSeparator = false;
			uiComboBoxItem8.Text = "4����";
			uiComboBoxItem8.Value = "4";
			uiComboBoxItem9.FormatStyle.Alpha = 0;
			uiComboBoxItem9.IsSeparator = false;
			uiComboBoxItem9.Text = "5����";
			uiComboBoxItem9.Value = "5";
			uiComboBoxItem10.FormatStyle.Alpha = 0;
			uiComboBoxItem10.IsSeparator = false;
			uiComboBoxItem10.Text = "6����";
			uiComboBoxItem10.Value = "7";
			uiComboBoxItem11.FormatStyle.Alpha = 0;
			uiComboBoxItem11.IsSeparator = false;
			uiComboBoxItem11.Text = "6����";
			uiComboBoxItem11.Value = "6";
			uiComboBoxItem12.FormatStyle.Alpha = 0;
			uiComboBoxItem12.IsSeparator = false;
			uiComboBoxItem12.Text = "8����";
			uiComboBoxItem12.Value = "8";
			uiComboBoxItem13.FormatStyle.Alpha = 0;
			uiComboBoxItem13.IsSeparator = false;
			uiComboBoxItem13.Text = "9����";
			uiComboBoxItem13.Value = "9";
			this.cbBannerOrd.Items.AddRange(new Janus.Windows.EditControls.UIComboBoxItem[] {
            uiComboBoxItem5,
            uiComboBoxItem6,
            uiComboBoxItem7,
            uiComboBoxItem8,
            uiComboBoxItem9,
            uiComboBoxItem10,
            uiComboBoxItem11,
            uiComboBoxItem12,
            uiComboBoxItem13});
			this.cbBannerOrd.Location = new System.Drawing.Point(900, 20);
			this.cbBannerOrd.Name = "cbBannerOrd";
			this.cbBannerOrd.Size = new System.Drawing.Size(66, 21);
			this.cbBannerOrd.Sorted = true;
			this.cbBannerOrd.TabIndex = 92;
			this.cbBannerOrd.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
			// 
			// uiGroupBox1
			// 
			this.uiGroupBox1.BackColor = System.Drawing.Color.Transparent;
			this.uiGroupBox1.Controls.Add(this.chkIPad);
			this.uiGroupBox1.Controls.Add(this.chkAndroidTab);
			this.uiGroupBox1.Controls.Add(this.chkIphone);
			this.uiGroupBox1.Controls.Add(this.chkAndroid);
			this.uiGroupBox1.Location = new System.Drawing.Point(534, 46);
			this.uiGroupBox1.Name = "uiGroupBox1";
			this.uiGroupBox1.Size = new System.Drawing.Size(446, 40);
			this.uiGroupBox1.TabIndex = 93;
			this.uiGroupBox1.Text = "����̽� ����";
			this.uiGroupBox1.VisualStyle = Janus.Windows.UI.Dock.PanelVisualStyle.Office2007;
			// 
			// chkAndroid
			// 
			this.chkAndroid.AutoSize = true;
			this.chkAndroid.Location = new System.Drawing.Point(12, 18);
			this.chkAndroid.Name = "chkAndroid";
			this.chkAndroid.Size = new System.Drawing.Size(65, 18);
			this.chkAndroid.TabIndex = 0;
			this.chkAndroid.Text = "Android";
			this.chkAndroid.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
			// 
			// chkIphone
			// 
			this.chkIphone.AutoSize = true;
			this.chkIphone.Location = new System.Drawing.Point(115, 18);
			this.chkIphone.Name = "chkIphone";
			this.chkIphone.Size = new System.Drawing.Size(59, 18);
			this.chkIphone.TabIndex = 1;
			this.chkIphone.Text = "iPhone";
			this.chkIphone.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
			// 
			// chkIPad
			// 
			this.chkIPad.AutoSize = true;
			this.chkIPad.Location = new System.Drawing.Point(353, 18);
			this.chkIPad.Name = "chkIPad";
			this.chkIPad.Size = new System.Drawing.Size(45, 18);
			this.chkIPad.TabIndex = 3;
			this.chkIPad.Text = "iPad";
			this.chkIPad.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
			// 
			// chkAndroidTab
			// 
			this.chkAndroidTab.AutoSize = true;
			this.chkAndroidTab.Location = new System.Drawing.Point(212, 18);
			this.chkAndroidTab.Name = "chkAndroidTab";
			this.chkAndroidTab.Size = new System.Drawing.Size(103, 18);
			this.chkAndroidTab.TabIndex = 2;
			this.chkAndroidTab.Text = "Android Tablet";
			this.chkAndroidTab.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
			// 
			// gbBanner
			// 
			this.gbBanner.BackColor = System.Drawing.Color.Transparent;
			this.gbBanner.Controls.Add(this.btnBannerFile);
			this.gbBanner.Controls.Add(this.label16);
			this.gbBanner.Controls.Add(this.ebBannerFileNm);
			this.gbBanner.Location = new System.Drawing.Point(12, 90);
			this.gbBanner.Name = "gbBanner";
			this.gbBanner.Size = new System.Drawing.Size(346, 70);
			this.gbBanner.TabIndex = 94;
			this.gbBanner.Text = "��� �̹���";
			this.gbBanner.VisualStyle = Janus.Windows.UI.Dock.PanelVisualStyle.Office2007;
			// 
			// ebBannerFileNm
			// 
			this.ebBannerFileNm.Location = new System.Drawing.Point(70, 18);
			this.ebBannerFileNm.MaxLength = 50;
			this.ebBannerFileNm.Name = "ebBannerFileNm";
			this.ebBannerFileNm.Size = new System.Drawing.Size(254, 21);
			this.ebBannerFileNm.TabIndex = 92;
			this.ebBannerFileNm.Text = "���ϸ��� �Է��ϼ���";
			this.ebBannerFileNm.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
			this.ebBannerFileNm.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
			// 
			// label16
			// 
			this.label16.BackColor = System.Drawing.Color.Transparent;
			this.label16.Location = new System.Drawing.Point(12, 18);
			this.label16.Name = "label16";
			this.label16.Size = new System.Drawing.Size(56, 21);
			this.label16.TabIndex = 93;
			this.label16.Text = "���ϸ�";
			this.label16.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// btnBannerFile
			// 
			this.btnBannerFile.Enabled = false;
			this.btnBannerFile.Location = new System.Drawing.Point(70, 42);
			this.btnBannerFile.Name = "btnBannerFile";
			this.btnBannerFile.Size = new System.Drawing.Size(100, 22);
			this.btnBannerFile.TabIndex = 94;
			this.btnBannerFile.Text = "�ű����ϼ���";
			this.btnBannerFile.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
			// 
			// picAdFile
			// 
			this.picAdFile.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.picAdFile.Image = ((System.Drawing.Image)(resources.GetObject("picAdFile.Image")));
			this.picAdFile.Location = new System.Drawing.Point(372, 94);
			this.picAdFile.Name = "picAdFile";
			this.picAdFile.Size = new System.Drawing.Size(368, 66);
			this.picAdFile.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
			this.picAdFile.TabIndex = 95;
			this.picAdFile.TabStop = false;
			// 
			// ContractItemControl
			// 
			this.Controls.Add(this.uiPanelContract);
			this.Controls.Add(this.TopRebar1);
			this.Font = global::AdManagerClient.Properties.Settings.Default.Font1;
			this.Name = "ContractItemControl";
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
			((System.ComponentModel.ISupportInitialize)(this.grdExItemList)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.dvContractItem)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.contractItemDs)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.uiPanelDetail)).EndInit();
			this.uiPanelDetail.ResumeLayout(false);
			this.uiPanelDetailContainer.ResumeLayout(false);
			this.pnlUserDetail.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.tabContractItem)).EndInit();
			this.tabContractItem.ResumeLayout(false);
			this.tabPageItem.ResumeLayout(false);
			this.tabPageItem.PerformLayout();
			this.tabPageFile.ResumeLayout(false);
			this.tabPageFile.PerformLayout();
			this.tabPageContract.ResumeLayout(false);
			this.tabPageContract.PerformLayout();
			this.tabPageHistory.ResumeLayout(false);
			this.tabPageHistory.PerformLayout();
			this.pnlHistory.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.grdExContactHistory)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.dvContractHistory)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.gridEX1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.cmdManager)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.BottomRebar1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.ctxMenu)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.LeftRebar1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.RightRebar1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.TopRebar1)).EndInit();
			this.tabBanner.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.gbLinkType)).EndInit();
			this.gbLinkType.ResumeLayout(false);
			this.gbLinkType.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.gbViewLoc)).EndInit();
			this.gbViewLoc.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.uiGroupBox1)).EndInit();
			this.uiGroupBox1.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.gbBanner)).EndInit();
			this.gbBanner.ResumeLayout(false);
			this.gbBanner.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.picAdFile)).EndInit();
			this.ResumeLayout(false);

        }
        #endregion

        #region ��Ʈ�� �ε�
        private void UserControl_Load(object sender, System.EventArgs e)
        {
            // �����Ͱ����� ��ü����
            dt = ((DataView)grdExItemList.DataSource).Table;  
            cm = (CurrencyManager) this.BindingContext[grdExItemList.DataSource]; 
            //            cm.PositionChanged += new System.EventHandler(OnGrdRowChanged); 

            dtDetail = ((DataView)grdExContactHistory.DataSource).Table;  
            cmDetail = (CurrencyManager) this.BindingContext[grdExContactHistory.DataSource]; 
			cmDetail.PositionChanged += new System.EventHandler(OnGrdDetailRowChanged); 

            // ��Ʈ�� �ʱ�ȭ
            InitControl();	
			
			// �˾��޴� ����
			setPopMenu();

			// ��Ƽ���� ���º��� �޺��ڽ� �ʱ�ȭ
			initCbMultiAdState();

			// �������(����) Reading
			AD_STATE_DEL = ConfigurationManager.AppSettings.Get("StAdDel");
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
                SearchContractItem();
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
                //ResetTextReadonly();
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
            Init_MediaCode();
            Init_RapCode();
            Init_AgencyCode();
            Init_AdvertiserCode();
            Init_AdState();
            Init_AdClass();
			Init_Mgrade();
            Init_AdType();
            InitCombo_Level();
            Init_ScheduleTypeCode();
            
        }

        private void InitCombo_Level()
        {
            if(commonModel.UserLevel == "20")
            {
                // �޺��Ƚ�						
                cbSearchMedia.SelectedValue = commonModel.MediaCode;			
                cbSearchMedia.ReadOnly = true;				            
            }
            else
            {
				// design time �� Data Set ������� �����Ƿ� �ּ�.bae
				/*
				for(int i=0;i < contractItemDs.Medias.Rows.Count;i++)
				{
					DataRow row = contractItemDs.Medias.Rows[i];					
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
				*/
				// ���� ��Ʈ�ѿ� �� ���ε�
				cbSearchMedia.SelectedValue = FrameSystem._HANATV.ToString();

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
            if(canRead)   btnSearch.Enabled = true;
            if(canCreate) btnAdd.Enabled    = true;
            //if(canCreate) btnCopy.Enabled	= true;

            btnSearchContract.Enabled = false;
            if (!IsAdding && ebItemName.Text.Trim().Length > 0) 
            {
                if(canDelete) btnDelete.Enabled = true;
                if(canUpdate) btnSave.Enabled   = true;
                //if(canUpdate) btnCopy.Enabled   = true;
            }

            if(IsAdding)
            {
                btnAdd.Enabled    = false;
                tabPageItem.Selected = true;
                tabPageFile.Enabled = false;
                tabPageContract.Enabled = false;
                tabPageHistory.Enabled = false;
                if(canCreate) btnSave.Enabled   = true;
                //if(canUpdate) btnCopy.Enabled   = true;
                if(canCreate) btnSearchContract.Enabled = true;
          
            }
            else
            {
                tabPageFile.Enabled = true;
                tabPageContract.Enabled = true;
                tabPageHistory.Enabled = true;
                btnSearchContract.Enabled = false;
            }

            //grdExItemList.Focus();

            Application.DoEvents();			
        }

        private void Init_MediaCode()
        {			
			
            // ��ü�� ��ȸ�Ѵ�.
            MediaCodeModel mediaCodeModel = new MediaCodeModel();
            new MediaCodeManager(systemModel, commonModel).GetMediaCodeList(mediaCodeModel);
			
            if (mediaCodeModel.ResultCD.Equals("0000"))
            {
                // �����ͼ¿� ����
                Utility.SetDataTable(contractItemDs.Medias, mediaCodeModel.MediaCodeDataSet);				
            }

            // �˻������� �޺�
            this.cbSearchMedia.Items.Clear();
			
            // �޺��ڽ��� ��Ʈ�� �ڵ����� ���� Item�迭�� ����
            Janus.Windows.EditControls.UIComboBoxItem[] comboItems = new Janus.Windows.EditControls.UIComboBoxItem[mediaCodeModel.ResultCnt + 1];

            comboItems[0] = new Janus.Windows.EditControls.UIComboBoxItem("��ü����","00");
			
            for(int i = 0; i < mediaCodeModel.ResultCnt;i++)
            {
                DataRow row = contractItemDs.Medias.Rows[i];

                string val = row["MediaCode"].ToString();
                string txt = row["MediaName"].ToString();
                comboItems[i+1] = new Janus.Windows.EditControls.UIComboBoxItem(txt,val);
            }
            // �˻� �޺��� ��Ʈ
            this.cbSearchMedia.Items.AddRange(comboItems);
            this.cbSearchMedia.SelectedIndex = 0;
				
            Application.DoEvents();
			
		

			
        }

        private void Init_RapCode()
        {
			/* ����� ������� ������
            // ���� ��ȸ�Ѵ�.
            MediaRapCodeModel mediaRapCodeModel = new MediaRapCodeModel();
            new MediaRapCodeManager(systemModel, commonModel).GetMediaRapCodeList(mediaRapCodeModel);
			
            if (mediaRapCodeModel.ResultCD.Equals("0000"))
            {
                // �����ͼ¿� ����
                Utility.SetDataTable(contractItemDs.MediaRaps, mediaRapCodeModel.MediaRapCodeDataSet);				
            }

            // �˻������� �޺�
            this.cbSearchRap.Items.Clear();
			
            // �޺��ڽ��� ��Ʈ�� �ڵ����� ���� Item�迭�� ����
            Janus.Windows.EditControls.UIComboBoxItem[] comboItems = new Janus.Windows.EditControls.UIComboBoxItem[mediaRapCodeModel.ResultCnt + 1];

            comboItems[0] = new Janus.Windows.EditControls.UIComboBoxItem("�̵�����","00");
			
            for(int i=0;i<mediaRapCodeModel.ResultCnt;i++)
            {
                DataRow row = contractItemDs.MediaRaps.Rows[i];

                string val = row["RapCode"].ToString();
                string txt = row["RapName"].ToString();
                comboItems[i+1] = new Janus.Windows.EditControls.UIComboBoxItem(txt,val);
            }
            // �޺��� ��Ʈ
            this.cbSearchRap.Items.AddRange(comboItems);
            this.cbSearchRap.SelectedIndex = 0;

            Application.DoEvents();
			*/
            this.cbSearchRap.Items.Clear();
            DataSet ds = new DataSet("rapset");
            DataTable dt = new DataTable("rap");
            dt.Columns.Add("RapCode", typeof(string));
            dt.Columns.Add("RapName", typeof(string));
            DataRow nRow = dt.NewRow();
            nRow["RapCode"] = "00";
            nRow["RapName"] = "��ü";
            dt.Rows.Add(nRow);
            ds.Tables.Add(dt);

            Utility.SetDataTable(contractItemDs.MediaRaps, ds);
            // �˻������� �޺�
            this.cbSearchRap.Items.Clear();
            // �޺��ڽ��� ��Ʈ�� �ڵ����� ���� Item�迭�� ����
            Janus.Windows.EditControls.UIComboBoxItem[] comboItems = new Janus.Windows.EditControls.UIComboBoxItem[2];

            comboItems[0] = new Janus.Windows.EditControls.UIComboBoxItem("�̵�����", "00");

            for (int i = 0; i < 1; i++)
            {
                DataRow row = contractItemDs.MediaRaps.Rows[i];

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
                Utility.SetDataTable(contractItemDs.Agencys, agencyCodeModel.AgencyCodeDataSet);				
            }

            // �˻������� �޺�
            this.cbSearchAgency.Items.Clear();

            // �޺��ڽ��� ��Ʈ�� �ڵ����� ���� Item�迭�� ����
            Janus.Windows.EditControls.UIComboBoxItem[] comboItems = new Janus.Windows.EditControls.UIComboBoxItem[agencyCodeModel.ResultCnt + 1];

            comboItems[0] = new Janus.Windows.EditControls.UIComboBoxItem("����缱��","00");
			
            for(int i=0;i<agencyCodeModel.ResultCnt;i++)
            {
                DataRow row = contractItemDs.Agencys.Rows[i];

                string val = row["AgencyCode"].ToString();
                string txt = row["AgencyName"].ToString();
                comboItems[i+1] = new Janus.Windows.EditControls.UIComboBoxItem(txt,val);
            }
			
            // �޺��� ��Ʈ
            this.cbSearchAgency.Items.AddRange(comboItems);
            this.cbSearchAgency.SelectedIndex = 0;

            Application.DoEvents();
			
        }

        private void Init_AdvertiserCode()
        {
			
            // �����ָ� ��ȸ�Ѵ�.
            ClientModel clientModel = new ClientModel();
            new ClientManager(systemModel, commonModel).GetAdvertiserList(clientModel);
			
            if (clientModel.ResultCD.Equals("0000"))
            {
                // �����ͼ¿� ����
                Utility.SetDataTable(contractItemDs.Advertisers, clientModel.ClientDataSet);				
            }

            // �˻������� �޺�
            this.cbSearchAdvertiser.Items.Clear();

            // �޺��ڽ��� ��Ʈ�� �ڵ����� ���� Item�迭�� ����
            Janus.Windows.EditControls.UIComboBoxItem[] comboItems = new Janus.Windows.EditControls.UIComboBoxItem[clientModel.ResultCnt + 1];

            comboItems[0] = new Janus.Windows.EditControls.UIComboBoxItem("�����ּ���","00");
			
            for(int i=0;i<clientModel.ResultCnt;i++)
            {
                DataRow row = contractItemDs.Advertisers.Rows[i];

                string val = row["AdvertiserCode"].ToString();
                string txt = row["AdvertiserName"].ToString();
                comboItems[i+1] = new Janus.Windows.EditControls.UIComboBoxItem(txt,val);
            }
			
            // �޺��� ��Ʈ
            this.cbSearchAdvertiser.Items.AddRange(comboItems);
            this.cbSearchAdvertiser.SelectedIndex = 0;

            Application.DoEvents();
		
			
        }

        private void Init_AdState()
        {
			
            // �ڵ忡�� �������¸� ��ȸ�Ѵ�.
            CodeModel codeModel = new CodeModel();
            codeModel.Section = "25";				// �ڵ�з� '25':�������  TODO: �ڵ�з��� ���� XML�� �����Ǿ��...
            new CodeManager(systemModel, commonModel).GetCodeList(codeModel);
			
            if (codeModel.ResultCD.Equals("0000"))
            {
                // �����ͼ¿� ����
                Utility.SetDataTable(contractItemDs.AdState, codeModel.CodeDataSet);				
            }
  
            Application.DoEvents();
        }

        private void Init_AdType()
        {
			
			// �ڵ忡�� �������¸� ��ȸ�Ѵ�.
			CodeModel codeModel = new CodeModel();
			codeModel.Section = "26";				// '26':��������  TODO: �ڵ�з��� ���� XML�� �����Ǿ��...
			new CodeManager(systemModel, commonModel).GetCodeList(codeModel);
			
			if (codeModel.ResultCD.Equals("0000"))
			{
				// �����ͼ¿� ����
				Utility.SetDataTable(contractItemDs.AdType, codeModel.CodeDataSet);				
			}
 
            // �˻������� �޺�
            this.cbSearchAdType.Items.Clear();
			
            // �޺��ڽ��� ��Ʈ�� �ڵ����� ���� Item�迭�� ����
            Janus.Windows.EditControls.UIComboBoxItem[] comboItems = new Janus.Windows.EditControls.UIComboBoxItem[codeModel.ResultCnt + 1];

            comboItems[0] = new Janus.Windows.EditControls.UIComboBoxItem("��������","00");
			
            for(int i=0;i<codeModel.ResultCnt;i++)
            {
                DataRow row = contractItemDs.AdType.Rows[i];

                string val = row["Code"].ToString();
                string txt = row["CodeName"].ToString();
                comboItems[i+1] = new Janus.Windows.EditControls.UIComboBoxItem(txt,val);
            }
            // �޺��� ��Ʈ
            this.cbSearchAdType.Items.AddRange(comboItems);
            this.cbSearchAdType.SelectedIndex = 0;

            Application.DoEvents();
		
        }

        private void Init_AdClass()
        {
			
            // �ڵ忡�� �������¸� ��ȸ�Ѵ�.
            CodeModel codeModel = new CodeModel();
            codeModel.Section = "29";				// ����뵵 '29':����뵵  TODO: �ڵ�з��� ���� XML�� �����Ǿ��...
            new CodeManager(systemModel, commonModel).GetCodeList(codeModel);
			
            if (codeModel.ResultCD.Equals("0000"))
            {
                // �����ͼ¿� ����
                Utility.SetDataTable(contractItemDs.AdClass, codeModel.CodeDataSet);				
            }

            Application.DoEvents();
			

		
        }

		private void Init_Mgrade()
		{	
			/*
			ContractItemModel contractItemModel = new ContractItemModel();		
			new ContractItemManager(systemModel, commonModel).GetGradeCodeList(contractItemModel);
			
			if (contractItemModel.ResultCD.Equals("0000"))
			{
				// �����ͼ¿� ����
				Utility.SetDataTable(contractItemDs.Grade, contractItemModel.GradeDataSet);				
			}

			// ����ȸ �޺�
			// �������� �޺��� Dataset�� �����ͼҽ��� ������.

			// �˻������� �޺�
			this.cbMgrade.Items.Clear();
			
			// �޺��ڽ��� ��Ʈ�� �ڵ����� ���� Item�迭�� ����
			Janus.Windows.EditControls.UIComboBoxItem[] comboItems = new Janus.Windows.EditControls.UIComboBoxItem[contractItemModel.ResultCnt + 1];

			comboItems[0] = new Janus.Windows.EditControls.UIComboBoxItem("��޼���","00");
			
			for(int i=0;i<contractItemModel.ResultCnt;i++)
			{
				DataRow row = contractItemDs.Grade.Rows[i];

				string val = row["Code"].ToString();
				string txt = row["CodeName"].ToString();
				comboItems[i+1] = new Janus.Windows.EditControls.UIComboBoxItem(txt,val);
			}
			// �޺��� ��Ʈ
			this.cbMgrade.Items.AddRange(comboItems);
			this.cbMgrade.SelectedIndex = 0;

			Application.DoEvents();
			*/

            // �ڵ忡�� �������¸� ��ȸ�Ѵ�.
            
            //CodeModel codeModel = new CodeModel();
            //codeModel.Section = "41";				// OAP �����õ��
            //new CodeManager(systemModel, commonModel).GetCodeList(codeModel);

            //if (codeModel.ResultCD.Equals("0000"))
            //{
            //    // �����ͼ¿� ����
            //    //Utility.SetDataTable(contractItemDs.Grade, codeModel.CodeDataSet);
            //    cbMgrade.DataSource = codeModel.CodeDataSet.Tables[0];
            //    cbMgrade.DisplayMember = "codeName";
            //    cbMgrade.ValueMember = "code";
            //    cbMgrade.SelectedIndex = 0;
            //}
            
            //Application.DoEvents();
            
		}

        private void DisableButton()
        {
            btnSearch.Enabled = false;
            btnAdd.Enabled    = false;
            btnSave.Enabled   = false;
            btnDelete.Enabled = false;
            Application.DoEvents();            
        }

        //������ Select
        private void Init_ScheduleTypeCode()
        {
            // �ڵ忡�� �������¸� ��ȸ�Ѵ�.
            CodeModel codeModel = new CodeModel();
            codeModel.Section = "27";				
            new CodeManager(systemModel, commonModel).GetCodeList(codeModel);

            if (codeModel.ResultCD.Equals("0000"))
            {
                // �����ͼ¿� ����
                Utility.SetDataTable(contractItemDs.ScheduleType, codeModel.CodeDataSet);
            }

            //this.cbScheduleType.Items.Clear();

            // �޺��ڽ��� ��Ʈ�� �ڵ����� ���� Item�迭�� ����
            Janus.Windows.EditControls.UIComboBoxItem[] comboItems = new Janus.Windows.EditControls.UIComboBoxItem[codeModel.ResultCnt ];

            //comboItems[0] = new Janus.Windows.EditControls.UIComboBoxItem("������", "00");

            for (int i = 0; i < codeModel.ResultCnt; i++)
            {
                DataRow row = contractItemDs.ScheduleType.Rows[i];

                string val = row["Code"].ToString();
                string txt = row["CodeName"].ToString();
                
                comboItems[i] = new Janus.Windows.EditControls.UIComboBoxItem(txt, val);
            }
            // �޺��� ��Ʈ
            this.cbScheduleType.Items.AddRange(comboItems);
            Application.DoEvents();
        }

        #endregion

        #region ������ �׼�ó�� �޼ҵ�

        /// <summary>
        /// �׸����� Row�����
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnGrdRowChanged(object sender, System.EventArgs e) 
        {
            if (!IsSearching) // 2011.11.28 RH.Jung ��ȸ���� �ƴҰ�쿡�� �����ϵ��� ����
            {
                try
                {
                    if (IsNotLoading)
                    {
                        //tabPageItem.Selected = true;
                        ResetDetailText();
                        SetDetailText();
                        InitButton();
                    }
                }
                catch (Exception ex)
                {
                    Trace.WriteLine(ex.Message);
                }
            }
        }
		
        /// <summary>
        /// ������� �����丮 �׸����� Row�����
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnGrdDetailRowChanged(object sender, System.EventArgs e) 
        {
            if(grdExContactHistory.RowCount > 0)
            {
                SetContactHistoryDetailText();                
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
            ResetDetailText();
            SearchContractItem();
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
            if(canCreate)btnSave.Enabled   = true;
            if(canCreate) btnSearchContract.Enabled = true;
            IsAdding = true;
            
            ResetTextReadonly();
            ResetDetailText();
            InitButton();
            ebItemName.Focus();	// �߰��� ���� ��ġ

            #region [ 1. ����Ÿ�� ���� ]
            // ����Ÿ���� �����Ѵ�
            AdManagerClient._20_Contract.ContractItem_AddType Form1 = new AdManagerClient._20_Contract.ContractItem_AddType();
            DialogResult    result = Form1.ShowDialog(this);
            
			// Ÿ���� ����Ǿ�����, �����ϼ���
			// 10: PCM( PreRoll ), 18: MCM( MidRoll ), 19: TCM(PostRoll) , 20:OAP������, 31:ZCM( ä������) 51:BCM(Banner)
            if( result == DialogResult.Yes )
            { 
                int adType = Form1.AdType;
               
                switch( adType )       
                {         
                    case 10:    // CM   �Ϲ� �������   
                        //cbMgrade.SelectedValue = "50";          // �����õ��(��Ÿ���)
                        cbAdState.SelectedValue = "10";         // �������(���)
                        cbAdClass.SelectedValue = "90";         // OAP����(��Ÿ)
                        cbAdType.SelectedValue = Form1.AdType;  // �˾����� ���õȰ�
                        cbAdRate.SelectedValue = "0";           // ������(��ü�̿밡)
                        ebAdTime.Text = "15";                   // ����Ÿ��
                        
                        //lblMsg.Text = "��ȨCM�뵵�� ��� Ȩ���� ýũ�ؾ� �մϴ�";

                        //cbMgrade.Enabled    = false;
                        cbAdState.Enabled    = false;
                        cbAdClass.Enabled    = false;
                        cbAdType.Enabled    = false;

                        //������ ��� (�ð��� ����) 
                        cbScheduleType.SelectedValue = "10";    //�Ϲ��� ����
                        cbScheduleType.Enabled = true;

                        break;                  
                    case 11:    // EAP   �ʼ� ���˱���
                        //cbMgrade.SelectedValue = "50";          // �����õ��
                        cbAdState.SelectedValue = "10";         // �������
                        cbAdClass.SelectedValue = "90";         // OAP����
                        cbAdType.SelectedValue = Form1.AdType;  // �˾����� ���õȰ�
                        cbAdRate.SelectedValue = "0";           // ������(��ü�̿밡)
                        ebAdTime.Text = "30";                   // ����Ÿ��

                        //cbMgrade.Enabled    = true;
                        cbAdState.Enabled   = false;
                        cbAdClass.Enabled   = true;
                        cbAdType.Enabled    = false;

                        //������ ��� (�ð��� ����) 
                        cbScheduleType.SelectedValue = "10";    //�Ϲ��� ����
                        cbScheduleType.Enabled = false;

                        break;
                    case 12:    // SCM   ���α���
                        //cbMgrade.SelectedValue = "50";          // �����õ��
                        cbAdState.SelectedValue = "10";         // �������
                        cbAdClass.SelectedValue = "90";         // OAP����
                        cbAdType.SelectedValue = Form1.AdType;  // �˾����� ���õȰ�
                        cbAdRate.SelectedValue = "0";           // ������(��ü�̿밡)
                        ebAdTime.Text = "15";                   // ����Ÿ��

                        //cbMgrade.Enabled    = false;
                        cbAdState.Enabled   = false;
                        cbAdClass.Enabled   = true;
                        cbAdType.Enabled    = false;

                        //������ ��� (�ð��� ����) 
                        cbScheduleType.SelectedValue = "10";    //�Ϲ��� ����
                        cbScheduleType.Enabled = false;

                        break;
                    case 13:    // CSS   ī�װ� �������� ����
                        //cbMgrade.SelectedValue = "50";          // �����õ��
                        cbAdState.SelectedValue = "10";         // �������
                        cbAdClass.SelectedValue = "90";         // OAP����
                        cbAdType.SelectedValue = Form1.AdType;  // �˾����� ���õȰ�
                        cbAdRate.SelectedValue = "0";           // ������(��ü�̿밡)
                        ebAdTime.Text = "15";                   // ����Ÿ��                        

                        //cbMgrade.Enabled    = false;
                        cbAdState.Enabled   = false;
                        cbAdClass.Enabled   = false;
                        cbAdType.Enabled    = false;

                        //������ ��� (�ð��� ����) 
                        cbScheduleType.SelectedValue = "10";    //�Ϲ��� ����
                        cbScheduleType.Enabled = false;

                        break;
                    case 14:    // ACM   �߰�����
                        //cbMgrade.SelectedValue  = "50";          // �����õ��
                        //cbMgrade.Enabled    = false;
                        cbAdState.SelectedValue = "10";         // �������
                        cbAdClass.SelectedValue = "90";         // OAP����
                        cbAdType.SelectedValue  = Form1.AdType;  // �˾����� ���õȰ�
                        cbAdRate.SelectedValue  = "0";           // ������(��ü�̿밡)
                        ebAdTime.Text           = "15";                   // ����Ÿ��

                        //cbMgrade.Enabled    = false;
                        cbAdState.Enabled   = false;
                        cbAdClass.Enabled   = false;
                        cbAdType.Enabled    = false;
                        break;

                    case 15:    // CCM   �ڳʱ���
                        //cbMgrade.SelectedValue  = "50";         // �����õ��
                        //cbMgrade.Enabled    = false;
                        cbAdState.SelectedValue = "10";         // �������
                        cbAdClass.SelectedValue = "90";         // OAP����
                        cbAdType.SelectedValue  = Form1.AdType; // �˾����� ���õȰ�
                        cbAdRate.SelectedValue  = "0";          // ������(��ü�̿밡)
                        ebAdTime.Text           = "5";          // ����Ÿ��

                        //cbMgrade.Enabled    = false;
                        cbAdState.Enabled   = false;
                        cbAdClass.Enabled   = false;
                        cbAdType.Enabled    = false;

                        //������ ��� (�ð��� ����) 
                        cbScheduleType.SelectedValue = "10";    //�Ϲ��� ����
                        cbScheduleType.Enabled = false;

                        break;
                    case 16:    // RCM   ��û���� ����
                        //cbMgrade.SelectedValue  = "50";         // �����õ��
                        //cbMgrade.Enabled    = false;
                        cbAdState.SelectedValue = "10";         // �������
                        cbAdClass.SelectedValue = "90";         // OAP����
                        cbAdType.SelectedValue  = Form1.AdType; // �˾����� ���õȰ�
                        cbAdRate.SelectedValue  = "0";          // ������(��ü�̿밡)
                        ebAdTime.Text           = "5";          // ����Ÿ��
                        //lblMsg.Text = "��ȨCM�뵵�� ��� Ȩ���� ýũ�ؾ� �մϴ�";                        

                        //cbMgrade.Enabled    = false;
                        cbAdState.Enabled   = false;
                        cbAdClass.Enabled   = false;
                        cbAdType.Enabled    = false;

                        //������ ��� (�ð��� ����) 
                        cbScheduleType.SelectedValue = "10";    //�Ϲ��� ����
                        cbScheduleType.Enabled = false;

                        break;
                    case 17:    // MCM   �߰�����
                        //cbMgrade.SelectedValue  = "50";         // �����õ��
                        //cbMgrade.Enabled    = false;
                        cbAdState.SelectedValue = "10";         // �������
                        cbAdClass.SelectedValue = "90";         // OAP����
                        cbAdType.SelectedValue  = Form1.AdType; // �˾����� ���õȰ�
                        cbAdRate.SelectedValue  = "0";          // ������(��ü�̿밡)
                        ebAdTime.Text           = "15";          // ����Ÿ��
                        
                        //lblMsg.Text = "��ȨCM�뵵�� ��� Ȩ���� ýũ�ؾ� �մϴ�";

                        //cbMgrade.Enabled    = false;
                        cbAdState.Enabled   = false;
                        cbAdClass.Enabled   = false;
                        cbAdType.Enabled    = false;

                        //������ ��� (�ð��� ����) 
                        cbScheduleType.SelectedValue = "10";    //�Ϲ��� ����
                        cbScheduleType.Enabled = false;

                        break;
                    case 19:    // TCM  ��������
                        //cbMgrade.SelectedValue = "50";          // �����õ��
                        cbAdState.SelectedValue = "10";         // �������
                        cbAdClass.SelectedValue = "90";         // OAP����
                        cbAdType.SelectedValue = Form1.AdType;  // �˾����� ���õȰ�
                        cbAdRate.SelectedValue = "0";           // ������(��ü�̿밡)
                        ebAdTime.Text = "15";                   // ����Ÿ��
                        
                        //lblMsg.Text = "�� ��󿣵��� ��� �������� �����Ͻʽÿ�...";

                        //cbMgrade.Enabled    = false;
                        cbAdState.Enabled   = false;
                        cbAdClass.Enabled   = false;
                        cbAdType.Enabled    = false;

                        //������ ��� (�ð��� ����) 
                        cbScheduleType.Enabled = true;
                        cbScheduleType.SelectedValue = "10";    //�Ϲ��� ����


                        break;
                    case 20:    // OAP  �ɼ����˱���
                        //cbMgrade.SelectedValue  = "30";         // �����õ��(B���)
                        //cbMgrade.Enabled    = true;
                        cbAdState.SelectedValue = "10";         // �������
                        cbAdClass.SelectedValue = "20";         // OAP����(ä��)
                        cbAdType.SelectedValue  = Form1.AdType; // �˾����� ���õȰ�
                        cbAdRate.SelectedValue  = "0";          // ������(��ü�̿밡)
                        ebAdTime.Text           = "15";          // ����Ÿ��

                        //cbMgrade.Enabled    = true;
                        cbAdState.Enabled   = false;
                        cbAdClass.Enabled   = true;
                        cbAdType.Enabled    = false;

                        //������ ��� (�ð��� ����) 
                        cbScheduleType.SelectedValue = "10";    //�Ϲ��� ����
                        cbScheduleType.Enabled = false;

                        break;
                    case 21:    // TMCM   �ð��뵶�� - ������� �߰� ��.
                        //cbMgrade.SelectedValue = "50";          // �����õ��(��Ÿ���)
                        cbAdState.SelectedValue = "10";         // �������(���)
                        cbAdClass.SelectedValue = "90";         // OAP����(��Ÿ)
                        cbAdType.SelectedValue = "10";  // �˾����� ���õȰ�
                        cbAdRate.SelectedValue = "0";           // ������(��ü�̿밡)
                        ebAdTime.Text = "15";                   // ����Ÿ��

                        //cbMgrade.Enabled = false;
                        cbAdState.Enabled = false;
                        cbAdClass.Enabled = false;
                        cbAdType.Enabled = false;

                        //������ ��� (�ð��� ����) 
                        cbScheduleType.SelectedValue = "30";    //�ð��뵶���� ����
                        cbScheduleType.Enabled = false;

                        break;    
                    case 99:    // �⺻����
                        //cbMgrade.SelectedValue  = "50";         // �����õ��(B���)
                        cbAdState.SelectedValue = "10";         // �������
                        cbAdClass.SelectedValue = "90";         // OAP����(ä��)
                        cbAdType.SelectedValue  = Form1.AdType; // �˾����� ���õȰ�
                        cbAdRate.SelectedValue  = "0";          // ������(��ü�̿밡)
                        ebAdTime.Text           = "15";          // ����Ÿ��

                        //cbMgrade.Enabled    = false;
                        cbAdState.Enabled   = false;
                        cbAdClass.Enabled   = false;
                        cbAdType.Enabled    = false;

                        //������ ��� (�ð��� ����) 
                        cbScheduleType.SelectedValue = "10";    //�Ϲ��� ����
                        cbScheduleType.Enabled = false;

                        break;
                }
            }
            else
            {
                cbAdType.SelectedValue = "00";
            }
            Form1.Dispose();
            Form1 = null;

            #endregion

            #region [ 2. ������ ���� ]
            // �������� �����Ѵ�
            ContractItem_pForm Form2 = new ContractItem_pForm(this);

            Form2.ShowDialog();
            
            Form2.Dispose();
            Form2 = null;
            #endregion
        }

        /// <summary>
        /// �����ư Ŭ��
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_Click(object sender, System.EventArgs e)
        {
            SaveContractItemDetail();
        }

        /// <summary>
        /// �ּ�ó��
        /// </summary>        
		private void btnSave2_Click(object sender, System.EventArgs e)
		{
            //SaveLinkChannel(keyLinkType);
		}

        /// <summary>
        /// ������ư Ŭ��
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDelete_Click(object sender, System.EventArgs e)
        {
            DeleteContractItem();
        }

		private void btnDeleteLink2Channel_Click(object sender, System.EventArgs e)
		{
            
			DeleteLinkChannel(keyLinkType);
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
                SearchContractItem();
            }
        }
        private void btnSearchContract_Click(object sender, System.EventArgs e)
        {
            // ������ ��� �˻� �˾� ����
            ContractItem_pForm pForm = new ContractItem_pForm(this);

            pForm.ShowDialog();
            
            pForm.Dispose();
            pForm = null;
        }


		private void cbAdState_SelectedItemChanged(object sender, System.EventArgs e)
		{
			if(cbAdState.SelectedValue.ToString().Equals("40")) // ������� 40:����
			{
				meRealEndDay.Value = DateTime.Now;
			}
		
		}

        #endregion

        #region ó���޼ҵ�

        /// <summary>
        /// ��������� ��ȸ
        /// </summary>
        private void SearchContractItem()
        {
            IsSearching = true;

            StatusMessage("������ ������ ��ȸ�մϴ�.");

            try
            {
                //�˻� ���� ���� �ʱ�ȭ ���ش�.
                contractItemModel.Init();
                // �����͸𵨿� ������ ������ ��Ʈ�Ѵ�.
                contractItemModel.MediaCode      = cbSearchMedia.SelectedValue.ToString();
                contractItemModel.RapCode        = cbSearchRap.SelectedValue.ToString();
                contractItemModel.AgencyCode     = cbSearchAgency.SelectedValue.ToString();
                contractItemModel.AdvertiserCode = cbSearchAdvertiser.SelectedValue.ToString();
                contractItemModel.AdType		 = cbSearchAdType.SelectedValue.ToString();

				if(chkAdState_10.Checked)   contractItemModel.SearchchkAdState_10   = "Y";
                if(chkAdState_20.Checked)   contractItemModel.SearchchkAdState_20   = "Y";
                if(chkAdState_30.Checked)   contractItemModel.SearchchkAdState_30   = "Y";
                if(chkAdState_40.Checked)   contractItemModel.SearchchkAdState_40   = "Y";

				if(rbSch_Y.Checked)
				{
					contractItemModel.SearchChkSch_YN   = "Y";
				}
				else if(rbSch_N.Checked)
				{
					contractItemModel.SearchChkSch_YN   = "N";
				}
				else
				{
					contractItemModel.SearchChkSch_YN   = "";
				}

                if(IsNewSearchKey)
                {
                    contractItemModel.SearchKey = "";
                }
                else
                {
                    contractItemModel.SearchKey  = ebSearchKey.Text;
                }
				

                // ���� ���� ��� ���񽺸� ȣ���Ѵ�.
                new ContractItemManager(systemModel,commonModel).GetContractItemList(contractItemModel);
				
                if (contractItemModel.ResultCD.Equals("0000"))
                {
                    Utility.SetDataTableFast(contractItemDs.ContractItem, contractItemModel.ContractItemDataSet);				
                    StatusMessage(contractItemModel.ResultCnt + "���� �������� ������ ��ȸ�Ǿ����ϴ�.");
					
                    if(canUpdate)
                    {						
                        AddSchChoice();									
                    }										
                    SetDetailText();
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
            finally
            {
                IsSearching = false;
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
                if ( contractItemDs.Tables["ContractItem"].Rows.Count < 1 ) return;
              
                foreach (DataRow row in contractItemDs.Tables["ContractItem"].Rows)
                {					
                    if(IsAdding)
                    {
                        cm.Position = 0;
                        keyItemNo = null;									
                    }
                    else
                    {												
                        if(row["ItemNo"].ToString().Equals(rowItemNo))
                        {					
                            cm.Position = rowIndex;
                            break;								
                        }
                    }

                    rowIndex++;
                    grdExItemList.EnsureVisible();
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
		/// �������α׷� �������󼼸�� ��ȸ -����������� ����
		/// </summary>
		private void SearchLinkChannelDetail()
		{
            /* ���� �� ��
			contractItemModel.ItemNo = keyItemNo;
            contractItemModel.LinkType = 1;
			// ����ø��� ��� ���񽺸� ȣ���Ѵ�.
			new ContractItemManager(systemModel,commonModel).GetLinkChannel(contractItemModel);

			if (contractItemModel.ResultCD.Equals("0000"))
			{
				Utility.SetDataTable(contractItemDs.LinkChannel, contractItemModel.LinkChannelDataSet);				
				StatusMessage(contractItemModel.ResultCnt + "���� �������� ������ ��ȸ�Ǿ����ϴ�.");

				if(contractItemModel.ResultCnt > 0)
				{
                    tabPageLinkChannel.Text = "����ø���(����Ⱥ���)[" + contractItemModel.ResultCnt.ToString() + "]";

					for (int i =0; i < contractItemDs.LinkChannel.Rows.Count; i++)
					{

						DataRow Row = contractItemDs.LinkChannel.Rows[i];						
						
						if(i==0)
						{
							ebLinkChannelNo.Text			= Row["Channel"].ToString();		//ChannelNo�� Int���ε�..TR�� '�Է��� ���ڿ��� ���� �߸��Ǿ��ٴ� ������ ����..' �� ã�ƺ����� �κ��̴�.
							ebLinkChannelNm.Text			= Row["ChannelNm"].ToString();    

							ebLinkChannelNo2.Text			= "";
							ebLinkChannelNm2.Text			= "";
					
							ebLinkChannelNo3.Text			= "";
							ebLinkChannelNm3.Text			= "";
					
							ebLinkChannelNo4.Text			= "";
							ebLinkChannelNm4.Text			= "";
					
							ebLinkChannelNo5.Text			= "";
							ebLinkChannelNm5.Text			= "";
						}
						if(i==1)
						{
							ebLinkChannelNo2.Text			= Row["Channel"].ToString();		//ChannelNo�� Int���ε�..TR�� '�Է��� ���ڿ��� ���� �߸��Ǿ��ٴ� ������ ����..' �� ã�ƺ����� �κ��̴�.
							ebLinkChannelNm2.Text			= Row["ChannelNm"].ToString();                

							ebLinkChannelNo3.Text			= "";
							ebLinkChannelNm3.Text			= "";
					
							ebLinkChannelNo4.Text			= "";
							ebLinkChannelNm4.Text			= "";
					
							ebLinkChannelNo5.Text			= "";
							ebLinkChannelNm5.Text			= "";
						}
						if(i==2)
						{
							ebLinkChannelNo3.Text			= Row["Channel"].ToString();		//ChannelNo�� Int���ε�..TR�� '�Է��� ���ڿ��� ���� �߸��Ǿ��ٴ� ������ ����..' �� ã�ƺ����� �κ��̴�.
							ebLinkChannelNm3.Text			= Row["ChannelNm"].ToString();                

							ebLinkChannelNo4.Text			= "";
							ebLinkChannelNm4.Text			= "";
					
							ebLinkChannelNo5.Text			= "";
							ebLinkChannelNm5.Text			= "";
						}
						if(i==3)
						{
							ebLinkChannelNo4.Text			= Row["Channel"].ToString();		//ChannelNo�� Int���ε�..TR�� '�Է��� ���ڿ��� ���� �߸��Ǿ��ٴ� ������ ����..' �� ã�ƺ����� �κ��̴�.
							ebLinkChannelNm4.Text			= Row["ChannelNm"].ToString();               

							ebLinkChannelNo5.Text			= "";
							ebLinkChannelNm5.Text			= "";
						}
						if(i==4)
						{					
							ebLinkChannelNo5.Text			= Row["Channel"].ToString();		//ChannelNo�� Int���ε�..TR�� '�Է��� ���ڿ��� ���� �߸��Ǿ��ٴ� ������ ����..' �� ã�ƺ����� �κ��̴�.
							ebLinkChannelNm5.Text			= Row["ChannelNm"].ToString();      
						}
					}


					//�Է� �� Clear ��ư(E_01)
					this.btnClearLinkChannel.Enabled = false;
				}
				else
				{
                    tabPageLinkChannel.Text = "����ø���(����Ⱥ���)";
					ebLinkChannelNo.Text			= "";
					ebLinkChannelNm.Text			= "";
					
					ebLinkChannelNo2.Text			= "";
					ebLinkChannelNm2.Text			= "";
					
					ebLinkChannelNo3.Text			= "";
					ebLinkChannelNm3.Text			= "";
					
					ebLinkChannelNo4.Text			= "";
					ebLinkChannelNm4.Text			= "";
					
					ebLinkChannelNo5.Text			= "";
					ebLinkChannelNm5.Text			= "";

					ebLinkChannelNo.ReadOnly			= false;
					ebLinkChannelNm.ReadOnly			= false;
					
					ebLinkChannelNo2.ReadOnly			= false;
					ebLinkChannelNm2.ReadOnly			= false;
					
					ebLinkChannelNo3.ReadOnly			= false;
					ebLinkChannelNm3.ReadOnly			= false;
					
					ebLinkChannelNo4.ReadOnly			= false;
					ebLinkChannelNm4.ReadOnly			= false;
					
					ebLinkChannelNo5.ReadOnly			= false;
					ebLinkChannelNm5.ReadOnly			= false;

					//�Է� �� Clear ��ư(E_01)
					this.btnClearLinkChannel.Enabled = true;
				}
			}
            */
		}

        /// <summary>
        /// ���� ������ ������-�������
        /// </summary>
        /// <param name="isDaul"></param>
        private void SearchLinkItemList(bool isDaul)
        {
            /* ����
            contractItemModel.ItemNo = keyItemNo;
            if( isDaul )
                contractItemModel.ScheduleType = "40";
            else
                contractItemModel.ScheduleType = "00";

            // ����ø��� ��� ���񽺸� ȣ���Ѵ�.
            new ContractItemManager(systemModel, commonModel).GetLinkItem(contractItemModel);

            if (contractItemModel.ResultCD.Equals("0000"))
            {
                if (contractItemModel.ResultCnt > 0)
                {
                    Utility.SetDataTable(contractItemDs.LinkItem, contractItemModel.ContractItemDataSet);
                    uiTabPageDualAd.Text = "��󱤰��� [" + contractItemModel.ResultCnt.ToString() + "]";
                }
                else
                {
                    uiTabPageDualAd.Text = "��󱤰���";
                }
            }
            */
        }



        /// <summary>
        /// �������󼼸�� ��ȸ -������������� ����
        /// </summary>
        private void SearchLink2ChannelDetail()
        {
            /*
            contractItemModel.ItemNo = keyItemNo;
            contractItemModel.LinkType = 2;

            // ����ø��� ��� ���񽺸� ȣ���Ѵ�.
            new ContractItemManager(systemModel, commonModel).GetLinkChannel(contractItemModel);

            if (contractItemModel.ResultCD.Equals("0000"))
            {
                Utility.SetDataTable(contractItemDs.Link2Channel, contractItemModel.LinkChannelDataSet);
                StatusMessage(contractItemModel.ResultCnt + "���� �������� ������ ��ȸ�Ǿ����ϴ�.");

                if (contractItemModel.ResultCnt > 0)
                {
                    tabPageLink2Channel.Text = "����ø���(������) [" + contractItemModel.ResultCnt.ToString() + "]";

                    for (int i = 0; i < contractItemDs.Link2Channel.Rows.Count; i++)
                    {

                        DataRow Row = contractItemDs.Link2Channel.Rows[i];

                        if (i == 0)
                        {
                            ebLink2ChannelNo.Text = Row["Channel"].ToString();		
                            ebLink2ChannelNm.Text = Row["ChannelNm"].ToString();

                            ebLink2ChannelNo2.Text = "";
                            ebLink2ChannelNm2.Text = "";

                            ebLink2ChannelNo3.Text = "";
                            ebLink2ChannelNm3.Text = "";

                            ebLink2ChannelNo4.Text = "";
                            ebLink2ChannelNm4.Text = "";

                            ebLink2ChannelNo5.Text = "";
                            ebLink2ChannelNm5.Text = "";
                        }
                        if (i == 1)
                        {
                            ebLink2ChannelNo2.Text = Row["Channel"].ToString();		
                            ebLink2ChannelNm2.Text = Row["ChannelNm"].ToString();

                            ebLink2ChannelNo3.Text = "";
                            ebLink2ChannelNm3.Text = "";

                            ebLink2ChannelNo4.Text = "";
                            ebLink2ChannelNm4.Text = "";

                            ebLink2ChannelNo5.Text = "";
                            ebLink2ChannelNm5.Text = "";
                        }
                        if (i == 2)
                        {
                            ebLink2ChannelNo3.Text = Row["Channel"].ToString();		
                            ebLink2ChannelNm3.Text = Row["ChannelNm"].ToString();

                            ebLink2ChannelNo4.Text = "";
                            ebLink2ChannelNm4.Text = "";

                            ebLink2ChannelNo5.Text = "";
                            ebLink2ChannelNm5.Text = "";
                        }
                        if (i == 3)
                        {
                            ebLink2ChannelNo4.Text = Row["Channel"].ToString();
                            ebLink2ChannelNm4.Text = Row["ChannelNm"].ToString();

                            ebLink2ChannelNo5.Text = "";
                            ebLink2ChannelNm5.Text = "";
                        }
                        if (i == 4)
                        {
                            ebLink2ChannelNo5.Text = Row["Channel"].ToString();		
                            ebLink2ChannelNm5.Text = Row["ChannelNm"].ToString();
                        }
                    }


                    //�Է� �� Clear ��ư(E_01)
                    this.btnClearLink2Channel.Enabled = false;
                }
                else
                {
                    tabPageLink2Channel.Text = "����ø���(������)";
                    ebLink2ChannelNo.Text = "";
                    ebLink2ChannelNm.Text = "";

                    ebLink2ChannelNo2.Text = "";
                    ebLink2ChannelNm2.Text = "";

                    ebLink2ChannelNo3.Text = "";
                    ebLink2ChannelNm3.Text = "";

                    ebLink2ChannelNo4.Text = "";
                    ebLink2ChannelNm4.Text = "";

                    ebLink2ChannelNo5.Text = "";
                    ebLink2ChannelNm5.Text = "";

                    ebLink2ChannelNo.ReadOnly = false;
                    ebLink2ChannelNm.ReadOnly = false;

                    ebLink2ChannelNo2.ReadOnly = false;
                    ebLink2ChannelNm2.ReadOnly = false;

                    ebLink2ChannelNo3.ReadOnly = false;
                    ebLink2ChannelNm3.ReadOnly = false;

                    ebLink2ChannelNo4.ReadOnly = false;
                    ebLink2ChannelNm4.ReadOnly = false;

                    ebLink2ChannelNo5.ReadOnly = false;
                    ebLink2ChannelNm5.ReadOnly = false;

                    //�Է� �� Clear ��ư(E_01)
                    this.btnClearLink2Channel.Enabled = true;
                }
            }
            */
        }


        /// <summary>
        /// �������󼼸�� ��ȸ
        /// </summary>
        private void SearchContractItemDetail()
        {
            StatusMessage("������ ������ ��ȸ�մϴ�.");

            try
            {
                //��ȸ ���� ���� �ʱ�ȭ ���ش�.
                contractItemModel.Init();
                // �����͸𵨿� ������ ������ ��Ʈ�Ѵ�.
                contractItemModel.ItemNo = keyItemNo;

                // ���� ���� �� ��� ���񽺸� ȣ���Ѵ�.
                new ContractItemManager(systemModel,commonModel).GetContractItemDetail(contractItemModel);

                if (contractItemModel.ResultCD.Equals("0000"))
                {
                    Utility.SetDataTable(contractItemDs.ContractItemDetail, contractItemModel.ContractItemDataSet);				
                    StatusMessage(contractItemModel.ResultCnt + "���� �������� ������ ��ȸ�Ǿ����ϴ�.");
                
                }			

                // ���� ���� �����丮 ��� ���񽺸� ȣ���Ѵ�.
                new ContractItemManager(systemModel,commonModel).GetContractItemHIstoryList(contractItemModel);

                if (contractItemModel.ResultCD.Equals("0000"))
                {
                
                    Utility.SetDataTable(contractItemDs.ContractItemHistory, contractItemModel.ContractItemDataSet);				
                    StatusMessage(contractItemModel.ResultCnt + "���� �������� ������ ��ȸ�Ǿ����ϴ�.");
					
					if (contractItemDs.ContractItemDetail.Rows.Count == 0) return; // ������ ���� 0�̹Ƿ� �Ʒ� ����ó�� �� �ʿ䵵 ���� return.bae

                    //���� ���೻�� ����
                    ebContractSeq.Text              = contractItemDs.ContractItemDetail.Rows[0]["ContractSeq"].ToString();
                    ebItemName.Text                 = contractItemDs.ContractItemDetail.Rows[0]["ItemName"].ToString();
                    ebContractName.Text             = contractItemDs.ContractItemDetail.Rows[0]["ContractName"].ToString();
                    cbAdState.SelectedValue         = contractItemDs.ContractItemDetail.Rows[0]["AdState"].ToString();
                    string ContStartDay           = contractItemDs.ContractItemDetail.Rows[0]["ExcuteStartDay"].ToString();
					if(ContStartDay.Length == 8)
					{
						int yyyy = Convert.ToInt32(ContStartDay.Substring(0, 4));
						int mm   = Convert.ToInt32(ContStartDay.Substring(4, 2));
						int dd   = Convert.ToInt32(ContStartDay.Substring(6, 2));
						meExcuteStartDay.Value = new DateTime(yyyy,mm,dd);	
					}
					else
					{
						meExcuteStartDay.Value = DateTime.Now;	
					}

					string ContEndDay = contractItemDs.ContractItemDetail.Rows[0]["ExcuteEndDay"].ToString();
					if(ContEndDay.Length == 8)
					{
						int yyyy = Convert.ToInt32(ContEndDay.Substring(0, 4));
						int mm   = Convert.ToInt32(ContEndDay.Substring(4, 2));
						int dd   = Convert.ToInt32(ContEndDay.Substring(6, 2));
						meExcuteEndDay.Value = new DateTime(yyyy,mm,dd);	
					}
					else
					{
						meExcuteEndDay.Value = DateTime.Now;	
					}

					string RealEndDay = contractItemDs.ContractItemDetail.Rows[0]["RealEndDay"].ToString();
					if(RealEndDay.Length == 8)
					{
						int yyyy = Convert.ToInt32(RealEndDay.Substring(0, 4));
						int mm   = Convert.ToInt32(RealEndDay.Substring(4, 2));
						int dd   = Convert.ToInt32(RealEndDay.Substring(6, 2));
						meRealEndDay.Value = new DateTime(yyyy,mm,dd);	
					}
					else
					{
						meRealEndDay.Value = DateTime.Now;	
					}					
                    cbAdClass.SelectedValue         = contractItemDs.ContractItemDetail.Rows[0]["AdClass"].ToString();
                    cbAdType.SelectedValue          = contractItemDs.ContractItemDetail.Rows[0]["AdType"].ToString();
                    cbAdRate.SelectedValue          = contractItemDs.ContractItemDetail.Rows[0]["AdRate"].ToString();
                    //cbMgrade.SelectedValue          = contractItemDs.ContractItemDetail.Rows[0]["Mgrade"].ToString();
                    ebAdTime.Text                   = contractItemDs.ContractItemDetail.Rows[0]["AdTime"].ToString();
                    ebRegDt.Text                    = contractItemDs.ContractItemDetail.Rows[0]["RegDt"].ToString();
                    ebModDt.Text                    = contractItemDs.ContractItemDetail.Rows[0]["ModDt"].ToString();
                    ebRegName.Text                  = contractItemDs.ContractItemDetail.Rows[0]["RegName"].ToString();

                    string sScheduleType            = contractItemDs.ContractItemDetail.Rows[0]["ScheduleType"].ToString(); //�߰� 
                    keySchType = sScheduleType;                         // �߰�
                    cbScheduleType.SelectedValue    = sScheduleType;    //�߰� 

                    //���� ���� ����
                    ebFileName.Text                 = contractItemDs.ContractItemDetail.Rows[0]["FileName"].ToString();
                    ebFileState.Text                = contractItemDs.ContractItemDetail.Rows[0]["FileStateName"].ToString();
                    ebFileType.Text                 = contractItemDs.ContractItemDetail.Rows[0]["FileTypeName"].ToString();

					long fileLen = 0;
					try
					{
						fileLen = Convert.ToInt64(contractItemDs.ContractItemDetail.Rows[0]["FileLength"].ToString());
					}
					catch (Exception)
					{
						fileLen = 0;
					}
					ebFileLength.Text = fileLen.ToString("##,##0");

                    ebDownLevel.Text                = contractItemDs.ContractItemDetail.Rows[0]["DownLevel"].ToString();
                    ebFileRegDt.Text                = contractItemDs.ContractItemDetail.Rows[0]["FileRegDt"].ToString();
                    ebFileRegName.Text              = contractItemDs.ContractItemDetail.Rows[0]["FileRegName"].ToString();
                    ebFilePath.Text                 = contractItemDs.ContractItemDetail.Rows[0]["FilePath"].ToString();
                    //������� ����
                    ebContractName2.Text             = contractItemDs.ContractItemDetail.Rows[0]["ContractName"].ToString();
                    ebContractState.Text            = contractItemDs.ContractItemDetail.Rows[0]["State"].ToString();
                    ebMedia.Text                    = contractItemDs.ContractItemDetail.Rows[0]["MediaName"].ToString();
                    ebRap.Text                      = contractItemDs.ContractItemDetail.Rows[0]["RapName"].ToString();
                    ebAgency.Text                   = contractItemDs.ContractItemDetail.Rows[0]["AgencyName"].ToString();
                    ebContStartDay.Text             = contractItemDs.ContractItemDetail.Rows[0]["ContStartDay"].ToString();
                    ebContEndDay.Text               = contractItemDs.ContractItemDetail.Rows[0]["ContEndDay"].ToString();
                    ebComment.Text                  = contractItemDs.ContractItemDetail.Rows[0]["Comment"].ToString();
                    ebContractRegDt.Text            = contractItemDs.ContractItemDetail.Rows[0]["ContractRegDt"].ToString();
                    ebContractModDt.Text            = contractItemDs.ContractItemDetail.Rows[0]["ContractModDt"].ToString();
                    ebContractRegName.Text          = contractItemDs.ContractItemDetail.Rows[0]["ContractRegName"].ToString();
                    
                    #region ���� �� �� (������� ������)
                    /*
					string HomeYn           = contractItemDs.ContractItemDetail.Rows[0]["HomeYn"].ToString();
					if(HomeYn.Equals("Y"))  rbHome.Checked = true;						
					else                    rbHome.Checked = false;

					string ChannelYn             = contractItemDs.ContractItemDetail.Rows[0]["ChannelYn"].ToString();
					if(ChannelYn.Equals("Y"))   rbChannel.Checked = true;						
					else                        rbChannel.Checked = false;
					
					// Cug item ���� [E_02] 
					string CugYn            = contractItemDs.ContractItemDetail.Rows[0]["CugYn"].ToString();
					if (CugYn.Equals("Y"))  rbCug.Checked = true;
					else                    rbCug.Checked = false;
                    */
                    #endregion

                    #region ���� �� ��
                    /*����������� ������
					// ��ž����:�������� �ƴϰ�, HDD�ִ� ��ž(����,����) ���� ��ž(�Ｚ,�ű�)
					string STBType = contractItemDs.ContractItemDetail.Rows[0]["STBType"].ToString();
					if (STBType.Equals("0"))
					{
						rbSTB_All.Checked = true;
						rbSTB_Default.Checked = false;
						rbSTB_SamSung.Checked = false;
					}
					else if (STBType.Equals("1"))
					{
						rbSTB_All.Checked = false;
						rbSTB_Default.Checked = true;
						rbSTB_SamSung.Checked = false;
					}
					else if (STBType.Equals("2"))
					{
						rbSTB_All.Checked = false;
						rbSTB_Default.Checked = false;
						rbSTB_SamSung.Checked = true;
					}
					else 
					{
						rbSTB_All.Checked = false;
						rbSTB_Default.Checked = false;
						rbSTB_SamSung.Checked = false;
					}
					*/
                    #endregion

                    MediaCode = contractItemDs.ContractItemDetail.Rows[0]["MediaCode"].ToString();

					AdState = cbAdState.SelectedValue.ToString();
					AdType  =  cbAdType.SelectedValue.ToString();

                    SetContactHistoryDetailText();
                    SelectedSetCotrol(sScheduleType);
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
		/// ��ü �������� ��ȸ(������������� ������)
		/// </summary>
		private void SearchFileInfo()
		{
            /* ������� ���� - �������������
			try
			{
				if (lblError.Visible) lblError.Visible = false;

				contractItemModel.Init();

				new ContractItemManager(systemModel, commonModel).GetFileInfo(contractItemModel);

				if (contractItemModel.ResultCD.Equals("0000"))
				{
					int		totCount = Convert.ToInt32(contractItemModel.ContractItemDataSet.Tables[0].Rows[0]["totalCount"].ToString());
					int		totCountN = Convert.ToInt32(contractItemModel.ContractItemDataSet.Tables[0].Rows[0]["totalCountNoFile"].ToString());
					long	totFile = Convert.ToInt64(contractItemModel.ContractItemDataSet.Tables[0].Rows[0]["totalFile"].ToString());

					int		homeCount = Convert.ToInt32(contractItemModel.ContractItemDataSet.Tables[0].Rows[0]["homeCount"].ToString());
					long	homeFile = Convert.ToInt64(contractItemModel.ContractItemDataSet.Tables[0].Rows[0]["homeFile"].ToString());

					int		homeCount1 = Convert.ToInt32(contractItemModel.ContractItemDataSet.Tables[0].Rows[0]["homeCount1"].ToString());
					long	homeFile1 = Convert.ToInt64(contractItemModel.ContractItemDataSet.Tables[0].Rows[0]["homeFile1"].ToString());

					int		homeCount2 = Convert.ToInt32(contractItemModel.ContractItemDataSet.Tables[0].Rows[0]["homeCount2"].ToString());
					long	homeFile2 = Convert.ToInt64(contractItemModel.ContractItemDataSet.Tables[0].Rows[0]["homeFile2"].ToString());

					int homeCount3 = Convert.ToInt32(contractItemModel.ContractItemDataSet.Tables[0].Rows[0]["homeCount0"].ToString());
					long homeFile3 = Convert.ToInt64(contractItemModel.ContractItemDataSet.Tables[0].Rows[0]["homeFile0"].ToString());
					
					long homeMbyte1 = (homeFile3 +	homeFile2	+ homeFile1	) / 1000000;
					long homeMbyte2 = (homeFile3 +	homeFile2	+	0		) / 1000000;
					long homeMbyte3 = (homeFile3 +	0			+	0		) / 1000000;

					lblFileSizeT.Text = string.Format("�������� ({0})",totCountN);
					txtTotalCount.Text = string.Format("{0:##,#0}",totCount);
					txtTotalSize.Text = string.Format("{0:##,#0}", totFile);
					txtHomeCount.Text = string.Format("{0:##,#0}", homeCount);
					txtHomeSize.Text = string.Format("{0:##,#0}", homeFile);

					txtHomeCount1.Text = string.Format("{0:##,#0}({1})", homeCount3 + homeCount2 + homeCount1, homeCount1);
					txtHomeSize1.Text = string.Format("{0:##,#0}({1:##,#0})", homeMbyte1, homeFile1/1000000);

					txtHomeCount2.Text = string.Format("{0:##,#0}({1})", homeCount3 + homeCount2, homeCount2);
					if( homeFile2 > 0 )
						txtHomeSize2.Text = string.Format("{0:##,#0}({1:##,#0})", homeMbyte2, homeFile2 / 1000 / 1000);
					else
						txtHomeSize2.Text = string.Format("{0:##,#0}({1:##,#0})", homeMbyte2, 0);

					txtHomeCount3.Text = string.Format("{0:##,#0}", homeCount3);
					txtHomeSize3.Text = string.Format("{0:##,#0}", homeMbyte3);

					if (homeMbyte2 > 1500)
					{
						lblError.Text = "[�Ｚ]��ž�� ������ ����Size�� ���Ѱ��� ��ȭ�մϴ�.!!!!!";
						if (!lblError.Visible) lblError.Visible = true;
					}
					else if (homeMbyte3 > 1000)
					{
						lblError.Text = "[����] ��ž�� ������ ����Size�� ���Ѱ��� ��ȭ�մϴ�.!!!!!";
						if (!lblError.Visible) lblError.Visible = true;
					}
				}
			}
			catch (FrameException fe)
			{
				FrameSystem.showMsgForm("��ü�������� ��ȸ����", new string[] { fe.ErrCode, fe.ResultMsg });
			}
			catch (Exception ex)
			{
				FrameSystem.showMsgForm("��ü�������� ��ȸ����", new string[] { "", ex.Message });
			}
            */
		}

        /// <summary>
        /// ������������ ����
        /// </summary>
        private void SaveContractItemDetail()
        {
			string AdTypeChangeType = "0"; // ������������ ���� 0:������� 1:�ʼ�->�ɼ� 2:�ɼ�->�ʼ�
           
            if(ebItemName.Text.Trim().Length == 0) 
            {
				MessageBox.Show("������� �Էµ��� �ʾҽ��ϴ�.","������ ����", MessageBoxButtons.OK, MessageBoxIcon.Information );
				ebItemName.Focus();
				return;	               
            }

            if(ebContractName.Text.Trim().Length == 0) 
            {
				MessageBox.Show("��������� �Էµ��� �ʾҽ��ϴ�.\n��༱�� ��ư�� ������ �������ּ���.","������ ����", MessageBoxButtons.OK, MessageBoxIcon.Information );               
                ebContractName.Focus();
                return;
            }

			if(cbAdState.SelectedValue.ToString().Equals("40"))
			{
				DialogResult result = MessageBox.Show("��������� ����� ������ ��� ���ϻ��´� ��ž������ ����˴ϴ�.","������º���", MessageBoxButtons.YesNo, MessageBoxIcon.Question,MessageBoxDefaultButton.Button2);
				if (result == DialogResult.No) return;
			}

			// ���������� ����Ǿ���
			// ������°� 10:��Ⱑ �ƴ϶�� ���޽����� ǥ���Ѵ�.
			if(!AdState.Equals("10") && !AdType.Equals(cbAdType.SelectedValue.ToString()))
			{
				DialogResult result = MessageBox.Show("������� �ƴ� ������ ���������� �����ϸ� �ش� ������ ������������ �ϰ������˴ϴ�.","������������", 
					MessageBoxButtons.YesNo, MessageBoxIcon.Question,
					MessageBoxDefaultButton.Button2);

				if (result == DialogResult.No) return;
					
				if(AdType.StartsWith("1")) // 10~19���̰� �ʼ������̴�.
				{
					// �ʼ������� �ʼ����� �ƴѰ�(�ɼǱ���)�� ����Ǹ�
					if(!cbAdType.SelectedValue.ToString().StartsWith("1"))
					{
						AdTypeChangeType = "1";
					}
				}
				else 
				{
					// �ɼǱ����� �ʼ������ ����Ǹ�
					if(cbAdType.SelectedValue.ToString().StartsWith("1"))
					{
						AdTypeChangeType = "2";
					}
				}
			}
                        
			// ����ð� üũ[E_03]
			try
			{
				Convert.ToInt32(ebAdTime.Text.Trim()); //����
				
				if (ebAdTime.Text == null || ebAdTime.Text.Trim().Length < 0)
					throw new Exception();
			}
			catch(Exception)
			{
				MessageBox.Show("���� �ð��� �ش�Ǵ� ���� �Է��ϼ���!","����ð�",MessageBoxButtons.OK ,MessageBoxIcon.Information);
				return;
			}

            try
            {
                
                contractItemModel.Init();
                // �����͸𵨿� ������ ������ ��Ʈ�Ѵ�.
                //���� ���� 
                contractItemModel.ContractSeq     = ebContractSeq.Text.ToString(); 

                //
                contractItemModel.MediaCode      =MediaCode;     
                contractItemModel.RapCode        =RapCode;       
                contractItemModel.AgencyCode     =AgencyCode;    
                contractItemModel.AdvertiserCode =AdvertiserCode;				
                //���౤�� �������
                contractItemModel.ItemName        = ebItemName.Text.ToString();
                contractItemModel.AdState         = cbAdState.SelectedValue.ToString();
                //��¥�� 8�ڸ� �ٲپ��ش�.
                contractItemModel.ExcuteStartDay  = meExcuteStartDay.Value.ToString("yyyyMMdd");
                contractItemModel.ExcuteEndDay    = meExcuteEndDay.Value.ToString("yyyyMMdd");
                contractItemModel.RealEndDay      = meRealEndDay.Value.ToString("yyyyMMdd");
                
                //contractItemModel.ScheduleType    = "20";		// �⺻���� 20:����ä�η� ��Ʈ�Ѵ�.
                contractItemModel.ScheduleType    = cbScheduleType.SelectedValue.ToString();

                contractItemModel.AdClass         = cbAdClass.SelectedValue.ToString();
                contractItemModel.AdType          = cbAdType.SelectedValue.ToString();
                contractItemModel.AdRate          = cbAdRate.SelectedValue.ToString();
                contractItemModel.AdTime          = ebAdTime.Text.ToString();			
				//ChannelNo�� Int���ε�..TR�� '�Է��� ���ڿ��� ���� �߸��Ǿ��ٴ� ������ ����..' �� ã�ƺ����� �κ��̴�.(08.03.21)
				//�̻��ϴ� �и� int���� ���� ���� ����(���������� �׽�Ʈ����)�� �����µ�..TYPE�� VARCHAR�� �ѵ�..�����͸� �Ѱ� �μ�Ʈ
				//�� �� �ٽ� int������ �ٲٴ� ����� ���ư���..���ذ� �ȵǳ�..�����Ѱ� DB�� type�� �ٲ��� ���ε�..(08.03.31)			
				//contractItemModel.LinkChannel	  = ebLinkChannelNo.Text;
								
                //contractItemModel.Mgrade          = cbMgrade.SelectedValue.ToString();

                #region ���� �� ��
                /* ����� �� ��� ����
				//Ȩ��뿩��
				if(rbHome.Checked)      contractItemModel.HomeYn       = "Y";
				else                    contractItemModel.HomeYn       = "N";

				//ä�λ�뿩��
				if(rbChannel.Checked)   contractItemModel.ChannelYn       = "Y";
				else                    contractItemModel.ChannelYn       = "N";
				     				
				// Cug ���� ����
				if (rbCug.Checked) contractItemModel.CugYn = "Y";
				else               contractItemModel.CugYn = "N";

				// ��ž���� ����
				if (rbSTB_All.Checked)
					contractItemModel.STBType = 0;
				else if (rbSTB_Default.Checked)
					contractItemModel.STBType = 1;
				else if (rbSTB_SamSung.Checked)
					contractItemModel.STBType = 2;
				else
					contractItemModel.STBType = 1;
				*/

                #endregion

                // ������������ ����
				contractItemModel.AdTypeChangeType = AdTypeChangeType;

				if(meExcuteStartDay.Value > meExcuteEndDay.Value)
				{
					MessageBox.Show("�������� �����Ϻ��� �۽��ϴ�.","������ ����", MessageBoxButtons.OK, MessageBoxIcon.Information );
					meExcuteEndDay.Focus();
					return;		
				}

                if (!contractItemModel.AdType.Equals("19"))
                {
                    if (contractItemModel.ScheduleType.Equals("40"))
                    {
                        MessageBox.Show("���Ÿ���� ���������϶��� ������ �� �ֽ��ϴ�.", "��Ÿ�� ����", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        meExcuteEndDay.Focus();
                        return;		
                    }
                }
                
                cbScheduleType.Enabled = true;
           
                // ������� ������ ���� ���񽺸� ȣ���Ѵ�.
                if (IsAdding)
                {
                    new ContractItemManager(systemModel,commonModel).SetContractItemAdd(contractItemModel);
                    StatusMessage("������� ������ �߰��Ǿ����ϴ�.");

                    ResetDetailText();
                    IsAdding = false;
                    ResetDetailText();
                }
                else
                {   
                    //������ �ƴϰ� ������Ʈ�ϰ�쿡�� ������Key�� ����
                    contractItemModel.ItemNo   = keyItemNo;				
                    new ContractItemManager(systemModel,commonModel).SetContractItemUpdate(contractItemModel);
                    StatusMessage("������� ������ ����Ǿ����ϴ�.");
                    
                }

				// 2007.11.21 RH.Jung
				// ������°� ����(40)�� ��� ������ ��ž���� ó���Ѵ�.
				if(contractItemModel.AdState.Equals("40"))
				{
					adFileWideModel.Init();

					adFileWideModel.MediaCode   = MediaCode;
					adFileWideModel.ItemNo		= keyItemNo;						
					adFileWideModel.ItemName	= ebItemName.Text.ToString();						
					adFileWideModel.FileName	= ebFileName.Text.ToString();		
				
					new AdFileWideManager(systemModel,commonModel).SetAdFileSTBDelete(adFileWideModel);						

				}

                MediaCode = "";     
                RapCode = "";       
                AgencyCode = "";    
                AdvertiserCode = "";
				
				SetDetailText();
                DisableButton();				   
                SearchContractItem();
                InitButton();
                        
            }
            catch(FrameException fe)
            {
                FrameSystem.showMsgForm("������ �������", new string[] {fe.ErrCode, fe.ResultMsg});
            }
            catch(Exception ex)
            {
                FrameSystem.showMsgForm("������ �������",new string[] {"",ex.Message});
            }			
        }

		/// <summary>
		/// ����ø������� ����
		/// </summary>
		private void SaveLinkChannel(int linkType)
		{
            /*
			StatusMessage("������ ������ �����մϴ�.");
            			                        
			try
			{
                
				contractItemModel.Init();
				// �����͸𵨿� ������ ������ ��Ʈ�Ѵ�.
				
				// ������� ������ ���� ���񽺸� ȣ���Ѵ�.
//				if (IsAdding)
//				{
                    contractItemModel.ItemNo = keyItemNo;
                    contractItemModel.LinkType = keyLinkType;

                    if (linkType == 1)
                    {
                        if (!ebLinkChannelNo.Text.Equals(null) && !ebLinkChannelNo.Text.Equals(""))
                        {
                            contractItemModel.LinkChannel = ebLinkChannelNo.Text;

                            new ContractItemManager(systemModel, commonModel).SetLinkChannelCreate(contractItemModel);
                            StatusMessage("����ø��� ������ �߰��Ǿ����ϴ�.");
                            contractItemModel.LinkChannel = "";
                        }
                        if (!ebLinkChannelNo2.Text.Equals(null) && !ebLinkChannelNo2.Text.Equals(""))
                        {
                            contractItemModel.LinkChannel = ebLinkChannelNo2.Text;

                            new ContractItemManager(systemModel, commonModel).SetLinkChannelCreate(contractItemModel);
                            StatusMessage("����ø��� ������ �߰��Ǿ����ϴ�.");
                            contractItemModel.LinkChannel = "";
                        }
                        if (!ebLinkChannelNo3.Text.Equals(null) && !ebLinkChannelNo3.Text.Equals(""))
                        {
                            contractItemModel.LinkChannel = ebLinkChannelNo3.Text;

                            new ContractItemManager(systemModel, commonModel).SetLinkChannelCreate(contractItemModel);
                            StatusMessage("����ø��� ������ �߰��Ǿ����ϴ�.");
                            contractItemModel.LinkChannel = "";
                        }
                        if (!ebLinkChannelNo4.Text.Equals(null) && !ebLinkChannelNo4.Text.Equals(""))
                        {
                            contractItemModel.LinkChannel = ebLinkChannelNo4.Text;

                            new ContractItemManager(systemModel, commonModel).SetLinkChannelCreate(contractItemModel);
                            StatusMessage("����ø��� ������ �߰��Ǿ����ϴ�.");
                            contractItemModel.LinkChannel = "";
                        }
                        if (!ebLinkChannelNo5.Text.Equals(null) && !ebLinkChannelNo5.Text.Equals(""))
                        {
                            contractItemModel.LinkChannel = ebLinkChannelNo5.Text;

                            new ContractItemManager(systemModel, commonModel).SetLinkChannelCreate(contractItemModel);
                            StatusMessage("����ø��� ������ �߰��Ǿ����ϴ�.");
                            contractItemModel.LinkChannel = "";
                        }
                    }
                    else if (linkType == 2)
                    {
                        if (!ebLink2ChannelNo.Text.Equals(null) && !ebLink2ChannelNo.Text.Equals(""))
                        {
                            contractItemModel.LinkChannel = ebLink2ChannelNo.Text;

                            new ContractItemManager(systemModel, commonModel).SetLinkChannelCreate(contractItemModel);
                            StatusMessage("����ø��� ������ �߰��Ǿ����ϴ�.");
                            contractItemModel.LinkChannel = "";
                        }
                        if (!ebLink2ChannelNo2.Text.Equals(null) && !ebLink2ChannelNo2.Text.Equals(""))
                        {
                            contractItemModel.LinkChannel = ebLink2ChannelNo2.Text;

                            new ContractItemManager(systemModel, commonModel).SetLinkChannelCreate(contractItemModel);
                            StatusMessage("����ø��� ������ �߰��Ǿ����ϴ�.");
                            contractItemModel.LinkChannel = "";
                        }
                        if (!ebLink2ChannelNo3.Text.Equals(null) && !ebLink2ChannelNo3.Text.Equals(""))
                        {
                            contractItemModel.LinkChannel = ebLink2ChannelNo3.Text;

                            new ContractItemManager(systemModel, commonModel).SetLinkChannelCreate(contractItemModel);
                            StatusMessage("����ø��� ������ �߰��Ǿ����ϴ�.");
                            contractItemModel.LinkChannel = "";
                        }
                        if (!ebLink2ChannelNo4.Text.Equals(null) && !ebLink2ChannelNo4.Text.Equals(""))
                        {
                            contractItemModel.LinkChannel = ebLink2ChannelNo4.Text;

                            new ContractItemManager(systemModel, commonModel).SetLinkChannelCreate(contractItemModel);
                            StatusMessage("����ø��� ������ �߰��Ǿ����ϴ�.");
                            contractItemModel.LinkChannel = "";
                        }
                        if (!ebLink2ChannelNo5.Text.Equals(null) && !ebLink2ChannelNo5.Text.Equals(""))
                        {
                            contractItemModel.LinkChannel = ebLink2ChannelNo5.Text;

                            new ContractItemManager(systemModel, commonModel).SetLinkChannelCreate(contractItemModel);
                            StatusMessage("����ø��� ������ �߰��Ǿ����ϴ�.");
                            contractItemModel.LinkChannel = "";
                        }
                    }
				
				//SetDetailText();
				DisableButton();				   
				SearchContractItem();
				InitButton();
                        
			}
			catch(FrameException fe)
			{
				FrameSystem.showMsgForm("������ �������", new string[] {fe.ErrCode, fe.ResultMsg});
			}
			catch(Exception ex)
			{
				FrameSystem.showMsgForm("������ �������",new string[] {"",ex.Message});
			}		
	        */
	
		}

        /// <summary>
        /// ������ ����
        /// </summary>
        private void DeleteContractItem()
        {
            StatusMessage("������ ������ �����մϴ�.");
        
            if(keyItemNo.Trim().Length == 0) 
            {
                FrameSystem.showMsgForm("���������� ��������",new string[] {"", "������ ������ ������ �����ϴ�.", "" });
                return;
            }
                

            DialogResult result = MessageBox.Show("�ش� ������ ������ ���� �Ͻðڽ��ϱ�?","������ ����",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question,
                MessageBoxDefaultButton.Button2);

            if (result == DialogResult.No) return;

            try 
            {
                // �����͸𵨿� ������ ������ ��Ʈ�Ѵ�.
                contractItemModel.ItemNo = keyItemNo;

                // ��ü���౤���� ������ ���� ���񽺸� ȣ���Ѵ�.
                new ContractItemManager(systemModel,commonModel).SetContractItemDelete(contractItemModel);
                StatusMessage("������ ������ �����Ǿ����ϴ�.");			

                ResetDetailText();
                DisableButton();
                SearchContractItem();
                InitButton();

            }
            catch(FrameException fe)
            {
                FrameSystem.showMsgForm("���������� ��������", new string[] {fe.ErrCode, fe.ResultMsg});
            }
            catch(Exception ex)
            {
                FrameSystem.showMsgForm("���������� ��������",new string[] {"",ex.Message});
            }		
        }

		/// <summary>
		/// ����ø��� ���� - ������� ������ ����(�ּ�ó��)
		/// </summary>
        private void DeleteLinkChannel(int LinkType)
		{
            /*
			StatusMessage("����ø��� ������ �����մϴ�.");

            if (LinkType == 1)
            {
                if (ebLinkChannelNo.Text.Trim().Length == 0)
                {
                    FrameSystem.showMsgForm("����ø������� ��������", new string[] { "", "������ ����ø��� ������ �����ϴ�.", "" });
                    return;
                }
            }
            else if (LinkType == 2)
            {
                if (ebLink2ChannelNo.Text.Trim().Length == 0)
                {
                    FrameSystem.showMsgForm("����ø������� ��������", new string[] { "", "������ ����ø��� ������ �����ϴ�.", "" });
                    return;
                }
            }

			DialogResult result = MessageBox.Show("�ش� ����ø��� ������ ���� �Ͻðڽ��ϱ�?","����ø��� ����",
				MessageBoxButtons.YesNo, MessageBoxIcon.Question,
				MessageBoxDefaultButton.Button2);

			if (result == DialogResult.No) return;

			try 
			{
				// �����͸𵨿� ������ ������ ��Ʈ�Ѵ�.
                contractItemModel.ItemNo = keyItemNo;
                contractItemModel.LinkType = LinkType;

                if (LinkType == 1)
                {

                    if (chk1.Checked)
                    {
                        contractItemModel.LinkChannel = ebLinkChannelNo.Text;
                        new ContractItemManager(systemModel, commonModel).SetLinkChannelDelete(contractItemModel);
                        StatusMessage("����ø�������1�����Ǿ����ϴ�.");
                    }
                    if (chk2.Checked)
                    {
                        contractItemModel.LinkChannel = ebLinkChannelNo2.Text;
                        new ContractItemManager(systemModel, commonModel).SetLinkChannelDelete(contractItemModel);
                        StatusMessage("����ø�������2 �����Ǿ����ϴ�.");
                    }
                    if (chk3.Checked)
                    {
                        contractItemModel.LinkChannel = ebLinkChannelNo3.Text;
                        new ContractItemManager(systemModel, commonModel).SetLinkChannelDelete(contractItemModel);
                        StatusMessage("����ø�������3 �����Ǿ����ϴ�.");
                    }
                    if (chk4.Checked)
                    {
                        contractItemModel.LinkChannel = ebLinkChannelNo4.Text;
                        new ContractItemManager(systemModel, commonModel).SetLinkChannelDelete(contractItemModel);
                        StatusMessage("����ø�������4 �����Ǿ����ϴ�.");
                    }
                    if (chk5.Checked)
                    {
                        contractItemModel.LinkChannel = ebLinkChannelNo5.Text;
                        new ContractItemManager(systemModel, commonModel).SetLinkChannelDelete(contractItemModel);
                        StatusMessage("����ø�������5 �����Ǿ����ϴ�.");
                    }
                }
                else if(LinkType == 2)
                {

                    if (ckbLink2Channel.Checked)
                    {
                        contractItemModel.LinkChannel = ebLink2ChannelNo.Text;
                        new ContractItemManager(systemModel, commonModel).SetLinkChannelDelete(contractItemModel);
                        StatusMessage("����ø�������1�����Ǿ����ϴ�.");
                    }
                    if (ckbLink2Channel2.Checked)
                    {
                        contractItemModel.LinkChannel = ebLink2ChannelNo2.Text;
                        new ContractItemManager(systemModel, commonModel).SetLinkChannelDelete(contractItemModel);
                        StatusMessage("����ø�������2 �����Ǿ����ϴ�.");
                    }
                    if (ckbLink2Channel3.Checked)
                    {
                        contractItemModel.LinkChannel = ebLink2ChannelNo3.Text;
                        new ContractItemManager(systemModel, commonModel).SetLinkChannelDelete(contractItemModel);
                        StatusMessage("����ø�������3 �����Ǿ����ϴ�.");
                    }
                    if (ckbLink2Channel4.Checked)
                    {
                        contractItemModel.LinkChannel = ebLink2ChannelNo4.Text;
                        new ContractItemManager(systemModel, commonModel).SetLinkChannelDelete(contractItemModel);
                        StatusMessage("����ø�������4 �����Ǿ����ϴ�.");
                    }
                    if (ckbLink2Channel5.Checked)
                    {
                        contractItemModel.LinkChannel = ebLink2ChannelNo5.Text;
                        new ContractItemManager(systemModel, commonModel).SetLinkChannelDelete(contractItemModel);
                        StatusMessage("����ø�������5 �����Ǿ����ϴ�.");
                    }
                }
				
				ResetDetailText();
//				DisableButton();
				SearchContractItem();
				InitButton();

			}
			catch(FrameException fe)
			{
				FrameSystem.showMsgForm("����ø������� ��������", new string[] {fe.ErrCode, fe.ResultMsg});
			}
			catch(Exception ex)
			{
				FrameSystem.showMsgForm("����ø������� ��������",new string[] {"",ex.Message});
			}		
            */
		}
		
        /// <summary>
        /// ������ �������� ��Ʈ
        /// </summary>
        private void SetDetailText()
        {
            int curRow = cm.Position;

            if(curRow >= 0 )
            {	
                IsNotLoading = false;	// ��ȸ�� �ٽ� ��ȸ�Ǵ� ���� ������.
                try
                {
                    // Key��Ʈ
                    keyItemNo        = dt.Rows[curRow]["ItemNo"].ToString();
					rowItemNo        = dt.Rows[curRow]["ItemNo"].ToString();
                    keyAdverNo = dt.Rows[curRow]["AdvertiserCode"].ToString();
                    //string adSchType = dt.Rows[curRow]["ScheduleType"].ToString();
                    string adType = dt.Rows[curRow]["AdType"].ToString();
                    
                    /* �������α׷� ���� �� ��
					if(dt.Rows[curRow]["AdClass"].ToString() == "20") // OAP�� (ä��)�ΰ�쿡��
					{
                        //����ø���
                        btnLinkChannel.Enabled  = true;
                        btnLinkChannel2.Enabled = true;
                        btnLinkChannel3.Enabled = true;
                        btnLinkChannel4.Enabled = true;
                        btnLinkChannel5.Enabled = true;
                    
                        //����������
                        btnLink2Channel.Enabled  = true;
                        btnLink2Channel2.Enabled = true;
                        btnLink2Channel3.Enabled = true;
                        btnLink2Channel4.Enabled = true;
                        btnLink2Channel5.Enabled = true;
                    }
					else
					{
                        //����ø���
                        btnLinkChannel.Enabled  = false;
						btnLinkChannel2.Enabled = false;
						btnLinkChannel3.Enabled = false;
						btnLinkChannel4.Enabled = false;
						btnLinkChannel5.Enabled = false;

                        //����������
                        btnLink2Channel.Enabled  = false;
                        btnLink2Channel2.Enabled = false;
                        btnLink2Channel3.Enabled = false;
                        btnLink2Channel4.Enabled = false;
                        btnLink2Channel5.Enabled = false;
                    }
					*/
                    contractItemDs.ContractItemDetail.Clear();
					contractItemDs.LinkChannel.Clear();
                    contractItemDs.LinkItem.Clear();

					//BMK ������ ���� �о���� �κ�
                    SearchContractItemDetail();
					//������ SearchLinkChannelDetail(); 
                    //������ SearchLink2ChannelDetail();
					//���� SearchFileInfo();

                    //keySchType=40�̸� ��󿣵�
                    // ���������϶��� �����
                   
                    Application.DoEvents();
					
                    IsAdding    = false;					
                }
                finally
                {
                    IsNotLoading = true;
                }
            }
            StatusMessage("�غ�");

        }

        private void SetContactHistoryDetailText()
        { 
            
			// 2010.05.11 if (cmDetail.Position >= 0) �߰�, bae
			// �����̷� ������ ���� ��� ������ ����(cmDetail.Position ���� -1) 
			// ó���� ���ؼ� �߰� ��.
			if (cmDetail.Position >= 0)
			{
				ebItemName2.Text = dtDetail.Rows[cmDetail.Position]["ItemName"].ToString();
				ebExcuteStartDay2.Text = dtDetail.Rows[cmDetail.Position]["ExcuteStartDay"].ToString();
				ebExcuteEndDay2.Text = dtDetail.Rows[cmDetail.Position]["ExcuteEndDay"].ToString();
				ebScheduleType2.Text = dtDetail.Rows[cmDetail.Position]["ScheduleTypeName"].ToString();
				ebAdState2.Text = dtDetail.Rows[cmDetail.Position]["AdStateName"].ToString();
				ebAdClass2.Text = dtDetail.Rows[cmDetail.Position]["AdClassName"].ToString();
				ebAdRate2.Text = dtDetail.Rows[cmDetail.Position]["AdRate"].ToString();
				ebAdType2.Text = dtDetail.Rows[cmDetail.Position]["AdTypeName"].ToString();
				ebAdTime2.Text = dtDetail.Rows[cmDetail.Position]["AdTime"].ToString();
			}		
            
        }

        private void ResetDetailText()
        {
            keyItemNo    = "";
            //������������
            ebItemName.Text                 =  "";
            ebContractName.Text             =  "";
            cbAdState.SelectedIndex         =  0;
            meExcuteStartDay.Value			= DateTime.Now;	
            meExcuteEndDay.Value			= DateTime.Now.AddMonths(1);	
            meRealEndDay.Value              = DateTime.Now.AddMonths(1);	
            cbAdClass.SelectedValue         =  "30";   // ����뵵 30:���ձ���
            cbAdType.SelectedIndex          =  0;
            cbAdRate.SelectedValue          =  0;
            ebAdTime.Text                   =  "15";
            ebRegDt.Text                    =  "";
            ebModDt.Text                    =  "";
            ebRegName.Text                  =  "";
			//rbSTB_All.Checked = true;


            //���� ���� ����                
            ebFileName.Text                 =  "";			
            ebFileState.Text                =  "";
            ebFileType.Text                 =  "";
            ebFileLength.Text               =  "";
            ebDownLevel.Text                =  "";
            ebFileLength.Text               =  "";
            ebFileRegDt.Text                =  "";
            ebFileRegName.Text              =  "";
            ebFilePath.Text                 =  "";
            //������� ����                 
            ebContractSeq.Text              =  "";
            ebContractName2.Text             =  "";
            ebContractState.Text            =  "";
            ebMedia.Text                    =  "";
            ebRap.Text                      =  "";
            ebAgency.Text                   =  "";
            ebContStartDay.Text             =  "";
            ebContEndDay.Text               =  "";
            ebComment.Text                  =  "";
            ebContractRegDt.Text            =  "";
            ebContractModDt.Text            =  "";
            ebContractRegName.Text          =  "";

            //�����丮 ���� ����
            contractItemDs.ContractItemHistory.Clear();
            ebItemName2.Text = "";
            ebExcuteEndDay2.Text = "";
            ebExcuteStartDay2.Text = "";
            ebScheduleType2.Text = "";
            ebAdState2.Text = "";
            ebAdClass2.Text = "";
            ebAdRate2.Text = "";
            ebAdType2.Text = "";
            ebAdTime2.Text = "";

			AdState = cbAdState.SelectedValue.ToString();
			AdType  =  cbAdType.SelectedValue.ToString();

            /////////////////////
            // �������α׷� -�����ض�
            //chk1.Checked = false;
            //chk2.Checked = false;
            //chk3.Checked = false;
            //chk4.Checked = false;
            //chk5.Checked = false;
            // ����//
        }
		
        /// <summary>
        /// ������ ReadOnly
        /// </summary>
        private void SetTextReadonly()
        {

        }

        /// <summary>
        /// ������ ����������
        /// </summary>
        private void ResetTextReadonly()
        {
  
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
        public void adOn_AddContract(ContractModel contractModel )
        {
            ebContractName.Text = contractModel.ContractName;
            ebItemName.Text = contractModel.ContractName;
            ebContractSeq.Text  = contractModel.ContractSeq;
			//ebAdTime.Text		= contractModel.AdTime;
    
            MediaCode       = contractModel.MediaCode;
            RapCode         = contractModel.RapCode;
            AgencyCode      = contractModel.AgencyCode;
            AdvertiserCode  = contractModel.AdvertiserCode;
			
			if(contractModel.ContractSeq.Equals("0"))
			{
				cbAdType.SelectedValue = "99";
			}
			
			if(contractModel.ContStartDay.Length == 8)
			{
				int yyyy = Convert.ToInt32(contractModel.ContStartDay.Substring(0, 4));
				int mm   = Convert.ToInt32(contractModel.ContStartDay.Substring(4, 2));
				int dd   = Convert.ToInt32(contractModel.ContStartDay.Substring(6, 2));
				meExcuteStartDay.Value = new DateTime(yyyy,mm,dd);	
			}
			else
			{
				meExcuteStartDay.Value = DateTime.Now;	
			}

			if(contractModel.ContEndDay.Length == 8)
			{
				int yyyy = Convert.ToInt32(contractModel.ContEndDay.Substring(0, 4));
				int mm   = Convert.ToInt32(contractModel.ContEndDay.Substring(4, 2));
				int dd   = Convert.ToInt32(contractModel.ContEndDay.Substring(6, 2));
				meExcuteEndDay.Value = DateTime.Now.AddMonths(1);	
				meRealEndDay.Value = DateTime.Now.AddMonths(1);	
			}
			else
			{
				meExcuteEndDay.Value = DateTime.Now.AddMonths(1);	
				meRealEndDay.Value = DateTime.Now.AddMonths(1);	
			}

        }

        #endregion
		
		private void tabContractItem_SelectedTabChanged(object sender, Janus.Windows.UI.Tab.TabEventArgs e)
        {	//����ø���(������) ��
            //if (tabContractItem.SelectedIndex.Equals(6))
            //{
            //    keyLinkType = 2;

            //    btnSave.Visible = false;
            //    btnDelete.Visible = false;
            //    btnAdd.Visible = false;
            //    btnCopy.Visible = false;
            //}
            ////����ø��� ��
            //if (tabContractItem.SelectedIndex.Equals(4))
            //{
            //    keyLinkType = 1;
                
            //    btnSave.Visible = false;
            //    btnDelete.Visible = false;
            //    btnAdd.Visible = false;
            //    btnCopy.Visible = false;
            //}	
			
            ////�����ڷ� ÷�� ��(��ɻ�����)
            //if(tabContractItem.SelectedIndex.Equals(5))
            //{
            //    btnSave.Visible = false;
            //    btnDelete.Visible = false;
            //    btnAdd.Visible = false;		
            //}

			if(tabContractItem.SelectedIndex.Equals(0) || tabContractItem.SelectedIndex.Equals(1) || tabContractItem.SelectedIndex.Equals(2) || tabContractItem.SelectedIndex.Equals(3))
			{
				btnSave.Visible = true;
				btnDelete.Visible = true;
				btnAdd.Visible = true;
                //btnCopy.Visible = true;
			}
		}
		
		#region �ּ�...����ø��� ����(E_01 ���� �ڵ�)
		/*
		private void btnLinkChannel_Click(object sender, System.EventArgs e)
		{
			// ä�� �˻� �˾� ����
			ContractItem_SearchChannelForm pForm = new ContractItem_SearchChannelForm(this);

			pForm.keyMediaCode = MediaCode;
			pForm.popNum = "1";	//���° �˾����� �˷��ش�. ���� ���Ϲ�����..���
			pForm.ShowDialog();
            
			pForm.Dispose();
			pForm = null;	
		}

		private void btnLinkChannel_Click2(object sender, System.EventArgs e)
		{
			// ä�� �˻� �˾� ����
			ContractItem_SearchChannelForm pForm = new ContractItem_SearchChannelForm(this);

			pForm.keyMediaCode = MediaCode;
			pForm.popNum = "2";	//���° �˾����� �˷��ش�. ���� ���Ϲ�����..���
			pForm.ShowDialog();
            
			pForm.Dispose();
			pForm = null;	
		}

		private void btnLinkChannel_Click3(object sender, System.EventArgs e)
		{
			// ä�� �˻� �˾� ����
			ContractItem_SearchChannelForm pForm = new ContractItem_SearchChannelForm(this);

			pForm.keyMediaCode = MediaCode;
			pForm.popNum = "3";	//���° �˾����� �˷��ش�. ���� ���Ϲ�����..���
			pForm.ShowDialog();
            
			pForm.Dispose();
			pForm = null;	
		}

		private void btnLinkChannel_Click4(object sender, System.EventArgs e)
		{
			// ä�� �˻� �˾� ����
			ContractItem_SearchChannelForm pForm = new ContractItem_SearchChannelForm(this);

			pForm.keyMediaCode = MediaCode;
			pForm.popNum = "4";	//���° �˾����� �˷��ش�. ���� ���Ϲ�����..���
			pForm.ShowDialog();
            
			pForm.Dispose();
			pForm = null;	
		}

		private void btnLinkChannel_Click5(object sender, System.EventArgs e)
		{
			// ä�� �˻� �˾� ����
			ContractItem_SearchChannelForm pForm = new ContractItem_SearchChannelForm(this);

			pForm.keyMediaCode = MediaCode;
			pForm.popNum = "5";	//���° �˾����� �˷��ش�. ���� ���Ϲ�����..���
			pForm.ShowDialog();
            
			pForm.Dispose();
			pForm = null;	
		}
       */

        /// <summary>
        /// ������� ������ �����Ұ� �ּ�ó��
        /// </summary>
        /// <param name="ChannelNo"></param>
        /// <param name="ChannelName"></param>
        /// <param name="PopNum"></param>
		public void SetChannel(string ChannelNo, string ChannelName, string PopNum)
		{
            //if (keyLinkType == 1)
            //{
            //    SetLinkChannel(ChannelNo, ChannelName, PopNum);
            //}
            //else
            //{
            //    SetLink2Channel(ChannelNo, ChannelName, PopNum);
            //}
            
		}

        /// <summary>
        /// ������� �����Ұ� -�ּ�ó��
        /// </summary>
        /// <param name="ChannelNo"></param>
        /// <param name="ChannelName"></param>
        /// <param name="PopNum"></param>
        public void SetLinkChannel(string ChannelNo, string ChannelName, string PopNum)
        {
            /*
            if (PopNum.Equals("1"))
            {
                ebLinkChannelNo.Text = ChannelNo;
                ebLinkChannelNm.Text = ChannelName;
            }
            if (PopNum.Equals("2"))
            {
                ebLinkChannelNo2.Text = ChannelNo;
                ebLinkChannelNm2.Text = ChannelName;
            }
            if (PopNum.Equals("3"))
            {
                ebLinkChannelNo3.Text = ChannelNo;
                ebLinkChannelNm3.Text = ChannelName;
            }
            if (PopNum.Equals("4"))
            {
                ebLinkChannelNo4.Text = ChannelNo;
                ebLinkChannelNm4.Text = ChannelName;
            }
            if (PopNum.Equals("5"))
            {
                ebLinkChannelNo5.Text = ChannelNo;
                ebLinkChannelNm5.Text = ChannelName;
            }
            */
        }

        /// <summary>
        /// ������� ������ �����Ұ� �ּ�ó��
        /// </summary>
        /// <param name="ChannelNo"></param>
        /// <param name="ChannelName"></param>
        /// <param name="PopNum"></param>
        public void SetLink2Channel(string ChannelNo, string ChannelName, string PopNum)
        {
            //if (PopNum.Equals("1"))
            //{
            //    ebLink2ChannelNo.Text = ChannelNo;
            //    ebLink2ChannelNm.Text = ChannelName;
            //}
            //if (PopNum.Equals("2"))
            //{
            //    ebLink2ChannelNo2.Text = ChannelNo;
            //    ebLink2ChannelNm2.Text = ChannelName;
            //}
            //if (PopNum.Equals("3"))
            //{
            //    ebLink2ChannelNo3.Text = ChannelNo;
            //    ebLink2ChannelNm3.Text = ChannelName;
            //}
            //if (PopNum.Equals("4"))
            //{
            //    ebLink2ChannelNo4.Text = ChannelNo;
            //    ebLink2ChannelNm4.Text = ChannelName;
            //}
            //if (PopNum.Equals("5"))
            //{
            //    ebLink2ChannelNo5.Text = ChannelNo;
            //    ebLink2ChannelNm5.Text = ChannelName;
            //}
        }
	
		#endregion

		/// <summary>
		/// ������ ���� �׽�Ʈ - ������ �ּ�ó��
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnCopy_Click(object sender, System.EventArgs e)
		{
            ////meExcuteStartDay.Value			= DateTime.Now;	
            ////meExcuteEndDay.Value			= DateTime.Now.AddMonths(1);	
            ////meRealEndDay.Value              = DateTime.Now.AddMonths(1);	
            //AdManagerClient._20_Contract._07_ContractItem.ContractItem_Copy	Form1 = new AdManagerClient._20_Contract._07_ContractItem.ContractItem_Copy();
			
            //Form1.SetItemNo	= Convert.ToInt32( keyItemNo ); 
            //Form1.SetContractItem = ebItemName.Text;
            ////Form1.SetDateBegin	=	meRealEndDay.Value.AddDays(1);

            //DialogResult    result = Form1.ShowDialog(this);
		}
        		
		/// <summary>
		/// ����ø��� ��ư Ŭ�� �ڵ鷯(E_01)//2�������� - ������� ������ �ּ�ó��
		/// </summary>	
		private void btnSetLinkChannel(object sender, System.EventArgs e)
		{	
		    /*
			// 1. ä�� �˻� �� ȣ��
			//    - ������ ��� �� ä�� �� ����
			// 2. ������ ä�� ���� ���� �б�
			//   - 1�� �� ��: ���� �� ��ư�� ��Ʈ�ѿ� ��Ī
			//   - 2�� �̻��� �� ������ ���������� ��Ī ��Ŵ.

			int newRowCount = 0;                                 // �űԷ� ���õ� ä�� ��
			int oldRowCount = contractItemDs.LinkChannel.Rows.Count; // ���� ��� �� ä�� ��

            if (tabContractItem.SelectedTab.Equals(tabPageLink2Channel))
            {
                oldRowCount = contractItemDs.Link2Channel.Rows.Count;
            }

			
			string chlName = "";
			string chlNo = "";
			string tag = "";

			ContractItem_SearchChannelForm pForm = null; // ä�� �˻� ��
			DataTable dt = null;                         // ���� �� ä���� ���� table
            
			try
			{
				// ä�� �˻� �� ����
				pForm = new ContractItem_SearchChannelForm();
                
				// �̹� ��� �� ä�� ��
				pForm.SetExistCount = oldRowCount;              
				
				pForm.keyMediaCode = MediaCode;

				if (pForm.ShowDialog() == DialogResult.Yes) // ���� ���� �Ϸ�
				{
					// ��ư �����ϴ� tag����(1,2,3,4,5 �������� ���� Tag�Ӽ� ���� �Է�ó�� �� ����)
					tag = ((Janus.Windows.EditControls.UIButton)(sender)).Tag.ToString();

					dt = pForm.GetChannelList;
					newRowCount = dt.Rows.Count;

					if (dt != null && newRowCount > 1) 
					{
						// 2�� �̻� ���õǾ��ٸ� �� ����ø��� ������ ��Ī
						foreach(DataRow row in dt.Rows)
						{
							chlNo =  row["ChannelNo"].ToString();
							chlName = row["ChannelName"].ToString();
                            if(keyLinkType == 1)
                            {
							    fillLinkMultiChannel(chlNo, chlName);							
                            }
                            else if(keyLinkType == 2)
                            {
							    fillLink2MultiChannel(chlNo, chlName);							
                            }
						}
					}
					else if (dt != null && newRowCount == 1)
					{
						// ä�� ��ȣ�� ä�� �� 
						chlNo   = dt.Rows[0]["ChannelNo"].ToString();
						chlName = dt.Rows[0]["ChannelName"].ToString(); 
						if(keyLinkType == 1)
							 fillLinkOneChannel(chlNo, chlName, tag);
                        else
                             fillLink2OneChannel(chlNo, chlName, tag);
					}
				}
			}
			catch(Exception ex)
			{
				MessageBox.Show( ex.Message ,"����ø���",MessageBoxButtons.OK ,MessageBoxIcon.Error);
				StatusMessage("");

			}
			finally
			{
				if (pForm != null)
				{
					pForm.Dispose();
					pForm = null;
					GC.Collect();
				}
			}
            */
		}
        		
		/// <summary>
		/// ������ ä���� ������ ��� ��Ʈ�Ѱ� ��Ī(E_01) - ������ �ּ�ó��
		/// </summary>		
		private void fillLinkMultiChannel(string chlNo, string chlName)
		{
            //if (ebLinkChannelNo.Text == "")
            //{
            //    ebLinkChannelNo.Text = chlNo;
            //    ebLinkChannelNm.Text = chlName;
            //    return;
            //}
            //if (ebLinkChannelNo2.Text == "")
            //{
            //    ebLinkChannelNo2.Text = chlNo;
            //    ebLinkChannelNm2.Text = chlName;
            //    return;
            //}
            //if (ebLinkChannelNo3.Text == "")
            //{
            //    ebLinkChannelNo3.Text = chlNo;
            //    ebLinkChannelNm3.Text = chlName;
            //    return;
            //}
            //if (ebLinkChannelNo4.Text == "")
            //{
            //    ebLinkChannelNo4.Text = chlNo;
            //    ebLinkChannelNm4.Text = chlName;
            //    return;
            //}
            //if (ebLinkChannelNo5.Text == "")
            //{
            //    ebLinkChannelNo5.Text = chlNo;
            //    ebLinkChannelNm5.Text = chlName;
            //    return;
            //}
		}
		
		
		/// <summary>
		/// 1���� ä�θ� ������ ��� ��Ʈ�Ѱ� ��Ī(E_01) - ������� ������ �ּ�ó��
		/// </summary>		
		private void fillLinkOneChannel(string chlNo, string chlName, string tag)
		{		
			/*				
			if (tag.Equals("1"))
			{
				ebLinkChannelNo.Text = chlNo;
				ebLinkChannelNm.Text = chlName;
			}
			else if (tag.Equals("2"))
			{
				ebLinkChannelNo2.Text = chlNo;
				ebLinkChannelNm2.Text = chlName; 
			}
			else if (tag.Equals("3"))
			{
				ebLinkChannelNo3.Text = chlNo;
				ebLinkChannelNm3.Text = chlName; 
			}
			else if (tag.Equals("4"))
			{
				ebLinkChannelNo4.Text = chlNo;
				ebLinkChannelNm4.Text = chlName; 
			}
			else if (tag.Equals("5"))
			{
				ebLinkChannelNo5.Text = chlNo;
				ebLinkChannelNm5.Text = chlName; 
			}
            */
		}		
	
		


        /// <summary>
        /// ������ ä���� ������ ��� ��Ʈ�Ѱ� ��Ī(E_04) - ������� ������ �ּ�ó��
        /// </summary>		
        private void fillLink2MultiChannel(string chlNo, string chlName)
        {
            /*
            if (ebLink2ChannelNo.Text == "")
            {
                ebLink2ChannelNo.Text = chlNo;
                ebLink2ChannelNm.Text = chlName;
                return;
            }
            if (ebLink2ChannelNo2.Text == "")
            {
                ebLink2ChannelNo2.Text = chlNo;
                ebLink2ChannelNm2.Text = chlName;
                return;
            }
            if (ebLink2ChannelNo3.Text == "")
            {
                ebLink2ChannelNo3.Text = chlNo;
                ebLink2ChannelNm3.Text = chlName;
                return;
            }
            if (ebLink2ChannelNo4.Text == "")
            {
                ebLink2ChannelNo4.Text = chlNo;
                ebLink2ChannelNm4.Text = chlName;
                return;
            }
            if (ebLink2ChannelNo5.Text == "")
            {
                ebLink2ChannelNo5.Text = chlNo;
                ebLink2ChannelNm5.Text = chlName;
                return;
            }
            */
        }


        /// <summary>
        /// 1���� ä�θ� ������ ��� ��Ʈ�Ѱ� ��Ī(E_04) - �ּ�ó��
        /// </summary>		
        private void fillLink2OneChannel(string chlNo, string chlName, string tag)
        {
            /*
            if (tag.Equals("1"))
            {
                ebLink2ChannelNo.Text = chlNo;
                ebLink2ChannelNm.Text = chlName;
            }
            else if (tag.Equals("2"))
            {
                ebLink2ChannelNo2.Text = chlNo;
                ebLink2ChannelNm2.Text = chlName;
            }
            else if (tag.Equals("3"))
            {
                ebLink2ChannelNo3.Text = chlNo;
                ebLink2ChannelNm3.Text = chlName;
            }
            else if (tag.Equals("4"))
            {
                ebLink2ChannelNo4.Text = chlNo;
                ebLink2ChannelNm4.Text = chlName;
            }
            else if (tag.Equals("5"))
            {
                ebLink2ChannelNo5.Text = chlNo;
                ebLink2ChannelNm5.Text = chlName;
            } */
        }


		/// <summary>
		/// �˾��޴�����(E_01,������ ���� �����ؼ� �������,����ø��� �����ϵ��� �ϴ� �޴�)
		/// </summary>
		private void setPopMenu()
		{
			cmdManager.SetContextMenu(grdExItemList, ctxMenu);																
		}

		/// <summary>
		/// ������� �ϰ�����ó���� �޺��ڽ� �ʱ�ȭ(E_01)
		/// </summary>
		private void initCbMultiAdState()
		{
			try
			{
				cbMultiAdState = ctxMenu.Commands["SetAdState"].GetUIComboBox();				
				cbMultiAdState.SelectedIndexChanged +=new EventHandler(cbMultiAdState_SelectedIndexChanged);

				foreach(DataRow row in contractItemDs.AdState.Rows)
				{
					cbMultiAdState.Items.Add(row["CodeName"].ToString(), row["Code"].ToString());
				}
			}
			catch(Exception)
			{
				throw new Exception("������� �ϰ����� �ʱ�ȭ �����Դϴ�.!");
			}
		}


		/// <summary>
		/// ���� ������ 1�� �̻� ���õ� ������(E_01,������ ���� Message�߻�)
		/// </summary>		
		private void checkMultiRow()
		{
			if (grdExItemList.GetCheckedRows().Length < 1)
			{
				throw new Exception("���౤���Ͽ��� ó�� �� ���� üũ�ϼ���.");				
			}			
		}

		
		/// <summary>
		/// �ϰ� ���� �� ������� �������� üũ(E_01,true:���� ����)
		/// </summary>		
		private bool isNonSelectedAdState()
		{
			if ( cbMultiAdState.SelectedValue == null
				|| cbMultiAdState.SelectedValue.ToString() =="")
				return true;

			return false;
		}


		/// <summary>
		/// ������ ������� �ϰ�����ó�� �ڵ鷯(E_01)
		/// </summary>		
		private void cbMultiAdState_SelectedIndexChanged(object sender, EventArgs e)
		{		
			try
			{				
				// ������¸� �������� ������ �������� ����
				if (isNonSelectedAdState())
					return;
				
				// �Է� �� üũ
				checkMultiRow(); 
				
				if (cbMultiAdState.SelectedValue.ToString() != "")
				{						
					StatusMessage("������� ������ �ϰ����� �մϴ�.");

					// �������:���� �� ��� ���ϻ��°� ��ž������ �ȴٴ� Ȯ������ ��.
					if(cbMultiAdState.SelectedValue.ToString().Equals(AD_STATE_DEL))
					{
						DialogResult result = MessageBox.Show("������¸� ����� ������ ��� ���ϻ��´� ��ž������ ����˴ϴ�.","������º���", MessageBoxButtons.YesNo, MessageBoxIcon.Question,MessageBoxDefaultButton.Button2);
						if (result == DialogResult.No) return;
					}
					
					DialogResult re =  MessageBox.Show("������� ������[" + cbMultiAdState.SelectedItem.Text+ "]"
						                              +"�� �ϰ� �����Ͻðڽ��ϱ�?","������� �ϰ�����"
						                              ,MessageBoxButtons.YesNo, MessageBoxIcon.Question,MessageBoxDefaultButton.Button2);
					if (re == DialogResult.No) return;

					// ���⿡ ���� �����ϴ� ���� �߰� or mehtod ȣ��//
					editMultiAdState( cbMultiAdState.SelectedValue.ToString() );
					StatusMessage("������� ���� �ϰ����� �Ϸ��߽��ϴ�.");
					
					MediaCode = "";     
					RapCode = "";       
					AgencyCode = "";    
					AdvertiserCode = "";
				
					SetDetailText();
					DisableButton();				   
					SearchContractItem();
					InitButton();
				}
			}
			catch(Exception ex)
			{
				MessageBox.Show( ex.Message ,"�ϰ�����"
					,MessageBoxButtons.OK ,MessageBoxIcon.Information);
				StatusMessage("");
			}
			finally
			{
				// ���� ���ýÿ� ���� index�� �����ص� �� �ڵ鷯�� 
				// ����Ǿ�� �ϹǷ� ���Ƿ� -1 ���� �� ��.
				// �����ָ� ���� ���� ����ڰ� click�ص� �ƹ� ���� ����.
				if (cbMultiAdState.Items.Count > 0)
					cbMultiAdState.SelectedIndex = -1;
			}
		}
		
		
		/// <summary>
		/// ���õ� ������ ������� �ϰ� �����ϱ�(E_01,�Ϸ�/���� ��� Message���� ó��)
		/// </summary>
		private void editMultiAdState(string adState)
		{
			// �������:adState ���� '40' �����̸� realEndDay ���� ó���ϴ� ���ϳ�¥������ ����
			string realEndDay = "";
			
			if (adState.Equals(AD_STATE_DEL))
				realEndDay = DateTime.Now.ToString("yyyyMMdd");

			// table layout ����
			DataTable dt = new DataTable("AdState");
			dt.Columns.Add("ItemNo", typeof(string));
			dt.Columns.Add("RealEndDay" ,typeof(string));					
		
			// üũ �� ���౤�� table row �� ����
			foreach(GridEXRow gr in grdExItemList.GetCheckedRows())
			{
				DataRow nRow = dt.NewRow();
				nRow["ItemNo"] = gr.Cells["ItemNo"].Value.ToString();
				nRow["RealEndDay"] = realEndDay;
				dt.Rows.Add(nRow);
			}

			// ������ table DataSet�� �߰�
			ContractItemModel model = new ContractItemModel();
			model.Init();
			model.AdState = adState;
			model.ContractItemDataSet.Tables.Add(dt);
			
			new ContractItemManager(systemModel,commonModel).SetMultiAdState(model);
			if (model.ResultCD.Equals(FrameSystem.DBSuccess))
			{
				MessageBox.Show("������¸� �ϰ� ����ó�� �Ϸ��߽��ϴ�.","������º���"
					,MessageBoxButtons.OK,MessageBoxIcon.Information);
			}					
		}


        /// <summary>
        /// �����̺�Ʈ�� �� �߰� �� ��Ʈ���� ���� �� �� ������ �ʾ� �߰��� ��Ʈ�� 
        /// </summary>
        private void SelectedSetCotrol(string pScheduleType)
        {
            //cbMgrade.Enabled        = true;
            cbAdState.Enabled       = true;
            cbAdClass.Enabled       = true;
            cbAdType.Enabled        = true;
            cbScheduleType.Enabled  = true;

            // ���� �� �� -������
            //if (pScheduleType == "30") rbHome.Enabled = false;
            //else rbHome.Enabled = true;
            //
        }

        //���� ���� �˾�â
        ItemMultiChoiceForm pForm = null;
        /// <summary>
        /// ���ũ���� ���� - ������� ������-�����ּ�ó��
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDualAdd_Click(object sender, EventArgs e)
        {
            /*
			//  Ȩ���� ����� �˻�â 
            pForm = new ItemMultiChoiceForm(this);

			// ��ü�ڵ��Ʈ
			pForm.keyMediaCode = "1";
            pForm.keyAdverCode = keyAdverNo;

            pForm.callType = "ContractItemControl";
            pForm.ReturnDate += new ItemMultiChoiceForm.PopupService(pForm_ReturnDate);
			pForm.ShowDialog();            
			pForm.Dispose();
			pForm = null;
            */
        }

        void pForm_ReturnDate(object sender, EventArgs args)
        {
            ItemEventArgs itemEventArgs = (ItemEventArgs)args;
            ItemMultiChoice_pDs itemMultiChoice_pDs = (ItemMultiChoice_pDs)itemEventArgs.dataSet;

            try
            {
                //int SetCount = 0;
                DataRow row = null;
                for (int i = 0; i < itemMultiChoice_pDs.ChoiceAdItems.Rows.Count; i++)
                {
                    row = itemMultiChoice_pDs.ChoiceAdItems.Rows[i];

                    if (row["CheckYn"].ToString().Equals("True"))
                    {
                        contractItemModel.Init();
                        contractItemModel.ItemNo = keyItemNo;
                        contractItemModel.LinkChannel = row["ItemNo"].ToString();

                        new ContractItemManager(systemModel, commonModel).SetLinkItemCreate(contractItemModel);
                    }
                }

                SearchLinkItemList(true);
            }
            catch (FrameException fe)
            {
                FrameSystem.showMsgForm("��󱤰� ��������", new string[] { fe.ErrCode, fe.ResultMsg });
            }
            catch (Exception ex)
            {
                FrameSystem.showMsgForm("��󱤰� ��������", new string[] { "", ex.Message });
            }	
        }
        
	}
}