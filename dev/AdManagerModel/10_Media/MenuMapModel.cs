using System;
using System.Data;

using WinFramework.Base;

namespace AdManagerModel
{
	/// <summary>
	/// 메뉴 매핑 정보 모델
	/// </summary>
	public class MenuMapModel : BaseModel
	{
		// 조회용
		public string SearchCategory { get; set; }
		public string SearchMenuCode { get; set; }
		
		public string MenuCode { get; set; }

		public string MenuName { get; set; }

		public string UpperMenuCode { get; set; }

		public string MenuLevel { get; set; }

		public DateTime ModDt { get; set; }

		public string MenuOrder { get; set; }

		public string AdGenre { get; set; }

		public string MenuCode4 { get; set; }

		public string MenuName4 { get; set; }

		public string UpperMenuCode4 { get; set; }

		public string MenuLevel4 { get; set; }

		public DateTime ModDt4 { get; set; }

		public string MenuOrder4 { get; set; }

		public string AdGenre4 { get; set; }

		public string UiName { get; set; }

		public DataSet CategoryDs { get; set; }
		public DataSet MenuMapDs { get; set; }
        public DataSet NoneMenuStdDs { get; set; }
        public DataSet NoneMenu4Ds { get; set; }
		
		public MenuMapModel()
			: base() 
		{
			Init();
		}

		#region Public 메소드
		public void Init()
		{			
			SearchCategory = "";
			SearchMenuCode = "";
		
			MenuCode = "";
			MenuName = "";
			UpperMenuCode = "";
			MenuLevel = "";
			ModDt = new DateTime();
			MenuOrder = "";
			AdGenre = "";
			
			MenuCode4 = "";
			MenuName4 = "";
			UpperMenuCode4 = "";
			MenuLevel4 = "";
			ModDt4 = new DateTime();
			MenuOrder4 = "";
			AdGenre4 = "";
			UiName = "";

			CategoryDs = new DataSet();
			MenuMapDs = new DataSet();
            NoneMenuStdDs = new DataSet();
            NoneMenu4Ds = new DataSet();
		}

		#endregion
	}
}
