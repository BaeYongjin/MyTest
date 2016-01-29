// TODO : �� Ŭ������ ���� XML���� �����ڵ带 ã�� �޽����� ��ȯ�ϴ� ������ �����ؾ��Ѵ�.

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
				case    0:strMsg = " ������ �߻��Ͽ����ϴ�";																break;
					
				//1000 ���� ����
				case 1000: strMsg = "�˼����� ������ �߻��Ͽ����ϴ�.";													break;
				case 1001: strMsg = "�α׸� ������ �� �����ϴ�.\n�α������� �ִ��� Ȯ���Ͻñ� �ٶ��ϴ�";				break;
				case 1002: strMsg = "ȯ�������� ã�� �� �����ϴ�.\nConfig.xml ������ �ִ��� Ȯ���Ͻñ� �ٶ��ϴ�";		break;
				case 1003: strMsg = "ȯ�����Ͽ� ������ �ֽ��ϴ�.\nConfig.xml ������ ������ Ȯ���Ͻñ� �ٶ��ϴ�";		break;
				case 1004: strMsg = "�����ͺ��̽��� ������ �� �����ϴ�.\n��Ʈ��ũ ���� �Ǵ� ȯ�������� Ȯ���Ͻñ� �ٶ��ϴ�.";			break;
				case 1005: strMsg = "������ ������ �� �����ϴ�\n��Ʈ��ũ ���� �Ǵ� ȯ�������� Ȯ���Ͻñ� �ٶ��ϴ�.";	break;
				case 1101: strMsg = "���ð��ʰ��� �����κ��� ������ ���� ���Ͽ����ϴ�.";								break;
				case 1102: strMsg = "�����ý��� ����Դϴ�.\n����� �ٽ� �����Ͽ� ����Ͻñ� �ٶ��ϴ�.";				break;  				
				case 1103: strMsg = "DBMS���� ������ �߻��Ͽ����ϴ�.\n�����ڿ��� �����Ͻñ� �ٶ��ϴ�.";					break;
				case 1104: strMsg = "�����ڿ��� �����Ͻʽÿ�";															break;
				case 1201: strMsg = "�ش��ϴ� �����ڵ尡 �����ϴ�.\n�ý��� ������ Ȯ���Ͻñ� �ٶ��ϴ�.";				break;
				
				//2000  LOGIN, ��������			
				case 2000: strMsg = "�ý��۹����� ��ġ���� �ʽ��ϴ�.\n ���α׷��� ������Ʈ�Ͻñ� �ٶ��ϴ�.";			break;
				case 2001: strMsg = "����� ���̵� �����ϴ�.";														break;
				case 2002: strMsg = "��ϵ��� ���� ������Դϴ�.\n�����ڿ��� �����Ͻñ� �ٶ��ϴ�";						break;
				case 2003: strMsg = "��й�ȣ�� ��ġ���� �ʽ��ϴ�.\nȮ���� �ٽ� �α����Ͻñ� �ٶ��ϴ�.";				break;								
				case 2004: strMsg = "���ε��� ���� ������Դϴ�.\n�����ڿ��� �����Ͻñ� �ٶ��ϴ�.";						break;
				case 2005: strMsg = "�α��� ���� DBMS���� ������ �߻��Ͽ����ϴ�.";										break;
				case 2101: strMsg = "�� ������ �������� ������ ���� �ʽ��ϴ�.";										break;
				case 2102: strMsg = "���������ð��� �ʰ��Ǿ����ϴ�.\n�ڵ����� �α׾ƿ� �˴ϴ�.";						break;

				//3000  �ڷ� insert, update, delete
				case 3000: strMsg = "��ȸ �� ������ �߻��Ͽ����ϴ�";													break;

				case 3100: strMsg = "�ڷᰡ ���������� ��ϵǾ����ϴ�";													break;
				case 3101: strMsg = "�ڷḦ ����ϴ� �� ������ �߻��Ͽ����ϴ�.";										break;
				
				case 3200: strMsg = "�ڷᰡ ���������� �����Ǿ����ϴ�";													break;
				case 3201: strMsg = "�ڷḦ �����ϴ� �� ������ �߻��Ͽ����ϴ�.";										break;

				case 3300: strMsg = "�ڷᰡ ���������� �����Ǿ����ϴ�";													break;
				case 3301: strMsg = "�ڷḦ �����ϴ� �� ������ �߻��Ͽ����ϴ�.";										break;

				case 3400: strMsg = "�ڷᰡ ���������� ��ȸ�Ǿ����ϴ�";													break;
				case 3401: strMsg = "�ڷḦ ��ȸ�ϴ� �� ������ �߻��Ͽ����ϴ�.";										break;

				case 3500: strMsg = "�ڷᰡ ���������� ó���Ǿ����ϴ�";													break;
				case 3501: strMsg = "�ڷḦ ó���ϴ� �� ������ �߻��Ͽ����ϴ�.";										break;
				
				case 3600: strMsg = "�ڷᰡ ���������� ����Ǿ����ϴ�";													break;
				case 3601: strMsg = "�ڷḦ �����ϴ� �� ������ �߻��Ͽ����ϴ�.";										break;

				//4000 �����Ͱ˻�
				case 4001: strMsg = "����� ���̵� �Է��� �ֽʽÿ�";													break;
				case 4002: strMsg = "�̹� ������� ����� ���̵��Դϴ�. �ٸ� ���̵� �Է��� �ֽʽÿ�";					break;
				case 4003: strMsg = "��ϵ� ����ڰ� �ƴմϴ�.";														break;
				
				case 9000: strMsg = "�ý������� ���Դϴ�.\n����� �ٽ� ����Ͻñ� �ٶ��ϴ�.";							break;
					
				default:strMsg = "�˼����� ������ �߻��Ͽ����ϴ�.\n�����ڿ��� �����Ͻñ� �ٶ��ϴ�.";																break;
			}

			return strMsg;
		}
	}
}