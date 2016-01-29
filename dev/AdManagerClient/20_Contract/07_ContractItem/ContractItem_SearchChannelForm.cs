/*
 * -------------------------------------------------------
 * Class Name: ContractItem_SearchChannelForm
 * 주요기능  : 시리즈 검색 및 시리즈의 다중 선택 기능
 * 작성자    : bae 
 * 작성일    : 2010.06.07
 * 특이사항  : 
 * -------------------------------------------------------
 * [수정사항]
 * -------------------------------------------------------
 * 수정코드  : [E_01]
 * 수정자    : bae
 * 수정일    : 2010.06.07
 * 수정내용  :           
 *            - 시리즈을 다중 선택가능하도록 한다.
 *            - 선택한 시리즈이 5개 초과 하지 않도록 한다.
 * --------------------------------------------------------
 * 
 */


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
	public class ContractItem_SearchChannelForm : System.Windows.Forms.Form
	{
		// 기 등록 된 시리즈 수
		private int existCount = 0;

		// 체크 된 시리즈 수
		private DataTable dtList = null;

		/// <summary>
		/// 이미 연결되어 있는 시리즈 수(E_01)
		/// </summary>
		public int SetExistCount
		{
			set{this.existCount = value;}			
		}

		
		/// <summary>
		/// 선택 된 시리즈 list(E_01)
		/// </summary>
		public DataTable GetChannelList
		{
			get{ return dtList;}
		}

		

		#region 사용자정의 객체 및 변수

		public string keyMediaCode = "";
		public string popNum = "";
		// 오프너
		private ContractItemControl Opener = null;

		private System.Windows.Forms.Panel pnlSearch;
		private Janus.Windows.GridEX.EditControls.EditBox ebSearchKey;
		private System.Windows.Forms.Panel pnlBtn;
		private Janus.Windows.EditControls.UIButton btnClose;
		private Janus.Windows.EditControls.UIButton btnOk;
		private System.Windows.Forms.Panel panel1;
		//        private System.ComponentModel.IContainer components;
		private Janus.Windows.EditControls.UIButton btnSearch;
		private System.Data.DataView dvChannel;
		private AdManagerClient.ContractItemDs contractItemDs;
		private Janus.Windows.GridEX.GridEX grdExChannelList;			// 팝업호출시 매체셋트


		#endregion

		#region 생성자 및 소멸자
		public ContractItem_SearchChannelForm()
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
		public ContractItem_SearchChannelForm(ContractItemControl sender)
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
		ContractItemModel contractItemModel  = new ContractItemModel();	// 광고내역모델
		
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
            Janus.Windows.GridEX.GridEXLayout grdExChannelList_Layout_0 = new Janus.Windows.GridEX.GridEXLayout();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ContractItem_SearchChannelForm));
            this.dvChannel = new System.Data.DataView();
            this.contractItemDs = new AdManagerClient.ContractItemDs();
            this.pnlSearch = new System.Windows.Forms.Panel();
            this.btnSearch = new Janus.Windows.EditControls.UIButton();
            this.ebSearchKey = new Janus.Windows.GridEX.EditControls.EditBox();
            this.pnlBtn = new System.Windows.Forms.Panel();
            this.btnClose = new Janus.Windows.EditControls.UIButton();
            this.btnOk = new Janus.Windows.EditControls.UIButton();
            this.panel1 = new System.Windows.Forms.Panel();
            this.grdExChannelList = new Janus.Windows.GridEX.GridEX();
            ((System.ComponentModel.ISupportInitialize)(this.dvChannel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.contractItemDs)).BeginInit();
            this.pnlSearch.SuspendLayout();
            this.pnlBtn.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdExChannelList)).BeginInit();
            this.SuspendLayout();
            // 
            // dvChannel
            // 
            this.dvChannel.Table = this.contractItemDs.ChannelSearch;
            // 
            // contractItemDs
            // 
            this.contractItemDs.DataSetName = "ContractItemDs";
            this.contractItemDs.Locale = new System.Globalization.CultureInfo("en-US");
            this.contractItemDs.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // pnlSearch
            // 
            this.pnlSearch.BackColor = System.Drawing.SystemColors.Window;
            this.pnlSearch.Controls.Add(this.btnSearch);
            this.pnlSearch.Controls.Add(this.ebSearchKey);
            this.pnlSearch.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlSearch.Location = new System.Drawing.Point(0, 0);
            this.pnlSearch.Name = "pnlSearch";
            this.pnlSearch.Size = new System.Drawing.Size(704, 40);
            this.pnlSearch.TabIndex = 4;
            // 
            // btnSearch
            // 
            this.btnSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSearch.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnSearch.Location = new System.Drawing.Point(592, 8);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(104, 24);
            this.btnSearch.TabIndex = 2;
            this.btnSearch.Text = "조 회";
            this.btnSearch.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // ebSearchKey
            // 
            this.ebSearchKey.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.ebSearchKey.Location = new System.Drawing.Point(8, 8);
            this.ebSearchKey.Name = "ebSearchKey";
            this.ebSearchKey.Size = new System.Drawing.Size(168, 23);
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
            this.pnlBtn.Location = new System.Drawing.Point(0, 427);
            this.pnlBtn.Name = "pnlBtn";
            this.pnlBtn.Size = new System.Drawing.Size(704, 42);
            this.pnlBtn.TabIndex = 17;
            // 
            // btnClose
            // 
            this.btnClose.BackColor = System.Drawing.SystemColors.Control;
            this.btnClose.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnClose.Location = new System.Drawing.Point(356, 8);
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
            this.btnOk.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnOk.Location = new System.Drawing.Point(244, 8);
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
            this.panel1.Controls.Add(this.grdExChannelList);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 40);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(704, 387);
            this.panel1.TabIndex = 18;
            // 
            // grdExChannelList
            // 
            this.grdExChannelList.AlternatingColors = true;
            this.grdExChannelList.BorderStyle = Janus.Windows.GridEX.BorderStyle.Flat;
            this.grdExChannelList.DataSource = this.dvChannel;
            this.grdExChannelList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdExChannelList.EmptyRows = true;
            this.grdExChannelList.FocusCellFormatStyle.BackColor = System.Drawing.SystemColors.Highlight;
            this.grdExChannelList.FocusCellFormatStyle.ForeColor = System.Drawing.Color.White;
            this.grdExChannelList.FocusStyle = Janus.Windows.GridEX.FocusStyle.None;
            this.grdExChannelList.Font = new System.Drawing.Font("굴림", 9F);
            this.grdExChannelList.GridLines = Janus.Windows.GridEX.GridLines.Vertical;
            this.grdExChannelList.GridLineStyle = Janus.Windows.GridEX.GridLineStyle.Solid;
            this.grdExChannelList.GroupByBoxVisible = false;
            this.grdExChannelList.GroupMode = Janus.Windows.GridEX.GroupMode.Collapsed;
            this.grdExChannelList.HeaderFormatStyle.TextAlignment = Janus.Windows.GridEX.TextAlignment.Center;
            grdExChannelList_Layout_0.DataSource = this.dvChannel;
            grdExChannelList_Layout_0.IsCurrentLayout = true;
            grdExChannelList_Layout_0.Key = "bae";
            grdExChannelList_Layout_0.LayoutString = resources.GetString("grdExChannelList_Layout_0.LayoutString");
            this.grdExChannelList.Layouts.AddRange(new Janus.Windows.GridEX.GridEXLayout[] {
            grdExChannelList_Layout_0});
            this.grdExChannelList.Location = new System.Drawing.Point(0, 0);
            this.grdExChannelList.Name = "grdExChannelList";
            this.grdExChannelList.ScrollBars = Janus.Windows.GridEX.ScrollBars.Vertical;
            this.grdExChannelList.Size = new System.Drawing.Size(704, 387);
            this.grdExChannelList.TabIndex = 3;
            this.grdExChannelList.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            // 
            // ContractItem_SearchChannelForm
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(6, 14);
            this.ClientSize = new System.Drawing.Size(704, 469);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.pnlBtn);
            this.Controls.Add(this.pnlSearch);
            this.Font = global::AdManagerClient.Properties.Settings.Default.Font1;
            this.Name = "ContractItem_SearchChannelForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "시리즈검색";
            this.Load += new System.EventHandler(this.UserControl_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dvChannel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.contractItemDs)).EndInit();
            this.pnlSearch.ResumeLayout(false);
            this.pnlSearch.PerformLayout();
            this.pnlBtn.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdExChannelList)).EndInit();
            this.ResumeLayout(false);

        }
		#endregion

		#region 컨트롤 로드
		private void UserControl_Load(object sender, System.EventArgs e)
		{
			// 데이터관리용 객체생성
			dt = ((DataView)grdExChannelList.DataSource).Table;  
			cm = (CurrencyManager) this.BindingContext[grdExChannelList.DataSource]; 
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
			if(grdExChannelList.RowCount > 0)
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
			SearchChannel();
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
				SearchChannel();
			}
		}

		private void btnOk_Click(object sender, System.EventArgs e)
		{
			//SelectChannel();
			//this.Close();

			// 신규(E_01)			
			try
			{
				this.setChannelList();
				this.DialogResult = DialogResult.Yes;
				this.Close();
			}
			catch(Exception ex)
			{
				MessageBox.Show(ex.Message, "시리즈선택",MessageBoxButtons.OK,MessageBoxIcon.Information);
			}
		}

		private void btnClose_Click(object sender, System.EventArgs e)
		{
			//this.Close();

			// 신규(E_01)
			this.DialogResult = DialogResult.No;
			this.Close();
		}

		private void grdExChannelList_DoubleClick(object sender, System.EventArgs e)
		{
			//SelectChannel();
			//this.Close();		

			// 신규(E_01)			
			try
			{
				this.setChannelList();
				this.DialogResult = DialogResult.Yes;
				this.Close();
			}
			catch(Exception ex)
			{
				MessageBox.Show(ex.Message, "시리즈선택",MessageBoxButtons.OK,MessageBoxIcon.Information);
			}			
		}

		#endregion

		#region 처리메소드

		/// <summary>
		/// 시리즈검색
		/// </summary>
		private void SearchChannel()
		{
			StatusMessage("시리즈 정보를 검색합니다.");

			if(IsNewSearchKey || ebSearchKey.Text.Trim().Length == 0) 
			{
				MessageBox.Show("검색어를 입력해 주시기 바랍니다.","시리즈검색", 
					MessageBoxButtons.OK, MessageBoxIcon.Information );
				return;
			}

			try
			{
				//검색 전에 모델을 초기화 해준다.
				contractItemModel.Init();
				// 데이터모델에 전송할 내용을 셋트한다.
				contractItemModel.SearchMediaCode      = keyMediaCode;
    
				if(IsNewSearchKey)
				{
					contractItemModel.SearchKey = "";
				}
				else
				{
					contractItemModel.SearchKey  = ebSearchKey.Text;
				}

				// 시리즈검색 서비스를 호출한다.
				new ContractItemManager(systemModel,commonModel).GetChannelList(contractItemModel);

				if (contractItemModel.ResultCD.Equals("0000"))
				{
					Utility.SetDataTable(contractItemDs.ChannelSearch, contractItemModel.ContractItemDataSet);				
					StatusMessage(contractItemModel.ResultCnt + "건의 시리즈 정보가 조회되었습니다.");
					if(Convert.ToInt32(contractItemModel.ResultCnt) > 0) cm.Position = 0;
					grdExChannelList.EnsureVisible();

				}
			}
			catch(FrameException fe)
			{
				FrameSystem.showMsgForm("시리즈검색 오류", new string[] {fe.ErrCode, fe.ResultMsg});
			}
			catch(Exception ex)
			{
				FrameSystem.showMsgForm("시리즈검색 오류",new string[] {"",ex.Message});
			}
		}


		/// <summary>
		/// 광고를 선택
		/// </summary>
		private void SelectChannel()
		{

			int curRow = cm.Position;
			if(curRow < 0) return;	// 데이터가 없으면 실행하지 않는다.

			//string GenreCode      = dt.Rows[curRow]["GenreCode"].ToString();			
			//string GenreName      = dt.Rows[curRow]["GenreName"].ToString();			
			string ChannelNo      = dt.Rows[curRow]["ChannelNo"].ToString();			
			string ChannelName    = dt.Rows[curRow]["ChannelName"].ToString();			
			string PopNum		  = popNum;			
	
			Opener.SetChannel(ChannelNo, ChannelName, PopNum);
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

		/// <summary>
		/// 선택 한  전체 시리즈 수 체크(E_01)
		/// </summary>
		private void checkChannelTotal()
		{
			int selectedRows = grdExChannelList.GetCheckedRows().Length;

            if (selectedRows <= 0)
            {
                throw new Exception("선택한 시리즈이 없습니다! 등록을 원하는 시리즈을 선택해주세요.");
            }

            //기존의 연결시리즈 갯수 포함하여 계산
            selectedRows = existCount + grdExChannelList.GetCheckedRows().Length;

			if (selectedRows > 5)
			{
				throw new Exception("선택한 시리즈 수는 총 5개를 초과할 수 없습니다!");
			}
		}

		
		/// <summary>
		/// 선택한 시리즈 정보 구성(E_01)
		/// </summary>
		private void setChannelList()
		{
			// 1.시리즈 수 체크
			// 2.선택한 시리즈 dataTable에 추가
			
			checkChannelTotal(); // 총 시리즈 수 체크

			if (grdExChannelList.GetCheckedRows().Length > 0)
			{				
				if (dtList == null)
				{
					dtList = new DataTable("Channel");
					dtList.Columns.Add("ChannelNo", typeof(string));
					dtList.Columns.Add("ChannelName" ,typeof(string));
				}
			
				foreach(Janus.Windows.GridEX.GridEXRow row in grdExChannelList.GetCheckedRows())
				{
					DataRow nrow = dtList.NewRow();
					nrow["ChannelNo"] = row.Cells["ChannelNo"].Value.ToString();
					nrow["ChannelName"] = row.Cells["ChannelName"].Value.ToString();
					dtList.Rows.Add(nrow);
				}
			}						
		}

		
	}
}