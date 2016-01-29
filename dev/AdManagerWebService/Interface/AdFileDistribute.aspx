<%@ Page language="c#" Codebehind="AdFileDistribute.aspx.cs" AutoEventWireup="True" Inherits="AdManagerWebService.Interface.AdFileDistribute" EnableSessionState="False" enableViewState="False" smartNavigation="True"%>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>AdFileDistribute</title>
		<meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" content="C#">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
	</HEAD>
	<body MS_POSITIONING="GridLayout">
		<form id="Form1" method="post" runat="server">
			<FONT face="굴림">
				<TABLE style="Z-INDEX: 102; POSITION: absolute; WIDTH: 1000px; HEIGHT: 72px; TOP: 8px; LEFT: 8px"
					id="Table1" border="0" cellSpacing="0" cellPadding="0" width="1000">
					<TR height="3" width="1000">
						<td height="3" width="3"><IMG src="images/table_01.jpg"></td>
						<td height="3" background="images/table_02.jpg" width="994"></td>
						<td height="3" width="3"><IMG src="images/table_03.jpg"></td>
					</TR>
					<TR>
						<td background="images/table_04.jpg" width="2"></td>
						<td background="images/table_05.jpg">
							<table style="FONT-FAMILY: '맑은 고딕'; FONT-SIZE: 9pt" border="0" cellSpacing="0" cellPadding="0" width="100%" height="30">
								<tr>
									<td style="WIDTH: 154px; COLOR: silver; FONT-WEIGHT: bold" width="154">&nbsp;# 
										파일배포요청 목록</td>
									<td width="3"></td>
									<td style="COLOR: yellow">&nbsp;&nbsp; 조회일자 :
										<asp:textbox style="Z-INDEX: 0" id="TextBox1" runat="server" Font-Size="8pt" Width="72px">2001-10-13</asp:textbox>&nbsp;~
										<asp:textbox style="Z-INDEX: 0" id="TextBox2" runat="server" Font-Size="8pt" Width="64px">2001-10-13</asp:textbox>&nbsp;&nbsp;
										<% //<asp:button style="Z-INDEX: 0" id="Button1" runat="server" Font-Size="8pt" Text="조회" onclick="Button1_Click"></asp:button  -->&nbsp;  %>
										# 날짜를 yyyy-mm-dd형식으로 입력하세요</td>
								</tr>
							</table>
						</td>
						<td background="images/table_06.jpg"></td>
					</TR>
					<TR height="3" width="1000">
						<td height="3" width="3"><IMG src="images/table_07.jpg"></td>
						<td height="3" background="images/table_08.jpg" width="994"></td>
						<td height="3" width="3"><IMG src="images/table_09.jpg"></td>
					</TR>
					<TR height="3">
						<TD></TD>
						<TD></TD>
						<TD></TD>
					</TR>
					<TR>
						<TD colSpan="3"><asp:datagrid style="Z-INDEX: 0" id=DataGrid1 runat="server" CellSpacing="1" CellPadding="3" BackColor="White" BorderWidth="2px" BorderStyle="Double" BorderColor="#E7E7FF" DataSource="<%# dsFileDis %>" AutoGenerateColumns="False" Font-Size="8pt" Font-Names="맑은 고딕" EnableViewState="False" HorizontalAlign="Center" UseAccessibleHeader="True" Width="1000px" Height="96px">
								<FooterStyle ForeColor="#4A3C8C" BackColor="#B5C7DE"></FooterStyle>
								<SelectedItemStyle Font-Bold="True" ForeColor="#F7F7F7" BackColor="#738A9C"></SelectedItemStyle>
								<AlternatingItemStyle BackColor="#F7F7F7"></AlternatingItemStyle>
								<ItemStyle ForeColor="#4A3C8C" BackColor="#E7E7FF"></ItemStyle>
								<HeaderStyle Font-Bold="True" ForeColor="#F7F7F7" BackColor="#4A3C8C"></HeaderStyle>
								<Columns>
									<asp:BoundColumn DataField="ItemNo" SortExpression="WorkDt" HeaderText="번호">
										<HeaderStyle Width="40px"></HeaderStyle>
										<ItemStyle HorizontalAlign="Center"></ItemStyle>
									</asp:BoundColumn>
									<asp:BoundColumn DataField="ItemName" HeaderText="광고명">
										<HeaderStyle Width="200px"></HeaderStyle>
										<ItemStyle HorizontalAlign="Left"></ItemStyle>
									</asp:BoundColumn>
									<asp:BoundColumn DataField="WorkDt" HeaderText="요청일" DataFormatString="{0:MM-dd HH:mm:ss}">
										<HeaderStyle Width="100px"></HeaderStyle>
										<ItemStyle HorizontalAlign="Center"></ItemStyle>
									</asp:BoundColumn>
									<asp:BoundColumn DataField="ResultDt" HeaderText="처리일" DataFormatString="{0:MM-dd HH:mm:ss}">
										<HeaderStyle Width="100px"></HeaderStyle>
										<ItemStyle HorizontalAlign="Center"></ItemStyle>
									</asp:BoundColumn>
									<asp:BoundColumn DataField="FileName" HeaderText="파일명">
										<HeaderStyle Width="130px"></HeaderStyle>
									</asp:BoundColumn>
									<asp:BoundColumn DataField="RequestStatus" HeaderText="요청">
										<HeaderStyle Width="40px"></HeaderStyle>
										<ItemStyle HorizontalAlign="Center"></ItemStyle>
									</asp:BoundColumn>
									<asp:BoundColumn DataField="ProcStatus" HeaderText="처리">
										<HeaderStyle Width="40px"></HeaderStyle>
										<ItemStyle HorizontalAlign="Center"></ItemStyle>
									</asp:BoundColumn>
									<asp:BoundColumn DataField="SyncServer" HeaderText="Sync">
										<HeaderStyle Width="30px"></HeaderStyle>
										<ItemStyle HorizontalAlign="Center"></ItemStyle>
									</asp:BoundColumn>
									<asp:BoundColumn DataField="DescServer" HeaderText="Desc">
										<HeaderStyle Width="30px"></HeaderStyle>
										<ItemStyle HorizontalAlign="Center"></ItemStyle>
									</asp:BoundColumn>
									<asp:BoundColumn DataField="ResultMsg" HeaderText="비고"></asp:BoundColumn>
								</Columns>
								<PagerStyle HorizontalAlign="Right" ForeColor="#4A3C8C" BackColor="#E7E7FF" Mode="NumericPages"></PagerStyle>
							</asp:datagrid></TD>
					</TR>
				</TABLE>
			</FONT>
		</form>
	</body>
</HTML>
