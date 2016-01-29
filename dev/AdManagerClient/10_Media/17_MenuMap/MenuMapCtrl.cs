// ===============================================================================
// MenuMapCtrl 
//
// MenuMapCtrl.cs
//
// 메뉴 매핑 관리
//
// ===============================================================================
// Release history
// 최초 작성 2015.10.26 yungseok.Jang
// ===============================================================================
// Copyright (C) 2054 Dartmedia co..
// All rights reserved.
// 
using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.Diagnostics;

using WinFramework.Misc;
using WinFramework.Base;
using WinFramework.Data;

using AdManagerModel;
using AdManagerClient.Common.Args;
using Janus.Windows.GridEX;
using System.Text;
using System.Text.RegularExpressions;

namespace AdManagerClient
{
	public partial class MenuMapCtrl : UserControl , IUserControl
	{
		#region 이벤트핸들러 및 함수
		public event StatusEventHandler StatusEvent;			// 상태이벤트 핸들러
		public event ProgressEventHandler ProgressEvent;	    // 처리중이벤트 핸들러

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
		public string menuCode = "";

		// 화면처리용 변수
		CurrencyManager cmCategory  = null;
        CurrencyManager cmMenuMap   = null;
        CurrencyManager cmMenuStd   = null;
        CurrencyManager cmMenu4     = null;
		DataTable dtCategory    = null;
        DataTable dtMenuMap     = null;
        DataTable dtMenuStd     = null;
        DataTable dtMenu4       = null;

		// 사용권한
		bool IsSearching = false;       // 조회중 상세화면이 업데이트 되는 것을 방지 하기위함
		bool IsNewSearchKey = true;		// 검색어입력 여부
		bool IsAdding = false;
        bool IsMapping = false;         // 매핑 작업중인지 여부(true : 메뉴 매핑, false : 매핑 해제)
        bool IsEditingStart = false;    // 매핑 편집 작업 시작 여부( 조회를 이용 리스트 초기화시 같이 초기화)

		bool canCreate = false;
		bool canRead = false;
		bool canUpdate = false;
		bool canDelete = false;
		
        // 여기부터 사용자가 설정하는 오브젝트 및 변수들
		// 사용할 정보모델
		MenuMapModel model = new MenuMapModel();

		int keyCategory = 0;    // 메뉴코드 조회용 카테고리ID
        string upperMenuCode4 = string.Empty; // UI4.0 정보의 UpperMenuCode

        private enum Version
        {
            Standard
            , ver4
        }

		#endregion

		public MenuMapCtrl()
		{
			InitializeComponent();
		}

		#region 컨트롤 로드
		private void MenuMapCtrl_Load(object sender, System.EventArgs e)
		{
			// 데이터관리용 객체생성
			dtCategory = ((DataView)grdCategory.DataSource).Table;
			cmCategory = (CurrencyManager)this.BindingContext[grdCategory.DataSource];
            grdCategory.Click += new EventHandler(OnGrdRowClickChangedCategory);

            dtMenuMap = ((DataView)grdMenuMap.DataSource).Table;
            cmMenuMap = (CurrencyManager)this.BindingContext[grdMenuMap.DataSource];
            grdMenuMap.Click += new EventHandler(OnGrdRowClickChangedMenuMap);

            dtMenuStd = ((DataView)grdNoneMenuStd.DataSource).Table;
            cmMenuStd = (CurrencyManager)this.BindingContext[grdNoneMenuStd.DataSource];
            grdNoneMenuStd.Click += new EventHandler(OnGrdRowClickChangedMenuStd);

            dtMenu4 = ((DataView)grdNoneMenu4.DataSource).Table;
            cmMenu4 = (CurrencyManager)this.BindingContext[grdNoneMenu4.DataSource];
            grdNoneMenu4.Click += new EventHandler(OnGrdRowClickChangedMenu4);

			// 컨트롤 초기화
			InitControl();
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


			if (canRead)
			{
				SearchCategory();
			}
		}

		private void InitButton()
		{
			if (canRead) btnSearch.Enabled = true;

            // 매핑목록, 기준정보, UI4.0 중 한곳을 클릭해야 편집 시작으로 간주, 버튼 기능 활성화
            if (IsEditingStart)
            {
                if (IsMapping)
                {
                    // UI4.0 정보만 선택된 경우 활성 이유는 기준 메뉴를 신규로 생성하고 맵핑하는 처리를 위해
                    if (!ebMenuCode4.Text.Equals(string.Empty)
                        || (!ebMenuCode.Text.Equals(string.Empty) && !ebMenuCode4.Text.Equals(string.Empty)))
                    {
                        btnMapping.Enabled = true;
                    }
                }
                else
                {
                    btnDelete.Enabled = true;
                }
            }
            
			Application.DoEvents();
		}

		private void DisableButton()
		{
			btnSearch.Enabled   = false;
            btnMapping.Enabled  = false;
            btnDelete.Enabled    = false;
			Application.DoEvents();
		}
		#endregion

		#region 처리 메소드
		/// <summary>
		/// 1번 그리드인 카테고리목록을 가져온다
		/// </summary>
		private void SearchCategory()
		{
			IsSearching = true;
			StatusMessage("카테고리 정보를 조회합니다.");

			try
			{
				ProgressStart();
				
				model.Init();
				menuMapDs.Category.Clear();
				menuMapDs.MenuMap.Clear();
                menuMapDs.MenuStd.Clear();
                menuMapDs.Menu4.Clear();
				
				// 슬롯 세팅 현황 목록 조회 서비스를 호출한다.
				new MenuMapManager(systemModel, commonModel).GetCategoryList(model);

				if (model.ResultCD.Equals("0000"))
				{
					Utility.SetDataTable(menuMapDs.Category, model.CategoryDs);

					StatusMessage(model.ResultCnt + "건의 카테고리 정보가 조회되었습니다.");
					grdCategory.Focus();
				}
			}
			catch (FrameException fe)
			{
				FrameSystem.showMsgForm("카테고리 조회오류", new string[] { fe.ErrCode, fe.ResultMsg });
			}
			catch (Exception ex)
			{
				FrameSystem.showMsgForm("카테고리 조회오류", new string[] { "", ex.Message });
			}
			finally
			{
				IsSearching = false; // 조회중 Flag 리셋
				ProgressStop();
			}
		}

        /// <summary>
        /// 1번 그리드인 카테고리목록을 가져온다
        /// </summary>
        /// <param name="position">카테고리 포지션</param>
        private void SearchCategory(int position)
        {
            SearchCategory();

            cmCategory.Position = position;
        }

        /// <summary>
        /// 2번 그리드 그룹용 메뉴매핑 목록을 가져온다.
        /// </summary>
		private void SearchMenuMap()
		{
            IsSearching = true;
            StatusMessage("메뉴매핑 정보를 조회합니다.");

            try
            {
                ProgressStart();

                model.Init();
                menuMapDs.MenuMap.Clear();
                menuMapDs.MenuStd.Clear();
                menuMapDs.Menu4.Clear();

                model.SearchCategory = keyCategory.ToString();
                // 데이터 입력및 조회 화면 초기화
                //ResetDetail();

                // 슬롯 세팅 현황 목록 조회 서비스를 호출한다.
                new MenuMapManager(systemModel, commonModel).GetMenuMapList(model);

                if (model.ResultCD.Equals("0000"))
                {
                    SetDataTable(menuMapDs.MenuMap, model.MenuMapDs, "MapCode4 Is Not Null and MapCode Is Not Null");
                    SetDataTable(menuMapDs.MenuStd, model.MenuMapDs, "MenuCode Is Not Null and MenuCode4 Is Null");
                    SetDataTable(menuMapDs.Menu4, model.MenuMapDs  , "MenuCode4 Is Not Null and MenuCode Is Null");

                    // SetDataTable의 Select 과정에서 그리드의 포커스 Row가 바뀌는 경우가 있어 임의로 포커스 Row를 0으로 지정
                    if (grdMenuMap.RowCount > 0)        grdMenuMap.Row = 0;
                    if (grdNoneMenu4.RowCount > 0)      grdNoneMenu4.Row = 0;
                    if (grdNoneMenuStd.RowCount > 0)    grdNoneMenuStd.Row = 0;

                    StatusMessage(model.ResultCnt + "건의 메뉴매핑 정보가 조회되었습니다.");
                    grdMenuMap.Focus();
                }
            }
            catch (FrameException fe)
            {
                FrameSystem.showMsgForm("메뉴매핑 조회오류", new string[] { fe.ErrCode, fe.ResultMsg });
            }
            catch (Exception ex)
            {
                FrameSystem.showMsgForm("메뉴매핑 조회오류", new string[] { "", ex.Message });
            }
            finally
            {
                IsSearching = false; // 조회중 Flag 리셋
                ProgressStop();
            }
		}

        /// <summary>
        /// 2번 그리드 그룹용 메뉴매핑 목록을 가져온다.
        /// </summary>
        /// <param name="grd1Position">매핑 목록 그리드</param>
        /// <param name="grd2Position">기준 정보 그리드</param>
        /// <param name="grd3Position">UI4.0 정보 그리드</param>
        private void SearchMenuMap(int grd1Position, int grd2Position, int grd3Position)
        {
            SearchMenuMap();

            cmMenuMap.Position = grd1Position;
            cmMenuStd.Position = grd2Position;
            cmMenu4.Position = grd3Position;
        }

        /// <summary>
        /// 메뉴패밍 해제
        /// </summary>
        /// <returns>true : 해제 성공, false : 해제 실패</returns>
        private bool DeleteMenuMap()
        {
            StatusMessage("메뉴매핑 정보를 삭제합니다.");
            
            // 매핑 목록 Row 선택시에만 해제 버튼이 활성화하기 때문에 데이터 검증 과정 없음
            DialogResult result = MessageBox.Show("해당 메뉴매핑 정보를 해제 하시겠습니까?", "매핑 해제",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question,
                MessageBoxDefaultButton.Button2);

            if (result == DialogResult.No) return false;

            try
            {
                model.Init();

                // 데이터모델에 전송할 내용을 셋트한다.
                model.AdGenre = ebAdGenre.Text.Trim();

                // 사용자 상세정보 저장 서비스를 호출한다.
                new MenuMapManager(systemModel, commonModel).SetMenuMapDelete(model);

                ResetDetail();
                StatusMessage("메뉴매핑 설정이 해제 되었습니다.");

                return true;
            }
            catch (FrameException fe)
            {
                FrameSystem.showMsgForm("메뉴정보 해제 오류", new string[] { fe.ErrCode, fe.ResultMsg });
                return false;
            }
            catch (Exception ex)
            {
                FrameSystem.showMsgForm("메뉴정보 해제 오류", new string[] { "", ex.Message });
                return false;
            }
        }

        /// <summary>
        /// 메뉴매핑 정보 저장
        /// </summary>
        /// <returns>true : 저장 성공, false : 저장 실패</returns>
        private bool SaveMenuMapSetDetail()
        {
            //IsAdding = true;

            StatusMessage("메뉴매핑 정보를 저장합니다.");

            // 기준정보, UI4.0 목록에서 각각 Row 선택시(둘 다 선택)에만 매핑 버튼이 활성화하기 때문에 데이터 검증 과정 없음
            
            // 메뉴명이 다른 경우는 매핑 설정 확인
            if(!CreateComparisonString(ebMenuName.Text).Equals(CreateComparisonString(ebMenuName4.Text)))
            {
                DialogResult result = MessageBox.Show("해당 메뉴명이 다릅니다. 매핑 하시겠습니까?", "매핑 설정",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question,
                MessageBoxDefaultButton.Button2);

                if (result == DialogResult.No) return false;
            }

            try
            {
                //저장 전에 모델을 초기화 해준다.
                model.Init();
                // 데이터모델에 전송할 내용을 셋트한다.
                model.MenuCode4 = ebMenuCode4.Text;
                model.AdGenre = ebAdGenre.Text;                       

                // 채널 상세정보 저장 서비스를 호출한다.
                if (IsAdding)
                {
                    new MenuMapManager(systemModel, commonModel).SetMenuMapCreate(model);
                    StatusMessage("메뉴매핑 정보가 설정되었습니다.");
                    IsAdding = false;
                    ResetDetail();
                }
                else
                {
                    new MenuMapManager(systemModel, commonModel).SetMenuMapUpdate(model);
                    StatusMessage("메뉴매핑 정보가 수정되었습니다.");
                    ResetDetail();
                }

                return true;
            }
            catch (FrameException fe)
            {
                FrameSystem.showMsgForm("메뉴매핑정보 저장오류", new string[] { fe.ErrCode, fe.ResultMsg });
                return false;
            }
            catch (Exception ex)
            {
                FrameSystem.showMsgForm("메뉴매핑정보 저장오류", new string[] { "", ex.Message });
                return false;
            }
        }

        /// <summary>
        /// 기준 메뉴 생성 ( UI4.0 메뉴 정보를 사용 )
        /// </summary>
        /// <returns>true : 추가 성공, false : 추가 실패</returns>
        private bool CreateMenuOfMenu4()
        {
            StatusMessage("기준 메뉴 정보를 저장합니다.");

            DialogResult result = MessageBox.Show("기준 메뉴 정보가 없습니다. UI4.0 메뉴를 기준 메뉴로 등록 후 맵핑하시겠습니까?", "매핑 설정",
            MessageBoxButtons.YesNo, MessageBoxIcon.Question,
            MessageBoxDefaultButton.Button2);

            if (result == DialogResult.No) return false;

            try
            {
                //저장 전에 모델을 초기화 해준다.
                model.Init();
                // 데이터모델에 전송할 내용을 셋트한다.
                model.MenuCode4 = ebMenuCode4.Text;
                model.MenuName4 = ebMenuName4.Text;
                model.UpperMenuCode4 = upperMenuCode4;

                new MenuMapManager(systemModel, commonModel).SetMenuCreate(model);
                StatusMessage("기준 메뉴 정보가 추가되었습니다.");

                return true;
            }
            catch (FrameException fe)
            {
                FrameSystem.showMsgForm("기준 메뉴 추가오류", new string[] { fe.ErrCode, fe.ResultMsg });
                return false;
            }
            catch (Exception ex)
            {
                FrameSystem.showMsgForm("기준 메뉴 추가오류", new string[] { "", ex.Message });
                return false;
            }
        }

        /// <summary>
        /// 조건에 맞는 데이터 추출 DataTable에 셋팅
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="ds"></param>
        /// <param name="option">조건</param>
        private void SetDataTable (DataTable dt, DataSet ds, string option)
        {
            dt.Clear();

            foreach (DataRow row in ds.Tables[0].Select(option))
            {   
                dt.ImportRow(row);
            }
        }

        /// <summary>
        /// 메뉴맵핑 상세정보의 셋트
        /// </summary>
        private void SetMapDetailText()
        {
            int curRow = cmMenuMap.Position;
            
            if (curRow >= 0)
            {
                SetMenuStdText(dtMenuMap.Rows[curRow]["MenuCode"].ToString()
                              ,dtMenuMap.Rows[curRow]["MenuName"].ToString()
                              ,dtMenuMap.Rows[curRow]["AdGenre"].ToString());
                
                SetMenu4Text(dtMenuMap.Rows[curRow]["MenuCode4"].ToString()
                            ,dtMenuMap.Rows[curRow]["MenuName4"].ToString());
                upperMenuCode4 = dtMenuMap.Rows[curRow]["UpperMenu4"].ToString();
            }
            StatusMessage("준비");
        }

        /// <summary>
        /// 메뉴맵핑 상세정보의 셋트
        /// </summary>
        private void SetDetailText(Version version)
        {
            int curRow;

            if (version.Equals(Version.Standard))
            {
                curRow = cmMenuStd.Position;

                if (curRow >= 0)
                {
                    SetMenuStdText(dtMenuStd.Rows[curRow]["MenuCode"].ToString()
                                  ,dtMenuStd.Rows[curRow]["MenuName"].ToString()
                                  ,dtMenuStd.Rows[curRow]["AdGenre"].ToString());

                    // MapCode가 빈문자열인 경우 기존 정보가 없으므로 추가, 그 외에는 업데이트
                    if (dtMenuStd.Rows[curRow]["MapCode"].ToString().Equals(string.Empty))
                    {
                        IsAdding = true; // 추가
                    }
                    else
                    {
                        IsAdding = false; // 업데이트
                    }
                }
            }
            else if (version.Equals(Version.ver4))
            {
                curRow = cmMenu4.Position;

                if (curRow >= 0)
                {
                    SetMenu4Text(dtMenu4.Rows[curRow]["MenuCode4"].ToString()
                                ,dtMenu4.Rows[curRow]["MenuName4"].ToString());
                    upperMenuCode4 = dtMenuMap.Rows[curRow]["UpperMenu4"].ToString();

                    ScrollToCurrent(dtMenu4.Rows[curRow]["MenuName4"].ToString());
                }
            }

            StatusMessage("준비");
        }

        /// <summary>
        /// 메뉴코드 Ver. Std 상세정보 셋팅
        /// </summary>
        /// <param name="code">장르</param>
        /// <param name="name">메뉴명</param>
        /// <param name="adGenre">메뉴코드</param>
        private void SetMenuStdText(string code, string name, string adGenre)
        {
            ebMenuCode.Text = code;
            ebMenuName.Text = name;
            ebAdGenre.Text = adGenre;
        }

        /// <summary>
        /// 메뉴코드 Ver. 4.0 상세정보 셋팅
        /// </summary>
        /// <param name="code">메뉴코드</param>
        /// <param name="name">메뉴명</param>
        private void SetMenu4Text(string code, string name)
        {
            ebMenuCode4.Text = code;
            ebMenuName4.Text = name;
        }

        private void SetDetailList()
        {
            int curRow = cmCategory.Position;
            if (curRow < 0) return;

            keyCategory = Convert.ToInt32(dtCategory.Rows[curRow]["category"].ToString());
            // 광고파일을 조회한다.
            SearchMenuMap();
        }

        private void ResetDetail()
        {
            ResetDetailStd();
            ResetDetail4();
        }

        private void ResetDetailStd()
        {
            ebMenuCode.Text = string.Empty;
            ebMenuName.Text = string.Empty;
            ebAdGenre.Text = string.Empty;
        }

        private void ResetDetail4()
        {
            ebMenuCode4.Text = string.Empty;
            ebMenuName4.Text = string.Empty;
            upperMenuCode4   = string.Empty; 
        }

        /// <summary>
        /// 비교용 문자열 생성
        /// 1. 특수문자 모두 제거
        /// 2. 영문 전부 대문자로 전환
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        private string CreateComparisonString(string str)
        {
            string result = string.Empty;

            result = Regex.Replace(str, @"[^a-zA-Z0-9가-힣]", "", RegexOptions.Singleline).ToUpper();

            return result;
        }

        /// <summary>
        /// 기본 정보 리스트에서 메뉴명이 UI4.0과 동일한 Row를 찾는다.
        /// </summary>
        /// <param name="name">찾을 메뉴명</param>
        /// <param name="version">찾을 그리드</param>
        private void ScrollToCurrent(string name)
        {
            DataTable dt = dtMenuStd;
            CurrencyManager cm = cmMenuStd;
            GridEX grd = grdNoneMenuStd;

            // 특수문자가 제거된 메뉴명으로 매핑 대상 목록에서 같은 메뉴명의 Row 검색
            var equalRows = from p in dt.AsEnumerable()
                            where CreateComparisonString(p.Field<string>("MenuName")).Equals(CreateComparisonString(name))
                            select new
                            {
                                MenuCode = p.Field<int>("MenuCode")
                            };

            bool IsOnlyRow = false; // 같은 메뉴명을 가진 Row가 유일한지 여부
            int equalMenuCode = 0;  // 매핑 대상 목록의 동일한 메뉴명을 가진 Row의 메뉴코드

            foreach (var equalRow in equalRows)
            {
                // 같은 메뉴명을 가진 데이터가 여러개인 경우 처리 중단
                if (!IsOnlyRow)
                {
                    IsOnlyRow = true;
                    equalMenuCode = equalRow.MenuCode;
                }
                else
                {
                    return;
                }
            }

            int index = 0;                  // Row Position을 이동시킬 Index

            // 대상을 검색하므로 상세 정보를 제거한다.
            ResetDetailStd();

            // 대상 목록이 없는 경우 처리 중단
            if (grd.RowCount > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    // 메뉴 코드가 같은지 확인 현재 선택되어 있는 Row Position과 동일한 경우 처리 중단
                    if (row.Field<int>("MenuCode").Equals(equalMenuCode))
                    {
                        cm.Position = index;

                        OnGrdRowClickChangedMenuStd(null, null);
                    }
                    index++;
                }
            }
            else
            {
                return;
            }

            grd.EnsureVisible();
        }
		#endregion

        #region 이벤트
        private void btnSearch_Click(object sender, EventArgs e)
		{
            // 카테고리 리스트 재조회는 모든 기능 초기화로 간주 편집중 여부도 초기화
            IsEditingStart = false;

			ResetDetail();
			DisableButton();
			SearchCategory();
            InitButton();
		}

        private void btnMapping_Click(object sender, EventArgs e)
        {
            // UI4.0의 메뉴 중 기준 메뉴에 매핑할 메뉴가 없는 경우에 처리( UI4.0 메뉴 정보를 기준으로 기준 정보를 생성한다.
            if (ebMenuCode.Text.Equals(string.Empty))
            {
                // 기준 메뉴 생성을 취소 하거나 어떤 오류로 인한 중단 발생시 처리 중지
                if (CreateMenuOfMenu4())
                {
                    int menuMapPosition = cmMenuMap.Position;
                    int menuStdPosition = cmMenuStd.Position;
                    int menu4Position = cmMenu4.Position;

                    SearchMenuMap(menuMapPosition, menuStdPosition, menu4Position);

                    ScrollToCurrent(ebMenuName4.Text);

                    OnGrdRowClickChangedMenuStd(null, null);
                }
                else
                {
                    return;
                }
            }
                
            bool result = SaveMenuMapSetDetail();

            if (result)
            {
                IsEditingStart = false; // 메뉴매핑 작업이 완료 되었기 때문에 작업 상태를 종료로 전환

                ProgressStart();

                int categoryPosition = cmCategory.Position;

                DisableButton();
                SearchCategory(categoryPosition);
                SearchMenuMap();
                InitButton();

                ProgressStop();
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            bool result = DeleteMenuMap();
            
            if (result)
            {
                IsEditingStart = false; // 메뉴매핑 해제 작업이 완료 되었기 때문에 작업 상태를 종료로 전환

                ProgressStart();

                int categoryPosition = cmCategory.Position;

                DisableButton();
                SearchCategory(categoryPosition);
                SearchMenuMap();
                InitButton();

                ProgressStop();
            }
        }

		private void OnGrdRowClickChangedCategory(object sender, System.EventArgs e)
		{
			if (!IsSearching) // 조회중이 아닐경우에만 동작하도록 변경
			{
                // 카테고리가 변경되는 경우도 편집 종료 상태로 간주
                IsEditingStart = false;

                ResetDetail();
                DisableButton();
				SetDetailList();
				InitButton();
			}
		}

        private void OnGrdRowClickChangedMenuMap(object sender, System.EventArgs e)
        {
            if (!IsSearching) // 조회중이 아닐경우에만 동작하도록 변경
            {
                // false인 경우 매핑 해제 작업에서 매핑 작업으로 넘어온 경우이므로 기존 상세정보 폐기
                if (IsMapping)
                {
                    ResetDetail();
                    IsMapping = false;
                }

                if (!IsEditingStart)
                {
                    IsEditingStart = true;
                }

                DisableButton();
                SetMapDetailText();
                InitButton();
            }
        }

        private void OnGrdRowClickChangedMenuStd(object sender, System.EventArgs e)
        {
            if (!IsSearching) // 조회중이 아닐경우에만 동작하도록 변경
            {
                // true인 경우 매핑 작업에서 해제 작업으로 넘어온 경우이므로 기존 상세정보 폐기
                if (!IsMapping)
                {
                    ResetDetail();
                    IsMapping = true;
                }

                if (!IsEditingStart)
                {
                    IsEditingStart = true;
                }

                DisableButton();
                SetDetailText(Version.Standard);
                InitButton();
            }
        }

        private void OnGrdRowClickChangedMenu4(object sender, System.EventArgs e)
        {
            if (!IsSearching) // 조회중이 아닐경우에만 동작하도록 변경
            {
                // true인 경우 매핑 작업에서 해제 작업으로 넘어온 경우이므로 기존 상세정보 폐기
                if (!IsMapping)
                {
                    ResetDetail();
                    IsMapping = true;
                }

                if (!IsEditingStart)
                {
                    IsEditingStart = true;
                }

                DisableButton();
                SetDetailText(Version.ver4);
                InitButton();
            }
        }
        #endregion
    }
}
