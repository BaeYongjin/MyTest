/*
 * -------------------------------------------------------
 * [수정사항]
 * -------------------------------------------------------
 * 수정코드	: [E_01]
 * 수정자		: YJ.Park
 * 수정일		: 2014.11.13
 * 수정내용	: 홈광고(키즈) 추가에 따라 구분 추가
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

namespace AdManagerClient
{
    /// <summary>
    /// ItemMultiChoiceForm에 대한 요약 설명입니다.
    /// </summary>
    /// 
    public class ItemMultiChoiceForm : System.Windows.Forms.Form
    {
       
        #region 이벤트핸들러
        public event StatusEventHandler 			StatusEvent;			// 상태이벤트 핸들러
        #endregion
			
        #region 사용자정의 객체 및 변수

        // 시스템 정보 : 화면공통
        private SystemModel   systemModel   = FrameSystem.oSysModel;
        private CommonModel   commonModel   = FrameSystem.oComModel;
        private Logger        log           = FrameSystem.oLog;

        public delegate void    PopupService(object sender, EventArgs args);
        public event PopupService ReturnDate;

        // 사용할 정보모델
        ItemModel itemModel = new ItemModel();	// 지정광고편성모델

        // 화면처리용 변수
        bool IsNewSearchKey		  = true;					// 검색어입력 여부
		CurrencyManager cm        = null;					// 데이터 그리드의 변경에 따른 데이터셋 관리를 위하여			
		DataTable       dt        = null;

		// 메뉴/채널 구분
		const int AD_MENU     = 1;
		const int AD_CHANNEL  = 2;

        //호출한 객체
        public string callType = "";    //팝업을 호출한 부모를 구분하기 위해서 선언함

        // 2007.10.02 파일리스트건수 검사
        private const int FILEMAX = 1000;	// 최대 파일리스트 건수 >= 현재파일리스트 = 홈광고건수 + CDN배포완료 상태파일 건수
        private int FileListCnt = 0;		// 현재파일리스트 건수

		// 매체
        public string keyOrder = "";
        public string keyCugCode = "";				// 팝업호출시 CUG셋트
        public string keyCugName = "";				// 팝업호출시 CUG셋트
        public string keyAdverCode = "";

		/// <summary>
		/// 조회시 상업광고류인지 매체광고류인지 구분
		/// 편성승인분리로 인해서 추가됨
		/// </summary>
		public	string	keySchType	= "00";

        private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Panel panel4;
		private Janus.Windows.EditControls.UIButton btnSearch;
		private Janus.Windows.GridEX.EditControls.EditBox ebSearchKey;
		private System.Windows.Forms.Panel pnlBtn;
		private Janus.Windows.EditControls.UIButton btnClose;
		private Janus.Windows.EditControls.UIButton btnOk;
		private System.Data.DataView dvItems;
		private Janus.Windows.GridEX.GridEX grdExItemList;
		private System.Windows.Forms.Label label13;
		private Janus.Windows.EditControls.UICheckBox chkAdState_10;
		private Janus.Windows.EditControls.UICheckBox chkAdState_40;
		private Janus.Windows.EditControls.UICheckBox chkAdState_30;
		private Janus.Windows.EditControls.UICheckBox chkAdState_20;
        private Label label2;
        private Label lbFileListCount;
        private Janus.Windows.EditControls.UIComboBox cbSearchRap;
		private AdManagerClient.ItemMultiChoice_pDs itemMultiChoice_pDs;

		#endregion

		#region 생성자 및 소멸자
		public ItemMultiChoiceForm(UserControl parent)
		{
			// Windows Form 디자이너 지원에 필요합니다.
			InitializeComponent();
           
			// 이창을 호출한 컨트롤
			itemModel.Init();
		}		
		public ItemMultiChoiceForm()
        {
            // Windows Form 디자이너 지원에 필요합니다.
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ItemMultiChoiceForm));
			this.dvItems = new System.Data.DataView();
			this.itemMultiChoice_pDs = new AdManagerClient.ItemMultiChoice_pDs();
			this.panel1 = new System.Windows.Forms.Panel();
			this.grdExItemList = new Janus.Windows.GridEX.GridEX();
			this.pnlBtn = new System.Windows.Forms.Panel();
			this.lbFileListCount = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.btnClose = new Janus.Windows.EditControls.UIButton();
			this.btnOk = new Janus.Windows.EditControls.UIButton();
			this.panel4 = new System.Windows.Forms.Panel();
			this.cbSearchRap = new Janus.Windows.EditControls.UIComboBox();
			this.label13 = new System.Windows.Forms.Label();
			this.chkAdState_10 = new Janus.Windows.EditControls.UICheckBox();
			this.chkAdState_40 = new Janus.Windows.EditControls.UICheckBox();
			this.chkAdState_30 = new Janus.Windows.EditControls.UICheckBox();
			this.chkAdState_20 = new Janus.Windows.EditControls.UICheckBox();
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
			this.grdExItemList.GridLines = Janus.Windows.GridEX.GridLines.Vertical;
			this.grdExItemList.GridLineStyle = Janus.Windows.GridEX.GridLineStyle.Solid;
			this.grdExItemList.GroupByBoxVisible = false;
			this.grdExItemList.GroupMode = Janus.Windows.GridEX.GroupMode.Collapsed;
			this.grdExItemList.HeaderFormatStyle.TextAlignment = Janus.Windows.GridEX.TextAlignment.Center;
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
			this.grdExItemList.TabIndex = 7;
			this.grdExItemList.TabKeyBehavior = Janus.Windows.GridEX.TabKeyBehavior.ControlNavigation;
			this.grdExItemList.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
			this.grdExItemList.CellValueChanged += new Janus.Windows.GridEX.ColumnActionEventHandler(this.grdExItemList_CellValueChanged);
			this.grdExItemList.ColumnHeaderClick += new Janus.Windows.GridEX.ColumnActionEventHandler(this.grdExItemList_ColumnHeaderClick);
			// 
			// pnlBtn
			// 
			this.pnlBtn.BackColor = System.Drawing.Color.WhiteSmoke;
			this.pnlBtn.Controls.Add(this.lbFileListCount);
			this.pnlBtn.Controls.Add(this.label2);
			this.pnlBtn.Controls.Add(this.btnClose);
			this.pnlBtn.Controls.Add(this.btnOk);
			this.pnlBtn.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.pnlBtn.Location = new System.Drawing.Point(0, 423);
			this.pnlBtn.Name = "pnlBtn";
			this.pnlBtn.Size = new System.Drawing.Size(792, 43);
			this.pnlBtn.TabIndex = 19;
			// 
			// lbFileListCount
			// 
			this.lbFileListCount.ForeColor = System.Drawing.SystemColors.ActiveCaption;
			this.lbFileListCount.Location = new System.Drawing.Point(162, 11);
			this.lbFileListCount.Name = "lbFileListCount";
			this.lbFileListCount.Size = new System.Drawing.Size(88, 22);
			this.lbFileListCount.TabIndex = 48;
			this.lbFileListCount.Text = "0/0";
			this.lbFileListCount.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label2
			// 
			this.label2.ForeColor = System.Drawing.SystemColors.ActiveCaption;
			this.label2.Location = new System.Drawing.Point(12, 10);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(144, 22);
			this.label2.TabIndex = 47;
			this.label2.Text = "현재홈+파일리스트갯수:";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// btnClose
			// 
			this.btnClose.BackColor = System.Drawing.SystemColors.Control;
			this.btnClose.Location = new System.Drawing.Point(400, 9);
			this.btnClose.Name = "btnClose";
			this.btnClose.Size = new System.Drawing.Size(104, 24);
			this.btnClose.TabIndex = 9;
			this.btnClose.Text = "닫 기";
			this.btnClose.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
			this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
			// 
			// btnOk
			// 
			this.btnOk.BackColor = System.Drawing.SystemColors.Control;
			this.btnOk.Location = new System.Drawing.Point(288, 9);
			this.btnOk.Name = "btnOk";
			this.btnOk.Size = new System.Drawing.Size(104, 24);
			this.btnOk.TabIndex = 8;
			this.btnOk.Text = "편 성";
			this.btnOk.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
			this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
			// 
			// panel4
			// 
			this.panel4.BackColor = System.Drawing.Color.White;
			this.panel4.Controls.Add(this.cbSearchRap);
			this.panel4.Controls.Add(this.label13);
			this.panel4.Controls.Add(this.chkAdState_10);
			this.panel4.Controls.Add(this.chkAdState_40);
			this.panel4.Controls.Add(this.chkAdState_30);
			this.panel4.Controls.Add(this.chkAdState_20);
			this.panel4.Controls.Add(this.btnSearch);
			this.panel4.Controls.Add(this.ebSearchKey);
			this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
			this.panel4.Location = new System.Drawing.Point(0, 0);
			this.panel4.Name = "panel4";
			this.panel4.Size = new System.Drawing.Size(792, 43);
			this.panel4.TabIndex = 16;
			// 
			// cbSearchRap
			// 
			this.cbSearchRap.ComboStyle = Janus.Windows.EditControls.ComboStyle.DropDownList;
			this.cbSearchRap.Location = new System.Drawing.Point(185, 11);
			this.cbSearchRap.Name = "cbSearchRap";
			this.cbSearchRap.Size = new System.Drawing.Size(120, 21);
			this.cbSearchRap.TabIndex = 20;
			this.cbSearchRap.Text = "미디어렙선택";
			this.cbSearchRap.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
			// 
			// label13
			// 
			this.label13.Location = new System.Drawing.Point(334, 14);
			this.label13.Name = "label13";
			this.label13.Size = new System.Drawing.Size(66, 16);
			this.label13.TabIndex = 39;
			this.label13.Text = "광고상태";
			this.label13.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// chkAdState_10
			// 
			this.chkAdState_10.Checked = true;
			this.chkAdState_10.CheckedValue = "";
			this.chkAdState_10.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkAdState_10.ForeColor = System.Drawing.SystemColors.ControlText;
			this.chkAdState_10.Location = new System.Drawing.Point(406, 12);
			this.chkAdState_10.Name = "chkAdState_10";
			this.chkAdState_10.Size = new System.Drawing.Size(65, 19);
			this.chkAdState_10.TabIndex = 2;
			this.chkAdState_10.Text = "대기";
			this.chkAdState_10.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
			// 
			// chkAdState_40
			// 
			this.chkAdState_40.Location = new System.Drawing.Point(604, 11);
			this.chkAdState_40.Name = "chkAdState_40";
			this.chkAdState_40.Size = new System.Drawing.Size(66, 20);
			this.chkAdState_40.TabIndex = 5;
			this.chkAdState_40.Text = "종료";
			this.chkAdState_40.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
			// 
			// chkAdState_30
			// 
			this.chkAdState_30.BackColor = System.Drawing.SystemColors.Window;
			this.chkAdState_30.Location = new System.Drawing.Point(542, 11);
			this.chkAdState_30.Name = "chkAdState_30";
			this.chkAdState_30.Size = new System.Drawing.Size(56, 20);
			this.chkAdState_30.TabIndex = 4;
			this.chkAdState_30.Text = "중지";
			this.chkAdState_30.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
			// 
			// chkAdState_20
			// 
			this.chkAdState_20.Checked = true;
			this.chkAdState_20.CheckedValue = "";
			this.chkAdState_20.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkAdState_20.ForeColor = System.Drawing.SystemColors.ControlText;
			this.chkAdState_20.Location = new System.Drawing.Point(477, 11);
			this.chkAdState_20.Name = "chkAdState_20";
			this.chkAdState_20.Size = new System.Drawing.Size(59, 20);
			this.chkAdState_20.TabIndex = 3;
			this.chkAdState_20.Text = "편성";
			this.chkAdState_20.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
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
			this.ebSearchKey.ButtonFont = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.ebSearchKey.Location = new System.Drawing.Point(8, 11);
			this.ebSearchKey.Name = "ebSearchKey";
			this.ebSearchKey.Size = new System.Drawing.Size(171, 21);
			this.ebSearchKey.TabIndex = 1;
			this.ebSearchKey.Text = "검색어를 입력하세요...";
			this.ebSearchKey.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
			this.ebSearchKey.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
			this.ebSearchKey.TextChanged += new System.EventHandler(this.ebSearchKey_TextChanged);
			this.ebSearchKey.Click += new System.EventHandler(this.ebSearchKey_Click);
			this.ebSearchKey.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ebSearchKey_KeyDown);
			// 
			// ItemMultiChoiceForm
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(6, 14);
			this.ClientSize = new System.Drawing.Size(792, 466);
			this.Controls.Add(this.panel1);
			this.DoubleBuffered = true;
			this.Font = global::AdManagerClient.Properties.Settings.Default.Font1;
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.Name = "ItemMultiChoiceForm";
			this.ShowIcon = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "광고 검색";
			this.TopMost = true;
			this.Load += new System.EventHandler(this.ItemMultiChoiceForm_Load);
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
        private void ItemMultiChoiceForm_Load(object sender, System.EventArgs e)
        {
			// 데이터관리용 객체생성
			dt = ((DataView)grdExItemList.DataSource).Table;  
			cm = (CurrencyManager) this.BindingContext[grdExItemList.DataSource];

            Init_RapCode();
            InitCombo_Level();

            SearchChoiceAdItem();
            ebSearchKey.Focus();
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
            if (e.KeyCode == Keys.Enter && !(IsNewSearchKey))
            {
                SearchChoiceAdItem();
            }
        }

		private void btnOk_Click(object sender, System.EventArgs e)
		{
			AddSchChoiceAd();
			this.Close();
		}

		private void btnClose_Click(object sender, System.EventArgs e)
		{
			this.Close();
		}

		private void btnSearch_Click(object sender, System.EventArgs e)
		{
			SearchChoiceAdItem();
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


        private void grdExItemList_CellValueChanged(object sender, Janus.Windows.GridEX.ColumnActionEventArgs e)
        {
            if (e.Column.EditType == Janus.Windows.GridEX.EditType.CheckBox)
            {
                int curRow = cm.Position;
                if (curRow >= 0)
                {
                    dt.Rows[curRow].BeginEdit();
                    dt.Rows[curRow]["CheckYn"] = grdExItemList.GetValue(e.Column).ToString();
                    dt.Rows[curRow].EndEdit();
                }
            }
        }

		#endregion
  
        #region 처리메소드

        /// <summary>
        /// 지정광고 편성대상 광고목록 조회
        /// </summary>
        private void SearchChoiceAdItem()
        {
            StatusMessage("지정광고 편성대상 광고목록을 조회합니다.");

            try
            {
				grdExItemList.UnCheckAllRecords();

				itemModel.Init();

                // 데이터모델에 전송할 내용을 셋트한다.
                if(IsNewSearchKey || ebSearchKey.Text.Length == 0)  itemModel.SearchKey = "";
                else                                                itemModel.SearchKey  = ebSearchKey.Text;

				if(chkAdState_10.Checked)   itemModel.SearchchkAdState_10   = "Y";
				if(chkAdState_20.Checked)   itemModel.SearchchkAdState_20   = "Y";
				if(chkAdState_30.Checked)   itemModel.SearchchkAdState_30   = "Y";
				if(chkAdState_40.Checked)   itemModel.SearchchkAdState_40   = "Y";

				itemModel.SearchMediaCode = "1";
				itemModel.MediaCode = "1";

                itemModel.RapCode = cbSearchRap.SelectedValue.ToString();
                itemModel.SearchCugCode = keyCugCode;   // CUG코드셋트

				// 편성승인구분값으로 사용됨
                itemModel.SearchAdType = keySchType.ToString();

                // 광고내역에서 듀얼엔딩일때만 사용함
                itemModel.SearchAdvertiserCode = keyAdverCode;

                // 목록조회 서비스를 호출한다.
                new ItemManager(systemModel,commonModel).GetContractItemList(itemModel, callType);

                if (itemModel.ResultCD.Equals("0000"))
                {
                    Utility.SetDataTableFast(itemMultiChoice_pDs.ChoiceAdItems, itemModel.ScheduleDataSet);				
                    StatusMessage(itemModel.ResultCnt + "건의 광고목록이 조회되었습니다.");

					if (callType.Equals("SchHomeAdControl") 
						|| callType.Equals("CugHomeAdControl") 
						|| callType.Equals("SchHomeKidsControl"))	//[E_01]
                    {
                        // 2007.10.01 파일리스트건수 검사
                        FileListCnt = itemModel.FileListCount;
                        lbFileListCount.Text = FileListCnt.ToString() + "/" + FILEMAX.ToString(); 
                    }

					if(itemModel.ResultCnt == 0)
					{
						MessageBox.Show("조회된 지정광고 편성대상 광고목록이 없습니다", "조회결과",	MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					}
                }
            }
            catch(FrameException fe)
            {
                FrameSystem.showMsgForm("광고목록 조회오류", new string[] {fe.ErrCode, fe.ResultMsg});
            }
            catch(Exception ex)
            {
                FrameSystem.showMsgForm("광고목록 조회오류",new string[] {"",ex.Message});
            }
        }

		/// <summary>
		/// 선택된광고를 지정광고에 편성
		/// </summary>
		private void AddSchChoiceAd()
		{
			StatusMessage("선택된 광고를 지정광고에 편성합니다.");

			// 그리드에 변경된 데이터를 Datasource에 업데이트 한다.
			grdExItemList.UpdateData();

            ItemEventArgs args = new ItemEventArgs();
            args.dataSet = itemMultiChoice_pDs;
			args.keyMediaCode = "1";
            args.keyOrder = keyOrder;
            args.KeyCugCode = keyCugCode; // 추가(cug 지정광고편성시 사용)
            
            ReturnDate(this, args);		
		}

        #endregion

        /// <summary>
        /// 렙코드를 읽어온다
        /// </summary>
        private void Init_RapCode()
        {
            // 랩을 조회한다.
            MediaRapCodeModel mediaRapCodeModel = new MediaRapCodeModel();
            new MediaRapCodeManager(systemModel, commonModel).GetMediaRapCodeList(mediaRapCodeModel);

            if (mediaRapCodeModel.ResultCD.Equals("0000"))
            {
                Utility.SetDataTable(itemMultiChoice_pDs.MediaRaps, mediaRapCodeModel.MediaRapCodeDataSet);
            }

            // 검색조건의 콤보
            this.cbSearchRap.Items.Clear();

            // 콤보박스에 셋트할 코드목록을 담을 Item배열을 선언
            Janus.Windows.EditControls.UIComboBoxItem[] comboItems = new Janus.Windows.EditControls.UIComboBoxItem[mediaRapCodeModel.ResultCnt + 1];

            comboItems[0] = new Janus.Windows.EditControls.UIComboBoxItem("미디어렙선택", "00");

            for (int i = 0; i < mediaRapCodeModel.ResultCnt; i++)
            {
                DataRow row = itemMultiChoice_pDs.MediaRaps.Rows[i];

                string val = row["RapCode"].ToString();
                string txt = row["RapName"].ToString();
                comboItems[i + 1] = new Janus.Windows.EditControls.UIComboBoxItem(txt, val);
            }
            // 콤보에 셋트
            this.cbSearchRap.Items.AddRange(comboItems);
            if (mediaRapCodeModel.ResultCnt > 1)
                this.cbSearchRap.SelectedIndex = 1;
            else this.cbSearchRap.SelectedIndex = 0;

            Application.DoEvents();

        }

        /// <summary>
        /// 콤보 초기화
        /// </summary>
        private void InitCombo_Level()
        {
            if (commonModel.UserLevel == "30")
            {
                cbSearchRap.SelectedValue = commonModel.RapCode;
                cbSearchRap.ReadOnly = true;
            }

            Application.DoEvents();
        }

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

        public void ClearListCheck()
        {
            // 체크된 모든 항목을 클리어
            grdExItemList.UnCheckAllRecords();
            grdExItemList.UpdateData();

            // 데이터 클리어
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                dt.Rows[i].BeginEdit();
                dt.Rows[i]["CheckYn"] = "False";
                dt.Rows[i].EndEdit();
            }
        }

    }
}
