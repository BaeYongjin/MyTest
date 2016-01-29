using System;
using System.Collections;
using System.ComponentModel;
using System.Data;

using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Text;

namespace AdManagerWebService.Interface
{
	/// <summary>
    /// ******************************프로그램 설명*******************************
    /// 설명			: Ukey캠페인시스템에서 캠페인등록시 템플릿ID(IPTV_TMPLT_ID, _NM)설정용 페이지임
    /// 관련프로그램(P)	: 
    /// 관련프로그램(C)	: 
    /// 관련테이블		: 
    /// 관련프로시져	: 
    /// 특이사항		: 
    /// 작성자			: 장용석
    /// 작성일			: 2011년 04월 05일
    /// ******************************프로그램 설명*******************************
	/// </summary>
	public partial class RELAY_FORM : Class.ClsComm
	{

        protected string Script = string.Empty;
    
		protected void Page_Load(object sender, System.EventArgs e)
		{
            if(!IsPostBack)
            {
                Fu_DataInit();
                Fu_DataList();

            }
		}

		#region Web Form 디자이너에서 생성한 코드
		override protected void OnInit(EventArgs e)
		{
			//
			// CODEGEN: 이 호출은 ASP.NET Web Form 디자이너에 필요합니다.
			//
			InitializeComponent();
			base.OnInit(e);
		}
		
		/// <summary>
		/// 디자이너 지원에 필요한 메서드입니다.
		/// 이 메서드의 내용을 코드 편집기로 수정하지 마십시오.
		/// </summary>
		private void InitializeComponent()
		{    

        }
		#endregion

        #region 리스트 출력 부분                 :: Fu_DataList()
        /// <summary>
        /// 화면에 리스트 내용을 출력한다.
        /// </summary>
        public void Fu_DataList()
        {
            HtmlTableRow	TempRow;
            HtmlTableCell	TempCell1;
            HtmlTableCell	TempCell2;
            HtmlTableCell	TempCell3;
            HtmlTableCell	TempCell4;
            HtmlTableCell	TempCell5;
            HtmlTableCell	TempCell6;
            HtmlTableCell	TempCell7;

            try
            {
                ItemList2UkeyBiz biz = new ItemList2UkeyBiz();
                DataSet          ds  = new DataSet();

                ds = biz.GetDataList();
                //string  yyyymmdd;
                //string  yyyy;
                //string  mm;
                //string  dd;

                int Cnt = 0;

                

                foreach( DataRow row in ds.Tables[0].Rows )
                {
                    TempRow			= new HtmlTableRow();										//<TR>
                    TempRow.Height	= "24";
                    TempRow.VAlign  = "center";

                    TempRow.Attributes.Add("onmouseover","this.style.backgroundColor='#ECECEC';");
                    TempRow.Attributes.Add("onmouseout","this.style.backgroundColor='#FFFFFF';");
                    TempRow.Attributes.Add("style","CURSOR:hand;");
					
                    TempRow.Attributes.Add("onclick","return JS_Select('" + row["ItemNo"].ToString() + "','" + row["ItemName"].ToString() + "');");
					
                    TempCell1			= new HtmlTableCell();									//<TD>그룹명
                    TempCell1.Align		= "center";
                    TempCell1.Attributes.Add("class","BoardTD");
                    TempCell1.InnerHtml	= row["ItemNo"].ToString();

                    TempCell2			= new HtmlTableCell();									//<TD>매장 명
                    TempCell2.Align		= "left";
                    TempCell2.Attributes.Add("class","BoardTD");
                    TempCell2.InnerHtml	= "&nbsp;&nbsp;&nbsp;" + row["ItemName"].ToString();
                

                    TempCell3			= new HtmlTableCell();									//<TD>미디어 코드
                    TempCell3.Align		= "center";
                    TempCell3.Attributes.Add("class","BoardTD");
                    TempCell3.InnerHtml	= "&nbsp;&nbsp;&nbsp;" + WinFramework.Misc.Utility.reConvertDate( row["ExcuteStartDay"].ToString());

                    TempCell4			= new HtmlTableCell();									//<TD>미디어 명
                    TempCell4.Align		= "center";
                    TempCell4.Attributes.Add("class","BoardTD");
                    TempCell4.InnerHtml	= "&nbsp;&nbsp;&nbsp;" + WinFramework.Misc.Utility.reConvertDate( row["RealEndDay"].ToString());

                    TempCell5			= new HtmlTableCell();									//<TD>고객명
                    TempCell5.Align		= "center";
                    TempCell5.Attributes.Add("class","BoardTD");
                    TempCell5.InnerHtml	= "&nbsp;&nbsp;&nbsp;" + row["AdType"].ToString();

                    TempCell6			= new HtmlTableCell();									//<TD>설치일
                    TempCell6.Align		= "center";
                    TempCell6.Attributes.Add("class","BoardTD");
                    TempCell6.InnerHtml	= "&nbsp;&nbsp;&nbsp;" + row["AdState"].ToString();

                    TempCell7			= new HtmlTableCell();									//<TD>상태
                    TempCell7.Align		= "left";
                    TempCell7.Attributes.Add("class","BoardTD");
                    TempCell7.InnerHtml	= "&nbsp;&nbsp;&nbsp;" + row["FileState"].ToString();

                    TempRow.Controls.Add(TempCell1);
                    TempRow.Controls.Add(TempCell2);
                    TempRow.Controls.Add(TempCell3);
                    TempRow.Controls.Add(TempCell4);
                    TempRow.Controls.Add(TempCell5);
                    TempRow.Controls.Add(TempCell6);
                    TempRow.Controls.Add(TempCell7);
                    TableResult.Controls.Add(TempRow);

                    Cnt += 1;

                }
                if(Cnt == 0)
                {
                    TempRow			= new HtmlTableRow();		// </TR>
                    TempRow.Height	= "24";

                    TempCell1			= new HtmlTableCell();		
                    TempCell1.ColSpan	= 7;
                    TempCell1.Align		= "Center";
                    TempCell1.Attributes.Add("class","BoardTD");
                    TempCell1.InnerHtml = "내용이 없습니다.";

                    TempRow.Controls.Add(TempCell1);
                    TableResult.Controls.Add(TempRow);
                }
                else
                {
                    // 데이터 행의 수가 20을 넘을 경우 스크롤 출력
                    if(Cnt > 20)
                    {
                        this.DivScroll.Attributes.Add("style","OVERFLOW-Y: scroll; OVERFLOW-X: hidden; WIDTH: 100%; HEIGHT: 386px");
                    }
                    else
                    {
                        this.DivScroll.Attributes.Add("style","");
                    }
                }
                //Total 결과수
                Fu_TableTotal(Cnt, TableTotal);
                ds.Dispose();
                ds = null;

            }
            catch(SystemException ex)
            {
                Response.Write("<script language=javascript>");
                Response.Write("alert('System Error가 발생하였습니다.\\n\\n");
                Response.Write("아래 메세지를 관리자에게 문의하여주십시요.\\n\\n") ;
                Response.Write("에러메세지: "+ ex.Message.ToString().Replace("'","").Replace("\n","").Replace("\n","") +"');");
                Response.Write("</"+"script>");	
            }
            catch(Exception ex)
            {
                Response.Write("<script language=javascript>");
                Response.Write("alert('구문 에러가 발생하였습니다.\\n\\n");
                Response.Write("아래 메세지를 관리자에게 문의하여주십시요.\\n\\n") ;
                Response.Write("에러메세지: "+ ex.Message.ToString().Replace("'","").Replace("\n","").Replace("\n","") +"');");
                Response.Write("</"+"script>");	
            }
            finally
            {
                //CloseDB();
            }
        }
        #endregion
	}
}
