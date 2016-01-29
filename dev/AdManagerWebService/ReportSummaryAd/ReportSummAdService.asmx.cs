using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;

using WinFramework.Base;
using AdManagerModel.ReportAd;

namespace AdManagerWebService.ReportSummaryAd
{
    /// <summary>
    /// 기본네임스페이스를 설정한다.
    /// </summary>
    [WebService(Namespace = "http://advmgt.hanafostv.com/AdManagerWebService/", Description = "Btv 레포팅 데이터 웹서비스 입니다.")]
    [System.ComponentModel.ToolboxItem(false)]
    public class ReportSummAdService : System.Web.Services.WebService
    {

        [WebMethod(Description = "광고총괄 리포팅 집계")]
        public RptAdBaseModel GetList(HeaderModel header, RptAdBaseModel model)
        {
            ReportSummAdBiz biz = new ReportSummAdBiz();

            biz.GetList_Combine(header, model);
            return model;
        }
    }
}
