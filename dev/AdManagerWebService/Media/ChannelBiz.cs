/*
 * -------------------------------------------------------
 * Class Name: ChannelBiz
 * 주요기능  : 실시간채널정보 관리
 * 작성자    : 
 * 작성일    : 2015
 * 특이사항  : 실시간채널정보만 관리 함.
 * -------------------------------------------------------
 */
using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;

using Oracle.DataAccess;
using Oracle.DataAccess.Client;
using Oracle.DataAccess.Types;

using WinFramework.Data;
using WinFramework.Misc;
using WinFramework.Base;

using AdManagerModel;

namespace AdManagerWebService.Media
{
    /// <summary>
    /// ChannelBiz에 대한 요약 설명입니다.
    /// </summary>
    public class ChannelBiz : BaseBiz
    {
        public ChannelBiz() : base(FrameSystem.connDbString, true)
        {
            _log = FrameSystem.oLog;
        }


        /// <summary>
        /// 장르정보가져오는 것임
        /// </summary>
        /// <param name="channelModel"></param>
        public void GetChannelList(HeaderModel header, ChannelModel channelModel)
        {
            try
            {   // 데이터베이스를 OPEN한다
                _db.Open();

                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "GetChannelList() Start");
                _log.Debug("-----------------------------------------");

                // __DEBUG__
                _log.Debug("<입력정보>");
                _log.Debug("SearchKey      :[" + channelModel.SearchKey       + "]");
               
                // __DEBUG__

                StringBuilder sbQuery = new StringBuilder();

                // 쿼리생성
                // stm_cod 에서 채널장르정보가 있는 코드만 가져옴.
                sbQuery.AppendLine(" SELECT                 ");
                sbQuery.AppendLine("      stm_cod           ");
                sbQuery.AppendLine("     ,stm_cod_cls       ");
                sbQuery.AppendLine("     ,stm_cod_nm        ");
                sbQuery.AppendLine(" FROM STM_COD           ");
                sbQuery.AppendLine(" WHERE 1=1              ");
                sbQuery.AppendLine(" AND stm_cod_cls = '78' ");
                sbQuery.AppendLine(" AND stm_cod >'00'      ");
                sbQuery.AppendLine(" ORDER BY stm_cod_nm    ");
    
                // __DEBUG__
                _log.Debug(sbQuery.ToString());
                // __DEBUG__
				
                // 쿼리실행
                DataSet ds = new DataSet();
                _db.ExecuteQuery(ds,sbQuery.ToString());

                // 결과 DataSet의 채널모델에 복사
                channelModel.ChannelDataSet = ds.Copy();
                // 결과
                channelModel.ResultCnt = Utility.GetDatasetCount(channelModel.ChannelDataSet);
                // 결과코드 셋트
                channelModel.ResultCD = "0000";

                // __DEBUG__
                _log.Debug("<출력정보>");
                _log.Debug("ResultCnt:[" + channelModel.ResultCnt + "]");
                // __DEBUG__

                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "GetChannelList() End");
                _log.Debug("-----------------------------------------");
            }
            catch(Exception ex)
            {
                channelModel.ResultCD = "3000";
                channelModel.ResultDesc = "채널장르 조회중 오류가 발생하였습니다";
                _log.Exception(ex);
            }
            finally
            {
                // 데이터베이스를  Close한다
                _db.Close();
            }

        }

        /// <summary>
        /// 채널장르의 실시간채널정보
        /// </summary>
        /// <param name="channelModel"></param>
        public void GetChannelDetailList(HeaderModel header, ChannelModel channelModel)
        {
            try
            {   // 데이터베이스를 OPEN한다
                _db.Open();

                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "GetChannelDetailList() Start");
                _log.Debug("-----------------------------------------");

                // __DEBUG__
                _log.Debug("<입력정보>");

                _log.Debug("GenreCode :[" + channelModel.ChannelNo + "]");
                // __DEBUG__

                StringBuilder sbQuery = new StringBuilder();

                int i = 0;
                OracleParameter[] sqlParams = new OracleParameter[1];
                // 쿼리생성               
                sbQuery.AppendLine(" SELECT         ");
                sbQuery.AppendLine("        svc_id  ");
                sbQuery.AppendLine("       ,ch_no   ");
                sbQuery.AppendLine("       ,ch_nm   ");
                sbQuery.AppendLine("       ,gnr_cod ");
                sbQuery.AppendLine("       ,ch_rank ");
                sbQuery.AppendLine("       ,use_yn  ");
                sbQuery.AppendLine("       ,ad_yn   ");
                sbQuery.AppendLine("       ,ad_rate ");
                sbQuery.AppendLine("       ,adn_rate ");
                sbQuery.AppendLine(" FROM CHNL_COD  ");
                sbQuery.AppendLine(" WHERE 1=1      ");    
                sbQuery.AppendLine(" AND GNR_COD = :GenreCode ");
                
                if (channelModel.CheckYn.Equals("Y"))
                    sbQuery.AppendLine(" AND use_yn = 'Y' ");

                sbQuery.AppendLine(" ORDER BY ch_nm ");

                i = 0;                
                sqlParams[i++] = new OracleParameter(":GenreCode"    , OracleDbType.Varchar2,4);
                               
                i = 0;
                sqlParams[i++].Value = channelModel.GenreCode; //장르코드임


                // __DEBUG__
                _log.Debug(sbQuery.ToString());
                // __DEBUG__
				
                // 쿼리실행
                DataSet ds = new DataSet();
                _db.ExecuteQueryParams(ds,sbQuery.ToString(),sqlParams);

                // 결과 DataSet의 채널모델에 복사
                channelModel.ChannelDataSet = ds.Copy();
                // 결과
                channelModel.ResultCnt = Utility.GetDatasetCount(channelModel.ChannelDataSet);
                // 결과코드 셋트
                channelModel.ResultCD = "0000";

                // __DEBUG__
                _log.Debug("<출력정보>");
                _log.Debug("ResultCnt:[" + channelModel.ResultCnt + "]");
                // __DEBUG__

                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "GetChannelDetailList() End");
                _log.Debug("-----------------------------------------");
            }
            catch(Exception ex)
            {
                channelModel.ResultCD = "3000";
                channelModel.ResultDesc = "실시간채널 정보 조회중 오류가 발생하였습니다";
                _log.Exception(ex);
            }
            finally
            {
                // 데이터베이스를  Close한다
                _db.Close();
            }

        }

        /// <summary>
        /// 채널구성 디테일 목록조회 - 사용안함
        /// </summary>
        /// <param name="channelModel"></param>
        public void GetChannelSetDetailList(HeaderModel header, ChannelModel channelModel)
        {
            try
            {   // 데이터베이스를 OPEN한다
                _db.Open();

                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "GetChannelSetDetailList() Start");
                _log.Debug("-----------------------------------------");

                // __DEBUG__
                _log.Debug("<입력정보>");
                _log.Debug("MediaCode :[" + channelModel.MediaCode       + "]");
                _log.Debug("ChannelNo :[" + channelModel.ChannelNo       + "]");
                // __DEBUG__

                StringBuilder sbQuery = new StringBuilder();

                int i = 0;
                SqlParameter[] sqlParams = new SqlParameter[2];
                // 쿼리생성
                sbQuery.Append("\n"
                    + " SELECT a.CategoryCode ,a.GenreCode	\n"
                    + " ,(SPACE(5 - LEN(CONVERT(VARCHAR(10),a.CategoryCode))) +  CONVERT(VARCHAR(10),a.CategoryCode) + ' ' + b.CategoryName) AS CategoryName \n"
                    + " ,(SPACE(5 - LEN(CONVERT(VARCHAR(10),a.GenreCode)))    +  CONVERT(VARCHAR(10),a.GenreCode)    + ' ' + C.GenreName   ) AS GenreName 	 \n"
                    + " FROM (													 \n"
                    + " 	   SELECT MediaCode,CategoryCode ,GenreCode			 \n"
                    + " 	   FROM ChannelSet a with(nolock)					 \n"
                    + "        WHERE 1=1										 \n"
                    + "          AND MediaCode = @MediaCode						 \n"
                    + "          AND ChannelNo = @ChannelNo						 \n"
                    + "     GROUP BY MediaCode,CategoryCode,GenreCode			 \n"
                    + " 	  ) a												 \n"
                    + " 	LEFT JOIN Category	b with(nolock) ON(a.CategoryCode = b.CategoryCode)	\n"
                    + " 	LEFT JOIN Genre		c with(nolock) ON (a.GenreCode = c.GenreCode)		\n"	
                    );	 
                sbQuery.Append("  ORDER BY a.CategoryCode,a.GenreCode ");

                i = 0;
                sqlParams[i++] = new SqlParameter("@MediaCode"       , SqlDbType.TinyInt);
                sqlParams[i++] = new SqlParameter("@ChannelNo"    , SqlDbType.Int);
                               
                i = 0;
                sqlParams[i++].Value = channelModel.MediaCode;	
                sqlParams[i++].Value = channelModel.ChannelNo;


                // __DEBUG__
                _log.Debug(sbQuery.ToString());
                // __DEBUG__
				
                // 쿼리실행
                DataSet ds = new DataSet();
                _db.ExecuteQueryParams(ds,sbQuery.ToString(),sqlParams);

                // 결과 DataSet의 채널모델에 복사
                channelModel.ChannelDataSet = ds.Copy();
                // 결과
                channelModel.ResultCnt = Utility.GetDatasetCount(channelModel.ChannelDataSet);
                // 결과코드 셋트
                channelModel.ResultCD = "0000";

                // __DEBUG__
                _log.Debug("<출력정보>");
                _log.Debug("ResultCnt:[" + channelModel.ResultCnt + "]");
                // __DEBUG__

                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "GetChannelSetDetailList() End");
                _log.Debug("-----------------------------------------");
            }
            catch(Exception ex)
            {
                channelModel.ResultCD = "3000";
                channelModel.ResultDesc = "채널구성 디테일 정보 조회중 오류가 발생하였습니다";
                _log.Exception(ex);
            }
            finally
            {
                // 데이터베이스를  Close한다
                _db.Close();
            }

        }


           
        /// <summary>
        /// 실시간채널정보 업데이트
        /// </summary>
        /// <param name="header"></param>
        /// <param name="channelModel"></param>
        public void SetChannelUpdate(HeaderModel header, ChannelModel channelModel)
        {
            try
            {
                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "SetChannelUpdate() Start");
                _log.Debug("-----------------------------------------");

                // 데이터베이스를 OPEN한다
                _db.Open();

                StringBuilder sbQuery = new StringBuilder();
                sbQuery.AppendLine(" UPDATE CHNL_COD        ");
                sbQuery.AppendLine("   SET use_yn = :UseYn  ");
                sbQuery.AppendLine("        ,ad_yn  = :AdYn ");
                sbQuery.AppendLine("        ,ad_rate = :AdRate      ");
                sbQuery.AppendLine("        ,adn_rate = :AdnRate    ");
                sbQuery.AppendLine(" WHERE svc_id = :SvcId  ");
                sbQuery.AppendLine(" AND    ch_no = :ChNo   ");

                int i = 0;
                int rc = 0;
                OracleParameter[] sqlParams = new OracleParameter[6];
                               

                sqlParams[i++] = new OracleParameter(":UseYn", OracleDbType.Char,1);
                sqlParams[i++] = new OracleParameter(":AdYn", OracleDbType.Char,1);
                sqlParams[i++] = new OracleParameter(":AdRate", OracleDbType.Int32);
                sqlParams[i++] = new OracleParameter(":AdnRate", OracleDbType.Int32);
                sqlParams[i++] = new OracleParameter(":SvcId", OracleDbType.Varchar2,10);
                sqlParams[i++] = new OracleParameter(":ChNo", OracleDbType.Varchar2,10);

                i = 0;
                sqlParams[i++].Value = channelModel.UseYn;
                sqlParams[i++].Value = channelModel.AdYn;
                sqlParams[i++].Value = Convert.ToInt32(channelModel.AdRate);
                sqlParams[i++].Value = Convert.ToInt32(channelModel.AdnRate);
                sqlParams[i++].Value = channelModel.ServiceID;
                sqlParams[i++].Value = channelModel.ChannelNumber;
                               

                _log.Debug(sbQuery.ToString());


                // __DEBUG__
                _log.Debug(sbQuery.ToString());
                // __DEBUG__

                // 쿼리실행
                try
                {
                    _db.BeginTran();
                    rc = _db.ExecuteNonQueryParams(sbQuery.ToString(), sqlParams);
                    _db.CommitTran();

                    // __MESSAGE__
                    _log.Message("실시간채널정보 수정:[" + channelModel.ServiceID + "] 등록자:[" + header.UserID + "]");

                }
                catch (Exception ex)
                {
                    _db.RollbackTran();
                    throw ex;
                }

                channelModel.ResultCD = "0000";  // 정상

                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "SetSchRateUpdate() End");
                _log.Debug("-----------------------------------------");

            }
            catch (Exception ex)
            {
                channelModel.ResultCD = "3101";
                channelModel.ResultDesc = "등급정보 생성 중 오류가 발생하였습니다";
                _log.Exception(ex);
            }
            finally
            {
                // 데이터베이스를  Close한다
                _db.Close();
            }		

        }

        /// <summary>
        /// 채널 생성 - 사용안함
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
        public void SetChannelCreate(HeaderModel header, ChannelModel channelModel)
        {
            try
            {   // 데이터베이스를 OPEN한다
                _db.Open();

                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "SetChannelCreate() Start");
                _log.Debug("-----------------------------------------");
                        
                StringBuilder sbQuery = new StringBuilder();
                StringBuilder sbQuery2 = new StringBuilder();
                        			

                sbQuery.Append( ""
                    + "        DELETE FROM  Channel			                    \n"
                    + "              WHERE MediaCode = @MediaCode				\n"
                    + "                AND ChannelNo = @ChannelNo				\n"
                    );                                       
                sbQuery2.Append( ""
                    + "        INSERT INTO Channel			                    \n"
                    + "                   (                                     \n"
                    + "                    MediaCode							\n"
                    + "                   ,ChannelNo							\n"
                    + "                   ,SeriesNo								\n"
                    + "                   ,Title								\n"
                    + "                   ,ContentId							\n"
                    + "                   ,TotalSeries							\n"
                    + "                   ,ModDt								\n"
                    + "                   )                                     \n"
                    + "             VALUES										\n"
                    + "                   (                                     \n"
                    + "                    @MediaCode							\n"
                    + "                   ,@ChannelNo							\n"
                    + "                   ,@SeriesNo							\n"
                    + "                   ,@Title								\n"
                    + "                   ,@ContentId							\n"
                    + "                   ,@TotalSeries							\n"
                    + "                   ,GETDATE()							\n"
                    + "                   )                                     \n"
                    );                                       
                               
                        
                		
                        
                // 쿼리실행
                try
                {
                    
                    int i = 0;
                    int rc = 0;
                    SqlParameter[] sqlParams = new SqlParameter[2];
                    _db.BeginTran();
                     
                    //먼저 인서트 된 데이터를 지워주고 그이후에 인서트를 한다.
                    i = 0;
                    sqlParams[i++] = new SqlParameter("@MediaCode"       , SqlDbType.TinyInt);
                    sqlParams[i++] = new SqlParameter("@ChannelNo"     , SqlDbType.Int );
                                                   
                    i = 0;
                    sqlParams[i++].Value = channelModel.MediaCode;
                    sqlParams[i++].Value = channelModel.ChannelNo;
                    rc =  _db.ExecuteNonQueryParams(sbQuery.ToString(), sqlParams);

                    // __DEBUG__
                    _log.Debug("MediaCode = "+channelModel.MediaCode);
                    _log.Debug("ChannelNo = "+channelModel.ChannelNo);
                    _log.Debug(sbQuery.ToString());
                    // __DEBUG__

                    sqlParams = new SqlParameter[10];

                    for(int count=0;count < channelModel.ChannelDataSet.Tables["Contents"].Rows.Count;count++)

                    {
                        i = 0;
                        sqlParams[i++] = new SqlParameter("@MediaCode"       , SqlDbType.VarChar , 120);
                        sqlParams[i++] = new SqlParameter("@ChannelNo"     , SqlDbType.Int );
                        sqlParams[i++] = new SqlParameter("@SeriesNo" , SqlDbType.TinyInt);
                        sqlParams[i++] = new SqlParameter("@Title"    , SqlDbType.VarChar , 120);
                        sqlParams[i++] = new SqlParameter("@ContentId"    , SqlDbType.VarChar,36);
                        sqlParams[i++] = new SqlParameter("@TotalSeries"    , SqlDbType.Int );
                       
                         
                          
                        
                        i = 0;
                        sqlParams[i++].Value = channelModel.MediaCode;
                        sqlParams[i++].Value = channelModel.ChannelNo;
                        sqlParams[i++].Value = channelModel.ChannelDataSet.Tables["Contents"].Rows[count][3].ToString();
                        sqlParams[i++].Value = channelModel.Title;
                        sqlParams[i++].Value = channelModel.ChannelDataSet.Tables["Contents"].Rows[count][1].ToString();
                        
                        if(channelModel.TotalSeries.Trim().Length > 0)
                        {
                            sqlParams[i++].Value = channelModel.TotalSeries;
                        }
                        else
                        {
                            sqlParams[i++].Value = null;		
                        }
                        

                        rc =  _db.ExecuteNonQueryParams(sbQuery2.ToString(), sqlParams);
                        // __DEBUG__
                        _log.Debug(sbQuery2.ToString());
                        // __DEBUG__
                    }
                    _db.CommitTran();
                        
                    // __MESSAGE__
                    _log.Message("채널정보생성:[성공]");
                        
                }
                catch(Exception ex)
                {
                    _db.RollbackTran();
                    throw ex;
                }
                        
                channelModel.ResultCD = "0000";  // 정상
                        
                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "SetChannelCreate() End");
                _log.Debug("-----------------------------------------");	
            }
            catch(Exception ex)
            {
                channelModel.ResultCD   = "3101";
                channelModel.ResultDesc = "채널정보 생성 중 오류가 발생하였습니다";
                _log.Exception(ex);
            }
            finally
            {
                // 데이터베이스를  Close한다
                _db.Close();
            }
        }

        /// <summary>
        /// 사용안함
        /// </summary>
        /// <param name="header"></param>
        /// <param name="channelModel"></param>
        public void SetChannelDelete(HeaderModel header, ChannelModel channelModel)
        {
            try
            {   // 데이터베이스를 OPEN한다
                _db.Open();

                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "SetChannelDelete() Start");
                _log.Debug("-----------------------------------------");
                        
                StringBuilder sbQuery = new StringBuilder();
                        
                int i = 0;
                int rc = 0;
                SqlParameter[] sqlParams = new SqlParameter[2];
                        
                sbQuery.Append( ""
                    + "        DELETE FROM  Channel			                    \n"
                    + "              WHERE MediaCode = @MediaCode				\n"
                    + "                AND ChannelNo = @ChannelNo				\n"
                    );                             
                                        
                sqlParams[i++] = new SqlParameter("@MediaCode"       , SqlDbType.TinyInt);
                sqlParams[i++] = new SqlParameter("@ChannelNo"       , SqlDbType.Int );
                        
                i = 0;
                sqlParams[i++].Value = channelModel.MediaCode;
                sqlParams[i++].Value = channelModel.ChannelNo;
                        
                // 쿼리실행
                try
                {
                    _db.BeginTran();
                    rc =  _db.ExecuteNonQueryParams(sbQuery.ToString(), sqlParams);
                    _db.CommitTran();
                        
                    // __MESSAGE__
                    _log.Message("채널정보삭제:[" + channelModel.ChannelNo + "] 등록자:[" + header.UserID + "]");
                        
                }
                catch(Exception ex)
                {
                    _db.RollbackTran();
                    throw ex;
                }
                        
                channelModel.ResultCD = "0000";  // 정상
                        	
                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "SetChannelDelete() End");
                _log.Debug("-----------------------------------------");	
            }
            catch(Exception ex)
            {
                channelModel.ResultCD   = "3301";
                channelModel.ResultDesc = "채널정보 삭제중 오류가 발생하였습니다";
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