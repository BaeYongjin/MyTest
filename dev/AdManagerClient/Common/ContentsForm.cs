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

namespace AdManagerClient.Common
{
	/// <summary>
	/// ContentsForm에 대한 요약 설명입니다.
	/// </summary>
	public class ContentsForm : System.Windows.Forms.Form
	{
        #region 사용자정의 객체 및 변수
        // 시스템 정보 : 화면공통
        private SystemModel   systemModel   = FrameSystem.oSysModel;
        private CommonModel   commonModel   = FrameSystem.oComModel;
        private Logger        log           = FrameSystem.oLog;

        // 사용할 정보모델
        ContentsModel   modelContents   = new ContentsModel();
        CurrencyManager cmContents      = null;
        DataTable       dtContents      = null;
        #endregion

        private Janus.Windows.GridEX.GridEX grdExContentList;
        private System.Windows.Forms.Panel pnlBtn;
        private Janus.Windows.EditControls.UIButton btnClose;
        private Janus.Windows.EditControls.UIButton btnOk;
        private System.Windows.Forms.Panel pnlSearch;
        private Janus.Windows.EditControls.UIButton btnSearch;
        private Janus.Windows.GridEX.EditControls.EditBox ebSearchKey;
        private System.Windows.Forms.Panel pnlGrid;
        private AdManagerClient.Common.ContentsDs contentsDs;
        private System.Data.DataView dvContents;
		/// <summary>
		/// 필수 디자이너 변수입니다.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public ContentsForm()
		{
			//
			// Windows Form 디자이너 지원에 필요합니다.
			//
			InitializeComponent();

			//
			
			//
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
            Janus.Windows.GridEX.GridEXLayout grdExContentList_Layout_0 = new Janus.Windows.GridEX.GridEXLayout();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ContentsForm));
            this.dvContents = new System.Data.DataView();
            this.contentsDs = new AdManagerClient.Common.ContentsDs();
            this.pnlGrid = new System.Windows.Forms.Panel();
            this.grdExContentList = new Janus.Windows.GridEX.GridEX();
            this.pnlBtn = new System.Windows.Forms.Panel();
            this.btnClose = new Janus.Windows.EditControls.UIButton();
            this.btnOk = new Janus.Windows.EditControls.UIButton();
            this.pnlSearch = new System.Windows.Forms.Panel();
            this.btnSearch = new Janus.Windows.EditControls.UIButton();
            this.ebSearchKey = new Janus.Windows.GridEX.EditControls.EditBox();
            ((System.ComponentModel.ISupportInitialize)(this.dvContents)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.contentsDs)).BeginInit();
            this.pnlGrid.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdExContentList)).BeginInit();
            this.pnlBtn.SuspendLayout();
            this.pnlSearch.SuspendLayout();
            this.SuspendLayout();
            // 
            // dvContents
            // 
            this.dvContents.AllowDelete = false;
            this.dvContents.AllowEdit = false;
            this.dvContents.AllowNew = false;
            this.dvContents.Table = this.contentsDs.Contents;
            // 
            // contentsDs
            // 
            this.contentsDs.DataSetName = "ContentsDs";
            this.contentsDs.Locale = new System.Globalization.CultureInfo("en-US");
            this.contentsDs.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // pnlGrid
            // 
            this.pnlGrid.BackColor = System.Drawing.Color.WhiteSmoke;
            this.pnlGrid.Controls.Add(this.grdExContentList);
            this.pnlGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlGrid.Location = new System.Drawing.Point(0, 40);
            this.pnlGrid.Name = "pnlGrid";
            this.pnlGrid.Size = new System.Drawing.Size(779, 403);
            this.pnlGrid.TabIndex = 21;
            // 
            // grdExContentList
            // 
            this.grdExContentList.AlternatingColors = true;
            this.grdExContentList.BorderStyle = Janus.Windows.GridEX.BorderStyle.Flat;
            this.grdExContentList.DataSource = this.dvContents;
            this.grdExContentList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdExContentList.EmptyRows = true;
            this.grdExContentList.FocusCellFormatStyle.BackColor = System.Drawing.SystemColors.Highlight;
            this.grdExContentList.FocusCellFormatStyle.ForeColor = System.Drawing.Color.White;
            this.grdExContentList.FocusStyle = Janus.Windows.GridEX.FocusStyle.None;
            this.grdExContentList.Font = new System.Drawing.Font("맑은 고딕", 8.25F);
            this.grdExContentList.FrozenColumns = 4;
            this.grdExContentList.GridLines = Janus.Windows.GridEX.GridLines.Vertical;
            this.grdExContentList.GridLineStyle = Janus.Windows.GridEX.GridLineStyle.Solid;
            this.grdExContentList.GroupByBoxVisible = false;
            this.grdExContentList.GroupMode = Janus.Windows.GridEX.GroupMode.Collapsed;
            this.grdExContentList.HeaderFormatStyle.TextAlignment = Janus.Windows.GridEX.TextAlignment.Center;
            grdExContentList_Layout_0.DataSource = this.dvContents;
            grdExContentList_Layout_0.IsCurrentLayout = true;
            grdExContentList_Layout_0.Key = "bae";
            grdExContentList_Layout_0.LayoutString = resources.GetString("grdExContentList_Layout_0.LayoutString");
            this.grdExContentList.Layouts.AddRange(new Janus.Windows.GridEX.GridEXLayout[] {
            grdExContentList_Layout_0});
            this.grdExContentList.Location = new System.Drawing.Point(0, 0);
            this.grdExContentList.Name = "grdExContentList";
            this.grdExContentList.ScrollBars = Janus.Windows.GridEX.ScrollBars.Vertical;
            this.grdExContentList.Size = new System.Drawing.Size(779, 403);
            this.grdExContentList.TabIndex = 4;
            this.grdExContentList.TabKeyBehavior = Janus.Windows.GridEX.TabKeyBehavior.ControlNavigation;
            this.grdExContentList.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            // 
            // pnlBtn
            // 
            this.pnlBtn.BackColor = System.Drawing.Color.WhiteSmoke;
            this.pnlBtn.Controls.Add(this.btnClose);
            this.pnlBtn.Controls.Add(this.btnOk);
            this.pnlBtn.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlBtn.Location = new System.Drawing.Point(0, 443);
            this.pnlBtn.Name = "pnlBtn";
            this.pnlBtn.Size = new System.Drawing.Size(779, 42);
            this.pnlBtn.TabIndex = 20;
            // 
            // btnClose
            // 
            this.btnClose.BackColor = System.Drawing.SystemColors.Control;
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.No;
            this.btnClose.Location = new System.Drawing.Point(396, 8);
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
            this.btnOk.DialogResult = System.Windows.Forms.DialogResult.Yes;
            this.btnOk.Location = new System.Drawing.Point(284, 8);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(104, 24);
            this.btnOk.TabIndex = 4;
            this.btnOk.Text = "확 인";
            this.btnOk.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // pnlSearch
            // 
            this.pnlSearch.BackColor = System.Drawing.SystemColors.Window;
            this.pnlSearch.Controls.Add(this.btnSearch);
            this.pnlSearch.Controls.Add(this.ebSearchKey);
            this.pnlSearch.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlSearch.Location = new System.Drawing.Point(0, 0);
            this.pnlSearch.Name = "pnlSearch";
            this.pnlSearch.Size = new System.Drawing.Size(779, 40);
            this.pnlSearch.TabIndex = 19;
            // 
            // btnSearch
            // 
            this.btnSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSearch.Location = new System.Drawing.Point(667, 8);
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
            this.ebSearchKey.Size = new System.Drawing.Size(312, 22);
            this.ebSearchKey.TabIndex = 1;
            this.ebSearchKey.Text = "검색할 채널명을 입력하세요";
            this.ebSearchKey.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
            this.ebSearchKey.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            // 
            // ContentsForm
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(6, 15);
            this.ClientSize = new System.Drawing.Size(779, 485);
            this.Controls.Add(this.pnlGrid);
            this.Controls.Add(this.pnlBtn);
            this.Controls.Add(this.pnlSearch);
            this.Font = new System.Drawing.Font("맑은 고딕", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "ContentsForm";
            this.Text = "컨텐츠 찾기";
            this.Load += new System.EventHandler(this.ContentsForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dvContents)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.contentsDs)).EndInit();
            this.pnlGrid.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdExContentList)).EndInit();
            this.pnlBtn.ResumeLayout(false);
            this.pnlSearch.ResumeLayout(false);
            this.pnlSearch.PerformLayout();
            this.ResumeLayout(false);

        }
		#endregion

        private void ContentsForm_Load(object sender, System.EventArgs e)
        {
            dtContents  = ((DataView)grdExContentList.DataSource).Table;
            cmContents  = (CurrencyManager)this.BindingContext[ grdExContentList.DataSource];
            cmContents.PositionChanged += new EventHandler(cmContents_PositionChanged);
        }


        private int     keyCategoryCode = 0;
        private string  keyCategoryName = "";
        private int     keyGenreCode    = 0;
        private string  keyGenreName    = "";
        private int     keyChannelNo    = 0;
        private int     keySeriesNo     = 0;
        private string  keyTitle        = "";
        private string  keySubTitle     = "";
        private string  keyContentId    = "";

        private void cmContents_PositionChanged(object sender, EventArgs e)
        {
            int currRow = cmContents.Position;

            if( currRow > -1 )
            {
                keyCategoryCode = Convert.ToInt32( dtContents.Rows[currRow]["CategoryCode"].ToString());
                keyCategoryName = dtContents.Rows[currRow]["CategoryName"].ToString();
                keyGenreCode    = Convert.ToInt32( dtContents.Rows[currRow]["GenreCode"].ToString());
                keyGenreName    = dtContents.Rows[currRow]["GenreName"].ToString();
                keyChannelNo    = Convert.ToInt32( dtContents.Rows[currRow]["ChannelNo"].ToString());
                keySeriesNo     = Convert.ToInt32( dtContents.Rows[currRow]["SeriesNo"].ToString());
                keyTitle        = dtContents.Rows[currRow]["Title"].ToString();
                keySubTitle     = dtContents.Rows[currRow]["SubTitle"].ToString();
                keyContentId    = dtContents.Rows[currRow]["ContentId"].ToString();
            }
        }

        /// <summary>
        /// 컨텐츠을 찾아온다
        /// </summary>
        private void SearchContentsList()
        {
            try
            {
                modelContents.Init();
                contentsDs.Contents.Clear();
                modelContents.SearchKey = ebSearchKey.Text.Trim();

                new ContentsManager( systemModel, commonModel).GetContentsListCommon( modelContents );

                if (modelContents.ResultCD.Equals("0000"))
                {
                    Utility.SetDataTable(contentsDs.Contents, modelContents.ContentsDataSet);
                }
            }
            catch(FrameException fe)
            {
                FrameSystem.showMsgForm("컨텐츠 리스트 조회오류", new string[] {fe.ErrCode, fe.ResultMsg});
            }
            catch(Exception ex)
            {
                FrameSystem.showMsgForm("컨텐츠 리스트 조회오류",new string[] {"",ex.Message});
            }
        }

        private void btnSearch_Click(object sender, System.EventArgs e)
        {
            SearchContentsList();
        }

        /// <summary>
        /// 장르선택 이벤트 처리기
        /// </summary>
        public event ContentsEventHandler   SelectContents;

        /// <summary>
        /// 장르선택 이벤트 처리함수
        /// </summary>
        private void Selected()
        {
            if( keyContentId.Length == 36 )
            {
                if( SelectContents != null )
                {
                    ContentsEventArgs args = new ContentsEventArgs();
                    args.CategoryCode   = keyCategoryCode;
                    args.CategoryName   = keyCategoryName;
                    args.GenreCode      = keyGenreCode;
                    args.GenreName      = keyGenreName;
                    args.ChannelNo      = keyChannelNo;
                    args.SeriesNo       = keySeriesNo;
                    args.Title          = keyTitle;
                    args.SubTitle       = keySubTitle;
                    args.ContentId      = keyContentId;

                    SelectContents( this, args );
                }
                this.Close();
            }
        }

        private void btnOk_Click(object sender, System.EventArgs e)
        {
            this.Selected();
        }

        private void btnClose_Click(object sender, System.EventArgs e)
        {
            this.Close();
        }
    }
}
