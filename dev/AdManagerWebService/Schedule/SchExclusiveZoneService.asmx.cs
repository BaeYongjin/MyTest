using System;
using System.Data;
using System.Diagnostics;
using System.Web;
using System.Web.Services;

using WinFramework.Base;
using AdManagerModel;

namespace AdManagerWebService.Schedule
{
    /// <summary>
    /// 기본네임스페이스를 설정한다.
    /// </summary>
    [WebService(Namespace = "http://advmgt.hanafostv.com/AdManagerWebService/")]
    [System.ComponentModel.ToolboxItem(false)]

    /// <summary>
    /// SchExclusiveZoneService의 요약 설명입니다.
    /// </summary>
    public class SchExclusiveZoneService : System.Web.Services.WebService
    {
        private SchExclusiveZoneBiz schExclusiveZoneBiz = null;

        public SchExclusiveZoneService()
        {
            schExclusiveZoneBiz = new SchExclusiveZoneBiz();
        }

        /// <summary>
        /// 타겟팅정보리스트 가지고 오기 가져와서 데이타 테이블로 재정의 한다.
        /// </summary>
        [WebMethod]
        public SchExclusiveZoneModel GetSchExclusiveList(HeaderModel header, SchExclusiveZoneModel schExclusiveZoneModel)
        {
            schExclusiveZoneBiz.GetSchExclusiveList(header, schExclusiveZoneModel);
            return schExclusiveZoneModel;
        }

        /// <summary>
        /// 타겟팅정보리스트 가지고 오기 가져와서 데이타 테이블로 재정의 한다.
        /// </summary>
        [WebMethod]
        public SchExclusiveZoneModel GetTagetingList(HeaderModel header, SchExclusiveZoneModel schExclusiveZoneModel)
        {
            schExclusiveZoneBiz.GetTagetingList(header, schExclusiveZoneModel);
            return schExclusiveZoneModel;
        }

        /// <summary>
        /// 타겟팅 추가 및 업데이트 
        /// </summary>
        [WebMethod]
        public SchExclusiveZoneModel SetTargetingDetailUpdate(HeaderModel header, SchExclusiveZoneModel schExclusiveZoneModel)
        {
            schExclusiveZoneBiz.SetTargetingDetailUpdate(header, schExclusiveZoneModel);
            return schExclusiveZoneModel;
        }
        
        /// <summary>
        /// 선택한 타겟팅
        /// </summary>
        [WebMethod]
        public SchExclusiveZoneModel GetTimeTargetDetail(HeaderModel header, SchExclusiveZoneModel schExclusiveZoneModel)
        {
            schExclusiveZoneBiz.GetTimeTargetDetail(header, schExclusiveZoneModel);
            return schExclusiveZoneModel;
        }

        /// <summary>
        /// 선택된 시간대 독점 편성
        /// </summary>
        [WebMethod]
        public SchExclusiveZoneModel SetSchExclusivUpdate(HeaderModel header, SchExclusiveZoneModel schExclusiveZoneModel)
        {
            schExclusiveZoneBiz.SetSchExclusivUpdate(header, schExclusiveZoneModel);
            return schExclusiveZoneModel;
        }
        
    }
}
