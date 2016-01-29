using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using WinFramework.Data;
using WinFramework.Misc;
using WinFramework.Base;

using AdManagerModel;

namespace AdManagerWebService.Common
{
	/// <summary>
	/// 카테고리/장르 서비스
	/// </summary>
	public class CategoryGenreBiz : BaseBiz
	{
		public CategoryGenreBiz() : base(FrameSystem.connDbString)
		{
            _log = FrameSystem.oLog;
		}

        /// <summary>
        /// 카테고리 리스트조회
        /// </summary>
        /// <param name="header"></param>
        /// <param name="data"></param>
        public void GetCategoryList( HeaderModel header , CategoryModel data )
        {
            StringBuilder sb = new StringBuilder();
            DataSet ds = new DataSet();

            try
            {
                sb.Append("\n");
                sb.Append(" select  a.CategoryCode" + "\n");
                sb.Append("        ,a.CategoryName" + "\n");
                sb.Append(" from	Category a with(noLock)" + "\n");
                sb.Append(" where	a.MediaCode = 1" + "\n");
                sb.Append(" and     a.Flag = 'Y'" + "\n");
                sb.Append(" and	    a.CategoryCode < 9999" + "\n");
                sb.Append(" order by a.SortNo" + "\n");
                
                _db.Open();
                _db.ExecuteQuery( ds, sb.ToString() );
                data.UserDataSet = ds.Copy();
                data.ResultCD   = "0000";
                data.ResultCnt  = Utility.GetDatasetCount( data.UserDataSet );
                
            }
            catch(Exception ex)
            {
                data.ResultCD = "3000";
                data.ResultDesc = ex.Message;
                _log.Exception(ex);
            }
            finally
            {
                ds.Dispose();
                _db.Close();
            }
        }

        /// <summary>
        /// 장르리스트 조회
        /// </summary>
        /// <param name="header"></param>
        /// <param name="data"></param>
        public void GetGenreList( HeaderModel header , GenreModel data )
        {
            StringBuilder sb = new StringBuilder();
            DataSet ds = new DataSet();

            try
            {
                sb.Append("\n");
                sb.Append(" select    d.GenreCode			as	Genre" + "\n");
                sb.Append(" 		, d.GenreName			as	GenreName" + "\n");
                sb.Append(" from (  select a.CategoryCode, a.GenreCode" + "\n");
                sb.Append(" 		from	ChannelSet a with(noLock)" + "\n");
                sb.Append(" 		inner join Channel b with(noLock) on a.MediaCode = b.MediaCode and a.ChannelNo = b.ChannelNo and a.SeriesNo = b.SeriesNo" + "\n");
                sb.Append(" 		inner join Contents c with(noLock) on b.ContentId = c.ContentId and c.ContentsState <= '65' and c.SeriesId is not null" + "\n");
                sb.Append(" 		where	a.CategoryCode = " + data.CategoryCode + "\n");
                sb.Append(" 		group by a.CategoryCode, a.GenreCode) x" + "\n");
                sb.Append(" inner join Genre d with(noLock) on d.GenreCode = x.GenreCode" + "\n");
                sb.Append(" order by d.SortOrder;" + "\n");

                _db.Open();
                _db.ExecuteQuery( ds, sb.ToString() );
                data.UserDataSet = ds.Copy();
                data.ResultCD   = "0000";
                data.ResultCnt  = Utility.GetDatasetCount( data.UserDataSet );
            }
            catch(Exception ex)
            {
                data.ResultCD = "3000";
                data.ResultDesc = ex.Message;
                _log.Exception(ex);
            }
            finally
            {
                ds.Dispose();
                _db.Close();
            }
        }
	}
}
