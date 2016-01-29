namespace AdManagerClient
{
    partial class AnalysisItemAutoAddForm
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
            Janus.Windows.GridEX.GridEXLayout grdExContractItemList_Layout_0 = new Janus.Windows.GridEX.GridEXLayout();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AnalysisItemAutoAddForm));
            this.bsContractItem = new System.Windows.Forms.BindingSource(this.components);
            this.analysisItemGroupDs = new AdManagerClient._10_Media._13_AnalysisItemGroup.AnalysisItemGroupDs();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnClose = new Janus.Windows.EditControls.UIButton();
            this.btnOk = new Janus.Windows.EditControls.UIButton();
            this.cbAnalysisItemGroupType = new Janus.Windows.EditControls.UIComboBox();
            this.ebComment = new Janus.Windows.GridEX.EditControls.EditBox();
            this.ebAnalysisItemGroupName = new Janus.Windows.GridEX.EditControls.EditBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.grdExContractItemList = new Janus.Windows.GridEX.GridEX();
            ((System.ComponentModel.ISupportInitialize)(this.bsContractItem)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.analysisItemGroupDs)).BeginInit();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdExContractItemList)).BeginInit();
            this.SuspendLayout();
            // 
            // bsContractItem
            // 
            this.bsContractItem.DataMember = "ContractItem";
            this.bsContractItem.DataSource = this.analysisItemGroupDs;
            // 
            // analysisItemGroupDs
            // 
            this.analysisItemGroupDs.DataSetName = "AnalysisItemGroupDs";
            this.analysisItemGroupDs.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.Window;
            this.panel1.Controls.Add(this.btnClose);
            this.panel1.Controls.Add(this.btnOk);
            this.panel1.Controls.Add(this.cbAnalysisItemGroupType);
            this.panel1.Controls.Add(this.ebComment);
            this.panel1.Controls.Add(this.ebAnalysisItemGroupName);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.grdExContractItemList);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(773, 578);
            this.panel1.TabIndex = 0;
            // 
            // btnClose
            // 
            this.btnClose.BackColor = System.Drawing.SystemColors.Control;
            this.btnClose.Location = new System.Drawing.Point(529, 547);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(70, 23);
            this.btnClose.TabIndex = 22;
            this.btnClose.Text = "닫기";
            this.btnClose.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnOk
            // 
            this.btnOk.BackColor = System.Drawing.SystemColors.Control;
            this.btnOk.Location = new System.Drawing.Point(449, 547);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(70, 23);
            this.btnOk.TabIndex = 21;
            this.btnOk.Text = "확인";
            this.btnOk.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // cbAnalysisItemGroupType
            // 
            this.cbAnalysisItemGroupType.ComboStyle = Janus.Windows.EditControls.ComboStyle.DropDownList;
            this.cbAnalysisItemGroupType.Location = new System.Drawing.Point(213, 419);
            this.cbAnalysisItemGroupType.Name = "cbAnalysisItemGroupType";
            this.cbAnalysisItemGroupType.Size = new System.Drawing.Size(128, 21);
            this.cbAnalysisItemGroupType.TabIndex = 15;
            this.cbAnalysisItemGroupType.Text = "기본";
            this.cbAnalysisItemGroupType.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            // 
            // ebComment
            // 
            this.ebComment.Location = new System.Drawing.Point(213, 443);
            this.ebComment.MaxLength = 100;
            this.ebComment.Multiline = true;
            this.ebComment.Name = "ebComment";
            this.ebComment.Size = new System.Drawing.Size(386, 88);
            this.ebComment.TabIndex = 20;
            this.ebComment.TabStop = false;
            this.ebComment.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
            this.ebComment.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            // 
            // ebAnalysisItemGroupName
            // 
            this.ebAnalysisItemGroupName.Location = new System.Drawing.Point(213, 395);
            this.ebAnalysisItemGroupName.MaxLength = 25;
            this.ebAnalysisItemGroupName.Name = "ebAnalysisItemGroupName";
            this.ebAnalysisItemGroupName.Size = new System.Drawing.Size(386, 21);
            this.ebAnalysisItemGroupName.TabIndex = 19;
            this.ebAnalysisItemGroupName.TabStop = false;
            this.ebAnalysisItemGroupName.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
            this.ebAnalysisItemGroupName.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(141, 549);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(378, 21);
            this.label2.TabIndex = 16;
            this.label2.Text = "다음과 같은 내용으로 광고묶음이 새로 등록됩니다.";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(141, 396);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(83, 21);
            this.label4.TabIndex = 16;
            this.label4.Text = "광고묶음명";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(141, 420);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(72, 21);
            this.label3.TabIndex = 17;
            this.label3.Text = "타입";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(141, 444);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(72, 21);
            this.label1.TabIndex = 18;
            this.label1.Text = "비고";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // grdExContractItemList
            // 
            this.grdExContractItemList.AlternatingColors = true;
            this.grdExContractItemList.BorderStyle = Janus.Windows.GridEX.BorderStyle.Flat;
            this.grdExContractItemList.DataSource = this.bsContractItem;
            this.grdExContractItemList.EmptyRows = true;
            this.grdExContractItemList.FocusCellFormatStyle.BackColor = System.Drawing.SystemColors.Highlight;
            this.grdExContractItemList.FocusCellFormatStyle.ForeColor = System.Drawing.Color.White;
            this.grdExContractItemList.FocusStyle = Janus.Windows.GridEX.FocusStyle.None;
            this.grdExContractItemList.Font = new System.Drawing.Font("굴림", 9F);
            this.grdExContractItemList.GridLines = Janus.Windows.GridEX.GridLines.Vertical;
            this.grdExContractItemList.GridLineStyle = Janus.Windows.GridEX.GridLineStyle.Solid;
            this.grdExContractItemList.GroupByBoxVisible = false;
            this.grdExContractItemList.GroupMode = Janus.Windows.GridEX.GroupMode.Collapsed;
            this.grdExContractItemList.HeaderFormatStyle.TextAlignment = Janus.Windows.GridEX.TextAlignment.Center;
            grdExContractItemList_Layout_0.DataSource = this.bsContractItem;
            grdExContractItemList_Layout_0.IsCurrentLayout = true;
            grdExContractItemList_Layout_0.Key = "Lee";
            grdExContractItemList_Layout_0.LayoutString = resources.GetString("grdExContractItemList_Layout_0.LayoutString");
            this.grdExContractItemList.Layouts.AddRange(new Janus.Windows.GridEX.GridEXLayout[] {
            grdExContractItemList_Layout_0});
            this.grdExContractItemList.Location = new System.Drawing.Point(12, 12);
            this.grdExContractItemList.Name = "grdExContractItemList";
            this.grdExContractItemList.ScrollBars = Janus.Windows.GridEX.ScrollBars.Vertical;
            this.grdExContractItemList.Size = new System.Drawing.Size(750, 376);
            this.grdExContractItemList.TabIndex = 0;
            this.grdExContractItemList.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            // 
            // AnalysisItemAutoAddForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(773, 578);
            this.Controls.Add(this.panel1);
            this.MaximumSize = new System.Drawing.Size(789, 617);
            this.MinimumSize = new System.Drawing.Size(789, 617);
            this.Name = "AnalysisItemAutoAddForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "광고묶음 자동등록";
            this.Load += new System.EventHandler(this.AnalysisItemAutoAddForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.bsContractItem)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.analysisItemGroupDs)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdExContractItemList)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private Janus.Windows.GridEX.GridEX grdExContractItemList;
        private Janus.Windows.EditControls.UIComboBox cbAnalysisItemGroupType;
        private Janus.Windows.GridEX.EditControls.EditBox ebComment;
        private Janus.Windows.GridEX.EditControls.EditBox ebAnalysisItemGroupName;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label1;
        private Janus.Windows.EditControls.UIButton btnClose;
        private Janus.Windows.EditControls.UIButton btnOk;
        private System.Windows.Forms.BindingSource bsContractItem;
        private _10_Media._13_AnalysisItemGroup.AnalysisItemGroupDs analysisItemGroupDs;
    }
}