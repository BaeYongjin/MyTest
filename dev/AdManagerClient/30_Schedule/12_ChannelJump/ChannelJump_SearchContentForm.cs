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

using WinFramework.Misc;
using WinFramework.Base;
using WinFramework.Data;

using AdManagerModel;

namespace AdManagerClient
{
    /// <summary>
    /// StatisticsDaily_pForm에 대한 요약 설명입니다.
    /// </summary>
    public class ChannelJump_SearchContentForm : System.Windows.Forms.Form
    {

		#region 사용자정의 객체 및 변수

		public string keyMediaCode = "";
		// 오프너
		private ChannelJumpControl Opener = null;

        private System.Windows.Forms.Panel pnlSearch;
        private Janus.Windows.GridEX.EditControls.EditBox ebSearchKey;
        private System.Windows.Forms.Panel pnlBtn;
        private Janus.Windows.EditControls.UIButton btnClose;
        private Janus.Windows.EditControls.UIButton btnOk;
        private System.Windows.Forms.Panel panel1;
//        private System.ComponentModel.IContainer components;
        private Janus.Windows.EditControls.UIButton btnSearch;
		private System.Data.DataView dvContent;
		private Janus.Windows.GridEX.GridEX grdExContentList;
		private AdManagerClient.ChannelJumpDs channelJumpDs;			// 팝업호출시 매체셋트


		#endregion

		#region 생성자 및 소멸자
        public ChannelJump_SearchContentForm()
        {
            //
            // Windows Form 디자이너 지원에 필요합니다.
            //
            InitializeComponent();

            //
            
            //
        }

        /// <summary>
        /// 데이터 넘겨야 할 넘
        /// </summary>
        /// <param name="sender"></param>
        public ChannelJump_SearchContentForm(ChannelJumpControl sender)
        {
            //
            // Windows Form 디자이너 지원에 필요합니다.
            //
            InitializeComponent();

            //
            
            //
            
            Opener = sender;
        }


        /// <summary>
        /// 사용 중인 모든 리소스를 정리합니다.
        /// </summary>
        protected override void Dispose( bool disposing )
        {
            if( disposing )
            {
//                if(components != null)
//                {
//                    components.Dispose();
//                }
            }
            base.Dispose( disposing );
        }


        #endregion

        #region 이벤트핸들러
        public event StatusEventHandler 			StatusEvent;			// 상태이벤트 핸들러

        #endregion

        #region 사용자정의 객체 및 변수

        // 시스템 정보 : 화면공통
        private SystemModel   systemModel   = FrameSystem.oSysModel;
        private CommonModel   commonModel   = FrameSystem.oComModel;
        private Logger        log           = FrameSystem.oLog;


        // 사용할 정보모델
        ChannelJumpModel channelJumpModel  = new ChannelJumpModel();	// 채널점핑정보모델

        // 화면처리용 변수
        bool IsNewSearchKey		  = true;					// 검색어입력 여부
        CurrencyManager cm        = null;					// 데이터 그리드의 변경에 따른 데이터셋 관리를 위하여			
        DataTable       dt        = null;

        #endregion

        #region Windows Form 디자이너에서 생성한 코드
        /// <summary>
        /// 디자이너 지원에 필요한 메서드입니다.
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마십시오.
        /// </summary>
        private void InitializeComponent()
        {
            Janus.Windows.GridEX.GridEXLayout grdExContentList_Layout_0 = new Janus.Windows.GridEX.GridEXLayout();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ChannelJump_SearchContentForm));
            this.dvContent = new System.Data.DataView();
            this.channelJumpDs = new AdManagerClient.ChannelJumpDs();
            this.pnlSearch = new System.Windows.Forms.Panel();
            this.btnSearch = new Janus.Windows.EditControls.UIButton();
            this.ebSearchKey = new Janus.Windows.GridEX.EditControls.EditBox();
            this.pnlBtn = new System.Windows.Forms.Panel();
            this.btnClose = new Janus.Windows.EditControls.UIButton();
            this.btnOk = new Janus.Windows.EditControls.UIButton();
            this.panel1 = new System.Windows.Forms.Panel();
            this.grdExContentList = new Janus.Windows.GridEX.GridEX();
            ((System.ComponentModel.ISupportInitialize)(this.dvContent)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.channelJumpDs)).BeginInit();
            this.pnlSearch.SuspendLayout();
            this.pnlBtn.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdExContentList)).BeginInit();
            this.SuspendLayout();
            // 
            // dvContent
            // 
            this.dvContent.Table = this.channelJumpDs.ContentSearch;
            // 
            // channelJumpDs
            // 
            this.channelJumpDs.DataSetName = "ChannelJumpDs";
            this.channelJumpDs.Locale = new System.Globalization.CultureInfo("en-US");
            this.channelJumpDs.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // pnlSearch
            // 
            this.pnlSearch.BackColor = System.Drawing.SystemColors.Window;
            this.pnlSearch.Controls.Add(this.btnSearch);
            this.pnlSearch.Controls.Add(this.ebSearchKey);
            this.pnlSearch.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlSearch.Location = new System.Drawing.Point(0, 0);
            this.pnlSearch.Name = "pnlSearch";
            this.pnlSearch.Size = new System.Drawing.Size(784, 40);
            this.pnlSearch.TabIndex = 3;
            // 
            // btnSearch
            // 
            this.btnSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSearch.Location = new System.Drawing.Point(672, 8);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(104, 24);
            this.btnSearch.TabIndex = 2;
            this.btnSearch.Text = "조 회";
            this.btnSearch.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // ebSearchKey
            // 
            this.ebSearchKey.Location = new System.Drawing.Point(8, 8);
            this.ebSearchKey.Name = "ebSearchKey";
            this.ebSearchKey.Size = new System.Drawing.Size(168, 21);
            this.ebSearchKey.TabIndex = 1;
            this.ebSearchKey.Text = "검색어를 입력하세요";
            this.ebSearchKey.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
            this.ebSearchKey.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            this.ebSearchKey.TextChanged += new System.EventHandler(this.ebSearchKey_TextChanged);
            this.ebSearchKey.Click += new System.EventHandler(this.ebSearchKey_Click);
            this.ebSearchKey.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ebSearchKey_KeyDown);
            // 
            // pnlBtn
            // 
            this.pnlBtn.BackColor = System.Drawing.Color.WhiteSmoke;
            this.pnlBtn.Controls.Add(this.btnClose);
            this.pnlBtn.Controls.Add(this.btnOk);
            this.pnlBtn.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlBtn.Location = new System.Drawing.Point(0, 420);
            this.pnlBtn.Name = "pnlBtn";
            this.pnlBtn.Size = new System.Drawing.Size(784, 42);
            this.pnlBtn.TabIndex = 17;
            // 
            // btnClose
            // 
            this.btnClose.BackColor = System.Drawing.SystemColors.Control;
            this.btnClose.Location = new System.Drawing.Point(400, 8);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(104, 24);
            this.btnClose.TabIndex = 5;
            this.btnClose.Text = "취 소";
            this.btnClose.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnOk
            // 
            this.btnOk.BackColor = System.Drawing.SystemColors.Control;
            this.btnOk.Location = new System.Drawing.Point(288, 8);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(104, 24);
            this.btnOk.TabIndex = 4;
            this.btnOk.Text = "확 인";
            this.btnOk.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.WhiteSmoke;
            this.panel1.Controls.Add(this.grdExContentList);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 40);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(784, 380);
            this.panel1.TabIndex = 18;
            // 
            // grdExContentList
            // 
            this.grdExContentList.AlternatingColors = true;
            this.grdExContentList.BorderStyle = Janus.Windows.GridEX.BorderStyle.Flat;
            this.grdExContentList.DataSource = this.dvContent;
            this.grdExContentList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdExContentList.EmptyRows = true;
            this.grdExContentList.FocusCellFormatStyle.BackColor = System.Drawing.SystemColors.Highlight;
            this.grdExContentList.FocusCellFormatStyle.ForeColor = System.Drawing.Color.White;
            this.grdExContentList.FocusStyle = Janus.Windows.GridEX.FocusStyle.None;
            this.grdExContentList.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.grdExContentList.GridLines = Janus.Windows.GridEX.GridLines.Vertical;
            this.grdExContentList.GridLineStyle = Janus.Windows.GridEX.GridLineStyle.Solid;
            this.grdExContentList.GroupByBoxVisible = false;
            this.grdExContentList.GroupMode = Janus.Windows.GridEX.GroupMode.Collapsed;
            this.grdExContentList.HeaderFormatStyle.TextAlignment = Janus.Windows.GridEX.TextAlignment.Center;
            grdExContentList_Layout_0.DataSource = this.dvContent;
            grdExContentList_Layout_0.IsCurrentLayout = true;
            grdExContentList_Layout_0.Key = "bae";
            grdExContentList_Layout_0.LayoutString = resources.GetString("grdExContentList_Layout_0.LayoutString");
            this.grdExContentList.Layouts.AddRange(new Janus.Windows.GridEX.GridEXLayout[] {
            grdExContentList_Layout_0});
            this.grdExContentList.Location = new System.Drawing.Point(0, 0);
            this.grdExContentList.Name = "grdExContentList";
            this.grdExContentList.ScrollBars = Janus.Windows.GridEX.ScrollBars.Vertical;
            this.grdExContentList.Size = new System.Drawing.Size(784, 380);
            this.grdExContentList.TabIndex = 4;
            this.grdExContentList.TabKeyBehavior = Janus.Windows.GridEX.TabKeyBehavior.ControlNavigation;
            this.grdExContentList.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            this.grdExContentList.DoubleClick += new System.EventHandler(this.grdExContentList_DoubleClick);
            // 
            // ChannelJump_SearchContentForm
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(6, 14);
            this.ClientSize = new System.Drawing.Size(784, 462);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.pnlBtn);
            this.Controls.Add(this.pnlSearch);
            this.Name = "ChannelJump_SearchContentForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "컨텐츠검색";
            this.Load += new System.EventHandler(this.UserControl_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dvContent)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.channelJumpDs)).EndInit();
            this.pnlSearch.ResumeLayout(false);
            this.pnlSearch.PerformLayout();
            this.pnlBtn.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdExContentList)).EndInit();
            this.ResumeLayout(false);

		}
        #endregion

        #region 컨트롤 로드
        private void UserControl_Load(object sender, System.EventArgs e)
        {
            // 데이터관리용 객체생성
            dt = ((DataView)grdExContentList.DataSource).Table;  
            cm = (CurrencyManager) this.BindingContext[grdExContentList.DataSource]; 
            cm.PositionChanged += new System.EventHandler(OnGrdRowChanged); 

            // 컨트롤 초기화
            InitControl();	
        }

        #endregion

        #region 컨트롤 초기화
        private void InitControl()
        {
 
		}

        #endregion

        #region 계약정보 액션처리 메소드

        /// <summary>
        /// 그리드의 Row변경시
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnGrdRowChanged(object sender, System.EventArgs e) 
        {
            if(grdExContentList.RowCount > 0)
            {
                //                SetDetailText();
                //                InitButton();
            }
        }

        /// <summary>
        /// 조회버튼 클릭
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSearch_Click(object sender, System.EventArgs e)
        {
            SearchContent();
        }

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
            if(e.KeyCode == Keys.Enter)
            {
                SearchContent();
            }
        }

		private void btnOk_Click(object sender, System.EventArgs e)
		{
			SelectContent();
			this.Close();
		}

		private void btnClose_Click(object sender, System.EventArgs e)
		{
			this.Close();
		}

		private void grdExContentList_DoubleClick(object sender, System.EventArgs e)
		{
			SelectContent();
			this.Close();		
		}

        #endregion

        #region 처리메소드

        /// <summary>
        /// 채널검색
        /// </summary>
        private void SearchContent()
        {
            StatusMessage("컨텐츠 정보를 검색합니다.");

			if(IsNewSearchKey || ebSearchKey.Text.Trim().Length == 0) 
			{
				MessageBox.Show("검색어를 입력해 주시기 바랍니다.","컨텐츠검색", 
					MessageBoxButtons.OK, MessageBoxIcon.Information );
				return;
			}

            try
            {
                //검색 전에 모델을 초기화 해준다.
                channelJumpModel.Init();
                // 데이터모델에 전송할 내용을 셋트한다.
                channelJumpModel.SearchMediaCode      = keyMediaCode;
    
                if(IsNewSearchKey)
                {
                    channelJumpModel.SearchKey = "";
                }
                else
                {
                    channelJumpModel.SearchKey  = ebSearchKey.Text;
                }

                // 채널검색 서비스를 호출한다.
                new ChannelJumpManager(systemModel,commonModel).GetContentList(channelJumpModel);

                if (channelJumpModel.ResultCD.Equals("0000"))
                {
                    Utility.SetDataTable(channelJumpDs.ContentSearch, channelJumpModel.ContentListDataSet);				
                    StatusMessage(channelJumpModel.ResultCnt + "건의 컨텐트 정보가 조회되었습니다.");
					if(Convert.ToInt32(channelJumpModel.ResultCnt) > 0) cm.Position = 0;
					grdExContentList.EnsureVisible();

                }
            }
            catch(FrameException fe)
            {
                FrameSystem.showMsgForm("컨텐츠검색 오류", new string[] {fe.ErrCode, fe.ResultMsg});
            }
            catch(Exception ex)
            {
                FrameSystem.showMsgForm("컨텐츠검색 오류",new string[] {"",ex.Message});
            }
        }


		/// <summary>
		/// 광고를 선택
		/// </summary>
		private void SelectContent()
		{

			int curRow = cm.Position;
			if(curRow < 0) return;	// 데이터가 없으면 실행하지 않는다.

			string GenreCode      = dt.Rows[curRow]["GenreCode"].ToString();			
			string GenreName      = dt.Rows[curRow]["GenreName"].ToString();			
			string ContentID      = dt.Rows[curRow]["ContentID"].ToString();			
			string ContentName    = dt.Rows[curRow]["ContentName"].ToString();			
	
			Opener.SetContent(GenreCode, GenreName, ContentID, ContentName);
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
    }
}