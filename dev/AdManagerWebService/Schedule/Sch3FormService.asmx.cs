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
	public class Sch3FormService : System.Web.Services.WebService
	{
		private Sch3FormBiz	biz	= null;

		public Sch3FormService()
		{
			InitializeComponent();
			biz = new Sch3FormBiz();
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

		[WebMethod( Description="CSS���� ���Ǵ� �˾��������� ù��° ����Ʈ�� ī�װ�/�帣 ����� �����ɴϴ�")]
		public Sch3FormModel GetGenreListCSS(HeaderModel header, Sch3FormModel	model)
		{
			biz.GetGenreListCSS( header, model );
			return model;
		}

		[WebMethod( Description="�������� ���Ǵ� �˾��������� ù��° ����Ʈ�� ī�װ�/�帣 ����� �����ɴϴ�")]
		public Sch3FormModel GetGenreListDesign(HeaderModel header, Sch3FormModel	model)
		{
			biz.GetGenreListDesign( header, model );
			return model;
		}

		[WebMethod( Description="�Ϲ����� ���Ǵ� �˾��������� ù��° ����Ʈ�� ī�װ�/�帣 ����� �����ɴϴ�")]
		public Sch3FormModel GetGenreListTot(HeaderModel header, Sch3FormModel	model)
		{
			biz.GetGenreListTot( header, model );
			return model;
		}

		[WebMethod( Description="���� ���Ǵ� �˾��������� �ι�° ����Ʈ�� ä�� ����� �����ɴϴ�")]
		public Sch3FormModel GetChannelList(HeaderModel header, Sch3FormModel	model)
		{
			biz.GetChannelList( header, model );
			return model;
		}

		[WebMethod( Description="���� ���Ǵ� �˾��������� ����° ����Ʈ�� �ø���ä�� ����� �����ɴϴ�")]
		public Sch3FormModel GetSeriesList(HeaderModel header, Sch3FormModel	model)
		{
			biz.GetSeriesList( header, model );
			return model;
		}
	}
}
