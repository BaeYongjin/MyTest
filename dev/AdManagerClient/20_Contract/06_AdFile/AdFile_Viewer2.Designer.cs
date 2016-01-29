namespace AdManagerClient.AdFile
{
    partial class AdFile_Viewer2
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
                if (vlcPlayer != null)
                    vlcPlayer.Uninitialize();
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
            this.stlabel = new System.Windows.Forms.StatusStrip();
            this.stlabelTitle = new System.Windows.Forms.ToolStripStatusLabel();
            this.stlabelDesc = new System.Windows.Forms.ToolStripStatusLabel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.lbTimeSpan = new System.Windows.Forms.Label();
            this.lbFileFlow = new System.Windows.Forms.Label();
            this.tbVolume = new System.Windows.Forms.TrackBar();
            this.vlcPlayer = new DZ.MediaPlayer.Vlc.WindowsForms.VlcPlayerControl();
            this.btnDown = new Janus.Windows.EditControls.UIButton();
            this.btnPlaynPause = new Janus.Windows.EditControls.UIButton();
            this.btnStop = new Janus.Windows.EditControls.UIButton();
            this.btnClose = new Janus.Windows.EditControls.UIButton();
            this.pgBarPlay = new Janus.Windows.EditControls.UIProgressBar();
            this.progBar = new Janus.Windows.EditControls.UIProgressBar();
            this.stlabel.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tbVolume)).BeginInit();
            this.SuspendLayout();
            // 
            // stlabel
            // 
            this.stlabel.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.stlabelTitle,
            this.stlabelDesc});
            this.stlabel.Location = new System.Drawing.Point(0, 550);
            this.stlabel.Name = "stlabel";
            this.stlabel.Size = new System.Drawing.Size(720, 26);
            this.stlabel.TabIndex = 0;
            this.stlabel.Text = "Status";
            // 
            // stlabelTitle
            // 
            this.stlabelTitle.Name = "stlabelTitle";
            this.stlabelTitle.Size = new System.Drawing.Size(34, 21);
            this.stlabelTitle.Text = "상태:";
            this.stlabelTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // stlabelDesc
            // 
            this.stlabelDesc.AutoSize = false;
            this.stlabelDesc.Name = "stlabelDesc";
            this.stlabelDesc.Size = new System.Drawing.Size(500, 21);
            this.stlabelDesc.Text = "상태설명";
            this.stlabelDesc.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.White;
            this.panel1.Controls.Add(this.progBar);
            this.panel1.Controls.Add(this.pgBarPlay);
            this.panel1.Controls.Add(this.btnClose);
            this.panel1.Controls.Add(this.btnStop);
            this.panel1.Controls.Add(this.btnPlaynPause);
            this.panel1.Controls.Add(this.btnDown);
            this.panel1.Controls.Add(this.lbTimeSpan);
            this.panel1.Controls.Add(this.lbFileFlow);
            this.panel1.Controls.Add(this.tbVolume);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 480);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(720, 70);
            this.panel1.TabIndex = 1;
            // 
            // lbTimeSpan
            // 
            this.lbTimeSpan.Location = new System.Drawing.Point(200, 14);
            this.lbTimeSpan.Name = "lbTimeSpan";
            this.lbTimeSpan.Size = new System.Drawing.Size(59, 15);
            this.lbTimeSpan.TabIndex = 8;
            this.lbTimeSpan.Text = "00:00";
            this.lbTimeSpan.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lbFileFlow
            // 
            this.lbFileFlow.Location = new System.Drawing.Point(93, 14);
            this.lbFileFlow.Name = "lbFileFlow";
            this.lbFileFlow.Size = new System.Drawing.Size(100, 15);
            this.lbFileFlow.TabIndex = 7;
            this.lbFileFlow.Text = "0/0";
            this.lbFileFlow.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tbVolume
            // 
            this.tbVolume.LargeChange = 10;
            this.tbVolume.Location = new System.Drawing.Point(589, 5);
            this.tbVolume.Margin = new System.Windows.Forms.Padding(1);
            this.tbVolume.Maximum = 15;
            this.tbVolume.Name = "tbVolume";
            this.tbVolume.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.tbVolume.Size = new System.Drawing.Size(45, 63);
            this.tbVolume.TabIndex = 4;
            this.tbVolume.TickStyle = System.Windows.Forms.TickStyle.Both;
            this.tbVolume.ValueChanged += new System.EventHandler(this.tbVolume_ValueChanged);
            // 
            // vlcPlayer
            // 
            this.vlcPlayer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.vlcPlayer.Location = new System.Drawing.Point(0, 0);
            this.vlcPlayer.Name = "vlcPlayer";
            this.vlcPlayer.Position = 0D;
            this.vlcPlayer.Size = new System.Drawing.Size(720, 480);
            this.vlcPlayer.TabIndex = 2;
            this.vlcPlayer.Time = System.TimeSpan.Parse("00:00:00");
            this.vlcPlayer.Volume = 50;
            // 
            // btnDown
            // 
            this.btnDown.Location = new System.Drawing.Point(12, 7);
            this.btnDown.Name = "btnDown";
            this.btnDown.Size = new System.Drawing.Size(75, 27);
            this.btnDown.TabIndex = 9;
            this.btnDown.Text = "다운로드";
            this.btnDown.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.btnDown.Click += new System.EventHandler(this.btnDown_Click);
            // 
            // btnPlaynPause
            // 
            this.btnPlaynPause.Location = new System.Drawing.Point(12, 37);
            this.btnPlaynPause.Name = "btnPlaynPause";
            this.btnPlaynPause.Size = new System.Drawing.Size(75, 27);
            this.btnPlaynPause.TabIndex = 10;
            this.btnPlaynPause.Text = "Play";
            this.btnPlaynPause.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.btnPlaynPause.Click += new System.EventHandler(this.btnPlaynPause_Click);
            // 
            // btnStop
            // 
            this.btnStop.Location = new System.Drawing.Point(93, 37);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(75, 27);
            this.btnStop.TabIndex = 11;
            this.btnStop.Text = "Stop";
            this.btnStop.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(642, 37);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 27);
            this.btnClose.TabIndex = 12;
            this.btnClose.Text = "닫기";
            this.btnClose.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // pgBarPlay
            // 
            this.pgBarPlay.Location = new System.Drawing.Point(265, 41);
            this.pgBarPlay.Maximum = 1000;
            this.pgBarPlay.Name = "pgBarPlay";
            this.pgBarPlay.ShowPercentage = true;
            this.pgBarPlay.Size = new System.Drawing.Size(320, 18);
            this.pgBarPlay.TabIndex = 14;
            this.pgBarPlay.Visible = false;
            this.pgBarPlay.VisualStyle = Janus.Windows.UI.VisualStyle.VS2005;
            // 
            // progBar
            // 
            this.progBar.Location = new System.Drawing.Point(265, 14);
            this.progBar.Maximum = 1000;
            this.progBar.Name = "progBar";
            this.progBar.ShowPercentage = true;
            this.progBar.Size = new System.Drawing.Size(320, 18);
            this.progBar.TabIndex = 15;
            this.progBar.Visible = false;
            this.progBar.VisualStyle = Janus.Windows.UI.VisualStyle.VS2005;
            // 
            // AdFile_Viewer2
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(720, 576);
            this.Controls.Add(this.vlcPlayer);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.stlabel);
            this.Font = new System.Drawing.Font("나눔고딕", 8.999999F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "AdFile_Viewer2";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "동영상 재생";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.AdFile_Viewer2_Load);
            this.stlabel.ResumeLayout(false);
            this.stlabel.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tbVolume)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.StatusStrip stlabel;
        private System.Windows.Forms.ToolStripStatusLabel stlabelTitle;
        private System.Windows.Forms.ToolStripStatusLabel stlabelDesc;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TrackBar tbVolume;
        private DZ.MediaPlayer.Vlc.WindowsForms.VlcPlayerControl vlcPlayer;
        private System.Windows.Forms.Label lbFileFlow;
        private System.Windows.Forms.Label lbTimeSpan;
        private Janus.Windows.EditControls.UIButton btnDown;
        private Janus.Windows.EditControls.UIButton btnPlaynPause;
        private Janus.Windows.EditControls.UIButton btnStop;
        private Janus.Windows.EditControls.UIButton btnClose;
        private Janus.Windows.EditControls.UIProgressBar pgBarPlay;
        private Janus.Windows.EditControls.UIProgressBar progBar;
    }
}