using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Web;
using System.Web.Services;

namespace AdManagerWebService.Interface
{
    /// <summary>
    /// �⺻���ӽ����̽��� �����Ѵ�.
    /// </summary>
    [WebService(Name="������ ������",Namespace="http://advmgt.hanafostv.com/AdManagerWebService/",Description="��ε��TV ������ü�� ������ ������ ������ �Դϴ�.")]

	/// <summary>
	/// ReportService�� ���� ��� �����Դϴ�.
	/// </summary>
	public class ReportService : System.Web.Services.WebService
	{
		public ReportService()
		{
			//CODEGEN: �� ȣ���� ASP.NET �� ���� �����̳ʿ� �ʿ��մϴ�.
			InitializeComponent();
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


        [WebMethod(Description="�׽�Ʈ�Դϴ�")]
        public string HelloWorld()
        {
            return DateTime.Now.ToString();
        }

        [WebMethod(Description="�׽�Ʈ�Դϴ�2")]
        public TestData[] HelloWorldCC()
        {
            TestData[] Test = new TestData[3];

         
            
            TestData    a = new TestData();
            a.Num = 1;
            a.STR = "A";
            Test[0] = a;

            TestData    b = new TestData();
            a.Num = 2;
            a.STR = "B";
            Test[1] = b;

            TestData    c = new TestData();
            a.Num = 3;
            a.STR = "C";
            Test[2] = c;

            return Test;
        }

        [WebMethod(Description="���α׷��� �����û ����")]
        public ResultViewCntAdverEachProgram PeriodViewCntAdver_EachProgram()
        {
            ReportBiz   biz = new ReportBiz();
            DataSet     ds  = new DataSet();
            ViewCntAdverEachProgram[]   resultSet		= null;
			ResultViewCntAdverEachProgram	returnData	= new ResultViewCntAdverEachProgram();

			try
			{
				ds = biz.PeriodViewCntAdver_EachProgram(396,0,14504,"090706","090706");
			
				if ( ds != null && ds.Tables.Count > 0 )
				{
					int rowCnt = ds.Tables[0].Rows.Count;
					resultSet = new ViewCntAdverEachProgram[rowCnt];

					int     CategoryCode;
					string  CategoryName;
					int     GenreCode;
					string  GenreName;
					int     ChannelNo;
					string  ProgramNm;
					long    AdHitCnt;
					long    PgHitCnt;
					int     idx = 0;

					if( ds.Tables[0].Rows.Count > 0 )
					{
						foreach( DataRow row in ds.Tables[0].Rows )
						{
							CategoryCode =   Convert.ToInt32(row["CategoryCode"].ToString());
							CategoryName =   row["CategoryName"].ToString();
							GenreCode    =   Convert.ToInt32(row["GenreCode"].ToString());
							GenreName    =   row["GenreName"].ToString();
							ChannelNo    =   Convert.ToInt32(row["ChannelNo"].ToString());
							ProgramNm    =   row["ProgramNm"].ToString();
							AdHitCnt     =   Convert.ToInt64(row["AdHitCnt"].ToString());
							PgHitCnt     =   Convert.ToInt64(row["PgHitCnt"].ToString());
							resultSet[idx] = new ViewCntAdverEachProgram(CategoryCode, CategoryName, GenreCode, GenreName, ChannelNo, ProgramNm, AdHitCnt, PgHitCnt);
							idx++;
						}

						returnData.SUCCESS		=	true;
						returnData.ResultCnt	=	rowCnt;
						returnData.Message		=	"����";
						returnData.ResultSet	=	resultSet;
					}
					else
					{
						returnData.SUCCESS		=	true;
						returnData.ResultCnt	=	0;
						returnData.Message		=	"��� �����Ͱ� �����ϴ�";
						returnData.ResultSet	=	null;
					}
				}
				else
				{
					returnData.SUCCESS		=	false;
					returnData.ResultCnt	=	0;
					returnData.Message		=	"DBó���� ������ �߻��Ͽ����ϴ�. ����ڿ��� ���� �Ͻʽÿ�";
					returnData.ResultSet	=	null;
				}
			}
			catch(Exception ex)
			{
				returnData.SUCCESS		=	false;
				returnData.ResultCnt	=	0;
				returnData.Message		=	ex.Message;
				returnData.ResultSet	=	null;
			}
            return returnData;
        }
	}
}
