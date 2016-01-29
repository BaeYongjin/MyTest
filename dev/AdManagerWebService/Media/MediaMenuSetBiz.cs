using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using WinFramework.Data;
using WinFramework.Misc;
using WinFramework.Base;

using AdManagerModel;

namespace AdManagerWebService.Media
{
	/// <summary>
	/// MediaMenuSetBiz에 대한 요약 설명입니다.
	/// </summary>
	public class MediaMenuSetBiz : BaseBiz  
	{
        public MediaMenuSetBiz() : base(FrameSystem.connDbString)
        {
            _log = FrameSystem.oLog;
        }

        /// <summary>
        /// 편성대상업무에서 사용되는 카테고리정보를 가져온다
        /// </summary>
        /// <param name="header"></param>
        /// <param name="data"></param>
        public void GetCategoryList(HeaderModel header, MediaMenuSetModel data)
        {
            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "GetCategoryList() Start");
                _log.Debug("-----------------------------------------");

                // 데이터베이스를 OPEN한다
                _db.Open();

                StringBuilder sbQuery = new StringBuilder();
                sbQuery.Append("\n");
                sbQuery.Append(" select  a.CategoryCode" + "\n");
                sbQuery.Append("        ,a.CategoryName" + "\n");
                sbQuery.Append(" 		,( select count(*) from AdContents b with(noLock) where b.MenuLevel1 = a.CategoryCode and b.MenuLevel2 = 0 and Channel = 0 and UseYN='Y' and AdSlotCode='" + data.SearchAdType + "') as SetCategory" + "\n");
                sbQuery.Append(" 		,( select count(*) from AdContents b with(noLock) where b.MenuLevel1 = a.CategoryCode and b.MenuLevel2 > 0 and Channel = 0 and UseYN='Y' and AdSlotCode='" + data.SearchAdType + "') as SetGenre" + "\n");
                sbQuery.Append(" 		,( select count(*) from AdContents b with(noLock) where b.MenuLevel1 = a.CategoryCode and b.MenuLevel2 > 0 and Channel > 0 and UseYN='Y' and AdSlotCode='" + data.SearchAdType + "') as SetChannel" + "\n");
                sbQuery.Append(" from	Category a with(noLock)" + "\n");
                sbQuery.Append(" where	a.MediaCode = 1" + "\n");
                sbQuery.Append(" and	a.Flag = 'Y'" + "\n");
                sbQuery.Append(" and	a.CategoryCode < 9999" + "\n");
                sbQuery.Append(" order by a.SortNo" + "\n");

                // __DEBUG__
                _log.Debug("<입력정보>");
                _log.Debug("SearchKey      :[" + data.SearchAdType  + "]");
                _log.Debug(sbQuery.ToString());
                // __DEBUG__
				
                // 쿼리실행
                DataSet ds = new DataSet();
                _db.ExecuteQuery(ds,sbQuery.ToString());

                // 결과 DataSet의 매체모델에 복사
                data.CategoryDataSet    = ds.Copy();
                data.ResultCnt          = Utility.GetDatasetCount( data.CategoryDataSet );
                data.ResultCD           = "0000";

                // __DEBUG__
                _log.Debug("<출력정보>");
                _log.Debug("ResultCnt:[" + data.ResultCnt + "]");
                // __DEBUG__

                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "GetCategoryList() End");
                _log.Debug("-----------------------------------------");
            }
            catch(Exception ex)
            {
                data.ResultCD = "3000";
                data.ResultDesc = "카테고리정보 조회중 오류가 발생하였습니다";
                _log.Exception(ex);
            }
            finally
            {
                // 데이터베이스를  Close한다
                _db.Close();			
            }
        }


        /// <summary>
        /// 편성대상업무에서 사용되는 장르정보를 가져온다
        /// </summary>
        /// <param name="header"></param>
        /// <param name="data"></param>
        public void GetGenreList(HeaderModel header, MediaMenuSetModel data)
        {
            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "GetGenreList() Start");
                _log.Debug("-----------------------------------------");

                // 데이터베이스를 OPEN한다
                _db.Open();

                StringBuilder sbQuery = new StringBuilder();
                sbQuery.Append("\n");
                sbQuery.Append(" select	x.CategoryCode	as	Category" + "\n");
                sbQuery.Append("        , d.GenreCode			as	Genre" + "\n");
                sbQuery.Append(" 		, d.GenreName			as	GenreName" + "\n");
                sbQuery.Append(" 		,( select count(*) from AdContents b with(noLock) where b.MenuLevel1 = x.CategoryCode and b.MenuLevel2 = 0           and b.MenuLevel3 = 0 and Channel = 0 and UseYN='Y' and AdSlotCode=  '" + data.SearchAdType + "')   as SetCategory" + "\n");
                sbQuery.Append(" 		,( select count(*) from AdContents b with(noLock) where b.MenuLevel1 = x.CategoryCode and b.MenuLevel2 = x.GenreCode and b.MenuLevel3 = 0 and Channel = 0 and UseYN='Y' and AdSlotCode = '" + data.SearchAdType + "')	as SetGenre" + "\n");
                sbQuery.Append(" 		,( select count(*) from AdContents b with(noLock) where b.MenuLevel1 = x.CategoryCode and b.MenuLevel2 = x.GenreCode and b.MenuLevel3 = 0 and Channel > 0 and UseYN='Y' and AdSlotCode = '" + data.SearchAdType + "')	as SetChannel" + "\n");
                sbQuery.Append(" from (  select a.CategoryCode, a.GenreCode" + "\n");
                sbQuery.Append(" 		from	ChannelSet a with(noLock)" + "\n");
                sbQuery.Append(" 		inner join Channel b with(noLock) on a.MediaCode = b.MediaCode and a.ChannelNo = b.ChannelNo and a.SeriesNo = b.SeriesNo" + "\n");
                sbQuery.Append(" 		inner join Contents c with(noLock) on b.ContentId = c.ContentId and c.ContentsState <= '65' and c.SeriesId is not null" + "\n");
                sbQuery.Append(" 		where	a.CategoryCode = " + data.Category + "\n");
                sbQuery.Append(" 		group by a.CategoryCode, a.GenreCode) x" + "\n");
                sbQuery.Append(" inner join Genre d with(noLock) on d.GenreCode = x.GenreCode" + "\n");
                sbQuery.Append(" order by d.SortOrder;" + "\n");
                              

                // __DEBUG__
                _log.Debug("<입력정보>");
                _log.Debug("AdType      :[" + data.SearchAdType  + "]");
                _log.Debug("Category    :[" + data.Category  + "]");
                _log.Debug(sbQuery.ToString());
                // __DEBUG__
				
                // 쿼리실행
                DataSet ds = new DataSet();
                _db.ExecuteQuery(ds,sbQuery.ToString());

                // 결과 DataSet의 매체모델에 복사
                data.GenreDataSet   = ds.Copy();
                data.ResultCnt      = Utility.GetDatasetCount( data.GenreDataSet );
                data.ResultCD       = "0000";

                // __DEBUG__
                _log.Debug("<출력정보>");
                _log.Debug("ResultCnt:[" + data.ResultCnt + "]");
                // __DEBUG__

                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "GetGenreList() End");
                _log.Debug("-----------------------------------------");
            }
            catch(Exception ex)
            {
                data.ResultCD = "3000";
                data.ResultDesc = "장르정보 조회중 오류가 발생하였습니다";
                _log.Exception(ex);
            }
            finally
            {
                _db.Close();			
            }
        }


        /// <summary>
        /// 편성대상 카테고리를 삭제한다
        /// </summary>
        /// <param name="header"></param>
        /// <param name="data"></param>
        public void CategoryDelete(HeaderModel header, MediaMenuSetModel data)
        {
            int rc = 0;

            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "CategoryDelete() Start");
                _log.Debug("-----------------------------------------");

                StringBuilder sbQuery = new StringBuilder();
                sbQuery.Append("\n");
                sbQuery.Append(" delete from adContents" + "\n");
                sbQuery.Append(" where	MediaCode = 0" + "\n");
                sbQuery.Append(" and    AdSlotCode  = '" + data.SearchAdType + "'" + "\n");
                sbQuery.Append(" and	MenuLevel1	= " + data.Category + "\n");

                // __DEBUG__
                _log.Debug("<입력정보>");
                _log.Debug("AdType      :[" + data.SearchAdType  + "]");
                _log.Debug("Category    :[" + data.Category  + "]");
                _log.Debug(sbQuery.ToString());
                // __DEBUG__

                _db.Open();
                _db.BeginTran();
                rc =  _db.ExecuteNonQuery(sbQuery.ToString());

                if( rc > 0 )
                {
                    data.ResultDesc = "[Step1] 카테고리 삭제완료";
                    data.ResultCD   = "0000";
                    _db.CommitTran();
                }
                else
                {
                    data.ResultDesc = "[Step1] 카테고리 삭제경고, 처리된 데이터가 없습니다";
                    data.ResultCD   = "0000";
                    _db.RollbackTran();
                }
                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "CategoryDelete() End");
                _log.Debug("-----------------------------------------");
            }
            catch(Exception ex)
            {
                _db.RollbackTran();
                data.ResultCD = "3000";
                data.ResultDesc = "카테고리 편성대상 삭제중 오류가 발생하였습니다";
                _log.Exception(ex);
            }
            finally
            {
                _db.Close();			
            }
        }


        /// <summary>
        /// 편성대상 카테고리를 추가한다
        /// </summary>
        /// <param name="header"></param>
        /// <param name="data"></param>
        public void CategoryInsert(HeaderModel header, MediaMenuSetModel data)
        {
            int rc = 0;

            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "CategoryDelete() Start");
                _log.Debug("-----------------------------------------");

                StringBuilder sbQuery = new StringBuilder();
                sbQuery.Append("\n");
                sbQuery.Append(" delete from adContents" + "\n");
                sbQuery.Append(" where	MediaCode = 0" + "\n");
                sbQuery.Append(" and    AdSlotCode  = '" + data.SearchAdType + "'" + "\n");
                sbQuery.Append(" and	MenuLevel1	= " + data.Category + "\n");

                // __DEBUG__
                _log.Debug("<입력정보>");
                _log.Debug("AdType      :[" + data.SearchAdType  + "]");
                _log.Debug("Category    :[" + data.Category  + "]");
                _log.Debug(sbQuery.ToString());
                // __DEBUG__

                _db.Open();
                _db.BeginTran();
                rc =  _db.ExecuteNonQuery(sbQuery.ToString());

                sbQuery = new StringBuilder();
                sbQuery.Append(" INSERT INTO AdContents" + "\n");
                sbQuery.Append("            ([MediaCode]" + "\n");
                sbQuery.Append("            ,[AdSlotCode]" + "\n");
                sbQuery.Append("            ,[MenuLevel1]" + "\n");
                sbQuery.Append("            ,[MenuLevel2]" + "\n");
                sbQuery.Append("            ,[MenuLevel3]" + "\n");
                sbQuery.Append("            ,[MenuLevel4]" + "\n");
                sbQuery.Append("            ,[Channel]" + "\n");
                sbQuery.Append("            ,[SeriesNo]" + "\n");
                sbQuery.Append("            ,[UseYn])" + "\n");
                sbQuery.Append("      VALUES" + "\n");
                sbQuery.Append("            (0" + "\n");
                sbQuery.Append("            ,'" + data.SearchAdType + "'" + "\n");
                sbQuery.Append("            ,"  + data.Category + "\n");
                sbQuery.Append("            ,0" + "\n");
                sbQuery.Append("            ,0" + "\n");
                sbQuery.Append("            ,0" + "\n");
                sbQuery.Append("            ,0" + "\n");
                sbQuery.Append("            ,0" + "\n");
                sbQuery.Append("            ,'Y') " + "\n");
                
                rc =  _db.ExecuteNonQuery(sbQuery.ToString());

                if( rc > 0 )
                {
                    data.ResultDesc = "[Step1] 편성대상 카테고리 추가완료";
                    data.ResultCD   = "0000";
                    _db.CommitTran();
                }
                else
                {
                    data.ResultDesc = "[Step1] 카테고리 추가경고, 처리된 데이터가 없습니다";
                    data.ResultCD   = "0000";
                    _db.RollbackTran();
                }
                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "CategoryInsert() End");
                _log.Debug("-----------------------------------------");
            }
            catch(Exception ex)
            {
                _db.RollbackTran();
                data.ResultCD = "3000";
                data.ResultDesc = "카테고리 편성대상 추가중 오류가 발생하였습니다";
                _log.Exception(ex);
            }
            finally
            {
                _db.Close();			
            }
        }

        /// <summary>
        /// 편성대상 장르 삭제
        /// </summary>
        /// <param name="header"></param>
        /// <param name="data"></param>
        public void GenreDelete(HeaderModel header, MediaMenuSetModel data)
        {
            int rc = 0;

            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "GenreDelete() Start");
                _log.Debug("-----------------------------------------");

                StringBuilder sbQuery = new StringBuilder();
                sbQuery.Append("\n");
                sbQuery.Append(" delete from adContents" + "\n");
                sbQuery.Append(" where	MediaCode = 0" + "\n");
                sbQuery.Append(" and    AdSlotCode  = '" + data.SearchAdType + "'" + "\n");
                sbQuery.Append(" and	MenuLevel1	= " + data.Category + "\n");
                sbQuery.Append(" and	MenuLevel2	= " + data.Genre    + "\n");
                sbQuery.Append(" and	MenuLevel3	= 0" +  "\n");
                sbQuery.Append(" and	MenuLevel4	= 0" +  "\n");
                sbQuery.Append(" and	Channel 	= 0" +  "\n");
                sbQuery.Append(" and	SeriesNo    = 0;" +  "\n");


                // __DEBUG__
                _log.Debug("<입력정보>");
                _log.Debug("AdType      :[" + data.SearchAdType  + "]");
                _log.Debug("Category    :[" + data.Category  + "]");
                _log.Debug("Category    :[" + data.Genre  + "]");
                _log.Debug(sbQuery.ToString());
                // __DEBUG__

                _db.Open();
                _db.BeginTran();
                rc =  _db.ExecuteNonQuery(sbQuery.ToString());

                if( rc > 0 )
                {
                    data.ResultDesc = "[Step1] 카테고리 삭제완료";
                    data.ResultCD   = "0000";
                    _db.CommitTran();
                }
                else
                {
                    data.ResultDesc = "[Step1] 카테고리 삭제경고, 처리된 데이터가 없습니다";
                    data.ResultCD   = "0000";
                    _db.RollbackTran();
                }
                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "CategoryDelete() End");
                _log.Debug("-----------------------------------------");
            }
            catch(Exception ex)
            {
                _db.RollbackTran();
                data.ResultCD = "3000";
                data.ResultDesc = "카테고리 편성대상 삭제중 오류가 발생하였습니다";
                _log.Exception(ex);
            }
            finally
            {
                _db.Close();			
            }
        }


        /// <summary>
        /// 편성대상 장르 추가
        /// </summary>
        /// <param name="header"></param>
        /// <param name="data"></param>
        public void GenreInsert(HeaderModel header, MediaMenuSetModel data)
        {
            int rc = 0;

            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "GenreInsert() Start");
                _log.Debug("-----------------------------------------");

                StringBuilder sbQuery = new StringBuilder();
                // 장르추가시 상위 카테고리가 적용되어 있으면 삭제처리한다
                // 채널도 사용할 경우엔, 채널도 삭제하는 부분이 있어야 한다
                sbQuery.Append("\n");
                sbQuery.Append(" delete from adContents" + "\n");
                sbQuery.Append(" where	MediaCode = 0" + "\n");
                sbQuery.Append(" and    AdSlotCode  = '" + data.SearchAdType + "'" + "\n");
                sbQuery.Append(" and	MenuLevel1	= " + data.Category + "\n");
                sbQuery.Append(" and	MenuLevel2	= 0" +  "\n");
                sbQuery.Append(" and	MenuLevel3	= 0" +  "\n");
                sbQuery.Append(" and	MenuLevel4	= 0" +  "\n");
                sbQuery.Append(" and	Channel 	= 0" +  "\n");
                sbQuery.Append(" and	SeriesNo    = 0;" +  "\n");

                // __DEBUG__
                _log.Debug("<입력정보>");
                _log.Debug("AdType      :[" + data.SearchAdType  + "]");
                _log.Debug("Category    :[" + data.Category  + "]");
                _log.Debug("Genre       :[" + data.Genre  + "]");
                
                _log.Debug(sbQuery.ToString());
                // __DEBUG__

                _db.Open();
                _db.BeginTran();
                rc =  _db.ExecuteNonQuery(sbQuery.ToString());

                sbQuery = new StringBuilder();
                sbQuery.Append(" INSERT INTO AdContents" + "\n");
                sbQuery.Append("            ([MediaCode]" + "\n");
                sbQuery.Append("            ,[AdSlotCode]" + "\n");
                sbQuery.Append("            ,[MenuLevel1]" + "\n");
                sbQuery.Append("            ,[MenuLevel2]" + "\n");
                sbQuery.Append("            ,[MenuLevel3]" + "\n");
                sbQuery.Append("            ,[MenuLevel4]" + "\n");
                sbQuery.Append("            ,[Channel]" + "\n");
                sbQuery.Append("            ,[SeriesNo]" + "\n");
                sbQuery.Append("            ,[UseYn])" + "\n");
                sbQuery.Append("      VALUES" + "\n");
                sbQuery.Append("            (0" + "\n");
                sbQuery.Append("            ,'" + data.SearchAdType + "'" + "\n");
                sbQuery.Append("            ,"  + data.Category + "\n");
                sbQuery.Append("            ,"  + data.Genre + "\n");
                sbQuery.Append("            ,0" + "\n");
                sbQuery.Append("            ,0" + "\n");
                sbQuery.Append("            ,0" + "\n");
                sbQuery.Append("            ,0" + "\n");
                sbQuery.Append("            ,'Y') " + "\n");
                
                rc =  _db.ExecuteNonQuery(sbQuery.ToString());

                if( rc > 0 )
                {
                    data.ResultDesc = "[Step1] 편성대상 장르 추가완료";
                    data.ResultCD   = "0000";
                    _db.CommitTran();
                }
                else
                {
                    data.ResultDesc = "[Step1] 편성대상 장르 추가경고, 처리된 데이터가 없습니다";
                    data.ResultCD   = "0000";
                    _db.RollbackTran();
                }
                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "CategoryInsert() End");
                _log.Debug("-----------------------------------------");
            }
            catch(Exception ex)
            {
                _db.RollbackTran();
                data.ResultCD = "3000";
                data.ResultDesc = "[편성대상 장르 추가 오류]\n" + ex.Message;
                _log.Exception(ex);
            }
            finally
            {
                _db.Close();			
            }
        }

	}
}
