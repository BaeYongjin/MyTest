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
	/// SlotOrganizationService�� ���� ��� �����Դϴ�.
	/// </summary>
	public class SlotOrganizationService : System.Web.Services.WebService
	{

		private SlotOrganizationBiz slotOrganizationBiz = null;

		public SlotOrganizationService()
		{
			//CODEGEN: �� ȣ���� ASP.NET �� ���� �����̳ʿ� �ʿ��մϴ�.
			InitializeComponent();

			slotOrganizationBiz = new SlotOrganizationBiz();
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
		public SlotOrganizationModel GetSlotList(HeaderModel header, SlotOrganizationModel slotOrganizationModel)
		{
			slotOrganizationBiz.GetSlotList(header, slotOrganizationModel);
			return slotOrganizationModel;
		}

		[WebMethod]
		public SlotOrganizationModel GetSlotCodeList(HeaderModel header, SlotOrganizationModel slotOrganizationModel)
		{
			slotOrganizationBiz.GetSlotCodeList(header, slotOrganizationModel);
			return slotOrganizationModel;
		}

		[WebMethod]
		public SlotOrganizationModel GetCategoryList(HeaderModel header, SlotOrganizationModel slotOrganizationModel)
		{
			slotOrganizationBiz.GetCategoryList(header, slotOrganizationModel);
			return slotOrganizationModel;
		}

		[WebMethod]
		public SlotOrganizationModel GetGenreList(HeaderModel header, SlotOrganizationModel slotOrganizationModel)
		{
			slotOrganizationBiz.GetGenreList(header, slotOrganizationModel);
			return slotOrganizationModel;
		}

		[WebMethod]
		public SlotOrganizationModel SetSlotUpdate(HeaderModel header, SlotOrganizationModel slotOrganizationModel)
		{
			slotOrganizationBiz.SetSlotUpdate(header, slotOrganizationModel);
			return slotOrganizationModel;
		}

		[WebMethod]
		public SlotOrganizationModel SetSlotCreate(HeaderModel header, SlotOrganizationModel slotOrganizationModel)
		{
			slotOrganizationBiz.SetSlotCreate(header, slotOrganizationModel);
			return slotOrganizationModel;
		}

		[WebMethod]
		public SlotOrganizationModel SetSlotDelete(HeaderModel header, SlotOrganizationModel slotOrganizationModel)
		{
			slotOrganizationBiz.SetSlotDelete(header, slotOrganizationModel);
			return slotOrganizationModel;
		}
	}
}
