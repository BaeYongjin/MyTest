using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace AdManagerClient._10_Media
{
	/// <summary>
	/// Form1에 대한 요약 설명입니다.
	/// </summary>
	public class Form1 : System.Windows.Forms.Form
	{
		private Janus.Windows.GridEX.EditControls.EditBox editBox1;
		private Janus.Windows.GridEX.EditControls.IntegerUpDown integerUpDown1;
		private Janus.Windows.GridEX.EditControls.MaskedEditBox maskedEditBox1;
		private Janus.Windows.GridEX.EditControls.MultiColumnCombo multiColumnCombo1;
		private Janus.Windows.GridEX.EditControls.NumericEditBox numericEditBox1;
		private Janus.Windows.GridEX.EditControls.ValueListUpDown valueListUpDown1;
		private Janus.Windows.GridEX.GridEXPrintDocument gridEXPrintDocument1;
		private Janus.Windows.CalendarCombo.CalendarCombo calendarCombo1;
		private Janus.Windows.EditControls.UIButton uiButton1;
		private Janus.Windows.EditControls.UICheckBox uiCheckBox1;
		private Janus.Windows.EditControls.UIColorButton uiColorButton1;
		private Janus.Windows.EditControls.UIColorPicker uiColorPicker1;
		private Janus.Windows.EditControls.UIComboBox uiComboBox1;
		private Janus.Windows.EditControls.UIFontPicker uiFontPicker1;
		private Janus.Windows.EditControls.UIGroupBox uiGroupBox1;
		private Janus.Windows.EditControls.UIProgressBar uiProgressBar1;
		private Janus.Windows.EditControls.UIRadioButton uiRadioButton1;
		private Janus.Windows.UI.CommandBars.UICommandManager uiCommandManager1;
		private Janus.Windows.UI.Dock.UIPanelManager uiPanelManager1;
		private Janus.Windows.UI.Dock.UIPager uiPager1;
		private Janus.Windows.UI.StatusBar.UIStatusBar uiStatusBar1;
		private Janus.Windows.UI.Tab.UITab uiTab1;
		private Janus.Windows.ExplorerBar.ExplorerBar explorerBar1;
		private Janus.Windows.ButtonBar.ButtonBar buttonBar1;
		private Janus.Windows.Schedule.Calendar calendar1;
		private System.ComponentModel.IContainer components;

		public Form1()
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
			this.components = new System.ComponentModel.Container();
			Janus.Windows.GridEX.GridEXLayout gridEXLayout1 = new Janus.Windows.GridEX.GridEXLayout();
			this.editBox1 = new Janus.Windows.GridEX.EditControls.EditBox();
			this.integerUpDown1 = new Janus.Windows.GridEX.EditControls.IntegerUpDown();
			this.maskedEditBox1 = new Janus.Windows.GridEX.EditControls.MaskedEditBox();
			this.multiColumnCombo1 = new Janus.Windows.GridEX.EditControls.MultiColumnCombo();
			this.numericEditBox1 = new Janus.Windows.GridEX.EditControls.NumericEditBox();
			this.valueListUpDown1 = new Janus.Windows.GridEX.EditControls.ValueListUpDown();
			this.gridEXPrintDocument1 = new Janus.Windows.GridEX.GridEXPrintDocument();
			this.calendarCombo1 = new Janus.Windows.CalendarCombo.CalendarCombo();
			this.uiButton1 = new Janus.Windows.EditControls.UIButton();
			this.uiCheckBox1 = new Janus.Windows.EditControls.UICheckBox();
			this.uiColorButton1 = new Janus.Windows.EditControls.UIColorButton();
			this.uiColorPicker1 = new Janus.Windows.EditControls.UIColorPicker();
			this.uiComboBox1 = new Janus.Windows.EditControls.UIComboBox();
			this.uiFontPicker1 = new Janus.Windows.EditControls.UIFontPicker();
			this.uiGroupBox1 = new Janus.Windows.EditControls.UIGroupBox();
			this.uiProgressBar1 = new Janus.Windows.EditControls.UIProgressBar();
			this.uiRadioButton1 = new Janus.Windows.EditControls.UIRadioButton();
			this.uiCommandManager1 = new Janus.Windows.UI.CommandBars.UICommandManager(this.components);
			this.uiPanelManager1 = new Janus.Windows.UI.Dock.UIPanelManager(this.components);
			this.uiPager1 = new Janus.Windows.UI.Dock.UIPager();
			this.uiStatusBar1 = new Janus.Windows.UI.StatusBar.UIStatusBar();
			this.uiTab1 = new Janus.Windows.UI.Tab.UITab();
			this.explorerBar1 = new Janus.Windows.ExplorerBar.ExplorerBar();
			this.buttonBar1 = new Janus.Windows.ButtonBar.ButtonBar();
			this.calendar1 = new Janus.Windows.Schedule.Calendar();
			((System.ComponentModel.ISupportInitialize)(this.uiGroupBox1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.uiCommandManager1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.uiPanelManager1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.uiTab1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.explorerBar1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.buttonBar1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.calendar1)).BeginInit();
			this.SuspendLayout();
			// 
			// editBox1
			// 
			this.editBox1.AutoScrollMargin = new System.Drawing.Size(0, 0);
			this.editBox1.AutoScrollMinSize = new System.Drawing.Size(0, 0);
			this.editBox1.Location = new System.Drawing.Point(48, 24);
			this.editBox1.Name = "editBox1";
			this.editBox1.Size = new System.Drawing.Size(75, 21);
			this.editBox1.TabIndex = 0;
			this.editBox1.Text = "editBox1";
			this.editBox1.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
			// 
			// integerUpDown1
			// 
			this.integerUpDown1.AutoScrollMargin = new System.Drawing.Size(0, 0);
			this.integerUpDown1.AutoScrollMinSize = new System.Drawing.Size(0, 0);
			this.integerUpDown1.Location = new System.Drawing.Point(160, 32);
			this.integerUpDown1.Name = "integerUpDown1";
			this.integerUpDown1.Size = new System.Drawing.Size(75, 21);
			this.integerUpDown1.TabIndex = 1;
			this.integerUpDown1.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
			// 
			// maskedEditBox1
			// 
			this.maskedEditBox1.AutoScrollMargin = new System.Drawing.Size(0, 0);
			this.maskedEditBox1.AutoScrollMinSize = new System.Drawing.Size(0, 0);
			this.maskedEditBox1.Location = new System.Drawing.Point(272, 32);
			this.maskedEditBox1.Name = "maskedEditBox1";
			this.maskedEditBox1.Size = new System.Drawing.Size(75, 21);
			this.maskedEditBox1.TabIndex = 2;
			this.maskedEditBox1.Text = "maskedEditBox1";
			this.maskedEditBox1.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
			// 
			// multiColumnCombo1
			// 
			this.multiColumnCombo1.AutoScrollMargin = new System.Drawing.Size(0, 0);
			this.multiColumnCombo1.AutoScrollMinSize = new System.Drawing.Size(0, 0);
			gridEXLayout1.LayoutString = @"<GridEXLayoutData><RootTable><GroupCondition ID="""" /></RootTable><RowWithErrorsFormatStyle><PredefinedStyle>RowWithErrorsFormatStyle</PredefinedStyle></RowWithErrorsFormatStyle><LinkFormatStyle><PredefinedStyle>LinkFormatStyle</PredefinedStyle></LinkFormatStyle><CardCaptionFormatStyle><PredefinedStyle>CardCaptionFormatStyle</PredefinedStyle></CardCaptionFormatStyle><GroupByBoxFormatStyle><PredefinedStyle>GroupByBoxFormatStyle</PredefinedStyle></GroupByBoxFormatStyle><GroupByBoxInfoFormatStyle><PredefinedStyle>GroupByBoxInfoFormatStyle</PredefinedStyle></GroupByBoxInfoFormatStyle><GroupRowFormatStyle><PredefinedStyle>GroupRowFormatStyle</PredefinedStyle></GroupRowFormatStyle><HeaderFormatStyle><PredefinedStyle>HeaderFormatStyle</PredefinedStyle></HeaderFormatStyle><PreviewRowFormatStyle><PredefinedStyle>PreviewRowFormatStyle</PredefinedStyle></PreviewRowFormatStyle><RowFormatStyle><PredefinedStyle>RowFormatStyle</PredefinedStyle></RowFormatStyle><SelectedFormatStyle><PredefinedStyle>SelectedFormatStyle</PredefinedStyle></SelectedFormatStyle><SelectedInactiveFormatStyle><PredefinedStyle>SelectedInactiveFormatStyle</PredefinedStyle></SelectedInactiveFormatStyle><WatermarkImage /><ControlStyle /><VisualStyle>Standard</VisualStyle><AllowEdit>False</AllowEdit><ExpandableGroups>False</ExpandableGroups><GroupByBoxVisible>False</GroupByBoxVisible><HideSelection>Highlight</HideSelection></GridEXLayoutData>";
			this.multiColumnCombo1.DesignTimeLayout = gridEXLayout1;
			this.multiColumnCombo1.Location = new System.Drawing.Point(408, 40);
			this.multiColumnCombo1.Name = "multiColumnCombo1";
			this.multiColumnCombo1.SelectedItem = null;
			this.multiColumnCombo1.Size = new System.Drawing.Size(75, 21);
			this.multiColumnCombo1.TabIndex = 3;
			this.multiColumnCombo1.Text = "multiColumnCombo1";
			this.multiColumnCombo1.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
			this.multiColumnCombo1.Value = null;
			// 
			// numericEditBox1
			// 
			this.numericEditBox1.AutoScrollMargin = new System.Drawing.Size(0, 0);
			this.numericEditBox1.AutoScrollMinSize = new System.Drawing.Size(0, 0);
			this.numericEditBox1.Location = new System.Drawing.Point(64, 72);
			this.numericEditBox1.Name = "numericEditBox1";
			this.numericEditBox1.Size = new System.Drawing.Size(75, 21);
			this.numericEditBox1.TabIndex = 4;
			this.numericEditBox1.Text = "0.00";
			this.numericEditBox1.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
			this.numericEditBox1.Value = new System.Decimal(new int[] {
																		  0,
																		  0,
																		  0,
																		  0});
			// 
			// valueListUpDown1
			// 
			this.valueListUpDown1.AutoScrollMargin = new System.Drawing.Size(0, 0);
			this.valueListUpDown1.AutoScrollMinSize = new System.Drawing.Size(0, 0);
			this.valueListUpDown1.Location = new System.Drawing.Point(216, 88);
			this.valueListUpDown1.Name = "valueListUpDown1";
			this.valueListUpDown1.Size = new System.Drawing.Size(75, 21);
			this.valueListUpDown1.TabIndex = 5;
			this.valueListUpDown1.Text = "valueListUpDown1";
			this.valueListUpDown1.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
			this.valueListUpDown1.Value = "valueListUpDown1";
			// 
			// calendarCombo1
			// 
			// 
			// calendarCombo1.DropDownCalendar
			// 
			this.calendarCombo1.DropDownCalendar.FirstMonth = new System.DateTime(2010, 4, 1, 0, 0, 0, 0);
			this.calendarCombo1.DropDownCalendar.Location = new System.Drawing.Point(0, 0);
			this.calendarCombo1.DropDownCalendar.Name = "";
			this.calendarCombo1.DropDownCalendar.Size = new System.Drawing.Size(182, 168);
			this.calendarCombo1.DropDownCalendar.TabIndex = 0;
			this.calendarCombo1.DropDownCalendar.VisualStyle = Janus.Windows.CalendarCombo.VisualStyle.Standard;
			this.calendarCombo1.Location = new System.Drawing.Point(96, 136);
			this.calendarCombo1.Name = "calendarCombo1";
			this.calendarCombo1.Size = new System.Drawing.Size(200, 21);
			this.calendarCombo1.TabIndex = 6;
			// 
			// uiButton1
			// 
			this.uiButton1.Location = new System.Drawing.Point(112, 200);
			this.uiButton1.Name = "uiButton1";
			this.uiButton1.Size = new System.Drawing.Size(75, 23);
			this.uiButton1.TabIndex = 7;
			this.uiButton1.Text = "uiButton1";
			// 
			// uiCheckBox1
			// 
			this.uiCheckBox1.Location = new System.Drawing.Point(56, 248);
			this.uiCheckBox1.Name = "uiCheckBox1";
			this.uiCheckBox1.Size = new System.Drawing.Size(104, 23);
			this.uiCheckBox1.TabIndex = 8;
			this.uiCheckBox1.Text = "uiCheckBox1";
			// 
			// uiColorButton1
			// 
			// 
			// uiColorButton1.ColorPicker
			// 
			this.uiColorButton1.ColorPicker.Location = new System.Drawing.Point(0, 0);
			this.uiColorButton1.ColorPicker.Name = "";
			this.uiColorButton1.ColorPicker.Size = new System.Drawing.Size(100, 100);
			this.uiColorButton1.ColorPicker.TabIndex = 0;
			this.uiColorButton1.ImageReplaceableColor = System.Drawing.Color.Empty;
			this.uiColorButton1.Location = new System.Drawing.Point(328, 224);
			this.uiColorButton1.Name = "uiColorButton1";
			this.uiColorButton1.Size = new System.Drawing.Size(75, 23);
			this.uiColorButton1.TabIndex = 9;
			this.uiColorButton1.Text = "uiColorButton1";
			// 
			// uiColorPicker1
			// 
			this.uiColorPicker1.Location = new System.Drawing.Point(240, 264);
			this.uiColorPicker1.Name = "uiColorPicker1";
			this.uiColorPicker1.Size = new System.Drawing.Size(152, 151);
			this.uiColorPicker1.TabIndex = 10;
			this.uiColorPicker1.Text = "uiColorPicker1";
			// 
			// uiComboBox1
			// 
			this.uiComboBox1.Location = new System.Drawing.Point(432, 160);
			this.uiComboBox1.Name = "uiComboBox1";
			this.uiComboBox1.Size = new System.Drawing.Size(176, 21);
			this.uiComboBox1.TabIndex = 11;
			this.uiComboBox1.Text = "uiComboBox1";
			// 
			// uiFontPicker1
			// 
			this.uiFontPicker1.Location = new System.Drawing.Point(560, 240);
			this.uiFontPicker1.Name = "uiFontPicker1";
			this.uiFontPicker1.SelectedIndex = 0;
			this.uiFontPicker1.Size = new System.Drawing.Size(176, 22);
			this.uiFontPicker1.TabIndex = 12;
			// 
			// uiGroupBox1
			// 
			this.uiGroupBox1.Location = new System.Drawing.Point(608, 88);
			this.uiGroupBox1.Name = "uiGroupBox1";
			this.uiGroupBox1.Size = new System.Drawing.Size(200, 100);
			this.uiGroupBox1.TabIndex = 13;
			this.uiGroupBox1.Text = "uiGroupBox1";
			// 
			// uiProgressBar1
			// 
			this.uiProgressBar1.Location = new System.Drawing.Point(408, 104);
			this.uiProgressBar1.Name = "uiProgressBar1";
			this.uiProgressBar1.Size = new System.Drawing.Size(75, 23);
			this.uiProgressBar1.TabIndex = 14;
			this.uiProgressBar1.Text = "uiProgressBar1";
			// 
			// uiRadioButton1
			// 
			this.uiRadioButton1.Location = new System.Drawing.Point(312, 184);
			this.uiRadioButton1.Name = "uiRadioButton1";
			this.uiRadioButton1.Size = new System.Drawing.Size(104, 23);
			this.uiRadioButton1.TabIndex = 15;
			this.uiRadioButton1.TabStop = false;
			this.uiRadioButton1.Text = "uiRadioButton1";
			// 
			// uiCommandManager1
			// 
			this.uiCommandManager1.ContainerControl = this;
			this.uiCommandManager1.Id = new System.Guid("0f722e67-d5b4-44f4-9ede-ba5435add81e");
			// 
			// uiPanelManager1
			// 
			this.uiPanelManager1.BackColorGradientAutoHideStrip = System.Drawing.Color.FromArgb(((System.Byte)(196)), ((System.Byte)(218)), ((System.Byte)(250)));
			this.uiPanelManager1.ContainerControl = this;
			// 
			// uiPager1
			// 
			this.uiPager1.Location = new System.Drawing.Point(568, 48);
			this.uiPager1.Name = "uiPager1";
			this.uiPager1.Size = new System.Drawing.Size(200, 100);
			this.uiPager1.TabIndex = 20;
			this.uiPager1.TabStop = false;
			// 
			// uiStatusBar1
			// 
			this.uiStatusBar1.Location = new System.Drawing.Point(0, 334);
			this.uiStatusBar1.Name = "uiStatusBar1";
			this.uiStatusBar1.PanelsBorderColor = System.Drawing.SystemColors.ControlDark;
			this.uiStatusBar1.Size = new System.Drawing.Size(864, 23);
			this.uiStatusBar1.TabIndex = 21;
			// 
			// uiTab1
			// 
			this.uiTab1.Location = new System.Drawing.Point(64, 160);
			this.uiTab1.Name = "uiTab1";
			this.uiTab1.Size = new System.Drawing.Size(200, 100);
			this.uiTab1.TabIndex = 22;
			// 
			// explorerBar1
			// 
//			this.explorerBar1.HoverItemFormatStyle.FontUnderline = Janus.Windows.ExplorerBar.TriState.True;
			this.explorerBar1.Location = new System.Drawing.Point(376, 96);
			this.explorerBar1.Name = "explorerBar1";
			this.explorerBar1.Size = new System.Drawing.Size(75, 23);
			this.explorerBar1.TabIndex = 23;
			this.explorerBar1.Text = "explorerBar1";
			// 
			// buttonBar1
			// 
			this.buttonBar1.Location = new System.Drawing.Point(312, 120);
			this.buttonBar1.Name = "buttonBar1";
			this.buttonBar1.Size = new System.Drawing.Size(75, 23);
			this.buttonBar1.TabIndex = 24;
			this.buttonBar1.Text = "buttonBar1";
			// 
			// calendar1
			// 
			this.calendar1.CurrentDate = new System.DateTime(2010, 4, 15, 0, 0, 0, 0);
			this.calendar1.FirstMonth = new System.DateTime(2010, 4, 1, 0, 0, 0, 0);
			this.calendar1.HideSelection = Janus.Windows.Schedule.HideSelection.HighlightInactive;
			this.calendar1.Location = new System.Drawing.Point(528, 80);
			this.calendar1.Name = "calendar1";
			this.calendar1.Size = new System.Drawing.Size(170, 136);
			this.calendar1.TabIndex = 25;
			// 
			// Form1
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(6, 14);
			this.ClientSize = new System.Drawing.Size(864, 357);
			this.Controls.Add(this.calendar1);
			this.Controls.Add(this.buttonBar1);
			this.Controls.Add(this.explorerBar1);
			this.Controls.Add(this.uiTab1);
			this.Controls.Add(this.uiPager1);
			this.Controls.Add(this.uiRadioButton1);
			this.Controls.Add(this.uiProgressBar1);
			this.Controls.Add(this.uiGroupBox1);
			this.Controls.Add(this.uiFontPicker1);
			this.Controls.Add(this.uiComboBox1);
			this.Controls.Add(this.uiColorPicker1);
			this.Controls.Add(this.uiColorButton1);
			this.Controls.Add(this.uiCheckBox1);
			this.Controls.Add(this.uiButton1);
			this.Controls.Add(this.calendarCombo1);
			this.Controls.Add(this.valueListUpDown1);
			this.Controls.Add(this.numericEditBox1);
			this.Controls.Add(this.multiColumnCombo1);
			this.Controls.Add(this.maskedEditBox1);
			this.Controls.Add(this.integerUpDown1);
			this.Controls.Add(this.editBox1);
			this.Controls.Add(this.uiStatusBar1);
			this.Name = "Form1";
			this.Text = "Form1";
			((System.ComponentModel.ISupportInitialize)(this.uiGroupBox1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.uiCommandManager1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.uiPanelManager1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.uiTab1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.explorerBar1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.buttonBar1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.calendar1)).EndInit();
			this.ResumeLayout(false);

		}
		#endregion
	}
}
