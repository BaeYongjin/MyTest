using System.Web.Services;

using WinFramework.Base;
using AdManagerModel;

namespace AdManagerWebService.ReportMedia
{
    /// <summary>
    /// 기본네임스페이스를 설정한다.
    /// </summary>
    [WebService(Namespace = "http://advmgt.hanafostv.com/AdManagerWebService/")]
    [System.ComponentModel.ToolboxItem(false)]

	public class StatisticsPgService : System.Web.Services.WebService
	{
        public StatisticsPgService()
		{
		}


        [WebMethod]
        public StatisticsPgModel GetCategoryList(HeaderModel header, StatisticsPgModel model)
        {
            new StatisticsPgBiz().GetCategoryList(header, model);
            return model;
        }

        [WebMethod]
        public StatisticsPgModel GetGenreList(HeaderModel header, StatisticsPgModel model)
        {
            new StatisticsPgBiz().GetGenreList(header, model);
            return model;
        }

        [WebMethod]
        public StatisticsPgModel GetStatisticsPgAge(HeaderModel header, StatisticsPgModel model)
        {
            new StatisticsPgBiz().GetStatisticsPgAge(header, model);
            return model;
        }

        [WebMethod]
        public StatisticsPgModel GetStatisticsPgAgeAVG(HeaderModel header, StatisticsPgModel model)
        {
            new StatisticsPgBiz().GetStatisticsPgAgeAVG(header, model);
            return model;
        }

	}
}