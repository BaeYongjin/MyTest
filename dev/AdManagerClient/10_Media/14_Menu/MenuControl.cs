/*
 * -------------------------------------------------------
 * Class Name: MenuControl
 * 주요기능  : 메뉴관리
 * 작성자    : YJ.Park
 * 작성일    : 2014.08.20
 * -------------------------------------------------------
 * 2014.04.01 이어보기광고 유/무료 구분 첵크박스 추가 by YS.Jang
 */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;

using WinFramework.Misc;
using WinFramework.Base;
using WinFramework.Data;
using AdManagerModel;
using Janus.Windows.EditControls;

namespace AdManagerClient
{
    public partial class MenuControl : System.Windows.Forms.UserControl,IUserControl
    {
        #region [공통코드] 이벤트핸들러및 함수
        public event StatusEventHandler StatusEvent;
        public event ProgressEventHandler ProgressEvent;

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

        #region [공통코드] 사용자 정의 객체 및 변수
        // 시스템 정보 : 화면공통
        private SystemModel     systemModel = FrameSystem.oSysModel;
        private CommonModel     commonModel = FrameSystem.oComModel;
        private Logger          log         = FrameSystem.oLog;
        private MenuPower       menu        = FrameSystem.oMenu;

        // 메뉴코드 : 보안이 필요한 화면에 필요함
        private string          menuCode    = "";

        //사용할 정보모델
        MediaMenuModel mediaMenuModel = new MediaMenuModel();

        //화면 처리용 변수
        CurrencyManager cm      = null;
        CurrencyManager cmChild = null;
        DataTable       dt      = null;
        DataTable       dtChild = null;

        bool IsSearching			= false; //조회중 상세화면이 업데이트 되는것을 방지..
        bool IsAdding				= false;
        bool canRead				= false;
        bool canUpdate				= false;
        bool canCreate				= false;
        bool canDelete				= false;
        bool canSync				= false;
		bool replayYnCheck			= false;	//이어보기 광고 체크박스 변경 체크
		bool rendingYnCheck			= false;	//추천엔딩 광고 체크박스 변경 체크
        private string mediaCode	= null;
        private string categoryCode	= null;
        private string dataMenuCode	= null;

        private string preCategoryPreRollYn = null;
        private string preCategoryMidRollYn = null;
        private string preCategoryPostRollYn = null;
        private string preCategoryPayYn = null;
        private int preCategoryRate = 0;
        private int preCategoryNRate = 0;

        private string preMenuPreRollYn = null;
        private string preMenuMidRollYn = null;
        private string preMenuPostRollYn = null;
        private string preMenuPayYn = null;
        private int preMenuRate = 0;
        private int preMenuNRate = 0;

        bool menufocus              = false; // 현재 포커싱이 어디되어있는지..
        #endregion

        #region [공통코드] IUserControl 구현
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
        }
        #endregion

        #region 화면 컴포넌트
        public MenuControl()
        {
            InitializeComponent();
        }
        #endregion

        #region 컨트롤 로드

        private void MenuControl_Load(object sender, EventArgs e)
        {
            dt = ((DataView)grdExCategoryList.DataSource).Table;
            dtChild = ((DataView)grdEXMenuList.DataSource).Table;

            cm = (CurrencyManager)this.BindingContext[grdExCategoryList.DataSource];
            //cm.PositionChanged += new System.EventHandler(OnGrdRowChanged);

            cmChild = (CurrencyManager)this.BindingContext[grdEXMenuList.DataSource];
            //cmChild.PositionChanged += new System.EventHandler(OnGrdRowDetailChanged);

            preCategoryPreRollYn = "";
            preCategoryMidRollYn = "";
            preCategoryPostRollYn = "";
            preCategoryPayYn = "";
            preCategoryRate = 0;
            preCategoryNRate = 0;

            preMenuPreRollYn = "";
            preMenuMidRollYn = "";
            preMenuPostRollYn = "";
            preMenuPayYn = "";
            preMenuRate = 0;
            preMenuNRate = 0;

            //컨트롤 초기화
            InitControl();
        }
        #endregion

        #region 컨트롤 초기화
        private void InitControl()
        {
            ProgressStart();
            //InitCombo();
            //InitCombo_Level();

            //조회 권한 검사
            if (menu.CanRead(MenuCode))
            {
                canRead = true;
                canSync = true;
                SearchCategory();
            }
            //추가버튼 활성화
			//if (menu.CanCreate(MenuCode))
			//{
			//    canCreate = true;
			//}
            //삭제버튼 활성화
			canDelete = false;
			//if (menu.CanDelete(MenuCode))
			//{
			//    canDelete = true;
			//}
            //저장버튼 활성화
			
            if (menu.CanUpdate(MenuCode))
            {
                //canUpdate = true;
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
            MediaCodeModel mediacodeModel = new MediaCodeModel();
            new MediaCodeManager(systemModel, commonModel).GetMediaCodeList(mediacodeModel);

            if (mediacodeModel.ResultCD.Equals("0000"))
            {
                //데이터셋에 셋팅
                Utility.SetDataTable(menuDs1.Media, mediacodeModel.MediaCodeDataSet);
            }

            //매체 콤보

            this.cbSearchMediaName.Items.Clear();
            //콤보박스에 셋트할 코드목록을 담을 Item배열을 선언
            Janus.Windows.EditControls.UIComboBoxItem[] comboItems = new Janus.Windows.EditControls.UIComboBoxItem[mediacodeModel.ResultCnt + 1];

            comboItems[0] = new Janus.Windows.EditControls.UIComboBoxItem("매체선택", "00");

            for (int i = 0; i < mediacodeModel.ResultCnt; i++)
            {
                DataRow row = menuDs1.Media.Rows[i];

                string val = row["MediaCode"].ToString();
                string txt = row["MediaName"].ToString();
                comboItems[i + 1] = new Janus.Windows.EditControls.UIComboBoxItem(txt, val);
            }
            //콤보에 셋트
            this.cbSearchMediaName.Items.AddRange(comboItems);
            this.cbSearchMediaName.SelectedIndex = 0;

            Application.DoEvents();
        }
        private void InitCombo_Level()
        {
            if (commonModel.UserLevel == "20")
            {
                cbSearchMediaName.SelectedValue = commonModel.MediaCode;
                cbSearchMediaName.ReadOnly = true;
            }
            else
            {
                for (int i = 0; i < menuDs1.Media.Rows.Count; i++)
                {
                    DataRow row = menuDs1.Media.Rows[i];
                    if (row["MediaCode"].ToString().Equals(FrameSystem._HANATV.ToString()))
                    {
                        cbSearchMediaName.SelectedValue = FrameSystem._HANATV;
                        break;
                    }
                    else
                    {
                        cbSearchMediaName.SelectedValue = "00";
                    }
                }
            }

            Application.DoEvents();
        }

        private void InitButton()
        {
            if (canRead)
            {
                btnSearch.Enabled = true;
            }
            if (ebCategoryName.Text.Trim().Length > 0)
            {
                if (canUpdate) btnSave.Enabled = true;
            }

			replayYnCheck = false;
			
			rendingYnCheck = false;

            Application.DoEvents();
        }

        private void DisableButton()
        {
            btnSearch.Enabled = false;
            btnSave.Enabled = false;

            Application.DoEvents();
        }

        #endregion

        #region 사용자 액션처리 메소드
        /// <summary>
        /// 카테고리 그리드의 Row 변경시
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnGrdRowChanged(object sender, EventArgs e)
        {
            if (!IsSearching)
            {
                if (grdExCategoryList.RecordCount > 0)
                {
                    ResetDetail();
                    SetCategoryDetailText();
                    SetMenuList();
                    IsAdding = false;
                    StatusMessage("준비");
                }

                menufocus = false;
                InitButton();
            }
        }

        /// <summary>
        /// 메뉴 그리드의 ROW 변경시
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnGrdRowDetailChanged(object sender, EventArgs e)
        {
            if (grdEXMenuList.RecordCount > 0)
            {
                SetMenuDetailText();
                IsAdding = false;
                StatusMessage("준비");
            }

            InitButton();
        }

        /// <summary>
        /// 조회버튼 클릭
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSearch_Click(object sender, EventArgs e)
        {
            ProgressStart();
            DisableButton();
            if (cbSearchMediaName.SelectedValue.ToString() == "00")
            {
                FrameSystem.showMsgForm("정보검색 오류", new string[] { "", "매체를 선택하여 주세요.", "" });
                ProgressStop();
                InitButton();
                return;
            }

            SearchCategory();
            InitButton();
            ProgressStop();
        }

        /// <summary>
        /// 저장버튼 클릭
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!p_mPreYn.Enabled)
            {
                SaveCategoryDetail();
            }
            else
            {
                SaveMenuDetail();
            }
        }

        private void grdEXMenuList_Click(object sender, EventArgs e)
        {
            menufocus = true;

            SetTextStatus();
            int curRow = cmChild.Position;
            if (curRow < 0)
            {
                ResetMenuDetail();
            }
            else
            {
                SetMenuDetailText();
                IsAdding = false;
                StatusMessage("준비");
            }

            InitButton();
        }

        private void grdExCategoryList_Click(object sender, EventArgs e)
        {
            menufocus = false;

            if (grdExCategoryList.RecordCount > 0)
            {
                ResetDetail();
                SetCategoryDetailText();
                SetMenuList();
                IsAdding = false;
                StatusMessage("준비");
            }

            InitButton();
        }

        #endregion

        #region 처리메소드
        /// <summary>
        /// 카테고리 조회
        /// </summary>
        private void SearchCategory()
        {
            IsSearching = true;

            StatusMessage("카테고리 정보를 조회합니다.");

            try
            {
                mediaMenuModel.Init();
                menuDs1.Category.Clear();

                new MediaMenuManager(systemModel, commonModel).GetCategoryList(mediaMenuModel);

                if (mediaMenuModel.ResultCD.Equals("0000"))
                {
                    Utility.SetDataTable(menuDs1.Category, mediaMenuModel.UserDataset);
                    StatusMessage(mediaMenuModel.ResultCnt + "건의 카테고리 정보가 조회되었습니다.");

                    //RowChoice();

                    SetCategoryDetailText();
                    SetMenuList();
                    grdExCategoryList.Focus();
                }

            }
            catch (FrameException fe)
            {
                FrameSystem.showMsgForm("카테고리 조회 오류", new string[] { fe.ErrCode, fe.ResultMsg });
            }
            catch (Exception ex)
            {
                FrameSystem.showMsgForm("카테고리 조회 오류", new string[] { "", ex.Message });
            }
            finally
            {
                IsSearching = false;
            }
        }

        /// <summary>
        /// 카테고리 상세정보
        /// </summary>
        private void SetCategoryDetailText()
        {
            int curRow = cm.Position;

            if (curRow >= 0)
            {
                SetTextStatus();

                preCategoryPreRollYn  = "";
                preCategoryMidRollYn  = "";
                preCategoryPostRollYn = "";
                preCategoryPayYn  = "";

                preCategoryRate = 0;
                preCategoryNRate = 0;

                ebCategoryCode.Text = dt.Rows[curRow]["MENU_COD"].ToString();
                ebCategoryName.Text = dt.Rows[curRow]["MENU_NM"].ToString();

                preCategoryPreRollYn = dt.Rows[curRow]["AD_PRE_YN"].ToString();
                preCategoryMidRollYn = dt.Rows[curRow]["AD_MID_YN"].ToString();
                preCategoryPostRollYn = dt.Rows[curRow]["AD_POST_YN"].ToString();
                preCategoryPayYn = dt.Rows[curRow]["AD_PAY_YN"].ToString();

				// PreRoll
                if (dt.Rows[curRow]["AD_PRE_YN"].ToString().Equals("Y"))
                    p_cPreYn.CheckState = CheckState.Checked;
                else if (dt.Rows[curRow]["AD_PRE_YN"].ToString().Equals("I"))
                    p_cPreYn.CheckState = CheckState.Indeterminate;
                else
                    p_cPreYn.CheckState = CheckState.Unchecked;
                
                // MidRoll
                if (dt.Rows[curRow]["AD_MID_YN"].ToString().Equals("Y"))
                    p_cMidYn.CheckState = CheckState.Checked;
                else if (dt.Rows[curRow]["AD_MID_YN"].ToString().Equals("I"))
                    p_cMidYn.CheckState = CheckState.Indeterminate;
                else
                    p_cMidYn.CheckState = CheckState.Unchecked;

                // PostRoll
                if (dt.Rows[curRow]["AD_POST_YN"].ToString().Equals("Y"))
                    p_cPostYn.CheckState = CheckState.Checked;
                else if (dt.Rows[curRow]["AD_POST_YN"].ToString().Equals("I"))
                    p_cPostYn.CheckState = CheckState.Indeterminate;
                else
                    p_cPostYn.CheckState = CheckState.Unchecked;

                // 유료컨텐츠
                if (dt.Rows[curRow]["AD_PAY_YN"].ToString().Equals("Y"))
                    p_cPayYn.CheckState = CheckState.Checked;
                else if (dt.Rows[curRow]["AD_PAY_YN"].ToString().Equals("I"))
                    p_cPayYn.CheckState = CheckState.Indeterminate;
                else
                    p_cPayYn.CheckState = CheckState.Unchecked;

                // 광고집행비율
                if (!dt.Rows[curRow]["AD_RATE"].ToString().Equals(string.Empty))
                {
                    ud_cAdRate.Value = Convert.ToInt32(dt.Rows[curRow]["AD_RATE"].ToString());
                    preCategoryRate = ud_cAdRate.Value;
                }

                // ADNetwork 광고집행비율
                if (!dt.Rows[curRow]["ADN_RATE"].ToString().Equals(string.Empty))
                {
                    ud_cAdNRate.Value = Convert.ToInt32(dt.Rows[curRow]["ADN_RATE"].ToString());
                    preCategoryNRate = ud_cAdNRate.Value;
                }
            }
        }

        /// <summary>
        /// 메뉴 조회
        /// </summary>
        private void SetMenuList()
        {
            try
            {
                mediaMenuModel.Init();
                menuDs1.Menu.Clear();

                int curRow = cm.Position;

                if (curRow < 0) return;
                mediaMenuModel.CategoryCode = dt.Rows[curRow]["MENU_COD"].ToString();

                new MediaMenuManager(systemModel, commonModel).GetMenuList(mediaMenuModel);

                if (mediaMenuModel.ResultCD.Equals("0000"))
                {
                    Utility.SetDataTable(menuDs1.Menu, mediaMenuModel.UserDataset);
                    StatusMessage(mediaMenuModel.ResultCnt + "건의 메뉴 정보가 조회되었습니다.");
                }
            }
            catch (FrameException fe)
            {
                FrameSystem.showMsgForm("메뉴 조회 오류", new string[] { fe.ErrCode, fe.ResultMsg });
            }
            catch (Exception ex)
            {
                FrameSystem.showMsgForm("메뉴 조회 오류", new string[] { "", ex.Message });
            }
            
        }

        /// <summary>
        /// 메뉴 상세정보
        /// </summary>
        private void SetMenuDetailText()
        {
            int curRow = cmChild.Position;

            if (curRow >= 0)
            {
                SetTextStatus();

                preMenuPreRollYn = "";
                preMenuMidRollYn = "";
                preMenuPostRollYn = "";
                preMenuPayYn = "";

                preMenuRate = 0;
                preMenuNRate = 0;

                ebMenuCode.Text = dtChild.Rows[curRow]["MENU_COD"].ToString();
                ebMenuName.Text = dtChild.Rows[curRow]["MENU_NM"].ToString();

                preMenuPreRollYn = dtChild.Rows[curRow]["AD_PRE_YN"].ToString();
                preMenuMidRollYn = dtChild.Rows[curRow]["AD_MID_YN"].ToString();
                preMenuPostRollYn = dtChild.Rows[curRow]["AD_POST_YN"].ToString();
                preMenuPayYn = dtChild.Rows[curRow]["AD_PAY_YN"].ToString();

                // PreRoll
                if (dtChild.Rows[curRow]["AD_PRE_YN"].ToString().Equals("Y"))
                    p_mPreYn.CheckState = CheckState.Checked;
                else
                    p_mPreYn.CheckState = CheckState.Unchecked;

                // MidRoll
                if (dtChild.Rows[curRow]["AD_MID_YN"].ToString().Equals("Y"))
                    p_mMidYn.CheckState = CheckState.Checked;
                else
                    p_mMidYn.CheckState = CheckState.Unchecked;

                // PostRoll
                if (dtChild.Rows[curRow]["AD_POST_YN"].ToString().Equals("Y"))
                    p_mPostYn.CheckState = CheckState.Checked;
                else
                    p_mPostYn.CheckState = CheckState.Unchecked;

                // 유료컨텐츠
                if (dtChild.Rows[curRow]["AD_PAY_YN"].ToString().Equals("Y"))
                    p_mPayYn.CheckState = CheckState.Checked;
                else
                    p_mPayYn.CheckState = CheckState.Unchecked;

                // 광고집행비율
                if (!dtChild.Rows[curRow]["AD_RATE"].ToString().Equals(string.Empty))
                {
                    ud_mAdRate.Value = Convert.ToInt32(dtChild.Rows[curRow]["AD_RATE"].ToString());
                    preMenuRate = ud_mAdRate.Value;
                }

                // ADNetwork 광고집행비율
                if (!dtChild.Rows[curRow]["ADN_RATE"].ToString().Equals(string.Empty))
                {
                    ud_mAdNRate.Value = Convert.ToInt32(dtChild.Rows[curRow]["ADN_RATE"].ToString());
                    preMenuNRate = ud_mAdNRate.Value;
                }
            }
        }

        /// <summary>
        /// 카테고리 상세정보 저장
        /// </summary>
        private void SaveCategoryDetail()
        {
            StatusMessage("카테고리 정보를 저장합니다.");

            try
            {
                mediaMenuModel.Init();
                
                mediaMenuModel.CategoryCode = ebCategoryCode.Text.Trim();
                string rowPointKeyValue = mediaMenuModel.CategoryCode;

                string preYn = GetCheckedToString(p_cPreYn);
                if (!preYn.Equals(preCategoryPreRollYn))
                {
                    mediaMenuModel.CategoryAdPreRollYn = preYn;
                    mediaMenuModel.MenuAdPreRollYn = GetMenuCodeAccordingToCategory(p_cPreYn, preCategoryPreRollYn);
                }

                string midYn = GetCheckedToString(p_cMidYn);
                if (!midYn.Equals(preCategoryMidRollYn))
                {
                    mediaMenuModel.CategoryAdMidRollYn = midYn;
                    mediaMenuModel.MenuAdMidRollYn = GetMenuCodeAccordingToCategory(p_cMidYn, preCategoryMidRollYn);
                }

                string postYn = GetCheckedToString(p_cPostYn);
                if (!postYn.Equals(preCategoryPostRollYn))
                {
                    mediaMenuModel.CategoryAdPostRollYn = postYn;
                    mediaMenuModel.MenuAdPostRollYn = GetMenuCodeAccordingToCategory(p_cPostYn, preCategoryPostRollYn);
                }

                string payYn = GetCheckedToString(p_cPayYn);
                if (!payYn.Equals(preCategoryPayYn))
                {
                    mediaMenuModel.CategoryAdPayYn = payYn;
                    mediaMenuModel.MenuAdPayYn = GetMenuCodeAccordingToCategory(p_cPayYn, preCategoryPayYn);
                }
                
                mediaMenuModel.CategoryAdRate = ud_cAdRate.Value;
                mediaMenuModel.CategoryAdNRate = ud_cAdNRate.Value;

                new MediaMenuManager(systemModel, commonModel).SetCategoryUpdate(mediaMenuModel);

                // 저장 서비스 호출
                //if (IsAdding)
                //{
                //    new MediaMenuManager(systemModel, commonModel).SetCategoryCreate(mediaMenuModel);
                //}
                //else
                //{
                //    new MediaMenuManager(systemModel, commonModel).SetCategoryUpdate(mediaMenuModel);
                //}

                StatusMessage("카테고리 정보가 저장되었습니다.");
                DisableButton();
                SearchCategory();
                InitButton();

                RowChoice(rowPointKeyValue);
            }
            catch (FrameException fe)
            {
                FrameSystem.showMsgForm("카테고리 정보 저장 오류", new string[] { fe.ErrCode, fe.ResultMsg });
            }
            catch (Exception ex)
            {
                FrameSystem.showMsgForm("카테고리 정보 저장 오류", new string[] { "", ex.Message });
            }
        }

        /// <summary>
        /// 메뉴 상세정보 저장
        /// </summary>
        private void SaveMenuDetail()
        {
            StatusMessage("메뉴 정보를 저장합니다.");
            
            try
            {
                mediaMenuModel.CategoryCode = ebCategoryCode.Text.Trim();
                mediaMenuModel.MenuCode = ebMenuCode.Text.Trim();

                string categoryCode = mediaMenuModel.CategoryCode;
                string menuCode = mediaMenuModel.MenuCode;

                string preYn = GetCheckedToString(p_mPreYn);
                if (!preYn.Equals(preMenuPreRollYn))
                {
                    mediaMenuModel.MenuAdPreRollYn = preYn;
                    mediaMenuModel.CategoryAdPreRollYn = GetCategoryCodeAccordingToMenu(p_mPreYn, preMenuPreRollYn, preCategoryPreRollYn);
                }

                string midYn = GetCheckedToString(p_mMidYn);
                if (!midYn.Equals(preMenuMidRollYn))
                {
                    mediaMenuModel.MenuAdMidRollYn = midYn;
                    mediaMenuModel.CategoryAdMidRollYn = GetCategoryCodeAccordingToMenu(p_mMidYn, preMenuMidRollYn, preCategoryMidRollYn);
                }

                string postYn = GetCheckedToString(p_mPostYn);
                if (!postYn.Equals(preMenuPostRollYn))
                {
                    mediaMenuModel.MenuAdPostRollYn = postYn;
                    mediaMenuModel.CategoryAdPostRollYn = GetCategoryCodeAccordingToMenu(p_mPostYn, preMenuPostRollYn, preCategoryPostRollYn);
                }

                string payYn = GetCheckedToString(p_mPayYn);
                if (!payYn.Equals(preMenuPayYn))
                {
                    mediaMenuModel.MenuAdPayYn = payYn;
                    mediaMenuModel.CategoryAdPayYn = GetCategoryCodeAccordingToMenu(p_mPayYn, preMenuPayYn ,preCategoryPayYn);
                }

                mediaMenuModel.MenuAdRate = ud_mAdRate.Value;
                mediaMenuModel.MenuAdNRate = ud_mAdNRate.Value;

                mediaMenuModel.CategoryAdRate = ud_cAdRate.Value;
                mediaMenuModel.CategoryAdNRate = ud_cAdNRate.Value;

                new MediaMenuManager(systemModel, commonModel).SetMenuUpdate(mediaMenuModel);

                //상세정보 저장 서비스 호출
                //if (IsAdding)
                //{
                //    new MediaMenuManager(systemModel, commonModel).SetMenuCreate(mediaMenuModel);
                //}
                //else
                //{
                //    new MediaMenuManager(systemModel, commonModel).SetMenuUpdate(mediaMenuModel);
                //}

                StatusMessage("메뉴 정보가 저장되었습니다.");
                DisableButton();
				SearchCategory();
                grdEXMenuList.Focus();
                InitButton();

                RowChoice(categoryCode);
                RowChoiceDetail(menuCode);
            }
            catch (FrameException fe)
            {
                FrameSystem.showMsgForm("메뉴 정보 저장 오류", new string[] { fe.ErrCode, fe.ResultMsg });
            }
            catch (Exception ex)
            {
                FrameSystem.showMsgForm("메뉴 정보 저장 오류", new string[] { "", ex.Message });
            }
        }

        private void ResetDetail()
        {
            if (!menufocus)
            {
                ResetCategoryDetail();
            }

            ResetMenuDetail();
        }

        private void ResetCategoryDetail()
        {
            ebCategoryCode.Text = "";
            ebCategoryName.Text = "";

            p_cPreYn.CheckState = CheckState.Unchecked;
            p_cMidYn.CheckState = CheckState.Unchecked;
            p_cPostYn.CheckState = CheckState.Unchecked;
            p_cPayYn.CheckState = CheckState.Unchecked;
            ud_cAdRate.Value = 0;
            ud_cAdNRate.Value = 0;
        }

        private void ResetMenuDetail()
        {
            ebMenuCode.Text = "";
            ebMenuName.Text = "";

            p_mPreYn.CheckState = CheckState.Unchecked;
            p_mMidYn.CheckState = CheckState.Unchecked;
            p_mPostYn.CheckState = CheckState.Unchecked;
            p_mPayYn.CheckState = CheckState.Unchecked;
            ud_mAdRate.Value = 0;
            ud_mAdNRate.Value = 0;
        }

        /// <summary>
        /// 상세정보 ReadOnly
        /// </summary>
        private void SetTextReadonly()
        {
            p_cPreYn.Enabled    = false;
            p_cMidYn.Enabled    = false;
            p_cPostYn.Enabled   = false;
            p_cPayYn.Enabled    = false;
            ud_cAdRate.Enabled  = false;
            ud_cAdNRate.Enabled = false;

            p_mPreYn.Enabled    = false;
            p_mMidYn.Enabled    = false;
            p_mPostYn.Enabled   = false;
            p_mPayYn.Enabled    = false;
            ud_mAdRate.Enabled  = false;
            ud_mAdNRate.Enabled = false;
        }

        private void SetTextStatus()
        {
            SetTextReadonly();

            if (!menufocus)
            {
                setCategoryTextStatus();
            }
            else
            {
                setMenuTextStatus();
            }
        }

        /// <summary>
        /// 카테고리 focus시에 상세정보 수정 가능하도록
        /// </summary>
        private void setCategoryTextStatus()
        {
            p_cPreYn.Enabled    = true;
            p_cMidYn.Enabled    = true;
            p_cPostYn.Enabled   = true;
            p_cPayYn.Enabled    = true;
            ud_cAdRate.Enabled  = true;
            ud_cAdNRate.Enabled = true;
        }

        /// <summary>
        /// 메뉴 focus시에 상세정보 수정 가능하도록
        /// </summary>
        private void setMenuTextStatus()
        {
            p_mPreYn.Enabled    = true;
            p_mMidYn.Enabled    = true;
            p_mPostYn.Enabled   = true;
            p_mPayYn.Enabled    = true;
            ud_mAdRate.Enabled  = true;
            ud_mAdNRate.Enabled = true;
        }

        /// <summary>
        /// 키값을 찾아 그리드 키에 해당되는 로우로.
        /// </summary>
        private void RowChoice(string rowPointKeyValue)
        {
            StatusMessage("키값 찾기");
            try
            {
                int rowindex = 0;
                if (menuDs1.Tables["Category"].Rows.Count < 1) return;

                foreach (DataRow row in menuDs1.Tables["Category"].Rows)
                {
                    if (!IsAdding)
                    {
                        if (row["MENU_COD"].ToString().Equals(rowPointKeyValue))
                        {
                            cm.Position = rowindex;
                            break;
                        }
                    }

                    rowindex++;
                    grdExCategoryList.EnsureVisible();
                }

                SetCategoryDetailText();
                SetMenuList();
            }
            catch (FrameException fe)
            {
                FrameSystem.showMsgForm("키값 오류", new string[] { fe.ErrCode, fe.ResultMsg });
            }
            catch (Exception ex)
            {
                FrameSystem.showMsgForm("키캆오류", new string[] { "", ex.Message });
            }
        }

		/// <summary>
		/// Row이동을 위한 직전 키값
		/// </summary>
		private string keyMenuBefore = "";
		private int keyMenuBeforeIdx = 0;
		/// <summary>
		/// 메뉴Row이동시키기
		/// </summary>
        private void RowChoiceDetail(string rowPointKeyValue)
		{
			try
			{
				int rowindex = 0;
				if (menuDs1.Tables["Menu"].Rows.Count < 1) return;

				foreach (DataRow row in menuDs1.Tables["Menu"].Rows)
				{
                    if (row["MENU_COD"].ToString().Equals(rowPointKeyValue))
					{
						cmChild.Position = rowindex;
						break;
					}
					rowindex++;
					//grdEXMenuList.VerticalScroll.Value = keyMenuBeforeIdx;
					grdEXMenuList.EnsureVisible();
				}
			}
			catch (FrameException fe)
			{
				FrameSystem.showMsgForm("키값 오류", new string[] { fe.ErrCode, fe.ResultMsg });
			}
			catch (Exception ex)
			{
				FrameSystem.showMsgForm("키캆오류", new string[] { "", ex.Message });
			}
		}
        #endregion

        #region UICheckBox 처리
        private void p_cPreYn_CheckedChanged(object sender, EventArgs e)
        {
            if ((preCategoryPreRollYn.Equals("N") || preCategoryPreRollYn.Equals("I")) && p_cPreYn.CheckState == CheckState.Checked)
            {
                if (MessageBox.Show("[PreRoll 집행설정]\n\n 카테고리 전체 적용으로 변경 하시겠습니까?", "설정확인", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2, MessageBoxOptions.DefaultDesktopOnly) == DialogResult.No)
                {
                    if (preCategoryPreRollYn.Equals("N"))
                    {
                        p_cPreYn.CheckState = CheckState.Unchecked;
                    }
                    else if (preCategoryPreRollYn.Equals("I"))
                    {
                        p_cPreYn.CheckState = CheckState.Indeterminate;
                    }
                }
            }
        }

        private void p_cMidYn_CheckedChanged(object sender, EventArgs e)
        {
            if ((preCategoryMidRollYn.Equals("N") || preCategoryMidRollYn.Equals("I")) && p_cMidYn.CheckState == CheckState.Checked)
            {
                if (MessageBox.Show("[MidRoll 집행설정]\n\n 카테고리 전체 적용으로 변경 하시겠습니까?", "설정확인", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2, MessageBoxOptions.DefaultDesktopOnly) == DialogResult.No)
                {
                    if (preCategoryMidRollYn.Equals("N"))
                    {
                        p_cMidYn.CheckState = CheckState.Unchecked;
                    }
                    else if (preCategoryMidRollYn.Equals("I"))
                    {
                        p_cMidYn.CheckState = CheckState.Indeterminate;
                    }
                }
            }
        }

        private void p_cPostYn_CheckedChanged(object sender, EventArgs e)
        {
            if ((preCategoryPostRollYn.Equals("N") || preCategoryPostRollYn.Equals("I")) && p_cPostYn.CheckState == CheckState.Checked)
            {
                if (MessageBox.Show("[PostRoll 집행설정]\n\n 카테고리 전체 적용으로 변경 하시겠습니까?", "설정확인", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2, MessageBoxOptions.DefaultDesktopOnly) == DialogResult.No)
                {
                    if (preCategoryPostRollYn.Equals("N"))
                    {
                        p_cPostYn.CheckState = CheckState.Unchecked;
                    }
                    else if (preCategoryPostRollYn.Equals("I"))
                    {
                        p_cPostYn.CheckState = CheckState.Indeterminate;
                    }
                }
            }
        }

        private void p_cPayYn_CheckedChanged(object sender, EventArgs e)
        {
            if ((preCategoryPayYn.Equals("N") || preCategoryPayYn.Equals("I")) && p_cPayYn.CheckState == CheckState.Checked)
            {
                if (MessageBox.Show("[유료컨텐츠 집행설정]\n\n 카테고리 전체 적용으로 변경 하시겠습니까?", "설정확인", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2, MessageBoxOptions.DefaultDesktopOnly) == DialogResult.No)
                {
                    if (preCategoryPayYn.Equals("N"))
                    {
                        p_cPayYn.CheckState = CheckState.Unchecked;
                    }
                    else if (preCategoryPayYn.Equals("I"))
                    {
                        p_cPayYn.CheckState = CheckState.Indeterminate;
                    }
                }
            }
        }

        private string GetCheckedToString(UICheckBox chkBox)
        {
            string result = "";
            
            result = chkBox.CheckState == CheckState.Checked ? "Y" : chkBox.CheckState == CheckState.Indeterminate ? "I" : "N";
            
            return result;
        }

        private string GetMenuCodeAccordingToCategory(UICheckBox chkBox, string preValue)
        {
            string result = "";
            string value = GetCheckedToString(chkBox);

            if(preValue.Equals("Y") && (value.Equals("N") || value.Equals("I")))
            {
                result = "N";
            }
            else if (preValue.Equals("N") && (value.Equals("Y")))
            {
                result = "Y";
            }
            else if (preValue.Equals("I") && (value.Equals("Y") || value.Equals("N")))
            {
                result = value;
            }

            return result;
        }

        private string GetCategoryCodeAccordingToMenu(UICheckBox chkBox, string preMenuValue, string preCategoryVale)
        {
            string result = "";
            string value = GetCheckedToString(chkBox);

            if (preMenuValue.Equals("Y") && value.Equals("N"))
            {
                if (preCategoryVale.Equals("Y"))
                {
                    result = "I";
                }
            }
            else if (preMenuValue.Equals("N") && (value.Equals("Y")))
            {
                if (preCategoryVale.Equals("N"))
                {
                    result = "I";
                }
            }

            return result;
        }
        #endregion
    }
}