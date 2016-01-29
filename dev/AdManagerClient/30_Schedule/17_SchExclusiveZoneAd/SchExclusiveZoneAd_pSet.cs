using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using WinFramework.Misc;
using WinFramework.Base;
using WinFramework.Data;

using AdManagerModel;

namespace AdManagerClient
{
    public partial class SchExclusiveZoneAd_pSet : Form
    {
        #region 이벤트핸들러
        public event StatusEventHandler StatusEvent;			// 상태이벤트 핸들러
        public event ProgressEventHandler ProgressEvent;
        #endregion

        #region 사용자 정의 객체 및 변수
        // 시스템 정보 : 화면공통
        private SystemModel systemModel = FrameSystem.oSysModel;
        private CommonModel commonModel = FrameSystem.oComModel;
        private Logger log = FrameSystem.oLog;

        //부모로 부터 넘어오는 값
        private string keyItem = "";

        public string KeyItem
        {
            get { return keyItem; }
            set { keyItem = value; }
        }

        private string itemNo = "";     //광고번호
        private string itemName = "";   //광고명


        //주중/주말 구분 값 
        private int dayOfNum = 0;

        //사용할 정보 모델 
        SchExclusiveZoneModel schExclusiveZoneModel = new SchExclusiveZoneModel();

        
        //지정된 시간 설정값
        private string strTimes = string.Empty;

        //설정되 시간값
        private string setTimes = string.Empty;

        string setMon = "";
        string setThu = "";
        string setWed = "";
        string setThe = "";
        string setFri = "";
        string setSat = "";
        string setSun = "";
        
        #endregion
        

        #region 생성자, 디자인

        public SchExclusiveZoneAd_pSet()
        {
            InitializeComponent();
        }
        
        #endregion

        #region 폼 로드 => 시작점
        private void SchExclusiveZoneAd_pSet_Load(object sender, EventArgs e)
        {
            string[] items = KeyItem.Split('_');

            InitControl();

            if (items.Length > 0)
            {
                itemNo = items[0].ToString();
                itemName = items[1].ToString();
                //시간대 , 요일 타겟 정보 가지고 오기 
                lbl_AdName.Text = "[" + itemNo + "] " + itemName;
                
                //선택한 시간대 독점 광고 함수 불러 오기 
                TimeZonTargetingInfo(); //<= 이부분 수정 예정 정보 값에 따라 요일, 시간대 설정 뿌릴 예정임.
            }

        }
        #endregion

        #region 컨트롤 초기화 처리
        private void InitControl()
        {
            ProgressStart();

            init_tvTimes();

            ProgressStop();
        }
        #endregion

        #region 시간대, 요일 타겟 정보 가지고 오기
        private void TimeZonTargetingInfo()
        {
            //모델 초기화 
            schExclusiveZoneModel.Init();
            //조건값
            schExclusiveZoneModel.ItemNo = itemNo;

            // 타겟팅 상세조회 서비스를 호출한다.
            new SchExclusiveZoneAdManager(systemModel, commonModel).GetTimeTargetDetail(schExclusiveZoneModel);

            if (schExclusiveZoneModel.ResultCD.Equals("0000") && schExclusiveZoneModel.ResultCnt > 0)
            {
                
                Utility.SetDataTable(schExclusiveZoneAdDs.SchExclusiveDetail, schExclusiveZoneModel.SchExDetailDataSet);
                DataRow row = schExclusiveZoneAdDs.SchExclusiveDetail.Rows[0];

                //MessageBox.Show(row["ItemNo"].ToString());
                //MessageBox.Show(row["tgtTimeYn"].ToString());
                //MessageBox.Show(row["TgtTime"].ToString());
                //MessageBox.Show(row["TgtWeekYn"].ToString());

                //값에 따라 뿌려 주는 제어 하는 작업을 한다. 

                #region 요일 정보 설정
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

                #region 시간정보설정 
                // 노출시간대
                // 요일 정보보다 후에 체크
                if (row["TgtTimeYn"].ToString().Equals("Y"))
                {
                    chkTimeYn.Checked = true;
                    ugbTime.Enabled = true;
                    strTimes = row["TgtTime"].ToString();
                    setTimesBinding();
                    //값 바인딩 처리 해야 함. 
                    filteringDaynEnd();
                }
                else
                {
                    chkTimeYn.Checked = false;
                    ugbTime.Enabled = false;
                }
                #endregion

            }
 
        }
        #endregion


        #region 사용자 처리 함수 
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

        //요일별 체크 
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

        //시간 구분 체크 
        private void RadioWeekChecked()
        {
            rbWeek.Checked = false;
            rbWeekDay.Checked = false;
            rbWeekEnd.Checked = false;
            rbWeekAll.Checked = false;
        }

        
        // 요일 타겟팅 관련해서 treeview컨트롤 제어
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
            else if (dayOfNum == 0)
            {
                tlvDay.Enabled = true;
                tlvEnd.Enabled = true;
            }
        }

        
        // 요일이 체크 된 상태에서 주중/주말 값 숫자로 얻어오기
        private int getDayOfNum()
        {
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

        private void setTimesBinding()
        {
            
            dayOfNum = getDayOfNum();

            if (strTimes != null && strTimes.Length > 3)
            {
                string[] weeks = Utility.SplitByString(strTimes, "-");

                if (weeks != null && weeks.Length > 1)
                {
                    rbWeek.Checked = true;	// 주중, 주말 분리
                    dayOfNum = 0;
                    enableTreeView();
                }
                else
                {
                    if (strTimes.Substring(0, 1) == "d") // 주중											
                        rbWeekDay.Checked = true;
                    else if (strTimes.Substring(0, 1) == "e") // 주말					
                        rbWeekEnd.Checked = true;
                    else
                    {
                        string[] splits = strTimes.Split('^');

                        //**과거에 등록된 데이터 처리**//
                        // 요일별 체크 항목이 있다면 주중/주말 구분해서 처리하고
                        // 없다면 혼합 모드로 처리 한다.
                        if (dayOfNum >= 1 && dayOfNum <= 15)
                        {
                            rbWeekDay.Checked = true; // 주중
                            dayOfNum = 1;
                            enableTreeView();
                        }
                        else if (dayOfNum == 16 || dayOfNum == 18 || dayOfNum == 34)
                        {
                            rbWeekEnd.Checked = true; // 주말
                            dayOfNum = 16;
                            enableTreeView();
                        }
                        else
                        {
                            rbWeekAll.Checked = true; // 주중, 주말 공통(주중,주말 관계 없다는 의미)
                            dayOfNum = -1;
                            enableTreeView();
                        }																																		
                    }
                }
            }
        }

        /// <summary>
        /// 시간 데이터들의 필터링-주중/주말/혼합 구분처리
        /// </summary>
        private void filteringDaynEnd()
        {
            string[] weeks = strTimes.Split('-');
            if (weeks != null && weeks.Length > 1)
            {
                // 주중, 주말 분리
                string[] splits_d = weeks[0].Substring(1, weeks[0].Length - 1).Split('^');
                string[] splits_e = weeks[1].Substring(1, weeks[1].Length - 1).Split('^');
                checkingTreeView(splits_d, WeekDayOfEnd.day);
                checkingTreeView(splits_e, WeekDayOfEnd.end);
            }
            else
            {
                if (strTimes.Substring(0, 1) == "d") // 주중
                {
                    string[] splits = strTimes.Substring(1, strTimes.Length - 1).Split('^');
                    checkingTreeView(splits, WeekDayOfEnd.day);
                }
                else if (strTimes.Substring(0, 1) == "e") // 주말
                {
                    string[] splits = strTimes.Substring(1, strTimes.Length - 1).Split('^');
                    checkingTreeView(splits, WeekDayOfEnd.end);
                }
                else
                {
                    string[] splits = strTimes.Split('^');

                    //**과거에 등록된 데이터 처리**//
                    // 요일별 체크 항목이 있다면 주중/주말 구분해서 처리하고
                    // 없다면 혼합 모드로 처리 한다.
                    if (dayOfNum >= 1 && dayOfNum <= 15)
                    {
                        checkingTreeView(splits, WeekDayOfEnd.day); // 주중
                    }
                    else if (dayOfNum == 16 || dayOfNum == 18 || dayOfNum == 34)
                    {
                        checkingTreeView(splits, WeekDayOfEnd.end); // 주말
                    }
                    else
                    {
                        // 주중, 주말 공통(주중,주말 관계 없다는 의미)
                        // 그래서 하나의 컨트롤만 enable
                        checkingTreeView(splits, WeekDayOfEnd.dayNend);
                        //checkingTreeView(splits, WeekDayOfEnd.end);
                    }
                }
            }
        }

        /// <summary>
        /// 시간 값을 treeview에 바인딩(check-in n out)
        /// </summary>
        /// <param name="chkList">시간 값</param>
        /// <param name="weekType">주중/주말구분</param>
        private void checkingTreeView(string[] chkList, WeekDayOfEnd weekType)
        {
            if (weekType == WeekDayOfEnd.day) // 주중
            {
                for (int i = 0; i < chkList.Length; i++)
                {
                    for (int j = 0; j < tlvDay.Items[0].Items.Count; j++)
                    {
                        TreeListViewItem item = tlvDay.Items[0].Items[j];

                        if (item.Tag.ToString() == chkList[i])
                        {
                            item.Checked = true;
                            break;
                        }
                    }
                }
            }
            else if (weekType == WeekDayOfEnd.end) // 주말
            {
                for (int i = 0; i < chkList.Length; i++)
                {
                    for (int j = 0; j < tlvEnd.Items[0].Items.Count; j++)
                    {
                        TreeListViewItem item = tlvEnd.Items[0].Items[j];

                        if (item.Tag.ToString() == chkList[i])
                        {
                            item.Checked = true;
                            break;
                        }
                    }
                }
            }
            else if (weekType == WeekDayOfEnd.dayNend) // 주중, 주말 구분 없음.
            {
                
                for (int i = 0; i < chkList.Length; i++)
                {
                    for (int j = 0; j < tlvDay.Items[0].Items.Count; j++)
                    {
                        TreeListViewItem item = tlvDay.Items[0].Items[j];

                        if (item.Tag.ToString() == chkList[i])
                        {
                            item.Checked = true;
                            break;
                        }
                    }
                }
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
            else if (dayOfNum == 16 || dayOfNum == 18 || dayOfNum == 34) //주말
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


        #region [핸들러에 의한 이벤트함수]
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

        #region 사용자 이벤트 처리 

        //저장 클릭 이벤트 
        private void btnConfirm_Click(object sender, EventArgs e)
        {
            compositTimes();

            //MessageBox.Show(setTimes);


            if (chkTimeYn.Checked)
            {
                if (setTimes == "" || setTimes.Length == 0)
                {
                    DialogResult result = MessageBox.Show("시간대 독점 편성 시간 설정이 된 데이터가 없습니다!\n 시간대 독점 편성 설정이 되지 않습니다. 그래도 닫겠습니까?"
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
                    DialogResult result = MessageBox.Show(" 시간대 독점 편성에 설정된 시간으로 저장하겠습니까?"
                        , "시간대 독점 편성", MessageBoxButtons.YesNo, MessageBoxIcon.Question
                        , MessageBoxDefaultButton.Button2);
                    if (result == DialogResult.Yes)
                    {

                        SetSchExclusivUpdate();
                        this.DialogResult = DialogResult.Yes;
                        this.Close();
                    }
                }
            }
            else
            {
                DialogResult result = MessageBox.Show(" 시간대 선택을 취소하여 시간대 독점 편성을 하지 않습니다. 그래도 설정하겠습니까?"
                        , "시간대 독점 편성", MessageBoxButtons.YesNo, MessageBoxIcon.Question
                        , MessageBoxDefaultButton.Button2);

                if (result == DialogResult.Yes)
                {
                    SetSchExclusivUpdate();
                    this.DialogResult = DialogResult.Yes;
                    this.Close();
                }
            }
            

        }


        /// <summary>
        /// 광고 타겟팅 내역상세정보 저장
        /// </summary>
        private void SetSchExclusivUpdate()
        {
            StatusMessage("선택된 시간대 독점 편성 정보를 저장합니다.");
            try
            {
                //저장 전에 모델을 초기화 해준다.
                schExclusiveZoneModel.Init();

                //광고번호
                schExclusiveZoneModel.ItemNo = itemNo;

                #region 시간대 독점 설정
                if (chkTimeYn.Checked)
                {
                    schExclusiveZoneModel.TgtTimeYn = "Y";
                    schExclusiveZoneModel.TgtTime = setTimes;
                }
                else
                {
                    schExclusiveZoneModel.TgtTimeYn = "N";
                    schExclusiveZoneModel.TgtTime = "";
                }
                #endregion

                #region 요일별 설정
                if (chkWeekYn.Checked)
                {
                    schExclusiveZoneModel.TgtWeekYn = "Y";
                    if (chkSun.Checked)
                    {
                        setSun = "1^";
                    }
                    else
                    { setSun = ""; }
                    if (chkMon.Checked)
                    {
                        //'^'는 한 필드에 여러개의 체크박스값들을 입력하기 때문에 구분을 위해 사용..
                        setMon = "2^";
                    }
                    else
                    { setMon = ""; }
                    if (chkThu.Checked)
                    {
                        setThu = "3^";
                    }
                    else
                    { setThu = ""; }
                    if (chkWed.Checked)
                    {
                        setWed = "4^";
                    }
                    else
                    { setWed = ""; }
                    if (chkThe.Checked)
                    {
                        setThe = "5^";
                    }
                    else
                    { setThe = ""; }
                    if (chkFri.Checked)
                    {
                        setFri = "6^";
                    }
                    else
                    { setFri = ""; }
                    if (chkSat.Checked)
                    {
                        setSat = "7^";
                    }
                    else
                    { setSat = ""; }
                }
                else
                {
                    schExclusiveZoneModel.TgtWeekYn = "N";
                    setSun = "";
                    setMon = "";
                    setThu = "";
                    setWed = "";
                    setThe = "";
                    setFri = "";
                    setSat = "";
                }
                //체크박스의 값들을 string으로 결합하여 Model에 담음..
                schExclusiveZoneModel.TgtWeek = setSun.ToString() + setMon.ToString() + setThu.ToString() + setWed.ToString() + setThe.ToString() + setFri.ToString() + setSat.ToString();

                //Model에 담긴 값을 Substring으로 마지막 한자리를 자른다..이유는 하나의 값뒤에 붙어있는 '^'를 제거하기 위해..
                //ex)DB에 저장된 마지막 값에는 항상'^'붙어있기 때문에..
                if (schExclusiveZoneModel.TgtWeekYn.Equals("Y"))
                {
                    schExclusiveZoneModel.TgtWeek = schExclusiveZoneModel.TgtWeek.Substring(0, schExclusiveZoneModel.TgtWeek.Length - 1);
                }
                #endregion


                // 타겟팅 상세정보 저장 서비스를 호출한다.
                new SchExclusiveZoneAdManager(systemModel, commonModel).SetSchExclusivUpdate(schExclusiveZoneModel);

                if (schExclusiveZoneModel.ResultCD.Equals("0000"))
                {

                    StatusMessage("타겟팅 정보가 저장되었습니다.");
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




        //취소 클릭 이벤트
        private void btnCancel_Click(object sender, EventArgs e)
        {   
            this.Close();
        }

        //요일별 체크 이벤트 
        private void chkWeekYn_CheckedChanged(object sender, EventArgs e)
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

                chkTimeYn.Checked = false;
                
                init_tvTimes();
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
        
        private void chkTimeYn_CheckedChanged(object sender, EventArgs e)
        {
            if (chkTimeYn.Checked)
            {
                //tlvDay.Enabled = true;
                //tlvEnd.Enabled = true;
                rbWeek.Enabled      = true;
                rbWeekDay.Enabled   = true;
                rbWeekEnd.Enabled   = true;
                rbWeekAll.Enabled   = true;
                RadioWeekChecked();
            }
            else
            {
                //tlvDay.Enabled = true;
                //tlvEnd.Enabled = true;
                rbWeek.Enabled      = false;
                rbWeekDay.Enabled   = false;
                rbWeekEnd.Enabled   = false;
                rbWeekAll.Enabled   = false;
                RadioWeekChecked();

                init_tvTimes();
                tlvDay.Enabled = false;
                tlvEnd.Enabled = false;

            }
        }

        //분리 클릭 이벤트 
        private void rbWeek_Click(object sender, EventArgs e)
        {
            int num = getDayOfNum();
            
            if (num >= 1 && num <= 15)
            {
                MessageBox.Show("시간 타겟과 요일 타겟 값이 일치 하지 않습니다!", "타겟팅", MessageBoxButtons.OK, MessageBoxIcon.Information);
                init_tvTimes();
                tlvDay.Enabled = false;
                tlvEnd.Enabled = false;
            }
            else if (num == 16 || num == 18 || num == 34)
            {
                MessageBox.Show("시간 타겟과 요일 타겟 값이 일치 하지 않습니다!", "타겟팅", MessageBoxButtons.OK, MessageBoxIcon.Information);
                init_tvTimes();
                tlvDay.Enabled = false;
                tlvEnd.Enabled = false;
            }
            else
            {
                dayOfNum = 0;
                init_tvTimes();
                enableTreeView();
            }
        }
        //주중 클릭 이벤트 
        private void rbWeekDay_Click(object sender, EventArgs e)
        {
            int num = getDayOfNum();

            if (num == 0)
            {
                dayOfNum = 1;
                init_tvTimes();
                enableTreeView();
            }
            else
            {
                if (num >= 1 && num <= 15)
                {
                    dayOfNum = 1;
                    init_tvTimes();
                    enableTreeView();
                }
                else
                {
                    MessageBox.Show("시간 타겟과 요일 타겟 값이 일치 하지 않습니다!", "타겟팅", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    init_tvTimes();
                    tlvDay.Enabled = false;
                    tlvEnd.Enabled = false;
                }
            }
        }
        //주말 클릭 이벤트 
        private void rbWeekEnd_Click(object sender, EventArgs e)
        {

            int num = getDayOfNum();

            if (num == 0)  // 요일타겟이 없으르모 주말 시간을 의미 하는 값으로 설정
            {
                dayOfNum = 16;
                init_tvTimes();
                enableTreeView();
            }
            else
            {
                if (num == 16 || num == 18 || num == 34)
                {
                    dayOfNum = 16;
                    init_tvTimes();
                    enableTreeView();
                }
                else
                {
                    MessageBox.Show("시간 타겟과 요일 타겟 값이 일치 하지 않습니다!", "타겟팅", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    init_tvTimes();
                    tlvDay.Enabled = false;
                    tlvEnd.Enabled = false;
                }
            }
        }
        //없음 클릭 이벤트 
        private void rbWeekAll_Click(object sender, EventArgs e)
        {

            int num = getDayOfNum();

            if (num >= 1 && num <= 15)
            {
                MessageBox.Show("시간 타겟과 요일 타겟 값이 일치 하지 않습니다!", "타겟팅", MessageBoxButtons.OK, MessageBoxIcon.Information);
                init_tvTimes();
                tlvDay.Enabled = false;
                tlvEnd.Enabled = false;
            }
            else if (num == 16 || num == 18 || num == 34)
            {
                MessageBox.Show("시간 타겟과 요일 타겟 값이 일치 하지 않습니다!", "타겟팅", MessageBoxButtons.OK, MessageBoxIcon.Information);
                init_tvTimes();
                tlvDay.Enabled = false;
                tlvEnd.Enabled = false;
            }
            else
            {
                dayOfNum = -1;
                init_tvTimes();
                enableTreeView();
            }
        }

        #endregion


        


        

        
    }
}
