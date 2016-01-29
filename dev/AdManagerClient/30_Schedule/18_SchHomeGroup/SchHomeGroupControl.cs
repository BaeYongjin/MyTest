// ===============================================================================
// SchHomeGroupControl for Charites Project
//
// SchHomeGroupControl.cs
//
// 홈OAP그룹편성현황 컨트롤를 정의 합니다.  
//
// ===============================================================================

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.Collections;
using Janus.Windows.GridEX;

using WinFramework.Misc;
using WinFramework.Base;
using WinFramework.Data;

using AdManagerModel;

namespace AdManagerClient
{
    /// <summary>
    /// 홈OAP그룹그룹편성 컨트롤
    /// </summary>
    public partial class SchHomeGroupControl : System.Windows.Forms.UserControl, IUserControl
    {

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
        private MenuPower menu = FrameSystem.oMenu;

        // 사용할 정보모델
        SchHomeGroupModel schHomeGroupModel = new SchHomeGroupModel();

        // 메뉴코드 : 보안이 필요한 화면에 필요함
        public string menuCode = "";

        bool canCreate = false;
        bool canRead = false;
        bool canUpdate = false;
        bool canDelete = false;

        bool IsSearching = false; // 조회중 상세화면이 업데이트 되는 것을 방지 하기위함

        // 매체
        public string keyGroupCode = "";
        public string keyWeek = "";
        public string keyTime = "";
        
        private string keyGroupName = ""; // 선택 된 홈OAP그룹명
        private string keyTgtWeek = "";
        private string keyTgtTime = "";
        Hashtable hTable = new Hashtable(); // 그룹과 그 그룹이 설정된 컬러의 인덱스 값.
        
        private int colorIndex = 0;

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

        #region 컨트롤 로드

        public SchHomeGroupControl()
        {
            InitializeComponent();
        }

        private void SchHomeGroupControl_Load(object sender, EventArgs e)
        {
            // 컨트롤 초기화
            InitControl();
            hTable.Clear();

            GetSchHomeList1();
            GetSchHomeList2();
            grdList1.SelectedItems.Clear(); // 목록을 조회한 뒤 Default로 맨위에 있는 row가 선택됨. 그래서 선택된 row를 없애기 위해 clear함
            grdList2.SelectedItems.Clear();

        }

        #endregion

        #region 컨트롤 초기화

        private void InitControl()
        {
            ProgressStart();

            // 추가권한 검사
            if (menu.CanCreate(MenuCode))
            {
                canCreate = true;
            }

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

            InitButton();
            ProgressStop();
        }

        private void InitButton()
        {
            if (canCreate)
            {
                btnAdd1.Enabled = true;
                btnAdd2.Enabled = true;
            }
        }

        private void DisableButton()
        {
            btnAdd1.Enabled = false;
            btnAdd2.Enabled = false;
        }

        #endregion

        #region 컨트롤 액션처리 메소드

        /// <summary>
        /// 조회버튼 클릭시
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSearch_Click(object sender, EventArgs e)
        {
            GetSchHomeList1();
            GetSchHomeList2();
        }

        /// <summary>
        /// 주중편성 버튼 클릭시
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAdd1_Click(object sender, EventArgs e)
        {
            try
            {
                HomeGroupPopup pForm = new HomeGroupPopup();
                pForm.OnRefreshParent += new HomeGroupPopup.RefreshDelegate(pForm_OnRefreshParent);

                pForm.TimeData1 = getDate1();
                pForm.Week = "1";
                pForm.ShowDialog();
                pForm.Dispose();
                pForm = null;

                InitButton();
            }
            catch
            {
                InitButton();               
            }
        }

        /// <summary>
        /// 주말편성 버튼 클릭시
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAdd2_Click(object sender, EventArgs e)
        {
            try
            {
                HomeGroupPopup pForm = new HomeGroupPopup();
                pForm.OnRefreshParent += new HomeGroupPopup.RefreshDelegate(pForm_OnRefreshParent);

                pForm.TimeData2 = getDate2();
                pForm.Week = "2";
                pForm.ShowDialog();
                pForm.Dispose();
                pForm = null;

                InitButton();
            }
            catch
            {
                InitButton();
            }
        }
        
        /// <summary>
        /// 새로고침 핸들러
        /// </summary>
        void pForm_OnRefreshParent()
        {
            hTable.Clear();
            GetSchHomeList1();
            GetSchHomeList2();
        }

        private void grdList1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            try
            {
                // 특정 그룹을 선택한 값(현재는 그룹명...쿼리에서 그룹명과 코드 동시 보여주기가 깔끔하지 않아서..)
                if (grdList1.HitTest(e.X, e.Y) == Janus.Windows.GridEX.GridArea.Cell)
                {
                    string grpName = "";
                    string week = "";
                    string time = "";

                    keyGroupName = "";// init
                    keyTgtTime = "";
                    keyTgtWeek = "";

                    grdList1.CurrentColumn = grdList1.ColumnFromPoint(e.X, e.Y);
                    grdList1.Row = grdList1.RowPositionFromPoint(e.X, e.Y);
                    if (grdList1.CurrentColumn != null && grdList1.Row >= 0)
                    {
                        grpName = grdList1.CurrentRow.Cells[grdList1.CurrentColumn].Value.ToString();
                        week = grdList1.CurrentColumn.Key;
                        time = grdList1.CurrentRow.Cells["TgtTime"].Value.ToString();

                        if (grpName.Length > 0) keyGroupName = grpName;
                        if (week.Length > 0) keyTgtWeek = week;
                        if (time.Length > 0) keyTgtTime = time;
                    }

                    HomeGroupPopup pForm = new HomeGroupPopup();
                    pForm.OnRefreshParent += new HomeGroupPopup.RefreshDelegate(pForm_OnRefreshParent);
                    pForm.Week = "1";
                    pForm.TimeData1 = getDate1();
                    pForm.WeekDay = keyTgtWeek;
                    pForm.ShowDialog();
                    pForm.Dispose();
                    pForm = null;
                }
            }
            catch
            {
                InitButton();
            }
        }

        private void grdList2_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            try
            {
                //특정 그룹을 선택한 값(현재는 그룹명...쿼리에서 그룹명과 코드 동시 보여주기가 깔끔하지 않아서..)
                if (grdList2.HitTest(e.X, e.Y) == Janus.Windows.GridEX.GridArea.Cell)
                {
                    string grpName = "";
                    string week = "";
                    string time = "";

                    keyGroupName = "";// init
                    keyTgtTime = "";
                    keyTgtWeek = "";

                    grdList2.CurrentColumn = grdList2.ColumnFromPoint(e.X, e.Y);
                    grdList2.Row = grdList2.RowPositionFromPoint(e.X, e.Y);
                    if (grdList2.CurrentColumn != null && grdList2.Row >= 0)
                    {
                        grpName = grdList2.CurrentRow.Cells[grdList2.CurrentColumn].Value.ToString();
                        week = grdList2.CurrentColumn.Key;
                        time = grdList2.CurrentRow.Cells["TgtTime"].Value.ToString();

                        if (grpName.Length > 0) keyGroupName = grpName;
                        if (week.Length > 0) keyTgtWeek = week;
                        if (time.Length > 0) keyTgtTime = time;
                    }

                    HomeGroupPopup pForm = new HomeGroupPopup();
                    pForm.OnRefreshParent += new HomeGroupPopup.RefreshDelegate(pForm_OnRefreshParent);
                    pForm.TimeData2 = getDate2();
                    pForm.Week = "2";
                    pForm.WeekDay = keyTgtWeek;
                    pForm.ShowDialog();
                    pForm.Dispose();
                    pForm = null;
                }
            }
            catch
            {
                InitButton();
            }
        }

        #endregion

        #region 처리 메소드

        /// <summary>
        /// 그룹 선택 여부
        /// </summary>
        /// <returns></returns>
        private bool isSelectedGroup()
        {
            if (keyGroupName.Length == 0)
            {
                MessageBox.Show("선택 된 그룹이 없습니다!");
                return false;
            }
            else
                return true;
        }

        /// <summary>
        /// 주중 조회
        /// </summary>
        private void GetSchHomeList1()
        {
            IsSearching = true;

            try
            {
                // 광고파일목록조회 서비스를 호출한다.
                new SchHomeGroupManager(systemModel, commonModel).GetSchHomeList1(schHomeGroupModel);

                if (schHomeGroupModel.ResultCD.Equals("0000"))
                {
                    grdList1.DataSource = schHomeGroupModel.SchHomeGroupModelDataSet.Tables[0];
                }

                GetSchHomeListCount();
                fillColorCell();
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
                IsSearching = false; // 조회중 Flag 리셋
            }
        }

        /// <summary>
        /// 주말 조회
        /// </summary>
        private void GetSchHomeList2()
        {
            IsSearching = true;

            try
            {
                // 광고파일목록조회 서비스를 호출한다.
                new SchHomeGroupManager(systemModel, commonModel).GetSchHomeList2(schHomeGroupModel);

                if (schHomeGroupModel.ResultCD.Equals("0000"))
                {
                    grdList2.DataSource = schHomeGroupModel.SchHomeGroupModelDataSet.Tables[0];
                }
                fillColor2();
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
                IsSearching = false; // 조회중 Flag 리셋
            }
        }

        /// <summary>
        /// 홈OAP그룹편성갯수 조회
        /// </summary>
        private void GetSchHomeListCount()
        {
            try
            {
                // 광고파일목록조회 서비스를 호출한다.
                new SchHomeGroupManager(systemModel, commonModel).GetSchHomeListCount(schHomeGroupModel);

                if (schHomeGroupModel.ResultCD.Equals("0000"))
                {
                                      
                    if (schHomeGroupModel.Count < 168)
                    {
                        lbNotice.Visible = true;
                    }
                    else
                    {
                        lbNotice.Visible = false;
                    }
                }
            }
            catch (FrameException fe)
            {
                FrameSystem.showMsgForm("그룹편성갯수 조회오류", new string[] { fe.ErrCode, fe.ResultMsg });
            }
            catch (Exception ex)
            {
                FrameSystem.showMsgForm("홈광고 편성현황 조회오류", new string[] { "", ex.Message });
            }
        }

        /// <summary>
        /// 삭제
        /// </summary>
        private void SetSchHomeDelete()
        {
            if (keyGroupName.Length <= 0)
            {
                MessageBox.Show("선택된 광고가 없습니다.", "홈OAP광고그룹 편성내역 삭제", MessageBoxButtons.OK);

                return;
            }

            DialogResult result = MessageBox.Show("해당 홈OAP광고그룹 편성내역을 삭제 하시겠습니까?", "홈OAP광고그룹 편성내역 삭제",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question,
                MessageBoxDefaultButton.Button2);

            if (result == DialogResult.No) return;

            try
            {
            
                if (keyTgtTime.Length > 0 && keyGroupName.Length > 0 && keyTgtWeek.Length > 0)
                {
                    schHomeGroupModel.TgtWeek = keyTgtWeek;
                    schHomeGroupModel.TgtTime = keyTgtTime;
                    schHomeGroupModel.GroupName = keyGroupName;
                    new SchHomeGroupManager(systemModel, commonModel).SetSchHomeDelete(schHomeGroupModel);
                    //삭제 완료
                    FrameSystem.showMsgForm("지정그룹광고 상세편성 삭제", new string[] { "", "삭제 완료 했습니다.!", "" });
                    GetSchHomeList1();
                }

            }
            catch (FrameException fe)
            {
                FrameSystem.showMsgForm("지정그룹광고 상세편성 삭제오류", new string[] { fe.ErrCode, fe.ResultMsg });
            }
            catch (Exception ex)
            {
                FrameSystem.showMsgForm("지정그룹광고 상세편성 삭제오류", new string[] { "", ex.Message });
            }		

        }

        /// <summary>
        /// 주중 날짜데이터 얻기
        /// </summary>
        /// <returns></returns>
        private string getDate1()
        {
            string time1 = "";

            foreach (GridEXRow row in grdList1.GetRows())
            {
                if (row.Selected)
                {
                    time1 += "'" + Convert.ToString(row.Cells["TgtTime"].Value) + "',";
                }
            }

            if (time1.Length >= 5)
                time1= time1.Substring(0, time1.Length - 1);

            return time1;
        }

        /// <summary>
        /// 주중 날짜데이터 얻기
        /// </summary>
        /// <returns></returns>
        private string getDate2()
        {
            string time1 = "";
            
            foreach (GridEXRow row in grdList2.GetRows())
            {
                if (row.Selected)
                {
                    time1 += "'" + Convert.ToString(row.Cells["TgtTime"].Value) + "',";
                }
            }

            if (time1.Length >= 5)
                time1 = time1.Substring(0, time1.Length - 1);
                        
            return time1;
        }

        /// <summary>
        /// 그리드뷰 색 지정함수
        /// </summary>
        /// <returns></returns>
        private GridEXFormatStyle[] makeOfColors()
        {
            Color[] bgColor = new Color[7];
            bgColor[0] = Color.PowderBlue;
            bgColor[1] = Color.Khaki;
            bgColor[2] = Color.LemonChiffon;
            bgColor[3] = Color.Beige;
            bgColor[4] = Color.Gold;
            bgColor[5] = Color.Wheat;
            bgColor[6] = Color.SandyBrown;

            GridEXFormatStyle[] fStyle = new GridEXFormatStyle[7];
            for (int i = 0; i < bgColor.Length; i++)
            {
                fStyle[i] = new GridEXFormatStyle();
                fStyle[i].BackColor = bgColor[i];
            }

            return fStyle;
        }

        /// <summary>
        /// 같은 그룹은 cell의 배경색을 같은 색으로... - 주중
        /// </summary>
        private void fillColorCell()
        {
            string groupNm = "";
            int inxColor = 0;
            colorIndex = 0;

            try
            {

                GridEXFormatStyle[] fStyle = makeOfColors();  // 컬럼 정보를 구성해서 저장.                      

                // 전체 row 수를 체크해야 함.
                for (int i = 0; i < grdList1.GetRows().Length; i++)
                {
                    grdList1.Row = i; // 현재 row
                    GridEXCellCollection cells = grdList1.CurrentRow.Cells; // 현재 row의 cell[시간 컬럼을 제외한 컬럼만 알면 됨.그래서 시작 인덱스가 1]

                    for (int j = 1; j < cells.Count; j++)
                    {
                        if (inxColor >= fStyle.Length) inxColor = 0; // 컬러정보가 있는 배열 수 초과를 막기 위해서 필히 검수

                        if (null != grdList1.CurrentRow.Cells[j].Value
                            && "" != grdList1.CurrentRow.Cells[j].Value.ToString())
                        {
                            groupNm = grdList1.CurrentRow.Cells[j].Value.ToString();

                            if (hTable.Count > 0 && null != hTable[groupNm]) // hash table에 값이 있으면 그룹명으로 검색을 통해서 값이 존재하면 같은 그룹이므로 같은 컬러 인덱스 배열 값만 찾으면 됨.
                            {
                                grdList1.CurrentRow.Cells[j].FormatStyle = fStyle[Convert.ToInt32(hTable[groupNm])];
                            }
                            else
                            {
                                // 기존 hash table에 없는 그룹이므로 신규로 추가를 해주고 컬러 배열 인덱스 값을 증가 처리
                                grdList1.CurrentRow.Cells[j].FormatStyle = fStyle[inxColor];
                                hTable.Add(groupNm, inxColor);
                                inxColor++;
                            }
                        }
                    }
                }
                colorIndex = inxColor;
            }
            catch (Exception) { }
        }

        /// <summary>
        /// 같은 그룹은 cell의 배경색을 같은 색으로... - 주말
        /// </summary>
        private void fillColor2()
        {
            string groupNm = "";
            int inxColor = colorIndex;

            try
            {
                GridEXFormatStyle[] fStyle = makeOfColors();  // 컬럼 정보를 구성해서 저장.                      

                // 전체 row 수를 체크해야 함.
                for (int i = 0; i < grdList2.GetRows().Length; i++)
                {
                    grdList2.Row = i; // 현재 row
                    GridEXCellCollection cells = grdList2.CurrentRow.Cells; // 현재 row의 cell[시간 컬럼을 제외한 컬럼만 알면 됨.그래서 시작 인덱스가 1]

                    for (int j = 1; j < cells.Count; j++)
                    {
                        if (inxColor >= fStyle.Length) inxColor = 0; // 컬러정보가 있는 배열 수 초과를 막기 위해서 필히 검수

                        if (null != grdList2.CurrentRow.Cells[j].Value
                           && "" != grdList2.CurrentRow.Cells[j].Value.ToString())
                        {
                            groupNm = grdList2.CurrentRow.Cells[j].Value.ToString();

                            if (hTable.Count > 0 && null != hTable[groupNm]) // hash table에 값이 있으면 그룹명으로 검색을 통해서 값이 존재하면 같은 그룹이므로 같은 컬러 인덱스 배열 값만 찾으면 됨.
                            {
                                grdList2.CurrentRow.Cells[j].FormatStyle = fStyle[Convert.ToInt32(hTable[groupNm])];
                            }
                            else
                            {
                                // 기존 hash table에 없는 그룹이므로 신규로 추가를 해주고 컬러 배열 인덱스 값을 증가 처리
                                grdList2.CurrentRow.Cells[j].FormatStyle = fStyle[inxColor];
                                hTable.Add(groupNm, inxColor);
                                inxColor++;
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
            }
        }

        #endregion

    }
}
