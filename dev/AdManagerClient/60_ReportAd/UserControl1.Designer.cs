namespace AdManagerClient._60_ReportAd
{
	partial class UserControl1
	{
		/// <summary> 
		/// 필수 디자이너 변수입니다.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary> 
		/// 사용 중인 모든 리소스를 정리합니다.
		/// </summary>
		/// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region 구성 요소 디자이너에서 생성한 코드

		/// <summary> 
		/// 디자이너 지원에 필요한 메서드입니다. 
		/// 이 메서드의 내용을 코드 편집기로 수정하지 마십시오.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			Janus.Windows.GridEX.GridEXLayout grdExMediaList_DesignTimeLayout = new Janus.Windows.GridEX.GridEXLayout();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UserControl1));
			this.uiPanelManager1 = new Janus.Windows.UI.Dock.UIPanelManager(this.components);
			this.uiPanel0 = new Janus.Windows.UI.Dock.UIPanel();
			this.uiPanel0Container = new Janus.Windows.UI.Dock.UIPanelInnerContainer();
			this.uiPanel1 = new Janus.Windows.UI.Dock.UIPanel();
			this.uiPanel1Container = new Janus.Windows.UI.Dock.UIPanelInnerContainer();
			this.pnlSearch = new System.Windows.Forms.Panel();
			this.uiCheckBox3 = new Janus.Windows.EditControls.UICheckBox();
			this.ebSearchKey = new Janus.Windows.GridEX.EditControls.EditBox();
			this.btnSearch = new Janus.Windows.EditControls.UIButton();
			this.lbUserTell = new System.Windows.Forms.Label();
			this.grdExMediaList = new Janus.Windows.GridEX.GridEX();
			this.uiGroupBox1 = new Janus.Windows.EditControls.UIGroupBox();
			this.label1 = new System.Windows.Forms.Label();
			this.editBox1 = new Janus.Windows.GridEX.EditControls.EditBox();
			this.editBox2 = new Janus.Windows.GridEX.EditControls.EditBox();
			this.label2 = new System.Windows.Forms.Label();
			this.editBox3 = new Janus.Windows.GridEX.EditControls.EditBox();
			this.label3 = new System.Windows.Forms.Label();
			this.editBox4 = new Janus.Windows.GridEX.EditControls.EditBox();
			this.label4 = new System.Windows.Forms.Label();
			this.editBox5 = new Janus.Windows.GridEX.EditControls.EditBox();
			this.label5 = new System.Windows.Forms.Label();
			((System.ComponentModel.ISupportInitialize)(this.uiPanelManager1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.uiPanel0)).BeginInit();
			this.uiPanel0.SuspendLayout();
			this.uiPanel0Container.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.uiPanel1)).BeginInit();
			this.uiPanel1.SuspendLayout();
			this.uiPanel1Container.SuspendLayout();
			this.pnlSearch.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.grdExMediaList)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.uiGroupBox1)).BeginInit();
			this.uiGroupBox1.SuspendLayout();
			this.SuspendLayout();
			// 
			// uiPanelManager1
			// 
			this.uiPanelManager1.ContainerControl = this;
			this.uiPanel0.Id = new System.Guid("9aa2f51a-69c7-40dd-83dc-8025739bb52c");
			this.uiPanelManager1.Panels.Add(this.uiPanel0);
			this.uiPanel1.Id = new System.Guid("0ec555e9-738b-4189-8f6a-96da4065acba");
			this.uiPanelManager1.Panels.Add(this.uiPanel1);
			// 
			// Design Time Panel Info:
			// 
			this.uiPanelManager1.BeginPanelInfo();
			this.uiPanelManager1.AddDockPanelInfo(new System.Guid("9aa2f51a-69c7-40dd-83dc-8025739bb52c"), Janus.Windows.UI.Dock.PanelDockStyle.Top, new System.Drawing.Size(1004, 157), true);
			this.uiPanelManager1.AddDockPanelInfo(new System.Guid("0ec555e9-738b-4189-8f6a-96da4065acba"), Janus.Windows.UI.Dock.PanelDockStyle.Fill, new System.Drawing.Size(1004, 511), true);
			this.uiPanelManager1.EndPanelInfo();
			// 
			// uiPanel0
			// 
			this.uiPanel0.InnerContainer = this.uiPanel0Container;
			this.uiPanel0.Location = new System.Drawing.Point(3, 3);
			this.uiPanel0.Name = "uiPanel0";
			this.uiPanel0.Size = new System.Drawing.Size(1004, 157);
			this.uiPanel0.TabIndex = 4;
			this.uiPanel0.Text = "가구별 시청행태";
			// 
			// uiPanel0Container
			// 
			this.uiPanel0Container.Controls.Add(this.pnlSearch);
			this.uiPanel0Container.Location = new System.Drawing.Point(1, 23);
			this.uiPanel0Container.Name = "uiPanel0Container";
			this.uiPanel0Container.Size = new System.Drawing.Size(1002, 129);
			this.uiPanel0Container.TabIndex = 0;
			// 
			// uiPanel1
			// 
			this.uiPanel1.InnerContainer = this.uiPanel1Container;
			this.uiPanel1.Location = new System.Drawing.Point(3, 160);
			this.uiPanel1.Name = "uiPanel1";
			this.uiPanel1.Size = new System.Drawing.Size(1004, 511);
			this.uiPanel1.TabIndex = 4;
			this.uiPanel1.Text = "시청행태 보기";
			// 
			// uiPanel1Container
			// 
			this.uiPanel1Container.Controls.Add(this.grdExMediaList);
			this.uiPanel1Container.Location = new System.Drawing.Point(1, 23);
			this.uiPanel1Container.Name = "uiPanel1Container";
			this.uiPanel1Container.Size = new System.Drawing.Size(1002, 487);
			this.uiPanel1Container.TabIndex = 0;
			// 
			// pnlSearch
			// 
			this.pnlSearch.BackColor = System.Drawing.SystemColors.Window;
			this.pnlSearch.Controls.Add(this.uiGroupBox1);
			this.pnlSearch.Controls.Add(this.lbUserTell);
			this.pnlSearch.Controls.Add(this.uiCheckBox3);
			this.pnlSearch.Controls.Add(this.ebSearchKey);
			this.pnlSearch.Controls.Add(this.btnSearch);
			this.pnlSearch.Dock = System.Windows.Forms.DockStyle.Fill;
			this.pnlSearch.Location = new System.Drawing.Point(0, 0);
			this.pnlSearch.Name = "pnlSearch";
			this.pnlSearch.Size = new System.Drawing.Size(1002, 129);
			this.pnlSearch.TabIndex = 1;
			// 
			// uiCheckBox3
			// 
			this.uiCheckBox3.Location = new System.Drawing.Point(339, 9);
			this.uiCheckBox3.Name = "uiCheckBox3";
			this.uiCheckBox3.Size = new System.Drawing.Size(104, 23);
			this.uiCheckBox3.TabIndex = 52;
			this.uiCheckBox3.Text = "사용안함 포함";
			// 
			// ebSearchKey
			// 
			this.ebSearchKey.Location = new System.Drawing.Point(115, 9);
			this.ebSearchKey.Name = "ebSearchKey";
			this.ebSearchKey.Size = new System.Drawing.Size(208, 21);
			this.ebSearchKey.TabIndex = 1;
			this.ebSearchKey.Text = "검색어를 입력하세요";
			this.ebSearchKey.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
			this.ebSearchKey.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
			// 
			// btnSearch
			// 
			this.btnSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnSearch.Enabled = false;
			this.btnSearch.Location = new System.Drawing.Point(887, 8);
			this.btnSearch.Name = "btnSearch";
			this.btnSearch.Size = new System.Drawing.Size(104, 24);
			this.btnSearch.TabIndex = 3;
			this.btnSearch.Text = "조 회";
			this.btnSearch.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
			// 
			// lbUserTell
			// 
			this.lbUserTell.BackColor = System.Drawing.SystemColors.Window;
			this.lbUserTell.Location = new System.Drawing.Point(36, 12);
			this.lbUserTell.Name = "lbUserTell";
			this.lbUserTell.Size = new System.Drawing.Size(72, 16);
			this.lbUserTell.TabIndex = 53;
			this.lbUserTell.Text = "가구찾기";
			// 
			// grdExMediaList
			// 
			this.grdExMediaList.AllowEdit = Janus.Windows.GridEX.InheritableBoolean.False;
			this.grdExMediaList.AlternatingColors = true;
			this.grdExMediaList.AlternatingRowFormatStyle.BackgroundImageDrawMode = Janus.Windows.GridEX.BackgroundImageDrawMode.Tile;
			this.grdExMediaList.BorderStyle = Janus.Windows.GridEX.BorderStyle.None;
			grdExMediaList_DesignTimeLayout.LayoutString = resources.GetString("grdExMediaList_DesignTimeLayout.LayoutString");
			this.grdExMediaList.DesignTimeLayout = grdExMediaList_DesignTimeLayout;
			this.grdExMediaList.Dock = System.Windows.Forms.DockStyle.Fill;
			this.grdExMediaList.EditorsControlStyle.ButtonAppearance = Janus.Windows.GridEX.ButtonAppearance.Regular;
			this.grdExMediaList.EmptyRows = true;
			this.grdExMediaList.FocusCellFormatStyle.BackColor = System.Drawing.SystemColors.Highlight;
			this.grdExMediaList.FocusCellFormatStyle.ForeColor = System.Drawing.Color.White;
			this.grdExMediaList.FocusStyle = Janus.Windows.GridEX.FocusStyle.None;
			this.grdExMediaList.FrozenColumns = 2;
			this.grdExMediaList.GridLineColor = System.Drawing.Color.Silver;
			this.grdExMediaList.GridLines = Janus.Windows.GridEX.GridLines.Vertical;
			this.grdExMediaList.GridLineStyle = Janus.Windows.GridEX.GridLineStyle.Solid;
			this.grdExMediaList.GroupByBoxVisible = false;
			this.grdExMediaList.HeaderFormatStyle.TextAlignment = Janus.Windows.GridEX.TextAlignment.Center;
			this.grdExMediaList.HideSelection = Janus.Windows.GridEX.HideSelection.HighlightInactive;
			this.grdExMediaList.Location = new System.Drawing.Point(0, 0);
			this.grdExMediaList.Name = "grdExMediaList";
			this.grdExMediaList.ScrollBars = Janus.Windows.GridEX.ScrollBars.Vertical;
			this.grdExMediaList.SelectedInactiveFormatStyle.BackColor = System.Drawing.Color.Gold;
			this.grdExMediaList.Size = new System.Drawing.Size(1002, 487);
			this.grdExMediaList.TabIndex = 5;
			this.grdExMediaList.TabKeyBehavior = Janus.Windows.GridEX.TabKeyBehavior.ControlNavigation;
			this.grdExMediaList.ThemedAreas = ((Janus.Windows.GridEX.ThemedArea)(((((((((Janus.Windows.GridEX.ThemedArea.ScrollBars | Janus.Windows.GridEX.ThemedArea.EditControls) 
            | Janus.Windows.GridEX.ThemedArea.Headers) 
            | Janus.Windows.GridEX.ThemedArea.GroupByBox) 
            | Janus.Windows.GridEX.ThemedArea.GroupRows) 
            | Janus.Windows.GridEX.ThemedArea.ControlBorder) 
            | Janus.Windows.GridEX.ThemedArea.Cards) 
            | Janus.Windows.GridEX.ThemedArea.Gridlines) 
            | Janus.Windows.GridEX.ThemedArea.CheckBoxes)));
			this.grdExMediaList.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
			// 
			// uiGroupBox1
			// 
			this.uiGroupBox1.Controls.Add(this.editBox4);
			this.uiGroupBox1.Controls.Add(this.label4);
			this.uiGroupBox1.Controls.Add(this.editBox5);
			this.uiGroupBox1.Controls.Add(this.label5);
			this.uiGroupBox1.Controls.Add(this.editBox3);
			this.uiGroupBox1.Controls.Add(this.label3);
			this.uiGroupBox1.Controls.Add(this.editBox2);
			this.uiGroupBox1.Controls.Add(this.label2);
			this.uiGroupBox1.Controls.Add(this.editBox1);
			this.uiGroupBox1.Controls.Add(this.label1);
			this.uiGroupBox1.Location = new System.Drawing.Point(26, 43);
			this.uiGroupBox1.Name = "uiGroupBox1";
			this.uiGroupBox1.Size = new System.Drawing.Size(964, 77);
			this.uiGroupBox1.TabIndex = 54;
			this.uiGroupBox1.Text = "가구및 구성원 정보";
			// 
			// label1
			// 
			this.label1.BackColor = System.Drawing.SystemColors.Window;
			this.label1.Location = new System.Drawing.Point(30, 20);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(72, 16);
			this.label1.TabIndex = 54;
			this.label1.Text = "가입자 정보";
			// 
			// editBox1
			// 
			this.editBox1.Location = new System.Drawing.Point(108, 17);
			this.editBox1.Name = "editBox1";
			this.editBox1.Size = new System.Drawing.Size(213, 21);
			this.editBox1.TabIndex = 55;
			this.editBox1.Text = "서울시 동작구 사당4동, 남성, 45세";
			this.editBox1.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
			this.editBox1.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
			// 
			// editBox2
			// 
			this.editBox2.Location = new System.Drawing.Point(718, 17);
			this.editBox2.Name = "editBox2";
			this.editBox2.Size = new System.Drawing.Size(213, 21);
			this.editBox2.TabIndex = 57;
			this.editBox2.Text = "여성, 10대";
			this.editBox2.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
			this.editBox2.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
			// 
			// label2
			// 
			this.label2.BackColor = System.Drawing.SystemColors.Window;
			this.label2.Location = new System.Drawing.Point(640, 20);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(72, 16);
			this.label2.TabIndex = 56;
			this.label2.Text = "세대원3 정보";
			// 
			// editBox3
			// 
			this.editBox3.Location = new System.Drawing.Point(440, 17);
			this.editBox3.Name = "editBox3";
			this.editBox3.Size = new System.Drawing.Size(167, 21);
			this.editBox3.TabIndex = 59;
			this.editBox3.Text = "남성, 40대";
			this.editBox3.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
			this.editBox3.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
			// 
			// label3
			// 
			this.label3.BackColor = System.Drawing.SystemColors.Window;
			this.label3.Location = new System.Drawing.Point(362, 20);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(72, 16);
			this.label3.TabIndex = 58;
			this.label3.Text = "세대원1 정보";
			// 
			// editBox4
			// 
			this.editBox4.Location = new System.Drawing.Point(440, 44);
			this.editBox4.Name = "editBox4";
			this.editBox4.Size = new System.Drawing.Size(167, 21);
			this.editBox4.TabIndex = 63;
			this.editBox4.Text = "여성, 30대";
			this.editBox4.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
			this.editBox4.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
			// 
			// label4
			// 
			this.label4.BackColor = System.Drawing.SystemColors.Window;
			this.label4.Location = new System.Drawing.Point(362, 47);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(72, 16);
			this.label4.TabIndex = 62;
			this.label4.Text = "세대원2 정보";
			// 
			// editBox5
			// 
			this.editBox5.Location = new System.Drawing.Point(718, 44);
			this.editBox5.Name = "editBox5";
			this.editBox5.Size = new System.Drawing.Size(213, 21);
			this.editBox5.TabIndex = 61;
			this.editBox5.Text = "미등록";
			this.editBox5.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
			this.editBox5.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
			// 
			// label5
			// 
			this.label5.BackColor = System.Drawing.SystemColors.Window;
			this.label5.Location = new System.Drawing.Point(640, 47);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(72, 16);
			this.label5.TabIndex = 60;
			this.label5.Text = "세대원4 정보";
			// 
			// UserControl1
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.Controls.Add(this.uiPanel1);
			this.Controls.Add(this.uiPanel0);
			this.Font = new System.Drawing.Font("나눔고딕", 8.999999F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
			this.Name = "UserControl1";
			this.Size = new System.Drawing.Size(1010, 674);
			((System.ComponentModel.ISupportInitialize)(this.uiPanelManager1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.uiPanel0)).EndInit();
			this.uiPanel0.ResumeLayout(false);
			this.uiPanel0Container.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.uiPanel1)).EndInit();
			this.uiPanel1.ResumeLayout(false);
			this.uiPanel1Container.ResumeLayout(false);
			this.pnlSearch.ResumeLayout(false);
			this.pnlSearch.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.grdExMediaList)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.uiGroupBox1)).EndInit();
			this.uiGroupBox1.ResumeLayout(false);
			this.uiGroupBox1.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private Janus.Windows.UI.Dock.UIPanelManager uiPanelManager1;
		private Janus.Windows.UI.Dock.UIPanel uiPanel0;
		private Janus.Windows.UI.Dock.UIPanelInnerContainer uiPanel0Container;
		private Janus.Windows.UI.Dock.UIPanel uiPanel1;
		private Janus.Windows.UI.Dock.UIPanelInnerContainer uiPanel1Container;
		private System.Windows.Forms.Panel pnlSearch;
		private Janus.Windows.EditControls.UICheckBox uiCheckBox3;
		private Janus.Windows.GridEX.EditControls.EditBox ebSearchKey;
		private Janus.Windows.EditControls.UIButton btnSearch;
		private System.Windows.Forms.Label lbUserTell;
		private Janus.Windows.GridEX.GridEX grdExMediaList;
		private Janus.Windows.EditControls.UIGroupBox uiGroupBox1;
		private Janus.Windows.GridEX.EditControls.EditBox editBox4;
		private System.Windows.Forms.Label label4;
		private Janus.Windows.GridEX.EditControls.EditBox editBox5;
		private System.Windows.Forms.Label label5;
		private Janus.Windows.GridEX.EditControls.EditBox editBox3;
		private System.Windows.Forms.Label label3;
		private Janus.Windows.GridEX.EditControls.EditBox editBox2;
		private System.Windows.Forms.Label label2;
		private Janus.Windows.GridEX.EditControls.EditBox editBox1;
		private System.Windows.Forms.Label label1;
	}
}
