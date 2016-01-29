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
    /// �⺻���ӽ����̽��� �����Ѵ�.
    /// </summary>
    [WebService(Namespace = "http://advmgt.hanafostv.com/AdManagerWebService/")]
    [System.ComponentModel.ToolboxItem(false)]

	/// <summary>
	/// ChannelSetService�� ���� ��� �����Դϴ�.
	/// </summary>
	public class ChannelSetService : System.Web.Services.WebService
	{

		private ChannelSetBiz channelSetBiz = null;

		public ChannelSetService()
		{
			//CODEGEN: �� ȣ���� ASP.NET �� ���� �����̳ʿ� �ʿ��մϴ�.
			InitializeComponent();

			channelSetBiz = new ChannelSetBiz();
		}

		#region ���� ��� �����̳ʿ��� ������ �ڵ�
		
		//�� ���� �����̳ʿ� �ʿ��մϴ�. 
		private IContainer components = null;
				
		/// <summary>
		/// �����̳� ������ �ʿ��� �޼����Դϴ�.
		/// �� �޼����� ������ �ڵ� ������� �������� ���ʽÿ�.
		/// </summary>
		private void InitializeComponent()
		{
		}

		/// <summary>
		/// ��� ���� ��� ���ҽ��� �����մϴ�.
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
