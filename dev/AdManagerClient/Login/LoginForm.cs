// ===============================================================================
//
// LoginForm.cs
//
// 로그인처리 폼
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
		// 시스템 정보 : 화면공통
		private SystemModel   systemModel   = FrameSystem.oSysModel;
		private CommonModel   commonModel   = FrameSystem.oComModel;
		private Logger        log           = FrameSystem.oLog;

		// 데이터 모델 
		private LoginModel loginModel = new LoginModel();
		

		// private 변수
		private bool _IsLogin = false;

		private int tryCount = 3;

		#region 프로퍼티
		public bool IsLogin
		{
			get { return _IsLogin; }
		}
		#endregion

		#region 화면 objects
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

		#region 생성자 		
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

		#region Windows Form 디자이너에서 생성한 코드
		/// <summary>
		/// 디자이너 지원에 필요한 메서드입니다.
		/// 이 메서드의 내용을 코드 편집기로 수정하지 마십시오.
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
			this.btnLogin.Text = "확 인";
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
			this.btnCancel.Text = "취 소";
			this.btnCancel.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
			this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
			// 
			// lbClientType
			// 
			this.lbClientType.BackColor = System.Drawing.Color.Transparent;
			this.lbClientType.Font = new System.Drawing.Font("나눔고딕", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
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
			this.lbVersion.Font = new System.Drawing.Font("나눔고딕", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
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
				
		#region 화면초기화

		public void Init_Form()
		{
            // 2011.11.07 RH.Jung
            // 로그인 배경화면 이미지를 로드
            picLoginImg.ImageLocation = Application.StartupPath.ToString() + "/Images/login.jpg";
            Application.DoEvents(); // 일단 적용 

            // Label 배경을 투명하게 하려면 
            // Label.BackColor = System.Drawing.Color.Transparent;
            // Label.Parent = picLoginImg; //부모컨트롤
            lbVersion.Parent    = picLoginImg;
            lbClientType.Parent = picLoginImg;


			lbVersion.Text = FrameSystem.m_SystemVersion;
	
			switch(FrameSystem.m_ClientType)
			{
				case FrameSystem._DEV:
					lbClientType.Text = "개발자";
					break;
				case FrameSystem._REAL:
					lbClientType.Text = "";
					break;
				case FrameSystem._TEST:
					lbClientType.Text = "테스트";
					break;
				default:
					lbClientType.Text = "Unknown Client Type";
					break;
			}
		}

		#endregion

		#region 엑션처리 메소드
				
		private void btnLogin_Click(object sender, System.EventArgs e)
		{
			if (edtId.Text.Length == 0)	// 아이디 입력안함
			{
				MessageBox.Show(FrameMessage.GetMessage(4001),"로그인오류", MessageBoxButtons.OK, MessageBoxIcon.Information );	
				edtId.Focus();
				return;		
			}
				//FrameSystem.showMsgForm("로그인오류",new string [] {"",FrameMessage.GetMessage(4001)});			
			else
			{

				try
				{
					loginModel.UserID        = this.edtId.Text.Trim();
					loginModel.UserPassword  = Security.Encrypt(this.edtPwd.Text) ;
					loginModel.SystemVersion = FrameSystem.m_SystemVersion;

					// 로그인 서비스를 호출한다.
					new LoginManager(systemModel).Login(loginModel);

					// 로그인이 성공하였으면 CommonModel을 생성한다.
					// 로그인시 성고하지 않으면 LoginManager에서 Exception을 발생시키도록 되어있다.

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

					//1.커먼의 랩코드와 미디어 랩의 코드를 루프를 졸면서 비교
					//2.일치하는게 있다면 true 그러면 안으로 들어와 미디어렙의 사용여부를 체크
					//3.'N'이면 경고창					
					
					_IsLogin = true;

					this.Close();
				}
				catch(FrameException fe)
				{
					if(fe.ErrCode.Equals("1005"))
					{
						if(tryCount > 0)
						{
							MessageBox.Show(fe.ResultMsg + " 다시 시도해주십시오.","로그인오류", 
								MessageBoxButtons.OK, MessageBoxIcon.Information );	
							tryCount--;
						}
						else
						{
							MessageBox.Show("서버에 연결할 수 없습니다\n네트워크 상태 또는 환경파일을 확인하시기 바랍니다.","로그인오류", 
								MessageBoxButtons.OK, MessageBoxIcon.Information );	
						}
						return;		
					}
					if(fe.ErrCode.Equals("2000"))
					{
						MessageBox.Show(fe.ResultMsg,"로그인오류", 
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
						MessageBox.Show("ID : "+edtId.Text+"\n"+fe.ResultMsg,"로그인오류", 
							MessageBoxButtons.OK, MessageBoxIcon.Information );	
						edtId.Focus();
						return;		
					}
					else if(fe.ErrCode.Equals("2003"))
					{
						MessageBox.Show(fe.ResultMsg,"로그인오류", 
							MessageBoxButtons.OK, MessageBoxIcon.Information );	
						edtPwd.Text = "";
						edtPwd.Focus();
						return;			
					}
					else if(fe.ErrCode.Equals("2004"))
					{						
						MessageBox.Show("ID : " + edtId.Text + "는 매체레벨의 사용여부가 'N'으로 설정되어있습니다." +"\n" + "관리자에게 문의하여 주시기 바랍니다. ","로그인오류", 
							MessageBoxButtons.OK, MessageBoxIcon.Information );	
						edtId.Focus();
						return;		
					}
					else if(fe.ErrCode.Equals("2006"))
					{						
						MessageBox.Show("ID : " + edtId.Text + "는 랩사레벨의 사용여부가 'N'으로 설정되어있습니다." +"\n" + "관리자에게 문의하여 주시기 바랍니다.","로그인오류", 
							MessageBoxButtons.OK, MessageBoxIcon.Information );	
						edtId.Focus();
						return;		
					}
					else if(fe.ErrCode.Equals("2007"))
					{						
						MessageBox.Show("ID : " + edtId.Text + "는 대행사레벨의 사용여부가 'N'으로 설정되어있습니다." +"\n" + "관리자에게 문의하여 주시기 바랍니다.","로그인오류", 
							MessageBoxButtons.OK, MessageBoxIcon.Information );	
						edtId.Focus();
						return;		
					}
					else
					{
						FrameSystem.showMsgForm("로그인오류", new string[] {fe.ResultMsg, fe.ErrCode });
					}
				}
				catch(Exception ex)
				{
					MessageBox.Show(ex.Message,"로그인오류", 
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
