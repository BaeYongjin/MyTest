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
	/// CodeService�� ���� ��� �����Դϴ�.
	/// </summary>
	public class CodeService : System.Web.Services.WebService
	{
		private CodeBiz codeBiz = null;

		public CodeService()
		{
			//CODEGEN: �� ȣ���� ASP.NET �� ���� �����̳ʿ� �ʿ��մϴ�.
			InitializeComponent();

			codeBiz = new CodeBiz();
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
		public CodeModel GetSectionList(HeaderModel header, CodeModel codeModel)
		{
			codeBiz.GetSectionList(header, codeModel);
			return codeModel;
		}

		[WebMethod]
		public CodeModel GetCodeList(HeaderModel header, CodeModel codeModel)
		{
			codeBiz.GetCodeList(header, codeModel);
			return codeModel;
		}

		[WebMethod]
		public CodeModel SetSectionUpdate(HeaderModel header, CodeModel codeModel)
		{
			codeBiz.SetSectionUpdate(header, codeModel);
			return codeModel;
		}

		[WebMethod]
		public CodeModel SetCodeUpdate(HeaderModel header, CodeModel codeModel)
		{
			codeBiz.SetCodeUpdate(header, codeModel);
			return codeModel;
		}

		[WebMethod]
		public CodeModel SetCodeCreate(HeaderModel header, CodeModel codeModel)
		{
			codeBiz.SetCodeCreate(header, codeModel);
			return codeModel;
		}

		[WebMethod]
		public CodeModel SetCodeDelete(HeaderModel header, CodeModel codeModel)
		{
			codeBiz.SetCodeDelete(header, codeModel);
			return codeModel;
		}

	}
}
