using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Web;
using System.Web.Services;

using WinFramework.Base;

using AdManagerModel;

namespace AdManagerWebService.ReportSummaryAd
{
    /// <summary>
    /// 기본네임스페이스를 설정한다.
    /// </summary>
    [WebService(Namespace = "http://advmgt.hanafostv.com/AdManagerWebService/")]
    [System.ComponentModel.ToolboxItem(false)]

    /// <summary>
    /// PreferenceTotalizeService의 요약 설명입니다.
    /// </summary>
    public class PreferenceTotalizeService : System.Web.Services.WebService
    {
        private PreferenceTotalizeBiz preferenceTotalizeBiz = null;

        public PreferenceTotalizeService()
        {
            preferenceTotalizeBiz = new PreferenceTotalizeBiz();
        }

        [WebMethod]
        public PreferenceTotalizeModel GetAdList(HeaderModel header, PreferenceTotalizeModel model)
        {
            new PreferenceTotalizeBiz().GetAdList(header, model);
            return model;
        }

        [WebMethod]
        public PreferenceTotalizeModel getPopupList(HeaderModel header, PreferenceTotalizeModel model)
        {
            new PreferenceTotalizeBiz().getPopupList(header, model);
            return model;
        }
    }
}
