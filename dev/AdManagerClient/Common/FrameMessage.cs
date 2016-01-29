// TODO : 이 클래스는 향후 XML에서 에러코드를 찾아 메시지를 반환하는 것으로 변경해야한다.

using System;

namespace AdManagerClient
{
	public class FrameMessage
	{
		public FrameMessage(){}

		public static string GetMessage(string MsgCode)
		{
			int code;

			if( MsgCode == null || MsgCode == "") MsgCode="0000";

			code = Convert.ToInt32(MsgCode);

			return GetMessage(code);
		}

		
		public static string GetMessage(int code)
		{
			string strMsg = null;

			switch(code)
			{
				case    0:strMsg = " 오류가 발생하였습니다";																break;
					
				//1000 공통 에러
				case 1000: strMsg = "알수없는 오류가 발생하였습니다.";													break;
				case 1001: strMsg = "로그를 실행할 수 없습니다.\n로그파일이 있는지 확인하시기 바랍니다";				break;
				case 1002: strMsg = "환경파일을 찾을 수 없습니다.\nConfig.xml 파일이 있는지 확인하시기 바랍니다";		break;
				case 1003: strMsg = "환경파일에 오류가 있습니다.\nConfig.xml 파일의 내용을 확인하시기 바랍니다";		break;
				case 1004: strMsg = "데이터베이스에 연결할 수 없습니다.\n네트워크 상태 또는 환경파일을 확인하시기 바랍니다.";			break;
				case 1005: strMsg = "서버에 연결할 수 없습니다\n네트워크 상태 또는 환경파일을 확인하시기 바랍니다.";	break;
				case 1101: strMsg = "대기시간초과로 서버로부터 응답을 받지 못하였습니다.";								break;
				case 1102: strMsg = "서버시스템 장애입니다.\n잠시후 다시 접속하여 사용하시기 바랍니다.";				break;  				
				case 1103: strMsg = "DBMS에서 오류가 발생하였습니다.\n관리자에게 문의하시기 바랍니다.";					break;
				case 1104: strMsg = "관리자에게 문의하십시오";															break;
				case 1201: strMsg = "해당하는 서비스코드가 없습니다.\n시스템 버전을 확인하시기 바랍니다.";				break;
				
				//2000  LOGIN, 접속유지			
				case 2000: strMsg = "시스템버전이 일치하지 않습니다.\n 프로그램을 업데이트하시기 바랍니다.";			break;
				case 2001: strMsg = "사용자 아이디가 없습니다.";														break;
				case 2002: strMsg = "등록되지 않은 사용자입니다.\n관리자에게 문의하시기 바랍니다";						break;
				case 2003: strMsg = "비밀번호가 일치하지 않습니다.\n확인후 다시 로그인하시기 바랍니다.";				break;								
				case 2004: strMsg = "승인되지 않은 사용자입니다.\n관리자에게 문의하시기 바랍니다.";						break;
				case 2005: strMsg = "로그인 도중 DBMS에서 오류가 발생하였습니다.";										break;
				case 2101: strMsg = "본 서비스의 사용권한을 가지고 있지 않습니다.";										break;
				case 2102: strMsg = "접속유지시간이 초과되었습니다.\n자동으로 로그아웃 됩니다.";						break;

				//3000  자료 insert, update, delete
				case 3000: strMsg = "조회 중 오류가 발생하였습니다";													break;

				case 3100: strMsg = "자료가 정상적으로 등록되었습니다";													break;
				case 3101: strMsg = "자료를 등록하는 중 오류가 발생하였습니다.";										break;
				
				case 3200: strMsg = "자료가 정상적으로 수정되었습니다";													break;
				case 3201: strMsg = "자료를 수정하는 중 오류가 발생하였습니다.";										break;

				case 3300: strMsg = "자료가 정상적으로 삭제되었습니다";													break;
				case 3301: strMsg = "자료를 삭제하는 중 오류가 발생하였습니다.";										break;

				case 3400: strMsg = "자료가 정상적으로 조회되었습니다";													break;
				case 3401: strMsg = "자료를 조회하는 중 오류가 발생하였습니다.";										break;

				case 3500: strMsg = "자료가 정상적으로 처리되었습니다";													break;
				case 3501: strMsg = "자료를 처리하는 중 오류가 발생하였습니다.";										break;
				
				case 3600: strMsg = "자료가 정상적으로 저장되었습니다";													break;
				case 3601: strMsg = "자료를 저장하는 중 오류가 발생하였습니다.";										break;

				//4000 데이터검사
				case 4001: strMsg = "사용자 아이디를 입력해 주십시오";													break;
				case 4002: strMsg = "이미 사용중이 사용자 아이디입니다. 다른 아이디를 입력해 주십시오";					break;
				case 4003: strMsg = "등록된 사용자가 아닙니다.";														break;
				
				case 9000: strMsg = "시스템점검 중입니다.\n잠시후 다시 사용하시기 바랍니다.";							break;
					
				default:strMsg = "알수없는 오류가 발생하였습니다.\n관리자에게 문의하시기 바랍니다.";																break;
			}

			return strMsg;
		}
	}
}