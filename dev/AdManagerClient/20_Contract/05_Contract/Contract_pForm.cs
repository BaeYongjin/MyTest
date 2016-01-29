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
    /// Contract_pForm�� ���� ��� �����Դϴ�.
    /// </summary>
    public class Contract_pForm : System.Windows.Forms.Form
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
//        private System.ComponentModel.IContainer components;
        private Janus.Windows.EditControls.UIButton btnSearch;
        private Janus.Windows.GridEX.GridEX grdExClientList;
        private AdManagerClient.Contract_pDs contract_pDs;
        private System.Data.DataView dvClient;
        
        private ContractControl parentCtl = null;

        public Contract_pForm()
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
        public Contract_pForm(ContractControl sender)
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

        // ����� ������
        ClientModel clientModel  = new ClientModel();	// ��ü���౤����������
        
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
            Janus.Windows.GridEX.GridEXLayout grdExClientList_DesignTimeLayout = new Janus.Windows.GridEX.GridEXLayout();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Contract_pForm));
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
            this.grdExClientList = new Janus.Windows.GridEX.GridEX();
            this.dvClient = new System.Data.DataView();
            this.contract_pDs = new AdManagerClient.Contract_pDs();
            this.pnlSearch.SuspendLayout();
            this.pnlBtn.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdExClientList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dvClient)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.contract_pDs)).BeginInit();
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
            this.pnlSearch.TabIndex = 1;
            // 
            // btnSearch
            // 
            this.btnSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSearch.Location = new System.Drawing.Point(528, 8);
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
            this.ebSearchKey.Size = new System.Drawing.Size(248, 21);
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
            this.cbSearchMedia.Size = new System.Drawing.Size(120, 21);
            this.cbSearchMedia.TabIndex = 1;
            this.cbSearchMedia.Text = "��ü����";
            this.cbSearchMedia.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            // 
            // cbSearchRap
            // 
            this.cbSearchRap.ComboStyle = Janus.Windows.EditControls.ComboStyle.DropDownList;
            this.cbSearchRap.Location = new System.Drawing.Point(136, 8);
            this.cbSearchRap.Name = "cbSearchRap";
            this.cbSearchRap.Size = new System.Drawing.Size(120, 21);
            this.cbSearchRap.TabIndex = 2;
            this.cbSearchRap.Text = "���缱��";
            this.cbSearchRap.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            // 
            // cbSearchAgency
            // 
            this.cbSearchAgency.ComboStyle = Janus.Windows.EditControls.ComboStyle.DropDownList;
            this.cbSearchAgency.Location = new System.Drawing.Point(264, 8);
            this.cbSearchAgency.Name = "cbSearchAgency";
            this.cbSearchAgency.Size = new System.Drawing.Size(120, 21);
            this.cbSearchAgency.TabIndex = 3;
            this.cbSearchAgency.Text = "����缱��";
            this.cbSearchAgency.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            // 
            // cbSearchAdvertiser
            // 
            this.cbSearchAdvertiser.ComboStyle = Janus.Windows.EditControls.ComboStyle.DropDownList;
            this.cbSearchAdvertiser.Location = new System.Drawing.Point(392, 8);
            this.cbSearchAdvertiser.Name = "cbSearchAdvertiser";
            this.cbSearchAdvertiser.Size = new System.Drawing.Size(120, 21);
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
            this.pnlBtn.TabIndex = 10;
            // 
            // btnClose
            // 
            this.btnClose.BackColor = System.Drawing.SystemColors.Control;
            this.btnClose.Location = new System.Drawing.Point(333, 9);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(70, 23);
            this.btnClose.TabIndex = 9;
            this.btnClose.Text = "�ݱ�";
            this.btnClose.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnOk
            // 
            this.btnOk.BackColor = System.Drawing.SystemColors.Control;
            this.btnOk.Location = new System.Drawing.Point(245, 9);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(70, 23);
            this.btnOk.TabIndex = 8;
            this.btnOk.Text = "Ȯ��";
            this.btnOk.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.WhiteSmoke;
            this.panel1.Controls.Add(this.grdExClientList);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 62);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(656, 358);
            this.panel1.TabIndex = 18;
            // 
            // grdExClientList
            // 
            this.grdExClientList.AllowEdit = Janus.Windows.GridEX.InheritableBoolean.False;
            this.grdExClientList.AlternatingColors = true;
            this.grdExClientList.BorderStyle = Janus.Windows.GridEX.BorderStyle.None;
            this.grdExClientList.DataSource = this.dvClient;
            grdExClientList_DesignTimeLayout.LayoutString = resources.GetString("grdExClientList_DesignTimeLayout.LayoutString");
            this.grdExClientList.DesignTimeLayout = grdExClientList_DesignTimeLayout;
            this.grdExClientList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdExClientList.EditorsControlStyle.ButtonAppearance = Janus.Windows.GridEX.ButtonAppearance.Regular;
            this.grdExClientList.EmptyRows = true;
            this.grdExClientList.FocusCellFormatStyle.BackColor = System.Drawing.SystemColors.Highlight;
            this.grdExClientList.FocusCellFormatStyle.ForeColor = System.Drawing.Color.White;
            this.grdExClientList.FocusStyle = Janus.Windows.GridEX.FocusStyle.None;
            this.grdExClientList.Font = new System.Drawing.Font("����ü", 9F);
            this.grdExClientList.FrozenColumns = 2;
            this.grdExClientList.GridLineColor = System.Drawing.Color.Silver;
            this.grdExClientList.GridLines = Janus.Windows.GridEX.GridLines.Vertical;
            this.grdExClientList.GridLineStyle = Janus.Windows.GridEX.GridLineStyle.Solid;
            this.grdExClientList.GroupByBoxVisible = false;
            this.grdExClientList.HeaderFormatStyle.TextAlignment = Janus.Windows.GridEX.TextAlignment.Center;
            this.grdExClientList.HideSelection = Janus.Windows.GridEX.HideSelection.HighlightInactive;
            this.grdExClientList.Location = new System.Drawing.Point(0, 0);
            this.grdExClientList.Name = "grdExClientList";
            this.grdExClientList.ScrollBars = Janus.Windows.GridEX.ScrollBars.Vertical;
            this.grdExClientList.SelectedInactiveFormatStyle.BackColor = System.Drawing.Color.Gold;
            this.grdExClientList.Size = new System.Drawing.Size(656, 358);
            this.grdExClientList.TabIndex = 7;
            this.grdExClientList.TabKeyBehavior = Janus.Windows.GridEX.TabKeyBehavior.ControlNavigation;
            this.grdExClientList.ThemedAreas = ((Janus.Windows.GridEX.ThemedArea)(((((((((Janus.Windows.GridEX.ThemedArea.ScrollBars | Janus.Windows.GridEX.ThemedArea.EditControls)
                        | Janus.Windows.GridEX.ThemedArea.Headers)
                        | Janus.Windows.GridEX.ThemedArea.GroupByBox)
                        | Janus.Windows.GridEX.ThemedArea.GroupRows)
                        | Janus.Windows.GridEX.ThemedArea.ControlBorder)
                        | Janus.Windows.GridEX.ThemedArea.Cards)
                        | Janus.Windows.GridEX.ThemedArea.Gridlines)
                        | Janus.Windows.GridEX.ThemedArea.CheckBoxes)));
            this.grdExClientList.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            // 
            // dvClient
            // 
            this.dvClient.Table = this.contract_pDs.Clients;
            // 
            // contract_pDs
            // 
            this.contract_pDs.DataSetName = "Contract_pDs";
            this.contract_pDs.Locale = new System.Globalization.CultureInfo("en-US");
            this.contract_pDs.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // Contract_pForm
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(6, 14);
            this.ClientSize = new System.Drawing.Size(656, 462);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.pnlBtn);
            this.Controls.Add(this.pnlSearch);
            this.Name = "Contract_pForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "�����ָ��";
            this.Load += new System.EventHandler(this.UserControl_Load);
            this.pnlSearch.ResumeLayout(false);
            this.pnlSearch.PerformLayout();
            this.pnlBtn.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdExClientList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dvClient)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.contract_pDs)).EndInit();
            this.ResumeLayout(false);

		}
        #endregion

        #region ��Ʈ�� �ε�
        private void UserControl_Load(object sender, System.EventArgs e)
        {
            // �����Ͱ����� ��ü����
            dt = ((DataView)grdExClientList.DataSource).Table;  
            cm = (CurrencyManager) this.BindingContext[grdExClientList.DataSource]; 
            cm.PositionChanged += new System.EventHandler(OnGrdRowChanged); 

            // ��Ʈ�� �ʱ�ȭ
            InitControl();	
        }

        #endregion

        #region ��Ʈ�� �ʱ�ȭ
        private void InitControl()
        {
            InitCombo();

			SearchClient();

        }

        private void InitCombo()
        {
            Init_MediaCode();
            Init_RapCode();
            Init_AgencyCode();
            Init_AdvertiserCode();
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
                Utility.SetDataTable(contract_pDs.Medias, mediaCodeModel.MediaCodeDataSet);				
            }

            // �˻������� �޺�
            this.cbSearchMedia.Items.Clear();
			
            // �޺��ڽ��� ��Ʈ�� �ڵ����� ���� Item�迭�� ����
            Janus.Windows.EditControls.UIComboBoxItem[] comboItems = new Janus.Windows.EditControls.UIComboBoxItem[mediaCodeModel.ResultCnt + 1];

            comboItems[0] = new Janus.Windows.EditControls.UIComboBoxItem("��ü����","00");
			
            for(int i=0;i<mediaCodeModel.ResultCnt;i++)
            {
                DataRow row = contract_pDs.Medias.Rows[i];

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
            /* ��ȸ�κл����ϰ� 1�� ���� 
             * ��ȸ �κ��� �츮���� �� �κп������� Ǯ�� ���� �ε��� ���� ��
            // ���� ��ȸ�Ѵ�.
            MediaRapCodeModel mediaRapCodeModel = new MediaRapCodeModel();
            new MediaRapCodeManager(systemModel, commonModel).GetMediaRapCodeList(mediaRapCodeModel);
			
            if (mediaRapCodeModel.ResultCD.Equals("0000"))
            {
                // �����ͼ¿� ����
                Utility.SetDataTable(contract_pDs.MediaRaps, mediaRapCodeModel.MediaRapCodeDataSet);				
            }

            // �˻������� �޺�
            this.cbSearchRap.Items.Clear();
           
            // �޺��ڽ��� ��Ʈ�� �ڵ����� ���� Item�迭�� ����
            Janus.Windows.EditControls.UIComboBoxItem[] comboItems = new Janus.Windows.EditControls.UIComboBoxItem[mediaRapCodeModel.ResultCnt + 1];

            comboItems[0] = new Janus.Windows.EditControls.UIComboBoxItem("�̵�����","00");
			
            for(int i=0;i<mediaRapCodeModel.ResultCnt;i++)
            {
                DataRow row = contract_pDs.MediaRaps.Rows[i];

                string val = row["RapCode"].ToString();
                string txt = row["RapName"].ToString();
                comboItems[i+1] = new Janus.Windows.EditControls.UIComboBoxItem(txt,val);
            }
            // �޺��� ��Ʈ
            this.cbSearchRap.Items.AddRange(comboItems);
            this.cbSearchRap.SelectedIndex = 0;

			Application.DoEvents();
            */
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

            Utility.SetDataTable(contract_pDs.MediaRaps, ds);
            // �˻������� �޺�
            this.cbSearchRap.Items.Clear();
            // �޺��ڽ��� ��Ʈ�� �ڵ����� ���� Item�迭�� ����
            Janus.Windows.EditControls.UIComboBoxItem[] comboItems = new Janus.Windows.EditControls.UIComboBoxItem[2];

            comboItems[0] = new Janus.Windows.EditControls.UIComboBoxItem("�̵�����", "00");

            for (int i = 0; i < 1; i++)
            {
                DataRow row = contract_pDs.MediaRaps.Rows[i];

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
                Utility.SetDataTable(contract_pDs.Agencys, agencyCodeModel.AgencyCodeDataSet);				
            }

            // �˻������� �޺�
            this.cbSearchAgency.Items.Clear();
			
            // �޺��ڽ��� ��Ʈ�� �ڵ����� ���� Item�迭�� ����
            Janus.Windows.EditControls.UIComboBoxItem[] comboItems = new Janus.Windows.EditControls.UIComboBoxItem[agencyCodeModel.ResultCnt + 1];

            comboItems[0] = new Janus.Windows.EditControls.UIComboBoxItem("����缱��","00");
			
            for(int i=0;i<agencyCodeModel.ResultCnt;i++)
            {
                DataRow row = contract_pDs.Agencys.Rows[i];

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
                Utility.SetDataTable(contract_pDs.Advertisers, clientModel.ClientDataSet);				
            }

            // �˻������� �޺�
            this.cbSearchAdvertiser.Items.Clear();
          
            // �޺��ڽ��� ��Ʈ�� �ڵ����� ���� Item�迭�� ����
            Janus.Windows.EditControls.UIComboBoxItem[] comboItems = new Janus.Windows.EditControls.UIComboBoxItem[clientModel.ResultCnt + 1];

            comboItems[0] = new Janus.Windows.EditControls.UIComboBoxItem("�����ּ���","00");
			
            for(int i=0;i<clientModel.ResultCnt;i++)
            {
                DataRow row = contract_pDs.Advertisers.Rows[i];

                string val = row["AdvertiserCode"].ToString();
                string txt = row["AdvertiserName"].ToString();
                comboItems[i+1] = new Janus.Windows.EditControls.UIComboBoxItem(txt,val);
            }
			
            // �޺��� ��Ʈ
            this.cbSearchAdvertiser.Items.AddRange(comboItems);
            this.cbSearchAdvertiser.SelectedIndex = 0;
		
			Application.DoEvents();
        }

        private void InitCombo_Level()
        {
			
            if(commonModel.UserLevel=="20")
            {
                cbSearchMedia.SelectedValue = commonModel.MediaCode;			
                cbSearchMedia.ReadOnly = true;					
            }
            else
            {
                cbSearchMedia.SelectedValue = FrameSystem._HANATV; // �ϳ�TV�� �⺻������ �Ѵ�.
            }
            if(commonModel.UserLevel=="30")
            {
                cbSearchRap.SelectedValue = commonModel.RapCode;			
                cbSearchRap.ReadOnly = true;				
            }
            if(commonModel.UserLevel=="40")
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
            if(grdExClientList.RowCount > 0)
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
            SearchClient();
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
                SearchClient();
            }
        }

        #endregion

        #region ó���޼ҵ�

        /// <summary>
        /// ��ü���౤���ָ�� ��ȸ
        /// </summary>
        private void SearchClient()
        {
            StatusMessage("��ü���౤���� ������ ��ȸ�մϴ�.");

            try
            {
                clientModel.Init();
                // �����͸𵨿� ������ ������ ��Ʈ�Ѵ�.
                if(IsNewSearchKey)
                {
                    clientModel.SearchKey = "";
                }
                else
                {
                    clientModel.SearchKey  = ebSearchKey.Text;
                }

                clientModel.SearchMediaName = cbSearchMedia.SelectedItem.Value.ToString();
                clientModel.SearchRapName = cbSearchRap.SelectedItem.Value.ToString();
                clientModel.SearchMediaAgency = cbSearchAgency.SelectedItem.Value.ToString();
                clientModel.SearchAdvertiserName = cbSearchAdvertiser.SelectedItem.Value.ToString();


                //LastOrder = 0;


                // ��ü���౤���ָ����ȸ ���񽺸� ȣ���Ѵ�.
                new ClientManager(systemModel,commonModel).GetClientList(clientModel);

                if (clientModel.ResultCD.Equals("0000"))
                {					
                    Utility.SetDataTable(contract_pDs.Clients, clientModel.ClientDataSet);				
                    StatusMessage(clientModel.ResultCnt + "���� ��ü���౤���� ������ ��ȸ�Ǿ����ϴ�.");
//                    LastOrder = clientModel.ResultCnt;
//					
//                    if(canUpdate)
//                    {
//                        AddSchChoice();									
//                    }					
//                    SetClientDetailText();
                }
            }
            catch(FrameException fe) 
            {
                FrameSystem.showMsgForm("��ü���౤������ȸ����", new string[] {fe.ErrCode, fe.ResultMsg});				
            }
            catch(Exception ex)
            {
                FrameSystem.showMsgForm("��ü���౤������ȸ����",new string[] {"",ex.Message});
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
            //�ڵ� �Ѱ���
            contractModel.MediaCode = dt.Rows[grdExClientList.GetRow().RowIndex]["MediaCode"].ToString();
            contractModel.RapCode = dt.Rows[grdExClientList.GetRow().RowIndex]["RapCode"].ToString();
            contractModel.AgencyCode = dt.Rows[grdExClientList.GetRow().RowIndex]["AgencyCode"].ToString();
            contractModel.AdvertiserCode = dt.Rows[grdExClientList.GetRow().RowIndex]["AdvertiserCode"].ToString();
            //�̸� �Ѱ���
            contractModel.MediaName = dt.Rows[grdExClientList.GetRow().RowIndex]["MediaName"].ToString();
            contractModel.RapName = dt.Rows[grdExClientList.GetRow().RowIndex]["RapName"].ToString();
            contractModel.AgencyName = dt.Rows[grdExClientList.GetRow().RowIndex]["AgencyName"].ToString();
            contractModel.AdvertiserName = dt.Rows[grdExClientList.GetRow().RowIndex]["AdvertiserName"].ToString();

            parentCtl.adOn_AddContract(contractModel);
            this.Close();

        }

        private void btnClose_Click(object sender, System.EventArgs e)
        {
            this.Close();
        }



    }
}