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
    public class ContractItem_pForm : System.Windows.Forms.Form
    {

        #region ȭ�� ������Ʈ, ������, �Ҹ���
        private System.Windows.Forms.Panel pnlSearch;
        private Janus.Windows.GridEX.EditControls.EditBox ebSearchKey;
        private Janus.Windows.EditControls.UIComboBox cbSearchMedia;
        private Janus.Windows.EditControls.UIComboBox cbSearchRap;
        private Janus.Windows.EditControls.UIComboBox cbSearchAgency;
        private Janus.Windows.EditControls.UIComboBox cbSearchAdvertiser;
        private System.Windows.Forms.Panel pnlBtn;
        private Janus.Windows.EditControls.UIButton btnClose;
        private Janus.Windows.EditControls.UIButton btnOk;
        private System.Windows.Forms.Panel panel1;
        private Janus.Windows.GridEX.GridEX grdExContractList;
        private System.Data.DataView dvContract;
//        private System.ComponentModel.IContainer components;
        private Janus.Windows.EditControls.UIButton btnSearch;
        private AdManagerClient.ContractItem_pDs contractItem_pDs;
        
        private ContractItemControl parentCtl = null;

        public ContractItem_pForm()
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
        public ContractItem_pForm(ContractItemControl sender)
        {
            //
            // Windows Form �����̳� ������ �ʿ��մϴ�.
            //
            InitializeComponent();

            //
            
            //
            
            parentCtl = sender;
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ContractItem_pForm));
            this.pnlSearch = new System.Windows.Forms.Panel();
            this.btnSearch = new Janus.Windows.EditControls.UIButton();
            this.ebSearchKey = new Janus.Windows.GridEX.EditControls.EditBox();
            this.cbSearchMedia = new Janus.Windows.EditControls.UIComboBox();
            this.cbSearchRap = new Janus.Windows.EditControls.UIComboBox();
            this.cbSearchAgency = new Janus.Windows.EditControls.UIComboBox();
            this.cbSearchAdvertiser = new Janus.Windows.EditControls.UIComboBox();
            this.pnlBtn = new System.Windows.Forms.Panel();
            this.btnClose = new Janus.Windows.EditControls.UIButton();
            this.btnOk = new Janus.Windows.EditControls.UIButton();
            this.panel1 = new System.Windows.Forms.Panel();
            this.grdExContractList = new Janus.Windows.GridEX.GridEX();
            this.dvContract = new System.Data.DataView();
            this.contractItem_pDs = new AdManagerClient.ContractItem_pDs();
            this.pnlSearch.SuspendLayout();
            this.pnlBtn.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdExContractList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dvContract)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.contractItem_pDs)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlSearch
            // 
            this.pnlSearch.BackColor = System.Drawing.SystemColors.Window;
            this.pnlSearch.Controls.Add(this.btnSearch);
            this.pnlSearch.Controls.Add(this.ebSearchKey);
            this.pnlSearch.Controls.Add(this.cbSearchMedia);
            this.pnlSearch.Controls.Add(this.cbSearchRap);
            this.pnlSearch.Controls.Add(this.cbSearchAgency);
            this.pnlSearch.Controls.Add(this.cbSearchAdvertiser);
            this.pnlSearch.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlSearch.Location = new System.Drawing.Point(0, 0);
            this.pnlSearch.Name = "pnlSearch";
            this.pnlSearch.Size = new System.Drawing.Size(656, 62);
            this.pnlSearch.TabIndex = 4;
            // 
            // btnSearch
            // 
            this.btnSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSearch.Location = new System.Drawing.Point(544, 8);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(104, 24);
            this.btnSearch.TabIndex = 6;
            this.btnSearch.Text = "�� ȸ";
            this.btnSearch.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // ebSearchKey
            // 
            this.ebSearchKey.Location = new System.Drawing.Point(8, 32);
            this.ebSearchKey.Name = "ebSearchKey";
            this.ebSearchKey.Size = new System.Drawing.Size(248, 23);
            this.ebSearchKey.TabIndex = 5;
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
            // cbSearchAdvertiser
            // 
            this.cbSearchAdvertiser.ComboStyle = Janus.Windows.EditControls.ComboStyle.DropDownList;
            this.cbSearchAdvertiser.Location = new System.Drawing.Point(392, 8);
            this.cbSearchAdvertiser.Name = "cbSearchAdvertiser";
            this.cbSearchAdvertiser.Size = new System.Drawing.Size(120, 23);
            this.cbSearchAdvertiser.TabIndex = 4;
            this.cbSearchAdvertiser.Text = "�����ּ���";
            this.cbSearchAdvertiser.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            // 
            // pnlBtn
            // 
            this.pnlBtn.BackColor = System.Drawing.Color.WhiteSmoke;
            this.pnlBtn.Controls.Add(this.btnClose);
            this.pnlBtn.Controls.Add(this.btnOk);
            this.pnlBtn.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlBtn.Location = new System.Drawing.Point(0, 420);
            this.pnlBtn.Name = "pnlBtn";
            this.pnlBtn.Size = new System.Drawing.Size(656, 42);
            this.pnlBtn.TabIndex = 17;
            // 
            // btnClose
            // 
            this.btnClose.BackColor = System.Drawing.SystemColors.Control;
            this.btnClose.Location = new System.Drawing.Point(328, 9);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(104, 24);
            this.btnClose.TabIndex = 9;
            this.btnClose.Text = "�ݱ�";
            this.btnClose.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnOk
            // 
            this.btnOk.BackColor = System.Drawing.SystemColors.Control;
            this.btnOk.Location = new System.Drawing.Point(216, 8);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(104, 24);
            this.btnOk.TabIndex = 8;
            this.btnOk.Text = "Ȯ��";
            this.btnOk.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.WhiteSmoke;
            this.panel1.Controls.Add(this.grdExContractList);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 62);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(656, 358);
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
            this.grdExContractList.FocusStyle = Janus.Windows.GridEX.FocusStyle.None;
            this.grdExContractList.Font = new System.Drawing.Font("���� ���", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.grdExContractList.GridLineColor = System.Drawing.Color.Silver;
            this.grdExContractList.GridLines = Janus.Windows.GridEX.GridLines.Vertical;
            this.grdExContractList.GridLineStyle = Janus.Windows.GridEX.GridLineStyle.Solid;
            this.grdExContractList.GroupByBoxVisible = false;
            this.grdExContractList.Location = new System.Drawing.Point(0, 0);
            this.grdExContractList.Name = "grdExContractList";
            this.grdExContractList.ScrollBars = Janus.Windows.GridEX.ScrollBars.Vertical;
            this.grdExContractList.Size = new System.Drawing.Size(656, 358);
            this.grdExContractList.TabIndex = 7;
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
            this.dvContract.Table = this.contractItem_pDs.Contract;
            // 
            // contractItem_pDs
            // 
            this.contractItem_pDs.DataSetName = "ContractItem_pDs";
            this.contractItem_pDs.Locale = new System.Globalization.CultureInfo("en-US");
            this.contractItem_pDs.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // ContractItem_pForm
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(6, 16);
            this.ClientSize = new System.Drawing.Size(656, 462);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.pnlBtn);
            this.Controls.Add(this.pnlSearch);
            this.Font = new System.Drawing.Font("���� ���", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.MaximizeBox = false;
            this.Name = "ContractItem_pForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "������˻�";
            this.Load += new System.EventHandler(this.UserControl_Load);
            this.pnlSearch.ResumeLayout(false);
            this.pnlSearch.PerformLayout();
            this.pnlBtn.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdExContractList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dvContract)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.contractItem_pDs)).EndInit();
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

        }

        private void InitCombo()
        {
            Init_MediaCode();
            Init_RapCode();
            Init_AgencyCode();
            Init_AdvertiserCode();

        }

        private void Init_MediaCode()
        {
            // ��ü�� ��ȸ�Ѵ�.
            MediaCodeModel mediaCodeModel = new MediaCodeModel();
            new MediaCodeManager(systemModel, commonModel).GetMediaCodeList(mediaCodeModel);
			
            if (mediaCodeModel.ResultCD.Equals("0000"))
            {
                // �����ͼ¿� ����
                Utility.SetDataTable(contractItem_pDs.Medias, mediaCodeModel.MediaCodeDataSet);				
            }

            // �˻������� �޺�
            this.cbSearchMedia.Items.Clear();
			
            // �޺��ڽ��� ��Ʈ�� �ڵ����� ���� Item�迭�� ����
            Janus.Windows.EditControls.UIComboBoxItem[] comboItems = new Janus.Windows.EditControls.UIComboBoxItem[mediaCodeModel.ResultCnt + 1];

            comboItems[0] = new Janus.Windows.EditControls.UIComboBoxItem("��ü����","00");
			
            for(int i=0;i<mediaCodeModel.ResultCnt;i++)
            {
                DataRow row = contractItem_pDs.Medias.Rows[i];

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
            /*
            // ���� ��ȸ�Ѵ�.
            MediaRapCodeModel mediaRapCodeModel = new MediaRapCodeModel();
            new MediaRapCodeManager(systemModel, commonModel).GetMediaRapCodeList(mediaRapCodeModel);
			
            if (mediaRapCodeModel.ResultCD.Equals("0000"))
            {
                // �����ͼ¿� ����
                Utility.SetDataTable(contractItem_pDs.MediaRaps, mediaRapCodeModel.MediaRapCodeDataSet);				
            }

            // �˻������� �޺�
            this.cbSearchRap.Items.Clear();
           
            // �޺��ڽ��� ��Ʈ�� �ڵ����� ���� Item�迭�� ����
            Janus.Windows.EditControls.UIComboBoxItem[] comboItems = new Janus.Windows.EditControls.UIComboBoxItem[mediaRapCodeModel.ResultCnt + 1];

            comboItems[0] = new Janus.Windows.EditControls.UIComboBoxItem("�̵�����","00");
			
            for(int i=0;i<mediaRapCodeModel.ResultCnt;i++)
            {
                DataRow row = contractItem_pDs.MediaRaps.Rows[i];

                string val = row["RapCode"].ToString();
                string txt = row["RapName"].ToString();
                comboItems[i+1] = new Janus.Windows.EditControls.UIComboBoxItem(txt,val);
            }
            // �޺��� ��Ʈ
            this.cbSearchRap.Items.AddRange(comboItems);
            this.cbSearchRap.SelectedIndex = 0;

            Application.DoEvents();
            */
            //���� ����
            this.cbSearchRap.Items.Clear();
            DataSet ds = new DataSet("rapset");
            DataTable dt = new DataTable("rap");
            dt.Columns.Add("RapCode", typeof(string));
            dt.Columns.Add("RapName", typeof(string));
            DataRow nRow = dt.NewRow();
            nRow["RapCode"] = "1";
            nRow["RapName"] = "����� ����";
            dt.Rows.Add(nRow);
            ds.Tables.Add(dt);

            Utility.SetDataTable(contractItem_pDs.MediaRaps, ds);
            // �˻������� �޺�
            this.cbSearchRap.Items.Clear();
            // �޺��ڽ��� ��Ʈ�� �ڵ����� ���� Item�迭�� ����
            Janus.Windows.EditControls.UIComboBoxItem[] comboItems = new Janus.Windows.EditControls.UIComboBoxItem[2];

            comboItems[0] = new Janus.Windows.EditControls.UIComboBoxItem("�̵�����", "00");

            for (int i = 0; i < 1; i++)
            {
                DataRow row = contractItem_pDs.MediaRaps.Rows[i];

                string val = row["RapCode"].ToString();
                string txt = row["RapName"].ToString();
                comboItems[i + 1] = new Janus.Windows.EditControls.UIComboBoxItem(txt, val);
            }
            // �޺��� ��Ʈ
            this.cbSearchRap.Items.AddRange(comboItems);
            this.cbSearchRap.SelectedIndex = 1;
            this.cbSearchRap.ReadOnly = true;
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
                Utility.SetDataTable(contractItem_pDs.Agencys, agencyCodeModel.AgencyCodeDataSet);				
            }

            // �˻������� �޺�
            this.cbSearchAgency.Items.Clear();
			
            // �޺��ڽ��� ��Ʈ�� �ڵ����� ���� Item�迭�� ����
            Janus.Windows.EditControls.UIComboBoxItem[] comboItems = new Janus.Windows.EditControls.UIComboBoxItem[agencyCodeModel.ResultCnt + 1];

            comboItems[0] = new Janus.Windows.EditControls.UIComboBoxItem("����缱��","00");
			
            for(int i=0;i<agencyCodeModel.ResultCnt;i++)
            {
                DataRow row = contractItem_pDs.Agencys.Rows[i];

                string val = row["AgencyCode"].ToString();
                string txt = row["AgencyName"].ToString();
                comboItems[i+1] = new Janus.Windows.EditControls.UIComboBoxItem(txt,val);
            }
			
            // �޺��� ��Ʈ
            this.cbSearchAgency.Items.AddRange(comboItems);
            this.cbSearchAgency.SelectedIndex = 0;

            Application.DoEvents();
        }

        private void Init_AdvertiserCode()
        {
            // �����ָ� ��ȸ�Ѵ�.
            ClientModel clientModel = new ClientModel();
            new ClientManager(systemModel, commonModel).GetAdvertiserList(clientModel);
			
            if (clientModel.ResultCD.Equals("0000"))
            {
                // �����ͼ¿� ����
                Utility.SetDataTable(contractItem_pDs.Advertisers, clientModel.ClientDataSet);				
            }

            // �˻������� �޺�
            this.cbSearchAdvertiser.Items.Clear();
          
            // �޺��ڽ��� ��Ʈ�� �ڵ����� ���� Item�迭�� ����
            Janus.Windows.EditControls.UIComboBoxItem[] comboItems = new Janus.Windows.EditControls.UIComboBoxItem[clientModel.ResultCnt + 1];

            comboItems[0] = new Janus.Windows.EditControls.UIComboBoxItem("�����ּ���","00");
			
            for(int i=0;i<clientModel.ResultCnt;i++)
            {
                DataRow row = contractItem_pDs.Advertisers.Rows[i];

                string val = row["AdvertiserCode"].ToString();
                string txt = row["AdvertiserName"].ToString();
                comboItems[i+1] = new Janus.Windows.EditControls.UIComboBoxItem(txt,val);
            }
			
            // �޺��� ��Ʈ
            this.cbSearchAdvertiser.Items.AddRange(comboItems);
            this.cbSearchAdvertiser.SelectedIndex = 0;
		
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
                contractModel.AdvertiserCode = cbSearchAdvertiser.SelectedValue.ToString();
                //������ ���� ����(10:��� , 20:����)
                contractModel.State          = "10"; 
          

                if(IsNewSearchKey)
                {
                    contractModel.SearchKey = "";
                }
                else
                {
                    contractModel.SearchKey  = ebSearchKey.Text;
                }

                // ���� ��� ��� ���񽺸� ȣ���Ѵ�.
                new ContractManager(systemModel,commonModel).GetContractList(contractModel);

                if (contractModel.ResultCD.Equals("0000"))
                {
                    Utility.SetDataTable(contractItem_pDs.Contract, contractModel.ContractDataSet);				
                    StatusMessage(contractModel.ResultCnt + "���� ������� ������ ��ȸ�Ǿ����ϴ�.");
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
			if(grdExContractList.GetRow().RowIndex < 0) return;	// �����Ͱ� ������ �������� �ʴ´�.
			SelectContract();
			this.Close();		
		}

		private void SelectContract()
		{
			contractModel.MediaCode      = dt.Rows[grdExContractList.GetRow().RowIndex]["MediaCode"].ToString();
			contractModel.RapCode        = dt.Rows[grdExContractList.GetRow().RowIndex]["RapCode"].ToString();
			contractModel.AgencyCode     = dt.Rows[grdExContractList.GetRow().RowIndex]["AgencyCode"].ToString();
			contractModel.AdvertiserCode = dt.Rows[grdExContractList.GetRow().RowIndex]["AdvertiserCode"].ToString();
			contractModel.ContractSeq    = dt.Rows[grdExContractList.GetRow().RowIndex]["ContractSeq"].ToString();
			contractModel.ContractName   = dt.Rows[grdExContractList.GetRow().RowIndex]["ContractName"].ToString();
			contractModel.ContStartDay   = dt.Rows[grdExContractList.GetRow().RowIndex]["ContStartDay"].ToString();
			contractModel.ContEndDay     = dt.Rows[grdExContractList.GetRow().RowIndex]["ContEndDay"].ToString();
			contractModel.AdTime		 = dt.Rows[grdExContractList.GetRow().RowIndex]["AdTime"].ToString();
		
			parentCtl.adOn_AddContract(contractModel);

		}
    }
}