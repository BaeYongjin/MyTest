using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Text;
using System.IO;
using System.Diagnostics;


namespace AdManagerClient
{
	/// <summary>
	/// MessageForm�� ���� ��� �����Դϴ�.
	/// </summary>
	public class MessageForm : System.Windows.Forms.Form
	{	
		
		private int flag = 0;

		private string[] msgList = null;


		private Janus.Windows.EditControls.UIButton btnOk;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.PictureBox pictureBox1;
		private System.Windows.Forms.Panel panel2;
		private System.Windows.Forms.RichTextBox rcText;
		/// <summary>
		/// �ʼ� �����̳� �����Դϴ�.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public MessageForm()
		{
			//
			// Windows Form �����̳� ������ �ʿ��մϴ�.
			//
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MessageForm));
            this.btnOk = new Janus.Windows.EditControls.UIButton();
            this.panel1 = new System.Windows.Forms.Panel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.rcText = new System.Windows.Forms.RichTextBox();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnOk
            // 
            this.btnOk.ButtonStyle = Janus.Windows.EditControls.ButtonStyle.Button;
            this.btnOk.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.btnOk.FlatBorderColor = System.Drawing.SystemColors.Control;
            this.btnOk.Location = new System.Drawing.Point(0, 87);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(402, 24);
            this.btnOk.TabIndex = 0;
            this.btnOk.Text = "Ȯ   ��";
            this.btnOk.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.White;
            this.panel1.Controls.Add(this.pictureBox1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(48, 87);
            this.panel1.TabIndex = 2;
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(8, 16);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(32, 32);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.rcText);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(48, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(354, 87);
            this.panel2.TabIndex = 3;
            // 
            // rcText
            // 
            this.rcText.BackColor = System.Drawing.Color.White;
            this.rcText.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.rcText.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rcText.Font = new System.Drawing.Font("����", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.rcText.ForeColor = System.Drawing.Color.DarkSlateGray;
            this.rcText.Location = new System.Drawing.Point(0, 0);
            this.rcText.Name = "rcText";
            this.rcText.ReadOnly = true;
            this.rcText.Size = new System.Drawing.Size(354, 87);
            this.rcText.TabIndex = 2;
            this.rcText.Text = "";
            // 
            // MessageForm
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(6, 14);
            this.ClientSize = new System.Drawing.Size(402, 111);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.btnOk);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MessageForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.TopMost = true;
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);

		}
		#endregion

		private void btnOk_Click(object sender, System.EventArgs e)
		{
			this.Close();
		}


		/// <summary>
		/// Title ����
		/// </summary>
		public string SetTitle
		{
			set
			{
				this.Text  = value;
			}
		}

		
		/// <summary>
		/// �޽��� Ÿ�� ����= 1�̸� �ܼ��� �޽��� â�� ����
		/// </summary>
		public int SetMessageType
		{
			set
			{
				this.flag  = value;
				if (value == 1)		// �ܼ� �˸� �޽����� ��ư �����
				{
					btnOk.Visible = false;
				}
			}
		}

		
		/// <summary>
		/// �޽��� �ڽ� ��� �� ����
		/// </summary>
		public Color SetBacColor
		{
			set
			{
				this.rcText.BackColor = value;
			}
		}

		
		
		/// <summary>
		/// �޽��� ����
		/// </summary>
		public string[] SetMessage
		{
			set{ this.msgList = value; }
		}
		

		/// <summary>
		/// �޽��� �� �� ����
		/// </summary>
		public Color SetBackColor
		{
			set{ this.rcText.BackColor = value;}
		}

		
		/// <summary>
		/// ����� ���� �� ����
		/// </summary>
		public int SetWidth
		{
			set
			{
				this.Width	=	value;
			}
		}


		/// <summary>
		/// ����� ���� �� ����
		/// </summary>
		public int SetHeight
		{
			set
			{
				this.Height = value;
			}
		}


		
		/// <summary>
		/// �޽��� ��Ʈ ����
		/// </summary>
		public Font SetFont
		{
			set{ this.rcText.Font = value; }
		}



		/// <summary>
		/// �޽��� ���
		/// </summary>
		public void showMessage()
		{
			try
			{								
				rcText.Lines = msgList;				
			}
			catch(Exception ex)
			{
				Trace.WriteLine(ex.Message);
			}
		}

	}
}
