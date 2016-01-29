using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Web;
using System.Web.Services;

using WinFramework.Base;
using AdManagerModel;

namespace AdManagerWebService.Media
{
    /// <summary>
    /// 기본네임스페이스를 설정한다.
    /// </summary>
    [WebService(Namespace = "http://advmgt.hanafostv.com/AdManagerWebService/")]
    [System.ComponentModel.ToolboxItem(false)]

	/// <summary>
	/// ChannelSetService에 대한 요약 설명입니다.
	/// </summary>
	public class ChannelSetService : System.Web.Services.WebService
	{

		private ChannelSetBiz channelSetBiz = null;

		public ChannelSetService()
		{
			//CODEGEN: 이 호출은 ASP.NET 웹 서비스 디자이너에 필요합니다.
			InitializeComponent();

			channelSetBiz = new ChannelSetBiz();
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
		public ChannelSetModel GetCategoryList(HeaderModel header, ChannelSetModel channelSetModel)
		{
			channelSetBiz.GetCategoryList(header, channelSetModel);
			return channelSetModel;
		}
		[WebMethod]
		public ChannelSetModel GetGenreList(HeaderModel header, ChannelSetModel channelSetModel)
		{
			channelSetBiz.GetGenreList(header, channelSetModel);
			return channelSetModel;
		}
		[WebMethod]
		public ChannelSetModel GetChannelSetList(HeaderModel header, ChannelSetModel channelSetModel)
		{
			channelSetBiz.GetChannelSetList(header, channelSetModel);
			return channelSetModel;
		}
		[WebMethod]
		public ChannelSetModel GetCategenList(HeaderModel header, ChannelSetModel channelSetModel)
		{
			channelSetBiz.GetCategenList(header, channelSetModel);
			return channelSetModel;
		}

		[WebMethod]
		public ChannelSetModel GetChannelNoPopList(HeaderModel header, ChannelSetModel channelSetModel)
		{
			channelSetBiz.GetChannelNoPopList(header, channelSetModel);
			return channelSetModel;
		}

		[WebMethod]
		public ChannelSetModel SetChannelSetUpdate(HeaderModel header, ChannelSetModel channelSetModel)
		{
			channelSetBiz.SetChannelSetUpdate(header, channelSetModel);
			return channelSetModel;
		}

		[WebMethod]
		public ChannelSetModel SetChannelSetCreate(HeaderModel header, ChannelSetModel channelSetModel)
		{
			channelSetBiz.SetChannelSetCreate(header, channelSetModel);
			return channelSetModel;
		}

		[WebMethod]
		public ChannelSetModel SetChannelSetDelete(HeaderModel header, ChannelSetModel channelSetModel)
		{
			channelSetBiz.SetChannelSetDelete(header, channelSetModel);
			return channelSetModel;
		}		
	}
}
