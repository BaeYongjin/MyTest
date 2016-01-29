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
	/// SchMenuAdService�� ���� ��� �����Դϴ�.
	/// </summary>
	public class SchMenuAdService : System.Web.Services.WebService
	{

		private SchMenuAdBiz schMenuAdBiz = null;

		public SchMenuAdService()
		{
			//CODEGEN: �� ȣ���� ASP.NET �� ���� �����̳ʿ� �ʿ��մϴ�.
			InitializeComponent();

			schMenuAdBiz = new SchMenuAdBiz();

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

		[WebMethod( Description="Ư������ ������ �������� ����[�帣/ä��/ȸ�� �� ����ó�� ����]")]
		public SchedulePerItemModel	SetSchedulePerItemDelete( HeaderModel header, SchedulePerItemModel data)
		{
			schMenuAdBiz.SetSchedulePerItemDelete( header, data );
			return data;
		}

		[WebMethod( Description="Ư������ ������ �������� �߰�[�帣/ä��/ȸ�� �� �߰�ó�� ����]")]
		public SchedulePerItemModel	SetSchedulePerItemInsert( HeaderModel header, SchedulePerItemModel data)
		{
			schMenuAdBiz.SetSchedulePerItemInsert( header, data );
			return data;
		}

		[WebMethod]
		public SchMenuAdModel GetMenuList(HeaderModel header, SchMenuAdModel schMenuAdModel)
		{
			schMenuAdBiz.GetMenuList(header, schMenuAdModel);
			return schMenuAdModel;
		}

		[WebMethod]
		public SchMenuAdModel GetItemScheduleList(HeaderModel header, SchMenuAdModel schMenuAdModel)
		{
			schMenuAdBiz.GetItemScheduleList(header, schMenuAdModel);
			return schMenuAdModel;
		}

		[WebMethod]
		public SchMenuAdModel GetContractItemList(HeaderModel header, SchMenuAdModel schMenuAdModel)
		{
			schMenuAdBiz.GetContractItemList(header, schMenuAdModel);
			return schMenuAdModel;
		}

		[WebMethod]
		public SchMenuAdModel GetSchChoiceMenuDetailList(HeaderModel header, SchMenuAdModel schMenuAdModel)
		{
			schMenuAdBiz.GetSchChoiceMenuDetailList(header, schMenuAdModel);
			return schMenuAdModel;
		}

		[WebMethod]
		public SchMenuAdModel GetSchChoiceMenuDetailContractSeq(HeaderModel header, SchMenuAdModel schMenuAdModel)
		{
			schMenuAdBiz.GetSchChoiceMenuDetailContractSeq(header, schMenuAdModel);
			return schMenuAdModel;
		}

		[WebMethod]
		public SchMenuAdModel GetChooseAdScheduleListMenu(HeaderModel header, SchMenuAdModel schMenuAdModel)
		{
			schMenuAdBiz.GetChooseAdScheduleListMenu(header, schMenuAdModel);
			return schMenuAdModel;
		}

		[WebMethod]
		public SchMenuAdModel GetChooseAdScheduleListContract(HeaderModel header, SchMenuAdModel schMenuAdModel)
		{
			schMenuAdBiz.GetChooseAdScheduleListContract(header, schMenuAdModel);
			return schMenuAdModel;
		}
	}
}
