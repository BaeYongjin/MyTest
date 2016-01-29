using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;

using WinFramework.Misc;
using WinFramework.Base;
using WinFramework.Data;

using AdManagerModel;

namespace AdManagerClient
{
    /// <summary>
    /// HomeGroupPopup에 대한 요약 설명입니다.
    /// </summary>
    public partial class HomeGroupPopup : System.Windows.Forms.Form
    {
        #region 호출자에게 새로고침 이벤트 전달하기 위해서...[E_01]
        public delegate void RefreshDelegate();
        public event RefreshDelegate OnRefreshParent;
        #endregion

        public HomeGroupPopup()
        {
            InitializeComponent();
        }

        #region 이벤트핸들러
        public event StatusEventHandler StatusEvent;			// 상태이벤트 핸들러
        public event ProgressEventHandler ProgressEvent;			// 처리중이벤트 핸들러
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

        #region 사용자정의 객체 및 변수

        // 시스템 정보 : 화면공통
        private SystemModel systemModel = FrameSystem.oSysModel;
        private CommonModel commonModel = FrameSystem.oComModel;
        private Logger log = FrameSystem.oLog;

        // 사용할 정보모델
        SchHomeGroupModel schHomeGroupModel = new SchHomeGroupModel();

        // 화면처리용 변수
        CurrencyManager cm = null;					// 데이터 그리드의 변경에 따른 데이터셋 관리를 위하여			
        CurrencyManager cmdetail = null;
        DataTable dt = null;
        DataTable dtDetail = null;
        
        //DataTable dtSchGroup = null; //수정모드일 때 사용

        // 매체
        public string keyGroupCode = "";
        public string keyWeek = "";
        public string keyTime = "";
        
        /// <summary>
        /// 등록모드이면 true,수정모드이면 false
        /// </summary>
        private bool isAdd = true;

        /// <summary>
        /// 선택 된 그룹명
        /// </summary>
        public string KeyGroupName { set; get; }

        // 멀티셀렉트된 시간값
        public string TimeData1 = "";
        public string TimeData2 = "";

        public string Week { set; get; }   // 주중/주말 체크
        public string WeekDay { set; get; } // 요일 체크

        #endregion

        #region 사용자 액션처리 메소드

        private void HomeGroupPopup_Load(object sender, EventArgs e)
        {
            // 데이터관리용 객체생성
            dt = ((DataView)grdGroupList.DataSource).Table;
            dtDetail = ((DataView)grdGroupDetail.DataSource).Table;

            cm = (CurrencyManager)this.BindingContext[grdGroupList.DataSource];
            cmdetail = (CurrencyManager)this.BindingContext[grdGroupDetail.DataSource];
            
            GetSchHomeGroupList();
            bindingTime();
            bindingWeek();

            // parent 에서 특정 그룹명이나 그룹코드 값이 넘어온 경우에는 수정 환경임.
            if (KeyGroupName != null && KeyGroupName.Length > 0)
            {
                isAdd = false; // 수정모드
                btnAdd.Text = "수정";
                getSelectedSchHomeGroup(); // 특정 그룹의 스케줄링 정보만 조회(수정 모드일 때는 초기에 실행)
            }
        }

        /// <summary>
        /// 추가버튼 클릭시
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                SetSchHomeCreate();
                //this.Close(); why?
                OnRefreshParent();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// 취소버튼 클릭시
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void grdGroupList_SelectionChanged(object sender, EventArgs e)
        {
            int curRow = cm.Position;

            if (curRow >= 0)
            {
                try
                {
                    keyGroupCode = dt.Rows[curRow]["GroupCode"].ToString();
                    GetSchHomeGroupDetailList();                    
                }
                catch
                {
                }
            }
        }

        #endregion

        /// <summary>
        /// 그룹목록 조회
        /// </summary>
        private void GetSchHomeGroupList()
        {
            try
            {
                schHomeGroupModel.GroupCode = keyGroupCode;
                schHomeGroupModel.GroupName = KeyGroupName;
                // 목록조회 서비스를 호출한다.
                new SchHomeGroupManager(systemModel, commonModel).GetSchHomeGroupList(schHomeGroupModel);

                if (schHomeGroupModel.ResultCD.Equals("0000"))
                {
                    Utility.SetDataTable(schHomeGroupDs1.GroupList, schHomeGroupModel.SchHomeGroupModelDataSet);
                    StatusMessage(schHomeGroupModel.ResultCnt + "건의  홈광고 편성대상 광고목록이 조회되었습니다.");
                }
            }
            catch (FrameException fe)
            {
                FrameSystem.showMsgForm("광고목록 조회오류", new string[] { fe.ErrCode, fe.ResultMsg });
            }
            catch (Exception ex)
            {
                FrameSystem.showMsgForm("광고목록 조회오류", new string[] { "", ex.Message });
            }
        }

        /// <summary>
        /// 특정 홈그룹의 편성정보만 조회
        /// 요일과 시간에 체킹 기능을 위해서 추가 함
        /// 수정모드는 불필요 주석처리
        /// </summary>
        private void getSelectedSchHomeGroup()
        {
            /*
            try
            {
                SchHomeGroupModel model = new SchHomeGroupModel();
                model.GroupCode = keyGroupCode;

                // 광고파일목록조회 서비스를 호출한다.
                new SchHomeGroupManager(systemModel, commonModel).GetSchHomeList(model);

                if (schHomeGroupModel.ResultCD.Equals("0000"))
                {
                    if (dtSchGroup != null)
                        dtSchGroup.Clear();

                    dtSchGroup = model.SchHomeGroupModelDataSet.Tables[0].Copy();
                    bindingWeek();
                }
                
            }
            catch (FrameException fe)
            {
                FrameSystem.showMsgForm("홈광고 편성현황 조회오류", new string[] { fe.ErrCode, fe.ResultMsg });
            }
            catch (Exception ex)
            {
                FrameSystem.showMsgForm("홈광고 편성현황 조회오류", new string[] { "", ex.Message });
            }
            finally
            {
                
            }
            */
        }

        /// <summary>
        /// 특정그룹의 요일별 편성 유무 파악해서 요일 체킹박스에 체킹(수정모드에서만)
        /// 수정 모드 불필요해서 필요 없을 듯...
        /// 주석처리...수정모드..
        /// </summary>
        private void bindingWeekNO()
        {
            /*
            if (isAdd == false)
            {
                if (dtSchGroup == null || dtSchGroup.Rows.Count == 0) return;
                string query = "";

                foreach (Control ctrl in gbDays.Controls)
                {
                    Janus.Windows.EditControls.UICheckBox cb = (Janus.Windows.EditControls.UICheckBox)ctrl;
                    query = "TgtWeek =" + cb.Tag.ToString();
                    DataRow[] rows = dtSchGroup.Select(query);
                    if (null != rows && rows.Length > 0)
                        cb.Checked = true;
                }
            }
            */
        }

        /// <summary>
        /// 그룹별 광고목록 조회
        /// </summary>
        private void GetSchHomeGroupDetailList()
        {
            int curRow = cm.Position;

            try
            {
                schHomeGroupModel.GroupCode = dt.Rows[curRow]["GroupCode"].ToString();

                // 목록조회 서비스를 호출한다.
                new SchHomeGroupManager(systemModel, commonModel).GetSchHomeGroupDetailList(schHomeGroupModel);

                if (schHomeGroupModel.ResultCD.Equals("0000"))
                {
                    grdGroupDetail.DataSource = schHomeGroupModel.SchHomeGroupModelDataSet.Tables[0];
                    StatusMessage(schHomeGroupModel.ResultCnt + "건의  홈광고 편성대상 광고목록이 조회되었습니다.");
                }
            }
            catch (FrameException fe)
            {
                FrameSystem.showMsgForm("광고목록 조회오류", new string[] { fe.ErrCode, fe.ResultMsg });
            }
            catch (Exception ex)
            {
                FrameSystem.showMsgForm("광고목록 조회오류", new string[] { "", ex.Message });
            }
        }

        /// <summary>
        /// 홈광고편성추가
        /// </summary>
        private void SetSchHomeCreate()
        {
            string week = "";
            string times = "";

            try
            {
                int curRow = cm.Position;
                
                // 체크 값 구성및 검증
                week = getWeekDay();
                times = getTimes();
                
                if (week.Length <= 0)
                    throw new Exception("요일 정보를 선택하세요!");
                if (times.Length <= 0)
                    throw new Exception("시간 정보를 선택하세요!");

                schHomeGroupModel.Init();
                schHomeGroupModel.GroupCode = keyGroupCode;
                schHomeGroupModel.TgtWeek = week;
                schHomeGroupModel.TgtTime = times;
                new SchHomeGroupManager(systemModel, commonModel).SetSchHomeCreate(schHomeGroupModel);                               
                MessageBox.Show("그룹 편성 추가 완료했습니다.");
                // parent 새로 고침

                //for(int i=0; i < ; i++) // 체크한 요일
                //{
                //    for(int j = 0; j < ; j++)   // 체크한 시간
                //    {
                //        schHomeGroupModel.GroupCode = dt.Rows[curRow]["GroupCode"].ToString();
                //        schHomeGroupModel.TgtWeek = ebGroupName.Text;
                //        schHomeGroupModel.TgtTime = ebComment.Text;

                //        // 추가 
                //        new SchHomeGroupManager(systemModel, commonModel).SetSchHomeCreate(schHomeGroupModel);
                //    }
                //}
            }
            catch (FrameException fe)
            {
                FrameSystem.showMsgForm("홈광고편성추가 저장오류", new string[] { fe.ErrCode, fe.ResultMsg });
            }
            catch (Exception ex)
            {
                FrameSystem.showMsgForm("홈광고편성추가 저장오류", new string[] { "", ex.Message });
            }		
        }

        /// <summary>
        /// 요일 체크 값 문자열로 합성
        /// </summary>
        /// <returns></returns>
        private string getWeekDay()
        {
            string week = "";
            foreach (Control ctrl in gbDays.Controls)
            {
                Janus.Windows.EditControls.UICheckBox cb = (Janus.Windows.EditControls.UICheckBox)ctrl;
                if(cb.Checked)
                    week += "'"+ cb.Tag.ToString() + "',";
            }

            if (week.Length > 3)
                week = week.Substring(0, week.Length - 1); // 마지막 ,문자 제외
            
            return week;
        }

        /// <summary>
        /// 시간 체크 값 문자열로 합성
        /// </summary>
        /// <returns></returns>
        private string getTimes()
        {
            string times = "";
            foreach (Control ctrl in grpTime.Controls)
            {
                Janus.Windows.EditControls.UICheckBox cb = (Janus.Windows.EditControls.UICheckBox)ctrl;
                if (cb.Checked)
                    times += "'" + cb.Tag.ToString() + "',";
            }

            if (times.Length > 4)
                times = times.Substring(0, times.Length - 1); // 마지막,문자 제외
            
            return times;
        }

        /// <summary>
        /// 시간 바인딩
        /// </summary>
        private void bindingTime()
        {
            if (TimeData1.Length >= 4)
            {
                Regex reg = new Regex(",");
                string[] regtime = reg.Split(TimeData1);

                string subregtime = "";

                for (int i = 0; i < regtime.Length; i++)
                {
                    subregtime = regtime[i].Replace("'", "");
                    foreach (Janus.Windows.EditControls.UICheckBox ctrl in grpTime.Controls)
                    {
                        if (ctrl.Tag.ToString() == subregtime)
                            ctrl.Checked = true;
                    }
                }
            }
            else if (TimeData2.Length >= 4)
            {
                Regex reg = new Regex(",");
                string[] regtime = reg.Split(TimeData2);

                string subregtime = "";

                for (int i = 0; i < regtime.Length; i++)
                {
                    subregtime = regtime[i].Replace("'", "");
                    foreach (Janus.Windows.EditControls.UICheckBox ctrl in grpTime.Controls)
                    {
                        if (ctrl.Tag.ToString() == subregtime)
                            ctrl.Checked = true;
                    }
                }
            }
        }

        /// <summary>
        /// 요일 바인딩
        /// </summary>
        private void bindingWeek()
        {
            if (TimeData1.Length >= 4 || Week == "1") //주중
            {
                foreach (Janus.Windows.EditControls.UICheckBox ctrl in gbDays.Controls)
                {
                    if (Convert.ToInt32(ctrl.Tag) < 6)
                    {
                        if (WeekDay != null && WeekDay.Length > 0)
                        {
                            if (WeekDay == ctrl.Tag.ToString())
                            {
                                ctrl.Checked = true;
                                return;
                            }
                        }
                        else
                        {
                            ctrl.Checked = true;
                        }
                    }
                }
            }
            else if (TimeData2.Length >= 4 || Week== "2") //주말
            {
                foreach (Janus.Windows.EditControls.UICheckBox ctrl in gbDays.Controls)
                {
                    if (Convert.ToInt32(ctrl.Tag) > 5)
                    {
                        if (WeekDay != null && WeekDay.Length > 0)
                        {
                            if(WeekDay == ctrl.Tag.ToString())
                            {
                                ctrl.Checked = true;
                                return;
                            }
                        }
                        else
                        {
                            ctrl.Checked = true;
                        }
                    }
                }
            }
        }
        
        /// <summary>
        /// 시간 체크박스 전체 선택/비선택 기능 추가
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chkTime_CheckedChanged(object sender, EventArgs e)
        {
            foreach (Control ctrl in grpTime.Controls)
            {
                Janus.Windows.EditControls.UICheckBox cb = (Janus.Windows.EditControls.UICheckBox)ctrl;
                cb.Checked = chkTime.Checked;
            }
        }
    
    }
}
