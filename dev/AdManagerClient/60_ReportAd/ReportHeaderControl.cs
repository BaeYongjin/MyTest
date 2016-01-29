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
	/// ReportHeader1�� ���� ��� �����Դϴ�.
	/// </summary>
    public class ReportHeaderControl : System.Windows.Forms.UserControl
	{
		#region [ ��������� ��ü �� ���� ]
		// �ý��� ���� : ȭ�����
		private	SystemModel	oSystemModel	=	FrameSystem.oSysModel;
		private	CommonModel	oCommonModel	=	FrameSystem.oComModel;
		private	Logger		oLog				=	FrameSystem.oLog;
		private	MenuPower	oMenu				=	FrameSystem.oMenu;

		// �޴��ڵ� : ������ �ʿ��� ȭ�鿡 �ʿ���
		private	string	MenuCode			= "";
		private	bool		canRead			= false;
		private	bool		canPrint			= false;

		private	SearchReportData	rptData;
		private	SummaryAdModel summaryAdModel = new SummaryAdModel();	// �Ѱ������

		#endregion

		#region [ ȭ�� UI���� ������Ʈ ���� ]
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
		/// �ʼ� �����̳� �����Դϴ�.
		/// </summary>
		private System.ComponentModel.Container components = null;
		#endregion
	
		#region[  �� �� �� ] 
		/// <summary>
		/// �������� �����ϰų�, �����ɴϴ�.
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
		/// ����Ʈ�� ���ɿ��θ� �����ϰų�, �����ɴϴ�.
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
		/// ��ȸ ���ڱ����� �ؽ�Ʈ�� �����ɴϴ�(�Ⱓ/�ϰ�/�ְ�/����)
		/// </summary>
		public string u_DayType
		{
			get
			{
				string rtn = "";
				if( rbSearchType_C.Checked )			rtn = "�Ⱓ";
				else if( rbSearchType_D.Checked )	rtn = "�ϰ�";
				else if( rbSearchType_W.Checked )	rtn = "�ְ�";
				else if( rbSearchType_M.Checked )	rtn = "����";
				else											rtn = "����";

				return rtn;
			
			}
		}
		#endregion

		public ReportHeaderControl()
		{
			// �� ȣ���� Windows.Forms Form �����̳ʿ� �ʿ��մϴ�.
			InitializeComponent();
		}

		/// <summary> 
		/// ��� ���� ��� ���ҽ��� �����մϴ�.
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


		#region ���� ��� �����̳ʿ��� ������ �ڵ�
		/// <summary> 
		/// �����̳� ������ �ʿ��� �޼����Դϴ�. 
		/// �� �޼����� ������ �ڵ� ������� �������� ���ʽÿ�.
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
            this.pnlSearch.Font = new System.Drawing.Font("���� ���", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
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
            this.label5.Text = "�����/������";
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
            this.lblContractDate.Text = "�������";
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
            this.label4.Text = "��������";
            this.label4.TextAlign = System.Drawing.ContentAlignment.BottomRight;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(80, 31);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(44, 21);
            this.label2.TabIndex = 183;
            this.label2.Text = "ķ����";
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
            this.cbCampaign.Text = "ķ���μ���";
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
            this.rbSearchType_W.Text = "�ְ�";
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
            this.rbSearchType_C.Text = "�Ⱓ";
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
            this.rbSearchType_M.Text = "����";
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
            this.rbSearchType_D.Text = "�ϰ�";
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
            this.lbSearchDate.Text = "��������";
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
            this.cbSearchMedia.Text = "��ü����";
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
            this.cbSearchContract.Text = "�����༱��";
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
            this.btnSearch.Text = "�� ȸ";
            this.btnSearch.ToolTipText = "������ ���Ǵ�� �����մϴ�";
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
            this.btnExcel.Text = "EXCEL ���";
            this.btnExcel.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.btnExcel.Click += new System.EventHandler(this.btnExcel_Click);
            // 
            // label6
            // 
            this.label6.Location = new System.Drawing.Point(392, 10);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(68, 21);
            this.label6.TabIndex = 14;
            this.label6.Text = "�Ⱓ����";
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
            this.cbSearchItem.Text = "������";
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

		#region [ UI ���� �̺�Ʈ ó���� ]
		/// <summary>
		/// ��Ʈ�� Load�̺�Ʈ ó����
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void ReportHeader1_Load(object sender, System.EventArgs e)
		{
			// ��Ʈ�� �ʱ�ȭ
			//InitControl();		
		}
		#endregion

		#region [ ��Ʈ�� �ʱ�ȭ ]
		/// <summary>
		/// ��Ʈ�ѵ��� �ʱ�ȭ ��Ų��.
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
		/// �޺��ڽ� �ʱ�ȭ�� ��Ÿ ��Ʈ�� �ʱ�ȭ
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
		/// ��ü�ڵ� �ʱ�ȭ �� ������ �ε�
		/// </summary>
		private void Init_MediaCode()
		{
			// ��ü�� ��ȸ�Ѵ�.
			MediaCodeModel mediacodeModel = new MediaCodeModel();
			new MediaCodeManager(oSystemModel, oCommonModel).GetMediaCodeList(mediacodeModel);
			
			if (mediacodeModel.ResultCD.Equals("0000"))
			{
				Utility.SetDataTable( rptHeaderDs.Media, mediacodeModel.MediaCodeDataSet);				
			}

			// �˻������� �޺�
			this.cbSearchMedia.Items.Clear();
			
			// �޺��ڽ��� ��Ʈ�� �ڵ����� ���� Item�迭�� ����
			Janus.Windows.EditControls.UIComboBoxItem[] comboItems = new Janus.Windows.EditControls.UIComboBoxItem[mediacodeModel.ResultCnt + 1];

			comboItems[0] = new Janus.Windows.EditControls.UIComboBoxItem("�� ��ü���� ��","00");
			
			for(int i=0;i<mediacodeModel.ResultCnt;i++)
			{
				DataRow row = rptHeaderDs.Media.Rows[i];

				string	val = row["MediaCode"].ToString();
				string	txt = row["MediaName"].ToString();
				comboItems[i+1] = new Janus.Windows.EditControls.UIComboBoxItem(txt,val);
			}
			// �޺��� ��Ʈ
			this.cbSearchMedia.Items.AddRange(comboItems);
			this.cbSearchMedia.SelectedIndex = 0;

			Application.DoEvents();
		}


		/// <summary>
		/// ��൥���� �о����
		/// </summary>
		private void Init_cbSearchContract()
		{

			// �˻������� �޺�
			this.cbSearchContract.Items.Clear();
			
			// �޺��� ��Ʈ
			this.cbSearchContract.Items.Add(new Janus.Windows.EditControls.UIComboBoxItem("��� ����","00"));
			this.cbSearchContract.SelectedIndex = 0;

			Application.DoEvents();
		}


		/// <summary>
		/// ķ�����޺� �ʱ�ȭ
		/// </summary>
		private void Init_cbCampaignCode()
		{
			// �˻������� �޺�
			this.cbCampaign.Items.Clear();

			// �޺��� ����
			this.cbCampaign.Items.Add(new Janus.Windows.EditControls.UIComboBoxItem("ķ���� ����","00"));
			this.cbCampaign.SelectedIndex = 0;
			Application.DoEvents();
		}


		/// <summary>
		/// �����޺� �ʱ�ȭ
		/// </summary>
		private void Init_cbSearchItem()
		{
			// �˻������� �޺�
			this.cbSearchItem.Items.Clear();
			this.cbSearchItem.Items.Add(new Janus.Windows.EditControls.UIComboBoxItem("���� ����","00"));
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
						cbSearchMedia.SelectedValue = FrameSystem._HANATV; // �ϳ�TV�� �⺻������ �Ѵ�.	 		
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

		#region [ �̺�Ʈ �ڵ鷯 �� �Լ� ]
		public event StatusEventHandler 			StatusEvent;			// �����̺�Ʈ �ڵ鷯
		private void StatusMessage(string strMessage)
		{
			if (StatusEvent != null) 
			{
				StatusEventArgs ea = new StatusEventArgs();
				ea.Message   = strMessage;
				StatusEvent(this,ea);
			}
		}

		public event ProgressEventHandler 			ProgressEvent;			// ó�����̺�Ʈ �ڵ鷯
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
		/// ��Ʈ�ѵ��� �ʱ�ȭ ��Ų��, ��ü�ڵ带 �о�� �����մϴ�.
		/// </summary>
		public void u_InitControl()
		{
			InitControl();
		}

		/// <summary>
		/// ����� Ŭ�������� ó��
		/// ��� �˾��� ����� ����� �����Ѵ�
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void cbSearchContract_Click(object sender, System.EventArgs e)
		{
			if(cbSearchMedia.SelectedValue.Equals("00"))
			{
				MessageBox.Show("��ü�� ������ �ֽʽÿ�.", "����ý���",MessageBoxButtons.OK, MessageBoxIcon.Information);
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
		/// ���˻� �����쿡�� ��༱���̺�Ʈ�� ó���ϴ� �Լ�
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

			//contractEndDay   = Utility.reConvertDate(EndDay);	// �̺�Ʈ ó���� ���� EndDay�� ���� ��Ʈ�Ѵ�.
			//contractStartDay = Utility.reConvertDate(StartDay);
			ebAgency.Text     = rptData.AgencyName;
			ebAdvertiser.Text = rptData.AdvertiserName;

			//summaryAdDs.Report.Clear();

			canPrint = false;
			InitButton();
		}

		/// <summary>
		/// ����޺��� �о�� ���� ����
		/// </summary>
		private void SetContract()
		{
			// �˻������� �޺�
			this.cbSearchContract.Items.Clear();
		
			// �޺��� ��Ʈ
			this.cbSearchContract.Items.Add(new Janus.Windows.EditControls.UIComboBoxItem( rptData.ContractName, rptData.ContractSeq));
			this.cbSearchContract.SelectedIndex = 0;

			Application.DoEvents();
		}

		/// <summary>
		/// ��࿡ �ش��ϴ� ķ������ �о�´�.
		/// </summary>
		private void SetCampaign()
		{
			// ��ü�� ��ȸ�Ѵ�.
			CampaignCodeModel campaigncodeModel = new CampaignCodeModel();
			campaigncodeModel.MediaCode			= cbSearchMedia.SelectedValue.ToString();
			campaigncodeModel.ContractSeq			= cbSearchContract.SelectedValue.ToString();
			new CampaignCodeManager(oSystemModel, oCommonModel).GetCampaignCodeList(campaigncodeModel);
			
			if (campaigncodeModel.ResultCD.Equals("0000"))
			{
				// �����ͼ¿� ����
				Utility.SetDataTable(rptHeaderDs.Campaign, campaigncodeModel.CampaignCodeDataSet);				
			}

			// �˻������� �޺�
			this.cbCampaign.Items.Clear();
			
			// �޺��ڽ��� ��Ʈ�� �ڵ����� ���� Item�迭�� ����
			Janus.Windows.EditControls.UIComboBoxItem[] comboItems = new Janus.Windows.EditControls.UIComboBoxItem[campaigncodeModel.ResultCnt + 1];

			if( campaigncodeModel.ResultCnt == 0 )
			{
				comboItems[0] = new Janus.Windows.EditControls.UIComboBoxItem("ķ���� ����","00");
			}
			else
			{			
				comboItems[0] = new Janus.Windows.EditControls.UIComboBoxItem("[��ü]","00");

				for(int i=0;i<campaigncodeModel.ResultCnt;i++)
				{
					DataRow row = rptHeaderDs.Campaign.Rows[i];

					string val = row["CampaignCode"].ToString();
					string txt = row["CampaignName"].ToString();
					comboItems[i+1] = new Janus.Windows.EditControls.UIComboBoxItem(txt,val);
				}
			}
			// �޺��� ��Ʈ
			this.cbCampaign.Items.AddRange(comboItems);
			this.cbCampaign.SelectedIndex = 0;

			Application.DoEvents();
		}


		private void SetItem(string ContractSeq)
		{
			// �����͸� �ʱ�ȭ
			summaryAdModel.Init();
			summaryAdModel.SearchContractSeq =  cbSearchContract.SelectedValue.ToString(); 
			summaryAdModel.CampaignCode		=  cbCampaign.SelectedValue.ToString(); 

			if( !summaryAdModel.SearchContractSeq.Equals("00") )
			{
				//  �����࿡ ���� ������ ����Ʈ ��ȸ ���񽺸� ȣ���Ѵ�.
				new SummaryAdManager(oSystemModel,oCommonModel).GetContractItemList(summaryAdModel);

				if (summaryAdModel.ResultCD.Equals("0000"))
				{
					Utility.SetDataTable(rptHeaderDs.Items, summaryAdModel.ItemDataSet);				
				}
				// �˻������� �޺�
				this.cbSearchItem.Items.Clear();
			
				// �޺��ڽ��� ��Ʈ�� �ڵ����� ���� Item�迭�� ����
				Janus.Windows.EditControls.UIComboBoxItem[] comboItems = new Janus.Windows.EditControls.UIComboBoxItem[summaryAdModel.ResultCnt + 1];

				comboItems[0] = new Janus.Windows.EditControls.UIComboBoxItem("[��ü]","00");
			
				for(int i=0;i<summaryAdModel.ResultCnt;i++)
				{
					DataRow row = rptHeaderDs.Items.Rows[i];

					string val = row["ItemNo"].ToString();
					string txt = row["ItemName"].ToString();
					comboItems[i+1] = new Janus.Windows.EditControls.UIComboBoxItem(txt,val);
				}
				// �޺��� ��Ʈ
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
				// �������� ������ ���
				DataRow[] rows = rptHeaderDs.Items.Select("ItemNo = '" + rptData.ItemNo + "' ");

				if(rows.Length > 0)
				{
					rptData.ItemBeginDay	= Utility.reConvertDate(rows[0]["EndDay"].ToString());    // �̺�Ʈ ó���� ���� EndDay�� ������Ʈ
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
                
					// ����϶�
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

		
		#region [ ��¥ ���� ��Ʈ�� �̺�Ʈ ó�� ]
		/// <summary>
		/// ��������� �Ⱓ���� ����������
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
		/// �ϰ��� ����������
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

		#region [ ��ư ���� �̺�Ʈ ó�� ]
		public event SearchClickEventHandler	SearchClicked;
		/// <summary>
		/// ��ȸ���� �������Ʈ�ѿ��� ��ȸ��ư�� Ŭ���ϸ� �߻��մϴ�.
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
		/// ��ȸ���� �������Ʈ�ѿ��� ������ư�� Ŭ���ϸ� �߻��մϴ�.
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
