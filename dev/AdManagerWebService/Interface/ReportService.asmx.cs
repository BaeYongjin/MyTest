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
    /// 기본네임스페이스를 설정한다.
    /// </summary>
    [WebService(Name="레포팅 웹서비스",Namespace="http://advmgt.hanafostv.com/AdManagerWebService/",Description="브로드앤TV 유관업체용 레포팅 데이터 웹서비스 입니다.")]

	/// <summary>
	/// ReportService에 대한 요약 설명입니다.
	/// </summary>
	public class ReportService : System.Web.Services.WebService
	{
		public ReportService()
		{
			//CODEGEN: 이 호출은 ASP.NET 웹 서비스 디자이너에 필요합니다.
			InitializeComponent();
		}

		#region 구성 요소 디자이너에서 생성한 코드
		
		//웹 서비스 디자이너에 필요합니다. 
		private IContainer components = null;
				
		/// <summary>
		/// 디자이너 지원에 필요한 메서드입니다.
		/// 이 메서드의 내용을 코드 편집기로 수정하지 마십시오.
		/// </summary>
		private void InitializeComponent()
		{
		}

		/// <summary>
		/// 사용 중인 모든 리소스를 정리합니다.
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


        [WebMethod(Description="테스트입니다")]
        public string HelloWorld()
        {
            return DateTime.Now.ToString();
        }

        [WebMethod(Description="테스트입니다2")]
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

        [WebMethod(Description="프로그램별 광고시청 집계")]
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
						returnData.Message		=	"정상";
						returnData.ResultSet	=	resultSet;
					}
					else
					{
						returnData.SUCCESS		=	true;
						returnData.ResultCnt	=	0;
						returnData.Message		=	"결과 데이터가 없습니다";
						returnData.ResultSet	=	null;
					}
				}
				else
				{
					returnData.SUCCESS		=	false;
					returnData.ResultCnt	=	0;
					returnData.Message		=	"DB처리중 오류가 발생하였습니다. 담당자에게 문의 하십시요";
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
