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
	/// FilePublishService�� ���� ��� �����Դϴ�.
	/// </summary>
	public class FilePublishService : System.Web.Services.WebService
	{
		public FilePublishService()
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

		[ WebMethod(Description="���Ϲ��� �����۾� ����ȸ") ]
		public FilePublishModel getReserveWorkSelect(HeaderModel header, FilePublishModel model)
		{
			new FilePublishBiz().GetReserveWorkSelect(header, model);
			return model;
		}

		[ WebMethod(Description="���Ϲ��� �����۾� �Է�") ]
		public FilePublishModel setReserveWorkInsert(HeaderModel header, FilePublishModel model)
		{
			new FilePublishBiz().SetReserveWorkInsert(header, model);
			return model;
		}

		[ WebMethod(Description="���Ϲ��� �����۾� ����") ]
		public FilePublishModel setReserveWorkUpdate(HeaderModel header, FilePublishModel model)
		{
			new FilePublishBiz().SetReserveWorkUpdate(header, model);
			return model;
		}

		[ WebMethod(Description="���Ϲ��� �����۾� ��ȸ") ]
		public FilePublishModel setReserveFileSelect(HeaderModel header, FilePublishModel model)
		{
			new FilePublishBiz().SetReserveFileSelect(header, model);
			return model;
		}

		[ WebMethod(Description="���Ϲ��� �����۾� ����") ]
		public FilePublishModel setReserveFileUpdate(HeaderModel header, FilePublishModel model)
		{
			new FilePublishBiz().SetReserveFileUpdate(header, model);
			return model;
		}

	}
}
