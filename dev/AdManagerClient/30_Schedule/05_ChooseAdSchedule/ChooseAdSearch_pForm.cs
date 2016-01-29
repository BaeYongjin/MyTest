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
    /// ChooseAdSearch_pForm�� ���� ��� �����Դϴ�.
    /// </summary>
    /// 
    public class ChooseAdSearch_pForm : System.Windows.Forms.Form
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
		SchChoiceAdModel schChoiceAdModel  = new SchChoiceAdModel();	// ������������

        // ȭ��ó���� ����
        bool IsNewSearchKey		  = true;					// �˻����Է� ����
		CurrencyManager cm        = null;					// ������ �׸����� ���濡 ���� �����ͼ� ������ ���Ͽ�			
		DataTable       dt        = null;

		// �� â�� �� ��Ʈ��
        private Object  Opener = null;

		// �޴�/ä������
		public	string	keyMediaCode    = "";
		public	string	keyCategoryCode = "";
		public	string	keyGenreCode    = "";
		public	string	keyChannelNo    = "";
		public	string	keySeriesNo		= "";

		#endregion

        #region [ �Ӽ��� ]
		private	TYPE_Schedule _schType;
	
		/// <summary>
		/// �� ���� Ÿ���� �����մϴ�
		/// </summary>
		public	TYPE_Schedule ScheduleType
		{
			set
			{
				_schType = value;
			}
		}

        private int _adType = 0;
        /// <summary>
        /// ��ȸ�� ����Ÿ���� �����մϴ�
        /// </summary>
        public int AdType
        {
            set
            {
                _adType = value;
            }
        }

        #endregion

		#region ȭ�� ������Ʈ, ������, �Ҹ���

        private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Panel panel4;
		private Janus.Windows.EditControls.UIButton btnSearch;
		private Janus.Windows.GridEX.EditControls.EditBox ebSearchKey;
		private System.Windows.Forms.Panel pnlBtn;
		private Janus.Windows.EditControls.UIButton btnClose;
		private Janus.Windows.EditControls.UIButton btnOk;
		private System.Data.DataView dvItems;
		private System.Windows.Forms.Label label13;
		private Janus.Windows.EditControls.UICheckBox chkAdState_10;
		private Janus.Windows.EditControls.UICheckBox chkAdState_40;
		private Janus.Windows.EditControls.UICheckBox chkAdState_30;
		private Janus.Windows.EditControls.UICheckBox chkAdState_20;
		private Janus.Windows.GridEX.GridEX grdExItemList;
		private AdManagerClient.SchChoiceAdSearch_pDs schChoiceAdSearch_pDs;

		public ChooseAdSearch_pForm(UserControl parent)
		{
			//
			// Windows Form �����̳� ������ �ʿ��մϴ�.
			//
			InitializeComponent();
           
			// ��â�� ȣ���� ��Ʈ��
			Opener = (ChooseAdScheduleControl) parent;
			schChoiceAdModel.Init();
		}

        public ChooseAdSearch_pForm(UserControl parent, string type)
        {
            //
            // Windows Form �����̳� ������ �ʿ��մϴ�.
            //
            InitializeComponent();
           
            // ��â�� ȣ���� ��Ʈ��
            schChoiceAdModel.Init();
        }
		
		public ChooseAdSearch_pForm()
        {
            //
            // Windows Form �����̳� ������ �ʿ��մϴ�.
            //
            InitializeComponent();

            schChoiceAdModel.Init();
        }

        /// <summary>
        /// ��� ���� ��� ���ҽ��� �����մϴ�.
        /// </summary>
        protected override void Dispose( bool disposing )
        {
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
			Janus.Windows.GridEX.GridEXLayout grdExItemList_Layout_0 = new Janus.Windows.GridEX.GridEXLayout();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ChooseAdSearch_pForm));
			this.dvItems = new System.Data.DataView();
			this.schChoiceAdSearch_pDs = new AdManagerClient.SchChoiceAdSearch_pDs();
			this.panel1 = new System.Windows.Forms.Panel();
			this.grdExItemList = new Janus.Windows.GridEX.GridEX();
			this.pnlBtn = new System.Windows.Forms.Panel();
			this.btnClose = new Janus.Windows.EditControls.UIButton();
			this.btnOk = new Janus.Windows.EditControls.UIButton();
			this.panel4 = new System.Windows.Forms.Panel();
			this.label13 = new System.Windows.Forms.Label();
			this.chkAdState_10 = new Janus.Windows.EditControls.UICheckBox();
			this.chkAdState_40 = new Janus.Windows.EditControls.UICheckBox();
			this.chkAdState_30 = new Janus.Windows.EditControls.UICheckBox();
			this.chkAdState_20 = new Janus.Windows.EditControls.UICheckBox();
			this.btnSearch = new Janus.Windows.EditControls.UIButton();
			this.ebSearchKey = new Janus.Windows.GridEX.EditControls.EditBox();
			((System.ComponentModel.ISupportInitialize)(this.dvItems)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.schChoiceAdSearch_pDs)).BeginInit();
			this.panel1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.grdExItemList)).BeginInit();
			this.pnlBtn.SuspendLayout();
			this.panel4.SuspendLayout();
			this.SuspendLayout();
			// 
			// dvItems
			// 
			this.dvItems.Table = this.schChoiceAdSearch_pDs.ChoiceAdItems;
			// 
			// schChoiceAdSearch_pDs
			// 
			this.schChoiceAdSearch_pDs.DataSetName = "SchChoiceAdSearch_pDs";
			this.schChoiceAdSearch_pDs.Locale = new System.Globalization.CultureInfo("en-US");
			this.schChoiceAdSearch_pDs.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
			// 
			// panel1
			// 
			this.panel1.Controls.Add(this.grdExItemList);
			this.panel1.Controls.Add(this.pnlBtn);
			this.panel1.Controls.Add(this.panel4);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel1.Location = new System.Drawing.Point(0, 0);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(792, 466);
			this.panel1.TabIndex = 17;
			// 
			// grdExItemList
			// 
			this.grdExItemList.AlternatingColors = true;
			this.grdExItemList.BorderStyle = Janus.Windows.GridEX.BorderStyle.Flat;
			this.grdExItemList.DataSource = this.dvItems;
			this.grdExItemList.Dock = System.Windows.Forms.DockStyle.Fill;
			this.grdExItemList.EmptyRows = true;
			this.grdExItemList.FocusCellFormatStyle.BackColor = System.Drawing.SystemColors.Highlight;
			this.grdExItemList.FocusCellFormatStyle.ForeColor = System.Drawing.Color.White;
			this.grdExItemList.FocusStyle = Janus.Windows.GridEX.FocusStyle.None;
			this.grdExItemList.GridLines = Janus.Windows.GridEX.GridLines.Vertical;
			this.grdExItemList.GridLineStyle = Janus.Windows.GridEX.GridLineStyle.Solid;
			this.grdExItemList.GroupByBoxVisible = false;
			this.grdExItemList.GroupMode = Janus.Windows.GridEX.GroupMode.Collapsed;
			this.grdExItemList.HeaderFormatStyle.TextAlignment = Janus.Windows.GridEX.TextAlignment.Center;
			grdExItemList_Layout_0.DataSource = this.dvItems;
			grdExItemList_Layout_0.IsCurrentLayout = true;
			grdExItemList_Layout_0.Key = "bae";
			grdExItemList_Layout_0.LayoutString = resources.GetString("grdExItemList_Layout_0.LayoutString");
			this.grdExItemList.Layouts.AddRange(new Janus.Windows.GridEX.GridEXLayout[] {
            grdExItemList_Layout_0});
			this.grdExItemList.Location = new System.Drawing.Point(0, 40);
			this.grdExItemList.Name = "grdExItemList";
			this.grdExItemList.ScrollBars = Janus.Windows.GridEX.ScrollBars.Vertical;
			this.grdExItemList.Size = new System.Drawing.Size(792, 386);
			this.grdExItemList.TabIndex = 7;
			this.grdExItemList.TabKeyBehavior = Janus.Windows.GridEX.TabKeyBehavior.ControlNavigation;
			this.grdExItemList.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
			this.grdExItemList.ColumnHeaderClick += new Janus.Windows.GridEX.ColumnActionEventHandler(this.grdExItemList_ColumnHeaderClick);
			// 
			// pnlBtn
			// 
			this.pnlBtn.BackColor = System.Drawing.Color.WhiteSmoke;
			this.pnlBtn.Controls.Add(this.btnClose);
			this.pnlBtn.Controls.Add(this.btnOk);
			this.pnlBtn.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.pnlBtn.Location = new System.Drawing.Point(0, 426);
			this.pnlBtn.Name = "pnlBtn";
			this.pnlBtn.Size = new System.Drawing.Size(792, 40);
			this.pnlBtn.TabIndex = 19;
			// 
			// btnClose
			// 
			this.btnClose.BackColor = System.Drawing.SystemColors.Control;
			this.btnClose.Location = new System.Drawing.Point(400, 8);
			this.btnClose.Name = "btnClose";
			this.btnClose.Size = new System.Drawing.Size(104, 23);
			this.btnClose.TabIndex = 9;
			this.btnClose.Text = "�� ��";
			this.btnClose.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
			this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
			// 
			// btnOk
			// 
			this.btnOk.BackColor = System.Drawing.SystemColors.Control;
			this.btnOk.Location = new System.Drawing.Point(288, 8);
			this.btnOk.Name = "btnOk";
			this.btnOk.Size = new System.Drawing.Size(104, 23);
			this.btnOk.TabIndex = 8;
			this.btnOk.Text = "�� ��";
			this.btnOk.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
			this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
			// 
			// panel4
			// 
			this.panel4.BackColor = System.Drawing.Color.White;
			this.panel4.Controls.Add(this.label13);
			this.panel4.Controls.Add(this.chkAdState_10);
			this.panel4.Controls.Add(this.chkAdState_40);
			this.panel4.Controls.Add(this.chkAdState_30);
			this.panel4.Controls.Add(this.chkAdState_20);
			this.panel4.Controls.Add(this.btnSearch);
			this.panel4.Controls.Add(this.ebSearchKey);
			this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
			this.panel4.Location = new System.Drawing.Point(0, 0);
			this.panel4.Name = "panel4";
			this.panel4.Size = new System.Drawing.Size(792, 40);
			this.panel4.TabIndex = 0;
			// 
			// label13
			// 
			this.label13.Location = new System.Drawing.Point(238, 12);
			this.label13.Name = "label13";
			this.label13.Size = new System.Drawing.Size(64, 16);
			this.label13.TabIndex = 44;
			this.label13.Text = "�������";
			this.label13.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// chkAdState_10
			// 
			this.chkAdState_10.Checked = true;
			this.chkAdState_10.CheckedValue = "";
			this.chkAdState_10.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkAdState_10.ForeColor = System.Drawing.SystemColors.ControlText;
			this.chkAdState_10.Location = new System.Drawing.Point(310, 12);
			this.chkAdState_10.Name = "chkAdState_10";
			this.chkAdState_10.Size = new System.Drawing.Size(44, 16);
			this.chkAdState_10.TabIndex = 2;
			this.chkAdState_10.Text = "���";
			this.chkAdState_10.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
			// 
			// chkAdState_40
			// 
			this.chkAdState_40.Location = new System.Drawing.Point(510, 12);
			this.chkAdState_40.Name = "chkAdState_40";
			this.chkAdState_40.Size = new System.Drawing.Size(44, 16);
			this.chkAdState_40.TabIndex = 5;
			this.chkAdState_40.Text = "����";
			this.chkAdState_40.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
			// 
			// chkAdState_30
			// 
			this.chkAdState_30.BackColor = System.Drawing.SystemColors.Window;
			this.chkAdState_30.Location = new System.Drawing.Point(446, 12);
			this.chkAdState_30.Name = "chkAdState_30";
			this.chkAdState_30.Size = new System.Drawing.Size(44, 16);
			this.chkAdState_30.TabIndex = 4;
			this.chkAdState_30.Text = "����";
			this.chkAdState_30.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
			// 
			// chkAdState_20
			// 
			this.chkAdState_20.Checked = true;
			this.chkAdState_20.CheckedValue = "";
			this.chkAdState_20.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkAdState_20.ForeColor = System.Drawing.SystemColors.ControlText;
			this.chkAdState_20.Location = new System.Drawing.Point(382, 12);
			this.chkAdState_20.Name = "chkAdState_20";
			this.chkAdState_20.Size = new System.Drawing.Size(44, 16);
			this.chkAdState_20.TabIndex = 3;
			this.chkAdState_20.Text = "��";
			this.chkAdState_20.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
			// 
			// btnSearch
			// 
			this.btnSearch.BackColor = System.Drawing.Color.White;
			this.btnSearch.Location = new System.Drawing.Point(680, 8);
			this.btnSearch.Name = "btnSearch";
			this.btnSearch.Size = new System.Drawing.Size(104, 24);
			this.btnSearch.TabIndex = 6;
			this.btnSearch.Text = "�� ȸ";
			this.btnSearch.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
			this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
			// 
			// ebSearchKey
			// 
			this.ebSearchKey.ButtonFont = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.ebSearchKey.ImeMode = System.Windows.Forms.ImeMode.Hangul;
			this.ebSearchKey.Location = new System.Drawing.Point(8, 8);
			this.ebSearchKey.Name = "ebSearchKey";
			this.ebSearchKey.Size = new System.Drawing.Size(208, 21);
			this.ebSearchKey.TabIndex = 1;
			this.ebSearchKey.Text = "�˻�� �Է��ϼ���";
			this.ebSearchKey.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
			this.ebSearchKey.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
			this.ebSearchKey.TextChanged += new System.EventHandler(this.ebSearchKey_TextChanged);
			this.ebSearchKey.Click += new System.EventHandler(this.ebSearchKey_Click);
			this.ebSearchKey.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ebSearchKey_KeyDown);
			// 
			// ChooseAdSearch_pForm
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(6, 14);
			this.ClientSize = new System.Drawing.Size(792, 466);
			this.Controls.Add(this.panel1);
			this.Font = global::AdManagerClient.Properties.Settings.Default.Font1;
			this.MaximizeBox = false;
			this.Name = "ChooseAdSearch_pForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "������ �� ����˻�";
			this.Load += new System.EventHandler(this.ChooseAdSearch_pForm_Load);
			((System.ComponentModel.ISupportInitialize)(this.dvItems)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.schChoiceAdSearch_pDs)).EndInit();
			this.panel1.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.grdExItemList)).EndInit();
			this.pnlBtn.ResumeLayout(false);
			this.panel4.ResumeLayout(false);
			this.panel4.PerformLayout();
			this.ResumeLayout(false);

		}
        #endregion

        #region ��Ʈ�� �ε�
        private void ChooseAdSearch_pForm_Load(object sender, System.EventArgs e)
        {
            
			// �����Ͱ����� ��ü����
			dt = ((DataView)grdExItemList.DataSource).Table;  
			cm = (CurrencyManager) this.BindingContext[grdExItemList.DataSource]; 

            SearchChoiceAdItem();
        }
        #endregion

        #region ����� �׼�ó�� �޼ҵ�

        /// <summary>
        /// �˻��� ����
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ebSearchKey_TextChanged(object sender, System.EventArgs e)
        {
            IsNewSearchKey = false;
        }

        /// <summary>
        /// �˻��� Ŭ�� 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ebSearchKey_Click(object sender, System.EventArgs e)
        {
            if(IsNewSearchKey)
            {
                ebSearchKey.Text = "";
            }
            else
            {
                ebSearchKey.SelectAll();
            }
        }

        private void ebSearchKey_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter && !(IsNewSearchKey))
            {
                SearchChoiceAdItem();
            }
        }


		private void btnOk_Click(object sender, System.EventArgs e)
		{
			AddSchChoiceAd();
		}

		private void btnClose_Click(object sender, System.EventArgs e)
		{
			this.Close();
		}

		private void btnSearch_Click(object sender, System.EventArgs e)
		{
			SearchChoiceAdItem();
		}

		private void grdExItemList_ColumnHeaderClick(object sender, Janus.Windows.GridEX.ColumnActionEventArgs e)
		{

			//�÷�Index 0(üũ�ڽ��÷���)�� �ƴϸ� ���������� ó��.
			if(e.Column.Index != 0)
			{
				return;
			}
            
			//ColumnHeader Click�ÿ� dt Setting 
			DataRow[] foundRows = dt.Select("CheckYn = 'False'");
         
			// if(grdExItemList.CurrentColumn.Position == 0){
			if( foundRows.Length == 0 )
			{
				for(int i=0;i < dt.Rows.Count;i++)
				{
					dt.Rows[i].BeginEdit();
					dt.Rows[i]["CheckYn"]="False";
					dt.Rows[i].EndEdit();
				}
			}
			else
			{
				for(int i=0;i < dt.Rows.Count;i++)
				{
					dt.Rows[i].BeginEdit();
					dt.Rows[i]["CheckYn"]="True";
					dt.Rows[i].EndEdit();
				}
			}
		}

		#endregion
  
        #region ó���޼ҵ�

        /// <summary>
        /// �������� ����� ������ ��ȸ
        /// </summary>
        private void SearchChoiceAdItem()
        {
            StatusMessage("������ ��� �������� ��ȸ�մϴ�.");

            try
            {
				grdExItemList.UnCheckAllRecords();

				schChoiceAdModel.Init();

                // �����͸𵨿� ������ ������ ��Ʈ�Ѵ�.
                if(IsNewSearchKey || ebSearchKey.Text.Length == 0)  schChoiceAdModel.SearchKey = "";
                else                                                schChoiceAdModel.SearchKey  = ebSearchKey.Text;

				if(chkAdState_10.Checked)   schChoiceAdModel.SearchchkAdState_10   = "Y";
				if(chkAdState_20.Checked)   schChoiceAdModel.SearchchkAdState_20   = "Y";
				if(chkAdState_30.Checked)   schChoiceAdModel.SearchchkAdState_30   = "Y";
				if(chkAdState_40.Checked)   schChoiceAdModel.SearchchkAdState_40   = "Y";

                schChoiceAdModel.SearchAdType   = _adType.ToString();


                // �����ȸ ���񽺸� ȣ���Ѵ�.
                new SchChoiceAdManager(systemModel,commonModel).GetContractItemList(schChoiceAdModel);

                if (schChoiceAdModel.ResultCD.Equals("0000"))
                {
                    Utility.SetDataTable(schChoiceAdSearch_pDs.ChoiceAdItems, schChoiceAdModel.ScheduleDataSet);				
                    StatusMessage(schChoiceAdModel.ResultCnt + "����  �������� ����� �������� ��ȸ�Ǿ����ϴ�.");

					if(schChoiceAdModel.ResultCnt == 0)
					{
						MessageBox.Show("��ȸ�� �������� ����� �������� �����ϴ�", "��ȸ���",MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					}
                }
            }
            catch(FrameException fe)
            {
                FrameSystem.showMsgForm("������ ��ȸ����", new string[] {fe.ErrCode, fe.ResultMsg});
            }
            catch(Exception ex)
            {
                FrameSystem.showMsgForm("������ ��ȸ����",new string[] {"",ex.Message});
            }
        }


		/// <summary>
		/// ���õȱ��� �������� ��
		/// </summary>
		private void AddSchChoiceAd()
		{
			StatusMessage("���õ� ���� ������������ ���մϴ�.");

			grdExItemList.UpdateData();

			try
			{
				int SetCount = 0;

				for(int i=0;i < schChoiceAdSearch_pDs.ChoiceAdItems.Rows.Count;i++)
				{
					DataRow row = schChoiceAdSearch_pDs.ChoiceAdItems.Rows[i];

					if(row["CheckYn"].ToString().Equals("True"))
					{
						schChoiceAdModel.Init();

						switch(_schType)
						{
							case TYPE_Schedule.Genre :
								schChoiceAdModel.ItemNo   = row["ItemNo"].ToString();
								schChoiceAdModel.ItemName = row["ItemName"].ToString();
								schChoiceAdModel.AdType   = row["AdType"].ToString();

								schChoiceAdModel.MediaCode = keyMediaCode;
								schChoiceAdModel.GenreCode = keyGenreCode;
								schChoiceAdModel.ChannelNo = keyChannelNo;
								schChoiceAdModel.SeriesNo	= keySeriesNo;
								schChoiceAdModel.ScheduleType = _schType;
								
								new SchChoiceAdManager(systemModel,commonModel).SetSchChoiceMenuDetailAdd(schChoiceAdModel);

                                ((ChooseAdScheduleControl)Opener).keyItemNo = row["ItemNo"].ToString();

								break;
							case TYPE_Schedule.Channel:
								schChoiceAdModel.ItemNo   = row["ItemNo"].ToString();
								schChoiceAdModel.ItemName = row["ItemName"].ToString();
								schChoiceAdModel.AdType   = row["AdType"].ToString();

								schChoiceAdModel.MediaCode = keyMediaCode;
								schChoiceAdModel.GenreCode = keyGenreCode;
								schChoiceAdModel.ChannelNo = keyChannelNo;
								schChoiceAdModel.SeriesNo	= keySeriesNo;
								schChoiceAdModel.ScheduleType = _schType;

								new SchChoiceAdManager(systemModel,commonModel).SetSchChoiceChannelDetailAdd(schChoiceAdModel);

                                ((ChooseAdScheduleControl)Opener).keyItemNo = row["ItemNo"].ToString();

								break;
							case TYPE_Schedule.Series:
								schChoiceAdModel.ItemNo   = row["ItemNo"].ToString();
								schChoiceAdModel.ItemName = row["ItemName"].ToString();
								schChoiceAdModel.AdType   = row["AdType"].ToString();

								schChoiceAdModel.MediaCode = keyMediaCode;
								schChoiceAdModel.GenreCode = keyGenreCode;
								schChoiceAdModel.ChannelNo = keyChannelNo;
								schChoiceAdModel.SeriesNo	= keySeriesNo;
								schChoiceAdModel.ScheduleType = _schType;

								new SchChoiceAdManager(systemModel,commonModel).SetSchChoiceSeriesDetailAdd(schChoiceAdModel);

								((ChooseAdScheduleControl)Opener).keyItemNo = row["ItemNo"].ToString();

								break;
						}

						if(schChoiceAdModel.ResultCD.Equals("0000"))
						{
							SetCount++;
						}
					}
				}

				// üũ�� ��� �׸��� Ŭ����
				ClearListCheck();

				if(SetCount > 0)
				{
                    ((ChooseAdScheduleControl)Opener).ReloadList();
				}
				else
				{
					MessageBox.Show("������ ���� �����ϴ�.", "����������",MessageBoxButtons.OK, MessageBoxIcon.Information);
				}
			}
			catch(FrameException fe)
			{
				FrameSystem.showMsgForm("�������� ������", new string[] {fe.ErrCode, fe.ResultMsg});
			}
			catch(Exception ex)
			{
				FrameSystem.showMsgForm("�������� ������",new string[] {"",ex.Message});
			}			
		}

		private void ClearListCheck()
		{

			// üũ�� ��� �׸��� Ŭ����
			grdExItemList.UnCheckAllRecords();
			grdExItemList.UpdateData();
				   
			// ������ Ŭ����
			for(int i=0;i < dt.Rows.Count;i++)
			{
				dt.Rows[i].BeginEdit();
				dt.Rows[i]["CheckYn"]="False";
				dt.Rows[i].EndEdit();
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
		#endregion

    }
}
