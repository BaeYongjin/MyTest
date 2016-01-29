using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;

using WinFramework.Misc;
using WinFramework.Base;
using WinFramework.Data;

using AdManagerModel;

namespace AdManagerClient
{
	/// <summary>
	/// ReportHeader1에 대한 요약 설명입니다.
	/// </summary>
    public class ReportHeaderControl : System.Windows.Forms.UserControl
	{
		#region [ 사용자정의 객체 및 변수 ]
		// 시스템 정보 : 화면공통
		private	SystemModel	oSystemModel	=	FrameSystem.oSysModel;
		private	CommonModel	oCommonModel	=	FrameSystem.oComModel;
		private	Logger		oLog				=	FrameSystem.oLog;
		private	MenuPower	oMenu				=	FrameSystem.oMenu;

		// 메뉴코드 : 보안이 필요한 화면에 필요함
		private	string	MenuCode			= "";
		private	bool		canRead			= false;
		private	bool		canPrint			= false;

		private	SearchReportData	rptData;
		private	SummaryAdModel summaryAdModel = new SummaryAdModel();	// 총괄집계모델

		#endregion

		#region [ 화면 UI관련 컴포넌트 정의 ]
		private System.Windows.Forms.Panel pnlSearch;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label2;
		private Janus.Windows.EditControls.UIComboBox cbCampaign;
		private Janus.Windows.EditControls.UIGroupBox uiGroupBox1;
		private Janus.Windows.EditControls.UIRadioButton rbSearchType_W;
		private Janus.Windows.EditControls.UIRadioButton rbSearchType_C;
		private Janus.Windows.EditControls.UIRadioButton rbSearchType_M;
		private Janus.Windows.EditControls.UIRadioButton rbSearchType_D;
		private Janus.Windows.CalendarCombo.CalendarCombo cbSearchStartDay;
		private System.Windows.Forms.Label lbSearchDate;
		private Janus.Windows.EditControls.UIComboBox cbSearchMedia;
		private Janus.Windows.EditControls.UIComboBox cbSearchContract;
		private Janus.Windows.EditControls.UIButton btnSearch;
		private Janus.Windows.EditControls.UIButton btnExcel;
		private System.Windows.Forms.Label label6;
		private Janus.Windows.EditControls.UIComboBox cbSearchItem;
		private System.Windows.Forms.Label label10;
		private Janus.Windows.CalendarCombo.CalendarCombo cbSearchEndDay;
		public AdManagerClient._60_ReportAd.ReportHeaderDs rptHeaderDs;
		private Janus.Windows.GridEX.EditControls.EditBox ebExcuteStartDay;
		private Janus.Windows.GridEX.EditControls.EditBox ebExcuteEndDay;
		private System.Windows.Forms.Label lblContractDate;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label5;
		private Janus.Windows.GridEX.EditControls.EditBox ebAgency;
		private Janus.Windows.GridEX.EditControls.EditBox ebAdvertiser;
		/// <summary> 
		/// 필수 디자이너 변수입니다.
		/// </summary>
		private System.ComponentModel.Container components = null;
		#endregion
	
		#region[  속 성 들 ] 
		/// <summary>
		/// 업무명을 설정하거나, 가져옵니다.
		/// </summary>
		public string u_MenuName
		{
			get
			{
				return MenuCode;
			}

			set
			{
				MenuCode = value;
			}
		}

		/// <summary>
		/// 프린트가 가능여부를 설정하거나, 가져옵니다.
		/// </summary>
		public bool u_IsPrint
		{
			get
			{
				return canPrint;
			}

			set
			{
				DisableButton();
				canPrint = value;
				InitButton();
			}
		}

		/// <summary>
		/// 조회 일자구분을 텍스트로 가져옵니다(기간/일간/주간/월간)
		/// </summary>
		public string u_DayType
		{
			get
			{
				string rtn = "";
				if( rbSearchType_C.Checked )			rtn = "기간";
				else if( rbSearchType_D.Checked )	rtn = "일간";
				else if( rbSearchType_W.Checked )	rtn = "주간";
				else if( rbSearchType_M.Checked )	rtn = "월간";
				else											rtn = "일일";

				return rtn;
			
			}
		}
		#endregion

		public ReportHeaderControl()
		{
			// 이 호출은 Windows.Forms Form 디자이너에 필요합니다.
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


		#region 구성 요소 디자이너에서 생성한 코드
		/// <summary> 
		/// 디자이너 지원에 필요한 메서드입니다. 
		/// 이 메서드의 내용을 코드 편집기로 수정하지 마십시오.
		/// </summary>
		private void InitializeComponent()
		{
            this.pnlSearch = new System.Windows.Forms.Panel();
            this.label5 = new System.Windows.Forms.Label();
            this.ebAgency = new Janus.Windows.GridEX.EditControls.EditBox();
            this.ebAdvertiser = new Janus.Windows.GridEX.EditControls.EditBox();
            this.label1 = new System.Windows.Forms.Label();
            this.lblContractDate = new System.Windows.Forms.Label();
            this.ebExcuteStartDay = new Janus.Windows.GridEX.EditControls.EditBox();
            this.ebExcuteEndDay = new Janus.Windows.GridEX.EditControls.EditBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.cbCampaign = new Janus.Windows.EditControls.UIComboBox();
            this.uiGroupBox1 = new Janus.Windows.EditControls.UIGroupBox();
            this.rbSearchType_W = new Janus.Windows.EditControls.UIRadioButton();
            this.rbSearchType_C = new Janus.Windows.EditControls.UIRadioButton();
            this.rbSearchType_M = new Janus.Windows.EditControls.UIRadioButton();
            this.rbSearchType_D = new Janus.Windows.EditControls.UIRadioButton();
            this.cbSearchStartDay = new Janus.Windows.CalendarCombo.CalendarCombo();
            this.lbSearchDate = new System.Windows.Forms.Label();
            this.cbSearchMedia = new Janus.Windows.EditControls.UIComboBox();
            this.cbSearchContract = new Janus.Windows.EditControls.UIComboBox();
            this.btnSearch = new Janus.Windows.EditControls.UIButton();
            this.btnExcel = new Janus.Windows.EditControls.UIButton();
            this.label6 = new System.Windows.Forms.Label();
            this.cbSearchItem = new Janus.Windows.EditControls.UIComboBox();
            this.label10 = new System.Windows.Forms.Label();
            this.cbSearchEndDay = new Janus.Windows.CalendarCombo.CalendarCombo();
            this.rptHeaderDs = new AdManagerClient._60_ReportAd.ReportHeaderDs();
            this.pnlSearch.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uiGroupBox1)).BeginInit();
            this.uiGroupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.rptHeaderDs)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlSearch
            // 
            this.pnlSearch.BackColor = System.Drawing.SystemColors.Window;
            this.pnlSearch.Controls.Add(this.label5);
            this.pnlSearch.Controls.Add(this.ebAgency);
            this.pnlSearch.Controls.Add(this.ebAdvertiser);
            this.pnlSearch.Controls.Add(this.label1);
            this.pnlSearch.Controls.Add(this.lblContractDate);
            this.pnlSearch.Controls.Add(this.ebExcuteStartDay);
            this.pnlSearch.Controls.Add(this.ebExcuteEndDay);
            this.pnlSearch.Controls.Add(this.label4);
            this.pnlSearch.Controls.Add(this.label2);
            this.pnlSearch.Controls.Add(this.cbCampaign);
            this.pnlSearch.Controls.Add(this.uiGroupBox1);
            this.pnlSearch.Controls.Add(this.cbSearchStartDay);
            this.pnlSearch.Controls.Add(this.lbSearchDate);
            this.pnlSearch.Controls.Add(this.cbSearchMedia);
            this.pnlSearch.Controls.Add(this.cbSearchContract);
            this.pnlSearch.Controls.Add(this.btnSearch);
            this.pnlSearch.Controls.Add(this.btnExcel);
            this.pnlSearch.Controls.Add(this.label6);
            this.pnlSearch.Controls.Add(this.cbSearchItem);
            this.pnlSearch.Controls.Add(this.label10);
            this.pnlSearch.Controls.Add(this.cbSearchEndDay);
            this.pnlSearch.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlSearch.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.pnlSearch.Location = new System.Drawing.Point(0, 0);
            this.pnlSearch.Name = "pnlSearch";
            this.pnlSearch.Size = new System.Drawing.Size(1010, 108);
            this.pnlSearch.TabIndex = 4;
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(27, 80);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(96, 21);
            this.label5.TabIndex = 190;
            this.label5.Text = "대행사/광고주";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // ebAgency
            // 
            this.ebAgency.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ebAgency.Location = new System.Drawing.Point(124, 80);
            this.ebAgency.Name = "ebAgency";
            this.ebAgency.ReadOnly = true;
            this.ebAgency.Size = new System.Drawing.Size(144, 23);
            this.ebAgency.TabIndex = 192;
            this.ebAgency.TabStop = false;
            this.ebAgency.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
            this.ebAgency.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            // 
            // ebAdvertiser
            // 
            this.ebAdvertiser.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ebAdvertiser.Location = new System.Drawing.Point(272, 80);
            this.ebAdvertiser.Name = "ebAdvertiser";
            this.ebAdvertiser.ReadOnly = true;
            this.ebAdvertiser.Size = new System.Drawing.Size(144, 23);
            this.ebAdvertiser.TabIndex = 191;
            this.ebAdvertiser.TabStop = false;
            this.ebAdvertiser.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
            this.ebAdvertiser.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(588, 60);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(20, 16);
            this.label1.TabIndex = 188;
            this.label1.Text = "~";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblContractDate
            // 
            this.lblContractDate.Location = new System.Drawing.Point(392, 56);
            this.lblContractDate.Name = "lblContractDate";
            this.lblContractDate.Size = new System.Drawing.Size(72, 21);
            this.lblContractDate.TabIndex = 187;
            this.lblContractDate.Text = "계약일자";
            this.lblContractDate.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ebExcuteStartDay
            // 
            this.ebExcuteStartDay.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ebExcuteStartDay.Location = new System.Drawing.Point(476, 56);
            this.ebExcuteStartDay.Name = "ebExcuteStartDay";
            this.ebExcuteStartDay.Size = new System.Drawing.Size(104, 23);
            this.ebExcuteStartDay.TabIndex = 185;
            this.ebExcuteStartDay.TextAlignment = Janus.Windows.GridEX.TextAlignment.Center;
            this.ebExcuteStartDay.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            // 
            // ebExcuteEndDay
            // 
            this.ebExcuteEndDay.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ebExcuteEndDay.Location = new System.Drawing.Point(616, 56);
            this.ebExcuteEndDay.Name = "ebExcuteEndDay";
            this.ebExcuteEndDay.Size = new System.Drawing.Size(104, 23);
            this.ebExcuteEndDay.TabIndex = 186;
            this.ebExcuteEndDay.TextAlignment = Janus.Windows.GridEX.TextAlignment.Center;
            this.ebExcuteEndDay.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(63, 54);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(60, 21);
            this.label4.TabIndex = 184;
            this.label4.Text = "개별광고";
            this.label4.TextAlign = System.Drawing.ContentAlignment.BottomRight;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(80, 31);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(44, 21);
            this.label2.TabIndex = 183;
            this.label2.Text = "캠페인";
            this.label2.TextAlign = System.Drawing.ContentAlignment.BottomRight;
            // 
            // cbCampaign
            // 
            this.cbCampaign.BackColor = System.Drawing.Color.White;
            this.cbCampaign.ComboStyle = Janus.Windows.EditControls.ComboStyle.DropDownList;
            this.cbCampaign.Location = new System.Drawing.Point(124, 32);
            this.cbCampaign.Name = "cbCampaign";
            this.cbCampaign.Size = new System.Drawing.Size(244, 23);
            this.cbCampaign.TabIndex = 2;
            this.cbCampaign.Text = "캠페인선택";
            this.cbCampaign.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.cbCampaign.SelectedIndexChanged += new System.EventHandler(this.cbCampaign_SelectedIndexChanged);
            // 
            // uiGroupBox1
            // 
            this.uiGroupBox1.BorderColor = System.Drawing.Color.DarkGray;
            this.uiGroupBox1.Controls.Add(this.rbSearchType_W);
            this.uiGroupBox1.Controls.Add(this.rbSearchType_C);
            this.uiGroupBox1.Controls.Add(this.rbSearchType_M);
            this.uiGroupBox1.Controls.Add(this.rbSearchType_D);
            this.uiGroupBox1.FrameStyle = Janus.Windows.EditControls.FrameStyle.None;
            this.uiGroupBox1.Location = new System.Drawing.Point(472, -4);
            this.uiGroupBox1.Name = "uiGroupBox1";
            this.uiGroupBox1.Size = new System.Drawing.Size(256, 32);
            this.uiGroupBox1.TabIndex = 20;
            this.uiGroupBox1.VisualStyle = Janus.Windows.UI.Dock.PanelVisualStyle.Office2003;
            // 
            // rbSearchType_W
            // 
            this.rbSearchType_W.Location = new System.Drawing.Point(140, 12);
            this.rbSearchType_W.Name = "rbSearchType_W";
            this.rbSearchType_W.ShowFocusRectangle = false;
            this.rbSearchType_W.Size = new System.Drawing.Size(48, 23);
            this.rbSearchType_W.TabIndex = 2;
            this.rbSearchType_W.Text = "주간";
            this.rbSearchType_W.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.rbSearchType_W.CheckedChanged += new System.EventHandler(this.rbSearchType_W_CheckedChanged);
            // 
            // rbSearchType_C
            // 
            this.rbSearchType_C.BackColor = System.Drawing.SystemColors.Window;
            this.rbSearchType_C.Location = new System.Drawing.Point(4, 12);
            this.rbSearchType_C.Name = "rbSearchType_C";
            this.rbSearchType_C.ShowFocusRectangle = false;
            this.rbSearchType_C.Size = new System.Drawing.Size(64, 23);
            this.rbSearchType_C.TabIndex = 0;
            this.rbSearchType_C.Text = "기간";
            this.rbSearchType_C.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.rbSearchType_C.CheckedChanged += new System.EventHandler(this.rbSearchType_C_CheckedChanged);
            // 
            // rbSearchType_M
            // 
            this.rbSearchType_M.Location = new System.Drawing.Point(204, 12);
            this.rbSearchType_M.Name = "rbSearchType_M";
            this.rbSearchType_M.ShowFocusRectangle = false;
            this.rbSearchType_M.Size = new System.Drawing.Size(48, 23);
            this.rbSearchType_M.TabIndex = 3;
            this.rbSearchType_M.Text = "월간";
            this.rbSearchType_M.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.rbSearchType_M.CheckedChanged += new System.EventHandler(this.rbSearchType_M_CheckedChanged);
            // 
            // rbSearchType_D
            // 
            this.rbSearchType_D.Checked = true;
            this.rbSearchType_D.Location = new System.Drawing.Point(76, 12);
            this.rbSearchType_D.Name = "rbSearchType_D";
            this.rbSearchType_D.ShowFocusRectangle = false;
            this.rbSearchType_D.Size = new System.Drawing.Size(56, 23);
            this.rbSearchType_D.TabIndex = 1;
            this.rbSearchType_D.TabStop = true;
            this.rbSearchType_D.Text = "일간";
            this.rbSearchType_D.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.rbSearchType_D.CheckedChanged += new System.EventHandler(this.rbSearchType_D_CheckedChanged);
            // 
            // cbSearchStartDay
            // 
            // 
            // 
            // 
            this.cbSearchStartDay.DropDownCalendar.BorderStyle = Janus.Windows.CalendarCombo.BorderStyle.SunkenLight3D;
            this.cbSearchStartDay.DropDownCalendar.FirstDayOfWeek = Janus.Windows.CalendarCombo.CalendarDayOfWeek.Sunday;
            this.cbSearchStartDay.DropDownCalendar.FirstMonth = new System.DateTime(2007, 6, 1, 0, 0, 0, 0);
            this.cbSearchStartDay.DropDownCalendar.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbSearchStartDay.DropDownCalendar.HeaderAppearance = Janus.Windows.CalendarCombo.ButtonAppearance.Regular;
            this.cbSearchStartDay.DropDownCalendar.HeaderFont = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbSearchStartDay.DropDownCalendar.VisualStyle = Janus.Windows.CalendarCombo.VisualStyle.Office2007;
            this.cbSearchStartDay.Location = new System.Drawing.Point(476, 32);
            this.cbSearchStartDay.Name = "cbSearchStartDay";
            this.cbSearchStartDay.Size = new System.Drawing.Size(104, 23);
            this.cbSearchStartDay.TabIndex = 4;
            this.cbSearchStartDay.Value = new System.DateTime(2007, 9, 14, 0, 0, 0, 0);
            this.cbSearchStartDay.VisualStyle = Janus.Windows.CalendarCombo.VisualStyle.Office2007;
            this.cbSearchStartDay.TextChanged += new System.EventHandler(this.cbSearchStartDay_TextChanged);
            // 
            // lbSearchDate
            // 
            this.lbSearchDate.Location = new System.Drawing.Point(392, 34);
            this.lbSearchDate.Name = "lbSearchDate";
            this.lbSearchDate.Size = new System.Drawing.Size(72, 21);
            this.lbSearchDate.TabIndex = 14;
            this.lbSearchDate.Text = "집계일자";
            this.lbSearchDate.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cbSearchMedia
            // 
            this.cbSearchMedia.BackColor = System.Drawing.Color.White;
            this.cbSearchMedia.ComboStyle = Janus.Windows.EditControls.ComboStyle.DropDownList;
            this.cbSearchMedia.Location = new System.Drawing.Point(8, 8);
            this.cbSearchMedia.Name = "cbSearchMedia";
            this.cbSearchMedia.Size = new System.Drawing.Size(112, 23);
            this.cbSearchMedia.TabIndex = 0;
            this.cbSearchMedia.Text = "매체선택";
            this.cbSearchMedia.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.cbSearchMedia.SelectedIndexChanged += new System.EventHandler(this.cbSearchMedia_SelectedIndexChanged);
            // 
            // cbSearchContract
            // 
            this.cbSearchContract.BackColor = System.Drawing.Color.White;
            this.cbSearchContract.BorderStyle = Janus.Windows.UI.BorderStyle.Sunken;
            this.cbSearchContract.ComboStyle = Janus.Windows.EditControls.ComboStyle.DropDownList;
            this.cbSearchContract.Location = new System.Drawing.Point(124, 8);
            this.cbSearchContract.Name = "cbSearchContract";
            this.cbSearchContract.Size = new System.Drawing.Size(244, 23);
            this.cbSearchContract.TabIndex = 1;
            this.cbSearchContract.Text = "광고계약선택";
            this.cbSearchContract.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.cbSearchContract.Click += new System.EventHandler(this.cbSearchContract_Click);
            // 
            // btnSearch
            // 
            this.btnSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSearch.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSearch.Enabled = false;
            this.btnSearch.Location = new System.Drawing.Point(901, 14);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(104, 24);
            this.btnSearch.TabIndex = 6;
            this.btnSearch.Text = "조 회";
            this.btnSearch.ToolTipText = "선택한 조건대로 집계합니다";
            this.btnSearch.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // btnExcel
            // 
            this.btnExcel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExcel.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnExcel.Enabled = false;
            this.btnExcel.Location = new System.Drawing.Point(901, 42);
            this.btnExcel.Name = "btnExcel";
            this.btnExcel.Size = new System.Drawing.Size(104, 24);
            this.btnExcel.TabIndex = 7;
            this.btnExcel.Text = "EXCEL 출력";
            this.btnExcel.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.btnExcel.Click += new System.EventHandler(this.btnExcel_Click);
            // 
            // label6
            // 
            this.label6.Location = new System.Drawing.Point(392, 10);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(68, 21);
            this.label6.TabIndex = 14;
            this.label6.Text = "기간구분";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cbSearchItem
            // 
            this.cbSearchItem.BackColor = System.Drawing.Color.White;
            this.cbSearchItem.ComboStyle = Janus.Windows.EditControls.ComboStyle.DropDownList;
            this.cbSearchItem.Location = new System.Drawing.Point(124, 56);
            this.cbSearchItem.Name = "cbSearchItem";
            this.cbSearchItem.Size = new System.Drawing.Size(244, 23);
            this.cbSearchItem.TabIndex = 3;
            this.cbSearchItem.Text = "광고선택";
            this.cbSearchItem.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.cbSearchItem.SelectedItemChanged += new System.EventHandler(this.cbSearchItem_SelectedItemChanged);
            // 
            // label10
            // 
            this.label10.Location = new System.Drawing.Point(588, 36);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(20, 16);
            this.label10.TabIndex = 4;
            this.label10.Text = "~";
            this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // cbSearchEndDay
            // 
            // 
            // 
            // 
            this.cbSearchEndDay.DropDownCalendar.FirstMonth = new System.DateTime(2007, 8, 1, 0, 0, 0, 0);
            this.cbSearchEndDay.DropDownCalendar.VisualStyle = Janus.Windows.CalendarCombo.VisualStyle.Office2007;
            this.cbSearchEndDay.Enabled = false;
            this.cbSearchEndDay.Location = new System.Drawing.Point(616, 32);
            this.cbSearchEndDay.Name = "cbSearchEndDay";
            this.cbSearchEndDay.Size = new System.Drawing.Size(104, 23);
            this.cbSearchEndDay.TabIndex = 5;
            this.cbSearchEndDay.Value = new System.DateTime(2007, 9, 14, 0, 0, 0, 0);
            this.cbSearchEndDay.VisualStyle = Janus.Windows.CalendarCombo.VisualStyle.Office2007;
            // 
            // rptHeaderDs
            // 
            this.rptHeaderDs.DataSetName = "ReportHeaderDs";
            this.rptHeaderDs.Locale = new System.Globalization.CultureInfo("en-US");
            this.rptHeaderDs.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // ReportHeaderControl
            // 
            this.BackColor = System.Drawing.Color.DeepSkyBlue;
            this.Controls.Add(this.pnlSearch);
            this.Name = "ReportHeaderControl";
            this.Size = new System.Drawing.Size(1010, 108);
            this.Load += new System.EventHandler(this.ReportHeader1_Load);
            this.pnlSearch.ResumeLayout(false);
            this.pnlSearch.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uiGroupBox1)).EndInit();
            this.uiGroupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.rptHeaderDs)).EndInit();
            this.ResumeLayout(false);

        }
		#endregion

		#region [ UI 관련 이벤트 처리기 ]
		/// <summary>
		/// 컨트롤 Load이벤트 처리기
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void ReportHeader1_Load(object sender, System.EventArgs e)
		{
			// 컨트롤 초기화
			//InitControl();		
		}
		#endregion

		#region [ 컨트롤 초기화 ]
		/// <summary>
		/// 컨트롤들을 초기화 시킨다.
		/// </summary>
		private void InitControl()
		{
			ProgressStart();
			InitCombo();
			if( oMenu.CanRead(MenuCode))	canRead = true;
			InitButton();
			ProgressStop();
		}


		/// <summary>
		/// 콤보박스 초기화및 기타 컨트롤 초기화
		/// </summary>
		private void InitCombo()
		{
			Init_MediaCode();
			Init_cbSearchContract();
			Init_cbCampaignCode();
			Init_cbSearchItem();
			InitCombo_Level();
			cbSearchStartDay.Value = DateTime.Now.AddDays(-1);
			rbSearchType_D.Checked = true;
		}

		/// <summary>
		/// 매체코드 초기화 및 데이터 로드
		/// </summary>
		private void Init_MediaCode()
		{
			// 매체를 조회한다.
			MediaCodeModel mediacodeModel = new MediaCodeModel();
			new MediaCodeManager(oSystemModel, oCommonModel).GetMediaCodeList(mediacodeModel);
			
			if (mediacodeModel.ResultCD.Equals("0000"))
			{
				Utility.SetDataTable( rptHeaderDs.Media, mediacodeModel.MediaCodeDataSet);				
			}

			// 검색조건의 콤보
			this.cbSearchMedia.Items.Clear();
			
			// 콤보박스에 셋트할 코드목록을 담을 Item배열을 선언
			Janus.Windows.EditControls.UIComboBoxItem[] comboItems = new Janus.Windows.EditControls.UIComboBoxItem[mediacodeModel.ResultCnt + 1];

			comboItems[0] = new Janus.Windows.EditControls.UIComboBoxItem("【 매체선택 】","00");
			
			for(int i=0;i<mediacodeModel.ResultCnt;i++)
			{
				DataRow row = rptHeaderDs.Media.Rows[i];

				string	val = row["MediaCode"].ToString();
				string	txt = row["MediaName"].ToString();
				comboItems[i+1] = new Janus.Windows.EditControls.UIComboBoxItem(txt,val);
			}
			// 콤보에 셋트
			this.cbSearchMedia.Items.AddRange(comboItems);
			this.cbSearchMedia.SelectedIndex = 0;

			Application.DoEvents();
		}


		/// <summary>
		/// 계약데이터 읽어오기
		/// </summary>
		private void Init_cbSearchContract()
		{

			// 검색조건의 콤보
			this.cbSearchContract.Items.Clear();
			
			// 콤보에 셋트
			this.cbSearchContract.Items.Add(new Janus.Windows.EditControls.UIComboBoxItem("계약 선택","00"));
			this.cbSearchContract.SelectedIndex = 0;

			Application.DoEvents();
		}


		/// <summary>
		/// 캠페인콤보 초기화
		/// </summary>
		private void Init_cbCampaignCode()
		{
			// 검색조건의 콤보
			this.cbCampaign.Items.Clear();

			// 콤보에 셋팅
			this.cbCampaign.Items.Add(new Janus.Windows.EditControls.UIComboBoxItem("캠페인 선택","00"));
			this.cbCampaign.SelectedIndex = 0;
			Application.DoEvents();
		}


		/// <summary>
		/// 내역콤보 초기화
		/// </summary>
		private void Init_cbSearchItem()
		{
			// 검색조건의 콤보
			this.cbSearchItem.Items.Clear();
			this.cbSearchItem.Items.Add(new Janus.Windows.EditControls.UIComboBoxItem("광고 선택","00"));
			this.cbSearchItem.SelectedIndex = 0;

			Application.DoEvents();
		}


		private void InitCombo_Level()
		{

			if(oCommonModel.UserLevel=="20")
			{
				cbSearchMedia.SelectedValue = oCommonModel.MediaCode;			
				cbSearchMedia.ReadOnly = true;					
			}
			else
			{
				for(int i=0;i < rptHeaderDs.Media.Rows.Count;i++)
				{
					DataRow row = rptHeaderDs.Media.Rows[i];					
					if(row["MediaCode"].ToString().Equals(FrameSystem._HANATV.ToString()))
					{
						cbSearchMedia.SelectedValue = FrameSystem._HANATV; // 하나TV를 기본값으로 한다.	 		
						break;															
					}
					else
					{
						cbSearchMedia.SelectedValue="00";
					}
				}	
			}
		
			Application.DoEvents();
		}


		private void InitButton()
		{
			if(canRead)
			{
				btnSearch.Enabled = true;
			}
			
			if(canPrint)
			{
				btnExcel.Enabled = true;
			}

			cbSearchContract.Focus();
			Application.DoEvents();
		}
       
		private void DisableButton()
		{
			btnSearch.Enabled	= false;
			btnExcel.Enabled  = false;
		}



		#endregion

		#region [ 이벤트 핸들러 및 함수 ]
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

		public event ProgressEventHandler 			ProgressEvent;			// 처리중이벤트 핸들러
		private void ProgressStart()
		{
			if (ProgressEvent != null) 
			{
				ProgressEventArgs ea = new ProgressEventArgs();
				ea.Type   = ProgressEventArgs.Start;
				ProgressEvent(this,ea);
			}
		}

		private void ProgressStop()
		{
			if (ProgressEvent != null) 
			{
				ProgressEventArgs ea = new ProgressEventArgs();
				ea.Type   = ProgressEventArgs.Stop;
				ProgressEvent(this,ea);
			}
		}
		#endregion

		/// <summary>
		/// 컨트롤들을 초기화 시킨후, 매체코드를 읽어와 설정합니다.
		/// </summary>
		public void u_InitControl()
		{
			InitControl();
		}

		/// <summary>
		/// 계약을 클릭했을때 처리
		/// 계약 팝업을 띄운후 계약을 선택한다
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void cbSearchContract_Click(object sender, System.EventArgs e)
		{
			if(cbSearchMedia.SelectedValue.Equals("00"))
			{
				MessageBox.Show("매체를 선택해 주십시오.", "광고시스템",MessageBoxButtons.OK, MessageBoxIcon.Information);
				cbSearchMedia.Focus();
				return;
			}

			DisableButton();	
			
			SearchContractForm	form = new SearchContractForm();
			form.ContractSelected += new ContractEventHandler(OnContractSelected);
			form.ShowDialog();

			if( form != null )
			{
				form.Dispose();
				form = null;
			}
			InitButton();				
		}

		/// <summary>
		/// 계약검색 윈도우에서 계약선택이벤트건 처리하는 함수
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void OnContractSelected(object sender, ContractEventArgs e)
		{
			//_60_ReportAd.SearchReportData data;
			//data.ContractSeq	=	e.ContractSeq;
			//data.ContractName	=	e.ContractName;
			rptData.ContractSeq	=	e.ContractSeq;
			rptData.ContractName	=	e.ContractName;
			rptData.ContBeginDay	=	e.BeginDay;
			rptData.ContEndDay	=	e.EndDay;
			rptData.CampaignNo	=	"00";
			rptData.CampaignName	=	"";
			rptData.ItemNo			=	"00";
			rptData.ItemName		=	"";
			rptData.ItemBeginDay	=  "";
			rptData.ItemEndDay	=  "";
			rptData.AgencyName	=	e.AgencyName;
			rptData.AdvertiserName = e.AdvertiserName;
			
			SetContract();
			SetCampaign();

			ebExcuteStartDay.Text	= Utility.reConvertDate(e.BeginDay);
			ebExcuteEndDay.Text		= Utility.reConvertDate(e.EndDay);

			//contractEndDay   = Utility.reConvertDate(EndDay);	// 이벤트 처리를 위해 EndDay를 먼저 셋트한다.
			//contractStartDay = Utility.reConvertDate(StartDay);
			ebAgency.Text     = rptData.AgencyName;
			ebAdvertiser.Text = rptData.AdvertiserName;

			//summaryAdDs.Report.Clear();

			canPrint = false;
			InitButton();
		}

		/// <summary>
		/// 계약콤보에 읽어온 값을 설정
		/// </summary>
		private void SetContract()
		{
			// 검색조건의 콤보
			this.cbSearchContract.Items.Clear();
		
			// 콤보에 셋트
			this.cbSearchContract.Items.Add(new Janus.Windows.EditControls.UIComboBoxItem( rptData.ContractName, rptData.ContractSeq));
			this.cbSearchContract.SelectedIndex = 0;

			Application.DoEvents();
		}

		/// <summary>
		/// 계약에 해당하는 캠패인을 읽어온다.
		/// </summary>
		private void SetCampaign()
		{
			// 매체를 조회한다.
			CampaignCodeModel campaigncodeModel = new CampaignCodeModel();
			campaigncodeModel.MediaCode			= cbSearchMedia.SelectedValue.ToString();
			campaigncodeModel.ContractSeq			= cbSearchContract.SelectedValue.ToString();
			new CampaignCodeManager(oSystemModel, oCommonModel).GetCampaignCodeList(campaigncodeModel);
			
			if (campaigncodeModel.ResultCD.Equals("0000"))
			{
				// 데이터셋에 셋팅
				Utility.SetDataTable(rptHeaderDs.Campaign, campaigncodeModel.CampaignCodeDataSet);				
			}

			// 검색조건의 콤보
			this.cbCampaign.Items.Clear();
			
			// 콤보박스에 셋트할 코드목록을 담을 Item배열을 선언
			Janus.Windows.EditControls.UIComboBoxItem[] comboItems = new Janus.Windows.EditControls.UIComboBoxItem[campaigncodeModel.ResultCnt + 1];

			if( campaigncodeModel.ResultCnt == 0 )
			{
				comboItems[0] = new Janus.Windows.EditControls.UIComboBoxItem("캠페인 선택","00");
			}
			else
			{			
				comboItems[0] = new Janus.Windows.EditControls.UIComboBoxItem("[전체]","00");

				for(int i=0;i<campaigncodeModel.ResultCnt;i++)
				{
					DataRow row = rptHeaderDs.Campaign.Rows[i];

					string val = row["CampaignCode"].ToString();
					string txt = row["CampaignName"].ToString();
					comboItems[i+1] = new Janus.Windows.EditControls.UIComboBoxItem(txt,val);
				}
			}
			// 콤보에 셋트
			this.cbCampaign.Items.AddRange(comboItems);
			this.cbCampaign.SelectedIndex = 0;

			Application.DoEvents();
		}


		private void SetItem(string ContractSeq)
		{
			// 데이터모델 초기화
			summaryAdModel.Init();
			summaryAdModel.SearchContractSeq =  cbSearchContract.SelectedValue.ToString(); 
			summaryAdModel.CampaignCode		=  cbCampaign.SelectedValue.ToString(); 

			if( !summaryAdModel.SearchContractSeq.Equals("00") )
			{
				//  광고계약에 속한 광고내역 리스트 조회 서비스를 호출한다.
				new SummaryAdManager(oSystemModel,oCommonModel).GetContractItemList(summaryAdModel);

				if (summaryAdModel.ResultCD.Equals("0000"))
				{
					Utility.SetDataTable(rptHeaderDs.Items, summaryAdModel.ItemDataSet);				
				}
				// 검색조건의 콤보
				this.cbSearchItem.Items.Clear();
			
				// 콤보박스에 셋트할 코드목록을 담을 Item배열을 선언
				Janus.Windows.EditControls.UIComboBoxItem[] comboItems = new Janus.Windows.EditControls.UIComboBoxItem[summaryAdModel.ResultCnt + 1];

				comboItems[0] = new Janus.Windows.EditControls.UIComboBoxItem("[전체]","00");
			
				for(int i=0;i<summaryAdModel.ResultCnt;i++)
				{
					DataRow row = rptHeaderDs.Items.Rows[i];

					string val = row["ItemNo"].ToString();
					string txt = row["ItemName"].ToString();
					comboItems[i+1] = new Janus.Windows.EditControls.UIComboBoxItem(txt,val);
				}
				// 콤보에 셋트
				this.cbSearchItem.Items.AddRange(comboItems);
				if(summaryAdModel.ResultCnt > 0)
				{
					this.cbSearchItem.SelectedIndex = 1;
				}
				else
				{
					this.cbSearchItem.SelectedIndex = 0;
				}
			}

			Application.DoEvents();
		}

		private void cbCampaign_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			rptData.CampaignNo	= cbCampaign.SelectedItem.Value.ToString();
			rptData.CampaignName = cbCampaign.SelectedItem.Text.ToString();
			SetItem("");
		}

		private void cbSearchItem_SelectedItemChanged(object sender, System.EventArgs e)
		{
			rptData.ItemNo		=	cbSearchItem.SelectedItem.Value.ToString();
			rptData.ItemName	=	cbSearchItem.SelectedItem.Text.ToString();

			if(!rptData.ItemNo.Equals("00"))
			{
				// 광고내역을 선택한 경우
				DataRow[] rows = rptHeaderDs.Items.Select("ItemNo = '" + rptData.ItemNo + "' ");

				if(rows.Length > 0)
				{
					rptData.ItemBeginDay	= Utility.reConvertDate(rows[0]["EndDay"].ToString());    // 이벤트 처리를 위해 EndDay를 먼저셋트
					rptData.ItemEndDay	= Utility.reConvertDate(rows[0]["BeginDay"].ToString());

					if(rbSearchType_C.Checked)
					{
						if( !cbSearchStartDay.Enabled   )   cbSearchStartDay.Enabled = true;
						if( !cbSearchEndDay.Enabled     )   cbSearchEndDay.Enabled   = true;

						//if( rptData.ItemBeginDay.Length == 10 && rptData.ItemEndDay.Length == 10 )
						//{
						//	cbSearchStartDay.Value = FrameSystem.ConverStrTotDate( rptData.ItemBeginDay );
						//	cbSearchEndDay.Text = rptData.ItemEndDay;
						//}
						//else
						//{
						//	cbSearchStartDay.Value = DateTime.Now.AddDays(-1);
						//	cbSearchEndDay.Value  = cbSearchStartDay.Value;
						//}
					}	
				}
			}
			else
			{
				if(rbSearchType_C.Checked)
				{
					if( !cbSearchStartDay.Enabled   )   cbSearchStartDay.Enabled = true;
					if( !cbSearchEndDay.Enabled     )   cbSearchEndDay.Enabled   = true;
                
					// 계약일때
					if( rptData.ContBeginDay.Length == 10 && rptData.ContEndDay.Length == 10 )
					{
						cbSearchStartDay.Text= rptData.ContBeginDay;
						cbSearchEndDay.Text	= rptData.ContEndDay;
					}
					else
					{
						cbSearchStartDay.Value = DateTime.Now.AddDays(-1);
						cbSearchEndDay.Value  = cbSearchStartDay.Value;
					}
				}
			}
		}

		
		#region [ 날짜 관련 컨트롤 이벤트 처리 ]
		/// <summary>
		/// 집행실적을 기간으로 선택했을때
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void rbSearchType_C_CheckedChanged(object sender, System.EventArgs e)
		{
			if(rbSearchType_C.Checked)
			{
				if( !cbSearchStartDay.Enabled   )   cbSearchStartDay.Enabled = true;
				if( !cbSearchEndDay.Enabled     )   cbSearchEndDay.Enabled   = true;
			}	
		}


		/// <summary>
		/// 일간을 선택했을때
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void rbSearchType_D_CheckedChanged(object sender, System.EventArgs e)
		{
			if(rbSearchType_D.Checked)
			{
				cbSearchStartDay.Value = DateTime.Now.AddDays(-1);
				cbSearchEndDay.Value  = cbSearchStartDay.Value;
				cbSearchStartDay.Enabled = true;
				cbSearchEndDay.Enabled   = false;
			}			
		}

		private void rbSearchType_W_CheckedChanged(object sender, System.EventArgs e)
		{
			if(rbSearchType_W.Checked)
			{
				cbSearchStartDay.Enabled = true;
				cbSearchEndDay.Enabled   = false;

				DateTime dt = cbSearchStartDay.Value;

				for(int i=0;i<7;i++)
				{
					if(dt.DayOfWeek == System.DayOfWeek.Monday)
					{
						break;
					}
					dt = dt.AddDays(-1);
				}
				cbSearchStartDay.Value = dt;
				cbSearchEndDay.Value   = cbSearchStartDay.Value.AddDays(6);
			}			
		}


		private void rbSearchType_M_CheckedChanged(object sender, System.EventArgs e)
		{
			if(rbSearchType_M.Checked)
			{
				cbSearchStartDay.Enabled = true;
				cbSearchEndDay.Enabled   = false;

				cbSearchStartDay.Text = cbSearchStartDay.Value.ToString("yyyy-MM-01");
				cbSearchEndDay.Value  = cbSearchStartDay.Value.AddMonths(1).AddDays(-1);
			}			
		}

		private void cbSearchStartDay_TextChanged(object sender, System.EventArgs e)
		{
			if(rbSearchType_D.Checked)
			{
				cbSearchEndDay.Value  = cbSearchStartDay.Value;
			}
			else if(rbSearchType_W.Checked)
			{
				cbSearchEndDay.Value  = cbSearchStartDay.Value.AddDays(6);

			}
			else if(rbSearchType_M.Checked)
			{
				cbSearchEndDay.Value  = cbSearchStartDay.Value.AddMonths(1).AddDays(-1);
			}
		}

		private void cbSearchMedia_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			rptData.MediaCode	= cbSearchMedia.SelectedItem.Value.ToString();
			rptData.MediaName	= cbSearchMedia.SelectedItem.Text.ToString();

			Init_cbSearchContract();
			Init_cbCampaignCode();
			Init_cbSearchItem();
		}
		#endregion

		#region [ 버튼 엑션 이벤트 처리 ]
		public event SearchClickEventHandler	SearchClicked;
		/// <summary>
		/// 조회조건 사용자컨트롤에서 조회버튼을 클릭하면 발생합니다.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnSearch_Click(object sender, System.EventArgs e)
		{
			rptData.ItemBeginDay	= cbSearchStartDay.Value.ToString("yyMMdd");
			rptData.ItemEndDay	= cbSearchEndDay.Value.ToString("yyMMdd");

			if( SearchClicked != null )	SearchClicked(this, rptData );
		}


		public event ExcelClickEventHandler		ExcelClicked;
		/// <summary>
		/// 조회조건 사용자컨트롤에서 엑셀버튼을 클릭하면 발생합니다.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnExcel_Click(object sender, System.EventArgs e)
		{
			if( ExcelClicked != null )	ExcelClicked( this, null);
		}
		#endregion
	}
}
