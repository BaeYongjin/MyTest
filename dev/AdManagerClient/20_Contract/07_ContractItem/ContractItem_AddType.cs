using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace AdManagerClient._20_Contract
{
	/// <summary>
	/// ContractItem_AddType에 대한 요약 설명입니다.
	/// </summary>
	public class ContractItem_AddType : System.Windows.Forms.Form
	{
		private Janus.Windows.EditControls.UIGroupBox uiGroupBox1;
        private Janus.Windows.EditControls.UIButton btnOk;
		private Janus.Windows.EditControls.UIButton btnCancel;
        private Janus.Windows.EditControls.UIRadioButton rdZCM;
		private Janus.Windows.EditControls.UIRadioButton rdBCM;
		private Janus.Windows.EditControls.UIRadioButton rdMCM;
        private Janus.Windows.EditControls.UIGroupBox uiGroupBox2;
        private Janus.Windows.EditControls.UIRadioButton rdTCM;
		private System.Windows.Forms.Label label2;
        private Janus.Windows.EditControls.UIRadioButton rdCM;
		/// <summary>
		/// 필수 디자이너 변수입니다.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public ContractItem_AddType()
		{
			InitializeComponent();
		}

		/// <summary>
		/// 사용 중인 모든 리소스를 정리합니다.
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

		#region Windows Form 디자이너에서 생성한 코드
		/// <summary>
		/// 디자이너 지원에 필요한 메서드입니다.
		/// 이 메서드의 내용을 코드 편집기로 수정하지 마십시오.
		/// </summary>
		private void InitializeComponent()
		{
			this.uiGroupBox1 = new Janus.Windows.EditControls.UIGroupBox();
			this.uiGroupBox2 = new Janus.Windows.EditControls.UIGroupBox();
			this.label2 = new System.Windows.Forms.Label();
			this.rdTCM = new Janus.Windows.EditControls.UIRadioButton();
			this.rdCM = new Janus.Windows.EditControls.UIRadioButton();
			this.rdMCM = new Janus.Windows.EditControls.UIRadioButton();
			this.rdBCM = new Janus.Windows.EditControls.UIRadioButton();
			this.rdZCM = new Janus.Windows.EditControls.UIRadioButton();
			this.btnCancel = new Janus.Windows.EditControls.UIButton();
			this.btnOk = new Janus.Windows.EditControls.UIButton();
			((System.ComponentModel.ISupportInitialize)(this.uiGroupBox1)).BeginInit();
			this.uiGroupBox1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.uiGroupBox2)).BeginInit();
			this.uiGroupBox2.SuspendLayout();
			this.SuspendLayout();
			// 
			// uiGroupBox1
			// 
			this.uiGroupBox1.BackgroundStyle = Janus.Windows.EditControls.BackgroundStyle.ExplorerBarGroupBackground;
			this.uiGroupBox1.Controls.Add(this.uiGroupBox2);
			this.uiGroupBox1.Controls.Add(this.btnCancel);
			this.uiGroupBox1.Controls.Add(this.btnOk);
			this.uiGroupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.uiGroupBox1.FrameStyle = Janus.Windows.EditControls.FrameStyle.None;
			this.uiGroupBox1.Location = new System.Drawing.Point(0, 0);
			this.uiGroupBox1.Name = "uiGroupBox1";
			this.uiGroupBox1.Size = new System.Drawing.Size(402, 204);
			this.uiGroupBox1.TabIndex = 0;
			this.uiGroupBox1.Text = "입력할 광고종류를 선택하십시요";
			this.uiGroupBox1.TextOffset = 11;
			this.uiGroupBox1.VisualStyle = Janus.Windows.UI.Dock.PanelVisualStyle.Office2007;
			// 
			// uiGroupBox2
			// 
			this.uiGroupBox2.BackColor = System.Drawing.Color.Transparent;
			this.uiGroupBox2.Controls.Add(this.label2);
			this.uiGroupBox2.Controls.Add(this.rdTCM);
			this.uiGroupBox2.Controls.Add(this.rdCM);
			this.uiGroupBox2.Controls.Add(this.rdMCM);
			this.uiGroupBox2.Controls.Add(this.rdBCM);
			this.uiGroupBox2.Controls.Add(this.rdZCM);
			this.uiGroupBox2.Location = new System.Drawing.Point(14, 24);
			this.uiGroupBox2.Name = "uiGroupBox2";
			this.uiGroupBox2.Size = new System.Drawing.Size(372, 134);
			this.uiGroupBox2.TabIndex = 13;
			this.uiGroupBox2.Text = "광고종류";
			this.uiGroupBox2.VisualStyle = Janus.Windows.UI.Dock.PanelVisualStyle.Office2003;
			// 
			// label2
			// 
			this.label2.BackColor = System.Drawing.Color.DimGray;
			this.label2.Location = new System.Drawing.Point(184, 24);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(2, 100);
			this.label2.TabIndex = 11;
			// 
			// rdTCM
			// 
			this.rdTCM.BackColor = System.Drawing.Color.Transparent;
			this.rdTCM.Cursor = System.Windows.Forms.Cursors.Hand;
			this.rdTCM.Location = new System.Drawing.Point(20, 92);
			this.rdTCM.Name = "rdTCM";
			this.rdTCM.Size = new System.Drawing.Size(165, 20);
			this.rdTCM.TabIndex = 10;
			this.rdTCM.Text = "TCM - PostRoll AD";
			this.rdTCM.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
			// 
			// rdCM
			// 
			this.rdCM.BackColor = System.Drawing.Color.Transparent;
			this.rdCM.Checked = true;
			this.rdCM.Cursor = System.Windows.Forms.Cursors.Hand;
			this.rdCM.Location = new System.Drawing.Point(20, 24);
			this.rdCM.Name = "rdCM";
			this.rdCM.Size = new System.Drawing.Size(119, 20);
			this.rdCM.TabIndex = 1;
			this.rdCM.TabStop = true;
			this.rdCM.Text = "PCM - PreRoll AD";
			this.rdCM.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
			// 
			// rdMCM
			// 
			this.rdMCM.BackColor = System.Drawing.Color.Transparent;
			this.rdMCM.Cursor = System.Windows.Forms.Cursors.Hand;
			this.rdMCM.ForeColor = System.Drawing.SystemColors.ControlText;
			this.rdMCM.Location = new System.Drawing.Point(20, 58);
			this.rdMCM.Name = "rdMCM";
			this.rdMCM.Size = new System.Drawing.Size(165, 20);
			this.rdMCM.TabIndex = 9;
			this.rdMCM.Text = "MCM - MidRoll AD";
			this.rdMCM.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
			// 
			// rdBCM
			// 
			this.rdBCM.BackColor = System.Drawing.Color.Transparent;
			this.rdBCM.Cursor = System.Windows.Forms.Cursors.Hand;
			this.rdBCM.Location = new System.Drawing.Point(208, 58);
			this.rdBCM.Name = "rdBCM";
			this.rdBCM.Size = new System.Drawing.Size(153, 20);
			this.rdBCM.TabIndex = 4;
			this.rdBCM.Text = "BCM - Banner AD";
			this.rdBCM.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
			// 
			// rdZCM
			// 
			this.rdZCM.BackColor = System.Drawing.Color.Transparent;
			this.rdZCM.Cursor = System.Windows.Forms.Cursors.Hand;
			this.rdZCM.Location = new System.Drawing.Point(208, 24);
			this.rdZCM.Name = "rdZCM";
			this.rdZCM.Size = new System.Drawing.Size(150, 20);
			this.rdZCM.TabIndex = 3;
			this.rdZCM.Text = "ZCM - Zapping AD";
			this.rdZCM.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
			// 
			// btnCancel
			// 
			this.btnCancel.Cursor = System.Windows.Forms.Cursors.Hand;
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.No;
			this.btnCancel.Location = new System.Drawing.Point(199, 170);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(70, 22);
			this.btnCancel.TabIndex = 12;
			this.btnCancel.Text = "취소";
			this.btnCancel.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
			this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
			// 
			// btnOk
			// 
			this.btnOk.Cursor = System.Windows.Forms.Cursors.Hand;
			this.btnOk.DialogResult = System.Windows.Forms.DialogResult.Yes;
			this.btnOk.Location = new System.Drawing.Point(125, 170);
			this.btnOk.Name = "btnOk";
			this.btnOk.Size = new System.Drawing.Size(70, 22);
			this.btnOk.TabIndex = 11;
			this.btnOk.Text = "선택";
			this.btnOk.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
			this.btnOk.Click += new System.EventHandler(this.uiButton1_Click);
			// 
			// ContractItem_AddType
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.ClientSize = new System.Drawing.Size(402, 204);
			this.Controls.Add(this.uiGroupBox1);
			this.Font = global::AdManagerClient.Properties.Settings.Default.Font1;
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.Name = "ContractItem_AddType";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "광고종류 선택";
			this.TopMost = true;
			((System.ComponentModel.ISupportInitialize)(this.uiGroupBox1)).EndInit();
			this.uiGroupBox1.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.uiGroupBox2)).EndInit();
			this.uiGroupBox2.ResumeLayout(false);
			this.ResumeLayout(false);

        }
		#endregion

        #region [ 외부에서 접근할수 있게 ]
        private int mAdType = 10;

        private void uiButton1_Click(object sender, System.EventArgs e)
        {
			if (rdCM.Checked) mAdType = 10;
			else if (rdMCM.Checked) mAdType = 18;
			else if (rdTCM.Checked) mAdType = 19;
			else if (rdZCM.Checked) mAdType = 31;
			else if (rdBCM.Checked) mAdType = 51;
			else mAdType = 99;
        }
    
		/// <summary>
		/// 광고종류 코드값을 가져온다
		/// </summary>
        public int AdType
        {
            get
            {
                return mAdType;
            }
        }
        #endregion

		private void btnCancel_Click(object sender, EventArgs e)
		{

		}
    }
}