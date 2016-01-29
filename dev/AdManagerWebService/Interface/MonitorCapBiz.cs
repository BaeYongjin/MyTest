using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;

using WinFramework.Data;
using WinFramework.Misc;
using WinFramework.Base;

namespace AdManagerWebService.Interface
{
    /// <summary>
    /// ReportBiz에 대한 요약 설명입니다.
    /// </summary>
    public class MonitorCapBiz : BaseBiz
    {
        public MonitorCapBiz() : base(FrameSystem.connDbString)
        {
            _log = FrameSystem.oLog;
        }

        /// <summary>
        /// 프로그램별 광고시청 집계
        /// </summary>
        public DataSet GetAdList( int userId )
        {
            DataSet ds = new DataSet();

            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "GetAdList() Start");
                _log.Debug("-----------------------------------------");
			
                #region [SQL]
                StringBuilder sb = new StringBuilder();
                sb.Append("\n select     v.ItemNo ");
                sb.Append("\n 		    ,v.ItemName ");
                sb.Append("\n 		    ,v.AdTime ");
                sb.Append("\n 		    ,v.ExcuteStartDay ");
                sb.Append("\n 		    ,v.RealEndDay ");
                sb.Append("\n 		    ,v.Menu1 ");
                sb.Append("\n 		    ,dbo.ufnGetCategoryName(1,v.Menu1)	as Menu1Name ");
                sb.Append("\n 		    ,v.Menu2 ");
                sb.Append("\n 		    ,dbo.ufnGetGenreName(1,v.Menu2)		as Menu2Name ");
                sb.Append("\n from (    select   a.ItemNo ");
                sb.Append("\n 			        ,a.ItemName ");
                sb.Append("\n 				    ,a.AdTime ");
                sb.Append("\n 				    ,substring(a.ExcuteStartDay,1,4) + '-' + substring(a.ExcuteStartDay,5,2) + '-' + substring(a.ExcuteStartDay,7,2) as ExcuteStartDay ");
                sb.Append("\n 				    ,substring(a.RealEndDay,1,4) + '-' + substring(a.RealEndDay,5,2) + '-' + substring(a.RealEndDay,7,2) as RealEndDay ");
                sb.Append("\n 				    ,isnull(b.Category	,0)	as Menu1 ");
                sb.Append("\n 				    ,isnull(b.Genre		,0)	as Menu2 ");
                sb.Append("\n 		    from	ContractItem a with(nolock) ");
                sb.Append("\n 		    left outer join SchMonitorDetail b with(nolock) on a.ItemNo = b.ItemNo and b.UserId = " + userId );
                sb.Append("\n 		    where	AdState < '40' ");
                sb.Append("\n 		    and		AdType  in('10','13','16','17','18','19') ");
                sb.Append("\n 		    and		FileState < '90' ");
                sb.Append("\n 		    and		RapCode   = 3 ) v ");
                sb.Append("\n order by v.ItemNo desc; ");
                #endregion

                #region [ 파라메터 DEBUG ]
                _log.Debug("UserId :[" + userId.ToString() + "]");
                _log.Debug("-----------------------------------------");
                _log.Debug(sb.ToString());
                _log.Debug("-----------------------------------------");
                #endregion
				
                // 쿼리실행
                _db.Open();
                _db.ExecuteQuery(ds,sb.ToString());

                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "GetAdList() End");
                _log.Debug("-----------------------------------------");

                return ds.Copy();
            }
            catch(Exception ex)
            {
                _log.Exception(ex);
                return null;
            }
            finally
            {
                _db.Close();
                if( ds != null )
                {
                    ds.Dispose();
                    ds = null;
                }
            }
        }

    }
}
