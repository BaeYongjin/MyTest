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
	/// SchChoiceAdService�� ���� ��� �����Դϴ�.
	/// </summary>
	public class SchChoiceAdService : System.Web.Services.WebService
	{

		private SchChoiceAdBiz schChoiceAdBiz = null;

		public SchChoiceAdService()
		{
			//CODEGEN: �� ȣ���� ASP.NET �� ���� �����̳ʿ� �ʿ��մϴ�.
			InitializeComponent();

			schChoiceAdBiz = new SchChoiceAdBiz();
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


        [WebMethod(Description="������� ����� �����ɴϴ�")]
        public SchChoiceAdModel mGetAdList10(HeaderModel header, SchChoiceAdModel schChoiceAdModel)
        {
            schChoiceAdBiz.GetAdList10(header, schChoiceAdModel);
            return schChoiceAdModel;
        }


		[WebMethod]
		public SchChoiceAdModel GetSchChoiceAdList(HeaderModel header, SchChoiceAdModel schChoiceAdModel)
		{
			schChoiceAdBiz.GetSchChoiceAdList(header, schChoiceAdModel);
			return schChoiceAdModel;
		}

		[WebMethod]
		public SchChoiceAdModel GetInspectItemList(HeaderModel header, SchChoiceAdModel schChoiceAdModel)
		{
			schChoiceAdBiz.GetInspectItemList(header, schChoiceAdModel);
			return schChoiceAdModel;
		}

		[WebMethod]
		public SchChoiceAdModel GetContractItemList(HeaderModel header, SchChoiceAdModel schChoiceAdModel)
		{
			schChoiceAdBiz.GetContractItemList(header, schChoiceAdModel);
			return schChoiceAdModel;
		}

        [WebMethod]
        public SchChoiceAdModel GetContractItemList_0907a(HeaderModel header, SchChoiceAdModel schChoiceAdModel)
        {
            // ������ ���׷��̵� ������
            // ��ġ �Ϸ��� ���� �Լ��� �����Ұ�
            schChoiceAdBiz.GetContractItemList_0907a(header, schChoiceAdModel);
            return schChoiceAdModel;
        }
		
		[WebMethod]
		public SchChoiceAdModel SetSchChoiceChannelCreate(HeaderModel header, SchChoiceAdModel schChoiceAdModel)
		{
			schChoiceAdBiz.SetSchChoiceChannelCreate(header, schChoiceAdModel);
			return schChoiceAdModel;
		}

		[WebMethod]
		public SchChoiceAdModel SetSchChoiceChannelDelete(HeaderModel header, SchChoiceAdModel schChoiceAdModel)
		{
			schChoiceAdBiz.SetSchChoiceChannelDelete(header, schChoiceAdModel);
			return schChoiceAdModel;
		}

		

		[WebMethod]
		public SchChoiceAdModel SetSchChoiceChannelDetailCreate(HeaderModel header, SchChoiceAdModel schChoiceAdModel)
		{
			schChoiceAdBiz.SetSchChoiceChannelDetailCreate(header, schChoiceAdModel);
			return schChoiceAdModel;
		}

		[WebMethod]
		public SchChoiceAdModel SetSchChoiceSeriesDetailCreate(HeaderModel header, SchChoiceAdModel schChoiceAdModel)
		{
			schChoiceAdBiz.SetSchChoiceSeriesDetailCreate(header, schChoiceAdModel);
			return schChoiceAdModel;
		}

		[WebMethod]
		public SchChoiceAdModel SetSchChoiceChannelDetailDelete(HeaderModel header, SchChoiceAdModel schChoiceAdModel)
		{
			schChoiceAdBiz.SetSchChoiceChannelDetailDelete(header, schChoiceAdModel);
			return schChoiceAdModel;
		}

		[WebMethod]
		public SchChoiceAdModel SetSchChoiceMenuCreate(HeaderModel header, SchChoiceAdModel schChoiceAdModel)
		{
			schChoiceAdBiz.SetSchChoiceMenuCreate(header, schChoiceAdModel);
			return schChoiceAdModel;
		}

		[WebMethod]
		public SchChoiceAdModel SetSchChoiceMenuDelete(HeaderModel header, SchChoiceAdModel schChoiceAdModel)
		{
			schChoiceAdBiz.SetSchChoiceMenuDelete(header, schChoiceAdModel);
			return schChoiceAdModel;
		}

		[WebMethod]
		public SchChoiceAdModel SetSchChoiceMenuDetailCreate(HeaderModel header, SchChoiceAdModel schChoiceAdModel)
		{
			schChoiceAdBiz.SetSchChoiceMenuDetailCreate(header, schChoiceAdModel);
			return schChoiceAdModel;
		}

		[WebMethod(Description = "���α��� �󼼳��� �� ó��")]
		public SchChoiceAdModel SetSchChoiceRealChDetailCreate(HeaderModel header, SchChoiceAdModel schChoiceAdModel)
		{
			schChoiceAdBiz.SetSchChoiceRealChDetailCreate(header, schChoiceAdModel);
			return schChoiceAdModel;
		}

		[WebMethod]
		public SchChoiceAdModel SetSchChoiceMenuDetailDelete(HeaderModel header, SchChoiceAdModel schChoiceAdModel)
		{
			schChoiceAdBiz.SetSchChoiceMenuDetailDelete(header, schChoiceAdModel);
			return schChoiceAdModel;
		}

		[WebMethod(Description = "���α��� �󼼳��� �� ����")]
		public SchChoiceAdModel SetSchChoiceRealChDetailDelete(HeaderModel header, SchChoiceAdModel schChoiceAdModel)
		{
			schChoiceAdBiz.SetSchChoiceRealChDetailDelete(header, schChoiceAdModel);
			return schChoiceAdModel;
		}

		[WebMethod]
		public SchChoiceAdModel GetSchChoiceMenuDetailList(HeaderModel header, SchChoiceAdModel schChoiceAdModel)
		{
			schChoiceAdBiz.GetSchChoiceMenuDetailList(header, schChoiceAdModel);
			return schChoiceAdModel;
		}

		[WebMethod]
		public SchChoiceAdModel GetSchChoiceChannelDetailList(HeaderModel header, SchChoiceAdModel schChoiceAdModel)
		{
			schChoiceAdBiz.GetSchChoiceChannelDetailList(header, schChoiceAdModel);
			return schChoiceAdModel;
		}

		[WebMethod]
		public SchChoiceAdModel GetSchChoiceLastItemNo(HeaderModel header, SchChoiceAdModel schChoiceAdModel)
		{
			schChoiceAdBiz.GetSchChoiceLastItemNo(header, schChoiceAdModel);
			return schChoiceAdModel;
		}

		[WebMethod]
		public SchChoiceAdModel SetSchChoiceMenuDelete_To(HeaderModel header, SchChoiceAdModel schChoiceAdModel)
		{
			schChoiceAdBiz.SetSchChoiceMenuDelete_To(header, schChoiceAdModel);
			return schChoiceAdModel;
		}

		[WebMethod]
		public SchChoiceAdModel SetSchChoiceMenuDetailCreate_To(HeaderModel header, SchChoiceAdModel schChoiceAdModel)
		{
			schChoiceAdBiz.SetSchChoiceMenuDetailCreate_To(header, schChoiceAdModel);
			return schChoiceAdModel;
		}

		[WebMethod]
		public SchChoiceAdModel SetSchChoiceSearch(HeaderModel header, SchChoiceAdModel schChoiceAdModel)
		{
			schChoiceAdBiz.SetSchChoiceSearch(header, schChoiceAdModel);
			return schChoiceAdModel;
		}	
	
		[WebMethod]
		public SchChoiceAdModel SetSchChoiceChannelSearch(HeaderModel header, SchChoiceAdModel schChoiceAdModel)
		{
			schChoiceAdBiz.SetSchChoiceChannelSearch(header, schChoiceAdModel);
			return schChoiceAdModel;
		}	

		[WebMethod]
		public SchChoiceAdModel SetSchChoiceChannelDelete_To(HeaderModel header, SchChoiceAdModel schChoiceAdModel)
		{
			schChoiceAdBiz.SetSchChoiceChannelDelete_To(header, schChoiceAdModel);
			return schChoiceAdModel;
		}

		[WebMethod]
		public SchChoiceAdModel SetSchChoiceChannelDetailCreate_To(HeaderModel header, SchChoiceAdModel schChoiceAdModel)
		{
			schChoiceAdBiz.SetSchChoiceChannelDetailCreate_To(header, schChoiceAdModel);
			return schChoiceAdModel;
		}

		[WebMethod]
		public SchChoiceAdModel GetSchChoiceLastItemNoDelete(HeaderModel header, SchChoiceAdModel schChoiceAdModel)
		{
			schChoiceAdBiz.GetSchChoiceLastItemNoDelete(header, schChoiceAdModel);
			return schChoiceAdModel;
		}

        /// <summary>
        /// ���� ���� ���� �� ������ �����ϱ� ���� �߰�.
        /// </summary>
        /// <param name="header"></param>
        /// <param name="schChoiceAdModel"></param>
        /// <returns></returns>
        [WebMethod]
        public SchChoiceAdModel SetSchChoiceAdCopy(HeaderModel header, SchChoiceAdModel schChoiceAdModel)
        {
            schChoiceAdBiz.SetSchChoiceAdCopy(header, schChoiceAdModel);
            return schChoiceAdModel;
        }


        /// <summary>
        /// ������ �����¸� Ȯ���ϱ� ���� �߰�.
        /// </summary>
        /// <param name="header"></param>
        /// <param name="schChoiceAdModel"></param>
        /// <returns></returns>
        [WebMethod]
        public SchChoiceAdModel CheckSchChoice(HeaderModel header, SchChoiceAdModel schChoiceAdModel)
        {
            schChoiceAdBiz.CheckSchChoice(header, schChoiceAdModel);
            return schChoiceAdModel;
        }
	}
}
