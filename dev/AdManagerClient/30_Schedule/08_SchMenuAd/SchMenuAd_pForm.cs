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
    /// SchChoiceAdSearchMenu_pForm에 대한 요약 설명입니다.
    /// </summary>
    /// 

    public class SchMenuAd_pForm : System.Windows.Forms.Form
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
        GenreGroupModel  genreGroupModel   = new GenreGroupModel();		// 장르정보모델
		SchChoiceAdModel schChoiceAdModel  = new SchChoiceAdModel();	// 지정광고편성모델

		// 이 창을 연 컨트롤
		SchMenuAdControl Opener = null;
  
        // 화면처리용 변수
        DataTable       dt        = null;

        #endregion

		#region  생성자 소멸자 컨트롤선언

        private System.Windows.Forms.Panel pnlBtn;
        private Janus.Windows.EditControls.UIButton btnClose;
        private Janus.Windows.EditControls.UIButton btnOk;
        private System.Data.DataView dvGenre;
		private AdManagerClient.SchChoiceAdSearch_pDs schChoiceAdSearch_pDs;
		private Janus.Windows.UI.Dock.UIPanelManager uiPM;
		private Janus.Windows.UI.Dock.UIPanelGroup uiPanel0;
		private Janus.Windows.UI.Dock.UIPanel uiPanel1;
		private Janus.Windows.UI.Dock.UIPanelInnerContainer uiPanel1Container;
		private Janus.Windows.GridEX.GridEX grdExGenreList;
        private System.ComponentModel.IContainer components;

        /// <summary>
        /// 데이터 넘겨야 할 넘
        /// </summary>
        /// <param name="sender"></param>
        public SchMenuAd_pForm(UserControl parent)
        {
            //
            // Windows Form 디자이너 지원에 필요합니다.
            //
            InitializeComponent();
			Opener = (SchMenuAdControl) parent;

			genreGroupModel.Init();
        }

        /// <summary>
        /// 일반사용자
        /// </summary>
        public SchMenuAd_pForm()
        {
            //
            // Windows Form 디자이너 지원에 필요합니다.
            //
            InitializeComponent();
            
			genreGroupModel.Init();
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SchMenuAd_pForm));
			this.dvGenre = new System.Data.DataView();
			this.schChoiceAdSearch_pDs = new AdManagerClient.SchChoiceAdSearch_pDs();
			this.pnlBtn = new System.Windows.Forms.Panel();
			this.btnClose = new Janus.Windows.EditControls.UIButton();
			this.btnOk = new Janus.Windows.EditControls.UIButton();
			this.uiPM = new Janus.Windows.UI.Dock.UIPanelManager(this.components);
			this.uiPanel0 = new Janus.Windows.UI.Dock.UIPanelGroup();
			this.uiPanel1 = new Janus.Windows.UI.Dock.UIPanel();
			this.uiPanel1Container = new Janus.Windows.UI.Dock.UIPanelInnerContainer();
			this.grdExGenreList = new Janus.Windows.GridEX.GridEX();
			((System.ComponentModel.ISupportInitialize)(this.dvGenre)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.schChoiceAdSearch_pDs)).BeginInit();
			this.pnlBtn.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.uiPM)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.uiPanel0)).BeginInit();
			this.uiPanel0.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.uiPanel1)).BeginInit();
			this.uiPanel1.SuspendLayout();
			this.uiPanel1Container.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.grdExGenreList)).BeginInit();
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
			// pnlBtn
			// 
			this.pnlBtn.BackColor = System.Drawing.Color.WhiteSmoke;
			this.pnlBtn.Controls.Add(this.btnClose);
			this.pnlBtn.Controls.Add(this.btnOk);
			this.pnlBtn.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.pnlBtn.Location = new System.Drawing.Point(0, 509);
			this.pnlBtn.Name = "pnlBtn";
			this.pnlBtn.Size = new System.Drawing.Size(513, 44);
			this.pnlBtn.TabIndex = 3;
			// 
			// btnClose
			// 
			this.btnClose.BackColor = System.Drawing.SystemColors.Control;
			this.btnClose.Cursor = System.Windows.Forms.Cursors.Hand;
			this.btnClose.ImageIndex = 1;
			this.btnClose.Location = new System.Drawing.Point(264, 9);
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
			this.btnOk.Cursor = System.Windows.Forms.Cursors.Hand;
			this.btnOk.ImageIndex = 0;
			this.btnOk.Location = new System.Drawing.Point(152, 9);
			this.btnOk.Name = "btnOk";
			this.btnOk.Size = new System.Drawing.Size(104, 24);
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
			this.uiPanel0.Id = new System.Guid("37b8c3c2-301d-4a8e-aaca-c4970f80c1a9");
			this.uiPanel0.StaticGroup = true;
			this.uiPanel1.Id = new System.Guid("984fe0c4-19ba-40fc-8766-7e094025e19a");
			this.uiPanel0.Panels.Add(this.uiPanel1);
			this.uiPM.Panels.Add(this.uiPanel0);
			// 
			// Design Time Panel Info:
			// 
			this.uiPM.BeginPanelInfo();
			this.uiPM.AddDockPanelInfo(new System.Guid("37b8c3c2-301d-4a8e-aaca-c4970f80c1a9"), Janus.Windows.UI.Dock.PanelGroupStyle.VerticalTiles, Janus.Windows.UI.Dock.PanelDockStyle.Fill, true, new System.Drawing.Size(507, 503), true);
			this.uiPM.AddDockPanelInfo(new System.Guid("984fe0c4-19ba-40fc-8766-7e094025e19a"), new System.Guid("37b8c3c2-301d-4a8e-aaca-c4970f80c1a9"), 507, true);
			this.uiPM.AddFloatingPanelInfo(new System.Guid("37b8c3c2-301d-4a8e-aaca-c4970f80c1a9"), Janus.Windows.UI.Dock.PanelGroupStyle.VerticalTiles, true, new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
			this.uiPM.AddFloatingPanelInfo(new System.Guid("984fe0c4-19ba-40fc-8766-7e094025e19a"), new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
			this.uiPM.EndPanelInfo();
			// 
			// uiPanel0
			// 
			this.uiPanel0.CaptionVisible = Janus.Windows.UI.InheritableBoolean.False;
			this.uiPanel0.GroupStyle = Janus.Windows.UI.Dock.PanelGroupStyle.VerticalTiles;
			this.uiPanel0.Location = new System.Drawing.Point(3, 3);
			this.uiPanel0.Name = "uiPanel0";
			this.uiPanel0.Size = new System.Drawing.Size(507, 503);
			this.uiPanel0.TabIndex = 4;
			this.uiPanel0.Text = "지정메뉴광고 메뉴편성";
			// 
			// uiPanel1
			// 
			this.uiPanel1.InnerContainer = this.uiPanel1Container;
			this.uiPanel1.Location = new System.Drawing.Point(0, 0);
			this.uiPanel1.Name = "uiPanel1";
			this.uiPanel1.Size = new System.Drawing.Size(507, 503);
			this.uiPanel1.TabIndex = 1;
			this.uiPanel1.Text = "메뉴";
			// 
			// uiPanel1Container
			// 
			this.uiPanel1Container.Controls.Add(this.grdExGenreList);
			this.uiPanel1Container.Location = new System.Drawing.Point(1, 23);
			this.uiPanel1Container.Name = "uiPanel1Container";
			this.uiPanel1Container.Size = new System.Drawing.Size(505, 479);
			this.uiPanel1Container.TabIndex = 0;
			// 
			// grdExGenreList
			// 
			this.grdExGenreList.AlternatingColors = true;
			this.grdExGenreList.BorderStyle = Janus.Windows.GridEX.BorderStyle.None;
			this.grdExGenreList.CellToolTipText = "장르를 더블클릭하시면 해당 카테고리의 하위장르가 전체 선택됩니다";
			this.grdExGenreList.DataSource = this.dvGenre;
			this.grdExGenreList.Dock = System.Windows.Forms.DockStyle.Fill;
			this.grdExGenreList.EmptyRows = true;
			this.grdExGenreList.FocusStyle = Janus.Windows.GridEX.FocusStyle.None;
			this.grdExGenreList.GridLines = Janus.Windows.GridEX.GridLines.Vertical;
			this.grdExGenreList.GridLineStyle = Janus.Windows.GridEX.GridLineStyle.Solid;
			this.grdExGenreList.GroupByBoxVisible = false;
			this.grdExGenreList.GroupMode = Janus.Windows.GridEX.GroupMode.Collapsed;
			this.grdExGenreList.Hierarchical = true;
			grdExGenreList_Layout_0.DataSource = this.dvGenre;
			grdExGenreList_Layout_0.IsCurrentLayout = true;
			grdExGenreList_Layout_0.Key = "bae";
			grdExGenreList_Layout_0.LayoutString = resources.GetString("grdExGenreList_Layout_0.LayoutString");
			this.grdExGenreList.Layouts.AddRange(new Janus.Windows.GridEX.GridEXLayout[] {
            grdExGenreList_Layout_0});
			this.grdExGenreList.Location = new System.Drawing.Point(0, 0);
			this.grdExGenreList.Name = "grdExGenreList";
			this.grdExGenreList.ScrollBars = Janus.Windows.GridEX.ScrollBars.Vertical;
			this.grdExGenreList.Size = new System.Drawing.Size(505, 479);
			this.grdExGenreList.TabIndex = 2;
			this.grdExGenreList.TabKeyBehavior = Janus.Windows.GridEX.TabKeyBehavior.ControlNavigation;
			this.grdExGenreList.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
			this.grdExGenreList.RowDoubleClick += new Janus.Windows.GridEX.RowActionEventHandler(this.grdExGenreList_RowDoubleClick);
			this.grdExGenreList.ColumnHeaderClick += new Janus.Windows.GridEX.ColumnActionEventHandler(this.grdExGenreList_ColumnHeaderClick);
			// 
			// SchMenuAd_pForm
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.ClientSize = new System.Drawing.Size(513, 553);
			this.Controls.Add(this.uiPanel0);
			this.Controls.Add(this.pnlBtn);
			this.Font = global::AdManagerClient.Properties.Settings.Default.Font1;
			this.MaximizeBox = false;
			this.Name = "SchMenuAd_pForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "편성 채널선택";
			this.Load += new System.EventHandler(this.SchMenuAd_pForm_Load);
			((System.ComponentModel.ISupportInitialize)(this.dvGenre)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.schChoiceAdSearch_pDs)).EndInit();
			this.pnlBtn.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.uiPM)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.uiPanel0)).EndInit();
			this.uiPanel0.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.uiPanel1)).EndInit();
			this.uiPanel1.ResumeLayout(false);
			this.uiPanel1Container.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.grdExGenreList)).EndInit();
			this.ResumeLayout(false);

		}
        #endregion

        #region 컨트롤 로드
		private void SchMenuAd_pForm_Load(object sender, EventArgs e)
		{
			// 데이터관리용 객체생성
			dt = ((DataView)grdExGenreList.DataSource).Table;

			SearchGenre();
		}
        #endregion

        #region 사용자 액션처리 메소드

		private void btnOk_Click(object sender, System.EventArgs e)
		{
            AddSchChoiceAdDeailMenu();
		}
		private void btnClose_Click(object sender, System.EventArgs e)
		{
			this.Close();
		}


		private void btnSearch_Click(object sender, System.EventArgs e)
		{
			SearchGenre();
		}

		private void grdExGenreList_ColumnHeaderClick(object sender, Janus.Windows.GridEX.ColumnActionEventArgs e)
		{
            
			//컬럼Index 0(체크박스컬럼이)이 아니면 빠져나가게 처리.
			if(e.Column.Index != 0)
			{
				return;
			}
            
			//ColumnHeader Click시에 dt Setting 
			DataRow[] foundRows = dt.Select("CheckYn = 'False'");
         
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
        /// 컨텐츠목록 조회
        /// </summary>
        private void SearchGenre()
        {
            StatusMessage("장르목록을 조회합니다.");

            try
            {
				grdExGenreList.UnCheckAllRecords();

				genreGroupModel.Init();
                genreGroupModel.MediaCode = Opener.keyMediaCode;

				if(genreGroupModel.MediaCode.Trim().Length == 0)
				{
					MessageBox.Show("광고내역이 선택되지 않았습니다.", "조회오류",MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				}

                // 컨텐츠목록조회 서비스를 호출한다.
                new GenreGroupSearch_pManager(systemModel,commonModel).GetChannelList_p(genreGroupModel);

                if (genreGroupModel.ResultCD.Equals("0000"))
                {
                    Utility.SetDataTable(schChoiceAdSearch_pDs.Genre, genreGroupModel.GenreGroupDataSet);				
                    StatusMessage(genreGroupModel.ResultCnt + "건의 채널 정보가 조회되었습니다.");
                }
            }
            catch(FrameException fe)
            {
                FrameSystem.showMsgForm("장르조회 오류", new string[] {fe.ErrCode, fe.ResultMsg});
            }
            catch(Exception ex)
            {
                FrameSystem.showMsgForm("장르조회 오류",new string[] {"",ex.Message});
            }
        }

		/// <summary>
		/// 선택한 채널을 채널편성테이블에 추가합니다.
		/// </summary>
		private void AddSchChoiceAdDeailMenu()
		{
			StatusMessage("선택한 채널을 재핑광고에 편성합니다.");

			grdExGenreList.UpdateData();

			try
			{
				int SetCount = 0;

				//인서트 시킴
				for(int i=0;i < schChoiceAdSearch_pDs.Genre.Rows.Count;i++)
				{
					DataRow row = schChoiceAdSearch_pDs.Genre.Rows[i];

					if(row["CheckYn"].ToString().Equals("True"))
					{
						schChoiceAdModel.Init();

						schChoiceAdModel.ItemNo    = Opener.keyItemNo;
						schChoiceAdModel.MediaCode = row["MediaCode"].ToString();
						schChoiceAdModel.GenreCode = row["GenreCode"].ToString();
						schChoiceAdModel.GenreName = row["GenreName"].ToString();
						schChoiceAdModel.CategoryCode = row["CategoryCode"].ToString();
						schChoiceAdModel.AdType = "31";

						new SchChoiceAdManager(systemModel, commonModel).SetSchChoiceRealChDetailAdd(schChoiceAdModel);
						
						if(schChoiceAdModel.ResultCD.Equals("0000"))
						{
							SetCount++;
						}
					}
				}

				// 체크된 모든 항목을 클리어
				ClearListCheck();

				// 호출한 컨트롤의 메뉴목록을 갱신한다.
				Opener.ReloadMenuList();
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
			grdExGenreList.UnCheckAllRecords();
			grdExGenreList.UpdateData();
				   
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

		private void grdExGenreList_RowDoubleClick(object sender, Janus.Windows.GridEX.RowActionEventArgs e)
		{
			Debug.WriteLine( e.Row.RowType.ToString() );
			// Row더블클릭시 해당 그룹 전체 선택됨
			if( e.Row.RowType == Janus.Windows.GridEX.RowType.Record )
			{
				string	groupName = e.Row.Cells["CategoryName"].Text;
				for(int i=0;i < schChoiceAdSearch_pDs.Genre.Rows.Count;i++)
				{
					if( schChoiceAdSearch_pDs.Genre.Rows[i]["CategoryName"].ToString().Equals( groupName ) )
					{
						schChoiceAdSearch_pDs.Genre.Rows[i].BeginEdit();
						
						if( schChoiceAdSearch_pDs.Genre.Rows[i]["CheckYn"].ToString() == "True" )	schChoiceAdSearch_pDs.Genre.Rows[i]["CheckYn"]="False";
						else																		schChoiceAdSearch_pDs.Genre.Rows[i]["CheckYn"]="True";

						schChoiceAdSearch_pDs.Genre.Rows[i].EndEdit();
					}
				}
			}
		}
    }
}