/*
 * -------------------------------------------------------
 * Class Name: TargetingService
 * �ֿ���  : Targeting WebService �Լ�����
 * �ۼ���    : ��
 * �ۼ���    : ��
 * Ư�̻���  : 
 * -------------------------------------------------------
 * [��������]
 * -------------------------------------------------------
 * �����ڵ�  : [E_01]
 * ������    : bae
 * ������    : 2010.10.04
 * �����κ�  :
 *			  - GetTargetingDetail(..) ���� ȣ�� �Լ� ����
 *            - SetTargetingDetailUpdate(..) ���� ȣ�� �Լ� ����
 * ��������  : 
 *            - 2Slot ó������ ���� method ȣ�� ����
 *            - 
 * --------------------------------------------------------
 * 2012.02.16 ����Ÿ���� �߰� RH.Jung
 * -------------------------------------------------------
 * �����ڵ�  : [E_03]
 * ������    : �躸��
 * ������    : 2013.04.01
 * �����κ�  :
 *			  - GetStbList() ���� ȣ�� �Լ� �߰�
 * ��������  : 
 *            - ��ž�����ȸ�� ����ϴ� �Լ� �߰�
 * --------------------------------------------------------
 * �����ڵ�  : [E_04]
 * ������    : �躸��
 * ������    : 2013.10.16
 * �����κ�  :
 *			  - SetTargetingProfileAdd() ���� ȣ�� �Լ� �߰�
 * ��������  : 
 *            - �������� Ÿ���� ���� �Լ� �߰�
 * --------------------------------------------------------
 */

using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Web;
using System.Web.Services;

using WinFramework.Base;
using AdManagerModel;

namespace AdManagerWebService.Target
{
    /// <summary>
    /// �⺻���ӽ����̽��� �����Ѵ�.
    /// </summary>
    [WebService(Namespace = "http://advmgt.hanafostv.com/AdManagerWebService/")]
    [System.ComponentModel.ToolboxItem(false)]

	/// <summary>
	/// TargetingService�� ���� ��� �����Դϴ�.
	/// </summary>
	public class TargetingService : System.Web.Services.WebService
	{
		private TargetingBiz targetingBiz = null;

		public TargetingService()
		{
			//CODEGEN: �� ȣ���� ASP.NET �� ���� �����̳ʿ� �ʿ��մϴ�.
			InitializeComponent();

			targetingBiz = new TargetingBiz();
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
		public TargetingModel GetTargetingList(HeaderModel header, TargetingModel targetingModel)
		{
			targetingBiz.GetTargetingList(header, targetingModel);
			return targetingModel;			
		}

		[WebMethod]
		public TargetingModel GetCollectionList(HeaderModel header, TargetingModel targetingModel)
		{
            targetingBiz.GetCollectionList(header, targetingModel);
			return targetingModel;
		}

		[WebMethod]
		public TargetingModel GetTargetingDetail(HeaderModel header, TargetingModel targetingModel)
		{
			//[E_01] ���� ȣ��
			//targetingBiz.GetTargetingDetail(header, targetingModel);
			//return targetingModel;

			//[E_01] 2SLOT
			targetingBiz.GetTargetingDetail_10_04(header, targetingModel);
			return targetingModel;
		}

        //GetTargetingDetail2

        [WebMethod]
        public TargetingModel GetTargetingDetail2(HeaderModel header, TargetingModel targetingModel)
        {
            targetingBiz.GetTargetingDetail2(header, targetingModel);
            return targetingModel;
        }

		[WebMethod]
		public TargetingModel GetTargetingRate(HeaderModel header, TargetingModel targetingModel)
		{
			targetingBiz.GetTargetingRate(header, targetingModel);
			return targetingModel;
		}

        [WebMethod]
        public TargetingModel SetTargetingDetailUpdate(HeaderModel header, TargetingModel targetingModel)
        {
			//[E_01] ���� ȣ��
            //targetingBiz.SetTargetingDetailUpdate(header, targetingModel);
            //return targetingModel;

			//[E_01] 2SLOT
			targetingBiz.SetTargetingDetailUpdate_10_04(header, targetingModel);
			return targetingModel;
        }

		[WebMethod]
		public TargetingModel SetTargetingRateUpdate(HeaderModel header, TargetingModel targetingModel)
		{
			
			targetingBiz.SetTargetingRateUpdate(header, targetingModel);
			return targetingModel;
			
		}

		[WebMethod]
		public TargetingModel GetRegionList(HeaderModel header, TargetingModel targetingModel)
		{
			targetingBiz.GetRegionList(header, targetingModel);
			return targetingModel;
		}      

		[WebMethod]
		public TargetingModel GetAgeList(HeaderModel header, TargetingModel targetingModel)
		{
			targetingBiz.GetAgeList(header, targetingModel);
			return targetingModel;
		}

        // ����Ÿ���� ���� �߰� 2012.02.14 RH.Jung 
        [WebMethod]
        public TargetingModel GetTargetingCollectionList(HeaderModel header, TargetingModel targetingModel)
        {
            targetingBiz.GetTargetingCollectionList(header, targetingModel);
            return targetingModel;
        }

        [WebMethod]
        public TargetingModel SetTargetingCollectionAdd(HeaderModel header, TargetingModel targetingModel)
        {
            targetingBiz.SetTargetingCollectionAdd(header, targetingModel);
            return targetingModel;
        }

        [WebMethod]
        public TargetingModel SetTargetingCollectionDelete(HeaderModel header, TargetingModel targetingModel)
        {
            targetingBiz.SetTargetingCollectionDelete(header, targetingModel);
            return targetingModel;
        }

        [WebMethod]
        public TargetingModel GetStbList(HeaderModel header, TargetingModel targetingModel)
        {
            targetingBiz.GetStbList(header, targetingModel);
            return targetingModel;
        }

        // [E_04]
        [WebMethod]
        public TargetingModel SetTargetingProfileAdd(HeaderModel header, TargetingModel targetingModel)
        {
            targetingBiz.SetTargetingProfileAdd(header, targetingModel);
            return targetingModel;
        }

	}
}
