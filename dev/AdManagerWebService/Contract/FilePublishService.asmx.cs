using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Web;
using System.Web.Services;

using WinFramework.Base;
using AdManagerModel;

namespace AdManagerWebService.Contract
{
    /// <summary>
    /// 기본네임스페이스를 설정한다.
    /// </summary>
    [WebService(Namespace = "http://advmgt.hanafostv.com/AdManagerWebService/")]
    [System.ComponentModel.ToolboxItem(false)]

	/// <summary>
	/// FilePublishService에 대한 요약 설명입니다.
	/// </summary>
	public class FilePublishService : System.Web.Services.WebService
	{
		public FilePublishService()
		{
			//CODEGEN: 이 호출은 ASP.NET 웹 서비스 디자이너에 필요합니다.
			InitializeComponent();
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
		public FilePublishModel GetPublishState(HeaderModel header, FilePublishModel model)
		{
			new FilePublishBiz().GetPublishState(header, model);
			return model;
		}

		[WebMethod]
		public FilePublishModel GetPublishList(HeaderModel header, FilePublishModel model)
		{
			new FilePublishBiz().GetPublishList(header, model);
			return model;
		}

		[WebMethod]
		public FilePublishModel GetPublishHistory(HeaderModel header, FilePublishModel model)
		{
			new FilePublishBiz().GetPublishHistory(header, model);
			return model;
		}

		[WebMethod]
		public FilePublishModel SetFilePublish(HeaderModel header, FilePublishModel model)
		{
			new FilePublishBiz().SetFilePublish(header, model);
			return model;
		}

		[WebMethod]
		public FilePublishModel GetPublishFileList(HeaderModel header, FilePublishModel model)
		{
			new FilePublishBiz().GetPublishFileList(header, model);
			return model;
		}

		[WebMethod]
		public FilePublishModel GetReserveFiles(HeaderModel header, FilePublishModel model)
		{
			new FilePublishBiz().GetReserveFiles(header, model);
			return model;
		}

		[WebMethod]
		public FilePublishModel GetReserveWorks(HeaderModel header, FilePublishModel model)
		{
			new FilePublishBiz().GetReserveWorks(header, model);
			return model;
		}

		[ WebMethod(Description="파일배포 예약작업 상세조회") ]
		public FilePublishModel getReserveWorkSelect(HeaderModel header, FilePublishModel model)
		{
			new FilePublishBiz().GetReserveWorkSelect(header, model);
			return model;
		}

		[ WebMethod(Description="파일배포 예약작업 입력") ]
		public FilePublishModel setReserveWorkInsert(HeaderModel header, FilePublishModel model)
		{
			new FilePublishBiz().SetReserveWorkInsert(header, model);
			return model;
		}

		[ WebMethod(Description="파일배포 예약작업 저장") ]
		public FilePublishModel setReserveWorkUpdate(HeaderModel header, FilePublishModel model)
		{
			new FilePublishBiz().SetReserveWorkUpdate(header, model);
			return model;
		}

		[ WebMethod(Description="파일배포 파일작업 조회") ]
		public FilePublishModel setReserveFileSelect(HeaderModel header, FilePublishModel model)
		{
			new FilePublishBiz().SetReserveFileSelect(header, model);
			return model;
		}

		[ WebMethod(Description="파일배포 파일작업 저장") ]
		public FilePublishModel setReserveFileUpdate(HeaderModel header, FilePublishModel model)
		{
			new FilePublishBiz().SetReserveFileUpdate(header, model);
			return model;
		}

	}
}
