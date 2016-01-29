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
	/// UserInfoService�� ���� ��� �����Դϴ�.
	/// </summary>
	public class MediaInfoService : System.Web.Services.WebService
	{

		private MediaInfoBiz mediaInfoBiz = null;

		public MediaInfoService()
		{
			//CODEGEN: �� ȣ���� ASP.NET �� ���� �����̳ʿ� �ʿ��մϴ�.
			InitializeComponent();

			mediaInfoBiz = new MediaInfoBiz();
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
		public MediaInfoModel GetUsersList(HeaderModel header, MediaInfoModel mediasModel)
		{
			mediaInfoBiz.GetUsersList(header, mediasModel);
			return mediasModel;
		}

		[WebMethod]
		public MediaInfoModel SetUserUpdate(HeaderModel header, MediaInfoModel mediasModel)
		{
			mediaInfoBiz.SetUserUpdate(header, mediasModel);
			return mediasModel;
		}

		[WebMethod]
		public MediaInfoModel SetUserCreate(HeaderModel header, MediaInfoModel mediasModel)
		{
			mediaInfoBiz.SetUserCreate(header, mediasModel);
			return mediasModel;
		}

		[WebMethod]
		public MediaInfoModel SetUserDelete(HeaderModel header, MediaInfoModel mediasModel)
		{
			mediaInfoBiz.SetUserDelete(header, mediasModel);
			return mediasModel;
		}
	}
}