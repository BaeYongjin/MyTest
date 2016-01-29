
using System;
using System.Collections;

namespace AdManagerClient
{
	/// <summary>
	/// ȭ�麰 ���ȷ����� �����Ѵ�
	/// </summary>
	public class MenuPower
	{
		// �޴��� ���� ���� �� �˻��� ���� �ؽ����̺�
		private Hashtable	menuPowerHT = null;

		#region ������

		public MenuPower()
		{
			menuPowerHT = new Hashtable();
		}

		#endregion

		#region �ʱ�ȭ
		
		public void Add(string MenuCode, string MenuPower)
		{
			// Ŭ���̾�Ʈ�� �ؽ����̺� �߰��Ѵ�.
			menuPowerHT.Add(MenuCode, MenuPower);
		}

		#endregion

		#region ������ȸ

		public bool IsMenu(string MenuCode)
		{
			return menuPowerHT.ContainsKey(MenuCode);
		}

		public string GetPower(string MenuCode)
		{
			if (!menuPowerHT.ContainsKey(MenuCode) ) return null;
			string formCRUD = (string) menuPowerHT[MenuCode];

			return formCRUD;
		}
	
		public bool CanCreate(string MenuCode)
		{
			if (!menuPowerHT.ContainsKey(MenuCode) ) return false;
			string formCRUD = (string) menuPowerHT[MenuCode];

			if(formCRUD.IndexOf("C") < 0) return false;
			return true;
		}

		public bool CanRead(string MenuCode)
		{
			if (!menuPowerHT.ContainsKey(MenuCode) ) return false;
			string formCRUD = (string) menuPowerHT[MenuCode];

			if(formCRUD.IndexOf("R") < 0) return false;
			return true;
		}

		public bool CanUpdate(string MenuCode)
		{
			if (!menuPowerHT.ContainsKey(MenuCode) ) return false;
			string formCRUD = (string) menuPowerHT[MenuCode];

			if(formCRUD.IndexOf("U") < 0) return false;
			return true;
		}

		public bool CanDelete(string MenuCode)
		{
			if (!menuPowerHT.ContainsKey(MenuCode) ) return false;
			string formCRUD = (string) menuPowerHT[MenuCode];

			if(formCRUD.IndexOf("D") < 0) return false;
			return true;
		}
		#endregion
	}
}
