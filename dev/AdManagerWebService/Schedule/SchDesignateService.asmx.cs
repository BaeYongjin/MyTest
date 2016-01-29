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
    /// 기본네임스페이스를 설정한다.
    /// </summary>
    [WebService(Namespace = "http://advmgt.hanafostv.com/AdManagerWebService/")]
    [System.ComponentModel.ToolboxItem(false)]

	/// <summary>
	/// Sch3FormService에 대한 요약 설명입니다.
	/// </summary>
	public class SchDesignateService : System.Web.Services.WebService
	{
		private SchDesignateBiz	biz	= null;

		/// <summary>
		/// 지정편성 웹서비스를 생성합니다
		/// </summary>
		public SchDesignateService()
		{
			InitializeComponent(); 
			biz = new SchDesignateBiz();
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

		[WebMethod( Description="지정편성이 설정된 편성정보를 가져옵니다")]
		public SchDesignateModel GetList(HeaderModel header, SchDesignateModel	model)
		{
			biz.GetList( header, model );
			return model;
		}

		[WebMethod( Description="지정편성 대상 광고목록을 가져옵니다")]
		public SchDesignateModel GetItemList(HeaderModel header, SchDesignateModel	model)
		{
			biz.GetItemList( header, model );
			return model;
		}

		[WebMethod( Description="지정편성을 추가합니다")]
		public SchDesignateModel InsertData(HeaderModel header, SchDesignateModel	model)
		{
			biz.InsertData( header, model );
			return model;
		}

		[WebMethod( Description="지정편성을 삭제합니다")]
		public SchDesignateModel DeleteData(HeaderModel header, SchDesignateModel	model)
		{
			biz.DeleteData( header, model );
			return model;
		}
	}
}
