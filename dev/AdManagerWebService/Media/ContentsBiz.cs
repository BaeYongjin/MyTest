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
    /// ContentsBiz에 대한 요약 설명입니다.
    /// </summary>
    public class ContentsBiz : BaseBiz
    {
        public ContentsBiz() : base(FrameSystem.connDbString)
        {
            _log = FrameSystem.oLog;
        }

        /// <summary>
        /// 컨텐츠리스트조회 공용
        /// </summary>
        /// <param name="header"></param>
        /// <param name="contentsModel"></param>
        public void GetContentsListCommon(HeaderModel header, ContentsModel contentsModel)
        {
            try
            {   
                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "GetContentsListCommon() Start");
                _log.Debug("<입력정보>");
                _log.Debug("SearchKey      :[" + contentsModel.SearchKey       + "]");
                _log.Debug("-----------------------------------------");

                StringBuilder sb = new StringBuilder();

                sb.Append(" select	  c.CategoryCode " + "\n");
                sb.Append("         , ( select CategoryName from Category noLock where CategoryCode = c.CategoryCode) as CategoryName " + "\n");
                sb.Append(" 		, c.GenreCode " + "\n");
                sb.Append(" 		, ( select GenreName from Genre noLock where GenreCode = c.GenreCode) as GenreName " + "\n");
                sb.Append(" 		, c.ChannelNo " + "\n");
                sb.Append(" 		, c.SeriesNo " + "\n");
                sb.Append(" 		, a.Title " + "\n");
                sb.Append(" 		, a.SubTitle " + "\n");
                sb.Append(" 		, a.ContentId " + "\n");
                sb.Append(" from	Contents a with(noLock) " + "\n");
                sb.Append(" inner join Channel b with(noLock)	on b.ContentId = a.ContentId " + "\n");
                sb.Append(" inner join ChannelSet c with(noLock)  " + "\n");
                sb.Append("         on	c.MediaCode = b.MediaCode " + "\n");
                sb.Append(" 		and	c.ChannelNo	= b.ChannelNo " + "\n");
                sb.Append(" 		and	c.SeriesNo	= b.SeriesNo " + "\n");

                if (contentsModel.SearchKey.Trim().Length > 0)
                {
                    sb.Append(" where a.Title like '%" + contentsModel.SearchKey.Trim() + "%' " + "\n");
                }
                sb.Append(" order by c.CategoryCode, c.GenreCode, c.ChannelNo, c.SeriesNo " + "\n");

                // __DEBUG__
                _log.Debug(sb.ToString());
                // __DEBUG__
				
                // 쿼리실행
                DataSet ds = new DataSet();
                _db.Open();
                _db.ExecuteQuery(ds,sb.ToString());

                contentsModel.ContentsDataSet = ds.Copy();
                contentsModel.ResultCnt = Utility.GetDatasetCount(contentsModel.ContentsDataSet);
                contentsModel.ResultCD = "0000";

                // __DEBUG__
                _log.Debug("<출력정보>");
                _log.Debug("ResultCnt:[" + contentsModel.ResultCnt + "]");
                // __DEBUG__

                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "GetContentsListCommon() End");
                _log.Debug("-----------------------------------------");
            }
            catch(Exception ex)
            {
                contentsModel.ResultCD = "3000";
                contentsModel.ResultDesc = "컨텐츠정보 조회중 오류가 발생하였습니다";
                _log.Exception(ex);
            }
            finally
            {
                // 데이터베이스를  Close한다
                _db.Close();
            }

        }

        /// <summary>
        /// 컨텐츠목록조회
        /// </summary>
        /// <param name="contentsModel"></param>
        public void GetContentsList(HeaderModel header, ContentsModel contentsModel)
        {
            try
            {   // 데이터베이스를 OPEN한다
                _db.Open();

                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "GetContentsList() Start");
                _log.Debug("-----------------------------------------");

                // __DEBUG__
                _log.Debug("<입력정보>");
                _log.Debug("SearchKey      :[" + contentsModel.SearchKey       + "]");
               
                // __DEBUG__

                StringBuilder sbQuery = new StringBuilder();

                // 쿼리생성
                sbQuery.Append("\n"
                    + " SELECT ContentKey					            \n"
                    + "       ,ContentId					            \n"
                    + "       ,Title						            \n"
                    + "       ,ContentsState				            \n"
                    + "       ,convert(char(19), RegDate, 120) RegDate  \n"
                    + "       ,Rate						                \n"
                    + "       ,SubTitle                                 \n"
                    + "       ,OrgTitle                                 \n"
                    + "       ,ModDt                                    \n"
                    + "   FROM Contents with(nolock)                    \n"
                    );
				
                // 검색어가 있으면
                if (contentsModel.SearchKey.Trim().Length > 0)
                {
                    // 여러컬럼에 대하여 LIKE 검색을 한다.
                    sbQuery.Append(" Where ("
                        + "    ContentId      LIKE '%" + contentsModel.SearchKey.Trim() + "%' \n"
                        + " OR Title    LIKE '%" + contentsModel.SearchKey.Trim() + "%' \n"
                        + " OR SubTitle    LIKE '%" + contentsModel.SearchKey.Trim() + "%' \n"
                        + " OR OrgTitle    LIKE '%" + contentsModel.SearchKey.Trim() + "%' \n"
                        + " ) ");
                }
                sbQuery.Append("  ORDER BY ContentKey DESC	  ");

                // __DEBUG__
                _log.Debug(sbQuery.ToString());
                // __DEBUG__
				
                // 쿼리실행
                DataSet ds = new DataSet();
                _db.ExecuteQuery(ds,sbQuery.ToString());

                // 결과 DataSet의 컨텐츠모델에 복사
                contentsModel.ContentsDataSet = ds.Copy();
                // 결과
                contentsModel.ResultCnt = Utility.GetDatasetCount(contentsModel.ContentsDataSet);
                // 결과코드 셋트
                contentsModel.ResultCD = "0000";

                // __DEBUG__
                _log.Debug("<출력정보>");
                _log.Debug("ResultCnt:[" + contentsModel.ResultCnt + "]");
                // __DEBUG__

                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "GetContentsList() End");
                _log.Debug("-----------------------------------------");
            }
            catch(Exception ex)
            {
                contentsModel.ResultCD = "3000";
                contentsModel.ResultDesc = "컨텐츠정보 조회중 오류가 발생하였습니다";
                _log.Exception(ex);
            }
            finally
            {
                // 데이터베이스를  Close한다
                _db.Close();
            }

        }


        /// <summary>
        /// 컨텐츠목록조회
        /// </summary>
        /// <param name="contentsModel"></param>
        public void GetContentsListPopUp(HeaderModel header, ContentsModel contentsModel)
        {
            try
            {   // 데이터베이스를 OPEN한다
                _db.Open();

                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "GetContentsList() Start");
                _log.Debug("-----------------------------------------");

                // __DEBUG__
                _log.Debug("<입력정보>");
                _log.Debug("SearchKey      :[" + contentsModel.SearchKey       + "]");
               
                // __DEBUG__

                StringBuilder sbQuery = new StringBuilder();

                // 쿼리생성
                sbQuery.Append("\n"
                    + " Select                                \n"
                    + " 'False' AS CheckYn                     \n"
                    + " ,ContentId				        	  \n"
                    + " ,Title						          \n"	
                    + "   FROM Contents with(nolock)          \n"
                    );
				
                // 검색어가 있으면
                if (contentsModel.SearchKey.Trim().Length > 0)
                {
                    // 여러컬럼에 대하여 LIKE 검색을 한다.
                    sbQuery.Append(" Where ("
                        + "    ContentId      LIKE '%" + contentsModel.SearchKey.Trim() + "%' \n"
                        + " OR Title    LIKE '%" + contentsModel.SearchKey.Trim() + "%' \n"
                        + " ) ");
                }

                sbQuery.Append("  ORDER BY Title 	  ");

                // __DEBUG__
                _log.Debug(sbQuery.ToString());
                // __DEBUG__
				
                // 쿼리실행
                DataSet ds = new DataSet();
                _db.ExecuteQuery(ds,sbQuery.ToString());

                // 결과 DataSet의 컨텐츠모델에 복사
                contentsModel.ContentsDataSet = ds.Copy();
                // 결과
                contentsModel.ResultCnt = Utility.GetDatasetCount(contentsModel.ContentsDataSet);
                // 결과코드 셋트
                contentsModel.ResultCD = "0000";

                // __DEBUG__
                _log.Debug("<출력정보>");
                _log.Debug("ResultCnt:[" + contentsModel.ResultCnt + "]");
                // __DEBUG__

                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "GetContentsListPopUp() End");
                _log.Debug("-----------------------------------------");
            }
            catch(Exception ex)
            {
                contentsModel.ResultCD = "3000";
                contentsModel.ResultDesc = "컨텐츠정보 조회중 오류가 발생하였습니다";
                _log.Exception(ex);
            }
            finally
            {
                // 데이터베이스를  Close한다
                _db.Close();
            }

        }
   
        // 컨텐츠정보 저장

        public void SetContentsUpdate(HeaderModel header, ContentsModel contentsModel)
        {
            try
            {   // 데이터베이스를 OPEN한다
                _db.Open();

                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "SetContentsUpdate() Start");
                _log.Debug("-----------------------------------------");
            
                StringBuilder sbQuery = new StringBuilder();
            
                int i = 0;
                int rc = 0;
                SqlParameter[] sqlParams = new SqlParameter[10];
            
                sbQuery.Append(""
                    + "UPDATE Contents                        \n"
                    + "   SET Title = @Title                  \n"
                    + "      ,ContentsState = @ContentsState  \n"
                    + "      ,SubTitle = @SubTitle            \n"
                    + "      ,Rate = @Rate                    \n"
                    + "      ,OrgTitle = @OrgTitle            \n"
                    + "      ,ModDt = GETDATE()               \n"
                    + "WHERE  ContentKey = @ContentKey        \n"
                    ); 
            
                             
                sqlParams[i++] = new SqlParameter("@Title"         , SqlDbType.VarChar , 120);
                sqlParams[i++] = new SqlParameter("@ContentsState" , SqlDbType.TinyInt);
                sqlParams[i++] = new SqlParameter("@Rate"          , SqlDbType.TinyInt );
                sqlParams[i++] = new SqlParameter("@SubTitle"      , SqlDbType.VarChar , 40);
                sqlParams[i++] = new SqlParameter("@OrgTitle"      , SqlDbType.VarChar , 40);
                sqlParams[i++] = new SqlParameter("@ContentKey"      , SqlDbType.Int);

               
                i = 0;
                sqlParams[i++].Value = contentsModel.Title;	
                sqlParams[i++].Value = contentsModel.ContentsState;
                sqlParams[i++].Value = contentsModel.Rate;
                sqlParams[i++].Value = contentsModel.SubTitle;
                sqlParams[i++].Value = contentsModel.OrgTitle;
                sqlParams[i++].Value = contentsModel.ContentKey;
            
                            
                           

                // 쿼리실행
                try
                {
                    _db.BeginTran();
                    rc =  _db.ExecuteNonQueryParams(sbQuery.ToString(), sqlParams);
                    _db.CommitTran();
            
                    // __MESSAGE__
                    _log.Message("컨텐츠정보수정:["+contentsModel.ContentKey + "] 등록자:[" + header.UserID + "]");
            
                }
                catch(Exception ex)
                {
                    _db.RollbackTran();
                    throw ex;
                }
            
                contentsModel.ResultCD = "0000";  // 정상
            
                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "SetContentsUpdate() End");
                _log.Debug("-----------------------------------------");
            
            }
            catch(Exception ex)
            {
                contentsModel.ResultCD   = "3201";
                contentsModel.ResultDesc = "컨텐츠정보 수정중 오류가 발생하였습니다";
                _log.Exception(ex);
            }
            finally
            {
                // 데이터베이스를  Close한다
                _db.Close();
            }

        }

        /// <summary>
        /// 컨텐츠 생성
        /// </summary>

        public void SetContentsCreate(HeaderModel header, ContentsModel contentsModel)
        {
            try
            {   // 데이터베이스를 OPEN한다
                _db.Open();

                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "SetContentsCreate() Start");
                _log.Debug("-----------------------------------------");
            
                StringBuilder sbQuery = new StringBuilder();
            			
                int i = 0;
                int rc = 0;
                SqlParameter[] sqlParams = new SqlParameter[10];

                sbQuery.Append( ""
                    + "INSERT INTO Contents                     \n"
                    + "        (                                \n"
                    + "         ContentKey                      \n"
                    + "        ,ContentId                       \n"
                    + "        ,Title                           \n"
                    + "        ,ContentsState                   \n"
                    + "        ,Rate                            \n"
                    + "        ,SubTitle                        \n"
                    + "        ,OrgTitle                        \n"
                    + "        ,ModDt                           \n"
                    + "        ,RegDate                         \n"
                    + "        )                                \n"
                    + " SELECT                                  \n"
                    + "        ISNULL(Max(ContentKey),'0')+1    \n"
                    + "        ,NEWID()                         \n"
                    + "        ,@Title                          \n"
                    + "        ,@ContentsState                  \n"
                    + "        ,@Rate                           \n"
                    + "        ,@SubTitle                       \n"
                    + "        ,@OrgTitle                       \n"
                    + "        ,GETDATE()                       \n"
                    + "        ,GETDATE()                       \n"
                    + " FROM Contents with(nolock)              \n"
                    );                                       

                sqlParams[i++] = new SqlParameter("@Title"         , SqlDbType.VarChar , 120);
                sqlParams[i++] = new SqlParameter("@ContentsState" , SqlDbType.TinyInt);
                sqlParams[i++] = new SqlParameter("@Rate"          , SqlDbType.TinyInt );
                sqlParams[i++] = new SqlParameter("@SubTitle"      , SqlDbType.VarChar , 40);
                sqlParams[i++] = new SqlParameter("@OrgTitle"      , SqlDbType.VarChar , 40);
               
                i = 0;
                sqlParams[i++].Value = contentsModel.Title;	
                sqlParams[i++].Value = contentsModel.ContentsState;
                sqlParams[i++].Value = contentsModel.Rate;
                sqlParams[i++].Value = contentsModel.SubTitle;
                sqlParams[i++].Value = contentsModel.OrgTitle;
            
                // 쿼리실행
                try
                {
                    _db.BeginTran();
                    rc =  _db.ExecuteNonQueryParams(sbQuery.ToString(), sqlParams);
                    _db.CommitTran();
            
                    // __MESSAGE__
                    _log.Message("컨텐츠정보생성:[성공]");
            
                }
                catch(Exception ex)
                {
                    _db.RollbackTran();
                    throw ex;
                }
            
                contentsModel.ResultCD = "0000";  // 정상
            
                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "SetContentsCreate() End");
                _log.Debug("-----------------------------------------");	
            }
            catch(Exception ex)
            {
                contentsModel.ResultCD   = "3101";
                contentsModel.ResultDesc = "컨텐츠정보 생성 중 오류가 발생하였습니다";
                _log.Exception(ex);
            }
            finally
            {
                // 데이터베이스를  Close한다
                _db.Close();
            }
        }


        public void SetContentsDelete(HeaderModel header, ContentsModel contentsModel)
        {
            try
            {   // 데이터베이스를 OPEN한다
                _db.Open();

                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "SetContentsDelete() Start");
                _log.Debug("-----------------------------------------");
            
                StringBuilder sbQuery = new StringBuilder();
            
                int i = 0;
                int rc = 0;
                SqlParameter[] sqlParams = new SqlParameter[1];
            
                sbQuery.Append(""
                    + "DELETE Contents         \n"
                    + " WHERE ContentKey  = @ContentKey  \n"
                    );
            
                sqlParams[i++] = new SqlParameter("@ContentKey"       , SqlDbType.Int);
            
                i = 0;
                sqlParams[i++].Value = contentsModel.ContentKey;
            
                // 쿼리실행
                try
                {
                    _db.BeginTran();
                    rc =  _db.ExecuteNonQueryParams(sbQuery.ToString(), sqlParams);
                    _db.CommitTran();
            
                    // __MESSAGE__
                    _log.Message("컨텐츠정보삭제:[" + contentsModel.ContentKey + "] 등록자:[" + header.UserID + "]");
            
                }
                catch(Exception ex)
                {
                    _db.RollbackTran();
                    throw ex;
                }
            
                contentsModel.ResultCD = "0000";  // 정상
            	
                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "SetContentsDelete() End");
                _log.Debug("-----------------------------------------");	
            }
            catch(Exception ex)
            {
                contentsModel.ResultCD   = "3301";
                contentsModel.ResultDesc = "컨텐츠정보 삭제중 오류가 발생하였습니다";
                _log.Exception(ex);
            }
            finally
            {
                // 데이터베이스를  Close한다
                _db.Close();
            }

        }
    }
}
