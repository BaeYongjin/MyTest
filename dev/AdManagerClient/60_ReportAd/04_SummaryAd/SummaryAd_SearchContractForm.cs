using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using System.Threading;

using WinFramework.Misc;
using WinFramework.Base;
using WinFramework.Data;

using AdManagerModel;

namespace AdManagerClient
{
    /// <summary>
    /// ContractItem_pForm�� ���� ��� �����Դϴ�.
    /// </summary>
    public class SummaryAd_SearchContractForm : System.Windows.Forms.Form
    {

		#region ��������� ��ü �� ����

        private System.Windows.Forms.Panel pnlSearch;
        private Janus.Windows.GridEX.EditControls.EditBox ebSearchKey;
        private Janus.Windows.EditControls.UIComboBox cbSearchMedia;
        private Janus.Windows.EditControls.UIComboBox cbSearchRap;
        private Janus.Windows.EditControls.UIComboBox cbSearchAgency;
        private System.Windows.Forms.Panel pnlBtn;
        private Janus.Windows.EditControls.UIButton btnClose;
        private Janus.Windows.EditControls.UIButton btnOk;
        private System.Windows.Forms.Panel panel1;
        private Janus.Windows.GridEX.GridEX grdExContractList;
        private System.Data.DataView dvContract;
//        private System.ComponentModel.IContainer components;
        private Janus.Windows.EditControls.UIButton btnSearch;
		private System.Windows.Forms.Label label7;
		private Janus.Windows.EditControls.UICheckBox chkContractState_20;
		private Janus.Windows.EditControls.UICheckBox chkContractState_10;
		private AdManagerClient.SummaryAd_SearchContractDs summaryAd_SearchContractDs;

		// ��ü
		public string keyMediaCode = "";			// �˾�ȣ��� ��ü��Ʈ

		// ������
		private SummaryAdControl Opener = null;

		#endregion

		#region ������ �� �Ҹ���
        public SummaryAd_SearchContractForm()
        {
            //
            // Windows Form �����̳� ������ �ʿ��մϴ�.
            //
            InitializeComponent();

            //
            
            //
        }

        /// <summary>
        /// ������ �Ѱܾ� �� ��
        /// </summary>
        /// <param name="sender"></param>
        public SummaryAd_SearchContractForm(SummaryAdControl sender)
        {
            //
            // Windows Form �����̳� ������ �ʿ��մϴ�.
            //
            InitializeComponent();

            //
            
            //
            
            Opener = sender;
        }


        /// <summary>
        /// ��� ���� ��� ���ҽ��� �����մϴ�.
        /// </summary>
        protected override void Dispose( bool disposing )
        {
            if( disposing )
            {
//                if(components != null)
//                {
//                    components.Dispose();
//                }
            }
            base.Dispose( disposing );
        }


        #endregion

        #region �̺�Ʈ�ڵ鷯
        public event StatusEventHandler 			StatusEvent;			// �����̺�Ʈ �ڵ鷯

        #endregion

        #region ��������� ��ü �� ����

        // �ý��� ���� : ȭ�����
        private SystemModel   systemModel   = FrameSystem.oSysModel;
        private CommonModel   commonModel   = FrameSystem.oComModel;
        private Logger        log           = FrameSystem.oLog;


        // ����� ������
        ContractModel contractModel  = new ContractModel();	// ���������

        // ȭ��ó���� ����
        bool IsNewSearchKey		  = true;					// �˻����Է� ����
        CurrencyManager cm        = null;					// ������ �׸����� ���濡 ���� �����ͼ� ������ ���Ͽ�			
        DataTable       dt        = null;

        #endregion

        #region Windows Form �����̳ʿ��� ������ �ڵ�
        /// <summary>
        /// �����̳� ������ �ʿ��� �޼����Դϴ�.
        /// �� �޼����� ������ �ڵ� ������� �������� ���ʽÿ�.
        /// </summary>
        private void InitializeComponent()
        {
            Janus.Windows.GridEX.GridEXLayout grdExContractList_DesignTimeLayout = new Janus.Windows.GridEX.GridEXLayout();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SummaryAd_SearchContractForm));
            this.pnlSearch = new System.Windows.Forms.Panel();
            this.chkContractState_20 = new Janus.Windows.EditControls.UICheckBox();
            this.chkContractState_10 = new Janus.Windows.EditControls.UICheckBox();
            this.label7 = new System.Windows.Forms.Label();
            this.btnSearch = new Janus.Windows.EditControls.UIButton();
            this.ebSearchKey = new Janus.Windows.GridEX.EditControls.EditBox();
            this.cbSearchMedia = new Janus.Windows.EditControls.UIComboBox();
            this.cbSearchRap = new Janus.Windows.EditControls.UIComboBox();
            this.cbSearchAgency = new Janus.Windows.EditControls.UIComboBox();
            this.pnlBtn = new System.Windows.Forms.Panel();
            this.btnClose = new Janus.Windows.EditControls.UIButton();
            this.btnOk = new Janus.Windows.EditControls.UIButton();
            this.panel1 = new System.Windows.Forms.Panel();
            this.grdExContractList = new Janus.Windows.GridEX.GridEX();
            this.dvContract = new System.Data.DataView();
            this.summaryAd_SearchContractDs = new AdManagerClient.SummaryAd_SearchContractDs();
            this.pnlSearch.SuspendLayout();
            this.pnlBtn.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdExContractList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dvContract)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.summaryAd_SearchContractDs)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlSearch
            // 
            this.pnlSearch.BackColor = System.Drawing.SystemColors.Window;
            this.pnlSearch.Controls.Add(this.chkContractState_20);
            this.pnlSearch.Controls.Add(this.chkContractState_10);
            this.pnlSearch.Controls.Add(this.label7);
            this.pnlSearch.Controls.Add(this.btnSearch);
            this.pnlSearch.Controls.Add(this.ebSearchKey);
            this.pnlSearch.Controls.Add(this.cbSearchMedia);
            this.pnlSearch.Controls.Add(this.cbSearchRap);
            this.pnlSearch.Controls.Add(this.cbSearchAgency);
            this.pnlSearch.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlSearch.Location = new System.Drawing.Point(0, 0);
            this.pnlSearch.Name = "pnlSearch";
            this.pnlSearch.Size = new System.Drawing.Size(680, 56);
            this.pnlSearch.TabIndex = 4;
            // 
            // chkContractState_20
            // 
            this.chkContractState_20.BackColor = System.Drawing.SystemColors.Window;
            this.chkContractState_20.Location = new System.Drawing.Point(152, 37);
            this.chkContractState_20.Name = "chkContractState_20";
            this.chkContractState_20.Size = new System.Drawing.Size(44, 16);
            this.chkContractState_20.TabIndex = 6;
            this.chkContractState_20.Text = "����";
            this.chkContractState_20.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            // 
            // chkContractState_10
            // 
            this.chkContractState_10.Checked = true;
            this.chkContractState_10.CheckedValue = "";
            this.chkContractState_10.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkContractState_10.ForeColor = System.Drawing.SystemColors.ControlText;
            this.chkContractState_10.Location = new System.Drawing.Point(80, 37);
            this.chkContractState_10.Name = "chkContractState_10";
            this.chkContractState_10.Size = new System.Drawing.Size(64, 16);
            this.chkContractState_10.TabIndex = 5;
            this.chkContractState_10.Text = "���";
            this.chkContractState_10.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            // 
            // label7
            // 
            this.label7.Location = new System.Drawing.Point(8, 32);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(72, 21);
            this.label7.TabIndex = 15;
            this.label7.Text = "������";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnSearch
            // 
            this.btnSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSearch.Location = new System.Drawing.Point(568, 8);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(104, 24);
            this.btnSearch.TabIndex = 7;
            this.btnSearch.Text = "�� ȸ";
            this.btnSearch.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // ebSearchKey
            // 
            this.ebSearchKey.ImeMode = System.Windows.Forms.ImeMode.Hangul;
            this.ebSearchKey.Location = new System.Drawing.Point(392, 8);
            this.ebSearchKey.Name = "ebSearchKey";
            this.ebSearchKey.Size = new System.Drawing.Size(168, 23);
            this.ebSearchKey.TabIndex = 4;
            this.ebSearchKey.Text = "�˻�� �Է��ϼ���";
            this.ebSearchKey.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
            this.ebSearchKey.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            this.ebSearchKey.TextChanged += new System.EventHandler(this.ebSearchKey_TextChanged);
            this.ebSearchKey.Click += new System.EventHandler(this.ebSearchKey_Click);
            this.ebSearchKey.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ebSearchKey_KeyDown);
            // 
            // cbSearchMedia
            // 
            this.cbSearchMedia.BackColor = System.Drawing.Color.White;
            this.cbSearchMedia.ComboStyle = Janus.Windows.EditControls.ComboStyle.DropDownList;
            this.cbSearchMedia.Location = new System.Drawing.Point(8, 8);
            this.cbSearchMedia.Name = "cbSearchMedia";
            this.cbSearchMedia.Size = new System.Drawing.Size(120, 23);
            this.cbSearchMedia.TabIndex = 1;
            this.cbSearchMedia.Text = "��ü����";
            this.cbSearchMedia.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            // 
            // cbSearchRap
            // 
            this.cbSearchRap.ComboStyle = Janus.Windows.EditControls.ComboStyle.DropDownList;
            this.cbSearchRap.Location = new System.Drawing.Point(136, 8);
            this.cbSearchRap.Name = "cbSearchRap";
            this.cbSearchRap.Size = new System.Drawing.Size(120, 23);
            this.cbSearchRap.TabIndex = 2;
            this.cbSearchRap.Text = "�̵�����";
            this.cbSearchRap.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            // 
            // cbSearchAgency
            // 
            this.cbSearchAgency.ComboStyle = Janus.Windows.EditControls.ComboStyle.DropDownList;
            this.cbSearchAgency.Location = new System.Drawing.Point(264, 8);
            this.cbSearchAgency.Name = "cbSearchAgency";
            this.cbSearchAgency.Size = new System.Drawing.Size(120, 23);
            this.cbSearchAgency.TabIndex = 3;
            this.cbSearchAgency.Text = "����缱��";
            this.cbSearchAgency.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            // 
            // pnlBtn
            // 
            this.pnlBtn.BackColor = System.Drawing.Color.WhiteSmoke;
            this.pnlBtn.Controls.Add(this.btnClose);
            this.pnlBtn.Controls.Add(this.btnOk);
            this.pnlBtn.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlBtn.Location = new System.Drawing.Point(0, 420);
            this.pnlBtn.Name = "pnlBtn";
            this.pnlBtn.Size = new System.Drawing.Size(680, 42);
            this.pnlBtn.TabIndex = 17;
            // 
            // btnClose
            // 
            this.btnClose.BackColor = System.Drawing.SystemColors.Control;
            this.btnClose.Location = new System.Drawing.Point(344, 8);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(104, 24);
            this.btnClose.TabIndex = 10;
            this.btnClose.Text = "�� ��";
            this.btnClose.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnOk
            // 
            this.btnOk.BackColor = System.Drawing.SystemColors.Control;
            this.btnOk.Location = new System.Drawing.Point(232, 8);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(104, 24);
            this.btnOk.TabIndex = 9;
            this.btnOk.Text = "Ȯ ��";
            this.btnOk.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.WhiteSmoke;
            this.panel1.Controls.Add(this.grdExContractList);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 56);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(680, 364);
            this.panel1.TabIndex = 18;
            // 
            // grdExContractList
            // 
            this.grdExContractList.AllowEdit = Janus.Windows.GridEX.InheritableBoolean.False;
            this.grdExContractList.AlternatingColors = true;
            this.grdExContractList.BorderStyle = Janus.Windows.GridEX.BorderStyle.Flat;
            this.grdExContractList.DataSource = this.dvContract;
            grdExContractList_DesignTimeLayout.LayoutString = resources.GetString("grdExContractList_DesignTimeLayout.LayoutString");
            this.grdExContractList.DesignTimeLayout = grdExContractList_DesignTimeLayout;
            this.grdExContractList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdExContractList.EditorsControlStyle.ButtonAppearance = Janus.Windows.GridEX.ButtonAppearance.Regular;
            this.grdExContractList.EmptyRows = true;
            this.grdExContractList.FocusCellFormatStyle.BackColor = System.Drawing.SystemColors.Highlight;
            this.grdExContractList.FocusCellFormatStyle.ForeColor = System.Drawing.Color.White;
            this.grdExContractList.FocusStyle = Janus.Windows.GridEX.FocusStyle.None;
            this.grdExContractList.Font = new System.Drawing.Font("���� ���", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.grdExContractList.GridLineColor = System.Drawing.Color.Silver;
            this.grdExContractList.GridLines = Janus.Windows.GridEX.GridLines.Vertical;
            this.grdExContractList.GridLineStyle = Janus.Windows.GridEX.GridLineStyle.Solid;
            this.grdExContractList.GroupByBoxVisible = false;
            this.grdExContractList.HeaderFormatStyle.TextAlignment = Janus.Windows.GridEX.TextAlignment.Center;
            this.grdExContractList.Location = new System.Drawing.Point(0, 0);
            this.grdExContractList.Name = "grdExContractList";
            this.grdExContractList.ScrollBars = Janus.Windows.GridEX.ScrollBars.Vertical;
            this.grdExContractList.Size = new System.Drawing.Size(680, 364);
            this.grdExContractList.TabIndex = 8;
            this.grdExContractList.TabKeyBehavior = Janus.Windows.GridEX.TabKeyBehavior.ControlNavigation;
            this.grdExContractList.ThemedAreas = ((Janus.Windows.GridEX.ThemedArea)(((((((((Janus.Windows.GridEX.ThemedArea.ScrollBars | Janus.Windows.GridEX.ThemedArea.EditControls)
                        | Janus.Windows.GridEX.ThemedArea.Headers)
                        | Janus.Windows.GridEX.ThemedArea.GroupByBox)
                        | Janus.Windows.GridEX.ThemedArea.GroupRows)
                        | Janus.Windows.GridEX.ThemedArea.ControlBorder)
                        | Janus.Windows.GridEX.ThemedArea.Cards)
                        | Janus.Windows.GridEX.ThemedArea.Gridlines)
                        | Janus.Windows.GridEX.ThemedArea.CheckBoxes)));
            this.grdExContractList.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            this.grdExContractList.DoubleClick += new System.EventHandler(this.grdExContractList_DoubleClick);
            // 
            // dvContract
            // 
            this.dvContract.Table = this.summaryAd_SearchContractDs.Contract;
            // 
            // summaryAd_SearchContractDs
            // 
            this.summaryAd_SearchContractDs.DataSetName = "SummaryAd_SearchContractDs";
            this.summaryAd_SearchContractDs.Locale = new System.Globalization.CultureInfo("en-US");
            this.summaryAd_SearchContractDs.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // SummaryAd_SearchContractForm
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(6, 16);
            this.ClientSize = new System.Drawing.Size(680, 462);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.pnlBtn);
            this.Controls.Add(this.pnlSearch);
            this.Font = new System.Drawing.Font("���� ���", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.Name = "SummaryAd_SearchContractForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "������˻�";
            this.Load += new System.EventHandler(this.UserControl_Load);
            this.pnlSearch.ResumeLayout(false);
            this.pnlSearch.PerformLayout();
            this.pnlBtn.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdExContractList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dvContract)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.summaryAd_SearchContractDs)).EndInit();
            this.ResumeLayout(false);

        }
        #endregion

        #region ��Ʈ�� �ε�
        private void UserControl_Load(object sender, System.EventArgs e)
        {
            // �����Ͱ����� ��ü����
            dt = ((DataView)grdExContractList.DataSource).Table;  
            cm = (CurrencyManager) this.BindingContext[grdExContractList.DataSource]; 
            cm.PositionChanged += new System.EventHandler(OnGrdRowChanged); 

            // ��Ʈ�� �ʱ�ȭ
            InitControl();	
            ebSearchKey.Focus();
        }

        #endregion

        #region ��Ʈ�� �ʱ�ȭ
        private void InitControl()
        {
            InitCombo();
            SearchContract();
			ebSearchKey.Focus();
        }

        private void InitCombo()
        {
            Init_MediaCode();
            Init_RapCode();
            Init_AgencyCode();
			InitCombo_Level();
        }

        private void Init_MediaCode()
        {
            // ��ü�� ��ȸ�Ѵ�.
            MediaCodeModel mediaCodeModel = new MediaCodeModel();
            new MediaCodeManager(systemModel, commonModel).GetMediaCodeList(mediaCodeModel);
			
            if (mediaCodeModel.ResultCD.Equals("0000"))
            {
                // �����ͼ¿� ����
                Utility.SetDataTable(summaryAd_SearchContractDs.Medias, mediaCodeModel.MediaCodeDataSet);				
            }

            // �˻������� �޺�
            this.cbSearchMedia.Items.Clear();
			
            // �޺��ڽ��� ��Ʈ�� �ڵ����� ���� Item�迭�� ����
            Janus.Windows.EditControls.UIComboBoxItem[] comboItems = new Janus.Windows.EditControls.UIComboBoxItem[mediaCodeModel.ResultCnt + 1];

            comboItems[0] = new Janus.Windows.EditControls.UIComboBoxItem("��ü����","00");
			
            for(int i=0;i<mediaCodeModel.ResultCnt;i++)
            {
                DataRow row = summaryAd_SearchContractDs.Medias.Rows[i];

                string val = row["MediaCode"].ToString();
                string txt = row["MediaName"].ToString();
                comboItems[i+1] = new Janus.Windows.EditControls.UIComboBoxItem(txt,val);
            }
            // �˻� �޺��� ��Ʈ
            this.cbSearchMedia.Items.AddRange(comboItems);
            this.cbSearchMedia.SelectedIndex = 0;

            Application.DoEvents();
        }

        private void Init_RapCode()
        {
            // ���� ��ȸ�Ѵ�.
            MediaRapCodeModel mediaRapCodeModel = new MediaRapCodeModel();
            new MediaRapCodeManager(systemModel, commonModel).GetMediaRapCodeList(mediaRapCodeModel);
			
            if (mediaRapCodeModel.ResultCD.Equals("0000"))
            {
                // �����ͼ¿� ����
                Utility.SetDataTable(summaryAd_SearchContractDs.MediaRaps, mediaRapCodeModel.MediaRapCodeDataSet);				
            }

            // �˻������� �޺�
            this.cbSearchRap.Items.Clear();
           
            // �޺��ڽ��� ��Ʈ�� �ڵ����� ���� Item�迭�� ����
            Janus.Windows.EditControls.UIComboBoxItem[] comboItems = new Janus.Windows.EditControls.UIComboBoxItem[mediaRapCodeModel.ResultCnt + 1];

            comboItems[0] = new Janus.Windows.EditControls.UIComboBoxItem("�̵�����","00");
			
            for(int i=0;i<mediaRapCodeModel.ResultCnt;i++)
            {
                DataRow row = summaryAd_SearchContractDs.MediaRaps.Rows[i];

                string val = row["RapCode"].ToString();
                string txt = row["RapName"].ToString();
                comboItems[i+1] = new Janus.Windows.EditControls.UIComboBoxItem(txt,val);
            }
            // �޺��� ��Ʈ
            this.cbSearchRap.Items.AddRange(comboItems);
            this.cbSearchRap.SelectedIndex = 0;

            Application.DoEvents();

        }

        private void Init_AgencyCode()
        {
            // ����縦 ��ȸ�Ѵ�.
            AgencyCodeModel agencyCodeModel = new AgencyCodeModel();
            new AgencyCodeManager(systemModel, commonModel).GetAgencyCodeList(agencyCodeModel);
			
            if (agencyCodeModel.ResultCD.Equals("0000"))
            {
                // �����ͼ¿� ����
                Utility.SetDataTable(summaryAd_SearchContractDs.Agencys, agencyCodeModel.AgencyCodeDataSet);				
            }

            // �˻������� �޺�
            this.cbSearchAgency.Items.Clear();
			
            // �޺��ڽ��� ��Ʈ�� �ڵ����� ���� Item�迭�� ����
            Janus.Windows.EditControls.UIComboBoxItem[] comboItems = new Janus.Windows.EditControls.UIComboBoxItem[agencyCodeModel.ResultCnt + 1];

            comboItems[0] = new Janus.Windows.EditControls.UIComboBoxItem("����缱��","00");
			
            for(int i=0;i<agencyCodeModel.ResultCnt;i++)
            {
                DataRow row = summaryAd_SearchContractDs.Agencys.Rows[i];

                string val = row["AgencyCode"].ToString();
                string txt = row["AgencyName"].ToString();
                comboItems[i+1] = new Janus.Windows.EditControls.UIComboBoxItem(txt,val);
            }
			
            // �޺��� ��Ʈ
            this.cbSearchAgency.Items.AddRange(comboItems);
            this.cbSearchAgency.SelectedIndex = 0;

            Application.DoEvents();
        }

		private void InitCombo_Level()
		{
			if(commonModel.UserLevel == "20")
			{
				// �޺��Ƚ�						
				cbSearchMedia.SelectedValue = commonModel.MediaCode;			
				cbSearchMedia.ReadOnly = true;				            
			}
			else
			{
				for(int i=0;i < summaryAd_SearchContractDs.Medias.Rows.Count;i++)
				{
					DataRow row = summaryAd_SearchContractDs.Medias.Rows[i];					
					if(row["MediaCode"].ToString().Equals(FrameSystem._HANATV.ToString()))
					{
						cbSearchMedia.SelectedValue = FrameSystem._HANATV; // �ϳ�TV�� �⺻������ �Ѵ�.	 		
						break;															
					}
					else
					{
						cbSearchMedia.SelectedValue="00";
					}
				}	
			}
			if (commonModel.UserLevel == "30")
			{
				cbSearchRap.SelectedValue = commonModel.RapCode;
				cbSearchRap.ReadOnly = true;
			}
			if (commonModel.UserLevel == "40")
			{
				cbSearchAgency.SelectedValue = commonModel.AgencyCode;
				cbSearchAgency.ReadOnly = true;
			}


			Application.DoEvents();
		}

        #endregion

        #region ������� �׼�ó�� �޼ҵ�

        /// <summary>
        /// �׸����� Row�����
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnGrdRowChanged(object sender, System.EventArgs e) 
        {
            if(grdExContractList.RowCount > 0)
            {
                //                SetDetailText();
                //                InitButton();
            }
        }

        /// <summary>
        /// ��ȸ��ư Ŭ��
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSearch_Click(object sender, System.EventArgs e)
        {
            SearchContract();
        }

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
            if(e.KeyCode == Keys.Enter)
            {
                SearchContract();
            }
        }

		private void btnOk_Click(object sender, System.EventArgs e)
		{
			SelectContract();
			this.Close();
		}

		private void btnClose_Click(object sender, System.EventArgs e)
		{
			this.Close();
		}

		private void grdExContractList_DoubleClick(object sender, System.EventArgs e)
		{
			SelectContract();
			this.Close();		
		}

        #endregion

        #region ó���޼ҵ�

        /// <summary>
        /// �������� ��ȸ
        /// </summary>
        private void SearchContract()
        {
            StatusMessage("������ ������ ��ȸ�մϴ�.");

            try
            {
                //�˻� ���� ���� �ʱ�ȭ ���ش�.
                contractModel.Init();
                // �����͸𵨿� ������ ������ ��Ʈ�Ѵ�.
                contractModel.MediaCode      = cbSearchMedia.SelectedValue.ToString();
                contractModel.RapCode        = cbSearchRap.SelectedValue.ToString();
                contractModel.AgencyCode     = cbSearchAgency.SelectedValue.ToString();
				if(chkContractState_10.Checked) contractModel.SearchState_10 = "Y";
				if(chkContractState_20.Checked) contractModel.SearchState_20 = "Y";
    
                if(IsNewSearchKey)
                {
                    contractModel.SearchKey = "";
                }
                else
                {
                    contractModel.SearchKey  = ebSearchKey.Text;
                }

                // ���� ��� ��� ���񽺸� ȣ���Ѵ�.
                new ContractManager(systemModel,commonModel).GetContractList2(contractModel);

                if (contractModel.ResultCD.Equals("0000"))
                {
                    Utility.SetDataTable(summaryAd_SearchContractDs.Contract, contractModel.ContractDataSet);				
                    StatusMessage(contractModel.ResultCnt + "���� ������� ������ ��ȸ�Ǿ����ϴ�.");
					if(Convert.ToInt32(contractModel.ResultCnt) > 0) cm.Position = 0;
					grdExContractList.EnsureVisible();
					grdExContractList.Focus();

                }
            }
            catch(FrameException fe)
            {
                FrameSystem.showMsgForm("���������ȸ����", new string[] {fe.ErrCode, fe.ResultMsg});
            }
            catch(Exception ex)
            {
                FrameSystem.showMsgForm("���������ȸ����",new string[] {"",ex.Message});
            }
        }


		/// <summary>
		/// �����ฦ ����
		/// </summary>
		private void SelectContract()
		{

			int curRow = cm.Position;
			if(curRow < 0) return;	// �����Ͱ� ������ �������� �ʴ´�.

			string ContractSeq    = dt.Rows[curRow]["ContractSeq"].ToString();			
			string ContractName   = dt.Rows[curRow]["ContractName"].ToString();			
			string StartDay       = dt.Rows[curRow]["ContStartDay"].ToString();			
			string EndDay         = dt.Rows[curRow]["ContEndDay"].ToString();
			string AgencyName     = dt.Rows[curRow]["AgencyName"].ToString(); 
			string AdvertiserName = dt.Rows[curRow]["AdvertiserName"].ToString(); 
	
			Opener.SetContract(ContractSeq, ContractName, StartDay, EndDay, AgencyName, AdvertiserName);
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