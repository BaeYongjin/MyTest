<%@ Page language="c#" Codebehind="Relay.aspx.cs" AutoEventWireup="True" Inherits="AdManagerWebService.Interface.Relay" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>Relay</title>
		<meta name="GENERATOR" Content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" Content="C#">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
		<SCRIPT language="javascript">
		//�ǽð��˾����� ����Ʈ��ȸ 
		var surl = "ItemList2Ukey.aspx";
		var sEnv = "resizable=no scrollbars=no width=800 height=480 left=100% top=80%";
		window.open(surl,"AdList", sEnv);



		function setParameters(POPUP_STAT, TMPLT_ID, TMPLT_NM)
		{
			var frm = document.RELAY_FORM;
			frm.POPUP_STAT.value = POPUP_STAT;	
			frm.TMPLT_ID.value = TMPLT_ID;	
			frm.TMPLT_NM.value = TMPLT_NM;			
		}

		//������ ���ø� �����˾��� ������½� POPUP_STAT �� "Y" �� ����
		function setPopupStat(POPUP_STAT)
		{
			var frm = document.RELAY_FORM;
			frm.POPUP_STAT.value = POPUP_STAT;			
		}

		</SCRIPT>
	</HEAD>
	<body leftmargin="0" topmargin="0">
		<form name="RELAY_FORM" method="post">
			�˾�����:<input name="POPUP_STAT" value="I" size="100" readonly><br>
			���ø�ID:<input name="TMPLT_ID" size="100" readonly><br>
			���ø���:<input name="TMPLT_NM" size="100" readonly><br>
		</form>
	</body>
</HTML>
