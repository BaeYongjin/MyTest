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
    public class ChannelJump_SearchAdPopForm : System.Windows.Forms.Form
    {

		#region 사용자정의 객체 및 변수

		// 오프너
		private ChannelJumpControl Opener = null;
        private System.Windows.Forms.Panel pnlBtn;
        private Janus.Windows.EditControls.UIButton btnClose;
        private Janus.Windows.EditControls.UIButton btnOk;
        private System.Windows.Forms.Panel panel1;
		private Janus.Windows.GridEX.GridEX grdExAdPopList;
		private System.Data.DataView dvAdPopList;
		private AdManagerClient.ChannelJumpDs channelJumpDs;			// 팝업호출시 매체셋트


		#endregion

		#region 생성자 및 소멸자
       
        /// <summary>
        /// 데이터 넘겨야 할 넘
        /// </summary>
        /// <param name="sender"></param>
        public ChannelJump_SearchAdPopForm(ChannelJumpControl sender)
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

		public string keyType = "";
				
        // 시스템 정보 : 화면공통
        private SystemModel   systemModel   = FrameSystem.oSysModel;
        private CommonModel   commonModel   = FrameSystem.oComModel;
        private Logger        log           = FrameSystem.oLog;


        // 사용할 정보모델
        ChannelJumpModel channelJumpModel  = new ChannelJumpModel();	// 채널점핑정보모델

        // 화면처리용 변수
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
            Janus.Windows.GridEX.GridEXLayout grdExAdPopList_Layout_0 = new Janus.Windows.GridEX.GridEXLayout();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ChannelJump_SearchAdPopForm));
            this.dvAdPopList = new System.Data.DataView();
            this.channelJumpDs = new AdManagerClient.ChannelJumpDs();
            this.pnlBtn = new System.Windows.Forms.Panel();
            this.btnClose = new Janus.Windows.EditControls.UIButton();
            this.btnOk = new Janus.Windows.EditControls.UIButton();
            this.panel1 = new System.Windows.Forms.Panel();
            this.grdExAdPopList = new Janus.Windows.GridEX.GridEX();
            ((System.ComponentModel.ISupportInitialize)(this.dvAdPopList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.channelJumpDs)).BeginInit();
            this.pnlBtn.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdExAdPopList)).BeginInit();
            this.SuspendLayout();
            // 
            // dvAdPopList
            // 
            this.dvAdPopList.Table = this.channelJumpDs.AdPopList;
            // 
            // channelJumpDs
            // 
            this.channelJumpDs.DataSetName = "ChannelJumpDs";
            this.channelJumpDs.Locale = new System.Globalization.CultureInfo("en-US");
            this.channelJumpDs.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // pnlBtn
            // 
            this.pnlBtn.BackColor = System.Drawing.Color.WhiteSmoke;
            this.pnlBtn.Controls.Add(this.btnClose);
            this.pnlBtn.Controls.Add(this.btnOk);
            this.pnlBtn.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlBtn.Location = new System.Drawing.Point(0, 420);
            this.pnlBtn.Name = "pnlBtn";
            this.pnlBtn.Size = new System.Drawing.Size(680, 42);
            this.pnlBtn.TabIndex = 17;
            // 
            // btnClose
            // 
            this.btnClose.BackColor = System.Drawing.SystemColors.Control;
            this.btnClose.Location = new System.Drawing.Point(344, 8);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(104, 24);
            this.btnClose.TabIndex = 3;
            this.btnClose.Text = "취 소";
            this.btnClose.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnOk
            // 
            this.btnOk.BackColor = System.Drawing.SystemColors.Control;
            this.btnOk.Location = new System.Drawing.Point(232, 8);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(104, 24);
            this.btnOk.TabIndex = 2;
            this.btnOk.Text = "확 인";
            this.btnOk.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.WhiteSmoke;
            this.panel1.Controls.Add(this.grdExAdPopList);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(680, 420);
            this.panel1.TabIndex = 18;
            // 
            // grdExAdPopList
            // 
            this.grdExAdPopList.AllowEdit = Janus.Windows.GridEX.InheritableBoolean.False;
            this.grdExAdPopList.AlternatingColors = true;
            this.grdExAdPopList.BorderStyle = Janus.Windows.GridEX.BorderStyle.Flat;
            this.grdExAdPopList.DataSource = this.dvAdPopList;
            this.grdExAdPopList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdExAdPopList.EmptyRows = true;
            this.grdExAdPopList.FocusCellFormatStyle.BackColor = System.Drawing.SystemColors.Highlight;
            this.grdExAdPopList.FocusCellFormatStyle.ForeColor = System.Drawing.Color.White;
            this.grdExAdPopList.FocusStyle = Janus.Windows.GridEX.FocusStyle.None;
            this.grdExAdPopList.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.grdExAdPopList.FrozenColumns = 2;
            this.grdExAdPopList.GridLines = Janus.Windows.GridEX.GridLines.Vertical;
            this.grdExAdPopList.GridLineStyle = Janus.Windows.GridEX.GridLineStyle.Solid;
            this.grdExAdPopList.GroupByBoxVisible = false;
            this.grdExAdPopList.GroupMode = Janus.Windows.GridEX.GroupMode.Collapsed;
            this.grdExAdPopList.HeaderFormatStyle.TextAlignment = Janus.Windows.GridEX.TextAlignment.Center;
            grdExAdPopList_Layout_0.DataSource = this.dvAdPopList;
            grdExAdPopList_Layout_0.IsCurrentLayout = true;
            grdExAdPopList_Layout_0.Key = "bae";
            grdExAdPopList_Layout_0.LayoutString = resources.GetString("grdExAdPopList_Layout_0.LayoutString");
            this.grdExAdPopList.Layouts.AddRange(new Janus.Windows.GridEX.GridEXLayout[] {
            grdExAdPopList_Layout_0});
            this.grdExAdPopList.Location = new System.Drawing.Point(0, 0);
            this.grdExAdPopList.Name = "grdExAdPopList";
            this.grdExAdPopList.Size = new System.Drawing.Size(680, 420);
            this.grdExAdPopList.TabIndex = 1;
            this.grdExAdPopList.TabKeyBehavior = Janus.Windows.GridEX.TabKeyBehavior.ControlNavigation;
            this.grdExAdPopList.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            this.grdExAdPopList.DoubleClick += new System.EventHandler(this.grdExAdPopList_DoubleClick);
            // 
            // ChannelJump_SearchAdPopForm
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(6, 14);
            this.ClientSize = new System.Drawing.Size(680, 462);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.pnlBtn);
            this.Name = "ChannelJump_SearchAdPopForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "팝업공지검색";
            this.Load += new System.EventHandler(this.UserControl_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dvAdPopList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.channelJumpDs)).EndInit();
            this.pnlBtn.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdExAdPopList)).EndInit();
            this.ResumeLayout(false);

		}
        #endregion

        #region 컨트롤 로드
        private void UserControl_Load(object sender, System.EventArgs e)
        {
            // 데이터관리용 객체생성
            dt = ((DataView)grdExAdPopList.DataSource).Table;  
            cm = (CurrencyManager) this.BindingContext[grdExAdPopList.DataSource]; 
            cm.PositionChanged += new System.EventHandler(OnGrdRowChanged); 

            // 컨트롤 초기화
            InitControl();	
        }

        #endregion

        #region 컨트롤 초기화
		private void InitControl()
		{
			SearchAdPopList();
		}

        #endregion

        #region 액션처리 메소드

        /// <summary>
        /// 그리드의 Row변경시
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnGrdRowChanged(object sender, System.EventArgs e) 
        {
            if(grdExAdPopList.RowCount > 0)
            {
                //                SetDetailText();
                //                InitButton();
            }
        }

		private void btnOk_Click(object sender, System.EventArgs e)
		{
			SelectAdPopList();
			this.Close();
		}

		private void btnClose_Click(object sender, System.EventArgs e)
		{
			this.Close();
		}

		private void grdExAdPopList_DoubleClick(object sender, System.EventArgs e)
		{
			SelectAdPopList();
			this.Close();		
		}

        #endregion

        #region 처리메소드

        /// <summary>
        /// 광고계약목록 조회
        /// </summary>
        private void SearchAdPopList()
        {
            try
            {
                //검색 전에 모델을 초기화 해준다.
                channelJumpModel.Init();
			
				channelJumpModel.Type      = keyType;
                // 광고 계약 목록 서비스를 호출한다.
                new ChannelJumpManager(systemModel,commonModel).GetAdPopList(channelJumpModel);

                if (channelJumpModel.ResultCD.Equals("0000"))
                {
					if(channelJumpModel.ResultCnt > 0)
					{
						Utility.SetDataTable(channelJumpDs.AdPopList, channelJumpModel.AdPopListDataSet);				
					}

					if(Convert.ToInt32(channelJumpModel.ResultCnt) > 0) cm.Position = 0;
					grdExAdPopList.EnsureVisible();

                }
            }
            catch(FrameException fe)
            {
                FrameSystem.showMsgForm("팝업공지 조회오류", new string[] {fe.ErrCode, fe.ResultMsg});
            }
            catch(Exception ex)
            {
                FrameSystem.showMsgForm("팝업공지 조회오류",new string[] {"",ex.Message});
            }
        }


		/// <summary>
		/// 광고를 선택
		/// </summary>
		private void SelectAdPopList()
		{

			int curRow = cm.Position;
			if(curRow < 0) return;	// 데이터가 없으면 실행하지 않는다.

			string PopID        = dt.Rows[curRow]["nid"].ToString();			
			string Title        = dt.Rows[curRow]["ntitle"].ToString();			
	
			Opener.SetPopup(PopID, Title);
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