namespace AdManagerClient
{
    partial class SchExclusiveZoneAdControl
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
			Janus.Windows.TimeLine.TimeLineLayout timeLineEx_DesignTimeLayout = new Janus.Windows.TimeLine.TimeLineLayout();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SchExclusiveZoneAdControl));
			this.schExclusiveZoneAdDs = new AdManagerClient.SchExclusiveZoneAdDs();
			this.uiPM = new Janus.Windows.UI.Dock.UIPanelManager(this.components);
			this.uiPanel0 = new Janus.Windows.UI.Dock.UIPanelGroup();
			this.uiPanelMiddle = new Janus.Windows.UI.Dock.UIPanel();
			this.uiPanel2Container = new Janus.Windows.UI.Dock.UIPanelInnerContainer();
			this.timeLineEx = new Janus.Windows.TimeLine.TimeLine();
			this.uiPanelBottom = new Janus.Windows.UI.Dock.UIPanel();
			this.uiPanel3Container = new Janus.Windows.UI.Dock.UIPanelInnerContainer();
			this.panel2 = new System.Windows.Forms.Panel();
			this.btnAdd = new Janus.Windows.EditControls.UIButton();
			((System.ComponentModel.ISupportInitialize)(this.schExclusiveZoneAdDs)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.uiPM)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.uiPanel0)).BeginInit();
			this.uiPanel0.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.uiPanelMiddle)).BeginInit();
			this.uiPanelMiddle.SuspendLayout();
			this.uiPanel2Container.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.timeLineEx)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.uiPanelBottom)).BeginInit();
			this.uiPanelBottom.SuspendLayout();
			this.uiPanel3Container.SuspendLayout();
			this.panel2.SuspendLayout();
			this.SuspendLayout();
			// 
			// schExclusiveZoneAdDs
			// 
			this.schExclusiveZoneAdDs.DataSetName = "SchExclusiveZoneAdDs";
			this.schExclusiveZoneAdDs.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
			// 
			// uiPM
			// 
			this.uiPM.BackColorGradientAutoHideStrip = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(218)))), ((int)(((byte)(250)))));
			this.uiPM.ContainerControl = this;
			this.uiPM.VisualStyle = Janus.Windows.UI.Dock.PanelVisualStyle.Office2007;
			this.uiPanel0.Id = new System.Guid("778d49c1-98b1-46ca-a325-81ee853a2823");
			this.uiPanel0.StaticGroup = true;
			this.uiPanelMiddle.Id = new System.Guid("9ef54050-7e87-4971-b80e-8cb889dbc042");
			this.uiPanel0.Panels.Add(this.uiPanelMiddle);
			this.uiPanelBottom.Id = new System.Guid("192ade6d-8b2b-422f-b2d7-0172e1ff5df5");
			this.uiPanel0.Panels.Add(this.uiPanelBottom);
			this.uiPM.Panels.Add(this.uiPanel0);
			// 
			// Design Time Panel Info:
			// 
			this.uiPM.BeginPanelInfo();
			this.uiPM.AddDockPanelInfo(new System.Guid("778d49c1-98b1-46ca-a325-81ee853a2823"), Janus.Windows.UI.Dock.PanelGroupStyle.HorizontalTiles, Janus.Windows.UI.Dock.PanelDockStyle.Fill, true, new System.Drawing.Size(1004, 671), true);
			this.uiPM.AddDockPanelInfo(new System.Guid("9ef54050-7e87-4971-b80e-8cb889dbc042"), new System.Guid("778d49c1-98b1-46ca-a325-81ee853a2823"), 589, true);
			this.uiPM.AddDockPanelInfo(new System.Guid("192ade6d-8b2b-422f-b2d7-0172e1ff5df5"), new System.Guid("778d49c1-98b1-46ca-a325-81ee853a2823"), 56, true);
			this.uiPM.AddFloatingPanelInfo(new System.Guid("778d49c1-98b1-46ca-a325-81ee853a2823"), Janus.Windows.UI.Dock.PanelGroupStyle.HorizontalTiles, true, new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
			this.uiPM.AddFloatingPanelInfo(new System.Guid("9ef54050-7e87-4971-b80e-8cb889dbc042"), new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
			this.uiPM.AddFloatingPanelInfo(new System.Guid("192ade6d-8b2b-422f-b2d7-0172e1ff5df5"), new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
			this.uiPM.EndPanelInfo();
			// 
			// uiPanel0
			// 
			this.uiPanel0.ActiveCaptionMode = Janus.Windows.UI.Dock.ActiveCaptionMode.Never;
			this.uiPanel0.BackColor = System.Drawing.SystemColors.Window;
			this.uiPanel0.CloseButtonVisible = Janus.Windows.UI.InheritableBoolean.False;
			this.uiPanel0.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
			this.uiPanel0.Location = new System.Drawing.Point(3, 3);
			this.uiPanel0.Name = "uiPanel0";
			this.uiPanel0.Size = new System.Drawing.Size(1004, 671);
			this.uiPanel0.TabIndex = 4;
			this.uiPanel0.Text = "시간대 독점 편성";
			// 
			// uiPanelMiddle
			// 
			this.uiPanelMiddle.AllowResize = Janus.Windows.UI.InheritableBoolean.True;
			this.uiPanelMiddle.CaptionVisible = Janus.Windows.UI.InheritableBoolean.False;
			this.uiPanelMiddle.InnerContainer = this.uiPanel2Container;
			this.uiPanelMiddle.Location = new System.Drawing.Point(0, 22);
			this.uiPanelMiddle.Name = "uiPanelMiddle";
			this.uiPanelMiddle.Size = new System.Drawing.Size(1004, 589);
			this.uiPanelMiddle.TabIndex = 4;
			this.uiPanelMiddle.Text = "시간대독점화면";
			// 
			// uiPanel2Container
			// 
			this.uiPanel2Container.Controls.Add(this.timeLineEx);
			this.uiPanel2Container.Location = new System.Drawing.Point(1, 1);
			this.uiPanel2Container.Name = "uiPanel2Container";
			this.uiPanel2Container.Size = new System.Drawing.Size(1002, 503);
			this.uiPanel2Container.TabIndex = 0;
			// 
			// timeLineEx
			// 
			this.timeLineEx.AllowEdit = false;
			this.timeLineEx.BottomTier.CustomFormat = "HH";
			this.timeLineEx.BottomTier.Font = new System.Drawing.Font("맑은 고딕", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.timeLineEx.BottomTier.Interval = Janus.Windows.TimeLine.TimeLineInterval.Hour;
			this.timeLineEx.DataMember = "SchExclusiveZoneList";
			this.timeLineEx.DataSource = this.schExclusiveZoneAdDs;
			timeLineEx_DesignTimeLayout.DataMember = "SchExclusiveZoneList";
			timeLineEx_DesignTimeLayout.DataSource = this.schExclusiveZoneAdDs;
			timeLineEx_DesignTimeLayout.LayoutString = resources.GetString("timeLineEx_DesignTimeLayout.LayoutString");
			this.timeLineEx.DesignTimeLayout = timeLineEx_DesignTimeLayout;
			this.timeLineEx.Dock = System.Windows.Forms.DockStyle.Fill;
			// 
			// 
			// 
			this.timeLineEx.DropDownCalendar.Name = "";
			this.timeLineEx.EndTimeMember = "EndDate";
			this.timeLineEx.FirstDate = new System.DateTime(2011, 12, 26, 0, 0, 0, 0);
			this.timeLineEx.FirstDayOfWeek = Janus.Windows.TimeLine.DayOfWeek.Monday;
			this.timeLineEx.HighlightCurrentTime = true;
			this.timeLineEx.IntervalSize = 60;
			this.timeLineEx.ItemsBarFormatStyle.Alpha = 200;
			this.timeLineEx.ItemsBarFormatStyle.BackColor = System.Drawing.Color.MediumSlateBlue;
			this.timeLineEx.ItemsBarFormatStyle.BackColorAlphaMode = Janus.Windows.TimeLine.AlphaMode.UseAlpha;
			this.timeLineEx.ItemsBarFormatStyle.BackColorGradient = System.Drawing.Color.FromArgb(((int)(((byte)(174)))), ((int)(((byte)(196)))), ((int)(((byte)(232)))));
			this.timeLineEx.ItemsBarFormatStyle.BackgroundGradientMode = Janus.Windows.TimeLine.BackgroundGradientMode.Vertical;
			this.timeLineEx.ItemsLineStyle = Janus.Windows.TimeLine.ItemsLineStyle.Solid;
			this.timeLineEx.Location = new System.Drawing.Point(0, 0);
			this.timeLineEx.MiddleTier.CustomFormat = "D";
			this.timeLineEx.MiddleTier.Interval = Janus.Windows.TimeLine.TimeLineInterval.Day;
			this.timeLineEx.MultiSelect = false;
			this.timeLineEx.Name = "timeLineEx";
			this.timeLineEx.Size = new System.Drawing.Size(1002, 503);
			this.timeLineEx.StartTimeMember = "StartDate";
			this.timeLineEx.TabIndex = 0;
			this.timeLineEx.Text = "timeLine1";
			this.timeLineEx.TextMember = "ItemName";
			this.timeLineEx.TimescaleTiers = Janus.Windows.TimeLine.TimescaleTiers.ThreeTiers;
			this.timeLineEx.TooltipFont = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
			this.timeLineEx.TopTier.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.timeLineEx.TopTier.Interval = Janus.Windows.TimeLine.TimeLineInterval.Week;
			this.timeLineEx.VisualStyle = Janus.Windows.TimeLine.VisualStyle.Office2007;
			this.timeLineEx.WorkEndTime = System.TimeSpan.Parse("23:59:59");
			this.timeLineEx.WorkStartTime = System.TimeSpan.Parse("00:00:00");
			this.timeLineEx.WorkWeek = ((Janus.Windows.TimeLine.DayOfWeek)(((((((Janus.Windows.TimeLine.DayOfWeek.Sunday | Janus.Windows.TimeLine.DayOfWeek.Monday) 
            | Janus.Windows.TimeLine.DayOfWeek.Tuesday) 
            | Janus.Windows.TimeLine.DayOfWeek.Wednesday) 
            | Janus.Windows.TimeLine.DayOfWeek.Thursday) 
            | Janus.Windows.TimeLine.DayOfWeek.Friday) 
            | Janus.Windows.TimeLine.DayOfWeek.Saturday)));
			this.timeLineEx.DoubleClick += new System.EventHandler(this.timeLineEx_DoubleClick);
			// 
			// uiPanelBottom
			// 
			this.uiPanelBottom.ActiveCaptionMode = Janus.Windows.UI.Dock.ActiveCaptionMode.Never;
			this.uiPanelBottom.AllowResize = Janus.Windows.UI.InheritableBoolean.True;
			this.uiPanelBottom.CaptionVisible = Janus.Windows.UI.InheritableBoolean.False;
			this.uiPanelBottom.InnerContainer = this.uiPanel3Container;
			this.uiPanelBottom.Location = new System.Drawing.Point(0, 615);
			this.uiPanelBottom.Name = "uiPanelBottom";
			this.uiPanelBottom.Size = new System.Drawing.Size(1004, 56);
			this.uiPanelBottom.TabIndex = 4;
			this.uiPanelBottom.Text = "하단패널";
			// 
			// uiPanel3Container
			// 
			this.uiPanel3Container.Controls.Add(this.panel2);
			this.uiPanel3Container.Location = new System.Drawing.Point(1, 1);
			this.uiPanel3Container.Name = "uiPanel3Container";
			this.uiPanel3Container.Size = new System.Drawing.Size(1002, 46);
			this.uiPanel3Container.TabIndex = 0;
			// 
			// panel2
			// 
			this.panel2.Controls.Add(this.btnAdd);
			this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel2.Location = new System.Drawing.Point(0, 0);
			this.panel2.Name = "panel2";
			this.panel2.Size = new System.Drawing.Size(1002, 46);
			this.panel2.TabIndex = 0;
			// 
			// btnAdd
			// 
			this.btnAdd.ButtonStyle = Janus.Windows.EditControls.ButtonStyle.Button;
			this.btnAdd.Cursor = System.Windows.Forms.Cursors.Hand;
			this.btnAdd.Enabled = false;
			this.btnAdd.Location = new System.Drawing.Point(8, 8);
			this.btnAdd.Name = "btnAdd";
			this.btnAdd.ShowFocusRectangle = false;
			this.btnAdd.Size = new System.Drawing.Size(104, 24);
			this.btnAdd.TabIndex = 1;
			this.btnAdd.Text = "추가";
			this.btnAdd.ToolTipText = "시간대 독점 편성할 광고 시간을 신규 추가 합니다.";
			this.btnAdd.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
			this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
			// 
			// SchExclusiveZoneAdControl
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.Controls.Add(this.uiPanel0);
			this.Font = global::AdManagerClient.Properties.Settings.Default.Font1;
			this.Name = "SchExclusiveZoneAdControl";
			this.Size = new System.Drawing.Size(1010, 677);
			this.Load += new System.EventHandler(this.SchExclusiveZoneAdControl_Load);
			((System.ComponentModel.ISupportInitialize)(this.schExclusiveZoneAdDs)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.uiPM)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.uiPanel0)).EndInit();
			this.uiPanel0.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.uiPanelMiddle)).EndInit();
			this.uiPanelMiddle.ResumeLayout(false);
			this.uiPanel2Container.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.timeLineEx)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.uiPanelBottom)).EndInit();
			this.uiPanelBottom.ResumeLayout(false);
			this.uiPanel3Container.ResumeLayout(false);
			this.panel2.ResumeLayout(false);
			this.ResumeLayout(false);

        }

        #endregion

        private Janus.Windows.UI.Dock.UIPanelManager uiPM;
        private Janus.Windows.UI.Dock.UIPanelGroup uiPanel0;
        private Janus.Windows.UI.Dock.UIPanel uiPanelMiddle;
        private Janus.Windows.UI.Dock.UIPanelInnerContainer uiPanel2Container;
        private Janus.Windows.TimeLine.TimeLine timeLineEx;
        private Janus.Windows.UI.Dock.UIPanel uiPanelBottom;
        private Janus.Windows.UI.Dock.UIPanelInnerContainer uiPanel3Container;
        private System.Windows.Forms.Panel panel2;
        private Janus.Windows.EditControls.UIButton btnAdd;
        private SchExclusiveZoneAdDs schExclusiveZoneAdDs;


    }
}
