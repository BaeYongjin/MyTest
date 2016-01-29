using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;

using WinFramework.Data;
using WinFramework.Misc;
using WinFramework.Base;

using AdManagerModel;

namespace AdManagerWebService.Contract
{
    /// <summary>
    /// ChannelGroupBiz에 대한 요약 설명입니다.
    /// </summary>
    public class ChannelGroupBiz : BaseBiz
    {
        public ChannelGroupBiz() : base(FrameSystem.connDbString)
        {
            _log = FrameSystem.oLog;
        }


        /// <summary>
        /// 채널그룹목록조회
        /// </summary>
        /// <param name="channelGroupModel"></param>
        public void GetChannelGroupList(HeaderModel header, ChannelGroupModel channelGroupModel)
        {
            try
            {   // 데이터베이스를 OPEN한다
                _db.Open();
                
                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "GetChannelGroupList() Start");
                _log.Debug("-----------------------------------------");

                // __DEBUG__
                _log.Debug("<입력정보>");
                _log.Debug("SearchKey      :[" + channelGroupModel.SearchKey       + "]");
               
                // __DEBUG__

                StringBuilder sbQuery = new StringBuilder();

                // 쿼리생성
                sbQuery.Append("\n");
                sbQuery.Append("SELECT  a.MediaCode	              \n");
                sbQuery.Append("       ,b.MediaName	              \n");
                sbQuery.Append("       ,a.AdGroupCode	          \n");
                sbQuery.Append("       ,a.AdGroupName			  \n");
                sbQuery.Append("       ,a.MenuType		          \n");
                sbQuery.Append("       ,a.Comment                 \n");
                sbQuery.Append("       ,a.UseYn		              \n");
                sbQuery.Append("       ,CASE WHEN a.UseYn='Y'THEN '예' ELSE '아니오' END UseYnName         \n");
                sbQuery.Append("       ,a.RegDt		              \n");
                sbQuery.Append("       ,a.ModDt		              \n");
                sbQuery.Append("       ,c.UserName RegName        \n");
                sbQuery.Append("FROM    AdGroup a LEFT JOIN Media b ON( a.MediaCode = b.MediaCode )        \n");
                sbQuery.Append("                  LEFT JOIN SystemUser c ON( a.RegId = c.UserId )           \n");
                sbQuery.Append("WHERE   a.MenuType = '10'         \n");
                if(!channelGroupModel.MediaCode.Equals("00"))
                {
                    sbQuery.Append("  AND   a.MediaCode = '"+channelGroupModel.MediaCode+"' \n");
                }
				
                // 검색어가 있으면
                if (channelGroupModel.SearchKey.Trim().Length > 0)
                {
                    // 여러컬럼에 대하여 LIKE 검색을 한다.
                    sbQuery.Append("\n"
                        + "  AND ( a.AdGroupName LIKE '%" + channelGroupModel.SearchKey.Trim() + "%' OR		\n"
                        + "        a.Comment LIKE '%" + channelGroupModel.SearchKey.Trim() + "%'			\n"
						+ " OR b.MediaName    LIKE '%" + channelGroupModel.SearchKey.Trim() + "%' )       \n"
                        );
                }
       
                sbQuery.Append("  ORDER BY a.MediaCode,a.AdGroupCode Desc ");

                // __DEBUG__
                _log.Debug(sbQuery.ToString());
                // __DEBUG__
				
                // 쿼리실행
                DataSet ds = new DataSet();
                _db.ExecuteQuery(ds,sbQuery.ToString());

                // 결과 DataSet의 채널그룹모델에 복사
                channelGroupModel.ChannelGroupDataSet = ds.Copy();
                // 결과
                channelGroupModel.ResultCnt = Utility.GetDatasetCount(channelGroupModel.ChannelGroupDataSet);
                // 결과코드 셋트
                channelGroupModel.ResultCD = "0000";

                // __DEBUG__
                _log.Debug("<출력정보>");
                _log.Debug("ResultCnt:[" + channelGroupModel.ResultCnt + "]");
                // __DEBUG__

                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "GetChannelGroupList() End");
                _log.Debug("-----------------------------------------");
            }
            catch(Exception ex)
            {
                channelGroupModel.ResultCD = "3000";
                channelGroupModel.ResultDesc = "채널그룹정보 조회중 오류가 발생하였습니다";
                _log.Exception(ex);
            }
            finally
            {
                // 데이터베이스를  Close한다
                _db.Close();
            }

        }

        /// <summary>
        /// 채널그룹 디테일 목록조회
        /// </summary>
        /// <param name="channelGroupModel"></param>
        public void GetChannelGroupDetailList(HeaderModel header, ChannelGroupModel channelGroupModel)
        {
            try
            {   // 데이터베이스를 OPEN한다
                _db.Open();
                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "GetChannelGroupDetailList() Start");
                _log.Debug("-----------------------------------------");

                // __DEBUG__
                _log.Debug("<입력정보>");
                _log.Debug("SearchKey      :[" + channelGroupModel.SearchKey       + "]");
               
                // __DEBUG__

                StringBuilder sbQuery = new StringBuilder();

                int i = 0;
                SqlParameter[] sqlParams = new SqlParameter[2];
                // 쿼리생성
                sbQuery.Append("\n");
                sbQuery.Append("	SELECT    'False' AS CheckYn			\n");
                sbQuery.Append("			 ,b.ChannelNo                   \n");
                sbQuery.Append("	         ,b.Title                       \n");
                sbQuery.Append("	FROM      ChannelGroup a , Channel b	\n");
                sbQuery.Append("	WHERE     a.ChannelNo = b.ChannelNo     \n");
                sbQuery.Append("	  AND     a.MediaCode = @MediaCode      \n");
                sbQuery.Append("	  AND     a.AdGroupCode = @AdGroupCode  \n");
                sbQuery.Append("	GROUP BY  b.ChannelNo,b.Title           \n");
                sbQuery.Append("	ORDER BY  b.Title                       \n");                                 
      

                i = 0;
                sqlParams[i++] = new SqlParameter("@MediaCode"    , SqlDbType.TinyInt);
                sqlParams[i++] = new SqlParameter("@AdGroupCode"  , SqlDbType.Int);
                               
                i = 0;
                sqlParams[i++].Value = channelGroupModel.MediaCode;	
                sqlParams[i++].Value = channelGroupModel.AdGroupCode;

                _log.Debug("-->"+channelGroupModel.MediaCode);
                _log.Debug("-->"+channelGroupModel.AdGroupCode);
               
                // __DEBUG__
                _log.Debug(sbQuery.ToString());
                // __DEBUG__
				
                // 쿼리실행
                DataSet ds = new DataSet();
                _db.ExecuteQueryParams(ds,sbQuery.ToString(),sqlParams);

                // 결과 DataSet의 채널그룹모델에 복사
                channelGroupModel.ChannelGroupDataSet = ds.Copy();
                // 결과
                channelGroupModel.ResultCnt = Utility.GetDatasetCount(channelGroupModel.ChannelGroupDataSet);
                // 결과코드 셋트
                channelGroupModel.ResultCD = "0000";

                // __DEBUG__
                _log.Debug("<출력정보>");
                _log.Debug("ResultCnt:[" + channelGroupModel.ResultCnt + "]");
                // __DEBUG__

                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "GetChannelGroupDetailList() End");
                _log.Debug("-----------------------------------------");
            }
            catch(Exception ex)
            {
                channelGroupModel.ResultCD = "3000";
                channelGroupModel.ResultDesc = "채널그룹 디테일 정보 조회중 오류가 발생하였습니다";
                _log.Exception(ex);
            }
            finally
            {
                // 데이터베이스를  Close한다
                _db.Close();
            }

        }
        /// <summary>
        /// 채널목록조회
        /// </summary>
        /// <param name="channelGroupModel"></param>
        public void GetChannelList_p(HeaderModel header, ChannelGroupModel channelGroupModel)
        {
            try
            {   // 데이터베이스를 OPEN한다
                _db.Open();

                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "GetChannelGroupList() Start");
                _log.Debug("-----------------------------------------");

                // __DEBUG__
                _log.Debug("<입력정보>");
                _log.Debug("SearchKey      :[" + channelGroupModel.SearchKey       + "]");
               
                // __DEBUG__

                StringBuilder sbQuery = new StringBuilder();
                int i = 0;
                SqlParameter[] sqlParams = new SqlParameter[3];
                // 쿼리생성
                sbQuery.Append("\n");											 
                sbQuery.Append("   SELECT 'False' AS CheckYn                \n");
                sbQuery.Append("         ,a.ChannelNo,b.Title               \n");
                sbQuery.Append("   FROM   channelSet a, Channel b           \n");
                sbQuery.Append("   WHERE  a.ChannelNo = b.ChannelNo         \n");
                sbQuery.Append("     AND  a.MediaCode = @MediaCode          \n");
                sbQuery.Append("     AND  a.CategoryCode = @CategoryCode    \n");
                sbQuery.Append("     AND  a.GenreCode = @GenreCode          \n");
                sbQuery.Append(" GROUP BY a.ChannelNo,b.Title	            \n");
                sbQuery.Append(" ORDER BY a.ChannelNo			            \n");

                i = 0;
                sqlParams[i++] = new SqlParameter("@MediaCode"    , SqlDbType.TinyInt);
                sqlParams[i++] = new SqlParameter("@CategoryCode"    , SqlDbType.Int);
                sqlParams[i++] = new SqlParameter("@GenreCode"    , SqlDbType.Int);
                               
                i = 0;
                sqlParams[i++].Value = channelGroupModel.MediaCode;	
                sqlParams[i++].Value = channelGroupModel.CategoryCode;	
                sqlParams[i++].Value = channelGroupModel.GenreCode;	


                // __DEBUG__
                _log.Debug(sbQuery.ToString());
                // __DEBUG__
				
                // 쿼리실행
                DataSet ds = new DataSet();
                _db.ExecuteQueryParams(ds,sbQuery.ToString(),sqlParams);

                // 결과 DataSet의 채널모델에 복사
                channelGroupModel.ChannelGroupDataSet = ds.Copy();
                // 결과
                channelGroupModel.ResultCnt = Utility.GetDatasetCount(channelGroupModel.ChannelGroupDataSet);
                // 결과코드 셋트
                channelGroupModel.ResultCD = "0000";

                // __DEBUG__
                _log.Debug("<출력정보>");
                _log.Debug("ResultCnt:[" + channelGroupModel.ResultCnt + "]");
                // __DEBUG__

                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "GetChannelGroupListPopUp() End");
                _log.Debug("-----------------------------------------");
            }
            catch(Exception ex)
            {
                channelGroupModel.ResultCD = "3000";
                channelGroupModel.ResultDesc = "채널정보 조회중 오류가 발생하였습니다";
                _log.Exception(ex);
            }
            finally
            {
                // 데이터베이스를  Close한다
                _db.Close();
            }

        }

		/// <summary>
		/// 엑셀 채널목록조회
		/// </summary>
		/// <param name="channelGroupModel"></param>
		public void GetChannelList_Excel(HeaderModel header, ChannelGroupModel channelGroupModel)
		{
			try
			{   // 데이터베이스를 OPEN한다
				_db.Open();

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetChannelList_Excel() Start");
				_log.Debug("-----------------------------------------");

				// __DEBUG__
				_log.Debug("<입력정보>");
				_log.Debug("SearchKey      :[" + channelGroupModel.SearchKey       + "]");
               
				// __DEBUG__

				StringBuilder sbQuery = new StringBuilder();
				int i = 0;
				SqlParameter[] sqlParams = new SqlParameter[3];
				// 쿼리생성
				sbQuery.Append("\n");											 
				sbQuery.Append("   SELECT 'False' AS CheckYn                \n");
				sbQuery.Append("         ,a.ChannelNo,b.Title               \n");
				sbQuery.Append("   FROM   channelSet a, Channel b           \n");
				sbQuery.Append("   WHERE  a.ChannelNo = b.ChannelNo         \n");
				sbQuery.Append("     AND  a.MediaCode = @MediaCode          \n");				
				sbQuery.Append(" GROUP BY a.ChannelNo,b.Title	            \n");
				sbQuery.Append(" ORDER BY a.ChannelNo			            \n");

				i = 0;
				sqlParams[i++] = new SqlParameter("@MediaCode"    , SqlDbType.TinyInt);
                               
				i = 0;
				sqlParams[i++].Value = channelGroupModel.MediaCode;	


				// __DEBUG__
				_log.Debug(sbQuery.ToString());
				// __DEBUG__
				
				// 쿼리실행
				DataSet ds = new DataSet();
				_db.ExecuteQueryParams(ds,sbQuery.ToString(),sqlParams);

				// 결과 DataSet의 채널모델에 복사
				channelGroupModel.ChannelGroupDataSet = ds.Copy();
				// 결과
				channelGroupModel.ResultCnt = Utility.GetDatasetCount(channelGroupModel.ChannelGroupDataSet);
				// 결과코드 셋트
				channelGroupModel.ResultCD = "0000";

				// __DEBUG__
				_log.Debug("<출력정보>");
				_log.Debug("ResultCnt:[" + channelGroupModel.ResultCnt + "]");
				// __DEBUG__

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetChannelList_Excel() End");
				_log.Debug("-----------------------------------------");
			}
			catch(Exception ex)
			{
				channelGroupModel.ResultCD = "3000";
				channelGroupModel.ResultDesc = "채널정보 조회중 오류가 발생하였습니다";
				_log.Exception(ex);
			}
			finally
			{
				// 데이터베이스를  Close한다
				_db.Close();
			}

		}

   
    

        /// <summary>
        /// 채널그룹 생성
        /// </summary>

        /// <returns></returns>
        public void SetChannelGroupCreate(HeaderModel header, ChannelGroupModel channelGroupModel)
        {
            try
            {   // 데이터베이스를 OPEN한다
                _db.Open();

                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "SetChannelGroupCreate() Start");
                _log.Debug("-----------------------------------------");
                     
                
                //AdGroup AdGroupCode Query
                StringBuilder sbQueryAdGroupCode = new StringBuilder();
											 
                sbQueryAdGroupCode.Append("\n SELECT ISNULL(MAX(AdGroupCode),'0')+1 AdGroupCode FROM AdGroup ");

                // __DEBUG__
                _log.Debug(sbQueryAdGroupCode.ToString());
                // __DEBUG__
				
                // 쿼리실행
                DataSet ds = new DataSet();
                _db.ExecuteQuery(ds,sbQueryAdGroupCode.ToString());

                // DataSet의 결과를 AdGroupCode에 대입
                channelGroupModel.AdGroupCode = ds.Tables[0].Rows[0][0].ToString();
                
                //AdGroup Insert Query
                StringBuilder sbQuery = new StringBuilder();
                                                     
                sbQuery.Append("\n");
                sbQuery.Append("   INSERT INTO AdGroup			  \n");
                sbQuery.Append("              (AdGroupCode		  \n");
                sbQuery.Append("              ,AdGroupName		  \n");
                sbQuery.Append("              ,MediaCode		  \n");
                sbQuery.Append("              ,MenuType			  \n");
                sbQuery.Append("              ,Comment			  \n");
                sbQuery.Append("              ,RegDt			  \n");
                sbQuery.Append("              ,UseYn			  \n");
                sbQuery.Append("              ,ModDt			  \n");
                sbQuery.Append("              ,RegID)			  \n");
                sbQuery.Append("        VALUES(					  \n");
                sbQuery.Append("               @AdGroupCode       \n");
                sbQuery.Append("              ,@AdGroupName	      \n");
                sbQuery.Append("              ,@MediaCode		  \n");
                sbQuery.Append("              ,@MenuType		  \n");
                sbQuery.Append("              ,@Comment		      \n");
                sbQuery.Append("              ,GETDATE()		  \n");
                sbQuery.Append("              ,@UseYn			  \n");
                sbQuery.Append("              ,GETDATE()		  \n");
                sbQuery.Append("              ,@RegID)			  \n");
                                        

                //ChannelGroup InsertQuery
                StringBuilder sbQueryGenreInsert = new StringBuilder();
                                                     
                sbQueryGenreInsert.Append("\n");
                sbQueryGenreInsert.Append("   INSERT INTO  ChannelGroup         \n");
                sbQueryGenreInsert.Append("               (MediaCode          \n");
                sbQueryGenreInsert.Append("               ,ChannelNo          \n");
                sbQueryGenreInsert.Append("               ,AdGroupCode)       \n");
                sbQueryGenreInsert.Append("        VALUES                     \n");
                sbQueryGenreInsert.Append("               (@MediaCode         \n");
                sbQueryGenreInsert.Append("               ,@ChannelNo         \n");
                sbQueryGenreInsert.Append("               ,@AdGroupCode)      \n");
                                        
                // 쿼리실행
                try
                {
                                    
                    int i = 0;
                    int rc = 0;
                    //광고 그룹 Insert
                    SqlParameter[] sqlParams = new SqlParameter[10];
                    _db.BeginTran();
                    
                   
                    //메뉴타입 10:채널광고 |  20:메뉴광고
                    //메뉴 광고로 설정
                    channelGroupModel.MenuType = "10";
                   
                    i = 0;
                    sqlParams[i++] = new SqlParameter("@AdGroupCode"       , SqlDbType.Int);
                    sqlParams[i++] = new SqlParameter("@AdGroupName"       , SqlDbType.VarChar , 50);
                    sqlParams[i++] = new SqlParameter("@MediaCode"     , SqlDbType.TinyInt );
                    sqlParams[i++] = new SqlParameter("@MenuType" , SqlDbType.Char, 2);
                    sqlParams[i++] = new SqlParameter("@Comment"    , SqlDbType.VarChar , 100);
                    sqlParams[i++] = new SqlParameter("@UseYn"    , SqlDbType.Char, 1);
                    sqlParams[i++] = new SqlParameter("@RegID"    , SqlDbType.VarChar , 10 );
                                       
                                         
                                          
                    i = 0;
                    sqlParams[i++].Value = channelGroupModel.AdGroupCode;
                    sqlParams[i++].Value = channelGroupModel.AdGroupName;
                    sqlParams[i++].Value = channelGroupModel.MediaCode;
                    sqlParams[i++].Value = channelGroupModel.MenuType;
                    sqlParams[i++].Value = channelGroupModel.Comment;
                    sqlParams[i++].Value = channelGroupModel.UseYn;
                    sqlParams[i++].Value = header.UserID;
                        
                    // __DEBUG__
                    _log.Debug(sbQuery.ToString());
                    // __DEBUG__
                    
                    rc =  _db.ExecuteNonQueryParams(sbQuery.ToString(), sqlParams);
       

                    // __MESSAGE__
                    _log.Message("채널그룹정보생성:[성공]");

                    //메뉴광고 그룹 Insert
                    sqlParams = new SqlParameter[3];
                   
                    
                    for(int count=0;count < channelGroupModel.ChannelGroupDataSet.Tables["Channel"].Rows.Count;count++)

                    {
                        i = 0;
                        sqlParams[i++] = new SqlParameter("@MediaCode"     , SqlDbType.TinyInt );
                        sqlParams[i++] = new SqlParameter("@ChannelNo"    , SqlDbType.Int );
                        sqlParams[i++] = new SqlParameter("@AdGroupCode"       , SqlDbType.Int);
                                       
                                         
                                          
                        i = 0;
                        sqlParams[i++].Value = channelGroupModel.MediaCode;
                        sqlParams[i++].Value = channelGroupModel.ChannelGroupDataSet.Tables["Channel"].Rows[count][1].ToString();
                        sqlParams[i++].Value = channelGroupModel.AdGroupCode;
                    
                        // __DEBUG__
                        _log.Debug(sbQueryGenreInsert.ToString());
                        // __DEBUG__
                    
                        rc =  _db.ExecuteNonQueryParams(sbQueryGenreInsert.ToString(), sqlParams);
                    }
                    // __MESSAGE__
                    _log.Message("채널그룹정보생성:[성공]");                    

                    _db.CommitTran();

                }
                catch(Exception ex)
                {
                    _db.RollbackTran();
                    throw ex;
                }
                                        
                channelGroupModel.ResultCD = "0000";  // 정상
                                        
                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "SetChannelGroupCreate() End");
                _log.Debug("-----------------------------------------");	
                            
            }
            catch(Exception ex)
            {
                channelGroupModel.ResultCD   = "3101";
                channelGroupModel.ResultDesc = "채널그룹정보 생성 중 오류가 발생하였습니다";
                _log.Exception(ex);
            }
            finally
            {
                // 데이터베이스를  Close한다
                _db.Close();
            }
        }

        // 채널그룹정보 수정

        public void SetChannelGroupUpdate(HeaderModel header, ChannelGroupModel channelGroupModel)
        {
            try
            {   // 데이터베이스를 OPEN한다
                _db.Open();

                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "SetChannelGroupUpdate() Start");
                _log.Debug("-----------------------------------------");
                        
                StringBuilder sbQuery = new StringBuilder();
                        
                int i = 0;
                int rc = 0;
                
                        
                sbQuery.Append("\n");
                sbQuery.Append("   UPDATE  AdGroup						    \n");
                sbQuery.Append("      SET  AdGroupName = @AdGroupName	    \n");
                sbQuery.Append("          ,Comment     = @Comment		    \n");
                sbQuery.Append("          ,UseYn       = @UseYn 		    \n");
                sbQuery.Append("          ,ModDt       = GETDATE()		    \n");
                sbQuery.Append("   WHERE  MediaCode = @MediaCode           \n");
                sbQuery.Append("     AND  AdGroupCode = @AdGroupCode       \n");
                 
                //ChannelGroup Delete Query
                StringBuilder sbQueryGenreDelete = new StringBuilder();
                
                sbQueryGenreDelete.Append("\n");
                sbQueryGenreDelete.Append("DELETE  ChannelGroup         \n");
                sbQueryGenreDelete.Append(" WHERE  MediaCode = @MediaCode      \n");
                sbQueryGenreDelete.Append("   AND  AdGroupCode = @AdGroupCode  \n");
            
                //ChannelGroup InsertQuery
                StringBuilder sbQueryGenreInsert = new StringBuilder();
                                                     
                sbQueryGenreInsert.Append("\n");
                sbQueryGenreInsert.Append("   INSERT INTO  ChannelGroup         \n");
                sbQueryGenreInsert.Append("               (MediaCode          \n");
                sbQueryGenreInsert.Append("               ,ChannelNo          \n");
                sbQueryGenreInsert.Append("               ,AdGroupCode)       \n");
                sbQueryGenreInsert.Append("        VALUES                     \n");
                sbQueryGenreInsert.Append("               (@MediaCode         \n");
                sbQueryGenreInsert.Append("               ,@ChannelNo         \n");
                sbQueryGenreInsert.Append("               ,@AdGroupCode)      \n");

                // 쿼리실행
                try
                {
                    SqlParameter[] sqlParams = new SqlParameter[6];

                    _db.BeginTran();
                    //AdGroup Table Update 
                    i = 0;
                    sqlParams[i++] = new SqlParameter("@AdGroupName"   , SqlDbType.VarChar, 50);
                    sqlParams[i++] = new SqlParameter("@Comment"       , SqlDbType.VarChar, 100 );
                    sqlParams[i++] = new SqlParameter("@UseYn"         , SqlDbType.Char, 1 );
                    sqlParams[i++] = new SqlParameter("@MediaCode"     , SqlDbType.TinyInt );
                    sqlParams[i++] = new SqlParameter("@AdGroupCode"   , SqlDbType.Int);
                                       
                    i = 0;
                    sqlParams[i++].Value = channelGroupModel.AdGroupName;
                    sqlParams[i++].Value = channelGroupModel.Comment;
                    sqlParams[i++].Value = channelGroupModel.UseYn;
                    sqlParams[i++].Value = channelGroupModel.MediaCode;
                    sqlParams[i++].Value = channelGroupModel.AdGroupCode;

                    rc =  _db.ExecuteNonQueryParams(sbQuery.ToString(), sqlParams);

                    sqlParams = new SqlParameter[2];
                   
                    //ChannelGroup Table Delete
                    i = 0;
                    sqlParams[i++] = new SqlParameter("@MediaCode"     , SqlDbType.TinyInt );
                    sqlParams[i++] = new SqlParameter("@AdGroupCode"       , SqlDbType.Int);
                                       
                    i = 0;
                    sqlParams[i++].Value = channelGroupModel.MediaCode;
                    sqlParams[i++].Value = channelGroupModel.AdGroupCode;

                    rc =  _db.ExecuteNonQueryParams(sbQueryGenreDelete.ToString(), sqlParams);

                    //메뉴광고 그룹 Insert
                    sqlParams = new SqlParameter[3];
                   
                    
                    for(int count=0;count < channelGroupModel.ChannelGroupDataSet.Tables["Channel"].Rows.Count;count++)

                    {
                        i = 0;
                        sqlParams[i++] = new SqlParameter("@MediaCode"     , SqlDbType.TinyInt );
                        sqlParams[i++] = new SqlParameter("@ChannelNo"    , SqlDbType.Int );
                        sqlParams[i++] = new SqlParameter("@AdGroupCode"       , SqlDbType.Int);
                                       
                                         
                                          
                        i = 0;
                        sqlParams[i++].Value = channelGroupModel.MediaCode;
                        sqlParams[i++].Value = channelGroupModel.ChannelGroupDataSet.Tables["Channel"].Rows[count][1].ToString();
                        sqlParams[i++].Value = channelGroupModel.AdGroupCode;
                    
                        // __DEBUG__
                        _log.Debug(sbQueryGenreInsert.ToString());
                        // __DEBUG__
                    
                        rc =  _db.ExecuteNonQueryParams(sbQueryGenreInsert.ToString(), sqlParams);
                    }

                    _db.CommitTran();
                        
                    // __MESSAGE__
                    _log.Message("채널그룹정보수정:["+channelGroupModel.AdGroupCode + "] 등록자:[" + header.UserID + "]");
                        
                }
                catch(Exception ex)
                {
                    _db.RollbackTran();
                    throw ex;
                }
                        
                channelGroupModel.ResultCD = "0000";  // 정상
                        
                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "SetChannelGroupUpdate() End");
                _log.Debug("-----------------------------------------");
                        
            }
            catch(Exception ex)
            {
                channelGroupModel.ResultCD   = "3201";
                channelGroupModel.ResultDesc = "채널그룹정보 수정중 오류가 발생하였습니다";
                _log.Exception(ex);
            }
            finally
            {
                // 데이터베이스를  Close한다
                _db.Close();
            }

        }



        public void SetChannelGroupDelete(HeaderModel header, ChannelGroupModel channelGroupModel)
        {
          
        }
    }
}