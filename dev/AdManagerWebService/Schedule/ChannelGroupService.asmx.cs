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
    /// �⺻���ӽ����̽��� �����Ѵ�.
    /// </summary>
    [WebService(Namespace = "http://advmgt.hanafostv.com/AdManagerWebService/")]
    [System.ComponentModel.ToolboxItem(false)]
	/// <summary>
	/// ChannelGroupService�� ���� ��� �����Դϴ�.
	/// </summary>
	public class ChannelGroupService : System.Web.Services.WebService
	{

		private ChannelGroupBiz channelGroupBiz = null;

		public ChannelGroupService()
		{
			//CODEGEN: �� ȣ���� ASP.NET �� ���� �����̳ʿ� �ʿ��մϴ�.
			InitializeComponent();

			channelGroupBiz = new ChannelGroupBiz();
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
        public ChannelGroupModel GetChannelGroupList(HeaderModel header, ChannelGroupModel channelGroupModel)
        {
            channelGroupBiz.GetChannelGroupList(header, channelGroupModel);
            return channelGroupModel;
        }
        [WebMethod]
        public ChannelGroupModel GetChannelGroupDetailList(HeaderModel header, ChannelGroupModel channelGroupModel)
        {
            channelGroupBiz.GetChannelGroupDetailList(header, channelGroupModel);
            return channelGroupModel;
        }
        [WebMethod]
        public ChannelGroupModel GetChannelList_p(HeaderModel header, ChannelGroupModel channelGroupModel)
        {
            channelGroupBiz.GetChannelList_p(header, channelGroupModel);
            return channelGroupModel;
        }

		[WebMethod]
		public ChannelGroupModel GetChannelList_Excel(HeaderModel header, ChannelGroupModel channelGroupModel)
		{
			channelGroupBiz.GetChannelList_Excel(header, channelGroupModel);
			return channelGroupModel;
		}

		[WebMethod]
		public ChannelGroupModel SetChannelGroupUpdate(HeaderModel header, ChannelGroupModel channelGroupModel)
		{
			channelGroupBiz.SetChannelGroupUpdate(header, channelGroupModel);
			return channelGroupModel;
		}

		[WebMethod]
		public ChannelGroupModel SetChannelGroupCreate(HeaderModel header, ChannelGroupModel channelGroupModel)
		{
			channelGroupBiz.SetChannelGroupCreate(header, channelGroupModel);
			return channelGroupModel;
		}

		[WebMethod]
		public ChannelGroupModel SetChannelGroupDelete(HeaderModel header, ChannelGroupModel channelGroupModel)
		{
			channelGroupBiz.SetChannelGroupDelete(header, channelGroupModel);
			return channelGroupModel;
		}
	}
}
