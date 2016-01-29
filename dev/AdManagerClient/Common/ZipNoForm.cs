/*
 * -------------------------------------------------------
 * Class Name: ZipNoForm
 * 주요기능  : 우편번호 선택
 * 작성자    : ? 
 * 작성일    : ?
 * 특이사항  : 
 * -------------------------------------------------------
 * [수정사항]
 * -------------------------------------------------------
 * 수정코드  : [E_01]
 * 수정자    : bae
 * 수정일    : 2010.08.13
 * 수정부분  :
 *            - 멤버 변수와 속성들추가 
 *			  - 디자인 타임에 설정된
 *              btnOk 의 DialogResult 값을 Yes에서 None처리 함
 *            - checkRowTotal(..)
 *              initDataTable(..)
 *              initZip(..)
 *              setZip(..)
 *              btnOk_Click(..)
 *              btnSearch_Click(..)
 *              SearchContentsList(..) Overload..
 *              ContentsForm_Load(..)
 * 수정내용  :           
				- 우편번호 다중 선택가능 하도록 기능 추가
				  최대 100개이 우편번호만 선택하도록 함.
 * -------------------------------------------------------    
 
 */

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
	public class ZipNoForm : System.Windows.Forms.Form
	{
        #region 사용자정의 객체 및 변수
        // 시스템 정보 : 화면공통
        private SystemModel   systemModel   = FrameSystem.oSysModel;
        private CommonModel   commonModel   = FrameSystem.oComModel;
        private Logger        log           = FrameSystem.oLog;

        // 사용할 정보모델
        ZipCodeModel	modelData	= new ZipCodeModel();
        CurrencyManager cm			= null;
        DataTable       dt			= null;
        #endregion
		
		#region [E_01] 인한 추가 항목들...
		
		/// <summary>
		/// 최대 우편번호 선택 제한 수(default 100)
		/// </summary>
		private int maxCount = 100;
		
		/// <summary>
		/// 선택한 우편번호 수
		/// </summary>
		private int existCount = 0;
		
		/// <summary>
		/// 체크 된 우편번호 목록
		/// </summary>
		private DataTable dtList = null;
		
		/// <summary>
		/// 신규추가여부(true:신규)
		/// </summary>
		private bool isNew = true;

		/// <summary>
		/// 우편번호 단순 조회(기존값 매칭:true..else..신규 or 추가)
		/// </summary>
		private bool isReadOnly  = false;

		/// <summary>
		/// 우편번호 신규 추가여부(true:신규 else 기존것에 추가)
		/// </summary>
		public bool IsNewZip
		{
			set{ isNew = value;}
		}

		
		/// <summary>
		/// 선택된 우편번호 리스트 수
		/// </summary>
		public int ExistZipCount
		{
			set{ existCount = value;}
			get{ return existCount;}
		}


		/// <summary>
		/// 선택한 우편번호 Set/Get
		/// </summary>
		public DataTable ZipCodes
		{
			get{return dtList;}
			
			set
			{
				dtList = value;
				initZip(); // grid에 우편번호 매칭하기

				bindingZip();
			}
		}


		/// <summary>
		/// 기 선택된 우편번호 매칭작업여부(true:매칭만, else 신규 or 추가)
		/// </summary>
		public bool IsReadOnly
		{
			set{isReadOnly = value;}
		}
		#endregion

        private System.Windows.Forms.Panel pnlBtn;
        private Janus.Windows.EditControls.UIButton btnClose;
        private Janus.Windows.EditControls.UIButton btnOk;
        private System.Windows.Forms.Panel pnlGrid;
		private System.Data.DataView dvZip;
		private AdManagerClient.Common.ZipCodeDs zipCodeDs1;
		private System.Windows.Forms.Panel pnlOld;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Panel pnlSearch;
		private System.Windows.Forms.Label lblEx;
		private Janus.Windows.EditControls.UIButton btnSearch;
		private Janus.Windows.GridEX.EditControls.EditBox ebSearchKey;
		private System.Windows.Forms.Panel pnlNew;
		private Janus.Windows.GridEX.GridEX grdExZipList;
		private Janus.Windows.GridEX.GridEX grdOld;
		/// <summary>
		/// 필수 디자이너 변수입니다.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public ZipNoForm()
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
            Janus.Windows.GridEX.GridEXLayout grdExZipList_Layout_0 = new Janus.Windows.GridEX.GridEXLayout();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ZipNoForm));
            Janus.Windows.GridEX.GridEXLayout grdOld_Layout_0 = new Janus.Windows.GridEX.GridEXLayout();
            Janus.Windows.GridEX.GridEXLayout grdOld_Layout_1 = new Janus.Windows.GridEX.GridEXLayout();
            this.dvZip = new System.Data.DataView();
            this.zipCodeDs1 = new AdManagerClient.Common.ZipCodeDs();
            this.pnlGrid = new System.Windows.Forms.Panel();
            this.pnlNew = new System.Windows.Forms.Panel();
            this.grdExZipList = new Janus.Windows.GridEX.GridEX();
            this.pnlSearch = new System.Windows.Forms.Panel();
            this.lblEx = new System.Windows.Forms.Label();
            this.btnSearch = new Janus.Windows.EditControls.UIButton();
            this.ebSearchKey = new Janus.Windows.GridEX.EditControls.EditBox();
            this.pnlOld = new System.Windows.Forms.Panel();
            this.grdOld = new Janus.Windows.GridEX.GridEX();
            this.label1 = new System.Windows.Forms.Label();
            this.pnlBtn = new System.Windows.Forms.Panel();
            this.btnClose = new Janus.Windows.EditControls.UIButton();
            this.btnOk = new Janus.Windows.EditControls.UIButton();
            ((System.ComponentModel.ISupportInitialize)(this.dvZip)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.zipCodeDs1)).BeginInit();
            this.pnlGrid.SuspendLayout();
            this.pnlNew.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdExZipList)).BeginInit();
            this.pnlSearch.SuspendLayout();
            this.pnlOld.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdOld)).BeginInit();
            this.pnlBtn.SuspendLayout();
            this.SuspendLayout();
            // 
            // dvZip
            // 
            this.dvZip.AllowDelete = false;
            this.dvZip.AllowEdit = false;
            this.dvZip.AllowNew = false;
            this.dvZip.Table = this.zipCodeDs1.SystemZip;
            // 
            // zipCodeDs1
            // 
            this.zipCodeDs1.DataSetName = "ZipCodeDs";
            this.zipCodeDs1.Locale = new System.Globalization.CultureInfo("en-US");
            this.zipCodeDs1.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // pnlGrid
            // 
            this.pnlGrid.BackColor = System.Drawing.Color.WhiteSmoke;
            this.pnlGrid.Controls.Add(this.pnlNew);
            this.pnlGrid.Controls.Add(this.pnlOld);
            this.pnlGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlGrid.Location = new System.Drawing.Point(0, 0);
            this.pnlGrid.Name = "pnlGrid";
            this.pnlGrid.Size = new System.Drawing.Size(620, 504);
            this.pnlGrid.TabIndex = 21;
            // 
            // pnlNew
            // 
            this.pnlNew.Controls.Add(this.grdExZipList);
            this.pnlNew.Controls.Add(this.pnlSearch);
            this.pnlNew.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlNew.Location = new System.Drawing.Point(0, 184);
            this.pnlNew.Name = "pnlNew";
            this.pnlNew.Size = new System.Drawing.Size(620, 320);
            this.pnlNew.TabIndex = 27;
            // 
            // grdExZipList
            // 
            this.grdExZipList.AllowEdit = Janus.Windows.GridEX.InheritableBoolean.False;
            this.grdExZipList.AlternatingColors = true;
            this.grdExZipList.AutomaticSort = false;
            this.grdExZipList.BorderStyle = Janus.Windows.GridEX.BorderStyle.Flat;
            this.grdExZipList.DataSource = this.dvZip;
            this.grdExZipList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdExZipList.EmptyRows = true;
            this.grdExZipList.FocusCellFormatStyle.BackColor = System.Drawing.SystemColors.Highlight;
            this.grdExZipList.FocusCellFormatStyle.ForeColor = System.Drawing.Color.White;
            this.grdExZipList.FocusStyle = Janus.Windows.GridEX.FocusStyle.None;
            this.grdExZipList.Font = new System.Drawing.Font("맑은 고딕", 8.25F);
            this.grdExZipList.FrozenColumns = 4;
            this.grdExZipList.GridLines = Janus.Windows.GridEX.GridLines.Vertical;
            this.grdExZipList.GridLineStyle = Janus.Windows.GridEX.GridLineStyle.Solid;
            this.grdExZipList.GroupByBoxVisible = false;
            this.grdExZipList.GroupMode = Janus.Windows.GridEX.GroupMode.Collapsed;
            this.grdExZipList.HeaderFormatStyle.TextAlignment = Janus.Windows.GridEX.TextAlignment.Center;
            grdExZipList_Layout_0.DataSource = this.dvZip;
            grdExZipList_Layout_0.IsCurrentLayout = true;
            grdExZipList_Layout_0.Key = "bae";
            grdExZipList_Layout_0.LayoutString = resources.GetString("grdExZipList_Layout_0.LayoutString");
            this.grdExZipList.Layouts.AddRange(new Janus.Windows.GridEX.GridEXLayout[] {
            grdExZipList_Layout_0});
            this.grdExZipList.Location = new System.Drawing.Point(0, 43);
            this.grdExZipList.Name = "grdExZipList";
            this.grdExZipList.ScrollBars = Janus.Windows.GridEX.ScrollBars.Vertical;
            this.grdExZipList.Size = new System.Drawing.Size(620, 277);
            this.grdExZipList.TabIndex = 27;
            this.grdExZipList.TabKeyBehavior = Janus.Windows.GridEX.TabKeyBehavior.ControlNavigation;
            this.grdExZipList.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            // 
            // pnlSearch
            // 
            this.pnlSearch.BackColor = System.Drawing.SystemColors.Window;
            this.pnlSearch.Controls.Add(this.lblEx);
            this.pnlSearch.Controls.Add(this.btnSearch);
            this.pnlSearch.Controls.Add(this.ebSearchKey);
            this.pnlSearch.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlSearch.Location = new System.Drawing.Point(0, 0);
            this.pnlSearch.Name = "pnlSearch";
            this.pnlSearch.Size = new System.Drawing.Size(620, 43);
            this.pnlSearch.TabIndex = 24;
            // 
            // lblEx
            // 
            this.lblEx.Location = new System.Drawing.Point(219, 10);
            this.lblEx.Name = "lblEx";
            this.lblEx.Size = new System.Drawing.Size(261, 16);
            this.lblEx.TabIndex = 3;
            this.lblEx.Text = "예) 동이름 입력시 \'문래동\', 건물명 입력시 oo빌딩";
            // 
            // btnSearch
            // 
            this.btnSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSearch.Location = new System.Drawing.Point(508, 8);
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
            this.ebSearchKey.Size = new System.Drawing.Size(200, 22);
            this.ebSearchKey.TabIndex = 1;
            this.ebSearchKey.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
            this.ebSearchKey.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2003;
            // 
            // pnlOld
            // 
            this.pnlOld.BackColor = System.Drawing.SystemColors.Control;
            this.pnlOld.Controls.Add(this.grdOld);
            this.pnlOld.Controls.Add(this.label1);
            this.pnlOld.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlOld.Location = new System.Drawing.Point(0, 0);
            this.pnlOld.Name = "pnlOld";
            this.pnlOld.Size = new System.Drawing.Size(620, 184);
            this.pnlOld.TabIndex = 23;
            // 
            // grdOld
            // 
            this.grdOld.AllowEdit = Janus.Windows.GridEX.InheritableBoolean.False;
            this.grdOld.AlternatingColors = true;
            this.grdOld.AutomaticSort = false;
            this.grdOld.BorderStyle = Janus.Windows.GridEX.BorderStyle.Flat;
            this.grdOld.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdOld.EmptyRows = true;
            this.grdOld.FocusCellFormatStyle.BackColor = System.Drawing.SystemColors.Highlight;
            this.grdOld.FocusCellFormatStyle.ForeColor = System.Drawing.Color.White;
            this.grdOld.FocusStyle = Janus.Windows.GridEX.FocusStyle.None;
            this.grdOld.Font = new System.Drawing.Font("맑은 고딕", 8.25F);
            this.grdOld.FrozenColumns = 4;
            this.grdOld.GridLines = Janus.Windows.GridEX.GridLines.Vertical;
            this.grdOld.GridLineStyle = Janus.Windows.GridEX.GridLineStyle.Solid;
            this.grdOld.GroupByBoxVisible = false;
            this.grdOld.GroupMode = Janus.Windows.GridEX.GroupMode.Collapsed;
            this.grdOld.HeaderFormatStyle.TextAlignment = Janus.Windows.GridEX.TextAlignment.Center;
            grdOld_Layout_0.IsCurrentLayout = true;
            grdOld_Layout_0.Key = "bae";
            grdOld_Layout_0.LayoutString = resources.GetString("grdOld_Layout_0.LayoutString");
            grdOld_Layout_1.Key = "Layout1";
            this.grdOld.Layouts.AddRange(new Janus.Windows.GridEX.GridEXLayout[] {
            grdOld_Layout_0,
            grdOld_Layout_1});
            this.grdOld.Location = new System.Drawing.Point(0, 24);
            this.grdOld.Name = "grdOld";
            this.grdOld.ScrollBars = Janus.Windows.GridEX.ScrollBars.Vertical;
            this.grdOld.Size = new System.Drawing.Size(620, 160);
            this.grdOld.TabIndex = 23;
            this.grdOld.TabKeyBehavior = Janus.Windows.GridEX.TabKeyBehavior.ControlNavigation;
            this.grdOld.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.SystemColors.Window;
            this.label1.Dock = System.Windows.Forms.DockStyle.Top;
            this.label1.Font = new System.Drawing.Font("맑은 고딕", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(620, 24);
            this.label1.TabIndex = 3;
            this.label1.Text = "타겟팅 된 우편번호 정보입니다.!";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // pnlBtn
            // 
            this.pnlBtn.BackColor = System.Drawing.Color.WhiteSmoke;
            this.pnlBtn.Controls.Add(this.btnClose);
            this.pnlBtn.Controls.Add(this.btnOk);
            this.pnlBtn.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlBtn.Location = new System.Drawing.Point(0, 504);
            this.pnlBtn.Name = "pnlBtn";
            this.pnlBtn.Size = new System.Drawing.Size(620, 42);
            this.pnlBtn.TabIndex = 20;
            // 
            // btnClose
            // 
            this.btnClose.BackColor = System.Drawing.SystemColors.Control;
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.No;
            this.btnClose.Location = new System.Drawing.Point(314, 8);
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
            this.btnOk.Location = new System.Drawing.Point(202, 8);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(104, 24);
            this.btnOk.TabIndex = 4;
            this.btnOk.Text = "확 인";
            this.btnOk.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // ZipNoForm
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(6, 15);
            this.ClientSize = new System.Drawing.Size(620, 546);
            this.Controls.Add(this.pnlGrid);
            this.Controls.Add(this.pnlBtn);
            this.Font = new System.Drawing.Font("맑은 고딕", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "ZipNoForm";
            this.Text = "우편번호 검색";
            this.Load += new System.EventHandler(this.ContentsForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dvZip)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.zipCodeDs1)).EndInit();
            this.pnlGrid.ResumeLayout(false);
            this.pnlNew.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdExZipList)).EndInit();
            this.pnlSearch.ResumeLayout(false);
            this.pnlSearch.PerformLayout();
            this.pnlOld.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdOld)).EndInit();
            this.pnlBtn.ResumeLayout(false);
            this.ResumeLayout(false);

        }
		#endregion

        private void ContentsForm_Load(object sender, System.EventArgs e)
        {
            dt  = ((DataView)grdExZipList.DataSource).Table;
            cm  = (CurrencyManager)this.BindingContext[ grdExZipList.DataSource];
            cm.PositionChanged += new EventHandler(cmContents_PositionChanged);
			ebSearchKey.Focus();
			initControls(); //[E_01]
        }

		private	string	keyCode = "";
        private string  keyText = "";

		/// <summary>
		/// mode 속성에 따라서 컨트롤 visible 제어[E_01]
		/// </summary>
		private void initControls()
		{
			// 읽기 전용이면 검색 불가[E_01]
			if (isReadOnly)
			{
				btnSearch.Enabled = false;
				pnlNew.Visible = false;  // 등록 컨트롤 숨김
				pnlOld.Dock = DockStyle.Fill;
			}
			else
			{
				if (isNew)
					pnlOld.Visible  = false; // 신규추가시에 확인용 읽기 전용 컨트롤 숨김 
			}
		}

        private void cmContents_PositionChanged(object sender, EventArgs e)
        {
            int currRow = cm.Position;

            if( currRow > -1 )
            {
                keyCode = dt.Rows[currRow]["ZipCode"].ToString();
                keyText = dt.Rows[currRow]["AddrFull"].ToString();
            }
        }

        
		/// <summary>
        /// 컨텐츠을 찾아온다
        /// </summary>
        private void SearchContentsList()
        {
            try
            {
				modelData.Init();
				zipCodeDs1.SystemZip.Clear();
                modelData.SearchKey	= ebSearchKey.Text.Trim();

				if ( modelData.SearchKey.Length > 0 )
				{					
					new Common.ZipCodeManager( systemModel, commonModel).GetZipCodeList( modelData );
					if (modelData.ResultCD.Equals("0000"))
					{
						Utility.SetDataTable(zipCodeDs1.SystemZip, modelData.DsAddr);
					}
				}
				else
				{
					MessageBox.Show("검색정보를 입력하십시요.","우편번호 찾기", MessageBoxButtons.OK, MessageBoxIcon.Information );
					ebSearchKey.Focus();
					return;				
				}
            }
            catch(FrameException fe)
            {
                FrameSystem.showMsgForm("우편번호 찾기 조회오류", new string[] {fe.ErrCode, fe.ResultMsg});
            }
            catch(Exception ex)
            {
                FrameSystem.showMsgForm("우편번호 찾기 조회오류",new string[] {"",ex.Message});
            }
        }

		

		 
		/// <summary>
		/// 컨텐츠을 찾아온다(우편번호 앞3자리 기본+ 옵션(동 이름검색)[E_01]
		/// </summary>
		private void SearchContentsList(string preZip)
		{
			try
			{
				modelData.Init();
				zipCodeDs1.SystemZip.Clear();
				modelData.SearchKey	= ebSearchKey.Text.Trim(); // 동 이름
				modelData.SearchZip = preZip;
				
				new Common.ZipCodeManager( systemModel, commonModel).GetZipPreCodeList( modelData );

				if (modelData.ResultCD.Equals("0000"))
				{
					Utility.SetDataTable(zipCodeDs1.SystemZip, modelData.DsAddr);
				}				
			}
			catch(FrameException fe)
			{
				FrameSystem.showMsgForm("우편번호 찾기 조회오류", new string[] {fe.ErrCode, fe.ResultMsg});
			}
			catch(Exception ex)
			{
				FrameSystem.showMsgForm("우편번호 찾기 조회오류",new string[] {"",ex.Message});
			}
		}

		/// <summary>
		/// 이전에 타겟설정한 우편번호 가져와서 컨트롤 바인딩...
		/// </summary>
		/// <param name="zip"></param>
		private void SearchReadOnly(string zip)
		{
			try
			{
				modelData.Init();
				zipCodeDs1.SystemZip.Clear();				
				modelData.SearchZip = zip;
				
				// 확인용(read only)-타겟팅으로 등록된 우편번호만 검색				
				new Common.ZipCodeManager( systemModel, commonModel).GetZipIncludeList( modelData );					
				grdOld.DataSource = modelData.DsAddr.Tables[0];												
			}
			catch(FrameException fe)
			{
				FrameSystem.showMsgForm("우편번호 찾기 조회오류", new string[] {fe.ErrCode, fe.ResultMsg});
			}
			catch(Exception ex)
			{
				FrameSystem.showMsgForm("우편번호 찾기 조회오류",new string[] {"",ex.Message});
			}
		}


		/// <summary>
		/// 우편번호 검색 [E_01]
		/// </summary>		
        private void btnSearch_Click(object sender, System.EventArgs e)
        {            				
			SearchContentsList();			
        }

        
		/// <summary>
        /// 장르선택 이벤트 처리기
        /// </summary>
        public event ZipCodeEventHandler	SelectZipCode;

        /// <summary>
        /// 장르선택 이벤트 처리함수
        /// </summary>
        private void Selected()
        {
			try
			{				

				if( SelectZipCode != null )
				{					
					ZipCodeEventArgs args = new ZipCodeEventArgs();
					args.ZipCode	= keyCode;
					args.ZipAddr	= keyText;
					SelectZipCode( this, args );
				}

				this.Close();
			}
			catch(Exception ex)
			{
				MessageBox.Show(ex.Message);
			}            
        }

        private void btnOk_Click(object sender, System.EventArgs e)
        {
			// [E_01]
            //this.Selected();
			setZip();
			
        }

        private void btnClose_Click(object sender, System.EventArgs e)
        {
			this.DialogResult = DialogResult.No;
            this.Close();
        }

		private void grdExZipList_DoubleClick(object sender, System.EventArgs e)
		{
			// [E_01]
			//this.Selected();
		}

		
		
		/// <summary>
		/// 선택 한  전체 채널 수 체크(E_01)
		/// </summary>
		private void checkRowTotal()
		{
			int selectedRows = 0;
			
			// 이전에 등록 된 우편번호 중 체크된 목록 수도 포함시킴(신규가 아닌 추가인 경우)
			if (isReadOnly)			
				selectedRows = grdOld.GetCheckedRows().Length;
			
			selectedRows += grdExZipList.GetCheckedRows().Length;

			if (selectedRows > maxCount)
			{
				throw new Exception( string.Format("선택한 우편번호는 총 {0}개를 초과할 수 없습니다.!" ,maxCount) );
			}
		}
		
		
		/// <summary>
		/// TALBE 구성[E_01]
		/// </summary>
		private void initDataTable()
		{
			if (dtList == null)
			{
				dtList = new DataTable("Zip");
				dtList.Columns.Add("ZipCode" ,typeof(string));						
			}
		}

		
		/// <summary>
		/// 우편번호 기 선택된 항목은 Check-in 처리 함.[E_01]
		/// </summary>
		private void initZip()
		{			
			string groupZip = "";
			try
			{				
				initDataTable();

				if (dtList != null)
				{
					if (isNew == false) // 타겟팅 설정 한 우편번호 //신규 추가 아니면 무조건 실행
					{
						foreach(DataRow row in dtList.Rows)
						{
							// systemZip 에 등록된 데이터 형식에 맞게 구성
							groupZip += "'" + row["ZipCode"].ToString().Substring(0,3)
								            + "-"
								            + row["ZipCode"].ToString().Substring(3,3)
								            + "',";
						}
						// 마지지막 구분자 제거(,)
						if (groupZip.Length > 6)
							groupZip = groupZip.Substring(0, groupZip.Length -1);
						
						// 검색
						SearchReadOnly(groupZip);
					}

					#region 이전에 타겟설정한 우편번호 컨트롤 분리 처리하므로 불필요...
					/*
					if (isReadOnly == false)
					{
						groupZip = "";
						// 리스트에서 우편번호 앞3자리만 처리해서
						// 우편번호 목록을 가져온 후 매칭처리 함.
						// why? 우편번호가 너무 많아서(5만)
						foreach(DataRow row in dtList.Rows)
						{
							if (oldZip == "")
							{
								oldZip =  row["ZipCode"].ToString().Trim().Substring(0,3);
								groupZip += oldZip + ",";
							}
							else
							{
								newZip =  row["ZipCode"].ToString().Trim().Substring(0,3);
								if (oldZip != newZip)
								{
									groupZip += newZip + ",";
									oldZip = newZip;
								}
							}
						}

						// 중복되는 것은 제외한 우편번호 앞 3자리 값
						groupZip = groupZip.Substring(0, groupZip.Length - 1);
						// 검색
						SearchContentsList(groupZip);
					}					
					*/					
					#endregion

					
					
				}// end if
				
			}
			catch(Exception)
			{
				if (dtList != null)
				{
					dtList.Clear();
					dtList.Dispose();
					dtList = null;
				}
			}
		
		}


		/// <summary>
		/// 기 타겟 설정한 우편번호 체크박스 바인딩 처리
		/// </summary>
		private void bindingZip()
		{
			string oldZip = "";
			string gridZip = "";
			try
			{
				foreach(DataRow row in dtList.Rows)
				{
					oldZip =  row["ZipCode"].ToString().Trim();
					// 기존 우편번호 그리드에 체크인 처리(상단 그리드)		
					foreach(Janus.Windows.GridEX.GridEXRow grow in grdOld.GetRows())
					{
						gridZip = grow.Cells["ZipCode"].Value.ToString().Trim().Replace("-","");						
						if (oldZip.Equals(gridZip))						
							grow.Cells.Row.IsChecked = true;						
					}

					#region 이전에 타겟설정한 우편번호 컨트롤 분리 처리하므로 불필요...
					/*						
						if (isReadOnly == false) // 읽기 상태에선 처리 불 필요(수정 되는 항목이므로)
						{
							foreach(Janus.Windows.GridEX.GridEXRow grow in grdExZipList.GetRows())
							{
								gridZip = grow.Cells["ZipCode"].Value.ToString().Trim().Replace("-","");
						
								if (oldZip.Equals(gridZip))
								{
									//grow.Cells.Row.IsChecked = true;
									
									
								}
							}
						}
						*/
					#endregion
				}
			}
			catch(Exception)
			{
			}
		}



		/// <summary>
		/// 우편번호 선택 값 테이블 구성[E_01]
		/// </summary>
		private void setZip()
		{
			try
			{
				checkRowTotal(); // 체크 수 체크
				
				initDataTable();
				dtList.Clear();
				
				// 기존 값에 추가하는 경우에만 적용
				if (isNew == false && isReadOnly == false)
				{
					if (grdOld.GetCheckedRows().Length > 0)
					{										
						foreach(Janus.Windows.GridEX.GridEXRow row in grdOld.GetCheckedRows())
						{
							DataRow nrow    = dtList.NewRow();
							nrow["ZipCode"] = row.Cells["ZipCode"].Value.ToString().Replace("-","");						
							dtList.Rows.Add(nrow);
						}
					}
				}
				
				if (isReadOnly == false)
				{
					if (grdExZipList.GetCheckedRows().Length > 0)
					{										
						foreach(Janus.Windows.GridEX.GridEXRow row in grdExZipList.GetCheckedRows())
						{
							DataRow nrow    = dtList.NewRow();
							nrow["ZipCode"] = row.Cells["ZipCode"].Value.ToString().Replace("-","");						
							dtList.Rows.Add(nrow);
						}
					}
				}

				this.DialogResult = DialogResult.Yes;
				this.Close();
			}
			catch(Exception ex)
			{				
				MessageBox.Show(ex.Message, "정보" ,MessageBoxButtons.OK, MessageBoxIcon.Information);
			}
		}

    }
}
