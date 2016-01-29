namespace AdManagerClient
{
    partial class SchExclusiveZoneAd_pSet
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SchExclusiveZoneAd_pSet));
            System.Windows.Forms.TreeListViewItemCollection.TreeListViewItemCollectionComparer treeListViewItemCollectionComparer1 = new System.Windows.Forms.TreeListViewItemCollection.TreeListViewItemCollectionComparer();
            System.Windows.Forms.TreeListViewItemCollection.TreeListViewItemCollectionComparer treeListViewItemCollectionComparer2 = new System.Windows.Forms.TreeListViewItemCollection.TreeListViewItemCollectionComparer();
            this.chkTimeYn = new Janus.Windows.EditControls.UICheckBox();
            this.ugbTime = new Janus.Windows.EditControls.UIGroupBox();
            this.rbWeekAll = new System.Windows.Forms.RadioButton();
            this.rbWeekEnd = new System.Windows.Forms.RadioButton();
            this.rbWeekDay = new System.Windows.Forms.RadioButton();
            this.rbWeek = new System.Windows.Forms.RadioButton();
            this.lbl_AdName = new System.Windows.Forms.Label();
            this.imgTree = new System.Windows.Forms.ImageList(this.components);
            this.lblWeekDay = new System.Windows.Forms.Label();
            this.tlvDay = new System.Windows.Forms.TreeListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.tlvEnd = new System.Windows.Forms.TreeListView();
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.btnConfirm = new Janus.Windows.EditControls.UIButton();
            this.btnCancel = new Janus.Windows.EditControls.UIButton();
            this.label2 = new System.Windows.Forms.Label();
            this.chkWeekYn = new Janus.Windows.EditControls.UICheckBox();
            this.gbDays = new Janus.Windows.EditControls.UIGroupBox();
            this.chkSun = new Janus.Windows.EditControls.UICheckBox();
            this.chkSat = new Janus.Windows.EditControls.UICheckBox();
            this.chkFri = new Janus.Windows.EditControls.UICheckBox();
            this.chkThe = new Janus.Windows.EditControls.UICheckBox();
            this.chkWed = new Janus.Windows.EditControls.UICheckBox();
            this.chkMon = new Janus.Windows.EditControls.UICheckBox();
            this.chkThu = new Janus.Windows.EditControls.UICheckBox();
            this.schExclusiveZoneAdDs = new AdManagerClient.SchExclusiveZoneAdDs();
            ((System.ComponentModel.ISupportInitialize)(this.ugbTime)).BeginInit();
            this.ugbTime.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gbDays)).BeginInit();
            this.gbDays.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.schExclusiveZoneAdDs)).BeginInit();
            this.SuspendLayout();
            // 
            // chkTimeYn
            // 
            this.chkTimeYn.Font = new System.Drawing.Font("맑은 고딕", 8.25F, System.Drawing.FontStyle.Bold);
            this.chkTimeYn.Location = new System.Drawing.Point(16, 144);
            this.chkTimeYn.Name = "chkTimeYn";
            this.chkTimeYn.Size = new System.Drawing.Size(68, 23);
            this.chkTimeYn.TabIndex = 0;
            this.chkTimeYn.Text = "시간대";
            this.chkTimeYn.CheckedChanged += new System.EventHandler(this.chkTimeYn_CheckedChanged);
            // 
            // ugbTime
            // 
            this.ugbTime.BorderColor = System.Drawing.Color.Black;
            this.ugbTime.Controls.Add(this.rbWeekAll);
            this.ugbTime.Controls.Add(this.rbWeekEnd);
            this.ugbTime.Controls.Add(this.rbWeekDay);
            this.ugbTime.Controls.Add(this.rbWeek);
            this.ugbTime.Location = new System.Drawing.Point(16, 169);
            this.ugbTime.Name = "ugbTime";
            this.ugbTime.Size = new System.Drawing.Size(369, 45);
            this.ugbTime.TabIndex = 1;
            // 
            // rbWeekAll
            // 
            this.rbWeekAll.AutoSize = true;
            this.rbWeekAll.Enabled = false;
            this.rbWeekAll.Font = new System.Drawing.Font("맑은 고딕", 8.25F, System.Drawing.FontStyle.Bold);
            this.rbWeekAll.Location = new System.Drawing.Point(284, 16);
            this.rbWeekAll.Name = "rbWeekAll";
            this.rbWeekAll.Size = new System.Drawing.Size(47, 17);
            this.rbWeekAll.TabIndex = 3;
            this.rbWeekAll.TabStop = true;
            this.rbWeekAll.Text = "없음";
            this.rbWeekAll.UseVisualStyleBackColor = true;
            this.rbWeekAll.Click += new System.EventHandler(this.rbWeekAll_Click);
            // 
            // rbWeekEnd
            // 
            this.rbWeekEnd.AutoSize = true;
            this.rbWeekEnd.Enabled = false;
            this.rbWeekEnd.Font = new System.Drawing.Font("맑은 고딕", 8.25F, System.Drawing.FontStyle.Bold);
            this.rbWeekEnd.Location = new System.Drawing.Point(204, 16);
            this.rbWeekEnd.Name = "rbWeekEnd";
            this.rbWeekEnd.Size = new System.Drawing.Size(47, 17);
            this.rbWeekEnd.TabIndex = 2;
            this.rbWeekEnd.TabStop = true;
            this.rbWeekEnd.Text = "주말";
            this.rbWeekEnd.UseVisualStyleBackColor = true;
            this.rbWeekEnd.Click += new System.EventHandler(this.rbWeekEnd_Click);
            // 
            // rbWeekDay
            // 
            this.rbWeekDay.AutoSize = true;
            this.rbWeekDay.Enabled = false;
            this.rbWeekDay.Font = new System.Drawing.Font("맑은 고딕", 8.25F, System.Drawing.FontStyle.Bold);
            this.rbWeekDay.Location = new System.Drawing.Point(124, 16);
            this.rbWeekDay.Name = "rbWeekDay";
            this.rbWeekDay.Size = new System.Drawing.Size(47, 17);
            this.rbWeekDay.TabIndex = 1;
            this.rbWeekDay.TabStop = true;
            this.rbWeekDay.Text = "주중";
            this.rbWeekDay.UseVisualStyleBackColor = true;
            this.rbWeekDay.Click += new System.EventHandler(this.rbWeekDay_Click);
            // 
            // rbWeek
            // 
            this.rbWeek.AutoSize = true;
            this.rbWeek.Enabled = false;
            this.rbWeek.Font = new System.Drawing.Font("맑은 고딕", 8.25F, System.Drawing.FontStyle.Bold);
            this.rbWeek.Location = new System.Drawing.Point(44, 16);
            this.rbWeek.Name = "rbWeek";
            this.rbWeek.Size = new System.Drawing.Size(47, 17);
            this.rbWeek.TabIndex = 0;
            this.rbWeek.TabStop = true;
            this.rbWeek.Text = "분리";
            this.rbWeek.UseVisualStyleBackColor = true;
            this.rbWeek.Click += new System.EventHandler(this.rbWeek_Click);
            // 
            // lbl_AdName
            // 
            this.lbl_AdName.AutoSize = true;
            this.lbl_AdName.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lbl_AdName.Location = new System.Drawing.Point(18, 15);
            this.lbl_AdName.Name = "lbl_AdName";
            this.lbl_AdName.Size = new System.Drawing.Size(0, 15);
            this.lbl_AdName.TabIndex = 2;
            // 
            // imgTree
            // 
            this.imgTree.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imgTree.ImageStream")));
            this.imgTree.TransparentColor = System.Drawing.Color.Transparent;
            this.imgTree.Images.SetKeyName(0, "");
            // 
            // lblWeekDay
            // 
            this.lblWeekDay.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblWeekDay.Location = new System.Drawing.Point(14, 228);
            this.lblWeekDay.Name = "lblWeekDay";
            this.lblWeekDay.Size = new System.Drawing.Size(173, 23);
            this.lblWeekDay.TabIndex = 215;
            this.lblWeekDay.Text = "주중 시간 설정";
            this.lblWeekDay.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tlvDay
            // 
            this.tlvDay.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tlvDay.CheckBoxes = System.Windows.Forms.CheckBoxesTypes.Recursive;
            this.tlvDay.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1});
            treeListViewItemCollectionComparer1.Column = 0;
            treeListViewItemCollectionComparer1.SortOrder = System.Windows.Forms.SortOrder.None;
            this.tlvDay.Comparer = treeListViewItemCollectionComparer1;
            this.tlvDay.Enabled = false;
            this.tlvDay.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.tlvDay.Location = new System.Drawing.Point(16, 254);
            this.tlvDay.Name = "tlvDay";
            this.tlvDay.Size = new System.Drawing.Size(176, 346);
            this.tlvDay.SmallImageList = this.imgTree;
            this.tlvDay.Sorting = System.Windows.Forms.SortOrder.None;
            this.tlvDay.TabIndex = 216;
            this.tlvDay.UseCompatibleStateImageBehavior = false;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "시간";
            this.columnHeader1.Width = 150;
            // 
            // tlvEnd
            // 
            this.tlvEnd.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tlvEnd.CheckBoxes = System.Windows.Forms.CheckBoxesTypes.Recursive;
            this.tlvEnd.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader2});
            treeListViewItemCollectionComparer2.Column = 0;
            treeListViewItemCollectionComparer2.SortOrder = System.Windows.Forms.SortOrder.None;
            this.tlvEnd.Comparer = treeListViewItemCollectionComparer2;
            this.tlvEnd.Enabled = false;
            this.tlvEnd.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.tlvEnd.Location = new System.Drawing.Point(214, 254);
            this.tlvEnd.Name = "tlvEnd";
            this.tlvEnd.Size = new System.Drawing.Size(176, 346);
            this.tlvEnd.SmallImageList = this.imgTree;
            this.tlvEnd.Sorting = System.Windows.Forms.SortOrder.None;
            this.tlvEnd.TabIndex = 217;
            this.tlvEnd.UseCompatibleStateImageBehavior = false;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "시간";
            this.columnHeader2.Width = 150;
            // 
            // btnConfirm
            // 
            this.btnConfirm.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnConfirm.BackColor = System.Drawing.SystemColors.Window;
            this.btnConfirm.Location = new System.Drawing.Point(107, 634);
            this.btnConfirm.Name = "btnConfirm";
            this.btnConfirm.Size = new System.Drawing.Size(80, 24);
            this.btnConfirm.TabIndex = 218;
            this.btnConfirm.Text = "저 장";
            this.btnConfirm.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.btnConfirm.Click += new System.EventHandler(this.btnConfirm_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnCancel.BackColor = System.Drawing.SystemColors.Window;
            this.btnCancel.Location = new System.Drawing.Point(214, 634);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(80, 24);
            this.btnCancel.TabIndex = 219;
            this.btnCancel.Text = "취 소";
            this.btnCancel.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label2.Location = new System.Drawing.Point(212, 228);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(100, 23);
            this.label2.TabIndex = 220;
            this.label2.Text = "주말 시간 설정";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // chkWeekYn
            // 
            this.chkWeekYn.BackColor = System.Drawing.Color.Transparent;
            this.chkWeekYn.Font = new System.Drawing.Font("맑은 고딕", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.chkWeekYn.Location = new System.Drawing.Point(24, 46);
            this.chkWeekYn.Name = "chkWeekYn";
            this.chkWeekYn.Size = new System.Drawing.Size(68, 20);
            this.chkWeekYn.TabIndex = 221;
            this.chkWeekYn.Text = "요일별";
            this.chkWeekYn.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.chkWeekYn.CheckedChanged += new System.EventHandler(this.chkWeekYn_CheckedChanged);
            // 
            // gbDays
            // 
            this.gbDays.BackColor = System.Drawing.Color.Transparent;
            this.gbDays.BorderColor = System.Drawing.Color.Black;
            this.gbDays.Controls.Add(this.chkSun);
            this.gbDays.Controls.Add(this.chkSat);
            this.gbDays.Controls.Add(this.chkFri);
            this.gbDays.Controls.Add(this.chkThe);
            this.gbDays.Controls.Add(this.chkWed);
            this.gbDays.Controls.Add(this.chkMon);
            this.gbDays.Controls.Add(this.chkThu);
            this.gbDays.Location = new System.Drawing.Point(19, 75);
            this.gbDays.Name = "gbDays";
            this.gbDays.Size = new System.Drawing.Size(369, 48);
            this.gbDays.TabIndex = 222;
            // 
            // chkSun
            // 
            this.chkSun.Enabled = false;
            this.chkSun.Font = new System.Drawing.Font("맑은 고딕", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.chkSun.ForeColor = System.Drawing.Color.Red;
            this.chkSun.Location = new System.Drawing.Point(321, 15);
            this.chkSun.Name = "chkSun";
            this.chkSun.Size = new System.Drawing.Size(32, 21);
            this.chkSun.TabIndex = 30;
            this.chkSun.Tag = "18";
            this.chkSun.Text = "일";
            this.chkSun.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            // 
            // chkSat
            // 
            this.chkSat.Enabled = false;
            this.chkSat.Font = new System.Drawing.Font("맑은 고딕", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.chkSat.ForeColor = System.Drawing.Color.Blue;
            this.chkSat.Location = new System.Drawing.Point(270, 16);
            this.chkSat.Name = "chkSat";
            this.chkSat.Size = new System.Drawing.Size(32, 21);
            this.chkSat.TabIndex = 29;
            this.chkSat.Tag = "16";
            this.chkSat.Text = "토";
            this.chkSat.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            // 
            // chkFri
            // 
            this.chkFri.Enabled = false;
            this.chkFri.Location = new System.Drawing.Point(219, 16);
            this.chkFri.Name = "chkFri";
            this.chkFri.Size = new System.Drawing.Size(32, 21);
            this.chkFri.TabIndex = 28;
            this.chkFri.Tag = "5";
            this.chkFri.Text = "금";
            this.chkFri.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            // 
            // chkThe
            // 
            this.chkThe.Enabled = false;
            this.chkThe.Location = new System.Drawing.Point(168, 16);
            this.chkThe.Name = "chkThe";
            this.chkThe.Size = new System.Drawing.Size(32, 21);
            this.chkThe.TabIndex = 27;
            this.chkThe.Tag = "4";
            this.chkThe.Text = "목";
            this.chkThe.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            // 
            // chkWed
            // 
            this.chkWed.Enabled = false;
            this.chkWed.Location = new System.Drawing.Point(117, 16);
            this.chkWed.Name = "chkWed";
            this.chkWed.Size = new System.Drawing.Size(32, 21);
            this.chkWed.TabIndex = 26;
            this.chkWed.Tag = "3";
            this.chkWed.Text = "수";
            this.chkWed.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            // 
            // chkMon
            // 
            this.chkMon.Enabled = false;
            this.chkMon.Location = new System.Drawing.Point(15, 16);
            this.chkMon.Name = "chkMon";
            this.chkMon.Size = new System.Drawing.Size(32, 21);
            this.chkMon.TabIndex = 24;
            this.chkMon.Tag = "1";
            this.chkMon.Text = "월";
            this.chkMon.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            // 
            // chkThu
            // 
            this.chkThu.Enabled = false;
            this.chkThu.Location = new System.Drawing.Point(66, 16);
            this.chkThu.Name = "chkThu";
            this.chkThu.Size = new System.Drawing.Size(32, 21);
            this.chkThu.TabIndex = 25;
            this.chkThu.Tag = "2";
            this.chkThu.Text = "화";
            this.chkThu.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            // 
            // schExclusiveZoneAdDs
            // 
            this.schExclusiveZoneAdDs.DataSetName = "SchExclusiveZoneAdDs";
            this.schExclusiveZoneAdDs.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // SchExclusiveZoneAd_pSet
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(406, 670);
            this.Controls.Add(this.gbDays);
            this.Controls.Add(this.chkWeekYn);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnConfirm);
            this.Controls.Add(this.tlvEnd);
            this.Controls.Add(this.tlvDay);
            this.Controls.Add(this.lblWeekDay);
            this.Controls.Add(this.lbl_AdName);
            this.Controls.Add(this.ugbTime);
            this.Controls.Add(this.chkTimeYn);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SchExclusiveZoneAd_pSet";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "시간대독점설정";
            this.Load += new System.EventHandler(this.SchExclusiveZoneAd_pSet_Load);
            ((System.ComponentModel.ISupportInitialize)(this.ugbTime)).EndInit();
            this.ugbTime.ResumeLayout(false);
            this.ugbTime.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gbDays)).EndInit();
            this.gbDays.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.schExclusiveZoneAdDs)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Janus.Windows.EditControls.UICheckBox chkTimeYn;
        private Janus.Windows.EditControls.UIGroupBox ugbTime;
        private System.Windows.Forms.Label lbl_AdName;
        private System.Windows.Forms.RadioButton rbWeek;
        private System.Windows.Forms.RadioButton rbWeekAll;
        private System.Windows.Forms.RadioButton rbWeekEnd;
        private System.Windows.Forms.RadioButton rbWeekDay;
        private System.Windows.Forms.ImageList imgTree;
        private System.Windows.Forms.Label lblWeekDay;
        private System.Windows.Forms.TreeListView tlvDay;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.TreeListView tlvEnd;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private Janus.Windows.EditControls.UIButton btnConfirm;
        private Janus.Windows.EditControls.UIButton btnCancel;
        private System.Windows.Forms.Label label2;
        private Janus.Windows.EditControls.UICheckBox chkWeekYn;
        private Janus.Windows.EditControls.UIGroupBox gbDays;
        private Janus.Windows.EditControls.UICheckBox chkSun;
        private Janus.Windows.EditControls.UICheckBox chkSat;
        private Janus.Windows.EditControls.UICheckBox chkFri;
        private Janus.Windows.EditControls.UICheckBox chkThe;
        private Janus.Windows.EditControls.UICheckBox chkWed;
        private Janus.Windows.EditControls.UICheckBox chkMon;
        private Janus.Windows.EditControls.UICheckBox chkThu;
        private SchExclusiveZoneAdDs schExclusiveZoneAdDs;
    }
}