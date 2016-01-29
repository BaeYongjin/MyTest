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
	/// ChannelService�� ���� ��� �����Դϴ�.
	/// </summary>
	public class ChannelService : System.Web.Services.WebService
	{

		private ChannelBiz channelBiz = null;

		public ChannelService()
		{
			//CODEGEN: �� ȣ���� ASP.NET �� ���� �����̳ʿ� �ʿ��մϴ�.
			InitializeComponent();

			channelBiz = new ChannelBiz();
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
        public ChannelModel GetChannelList(HeaderModel header, ChannelModel channelModel)
        {
            channelBiz.GetChannelList(header, channelModel);
            return channelModel;
        }
        [WebMethod]
        public ChannelModel GetChannelDetailList(HeaderModel header, ChannelModel channelModel)
        {
            channelBiz.GetChannelDetailList(header, channelModel);
            return channelModel;
        }
        [WebMethod]
        public ChannelModel GetChannelSetDetailList(HeaderModel header, ChannelModel channelModel)
        {
            channelBiz.GetChannelSetDetailList(header, channelModel);
            return channelModel;
        }
		[WebMethod]
		public ChannelModel SetChannelUpdate(HeaderModel header, ChannelModel channelModel)
		{
			channelBiz.SetChannelUpdate(header, channelModel);
			return channelModel;
		}

		[WebMethod]
		public ChannelModel SetChannelCreate(HeaderModel header, ChannelModel channelModel)
		{
			channelBiz.SetChannelCreate(header, channelModel);
			return channelModel;
		}

		[WebMethod]
		public ChannelModel SetChannelDelete(HeaderModel header, ChannelModel channelModel)
		{
			channelBiz.SetChannelDelete(header, channelModel);
			return channelModel;
		}
	}
}
