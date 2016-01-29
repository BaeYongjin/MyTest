using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Security.Cryptography;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Web.UI;

namespace AdManagerWebService.Interface.Class
{
	/// <summary>
	/// ******************************프로그램 설명*******************************
	/// 설명			: 공통모듈 정의 Class
	/// 관련프로그램(P)	: ALL
	/// 관련프로그램(C)	: ALL
	/// 관련테이블		: 
	/// 관련프로시져	: 
	/// 특이사항		: 데이터베이스 연결설정및 공통모듈
	/// 작성자			: 오정훈
	/// 작성일			: 2004년 11월 09일
	/// ******************************프로그램 설명*******************************
	/// </summary>
	public class ClsComm : System.Web.UI.Page
	{
		/// <summary>
		/// DB연결 설정
		/// </summary>
		//Connection 객체 정의
		protected bool			SysAdmin	= false;
		protected string		adminSql	= "";
		protected SqlConnection sqlConnection = null;
		private string strConnection = string.Empty;

		public ClsComm()
		{
			strConnection = "data source=218.145.47.33;initial catalog=iFrame;password=iframe_12#4;persist security info=True;user id=iframeadmin;packet size=4096;max pool size=10;min pool size=5";
			
		}

		#region DB연결 설정
		protected void ConnDB()
		{
			try
			{
				sqlConnection = new SqlConnection(strConnection);
				if(sqlConnection.State == ConnectionState.Closed)
				{
					sqlConnection.Open();
				}
			}
			catch
			{
			}

		}

		protected void CloseDB()
		{
			if( sqlConnection != null)
			{
				if(sqlConnection.State == ConnectionState.Open)
					sqlConnection.Close();
			}
		}
		#endregion

		/// <summary>
		/// DropDownList세팅을 위한 정의 및 함수 모듈
		/// SetValue Function overloading
		/// </summary>
		#region Dropdownlist세팅에 관련된 정의 및 함수
		public struct strSetting
		{
			private string strPItem1, strPItem2;
			public strSetting(string strRtnItem1, string strRtnItem2) 
			{
				strPItem1 = strRtnItem1;
				strPItem2 = strRtnItem2;
			}
			public string strItem1 { get { return strPItem1; } }
			public string strItem2 { get { return strPItem2; } }
		}


		public void SetiFrameDDL(DropDownList DropList)
		{
			StringBuilder strSql = new StringBuilder();
			strSql.Append(" Select ServiceName SELECT_NM, ServiceCode SELECT_CD From Tbl_iFrame");
			if( this.SysAdmin == false)
			{
				strSql.Append(" where  ServiceCode = ( select admin_group from tbl_admin where admin_id = '" + Session["Admin_ID"] + "') ");
			}
			strSql.Append(" Order by ServiceCode ");

			ConnDB();
			SqlCommand sqlCommand = new SqlCommand();
			sqlCommand.Connection = sqlConnection;
			sqlCommand.CommandText = strSql.ToString();
			SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();

			//ListItem SItem = new ListItem("선택", "");
			//DropList.Items.Insert(0,SItem);
			int idx =0;
			while(sqlDataReader.Read())
			{
				ListItem DItem = new ListItem(sqlDataReader["SELECT_NM"].ToString(), sqlDataReader["SELECT_CD"].ToString());
				DropList.Items.Insert(idx,DItem);
				idx += 1;
			}
			sqlDataReader.Close();
			CloseDB();
		}

		/// <summary>
		/// 컨텐츠코드의 최상위코드인 상위업종 드룹다운생성
		/// </summary>
		/// <param name="DropList"></param>
		public void SetHighCataDDL(DropDownList DropList)
		{
			StringBuilder strSql = new StringBuilder();
			strSql.Append(" Select HighCata_Name SELECT_NM, HighCata_Id SELECT_CD From Tbl_HighCata");
			if( this.SysAdmin == false)
			{
				strSql.Append(" where  group_cd = ( select admin_group from tbl_admin where admin_id = '" + Session["Admin_ID"] + "') ");
			}
			strSql.Append(" Order by HighCata_Id ");

			ConnDB();
			SqlCommand sqlCommand = new SqlCommand();
			sqlCommand.Connection = sqlConnection;
			sqlCommand.CommandText = strSql.ToString();
			SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();

			//ListItem SItem = new ListItem("선택", "");
			//DropList.Items.Insert(0,SItem);
			int idx =0;
			while(sqlDataReader.Read())
			{
				ListItem DItem = new ListItem(sqlDataReader["SELECT_NM"].ToString(), sqlDataReader["SELECT_CD"].ToString());
				DropList.Items.Insert(idx,DItem);
				idx += 1;
			}
			sqlDataReader.Close();
			CloseDB();
		}

		public void SetLowCataDDL(DropDownList DropList, string High)
		{
			StringBuilder strSql = new StringBuilder();
			strSql.Append(" SELECT LOWCATA_ID Select_Cd, LOWCATA_NAME Select_Nm FROM TBL_LOWCATA Where HighCata_Id = '" + High + "' Order by LowCata_Id");
				
			ConnDB();
			SqlCommand sqlCommand = new SqlCommand();
			sqlCommand.Connection = sqlConnection;
			sqlCommand.CommandText = strSql.ToString();
			SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();

			//ListItem SItem = new ListItem("선택", "");
			//DropList.Items.Insert(0,SItem);
			DropList.Items.Clear();
			int idx =0;
			while(sqlDataReader.Read())
			{
				ListItem DItem = new ListItem(sqlDataReader["SELECT_NM"].ToString(), sqlDataReader["SELECT_CD"].ToString());
				DropList.Items.Insert(idx,DItem);
				idx += 1;
			}
			sqlDataReader.Close();
			CloseDB();
		}



		/// <summary>
		/// 그룹 드룹다운리스트를 생성한다.
		/// </summary>
		/// <param name="DropList"></param>
		public void SetGroupDDL(DropDownList DropList)
		{
			StringBuilder strSql = new StringBuilder();
			strSql.Append("SELECT GROUP_NM SELECT_NM, GROUP_CD SELECT_CD FROM TBL_GROUP");
			if( this.SysAdmin == false)
			{
				strSql.Append(" where " + adminSql);
			}
			strSql.Append(" ORDER BY GROUP_CD ");

			ConnDB();
			SqlCommand sqlCommand = new SqlCommand();
			sqlCommand.Connection = sqlConnection;
			sqlCommand.CommandText = strSql.ToString();
			SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();

			//ListItem SItem = new ListItem("선택", "");
			//DropList.Items.Insert(0,SItem);
			int idx =0;
			while(sqlDataReader.Read())
			{
				ListItem DItem = new ListItem(sqlDataReader["SELECT_NM"].ToString(), sqlDataReader["SELECT_CD"].ToString());
				DropList.Items.Insert(idx,DItem);
				idx += 1;
			}
			sqlDataReader.Close();
			CloseDB();
		}


		public void SetShopDDL(DropDownList DropList,string Group)
		{
			StringBuilder strSql = new StringBuilder();
			strSql.Append("SELECT SHOP_CD Select_Cd, SHOP_NM Select_Nm FROM TBL_SHOP Where Group_Cd = '" + Group + "' ORDER BY 1");

			ConnDB();
			SqlCommand sqlCommand = new SqlCommand();
			sqlCommand.Connection = sqlConnection;
			sqlCommand.CommandText = strSql.ToString();
			SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();

			DropList.Items.Clear();
			ListItem SItem = new ListItem("[전체]", "0000");
			DropList.Items.Insert(0,SItem);
			int idx =1;
			while(sqlDataReader.Read())
			{
				ListItem DItem = new ListItem(sqlDataReader["SELECT_NM"].ToString(), sqlDataReader["SELECT_CD"].ToString());
				DropList.Items.Insert(idx,DItem);
				idx += 1;
			}
			sqlDataReader.Close();
			CloseDB();
		}

		public void SetMediaDDL(DropDownList DropList,string Group,string Shop)
		{
			StringBuilder strSql = new StringBuilder();
			strSql.Append("SELECT Media_CD Select_Cd, Media_NM Select_Nm FROM TBL_Media Where Group_Cd = '" + Group + "' and Shop_Cd = '" + Shop + "' ORDER BY 1");

			ConnDB();
			SqlCommand sqlCommand = new SqlCommand();
			sqlCommand.Connection = sqlConnection;
			sqlCommand.CommandText = strSql.ToString();
			SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();

			DropList.Items.Clear();
			int idx =0;
			while(sqlDataReader.Read())
			{
				ListItem DItem = new ListItem(sqlDataReader["SELECT_NM"].ToString(), sqlDataReader["SELECT_CD"].ToString());
				DropList.Items.Insert(idx,DItem);
				idx += 1;
			}
			sqlDataReader.Close();
			CloseDB();
		}

		public void SetValue(StringBuilder strSql, DropDownList DropList)
		{
			ConnDB();
			SqlCommand sqlCommand = new SqlCommand();
			sqlCommand.Connection = sqlConnection;
			sqlCommand.CommandText = strSql.ToString();
			SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();

			//ListItem SItem = new ListItem("선택", "");
			//DropList.Items.Insert(0,SItem);
			int idx =0;
			while(sqlDataReader.Read())
			{
				ListItem DItem = new ListItem(sqlDataReader["SELECT_NM"].ToString(), sqlDataReader["SELECT_CD"].ToString());
				DropList.Items.Insert(idx,DItem);
				idx += 1;
			}
			sqlDataReader.Close();
			CloseDB();
		}

		public void SetValue(strSetting[] strSet, DropDownList DropList)
		{
			int idx =0;
			foreach(strSetting S in strSet)
			{
				ListItem DItem = new ListItem(S.strItem1, S.strItem2);
				DropList.Items.Insert(idx, DItem);
				idx += 1;
			}
		}
		#endregion


		/// <summary>
		/// 패스워드  Encryption 관련 함수
		/// Key값은 iFrame으로 fix한다.
		/// </summary>
		#region 패스워드 Encryption 관련 function
		public string SetEncrypting(string Source)
		{
			string Key = "iFrame";
			RC2CryptoServiceProvider SymmCrypto = new RC2CryptoServiceProvider();
			byte[] BytLn = System.Text.ASCIIEncoding.ASCII.GetBytes(Source);
			System.IO.MemoryStream Ms = new System.IO.MemoryStream();

			byte[] BytKey = GetLegalKey(SymmCrypto,Key);

			SymmCrypto.Key  = BytKey;
			SymmCrypto.IV	= BytKey;

			ICryptoTransform Encrypto = SymmCrypto.CreateEncryptor();

			CryptoStream Cs = new CryptoStream(Ms, Encrypto, CryptoStreamMode.Write);
			Cs.Write(BytLn,0, BytLn.Length);
			Cs.FlushFinalBlock();

			byte[] BytOut = Ms.GetBuffer();
			int i = 0;
			for(i = 0; i < BytOut.Length; i++)
				if(BytOut[i] == 0)
					break;
			return System.Convert.ToBase64String(BytOut, 0, i);
		}

		private byte[] GetLegalKey(RC2CryptoServiceProvider SymmCrypto, string Key)
		{
			string ReturnValue;
			if(SymmCrypto.LegalKeySizes.Length > 0)
			{
				int MoreSize = SymmCrypto.LegalKeySizes[0].MinSize;

				if(Key.Length * 8 > 64)
				{
					ReturnValue = Key.Substring(0,8);
				}
				else
				{
					ReturnValue = Key.PadRight(MoreSize/8,' ');
				}
			}
			else
				ReturnValue = Key;

			return ASCIIEncoding.ASCII.GetBytes(ReturnValue);
		}
		#endregion


		#region 로그인 관련(Session Check)
		public void GetSessionCheck(System.Web.HttpResponse resp)
		{
			if(Session["Admin_ID"] == null || Session["Admin_ID"].ToString() == "")
			{
				resp.Write("<script language=javascript>");
				resp.Write(" alert('세션이 끊겼거나, 로그인 하지 않았습니다.\\n\\n로그인 해 주십시요.');");
				resp.Write(" parent.parent.location.href='http://manager.i-frame.co.kr'");
				resp.Write("</"+"script>");
				return;
			}

			if(Session["Admin_ID"].ToString() == "admin")
			{
				this.SysAdmin	= true;
				this.adminSql = " ";
			}
			else
			{
				this.SysAdmin	= false;
				this.adminSql = " group_cd = ( select admin_group from tbl_admin where admin_id = '" + Session["Admin_ID"] + "') ";
			}
		}
		#endregion

		#region GetAuthority
		public bool GetAuthority(string strGubun)
		{
			bool bl = false;
			if(strGubun == "1")	//전체운영자
				if(Session["Admin_Level"] != null && Session["Admin_Level"].ToString() == "1")
					bl = true;

			if(strGubun == "2")	//개별운영자
				if(Session["Admin_Level"].ToString() == "1" || Session["Admin_Level"].ToString() == "2")
					bl = true;

			if(strGubun == "3")	//설치자
				if(Session["Admin_Level"].ToString() == "1" || Session["Admin_Level"].ToString() == "2" || Session["Admin_Level"].ToString() == "3")
					bl = true;
			return bl;
		}
		#endregion


		#region 검색 결과의 수를 출력            :: Fu_TableTotal()
		protected void Fu_TableTotal(int TotalCnt, HtmlTable TableTotal)
		{
			HtmlTableRow TempRow = new HtmlTableRow();		// </TR>
			TempRow.Height	= "24";
			HtmlTableCell TempCell = new HtmlTableCell();		
			TempCell.Align		= "Right";
			TempCell.Attributes.Add("class","SearchTotal");
			TempCell.InnerHtml = "Total : " +TotalCnt.ToString()+" 건";
			TempRow.Controls.Add(TempCell);
			TableTotal.Controls.Add(TempRow);
		}
		#endregion

		/// <summary>
		/// 데이터 초기화부
		/// </summary>
		protected virtual void Fu_DataInit()
		{
		}

		/// <summary>
		/// 데이터 로딩부
		/// </summary>
		protected virtual void Fu_DataLoad()
		{
		}	

		/// <summary>
		/// 데이터 저장부
		/// </summary>
		protected virtual void Fu_DataSave()
		{
		}

		/// <summary>
		/// 데이터 수정부
		/// </summary>
		protected virtual void Fu_DataUpdate()
		{
		}

		/// <summary>
		/// 데이터 삭제부
		/// </summary>
		protected virtual void Fu_DataDelete()
		{
		}

		/// <summary>
		/// 파라메터 전송부
		/// </summary>
		protected virtual void Fu_GetParameter()
		{
		}

		/// <summary>
		/// 파라메터 전송부
		/// </summary>
		protected virtual void Fu_DataNew()
		{
		}

		/// <summary>
		/// 글작성기 초기화
		/// </summary>
		/// <param name="strPath">경로</param>
		/// <param name="strCtrlName">textbox 명</param>
		public void SetHtmlEditor(string strPath, string strCtrlName, string strChoice)
		{
			string strView = string.Empty;
			string jsFileNm = string.Empty;
			StringBuilder strBuilder = new StringBuilder();
			if(strChoice == "V")
				jsFileNm = "view.js";
			else
				jsFileNm = "editor.js";

			strBuilder.Append("<script language=javascript>\n");
			strBuilder.Append("_editor_url = '"+ strPath +"';\n");
			strBuilder.Append("var win_ie_ver = parseFloat(navigator.appVersion.split('MSIE')[1]);\n");
			strBuilder.Append("if (navigator.userAgent.indexOf('Mac')        >= 0) { win_ie_ver = 0; }\n");
			strBuilder.Append("if (navigator.userAgent.indexOf('Windows CE') >= 0) { win_ie_ver = 0; }\n");
			strBuilder.Append("if (navigator.userAgent.indexOf('Opera')      >= 0) { win_ie_ver = 0; }\n");
			strBuilder.Append("if (win_ie_ver >= 5.5) {\n");
			strBuilder.Append("document.write('<scr' + 'ipt src=' +_editor_url+ '"+ jsFileNm +"');\n");
			strBuilder.Append("document.write(' language=Javascript1.2></scr' + 'ipt>');  \n");
			strBuilder.Append("} else { document.write('<scr'+'ipt>function editor_generate() { return false; }</scr'+'ipt>'); }\n");
			strBuilder.Append("</script>");
			Response.Write(strBuilder.ToString());
			
			strView = "<script language=javascript>editor_generate('"+ strCtrlName +"');</script>";
            ClientScript.RegisterStartupScript(this.GetType(), "startup", strView);
            //프레임웍버젼변경으로
            //this.RegisterStartupScript("startup", strView);	//</form>이전에 write 한다.
		}

		#region  페이징 구현 				         :: Fu_JumpToPage(HtmlTable, string, string, int, int, int)
		/// <summary>
		/// ★ Get방식의 인자값 현재페이지:CurPageNum,  검색어:curSearchWord, 
		/// </summary>
		/// <param name="pTableName">테이블 객체명</param>
		/// <param name="pPageName">페이지이름</param>
		/// <param name="pCurPage">현재 페이지 번호</param>
		/// <param name="pTotalPage">총페이지 수</param>
		/// <param name="pAddParam">인자값으로 가지고 다닐 값들</param>		 
		
		//SearchNum=20&SearchMode=T

		public void Fu_JumpToPage(HtmlTable pTableName, string pPageName, int pCurPage, int pTotalPage, string pAddParam)
		{
			HtmlTableRow	TempRow		= new HtmlTableRow();
			HtmlTableCell	TempCell	= new HtmlTableCell();
			HyperLink		TempLink;	

			pTableName.Controls.Clear();
			TempRow.Align = "Center";
	
			TempLink				= new HyperLink();			// 처음 페이지 관련
			if(pCurPage != 1)		TempLink.NavigateUrl	= pPageName+"?Page=1"+ pAddParam;
		
			TempLink.ImageUrl		= "../images/Icon/Icon_Top.gif";
			TempCell.Controls.Add(TempLink);
			TempCell.Controls.Add(new LiteralControl("&nbsp;&nbsp;"));

			TempLink				= new HyperLink();			// 이전 페이지 관련
			if(pCurPage != 1)		TempLink.NavigateUrl	= pPageName+"?Page="+Convert.ToString(pCurPage-1)+pAddParam;
			TempLink.ImageUrl		= "../images/Icon/Icon_Prev.gif";
			TempCell.Controls.Add(TempLink);
			TempCell.Controls.Add(new LiteralControl("&nbsp;&nbsp;"));
	

			int page		= ((pCurPage-1)/10);
			int pageStart	= page*10+1;
			int pageEnd		= (page*10)+10;							// *11로 할경우에는 page값이 0이면 계산 오류가 발생되기때문에 +10으로 처리

			for(int j=pageStart; j<=pageEnd; j++)					// 페이지 번호 삽입 관련
			{
				if(pCurPage == j)
				{
					TempCell.Controls.Add(new LiteralControl("["+j.ToString()+"]"));
					TempCell.Controls.Add(new LiteralControl("&nbsp;"));
				}
				else
				{
					TempLink				= new HyperLink();
					TempLink.NavigateUrl	= pPageName+"?Page="+j.ToString()+pAddParam;
					TempLink.Text			= j.ToString();
					TempCell.Controls.Add(TempLink);
					TempCell.Controls.Add(new LiteralControl("&nbsp;&nbsp;"));
				}
				if (j == pTotalPage)
				{
					break;
				}
			}


			TempCell.Controls.Add(new LiteralControl("&nbsp;"));
			TempLink				= new HyperLink();			// 다음 페이지 관련
			if (pCurPage != pTotalPage)	TempLink.NavigateUrl	= pPageName+"?Page="+Convert.ToString(pCurPage+1)+pAddParam;
			TempLink.ImageUrl		= "../images/Icon/Icon_Next.gif";
			TempCell.Controls.Add(TempLink);

			TempCell.Controls.Add(new LiteralControl("&nbsp;"));
			TempLink				= new HyperLink();			// 마지막 페이지 관련
			if (pCurPage != pTotalPage)	TempLink.NavigateUrl	= pPageName+"?Page="+pTotalPage.ToString()+pAddParam;
			TempLink.ImageUrl		= "../images/Icon/Icon_End.gif";
			TempCell.Controls.Add(TempLink);
			TempRow.Controls.Add(TempCell);
			pTableName.Controls.Add(TempRow);
		}
		#endregion

		protected void OpenWin(string Url,string width,string height,string name)
		{
			string html = " Contents_Windows = window.open('" + Url + "','" + name + "'"
						+ ",'toolbar=no,status=no"
				        + ",width=" + width
						+ ",height=" + height + ",top=100,left=100');Contents_Windows.focus();";

			Response.Write("<script language=javascript>");
			Response.Write( html);
			Response.Write("</"+"script>");

		}
	}
}
