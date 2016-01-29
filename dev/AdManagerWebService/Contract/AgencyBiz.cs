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
	/// AgencyBiz에 대한 요약 설명입니다.
	/// </summary>
	public class AgencyBiz : BaseBiz
	{
		public AgencyBiz() : base(FrameSystem.connDbString, true)
		{
			_log = FrameSystem.oLog;
		}


		/// <summary>
		/// 대행사목록조회
		/// </summary>
		/// <param name="header"></param>
		/// <param name="agencyModel"></param>
		public void GetAgencyList(HeaderModel header, AgencyModel agencyModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetAgencyList() Start");
				_log.Debug("-----------------------------------------");

				// 데이터베이스를 OPEN한다
				_db.Open();

				// __DEBUG__
				_log.Debug("<입력정보>");
				_log.Debug("SearchKey      :[" + agencyModel.SearchKey       + "]");
				_log.Debug("SearchAgencyType:[" + agencyModel.SearchAgencyType + "]");
				// __DEBUG__

				StringBuilder sbQuery = new StringBuilder();
                
                #region 삭제 할 것
                /*
				// 쿼리생성
				sbQuery.Append("\n"
					+ " SELECT a.AgencyCode, a.AgencyName  \n"
					+ "       ,a.RapCode				                 \n"
					+ "       ,ISNULL(d.RapName,'공용') AS RapName        \n"
					+ "       ,a.AgencyType				                 \n"
					+ "       ,b.CodeName AS AgencyTypeName              \n"
					+ "       ,a.Address , a.Tell                        \n"
					+ "       ,a.Comment                        \n"
					+ "       ,a.UseYn                        \n"
					+ "       ,CASE A.UseYn WHEN 'Y' THEN '' WHEN 'N' THEN '사용안함' END AS UseYn_N  \n"
					+ "       ,convert(char(19), a.RegDt, 120) RegDt     \n"
					+ "       ,convert(char(19), a.ModDt, 120) ModDt     \n"
					+ "       ,a.RegID, c.UserName as RegName            \n"
					+ "  FROM Agency a with(NoLock) \n"
					+ "       LEFT JOIN SystemCode b with(NoLock) ON (a.AgencyType = b.Code and b.Section = '22') \n"
					+ "       LEFT JOIN SystemUser c with(NoLock) ON (a.RegID = c.UserID)     \n"
					+ "       LEFT JOIN MediaRap   d with(NoLock) ON (a.RapCode = d.RapCode)     \n"
					+ " WHERE 1 = 1  \n"
					);
				if(agencyModel.SearchchkAdState_10.Trim().Length > 0 && agencyModel.SearchchkAdState_10.Trim().Equals("Y"))
				{
					sbQuery.Append(" AND A.UseYn = 'Y' OR A.UseYn = 'N' \n");
				}	
				if(agencyModel.SearchchkAdState_10.Trim().Length > 0 && agencyModel.SearchchkAdState_10.Trim().Equals("N"))
				{
					sbQuery.Append(" AND  a.UseYn  = 'Y' \n");					
				}	
				if(!agencyModel.SearchRap.Equals("00"))
				{
					sbQuery.Append("  AND(  a.RapCode = '"+agencyModel.SearchRap+"' OR a.RapCode = 0 ) \n");
				}    
			
				// 검색어가 있으면
				if (agencyModel.SearchKey.Trim().Length > 0)
				{
					// 여러컬럼에 대하여 LIKE 검색을 한다.
					sbQuery.Append(" AND ("
						+ "    a.AgencyName LIKE '%" + agencyModel.SearchKey.Trim() + "%' \n"
						+ " OR a.Address   LIKE '%" + agencyModel.SearchKey.Trim() + "%' \n"
						+ " OR a.Tell      LIKE '%" + agencyModel.SearchKey.Trim() + "%' \n"
						+ " OR a.Comment   LIKE '%" + agencyModel.SearchKey.Trim() + "%' \n"
						+ " ) ");
				}

				// 대행사구분을 선택했으면
				if(agencyModel.SearchAgencyType.Trim().Length > 0 && !agencyModel.SearchAgencyType.Trim().Equals("00"))
				{
					sbQuery.Append(" AND a.AgencyType = '" + agencyModel.SearchAgencyType.Trim() + "' \n");
				}		
	
				sbQuery.Append(" ORDER BY a.AgencyCode Desc \n");
                */
                #endregion
                
                sbQuery.Append("\n" + " SELECT ");
                sbQuery.Append("\n" + "      a.agnc_cod as AgencyCode   ");
                sbQuery.Append("\n" + "     ,a.agnc_nm as AgencyName    ");
                sbQuery.Append("\n" + "     ,a.rep_cod as RapCode       ");
                sbQuery.Append("\n" + "     ,NVL(D.rep_nm,'공용') as RapName ");
                sbQuery.Append("\n" + "     ,a.agnc_tp as AgencyType    ");            
                sbQuery.Append("\n" + "     ,b.stm_cod_nm as AgencyTypeName ");
                sbQuery.Append("\n" + "     ,'' as Adress               ");
                sbQuery.Append("\n" + "     ,a.tel_no as Tell           ");
                sbQuery.Append("\n" + "     ,a.agnc_memo as \"Comment\" ");
                sbQuery.Append("\n" + "     ,a.use_yn as UseYn          ");
                sbQuery.Append("\n" + "     ,CASE a.use_yn WHEN 'Y' THEN '' WHEN 'N' THEN '사용안함' END as UseYn_N ");
                sbQuery.Append("\n" + "     ,a.dt_insert as RegDt       ");
                sbQuery.Append("\n" + "     ,a.dt_update as ModDt       ");
                sbQuery.Append("\n" + "     ,a.id_insert as RegID       ");
                sbQuery.Append("\n" + "     ,c.user_nm as RegName       ");
                sbQuery.Append("\n" + " FROM    AGNC a                  ");
                sbQuery.Append("\n" + " LEFT JOIN STM_COD b ON (a.agnc_tp = b.stm_cod AND b.stm_cod_cls='22')   ");
                sbQuery.Append("\n" + " LEFT JOIN STM_USER c ON (a.id_insert = c.user_id)       ");
                sbQuery.Append("\n" + " LEFT JOIN MDA_REP d ON (a.rep_cod = d.rep_cod)          ");
                sbQuery.Append("\n" + " WHERE 1 = 1     ");

                if (agencyModel.SearchchkAdState_10.Trim().Length > 0 && agencyModel.SearchchkAdState_10.Trim().Equals("Y"))
                {
                    sbQuery.Append("\n" + " AND a.use_yn = 'Y' OR a.use_yn = 'N' ");
                }
                if (agencyModel.SearchchkAdState_10.Trim().Length > 0 && agencyModel.SearchchkAdState_10.Trim().Equals("N"))
                {
                    sbQuery.Append("\n" + " AND a.use_yn = 'Y' ");
                }
                if (!agencyModel.SearchRap.Equals("00"))
                {
                    sbQuery.Append("\n" + " AND(  a.rep_cod = " + agencyModel.SearchRap + " OR a.rep_cod = 0 ) ");
                }

                // 검색어가 있으면
                if (agencyModel.SearchKey.Trim().Length > 0)
                {
                    // 여러컬럼에 대하여 LIKE 검색을 한다.
                    sbQuery.Append("\n" +  " AND ("
                        + " a.agnc_nm LIKE '%" + agencyModel.SearchKey.Trim() + "%' \n"
                        + "  OR a.tel_no      LIKE '%" + agencyModel.SearchKey.Trim() + "%' \n"
                        + "  OR a.agnc_memo   LIKE '%" + agencyModel.SearchKey.Trim() + "%' \n"
                        + " ) ");
                }

                // 대행사구분을 선택했으면
                if (agencyModel.SearchAgencyType.Trim().Length > 0 && !agencyModel.SearchAgencyType.Trim().Equals("00"))
                {
                    sbQuery.Append("\n" + " AND a.agnc_tp = '" + agencyModel.SearchAgencyType.Trim() + "' \n");
                }

                sbQuery.Append("\n" + " ORDER BY a.agnc_cod Desc ");

				// __DEBUG__
				_log.Debug(sbQuery.ToString());
				// __DEBUG__
				
				// 쿼리실행
				DataSet ds = new DataSet();
				_db.ExecuteQuery(ds,sbQuery.ToString());

				// 결과 DataSet의 대행사모델에 복사
				agencyModel.AgencyDataSet = ds.Copy();
				// 결과
				agencyModel.ResultCnt = Utility.GetDatasetCount(agencyModel.AgencyDataSet);
				// 결과코드 셋트
				agencyModel.ResultCD = "0000";

				// __DEBUG__
				_log.Debug("<출력정보>");
				_log.Debug("ResultCnt:[" + agencyModel.ResultCnt + "]");
				// __DEBUG__

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetAgencyList() End");
				_log.Debug("-----------------------------------------");
			}
			catch(Exception ex)
			{
				agencyModel.ResultCD = "3000";
				agencyModel.ResultDesc = "대행사정보 조회중 오류가 발생하였습니다";
				_log.Exception(ex);
			}
			finally
			{
				// 데이터베이스를  Close한다
				_db.Close();			
			}		

		}

		/// <summary>
		/// 대행사정보 저장
		/// </summary>
		/// <param name="header"></param>
		/// <param name="agencyModel"></param>
		public void SetAgencyUpdate(HeaderModel header, AgencyModel agencyModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "SetAgencyUpdate() Start");
				_log.Debug("-----------------------------------------");

				// 데이터베이스를 OPEN한다
				_db.Open();

				//입력데이터의 Validation 검사
				if(agencyModel.AgencyCode.Length < 1) 
				{
					throw new FrameException("대행사코드가 입력되지 않습니다.");
				}

				StringBuilder sbQuery = new StringBuilder();

				int i = 0;
				int rc = 0;
                OracleParameter[] sqlParams = new OracleParameter[8];
				
                sbQuery.Append("\n" + " UPDATE AGNC                     ");
				sbQuery.Append("\n"	+ "   SET agnc_nm   = :AgencyName  ");
				sbQuery.Append("\n"	+ "      ,rep_cod   = :RapCode     ");
				sbQuery.Append("\n"	+ "      ,agnc_tp   = :AgencyType  "); 					
				sbQuery.Append("\n"	+ "      ,tel_no    = :Tell        ");
				sbQuery.Append("\n"	+ "      ,agnc_memo = :Comments     ");
				sbQuery.Append("\n"	+ "      ,use_yn     = :UseYn       ");
				sbQuery.Append("\n"	+ "      ,dt_update = SYSDATE      ");
                sbQuery.Append("\n" + "      ,id_update = :RegID       ");
				sbQuery.Append("\n"	+ " WHERE agnc_cod  = :AgencyCode  ");

				i = 0;
                sqlParams[i++] = new OracleParameter(":AgencyName", OracleDbType.Varchar2, 20);
                sqlParams[i++] = new OracleParameter(":RapCode", OracleDbType.Int32);
                sqlParams[i++] = new OracleParameter(":AgencyType", OracleDbType.Char, 2);                
                sqlParams[i++] = new OracleParameter(":Tell", OracleDbType.Varchar2, 15);
                sqlParams[i++] = new OracleParameter(":Comments", OracleDbType.Varchar2, 50);
                sqlParams[i++] = new OracleParameter(":UseYn", OracleDbType.Char, 1);
                sqlParams[i++] = new OracleParameter(":RegID", OracleDbType.Varchar2, 10);
                sqlParams[i++] = new OracleParameter(":AgencyCode", OracleDbType.Int32);

				i = 0;
				sqlParams[i++].Value = agencyModel.AgencyName;
				if(agencyModel.RapCode.Trim().Length > 0)
				{
					sqlParams[i++].Value = Convert.ToInt32(agencyModel.RapCode);		
				}
				else
				{
					sqlParams[i++].Value = 0;		
				}
				sqlParams[i++].Value = agencyModel.AgencyType;				
				sqlParams[i++].Value = agencyModel.Tell;
				sqlParams[i++].Value = agencyModel.Comment;
				sqlParams[i++].Value = agencyModel.UseYn;
				sqlParams[i++].Value = header.UserID;      // 등록자
				sqlParams[i++].Value = agencyModel.AgencyCode;

				// 쿼리실행
				try
				{
					_db.BeginTran();
					rc =  _db.ExecuteNonQueryParams(sbQuery.ToString(), sqlParams);
					_db.CommitTran();

					// __MESSAGE__
					_log.Message("대행사정보수정:["+agencyModel.AgencyName + "] 등록자:[" + header.UserID + "]");

				}
				catch(Exception ex)
				{
					_db.RollbackTran();
					throw ex;
				}

				agencyModel.ResultCD = "0000";  // 정상

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "SetUserUpdate() End");
				_log.Debug("-----------------------------------------");

			}
			catch(Exception ex)
			{
				agencyModel.ResultCD   = "3201";
				agencyModel.ResultDesc = "대행사정보 수정중 오류가 발생하였습니다";
				_log.Exception(ex);
			}
			finally
			{
				// 데이터베이스를  Close한다
				_db.Close();			
			}		

		}

		/// <summary>
		/// 대행사 생성
		/// </summary>
		/// <param name="header"></param>
		/// <param name="agencyModel"></param>
		/// 
		public void SetAgencyCreate(HeaderModel header, AgencyModel agencyModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "SetAgencyCreate() Start");
				_log.Debug("-----------------------------------------");

				// 데이터베이스를 OPEN한다
				_db.Open();
                
				StringBuilder sbQuery = new StringBuilder();
			
				int i = 0;
				int rc = 0;
				
                OracleParameter[] sqlParams = new OracleParameter[7];
                #region 삭제할것
                /*
                SqlParameter[] sqlParams = new SqlParameter[8];
				sbQuery.Append( ""
					+ "INSERT INTO Agency (\n"
					+ "       AgencyCode   \n"					
					+ "      ,AgencyName   \n"
					+ "      ,RapCode   \n"
					+ "      ,AgencyType   \n" 
					+ "      ,Address     \n"
					+ "      ,Tell        \n"
					+ "      ,Comment     \n"
					+ "      ,UseYn       \n"
					+ "      ,RegDt       \n"					
					+ "      ,ModDt       \n"
					+ "      ,RegID       \n"
					+ "      )            \n"
					+ " SELECT            \n"
					+ "       ISNULL(MAX(AgencyCode),0) + 1 \n"
					+ "      ,@AgencyName  \n"
					+ "      ,@RapCode  \n"
					+ "      ,@AgencyType  \n" 
					+ "      ,@Address    \n"
					+ "      ,@Tell       \n"					
					+ "      ,@Comment    \n"					
					+ "      ,@UseYn      \n"					
					+ "      ,GETDATE()   \n"
					+ "      ,GETDATE()   \n"	
					+ "      ,@RegID      \n"
					+ " FROM Agency		  \n"
					
					);
                

				sqlParams[i++] = new SqlParameter("@AgencyName"  , SqlDbType.VarChar , 20);
				sqlParams[i++] = new SqlParameter("@RapCode"  , SqlDbType.TinyInt);
				sqlParams[i++] = new SqlParameter("@AgencyType"  , SqlDbType.Char    ,  2);
				sqlParams[i++] = new SqlParameter("@Address"    , SqlDbType.VarChar , 50);
				sqlParams[i++] = new SqlParameter("@Tell"       , SqlDbType.VarChar , 15);
				sqlParams[i++] = new SqlParameter("@Comment"    , SqlDbType.VarChar , 50);
				sqlParams[i++] = new SqlParameter("@UseYn"      , SqlDbType.Char    ,  1);
				sqlParams[i++] = new SqlParameter("@RegID"      , SqlDbType.VarChar , 10);
                */
                #endregion

                sbQuery.Append("\n " + " INSERT INTO AGNC (     ");
                sbQuery.Append("\n " + "          agnc_cod      ");
                sbQuery.Append("\n " + "         ,agnc_nm       ");
                sbQuery.Append("\n " + "         ,rep_cod       ");
                sbQuery.Append("\n " + "         ,agnc_tp       ");
                sbQuery.Append("\n " + "         ,tel_no        ");
                sbQuery.Append("\n " + "         ,agnc_memo     ");
                sbQuery.Append("\n " + "         ,use_yn        ");
                sbQuery.Append("\n " + "         ,dt_insert     ");
                sbQuery.Append("\n " + "         ,dt_update     ");
                sbQuery.Append("\n " + "         ,id_insert     ");
                sbQuery.Append("\n " + "         ,id_update     ");
                sbQuery.Append("\n " + "         )              ");
                sbQuery.Append("\n " + " SELECT                 ");
                sbQuery.Append("\n " + "     NVL(MAX(agnc_cod),0) + 1   ");
                sbQuery.Append("\n " + "    ,:AgencyName        ");
                sbQuery.Append("\n " + "    ,:RapCode           ");
                sbQuery.Append("\n " + "    ,:AgencyType        ");
                sbQuery.Append("\n " + "    ,:Tell              ");
                sbQuery.Append("\n " + "    ,:Comments           ");
                sbQuery.Append("\n " + "    ,:UseYn             ");
                sbQuery.Append("\n " + "    ,SYSDATE            ");
                sbQuery.Append("\n " + "    ,SYSDATE            ");
                sbQuery.Append("\n " + "    ,:RegID             ");
                sbQuery.Append("\n " + "    ,:RegID             ");
                sbQuery.Append("\n " + " FROM AGNC              ");

                sqlParams[i++] = new OracleParameter(":AgencyName", OracleDbType.Varchar2, 40);
                sqlParams[i++] = new OracleParameter(":RapCode", OracleDbType.Int32);
                sqlParams[i++] = new OracleParameter(":AgencyType", OracleDbType.Char, 2);
                sqlParams[i++] = new OracleParameter(":Tell", OracleDbType.Varchar2, 15);
                sqlParams[i++] = new OracleParameter(":Comments", OracleDbType.Varchar2, 50);
                sqlParams[i++] = new OracleParameter(":UseYn", OracleDbType.Char, 1);
                sqlParams[i++] = new OracleParameter(":RegID", OracleDbType.Varchar2, 10);

				i = 0;
				sqlParams[i++].Value = agencyModel.AgencyName;
				if(agencyModel.RapCode.Trim().Length > 0)
				{
					sqlParams[i++].Value = Convert.ToInt16(agencyModel.RapCode);		
				}
				else
				{
					sqlParams[i++].Value = 0;		
				}
				sqlParams[i++].Value = agencyModel.AgencyType;				
				sqlParams[i++].Value = agencyModel.Tell;
				sqlParams[i++].Value = agencyModel.Comment;
				sqlParams[i++].Value = agencyModel.UseYn;
				sqlParams[i++].Value = header.UserID;      // 등록자

				// 쿼리실행
				try
				{
					_db.BeginTran();
					rc =  _db.ExecuteNonQueryParams(sbQuery.ToString(), sqlParams);
					_db.CommitTran();

					// __MESSAGE__
					_log.Message("대행사정보생성:[" + agencyModel.AgencyName + "(" + agencyModel.AgencyCode + ")] 등록자:[" + header.UserID + "]");

				}
				catch(Exception ex)
				{
					_db.RollbackTran();
					throw ex;
				}

				agencyModel.ResultCD = "0000";  // 정상

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "SetAgencyCreate() End");
				_log.Debug("-----------------------------------------");	
			}
			catch(Exception ex)
			{
				agencyModel.ResultCD   = "3101";
				agencyModel.ResultDesc = "대행사정보 생성 중 오류가 발생하였습니다";
				_log.Exception(ex);
			}
			finally
			{
				// 데이터베이스를  Close한다
				_db.Close();			
			}		
		}


		public void SetAgencyDelete(HeaderModel header, AgencyModel agencyModel)
		{
			int MediaAgencyCount = 0;
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "SetAgencyDelete() Start");
				_log.Debug("-----------------------------------------");

				// 데이터베이스를 OPEN한다
				_db.Open();

				//입력데이터의 Validation 검사
				if(agencyModel.AgencyCode.Length < 1) 
				{
					throw new FrameException("대행사코드가 입력되지 않습니다.");
				}

				StringBuilder sbQuery = new StringBuilder();
				StringBuilder sbQueryMediaAgencyCount = new StringBuilder();
				

				int i = 0;
				int rc = 0;
                OracleParameter[] sqlParams = new OracleParameter[1];

                sbQueryMediaAgencyCount.Append("\n " + "  SELECT COUNT(*) FROM    MDA_AGNC	");
				sbQueryMediaAgencyCount.Append("\n " + "  WHERE agnc_cod  = :AgencyCode     ");				

				sbQuery.Append("\n " + " DELETE AGNC         ");
				sbQuery.Append("\n " + " WHERE agnc_cod  = :AgencyCode ");

                sqlParams[i++] = new OracleParameter(":AgencyCode", OracleDbType.Int32);

				i = 0;
				sqlParams[i++].Value = Convert.ToInt16(agencyModel.AgencyCode);

				// 쿼리실행
				try
				{
					//매체대행광고주 관계 Count조사///////////////////////////////////////////////
					// __DEBUG__
					_log.Debug(sbQueryMediaAgencyCount.ToString());
					// __DEBUG__

					// 쿼리실행
					DataSet ds = new DataSet();
					_db.ExecuteQueryParams(ds,sbQueryMediaAgencyCount.ToString(),sqlParams);
                    
					MediaAgencyCount =  Convert.ToInt32(ds.Tables[0].Rows[0][0].ToString());

					ds.Dispose();

					_log.Debug("MediaAgencyCount          -->" + MediaAgencyCount);

					// 이미 다른테이블에 사용중인 데이터가 있다면 Exception를 발생시킨다.
					if(MediaAgencyCount > 0) throw new Exception();

                    i = 0;
                    sqlParams[i++] = new OracleParameter(":AgencyCode", OracleDbType.Int32);

                    i = 0;
                    sqlParams[i++].Value = Convert.ToInt16(agencyModel.AgencyCode);

					_db.BeginTran();
					rc =  _db.ExecuteNonQueryParams(sbQuery.ToString(), sqlParams);
					_db.CommitTran();

					// __MESSAGE__
					_log.Message("대행사정보삭제:[" + agencyModel.AgencyCode + "] 등록자:[" + header.UserID + "]");

				}
				catch(Exception ex)
				{
					_db.RollbackTran();
					throw ex;
				}

				agencyModel.ResultCD = "0000";  // 정상
	
				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "SetUserDelete() End");
				_log.Debug("-----------------------------------------");	
			}
			catch(Exception ex)
			{
				agencyModel.ResultCD   = "3301";
				// 이미 다른테이블에 사용중인 데이터가 있다면
				if(MediaAgencyCount > 0 )
				{
					agencyModel.ResultDesc = "등록된 매체대행사가 있으므로 대행사정보를 삭제할수 없습니다.";
				}
				else
				{
					agencyModel.ResultDesc = "대행사정보 삭제중 오류가 발생하였습니다";
					_log.Exception(ex);
				}
			}
			finally
			{
				// 데이터베이스를  Close한다
				_db.Close();			
			}		
		}
	}
}
