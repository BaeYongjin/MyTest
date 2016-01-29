// ===============================================================================
// SchHomeAd SearchForm  for Charites Project
//
// SchHomeAdSearch_pForm.cs
//
// 홈상업광고 편성대상 조회. 
//
// ===============================================================================
// Release history
//
// ===============================================================================
// Copyright (C) 2007 G-Inno Systems Inc.
// All rights reserved.
// ==============================================================================
/*
 * [수정사항]
 * -------------------------------------------------------
 * 수정코드  : [E_01]
 * 수정자    : YJ.Park
 * 수정일    : 2014.08.8
 * 수정내용  :        
 *            - 편성복사 기능추가.
 * --------------------------------------------------------
 */
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
using AdManagerClient.Common;
using System.Text;

namespace AdManagerClient
{
    /// <summary>
    /// SchHomeAdSearch_pForm에 대한 요약 설명입니다.
    /// </summary>
    /// 
    public class ItemSingleChoiceForm : System.Windows.Forms.Form
    {
        #region 이벤트핸들러
        public event StatusEventHandler 			StatusEvent;			// 상태이벤트 핸들러
        #endregion
			
        #region 사용자정의 객체 및 변수

        // 시스템 정보 : 화면공통
        private SystemModel   systemModel   = FrameSystem.oSysModel;
        private CommonModel   commonModel   = FrameSystem.oComModel;
        private Logger        log           = FrameSystem.oLog;

        public delegate void PopupService(object sender, EventArgs args);
        public event PopupService ReturnDate;

        // 사용할 정보모델
        ItemModel itemModel = new ItemModel();	// 지정광고편성모델

        // 화면처리용 변수
        bool IsNewSearchKey = true;				// 검색어입력 여부
        CurrencyManager cm = null;				// 데이터 그리드의 변경에 따른 데이터셋 관리를 위하여			
        DataTable dt = null;

        //호출한 객체
        public string callType = "";    //팝업을 호출한 부모를 구분하기 위해서 선언함
        public string callItemNo = ""; //팝업을 호출 한 부모에서 선택된 ItemNo값을 받아오기 위해 선언 [E_01]

		// 매체
		public string	keyMediaCode = "";			// 팝업호출시 매체셋트
		public string	keyOrder	= "";

        private System.Windows.Forms.Panel panel1;
		private AdManagerClient.ItemMultiChoice_pDs itemMultiChoice_pDs;
		private System.Windows.Forms.Panel panel4;
		private Janus.Windows.EditControls.UIButton btnSearch;
		private Janus.Windows.GridEX.EditControls.EditBox ebSearchKey;
		private System.Windows.Forms.Panel pnlBtn;
		private Janus.Windows.EditControls.UIButton btnClose;
		private Janus.Windows.EditControls.UIButton btnOk;
		private System.Data.DataView dvItems;
		private System.Windows.Forms.Label label13;
		private Janus.Windows.EditControls.UICheckBox chkAdState_40;
		private Janus.Windows.EditControls.UICheckBox chkAdState_30;
		private System.Windows.Forms.Label lbFileListCount;
		private Janus.Windows.EditControls.UICheckBox chkAdState_20;
		private Janus.Windows.EditControls.UICheckBox chkAdState_10;
		private Janus.Windows.GridEX.GridEX grdExItemList;

		#endregion

		#region 생성자 및 소멸자
		public ItemSingleChoiceForm(UserControl parent)
		{
			//
			// Windows Form 디자이너 지원에 필요합니다.
			//
			InitializeComponent();
           
			// 이창을 호출한 컨트롤
            itemModel.Init();
		}		
		public ItemSingleChoiceForm()
        {
            //
            // Windows Form 디자이너 지원에 필요합니다.
            //
            InitializeComponent();

            itemModel.Init();
        }

        /// <summary>
        /// 사용 중인 모든 리소스를 정리합니다.
        /// </summary>
        protected override void Dispose( bool disposing )
        {

            base.Dispose( disposing );
        }

		#endregion

        #region Windows Form 디자이너에서 생성한 코드
        /// <summary>
        /// 디자이너 지원에 필요한 메서드입니다.
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마십시오.
        /// </summary>
        private void InitializeComponent()
        {
			Janus.Windows.GridEX.GridEXLayout grdExItemList_Layout_0 = new Janus.Windows.GridEX.GridEXLayout();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ItemSingleChoiceForm));
			this.dvItems = new System.Data.DataView();
			this.itemMultiChoice_pDs = new AdManagerClient.ItemMultiChoice_pDs();
			this.panel1 = new System.Windows.Forms.Panel();
			this.grdExItemList = new Janus.Windows.GridEX.GridEX();
			this.pnlBtn = new System.Windows.Forms.Panel();
			this.lbFileListCount = new System.Windows.Forms.Label();
			this.btnClose = new Janus.Windows.EditControls.UIButton();
			this.btnOk = new Janus.Windows.EditControls.UIButton();
			this.panel4 = new System.Windows.Forms.Panel();
			this.chkAdState_10 = new Janus.Windows.EditControls.UICheckBox();
			this.chkAdState_20 = new Janus.Windows.EditControls.UICheckBox();
			this.label13 = new System.Windows.Forms.Label();
			this.chkAdState_40 = new Janus.Windows.EditControls.UICheckBox();
			this.chkAdState_30 = new Janus.Windows.EditControls.UICheckBox();
			this.btnSearch = new Janus.Windows.EditControls.UIButton();
			this.ebSearchKey = new Janus.Windows.GridEX.EditControls.EditBox();
			((System.ComponentModel.ISupportInitialize)(this.dvItems)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.itemMultiChoice_pDs)).BeginInit();
			this.panel1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.grdExItemList)).BeginInit();
			this.pnlBtn.SuspendLayout();
			this.panel4.SuspendLayout();
			this.SuspendLayout();
			// 
			// dvItems
			// 
			this.dvItems.Table = this.itemMultiChoice_pDs.ChoiceAdItems;
			// 
			// itemMultiChoice_pDs
			// 
			this.itemMultiChoice_pDs.DataSetName = "ItemMultiChoice_pDs";
			this.itemMultiChoice_pDs.Locale = new System.Globalization.CultureInfo("en-US");
			this.itemMultiChoice_pDs.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
			// 
			// panel1
			// 
			this.panel1.Controls.Add(this.grdExItemList);
			this.panel1.Controls.Add(this.pnlBtn);
			this.panel1.Controls.Add(this.panel4);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel1.Location = new System.Drawing.Point(0, 0);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(792, 466);
			this.panel1.TabIndex = 17;
			// 
			// grdExItemList
			// 
			this.grdExItemList.AlternatingColors = true;
			this.grdExItemList.BorderStyle = Janus.Windows.GridEX.BorderStyle.Flat;
			this.grdExItemList.DataSource = this.dvItems;
			this.grdExItemList.Dock = System.Windows.Forms.DockStyle.Fill;
			this.grdExItemList.EmptyRows = true;
			this.grdExItemList.FocusCellFormatStyle.BackColor = System.Drawing.SystemColors.Highlight;
			this.grdExItemList.FocusCellFormatStyle.ForeColor = System.Drawing.Color.White;
			this.grdExItemList.FocusStyle = Janus.Windows.GridEX.FocusStyle.None;
			this.grdExItemList.Font = new System.Drawing.Font("굴림", 9F);
			this.grdExItemList.GridLines = Janus.Windows.GridEX.GridLines.Vertical;
			this.grdExItemList.GridLineStyle = Janus.Windows.GridEX.GridLineStyle.Solid;
			this.grdExItemList.GroupByBoxVisible = false;
			this.grdExItemList.GroupMode = Janus.Windows.GridEX.GroupMode.Collapsed;
			this.grdExItemList.HeaderFormatStyle.TextAlignment = Janus.Windows.GridEX.TextAlignment.Center;
			this.grdExItemList.HideSelection = Janus.Windows.GridEX.HideSelection.Highlight;
			grdExItemList_Layout_0.DataSource = this.dvItems;
			grdExItemList_Layout_0.IsCurrentLayout = true;
			grdExItemList_Layout_0.Key = "bae";
			grdExItemList_Layout_0.LayoutString = resources.GetString("grdExItemList_Layout_0.LayoutString");
			this.grdExItemList.Layouts.AddRange(new Janus.Windows.GridEX.GridEXLayout[] {
            grdExItemList_Layout_0});
			this.grdExItemList.Location = new System.Drawing.Point(0, 43);
			this.grdExItemList.Name = "grdExItemList";
			this.grdExItemList.ScrollBars = Janus.Windows.GridEX.ScrollBars.Vertical;
			this.grdExItemList.Size = new System.Drawing.Size(792, 380);
			this.grdExItemList.TabIndex = 8;
			this.grdExItemList.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
			this.grdExItemList.ColumnHeaderClick += new Janus.Windows.GridEX.ColumnActionEventHandler(this.grdExItemList_ColumnHeaderClick);
			// 
			// pnlBtn
			// 
			this.pnlBtn.BackColor = System.Drawing.Color.WhiteSmoke;
			this.pnlBtn.Controls.Add(this.lbFileListCount);
			this.pnlBtn.Controls.Add(this.btnClose);
			this.pnlBtn.Controls.Add(this.btnOk);
			this.pnlBtn.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.pnlBtn.Location = new System.Drawing.Point(0, 423);
			this.pnlBtn.Name = "pnlBtn";
			this.pnlBtn.Size = new System.Drawing.Size(792, 43);
			this.pnlBtn.TabIndex = 9;
			// 
			// lbFileListCount
			// 
			this.lbFileListCount.ForeColor = System.Drawing.SystemColors.ActiveCaption;
			this.lbFileListCount.Location = new System.Drawing.Point(152, 9);
			this.lbFileListCount.Name = "lbFileListCount";
			this.lbFileListCount.Size = new System.Drawing.Size(88, 22);
			this.lbFileListCount.TabIndex = 45;
			this.lbFileListCount.Text = "0/0";
			this.lbFileListCount.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// btnClose
			// 
			this.btnClose.BackColor = System.Drawing.SystemColors.Control;
			this.btnClose.Location = new System.Drawing.Point(400, 9);
			this.btnClose.Name = "btnClose";
			this.btnClose.Size = new System.Drawing.Size(104, 24);
			this.btnClose.TabIndex = 11;
			this.btnClose.Text = "취 소";
			this.btnClose.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
			this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
			// 
			// btnOk
			// 
			this.btnOk.BackColor = System.Drawing.SystemColors.Control;
			this.btnOk.Location = new System.Drawing.Point(288, 9);
			this.btnOk.Name = "btnOk";
			this.btnOk.Size = new System.Drawing.Size(104, 24);
			this.btnOk.TabIndex = 10;
			this.btnOk.Text = "편 성";
			this.btnOk.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
			this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
			// 
			// panel4
			// 
			this.panel4.BackColor = System.Drawing.Color.White;
			this.panel4.Controls.Add(this.chkAdState_10);
			this.panel4.Controls.Add(this.chkAdState_20);
			this.panel4.Controls.Add(this.label13);
			this.panel4.Controls.Add(this.chkAdState_40);
			this.panel4.Controls.Add(this.chkAdState_30);
			this.panel4.Controls.Add(this.btnSearch);
			this.panel4.Controls.Add(this.ebSearchKey);
			this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
			this.panel4.Location = new System.Drawing.Point(0, 0);
			this.panel4.Name = "panel4";
			this.panel4.Size = new System.Drawing.Size(792, 43);
			this.panel4.TabIndex = 7;
			// 
			// chkAdState_10
			// 
			this.chkAdState_10.BackColor = System.Drawing.SystemColors.Window;
			this.chkAdState_10.Checked = true;
			this.chkAdState_10.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkAdState_10.Location = new System.Drawing.Point(320, 13);
			this.chkAdState_10.Name = "chkAdState_10";
			this.chkAdState_10.Size = new System.Drawing.Size(58, 20);
			this.chkAdState_10.TabIndex = 36;
			this.chkAdState_10.Text = "대기";
			this.chkAdState_10.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
			// 
			// chkAdState_20
			// 
			this.chkAdState_20.BackColor = System.Drawing.SystemColors.Window;
			this.chkAdState_20.Checked = true;
			this.chkAdState_20.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkAdState_20.Location = new System.Drawing.Point(384, 13);
			this.chkAdState_20.Name = "chkAdState_20";
			this.chkAdState_20.Size = new System.Drawing.Size(56, 19);
			this.chkAdState_20.TabIndex = 35;
			this.chkAdState_20.Text = "편성";
			this.chkAdState_20.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
			// 
			// label13
			// 
			this.label13.Location = new System.Drawing.Point(238, 12);
			this.label13.Name = "label13";
			this.label13.Size = new System.Drawing.Size(64, 22);
			this.label13.TabIndex = 34;
			this.label13.Text = "광고상태";
			this.label13.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// chkAdState_40
			// 
			this.chkAdState_40.Location = new System.Drawing.Point(510, 13);
			this.chkAdState_40.Name = "chkAdState_40";
			this.chkAdState_40.Size = new System.Drawing.Size(55, 20);
			this.chkAdState_40.TabIndex = 5;
			this.chkAdState_40.Text = "종료";
			this.chkAdState_40.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
			// 
			// chkAdState_30
			// 
			this.chkAdState_30.BackColor = System.Drawing.SystemColors.Window;
			this.chkAdState_30.Location = new System.Drawing.Point(446, 13);
			this.chkAdState_30.Name = "chkAdState_30";
			this.chkAdState_30.Size = new System.Drawing.Size(58, 19);
			this.chkAdState_30.TabIndex = 4;
			this.chkAdState_30.Text = "중지";
			this.chkAdState_30.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
			// 
			// btnSearch
			// 
			this.btnSearch.BackColor = System.Drawing.Color.White;
			this.btnSearch.Location = new System.Drawing.Point(680, 9);
			this.btnSearch.Name = "btnSearch";
			this.btnSearch.Size = new System.Drawing.Size(104, 25);
			this.btnSearch.TabIndex = 6;
			this.btnSearch.Text = "조 회";
			this.btnSearch.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
			this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
			// 
			// ebSearchKey
			// 
			this.ebSearchKey.Location = new System.Drawing.Point(8, 11);
			this.ebSearchKey.Name = "ebSearchKey";
			this.ebSearchKey.Size = new System.Drawing.Size(208, 21);
			this.ebSearchKey.TabIndex = 1;
			this.ebSearchKey.Text = "검색어를 입력하세요";
			this.ebSearchKey.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
			this.ebSearchKey.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
			this.ebSearchKey.TextChanged += new System.EventHandler(this.ebSearchKey_TextChanged);
			this.ebSearchKey.Click += new System.EventHandler(this.ebSearchKey_Click);
			this.ebSearchKey.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ebSearchKey_KeyDown);
			// 
			// ItemSingleChoiceForm
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(6, 14);
			this.ClientSize = new System.Drawing.Size(792, 466);
			this.Controls.Add(this.panel1);
			this.DoubleBuffered = true;
			this.Font = global::AdManagerClient.Properties.Settings.Default.Font1;
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.MaximizeBox = false;
			this.Name = "ItemSingleChoiceForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "광고 검색";
			this.Load += new System.EventHandler(this.SchHomeAdSearch_pForm_Load);
			((System.ComponentModel.ISupportInitialize)(this.dvItems)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.itemMultiChoice_pDs)).EndInit();
			this.panel1.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.grdExItemList)).EndInit();
			this.pnlBtn.ResumeLayout(false);
			this.panel4.ResumeLayout(false);
			this.panel4.PerformLayout();
			this.ResumeLayout(false);

        }
        #endregion

        #region 컨트롤 로드
        private void SchHomeAdSearch_pForm_Load(object sender, System.EventArgs e)
        {
            
			// 데이터관리용 객체생성
			dt = ((DataView)grdExItemList.DataSource).Table;  
			cm = (CurrencyManager) this.BindingContext[grdExItemList.DataSource]; 
			cm.PositionChanged += new EventHandler(cm_PositionChanged);

            SearchHomeAdItem();
        }

        #endregion

        #region 사용자 액션처리 메소드

        /// <summary>
        /// 검색어 변경
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ebSearchKey_TextChanged(object sender, System.EventArgs e)
        {
            IsNewSearchKey = false;
        }

        /// <summary>
        /// 검색어 클릭 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ebSearchKey_Click(object sender, System.EventArgs e)
        {
            if(IsNewSearchKey)
            {
                ebSearchKey.Text = "";
            }
            else
            {
                ebSearchKey.SelectAll();
            }
        }

        private void ebSearchKey_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter && !(IsNewSearchKey))
            {
                SearchHomeAdItem();
            }
        }


		private void btnOk_Click(object sender, System.EventArgs e)
		{
			AddSchHomeAd();
		}
		private void btnClose_Click(object sender, System.EventArgs e)
		{
			this.Close();
		}


		private void btnSearch_Click(object sender, System.EventArgs e)
		{
			SearchHomeAdItem();
		}

		private void grdExItemList_ColumnHeaderClick(object sender, Janus.Windows.GridEX.ColumnActionEventArgs e)
		{

			//컬럼Index 0(체크박스컬럼이)이 아니면 빠져나가게 처리.
			if(e.Column.Index != 0)
			{
				return;
			}
            
			//ColumnHeader Click시에 dt Setting 
			DataRow[] foundRows = dt.Select("CheckYn = 'False'");
         
			// if(grdExItemList.CurrentColumn.Position == 0){
			if( foundRows.Length == 0 )
			{
				for(int i=0;i < dt.Rows.Count;i++)
				{
					dt.Rows[i].BeginEdit();
					dt.Rows[i]["CheckYn"]="False";
					dt.Rows[i].EndEdit();
				}
			}
			else
			{
				for(int i=0;i < dt.Rows.Count;i++)
				{
					dt.Rows[i].BeginEdit();
					dt.Rows[i]["CheckYn"]="True";
					dt.Rows[i].EndEdit();
				}
			}
		}

		#endregion
  
        #region 처리메소드

        /// <summary>
        /// 홈상업광고 편성대상 광고목록 조회
        /// </summary>
        private void SearchHomeAdItem()
        {
            StatusMessage("홈상업광고 편성대상 광고목록을 조회합니다.");

            try
            {
                grdExItemList.UnCheckAllRecords();

                itemModel.Init();

                // 데이터모델에 전송할 내용을 셋트한다.
                if (IsNewSearchKey || ebSearchKey.Text.Length == 0) itemModel.SearchKey = "";
                else itemModel.SearchKey = ebSearchKey.Text;

                if (chkAdState_10.Checked) itemModel.SearchchkAdState_10 = "Y";
                if (chkAdState_20.Checked) itemModel.SearchchkAdState_20 = "Y";
                if (chkAdState_30.Checked) itemModel.SearchchkAdState_30 = "Y";
                if (chkAdState_40.Checked) itemModel.SearchchkAdState_40 = "Y";

                itemModel.SearchMediaCode = keyMediaCode; // 매체코드셋트

                //기편성된 광고 편성 내역을 불러올 경우 [E_01]
                if (callType.Equals("GetSchAdItemList"))
                {
                    itemModel.ItemNo = callItemNo;
                    new ItemManager(systemModel, commonModel).GetSchAdItemList(itemModel);
                }
                else
                {
                    new ItemManager(systemModel, commonModel).GetContractItemList(itemModel, callType);
                }
                if (itemModel.ResultCD.Equals("0000"))
                {
                    Utility.SetDataTable(itemMultiChoice_pDs.ChoiceAdItems, itemModel.ScheduleDataSet);
                    StatusMessage(itemModel.ResultCnt + "건의 광고목록이 조회되었습니다.");
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
        /// 선택된광고를 홈상업광고에 편성
        /// </summary>
        private void AddSchHomeAd()
        {
            StatusMessage("선택된 광고를 편성합니다.");

            if (keyItemNo == 0)
            {
                MessageBox.Show("선택된 광고파일이 없습니다.", "광고편성", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            else
            {
                ItemEventArgs args = new ItemEventArgs();

                args.keyItemNo = keyItemNo;
                args.keyItemName = keyItemName;
                args.AdType = AdType;
                
                if (callType.Equals("GetSchAdItemList"))
                {
                    args.CheckSchResult = CheckSchAd(callItemNo);
                    if (!args.CheckSchResult.Equals(""))
                    {
                        ReturnDate(this, args);
                        this.Close();
                    }
                }
                else
                {
                    ReturnDate(this, args);
                    this.Close();
                }

            }
        }

        /// <summary>
        /// 편성 내역 체크 및 편성 붙여넣기 조건을 받아옴 [E_01]
        /// </summary>
        /// <param name="ItemNo"></param>
        /// <returns></returns>
        private string CheckSchAd(string ItemNo)
        {
            SchChoiceAdModel schChoiceAdModel = new SchChoiceAdModel();
            schChoiceAdModel.ItemNo = ItemNo;   //편성을 적용할 광고 ItemNo
            StringBuilder CheckSch = new StringBuilder(); //편성 상태 체크
            try
            {

                new SchChoiceAdManager(systemModel, commonModel).CheckSchChoice(schChoiceAdModel);
                //체크한 편성상태를 출력하기 위함
                if (schChoiceAdModel.CheckMenu)
                {
                    CheckSch.Append("메뉴편성");
                    CheckSch.Append("/");
                }
                if (schChoiceAdModel.CheckChannel)
                {
                    CheckSch.Append("채널편성");
                    CheckSch.Append("/");
                }
                if (schChoiceAdModel.CheckSeries)
                {
                    CheckSch.Append("회차편성");
                    CheckSch.Append("/");
                }
                if (schChoiceAdModel.CheckDetail)
                {
                    CheckSch.Append("지정편성");
                    CheckSch.Append("/");
                }
                if (CheckSch.Length > 0)
                {
                    CheckSch.Remove(CheckSch.Length - 1, 1);
                }

                //메뉴편성, 채널편성, 회차편성중 1건이라도 조회될경우 yj
                if (schChoiceAdModel.CheckMenu || schChoiceAdModel.CheckChannel || schChoiceAdModel.CheckSeries || schChoiceAdModel.CheckDetail)
                {
                    DialogResult result = new DialogResult();

                    result = UserMessageBox.Show("선택한 광고번호:" + ItemNo + "에 \n" + CheckSch.ToString() + " 내역이 존재합니다.\n'추가복사' 선택시 기존의 편성내역에 추가 복사됩니다.\n'삭제복사' 선택시 기존의 편성내역을 삭제후 편성내역이 복사 됩니다.", "편성 복사하기", "추가복사", "삭제복사", "취소");

                    if (result == DialogResult.Yes)
                    {
                        return "ADD";
                    }
                    else if (result == DialogResult.No)
                    {
                        return "DELETE";
                    }
                }
                else
                {
                    return "ADD";
                }
            }
            catch (FrameException fe)
            {
                FrameSystem.showMsgForm("편성 상태 체크 오류", new string[] { fe.ErrCode, fe.ResultMsg });
            }
            catch (Exception ex)
            {
                FrameSystem.showMsgForm("편성 상태 체크 오류", new string[] { "", ex.Message });
            }
            return "";
        }

		private void ClearListCheck()
		{

			// 체크된 모든 항목을 클리어
			grdExItemList.UnCheckAllRecords();
			grdExItemList.UpdateData();
				   
			// 데이터 클리어
			for(int i=0;i < dt.Rows.Count;i++)
			{
				dt.Rows[i].BeginEdit();
				dt.Rows[i]["CheckYn"]="False";
				dt.Rows[i].EndEdit();
			}
		}



        #endregion

        #region 이벤트함수

        private void StatusMessage(string strMessage)
        {
            if (StatusEvent != null) 
            {
                StatusEventArgs ea = new StatusEventArgs();
                ea.Message   = strMessage;
                StatusEvent(this,ea);
            }
        }
		#endregion

		private	int		keyItemNo = 0;
		private	string	keyItemName = "";
        private string AdType = ""; 

		private void cm_PositionChanged(object sender, EventArgs e)
		{
			int	idx = cm.Position;
			if( idx >= 0 )
			{
				keyItemNo	=	Convert.ToInt32( dt.Rows[idx]["ItemNo"].ToString() );
				keyItemName	=	dt.Rows[idx]["ItemName"].ToString();
                AdType = dt.Rows[idx]["AdType"].ToString(); 
			}
			else
			{
				keyItemNo	=	0;
				keyItemName	=	"";
                AdType = ""; 
			}
			Debug.WriteLine( idx.ToString() + "-" + keyItemNo.ToString() + "-" + keyItemName );
		}
	}
}
