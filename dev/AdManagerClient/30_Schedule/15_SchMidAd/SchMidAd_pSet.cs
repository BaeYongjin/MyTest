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

namespace AdManagerClient.Schedule
{
	/// <summary>
	/// SchMidAd_pSet에 대한 요약 설명입니다.
	/// </summary>
	public class SchMidAd_pSet : System.Windows.Forms.Form
	{

        #region 사용자정의 객체 및 변수

        // 시스템 정보 : 화면공통
        private SystemModel   systemModel   = FrameSystem.oSysModel;
        private CommonModel   commonModel   = FrameSystem.oComModel;
        private Logger        log           = FrameSystem.oLog;

        // 사용할 정보모델
        ChooseAdScheduleModel chooseAdScheduleModel = new ChooseAdScheduleModel();

        // 화면처리용 변수
        CurrencyManager cm        = null;					// 데이터 그리드의 변경에 따른 데이터셋 관리를 위하여			
        DataTable       dt        = null;

        // 메뉴/채널정보
        public string keyMediaCode    = "";
        public string keyCategoryCode = "";
        public string keyGenreCode    = "";
        public string keyChannelNo    = "";

        #endregion


        private System.Windows.Forms.Panel panelTop;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel3;
        private AdManagerClient.SchMidAdDs schMidAdDs;
        private Janus.Windows.GridEX.GridEX gridExList;
        private System.Data.DataView dvSeriesList;
        private System.Windows.Forms.Label lblChannelNo;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblSeries;
        private Janus.Windows.GridEX.EditControls.EditBox editBox1;
        private Janus.Windows.GridEX.EditControls.NumericEditBox numericEditBox1;
        private Janus.Windows.GridEX.EditControls.IntegerUpDown integerUpDown1;
        private Janus.Windows.EditControls.UIGroupBox uiGroupBox1;
        private Janus.Windows.EditControls.UIRadioButton uiRadioButton3;
        private Janus.Windows.EditControls.UIRadioButton uiRadioButton4;
        private Janus.Windows.GridEX.EditControls.NumericEditBox numericEditBox2;
        private System.Windows.Forms.Label label3;
        private Janus.Windows.EditControls.UIGroupBox uiGroupBox2;
        private System.Windows.Forms.Label label4;
        private Janus.Windows.GridEX.EditControls.NumericEditBox numericEditBox3;
        private Janus.Windows.EditControls.UIRadioButton uiRadioButton5;
        private Janus.Windows.EditControls.UIRadioButton uiRadioButton6;
        private System.Windows.Forms.Label label5;
        private Janus.Windows.EditControls.UIGroupBox uiGroupBox3;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private Janus.Windows.EditControls.UIRadioButton uiRadioButton7;
        private Janus.Windows.EditControls.UIRadioButton uiRadioButton8;
        private Janus.Windows.GridEX.EditControls.EditBox editBox2;
        private NobleTech.Form.FormHeader.ColorSlideFormHeader FormHeader;
        private Janus.Windows.EditControls.UIButton btnClose;
		/// <summary>
		/// 필수 디자이너 변수입니다.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public SchMidAd_pSet()
		{
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
            Janus.Windows.GridEX.GridEXLayout gridExList_Layout_0 = new Janus.Windows.GridEX.GridEXLayout();
            Janus.Windows.Common.Layouts.JanusLayoutReference gridExList_Layout_0_Reference_0 = new Janus.Windows.Common.Layouts.JanusLayoutReference("GridEXLayoutData.RootTable.FormatConditions.Condition0.FormatStyle.BackgroundImag" +
                    "e");
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SchMidAd_pSet));
            Janus.Windows.Common.Layouts.JanusLayoutReference gridExList_Layout_0_Reference_1 = new Janus.Windows.Common.Layouts.JanusLayoutReference("GridEXLayoutData.RootTable.FormatConditions.Condition1.FormatStyle.BackgroundImag" +
                    "e");
            this.dvSeriesList = new System.Data.DataView();
            this.schMidAdDs = new AdManagerClient.SchMidAdDs();
            this.panelTop = new System.Windows.Forms.Panel();
            this.btnClose = new Janus.Windows.EditControls.UIButton();
            this.lblChannelNo = new System.Windows.Forms.Label();
            this.lblTitle = new System.Windows.Forms.Label();
            this.FormHeader = new NobleTech.Form.FormHeader.ColorSlideFormHeader();
            this.panel1 = new System.Windows.Forms.Panel();
            this.gridExList = new Janus.Windows.GridEX.GridEX();
            this.panel2 = new System.Windows.Forms.Panel();
            this.editBox2 = new Janus.Windows.GridEX.EditControls.EditBox();
            this.uiGroupBox3 = new Janus.Windows.EditControls.UIGroupBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.uiRadioButton7 = new Janus.Windows.EditControls.UIRadioButton();
            this.uiRadioButton8 = new Janus.Windows.EditControls.UIRadioButton();
            this.numericEditBox1 = new Janus.Windows.GridEX.EditControls.NumericEditBox();
            this.integerUpDown1 = new Janus.Windows.GridEX.EditControls.IntegerUpDown();
            this.uiGroupBox2 = new Janus.Windows.EditControls.UIGroupBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.numericEditBox3 = new Janus.Windows.GridEX.EditControls.NumericEditBox();
            this.uiRadioButton5 = new Janus.Windows.EditControls.UIRadioButton();
            this.uiRadioButton6 = new Janus.Windows.EditControls.UIRadioButton();
            this.uiGroupBox1 = new Janus.Windows.EditControls.UIGroupBox();
            this.label3 = new System.Windows.Forms.Label();
            this.numericEditBox2 = new Janus.Windows.GridEX.EditControls.NumericEditBox();
            this.uiRadioButton3 = new Janus.Windows.EditControls.UIRadioButton();
            this.uiRadioButton4 = new Janus.Windows.EditControls.UIRadioButton();
            this.editBox1 = new Janus.Windows.GridEX.EditControls.EditBox();
            this.lblSeries = new System.Windows.Forms.Label();
            this.panel3 = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.dvSeriesList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.schMidAdDs)).BeginInit();
            this.panelTop.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridExList)).BeginInit();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uiGroupBox3)).BeginInit();
            this.uiGroupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uiGroupBox2)).BeginInit();
            this.uiGroupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uiGroupBox1)).BeginInit();
            this.uiGroupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // dvSeriesList
            // 
            this.dvSeriesList.Table = this.schMidAdDs.SeriesList;
            // 
            // schMidAdDs
            // 
            this.schMidAdDs.DataSetName = "SchMidAdDs";
            this.schMidAdDs.Locale = new System.Globalization.CultureInfo("en-US");
            this.schMidAdDs.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // panelTop
            // 
            this.panelTop.BackColor = System.Drawing.Color.White;
            this.panelTop.Controls.Add(this.btnClose);
            this.panelTop.Controls.Add(this.lblChannelNo);
            this.panelTop.Controls.Add(this.lblTitle);
            this.panelTop.Controls.Add(this.FormHeader);
            this.panelTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelTop.Location = new System.Drawing.Point(2, 2);
            this.panelTop.Name = "panelTop";
            this.panelTop.Size = new System.Drawing.Size(790, 40);
            this.panelTop.TabIndex = 1;
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(684, 8);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(90, 24);
            this.btnClose.TabIndex = 3;
            this.btnClose.Text = "닫 기";
            this.btnClose.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // lblChannelNo
            // 
            this.lblChannelNo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblChannelNo.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblChannelNo.Location = new System.Drawing.Point(78, 10);
            this.lblChannelNo.Name = "lblChannelNo";
            this.lblChannelNo.Size = new System.Drawing.Size(50, 22);
            this.lblChannelNo.TabIndex = 0;
            this.lblChannelNo.Text = "30257";
            this.lblChannelNo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblTitle
            // 
            this.lblTitle.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblTitle.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblTitle.Location = new System.Drawing.Point(132, 10);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(250, 22);
            this.lblTitle.TabIndex = 1;
            this.lblTitle.Text = "안녕 프란체스카";
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // FormHeader
            // 
            //this.FormHeader.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            //            | System.Windows.Forms.AnchorStyles.Right)));
            this.FormHeader.BoundrySize = 15;
            this.FormHeader.Color1 = System.Drawing.Color.DeepSkyBlue;
            this.FormHeader.Color2 = System.Drawing.Color.White;
            this.FormHeader.Dock = System.Windows.Forms.DockStyle.Fill;
            this.FormHeader.Font = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.FormHeader.Icon = null;
            this.FormHeader.Image = null;
            this.FormHeader.Location = new System.Drawing.Point(0, 0);
            this.FormHeader.Message = "";
            this.FormHeader.MessageFontStyle = System.Drawing.FontStyle.Regular;
            this.FormHeader.Name = "FormHeader";
            this.FormHeader.Size = new System.Drawing.Size(790, 40);
            this.FormHeader.TabIndex = 0;
            this.FormHeader.TextStartPosition = new System.Drawing.Point(15, 10);
            this.FormHeader.Title = "선택채널";
            this.FormHeader.TitleFontStyle = System.Drawing.FontStyle.Bold;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.gridExList);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.Location = new System.Drawing.Point(2, 42);
            this.panel1.Name = "panel1";
            this.panel1.Padding = new System.Windows.Forms.Padding(2);
            this.panel1.Size = new System.Drawing.Size(436, 536);
            this.panel1.TabIndex = 2;
            // 
            // gridExList
            // 
            this.gridExList.AlternatingColors = true;
            this.gridExList.BorderStyle = Janus.Windows.GridEX.BorderStyle.Flat;
            this.gridExList.DataSource = this.dvSeriesList;
            this.gridExList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridExList.EmptyRows = true;
            this.gridExList.FocusCellFormatStyle.BackColor = System.Drawing.SystemColors.Highlight;
            this.gridExList.FocusCellFormatStyle.ForeColor = System.Drawing.Color.White;
            this.gridExList.FocusStyle = Janus.Windows.GridEX.FocusStyle.None;
            this.gridExList.Font = new System.Drawing.Font("맑은 고딕", 8.25F);
            this.gridExList.GridLines = Janus.Windows.GridEX.GridLines.Vertical;
            this.gridExList.GridLineStyle = Janus.Windows.GridEX.GridLineStyle.Solid;
            this.gridExList.GroupByBoxVisible = false;
            this.gridExList.GroupMode = Janus.Windows.GridEX.GroupMode.Collapsed;
            this.gridExList.HeaderFormatStyle.TextAlignment = Janus.Windows.GridEX.TextAlignment.Center;
            gridExList_Layout_0.DataSource = this.dvSeriesList;
            gridExList_Layout_0.IsCurrentLayout = true;
            gridExList_Layout_0.Key = "bae";
            gridExList_Layout_0_Reference_0.Instance = ((object)(resources.GetObject("gridExList_Layout_0_Reference_0.Instance")));
            gridExList_Layout_0_Reference_1.Instance = ((object)(resources.GetObject("gridExList_Layout_0_Reference_1.Instance")));
            gridExList_Layout_0.LayoutReferences.AddRange(new Janus.Windows.Common.Layouts.JanusLayoutReference[] {
            gridExList_Layout_0_Reference_0,
            gridExList_Layout_0_Reference_1});
            gridExList_Layout_0.LayoutString = resources.GetString("gridExList_Layout_0.LayoutString");
            this.gridExList.Layouts.AddRange(new Janus.Windows.GridEX.GridEXLayout[] {
            gridExList_Layout_0});
            this.gridExList.Location = new System.Drawing.Point(2, 2);
            this.gridExList.Name = "gridExList";
            this.gridExList.ScrollBars = Janus.Windows.GridEX.ScrollBars.Vertical;
            this.gridExList.Size = new System.Drawing.Size(432, 532);
            this.gridExList.TabIndex = 8;
            this.gridExList.TabKeyBehavior = Janus.Windows.GridEX.TabKeyBehavior.ControlNavigation;
            this.gridExList.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.White;
            this.panel2.Controls.Add(this.editBox2);
            this.panel2.Controls.Add(this.uiGroupBox3);
            this.panel2.Controls.Add(this.uiGroupBox2);
            this.panel2.Controls.Add(this.uiGroupBox1);
            this.panel2.Controls.Add(this.editBox1);
            this.panel2.Controls.Add(this.lblSeries);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(438, 42);
            this.panel2.Name = "panel2";
            this.panel2.Padding = new System.Windows.Forms.Padding(2);
            this.panel2.Size = new System.Drawing.Size(354, 536);
            this.panel2.TabIndex = 3;
            // 
            // editBox2
            // 
            this.editBox2.Location = new System.Drawing.Point(136, 32);
            this.editBox2.Name = "editBox2";
            this.editBox2.Size = new System.Drawing.Size(192, 20);
            this.editBox2.TabIndex = 13;
            this.editBox2.TextAlignment = Janus.Windows.GridEX.TextAlignment.Center;
            this.editBox2.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            // 
            // uiGroupBox3
            // 
            this.uiGroupBox3.Controls.Add(this.label6);
            this.uiGroupBox3.Controls.Add(this.label7);
            this.uiGroupBox3.Controls.Add(this.uiRadioButton7);
            this.uiGroupBox3.Controls.Add(this.uiRadioButton8);
            this.uiGroupBox3.Controls.Add(this.numericEditBox1);
            this.uiGroupBox3.Controls.Add(this.integerUpDown1);
            this.uiGroupBox3.Location = new System.Drawing.Point(28, 64);
            this.uiGroupBox3.Name = "uiGroupBox3";
            this.uiGroupBox3.Size = new System.Drawing.Size(306, 116);
            this.uiGroupBox3.TabIndex = 12;
            this.uiGroupBox3.Text = "중간광고";
            // 
            // label6
            // 
            this.label6.Font = new System.Drawing.Font("맑은 고딕", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label6.ForeColor = System.Drawing.Color.SteelBlue;
            this.label6.Location = new System.Drawing.Point(20, 94);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(274, 18);
            this.label6.TabIndex = 10;
            this.label6.Text = "광고위치를 기준으로 설정합니다";
            // 
            // label7
            // 
            this.label7.Font = new System.Drawing.Font("맑은 고딕", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label7.ForeColor = System.Drawing.Color.SteelBlue;
            this.label7.Location = new System.Drawing.Point(20, 80);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(274, 18);
            this.label7.TabIndex = 9;
            this.label7.Text = "광고종료후 본편재생시 되감기할 위치를 설정합니다";
            // 
            // uiRadioButton7
            // 
            this.uiRadioButton7.AutoCheck = false;
            this.uiRadioButton7.Location = new System.Drawing.Point(34, 46);
            this.uiRadioButton7.Name = "uiRadioButton7";
            this.uiRadioButton7.Size = new System.Drawing.Size(76, 23);
            this.uiRadioButton7.TabIndex = 7;
            this.uiRadioButton7.Text = "퍼센트(%)";
            this.uiRadioButton7.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            // 
            // uiRadioButton8
            // 
            this.uiRadioButton8.AutoCheck = false;
            this.uiRadioButton8.Checked = true;
            this.uiRadioButton8.Location = new System.Drawing.Point(34, 20);
            this.uiRadioButton8.Name = "uiRadioButton8";
            this.uiRadioButton8.Size = new System.Drawing.Size(74, 23);
            this.uiRadioButton8.TabIndex = 6;
            this.uiRadioButton8.TabStop = true;
            this.uiRadioButton8.Text = "바이트(B)";
            this.uiRadioButton8.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            // 
            // numericEditBox1
            // 
            this.numericEditBox1.EditMode = Janus.Windows.GridEX.NumericEditMode.Value;
            this.numericEditBox1.Location = new System.Drawing.Point(114, 46);
            this.numericEditBox1.Name = "numericEditBox1";
            this.numericEditBox1.Size = new System.Drawing.Size(118, 23);
            this.numericEditBox1.TabIndex = 7;
            this.numericEditBox1.Text = "314,572,800";
            this.numericEditBox1.TextAlignment = Janus.Windows.GridEX.TextAlignment.Far;
            this.numericEditBox1.Value = ((long)(314572800));
            this.numericEditBox1.ValueType = Janus.Windows.GridEX.NumericEditValueType.Int64;
            this.numericEditBox1.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            // 
            // integerUpDown1
            // 
            this.integerUpDown1.Location = new System.Drawing.Point(114, 20);
            this.integerUpDown1.Name = "integerUpDown1";
            this.integerUpDown1.Size = new System.Drawing.Size(78, 23);
            this.integerUpDown1.TabIndex = 8;
            this.integerUpDown1.TextAlignment = Janus.Windows.GridEX.TextAlignment.Far;
            this.integerUpDown1.Value = 50;
            this.integerUpDown1.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            // 
            // uiGroupBox2
            // 
            this.uiGroupBox2.Controls.Add(this.label5);
            this.uiGroupBox2.Controls.Add(this.label4);
            this.uiGroupBox2.Controls.Add(this.numericEditBox3);
            this.uiGroupBox2.Controls.Add(this.uiRadioButton5);
            this.uiGroupBox2.Controls.Add(this.uiRadioButton6);
            this.uiGroupBox2.Location = new System.Drawing.Point(28, 302);
            this.uiGroupBox2.Name = "uiGroupBox2";
            this.uiGroupBox2.Size = new System.Drawing.Size(306, 106);
            this.uiGroupBox2.TabIndex = 11;
            this.uiGroupBox2.Text = "후행정보";
            // 
            // label5
            // 
            this.label5.Font = new System.Drawing.Font("맑은 고딕", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label5.ForeColor = System.Drawing.Color.SteelBlue;
            this.label5.Location = new System.Drawing.Point(20, 84);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(274, 18);
            this.label5.TabIndex = 10;
            this.label5.Text = "광고위치를 기준으로 설정합니다";
            // 
            // label4
            // 
            this.label4.Font = new System.Drawing.Font("맑은 고딕", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label4.ForeColor = System.Drawing.Color.SteelBlue;
            this.label4.Location = new System.Drawing.Point(20, 70);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(274, 18);
            this.label4.TabIndex = 9;
            this.label4.Text = "광고종료후 본편재생시 되감기할 위치를 설정합니다";
            // 
            // numericEditBox3
            // 
            this.numericEditBox3.EditMode = Janus.Windows.GridEX.NumericEditMode.Value;
            this.numericEditBox3.Location = new System.Drawing.Point(96, 32);
            this.numericEditBox3.Name = "numericEditBox3";
            this.numericEditBox3.Size = new System.Drawing.Size(90, 23);
            this.numericEditBox3.TabIndex = 8;
            this.numericEditBox3.Text = "10,000";
            this.numericEditBox3.TextAlignment = Janus.Windows.GridEX.TextAlignment.Far;
            this.numericEditBox3.Value = ((long)(10000));
            this.numericEditBox3.ValueType = Janus.Windows.GridEX.NumericEditValueType.Int64;
            this.numericEditBox3.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            // 
            // uiRadioButton5
            // 
            this.uiRadioButton5.AutoCheck = false;
            this.uiRadioButton5.Location = new System.Drawing.Point(34, 42);
            this.uiRadioButton5.Name = "uiRadioButton5";
            this.uiRadioButton5.Size = new System.Drawing.Size(40, 23);
            this.uiRadioButton5.TabIndex = 7;
            this.uiRadioButton5.Text = "KB";
            this.uiRadioButton5.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            // 
            // uiRadioButton6
            // 
            this.uiRadioButton6.AutoCheck = false;
            this.uiRadioButton6.Checked = true;
            this.uiRadioButton6.Location = new System.Drawing.Point(34, 20);
            this.uiRadioButton6.Name = "uiRadioButton6";
            this.uiRadioButton6.Size = new System.Drawing.Size(40, 23);
            this.uiRadioButton6.TabIndex = 6;
            this.uiRadioButton6.TabStop = true;
            this.uiRadioButton6.Text = "MB";
            this.uiRadioButton6.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            // 
            // uiGroupBox1
            // 
            this.uiGroupBox1.Controls.Add(this.label3);
            this.uiGroupBox1.Controls.Add(this.numericEditBox2);
            this.uiGroupBox1.Controls.Add(this.uiRadioButton3);
            this.uiGroupBox1.Controls.Add(this.uiRadioButton4);
            this.uiGroupBox1.Location = new System.Drawing.Point(28, 188);
            this.uiGroupBox1.Name = "uiGroupBox1";
            this.uiGroupBox1.Size = new System.Drawing.Size(306, 106);
            this.uiGroupBox1.TabIndex = 10;
            this.uiGroupBox1.Text = "선행정보";
            // 
            // label3
            // 
            this.label3.Font = new System.Drawing.Font("맑은 고딕", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label3.ForeColor = System.Drawing.Color.SteelBlue;
            this.label3.Location = new System.Drawing.Point(20, 70);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(274, 34);
            this.label3.TabIndex = 9;
            this.label3.Text = "중간광고 안내 아이콘이 노출되는 위치를 설정합니다광고위치를 기준으로 설정합니다";
            // 
            // numericEditBox2
            // 
            this.numericEditBox2.EditMode = Janus.Windows.GridEX.NumericEditMode.Value;
            this.numericEditBox2.Location = new System.Drawing.Point(96, 32);
            this.numericEditBox2.Name = "numericEditBox2";
            this.numericEditBox2.Size = new System.Drawing.Size(90, 23);
            this.numericEditBox2.TabIndex = 8;
            this.numericEditBox2.Text = "10,000";
            this.numericEditBox2.TextAlignment = Janus.Windows.GridEX.TextAlignment.Far;
            this.numericEditBox2.Value = ((long)(10000));
            this.numericEditBox2.ValueType = Janus.Windows.GridEX.NumericEditValueType.Int64;
            this.numericEditBox2.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            // 
            // uiRadioButton3
            // 
            this.uiRadioButton3.AutoCheck = false;
            this.uiRadioButton3.Location = new System.Drawing.Point(34, 42);
            this.uiRadioButton3.Name = "uiRadioButton3";
            this.uiRadioButton3.Size = new System.Drawing.Size(40, 23);
            this.uiRadioButton3.TabIndex = 7;
            this.uiRadioButton3.Text = "KB";
            this.uiRadioButton3.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            // 
            // uiRadioButton4
            // 
            this.uiRadioButton4.AutoCheck = false;
            this.uiRadioButton4.Checked = true;
            this.uiRadioButton4.Location = new System.Drawing.Point(34, 20);
            this.uiRadioButton4.Name = "uiRadioButton4";
            this.uiRadioButton4.Size = new System.Drawing.Size(40, 23);
            this.uiRadioButton4.TabIndex = 6;
            this.uiRadioButton4.TabStop = true;
            this.uiRadioButton4.Text = "MB";
            this.uiRadioButton4.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            // 
            // editBox1
            // 
            this.editBox1.Location = new System.Drawing.Point(84, 32);
            this.editBox1.Name = "editBox1";
            this.editBox1.Size = new System.Drawing.Size(46, 20);
            this.editBox1.TabIndex = 3;
            this.editBox1.TextAlignment = Janus.Windows.GridEX.TextAlignment.Center;
            this.editBox1.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            // 
            // lblSeries
            // 
            this.lblSeries.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblSeries.Location = new System.Drawing.Point(28, 32);
            this.lblSeries.Name = "lblSeries";
            this.lblSeries.Size = new System.Drawing.Size(50, 20);
            this.lblSeries.TabIndex = 1;
            this.lblSeries.Text = "시리즈";
            this.lblSeries.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // panel3
            // 
            this.panel3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel3.Location = new System.Drawing.Point(438, 476);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(354, 102);
            this.panel3.TabIndex = 4;
            // 
            // SchMidAd_pSet
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(6, 16);
            this.BackColor = System.Drawing.Color.Gray;
            this.ClientSize = new System.Drawing.Size(794, 580);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panelTop);
            this.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "SchMidAd_pSet";
            this.Padding = new System.Windows.Forms.Padding(2);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "중간광고 정보설정";
            this.Load += new System.EventHandler(this.SchMidAd_pSet_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dvSeriesList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.schMidAdDs)).EndInit();
            this.panelTop.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridExList)).EndInit();
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.uiGroupBox3)).EndInit();
            this.uiGroupBox3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.uiGroupBox2)).EndInit();
            this.uiGroupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.uiGroupBox1)).EndInit();
            this.uiGroupBox1.ResumeLayout(false);
            this.ResumeLayout(false);

        }
		#endregion

        private void SchMidAd_pSet_Load(object sender, System.EventArgs e)
        {
            // 데이터관리용 객체생성
            dt = ((DataView)gridExList.DataSource).Table;
            cm = (CurrencyManager) this.BindingContext[gridExList.DataSource]; 

            SearchList();
        }

        public string Title
        {
            set
            {
                lblTitle.Text = value;
            }
        }

        public string   ChannelNo
        {
            set
            {
                lblChannelNo.Text = value;
            }

        }


        private void SearchList()
        {
            try
            {
                chooseAdScheduleModel.Init();
				
                // 데이터 클리어
                schMidAdDs.SeriesList.Clear();

                // 데이터모델에 전송할 내용을 셋트한다.				
                chooseAdScheduleModel.MediaCode     = keyMediaCode;
                chooseAdScheduleModel.CategoryCode  = keyCategoryCode;
                chooseAdScheduleModel.GenreCode     = keyGenreCode;
                chooseAdScheduleModel.ChannelNo     = keyChannelNo;

                // 장르메뉴 조회 서비스를 호출한다.
                new SchMidAdManager(systemModel,commonModel).GetMidAdInfoListSeries(chooseAdScheduleModel);

                if (chooseAdScheduleModel.ResultCD.Equals("0000"))
                {
                    Utility.SetDataTable(schMidAdDs.SeriesList, chooseAdScheduleModel.ChooseAdScheduleDataSet);
                }

            }
            catch(FrameException fe)
            {
                FrameSystem.showMsgForm("중간광고용 시리즈정보 조회오류", new string[] {fe.ErrCode, fe.ResultMsg});
            }
            catch(Exception ex)
            {
                FrameSystem.showMsgForm("중간광고용 시리즈정보 조회오류",new string[] {"",ex.Message});
            }
        }

        private void btnClose_Click(object sender, System.EventArgs e)
        {
            this.Close();
        }

	}
}
