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
	/// ChannelNoPopForm1�� ���� ��� �����Դϴ�.
	/// </summary>
	/// 



	public class ChannelNoPopForm1 : System.Windows.Forms.Form
	{
		#region �̺�Ʈ�ڵ鷯
		public event StatusEventHandler 			StatusEvent;			// �����̺�Ʈ �ڵ鷯
        public event ProgressEventHandler           ProgressEvent;			// ó�����̺�Ʈ �ڵ鷯
		#endregion
			
		#region ��������� ��ü �� ����

		// �ý��� ���� : ȭ�����
		private SystemModel   systemModel   = FrameSystem.oSysModel;
		private CommonModel   commonModel   = FrameSystem.oComModel;
		private Logger        log           = FrameSystem.oLog;

		// ����� ������
		GroupModel groupModel  = new GroupModel();	// ������������
				
		GroupControl ChannelSetCtl = null;

		public string keyMedia		= "1";
        public string KeyCategory = "";
        public string KeyGenre = "";
        public string KeyCategoryName = "";
        public string KeyGenreName = "";

		// ȭ��ó���� ����
		bool IsNewSearchKey		  = true;					// �˻����Է� ����
		CurrencyManager cm        = null;					// ������ �׸����� ���濡 ���� �����ͼ� ������ ���Ͽ�			
		DataTable       dt        = null;

		#endregion

		private System.Windows.Forms.Panel panel4;
		private System.Windows.Forms.Panel panel1;
		private System.Data.DataView dvChannelSet;
		private System.Windows.Forms.Panel pnlBtn;
		private Janus.Windows.EditControls.UIButton btnClose;
        private Janus.Windows.EditControls.UIButton btnOk;
        private AdManagerClient._10_Media._10_Group.GroupDs groupDs;
        private Janus.Windows.UI.Dock.UIPanelInnerContainer panelTopContainer;
        private Label lbChannel;
        private Janus.Windows.GridEX.EditControls.EditBox ebSearchKey;
        private Janus.Windows.EditControls.UIButton btnChannelSearch;
        private Janus.Windows.EditControls.UIComboBox cbSearchCategoryName;
        private Label label1;
        private Label lbGenre;
        private Janus.Windows.EditControls.UIComboBox cbSearchMediaName;
        private Janus.Windows.EditControls.UIComboBox cbSearchGenreName;
        private Janus.Windows.EditControls.UICheckBox chkInvalidMenu;
		private Janus.Windows.GridEX.GridEX grdExChannelNoList;
		//		private System.ComponentModel.IContainer components;

		/// <summary>
		/// ������ �Ѱܾ� �� ��
		/// </summary>
		/// <param name="sender"></param>
		public ChannelNoPopForm1(GroupControl sender)
		{
			InitializeComponent();
			ChannelSetCtl = sender;
		}

		/// <summary>
		/// �Ϲݻ����
		/// </summary>
		public ChannelNoPopForm1()
		{
			InitializeComponent();
			ChannelSetCtl = null;
		}

		/// <summary>
		/// ��� ���� ��� ���ҽ��� �����մϴ�.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				/*
				if(components != null)
				{
					components.Dispose();
				}
				*/
			}
			base.Dispose( disposing );
		}

		#region Windows Form �����̳ʿ��� ������ �ڵ�
		/// <summary>
		/// �����̳� ������ �ʿ��� �޼����Դϴ�.
		/// �� �޼����� ������ �ڵ� ������� �������� ���ʽÿ�.
		/// </summary>
		private void InitializeComponent()
		{
            Janus.Windows.GridEX.GridEXLayout grdExChannelNoList_Layout_0 = new Janus.Windows.GridEX.GridEXLayout();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ChannelNoPopForm1));
            this.dvChannelSet = new System.Data.DataView();
            this.groupDs = new AdManagerClient._10_Media._10_Group.GroupDs();
            this.panel4 = new System.Windows.Forms.Panel();
            this.panelTopContainer = new Janus.Windows.UI.Dock.UIPanelInnerContainer();
            this.lbChannel = new System.Windows.Forms.Label();
            this.ebSearchKey = new Janus.Windows.GridEX.EditControls.EditBox();
            this.btnChannelSearch = new Janus.Windows.EditControls.UIButton();
            this.cbSearchCategoryName = new Janus.Windows.EditControls.UIComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.lbGenre = new System.Windows.Forms.Label();
            this.cbSearchMediaName = new Janus.Windows.EditControls.UIComboBox();
            this.cbSearchGenreName = new Janus.Windows.EditControls.UIComboBox();
            this.chkInvalidMenu = new Janus.Windows.EditControls.UICheckBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.grdExChannelNoList = new Janus.Windows.GridEX.GridEX();
            this.pnlBtn = new System.Windows.Forms.Panel();
            this.btnClose = new Janus.Windows.EditControls.UIButton();
            this.btnOk = new Janus.Windows.EditControls.UIButton();
            ((System.ComponentModel.ISupportInitialize)(this.dvChannelSet)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupDs)).BeginInit();
            this.panel4.SuspendLayout();
            this.panelTopContainer.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdExChannelNoList)).BeginInit();
            this.pnlBtn.SuspendLayout();
            this.SuspendLayout();
            // 
            // dvChannelSet
            // 
            this.dvChannelSet.Table = this.groupDs.Channel;
            // 
            // groupDs
            // 
            this.groupDs.DataSetName = "GroupDs";
            this.groupDs.Locale = new System.Globalization.CultureInfo("en-US");
            this.groupDs.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // panel4
            // 
            this.panel4.BackColor = System.Drawing.Color.White;
            this.panel4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel4.Controls.Add(this.panelTopContainer);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel4.Location = new System.Drawing.Point(0, 0);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(645, 62);
            this.panel4.TabIndex = 0;
            // 
            // panelTopContainer
            // 
            this.panelTopContainer.Controls.Add(this.lbChannel);
            this.panelTopContainer.Controls.Add(this.ebSearchKey);
            this.panelTopContainer.Controls.Add(this.btnChannelSearch);
            this.panelTopContainer.Controls.Add(this.cbSearchCategoryName);
            this.panelTopContainer.Controls.Add(this.label1);
            this.panelTopContainer.Controls.Add(this.lbGenre);
            this.panelTopContainer.Controls.Add(this.cbSearchMediaName);
            this.panelTopContainer.Controls.Add(this.cbSearchGenreName);
            this.panelTopContainer.Controls.Add(this.chkInvalidMenu);
            this.panelTopContainer.Location = new System.Drawing.Point(0, 0);
            this.panelTopContainer.Name = "panelTopContainer";
            this.panelTopContainer.Size = new System.Drawing.Size(644, 61);
            this.panelTopContainer.TabIndex = 1;
            // 
            // lbChannel
            // 
            this.lbChannel.AutoSize = true;
            this.lbChannel.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.lbChannel.Location = new System.Drawing.Point(295, 12);
            this.lbChannel.Name = "lbChannel";
            this.lbChannel.Size = new System.Drawing.Size(88, 13);
            this.lbChannel.TabIndex = 53;
            this.lbChannel.Text = "���α׷��� �˻�";
            // 
            // ebSearchKey
            // 
            this.ebSearchKey.ButtonFont = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ebSearchKey.Location = new System.Drawing.Point(396, 6);
            this.ebSearchKey.Name = "ebSearchKey";
            this.ebSearchKey.Size = new System.Drawing.Size(159, 22);
            this.ebSearchKey.TabIndex = 52;
            this.ebSearchKey.Text = "�˻�� �Է��ϼ���";
            this.ebSearchKey.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
            this.ebSearchKey.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            this.ebSearchKey.TextChanged += new System.EventHandler(this.ebSearchKey_TextChanged);
            this.ebSearchKey.Click += new System.EventHandler(this.ebSearchKey_Click);
            this.ebSearchKey.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ebSearchKey_KeyDown);
            // 
            // btnChannelSearch
            // 
            this.btnChannelSearch.BackColor = System.Drawing.Color.White;
            this.btnChannelSearch.Location = new System.Drawing.Point(561, 6);
            this.btnChannelSearch.Name = "btnChannelSearch";
            this.btnChannelSearch.Size = new System.Drawing.Size(72, 21);
            this.btnChannelSearch.TabIndex = 51;
            this.btnChannelSearch.Text = "�� ȸ";
            this.btnChannelSearch.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.btnChannelSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // cbSearchCategoryName
            // 
            this.cbSearchCategoryName.ComboStyle = Janus.Windows.EditControls.ComboStyle.DropDownList;
            this.cbSearchCategoryName.Location = new System.Drawing.Point(108, 33);
            this.cbSearchCategoryName.Name = "cbSearchCategoryName";
            this.cbSearchCategoryName.Size = new System.Drawing.Size(184, 22);
            this.cbSearchCategoryName.TabIndex = 36;
            this.cbSearchCategoryName.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.cbSearchCategoryName.SelectedIndexChanged += new System.EventHandler(this.cbSearchCategoryName_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.label1.Location = new System.Drawing.Point(7, 38);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(96, 13);
            this.label1.TabIndex = 38;
            this.label1.Text = "1���޴�(ī�װ�)";
            // 
            // lbGenre
            // 
            this.lbGenre.AutoSize = true;
            this.lbGenre.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.lbGenre.Location = new System.Drawing.Point(302, 38);
            this.lbGenre.Name = "lbGenre";
            this.lbGenre.Size = new System.Drawing.Size(82, 13);
            this.lbGenre.TabIndex = 37;
            this.lbGenre.Text = "2�� �޴� (�帣)";
            // 
            // cbSearchMediaName
            // 
            this.cbSearchMediaName.ComboStyle = Janus.Windows.EditControls.ComboStyle.DropDownList;
            this.cbSearchMediaName.Location = new System.Drawing.Point(8, 67);
            this.cbSearchMediaName.Name = "cbSearchMediaName";
            this.cbSearchMediaName.Size = new System.Drawing.Size(28, 22);
            this.cbSearchMediaName.TabIndex = 0;
            this.cbSearchMediaName.Visible = false;
            this.cbSearchMediaName.VisualStyle = Janus.Windows.UI.VisualStyle.Office2003;
            // 
            // cbSearchGenreName
            // 
            this.cbSearchGenreName.ComboStyle = Janus.Windows.EditControls.ComboStyle.DropDownList;
            this.cbSearchGenreName.Location = new System.Drawing.Point(396, 33);
            this.cbSearchGenreName.Name = "cbSearchGenreName";
            this.cbSearchGenreName.Size = new System.Drawing.Size(237, 22);
            this.cbSearchGenreName.TabIndex = 1;
            this.cbSearchGenreName.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.cbSearchGenreName.SelectedIndexChanged += new System.EventHandler(this.cbSearchGenreName_SelectedIndexChanged);
            // 
            // chkInvalidMenu
            // 
            this.chkInvalidMenu.Location = new System.Drawing.Point(9, 1);
            this.chkInvalidMenu.Name = "chkInvalidMenu";
            this.chkInvalidMenu.ShowFocusRectangle = false;
            this.chkInvalidMenu.Size = new System.Drawing.Size(88, 26);
            this.chkInvalidMenu.TabIndex = 50;
            this.chkInvalidMenu.Text = "��ȿ�޴�����";
            this.chkInvalidMenu.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.chkInvalidMenu.CheckedChanged += new System.EventHandler(this.chkInvalidMenu_CheckedChanged);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.grdExChannelNoList);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 62);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(645, 490);
            this.panel1.TabIndex = 1;
            // 
            // grdExChannelNoList
            // 
            this.grdExChannelNoList.AllowEdit = Janus.Windows.GridEX.InheritableBoolean.False;
            this.grdExChannelNoList.AlternatingColors = true;
            this.grdExChannelNoList.AutomaticSort = false;
            this.grdExChannelNoList.DataSource = this.dvChannelSet;
            this.grdExChannelNoList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdExChannelNoList.EmptyRows = true;
            this.grdExChannelNoList.FocusCellFormatStyle.BackColor = System.Drawing.SystemColors.Highlight;
            this.grdExChannelNoList.FocusCellFormatStyle.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.grdExChannelNoList.FocusStyle = Janus.Windows.GridEX.FocusStyle.None;
            this.grdExChannelNoList.Font = new System.Drawing.Font("����", 9F);
            this.grdExChannelNoList.GridLineColor = System.Drawing.Color.Silver;
            this.grdExChannelNoList.GridLines = Janus.Windows.GridEX.GridLines.Vertical;
            this.grdExChannelNoList.GridLineStyle = Janus.Windows.GridEX.GridLineStyle.Solid;
            this.grdExChannelNoList.GroupByBoxVisible = false;
            this.grdExChannelNoList.GroupMode = Janus.Windows.GridEX.GroupMode.Collapsed;
            this.grdExChannelNoList.GroupRowVisualStyle = Janus.Windows.GridEX.GroupRowVisualStyle.Outlook2003;
            this.grdExChannelNoList.HideSelection = Janus.Windows.GridEX.HideSelection.HighlightInactive;
            grdExChannelNoList_Layout_0.DataSource = this.dvChannelSet;
            grdExChannelNoList_Layout_0.IsCurrentLayout = true;
            grdExChannelNoList_Layout_0.Key = "bae";
            grdExChannelNoList_Layout_0.LayoutString = resources.GetString("grdExChannelNoList_Layout_0.LayoutString");
            this.grdExChannelNoList.Layouts.AddRange(new Janus.Windows.GridEX.GridEXLayout[] {
            grdExChannelNoList_Layout_0});
            this.grdExChannelNoList.Location = new System.Drawing.Point(0, 0);
            this.grdExChannelNoList.Name = "grdExChannelNoList";
            this.grdExChannelNoList.ScrollBars = Janus.Windows.GridEX.ScrollBars.Vertical;
            this.grdExChannelNoList.SelectedInactiveFormatStyle.BackColor = System.Drawing.Color.Gold;
            this.grdExChannelNoList.Size = new System.Drawing.Size(645, 490);
            this.grdExChannelNoList.TabIndex = 0;
            this.grdExChannelNoList.TabKeyBehavior = Janus.Windows.GridEX.TabKeyBehavior.ControlNavigation;
            this.grdExChannelNoList.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            this.grdExChannelNoList.RowDoubleClick += new Janus.Windows.GridEX.RowActionEventHandler(this.grdExChannelNoList_RowDoubleClick);
            // 
            // pnlBtn
            // 
            this.pnlBtn.BackColor = System.Drawing.Color.White;
            this.pnlBtn.Controls.Add(this.btnClose);
            this.pnlBtn.Controls.Add(this.btnOk);
            this.pnlBtn.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlBtn.Location = new System.Drawing.Point(0, 552);
            this.pnlBtn.Name = "pnlBtn";
            this.pnlBtn.Size = new System.Drawing.Size(645, 50);
            this.pnlBtn.TabIndex = 2;
            // 
            // btnClose
            // 
            this.btnClose.BackColor = System.Drawing.SystemColors.Control;
            this.btnClose.Location = new System.Drawing.Point(301, 11);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(70, 23);
            this.btnClose.TabIndex = 1;
            this.btnClose.Text = "�ݱ�";
            this.btnClose.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnOk
            // 
            this.btnOk.BackColor = System.Drawing.SystemColors.Control;
            this.btnOk.Location = new System.Drawing.Point(221, 11);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(70, 23);
            this.btnOk.TabIndex = 0;
            this.btnOk.Text = "��";
            this.btnOk.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // ChannelNoPopForm1
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(6, 15);
            this.ClientSize = new System.Drawing.Size(645, 602);
            this.Controls.Add(this.pnlBtn);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panel4);
            this.Font = new System.Drawing.Font("���� ���", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "ChannelNoPopForm1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "���α׷�(ä��)��ϰ˻�";
            this.Load += new System.EventHandler(this.ChannelNoPopForm1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dvChannelSet)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupDs)).EndInit();
            this.panel4.ResumeLayout(false);
            this.panelTopContainer.ResumeLayout(false);
            this.panelTopContainer.PerformLayout();
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdExChannelNoList)).EndInit();
            this.pnlBtn.ResumeLayout(false);
            this.ResumeLayout(false);

		}
		#endregion

		#region ��Ʈ�� �ε�
		private void ChannelNoPopForm1_Load(object sender, System.EventArgs e)
		{           
			// �����Ͱ����� ��ü����
			dt = ((DataView)grdExChannelNoList.DataSource).Table;  
			cm = (CurrencyManager) this.BindingContext[grdExChannelNoList.DataSource]; 
			//cm.PositionChanged += new System.EventHandler(OnGrdRowChanged); 

			// ��Ʈ�� �ʱ�ȭ
			InitControl();
		}
		#endregion

		#region ����� �׼�ó�� �޼ҵ�

		/// <summary>
		/// �˻��� ����
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void cbSearchMediaName_TextChanged(object sender, System.EventArgs e)
		{
			IsNewSearchKey = false;
		}

		/// <summary>
		/// �˻��� Ŭ�� 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void cbSearchMediaName_Click(object sender, System.EventArgs e)
		{
			if(IsNewSearchKey)
			{
				cbSearchMediaName.Text = "";
			}
			else
			{
				//cbSearchMediaName.SelectAll();
			}
		}

		private void cbSearchMediaName_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
		{
			if(e.KeyCode == Keys.Enter && !(IsNewSearchKey))
			{
				SearchChannelSet();
			}
		}

		#endregion

		#region ��Ʈ�� �ʱ�ȭ
		private void InitControl()
		{
			//InitCombo();
			InitCombo_Category();
			//InitCombo_Genre();												
		}

		private void InitCombo()
		{			
			MediaCodeModel mediacodeModel = new MediaCodeModel();		
			new MediaCodeManager(systemModel, commonModel).GetMediaCodeList(mediacodeModel);
			
			if (mediacodeModel.ResultCD.Equals("0000"))
			{
				// �����ͼ¿� ����
				Utility.SetDataTable(groupDs.Medias, mediacodeModel.MediaCodeDataSet);				
			}

			// ����ȸ �޺�
			// �������� �޺��� Dataset�� �����ͼҽ��� ������.

			// �˻������� �޺�
			this.cbSearchMediaName.Items.Clear();
			
			// �޺��ڽ��� ��Ʈ�� �ڵ����� ���� Item�迭�� ����
			Janus.Windows.EditControls.UIComboBoxItem[] comboItems = new Janus.Windows.EditControls.UIComboBoxItem[mediacodeModel.ResultCnt];
			
			for(int i=0;i<mediacodeModel.ResultCnt;i++)
			{
				DataRow row = groupDs.Medias.Rows[i];

				string val = row["MediaCode"].ToString();
				string txt = row["MediaName"].ToString();
				comboItems[i] = new Janus.Windows.EditControls.UIComboBoxItem(txt,val);
			}
			// �޺��� ��Ʈ
			this.cbSearchMediaName.Items.AddRange(comboItems);
			if(this.cbSearchMediaName.Items.Count > 1)  this.cbSearchMediaName.SelectedIndex = 1;

			Application.DoEvents();

		}
		

		public void InitCombo_Category()
		{
			// �ڵ忡�� ���ȷ����� ��ȸ�Ѵ�.
			GroupModel groupModel = new GroupModel();


            //�˻� ������ ���α׷� - ���α׷��� �˻�� ���Ե� ī�װ� ����� �����´�.
            groupModel.SearchType = "P";

            if (IsNewSearchKey) groupModel.SearchKey = "";
            else groupModel.SearchKey = ebSearchKey.Text.Trim();

            //��ȿ �޴� üũ�Ǵ�
            if (chkInvalidMenu.Checked)
            {
                groupModel.InvalidYn = true;
            }
            else
            {
                groupModel.InvalidYn = false;
            }

            
			new GroupManager(systemModel, commonModel).GetCategoryList2(groupModel);
			
			if (groupModel.ResultCD.Equals("0000"))
			{
				Utility.SetDataTable(groupDs.Category, groupModel.CategoryDataSet);	
			}

			// �ϴ� ��ü �޺��� �� �ʱ�ȭ. 
			this.cbSearchCategoryName.Items.Clear();
            
			// �޺��ڽ��� ��Ʈ�� �ڵ����� ���� Item�迭�� ����
			Janus.Windows.EditControls.UIComboBoxItem[] comboItems = new Janus.Windows.EditControls.UIComboBoxItem[groupModel.ResultCnt];
//            comboItems[0] = new Janus.Windows.EditControls.UIComboBoxItem("1���޴�����", "00");

			for(int i=0;i<groupModel.ResultCnt;i++)
			{
				DataRow row = groupDs.Category.Rows[i];

				string val = row["CategoryCode"].ToString();
				string txt = row["CategoryName"].ToString();
				comboItems[i] = new Janus.Windows.EditControls.UIComboBoxItem(txt,val);
			}

			// �ϴ� �޺��� ��Ʈ
			this.cbSearchCategoryName.Items.AddRange(comboItems);
            if (this.cbSearchCategoryName.Items.Count > 0)
            {
                this.cbSearchCategoryName.SelectedIndex = 0;
            }
            else
            {
                MessageBox.Show(groupModel.SearchKey + "(��)�� �˻����� �ʾҽ��ϴ�.", "�˻�", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
			
			Application.DoEvents();
		}


		public void InitCombo_Genre()
		{
			// �ڵ忡�� ���ȷ����� ��ȸ�Ѵ�.
			GroupModel groupModel = new GroupModel();


            //�˻� ������ ���α׷� - ���α׷��� �˻�� ���Ե� �帣 ����� �����´�.
            groupModel.SearchType = "P";

            if (IsNewSearchKey) groupModel.SearchKey = "";
            else groupModel.SearchKey = ebSearchKey.Text.Trim(); 
            
            groupModel.CategoryCode = KeyCategory;


            //��ȿ �޴� üũ�Ǵ� - ī�װ��� �� �����´�
            if (chkInvalidMenu.Checked)
            {
                groupModel.InvalidYn = true;
            }
            else
            {
                groupModel.InvalidYn = false;
            }

			new GroupManager(systemModel, commonModel).GetGenreList2(groupModel);
			
			if (groupModel.ResultCD.Equals("0000"))
			{
				// �����ͼ¿� ����
				Utility.SetDataTable(groupDs.Genre, groupModel.GenreDataSet);	
			}

			// �ϴ� ��ü �޺��� �� �ʱ�ȭ. 
			this.cbSearchGenreName.Items.Clear();
            
			// �޺��ڽ��� ��Ʈ�� �ڵ����� ���� Item�迭�� ����
			Janus.Windows.EditControls.UIComboBoxItem[] comboItems = new Janus.Windows.EditControls.UIComboBoxItem[groupModel.ResultCnt];

//            comboItems[0] = new Janus.Windows.EditControls.UIComboBoxItem("2���޴�����", "00");
			
			for(int i=0;i<groupModel.ResultCnt;i++)
			{
				DataRow row = groupDs.Genre.Rows[i];

				string val = row["GenreCode"].ToString();
				string txt = row["GenreName"].ToString();
				comboItems[i] = new Janus.Windows.EditControls.UIComboBoxItem(txt,val);
			}


			// �ϴ� �޺��� ��Ʈ
			this.cbSearchGenreName.Items.AddRange(comboItems);
            if (cbSearchGenreName.Items.Count > 0) this.cbSearchGenreName.SelectedIndex = 0;

			Application.DoEvents();

		}

		#endregion
  
		#region ó���޼ҵ�

		/// <summary>
		/// ��������� ��ȸ
		/// </summary>
		private void SearchChannelSet()
		{
			StatusMessage("������ ������ ��ȸ�մϴ�.");

			try
			{				
				groupModel.MediaCode = keyMedia;
				groupModel.CategoryCode = KeyCategory;
				groupModel.GenreCode = KeyGenre;

                if (IsNewSearchKey) groupModel.SearchKey = "";
                else groupModel.SearchKey = ebSearchKey.Text.Trim();


				// �����������ȸ ���񽺸� ȣ���Ѵ�.
				new GroupManager(systemModel,commonModel).GetChannelNoPopList2(groupModel);

				if (groupModel.ResultCD.Equals("0000"))
				{
					Utility.SetDataTable(groupDs.Channel, groupModel.ChannelDataSet);				
					StatusMessage(groupModel.ResultCnt + "���� ������ ������ ��ȸ�Ǿ����ϴ�.");
				}
			}
			catch(FrameException fe)
			{
				FrameSystem.showMsgForm("��������ȸ����", new string[] {fe.ErrCode, fe.ResultMsg});
			}
			catch(Exception ex)
			{
				FrameSystem.showMsgForm("��������ȸ����",new string[] {"",ex.Message});
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

        private void ProgressStart()
        {
            if (ProgressEvent != null)
            {
                ProgressEventArgs ea = new ProgressEventArgs();
                ea.Type = ProgressEventArgs.Start;
                ProgressEvent(this, ea);
            }
        }

        private void ProgressStop()
        {
            if (ProgressEvent != null)
            {
                ProgressEventArgs ea = new ProgressEventArgs();
                ea.Type = ProgressEventArgs.Stop;
                ProgressEvent(this, ea);
            }
        }


		private void btnOk_Click(object sender, System.EventArgs e)
		{
            /** 2015.05.27 Youngil.Yi ���� 
			string newKey	= grdExChannelNoList.SelectedItems[0].GetRow().Cells["ChannelNo"].Value.ToString();			
			string newName	= grdExChannelNoList.SelectedItems[0].GetRow().Cells["Title"].Value.ToString();		
	
			if ( this.ChannelSetCtl.ChannelNo( KeyCategory, KeyGenre, newKey) )
			{
				MessageBox.Show("[" + newKey + "] " + newName + "\n\n�����Ϸ�!!!", "�׷��",MessageBoxButtons.OK, MessageBoxIcon.Information);
			}
            **/
            ProgressStart();
            try
            {
                int rc = 0;
                string newKey = "";

                for (int i = 0; i < grdExChannelNoList.RowCount; i++)
                {
                    if (grdExChannelNoList.GetRows()[i].Cells["CheckYn"].Value.ToString().Equals("True"))
                    {
                        newKey = grdExChannelNoList.GetRows()[i].Cells["ChannelNo"].Value.ToString();

                        if (!this.ChannelSetCtl.ChannelNo(KeyCategory, KeyGenre, newKey))
                            return;
                        rc++;
                    }
                }

                if (rc > 0)
                    MessageBox.Show("1���޴�[" + KeyCategory + "] " + KeyCategoryName + ", 2���޴�[" + KeyGenre + "] " + KeyGenreName + "�� �ø���[" + rc + "]���� �ø��� ��� �Ϸ�", "�ø��� ���", MessageBoxButtons.OK, MessageBoxIcon.Information);
                else 
                    return;

                ProgressStop();
            }
            catch (Exception ex)
            {
                FrameSystem.showMsgForm("�ø��� �� ��� ����", new string[] { "", ex.Message });
            }
            finally
            {
                ProgressStop();
            }
		}


		private void btnClose_Click(object sender, System.EventArgs e)
		{
			this.Close();
		}


		private void grdExChannelNoList_RowDoubleClick(object sender, Janus.Windows.GridEX.RowActionEventArgs e)
		{						
			string newKey	= grdExChannelNoList.SelectedItems[0].GetRow().Cells["ChannelNo"].Value.ToString();			
			string newName	= grdExChannelNoList.SelectedItems[0].GetRow().Cells["Title"].Value.ToString();		
	
			if ( this.ChannelSetCtl.ChannelNo( KeyCategory, KeyGenre, newKey) )
			{
				MessageBox.Show("[" + newKey + "] " + newName + "\n\n�����Ϸ�!!!", "�׷��",MessageBoxButtons.OK, MessageBoxIcon.Information);
			}
		}

		/// <summary>
		/// ī�װ��� ����ɴ�
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void cbSearchCategoryName_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			keyMedia	= "1";	//cbSearchMediaName.SelectedItem.Value.ToString();
            KeyCategory = cbSearchCategoryName.SelectedItem.Value.ToString();
            KeyCategoryName = cbSearchCategoryName.SelectedItem.Text.ToString();

			InitCombo_Genre();
		}

		private void cbSearchGenreName_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			KeyGenre	= cbSearchGenreName.SelectedItem.Value.ToString();
            KeyGenreName = cbSearchGenreName.SelectedItem.Text.ToString();
			SearchChannelSet();
		}

        private void btnSearch_Click(object sender, EventArgs e)
        {
            //SearchChannelSet();
            InitCombo_Category();
        }

        /// <summary>
        /// �˻��� ����
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// 
        private void ebSearchKey_TextChanged(object sender, EventArgs e)
        {
            IsNewSearchKey = false;
        }
       
        /// <summary>
        /// �˻��� Ŭ��
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// 
        private void ebSearchKey_Click(object sender, EventArgs e)
        {
            if (IsNewSearchKey)
            {
                ebSearchKey.Text = "";
            }
            else
            {
                ebSearchKey.SelectAll();
            }
        }

        private void chkInvalidMenu_CheckedChanged(object sender, EventArgs e)
        {
            InitCombo_Category();
        }

        private void ebSearchKey_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                InitCombo_Category();
            }
        }		
	}
	#endregion


}
