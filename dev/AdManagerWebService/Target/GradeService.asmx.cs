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
	/// GradeService�� ���� ��� �����Դϴ�.
	/// </summary>
	public class GradeService : System.Web.Services.WebService
	{

		private GradeBiz gradeBiz = null;

		public GradeService()
		{
			//CODEGEN: �� ȣ���� ASP.NET �� ���� �����̳ʿ� �ʿ��մϴ�.
			InitializeComponent();

			gradeBiz = new GradeBiz();
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
		public GradeModel GetGradeCodeList(HeaderModel header, GradeModel gradeModel)
		{
			gradeBiz.GetGradeCodeList(header, gradeModel);
			return gradeModel;
		}		
		[WebMethod]
		public GradeModel GetGradeList(HeaderModel header, GradeModel gradeModel)
		{
			gradeBiz.GetGradeList(header, gradeModel);
			return gradeModel;
		}
		[WebMethod]
		public GradeModel GetContractItemList(HeaderModel header, GradeModel gradeModel)
		{
			gradeBiz.GetContractItemList(header, gradeModel);
			return gradeModel;
		}

		[WebMethod]
		public GradeModel SetGradeUpdate(HeaderModel header, GradeModel gradeModel)
		{
			gradeBiz.SetGradeUpdate(header, gradeModel);
			return gradeModel;
		}

		[WebMethod]
		public GradeModel SetGradeCreate(HeaderModel header, GradeModel gradeModel)
		{
			gradeBiz.SetGradeCreate(header, gradeModel);
			return gradeModel;
		}

		[WebMethod]
		public GradeModel SetGradeDelete(HeaderModel header, GradeModel gradeModel)
		{
			gradeBiz.SetGradeDelete(header, gradeModel);
			return gradeModel;
		}		
	}
}
