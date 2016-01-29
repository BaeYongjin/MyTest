// ===============================================================================
//
// MainForm.cs
//
// ������ 
//
// ===============================================================================
// Release history
// 2007.08.15 RH.Jung Initialize
// �߷�
// 2007.12.12 RH.Jung ������/Ű������ �޴� �߰�
// 2011.10.31 RH.Jung �޴�ǥ�ù�� ���� : ButtonBar -> CommandBar
//                    �޴�ó����� ���� : ȭ��Control�� ó���Լ� -> IUserControl�̿� ���� Class ����
// ===============================================================================
// Copyright (C) 2007 G-Inno Systems Inc.
// All rights reserved.
// ==============================================================================
/*
 * -------------------------------------------------------
 * Class Name: MainForm
 * �ֿ���  :  �޴� ��Ʈ�� �����̳� ����.
 * �ۼ���    : ��
 * �ۼ���    : ��
 * Ư�̻���  : �޴� ��Ʈ�� ȣ�� �ϴ� - ������� �ʴ� �ҽ� ����
 * -------------------------------------------------------
 * [��������]
 * -------------------------------------------------------
 * �����ڵ�  : [E_01]
 * ������    : bae
 * ������    : 2013.04.29
 * ��������  :        
 *            - ctlActive ��� ���� ����
 *              why: iUserCtrl �������̽� ������� ����ϹǷ� �ߺ�����
 *            - ��Ÿ ������� �ʴ� �ҽ� ����  
 * �����Լ�  :
 *            - uiCommandBar_Menu_CommandClick(.)
 *              releaseCtlActive()
 *              progressForm()
 * --------------------------------------------------------
 * �����ڵ�  : [E_02]
 * ������    : bae
 * ������    : 2013.07.10
 * ��������  :        
 *             ��������� ��ð��� �ҿ�ǹǷ� ���� �� �ٸ� �۾���
 *             �����ϵ��� �ϱ����ؼ� ���������� ����� �� ������ ���� ����
 *             �ϵ��� �Ѵ�.
 * �����Լ�  :
 *            Global_TgtCollecton ���� �߰�
 *            Global_TgtRefresh() �Լ� �߰�
 * --------------------------------------------------------
 */

using System;
using System.Diagnostics;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Reflection;
using System.Windows.Forms;
using System.Threading;
using System.Data;

using WinFramework.Misc;
using WinFramework.Base;

using AdManagerModel;

namespace AdManagerClient
{
    /// <summary>
    /// TmsMain�� ���� ��� �����Դϴ�.
    /// </summary>
    public class MainForm : System.Windows.Forms.Form
    {
        #region �޴��Լ� ����ü
        struct Proc
        {
            public string MenuCode;				// �޴��ڵ�
            public string FuncName;				// ó���Լ��� (��ȭ�� Control�� Class��)

            public Proc (string m, string f)
            {
                MenuCode = m;
                FuncName = f;
            }
        }
        #endregion

        #region �޴�ó���Լ� ����Ʈ�� ������ : �޴��߰��� �̰��� ����

        /// <summary>
        /// ó���Լ��� ����Ʈ�̴�
        /// �޴��ڵ尡 �߰��Ǹ� �� ����Ʈ�� �� ó���Լ��� �߰��� �־�� �Ѵ�.
        /// ó���Լ��� �� �ҽ� �ϴ��� ó���Լ� ��Ͽ� �߰��Ѵ�.
        /// 
        /// 2011.10.31 RH.Jung �޴�ó����� ����
        /// ó���Լ����� �ش� ȭ���� Class���̴�. 
        /// 
        /// </summary>
        Proc [] procList = 
        {

            // ��ü����
             new Proc("100100", "MediaInfoControl"   )				// ��ü��������
            ,new Proc("100200", "MediaRapControl"    )				// �̵�����
            ,new Proc("100300", "CategoryControl"    )				// ī�װ�����
            ,new Proc("100400", "GenreControl"       )				// �帣����
            ,new Proc("100500", "ChannelControl"     )				// ä�ΰ���
            ,new Proc("100600", "ContentsControl"    )				// ����������
            ,new Proc("100700", "ChannelSetControl"  )				// ����ڰ���
            ,new Proc("100800", "UserInfoControl"    )				// ����ڰ���
//			,new Proc("100900", "SlotOrganizationControl")          // ���԰���
            ,new Proc("100900", "SlotAdInfoControl"	)               // �������������	2014.09.15    
			,new Proc("101000", "GroupControl"      )			    // �׷����
            ,new Proc("102100", "MediaMenuSetCtrl"	)			    // ��������
            ,new Proc("101100", "GroupOrganizationControl")         // OAP ���׷����	2013.03.
			,new Proc("101200", "MenuControl"       )               // �޴�����     
            ,new Proc("101300", "SchOrgGenreControl")               // ���帣���OAP������
			,new Proc("101400", "MenuMapCtrl"		)               // �޴����ΰ���		2015.10
			
            // ���������
            ,new Proc("200100", "AgencyControl"      )				// ��������
            ,new Proc("200200", "AdvertiserControl"  )				// �����ְ���
            ,new Proc("200300", "MediaAgencyControl" )				// ��ü�� ��������
			,new Proc("200400", "ClientControl"      )				// ����纰 �����ְ���
            ,new Proc("200500", "ContractControl"    )				// ���������
            ,new Proc("200600", "AdFileControl"      )				// �������ϰ���
            ,new Proc("200700", "ContractItemControl")				// ����������
			,new Proc("200800", "AdFileWideControl"      )			// �������Ϲ�������
			,new Proc("200900", "ContractPackageControl" )			// �����ǰ����
			,new Proc("201000", "FilePublishControl" )				// ���Ϲ�������
			,new Proc("201100", "CampaignControl" )			    	// ķ����

            // ����������
//            ,new Proc("300100", "menuGenreGroup"  )				// �޴�����׷����
//            ,new Proc("300200", "menuChannelGroup")				// ä�α���׷����
            ,new Proc("300300", "SchHomeAdControl"     )    		// Ȩ������
            ,new Proc("300310", "SchHomeKidsControl"     )    		// Ȩ����(Ű������)��	2014.11
            ,new Proc("300320", "SchTargetHomeAdControl"     )      // Ȩ������(����)	2015.05
			,new Proc("300350", "SchHomeCmControl"   )				// Ȩ���������
            ,new Proc("300400", "SchChoiceAdControl"   )			// ����������
            ,new Proc("300500", "ChooseAdScheduleControl"   )		// �޴�/ä�κ� ����Ȳ
			,new Proc("300600", "AdStatusControl"    )				// ��������Ȳ
			,new Proc("300700", "SchPublishControl"  )				// ����������
			,new Proc("300800", "SchMenuAdControl"  )				// �޴�������
			,new Proc("300900", "SchAppendAdControl"  )				// �߰�������
			//,new Proc("301000", "SchExcelControl"  )				// ���ϰ����
			,new Proc("301000", "SchRecommControl"  )				// ����õ
			,new Proc("301100", "SchPopularChannelControl"  )		// ����ä����
			,new Proc("301200", "ChannelJumpControl"  )			    // ä�����ΰ���
			,new Proc("301300", "SchKeywordControl"  )				// Ű������
			,new Proc("301400", "SchGroupControl"  )				// �׷���
            ,new Proc("302000", "SchMidAdControl"  )                // �߰������� 2009.07.10
			,new Proc("300450", "SchDesignateAdControl"  )          // ä�κ� ������	2009.09.20
            ,new Proc("303000", "SchExclusiveZoneAdControl")        // �ð��뺰 ������ ��	2011.12.22
            ,new Proc("301500", "SchHomeGroupControl")              // ȨOAP�׷��� 2013.06.

            // Ÿ���ð���
            ,new Proc("400100", "TargetingControl"  )				// ������� Ÿ���ð���
			,new Proc("400150", "TargetingOapControl"  )			// ��ü���� Ÿ���ð���
			,new Proc("400200", "TargetingHomeControl"  )			// Ȩ���� Ÿ���ð���
			,new Proc("400300", "TargetCollectionControl"  )		// Ÿ�ٰ�������  //CollectionHomeControl  /  TargetCollectionControl
            ,new Proc("400330", "CollectionHomeControl"     )    	// Ÿ�ٰ�������(Ȩ����)2015.05
			,new Proc("400350", "CrmControl"			)		    // CRM��������
			,new Proc("400400", "GradeControl"				)		// Ÿ�ٰ�������
			,new Proc("400500", "RatioControl"				)		// Ÿ�ٰ�������
			,new Proc("400600", "UkeyTargetCollectionControl")		// UKeyķ���ΰ���
            ,new Proc("400700", "TargetingCollectionControl")       // ���� Ÿ���ð��� 2012.02.21 RH.Jung
            ,new Proc("400800", "TargetingAdDenyControl")           // ����ź��� ����
		
			// ��ü�м�����
//			,new Proc("500100", "menuPgmDailyRating" )			    // �Ϻ���û��
//			,new Proc("500100", "menuContentRating"  )			    // ��������   ��û������
//			,new Proc("500200", "menuGenreRating"    )			    // �帣��     ��û������
//			,new Proc("500300", "menuCategoryRating" )			    // ī�װ��� ��û������
			,new Proc("500400", "StatisticsPgCategoryControl" )	    // ī�װ����
			,new Proc("500500", "StatisticsPgChannelControl" )	    // ä�����
			,new Proc("500600", "StatisticsPgRegionControl"  )		// ���������
			,new Proc("500700", "StatisticsPgTimeControl"  )		// �ð��뺰���
			,new Proc("500800", "StatisticsPgWeekControl"  )		// ���Ϻ����
			,new Proc("500900", "StatisticsPgAgeControl"  )		    // ���ɺ����

			//�Ѱ�����
			,new Proc("510100", "RptSummaryAdDailyControl"  )		// �����ϰ�����Ʈ
			,new Proc("510200", "RptSummaryAdWeeklyControl"  )		// �����ְ�����Ʈ
//			,new Proc("510300", "menuSummaryAdMonthly"  )		    // �����ְ�����Ʈ
			,new Proc("510400", "DailyAdExecSummaryRptControl"  )	// �����ϰ�����Ʈ
			,new Proc("510500", "RptAdCategorySummaryControl"  )	// �����ϰ�����Ʈ
			,new Proc("510600", "OAPWeekHomeAdControl"  )			// �����ϰ�����Ʈ
			,new Proc("510700", "OAPWeekChannelJumpControl"  )		// �����ϰ�����Ʈ
			,new Proc("510800", "DateAdTypeSummaryRptControl"  )	// �����ϰ�����Ʈ
			,new Proc("510900", "AdTypeMoniteringControl"  )		// �����ϰ�����Ʈ
			,new Proc("511000", "InventoryPresentConditionControl")	// �κ��丮��Ȳ
            ,new Proc("512000", "HomeCmRptControl"  )	            // Ȩ�������^2010.11.08
			,new Proc("513000", "PreferenceTotalizeControl")        // ��ȣ�� ���� �˾� ����( 2013-06-20 )
            ,new Proc("514000", "CombineRptCtrl")                   // ���� ����Ʈ( 2013-06-27 )
            ,new Proc("515000", "BizManageTgtControl")              // ��������

			// �������ຸ��
			,new Proc("600100", "ProgramAdHitControl"  )			// ���α׷��� �����û����
			,new Proc("600200", "DailyProgramHitControl"  )		    // �Ϻ� ���α׷� ��û����
			,new Proc("600300", "DailyAdHitControl"  )				// �Ϻ� ���� ��û����
			,new Proc("600400", "SummaryAdControl"  )				// �Ѱ� ���� ��û����
			,new Proc("600500", "AdHitStatusControl"  )			    // ���� ��û��Ȳ
			,new Proc("600600", "StatisticsTotalControl"  )		    // ��ü���
			,new Proc("600700", "StatisticsDailyControl"  )		    // �Ϻ����
			,new Proc("600800", "StatisticsChannelControl"  )		// ä�κ����
			,new Proc("600900", "StatisticsRegionControl"  )		// ���������
			,new Proc("601000", "StatisticsTimeControl"  )			// �ð��뺰���
			,new Proc("601100", "AreaDayControl"  )			        // �ð��뺰���
			,new Proc("601200", "TimeDayControl"  )			        // �ð��뺰���
			,new Proc("601300", "GenreDayControl"  )			    // �ð��뺰���
			,new Proc("601400", "ChannelDayControl"  )			    // �ð��뺰���
			,new Proc("601500", "AreaTimeControl"  )			    // �ð��뺰���
			,new Proc("601600", "DateTimeControl"  )			    // �ð��뺰���
			,new Proc("601700", "GenreTimeControl"  )			    // �ð��뺰���
			,new Proc("601800", "ChannelTimeControl"  )			    // �ð��뺰���
			,new Proc("601900", "DateGenreControl"  )			    // �ð��뺰���
			,new Proc("602000", "DateChannelControl"  )			    // �ð��뺰���
			//,//new Proc("602100", "menuRegionDate"  )			    // �ð��뺰���
			,new Proc("602100", "RegionGenreControl"  )			    // �ð��뺰���
			,new Proc("602200", "HouseHoldAccountControl"  )		// �Ͽ콺Ȧ������
			            
            //�����û������ --20140811 �߰� - Youngil.Yi
			,new Proc("610100", "AdRatingForecastControl"	  )		// �����û������           

			// CUG����
			,new Proc("700100", "CugControl"  )					    // CUG ����
			,new Proc("700200", "CugHomeAdControl"     )			// CUG Ȩ������
			,new Proc("700300", "CugChoiceAdControl"     )			// CUG ����������
			,new Proc("700400", "CugChooseAdControl"     )			// CUG �޴�/ä�α�����
			,new Proc("700500", "CugAdStatusControl"     )			// CUG ����Ȳ


			// VAS ����

			,new Proc("800100", "VasChannelControl"  )				// VAS ä�ΰ���
			,new Proc("800200", "VasPackageControl"  )				// VAS ��ǰ����
			,new Proc("800300", "VasSlotControl"  )				    // VAS ���԰���
			,new Proc("800400", "VasCampaignControl"  )			    // VAS ķ���ΰ���
			,new Proc("800500", "VasItemControl"  )				    // VAS ����������
			,new Proc("800600", "VasScheduleItemControl"  )		    // VAS ����������
			,new Proc("800700", "VasScheduleSlotControl"  )		    // VAS ����������
			,new Proc("800800", "VasTargetingControl"  )			// VAS Ÿ���ð���
			,new Proc("800900", "VasAdHitStatusControl"  )			// VAS ���� ������Ȳ

			//
			// TODO : ���⿡ �޴�ó���Լ�(ȭ�� ��Ʈ��Ŭ����)�� �߰��մϴ�.
			//

            ,new Proc("900100", "_EXIT"	      )				        // ����
			,new Proc("900200", "CodeControl"	      )				// �ڵ����
            ,new Proc("900300", "MenuPowerControl"	      )			// �ڵ����
			,new Proc("900400", "SystemMenuControl"	      )		    // �޴���������
			,new Proc("900500", "SystemConfigControl"	      )		// ȯ�漳��
        };

        #endregion

        #region ��������� ��Ʈ�� 
		
		private Thread thProgress = null;
		private bool   isLoading  = false;        

        /// <summary>
        /// ����� ���� ��Ʈ�� �������̽�  2011.10.31 �߰� RH.Jung
        /// </summary>
        private IUserControl iUserCtrl = null;

      
        #endregion

        #region  �����̳� ��ü���
        public static MainForm CMain;
        private Janus.Windows.UI.Dock.UIPanel uiPanelMainMenu;
        private Janus.Windows.UI.Dock.UIPanelInnerContainer uiPanelMainMenuContainer;
        private Janus.Windows.UI.Dock.UIPanel uiPanelMain;
        private Janus.Windows.UI.Dock.UIPanelInnerContainer uiPanelMainContainer;
        private Janus.Windows.UI.Dock.UIPanelManager uiPnlMgr;
        private Janus.Windows.UI.CommandBars.UICommand cmdUsersControl1;
        private Janus.Windows.UI.StatusBar.UIStatusBar stbMainStatus;
        private Janus.Windows.UI.CommandBars.UIRebar BottomRebar1;
        private Janus.Windows.UI.CommandBars.UICommandManager uiCommandManager;
        private Janus.Windows.UI.CommandBars.UIRebar LeftRebar1;
        private Janus.Windows.UI.CommandBars.UIRebar RightRebar1;
        private Janus.Windows.UI.CommandBars.UIRebar TopRebar1;
        private Janus.Windows.UI.CommandBars.UICommandBar uiCommandBar_Menu;
        private ImageList imageList;
        private System.ComponentModel.IContainer components;
        #endregion

		#region [0] �ý��۱⵿ <==���Ⱑ �������Դϴ�
		[STAThread]
		static void Main()
		{
			try
			{
				// ���μ������� �˻��Ͽ� �̹� �������̸� �������� �ʴ´�.
				Process[] main = Process.GetProcessesByName(Application.ProductName);
				if (main.Length > 1) return;

				int rc = FrameSystem.Start();

				// 0���� ũ�� �⵿ ����
				if (rc > 0)
				{
					MessageForm mform = new MessageForm();
					mform.SetTitle = "�ý��� ����";
					mform.SetMessage = new string[] { "", FrameMessage.GetMessage(rc), "" };
					mform.showMessage();
					mform.ShowDialog();
					return;
				}

				// �α��� ó��
				LoginForm login = new LoginForm();

				login.ShowDialog();

				if (login.IsLogin)
				{
					Application.Run(new MainForm());
				}
				else
				{
					return;
				}
			}
			catch (Exception ex)
			{
				Trace.WriteLine(ex.Message);
			}
		}
		#endregion

        #region [1] ���� & �Ҹ���
        public MainForm()
        {			
            InitializeComponent();
            InitializeMenu();
            InitializeLogin();
            InitializeHome();
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

        #region [1.1] Windows Form �����̳ʿ��� ������ �ڵ�
        /// <summary>
        /// �����̳� ������ �ʿ��� �޼����Դϴ�.
        /// �� �޼����� ������ �ڵ� ������� �������� ���ʽÿ�.
        /// </summary>
        private void InitializeComponent()
        {
			this.components = new System.ComponentModel.Container();
			Janus.Windows.UI.StatusBar.UIStatusBarPanel uiStatusBarPanel1 = new Janus.Windows.UI.StatusBar.UIStatusBarPanel();
			Janus.Windows.UI.StatusBar.UIStatusBarPanel uiStatusBarPanel2 = new Janus.Windows.UI.StatusBar.UIStatusBarPanel();
			Janus.Windows.UI.StatusBar.UIStatusBarPanel uiStatusBarPanel3 = new Janus.Windows.UI.StatusBar.UIStatusBarPanel();
			Janus.Windows.UI.StatusBar.UIStatusBarPanel uiStatusBarPanel4 = new Janus.Windows.UI.StatusBar.UIStatusBarPanel();
			Janus.Windows.UI.StatusBar.UIStatusBarPanel uiStatusBarPanel5 = new Janus.Windows.UI.StatusBar.UIStatusBarPanel();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
			this.uiPnlMgr = new Janus.Windows.UI.Dock.UIPanelManager(this.components);
			this.uiPanelMain = new Janus.Windows.UI.Dock.UIPanel();
			this.uiPanelMainContainer = new Janus.Windows.UI.Dock.UIPanelInnerContainer();
			this.uiPanelMainMenu = new Janus.Windows.UI.Dock.UIPanel();
			this.uiPanelMainMenuContainer = new Janus.Windows.UI.Dock.UIPanelInnerContainer();
			this.stbMainStatus = new Janus.Windows.UI.StatusBar.UIStatusBar();
			this.cmdUsersControl1 = new Janus.Windows.UI.CommandBars.UICommand("cmdScheduleMgr");
			this.uiCommandManager = new Janus.Windows.UI.CommandBars.UICommandManager(this.components);
			this.BottomRebar1 = new Janus.Windows.UI.CommandBars.UIRebar();
			this.uiCommandBar_Menu = new Janus.Windows.UI.CommandBars.UICommandBar();
			this.LeftRebar1 = new Janus.Windows.UI.CommandBars.UIRebar();
			this.RightRebar1 = new Janus.Windows.UI.CommandBars.UIRebar();
			this.TopRebar1 = new Janus.Windows.UI.CommandBars.UIRebar();
			this.imageList = new System.Windows.Forms.ImageList(this.components);
			((System.ComponentModel.ISupportInitialize)(this.uiPnlMgr)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.uiPanelMain)).BeginInit();
			this.uiPanelMain.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.uiPanelMainMenu)).BeginInit();
			this.uiPanelMainMenu.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.uiCommandManager)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.BottomRebar1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.uiCommandBar_Menu)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.LeftRebar1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.RightRebar1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.TopRebar1)).BeginInit();
			this.TopRebar1.SuspendLayout();
			this.SuspendLayout();
			// 
			// uiPnlMgr
			// 
			this.uiPnlMgr.BackColorGradientAutoHideStrip = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(218)))), ((int)(((byte)(250)))));
			this.uiPnlMgr.ContainerControl = this;
			this.uiPnlMgr.VisualStyle = Janus.Windows.UI.Dock.PanelVisualStyle.Office2007;
			this.uiPanelMain.Id = new System.Guid("112b119a-5df3-4ca3-9348-08eb77d45100");
			this.uiPnlMgr.Panels.Add(this.uiPanelMain);
			// 
			// Design Time Panel Info:
			// 
			this.uiPnlMgr.BeginPanelInfo();
			this.uiPnlMgr.AddDockPanelInfo(new System.Guid("112b119a-5df3-4ca3-9348-08eb77d45100"), Janus.Windows.UI.Dock.PanelDockStyle.Fill, new System.Drawing.Size(1010, 678), true);
			this.uiPnlMgr.AddFloatingPanelInfo(new System.Guid("3ba82f24-8733-445c-a7b7-0920cc193ecf"), Janus.Windows.UI.Dock.PanelGroupStyle.HorizontalTiles, true, new System.Drawing.Point(128, 195), new System.Drawing.Size(200, 200), false);
			this.uiPnlMgr.AddFloatingPanelInfo(new System.Guid("99517015-6a66-4050-9f91-db016bcb12db"), new System.Guid("3ba82f24-8733-445c-a7b7-0920cc193ecf"), 221, false);
			this.uiPnlMgr.AddFloatingPanelInfo(new System.Guid("e23dc05a-2827-4eaa-b8f9-ba1db5a2a815"), new System.Guid("3ba82f24-8733-445c-a7b7-0920cc193ecf"), 221, false);
			this.uiPnlMgr.AddFloatingPanelInfo(new System.Guid("112b119a-5df3-4ca3-9348-08eb77d45100"), new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
			this.uiPnlMgr.EndPanelInfo();
			// 
			// uiPanelMain
			// 
			this.uiPanelMain.BorderPanel = Janus.Windows.UI.InheritableBoolean.False;
			this.uiPanelMain.CaptionDoubleClickAction = Janus.Windows.UI.Dock.CaptionDoubleClickAction.None;
			this.uiPanelMain.CloseButtonVisible = Janus.Windows.UI.InheritableBoolean.False;
			this.uiPanelMain.InnerContainer = this.uiPanelMainContainer;
			this.uiPanelMain.Location = new System.Drawing.Point(3, 29);
			this.uiPanelMain.Name = "uiPanelMain";
			this.uiPanelMain.Size = new System.Drawing.Size(1010, 678);
			this.uiPanelMain.TabIndex = 4;
			this.uiPanelMain.Text = "uiPanelMain (�� ĸ���� ����� ����������)";
			// 
			// uiPanelMainContainer
			// 
			this.uiPanelMainContainer.Location = new System.Drawing.Point(0, 23);
			this.uiPanelMainContainer.Name = "uiPanelMainContainer";
			this.uiPanelMainContainer.Size = new System.Drawing.Size(1010, 655);
			this.uiPanelMainContainer.TabIndex = 0;
			// 
			// uiPanelMainMenu
			// 
			this.uiPanelMainMenu.ActiveCaptionMode = Janus.Windows.UI.Dock.ActiveCaptionMode.Never;
			this.uiPanelMainMenu.Font = new System.Drawing.Font("���� ���", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
			this.uiPanelMainMenu.InnerContainer = this.uiPanelMainMenuContainer;
			this.uiPanelMainMenu.Location = new System.Drawing.Point(0, 0);
			this.uiPanelMainMenu.Name = "uiPanelMainMenu";
			this.uiPanelMainMenu.Size = new System.Drawing.Size(155, 677);
			this.uiPanelMainMenu.TabIndex = 4;
			this.uiPanelMainMenu.TabStop = false;
			this.uiPanelMainMenu.Text = "�޴�";
			// 
			// uiPanelMainMenuContainer
			// 
			this.uiPanelMainMenuContainer.Location = new System.Drawing.Point(0, 0);
			this.uiPanelMainMenuContainer.Name = "uiPanelMainMenuContainer";
			this.uiPanelMainMenuContainer.Size = new System.Drawing.Size(155, 677);
			this.uiPanelMainMenuContainer.TabIndex = 0;
			// 
			// stbMainStatus
			// 
			this.stbMainStatus.Location = new System.Drawing.Point(0, 710);
			this.stbMainStatus.Name = "stbMainStatus";
			this.stbMainStatus.Office2007ColorScheme = Janus.Windows.UI.Office2007ColorScheme.Blue;
			uiStatusBarPanel1.AutoSize = System.Windows.Forms.StatusBarPanelAutoSize.Spring;
			uiStatusBarPanel1.BorderColor = System.Drawing.Color.Empty;
			uiStatusBarPanel1.Key = "Message";
			uiStatusBarPanel1.ProgressBarValue = 0;
			uiStatusBarPanel1.Text = "�غ�";
			uiStatusBarPanel1.Width = 587;
			uiStatusBarPanel2.Alignment = System.Windows.Forms.HorizontalAlignment.Center;
			uiStatusBarPanel2.BorderColor = System.Drawing.Color.Empty;
			uiStatusBarPanel2.Key = "id";
			uiStatusBarPanel2.ProgressBarValue = 0;
			uiStatusBarPanel2.Text = "ID";
			uiStatusBarPanel3.Alignment = System.Windows.Forms.HorizontalAlignment.Center;
			uiStatusBarPanel3.BorderColor = System.Drawing.Color.Empty;
			uiStatusBarPanel3.Key = "name";
			uiStatusBarPanel3.ProgressBarValue = 0;
			uiStatusBarPanel3.Text = "�̸�";
			uiStatusBarPanel4.Alignment = System.Windows.Forms.HorizontalAlignment.Center;
			uiStatusBarPanel4.BorderColor = System.Drawing.Color.Empty;
			uiStatusBarPanel4.Key = "level";
			uiStatusBarPanel4.ProgressBarValue = 0;
			uiStatusBarPanel4.Text = "����";
			uiStatusBarPanel5.Alignment = System.Windows.Forms.HorizontalAlignment.Center;
			uiStatusBarPanel5.BorderColor = System.Drawing.Color.Empty;
			uiStatusBarPanel5.Key = "class";
			uiStatusBarPanel5.ProgressBarValue = 0;
			uiStatusBarPanel5.Text = "����";
			this.stbMainStatus.Panels.AddRange(new Janus.Windows.UI.StatusBar.UIStatusBarPanel[] {
            uiStatusBarPanel1,
            uiStatusBarPanel2,
            uiStatusBarPanel3,
            uiStatusBarPanel4,
            uiStatusBarPanel5});
			this.stbMainStatus.Size = new System.Drawing.Size(1016, 22);
			this.stbMainStatus.TabIndex = 14;
			this.stbMainStatus.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
			// 
			// cmdUsersControl1
			// 
			this.cmdUsersControl1.Key = "cmdScheduleMgr";
			this.cmdUsersControl1.Name = "cmdUsersControl1";
			// 
			// uiCommandManager
			// 
			this.uiCommandManager.BottomRebar = this.BottomRebar1;
			this.uiCommandManager.CommandBars.AddRange(new Janus.Windows.UI.CommandBars.UICommandBar[] {
            this.uiCommandBar_Menu});
			this.uiCommandManager.ContainerControl = this;
			this.uiCommandManager.Id = new System.Guid("2e98f62e-177e-43d4-8f7f-734647462753");
			this.uiCommandManager.LeftRebar = this.LeftRebar1;
			this.uiCommandManager.RightRebar = this.RightRebar1;
			this.uiCommandManager.TopRebar = this.TopRebar1;
			// 
			// BottomRebar1
			// 
			this.BottomRebar1.CommandManager = this.uiCommandManager;
			this.BottomRebar1.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.BottomRebar1.Location = new System.Drawing.Point(0, 710);
			this.BottomRebar1.Name = "BottomRebar1";
			this.BottomRebar1.Size = new System.Drawing.Size(1016, 0);
			// 
			// uiCommandBar_Menu
			// 
			this.uiCommandBar_Menu.Animation = Janus.Windows.UI.DropDownAnimation.Fade;
			this.uiCommandBar_Menu.CommandBarType = Janus.Windows.UI.CommandBars.CommandBarType.Menu;
			this.uiCommandBar_Menu.CommandManager = this.uiCommandManager;
			this.uiCommandBar_Menu.CommandsStyle = Janus.Windows.UI.CommandBars.CommandStyle.TextImage;
			this.uiCommandBar_Menu.Key = "CommandBar1";
			this.uiCommandBar_Menu.Location = new System.Drawing.Point(0, 0);
			this.uiCommandBar_Menu.Name = "uiCommandBar_Menu";
			this.uiCommandBar_Menu.RowIndex = 0;
			this.uiCommandBar_Menu.Size = new System.Drawing.Size(1016, 26);
			this.uiCommandBar_Menu.Text = "CommandBar_Menu";
			this.uiCommandBar_Menu.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
			this.uiCommandBar_Menu.CommandClick += new Janus.Windows.UI.CommandBars.CommandEventHandler(this.uiCommandBar_Menu_CommandClick);
			// 
			// LeftRebar1
			// 
			this.LeftRebar1.CommandManager = this.uiCommandManager;
			this.LeftRebar1.Dock = System.Windows.Forms.DockStyle.Left;
			this.LeftRebar1.Location = new System.Drawing.Point(0, 26);
			this.LeftRebar1.Name = "LeftRebar1";
			this.LeftRebar1.Size = new System.Drawing.Size(0, 684);
			// 
			// RightRebar1
			// 
			this.RightRebar1.CommandManager = this.uiCommandManager;
			this.RightRebar1.Dock = System.Windows.Forms.DockStyle.Right;
			this.RightRebar1.Location = new System.Drawing.Point(1016, 26);
			this.RightRebar1.Name = "RightRebar1";
			this.RightRebar1.Size = new System.Drawing.Size(0, 684);
			// 
			// TopRebar1
			// 
			this.TopRebar1.CommandBars.AddRange(new Janus.Windows.UI.CommandBars.UICommandBar[] {
            this.uiCommandBar_Menu});
			this.TopRebar1.CommandManager = this.uiCommandManager;
			this.TopRebar1.Controls.Add(this.uiCommandBar_Menu);
			this.TopRebar1.Dock = System.Windows.Forms.DockStyle.Top;
			this.TopRebar1.Location = new System.Drawing.Point(0, 0);
			this.TopRebar1.Name = "TopRebar1";
			this.TopRebar1.Size = new System.Drawing.Size(1016, 26);
			// 
			// imageList
			// 
			this.imageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList.ImageStream")));
			this.imageList.TransparentColor = System.Drawing.Color.Transparent;
			this.imageList.Images.SetKeyName(0, "100000");
			this.imageList.Images.SetKeyName(1, "100100");
			this.imageList.Images.SetKeyName(2, "100200");
			this.imageList.Images.SetKeyName(3, "100300");
			this.imageList.Images.SetKeyName(4, "100400");
			this.imageList.Images.SetKeyName(5, "100500");
			this.imageList.Images.SetKeyName(6, "100600");
			this.imageList.Images.SetKeyName(7, "100700");
			this.imageList.Images.SetKeyName(8, "100800");
			this.imageList.Images.SetKeyName(9, "100900");
			this.imageList.Images.SetKeyName(10, "101200");
			this.imageList.Images.SetKeyName(11, "102200");
			this.imageList.Images.SetKeyName(12, "200000");
			this.imageList.Images.SetKeyName(13, "200500");
			this.imageList.Images.SetKeyName(14, "200700");
			this.imageList.Images.SetKeyName(15, "201100");
			this.imageList.Images.SetKeyName(16, "200600");
			this.imageList.Images.SetKeyName(17, "200800");
			this.imageList.Images.SetKeyName(18, "201000");
			this.imageList.Images.SetKeyName(19, "200100");
			this.imageList.Images.SetKeyName(20, "200200");
			this.imageList.Images.SetKeyName(21, "300000");
			this.imageList.Images.SetKeyName(22, "400000");
			this.imageList.Images.SetKeyName(23, "500000");
			this.imageList.Images.SetKeyName(24, "510000");
			this.imageList.Images.SetKeyName(25, "600000");
			this.imageList.Images.SetKeyName(26, "700000");
			this.imageList.Images.SetKeyName(27, "800000");
			this.imageList.Images.SetKeyName(28, "900000");
			this.imageList.Images.SetKeyName(29, "101000");
			this.imageList.Images.SetKeyName(30, "300300");
			this.imageList.Images.SetKeyName(31, "300350");
			this.imageList.Images.SetKeyName(32, "300400");
			this.imageList.Images.SetKeyName(33, "300450");
			this.imageList.Images.SetKeyName(34, "300500");
			this.imageList.Images.SetKeyName(35, "300600");
			this.imageList.Images.SetKeyName(36, "300700");
			this.imageList.Images.SetKeyName(37, "300800");
			this.imageList.Images.SetKeyName(38, "301200");
			this.imageList.Images.SetKeyName(39, "400100");
			this.imageList.Images.SetKeyName(40, "400150");
			this.imageList.Images.SetKeyName(41, "400200");
			this.imageList.Images.SetKeyName(42, "400300");
			this.imageList.Images.SetKeyName(43, "400400");
			this.imageList.Images.SetKeyName(44, "400500");
			this.imageList.Images.SetKeyName(45, "400600");
			this.imageList.Images.SetKeyName(46, "610000");
			this.imageList.Images.SetKeyName(47, "900100");
			this.imageList.Images.SetKeyName(48, "900500");
			// 
			// MainForm
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(6, 14);
			this.BackColor = System.Drawing.Color.White;
			this.ClientSize = new System.Drawing.Size(1016, 732);
			this.Controls.Add(this.uiPanelMain);
			this.Controls.Add(this.LeftRebar1);
			this.Controls.Add(this.RightRebar1);
			this.Controls.Add(this.BottomRebar1);
			this.Controls.Add(this.stbMainStatus);
			this.Controls.Add(this.TopRebar1);
			this.DoubleBuffered = true;
			this.Font = global::AdManagerClient.Properties.Settings.Default.Font1;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MinimumSize = new System.Drawing.Size(1024, 736);
			this.Name = "MainForm";
			this.Text = "AdTargetsPlus for BTV";
			this.Closing += new System.ComponentModel.CancelEventHandler(this.MainForm_Closing);
			this.Load += new System.EventHandler(this.MainForm_Load);
			((System.ComponentModel.ISupportInitialize)(this.uiPnlMgr)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.uiPanelMain)).EndInit();
			this.uiPanelMain.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.uiPanelMainMenu)).EndInit();
			this.uiPanelMainMenu.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.uiCommandManager)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.BottomRebar1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.uiCommandBar_Menu)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.LeftRebar1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.RightRebar1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.TopRebar1)).EndInit();
			this.TopRebar1.ResumeLayout(false);
			this.ResumeLayout(false);

		}
        #endregion

        #region [1.2] �޴� �ʱ�ȭ
		/// <summary>
		/// �޴� �ʱ�ȭ �� ����
		/// </summary>
        private void InitializeMenu()
        {
            MenuModel menuModel = new MenuModel();
            new MenuManager(FrameSystem.oSysModel,FrameSystem.oComModel).GetUserMenuList(menuModel);

            if(menuModel.ResultCD.Equals("0000"))
            {
                FrameSystem.oMenu = new MenuPower();
                string LastGroupPower = string.Empty;
                string LastGroupName = string.Empty;
                string LastGroupCode  = "";

                // ���� �޴� Command 
                Janus.Windows.UI.CommandBars.UICommand uiCommandGrp = new Janus.Windows.UI.CommandBars.UICommand();

                // �޴� Command �߰�
                for (int i = 0; i < menuModel.ResultCnt; i++)
                {
                    string MenuCode  = Utility.GetDatasetString(menuModel.MenuDataSet, i, "MenuCode");
                    string MenuName  = Utility.GetDatasetString(menuModel.MenuDataSet, i, "MenuName");
                    string MenuLevel = Utility.GetDatasetString(menuModel.MenuDataSet, i, "MenuLevel");
                    string MenuPower = Utility.GetDatasetString(menuModel.MenuDataSet, i, "MenuPower");

                    // �޴������� 1�̸� �޴��׷�ϴ�.
                    if(MenuLevel.Equals("1"))
                    {
                        // ���� �޴��׷��� �ִٸ� �޴��� �׷��� ��Ʈ�Ѵ�.
                        if(!LastGroupCode.Equals(""))
                        {
                            // �޴��׷쿡 �б������ �ִ°�...
                            if(LastGroupPower.IndexOf("R") >= 0)
                            {
                                //�޴��ٿ� �޴��׷� �߰�
                                this.uiCommandBar_Menu.Commands.Add(uiCommandGrp);

                            }
							
                            //�� �޴� �׷��� ������ �޴��� ǥ������ �ʴ´�.
                        }

                        // �޴��׷� ����
                        uiCommandGrp = new Janus.Windows.UI.CommandBars.UICommand();
                        uiCommandGrp.Key = MenuCode;
                        uiCommandGrp.Text = MenuName;
                        try
                        {
                            uiCommandGrp.Image = this.imageList.Images[MenuCode];
                        }
                        catch (Exception )
                        {
                            //�̹����� ������ ������ �ȳ���...
                        }

                        LastGroupCode       = MenuCode;		// ���� �޴��׷����� ��Ʈ 
                        LastGroupPower      = MenuPower;
                    }
                    else
                    {
                        // �׷��� �ƴϸ� �޴��̹Ƿ� �׷쿡 �޴��� �߰��Ѵ�.

                        // �޴��� �б������ �ִ°�...
                        if(MenuPower.IndexOf("R") >= 0)
                        {
                            //�޴� ����
                            Janus.Windows.UI.CommandBars.UICommand uiCommand = new Janus.Windows.UI.CommandBars.UICommand();
                            uiCommand.Key = MenuCode;
                            uiCommand.Text = MenuName;
                            try
                            {
                                uiCommand.Image = this.imageList.Images[MenuCode];
                            }
                            catch (Exception)
                            {
                                //�̹����� ������ ������ �ȳ���...
                            }

                            //�޴��׷쿡 �޴��߰�
                            uiCommandGrp.Commands.Add(uiCommand);

                            // �޴� �ؽ����̺� �޴��ڵ带 �ִ´�.
                            FrameSystem.oMenu.Add(MenuCode, MenuPower);
                        }
                    }
                }

                // ������ �޴��׷� ��Ʈ
                if(!LastGroupCode.Equals(""))
                {
                    this.uiCommandBar_Menu.Commands.Add(uiCommandGrp);
                }

            }
            else
            {
                MessageForm mform = new MessageForm();					
			
                mform.SetTitle = "�޴� �ʱ�ȭ ����";
                mform.SetMessage = new string[] { "", menuModel.ResultDesc,""}  ;
                mform.showMessage();			
                mform.ShowDialog();
                this.Close();
            }

        }
        #endregion

        #region �α������� �ʱ�ȭ
        private void InitializeLogin()
        {	
            String ClientType = "";

            CommonModel   commonModel   = FrameSystem.oComModel;

            stbMainStatus.Panels["id"].Text = "ID:" + commonModel.UserID;
            stbMainStatus.Panels["name"].Text = "�̸�:" + commonModel.UserName;
            stbMainStatus.Panels["level"].Text = "����:" + commonModel.LevelName;
            stbMainStatus.Panels["class"].Text = "����:" + commonModel.ClassName;

            switch(FrameSystem.m_ClientType)
            {
                case FrameSystem._DEV:
                    ClientType = "������";
					stbMainStatus.Office2007ColorScheme = Janus.Windows.UI.Office2007ColorScheme.Black;
                    break;
                case FrameSystem._REAL:
                    ClientType = "";
					stbMainStatus.Office2007ColorScheme = Janus.Windows.UI.Office2007ColorScheme.Blue;
                    break;
                case FrameSystem._TEST:
                    ClientType = "�׽�Ʈ";
					stbMainStatus.Office2007ColorScheme = Janus.Windows.UI.Office2007ColorScheme.Silver;
                    break;
                default:
                    ClientType = "Unknown Client Type";
                    break;
            }

            this.Text = this.Text + " " + FrameSystem.m_SystemVersion + " (" + ClientType + ")" ;
        }

        #endregion

        #region Home �ʱ�ȭ
        private void InitializeHome()
        {
            try 
            {
                iUserCtrl			= new HomeControl();
                iUserCtrl.SetParent(uiPanelMainContainer);
                iUserCtrl.SetDockStyle(DockStyle.Fill);                
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        #endregion
		
        #region �޴���ư Ŭ�� ó�� 

        ////////////////////////////////////////////////////////////////////////
        // 2011.10.31 �޴�ó����� ���� �޴� CommandBar�� �̿�
        /// <summary>
        /// �޴�ó��
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void uiCommandBar_Menu_CommandClick(object sender, Janus.Windows.UI.CommandBars.CommandEventArgs e)
        {
            // ���θ޴��� ��ư ������ ���� 
            try
            {

                if (isLoading == false)
                {
                    isLoading = true;

                    string nowMenuCode = e.Command.Key;

                    // �޴��ڵ尡 �޴��� �����ϴ��� �˻�
                    if (!FrameSystem.oMenu.IsMenu(nowMenuCode))
                    {
                        MessageForm mform = new MessageForm();

                        mform.SetTitle = "�ý��� ����";
                        mform.SetMessage = new string[] { "", "�ش�޴��� �������� �ʽ��ϴ�", "" };
                        mform.showMessage();
                        mform.ShowDialog();
                        return;
                    }   

                    // �̹� ������� ��Ʈ���� �ִٸ� �����Ѵ�.
                    releaseCtlActive();

                    Type form = this.GetType();
                    // �޴��Լ� ���� ��Ͽ��� ã�Ƽ� �ش� �޴��Լ��� �����Ѵ�.
                    for (int i = 0; i < procList.Length; i++)
                    {
                        if (procList[i].MenuCode.Equals(nowMenuCode))
                        {
                            // �޴��� ������ "_EXIT"
                            if (procList[i].FuncName.Equals("_EXIT"))
                            {
                                InitializeHome();  // Ȩȭ���� ����. ��? ��������ϸ� Ȩȭ���� ���̰�
                                this.Close();
                                break;
                            }

                            //////////////////////////////////////////////////////////////////////
                            // �ش� Ŭ���������� Ŭ������ ��������� ���
                            //////////////////////////////////////////////////////////////////////
                            Type type = Type.GetType(form.Namespace + "." + procList[i].FuncName);

                            //////////////////////////////////////////////////////////////////////
                            //
                            // Ŭ������ �ν��Ͻ��� �����Ѵ�. ��� �޴�ȭ���� IUserControl�� ��ӹ޾� �ش� �������̽��� �����ؾ��Ѵ�.
                            //
                            //////////////////////////////////////////////////////////////////////
                            iUserCtrl = (IUserControl)Activator.CreateInstance(type);

                            /////////////////////////////////////////////////////////////////////
                            // �ν��Ͻ��� �ʱ�ȭ �Ѵ�.
                            ////////////////////////////////////////////////////////////////////
                            if (iUserCtrl != null)
                            {
                                iUserCtrl.ProgressEvent += new ProgressEventHandler(OnProgressEvent);
                                iUserCtrl.StatusEvent += new StatusEventHandler(OnStatusEvent);
                                iUserCtrl.MenuCode = nowMenuCode;
                                iUserCtrl.SetParent(uiPanelMainContainer);
                                iUserCtrl.SetDockStyle(DockStyle.Fill);                                
                            }
                        }
                    }

                    // ���� Ȱ��ȭ�� ȭ�� ��Ʈ���� ���ٸ� ���� �������� ���� �޴��̴�.                   
                    if(iUserCtrl == null)
                    {
                        InitializeHome();  // Ȩȭ���� ����. ��? �޴��� ������ Ȩȭ���� ���̰�

                        MessageForm mform = new MessageForm();

                        mform.SetTitle = "�ý��� ����";
                        mform.SetMessage = new string[] { "", "�ش�޴��� �غ����Դϴ�.", "�̿뿡 ������ ����� �˼��մϴ�." };
                        mform.showMessage();
                        mform.ShowDialog();
                    }

                    isLoading = false;
                }
            }
            catch (Exception ex)
            {
                isLoading = false;

                InitializeHome();

                FrameSystem.oLog.Message("�޴������ �����߻�");
                FrameSystem.oLog.Message(ex.Message);

                MessageBox.Show("�޴������ ������ �߻��Ͽ����ϴ�. �ٽ� �����Ͽ� �ֽʽÿ�.\n\n��� ������ �߻��ϸ� �ý��۰����ڿ��� �����Ͻñ� �ٶ��ϴ�.", "�ý��ۿ���",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                Trace.WriteLine(ex.Message);
            }

        }

        /// <summary>
        /// Ȱ����Ʈ�� ����
        /// </summary>
        private void releaseCtlActive()
        {            
            if(iUserCtrl != null)
            {                
                ((UserControl)iUserCtrl).Dispose();
                iUserCtrl = null;

                // ������ �÷��͸� ������ �����Ѵ�.
                GC.Collect();

                Application.DoEvents();
            }
        }    

        #endregion

        #region ��Ÿó���Լ�

        private void MainForm_Load(object sender, System.EventArgs e)
        {
            uiPanelMain.CaptionVisible = Janus.Windows.UI.InheritableBoolean.False;
			
            StatusBarMessage("�غ�");
        }

        private void MainForm_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            DialogResult result = MessageBox.Show("AdTargetsPlus Manager�� ���� �Ͻðڽ��ϱ�?","���α׷� ����",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question,
                MessageBoxDefaultButton.Button2);
            if (result == DialogResult.Yes)
                e.Cancel = false; //Application.Exit();
            else
                e.Cancel = true;
        }

        #endregion

        #region ���·α� ����Ʈ�信 ����Ѵ�.
		
        private void OnStatusEvent(object sender, StatusEventArgs e)
        {
            StatusBarMessage(e.Message);
        }

        /// <summary>
        /// ���¹ٿ� ����Ѵ�.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="Msg"></param>
        private void StatusBarMessage(string Msg)
        {
            stbMainStatus.Panels["Message"].Text = Msg;
        }

        #endregion

		#region ó���� ����â�� ó���Ѵ�.

		// Thread�� ����� ��� �� �Լ��� �̿��Ѵ�.
		private void progressForm()
		{
			// ó���� ����â�� ����.
			try
			{
                
				int x = this.Top;
				int y = this.Left;

				x = x + (this.Height / 2) - 50;
				y = y + (this.Width / 2 ) + 72 - 200;

				ProgressForm progFrm = new ProgressForm();
				progFrm.ShowInTaskbar = false;
				progFrm.Top = x;
				progFrm.Left = y;
				//progFrm.Size	= new System.Drawing.Size(64, 64);
				progFrm.Show();

				while(isLoading)
				{
					//Thread.Sleep(5);  // �־�� �� ����??-bae[E_01]
					Application.DoEvents();
				}

				progFrm.Close();
				progFrm.Dispose();
				progFrm = null;
			}
			catch(Exception ex)
			{
				Trace.WriteLine(ex.Message);
			}
		}
		

		private void OnProgressEvent(object sender, ProgressEventArgs e)
		{
			try
			{

				// ������ ó���� ����â�� �ݴ´�
				if (null != thProgress)
				{
						if( isLoading ) isLoading = false;
						Thread.Sleep(10);
				}

				// �����̸�
				if(e.Type == ProgressEventArgs.Start)
				{
					isLoading = true;

					// �۾� �޽��� Thread �� ó��
					thProgress = new Thread(new ThreadStart(this.progressForm));
					thProgress.IsBackground = true;
					thProgress.Name = "Loading Progress";
					thProgress.Start();
				}
			}
			catch(Exception ex)
			{
				Trace.WriteLine(ex.Message);
			}			
		}

		#endregion
	}
}