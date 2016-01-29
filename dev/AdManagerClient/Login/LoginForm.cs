// ===============================================================================
//
// LoginForm.cs
//
// �α���ó�� ��
//
// ===============================================================================
// Release history
// 2007.08.15 RH.Jung Initialize
// 
// ===============================================================================
// Copyright (C) 2007 G-Inno Systems Inc.
// All rights reserved.
// ==============================================================================

using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Configuration;
using System.Data;
using System.Text;
using System.IO;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Net;
using System.Text.RegularExpressions;

using WinFramework.Misc;
using WinFramework.Base;

using AdManagerModel;

namespace AdManagerClient
{
	public class LoginForm : System.Windows.Forms.Form
	{	
		// �ý��� ���� : ȭ�����
		private SystemModel   systemModel   = FrameSystem.oSysModel;
		private CommonModel   commonModel   = FrameSystem.oComModel;
		private Logger        log           = FrameSystem.oLog;

		// ������ �� 
		private LoginModel loginModel = new LoginModel();
		

		// private ����
		private bool _IsLogin = false;

		private int tryCount = 3;

		#region ������Ƽ
		public bool IsLogin
		{
			get { return _IsLogin; }
		}
		#endregion

		#region ȭ�� objects
		private Janus.Windows.GridEX.EditControls.EditBox edtId;
		private Janus.Windows.GridEX.EditControls.EditBox edtPwd;
		private Janus.Windows.EditControls.UIButton btnLogin;
		private Janus.Windows.EditControls.UIButton btnCancel;
		private System.Windows.Forms.ImageList imageList;
		private System.Windows.Forms.Label lbClientType;
		private System.Windows.Forms.Label lbVersion;
        private PictureBox picLoginImg;
		private System.ComponentModel.IContainer components;
		#endregion

		#region ������ 		
		public LoginForm()
		{
			InitializeComponent();		
	
			Init_Form();
		}
			
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
		#endregion

		#region Windows Form �����̳ʿ��� ������ �ڵ�
		/// <summary>
		/// �����̳� ������ �ʿ��� �޼����Դϴ�.
		/// �� �޼����� ������ �ڵ� ������� �������� ���ʽÿ�.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LoginForm));
			this.edtId = new Janus.Windows.GridEX.EditControls.EditBox();
			this.edtPwd = new Janus.Windows.GridEX.EditControls.EditBox();
			this.btnLogin = new Janus.Windows.EditControls.UIButton();
			this.imageList = new System.Windows.Forms.ImageList(this.components);
			this.btnCancel = new Janus.Windows.EditControls.UIButton();
			this.lbClientType = new System.Windows.Forms.Label();
			this.picLoginImg = new System.Windows.Forms.PictureBox();
			this.lbVersion = new System.Windows.Forms.Label();
			((System.ComponentModel.ISupportInitialize)(this.picLoginImg)).BeginInit();
			this.SuspendLayout();
			// 
			// edtId
			// 
			this.edtId.BackColor = System.Drawing.Color.White;
			this.edtId.BorderStyle = Janus.Windows.GridEX.BorderStyle.Flat;
			this.edtId.Location = new System.Drawing.Point(472, 102);
			this.edtId.MaxLength = 10;
			this.edtId.Name = "edtId";
			this.edtId.Size = new System.Drawing.Size(96, 21);
			this.edtId.TabIndex = 0;
			this.edtId.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
			this.edtId.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2003;
			this.edtId.KeyDown += new System.Windows.Forms.KeyEventHandler(this.edtId_KeyDown);
			// 
			// edtPwd
			// 
			this.edtPwd.BackColor = System.Drawing.Color.White;
			this.edtPwd.Location = new System.Drawing.Point(472, 125);
			this.edtPwd.MaxLength = 10;
			this.edtPwd.Name = "edtPwd";
			this.edtPwd.PasswordChar = '*';
			this.edtPwd.Size = new System.Drawing.Size(96, 21);
			this.edtPwd.TabIndex = 1;
			this.edtPwd.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
			this.edtPwd.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2003;
			this.edtPwd.KeyDown += new System.Windows.Forms.KeyEventHandler(this.edtPwd_KeyDown);
			// 
			// btnLogin
			// 
			this.btnLogin.Cursor = System.Windows.Forms.Cursors.Hand;
			this.btnLogin.ImageIndex = 0;
			this.btnLogin.ImageList = this.imageList;
			this.btnLogin.ImageSize = new System.Drawing.Size(24, 24);
			this.btnLogin.Location = new System.Drawing.Point(472, 152);
			this.btnLogin.Name = "btnLogin";
			this.btnLogin.Size = new System.Drawing.Size(96, 26);
			this.btnLogin.TabIndex = 3;
			this.btnLogin.Text = "Ȯ ��";
			this.btnLogin.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
			this.btnLogin.Click += new System.EventHandler(this.btnLogin_Click);
			// 
			// imageList
			// 
			this.imageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList.ImageStream")));
			this.imageList.TransparentColor = System.Drawing.Color.Transparent;
			this.imageList.Images.SetKeyName(0, "play");
			this.imageList.Images.SetKeyName(1, "stop");
			// 
			// btnCancel
			// 
			this.btnCancel.Cursor = System.Windows.Forms.Cursors.Hand;
			this.btnCancel.ImageIndex = 1;
			this.btnCancel.ImageList = this.imageList;
			this.btnCancel.ImageSize = new System.Drawing.Size(24, 24);
			this.btnCancel.Location = new System.Drawing.Point(472, 182);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(96, 26);
			this.btnCancel.TabIndex = 4;
			this.btnCancel.Text = "�� ��";
			this.btnCancel.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
			this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
			// 
			// lbClientType
			// 
			this.lbClientType.BackColor = System.Drawing.Color.Transparent;
			this.lbClientType.Font = new System.Drawing.Font("�������", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
			this.lbClientType.ForeColor = System.Drawing.Color.Red;
			this.lbClientType.Location = new System.Drawing.Point(419, 224);
			this.lbClientType.Name = "lbClientType";
			this.lbClientType.Size = new System.Drawing.Size(168, 31);
			this.lbClientType.TabIndex = 5;
			this.lbClientType.Text = "Client Type";
			this.lbClientType.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// picLoginImg
			// 
			this.picLoginImg.Dock = System.Windows.Forms.DockStyle.Fill;
			this.picLoginImg.Image = global::AdManagerClient.Properties.Resources.login;
			this.picLoginImg.InitialImage = ((System.Drawing.Image)(resources.GetObject("picLoginImg.InitialImage")));
			this.picLoginImg.Location = new System.Drawing.Point(0, 0);
			this.picLoginImg.Margin = new System.Windows.Forms.Padding(0);
			this.picLoginImg.Name = "picLoginImg";
			this.picLoginImg.Size = new System.Drawing.Size(610, 296);
			this.picLoginImg.TabIndex = 7;
			this.picLoginImg.TabStop = false;
			this.picLoginImg.Click += new System.EventHandler(this.picLoginImg_Click);
			// 
			// lbVersion
			// 
			this.lbVersion.BackColor = System.Drawing.Color.Transparent;
			this.lbVersion.Font = new System.Drawing.Font("�������", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
			this.lbVersion.Location = new System.Drawing.Point(204, 75);
			this.lbVersion.Name = "lbVersion";
			this.lbVersion.Size = new System.Drawing.Size(168, 16);
			this.lbVersion.TabIndex = 6;
			this.lbVersion.Text = "Ver 1.0";
			this.lbVersion.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// LoginForm
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(6, 14);
			this.BackColor = System.Drawing.SystemColors.Window;
			this.ClientSize = new System.Drawing.Size(610, 296);
			this.Controls.Add(this.lbVersion);
			this.Controls.Add(this.lbClientType);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.btnLogin);
			this.Controls.Add(this.edtPwd);
			this.Controls.Add(this.edtId);
			this.Controls.Add(this.picLoginImg);
			this.Font = global::AdManagerClient.Properties.Settings.Default.Font1;
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.KeyPreview = true;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "LoginForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Login";
			this.Load += new System.EventHandler(this.LoginForm_Load);
			((System.ComponentModel.ISupportInitialize)(this.picLoginImg)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

        }
		#endregion
				
		#region ȭ���ʱ�ȭ

		public void Init_Form()
		{
            // 2011.11.07 RH.Jung
            // �α��� ���ȭ�� �̹����� �ε�
            picLoginImg.ImageLocation = Application.StartupPath.ToString() + "/Images/login.jpg";
            Application.DoEvents(); // �ϴ� ���� 

            // Label ����� �����ϰ� �Ϸ��� 
            // Label.BackColor = System.Drawing.Color.Transparent;
            // Label.Parent = picLoginImg; //�θ���Ʈ��
            lbVersion.Parent    = picLoginImg;
            lbClientType.Parent = picLoginImg;


			lbVersion.Text = FrameSystem.m_SystemVersion;
	
			switch(FrameSystem.m_ClientType)
			{
				case FrameSystem._DEV:
					lbClientType.Text = "������";
					break;
				case FrameSystem._REAL:
					lbClientType.Text = "";
					break;
				case FrameSystem._TEST:
					lbClientType.Text = "�׽�Ʈ";
					break;
				default:
					lbClientType.Text = "Unknown Client Type";
					break;
			}
		}

		#endregion

		#region ����ó�� �޼ҵ�
				
		private void btnLogin_Click(object sender, System.EventArgs e)
		{
			if (edtId.Text.Length == 0)	// ���̵� �Է¾���
			{
				MessageBox.Show(FrameMessage.GetMessage(4001),"�α��ο���", MessageBoxButtons.OK, MessageBoxIcon.Information );	
				edtId.Focus();
				return;		
			}
				//FrameSystem.showMsgForm("�α��ο���",new string [] {"",FrameMessage.GetMessage(4001)});			
			else
			{

				try
				{
					loginModel.UserID        = this.edtId.Text.Trim();
					loginModel.UserPassword  = Security.Encrypt(this.edtPwd.Text) ;
					loginModel.SystemVersion = FrameSystem.m_SystemVersion;

					// �α��� ���񽺸� ȣ���Ѵ�.
					new LoginManager(systemModel).Login(loginModel);

					// �α����� �����Ͽ����� CommonModel�� �����Ѵ�.
					// �α��ν� �������� ������ LoginManager���� Exception�� �߻���Ű���� �Ǿ��ִ�.

					FrameSystem.oComModel = new CommonModel();
					FrameSystem.oComModel.UserID    = loginModel.UserID;
					FrameSystem.oComModel.UserName  = loginModel.UserName;
					FrameSystem.oComModel.UserLevel = loginModel.UserLevel;
					FrameSystem.oComModel.LevelName = loginModel.LevelName;
					FrameSystem.oComModel.UserClass = loginModel.UserClass;
					FrameSystem.oComModel.ClassName = loginModel.ClassName;
					FrameSystem.oComModel.MediaCode = loginModel.MediaCode;
					FrameSystem.oComModel.MediaName = loginModel.MediaName;
					FrameSystem.oComModel.RapCode = loginModel.RapCode;
					FrameSystem.oComModel.RapName = loginModel.RapName;
					FrameSystem.oComModel.AgencyCode = loginModel.AgencyCode;
					FrameSystem.oComModel.AgencyName = loginModel.AgencyName;
					FrameSystem.oComModel.LoginTime = loginModel.LoginTime;
					FrameSystem.oComModel.LastLogin = loginModel.LastLogin;

					//1.Ŀ���� ���ڵ�� �̵�� ���� �ڵ带 ������ ���鼭 ��
					//2.��ġ�ϴ°� �ִٸ� true �׷��� ������ ���� �̵��� ��뿩�θ� üũ
					//3.'N'�̸� ���â					
					
					_IsLogin = true;

					this.Close();
				}
				catch(FrameException fe)
				{
					if(fe.ErrCode.Equals("1005"))
					{
						if(tryCount > 0)
						{
							MessageBox.Show(fe.ResultMsg + " �ٽ� �õ����ֽʽÿ�.","�α��ο���", 
								MessageBoxButtons.OK, MessageBoxIcon.Information );	
							tryCount--;
						}
						else
						{
							MessageBox.Show("������ ������ �� �����ϴ�\n��Ʈ��ũ ���� �Ǵ� ȯ�������� Ȯ���Ͻñ� �ٶ��ϴ�.","�α��ο���", 
								MessageBoxButtons.OK, MessageBoxIcon.Information );	
						}
						return;		
					}
					if(fe.ErrCode.Equals("2000"))
					{
						MessageBox.Show(fe.ResultMsg,"�α��ο���", 
							MessageBoxButtons.OK, MessageBoxIcon.Information );	

						if(FrameSystem.m_ClientType == FrameSystem._REAL)
						{
							System.Diagnostics.Process.Start("http://"+ FrameSystem.m_WebServer_Host + ":" + FrameSystem.m_WebServer_Port + "/Update/AdManagerUpdate.html");
						}
						else if(FrameSystem.m_ClientType == FrameSystem._TEST)
						{
							System.Diagnostics.Process.Start("http://"+ FrameSystem.m_WebServer_Host + ":" + FrameSystem.m_WebServer_Port + "/download/AdManagerUpdate.html");
						}

						this.Close();
					}
					else if(fe.ErrCode.Equals("2002"))
					{						
						MessageBox.Show("ID : "+edtId.Text+"\n"+fe.ResultMsg,"�α��ο���", 
							MessageBoxButtons.OK, MessageBoxIcon.Information );	
						edtId.Focus();
						return;		
					}
					else if(fe.ErrCode.Equals("2003"))
					{
						MessageBox.Show(fe.ResultMsg,"�α��ο���", 
							MessageBoxButtons.OK, MessageBoxIcon.Information );	
						edtPwd.Text = "";
						edtPwd.Focus();
						return;			
					}
					else if(fe.ErrCode.Equals("2004"))
					{						
						MessageBox.Show("ID : " + edtId.Text + "�� ��ü������ ��뿩�ΰ� 'N'���� �����Ǿ��ֽ��ϴ�." +"\n" + "�����ڿ��� �����Ͽ� �ֽñ� �ٶ��ϴ�. ","�α��ο���", 
							MessageBoxButtons.OK, MessageBoxIcon.Information );	
						edtId.Focus();
						return;		
					}
					else if(fe.ErrCode.Equals("2006"))
					{						
						MessageBox.Show("ID : " + edtId.Text + "�� ���緹���� ��뿩�ΰ� 'N'���� �����Ǿ��ֽ��ϴ�." +"\n" + "�����ڿ��� �����Ͽ� �ֽñ� �ٶ��ϴ�.","�α��ο���", 
							MessageBoxButtons.OK, MessageBoxIcon.Information );	
						edtId.Focus();
						return;		
					}
					else if(fe.ErrCode.Equals("2007"))
					{						
						MessageBox.Show("ID : " + edtId.Text + "�� ����緹���� ��뿩�ΰ� 'N'���� �����Ǿ��ֽ��ϴ�." +"\n" + "�����ڿ��� �����Ͽ� �ֽñ� �ٶ��ϴ�.","�α��ο���", 
							MessageBoxButtons.OK, MessageBoxIcon.Information );	
						edtId.Focus();
						return;		
					}
					else
					{
						FrameSystem.showMsgForm("�α��ο���", new string[] {fe.ResultMsg, fe.ErrCode });
					}
				}
				catch(Exception ex)
				{
					MessageBox.Show(ex.Message,"�α��ο���", 
						MessageBoxButtons.OK, MessageBoxIcon.Information );					
				}
			}		
		}


		private void btnCancel_Click(object sender, System.EventArgs e)
		{
			_IsLogin = false;
			
			this.Close();			
		}

		private void edtId_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
		{
			if(e.KeyCode == Keys.Enter)
			{
				this.edtPwd.Focus();
			}		
		}

		private void edtPwd_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
		{
			if(e.KeyCode == Keys.Enter)
			{
				this.btnLogin_Click(btnLogin, e);
			}				
		}

		#endregion

        private void LoginForm_Load(object sender, System.EventArgs e)
        {
            edtId.Focus();
        }

        private void picLoginImg_Click(object sender, EventArgs e)
        {

        }

	}
}
