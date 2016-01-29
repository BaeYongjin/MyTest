using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Web;
using System.Web.Services;
using System.Data.SqlClient;

using WinFramework.Base;
using AdManagerModel;

namespace AdManagerWebService.Login
{
    /// <summary>
    /// 기본네임스페이스를 설정한다.
    /// </summary>
    [WebService(Namespace = "http://advmgt.hanafostv.com/AdManagerWebService/")]
    [System.ComponentModel.ToolboxItem(false)]

	/// <summary>
	/// LoginService에 대한 요약 설명입니다.
	/// </summary>
	public class LoginService : System.Web.Services.WebService
	{
		private LoginBiz loginBiz = null;

		public LoginService()
		{
			//CODEGEN: 이 호출은 ASP.NET 웹 서비스 디자이너에 필요합니다.
			InitializeComponent();
			loginBiz = new LoginBiz();
		}

		#region 구성 요소 디자이너에서 생성한 코드
		
		//웹 서비스 디자이너에 필요합니다. 
		private IContainer components = null;
				
		/// <summary>
		/// 디자이너 지원에 필요한 메서드입니다.
		/// 이 메서드의 내용을 코드 편집기로 수정하지 마십시오.
		/// </summary>
		private void InitializeComponent()
		{
		}

		/// <summary>
		/// 사용 중인 모든 리소스를 정리합니다.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if(disposing && components != null)
			{
				components.Dispose();
			}
			base.Dispose(disposing);		
		}
		
		#endregion

		// 웹 서비스 예제
		// HelloWorld() 예제 서비스는 Hello World라는 문자열을 반환합니다.
		// 빌드하려면 다음 줄의 주석 처리를 제거하고 저장한 후 해당 프로젝트를 빌드합니다.
		// 이 웹 서비스를 테스트하려면 <F5> 키를 누릅니다.

		[WebMethod]
		public string PasswordEncrypt(string str)
		{
			return loginBiz.PasswordEncrypt(str);
		}

		[WebMethod]
		public string PasswordDecrypt(string str)
		{
			return loginBiz.PasswordDecrypt(str);
		}

		[WebMethod]
		public LoginModel Login(HeaderModel header, LoginModel loginModel)
		{
			loginBiz = new LoginBiz();
			loginBiz.LoginCheck(header, loginModel);
			return loginModel;
		}
	}
}
