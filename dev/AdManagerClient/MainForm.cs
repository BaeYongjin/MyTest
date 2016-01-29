// ===============================================================================
//
// MainForm.cs
//
// 메인폼 
//
// ===============================================================================
// Release history
// 2007.08.15 RH.Jung Initialize
// 중략
// 2007.12.12 RH.Jung 편성관리/키워드편성 메뉴 추가
// 2011.10.31 RH.Jung 메뉴표시방식 변경 : ButtonBar -> CommandBar
//                    메뉴처리방식 변경 : 화면Control별 처리함수 -> IUserControl이용 직접 Class 생성
// ===============================================================================
// Copyright (C) 2007 G-Inno Systems Inc.
// All rights reserved.
// ==============================================================================
/*
 * -------------------------------------------------------
 * Class Name: MainForm
 * 주요기능  :  메뉴 컨트롤 컨테이너 역할.
 * 작성자    : 모름
 * 작성일    : 모름
 * 특이사항  : 메뉴 컨트롤 호출 하는 - 사용하지 않는 소스 삭제
 * -------------------------------------------------------
 * [수정사항]
 * -------------------------------------------------------
 * 수정코드  : [E_01]
 * 수정자    : bae
 * 수정일    : 2013.04.29
 * 수정내용  :        
 *            - ctlActive 멤버 변수 삭제
 *              why: iUserCtrl 인터페이스 멤버변수 사용하므로 중복제거
 *            - 기타 사용하지 않는 소스 삭제  
 * 수정함수  :
 *            - uiCommandBar_Menu_CommandClick(.)
 *              releaseCtlActive()
 *              progressForm()
 * --------------------------------------------------------
 * 수정코드  : [E_02]
 * 수정자    : bae
 * 수정일    : 2013.07.10
 * 수정내용  :        
 *             고객군등록이 장시간이 소요되므로 실행 후 다른 작업이
 *             가능하도록 하기위해서 전역변수와 비슷한 폼 변수를 만들어서 관리
 *             하도록 한다.
 * 수정함수  :
 *            Global_TgtCollecton 변수 추가
 *            Global_TgtRefresh() 함수 추가
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
    /// TmsMain에 대한 요약 설명입니다.
    /// </summary>
    public class MainForm : System.Windows.Forms.Form
    {
        #region 메뉴함수 구조체
        struct Proc
        {
            public string MenuCode;				// 메뉴코드
            public string FuncName;				// 처리함수명 (각화면 Control의 Class명)

            public Proc (string m, string f)
            {
                MenuCode = m;
                FuncName = f;
            }
        }
        #endregion

        #region 메뉴처리함수 리스트를 정의함 : 메뉴추가시 이곳에 정의

        /// <summary>
        /// 처리함수의 리스트이다
        /// 메뉴코드가 추가되면 이 리스트에 그 처리함수를 추가해 주어야 한다.
        /// 처리함수는 본 소스 하단의 처리함수 목록에 추가한다.
        /// 
        /// 2011.10.31 RH.Jung 메뉴처리방식 변경
        /// 처리함수명은 해당 화면의 Class명이다. 
        /// 
        /// </summary>
        Proc [] procList = 
        {

            // 매체관리
             new Proc("100100", "MediaInfoControl"   )				// 매체정보관리
            ,new Proc("100200", "MediaRapControl"    )				// 미디어렙관리
            ,new Proc("100300", "CategoryControl"    )				// 카테고리관리
            ,new Proc("100400", "GenreControl"       )				// 장르관리
            ,new Proc("100500", "ChannelControl"     )				// 채널관리
            ,new Proc("100600", "ContentsControl"    )				// 컨텐츠관리
            ,new Proc("100700", "ChannelSetControl"  )				// 사용자관리
            ,new Proc("100800", "UserInfoControl"    )				// 사용자관리
//			,new Proc("100900", "SlotOrganizationControl")          // 슬롯관리
            ,new Proc("100900", "SlotAdInfoControl"	)               // 유동광고슬룻관리	2014.09.15    
			,new Proc("101000", "GroupControl"      )			    // 그룹관리
            ,new Proc("102100", "MediaMenuSetCtrl"	)			    // 편성대상관리
            ,new Proc("101100", "GroupOrganizationControl")         // OAP 편성그룹관리	2013.03.
			,new Proc("101200", "MenuControl"       )               // 메뉴관리     
            ,new Proc("101300", "SchOrgGenreControl")               // 원장르기반OAP편성관리
			,new Proc("101400", "MenuMapCtrl"		)               // 메뉴매핑관리		2015.10
			
            // 광고계약관리
            ,new Proc("200100", "AgencyControl"      )				// 대행사관리
            ,new Proc("200200", "AdvertiserControl"  )				// 광고주관리
            ,new Proc("200300", "MediaAgencyControl" )				// 매체별 대행사관리
			,new Proc("200400", "ClientControl"      )				// 대행사별 광고주관리
            ,new Proc("200500", "ContractControl"    )				// 광고계약관리
            ,new Proc("200600", "AdFileControl"      )				// 광고파일관리
            ,new Proc("200700", "ContractItemControl")				// 광고내역관리
			,new Proc("200800", "AdFileWideControl"      )			// 광고파일배포관리
			,new Proc("200900", "ContractPackageControl" )			// 광고상품관리
			,new Proc("201000", "FilePublishControl" )				// 파일배포승인
			,new Proc("201100", "CampaignControl" )			    	// 캠페인

            // 광고편성관리
//            ,new Proc("300100", "menuGenreGroup"  )				// 메뉴광고그룹관리
//            ,new Proc("300200", "menuChannelGroup")				// 채널광고그룹관리
            ,new Proc("300300", "SchHomeAdControl"     )    		// 홈광고편성
            ,new Proc("300310", "SchHomeKidsControl"     )    		// 홈광고(키즈포털)편성	2014.11
            ,new Proc("300320", "SchTargetHomeAdControl"     )      // 홈광고편성(고객군)	2015.05
			,new Proc("300350", "SchHomeCmControl"   )				// 홈상업광고편성
            ,new Proc("300400", "SchChoiceAdControl"   )			// 광고별광고편성
            ,new Proc("300500", "ChooseAdScheduleControl"   )		// 메뉴/채널별 편성현황
			,new Proc("300600", "AdStatusControl"    )				// 광고편성현황
			,new Proc("300700", "SchPublishControl"  )				// 편성배포승인
			,new Proc("300800", "SchMenuAdControl"  )				// 메뉴광고편성
			,new Proc("300900", "SchAppendAdControl"  )				// 추가광고편성
			//,new Proc("301000", "SchExcelControl"  )				// 편성일괄등록
			,new Proc("301000", "SchRecommControl"  )				// 편성추천
			,new Proc("301100", "SchPopularChannelControl"  )		// 상위채널편성
			,new Proc("301200", "ChannelJumpControl"  )			    // 채널점핑관리
			,new Proc("301300", "SchKeywordControl"  )				// 키워드편성
			,new Proc("301400", "SchGroupControl"  )				// 그룹편성
            ,new Proc("302000", "SchMidAdControl"  )                // 중간광고편성 2009.07.10
			,new Proc("300450", "SchDesignateAdControl"  )          // 채널별 지정편성	2009.09.20
            ,new Proc("303000", "SchExclusiveZoneAdControl")        // 시간대별 지정편성 성	2011.12.22
            ,new Proc("301500", "SchHomeGroupControl")              // 홈OAP그룹편성 2013.06.

            // 타겟팅관리
            ,new Proc("400100", "TargetingControl"  )				// 사업광고 타겟팅관리
			,new Proc("400150", "TargetingOapControl"  )			// 매체광고 타겟팅관리
			,new Proc("400200", "TargetingHomeControl"  )			// 홈광고 타겟팅관리
			,new Proc("400300", "TargetCollectionControl"  )		// 타겟고객군관리  //CollectionHomeControl  /  TargetCollectionControl
            ,new Proc("400330", "CollectionHomeControl"     )    	// 타겟고객군관리(홈광고)2015.05
			,new Proc("400350", "CrmControl"			)		    // CRM연동관리
			,new Proc("400400", "GradeControl"				)		// 타겟고객군관리
			,new Proc("400500", "RatioControl"				)		// 타겟고객군관리
			,new Proc("400600", "UkeyTargetCollectionControl")		// UKey캠페인관리
            ,new Proc("400700", "TargetingCollectionControl")       // 고객군 타켓팅관리 2012.02.21 RH.Jung
            ,new Proc("400800", "TargetingAdDenyControl")           // 광고거부자 관리
		
			// 매체분석보고서
//			,new Proc("500100", "menuPgmDailyRating" )			    // 일별시청률
//			,new Proc("500100", "menuContentRating"  )			    // 컨텐츠별   시청률집계
//			,new Proc("500200", "menuGenreRating"    )			    // 장르별     시청률집계
//			,new Proc("500300", "menuCategoryRating" )			    // 카테고리별 시청률집계
			,new Proc("500400", "StatisticsPgCategoryControl" )	    // 카테고리통계
			,new Proc("500500", "StatisticsPgChannelControl" )	    // 채널통계
			,new Proc("500600", "StatisticsPgRegionControl"  )		// 지역별통계
			,new Proc("500700", "StatisticsPgTimeControl"  )		// 시간대별통계
			,new Proc("500800", "StatisticsPgWeekControl"  )		// 요일별통계
			,new Proc("500900", "StatisticsPgAgeControl"  )		    // 연령별통계

			//총괄보고서
			,new Proc("510100", "RptSummaryAdDailyControl"  )		// 광고일간레포트
			,new Proc("510200", "RptSummaryAdWeeklyControl"  )		// 광고주간레포트
//			,new Proc("510300", "menuSummaryAdMonthly"  )		    // 광고주간레포트
			,new Proc("510400", "DailyAdExecSummaryRptControl"  )	// 광고일간레포트
			,new Proc("510500", "RptAdCategorySummaryControl"  )	// 광고일간레포트
			,new Proc("510600", "OAPWeekHomeAdControl"  )			// 광고일간레포트
			,new Proc("510700", "OAPWeekChannelJumpControl"  )		// 광고일간레포트
			,new Proc("510800", "DateAdTypeSummaryRptControl"  )	// 광고일간레포트
			,new Proc("510900", "AdTypeMoniteringControl"  )		// 광고일간레포트
			,new Proc("511000", "InventoryPresentConditionControl")	// 인벤토리현황
            ,new Proc("512000", "HomeCmRptControl"  )	            // 홈상업광고^2010.11.08
			,new Proc("513000", "PreferenceTotalizeControl")        // 선호도 조사 팝업 집계( 2013-06-20 )
            ,new Proc("514000", "CombineRptCtrl")                   // 통합 레포트( 2013-06-27 )
            ,new Proc("515000", "BizManageTgtControl")              // 영업관리

			// 광고집행보고서
			,new Proc("600100", "ProgramAdHitControl"  )			// 프로그래별 광고시청집계
			,new Proc("600200", "DailyProgramHitControl"  )		    // 일별 프로그램 시청집계
			,new Proc("600300", "DailyAdHitControl"  )				// 일별 광고 시청집계
			,new Proc("600400", "SummaryAdControl"  )				// 총괄 광고 시청집계
			,new Proc("600500", "AdHitStatusControl"  )			    // 광고별 시청현황
			,new Proc("600600", "StatisticsTotalControl"  )		    // 전체통계
			,new Proc("600700", "StatisticsDailyControl"  )		    // 일별통계
			,new Proc("600800", "StatisticsChannelControl"  )		// 채널별통계
			,new Proc("600900", "StatisticsRegionControl"  )		// 지역별통계
			,new Proc("601000", "StatisticsTimeControl"  )			// 시간대별통계
			,new Proc("601100", "AreaDayControl"  )			        // 시간대별통계
			,new Proc("601200", "TimeDayControl"  )			        // 시간대별통계
			,new Proc("601300", "GenreDayControl"  )			    // 시간대별통계
			,new Proc("601400", "ChannelDayControl"  )			    // 시간대별통계
			,new Proc("601500", "AreaTimeControl"  )			    // 시간대별통계
			,new Proc("601600", "DateTimeControl"  )			    // 시간대별통계
			,new Proc("601700", "GenreTimeControl"  )			    // 시간대별통계
			,new Proc("601800", "ChannelTimeControl"  )			    // 시간대별통계
			,new Proc("601900", "DateGenreControl"  )			    // 시간대별통계
			,new Proc("602000", "DateChannelControl"  )			    // 시간대별통계
			//,//new Proc("602100", "menuRegionDate"  )			    // 시간대별통계
			,new Proc("602100", "RegionGenreControl"  )			    // 시간대별통계
			,new Proc("602200", "HouseHoldAccountControl"  )		// 하우스홀드집계
			            
            //광고시청률예측 --20140811 추가 - Youngil.Yi
			,new Proc("610100", "AdRatingForecastControl"	  )		// 광고시청률예측           

			// CUG관리
			,new Proc("700100", "CugControl"  )					    // CUG 관리
			,new Proc("700200", "CugHomeAdControl"     )			// CUG 홈광고편성
			,new Proc("700300", "CugChoiceAdControl"     )			// CUG 지정광고편성
			,new Proc("700400", "CugChooseAdControl"     )			// CUG 메뉴/채널광고편성
			,new Proc("700500", "CugAdStatusControl"     )			// CUG 편성현황


			// VAS 광고

			,new Proc("800100", "VasChannelControl"  )				// VAS 채널관리
			,new Proc("800200", "VasPackageControl"  )				// VAS 상품관리
			,new Proc("800300", "VasSlotControl"  )				    // VAS 슬롯관리
			,new Proc("800400", "VasCampaignControl"  )			    // VAS 캠페인관리
			,new Proc("800500", "VasItemControl"  )				    // VAS 광고소재관리
			,new Proc("800600", "VasScheduleItemControl"  )		    // VAS 지정광고편성
			,new Proc("800700", "VasScheduleSlotControl"  )		    // VAS 지정슬롯편성
			,new Proc("800800", "VasTargetingControl"  )			// VAS 타겟팅관리
			,new Proc("800900", "VasAdHitStatusControl"  )			// VAS 광고별 노출현황

			//
			// TODO : 여기에 메뉴처리함수(화면 컨트롤클래스)를 추가합니다.
			//

            ,new Proc("900100", "_EXIT"	      )				        // 종료
			,new Proc("900200", "CodeControl"	      )				// 코드관리
            ,new Proc("900300", "MenuPowerControl"	      )			// 코드관리
			,new Proc("900400", "SystemMenuControl"	      )		    // 메뉴생성관리
			,new Proc("900500", "SystemConfigControl"	      )		// 환경설정
        };

        #endregion

        #region 사용자정의 컨트롤 
		
		private Thread thProgress = null;
		private bool   isLoading  = false;        

        /// <summary>
        /// 사용자 정의 컨트롤 인터페이스  2011.10.31 추가 RH.Jung
        /// </summary>
        private IUserControl iUserCtrl = null;

      
        #endregion

        #region  디자이너 객체목록
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

		#region [0] 시스템기동 <==여기가 시작점입니다
		[STAThread]
		static void Main()
		{
			try
			{
				// 프로세스명을 검사하여 이미 실행중이면 실행하지 않는다.
				Process[] main = Process.GetProcessesByName(Application.ProductName);
				if (main.Length > 1) return;

				int rc = FrameSystem.Start();

				// 0보다 크면 기동 오류
				if (rc > 0)
				{
					MessageForm mform = new MessageForm();
					mform.SetTitle = "시스템 오류";
					mform.SetMessage = new string[] { "", FrameMessage.GetMessage(rc), "" };
					mform.showMessage();
					mform.ShowDialog();
					return;
				}

				// 로그인 처리
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

        #region [1] 생성 & 소멸자
        public MainForm()
        {			
            InitializeComponent();
            InitializeMenu();
            InitializeLogin();
            InitializeHome();
        }

        /// <summary>
        /// 사용 중인 모든 리소스를 정리합니다.
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

        #region [1.1] Windows Form 디자이너에서 생성한 코드
        /// <summary>
        /// 디자이너 지원에 필요한 메서드입니다.
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마십시오.
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
			this.uiPanelMain.Text = "uiPanelMain (이 캡션은 실행시 보이지않음)";
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
			this.uiPanelMainMenu.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
			this.uiPanelMainMenu.InnerContainer = this.uiPanelMainMenuContainer;
			this.uiPanelMainMenu.Location = new System.Drawing.Point(0, 0);
			this.uiPanelMainMenu.Name = "uiPanelMainMenu";
			this.uiPanelMainMenu.Size = new System.Drawing.Size(155, 677);
			this.uiPanelMainMenu.TabIndex = 4;
			this.uiPanelMainMenu.TabStop = false;
			this.uiPanelMainMenu.Text = "메뉴";
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
			uiStatusBarPanel1.Text = "준비";
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
			uiStatusBarPanel3.Text = "이름";
			uiStatusBarPanel4.Alignment = System.Windows.Forms.HorizontalAlignment.Center;
			uiStatusBarPanel4.BorderColor = System.Drawing.Color.Empty;
			uiStatusBarPanel4.Key = "level";
			uiStatusBarPanel4.ProgressBarValue = 0;
			uiStatusBarPanel4.Text = "레벨";
			uiStatusBarPanel5.Alignment = System.Windows.Forms.HorizontalAlignment.Center;
			uiStatusBarPanel5.BorderColor = System.Drawing.Color.Empty;
			uiStatusBarPanel5.Key = "class";
			uiStatusBarPanel5.ProgressBarValue = 0;
			uiStatusBarPanel5.Text = "구분";
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

        #region [1.2] 메뉴 초기화
		/// <summary>
		/// 메뉴 초기화 및 생성
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

                // 현재 메뉴 Command 
                Janus.Windows.UI.CommandBars.UICommand uiCommandGrp = new Janus.Windows.UI.CommandBars.UICommand();

                // 메뉴 Command 추가
                for (int i = 0; i < menuModel.ResultCnt; i++)
                {
                    string MenuCode  = Utility.GetDatasetString(menuModel.MenuDataSet, i, "MenuCode");
                    string MenuName  = Utility.GetDatasetString(menuModel.MenuDataSet, i, "MenuName");
                    string MenuLevel = Utility.GetDatasetString(menuModel.MenuDataSet, i, "MenuLevel");
                    string MenuPower = Utility.GetDatasetString(menuModel.MenuDataSet, i, "MenuPower");

                    // 메뉴레벨이 1이면 메뉴그룹니다.
                    if(MenuLevel.Equals("1"))
                    {
                        // 이전 메뉴그룹이 있다면 메뉴에 그룹을 셋트한다.
                        if(!LastGroupCode.Equals(""))
                        {
                            // 메뉴그룹에 읽기권한이 있는가...
                            if(LastGroupPower.IndexOf("R") >= 0)
                            {
                                //메뉴바에 메뉴그룹 추가
                                this.uiCommandBar_Menu.Commands.Add(uiCommandGrp);

                            }
							
                            //※ 메뉴 그룹이 없으면 메뉴를 표시하지 않는다.
                        }

                        // 메뉴그룹 생성
                        uiCommandGrp = new Janus.Windows.UI.CommandBars.UICommand();
                        uiCommandGrp.Key = MenuCode;
                        uiCommandGrp.Text = MenuName;
                        try
                        {
                            uiCommandGrp.Image = this.imageList.Images[MenuCode];
                        }
                        catch (Exception )
                        {
                            //이미지가 없더라도 오류가 안나게...
                        }

                        LastGroupCode       = MenuCode;		// 이전 메뉴그룹으로 셋트 
                        LastGroupPower      = MenuPower;
                    }
                    else
                    {
                        // 그룹이 아니면 메뉴이므로 그룹에 메뉴를 추가한다.

                        // 메뉴에 읽기권한이 있는가...
                        if(MenuPower.IndexOf("R") >= 0)
                        {
                            //메뉴 생성
                            Janus.Windows.UI.CommandBars.UICommand uiCommand = new Janus.Windows.UI.CommandBars.UICommand();
                            uiCommand.Key = MenuCode;
                            uiCommand.Text = MenuName;
                            try
                            {
                                uiCommand.Image = this.imageList.Images[MenuCode];
                            }
                            catch (Exception)
                            {
                                //이미지가 없더라도 오류가 안나게...
                            }

                            //메뉴그룹에 메뉴추가
                            uiCommandGrp.Commands.Add(uiCommand);

                            // 메뉴 해쉬테이블에 메뉴코드를 넣는다.
                            FrameSystem.oMenu.Add(MenuCode, MenuPower);
                        }
                    }
                }

                // 마지막 메뉴그룹 셋트
                if(!LastGroupCode.Equals(""))
                {
                    this.uiCommandBar_Menu.Commands.Add(uiCommandGrp);
                }

            }
            else
            {
                MessageForm mform = new MessageForm();					
			
                mform.SetTitle = "메뉴 초기화 오류";
                mform.SetMessage = new string[] { "", menuModel.ResultDesc,""}  ;
                mform.showMessage();			
                mform.ShowDialog();
                this.Close();
            }

        }
        #endregion

        #region 로그인정보 초기화
        private void InitializeLogin()
        {	
            String ClientType = "";

            CommonModel   commonModel   = FrameSystem.oComModel;

            stbMainStatus.Panels["id"].Text = "ID:" + commonModel.UserID;
            stbMainStatus.Panels["name"].Text = "이름:" + commonModel.UserName;
            stbMainStatus.Panels["level"].Text = "레벨:" + commonModel.LevelName;
            stbMainStatus.Panels["class"].Text = "구분:" + commonModel.ClassName;

            switch(FrameSystem.m_ClientType)
            {
                case FrameSystem._DEV:
                    ClientType = "개발자";
					stbMainStatus.Office2007ColorScheme = Janus.Windows.UI.Office2007ColorScheme.Black;
                    break;
                case FrameSystem._REAL:
                    ClientType = "";
					stbMainStatus.Office2007ColorScheme = Janus.Windows.UI.Office2007ColorScheme.Blue;
                    break;
                case FrameSystem._TEST:
                    ClientType = "테스트";
					stbMainStatus.Office2007ColorScheme = Janus.Windows.UI.Office2007ColorScheme.Silver;
                    break;
                default:
                    ClientType = "Unknown Client Type";
                    break;
            }

            this.Text = this.Text + " " + FrameSystem.m_SystemVersion + " (" + ClientType + ")" ;
        }

        #endregion

        #region Home 초기화
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
		
        #region 메뉴버튼 클릭 처리 

        ////////////////////////////////////////////////////////////////////////
        // 2011.10.31 메뉴처리방식 변경 메뉴 CommandBar를 이용
        /// <summary>
        /// 메뉴처리
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void uiCommandBar_Menu_CommandClick(object sender, Janus.Windows.UI.CommandBars.CommandEventArgs e)
        {
            // 메인메뉴의 버튼 종류에 따른 
            try
            {

                if (isLoading == false)
                {
                    isLoading = true;

                    string nowMenuCode = e.Command.Key;

                    // 메뉴코드가 메뉴에 존재하는지 검사
                    if (!FrameSystem.oMenu.IsMenu(nowMenuCode))
                    {
                        MessageForm mform = new MessageForm();

                        mform.SetTitle = "시스템 오류";
                        mform.SetMessage = new string[] { "", "해당메뉴가 존재하지 않습니다", "" };
                        mform.showMessage();
                        mform.ShowDialog();
                        return;
                    }   

                    // 이미 사용중인 컨트롤이 있다면 삭제한다.
                    releaseCtlActive();

                    Type form = this.GetType();
                    // 메뉴함수 정의 목록에서 찾아서 해당 메뉴함수를 실행한다.
                    for (int i = 0; i < procList.Length; i++)
                    {
                        if (procList[i].MenuCode.Equals(nowMenuCode))
                        {
                            // 메뉴가 종료라면 "_EXIT"
                            if (procList[i].FuncName.Equals("_EXIT"))
                            {
                                InitializeHome();  // 홈화면을 설정. 왜? 종료취소하면 홈화면이 보이게
                                this.Close();
                                break;
                            }

                            //////////////////////////////////////////////////////////////////////
                            // 해당 클래스명으로 클래스의 등록정보을 얻고
                            //////////////////////////////////////////////////////////////////////
                            Type type = Type.GetType(form.Namespace + "." + procList[i].FuncName);

                            //////////////////////////////////////////////////////////////////////
                            //
                            // 클래스의 인스턴스를 생성한다. 모든 메뉴화면은 IUserControl을 상속받아 해당 인터페이스를 구현해야한다.
                            //
                            //////////////////////////////////////////////////////////////////////
                            iUserCtrl = (IUserControl)Activator.CreateInstance(type);

                            /////////////////////////////////////////////////////////////////////
                            // 인스턴스를 초기화 한다.
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

                    // 만약 활성화된 화면 컨트롤이 없다면 아직 구현되지 않은 메뉴이다.                   
                    if(iUserCtrl == null)
                    {
                        InitializeHome();  // 홈화면을 설정. 왜? 메뉴가 없으면 홈화면이 보이게

                        MessageForm mform = new MessageForm();

                        mform.SetTitle = "시스템 오류";
                        mform.SetMessage = new string[] { "", "해당메뉴는 준비중입니다.", "이용에 불편을 드려서 죄송합니다." };
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

                FrameSystem.oLog.Message("메뉴실행시 오류발생");
                FrameSystem.oLog.Message(ex.Message);

                MessageBox.Show("메뉴실행시 오류가 발생하였습니다. 다시 실행하여 주십시오.\n\n계속 오류가 발생하면 시스템관리자에게 문의하시기 바랍니다.", "시스템오류",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                Trace.WriteLine(ex.Message);
            }

        }

        /// <summary>
        /// 활성컨트롤 해제
        /// </summary>
        private void releaseCtlActive()
        {            
            if(iUserCtrl != null)
            {                
                ((UserControl)iUserCtrl).Dispose();
                iUserCtrl = null;

                // 가베지 컬렉터를 강제로 실행한다.
                GC.Collect();

                Application.DoEvents();
            }
        }    

        #endregion

        #region 기타처리함수

        private void MainForm_Load(object sender, System.EventArgs e)
        {
            uiPanelMain.CaptionVisible = Janus.Windows.UI.InheritableBoolean.False;
			
            StatusBarMessage("준비");
        }

        private void MainForm_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            DialogResult result = MessageBox.Show("AdTargetsPlus Manager를 종료 하시겠습니까?","프로그램 종료",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question,
                MessageBoxDefaultButton.Button2);
            if (result == DialogResult.Yes)
                e.Cancel = false; //Application.Exit();
            else
                e.Cancel = true;
        }

        #endregion

        #region 상태로그 리스트뷰에 기록한다.
		
        private void OnStatusEvent(object sender, StatusEventArgs e)
        {
            StatusBarMessage(e.Message);
        }

        /// <summary>
        /// 상태바에 기록한다.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="Msg"></param>
        private void StatusBarMessage(string Msg)
        {
            stbMainStatus.Panels["Message"].Text = Msg;
        }

        #endregion

		#region 처리중 상태창을 처리한다.

		// Thread를 사용할 경우 이 함수를 이용한다.
		private void progressForm()
		{
			// 처리중 상태창을 연다.
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
					//Thread.Sleep(5);  // 있어야 할 이유??-bae[E_01]
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

				// 열려진 처리중 상태창을 닫는다
				if (null != thProgress)
				{
						if( isLoading ) isLoading = false;
						Thread.Sleep(10);
				}

				// 시작이면
				if(e.Type == ProgressEventArgs.Start)
				{
					isLoading = true;

					// 작업 메시지 Thread 로 처리
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