using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Web;
using System.Web.Services;

using WinFramework.Base;
using AdManagerModel;

namespace AdManagerWebService.Schedule
{
    /// <summary>
    /// SchHomeGroupService의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://advmgt.hanafostv.com/AdManagerWebService/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    public class SchHomeGroupService : System.Web.Services.WebService
    {
        private SchHomeGroupBiz schHomeGroupBiz = null;

        public SchHomeGroupService()
        {
            schHomeGroupBiz = new SchHomeGroupBiz();
        }

        [WebMethod]
        public SchHomeGroupModel GetSchHomeList1(HeaderModel header, SchHomeGroupModel data)
        {
            // 홈OAP그룹편성 주중 조회
            schHomeGroupBiz.GetSchHomeList1(header, data);
            return data;
        }

        [WebMethod]
        public SchHomeGroupModel GetSchHomeList2(HeaderModel header, SchHomeGroupModel data)
        {
            // 홈OAP그룹편성 주말 조회
            schHomeGroupBiz.GetSchHomeList2(header, data);
            return data;
        }

        [WebMethod]
        public SchHomeGroupModel GetSchHomeListCount(HeaderModel header, SchHomeGroupModel data)
        {
            schHomeGroupBiz.GetSchHomeListCount(header, data);
            return data;
        }

        [WebMethod]
        public SchHomeGroupModel GetSchHomeGroupList(HeaderModel header, SchHomeGroupModel data)
        {
            // 홈광고그룹 목록 조회
            schHomeGroupBiz.GetSchHomeGroupList(header, data);
            return data;
        }

        [WebMethod]
        public SchHomeGroupModel GetSchHomeGroupDetailList(HeaderModel header, SchHomeGroupModel data)
        {
            // 그룹별 광고목록 조회
            schHomeGroupBiz.GetSchHomeGroupDetailList(header, data);
            return data;
        }

        [WebMethod]
        public SchHomeGroupModel SetSchHomeCreate(HeaderModel header, SchHomeGroupModel data)
        {
            // 홈OAP그룹편성 추가
            schHomeGroupBiz.SetSchHomeCreate(header, data);
            return data;
        }

        [WebMethod]
        public SchHomeGroupModel SetSchHomeSave(HeaderModel header, SchHomeGroupModel data)
        {
            // 홈OAP그룹편성 수정
            schHomeGroupBiz.SetSchHomeSave(header, data);
            return data;
        }

        [WebMethod]
        public SchHomeGroupModel SetSchHomeDelete(HeaderModel header, SchHomeGroupModel data)
        {
            // 홈OAP그룹편성 삭제
            schHomeGroupBiz.SetSchHomeDelete(header, data);
            return data;
        }

    }
}
