/*
 * -------------------------------------------------------
 * [수정사항]
 * -------------------------------------------------------
 * 수정코드  : [E_01]
 * 수정자    : 
 * 수정일    : 
 * 수정내용  :
 *             
 * --------------------------------------------------------
 * -------------------------------------------------------
 * [수정사항]
 * -------------------------------------------------------
 * 수정코드  : [E_02]
 * 수정자    : 
 * 수정일    : 
 * 수정내용  : 
 * --------------------------------------------------------
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

namespace AdManagerWebService.Contract
{
	/// <summary>
	/// AdvertiserBiz에 대한 요약 설명입니다.
	/// </summary>
	public class AdvertiserBiz : BaseBiz
	{
		public AdvertiserBiz() : base(FrameSystem.connDbString, true)
		{
			_log = FrameSystem.oLog;
		}
		/// <summary>
		/// 광고주목록조회
		/// </summary>
		/// <param name="advertiserModel"></param>
		public void GetAdvertiserList(HeaderModel header, AdvertiserModel advertiserModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetAdvertiserList() Start");
				_log.Debug("-----------------------------------------");

				// 데이터베이스를 OPEN한다
				_db.Open();

				// __DEBUG__
				_log.Debug("<입력정보>");
				_log.Debug("SearchKey      :[" + advertiserModel.SearchKey       + "]");
				_log.Debug("SearchAdvertiserLevel:[" + advertiserModel.SearchAdvertiserLevel + "]");
				// __DEBUG__

				StringBuilder sbQuery = new StringBuilder();
                #region 삭제 할 것
                /*
                sbQuery.AppendLine("    SELECT	a.AdvertiserCode, a.AdvertiserName 		                                ");
                sbQuery.AppendLine("    	,	a.RapCode                                                               ");
                sbQuery.AppendLine("    	,	ISNULL(b.RapName,'공용') AS RapName                                     ");
                sbQuery.AppendLine("    	,	a.Comment                                                               ");
                sbQuery.AppendLine("    	,	a.UseYn                                                                 ");
                sbQuery.AppendLine("    	,	CASE a.UseYn WHEN 'Y' THEN '' WHEN 'N' THEN '사용안함' END AS UseYn_N   ");
                sbQuery.AppendLine("    	,	a.RegDt                                                                 ");
                sbQuery.AppendLine("    	,	a.ModDt                                                                 ");
                sbQuery.AppendLine("    	,	a.RegID                                                                 ");
                sbQuery.AppendLine("    	,	c1.JobCode                                                              "); // [A_01]
                sbQuery.AppendLine("    	,	c2.JobName as JobNameLevel1                                             "); // [A_01]
                sbQuery.AppendLine("    	,	c1.JobName as JobNameLevel2                                             "); // [A_01]
                sbQuery.AppendLine("    FROM Advertiser a with(NoLock)                                                  ");
                sbQuery.AppendLine("    LEFT JOIN MediaRap b with(NoLock) ON (b.RapCode = a.RapCode)                    ");
                sbQuery.AppendLine("    LEFT JOIN JobClass c1 with(nolock) ON (c1.JobCode = a.JobClass)                 "); // [A_01]
                sbQuery.AppendLine("    LEFT JOIN JobClass c2 with(nolock) ON (c2.JobCode = c1.Level1Code)              "); // [A_01]
                sbQuery.AppendLine("    WHERE 1 = 1                                                                     ");
				
				// 검색어가 있으면
				if (advertiserModel.SearchKey.Trim().Length > 0)
				{
					// 여러컬럼에 대하여 LIKE 검색을 한다.
					sbQuery.Append(" AND ("
						+ "    a.AdvertiserName      LIKE '%" + advertiserModel.SearchKey.Trim() + "%' \n"
                        + " OR a.Comment    LIKE '%" + advertiserModel.SearchKey.Trim() + "%' \n"   // [E_01]
						+ " ) ");
				}

				if(!advertiserModel.SearchRap.Equals("00"))
				{
					sbQuery.Append("  AND(  a.RapCode = '"+advertiserModel.SearchRap+"' OR a.RapCode = 0 ) \n");
				}      
				// 어드민과 슈퍼유저면 사용여부가 'Y', 'N' 데이터를 다 조회한다.
//				if (header.UserClass.Equals("10") || header.UserClass.Equals("20"))
//				{
//					sbQuery.Append(" AND UseYn = 'Y' OR UseYn = 'N' \n");
//				}
//				else
//				{
//					sbQuery.Append(" AND UseYn = 'Y' \n");
//				}

				if(advertiserModel.SearchchkAdState_10.Trim().Length > 0 && advertiserModel.SearchchkAdState_10.Trim().Equals("Y"))
				{
                    sbQuery.Append(" AND a.UseYn = 'Y' OR a.UseYn = 'N' \n");   // [E_01]
				}	
				if(advertiserModel.SearchchkAdState_10.Trim().Length > 0 && advertiserModel.SearchchkAdState_10.Trim().Equals("N"))
				{
					sbQuery.Append(" AND  a.UseYn  = 'Y' \n");					
				}	
				// 광고주레벨을 선택했으면
				/*if(advertiserModel.SearchAdvertiserLevel.Trim().Length > 0 && !advertiserModel.SearchAdvertiserLevel.Trim().Equals("00"))
				{
					sbQuery.Append(" AND A.MediaCode = '" + advertiserModel.SearchAdvertiserLevel.Trim() + "' \n");
				}*/			

				//sbQuery.Append(" ORDER BY a.AdvertiserCode DESC  \n");
                
                #endregion

                sbQuery.Append("\n" + " SELECT  ");
                sbQuery.Append("\n" + "    a.advter_cod as AdvertiserCode   "); 
                sbQuery.Append("\n" + "   ,a.advter_nm  as AdvertiserName   ");
                sbQuery.Append("\n" + "   ,a.rep_cod    as RapCode          ");
                sbQuery.Append("\n" + "   ,NVL(b.rep_nm,'공용') as RapName  "); 
                sbQuery.Append("\n" + "   ,a.adver_memo as \"Comment\"      ");
                sbQuery.Append("\n" + "   ,a.use_yn     as UseYn            ");
                sbQuery.Append("\n" + "   ,CASE a.use_yn WHEN 'Y' THEN '' WHEN 'N' THEN '사용안함' END AS UseYn_N   ");
                sbQuery.Append("\n" + "   ,a.dt_insert  as RegDt    ");        
                sbQuery.Append("\n" + "   ,a.dt_update  as ModDt    ");
                sbQuery.Append("\n" + "   ,a.id_insert  as RegID    ");                
                sbQuery.Append("\n" + " FROM ADVTER a   ");
                sbQuery.Append("\n" + " LEFT JOIN MDA_REP b ON (b.rep_cod = a.rep_cod)  ");
                sbQuery.Append("\n" + " WHERE 1 = 1     ");

                // 검색어가 있으면
                if (advertiserModel.SearchKey.Trim().Length > 0)
                {
                    // 여러컬럼에 대하여 LIKE 검색을 한다.
                    sbQuery.Append(" AND ("
                        + "    a.advter_nm      LIKE '%" + advertiserModel.SearchKey.Trim() + "%' \n"
                        + " OR a.adver_memo    LIKE '%" + advertiserModel.SearchKey.Trim() + "%' \n"   // [E_01]
                        + " ) ");
                }

                if (!advertiserModel.SearchRap.Equals("00"))
                {
                    sbQuery.Append("  AND(  a.rep_cod = '" + advertiserModel.SearchRap + "' OR a.rep_cod = 0 ) \n");
                }

                if (advertiserModel.SearchchkAdState_10.Trim().Length > 0 && advertiserModel.SearchchkAdState_10.Trim().Equals("Y"))
                {
                    sbQuery.Append(" AND a.use_yn = 'Y' OR a.use_yn = 'N' \n");   
                }
                if (advertiserModel.SearchchkAdState_10.Trim().Length > 0 && advertiserModel.SearchchkAdState_10.Trim().Equals("N"))
                {
                    sbQuery.Append(" AND  a.use_yn  = 'Y' \n");
                }
                
                sbQuery.Append(" ORDER BY a.advter_cod DESC  \n");

                // __DEBUG__
				_log.Debug(sbQuery.ToString());
				// __DEBUG__
				
				// 쿼리실행
				DataSet ds = new DataSet();
				_db.ExecuteQuery(ds,sbQuery.ToString());

				// 결과 DataSet의 광고주모델에 복사
				advertiserModel.AdvertiserDataSet = ds.Copy();
				// 결과
				advertiserModel.ResultCnt = Utility.GetDatasetCount(advertiserModel.AdvertiserDataSet);
				// 결과코드 셋트
				advertiserModel.ResultCD = "0000";

				// __DEBUG__
				_log.Debug("<출력정보>");
				_log.Debug("ResultCnt:[" + advertiserModel.ResultCnt + "]");
				// __DEBUG__

				_log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "GetAdvertiserList() End");
				_log.Debug("-----------------------------------------");
			}
			catch(Exception ex)
			{
				advertiserModel.ResultCD = "3000";
				advertiserModel.ResultDesc = "광고주정보 조회중 오류가 발생하였습니다";
				_log.Exception(ex);
			}
			finally
			{
				// 데이터베이스를  Close한다
				_db.Close();			
			}		

		}

		/// <summary>
        /// 광고주정보 저장
		/// </summary>
		/// <param name="header"></param>
		/// <param name="advertiserModel"></param>
		public void SetAdvertiserUpdate(HeaderModel header, AdvertiserModel advertiserModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "SetAdvertiserUpdate() Start");
				_log.Debug("-----------------------------------------");

				// 데이터베이스를 OPEN한다
				_db.Open();

				StringBuilder sbQuery = new StringBuilder();

				int i = 0;
				int rc = 0;				
                OracleParameter[] sqlParams = new OracleParameter[6];

                sbQuery.AppendLine("UPDATE ADVTER                           ");
                sbQuery.AppendLine("\n " + "   SET advter_nm    = :AdvertiserName   ");
                sbQuery.AppendLine("\n " + "   ,rep_cod			= :RapCode          ");
                sbQuery.AppendLine("\n " + "   ,adver_memo		= :Comments  		");
                sbQuery.AppendLine("\n " + "   ,use_yn			= :UseYn      		");
                sbQuery.AppendLine("\n " + "   ,dt_update		= SYSDATE           ");
                sbQuery.AppendLine("\n " + "   ,id_update		= :RegID            ");
                sbQuery.AppendLine("\n " + "WHERE advter_cod = :AdvertiserCode     ");        
                

				i = 0;
                sqlParams[i++] = new OracleParameter(":AdvertiserName", OracleDbType.Varchar2, 40);
                sqlParams[i++] = new OracleParameter(":RapCode", OracleDbType.Int32);
                sqlParams[i++] = new OracleParameter(":Comments", OracleDbType.Varchar2, 50);
                sqlParams[i++] = new OracleParameter(":UseYn", OracleDbType.Char, 1);
                sqlParams[i++] = new OracleParameter(":RegID", OracleDbType.Varchar2, 10);
                sqlParams[i++] = new OracleParameter(":AdvertiserCode", OracleDbType.Int32);      
		

				i = 0;
				sqlParams[i++].Value = advertiserModel.AdvertiserName;		
				if(advertiserModel.RapCode.Trim().Length > 0)
				{
					sqlParams[i++].Value = Convert.ToInt32(advertiserModel.RapCode);	
				}
				else
				{
					sqlParams[i++].Value = 0;		
				}
                /* 업종 구분용..
                if (!advertiserModel.JobCode.Trim().Equals(string.Empty))    // [A_01]
                {
                    sqlParams[i++].Value = string.Format("{0,6}", advertiserModel.JobCode).Replace(" ", "0");
                }
                else
                {
                    sqlParams[i++].Value = "";
                }
                */
				sqlParams[i++].Value = advertiserModel.Comment;
				sqlParams[i++].Value = advertiserModel.UseYn;
				sqlParams[i++].Value = header.UserID;      // 등록자
				sqlParams[i++].Value = Convert.ToInt32(advertiserModel.AdvertiserCode);

                _log.Debug("comment:[" + advertiserModel.Comment + "]");
                _log.Debug("useyn:[" + advertiserModel.UseYn + "]");                
                _log.Debug("advertiserCode:[" + advertiserModel.AdvertiserCode + "]");
                _log.Debug("RapCode:[" + advertiserModel.RapCode + "]");
                _log.Debug("AdvertiserName:[" + advertiserModel.AdvertiserName + "]");
                _log.Debug(sbQuery.ToString());

				// 쿼리실행
				try
				{
					_db.BeginTran();
					rc =  _db.ExecuteNonQueryParams(sbQuery.ToString(), sqlParams);
					_db.CommitTran();

					// __MESSAGE__
					_log.Message("광고주정보수정:["+advertiserModel.AdvertiserCode + "] 등록자:[" + header.UserID + "]");

				}
				catch(Exception ex)
				{
					_db.RollbackTran();
					throw ex;
				}

				advertiserModel.ResultCD = "0000";  // 정상

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "SetAdvertiserUpdate() End");
				_log.Debug("-----------------------------------------");

			}
			catch(Exception ex)
			{
				advertiserModel.ResultCD   = "3201";
				advertiserModel.ResultDesc = "광고주정보 수정중 오류가 발생하였습니다";
				_log.Exception(ex);
			}
			finally
			{
				// 데이터베이스를  Close한다
				_db.Close();			
			}		

		}

		/// <summary>
		/// 광고주 생성
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
		public void SetAdvertiserCreate(HeaderModel header, AdvertiserModel advertiserModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "SetAdvertiserCreate() Start");
				_log.Debug("-----------------------------------------");

				// 데이터베이스를 OPEN한다
				_db.Open();

				StringBuilder sbQuery = new StringBuilder();
			
				int i = 0;
				int rc = 0;
				
                OracleParameter[] sqlParams = new OracleParameter[4];
                sbQuery.Append("\n" + " INSERT INTO ADVTER(     ");
                sbQuery.Append("\n" + "   advter_cod            ");
                sbQuery.Append("\n" + "   ,advter_nm            ");
                sbQuery.Append("\n" + "   ,rep_cod              ");
                sbQuery.Append("\n" + "   ,adver_memo           ");    
                sbQuery.Append("\n" + "   ,use_yn               ");
                sbQuery.Append("\n" + "   ,dt_insert            ");
                sbQuery.Append("\n" + "   ,dt_update            ");
                sbQuery.Append("\n" + "   ,id_insert )          ");
                sbQuery.Append("\n" + " SELECT NVL(MAX(advter_cod),0)+1 ");
                sbQuery.Append("\n" + "       ,:AdvertiserName  ");
                sbQuery.Append("\n" + "       ,:RapCode         ");
                sbQuery.Append("\n" + "       ,:Comments         ");
                sbQuery.Append("\n" + "       ,'Y'              ");
                sbQuery.Append("\n" + "       ,SYSDATE          ");
                sbQuery.Append("\n" + "       ,SYSDATE          ");
                sbQuery.Append("\n" + "       ,:RegID           ");                
                sbQuery.Append("\n" + " FROM ADVTER             ");

                sqlParams[i++] = new OracleParameter(":AdvertiserName", OracleDbType.Varchar2, 40);
                sqlParams[i++] = new OracleParameter(":RapCode", OracleDbType.Int32);
                sqlParams[i++] = new OracleParameter(":Comments", OracleDbType.Varchar2, 50);
                sqlParams[i++] = new OracleParameter(":RegID", OracleDbType.Varchar2, 10);		

				i = 0;
				sqlParams[i++].Value = advertiserModel.AdvertiserName;
				if(advertiserModel.RapCode.Trim().Length > 0)
				{
					sqlParams[i++].Value = Convert.ToInt32(advertiserModel.RapCode);		
				}
				else
				{
					sqlParams[i++].Value = 0;		
				}
                
				sqlParams[i++].Value = advertiserModel.Comment;				
				sqlParams[i++].Value = header.UserID;      // 등록자
				
				// 쿼리실행
				try
				{
					_db.BeginTran();
					rc =  _db.ExecuteNonQueryParams(sbQuery.ToString(), sqlParams);
					_db.CommitTran();

					// __MESSAGE__
					_log.Message("광고주정보생성:[" + advertiserModel.AdvertiserCode + "(" + advertiserModel.AdvertiserName + ")] 등록자:[" + header.UserID + "]");

				}
				catch(Exception ex)
				{
					_db.RollbackTran();
					throw ex;
				}

				advertiserModel.ResultCD = "0000";  // 정상

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "SetAdvertiserCreate() End");
				_log.Debug("-----------------------------------------");	
			}
			catch(Exception ex)
			{
				advertiserModel.ResultCD   = "3101";
				advertiserModel.ResultDesc = "광고주정보 생성 중 오류가 발생하였습니다";
				_log.Exception(ex);
			}
			finally
			{
				// 데이터베이스를  Close한다
				_db.Close();			
			}		
		}


		public void SetAdvertiserDelete(HeaderModel header, AdvertiserModel advertiserModel)
		{   
			int ClientCount = 0;
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "SetAdvertiserDelete() Start");
				_log.Debug("-----------------------------------------");

				// 데이터베이스를 OPEN한다
				_db.Open();

				StringBuilder sbQuery = new StringBuilder();
				StringBuilder sbQueryClientCount = new StringBuilder();
				int i = 0;
				int rc = 0;
				OracleParameter[] sqlParams = new OracleParameter[1];
                
				sbQueryClientCount.Append( "\n"
					+ "   SELECT COUNT(*) FROM    CLNT 	      \n"
					+ "   WHERE adver_cod  = :AdvertiserCode  \n"
					);      

				sbQuery.Append(""
					+ " DELETE ADVTER         \n"
					+ " WHERE advter_cod  = :AdvertiserCode  \n"					
					);

                sqlParams[i++] = new OracleParameter(":AdvertiserCode", OracleDbType.Int32);  // [E_01]

				i = 0;				
				sqlParams[i++].Value = advertiserModel.AdvertiserCode;

				// 쿼리실행
				try
				{
					//매체대행광고주 관계 Count조사///////////////////////////////////////////////
					// __DEBUG__
					_log.Debug(sbQueryClientCount.ToString());
					// __DEBUG__

					// 쿼리실행
					DataSet ds = new DataSet();
					_db.ExecuteQueryParams(ds,sbQueryClientCount.ToString(),sqlParams);
                    
					ClientCount =  Convert.ToInt32(ds.Tables[0].Rows[0][0].ToString());

					ds.Dispose();

					_log.Debug("ClientCount          -->" + ClientCount);

					// 이미 다른테이블에 사용중인 데이터가 있다면 Exception를 발생시킨다.
					if(ClientCount > 0) throw new Exception();

                    i = 0;
                    sqlParams[i++] = new OracleParameter(":AdvertiserCode", OracleDbType.Int32);  // [E_01]
                    i = 0;
                    sqlParams[i++].Value = advertiserModel.AdvertiserCode;
					_db.BeginTran();
					rc =  _db.ExecuteNonQueryParams(sbQuery.ToString(), sqlParams);
					_db.CommitTran();

					// __MESSAGE__
					_log.Message("광고주정보삭제:[" + advertiserModel.AdvertiserCode + "] 등록자:[" + header.UserID + "]");

				}
				catch(Exception ex)
				{
					_db.RollbackTran();
					throw ex;
				}

				advertiserModel.ResultCD = "0000";  // 정상
	
				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "SetAdvertiserDelete() End");
				_log.Debug("-----------------------------------------");	
			}
			catch(Exception ex)
			{
				advertiserModel.ResultCD   = "3301";
				// 이미 다른테이블에 사용중인 데이터가 있다면
				if(ClientCount > 0 )
				{
					advertiserModel.ResultDesc = "등록된 매체대행광고주가 있으므로 광고주정보를 삭제할수 없습니다.";
				}
				else
				{
					advertiserModel.ResultDesc = "광고주 정보 삭제중 오류가 발생하였습니다";
					_log.Exception(ex);
				}
			}
			finally
			{
				// 데이터베이스를  Close한다
				_db.Close();			
			}		
		}

        /// <summary>
        /// [A_01]
        /// 업종 정보 목록
        /// </summary>
        /// <param name="contractModel"></param>
        public void GetJobClassList(HeaderModel header, AdvertiserModel model)
        {
            try
            {  // 데이터베이스를 OPEN한다
                _db.Open();

                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "GetJobClassList() Start");
                _log.Debug("-----------------------------------------");

                StringBuilder sbQuery = new StringBuilder();

                // 쿼리생성
                sbQuery.AppendLine("    Select	JobCode                     ");
                sbQuery.AppendLine("    	,	JobName                     ");
                sbQuery.AppendLine("    	,	Level1Code as JobUpperCode  ");
                sbQuery.AppendLine("    	,	Level	   as JobLevel      ");
                sbQuery.AppendLine("    From JobClass with(nolock)          ");
                sbQuery.AppendLine("    Where  1 = 1                        ");
                sbQuery.AppendLine("    AND (Level = 1 or Level = 2);       ");

                // __DEBUG__
                _log.Debug(sbQuery.ToString());
                // __DEBUG__

                // 쿼리실행
                DataSet ds = new DataSet();
                _db.ExecuteQuery(ds, sbQuery.ToString());

                // 결과 DataSet의 광고팩키지모델에 복사
                model.AdvertiserDataSet = ds.Copy();
                // 결과
                model.ResultCnt = Utility.GetDatasetCount(model.AdvertiserDataSet);
                // 결과코드 셋트
                model.ResultCD = "0000";

                // __DEBUG__
                _log.Debug("<출력정보>");
                _log.Debug("ResultCnt:[" + model.ResultCnt + "]");
                // __DEBUG__

                _log.Debug("-----------------------------------------");
                _log.Debug(this.ToString() + "GetJobClassList() End");
                _log.Debug("-----------------------------------------");
            }
            catch (Exception ex)
            {
                model.ResultCD = "3000";
                model.ResultDesc = "업종 정보 조회중 오류가 발생하였습니다";
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