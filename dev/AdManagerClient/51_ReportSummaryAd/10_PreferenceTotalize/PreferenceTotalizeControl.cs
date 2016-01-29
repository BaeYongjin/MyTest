/*
 * -------------------------------------------------------
 * Class Name: PreferenceTotalizeControl.cs
 * 주요기능  : 선호도 조사 팝업 집계 컨트롤
 * -------------------------------------------------------
 * [수정사항]
 * -------------------------------------------------------
 * 수정코드  : [E_01]
 * 수정자    : 김보배
 * 수정일    : 2013.09.05
 * 수정내용  :        
 *            - 광고 노출수 / 팝업 노출수 계산되도록 변경
 *            - 기존 데이터셋처럼 변경
 *            - 검색어 부분 수정
 * --------------------------------------------------------
 */
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using WinFramework.Misc;
using WinFramework.Base;
using WinFramework.Data;
using AdManagerClient.Common;
using System.Reflection;

using AdManagerModel;

namespace AdManagerClient
{
    public partial class PreferenceTotalizeControl : UserControl, IUserControl
    {
        #region 이벤트핸들러
		public event StatusEventHandler 			StatusEvent;			// 상태이벤트 핸들러
		public event ProgressEventHandler 			ProgressEvent;			// 처리중이벤트 핸들러
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
        }

        #endregion

        #region 사용자정의 객체 및 변수

        // 시스템 정보 : 화면공통
        private SystemModel systemModel = FrameSystem.oSysModel;
        private CommonModel commonModel = FrameSystem.oComModel;
        private Logger log = FrameSystem.oLog;
        private MenuPower menu = FrameSystem.oMenu;

        // 메뉴코드 : 보안이 필요한 화면에 필요함
        private string menuCode = "";

        // 사용할 정보모델
        PreferenceTotalizeModel preferenceTotalizeModel = new PreferenceTotalizeModel();

        bool canRead = false;

        // 화면처리용 변수
        bool IsNewSearchKey = true;     // 검색어입력 여부 
        bool IsSearching = false;       // 조회중 상세화면이 업데이트 되는 것을 방지 하기위함

        CurrencyManager cm    = null;
        DataTable       dt    = null;

        public string keyItemNo     = null; // 광고번호
        public string keyPopCode    = null; // 팝업 ID
        public string keyAdvtExmNo  = null; // 선호도 ID

        public string keyStartDay   = null; // 조회시작일
        public string keyEndDay     = null; // 조회종료일
        
        #endregion

        #region 컨트롤초기화

        private void InitControl()
        {
            ProgressStart();

            // 조회권한 검사
            if (menu.CanRead(menuCode))
            {
                canRead = true;
            }

            InitButton();
        }

        private void InitButton()
        {
            if (canRead)
            {
                btnSearch.Enabled = true;
                ebAdName.Enabled = true;
            }

            Application.DoEvents();
        }

        private void DisavleButton()
        {
            btnSearch.Enabled = false;
            ebAdName.Enabled = false;

            Application.DoEvents();
        }

        #endregion

        #region 컨트롤 로드

        public PreferenceTotalizeControl()
        {
            InitializeComponent();
        }

        private void PreferenceTotalizeControl_Load(object sender, EventArgs e)
        {
            // 데이터관리용 객체 생성
            dt = ((DataView)grdAdvList.DataSource).Table;
            cm = (CurrencyManager) this.BindingContext[grdAdvList.DataSource];
            cm.PositionChanged += new System.EventHandler(OnGrdRowChanged);

            // 컨트롤 초기화
            InitControl();

            GetAdList();
        }

        #endregion

        #region 컨트롤 액션처리 메소드

        private void btnSearch_Click(object sender, EventArgs e)
        {
            GetAdList();
        }

        /// <summary>
        /// 그리드의 Row변경시
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnGrdRowChanged(object sender, System.EventArgs e)
        {
            try
            {
                if (!IsSearching) // 조회중이 아닐경우에만 동작
                {
                    SetDetailText();
                    InitButton();
                }
            }
            catch
            {
                InitButton();
            }
        }

        private void ebAdName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                GetAdList();
            }
        }

        /// <summary>
        /// [E_01] 검색어 변경
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ebAdName_TextChanged(object sender, EventArgs e)
        {
            IsNewSearchKey = false;
        }

        /// <summary>
        /// [E_01] 검색어 클릭
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ebAdName_Click(object sender, EventArgs e)
        {
            if (IsNewSearchKey)
            {
                ebAdName.Text = "";
            }
            else
            {
                ebAdName.SelectAll();
            }
        }

        #endregion

        #region 처리 메소드

        /// <summary>
        /// 설문목록 리스트 조회
        /// </summary>
        private void GetAdList()
        {
            IsSearching = true;

            try
            {
                // 검색어 설정
                if (IsNewSearchKey)
                {
                    preferenceTotalizeModel.KeySearch = "";
                }
                else
                {
                    preferenceTotalizeModel.KeySearch = ebAdName.Text.ToString().Trim();
                }

                new PreferenceTotalizeManager(systemModel, commonModel).GetAdList(preferenceTotalizeModel);

                if (preferenceTotalizeModel.ResultCD.Equals("0000"))
                {
                    Utility.SetDataTable(preferenceTotalizeDs.AdList, preferenceTotalizeModel.PreferenceDataSet);

                    AddAdChoice();
                }
            }
            catch (FrameException fe)
            {
                FrameSystem.showMsgForm("광고목록 리스트 조회오류", new string[] { fe.ErrCode, fe.ResultMsg });
            }
            catch (Exception ex)
            {
                FrameSystem.showMsgForm("광고목록 리스트 조회오류", new string[] { "", ex.Message });
            }
            finally
            {
                IsSearching = false;
            }
        }

        /// <summary>
        /// 선호도 상세정보 가져오기
        /// </summary>
        private void getPopupDetail()
        {
            IsSearching = true;
            
            try
            {
                // 조회용 모델설정
                preferenceTotalizeModel.Init();
                preferenceTotalizeModel.KeySearch = "";         // 상세조회시엔 사용안함
                preferenceTotalizeModel.KeyItemNo = keyItemNo;
                preferenceTotalizeModel.KeyNoticeId = keyPopCode;
                preferenceTotalizeModel.KeyExmNo = keyAdvtExmNo;
                preferenceTotalizeModel.KeyStartDay =  keyStartDay.Replace("/","").Substring(2,6);
                preferenceTotalizeModel.KeyEndDay = keyEndDay.Replace("/", "").Substring(2, 6); ;
                
                new PreferenceTotalizeManager(systemModel, commonModel).getPopupDetail(preferenceTotalizeModel);
                                
                if (preferenceTotalizeModel.ResultCD.Equals("0000"))
                {
                    ebQTitle.Text = preferenceTotalizeModel.EventName;
                    ebStartDt.Text = keyStartDay;
                    ebEndDt.Text = keyEndDay;
                    ebPopExpCount.Text = preferenceTotalizeModel.PopExpCount.ToString("##,##0");
                    ebAdExpCount.Text = preferenceTotalizeModel.AdExpCount.ToString("##,##0");
                    ebRepCount.Text = preferenceTotalizeModel.RepCount.ToString("##,##0");
                    ebRepRate.Text = preferenceTotalizeModel.RepRate.ToString("#00.00");
                    ebPopType.Text = preferenceTotalizeModel.PopExpType.ToString();
                    
                    Utility.SetDataTable(preferenceTotalizeDs.PopupDetail, preferenceTotalizeModel.PreferenceDataSet);
                    //grdQList.DataSource = preferenceTotalizeModel.PreferenceDataSet.Tables[0];
                }
                else
                {
                    ebQTitle.Text = "";
                    ebStartDt.Text = "";
                    ebEndDt.Text = "";
                    ebPopExpCount.Text = "";
                    ebAdExpCount.Text = "";
                    ebRepCount.Text = "";
                    ebRepRate.Text = "";
                    ebPopType.Text = "";
                    Utility.SetDataTable(preferenceTotalizeDs.PopupDetail, preferenceTotalizeModel.PreferenceDataSet);
                }
            }
            catch (FrameException fe)
            {
                FrameSystem.showMsgForm("광고목록 리스트 조회오류", new string[] { fe.ErrCode, fe.ResultMsg });
            }
            catch (Exception ex)
            {
                FrameSystem.showMsgForm("광고목록 리스트 조회오류", new string[] { "", ex.Message });
            }
            finally
            {
                IsSearching = false;
            }
        }
       
        /// <summary>
        /// [E_01] 키캆을찾아 그리드 키에 해당되는 로우로 이동 - 광고 목록
        /// </summary>
        private void AddAdChoice()
        {
            int rowIndex = 0;
            if (dt.Rows.Count < 1) return;
            foreach (DataRow row in dt.Rows)
            {
                if (row["itemNo"].ToString().Equals(keyItemNo))
                {
                    cm.Position = rowIndex;
                    break;
                }
                rowIndex++;
            }
            grdAdvList.EnsureVisible();
        }

        /// <summary>
        /// [E_01] 광고 목록 상세정보의 셋트
        /// </summary>
        private void SetDetailText()
        {
            keyItemNo = "";
            keyPopCode = "";
            keyAdvtExmNo = "";

            int curRow = cm.Position;

            if (curRow >= 0)
            {
                if (grdAdvList.Row >= 0 && grdAdvList.RecordCount > 0)
                {
                    keyItemNo = dt.Rows[curRow]["ItemNo"].ToString();
                    keyPopCode = dt.Rows[curRow]["PopCode"].ToString();
                    keyAdvtExmNo = dt.Rows[curRow]["advt_exm_no"].ToString();
                    keyStartDay = dt.Rows[curRow]["StartDt"].ToString();
                    keyEndDay = dt.Rows[curRow]["EndDt"].ToString();

                    getPopupDetail();
                }
            }
        }
        #endregion

    }
}