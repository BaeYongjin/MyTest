using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Web;
using System.Web.Services;

using WinFramework.Base;
using AdManagerModel;

namespace AdManagerWebService.Schedule
{
	/// <summary>
	/// SchHomeKidsService의 요약 설명입니다.
	/// </summary>
	[WebService(Namespace = "http://advmgt.hanafostv.com/AdManagerWebService/")]
	[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
	[System.ComponentModel.ToolboxItem(false)]
	// ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
	// [System.Web.Script.Services.ScriptService]
	public class SchHomeKidsService : System.Web.Services.WebService
	{
		private SchHomeKidsBiz schHomeKidsBiz = null;

		public SchHomeKidsService()
		{
			InitializeComponent();
			schHomeKidsBiz = new SchHomeKidsBiz();
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
		protected override void Dispose(bool disposing)
		{
			if (disposing && components != null)
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#endregion

		[WebMethod]
		public SchHomeAdModel GetSchHomeKidsList(HeaderModel header, SchHomeAdModel schHomeKidsModel)
		{
			schHomeKidsBiz.GetSchHomeKidsList(header, schHomeKidsModel);
			return schHomeKidsModel;
		}

		[WebMethod]
		public SchHomeAdModel SetSchHomeKidsCreate(HeaderModel header, SchHomeAdModel schHomeKidsModel)
		{
			schHomeKidsBiz.SetSchHomeKidsCreate(header, schHomeKidsModel);
			return schHomeKidsModel;
		}

		[WebMethod]
		public SchHomeAdModel SetSchHomeKidsDelete(HeaderModel header, SchHomeAdModel schHomeKidsModel)
		{
			schHomeKidsBiz.SetSchHomeKidsDelete(header, schHomeKidsModel);
			return schHomeKidsModel;
		}

		[WebMethod]
		public SchHomeAdModel SetSchHomeKidsPlayType(HeaderModel header, SchHomeAdModel schHomeKidsModel)
		{
			schHomeKidsBiz.SetSchHomeKidsPlayType(header, schHomeKidsModel);
			return schHomeKidsModel;
		}

		[WebMethod]
		public SchHomeAdModel SetSchHomeKidsLogYn(HeaderModel header, SchHomeAdModel schHomeKidsModel)
		{
			schHomeKidsBiz.SetSchHomeKidsLogYn(header, schHomeKidsModel);
			return schHomeKidsModel;
		}


		[WebMethod]
		public SchHomeAdModel SetSchHomeKidsOrderFirst(HeaderModel header, SchHomeAdModel schHomeKidsModel)
		{
			schHomeKidsBiz.SetSchHomeKidsOrderFirst(header, schHomeKidsModel);
			return schHomeKidsModel;
		}

		[WebMethod]
		public SchHomeAdModel SetSchHomeKidsOrderUp(HeaderModel header, SchHomeAdModel schHomeKidsModel)
		{
			schHomeKidsBiz.SetSchHomeKidsOrderUp(header, schHomeKidsModel);
			return schHomeKidsModel;
		}

		[WebMethod]
		public SchHomeAdModel SetSchHomeKidsOrderDown(HeaderModel header, SchHomeAdModel schHomeKidsModel)
		{
			schHomeKidsBiz.SetSchHomeKidsOrderDown(header, schHomeKidsModel);
			return schHomeKidsModel;
		}

		[WebMethod]
		public SchHomeAdModel SetSchHomeKidsOrderLast(HeaderModel header, SchHomeAdModel schHomeKidsModel)
		{
			schHomeKidsBiz.SetSchHomeKidsOrderLast(header, schHomeKidsModel);
			return schHomeKidsModel;
		}

	}
}
