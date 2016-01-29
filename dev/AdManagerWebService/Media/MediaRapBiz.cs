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
	/// MediaRapBiz에 대한 요약 설명입니다.
	/// </summary>
	public class MediaRapBiz : BaseBiz
	{
		public MediaRapBiz() : base(FrameSystem.connDbString, true)
		{
			_log = FrameSystem.oLog;
		}


		/// <summary>
		/// 미디어렙목록조회
		/// </summary>
		/// <param name="mediarapModel"></param>
		public void GetMediaRapList(HeaderModel header, MediaRapModel mediarapModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetMediaRapList() Start");
				_log.Debug("-----------------------------------------");

				// 데이터베이스를 OPEN한다
				_db.Open();

				// __DEBUG__
				_log.Debug("<입력정보>");
				_log.Debug("SearchKey      :[" + mediarapModel.SearchKey       + "]");
				_log.Debug("SearchMediaRapLevel:[" + mediarapModel.SearchMediaRap + "]");
				// __DEBUG__

				StringBuilder sbQuery = new StringBuilder();

				// 쿼리생성
				sbQuery.Append("\n"
                    + " SELECT A.REP_COD AS RapCode, A.REP_NM AS RapName, A.REP_TP AS RapType, A.PHONE_NO AS Tell, A.REP_MEMO AS \"Comment\"  \n"
                    + "       ,A.USE_YN AS UseYn              \n"
                    + "       ,DECODE(A.USE_YN,'Y','','N','사용안함') AS UseYn_N  \n"
                    + "       ,TO_CHAR(A.DT_INSERT, 'YYYY-MM-DD') AS RegDt              \n"
                    + "       ,TO_CHAR(A.DT_UPDATE, 'YYYY-MM-DD') AS ModDt              \n"
                    + "       ,B.USER_NM AS RegName                                 \n"
                    + "  FROM MDA_REP A LEFT JOIN STM_USER B ON A.ID_INSERT = B.USER_ID \n                 \n"					
					+ " WHERE 1 = 1  \n"
					);


				// 어드민과 슈퍼유저면 사용여부가 'Y', 'N' 데이터를 다 조회한다.
//				if (header.UserClass.Equals("10") || header.UserClass.Equals("20"))
//				{
//					sbQuery.Append(" AND A.UseYn = 'Y' OR A.UseYn = 'N' \n");
//				}
//				else
//				{
//					sbQuery.Append(" AND A.UseYn = 'Y' \n");
//				}
				
				// 검색어가 있으면
				if (mediarapModel.SearchKey.Trim().Length > 0)
				{
					// 여러컬럼에 대하여 LIKE 검색을 한다.
					sbQuery.Append(" AND ("
                        + "    REP_COD      LIKE '%" + mediarapModel.SearchKey.Trim() + "%' \n"
                        + " OR PHONE_NO    LIKE '%" + mediarapModel.SearchKey.Trim() + "%' \n"
                        + " OR REP_MEMO    LIKE '%" + mediarapModel.SearchKey.Trim() + "%' \n"						
						+ " ) ");
				}

				// 미디어렙레벨을 선택했으면
				if(mediarapModel.SearchMediaRap.Trim().Length > 0 && !mediarapModel.SearchMediaRap.Trim().Equals("00"))
				{
                    sbQuery.Append(" AND REP_TP = '" + mediarapModel.SearchMediaRap.Trim() + "' \n");
				}			

				if(mediarapModel.SearchchkAdState_10.Trim().Length > 0 && mediarapModel.SearchchkAdState_10.Trim().Equals("Y"))
				{
                    sbQuery.Append(" AND A.USE_YN = 'Y' OR A.USE_YN = 'N' \n");
				}	
				if(mediarapModel.SearchchkAdState_10.Trim().Length > 0 && mediarapModel.SearchchkAdState_10.Trim().Equals("N"))
				{
                    sbQuery.Append(" AND  A.USE_YN  = 'Y' \n");					
				}

                sbQuery.Append(" ORDER BY A.REP_COD desc \n");

				// __DEBUG__
				_log.Debug(sbQuery.ToString());
				// __DEBUG__
				
				// 쿼리실행
				DataSet ds = new DataSet();
				_db.ExecuteQuery(ds,sbQuery.ToString());

				// 결과 DataSet의 미디어렙모델에 복사
				mediarapModel.MediaRapDataSet = ds.Copy();
				// 결과
				mediarapModel.ResultCnt = Utility.GetDatasetCount(mediarapModel.MediaRapDataSet);
				// 결과코드 셋트
				mediarapModel.ResultCD = "0000";

				// __DEBUG__
				_log.Debug("<출력정보>");
				_log.Debug("ResultCnt:[" + mediarapModel.ResultCnt + "]");
				// __DEBUG__

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "GetMediaRapList() End");
				_log.Debug("-----------------------------------------");
			}
			catch(Exception ex)
			{
				mediarapModel.ResultCD = "3000";
				mediarapModel.ResultDesc = "미디어렙정보 조회중 오류가 발생하였습니다";
				_log.Exception(ex);
			}
			finally
			{
				// 데이터베이스를  Close한다
				_db.Close();			
			}		
		}


		/// <summary>
		/// 미디어렙정보 저장
		/// </summary>
		/// <param name="MediaRapID"></param>
		/// <param name="MediaRapName"></param>
		/// <param name="MediaRapPassword"></param>
		/// <param name="MediaRapLevel"></param>
		/// <param name="MediaRapDept"></param>
		/// <param name="MediaRapTitle"></param>
		/// <param name="MediaRapTell"></param>
		/// <param name="MediaRapMobile"></param>
		/// <param name="MediaRapComment"></param>
		/// <returns></returns>
		public void SetMediaRapUpdate(HeaderModel header, MediaRapModel mediarapModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "SetMediaRapUpdate() Start");
				_log.Debug("-----------------------------------------");

				// 데이터베이스를 OPEN한다
				_db.Open();

				StringBuilder sbQuery = new StringBuilder();

				int i = 0;
				int rc = 0;
                OracleParameter[] sqlParams = new OracleParameter[7];

				sbQuery.Append(""
                    + "UPDATE MDA_REP                     \n"
                    + "   SET REP_NM     = :RapName      \n"
                    + "      ,REP_TP     = :RapType      \n"
                    + "      ,PHONE_NO   = :Tell      \n"
                    + "      ,REP_MEMO   = :Comment1   \n"
                    + "      ,USE_YN	 = :UseYn      \n"
                    + "      ,DT_UPDATE  = SYSDATE      \n"
                    + "      ,ID_UPDATE  = :RegID         \n"
                    + " WHERE REP_COD    = :RapCode        \n"
					);

				i = 0;
                sqlParams[i++] = new OracleParameter(":RapName", OracleDbType.Varchar2, 40);
                sqlParams[i++] = new OracleParameter(":RapType", OracleDbType.Char, 2);
                sqlParams[i++] = new OracleParameter(":Tell", OracleDbType.Varchar2, 15);
                sqlParams[i++] = new OracleParameter(":Comment1", OracleDbType.Varchar2, 50);
                sqlParams[i++] = new OracleParameter(":UseYn", OracleDbType.Char, 1);
                sqlParams[i++] = new OracleParameter(":RegID", OracleDbType.Varchar2, 10);
                sqlParams[i++] = new OracleParameter(":RapCode", OracleDbType.Int32);

				i = 0;
				sqlParams[i++].Value = mediarapModel.RapName;				
				sqlParams[i++].Value = mediarapModel.RapType;				
				sqlParams[i++].Value = mediarapModel.Tell;				
				sqlParams[i++].Value = mediarapModel.Comment;				
				sqlParams[i++].Value = mediarapModel.UseYn;
				sqlParams[i++].Value = header.UserID;      // 등록자
				sqlParams[i++].Value = Convert.ToInt32(mediarapModel.RapCode);

				// 쿼리실행
				try
				{
					_db.BeginTran();
					rc =  _db.ExecuteNonQueryParams(sbQuery.ToString(), sqlParams);
					_db.CommitTran();

					// __MESSAGE__
					_log.Message("미디어렙정보수정:["+mediarapModel.RapCode + "] 등록자:[" + header.UserID + "]");

				}
				catch(Exception ex)
				{
					_db.RollbackTran();
					throw ex;
				}

				mediarapModel.ResultCD = "0000";  // 정상

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "SetMediaRapUpdate() End");
				_log.Debug("-----------------------------------------");

			}
			catch(Exception ex)
			{
				mediarapModel.ResultCD   = "3201";
				mediarapModel.ResultDesc = "미디어렙정보 수정중 오류가 발생하였습니다";
				_log.Exception(ex);
			}
			// 데이터베이스를  Close한다
			_db.Close();

		}

		/// <summary>
		/// 미디어렙 생성
		/// </summary>
		/// <param name="MediaRapID"></param>
		/// <param name="MediaRapName"></param>
		/// <param name="MediaRapPassword"></param>
		/// <param name="MediaRapLevel"></param>
		/// <param name="MediaRapDept"></param>
		/// <param name="MediaRapTitle"></param>
		/// <param name="MediaRapTell"></param>
		/// <param name="MediaRapMobile"></param>
		/// <param name="MediaRapComment"></param>
		/// <returns></returns>
		public void SetMediaRapCreate(HeaderModel header, MediaRapModel mediarapModel)
		{
			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "SetMediaRapCreate() Start");
				_log.Debug("-----------------------------------------");

				// 데이터베이스를 OPEN한다
				_db.Open();

				StringBuilder sbQuery = new StringBuilder();
			
				int i = 0;
				int rc = 0;
                OracleParameter[] sqlParams = new OracleParameter[5];

				sbQuery.Append( ""
                    + "INSERT INTO MDA_REP (                         \n"
                    + "       REP_COD         \n"
                    + "      ,REP_NM        \n"
                    + "      ,REP_TP         \n"
                    + "      ,PHONE_NO         \n"
                    + "      ,REP_MEMO         \n"
                    + "		 ,USE_YN                \n"
                    + "      ,DT_INSERT         \n"
                    + "      ,DT_UPDATE         \n"
                    + "      ,ID_INSERT                                     \n"
					+ "      )                                          \n"
					+ " SELECT                                        \n"
                    + "       NVL(MAX(REP_COD),0)+1        \n"
					+ "      ,:RapName      \n"
					+ "      ,:RapType  \n" 
					+ "      ,:Tell     \n"
					+ "      ,:Comment1      \n"					
					+ "      ,'Y'      \n"
                    + "      ,SYSDATE      \n"
                    + "      ,SYSDATE      \n"
					+ "      ,:RegID         \n"
                    + "      FROM MDA_REP               \n"
					);

                sqlParams[i++] = new OracleParameter(":RapName", OracleDbType.Varchar2, 40);
                sqlParams[i++] = new OracleParameter(":RapType", OracleDbType.Char, 2);
                sqlParams[i++] = new OracleParameter(":Tell", OracleDbType.Varchar2, 15);
                sqlParams[i++] = new OracleParameter(":Comment1", OracleDbType.Varchar2, 50);
                sqlParams[i++] = new OracleParameter(":RegID", OracleDbType.Varchar2, 10);

				i = 0;				
				sqlParams[i++].Value = mediarapModel.RapName;
				sqlParams[i++].Value = mediarapModel.RapType;				
				sqlParams[i++].Value = mediarapModel.Tell;
				sqlParams[i++].Value = mediarapModel.Comment;				
				sqlParams[i++].Value = header.UserID;				// 등록자

				// 쿼리실행
				try
				{
					_db.BeginTran();
					rc =  _db.ExecuteNonQueryParams(sbQuery.ToString(), sqlParams);
					_db.CommitTran();

					// __MESSAGE__
					_log.Message("미디어렙정보생성:[" + mediarapModel.RapCode + "(" + mediarapModel.RapName + ")] 등록자:[" + header.UserID + "]");

				}
				catch(Exception ex)
				{
					_db.RollbackTran();
					throw ex;
				}

				mediarapModel.ResultCD = "0000";  // 정상

				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "SetMediaRapCreate() End");
				_log.Debug("-----------------------------------------");	
			}
			catch(Exception ex)
			{
				mediarapModel.ResultCD   = "3101";
				mediarapModel.ResultDesc = "미디어렙정보 생성 중 오류가 발생하였습니다";
				_log.Exception(ex);
			}
			finally
			{
				// 데이터베이스를  Close한다
				_db.Close();			
			}
		}


		public void SetMediaRapDelete(HeaderModel header, MediaRapModel mediarapModel)
		{
            int MediaAgencyCount = 0;

			try
			{
				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "SetMediaRapDelete() Start");
				_log.Debug("-----------------------------------------");

				// 데이터베이스를 OPEN한다
				_db.Open();

				StringBuilder sbQuery = new StringBuilder();
                StringBuilder sbQueryMediaAgencyCount = new StringBuilder();
				int i = 0;
				int rc = 0;
                OracleParameter[] sqlParams = new OracleParameter[1];
                OracleParameter[] sqlParams2 = new OracleParameter[1];
                sbQueryMediaAgencyCount.Append( "\n"
                    + "        SELECT COUNT(*) FROM    MDA_AGNC			    \n"
                    + "              WHERE REP_COD  = :RapCode          	\n"
                    );  

				sbQuery.Append(""
                    + "DELETE MDA_REP         \n"
                    + " WHERE REP_COD  = :RapCode  \n"
					);

                sqlParams[i++] = new OracleParameter(":RapCode", OracleDbType.Int32);
                sqlParams2[0] = new OracleParameter(":RapCode", OracleDbType.Int32);
				i = 0;
				sqlParams[i++].Value = Convert.ToInt32(mediarapModel.RapCode);
                sqlParams2[0].Value = Convert.ToInt32(mediarapModel.RapCode);
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
                    
                    MediaAgencyCount = Convert.ToInt32(ds.Tables[0].Rows[0][0].ToString());

                    _log.Debug("MediaAgencyCount          -->" + MediaAgencyCount);

                    // 이미 다른테이블에 사용중인 데이터가 있다면 Exception를 발생시킨다.
                    if(MediaAgencyCount > 0) throw new Exception();


					_db.BeginTran();
                    rc = _db.ExecuteNonQueryParams(sbQuery.ToString(), sqlParams2);
					_db.CommitTran();

					// __MESSAGE__
					_log.Message("미디어렙정보삭제:[" + mediarapModel.RapCode + "] 등록자:[" + header.UserID + "]");

				}
				catch(Exception ex)
				{
					_db.RollbackTran();
					throw ex;
				}

				mediarapModel.ResultCD = "0000";  // 정상
	
				_log.Debug("-----------------------------------------");
				_log.Debug(this.ToString() + "SetMediaRapDelete() End");
				_log.Debug("-----------------------------------------");	
			}
			catch(Exception ex)
			{
				mediarapModel.ResultCD   = "3301";
                // 이미 다른테이블에 사용중인 데이터가 있다면
                if(MediaAgencyCount > 0 )
                {
                    mediarapModel.ResultDesc = "등록된 매체대행사가 있으므로 미디어렙정보를 삭제할수 없습니다.";
                }
                else
                {
                    mediarapModel.ResultDesc = "미디어렙정보 삭제중 오류가 발생하였습니다";
                }
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
