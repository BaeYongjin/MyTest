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
	/// Advertiser_pForm�� ���� ��� �����Դϴ�.
	/// </summary>
	/// 
    
	public class Advertiser_pForm : System.Windows.Forms.Form
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
		ClientModel clientModel = new ClientModel();
				
		ClientControl AdvertiserCtl = null;

		// ȭ��ó���� ����
		bool IsNewSearchKey		  = true;					// �˻����Է� ����
		CurrencyManager ccm        = null;					// ������ �׸����� ���濡 ���� �����ͼ� ������ ���Ͽ�			
		DataTable       dtt        = null;

		public string        Search_AdvertiserFlag = null;
		
		#endregion



		private System.Windows.Forms.Panel panel4;
		private System.Windows.Forms.ImageList imageList;
		private System.Windows.Forms.Panel pnlBtn;
		private Janus.Windows.EditControls.UIButton btnClose;
		private Janus.Windows.EditControls.UIButton btnOk;
		private Janus.Windows.EditControls.UIButton btnSearch;
		private Janus.Windows.GridEX.EditControls.EditBox ebSearchKey;
		private Janus.Windows.GridEX.GridEX grdExAdvertiserList;
		private System.Data.DataView dvClient;
		private AdManagerClient._20_Contract._04_Client.MediaAgencyDs clientDs;
		private Janus.Windows.EditControls.UIComboBox cbSearchRap;
		private System.ComponentModel.IContainer components;

		/// <summary>
		/// ������ �Ѱܾ� �� ��
		/// </summary>
		/// <param name="sender"></param>
		public Advertiser_pForm(ClientControl sender)
		{
			//
			// Windows Form �����̳� ������ �ʿ��մϴ�.
			//
			InitializeComponent();

			//
			
			//
            
			AdvertiserCtl = sender;
		}

		/// <summary>
		/// �Ϲݻ����
		/// </summary>
		public Advertiser_pForm(ClientControl sender, string AdvertiserFlag)
		{
			//
			// Windows Form �����̳� ������ �ʿ��մϴ�.
			//
			InitializeComponent();

			//
			
			//
            			
			AdvertiserCtl = sender;
			Search_AdvertiserFlag = AdvertiserFlag;
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

		#region Windows Form �����̳ʿ��� ������ �ڵ�
		/// <summary>
		/// �����̳� ������ �ʿ��� �޼����Դϴ�.
		/// �� �޼����� ������ �ڵ� ������� �������� ���ʽÿ�.
		/// </summary>
		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            Janus.Windows.GridEX.GridEXLayout grdExAdvertiserList_Layout_0 = new Janus.Windows.GridEX.GridEXLayout();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Advertiser_pForm));
            this.dvClient = new System.Data.DataView();
            this.clientDs = new AdManagerClient._20_Contract._04_Client.MediaAgencyDs();
            this.panel4 = new System.Windows.Forms.Panel();
            this.cbSearchRap = new Janus.Windows.EditControls.UIComboBox();
            this.ebSearchKey = new Janus.Windows.GridEX.EditControls.EditBox();
            this.btnSearch = new Janus.Windows.EditControls.UIButton();
            this.imageList = new System.Windows.Forms.ImageList(this.components);
            this.pnlBtn = new System.Windows.Forms.Panel();
            this.btnClose = new Janus.Windows.EditControls.UIButton();
            this.btnOk = new Janus.Windows.EditControls.UIButton();
            this.grdExAdvertiserList = new Janus.Windows.GridEX.GridEX();
            ((System.ComponentModel.ISupportInitialize)(this.dvClient)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.clientDs)).BeginInit();
            this.panel4.SuspendLayout();
            this.pnlBtn.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdExAdvertiserList)).BeginInit();
            this.SuspendLayout();
            // 
            // dvClient
            // 
            this.dvClient.Table = this.clientDs.Advertisers_C;
            // 
            // clientDs
            // 
            this.clientDs.DataSetName = "ClientDs";
            this.clientDs.Locale = new System.Globalization.CultureInfo("en-US");
            this.clientDs.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // panel4
            // 
            this.panel4.BackColor = System.Drawing.Color.White;
            this.panel4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel4.Controls.Add(this.cbSearchRap);
            this.panel4.Controls.Add(this.ebSearchKey);
            this.panel4.Controls.Add(this.btnSearch);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel4.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.panel4.Location = new System.Drawing.Point(0, 0);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(400, 32);
            this.panel4.TabIndex = 1;
            // 
            // cbSearchRap
            // 
            this.cbSearchRap.ComboStyle = Janus.Windows.EditControls.ComboStyle.DropDownList;
            this.cbSearchRap.Location = new System.Drawing.Point(8, 5);
            this.cbSearchRap.Name = "cbSearchRap";
            this.cbSearchRap.Size = new System.Drawing.Size(120, 21);
            this.cbSearchRap.TabIndex = 1;
            this.cbSearchRap.Text = "�̵�����";
            this.cbSearchRap.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            // 
            // ebSearchKey
            // 
            this.ebSearchKey.ButtonFont = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ebSearchKey.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ebSearchKey.Location = new System.Drawing.Point(136, 5);
            this.ebSearchKey.Name = "ebSearchKey";
            this.ebSearchKey.Size = new System.Drawing.Size(144, 21);
            this.ebSearchKey.TabIndex = 2;
            this.ebSearchKey.Text = "�˻�� �Է��ϼ���";
            this.ebSearchKey.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
            this.ebSearchKey.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            this.ebSearchKey.TextChanged += new System.EventHandler(this.ebSearchKey_TextChanged);
            this.ebSearchKey.Click += new System.EventHandler(this.ebSearchKey_Click);
            this.ebSearchKey.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ebSearchKey_KeyDown);
            // 
            // btnSearch
            // 
            this.btnSearch.BackColor = System.Drawing.Color.White;
            this.btnSearch.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSearch.Location = new System.Drawing.Point(288, 3);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(104, 24);
            this.btnSearch.TabIndex = 3;
            this.btnSearch.Text = "�� ȸ";
            this.btnSearch.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // imageList
            // 
            this.imageList.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
            this.imageList.ImageSize = new System.Drawing.Size(16, 16);
            this.imageList.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // pnlBtn
            // 
            this.pnlBtn.BackColor = System.Drawing.Color.WhiteSmoke;
            this.pnlBtn.Controls.Add(this.btnClose);
            this.pnlBtn.Controls.Add(this.btnOk);
            this.pnlBtn.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlBtn.Location = new System.Drawing.Point(0, 382);
            this.pnlBtn.Name = "pnlBtn";
            this.pnlBtn.Size = new System.Drawing.Size(400, 40);
            this.pnlBtn.TabIndex = 10;
            // 
            // btnClose
            // 
            this.btnClose.BackColor = System.Drawing.SystemColors.Control;
            this.btnClose.ImageIndex = 1;
            this.btnClose.ImageList = this.imageList;
            this.btnClose.Location = new System.Drawing.Point(200, 8);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(64, 23);
            this.btnClose.TabIndex = 6;
            this.btnClose.Text = "�ݱ�";
            this.btnClose.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnOk
            // 
            this.btnOk.BackColor = System.Drawing.SystemColors.Control;
            this.btnOk.ImageIndex = 0;
            this.btnOk.ImageList = this.imageList;
            this.btnOk.Location = new System.Drawing.Point(120, 8);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(64, 23);
            this.btnOk.TabIndex = 5;
            this.btnOk.Text = "Ȯ��";
            this.btnOk.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // grdExAdvertiserList
            // 
            this.grdExAdvertiserList.AllowEdit = Janus.Windows.GridEX.InheritableBoolean.False;
            this.grdExAdvertiserList.AlternatingColors = true;
            this.grdExAdvertiserList.BorderStyle = Janus.Windows.GridEX.BorderStyle.None;
            this.grdExAdvertiserList.DataSource = this.dvClient;
            this.grdExAdvertiserList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdExAdvertiserList.EmptyRows = true;
            this.grdExAdvertiserList.FocusCellFormatStyle.BackColor = System.Drawing.SystemColors.Highlight;
            this.grdExAdvertiserList.FocusCellFormatStyle.ForeColor = System.Drawing.Color.White;
            this.grdExAdvertiserList.FocusStyle = Janus.Windows.GridEX.FocusStyle.None;
            this.grdExAdvertiserList.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.grdExAdvertiserList.GridLines = Janus.Windows.GridEX.GridLines.Vertical;
            this.grdExAdvertiserList.GridLineStyle = Janus.Windows.GridEX.GridLineStyle.Solid;
            this.grdExAdvertiserList.GroupByBoxVisible = false;
            this.grdExAdvertiserList.HeaderFormatStyle.TextAlignment = Janus.Windows.GridEX.TextAlignment.Center;
            grdExAdvertiserList_Layout_0.DataSource = this.dvClient;
            grdExAdvertiserList_Layout_0.IsCurrentLayout = true;
            grdExAdvertiserList_Layout_0.Key = "bae";
            grdExAdvertiserList_Layout_0.LayoutString = resources.GetString("grdExAdvertiserList_Layout_0.LayoutString");
            this.grdExAdvertiserList.Layouts.AddRange(new Janus.Windows.GridEX.GridEXLayout[] {
            grdExAdvertiserList_Layout_0});
            this.grdExAdvertiserList.Location = new System.Drawing.Point(0, 32);
            this.grdExAdvertiserList.Name = "grdExAdvertiserList";
            this.grdExAdvertiserList.ScrollBars = Janus.Windows.GridEX.ScrollBars.Vertical;
            this.grdExAdvertiserList.SelectedInactiveFormatStyle.BackColor = System.Drawing.Color.Gold;
            this.grdExAdvertiserList.Size = new System.Drawing.Size(400, 350);
            this.grdExAdvertiserList.TabIndex = 4;
            this.grdExAdvertiserList.TabKeyBehavior = Janus.Windows.GridEX.TabKeyBehavior.ControlNavigation;
            this.grdExAdvertiserList.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            this.grdExAdvertiserList.RowDoubleClick += new Janus.Windows.GridEX.RowActionEventHandler(this.grdExAdvertiserList_RowDoubleClick);
            // 
            // Advertiser_pForm
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(6, 14);
            this.ClientSize = new System.Drawing.Size(400, 422);
            this.Controls.Add(this.grdExAdvertiserList);
            this.Controls.Add(this.pnlBtn);
            this.Controls.Add(this.panel4);
            this.Name = "Advertiser_pForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "�����ְ˻�";
            this.Load += new System.EventHandler(this.Advertiser_pForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dvClient)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.clientDs)).EndInit();
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.pnlBtn.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdExAdvertiserList)).EndInit();
            this.ResumeLayout(false);

		}
		#endregion

		#region ��Ʈ�� �ε�
		private void Advertiser_pForm_Load(object sender, System.EventArgs e)
		{
            
			// �����Ͱ����� ��ü����
			dtt = ((DataView)grdExAdvertiserList.DataSource).Table;  
			ccm = (CurrencyManager) this.BindingContext[grdExAdvertiserList.DataSource]; 
			//cm.PositionChanged += new System.EventHandler(OnGrdRowChanged); 

			// ��Ʈ�� �ʱ�ȭ
			InitControl();
			SearchAdvertiser();
		}
		#endregion

		#region ����� �׼�ó�� �޼ҵ�

		/// <summary>
		/// �˻��� ����
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		/// 
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
		
		/// <summary>
		/// �˻��� Ŭ�� 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void ebSearchKey_TextChanged(object sender, System.EventArgs e)
		{
			IsNewSearchKey = false;
		}

		private void ebSearchKey_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
		{
			if(e.KeyCode == Keys.Enter)
			{
				SearchAdvertiser();
			}
		}

		#endregion

		#region ��Ʈ�� �ʱ�ȭ
		private void InitControl()
		{
			Init_RapCode();
			InitCombo_Level(); 					
		}

		private void InitCombo_Level() 
		{			

			//�̵� �����ϰ���� ���ȵ��ó��
			if (commonModel.UserLevel == "30")
			{
				//��ȸ �޺� �Ƚ�
				cbSearchRap.SelectedValue = commonModel.RapCode;
				cbSearchRap.ReadOnly = true;                
			}
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
				Utility.SetDataTable(clientDs.MediaRaps, mediaRapCodeModel.MediaRapCodeDataSet);				
			}

			// �˻������� �޺�
			this.cbSearchRap.Items.Clear();
			// �޺��ڽ��� ��Ʈ�� �ڵ����� ���� Item�迭�� ����
			Janus.Windows.EditControls.UIComboBoxItem[] comboItems = new Janus.Windows.EditControls.UIComboBoxItem[mediaRapCodeModel.ResultCnt + 1];

			comboItems[0] = new Janus.Windows.EditControls.UIComboBoxItem("�̵�����","00");
			
			for(int i=0;i<mediaRapCodeModel.ResultCnt;i++)
			{
				DataRow row = clientDs.MediaRaps.Rows[i];

				string val = row["RapCode"].ToString();
				string txt = row["RapName"].ToString();
				comboItems[i+1] = new Janus.Windows.EditControls.UIComboBoxItem(txt,val);
			}
			// �޺��� ��Ʈ
			this.cbSearchRap.Items.AddRange(comboItems);
			this.cbSearchRap.SelectedIndex = 0;

			Application.DoEvents();
		}
				
		#endregion
  
		#region ó���޼ҵ�

		/// <summary>
		/// ��������� ��ȸ
		/// </summary>
		private void SearchAdvertiser()
		{
			StatusMessage("������ ������ ��ȸ�մϴ�.");

			try
			{
				// �����͸𵨿� ������ ������ ��Ʈ�Ѵ�.
				if(IsNewSearchKey)
				{
					clientModel.SearchKey = "";
				}
				else
				{
					clientModel.SearchKey  = ebSearchKey.Text;
				}
				clientModel.SearchRap        = cbSearchRap.SelectedValue.ToString();
				// �����������ȸ ���񽺸� ȣ���Ѵ�.
				new ClientManager(systemModel, commonModel).GetClientAdvertiserList(clientModel);

				if (clientModel.ResultCD.Equals("0000"))
				{
					Utility.SetDataTable(clientDs.Advertisers_C, clientModel.ClientDataSet);				
					StatusMessage(clientModel.ResultCnt + "���� ������ ������ ��ȸ�Ǿ����ϴ�.");
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

		private void btnOk_Click(object sender, System.EventArgs e)
		{
			string newKey = grdExAdvertiserList.SelectedItems[0].GetRow().Cells["AdvertiserCode"].Value.ToString();			
			string newKey_N = grdExAdvertiserList.SelectedItems[0].GetRow().Cells["AdvertiserName"].Value.ToString();			

			if(Search_AdvertiserFlag.Equals("Advertiser"))
			{
				this.AdvertiserCtl.AdvertiserCode = newKey;	
				this.AdvertiserCtl.AdvertiserName = newKey_N;	
			}
			else
			{
				this.AdvertiserCtl.SearchAdvertiserCode = newKey;	
				this.AdvertiserCtl.SearchAdvertiserName = newKey_N;	
			}	
			this.Close();
		}
     


		private void btnClose_Click(object sender, System.EventArgs e)
		{
			this.Close();
		}

		private void btnSearch_Click(object sender, System.EventArgs e)
		{
			SearchAdvertiser();
		}

		private void grdExAdvertiserList_RowDoubleClick(object sender, Janus.Windows.GridEX.RowActionEventArgs e)
		{						
			string newKey = grdExAdvertiserList.SelectedItems[0].GetRow().Cells["AdvertiserCode"].Value.ToString();			
			string newKey_N = grdExAdvertiserList.SelectedItems[0].GetRow().Cells["AdvertiserName"].Value.ToString();						
			if(Search_AdvertiserFlag.Equals("Advertiser"))
			{
				this.AdvertiserCtl.AdvertiserCode = newKey;	
				this.AdvertiserCtl.AdvertiserName = newKey_N;	
			}
			else
			{
				this.AdvertiserCtl.SearchAdvertiserCode = newKey;	
				this.AdvertiserCtl.SearchAdvertiserName = newKey_N;	
			}	
			this.Close();
		}		
		
	}
   

	#endregion


}
