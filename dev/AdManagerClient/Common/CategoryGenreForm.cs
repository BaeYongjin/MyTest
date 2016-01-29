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
	/// CategoryGenreForm에 대한 요약 설명입니다.
	/// </summary>
	public class CategoryGenreForm : System.Windows.Forms.Form
	{
        #region 사용자정의 객체 및 변수

        // 시스템 정보 : 화면공통
        private SystemModel   systemModel   = FrameSystem.oSysModel;
        private CommonModel   commonModel   = FrameSystem.oComModel;
        private Logger        log           = FrameSystem.oLog;

        // 사용할 정보모델
        CategoryModel   modelCategory   = new CategoryModel();
        GenreModel      modelGenre      = new GenreModel();
        
        CurrencyManager cmCategory      = null;
        CurrencyManager cmGenre         = null;
        DataTable       dtCategory      = null;
        DataTable       dtGenre         = null;
        #endregion

        private Janus.Windows.UI.Dock.UIPanelManager uiPM;
        private Janus.Windows.UI.Dock.UIPanelGroup PanelMain;
        private Janus.Windows.UI.Dock.UIPanel PanelCommand;
        private Janus.Windows.UI.Dock.UIPanelInnerContainer PanelCommandContainer;
        private Janus.Windows.UI.Dock.UIPanel PanelCategory;
        private Janus.Windows.UI.Dock.UIPanelInnerContainer PanelCategoryContainer;
        private Janus.Windows.UI.Dock.UIPanel PanelGenre;
        private Janus.Windows.UI.Dock.UIPanelInnerContainer PanelGenreContainer;
        private Janus.Windows.GridEX.GridEX gridEXCategory;
        private Janus.Windows.GridEX.GridEX gridEXGenre;
        private AdManagerClient.Common.CategoryGenre categoryGenreDs;
        private System.Data.DataView dvCategory;
        private System.Data.DataView dvGenre;
        private Janus.Windows.EditControls.UIButton btnClose;
        private Janus.Windows.EditControls.UIButton btnApply;
        private System.ComponentModel.IContainer components;

		public CategoryGenreForm()
		{
			//
			// Windows Form 디자이너 지원에 필요합니다.
			//
			InitializeComponent();
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
            Janus.Windows.GridEX.GridEXLayout gridEXCategory_DesignTimeLayout = new Janus.Windows.GridEX.GridEXLayout();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CategoryGenreForm));
            Janus.Windows.GridEX.GridEXLayout gridEXGenre_DesignTimeLayout = new Janus.Windows.GridEX.GridEXLayout();
            this.uiPM = new Janus.Windows.UI.Dock.UIPanelManager(this.components);
            this.PanelCommand = new Janus.Windows.UI.Dock.UIPanel();
            this.PanelCommandContainer = new Janus.Windows.UI.Dock.UIPanelInnerContainer();
            this.btnApply = new Janus.Windows.EditControls.UIButton();
            this.btnClose = new Janus.Windows.EditControls.UIButton();
            this.PanelMain = new Janus.Windows.UI.Dock.UIPanelGroup();
            this.PanelCategory = new Janus.Windows.UI.Dock.UIPanel();
            this.PanelCategoryContainer = new Janus.Windows.UI.Dock.UIPanelInnerContainer();
            this.gridEXCategory = new Janus.Windows.GridEX.GridEX();
            this.dvCategory = new System.Data.DataView();
            this.categoryGenreDs = new AdManagerClient.Common.CategoryGenre();
            this.PanelGenre = new Janus.Windows.UI.Dock.UIPanel();
            this.PanelGenreContainer = new Janus.Windows.UI.Dock.UIPanelInnerContainer();
            this.gridEXGenre = new Janus.Windows.GridEX.GridEX();
            this.dvGenre = new System.Data.DataView();
            ((System.ComponentModel.ISupportInitialize)(this.uiPM)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PanelCommand)).BeginInit();
            this.PanelCommand.SuspendLayout();
            this.PanelCommandContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PanelMain)).BeginInit();
            this.PanelMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PanelCategory)).BeginInit();
            this.PanelCategory.SuspendLayout();
            this.PanelCategoryContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridEXCategory)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dvCategory)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.categoryGenreDs)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PanelGenre)).BeginInit();
            this.PanelGenre.SuspendLayout();
            this.PanelGenreContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridEXGenre)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dvGenre)).BeginInit();
            this.SuspendLayout();
            // 
            // uiPM
            // 
            this.uiPM.BackColorGradientAutoHideStrip = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(218)))), ((int)(((byte)(250)))));
            this.uiPM.ContainerControl = this;
            this.uiPM.SplitterSize = 2;
            this.PanelCommand.Id = new System.Guid("b6f9b98b-0401-477c-83c0-70685a08d029");
            this.uiPM.Panels.Add(this.PanelCommand);
            this.PanelMain.Id = new System.Guid("156e8d59-99b2-4b24-a72c-a5da41097b68");
            this.PanelMain.StaticGroup = true;
            this.PanelCategory.Id = new System.Guid("4385a8e6-84db-47fc-9353-607e7b07fae2");
            this.PanelMain.Panels.Add(this.PanelCategory);
            this.PanelGenre.Id = new System.Guid("223da5bd-4c2f-4cde-8c1b-6b8c3f1dffe8");
            this.PanelMain.Panels.Add(this.PanelGenre);
            this.uiPM.Panels.Add(this.PanelMain);
            // 
            // Design Time Panel Info:
            // 
            this.uiPM.BeginPanelInfo();
            this.uiPM.AddDockPanelInfo(new System.Guid("156e8d59-99b2-4b24-a72c-a5da41097b68"), Janus.Windows.UI.Dock.PanelGroupStyle.VerticalTiles, Janus.Windows.UI.Dock.PanelDockStyle.Fill, true, new System.Drawing.Size(734, 433), true);
            this.uiPM.AddDockPanelInfo(new System.Guid("4385a8e6-84db-47fc-9353-607e7b07fae2"), new System.Guid("156e8d59-99b2-4b24-a72c-a5da41097b68"), 203, true);
            this.uiPM.AddDockPanelInfo(new System.Guid("223da5bd-4c2f-4cde-8c1b-6b8c3f1dffe8"), new System.Guid("156e8d59-99b2-4b24-a72c-a5da41097b68"), 203, true);
            this.uiPM.AddDockPanelInfo(new System.Guid("b6f9b98b-0401-477c-83c0-70685a08d029"), Janus.Windows.UI.Dock.PanelDockStyle.Bottom, new System.Drawing.Size(734, 48), true);
            this.uiPM.AddFloatingPanelInfo(new System.Guid("b6f9b98b-0401-477c-83c0-70685a08d029"), new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
            this.uiPM.AddFloatingPanelInfo(new System.Guid("156e8d59-99b2-4b24-a72c-a5da41097b68"), Janus.Windows.UI.Dock.PanelGroupStyle.VerticalTiles, true, new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
            this.uiPM.AddFloatingPanelInfo(new System.Guid("4385a8e6-84db-47fc-9353-607e7b07fae2"), new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
            this.uiPM.AddFloatingPanelInfo(new System.Guid("223da5bd-4c2f-4cde-8c1b-6b8c3f1dffe8"), new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
            this.uiPM.EndPanelInfo();
            // 
            // PanelCommand
            // 
            this.PanelCommand.CaptionVisible = Janus.Windows.UI.InheritableBoolean.False;
            this.PanelCommand.InnerAreaStyle = Janus.Windows.UI.Dock.PanelInnerAreaStyle.UseFormatStyle;
            this.PanelCommand.InnerContainer = this.PanelCommandContainer;
            this.PanelCommand.Location = new System.Drawing.Point(3, 436);
            this.PanelCommand.Name = "PanelCommand";
            this.PanelCommand.Size = new System.Drawing.Size(734, 48);
            this.PanelCommand.TabIndex = 4;
            this.PanelCommand.Text = "Panel 1";
            // 
            // PanelCommandContainer
            // 
            this.PanelCommandContainer.BackColor = System.Drawing.Color.WhiteSmoke;
            this.PanelCommandContainer.Controls.Add(this.btnApply);
            this.PanelCommandContainer.Controls.Add(this.btnClose);
            this.PanelCommandContainer.Location = new System.Drawing.Point(1, 3);
            this.PanelCommandContainer.Name = "PanelCommandContainer";
            this.PanelCommandContainer.Size = new System.Drawing.Size(732, 44);
            this.PanelCommandContainer.TabIndex = 0;
            // 
            // btnApply
            // 
            this.btnApply.DialogResult = System.Windows.Forms.DialogResult.Yes;
            this.btnApply.Location = new System.Drawing.Point(292, 8);
            this.btnApply.Name = "btnApply";
            this.btnApply.Size = new System.Drawing.Size(70, 24);
            this.btnApply.TabIndex = 5;
            this.btnApply.Text = "선 택";
            this.btnApply.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.btnApply.Click += new System.EventHandler(this.btnApply_Click);
            // 
            // btnClose
            // 
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.No;
            this.btnClose.Location = new System.Drawing.Point(370, 9);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(70, 24);
            this.btnClose.TabIndex = 4;
            this.btnClose.Text = "닫 기";
            this.btnClose.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // PanelMain
            // 
            this.PanelMain.AllowResize = Janus.Windows.UI.InheritableBoolean.True;
            this.PanelMain.CaptionVisible = Janus.Windows.UI.InheritableBoolean.False;
            this.PanelMain.CloseButtonVisible = Janus.Windows.UI.InheritableBoolean.False;
            this.PanelMain.GroupStyle = Janus.Windows.UI.Dock.PanelGroupStyle.VerticalTiles;
            this.PanelMain.Location = new System.Drawing.Point(3, 3);
            this.PanelMain.Name = "PanelMain";
            this.PanelMain.Size = new System.Drawing.Size(734, 433);
            this.PanelMain.TabIndex = 4;
            this.PanelMain.Text = "Panel 0";
            // 
            // PanelCategory
            // 
            this.PanelCategory.CaptionFormatStyle.FontBold = Janus.Windows.UI.TriState.True;
            this.PanelCategory.CaptionStyle = Janus.Windows.UI.Dock.PanelCaptionStyle.Light;
            this.PanelCategory.InnerContainer = this.PanelCategoryContainer;
            this.PanelCategory.Location = new System.Drawing.Point(0, 0);
            this.PanelCategory.Name = "PanelCategory";
            this.PanelCategory.Size = new System.Drawing.Size(366, 433);
            this.PanelCategory.TabIndex = 4;
            this.PanelCategory.Text = "카테고리(Category)";
            // 
            // PanelCategoryContainer
            // 
            this.PanelCategoryContainer.Controls.Add(this.gridEXCategory);
            this.PanelCategoryContainer.Location = new System.Drawing.Point(1, 23);
            this.PanelCategoryContainer.Name = "PanelCategoryContainer";
            this.PanelCategoryContainer.Size = new System.Drawing.Size(364, 409);
            this.PanelCategoryContainer.TabIndex = 0;
            // 
            // gridEXCategory
            // 
            this.gridEXCategory.AlternatingColors = true;
            this.gridEXCategory.BorderStyle = Janus.Windows.GridEX.BorderStyle.None;
            this.gridEXCategory.DataSource = this.dvCategory;
            gridEXCategory_DesignTimeLayout.LayoutString = resources.GetString("gridEXCategory_DesignTimeLayout.LayoutString");
            this.gridEXCategory.DesignTimeLayout = gridEXCategory_DesignTimeLayout;
            this.gridEXCategory.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridEXCategory.FocusCellFormatStyle.BackColor = System.Drawing.SystemColors.Highlight;
            this.gridEXCategory.FocusCellFormatStyle.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.gridEXCategory.FocusStyle = Janus.Windows.GridEX.FocusStyle.None;
            this.gridEXCategory.GroupByBoxVisible = false;
            this.gridEXCategory.HideSelection = Janus.Windows.GridEX.HideSelection.Highlight;
            this.gridEXCategory.Location = new System.Drawing.Point(0, 0);
            this.gridEXCategory.Name = "gridEXCategory";
            this.gridEXCategory.ScrollBars = Janus.Windows.GridEX.ScrollBars.Vertical;
            this.gridEXCategory.Size = new System.Drawing.Size(364, 409);
            this.gridEXCategory.TabIndex = 0;
            this.gridEXCategory.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            this.gridEXCategory.Enter += new System.EventHandler(this.cmCategory_PositionChanged);
            // 
            // dvCategory
            // 
            this.dvCategory.AllowDelete = false;
            this.dvCategory.AllowEdit = false;
            this.dvCategory.AllowNew = false;
            this.dvCategory.Table = this.categoryGenreDs.Category;
            // 
            // categoryGenreDs
            // 
            this.categoryGenreDs.DataSetName = "CategoryGenre";
            this.categoryGenreDs.Locale = new System.Globalization.CultureInfo("en-US");
            this.categoryGenreDs.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // PanelGenre
            // 
            this.PanelGenre.CaptionFormatStyle.FontBold = Janus.Windows.UI.TriState.True;
            this.PanelGenre.CaptionStyle = Janus.Windows.UI.Dock.PanelCaptionStyle.Light;
            this.PanelGenre.InnerContainer = this.PanelGenreContainer;
            this.PanelGenre.Location = new System.Drawing.Point(368, 0);
            this.PanelGenre.Name = "PanelGenre";
            this.PanelGenre.Size = new System.Drawing.Size(366, 433);
            this.PanelGenre.TabIndex = 4;
            this.PanelGenre.Text = "장르(Genre)";
            // 
            // PanelGenreContainer
            // 
            this.PanelGenreContainer.Controls.Add(this.gridEXGenre);
            this.PanelGenreContainer.Location = new System.Drawing.Point(1, 23);
            this.PanelGenreContainer.Name = "PanelGenreContainer";
            this.PanelGenreContainer.Size = new System.Drawing.Size(364, 409);
            this.PanelGenreContainer.TabIndex = 0;
            // 
            // gridEXGenre
            // 
            this.gridEXGenre.AlternatingColors = true;
            this.gridEXGenre.BorderStyle = Janus.Windows.GridEX.BorderStyle.None;
            this.gridEXGenre.DataSource = this.dvGenre;
            gridEXGenre_DesignTimeLayout.LayoutString = resources.GetString("gridEXGenre_DesignTimeLayout.LayoutString");
            this.gridEXGenre.DesignTimeLayout = gridEXGenre_DesignTimeLayout;
            this.gridEXGenre.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridEXGenre.FocusCellFormatStyle.BackColor = System.Drawing.SystemColors.Highlight;
            this.gridEXGenre.FocusCellFormatStyle.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.gridEXGenre.FocusStyle = Janus.Windows.GridEX.FocusStyle.None;
            this.gridEXGenre.GroupByBoxVisible = false;
            this.gridEXGenre.HideSelection = Janus.Windows.GridEX.HideSelection.Highlight;
            this.gridEXGenre.Location = new System.Drawing.Point(0, 0);
            this.gridEXGenre.Name = "gridEXGenre";
            this.gridEXGenre.ScrollBars = Janus.Windows.GridEX.ScrollBars.Vertical;
            this.gridEXGenre.Size = new System.Drawing.Size(364, 409);
            this.gridEXGenre.TabIndex = 0;
            this.gridEXGenre.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            this.gridEXGenre.DoubleClick += new System.EventHandler(this.gridEXGenre_DoubleClick);
            // 
            // dvGenre
            // 
            this.dvGenre.AllowDelete = false;
            this.dvGenre.AllowEdit = false;
            this.dvGenre.AllowNew = false;
            this.dvGenre.Table = this.categoryGenreDs.Genre;
            // 
            // CategoryGenreForm
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(6, 15);
            this.ClientSize = new System.Drawing.Size(740, 487);
            this.Controls.Add(this.PanelMain);
            this.Controls.Add(this.PanelCommand);
            this.Font = new System.Drawing.Font("맑은 고딕", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "CategoryGenreForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "카테고리/장르 찾기";
            this.Load += new System.EventHandler(this.CategoryGenreForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.uiPM)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PanelCommand)).EndInit();
            this.PanelCommand.ResumeLayout(false);
            this.PanelCommandContainer.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.PanelMain)).EndInit();
            this.PanelMain.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.PanelCategory)).EndInit();
            this.PanelCategory.ResumeLayout(false);
            this.PanelCategoryContainer.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridEXCategory)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dvCategory)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.categoryGenreDs)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PanelGenre)).EndInit();
            this.PanelGenre.ResumeLayout(false);
            this.PanelGenreContainer.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridEXGenre)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dvGenre)).EndInit();
            this.ResumeLayout(false);

        }
		#endregion

        private void CategoryGenreForm_Load(object sender, System.EventArgs e)
        {
            dtCategory  = ((DataView)gridEXCategory.DataSource).Table;
            dtGenre     = ((DataView)gridEXGenre.DataSource).Table;

            cmCategory  = (CurrencyManager)this.BindingContext[ gridEXCategory.DataSource];
            cmGenre     = (CurrencyManager)this.BindingContext[ gridEXGenre.DataSource];

            cmCategory.PositionChanged += new EventHandler(cmCategory_PositionChanged);
            cmGenre.PositionChanged    += new EventHandler(cmGenre_PositionChanged);

            SearchCategoryList();
        }

        /// <summary>
        /// 카테고리정보 조회
        /// </summary>
        private void SearchCategoryList()
        {
            try
            {
                modelCategory.Init();
                modelGenre.Init();
                categoryGenreDs.Category.Clear();
                categoryGenreDs.Genre.Clear();

                new CategoryGenreManager( systemModel, commonModel).GetCategoryList( modelCategory );

                if (modelCategory.ResultCD.Equals("0000"))
                {
                    Utility.SetDataTable(categoryGenreDs.Category, modelCategory.UserDataSet);
                }

            }
            catch(FrameException fe)
            {
                FrameSystem.showMsgForm("카테고리 리스트 조회오류", new string[] {fe.ErrCode, fe.ResultMsg});
            }
            catch(Exception ex)
            {
                FrameSystem.showMsgForm("카테고리 리스트 조회오류",new string[] {"",ex.Message});
            }
        }


        /// <summary>
        /// 장르정보 조회
        /// </summary>
        /// <param name="category"></param>
        private void SearchGenreList(int category )
        {
            try
            {
                modelGenre.Init();
                categoryGenreDs.Genre.Clear();

                modelGenre.CategoryCode = category.ToString();
                new CategoryGenreManager( systemModel, commonModel).GetGenreList( modelGenre );

                if (modelGenre.ResultCD.Equals("0000"))
                {
                    Utility.SetDataTable(categoryGenreDs.Genre, modelGenre.UserDataSet);
                }

            }
            catch(FrameException fe)
            {
                FrameSystem.showMsgForm("장르 리스트 조회오류", new string[] {fe.ErrCode, fe.ResultMsg});
            }
            catch(Exception ex)
            {
                FrameSystem.showMsgForm("장르 리스트 조회오류",new string[] {"",ex.Message});
            }
        }


        private int keyCategory = 0;
        private string keyCategoryName = "";
        private void cmCategory_PositionChanged(object sender, EventArgs e)
        {
            int currRow = cmCategory.Position;

            if( currRow > -1 )
            {
                keyCategory = Convert.ToInt32(dtCategory.Rows[currRow]["Category"].ToString());
                keyCategoryName = dtCategory.Rows[currRow]["CategoryName"].ToString();
                SearchGenreList( keyCategory);
            }
        }

        private int keyGenre = 0;
        private string keyGenreName = "";
        private void cmGenre_PositionChanged(object sender, EventArgs e)
        {
            int currRow = cmGenre.Position;

            if( currRow > -1 )
            {
                keyGenre        = Convert.ToInt32(dtGenre.Rows[currRow]["Genre"].ToString());
                keyGenreName    = dtGenre.Rows[currRow]["GenreName"].ToString();
            }
        }

        /// <summary>
        /// 장르선택 이벤트 처리기
        /// </summary>
        public event CategoryGenreEventHandler  SelectCategoryGenre;

        /// <summary>
        /// 장르선택 이벤트 처리함수
        /// </summary>
        private void Selected()
        {
            if( keyCategory > 0 && keyGenre > 0 )
            {
                CategoryGenreEventArgs args = new CategoryGenreEventArgs(keyCategory, keyGenre, keyCategoryName, keyGenreName);
                if( SelectCategoryGenre != null ) SelectCategoryGenre( this, args );
                this.Close();
            }
        }

        /// <summary>
        /// 선택버튼 클릭시
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnApply_Click(object sender, System.EventArgs e)
        {
            this.Selected();
        }

        /// <summary>
        /// 장르 스프레드 더블클릭으로 선택시
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridEXGenre_DoubleClick(object sender, System.EventArgs e)
        {
            this.btnApply_Click(this,null);
        }

        /// <summary>
        /// 취소버튼 클릭시
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClose_Click(object sender, System.EventArgs e)
        {
            this.Close();
        }
    }
}
