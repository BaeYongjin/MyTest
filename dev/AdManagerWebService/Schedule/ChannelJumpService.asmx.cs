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
	/// ChannelJumpService에 대한 요약 설명입니다.
	/// </summary>
	public class ChannelJumpService : System.Web.Services.WebService
	{
		public ChannelJumpService()
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
		public ChannelJumpModel GetChannelJumpList(HeaderModel header, ChannelJumpModel model)
		{
			new ChannelJumpBiz().GetChannelJumpList(header, model);
			return model;
		}

        [WebMethod]
        public string   GetContentListPopUrl()
        {
            ChannelJumpBiz Biz = new ChannelJumpBiz();
            string  rtnValue = "";
            rtnValue = Biz.GetContentListPopUrl();
            return rtnValue;
        }

        [WebMethod]
        public ChannelJumpModel GetChannelJump(HeaderModel header, ChannelJumpModel model)
        {
            new ChannelJumpBiz().GetChannelJump(header, model);
            return model;
        }
		
		[WebMethod]
		public ChannelJumpModel SetChannelJumpCreate(HeaderModel header, ChannelJumpModel model)
		{
			new ChannelJumpBiz().SetChannelJumpCreate(header, model);
			return model;
		}
		
		[WebMethod]
		public ChannelJumpModel SetChannelJumpUpdate(HeaderModel header, ChannelJumpModel model)
		{
			new ChannelJumpBiz().SetChannelJumpUpdate(header, model);
			return model;
		}
		
		[WebMethod]
		public ChannelJumpModel SetChannelJumpDelete(HeaderModel header, ChannelJumpModel model)
		{
			new ChannelJumpBiz().SetChannelJumpDelete(header, model);
			return model;
		}

		[WebMethod]
		public ChannelJumpModel GetContractItemList(HeaderModel header, ChannelJumpModel model)
		{
			new ChannelJumpBiz().GetContractItemList(header, model);
			return model;
		}

		[WebMethod]
		public ChannelJumpModel GetChannelList(HeaderModel header, ChannelJumpModel model)
		{
			new ChannelJumpBiz().GetChannelList(header, model);
			return model;
		}

		[WebMethod]
		public ChannelJumpModel GetContentList(HeaderModel header, ChannelJumpModel model)
		{
			new ChannelJumpBiz().GetContentList(header, model);
			return model;
		}

		[WebMethod]
		public ChannelJumpModel GetAdPopList(HeaderModel header, ChannelJumpModel model)
		{
			new ChannelJumpBiz().GetAdPopList(header, model);
			return model;
		}
	}
}
