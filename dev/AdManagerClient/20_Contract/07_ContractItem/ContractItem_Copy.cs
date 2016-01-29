using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

using WinFramework.Misc;
using WinFramework.Base;
using WinFramework.Data;

using AdManagerModel;

namespace AdManagerClient._20_Contract._07_ContractItem
{
	/// <summary>
	/// ContractItem_Copy�� ���� ��� �����Դϴ�.
	/// </summary>
	public class ContractItem_Copy : System.Windows.Forms.Form
	{
		private Janus.Windows.EditControls.UIGroupBox uiGroupBox1;
		private System.Windows.Forms.Label lbItemName;
		private Janus.Windows.GridEX.EditControls.EditBox ebItemName;
		private Janus.Windows.EditControls.UIButton uiButton2;
		/// <summary>
		/// �ʼ� �����̳� �����Դϴ�.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private Janus.Windows.GridEX.EditControls.EditBox ebItemNo;
		private Janus.Windows.EditControls.UIButton btnCopy;

		private SystemModel   systemModel   = FrameSystem.oSysModel;
		private CommonModel   commonModel   = FrameSystem.oComModel;
		private Logger        log           = FrameSystem.oLog;


		// ����� ������
		private	ItemCopyModel	dataModel	= new ItemCopyModel();
		private System.Windows.Forms.Label label1;

		#region [�Ӽ�] �����ȣ
		private	int mItemNo	= 0;

		/// <summary>
		/// ���� ���� �����ȣ�� �����Ѵ�
		/// </summary>
		public	int	SetItemNo
		{
			set
			{
				mItemNo			=	value;
				ebItemNo.Text	=	mItemNo.ToString();
			}
		}
		#endregion

		#region[�Ӽ�] �����
		/// <summary>
		/// ������� �����մϴ�
		/// </summary>
		public	string	SetContractItem
		{
			set
			{
				ebItemName.Text	=	value;
			}
		}
		#endregion

		public ContractItem_Copy()
		{
			InitializeComponent();
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
            this.uiGroupBox1 = new Janus.Windows.EditControls.UIGroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.ebItemNo = new Janus.Windows.GridEX.EditControls.EditBox();
            this.uiButton2 = new Janus.Windows.EditControls.UIButton();
            this.btnCopy = new Janus.Windows.EditControls.UIButton();
            this.lbItemName = new System.Windows.Forms.Label();
            this.ebItemName = new Janus.Windows.GridEX.EditControls.EditBox();
            ((System.ComponentModel.ISupportInitialize)(this.uiGroupBox1)).BeginInit();
            this.uiGroupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // uiGroupBox1
            // 
            this.uiGroupBox1.BackgroundStyle = Janus.Windows.EditControls.BackgroundStyle.TabPage;
            this.uiGroupBox1.Controls.Add(this.label1);
            this.uiGroupBox1.Controls.Add(this.ebItemNo);
            this.uiGroupBox1.Controls.Add(this.uiButton2);
            this.uiGroupBox1.Controls.Add(this.btnCopy);
            this.uiGroupBox1.Controls.Add(this.lbItemName);
            this.uiGroupBox1.Controls.Add(this.ebItemName);
            this.uiGroupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.uiGroupBox1.Location = new System.Drawing.Point(3, 0);
            this.uiGroupBox1.Name = "uiGroupBox1";
            this.uiGroupBox1.Size = new System.Drawing.Size(532, 76);
            this.uiGroupBox1.TabIndex = 0;
            this.uiGroupBox1.VisualStyle = Janus.Windows.UI.Dock.PanelVisualStyle.Office2003;
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.ForeColor = System.Drawing.Color.SlateBlue;
            this.label1.Location = new System.Drawing.Point(6, 50);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(354, 22);
            this.label1.TabIndex = 106;
            this.label1.Text = "*�űԱ������ �Է��Ͻ��� ��������ư�� Ŭ���Ͻø� �˴ϴ�";
            // 
            // ebItemNo
            // 
            this.ebItemNo.BackColor = System.Drawing.Color.MediumSpringGreen;
            this.ebItemNo.Location = new System.Drawing.Point(82, 18);
            this.ebItemNo.MaxLength = 50;
            this.ebItemNo.Name = "ebItemNo";
            this.ebItemNo.ReadOnly = true;
            this.ebItemNo.Size = new System.Drawing.Size(60, 23);
            this.ebItemNo.TabIndex = 105;
            this.ebItemNo.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
            this.ebItemNo.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            // 
            // uiButton2
            // 
            this.uiButton2.Cursor = System.Windows.Forms.Cursors.Hand;
            this.uiButton2.DialogResult = System.Windows.Forms.DialogResult.No;
            this.uiButton2.Location = new System.Drawing.Point(450, 50);
            this.uiButton2.Name = "uiButton2";
            this.uiButton2.Size = new System.Drawing.Size(70, 22);
            this.uiButton2.TabIndex = 104;
            this.uiButton2.Text = "���";
            this.uiButton2.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            // 
            // btnCopy
            // 
            this.btnCopy.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnCopy.Location = new System.Drawing.Point(376, 50);
            this.btnCopy.Name = "btnCopy";
            this.btnCopy.Size = new System.Drawing.Size(70, 22);
            this.btnCopy.TabIndex = 103;
            this.btnCopy.Text = "�������";
            this.btnCopy.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.btnCopy.Click += new System.EventHandler(this.uiButton1_Click);
            // 
            // lbItemName
            // 
            this.lbItemName.BackColor = System.Drawing.Color.Transparent;
            this.lbItemName.Location = new System.Drawing.Point(8, 19);
            this.lbItemName.Name = "lbItemName";
            this.lbItemName.Size = new System.Drawing.Size(68, 21);
            this.lbItemName.TabIndex = 100;
            this.lbItemName.Text = "�����";
            this.lbItemName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ebItemName
            // 
            this.ebItemName.Location = new System.Drawing.Point(146, 18);
            this.ebItemName.MaxLength = 50;
            this.ebItemName.Name = "ebItemName";
            this.ebItemName.Size = new System.Drawing.Size(376, 23);
            this.ebItemName.TabIndex = 99;
            this.ebItemName.Text = "�űԷ� ����� �����";
            this.ebItemName.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
            this.ebItemName.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            // 
            // ContractItem_Copy
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(6, 16);
            this.ClientSize = new System.Drawing.Size(538, 80);
            this.Controls.Add(this.uiGroupBox1);
            this.Font = new System.Drawing.Font("���� ���", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "ContractItem_Copy";
            this.Padding = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "������ ����";
            ((System.ComponentModel.ISupportInitialize)(this.uiGroupBox1)).EndInit();
            this.uiGroupBox1.ResumeLayout(false);
            this.uiGroupBox1.PerformLayout();
            this.ResumeLayout(false);

		}
		#endregion

		private void uiButton1_Click(object sender, System.EventArgs e)
		{
			ContractItemCopy();
		}

		private void ContractItemCopy()
		{
			if( mItemNo == 0 )
			{
				MessageBox.Show("������� �������� ���õ��� �ʾҽ��ϴ�.","������ ����", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				return;
			}

			if(ebItemName.Text.Trim().Length == 0) 
			{
				MessageBox.Show("������� �Էµ��� �ʾҽ��ϴ�.","������ ����", MessageBoxButtons.OK, MessageBoxIcon.Information );
				ebItemName.Focus();
				return;	               
			}

			DialogResult result = MessageBox.Show("�����ѱ������� �����մϴ�.","����������", 
				MessageBoxButtons.YesNo, MessageBoxIcon.Question,
				MessageBoxDefaultButton.Button2);

			if (result == DialogResult.No) return;

                       
			try
			{
				dataModel.Init();
				dataModel.ItemNoSou	=	mItemNo;
				dataModel.ItemName	=	ebItemName.Text.ToString();
				new ContractItemManager( systemModel, commonModel).SetContractItemCopy( dataModel );

				if( dataModel.ResultCD == "0000" )
				{
					if( dataModel.ItemNoDes > mItemNo )
					{
						MessageBox.Show("�����۾��� �Ϸ�Ǿ����ϴ�.","������ ����", MessageBoxButtons.OK, MessageBoxIcon.Information);
						this.Close();
					}
				}
			}
			catch(FrameException fe)
			{
				FrameSystem.showMsgForm("������ �������", new string[] {fe.ErrCode, fe.ResultMsg});
			}
			catch(Exception ex)
			{
				FrameSystem.showMsgForm("������ �������",new string[] {"",ex.Message});
			}			
		}
	}
}
