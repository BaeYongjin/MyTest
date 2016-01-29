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
    /// ******************************���α׷� ����*******************************
    /// ����			: Ukeyķ���νý��ۿ��� ķ���ε�Ͻ� ���ø�ID(IPTV_TMPLT_ID, _NM)������ ��������
    /// �������α׷�(P)	: 
    /// �������α׷�(C)	: 
    /// �������̺�		: 
    /// �������ν���	: 
    /// Ư�̻���		: 
    /// �ۼ���			: ��뼮
    /// �ۼ���			: 2011�� 04�� 05��
    /// ******************************���α׷� ����*******************************
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

		#region Web Form �����̳ʿ��� ������ �ڵ�
		override protected void OnInit(EventArgs e)
		{
			//
			// CODEGEN: �� ȣ���� ASP.NET Web Form �����̳ʿ� �ʿ��մϴ�.
			//
			InitializeComponent();
			base.OnInit(e);
		}
		
		/// <summary>
		/// �����̳� ������ �ʿ��� �޼����Դϴ�.
		/// �� �޼����� ������ �ڵ� ������� �������� ���ʽÿ�.
		/// </summary>
		private void InitializeComponent()
		{    

        }
		#endregion

        #region ����Ʈ ��� �κ�                 :: Fu_DataList()
        /// <summary>
        /// ȭ�鿡 ����Ʈ ������ ����Ѵ�.
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
					
                    TempCell1			= new HtmlTableCell();									//<TD>�׷��
                    TempCell1.Align		= "center";
                    TempCell1.Attributes.Add("class","BoardTD");
                    TempCell1.InnerHtml	= row["ItemNo"].ToString();

                    TempCell2			= new HtmlTableCell();									//<TD>���� ��
                    TempCell2.Align		= "left";
                    TempCell2.Attributes.Add("class","BoardTD");
                    TempCell2.InnerHtml	= "&nbsp;&nbsp;&nbsp;" + row["ItemName"].ToString();
                

                    TempCell3			= new HtmlTableCell();									//<TD>�̵�� �ڵ�
                    TempCell3.Align		= "center";
                    TempCell3.Attributes.Add("class","BoardTD");
                    TempCell3.InnerHtml	= "&nbsp;&nbsp;&nbsp;" + WinFramework.Misc.Utility.reConvertDate( row["ExcuteStartDay"].ToString());

                    TempCell4			= new HtmlTableCell();									//<TD>�̵�� ��
                    TempCell4.Align		= "center";
                    TempCell4.Attributes.Add("class","BoardTD");
                    TempCell4.InnerHtml	= "&nbsp;&nbsp;&nbsp;" + WinFramework.Misc.Utility.reConvertDate( row["RealEndDay"].ToString());

                    TempCell5			= new HtmlTableCell();									//<TD>����
                    TempCell5.Align		= "center";
                    TempCell5.Attributes.Add("class","BoardTD");
                    TempCell5.InnerHtml	= "&nbsp;&nbsp;&nbsp;" + row["AdType"].ToString();

                    TempCell6			= new HtmlTableCell();									//<TD>��ġ��
                    TempCell6.Align		= "center";
                    TempCell6.Attributes.Add("class","BoardTD");
                    TempCell6.InnerHtml	= "&nbsp;&nbsp;&nbsp;" + row["AdState"].ToString();

                    TempCell7			= new HtmlTableCell();									//<TD>����
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
                    TempCell1.InnerHtml = "������ �����ϴ�.";

                    TempRow.Controls.Add(TempCell1);
                    TableResult.Controls.Add(TempRow);
                }
                else
                {
                    // ������ ���� ���� 20�� ���� ��� ��ũ�� ���
                    if(Cnt > 20)
                    {
                        this.DivScroll.Attributes.Add("style","OVERFLOW-Y: scroll; OVERFLOW-X: hidden; WIDTH: 100%; HEIGHT: 386px");
                    }
                    else
                    {
                        this.DivScroll.Attributes.Add("style","");
                    }
                }
                //Total �����
                Fu_TableTotal(Cnt, TableTotal);
                ds.Dispose();
                ds = null;

            }
            catch(SystemException ex)
            {
                Response.Write("<script language=javascript>");
                Response.Write("alert('System Error�� �߻��Ͽ����ϴ�.\\n\\n");
                Response.Write("�Ʒ� �޼����� �����ڿ��� �����Ͽ��ֽʽÿ�.\\n\\n") ;
                Response.Write("�����޼���: "+ ex.Message.ToString().Replace("'","").Replace("\n","").Replace("\n","") +"');");
                Response.Write("</"+"script>");	
            }
            catch(Exception ex)
            {
                Response.Write("<script language=javascript>");
                Response.Write("alert('���� ������ �߻��Ͽ����ϴ�.\\n\\n");
                Response.Write("�Ʒ� �޼����� �����ڿ��� �����Ͽ��ֽʽÿ�.\\n\\n") ;
                Response.Write("�����޼���: "+ ex.Message.ToString().Replace("'","").Replace("\n","").Replace("\n","") +"');");
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
