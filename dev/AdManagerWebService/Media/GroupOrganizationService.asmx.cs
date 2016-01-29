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
    /// 기본네임스페이스를 설정한다.
    /// </summary>
    [WebService(Namespace = "http://advmgt.hanafostv.com/AdManagerWebService/")]
    [System.ComponentModel.ToolboxItem(false)]

    /// <summary>
    /// GroupOrganizationService에 대한 요약 설명입니다.
    /// </summary>
    public class GroupOrganizationService : System.Web.Services.WebService
    {
        private GroupOrganizationBiz groupOrganizationBiz = null;

        public GroupOrganizationService()
        {
            groupOrganizationBiz = new GroupOrganizationBiz();
        }

        [WebMethod]
        public GroupOrganizationModel GetGroupList(HeaderModel header, GroupOrganizationModel groupOrganizationModel)
        {
            groupOrganizationBiz.GetGroupList(header, groupOrganizationModel);
            return groupOrganizationModel;
        }

        [WebMethod]
        public GroupOrganizationModel SetGroupAdd(HeaderModel header, GroupOrganizationModel groupOrganizationModel)
        {
            groupOrganizationBiz.SetGroupAdd(header, groupOrganizationModel);
            return groupOrganizationModel;
        }

        [WebMethod]
        public GroupOrganizationModel SetGroupUpdate(HeaderModel header, GroupOrganizationModel groupOrganizationModel)
        {
            groupOrganizationBiz.SetGroupUpdate(header, groupOrganizationModel);
            return groupOrganizationModel;
        }

        [WebMethod]
        public GroupOrganizationModel SetGroupDelete(HeaderModel header, GroupOrganizationModel groupOrganizationModel)
        {
            groupOrganizationBiz.SetGroupDelete(header, groupOrganizationModel);
            return groupOrganizationModel;
        }

        [WebMethod]
        public GroupOrganizationModel GetSchHomeAdList(HeaderModel header, GroupOrganizationModel groupOrganizationModel)
        {
            groupOrganizationBiz.GetSchHomeAdList(header, groupOrganizationModel);
            return groupOrganizationModel;
        }

        [WebMethod]
        public GroupOrganizationModel GetHomePublishState(HeaderModel header, GroupOrganizationModel groupOrganizationModel)
        {
            groupOrganizationBiz.GetHomePublishState(header, groupOrganizationModel);
            return groupOrganizationModel;
        }

        [WebMethod]
        public GroupOrganizationModel SetSchHomeAdCreate(HeaderModel header, GroupOrganizationModel groupOrganizationModel)
        {
            groupOrganizationBiz.SetSchHomeAdCreate(header, groupOrganizationModel);
            return groupOrganizationModel;
        }

        [WebMethod]
        public GroupOrganizationModel SetSchHomeAdDelete(HeaderModel header, GroupOrganizationModel groupOrganizationModel)
        {
            groupOrganizationBiz.SetSchHomeAdDelete(header, groupOrganizationModel);
            return groupOrganizationModel;
        }

        [WebMethod]
        public GroupOrganizationModel SetSchHomeCommonYn(HeaderModel header, GroupOrganizationModel groupOrganizationModel)
        {
            groupOrganizationBiz.SetSchHomeCommonYn(header, groupOrganizationModel);
            return groupOrganizationModel;
        }

        [WebMethod]
        public GroupOrganizationModel SetSchHomeAdLogYn(HeaderModel header, GroupOrganizationModel groupOrganizationModel)
        {
            groupOrganizationBiz.SetSchHomeAdLogYn(header, groupOrganizationModel);
            return groupOrganizationModel;
        }

        [WebMethod]
        public GroupOrganizationModel SetSchHomeAdOrderFirst(HeaderModel header, GroupOrganizationModel groupOrganizationModel)
        {
            groupOrganizationBiz.SetSchHomeAdOrderFirst(header, groupOrganizationModel);
            return groupOrganizationModel;
        }

        [WebMethod]
        public GroupOrganizationModel SetSchHomeAdOrderUp(HeaderModel header, GroupOrganizationModel groupOrganizationModel)
        {
            groupOrganizationBiz.SetSchHomeAdOrderUp(header, groupOrganizationModel);
            return groupOrganizationModel;
        }

        [WebMethod]
        public GroupOrganizationModel SetSchHomeAdOrderDown(HeaderModel header, GroupOrganizationModel groupOrganizationModel)
        {
            groupOrganizationBiz.SetSchHomeAdOrderDown(header, groupOrganizationModel);
            return groupOrganizationModel;
        }

        [WebMethod]
        public GroupOrganizationModel SetSchHomeAdOrderLast(HeaderModel header, GroupOrganizationModel groupOrganizationModel)
        {
            groupOrganizationBiz.SetSchHomeAdOrderLast(header, groupOrganizationModel);
            return groupOrganizationModel;
        }

    }
}
