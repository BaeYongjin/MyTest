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
	/// Service1�� ���� ��� �����Դϴ�.
	/// </summary>
	public class CategoryService : System.Web.Services.WebService
	{
		private CategoryBiz categoryBiz = null;
		public CategoryService()
		{
			//CODEGEN: �� ȣ���� ASP.NET �� ���� �����̳ʿ� �ʿ��մϴ�.
			InitializeComponent();
			categoryBiz = new CategoryBiz();
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
		public CategoryModel GetCategoryList(HeaderModel header, CategoryModel categoryModel)
		{
			categoryBiz.GetCategoryList(header, categoryModel);
			return categoryModel;
		}

		[WebMethod]
		public CategoryModel SetCategoryUpdate(HeaderModel header, CategoryModel categoryModel)
		{
			categoryBiz.SetCategoryUpdate(header, categoryModel);
			return categoryModel;
		}

		[WebMethod]
		public CategoryModel SetCategoryCreate(HeaderModel header, CategoryModel categoryModel)
		{
			categoryBiz.SetCategoryCreate(header, categoryModel);
			return categoryModel;
		}

		[WebMethod]
		public CategoryModel SetCategoryDelete(HeaderModel header, CategoryModel categoryModel)
		{
			categoryBiz.SetCategoryDelete(header, categoryModel);
			return categoryModel;
		}
	}
}