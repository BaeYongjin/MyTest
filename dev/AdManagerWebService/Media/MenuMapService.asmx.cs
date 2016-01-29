using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Web;
using System.Web.Services;

using WinFramework.Base;
using AdManagerModel;

namespace AdManagerWebService.Media
{
	/// <summary>
	/// MenuMapService의 요약 설명입니다.
	/// </summary>
	[WebService(Namespace = "http://advmgt.hanafostv.com/AdManagerWebService/")]
	[System.ComponentModel.ToolboxItem(false)]
	
	public class MenuMapService : System.Web.Services.WebService
	{
		private MenuMapBiz biz = null;

		public MenuMapService()
		{
			biz = new MenuMapBiz();
		}

		[WebMethod]
		public MenuMapModel GetCategoryList(HeaderModel header, MenuMapModel model)
		{
			biz.GetCategoryList(header, model);
			return model;
		}

        [WebMethod]
        public MenuMapModel GetMenuMapList(HeaderModel header, MenuMapModel model) 
        {
            biz.GetMenuMapList(header, model);
            return model;
        }

        [WebMethod]
        public MenuMapModel SetMenuMapUpdate(HeaderModel header, MenuMapModel model)
        {
            biz.SetMenuMapUpdate(header, model);
            return model;
        }

        [WebMethod]
        public MenuMapModel SetMenuMapCreate(HeaderModel header, MenuMapModel model)
        {
            biz.SetMenuMapCreate(header, model);
            return model;
        }

        [WebMethod]
        public MenuMapModel SetMenuMapDelete(HeaderModel header, MenuMapModel model)
        {
            biz.SetMenuMapDelete(header, model);
            return model;
        }

        [WebMethod]
        public MenuMapModel SetMenuCreate(HeaderModel header, MenuMapModel model)
        {
            biz.SetMenuCreate(header, model);
            return model;
        }
	}
}
