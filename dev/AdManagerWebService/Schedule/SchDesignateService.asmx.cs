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
	/// Sch3FormService�� ���� ��� �����Դϴ�.
	/// </summary>
	public class SchDesignateService : System.Web.Services.WebService
	{
		private SchDesignateBiz	biz	= null;

		/// <summary>
		/// ������ �����񽺸� �����մϴ�
		/// </summary>
		public SchDesignateService()
		{
			InitializeComponent(); 
			biz = new SchDesignateBiz();
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

		[WebMethod( Description="�������� ������ �������� �����ɴϴ�")]
		public SchDesignateModel GetList(HeaderModel header, SchDesignateModel	model)
		{
			biz.GetList( header, model );
			return model;
		}

		[WebMethod( Description="������ ��� �������� �����ɴϴ�")]
		public SchDesignateModel GetItemList(HeaderModel header, SchDesignateModel	model)
		{
			biz.GetItemList( header, model );
			return model;
		}

		[WebMethod( Description="�������� �߰��մϴ�")]
		public SchDesignateModel InsertData(HeaderModel header, SchDesignateModel	model)
		{
			biz.InsertData( header, model );
			return model;
		}

		[WebMethod( Description="�������� �����մϴ�")]
		public SchDesignateModel DeleteData(HeaderModel header, SchDesignateModel	model)
		{
			biz.DeleteData( header, model );
			return model;
		}
	}
}
