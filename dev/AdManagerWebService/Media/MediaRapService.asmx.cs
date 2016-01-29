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
	/// MediaRapService�� ���� ��� �����Դϴ�.
	/// </summary>
	public class MediaRapService : System.Web.Services.WebService
	{

		private MediaRapBiz mediarapBiz = null;

		public MediaRapService()
		{
			//CODEGEN: �� ȣ���� ASP.NET �� ���� �����̳ʿ� �ʿ��մϴ�.
			InitializeComponent();

			mediarapBiz = new MediaRapBiz();
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
		public MediaRapModel GetMediaRapList(HeaderModel header, MediaRapModel mediarapModel)
		{
			mediarapBiz.GetMediaRapList(header, mediarapModel);
			return mediarapModel;
		}

		[WebMethod]
		public MediaRapModel SetMediaRapUpdate(HeaderModel header, MediaRapModel mediarapModel)
		{
			mediarapBiz.SetMediaRapUpdate(header, mediarapModel);
			return mediarapModel;
		}

		[WebMethod]
		public MediaRapModel SetMediaRapCreate(HeaderModel header, MediaRapModel mediarapModel)
		{
			mediarapBiz.SetMediaRapCreate(header, mediarapModel);
			return mediarapModel;
		}

		[WebMethod]
		public MediaRapModel SetMediaRapDelete(HeaderModel header, MediaRapModel mediarapModel)
		{
			mediarapBiz.SetMediaRapDelete(header, mediarapModel);
			return mediarapModel;
		}
	}
}