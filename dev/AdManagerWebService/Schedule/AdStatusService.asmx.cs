/*
 * -------------------------------------------------------
 * [��������]
 * -------------------------------------------------------
 * �����ڵ�  : [E_01]
 * ������    : �躸��
 * ������    : 2013.06.04
 * ��������  : 
 *            - GetAdStatusList_Comp() �Լ� ����
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

namespace AdManagerWebService.Schedule
{
    /// <summary>
    /// �⺻���ӽ����̽��� �����Ѵ�.
    /// </summary>
    [WebService(Namespace = "http://advmgt.hanafostv.com/AdManagerWebService/")]
    [System.ComponentModel.ToolboxItem(false)]

	/// <summary>
	/// AdStatusService�� ���� ��� �����Դϴ�.
	/// </summary>
	public class AdStatusService : System.Web.Services.WebService
	{
		private AdStatusBiz adStatusBiz = null;
		public AdStatusService()
		{
			//CODEGEN: �� ȣ���� ASP.NET �� ���� �����̳ʿ� �ʿ��մϴ�.
			InitializeComponent();
			adStatusBiz = new AdStatusBiz();
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
		public AdStatusModel GetAdStatusList(HeaderModel header, AdStatusModel adStatusModel)
		{
			adStatusBiz.GetAdStatusList(header, adStatusModel);
			return adStatusModel;
		}

        [WebMethod]
        public AdStatusModel GetAdStatusList_Comp(HeaderModel header, AdStatusModel adStatusModel)
        {
            adStatusBiz.GetAdStatusList_Compress(header, adStatusModel);
            return adStatusModel;
        }

	}
}