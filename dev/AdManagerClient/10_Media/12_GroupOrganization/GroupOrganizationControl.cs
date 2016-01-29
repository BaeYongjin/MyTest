/*
 * -------------------------------------------------------
 * Class Name: GroupOrganizationBiz.cs
 * 주요기능  : OAP편성그룹관리 서비스
 * -------------------------------------------------------
 * [수정사항]
 * -------------------------------------------------------
 * 수정코드  : [E_01]
 * 수정자    : 김보배
 * 수정일    : 2013.05.21
 * 수정내용  :        
 *            - 셋탑 FileSize별 계산 오류 수정
 * 수정함수  :
 *            - myCaclFileSize()         
 * --------------------------------------------------------
 */

using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using System.Threading;
using System.Reflection;

using AdManagerClient.Common.Args;

using WinFramework.Misc;
using WinFramework.Base;
using WinFramework.Data;

using AdManagerModel;

namespace AdManagerClient
{
    /// <summary>
    /// OAP 편성그룹관리 컨트롤
    /// </summary>
    public partial class GroupOrganizationControl : System.Windows.Forms.UserControl, IUserControl
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
        GroupOrganizationModel groupOrganizationModel = new GroupOrganizationModel();

        // 메뉴코드 : 보안이 필요한 화면에 필요함
        public string menuCode = "";

        // 화면처리용 변수
        CurrencyManager cmGroup = null;
        CurrencyManager cmDetail = null;
        DataTable dtGroup = null;
        DataTable dtDetail = null;

        // 사용권한
        bool IsSearching = false; // 조회중 상세화면이 업데이트 되는 것을 방지 하기위함
        bool IsAdding = false;

        bool canCreate = false;
        bool canRead = false;
        bool canUpdate = false;
        bool canDelete = false;

        // Key
        string keyMediaCode = "1";
        string keyItemNo = ""; 
        string keyItemName = "";
        string keyLastOrder = "";

        string groupCode    = "";    // 그룹코드 저장변수
        string reGroupCode  = "";    //

        //광고 선택 팝업창
        ItemMultiChoiceForm pForm = null;

        string defMedaiCode = "1";			        // 편성대상광고를 위한 매체코드. 편성목록조회시 셋트
        public string keyScheduleOrder = "";		// 팝업에서도 사용하기위해  public
        public string keyScheduleOrderSet = "";

        // 순위변경구분
        const int ORDER_FIRST = 1;
        const int ORDER_LAST = 2;
        const int ORDER_UP = 3;
        const int ORDER_DOWN = 4;

        // 편성배포 승인상태 처리용
        private string keyAckNo = "";
        private string keyAckState = "";

        // 셋탑
        private int _fileDefaultSize = 0;
        private int _fileDefaultCnt = 0;
        private int _fileSamSungSize = 0;
        private int _fileSamSungCnt = 0;
        private int _fileKaonSize = 0;
        private int _fileKaonCnt = 0;

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

        public GroupOrganizationControl()
        {
            InitializeComponent();
        }

        private void GroupOrganization_Load(object sender, EventArgs e)
        {
			cmGroup = (CurrencyManager) this.BindingContext[grdExGroupList.DataSource];
            dtGroup = ((DataView)grdExGroupList.DataSource).Table;

			cmDetail = (CurrencyManager) this.BindingContext[grdExScheduleList.DataSource];
            dtDetail = ((DataView)grdExScheduleList.DataSource).Table;
            cmGroup.PositionChanged += new EventHandler(OnGrdGroupRowChange);

			// 컨트롤 초기화
			InitControl();
            SearchGroupList();
        }

        #endregion

        #region 컨트롤 초기화

        private void InitControl()
		{
			ProgressStart();
								
			// 추가권한 검사
			if(menu.CanCreate(MenuCode))
			{
				canCreate = true;
			}

			// 조회권한 검사
			if(menu.CanRead(MenuCode))
			{
				canRead = true;
			}

			// 수정권한 검사
			if(menu.CanUpdate(MenuCode))
			{				
				ResetTextReadonly();
				canUpdate = true;
			}

			// 삭제권한 검사
			if(menu.CanDelete(MenuCode))
			{
				canDelete = true;
			}

			InitButton();
			ProgressStop();
		}

        private void InitButton()
		{
			if(canCreate) btnAdd.Enabled    = true;
			if(canDelete) btnDelete.Enabled = true;
			if(canUpdate)
            {
                btnSave.Enabled   = true;
            }
		}

		private void DisableButton()
		{
			btnDelete.Enabled	= false;
			btnSave.Enabled		= false;
			btnAdd.Enabled		= false;
		}

        #endregion

        #region 컨트롤 액션처리 메소드

        /// <summary>
		/// 추가버튼 클릭
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
        private void btnAdd_Click(object sender, EventArgs e)
        {
            btnAdd.Enabled = false;
            btnDelete.Enabled = false;
            btnSave.Enabled = true;

            IsAdding = true;
            ResetTextReadonly();
            ResetGroupText();

            ebGroupName.Focus();
        }

		/// <summary>
		/// 삭제버튼 클릭
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
        private void btnDelete_Click(object sender, EventArgs e)
        {
            DeleteGroup();
            ResetGroupText();
        }

        /// <summary>
		/// 저장버튼 클릭
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
        private void btnSave_Click(object sender, EventArgs e)
        {
            SaveGroup();
        }

        /// <summary>
        /// 그룹 그리드 Row 변경시
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnGrdGroupRowChange(object sender, EventArgs e)
        {
            if (grdExGroupList.SelectedItems.Count < 1 && grdExGroupList.Focus())
            {
                //SetGroupDetailText();
                InitButton();
            }
        }

        private void grdExGroupList_SelectionChanged(object sender, EventArgs e)
        {
            if (grdExGroupList.RecordCount > 0 && grdExGroupList.RowCount > 0)
            {
                SetGroupDetailText();
                SearchScheduleHomeAd();
                InitButton();
            }
        }

        /// <summary>
        /// 명령구분 / 추가버튼 클릭
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAdd1_Click(object sender, EventArgs e)
        {
            DisableButton();

            //  홈광고 대상목록 검색창 
            pForm = new ItemMultiChoiceForm(this);

            // 매체코드셋트
            //pForm.keyMediaCode = defMedaiCode;
            pForm.keyOrder = keyScheduleOrderSet;
            pForm.callType = "GroupOrganizationControl";
            pForm.ReturnDate += new ItemMultiChoiceForm.PopupService(ItemMultiChoiceForm_Return);
            pForm.ShowDialog();
            pForm.Dispose();
            pForm = null;

            InitButton();
        }

        /// <summary>
        /// pForm 폼에서 광고 선택
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        public void ItemMultiChoiceForm_Return(object sender, EventArgs args)
        {
            ItemEventArgs itemEventArgs = (ItemEventArgs)args;
            ItemMultiChoice_pDs itemMultiChoice_pDs = (ItemMultiChoice_pDs)itemEventArgs.dataSet;

            try
            {
                int SetCount = 0;
                DataRow row = null;

                for (int i = 0; i < itemMultiChoice_pDs.ChoiceAdItems.Rows.Count; i++)
                {
                    row = itemMultiChoice_pDs.ChoiceAdItems.Rows[i];

                    if (row["CheckYn"].ToString().Equals("True"))
                    {
                        groupOrganizationModel.GroupCode = groupCode;
                        groupOrganizationModel.MediaCode = keyMediaCode;
                        groupOrganizationModel.ItemNo = row["ItemNo"].ToString();
                        groupOrganizationModel.ItemName = row["ItemName"].ToString();
                        groupOrganizationModel.ScheduleOrder = keyScheduleOrderSet;

                        new GroupOrganizationManager(systemModel, commonModel).SetSchHomeAdAdd(groupOrganizationModel);

                        if (groupOrganizationModel.ResultCD.Equals("0000"))
                        {
                            SetCount++;
                            reGroupCode = groupCode;
                        }

                        keyScheduleOrder = groupOrganizationModel.ScheduleOrder;
                    }
                }

                SearchGroupList();
                GroupListChoice();
            }
            catch (FrameException fe)
            {
                FrameSystem.showMsgForm("홈광고 편성오류", new string[] { fe.ErrCode, fe.ResultMsg });
            }
            catch (Exception ex)
            {
                FrameSystem.showMsgForm("홈광고 편성오류", new string[] { "", ex.Message });
            }	
        }

        /// <summary>
        /// 명령구분 / 상업광고추가 버튼 클릭시
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAddCm_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("상업광고 슬롯을 추가 합니다.", "홈광고 편성", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
            if (result == DialogResult.No)
            {
                StatusMessage("상업광고 슬롯추가 취소");
                return;
            }

            AddSchHomeAdd();
        }

        /// <summary>
        /// 명령구분 / 삭제버튼 클릭
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDelete1_Click(object sender, EventArgs e)
        {
            DeleteScheduleHomeAd();
        }

        /// <summary>
        /// 셋탑구분 - 공용버튼 클릭
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSTB_Common_Click(object sender, EventArgs e)
        {
            SetSchHomeCommonYn(1);
        }

        /// <summary>
        /// 셋탑구분 - 기본버튼 클릭
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSTB_HDD_Click(object sender, EventArgs e)
        {
            SetSchHomeCommonYn(0);
        }

        /// <summary>
        /// 로그설정 - 설정버튼클릭
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOk_Click(object sender, EventArgs e)
        {
            // 로그설정버튼 클릭시 Log에 1값 셋팅
            SetLogYnSchHome(1);
        }

        /// <summary>
        /// 로그설정 - 해제버튼 클릭
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnNot_Click(object sender, EventArgs e)
        {
            // 로그설정해제버튼 클릭시 Log에 0값 셋팅
            SetLogYnSchHome(0);
        }

        /// <summary>
        /// 편성순서변경 - 처음버튼 클릭
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOrderFirst_Click(object sender, EventArgs e)
        {
            OrderSetScheduleHomeAd(ORDER_FIRST);
        }

        /// <summary>
        /// 편성순서변경 - 올림버튼 클릭
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOrderUp_Click(object sender, EventArgs e)
        {
            OrderSetScheduleHomeAd(ORDER_UP);
        }

        /// <summary>
        /// 편성순서변경 - 내림버튼 클릭
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOrderDown_Click(object sender, EventArgs e)
        {
            OrderSetScheduleHomeAd(ORDER_DOWN);
        }

        /// <summary>
        /// 편성순서변경 - 끝버튼 클릭
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOrderLast_Click(object sender, EventArgs e)
        {
            OrderSetScheduleHomeAd(ORDER_LAST);
        }

        /// <summary>
        /// 홈광고 편성현황 그리드 셀 변경시
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void grdExScheduleList_CellValueChanged(object sender, Janus.Windows.GridEX.ColumnActionEventArgs e)
        {
            if (e.Column.EditType == Janus.Windows.GridEX.EditType.CheckBox)
            {
                int curRow = cmDetail.Position;

                if (curRow >= 0)
                {
                    dtDetail.Rows[curRow].BeginEdit();
                    dtDetail.Rows[curRow]["CheckYn"] = grdExScheduleList.GetValue(e.Column).ToString();
                    dtDetail.Rows[curRow].EndEdit();
                }
            }
        }

        /// <summary>
        /// 홈광고 그리드뷰 헤더 체크박스 클릭시
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void grdExScheduleList_ColumnHeaderClick(object sender, Janus.Windows.GridEX.ColumnActionEventArgs e)
        {
            //체크박스컬럼이 아니면 빠져나가게 처리.
            if (e.Column.Index != 0)    return;
            
            //ColumnHeader Click시에 dtDetail Setting 
            DataRow[] rows = dtDetail.Select("CheckYn = 'False'");

            if (rows.Length == 0)
            {
                for (int i = 0; i < dtDetail.Rows.Count; i++)
                {
                    dtDetail.Rows[i].BeginEdit();
                    dtDetail.Rows[i]["CheckYn"] = "False";
                    dtDetail.Rows[i].EndEdit();
                }
            }
            else
            {
                for (int i = 0; i < dtDetail.Rows.Count; i++)
                {
                    dtDetail.Rows[i].BeginEdit();
                    dtDetail.Rows[i]["CheckYn"] = "True";
                    dtDetail.Rows[i].EndEdit();
                }
            }
        }

        #endregion

        #region 처리메소드
        
        /// <summary>
		/// 그룹목록 조회
		/// </summary>
		private void SearchGroupList()
		{
            IsSearching = true;

			StatusMessage("편성그룹목록을 조회합니다.");

			try
            {
			    // 그룹목록조회 서비스를 호출한다.
                new GroupOrganizationManager(systemModel, commonModel).GetGroupList(groupOrganizationModel);

                if (groupOrganizationModel.ResultCD.Equals("0000"))
				{
                    Utility.SetDataTable(groupOrganizationDs.GroupList, groupOrganizationModel.GroupOrganizationDataSet);
                    StatusMessage(groupOrganizationModel.ResultCnt + "건의 편성그룹 정보가 조회되었습니다.");
				}

				SetGroupDetailText();

                if (canCreate) btnAdd1.Enabled = true;
                if (canUpdate) btnSave.Enabled = true;
                if (canDelete) btnDelete1.Enabled = true;
			}
			catch(FrameException fe)
			{
                FrameSystem.showMsgForm("편성그룹조회오류", new string[] { fe.ErrCode, fe.ResultMsg });
			}
			catch(Exception ex)
			{
                FrameSystem.showMsgForm("편성그룹조회오류", new string[] { "", ex.Message });
			}
            finally
            {
                IsSearching = false; // 조회중 Flag 리셋
            }
		}

        /// <summary>
		///  그룹목록저장 & 추가
		/// </summary>
		private void SaveGroup()
		{
            StatusMessage("편성그룹 정보를 저장합니다.");
			
			if(ebGroupName.Text.Trim().Length == 0) 
			{
				MessageBox.Show("그룹명이 입력되지 않았습니다.","그룹 저장", 
					MessageBoxButtons.OK, MessageBoxIcon.Information );
                ebGroupName.Focus();

				return;		
			}

			try
			{
				int curRow = cmGroup.Position;
                groupOrganizationModel.Init();

				if(curRow >= 0)
				{
                    groupOrganizationModel.GroupCode = dtGroup.Rows[curRow]["GroupCode"].ToString();
				}
                groupOrganizationModel.GroupName = ebGroupName.Text;
                groupOrganizationModel.Comment = ebComment.Text;
                if (rbUseYn_Y.Checked)				//사용여부
				{
                    groupOrganizationModel.UseYn = "Y";
				}
				else
				{
                    groupOrganizationModel.UseYn = "N";
				}

				if (IsAdding)
				{
                    // 추가 
                    new GroupOrganizationManager(systemModel, commonModel).SetGroupAdd(groupOrganizationModel);
					StatusMessage("그룹 정보가 추가되었습니다.");
					IsAdding = false;
				}
				else
				{
                    // 저장
                    new GroupOrganizationManager(systemModel, commonModel).SetGroupUpdate(groupOrganizationModel);
                    StatusMessage("그룹 정보가 저장되었습니다.");
                }
                reGroupCode = groupCode;

                DisableButton();
                SearchGroupList();
                GroupListChoice();
                InitButton();
			}
			catch(FrameException fe)
			{
                FrameSystem.showMsgForm("편성그룹 비고 저장오류", new string[] { fe.ErrCode, fe.ResultMsg });
			}
			catch(Exception ex)
			{
                FrameSystem.showMsgForm("편성그룹 비고 저장오류", new string[] { "", ex.Message });
			}			
		}

		/// <summary>
		/// 그룹 삭제
		/// </summary>
		private void DeleteGroup()
		{
            StatusMessage("편성그룹을 삭제합니다.");

            DialogResult result = MessageBox.Show("편성그룹을 삭제 하시겠습니까?", "편성그룹 삭제",
				MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);

			if (result == DialogResult.No) return;

			try
			{
				int SetCount = 0;

				// 삭제 시킴
				int curRow = cmGroup.Position;
				if(curRow >= 0)
				{
                    groupOrganizationModel.Init();
                    groupOrganizationModel.GroupCode = dtGroup.Rows[curRow]["GroupCode"].ToString();
					
					new GroupOrganizationManager(systemModel,commonModel).SetGroupDelete(groupOrganizationModel);

                    if (groupOrganizationModel.ResultCD.Equals("0000"))
					{
						SetCount++;
					}
				}

				if(SetCount > 0)
				{
                    StatusMessage("편성그룹이 삭제되었습니다.");
                    DisableButton();
                    SearchGroupList();
                    InitButton();
				}
				else
				{
                    MessageBox.Show("선택된 편성그룹이 없습니다.", "편성그룹 삭제",
						MessageBoxButtons.OK, MessageBoxIcon.Information);
				}
			}
			catch(FrameException fe)
			{
                FrameSystem.showMsgForm("편성그룹 삭제오류", new string[] { fe.ErrCode, fe.ResultMsg });
			}
			catch(Exception ex)
			{
                FrameSystem.showMsgForm("편성그룹 삭제오류", new string[] { "", ex.Message });
			}			
		}

        /// <summary>
        /// 홈광고편성현황 조회
        /// </summary>
        private void SearchScheduleHomeAd()
        {
            IsSearching = true;

            StatusMessage("홈광고 편성현황을 조회합니다.");

            try
            {
                // 데이터모델 초기화
                groupOrganizationModel.Init();
                groupOrganizationModel.GroupCode = groupCode;
                groupOrganizationModel.ScheduleOrder = keyScheduleOrder;

                // 광고파일목록조회 서비스를 호출한다.
                new GroupOrganizationManager(systemModel, commonModel).GetSchHomeAdList(groupOrganizationModel);

                if (groupOrganizationModel.ResultCD.Equals("0000"))
                {
                    Utility.SetDataTable(groupOrganizationDs.AdSchedule, groupOrganizationModel.GroupOrganizationDataSet);
                    StatusMessage(groupOrganizationModel.ResultCnt + "건의 광고파일 정보가 조회되었습니다.");

                    defMedaiCode = "1";

                    keyLastOrder = groupOrganizationModel.LastOrder;
                    AddSchChoice();
                   
                    myCaclFileSize();
                }

                // 편성배포승인 처리상태를 조회한다.
                keyAckNo = "";
                keyAckState = "";

                groupOrganizationModel.GroupCode = groupCode;

                // 현재 승인상태조회 서비스를 호출한다.
                new GroupOrganizationManager(systemModel, commonModel).GetPublishState(groupOrganizationModel, 0);

                if (groupOrganizationModel.ResultCD.Equals("0000"))
                {
                    keyAckNo = groupOrganizationModel.AckNo;
                    keyAckState = groupOrganizationModel.State;

                    if (keyAckState.Equals("10"))	// 승인상태가 10:편성중이면
                    {
                        //lbMsg.Text = "편성 진행중입니다.";
                    }
                    else if (keyAckState.Equals("20")) // 승인상태가 20:편성승인 상태이면 편성이 불가하다.
                    {
                        canCreate = false;
                        canUpdate = false;
                        canDelete = false;

                        MessageBox.Show("현재 편성승인 후 검수승인 대기상태이므로 편성을 변경할 수 없습니다.", "홈광고편성", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else if (keyAckState.Equals("25")) // 승인상태가 25:배포대기 상태이면 편성이 불가하다.
                    {
                        //lbMsg.Text = "검수승인 후 배포승인 대기중입니다.";
                        canCreate = false;
                        canUpdate = false;
                        canDelete = false;

                        MessageBox.Show("현재 검수승인 후 배포승인 대기상태이므로 편성을 변경할 수 없습니다.", "홈광고편성", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else if (keyAckState.Equals("30")) // 승인상태가 30:배포승인 상태이면 신규편성이 가능하다.
                    {
                        //lbMsg.Text = "";
                    }
                }

                SetDetailText();
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
        /// 상업광고 추가
        /// </summary>
        private void AddSchHomeAdd()
        {
            StatusMessage("상업광고 위치Slot를 추가합니다.");

            try
            {
                groupOrganizationModel.Init();
                groupOrganizationModel.GroupCode = groupCode;
                groupOrganizationModel.MediaCode = keyMediaCode;
                groupOrganizationModel.ItemNo = "0000";         // 상업광고는 광고번호를 0000으로 한다
                groupOrganizationModel.ItemName = "상업광고 슬롯";

                new GroupOrganizationManager(systemModel, commonModel).SetSchHomeAdAdd(groupOrganizationModel);
                
                if (groupOrganizationModel.ResultCD.Equals("0000"))
                {
                    reGroupCode = groupCode;
                    keyScheduleOrder = groupOrganizationModel.ScheduleOrder;

                    SearchGroupList();
                    GroupListChoice();
                    StatusMessage("상업광고 위치Slot 추가");
                }
            }
            catch (FrameException fe)
            {
                FrameSystem.showMsgForm("홈광고 편성오류", new string[] { fe.ErrCode, fe.ResultMsg });
            }
            catch (Exception ex)
            {
                FrameSystem.showMsgForm("홈광고 편성오류", new string[] { "", ex.Message });
            }
        }

        /// <summary>
        /// 홈광고 편성내역 삭제
        /// </summary>
        private void DeleteScheduleHomeAd()
        {
            StatusMessage("홈광고 편성내역을 삭제합니다.");

            DialogResult result = MessageBox.Show("해당 홈광고 편성내역을 삭제 하시겠습니까?", "홈광고 편성내역 삭제",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);

            if (result == DialogResult.No) return;

            try
            {
                int SetCount = 0;

                // 홈광고는 삭제할때 노출순서도 바꿔줘야 하므로
                // 삭제순서는 순위가 높은 것부터 낮은 순으로 해야 한다.			
                for (int i = groupOrganizationDs.AdSchedule.Rows.Count - 1; i >= 0; i--)
                {
                    DataRow row = groupOrganizationDs.AdSchedule.Rows[i];

                    if (row["CheckYn"].ToString().Equals("True"))
                    {
                        // 데이터모델에 전송할 내용을 셋트한다.
                        groupOrganizationModel.Init();
                        groupOrganizationModel.GroupCode = row["GroupCode"].ToString();
                        groupOrganizationModel.MediaCode = row["MediaCode"].ToString();
                        groupOrganizationModel.ItemNo = row["ItemNo"].ToString();
                        groupOrganizationModel.ItemName = row["ItemName"].ToString();
                        groupOrganizationModel.ScheduleOrder = row["ScheduleOrder"].ToString();

                        // 홈광고 편성내역 삭제 서비스를 호출한다.
                        new GroupOrganizationManager(systemModel, commonModel).SetSchHomeAdDelete(groupOrganizationModel);

                        if (groupOrganizationModel.ResultCD.Equals("0000"))
                        {
                            SetCount++;
                        }
                    }
                }

                if (SetCount > 0)
                {
                    reGroupCode = groupCode;
                    keyScheduleOrder = groupOrganizationModel.ScheduleOrder;
                    ResetDetailText();
                    DisableButton();
                    SearchGroupList();
                    GroupListChoice();
                    InitButton();

                    StatusMessage("홈광고 편성내역이 삭제되었습니다.");
                }
                else
                {
                    MessageBox.Show("선택된 홈광고 편성내역이 없습니다.", "홈광고편성",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (FrameException fe)
            {
                FrameSystem.showMsgForm("홈광고 편성내역삭제오류", new string[] { fe.ErrCode, fe.ResultMsg });
            }
            catch (Exception ex)
            {
                FrameSystem.showMsgForm("홈광고 편성내역 삭제오류", new string[] { "", ex.Message });
            }
        }
        
        /// <summary>
        /// 셋탑구분여부 편성여부변경
        /// </summary>
        /// <param name="LogYn"></param>
        private void SetSchHomeCommonYn(int CommonYn)
        {
            try
            {
                StatusMessage("홈광고 편성 적용방식을 변경합니다.");

                int SetCount = 0;

                for (int i = 0; i < groupOrganizationDs.AdSchedule.Rows.Count; i++)
                {
                    DataRow row = groupOrganizationDs.AdSchedule.Rows[i];

                    if (row["CheckYn"].ToString().Equals("True"))
                    {
                        SetCount++;
                    }
                }

                if (SetCount > 0)
                {
                    DialogResult result;

                    if (CommonYn == 1)
                    {
                        result = MessageBox.Show("해당 홈광고 적용형식을 공용(셀론/현대/삼성)셋탑으로 변경합니다.", "홈광고 편성", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
                    }
                    else
                    {
                        result = MessageBox.Show("해당 홈광고 적용형식을 일반(셀론/현대)셋탑으로 변경합니다.", "홈광고 편성", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
                    }

                    if (result == DialogResult.No) return;
                }
                else
                {
                    MessageBox.Show("작업을 위해선 홈광고 앞의 체크박스를 선택해야 합니다", "홈광고편성", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                for (int i = 0; i < groupOrganizationDs.AdSchedule.Rows.Count; i++)
                {
                    DataRow row = groupOrganizationDs.AdSchedule.Rows[i];

                    if (row["CheckYn"].ToString().Equals("True"))
                    {
                        // 데이터모델에 전송할 내용을 셋트한다.
                        groupOrganizationModel.Init();
                        groupOrganizationModel.GroupCode = row["GroupCode"].ToString();
                        groupOrganizationModel.ScheduleOrder = row["ScheduleOrder"].ToString();
                        groupOrganizationModel.LogYn = CommonYn;
                        groupOrganizationModel.CommonYn = CommonYn;

                        // 홈광고 편성내역 Update 서비스를 호출한다.
                        new GroupOrganizationManager(systemModel, commonModel).SetSchHomeAdCommonYn(groupOrganizationModel);

                        if (groupOrganizationModel.ResultCD.Equals("0000"))
                        {
                            SetCount++;
                        }
                    }
                }

                if (SetCount > 0)
                {
                    keyScheduleOrder = groupOrganizationModel.ScheduleOrder;
                    ResetDetailText();
                    DisableButton();
                    SearchScheduleHomeAd();
                    InitButton();

                    StatusMessage("홈광고 편성 적용방식이 변경되었습니다.");
                }
            }
            catch (FrameException fe)
            {
                FrameSystem.showMsgForm("홈광고 편성내역 설정오류", new string[] { fe.ErrCode, fe.ResultMsg });
            }
            catch (Exception ex)
            {
                FrameSystem.showMsgForm("홈광고 편성내역 설정오류", new string[] { "", ex.Message });
            }
        }

        /// <summary>
        /// 홈광고 로그 설정및 해제
        /// </summary>
        /// <param name="LogYn"></param>
        private void SetLogYnSchHome(int LogYn)
        {
            try
            {
                StatusMessage("홈광고 로그설정을 변경합니다.");

                int SetCount = 0;

                for (int i = 0; i < groupOrganizationDs.AdSchedule.Rows.Count; i++)
                {
                    DataRow row = groupOrganizationDs.AdSchedule.Rows[i];

                    if (row["CheckYn"].ToString().Equals("True"))
                    {
                        SetCount++;
                    }
                }

                if (SetCount > 0)
                {
                    DialogResult result = MessageBox.Show("해당 홈광고 편성내역을 로그를 변경하시겠습니까?", "홈광고 편성내역의 로그",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Question,
                        MessageBoxDefaultButton.Button2);

                    if (result == DialogResult.No) return;

                }
                else
                {
                    MessageBox.Show("작업을 위해선 홈광고 앞의 체크박스를 선택해야 합니다", "홈광고편성", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                for (int i = 0; i < groupOrganizationDs.AdSchedule.Rows.Count; i++)
                {
                    DataRow row = groupOrganizationDs.AdSchedule.Rows[i];

                    if (row["CheckYn"].ToString().Equals("True"))
                    {
                        // 데이터모델에 전송할 내용을 셋트한다.
                        groupOrganizationModel.Init();
                        groupOrganizationModel.GroupCode = row["GroupCode"].ToString();
                        groupOrganizationModel.ItemNo = row["ItemNo"].ToString();
                        groupOrganizationModel.ItemName = row["ItemName"].ToString();
                        groupOrganizationModel.ScheduleOrder = row["ScheduleOrder"].ToString();
                        groupOrganizationModel.LogYn = LogYn;

                        // 홈광고 편성내역 Update 서비스를 호출한다.
                        new GroupOrganizationManager(systemModel, commonModel).SetSchHomeAdLogYn(groupOrganizationModel);

                        if (groupOrganizationModel.ResultCD.Equals("0000"))
                        {
                            SetCount++;
                        }
                    }
                }

                if (SetCount > 0)
                {
                    keyScheduleOrder = groupOrganizationModel.ScheduleOrder;
                    ResetDetailText();
                    DisableButton();
                    SearchScheduleHomeAd();
                    InitButton();

                    StatusMessage("홈광고 로그설정이 변경되었습니다.");
                }
            }
            catch (FrameException fe)
            {
                FrameSystem.showMsgForm("홈광고 편성내역로그설정오류", new string[] { fe.ErrCode, fe.ResultMsg });
            }
            catch (Exception ex)
            {
                FrameSystem.showMsgForm("홈광고 편성내역로그설정오류", new string[] { "", ex.Message });
            }

        }

        /// <summary>
        /// 홈광고 편성내역 순위변경
        /// </summary>
        private void OrderSetScheduleHomeAd(int OrderSet)
        {
            try
            {
                StatusMessage("홈광고 편성내역의 편성순위를 변경합니다.");

                int SetCount = 0;

                for (int i = 0; i < groupOrganizationDs.AdSchedule.Rows.Count; i++)
                {
                    DataRow row = groupOrganizationDs.AdSchedule.Rows[i];

                    if (row["CheckYn"].ToString().Equals("True"))
                    {
                        SetCount++;
                    }
                }
                if (SetCount <= 0)
                {
                    MessageBox.Show("변경할 홈광고 편성내역이 선택되지 않았습니다.", "홈광고 편성내역 순위변경", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                
                int curRow = cmDetail.Position;

                // 데이터모델에 전송할 내용을 셋트한다.
                groupOrganizationModel.Init();
                groupOrganizationModel.GroupCode = groupCode;
                groupOrganizationModel.MediaCode = keyMediaCode;
                groupOrganizationModel.ItemNo = dtDetail.Rows[curRow]["ItemNo"].ToString();
                groupOrganizationModel.ItemName = keyItemName;
                groupOrganizationModel.ScheduleOrder = dtDetail.Rows[curRow]["ScheduleOrder"].ToString();

                int NowOrder = Convert.ToInt32(dtDetail.Rows[curRow]["ScheduleOrder"].ToString());
                int LastOrder = Convert.ToInt32(keyLastOrder);

                switch (OrderSet)
                {
                    case ORDER_FIRST:
                        if (NowOrder <= 1)
                        {
                            MessageBox.Show("해당 홈광고 편성내역이 첫번째 순위입니다.", "홈광고 편성내역 순위변경", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return;
                        }
                        break;
                    case ORDER_UP:
                        if (NowOrder <= 1)
                        {
                            MessageBox.Show("해당 홈광고 편성내역이 첫번째 순위입니다.", "홈광고 편성내역 순위변경", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return;
                        }
                        break;
                    case ORDER_DOWN:
                        if (NowOrder >= LastOrder)
                        {
                            MessageBox.Show("해당 홈광고 편성내역이 마지막 순위입니다.", "홈광고 편성내역 순위변경", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return;
                        }
                        break;
                    case ORDER_LAST:
                        if (NowOrder >= LastOrder)
                        {
                            MessageBox.Show("해당 홈광고 편성내역이 마지막 순위입니다.", "홈광고 편성내역 순위변경", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return;
                        }
                        break;
                }

                // 홈광고 편성내역 순서변경 서비스를 호출한다.
                new GroupOrganizationManager(systemModel, commonModel).SetSchHomeAdOrderSet(groupOrganizationModel, OrderSet);

                keyScheduleOrder = groupOrganizationModel.ScheduleOrder;
                StatusMessage("홈광고 편성내역의 순위가 변경되었습니다.");

                ResetDetailText();
                SearchScheduleHomeAd();
            }
            catch (FrameException fe)
            {
                FrameSystem.showMsgForm("홈광고 편성내역 순위변경 오류", new string[] { fe.ErrCode, fe.ResultMsg });
            }
            catch (Exception ex)
            {
                FrameSystem.showMsgForm("홈광고 편성내역 순위변경 오류", new string[] { "", ex.Message });
            }
        }

        /// <summary>
		/// 그룹목록 상세정보의 셋트
		/// </summary>
		private void SetGroupDetailText()
		{
			int curRow = cmGroup.Position;

			if(curRow >= 0)
			{
                groupCode = dtGroup.Rows[curRow]["GroupCode"].ToString();
                ebGroupName.Text = dtGroup.Rows[curRow]["GroupName"].ToString();
                ebComment.Text = dtGroup.Rows[curRow]["Comment"].ToString();
                keyScheduleOrderSet = dtGroup.Rows[curRow]["SchCount"].ToString();

                string UseYn = dtGroup.Rows[curRow]["UseYn"].ToString();
				if(UseYn.Equals("Y"))
				{
					rbUseYn_Y.Checked = true;
					rbUseYn_N.Checked = false;
				}
				else
				{
					rbUseYn_Y.Checked = false;
					rbUseYn_N.Checked = true;
				}			
				IsAdding = false;
				ResetTextReadonly();
			}
			StatusMessage("준비");
		}

        /// <summary>
        /// 광고파일 상세정보의 셋트
        /// </summary>
        private void SetDetailText()
        {
            int curRow = cmDetail.Position;

            if (curRow >= 0)
            {
                keyItemNo = dtDetail.Rows[curRow]["ItemNo"].ToString();
                keyItemName = dtDetail.Rows[curRow]["ItemName"].ToString();
                keyScheduleOrder = dtDetail.Rows[curRow]["ScheduleOrder"].ToString();

                if (canCreate)
                {
                    btnAdd.Enabled = true;
                    btnAddCm.Enabled = true;
                }
                if (canDelete)
                {
                    btnDelete.Enabled = true;
                }
                if (canUpdate)
                {
                    btnOrderUp.Enabled = true;
                    btnOrderDown.Enabled = true;
                    btnOrderFirst.Enabled = true;
                    btnOrderLast.Enabled = true;

                    btnOk.Enabled = true;
                    btnNot.Enabled = true;

                    if (Convert.ToInt32(dtDetail.Rows[curRow]["FileSize"].ToString()) == 0)
                    {
                        gbSTB.Enabled = false;
                    }
                    else
                    {
                        gbSTB.Enabled = true;
                    }
                }
            }
            Application.DoEvents();

            StatusMessage("준비");
        }

        /// <summary>
        /// 키캆을찾아 그룹목록 그리드 키에 해당되는로우로..
        /// </summary>
        private void GroupListChoice()
        {
            int rowIndex = 0;

            if (dtGroup.Rows.Count < 1) return;

            foreach (DataRow row in dtGroup.Rows)
            {
                if (row["GroupCode"].ToString().Equals(reGroupCode))
                {
                    cmGroup.Position = rowIndex;
                    break;
                }
                rowIndex++;
            }
        }

        /// <summary>
		/// 키캆을찾아 홈광고편성 그리드 키에 해당되는로우로..
		/// </summary>
		private void AddSchChoice()
		{
			int rowIndex = 0;

			if ( dtDetail.Rows.Count < 1 ) return;

            foreach (DataRow row in dtDetail.Rows)
			{					
				if(row["ScheduleOrder"].ToString().Equals(keyScheduleOrder))
				{					
					cmDetail.Position = rowIndex;
					break;								
				}				
				rowIndex++;
			}
		}

        /// <summary>
        /// 상세정보 수정가능케
        /// </summary>
        private void ResetTextReadonly()
        {
            ebGroupName.ReadOnly = false;
            ebComment.ReadOnly = false;
            // 사용자구분이 어드민 또는 수퍼유저인 경우만 사용레벨, 사용자구분, 사용여부를 수정할 수 있다.
            if (commonModel.UserClass.Equals("10") || commonModel.UserClass.Equals("20"))
            {
                rbUseYn_Y.Enabled = true;
                rbUseYn_N.Enabled = true;
            }

            ebGroupName.BackColor = Color.White;
            ebComment.BackColor = Color.White;
        }

        private void ResetDetailText()
        {
            keyItemNo = "";
            keyItemName = "";
        }

        /// <summary>
        /// 그룹 텍스트박스 초기화
        /// </summary>
        private void ResetGroupText()
        {
            ebGroupName.Text = "";
            ebComment.Text = "";
            rbUseYn_Y.Checked = true;
            rbUseYn_N.Checked = false;

            if (!IsAdding)
            {
                ebGroupName.ReadOnly = true;
                ebComment.ReadOnly = true;
                ebGroupName.BackColor = Color.WhiteSmoke;
                ebComment.BackColor = Color.WhiteSmoke;
            }
        }

        /// <summary>
        /// 셋탑 FileSize
        /// </summary>
        private void myCaclFileSize()
        {
            try
            {
                _fileDefaultSize = 0;
                _fileDefaultCnt = 0;
                _fileSamSungSize = 0;
                _fileSamSungCnt = 0;
                _fileKaonSize = 0;
                _fileKaonCnt = 0;

                for (int inx = 0; inx < groupOrganizationDs.AdSchedule.Rows.Count; inx++)
                {
                    if (Convert.ToInt16(groupOrganizationDs.AdSchedule.Rows[inx]["CommonYn"].ToString()) == 0)
                    {
                        _fileDefaultSize += Convert.ToInt32(groupOrganizationDs.AdSchedule.Rows[inx]["FileSize"].ToString());
                        _fileDefaultCnt++;

                        _fileSamSungSize += Convert.ToInt32(groupOrganizationDs.AdSchedule.Rows[inx]["FileSize"].ToString());
                        _fileSamSungCnt++;

                        _fileKaonSize += Convert.ToInt32(groupOrganizationDs.AdSchedule.Rows[inx]["FileSize"].ToString());
                        _fileKaonCnt++;
                    }
                    else if (Convert.ToInt16(groupOrganizationDs.AdSchedule.Rows[inx]["CommonYn"].ToString()) == 1)
                    {
                        _fileDefaultSize += Convert.ToInt32(groupOrganizationDs.AdSchedule.Rows[inx]["FileSize"].ToString());
                        _fileDefaultCnt++;
                    }
                    else if (Convert.ToInt16(groupOrganizationDs.AdSchedule.Rows[inx]["CommonYn"].ToString()) == 2)
                    {
                        _fileDefaultSize += Convert.ToInt32(groupOrganizationDs.AdSchedule.Rows[inx]["FileSize"].ToString());
                        _fileDefaultCnt++;
                        _fileSamSungSize += Convert.ToInt32(groupOrganizationDs.AdSchedule.Rows[inx]["FileSize"].ToString());
                        _fileSamSungCnt++;
                    }
                }

                lbl_SizeDefault.Text = _fileDefaultSize.ToString("##,##0");
                lbl_CntDefault.Text = _fileDefaultCnt.ToString();

                lbl_SizeSamSung.Text = _fileSamSungSize.ToString("##,##0");
                lbl_CntSamSung.Text = _fileSamSungCnt.ToString();

                lbl_SizeKaon.Text = _fileKaonSize.ToString("##,##0");
                lbl_CntKaon.Text = _fileKaonCnt.ToString();
            }
            catch (Exception ex)
            {
                lbl_SizeDefault.Text = "0";
                lbl_CntDefault.Text = "0";

                lbl_SizeSamSung.Text = "0";
                lbl_CntSamSung.Text = "0";

                lbl_SizeKaon.Text = "0";
                lbl_CntKaon.Text = "0";

                FrameSystem.showMsgForm("홈광고 파일사이즈 계산", new string[] { "", ex.Message });
            }
        }
        
        #endregion

    }
}