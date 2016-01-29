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
	/// RapPopForm에 대한 요약 설명입니다.
	/// </summary>
	/// 
    
	public class ContractPackage_RepSearchForm : System.Windows.Forms.Form
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
		MediaRapCodeModel mediarapcodeModel = new MediaRapCodeModel();			
				
		ContractPackageControl parent = null;

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
		private Janus.Windows.GridEX.GridEX grdExRapList;
		private System.Data.DataView dvMediaRep;
		private AdManagerClient.ContractPackageDs contractPackageDs;
//		private System.ComponentModel.IContainer components;

		/// <summary>
		/// 데이터 넘겨야 할 넘
		/// </summary>
		/// <param name="sender"></param>
		public ContractPackage_RepSearchForm(ContractPackageControl sender)
		{
			//
			// Windows Form 디자이너 지원에 필요합니다.
			//
			InitializeComponent();

			//
			
			//
            
			parent = sender;
		}

		/// <summary>
		/// 사용 중인 모든 리소스를 정리합니다.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
//				if(components != null)
//				{
//					components.Dispose();
//				}
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
            Janus.Windows.GridEX.GridEXLayout grdExRapList_Layout_0 = new Janus.Windows.GridEX.GridEXLayout();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ContractPackage_RepSearchForm));
            this.dvMediaRep = new System.Data.DataView();
            this.contractPackageDs = new AdManagerClient.ContractPackageDs();
            this.panel4 = new System.Windows.Forms.Panel();
            this.ebSearchKey = new Janus.Windows.GridEX.EditControls.EditBox();
            this.btnSearch = new Janus.Windows.EditControls.UIButton();
            this.pnlBtn = new System.Windows.Forms.Panel();
            this.btnClose = new Janus.Windows.EditControls.UIButton();
            this.btnOk = new Janus.Windows.EditControls.UIButton();
            this.grdExRapList = new Janus.Windows.GridEX.GridEX();
            ((System.ComponentModel.ISupportInitialize)(this.dvMediaRep)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.contractPackageDs)).BeginInit();
            this.panel4.SuspendLayout();
            this.pnlBtn.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdExRapList)).BeginInit();
            this.SuspendLayout();
            // 
            // dvMediaRep
            // 
            this.dvMediaRep.Table = this.contractPackageDs.SearchRep;
            // 
            // contractPackageDs
            // 
            this.contractPackageDs.DataSetName = "ContractPackageDs";
            this.contractPackageDs.Locale = new System.Globalization.CultureInfo("en-US");
            this.contractPackageDs.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // panel4
            // 
            this.panel4.BackColor = System.Drawing.Color.White;
            this.panel4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel4.Controls.Add(this.ebSearchKey);
            this.panel4.Controls.Add(this.btnSearch);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel4.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.panel4.Location = new System.Drawing.Point(0, 0);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(320, 32);
            this.panel4.TabIndex = 0;
            this.panel4.TabStop = true;
            // 
            // ebSearchKey
            // 
            this.ebSearchKey.ButtonFont = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ebSearchKey.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ebSearchKey.Location = new System.Drawing.Point(8, 5);
            this.ebSearchKey.Name = "ebSearchKey";
            this.ebSearchKey.Size = new System.Drawing.Size(184, 21);
            this.ebSearchKey.TabIndex = 1;
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
            this.btnSearch.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSearch.Location = new System.Drawing.Point(208, 3);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(104, 24);
            this.btnSearch.TabIndex = 2;
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
            this.pnlBtn.Location = new System.Drawing.Point(0, 382);
            this.pnlBtn.Name = "pnlBtn";
            this.pnlBtn.Size = new System.Drawing.Size(320, 40);
            this.pnlBtn.TabIndex = 16;
            // 
            // btnClose
            // 
            this.btnClose.BackColor = System.Drawing.SystemColors.Control;
            this.btnClose.ImageIndex = 1;
            this.btnClose.Location = new System.Drawing.Point(160, 8);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(96, 24);
            this.btnClose.TabIndex = 5;
            this.btnClose.Text = "닫기";
            this.btnClose.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnOk
            // 
            this.btnOk.BackColor = System.Drawing.SystemColors.Control;
            this.btnOk.ImageIndex = 0;
            this.btnOk.Location = new System.Drawing.Point(56, 8);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(96, 24);
            this.btnOk.TabIndex = 4;
            this.btnOk.Text = "확인";
            this.btnOk.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // grdExRapList
            // 
            this.grdExRapList.AllowEdit = Janus.Windows.GridEX.InheritableBoolean.False;
            this.grdExRapList.AlternatingColors = true;
            this.grdExRapList.BorderStyle = Janus.Windows.GridEX.BorderStyle.None;
            this.grdExRapList.DataSource = this.dvMediaRep;
            this.grdExRapList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdExRapList.EmptyRows = true;
            this.grdExRapList.FocusCellFormatStyle.BackColor = System.Drawing.SystemColors.Highlight;
            this.grdExRapList.FocusCellFormatStyle.ForeColor = System.Drawing.Color.White;
            this.grdExRapList.FocusStyle = Janus.Windows.GridEX.FocusStyle.None;
            this.grdExRapList.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.grdExRapList.GridLines = Janus.Windows.GridEX.GridLines.Vertical;
            this.grdExRapList.GridLineStyle = Janus.Windows.GridEX.GridLineStyle.Solid;
            this.grdExRapList.GroupByBoxVisible = false;
            this.grdExRapList.HeaderFormatStyle.TextAlignment = Janus.Windows.GridEX.TextAlignment.Center;
            grdExRapList_Layout_0.DataSource = this.dvMediaRep;
            grdExRapList_Layout_0.IsCurrentLayout = true;
            grdExRapList_Layout_0.Key = "bae";
            grdExRapList_Layout_0.LayoutString = resources.GetString("grdExRapList_Layout_0.LayoutString");
            this.grdExRapList.Layouts.AddRange(new Janus.Windows.GridEX.GridEXLayout[] {
            grdExRapList_Layout_0});
            this.grdExRapList.Location = new System.Drawing.Point(0, 32);
            this.grdExRapList.Name = "grdExRapList";
            this.grdExRapList.ScrollBars = Janus.Windows.GridEX.ScrollBars.Vertical;
            this.grdExRapList.SelectedInactiveFormatStyle.BackColor = System.Drawing.Color.Gold;
            this.grdExRapList.Size = new System.Drawing.Size(320, 350);
            this.grdExRapList.TabIndex = 3;
            this.grdExRapList.TabKeyBehavior = Janus.Windows.GridEX.TabKeyBehavior.ControlNavigation;
            this.grdExRapList.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            this.grdExRapList.RowDoubleClick += new Janus.Windows.GridEX.RowActionEventHandler(this.grdExRapList_RowDoubleClick);
            // 
            // ContractPackage_RepSearchForm
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(6, 14);
            this.ClientSize = new System.Drawing.Size(320, 422);
            this.Controls.Add(this.grdExRapList);
            this.Controls.Add(this.pnlBtn);
            this.Controls.Add(this.panel4);
            this.Name = "ContractPackage_RepSearchForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "미디어렙검색";
            this.Load += new System.EventHandler(this.RapPopForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dvMediaRep)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.contractPackageDs)).EndInit();
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.pnlBtn.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdExRapList)).EndInit();
            this.ResumeLayout(false);

		}
		#endregion

		#region 컨트롤 로드
		private void RapPopForm_Load(object sender, System.EventArgs e)
		{
            
			// 데이터관리용 객체생성
			dtt = ((DataView)grdExRapList.DataSource).Table;  
			ccm = (CurrencyManager) this.BindingContext[grdExRapList.DataSource]; 
			
			// 컨트롤 초기화
			InitControl();
			SearchRap();
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
				SearchRap();
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
		/// 컨텐츠목록 조회
		/// </summary>
		private void SearchRap()
		{
			try
			{
				// 데이터모델에 전송할 내용을 셋트한다.
				if(IsNewSearchKey)
				{
					mediarapcodeModel.SearchKey = "";
				}
				else
				{
					mediarapcodeModel.SearchKey  = ebSearchKey.Text;
				}

				// 컨텐츠목록조회 서비스를 호출한다.
				new MediaRapCodeManager(systemModel, commonModel).GetMediaRapCodeList(mediarapcodeModel);

				if (mediarapcodeModel.ResultCD.Equals("0000"))
				{
					Utility.SetDataTable(contractPackageDs.SearchRep, mediarapcodeModel.MediaRapCodeDataSet);				
					StatusMessage(mediarapcodeModel.ResultCnt + "건의 미디어렙이 조회되었습니다.");
				}
			}
			catch(FrameException fe)
			{
				FrameSystem.showMsgForm("미디어렙 조회오류", new string[] {fe.ErrCode, fe.ResultMsg});
			}
			catch(Exception ex)
			{
				FrameSystem.showMsgForm("미디어렙 조회오류",new string[] {"",ex.Message});
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
			SetRep();	
			this.Close();
		}
     


		private void btnClose_Click(object sender, System.EventArgs e)
		{
			this.Close();
		}

		private void btnSearch_Click(object sender, System.EventArgs e)
		{
			SearchRap();
		}

		private void grdExRapList_RowDoubleClick(object sender, Janus.Windows.GridEX.RowActionEventArgs e)
		{		
			SetRep();	
			this.Close();
		}

		private void SetRep()
		{
			string RepCode = grdExRapList.SelectedItems[0].GetRow().Cells["RapCode"].Value.ToString();			
			string RepName = grdExRapList.SelectedItems[0].GetRow().Cells["RapName"].Value.ToString();			
			
			this.parent.SetMediaRep(RepCode, RepName);	

		}
		
	}
   

	#endregion


}
