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
	/// ContractItem_Copy에 대한 요약 설명입니다.
	/// </summary>
	public class ContractItem_Copy : System.Windows.Forms.Form
	{
		private Janus.Windows.EditControls.UIGroupBox uiGroupBox1;
		private System.Windows.Forms.Label lbItemName;
		private Janus.Windows.GridEX.EditControls.EditBox ebItemName;
		private Janus.Windows.EditControls.UIButton uiButton2;
		/// <summary>
		/// 필수 디자이너 변수입니다.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private Janus.Windows.GridEX.EditControls.EditBox ebItemNo;
		private Janus.Windows.EditControls.UIButton btnCopy;

		private SystemModel   systemModel   = FrameSystem.oSysModel;
		private CommonModel   commonModel   = FrameSystem.oComModel;
		private Logger        log           = FrameSystem.oLog;


		// 사용할 정보모델
		private	ItemCopyModel	dataModel	= new ItemCopyModel();
		private System.Windows.Forms.Label label1;

		#region [속성] 광고번호
		private	int mItemNo	= 0;

		/// <summary>
		/// 복사 원본 광고번호를 설정한다
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

		#region[속성] 광고명
		/// <summary>
		/// 광고명을 설정합니다
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
            this.label1.Text = "*신규광고명을 입력하신후 복사실행버튼을 클릭하시면 됩니다";
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
            this.uiButton2.Text = "취소";
            this.uiButton2.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            // 
            // btnCopy
            // 
            this.btnCopy.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnCopy.Location = new System.Drawing.Point(376, 50);
            this.btnCopy.Name = "btnCopy";
            this.btnCopy.Size = new System.Drawing.Size(70, 22);
            this.btnCopy.TabIndex = 103;
            this.btnCopy.Text = "복사실행";
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
            this.lbItemName.Text = "광고명";
            this.lbItemName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ebItemName
            // 
            this.ebItemName.Location = new System.Drawing.Point(146, 18);
            this.ebItemName.MaxLength = 50;
            this.ebItemName.Name = "ebItemName";
            this.ebItemName.Size = new System.Drawing.Size(376, 23);
            this.ebItemName.TabIndex = 99;
            this.ebItemName.Text = "신규로 복사될 광고명";
            this.ebItemName.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
            this.ebItemName.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            // 
            // ContractItem_Copy
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(6, 16);
            this.ClientSize = new System.Drawing.Size(538, 80);
            this.Controls.Add(this.uiGroupBox1);
            this.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "ContractItem_Copy";
            this.Padding = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "광고내역 복사";
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
				MessageBox.Show("복사원본 광고내역이 선택되지 않았습니다.","광고내역 복사", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				return;
			}

			if(ebItemName.Text.Trim().Length == 0) 
			{
				MessageBox.Show("광고명이 입력되지 않았습니다.","광고내역 복사", MessageBoxButtons.OK, MessageBoxIcon.Information );
				ebItemName.Focus();
				return;	               
			}

			DialogResult result = MessageBox.Show("선택한광고내역을 복사합니다.","광고내역복사", 
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
						MessageBox.Show("복사작업이 완료되었습니다.","광고내역 복사", MessageBoxButtons.OK, MessageBoxIcon.Information);
						this.Close();
					}
				}
			}
			catch(FrameException fe)
			{
				FrameSystem.showMsgForm("광고내역 저장오류", new string[] {fe.ErrCode, fe.ResultMsg});
			}
			catch(Exception ex)
			{
				FrameSystem.showMsgForm("광고내역 저장오류",new string[] {"",ex.Message});
			}			
		}
	}
}
