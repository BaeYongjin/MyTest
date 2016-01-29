using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Web;
using System.Web.Services;

using WinFramework.Base;
using AdManagerModel;

namespace AdManagerWebService.Common
{

	/// <summary>
	/// �⺻���ӽ����̽��� �����Ѵ�.
	/// </summary>
	[WebService(Namespace="http://advmgt.hanafostv.com/AdManagerWebService/")]
    [System.ComponentModel.ToolboxItem(false)]

	/// <summary>
	/// MediaRapCodeService�� ���� ��� �����Դϴ�.
	/// </summary>
	public class MediaRapCodeService : System.Web.Services.WebService
	{
		private MediaRapCodeBiz mediarapcodeBiz = null;

		public MediaRapCodeService()
		{
			//CODEGEN: �� ȣ���� ASP.NET �� ���� �����̳ʿ� �ʿ��մϴ�.
			InitializeComponent();

			mediarapcodeBiz = new MediaRapCodeBiz();
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
		public MediaRapCodeModel GetMediaRapCodeList(HeaderModel header, MediaRapCodeModel mediarapcodeModel)
		{
			mediarapcodeBiz.GetMediaRapCodeList(header, mediarapcodeModel);
			return mediarapcodeModel;
		}

		[WebMethod]
		public MediaRapCodeModel GetMediaRapCodeListIdCheck(HeaderModel header, MediaRapCodeModel mediarapcodeModel)
		{
			mediarapcodeBiz.GetMediaRapCodeListIdCheck(header, mediarapcodeModel);
			return mediarapcodeModel;
		}

	}
}
