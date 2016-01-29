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

namespace AdManagerClient
{
    /// <summary>
    /// SchChoiceAdSearchChannel_pForm�� ���� ��� �����Դϴ�.
    /// </summary>
    /// 

    public class SchChoiceAdSearchChannel_pForm : System.Windows.Forms.Form
    {

        #region �̺�Ʈ�ڵ鷯
        public event StatusEventHandler 			StatusEvent;			// �����̺�Ʈ �ڵ鷯
        #endregion
			
        #region ��������� ��ü �� ����

        // �ý��� ���� : ȭ�����
        private SystemModel   systemModel   = FrameSystem.oSysModel;
        private CommonModel   commonModel   = FrameSystem.oComModel;
        private Logger        log           = FrameSystem.oLog;

        // ����� ������
        ChannelGroupModel channelGroupModel  = new ChannelGroupModel();	// �帣������
		SchChoiceAdModel  schChoiceAdModel   = new SchChoiceAdModel();	// ������������

		// �� â�� �� ��Ʈ��
		SchChoiceAdControl Opener = null;
       

        // ȭ��ó���� ����
        public String MediaCode   = null;
        CurrencyManager cm        = null;					// ������ �׸����� ���濡 ���� �����ͼ� ������ ���Ͽ�
        CurrencyManager cmChannel = null;					// ������ �׸����� ���濡 ���� �����ͼ� ������ ���Ͽ�
        DataTable       dt        = null;
        DataTable       dtChannel = null;

        #endregion

		#region  ������ �Ҹ��� ��Ʈ�Ѽ���

        private System.Windows.Forms.Panel pnlBtn;
        private Janus.Windows.EditControls.UIButton btnClose;
        private Janus.Windows.EditControls.UIButton btnOk;
        private System.Data.DataView dvGenre;
        private System.Data.DataView dvChannel;
		private Janus.Windows.UI.Dock.UIPanelManager uiPM;
		private Janus.Windows.UI.Dock.UIPanelGroup uiPanel0;
		private Janus.Windows.UI.Dock.UIPanel uiPanel1;
		private Janus.Windows.UI.Dock.UIPanelInnerContainer uiPanel1Container;
		private Janus.Windows.UI.Dock.UIPanel uiPanel2;
		private Janus.Windows.UI.Dock.UIPanelInnerContainer uiPanel2Container;
		private Janus.Windows.GridEX.GridEX grdExGenreList;
		private AdManagerClient.SchChoiceAdSearch_pDs schChoiceAdSearch_pDs;
		private Janus.Windows.GridEX.GridEX grdExChannelList;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private Janus.Windows.GridEX.EditControls.EditBox ebChannelNo;
        private System.ComponentModel.IContainer components;

        /// <summary>
        /// ������ �Ѱܾ� �� ��
        /// </summary>
        /// <param name="sender"></param>
        public SchChoiceAdSearchChannel_pForm(UserControl parent)
        {
            //
            // Windows Form �����̳� ������ �ʿ��մϴ�.
            //
            InitializeComponent();

			// ��â�� ȣ���� ��Ʈ��
			Opener = (SchChoiceAdControl) parent;
			channelGroupModel.Init();
		}

        /// <summary>
        /// �Ϲݻ����
        /// </summary>
        public SchChoiceAdSearchChannel_pForm()
        {
            //
            // Windows Form �����̳� ������ �ʿ��մϴ�.
            //
            InitializeComponent();

			channelGroupModel.Init();
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

		#endregion

        #region Windows Form �����̳ʿ��� ������ �ڵ�
        /// <summary>
        /// �����̳� ������ �ʿ��� �޼����Դϴ�.
        /// �� �޼����� ������ �ڵ� ������� �������� ���ʽÿ�.
        /// </summary>
        private void InitializeComponent()
        {
			this.components = new System.ComponentModel.Container();
			Janus.Windows.GridEX.GridEXLayout grdExGenreList_Layout_0 = new Janus.Windows.GridEX.GridEXLayout();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SchChoiceAdSearchChannel_pForm));
			Janus.Windows.GridEX.GridEXLayout grdExChannelList_Layout_0 = new Janus.Windows.GridEX.GridEXLayout();
			this.dvGenre = new System.Data.DataView();
			this.schChoiceAdSearch_pDs = new AdManagerClient.SchChoiceAdSearch_pDs();
			this.dvChannel = new System.Data.DataView();
			this.pnlBtn = new System.Windows.Forms.Panel();
			this.label2 = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.ebChannelNo = new Janus.Windows.GridEX.EditControls.EditBox();
			this.btnClose = new Janus.Windows.EditControls.UIButton();
			this.btnOk = new Janus.Windows.EditControls.UIButton();
			this.uiPM = new Janus.Windows.UI.Dock.UIPanelManager(this.components);
			this.uiPanel0 = new Janus.Windows.UI.Dock.UIPanelGroup();
			this.uiPanel1 = new Janus.Windows.UI.Dock.UIPanel();
			this.uiPanel1Container = new Janus.Windows.UI.Dock.UIPanelInnerContainer();
			this.grdExGenreList = new Janus.Windows.GridEX.GridEX();
			this.uiPanel2 = new Janus.Windows.UI.Dock.UIPanel();
			this.uiPanel2Container = new Janus.Windows.UI.Dock.UIPanelInnerContainer();
			this.grdExChannelList = new Janus.Windows.GridEX.GridEX();
			((System.ComponentModel.ISupportInitialize)(this.dvGenre)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.schChoiceAdSearch_pDs)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.dvChannel)).BeginInit();
			this.pnlBtn.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.uiPM)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.uiPanel0)).BeginInit();
			this.uiPanel0.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.uiPanel1)).BeginInit();
			this.uiPanel1.SuspendLayout();
			this.uiPanel1Container.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.grdExGenreList)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.uiPanel2)).BeginInit();
			this.uiPanel2.SuspendLayout();
			this.uiPanel2Container.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.grdExChannelList)).BeginInit();
			this.SuspendLayout();
			// 
			// dvGenre
			// 
			this.dvGenre.Table = this.schChoiceAdSearch_pDs.Genre;
			// 
			// schChoiceAdSearch_pDs
			// 
			this.schChoiceAdSearch_pDs.DataSetName = "SchChoiceAdSearch_pDs";
			this.schChoiceAdSearch_pDs.Locale = new System.Globalization.CultureInfo("en-US");
			this.schChoiceAdSearch_pDs.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
			// 
			// dvChannel
			// 
			this.dvChannel.Table = this.schChoiceAdSearch_pDs.Channel;
			// 
			// pnlBtn
			// 
			this.pnlBtn.BackColor = System.Drawing.Color.WhiteSmoke;
			this.pnlBtn.Controls.Add(this.label2);
			this.pnlBtn.Controls.Add(this.label1);
			this.pnlBtn.Controls.Add(this.ebChannelNo);
			this.pnlBtn.Controls.Add(this.btnClose);
			this.pnlBtn.Controls.Add(this.btnOk);
			this.pnlBtn.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.pnlBtn.Location = new System.Drawing.Point(0, 426);
			this.pnlBtn.Name = "pnlBtn";
			this.pnlBtn.Size = new System.Drawing.Size(792, 40);
			this.pnlBtn.TabIndex = 16;
			// 
			// label2
			// 
			this.label2.BackColor = System.Drawing.Color.Transparent;
			this.label2.Location = new System.Drawing.Point(392, 8);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(72, 21);
			this.label2.TabIndex = 13;
			this.label2.Text = "ä�ι�ȣ";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label1
			// 
			this.label1.BackColor = System.Drawing.Color.Transparent;
			this.label1.Location = new System.Drawing.Point(8, 8);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(264, 21);
			this.label1.TabIndex = 12;
			this.label1.Text = "ä�ι�ȣ�� ���� �Է��ϼŵ� �� �����մϴ�.";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// ebChannelNo
			// 
			this.ebChannelNo.Location = new System.Drawing.Point(464, 8);
			this.ebChannelNo.MaxLength = 10;
			this.ebChannelNo.Name = "ebChannelNo";
			this.ebChannelNo.Size = new System.Drawing.Size(88, 21);
			this.ebChannelNo.TabIndex = 3;
			this.ebChannelNo.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
			this.ebChannelNo.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
			this.ebChannelNo.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ebChannelNo_KeyDown);
			// 
			// btnClose
			// 
			this.btnClose.BackColor = System.Drawing.SystemColors.Control;
			this.btnClose.Location = new System.Drawing.Point(680, 8);
			this.btnClose.Name = "btnClose";
			this.btnClose.Size = new System.Drawing.Size(104, 23);
			this.btnClose.TabIndex = 5;
			this.btnClose.Text = "�� ��";
			this.btnClose.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
			this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
			// 
			// btnOk
			// 
			this.btnOk.BackColor = System.Drawing.SystemColors.Control;
			this.btnOk.Location = new System.Drawing.Point(568, 8);
			this.btnOk.Name = "btnOk";
			this.btnOk.Size = new System.Drawing.Size(104, 23);
			this.btnOk.TabIndex = 4;
			this.btnOk.Text = "�� ��";
			this.btnOk.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
			this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
			// 
			// uiPM
			// 
			this.uiPM.BackColorGradientAutoHideStrip = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(218)))), ((int)(((byte)(250)))));
			this.uiPM.ContainerControl = this;
			this.uiPM.VisualStyle = Janus.Windows.UI.Dock.PanelVisualStyle.Office2007;
			this.uiPanel0.Id = new System.Guid("083e8f61-9d7d-4e6b-831f-be80f6b8df6d");
			this.uiPanel0.StaticGroup = true;
			this.uiPanel1.Id = new System.Guid("e04fffea-30bd-41ee-bac8-eb8c6dae0c44");
			this.uiPanel0.Panels.Add(this.uiPanel1);
			this.uiPanel2.Id = new System.Guid("af08113b-57a7-4b6c-8b5d-55bc6195bc72");
			this.uiPanel0.Panels.Add(this.uiPanel2);
			this.uiPM.Panels.Add(this.uiPanel0);
			// 
			// Design Time Panel Info:
			// 
			this.uiPM.BeginPanelInfo();
			this.uiPM.AddDockPanelInfo(new System.Guid("083e8f61-9d7d-4e6b-831f-be80f6b8df6d"), Janus.Windows.UI.Dock.PanelGroupStyle.VerticalTiles, Janus.Windows.UI.Dock.PanelDockStyle.Fill, true, new System.Drawing.Size(786, 420), true);
			this.uiPM.AddDockPanelInfo(new System.Guid("e04fffea-30bd-41ee-bac8-eb8c6dae0c44"), new System.Guid("083e8f61-9d7d-4e6b-831f-be80f6b8df6d"), 196, true);
			this.uiPM.AddDockPanelInfo(new System.Guid("af08113b-57a7-4b6c-8b5d-55bc6195bc72"), new System.Guid("083e8f61-9d7d-4e6b-831f-be80f6b8df6d"), 196, true);
			this.uiPM.AddFloatingPanelInfo(new System.Guid("083e8f61-9d7d-4e6b-831f-be80f6b8df6d"), Janus.Windows.UI.Dock.PanelGroupStyle.VerticalTiles, true, new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
			this.uiPM.AddFloatingPanelInfo(new System.Guid("e04fffea-30bd-41ee-bac8-eb8c6dae0c44"), new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
			this.uiPM.AddFloatingPanelInfo(new System.Guid("af08113b-57a7-4b6c-8b5d-55bc6195bc72"), new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
			this.uiPM.EndPanelInfo();
			// 
			// uiPanel0
			// 
			this.uiPanel0.CaptionVisible = Janus.Windows.UI.InheritableBoolean.False;
			this.uiPanel0.GroupStyle = Janus.Windows.UI.Dock.PanelGroupStyle.VerticalTiles;
			this.uiPanel0.Location = new System.Drawing.Point(3, 3);
			this.uiPanel0.Name = "uiPanel0";
			this.uiPanel0.Size = new System.Drawing.Size(786, 420);
			this.uiPanel0.TabIndex = 4;
			this.uiPanel0.Text = "����ä�α��� ä����";
			// 
			// uiPanel1
			// 
			this.uiPanel1.InnerContainer = this.uiPanel1Container;
			this.uiPanel1.Location = new System.Drawing.Point(0, 0);
			this.uiPanel1.Name = "uiPanel1";
			this.uiPanel1.Size = new System.Drawing.Size(391, 420);
			this.uiPanel1.TabIndex = 4;
			this.uiPanel1.Text = "�޴�";
			// 
			// uiPanel1Container
			// 
			this.uiPanel1Container.Controls.Add(this.grdExGenreList);
			this.uiPanel1Container.Location = new System.Drawing.Point(1, 23);
			this.uiPanel1Container.Name = "uiPanel1Container";
			this.uiPanel1Container.Size = new System.Drawing.Size(389, 396);
			this.uiPanel1Container.TabIndex = 0;
			// 
			// grdExGenreList
			// 
			this.grdExGenreList.AlternatingColors = true;
			this.grdExGenreList.AutomaticSort = false;
			this.grdExGenreList.BorderStyle = Janus.Windows.GridEX.BorderStyle.None;
			this.grdExGenreList.DataSource = this.dvGenre;
			this.grdExGenreList.Dock = System.Windows.Forms.DockStyle.Fill;
			this.grdExGenreList.EmptyRows = true;
			this.grdExGenreList.FocusStyle = Janus.Windows.GridEX.FocusStyle.None;
			this.grdExGenreList.GridLines = Janus.Windows.GridEX.GridLines.Vertical;
			this.grdExGenreList.GridLineStyle = Janus.Windows.GridEX.GridLineStyle.Solid;
			this.grdExGenreList.GroupByBoxVisible = false;
			this.grdExGenreList.GroupMode = Janus.Windows.GridEX.GroupMode.Collapsed;
			this.grdExGenreList.HideSelection = Janus.Windows.GridEX.HideSelection.HighlightInactive;
			grdExGenreList_Layout_0.DataSource = this.dvGenre;
			grdExGenreList_Layout_0.IsCurrentLayout = true;
			grdExGenreList_Layout_0.Key = "bae";
			grdExGenreList_Layout_0.LayoutString = resources.GetString("grdExGenreList_Layout_0.LayoutString");
			this.grdExGenreList.Layouts.AddRange(new Janus.Windows.GridEX.GridEXLayout[] {
            grdExGenreList_Layout_0});
			this.grdExGenreList.Location = new System.Drawing.Point(0, 0);
			this.grdExGenreList.Name = "grdExGenreList";
			this.grdExGenreList.ScrollBars = Janus.Windows.GridEX.ScrollBars.Vertical;
			this.grdExGenreList.SelectedInactiveFormatStyle.BackColor = System.Drawing.Color.Gold;
			this.grdExGenreList.Size = new System.Drawing.Size(389, 396);
			this.grdExGenreList.TabIndex = 1;
			this.grdExGenreList.TabKeyBehavior = Janus.Windows.GridEX.TabKeyBehavior.ControlNavigation;
			this.grdExGenreList.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
			// 
			// uiPanel2
			// 
			this.uiPanel2.InnerContainer = this.uiPanel2Container;
			this.uiPanel2.Location = new System.Drawing.Point(395, 0);
			this.uiPanel2.Name = "uiPanel2";
			this.uiPanel2.Size = new System.Drawing.Size(391, 420);
			this.uiPanel2.TabIndex = 4;
			this.uiPanel2.Text = "ä��";
			// 
			// uiPanel2Container
			// 
			this.uiPanel2Container.Controls.Add(this.grdExChannelList);
			this.uiPanel2Container.Location = new System.Drawing.Point(1, 23);
			this.uiPanel2Container.Name = "uiPanel2Container";
			this.uiPanel2Container.Size = new System.Drawing.Size(389, 396);
			this.uiPanel2Container.TabIndex = 0;
			// 
			// grdExChannelList
			// 
			this.grdExChannelList.AlternatingColors = true;
			this.grdExChannelList.BorderStyle = Janus.Windows.GridEX.BorderStyle.None;
			this.grdExChannelList.DataSource = this.dvChannel;
			this.grdExChannelList.Dock = System.Windows.Forms.DockStyle.Fill;
			this.grdExChannelList.EmptyRows = true;
			this.grdExChannelList.FocusStyle = Janus.Windows.GridEX.FocusStyle.None;
			this.grdExChannelList.GridLines = Janus.Windows.GridEX.GridLines.Vertical;
			this.grdExChannelList.GridLineStyle = Janus.Windows.GridEX.GridLineStyle.Solid;
			this.grdExChannelList.GroupByBoxVisible = false;
			this.grdExChannelList.GroupMode = Janus.Windows.GridEX.GroupMode.Collapsed;
			grdExChannelList_Layout_0.DataSource = this.dvChannel;
			grdExChannelList_Layout_0.IsCurrentLayout = true;
			grdExChannelList_Layout_0.Key = "bae";
			grdExChannelList_Layout_0.LayoutString = resources.GetString("grdExChannelList_Layout_0.LayoutString");
			this.grdExChannelList.Layouts.AddRange(new Janus.Windows.GridEX.GridEXLayout[] {
            grdExChannelList_Layout_0});
			this.grdExChannelList.Location = new System.Drawing.Point(0, 0);
			this.grdExChannelList.Name = "grdExChannelList";
			this.grdExChannelList.ScrollBars = Janus.Windows.GridEX.ScrollBars.Vertical;
			this.grdExChannelList.Size = new System.Drawing.Size(389, 396);
			this.grdExChannelList.TabIndex = 2;
			this.grdExChannelList.TabKeyBehavior = Janus.Windows.GridEX.TabKeyBehavior.ControlNavigation;
			this.grdExChannelList.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
			this.grdExChannelList.CellValueChanged += new Janus.Windows.GridEX.ColumnActionEventHandler(this.grdExChannelList_CellValueChanged);
			this.grdExChannelList.ColumnHeaderClick += new Janus.Windows.GridEX.ColumnActionEventHandler(this.grdExChannelList_ColumnHeaderClick);
			// 
			// SchChoiceAdSearchChannel_pForm
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(6, 14);
			this.ClientSize = new System.Drawing.Size(792, 466);
			this.Controls.Add(this.uiPanel0);
			this.Controls.Add(this.pnlBtn);
			this.Font = global::AdManagerClient.Properties.Settings.Default.Font1;
			this.Name = "SchChoiceAdSearchChannel_pForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "ä����";
			this.Load += new System.EventHandler(this.SchChoiceAdSearchChannel_pForm_Load);
			((System.ComponentModel.ISupportInitialize)(this.dvGenre)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.schChoiceAdSearch_pDs)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.dvChannel)).EndInit();
			this.pnlBtn.ResumeLayout(false);
			this.pnlBtn.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.uiPM)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.uiPanel0)).EndInit();
			this.uiPanel0.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.uiPanel1)).EndInit();
			this.uiPanel1.ResumeLayout(false);
			this.uiPanel1Container.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.grdExGenreList)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.uiPanel2)).EndInit();
			this.uiPanel2.ResumeLayout(false);
			this.uiPanel2Container.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.grdExChannelList)).EndInit();
			this.ResumeLayout(false);

		}
        #endregion

        #region ��Ʈ�� �ε�
        private void SchChoiceAdSearchChannel_pForm_Load(object sender, System.EventArgs e)
        {           
            // �����Ͱ����� ��ü����
            dt        = ((DataView)grdExGenreList.DataSource).Table;  
            dtChannel = ((DataView)grdExChannelList.DataSource).Table;  
            cm        = (CurrencyManager) this.BindingContext[grdExGenreList.DataSource]; 
            cm.PositionChanged += new System.EventHandler(OnGrdRowChanged);

            cmChannel = (CurrencyManager)this.BindingContext[grdExChannelList.DataSource];
            //cm.PositionChanged += new System.EventHandler(OnGrdRowChanged); 

            SearchGenreList();
        }
        #endregion

        #region ����� �׼�ó�� �޼ҵ�

		/// <summary>
		/// �׸����� Row�����
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void OnGrdRowChanged(object sender, System.EventArgs e) 
		{
			if(grdExGenreList.RecordCount > 0 )
			{
				SearchChannelList();
			}

		}
     
		private void btnClose_Click(object sender, System.EventArgs e)
		{
			this.Close();
		}

		private void btnOk_Click(object sender, System.EventArgs e)
		{
			AddSchChoiceAdDeailChannel();
		}

		private void btnSearch_Click(object sender, System.EventArgs e)
		{
			SearchGenreList();
		}

		private void grdExChannelList_ColumnHeaderClick(object sender, Janus.Windows.GridEX.ColumnActionEventArgs e)
		{
            
			//�÷�Index 0(üũ�ڽ��÷���)�� �ƴϸ� ���������� ó��.
			if(e.Column.Index != 0)
			{
				return;
			}
            
			//ColumnHeader Click�ÿ� dt Setting 
			DataRow[] foundRows = dtChannel.Select("CheckYn = 'False'");
         
			if( foundRows.Length == 0 )
			{
				for(int i=0;i < dtChannel.Rows.Count;i++)
				{
					dtChannel.Rows[i].BeginEdit();
					dtChannel.Rows[i]["CheckYn"]="False";
					dtChannel.Rows[i].EndEdit();
				}
			}
			else
			{
				for(int i=0;i < dtChannel.Rows.Count;i++)
				{
					dtChannel.Rows[i].BeginEdit();
					dtChannel.Rows[i]["CheckYn"]="True";
					dtChannel.Rows[i].EndEdit();
				}
			}
		}

		private void ebChannelNo_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
		{
			if(e.KeyCode == Keys.Enter)
			{
				AddSchChoiceAdDeailChannel();
			}		
		}

		#endregion
  
        #region ó���޼ҵ�

        /// <summary>
        /// �帣��� ��ȸ
        /// </summary>
        private void SearchGenreList()
        {
            StatusMessage("�帣 ������ ��ȸ�մϴ�.");

            try
            {
				channelGroupModel.Init();

				// �����͸𵨿� ������ ������ ��Ʈ�Ѵ�.
				channelGroupModel.MediaCode = Opener.MediaCode;

				if(channelGroupModel.MediaCode.Trim().Length == 0)
				{
					MessageBox.Show("�������� ���õ��� �ʾҽ��ϴ�.", "��ȸ����",
						MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				}

                // �帣�����ȸ ���񽺸� ȣ���Ѵ�.
                new ChannelGroupSearch_pManager(systemModel,commonModel).GetGenreList(channelGroupModel);

                if (channelGroupModel.ResultCD.Equals("0000"))
                {
                    Utility.SetDataTable(schChoiceAdSearch_pDs.Genre, channelGroupModel.ChannelGroup_pDataSet);				
                    StatusMessage(channelGroupModel.ResultCnt + "���� �帣 ������ ��ȸ�Ǿ����ϴ�.");
					SearchChannelList();
                }
            }
            catch(FrameException fe)
            {
                FrameSystem.showMsgForm("�帣��ȸ����", new string[] {fe.ErrCode, fe.ResultMsg});
            }
            catch(Exception ex)
            {
                FrameSystem.showMsgForm("�帣��ȸ����",new string[] {"",ex.Message});
            }
        }
        /// <summary>
        /// ä�θ�� ��ȸ
        /// </summary>
        private void SearchChannelList()
        {
            StatusMessage("ä�� ������ ��ȸ�մϴ�.");

			int curRow = cm.Position;

			if(curRow < 0) return;

            try
            {
				grdExChannelList.UnCheckAllRecords();

                // �����͸𵨿� ������ ������ ��Ʈ�Ѵ�.
                channelGroupModel.MediaCode    = dt.Rows[curRow]["MediaCode"].ToString();
                channelGroupModel.CategoryCode = dt.Rows[curRow]["CategoryCode"].ToString();
                channelGroupModel.GenreCode    = dt.Rows[curRow]["GenreCode"].ToString();  

                // ä�θ����ȸ ���񽺸� ȣ���Ѵ�.
                new ChannelGroupSearch_pManager(systemModel,commonModel).GetChannelList(channelGroupModel);

                if (channelGroupModel.ResultCD.Equals("0000"))
                {
                    Utility.SetDataTable(schChoiceAdSearch_pDs.Channel, channelGroupModel.ChannelGroup_pDataSet);				
                    StatusMessage(channelGroupModel.ResultCnt + "���� �帣 ������ ��ȸ�Ǿ����ϴ�.");
                }
            }
            catch(FrameException fe)
            {
                FrameSystem.showMsgForm("ä����ȸ����", new string[] {fe.ErrCode, fe.ResultMsg});
            }
            catch(Exception ex)
            {
                FrameSystem.showMsgForm("ä����ȸ����",new string[] {"",ex.Message});
            }
        }

		/// <summary>
		/// ���õ� ä�θ� ����ä�α��� �󼼳����� ��
		/// </summary>
		private void AddSchChoiceAdDeailChannel()
		{
			// �׸��忡 ����� �����͸� Datasource�� ������Ʈ �Ѵ�.
			grdExChannelList.UpdateData();

			try
			{
				int SetCount = 0;

				//�μ�Ʈ ��Ŵ
				for(int i=0;i < schChoiceAdSearch_pDs.Channel.Rows.Count;i++)
				{
					DataRow row = schChoiceAdSearch_pDs.Channel.Rows[i];

					if(row["CheckYn"].ToString().Equals("True"))
					{
						schChoiceAdModel.Init();

						schChoiceAdModel.ItemNo    = Opener.ItemNo;
						schChoiceAdModel.MediaCode = Opener.MediaCode;
						schChoiceAdModel.ChannelNo = row["ChannelNo"].ToString();
						schChoiceAdModel.Title     = row["Title"].ToString();
						schChoiceAdModel.AdType    = Opener.AdType;

						new SchChoiceAdManager(systemModel,commonModel).SetSchChoiceChannelDetailAdd(schChoiceAdModel);

						if(schChoiceAdModel.ResultCD.Equals("0000"))
						{
							SetCount++;
						}
					}
				}


				// ä�ι�ȣ �����Է��� �������
				if(ebChannelNo.Text.Trim().Length > 0)
				{
					schChoiceAdModel.Init();

					schChoiceAdModel.ItemNo    = Opener.ItemNo;
					schChoiceAdModel.MediaCode = Opener.MediaCode;
					schChoiceAdModel.ChannelNo = ebChannelNo.Text.Trim();
					schChoiceAdModel.AdType    = Opener.AdType;

					new SchChoiceAdManager(systemModel,commonModel).SetSchChoiceChannelDetailAdd(schChoiceAdModel);

					if(schChoiceAdModel.ResultCD.Equals("0000"))
					{
						SetCount++;
						ebChannelNo.Text = "";
					}
				}

				// üũ�Ȱ� Ŭ����
				ClearListCheck();

				// ȣ���� ��Ʈ���� �޴������ �����Ѵ�.
				Opener.ReloadChannelList();

				if(SetCount > 0)
				{
//					MessageBox.Show("�����Ͻ� ä�ο� ���Ǿ����ϴ�.", "ä�α�����",
//						MessageBoxButtons.OK, MessageBoxIcon.Information);
				}

			}
			catch(FrameException fe)
			{
				FrameSystem.showMsgForm("�����޴����� ��������", new string[] {fe.ErrCode, fe.ResultMsg});
			}
			catch(Exception ex)
			{
				FrameSystem.showMsgForm("�����޴����� ��������",new string[] {"",ex.Message});
			}			
		}

		private void ClearListCheck()
		{

			// üũ�� ��� �׸��� Ŭ����
			grdExChannelList.UnCheckAllRecords();
			grdExChannelList.UpdateData();
				   
			// ������ Ŭ����
			for(int i=0;i < dtChannel.Rows.Count;i++)
			{
				dtChannel.Rows[i].BeginEdit();
				dtChannel.Rows[i]["CheckYn"]="False";
				dtChannel.Rows[i].EndEdit();
			}
		}

        #endregion

        #region �̺�Ʈ�Լ�

		private void StatusMessage(string strMessage)
		{
			if (StatusEvent != null) 
			{
				StatusEventArgs ea = new StatusEventArgs();
				ea.Message   = strMessage;
				StatusEvent(this,ea);
			}
		}

        private void grdExChannelList_CellValueChanged(object sender, Janus.Windows.GridEX.ColumnActionEventArgs e)
        {
            if (e.Column.EditType == Janus.Windows.GridEX.EditType.CheckBox)
            {
                int curRow = cmChannel.Position;
                if (curRow >= 0)
                {
                    dtChannel.Rows[curRow].BeginEdit();
                    dtChannel.Rows[curRow]["CheckYn"] = grdExChannelList.GetValue(e.Column).ToString();
                    dtChannel.Rows[curRow].EndEdit();
                }
            }
        }
    }
    #endregion


}
