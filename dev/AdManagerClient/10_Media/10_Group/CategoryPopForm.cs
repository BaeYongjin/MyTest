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
	/// CategoryPopForm에 대한 요약 설명입니다.
	/// </summary>
	/// 
    
	public class CategoryPopForm : System.Windows.Forms.Form
	{

		#region 이벤트핸들러
		public event StatusEventHandler 			StatusEvent;			// 상태이벤트 핸들러

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
			
		#region 사용자정의 객체 및 변수

		// 시스템 정보 : 화면공통
		private SystemModel   systemModel   = FrameSystem.oSysModel;
		private CommonModel   commonModel   = FrameSystem.oComModel;
		private Logger        log           = FrameSystem.oLog;

		// 사용할 정보모델
		GroupModel groupModel  = new GroupModel();	// 컨텐츠정보모델
				
		GroupControl CategoryCtl = null;

		// 화면처리용 변수
		bool IsNewSearchKey		  = true;					// 검색어입력 여부
		CurrencyManager ccm        = null;					// 데이터 그리드의 변경에 따른 데이터셋 관리를 위하여			
		DataTable       dtt        = null;
		
		#endregion



		private System.Windows.Forms.Panel panel4;
		private System.Windows.Forms.Panel pnlBtn;
		private Janus.Windows.EditControls.UIButton btnClose;
		private Janus.Windows.EditControls.UIButton btnOk;
		private Janus.Windows.EditControls.UIButton btnSearch;
		private Janus.Windows.GridEX.EditControls.EditBox ebSearchKey;
		private System.Data.DataView dvCategory;
        private AdManagerClient._10_Media._10_Group.GroupDs groupDs;
		private Janus.Windows.GridEX.GridEX grdExGenreList;
		//		private System.ComponentModel.IContainer components;

		/// <summary>
		/// 데이터 넘겨야 할 넘
		/// </summary>
		/// <param name="sender"></param>
		public CategoryPopForm(GroupControl sender)
		{
			//
			// Windows Form 디자이너 지원에 필요합니다.
			//
			InitializeComponent();

			//
			
			//
            
			CategoryCtl = sender;
		}

		/// <summary>
		/// 일반사용자
		/// </summary>
		public CategoryPopForm()
		{
			//
			// Windows Form 디자이너 지원에 필요합니다.
			//
			InitializeComponent();

			//
			
			//
            
			CategoryCtl = null;
		}

		/// <summary>
		/// 사용 중인 모든 리소스를 정리합니다.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				/*
				if(components != null)
				{
					components.Dispose();
				}
				*/
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
            Janus.Windows.GridEX.GridEXLayout grdExGenreList_Layout_0 = new Janus.Windows.GridEX.GridEXLayout();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CategoryPopForm));
            this.dvCategory = new System.Data.DataView();
            this.groupDs = new AdManagerClient._10_Media._10_Group.GroupDs();
            this.panel4 = new System.Windows.Forms.Panel();
            this.ebSearchKey = new Janus.Windows.GridEX.EditControls.EditBox();
            this.btnSearch = new Janus.Windows.EditControls.UIButton();
            this.pnlBtn = new System.Windows.Forms.Panel();
            this.btnClose = new Janus.Windows.EditControls.UIButton();
            this.btnOk = new Janus.Windows.EditControls.UIButton();
            this.grdExGenreList = new Janus.Windows.GridEX.GridEX();
            ((System.ComponentModel.ISupportInitialize)(this.dvCategory)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupDs)).BeginInit();
            this.panel4.SuspendLayout();
            this.pnlBtn.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdExGenreList)).BeginInit();
            this.SuspendLayout();
            // 
            // dvCategory
            // 
            this.dvCategory.Table = this.groupDs.Category;
            // 
            // groupDs
            // 
            this.groupDs.DataSetName = "GroupDs";
            this.groupDs.Locale = new System.Globalization.CultureInfo("en-US");
            this.groupDs.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // panel4
            // 
            this.panel4.BackColor = System.Drawing.Color.White;
            this.panel4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel4.Controls.Add(this.ebSearchKey);
            this.panel4.Controls.Add(this.btnSearch);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel4.Location = new System.Drawing.Point(0, 0);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(320, 32);
            this.panel4.TabIndex = 15;
            // 
            // ebSearchKey
            // 
            this.ebSearchKey.ButtonFont = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ebSearchKey.Location = new System.Drawing.Point(3, 4);
            this.ebSearchKey.Name = "ebSearchKey";
            this.ebSearchKey.Size = new System.Drawing.Size(248, 22);
            this.ebSearchKey.TabIndex = 5;
            this.ebSearchKey.Text = "검색어를 입력하세요";
            this.ebSearchKey.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
            this.ebSearchKey.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            this.ebSearchKey.TextChanged += new System.EventHandler(this.ebSearchKey_TextChanged);
            this.ebSearchKey.Click += new System.EventHandler(this.ebSearchKey_Click);
            this.ebSearchKey.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ebSearchKey_KeyDown);
            // 
            // btnSearch
            // 
            this.btnSearch.BackColor = System.Drawing.Color.White;
            this.btnSearch.Location = new System.Drawing.Point(258, 3);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(54, 24);
            this.btnSearch.TabIndex = 4;
            this.btnSearch.Text = "조 회";
            this.btnSearch.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // pnlBtn
            // 
            this.pnlBtn.BackColor = System.Drawing.Color.WhiteSmoke;
            this.pnlBtn.Controls.Add(this.btnClose);
            this.pnlBtn.Controls.Add(this.btnOk);
            this.pnlBtn.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlBtn.Location = new System.Drawing.Point(0, 560);
            this.pnlBtn.Name = "pnlBtn";
            this.pnlBtn.Size = new System.Drawing.Size(320, 40);
            this.pnlBtn.TabIndex = 16;
            // 
            // btnClose
            // 
            this.btnClose.BackColor = System.Drawing.SystemColors.Control;
            this.btnClose.Location = new System.Drawing.Point(168, 8);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(64, 23);
            this.btnClose.TabIndex = 8;
            this.btnClose.Text = "닫기";
            this.btnClose.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnOk
            // 
            this.btnOk.BackColor = System.Drawing.SystemColors.Control;
            this.btnOk.Location = new System.Drawing.Point(88, 8);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(64, 23);
            this.btnOk.TabIndex = 9;
            this.btnOk.Text = "편성";
            this.btnOk.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // grdExGenreList
            // 
            this.grdExGenreList.AllowEdit = Janus.Windows.GridEX.InheritableBoolean.False;
            this.grdExGenreList.AlternatingColors = true;
            this.grdExGenreList.BorderStyle = Janus.Windows.GridEX.BorderStyle.None;
            this.grdExGenreList.DataSource = this.dvCategory;
            this.grdExGenreList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdExGenreList.EmptyRows = true;
            this.grdExGenreList.FocusStyle = Janus.Windows.GridEX.FocusStyle.None;
            this.grdExGenreList.Font = new System.Drawing.Font("굴림", 9F);
            this.grdExGenreList.GridLines = Janus.Windows.GridEX.GridLines.Vertical;
            this.grdExGenreList.GridLineStyle = Janus.Windows.GridEX.GridLineStyle.Solid;
            this.grdExGenreList.GroupByBoxVisible = false;
            this.grdExGenreList.HeaderFormatStyle.TextAlignment = Janus.Windows.GridEX.TextAlignment.Center;
            grdExGenreList_Layout_0.DataSource = this.dvCategory;
            grdExGenreList_Layout_0.IsCurrentLayout = true;
            grdExGenreList_Layout_0.Key = "bae";
            grdExGenreList_Layout_0.LayoutString = resources.GetString("grdExGenreList_Layout_0.LayoutString");
            this.grdExGenreList.Layouts.AddRange(new Janus.Windows.GridEX.GridEXLayout[] {
            grdExGenreList_Layout_0});
            this.grdExGenreList.Location = new System.Drawing.Point(0, 32);
            this.grdExGenreList.Name = "grdExGenreList";
            this.grdExGenreList.ScrollBars = Janus.Windows.GridEX.ScrollBars.Vertical;
            this.grdExGenreList.SelectedInactiveFormatStyle.BackColor = System.Drawing.Color.Gold;
            this.grdExGenreList.Size = new System.Drawing.Size(320, 528);
            this.grdExGenreList.TabIndex = 17;
            this.grdExGenreList.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            this.grdExGenreList.RowDoubleClick += new Janus.Windows.GridEX.RowActionEventHandler(this.grdExGenreList_RowDoubleClick);
            // 
            // CategoryPopForm
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(6, 15);
            this.ClientSize = new System.Drawing.Size(320, 600);
            this.Controls.Add(this.grdExGenreList);
            this.Controls.Add(this.pnlBtn);
            this.Controls.Add(this.panel4);
            this.Font = new System.Drawing.Font("맑은 고딕", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.MaximizeBox = false;
            this.Name = "CategoryPopForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "1차메뉴(카테고리)목록검색";
            this.Load += new System.EventHandler(this.CategoryPopForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dvCategory)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupDs)).EndInit();
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.pnlBtn.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdExGenreList)).EndInit();
            this.ResumeLayout(false);

		}
		#endregion

		#region 컨트롤 로드
		private void CategoryPopForm_Load(object sender, System.EventArgs e)
		{
            
			// 데이터관리용 객체생성
			dtt = ((DataView)grdExGenreList.DataSource).Table;  
			ccm = (CurrencyManager) this.BindingContext[grdExGenreList.DataSource]; 
			//cm.PositionChanged += new System.EventHandler(OnGrdRowChanged); 

			// 컨트롤 초기화
			InitControl();
			SearchCategory();
		}
		#endregion

		#region 사용자 액션처리 메소드

		/// <summary>
		/// 검색어 변경
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		/// 
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
		
		/// <summary>
		/// 검색어 클릭 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void ebSearchKey_TextChanged(object sender, System.EventArgs e)
		{
			IsNewSearchKey = false;
		}

		private void ebSearchKey_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
		{
			if(e.KeyCode == Keys.Enter)
			{
				SearchCategory();
			}
		}

		#endregion

		#region 컨트롤 초기화
		private void InitControl()
		{
													
		}
		
		#endregion
  
		#region 처리메소드


		/// <summary>
		/// 카테고리정보를 읽어온다
		/// </summary>
		private void SearchCategory()
		{
			StatusMessage("카테고리 정보를 조회합니다.");

			try
			{
                //검색 종류는 카테고리 검색
                groupModel.SearchType = "C"; 

				if(IsNewSearchKey)	groupModel.SearchKey = "";
				else				groupModel.SearchKey  = ebSearchKey.Text.Trim();

                //무효 메뉴 체크판단 - 카테고리는 무효 메뉴 체크 안함.
/*                if (chkInvalidMenu.Checked)
                {
                    groupModel.InvalidYn = true;
                }
                else
                {
                    groupModel.InvalidYn = false;
                }
*/
                groupModel.InvalidYn = true;
				// 컨텐츠목록조회 서비스를 호출한다.
				new GroupManager(systemModel,commonModel).GetCategoryList2(groupModel);

				if (groupModel.ResultCD.Equals("0000"))
				{
					Utility.SetDataTable(groupDs.Category, groupModel.CategoryDataSet);				
					StatusMessage(groupModel.ResultCnt + "건의 카테고리 정보가 조회되었습니다.");
				}
			}
			catch(FrameException fe)
			{
				FrameSystem.showMsgForm("카테고리조회오류", new string[] {fe.ErrCode, fe.ResultMsg});
			}
			catch(Exception ex)
			{
				FrameSystem.showMsgForm("카테고리조회오류",new string[] {"",ex.Message});
			}
		}
		#endregion
		#region 이벤트함수

		private void btnOk_Click(object sender, System.EventArgs e)
		{
			string newKey = grdExGenreList.SelectedItems[0].GetRow().Cells["CategoryCode"].Value.ToString();			
			string newKey_N = grdExGenreList.SelectedItems[0].GetRow().Cells["CategoryName"].Value.ToString();
            this.CategoryCtl.CategoryName = newKey_N;
            this.CategoryCtl.CategoryCode = newKey;
			
				
			//this.Close();
		}
     


		private void btnClose_Click(object sender, System.EventArgs e)
		{
			this.Close();
		}

		private void btnSearch_Click(object sender, System.EventArgs e)
		{
			SearchCategory();
		}

		private void grdExGenreList_RowDoubleClick(object sender, Janus.Windows.GridEX.RowActionEventArgs e)
		{						
			string newKey = grdExGenreList.SelectedItems[0].GetRow().Cells["CategoryCode"].Value.ToString();			
			string newKey_N = grdExGenreList.SelectedItems[0].GetRow().Cells["CategoryName"].Value.ToString();
            this.CategoryCtl.CategoryName = newKey_N;
            this.CategoryCtl.CategoryCode = newKey;							
			//this.Close();
		}

        private void chkInvalidMenu_CheckedChanged(object sender, EventArgs e)
        {
            SearchCategory();
        }

       
		
		
	}
   

	#endregion


}
