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
    /// GenreGroupBiz에 대한 요약 설명입니다.
    /// </summary>
    public class GenreGroupBiz : BaseBiz
    {
        public GenreGroupBiz() : base(FrameSystem.connDbString, true)
        {
            _log = FrameSystem.oLog;
        }


        /// <summary>
        /// 장르그룹목록조회
        /// </summary>
        /// <param name="genreGroupModel"></param>
        public void GetGenreGroupList(HeaderModel header, GenreGroupModel genreGroupModel)
        {
            try
            {   
                // 데이터베이스를 OPEN한다
                _db.Open();

                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "GetGenreGroupList() Start");
                _log.Debug("-----------------------------------------");

                // __DEBUG__
                _log.Debug("<입력정보>");
                _log.Debug("SearchKey      :[" + genreGroupModel.SearchKey       + "]");
               
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
                sbQuery.Append("       ,CASE WHEN a.UseYn='Y'THEN '예' ELSE '아니오' END UseYnName   \n");
                sbQuery.Append("       ,a.RegDt		              \n");
                sbQuery.Append("       ,a.ModDt		              \n");
                sbQuery.Append("       ,c.UserName RegName        \n");
                sbQuery.Append("FROM    AdGroup a LEFT JOIN Media b ON( a.MediaCode = b.MediaCode )  \n");
                sbQuery.Append("                  LEFT JOIN SystemUser c ON( a.RegId = c.UserId )     \n");
                sbQuery.Append("WHERE   a.MenuType = '20'         \n");
                if(!genreGroupModel.MediaCode.Equals("00"))
                {
                    sbQuery.Append("  AND   a.MediaCode = '"+genreGroupModel.MediaCode+"' \n");
                }
				
                // 검색어가 있으면
                if (genreGroupModel.SearchKey.Trim().Length > 0)
                {
                    // 여러컬럼에 대하여 LIKE 검색을 한다.
                    sbQuery.Append("\n"
                        + "  AND ( a.AdGroupName LIKE '%" + genreGroupModel.SearchKey.Trim() + "%' OR		\n"
                        + "        a.Comment LIKE '%" + genreGroupModel.SearchKey.Trim() + "%'             \n"
                        + " OR b.MediaName    LIKE '%" + genreGroupModel.SearchKey.Trim() + "%' )       \n"
                        );
                }
       
                sbQuery.Append("  ORDER BY   a.MediaCode,a.AdGroupCode Desc ");

                // __DEBUG__
                _log.Debug(sbQuery.ToString());
                // __DEBUG__
				
                // 쿼리실행
                DataSet ds = new DataSet();
                _db.ExecuteQuery(ds,sbQuery.ToString());

                // 결과 DataSet의 장르그룹모델에 복사
                genreGroupModel.GenreGroupDataSet = ds.Copy();
                // 결과
                genreGroupModel.ResultCnt = Utility.GetDatasetCount(genreGroupModel.GenreGroupDataSet);
                // 결과코드 셋트
                genreGroupModel.ResultCD = "0000";

                // __DEBUG__
                _log.Debug("<출력정보>");
                _log.Debug("ResultCnt:[" + genreGroupModel.ResultCnt + "]");
                // __DEBUG__

                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "GetGenreGroupList() End");
                _log.Debug("-----------------------------------------");
            }
            catch(Exception ex)
            {
                genreGroupModel.ResultCD = "3000";
                genreGroupModel.ResultDesc = "장르그룹정보 조회중 오류가 발생하였습니다";
                _log.Exception(ex);
            }
            finally
            {
                // 데이터베이스를  Close한다
                _db.Close();
            }

        }

        /// <summary>
        /// 장르그룹 디테일 목록조회
        /// </summary>
        /// <param name="genreGroupModel"></param>
        public void GetGenreGroupDetailList(HeaderModel header, GenreGroupModel genreGroupModel)
        {
            try
            {   
                // 데이터베이스를 OPEN한다
                _db.Open();

                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "GetGenreGroupDetailList() Start");
                _log.Debug("-----------------------------------------");

                // __DEBUG__
                _log.Debug("<입력정보>");
                _log.Debug("SearchKey      :[" + genreGroupModel.SearchKey       + "]");
               
                // __DEBUG__

                StringBuilder sbQuery = new StringBuilder();

                int i = 0;
                SqlParameter[] sqlParams = new SqlParameter[2];
                // 쿼리생성
                sbQuery.Append("\n");
                sbQuery.Append("SELECT   'False' AS CheckYn            \n");
                sbQuery.Append("        ,b.GenreCode                   \n");
                sbQuery.Append("        ,b.GenreName                   \n");
                sbQuery.Append("FROM     GenreGroup a , Genre b        \n");
                sbQuery.Append("WHERE    a.GenreCode = b.GenreCode     \n");
                sbQuery.Append("  AND    a.MediaCode = @MediaCode      \n");
                sbQuery.Append("  AND    a.AdGroupCode = @AdGroupCode  \n");
                sbQuery.Append("ORDER BY a.GenreCode                   \n");
                                                                      
      

                i = 0;
                sqlParams[i++] = new SqlParameter("@MediaCode"    , SqlDbType.TinyInt);
                sqlParams[i++] = new SqlParameter("@AdGroupCode"  , SqlDbType.Int);
                               
                i = 0;
                sqlParams[i++].Value = genreGroupModel.MediaCode;	
                sqlParams[i++].Value = genreGroupModel.AdGroupCode;


                // __DEBUG__
                _log.Debug(sbQuery.ToString());
                // __DEBUG__
				
                // 쿼리실행
                DataSet ds = new DataSet();
                _db.ExecuteQueryParams(ds,sbQuery.ToString(),sqlParams);

                // 결과 DataSet의 장르그룹모델에 복사
                genreGroupModel.GenreGroupDataSet = ds.Copy();
                // 결과
                genreGroupModel.ResultCnt = Utility.GetDatasetCount(genreGroupModel.GenreGroupDataSet);
                // 결과코드 셋트
                genreGroupModel.ResultCD = "0000";

                // __DEBUG__
                _log.Debug("<출력정보>");
                _log.Debug("ResultCnt:[" + genreGroupModel.ResultCnt + "]");
                // __DEBUG__

                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "GetGenreGroupDetailList() End");
                _log.Debug("-----------------------------------------");
            }
            catch(Exception ex)
            {
                genreGroupModel.ResultCD = "3000";
                genreGroupModel.ResultDesc = "장르그룹 디테일 정보 조회중 오류가 발생하였습니다";
                _log.Exception(ex);
            }
            finally
            {
                // 데이터베이스를  Close한다
                _db.Close();
            }

        }

		/// <summary>
		/// 장르목록조회(엑셀)
		/// </summary>
		/// <param name="genreGroupModel"></param>
		public void GetInspectGenreGroupList_p(HeaderModel header, GenreGroupModel genreGroupModel)
		{
			try
			{   // 데이터베이스를 OPEN한다
				_db.Open();

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetGenreGroupList() Start");
				_log.Debug("-----------------------------------------");

				// __DEBUG__
				_log.Debug("<입력정보>");
				_log.Debug("SearchKey      :[" + genreGroupModel.SearchKey       + "]");
               
				// __DEBUG__

				StringBuilder sbQuery = new StringBuilder();
				
				// 쿼리생성
				sbQuery.Append("\n SELECT  'False' AS CheckYn");
				sbQuery.Append("\n     ,   1       AS MediaCode");
				sbQuery.Append("\n     ,   A.MENU_COD_PAR  AS CategoryCode");
				sbQuery.Append("\n     ,   B.MENU_NM       AS CategoryName_r");
				sbQuery.Append("\n     ,   A.MENU_COD_PAR || ' ' || B.MENU_NM AS CategoryName");
				sbQuery.Append("\n     ,   A.MENU_COD      AS GenreCode");
				sbQuery.Append("\n     ,   A.MENU_NM       AS GenreName  ");
				sbQuery.Append("\n FROM    MENU_COD A");
				sbQuery.Append("\n     INNER JOIN MENU_COD B ON B.MENU_COD = A.MENU_COD_PAR");
				sbQuery.Append("\n WHERE   A.MENU_LVL = 3");
				
				// 검색어가 있으면
				if (genreGroupModel.SearchKey.Trim().Length > 0)
				{
					sbQuery.Append("   AND ( B.MENU_NM	LIKE '%" + genreGroupModel.SearchKey.Trim() + "%' \n");
					sbQuery.Append("   OR    A.MENU_NM  LIKE '%" + genreGroupModel.SearchKey.Trim() + "%' )       \n");
				}
				sbQuery.Append("   ORDER BY A.MENU_ORD");
				
				// __DEBUG__
				_log.Debug(sbQuery.ToString());
				// __DEBUG__
				
				// 쿼리실행
				DataSet ds = new DataSet();
				_db.ExecuteQuery(ds,sbQuery.ToString());

				// 결과 DataSet의 장르모델에 복사
				genreGroupModel.GenreGroupDataSet = ds.Copy();
				genreGroupModel.ResultCnt = Utility.GetDatasetCount(genreGroupModel.GenreGroupDataSet);
				genreGroupModel.ResultCD = "0000";

				// __DEBUG__
				_log.Debug("<출력정보>");
				_log.Debug("ResultCnt:[" + genreGroupModel.ResultCnt + "]");
				// __DEBUG__

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetGenreGroupListPopUp() End");
				_log.Debug("-----------------------------------------");
			}
			catch(Exception ex)
			{
				genreGroupModel.ResultCD = "3000";
				genreGroupModel.ResultDesc = "장르정보 조회중 오류가 발생하였습니다";
				_log.Exception(ex);
			}
			finally
			{
				// 데이터베이스를  Close한다
				_db.Close();
			}

		}

        /// <summary>
        /// 장르목록조회
        /// </summary>
        /// <param name="genreGroupModel"></param>
        public void GetGenreGroupList_p(HeaderModel header, GenreGroupModel genreGroupModel)
        {
            try
            {   // 데이터베이스를 OPEN한다
                _db.Open();

                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "GetGenreGroupList() Start");
                _log.Debug("-----------------------------------------");
                _log.Debug("<입력정보>");
                _log.Debug("SearchKey      :[" + genreGroupModel.SearchKey       + "]");
               
			
				StringBuilder sbQuery = new StringBuilder();
				#region 메뉴가져오기
                // 쿼리 변경 2013/07/13일 장용석
				sbQuery.Append("\n SELECT  'False'								AS CheckYn		");
				sbQuery.Append("\n     ,   1									AS MediaCode	");
				sbQuery.Append("\n     ,   A.MENU_COD_PAR						AS CategoryCode	");
				sbQuery.Append("\n     ,   A.MENU_COD_PAR || ' ' || B.MENU_NM	AS CategoryName	");
				sbQuery.Append("\n     ,   A.MENU_COD							AS GenreCode	");
				sbQuery.Append("\n     ,   A.MENU_COD     || ' ' || A.MENU_NM   AS GenreName	");
				sbQuery.Append("\n FROM    MENU_COD A											");
				sbQuery.Append("\n     INNER JOIN MENU_COD B ON B.MENU_COD = A.MENU_COD_PAR		");
				sbQuery.Append("\n WHERE   A.MENU_LVL = 3	");
				sbQuery.Append("   ORDER BY A.MENU_ORD		");
				
                // __DEBUG__
                _log.Debug(sbQuery.ToString());
                // __DEBUG__
				
                // 쿼리실행
                DataSet ds = new DataSet();
                _db.ExecuteQuery(ds,sbQuery.ToString());
                genreGroupModel.GenreGroupDataSet = ds.Copy();
				#endregion

				// 결과
                genreGroupModel.ResultCnt = Utility.GetDatasetCount(genreGroupModel.GenreGroupDataSet);
                genreGroupModel.ResultCD = "0000";

                // __DEBUG__
                _log.Debug("<출력정보>");
                _log.Debug("ResultCnt:[" + genreGroupModel.ResultCnt + "]");
                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "GetGenreGroupListPopUp() End");
                _log.Debug("-----------------------------------------");
            }
            catch(Exception ex)
            {
                genreGroupModel.ResultCD = "3000";
                genreGroupModel.ResultDesc = "장르정보 조회중 오류가 발생하였습니다";
                _log.Exception(ex);
            }
            finally
            {
                _db.Close();
            }
        }

		/// <summary>
		/// 실시간채널목록을 장르형식으로 가져온다
		/// </summary>
		/// <param name="header"></param>
		/// <param name="genreGroupModel"></param>
		public void GetChannelList_p(HeaderModel header, GenreGroupModel genreGroupModel)
		{
			try
			{   // 데이터베이스를 OPEN한다
				_db.Open();

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetChannelList_p() Start");
				_log.Debug("-----------------------------------------");
				_log.Debug("<입력정보>");
				_log.Debug("SearchKey      :[" + genreGroupModel.SearchKey + "]");


				StringBuilder sbQuery = new StringBuilder();
				#region 메뉴가져오기
				sbQuery.Append("\n SELECT  'False'						AS  CheckYn			");
				sbQuery.Append("\n     ,   1							AS  MediaCode		");
				sbQuery.Append("\n     ,   B.GNR_COD					AS  CategoryCode	");
				sbQuery.Append("\n     ,   C.STM_COD_NM					AS CategoryName		");
				sbQuery.Append("\n     ,   B.CH_NO						AS GenreCode		");
				sbQuery.Append("\n     ,   B.CH_NO || ' ' || B.CH_NM    AS GenreName		");
				sbQuery.Append("\n FROM    CHNL_COD B										");
				sbQuery.Append("\n     LEFT  JOIN STM_COD  C ON C.STM_COD = B.GNR_COD AND C.STM_COD_CLS = '78'	");
				sbQuery.Append("\n ORDER BY B.GNR_COD, B.CH_NO													");

				// __DEBUG__
				_log.Debug(sbQuery.ToString());
				// __DEBUG__

				// 쿼리실행
				DataSet ds = new DataSet();
				_db.ExecuteQuery(ds, sbQuery.ToString());
				genreGroupModel.GenreGroupDataSet = ds.Copy();
				#endregion

				// 결과
				genreGroupModel.ResultCnt = Utility.GetDatasetCount(genreGroupModel.GenreGroupDataSet);
				genreGroupModel.ResultCD = "0000";

				// __DEBUG__
				_log.Debug("<출력정보>");
				_log.Debug("ResultCnt:[" + genreGroupModel.ResultCnt + "]");
				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetChannelList_p() End");
				_log.Debug("-----------------------------------------");
			}
			catch (Exception ex)
			{
				genreGroupModel.ResultCD = "3000";
				genreGroupModel.ResultDesc = "실시간채널정보 조회중 오류가 발생하였습니다";
				_log.Exception(ex);
			}
			finally
			{
				_db.Close();
			}
		}

        /// <summary>
        /// 장르그룹 생성
        /// </summary>
        /// <param name="UserID"></param>
        /// <param name="UserName"></param>
        /// <param name="UserPassword"></param>
        /// <param name="UserLevel"></param>
        /// <param name="UserDept"></param>
        /// <param name="UserTitle"></param>
        /// <param name="UserTell"></param>
        /// <param name="UserMobile"></param>
        /// <param name="UserComment"></param>
        /// <returns></returns>
        public void SetGenreGroupCreate(HeaderModel header, GenreGroupModel genreGroupModel)
        {
            try
            {   
                // 데이터베이스를 OPEN한다
                _db.Open();

                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "SetGenreGroupCreate() Start");
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
                genreGroupModel.AdGroupCode = ds.Tables[0].Rows[0][0].ToString();
                
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
                sbQuery.Append("              ,ModDt			  \n");
                sbQuery.Append("              ,UseYn			  \n");
                sbQuery.Append("              ,RegID)			  \n");
                sbQuery.Append("        VALUES(					  \n");
                sbQuery.Append("               @AdGroupCode       \n");
                sbQuery.Append("              ,@AdGroupName	      \n");
                sbQuery.Append("              ,@MediaCode		  \n");
                sbQuery.Append("              ,@MenuType		  \n");
                sbQuery.Append("              ,@Comment		      \n");
                sbQuery.Append("              ,GETDATE()		  \n");
                sbQuery.Append("              ,GETDATE()		  \n");
                sbQuery.Append("              ,@UseYn			  \n");
                sbQuery.Append("              ,@RegID)			  \n");
                                        

                //GenreGroup InsertQuery
                StringBuilder sbQueryGenreInsert = new StringBuilder();
                                                     
                sbQueryGenreInsert.Append("\n");
                sbQueryGenreInsert.Append("   INSERT INTO  GenreGroup         \n");
                sbQueryGenreInsert.Append("               (MediaCode          \n");
                sbQueryGenreInsert.Append("               ,GenreCode          \n");
                sbQueryGenreInsert.Append("               ,AdGroupCode)       \n");
                sbQueryGenreInsert.Append("        VALUES                     \n");
                sbQueryGenreInsert.Append("               (@MediaCode         \n");
                sbQueryGenreInsert.Append("               ,@GenreCode         \n");
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
                    genreGroupModel.MenuType = "20";
                   
                    i = 0;
                    sqlParams[i++] = new SqlParameter("@AdGroupCode"       , SqlDbType.Int);
                    sqlParams[i++] = new SqlParameter("@AdGroupName"       , SqlDbType.VarChar , 50);
                    sqlParams[i++] = new SqlParameter("@MediaCode"     , SqlDbType.TinyInt );
                    sqlParams[i++] = new SqlParameter("@MenuType" , SqlDbType.Char, 2);
                    sqlParams[i++] = new SqlParameter("@Comment"    , SqlDbType.VarChar , 100);
                    sqlParams[i++] = new SqlParameter("@UseYn"    , SqlDbType.Char, 1);
                    sqlParams[i++] = new SqlParameter("@RegID"    , SqlDbType.VarChar , 10 );
                                       
                                         
                                          
                    i = 0;
                    sqlParams[i++].Value = genreGroupModel.AdGroupCode;
                    sqlParams[i++].Value = genreGroupModel.AdGroupName;
                    sqlParams[i++].Value = genreGroupModel.MediaCode;
                    sqlParams[i++].Value = genreGroupModel.MenuType;
                    sqlParams[i++].Value = genreGroupModel.Comment;
                    sqlParams[i++].Value = genreGroupModel.UseYn;
                    sqlParams[i++].Value = header.UserID;
                        
                    // __DEBUG__
                    _log.Debug(sbQuery.ToString());
                    // __DEBUG__
                    
                    rc =  _db.ExecuteNonQueryParams(sbQuery.ToString(), sqlParams);
       

                    // __MESSAGE__
                    _log.Message("장르그룹정보생성:[성공]");

                    //메뉴광고 그룹 Insert
                    sqlParams = new SqlParameter[3];
                   
                    
                    for(int count=0;count < genreGroupModel.GenreGroupDataSet.Tables["Genre"].Rows.Count;count++)

                    {
                        i = 0;
                        sqlParams[i++] = new SqlParameter("@MediaCode"     , SqlDbType.TinyInt );
                        sqlParams[i++] = new SqlParameter("@GenreCode"    , SqlDbType.Int );
                        sqlParams[i++] = new SqlParameter("@AdGroupCode"       , SqlDbType.Int);
                                       
                                         
                                          
                        i = 0;
                        sqlParams[i++].Value = genreGroupModel.MediaCode;
                        sqlParams[i++].Value = genreGroupModel.GenreGroupDataSet.Tables["Genre"].Rows[count][1].ToString();
                        sqlParams[i++].Value = genreGroupModel.AdGroupCode;
                    
                        // __DEBUG__
                        _log.Debug(sbQueryGenreInsert.ToString());
                        // __DEBUG__
                    
                        rc =  _db.ExecuteNonQueryParams(sbQueryGenreInsert.ToString(), sqlParams);
                    }
                    // __MESSAGE__
                    _log.Message("장르그룹정보생성:[성공]");                    

                    _db.CommitTran();

                }
                catch(Exception ex)
                {
                    _db.RollbackTran();
                    throw ex;
                }
                                        
                genreGroupModel.ResultCD = "0000";  // 정상
                                        
                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "SetGenreGroupCreate() End");
                _log.Debug("-----------------------------------------");	
                            
            }
            catch(Exception ex)
            {
                genreGroupModel.ResultCD   = "3101";
                genreGroupModel.ResultDesc = "장르그룹정보 생성 중 오류가 발생하였습니다";
                _log.Exception(ex);
            }
            finally
            {
                // 데이터베이스를  Close한다
                _db.Close();
            }
        }

        // 장르그룹정보 수정

        public void SetGenreGroupUpdate(HeaderModel header, GenreGroupModel genreGroupModel)
        {
            try
            {   // 데이터베이스를 OPEN한다
                _db.Open();

                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "SetGenreGroupUpdate() Start");
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
                 
                //GenreGroup Delete Query
                StringBuilder sbQueryGenreDelete = new StringBuilder();
                
                sbQueryGenreDelete.Append("\n");
                sbQueryGenreDelete.Append("DELETE  GenreGroup         \n");
                sbQueryGenreDelete.Append(" WHERE  MediaCode = @MediaCode      \n");
                sbQueryGenreDelete.Append("   AND  AdGroupCode = @AdGroupCode  \n");
            
                //GenreGroup InsertQuery
                StringBuilder sbQueryGenreInsert = new StringBuilder();
                                                     
                sbQueryGenreInsert.Append("\n");
                sbQueryGenreInsert.Append("   INSERT INTO  GenreGroup         \n");
                sbQueryGenreInsert.Append("               (MediaCode          \n");
                sbQueryGenreInsert.Append("               ,GenreCode          \n");
                sbQueryGenreInsert.Append("               ,AdGroupCode)       \n");
                sbQueryGenreInsert.Append("        VALUES                     \n");
                sbQueryGenreInsert.Append("               (@MediaCode         \n");
                sbQueryGenreInsert.Append("               ,@GenreCode         \n");
                sbQueryGenreInsert.Append("               ,@AdGroupCode)      \n");

                // 쿼리실행
                try
                {   // 데이터베이스를 OPEN한다
                    _db.Open();
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
                    sqlParams[i++].Value = genreGroupModel.AdGroupName;
                    sqlParams[i++].Value = genreGroupModel.Comment;
                    sqlParams[i++].Value = genreGroupModel.UseYn;
                    sqlParams[i++].Value = genreGroupModel.MediaCode;
                    sqlParams[i++].Value = genreGroupModel.AdGroupCode;

                    rc =  _db.ExecuteNonQueryParams(sbQuery.ToString(), sqlParams);

                    sqlParams = new SqlParameter[2];
                   
                    //GenreGroup Table Delete
                    i = 0;
                    sqlParams[i++] = new SqlParameter("@MediaCode"     , SqlDbType.TinyInt );
                    sqlParams[i++] = new SqlParameter("@AdGroupCode"       , SqlDbType.Int);
                                       
                    i = 0;
                    sqlParams[i++].Value = genreGroupModel.MediaCode;
                    sqlParams[i++].Value = genreGroupModel.AdGroupCode;

                    rc =  _db.ExecuteNonQueryParams(sbQueryGenreDelete.ToString(), sqlParams);

                    //메뉴광고 그룹 Insert
                    sqlParams = new SqlParameter[3];
                   
                    
                    for(int count=0;count < genreGroupModel.GenreGroupDataSet.Tables["Genre"].Rows.Count;count++)

                    {
                        i = 0;
                        sqlParams[i++] = new SqlParameter("@MediaCode"     , SqlDbType.TinyInt );
                        sqlParams[i++] = new SqlParameter("@GenreCode"    , SqlDbType.Int );
                        sqlParams[i++] = new SqlParameter("@AdGroupCode"       , SqlDbType.Int);
                                       
                                         
                                          
                        i = 0;
                        sqlParams[i++].Value = genreGroupModel.MediaCode;
                        sqlParams[i++].Value = genreGroupModel.GenreGroupDataSet.Tables["Genre"].Rows[count][1].ToString();
                        sqlParams[i++].Value = genreGroupModel.AdGroupCode;
                    
                        // __DEBUG__
                        _log.Debug(sbQueryGenreInsert.ToString());
                        // __DEBUG__
                    
                        rc =  _db.ExecuteNonQueryParams(sbQueryGenreInsert.ToString(), sqlParams);
                    }

                    _db.CommitTran();
                        
                    // __MESSAGE__
                    _log.Message("장르그룹정보수정:["+genreGroupModel.AdGroupCode + "] 등록자:[" + header.UserID + "]");
                        
                }
                catch(Exception ex)
                {
                    _db.RollbackTran();
                    throw ex;
                }
                        
                genreGroupModel.ResultCD = "0000";  // 정상
                        
                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "SetGenreGroupUpdate() End");
                _log.Debug("-----------------------------------------");
                        
            }
            catch(Exception ex)
            {
                genreGroupModel.ResultCD   = "3201";
                genreGroupModel.ResultDesc = "장르그룹정보 수정중 오류가 발생하였습니다";
                _log.Exception(ex);
            }
            finally
            {
                // 데이터베이스를  Close한다
                _db.Close();
            }

        }



        public void SetGenreGroupDelete(HeaderModel header, GenreGroupModel genreGroupModel)
        {

        }
    }
}