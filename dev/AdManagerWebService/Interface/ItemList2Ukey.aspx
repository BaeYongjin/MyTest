<%@ Page language="c#" Codebehind="ItemList2Ukey.aspx.cs" AutoEventWireup="True" Inherits="AdManagerWebService.Interface.RELAY_FORM" EnableSessionState="False" enableViewState="False"%>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>광고목록</title>
		<meta name="vs_snapToGrid" content="False">
		<meta name="vs_showGrid" content="False">
		<meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" content="C#">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
		<LINK rel="STYLESHEET" type="TEXT/CSS" href="cssComm.css"> <!-- 공통 --><LINK rel="STYLESHEET" type="TEXT/CSS" href="cssList.css"> <!-- 리스트 -->
		<script language="javascript">
		
			var	returnId = "";
			var returnNm = "";
			var returnCd = "";

			function init()
			{
				document.RELAY_FORM.POPUP_STAT.value = "Y";
				document.RELAY_FORM.TMPLT_ID.value = "";
				document.RELAY_FORM.TMPLT_NM.value = "";
				return true;
			}
		
			// 저장버튼 클릭시 이벤트 처리
			function JS_Select(p_cd, p_name)
			{
				document.RELAY_FORM.TMPLT_ID.value = p_cd;
				document.RELAY_FORM.TMPLT_NM.value = p_name;
				document.RELAY_FORM.POPUP_STAT.value = "N";
				return true;
			}
			
			function OnSelect()
			{
				document.RELAY_FORM.POPUP_STAT.value = "C";
				
				returnId	=	document.RELAY_FORM.TMPLT_ID.value;
				returnNm	=	document.RELAY_FORM.TMPLT_NM.value;
				returnCd	=	document.RELAY_FORM.POPUP_STAT.value;
				opener.setParameters(returnCd, returnId, returnNm);  
				window.close();
			}
		</script>
	</HEAD>
	<body leftmargin="0" topmargin="0" onload="javascript:init();">
		<form id="RELAY_FORM" method="post" runat="server">
			<table border="0" cellSpacing="0" cellPadding="0" width="800" height="480">
				<tr>
					<td><IMG align="absMiddle" src="images/icon/icon_Dot.gif"><b><font size="3"> 광고목록 리스트</font></b></td>
				</tr>
				<tr>
					<td height="3" width="800">&nbsp; <INPUT style="WIDTH: 92px; HEIGHT: 20px" size="10" name="TMPLT_ID">
						<INPUT style="BORDER-BOTTOM-STYLE: solid; BORDER-BOTTOM-COLOR: green; BORDER-RIGHT-STYLE: solid; BORDER-TOP-COLOR: green; WIDTH: 382px; BORDER-TOP-STYLE: solid; HEIGHT: 20px; BORDER-RIGHT-COLOR: green; BORDER-LEFT-STYLE: solid; BORDER-LEFT-COLOR: green"
							size="58" name="TMPLT_NM"> <INPUT style="WIDTH: 40px; HEIGHT: 20px" size="10" name="POPUP_STAT">
						<INPUT type="button" value="선택" style="BORDER-BOTTOM-STYLE:solid;TEXT-ALIGN:center;BORDER-RIGHT-STYLE:solid;WIDTH:63px;BORDER-TOP-STYLE:solid;HEIGHT:22px;VERTICAL-ALIGN:baseline;BORDER-LEFT-STYLE:solid"
							onclick="javascript:OnSelect();"></td>
				</tr>
				<tr>
					<td height="2"><FONT face="굴림"></FONT></td>
				</tr>
				<tr>
					<td>
						<!--**** 이곳에 목적 프로그램 추가 start ****-->
						<table class="BoardTable" cellSpacing="0">
							<tr class="BoardTR">
								<td style="WIDTH: 50px" id="ItemNo" class="BoardHeader">ItemNo</td>
								<td id="ItemNm" class="BoardHeader">광고명</td>
								<td style="WIDTH: 80px" id="BeginDay" class="BoardHeader">시작일</td>
								<td style="WIDTH: 80px" id="EndDay" class="BoardHeader">종료일</td>
								<td style="WIDTH: 70px" id="AdType" class="BoardHeader">종류</td>
								<td style="WIDTH: 70px" id="AdState" class="BoardHeader">상태</td>
								<td style="WIDTH: 120px" id="FileState" class="BoardHeader">파일상태</td>
							</tr>
						</table>
						<div id="DivScroll" runat="server"> <!-- 스크롤을 위한 Div테그 -->
							<table id="TableResult" class="BoardTable1" cellSpacing="0" runat="server">
								<tr class="BoardTR">
									<td style="WIDTH: 50px" id="ItemNo1" class="BoardHeader1"></td>
									<td id="ItemNm1" class="BoardHeader1"><FONT face="굴림"></FONT></td>
									<td style="WIDTH: 80px" id="BeginDay1" class="BoardHeader1"><FONT face="굴림"></FONT></td>
									<td style="WIDTH: 80px" id="EndDay1" class="BoardHeader1"></td>
									<td style="WIDTH: 70px" id="AdType1" class="BoardHeader1"></td>
									<td style="WIDTH: 70px" id="AdState1" class="BoardHeader1"></td>
									<td style="WIDTH: 120px" id="FileState1" class="BoardHeader1"></td>
								</tr>
							</table>
						</div>
						<TABLE style="MARGIN-TOP: 0px; WIDTH: 100%; HEIGHT: 20px" id="TableTotal" class="BoardTable1"
							runat="server">
						</TABLE> <!-- 조회 건수 삽입 부분-->
						<TABLE style="WIDTH: 100%; HEIGHT: 20px" id="TablePage" runat="server">
						</TABLE> <!-- 페이징 삽입 부분 -->
						<!--**** 이곳에 목적 프로그램 추가 end ****--></td>
				</tr>
			</table>
		</form>
		<%=this.Script%>
	</body>
</HTML>
