using System;
using System.Collections.Generic;
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
    public partial class AnalysisItemGroupControl : UserControl, IUserControl
    {

        #region 사용자정의 객체 및 변수

        // 시스템 정보 : 화면공통
        private SystemModel systemModel = FrameSystem.oSysModel;
        private CommonModel commonModel = FrameSystem.oComModel;
        private Logger log = FrameSystem.oLog;
        private MenuPower menu = FrameSystem.oMenu;

        // 메뉴코드 : 보안이 필요한 화면에 필요함
        public string menuCode = "";

        // 사용할 정보모델
        AnalysisItemGroupModel analysisItemGroupModel = new AnalysisItemGroupModel();	// 분석용 광고묶음 모델

        // 화면처리용 변수
        bool IsAdding = false;
        bool canRead = false;
        bool canUpdate = false;
        bool canCreate = false;
        bool canDelete = false;
        bool expandGridView = false;

        string SearchMonthP = "-1";     // 선택된 광고월
        string SearchMonthT = "-1";     // 불러온 광고월
        int IntcbSearchMonth = 1;       // 선택된 광고월 INDEX
        int ItemGroupNo = 0;            // 선택된 광고묶음번호(저장,조회 용)
        int ItemGroupNoCm = 0;          // 선택된 광고묶음번호(그리드[현재위치]용)
        int ItemGroupCount = 0;         // 광고묶음 갯수
        CurrencyManager cm = null;      // 데이터 그리드의 변경에 따른 데이터셋 관리를 위하여				
        DataTable dt = null;
        DataSet tempset = null;         // 자동등록에 전달할 광고내역

        #endregion

        #region 컨트롤 로드

        public AnalysisItemGroupControl()
        {
            InitializeComponent();
        }

        private void AnalysisItemGroupControl_Load(object sender, EventArgs e)
        {
            // 데이터관리용 객체생성
            dt = ((DataView)grdExAnalysisItemGroupList.DataSource).Table;
            cm = (CurrencyManager)this.BindingContext[grdExAnalysisItemGroupList.DataSource];

            InitControl();
        }

        #endregion

        #region 컨트롤 초기화

        private void InitControl()
        {
            SearchAnalysisMonths();
            SearchAnalysisItemGroup();
            SearchContractItem();
            AnalysisGroupChoice();
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

        #region 이벤트핸들러
        public event StatusEventHandler StatusEvent;			// 상태이벤트 핸들러
        public event ProgressEventHandler ProgressEvent;			// 처리중이벤트 핸들러
        #endregion

        #region 처리메소드

        /// <summary>
        /// 수행월 검색
        /// </summary>
        private void SearchAnalysisMonths()
        {

            StatusMessage("수행월 조회합니다.");
            try
            {
                //광고묶음테이블을 GROUPBY 하여 조회되는 연월을 뽑습니다.
                new AnalysisItemGroupManager(systemModel, commonModel).GetAnalysisMonths(analysisItemGroupModel);

                if (analysisItemGroupModel.ResultCD.Equals("0000"))
                {
                    Utility.SetDataTable(analysisItemGroupDs.AnalysisMonths, analysisItemGroupModel.AnalysisMonthsDataSet);

                    // 검색조건의 콤보
                    this.cbSearchMonth.Items.Clear();

                    //DateTime을 이용하여 비어있는 모든 연월을 구한뒤 위에서 뽑은 연월데이터와 바인딩합니다.
                    #region 연월 바인딩

                    Dictionary<string, string> monthdata = new Dictionary<string, string>();
                    monthdata.Clear();
                    for (int i = 0; i < analysisItemGroupModel.ResultCnt; i++)
                    {
                        DataRow row = analysisItemGroupDs.AnalysisMonths.Rows[i];
                        monthdata.Add("20" + row["AnalysisMonth"].ToString(), row["AnalysisItemGroupCount"].ToString());
                    }

                    //시작연월
                    DateTime StartDate = new DateTime(2008, 1, 1);

                    DateTime todayDate = DateTime.Today;

                    //앞선 월을 늘리기 위해서는 아래 숫자를 늘려줍니다.
                    todayDate = todayDate.AddMonths(3);

                    int j = 0;

                    //시작연월로 전달부터 시작합니다. 
                    DateTime lastDate = StartDate;

                    while (lastDate.Ticks < todayDate.Ticks)//
                    {
                        lastDate = lastDate.AddMonths(1);
                        j++;
                    }

                    lastDate = StartDate;

                    Janus.Windows.EditControls.UIComboBoxItem[] comboItems = new Janus.Windows.EditControls.UIComboBoxItem[j + 1];

                    string monthKey = null;
                    string monthKeyValue = null;
                    comboItems[0] = new Janus.Windows.EditControls.UIComboBoxItem("수행월 전체", "-1"); // 디폴트값
                    j = 0;
                    while (lastDate.Ticks < todayDate.Ticks)//
                    {

                        todayDate = todayDate.AddMonths(-1);
                        monthKey = String.Format("{0:yyyyMM}", todayDate);
                        monthKeyValue = "0";

                        foreach (string key in monthdata.Keys)
                        {
                            if (key.Equals(monthKey))
                            {
                                monthKeyValue = monthdata[key];
                                monthdata.Remove(key);
                                break;
                            }
                        }

                        comboItems[j + 1] = new Janus.Windows.EditControls.UIComboBoxItem(monthKey + " (" + monthKeyValue + ")", monthKey);

                        j++;
                    }

                    #endregion

                    // 콤보에 셋트
                    this.cbSearchMonth.Items.AddRange(comboItems);
                    this.cbSearchMonth.SelectedIndex = IntcbSearchMonth;

                }

            }
            catch (FrameException fe)
            {
                FrameSystem.showMsgForm("수행월 조회오류", new string[] { fe.ErrCode, fe.ResultMsg });
            }
            catch (Exception ex)
            {
                FrameSystem.showMsgForm("수행월 조회오류", new string[] { "", ex.Message });
            }
            finally
            {
                //IsSearching = false; // 조회중 Flag 리셋
            }
        }

        /// <summary>
        /// 분석용광고묶음 검색
        /// </summary>
        private void SearchAnalysisItemGroup()
        {

            StatusMessage("분석용광고묶음 조회합니다.");
            try
            {
                //Validation 부분
                if (!ebSearchKey.Text.Equals("검색어를 입력하세요"))
                {
                    analysisItemGroupModel.SearchKey = ebSearchKey.Text;
                }
                else
                {
                    analysisItemGroupModel.SearchKey = "";
                }

                if (!cbSearchMonth.SelectedValue.ToString().Equals("-1"))
                {
                    analysisItemGroupModel.SearchMonth = SearchMonthP.Substring(2, 4);
                    SearchMonthT = SearchMonthP;
                }
                else
                {
                    analysisItemGroupModel.SearchMonth = cbSearchMonth.SelectedValue.ToString();
                }

                new AnalysisItemGroupManager(systemModel, commonModel).GetAnalysisItemGroup(analysisItemGroupModel);

                if (analysisItemGroupModel.ResultCD.Equals("0000"))
                {
                    // 조회된 광고묶음갯수 저장
                    ItemGroupCount = analysisItemGroupModel.ResultCnt;
                    Utility.SetDataTable(analysisItemGroupDs.AnalysisItemGroup, analysisItemGroupModel.AnalysisItemGroupDataSet);
                }

                if (ItemGroupCount == 0)
                {
                    ItemGroupNo = 0;
                }

            }
            catch (FrameException fe)
            {
                FrameSystem.showMsgForm("분석용광고묶음 조회오류", new string[] { fe.ErrCode, fe.ResultMsg });
            }
            catch (Exception ex)
            {
                FrameSystem.showMsgForm("분석용광고묶음 조회오류", new string[] { "", ex.Message });
            }
            finally
            {
                //IsSearching = false; // 조회중 Flag 리셋
            }
        }

        /// <summary>
        /// 분석용광고묶음상세 검색
        /// </summary>
        private void SearchAnalysisItemGroupDetail()
        {
            StatusMessage("분석용광고묶음상세 조회합니다.");

            // 광고묶음에서 조회된 목록이 없을경우 PASS
            if (ItemGroupCount == 0)
                return;

            try
            {
                analysisItemGroupModel.AnalysisItemGroupNo = ItemGroupNo;
                analysisItemGroupModel.SearchMonth = SearchMonthP.Substring(2, 4);

                //선택된 광고묶음이 없을경우 패스
                if (ItemGroupNo != 0)
                {
                    new AnalysisItemGroupManager(systemModel, commonModel).GetAnalysisItemGroupDetail(analysisItemGroupModel);

                    if (analysisItemGroupModel.ResultCD.Equals("0000"))
                    {
                        Utility.SetDataTable(analysisItemGroupDs.AnalysisItemGroupDetail, analysisItemGroupModel.AnalysisItemGroupDetailDataSet);
                    }
                }

            }
            catch (FrameException fe)
            {
                FrameSystem.showMsgForm("분석용광고묶음상세 조회오류", new string[] { fe.ErrCode, fe.ResultMsg });
            }
            catch (Exception ex)
            {
                FrameSystem.showMsgForm("분석용광고묶음상세 조회오류", new string[] { "", ex.Message });
            }
        }

        /// <summary>
        /// 분석용광고내역 검색
        /// </summary>
        private void SearchContractItem()
        {
            StatusMessage("분석용광고내역 조회합니다.");
            try
            {
                if (!ebSearchKey1.Text.Equals("검색어를 입력하세요"))
                {
                    analysisItemGroupModel.SearchKey = ebSearchKey1.Text;
                }
                else
                {
                    analysisItemGroupModel.SearchKey = "";
                }

                if (!SearchMonthP.Equals("-1"))
                    analysisItemGroupModel.SearchMonth = SearchMonthP.Substring(2, 4);
                else
                    return;

                new AnalysisItemGroupManager(systemModel, commonModel).GetContractItemList(analysisItemGroupModel);

                if (analysisItemGroupModel.ResultCD.Equals("0000"))
                {
                    Utility.SetDataTable(analysisItemGroupDs.ContractItem, analysisItemGroupModel.ContractItemDataSet);
                    //자동등록용 데이터를 미리저장한다.
                    tempset = analysisItemGroupModel.ContractItemDataSet;
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
        }

        /// <summary>
        /// 분석용광고묶음상세 저장
        /// </summary>
        private void SaveAnalysisItemGroupDetail()
        {
            StatusMessage("분석용광고묶음상세를 저장합니다.");

            if (ItemGroupNo <= 0)
            {
                MessageBox.Show("선택된 광고묶음이 없습니다..", "광고묶음상세 저장",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                grdExAnalysisItemGroupList.Focus();
                return;
            }

            try
            {
                //저장될 광고내역 갯수와 저장된 갯수를 비교하여 실패건수 확인/
                int SetCount = 0;
                int AddCount = 0;

                foreach (Janus.Windows.GridEX.GridEXRow row in grdExContractItemList.GetCheckedRows())
                {

                    analysisItemGroupModel.Init();
                    analysisItemGroupModel.AnalysisItemGroupNo = ItemGroupNo;
                    ItemGroupNoCm = ItemGroupNo;
                    analysisItemGroupModel.ItemNo = Convert.ToInt16(row.Cells["ItemNo"].Value.ToString());

                    new AnalysisItemGroupManager(systemModel, commonModel).SetAnalysisItemGroupDetailCreate(analysisItemGroupModel);
                    AddCount++;
                    if (analysisItemGroupModel.ResultCD.Equals("0000"))
                    {
                        SetCount++;
                    }

                }

                if (SetCount > 0)
                {
                    StatusMessage("분석용광고묶음상세가 추가되었습니다.");
                    if (AddCount != SetCount)
                    {
                        MessageBox.Show((AddCount - SetCount) + "건의 광고등록에 실패하였습니다.", "광고묶음상세 저장",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    SearchAnalysisItemGroup();
                    SearchContractItem();
                    AnalysisGroupChoice();
                }
                else
                {
                    MessageBox.Show("등록된 광고목록이 없습니다.", "광고묶음상세 저장",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (FrameException fe)
            {
                FrameSystem.showMsgForm("분석용광고묶음상세 추가오류", new string[] { fe.ErrCode, fe.ResultMsg });
            }
            catch (Exception ex)
            {
                FrameSystem.showMsgForm("분석용광고묶음상세 추가오류", new string[] { "", ex.Message });
            }
        }

        /// <summary>
        /// 분석용광고묶음 저장
        /// </summary>
        private void SaveAnalysisItemGroup()
        {
            StatusMessage("분석용광고묶음을 저장합니다.");

            if (ebAnalysisItemGroupName.Text.Trim().Length == 0)
            {
                MessageBox.Show("광고묶음명이 입력되지 않았습니다.", "광고묶음 저장",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                ebAnalysisItemGroupName.Focus();
                return;
            }

            try
            {
                // 데이터모델에 전송할 내용을 셋트한다.
                analysisItemGroupModel.AnalysisItemGroupNo = ItemGroupNo;
                analysisItemGroupModel.AnalysisItemGroupName = ebAnalysisItemGroupName.Text;
                analysisItemGroupModel.AnalysisItemGroupMonth = SearchMonthT.Substring(2, 4);
                analysisItemGroupModel.AnalysisItemGroupType = "1";
                analysisItemGroupModel.Comment = ebComment.Text;

                // 분석용광고묶음 저장 서비스를 호출한다.
                if (IsAdding)
                {
                    new AnalysisItemGroupManager(systemModel, commonModel).SetAnalysisItemGroupCreate(analysisItemGroupModel);
                    ItemGroupNoCm = analysisItemGroupModel.AnalysisItemGroupNo;
                    StatusMessage("분석용광고묶음이 추가되었습니다.");
                    IsAdding = false;
                    ResetAnalysisItemGroupText();
                    SearchAnalysisMonths();
                }
                else
                {
                    new AnalysisItemGroupManager(systemModel, commonModel).SetAnalysisItemGroupUpdate(analysisItemGroupModel);
                    ItemGroupNoCm = analysisItemGroupModel.AnalysisItemGroupNo;
                    StatusMessage("분석용광고묶음이 저장되었습니다.");
                }
                SearchAnalysisItemGroup();
                AnalysisGroupChoice();
                InitButton();
            }
            catch (FrameException fe)
            {
                FrameSystem.showMsgForm("매체정보 저장오류", new string[] { fe.ErrCode, fe.ResultMsg });
            }
            catch (Exception ex)
            {
                FrameSystem.showMsgForm("매체정보 저장오류", new string[] { "", ex.Message });
            }



        }

        /// <summary>
        /// 분석용광고묶음 삭제
        /// </summary>
        private void DeleteAnalysisItemGroup()
        {
            StatusMessage("분석용광고묶음을 삭제합니다.");

            if (ItemGroupNo <= 0)
            {
                MessageBox.Show("광고묶음이 선택되지 않았습니다.", "광고묶음 삭제",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                grdExAnalysisItemGroupList.Focus();
                return;
            }

            DialogResult result = MessageBox.Show("해당 광고묶음 정보를 삭제 하시겠습니까?", "광고묶음 삭제",
            MessageBoxButtons.YesNo, MessageBoxIcon.Question,
            MessageBoxDefaultButton.Button2);

            if (result == DialogResult.No) return;

            try
            {
                analysisItemGroupModel.AnalysisItemGroupNo = ItemGroupNo;
                new AnalysisItemGroupManager(systemModel, commonModel).SetAnalysisItemGroupDelete(analysisItemGroupModel);
                ReloadList();
            }
            catch (FrameException fe)
            {
                FrameSystem.showMsgForm("분석용광고묶음 삭제오류", new string[] { fe.ErrCode, fe.ResultMsg });
            }
            catch (Exception ex)
            {
                FrameSystem.showMsgForm("분석용광고묶음 삭제오류", new string[] { "", ex.Message });
            }

        }

        /// <summary>
        /// 분석용광고묶음상세 삭제
        /// </summary>
        private void DeleteAnalysisItemGroupDetail()
        {
            StatusMessage("분석용광고묶음상세를 삭제합니다.");
            if (ItemGroupNo <= 0)
            {
                MessageBox.Show("광고묶음상세가 선택되지 않았습니다.", "광고묶음 삭제",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                grdExAnalysisItemGroupDetailList.Focus();
                return;
            }

            try
            {
                int SetCount = 0;
                int i = 0;

                foreach (Janus.Windows.GridEX.GridEXRow row in grdExAnalysisItemGroupDetailList.GetCheckedRows())
                {
                    i++;
                }

                if (i == 0)
                {
                    MessageBox.Show("삭제할 광고목록이 없습니다.", "광고묶음상세 삭제", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                foreach (Janus.Windows.GridEX.GridEXRow row in grdExAnalysisItemGroupDetailList.GetCheckedRows())
                {

                    analysisItemGroupModel.Init();
                    analysisItemGroupModel.AnalysisItemGroupNo = ItemGroupNo;
                    ItemGroupNoCm = ItemGroupNo;
                    analysisItemGroupModel.ItemNo = Convert.ToInt16(row.Cells["ItemNo"].Value.ToString());
                    new AnalysisItemGroupManager(systemModel, commonModel).SetAnalysisItemGroupDetailDelete(analysisItemGroupModel);

                    if (analysisItemGroupModel.ResultCD.Equals("0000"))
                    {
                        SetCount++;
                    }

                }

                if (SetCount > 0)
                {
                    ReloadList();
                    StatusMessage("분석용광고묶음상세가 삭제되었습니다.");

                }
                else
                {
                    MessageBox.Show("분석용광고묶음상세가 없습니다.", "광고묶음상세 삭제",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (FrameException fe)
            {
                FrameSystem.showMsgForm("분석용광고묶음상세 삭제오류", new string[] { fe.ErrCode, fe.ResultMsg });
            }
            catch (Exception ex)
            {
                FrameSystem.showMsgForm("분석용광고묶음상세 삭제오류", new string[] { "", ex.Message });
            }
        }

        /// <summary>
        /// 분석용광고묶음상세 출력
        /// </summary>
        private void SetAnalysisItemGroupDetailText()
        {
            try
            {
                if (grdExAnalysisItemGroupList.GetRow().RowIndex < 0) return;	// 데이터가 없으면 실행하지 않는다.
                ebAnalysisItemGroupName.Text = analysisItemGroupDs.AnalysisItemGroup.Rows[grdExAnalysisItemGroupList.GetRow().RowIndex]["AnalysisItemGroupName"].ToString();
                ebComment.Text = analysisItemGroupDs.AnalysisItemGroup.Rows[grdExAnalysisItemGroupList.GetRow().RowIndex]["Comment"].ToString();
                ebRegDt.Text = analysisItemGroupDs.AnalysisItemGroup.Rows[grdExAnalysisItemGroupList.GetRow().RowIndex]["RegID"].ToString();
                ebModDt.Text = analysisItemGroupDs.AnalysisItemGroup.Rows[grdExAnalysisItemGroupList.GetRow().RowIndex]["RegDt"].ToString();
                ebRegID.Text = analysisItemGroupDs.AnalysisItemGroup.Rows[grdExAnalysisItemGroupList.GetRow().RowIndex]["ModDt"].ToString();
                ebRegName.Text = analysisItemGroupDs.AnalysisItemGroup.Rows[grdExAnalysisItemGroupList.GetRow().RowIndex]["RegName"].ToString();
            }
            catch
            {
                //로우가 없을때로 에러를 자동으로 넘기면 문제가 없는것으로 판단
            }
            StatusMessage("준비");
        }

        /// <summary>
        /// 분석용광고묶음리스트 출력
        /// </summary>
        private void SetAnalysisItemGroupDetail()
        {
            try
            {
                //광고묶음상세, 광고내용 구분
                if (tabAnalysisItemGroup.SelectedIndex == 0)
                {
                    btnAdd.Enabled = true;
                    if (ItemGroupCount == 0)
                    {
                        btnSave.Enabled = false;
                        btnDelete.Enabled = false;
                    }
                    else
                    {
                        btnSave.Enabled = true;
                        btnDelete.Enabled = true;
                    }
                    btnAdd1.Enabled = false;
                    SetAnalysisItemGroupDetailText();
                    btnDelete.ToolTipText = "선택된 광고묶음이 삭제됩니다.";
                }
                else if (tabAnalysisItemGroup.SelectedIndex == 1)
                {
                    btnAdd.Enabled = false;
                    btnDelete.Enabled = true;
                    btnSave.Enabled = false;
                    btnAdd1.Enabled = true;
                    SearchAnalysisItemGroupDetail();
                    btnDelete.ToolTipText = "선택된 광고목록이 삭제됩니다.";

                }
            }
            catch
            {

            }

        }

        /// <summary>
        /// 분석용광고묶음상세 초기화
        /// </summary>
        private void ResetAnalysisItemGroupText()
        {

            try
            {
                if (analysisItemGroupDs.AnalysisItemGroupDetail != null)
                {
                    analysisItemGroupDs.AnalysisItemGroupDetail.Clear();
                }
            }
            catch
            {
            }

            ebAnalysisItemGroupName.Text = "";
            ebComment.Text = "";
            ebRegDt.Text = "";
            ebModDt.Text = "";
            ebRegID.Text = "";
            ebRegName.Text = "";
        }

        /// <summary>
        /// 키캆을찾아 그리드 키에 해당되는로우로..
        /// </summary>
        private void AnalysisGroupChoice()
        {
            StatusMessage("키캆");

            try
            {
                int rowIndex = 0;
                if (analysisItemGroupDs.Tables["AnalysisItemGroup"].Rows.Count < 1) return;

                foreach (DataRow row in analysisItemGroupDs.Tables["AnalysisItemGroup"].Rows)
                {
                    if (row["AnalysisItemGroupNo"].ToString().Equals(ItemGroupNoCm.ToString()))
                    {
                        cm.Position = rowIndex;
                        break;
                    }
                    rowIndex++;
                    grdExAnalysisItemGroupList.EnsureVisible();
                }

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
        /// 버튼 초기화
        /// </summary>
        private void InitButton()
        {
            IsAdding = false;

        }

        /// <summary>
        /// 리스트 재로드
        /// </summary>
        private void ReloadList()
        {
            SearchAnalysisMonths();
            ResetAnalysisItemGroupText();
            SearchAnalysisItemGroup();
            SearchContractItem();
            AnalysisGroupChoice();
        }

        /// <summary>
        /// 사용자구분에 따라 권한줌
        /// </summary>
        private void ResetTextReadonly()
        {
            // 사용자구분이 어드민 또는 수퍼유저인 경우만 수정할 수 있다. 아직 결정된건 없으므로 냅둠
            if (commonModel.UserClass.Equals("10") || commonModel.UserClass.Equals("20"))
            {
                btnAdd.Enabled = true;
                btnDelete.Enabled = true;
                btnSave.Enabled = true;
            }
        }

        /// <summary>
        /// 자동등록폼 호출
        /// /// </summary>
        private void AutoAddForm()
        {
            string filterStr = "";
            string analysisItemGroupName = "";

            AnalysisItemAutoAddForm aIForm = null;
            try
            {
                int i = 0;

                //자동등록폼에 전달할 Filter문장을 만듭니다.
                //바인딩소스에서 제공하는 where문 활용
                foreach (Janus.Windows.GridEX.GridEXRow row in grdExContractItemList.GetCheckedRows())
                {
                    if (i == 0)
                    {
                        analysisItemGroupName = row.Cells["ItemName"].Value.ToString();
                    }
                    else
                    {
                        filterStr += " or ";
                    }
                    filterStr += "(RapCode = " + Convert.ToInt16(row.Cells["RapCode"].Value.ToString()) + " and ";
                    filterStr += "AgencyCode = " + Convert.ToInt16(row.Cells["AgencyCode"].Value.ToString()) + " and ";
                    filterStr += "AdvertiserCode = " + Convert.ToInt16(row.Cells["AdvertiserCode"].Value.ToString()) + ")";
                    i++;
                }

                if (i == 0)
                {
                    MessageBox.Show("선택된 광고목록이 없습니다.", "자등등록", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                aIForm = new AnalysisItemAutoAddForm(tempset);
                aIForm.Filter = filterStr;
                aIForm.AnalysisItemGroupName = analysisItemGroupName;
                aIForm.AnalysisItemGroupMonth = SearchMonthT;
                if (aIForm.ShowDialog() == DialogResult.Yes)
                {
                    ItemGroupNoCm = aIForm.AnalysisItemGroupNo;
                    ReloadList();
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("여는데 실패함" + ex.Message, "자등등록", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

        }


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

        private void btnSearch1_Click(object sender, EventArgs e)
        {
            SearchContractItem();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            ReloadList();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            SaveAnalysisItemGroup();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            btnAdd.Enabled = false;
            btnDelete.Enabled = false;
            btnSave.Enabled = true;

            IsAdding = true;

            ResetTextReadonly();
            ResetAnalysisItemGroupText();
            ebAnalysisItemGroupName.Focus();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (tabAnalysisItemGroup.SelectedIndex == 0)
            {
                DeleteAnalysisItemGroup();
            }
            else if (tabAnalysisItemGroup.SelectedIndex == 1)
            {
                DeleteAnalysisItemGroupDetail();
            }
        }

        private void btnAdd1_Click(object sender, EventArgs e)
        {
            SaveAnalysisItemGroupDetail();

        }

        private void ebAutoAdd_Click(object sender, EventArgs e)
        {
            AutoAddForm();
        }

        private void ebSearchKey_Click(object sender, EventArgs e)
        {
            if (ebSearchKey.Text.Equals("검색어를 입력하세요"))
                ebSearchKey.Text = "";
        }

        private void ebSearchKey1_Click(object sender, EventArgs e)
        {
            if (ebSearchKey1.Text.Equals("검색어를 입력하세요"))
                ebSearchKey1.Text = "";
        }

        private void grdExContractItemList_SelectionChanged(object sender, EventArgs e)
        {
            analysisItemGroupDs.ContractItem.AcceptChanges();
            grdExContractItemList.UpdateData();
        }

        private void grdExAnalysisItemGroupList_SelectionChanged(object sender, EventArgs e)
        {
            try
            {
                string tempMonth = SearchMonthP;
                ItemGroupNo = Convert.ToInt16(analysisItemGroupDs.AnalysisItemGroup.Rows[grdExAnalysisItemGroupList.GetRow().RowIndex]["AnalysisItemGroupNo"].ToString());
                SearchMonthP = "20" + analysisItemGroupDs.AnalysisItemGroup.Rows[grdExAnalysisItemGroupList.GetRow().RowIndex]["AnalysisItemGroupMonth"].ToString();
                if (tempMonth != SearchMonthP)
                {
                    SearchContractItem();
                }
            }
            catch
            {
            }
            SetAnalysisItemGroupDetail();

        }

        private void tabAnalysisItemGroup_SelectedTabChanged(object sender, Janus.Windows.UI.Tab.TabEventArgs e)
        {
            SetAnalysisItemGroupDetail();
        }

        private void cbSearchMonth_SelectedIndexChanged(object sender, EventArgs e)
        {
            SearchMonthP = cbSearchMonth.SelectedValue.ToString();
            IntcbSearchMonth = cbSearchMonth.SelectedIndex;
        }

        #endregion

    }
}
