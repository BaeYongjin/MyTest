using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using WinFramework.Misc;
using WinFramework.Base;
using WinFramework.Data;

using AdManagerModel;

namespace AdManagerClient
{
    public partial class SchExclusiveZoneAd_pAdd : Form
    {
        #region 이벤트핸들러
        public event StatusEventHandler StatusEvent;			// 상태이벤트 핸들러
        public event ProgressEventHandler ProgressEvent;			// 처리중이벤트 핸들러
        #endregion

        #region 사용자정의 객체 및 변수

        //시스템 정보 : 화면 공통
        private SystemModel systemModel = FrameSystem.oSysModel;
        private CommonModel commonModel = FrameSystem.oComModel;
        private Logger log = FrameSystem.oLog;

        //사용할 정보 모델 
        SchExclusiveZoneModel schExclusiveZoneModel = new SchExclusiveZoneModel();

        // 화면처리용 변수
        bool IsNewSearchKey = true;					// 검색어입력 여부
        CurrencyManager cm = null;					// 데이터 그리드의 변경에 따른 데이터셋 관리를 위하여			
        DataTable dt = null;

        bool IsSearching = false; // 조회중 상세화면이 업데이트 되는 것을 방지 하기위함 2011.11.29 JH.Park

        //키데이타
        string keyItemNo = "";
        string keyItemName = "";

        private string setTimes = string.Empty;

        public string ItemNo
        {
            get { return keyItemNo; }
            set { keyItemNo = value; }
        }

        // 주중/주말 값을 정수의 합으로 표현
        private int dayOfNum = 0;

        #endregion


        #region 생성자 , 디자인
        public SchExclusiveZoneAd_pAdd()
        {
            InitializeComponent();
        }
        #endregion

       




        #region [##컨트롤 시작점##]
        private void SchExclusiveZoneAd_pAdd_Load(object sender, EventArgs e)
        {
            // 데이터관리용 객체생성
            dt = ((DataView)grdExScheduleList.DataSource).Table;
            cm = (CurrencyManager)this.BindingContext[grdExScheduleList.DataSource];
            cm.PositionChanged += new EventHandler(OnGrdRowChanged);

            // 컨트롤 초기화
            InitControl();
        }

        
        #endregion

        #region 컨트롤 초기화 처리
        //컨트롤 초기화#
        private void InitControl()
        {
            ProgressStart();

            //초기화 콤보박스
            InitCombo();

            SearchTargeting();

            init_tvTimes();

            ProgressStop();
        }
        #endregion

        #region 드롭 리스트 초기 화
        //콤보 초기 화 
        private void InitCombo()
        {
            Init_MediaCode();   //매체 드롭리스트 
            Init_RapCode();     //미디어 렙 드롭리스트
            Init_AgencyCode();  //대행사 드롭리스트
        }

        //매체 드롭 리스트 
        private void Init_MediaCode()
        {
            // 매체를 조회한다.
            MediaCodeModel mediacodeModel = new MediaCodeModel();
            new MediaCodeManager(systemModel, commonModel).GetMediaCodeList(mediacodeModel);

            if (mediacodeModel.ResultCD.Equals("0000"))
            {
                // 데이터셋에 셋팅
                Utility.SetDataTable(schExclusiveZoneAdDs.Medias, mediacodeModel.MediaCodeDataSet);
            }

            // 검색조건의 콤보
            this.cbSearchMedia.Items.Clear();

            // 콤보박스에 셋트할 코드목록을 담을 Item배열을 선언
            Janus.Windows.EditControls.UIComboBoxItem[] comboItems = new Janus.Windows.EditControls.UIComboBoxItem[mediacodeModel.ResultCnt + 1];

            comboItems[0] = new Janus.Windows.EditControls.UIComboBoxItem("매체선택", "00");

            for (int i = 0; i < mediacodeModel.ResultCnt; i++)
            {
                DataRow row = schExclusiveZoneAdDs.Medias.Rows[i];

                string val = row["MediaCode"].ToString();
                string txt = row["MediaName"].ToString();
                comboItems[i + 1] = new Janus.Windows.EditControls.UIComboBoxItem(txt, val);
            }
            // 콤보에 셋트
            this.cbSearchMedia.Items.AddRange(comboItems);
            this.cbSearchMedia.SelectedIndex = 0;

            Application.DoEvents();
        }
        //미디어 렙 드롭리스트
        private void Init_RapCode()
        {
            // 미디어 랩을 조회한다.
            MediaRapCodeModel mediarapcodeModel = new MediaRapCodeModel();
            new MediaRapCodeManager(systemModel, commonModel).GetMediaRapCodeList(mediarapcodeModel);

            if (mediarapcodeModel.ResultCD.Equals("0000"))
            {
                // 데이터셋에 셋팅
                Utility.SetDataTable(schExclusiveZoneAdDs.MediaRaps, mediarapcodeModel.MediaRapCodeDataSet);
            }

            // 검색조건의 콤보
            this.cbSearchRap.Items.Clear();

            // 콤보박스에 셋트할 코드목록을 담을 Item배열을 선언
            Janus.Windows.EditControls.UIComboBoxItem[] comboItems = new Janus.Windows.EditControls.UIComboBoxItem[mediarapcodeModel.ResultCnt + 1];

            comboItems[0] = new Janus.Windows.EditControls.UIComboBoxItem("미디어렙선택", "00");

            for (int i = 0; i < mediarapcodeModel.ResultCnt; i++)
            {
                DataRow row = schExclusiveZoneAdDs.MediaRaps.Rows[i];

                string val = row["RapCode"].ToString();
                string txt = row["RapName"].ToString();
                comboItems[i + 1] = new Janus.Windows.EditControls.UIComboBoxItem(txt, val);
            }
            // 콤보에 셋트
            this.cbSearchRap.Items.AddRange(comboItems);
            this.cbSearchRap.SelectedIndex = 0;

            Application.DoEvents();
        }
        //대행사 드롭리스트
        private void Init_AgencyCode()
        {
            // 대행사를 조회한다.
            AgencyCodeModel agencycodeModel = new AgencyCodeModel();
            new AgencyCodeManager(systemModel, commonModel).GetAgencyCodeList(agencycodeModel);

            if (agencycodeModel.ResultCD.Equals("0000"))
            {
                // 데이터셋에 셋팅
                Utility.SetDataTable(schExclusiveZoneAdDs.Agencys, agencycodeModel.AgencyCodeDataSet);
            }

            // 검색조건의 콤보
            this.cbSearchAgency.Items.Clear();

            // 콤보박스에 셋트할 코드목록을 담을 Item배열을 선언
            Janus.Windows.EditControls.UIComboBoxItem[] comboItems = new Janus.Windows.EditControls.UIComboBoxItem[agencycodeModel.ResultCnt + 1];

            comboItems[0] = new Janus.Windows.EditControls.UIComboBoxItem("대행사선택", "00");

            for (int i = 0; i < agencycodeModel.ResultCnt; i++)
            {
                DataRow row = schExclusiveZoneAdDs.Agencys.Rows[i];

                string val = row["AgencyCode"].ToString();
                string txt = row["AgencyName"].ToString();
                comboItems[i + 1] = new Janus.Windows.EditControls.UIComboBoxItem(txt, val);
            }

            // 콤보에 셋트
            this.cbSearchAgency.Items.AddRange(comboItems);
            this.cbSearchAgency.SelectedIndex = 0;

            Application.DoEvents();
        }

        #endregion

        #region [버튼 초기화]
        private void InitButton()
        {
            grdExScheduleList.Focus();

            Application.DoEvents();
        }
        #endregion

        #region 사용자 처리 메소드
        private void SearchTargeting()
        {
            IsSearching = true;

            StatusMessage("광고타겟팅 편성현황을 조회합니다.");
            try
            {
                // 데이터모델 초기화
                schExclusiveZoneModel.Init();

                // 데이터모델에 전송할 내용을 셋트한다.
                if (IsNewSearchKey) schExclusiveZoneModel.SearchKey = "";
                else schExclusiveZoneModel.SearchKey = ebSearchKey.Text;

                schExclusiveZoneModel.SearchMediaCode = cbSearchMedia.SelectedItem.Value.ToString();
                schExclusiveZoneModel.SearchRapCode = cbSearchRap.SelectedItem.Value.ToString();
                schExclusiveZoneModel.SearchAgencyCode = cbSearchAgency.SelectedItem.Value.ToString();

                if (chkAdState_20.Checked) schExclusiveZoneModel.SearchchkAdState_20 = "Y";
                if (chkAdState_30.Checked) schExclusiveZoneModel.SearchchkAdState_30 = "Y";
                if (chkAdState_40.Checked) schExclusiveZoneModel.SearchchkAdState_40 = "Y";

                //    // 광고 타겟팅 목록조회 서비스를 호출한다.
                new SchExclusiveZoneAdManager(systemModel, commonModel).GetTargetingList(schExclusiveZoneModel);
                

                if (schExclusiveZoneModel.ResultCD.Equals("0000"))
                {
                    Utility.SetDataTable(schExclusiveZoneAdDs.TargetList, schExclusiveZoneModel.TargetingDataSet);
                    StatusMessage(schExclusiveZoneModel.ResultCnt + "건의 광고 정보가 조회되었습니다.");
                    AddSchChoice();
                    SetDetailText();
                

                    
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
        /// 광고 타겟팅 광고 번호 가져오기
        /// </summary>
        private void SetDetailText()
        {
            int curRow = cm.Position;

            if (curRow >= 0)
            {
                keyItemNo = dt.Rows[curRow]["ItemNo"].ToString();		//광고번호
                keyItemName = dt.Rows[curRow]["ItemName"].ToString();   //광고명
                string samount = dt.Rows[curRow]["TgtAmount"].ToString();

                if (samount != "")
                {
                    ebContractAmt.Text = samount;
                }
                else
                {
                    ebContractAmt.Text = "0";
                }
            }
        }

        /// <summary>
		/// 타겟정보를 읽어온다
		/// </summary>
        private void GetTargetingData()
        {
            int curRow = cm.Position;

			if(curRow >= 0)
			{
				keyItemNo   = dt.Rows[curRow]["ItemNo"].ToString();		//광고번호
                keyItemName = dt.Rows[curRow]["ItemName"].ToString();   //광고명
                string samount =  dt.Rows[curRow]["TgtAmount"].ToString();

                if (samount != "")
                {
                    ebContractAmt.Text = samount;
                }
                else
                {
                    ebContractAmt.Text = "0";
                }
                
            }
        }

        /// <summary>
        /// 시간대 treeView 초기 구성(주중/주말)
        /// </summary>
        private void init_tvTimes()
        {
            int fromTm = 0;
            int toTm = 0;
            string strFrom = "";
            string strTo = "";

            tlvDay.Items.Clear();
            tlvEnd.Items.Clear();

            TreeListViewItem dayItems = null;

            if (dayOfNum == -1) // 주중/주말 구분 없음
            {
                lblWeekDay.Text = "주중-주말 구분없음";
                dayItems = new TreeListViewItem("구분없음", 0);
            }
            else
            {
                lblWeekDay.Text = "주중 시간 설정";
                dayItems = new TreeListViewItem("주중", 0);
            }

            TreeListViewItem endItems = new TreeListViewItem("주말", 0);
            TreeListViewItem timeDay = null;
            TreeListViewItem timeEnd = null;
            dayItems.Tag = "day";
            endItems.Tag = "end";

            for (int i = 0; i < 24; i++)
            {
                fromTm = i;
                toTm = i + 1;

                strFrom = string.Format("{0:0#}", fromTm);
                strTo = string.Format("{0:0#}", toTm);
                // 주중 시간대
                timeDay = new TreeListViewItem(strFrom + "시", 1);
                timeDay.SubItems.Add(strFrom + "-" + strTo);
                timeDay.Tag = strFrom;
                timeDay.Items.SortOrder = System.Windows.Forms.SortOrder.None;
                dayItems.Items.Add(timeDay);
                // 주말 시간대
                timeEnd = new TreeListViewItem(strFrom + "시", 1);
                timeEnd.SubItems.Add(strFrom + "-" + strTo);
                timeEnd.Tag = strFrom;
                timeEnd.Items.SortOrder = System.Windows.Forms.SortOrder.None;
                endItems.Items.Add(timeEnd);
            }

            tlvDay.Items.Add(dayItems);
            tlvEnd.Items.Add(endItems);
            // 모두 확장 된 상태가 되도록
            tlvDay.ExpandAll();
            tlvEnd.ExpandAll();

        }

        /// <summary>
        /// 요일 타겟팅 관련해서 treeview컨트롤 제어
        /// </summary>
        private void enableTreeView()
        {
            if (dayOfNum >= 1 && dayOfNum <= 15)
            {
                // 주중 treeview만 enable
                tlvDay.Enabled = true;
                tlvEnd.Enabled = false;
            }
            else if (dayOfNum == 16 || dayOfNum == 17 || dayOfNum == 33)
            {
                // 주말 treeview만 enable
                tlvDay.Enabled = false;
                tlvEnd.Enabled = true;
            }
            else if (dayOfNum == -1)
            {
                // 주중,주말 구분 없음
                // 그래서 하나의 컨트롤만 사용가능하게 함.
                tlvDay.Enabled = true;
                tlvEnd.Enabled = false;
            }
            else if(dayOfNum == 0)
            {
                tlvDay.Enabled = true;
                tlvEnd.Enabled = true;
            }
        }

        /// <summary>
        /// 타겟 시간 문자열 구성
        /// </summary>
        private void compositTimes()
        {
            string dayFix = "d";
            string middleFix = "-";
            string endFix = "e";
            string strDay = "";
            string strEnd = "";

            if (dayOfNum == -1) // 주중,주말 구분 없이..
            {
                strDay = compositDetail(WeekDayOfEnd.dayNend);
                //최종 타겟 시간 문자열 구성
                if (strDay.Length >= 2)
                    setTimes = strDay;// 예상(00^14)
                else
                    setTimes = "";
            }
            else if (dayOfNum >= 1 && dayOfNum <= 15) // 주중
            {
                strDay = compositDetail(WeekDayOfEnd.day);
                //최종 타겟 시간 문자열 구성
                if (strDay.Length >= 2)
                    setTimes = dayFix + strDay;// 예상(d00^14)
                else
                    setTimes = "";
            }
            else if (dayOfNum == 16 || dayOfNum == 17 || dayOfNum == 33) //주말
            {
                strEnd = compositDetail(WeekDayOfEnd.end);
                //최종 타겟 시간 문자열 구성
                if (strEnd.Length >= 2)
                    setTimes = endFix + strEnd;// 예상(e00^14)
                else
                    setTimes = "";
            }
            else
            {
                // 주중,주말 혼합형				
                strDay = compositDetail(WeekDayOfEnd.day);
                strEnd = compositDetail(WeekDayOfEnd.end);

                //최종 타겟 시간 문자열 구성
                if (strDay.Length >= 2 || strEnd.Length >= 2) // 적어도 하나의 시간 값과 앞 구분자는 3자리이므로
                    setTimes = dayFix + strDay + middleFix + endFix + strEnd;// 예상(d00^14-e01^23)
                else
                    setTimes = "";
            }
        }

        /// <summary>
        /// 타겟 시간 문자열 구성 상세
        /// </summary>
        /// <param name="weekType"></param>
        /// <returns></returns>
        private string compositDetail(WeekDayOfEnd weekType)
        {
            string strDay = "";

            if (weekType == WeekDayOfEnd.day)
            {
                for (int i = 0; i < tlvDay.Items[0].Items.Count; i++)
                {
                    TreeListViewItem tlv = tlvDay.Items[0].Items[i];
                    if (tlv.Checked)
                        strDay += tlv.Tag.ToString() + "^";
                }
                if (strDay.Length > 1) // 체크 값 길이 검증
                    strDay = strDay.Substring(0, strDay.Length - 1); // 마지막 구분자 제거
            }
            else if (weekType == WeekDayOfEnd.end)
            {
                // 주말 처리
                for (int i = 0; i < tlvEnd.Items[0].Items.Count; i++)
                {
                    TreeListViewItem tlv = tlvEnd.Items[0].Items[i];
                    if (tlv.Checked)
                        strDay += tlv.Tag.ToString() + "^";
                }

                if (strDay.Length > 1) // 체크 값 길이 검증
                    strDay = strDay.Substring(0, strDay.Length - 1); // 마지막 구분자 제거
            }
            else if (weekType == WeekDayOfEnd.dayNend)
            {
                // 주중,주말 구분 없이
                // 컨트롤은 주중 컨트롤과 같이 사용하므로..enableTreeView 참고
                for (int i = 0; i < tlvDay.Items[0].Items.Count; i++)
                {
                    TreeListViewItem tlv = tlvDay.Items[0].Items[i];
                    if (tlv.Checked)
                        strDay += tlv.Tag.ToString() + "^";
                }
                if (strDay.Length > 1) // 체크 값 길이 검증
                    strDay = strDay.Substring(0, strDay.Length - 1); // 마지막 구분자 제거
            }
            return strDay;
        }

        #endregion

        #region 사용자 등록 이벤트 
        /// <summary>
        /// 그리드의 Row변경시 새로운 광고의 타겟정보을 읽어온다
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnGrdRowChanged(object sender, System.EventArgs e)
        {
            if (!IsSearching) // 2011.11.29 JH.Park 조회중이 아닐경우에만 동작하도록 변경
            {
                GetTargetingData();
                
                InitButton();
            }
        }
        //텍스트체인지 이벤트 
        private void ebSearchKey_TextChanged(object sender, EventArgs e)
        {
            IsNewSearchKey = false;
        }
        //키다운 이벤트
        private void ebSearchKey_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SearchTargeting();
            }
        }
        //텍스트 클릭 이벤트
        private void ebSearchKey_Click(object sender, EventArgs e)
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
        //버튼 클릭 이벤트
        private void btnSearch_Click(object sender, EventArgs e)
        {   
            SearchTargeting();
            InitButton();
        }
        
        //취소버튼 이벤트 
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        //저장 버튼 클릭 이벤트 
        private void btnConfirm_Click(object sender, EventArgs e)
        {
            compositTimes();
            //MessageBox.Show(setTimes,keyItemNo;

            if (setTimes == "" || setTimes.Length == 0)
            {
                DialogResult result = MessageBox.Show("["+keyItemNo+"]"+keyItemName+" 의 시간 설정이 된 데이터가 없습니다!\n 시간대 독점 편성 설정이 되지 않습니다. 그래도 닫겠습니까?"
                    , "시간대 독점 편성", MessageBoxButtons.YesNo, MessageBoxIcon.Question
                    , MessageBoxDefaultButton.Button2);
                if (result == DialogResult.Yes)
                {
                    this.DialogResult = DialogResult.Yes;
                    this.Close();
                }
            }
            else
            {
                DialogResult result = MessageBox.Show("[" + keyItemNo + "]" + keyItemName + " 의 시간대 독점 편성 설정된 시간으로 저장하겠습니까?"
                    , "시간대 독점 편성", MessageBoxButtons.YesNo, MessageBoxIcon.Question
                    , MessageBoxDefaultButton.Button2);
                if (result == DialogResult.Yes)
                {
                    //이부분에서 설정 저장 액션을 발생시킨다 구현 예정임. 
                    SetTargetingDetailAdd();
                    
                    this.DialogResult = DialogResult.Yes;
                    this.Close();
                }
            }
            
        }


        /// <summary>
        /// 광고 타겟팅 내역상세정보 저장
        /// </summary>
        private void SetTargetingDetailAdd()
        {
            StatusMessage("타겟팅 정보를 저장합니다.");
            try
            {
                //저장 전에 모델을 초기화 해준다.
                schExclusiveZoneModel.Init();

                //광고번호
                schExclusiveZoneModel.ItemNo = keyItemNo;
                schExclusiveZoneModel.ItemName = keyItemName;

                // 광고물량
                schExclusiveZoneModel.ContractAmt = ebContractAmt.Text.Replace(",", "");

                //시간대
                schExclusiveZoneModel.TgtTime = setTimes;


                // 타겟팅 상세정보 저장 서비스를 호출한다.
                new SchExclusiveZoneAdManager(systemModel, commonModel).SetTargetingDetailUpdate(schExclusiveZoneModel);

                if (schExclusiveZoneModel.ResultCD.Equals("0000"))
                {

                    StatusMessage("타겟팅 정보가 저장되었습니다.");
                }
            }
            catch(FrameException fe)
            {
                FrameSystem.showMsgForm("타겟팅내역 저장오류", new string[] {fe.ErrCode, fe.ResultMsg});
            }
            catch(Exception ex)
            {
                FrameSystem.showMsgForm("타겟팅내역 저장오류",new string[] {"",ex.Message});
            }

        }





        //분리 체크 박스 
        private void rbWeek_Click(object sender, EventArgs e)
        {
            //getTargetTimes(0);

            dayOfNum = 0;

            init_tvTimes();
            enableTreeView();
        }
        //주중 체크 박스 
        private void rbWeekDay_Click(object sender, EventArgs e)
        {
            dayOfNum = 1;
            init_tvTimes();
            enableTreeView();
        }
        //주말 체크 박스 
        private void rbWeekEnd_Click(object sender, EventArgs e)
        {
            dayOfNum = 16;
            init_tvTimes();
            enableTreeView();
        }
        //없음 체크 박스
        private void rbWeekAll_Click(object sender, EventArgs e)
        {
            dayOfNum = -1;
            init_tvTimes();
            enableTreeView();
        }

        #endregion


        #region 이벤트 핸들러 함수 (상태, 프로그레스)

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

       
        
    }
}
