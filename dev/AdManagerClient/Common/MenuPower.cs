
using System;
using System.Collections;

namespace AdManagerClient
{
	/// <summary>
	/// 화면별 보안레벨을 설정한다
	/// </summary>
	public class MenuPower
	{
		// 메뉴별 권한 저장 및 검색을 위한 해쉬테이블
		private Hashtable	menuPowerHT = null;

		#region 생성자

		public MenuPower()
		{
			menuPowerHT = new Hashtable();
		}

		#endregion

		#region 초기화
		
		public void Add(string MenuCode, string MenuPower)
		{
			// 클라이언트모델 해쉬테이블에 추가한다.
			menuPowerHT.Add(MenuCode, MenuPower);
		}

		#endregion

		#region 권한조회

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
