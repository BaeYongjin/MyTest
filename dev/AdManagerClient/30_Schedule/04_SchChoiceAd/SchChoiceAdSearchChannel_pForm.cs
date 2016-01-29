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
    /// SchChoiceAdSearchChannel_pForm에 대한 요약 설명입니다.
    /// </summary>
    /// 

    public class SchChoiceAdSearchChannel_pForm : System.Windows.Forms.Form
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
        ChannelGroupModel channelGroupModel  = new ChannelGroupModel();	// 장르정보모델
		SchChoiceAdModel  schChoiceAdModel   = new SchChoiceAdModel();	// 지정광고편성모델

		// 이 창을 연 컨트롤
		SchChoiceAdControl Opener = null;
       

        // 화면처리용 변수
        public String MediaCode   = null;
        CurrencyManager cm        = null;					// 데이터 그리드의 변경에 따른 데이터셋 관리를 위하여
        CurrencyManager cmChannel = null;					// 데이터 그리드의 변경에 따른 데이터셋 관리를 위하여
        DataTable       dt        = null;
        DataTable       dtChannel = null;

        #endregion

		#region  생성자 소멸자 컨트롤선언

        private System.Windows.Forms.Panel pnlBtn;
        private Janus.Windows.EditControls.UIButton btnClose;
        private Janus.Windows.EditControls.UIButton btnOk;
        private System.Data.DataView dvGenre;
        private System.Data.DataView dvChannel;
		private Janus.Windows.UI.Dock.UIPanelManager uiPM;
		private Janus.Windows.UI.Dock.UIPanelGroup uiPanel0;
		private Janus.Windows.UI.Dock.UIPanel uiPanel1;
		private Janus.Windows.UI.Dock.UIPanelInnerContainer uiPanel1Container;
		private Janus.Windows.UI.Dock.UIPanel uiPanel2;
		private Janus.Windows.UI.Dock.UIPanelInnerContainer uiPanel2Container;
		private Janus.Windows.GridEX.GridEX grdExGenreList;
		private AdManagerClient.SchChoiceAdSearch_pDs schChoiceAdSearch_pDs;
		private Janus.Windows.GridEX.GridEX grdExChannelList;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private Janus.Windows.GridEX.EditControls.EditBox ebChannelNo;
        private System.ComponentModel.IContainer components;

        /// <summary>
        /// 데이터 넘겨야 할 넘
        /// </summary>
        /// <param name="sender"></param>
        public SchChoiceAdSearchChannel_pForm(UserControl parent)
        {
            //
            // Windows Form 디자이너 지원에 필요합니다.
            //
            InitializeComponent();

			// 이창을 호출한 컨트롤
			Opener = (SchChoiceAdControl) parent;
			channelGroupModel.Init();
		}

        /// <summary>
        /// 일반사용자
        /// </summary>
        public SchChoiceAdSearchChannel_pForm()
        {
            //
            // Windows Form 디자이너 지원에 필요합니다.
            //
            InitializeComponent();

			channelGroupModel.Init();
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

		#endregion

        #region Windows Form 디자이너에서 생성한 코드
        /// <summary>
        /// 디자이너 지원에 필요한 메서드입니다.
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마십시오.
        /// </summary>
        private void InitializeComponent()
        {
			this.components = new System.ComponentModel.Container();
			Janus.Windows.GridEX.GridEXLayout grdExGenreList_Layout_0 = new Janus.Windows.GridEX.GridEXLayout();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SchChoiceAdSearchChannel_pForm));
			Janus.Windows.GridEX.GridEXLayout grdExChannelList_Layout_0 = new Janus.Windows.GridEX.GridEXLayout();
			this.dvGenre = new System.Data.DataView();
			this.schChoiceAdSearch_pDs = new AdManagerClient.SchChoiceAdSearch_pDs();
			this.dvChannel = new System.Data.DataView();
			this.pnlBtn = new System.Windows.Forms.Panel();
			this.label2 = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.ebChannelNo = new Janus.Windows.GridEX.EditControls.EditBox();
			this.btnClose = new Janus.Windows.EditControls.UIButton();
			this.btnOk = new Janus.Windows.EditControls.UIButton();
			this.uiPM = new Janus.Windows.UI.Dock.UIPanelManager(this.components);
			this.uiPanel0 = new Janus.Windows.UI.Dock.UIPanelGroup();
			this.uiPanel1 = new Janus.Windows.UI.Dock.UIPanel();
			this.uiPanel1Container = new Janus.Windows.UI.Dock.UIPanelInnerContainer();
			this.grdExGenreList = new Janus.Windows.GridEX.GridEX();
			this.uiPanel2 = new Janus.Windows.UI.Dock.UIPanel();
			this.uiPanel2Container = new Janus.Windows.UI.Dock.UIPanelInnerContainer();
			this.grdExChannelList = new Janus.Windows.GridEX.GridEX();
			((System.ComponentModel.ISupportInitialize)(this.dvGenre)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.schChoiceAdSearch_pDs)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.dvChannel)).BeginInit();
			this.pnlBtn.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.uiPM)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.uiPanel0)).BeginInit();
			this.uiPanel0.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.uiPanel1)).BeginInit();
			this.uiPanel1.SuspendLayout();
			this.uiPanel1Container.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.grdExGenreList)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.uiPanel2)).BeginInit();
			this.uiPanel2.SuspendLayout();
			this.uiPanel2Container.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.grdExChannelList)).BeginInit();
			this.SuspendLayout();
			// 
			// dvGenre
			// 
			this.dvGenre.Table = this.schChoiceAdSearch_pDs.Genre;
			// 
			// schChoiceAdSearch_pDs
			// 
			this.schChoiceAdSearch_pDs.DataSetName = "SchChoiceAdSearch_pDs";
			this.schChoiceAdSearch_pDs.Locale = new System.Globalization.CultureInfo("en-US");
			this.schChoiceAdSearch_pDs.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
			// 
			// dvChannel
			// 
			this.dvChannel.Table = this.schChoiceAdSearch_pDs.Channel;
			// 
			// pnlBtn
			// 
			this.pnlBtn.BackColor = System.Drawing.Color.WhiteSmoke;
			this.pnlBtn.Controls.Add(this.label2);
			this.pnlBtn.Controls.Add(this.label1);
			this.pnlBtn.Controls.Add(this.ebChannelNo);
			this.pnlBtn.Controls.Add(this.btnClose);
			this.pnlBtn.Controls.Add(this.btnOk);
			this.pnlBtn.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.pnlBtn.Location = new System.Drawing.Point(0, 426);
			this.pnlBtn.Name = "pnlBtn";
			this.pnlBtn.Size = new System.Drawing.Size(792, 40);
			this.pnlBtn.TabIndex = 16;
			// 
			// label2
			// 
			this.label2.BackColor = System.Drawing.Color.Transparent;
			this.label2.Location = new System.Drawing.Point(392, 8);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(72, 21);
			this.label2.TabIndex = 13;
			this.label2.Text = "채널번호";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label1
			// 
			this.label1.BackColor = System.Drawing.Color.Transparent;
			this.label1.Location = new System.Drawing.Point(8, 8);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(264, 21);
			this.label1.TabIndex = 12;
			this.label1.Text = "채널번호를 직접 입력하셔도 편성 가능합니다.";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// ebChannelNo
			// 
			this.ebChannelNo.Location = new System.Drawing.Point(464, 8);
			this.ebChannelNo.MaxLength = 10;
			this.ebChannelNo.Name = "ebChannelNo";
			this.ebChannelNo.Size = new System.Drawing.Size(88, 21);
			this.ebChannelNo.TabIndex = 3;
			this.ebChannelNo.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
			this.ebChannelNo.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
			this.ebChannelNo.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ebChannelNo_KeyDown);
			// 
			// btnClose
			// 
			this.btnClose.BackColor = System.Drawing.SystemColors.Control;
			this.btnClose.Location = new System.Drawing.Point(680, 8);
			this.btnClose.Name = "btnClose";
			this.btnClose.Size = new System.Drawing.Size(104, 23);
			this.btnClose.TabIndex = 5;
			this.btnClose.Text = "취 소";
			this.btnClose.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
			this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
			// 
			// btnOk
			// 
			this.btnOk.BackColor = System.Drawing.SystemColors.Control;
			this.btnOk.Location = new System.Drawing.Point(568, 8);
			this.btnOk.Name = "btnOk";
			this.btnOk.Size = new System.Drawing.Size(104, 23);
			this.btnOk.TabIndex = 4;
			this.btnOk.Text = "편 성";
			this.btnOk.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
			this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
			// 
			// uiPM
			// 
			this.uiPM.BackColorGradientAutoHideStrip = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(218)))), ((int)(((byte)(250)))));
			this.uiPM.ContainerControl = this;
			this.uiPM.VisualStyle = Janus.Windows.UI.Dock.PanelVisualStyle.Office2007;
			this.uiPanel0.Id = new System.Guid("083e8f61-9d7d-4e6b-831f-be80f6b8df6d");
			this.uiPanel0.StaticGroup = true;
			this.uiPanel1.Id = new System.Guid("e04fffea-30bd-41ee-bac8-eb8c6dae0c44");
			this.uiPanel0.Panels.Add(this.uiPanel1);
			this.uiPanel2.Id = new System.Guid("af08113b-57a7-4b6c-8b5d-55bc6195bc72");
			this.uiPanel0.Panels.Add(this.uiPanel2);
			this.uiPM.Panels.Add(this.uiPanel0);
			// 
			// Design Time Panel Info:
			// 
			this.uiPM.BeginPanelInfo();
			this.uiPM.AddDockPanelInfo(new System.Guid("083e8f61-9d7d-4e6b-831f-be80f6b8df6d"), Janus.Windows.UI.Dock.PanelGroupStyle.VerticalTiles, Janus.Windows.UI.Dock.PanelDockStyle.Fill, true, new System.Drawing.Size(786, 420), true);
			this.uiPM.AddDockPanelInfo(new System.Guid("e04fffea-30bd-41ee-bac8-eb8c6dae0c44"), new System.Guid("083e8f61-9d7d-4e6b-831f-be80f6b8df6d"), 196, true);
			this.uiPM.AddDockPanelInfo(new System.Guid("af08113b-57a7-4b6c-8b5d-55bc6195bc72"), new System.Guid("083e8f61-9d7d-4e6b-831f-be80f6b8df6d"), 196, true);
			this.uiPM.AddFloatingPanelInfo(new System.Guid("083e8f61-9d7d-4e6b-831f-be80f6b8df6d"), Janus.Windows.UI.Dock.PanelGroupStyle.VerticalTiles, true, new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
			this.uiPM.AddFloatingPanelInfo(new System.Guid("e04fffea-30bd-41ee-bac8-eb8c6dae0c44"), new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
			this.uiPM.AddFloatingPanelInfo(new System.Guid("af08113b-57a7-4b6c-8b5d-55bc6195bc72"), new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
			this.uiPM.EndPanelInfo();
			// 
			// uiPanel0
			// 
			this.uiPanel0.CaptionVisible = Janus.Windows.UI.InheritableBoolean.False;
			this.uiPanel0.GroupStyle = Janus.Windows.UI.Dock.PanelGroupStyle.VerticalTiles;
			this.uiPanel0.Location = new System.Drawing.Point(3, 3);
			this.uiPanel0.Name = "uiPanel0";
			this.uiPanel0.Size = new System.Drawing.Size(786, 420);
			this.uiPanel0.TabIndex = 4;
			this.uiPanel0.Text = "지정채널광고 채널편성";
			// 
			// uiPanel1
			// 
			this.uiPanel1.InnerContainer = this.uiPanel1Container;
			this.uiPanel1.Location = new System.Drawing.Point(0, 0);
			this.uiPanel1.Name = "uiPanel1";
			this.uiPanel1.Size = new System.Drawing.Size(391, 420);
			this.uiPanel1.TabIndex = 4;
			this.uiPanel1.Text = "메뉴";
			// 
			// uiPanel1Container
			// 
			this.uiPanel1Container.Controls.Add(this.grdExGenreList);
			this.uiPanel1Container.Location = new System.Drawing.Point(1, 23);
			this.uiPanel1Container.Name = "uiPanel1Container";
			this.uiPanel1Container.Size = new System.Drawing.Size(389, 396);
			this.uiPanel1Container.TabIndex = 0;
			// 
			// grdExGenreList
			// 
			this.grdExGenreList.AlternatingColors = true;
			this.grdExGenreList.AutomaticSort = false;
			this.grdExGenreList.BorderStyle = Janus.Windows.GridEX.BorderStyle.None;
			this.grdExGenreList.DataSource = this.dvGenre;
			this.grdExGenreList.Dock = System.Windows.Forms.DockStyle.Fill;
			this.grdExGenreList.EmptyRows = true;
			this.grdExGenreList.FocusStyle = Janus.Windows.GridEX.FocusStyle.None;
			this.grdExGenreList.GridLines = Janus.Windows.GridEX.GridLines.Vertical;
			this.grdExGenreList.GridLineStyle = Janus.Windows.GridEX.GridLineStyle.Solid;
			this.grdExGenreList.GroupByBoxVisible = false;
			this.grdExGenreList.GroupMode = Janus.Windows.GridEX.GroupMode.Collapsed;
			this.grdExGenreList.HideSelection = Janus.Windows.GridEX.HideSelection.HighlightInactive;
			grdExGenreList_Layout_0.DataSource = this.dvGenre;
			grdExGenreList_Layout_0.IsCurrentLayout = true;
			grdExGenreList_Layout_0.Key = "bae";
			grdExGenreList_Layout_0.LayoutString = resources.GetString("grdExGenreList_Layout_0.LayoutString");
			this.grdExGenreList.Layouts.AddRange(new Janus.Windows.GridEX.GridEXLayout[] {
            grdExGenreList_Layout_0});
			this.grdExGenreList.Location = new System.Drawing.Point(0, 0);
			this.grdExGenreList.Name = "grdExGenreList";
			this.grdExGenreList.ScrollBars = Janus.Windows.GridEX.ScrollBars.Vertical;
			this.grdExGenreList.SelectedInactiveFormatStyle.BackColor = System.Drawing.Color.Gold;
			this.grdExGenreList.Size = new System.Drawing.Size(389, 396);
			this.grdExGenreList.TabIndex = 1;
			this.grdExGenreList.TabKeyBehavior = Janus.Windows.GridEX.TabKeyBehavior.ControlNavigation;
			this.grdExGenreList.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
			// 
			// uiPanel2
			// 
			this.uiPanel2.InnerContainer = this.uiPanel2Container;
			this.uiPanel2.Location = new System.Drawing.Point(395, 0);
			this.uiPanel2.Name = "uiPanel2";
			this.uiPanel2.Size = new System.Drawing.Size(391, 420);
			this.uiPanel2.TabIndex = 4;
			this.uiPanel2.Text = "채널";
			// 
			// uiPanel2Container
			// 
			this.uiPanel2Container.Controls.Add(this.grdExChannelList);
			this.uiPanel2Container.Location = new System.Drawing.Point(1, 23);
			this.uiPanel2Container.Name = "uiPanel2Container";
			this.uiPanel2Container.Size = new System.Drawing.Size(389, 396);
			this.uiPanel2Container.TabIndex = 0;
			// 
			// grdExChannelList
			// 
			this.grdExChannelList.AlternatingColors = true;
			this.grdExChannelList.BorderStyle = Janus.Windows.GridEX.BorderStyle.None;
			this.grdExChannelList.DataSource = this.dvChannel;
			this.grdExChannelList.Dock = System.Windows.Forms.DockStyle.Fill;
			this.grdExChannelList.EmptyRows = true;
			this.grdExChannelList.FocusStyle = Janus.Windows.GridEX.FocusStyle.None;
			this.grdExChannelList.GridLines = Janus.Windows.GridEX.GridLines.Vertical;
			this.grdExChannelList.GridLineStyle = Janus.Windows.GridEX.GridLineStyle.Solid;
			this.grdExChannelList.GroupByBoxVisible = false;
			this.grdExChannelList.GroupMode = Janus.Windows.GridEX.GroupMode.Collapsed;
			grdExChannelList_Layout_0.DataSource = this.dvChannel;
			grdExChannelList_Layout_0.IsCurrentLayout = true;
			grdExChannelList_Layout_0.Key = "bae";
			grdExChannelList_Layout_0.LayoutString = resources.GetString("grdExChannelList_Layout_0.LayoutString");
			this.grdExChannelList.Layouts.AddRange(new Janus.Windows.GridEX.GridEXLayout[] {
            grdExChannelList_Layout_0});
			this.grdExChannelList.Location = new System.Drawing.Point(0, 0);
			this.grdExChannelList.Name = "grdExChannelList";
			this.grdExChannelList.ScrollBars = Janus.Windows.GridEX.ScrollBars.Vertical;
			this.grdExChannelList.Size = new System.Drawing.Size(389, 396);
			this.grdExChannelList.TabIndex = 2;
			this.grdExChannelList.TabKeyBehavior = Janus.Windows.GridEX.TabKeyBehavior.ControlNavigation;
			this.grdExChannelList.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
			this.grdExChannelList.CellValueChanged += new Janus.Windows.GridEX.ColumnActionEventHandler(this.grdExChannelList_CellValueChanged);
			this.grdExChannelList.ColumnHeaderClick += new Janus.Windows.GridEX.ColumnActionEventHandler(this.grdExChannelList_ColumnHeaderClick);
			// 
			// SchChoiceAdSearchChannel_pForm
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(6, 14);
			this.ClientSize = new System.Drawing.Size(792, 466);
			this.Controls.Add(this.uiPanel0);
			this.Controls.Add(this.pnlBtn);
			this.Font = global::AdManagerClient.Properties.Settings.Default.Font1;
			this.Name = "SchChoiceAdSearchChannel_pForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "채널편성";
			this.Load += new System.EventHandler(this.SchChoiceAdSearchChannel_pForm_Load);
			((System.ComponentModel.ISupportInitialize)(this.dvGenre)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.schChoiceAdSearch_pDs)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.dvChannel)).EndInit();
			this.pnlBtn.ResumeLayout(false);
			this.pnlBtn.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.uiPM)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.uiPanel0)).EndInit();
			this.uiPanel0.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.uiPanel1)).EndInit();
			this.uiPanel1.ResumeLayout(false);
			this.uiPanel1Container.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.grdExGenreList)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.uiPanel2)).EndInit();
			this.uiPanel2.ResumeLayout(false);
			this.uiPanel2Container.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.grdExChannelList)).EndInit();
			this.ResumeLayout(false);

		}
        #endregion

        #region 컨트롤 로드
        private void SchChoiceAdSearchChannel_pForm_Load(object sender, System.EventArgs e)
        {           
            // 데이터관리용 객체생성
            dt        = ((DataView)grdExGenreList.DataSource).Table;  
            dtChannel = ((DataView)grdExChannelList.DataSource).Table;  
            cm        = (CurrencyManager) this.BindingContext[grdExGenreList.DataSource]; 
            cm.PositionChanged += new System.EventHandler(OnGrdRowChanged);

            cmChannel = (CurrencyManager)this.BindingContext[grdExChannelList.DataSource];
            //cm.PositionChanged += new System.EventHandler(OnGrdRowChanged); 

            SearchGenreList();
        }
        #endregion

        #region 사용자 액션처리 메소드

		/// <summary>
		/// 그리드의 Row변경시
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void OnGrdRowChanged(object sender, System.EventArgs e) 
		{
			if(grdExGenreList.RecordCount > 0 )
			{
				SearchChannelList();
			}

		}
     
		private void btnClose_Click(object sender, System.EventArgs e)
		{
			this.Close();
		}

		private void btnOk_Click(object sender, System.EventArgs e)
		{
			AddSchChoiceAdDeailChannel();
		}

		private void btnSearch_Click(object sender, System.EventArgs e)
		{
			SearchGenreList();
		}

		private void grdExChannelList_ColumnHeaderClick(object sender, Janus.Windows.GridEX.ColumnActionEventArgs e)
		{
            
			//컬럼Index 0(체크박스컬럼이)이 아니면 빠져나가게 처리.
			if(e.Column.Index != 0)
			{
				return;
			}
            
			//ColumnHeader Click시에 dt Setting 
			DataRow[] foundRows = dtChannel.Select("CheckYn = 'False'");
         
			if( foundRows.Length == 0 )
			{
				for(int i=0;i < dtChannel.Rows.Count;i++)
				{
					dtChannel.Rows[i].BeginEdit();
					dtChannel.Rows[i]["CheckYn"]="False";
					dtChannel.Rows[i].EndEdit();
				}
			}
			else
			{
				for(int i=0;i < dtChannel.Rows.Count;i++)
				{
					dtChannel.Rows[i].BeginEdit();
					dtChannel.Rows[i]["CheckYn"]="True";
					dtChannel.Rows[i].EndEdit();
				}
			}
		}

		private void ebChannelNo_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
		{
			if(e.KeyCode == Keys.Enter)
			{
				AddSchChoiceAdDeailChannel();
			}		
		}

		#endregion
  
        #region 처리메소드

        /// <summary>
        /// 장르목록 조회
        /// </summary>
        private void SearchGenreList()
        {
            StatusMessage("장르 정보를 조회합니다.");

            try
            {
				channelGroupModel.Init();

				// 데이터모델에 전송할 내용을 셋트한다.
				channelGroupModel.MediaCode = Opener.MediaCode;

				if(channelGroupModel.MediaCode.Trim().Length == 0)
				{
					MessageBox.Show("광고내역이 선택되지 않았습니다.", "조회오류",
						MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				}

                // 장르목록조회 서비스를 호출한다.
                new ChannelGroupSearch_pManager(systemModel,commonModel).GetGenreList(channelGroupModel);

                if (channelGroupModel.ResultCD.Equals("0000"))
                {
                    Utility.SetDataTable(schChoiceAdSearch_pDs.Genre, channelGroupModel.ChannelGroup_pDataSet);				
                    StatusMessage(channelGroupModel.ResultCnt + "건의 장르 정보가 조회되었습니다.");
					SearchChannelList();
                }
            }
            catch(FrameException fe)
            {
                FrameSystem.showMsgForm("장르조회오류", new string[] {fe.ErrCode, fe.ResultMsg});
            }
            catch(Exception ex)
            {
                FrameSystem.showMsgForm("장르조회오류",new string[] {"",ex.Message});
            }
        }
        /// <summary>
        /// 채널목록 조회
        /// </summary>
        private void SearchChannelList()
        {
            StatusMessage("채널 정보를 조회합니다.");

			int curRow = cm.Position;

			if(curRow < 0) return;

            try
            {
				grdExChannelList.UnCheckAllRecords();

                // 데이터모델에 전송할 내용을 셋트한다.
                channelGroupModel.MediaCode    = dt.Rows[curRow]["MediaCode"].ToString();
                channelGroupModel.CategoryCode = dt.Rows[curRow]["CategoryCode"].ToString();
                channelGroupModel.GenreCode    = dt.Rows[curRow]["GenreCode"].ToString();  

                // 채널목록조회 서비스를 호출한다.
                new ChannelGroupSearch_pManager(systemModel,commonModel).GetChannelList(channelGroupModel);

                if (channelGroupModel.ResultCD.Equals("0000"))
                {
                    Utility.SetDataTable(schChoiceAdSearch_pDs.Channel, channelGroupModel.ChannelGroup_pDataSet);				
                    StatusMessage(channelGroupModel.ResultCnt + "건의 장르 정보가 조회되었습니다.");
                }
            }
            catch(FrameException fe)
            {
                FrameSystem.showMsgForm("채널조회오류", new string[] {fe.ErrCode, fe.ResultMsg});
            }
            catch(Exception ex)
            {
                FrameSystem.showMsgForm("채널조회오류",new string[] {"",ex.Message});
            }
        }

		/// <summary>
		/// 선택된 채널를 지정채널광고 상세내역에 편성
		/// </summary>
		private void AddSchChoiceAdDeailChannel()
		{
			// 그리드에 변경된 데이터를 Datasource에 업데이트 한다.
			grdExChannelList.UpdateData();

			try
			{
				int SetCount = 0;

				//인서트 시킴
				for(int i=0;i < schChoiceAdSearch_pDs.Channel.Rows.Count;i++)
				{
					DataRow row = schChoiceAdSearch_pDs.Channel.Rows[i];

					if(row["CheckYn"].ToString().Equals("True"))
					{
						schChoiceAdModel.Init();

						schChoiceAdModel.ItemNo    = Opener.ItemNo;
						schChoiceAdModel.MediaCode = Opener.MediaCode;
						schChoiceAdModel.ChannelNo = row["ChannelNo"].ToString();
						schChoiceAdModel.Title     = row["Title"].ToString();
						schChoiceAdModel.AdType    = Opener.AdType;

						new SchChoiceAdManager(systemModel,commonModel).SetSchChoiceChannelDetailAdd(schChoiceAdModel);

						if(schChoiceAdModel.ResultCD.Equals("0000"))
						{
							SetCount++;
						}
					}
				}


				// 채널번호 직접입력이 있을경우
				if(ebChannelNo.Text.Trim().Length > 0)
				{
					schChoiceAdModel.Init();

					schChoiceAdModel.ItemNo    = Opener.ItemNo;
					schChoiceAdModel.MediaCode = Opener.MediaCode;
					schChoiceAdModel.ChannelNo = ebChannelNo.Text.Trim();
					schChoiceAdModel.AdType    = Opener.AdType;

					new SchChoiceAdManager(systemModel,commonModel).SetSchChoiceChannelDetailAdd(schChoiceAdModel);

					if(schChoiceAdModel.ResultCD.Equals("0000"))
					{
						SetCount++;
						ebChannelNo.Text = "";
					}
				}

				// 체크된것 클리어
				ClearListCheck();

				// 호출한 컨트롤의 메뉴목록을 갱신한다.
				Opener.ReloadChannelList();

				if(SetCount > 0)
				{
//					MessageBox.Show("지정하신 채널에 편성되었습니다.", "채널광고편성",
//						MessageBoxButtons.OK, MessageBoxIcon.Information);
				}

			}
			catch(FrameException fe)
			{
				FrameSystem.showMsgForm("지정메뉴광고 상세편성오류", new string[] {fe.ErrCode, fe.ResultMsg});
			}
			catch(Exception ex)
			{
				FrameSystem.showMsgForm("지정메뉴광고 상세편성오류",new string[] {"",ex.Message});
			}			
		}

		private void ClearListCheck()
		{

			// 체크된 모든 항목을 클리어
			grdExChannelList.UnCheckAllRecords();
			grdExChannelList.UpdateData();
				   
			// 데이터 클리어
			for(int i=0;i < dtChannel.Rows.Count;i++)
			{
				dtChannel.Rows[i].BeginEdit();
				dtChannel.Rows[i]["CheckYn"]="False";
				dtChannel.Rows[i].EndEdit();
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

        private void grdExChannelList_CellValueChanged(object sender, Janus.Windows.GridEX.ColumnActionEventArgs e)
        {
            if (e.Column.EditType == Janus.Windows.GridEX.EditType.CheckBox)
            {
                int curRow = cmChannel.Position;
                if (curRow >= 0)
                {
                    dtChannel.Rows[curRow].BeginEdit();
                    dtChannel.Rows[curRow]["CheckYn"] = grdExChannelList.GetValue(e.Column).ToString();
                    dtChannel.Rows[curRow].EndEdit();
                }
            }
        }
    }
    #endregion


}
