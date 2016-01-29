using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Web;
using System.Web.Services;

using WinFramework.Base;
using AdManagerModel;

namespace AdManagerWebService.Common
{

	/// <summary>
	/// 기본네임스페이스를 설정한다.
	/// </summary>
	[WebService(Namespace="http://advmgt.hanafostv.com/AdManagerWebService/")]
    [System.ComponentModel.ToolboxItem(false)]

	/// <summary>
	/// SystemMenuService에 대한 요약 설명입니다.
	/// </summary>
	public class SystemMenuService : System.Web.Services.WebService
	{
		private SystemMenuBiz systemMenuBiz = null;

		public SystemMenuService()
		{
			//CODEGEN: 이 호출은 ASP.NET 웹 서비스 디자이너에 필요합니다.
			InitializeComponent();

			systemMenuBiz = new SystemMenuBiz();
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

		[WebMethod]
		public SystemMenuModel GetComboList(HeaderModel header, SystemMenuModel systemMenuModel)
		{
			systemMenuBiz.GetComboList(header, systemMenuModel);
			return systemMenuModel;
		}

		[WebMethod]
		public SystemMenuModel GetUpperMenuList(HeaderModel header, SystemMenuModel systemMenuModel)
		{
			systemMenuBiz.GetUpperMenuList(header, systemMenuModel);
			return systemMenuModel;
		}

		[WebMethod]
		public SystemMenuModel GetMenuList(HeaderModel header, SystemMenuModel systemMenuModel)
		{
			systemMenuBiz.GetMenuList(header, systemMenuModel);
			return systemMenuModel;
		}

		[WebMethod]
		public SystemMenuModel SetUpperMenuUpdate(HeaderModel header, SystemMenuModel systemMenuModel)
		{
			systemMenuBiz.SetUpperMenuUpdate(header, systemMenuModel);
			return systemMenuModel;
		}

		[WebMethod]
		public SystemMenuModel SetMenuCodeUpdate(HeaderModel header, SystemMenuModel systemMenuModel)
		{
			systemMenuBiz.SetMenuCodeUpdate(header, systemMenuModel);
			return systemMenuModel;
		}

		[WebMethod]
		public SystemMenuModel SetUpperMenuCreate(HeaderModel header, SystemMenuModel systemMenuModel)
		{
			systemMenuBiz.SetUpperMenuCreate(header, systemMenuModel);
			return systemMenuModel;
		}

		[WebMethod]
		public SystemMenuModel SetMenuCodeCreate(HeaderModel header, SystemMenuModel systemMenuModel)
		{
			systemMenuBiz.SetMenuCodeCreate(header, systemMenuModel);
			return systemMenuModel;
		}

		[WebMethod]
		public SystemMenuModel SetUpperMenuDelete(HeaderModel header, SystemMenuModel systemMenuModel)
		{
			systemMenuBiz.SetUpperMenuDelete(header, systemMenuModel);
			return systemMenuModel;
		}

		[WebMethod]
		public SystemMenuModel SetMenuCodeDelete(HeaderModel header, SystemMenuModel systemMenuModel)
		{
			systemMenuBiz.SetMenuCodeDelete(header, systemMenuModel);
			return systemMenuModel;
		}

		[WebMethod]
		public SystemMenuModel SetMenuCodeOrderUp(HeaderModel header, SystemMenuModel systemMenuModel)
		{
			systemMenuBiz.SetMenuCodeOrderUp(header, systemMenuModel);
			return systemMenuModel;
		}

		[WebMethod]
		public SystemMenuModel SetMenuCodeOrderDown(HeaderModel header, SystemMenuModel systemMenuModel)
		{
			systemMenuBiz.SetMenuCodeOrderDown(header, systemMenuModel);
			return systemMenuModel;
		}

	}
}
