// ===============================================================================
// SchExclusiveZoneAdControl for Charites Project
//
// SchExclusiveZoneAdControl.cs
//
// 시간대 독점 편성 컨트롤를 정의 합니다.  
//
// ===============================================================================
// Release history
//
// ===============================================================================

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
//using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.Threading;
using WinFramework.Misc;
using WinFramework.Base;
using WinFramework.Data;
using AdManagerModel;

using Janus.Windows.TimeLine;


namespace AdManagerClient
{
    public partial class SchExclusiveZoneAdControl : UserControl, IUserControl
    {
        #region 이벤트 핸들러
        public event StatusEventHandler StatusEvent;
        public event ProgressEventHandler ProgressEvent;
        #endregion

        #region 사용자정의 객체 및 변수

        // 시스템 정보 : 화면공통
        private SystemModel systemModel = FrameSystem.oSysModel;
        private CommonModel commonModel = FrameSystem.oComModel;
        private Logger log = FrameSystem.oLog;
        private MenuPower menu = FrameSystem.oMenu;

        // 메뉴코드 : 보안이 필요한 화면에 필요함
        private string menuCode = "";

        //사용자 정보 모델 
        SchExclusiveZoneModel schExclusiveZoneModel = new SchExclusiveZoneModel();

        //bool IsNewSearchKey = false;			    // 검색어입력 여부        			
        //DataTable dt = null;

        //bool IsSearching = false;
        bool canRead = false;
        bool canUpdate = false;
        bool canDelete = false;

        #endregion

        #region [생성자, 화면 디자인 ]
        public SchExclusiveZoneAdControl()
        {
            InitializeComponent();
        }
        #endregion

        #region [IUserControl 인터페이스]

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

        public void SetDockStyle(DockStyle style)
        {
            this.Dock = style;
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


        #region [ 컨트롤 로드 (시작점) ]
        private void SchExclusiveZoneAdControl_Load(object sender, EventArgs e)
        {
            Application.DoEvents();

            // 컨트롤 초기화
            InitControl();
        }
        #endregion

        #region [컨트롤 초기화]
        private void InitControl()
        {
            ProgressStart();

            // 조회권한 검사
            if (menu.CanRead(MenuCode))
            {
                canRead = true;
            }

            // 수정권한 검사
            if (menu.CanUpdate(MenuCode))
            {
                canUpdate = true;
            }

            // 삭제권한 검사
            if (menu.CanDelete(MenuCode))
            {
                canDelete = true;
            }

            //초기 버튼 설정
            InitButton();

            //초기 스케줄 설정
            InitTimeLine();

            //조회할 리스트 뿌리기 
            if (canRead) SearchTimeLine();

            ProgressStop();
        }

        private void InitButton()
        {
            if (canUpdate == true) btnAdd.Enabled = true;
            if (canDelete == true) btnAdd.Enabled = true;
            Application.DoEvents();
        }

        private void InitTimeLine()
        {
            timeLineEx.FirstDate = DateTime.Now;
            timeLineEx.TimescaleTiers = TimescaleTiers.ThreeTiers;
            timeLineEx.TopTier.Interval = TimeLineInterval.Week;

            timeLineEx.MiddleTier.Interval = TimeLineInterval.Day;
            timeLineEx.MiddleTier.CustomFormat = "D";
            timeLineEx.MiddleTier.Count = 1;

            timeLineEx.BottomTier.Interval = TimeLineInterval.Hour;
            timeLineEx.BottomTier.CustomFormat = "HH";
            timeLineEx.IntervalSize = 60;
            timeLineEx.GridLines = GridLines.Vertical;

        }
        #endregion

        /// <summary>
        /// 검색 시간대 독점 타임 리스트 
        /// </summary>
        private void SearchTimeLine()
        {
            StatusMessage("시간대 독점 편성 현황을 조회합니다.");

            try
            {
                // 데이터모델 초기화
                schExclusiveZoneModel.Init();

                // 광고 타겟팅 목록조회 서비스를 호출한다.
                new SchExclusiveZoneAdManager(systemModel, commonModel).GetSchExclusiveList(schExclusiveZoneModel);

                if (schExclusiveZoneModel.ResultCD.Equals("0000"))
                {


                    #region 시간대 독점 설정에 대한 표현 코딩
                    //1. 데이타 가지고 온다. 
                    DataSet ds = schExclusiveZoneModel.SchExclusiveDataSet;
                    DataTable dt = schExclusiveZoneAdDs.SchExclusiveZoneList;

                    //다시 로드 하기 위해 클리너
                    dt.Clear();

                    string itemNo = string.Empty;
                    string itemName = string.Empty;
                    string startday = string.Empty;
                    string endday = string.Empty;
                    string settime = string.Empty;
                    //요일 적용 추가 
                    string tgtweekYn = string.Empty;
                    string tgtweeks = string.Empty;

                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {

                        itemNo = ds.Tables[0].Rows[i][0].ToString();
                        itemName = ds.Tables[0].Rows[i][1].ToString();
                        startday = ds.Tables[0].Rows[i][2].ToString();
                        endday = ds.Tables[0].Rows[i][3].ToString();
                        settime = ds.Tables[0].Rows[i][4].ToString();

                        tgtweekYn = ds.Tables[0].Rows[i][5].ToString();  //요일적용
                        tgtweeks = ds.Tables[0].Rows[i][6].ToString();    //요일적용

                        string sday = startday.Substring(0, 4) + "/" + startday.Substring(4, 2) + "/" + startday.Substring(6, 2);
                        string eday = endday.Substring(0, 4) + "/" + endday.Substring(4, 2) + "/" + endday.Substring(6, 2);

                        DateTime sDt = Convert.ToDateTime(sday);
                        DateTime eDt = Convert.ToDateTime(eday);

                        //시간 분리
                        string[] weeks = settime.Split('-');
                        string[] splits_d = null;
                        string[] splits_e = null;
                        string[] splits = null;

                        if (weeks != null && weeks.Length > 1)
                        {
                            // 주중, 주말 분리
                            splits_d = weeks[0].Substring(1, weeks[0].Length - 1).Split('^');
                            splits_e = weeks[1].Substring(1, weeks[1].Length - 1).Split('^');
                        }
                        else
                        {
                            if (settime.Substring(0, 1) == "d") // 주중
                            {
                                splits_d = settime.Substring(1, settime.Length - 1).Split('^');

                            }
                            else if (settime.Substring(0, 1) == "e") // 주말
                            {
                                splits_e = settime.Substring(1, settime.Length - 1).Split('^');
                            }
                            else
                            {
                                splits = settime.Split('^');
                            }
                        }

                        //계약 기간 계산해서 DT에 넣기 

                        TimeSpan ts = eDt - sDt;
                        DateTime calDt = new DateTime();
                        calDt = sDt;


                        if (tgtweekYn.Equals("Y"))
                        {
                            //#############요일 선택값이 있다면 #############

                            string[] splitweek = tgtweeks.Split('^');

                            for (int k = 1; k <= ts.Days; k++)
                            {
                                if (k > 1) calDt = calDt.AddDays(1);

                                switch (calDt.DayOfWeek)
                                {
                                    case System.DayOfWeek.Monday:
                                        for (int j = 0; j < splitweek.Length; j++)
                                        {
                                            if (splitweek[j].Equals("2"))
                                            {
                                                #region 평일 Dt 생성
                                                if (splits_d != null && splits_d.Length > 0)
                                                {
                                                    if (!splits_d[0].Equals(""))
                                                    {
                                                        for (int sd = 0; sd < splits_d.Length; sd++)
                                                        {
                                                            int calnextInt = 0;
                                                            calnextInt = Convert.ToInt32(splits_d[sd]) + 1;

                                                            DataRow drNew = dt.NewRow();
                                                            drNew[0] = itemNo;
                                                            drNew[1] = itemNo + "_" + itemName;
                                                            drNew[2] = Convert.ToDateTime(string.Format("{0} {1:D2}:00", calDt.ToString("yyyy/MM/dd"), Convert.ToInt32(splits_d[sd])));
                                                            if (calnextInt == 24)
                                                            {
                                                                drNew[3] = string.Format("{0} 00:00", calDt.AddDays(1).ToString("yyyy/MM/dd"));
                                                            }
                                                            else
                                                            {
                                                                drNew[3] = string.Format("{0} {1:D2}:00", calDt.ToString("yyyy/MM/dd"), calnextInt);
                                                            }

                                                            dt.Rows.Add(drNew);
                                                        }
                                                    }
                                                }
                                                else if (splits != null && splits.Length > 0)
                                                {
                                                    if (!splits[0].Equals(""))
                                                    {
                                                        for (int s = 0; s < splits.Length; s++)
                                                        {
                                                            int calnextInt = 0;
                                                            calnextInt = Convert.ToInt32(splits[s]) + 1;

                                                            DataRow drNew = dt.NewRow();
                                                            drNew[0] = itemNo;
                                                            drNew[1] = itemNo + "_" + itemName;
                                                            drNew[2] = Convert.ToDateTime(string.Format("{0} {1:D2}:00", calDt.ToString("yyyy/MM/dd"), Convert.ToInt32(splits[s])));
                                                            if (calnextInt == 24)
                                                            {
                                                                drNew[3] = string.Format("{0} 00:00", calDt.AddDays(1).ToString("yyyy/MM/dd"));
                                                            }
                                                            else
                                                            {
                                                                drNew[3] = string.Format("{0} {1:D2}:00", calDt.ToString("yyyy/MM/dd"), calnextInt);
                                                            }

                                                            dt.Rows.Add(drNew);
                                                        }
                                                    }
                                                }
                                                #endregion
                                            }
                                        }
                                        break;
                                    case System.DayOfWeek.Tuesday:
                                        for (int j = 0; j < splitweek.Length; j++)
                                        {
                                            if (splitweek[j].Equals("3"))
                                            {
                                                #region 평일 Dt 생성
                                                if (splits_d != null && splits_d.Length > 0)
                                                {
                                                    if (!splits_d[0].Equals(""))
                                                    {
                                                        for (int sd = 0; sd < splits_d.Length; sd++)
                                                        {
                                                            int calnextInt = 0;
                                                            calnextInt = Convert.ToInt32(splits_d[sd]) + 1;

                                                            DataRow drNew = dt.NewRow();
                                                            drNew[0] = itemNo;
                                                            drNew[1] = itemNo + "_" + itemName;
                                                            drNew[2] = Convert.ToDateTime(string.Format("{0} {1:D2}:00", calDt.ToString("yyyy/MM/dd"), Convert.ToInt32(splits_d[sd])));
                                                            if (calnextInt == 24)
                                                            {
                                                                drNew[3] = string.Format("{0} 00:00", calDt.AddDays(1).ToString("yyyy/MM/dd"));
                                                            }
                                                            else
                                                            {
                                                                drNew[3] = string.Format("{0} {1:D2}:00", calDt.ToString("yyyy/MM/dd"), calnextInt);
                                                            }

                                                            dt.Rows.Add(drNew);
                                                        }
                                                    }
                                                }
                                                else if (splits != null && splits.Length > 0)
                                                {
                                                    if (!splits[0].Equals(""))
                                                    {
                                                        for (int s = 0; s < splits.Length; s++)
                                                        {
                                                            int calnextInt = 0;
                                                            calnextInt = Convert.ToInt32(splits[s]) + 1;

                                                            DataRow drNew = dt.NewRow();
                                                            drNew[0] = itemNo;
                                                            drNew[1] = itemNo + "_" + itemName;
                                                            drNew[2] = Convert.ToDateTime(string.Format("{0} {1:D2}:00", calDt.ToString("yyyy/MM/dd"), Convert.ToInt32(splits[s])));
                                                            if (calnextInt == 24)
                                                            {
                                                                drNew[3] = string.Format("{0} 00:00", calDt.AddDays(1).ToString("yyyy/MM/dd"));
                                                            }
                                                            else
                                                            {
                                                                drNew[3] = string.Format("{0} {1:D2}:00", calDt.ToString("yyyy/MM/dd"), calnextInt);
                                                            }

                                                            dt.Rows.Add(drNew);
                                                        }
                                                    }
                                                }
                                                #endregion
                                            }
                                        }
                                        break;
                                    case System.DayOfWeek.Wednesday:
                                        for (int j = 0; j < splitweek.Length; j++)
                                        {
                                            if (splitweek[j].Equals("4"))
                                            {
                                                #region 평일 Dt 생성
                                                if (splits_d != null && splits_d.Length > 0)
                                                {
                                                    if (!splits_d[0].Equals(""))
                                                    {
                                                        for (int sd = 0; sd < splits_d.Length; sd++)
                                                        {
                                                            int calnextInt = 0;
                                                            calnextInt = Convert.ToInt32(splits_d[sd]) + 1;

                                                            DataRow drNew = dt.NewRow();
                                                            drNew[0] = itemNo;
                                                            drNew[1] = itemNo + "_" + itemName;
                                                            drNew[2] = Convert.ToDateTime(string.Format("{0} {1:D2}:00", calDt.ToString("yyyy/MM/dd"), Convert.ToInt32(splits_d[sd])));
                                                            if (calnextInt == 24)
                                                            {
                                                                drNew[3] = string.Format("{0} 00:00", calDt.AddDays(1).ToString("yyyy/MM/dd"));
                                                            }
                                                            else
                                                            {
                                                                drNew[3] = string.Format("{0} {1:D2}:00", calDt.ToString("yyyy/MM/dd"), calnextInt);
                                                            }

                                                            dt.Rows.Add(drNew);
                                                        }
                                                    }
                                                }
                                                else if (splits != null && splits.Length > 0)
                                                {
                                                    if (!splits[0].Equals(""))
                                                    {
                                                        for (int s = 0; s < splits.Length; s++)
                                                        {
                                                            int calnextInt = 0;
                                                            calnextInt = Convert.ToInt32(splits[s]) + 1;

                                                            DataRow drNew = dt.NewRow();
                                                            drNew[0] = itemNo;
                                                            drNew[1] = itemNo + "_" + itemName;
                                                            drNew[2] = Convert.ToDateTime(string.Format("{0} {1:D2}:00", calDt.ToString("yyyy/MM/dd"), Convert.ToInt32(splits[s])));
                                                            if (calnextInt == 24)
                                                            {
                                                                drNew[3] = string.Format("{0} 00:00", calDt.AddDays(1).ToString("yyyy/MM/dd"));
                                                            }
                                                            else
                                                            {
                                                                drNew[3] = string.Format("{0} {1:D2}:00", calDt.ToString("yyyy/MM/dd"), calnextInt);
                                                            }

                                                            dt.Rows.Add(drNew);
                                                        }
                                                    }
                                                }
                                                #endregion
                                            }
                                        }
                                        break;
                                    case System.DayOfWeek.Thursday:
                                        for (int j = 0; j < splitweek.Length; j++)
                                        {
                                            if (splitweek[j].Equals("5"))
                                            {
                                                #region 평일 Dt 생성
                                                if (splits_d != null && splits_d.Length > 0)
                                                {
                                                    if (!splits_d[0].Equals(""))
                                                    {
                                                        for (int sd = 0; sd < splits_d.Length; sd++)
                                                        {
                                                            int calnextInt = 0;
                                                            calnextInt = Convert.ToInt32(splits_d[sd]) + 1;

                                                            DataRow drNew = dt.NewRow();
                                                            drNew[0] = itemNo;
                                                            drNew[1] = itemNo + "_" + itemName;
                                                            drNew[2] = Convert.ToDateTime(string.Format("{0} {1:D2}:00", calDt.ToString("yyyy/MM/dd"), Convert.ToInt32(splits_d[sd])));
                                                            if (calnextInt == 24)
                                                            {
                                                                drNew[3] = string.Format("{0} 00:00", calDt.AddDays(1).ToString("yyyy/MM/dd"));
                                                            }
                                                            else
                                                            {
                                                                drNew[3] = string.Format("{0} {1:D2}:00", calDt.ToString("yyyy/MM/dd"), calnextInt);
                                                            }

                                                            dt.Rows.Add(drNew);
                                                        }
                                                    }
                                                }
                                                else if (splits != null && splits.Length > 0)
                                                {
                                                    if (!splits[0].Equals(""))
                                                    {
                                                        for (int s = 0; s < splits.Length; s++)
                                                        {
                                                            int calnextInt = 0;
                                                            calnextInt = Convert.ToInt32(splits[s]) + 1;

                                                            DataRow drNew = dt.NewRow();
                                                            drNew[0] = itemNo;
                                                            drNew[1] = itemNo + "_" + itemName;
                                                            drNew[2] = Convert.ToDateTime(string.Format("{0} {1:D2}:00", calDt.ToString("yyyy/MM/dd"), Convert.ToInt32(splits[s])));
                                                            if (calnextInt == 24)
                                                            {
                                                                drNew[3] = string.Format("{0} 00:00", calDt.AddDays(1).ToString("yyyy/MM/dd"));
                                                            }
                                                            else
                                                            {
                                                                drNew[3] = string.Format("{0} {1:D2}:00", calDt.ToString("yyyy/MM/dd"), calnextInt);
                                                            }

                                                            dt.Rows.Add(drNew);
                                                        }
                                                    }
                                                }
                                                #endregion
                                            }
                                        }
                                        break;
                                    case System.DayOfWeek.Friday:
                                        for (int j = 0; j < splitweek.Length; j++)
                                        {
                                            if (splitweek[j].Equals("6"))
                                            {
                                                #region 평일 Dt 생성
                                                if (splits_d != null && splits_d.Length > 0)
                                                {
                                                    if (!splits_d[0].Equals(""))
                                                    {
                                                        for (int sd = 0; sd < splits_d.Length; sd++)
                                                        {
                                                            int calnextInt = 0;
                                                            calnextInt = Convert.ToInt32(splits_d[sd]) + 1;

                                                            DataRow drNew = dt.NewRow();
                                                            drNew[0] = itemNo;
                                                            drNew[1] = itemNo + "_" + itemName;
                                                            drNew[2] = Convert.ToDateTime(string.Format("{0} {1:D2}:00", calDt.ToString("yyyy/MM/dd"), Convert.ToInt32(splits_d[sd])));
                                                            if (calnextInt == 24)
                                                            {
                                                                drNew[3] = string.Format("{0} 00:00", calDt.AddDays(1).ToString("yyyy/MM/dd"));
                                                            }
                                                            else
                                                            {
                                                                drNew[3] = string.Format("{0} {1:D2}:00", calDt.ToString("yyyy/MM/dd"), calnextInt);
                                                            }

                                                            dt.Rows.Add(drNew);
                                                        }
                                                    }
                                                }
                                                else if (splits != null && splits.Length > 0)
                                                {
                                                    if (!splits[0].Equals(""))
                                                    {
                                                        for (int s = 0; s < splits.Length; s++)
                                                        {
                                                            int calnextInt = 0;
                                                            calnextInt = Convert.ToInt32(splits[s]) + 1;

                                                            DataRow drNew = dt.NewRow();
                                                            drNew[0] = itemNo;
                                                            drNew[1] = itemNo + "_" + itemName;
                                                            drNew[2] = Convert.ToDateTime(string.Format("{0} {1:D2}:00", calDt.ToString("yyyy/MM/dd"), Convert.ToInt32(splits[s])));
                                                            if (calnextInt == 24)
                                                            {
                                                                drNew[3] = string.Format("{0} 00:00", calDt.AddDays(1).ToString("yyyy/MM/dd"));
                                                            }
                                                            else
                                                            {
                                                                drNew[3] = string.Format("{0} {1:D2}:00", calDt.ToString("yyyy/MM/dd"), calnextInt);
                                                            }

                                                            dt.Rows.Add(drNew);
                                                        }
                                                    }
                                                }
                                                #endregion
                                            }
                                        }
                                        break;
                                    case System.DayOfWeek.Saturday:
                                        for (int j = 0; j < splitweek.Length; j++)
                                        {
                                            if (splitweek[j].Equals("7"))
                                            {
                                                #region 주말 Dt 생성
                                                if (splits_e != null && splits_e.Length > 0)
                                                {
                                                    if (!splits_e[0].Equals(""))
                                                    {
                                                        for (int se = 0; se < splits_e.Length; se++)
                                                        {
                                                            int calnextInt = 0;
                                                            calnextInt = Convert.ToInt32(splits_e[se]) + 1;

                                                            DataRow drNew = dt.NewRow();
                                                            drNew[0] = itemNo;
                                                            drNew[1] = itemNo + "_" + itemName;
                                                            drNew[2] = Convert.ToDateTime(string.Format("{0} {1:D2}:00", calDt.ToString("yyyy/MM/dd"), Convert.ToInt32(splits_e[se])));
                                                            if (calnextInt == 24)
                                                            {
                                                                drNew[3] = string.Format("{0} 00:00", calDt.AddDays(1).ToString("yyyy/MM/dd"));
                                                            }
                                                            else
                                                            {
                                                                drNew[3] = string.Format("{0} {1:D2}:00", calDt.ToString("yyyy/MM/dd"), calnextInt);
                                                            }

                                                            dt.Rows.Add(drNew);
                                                        }
                                                    }
                                                }
                                                else if (splits != null && splits.Length > 1)
                                                {
                                                    if (!splits[0].Equals(""))
                                                    {
                                                        for (int s = 0; s < splits.Length; s++)
                                                        {
                                                            int calnextInt = 0;
                                                            calnextInt = Convert.ToInt32(splits[s]) + 1;

                                                            DataRow drNew = dt.NewRow();
                                                            drNew[0] = itemNo;
                                                            drNew[1] = itemNo + "_" + itemName;
                                                            drNew[2] = Convert.ToDateTime(string.Format("{0} {1:D2}:00", calDt.ToString("yyyy/MM/dd"), Convert.ToInt32(splits[s])));
                                                            if (calnextInt == 24)
                                                            {
                                                                drNew[3] = string.Format("{0} 00:00", calDt.AddDays(1).ToString("yyyy/MM/dd"));
                                                            }
                                                            else
                                                            {
                                                                drNew[3] = string.Format("{0} {1:D2}:00", calDt.ToString("yyyy/MM/dd"), calnextInt);
                                                            }

                                                            dt.Rows.Add(drNew);
                                                        }
                                                    }
                                                }
                                                #endregion
                                            }
                                        }
                                        break;
                                    case System.DayOfWeek.Sunday:
                                        for (int j = 0; j < splitweek.Length; j++)
                                        {
                                            if (splitweek[j].Equals("1"))
                                            {
                                                #region 주말 Dt 생성
                                                if (splits_e != null && splits_e.Length > 0)
                                                {
                                                    if (!splits_e[0].Equals(""))
                                                    {
                                                        for (int se = 0; se < splits_e.Length; se++)
                                                        {
                                                            int calnextInt = 0;
                                                            calnextInt = Convert.ToInt32(splits_e[se]) + 1;

                                                            DataRow drNew = dt.NewRow();
                                                            drNew[0] = itemNo;
                                                            drNew[1] = itemNo + "_" + itemName;
                                                            drNew[2] = Convert.ToDateTime(string.Format("{0} {1:D2}:00", calDt.ToString("yyyy/MM/dd"), Convert.ToInt32(splits_e[se])));
                                                            if (calnextInt == 24)
                                                            {
                                                                drNew[3] = string.Format("{0} 00:00", calDt.AddDays(1).ToString("yyyy/MM/dd"));
                                                            }
                                                            else
                                                            {
                                                                drNew[3] = string.Format("{0} {1:D2}:00", calDt.ToString("yyyy/MM/dd"), calnextInt);
                                                            }

                                                            dt.Rows.Add(drNew);
                                                        }
                                                    }
                                                }
                                                else if (splits != null && splits.Length > 1)
                                                {
                                                    if (!splits[0].Equals(""))
                                                    {
                                                        for (int s = 0; s < splits.Length; s++)
                                                        {
                                                            int calnextInt = 0;
                                                            calnextInt = Convert.ToInt32(splits[s]) + 1;

                                                            DataRow drNew = dt.NewRow();
                                                            drNew[0] = itemNo;
                                                            drNew[1] = itemNo + "_" + itemName;
                                                            drNew[2] = Convert.ToDateTime(string.Format("{0} {1:D2}:00", calDt.ToString("yyyy/MM/dd"), Convert.ToInt32(splits[s])));
                                                            if (calnextInt == 24)
                                                            {
                                                                drNew[3] = string.Format("{0} 00:00", calDt.AddDays(1).ToString("yyyy/MM/dd"));
                                                            }
                                                            else
                                                            {
                                                                drNew[3] = string.Format("{0} {1:D2}:00", calDt.ToString("yyyy/MM/dd"), calnextInt);
                                                            }

                                                            dt.Rows.Add(drNew);
                                                        }
                                                    }
                                                }
                                                #endregion
                                            }
                                        }
                                        break;
                                    default:
                                        break;

                                }
                            }

                            //#############요일 선택값이 있다면 #############
                        }
                        else
                        {
                            //#############요일 선택값이 없다면 ##############
                            #region 요일 선택이 없다면 전체 처리
                            for (int k = 1; k <= ts.Days; k++)
                            {

                                if (k > 1) calDt = calDt.AddDays(1);

                                switch (calDt.DayOfWeek)
                                {
                                    case System.DayOfWeek.Monday:
                                    case System.DayOfWeek.Tuesday:
                                    case System.DayOfWeek.Wednesday:
                                    case System.DayOfWeek.Thursday:
                                    case System.DayOfWeek.Friday:

                                        #region 평일 Dt 생성
                                        if (splits_d != null && splits_d.Length > 0)
                                        {
                                            if (!splits_d[0].Equals(""))
                                            {
                                                for (int sd = 0; sd < splits_d.Length; sd++)
                                                {
                                                    int calnextInt = 0;
                                                    calnextInt = Convert.ToInt32(splits_d[sd]) + 1;

                                                    DataRow drNew = dt.NewRow();
                                                    drNew[0] = itemNo;
                                                    drNew[1] = itemNo + "_" + itemName;
                                                    drNew[2] = Convert.ToDateTime(string.Format("{0} {1:D2}:00", calDt.ToString("yyyy/MM/dd"), Convert.ToInt32(splits_d[sd])));
                                                    if (calnextInt == 24)
                                                    {
                                                        drNew[3] = string.Format("{0} 00:00", calDt.AddDays(1).ToString("yyyy/MM/dd"));
                                                    }
                                                    else
                                                    {
                                                        drNew[3] = string.Format("{0} {1:D2}:00", calDt.ToString("yyyy/MM/dd"), calnextInt);
                                                    }

                                                    dt.Rows.Add(drNew);
                                                }
                                            }
                                        }
                                        else if (splits != null && splits.Length > 0)
                                        {
                                            if (!splits[0].Equals(""))
                                            {
                                                for (int s = 0; s < splits.Length; s++)
                                                {
                                                    int calnextInt = 0;
                                                    calnextInt = Convert.ToInt32(splits[s]) + 1;

                                                    DataRow drNew = dt.NewRow();
                                                    drNew[0] = itemNo;
                                                    drNew[1] = itemNo + "_" + itemName;
                                                    drNew[2] = Convert.ToDateTime(string.Format("{0} {1:D2}:00", calDt.ToString("yyyy/MM/dd"), Convert.ToInt32(splits[s])));
                                                    if (calnextInt == 24)
                                                    {
                                                        drNew[3] = string.Format("{0} 00:00", calDt.AddDays(1).ToString("yyyy/MM/dd"));
                                                    }
                                                    else
                                                    {
                                                        drNew[3] = string.Format("{0} {1:D2}:00", calDt.ToString("yyyy/MM/dd"), calnextInt);
                                                    }

                                                    dt.Rows.Add(drNew);
                                                }
                                            }
                                        }
                                        #endregion

                                        break;
                                    case System.DayOfWeek.Saturday:
                                    case System.DayOfWeek.Sunday:

                                        #region 주말 Dt 생성
                                        if (splits_e != null && splits_e.Length > 0)
                                        {
                                            if (!splits_e[0].Equals(""))
                                            {
                                                for (int se = 0; se < splits_e.Length; se++)
                                                {
                                                    int calnextInt = 0;
                                                    calnextInt = Convert.ToInt32(splits_e[se]) + 1;

                                                    DataRow drNew = dt.NewRow();
                                                    drNew[0] = itemNo;
                                                    drNew[1] = itemNo + "_" + itemName;
                                                    drNew[2] = Convert.ToDateTime(string.Format("{0} {1:D2}:00", calDt.ToString("yyyy/MM/dd"), Convert.ToInt32(splits_e[se])));
                                                    if (calnextInt == 24)
                                                    {
                                                        drNew[3] = string.Format("{0} 00:00", calDt.AddDays(1).ToString("yyyy/MM/dd"));
                                                    }
                                                    else
                                                    {
                                                        drNew[3] = string.Format("{0} {1:D2}:00", calDt.ToString("yyyy/MM/dd"), calnextInt);
                                                    }

                                                    dt.Rows.Add(drNew);
                                                }
                                            }
                                        }
                                        else if (splits != null && splits.Length > 1)
                                        {
                                            if (!splits[0].Equals(""))
                                            {
                                                for (int s = 0; s < splits.Length; s++)
                                                {
                                                    int calnextInt = 0;
                                                    calnextInt = Convert.ToInt32(splits[s]) + 1;

                                                    DataRow drNew = dt.NewRow();
                                                    drNew[0] = itemNo;
                                                    drNew[1] = itemNo + "_" + itemName;
                                                    drNew[2] = Convert.ToDateTime(string.Format("{0} {1:D2}:00", calDt.ToString("yyyy/MM/dd"), Convert.ToInt32(splits[s])));
                                                    if (calnextInt == 24)
                                                    {
                                                        drNew[3] = string.Format("{0} 00:00", calDt.AddDays(1).ToString("yyyy/MM/dd"));
                                                    }
                                                    else
                                                    {
                                                        drNew[3] = string.Format("{0} {1:D2}:00", calDt.ToString("yyyy/MM/dd"), calnextInt);
                                                    }

                                                    dt.Rows.Add(drNew);
                                                }
                                            }
                                        }
                                        #endregion

                                        break;
                                    default:
                                        break;
                                }
                            }
                            #endregion
                            //#############요일 선택값이 없다면 ##############
                        }

                    }
                    #endregion

                    StatusMessage(schExclusiveZoneModel.ResultCnt + "건의 광고 정보가 조회되었습니다.");
                }


            }
            catch (FrameException fe)
            {
                FrameSystem.showMsgForm("시간대 독점편성 편성현황 조회오류", new string[] { fe.ErrCode, fe.ResultMsg });
            }
            catch (Exception ex)
            {
                FrameSystem.showMsgForm("시간대 독점 편성 편성현황 조회오류", new string[] { "", ex.Message });
            }
        }


        #region 처리 이벤트
        //더블 클릭시 이벤트 
        private void timeLineEx_DoubleClick(object sender, EventArgs e)
        {
            TimeLineArea tmlArea = this.timeLineEx.HitTest();
            SchExclusiveZoneAd_pSet pSet = null;
            if (tmlArea == TimeLineArea.TimeLineItem)
            {
                //팝업 창을 띄우는 부분이며 광고아이템 키를 가지고 간다. 
                //MessageBox.Show(timeLineEx.GetItemAt()..GetValue(1).ToString());
                //MessageBox.Show(timeLineEx.GetItemAt().Text.ToString());

                pSet = new SchExclusiveZoneAd_pSet();
                pSet.KeyItem = timeLineEx.GetItemAt().Text.ToString();

                if (pSet.ShowDialog() == DialogResult.Yes)
                {
                    ProgressStart();
                    SearchTimeLine();
                    ProgressStop();
                }

            }
        }
        //시간대 독점 편성 되지 않는 목록을 
        private void btnAdd_Click(object sender, EventArgs e)
        {
            SchExclusiveZoneAd_pAdd pAdd = new SchExclusiveZoneAd_pAdd();


            if (pAdd.ShowDialog() == DialogResult.Yes)
            {
                ProgressStart();

                SearchTimeLine();

                ProgressStop();
            }
        }
        #endregion


    }
}
