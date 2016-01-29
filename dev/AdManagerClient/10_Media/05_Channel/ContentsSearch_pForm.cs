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

namespace AdManagerClient
{
    /// <summary>
    /// ContentsSearch_pForm에 대한 요약 설명입니다.
    /// </summary>
    /// 



    public class ContentsSearch_pForm : System.Windows.Forms.Form
    {
       


        #region 이벤트핸들러
        public event StatusEventHandler 			StatusEvent;			// 상태이벤트 핸들러
        #endregion
			
        #region 사용자정의 객체 및 변수

        // 시스템 정보 : 화면공통
        private SystemModel   systemModel   = FrameSystem.oSysModel;
        private CommonModel   commonModel   = FrameSystem.oComModel;
        private Logger        log           = FrameSystem.oLog;

        // 사용할 정보모델
        ContentsModel contentsModel  = new ContentsModel();	// 컨텐츠정보모델

        ChannelControl ChannelCtl = null;

        // 화면처리용 변수
        bool IsNewSearchKey		  = true;					// 검색어입력 여부
        CurrencyManager cm        = null;					// 데이터 그리드의 변경에 따른 데이터셋 관리를 위하여			
        DataTable       dt        = null;

        #endregion



        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.ImageList imageList;
        private System.Data.DataView dvContents;
        private System.Windows.Forms.Panel pnlBtn;
        private Janus.Windows.EditControls.UIButton btnClose;
        private Janus.Windows.EditControls.UIButton btnOk;
        private Janus.Windows.EditControls.UIButton btnSearch;
        private Janus.Windows.GridEX.EditControls.EditBox ebSearchKey;
        private AdManagerClient.Contents_pDs contents_pDs;
		private Janus.Windows.GridEX.GridEX grdExContentsList;
        private System.ComponentModel.IContainer components;

        /// <summary>
        /// 데이터 넘겨야 할 넘
        /// </summary>
        /// <param name="sender"></param>
        public ContentsSearch_pForm(ChannelControl sender)
        {
            //
            // Windows Form 디자이너 지원에 필요합니다.
            //
            InitializeComponent();

            //
            
            //
            
            ChannelCtl = sender;
        }

        /// <summary>
        /// 일반사용자
        /// </summary>
        public ContentsSearch_pForm()
        {
            //
            // Windows Form 디자이너 지원에 필요합니다.
            //
            InitializeComponent();

            //
            
            //
            
            ChannelCtl = null;
        }

        /// <summary>
        /// 사용 중인 모든 리소스를 정리합니다.
        /// </summary>
        protected override void Dispose( bool disposing )
        {
            if( disposing )
            {
                if(components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose( disposing );
        }

        #region Windows Form 디자이너에서 생성한 코드
        /// <summary>
        /// 디자이너 지원에 필요한 메서드입니다.
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마십시오.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ContentsSearch_pForm));
            Janus.Windows.GridEX.GridEXLayout grdExContentsList_Layout_0 = new Janus.Windows.GridEX.GridEXLayout();
            this.dvContents = new System.Data.DataView();
            this.contents_pDs = new AdManagerClient.Contents_pDs();
            this.panel4 = new System.Windows.Forms.Panel();
            this.btnSearch = new Janus.Windows.EditControls.UIButton();
            this.ebSearchKey = new Janus.Windows.GridEX.EditControls.EditBox();
            this.imageList = new System.Windows.Forms.ImageList(this.components);
            this.pnlBtn = new System.Windows.Forms.Panel();
            this.btnClose = new Janus.Windows.EditControls.UIButton();
            this.btnOk = new Janus.Windows.EditControls.UIButton();
            this.grdExContentsList = new Janus.Windows.GridEX.GridEX();
            ((System.ComponentModel.ISupportInitialize)(this.dvContents)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.contents_pDs)).BeginInit();
            this.panel4.SuspendLayout();
            this.pnlBtn.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdExContentsList)).BeginInit();
            this.SuspendLayout();
            // 
            // dvContents
            // 
            this.dvContents.Table = this.contents_pDs.Contents;
            // 
            // contents_pDs
            // 
            this.contents_pDs.DataSetName = "Contents_pDs";
            this.contents_pDs.Locale = new System.Globalization.CultureInfo("en-US");
            this.contents_pDs.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // panel4
            // 
            this.panel4.BackColor = System.Drawing.Color.White;
            this.panel4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel4.Controls.Add(this.btnSearch);
            this.panel4.Controls.Add(this.ebSearchKey);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel4.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.panel4.Location = new System.Drawing.Point(0, 0);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(592, 40);
            this.panel4.TabIndex = 15;
            // 
            // btnSearch
            // 
            this.btnSearch.BackColor = System.Drawing.Color.White;
            this.btnSearch.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSearch.Location = new System.Drawing.Point(480, 8);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(104, 24);
            this.btnSearch.TabIndex = 4;
            this.btnSearch.Text = "조 회";
            this.btnSearch.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // ebSearchKey
            // 
            this.ebSearchKey.ButtonFont = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ebSearchKey.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ebSearchKey.Location = new System.Drawing.Point(8, 8);
            this.ebSearchKey.Name = "ebSearchKey";
            this.ebSearchKey.Size = new System.Drawing.Size(208, 21);
            this.ebSearchKey.TabIndex = 2;
            this.ebSearchKey.Text = "검색어를 입력하세요";
            this.ebSearchKey.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
            this.ebSearchKey.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            this.ebSearchKey.TextChanged += new System.EventHandler(this.ebSearchKey_TextChanged);
            this.ebSearchKey.Click += new System.EventHandler(this.ebSearchKey_Click);
            this.ebSearchKey.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ebSearchKey_KeyDown);
            // 
            // imageList
            // 
            this.imageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList.ImageStream")));
            this.imageList.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList.Images.SetKeyName(0, "");
            this.imageList.Images.SetKeyName(1, "");
            this.imageList.Images.SetKeyName(2, "");
            // 
            // pnlBtn
            // 
            this.pnlBtn.BackColor = System.Drawing.Color.WhiteSmoke;
            this.pnlBtn.Controls.Add(this.btnClose);
            this.pnlBtn.Controls.Add(this.btnOk);
            this.pnlBtn.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlBtn.Location = new System.Drawing.Point(0, 426);
            this.pnlBtn.Name = "pnlBtn";
            this.pnlBtn.Size = new System.Drawing.Size(592, 40);
            this.pnlBtn.TabIndex = 16;
            // 
            // btnClose
            // 
            this.btnClose.BackColor = System.Drawing.SystemColors.Control;
            this.btnClose.ImageIndex = 1;
            this.btnClose.ImageList = this.imageList;
            this.btnClose.Location = new System.Drawing.Point(296, 8);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(72, 23);
            this.btnClose.TabIndex = 8;
            this.btnClose.Text = "닫기";
            this.btnClose.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnOk
            // 
            this.btnOk.BackColor = System.Drawing.SystemColors.Control;
            this.btnOk.ImageIndex = 0;
            this.btnOk.ImageList = this.imageList;
            this.btnOk.Location = new System.Drawing.Point(216, 8);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(72, 23);
            this.btnOk.TabIndex = 9;
            this.btnOk.Text = "확인";
            this.btnOk.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // grdExContentsList
            // 
            this.grdExContentsList.AlternatingColors = true;
            this.grdExContentsList.BorderStyle = Janus.Windows.GridEX.BorderStyle.None;
            this.grdExContentsList.DataSource = this.dvContents;
            this.grdExContentsList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdExContentsList.EmptyRows = true;
            this.grdExContentsList.FocusStyle = Janus.Windows.GridEX.FocusStyle.None;
            this.grdExContentsList.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.grdExContentsList.GridLines = Janus.Windows.GridEX.GridLines.Vertical;
            this.grdExContentsList.GridLineStyle = Janus.Windows.GridEX.GridLineStyle.Solid;
            this.grdExContentsList.GroupByBoxVisible = false;
            grdExContentsList_Layout_0.DataSource = this.dvContents;
            grdExContentsList_Layout_0.IsCurrentLayout = true;
            grdExContentsList_Layout_0.Key = "bae";
            grdExContentsList_Layout_0.LayoutString = resources.GetString("grdExContentsList_Layout_0.LayoutString");
            this.grdExContentsList.Layouts.AddRange(new Janus.Windows.GridEX.GridEXLayout[] {
            grdExContentsList_Layout_0});
            this.grdExContentsList.Location = new System.Drawing.Point(0, 40);
            this.grdExContentsList.Name = "grdExContentsList";
            this.grdExContentsList.ScrollBars = Janus.Windows.GridEX.ScrollBars.Vertical;
            this.grdExContentsList.Size = new System.Drawing.Size(592, 386);
            this.grdExContentsList.TabIndex = 17;
            this.grdExContentsList.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            // 
            // ContentsSearch_pForm
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(6, 14);
            this.ClientSize = new System.Drawing.Size(592, 466);
            this.Controls.Add(this.grdExContentsList);
            this.Controls.Add(this.pnlBtn);
            this.Controls.Add(this.panel4);
            this.Name = "ContentsSearch_pForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "컨텐츠검색";
            this.Load += new System.EventHandler(this.ContentsSearch_pForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dvContents)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.contents_pDs)).EndInit();
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.pnlBtn.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdExContentsList)).EndInit();
            this.ResumeLayout(false);

        }
        #endregion

        #region 컨트롤 로드
        private void ContentsSearch_pForm_Load(object sender, System.EventArgs e)
        {

            
            // 데이터관리용 객체생성
            dt = ((DataView)grdExContentsList.DataSource).Table;  
            cm = (CurrencyManager) this.BindingContext[grdExContentsList.DataSource]; 
            //cm.PositionChanged += new System.EventHandler(OnGrdRowChanged); 

            // 컨트롤 초기화
            //InitControl();
            //SearchContents();


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
                SearchContents();
            }
        }

        #endregion
  
        #region 처리메소드

        /// <summary>
        /// 컨텐츠목록 조회
        /// </summary>
        private void SearchContents()
        {
            StatusMessage("컨텐츠 정보를 조회합니다.");

            try
            {

                //저장 전에 모델을 초기화 해준다.
                contentsModel.Init();

                // 데이터모델에 전송할 내용을 셋트한다.
                if(IsNewSearchKey || ebSearchKey.Text.Length == 0)
                {
                    contentsModel.SearchKey = "";
					MessageBox.Show("검색어를 입력하여 주세요.","채널정보검색 오류", 
						MessageBoxButtons.OK, MessageBoxIcon.Information );			
						ebSearchKey.Text="";
						ebSearchKey.Focus();
					return;							                   
                }
                else
                {
                    contentsModel.SearchKey  = ebSearchKey.Text;
                }


                // 컨텐츠목록조회 서비스를 호출한다.
                new ContentsSearch_pManager(systemModel,commonModel).GetContentsListPopUp(contentsModel);

                if (contentsModel.ResultCD.Equals("0000"))
                {
                    Utility.SetDataTable(contents_pDs.Contents, contentsModel.ContentsDataSet);				
                    StatusMessage(contentsModel.ResultCnt + "건의 컨텐츠 정보가 조회되었습니다.");
                }
            }
            catch(FrameException fe)
            {
                FrameSystem.showMsgForm("컨텐츠조회오류", new string[] {fe.ErrCode, fe.ResultMsg});
            }
            catch(Exception ex)
            {
                FrameSystem.showMsgForm("컨텐츠조회오류",new string[] {"",ex.Message});
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

        private void btnOk_Click(object sender, System.EventArgs e)
        {
            int i=0;
               
            foreach (Janus.Windows.GridEX.GridEXRow gr in grdExContentsList.GetRows())
            {					
                
                if(gr.Cells["CheckYn"].Value.ToString().Equals("True"))
                {
                    dt.Rows[i].BeginEdit();
                    dt.Rows[i]["CheckYn"]="True";
                    dt.Rows[i].EndEdit();

                }
                else
                {
                    dt.Rows[i].BeginEdit();
                    dt.Rows[i]["CheckYn"]="False";
                    dt.Rows[i].EndEdit();

                }
                i++;
            }
            dt.AcceptChanges();
            //ChannelCtl.adOn_AddContent(contents_pDs);
            this.Close();
        }

        



        private void btnClose_Click(object sender, System.EventArgs e)
        {
            this.Close();
        }

        private void btnSearch_Click(object sender, System.EventArgs e)
        {
            SearchContents();
        }
    }
   

    #endregion


}
