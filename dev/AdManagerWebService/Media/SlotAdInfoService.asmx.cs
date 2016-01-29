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
	/// SlotAdInfoService�� ���� ��� �����Դϴ�.
	/// </summary>
	public class SlotAdInfoService : System.Web.Services.WebService
	{

		private SlotAdInfoBiz slotAdInfoBiz = null;

		public SlotAdInfoService()
		{
			//CODEGEN: �� ȣ���� ASP.NET �� ���� �����̳ʿ� �ʿ��մϴ�.
			InitializeComponent();

			slotAdInfoBiz = new SlotAdInfoBiz();
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

        #region ������ ���� ���

        [WebMethod]
		public SlotAdInfoModel GetMenuList(HeaderModel header, SlotAdInfoModel slotAdInfoModel)
		{
			slotAdInfoBiz.GetMenuList(header, slotAdInfoModel);
			return slotAdInfoModel;
		}
        
        #endregion

        #region ������ ���� ����
        
        [WebMethod]
        public SlotAdInfoModel UpdateSlotAdTypeAssign(HeaderModel header, SlotAdInfoModel slotAdInfoModel)
		{
            slotAdInfoBiz.UpdateSlotAdTypeAssign(header, slotAdInfoModel);
			return slotAdInfoModel;
		}

        #endregion

        #region ������ ���� �߰�

        [WebMethod]
        public SlotAdInfoModel InsertSlotAdTypeAssign(HeaderModel header, SlotAdInfoModel slotAdInfoModel)
        {
            slotAdInfoBiz.InsertSlotAdTypeAssign(header, slotAdInfoModel);
            return slotAdInfoModel;
        }

        #endregion

        #region ������ ���� ����

        [WebMethod]
        public SlotAdInfoModel DeleteSlotAdTypeAssign(HeaderModel header, SlotAdInfoModel slotAdInfoModel)
        {
            slotAdInfoBiz.DeleteSlotAdTypeAssign(header, slotAdInfoModel);
            return slotAdInfoModel;
        }

        #endregion

        #region ������ ���� �⺻�� ��ȸ

        [WebMethod]
        public SlotAdInfoModel GetDefaultSlotAdInfo(HeaderModel header, SlotAdInfoModel slotAdInfoModel)
        {
            slotAdInfoBiz.GetDefaultSlotAdInfo(header, slotAdInfoModel);
            return slotAdInfoModel;
        }

        #endregion

    }
}
