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
	/// ChannelNoPopForm�� ���� ��� �����Դϴ�.
	/// </summary>
	/// 



	public class ChannelNoPopForm : System.Windows.Forms.Form
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
		ChannelSetModel channelSetModel  = new ChannelSetModel();	// ������������
				
		ChannelSetControl ChannelSetCtl = null;

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
		private Janus.Windows.EditControls.UIButton btnSearch;		
		private Janus.Windows.EditControls.UIComboBox cbSearchMediaName;
		private Janus.Windows.EditControls.UIComboBox cbSearchCategoryName;
		private Janus.Windows.EditControls.UIComboBox cbSearchGenreName;
		private Janus.Windows.GridEX.GridEX grdExChannelNoList;
		private AdManagerClient._10_Media._08_ChannelSet.ChannelSetDs channelSetDs1;
//		private System.ComponentModel.IContainer components;

		/// <summary>
		/// ������ �Ѱܾ� �� ��
		/// </summary>
		/// <param name="sender"></param>
		public ChannelNoPopForm(ChannelSetControl sender)
		{
			//
			// Windows Form �����̳� ������ �ʿ��մϴ�.
			//
			InitializeComponent();

			//
			
			//
            
			ChannelSetCtl = sender;
		}

		/// <summary>
		/// �Ϲݻ����
		/// </summary>
		public ChannelNoPopForm()
		{
			//
			// Windows Form �����̳� ������ �ʿ��մϴ�.
			//
			InitializeComponent();

			//
			
			//
            
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ChannelNoPopForm));
            this.dvChannelSet = new System.Data.DataView();
            this.channelSetDs1 = new AdManagerClient._10_Media._08_ChannelSet.ChannelSetDs();
            this.panel4 = new System.Windows.Forms.Panel();
            this.btnSearch = new Janus.Windows.EditControls.UIButton();
            this.cbSearchMediaName = new Janus.Windows.EditControls.UIComboBox();
            this.cbSearchCategoryName = new Janus.Windows.EditControls.UIComboBox();
            this.cbSearchGenreName = new Janus.Windows.EditControls.UIComboBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.grdExChannelNoList = new Janus.Windows.GridEX.GridEX();
            this.pnlBtn = new System.Windows.Forms.Panel();
            this.btnClose = new Janus.Windows.EditControls.UIButton();
            this.btnOk = new Janus.Windows.EditControls.UIButton();
            ((System.ComponentModel.ISupportInitialize)(this.dvChannelSet)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.channelSetDs1)).BeginInit();
            this.panel4.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdExChannelNoList)).BeginInit();
            this.pnlBtn.SuspendLayout();
            this.SuspendLayout();
            // 
            // dvChannelSet
            // 
            this.dvChannelSet.Table = this.channelSetDs1.ChannelSetPop;
            // 
            // channelSetDs1
            // 
            this.channelSetDs1.DataSetName = "ChannelSetDs";
            this.channelSetDs1.Locale = new System.Globalization.CultureInfo("en-US");
            this.channelSetDs1.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // panel4
            // 
            this.panel4.BackColor = System.Drawing.Color.White;
            this.panel4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel4.Controls.Add(this.btnSearch);
            this.panel4.Controls.Add(this.cbSearchMediaName);
            this.panel4.Controls.Add(this.cbSearchCategoryName);
            this.panel4.Controls.Add(this.cbSearchGenreName);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel4.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.panel4.Location = new System.Drawing.Point(0, 0);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(552, 32);
            this.panel4.TabIndex = 1;
            // 
            // btnSearch
            // 
            this.btnSearch.BackColor = System.Drawing.Color.White;
            this.btnSearch.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSearch.Location = new System.Drawing.Point(440, 3);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(104, 24);
            this.btnSearch.TabIndex = 4;
            this.btnSearch.Text = "�� ȸ";
            this.btnSearch.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // cbSearchMediaName
            // 
            this.cbSearchMediaName.ComboStyle = Janus.Windows.EditControls.ComboStyle.DropDownList;
            this.cbSearchMediaName.Font = new System.Drawing.Font("����ü", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.cbSearchMediaName.Location = new System.Drawing.Point(8, 5);
            this.cbSearchMediaName.Name = "cbSearchMediaName";
            this.cbSearchMediaName.Size = new System.Drawing.Size(120, 21);
            this.cbSearchMediaName.TabIndex = 1;
            this.cbSearchMediaName.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            // 
            // cbSearchCategoryName
            // 
            this.cbSearchCategoryName.ComboStyle = Janus.Windows.EditControls.ComboStyle.DropDownList;
            this.cbSearchCategoryName.Font = new System.Drawing.Font("����ü", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.cbSearchCategoryName.Location = new System.Drawing.Point(144, 5);
            this.cbSearchCategoryName.Name = "cbSearchCategoryName";
            this.cbSearchCategoryName.Size = new System.Drawing.Size(120, 21);
            this.cbSearchCategoryName.TabIndex = 2;
            this.cbSearchCategoryName.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            // 
            // cbSearchGenreName
            // 
            this.cbSearchGenreName.ComboStyle = Janus.Windows.EditControls.ComboStyle.DropDownList;
            this.cbSearchGenreName.Font = new System.Drawing.Font("����ü", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.cbSearchGenreName.Location = new System.Drawing.Point(280, 5);
            this.cbSearchGenreName.Name = "cbSearchGenreName";
            this.cbSearchGenreName.Size = new System.Drawing.Size(120, 21);
            this.cbSearchGenreName.TabIndex = 3;
            this.cbSearchGenreName.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.grdExChannelNoList);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 32);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(552, 478);
            this.panel1.TabIndex = 17;
            // 
            // grdExChannelNoList
            // 
            this.grdExChannelNoList.AllowEdit = Janus.Windows.GridEX.InheritableBoolean.False;
            this.grdExChannelNoList.AlternatingColors = true;
            this.grdExChannelNoList.AutomaticSort = false;
            this.grdExChannelNoList.BorderStyle = Janus.Windows.GridEX.BorderStyle.None;
            this.grdExChannelNoList.DataSource = this.dvChannelSet;
            this.grdExChannelNoList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdExChannelNoList.EmptyRows = true;
            this.grdExChannelNoList.FocusStyle = Janus.Windows.GridEX.FocusStyle.None;
            this.grdExChannelNoList.Font = new System.Drawing.Font("����ü", 9F);
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
            this.grdExChannelNoList.Size = new System.Drawing.Size(552, 478);
            this.grdExChannelNoList.TabIndex = 5;
            this.grdExChannelNoList.TabKeyBehavior = Janus.Windows.GridEX.TabKeyBehavior.ControlNavigation;
            this.grdExChannelNoList.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            this.grdExChannelNoList.RowDoubleClick += new Janus.Windows.GridEX.RowActionEventHandler(this.grdExChannelNoList_RowDoubleClick);
            // 
            // pnlBtn
            // 
            this.pnlBtn.BackColor = System.Drawing.Color.WhiteSmoke;
            this.pnlBtn.Controls.Add(this.btnClose);
            this.pnlBtn.Controls.Add(this.btnOk);
            this.pnlBtn.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlBtn.Location = new System.Drawing.Point(0, 470);
            this.pnlBtn.Name = "pnlBtn";
            this.pnlBtn.Size = new System.Drawing.Size(552, 40);
            this.pnlBtn.TabIndex = 16;
            // 
            // btnClose
            // 
            this.btnClose.BackColor = System.Drawing.SystemColors.Control;
            this.btnClose.Location = new System.Drawing.Point(281, 9);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(70, 23);
            this.btnClose.TabIndex = 7;
            this.btnClose.Text = "�ݱ�";
            this.btnClose.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnOk
            // 
            this.btnOk.BackColor = System.Drawing.SystemColors.Control;
            this.btnOk.Location = new System.Drawing.Point(201, 9);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(70, 23);
            this.btnOk.TabIndex = 6;
            this.btnOk.Text = "Ȯ��";
            this.btnOk.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // ChannelNoPopForm
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(6, 14);
            this.ClientSize = new System.Drawing.Size(552, 510);
            this.Controls.Add(this.pnlBtn);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panel4);
            this.Name = "ChannelNoPopForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "ä�θ�ϰ˻�";
            this.Load += new System.EventHandler(this.ChannelNoPopForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dvChannelSet)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.channelSetDs1)).EndInit();
            this.panel4.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdExChannelNoList)).EndInit();
            this.pnlBtn.ResumeLayout(false);
            this.ResumeLayout(false);

		}
		#endregion

		#region ��Ʈ�� �ε�
		private void ChannelNoPopForm_Load(object sender, System.EventArgs e)
		{

            
			// �����Ͱ����� ��ü����
			dt = ((DataView)grdExChannelNoList.DataSource).Table;  
			cm = (CurrencyManager) this.BindingContext[grdExChannelNoList.DataSource]; 
			//cm.PositionChanged += new System.EventHandler(OnGrdRowChanged); 

			// ��Ʈ�� �ʱ�ȭ
			InitControl();
			//SearchChannelSet();
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
			InitCombo();
			InitCombo_Category();
			InitCombo_Genre();												
		}

		private void InitCombo()
		{			
			MediaCodeModel mediacodeModel = new MediaCodeModel();		
			new MediaCodeManager(systemModel, commonModel).GetMediaCodeList(mediacodeModel);
			
			if (mediacodeModel.ResultCD.Equals("0000"))
			{
				// �����ͼ¿� ����
				Utility.SetDataTable(channelSetDs1.Medias, mediacodeModel.MediaCodeDataSet);				
			}

			// ����ȸ �޺�
			// �������� �޺��� Dataset�� �����ͼҽ��� ������.

			// �˻������� �޺�
			this.cbSearchMediaName.Items.Clear();
			
			// �޺��ڽ��� ��Ʈ�� �ڵ����� ���� Item�迭�� ����
			Janus.Windows.EditControls.UIComboBoxItem[] comboItems = new Janus.Windows.EditControls.UIComboBoxItem[mediacodeModel.ResultCnt + 1];

			comboItems[0] = new Janus.Windows.EditControls.UIComboBoxItem("��ü����","00");
			
			for(int i=0;i<mediacodeModel.ResultCnt;i++)
			{
				DataRow row = channelSetDs1.Medias.Rows[i];

				string val = row["MediaCode"].ToString();
				string txt = row["MediaName"].ToString();
				comboItems[i+1] = new Janus.Windows.EditControls.UIComboBoxItem(txt,val);
			}
			// �޺��� ��Ʈ
			this.cbSearchMediaName.Items.AddRange(comboItems);
			this.cbSearchMediaName.SelectedIndex = 0;

			Application.DoEvents();

		}
		
		public void InitCombo_Category()
		{
			// �ڵ忡�� ���ȷ����� ��ȸ�Ѵ�.
			ChannelSetModel channelSetModel = new ChannelSetModel();
			new ChannelSetManager(systemModel, commonModel).GetCategoryList(channelSetModel);
			
			if (channelSetModel.ResultCD.Equals("0000"))
			{
				// �����ͼ¿� ����
				Utility.SetDataTable(channelSetDs1.Categorys, channelSetModel.CategoryDataSet);	
			}

			// �ϴ� ��ü �޺��� �� �ʱ�ȭ. 
			this.cbSearchCategoryName.Items.Clear();
            
			// �޺��ڽ��� ��Ʈ�� �ڵ����� ���� Item�迭�� ����
			Janus.Windows.EditControls.UIComboBoxItem[] comboItems = new Janus.Windows.EditControls.UIComboBoxItem[channelSetModel.ResultCnt + 1];
            
			comboItems[0] = new Janus.Windows.EditControls.UIComboBoxItem("ī�װ�����","00");
			
			for(int i=0;i<channelSetModel.ResultCnt;i++)
			{
				DataRow row = channelSetDs1.Categorys.Rows[i];

				string val = row["CategoryCode"].ToString();
				string txt = row["CategoryName"].ToString();
				comboItems[i+1] = new Janus.Windows.EditControls.UIComboBoxItem(txt,val);
			}


			// �ϴ� �޺��� ��Ʈ
			this.cbSearchCategoryName.Items.AddRange(comboItems);
			this.cbSearchCategoryName.SelectedIndex = 0;
			
			Application.DoEvents();

		}

		public void InitCombo_Genre()
		{
			// �ڵ忡�� ���ȷ����� ��ȸ�Ѵ�.
			ChannelSetModel channelSetModel = new ChannelSetModel();
			new ChannelSetManager(systemModel, commonModel).GetGenreList(channelSetModel);
			
			if (channelSetModel.ResultCD.Equals("0000"))
			{
				// �����ͼ¿� ����
				Utility.SetDataTable(channelSetDs1.Genres, channelSetModel.GenreDataSet);	
			}

			// �ϴ� ��ü �޺��� �� �ʱ�ȭ. 
			this.cbSearchGenreName.Items.Clear();
            
			// �޺��ڽ��� ��Ʈ�� �ڵ����� ���� Item�迭�� ����
			Janus.Windows.EditControls.UIComboBoxItem[] comboItems = new Janus.Windows.EditControls.UIComboBoxItem[channelSetModel.ResultCnt + 1];
            
			comboItems[0] = new Janus.Windows.EditControls.UIComboBoxItem("�帣����","00");
			
			for(int i=0;i<channelSetModel.ResultCnt;i++)
			{
				DataRow row = channelSetDs1.Genres.Rows[i];

				string val = row["GenreCode"].ToString();
				string txt = row["GenreName"].ToString();
				comboItems[i+1] = new Janus.Windows.EditControls.UIComboBoxItem(txt,val);
			}


			// �ϴ� �޺��� ��Ʈ
			this.cbSearchGenreName.Items.AddRange(comboItems);
			this.cbSearchGenreName.SelectedIndex = 0;

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
				// �����͸𵨿� ������ ������ ��Ʈ�Ѵ�.
				if(cbSearchMediaName.SelectedValue.ToString() == "00")
				{					
					MessageBox.Show("��ü���� �����Ͽ� �ּ���.","ä�������˻�", 
						MessageBoxButtons.OK, MessageBoxIcon.Information );
					return;						
				}
				if(cbSearchCategoryName.SelectedValue.ToString() == "00")
				{					
					MessageBox.Show("ī�װ����� �����Ͽ� �ּ���.","ä�������˻�", 
						MessageBoxButtons.OK, MessageBoxIcon.Information );
					return;					
				}				
				else
				{
					channelSetModel.SearchKey  = cbSearchMediaName.Text;
				}
				channelSetModel.MediaCode_P = cbSearchMediaName.SelectedItem.Value.ToString();
				channelSetModel.CategoryCode_P = cbSearchCategoryName.SelectedItem.Value.ToString();
				channelSetModel.GenreCode_P = cbSearchGenreName.SelectedItem.Value.ToString();

				// �����������ȸ ���񽺸� ȣ���Ѵ�.
				new ChannelSetManager(systemModel,commonModel).GetChannelNoPopList(channelSetModel);

				if (channelSetModel.ResultCD.Equals("0000"))
				{
					Utility.SetDataTable(channelSetDs1.ChannelSetPop, channelSetModel.ChannelSetDataSet);				
					StatusMessage(channelSetModel.ResultCnt + "���� ������ ������ ��ȸ�Ǿ����ϴ�.");
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
			string newKey = grdExChannelNoList.SelectedItems[0].GetRow().Cells["ChannelNo"].Value.ToString();			
			this.ChannelSetCtl.ChannelNo = newKey;				
			this.Close();
		}
     


		private void btnClose_Click(object sender, System.EventArgs e)
		{
			this.Close();
		}

		private void btnSearch_Click(object sender, System.EventArgs e)
		{
			SearchChannelSet();
		}

		private void grdExChannelNoList_RowDoubleClick(object sender, Janus.Windows.GridEX.RowActionEventArgs e)
		{						
			string newKey = grdExChannelNoList.SelectedItems[0].GetRow().Cells["ChannelNo"].Value.ToString();			
			this.ChannelSetCtl.ChannelNo = newKey;				
			this.Close();
		}		
		
	}
   

	#endregion


}
