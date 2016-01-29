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
    /// �⺻���ӽ����̽��� �����Ѵ�.
    /// </summary>
    [WebService(Namespace = "http://advmgt.hanafostv.com/AdManagerWebService/")]
    [System.ComponentModel.ToolboxItem(false)]
	/// <summary>
	/// ChannelJumpService�� ���� ��� �����Դϴ�.
	/// </summary>
	public class ChannelJumpService : System.Web.Services.WebService
	{
		public ChannelJumpService()
		{
			//CODEGEN: �� ȣ���� ASP.NET �� ���� �����̳ʿ� �ʿ��մϴ�.
			InitializeComponent();
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
